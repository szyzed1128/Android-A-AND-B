using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using CarScannerXamarinForms.OBD2;

namespace CarScannerXamarinForms.ECUModels
{
	// Token: 0x02000539 RID: 1337
	internal class VolvoCAN11bitECU : CAN11bitECU
	{
		// Token: 0x06003229 RID: 12841 RVA: 0x0022BE38 File Offset: 0x0022A038
		public VolvoCAN11bitECU(string name, string request, string response, bool ms_can)
		{
			this.Name = name;
			base.RequestHeader = request;
			base.ResponseHeader = response;
			this.Protocol = 6;
			base.OpenSessionCommands = new List<string>();
			this.MS_CAN = ms_can;
			base.ReadDTCCommands = new List<string> { "1902AF", "19028D" };
			base.ClearDTCCommands = new List<string> { "14FFFFFF" };
			base.CloseSessionCommands = new List<string>();
			this.AddUDSIdents();
		}

		// Token: 0x0600322A RID: 12842 RVA: 0x0022BEC4 File Offset: 0x0022A0C4
		public override OBDRequest GetRequestForCommand(string cmd)
		{
			OBDRequest requestForCommand = base.GetRequestForCommand(cmd);
			if (this.MS_CAN)
			{
				List<string> list = requestForCommand.BeforeCommands.ToList<string>();
				list.RemoveAll((string x) => x.StartsWith("ATSP"));
				list.Add("STP53");
				requestForCommand.BeforeCommands = list.ToArray();
				List<string> list2 = requestForCommand.AfterCommands.ToList<string>();
				list2.Add("ATSP6");
				requestForCommand.AfterCommands = list2.ToArray();
			}
			return requestForCommand;
		}

		// Token: 0x0600322B RID: 12843 RVA: 0x0022BF50 File Offset: 0x0022A150
		private static List<IECU> BuildList()
		{
			ValueTuple<string, string>[] array = new ValueTuple<string, string>[]
			{
				new ValueTuple<string, string>("7E0", CAN11bitECU.ECU_ENGINE_NAME),
				new ValueTuple<string, string>("7E1", CAN11bitECU.ECU_TRANSMISSION_NAME),
				new ValueTuple<string, string>("7E7", "RDCM"),
				new ValueTuple<string, string>("793", "CVM"),
				new ValueTuple<string, string>("795", "4WD/HALDEX"),
				new ValueTuple<string, string>("726", "CEM"),
				new ValueTuple<string, string>("760", "BCM"),
				new ValueTuple<string, string>("737", "SRS (HS-CAN)"),
				new ValueTuple<string, string>("736", "PAM"),
				new ValueTuple<string, string>("764", "FSM"),
				new ValueTuple<string, string>("794", "PPM"),
				new ValueTuple<string, string>("720", "DIM (HS-CAN)"),
				new ValueTuple<string, string>("756", "PBM"),
				new ValueTuple<string, string>("797", "SAS"),
				new ValueTuple<string, string>("730", "PSCM"),
				new ValueTuple<string, string>("7C7", "ACCM"),
				new ValueTuple<string, string>("746", "DCDC"),
				new ValueTuple<string, string>("753", "OBC"),
				new ValueTuple<string, string>("7E4", "BECM"),
				new ValueTuple<string, string>("745", "ISC/ERAD"),
				new ValueTuple<string, string>("747", "ISG"),
				new ValueTuple<string, string>("727", "IAM (HS)")
			};
			ValueTuple<string, string>[] array2 = new ValueTuple<string, string>[]
			{
				new ValueTuple<string, string>("737", "SRS"),
				new ValueTuple<string, string>("720", "DIM"),
				new ValueTuple<string, string>("731", "KVM"),
				new ValueTuple<string, string>("7C4", "SODL"),
				new ValueTuple<string, string>("7C6", "SODR"),
				new ValueTuple<string, string>("7C1", "PAC"),
				new ValueTuple<string, string>("7A4", "AUD"),
				new ValueTuple<string, string>("727", "IAM"),
				new ValueTuple<string, string>("7D6", "DABM"),
				new ValueTuple<string, string>("707", "TVM"),
				new ValueTuple<string, string>("784", "ICM"),
				new ValueTuple<string, string>("754", "PHM"),
				new ValueTuple<string, string>("740", "DDM"),
				new ValueTuple<string, string>("741", "PDM"),
				new ValueTuple<string, string>("744", "PSM"),
				new ValueTuple<string, string>("733", "CCM"),
				new ValueTuple<string, string>("7E3", "CPM"),
				new ValueTuple<string, string>("791", "TRM"),
				new ValueTuple<string, string>("775", "TAILGATE")
			};
			List<IECU> list = new List<IECU>();
			list.Add(new OBD2Can11bitECU
			{
				ReadDTCCommands = new List<string> { "03", "03", "07", "07", "0A", "190208", "1902AC", "1902AF", "190FAC" },
				ClearDTCCommands = new List<string> { "04", "04", "04", "14FFFFFF", "14", "14FF00" }
			});
			foreach (ValueTuple<string, string> valueTuple in array)
			{
				string item = valueTuple.Item1;
				string item2 = valueTuple.Item2;
				string possibleResponseHeader = CAN11bitHelper.GetPossibleResponseHeader(item, "Volvo", null);
				VolvoCAN11bitECU volvoCAN11bitECU = new VolvoCAN11bitECU("[P3/VEA] " + item2, item, possibleResponseHeader, false);
				list.Add(volvoCAN11bitECU);
			}
			if (App.OBDReader.STCommandsStupported)
			{
				foreach (ValueTuple<string, string> valueTuple2 in array2)
				{
					string item3 = valueTuple2.Item1;
					string item4 = valueTuple2.Item2;
					string possibleResponseHeader2 = CAN11bitHelper.GetPossibleResponseHeader(item3, "Volvo", null);
					VolvoCAN11bitECU volvoCAN11bitECU2 = new VolvoCAN11bitECU("[P3/VEA] " + item4 + " (MS-CAN)", item3, possibleResponseHeader2, true);
					list.Add(volvoCAN11bitECU2);
				}
			}
			return list;
		}

		// Token: 0x170012FB RID: 4859
		// (get) Token: 0x0600322C RID: 12844 RVA: 0x0022C4A4 File Offset: 0x0022A6A4
		public static IReadOnlyList<IECU> ECUs
		{
			get
			{
				return VolvoCAN11bitECU.BuildList();
			}
		}

		// Token: 0x04001D28 RID: 7464
		public readonly bool MS_CAN;

		// Token: 0x0200053A RID: 1338
		[CompilerGenerated]
		[Serializable]
		private sealed class <>c
		{
			// Token: 0x0600322D RID: 12845 RVA: 0x0022C4AB File Offset: 0x0022A6AB
			// Note: this type is marked as 'beforefieldinit'.
			static <>c()
			{
			}

			// Token: 0x0600322E RID: 12846 RVA: 0x00002050 File Offset: 0x00000250
			public <>c()
			{
			}

			// Token: 0x0600322F RID: 12847 RVA: 0x0022C4B7 File Offset: 0x0022A6B7
			internal bool <GetRequestForCommand>b__2_0(string x)
			{
				return x.StartsWith("ATSP");
			}

			// Token: 0x04001D29 RID: 7465
			public static readonly VolvoCAN11bitECU.<>c <>9 = new VolvoCAN11bitECU.<>c();

			// Token: 0x04001D2A RID: 7466
			public static Predicate<string> <>9__2_0;
		}
	}
}
