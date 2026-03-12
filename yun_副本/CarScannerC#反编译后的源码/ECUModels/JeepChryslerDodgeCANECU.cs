using System;
using System.Collections.Generic;

namespace CarScannerXamarinForms.ECUModels
{
	// Token: 0x020004E5 RID: 1253
	internal class JeepChryslerDodgeCANECU : CAN11bitECU
	{
		// Token: 0x060030DE RID: 12510 RVA: 0x0021D124 File Offset: 0x0021B324
		public JeepChryslerDodgeCANECU(string name, string request, string response)
		{
			this.Name = name;
			base.RequestHeader = request;
			base.ResponseHeader = response;
			this.Protocol = 6;
			base.OpenSessionCommands = new List<string>();
			base.ReadDTCCommands = new List<string> { "1902AC", "190208", "1800FF00" };
			base.ClearDTCCommands = new List<string> { "14FFFFFF", "14FF00", "14", "14FFFF" };
			base.CloseSessionCommands = new List<string>();
			this.AddUDSIdents();
			this.AddKWP2000Idents();
			base.IdentsHEX.TryAdd("1A87", "ECU Ident");
			base.IdentsHEX.TryAdd("21E5", "ECU Ident");
			base.IdentsHEX.TryAdd("21EA", "ECU Ident");
			base.IdentsHEX.TryAdd("1A88", "ECU Ident");
			base.IdentsHEX.TryAdd("1A90", "ECU Ident");
			base.IdentsHEX.TryAdd("1A9C", "ECU Ident");
			base.IdentsHEX.TryAdd("22F100", "ECU Ident");
			base.IdentsHEX.TryAdd("22F1A5", "ECU Ident");
			base.IdentsHEX.TryAdd("22F110", "ECU Ident");
			base.IdentsHEX.TryAdd("22F1A0", "ECU Ident");
			base.IdentsHEX.TryAdd("22F190", "ECU Ident");
			base.IdentsHEX.TryAdd("22F132", "ECU Ident");
			base.IdentsHEX.TryAdd("22F150", "ECU Ident");
			base.IdentsHEX.TryAdd("22F151", "ECU Ident");
		}

		// Token: 0x060030DF RID: 12511 RVA: 0x0021D30C File Offset: 0x0021B50C
		private static List<IECU> BuildList()
		{
			ValueTuple<string, string, string>[] array = new ValueTuple<string, string, string>[]
			{
				new ValueTuple<string, string, string>("7E0", "7E8", CAN11bitECU.ECU_ENGINE_NAME),
				new ValueTuple<string, string, string>("7E1", "7E9", CAN11bitECU.ECU_TRANSMISSION_NAME),
				new ValueTuple<string, string, string>("784", "785", CAN11bitECU.ECU_ABS_NAME),
				new ValueTuple<string, string, string>("6E0", "51C", "SRS/Airbag"),
				new ValueTuple<string, string, string>("7F0", "53E", "Amplifier"),
				new ValueTuple<string, string, string>("710", "522", "Automatic high beam"),
				new ValueTuple<string, string, string>("690", "512", "Blind spot detection"),
				new ValueTuple<string, string, string>("640", "508", "Front left door"),
				new ValueTuple<string, string, string>("650", "50A", "Front right door"),
				new ValueTuple<string, string, string>("648", "509", "Rear left door"),
				new ValueTuple<string, string, string>("658", "50B", "Rear right door"),
				new ValueTuple<string, string, string>("7A0", "534", "DVD Changer"),
				new ValueTuple<string, string, string>("6CC", "6CD", "Electronic pedestrian protection"),
				new ValueTuple<string, string, string>("770", "52E", "Fold & Stow module"),
				new ValueTuple<string, string, string>("620", "504", "Front control"),
				new ValueTuple<string, string, string>("7F8", "53F", "Hands free"),
				new ValueTuple<string, string, string>("688", "511", "Heater, ventilation and A/C"),
				new ValueTuple<string, string, string>("6D8", "51B", "Seat heater"),
				new ValueTuple<string, string, string>("7C8", "539", "High intensity discharge translator"),
				new ValueTuple<string, string, string>("6A0", "514", "Instrument cluster"),
				new ValueTuple<string, string, string>("670", "50E", "Intrusion"),
				new ValueTuple<string, string, string>("780", "530", "Last row screen"),
				new ValueTuple<string, string, string>("690", "512", "Left blind spot sensor"),
				new ValueTuple<string, string, string>("660", "50C", "Memory seat"),
				new ValueTuple<string, string, string>("698", "513", "Park assist"),
				new ValueTuple<string, string, string>("628", "505", "Passive entry"),
				new ValueTuple<string, string, string>("728", "525", "Power liftgate"),
				new ValueTuple<string, string, string>("678", "50F", "Power sliding door (left)"),
				new ValueTuple<string, string, string>("680", "510", "Power sliding door (right)"),
				new ValueTuple<string, string, string>("6B0", "516", "Radio"),
				new ValueTuple<string, string, string>("6B8", "517", "Right blind spot sensor"),
				new ValueTuple<string, string, string>("738", "527", "Satellite digital radio/video"),
				new ValueTuple<string, string, string>("7D0", "53A", "Second row screen"),
				new ValueTuple<string, string, string>("622", "484", "Steering column"),
				new ValueTuple<string, string, string>("638", "507", "Sunroof"),
				new ValueTuple<string, string, string>("7DA", "7DB", "TPMS"),
				new ValueTuple<string, string, string>("600", "500", "TPMS (wireless control)")
			};
			List<IECU> list = new List<IECU>();
			list.Add(new OBD2Can11bitECU
			{
				ReadDTCCommands = new List<string> { "03", "03", "07", "07", "0A", "190208", "1902AC", "1902AF" },
				ClearDTCCommands = new List<string> { "04", "04", "04", "14FFFFFF", "14", "14FF00" }
			});
			foreach (ValueTuple<string, string, string> valueTuple in array)
			{
				string item = valueTuple.Item1;
				string item2 = valueTuple.Item2;
				JeepChryslerDodgeCANECU jeepChryslerDodgeCANECU = new JeepChryslerDodgeCANECU(valueTuple.Item3, item, item2);
				list.Add(jeepChryslerDodgeCANECU);
			}
			return list;
		}

		// Token: 0x170012C0 RID: 4800
		// (get) Token: 0x060030E0 RID: 12512 RVA: 0x0021D825 File Offset: 0x0021BA25
		public static IReadOnlyList<IECU> ECUs
		{
			get
			{
				return JeepChryslerDodgeCANECU.BuildList();
			}
		}
	}
}
