using System;
using System.Collections.Generic;
using CarScannerXamarinForms.OBD2;

namespace CarScannerXamarinForms.ECUModels
{
	// Token: 0x02000519 RID: 1305
	internal class SsangYongCANECU : CAN11bitECU
	{
		// Token: 0x060031BA RID: 12730 RVA: 0x00225068 File Offset: 0x00223268
		public SsangYongCANECU(string name, string request)
		{
			this.Name = name;
			base.RequestHeader = request;
			base.ResponseHeader = CAN11bitHelper.GetPossibleResponseHeader(request, "SsangYong", null);
			this.Protocol = 6;
			base.OpenSessionCommands = new List<string>();
			base.ReadDTCCommands = new List<string> { "1800FF00", "1902AC", "19020E" };
			base.ClearDTCCommands = new List<string> { "04", "14", "14FF00", "14FFFF", "14FFFFFF" };
			base.CloseSessionCommands = new List<string>();
		}

		// Token: 0x170012ED RID: 4845
		// (get) Token: 0x060031BB RID: 12731 RVA: 0x00225126 File Offset: 0x00223326
		public static IReadOnlyList<IECU> ECUs
		{
			get
			{
				return SsangYongCANECU.BuildList();
			}
		}

		// Token: 0x060031BC RID: 12732 RVA: 0x00225130 File Offset: 0x00223330
		private static List<IECU> BuildList()
		{
			ValueTuple<string, string>[] array = new ValueTuple<string, string>[]
			{
				new ValueTuple<string, string>("7E0", CAN11bitECU.ECU_ENGINE_NAME),
				new ValueTuple<string, string>("7E1", CAN11bitECU.ECU_TRANSMISSION_NAME),
				new ValueTuple<string, string>("7C7", "4WD"),
				new ValueTuple<string, string>("7C0", "ABS"),
				new ValueTuple<string, string>("7E2", "EPS"),
				new ValueTuple<string, string>("7D2", "TPMS"),
				new ValueTuple<string, string>("7D3", "SRS"),
				new ValueTuple<string, string>("7C5", "Smart key"),
				new ValueTuple<string, string>("701", "BCM")
			};
			List<IECU> list = new List<IECU>();
			list.Add(new OBD2Can11bitECU
			{
				ReadDTCCommands = new List<string> { "03", "03", "07", "07", "0A", "1800FF00", "1802FF00", "1902AC", "19020E" },
				ClearDTCCommands = new List<string> { "04", "04", "04", "14", "14FF00", "14FFFF", "14FFFFFF" }
			});
			foreach (ValueTuple<string, string> valueTuple in array)
			{
				string item = valueTuple.Item1;
				SsangYongCANECU ssangYongCANECU = new SsangYongCANECU(valueTuple.Item2, item);
				list.Add(ssangYongCANECU);
			}
			return list;
		}
	}
}
