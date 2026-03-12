using System;
using System.Collections.Generic;
using CarScannerXamarinForms.OBD2;

namespace CarScannerXamarinForms.ECUModels
{
	// Token: 0x020004FA RID: 1274
	internal class LiAutoCAN11bit : CAN11bitECU
	{
		// Token: 0x0600315A RID: 12634 RVA: 0x0022048C File Offset: 0x0021E68C
		public LiAutoCAN11bit(string name, string request)
		{
			this.Name = name;
			base.RequestHeader = request;
			base.ResponseHeader = CAN11bitHelper.GetPossibleResponseHeader(request, "Li Auto", null);
			base.OpenSessionCommands = new List<string>();
			base.ReadDTCCommands = new List<string> { "19020F" };
			base.ClearDTCCommands = new List<string> { "14FFFFFF" };
			base.CloseSessionCommands = new List<string>();
			this.AddUDSIdents();
		}

		// Token: 0x0600315B RID: 12635 RVA: 0x00220508 File Offset: 0x0021E708
		private static List<IECU> BuildList()
		{
			ValueTuple<string, string>[] array = new ValueTuple<string, string>[]
			{
				new ValueTuple<string, string>("7E0", CAN11bitECU.ECU_ENGINE_NAME),
				new ValueTuple<string, string>("7E3", "Vehicle Control Unit"),
				new ValueTuple<string, string>("720", "Dashboard"),
				new ValueTuple<string, string>("7C7", "ACCM"),
				new ValueTuple<string, string>("727", "ACM"),
				new ValueTuple<string, string>("7D0", "APIM"),
				new ValueTuple<string, string>("726", "BCM"),
				new ValueTuple<string, string>("6F0", "BCMC/BJB"),
				new ValueTuple<string, string>("7E4", "BECM"),
				new ValueTuple<string, string>("723", "BECMB"),
				new ValueTuple<string, string>("764", "CCM"),
				new ValueTuple<string, string>("7C1", "CMR"),
				new ValueTuple<string, string>("746", "DCDC"),
				new ValueTuple<string, string>("7A2", "DCME"),
				new ValueTuple<string, string>("762", "SCMF"),
				new ValueTuple<string, string>("7B3", "DCMG"),
				new ValueTuple<string, string>("7B4", "DCMH"),
				new ValueTuple<string, string>("740", "DDM"),
				new ValueTuple<string, string>("744", "DSM"),
				new ValueTuple<string, string>("783", "DSP"),
				new ValueTuple<string, string>("7A1", "GFM (FTRM)"),
				new ValueTuple<string, string>("732", "GSM"),
				new ValueTuple<string, string>("716", "GWM"),
				new ValueTuple<string, string>("734", "HCM"),
				new ValueTuple<string, string>("733", "HVAC"),
				new ValueTuple<string, string>("706", "IPMA"),
				new ValueTuple<string, string>("7D1", "OBCC"),
				new ValueTuple<string, string>("765", "OCS"),
				new ValueTuple<string, string>("750", "PACM"),
				new ValueTuple<string, string>("741", "PDM"),
				new ValueTuple<string, string>("730", "PSCM"),
				new ValueTuple<string, string>("737", "RCM"),
				new ValueTuple<string, string>("731", "RFA"),
				new ValueTuple<string, string>("775", "RGTM"),
				new ValueTuple<string, string>("724", "SCCM"),
				new ValueTuple<string, string>("7E2", "SOBDM (BCCM)"),
				new ValueTuple<string, string>("795", "SOBDMB"),
				new ValueTuple<string, string>("7E6", "SOBDMC"),
				new ValueTuple<string, string>("6F2", "SODCMC"),
				new ValueTuple<string, string>("6F3", "SODCMD"),
				new ValueTuple<string, string>("7C4", "SODL"),
				new ValueTuple<string, string>("7C6", "SODR"),
				new ValueTuple<string, string>("754", "TCU"),
				new ValueTuple<string, string>("721", "VDM"),
				new ValueTuple<string, string>("725", "WACM")
			};
			List<IECU> list = new List<IECU>();
			list.Add(new OBD2Can11bitECU
			{
				ReadDTCCommands = new List<string> { "03", "03", "07", "07", "0A", "19020F" },
				ClearDTCCommands = new List<string> { "04", "04", "04", "14FFFFFF" }
			});
			foreach (ValueTuple<string, string> valueTuple in array)
			{
				string item = valueTuple.Item1;
				LiAutoCAN11bit liAutoCAN11bit = new LiAutoCAN11bit(valueTuple.Item2, item);
				list.Add(liAutoCAN11bit);
			}
			return list;
		}

		// Token: 0x170012DF RID: 4831
		// (get) Token: 0x0600315C RID: 12636 RVA: 0x002209EA File Offset: 0x0021EBEA
		public static IReadOnlyList<IECU> ECUs
		{
			get
			{
				return LiAutoCAN11bit.BuildList();
			}
		}
	}
}
