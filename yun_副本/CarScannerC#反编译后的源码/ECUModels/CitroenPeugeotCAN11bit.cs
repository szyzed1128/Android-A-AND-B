using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using CarScannerXamarinForms.Common;
using CarScannerXamarinForms.OBD2;

namespace CarScannerXamarinForms.ECUModels
{
	// Token: 0x020004CC RID: 1228
	internal class CitroenPeugeotCAN11bit : CAN11bitECU
	{
		// Token: 0x06003076 RID: 12406 RVA: 0x002187C8 File Offset: 0x002169C8
		public CitroenPeugeotCAN11bit(string name, string request, string response)
		{
			this.Name = name;
			base.RequestHeader = request;
			base.ResponseHeader = response;
			this.Protocol = 6;
			base.OpenSessionCommands = new List<string> { "81", "10C0" };
			base.ReadDTCCommands = new List<string> { "17FF00", "17FFFF", "1902AF", "1902AC" };
			base.ClearDTCCommands = new List<string> { "14FFFFFF", "14FF00" };
			base.CloseSessionCommands = new List<string> { "82" };
			this.AddUDSIdents();
			this.AddKWP2000Idents();
		}

		// Token: 0x06003077 RID: 12407 RVA: 0x00218894 File Offset: 0x00216A94
		private static List<IECU> BuildList()
		{
			ValueTuple<string, string, string>[] array = new ValueTuple<string, string, string>[]
			{
				new ValueTuple<string, string, string>("6A8", "6C8", CAN11bitECU.ECU_ENGINE_NAME + "#2"),
				new ValueTuple<string, string, string>("6A9", "6C9", CAN11bitECU.ECU_TRANSMISSION_NAME + "#2"),
				new ValueTuple<string, string, string>("6A8", "688", CAN11bitECU.ECU_ENGINE_NAME + "#3"),
				new ValueTuple<string, string, string>("6A9", "689", CAN11bitECU.ECU_TRANSMISSION_NAME + "#3 / BSI"),
				new ValueTuple<string, string, string>("6A8", "688", CAN11bitECU.ECU_ENGINE_NAME + "#4"),
				new ValueTuple<string, string, string>("744", "644", "SRS/Airbag"),
				new ValueTuple<string, string, string>("6C1", "601", "Additional system"),
				new ValueTuple<string, string, string>("765", "665", "Display"),
				new ValueTuple<string, string, string>("760", "660", "Radio"),
				new ValueTuple<string, string, string>("752", "652", "BSI"),
				new ValueTuple<string, string, string>("742", "642", "Comfort"),
				new ValueTuple<string, string, string>("6B5", "695", "Power steering"),
				new ValueTuple<string, string, string>("6AD", "68D", "ABS"),
				new ValueTuple<string, string, string>("745", "765", "LCE / Key reader"),
				new ValueTuple<string, string, string>("75D", "65D", "Parking"),
				new ValueTuple<string, string, string>("77B", "67B", "Multifunction Facade (car radio + gps)"),
				new ValueTuple<string, string, string>("747", "647", "BSM / Engine relay unit"),
				new ValueTuple<string, string, string>("76D", "66D", "Climate"),
				new ValueTuple<string, string, string>("75F", "65F", "Dashboard"),
				new ValueTuple<string, string, string>("6C8", "628", "Steering wheel controls"),
				new ValueTuple<string, string, string>("6BC", "69C", "Voltage maintenance device / DMT"),
				new ValueTuple<string, string, string>("6AF", "68F", "TPMS"),
				new ValueTuple<string, string, string>("764", "664", "Telematics / Navigation")
			};
			List<IECU> list = new List<IECU>();
			list.Add(new OBD2Can11bitECU
			{
				ReadDTCCommands = new List<string> { "03", "03", "07", "07", "0A", "17FF00", "1902AF", "1902AC" },
				ClearDTCCommands = new List<string> { "04", "04", "14", "14FF00", "14FFFFFF" }
			});
			OBD2Can11bitECU obd2Can11bitECU = new OBD2Can11bitECU
			{
				ReadDTCCommands = new List<string> { "17FF00", "1902AF", "1902AF", "17FFFF" },
				ClearDTCCommands = new List<string> { "14FFFFFF", "14FF00" },
				OpenSessionCommands = new List<string> { "81", "10C0" },
				CloseSessionCommands = new List<string>(),
				Protocol = 6,
				RequestHeader = "7E0",
				ResponseHeader = "7E8",
				Name = CAN11bitECU.ECU_ENGINE_NAME + "#1"
			};
			list.Add(obd2Can11bitECU);
			OBD2Can11bitECU obd2Can11bitECU2 = new OBD2Can11bitECU
			{
				ReadDTCCommands = new List<string> { "17FF00", "1902AF", "1902AF" },
				ClearDTCCommands = new List<string> { "14FFFFFF", "14FF00" },
				OpenSessionCommands = new List<string> { "81", "10C0" },
				CloseSessionCommands = new List<string>(),
				Protocol = 6,
				RequestHeader = "7E1",
				ResponseHeader = "7E9",
				Name = CAN11bitECU.ECU_TRANSMISSION_NAME + "#1"
			};
			list.Add(obd2Can11bitECU2);
			foreach (ValueTuple<string, string, string> valueTuple in array)
			{
				string item = valueTuple.Item1;
				string item2 = valueTuple.Item2;
				CitroenPeugeotCAN11bit citroenPeugeotCAN11bit = new CitroenPeugeotCAN11bit(valueTuple.Item3, item, item2);
				list.Add(citroenPeugeotCAN11bit);
			}
			return list;
		}

		// Token: 0x06003078 RID: 12408 RVA: 0x00218DD0 File Offset: 0x00216FD0
		protected override OBDRequest[] GetDTCClearRequests()
		{
			OBDRequest requestForCommand = this.GetRequestForCommand("21CF00");
			OBDRequest writePostDataReq = this.GetRequestForCommand("34CF00");
			requestForCommand.ResponseReceived += delegate(OBDRequest req, string data)
			{
				if (data == null || data == "" || data.Contains("NO DATA"))
				{
					List<OBDRequest> queueCopy = App.OBDReader.GetQueueCopy();
					queueCopy.Remove(writePostDataReq);
					App.OBDReader.ReplaceQueue(queueCopy);
					return;
				}
				if (!OBDDataReader.FilterHexAndNewLineOnly(data).Contains(req.ResponseMarker))
				{
					List<OBDRequest> queueCopy2 = App.OBDReader.GetQueueCopy();
					queueCopy2.Remove(writePostDataReq);
					App.OBDReader.ReplaceQueue(queueCopy2);
					return;
				}
				DateTime nowSafe = DateTimeNowHelper.NowSafe;
				int num = nowSafe.Year % 100;
				writePostDataReq.Command = string.Concat(new string[]
				{
					"34CF00000009000000",
					nowSafe.Day.ToString("X2"),
					nowSafe.Month.ToString("X2"),
					num.ToString("X2"),
					"000010FFDF"
				});
			};
			List<OBDRequest> list = new List<OBDRequest>();
			list.Add(requestForCommand);
			list.Add(writePostDataReq);
			list.AddRange(base.GetDTCClearRequests());
			return list.ToArray();
		}

		// Token: 0x17001299 RID: 4761
		// (get) Token: 0x06003079 RID: 12409 RVA: 0x00218E3B File Offset: 0x0021703B
		public static IReadOnlyList<IECU> ECUs
		{
			get
			{
				return CitroenPeugeotCAN11bit.BuildList();
			}
		}

		// Token: 0x020004CD RID: 1229
		[CompilerGenerated]
		private sealed class <>c__DisplayClass2_0
		{
			// Token: 0x0600307A RID: 12410 RVA: 0x00002050 File Offset: 0x00000250
			public <>c__DisplayClass2_0()
			{
			}

			// Token: 0x0600307B RID: 12411 RVA: 0x00218E44 File Offset: 0x00217044
			internal void <GetDTCClearRequests>b__0(OBDRequest req, string data)
			{
				if (data == null || data == "" || data.Contains("NO DATA"))
				{
					List<OBDRequest> queueCopy = App.OBDReader.GetQueueCopy();
					queueCopy.Remove(this.writePostDataReq);
					App.OBDReader.ReplaceQueue(queueCopy);
					return;
				}
				if (!OBDDataReader.FilterHexAndNewLineOnly(data).Contains(req.ResponseMarker))
				{
					List<OBDRequest> queueCopy2 = App.OBDReader.GetQueueCopy();
					queueCopy2.Remove(this.writePostDataReq);
					App.OBDReader.ReplaceQueue(queueCopy2);
					return;
				}
				DateTime nowSafe = DateTimeNowHelper.NowSafe;
				int num = nowSafe.Year % 100;
				this.writePostDataReq.Command = string.Concat(new string[]
				{
					"34CF00000009000000",
					nowSafe.Day.ToString("X2"),
					nowSafe.Month.ToString("X2"),
					num.ToString("X2"),
					"000010FFDF"
				});
			}

			// Token: 0x04001C2E RID: 7214
			public OBDRequest writePostDataReq;
		}
	}
}
