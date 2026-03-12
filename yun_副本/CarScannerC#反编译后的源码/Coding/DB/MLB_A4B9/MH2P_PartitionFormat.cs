using System;

namespace CarScannerXamarinForms.Coding.DB.MLB_A4B9
{
	// Token: 0x02000B80 RID: 2944
	internal class MH2P_PartitionFormat : MQBServiceProcedure
	{
		// Token: 0x06005A57 RID: 23127 RVA: 0x00431A6C File Offset: 0x0042FC6C
		public MH2P_PartitionFormat()
		{
			base.Group = CodingGroup.Multimedia;
			base.Name = "Format MMI partitions (MIB2)";
			this.Unit = "5F";
			base.Options.Add(new MQBAdaptationOption("Navigation data", "3101025C040000"));
			base.Options.Add(new MQBAdaptationOption("Jukebox", "3101025C040001"));
			base.Options.Add(new MQBAdaptationOption("Database backup", "3101025C040002"));
			base.Options.Add(new MQBAdaptationOption("Gracenote DB", "3101025C040003"));
			base.Options.Add(new MQBAdaptationOption("Voice recognition", "3101025C040004"));
			base.Options.Add(new MQBAdaptationOption("Online services", "3101025C040005"));
			base.Options.Add(new MQBAdaptationOption("Logbook", "3101025C040006"));
			base.Options.Add(new MQBAdaptationOption("Cover cache memory", "3101025C040007"));
			base.Options.Add(new MQBAdaptationOption("OTA Update", "3101025C04000A"));
			this.cancelOption = new MQBAdaptationOption("Break format proccess", "3102025C");
			base.Options.Add(this.cancelOption);
		}
	}
}
