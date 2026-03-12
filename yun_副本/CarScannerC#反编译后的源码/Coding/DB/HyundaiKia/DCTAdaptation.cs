using System;

namespace CarScannerXamarinForms.Coding.DB.HyundaiKia
{
	// Token: 0x02000B8E RID: 2958
	internal static class DCTAdaptation
	{
		// Token: 0x06005A79 RID: 23161 RVA: 0x00432DA0 File Offset: 0x00430FA0
		public static CustomizableCodingTemplate CreateDTCAdaptation()
		{
			CustomizableCodingTemplate customizableCodingTemplate = new CustomizableCodingTemplate();
			customizableCodingTemplate.RequestHeader = "7E1";
			customizableCodingTemplate.ResponseHeader = "7E9";
			customizableCodingTemplate.OpenSessionCommand = "1003";
			customizableCodingTemplate.Name = "DCT7 transmission adaptation";
			customizableCodingTemplate.Description = "WARNING! Experimental option! Use at your own risk!\nIf you have faulty gearbox - this wouldn't help!";
			customizableCodingTemplate.InnerDescription = "Procedure should be done in 2 steps:\nStep 1: Ignition: ON, engine: NOT started, Selector position: P. Duration: ~30 seconds. Launch and wait for 30-60 seconds, go to step #2.\nStep 2: Ignition: ON, engine: RUNNING AT IDLE, Selector position: P. Duration: ~60 seconds. After step 1 completion, launch the engine and start step #2. Wait until finished.\nAfter both steps finished: turn off ignition and wait for 30 seconds before starting the engine." + SupportedItemsDetectorBase.GetAccessKeyWarning;
			customizableCodingTemplate.PasswordVisible = false;
			customizableCodingTemplate.MakeChangesToInitialData = false;
			customizableCodingTemplate.ValueType = AdaptationValueTypes.OptionType;
			customizableCodingTemplate.ReadModeAndAddress = "";
			customizableCodingTemplate.WriteModeAndAddress = "31";
			customizableCodingTemplate.HasCurrentState = false;
			MQBAdaptationOption mqbadaptationOption = new MQBAdaptationOption("Step 1: Ignition ON, engine OFF", "01F00301");
			MQBAdaptationOption mqbadaptationOption2 = new MQBAdaptationOption("Step 2: Ignition ON, engine ON", "01F00302");
			customizableCodingTemplate.Options.Add(mqbadaptationOption);
			customizableCodingTemplate.Options.Add(mqbadaptationOption2);
			return customizableCodingTemplate;
		}
	}
}
