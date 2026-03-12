using System;
using System.ComponentModel;

namespace CarScannerXamarinForms.Coding.DB.HyundaiKia
{
	// Token: 0x02000B90 RID: 2960
	internal class ElectricParkingBrakeService_OptimaJF : CustomizableCodingTemplate, IServiceProcedure, ICodingContainer, INotifyPropertyChanged
	{
		// Token: 0x06005A7B RID: 23163 RVA: 0x00432F30 File Offset: 0x00431130
		public ElectricParkingBrakeService_OptimaJF()
		{
			base.Name = "Electric parking brake service (Kia Optima JF)";
			base.Description = "Compatibility: Kia Optima JF";
			base.InnerDescription = "Experimental feature, not confirmed" + SupportedItemsDetectorBase.GetAccessKeyWarning;
			base.RequestHeader = "7D5";
			base.ResponseHeader = "7DD";
			this.ReadModeAndAddress = "";
			base.WriteModeAndAddress = "2FF0";
			base.OpenSessionCommand = "1003";
			this.Group = CodingGroup.Brakes;
			base.PreReadCommands = "";
			base.PreWriteCommands = "20;1003;2FF01002";
			base.PostWriteCommands = "";
			this.ValueType = AdaptationValueTypes.OptionType;
			this.Options.Add(new MQBAdaptationOption("Start open right", "1103"));
			this.Options.Add(new MQBAdaptationOption("Stop open right", "1100"));
			this.Options.Add(new MQBAdaptationOption("Start close right", "1203"));
			this.Options.Add(new MQBAdaptationOption("Stop close right", "1200"));
			this.Options.Add(new MQBAdaptationOption("Start open left", "1303"));
			this.Options.Add(new MQBAdaptationOption("Stop open left", "1300"));
			this.Options.Add(new MQBAdaptationOption("Start close left", "1403"));
			this.Options.Add(new MQBAdaptationOption("Stop close left", "1400"));
			this.Options.Add(new MQBAdaptationOption("Start open (service mode)", "1503"));
			this.Options.Add(new MQBAdaptationOption("Stop open (service mode)", "1500"));
			this.Options.Add(new MQBAdaptationOption("Start close (service mode)", "1603"));
			this.Options.Add(new MQBAdaptationOption("Stop close (service mode)", "1600"));
			this.Options.Add(new MQBAdaptationOption("Start actuator control - effect", "1703"));
			this.Options.Add(new MQBAdaptationOption("Stop actuator control - effect", "1700"));
			this.Options.Add(new MQBAdaptationOption("Start actuator control - lift", "1803"));
			this.Options.Add(new MQBAdaptationOption("Stop actuator control - lift", "1800"));
			this.PasswordVisible = false;
			base.MakeChangesToInitialData = false;
		}
	}
}
