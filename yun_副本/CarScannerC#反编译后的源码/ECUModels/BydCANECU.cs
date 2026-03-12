using System;
using System.Collections.Generic;

namespace CarScannerXamarinForms.ECUModels
{
	// Token: 0x020004B4 RID: 1204
	internal class BydCANECU : CAN11bitECU
	{
		// Token: 0x06002FDC RID: 12252 RVA: 0x00214940 File Offset: 0x00212B40
		public BydCANECU(string name, int request, string openSession = "1001", string[] readDTCCommands = null, string[] clearDTCCommands = null)
		{
			this.Name = name;
			base.RequestHeader = request.ToString("X3");
			base.ResponseHeader = (request + 8).ToString("X3");
			this.Protocol = 6;
			base.OpenSessionCommands = new List<string>(1) { openSession };
			if (readDTCCommands == null)
			{
				base.ReadDTCCommands = new List<string> { "190209", "180000FF" };
			}
			else
			{
				base.ReadDTCCommands = new List<string>(readDTCCommands);
			}
			if (clearDTCCommands == null)
			{
				base.ClearDTCCommands = new List<string>(4) { "14FFFFFF", "1400FF" };
			}
			else
			{
				base.ClearDTCCommands = new List<string>(clearDTCCommands);
			}
			this.AddUDSIdents();
			this.AddKWP2000Idents();
		}

		// Token: 0x06002FDD RID: 12253 RVA: 0x00214A14 File Offset: 0x00212C14
		private static List<IECU> BuildList()
		{
			string text = "14FFFFFF";
			string text2 = "190209";
			string text3 = "1902FF";
			string text4 = "1901C9";
			string text5 = "1902C9";
			string text6 = "180000FF";
			string text7 = "1400FF";
			string text8 = "19FF";
			ValueTuple<string, string>[] array = new ValueTuple<string, string>[]
			{
				new ValueTuple<string, string>("7E0", CAN11bitECU.ECU_ENGINE_NAME),
				new ValueTuple<string, string>("7E1", CAN11bitECU.ECU_TRANSMISSION_NAME)
			};
			return new List<IECU>(100)
			{
				new OBD2Can11bitECU
				{
					ReadDTCCommands = new List<string> { "03", "03", "07", "07", "0A", "190208", "1902AC", "1902AF", "190223", "190278" },
					ClearDTCCommands = new List<string> { "04", "04", "04", "14FFFFFF", "14", "14FF00" }
				},
				new BydCANECU("ECM", 2016, "1001", new string[] { text2, text8, "190109", text6 }, new string[] { text, text7 }),
				new BydCANECU("TCM", 2017, "1001", new string[] { text2, "1800FFFFFF" }, new string[] { text, text7 }),
				new BydCANECU("BPCM", 1792, "1001", null, null),
				new BydCANECU("IPA", 1793, "1001", null, null),
				new BydCANECU("DCVDM", 1794, "1001", null, null),
				new BydCANECU("MPC", 1796, "1001", null, null),
				new BydCANECU("Green clean PM2.5", 1797, "1001", null, null),
				new BydCANECU("DHCUYQ FR door handle", 1798, "1001", null, null),
				new BydCANECU("EPB", 1808, "1001", new string[] { text5, text4 }, null),
				new BydCANECU("SWS", 1809, "1001", null, null),
				new BydCANECU("MSRF", 1811, "1001", null, null),
				new BydCANECU("RCP", 1812, "1001", null, null),
				new BydCANECU("SWAS", 1813, "1001", null, null),
				new BydCANECU("IBCMB1", 1824, "1001", null, null),
				new BydCANECU("RL_L/TL_L/TL_R/CIL", 1825, "1001", null, null),
				new BydCANECU("INS", 1827, "1001", null, null),
				new BydCANECU("DHCUZH RL door handle", 1828, "1001", null, null),
				new BydCANECU("CS", 1829, "1001", null, null),
				new BydCANECU("DMCU/SRM/WLMRL/WLMRR/SSM", 1830, "1001", null, null),
				new BydCANECU("DMCU", 1838, "1001", null, null),
				new BydCANECU("ECL", 1841, "1001", new string[] { text2, text6 }, new string[] { text, text7 }),
				new BydCANECU("DHCUZQ FL door handle", 1842, "1001", null, null),
				new BydCANECU("Wireless charging", 1843, "1001", null, null),
				new BydCANECU("PAS", 1845, "1001", null, null),
				new BydCANECU("ACM/PAL", 1847, "1001", null, null),
				new BydCANECU("Multimedia", 1857, "1001", null, null),
				new BydCANECU("MSRZ", 1858, "1001", null, null),
				new BydCANECU("VCU", 1859, "1001", null, null),
				new BydCANECU("RHP", 1862, "1001", null, null),
				new BydCANECU("DCU_FL", 1863, "1001", null, null),
				new BydCANECU("SMRL/SMRR/SECU_D", 1872, "1001", null, null),
				new BydCANECU("AMP", 1873, "1001", null, null),
				new BydCANECU("DCU_FR", 1874, "1001", null, null),
				new BydCANECU("TPMS", 1876, "1001", null, null),
				new BydCANECU("M_P2", 1878, "1001", null, null),
				new BydCANECU("LBMS", 1890, "1001", null, null),
				new BydCANECU("Battery heater", 1891, "1001", null, null),
				new BydCANECU("PTC", 1894, "1001", null, null),
				new BydCANECU("FL_L/FMPL", 1904, "1001", null, null),
				new BydCANECU("FL_R", 1905, "1001", null, null),
				new BydCANECU("FV/FVC", 1907, "1001", null, null),
				new BydCANECU("Radio", 1908, "1001", null, null),
				new BydCANECU("MCU/Front drive MCU/EDCM/VTOG_DSP2", 1920, "1001", null, null),
				new BydCANECU("BMS", 1921, "1001", null, null),
				new BydCANECU("ABS/ESC/IPB", 1922, "1001", new string[] { text3 }, null),
				new BydCANECU("EPS", 1923, "1001", null, null),
				new BydCANECU("MCU/Rear drive MCU", 1925, "1001", null, null),
				new BydCANECU("Gear position actuator", 1927, "1001", null, null),
				new BydCANECU("Gateway", 1938, "1001", null, null),
				new BydCANECU("DC-DC", 1939, "1001", null, null),
				new BydCANECU("PH", 1940, "1001", null, null),
				new BydCANECU("CDU", 1943, "1001", null, null),
				new BydCANECU("Volume knob", 1953, "1001", null, null),
				new BydCANECU("VTOG_C/VTOG_D/VTOG_M", 1962, "1001", null, null),
				new BydCANECU("4G", 1955, "1001", null, null),
				new BydCANECU("SCPA", 1959, "1001", null, null),
				new BydCANECU("ACC", 1971, "1001", null, null),
				new BydCANECU("BCC", 1972, "1001", null, null),
				new BydCANECU("SECU_P", 1973, "1001", null, null),
				new BydCANECU("AVAS", 1974, "1001", null, null),
				new BydCANECU("ACC", 1979, "1001", null, null),
				new BydCANECU("BDECU", 1986, "1001", null, null),
				new BydCANECU("Blind spot monitoring", 1986, "1001", null, null),
				new BydCANECU("DHCUYH RR door handle", 1987, "1001", null, null),
				new BydCANECU("ACCP", 1990, "1001", null, null),
				new BydCANECU("PTECU", 1991, "1001", null, null),
				new BydCANECU("VTDR", 2001, "1001", null, null),
				new BydCANECU("SECU_RL", 2002, "1001", null, null),
				new BydCANECU("Blind spot distribution", 2004, "1001", null, null),
				new BydCANECU("NCM/Smart Entry", 2006, "1001", null, null),
				new BydCANECU("SRS", 2033, "1001", new string[] { text2, text6 }, new string[] { text, text7 }),
				new BydCANECU("FMRR", 2034, "1001", null, null)
			};
		}

		// Token: 0x17001274 RID: 4724
		// (get) Token: 0x06002FDE RID: 12254 RVA: 0x0021542A File Offset: 0x0021362A
		public static IReadOnlyList<IECU> ECUs
		{
			get
			{
				return BydCANECU.BuildList();
			}
		}
	}
}
