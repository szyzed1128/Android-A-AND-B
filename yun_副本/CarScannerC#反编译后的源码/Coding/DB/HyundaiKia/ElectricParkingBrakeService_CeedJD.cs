using System;
using System.ComponentModel;

namespace CarScannerXamarinForms.Coding.DB.HyundaiKia
{
	// Token: 0x02000B8F RID: 2959
	internal class ElectricParkingBrakeService_CeedJD : CustomizableCodingTemplate, IServiceProcedure, ICodingContainer, INotifyPropertyChanged
	{
		// Token: 0x06005A7A RID: 23162 RVA: 0x00432E68 File Offset: 0x00431068
		public ElectricParkingBrakeService_CeedJD()
		{
			base.Name = "Electric parking brake service (Kia Ceed JD)";
			base.Description = "Compatibility: Kia Ceed JD";
			base.InnerDescription = "Experimental feature, not confirmed";
			base.RequestHeader = "7D5";
			base.ResponseHeader = "7DD";
			this.ReadModeAndAddress = "";
			base.WriteModeAndAddress = "30";
			this.Group = CodingGroup.Brakes;
			base.PreReadCommands = "";
			base.PreWriteCommands = "20;1090";
			this.ValueType = AdaptationValueTypes.OptionType;
			this.Options.Add(new MQBAdaptationOption("Open", "0101"));
			this.Options.Add(new MQBAdaptationOption("Close", "0201"));
			this.PasswordVisible = false;
			base.MakeChangesToInitialData = false;
		}
	}
}
