using System;
using System.Collections.Generic;

namespace CarScannerXamarinForms.ECUModels
{
	// Token: 0x020004D1 RID: 1233
	internal class FiatCAN29bitECU : CAN29bitECU
	{
		// Token: 0x06003086 RID: 12422 RVA: 0x00219878 File Offset: 0x00217A78
		public FiatCAN29bitECU(string name, string ecuID)
			: base(name, "18DA" + ecuID + "F1", "18DAF1" + ecuID)
		{
			this.Protocol = 7;
			base.OpenSessionCommands = new List<string> { "3E00" };
			base.ReadDTCCommands = new List<string> { "190208", "19020D", "1800FF00" };
			base.ClearDTCCommands = new List<string> { "14FFFFFF" };
			base.CloseSessionCommands = new List<string>();
		}

		// Token: 0x06003087 RID: 12423 RVA: 0x00219914 File Offset: 0x00217B14
		private static List<IECU> BuildList()
		{
			OBD2Can29bitECU obd2Can29bitECU = new OBD2Can29bitECU();
			obd2Can29bitECU.ReadDTCCommands.AddRange(new string[] { "190208", "190204", "1902AF", "19020D" });
			obd2Can29bitECU.ClearDTCCommands.Add("14FFFFFF");
			return new List<IECU>
			{
				obd2Can29bitECU,
				new FiatCAN29bitECU(CAN11bitECU.ECU_ENGINE_NAME, "10"),
				new FiatCAN29bitECU(CAN11bitECU.ECU_TRANSMISSION_NAME, "18"),
				new FiatCAN29bitECU(CAN11bitECU.ECU_ABS_NAME, "28"),
				new FiatCAN29bitECU("BCM", "40"),
				new FiatCAN29bitECU("ACC", "2A"),
				new FiatCAN29bitECU("DTCM", "1A"),
				new FiatCAN29bitECU("FFCM", "31"),
				new FiatCAN29bitECU("ORC/AirBag", "C0"),
				new FiatCAN29bitECU("ESM (Electronic shifter module)", "1F"),
				new FiatCAN29bitECU("IC", "60"),
				new FiatCAN29bitECU("RFH", "C7"),
				new FiatCAN29bitECU("OCM", "58"),
				new FiatCAN29bitECU("AHLM", "71"),
				new FiatCAN29bitECU("DTCM", "1A"),
				new FiatCAN29bitECU("SCCM", "86"),
				new FiatCAN29bitECU("ESL", "C4"),
				new FiatCAN29bitECU("PTU", "1D"),
				new FiatCAN29bitECU("RDM", "1E"),
				new FiatCAN29bitECU("PAM", "A0"),
				new FiatCAN29bitECU("EPS", "30"),
				new FiatCAN29bitECU("EPB", "2B"),
				new FiatCAN29bitECU("RFH", "C7"),
				new FiatCAN29bitECU("DCU (Dosing Control Unit)", "01"),
				new FiatCAN29bitECU("AHCP", "12"),
				new FiatCAN29bitECU("MCPB (Motor Control Processor B)", "15"),
				new FiatCAN29bitECU("BPCM (Battery Pack Control Module)", "44"),
				new FiatCAN29bitECU("HCP (Hybrid Control Processor)", "4B"),
				new FiatCAN29bitECU("APM (Adjustable pedal module)", "4C"),
				new FiatCAN29bitECU("IDCM", "50"),
				new FiatCAN29bitECU("TBM (Telematics Box Module)", "C6"),
				new FiatCAN29bitECU("RF HUB", "C7"),
				new FiatCAN29bitECU("SGW (Security Gateway)", "CB")
			};
		}

		// Token: 0x1700129D RID: 4765
		// (get) Token: 0x06003088 RID: 12424 RVA: 0x00219C14 File Offset: 0x00217E14
		public static IReadOnlyList<IECU> ECUs
		{
			get
			{
				return FiatCAN29bitECU.BuildList();
			}
		}
	}
}
