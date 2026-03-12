using System;
using System.Collections.Generic;
using CarScannerXamarinForms.OBD2;

namespace CarScannerXamarinForms.ECUModels
{
	// Token: 0x020004CF RID: 1231
	internal class DongFengCAN11bitECU : CAN11bitECU
	{
		// Token: 0x06003080 RID: 12416 RVA: 0x00218FDC File Offset: 0x002171DC
		public DongFengCAN11bitECU(string name, string request, string response)
		{
			this.Name = name;
			base.RequestHeader = request;
			base.ResponseHeader = response;
			this.Protocol = 6;
			base.OpenSessionCommands = new List<string>();
			base.ReadDTCCommands = new List<string> { "190101", "190104", "190108", "190109", "190201", "190208", "19020C" };
			base.ClearDTCCommands = new List<string> { "14FFFFFF", "14", "14FF00", "14FFFF" };
			base.CloseSessionCommands = new List<string>();
			base.IdentsASCII.TryAdd("22F113", "Assembly ver.");
			base.IdentsASCII.TryAdd("22F188", "ECU Software Number.");
			this.AddUDSIdents();
			this.AddKWP2000Idents();
		}

		// Token: 0x06003081 RID: 12417 RVA: 0x002190E8 File Offset: 0x002172E8
		private static List<IECU> BuildList()
		{
			ValueTuple<string, string>[] array = new ValueTuple<string, string>[]
			{
				new ValueTuple<string, string>("7E0", CAN11bitECU.ECU_ENGINE_NAME),
				new ValueTuple<string, string>("7E1", CAN11bitECU.ECU_TRANSMISSION_NAME),
				new ValueTuple<string, string>("722", CAN11bitECU.ECU_ABS_NAME),
				new ValueTuple<string, string>("720", "SRS/Airbag"),
				new ValueTuple<string, string>("702", "Instruments cluster"),
				new ValueTuple<string, string>("721", "Electric parking brake"),
				new ValueTuple<string, string>("723", "Electric power steering (EPS)"),
				new ValueTuple<string, string>("701", "BCM"),
				new ValueTuple<string, string>("703", "PEPS (Keyless start system)"),
				new ValueTuple<string, string>("724", "Around view monitor"),
				new ValueTuple<string, string>("725", "Radio entertainment system"),
				new ValueTuple<string, string>("726", "A/C"),
				new ValueTuple<string, string>("728", "Telematics box"),
				new ValueTuple<string, string>("73D", "DC-DC Converter"),
				new ValueTuple<string, string>("729", "Gear selector module")
			};
			List<IECU> list = new List<IECU>();
			list.Add(new OBD2Can11bitECU
			{
				ReadDTCCommands = new List<string>
				{
					"03", "03", "07", "07", "0A", "190101", "190104", "190108", "190109", "190201",
					"190208", "19020C"
				},
				ClearDTCCommands = new List<string> { "04", "04", "04", "14FFFFFF", "14", "14FF00" }
			});
			foreach (ValueTuple<string, string> valueTuple in array)
			{
				string item = valueTuple.Item1;
				string item2 = valueTuple.Item2;
				string possibleResponseHeader = CAN11bitHelper.GetPossibleResponseHeader(item, "DongFeng", null);
				DongFengCAN11bitECU dongFengCAN11bitECU = new DongFengCAN11bitECU(item2, item, possibleResponseHeader);
				list.Add(dongFengCAN11bitECU);
			}
			foreach (ValueTuple<string, string, string> valueTuple2 in new ValueTuple<string, string, string>[]
			{
				new ValueTuple<string, string, string>("702", "712", CAN11bitECU.ECU_ABS_NAME + " #2"),
				new ValueTuple<string, string, string>("740", "750", "Instrument cluster"),
				new ValueTuple<string, string, string>("7F1", "7F9", "SRS/Airbag #2"),
				new ValueTuple<string, string, string>("712", "792", "MCU"),
				new ValueTuple<string, string, string>("713", "793", "BMS"),
				new ValueTuple<string, string, string>("711", "791", "VCU"),
				new ValueTuple<string, string, string>("714", "794", "On-board charger"),
				new ValueTuple<string, string, string>("718", "798", "Electronic parking lock"),
				new ValueTuple<string, string, string>("717", "797", "Thermal management controller")
			})
			{
				string item3 = valueTuple2.Item1;
				string item4 = valueTuple2.Item2;
				DongFengCAN11bitECU dongFengCAN11bitECU2 = new DongFengCAN11bitECU(valueTuple2.Item3, item3, item4);
				list.Add(dongFengCAN11bitECU2);
			}
			foreach (IECU iecu in RenaultDaciaCAN11bitECU.ECUs)
			{
				iecu.Name += " (Renault based ECU)";
			}
			return list;
		}

		// Token: 0x1700129B RID: 4763
		// (get) Token: 0x06003082 RID: 12418 RVA: 0x00219518 File Offset: 0x00217718
		public static IReadOnlyList<IECU> ECUs
		{
			get
			{
				return DongFengCAN11bitECU.BuildList();
			}
		}
	}
}
