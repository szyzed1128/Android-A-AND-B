using System;

namespace SevenZip.Compression.LZMA
{
	// Token: 0x02000029 RID: 41
	internal abstract class Base
	{
		// Token: 0x06000101 RID: 257 RVA: 0x00006583 File Offset: 0x00004783
		public static uint GetLenToPosState(uint len)
		{
			len -= 2U;
			if (len < 4U)
			{
				return len;
			}
			return 3U;
		}

		// Token: 0x06000102 RID: 258 RVA: 0x00002050 File Offset: 0x00000250
		protected Base()
		{
		}

		// Token: 0x040000A3 RID: 163
		public const uint kNumRepDistances = 4U;

		// Token: 0x040000A4 RID: 164
		public const uint kNumStates = 12U;

		// Token: 0x040000A5 RID: 165
		public const int kNumPosSlotBits = 6;

		// Token: 0x040000A6 RID: 166
		public const int kDicLogSizeMin = 0;

		// Token: 0x040000A7 RID: 167
		public const int kNumLenToPosStatesBits = 2;

		// Token: 0x040000A8 RID: 168
		public const uint kNumLenToPosStates = 4U;

		// Token: 0x040000A9 RID: 169
		public const uint kMatchMinLen = 2U;

		// Token: 0x040000AA RID: 170
		public const int kNumAlignBits = 4;

		// Token: 0x040000AB RID: 171
		public const uint kAlignTableSize = 16U;

		// Token: 0x040000AC RID: 172
		public const uint kAlignMask = 15U;

		// Token: 0x040000AD RID: 173
		public const uint kStartPosModelIndex = 4U;

		// Token: 0x040000AE RID: 174
		public const uint kEndPosModelIndex = 14U;

		// Token: 0x040000AF RID: 175
		public const uint kNumPosModels = 10U;

		// Token: 0x040000B0 RID: 176
		public const uint kNumFullDistances = 128U;

		// Token: 0x040000B1 RID: 177
		public const uint kNumLitPosStatesBitsEncodingMax = 4U;

		// Token: 0x040000B2 RID: 178
		public const uint kNumLitContextBitsMax = 8U;

		// Token: 0x040000B3 RID: 179
		public const int kNumPosStatesBitsMax = 4;

		// Token: 0x040000B4 RID: 180
		public const uint kNumPosStatesMax = 16U;

		// Token: 0x040000B5 RID: 181
		public const int kNumPosStatesBitsEncodingMax = 4;

		// Token: 0x040000B6 RID: 182
		public const uint kNumPosStatesEncodingMax = 16U;

		// Token: 0x040000B7 RID: 183
		public const int kNumLowLenBits = 3;

		// Token: 0x040000B8 RID: 184
		public const int kNumMidLenBits = 3;

		// Token: 0x040000B9 RID: 185
		public const int kNumHighLenBits = 8;

		// Token: 0x040000BA RID: 186
		public const uint kNumLowLenSymbols = 8U;

		// Token: 0x040000BB RID: 187
		public const uint kNumMidLenSymbols = 8U;

		// Token: 0x040000BC RID: 188
		public const uint kNumLenSymbols = 272U;

		// Token: 0x040000BD RID: 189
		public const uint kMatchMaxLen = 273U;

		// Token: 0x0200002A RID: 42
		public struct State
		{
			// Token: 0x06000103 RID: 259 RVA: 0x00006591 File Offset: 0x00004791
			public void Init()
			{
				this.Index = 0U;
			}

			// Token: 0x06000104 RID: 260 RVA: 0x0000659A File Offset: 0x0000479A
			public void UpdateChar()
			{
				if (this.Index < 4U)
				{
					this.Index = 0U;
					return;
				}
				if (this.Index < 10U)
				{
					this.Index -= 3U;
					return;
				}
				this.Index -= 6U;
			}

			// Token: 0x06000105 RID: 261 RVA: 0x000065D4 File Offset: 0x000047D4
			public void UpdateMatch()
			{
				this.Index = ((this.Index < 7U) ? 7U : 10U);
			}

			// Token: 0x06000106 RID: 262 RVA: 0x000065EA File Offset: 0x000047EA
			public void UpdateRep()
			{
				this.Index = ((this.Index < 7U) ? 8U : 11U);
			}

			// Token: 0x06000107 RID: 263 RVA: 0x00006600 File Offset: 0x00004800
			public void UpdateShortRep()
			{
				this.Index = ((this.Index < 7U) ? 9U : 11U);
			}

			// Token: 0x06000108 RID: 264 RVA: 0x00006617 File Offset: 0x00004817
			public bool IsCharState()
			{
				return this.Index < 7U;
			}

			// Token: 0x040000BE RID: 190
			public uint Index;
		}
	}
}
