using System;
using System.Collections.Generic;
using CarScannerXamarinForms.OBD2;

namespace CarScannerXamarinForms.ECUModels
{
	// Token: 0x020004DD RID: 1245
	internal class HongqiCANECU : CAN11bitECU
	{
		// Token: 0x060030AD RID: 12461 RVA: 0x0021BE08 File Offset: 0x0021A008
		public HongqiCANECU(string name, string request)
		{
			this.Name = name;
			base.RequestHeader = request;
			base.ResponseHeader = CAN11bitHelper.GetPossibleResponseHeader(request, "Hongqi", null);
			this.Protocol = 6;
			base.OpenSessionCommands = new List<string>();
			base.ReadDTCCommands = new List<string> { "19020C" };
			base.ClearDTCCommands = new List<string> { "14FFFFFF" };
			base.CloseSessionCommands = new List<string>();
			this.AddUDSIdents();
		}

		// Token: 0x060030AE RID: 12462 RVA: 0x0021BE8C File Offset: 0x0021A08C
		private static List<IECU> BuildList()
		{
			ValueTuple<string, string>[] array = new ValueTuple<string, string>[]
			{
				new ValueTuple<string, string>("7E0", CAN11bitECU.ECU_ENGINE_NAME),
				new ValueTuple<string, string>("7E1", CAN11bitECU.ECU_TRANSMISSION_NAME),
				new ValueTuple<string, string>("750", "BCM"),
				new ValueTuple<string, string>("76F", "Digital Key"),
				new ValueTuple<string, string>("773", "T-Box"),
				new ValueTuple<string, string>("780", "Gateway"),
				new ValueTuple<string, string>("7D1", "ACM"),
				new ValueTuple<string, string>("720", "SRS/Airbag"),
				new ValueTuple<string, string>("770", "IVI"),
				new ValueTuple<string, string>("771", "IVI2")
			};
			List<IECU> list = new List<IECU>();
			list.Add(new OBD2Can11bitECU
			{
				ReadDTCCommands = new List<string> { "03", "03", "07", "07", "0A", "190208", "1902AC", "1902AF", "190223", "190278" },
				ClearDTCCommands = new List<string> { "04", "04", "04", "14FFFFFF", "14", "14FF00" }
			});
			foreach (ValueTuple<string, string> valueTuple in array)
			{
				string item = valueTuple.Item1;
				string item2 = valueTuple.Item2;
				CAN11bitHelper.GetPossibleResponseHeader(item, "Hongqi", null);
				HongqiCANECU hongqiCANECU = new HongqiCANECU(item2, item);
				list.Add(hongqiCANECU);
			}
			return list;
		}

		// Token: 0x170012A6 RID: 4774
		// (get) Token: 0x060030AF RID: 12463 RVA: 0x0021C099 File Offset: 0x0021A299
		public static IReadOnlyList<IECU> ECUs
		{
			get
			{
				return HongqiCANECU.BuildList();
			}
		}
	}
}
