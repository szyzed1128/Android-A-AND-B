using System;
using System.Collections.Generic;

namespace CarScannerXamarinForms.ECUModels
{
	// Token: 0x020004E2 RID: 1250
	internal class ISUZUCAN11bitECU : CAN11bitECU
	{
		// Token: 0x060030D6 RID: 12502 RVA: 0x0021CBEC File Offset: 0x0021ADEC
		public ISUZUCAN11bitECU(string name, int request)
		{
			this.Name = name;
			base.RequestHeader = request.ToString("X3");
			base.ResponseHeader = (request + 128).ToString("X3");
			this.Protocol = 6;
			base.OpenSessionCommands = new List<string> { "3E00" };
			base.ReadDTCCommands = new List<string> { "1902AF", "1902AC", "1800FF00", "1902AC", "19D2FF00", "A98102F", "A98112F", "A98118F", "A9815AF" };
			base.ClearDTCCommands = new List<string> { "14FFFFFF", "14FF00" };
		}

		// Token: 0x170012BD RID: 4797
		// (get) Token: 0x060030D7 RID: 12503 RVA: 0x0021CCDE File Offset: 0x0021AEDE
		public static IReadOnlyList<IECU> ECUs
		{
			get
			{
				return ISUZUCAN11bitECU.BuildList();
			}
		}

		// Token: 0x060030D8 RID: 12504 RVA: 0x0021CCE8 File Offset: 0x0021AEE8
		private static List<IECU> BuildList()
		{
			List<IECU> list = new List<IECU>();
			list.AddRange(GMCANECU.ECUs);
			foreach (ValueTuple<string, int> valueTuple in new ValueTuple<string, int>[]
			{
				new ValueTuple<string, int>(CAN11bitECU.ECU_ENGINE_NAME + " (2019-)", 1568),
				new ValueTuple<string, int>(CAN11bitECU.ECU_TRANSMISSION_NAME + " (2019-)", 1569),
				new ValueTuple<string, int>("Intellingent Battery System (2019-)", 1540),
				new ValueTuple<string, int>("Idling Stop System (2019-)", 1538),
				new ValueTuple<string, int>(CAN11bitECU.ECU_ABS_NAME + " (2019-)", 1536),
				new ValueTuple<string, int>("4WD (2019-)", 1555),
				new ValueTuple<string, int>("Differential lock Module (2019-)", 1557),
				new ValueTuple<string, int>("Immobilizer (2019-)", 1545),
				new ValueTuple<string, int>("Central Gateway (2019-)", 1551),
				new ValueTuple<string, int>("Body Control Module (2019-)", 1541),
				new ValueTuple<string, int>("HVAC (2019-)", 1543),
				new ValueTuple<string, int>("Headlamp (2019-)", 1542),
				new ValueTuple<string, int>("Meter (2019-)", 1537),
				new ValueTuple<string, int>("Navigation (2019-)", 1560),
				new ValueTuple<string, int>("Parking Aid (2019-)", 1546),
				new ValueTuple<string, int>("Radar Sensor Left (2019-)", 1552),
				new ValueTuple<string, int>("Radar Sensor Right (2019-)", 1553),
				new ValueTuple<string, int>("SRS", 1539)
			})
			{
				string item = valueTuple.Item1;
				int item2 = valueTuple.Item2;
				ISUZUCAN11bitECU isuzucan11bitECU = new ISUZUCAN11bitECU(item, item2);
				list.Add(isuzucan11bitECU);
			}
			list.Add(new ISUZUCAN11bitECU("Transfer case (2019-)", 2020)
			{
				ResponseHeader = "7EC"
			});
			return list;
		}
	}
}
