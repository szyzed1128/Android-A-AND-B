using System;
using System.Collections.Generic;

namespace CarScannerXamarinForms.ECUModels
{
	// Token: 0x020004FE RID: 1278
	internal class MGZSEV : CAN11bitECU
	{
		// Token: 0x06003166 RID: 12646 RVA: 0x002212BC File Offset: 0x0021F4BC
		public MGZSEV(string name, string request, string response)
		{
			this.Name = name;
			base.RequestHeader = request;
			base.ResponseHeader = response;
			this.Protocol = 6;
			base.OpenSessionCommands = new List<string>(0);
			base.ReadDTCCommands = new List<string> { "1902AF", "19020C" };
			base.ClearDTCCommands = new List<string> { "14FFFFFF" };
			base.CloseSessionCommands = new List<string>();
			this.AddUDSIdents();
		}

		// Token: 0x06003167 RID: 12647 RVA: 0x00221340 File Offset: 0x0021F540
		private static List<IECU> BuildList()
		{
			ValueTuple<string, string, string>[] array = new ValueTuple<string, string, string>[]
			{
				new ValueTuple<string, string, string>("710", "718", "Gateway"),
				new ValueTuple<string, string, string>("711", "719", "Telematics"),
				new ValueTuple<string, string, string>("720", "728", "ABS"),
				new ValueTuple<string, string, string>("721", "729", "Electronic Power Steering"),
				new ValueTuple<string, string, string>("723", "72B", "Electronic Park Brake"),
				new ValueTuple<string, string, string>("724", "72C", "Tyre Pressure Monitoring"),
				new ValueTuple<string, string, string>("730", "738", "SRS"),
				new ValueTuple<string, string, string>("732", "73A", "Rear cross traffic"),
				new ValueTuple<string, string, string>("733", "73B", "Forward vehicle collision mitigation"),
				new ValueTuple<string, string, string>("734", "73C", "Front radar"),
				new ValueTuple<string, string, string>("740", "748", "BCM"),
				new ValueTuple<string, string, string>("742", "74A", "Electronic Steering Lock"),
				new ValueTuple<string, string, string>("745", "74D", "Passive Entry"),
				new ValueTuple<string, string, string>("750", "758", "Heating/Cooling (HVAC)"),
				new ValueTuple<string, string, string>("760", "768", "Instrument Panel"),
				new ValueTuple<string, string, string>("771", "779", "Gear selector"),
				new ValueTuple<string, string, string>("781", "789", "Battery Management System (HV)"),
				new ValueTuple<string, string, string>("782", "78A", "Inverter control"),
				new ValueTuple<string, string, string>("784", "78C", "Charger"),
				new ValueTuple<string, string, string>("785", "78D", "DC to DC converter"),
				new ValueTuple<string, string, string>("7E3", "7EB", "Vehicle Control")
			};
			List<IECU> list = new List<IECU>();
			list.Add(new OBD2Can11bitECU
			{
				ReadDTCCommands = new List<string> { "03", "03", "07", "07", "0A", "1902AF", "1902AC" },
				ClearDTCCommands = new List<string> { "04", "04", "14", "14FFFFFF" }
			});
			foreach (ValueTuple<string, string, string> valueTuple in array)
			{
				string item = valueTuple.Item1;
				string item2 = valueTuple.Item2;
				MGZSEV mgzsev = new MGZSEV(valueTuple.Item3, item, item2);
				list.Add(mgzsev);
			}
			return list;
		}

		// Token: 0x170012E3 RID: 4835
		// (get) Token: 0x06003168 RID: 12648 RVA: 0x00221678 File Offset: 0x0021F878
		public static IReadOnlyList<IECU> ECUs
		{
			get
			{
				return MGZSEV.BuildList();
			}
		}
	}
}
