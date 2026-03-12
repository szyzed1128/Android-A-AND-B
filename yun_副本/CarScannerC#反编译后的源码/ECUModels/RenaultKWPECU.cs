using System;
using System.Collections.Generic;
using CarScannerXamarinForms.OBD2;

namespace CarScannerXamarinForms.ECUModels
{
	// Token: 0x02000518 RID: 1304
	internal class RenaultKWPECU : KWPECU
	{
		// Token: 0x060031B6 RID: 12726 RVA: 0x00224CC8 File Offset: 0x00222EC8
		public RenaultKWPECU(string name, string testerAddress, string ecuAddress, string protocol, string[] readCommands = null, string[] openSessionCommands = null)
		{
			if (protocol == "5")
			{
				base.Name = name + " (KWP fast)";
			}
			else if (protocol == "4")
			{
				base.Name = name + " (KWP 5baud)";
			}
			else
			{
				base.Name = name + " (KWP)";
			}
			if (openSessionCommands == null)
			{
				base.OpenSessionCommands = new List<string>(1) { "10C0" };
			}
			else
			{
				base.OpenSessionCommands = new List<string>(openSessionCommands);
			}
			if (readCommands == null)
			{
				base.ReadDTCCommands = new List<string>(1) { "17FF00" };
			}
			else
			{
				base.ReadDTCCommands = new List<string>(readCommands);
			}
			base.ClearDTCCommands = new List<string>(1) { "14FF00" };
			base.ECUInitSequence = string.Concat(new string[] { "ATSH81", ecuAddress, testerAddress, ";ATSW96;ATIB10;ATSP", protocol, ";ATFI;" });
			base.AddReinitAfter = true;
		}

		// Token: 0x060031B7 RID: 12727 RVA: 0x00218FB1 File Offset: 0x002171B1
		protected override OBDRequest[] GetTestECUExistsRequest()
		{
			return new OBDRequest[0];
		}

		// Token: 0x170012EC RID: 4844
		// (get) Token: 0x060031B8 RID: 12728 RVA: 0x00224DD6 File Offset: 0x00222FD6
		public static IReadOnlyList<IECU> ECUs
		{
			get
			{
				return RenaultKWPECU.BuildList();
			}
		}

		// Token: 0x060031B9 RID: 12729 RVA: 0x00224DE0 File Offset: 0x00222FE0
		private static IReadOnlyList<IECU> BuildList()
		{
			ValueTuple<string, string, string>[] array = new ValueTuple<string, string, string>[]
			{
				new ValueTuple<string, string, string>(CAN11bitECU.ECU_ENGINE_NAME, "F1", "7A"),
				new ValueTuple<string, string, string>("Park assist", "F1", "0E"),
				new ValueTuple<string, string, string>("Electric parking brake", "F1", "0D"),
				new ValueTuple<string, string, string>("Airbag #2", "F1", "2C"),
				new ValueTuple<string, string, string>("ACC", "F1", "0B"),
				new ValueTuple<string, string, string>("Aux. heater", "E9", "8F"),
				new ValueTuple<string, string, string>("BVA", "F1", "6E"),
				new ValueTuple<string, string, string>("CAN Adapter", "6B", "00"),
				new ValueTuple<string, string, string>("Climate", "F1", "29"),
				new ValueTuple<string, string, string>("ABS #1", "F1", "01"),
				new ValueTuple<string, string, string>("ABS #2", "D1", "80"),
				new ValueTuple<string, string, string>("Direction assist", "F1", "04"),
				new ValueTuple<string, string, string>("AT", "F1", "6E"),
				new ValueTuple<string, string, string>("UCH", "F1", "26"),
				new ValueTuple<string, string, string>("Additional unit", "D0", "8F"),
				new ValueTuple<string, string, string>("HLL DDL", "6D", "8F"),
				new ValueTuple<string, string, string>("Suspension", "F1", "02")
			};
			List<IECU> list = new List<IECU>(array.Length * 2);
			foreach (ValueTuple<string, string, string> valueTuple in array)
			{
				string item = valueTuple.Item1;
				string item2 = valueTuple.Item2;
				string item3 = valueTuple.Item3;
				RenaultKWPECU renaultKWPECU = new RenaultKWPECU(item, item2, item3, "5", null, null);
				list.Add(renaultKWPECU);
			}
			foreach (ValueTuple<string, string, string> valueTuple2 in array)
			{
				string item4 = valueTuple2.Item1;
				string item5 = valueTuple2.Item2;
				string item6 = valueTuple2.Item3;
				RenaultKWPECU renaultKWPECU2 = new RenaultKWPECU(item4, item5, item6, "4", null, null);
				list.Add(renaultKWPECU2);
			}
			return list;
		}
	}
}
