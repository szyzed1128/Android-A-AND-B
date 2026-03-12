using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
using CarScannerXamarinForms.Common;

namespace CarScannerXamarinForms.Coding.DB.Toyota
{
	// Token: 0x020009D9 RID: 2521
	internal class ToyotaA8Item : IDataPreprocessor
	{
		// Token: 0x170017A2 RID: 6050
		// (get) Token: 0x0600514F RID: 20815 RVA: 0x003F136E File Offset: 0x003EF56E
		// (set) Token: 0x06005150 RID: 20816 RVA: 0x003F1376 File Offset: 0x003EF576
		public string CanIdHex
		{
			[CompilerGenerated]
			get
			{
				return this.<CanIdHex>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<CanIdHex>k__BackingField = value;
			}
		} = "";

		// Token: 0x170017A3 RID: 6051
		// (get) Token: 0x06005151 RID: 20817 RVA: 0x003F137F File Offset: 0x003EF57F
		// (set) Token: 0x06005152 RID: 20818 RVA: 0x003F1387 File Offset: 0x003EF587
		public string ExtendedAddress
		{
			[CompilerGenerated]
			get
			{
				return this.<ExtendedAddress>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<ExtendedAddress>k__BackingField = value;
			}
		} = "";

		// Token: 0x170017A4 RID: 6052
		// (get) Token: 0x06005153 RID: 20819 RVA: 0x003F1390 File Offset: 0x003EF590
		// (set) Token: 0x06005154 RID: 20820 RVA: 0x003F1398 File Offset: 0x003EF598
		public string IdHex
		{
			[CompilerGenerated]
			get
			{
				return this.<IdHex>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<IdHex>k__BackingField = value;
			}
		}

		// Token: 0x170017A5 RID: 6053
		// (get) Token: 0x06005155 RID: 20821 RVA: 0x003F13A1 File Offset: 0x003EF5A1
		// (set) Token: 0x06005156 RID: 20822 RVA: 0x003F13A9 File Offset: 0x003EF5A9
		public byte Id
		{
			[CompilerGenerated]
			get
			{
				return this.<Id>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<Id>k__BackingField = value;
			}
		}

		// Token: 0x170017A6 RID: 6054
		// (get) Token: 0x06005157 RID: 20823 RVA: 0x003F13B2 File Offset: 0x003EF5B2
		// (set) Token: 0x06005158 RID: 20824 RVA: 0x003F13BA File Offset: 0x003EF5BA
		public int Length
		{
			[CompilerGenerated]
			get
			{
				return this.<Length>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<Length>k__BackingField = value;
			}
		}

		// Token: 0x170017A7 RID: 6055
		// (get) Token: 0x06005159 RID: 20825 RVA: 0x003F13C3 File Offset: 0x003EF5C3
		// (set) Token: 0x0600515A RID: 20826 RVA: 0x003F13CB File Offset: 0x003EF5CB
		public string MaskHex
		{
			[CompilerGenerated]
			get
			{
				return this.<MaskHex>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<MaskHex>k__BackingField = value;
			}
		}

		// Token: 0x170017A8 RID: 6056
		// (get) Token: 0x0600515B RID: 20827 RVA: 0x003F13D4 File Offset: 0x003EF5D4
		// (set) Token: 0x0600515C RID: 20828 RVA: 0x003F13DC File Offset: 0x003EF5DC
		public byte[] Mask
		{
			[CompilerGenerated]
			get
			{
				return this.<Mask>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<Mask>k__BackingField = value;
			}
		}

		// Token: 0x170017A9 RID: 6057
		// (get) Token: 0x0600515D RID: 20829 RVA: 0x003F13E5 File Offset: 0x003EF5E5
		// (set) Token: 0x0600515E RID: 20830 RVA: 0x003F13ED File Offset: 0x003EF5ED
		public ToyotaA8Item.ItemTypes Type
		{
			[CompilerGenerated]
			get
			{
				return this.<Type>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<Type>k__BackingField = value;
			}
		}

		// Token: 0x0600515F RID: 20831 RVA: 0x003F13F8 File Offset: 0x003EF5F8
		public ToyotaA8Item(string a8command, string canID, string extAddress, byte[] data)
		{
			try
			{
				if (a8command == "A803")
				{
					this.Type = ToyotaA8Item.ItemTypes.WriteA803;
				}
				else
				{
					this.Type = ToyotaA8Item.ItemTypes.ReadA801;
				}
				this.CanIdHex = canID;
				this.ExtendedAddress = extAddress;
				this.Id = data[0];
				this.Length = (int)data[1];
				this.Mask = new byte[this.Length];
				Array.Copy(data, 2, this.Mask, 0, this.Mask.Length);
				this.MaskHex = BitHelpers.ByteArrayToHexString(this.Mask);
				this.IdHex = this.Id.ToString("X2");
			}
			catch (Exception)
			{
				throw new ArgumentException("Wrong data for Toyota A8 item");
			}
		}

		// Token: 0x06005160 RID: 20832 RVA: 0x003F14D0 File Offset: 0x003EF6D0
		public byte[] ProcessData(byte[] input)
		{
			List<byte> list = new List<byte>(this.Mask.Length);
			int num = 0;
			for (int i = 0; i < this.Mask.Length; i++)
			{
				if (this.Mask[i] == 0)
				{
					list.Add(0);
				}
				else
				{
					if (num < input.Length)
					{
						list.Add(input[num]);
					}
					else
					{
						list.Add(this.Mask[i]);
					}
					num++;
				}
			}
			return list.ToArray();
		}

		// Token: 0x06005161 RID: 20833 RVA: 0x003F153C File Offset: 0x003EF73C
		public bool CheckIfCodingIsSupported(CustomizableCodingTemplate coding)
		{
			bool flag;
			try
			{
				if (this.Mask.All((byte x) => x == 0))
				{
					flag = false;
				}
				else
				{
					byte[] maskBytesWithoutZero = this.Mask.Where((byte x) => x > 0).ToArray<byte>();
					if (coding.ValueType == AdaptationValueTypes.InputHexDataType)
					{
						flag = true;
					}
					else if (coding.ValueType == AdaptationValueTypes.ToyotaTPMSSensor)
					{
						flag = true;
					}
					else if (coding.ValueType == AdaptationValueTypes.TPMS)
					{
						flag = true;
					}
					else if (coding.ValueType == AdaptationValueTypes.InputValueType || coding.ValueType == AdaptationValueTypes.InputTextType)
					{
						if (coding.StartByteId < maskBytesWithoutZero.Length && coding.StartByteId + coding.DataLength <= maskBytesWithoutZero.Length)
						{
							flag = true;
						}
						else
						{
							flag = false;
						}
					}
					else if (coding.ValueType == AdaptationValueTypes.OptionType)
					{
						Func<KeyValuePair<int, int>, bool> <>9__3;
						flag = coding.Options.All(delegate(MQBAdaptationOption opt)
						{
							if (opt.Value.StartsWith("BIT:") || opt.Value.StartsWith("BITS:"))
							{
								IEnumerable<KeyValuePair<int, int>> bitsFromOptionBitValue = this.GetBitsFromOptionBitValue(opt.Value);
								Func<KeyValuePair<int, int>, bool> func;
								if ((func = <>9__3) == null)
								{
									func = (<>9__3 = (KeyValuePair<int, int> pair) => BitHelpers.GetBit_0_7(maskBytesWithoutZero[pair.Key], pair.Value));
								}
								return bitsFromOptionBitValue.All(func);
							}
							byte[] array = BitHelpers.ConvertHexToBytesX(opt.Value);
							byte[] array2 = new byte[array.Length];
							Array.Copy(maskBytesWithoutZero, coding.StartByteId, array2, 0, array2.Length);
							for (int i = 0; i < array2.Length; i++)
							{
								if (!this.ByteValidForMask((int)array[i], (int)array2[i]))
								{
									return false;
								}
							}
							return true;
						});
					}
					else
					{
						flag = false;
					}
				}
			}
			catch (Exception)
			{
				flag = false;
			}
			return flag;
		}

		// Token: 0x06005162 RID: 20834 RVA: 0x003F16B4 File Offset: 0x003EF8B4
		private bool ByteValidForMask(int b, int mask)
		{
			return (b & mask) == b;
		}

		// Token: 0x06005163 RID: 20835 RVA: 0x003F16C0 File Offset: 0x003EF8C0
		private IEnumerable<KeyValuePair<int, int>> GetBitsFromOptionBitValue(string value)
		{
			return value.Substring(value.IndexOf(':') + 1).Split(new char[] { ';', ',' }, StringSplitOptions.RemoveEmptyEntries).Select(delegate(string x)
			{
				string[] array = x.Split(new char[] { ':', '=' }, StringSplitOptions.RemoveEmptyEntries);
				int num = int.Parse(array[0], CultureInfo.InvariantCulture);
				int num2 = int.Parse(array[1], CultureInfo.InvariantCulture);
				return new KeyValuePair<int, int>(num, num2);
			});
		}

		// Token: 0x04003150 RID: 12624
		[CompilerGenerated]
		private string <CanIdHex>k__BackingField;

		// Token: 0x04003151 RID: 12625
		[CompilerGenerated]
		private string <ExtendedAddress>k__BackingField;

		// Token: 0x04003152 RID: 12626
		[CompilerGenerated]
		private string <IdHex>k__BackingField;

		// Token: 0x04003153 RID: 12627
		[CompilerGenerated]
		private byte <Id>k__BackingField;

		// Token: 0x04003154 RID: 12628
		[CompilerGenerated]
		private int <Length>k__BackingField;

		// Token: 0x04003155 RID: 12629
		[CompilerGenerated]
		private string <MaskHex>k__BackingField;

		// Token: 0x04003156 RID: 12630
		[CompilerGenerated]
		private byte[] <Mask>k__BackingField;

		// Token: 0x04003157 RID: 12631
		[CompilerGenerated]
		private ToyotaA8Item.ItemTypes <Type>k__BackingField;

		// Token: 0x020009DA RID: 2522
		public enum ItemTypes
		{
			// Token: 0x04003159 RID: 12633
			ReadA801,
			// Token: 0x0400315A RID: 12634
			WriteA803
		}

		// Token: 0x020009DB RID: 2523
		[CompilerGenerated]
		[Serializable]
		private sealed class <>c
		{
			// Token: 0x06005164 RID: 20836 RVA: 0x003F1717 File Offset: 0x003EF917
			// Note: this type is marked as 'beforefieldinit'.
			static <>c()
			{
			}

			// Token: 0x06005165 RID: 20837 RVA: 0x00002050 File Offset: 0x00000250
			public <>c()
			{
			}

			// Token: 0x06005166 RID: 20838 RVA: 0x0037EDD4 File Offset: 0x0037CFD4
			internal bool <CheckIfCodingIsSupported>b__35_0(byte x)
			{
				return x == 0;
			}

			// Token: 0x06005167 RID: 20839 RVA: 0x003F1723 File Offset: 0x003EF923
			internal bool <CheckIfCodingIsSupported>b__35_1(byte x)
			{
				return x > 0;
			}

			// Token: 0x06005168 RID: 20840 RVA: 0x003F172C File Offset: 0x003EF92C
			internal KeyValuePair<int, int> <GetBitsFromOptionBitValue>b__37_0(string x)
			{
				string[] array = x.Split(new char[] { ':', '=' }, StringSplitOptions.RemoveEmptyEntries);
				int num = int.Parse(array[0], CultureInfo.InvariantCulture);
				int num2 = int.Parse(array[1], CultureInfo.InvariantCulture);
				return new KeyValuePair<int, int>(num, num2);
			}

			// Token: 0x0400315B RID: 12635
			public static readonly ToyotaA8Item.<>c <>9 = new ToyotaA8Item.<>c();

			// Token: 0x0400315C RID: 12636
			public static Func<byte, bool> <>9__35_0;

			// Token: 0x0400315D RID: 12637
			public static Func<byte, bool> <>9__35_1;

			// Token: 0x0400315E RID: 12638
			public static Func<string, KeyValuePair<int, int>> <>9__37_0;
		}

		// Token: 0x020009DC RID: 2524
		[CompilerGenerated]
		private sealed class <>c__DisplayClass35_0
		{
			// Token: 0x06005169 RID: 20841 RVA: 0x00002050 File Offset: 0x00000250
			public <>c__DisplayClass35_0()
			{
			}

			// Token: 0x0600516A RID: 20842 RVA: 0x003F1774 File Offset: 0x003EF974
			internal bool <CheckIfCodingIsSupported>b__2(MQBAdaptationOption opt)
			{
				if (opt.Value.StartsWith("BIT:") || opt.Value.StartsWith("BITS:"))
				{
					IEnumerable<KeyValuePair<int, int>> bitsFromOptionBitValue = this.<>4__this.GetBitsFromOptionBitValue(opt.Value);
					Func<KeyValuePair<int, int>, bool> func;
					if ((func = this.<>9__3) == null)
					{
						func = (this.<>9__3 = (KeyValuePair<int, int> pair) => BitHelpers.GetBit_0_7(this.maskBytesWithoutZero[pair.Key], pair.Value));
					}
					return bitsFromOptionBitValue.All(func);
				}
				byte[] array = BitHelpers.ConvertHexToBytesX(opt.Value);
				byte[] array2 = new byte[array.Length];
				Array.Copy(this.maskBytesWithoutZero, this.coding.StartByteId, array2, 0, array2.Length);
				for (int i = 0; i < array2.Length; i++)
				{
					if (!this.<>4__this.ByteValidForMask((int)array[i], (int)array2[i]))
					{
						return false;
					}
				}
				return true;
			}

			// Token: 0x0600516B RID: 20843 RVA: 0x003F1834 File Offset: 0x003EFA34
			internal bool <CheckIfCodingIsSupported>b__3(KeyValuePair<int, int> pair)
			{
				return BitHelpers.GetBit_0_7(this.maskBytesWithoutZero[pair.Key], pair.Value);
			}

			// Token: 0x0400315F RID: 12639
			public ToyotaA8Item <>4__this;

			// Token: 0x04003160 RID: 12640
			public CustomizableCodingTemplate coding;

			// Token: 0x04003161 RID: 12641
			public byte[] maskBytesWithoutZero;

			// Token: 0x04003162 RID: 12642
			public Func<KeyValuePair<int, int>, bool> <>9__3;
		}
	}
}
