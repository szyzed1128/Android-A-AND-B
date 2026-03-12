using System;

namespace CarScannerXamarinForms.Coding.DB.VWTP
{
	// Token: 0x020009CE RID: 2510
	internal static class TestCodings
	{
		// Token: 0x0600512C RID: 20780 RVA: 0x003EFC2C File Offset: 0x003EDE2C
		public static void CreateTestCoding()
		{
			CustomizableCodingTemplate customizableCodingTemplate = new CustomizableCodingTemplate
			{
				Name = "Test Long Coding 19",
				RequestHeader = "000",
				ResponseHeader = "",
				ReadModeAndAddress = "VWTP:01:1A9A",
				WriteModeAndAddress = "VWTP:1F:1A9A",
				ValueType = AdaptationValueTypes.InputHexDataType,
				OpenSessionCommand = "VWTP:01:1089",
				MakeChangesToInitialData = true
			};
			CustomCodingsListViewModel customCodingsListViewModel = new CustomCodingsListViewModel();
			customCodingsListViewModel.Load();
			customCodingsListViewModel.Clear();
			customCodingsListViewModel.Add(customizableCodingTemplate);
			customCodingsListViewModel.Save();
		}
	}
}
