using System;
using System.IO;
using SevenZip.Compression.LZ;
using SevenZip.Compression.RangeCoder;

namespace SevenZip.Compression.LZMA
{
	// Token: 0x0200002F RID: 47
	public class Encoder : ICoder, ISetCoderProperties, IWriteCoderProperties
	{
		// Token: 0x0600011F RID: 287 RVA: 0x00007004 File Offset: 0x00005204
		static Encoder()
		{
			int num = 2;
			Encoder.g_FastPos[0] = 0;
			Encoder.g_FastPos[1] = 1;
			for (byte b = 2; b < 22; b += 1)
			{
				uint num2 = 1U << (b >> 1) - 1;
				uint num3 = 0U;
				while (num3 < num2)
				{
					Encoder.g_FastPos[num] = b;
					num3 += 1U;
					num++;
				}
			}
		}

		// Token: 0x06000120 RID: 288 RVA: 0x0000707E File Offset: 0x0000527E
		private static uint GetPosSlot(uint pos)
		{
			if (pos < 2048U)
			{
				return (uint)Encoder.g_FastPos[(int)pos];
			}
			if (pos < 2097152U)
			{
				return (uint)(Encoder.g_FastPos[(int)(pos >> 10)] + 20);
			}
			return (uint)(Encoder.g_FastPos[(int)(pos >> 20)] + 40);
		}

		// Token: 0x06000121 RID: 289 RVA: 0x000070B3 File Offset: 0x000052B3
		private static uint GetPosSlot2(uint pos)
		{
			if (pos < 131072U)
			{
				return (uint)(Encoder.g_FastPos[(int)(pos >> 6)] + 12);
			}
			if (pos < 134217728U)
			{
				return (uint)(Encoder.g_FastPos[(int)(pos >> 16)] + 32);
			}
			return (uint)(Encoder.g_FastPos[(int)(pos >> 26)] + 52);
		}

		// Token: 0x06000122 RID: 290 RVA: 0x000070F0 File Offset: 0x000052F0
		private void BaseInit()
		{
			this._state.Init();
			this._previousByte = 0;
			for (uint num = 0U; num < 4U; num += 1U)
			{
				this._repDistances[(int)num] = 0U;
			}
		}

		// Token: 0x06000123 RID: 291 RVA: 0x00007124 File Offset: 0x00005324
		private void Create()
		{
			if (this._matchFinder == null)
			{
				BinTree binTree = new BinTree();
				int num = 4;
				if (this._matchFinderType == Encoder.EMatchFinderType.BT2)
				{
					num = 2;
				}
				binTree.SetType(num);
				this._matchFinder = binTree;
			}
			this._literalEncoder.Create(this._numLiteralPosStateBits, this._numLiteralContextBits);
			if (this._dictionarySize == this._dictionarySizePrev && this._numFastBytesPrev == this._numFastBytes)
			{
				return;
			}
			this._matchFinder.Create(this._dictionarySize, 4096U, this._numFastBytes, 274U);
			this._dictionarySizePrev = this._dictionarySize;
			this._numFastBytesPrev = this._numFastBytes;
		}

		// Token: 0x06000124 RID: 292 RVA: 0x000071C8 File Offset: 0x000053C8
		public Encoder()
		{
			int num = 0;
			while ((long)num < 4096L)
			{
				this._optimum[num] = new Encoder.Optimal();
				num++;
			}
			int num2 = 0;
			while ((long)num2 < 4L)
			{
				this._posSlotEncoder[num2] = new BitTreeEncoder(6);
				num2++;
			}
		}

		// Token: 0x06000125 RID: 293 RVA: 0x00007391 File Offset: 0x00005591
		private void SetWriteEndMarkerMode(bool writeEndMarker)
		{
			this._writeEndMark = writeEndMarker;
		}

		// Token: 0x06000126 RID: 294 RVA: 0x0000739C File Offset: 0x0000559C
		private void Init()
		{
			this.BaseInit();
			this._rangeEncoder.Init();
			for (uint num = 0U; num < 12U; num += 1U)
			{
				for (uint num2 = 0U; num2 <= this._posStateMask; num2 += 1U)
				{
					uint num3 = (num << 4) + num2;
					this._isMatch[(int)num3].Init();
					this._isRep0Long[(int)num3].Init();
				}
				this._isRep[(int)num].Init();
				this._isRepG0[(int)num].Init();
				this._isRepG1[(int)num].Init();
				this._isRepG2[(int)num].Init();
			}
			this._literalEncoder.Init();
			for (uint num = 0U; num < 4U; num += 1U)
			{
				this._posSlotEncoder[(int)num].Init();
			}
			for (uint num = 0U; num < 114U; num += 1U)
			{
				this._posEncoders[(int)num].Init();
			}
			this._lenEncoder.Init(1U << this._posStateBits);
			this._repMatchLenEncoder.Init(1U << this._posStateBits);
			this._posAlignEncoder.Init();
			this._longestMatchWasFound = false;
			this._optimumEndIndex = 0U;
			this._optimumCurrentIndex = 0U;
			this._additionalOffset = 0U;
		}

		// Token: 0x06000127 RID: 295 RVA: 0x000074E4 File Offset: 0x000056E4
		private void ReadMatchDistances(out uint lenRes, out uint numDistancePairs)
		{
			lenRes = 0U;
			numDistancePairs = this._matchFinder.GetMatches(this._matchDistances);
			if (numDistancePairs > 0U)
			{
				lenRes = this._matchDistances[(int)(numDistancePairs - 2U)];
				if (lenRes == this._numFastBytes)
				{
					lenRes += this._matchFinder.GetMatchLen((int)(lenRes - 1U), this._matchDistances[(int)(numDistancePairs - 1U)], 273U - lenRes);
				}
			}
			this._additionalOffset += 1U;
		}

		// Token: 0x06000128 RID: 296 RVA: 0x00007558 File Offset: 0x00005758
		private void MovePos(uint num)
		{
			if (num > 0U)
			{
				this._matchFinder.Skip(num);
				this._additionalOffset += num;
			}
		}

		// Token: 0x06000129 RID: 297 RVA: 0x00007578 File Offset: 0x00005778
		private uint GetRepLen1Price(Base.State state, uint posState)
		{
			return this._isRepG0[(int)state.Index].GetPrice0() + this._isRep0Long[(int)((state.Index << 4) + posState)].GetPrice0();
		}

		// Token: 0x0600012A RID: 298 RVA: 0x000075AC File Offset: 0x000057AC
		private uint GetPureRepPrice(uint repIndex, Base.State state, uint posState)
		{
			uint num;
			if (repIndex == 0U)
			{
				num = this._isRepG0[(int)state.Index].GetPrice0();
				num += this._isRep0Long[(int)((state.Index << 4) + posState)].GetPrice1();
			}
			else
			{
				num = this._isRepG0[(int)state.Index].GetPrice1();
				if (repIndex == 1U)
				{
					num += this._isRepG1[(int)state.Index].GetPrice0();
				}
				else
				{
					num += this._isRepG1[(int)state.Index].GetPrice1();
					num += this._isRepG2[(int)state.Index].GetPrice(repIndex - 2U);
				}
			}
			return num;
		}

		// Token: 0x0600012B RID: 299 RVA: 0x0000765E File Offset: 0x0000585E
		private uint GetRepPrice(uint repIndex, uint len, Base.State state, uint posState)
		{
			return this._repMatchLenEncoder.GetPrice(len - 2U, posState) + this.GetPureRepPrice(repIndex, state, posState);
		}

		// Token: 0x0600012C RID: 300 RVA: 0x0000767C File Offset: 0x0000587C
		private uint GetPosLenPrice(uint pos, uint len, uint posState)
		{
			uint lenToPosState = Base.GetLenToPosState(len);
			uint num;
			if (pos < 128U)
			{
				num = this._distancesPrices[(int)(lenToPosState * 128U + pos)];
			}
			else
			{
				num = this._posSlotPrices[(int)((lenToPosState << 6) + Encoder.GetPosSlot2(pos))] + this._alignPrices[(int)(pos & 15U)];
			}
			return num + this._lenEncoder.GetPrice(len - 2U, posState);
		}

		// Token: 0x0600012D RID: 301 RVA: 0x000076DC File Offset: 0x000058DC
		private uint Backward(out uint backRes, uint cur)
		{
			this._optimumEndIndex = cur;
			uint num = this._optimum[(int)cur].PosPrev;
			uint num2 = this._optimum[(int)cur].BackPrev;
			do
			{
				if (this._optimum[(int)cur].Prev1IsChar)
				{
					this._optimum[(int)num].MakeAsChar();
					this._optimum[(int)num].PosPrev = num - 1U;
					if (this._optimum[(int)cur].Prev2)
					{
						this._optimum[(int)(num - 1U)].Prev1IsChar = false;
						this._optimum[(int)(num - 1U)].PosPrev = this._optimum[(int)cur].PosPrev2;
						this._optimum[(int)(num - 1U)].BackPrev = this._optimum[(int)cur].BackPrev2;
					}
				}
				uint num3 = num;
				uint num4 = num2;
				num2 = this._optimum[(int)num3].BackPrev;
				num = this._optimum[(int)num3].PosPrev;
				this._optimum[(int)num3].BackPrev = num4;
				this._optimum[(int)num3].PosPrev = cur;
				cur = num3;
			}
			while (cur > 0U);
			backRes = this._optimum[0].BackPrev;
			this._optimumCurrentIndex = this._optimum[0].PosPrev;
			return this._optimumCurrentIndex;
		}

		// Token: 0x0600012E RID: 302 RVA: 0x00007800 File Offset: 0x00005A00
		private uint GetOptimum(uint position, out uint backRes)
		{
			if (this._optimumEndIndex != this._optimumCurrentIndex)
			{
				uint num = this._optimum[(int)this._optimumCurrentIndex].PosPrev - this._optimumCurrentIndex;
				backRes = this._optimum[(int)this._optimumCurrentIndex].BackPrev;
				this._optimumCurrentIndex = this._optimum[(int)this._optimumCurrentIndex].PosPrev;
				return num;
			}
			this._optimumCurrentIndex = (this._optimumEndIndex = 0U);
			uint longestMatchLength;
			uint num2;
			if (!this._longestMatchWasFound)
			{
				this.ReadMatchDistances(out longestMatchLength, out num2);
			}
			else
			{
				longestMatchLength = this._longestMatchLength;
				num2 = this._numDistancePairs;
				this._longestMatchWasFound = false;
			}
			uint num3 = this._matchFinder.GetNumAvailableBytes() + 1U;
			if (num3 < 2U)
			{
				backRes = uint.MaxValue;
				return 1U;
			}
			if (num3 > 273U)
			{
			}
			uint num4 = 0U;
			for (uint num5 = 0U; num5 < 4U; num5 += 1U)
			{
				this.reps[(int)num5] = this._repDistances[(int)num5];
				this.repLens[(int)num5] = this._matchFinder.GetMatchLen(-1, this.reps[(int)num5], 273U);
				if (this.repLens[(int)num5] > this.repLens[(int)num4])
				{
					num4 = num5;
				}
			}
			if (this.repLens[(int)num4] >= this._numFastBytes)
			{
				backRes = num4;
				uint num6 = this.repLens[(int)num4];
				this.MovePos(num6 - 1U);
				return num6;
			}
			if (longestMatchLength >= this._numFastBytes)
			{
				backRes = this._matchDistances[(int)(num2 - 1U)] + 4U;
				this.MovePos(longestMatchLength - 1U);
				return longestMatchLength;
			}
			byte b = this._matchFinder.GetIndexByte(-1);
			byte b2 = this._matchFinder.GetIndexByte((int)(0U - this._repDistances[0] - 1U - 1U));
			if (longestMatchLength < 2U && b != b2 && this.repLens[(int)num4] < 2U)
			{
				backRes = uint.MaxValue;
				return 1U;
			}
			this._optimum[0].State = this._state;
			uint num7 = position & this._posStateMask;
			this._optimum[1].Price = this._isMatch[(int)((this._state.Index << 4) + num7)].GetPrice0() + this._literalEncoder.GetSubCoder(position, this._previousByte).GetPrice(!this._state.IsCharState(), b2, b);
			this._optimum[1].MakeAsChar();
			uint num8 = this._isMatch[(int)((this._state.Index << 4) + num7)].GetPrice1();
			uint num9 = num8 + this._isRep[(int)this._state.Index].GetPrice1();
			if (b2 == b)
			{
				uint num10 = num9 + this.GetRepLen1Price(this._state, num7);
				if (num10 < this._optimum[1].Price)
				{
					this._optimum[1].Price = num10;
					this._optimum[1].MakeAsShortRep();
				}
			}
			uint num11 = ((longestMatchLength >= this.repLens[(int)num4]) ? longestMatchLength : this.repLens[(int)num4]);
			if (num11 < 2U)
			{
				backRes = this._optimum[1].BackPrev;
				return 1U;
			}
			this._optimum[1].PosPrev = 0U;
			this._optimum[0].Backs0 = this.reps[0];
			this._optimum[0].Backs1 = this.reps[1];
			this._optimum[0].Backs2 = this.reps[2];
			this._optimum[0].Backs3 = this.reps[3];
			uint num12 = num11;
			do
			{
				this._optimum[(int)num12--].Price = 268435455U;
			}
			while (num12 >= 2U);
			for (uint num5 = 0U; num5 < 4U; num5 += 1U)
			{
				uint num13 = this.repLens[(int)num5];
				if (num13 >= 2U)
				{
					uint num14 = num9 + this.GetPureRepPrice(num5, this._state, num7);
					do
					{
						uint num15 = num14 + this._repMatchLenEncoder.GetPrice(num13 - 2U, num7);
						Encoder.Optimal optimal = this._optimum[(int)num13];
						if (num15 < optimal.Price)
						{
							optimal.Price = num15;
							optimal.PosPrev = 0U;
							optimal.BackPrev = num5;
							optimal.Prev1IsChar = false;
						}
					}
					while ((num13 -= 1U) >= 2U);
				}
			}
			uint num16 = num8 + this._isRep[(int)this._state.Index].GetPrice0();
			num12 = ((this.repLens[0] >= 2U) ? (this.repLens[0] + 1U) : 2U);
			if (num12 <= longestMatchLength)
			{
				uint num17 = 0U;
				while (num12 > this._matchDistances[(int)num17])
				{
					num17 += 2U;
				}
				for (;;)
				{
					uint num18 = this._matchDistances[(int)(num17 + 1U)];
					uint num19 = num16 + this.GetPosLenPrice(num18, num12, num7);
					Encoder.Optimal optimal2 = this._optimum[(int)num12];
					if (num19 < optimal2.Price)
					{
						optimal2.Price = num19;
						optimal2.PosPrev = 0U;
						optimal2.BackPrev = num18 + 4U;
						optimal2.Prev1IsChar = false;
					}
					if (num12 == this._matchDistances[(int)num17])
					{
						num17 += 2U;
						if (num17 == num2)
						{
							break;
						}
					}
					num12 += 1U;
				}
			}
			uint num20 = 0U;
			uint num21;
			for (;;)
			{
				num20 += 1U;
				if (num20 == num11)
				{
					break;
				}
				this.ReadMatchDistances(out num21, out num2);
				if (num21 >= this._numFastBytes)
				{
					goto Block_24;
				}
				position += 1U;
				uint num22 = this._optimum[(int)num20].PosPrev;
				Base.State state;
				if (this._optimum[(int)num20].Prev1IsChar)
				{
					num22 -= 1U;
					if (this._optimum[(int)num20].Prev2)
					{
						state = this._optimum[(int)this._optimum[(int)num20].PosPrev2].State;
						if (this._optimum[(int)num20].BackPrev2 < 4U)
						{
							state.UpdateRep();
						}
						else
						{
							state.UpdateMatch();
						}
					}
					else
					{
						state = this._optimum[(int)num22].State;
					}
					state.UpdateChar();
				}
				else
				{
					state = this._optimum[(int)num22].State;
				}
				if (num22 == num20 - 1U)
				{
					if (this._optimum[(int)num20].IsShortRep())
					{
						state.UpdateShortRep();
					}
					else
					{
						state.UpdateChar();
					}
				}
				else
				{
					uint num23;
					if (this._optimum[(int)num20].Prev1IsChar && this._optimum[(int)num20].Prev2)
					{
						num22 = this._optimum[(int)num20].PosPrev2;
						num23 = this._optimum[(int)num20].BackPrev2;
						state.UpdateRep();
					}
					else
					{
						num23 = this._optimum[(int)num20].BackPrev;
						if (num23 < 4U)
						{
							state.UpdateRep();
						}
						else
						{
							state.UpdateMatch();
						}
					}
					Encoder.Optimal optimal3 = this._optimum[(int)num22];
					if (num23 < 4U)
					{
						if (num23 == 0U)
						{
							this.reps[0] = optimal3.Backs0;
							this.reps[1] = optimal3.Backs1;
							this.reps[2] = optimal3.Backs2;
							this.reps[3] = optimal3.Backs3;
						}
						else if (num23 == 1U)
						{
							this.reps[0] = optimal3.Backs1;
							this.reps[1] = optimal3.Backs0;
							this.reps[2] = optimal3.Backs2;
							this.reps[3] = optimal3.Backs3;
						}
						else if (num23 == 2U)
						{
							this.reps[0] = optimal3.Backs2;
							this.reps[1] = optimal3.Backs0;
							this.reps[2] = optimal3.Backs1;
							this.reps[3] = optimal3.Backs3;
						}
						else
						{
							this.reps[0] = optimal3.Backs3;
							this.reps[1] = optimal3.Backs0;
							this.reps[2] = optimal3.Backs1;
							this.reps[3] = optimal3.Backs2;
						}
					}
					else
					{
						this.reps[0] = num23 - 4U;
						this.reps[1] = optimal3.Backs0;
						this.reps[2] = optimal3.Backs1;
						this.reps[3] = optimal3.Backs2;
					}
				}
				this._optimum[(int)num20].State = state;
				this._optimum[(int)num20].Backs0 = this.reps[0];
				this._optimum[(int)num20].Backs1 = this.reps[1];
				this._optimum[(int)num20].Backs2 = this.reps[2];
				this._optimum[(int)num20].Backs3 = this.reps[3];
				uint price = this._optimum[(int)num20].Price;
				b = this._matchFinder.GetIndexByte(-1);
				b2 = this._matchFinder.GetIndexByte((int)(0U - this.reps[0] - 1U - 1U));
				num7 = position & this._posStateMask;
				uint num24 = price + this._isMatch[(int)((state.Index << 4) + num7)].GetPrice0() + this._literalEncoder.GetSubCoder(position, this._matchFinder.GetIndexByte(-2)).GetPrice(!state.IsCharState(), b2, b);
				Encoder.Optimal optimal4 = this._optimum[(int)(num20 + 1U)];
				bool flag = false;
				if (num24 < optimal4.Price)
				{
					optimal4.Price = num24;
					optimal4.PosPrev = num20;
					optimal4.MakeAsChar();
					flag = true;
				}
				num8 = price + this._isMatch[(int)((state.Index << 4) + num7)].GetPrice1();
				num9 = num8 + this._isRep[(int)state.Index].GetPrice1();
				if (b2 == b && (optimal4.PosPrev >= num20 || optimal4.BackPrev != 0U))
				{
					uint num25 = num9 + this.GetRepLen1Price(state, num7);
					if (num25 <= optimal4.Price)
					{
						optimal4.Price = num25;
						optimal4.PosPrev = num20;
						optimal4.MakeAsShortRep();
						flag = true;
					}
				}
				uint num26 = this._matchFinder.GetNumAvailableBytes() + 1U;
				num26 = Math.Min(4095U - num20, num26);
				num3 = num26;
				if (num3 >= 2U)
				{
					if (num3 > this._numFastBytes)
					{
						num3 = this._numFastBytes;
					}
					if (!flag && b2 != b)
					{
						uint num27 = Math.Min(num26 - 1U, this._numFastBytes);
						uint matchLen = this._matchFinder.GetMatchLen(0, this.reps[0], num27);
						if (matchLen >= 2U)
						{
							Base.State state2 = state;
							state2.UpdateChar();
							uint num28 = (position + 1U) & this._posStateMask;
							uint num29 = num24 + this._isMatch[(int)((state2.Index << 4) + num28)].GetPrice1() + this._isRep[(int)state2.Index].GetPrice1();
							uint num30 = num20 + 1U + matchLen;
							while (num11 < num30)
							{
								this._optimum[(int)(num11 += 1U)].Price = 268435455U;
							}
							uint num31 = num29 + this.GetRepPrice(0U, matchLen, state2, num28);
							Encoder.Optimal optimal5 = this._optimum[(int)num30];
							if (num31 < optimal5.Price)
							{
								optimal5.Price = num31;
								optimal5.PosPrev = num20 + 1U;
								optimal5.BackPrev = 0U;
								optimal5.Prev1IsChar = true;
								optimal5.Prev2 = false;
							}
						}
					}
					uint num32 = 2U;
					for (uint num33 = 0U; num33 < 4U; num33 += 1U)
					{
						uint num34 = this._matchFinder.GetMatchLen(-1, this.reps[(int)num33], num3);
						if (num34 >= 2U)
						{
							uint num35 = num34;
							for (;;)
							{
								if (num11 >= num20 + num34)
								{
									uint num36 = num9 + this.GetRepPrice(num33, num34, state, num7);
									Encoder.Optimal optimal6 = this._optimum[(int)(num20 + num34)];
									if (num36 < optimal6.Price)
									{
										optimal6.Price = num36;
										optimal6.PosPrev = num20;
										optimal6.BackPrev = num33;
										optimal6.Prev1IsChar = false;
									}
									if ((num34 -= 1U) < 2U)
									{
										break;
									}
								}
								else
								{
									this._optimum[(int)(num11 += 1U)].Price = 268435455U;
								}
							}
							num34 = num35;
							if (num33 == 0U)
							{
								num32 = num34 + 1U;
							}
							if (num34 < num26)
							{
								uint num37 = Math.Min(num26 - 1U - num34, this._numFastBytes);
								uint matchLen2 = this._matchFinder.GetMatchLen((int)num34, this.reps[(int)num33], num37);
								if (matchLen2 >= 2U)
								{
									Base.State state3 = state;
									state3.UpdateRep();
									uint num38 = (position + num34) & this._posStateMask;
									uint num39 = num9 + this.GetRepPrice(num33, num34, state, num7) + this._isMatch[(int)((state3.Index << 4) + num38)].GetPrice0() + this._literalEncoder.GetSubCoder(position + num34, this._matchFinder.GetIndexByte((int)(num34 - 1U - 1U))).GetPrice(true, this._matchFinder.GetIndexByte((int)(num34 - 1U - (this.reps[(int)num33] + 1U))), this._matchFinder.GetIndexByte((int)(num34 - 1U)));
									state3.UpdateChar();
									num38 = (position + num34 + 1U) & this._posStateMask;
									uint num40 = num39 + this._isMatch[(int)((state3.Index << 4) + num38)].GetPrice1() + this._isRep[(int)state3.Index].GetPrice1();
									uint num41 = num34 + 1U + matchLen2;
									while (num11 < num20 + num41)
									{
										this._optimum[(int)(num11 += 1U)].Price = 268435455U;
									}
									uint num42 = num40 + this.GetRepPrice(0U, matchLen2, state3, num38);
									Encoder.Optimal optimal7 = this._optimum[(int)(num20 + num41)];
									if (num42 < optimal7.Price)
									{
										optimal7.Price = num42;
										optimal7.PosPrev = num20 + num34 + 1U;
										optimal7.BackPrev = 0U;
										optimal7.Prev1IsChar = true;
										optimal7.Prev2 = true;
										optimal7.PosPrev2 = num20;
										optimal7.BackPrev2 = num33;
									}
								}
							}
						}
					}
					if (num21 > num3)
					{
						num21 = num3;
						num2 = 0U;
						while (num21 > this._matchDistances[(int)num2])
						{
							num2 += 2U;
						}
						this._matchDistances[(int)num2] = num21;
						num2 += 2U;
					}
					if (num21 >= num32)
					{
						num16 = num8 + this._isRep[(int)state.Index].GetPrice0();
						while (num11 < num20 + num21)
						{
							this._optimum[(int)(num11 += 1U)].Price = 268435455U;
						}
						uint num43 = 0U;
						while (num32 > this._matchDistances[(int)num43])
						{
							num43 += 2U;
						}
						uint num44 = num32;
						for (;;)
						{
							uint num45 = this._matchDistances[(int)(num43 + 1U)];
							uint num46 = num16 + this.GetPosLenPrice(num45, num44, num7);
							Encoder.Optimal optimal8 = this._optimum[(int)(num20 + num44)];
							if (num46 < optimal8.Price)
							{
								optimal8.Price = num46;
								optimal8.PosPrev = num20;
								optimal8.BackPrev = num45 + 4U;
								optimal8.Prev1IsChar = false;
							}
							if (num44 == this._matchDistances[(int)num43])
							{
								if (num44 < num26)
								{
									uint num47 = Math.Min(num26 - 1U - num44, this._numFastBytes);
									uint matchLen3 = this._matchFinder.GetMatchLen((int)num44, num45, num47);
									if (matchLen3 >= 2U)
									{
										Base.State state4 = state;
										state4.UpdateMatch();
										uint num48 = (position + num44) & this._posStateMask;
										uint num49 = num46 + this._isMatch[(int)((state4.Index << 4) + num48)].GetPrice0() + this._literalEncoder.GetSubCoder(position + num44, this._matchFinder.GetIndexByte((int)(num44 - 1U - 1U))).GetPrice(true, this._matchFinder.GetIndexByte((int)(num44 - (num45 + 1U) - 1U)), this._matchFinder.GetIndexByte((int)(num44 - 1U)));
										state4.UpdateChar();
										num48 = (position + num44 + 1U) & this._posStateMask;
										uint num50 = num49 + this._isMatch[(int)((state4.Index << 4) + num48)].GetPrice1() + this._isRep[(int)state4.Index].GetPrice1();
										uint num51 = num44 + 1U + matchLen3;
										while (num11 < num20 + num51)
										{
											this._optimum[(int)(num11 += 1U)].Price = 268435455U;
										}
										num46 = num50 + this.GetRepPrice(0U, matchLen3, state4, num48);
										optimal8 = this._optimum[(int)(num20 + num51)];
										if (num46 < optimal8.Price)
										{
											optimal8.Price = num46;
											optimal8.PosPrev = num20 + num44 + 1U;
											optimal8.BackPrev = 0U;
											optimal8.Prev1IsChar = true;
											optimal8.Prev2 = true;
											optimal8.PosPrev2 = num20;
											optimal8.BackPrev2 = num45 + 4U;
										}
									}
								}
								num43 += 2U;
								if (num43 == num2)
								{
									break;
								}
							}
							num44 += 1U;
						}
					}
				}
			}
			return this.Backward(out backRes, num20);
			Block_24:
			this._numDistancePairs = num2;
			this._longestMatchLength = num21;
			this._longestMatchWasFound = true;
			return this.Backward(out backRes, num20);
		}

		// Token: 0x0600012F RID: 303 RVA: 0x000087F6 File Offset: 0x000069F6
		private bool ChangePair(uint smallDist, uint bigDist)
		{
			return smallDist < 33554432U && bigDist >= smallDist << 7;
		}

		// Token: 0x06000130 RID: 304 RVA: 0x0000880C File Offset: 0x00006A0C
		private void WriteEndMarker(uint posState)
		{
			if (!this._writeEndMark)
			{
				return;
			}
			this._isMatch[(int)((this._state.Index << 4) + posState)].Encode(this._rangeEncoder, 1U);
			this._isRep[(int)this._state.Index].Encode(this._rangeEncoder, 0U);
			this._state.UpdateMatch();
			uint num = 2U;
			this._lenEncoder.Encode(this._rangeEncoder, num - 2U, posState);
			uint num2 = 63U;
			uint lenToPosState = Base.GetLenToPosState(num);
			this._posSlotEncoder[(int)lenToPosState].Encode(this._rangeEncoder, num2);
			int num3 = 30;
			uint num4 = (1U << num3) - 1U;
			this._rangeEncoder.EncodeDirectBits(num4 >> 4, num3 - 4);
			this._posAlignEncoder.ReverseEncode(this._rangeEncoder, num4 & 15U);
		}

		// Token: 0x06000131 RID: 305 RVA: 0x000088E3 File Offset: 0x00006AE3
		private void Flush(uint nowPos)
		{
			this.ReleaseMFStream();
			this.WriteEndMarker(nowPos & this._posStateMask);
			this._rangeEncoder.FlushData();
			this._rangeEncoder.FlushStream();
		}

		// Token: 0x06000132 RID: 306 RVA: 0x00008910 File Offset: 0x00006B10
		public void CodeOneBlock(out long inSize, out long outSize, out bool finished)
		{
			inSize = 0L;
			outSize = 0L;
			finished = true;
			if (this._inStream != null)
			{
				this._matchFinder.SetStream(this._inStream);
				this._matchFinder.Init();
				this._needReleaseMFStream = true;
				this._inStream = null;
				if (this._trainSize > 0U)
				{
					this._matchFinder.Skip(this._trainSize);
				}
			}
			if (this._finished)
			{
				return;
			}
			this._finished = true;
			long num = this.nowPos64;
			if (this.nowPos64 == 0L)
			{
				if (this._matchFinder.GetNumAvailableBytes() == 0U)
				{
					this.Flush((uint)this.nowPos64);
					return;
				}
				uint num2;
				uint num3;
				this.ReadMatchDistances(out num2, out num3);
				uint num4 = (uint)this.nowPos64 & this._posStateMask;
				this._isMatch[(int)((this._state.Index << 4) + num4)].Encode(this._rangeEncoder, 0U);
				this._state.UpdateChar();
				byte indexByte = this._matchFinder.GetIndexByte((int)(0U - this._additionalOffset));
				this._literalEncoder.GetSubCoder((uint)this.nowPos64, this._previousByte).Encode(this._rangeEncoder, indexByte);
				this._previousByte = indexByte;
				this._additionalOffset -= 1U;
				this.nowPos64 += 1L;
			}
			if (this._matchFinder.GetNumAvailableBytes() == 0U)
			{
				this.Flush((uint)this.nowPos64);
				return;
			}
			for (;;)
			{
				uint num5;
				uint optimum = this.GetOptimum((uint)this.nowPos64, out num5);
				uint num6 = (uint)this.nowPos64 & this._posStateMask;
				uint num7 = (this._state.Index << 4) + num6;
				if (optimum == 1U && num5 == 4294967295U)
				{
					this._isMatch[(int)num7].Encode(this._rangeEncoder, 0U);
					byte indexByte2 = this._matchFinder.GetIndexByte((int)(0U - this._additionalOffset));
					Encoder.LiteralEncoder.Encoder2 subCoder = this._literalEncoder.GetSubCoder((uint)this.nowPos64, this._previousByte);
					if (!this._state.IsCharState())
					{
						byte indexByte3 = this._matchFinder.GetIndexByte((int)(0U - this._repDistances[0] - 1U - this._additionalOffset));
						subCoder.EncodeMatched(this._rangeEncoder, indexByte3, indexByte2);
					}
					else
					{
						subCoder.Encode(this._rangeEncoder, indexByte2);
					}
					this._previousByte = indexByte2;
					this._state.UpdateChar();
				}
				else
				{
					this._isMatch[(int)num7].Encode(this._rangeEncoder, 1U);
					if (num5 < 4U)
					{
						this._isRep[(int)this._state.Index].Encode(this._rangeEncoder, 1U);
						if (num5 == 0U)
						{
							this._isRepG0[(int)this._state.Index].Encode(this._rangeEncoder, 0U);
							if (optimum == 1U)
							{
								this._isRep0Long[(int)num7].Encode(this._rangeEncoder, 0U);
							}
							else
							{
								this._isRep0Long[(int)num7].Encode(this._rangeEncoder, 1U);
							}
						}
						else
						{
							this._isRepG0[(int)this._state.Index].Encode(this._rangeEncoder, 1U);
							if (num5 == 1U)
							{
								this._isRepG1[(int)this._state.Index].Encode(this._rangeEncoder, 0U);
							}
							else
							{
								this._isRepG1[(int)this._state.Index].Encode(this._rangeEncoder, 1U);
								this._isRepG2[(int)this._state.Index].Encode(this._rangeEncoder, num5 - 2U);
							}
						}
						if (optimum == 1U)
						{
							this._state.UpdateShortRep();
						}
						else
						{
							this._repMatchLenEncoder.Encode(this._rangeEncoder, optimum - 2U, num6);
							this._state.UpdateRep();
						}
						uint num8 = this._repDistances[(int)num5];
						if (num5 != 0U)
						{
							for (uint num9 = num5; num9 >= 1U; num9 -= 1U)
							{
								this._repDistances[(int)num9] = this._repDistances[(int)(num9 - 1U)];
							}
							this._repDistances[0] = num8;
						}
					}
					else
					{
						this._isRep[(int)this._state.Index].Encode(this._rangeEncoder, 0U);
						this._state.UpdateMatch();
						this._lenEncoder.Encode(this._rangeEncoder, optimum - 2U, num6);
						num5 -= 4U;
						uint posSlot = Encoder.GetPosSlot(num5);
						uint lenToPosState = Base.GetLenToPosState(optimum);
						this._posSlotEncoder[(int)lenToPosState].Encode(this._rangeEncoder, posSlot);
						if (posSlot >= 4U)
						{
							int num10 = (int)((posSlot >> 1) - 1U);
							uint num11 = (2U | (posSlot & 1U)) << num10;
							uint num12 = num5 - num11;
							if (posSlot < 14U)
							{
								BitTreeEncoder.ReverseEncode(this._posEncoders, num11 - posSlot - 1U, this._rangeEncoder, num10, num12);
							}
							else
							{
								this._rangeEncoder.EncodeDirectBits(num12 >> 4, num10 - 4);
								this._posAlignEncoder.ReverseEncode(this._rangeEncoder, num12 & 15U);
								this._alignPriceCount += 1U;
							}
						}
						uint num13 = num5;
						for (uint num14 = 3U; num14 >= 1U; num14 -= 1U)
						{
							this._repDistances[(int)num14] = this._repDistances[(int)(num14 - 1U)];
						}
						this._repDistances[0] = num13;
						this._matchPriceCount += 1U;
					}
					this._previousByte = this._matchFinder.GetIndexByte((int)(optimum - 1U - this._additionalOffset));
				}
				this._additionalOffset -= optimum;
				this.nowPos64 += (long)((ulong)optimum);
				if (this._additionalOffset == 0U)
				{
					if (this._matchPriceCount >= 128U)
					{
						this.FillDistancesPrices();
					}
					if (this._alignPriceCount >= 16U)
					{
						this.FillAlignPrices();
					}
					inSize = this.nowPos64;
					outSize = this._rangeEncoder.GetProcessedSizeAdd();
					if (this._matchFinder.GetNumAvailableBytes() == 0U)
					{
						break;
					}
					if (this.nowPos64 - num >= 4096L)
					{
						goto Block_24;
					}
				}
			}
			this.Flush((uint)this.nowPos64);
			return;
			Block_24:
			this._finished = false;
			finished = false;
		}

		// Token: 0x06000133 RID: 307 RVA: 0x00008F0A File Offset: 0x0000710A
		private void ReleaseMFStream()
		{
			if (this._matchFinder != null && this._needReleaseMFStream)
			{
				this._matchFinder.ReleaseStream();
				this._needReleaseMFStream = false;
			}
		}

		// Token: 0x06000134 RID: 308 RVA: 0x00008F2E File Offset: 0x0000712E
		private void SetOutStream(Stream outStream)
		{
			this._rangeEncoder.SetStream(outStream);
		}

		// Token: 0x06000135 RID: 309 RVA: 0x00008F3C File Offset: 0x0000713C
		private void ReleaseOutStream()
		{
			this._rangeEncoder.ReleaseStream();
		}

		// Token: 0x06000136 RID: 310 RVA: 0x00008F49 File Offset: 0x00007149
		private void ReleaseStreams()
		{
			this.ReleaseMFStream();
			this.ReleaseOutStream();
		}

		// Token: 0x06000137 RID: 311 RVA: 0x00008F58 File Offset: 0x00007158
		private void SetStreams(Stream inStream, Stream outStream, long inSize, long outSize)
		{
			this._inStream = inStream;
			this._finished = false;
			this.Create();
			this.SetOutStream(outStream);
			this.Init();
			this.FillDistancesPrices();
			this.FillAlignPrices();
			this._lenEncoder.SetTableSize(this._numFastBytes + 1U - 2U);
			this._lenEncoder.UpdateTables(1U << this._posStateBits);
			this._repMatchLenEncoder.SetTableSize(this._numFastBytes + 1U - 2U);
			this._repMatchLenEncoder.UpdateTables(1U << this._posStateBits);
			this.nowPos64 = 0L;
		}

		// Token: 0x06000138 RID: 312 RVA: 0x00008FF0 File Offset: 0x000071F0
		public void Code(Stream inStream, Stream outStream, long inSize, long outSize, ICodeProgress progress)
		{
			this._needReleaseMFStream = false;
			try
			{
				this.SetStreams(inStream, outStream, inSize, outSize);
				for (;;)
				{
					long num;
					long num2;
					bool flag;
					this.CodeOneBlock(out num, out num2, out flag);
					if (flag)
					{
						break;
					}
					if (progress != null)
					{
						progress.SetProgress(num, num2);
					}
				}
			}
			finally
			{
				this.ReleaseStreams();
			}
		}

		// Token: 0x06000139 RID: 313 RVA: 0x00009048 File Offset: 0x00007248
		public void WriteCoderProperties(Stream outStream)
		{
			this.properties[0] = (byte)((this._posStateBits * 5 + this._numLiteralPosStateBits) * 9 + this._numLiteralContextBits);
			for (int i = 0; i < 4; i++)
			{
				this.properties[1 + i] = (byte)((this._dictionarySize >> 8 * i) & 255U);
			}
			outStream.Write(this.properties, 0, 5);
		}

		// Token: 0x0600013A RID: 314 RVA: 0x000090B0 File Offset: 0x000072B0
		private void FillDistancesPrices()
		{
			for (uint num = 4U; num < 128U; num += 1U)
			{
				uint posSlot = Encoder.GetPosSlot(num);
				int num2 = (int)((posSlot >> 1) - 1U);
				uint num3 = (2U | (posSlot & 1U)) << num2;
				this.tempPrices[(int)num] = BitTreeEncoder.ReverseGetPrice(this._posEncoders, num3 - posSlot - 1U, num2, num - num3);
			}
			for (uint num4 = 0U; num4 < 4U; num4 += 1U)
			{
				BitTreeEncoder bitTreeEncoder = this._posSlotEncoder[(int)num4];
				uint num5 = num4 << 6;
				for (uint num6 = 0U; num6 < this._distTableSize; num6 += 1U)
				{
					this._posSlotPrices[(int)(num5 + num6)] = bitTreeEncoder.GetPrice(num6);
				}
				for (uint num6 = 14U; num6 < this._distTableSize; num6 += 1U)
				{
					this._posSlotPrices[(int)(num5 + num6)] += (num6 >> 1) - 1U - 4U << 6;
				}
				uint num7 = num4 * 128U;
				uint num8;
				for (num8 = 0U; num8 < 4U; num8 += 1U)
				{
					this._distancesPrices[(int)(num7 + num8)] = this._posSlotPrices[(int)(num5 + num8)];
				}
				while (num8 < 128U)
				{
					this._distancesPrices[(int)(num7 + num8)] = this._posSlotPrices[(int)(num5 + Encoder.GetPosSlot(num8))] + this.tempPrices[(int)num8];
					num8 += 1U;
				}
			}
			this._matchPriceCount = 0U;
		}

		// Token: 0x0600013B RID: 315 RVA: 0x000091FC File Offset: 0x000073FC
		private void FillAlignPrices()
		{
			for (uint num = 0U; num < 16U; num += 1U)
			{
				this._alignPrices[(int)num] = this._posAlignEncoder.ReverseGetPrice(num);
			}
			this._alignPriceCount = 0U;
		}

		// Token: 0x0600013C RID: 316 RVA: 0x00009234 File Offset: 0x00007434
		private static int FindMatchFinder(string s)
		{
			for (int i = 0; i < Encoder.kMatchFinderIDs.Length; i++)
			{
				if (s == Encoder.kMatchFinderIDs[i])
				{
					return i;
				}
			}
			return -1;
		}

		// Token: 0x0600013D RID: 317 RVA: 0x00009268 File Offset: 0x00007468
		public void SetCoderProperties(CoderPropID[] propIDs, object[] properties)
		{
			uint num = 0U;
			while ((ulong)num < (ulong)((long)properties.Length))
			{
				object obj = properties[(int)num];
				switch (propIDs[(int)num])
				{
				case CoderPropID.DictionarySize:
				{
					if (!(obj is int))
					{
						throw new InvalidParamException();
					}
					int num2 = (int)obj;
					if ((long)num2 < 1L || (long)num2 > 1073741824L)
					{
						throw new InvalidParamException();
					}
					this._dictionarySize = (uint)num2;
					int num3 = 0;
					while ((long)num3 < 30L && (long)num2 > (long)(1UL << (num3 & 31)))
					{
						num3++;
					}
					this._distTableSize = (uint)(num3 * 2);
					break;
				}
				case CoderPropID.UsedMemorySize:
				case CoderPropID.Order:
				case CoderPropID.BlockSize:
				case CoderPropID.MatchFinderCycles:
				case CoderPropID.NumPasses:
				case CoderPropID.NumThreads:
					goto IL_021C;
				case CoderPropID.PosStateBits:
				{
					if (!(obj is int))
					{
						throw new InvalidParamException();
					}
					int num4 = (int)obj;
					if (num4 < 0 || (long)num4 > 4L)
					{
						throw new InvalidParamException();
					}
					this._posStateBits = num4;
					this._posStateMask = (1U << this._posStateBits) - 1U;
					break;
				}
				case CoderPropID.LitContextBits:
				{
					if (!(obj is int))
					{
						throw new InvalidParamException();
					}
					int num5 = (int)obj;
					if (num5 < 0 || (long)num5 > 8L)
					{
						throw new InvalidParamException();
					}
					this._numLiteralContextBits = num5;
					break;
				}
				case CoderPropID.LitPosBits:
				{
					if (!(obj is int))
					{
						throw new InvalidParamException();
					}
					int num6 = (int)obj;
					if (num6 < 0 || (long)num6 > 4L)
					{
						throw new InvalidParamException();
					}
					this._numLiteralPosStateBits = num6;
					break;
				}
				case CoderPropID.NumFastBytes:
				{
					if (!(obj is int))
					{
						throw new InvalidParamException();
					}
					int num7 = (int)obj;
					if (num7 < 5 || (long)num7 > 273L)
					{
						throw new InvalidParamException();
					}
					this._numFastBytes = (uint)num7;
					break;
				}
				case CoderPropID.MatchFinder:
				{
					if (!(obj is string))
					{
						throw new InvalidParamException();
					}
					Encoder.EMatchFinderType matchFinderType = this._matchFinderType;
					int num8 = Encoder.FindMatchFinder(((string)obj).ToUpper());
					if (num8 < 0)
					{
						throw new InvalidParamException();
					}
					this._matchFinderType = (Encoder.EMatchFinderType)num8;
					if (this._matchFinder != null && matchFinderType != this._matchFinderType)
					{
						this._dictionarySizePrev = uint.MaxValue;
						this._matchFinder = null;
					}
					break;
				}
				case CoderPropID.Algorithm:
					break;
				case CoderPropID.EndMarker:
					if (!(obj is bool))
					{
						throw new InvalidParamException();
					}
					this.SetWriteEndMarkerMode((bool)obj);
					break;
				default:
					goto IL_021C;
				}
				num += 1U;
				continue;
				IL_021C:
				throw new InvalidParamException();
			}
		}

		// Token: 0x0600013E RID: 318 RVA: 0x000094A6 File Offset: 0x000076A6
		public void SetTrainSize(uint trainSize)
		{
			this._trainSize = trainSize;
		}

		// Token: 0x040000DC RID: 220
		private const uint kIfinityPrice = 268435455U;

		// Token: 0x040000DD RID: 221
		private static byte[] g_FastPos = new byte[2048];

		// Token: 0x040000DE RID: 222
		private Base.State _state;

		// Token: 0x040000DF RID: 223
		private byte _previousByte;

		// Token: 0x040000E0 RID: 224
		private uint[] _repDistances = new uint[4];

		// Token: 0x040000E1 RID: 225
		private const int kDefaultDictionaryLogSize = 22;

		// Token: 0x040000E2 RID: 226
		private const uint kNumFastBytesDefault = 32U;

		// Token: 0x040000E3 RID: 227
		private const uint kNumLenSpecSymbols = 16U;

		// Token: 0x040000E4 RID: 228
		private const uint kNumOpts = 4096U;

		// Token: 0x040000E5 RID: 229
		private Encoder.Optimal[] _optimum = new Encoder.Optimal[4096];

		// Token: 0x040000E6 RID: 230
		private IMatchFinder _matchFinder;

		// Token: 0x040000E7 RID: 231
		private Encoder _rangeEncoder = new Encoder();

		// Token: 0x040000E8 RID: 232
		private BitEncoder[] _isMatch = new BitEncoder[192];

		// Token: 0x040000E9 RID: 233
		private BitEncoder[] _isRep = new BitEncoder[12];

		// Token: 0x040000EA RID: 234
		private BitEncoder[] _isRepG0 = new BitEncoder[12];

		// Token: 0x040000EB RID: 235
		private BitEncoder[] _isRepG1 = new BitEncoder[12];

		// Token: 0x040000EC RID: 236
		private BitEncoder[] _isRepG2 = new BitEncoder[12];

		// Token: 0x040000ED RID: 237
		private BitEncoder[] _isRep0Long = new BitEncoder[192];

		// Token: 0x040000EE RID: 238
		private BitTreeEncoder[] _posSlotEncoder = new BitTreeEncoder[4];

		// Token: 0x040000EF RID: 239
		private BitEncoder[] _posEncoders = new BitEncoder[114];

		// Token: 0x040000F0 RID: 240
		private BitTreeEncoder _posAlignEncoder = new BitTreeEncoder(4);

		// Token: 0x040000F1 RID: 241
		private Encoder.LenPriceTableEncoder _lenEncoder = new Encoder.LenPriceTableEncoder();

		// Token: 0x040000F2 RID: 242
		private Encoder.LenPriceTableEncoder _repMatchLenEncoder = new Encoder.LenPriceTableEncoder();

		// Token: 0x040000F3 RID: 243
		private Encoder.LiteralEncoder _literalEncoder = new Encoder.LiteralEncoder();

		// Token: 0x040000F4 RID: 244
		private uint[] _matchDistances = new uint[548];

		// Token: 0x040000F5 RID: 245
		private uint _numFastBytes = 32U;

		// Token: 0x040000F6 RID: 246
		private uint _longestMatchLength;

		// Token: 0x040000F7 RID: 247
		private uint _numDistancePairs;

		// Token: 0x040000F8 RID: 248
		private uint _additionalOffset;

		// Token: 0x040000F9 RID: 249
		private uint _optimumEndIndex;

		// Token: 0x040000FA RID: 250
		private uint _optimumCurrentIndex;

		// Token: 0x040000FB RID: 251
		private bool _longestMatchWasFound;

		// Token: 0x040000FC RID: 252
		private uint[] _posSlotPrices = new uint[256];

		// Token: 0x040000FD RID: 253
		private uint[] _distancesPrices = new uint[512];

		// Token: 0x040000FE RID: 254
		private uint[] _alignPrices = new uint[16];

		// Token: 0x040000FF RID: 255
		private uint _alignPriceCount;

		// Token: 0x04000100 RID: 256
		private uint _distTableSize = 44U;

		// Token: 0x04000101 RID: 257
		private int _posStateBits = 2;

		// Token: 0x04000102 RID: 258
		private uint _posStateMask = 3U;

		// Token: 0x04000103 RID: 259
		private int _numLiteralPosStateBits;

		// Token: 0x04000104 RID: 260
		private int _numLiteralContextBits = 3;

		// Token: 0x04000105 RID: 261
		private uint _dictionarySize = 4194304U;

		// Token: 0x04000106 RID: 262
		private uint _dictionarySizePrev = uint.MaxValue;

		// Token: 0x04000107 RID: 263
		private uint _numFastBytesPrev = uint.MaxValue;

		// Token: 0x04000108 RID: 264
		private long nowPos64;

		// Token: 0x04000109 RID: 265
		private bool _finished;

		// Token: 0x0400010A RID: 266
		private Stream _inStream;

		// Token: 0x0400010B RID: 267
		private Encoder.EMatchFinderType _matchFinderType = Encoder.EMatchFinderType.BT4;

		// Token: 0x0400010C RID: 268
		private bool _writeEndMark;

		// Token: 0x0400010D RID: 269
		private bool _needReleaseMFStream;

		// Token: 0x0400010E RID: 270
		private uint[] reps = new uint[4];

		// Token: 0x0400010F RID: 271
		private uint[] repLens = new uint[4];

		// Token: 0x04000110 RID: 272
		private const int kPropSize = 5;

		// Token: 0x04000111 RID: 273
		private byte[] properties = new byte[5];

		// Token: 0x04000112 RID: 274
		private uint[] tempPrices = new uint[128];

		// Token: 0x04000113 RID: 275
		private uint _matchPriceCount;

		// Token: 0x04000114 RID: 276
		private static string[] kMatchFinderIDs = new string[] { "BT2", "BT4" };

		// Token: 0x04000115 RID: 277
		private uint _trainSize;

		// Token: 0x02000030 RID: 48
		private enum EMatchFinderType
		{
			// Token: 0x04000117 RID: 279
			BT2,
			// Token: 0x04000118 RID: 280
			BT4
		}

		// Token: 0x02000031 RID: 49
		private class LiteralEncoder
		{
			// Token: 0x0600013F RID: 319 RVA: 0x000094B0 File Offset: 0x000076B0
			public void Create(int numPosBits, int numPrevBits)
			{
				if (this.m_Coders != null && this.m_NumPrevBits == numPrevBits && this.m_NumPosBits == numPosBits)
				{
					return;
				}
				this.m_NumPosBits = numPosBits;
				this.m_PosMask = (1U << numPosBits) - 1U;
				this.m_NumPrevBits = numPrevBits;
				uint num = 1U << this.m_NumPrevBits + this.m_NumPosBits;
				this.m_Coders = new Encoder.LiteralEncoder.Encoder2[num];
				for (uint num2 = 0U; num2 < num; num2 += 1U)
				{
					this.m_Coders[(int)num2].Create();
				}
			}

			// Token: 0x06000140 RID: 320 RVA: 0x00009530 File Offset: 0x00007730
			public void Init()
			{
				uint num = 1U << this.m_NumPrevBits + this.m_NumPosBits;
				for (uint num2 = 0U; num2 < num; num2 += 1U)
				{
					this.m_Coders[(int)num2].Init();
				}
			}

			// Token: 0x06000141 RID: 321 RVA: 0x0000956D File Offset: 0x0000776D
			public Encoder.LiteralEncoder.Encoder2 GetSubCoder(uint pos, byte prevByte)
			{
				return this.m_Coders[(int)(((pos & this.m_PosMask) << this.m_NumPrevBits) + (uint)(prevByte >> 8 - this.m_NumPrevBits))];
			}

			// Token: 0x06000142 RID: 322 RVA: 0x00002050 File Offset: 0x00000250
			public LiteralEncoder()
			{
			}

			// Token: 0x04000119 RID: 281
			private Encoder.LiteralEncoder.Encoder2[] m_Coders;

			// Token: 0x0400011A RID: 282
			private int m_NumPrevBits;

			// Token: 0x0400011B RID: 283
			private int m_NumPosBits;

			// Token: 0x0400011C RID: 284
			private uint m_PosMask;

			// Token: 0x02000032 RID: 50
			public struct Encoder2
			{
				// Token: 0x06000143 RID: 323 RVA: 0x0000959A File Offset: 0x0000779A
				public void Create()
				{
					this.m_Encoders = new BitEncoder[768];
				}

				// Token: 0x06000144 RID: 324 RVA: 0x000095AC File Offset: 0x000077AC
				public void Init()
				{
					for (int i = 0; i < 768; i++)
					{
						this.m_Encoders[i].Init();
					}
				}

				// Token: 0x06000145 RID: 325 RVA: 0x000095DC File Offset: 0x000077DC
				public void Encode(Encoder rangeEncoder, byte symbol)
				{
					uint num = 1U;
					for (int i = 7; i >= 0; i--)
					{
						uint num2 = (uint)((symbol >> i) & 1);
						this.m_Encoders[(int)num].Encode(rangeEncoder, num2);
						num = (num << 1) | num2;
					}
				}

				// Token: 0x06000146 RID: 326 RVA: 0x0000961C File Offset: 0x0000781C
				public void EncodeMatched(Encoder rangeEncoder, byte matchByte, byte symbol)
				{
					uint num = 1U;
					bool flag = true;
					for (int i = 7; i >= 0; i--)
					{
						uint num2 = (uint)((symbol >> i) & 1);
						uint num3 = num;
						if (flag)
						{
							uint num4 = (uint)((matchByte >> i) & 1);
							num3 += 1U + num4 << 8;
							flag = num4 == num2;
						}
						this.m_Encoders[(int)num3].Encode(rangeEncoder, num2);
						num = (num << 1) | num2;
					}
				}

				// Token: 0x06000147 RID: 327 RVA: 0x00009680 File Offset: 0x00007880
				public uint GetPrice(bool matchMode, byte matchByte, byte symbol)
				{
					uint num = 0U;
					uint num2 = 1U;
					int i = 7;
					if (matchMode)
					{
						while (i >= 0)
						{
							uint num3 = (uint)((matchByte >> i) & 1);
							uint num4 = (uint)((symbol >> i) & 1);
							num += this.m_Encoders[(int)((1U + num3 << 8) + num2)].GetPrice(num4);
							num2 = (num2 << 1) | num4;
							if (num3 != num4)
							{
								i--;
								break;
							}
							i--;
						}
					}
					while (i >= 0)
					{
						uint num5 = (uint)((symbol >> i) & 1);
						num += this.m_Encoders[(int)num2].GetPrice(num5);
						num2 = (num2 << 1) | num5;
						i--;
					}
					return num;
				}

				// Token: 0x0400011D RID: 285
				private BitEncoder[] m_Encoders;
			}
		}

		// Token: 0x02000033 RID: 51
		private class LenEncoder
		{
			// Token: 0x06000148 RID: 328 RVA: 0x00009714 File Offset: 0x00007914
			public LenEncoder()
			{
				for (uint num = 0U; num < 16U; num += 1U)
				{
					this._lowCoder[(int)num] = new BitTreeEncoder(3);
					this._midCoder[(int)num] = new BitTreeEncoder(3);
				}
			}

			// Token: 0x06000149 RID: 329 RVA: 0x00009780 File Offset: 0x00007980
			public void Init(uint numPosStates)
			{
				this._choice.Init();
				this._choice2.Init();
				for (uint num = 0U; num < numPosStates; num += 1U)
				{
					this._lowCoder[(int)num].Init();
					this._midCoder[(int)num].Init();
				}
				this._highCoder.Init();
			}

			// Token: 0x0600014A RID: 330 RVA: 0x000097DC File Offset: 0x000079DC
			public void Encode(Encoder rangeEncoder, uint symbol, uint posState)
			{
				if (symbol < 8U)
				{
					this._choice.Encode(rangeEncoder, 0U);
					this._lowCoder[(int)posState].Encode(rangeEncoder, symbol);
					return;
				}
				symbol -= 8U;
				this._choice.Encode(rangeEncoder, 1U);
				if (symbol < 8U)
				{
					this._choice2.Encode(rangeEncoder, 0U);
					this._midCoder[(int)posState].Encode(rangeEncoder, symbol);
					return;
				}
				this._choice2.Encode(rangeEncoder, 1U);
				this._highCoder.Encode(rangeEncoder, symbol - 8U);
			}

			// Token: 0x0600014B RID: 331 RVA: 0x00009864 File Offset: 0x00007A64
			public void SetPrices(uint posState, uint numSymbols, uint[] prices, uint st)
			{
				uint price = this._choice.GetPrice0();
				uint price2 = this._choice.GetPrice1();
				uint num = price2 + this._choice2.GetPrice0();
				uint num2 = price2 + this._choice2.GetPrice1();
				uint num3;
				for (num3 = 0U; num3 < 8U; num3 += 1U)
				{
					if (num3 >= numSymbols)
					{
						return;
					}
					prices[(int)(st + num3)] = price + this._lowCoder[(int)posState].GetPrice(num3);
				}
				while (num3 < 16U)
				{
					if (num3 >= numSymbols)
					{
						return;
					}
					prices[(int)(st + num3)] = num + this._midCoder[(int)posState].GetPrice(num3 - 8U);
					num3 += 1U;
				}
				while (num3 < numSymbols)
				{
					prices[(int)(st + num3)] = num2 + this._highCoder.GetPrice(num3 - 8U - 8U);
					num3 += 1U;
				}
			}

			// Token: 0x0400011E RID: 286
			private BitEncoder _choice;

			// Token: 0x0400011F RID: 287
			private BitEncoder _choice2;

			// Token: 0x04000120 RID: 288
			private BitTreeEncoder[] _lowCoder = new BitTreeEncoder[16];

			// Token: 0x04000121 RID: 289
			private BitTreeEncoder[] _midCoder = new BitTreeEncoder[16];

			// Token: 0x04000122 RID: 290
			private BitTreeEncoder _highCoder = new BitTreeEncoder(8);
		}

		// Token: 0x02000034 RID: 52
		private class LenPriceTableEncoder : Encoder.LenEncoder
		{
			// Token: 0x0600014C RID: 332 RVA: 0x0000991E File Offset: 0x00007B1E
			public void SetTableSize(uint tableSize)
			{
				this._tableSize = tableSize;
			}

			// Token: 0x0600014D RID: 333 RVA: 0x00009927 File Offset: 0x00007B27
			public uint GetPrice(uint symbol, uint posState)
			{
				return this._prices[(int)(posState * 272U + symbol)];
			}

			// Token: 0x0600014E RID: 334 RVA: 0x00009939 File Offset: 0x00007B39
			private void UpdateTable(uint posState)
			{
				base.SetPrices(posState, this._tableSize, this._prices, posState * 272U);
				this._counters[(int)posState] = this._tableSize;
			}

			// Token: 0x0600014F RID: 335 RVA: 0x00009964 File Offset: 0x00007B64
			public void UpdateTables(uint numPosStates)
			{
				for (uint num = 0U; num < numPosStates; num += 1U)
				{
					this.UpdateTable(num);
				}
			}

			// Token: 0x06000150 RID: 336 RVA: 0x00009984 File Offset: 0x00007B84
			public new void Encode(Encoder rangeEncoder, uint symbol, uint posState)
			{
				base.Encode(rangeEncoder, symbol, posState);
				uint[] counters = this._counters;
				uint num = counters[(int)posState] - 1U;
				counters[(int)posState] = num;
				if (num == 0U)
				{
					this.UpdateTable(posState);
				}
			}

			// Token: 0x06000151 RID: 337 RVA: 0x000099B7 File Offset: 0x00007BB7
			public LenPriceTableEncoder()
			{
			}

			// Token: 0x04000123 RID: 291
			private uint[] _prices = new uint[4352];

			// Token: 0x04000124 RID: 292
			private uint _tableSize;

			// Token: 0x04000125 RID: 293
			private uint[] _counters = new uint[16];
		}

		// Token: 0x02000035 RID: 53
		private class Optimal
		{
			// Token: 0x06000152 RID: 338 RVA: 0x000099DC File Offset: 0x00007BDC
			public void MakeAsChar()
			{
				this.BackPrev = uint.MaxValue;
				this.Prev1IsChar = false;
			}

			// Token: 0x06000153 RID: 339 RVA: 0x000099EC File Offset: 0x00007BEC
			public void MakeAsShortRep()
			{
				this.BackPrev = 0U;
				this.Prev1IsChar = false;
			}

			// Token: 0x06000154 RID: 340 RVA: 0x000099FC File Offset: 0x00007BFC
			public bool IsShortRep()
			{
				return this.BackPrev == 0U;
			}

			// Token: 0x06000155 RID: 341 RVA: 0x00002050 File Offset: 0x00000250
			public Optimal()
			{
			}

			// Token: 0x04000126 RID: 294
			public Base.State State;

			// Token: 0x04000127 RID: 295
			public bool Prev1IsChar;

			// Token: 0x04000128 RID: 296
			public bool Prev2;

			// Token: 0x04000129 RID: 297
			public uint PosPrev2;

			// Token: 0x0400012A RID: 298
			public uint BackPrev2;

			// Token: 0x0400012B RID: 299
			public uint Price;

			// Token: 0x0400012C RID: 300
			public uint PosPrev;

			// Token: 0x0400012D RID: 301
			public uint BackPrev;

			// Token: 0x0400012E RID: 302
			public uint Backs0;

			// Token: 0x0400012F RID: 303
			public uint Backs1;

			// Token: 0x04000130 RID: 304
			public uint Backs2;

			// Token: 0x04000131 RID: 305
			public uint Backs3;
		}
	}
}
