using System;
using System.Runtime.CompilerServices;
using CarScannerXamarinForms.Common;

namespace CarScannerXamarinForms.Coding.DB.PQ26_NewRapid2020
{
	// Token: 0x02000A27 RID: 2599
	internal static class Dashboard
	{
		// Token: 0x060052A1 RID: 21153 RVA: 0x003F9E84 File Offset: 0x003F8084
		public static ICodingContainer DigitalDashboardConfigurationInMMI()
		{
			MQBEasyCodingItem mqbeasyCodingItem = new MQBEasyCodingItem("0B1D", "5F", "20103", "", delegate(byte[] data, string value, MQBEasyCodingItem codingItem)
			{
				byte[] array = new byte[data.Length];
				Array.Copy(data, 0, array, 0, data.Length);
				if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					BitHelpers.SwitchBitInByte(array, 69, 6, true);
					BitHelpers.SwitchBitInByte(array, 69, 5, true);
					BitHelpers.SwitchBitInByte(array, 69, 1, true);
					BitHelpers.SwitchBitInByte(array, 69, 0, true);
				}
				else
				{
					BitHelpers.SwitchBitInByte(array, 69, 6, false);
					BitHelpers.SwitchBitInByte(array, 69, 5, false);
					BitHelpers.SwitchBitInByte(array, 69, 1, false);
					BitHelpers.SwitchBitInByte(array, 69, 0, false);
				}
				return array;
			}, null);
			MQBEasyCodingItem mqbeasyCodingItem2 = new MQBEasyCodingItem("0B1B", "5F", "20103", "", delegate(byte[] data, string value, MQBEasyCodingItem codingItem)
			{
				byte[] array2 = new byte[data.Length];
				Array.Copy(data, 0, array2, 0, data.Length);
				if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					BitHelpers.SwitchBitInByte(array2, 56, 7, true);
					BitHelpers.SwitchBitInByte(array2, 56, 5, true);
				}
				else
				{
					BitHelpers.SwitchBitInByte(array2, 56, 7, false);
					BitHelpers.SwitchBitInByte(array2, 56, 5, false);
				}
				return array2;
			}, null);
			mqbeasyCodingItem2.PostWriteCommands = "1102";
			return new MQBMultipleCoding(CodingGroup.Dashboard, Translate.GetString("codingDB_DigitalLcdDashboardConfigurationMenuInMultimediaSystem_Name"), "", false, new ICodingContainer[] { mqbeasyCodingItem, mqbeasyCodingItem2 })
			{
				InnerDescription = Translate.GetString("codingDB_DigitalLcdDashboardConfigurationMenuInMultimediaSystem_InnerDescription"),
				RequiresPro = false
			};
		}

		// Token: 0x02000A28 RID: 2600
		[CompilerGenerated]
		[Serializable]
		private sealed class <>c
		{
			// Token: 0x060052A2 RID: 21154 RVA: 0x003F9F4B File Offset: 0x003F814B
			// Note: this type is marked as 'beforefieldinit'.
			static <>c()
			{
			}

			// Token: 0x060052A3 RID: 21155 RVA: 0x00002050 File Offset: 0x00000250
			public <>c()
			{
			}

			// Token: 0x060052A4 RID: 21156 RVA: 0x003F9F58 File Offset: 0x003F8158
			internal byte[] <DigitalDashboardConfigurationInMMI>b__0_0(byte[] data, string value, MQBEasyCodingItem codingItem)
			{
				byte[] array = new byte[data.Length];
				Array.Copy(data, 0, array, 0, data.Length);
				if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					BitHelpers.SwitchBitInByte(array, 69, 6, true);
					BitHelpers.SwitchBitInByte(array, 69, 5, true);
					BitHelpers.SwitchBitInByte(array, 69, 1, true);
					BitHelpers.SwitchBitInByte(array, 69, 0, true);
				}
				else
				{
					BitHelpers.SwitchBitInByte(array, 69, 6, false);
					BitHelpers.SwitchBitInByte(array, 69, 5, false);
					BitHelpers.SwitchBitInByte(array, 69, 1, false);
					BitHelpers.SwitchBitInByte(array, 69, 0, false);
				}
				return array;
			}

			// Token: 0x060052A5 RID: 21157 RVA: 0x003F9FE0 File Offset: 0x003F81E0
			internal byte[] <DigitalDashboardConfigurationInMMI>b__0_1(byte[] data, string value, MQBEasyCodingItem codingItem)
			{
				byte[] array = new byte[data.Length];
				Array.Copy(data, 0, array, 0, data.Length);
				if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					BitHelpers.SwitchBitInByte(array, 56, 7, true);
					BitHelpers.SwitchBitInByte(array, 56, 5, true);
				}
				else
				{
					BitHelpers.SwitchBitInByte(array, 56, 7, false);
					BitHelpers.SwitchBitInByte(array, 56, 5, false);
				}
				return array;
			}

			// Token: 0x04003281 RID: 12929
			public static readonly Dashboard.<>c <>9 = new Dashboard.<>c();

			// Token: 0x04003282 RID: 12930
			public static Func<byte[], string, MQBEasyCodingItem, byte[]> <>9__0_0;

			// Token: 0x04003283 RID: 12931
			public static Func<byte[], string, MQBEasyCodingItem, byte[]> <>9__0_1;
		}
	}
}
