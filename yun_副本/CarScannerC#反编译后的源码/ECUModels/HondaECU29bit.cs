using System;
using System.Collections.Generic;

namespace CarScannerXamarinForms.ECUModels
{
	// Token: 0x020004DC RID: 1244
	internal class HondaECU29bit : CAN29bitECU
	{
		// Token: 0x060030AA RID: 12458 RVA: 0x0021BC0C File Offset: 0x00219E0C
		public HondaECU29bit(string name, string ecuID)
			: base(name, "18DA" + ecuID + "F1", "")
		{
			this.Protocol = 7;
			base.OpenSessionCommands = new List<string> { "3E00" };
			base.ReadDTCCommands = new List<string> { "190208", "190204" };
			base.ClearDTCCommands = new List<string> { "14FFFFFF", "A410", "14" };
			base.CloseSessionCommands = new List<string>();
		}

		// Token: 0x060030AB RID: 12459 RVA: 0x0021BCAC File Offset: 0x00219EAC
		private static List<IECU> BuildList()
		{
			OBD2Can29bitECU obd2Can29bitECU = new OBD2Can29bitECU();
			obd2Can29bitECU.ReadDTCCommands.AddRange(new string[] { "190208", "190204", "1902AF" });
			obd2Can29bitECU.ClearDTCCommands.Add("14FFFFFF");
			return new List<IECU>
			{
				obd2Can29bitECU,
				new HondaECU29bit("Engine #1", "0E"),
				new HondaECU29bit("Engine #2", "10"),
				new HondaECU29bit("Engine #3", "11"),
				new HondaECU29bit("ABS", "28"),
				new HondaECU29bit("AT/CVT #1", "1D"),
				new HondaECU29bit("AT/CVT #2", "18"),
				new HondaECU29bit("SRS", "53"),
				new HondaECU29bit("EPS", "30"),
				new HondaECU29bit("Hybrid", "01"),
				new HondaECU29bit("FCV", "07"),
				new HondaECU29bit("Active suspension", "3A"),
				new HondaECU29bit("AWD/4WD", "25")
			};
		}

		// Token: 0x170012A5 RID: 4773
		// (get) Token: 0x060030AC RID: 12460 RVA: 0x0021BE00 File Offset: 0x0021A000
		public static IReadOnlyList<IECU> ECUs
		{
			get
			{
				return HondaECU29bit.BuildList();
			}
		}
	}
}
