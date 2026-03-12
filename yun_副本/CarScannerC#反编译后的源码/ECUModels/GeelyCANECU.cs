using System;
using System.Collections.Generic;
using CarScannerXamarinForms.OBD2;

namespace CarScannerXamarinForms.ECUModels
{
	// Token: 0x020004D4 RID: 1236
	internal class GeelyCANECU : CAN11bitECU
	{
		// Token: 0x0600308F RID: 12431 RVA: 0x0021A474 File Offset: 0x00218674
		public GeelyCANECU(string name, string request)
		{
			this.Name = name;
			base.RequestHeader = request;
			base.ResponseHeader = CAN11bitHelper.GetPossibleResponseHeader(request, "Geely", null);
			this.Protocol = 6;
			base.OpenSessionCommands = new List<string> { "1001" };
			base.ReadDTCCommands = new List<string> { "190209", "190101", "1800FF00" };
			base.ClearDTCCommands = new List<string> { "14FFFFFF" };
			base.CloseSessionCommands = new List<string>();
			this.AddUDSIdents();
		}

		// Token: 0x06003090 RID: 12432 RVA: 0x0021A518 File Offset: 0x00218718
		private static List<IECU> BuildList()
		{
			ValueTuple<string, string>[] array = new ValueTuple<string, string>[]
			{
				new ValueTuple<string, string>("7E0", CAN11bitECU.ECU_ENGINE_NAME),
				new ValueTuple<string, string>("7E1", CAN11bitECU.ECU_TRANSMISSION_NAME),
				new ValueTuple<string, string>("7E2", "Battery management System"),
				new ValueTuple<string, string>("7E3", "Instruments panel/Dashboard (IPK) #2/BSG"),
				new ValueTuple<string, string>("7E4", CAN11bitECU.ECU_ABS_NAME),
				new ValueTuple<string, string>("7E5", "SRS/Airbags"),
				new ValueTuple<string, string>("7E6", "VCU"),
				new ValueTuple<string, string>("714", "Transmission actuator system"),
				new ValueTuple<string, string>("720", "BCM"),
				new ValueTuple<string, string>("721", "Instruments panel/Dashboard (IPK) #1"),
				new ValueTuple<string, string>("737", "TPMS"),
				new ValueTuple<string, string>("746", "Seat ventilation and heating"),
				new ValueTuple<string, string>("791", "Front radar"),
				new ValueTuple<string, string>("792", "Adaptive lights / Steering column combination switch"),
				new ValueTuple<string, string>("793", "Front camera system"),
				new ValueTuple<string, string>("794", "Electronic shifter (EGSM)"),
				new ValueTuple<string, string>("795", "Easy entry (PEPS)"),
				new ValueTuple<string, string>("796", "4WD"),
				new ValueTuple<string, string>("7A0", "Telematics"),
				new ValueTuple<string, string>("7A1", "Camera"),
				new ValueTuple<string, string>("7A4", "Electric parking brake"),
				new ValueTuple<string, string>("7A5", "Parking assistant (PAS)"),
				new ValueTuple<string, string>("7A7", "Gateway"),
				new ValueTuple<string, string>("7A9", "360 camera"),
				new ValueTuple<string, string>("7B4", "DC DC Converter"),
				new ValueTuple<string, string>("7C1", "Multimedia"),
				new ValueTuple<string, string>("7C6", "A/C, Heater #2"),
				new ValueTuple<string, string>("7C9", "Multimedia (MMI)"),
				new ValueTuple<string, string>("7D1", "Steering column lock system"),
				new ValueTuple<string, string>("7D2", "Power steering")
			};
			List<IECU> list = new List<IECU>();
			list.Add(new OBD2Can11bitECU
			{
				ReadDTCCommands = new List<string> { "03", "03", "07", "07", "0A", "1902AF", "190209" },
				ClearDTCCommands = new List<string> { "04", "04", "14", "14FFFFFF" }
			});
			foreach (ValueTuple<string, string> valueTuple in array)
			{
				string item = valueTuple.Item1;
				GeelyCANECU geelyCANECU = new GeelyCANECU(valueTuple.Item2, item);
				list.Add(geelyCANECU);
			}
			return list;
		}

		// Token: 0x170012A0 RID: 4768
		// (get) Token: 0x06003091 RID: 12433 RVA: 0x0021A8AC File Offset: 0x00218AAC
		public static IReadOnlyList<IECU> ECUs
		{
			get
			{
				return GeelyCANECU.BuildList();
			}
		}
	}
}
