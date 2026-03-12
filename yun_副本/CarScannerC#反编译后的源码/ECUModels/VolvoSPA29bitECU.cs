using System;
using System.Collections.Generic;
using System.Globalization;

namespace CarScannerXamarinForms.ECUModels
{
	// Token: 0x0200053B RID: 1339
	internal class VolvoSPA29bitECU : CAN29bitECU
	{
		// Token: 0x06003230 RID: 12848 RVA: 0x0022C4C4 File Offset: 0x0022A6C4
		public VolvoSPA29bitECU(string name, string request, string response = "")
			: base(name, request, response)
		{
			this.Protocol = 7;
			base.OpenSessionCommands = new List<string> { "3E00" };
			base.ReadDTCCommands = new List<string> { "190220" };
			base.ClearDTCCommands = new List<string> { "14FFFFFF" };
			base.CloseSessionCommands = new List<string>();
			this.Name = name;
			this.CANPriority = request.Substring(0, 2);
			base.RequestHeader = request.Substring(2);
			if (string.IsNullOrEmpty(response))
			{
				int num = int.Parse(request.Substring(request.Length - 3), NumberStyles.HexNumber);
				int num2 = num + num;
				string text = (57344 + num2).ToString("X4");
				response = "1" + text + "E80";
			}
			base.ResponseHeader = response;
			base.IdentsHEX.TryAdd("22F186", "Ecu idents");
		}

		// Token: 0x06003231 RID: 12849 RVA: 0x0022C5B8 File Offset: 0x0022A7B8
		private static List<IECU> BuildList()
		{
			OBD2Can11bitECU obd2Can11bitECU = new OBD2Can11bitECU();
			obd2Can11bitECU.ReadDTCCommands.AddRange(new string[] { "190208", "190204", "1902AF" });
			obd2Can11bitECU.ClearDTCCommands.Add("14FFFFFF");
			List<IECU> list = new List<IECU>
			{
				obd2Can11bitECU,
				new VolvoSPA29bitECU("CEM", "1DD01A01", ""),
				new VolvoSPA29bitECU(CAN11bitECU.ECU_ENGINE_NAME, "1DD01630", ""),
				new VolvoSPA29bitECU(CAN11bitECU.ECU_TRANSMISSION_NAME, "1DD01632", ""),
				new VolvoSPA29bitECU(CAN11bitECU.ECU_ABS_NAME, "1DD01631", ""),
				new VolvoSPA29bitECU("DEM", "1DD01638", ""),
				new VolvoSPA29bitECU("SCL", "1DD01615", ""),
				new VolvoSPA29bitECU("SUM", "1DD01614", ""),
				new VolvoSPA29bitECU("AUD", "1DD01212", ""),
				new VolvoSPA29bitECU("DIM", "1DD01801", ""),
				new VolvoSPA29bitECU("DIM/BBS", "1DD01B51", ""),
				new VolvoSPA29bitECU("IMS", "1DD01B52", ""),
				new VolvoSPA29bitECU("SWM", "1DD01B91", ""),
				new VolvoSPA29bitECU("HUD", "1DD01841", ""),
				new VolvoSPA29bitECU("AUD", "1DD01212", ""),
				new VolvoSPA29bitECU("BMS", "1DD01B61", ""),
				new VolvoSPA29bitECU("CCD", "1DD01241", ""),
				new VolvoSPA29bitECU("AGU", "1DD01203", ""),
				new VolvoSPA29bitECU("OHC", "1DD01B22", ""),
				new VolvoSPA29bitECU("OFM", "1DD01639", ""),
				new VolvoSPA29bitECU("WAM", "1DD01431", ""),
				new VolvoSPA29bitECU("RML", "1DD01416", ""),
				new VolvoSPA29bitECU("RMR", "1DD01417", ""),
				new VolvoSPA29bitECU("TACM", "1DD0163C", ""),
				new VolvoSPA29bitECU("VDDM", "1DD01601", ""),
				new VolvoSPA29bitECU("TVM", "1DD01202", ""),
				new VolvoSPA29bitECU("PAC", "1DD01221", ""),
				new VolvoSPA29bitECU("DDM", "1DD01A12", ""),
				new VolvoSPA29bitECU("PDM", "1DD01A13", ""),
				new VolvoSPA29bitECU("POT", "1DD01A15", ""),
				new VolvoSPA29bitECU("TRM", "1DD01A17", ""),
				new VolvoSPA29bitECU("GSM", "1DD01661", ""),
				new VolvoSPA29bitECU("IEM", "1DD01637", ""),
				new VolvoSPA29bitECU("PSMD", "1DD01A14", ""),
				new VolvoSPA29bitECU("PSMP", "1DD01A1A", ""),
				new VolvoSPA29bitECU("SODL", "1DD01432", ""),
				new VolvoSPA29bitECU("SODR", "1DD01433", ""),
				new VolvoSPA29bitECU("IHU", "1DD01201", ""),
				new VolvoSPA29bitECU("OBC", "1DD01634", ""),
				new VolvoSPA29bitECU("PSCM", "1DD01612", ""),
				new VolvoSPA29bitECU("EGSM", "1DD01633", ""),
				new VolvoSPA29bitECU("ESM", "1DD0163A", ""),
				new VolvoSPA29bitECU("VCM", "1DD01001", ""),
				new VolvoSPA29bitECU("ASDM", "1DD01401", ""),
				new VolvoSPA29bitECU("NRCM", "1DD0163D", ""),
				new VolvoSPA29bitECU("SAS", "1DD01616", ""),
				new VolvoSPA29bitECU("IGM", "1DD01636", ""),
				new VolvoSPA29bitECU("HCML", "1DD01BB3", ""),
				new VolvoSPA29bitECU("HCMR", "1DD01BB4", ""),
				new VolvoSPA29bitECU("BECM", "1DD01635", ""),
				new VolvoSPA29bitECU(CAN11bitECU.ECU_ABS_NAME + " #2", "1DD016A1", ""),
				new VolvoSPA29bitECU("VCU #1", "1DD01602", ""),
				new VolvoSPA29bitECU("BBM", "1DD01617", ""),
				new VolvoSPA29bitECU("CCM", "1DD01A11", ""),
				new VolvoSPA29bitECU("IHFA", "1DD01692", ""),
				new VolvoSPA29bitECU("ISGM", "1DD01691", ""),
				new VolvoSPA29bitECU("MVBM", "1DD016B1", ""),
				new VolvoSPA29bitECU("MVCM", "1DD01681", ""),
				new VolvoSPA29bitECU("NRCM", "1DD0163D", "")
			};
			for (int i = 1; i < list.Count; i++)
			{
				VolvoSPA29bitECU volvoSPA29bitECU = (VolvoSPA29bitECU)list[i];
				volvoSPA29bitECU.Name = "[SPA/CMA] " + volvoSPA29bitECU.Name;
			}
			return list;
		}

		// Token: 0x170012FC RID: 4860
		// (get) Token: 0x06003232 RID: 12850 RVA: 0x0022CC34 File Offset: 0x0022AE34
		public static IReadOnlyList<IECU> ECUs
		{
			get
			{
				return VolvoSPA29bitECU.BuildList();
			}
		}
	}
}
