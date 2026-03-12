using System;
using System.Runtime.CompilerServices;

namespace CarScannerXamarinForms.Coding.DB.MLB_A4B9
{
	// Token: 0x02000B7D RID: 2941
	internal static class Dashboard
	{
		// Token: 0x06005A53 RID: 23123 RVA: 0x0043199C File Offset: 0x0042FB9C
		public static ICodingContainer WheelCircumference()
		{
			MQBEasyCodingItem mqbeasyCodingItem = new MQBEasyCodingItem("0600", "17", "", "", delegate(byte[] data, string value, MQBEasyCodingItem coding1)
			{
				byte[] array = new byte[data.Length];
				Array.Copy(data, 0, array, 0, data.Length);
				int num = (int)data[3];
				num &= 224;
				num += int.Parse(value);
				array[3] = (byte)num;
				return array;
			}, null);
			mqbeasyCodingItem.Options.Clear();
			for (int i = 0; i <= 31; i++)
			{
				MQBAdaptationOption mqbadaptationOption = new MQBAdaptationOption((i + 1).ToString(), i.ToString());
				mqbeasyCodingItem.Options.Add(mqbadaptationOption);
			}
			return mqbeasyCodingItem;
		}

		// Token: 0x02000B7E RID: 2942
		[CompilerGenerated]
		[Serializable]
		private sealed class <>c
		{
			// Token: 0x06005A54 RID: 23124 RVA: 0x00431A20 File Offset: 0x0042FC20
			// Note: this type is marked as 'beforefieldinit'.
			static <>c()
			{
			}

			// Token: 0x06005A55 RID: 23125 RVA: 0x00002050 File Offset: 0x00000250
			public <>c()
			{
			}

			// Token: 0x06005A56 RID: 23126 RVA: 0x00431A2C File Offset: 0x0042FC2C
			internal byte[] <WheelCircumference>b__0_0(byte[] data, string value, MQBEasyCodingItem coding1)
			{
				byte[] array = new byte[data.Length];
				Array.Copy(data, 0, array, 0, data.Length);
				int num = (int)data[3];
				num &= 224;
				num += int.Parse(value);
				array[3] = (byte)num;
				return array;
			}

			// Token: 0x040038B8 RID: 14520
			public static readonly Dashboard.<>c <>9 = new Dashboard.<>c();

			// Token: 0x040038B9 RID: 14521
			public static Func<byte[], string, MQBEasyCodingItem, byte[]> <>9__0_0;
		}
	}
}
