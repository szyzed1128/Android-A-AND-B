using System;
using System.ComponentModel;

namespace CarScannerXamarinForms.Coding.DB.HyundaiKia
{
	// Token: 0x02000B92 RID: 2962
	internal class ElectricParkingBrakeService_SportageQL : CustomizableCodingTemplate, IServiceProcedure, ICodingContainer, INotifyPropertyChanged
	{
		// Token: 0x06005A7D RID: 23165 RVA: 0x00433254 File Offset: 0x00431454
		public ElectricParkingBrakeService_SportageQL()
		{
			base.Name = "Electric parking brake service (Kia Sportage QL)";
			base.Description = "Compatibility: Kia Sportage QL";
			base.InnerDescription = "Experimental feature, not confirmed" + SupportedItemsDetectorBase.GetAccessKeyWarning;
			base.RequestHeader = "7D1";
			base.ResponseHeader = "7D9";
			this.ReadModeAndAddress = "";
			base.WriteModeAndAddress = "2FF0";
			this.Group = CodingGroup.Brakes;
			base.PreReadCommands = "";
			base.PreWriteCommands = "20;1003";
			this.ValueType = AdaptationValueTypes.OptionType;
			this.Options.Add(new MQBAdaptationOption("Open (Sportage QL)", "4103"));
			this.Options.Add(new MQBAdaptationOption("Close (Sportage QL)", "4203"));
			this.PasswordVisible = false;
			base.MakeChangesToInitialData = false;
		}
	}
}
