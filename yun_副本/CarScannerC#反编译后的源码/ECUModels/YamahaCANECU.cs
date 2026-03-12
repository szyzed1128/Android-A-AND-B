using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using CarScannerXamarinForms.DataRecorder;
using CarScannerXamarinForms.DTC;
using CarScannerXamarinForms.OBD2;
using Xamarin.Forms;

namespace CarScannerXamarinForms.ECUModels
{
	// Token: 0x0200053C RID: 1340
	internal class YamahaCANECU : CAN11bitECU
	{
		// Token: 0x06003233 RID: 12851 RVA: 0x0022CC3C File Offset: 0x0022AE3C
		public YamahaCANECU(string name, string requestHeader, string responseHeader, string openSessionCommand, string readCommand, string clearCommand)
		{
			this.Name = name;
			base.RequestHeader = requestHeader;
			base.ResponseHeader = responseHeader;
			this.Protocol = 6;
			base.OpenSessionCommands = new List<string> { openSessionCommand };
			base.ReadDTCCommands = new List<string> { readCommand };
			base.ClearDTCCommands = new List<string> { clearCommand };
			base.CloseSessionCommands = new List<string>();
			this.AddUDSIdents();
			this.AddKWP2000Idents();
		}

		// Token: 0x170012FD RID: 4861
		// (get) Token: 0x06003234 RID: 12852 RVA: 0x0022CCBB File Offset: 0x0022AEBB
		public static IReadOnlyList<IECU> ECUs
		{
			get
			{
				return YamahaCANECU.BuildList();
			}
		}

		// Token: 0x06003235 RID: 12853 RVA: 0x0022CCC4 File Offset: 0x0022AEC4
		private static List<IECU> BuildList()
		{
			return new List<IECU>
			{
				new YamahaCANECU(CAN11bitECU.ECU_ENGINE_NAME, "7E0", "7E8", "1003", "18A1FFFF", "14FFFFFF"),
				new YamahaCANECU(CAN11bitECU.ECU_ABS_NAME, "7E3", "7EB", "1003", "18A1FFFF", "14FFFFFF")
			};
		}

		// Token: 0x06003236 RID: 12854 RVA: 0x0022CD28 File Offset: 0x0022AF28
		protected override void DtcReadRequest_ResponseDecoded(OBDRequest request, byte[] data, bool decodeResult, string responseHeader)
		{
			if (data != null && data.Length != 0)
			{
				this.ECUExists = true;
				List<DTCItemV2> dtcs = DTCDecoder.DecodeData(request, request.Header, data);
				if (base.RemoveOtherRequestsIfUDSReadResponded && !string.IsNullOrEmpty(request.Header) && dtcs.Count > 0 && request.Command.StartsWith("19"))
				{
					List<OBDRequest> queueCopy = App.OBDReader.GetQueueCopy();
					Func<string, bool> <>9__2;
					queueCopy.RemoveAll(delegate(OBDRequest x)
					{
						if (x.Header == request.Header && !request.Command.StartsWith("19"))
						{
							IEnumerable<string> closeSessionCommands = this.CloseSessionCommands;
							Func<string, bool> func;
							if ((func = <>9__2) == null)
							{
								func = (<>9__2 = (string c) => c == request.Command);
							}
							return !closeSessionCommands.Any(func);
						}
						return false;
					});
					App.OBDReader.ReplaceQueue(queueCopy);
				}
				Device.BeginInvokeOnMainThread(delegate
				{
					bool flag = false;
					using (List<DTCItemV2>.Enumerator enumerator = dtcs.GetEnumerator())
					{
						while (enumerator.MoveNext())
						{
							DTCItemV2 dtc = enumerator.Current;
							if (dtc.Code != null && dtc.Code.Length >= 2)
							{
								dtc.Code = new string(dtc.Code.TakeLast(2).ToArray<char>());
							}
							if (!this.DTCCollection.ToArray().Any((DTCItemV2 x) => x.Code == dtc.Code))
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
						}
					}
					if (flag)
					{
						this.UpdateCollection();
					}
				});
			}
		}

		// Token: 0x0200053D RID: 1341
		[CompilerGenerated]
		private sealed class <>c__DisplayClass4_0
		{
			// Token: 0x06003237 RID: 12855 RVA: 0x00002050 File Offset: 0x00000250
			public <>c__DisplayClass4_0()
			{
			}

			// Token: 0x06003238 RID: 12856 RVA: 0x0022CE24 File Offset: 0x0022B024
			internal bool <DtcReadRequest_ResponseDecoded>b__1(OBDRequest x)
			{
				if (x.Header == this.request.Header && !this.request.Command.StartsWith("19"))
				{
					IEnumerable<string> closeSessionCommands = this.<>4__this.CloseSessionCommands;
					Func<string, bool> func;
					if ((func = this.<>9__2) == null)
					{
						func = (this.<>9__2 = (string c) => c == this.request.Command);
					}
					return !closeSessionCommands.Any(func);
				}
				return false;
			}

			// Token: 0x06003239 RID: 12857 RVA: 0x0022CE94 File Offset: 0x0022B094
			internal bool <DtcReadRequest_ResponseDecoded>b__2(string c)
			{
				return c == this.request.Command;
			}

			// Token: 0x04001D2B RID: 7467
			public OBDRequest request;

			// Token: 0x04001D2C RID: 7468
			public YamahaCANECU <>4__this;

			// Token: 0x04001D2D RID: 7469
			public string responseHeader;

			// Token: 0x04001D2E RID: 7470
			public Func<string, bool> <>9__2;
		}

		// Token: 0x0200053E RID: 1342
		[CompilerGenerated]
		private sealed class <>c__DisplayClass4_1
		{
			// Token: 0x0600323A RID: 12858 RVA: 0x00002050 File Offset: 0x00000250
			public <>c__DisplayClass4_1()
			{
			}

			// Token: 0x0600323B RID: 12859 RVA: 0x0022CEA8 File Offset: 0x0022B0A8
			internal void <DtcReadRequest_ResponseDecoded>b__0()
			{
				bool flag = false;
				using (List<DTCItemV2>.Enumerator enumerator = this.dtcs.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						YamahaCANECU.<>c__DisplayClass4_2 CS$<>8__locals1 = new YamahaCANECU.<>c__DisplayClass4_2();
						CS$<>8__locals1.dtc = enumerator.Current;
						if (CS$<>8__locals1.dtc.Code != null && CS$<>8__locals1.dtc.Code.Length >= 2)
						{
							CS$<>8__locals1.dtc.Code = new string(CS$<>8__locals1.dtc.Code.TakeLast(2).ToArray<char>());
						}
						if (!this.CS$<>8__locals1.<>4__this.DTCCollection.ToArray().Any((DTCItemV2 x) => x.Code == CS$<>8__locals1.dtc.Code))
						{
							CS$<>8__locals1.dtc.LoadDescription();
							CS$<>8__locals1.dtc.ECU = this.CS$<>8__locals1.<>4__this.Name;
							this.CS$<>8__locals1.<>4__this.DTCCollection.Add(CS$<>8__locals1.dtc);
							DataRecorderV2 recorder = App.OBDReader.CurrentCarData.Recorder;
							if (recorder != null)
							{
								recorder.Record(CS$<>8__locals1.dtc);
							}
							flag = true;
						}
					}
				}
				if (flag)
				{
					this.CS$<>8__locals1.<>4__this.UpdateCollection();
				}
			}

			// Token: 0x04001D2F RID: 7471
			public List<DTCItemV2> dtcs;

			// Token: 0x04001D30 RID: 7472
			public YamahaCANECU.<>c__DisplayClass4_0 CS$<>8__locals1;
		}

		// Token: 0x0200053F RID: 1343
		[CompilerGenerated]
		private sealed class <>c__DisplayClass4_2
		{
			// Token: 0x0600323C RID: 12860 RVA: 0x00002050 File Offset: 0x00000250
			public <>c__DisplayClass4_2()
			{
			}

			// Token: 0x0600323D RID: 12861 RVA: 0x0022CFEC File Offset: 0x0022B1EC
			internal bool <DtcReadRequest_ResponseDecoded>b__3(DTCItemV2 x)
			{
				return x.Code == this.dtc.Code;
			}

			// Token: 0x04001D31 RID: 7473
			public DTCItemV2 dtc;
		}
	}
}
