using System;
using System.Collections.Generic;
using CarScannerXamarinForms.OBD2;

namespace CarScannerXamarinForms.ECUModels
{
	// Token: 0x020004D5 RID: 1237
	internal class GenericCAN11bitBrand : CAN11bitECU
	{
		// Token: 0x06003092 RID: 12434 RVA: 0x0021A8B4 File Offset: 0x00218AB4
		public GenericCAN11bitBrand(string name, string request, string response)
		{
			this.Name = name;
			base.RequestHeader = request;
			base.ResponseHeader = response;
			this.Protocol = 6;
			base.OpenSessionCommands = new List<string>();
			base.ReadDTCCommands = new List<string> { "1902AF", "1902AC", "190223", "190278", "1800FF00", "1802FF00", "18FF00", "17FF00", "13FF00" };
			base.ClearDTCCommands = new List<string> { "14", "14FF00", "14FFFF", "14FFFFFF" };
			base.CloseSessionCommands = new List<string>();
			this.AddUDSIdents();
			this.AddKWP2000Idents();
		}

		// Token: 0x06003093 RID: 12435 RVA: 0x0021A9AC File Offset: 0x00218BAC
		private static List<IECU> BuildList()
		{
			ValueTuple<string, string>[] array = new ValueTuple<string, string>[]
			{
				new ValueTuple<string, string>("7E0", CAN11bitECU.ECU_ENGINE_NAME),
				new ValueTuple<string, string>("7E1", CAN11bitECU.ECU_TRANSMISSION_NAME)
			};
			List<IECU> list = new List<IECU>();
			list.Add(new OBD2Can11bitECU
			{
				ReadDTCCommands = new List<string>
				{
					"03", "03", "07", "07", "0A", "190208", "1902AC", "1902AF", "190223", "190278",
					"1800FF00", "1802FF00", "18FF00", "17FF00", "13FF00"
				},
				ClearDTCCommands = new List<string> { "04", "04", "04", "14FFFFFF", "14", "14FF00" }
			});
			foreach (ValueTuple<string, string> valueTuple in array)
			{
				string item = valueTuple.Item1;
				string item2 = valueTuple.Item2;
				string possibleResponseHeader = CAN11bitHelper.GetPossibleResponseHeader(item, "Ford", null);
				GenericCAN11bitBrand genericCAN11bitBrand = new GenericCAN11bitBrand(item2, item, possibleResponseHeader);
				list.Add(genericCAN11bitBrand);
			}
			return list;
		}

		// Token: 0x170012A1 RID: 4769
		// (get) Token: 0x06003094 RID: 12436 RVA: 0x0021AB41 File Offset: 0x00218D41
		public static IReadOnlyList<IECU> ECUs
		{
			get
			{
				return GenericCAN11bitBrand.BuildList();
			}
		}
	}
}
