using System;
using System.Collections.Generic;
using CarScannerXamarinForms.OBD2;

namespace CarScannerXamarinForms.ECUModels
{
	// Token: 0x020004E4 RID: 1252
	internal class JACCAN11bitECU : CAN11bitECU
	{
		// Token: 0x060030DB RID: 12507 RVA: 0x0021CF1C File Offset: 0x0021B11C
		public JACCAN11bitECU(string name, string request)
		{
			this.Name = name;
			base.RequestHeader = request;
			base.ResponseHeader = CAN11bitHelper.GetPossibleResponseHeader(request, "JAC", null);
			this.Protocol = 6;
			base.ReadDTCCommands = new List<string> { "190201", "190208", "1800FF00" };
			base.ClearDTCCommands = new List<string> { "14FFFFFF" };
			base.CloseSessionCommands = new List<string>();
			this.AddUDSIdents();
		}

		// Token: 0x060030DC RID: 12508 RVA: 0x0021CFAC File Offset: 0x0021B1AC
		private static List<IECU> BuildList()
		{
			ValueTuple<string, string>[] array = new ValueTuple<string, string>[]
			{
				new ValueTuple<string, string>("7E0", CAN11bitECU.ECU_ENGINE_NAME),
				new ValueTuple<string, string>("7E1", CAN11bitECU.ECU_TRANSMISSION_NAME),
				new ValueTuple<string, string>("742", CAN11bitECU.ECU_ABS_NAME),
				new ValueTuple<string, string>("741", "EPS"),
				new ValueTuple<string, string>("744", "SRS/Airbag"),
				new ValueTuple<string, string>("744", "BCM")
			};
			List<IECU> list = new List<IECU>();
			list.Add(new OBD2Can11bitECU
			{
				ReadDTCCommands = new List<string> { "03", "03", "07", "07", "0A", "190201", "190208" },
				ClearDTCCommands = new List<string> { "04", "04", "14", "14FFFFFF" }
			});
			foreach (ValueTuple<string, string> valueTuple in array)
			{
				string item = valueTuple.Item1;
				JACCAN11bitECU jaccan11bitECU = new JACCAN11bitECU(valueTuple.Item2, item);
				list.Add(jaccan11bitECU);
			}
			return list;
		}

		// Token: 0x170012BF RID: 4799
		// (get) Token: 0x060030DD RID: 12509 RVA: 0x0021D11A File Offset: 0x0021B31A
		public static IReadOnlyList<IECU> ECUs
		{
			get
			{
				return JACCAN11bitECU.BuildList();
			}
		}
	}
}
