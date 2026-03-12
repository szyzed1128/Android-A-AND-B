using System;
using System.Collections.Generic;
using System.Globalization;
using System.Runtime.CompilerServices;
using CarScannerXamarinForms.Common;

namespace CarScannerXamarinForms.Coding
{
	// Token: 0x020008E6 RID: 2278
	internal class MQB_LampType : ValueItemWithTranslation
	{
		// Token: 0x1700176A RID: 5994
		// (get) Token: 0x06004D01 RID: 19713 RVA: 0x0038B1E0 File Offset: 0x003893E0
		public static List<MQB_LampType> LampTypesList
		{
			get
			{
				if (MQB_LampType._LampTypesList == null)
				{
					MQB_LampType._LampTypesList = PackageFileReader.DeserilzeFromEmbeddedFile<List<MQB_LampType>>("mqb_light_types");
				}
				return MQB_LampType._LampTypesList;
			}
		}

		// Token: 0x06004D02 RID: 19714 RVA: 0x0038B1FD File Offset: 0x003893FD
		public static MQB_LampType FromValue(string hex)
		{
			return MQB_LampType.FromValue(byte.Parse(hex, NumberStyles.HexNumber));
		}

		// Token: 0x06004D03 RID: 19715 RVA: 0x0038B210 File Offset: 0x00389410
		public static MQB_LampType FromValue(byte data)
		{
			string hex_val = data.ToString("X2");
			MQB_LampType mqb_LampType = MQB_LampType.LampTypesList.Find((MQB_LampType x) => x.Value == hex_val);
			if (mqb_LampType == null)
			{
				return new MQB_LampType
				{
					Title = Translate.GetString("coding_StateUnknown"),
					Value = data.ToString("X2")
				};
			}
			return mqb_LampType;
		}

		// Token: 0x06004D04 RID: 19716 RVA: 0x0038B278 File Offset: 0x00389478
		public byte GetByteValue()
		{
			return byte.Parse(base.Value, NumberStyles.HexNumber);
		}

		// Token: 0x06004D05 RID: 19717 RVA: 0x0038B1C5 File Offset: 0x003893C5
		public MQB_LampType()
		{
		}

		// Token: 0x04002D75 RID: 11637
		private static List<MQB_LampType> _LampTypesList;

		// Token: 0x020008E7 RID: 2279
		[CompilerGenerated]
		private sealed class <>c__DisplayClass4_0
		{
			// Token: 0x06004D06 RID: 19718 RVA: 0x00002050 File Offset: 0x00000250
			public <>c__DisplayClass4_0()
			{
			}

			// Token: 0x06004D07 RID: 19719 RVA: 0x0038B28A File Offset: 0x0038948A
			internal bool <FromValue>b__0(MQB_LampType x)
			{
				return x.Value == this.hex_val;
			}

			// Token: 0x04002D76 RID: 11638
			public string hex_val;
		}
	}
}
