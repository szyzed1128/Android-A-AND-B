using System;
using System.Collections.Generic;
using System.Globalization;
using System.Runtime.CompilerServices;
using CarScannerXamarinForms.Common;

namespace CarScannerXamarinForms.Coding
{
	// Token: 0x020008E4 RID: 2276
	internal class MQB_LightFunction : ValueItemWithTranslation
	{
		// Token: 0x17001769 RID: 5993
		// (get) Token: 0x06004CFA RID: 19706 RVA: 0x0038B103 File Offset: 0x00389303
		public static List<MQB_LightFunction> LightFunctionsList
		{
			get
			{
				if (MQB_LightFunction._LightFunctionsList == null)
				{
					MQB_LightFunction._LightFunctionsList = PackageFileReader.DeserilzeFromEmbeddedFile<List<MQB_LightFunction>>("mqb_light_functions");
				}
				return MQB_LightFunction._LightFunctionsList;
			}
		}

		// Token: 0x06004CFB RID: 19707 RVA: 0x0038B120 File Offset: 0x00389320
		public static MQB_LightFunction FromValue(string hex)
		{
			return MQB_LightFunction.FromValue(byte.Parse(hex, NumberStyles.HexNumber));
		}

		// Token: 0x06004CFC RID: 19708 RVA: 0x0038B134 File Offset: 0x00389334
		public static MQB_LightFunction FromValue(byte data)
		{
			data = BitHelpers.SetBit(data, 7, false);
			string hex_val = data.ToString("X2");
			MQB_LightFunction mqb_LightFunction = MQB_LightFunction.LightFunctionsList.Find((MQB_LightFunction x) => x.Value == hex_val);
			if (mqb_LightFunction == null)
			{
				return new MQB_LightFunction
				{
					Title = Translate.GetString("coding_StateUnknown"),
					Value = data.ToString("X2")
				};
			}
			return mqb_LightFunction;
		}

		// Token: 0x06004CFD RID: 19709 RVA: 0x0038B1A6 File Offset: 0x003893A6
		public byte ApplyValueToByte(byte data)
		{
			return BitHelpers.SetBit(byte.Parse(base.Value, NumberStyles.HexNumber), 7, BitHelpers.GetBit_0_7(data, 7));
		}

		// Token: 0x06004CFE RID: 19710 RVA: 0x0038B1C5 File Offset: 0x003893C5
		public MQB_LightFunction()
		{
		}

		// Token: 0x04002D73 RID: 11635
		private static List<MQB_LightFunction> _LightFunctionsList;

		// Token: 0x020008E5 RID: 2277
		[CompilerGenerated]
		private sealed class <>c__DisplayClass4_0
		{
			// Token: 0x06004CFF RID: 19711 RVA: 0x00002050 File Offset: 0x00000250
			public <>c__DisplayClass4_0()
			{
			}

			// Token: 0x06004D00 RID: 19712 RVA: 0x0038B1CD File Offset: 0x003893CD
			internal bool <FromValue>b__0(MQB_LightFunction x)
			{
				return x.Value == this.hex_val;
			}

			// Token: 0x04002D74 RID: 11636
			public string hex_val;
		}
	}
}
