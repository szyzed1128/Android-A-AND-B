using System;
using System.Collections.Generic;

namespace CarScannerXamarinForms.ECUModels
{
	// Token: 0x02000500 RID: 1280
	internal class NissanCANECU : CAN11bitECU
	{
		// Token: 0x0600316C RID: 12652 RVA: 0x00221CD8 File Offset: 0x0021FED8
		public NissanCANECU(string name, string request, string response)
		{
			this.Name = name;
			base.RequestHeader = request;
			base.ResponseHeader = response;
			this.Protocol = 6;
			base.OpenSessionCommands = new List<string> { "10C0" };
			base.ReadDTCCommands = new List<string> { "17FF00", "1902AF", "19022B" };
			base.ClearDTCCommands = new List<string> { "14FFFFFF", "14FF00", "14" };
			base.CloseSessionCommands = new List<string> { "1081" };
			this.AddUDSIdents();
			this.AddKWP2000Idents();
			base.IdentsHEX.TryAdd("2181", "ECU Idents");
			base.IdentsHEX.TryAdd("2183", "ECU Idents");
			base.IdentsHEX.TryAdd("2110", "ECU Idents");
			base.IdentsHEX.TryAdd("21FE", "ECU Idents");
			base.IdentsHEX.TryAdd("22F1A0", "ECU Idents");
			base.IdentsHEX.TryAdd("22F190", "ECU Idents");
			base.IdentsHEX.TryAdd("22F18A", "ECU Idents");
			base.IdentsHEX.TryAdd("22F194", "ECU Idents");
			base.IdentsHEX.TryAdd("22F195", "ECU Idents");
			base.IdentsHEX.TryAdd("22F191", "ECU Idents");
		}

		// Token: 0x0600316D RID: 12653 RVA: 0x00221E74 File Offset: 0x00220074
		private static List<IECU> BuildList()
		{
			List<IECU> list2;
			try
			{
				ValueTuple<string, string, string>[] array = new ValueTuple<string, string, string>[]
				{
					new ValueTuple<string, string, string>("7E0", "7E8", CAN11bitECU.ECU_ENGINE_NAME),
					new ValueTuple<string, string, string>("7E1", "7E9", CAN11bitECU.ECU_TRANSMISSION_NAME),
					new ValueTuple<string, string, string>("740", "760", CAN11bitECU.ECU_ABS_NAME),
					new ValueTuple<string, string, string>("752", "772", "SRS/Airbags (11 bit)"),
					new ValueTuple<string, string, string>("748", "768", "All wheel drive system (AWD/4WD) (11 bit)"),
					new ValueTuple<string, string, string>("710", "730", "CAN network gateway (11 bit)"),
					new ValueTuple<string, string, string>("745", "765", "BCM (11 bit)"),
					new ValueTuple<string, string, string>("743", "763", "Dashboard/Instrument cluster (11 bit)"),
					new ValueTuple<string, string, string>("718", "738", "Dashboard/Instrument cluster submodule (11 bit)"),
					new ValueTuple<string, string, string>("75C", "77C", "Adaptive headlights (11 bit)"),
					new ValueTuple<string, string, string>("701", "721", "Four wheel active steering (11 bit)"),
					new ValueTuple<string, string, string>("71A", "73A", "Accelerator pedal position sensor (11 bit)"),
					new ValueTuple<string, string, string>("75D", "77D", "Adaptive cruise control (11 bit)"),
					new ValueTuple<string, string, string>("7C3", "7C9", "Adaptive cruise control submodule (11 bit)"),
					new ValueTuple<string, string, string>("74F", "76F", "Automatic driver positioner (11 bit)"),
					new ValueTuple<string, string, string>("749", "74A", "Active noise control (11 bit)"),
					new ValueTuple<string, string, string>("759", "779", "Sliding door (left) (11 bit)"),
					new ValueTuple<string, string, string>("709", "729", "Sliding door (right) (11 bit)"),
					new ValueTuple<string, string, string>("7B7", "7BA", "All-round vision camera (11 bit)"),
					new ValueTuple<string, string, string>("795", "7B5", "Blind spot monitor (11 bit)"),
					new ValueTuple<string, string, string>("798", "794", "Active steering (DAST1) (11 bit)"),
					new ValueTuple<string, string, string>("781", "7C2", "Active steering (DAST2) (11 bit)"),
					new ValueTuple<string, string, string>("742", "762", "Active steering (DAST3) (11 bit)"),
					new ValueTuple<string, string, string>("753", "773", "Active steering (HICAS) (11 bit)"),
					new ValueTuple<string, string, string>("746", "783", "Telematics (11 bit)"),
					new ValueTuple<string, string, string>("7A4", "7AC", "Differential lock (11 bit)"),
					new ValueTuple<string, string, string>("755", "775", "Hill start assist (11 bit)"),
					new ValueTuple<string, string, string>("790", "791", "Energy management (11 bit)"),
					new ValueTuple<string, string, string>("6FA", "49F", "Fuel pump (11 bit)"),
					new ValueTuple<string, string, string>("744", "764", "Heater & air conditioning (11 bit)"),
					new ValueTuple<string, string, string>("75B", "77B", "Audio amplifier (11 bit)"),
					new ValueTuple<string, string, string>("78E", "78F", "Multi-function display (11 bit)"),
					new ValueTuple<string, string, string>("70C", "700", "Chassis (11 bit)"),
					new ValueTuple<string, string, string>("747", "767", "Multi audio/video control unit (11 bit)"),
					new ValueTuple<string, string, string>("7E5", "7ED", "Hybrid battery (11 bit)"),
					new ValueTuple<string, string, string>("79B", "7BB", "Electric vehicle battery (11 bit)"),
					new ValueTuple<string, string, string>("707", "727", "Lane-keeping assist (11 bit)"),
					new ValueTuple<string, string, string>("7E3", "7EB", "Motor control (11 bit)"),
					new ValueTuple<string, string, string>("784", "78C", "EV motor control (11 bit)"),
					new ValueTuple<string, string, string>("792", "793", "Battery charging (11 bit)"),
					new ValueTuple<string, string, string>("713", "733", "Rear hatch (11 bit)"),
					new ValueTuple<string, string, string>("7A1", "7A9", "Mass airflow sensor (11 bit)"),
					new ValueTuple<string, string, string>("70A", "72A", "Pop-up engine hood for pedestrian protection (11 bit)"),
					new ValueTuple<string, string, string>("754", "774", "Precrash seatbelt protection (11 bit)"),
					new ValueTuple<string, string, string>("7D9", "7DB", "Auxiliary heater (11 bit)"),
					new ValueTuple<string, string, string>("723", "735", "Intelligent brake assist (11 bit)"),
					new ValueTuple<string, string, string>("74B", "76B", "Convertible top (11 bit)"),
					new ValueTuple<string, string, string>("704", "724", "Adaptive suspension (11 bit)"),
					new ValueTuple<string, string, string>("79D", "7BD", "Shift control unit (11 bit)"),
					new ValueTuple<string, string, string>("73D", "73E", "Blind spot warning (left) (11 bit)"),
					new ValueTuple<string, string, string>("7D4", "7D5", "Blind spot warning (right) (11 bit)"),
					new ValueTuple<string, string, string>("7DC", "7DD", "Side magic bumper (11 bit)"),
					new ValueTuple<string, string, string>("74C", "76C", "Entry / smart key (11 bit)"),
					new ValueTuple<string, string, string>("74E", "76E", "Sonar (11 bit)"),
					new ValueTuple<string, string, string>("748", "768", "All wheel drive system (AWD/4WD) (11 bit)"),
					new ValueTuple<string, string, string>("74D", "76D", "Intelligent power distribution module (IPDM) (11 bit)"),
					new ValueTuple<string, string, string>("797", "79A", "Vehicle control module (11 bit)"),
					new ValueTuple<string, string, string>("73F", "761", "Vehicle stability control (11 bit)"),
					new ValueTuple<string, string, string>("70E", "70F", "e-ACT brake controller (11 bit)"),
					new ValueTuple<string, string, string>("7CA", "7DA", "Inter-vehicle communication module (11 bit)"),
					new ValueTuple<string, string, string>("782", "7A2", "Audio amplifier (11 bit)"),
					new ValueTuple<string, string, string>("799", "7B9", "Hands free (11 bit)"),
					new ValueTuple<string, string, string>("7E7", "7EF", "Generator control module (11 bit)"),
					new ValueTuple<string, string, string>("796", "7B6", "Lithium-ion battery controller (11 bit)"),
					new ValueTuple<string, string, string>("7B0", "7B8", "Electronically controlled brake system (11 bit)"),
					new ValueTuple<string, string, string>("7E4", "7EC", "HCM (11 bit)"),
					new ValueTuple<string, string, string>("79C", "7BC", "HAS (11 bit)"),
					new ValueTuple<string, string, string>("7E6", "7EE", "ADCM (11 bit)"),
					new ValueTuple<string, string, string>("758", "778", "TPMS (11 bit)")
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
				foreach (ValueTuple<string, string, string> valueTuple in array)
				{
					string item = valueTuple.Item1;
					string item2 = valueTuple.Item2;
					NissanCANECU nissanCANECU = new NissanCANECU(valueTuple.Item3, item, item2);
					list.Add(nissanCANECU);
				}
				list2 = list;
			}
			catch (Exception ex)
			{
				throw ex;
			}
			return list2;
		}

		// Token: 0x170012E5 RID: 4837
		// (get) Token: 0x0600316E RID: 12654 RVA: 0x0022275C File Offset: 0x0022095C
		public static IReadOnlyList<IECU> ECUs
		{
			get
			{
				return NissanCANECU.BuildList();
			}
		}
	}
}
