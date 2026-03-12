using System;
using System.Collections.Generic;

namespace CarScannerXamarinForms.ECUModels
{
	// Token: 0x020004D3 RID: 1235
	internal class GACCAN11bitECU : CAN11bitECU
	{
		// Token: 0x0600308C RID: 12428 RVA: 0x0021A2C8 File Offset: 0x002184C8
		public GACCAN11bitECU(string name, string request, string response)
		{
			this.Name = name;
			base.RequestHeader = request;
			base.ResponseHeader = response;
			this.Protocol = 6;
			base.OpenSessionCommands = new List<string> { "3E00" };
			base.ReadDTCCommands = new List<string> { "1902AF" };
			base.ClearDTCCommands = new List<string> { "14FFFFFF" };
			base.CloseSessionCommands = new List<string>();
			this.AddUDSIdents();
		}

		// Token: 0x0600308D RID: 12429 RVA: 0x0021A34C File Offset: 0x0021854C
		private static List<IECU> BuildList()
		{
			ValueTuple<string, string, string>[] array = new ValueTuple<string, string, string>[]
			{
				new ValueTuple<string, string, string>("7E0", "7E8", CAN11bitECU.ECU_ENGINE_NAME),
				new ValueTuple<string, string, string>("7E1", "7E9", CAN11bitECU.ECU_TRANSMISSION_NAME)
			};
			List<IECU> list = new List<IECU>();
			list.Add(new OBD2Can11bitECU
			{
				ReadDTCCommands = new List<string> { "03", "03", "07", "07", "0A", "1902AF" },
				ClearDTCCommands = new List<string> { "04", "04", "04", "14FFFFFF" }
			});
			foreach (ValueTuple<string, string, string> valueTuple in array)
			{
				string item = valueTuple.Item1;
				string item2 = valueTuple.Item2;
				GACCAN11bitECU gaccan11bitECU = new GACCAN11bitECU(valueTuple.Item3, item, item2);
				list.Add(gaccan11bitECU);
			}
			return list;
		}

		// Token: 0x1700129F RID: 4767
		// (get) Token: 0x0600308E RID: 12430 RVA: 0x0021A46B File Offset: 0x0021866B
		public static IReadOnlyList<IECU> ECUs
		{
			get
			{
				return GACCAN11bitECU.BuildList();
			}
		}
	}
}
