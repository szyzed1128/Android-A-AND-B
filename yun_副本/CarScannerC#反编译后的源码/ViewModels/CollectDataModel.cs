using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using CarScannerXamarinForms.Coding.DB;
using CarScannerXamarinForms.Common;
using CarScannerXamarinForms.OBD2;
using CarScannerXamarinForms.PlatformAdapters;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace CarScannerXamarinForms.ViewModels
{
	// Token: 0x0200072B RID: 1835
	internal class CollectDataModel : INotifyPropertyChanged
	{
		// Token: 0x06003E7D RID: 15997 RVA: 0x0032C970 File Offset: 0x0032AB70
		public CollectDataModel()
		{
			this.Tasks = new ObservableCollection<string>(new string[] { "Renault: ABS, BCM, Dashboard, Engine, EMM, HVAC", "VAG: UDS FULL", "VAG: UDS 01, 02, 03, 08, 09, 17,36, 42, 44, 47, 52, 5F, 65, 6C, 76, A5" });
			foreach (string text in from x in Directory.GetFiles(FileSystemHelper.LocalStoragePath)
				where Path.GetFileName(x).StartsWith("scan_")
				select x)
			{
				try
				{
					File.Delete(text);
				}
				catch (Exception)
				{
				}
			}
		}

		// Token: 0x1400003E RID: 62
		// (add) Token: 0x06003E7E RID: 15998 RVA: 0x0032CA3C File Offset: 0x0032AC3C
		// (remove) Token: 0x06003E7F RID: 15999 RVA: 0x0032CA74 File Offset: 0x0032AC74
		public event PropertyChangedEventHandler PropertyChanged
		{
			[CompilerGenerated]
			add
			{
				PropertyChangedEventHandler propertyChangedEventHandler = this.PropertyChanged;
				PropertyChangedEventHandler propertyChangedEventHandler2;
				do
				{
					propertyChangedEventHandler2 = propertyChangedEventHandler;
					PropertyChangedEventHandler propertyChangedEventHandler3 = (PropertyChangedEventHandler)Delegate.Combine(propertyChangedEventHandler2, value);
					propertyChangedEventHandler = Interlocked.CompareExchange<PropertyChangedEventHandler>(ref this.PropertyChanged, propertyChangedEventHandler3, propertyChangedEventHandler2);
				}
				while (propertyChangedEventHandler != propertyChangedEventHandler2);
			}
			[CompilerGenerated]
			remove
			{
				PropertyChangedEventHandler propertyChangedEventHandler = this.PropertyChanged;
				PropertyChangedEventHandler propertyChangedEventHandler2;
				do
				{
					propertyChangedEventHandler2 = propertyChangedEventHandler;
					PropertyChangedEventHandler propertyChangedEventHandler3 = (PropertyChangedEventHandler)Delegate.Remove(propertyChangedEventHandler2, value);
					propertyChangedEventHandler = Interlocked.CompareExchange<PropertyChangedEventHandler>(ref this.PropertyChanged, propertyChangedEventHandler3, propertyChangedEventHandler2);
				}
				while (propertyChangedEventHandler != propertyChangedEventHandler2);
			}
		}

		// Token: 0x06003E80 RID: 16000 RVA: 0x0032CAA9 File Offset: 0x0032ACA9
		private void OnPropertyChanged(string name)
		{
			PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
			if (propertyChanged == null)
			{
				return;
			}
			propertyChanged(this, new PropertyChangedEventArgs(name));
		}

		// Token: 0x1700146A RID: 5226
		// (get) Token: 0x06003E81 RID: 16001 RVA: 0x0032CAC2 File Offset: 0x0032ACC2
		// (set) Token: 0x06003E82 RID: 16002 RVA: 0x0032CACA File Offset: 0x0032ACCA
		public ObservableCollection<string> Tasks
		{
			[CompilerGenerated]
			get
			{
				return this.<Tasks>k__BackingField;
			}
			[CompilerGenerated]
			private set
			{
				this.<Tasks>k__BackingField = value;
			}
		}

		// Token: 0x1700146B RID: 5227
		// (get) Token: 0x06003E83 RID: 16003 RVA: 0x0032CAD3 File Offset: 0x0032ACD3
		// (set) Token: 0x06003E84 RID: 16004 RVA: 0x0032CADB File Offset: 0x0032ACDB
		public bool IsResultsAvailable
		{
			get
			{
				return this._IsResultsAvailable;
			}
			set
			{
				this._IsResultsAvailable = value;
				this.OnPropertyChanged("IsResultsAvailable");
			}
		}

		// Token: 0x1700146C RID: 5228
		// (get) Token: 0x06003E85 RID: 16005 RVA: 0x0032CAEF File Offset: 0x0032ACEF
		// (set) Token: 0x06003E86 RID: 16006 RVA: 0x0032CAF7 File Offset: 0x0032ACF7
		public string ProgressString
		{
			get
			{
				return this._ProgressString;
			}
			set
			{
				this._ProgressString = value;
				this.OnPropertyChanged("ProgressString");
			}
		}

		// Token: 0x1700146D RID: 5229
		// (get) Token: 0x06003E87 RID: 16007 RVA: 0x0032CB0B File Offset: 0x0032AD0B
		// (set) Token: 0x06003E88 RID: 16008 RVA: 0x0032CB13 File Offset: 0x0032AD13
		public bool IsWorkInProgress
		{
			get
			{
				return this._IsWorkInProgress;
			}
			set
			{
				this._IsWorkInProgress = value;
				this.OnPropertyChanged("IsWorkInProgress");
			}
		}

		// Token: 0x1700146E RID: 5230
		// (get) Token: 0x06003E89 RID: 16009 RVA: 0x0032CB27 File Offset: 0x0032AD27
		public ICommand StartCommand
		{
			get
			{
				return new Command(delegate
				{
					this.Start();
				});
			}
		}

		// Token: 0x1700146F RID: 5231
		// (get) Token: 0x06003E8A RID: 16010 RVA: 0x0032CB3A File Offset: 0x0032AD3A
		public ICommand StopCommand
		{
			get
			{
				return new Command(delegate
				{
					App.OBDReader.ClearRequestQueue();
				});
			}
		}

		// Token: 0x17001470 RID: 5232
		// (get) Token: 0x06003E8B RID: 16011 RVA: 0x0032CB60 File Offset: 0x0032AD60
		public ICommand ShareCommand
		{
			get
			{
				return new Command(async delegate
				{
					localpath = FileSystemHelper.GetLocalFilePath("scan_" + DateTimeNowHelper.NowSafe.Ticks.ToString() + ".txt");
					using (StreamWriter streamWriter = new StreamWriter(localpath, false, Encoding.UTF8))
					{
						streamWriter.Write(this.ScanResults.GetJson());
						streamWriter.Flush();
					}
					await App.GetCurrentPage().DisplayAlert(Translate.GetString("dataCollectionPage_SendToNotes"), "E -mail: admin@carscanner.info", "OK");
					await Share.RequestAsync(new ShareFileRequest("Send to: admin@carscanner.info", new ShareFile(localpath)));
				});
			}
		}

		// Token: 0x17001471 RID: 5233
		// (get) Token: 0x06003E8C RID: 16012 RVA: 0x0032CB73 File Offset: 0x0032AD73
		public ICommand SaveFileCommand
		{
			get
			{
				return new Command(async delegate
				{
					localpath = FileSystemHelper.GetLocalFilePath("scan_" + DateTimeNowHelper.NowSafe.Ticks.ToString() + ".txt");
					using (StreamWriter streamWriter = new StreamWriter(localpath, false, Encoding.UTF8))
					{
						streamWriter.Write(this.ScanResults.GetJson());
						streamWriter.Flush();
					}
					if (PlatformHelper.IsAndroid)
					{
						await App.GetCurrentPage().DisplayAlert(Translate.GetString("dataCollectionPage_SendToNotes"), "E-mail: admin@carscanner.info", "OK");
						await Share.RequestAsync(new ShareFileRequest(new ShareFile(localpath)));
					}
					else
					{
						Share.RequestAsync(new ShareFileRequest("Send this report to admin@carscanner.info", new ShareFile(localpath)));
					}
				});
			}
		}

		// Token: 0x17001472 RID: 5234
		// (get) Token: 0x06003E8D RID: 16013 RVA: 0x0032CB86 File Offset: 0x0032AD86
		// (set) Token: 0x06003E8E RID: 16014 RVA: 0x0032CB8E File Offset: 0x0032AD8E
		public string SelectedTask
		{
			get
			{
				return this._SelectedTask;
			}
			set
			{
				this._SelectedTask = value;
				this.OnPropertyChanged("SelectedTask");
			}
		}

		// Token: 0x17001473 RID: 5235
		// (get) Token: 0x06003E8F RID: 16015 RVA: 0x0032CBA2 File Offset: 0x0032ADA2
		public ICommand RestartCommand
		{
			get
			{
				return new Command(delegate
				{
					this.ScanResults.Clear();
					this.IsResultsAvailable = false;
					this.IsWorkInProgress = false;
				});
			}
		}

		// Token: 0x06003E90 RID: 16016 RVA: 0x0032CBB8 File Offset: 0x0032ADB8
		private async Task Start()
		{
			List<OBDRequest> requests = new List<OBDRequest>(0);
			string selectedTask = this.SelectedTask;
			if (selectedTask != null)
			{
				if (!(selectedTask == "Renault: ABS, BCM, Dashboard, Engine, EMM, HVAC"))
				{
					if (!(selectedTask == "VAG: UDS FULL"))
					{
						if (selectedTask == "VAG: UDS 01, 02, 03, 08, 09, 17,36, 42, 44, 47, 52, 5F, 65, 6C, 76, A5")
						{
							requests = this.BuildRequestsForVAG_UDS_UNITS_FOR_CODING().ToList<OBDRequest>();
						}
					}
					else
					{
						requests = this.BuildRequestsForVAG_UDS_FULL().ToList<OBDRequest>();
					}
				}
				else
				{
					requests = this.BuildRequestsForRenault_RENAULT_ABS_BCM_DASH_ENGINE().ToList<OBDRequest>();
				}
				for (int i = 0; i < requests.Count; i++)
				{
					string s = string.Format("{0} / {1}", i + 1, requests.Count);
					Action <>9__1;
					requests[i].ResponseReceived += delegate(OBDRequest request, string data)
					{
						Action action;
						if ((action = <>9__1) == null)
						{
							action = (<>9__1 = delegate
							{
								this.ProgressString = s;
							});
						}
						MainThread.BeginInvokeOnMainThread(action);
					};
				}
				this.IsResultsAvailable = false;
				this.ScanResults.Clear();
				this.ScanResults.Task = this.SelectedTask;
				this.IsWorkInProgress = true;
				await App.OBDReader.DebugWrite("\r\n[SCAN TASK=" + this.SelectedTask + "]\r\n");
				App.OBDReader.ReplaceQueue(requests);
				await App.OBDReader.WaitForCommandQueue();
				this.IsWorkInProgress = false;
				this.IsResultsAvailable = true;
			}
		}

		// Token: 0x17001474 RID: 5236
		// (get) Token: 0x06003E91 RID: 16017 RVA: 0x0032CBFB File Offset: 0x0032ADFB
		// (set) Token: 0x06003E92 RID: 16018 RVA: 0x0032CC03 File Offset: 0x0032AE03
		public ScanContainer ScanResults
		{
			[CompilerGenerated]
			get
			{
				return this.<ScanResults>k__BackingField;
			}
			[CompilerGenerated]
			private set
			{
				this.<ScanResults>k__BackingField = value;
			}
		} = new ScanContainer();

		// Token: 0x06003E93 RID: 16019 RVA: 0x0032CC0C File Offset: 0x0032AE0C
		private IEnumerable<OBDRequest> BuildRequestsForRenault_RENAULT_ABS_BCM_DASH_ENGINE()
		{
			return this.BuildRequestsForRenault(new string[] { "7E0", "740", "743", "745", "74D", "744" });
		}

		// Token: 0x06003E94 RID: 16020 RVA: 0x0032CC4C File Offset: 0x0032AE4C
		private IEnumerable<OBDRequest> BuildRequestsForRenault(string[] headers)
		{
			List<OBDRequest> list = new List<OBDRequest>();
			foreach (string text in headers)
			{
				string possibleResponseHeader = CAN11bitHelper.GetPossibleResponseHeader(text, "Renault", null);
				string text2 = string.Concat(new string[] { "ATSP6;ATFCSH", text, ";ATFCSD300000;ATFCSM1;ATCRA", possibleResponseHeader, ";10C0" });
				string text3 = "ATFCSM0;ATAR";
				string[] array = new string[] { "2180", "22F1A0", "22F18A", "22F194", "22F195" };
				for (int j = 0; j < array.Length; j++)
				{
					string text4 = array[j];
					OBDRequest obdrequest = new OBDRequest(text4, text, text2, text3, false);
					obdrequest.CheckLength = true;
					obdrequest.ELMFormat = ELMFormat.CAN11bit;
					ResultContainer container = new ResultContainer
					{
						RequestHeader = text,
						ResponseHeader = possibleResponseHeader,
						Command = text4
					};
					obdrequest.ResponseReceived += CAN11bitHelper.Request_ResponseReceivedCheckForNegativeResponseAndForCancellation;
					obdrequest.ResponseReceived += delegate(OBDRequest request, string data)
					{
						container.ResponseRaw = data;
						if (!this.ScanResults.Contains(container))
						{
							this.ScanResults.Add(container);
						}
					};
					obdrequest.ResponseDecoded += delegate(OBDRequest request, byte[] data, bool decodeResult, string decodedResponseHeader)
					{
						if (data != null && data.Length != 0)
						{
							if (request.Command == "2180")
							{
								App.OBDReader.RemoveFromQueue((OBDRequest x) => x.Header == request.Header);
							}
							container.ResponseDecoded = BitHelpers.ByteArrayToHexString(data);
						}
					};
					list.Add(obdrequest);
				}
			}
			return list;
		}

		// Token: 0x06003E95 RID: 16021 RVA: 0x0032CDA4 File Offset: 0x0032AFA4
		private IEnumerable<OBDRequest> BuildRequestsForVAG_UDS_FULL()
		{
			string[] array = new string[]
			{
				"01", "02", "03", "04", "05", "06", "08", "09", "0E", "10",
				"11", "13", "14", "15", "16", "17", "18", "1B", "20", "21",
				"22", "23", "25", "26", "28", "32", "34", "36", "37", "38",
				"3C", "3D", "42", "44", "47", "51", "52", "53", "55", "57",
				"5F", "65", "69", "6C", "6D", "6F", "71", "75", "76", "77",
				"7F", "8C", "94", "95", "A5", "AD", "BB", "BC", "BD", "C6"
			};
			return this.BuildRequestsForVAG_UDS(array);
		}

		// Token: 0x06003E96 RID: 16022 RVA: 0x0032CFD4 File Offset: 0x0032B1D4
		private IEnumerable<OBDRequest> BuildRequestsForVAG_UDS_UNITS_FOR_CODING()
		{
			string[] array = new string[]
			{
				"01", "02", "03", "08", "09", "17", "36", "42", "44", "47",
				"52", "5F", "65", "6C", "76", "A5"
			};
			return this.BuildRequestsForVAG_UDS(array);
		}

		// Token: 0x06003E97 RID: 16023 RVA: 0x0032D078 File Offset: 0x0032B278
		private IEnumerable<OBDRequest> BuildRequestsForVAG_UDS(string[] units)
		{
			List<OBDRequest> list = new List<OBDRequest>();
			string[] commands = "1003;0902;222A2A;222A2E;22F19E;22F1A2;22F187;22F189;22F191;22F1A3;22F1A5;22F1DF;22F197;220601;220600;220606;220607;220608".Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
			foreach (string text in units)
			{
				string requestHeaderForMQBUnit = VagUnitHelper.GetRequestHeaderForMQBUnit(text);
				string responseHeaderForMQBUnit = VagUnitHelper.GetResponseHeaderForMQBUnit(text);
				string text2 = "ATST96;ATSP6;ATFCSH" + requestHeaderForMQBUnit + ";ATFCSD300000;ATFCSM1;ATCRA" + responseHeaderForMQBUnit;
				string text3 = "ATFCSM0;ATAR";
				string[] commands2 = commands;
				for (int j = 0; j < commands2.Length; j++)
				{
					string text4 = commands2[j];
					OBDRequest obdrequest = new OBDRequest(text4, requestHeaderForMQBUnit, text2, text3, false);
					obdrequest.CheckLength = true;
					obdrequest.ELMFormat = ELMFormat.CAN11bit;
					ResultContainer container = new ResultContainer
					{
						RequestHeader = requestHeaderForMQBUnit,
						ResponseHeader = responseHeaderForMQBUnit,
						Command = text4
					};
					obdrequest.ResponseReceived += CAN11bitHelper.Request_ResponseReceivedCheckForNegativeResponseAndForCancellation;
					obdrequest.ResponseReceived += delegate(OBDRequest request, string data)
					{
						if ((request.Command == commands[0] && data == null) || data.Contains("NO DATA"))
						{
							App.OBDReader.RemoveFromQueue((OBDRequest x) => x.Header == request.Header);
						}
						container.ResponseRaw = data;
						if (!this.ScanResults.Contains(container))
						{
							this.ScanResults.Add(container);
						}
					};
					obdrequest.ResponseDecoded += delegate(OBDRequest request, byte[] data, bool decodeResult, string decodedResponseHeader)
					{
						if (data != null && data.Length != 0)
						{
							container.ResponseDecoded = BitHelpers.ByteArrayToHexString(data);
						}
					};
					list.Add(obdrequest);
				}
			}
			return list;
		}

		// Token: 0x06003E98 RID: 16024 RVA: 0x0032D1B7 File Offset: 0x0032B3B7
		[CompilerGenerated]
		private void <get_StartCommand>b__27_0()
		{
			this.Start();
		}

		// Token: 0x06003E99 RID: 16025 RVA: 0x0032D1C0 File Offset: 0x0032B3C0
		[CompilerGenerated]
		private async void <get_ShareCommand>b__31_0()
		{
			string localpath = FileSystemHelper.GetLocalFilePath("scan_" + DateTimeNowHelper.NowSafe.Ticks.ToString() + ".txt");
			using (StreamWriter streamWriter = new StreamWriter(localpath, false, Encoding.UTF8))
			{
				streamWriter.Write(this.ScanResults.GetJson());
				streamWriter.Flush();
			}
			await App.GetCurrentPage().DisplayAlert(Translate.GetString("dataCollectionPage_SendToNotes"), "E -mail: admin@carscanner.info", "OK");
			await Share.RequestAsync(new ShareFileRequest("Send to: admin@carscanner.info", new ShareFile(localpath)));
		}

		// Token: 0x06003E9A RID: 16026 RVA: 0x0032D1F8 File Offset: 0x0032B3F8
		[CompilerGenerated]
		private async void <get_SaveFileCommand>b__33_0()
		{
			string localpath = FileSystemHelper.GetLocalFilePath("scan_" + DateTimeNowHelper.NowSafe.Ticks.ToString() + ".txt");
			using (StreamWriter streamWriter = new StreamWriter(localpath, false, Encoding.UTF8))
			{
				streamWriter.Write(this.ScanResults.GetJson());
				streamWriter.Flush();
			}
			if (PlatformHelper.IsAndroid)
			{
				await App.GetCurrentPage().DisplayAlert(Translate.GetString("dataCollectionPage_SendToNotes"), "E-mail: admin@carscanner.info", "OK");
				await Share.RequestAsync(new ShareFileRequest(new ShareFile(localpath)));
			}
			else
			{
				Share.RequestAsync(new ShareFileRequest("Send this report to admin@carscanner.info", new ShareFile(localpath)));
			}
		}

		// Token: 0x06003E9B RID: 16027 RVA: 0x0032D22F File Offset: 0x0032B42F
		[CompilerGenerated]
		private void <get_RestartCommand>b__39_0()
		{
			this.ScanResults.Clear();
			this.IsResultsAvailable = false;
			this.IsWorkInProgress = false;
		}

		// Token: 0x04002656 RID: 9814
		private const string RENAULT_ABS_BCM_DASH_ENGINE = "Renault: ABS, BCM, Dashboard, Engine, EMM, HVAC";

		// Token: 0x04002657 RID: 9815
		private const string VAG_FULL = "VAG: UDS FULL";

		// Token: 0x04002658 RID: 9816
		private const string VAG_FULL_UDS_CODING = "VAG: UDS 01, 02, 03, 08, 09, 17,36, 42, 44, 47, 52, 5F, 65, 6C, 76, A5";

		// Token: 0x04002659 RID: 9817
		private const string HYUNDAI_KIA_FULL = "Hyundai/Kia full";

		// Token: 0x0400265A RID: 9818
		private const string TOYOTA_FULL = "Toyota CAN Full";

		// Token: 0x0400265B RID: 9819
		[CompilerGenerated]
		private PropertyChangedEventHandler PropertyChanged;

		// Token: 0x0400265C RID: 9820
		[CompilerGenerated]
		private ObservableCollection<string> <Tasks>k__BackingField;

		// Token: 0x0400265D RID: 9821
		private bool _IsResultsAvailable;

		// Token: 0x0400265E RID: 9822
		private string _ProgressString = "";

		// Token: 0x0400265F RID: 9823
		private bool _IsWorkInProgress;

		// Token: 0x04002660 RID: 9824
		private string _SelectedTask;

		// Token: 0x04002661 RID: 9825
		[CompilerGenerated]
		private ScanContainer <ScanResults>k__BackingField;

		// Token: 0x0200072C RID: 1836
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <<get_SaveFileCommand>b__33_0>d : IAsyncStateMachine
		{
			// Token: 0x06003E9C RID: 16028 RVA: 0x0032D24C File Offset: 0x0032B44C
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				CollectDataModel collectDataModel = this;
				try
				{
					TaskAwaiter taskAwaiter;
					if (num != 0)
					{
						if (num == 1)
						{
							TaskAwaiter taskAwaiter2;
							taskAwaiter = taskAwaiter2;
							taskAwaiter2 = default(TaskAwaiter);
							num = (num2 = -1);
							goto IL_016B;
						}
						localpath = FileSystemHelper.GetLocalFilePath("scan_" + DateTimeNowHelper.NowSafe.Ticks.ToString() + ".txt");
						StreamWriter streamWriter = new StreamWriter(localpath, false, Encoding.UTF8);
						try
						{
							streamWriter.Write(collectDataModel.ScanResults.GetJson());
							streamWriter.Flush();
						}
						finally
						{
							if (num < 0 && streamWriter != null)
							{
								((IDisposable)streamWriter).Dispose();
							}
						}
						if (!PlatformHelper.IsAndroid)
						{
							Share.RequestAsync(new ShareFileRequest("Send this report to admin@carscanner.info", new ShareFile(localpath)));
							goto IL_018F;
						}
						taskAwaiter = App.GetCurrentPage().DisplayAlert(Translate.GetString("dataCollectionPage_SendToNotes"), "E-mail: admin@carscanner.info", "OK").GetAwaiter();
						if (!taskAwaiter.IsCompleted)
						{
							num = (num2 = 0);
							TaskAwaiter taskAwaiter2 = taskAwaiter;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, CollectDataModel.<<get_SaveFileCommand>b__33_0>d>(ref taskAwaiter, ref this);
							return;
						}
					}
					else
					{
						TaskAwaiter taskAwaiter2;
						taskAwaiter = taskAwaiter2;
						taskAwaiter2 = default(TaskAwaiter);
						num = (num2 = -1);
					}
					taskAwaiter.GetResult();
					taskAwaiter = Share.RequestAsync(new ShareFileRequest(new ShareFile(localpath))).GetAwaiter();
					if (!taskAwaiter.IsCompleted)
					{
						num = (num2 = 1);
						TaskAwaiter taskAwaiter2 = taskAwaiter;
						this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, CollectDataModel.<<get_SaveFileCommand>b__33_0>d>(ref taskAwaiter, ref this);
						return;
					}
					IL_016B:
					taskAwaiter.GetResult();
					IL_018F:;
				}
				catch (Exception ex)
				{
					num2 = -2;
					localpath = null;
					this.<>t__builder.SetException(ex);
					return;
				}
				num2 = -2;
				localpath = null;
				this.<>t__builder.SetResult();
			}

			// Token: 0x06003E9D RID: 16029 RVA: 0x0032D458 File Offset: 0x0032B658
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x04002662 RID: 9826
			public int <>1__state;

			// Token: 0x04002663 RID: 9827
			public AsyncVoidMethodBuilder <>t__builder;

			// Token: 0x04002664 RID: 9828
			public CollectDataModel <>4__this;

			// Token: 0x04002665 RID: 9829
			private string <localpath>5__2;

			// Token: 0x04002666 RID: 9830
			private TaskAwaiter <>u__1;
		}

		// Token: 0x0200072D RID: 1837
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <<get_ShareCommand>b__31_0>d : IAsyncStateMachine
		{
			// Token: 0x06003E9E RID: 16030 RVA: 0x0032D468 File Offset: 0x0032B668
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				CollectDataModel collectDataModel = this;
				try
				{
					TaskAwaiter taskAwaiter;
					if (num != 0)
					{
						if (num == 1)
						{
							TaskAwaiter taskAwaiter2;
							taskAwaiter = taskAwaiter2;
							taskAwaiter2 = default(TaskAwaiter);
							num = (num2 = -1);
							goto IL_0166;
						}
						localpath = FileSystemHelper.GetLocalFilePath("scan_" + DateTimeNowHelper.NowSafe.Ticks.ToString() + ".txt");
						StreamWriter streamWriter = new StreamWriter(localpath, false, Encoding.UTF8);
						try
						{
							streamWriter.Write(collectDataModel.ScanResults.GetJson());
							streamWriter.Flush();
						}
						finally
						{
							if (num < 0 && streamWriter != null)
							{
								((IDisposable)streamWriter).Dispose();
							}
						}
						taskAwaiter = App.GetCurrentPage().DisplayAlert(Translate.GetString("dataCollectionPage_SendToNotes"), "E -mail: admin@carscanner.info", "OK").GetAwaiter();
						if (!taskAwaiter.IsCompleted)
						{
							num = (num2 = 0);
							TaskAwaiter taskAwaiter2 = taskAwaiter;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, CollectDataModel.<<get_ShareCommand>b__31_0>d>(ref taskAwaiter, ref this);
							return;
						}
					}
					else
					{
						TaskAwaiter taskAwaiter2;
						taskAwaiter = taskAwaiter2;
						taskAwaiter2 = default(TaskAwaiter);
						num = (num2 = -1);
					}
					taskAwaiter.GetResult();
					taskAwaiter = Share.RequestAsync(new ShareFileRequest("Send to: admin@carscanner.info", new ShareFile(localpath))).GetAwaiter();
					if (!taskAwaiter.IsCompleted)
					{
						num = (num2 = 1);
						TaskAwaiter taskAwaiter2 = taskAwaiter;
						this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, CollectDataModel.<<get_ShareCommand>b__31_0>d>(ref taskAwaiter, ref this);
						return;
					}
					IL_0166:
					taskAwaiter.GetResult();
				}
				catch (Exception ex)
				{
					num2 = -2;
					localpath = null;
					this.<>t__builder.SetException(ex);
					return;
				}
				num2 = -2;
				localpath = null;
				this.<>t__builder.SetResult();
			}

			// Token: 0x06003E9F RID: 16031 RVA: 0x0032D654 File Offset: 0x0032B854
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x04002667 RID: 9831
			public int <>1__state;

			// Token: 0x04002668 RID: 9832
			public AsyncVoidMethodBuilder <>t__builder;

			// Token: 0x04002669 RID: 9833
			public CollectDataModel <>4__this;

			// Token: 0x0400266A RID: 9834
			private string <localpath>5__2;

			// Token: 0x0400266B RID: 9835
			private TaskAwaiter <>u__1;
		}

		// Token: 0x0200072E RID: 1838
		[CompilerGenerated]
		[Serializable]
		private sealed class <>c
		{
			// Token: 0x06003EA0 RID: 16032 RVA: 0x0032D662 File Offset: 0x0032B862
			// Note: this type is marked as 'beforefieldinit'.
			static <>c()
			{
			}

			// Token: 0x06003EA1 RID: 16033 RVA: 0x00002050 File Offset: 0x00000250
			public <>c()
			{
			}

			// Token: 0x06003EA2 RID: 16034 RVA: 0x0032D66E File Offset: 0x0032B86E
			internal bool <.ctor>b__5_0(string x)
			{
				return Path.GetFileName(x).StartsWith("scan_");
			}

			// Token: 0x06003EA3 RID: 16035 RVA: 0x0032D680 File Offset: 0x0032B880
			internal void <get_StopCommand>b__29_0()
			{
				App.OBDReader.ClearRequestQueue();
			}

			// Token: 0x0400266C RID: 9836
			public static readonly CollectDataModel.<>c <>9 = new CollectDataModel.<>c();

			// Token: 0x0400266D RID: 9837
			public static Func<string, bool> <>9__5_0;

			// Token: 0x0400266E RID: 9838
			public static Action <>9__29_0;
		}

		// Token: 0x0200072F RID: 1839
		[CompilerGenerated]
		private sealed class <>c__DisplayClass40_0
		{
			// Token: 0x06003EA4 RID: 16036 RVA: 0x00002050 File Offset: 0x00000250
			public <>c__DisplayClass40_0()
			{
			}

			// Token: 0x06003EA5 RID: 16037 RVA: 0x0032D690 File Offset: 0x0032B890
			internal void <Start>b__0(OBDRequest request, string data)
			{
				Action action;
				if ((action = this.<>9__1) == null)
				{
					action = (this.<>9__1 = delegate
					{
						this.<>4__this.ProgressString = this.s;
					});
				}
				MainThread.BeginInvokeOnMainThread(action);
			}

			// Token: 0x06003EA6 RID: 16038 RVA: 0x0032D6C1 File Offset: 0x0032B8C1
			internal void <Start>b__1()
			{
				this.<>4__this.ProgressString = this.s;
			}

			// Token: 0x0400266F RID: 9839
			public string s;

			// Token: 0x04002670 RID: 9840
			public CollectDataModel <>4__this;

			// Token: 0x04002671 RID: 9841
			public Action <>9__1;
		}

		// Token: 0x02000730 RID: 1840
		[CompilerGenerated]
		private sealed class <>c__DisplayClass46_0
		{
			// Token: 0x06003EA7 RID: 16039 RVA: 0x00002050 File Offset: 0x00000250
			public <>c__DisplayClass46_0()
			{
			}

			// Token: 0x06003EA8 RID: 16040 RVA: 0x0032D6D4 File Offset: 0x0032B8D4
			internal void <BuildRequestsForRenault>b__0(OBDRequest request, string data)
			{
				this.container.ResponseRaw = data;
				if (!this.<>4__this.ScanResults.Contains(this.container))
				{
					this.<>4__this.ScanResults.Add(this.container);
				}
			}

			// Token: 0x06003EA9 RID: 16041 RVA: 0x0032D710 File Offset: 0x0032B910
			internal void <BuildRequestsForRenault>b__1(OBDRequest request, byte[] data, bool decodeResult, string decodedResponseHeader)
			{
				CollectDataModel.<>c__DisplayClass46_1 CS$<>8__locals1 = new CollectDataModel.<>c__DisplayClass46_1();
				CS$<>8__locals1.request = request;
				if (data != null && data.Length != 0)
				{
					if (CS$<>8__locals1.request.Command == "2180")
					{
						App.OBDReader.RemoveFromQueue((OBDRequest x) => x.Header == CS$<>8__locals1.request.Header);
					}
					this.container.ResponseDecoded = BitHelpers.ByteArrayToHexString(data);
				}
			}

			// Token: 0x04002672 RID: 9842
			public ResultContainer container;

			// Token: 0x04002673 RID: 9843
			public CollectDataModel <>4__this;
		}

		// Token: 0x02000731 RID: 1841
		[CompilerGenerated]
		private sealed class <>c__DisplayClass46_1
		{
			// Token: 0x06003EAA RID: 16042 RVA: 0x00002050 File Offset: 0x00000250
			public <>c__DisplayClass46_1()
			{
			}

			// Token: 0x06003EAB RID: 16043 RVA: 0x0032D76F File Offset: 0x0032B96F
			internal bool <BuildRequestsForRenault>b__2(OBDRequest x)
			{
				return x.Header == this.request.Header;
			}

			// Token: 0x04002674 RID: 9844
			public OBDRequest request;
		}

		// Token: 0x02000732 RID: 1842
		[CompilerGenerated]
		private sealed class <>c__DisplayClass49_0
		{
			// Token: 0x06003EAC RID: 16044 RVA: 0x00002050 File Offset: 0x00000250
			public <>c__DisplayClass49_0()
			{
			}

			// Token: 0x04002675 RID: 9845
			public string[] commands;

			// Token: 0x04002676 RID: 9846
			public CollectDataModel <>4__this;
		}

		// Token: 0x02000733 RID: 1843
		[CompilerGenerated]
		private sealed class <>c__DisplayClass49_1
		{
			// Token: 0x06003EAD RID: 16045 RVA: 0x00002050 File Offset: 0x00000250
			public <>c__DisplayClass49_1()
			{
			}

			// Token: 0x06003EAE RID: 16046 RVA: 0x0032D788 File Offset: 0x0032B988
			internal void <BuildRequestsForVAG_UDS>b__0(OBDRequest request, string data)
			{
				CollectDataModel.<>c__DisplayClass49_2 CS$<>8__locals1 = new CollectDataModel.<>c__DisplayClass49_2();
				CS$<>8__locals1.request = request;
				if ((CS$<>8__locals1.request.Command == this.CS$<>8__locals1.commands[0] && data == null) || data.Contains("NO DATA"))
				{
					App.OBDReader.RemoveFromQueue((OBDRequest x) => x.Header == CS$<>8__locals1.request.Header);
				}
				this.container.ResponseRaw = data;
				if (!this.CS$<>8__locals1.<>4__this.ScanResults.Contains(this.container))
				{
					this.CS$<>8__locals1.<>4__this.ScanResults.Add(this.container);
				}
			}

			// Token: 0x06003EAF RID: 16047 RVA: 0x0032D82B File Offset: 0x0032BA2B
			internal void <BuildRequestsForVAG_UDS>b__1(OBDRequest request, byte[] data, bool decodeResult, string decodedResponseHeader)
			{
				if (data != null && data.Length != 0)
				{
					this.container.ResponseDecoded = BitHelpers.ByteArrayToHexString(data);
				}
			}

			// Token: 0x04002677 RID: 9847
			public ResultContainer container;

			// Token: 0x04002678 RID: 9848
			public CollectDataModel.<>c__DisplayClass49_0 CS$<>8__locals1;
		}

		// Token: 0x02000734 RID: 1844
		[CompilerGenerated]
		private sealed class <>c__DisplayClass49_2
		{
			// Token: 0x06003EB0 RID: 16048 RVA: 0x00002050 File Offset: 0x00000250
			public <>c__DisplayClass49_2()
			{
			}

			// Token: 0x06003EB1 RID: 16049 RVA: 0x0032D845 File Offset: 0x0032BA45
			internal bool <BuildRequestsForVAG_UDS>b__2(OBDRequest x)
			{
				return x.Header == this.request.Header;
			}

			// Token: 0x04002679 RID: 9849
			public OBDRequest request;
		}

		// Token: 0x02000735 RID: 1845
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <Start>d__40 : IAsyncStateMachine
		{
			// Token: 0x06003EB2 RID: 16050 RVA: 0x0032D860 File Offset: 0x0032BA60
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				CollectDataModel collectDataModel = this;
				try
				{
					TaskAwaiter taskAwaiter;
					if (num != 0)
					{
						if (num == 1)
						{
							TaskAwaiter taskAwaiter2;
							taskAwaiter = taskAwaiter2;
							taskAwaiter2 = default(TaskAwaiter);
							num2 = -1;
							goto IL_020B;
						}
						requests = new List<OBDRequest>(0);
						string selectedTask = collectDataModel.SelectedTask;
						if (selectedTask == null)
						{
							goto IL_0242;
						}
						if (!(selectedTask == "Renault: ABS, BCM, Dashboard, Engine, EMM, HVAC"))
						{
							if (!(selectedTask == "VAG: UDS FULL"))
							{
								if (selectedTask == "VAG: UDS 01, 02, 03, 08, 09, 17,36, 42, 44, 47, 52, 5F, 65, 6C, 76, A5")
								{
									requests = collectDataModel.BuildRequestsForVAG_UDS_UNITS_FOR_CODING().ToList<OBDRequest>();
								}
							}
							else
							{
								requests = collectDataModel.BuildRequestsForVAG_UDS_FULL().ToList<OBDRequest>();
							}
						}
						else
						{
							requests = collectDataModel.BuildRequestsForRenault_RENAULT_ABS_BCM_DASH_ENGINE().ToList<OBDRequest>();
						}
						for (int i = 0; i < requests.Count; i++)
						{
							CollectDataModel.<>c__DisplayClass40_0 CS$<>8__locals1 = new CollectDataModel.<>c__DisplayClass40_0();
							CS$<>8__locals1.<>4__this = collectDataModel;
							CS$<>8__locals1.s = string.Format("{0} / {1}", i + 1, requests.Count);
							requests[i].ResponseReceived += delegate(OBDRequest request, string data)
							{
								Action action;
								if ((action = CS$<>8__locals1.<>9__1) == null)
								{
									action = (CS$<>8__locals1.<>9__1 = delegate
									{
										CS$<>8__locals1.<>4__this.ProgressString = CS$<>8__locals1.s;
									});
								}
								MainThread.BeginInvokeOnMainThread(action);
							};
						}
						collectDataModel.IsResultsAvailable = false;
						collectDataModel.ScanResults.Clear();
						collectDataModel.ScanResults.Task = collectDataModel.SelectedTask;
						collectDataModel.IsWorkInProgress = true;
						taskAwaiter = App.OBDReader.DebugWrite("\r\n[SCAN TASK=" + collectDataModel.SelectedTask + "]\r\n").GetAwaiter();
						if (!taskAwaiter.IsCompleted)
						{
							num2 = 0;
							TaskAwaiter taskAwaiter2 = taskAwaiter;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, CollectDataModel.<Start>d__40>(ref taskAwaiter, ref this);
							return;
						}
					}
					else
					{
						TaskAwaiter taskAwaiter2;
						taskAwaiter = taskAwaiter2;
						taskAwaiter2 = default(TaskAwaiter);
						num2 = -1;
					}
					taskAwaiter.GetResult();
					App.OBDReader.ReplaceQueue(requests);
					taskAwaiter = App.OBDReader.WaitForCommandQueue().GetAwaiter();
					if (!taskAwaiter.IsCompleted)
					{
						num2 = 1;
						TaskAwaiter taskAwaiter2 = taskAwaiter;
						this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, CollectDataModel.<Start>d__40>(ref taskAwaiter, ref this);
						return;
					}
					IL_020B:
					taskAwaiter.GetResult();
					collectDataModel.IsWorkInProgress = false;
					collectDataModel.IsResultsAvailable = true;
				}
				catch (Exception ex)
				{
					num2 = -2;
					requests = null;
					this.<>t__builder.SetException(ex);
					return;
				}
				IL_0242:
				num2 = -2;
				requests = null;
				this.<>t__builder.SetResult();
			}

			// Token: 0x06003EB3 RID: 16051 RVA: 0x0032DAE8 File Offset: 0x0032BCE8
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x0400267A RID: 9850
			public int <>1__state;

			// Token: 0x0400267B RID: 9851
			public AsyncTaskMethodBuilder <>t__builder;

			// Token: 0x0400267C RID: 9852
			public CollectDataModel <>4__this;

			// Token: 0x0400267D RID: 9853
			private List<OBDRequest> <requests>5__2;

			// Token: 0x0400267E RID: 9854
			private TaskAwaiter <>u__1;
		}
	}
}
