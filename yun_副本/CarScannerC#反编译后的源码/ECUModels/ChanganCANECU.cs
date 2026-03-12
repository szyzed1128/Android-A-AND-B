using System;
using System.Collections.Generic;

namespace CarScannerXamarinForms.ECUModels
{
	// Token: 0x020004CA RID: 1226
	internal class ChanganCANECU : CAN11bitECU
	{
		// Token: 0x06003070 RID: 12400 RVA: 0x002181F8 File Offset: 0x002163F8
		public ChanganCANECU(string name, string requestHeader, string responseHeader, string openSessionCommand, string readCommand, string clearCommand)
		{
			this.Name = name;
			base.RequestHeader = requestHeader;
			base.ResponseHeader = responseHeader;
			this.Protocol = 6;
			base.OpenSessionCommands = new List<string> { openSessionCommand };
			base.ReadDTCCommands = new List<string> { readCommand };
			base.ClearDTCCommands = new List<string> { clearCommand };
			base.CloseSessionCommands = new List<string>();
			this.AddUDSIdents();
			this.AddKWP2000Idents();
		}

		// Token: 0x17001297 RID: 4759
		// (get) Token: 0x06003071 RID: 12401 RVA: 0x00218277 File Offset: 0x00216477
		public static IReadOnlyList<IECU> ECUs
		{
			get
			{
				return ChanganCANECU.BuildList();
			}
		}

		// Token: 0x06003072 RID: 12402 RVA: 0x00218280 File Offset: 0x00216480
		private static List<IECU> BuildList()
		{
			return new List<IECU>
			{
				new OBD2Can11bitECU
				{
					ReadDTCCommands = new List<string> { "03", "03", "07", "07", "0A", "190208", "1902AC", "1902AF", "190278" },
					ClearDTCCommands = new List<string> { "04", "04", "04", "14FFFFFF", "14", "14FF00" }
				},
				new ChanganCANECU(CAN11bitECU.ECU_ENGINE_NAME, "7E0", "7E8", "1003", "190289", "14FFFFFF"),
				new ChanganCANECU(CAN11bitECU.ECU_TRANSMISSION_NAME, "7E1", "7E9", "1003", "190208", "14FFFFFF"),
				new ChanganCANECU(CAN11bitECU.ECU_ABS_NAME, "780", "788", "1001", "190208", "14FFFFFF"),
				new ChanganCANECU("BCM", "700", "708", "1003", "19020B", "14FFFFFF"),
				new ChanganCANECU("Dashboard/Instruments cluster", "701", "709", "1001", "190209", "14FFFFFF"),
				new ChanganCANECU("Electronic power steering", "785", "78D", "1003", "190208", "14FFFFFF")
			};
		}
	}
}
