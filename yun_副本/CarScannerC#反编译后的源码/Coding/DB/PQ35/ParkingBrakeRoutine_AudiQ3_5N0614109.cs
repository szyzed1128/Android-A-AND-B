using System;
using CarScannerXamarinForms.Coding.DB.MQB;

namespace CarScannerXamarinForms.Coding.DB.PQ35
{
	// Token: 0x02000A22 RID: 2594
	internal class ParkingBrakeRoutine_AudiQ3_5N0614109 : ParkingBrakeRoutineMQB
	{
		// Token: 0x06005297 RID: 21143 RVA: 0x003F9BB8 File Offset: 0x003F7DB8
		public ParkingBrakeRoutine_AudiQ3_5N0614109()
		{
			base.Description = "Compatibility: 5N0614109BS/CA/CF (UDS protocol only!)";
			this.OpenOption.Value = "310103A101";
			this.CloseOption.Value = "310103A001";
			this.CheckOption.Value = "3101041001";
		}
	}
}
