using System;
using CarScannerXamarinForms.Coding.DB.MQB;

namespace CarScannerXamarinForms.Coding.DB.MLB_A4B9
{
	// Token: 0x02000B83 RID: 2947
	internal class ParkingBrakeRoutine_MLBEVO : ParkingBrakeRoutineMQB
	{
		// Token: 0x06005A61 RID: 23137 RVA: 0x00431F8C File Offset: 0x0043018C
		public ParkingBrakeRoutine_MLBEVO()
		{
			base.Description = "Compatibility: ESP 9 Premium";
			this.OpenOption.Value = "310103A1040000";
			this.CloseOption.Value = "310103A0040000";
			this.CheckOption.Value = "31010542040000";
			this.CheckOption.Title = "Parking brake start-up";
		}
	}
}
