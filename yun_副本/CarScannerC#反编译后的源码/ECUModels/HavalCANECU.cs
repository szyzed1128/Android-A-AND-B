using System;
using System.Collections.Generic;

namespace CarScannerXamarinForms.ECUModels
{
	// Token: 0x020004DA RID: 1242
	internal class HavalCANECU : CAN11bitECU
	{
		// Token: 0x060030A5 RID: 12453 RVA: 0x0021B4DC File Offset: 0x002196DC
		public HavalCANECU(string name, string request, string response)
		{
			this.Name = name;
			base.RequestHeader = request;
			base.ResponseHeader = response;
			this.Protocol = 6;
			base.OpenSessionCommands = new List<string> { "3E00" };
			base.ReadDTCCommands = new List<string> { "190201", "18000000" };
			base.ClearDTCCommands = new List<string> { "14FFFFFF", "14FF00" };
			this.AddUDSIdents();
			this.AddKWP2000Idents();
		}

		// Token: 0x060030A6 RID: 12454 RVA: 0x0021B570 File Offset: 0x00219770
		public HavalCANECU(string name, int request, int response_diff)
			: this(name, request.ToString("X3"), (request + response_diff).ToString("X3"))
		{
		}

		// Token: 0x170012A4 RID: 4772
		// (get) Token: 0x060030A7 RID: 12455 RVA: 0x0021B5A0 File Offset: 0x002197A0
		public static IReadOnlyList<IECU> ECUs
		{
			get
			{
				return HavalCANECU.BuildList();
			}
		}

		// Token: 0x060030A8 RID: 12456 RVA: 0x0021B5A8 File Offset: 0x002197A8
		private static List<IECU> BuildList()
		{
			ValueTuple<int, int, string>[] array = new ValueTuple<int, int, string>[]
			{
				new ValueTuple<int, int, string>(2016, 8, CAN11bitECU.ECU_ENGINE_NAME),
				new ValueTuple<int, int, string>(2017, 8, CAN11bitECU.ECU_TRANSMISSION_NAME),
				new ValueTuple<int, int, string>(1894, 64, "Instruments panel"),
				new ValueTuple<int, int, string>(1922, 64, CAN11bitECU.ECU_ABS_NAME),
				new ValueTuple<int, int, string>(1893, 64, "BCM"),
				new ValueTuple<int, int, string>(1898, 64, "SRS var.1"),
				new ValueTuple<int, int, string>(1824, 8, "SRS var.2"),
				new ValueTuple<int, int, string>(1926, 64, "TOD/4WD"),
				new ValueTuple<int, int, string>(1891, 64, "A/C"),
				new ValueTuple<int, int, string>(1896, 64, "Passive entry Passive Start (PEPS)"),
				new ValueTuple<int, int, string>(1889, 64, "Independent gateway"),
				new ValueTuple<int, int, string>(1923, 64, "Steering angle sensor"),
				new ValueTuple<int, int, string>(1900, 64, "TPMS"),
				new ValueTuple<int, int, string>(1908, 64, "Rear view camera"),
				new ValueTuple<int, int, string>(1910, 64, "Around view monitoring system"),
				new ValueTuple<int, int, string>(1909, 64, "Parking assistance system"),
				new ValueTuple<int, int, string>(1906, 64, "Adaptive cruise control"),
				new ValueTuple<int, int, string>(1902, 64, "Radar side detection system - Left"),
				new ValueTuple<int, int, string>(1903, 64, "Radar side detection system - Right"),
				new ValueTuple<int, int, string>(1914, 64, "Intelligent forward camera"),
				new ValueTuple<int, int, string>(1907, 64, "HUD"),
				new ValueTuple<int, int, string>(1895, 64, "Seat heat, massage system"),
				new ValueTuple<int, int, string>(1897, 64, "Electronic steering control lock"),
				new ValueTuple<int, int, string>(1918, 64, "Telematics box"),
				new ValueTuple<int, int, string>(1950, 64, "Glonass"),
				new ValueTuple<int, int, string>(1682, 1, CAN11bitECU.ECU_TRANSMISSION_NAME + " #2"),
				new ValueTuple<int, int, string>(1684, 1, CAN11bitECU.ECU_ABS_NAME + " #2"),
				new ValueTuple<int, int, string>(1698, 1, "SRS #2"),
				new ValueTuple<int, int, string>(1554, 1, "BCM #2"),
				new ValueTuple<int, int, string>(1704, 1, "Instruments panel #2"),
				new ValueTuple<int, int, string>(1552, 1, "Passive entry Passive Start (PEPS) #2"),
				new ValueTuple<int, int, string>(1562, 1, "A/C #2"),
				new ValueTuple<int, int, string>(1733, 1, "EPB"),
				new ValueTuple<int, int, string>(1538, 1, "Gateway"),
				new ValueTuple<int, int, string>(1616, 1, "DDCM"),
				new ValueTuple<int, int, string>(1618, 1, "PDCM"),
				new ValueTuple<int, int, string>(1620, 1, "DSM"),
				new ValueTuple<int, int, string>(1584, 1, "F-PAS"),
				new ValueTuple<int, int, string>(1586, 1, "L-PAS"),
				new ValueTuple<int, int, string>(1702, 1, "RVC #2"),
				new ValueTuple<int, int, string>(1696, 1, "AFS"),
				new ValueTuple<int, int, string>(1564, 1, "CD"),
				new ValueTuple<int, int, string>(1686, 1, "SAS"),
				new ValueTuple<int, int, string>(1700, 1, "TOD/4WD #2"),
				new ValueTuple<int, int, string>(1622, 1, "SCM"),
				new ValueTuple<int, int, string>(1912, 64, "EGD"),
				new ValueTuple<int, int, string>(1911, 64, "IFV"),
				new ValueTuple<int, int, string>(1714, 1, "SBWM"),
				new ValueTuple<int, int, string>(1930, 64, "VCU/HCU"),
				new ValueTuple<int, int, string>(1931, 64, "BMS"),
				new ValueTuple<int, int, string>(1927, 64, "MCU/P4M"),
				new ValueTuple<int, int, string>(1932, 64, "OBC"),
				new ValueTuple<int, int, string>(1929, 64, "MCU/P2M")
			};
			List<IECU> list = new List<IECU>();
			list.Add(new OBD2Can11bitECU
			{
				ReadDTCCommands = new List<string>
				{
					"03", "03", "03", "07", "07", "07", "0A", "17FF00", "1902AF", "1902AC",
					"1800FF00", "1802FF00"
				},
				ClearDTCCommands = new List<string> { "04", "04", "04", "14", "14FF00", "14FFFFFF" }
			});
			foreach (ValueTuple<int, int, string> valueTuple in array)
			{
				int item = valueTuple.Item1;
				int item2 = valueTuple.Item2;
				HavalCANECU havalCANECU = new HavalCANECU(valueTuple.Item3, item, item2);
				list.Add(havalCANECU);
			}
			return list;
		}
	}
}
