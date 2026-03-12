using System;
using System.Runtime.CompilerServices;

namespace CarScannerXamarinForms.Coding.DB.MLB
{
	// Token: 0x02000B84 RID: 2948
	internal static class TouaregNF
	{
		// Token: 0x06005A62 RID: 23138 RVA: 0x00431FEC File Offset: 0x004301EC
		public static ICodingContainer TouaregNF_BrakeDiskDrying()
		{
			return new MQBEasyCodingItem(CodingGroup.Brakes, Translate.GetString("codingDB_BrakeDiskDrying_Name"), Translate.GetString("codingDB_BrakeDiskDrying_Description"), "0603", "713", "77D", "", "", delegate(byte[] data, string value, MQBEasyCodingItem codingItem)
			{
				byte[] array = new byte[data.Length];
				Array.Copy(data, 0, array, 0, data.Length);
				if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					array[0] = 1;
				}
				else
				{
					array[0] = 0;
				}
				return array;
			}, null, new MQBAdaptationOption[]
			{
				MQBAdaptationTemplate.EnableOption,
				MQBAdaptationTemplate.DisableOption
			});
		}

		// Token: 0x02000B85 RID: 2949
		[CompilerGenerated]
		[Serializable]
		private sealed class <>c
		{
			// Token: 0x06005A63 RID: 23139 RVA: 0x00432062 File Offset: 0x00430262
			// Note: this type is marked as 'beforefieldinit'.
			static <>c()
			{
			}

			// Token: 0x06005A64 RID: 23140 RVA: 0x00002050 File Offset: 0x00000250
			public <>c()
			{
			}

			// Token: 0x06005A65 RID: 23141 RVA: 0x00432070 File Offset: 0x00430270
			internal byte[] <TouaregNF_BrakeDiskDrying>b__0_0(byte[] data, string value, MQBEasyCodingItem codingItem)
			{
				byte[] array = new byte[data.Length];
				Array.Copy(data, 0, array, 0, data.Length);
				if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					array[0] = 1;
				}
				else
				{
					array[0] = 0;
				}
				return array;
			}

			// Token: 0x040038BF RID: 14527
			public static readonly TouaregNF.<>c <>9 = new TouaregNF.<>c();

			// Token: 0x040038C0 RID: 14528
			public static Func<byte[], string, MQBEasyCodingItem, byte[]> <>9__0_0;
		}
	}
}
