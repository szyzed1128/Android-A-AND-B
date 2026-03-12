using System;
using System.Collections.Generic;
using CarScannerXamarinForms.OBD2;

namespace CarScannerXamarinForms.ECUModels
{
	// Token: 0x020004FF RID: 1279
	internal class MitsubishiCANECU : CAN11bitECU
	{
		// Token: 0x06003169 RID: 12649 RVA: 0x00221680 File Offset: 0x0021F880
		public MitsubishiCANECU(string name, string request, string response)
		{
			this.Name = name;
			base.RequestHeader = request;
			base.ResponseHeader = response;
			this.Protocol = 6;
			base.OpenSessionCommands = new List<string> { "1092" };
			base.ReadDTCCommands = new List<string> { "1800FF00", "1902AF" };
			base.ClearDTCCommands = new List<string> { "14FF00", "14FFFF", "14FFFFFF" };
			base.CloseSessionCommands = new List<string> { "1081" };
			this.AddKWP2000Idents();
			base.IdentsHEX.TryAdd("1A87", "Ident $1A87");
			base.IdentsHEX.TryAdd("1A9C", "Ident $1A9C");
		}

		// Token: 0x0600316A RID: 12650 RVA: 0x0022175C File Offset: 0x0021F95C
		private static List<IECU> BuildList()
		{
			ValueTuple<string, string>[] array = new ValueTuple<string, string>[]
			{
				new ValueTuple<string, string>("7E0", CAN11bitECU.ECU_ENGINE_NAME),
				new ValueTuple<string, string>("7E1", CAN11bitECU.ECU_TRANSMISSION_NAME),
				new ValueTuple<string, string>("783", CAN11bitECU.ECU_ABS_NAME + " #1"),
				new ValueTuple<string, string>("784", CAN11bitECU.ECU_ABS_NAME + " #2"),
				new ValueTuple<string, string>("7B6", "AWC 4WD #1"),
				new ValueTuple<string, string>("786", "AWC 4WD #2"),
				new ValueTuple<string, string>("7A4", "SRS airbag #1"),
				new ValueTuple<string, string>("78C", "SRS Airbag #2"),
				new ValueTuple<string, string>("782", "Dashboard/Meter #2"),
				new ValueTuple<string, string>("773", "TPMS"),
				new ValueTuple<string, string>("718", "ACC/FCM"),
				new ValueTuple<string, string>("71A", "Lane assistance"),
				new ValueTuple<string, string>("724", "Compressor"),
				new ValueTuple<string, string>("72A", "DC/DC converter"),
				new ValueTuple<string, string>("73A", "EV"),
				new ValueTuple<string, string>("773", "KOS/OSS/Immobilizer"),
				new ValueTuple<string, string>("78A", "ABS/ASC/ASTC/WSS"),
				new ValueTuple<string, string>("790", "Immobilizer"),
				new ValueTuple<string, string>("792", "BCM/ETACS #1"),
				new ValueTuple<string, string>("7A0", "BCM/ETACS #2"),
				new ValueTuple<string, string>("7A2", "Multifunction display"),
				new ValueTuple<string, string>("7A6", "A/C), climate), heater #1"),
				new ValueTuple<string, string>("771", "A/C), climate), heater #2"),
				new ValueTuple<string, string>("7B5", "AYC/ACD"),
				new ValueTuple<string, string>("761", "BMU"),
				new ValueTuple<string, string>("61E", "CMU #1"),
				new ValueTuple<string, string>("62E", "CMU #2"),
				new ValueTuple<string, string>("63E", "CMU #3"),
				new ValueTuple<string, string>("64E", "CMU #4"),
				new ValueTuple<string, string>("65E", "CMU #5"),
				new ValueTuple<string, string>("66E", "CMU #6"),
				new ValueTuple<string, string>("67E", "CMU #7"),
				new ValueTuple<string, string>("68E", "CMU #8"),
				new ValueTuple<string, string>("69E", "CMU #9"),
				new ValueTuple<string, string>("6AE", "CMU #10"),
				new ValueTuple<string, string>("6BE", "CMU #11"),
				new ValueTuple<string, string>("6CE", "CMU #12"),
				new ValueTuple<string, string>("751", "E/V ECU"),
				new ValueTuple<string, string>("755", "MCU"),
				new ValueTuple<string, string>("7AA", "Other unit #1")
			};
			List<IECU> list = new List<IECU>();
			list.Add(new OBD2Can11bitECU
			{
				ReadDTCCommands = new List<string> { "03", "03", "07", "07", "0A", "1800FF00", "1902AF" },
				ClearDTCCommands = new List<string> { "04", "04", "04", "14", "14FF00", "14FFFF", "14FFFFFF" }
			});
			foreach (ValueTuple<string, string> valueTuple in array)
			{
				string item = valueTuple.Item1;
				string item2 = valueTuple.Item2;
				string possibleResponseHeader = CAN11bitHelper.GetPossibleResponseHeader(item, "Mitsubishi", null);
				MitsubishiCANECU mitsubishiCANECU = new MitsubishiCANECU(item2, item, possibleResponseHeader);
				list.Add(mitsubishiCANECU);
			}
			foreach (ValueTuple<string, string, string> valueTuple2 in new ValueTuple<string, string, string>[]
			{
				new ValueTuple<string, string, string>("79E", "7A1", "Dashboard/Meter #3"),
				new ValueTuple<string, string, string>("6A0", "514", "Dashboard/Meter #1"),
				new ValueTuple<string, string, string>("688", "511", "A/C), climate), heater #3"),
				new ValueTuple<string, string, string>("600", "500", "TPMS #2")
			})
			{
				string item3 = valueTuple2.Item1;
				string item4 = valueTuple2.Item2;
				MitsubishiCANECU mitsubishiCANECU2 = new MitsubishiCANECU(valueTuple2.Item3, item3, item4);
				list.Add(mitsubishiCANECU2);
			}
			return list;
		}

		// Token: 0x170012E4 RID: 4836
		// (get) Token: 0x0600316B RID: 12651 RVA: 0x00221CCF File Offset: 0x0021FECF
		public static IReadOnlyList<IECU> ECUs
		{
			get
			{
				return MitsubishiCANECU.BuildList();
			}
		}
	}
}
