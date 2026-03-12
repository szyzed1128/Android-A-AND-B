using System;

namespace CarScannerXamarinForms.OBD2.PIDS.InternalActions
{
	// Token: 0x0200043B RID: 1083
	internal class InternalActionPID : CustomPID
	{
		// Token: 0x06002DC8 RID: 11720 RVA: 0x00200A3A File Offset: 0x001FEC3A
		public static CustomPID PID_ActionReset()
		{
			return new InternalActionPID(Translate.GetString("Action_ResetTrip"), Translate.GetString("Action_ResetTripShort"), "INTERNAL:RESETFUELDISTANCESPEED")
			{
				Id = 904
			};
		}

		// Token: 0x06002DC9 RID: 11721 RVA: 0x00200A68 File Offset: 0x001FEC68
		public InternalActionPID(string Name, string ShortName, string Command)
			: base(Name, ShortName, Command, "", "", UnitsHelper.Units.None, 0.0, 0.0, "", "", true, Roles.None, CustomPIDType.Action, 0, 0, 0.0, 0.0, 0.0, false, false, true, null)
		{
		}

		// Token: 0x17001251 RID: 4689
		// (get) Token: 0x06002DCA RID: 11722 RVA: 0x000A8D6F File Offset: 0x000A6F6F
		// (set) Token: 0x06002DCB RID: 11723 RVA: 0x000027D4 File Offset: 0x000009D4
		public override bool IsAvailable
		{
			get
			{
				return true;
			}
			set
			{
			}
		}
	}
}
