using System;
using System.Collections.Generic;
using CarScannerXamarinForms.OBD2;

namespace CarScannerXamarinForms.ECUModels
{
	// Token: 0x020004F8 RID: 1272
	internal class LadaECUCAN : CAN11bitECU
	{
		// Token: 0x06003154 RID: 12628 RVA: 0x0021F600 File Offset: 0x0021D800
		public LadaECUCAN(string name, string request, string response)
		{
			this.Name = name;
			base.RequestHeader = request;
			base.ResponseHeader = response;
			this.Protocol = 6;
			base.OpenSessionCommands = new List<string>();
			base.ReadDTCCommands = new List<string> { "1902AF", "190209", "19020B", "1802FF00", "1800FF00" };
			base.ClearDTCCommands = new List<string> { "14FFFFFF", "14FF00", "14", "14FFFF" };
			base.CloseSessionCommands = new List<string>();
			this.AddKWP2000Idents();
			this.AddUDSIdents();
		}

		// Token: 0x06003155 RID: 12629 RVA: 0x0021F6CC File Offset: 0x0021D8CC
		private static List<IECU> BuildList()
		{
			ValueTuple<string, string>[] array = new ValueTuple<string, string>[]
			{
				new ValueTuple<string, string>("7E0", CAN11bitECU.ECU_ENGINE_NAME),
				new ValueTuple<string, string>("7E1", CAN11bitECU.ECU_TRANSMISSION_NAME),
				new ValueTuple<string, string>("7E3", CAN11bitECU.ECU_ABS_NAME + "# 1"),
				new ValueTuple<string, string>("7E7", "BCM# 1"),
				new ValueTuple<string, string>("7E5", "SRS/Airbag# 1")
			};
			List<IECU> list = new List<IECU>();
			list.Add(new OBD2Can11bitECU
			{
				ReadDTCCommands = new List<string> { "03", "03", "07", "07", "0A", "1902AF", "1902AF", "1800FF00", "1802FF00", "17FF00" },
				ClearDTCCommands = new List<string> { "04", "04", "04", "14FFFFFF", "14", "14FF00" }
			});
			foreach (ValueTuple<string, string> valueTuple in array)
			{
				string item = valueTuple.Item1;
				string item2 = valueTuple.Item2;
				string possibleResponseHeader = CAN11bitHelper.GetPossibleResponseHeader(item, "Лада", null);
				LadaECUCAN ladaECUCAN = new LadaECUCAN(item2, item, possibleResponseHeader);
				list.Add(ladaECUCAN);
			}
			foreach (ValueTuple<string, string, string> valueTuple2 in new ValueTuple<string, string, string>[]
			{
				new ValueTuple<string, string, string>("710", "730", "CAN network gateway"),
				new ValueTuple<string, string, string>("740", "760", CAN11bitECU.ECU_ABS_NAME + "# 2"),
				new ValueTuple<string, string, string>("742", "762", "Power steering"),
				new ValueTuple<string, string, string>("743", "763", "Dashboard/Instrument cluster"),
				new ValueTuple<string, string, string>("744", "764", "Heater & air conditioning"),
				new ValueTuple<string, string, string>("745", "765", "BCM"),
				new ValueTuple<string, string, string>("74D", "76D", "Intelligent power distribution module (IPDM)"),
				new ValueTuple<string, string, string>("752", "772", "SRS/Airbag# 2"),
				new ValueTuple<string, string, string>("7CA", "7DA", "ГЛОНАСС"),
				new ValueTuple<string, string, string>("712", "732", "Multimedia")
			})
			{
				string item3 = valueTuple2.Item1;
				string item4 = valueTuple2.Item2;
				RenaultDaciaCAN11bitECU renaultDaciaCAN11bitECU = new RenaultDaciaCAN11bitECU(valueTuple2.Item3, item3, item4);
				if (item3 == "752")
				{
					renaultDaciaCAN11bitECU.OpenSessionCommands.Add("1003");
				}
				list.Add(renaultDaciaCAN11bitECU);
			}
			return list;
		}

		// Token: 0x170012DD RID: 4829
		// (get) Token: 0x06003156 RID: 12630 RVA: 0x0021F9F6 File Offset: 0x0021DBF6
		public static IReadOnlyList<IECU> ECUs
		{
			get
			{
				return LadaECUCAN.BuildList();
			}
		}
	}
}
