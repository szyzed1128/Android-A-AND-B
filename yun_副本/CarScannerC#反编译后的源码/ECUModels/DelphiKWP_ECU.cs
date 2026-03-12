using System;
using System.Collections.Generic;
using CarScannerXamarinForms.OBD2;

namespace CarScannerXamarinForms.ECUModels
{
	// Token: 0x020004CE RID: 1230
	internal class DelphiKWP_ECU : KWPECU
	{
		// Token: 0x0600307C RID: 12412 RVA: 0x00218F40 File Offset: 0x00217140
		public DelphiKWP_ECU(string name, string header)
		{
			base.Name = name;
			base.Protocol = 5;
			base.OpenSessionCommands = new List<string>(0);
			base.ReadDTCCommands = new List<string> { "18000000", "1800FF00" };
			base.ClearDTCCommands = new List<string> { "14FF00" };
			base.RequestHeader = header;
			this.AddKWP2000Idents();
		}

		// Token: 0x0600307D RID: 12413 RVA: 0x00218FB1 File Offset: 0x002171B1
		protected override OBDRequest[] GetTestECUExistsRequest()
		{
			return new OBDRequest[0];
		}

		// Token: 0x1700129A RID: 4762
		// (get) Token: 0x0600307E RID: 12414 RVA: 0x00218FB9 File Offset: 0x002171B9
		public static IReadOnlyList<IECU> ECUs
		{
			get
			{
				return DelphiKWP_ECU.BuildList();
			}
		}

		// Token: 0x0600307F RID: 12415 RVA: 0x00218FC0 File Offset: 0x002171C0
		private static IReadOnlyList<IECU> BuildList()
		{
			return new List<IECU>
			{
				new DelphiKWP_ECU("Delphi ECU", "8411F1")
			};
		}
	}
}
