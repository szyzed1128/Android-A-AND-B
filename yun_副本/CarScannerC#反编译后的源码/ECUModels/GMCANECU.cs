using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
using CarScannerXamarinForms.Common;
using CarScannerXamarinForms.DataRecorder;
using CarScannerXamarinForms.DTC;
using CarScannerXamarinForms.OBD2;
using Xamarin.Forms;

namespace CarScannerXamarinForms.ECUModels
{
	// Token: 0x020004D7 RID: 1239
	internal class GMCANECU : CAN11bitECU
	{
		// Token: 0x06003098 RID: 12440 RVA: 0x0021AD70 File Offset: 0x00218F70
		public GMCANECU(string name, string request)
		{
			this.Name = name;
			base.RequestHeader = request;
			base.ResponseHeader = "";
			this.Protocol = 6;
			base.OpenSessionCommands = new List<string>(0);
			base.ReadDTCCommands = new List<string> { "1800FF00", "1902AC", "19D2FF00", "A98102F", "A98112F", "A98118F", "A9815AF", "1201" };
			base.ClearDTCCommands = new List<string> { "04", "04", "14FFFFFF", "14FF00", "14", "14FFFF" };
			base.CloseSessionCommands = new List<string>();
			this.AddKWP2000Idents();
			this.AddUDSIdents();
			if (base.RequestHeader.StartsWith("7"))
			{
				this.ATCF = "4" + ((int.Parse(base.RequestHeader, NumberStyles.HexNumber) + 8) & 255).ToString("X2");
			}
			if (base.RequestHeader.StartsWith("2"))
			{
				this.ATCF = "4" + base.RequestHeader.Substring(1, 2);
			}
		}

		// Token: 0x06003099 RID: 12441 RVA: 0x0021AF04 File Offset: 0x00219104
		private static List<IECU> BuildList()
		{
			ValueTuple<string, string>[] array = new ValueTuple<string, string>[]
			{
				new ValueTuple<string, string>("7E0", CAN11bitECU.ECU_ENGINE_NAME),
				new ValueTuple<string, string>("7E1", CAN11bitECU.ECU_TRANSMISSION_NAME + " #1"),
				new ValueTuple<string, string>("7E2", CAN11bitECU.ECU_TRANSMISSION_NAME + " #2"),
				new ValueTuple<string, string>("241", "BCM"),
				new ValueTuple<string, string>("242", "EPS / TDM"),
				new ValueTuple<string, string>("243", "ABS/EBCM/ESC"),
				new ValueTuple<string, string>("244", "EHU (Entertainment Head Unit)"),
				new ValueTuple<string, string>("246", "SIC"),
				new ValueTuple<string, string>("247", "SDC/SRS"),
				new ValueTuple<string, string>("249", "AHL/AFL"),
				new ValueTuple<string, string>("24B", "Continuous Damping Control"),
				new ValueTuple<string, string>("24C", "Intruments cluster/Dashboard"),
				new ValueTuple<string, string>("251", "HVAC (Heater, ventilation, A/C)"),
				new ValueTuple<string, string>("254", "Parking brake")
			};
			List<IECU> list = new List<IECU>();
			list.Add(new OBD2Can11bitECU
			{
				ReadDTCCommands = new List<string> { "03", "03", "07", "07", "0A", "1902AF", "1902AF", "1800FF00", "1802FF00", "1201" },
				ClearDTCCommands = new List<string> { "04", "04", "04", "14FFFFFF", "14", "14FF00" }
			});
			foreach (ValueTuple<string, string> valueTuple in array)
			{
				string item = valueTuple.Item1;
				GMCANECU gmcanecu = new GMCANECU(valueTuple.Item2, item);
				list.Add(gmcanecu);
			}
			return list;
		}

		// Token: 0x0600309A RID: 12442 RVA: 0x0021B174 File Offset: 0x00219374
		protected override OBDRequest[] GetDTCReadRequests()
		{
			List<OBDRequest> list = base.GetDTCReadRequests().ToList<OBDRequest>();
			foreach (OBDRequest obdrequest in list.Where((OBDRequest x) => x.Command.StartsWith("A981")).ToArray<OBDRequest>())
			{
				obdrequest.BeforeCommands = obdrequest.BeforeCommands.Concat(new string[] { "ATCAF0" }).ToArray<string>();
				obdrequest.AfterCommands = obdrequest.AfterCommands.Concat(new string[] { "ATCAF1" }).ToArray<string>();
				obdrequest.ResponseDecoded -= this.DtcReadRequest_ResponseDecoded;
				obdrequest.ResponseReceived -= this.DecodeA981;
				obdrequest.ResponseReceived += this.DecodeA981;
				obdrequest.Command = "03" + obdrequest.Command;
			}
			return list.ToArray();
		}

		// Token: 0x0600309B RID: 12443 RVA: 0x0021B26C File Offset: 0x0021946C
		private void DecodeA981(OBDRequest request, string data)
		{
			if (string.IsNullOrEmpty(data))
			{
				return;
			}
			if (data.Contains("NO DATA"))
			{
				return;
			}
			string[] array = (from x in OBDDataReader.FilterHexAndNewLineOnly(data).Split(new char[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries)
				where x.Length > 3
				select x).ToArray<string>();
			for (int i = 0; i < array.Length; i++)
			{
				string text = array[i];
				string responseHeader = text.Substring(0, 3);
				string text2 = text.Substring(3);
				if (text2.StartsWith("81"))
				{
					byte[] array2 = BitHelpers.ConvertHexToBytesX(text2);
					DTCItemV2 dtc = DTCDecoder.DecodeGMA981(array2, request, request.Header);
					if (dtc != null)
					{
						Func<DTCItemV2, bool> <>9__2;
						Device.BeginInvokeOnMainThread(delegate
						{
							bool flag = false;
							IEnumerable<DTCItemV2> enumerable = this.DTCCollection.ToArray();
							Func<DTCItemV2, bool> func;
							if ((func = <>9__2) == null)
							{
								func = (<>9__2 = (DTCItemV2 x) => x.Code == dtc.Code && x.Payload == dtc.Payload);
							}
							if (enumerable.FirstOrDefault(func) == null)
							{
								dtc.LoadDescription();
								dtc.ECU = this.Name;
								this.DTCCollection.Add(dtc);
								DataRecorderV2 recorder = App.OBDReader.CurrentCarData.Recorder;
								if (recorder != null)
								{
									recorder.Record(dtc);
								}
								flag = true;
							}
							if (flag)
							{
								this.UpdateCollection();
							}
						});
					}
				}
			}
		}

		// Token: 0x0600309C RID: 12444 RVA: 0x0021B350 File Offset: 0x00219550
		public override OBDRequest GetRequestForCommand(string cmd)
		{
			OBDRequest requestForCommand = base.GetRequestForCommand(cmd);
			requestForCommand.BeforeCommands = requestForCommand.BeforeCommands.Concat(new string[]
			{
				"ATCF" + this.ATCF,
				"ATCM" + this.ATCM
			}).ToArray<string>();
			requestForCommand.AfterCommands = requestForCommand.AfterCommands.Concat(new string[] { "ATAR" }).ToArray<string>();
			return requestForCommand;
		}

		// Token: 0x170012A3 RID: 4771
		// (get) Token: 0x0600309D RID: 12445 RVA: 0x0021B3CC File Offset: 0x002195CC
		public static IReadOnlyList<IECU> ECUs
		{
			get
			{
				return GMCANECU.BuildList();
			}
		}

		// Token: 0x04001C2F RID: 7215
		private string ATCM = "4FF";

		// Token: 0x04001C30 RID: 7216
		private string ATCF = "";

		// Token: 0x020004D8 RID: 1240
		[CompilerGenerated]
		[Serializable]
		private sealed class <>c
		{
			// Token: 0x0600309E RID: 12446 RVA: 0x0021B3D3 File Offset: 0x002195D3
			// Note: this type is marked as 'beforefieldinit'.
			static <>c()
			{
			}

			// Token: 0x0600309F RID: 12447 RVA: 0x00002050 File Offset: 0x00000250
			public <>c()
			{
			}

			// Token: 0x060030A0 RID: 12448 RVA: 0x0021B3DF File Offset: 0x002195DF
			internal bool <GetDTCReadRequests>b__4_0(OBDRequest x)
			{
				return x.Command.StartsWith("A981");
			}

			// Token: 0x060030A1 RID: 12449 RVA: 0x0021B3F1 File Offset: 0x002195F1
			internal bool <DecodeA981>b__5_0(string x)
			{
				return x.Length > 3;
			}

			// Token: 0x04001C31 RID: 7217
			public static readonly GMCANECU.<>c <>9 = new GMCANECU.<>c();

			// Token: 0x04001C32 RID: 7218
			public static Func<OBDRequest, bool> <>9__4_0;

			// Token: 0x04001C33 RID: 7219
			public static Func<string, bool> <>9__5_0;
		}

		// Token: 0x020004D9 RID: 1241
		[CompilerGenerated]
		private sealed class <>c__DisplayClass5_0
		{
			// Token: 0x060030A2 RID: 12450 RVA: 0x00002050 File Offset: 0x00000250
			public <>c__DisplayClass5_0()
			{
			}

			// Token: 0x060030A3 RID: 12451 RVA: 0x0021B3FC File Offset: 0x002195FC
			internal void <DecodeA981>b__1()
			{
				bool flag = false;
				IEnumerable<DTCItemV2> enumerable = this.<>4__this.DTCCollection.ToArray();
				Func<DTCItemV2, bool> func;
				if ((func = this.<>9__2) == null)
				{
					func = (this.<>9__2 = (DTCItemV2 x) => x.Code == this.dtc.Code && x.Payload == this.dtc.Payload);
				}
				if (enumerable.FirstOrDefault(func) == null)
				{
					this.dtc.LoadDescription();
					this.dtc.ECU = this.<>4__this.Name;
					this.<>4__this.DTCCollection.Add(this.dtc);
					DataRecorderV2 recorder = App.OBDReader.CurrentCarData.Recorder;
					if (recorder != null)
					{
						recorder.Record(this.dtc);
					}
					flag = true;
				}
				if (flag)
				{
					this.<>4__this.UpdateCollection();
				}
			}

			// Token: 0x060030A4 RID: 12452 RVA: 0x0021B4A8 File Offset: 0x002196A8
			internal bool <DecodeA981>b__2(DTCItemV2 x)
			{
				return x.Code == this.dtc.Code && x.Payload == this.dtc.Payload;
			}

			// Token: 0x04001C34 RID: 7220
			public string responseHeader;

			// Token: 0x04001C35 RID: 7221
			public DTCItemV2 dtc;

			// Token: 0x04001C36 RID: 7222
			public GMCANECU <>4__this;

			// Token: 0x04001C37 RID: 7223
			public Func<DTCItemV2, bool> <>9__2;
		}
	}
}
