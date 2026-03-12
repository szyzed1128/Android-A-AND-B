using System;
using System.Collections.Generic;
using CarScannerXamarinForms.OBD2;

namespace CarScannerXamarinForms.ECUModels
{
	// Token: 0x020004FB RID: 1275
	internal class LifanECUCAN11bit : CAN11bitECU
	{
		// Token: 0x0600315D RID: 12637 RVA: 0x002209F4 File Offset: 0x0021EBF4
		public LifanECUCAN11bit(string name, string request, string response)
		{
			this.Name = name;
			base.RequestHeader = request;
			base.ResponseHeader = response;
			this.Protocol = 6;
			base.OpenSessionCommands = new List<string>();
			base.ReadDTCCommands = new List<string> { "1902AC", "190208", "1902AF", "1902AB", "1800FF00" };
			base.ClearDTCCommands = new List<string> { "14FFFFFF", "14FF00", "14", "14FFFF" };
			base.CloseSessionCommands = new List<string>();
			this.AddUDSIdents();
			this.AddKWP2000Idents();
		}

		// Token: 0x0600315E RID: 12638 RVA: 0x00220AC0 File Offset: 0x0021ECC0
		private static List<IECU> BuildList()
		{
			ValueTuple<string, string>[] array = new ValueTuple<string, string>[]
			{
				new ValueTuple<string, string>("7E0", CAN11bitECU.ECU_ENGINE_NAME),
				new ValueTuple<string, string>("7E1", CAN11bitECU.ECU_TRANSMISSION_NAME),
				new ValueTuple<string, string>("710", CAN11bitECU.ECU_ABS_NAME + " (BOSCH ABS9/ESP9 CAN)")
			};
			List<IECU> list = new List<IECU>();
			list.Add(new OBD2Can11bitECU
			{
				ReadDTCCommands = new List<string> { "03", "03", "07", "07", "0A", "190208", "1902AC", "1902AF", "1902AB", "1800FF00" },
				ClearDTCCommands = new List<string> { "04", "04", "04", "14FFFFFF", "14", "14FF00" }
			});
			foreach (ValueTuple<string, string> valueTuple in array)
			{
				string item = valueTuple.Item1;
				string item2 = valueTuple.Item2;
				string possibleResponseHeader = CAN11bitHelper.GetPossibleResponseHeader(item, "Lifan", null);
				LifanECUCAN11bit lifanECUCAN11bit = new LifanECUCAN11bit(item2, item, possibleResponseHeader);
				list.Add(lifanECUCAN11bit);
			}
			return list;
		}

		// Token: 0x170012E0 RID: 4832
		// (get) Token: 0x0600315F RID: 12639 RVA: 0x00220C3E File Offset: 0x0021EE3E
		public static IReadOnlyList<IECU> ECUs
		{
			get
			{
				return LifanECUCAN11bit.BuildList();
			}
		}
	}
}
