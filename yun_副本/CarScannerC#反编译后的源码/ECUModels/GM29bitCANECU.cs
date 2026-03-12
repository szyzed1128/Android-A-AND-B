using System;
using System.Collections.Generic;

namespace CarScannerXamarinForms.ECUModels
{
	// Token: 0x020004D6 RID: 1238
	internal class GM29bitCANECU : CAN29bitECU
	{
		// Token: 0x06003095 RID: 12437 RVA: 0x0021AB48 File Offset: 0x00218D48
		public GM29bitCANECU(string name, string requestHeader, string responseHeader)
			: base(name, requestHeader, responseHeader)
		{
			base.ReadDTCCommands.Clear();
			base.ReadDTCCommands.Add("19022C");
			base.ReadDTCCommands.Add("19020C");
			base.ClearDTCCommands.Clear();
			base.ClearDTCCommands.Add("04");
			base.ClearDTCCommands.Add("14FFFF");
			base.ClearDTCCommands.Add("14FFFFFF");
		}

		// Token: 0x06003096 RID: 12438 RVA: 0x0021ABC4 File Offset: 0x00218DC4
		private static List<IECU> BuildList()
		{
			return new List<IECU>
			{
				new OBD2Can29bitECU(),
				new GM29bitCANECU("Drive Motor Control Module (DMCM)", "14DA17F1", "142AF117"),
				new GM29bitCANECU("Drive Motor Control Module 2 (DMCM2)", "14DA1DF1", "142AF11D"),
				new GM29bitCANECU("Brake system (BSCM/ABS)", "14DA28F1", "142AF128"),
				new GM29bitCANECU("Rear wheel steering control", "14DA32F1", "142AF132"),
				new GM29bitCANECU("Power steering control module 2 (PSCM2)", "14DA35F1", "142AF135"),
				new GM29bitCANECU("Battery control module (BECM2)", "14DACBF1", "142AF1CB"),
				new GM29bitCANECU("Battery control module 2 (BECM2)", "14DACDF1", "142AF1CD"),
				new GM29bitCANECU("Engine control unit (29 bit)", "14DA11F1", "142AF111"),
				new GM29bitCANECU("Transmission control unit (29 bit)", "14DA18F1", "142AF118"),
				new GM29bitCANECU("Transfer case (29 bit)", "14DA1AF1", "142AF11A"),
				new GM29bitCANECU("CCMA (29 bit)", "14DA13F1", "142AF113"),
				new GM29bitCANECU("ABS (29 bit)", "14DA28F1", "142AF128"),
				new GM29bitCANECU("BCM (29 bit)", "14DA40F1", "142AF140"),
				new GM29bitCANECU("LCM (29 bit)", "14DA41F1", "142AF141"),
				new GM29bitCANECU("DMSM (29 bit)", "14DA6AF1", "142AF16A")
			};
		}

		// Token: 0x170012A2 RID: 4770
		// (get) Token: 0x06003097 RID: 12439 RVA: 0x0021AD67 File Offset: 0x00218F67
		public static IReadOnlyList<IECU> ECUs
		{
			get
			{
				return GM29bitCANECU.BuildList();
			}
		}
	}
}
