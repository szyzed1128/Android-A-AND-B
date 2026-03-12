using System;
using System.Runtime.CompilerServices;

namespace CarScannerXamarinForms.Coding.DB.MQB
{
	// Token: 0x02000B49 RID: 2889
	internal static class Service
	{
		// Token: 0x0600598A RID: 22922 RVA: 0x0042B3BC File Offset: 0x004295BC
		public static ICodingContainer ResetOilService()
		{
			MQBEasyCodingItem mqbeasyCodingItem = new MQBEasyCodingItem("2243", "17", "", "", delegate(byte[] data, string value, MQBEasyCodingItem coding1)
			{
				byte[] array = new byte[data.Length];
				Array.Copy(data, 0, array, 0, array.Length);
				if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					for (int i = 0; i < array.Length; i++)
					{
						array[i] = 0;
					}
				}
				return array;
			}, null);
			MQBEasyCodingItem mqbeasyCodingItem2 = new MQBEasyCodingItem("2244", "17", "", "", delegate(byte[] data, string value, MQBEasyCodingItem coding1)
			{
				byte[] array2 = new byte[data.Length];
				Array.Copy(data, 0, array2, 0, array2.Length);
				if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					for (int j = 0; j < array2.Length; j++)
					{
						array2[j] = 0;
					}
				}
				return array2;
			}, null);
			MQBEasyCodingItem mqbeasyCodingItem3 = new MQBEasyCodingItem("2232", "17", "", "", delegate(byte[] data, string value, MQBEasyCodingItem coding1)
			{
				byte[] array3 = new byte[data.Length];
				Array.Copy(data, 0, array3, 0, array3.Length);
				if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					array3[0] = 0;
				}
				return array3;
			}, null);
			MQBMultipleCoding mqbmultipleCoding = new MQBMultipleCoding(CodingGroup.ServiceReminderReset, Translate.GetString("codingDB_ResetOilServiceReminder_Name"), Translate.GetString("codingDB_ResetOilServiceReminder_Description"), true, new ICodingContainer[] { mqbeasyCodingItem, mqbeasyCodingItem2, mqbeasyCodingItem3 });
			mqbmultipleCoding.PasswordVisible = true;
			mqbmultipleCoding.PasswordHint = Translate.GetString("coding_PasswordNotRequired") + "\nMQB: 20103, PQ26: 25327, 20103";
			mqbmultipleCoding.Options.Clear();
			mqbmultipleCoding.Options.Add(new MQBAdaptationOption(Translate.GetString("coding_Start"), MQBAdaptationTemplate.EnableOption.Value));
			mqbmultipleCoding.HasCurrentState = false;
			return mqbmultipleCoding;
		}

		// Token: 0x0600598B RID: 22923 RVA: 0x0042B4FC File Offset: 0x004296FC
		public static ICodingContainer ResetInspectionService()
		{
			MQBEasyCodingItem mqbeasyCodingItem = new MQBEasyCodingItem("22A6", "17", "", "", delegate(byte[] data, string value, MQBEasyCodingItem coding1)
			{
				byte[] array = new byte[data.Length];
				Array.Copy(data, 0, array, 0, array.Length);
				if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					array[0] = 0;
					array[1] = 0;
				}
				return array;
			}, null);
			MQBEasyCodingItem mqbeasyCodingItem2 = new MQBEasyCodingItem("22A7", "17", "", "", delegate(byte[] data, string value, MQBEasyCodingItem coding1)
			{
				byte[] array2 = new byte[data.Length];
				Array.Copy(data, 0, array2, 0, array2.Length);
				if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					array2[0] = 0;
					array2[1] = 0;
				}
				return array2;
			}, null);
			MQBMultipleCoding mqbmultipleCoding = new MQBMultipleCoding(CodingGroup.ServiceReminderReset, Translate.GetString("codingDB_ResetInspectionServiceReminder_Name"), Translate.GetString("codingDB_ResetInspectionServiceReminder_Description"), true, new ICodingContainer[] { mqbeasyCodingItem, mqbeasyCodingItem2 });
			mqbmultipleCoding.PasswordVisible = true;
			mqbmultipleCoding.PasswordHint = Translate.GetString("coding_PasswordNotRequired") + "\nMQB: 20103, PQ26: 25327, 20103";
			mqbmultipleCoding.Options.Clear();
			mqbmultipleCoding.Options.Add(new MQBAdaptationOption(Translate.GetString("coding_Start"), MQBAdaptationTemplate.EnableOption.Value));
			mqbmultipleCoding.HasCurrentState = false;
			return mqbmultipleCoding;
		}

		// Token: 0x02000B4A RID: 2890
		[CompilerGenerated]
		[Serializable]
		private sealed class <>c
		{
			// Token: 0x0600598C RID: 22924 RVA: 0x0042B5FE File Offset: 0x004297FE
			// Note: this type is marked as 'beforefieldinit'.
			static <>c()
			{
			}

			// Token: 0x0600598D RID: 22925 RVA: 0x00002050 File Offset: 0x00000250
			public <>c()
			{
			}

			// Token: 0x0600598E RID: 22926 RVA: 0x0042B60C File Offset: 0x0042980C
			internal byte[] <ResetOilService>b__0_0(byte[] data, string value, MQBEasyCodingItem coding1)
			{
				byte[] array = new byte[data.Length];
				Array.Copy(data, 0, array, 0, array.Length);
				if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					for (int i = 0; i < array.Length; i++)
					{
						array[i] = 0;
					}
				}
				return array;
			}

			// Token: 0x0600598F RID: 22927 RVA: 0x0042B654 File Offset: 0x00429854
			internal byte[] <ResetOilService>b__0_1(byte[] data, string value, MQBEasyCodingItem coding1)
			{
				byte[] array = new byte[data.Length];
				Array.Copy(data, 0, array, 0, array.Length);
				if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					for (int i = 0; i < array.Length; i++)
					{
						array[i] = 0;
					}
				}
				return array;
			}

			// Token: 0x06005990 RID: 22928 RVA: 0x0042B69C File Offset: 0x0042989C
			internal byte[] <ResetOilService>b__0_2(byte[] data, string value, MQBEasyCodingItem coding1)
			{
				byte[] array = new byte[data.Length];
				Array.Copy(data, 0, array, 0, array.Length);
				if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					array[0] = 0;
				}
				return array;
			}

			// Token: 0x06005991 RID: 22929 RVA: 0x0042B6D8 File Offset: 0x004298D8
			internal byte[] <ResetInspectionService>b__1_0(byte[] data, string value, MQBEasyCodingItem coding1)
			{
				byte[] array = new byte[data.Length];
				Array.Copy(data, 0, array, 0, array.Length);
				if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					array[0] = 0;
					array[1] = 0;
				}
				return array;
			}

			// Token: 0x06005992 RID: 22930 RVA: 0x0042B718 File Offset: 0x00429918
			internal byte[] <ResetInspectionService>b__1_1(byte[] data, string value, MQBEasyCodingItem coding1)
			{
				byte[] array = new byte[data.Length];
				Array.Copy(data, 0, array, 0, array.Length);
				if (value == MQBAdaptationTemplate.EnableOption.Value)
				{
					array[0] = 0;
					array[1] = 0;
				}
				return array;
			}

			// Token: 0x040037EC RID: 14316
			public static readonly Service.<>c <>9 = new Service.<>c();

			// Token: 0x040037ED RID: 14317
			public static Func<byte[], string, MQBEasyCodingItem, byte[]> <>9__0_0;

			// Token: 0x040037EE RID: 14318
			public static Func<byte[], string, MQBEasyCodingItem, byte[]> <>9__0_1;

			// Token: 0x040037EF RID: 14319
			public static Func<byte[], string, MQBEasyCodingItem, byte[]> <>9__0_2;

			// Token: 0x040037F0 RID: 14320
			public static Func<byte[], string, MQBEasyCodingItem, byte[]> <>9__1_0;

			// Token: 0x040037F1 RID: 14321
			public static Func<byte[], string, MQBEasyCodingItem, byte[]> <>9__1_1;
		}
	}
}
