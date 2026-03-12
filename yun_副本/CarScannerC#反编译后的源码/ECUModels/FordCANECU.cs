using System;
using System.Collections.Generic;
using CarScannerXamarinForms.OBD2;

namespace CarScannerXamarinForms.ECUModels
{
	// Token: 0x020004D2 RID: 1234
	internal class FordCANECU : CAN11bitECU
	{
		// Token: 0x06003089 RID: 12425 RVA: 0x00219C1C File Offset: 0x00217E1C
		public FordCANECU(string name, string request, string response)
		{
			this.Name = name;
			base.RequestHeader = request;
			base.ResponseHeader = response;
			this.Protocol = 6;
			base.OpenSessionCommands = new List<string>();
			base.ReadDTCCommands = new List<string> { "1902AF", "1902AC", "190223", "190278", "1800FF00", "1802FF00", "18FF00", "17FF00", "13FF00" };
			base.ClearDTCCommands = new List<string> { "14", "14FF00", "14FFFF", "14FFFFFF" };
			base.CloseSessionCommands = new List<string>();
			base.IdentsASCII.TryAdd("22F113", "Assembly ver.");
			base.IdentsASCII.TryAdd("22F188", "ECU Software Number.");
			base.DefaultTestEcuExistsCommands.Add("220200");
			this.AddUDSIdents();
			this.AddKWP2000Idents();
		}

		// Token: 0x0600308A RID: 12426 RVA: 0x00219D50 File Offset: 0x00217F50
		private static List<IECU> BuildList()
		{
			ValueTuple<string, string>[] array = new ValueTuple<string, string>[]
			{
				new ValueTuple<string, string>("7E0", CAN11bitECU.ECU_ENGINE_NAME),
				new ValueTuple<string, string>("7E1", CAN11bitECU.ECU_TRANSMISSION_NAME),
				new ValueTuple<string, string>("760", "ABS/ESP"),
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
				new ValueTuple<string, string>("725", "WACM"),
				new ValueTuple<string, string>("703", "AWD")
			};
			List<IECU> list = new List<IECU>();
			list.Add(new OBD2Can11bitECU
			{
				ReadDTCCommands = new List<string>
				{
					"03", "03", "07", "07", "0A", "190208", "1902AC", "1902AF", "190223", "190278",
					"1800FF00"
				},
				ClearDTCCommands = new List<string> { "04", "04", "04", "14FFFFFF", "14", "14FF00" }
			});
			foreach (ValueTuple<string, string> valueTuple in array)
			{
				string item = valueTuple.Item1;
				string item2 = valueTuple.Item2;
				string possibleResponseHeader = CAN11bitHelper.GetPossibleResponseHeader(item, "Ford", null);
				FordCANECU fordCANECU = new FordCANECU(item2, item, possibleResponseHeader);
				list.Add(fordCANECU);
			}
			return list;
		}

		// Token: 0x1700129E RID: 4766
		// (get) Token: 0x0600308B RID: 12427 RVA: 0x0021A2BE File Offset: 0x002184BE
		public static IReadOnlyList<IECU> ECUs
		{
			get
			{
				return FordCANECU.BuildList();
			}
		}
	}
}
