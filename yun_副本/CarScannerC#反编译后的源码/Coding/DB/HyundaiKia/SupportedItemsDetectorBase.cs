using System;
using System.Collections.Generic;
using CarScannerXamarinForms.Common;
using CarScannerXamarinForms.OBD2;

namespace CarScannerXamarinForms.Coding.DB.HyundaiKia
{
	// Token: 0x02000BD4 RID: 3028
	internal class SupportedItemsDetectorBase
	{
		// Token: 0x17001851 RID: 6225
		// (get) Token: 0x06005B36 RID: 23350 RVA: 0x00437BDE File Offset: 0x00435DDE
		internal static string GetAccessKeyWarning
		{
			get
			{
				return "\nAround 2018, Hyundai/Kia began to introduce protection into some models of their cars: the car began to require a special access key to perform service functions. If your vehicle requires such an access key, this feature will not work.";
			}
		}

		// Token: 0x17001852 RID: 6226
		// (get) Token: 0x06005B37 RID: 23351 RVA: 0x00437BE5 File Offset: 0x00435DE5
		internal static string GetAccessKeyWarningRu
		{
			get
			{
				return "\nПримерно с 2018 года Hyundai/Kia стали внедрять защиту в некоторые модели своих автомобилей: для выполнения сервисных функций автомобиль стал требовать специальный ключ доступа. Если на вашем автомобиле требуется такой ключ доступа, то эта функция работать не будет.";
			}
		}

		// Token: 0x06005B38 RID: 23352 RVA: 0x00437BEC File Offset: 0x00435DEC
		public static List<int> GetSupportedIds(byte[] data)
		{
			List<int> list = new List<int>();
			for (int i = 0; i < data.Length; i++)
			{
				byte b = data[i];
				for (int j = 0; j <= 7; j++)
				{
					if (BitHelpers.GetBit_0_7(b, j))
					{
						int num = i * 8 + j + 1;
						list.Add(num);
					}
				}
			}
			return list;
		}

		// Token: 0x06005B39 RID: 23353 RVA: 0x00437C38 File Offset: 0x00435E38
		public static CustomizableCodingTemplate CreateActuatorTest(int id, string title, string header, string startPattern, string stopPattern, string writeMode, string openSession, CodingGroup group)
		{
			CustomizableCodingTemplate customizableCodingTemplate = new CustomizableCodingTemplate();
			customizableCodingTemplate.Name = title;
			customizableCodingTemplate.RequestHeader = header;
			customizableCodingTemplate.ResponseHeader = CAN11bitHelper.GetPossibleResponseHeader(header, "Kia", null);
			customizableCodingTemplate.PasswordVisible = false;
			customizableCodingTemplate.ReadModeAndAddress = "";
			customizableCodingTemplate.HasCurrentState = false;
			customizableCodingTemplate.WriteModeAndAddress = writeMode;
			customizableCodingTemplate.MakeChangesToInitialData = false;
			customizableCodingTemplate.ValueType = AdaptationValueTypes.OptionType;
			customizableCodingTemplate.Protocol = "";
			customizableCodingTemplate.OpenSessionCommand = openSession;
			customizableCodingTemplate.Group = group;
			MQBAdaptationOption mqbadaptationOption = new MQBAdaptationOption(Translate.GetString("coding_Start"), string.Format(startPattern, id.ToString("X2")));
			MQBAdaptationOption mqbadaptationOption2 = new MQBAdaptationOption(Translate.GetString("coding_Stop"), string.Format(stopPattern, id.ToString("X2")));
			customizableCodingTemplate.Options.Add(mqbadaptationOption);
			customizableCodingTemplate.Options.Add(mqbadaptationOption2);
			return customizableCodingTemplate;
		}

		// Token: 0x06005B3A RID: 23354 RVA: 0x00002050 File Offset: 0x00000250
		public SupportedItemsDetectorBase()
		{
		}
	}
}
