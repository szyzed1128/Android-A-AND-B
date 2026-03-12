using System;
using System.Collections.Generic;
using CarScannerXamarinForms.OBD2;

namespace CarScannerXamarinForms.ECUModels
{
	// Token: 0x020004FC RID: 1276
	internal class MazdaECUCAN11bit : CAN11bitECU
	{
		// Token: 0x06003160 RID: 12640 RVA: 0x00220C48 File Offset: 0x0021EE48
		public MazdaECUCAN11bit(string name, string request, string response)
		{
			this.Name = name;
			base.RequestHeader = request;
			base.ResponseHeader = response;
			this.Protocol = 6;
			base.OpenSessionCommands = new List<string>();
			base.ReadDTCCommands = new List<string> { "19028F", "1902AC", "1800FF00", "1802FF00" };
			base.ClearDTCCommands = new List<string> { "14FFFFFF", "14FF00", "14", "14FFFF" };
			base.CloseSessionCommands = new List<string>();
			this.AddUDSIdents();
			this.AddKWP2000Idents();
		}

		// Token: 0x06003161 RID: 12641 RVA: 0x00220D08 File Offset: 0x0021EF08
		private static List<IECU> BuildList()
		{
			ValueTuple<string, string>[] array = new ValueTuple<string, string>[]
			{
				new ValueTuple<string, string>("7E0", CAN11bitECU.ECU_ENGINE_NAME),
				new ValueTuple<string, string>("7E1", CAN11bitECU.ECU_TRANSMISSION_NAME),
				new ValueTuple<string, string>("760", CAN11bitECU.ECU_ABS_NAME),
				new ValueTuple<string, string>("720", "Dashboard"),
				new ValueTuple<string, string>("726", "BCM"),
				new ValueTuple<string, string>("730", "Steering"),
				new ValueTuple<string, string>("731", "Access system"),
				new ValueTuple<string, string>("734", "Adaptive headlights"),
				new ValueTuple<string, string>("737", "SRS/Airbag"),
				new ValueTuple<string, string>("744", "Driver seat"),
				new ValueTuple<string, string>("754", "DCM"),
				new ValueTuple<string, string>("756", "Parking brake"),
				new ValueTuple<string, string>("761", "4WD"),
				new ValueTuple<string, string>("775", "Rear door"),
				new ValueTuple<string, string>("784", "Multimedia"),
				new ValueTuple<string, string>("7AC", "Amplifier")
			};
			List<IECU> list = new List<IECU>();
			list.Add(new OBD2Can11bitECU
			{
				ReadDTCCommands = new List<string> { "03", "03", "07", "07", "0A", "190208", "1902AC", "1902AF", "190278" },
				ClearDTCCommands = new List<string> { "04", "04", "04", "14FFFFFF", "14", "14FF00" }
			});
			foreach (ValueTuple<string, string> valueTuple in array)
			{
				string item = valueTuple.Item1;
				string item2 = valueTuple.Item2;
				string possibleResponseHeader = CAN11bitHelper.GetPossibleResponseHeader(item, "Mazda", null);
				MazdaECUCAN11bit mazdaECUCAN11bit = new MazdaECUCAN11bit(item2, item, possibleResponseHeader);
				list.Add(mazdaECUCAN11bit);
			}
			return list;
		}

		// Token: 0x170012E1 RID: 4833
		// (get) Token: 0x06003162 RID: 12642 RVA: 0x00220F97 File Offset: 0x0021F197
		public static IReadOnlyList<IECU> ECUs
		{
			get
			{
				return MazdaECUCAN11bit.BuildList();
			}
		}
	}
}
