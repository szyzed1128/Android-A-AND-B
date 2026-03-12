using System;
using System.ComponentModel;

namespace CarScannerXamarinForms.Coding.DB.HyundaiKia
{
	// Token: 0x02000B91 RID: 2961
	internal class ElectricParkingBrakeService_OptimaTF : CustomizableCodingTemplate, IServiceProcedure, ICodingContainer, INotifyPropertyChanged
	{
		// Token: 0x06005A7C RID: 23164 RVA: 0x00433184 File Offset: 0x00431384
		public ElectricParkingBrakeService_OptimaTF()
		{
			base.Name = "Electric parking brake service (Kia Optima TF)";
			base.Description = "Compatibility: Kia Optima TF";
			base.InnerDescription = "Experimental feature, not confirmed" + SupportedItemsDetectorBase.GetAccessKeyWarning;
			base.RequestHeader = "7D5";
			base.ResponseHeader = "7DD";
			this.ReadModeAndAddress = "";
			base.WriteModeAndAddress = "30";
			this.Group = CodingGroup.Brakes;
			base.PreReadCommands = "";
			base.PreWriteCommands = "20;1090";
			this.ValueType = AdaptationValueTypes.OptionType;
			this.Options.Add(new MQBAdaptationOption("Open", "0107"));
			this.Options.Add(new MQBAdaptationOption("Close", "0207"));
			this.PasswordVisible = false;
			base.MakeChangesToInitialData = false;
		}
	}
}
