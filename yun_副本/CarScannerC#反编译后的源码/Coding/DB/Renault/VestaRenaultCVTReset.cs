using System;
using System.Globalization;
using CarScannerXamarinForms.Common;

namespace CarScannerXamarinForms.Coding.DB.Renault
{
	// Token: 0x02000A19 RID: 2585
	internal static class VestaRenaultCVTReset
	{
		// Token: 0x06005261 RID: 21089 RVA: 0x003F8924 File Offset: 0x003F6B24
		public static ICodingContainer VestaRenaultCVTResetProcedure()
		{
			return new CustomizableCodingTemplate
			{
				Group = CodingGroup.ServiceProcedures,
				Name = "Jatco CVT: Сброс счетчика старения масла вариатора",
				Description = "Jatco JF015E CVT",
				RequestHeader = "7E1",
				ResponseHeader = "7E9",
				PasswordVisible = false,
				ReadModeAndAddress = "",
				WriteModeAndAddress = "3B",
				ValueType = AdaptationValueTypes.OptionType,
				OpenSessionCommand = "10C0",
				MakeChangesToInitialData = false,
				Options = 
				{
					new MQBAdaptationOption("Сбросить", "0200000000")
				}
			};
		}

		// Token: 0x06005262 RID: 21090 RVA: 0x003F89BC File Offset: 0x003F6BBC
		public static ICodingContainer VestaRenaultCVTSetDateOfServiceProcedure()
		{
			CustomizableCodingTemplate customizableCodingTemplate = new CustomizableCodingTemplate();
			customizableCodingTemplate.Group = CodingGroup.ServiceProcedures;
			customizableCodingTemplate.Name = "Jatco CVT: Установить дату последнего обслуживания";
			customizableCodingTemplate.Description = "Jatco JF015E CVT";
			customizableCodingTemplate.RequestHeader = "7E1";
			customizableCodingTemplate.ResponseHeader = "7E9";
			customizableCodingTemplate.PasswordVisible = false;
			customizableCodingTemplate.ReadModeAndAddress = "";
			customizableCodingTemplate.WriteModeAndAddress = "3B";
			customizableCodingTemplate.ValueType = AdaptationValueTypes.OptionType;
			customizableCodingTemplate.OpenSessionCommand = "10C0";
			customizableCodingTemplate.MakeChangesToInitialData = false;
			DateTime nowSafe = DateTimeNowHelper.NowSafe;
			int num = int.Parse(nowSafe.Year.ToString().Substring(2), NumberStyles.None);
			customizableCodingTemplate.Options.Add(new MQBAdaptationOption("Сегодня", "B0" + nowSafe.Day.ToString("X2") + nowSafe.Month.ToString("X2") + num.ToString("X2")));
			return customizableCodingTemplate;
		}

		// Token: 0x06005263 RID: 21091 RVA: 0x003F8AB0 File Offset: 0x003F6CB0
		public static ICodingContainer VestaRenaultCVTResetECU()
		{
			CustomizableCodingTemplate customizableCodingTemplate = new CustomizableCodingTemplate();
			customizableCodingTemplate.Group = CodingGroup.ServiceProcedures;
			customizableCodingTemplate.Name = "Jatco CVT: Сброс ЭБУ";
			customizableCodingTemplate.Description = "Jatco JF015E CVT";
			customizableCodingTemplate.RequestHeader = "7E1";
			customizableCodingTemplate.ResponseHeader = "7E9";
			customizableCodingTemplate.PasswordVisible = false;
			customizableCodingTemplate.ReadModeAndAddress = "";
			customizableCodingTemplate.WriteModeAndAddress = "11";
			customizableCodingTemplate.ValueType = AdaptationValueTypes.OptionType;
			customizableCodingTemplate.OpenSessionCommand = "10C0";
			customizableCodingTemplate.MakeChangesToInitialData = false;
			DateTime nowSafe = DateTimeNowHelper.NowSafe;
			customizableCodingTemplate.Options.Add(new MQBAdaptationOption("Сброс", "FF"));
			return customizableCodingTemplate;
		}
	}
}
