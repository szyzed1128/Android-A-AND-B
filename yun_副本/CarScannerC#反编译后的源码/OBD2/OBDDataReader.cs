using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.ExceptionServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using CarScannerXamarinForms.CarPlay;
using CarScannerXamarinForms.Common;
using CarScannerXamarinForms.DataRecorder;
using CarScannerXamarinForms.OBD2.PIDS;
using CarScannerXamarinForms.OBD2.PIDS.CalculatedPIDs;
using CarScannerXamarinForms.OBD2.VWTP20;
using CarScannerXamarinForms.PlatformAdapters;
using CarScannerXamarinForms.ProfilesV2;
using CarScannerXamarinForms.Settings;
using CarScannerXamarinForms.UserControls;
using CarScannerXamarinForms.ViewModels;
using Plugin.BLE;
using Plugin.BLE.Abstractions.Contracts;
using Plugin.BLE.Abstractions.EventArgs;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Internals;

namespace CarScannerXamarinForms.OBD2
{
	// Token: 0x02000328 RID: 808
	public class OBDDataReader : INotifyPropertyChanged
	{
		// Token: 0x060024BC RID: 9404 RVA: 0x001C2660 File Offset: 0x001C0860
		public OBDDataReader()
		{
			this.CurrentMode = OBDDataReader.OBDModes.Universal;
			this.CurrentStatus = OBDDataReaderStatus.Disconnected;
			this._ECUHeaders = new ObservableCollection<ECUHeader>();
			this._ECUHeaders.CollectionChanged += this._ECUHeaders_CollectionChanged;
			this.stopwatch = new Stopwatch();
			this.stopwatch.Start();
		}

		// Token: 0x17001172 RID: 4466
		// (get) Token: 0x060024BD RID: 9405 RVA: 0x001C2758 File Offset: 0x001C0958
		private IVWTPManager vwTPManager
		{
			get
			{
				if (this._vwTPManager == null)
				{
					this._vwTPManager = new VWTPManager();
				}
				return this._vwTPManager;
			}
		}

		// Token: 0x17001173 RID: 4467
		// (get) Token: 0x060024BE RID: 9406 RVA: 0x001C2773 File Offset: 0x001C0973
		// (set) Token: 0x060024BF RID: 9407 RVA: 0x001C277B File Offset: 0x001C097B
		internal ELMState ELMStatus
		{
			[CompilerGenerated]
			get
			{
				return this.<ELMStatus>k__BackingField;
			}
			[CompilerGenerated]
			private set
			{
				this.<ELMStatus>k__BackingField = value;
			}
		} = new ELMState();

		// Token: 0x1400001D RID: 29
		// (add) Token: 0x060024C0 RID: 9408 RVA: 0x001C2784 File Offset: 0x001C0984
		// (remove) Token: 0x060024C1 RID: 9409 RVA: 0x001C27BC File Offset: 0x001C09BC
		public event EventHandler<int> DTCReadingQueueProgress
		{
			[CompilerGenerated]
			add
			{
				EventHandler<int> eventHandler = this.DTCReadingQueueProgress;
				EventHandler<int> eventHandler2;
				do
				{
					eventHandler2 = eventHandler;
					EventHandler<int> eventHandler3 = (EventHandler<int>)Delegate.Combine(eventHandler2, value);
					eventHandler = Interlocked.CompareExchange<EventHandler<int>>(ref this.DTCReadingQueueProgress, eventHandler3, eventHandler2);
				}
				while (eventHandler != eventHandler2);
			}
			[CompilerGenerated]
			remove
			{
				EventHandler<int> eventHandler = this.DTCReadingQueueProgress;
				EventHandler<int> eventHandler2;
				do
				{
					eventHandler2 = eventHandler;
					EventHandler<int> eventHandler3 = (EventHandler<int>)Delegate.Remove(eventHandler2, value);
					eventHandler = Interlocked.CompareExchange<EventHandler<int>>(ref this.DTCReadingQueueProgress, eventHandler3, eventHandler2);
				}
				while (eventHandler != eventHandler2);
			}
		}

		// Token: 0x060024C2 RID: 9410 RVA: 0x001C27F1 File Offset: 0x001C09F1
		public void SetELM327_LastSentHeader(string header)
		{
			this.ELM327_LastSentHeader = header;
		}

		// Token: 0x17001174 RID: 4468
		// (get) Token: 0x060024C3 RID: 9411 RVA: 0x001C27FA File Offset: 0x001C09FA
		// (set) Token: 0x060024C4 RID: 9412 RVA: 0x001C2802 File Offset: 0x001C0A02
		public bool STCommandsStupported
		{
			[CompilerGenerated]
			get
			{
				return this.<STCommandsStupported>k__BackingField;
			}
			[CompilerGenerated]
			private set
			{
				this.<STCommandsStupported>k__BackingField = value;
			}
		}

		// Token: 0x17001175 RID: 4469
		// (get) Token: 0x060024C5 RID: 9413 RVA: 0x001C280B File Offset: 0x001C0A0B
		// (set) Token: 0x060024C6 RID: 9414 RVA: 0x001C2813 File Offset: 0x001C0A13
		public bool VTCommandsStupported
		{
			[CompilerGenerated]
			get
			{
				return this.<VTCommandsStupported>k__BackingField;
			}
			[CompilerGenerated]
			private set
			{
				this.<VTCommandsStupported>k__BackingField = value;
			}
		}

		// Token: 0x17001176 RID: 4470
		// (get) Token: 0x060024C7 RID: 9415 RVA: 0x001C281C File Offset: 0x001C0A1C
		// (set) Token: 0x060024C8 RID: 9416 RVA: 0x001C2824 File Offset: 0x001C0A24
		public OBDDataReader.OBDModes CurrentMode
		{
			get
			{
				return this._CurrentMode;
			}
			set
			{
				object obj = this.curModeLock;
				lock (obj)
				{
					this._CurrentMode = value;
				}
			}
		}

		// Token: 0x1400001E RID: 30
		// (add) Token: 0x060024C9 RID: 9417 RVA: 0x001C2868 File Offset: 0x001C0A68
		// (remove) Token: 0x060024CA RID: 9418 RVA: 0x001C28A0 File Offset: 0x001C0AA0
		protected event EventHandler QueueCompleted
		{
			[CompilerGenerated]
			add
			{
				EventHandler eventHandler = this.QueueCompleted;
				EventHandler eventHandler2;
				do
				{
					eventHandler2 = eventHandler;
					EventHandler eventHandler3 = (EventHandler)Delegate.Combine(eventHandler2, value);
					eventHandler = Interlocked.CompareExchange<EventHandler>(ref this.QueueCompleted, eventHandler3, eventHandler2);
				}
				while (eventHandler != eventHandler2);
			}
			[CompilerGenerated]
			remove
			{
				EventHandler eventHandler = this.QueueCompleted;
				EventHandler eventHandler2;
				do
				{
					eventHandler2 = eventHandler;
					EventHandler eventHandler3 = (EventHandler)Delegate.Remove(eventHandler2, value);
					eventHandler = Interlocked.CompareExchange<EventHandler>(ref this.QueueCompleted, eventHandler3, eventHandler2);
				}
				while (eventHandler != eventHandler2);
			}
		}

		// Token: 0x17001177 RID: 4471
		// (get) Token: 0x060024CB RID: 9419 RVA: 0x001C28D5 File Offset: 0x001C0AD5
		protected ConcurrentQueue<OBDRequest> CommandQueue
		{
			get
			{
				return this._CommandQueue;
			}
		}

		// Token: 0x17001178 RID: 4472
		// (get) Token: 0x060024CC RID: 9420 RVA: 0x001C28DF File Offset: 0x001C0ADF
		private bool Running
		{
			get
			{
				return this._Running;
			}
		}

		// Token: 0x060024CD RID: 9421 RVA: 0x001C28E9 File Offset: 0x001C0AE9
		private void SetRunning(bool val, string predstavsa_mraz)
		{
			this.DebugWrite(string.Format("\r\n[SetRunning({0},{1})]\r\n", val, predstavsa_mraz));
			this._Running = val;
		}

		// Token: 0x1400001F RID: 31
		// (add) Token: 0x060024CE RID: 9422 RVA: 0x001C290C File Offset: 0x001C0B0C
		// (remove) Token: 0x060024CF RID: 9423 RVA: 0x001C2944 File Offset: 0x001C0B44
		public event CurrentStatusChangedEvent StatusChanged
		{
			[CompilerGenerated]
			add
			{
				CurrentStatusChangedEvent currentStatusChangedEvent = this.StatusChanged;
				CurrentStatusChangedEvent currentStatusChangedEvent2;
				do
				{
					currentStatusChangedEvent2 = currentStatusChangedEvent;
					CurrentStatusChangedEvent currentStatusChangedEvent3 = (CurrentStatusChangedEvent)Delegate.Combine(currentStatusChangedEvent2, value);
					currentStatusChangedEvent = Interlocked.CompareExchange<CurrentStatusChangedEvent>(ref this.StatusChanged, currentStatusChangedEvent3, currentStatusChangedEvent2);
				}
				while (currentStatusChangedEvent != currentStatusChangedEvent2);
			}
			[CompilerGenerated]
			remove
			{
				CurrentStatusChangedEvent currentStatusChangedEvent = this.StatusChanged;
				CurrentStatusChangedEvent currentStatusChangedEvent2;
				do
				{
					currentStatusChangedEvent2 = currentStatusChangedEvent;
					CurrentStatusChangedEvent currentStatusChangedEvent3 = (CurrentStatusChangedEvent)Delegate.Remove(currentStatusChangedEvent2, value);
					currentStatusChangedEvent = Interlocked.CompareExchange<CurrentStatusChangedEvent>(ref this.StatusChanged, currentStatusChangedEvent3, currentStatusChangedEvent2);
				}
				while (currentStatusChangedEvent != currentStatusChangedEvent2);
			}
		}

		// Token: 0x060024D0 RID: 9424 RVA: 0x001C2979 File Offset: 0x001C0B79
		internal void ClearPendingAfterCommands()
		{
			this.ELM327_PendingAfterCommands = new string[0];
		}

		// Token: 0x17001179 RID: 4473
		// (get) Token: 0x060024D1 RID: 9425 RVA: 0x001C2987 File Offset: 0x001C0B87
		// (set) Token: 0x060024D2 RID: 9426 RVA: 0x001C298F File Offset: 0x001C0B8F
		public int CurrentProtocolNumber
		{
			[CompilerGenerated]
			get
			{
				return this.<CurrentProtocolNumber>k__BackingField;
			}
			[CompilerGenerated]
			protected set
			{
				this.<CurrentProtocolNumber>k__BackingField = value;
			}
		}

		// Token: 0x1700117A RID: 4474
		// (get) Token: 0x060024D3 RID: 9427 RVA: 0x001C2998 File Offset: 0x001C0B98
		public ObservableCollection<ECUHeader> ECUHeaders
		{
			get
			{
				return this._ECUHeaders;
			}
		}

		// Token: 0x1700117B RID: 4475
		// (get) Token: 0x060024D4 RID: 9428 RVA: 0x001C29A0 File Offset: 0x001C0BA0
		// (set) Token: 0x060024D5 RID: 9429 RVA: 0x001C29B6 File Offset: 0x001C0BB6
		public bool BadELM
		{
			get
			{
				return SharedSettings.Current.ShowBadELMWarning && this._BadELM;
			}
			set
			{
				if (value != this._BadELM)
				{
					this._BadELM = value;
					this.NotifyPropertyChanged("BadELM");
				}
			}
		}

		// Token: 0x060024D6 RID: 9430 RVA: 0x001C29D3 File Offset: 0x001C0BD3
		public int GetQueueCount()
		{
			return this.CommandQueue.Count;
		}

		// Token: 0x060024D7 RID: 9431 RVA: 0x001C29E0 File Offset: 0x001C0BE0
		public List<OBDRequest> GetQueueCopy()
		{
			return this.CommandQueue.ToList<OBDRequest>();
		}

		// Token: 0x060024D8 RID: 9432 RVA: 0x001C29F0 File Offset: 0x001C0BF0
		internal void RemoveFromQueue(Predicate<OBDRequest> match)
		{
			List<OBDRequest> queueCopy = this.GetQueueCopy();
			queueCopy.RemoveAll(match);
			this.ReplaceQueue(queueCopy);
		}

		// Token: 0x060024D9 RID: 9433 RVA: 0x001C2A14 File Offset: 0x001C0C14
		internal void InsertRequestInQueue(OBDRequest request)
		{
			List<OBDRequest> queueCopy = this.GetQueueCopy();
			queueCopy.Insert(0, request);
			this.ReplaceQueue(queueCopy);
		}

		// Token: 0x060024DA RID: 9434 RVA: 0x001C2A37 File Offset: 0x001C0C37
		public void DebugWriteSync(byte[] data)
		{
			PCLDebugStream.CurrentInstance.Write(data);
		}

		// Token: 0x060024DB RID: 9435 RVA: 0x001C2A44 File Offset: 0x001C0C44
		public void DebugWriteSync(string data)
		{
			this.DebugWriteSync(Encoding.UTF8.GetBytes(data));
		}

		// Token: 0x060024DC RID: 9436 RVA: 0x001C2A58 File Offset: 0x001C0C58
		public async Task DebugWrite(byte[] data)
		{
			try
			{
				await PCLDebugStream.CurrentInstance.WriteAsync(data);
				if (SharedSettings.Current.FlushLog)
				{
					await PCLDebugStream.CurrentInstance.Flush();
				}
			}
			catch (Exception)
			{
			}
		}

		// Token: 0x060024DD RID: 9437 RVA: 0x001C2A9C File Offset: 0x001C0C9C
		public async Task DebugWrite(byte data)
		{
			try
			{
				PCLDebugStream.CurrentInstance.WriteByte(data);
				if (SharedSettings.Current.FlushLog)
				{
					await PCLDebugStream.CurrentInstance.Flush();
				}
			}
			catch (Exception)
			{
			}
		}

		// Token: 0x060024DE RID: 9438 RVA: 0x001C2AE0 File Offset: 0x001C0CE0
		public async Task DebugWrite(string data)
		{
			try
			{
				byte[] bytes = Encoding.UTF8.GetBytes(data);
				await this.DebugWrite(bytes);
				if (SharedSettings.Current.FlushLog)
				{
					await PCLDebugStream.CurrentInstance.Flush();
				}
			}
			catch (Exception)
			{
			}
		}

		// Token: 0x1700117C RID: 4476
		// (get) Token: 0x060024DF RID: 9439 RVA: 0x001C2B2B File Offset: 0x001C0D2B
		// (set) Token: 0x060024E0 RID: 9440 RVA: 0x001C2B33 File Offset: 0x001C0D33
		public int SelectedECU
		{
			get
			{
				return this._ECU_ID;
			}
			set
			{
				this._ECU_ID = value;
				this.NotifyPropertyChanged("SelectedECU");
			}
		}

		// Token: 0x1700117D RID: 4477
		// (get) Token: 0x060024E1 RID: 9441 RVA: 0x001C2B47 File Offset: 0x001C0D47
		// (set) Token: 0x060024E2 RID: 9442 RVA: 0x001C2B4F File Offset: 0x001C0D4F
		public TimeSpan ReadDelay
		{
			get
			{
				return this._ReadDelay;
			}
			set
			{
				this._ReadDelay = value;
				this.NotifyPropertyChanged("ReadDelay");
			}
		}

		// Token: 0x060024E3 RID: 9443 RVA: 0x001C2B63 File Offset: 0x001C0D63
		protected virtual void NotifyPropertyChanged(string PropertyName)
		{
			MainThreadHelper.InvokeOnMainThread(delegate
			{
				PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
				if (propertyChanged == null)
				{
					return;
				}
				propertyChanged(this, new PropertyChangedEventArgs(PropertyName));
			});
		}

		// Token: 0x14000020 RID: 32
		// (add) Token: 0x060024E4 RID: 9444 RVA: 0x001C2B88 File Offset: 0x001C0D88
		// (remove) Token: 0x060024E5 RID: 9445 RVA: 0x001C2BC0 File Offset: 0x001C0DC0
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

		// Token: 0x1700117E RID: 4478
		// (get) Token: 0x060024E6 RID: 9446 RVA: 0x001C2BF5 File Offset: 0x001C0DF5
		// (set) Token: 0x060024E7 RID: 9447 RVA: 0x001C2C00 File Offset: 0x001C0E00
		public OBDDataReaderStatus CurrentStatus
		{
			get
			{
				return this._CurrentStatus;
			}
			protected set
			{
				if (this._CurrentStatus != value)
				{
					OBDDataReaderStatus currentStatus = this._CurrentStatus;
					this._CurrentStatus = value;
					this.NotifyPropertyChanged("CurrentStatus");
					this.OnStatusChanged(value, currentStatus);
				}
			}
		}

		// Token: 0x1700117F RID: 4479
		// (get) Token: 0x060024E8 RID: 9448 RVA: 0x001C2C37 File Offset: 0x001C0E37
		// (set) Token: 0x060024E9 RID: 9449 RVA: 0x001C2C3F File Offset: 0x001C0E3F
		public bool ECUSelectorVisible
		{
			get
			{
				return this._ECUSelectorVisible;
			}
			set
			{
				this._ECUSelectorVisible = value;
				this.NotifyPropertyChanged("ECUSelectorVisible");
			}
		}

		// Token: 0x060024EA RID: 9450 RVA: 0x001C2C53 File Offset: 0x001C0E53
		protected void _ECUHeaders_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
		{
			if (this._ECUHeaders.Count > 1)
			{
				this.ECUSelectorVisible = true;
				return;
			}
			this.ECUSelectorVisible = false;
		}

		// Token: 0x060024EB RID: 9451 RVA: 0x001C2C74 File Offset: 0x001C0E74
		protected virtual async void OnStatusChanged(OBDDataReaderStatus newStatus, OBDDataReaderStatus oldStatus)
		{
			if (newStatus == OBDDataReaderStatus.ConnectedToECU)
			{
				SharedSettings.Current.FlushLog = false;
			}
			else
			{
				SharedSettings.Current.FlushLog = true;
			}
			OBDDataReaderStatus status = this.CurrentStatus;
			if (status == OBDDataReaderStatus.Disconnected)
			{
				try
				{
					this.DebugWrite("\r\n===== DISCONNECTED AT: " + DateTimeNowHelper.NowSafe.ToString("dd.MM.yyyy HH:mm:ss"));
				}
				catch (Exception)
				{
				}
				this.InvokeOnMainThread(delegate
				{
					IScreenKeeper screenKeeper = DependencyService.Get<IScreenKeeper>(0);
					if (screenKeeper == null)
					{
						return;
					}
					screenKeeper.LetScreenOff();
				});
			}
			else
			{
				this.InvokeOnMainThread(delegate
				{
					try
					{
						IScreenKeeper screenKeeper2 = DependencyService.Get<IScreenKeeper>(0);
						if (screenKeeper2 != null)
						{
							screenKeeper2.KeepScreenOn();
						}
					}
					catch (Exception)
					{
					}
				});
			}
			DriveCycle.SaveAndReset();
			CurrentStatusChangedEvent Event = this.StatusChanged;
			if (Event != null)
			{
				Device.BeginInvokeOnMainThread(delegate
				{
					Event(status);
				});
			}
			if (status == OBDDataReaderStatus.Disconnected)
			{
				await PCLDebugStream.CurrentInstance.Flush();
			}
			if (status == OBDDataReaderStatus.ConnectedToECU)
			{
				this.lastTimeConnected = this.stopwatch.ElapsedTicks;
			}
			Droid_IBackgroundService instance = Droid_BackgroundService.Instance;
			if (instance != null)
			{
				instance.UpdateStatus(newStatus);
			}
			MainThreadHelper.InvokeOnMainThread(delegate
			{
				CarPlayManager instance2 = CarPlayManager.Instance;
				if (instance2 == null)
				{
					return;
				}
				instance2.OnOBDStatusChanged(newStatus);
			});
			if (SharedSettings.Current.ShowConnectionStatusOverlay)
			{
				ToastHelper.DisplayOBDStatusToast(status);
			}
		}

		// Token: 0x060024EC RID: 9452 RVA: 0x001C2CB4 File Offset: 0x001C0EB4
		private bool ReplyIsZero(string reply)
		{
			foreach (char c in reply)
			{
				if (c != '\0' && c != '>' && c != '\r')
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x060024ED RID: 9453 RVA: 0x001C2CEC File Offset: 0x001C0EEC
		protected bool CheckNoData(string reply)
		{
			if (this.CurrentMode == OBDDataReader.OBDModes.Universal)
			{
				if (reply.Contains("NO DATA") || reply.Contains("UNABLE") || reply.Contains("ERROR") || reply.Contains("STOPPED"))
				{
					this.NO_DATA_Counter++;
					return true;
				}
				if (!reply.Contains('.') || !reply.Contains('V'))
				{
					if (this.CurrentELMFormat == ELMFormat.CAN11bit && reply.Length >= 7 && reply[5] == '7' && (reply[6] == 'F' || reply[6] == 'f'))
					{
						return reply[9] != '7' || reply[10] != '8';
					}
					this.NO_DATA_Counter = 0;
					return false;
				}
			}
			return false;
		}

		// Token: 0x060024EE RID: 9454 RVA: 0x001C2DB8 File Offset: 0x001C0FB8
		public bool CheckIfRequestInQueue(OBDRequest request)
		{
			return this.CommandQueue.Any((OBDRequest x) => x.Equals(request));
		}

		// Token: 0x060024EF RID: 9455 RVA: 0x001C2DEC File Offset: 0x001C0FEC
		protected async Task<int> GetCurrentProtocolNumber()
		{
			await this.SendString("ATDPN");
			return OBDResponseAnalyzer.ParseProtocolNumberResponseString(await this.ReadData(2500, null, -1));
		}

		// Token: 0x060024F0 RID: 9456 RVA: 0x001C2E30 File Offset: 0x001C1030
		protected string PrepareResponseLine(string line)
		{
			StringBuilder stringBuilder = new StringBuilder(line.Length);
			for (int i = 0; i < line.Length; i++)
			{
				if (char.IsLetterOrDigit(line, i) || line[i] == '\r')
				{
					stringBuilder.Append(line[i]);
				}
			}
			return stringBuilder.ToString();
		}

		// Token: 0x060024F1 RID: 9457 RVA: 0x001C2E84 File Offset: 0x001C1084
		protected int CountStringInString(string source, string pattern)
		{
			int length = source.Length;
			int length2 = source.Replace(pattern, string.Empty).Length;
			return (length - length2) / pattern.Length;
		}

		// Token: 0x060024F2 RID: 9458 RVA: 0x001C2EB4 File Offset: 0x001C10B4
		public async Task WaitForCommandQueue()
		{
			this.waitingForQueueSemaphore = new SemaphoreSlim(0, 1);
			await this.waitingForQueueSemaphore.WaitAsync();
			this.waitingForQueueSemaphore = null;
		}

		// Token: 0x17001180 RID: 4480
		// (get) Token: 0x060024F3 RID: 9459 RVA: 0x001C2EF7 File Offset: 0x001C10F7
		public bool IsNissanConsult2Protocol
		{
			get
			{
				return this.CurrentProtocolNumber == 11 || this.CurrentProtocolNumber == 43 || this.CurrentProtocolNumber == 46;
			}
		}

		// Token: 0x060024F4 RID: 9460 RVA: 0x001C2F1C File Offset: 0x001C111C
		public string GetDefaultHeader()
		{
			if (!SharedSettings.Current.UseDefaultInit && (SharedSettings.Current.UseDefaultInit || !string.IsNullOrEmpty(SharedSettings.Current.DefaultFunctionalHeader)))
			{
				return SharedSettings.Current.DefaultFunctionalHeader;
			}
			if (SharedSettings.Current.ProtocolNumber < 32 || SharedSettings.Current.ProtocolNumber > 45)
			{
				return this.GetHeaderFromCurrentProtocol();
			}
			switch (SharedSettings.Current.ProtocolNumber)
			{
			case 32:
				return "8013F1";
			case 33:
				return "8013F0";
			case 34:
				return "8213F0";
			case 35:
				return "8013FC";
			case 36:
				return "8013F1";
			case 37:
				return "8113F1";
			case 38:
				return "8110FC";
			case 39:
				return "8013F1";
			case 40:
				return "8113F1";
			case 41:
				return "8213F1";
			case 42:
				return "686AF1";
			case 43:
				return "7E0";
			case 44:
				return "8110F1";
			case 45:
				return "7E0";
			default:
				return this.GetHeaderFromCurrentProtocol();
			}
		}

		// Token: 0x060024F5 RID: 9461 RVA: 0x001C3034 File Offset: 0x001C1234
		private string GetHeaderFromCurrentProtocol()
		{
			switch (this.CurrentProtocolNumber)
			{
			case 1:
				return "616AF1";
			case 2:
			case 3:
				return "686AF1";
			case 4:
			case 5:
				return "C233F1";
			case 6:
			case 8:
				return "7DF";
			case 7:
			case 9:
				return "DB33F1";
			case 11:
				return "8110FC";
			}
			return "";
		}

		// Token: 0x17001181 RID: 4481
		// (get) Token: 0x060024F6 RID: 9462 RVA: 0x001C30A7 File Offset: 0x001C12A7
		// (set) Token: 0x060024F7 RID: 9463 RVA: 0x001C30AF File Offset: 0x001C12AF
		public Stopwatch stopwatch
		{
			[CompilerGenerated]
			get
			{
				return this.<stopwatch>k__BackingField;
			}
			[CompilerGenerated]
			private set
			{
				this.<stopwatch>k__BackingField = value;
			}
		}

		// Token: 0x17001182 RID: 4482
		// (get) Token: 0x060024F8 RID: 9464 RVA: 0x001C30B8 File Offset: 0x001C12B8
		// (set) Token: 0x060024F9 RID: 9465 RVA: 0x001C30D3 File Offset: 0x001C12D3
		public CarData CurrentCarData
		{
			get
			{
				if (this._CurrentCarData == null)
				{
					this._CurrentCarData = new CarData();
				}
				return this._CurrentCarData;
			}
			set
			{
				this._CurrentCarData = value;
			}
		}

		// Token: 0x060024FA RID: 9466 RVA: 0x001C30DC File Offset: 0x001C12DC
		public void ReplaceQueue(OBDRequest pid)
		{
			this.ReplaceQueue(new OBDRequest[] { pid });
		}

		// Token: 0x060024FB RID: 9467 RVA: 0x001C30F0 File Offset: 0x001C12F0
		public void ReplaceQueue(IEnumerable<OBDRequest> new_queue)
		{
			if (this.STCommandsStupported && (this.CurrentELMFormat == ELMFormat.CAN11bit || this.CurrentELMFormat == ELMFormat.CAN29bit))
			{
				foreach (OBDRequest obdrequest in new_queue)
				{
					CAN11bitHelper.SetKeysForSTN(obdrequest);
				}
			}
			if (SharedSettings.Current.CANOptimizeRequests && SharedSettings.Current.CANOptimizeMode22SelfLearningMode && (this.CurrentELMFormat == ELMFormat.CAN11bit || this.CurrentELMFormat == ELMFormat.CAN29bit))
			{
				foreach (OBDRequest obdrequest2 in new_queue)
				{
					OBDRequestQueueOptimizer.SetupRequestForLearning(obdrequest2);
				}
				OBDRequestQueueOptimizer.SetQueueReoptimize(new_queue);
			}
			this._CommandQueue = new ConcurrentQueue<OBDRequest>(new_queue);
		}

		// Token: 0x060024FC RID: 9468 RVA: 0x001C31C4 File Offset: 0x001C13C4
		public async Task SendRequest(OBDRequest request)
		{
			if (request.Header != this.ELM327_LastSentHeader && (!string.IsNullOrEmpty(request.Header) || !string.IsNullOrEmpty(this.ELM327_LastSentHeader)))
			{
				if (this.ELM327_PendingAfterCommands != null && this.ELM327_PendingAfterCommands.Length != 0)
				{
					foreach (string text in this.ELM327_PendingAfterCommands)
					{
						await this.SendString(text);
						await this.ReadData(2500, null, -1);
					}
					string[] array = null;
					this.ELM327_PendingAfterCommands = new string[0];
				}
				string text2;
				if (string.IsNullOrEmpty(request.Header))
				{
					text2 = this.GetDefaultHeader();
					this.ELM327_LastSentHeader = "";
				}
				else
				{
					text2 = request.Header;
					this.ELM327_LastSentHeader = text2;
				}
				await this.SendString("ATSH" + text2);
				await this.ReadData(2500, null, -1);
				this.ELM327_LastSentBeforeCommands = new string[0];
			}
			if (request.BeforeCommands != null && request.BeforeCommands.Length != 0)
			{
				string[] array = request.BeforeCommands;
				for (int i = 0; i < array.Length; i++)
				{
					await this.SendString(array[i]);
					await this.ReadData(2500, null, -1);
				}
				array = null;
			}
			await this.SendString(request.Command);
			this.ELM327_PendingAfterCommands = request.AfterCommands;
		}

		// Token: 0x060024FD RID: 9469 RVA: 0x001C320F File Offset: 0x001C140F
		public void SetStatusForTest(OBDDataReaderStatus NewStatus)
		{
			this.CurrentStatus = NewStatus;
		}

		// Token: 0x060024FE RID: 9470 RVA: 0x001C3218 File Offset: 0x001C1418
		public async Task<string> GetViecarDeviceId()
		{
			this.btle_devices = new ObservableCollection<IDevice>();
			CrossBluetoothLE.Current.Adapter.DeviceDiscovered += delegate(object sender, DeviceEventArgs e)
			{
				this.btle_devices.Add(e.Device);
			};
			await CrossBluetoothLE.Current.Adapter.StartScanningForDevicesAsync(null, null, false, default(CancellationToken));
			IDevice viecardevice;
			for (;;)
			{
				viecardevice = this.btle_devices.FirstOrDefault((IDevice x) => x.Name.ToLower().Contains("viecar"));
				if (viecardevice != null)
				{
					break;
				}
				await Task.Delay(50);
				viecardevice = null;
			}
			if (CrossBluetoothLE.Current.Adapter.IsScanning)
			{
				await CrossBluetoothLE.Current.Adapter.StopScanningForDevicesAsync();
			}
			return viecardevice.Id.ToString();
		}

		// Token: 0x060024FF RID: 9471 RVA: 0x001C325C File Offset: 0x001C145C
		public async Task<bool> Connect(bool checkELM = true)
		{
			this.LastAction = OBDDataReader.RemoteDeviceLastAction.Read;
			string DeviceID = this.GetDeviceIdForConnection();
			this.BadELM = false;
			this.SendDelay = SharedSettings.Current.SendDelay;
			int Attempt = 0;
			while (Attempt < SharedSettings.Current.ELM327ConnectionAttempts || SharedSettings.Current.ELM327ConnectionAttempts <= 0)
			{
				bool flag;
				if (this.DisconnectRequested)
				{
					flag = false;
				}
				else
				{
					int num = 0;
					try
					{
						if (this.DisconnectRequested)
						{
							this.CurrentStatus = OBDDataReaderStatus.Disconnected;
							return false;
						}
						this.CurrentStatus = OBDDataReaderStatus.ConnectingToELM;
						if (Attempt > 0)
						{
							await Task.Delay(1000);
							if (this.DisconnectRequested)
							{
								return false;
							}
						}
						DateTime nowSafe = DateTimeNowHelper.NowSafe;
						byte[] bytes = Encoding.UTF8.GetBytes(string.Concat(new string[]
						{
							"\r\n===== LOG STARTED: ",
							nowSafe.ToString("dd.MM.yyyy HH:mm:ss", CultureInfo.InvariantCulture),
							"=====\r\n=====Version=",
							App.Version,
							" Build=",
							App.Build,
							" P=",
							SharedSettings.Current.AdsProductPurchased.ToString(),
							" NoDelayELM327Init=",
							SharedSettings.Current.NoDelayELM327Init.ToString(),
							"\r\n====Connection profile=",
							SharedSettings.Current.BrandAndProfile
						}));
						await this.DebugWrite(bytes);
						if (this.Connection != null)
						{
							try
							{
								this.Connection.Disconect();
							}
							catch (Exception)
							{
							}
						}
						switch (SharedSettings.Current.ConnectionType)
						{
						case ConnectionTypes.WiFi:
						{
							await this.DebugWrite("\r\n[Connection type: WIFI]\r\n");
							string text = null;
							IWiFiHelper wiFiHelper = DependencyService.Get<IWiFiHelper>(0);
							if (wiFiHelper != null)
							{
								text = wiFiHelper.GetWiFiName();
							}
							if (text == null)
							{
								text = "<null>";
							}
							await this.DebugWrite("\r\n[WiFi name: " + text + "]\r\n");
							if (Device.RuntimePlatform == "Android")
							{
								switch (SharedSettings.Current.AndroidWiFiMode)
								{
								case AndroidWiFiConnectionModes.ModernBind:
								{
									ITCPConnectionV2Droid itcpconnectionV2Droid = DependencyService.Get<ITCPConnectionV2Droid>(1);
									itcpconnectionV2Droid.Bind = true;
									this.Connection = itcpconnectionV2Droid;
									goto IL_04EE;
								}
								case AndroidWiFiConnectionModes.LegacyBind:
								{
									ITCPConnectionV1Droid itcpconnectionV1Droid = DependencyService.Get<ITCPConnectionV1Droid>(1);
									itcpconnectionV1Droid.Bind = true;
									this.Connection = itcpconnectionV1Droid;
									goto IL_04EE;
								}
								case AndroidWiFiConnectionModes.ModernDontBind:
								{
									ITCPConnectionV2Droid itcpconnectionV2Droid2 = DependencyService.Get<ITCPConnectionV2Droid>(1);
									itcpconnectionV2Droid2.Bind = false;
									this.Connection = itcpconnectionV2Droid2;
									goto IL_04EE;
								}
								case AndroidWiFiConnectionModes.LegacyDontBind:
								{
									ITCPConnectionV1Droid itcpconnectionV1Droid2 = DependencyService.Get<ITCPConnectionV1Droid>(1);
									itcpconnectionV1Droid2.Bind = false;
									this.Connection = itcpconnectionV1Droid2;
									goto IL_04EE;
								}
								}
								if (this.DroidWiFiV2Failed)
								{
									ITCPConnectionV1Droid itcpconnectionV1Droid3 = DependencyService.Get<ITCPConnectionV1Droid>(1);
									itcpconnectionV1Droid3.Bind = true;
									this.Connection = itcpconnectionV1Droid3;
								}
								else
								{
									ITCPConnectionV2Droid itcpconnectionV2Droid3 = DependencyService.Get<ITCPConnectionV2Droid>(1);
									itcpconnectionV2Droid3.Bind = true;
									this.Connection = itcpconnectionV2Droid3;
								}
							}
							else if (Device.RuntimePlatform == "iOS" || Device.RuntimePlatform == "macOS")
							{
								this.Connection = DependencyService.Get<ITCPConnection>(1);
							}
							IL_04EE:
							if (this.Connection == null)
							{
								if (Device.RuntimePlatform == "Android")
								{
									DependencyService.Get<ITCPConnectionV2Droid>(1);
								}
								else
								{
									this.Connection = DependencyService.Get<ITCPConnection>(1);
								}
								await this.DebugWrite("[Connection==null]");
							}
							break;
						}
						case ConnectionTypes.BluetoothLE:
							await this.DebugWrite("\r\n[Connection type: BluetoothLE; DeviceName=" + SharedSettings.Current.BTLEDeviceName + "]\r\n");
							this.Connection = new BTLEConnection(new Guid(SharedSettings.Current.BTLEServiceID), new Guid(SharedSettings.Current.BTLEInputID), new Guid(SharedSettings.Current.BTLEOutputID));
							break;
						case ConnectionTypes.Bluetooth:
							await this.DebugWrite("\r\n[Connection type: Bluetooth; DeviceName=" + SharedSettings.Current.BTDeviceName + "]\r\n");
							this.Connection = DependencyService.Get<IBluetoothConnection>(1);
							if (this.Connection == null)
							{
								await this.DebugWrite("Connection==null");
								this.Connection = DependencyService.Get<IBluetoothConnection>(1);
							}
							break;
						case ConnectionTypes.MFI_OBDLinkMXPlus:
							await this.DebugWrite("\r\nConnection type: Bluetooth\r\n");
							this.Connection = DependencyService.Get<IBluetoothConnection>(1);
							break;
						}
						await this.DebugWrite("\r\n[Connecting to " + DeviceID + "]\r\n");
						this.initFailCounterOnAT = 0;
						TaskAwaiter<bool> taskAwaiter = this.Connection.ConnectAsync(DeviceID, PCLDebugStream.CurrentInstance).GetAwaiter();
						if (!taskAwaiter.IsCompleted)
						{
							await taskAwaiter;
							TaskAwaiter<bool> taskAwaiter2;
							taskAwaiter = taskAwaiter2;
							taskAwaiter2 = default(TaskAwaiter<bool>);
						}
						if (!taskAwaiter.GetResult())
						{
							if (Device.RuntimePlatform == "Android")
							{
								this.DroidWiFiV2Failed = !this.DroidWiFiV2Failed;
							}
							await this.DebugWrite("\r\nConnection failed.\r\n");
							this.CurrentStatus = OBDDataReaderStatus.Disconnected;
							goto IL_0C5F;
						}
						await this.DebugWrite("\r\n[Connected to " + DeviceID + "]\r\n");
						if (this.DisconnectRequested)
						{
							await this.DebugWrite("\r\n[Disconnect requested at connection stage]\r\n");
							try
							{
								if (this.Connection != null)
								{
									this.Connection.Disconect();
									this.Connection = null;
								}
							}
							catch (Exception)
							{
							}
							this.CurrentStatus = OBDDataReaderStatus.Disconnected;
							return false;
						}
						if (!checkELM)
						{
							return true;
						}
						bool flag2 = await this.CheckIsELM327();
						if (this.DisconnectRequested)
						{
							await this.DebugWrite("\r\n[Disconnect requested at connection stage]\r\n");
							try
							{
								if (this.Connection != null)
								{
									this.Connection.Disconect();
									this.Connection = null;
								}
							}
							catch (Exception)
							{
							}
							this.CurrentStatus = OBDDataReaderStatus.Disconnected;
							return false;
						}
						if (flag2)
						{
							this.CurrentStatus = OBDDataReaderStatus.ConnectedToELM;
							return true;
						}
						this.CurrentStatus = OBDDataReaderStatus.Disconnected;
						goto IL_0C5F;
					}
					catch (Exception obj)
					{
						num = 1;
					}
					goto IL_0B6F;
					IL_0C5F:
					num = Attempt;
					Attempt = num + 1;
					continue;
					IL_0B6F:
					if (num == 1)
					{
						object obj;
						await this.DebugWrite("\r\nConnection error: " + ((Exception)obj).ToString() + "\r\n");
						await this.DebugWrite("\r\nConnection failed.\r\n");
						this.CurrentStatus = OBDDataReaderStatus.Disconnected;
						goto IL_0C5F;
					}
					goto IL_0C5F;
				}
				return flag;
			}
			await this.DebugWrite("Connection attempts failed\r\n");
			this.CurrentStatus = OBDDataReaderStatus.Disconnected;
			return false;
		}

		// Token: 0x06002500 RID: 9472 RVA: 0x001C32A8 File Offset: 0x001C14A8
		private string GetDeviceIdForConnection()
		{
			string text = "";
			switch (SharedSettings.Current.ConnectionType)
			{
			case ConnectionTypes.WiFi:
				this.LastDeviceID = SharedSettings.Current.WiFiServer + ":" + SharedSettings.Current.WiFiPort;
				this.LastUsedConnectionType = ConnectionTypes.WiFi;
				text = this.LastDeviceID;
				break;
			case ConnectionTypes.BluetoothLE:
				this.LastUsedConnectionType = ConnectionTypes.BluetoothLE;
				text = SharedSettings.Current.BTLEDeviceID;
				break;
			case ConnectionTypes.Bluetooth:
				this.LastUsedConnectionType = ConnectionTypes.Bluetooth;
				text = SharedSettings.Current.BTDeviceID;
				break;
			case ConnectionTypes.MFI_OBDLinkMXPlus:
				this.LastUsedConnectionType = ConnectionTypes.MFI_OBDLinkMXPlus;
				text = SharedSettings.Current.BTDeviceID;
				break;
			}
			return text;
		}

		// Token: 0x06002501 RID: 9473 RVA: 0x001C3354 File Offset: 0x001C1554
		private async Task<bool> CheckIsELM327()
		{
			bool flag;
			try
			{
				if (!SharedSettings.Current.NoDelayELM327Init)
				{
					await Task.Delay(500);
				}
				TaskAwaiter<string> taskAwaiter = this.SendATZ().GetAwaiter();
				if (!taskAwaiter.IsCompleted)
				{
					await taskAwaiter;
					TaskAwaiter<string> taskAwaiter2;
					taskAwaiter = taskAwaiter2;
					taskAwaiter2 = default(TaskAwaiter<string>);
				}
				if (taskAwaiter.GetResult().Contains('>'))
				{
					flag = true;
				}
				else
				{
					flag = false;
				}
			}
			catch (Exception)
			{
				flag = false;
			}
			return flag;
		}

		// Token: 0x06002502 RID: 9474 RVA: 0x001C3398 File Offset: 0x001C1598
		private async ValueTask<bool> CheckIsSTCommandsSupported()
		{
			bool flag;
			if (SharedSettings.Current.ConnectionType == ConnectionTypes.Bluetooth && SharedSettings.Current.BTDeviceName != null && (SharedSettings.Current.BTDeviceName.Contains("OBDLink", StringComparison.InvariantCultureIgnoreCase) || SharedSettings.Current.BTDeviceName.Contains("vlinker", StringComparison.InvariantCultureIgnoreCase)))
			{
				await this.DebugWrite("\r\n[STSupported=True(ByName)]");
				flag = true;
			}
			else if (SharedSettings.Current.ConnectionType == ConnectionTypes.BluetoothLE && SharedSettings.Current.BTLEDeviceName != null && (SharedSettings.Current.BTLEDeviceName.Contains("OBDLink", StringComparison.InvariantCultureIgnoreCase) || SharedSettings.Current.BTLEDeviceName.Contains("vlinker", StringComparison.InvariantCultureIgnoreCase)))
			{
				await this.DebugWrite("\r\n[STSupported=True(ByName)]");
				flag = true;
			}
			else
			{
				await this.SendString("STI");
				string text = await this.ReadData(2500, null, -1);
				if (text != null && (text.Contains("STN") || text.Contains('.')) && !text.Contains('?') && !text.Contains("ELM"))
				{
					await this.DebugWrite("\r\n[STSupported=True(ByResponse)]");
					flag = true;
				}
				else
				{
					await this.DebugWrite("\r\n[STSupported=False(ByResponse)]");
					flag = false;
				}
			}
			return flag;
		}

		// Token: 0x06002503 RID: 9475 RVA: 0x001C33DC File Offset: 0x001C15DC
		private async ValueTask<bool> CheckIsVTCommandsSupported()
		{
			bool flag;
			if (SharedSettings.Current.ConnectionType == ConnectionTypes.Bluetooth && SharedSettings.Current.BTDeviceName != null && SharedSettings.Current.BTDeviceName.Contains("vlinker", StringComparison.InvariantCultureIgnoreCase))
			{
				await this.DebugWrite("\r\n[VTSupported=True(ByName)]");
				flag = true;
			}
			else if (SharedSettings.Current.ConnectionType == ConnectionTypes.BluetoothLE && SharedSettings.Current.BTLEDeviceName != null && SharedSettings.Current.BTLEDeviceName.Contains("vlinker", StringComparison.InvariantCultureIgnoreCase))
			{
				await this.DebugWrite("\r\n[VTSupported=True(ByName)]");
				flag = true;
			}
			else
			{
				await this.SendString("VTI");
				string text = await this.ReadData(2500, null, -1);
				if (text != null && text.Contains('.') && !text.Contains('?') && !text.Contains("ELM"))
				{
					await this.DebugWrite("\r\n[VTSupported=True(ByResponse)]");
					flag = true;
				}
				else
				{
					await this.DebugWrite("\r\n[VTSupported=False(ByResponse)]");
					flag = false;
				}
			}
			return flag;
		}

		// Token: 0x06002504 RID: 9476 RVA: 0x001C3420 File Offset: 0x001C1620
		private string[] GetStartDiagnosticsCommands()
		{
			string text = SharedSettings.Current.BrandAndProfile.ToLowerInvariant();
			if (text.Contains("renault") || text.Contains("dacia") || text.Contains("nissan") || text.Contains("citroen") || text.Contains("peugeot"))
			{
				return new string[] { "1081", "10C0" };
			}
			if (text.Contains("hyundai") || text.Contains("kia"))
			{
				return new string[] { "1003", "1090" };
			}
			if (text.Contains("opel") || text.Contains("chevrolet") || text.Contains("gm") || text.Contains("general motors") || text.Contains("opel") || text.Contains("chevrolet"))
			{
				return new string[] { "1003", "1081", "1090" };
			}
			if (text.Contains("dodge") || text.Contains("chrysler") || text.Contains("fiat"))
			{
				return new string[0];
			}
			return new string[] { "1081", "1090" };
		}

		// Token: 0x06002505 RID: 9477 RVA: 0x001C3577 File Offset: 0x001C1777
		public void Start(string predstavsa_mraz)
		{
			Task.Run(delegate
			{
				this.StartLoopV3(predstavsa_mraz);
			});
		}

		// Token: 0x06002506 RID: 9478 RVA: 0x001C35A0 File Offset: 0x001C17A0
		public async Task SendString(string data)
		{
			this.ELMStatus.UpdateStatusFromCommand(data);
			byte[] req_bytes = new byte[data.Length + 1];
			Encoding.ASCII.GetBytes(data, 0, data.Length, req_bytes, 0);
			req_bytes[req_bytes.Length - 1] = 13;
			int num = 5;
			if (data.Length > 17)
			{
				num = 15;
			}
			TimeSpan timeout = new TimeSpan(0, 0, num);
			int num2 = 0;
			try
			{
				if (this.LastAction == OBDDataReader.RemoteDeviceLastAction.Write)
				{
					return;
				}
				await this.Connection.WriteBytesAsync(req_bytes).TimeoutAfter(timeout);
				if (this.Connection.NeedFlush)
				{
					await this.Connection.FlushAsync().TimeoutAfter(timeout);
				}
				this.LastAction = OBDDataReader.RemoteDeviceLastAction.Write;
				this.LastReadOrWriteTicks = this.stopwatch.ElapsedTicks;
				this.CommandsCounter += 1L;
				await this.DebugWrite(req_bytes);
			}
			catch (TimeoutException obj)
			{
				num2 = 1;
			}
			catch (Exception obj)
			{
				num2 = 2;
			}
			int num3 = num2;
			object obj;
			if (num3 == 1)
			{
				TimeoutException texc = (TimeoutException)obj;
				await this.DebugWrite("\r\nWriteDataTimeoutException(Write timeout exception) for data: " + data + "\r\n" + texc.ToString());
				this.LastAction = OBDDataReader.RemoteDeviceLastAction.Write;
				throw new WriteDataTimeoutException("Write timeout exception", texc);
			}
			if (num3 == 2)
			{
				Exception exc = (Exception)obj;
				await this.DebugWrite("\r\nWriteDataTimeoutException(Write timeout exception)\r\n" + exc.ToString());
				this.LastAction = OBDDataReader.RemoteDeviceLastAction.Write;
				throw new GeneralWriteException("Write general exception", exc);
			}
			obj = null;
		}

		// Token: 0x06002507 RID: 9479 RVA: 0x001C35EC File Offset: 0x001C17EC
		public unsafe async Task<string> ReadData(int timeout_ms = 2500, Action lengthTooBigHandler = null, int maxLines = -1)
		{
			OBDDataReader.<>c__DisplayClass143_0 CS$<>8__locals1 = new OBDDataReader.<>c__DisplayClass143_0();
			int maxLengthBytes = 2048;
			bool wasRunningWhenLaunched = this.Running;
			if (this.SendDelay > 0)
			{
				TimeSpan timeSpan = new TimeSpan(this.stopwatch.ElapsedTicks - this.LastReadOrWriteTicks);
				if ((int)timeSpan.TotalMilliseconds > 0)
				{
					int num = this.SendDelay / 2 - (int)timeSpan.TotalMilliseconds;
					if (num > 0)
					{
						await Task.Delay(num);
						if (wasRunningWhenLaunched && !this.Running)
						{
							return "";
						}
					}
				}
			}
			this.readSb.Clear();
			int dataCount = 0;
			CS$<>8__locals1.hasFinishCharacter = false;
			this.swReadData.Reset();
			this.swReadData.Start();
			int loopcounter = 0;
			int receivedLines = 0;
			string res;
			for (;;)
			{
				if (this.swReadData.ElapsedMilliseconds < (long)timeout_ms)
				{
					OBDDataReader.<>c__DisplayClass143_1 CS$<>8__locals2 = new OBDDataReader.<>c__DisplayClass143_1();
					CS$<>8__locals2.CS$<>8__locals1 = CS$<>8__locals1;
					loopcounter++;
					byte[] bytes;
					try
					{
						bytes = await this.Connection.ReadBytesAsync();
						if (bytes.Length == 0)
						{
							await Task.Delay(10);
							continue;
						}
					}
					catch (Exception ex)
					{
						this.LastAction = OBDDataReader.RemoteDeviceLastAction.Read;
						throw new GeneralReadingException("General read exception", ex);
					}
					if (bytes != null && bytes.Length != 0)
					{
						await this.DebugWrite(bytes);
					}
					bool flag = false;
					int num2 = 0;
					for (int i = 0; i < bytes.Length; i++)
					{
						if (bytes[i] != 0)
						{
							num2++;
						}
					}
					CS$<>8__locals2.partLineCounter = 0;
					string text = string.Create<byte[]>(num2, bytes, delegate(Span<char> span, byte[] src)
					{
						int num3 = 0;
						foreach (byte b in src)
						{
							if (b != 0)
							{
								*span[num3++] = (char)b;
							}
							if (b == 62)
							{
								CS$<>8__locals2.CS$<>8__locals1.hasFinishCharacter = true;
							}
							if (b == 13 || b == 10)
							{
								int partLineCounter = CS$<>8__locals2.partLineCounter;
								CS$<>8__locals2.partLineCounter = partLineCounter + 1;
							}
						}
					});
					if (!string.IsNullOrEmpty(text))
					{
						this.readSb.Append(text);
						flag = true;
					}
					if (CS$<>8__locals2.CS$<>8__locals1.hasFinishCharacter)
					{
						this.swReadData.Stop();
					}
					else
					{
						if (maxLines > 0 && !CS$<>8__locals2.CS$<>8__locals1.hasFinishCharacter)
						{
							receivedLines += CS$<>8__locals2.partLineCounter;
							if (receivedLines > maxLines)
							{
								goto IL_0447;
							}
						}
						if (lengthTooBigHandler == null || dataCount <= maxLengthBytes)
						{
							if (flag)
							{
								this.swReadData.Restart();
							}
							CS$<>8__locals2 = null;
							bytes = null;
							continue;
						}
						if (lengthTooBigHandler != null)
						{
							lengthTooBigHandler();
						}
						CS$<>8__locals2.CS$<>8__locals1.hasFinishCharacter = true;
					}
				}
				IL_0447:
				res = this.readSb.ToString();
				if (CS$<>8__locals1.hasFinishCharacter || timeout_ms != 2500 || !res.Contains("SEARCHING", StringComparison.OrdinalIgnoreCase))
				{
					break;
				}
				timeout_ms = 7500;
			}
			this.swReadData.Stop();
			if (!CS$<>8__locals1.hasFinishCharacter)
			{
				await this.DebugWrite(string.Concat(new string[]
				{
					"\n[NoFinishCharacter, timeElapsed=",
					this.swReadData.ElapsedMilliseconds.ToString(),
					" loopCounter=",
					loopcounter.ToString(),
					" res.length=",
					res.Length.ToString(),
					"]\n"
				}));
				this.ELMStatus.CheckForErrors(res);
				switch (SharedSettings.Current.ReadPartialErrorAction)
				{
				case ReadPartialErrorActions.ResetConnection:
				{
					this.Connection.Disconect();
					TaskAwaiter<bool> taskAwaiter = this.Connect(false).GetAwaiter();
					if (!taskAwaiter.IsCompleted)
					{
						await taskAwaiter;
						TaskAwaiter<bool> taskAwaiter2;
						taskAwaiter = taskAwaiter2;
						taskAwaiter2 = default(TaskAwaiter<bool>);
					}
					if (taskAwaiter.GetResult())
					{
						this.CurrentStatus = OBDDataReaderStatus.ConnectedToECU;
						this.LastAction = OBDDataReader.RemoteDeviceLastAction.Read;
						return res;
					}
					this.CurrentStatus = OBDDataReaderStatus.ConnectedToECU;
					throw new GeneralReadingException("Fast reconnect failed");
				}
				case ReadPartialErrorActions.Ignore:
					this.initFailCounterOnAT++;
					if (this.initFailCounterOnAT > SharedSettings.Current.NoDataLimit)
					{
						throw new GeneralReadingException("initFailCounterOnAT>" + SharedSettings.Current.NoDataLimit.ToString());
					}
					this.LastAction = OBDDataReader.RemoteDeviceLastAction.Read;
					if (res == null)
					{
						res = "";
					}
					res = res.Trim();
					return res;
				}
				throw new GeneralReadingException("Data packet not finished: [" + res + "]");
			}
			this.initFailCounterOnAT = 0;
			this.LastAction = OBDDataReader.RemoteDeviceLastAction.Read;
			this.ELMStatus.CheckForErrors(res);
			return res;
		}

		// Token: 0x06002508 RID: 9480 RVA: 0x001C3648 File Offset: 0x001C1848
		private bool CheckResponseForELM327Reset(OBDRequest req, string response)
		{
			if (response == null)
			{
				return false;
			}
			bool flag = false;
			if (response.StartsWith(req.Command))
			{
				flag = true;
			}
			bool flag2 = false;
			if (response.Count((char c) => c == ' ') >= 1)
			{
				flag2 = true;
			}
			return flag && flag2;
		}

		// Token: 0x06002509 RID: 9481 RVA: 0x001C36A0 File Offset: 0x001C18A0
		public OBDRequest GetPingRequest()
		{
			string text;
			if (SharedSettings.Current.UseDefaultInit)
			{
				if (this.IsNissanConsult2Protocol)
				{
					text = "221201";
				}
				else
				{
					text = SharedSettings.Current.Mode01Prefix + "00" + (SharedSettings.Current.DaihatsuKLine ? "01" : "");
				}
			}
			else
			{
				text = SharedSettings.Current.TesterPresentCommand;
				if (string.IsNullOrEmpty(text))
				{
					text = this.GetDefaultPidCommand();
				}
			}
			return new OBDRequest(text, "", "", "", false, new List<PID>(0))
			{
				DoNotDecode = true
			};
		}

		// Token: 0x0600250A RID: 9482 RVA: 0x001C3738 File Offset: 0x001C1938
		public async void StartLoopV3(string predstavsa_mraz)
		{
			OBDDataReader.<>c__DisplayClass147_0 CS$<>8__locals1 = new OBDDataReader.<>c__DisplayClass147_0();
			TimeSpan lastPing = TimeSpan.Zero;
			string ping_cmd;
			if (SharedSettings.Current.UseDefaultInit)
			{
				if (this.IsNissanConsult2Protocol)
				{
					ping_cmd = "221201";
				}
				else
				{
					ping_cmd = SharedSettings.Current.Mode01Prefix + "00";
				}
			}
			else
			{
				ping_cmd = SharedSettings.Current.TesterPresentCommand;
				if (string.IsNullOrEmpty(ping_cmd))
				{
					ping_cmd = this.GetDefaultPidCommand();
				}
			}
			OBDRequest pingrequest = new OBDRequest(ping_cmd, "", "", "", false, new List<PID>(0))
			{
				DoNotDecode = true
			};
			CS$<>8__locals1.request = null;
			if (this.Running)
			{
				await Task.Delay(1000);
				if (this.Running || this.DisconnectRequested)
				{
					await this.DebugWrite("\r\n[STARTLOOP_ALREADY_RUNNING. STOPPING START_LOOP_ID]");
					return;
				}
			}
			else
			{
				this.SetRunning(true, "From Start #2518");
			}
			int local_loopId = new Random().Next();
			this.loopId = local_loopId;
			while (this.Running && !this.DisconnectRequested && local_loopId == this.loopId)
			{
				if (this.DisconnectRequested)
				{
					await this.DebugWrite(string.Format("\r\n[START_LOOP:DisconnectRequested. STOPPING START_LOOP_ID {0}]", local_loopId));
					break;
				}
				if (local_loopId != this.loopId)
				{
					break;
				}
				try
				{
					if (!this.CommandQueue.TryDequeue(out CS$<>8__locals1.request))
					{
						try
						{
							if (this.waitingForQueueSemaphore != null)
							{
								SemaphoreSlim semaphoreSlim = this.waitingForQueueSemaphore;
								if (semaphoreSlim != null)
								{
									semaphoreSlim.Release();
								}
								await Task.Delay(150);
							}
						}
						catch (Exception)
						{
						}
						if (!SharedSettings.Current.AlwaysPingECU || this.CurrentStatus != OBDDataReaderStatus.ConnectedToECU)
						{
							await Task.Delay(50);
							continue;
						}
						if ((double)this.stopwatch.ElapsedMilliseconds - lastPing.TotalMilliseconds <= 200.0)
						{
							await Task.Delay(50);
							continue;
						}
						CS$<>8__locals1.request = pingrequest;
						lastPing = this.stopwatch.Elapsed;
					}
					if (!string.IsNullOrEmpty(CS$<>8__locals1.request.Command))
					{
						if (CS$<>8__locals1.request.Repeat)
						{
							this.CommandQueue.Enqueue(CS$<>8__locals1.request);
						}
						if (CS$<>8__locals1.request.SkipCyclesTarget > 0 && this.CommandQueue.Count > 1)
						{
							if (CS$<>8__locals1.request.SkippedCycles != 0)
							{
								CS$<>8__locals1.request.SkippedCycles++;
								if (CS$<>8__locals1.request.SkippedCycles > CS$<>8__locals1.request.SkipCyclesTarget)
								{
									CS$<>8__locals1.request.SkippedCycles = 0;
								}
								continue;
							}
							CS$<>8__locals1.request.SkippedCycles++;
						}
						this.RWCycleEnded = false;
						if ((CS$<>8__locals1.request.Header != this.ELM327_LastSentHeader && (!string.IsNullOrEmpty(CS$<>8__locals1.request.Header) || !string.IsNullOrEmpty(this.ELM327_LastSentHeader))) || (CS$<>8__locals1.request.Header == this.ELM327_LastSentHeader && this.ELM327_PendingAfterCommands != null && this.ELM327_PendingAfterCommands.Contains("ATCAF1") && CS$<>8__locals1.request.BeforeCommands != null && !CS$<>8__locals1.request.BeforeCommands.Contains("ATCAF0") && EnumerableExtensions.IndexOf<string>(this.ELM327_PendingAfterCommands, "ATCAF0") > EnumerableExtensions.IndexOf<string>(this.ELM327_PendingAfterCommands, "ATCAF1")) || (CS$<>8__locals1.request.Header == this.ELM327_LastSentHeader && this.ELM327_PendingAfterCommands != null && this.ELM327_PendingAfterCommands.Contains("ATAR") && !ArrayHelpers.ArrayEquals<string>(this.ELM327_PendingAfterCommands, CS$<>8__locals1.request.AfterCommands)))
						{
							if (this.ELM327_PendingAfterCommands != null && this.ELM327_PendingAfterCommands.Length != 0)
							{
								string[] array = this.ELM327_PendingAfterCommands.ToArray<string>();
								this.ELM327_PendingAfterCommands = new string[0];
								await this.SendBeforeOrAfterCommands(CS$<>8__locals1.request, array, false);
							}
							string text;
							if (string.IsNullOrEmpty(CS$<>8__locals1.request.Header))
							{
								text = this.GetDefaultHeader();
								this.ELM327_LastSentHeader = "";
							}
							else
							{
								text = CS$<>8__locals1.request.Header;
								this.ELM327_LastSentHeader = text;
							}
							if (CS$<>8__locals1.request.ELMFormat != ELMFormat.VwTp20 || !(CS$<>8__locals1.request.Header == "000"))
							{
								await this.SendString("ATSH" + text);
								await this.ReadData(2500, null, -1);
								this.ELM327_LastSentBeforeCommands = new string[0];
							}
						}
						if (!ArrayHelpers.ArrayEquals<string>(this.ELM327_LastSentBeforeCommands, CS$<>8__locals1.request.BeforeCommands))
						{
							if (CS$<>8__locals1.request.BeforeCommands != null && CS$<>8__locals1.request.BeforeCommands.Length != 0)
							{
								bool flag = OBDDataReader.CheckIfNewCommandsContainsAllExistingCommands(this.ELM327_LastSentBeforeCommands, CS$<>8__locals1.request.BeforeCommands);
								string[] array2 = CS$<>8__locals1.request.BeforeCommands;
								if (flag)
								{
									array2 = CS$<>8__locals1.request.BeforeCommands.Except(this.ELM327_LastSentBeforeCommands).ToArray<string>();
								}
								await this.SendBeforeOrAfterCommands(CS$<>8__locals1.request, array2, true);
							}
							this.ELM327_LastSentBeforeCommands = CS$<>8__locals1.request.BeforeCommands;
						}
						if (CS$<>8__locals1.request.AfterCommands != null && CS$<>8__locals1.request.AfterCommands.Length != 0)
						{
							this.ELM327_PendingAfterCommands = CS$<>8__locals1.request.AfterCommands;
						}
						ELMFormat requestElmFormat = CS$<>8__locals1.request.ELMFormat;
						if (requestElmFormat == ELMFormat.Unknown)
						{
							requestElmFormat = this.CurrentELMFormat;
						}
						int num = 14;
						if (this.ELMStatus.ATCEA != "")
						{
							num = 12;
						}
						string request_result;
						if (CS$<>8__locals1.request.Command.Length > num && (requestElmFormat == ELMFormat.CAN11bit || CS$<>8__locals1.request.ELMFormat == ELMFormat.CAN29bit))
						{
							if (CS$<>8__locals1.request is OBDMultiRequest)
							{
								request_result = await this.SendLongCANMultiRequest(CS$<>8__locals1.request as OBDMultiRequest);
							}
							else
							{
								request_result = await this.SendLongCanRequest(CS$<>8__locals1.request);
							}
						}
						else if (requestElmFormat == ELMFormat.CAN11bit && CS$<>8__locals1.request.ForceManualFlowControl && !string.IsNullOrEmpty(CS$<>8__locals1.request.Header))
						{
							request_result = await this.ReadDataWithManualFlowControl_CAN(CS$<>8__locals1.request);
						}
						else if (CS$<>8__locals1.request.ELMFormat == ELMFormat.VwTp20 && CS$<>8__locals1.request.Header == "000")
						{
							request_result = await this.vwTPManager.SendCommand(CS$<>8__locals1.request.Command);
						}
						else
						{
							string text2 = OBDRequestQueueOptimizer.SetExpectedCounterForRequestCommand(CS$<>8__locals1.request);
							if (SharedSettings.Current.ExpectedResponseCountOptimization && text2.Length == 4 && text2.StartsWith("10") && CS$<>8__locals1.request.ELMFormat != ELMFormat.VwTp20)
							{
								text2 += "1";
							}
							await this.SendString(text2);
							request_result = await this.ReadData(2500, null, CS$<>8__locals1.request.MaxLines);
							if (requestElmFormat == ELMFormat.CAN11bit && CS$<>8__locals1.request.CheckLength && request_result.Contains("BUFFER FULL"))
							{
								request_result = await this.ReadDataWithManualFlowControl_CAN(CS$<>8__locals1.request);
								CS$<>8__locals1.request.ForceManualFlowControl = true;
							}
						}
						if (this.CheckResponseForELM327Reset(CS$<>8__locals1.request, request_result) && this.ELMStatus.ELMSupportsATS0)
						{
							this.ELMStatus.ELMUnexpectedResetDetected = true;
							await this.SendString("ATE0");
							await this.ReadData(2500, null, -1);
							await this.SendString("ATS0");
							await this.ReadData(2500, null, -1);
							await this.SendString("ATH1");
							await this.ReadData(2500, null, -1);
						}
						if (string.IsNullOrEmpty(request_result) && SharedSettings.Current.ReadPartialErrorAction != ReadPartialErrorActions.ResetConnection)
						{
							CS$<>8__locals1.request.FailCounter++;
							this.NO_DATA_Counter++;
							if (CS$<>8__locals1.request is OBDMultiRequest)
							{
								this.DisassembleFailedMultiRequest(CS$<>8__locals1.request);
							}
						}
						else
						{
							CS$<>8__locals1.request.FailCounter = 0;
						}
						this.RWCycleEnded = true;
						try
						{
							CS$<>8__locals1.request.OnResponseReceived(request_result);
						}
						catch (Exception)
						{
						}
						if (CS$<>8__locals1.request.DoNotDecode)
						{
							if (CS$<>8__locals1.request == pingrequest && this.CheckNoData(request_result) && this.NO_DATA_Counter > SharedSettings.Current.NoDataLimit)
							{
								this.NO_DATA_Counter = 0;
								this.SetRunning(false, "LOOPV3_NO_DATA_LIMIT_REACHED");
								this.ReinitializeConnectionToECU("LOOPV3_NO_DATA_LIMIT_REACHED");
								break;
							}
							if (this.CurrentMode == OBDDataReader.OBDModes.ReadDTC || this.CurrentMode == OBDDataReader.OBDModes.ClearDTC)
							{
								EventHandler<int> dtcreadingQueueProgress = this.DTCReadingQueueProgress;
								if (dtcreadingQueueProgress != null)
								{
									dtcreadingQueueProgress(this, this.CommandQueue.Count);
								}
							}
						}
						else
						{
							OBDDataReader.OBDModes obdmodes = CS$<>8__locals1.request.OBDMode;
							if (this.CurrentMode != OBDDataReader.OBDModes.Universal)
							{
								obdmodes = this.CurrentMode;
							}
							switch (obdmodes)
							{
							case OBDDataReader.OBDModes.Mode06:
							{
								bool flag2 = await this.DecodeMode06Data(request_result, CS$<>8__locals1.request.Command);
								goto IL_1819;
							}
							case OBDDataReader.OBDModes.ReadDTC:
								this.DecodeDTC(request_result, CS$<>8__locals1.request);
								goto IL_1819;
							case OBDDataReader.OBDModes.ClearDTC:
								goto IL_1819;
							case OBDDataReader.OBDModes.Terminal:
								this.PushDataToTerminalPage(request_result);
								goto IL_1819;
							}
							if (this.CheckNoData(request_result))
							{
								this.DisassembleFailedMultiRequest(CS$<>8__locals1.request);
								if (this.NO_DATA_Counter > SharedSettings.Current.NoDataLimit)
								{
									this.NO_DATA_Counter = 0;
									this.SetRunning(false, string.Format("LOOPV3_NO_DATA_LIMIT_REACHED, LOOP_ID {0}", local_loopId));
									this.ReinitializeConnectionToECU("LOOPV3_NO_DATA_LIMIT_REACHED");
									break;
								}
							}
							if (CS$<>8__locals1.request.Command.StartsWith("06", StringComparison.Ordinal))
							{
								await this.DecodeMode06Data(request_result, CS$<>8__locals1.request.Command);
							}
							else if (CS$<>8__locals1.request.Command == "ATRV")
							{
								this.ParseATRV(CS$<>8__locals1.request, request_result);
								if (SharedSettings.Current.AlwaysPingECU && CS$<>8__locals1.request.Repeat && this.CommandQueue.Count == 1)
								{
									await this.SendString(ping_cmd);
									this.CheckNoData(await this.ReadData(2500, null, -1));
								}
							}
							else
							{
								bool flag2 = ((CS$<>8__locals1.request.ELMFormat != ELMFormat.VwTp20 || !(CS$<>8__locals1.request.Header == "000")) ? (await this.DecodeData(request_result, CS$<>8__locals1.request, null)) : (await this.DecodeVWTPData(request_result, CS$<>8__locals1.request)));
								if (flag2)
								{
									CS$<>8__locals1.request.FailCounter = 0;
								}
								if (CS$<>8__locals1.request is OBDMultiRequest)
								{
									if (flag2 && SharedSettings.Current.OptimizedRequestStuckCounter > 0)
									{
										SharedSettings.Current.OptimizedRequestStuckCounter = 0;
									}
									else if (!flag2 && SharedSettings.Current.DisableOptimizationIfItFails)
									{
										CS$<>8__locals1.request.FailCounter = 2;
										this.DisassembleFailedMultiRequest(CS$<>8__locals1.request);
									}
								}
							}
						}
						IL_1819:
						if (this.CommandQueue.Count == 0)
						{
							try
							{
								if (this.waitingForQueueSemaphore != null)
								{
									SemaphoreSlim semaphoreSlim2 = this.waitingForQueueSemaphore;
									if (semaphoreSlim2 != null)
									{
										semaphoreSlim2.Release();
									}
									await Task.Delay(150);
								}
							}
							catch (Exception)
							{
							}
						}
						request_result = null;
					}
				}
				catch (Exception ex)
				{
					this.SetRunning(false, "StartLoopV3_ReadDataTimeoutException");
					if (CS$<>8__locals1.request.ELMFormat == ELMFormat.VwTp20 && CS$<>8__locals1.request.Header != "000")
					{
						CS$<>8__locals1.request.OnResponseReceived("ELMFAILTP20");
					}
					if (ex is ReadDataTimeoutException && CS$<>8__locals1.request is OBDMultiRequest)
					{
						this.DisassembleFailedMultiRequest(CS$<>8__locals1.request);
					}
					CS$<>8__locals1.request.FailCounter = CS$<>8__locals1.request.FailCounter + 1;
					OBDRequest obdrequest;
					if (CS$<>8__locals1.request.FailCounter >= 2 && this.CommandQueue.TryPeek(out obdrequest) && CS$<>8__locals1.request == obdrequest)
					{
						List<OBDRequest> list = this.CommandQueue.ToList<OBDRequest>();
						Predicate<OBDRequest> predicate;
						if ((predicate = CS$<>8__locals1.<>9__0) == null)
						{
							OBDDataReader.<>c__DisplayClass147_0 CS$<>8__locals2 = CS$<>8__locals1;
							Predicate<OBDRequest> predicate2 = (OBDRequest x) => x == CS$<>8__locals1.request;
							CS$<>8__locals2.<>9__0 = predicate2;
							predicate = predicate2;
						}
						list.RemoveAll(predicate);
						this.ReplaceQueue(list);
					}
					else if (CS$<>8__locals1.request != pingrequest && CS$<>8__locals1.request.FailCounter < 2)
					{
						List<OBDRequest> list2 = this.CommandQueue.ToList<OBDRequest>();
						if (!list2.Contains(CS$<>8__locals1.request))
						{
							list2.Insert(0, CS$<>8__locals1.request);
							this.ReplaceQueue(list2);
						}
					}
					if (local_loopId == this.loopId)
					{
						this.lastTimeConnected = this.stopwatch.ElapsedTicks;
						this.OnDisconnectDetected(OBDDataReader.DisconnectReason.ELMStuck);
					}
					break;
				}
			}
		}

		// Token: 0x0600250B RID: 9483 RVA: 0x001C3770 File Offset: 0x001C1970
		private async ValueTask<bool> DecodeVWTPData(string request_result, OBDRequest request)
		{
			TimeSpan elapsed = this.stopwatch.Elapsed;
			bool flag;
			if (string.IsNullOrEmpty(request_result) || request_result.Contains("NO DATA"))
			{
				request.OnResponseDecoded(EmptyArrays.EmptyByteArray, false, "");
				flag = false;
			}
			else
			{
				try
				{
					byte[] array = BitHelpers.ConvertHexToBytesX(request_result);
					int num = request.ResponseMarker.Length / 2 + 2;
					byte[] dataWithoutMarker = new byte[array.Length - num];
					Array.Copy(array, num, dataWithoutMarker, 0, dataWithoutMarker.Length);
					bool flag2 = await this.CurrentCarData.Decode(request.Command, "000", request, dataWithoutMarker, elapsed);
					request.OnResponseDecoded(dataWithoutMarker, flag2, "");
					flag = flag2;
				}
				catch (Exception)
				{
					request.OnResponseDecoded(EmptyArrays.EmptyByteArray, false, "");
					flag = false;
				}
			}
			return flag;
		}

		// Token: 0x0600250C RID: 9484 RVA: 0x001C37C4 File Offset: 0x001C19C4
		private void DisassembleFailedMultiRequest(OBDRequest request)
		{
			if (request.FailCounter > 1)
			{
				OBDMultiRequest obdmultiRequest = request as OBDMultiRequest;
				if (obdmultiRequest != null && SharedSettings.Current.DisableOptimizationIfItFails)
				{
					List<OBDRequest> list = new List<OBDRequest>(this.CommandQueue);
					list.Remove(request);
					list.AddRange(OBDMultiRequest.Disassemble(obdmultiRequest));
					this.ReplaceQueue(list);
					SharedSettings sharedSettings = SharedSettings.Current;
					int optimizedRequestStuckCounter = sharedSettings.OptimizedRequestStuckCounter;
					sharedSettings.OptimizedRequestStuckCounter = optimizedRequestStuckCounter + 1;
					if (SharedSettings.Current.OptimizedRequestStuckCounter > 3)
					{
						SharedSettings.Current.CANOptimizeRequests = false;
						List<OBDRequest> list2 = new List<OBDRequest>(list.Count);
						foreach (OBDRequest obdrequest in list)
						{
							OBDMultiRequest obdmultiRequest2 = obdrequest as OBDMultiRequest;
							if (obdmultiRequest2 != null)
							{
								list2.AddRange(OBDMultiRequest.Disassemble(obdmultiRequest2));
							}
							else
							{
								list2.Add(obdrequest);
							}
						}
						App.OBDReader.ReplaceQueue(list2);
					}
				}
			}
		}

		// Token: 0x0600250D RID: 9485 RVA: 0x001C38C4 File Offset: 0x001C1AC4
		private async ValueTask SendBeforeOrAfterCommands(OBDRequest request, string[] commandsToSend, bool isBeforeCommands)
		{
			if (commandsToSend != null)
			{
				bool atOptimization = SharedSettings.Current.ATCommandStateOptimization;
				foreach (string text in commandsToSend)
				{
					if (text != null)
					{
						if (text == "REINIT")
						{
							if (SharedSettings.Current.UseDefaultInit)
							{
								await this.InitializeDefaultInitString(SharedSettings.Current.ProtocolNumber, OBDDataReader.InitModes.RestoreConnection, false, true);
							}
							else
							{
								await this.InitializeCustomInitString(SharedSettings.Current.CustomInitString, OBDDataReader.InitModes.RestoreConnection, true);
							}
						}
						else
						{
							string fcmd = text;
							if (fcmd.IndexOf(' ') >= 0)
							{
								fcmd = fcmd.Replace(" ", "");
							}
							if (SharedSettings.Current.ExpectedResponseCountOptimization && ((fcmd.StartsWith("10") && fcmd.Length == 4) || fcmd == "20"))
							{
								await this.SendString(fcmd + "1");
							}
							else if (this.STCommandsStupported && !this.VTCommandsStupported && "ATCEA".Equals(fcmd, StringComparison.OrdinalIgnoreCase))
							{
								await this.SendString("STCAF0");
							}
							else if (this.STCommandsStupported && !this.VTCommandsStupported && fcmd.StartsWith("ATCEA", StringComparison.OrdinalIgnoreCase))
							{
								await this.SendString("STCAF1," + fcmd.Substring(5));
							}
							else if (this.STCommandsStupported && fcmd.StartsWith("ATFCSH", StringComparison.OrdinalIgnoreCase))
							{
								string fcheader = "";
								string rcvheader = "";
								request.Keys.TryGetValue("ST_FC_REQ", out fcheader);
								request.Keys.TryGetValue("ST_FC_RES", out rcvheader);
								if (fcheader != null)
								{
								}
								if (this.ELMStatus.FlowControlHeader != fcmd.Substring(6))
								{
									await this.SendString(fcmd);
									await this.ReadData(2500, null, -1);
								}
								if (this.ELMStatus.HasSTFlowControlPair(fcheader, rcvheader) || this.VTCommandsStupported)
								{
									goto IL_11B0;
								}
								await this.SendString("STCFCPA" + fcheader + "," + rcvheader);
								fcheader = null;
								rcvheader = null;
							}
							else
							{
								if ((atOptimization && fcmd == "ATCFC1" && this.ELMStatus.CANFlowControl_CFC) || (atOptimization && fcmd == "ATCFC0" && !this.ELMStatus.CANFlowControl_CFC) || (atOptimization && fcmd == "ATCAF1" && this.ELMStatus.CANAutoFormat_CAF) || (atOptimization && fcmd == "ATCAF0" && !this.ELMStatus.CANAutoFormat_CAF) || (atOptimization && fcmd == "ATFCSM0" && this.ELMStatus.FlowControlMode == ELMState.FlowControlModes.Auto_0) || (atOptimization && fcmd == "ATFCSM1" && this.ELMStatus.FlowControlMode == ELMState.FlowControlModes.UserDefinedFull_1) || (atOptimization && fcmd == "ATFCSM2" && this.ELMStatus.FlowControlMode == ELMState.FlowControlModes.UserDefinedData_2))
								{
									goto IL_11B0;
								}
								if (atOptimization && fcmd == "ATFCSM0")
								{
									if (!isBeforeCommands && request.BeforeCommands != null && request.BeforeCommands.Contains("ATFCSM1"))
									{
										goto IL_11B0;
									}
									await this.SendString(fcmd);
								}
								else if (fcmd.StartsWith("ATTA", StringComparison.OrdinalIgnoreCase) && fcmd.Length == 6)
								{
									if (atOptimization && fcmd.Substring(4) == this.ELMStatus.TesterAddress)
									{
										goto IL_11B0;
									}
									if (SharedSettings.Current.ReplaceATTAWithATCER)
									{
										await this.SendString(fcmd.Replace("ATTA", "ATCER"));
									}
									else
									{
										await this.SendString(fcmd);
										if (this.STCommandsStupported && this.VTCommandsStupported)
										{
											await this.ReadData(2500, null, -1);
											await this.SendString(fcmd.Replace("ATTA", "ATCER"));
										}
									}
								}
								else if (atOptimization && fcmd.StartsWith("ATFCSH", StringComparison.OrdinalIgnoreCase))
								{
									string text2 = fcmd.Substring(6);
									if (this.ELMStatus.FlowControlHeader == text2)
									{
										goto IL_11B0;
									}
									await this.SendString(fcmd);
								}
								else if (atOptimization && fcmd.StartsWith("ATFCSD", StringComparison.OrdinalIgnoreCase) && fcmd.Length > 6)
								{
									if (fcmd.Substring(6) == this.ELMStatus.FlowControlData)
									{
										goto IL_11B0;
									}
									await this.SendString(fcmd);
								}
								else if (atOptimization && fcmd.StartsWith("ATCRA"))
								{
									string text3 = fcmd.Substring(5);
									if (this.ELMStatus.ATCRA == text3 || (SharedSettings.Current.ATCRAOptimization && (this.ELMStatus.ATCM == "000" || this.ELMStatus.ATCM == "00000000")))
									{
										goto IL_11B0;
									}
									if (SharedSettings.Current.ATCRAOptimization)
									{
										await this.SendString("ATCM000");
									}
									else
									{
										await this.SendString(fcmd);
									}
								}
								else if (atOptimization && fcmd == "ATAR")
								{
									if (SharedSettings.Current.ATCRAOptimization && (this.ELMStatus.ATCM == "000" || this.ELMStatus.ATCM == "00000000"))
									{
										goto IL_11B0;
									}
									await this.SendString(fcmd);
								}
								else if (atOptimization && fcmd != "ATCEA" && fcmd.StartsWith("ATCEA"))
								{
									string text4 = fcmd.Substring(5);
									if (this.ELMStatus.ATCEA == text4)
									{
										goto IL_11B0;
									}
									await this.SendString(fcmd);
								}
								else if (atOptimization && fcmd.StartsWith("ATCP"))
								{
									string text5 = fcmd.Substring(4);
									if (this.ELMStatus.CAN29bitPriority == text5)
									{
										goto IL_11B0;
									}
									await this.SendString(fcmd);
								}
								else if (atOptimization && fcmd.StartsWith("ATSP") && fcmd.Length == 5 && fcmd != "ATSP0" && !fcmd.StartsWith("ATSPA"))
								{
									int num;
									if (int.TryParse(fcmd.Substring(4), NumberStyles.HexNumber, CultureInfo.InvariantCulture, out num))
									{
										if (num == this.ELMStatus.Protocol)
										{
											goto IL_11B0;
										}
										await this.SendString(fcmd);
									}
								}
								else if (atOptimization && fcmd.StartsWith("STP") && fcmd.Length == 5)
								{
									int num2;
									if (int.TryParse(fcmd.Substring(3, 2), NumberStyles.HexNumber, CultureInfo.InvariantCulture, out num2))
									{
										if (num2 == this.ELMStatus.Protocol)
										{
											goto IL_11B0;
										}
										await this.SendString(fcmd);
									}
								}
								else
								{
									await this.SendString(fcmd);
								}
							}
							await this.ReadData(2500, null, -1);
							fcmd = null;
						}
					}
					IL_11B0:;
				}
				string[] array = null;
			}
		}

		// Token: 0x0600250E RID: 9486 RVA: 0x001C3920 File Offset: 0x001C1B20
		private static bool CheckIfNewCommandsContainsAllExistingCommands(string[] existingCommands, string[] newCommands)
		{
			if (existingCommands == null || newCommands == null)
			{
				return false;
			}
			if (existingCommands.Length > newCommands.Length)
			{
				return false;
			}
			if (existingCommands.Length == 0)
			{
				return false;
			}
			foreach (string text in existingCommands)
			{
				if (!newCommands.Contains(text))
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x0600250F RID: 9487 RVA: 0x001C3968 File Offset: 0x001C1B68
		private async Task<string> ReadDataWithManualFlowControl_CAN(OBDRequest request)
		{
			OBDDataReader.<>c__DisplayClass152_0 CS$<>8__locals1 = new OBDDataReader.<>c__DisplayClass152_0();
			CS$<>8__locals1.request = request;
			if (this.CurrentMode == OBDDataReader.OBDModes.ReadDTC || this.CurrentMode == OBDDataReader.OBDModes.ClearDTC)
			{
				await this.SendString("3E");
				await this.ReadData(2500, null, -1);
			}
			if (CS$<>8__locals1.request.ELMFormat == ELMFormat.Unknown)
			{
				CS$<>8__locals1.request.ELMFormat = this.CurrentELMFormat;
			}
			int max_frames = 8;
			string atfcsd = "30" + max_frames.ToString("X2") + "00";
			CS$<>8__locals1.has_extended_address = false;
			string text = CS$<>8__locals1.request.BeforeCommands.FirstOrDefault((string x) => x.StartsWith("ATCEA") && x != "ATCEA");
			if (text != null)
			{
				text.Replace("ATCEA", "");
				CS$<>8__locals1.has_extended_address = true;
			}
			await this.SendString("ATCAF0");
			await this.ReadData(2500, null, -1);
			await this.SendString("ATCFC0");
			await this.ReadData(2500, null, -1);
			await this.SendString("ATAL");
			await this.ReadData(2500, null, -1);
			string text2 = (CS$<>8__locals1.request.Command.Length / 2).ToString("X2") + CS$<>8__locals1.request.Command;
			await this.SendString(text2);
			string[] array = OBDDataReader.FilterHexAndNewLineOnly(await this.ReadData(2500, null, -1)).Split(OBDDataReader.line_splitter, StringSplitOptions.RemoveEmptyEntries);
			List<CANFrame> resultFrames = (from x in array
				where x != null && !x.Contains("ERROR") && !x.Contains("BUFFER")
				select new CANFrame(x, CS$<>8__locals1.request.ELMFormat, CS$<>8__locals1.has_extended_address)).ToList<CANFrame>();
			CANFrame canframe2;
			for (CANFrame canframe = resultFrames.FirstOrDefault((CANFrame x) => x.Type == CANFrame.CANFrameTypes.MultiFrameFirstFrame); canframe != null; canframe = canframe2)
			{
				int num = (CS$<>8__locals1.has_extended_address ? 5 : 6);
				int num2 = (CS$<>8__locals1.has_extended_address ? 6 : 7);
				int num3 = canframe.ExpectedLength - num;
				int num4 = num3 / num2;
				if (num3 % num2 != 0)
				{
					num4++;
				}
				int read_cycles = num4 / max_frames;
				if (read_cycles == 0)
				{
					read_cycles = 1;
				}
				else if (num4 % max_frames > 0)
				{
					read_cycles++;
				}
				for (int i = 0; i < read_cycles; i++)
				{
					await this.SendString(atfcsd);
					string text3 = await this.ReadData(2500, null, -1);
					if (text3.Contains("NO DATA"))
					{
						goto IL_08BC;
					}
					IEnumerable<string> enumerable = OBDDataReader.FilterHexAndNewLineOnly(text3).Split(OBDDataReader.line_splitter, StringSplitOptions.RemoveEmptyEntries);
					Func<string, CANFrame> func;
					if ((func = CS$<>8__locals1.<>9__4) == null)
					{
						OBDDataReader.<>c__DisplayClass152_0 CS$<>8__locals2 = CS$<>8__locals1;
						Func<string, CANFrame> func2 = (string x) => new CANFrame(x, CS$<>8__locals1.request.ELMFormat, CS$<>8__locals1.has_extended_address);
						CS$<>8__locals2.<>9__4 = func2;
						func = func2;
					}
					CANFrame[] array2 = enumerable.Select(func).ToArray<CANFrame>();
					if (array2.Length == 1 && array2[0].Data.Length >= 3 && array2[0].Data[0] == 3 && array2[0].Data[1] == 127 && (int)array2[0].Data[2] == max_frames && array2[0].Data[3] == 34)
					{
						SharedSettings.Current.ForceUseManualFlowControlWhileReadingData = false;
						CS$<>8__locals1.request.ForceManualFlowControl = false;
						goto IL_08BC;
					}
					resultFrames.AddRange(array2);
					IProgress<string> progress = CS$<>8__locals1.request.Progress;
					if (progress != null)
					{
						progress.Report(string.Format("{0}/{1}", i + 1, read_cycles));
					}
				}
				if (resultFrames.Count <= 1)
				{
					break;
				}
				canframe2 = resultFrames.Last<CANFrame>();
				if (canframe2.Type != CANFrame.CANFrameTypes.SingleFrame && canframe2.Type != CANFrame.CANFrameTypes.MultiFrameFirstFrame)
				{
					break;
				}
			}
			IL_08BC:
			await this.SendString("ATCAF1");
			await this.ReadData(2500, null, -1);
			await this.SendString("ATCFC1");
			await this.ReadData(2500, null, -1);
			StringBuilder stringBuilder = new StringBuilder(resultFrames.Count * 2 + 1);
			foreach (CANFrame canframe3 in resultFrames)
			{
				stringBuilder.Append(canframe3.ToString());
				stringBuilder.Append('\r');
			}
			return stringBuilder.ToString();
		}

		// Token: 0x06002510 RID: 9488 RVA: 0x001C39B4 File Offset: 0x001C1BB4
		private async Task<string> SendLongCanRequest(OBDRequest request)
		{
			string text;
			if (this.STCommandsStupported && SharedSettings.Current.CANRequestSegmentationSTNLevel && request.Command.Length < 1000)
			{
				text = await this.SendLongCanRequestSTN(request);
			}
			else
			{
				await this.SendString("ATCAF0");
				await this.ReadData(2500, null, -1);
				await this.SendString("ATCFC0");
				await this.ReadData(2500, null, -1);
				await this.SendString("ATAL");
				await this.ReadData(2500, null, -1);
				bool atr0 = true;
				if (atr0)
				{
					await this.SendString("ATR0");
					await this.ReadData(2500, null, -1);
				}
				bool flag = request.BeforeCommands.Any((string x) => x.Contains("ATCEA") && x != "ATCEA");
				string[] frames;
				if (request is OBDMultiRequest)
				{
					frames = (request as OBDMultiRequest).CanFrames;
				}
				else
				{
					frames = OBDMultiRequest.PrepareCanFrames(request.Command, flag);
				}
				int delay = 0;
				int frameIdx = 0;
				string data = "";
				bool sent_atcfc = false;
				string[] tp = new string[0];
				string text2 = "";
				if (request.Keys.TryGetValue("TesterPresent", out text2) && !string.IsNullOrEmpty(text2))
				{
					tp = text2.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
				}
				ELMFormat elmFormat = request.ELMFormat;
				if (elmFormat == ELMFormat.Unknown)
				{
					elmFormat = this.CurrentELMFormat;
				}
				int sendTPPeriod = SharedSettings.Current.SendTesterPresentWhileLongUploadTimeMs;
				Stopwatch sw = new Stopwatch();
				sw.Start();
				while (frameIdx < frames.Length)
				{
					if (frameIdx == frames.Length - 1)
					{
						sent_atcfc = true;
						await App.OBDReader.DebugWrite("\n[" + App.OBDReader.stopwatch.Elapsed.TotalSeconds.ToString() + "]\n");
						await this.SendString("ATCFC1");
						await App.OBDReader.DebugWrite("\n[" + App.OBDReader.stopwatch.Elapsed.TotalSeconds.ToString() + "]\n");
						await this.ReadData(2500, null, -1);
						await App.OBDReader.DebugWrite("\n[" + App.OBDReader.stopwatch.Elapsed.TotalSeconds.ToString() + "]\n");
						if (atr0)
						{
							await App.OBDReader.DebugWrite("\n[" + App.OBDReader.stopwatch.Elapsed.TotalSeconds.ToString() + "]\n");
							await this.SendString("ATR1");
							await App.OBDReader.DebugWrite("\n[" + App.OBDReader.stopwatch.Elapsed.TotalSeconds.ToString() + "]\n");
							await this.ReadData(2500, null, -1);
							await App.OBDReader.DebugWrite("\n[" + App.OBDReader.stopwatch.Elapsed.TotalSeconds.ToString() + "]\n");
						}
					}
					if (atr0 && delay > 0)
					{
						await Task.Delay(delay);
					}
					await App.OBDReader.DebugWrite("\n[" + App.OBDReader.stopwatch.Elapsed.TotalSeconds.ToString() + "]\n");
					await this.SendString(frames[frameIdx]);
					if (this.DisconnectRequested || !this.Running)
					{
						return data;
					}
					await App.OBDReader.DebugWrite("\n[" + App.OBDReader.stopwatch.Elapsed.TotalSeconds.ToString() + "]\n");
					data = await this.ReadData(2500, null, -1);
					await App.OBDReader.DebugWrite("\n[" + App.OBDReader.stopwatch.Elapsed.TotalSeconds.ToString() + "]\n");
					if (sendTPPeriod > 0 && tp != null && tp.Length != 0 && sw.ElapsedMilliseconds > (long)sendTPPeriod)
					{
						string[] array = tp;
						for (int i = 0; i < array.Length; i++)
						{
							await this.SendString(array[i]);
							await this.ReadData(2500, null, -1);
						}
						array = null;
						sw.Restart();
					}
					if (this.DisconnectRequested || !this.Running)
					{
						return data;
					}
					frameIdx++;
					IProgress<string> progress = request.Progress;
					if (progress != null)
					{
						progress.Report(string.Format("{0}/{1}", frameIdx, frames.Length));
					}
					try
					{
						int num;
						int num2;
						if (frameIdx < frames.Length && this.ParseFlowControlFrame(data, elmFormat, out num, out num2))
						{
							delay = num2;
						}
					}
					catch (Exception)
					{
					}
				}
				await App.OBDReader.DebugWrite("\n[" + App.OBDReader.stopwatch.Elapsed.TotalSeconds.ToString() + "]\n");
				await this.SendString("ATCAF1");
				await App.OBDReader.DebugWrite("\n[" + App.OBDReader.stopwatch.Elapsed.TotalSeconds.ToString() + "]\n");
				await this.ReadData(2500, null, -1);
				await App.OBDReader.DebugWrite("\n[" + App.OBDReader.stopwatch.Elapsed.TotalSeconds.ToString() + "]\n");
				if (!sent_atcfc)
				{
					sent_atcfc = true;
					await App.OBDReader.DebugWrite("\n[" + App.OBDReader.stopwatch.Elapsed.TotalSeconds.ToString() + "]\n");
					await this.SendString("ATCFC1");
					await App.OBDReader.DebugWrite("\n[" + App.OBDReader.stopwatch.Elapsed.TotalSeconds.ToString() + "]\n");
					await this.ReadData(2500, null, -1);
					await App.OBDReader.DebugWrite("\n[" + App.OBDReader.stopwatch.Elapsed.TotalSeconds.ToString() + "]\n");
				}
				text = data;
			}
			return text;
		}

		// Token: 0x06002511 RID: 9489 RVA: 0x001C3A00 File Offset: 0x001C1C00
		private async Task<string> SendLongCanRequestSTN(OBDRequest request)
		{
			if (!this.ELMStatus.STNTransmitSegmentation)
			{
				await this.SetSTNLevelCANTransmitSegmentation(true);
			}
			await this.SendString(request.Command);
			return await this.ReadData(2500, null, -1);
		}

		// Token: 0x06002512 RID: 9490 RVA: 0x001C3A4C File Offset: 0x001C1C4C
		private async Task<string> SendLongCANMultiRequest(OBDMultiRequest request)
		{
			string text;
			if (this.STCommandsStupported && SharedSettings.Current.CANRequestSegmentationSTNLevel && request.Command.Length < 500)
			{
				text = await this.SendLongCanRequestSTN(request);
			}
			else
			{
				int delay = 0;
				int frameIdx = 0;
				string data = "";
				ELMFormat elmFormat = request.ELMFormat;
				if (elmFormat == ELMFormat.Unknown)
				{
					elmFormat = this.CurrentELMFormat;
				}
				await this.SendString("ATCFC0");
				await this.ReadData(2500, null, -1);
				while (frameIdx < request.CanFrames.Length)
				{
					if (frameIdx == request.CanFrames.Length - 1)
					{
						await this.SendString("ATCFC1");
						await this.ReadData(2500, null, -1);
					}
					else if (delay > 0)
					{
						await Task.Delay(delay);
					}
					await this.SendString(request.CanFrames[frameIdx]);
					frameIdx++;
					data = await this.ReadData(2500, null, -1);
					if (this.DisconnectRequested || !this.Running)
					{
						return data;
					}
					try
					{
						int num;
						int num2;
						if (frameIdx < request.CanFrames.Length && this.ParseFlowControlFrame(data, elmFormat, out num, out num2))
						{
							delay = num2;
						}
					}
					catch (Exception)
					{
					}
				}
				text = data;
			}
			return text;
		}

		// Token: 0x06002513 RID: 9491 RVA: 0x001C3A98 File Offset: 0x001C1C98
		private bool ParseFlowControlFrame(string frame, ELMFormat elmFormat, out int max_burst, out int delay)
		{
			frame = OBDDataReader.FilterHexAndNewLineOnly(frame).Replace("\r", "").Replace("\n", "");
			if (string.IsNullOrEmpty(frame))
			{
				max_burst = 0;
				delay = 0;
				return false;
			}
			if (elmFormat == ELMFormat.CAN11bit)
			{
				if (frame.Length < 9)
				{
					max_burst = 0;
					delay = 0;
					return false;
				}
				byte[] array = BitHelpers.ConvertHexToBytesX(frame.Substring(3).Substring(0, 6));
				max_burst = (int)array[1];
				if (max_burst == 0)
				{
					max_burst = 4095;
				}
				delay = (int)array[2];
				if (delay >= 241)
				{
					delay *= 100;
				}
				return array[0] == 48;
			}
			else
			{
				if (elmFormat != ELMFormat.CAN29bit)
				{
					max_burst = 0;
					delay = 0;
					return false;
				}
				if (frame.Length < 14)
				{
					max_burst = 0;
					delay = 0;
					return false;
				}
				byte[] array2 = BitHelpers.ConvertHexToBytesX(frame.Substring(8));
				max_burst = (int)array2[1];
				if (max_burst == 0)
				{
					max_burst = 4095;
				}
				delay = (int)array2[2];
				return array2[0] == 48;
			}
		}

		// Token: 0x06002514 RID: 9492 RVA: 0x001C3B88 File Offset: 0x001C1D88
		public async Task<bool> ReinitializeConnectionToECU(string predstavsa_mraz)
		{
			await this.DebugWrite("\nReinitializeConnectionToECU(" + predstavsa_mraz + ")\n");
			int num = 0;
			try
			{
				this.CurrentStatus = OBDDataReaderStatus.ConnectingToECU;
				DriveCycle.SaveAndReset();
				if (SharedSettings.Current.UseDefaultInit)
				{
					await this.InitializeDefaultInitString(SharedSettings.Current.ProtocolNumber, OBDDataReader.InitModes.RestoreConnection, false, true);
				}
				else
				{
					await this.InitializeCustomInitString(SharedSettings.Current.CustomInitString, OBDDataReader.InitModes.RestoreConnection, true);
				}
				this.CurrentStatus = OBDDataReaderStatus.ConnectedToECU;
				this.ELM327_LastSentHeader = "";
				this.ELM327_LastSentBeforeCommands = new string[0];
				this.ELM327_PendingAfterCommands = new string[0];
				this.Start("From ReinitializeConnectionToECU_Success");
				return true;
			}
			catch (Exception obj)
			{
				num = 1;
			}
			bool flag;
			if (num == 1)
			{
				object obj;
				Exception ex = (Exception)obj;
				this.CurrentStatus = OBDDataReaderStatus.ConnectingToECU;
				this.SetRunning(false, "From ReinitializeConnectionToECU #2239");
				this.ELM327_LastSentHeader = "";
				this.ELM327_LastSentBeforeCommands = new string[0];
				this.ELM327_PendingAfterCommands = new string[0];
				await this.OnDisconnectDetected(OBDDataReader.DisconnectReason.ELMStuck);
				await Task.Delay(500);
				if (!this.Running)
				{
					this.Start("From ReinitializeConnectionToECU_Exception");
				}
				flag = false;
			}
			return flag;
		}

		// Token: 0x06002515 RID: 9493 RVA: 0x001C3BD3 File Offset: 0x001C1DD3
		private void PushDataToTerminalPage(string reply)
		{
			Device.BeginInvokeOnMainThread(delegate
			{
				if (TerminalPage.Instance != null)
				{
					TerminalPage.Instance.OnResponseReceived(reply);
				}
			});
		}

		// Token: 0x06002516 RID: 9494 RVA: 0x001C3BF4 File Offset: 0x001C1DF4
		private string GetDefaultPidCommand()
		{
			string text;
			if (CustomPIDViewModel.CurrentProfile.PidCollection.Count > 0)
			{
				text = CustomPIDViewModel.CurrentProfile.PidCollection.FirstOrDefault<CustomPID>().Command;
			}
			else if (CustomPIDViewModel.CurrentCustom.PidCollection.Where((CustomPID x) => x.IsAvailable).Count<CustomPID>() > 0)
			{
				text = CustomPIDViewModel.CurrentCustom.PidCollection.Where((CustomPID x) => x.IsAvailable).FirstOrDefault<CustomPID>().Command;
			}
			else
			{
				text = SharedSettings.Current.Mode01Prefix + "00";
			}
			return text;
		}

		// Token: 0x06002517 RID: 9495 RVA: 0x001C3CB4 File Offset: 0x001C1EB4
		internal OBDRequest GetDefaultPidRequest()
		{
			if (CustomPIDViewModel.CurrentProfile.PidCollection.Count > 0)
			{
				CustomPID customPID = CustomPIDViewModel.CurrentProfile.PidCollection.FirstOrDefault<CustomPID>();
				return new OBDRequest(customPID.Command, customPID.Header, customPID.BeforeCommand, customPID.AfterCommand, false);
			}
			if (CustomPIDViewModel.CurrentCustom.PidCollection.Count > 0)
			{
				CustomPID customPID2 = CustomPIDViewModel.CurrentCustom.PidCollection.FirstOrDefault<CustomPID>();
				return new OBDRequest(customPID2.Command, customPID2.Header, customPID2.BeforeCommand, customPID2.AfterCommand, false);
			}
			return new OBDRequest(SharedSettings.Current.Mode01Prefix + "00", false);
		}

		// Token: 0x06002518 RID: 9496 RVA: 0x001C3D60 File Offset: 0x001C1F60
		private OBDRequest PreprocessRequest(OBDRequest Request)
		{
			if (Request.Command.Length == 4 && Request.Command.StartsWith("02", StringComparison.Ordinal))
			{
				return new OBDRequest(Request.Command + this.CurrentCarData.FreezeFrameNumber.ToString("X2", CultureInfo.InvariantCulture), Request.Repeat);
			}
			return Request;
		}

		// Token: 0x06002519 RID: 9497 RVA: 0x001C3DC4 File Offset: 0x001C1FC4
		public async Task ClearRequestQueue()
		{
			OBDRequest obdrequest;
			while (this.CommandQueue.TryDequeue(out obdrequest))
			{
			}
			await Task.Delay(50);
			while (this.CommandQueue.TryDequeue(out obdrequest))
			{
			}
		}

		// Token: 0x0600251A RID: 9498 RVA: 0x001C3E08 File Offset: 0x001C2008
		public void AddRequestToQueue(OBDRequest request)
		{
			if (request != null && !string.IsNullOrEmpty(request.Command) && !this.CheckIfRequestInQueue(request))
			{
				if (SharedSettings.Current.CANOptimizeRequests && SharedSettings.Current.CANOptimizeMode22SelfLearningMode && (this.CurrentELMFormat == ELMFormat.CAN11bit || this.CurrentELMFormat == ELMFormat.CAN29bit))
				{
					OBDRequestQueueOptimizer.SetupRequestForLearning(request);
				}
				if (this.STCommandsStupported && (this.CurrentELMFormat == ELMFormat.CAN11bit || this.CurrentELMFormat == ELMFormat.CAN29bit))
				{
					CAN11bitHelper.SetKeysForSTN(request);
				}
				this.CommandQueue.Enqueue(request);
			}
		}

		// Token: 0x0600251B RID: 9499 RVA: 0x001C3E8C File Offset: 0x001C208C
		public void AddRequestToQueue(string request)
		{
			if (request == null)
			{
				return;
			}
			OBDRequest obdrequest = new OBDRequest(request, false);
			this.AddRequestToQueue(obdrequest);
		}

		// Token: 0x0600251C RID: 9500 RVA: 0x001C3EAC File Offset: 0x001C20AC
		private void ResetVars()
		{
			this.WorkingWithQueue = false;
			this.CurrentMode = OBDDataReader.OBDModes.Universal;
			this.SelectedECU = 0;
			this.InvokeOnMainThread(delegate
			{
				this.ECUHeaders.Clear();
			});
			this.CurrentELMFormat = ELMFormat.Unknown;
			this.ECUSelectorVisible = false;
			this.NO_DATA_Counter = 0;
			this.LastReadTimeStampTicks = 0L;
			this.CommandsCounter = 0L;
			this.ELM327_LastSentHeader = "";
			this.ELM327_LastSentBeforeCommands = new string[0];
			this.ELM327_PendingAfterCommands = new string[0];
			this.initFailCounterOnAT = 0;
			this.STCommandsStupported = false;
			this.VTCommandsStupported = false;
		}

		// Token: 0x0600251D RID: 9501 RVA: 0x001C3F40 File Offset: 0x001C2140
		public async Task<bool> Initialize(int Attempts, OBDDataReader.InitModes initMode, IProgress<string> progress)
		{
			await this.DebugWrite("\nInitialize(initMode=" + initMode.ToString() + ")\n");
			int DefaultProtocol = SharedSettings.Current.ProtocolNumber;
			bool UseCustom = !SharedSettings.Current.UseDefaultInit;
			string CustomInitString = SharedSettings.Current.CustomInitString;
			int num = 0;
			try
			{
				bool initSuccess = false;
				if (this.DisconnectRequested)
				{
					this.CurrentStatus = OBDDataReaderStatus.Disconnected;
					return false;
				}
				this.CurrentStatus = OBDDataReaderStatus.ConnectingToECU;
				TaskAwaiter<bool> taskAwaiter2;
				for (int i = 0; i < Attempts; i++)
				{
					if (this.DisconnectRequested)
					{
						this.CurrentStatus = OBDDataReaderStatus.Disconnected;
						return false;
					}
					string progressString = "";
					try
					{
						if (UseCustom)
						{
							progressString = string.Concat(new string[]
							{
								Translate.GetString("ios_ECUinitProgressStage1_Advanced"),
								"\n",
								Translate.GetString("Settings_Control_tbChooseProfile.Text"),
								" ",
								SharedSettings.Current.BrandAndProfile
							});
							progressString = string.Format(progressString, (i + 1).ToString(), Attempts.ToString());
						}
						else
						{
							progressString = string.Concat(new string[]
							{
								Translate.GetString("ios_ECUinitProgressStage1"),
								"\n",
								Translate.GetString("Settings_Control_tbChooseProfile.Text"),
								" ",
								SharedSettings.Current.BrandAndProfile
							});
							progressString = string.Format(progressString, StaticLists.Protocols[DefaultProtocol], (i + 1).ToString(), Attempts.ToString());
						}
						if (progress != null)
						{
							progress.Report(progressString);
						}
					}
					catch (Exception)
					{
					}
					if (initMode == OBDDataReader.InitModes.Default)
					{
						if (Device.RuntimePlatform == "Android")
						{
							if (!DependencyService.Get<ISignatureChecker>(0).GetSignaturesStrings().Any((string x) => (PlatformHelper.AppMarket == Markets.RUS && x.Equals("Lrc7gArwidVPdp9SPHGRH0v9GbM=")) || x.Equals("Y5kpo3aphxDnrKsp2if0BMjSUvo=") || x.Equals("VWq6xreY9eTH4u5UC5tJ0t2GYuI=") || x.Equals("Y5kpo3aphxDnrKsp2if0BMjSUvo=") || x.Equals("VWq6xreY9eTH4u5UC5tJ0t2GYuI=") || x.Equals("W8RPrUKpDE0NIvMkG9bDYSf4D1E=") || x.Equals("v9gdPRvdsuLO6CXYxBIIJ5wEhjQ=")))
							{
								await this.DebugWrite("\n\rATSF\n\r>");
								await Task.Delay(5000);
								this.CurrentStatus = OBDDataReaderStatus.ConnectedToELM;
								return false;
							}
						}
						await MainThread.InvokeOnMainThreadAsync(new Action(this.ResetVars));
						this.CurrentCarData.ResetAvailablePids();
					}
					else
					{
						this.ELM327_LastSentHeader = "";
						this.ELM327_LastSentBeforeCommands = new string[0];
						this.ELM327_PendingAfterCommands = new string[0];
					}
					await Task.Delay(300);
					this.BadELM = false;
					if (this.DisconnectRequested)
					{
						this.CurrentStatus = OBDDataReaderStatus.Disconnected;
						return false;
					}
					if (this.CurrentStatus == OBDDataReaderStatus.Disconnected)
					{
						break;
					}
					int num2 = 0;
					try
					{
						if (UseCustom)
						{
							initSuccess = await this.InitializeCustomInitString(CustomInitString, initMode, true);
						}
						else if (i == Attempts - 1 && !SharedSettings.Current.ForceOnlyOneProtocol)
						{
							initSuccess = await this.InitializeDefaultInitString(0, initMode, false, true);
							if (initSuccess)
							{
								SharedSettings.Current.ProtocolNumber = 0;
							}
						}
						else
						{
							initSuccess = await this.InitializeDefaultInitString(DefaultProtocol, initMode, false, true);
						}
						if (initSuccess)
						{
							if (this.DisconnectRequested)
							{
								this.CurrentStatus = OBDDataReaderStatus.Disconnected;
								return false;
							}
							string old_progress = progressString;
							progressString = Translate.GetString("ios_RequestingPIDs");
							if (progress != null)
							{
								progress.Report(progressString);
							}
							if (initMode == OBDDataReader.InitModes.Default)
							{
								TaskAwaiter<bool> taskAwaiter = this.CheckSupportedPIDsV2().GetAwaiter();
								if (!taskAwaiter.IsCompleted)
								{
									await taskAwaiter;
									taskAwaiter = taskAwaiter2;
									taskAwaiter2 = default(TaskAwaiter<bool>);
								}
								if (!taskAwaiter.GetResult())
								{
									progressString = old_progress;
									if (progress != null)
									{
										progress.Report(progressString);
									}
									throw new Exception("Error requesting supported parameters");
								}
								LiveDataPIDModel.UpdatePIDCollection(this);
							}
							if (this.DisconnectRequested)
							{
								this.CurrentStatus = OBDDataReaderStatus.Disconnected;
								return false;
							}
							this.Start("From Initialize_initSuccess");
							this.CurrentStatus = OBDDataReaderStatus.ConnectedToECU;
							break;
						}
					}
					catch (Exception obj)
					{
						num2 = 1;
					}
					object obj;
					if (num2 == 1)
					{
						Exception ex = (Exception)obj;
						if (this.DisconnectRequested)
						{
							this.CurrentStatus = OBDDataReaderStatus.Disconnected;
							return false;
						}
						TaskAwaiter<bool> taskAwaiter = this.Connect(true).GetAwaiter();
						if (!taskAwaiter.IsCompleted)
						{
							await taskAwaiter;
							taskAwaiter = taskAwaiter2;
							taskAwaiter2 = default(TaskAwaiter<bool>);
						}
						if (!taskAwaiter.GetResult())
						{
							initSuccess = false;
						}
					}
					obj = null;
					progressString = null;
				}
				if (!initSuccess && !UseCustom && DefaultProtocol == 0 && initMode == OBDDataReader.InitModes.Default)
				{
					if (this.DisconnectRequested)
					{
						this.CurrentStatus = OBDDataReaderStatus.Disconnected;
						return false;
					}
					int num3 = await this.SearchForProtocol(progress);
					if (num3 > 0)
					{
						initSuccess = true;
						SharedSettings.Current.ProtocolNumber = num3;
						if (progress != null)
						{
							progress.Report(Translate.GetString("ios_RequestingPIDs"));
						}
						TaskAwaiter<bool> taskAwaiter = this.CheckSupportedPIDsV2().GetAwaiter();
						if (!taskAwaiter.IsCompleted)
						{
							await taskAwaiter;
							taskAwaiter = taskAwaiter2;
							taskAwaiter2 = default(TaskAwaiter<bool>);
						}
						if (!taskAwaiter.GetResult())
						{
							throw new Exception("Error requesting supported parameters");
						}
						LiveDataPIDModel.UpdatePIDCollection(this);
						this.Start("From Initialize_initSuccess_after_SearchingForProtocol");
					}
				}
				if (!initSuccess)
				{
					if (this.DisconnectRequested)
					{
						this.CurrentStatus = OBDDataReaderStatus.Disconnected;
						return false;
					}
					this.CurrentStatus = OBDDataReaderStatus.ConnectedToELM;
				}
				OBDRequestQueueOptimizer.RefreshOBD2Dictionary();
				if (this.DisconnectRequested)
				{
					this.CurrentStatus = OBDDataReaderStatus.Disconnected;
					return false;
				}
				return initSuccess;
			}
			catch (Exception obj2)
			{
				num = 1;
			}
			bool flag;
			if (num == 1)
			{
				object obj2;
				Exception ex2 = (Exception)obj2;
				await this.DebugWrite(Encoding.UTF8.GetBytes(ex2.ToString()));
				await this.Disconnect("Initialize result:false");
				this.InvokeOnMainThread(delegate
				{
					this.ECUHeaders.Clear();
				});
				flag = false;
			}
			else
			{
				CustomInitString = null;
			}
			return flag;
		}

		// Token: 0x0600251E RID: 9502 RVA: 0x001C3F9B File Offset: 0x001C219B
		private void InvokeOnMainThread(Action a)
		{
			if (MainThread.IsMainThread)
			{
				a();
				return;
			}
			MainThread.InvokeOnMainThreadAsync(a).Wait();
		}

		// Token: 0x0600251F RID: 9503 RVA: 0x001C3FB8 File Offset: 0x001C21B8
		private async Task<bool> InitializeCustomInitString(string InitString, OBDDataReader.InitModes initMode = OBDDataReader.InitModes.Default, bool Decode = true)
		{
			await this.DebugWrite("\r\nInitializeCustomInitString(initMode=" + initMode.ToString() + ")\r\n");
			bool initSuccess = false;
			try
			{
				if (InitString.IndexOf("ATE0", StringComparison.OrdinalIgnoreCase) < 0)
				{
					InitString += "\nATE0";
				}
				if (InitString.IndexOf("ATH1", StringComparison.OrdinalIgnoreCase) < 0)
				{
					InitString += "\nATH1";
				}
				if (InitString.IndexOf("ATH1", StringComparison.OrdinalIgnoreCase) < 0)
				{
					InitString += "\nATS0";
				}
				string[] StringArray = InitString.Split(new string[] { "\n", "\r", "\\n", ";" }, StringSplitOptions.RemoveEmptyEntries);
				for (int j = 0; j < StringArray.Length; j++)
				{
					StringArray[j] = StringArray[j].Trim().Replace("\r", "").ToUpperInvariant();
				}
				if (initMode == OBDDataReader.InitModes.RestoreConnection)
				{
					await this.SendATZ();
				}
				await this.SendString("ATE0");
				await Task.Delay(300);
				await this.ReadData(2500, null, -1);
				await this.SendString("ATE0");
				await Task.Delay(300);
				await this.ReadData(2500, null, -1);
				this.STCommandsStupported = await this.CheckIsSTCommandsSupported();
				this.VTCommandsStupported = await this.CheckIsVTCommandsSupported();
				foreach (string s in StringArray)
				{
					string s;
					if (s.StartsWith("DELAY"))
					{
						string text = s.Substring(5);
						int num = 0;
						if (int.TryParse(text, out num))
						{
							await Task.Delay(num);
						}
					}
					else if (s.StartsWith("WAIT"))
					{
						string text2 = s.Substring(4);
						int num2 = 0;
						if (int.TryParse(text2, out num2))
						{
							await Task.Delay(num2);
						}
					}
					else
					{
						await this.SendString(s);
						this.CheckReplyForBadELM(s, (s.StartsWith("AT") && (!s.Contains("ATFI") && !s.Contains("ATSI"))) ? (await this.ReadData(2500, null, -1)) : (await this.ReadData(15000, null, -1)));
						s = null;
					}
				}
				string[] array = null;
				if (!Decode)
				{
					return true;
				}
				bool DecodeResult = false;
				int k = 0;
				while (k < 2)
				{
					OBDDataReader.<>c__DisplayClass170_0 CS$<>8__locals1 = new OBDDataReader.<>c__DisplayClass170_0();
					CS$<>8__locals1.cmd = null;
					if (string.IsNullOrEmpty(SharedSettings.Current.DetectECUConnectionPID))
					{
						CS$<>8__locals1.cmd = this.GetDefaultPidRequest();
					}
					else
					{
						CS$<>8__locals1.cmd = new OBDRequest(SharedSettings.Current.DetectECUConnectionPID, false);
					}
					string s = "";
					for (int i = 0; i < 2; i++)
					{
						await this.SendRequest(CS$<>8__locals1.cmd);
						s = await this.ReadData(15000, async delegate
						{
							await this.DebugWrite("\r\n[InitializeCustomInitString:LengthTooBigHandler]\r\n");
						}, -1);
					}
					if (initMode == OBDDataReader.InitModes.Default)
					{
						try
						{
							int num3 = OBDResponseAnalyzer.GetProtocolNumberFromInitString(InitString);
							if (num3 <= 0 || num3 >= 10)
							{
								num3 = await this.GetCurrentProtocolNumber();
							}
							if (num3 >= 1 && num3 <= 10)
							{
								this.CurrentProtocolNumber = num3;
								this.CurrentELMFormat = OBDResponseAnalyzer.GetELMFormatFromProtocolNumber(num3);
							}
							else
							{
								this.CurrentELMFormat = OBDResponseAnalyzer.AnalyzeResponse(CS$<>8__locals1.cmd.Command, s);
								switch (this.CurrentELMFormat)
								{
								case ELMFormat.KWP:
									this.CurrentProtocolNumber = 4;
									break;
								case ELMFormat.CAN11bit:
									this.CurrentProtocolNumber = 6;
									break;
								case ELMFormat.CAN29bit:
									this.CurrentProtocolNumber = 7;
									break;
								}
							}
							this.ParseECUHeaders(CS$<>8__locals1.cmd.Command, s);
							List<PID> list = (from x in this.CurrentCarData.LiveDataPIDs.Concat(CustomPIDViewModel.CurrentProfile.PidCollection).Concat(CustomPIDViewModel.CurrentCustom.PidCollection)
								where x.Command == CS$<>8__locals1.cmd.Command
								select x).ToList<PID>();
							if (SharedSettings.Current.CheckOnlyPositiveResponseMarker && s != null)
							{
								string responseMarkerFromCommand = OBDRequest.GetResponseMarkerFromCommand(CS$<>8__locals1.cmd.Command);
								if (OBDDataReader.FilterHexAndNewLineOnly(s).Contains(responseMarkerFromCommand))
								{
									DecodeResult = true;
								}
							}
							else
							{
								DecodeResult = await this.DecodeData(s, new OBDRequest(CS$<>8__locals1.cmd.Command, false, list), null);
							}
							if (DecodeResult)
							{
								if (s != null)
								{
									if (s.Count((char c) => c == ' ') >= 1)
									{
										this.ELMStatus.ELMSupportsATS0 = false;
									}
								}
								break;
							}
							goto IL_0D35;
						}
						catch (Exception)
						{
							goto IL_0D35;
						}
						goto IL_0CB6;
					}
					goto IL_0CB6;
					IL_0D35:
					CS$<>8__locals1 = null;
					s = null;
					k++;
					continue;
					IL_0CB6:
					DecodeResult = await this.DecodeData(s, CS$<>8__locals1.cmd, null);
					goto IL_0D35;
				}
				if (DecodeResult)
				{
					await this.SetSTNLevelCANTransmitSegmentation(SharedSettings.Current.CANRequestSegmentationSTNLevel);
					await this.SetSTNLevelCANReceiveSegmentation(SharedSettings.Current.CANResponseSegmentationSTNLevel);
					initSuccess = true;
				}
				StringArray = null;
			}
			catch (Exception ex)
			{
				throw ex;
			}
			return initSuccess;
		}

		// Token: 0x06002520 RID: 9504 RVA: 0x001C4014 File Offset: 0x001C2214
		private async ValueTask SetSTNLevelCANTransmitSegmentation(bool value)
		{
			if (this.STCommandsStupported && SharedSettings.Current.CANRequestSegmentationSTNLevel)
			{
				if (value)
				{
					await this.SendString("STCSEGT1");
				}
				else
				{
					await this.SendString("STCSEGT0");
				}
				string text = await this.ReadData(2500, null, -1);
				if (text != null && text.Contains("OK"))
				{
					this.ELMStatus.SetSTNTransmitSegmentation(value);
				}
				else
				{
					SharedSettings.Current.CANRequestSegmentationSTNLevel = false;
				}
			}
		}

		// Token: 0x06002521 RID: 9505 RVA: 0x001C4060 File Offset: 0x001C2260
		private async ValueTask SetSTNLevelCANReceiveSegmentation(bool value)
		{
			if (this.STCommandsStupported && SharedSettings.Current.CANResponseSegmentationSTNLevel)
			{
				if (value)
				{
					await this.SendString("STCSEGR1");
				}
				else
				{
					await this.SendString("STCSEGR0");
				}
				string text = await this.ReadData(2500, null, -1);
				if (text != null && text.Contains("OK"))
				{
					this.ELMStatus.SetSTNReceiveSegmentation(value);
				}
				else
				{
					SharedSettings.Current.CANResponseSegmentationSTNLevel = false;
				}
			}
		}

		// Token: 0x06002522 RID: 9506 RVA: 0x001C40AC File Offset: 0x001C22AC
		private void CheckReplyForBadELM(string cmd, string reply)
		{
			if (cmd.StartsWith("AT", StringComparison.OrdinalIgnoreCase))
			{
				if (reply.Contains("?"))
				{
					this.BadELM = true;
					this.ELMStatus.ReportATCommandNotSupported(cmd);
					return;
				}
				if (cmd.StartsWith("ATSP") && reply.Contains("not"))
				{
					this.BadELM = true;
					this.ELMStatus.ReportATCommandNotSupported(cmd);
					return;
				}
				if (cmd.StartsWith("ATIB") && reply.Contains("KBAUD"))
				{
					this.BadELM = true;
					this.ELMStatus.ReportATCommandNotSupported(cmd);
					return;
				}
				if (!reply.EndsWith("\r\r>"))
				{
					this.BadELM = true;
					this.ELMStatus.ReportATCommandNotSupported(cmd);
					return;
				}
			}
		}

		// Token: 0x06002523 RID: 9507 RVA: 0x001C4168 File Offset: 0x001C2368
		private async Task<string> SendInitCommand(string command, int customReadTimeout_ms = -1)
		{
			string text;
			if (string.IsNullOrEmpty(command))
			{
				text = "";
			}
			else
			{
				command = command.ToUpperInvariant();
				if (command.StartsWith("DELAY"))
				{
					string text2 = command.Substring(5);
					int num = 0;
					if (int.TryParse(text2, out num))
					{
						await Task.Delay(num);
					}
					text = "";
				}
				else if (command.StartsWith("WAIT"))
				{
					string text3 = command.Substring(4);
					int num2 = 0;
					if (int.TryParse(text3, out num2))
					{
						await Task.Delay(num2);
					}
					text = "";
				}
				else
				{
					await this.SendString(command);
					string text4 = ((customReadTimeout_ms <= 0) ? (await this.ReadData(2500, null, -1)) : (await this.ReadData(customReadTimeout_ms, null, -1)));
					if (text4 != null && text4.Contains("STOPPED"))
					{
						await this.SendString(command);
						await Task.Delay(200);
						text4 = ((customReadTimeout_ms <= 0) ? (await this.ReadData(2500, null, -1)) : (await this.ReadData(customReadTimeout_ms, null, -1)));
					}
					this.CheckReplyForBadELM(command, text4);
					text = text4;
				}
			}
			return text;
		}

		// Token: 0x06002524 RID: 9508 RVA: 0x001C41BC File Offset: 0x001C23BC
		private async Task<bool> InitializeDefaultInitString(int ProtocolNumber, OBDDataReader.InitModes initMode = OBDDataReader.InitModes.Default, bool useATST96 = false, bool Decode = true)
		{
			OBDDataReader.<>c__DisplayClass175_0 CS$<>8__locals1 = new OBDDataReader.<>c__DisplayClass175_0();
			CS$<>8__locals1.<>4__this = this;
			CS$<>8__locals1.initMode = initMode;
			await this.DebugWrite(string.Concat(new string[]
			{
				"\r\n\nInitializeDefaultInitString(ProtocolNumber=",
				ProtocolNumber.ToString(),
				",initMode=",
				CS$<>8__locals1.initMode.ToString(),
				",useATST96=",
				useATST96.ToString(),
				")\r\n"
			}));
			bool initSuccess = false;
			string reply = string.Empty;
			string[] default_init = new string[] { "ATD", "ATD0", "ATE0", "ATH1" };
			string[] post_init = new string[]
			{
				"ATE0",
				"ATH1",
				"ATM0",
				"ATS0",
				"ATAT" + SharedSettings.Current.AdaptiveTimings.ToString(CultureInfo.InvariantCulture),
				"ATAL"
			};
			string[] nc2_init = new string[]
			{
				"ATD",
				"ATD0",
				"ATE0",
				"ATH1",
				"ATM0",
				"ATS0",
				"ATAT" + SharedSettings.Current.AdaptiveTimings.ToString(CultureInfo.InvariantCulture),
				"ATSP5",
				"ATAL",
				"ATIB10",
				"ATSH8110FC",
				"ATST20",
				"ATSW05",
				"2212010401",
				"221201",
				"ATSW05",
				"ATWM221201"
			};
			int num = 0;
			try
			{
				OBDDataReader.<>c__DisplayClass175_1 CS$<>8__locals2 = new OBDDataReader.<>c__DisplayClass175_1();
				if (ProtocolNumber == 11)
				{
					if (CS$<>8__locals1.initMode == OBDDataReader.InitModes.Default)
					{
						this.CurrentCarData.AddOrRemoveNissanConsultPIDsV2(true);
					}
					if (CS$<>8__locals1.initMode == OBDDataReader.InitModes.RestoreConnection)
					{
						await this.SendATZ();
					}
					foreach (string cmd in nc2_init)
					{
						if (cmd == "2212010401")
						{
							string text = await this.SendInitCommand(cmd, 10000);
							if (text != null && text.Contains("BUS INIT: ERROR"))
							{
								await this.SendInitCommand(cmd, 10000);
							}
						}
						else
						{
							await this.SendInitCommand(cmd, -1);
						}
						cmd = null;
					}
					string[] array = null;
				}
				else
				{
					if (CS$<>8__locals1.initMode == OBDDataReader.InitModes.Default)
					{
						bool flag = false;
						if (ProtocolNumber == 43 || ProtocolNumber == 46)
						{
							flag = true;
						}
						this.CurrentCarData.AddOrRemoveNissanConsultPIDsV2(flag);
					}
					if (CS$<>8__locals1.initMode == OBDDataReader.InitModes.RestoreConnection)
					{
						await this.SendATZ();
					}
					await this.SendString("ATE0");
					await Task.Delay(300);
					await this.ReadData(2500, null, -1);
					await this.SendString("ATE0");
					await Task.Delay(300);
					await this.ReadData(2500, null, -1);
					this.STCommandsStupported = await this.CheckIsSTCommandsSupported();
					this.VTCommandsStupported = await this.CheckIsVTCommandsSupported();
					string[] array = default_init;
					for (int j = 0; j < array.Length; j++)
					{
						await this.SendInitCommand(array[j], -1);
					}
					array = null;
					if (ProtocolNumber < 11)
					{
						await this.SendString("ATSP" + ProtocolNumber.ToString("X1", CultureInfo.InvariantCulture));
						reply = await this.ReadData(2500, null, -1);
						string.IsNullOrEmpty(reply);
						this.CheckReplyForBadELM("ATSP", reply);
					}
					if (ProtocolNumber > 11)
					{
						foreach (string text2 in OBDDataReader.GetAdditionalInit(ProtocolNumber))
						{
							await this.SendInitCommand(text2, -1);
						}
						List<string>.Enumerator enumerator = default(List<string>.Enumerator);
					}
					array = post_init;
					for (int j = 0; j < array.Length; j++)
					{
						await this.SendInitCommand(array[j], -1);
					}
					array = null;
					await this.SetSTNLevelCANReceiveSegmentation(SharedSettings.Current.CANResponseSegmentationSTNLevel);
					await this.SetSTNLevelCANTransmitSegmentation(SharedSettings.Current.CANRequestSegmentationSTNLevel);
				}
				if (!Decode)
				{
					return true;
				}
				bool DecodeResult = false;
				CS$<>8__locals2.atst = "64";
				if (useATST96)
				{
					CS$<>8__locals2.atst = "96";
				}
				else
				{
					CS$<>8__locals2.atst = SharedSettings.Current.GetATST();
				}
				for (int j = 0; j < 2; j++)
				{
					if (CS$<>8__locals2.atst != "")
					{
						await this.SendString("ATST" + CS$<>8__locals2.atst);
						await this.ReadData(2500, null, -1);
					}
					string cmd = "";
					if (ProtocolNumber == 11 || ProtocolNumber == 43)
					{
						cmd = "221201";
					}
					else
					{
						cmd = SharedSettings.Current.Mode01Prefix + "00";
					}
					int timeout = 10;
					if ((ProtocolNumber >= 6 && ProtocolNumber <= 9) || ProtocolNumber == 43)
					{
						timeout = 10;
					}
					else
					{
						timeout = 15;
					}
					if (CS$<>8__locals1.initMode == OBDDataReader.InitModes.Default)
					{
						string cmd_reply = "";
						for (int i = 0; i < 4; i++)
						{
							OBDDataReader.<>c__DisplayClass175_2 CS$<>8__locals3 = new OBDDataReader.<>c__DisplayClass175_2();
							CS$<>8__locals3.CS$<>8__locals1 = CS$<>8__locals2;
							CS$<>8__locals3.atstChangedBecauseOfLengthTooBigHandler = false;
							await this.SendString(cmd);
							cmd_reply = await this.ReadData(timeout * 1000, delegate
							{
								int num5 = 72;
								if (int.TryParse(CS$<>8__locals3.CS$<>8__locals1.atst, NumberStyles.HexNumber, CultureInfo.InvariantCulture.NumberFormat, out num5))
								{
									if (num5 <= 50)
									{
										if (num5 == 22 || num5 == 50)
										{
											CS$<>8__locals3.CS$<>8__locals1.atst = "08";
										}
									}
									else if (num5 != 100)
									{
										if (num5 != 150)
										{
											if (num5 == 255)
											{
												CS$<>8__locals3.CS$<>8__locals1.atst = "96";
											}
										}
										else
										{
											CS$<>8__locals3.CS$<>8__locals1.atst = "32";
										}
									}
									else
									{
										CS$<>8__locals3.CS$<>8__locals1.atst = "16";
									}
									CS$<>8__locals3.atstChangedBecauseOfLengthTooBigHandler = true;
								}
							}, -1);
							await this.DebugWrite("\r\n[cmd_reply=] " + (cmd_reply ?? "null") + "\r\n");
							if (CS$<>8__locals3.atstChangedBecauseOfLengthTooBigHandler)
							{
								await this.SendString("ATST" + CS$<>8__locals3.CS$<>8__locals1.atst);
								await this.ReadData(2500, null, -1);
								await this.SendString("ATST" + CS$<>8__locals3.CS$<>8__locals1.atst);
								await this.ReadData(2500, null, -1);
								string atst = CS$<>8__locals3.CS$<>8__locals1.atst;
								if (!(atst == "08"))
								{
									if (!(atst == "16"))
									{
										if (!(atst == "32"))
										{
											if (!(atst == "48"))
											{
												if (!(atst == "64"))
												{
													if (atst == "96")
													{
														SharedSettings.Current.ATSTIdx = 7;
													}
												}
												else
												{
													SharedSettings.Current.ATSTIdx = 6;
												}
											}
											else
											{
												SharedSettings.Current.ATSTIdx = 5;
											}
										}
										else
										{
											SharedSettings.Current.ATSTIdx = 4;
										}
									}
									else
									{
										SharedSettings.Current.ATSTIdx = 3;
									}
								}
								else
								{
									SharedSettings.Current.ATSTIdx = 2;
								}
							}
							if (cmd_reply != null)
							{
								cmd_reply = cmd_reply.Replace("SEARCHING", "").Replace("UNABLE TO CONNECT", "").Replace("CAN ERROR", "");
							}
							int num2 = 0;
							try
							{
								int _protocol = ProtocolNumber;
								if (_protocol == 0)
								{
									_protocol = await this.GetCurrentProtocolNumber();
									await this.DebugWrite(string.Format("\r\nProtocolFromATDPN={0}\r\n", _protocol));
								}
								if ((_protocol >= 1 && _protocol <= 11) || _protocol == 43)
								{
									this.CurrentProtocolNumber = _protocol;
									this.CurrentELMFormat = OBDResponseAnalyzer.GetELMFormatFromProtocolNumber(_protocol);
								}
								else
								{
									this.CurrentELMFormat = OBDResponseAnalyzer.AnalyzeResponse(cmd, cmd_reply);
									switch (this.CurrentELMFormat)
									{
									case ELMFormat.KWP:
										this.CurrentProtocolNumber = 4;
										break;
									case ELMFormat.CAN11bit:
										this.CurrentProtocolNumber = 6;
										break;
									case ELMFormat.CAN29bit:
										this.CurrentProtocolNumber = 7;
										break;
									}
								}
								if (this.CurrentELMFormat == ELMFormat.CAN11bit && (SharedSettings.Current.SelectedBrand == "Toyota" || SharedSettings.Current.SelectedBrand == "Lexus") && cmd == "0100" && cmd_reply != null && !cmd_reply.Contains("4100") && cmd_reply.Contains("7E8037F0111"))
								{
									SharedSettings.Current.ProfileUpdateAlias = "ad685c0700ea4f348b9078befe8d5709";
									new ProfileV2Model().GetProfileForUpdate("ad685c0700ea4f348b9078befe8d5709").Apply(SharedSettings.Current.SelectedBrand);
									this.CurrentCarData.CreateEmptyPIDS();
									return await this.InitializeCustomInitString(SharedSettings.Current.CustomInitString, CS$<>8__locals1.initMode, Decode);
								}
								this.ParseECUHeaders(cmd, cmd_reply);
								if (SharedSettings.Current.CheckOnlyPositiveResponseMarker)
								{
									if (cmd_reply != null)
									{
										string responseMarkerFromCommand = OBDRequest.GetResponseMarkerFromCommand(cmd);
										if (cmd_reply.Replace(" ", "").Contains(responseMarkerFromCommand))
										{
											DecodeResult = true;
										}
									}
								}
								else
								{
									await this.DebugWrite(string.Concat(new string[]
									{
										"\r\n[Trying to decode:\r\n",
										reply,
										"\r\nELMFormat=",
										this.CurrentELMFormat.ToString(),
										"]\r\n"
									}));
									DecodeResult = await this.DecodeData(cmd_reply, new OBDRequest(cmd, false), null);
								}
								await this.DebugWrite(string.Format("\r\n[DecodeResult={0}]\r\n", DecodeResult));
								if (DecodeResult && cmd_reply != null)
								{
									if (cmd_reply.Count((char c) => c == ' ') >= 1)
									{
										this.ELMStatus.ELMSupportsATS0 = false;
									}
								}
								if (!DecodeResult && this.CurrentELMFormat == ELMFormat.KWP && _protocol >= 1 && _protocol <= 5 && cmd_reply != null)
								{
									if (cmd_reply.Count((char c) => c == '\r') >= 9)
									{
										string[] array2 = cmd_reply.Split(OBDDataReader.line_splitter, StringSplitOptions.RemoveEmptyEntries);
										if (array2.Count((string line) => line.Length == 2) >= 9)
										{
											int num3 = EnumerableExtensions.IndexOf<string>(array2, (string x) => x.Length == 2);
											StringBuilder stringBuilder = new StringBuilder(12);
											for (int k = num3; k < array2.Length; k++)
											{
												if (array2[k].Length == 2)
												{
													stringBuilder.Append(array2[k]);
												}
											}
											this.ParseECUHeaders(cmd, stringBuilder.ToString());
											SharedSettings.Current.KWPConcatResponseLines = true;
											DecodeResult = await this.DecodeData(cmd_reply, new OBDRequest(cmd, false), null);
										}
									}
								}
							}
							catch (Exception obj)
							{
								num2 = 1;
							}
							object obj;
							if (num2 == 1)
							{
								Exception ex = (Exception)obj;
								await this.DebugWrite(Encoding.UTF8.GetBytes(string.Format("\r\nInit decode attempt #{0} failed! Command: {1}, cmd_reply:\r\n {2} \r\nException:\r\n {3} \r\n", new object[]
								{
									i,
									cmd,
									cmd_reply,
									ex.ToString()
								})));
							}
							obj = null;
							if (DecodeResult)
							{
								break;
							}
							CS$<>8__locals3 = null;
						}
						if (DecodeResult)
						{
							break;
						}
						cmd_reply = null;
					}
					else
					{
						OBDRequest request = new OBDRequest(cmd, this.GetDefaultHeader(), "", "", false);
						await this.SendRequest(request);
						int num4 = 15000;
						Action action;
						if ((action = CS$<>8__locals1.<>9__5) == null)
						{
							OBDDataReader.<>c__DisplayClass175_0 CS$<>8__locals4 = CS$<>8__locals1;
							Action action2 = delegate
							{
								OBDDataReader.<>c__DisplayClass175_0.<<InitializeDefaultInitString>b__5>d <<InitializeDefaultInitString>b__5>d;
								<<InitializeDefaultInitString>b__5>d.<>t__builder = AsyncVoidMethodBuilder.Create();
								<<InitializeDefaultInitString>b__5>d.<>4__this = CS$<>8__locals1;
								<<InitializeDefaultInitString>b__5>d.<>1__state = -1;
								<<InitializeDefaultInitString>b__5>d.<>t__builder.Start<OBDDataReader.<>c__DisplayClass175_0.<<InitializeDefaultInitString>b__5>d>(ref <<InitializeDefaultInitString>b__5>d);
							};
							CS$<>8__locals4.<>9__5 = action2;
							action = action2;
						}
						reply = await this.ReadData(num4, action, -1);
						DecodeResult = await this.DecodeData(reply, request, null);
						request = null;
					}
					cmd = null;
				}
				if (DecodeResult)
				{
					initSuccess = true;
					SharedSettings.Current.LastSuccessfulProtocol = ProtocolNumber;
				}
				CS$<>8__locals2 = null;
			}
			catch (Exception obj2)
			{
				num = 1;
			}
			object obj2;
			if (num == 1)
			{
				Exception exc = (Exception)obj2;
				await this.DebugWrite(Encoding.UTF8.GetBytes(string.Concat(new string[]
				{
					"\r\nInit error!\r\nData:\r\n",
					reply,
					"\r\nException:\r\n",
					exc.ToString(),
					"\r\n"
				})));
				throw exc;
			}
			obj2 = null;
			return initSuccess;
		}

		// Token: 0x06002525 RID: 9509 RVA: 0x001C4220 File Offset: 0x001C2420
		private async Task<string> SendATZ()
		{
			if (!SharedSettings.Current.SendATZATE)
			{
				await this.SendString("ATZ");
				this.ELMStatus.Reset();
			}
			else
			{
				await this.SendString("ATZ\rATE0\r\r\r\r\r\r\r\r\r\r");
				this.ELMStatus.Reset();
				this.ELMStatus.UpdateStatusFromCommand("ATE0");
			}
			if (!SharedSettings.Current.NoDelayELM327Init)
			{
				await Task.Delay(400);
			}
			string reply = await this.ReadData(6000, null, -1);
			if (SharedSettings.Current.ShowExperimental)
			{
				byte[] discardBuffer = await this.Connection.ReadBytesAsync();
				if (discardBuffer != null && discardBuffer.Length != 0)
				{
					await this.DebugWrite("[DiscardBuffer=" + BitHelpers.ByteArrayToHexString(discardBuffer) + "]");
					await this.DebugWrite(discardBuffer);
				}
				discardBuffer = null;
			}
			return reply;
		}

		// Token: 0x06002526 RID: 9510 RVA: 0x001C4264 File Offset: 0x001C2464
		private static List<string> GetAdditionalInit(int ProtocolNumber)
		{
			List<string> list = new List<string>(8);
			switch (ProtocolNumber)
			{
			case 12:
				list.Add("ATSP5");
				list.Add("ATIB96");
				break;
			case 13:
				list.Add("ATSP5");
				list.Add("ATIB48");
				break;
			case 14:
				list.Add("ATSP5");
				list.Add("ATIB48");
				list.Add("ATIIA7A");
				break;
			case 15:
				list.Add("ATSP5");
				list.Add("ATIB48");
				list.Add("ATIIA13");
				break;
			case 16:
				list.Add("ATSP5");
				list.Add("ATIB48");
				list.Add("ATIIA33");
				break;
			case 17:
				list.Add("ATSP4");
				list.Add("ATIB96");
				break;
			case 18:
				list.Add("ATSP4");
				list.Add("ATIB48");
				break;
			case 19:
				list.Add("ATSP4");
				list.Add("ATIB48");
				list.Add("ATIIA7A");
				break;
			case 20:
				list.Add("ATSP4");
				list.Add("ATIB48");
				list.Add("ATIIA13");
				break;
			case 21:
				list.Add("ATSP4");
				list.Add("ATIB48");
				list.Add("ATIIA33");
				break;
			case 22:
				list.Add("ATSP3");
				list.Add("ATIB96");
				break;
			case 23:
				list.Add("ATSP3");
				list.Add("ATIB48");
				break;
			case 24:
				list.Add("ATSP3");
				list.Add("ATIB48");
				list.Add("ATIIA7A");
				break;
			case 25:
				list.Add("ATSP3");
				list.Add("ATIB48");
				list.Add("ATIIA13");
				break;
			case 26:
				list.Add("ATSP3");
				list.Add("ATIB48");
				list.Add("ATIIA33");
				break;
			case 27:
				list.Add("ATSP5");
				list.Add("ATSH8013F1");
				list.Add("ATIB10");
				list.Add("ATIIA13");
				break;
			case 28:
				list.Add("ATSP5");
				list.Add("ATSH8013F0");
				list.Add("ATIB96");
				list.Add("ATIIA13");
				break;
			case 29:
				list.Add("ATSP5");
				list.Add("ATSH8213F0");
				list.Add("ATIB96");
				list.Add("ATIIA13");
				break;
			case 30:
				list.Add("ATSP5");
				list.Add("ATSH8013FC");
				list.Add("ATIB10");
				list.Add("ATIIA10");
				break;
			case 31:
				list.Add("ATSP5");
				list.Add("ATSH8013FC");
				list.Add("ATIB96");
				list.Add("ATIIA10");
				break;
			case 32:
				list.Add("ATSP4");
				list.Add("ATSH8013F1");
				list.Add("ATIB10");
				list.Add("ATIIA13");
				break;
			case 33:
				list.Add("ATSP4");
				list.Add("ATSH8013F0");
				list.Add("ATIB96");
				list.Add("ATIIA13");
				break;
			case 34:
				list.Add("ATSP4");
				list.Add("ATSH8213F0");
				list.Add("ATIB96");
				list.Add("ATIIA13");
				break;
			case 35:
				list.Add("ATSP4");
				list.Add("ATSH8013FC");
				list.Add("ATIB10");
				list.Add("ATIIA13");
				break;
			case 36:
				list.Add("ATSP4");
				list.Add("ATSH8013F1");
				list.Add("ATIB96");
				list.Add("ATIIA13");
				break;
			case 37:
				list.Add("ATSP4");
				list.Add("ATSH8113F1");
				list.Add("ATIB96");
				list.Add("ATIIA13");
				break;
			case 38:
				list.Add("ATSP5");
				list.Add("ATSH8110FC");
				list.Add("ATIB10");
				list.Add("ATIIA10");
				break;
			case 39:
				list.Add("ATSP4");
				list.Add("ATSH8013F1");
				list.Add("ATIB10");
				list.Add("ATIIA13");
				break;
			case 40:
				list.Add("ATSP5");
				list.Add("ATSH8113F1");
				list.Add("ATIB96");
				list.Add("ATIIA13");
				break;
			case 41:
				list.Add("ATSP5");
				list.Add("ATSH8213F1");
				list.Add("ATIB96");
				list.Add("ATIIA13");
				break;
			case 42:
				list.Add("ATSP3");
				list.Add("ATSH686AF1");
				list.Add("ATIB10");
				list.Add("ATIIA33");
				break;
			case 43:
				list.Add("ATSP6");
				list.Add("ATSH7E0");
				list.Add("10C0");
				break;
			case 44:
				list.Add("ATSP5");
				list.Add("ATSH8110F1");
				list.Add("ATIB10");
				list.Add("ATST20");
				break;
			case 45:
				list.Add("ATSP6");
				list.Add("ATFCSH7E0");
				list.Add("ATFCSD30000000");
				list.Add("ATFCSM1");
				break;
			}
			return list;
		}

		// Token: 0x06002527 RID: 9511 RVA: 0x001C4894 File Offset: 0x001C2A94
		public async Task<int> SearchForProtocol(IProgress<string> progress)
		{
			this.CurrentStatus = OBDDataReaderStatus.ConnectingToECU;
			int[] array = new int[]
			{
				0, 6, 7, 4, 5, 3, 1, 2, 8, 9,
				10, 11, 37, 33, 43, 44, 45
			};
			int[] array2 = new int[]
			{
				0, 6, 7, 4, 5, 3, 37, 33, 44, 12,
				13, 14, 15, 16, 17, 18, 19, 20, 21, 22,
				23, 24, 25, 26, 27, 28, 29, 30, 31, 32,
				34, 35, 36, 38, 39, 40, 41, 42, 45
			};
			int[] protocols = array;
			if (SharedSettings.Current.SelectedBrand == "Toyota" || SharedSettings.Current.SelectedBrand == "Lexus")
			{
				protocols = array2;
			}
			int i = 1;
			while (i < protocols.Length)
			{
				int num;
				if (this.DisconnectRequested)
				{
					num = 0;
				}
				else
				{
					string[] array3 = new string[6];
					array3[0] = Translate.GetString("ios_ECUinitProgressStage2");
					array3[1] = "\n[";
					array3[2] = i.ToString();
					array3[3] = "/";
					int num2 = 4;
					int num3 = protocols.Length;
					array3[num2] = num3.ToString();
					array3[5] = "]";
					string text = string.Concat(array3);
					string text2 = StaticLists.Protocols[protocols[i]];
					text = string.Format(text, text2);
					if (progress != null)
					{
						progress.Report(text);
					}
					bool result = false;
					int attempts = 0;
					while (attempts < 3)
					{
						if (this.DisconnectRequested)
						{
							return 0;
						}
						num3 = attempts;
						attempts = num3 + 1;
						num3 = 0;
						try
						{
							bool flag = await this.InitializeDefaultInitString(protocols[i], OBDDataReader.InitModes.Default, true, true);
							result = flag;
							break;
						}
						catch (Exception obj)
						{
							num3 = 1;
						}
						if (num3 == 1)
						{
							object obj;
							Exception ex = (Exception)obj;
							TaskAwaiter<bool> taskAwaiter = this.Connect(true).GetAwaiter();
							if (!taskAwaiter.IsCompleted)
							{
								await taskAwaiter;
								TaskAwaiter<bool> taskAwaiter2;
								taskAwaiter = taskAwaiter2;
								taskAwaiter2 = default(TaskAwaiter<bool>);
							}
							if (!taskAwaiter.GetResult() && attempts == 2)
							{
								return 0;
							}
						}
						if (this.DisconnectRequested)
						{
							return 0;
						}
					}
					if (!result)
					{
						num3 = i;
						i = num3 + 1;
						continue;
					}
					this.CurrentStatus = OBDDataReaderStatus.ConnectedToECU;
					SharedSettings.Current.ATSTIdx = 7;
					num = protocols[i];
				}
				return num;
			}
			return 0;
		}

		// Token: 0x06002528 RID: 9512 RVA: 0x001C48E0 File Offset: 0x001C2AE0
		public static string FilterHexAndNewLineOnly(string input)
		{
			if (input == null)
			{
				return "";
			}
			int num = 0;
			if (input.StartsWith("SEARCHING...", StringComparison.Ordinal))
			{
				num = 12;
			}
			bool flag = false;
			bool flag2 = false;
			int i = num;
			while (i < input.Length)
			{
				char c = input[i];
				if ((c < '0' || c > '9') && (c < 'A' || c > 'Z') && c != '\n' && c != '\r' && (c < 'a' || c > 'z'))
				{
					if (i == input.Length - 1)
					{
						flag2 = true;
						break;
					}
					flag = true;
					break;
				}
				else
				{
					i++;
				}
			}
			if (flag2)
			{
				return input.Substring(0, input.Length - 1);
			}
			if (flag)
			{
				OBDDataReader.filterSb.Clear();
				foreach (char c2 in input)
				{
					if ((c2 >= '0' && c2 <= '9') || (c2 >= 'A' && c2 <= 'Z') || c2 == '\n' || c2 == '\r' || (c2 >= 'a' && c2 <= 'z'))
					{
						OBDDataReader.filterSb.Append(c2);
					}
				}
				return OBDDataReader.filterSb.ToString();
			}
			return input;
		}

		// Token: 0x06002529 RID: 9513 RVA: 0x001C49F4 File Offset: 0x001C2BF4
		private void ParseECUHeaders(string cmd, string reply0100)
		{
			reply0100 = OBDDataReader.FilterHexAndNewLineOnly(reply0100);
			this.InvokeOnMainThread(delegate
			{
				this._ECUHeaders.Clear();
			});
			string reply_marker = OBDRequest.GetResponseMarkerFromCommand(cmd);
			string[] array = (from x in reply0100.Split(OBDDataReader.line_splitter, StringSplitOptions.RemoveEmptyEntries)
				select x.Trim() into x
				where x.Length > 3 && x.Contains(reply_marker)
				select x).ToArray<string>();
			if (array.Length != 0)
			{
				List<string> tHeaders = new List<string>();
				foreach (string text in array)
				{
					try
					{
						int num = text.IndexOf(reply_marker);
						string text2 = text.Substring(0, num);
						switch (this.CurrentELMFormat)
						{
						case ELMFormat.Unknown:
							this.ParseECUHeadersOld(cmd, reply0100);
							break;
						case ELMFormat.KWP:
							text2 = text2.Substring(4, 2);
							break;
						case ELMFormat.CAN11bit:
						{
							string text3 = text2.Substring(0, 3);
							text.Substring(num);
							if (text.Length % 2 == 0 && text[1] == '7')
							{
								text3 = text2.Substring(1, 3);
							}
							text2 = text3;
							break;
						}
						case ELMFormat.CAN29bit:
							if (num == 10 || num == 12 || (this.ELMStatus.STNReceiveSegmentation && num == 8) || num == 10)
							{
								text2 = text2.Substring(6, 2);
							}
							else
							{
								text2 = "";
							}
							break;
						}
						if (!string.IsNullOrEmpty(text2))
						{
							tHeaders.Add(text2);
						}
					}
					catch (Exception)
					{
					}
				}
				if (tHeaders.Any((string x) => x.Length == 3))
				{
					if (tHeaders.Any((string x) => x.Length != 3))
					{
						tHeaders.RemoveAll((string x) => x.Length != 3);
					}
				}
				tHeaders.Sort();
				this.InvokeOnMainThread(delegate
				{
					foreach (string text4 in tHeaders)
					{
						this._ECUHeaders.Add(new ECUHeader(text4));
					}
				});
				this.SelectedECU = 0;
			}
		}

		// Token: 0x0600252A RID: 9514 RVA: 0x001C4C5C File Offset: 0x001C2E5C
		public void ParseECUHeadersOld(string cmd, string reply0100)
		{
			this.InvokeOnMainThread(delegate
			{
				this._ECUHeaders.Clear();
			});
			string reply_marker = OBDRequest.GetResponseMarkerFromCommand(cmd);
			string[] array = (from x in reply0100.Replace(" ", string.Empty).Split(OBDDataReader.line_splitter, StringSplitOptions.RemoveEmptyEntries)
				select x.Trim() into x
				where x.Contains(reply_marker)
				select x).ToArray<string>();
			List<string> tHeaders = new List<string>();
			foreach (string text in array)
			{
				int num = text.IndexOf(reply_marker);
				if (num > 0)
				{
					string text2 = text.Substring(0, num);
					int length = text2.Length;
					if (length != 5)
					{
						if (length != 10)
						{
							text2 = text2.Substring(text2.Length - 2);
						}
						else
						{
							int num2 = BitHelpers.ConvertHexToInt(text2.Substring(text2.Length - 2));
							string text3 = text.Substring(num);
							if (num2 == text3.Length / 2)
							{
								text2 = text2.Substring(text2.Length - 4, 2);
							}
							else
							{
								text2 = text2.Substring(text2.Length - 2);
							}
						}
					}
					else
					{
						text2 = text2.Substring(0, 3);
					}
					tHeaders.Add(text2);
				}
			}
			tHeaders.Sort();
			if (tHeaders.Any((string x) => x.Length == 3))
			{
				if (tHeaders.Any((string x) => x.Length != 3))
				{
					tHeaders.RemoveAll((string x) => x.Length != 3);
				}
			}
			this.InvokeOnMainThread(delegate
			{
				foreach (string text4 in tHeaders)
				{
					this._ECUHeaders.Add(new ECUHeader(text4));
				}
			});
			this.SelectedECU = 0;
		}

		// Token: 0x0600252B RID: 9515 RVA: 0x001C4E6C File Offset: 0x001C306C
		public async Task Stop(string message)
		{
			await this.DebugWrite("\nStop(" + message + ")\n");
			this.SetRunning(false, "From Stop #3640");
			Task task = new Task(async delegate
			{
				await this.WaitForCommandQueue();
			});
			Task task2 = Task.Delay(TimeSpan.FromSeconds(2.0));
			await Task.WhenAny(new Task[] { task, task2 });
			await this.DebugWrite("\nStopped\n");
		}

		// Token: 0x0600252C RID: 9516 RVA: 0x001C4EB8 File Offset: 0x001C30B8
		private async Task OnDisconnectDetected(OBDDataReader.DisconnectReason reason)
		{
			OBDDataReader.<>c__DisplayClass184_0 CS$<>8__locals1 = new OBDDataReader.<>c__DisplayClass184_0();
			DriveCycle.SaveAndReset();
			await this.DebugWrite(string.Concat(new string[]
			{
				"\r\nOnDisconnectDetected, reason",
				reason.ToString(),
				" ",
				DateTimeNowHelper.NowSafe.ToString("dd.MM.yyyy HH:mm:ss", CultureInfo.InvariantCulture),
				"\r\n"
			}));
			if (reason == OBDDataReader.DisconnectReason.UserRequested)
			{
				await this.Disconnect("OnDisconnectDetected-UserRequested#1");
			}
			else
			{
				if (this.LastUsedConnectionType == ConnectionTypes.WiFi && this.CommandsCounter == 1L && SharedSettings.Current.AndroidWiFiMode == AndroidWiFiConnectionModes.Auto)
				{
					this.DroidWiFiV2Failed = !this.DroidWiFiV2Failed;
				}
				CS$<>8__locals1.old_queue = this.CommandQueue.ToArray();
				DataRecorderV2 old_recorder = App.OBDReader.CurrentCarData.Recorder;
				if (old_recorder != null)
				{
					App.OBDReader.StatusChanged -= DataRecorderV2.StopRecording;
					old_recorder.Save();
				}
				while (!this.DisconnectRequested)
				{
					if (SharedSettings.Current.StopConnectionAttemptsAfterFailsMinutes > 0 && (this.stopwatch.Elapsed - new TimeSpan(this.lastTimeConnected)).TotalMinutes > (double)SharedSettings.Current.StopConnectionAttemptsAfterFailsMinutes)
					{
						this.Disconnect("StopConnectionAttemptsAfterFailsMinutes");
						if (Device.RuntimePlatform == "Android")
						{
							IAndroidHelperImplementation androidHelperImplementation = DependencyService.Get<IAndroidHelperImplementation>(0);
							if (androidHelperImplementation != null)
							{
								androidHelperImplementation.StopService();
							}
						}
						this.InvokeOnMainThread(delegate
						{
							OBDDataReader.<>c__DisplayClass184_0.<<OnDisconnectDetected>b__0>d <<OnDisconnectDetected>b__0>d;
							<<OnDisconnectDetected>b__0>d.<>t__builder = AsyncVoidMethodBuilder.Create();
							<<OnDisconnectDetected>b__0>d.<>4__this = CS$<>8__locals1;
							<<OnDisconnectDetected>b__0>d.<>1__state = -1;
							<<OnDisconnectDetected>b__0>d.<>t__builder.Start<OBDDataReader.<>c__DisplayClass184_0.<<OnDisconnectDetected>b__0>d>(ref <<OnDisconnectDetected>b__0>d);
						});
						break;
					}
					this.CurrentStatus = OBDDataReaderStatus.ConnectingToELM;
					if (SharedSettings.Current.DelayBeforeReconnect <= 0)
					{
						await Task.Delay(1000);
					}
					else
					{
						await Task.Delay(TimeSpan.FromSeconds((double)SharedSettings.Current.DelayBeforeReconnect));
					}
					if (this.DisconnectRequested)
					{
						await this.Disconnect("OnDisconnectDetected-UserRequested#2");
						break;
					}
					if (this.Connection != null && this.Connection.Connected)
					{
						try
						{
							this.Connection.Disconect();
							await Task.Delay(500);
						}
						catch (Exception)
						{
						}
					}
					TaskAwaiter<bool> taskAwaiter = App.OBDReader.Connect(true).GetAwaiter();
					TaskAwaiter<bool> taskAwaiter2;
					if (!taskAwaiter.IsCompleted)
					{
						await taskAwaiter;
						taskAwaiter = taskAwaiter2;
						taskAwaiter2 = default(TaskAwaiter<bool>);
					}
					if (taskAwaiter.GetResult())
					{
						if (this.DisconnectRequested)
						{
							await this.Disconnect("OnDisconnectDetected-UserRequested#2");
							break;
						}
						taskAwaiter = App.OBDReader.Initialize(3, OBDDataReader.InitModes.RestoreConnection, new Progress<string>()).GetAwaiter();
						if (!taskAwaiter.IsCompleted)
						{
							await taskAwaiter;
							taskAwaiter = taskAwaiter2;
							taskAwaiter2 = default(TaskAwaiter<bool>);
						}
						if (taskAwaiter.GetResult())
						{
							OBDRequestQueueOptimizer.RefreshOBD2Dictionary();
							this.ReplaceQueue(CS$<>8__locals1.old_queue);
							if (old_recorder != null)
							{
								App.OBDReader.StatusChanged -= DataRecorderV2.StopRecording;
								App.OBDReader.StatusChanged += DataRecorderV2.StopRecording;
								App.OBDReader.CurrentCarData.Recorder = old_recorder;
							}
							if (!this.Running)
							{
								this.Start("Start from OnDisconnectRequestedAfterReconnect #4316");
							}
							break;
						}
					}
					if (App.OBDReader.CurrentStatus == OBDDataReaderStatus.ConnectedToECU && !this.DisconnectRequested)
					{
						break;
					}
				}
			}
		}

		// Token: 0x0600252D RID: 9517 RVA: 0x001C4F04 File Offset: 0x001C3104
		public async Task<bool> DecodeMode06Data(string data, string cmd)
		{
			int length = cmd.Length;
			bool flag = false;
			try
			{
				if (data.Length < 2 || data.Contains("NO DATA") || data.Contains("ELM"))
				{
					flag = false;
					return flag;
				}
				string header_filter = this.ECUHeaders[this.SelectedECU].Id;
				string text = "4" + cmd.Substring(1);
				bool IsCAN = false;
				string[] array;
				if (header_filter.Length == 3)
				{
					array = (from x in data.Replace(" ", string.Empty).Split(OBDDataReader.line_splitter, StringSplitOptions.RemoveEmptyEntries)
						select x.Trim() into x
						where x.StartsWith(header_filter)
						select x).ToArray<string>();
					IsCAN = true;
				}
				else
				{
					array = (from x in data.Replace(" ", string.Empty).Split(OBDDataReader.line_splitter, StringSplitOptions.RemoveEmptyEntries)
						select x.Trim()).Where(delegate(string x)
					{
						int num3 = x.IndexOf(header_filter);
						if (num3 % 2 != 0)
						{
							num3 = x.IndexOf(header_filter, num3 + 1);
						}
						if (num3 >= 6 && num3 <= 8 && num3 <= x.Length - header_filter.Length - 2)
						{
							IsCAN = true;
							return true;
						}
						return num3 == 4 && num3 <= x.Length - header_filter.Length - 2;
					}).ToArray<string>();
				}
				if (array.Length == 0)
				{
					flag = false;
				}
				else
				{
					StringBuilder stringBuilder = new StringBuilder(array.Length);
					for (int i = 0; i < array.Length; i++)
					{
						string text2 = this.PrepareResponseLine(array[i]);
						int num = text2.IndexOf(header_filter);
						if (num != 0 && num % 2 != 0)
						{
							num = text2.IndexOf(header_filter, num + 1);
						}
						array[i] = text2.Substring(num + header_filter.Length);
					}
					array = array.OrderBy((string x) => x).ToArray<string>();
					for (int j = 0; j < array.Length; j++)
					{
						string text3;
						if (j == 0)
						{
							text3 = "1";
						}
						else
						{
							text3 = "2";
						}
						if (array[j].Substring(0, 1) == text3)
						{
							array[j] = array[j].Substring(2);
						}
						stringBuilder.Append(array[j]);
					}
					string text4 = stringBuilder.ToString();
					int num2 = text4.IndexOf(text, StringComparison.Ordinal);
					if (num2 >= 0)
					{
						if (cmd.StartsWith("06", StringComparison.Ordinal))
						{
							text4 = text4.Substring(num2 + 2);
						}
						else
						{
							text4 = text4.Substring(num2 + cmd.Length);
						}
					}
					if (string.IsNullOrEmpty(text4))
					{
						flag = false;
					}
					else
					{
						byte[] array2 = BitHelpers.ConvertHexToBytesX(text4);
						if (cmd.StartsWith("06", StringComparison.Ordinal))
						{
							if (this.CurrentMode == OBDDataReader.OBDModes.Mode06)
							{
								flag = this.CurrentCarData.DecodeMode06(cmd, array2, IsCAN);
							}
							else
							{
								flag = this.CurrentCarData.DecodeWithoutRequest(cmd, array2, this.stopwatch.Elapsed, "", header_filter);
							}
						}
					}
				}
			}
			catch (Exception)
			{
				flag = false;
			}
			return flag;
		}

		// Token: 0x0600252E RID: 9518 RVA: 0x001C4F58 File Offset: 0x001C3158
		public async ValueTask<Tuple<bool, byte[]>> DecodeCAN11bitOptimized(string data, OBDMultiRequest request, TimeSpan timeStamp, string override_header = null)
		{
			Tuple<bool, byte[]> tuple;
			if (data == null || data == "" || data.Contains("NO DATA"))
			{
				tuple = new Tuple<bool, byte[]>(false, new byte[0]);
			}
			else
			{
				List<string> lines_with_headers = ListPool<string>.Rent();
				try
				{
					if (this.ELMStatus.STNReceiveSegmentation && data != null && data.Contains("<DATA ERROR"))
					{
						if (request.CheckLength)
						{
							return new Tuple<bool, byte[]>(false, null);
						}
						int num = data.IndexOf("<");
						data = data.Substring(0, num);
					}
					data = OBDDataReader.FilterHexAndNewLineOnly(data);
					string header_filter;
					if (!string.IsNullOrEmpty(this.ELM327_LastSentHeader))
					{
						header_filter = CAN11bitHelper.GetPossibleResponseHeader(this.ELM327_LastSentHeader, SharedSettings.Current.SelectedBrand, request);
					}
					else
					{
						header_filter = this.ECUHeaders[this.SelectedECU].Id;
					}
					if (override_header != null)
					{
						header_filter = override_header;
					}
					string cmd_response = request.ResponseMarker;
					string[] array = data.Split(OBDDataReader.line_splitter, StringSplitOptions.RemoveEmptyEntries);
					bool flag = false;
					int start_searching_response_from_position = 5;
					if (this.ELMStatus.STNReceiveSegmentation)
					{
						request.CheckLength = false;
						start_searching_response_from_position = 3;
					}
					foreach (string text in array)
					{
						int num2 = text.IndexOf(header_filter, StringComparison.Ordinal);
						if (num2 >= 0 && num2 <= 1)
						{
							lines_with_headers.Add(text);
							if (!flag && text.IndexOf(cmd_response, start_searching_response_from_position, StringComparison.Ordinal) >= 0)
							{
								flag = true;
							}
						}
					}
					if (override_header == null && !flag)
					{
						lines_with_headers.Clear();
						List<string> headers = this.GetHeaders(cmd_response, array, request.ELMFormat);
						if (headers.Count > 0)
						{
							header_filter = headers.FirstOrDefault((string x) => x != header_filter);
							if (header_filter == null)
							{
								return new Tuple<bool, byte[]>(false, null);
							}
							foreach (string text2 in array)
							{
								int num3 = text2.IndexOf(header_filter, StringComparison.Ordinal);
								if (num3 >= 0 && num3 <= 1)
								{
									lines_with_headers.Add(text2);
								}
							}
						}
					}
					string text3 = null;
					string text4 = null;
					if (lines_with_headers.Count > 0)
					{
						if (lines_with_headers.Count == 1)
						{
							string text5 = lines_with_headers[0];
							int num4 = text5.IndexOf(cmd_response, start_searching_response_from_position, StringComparison.Ordinal);
							if (num4 >= 0)
							{
								text3 = text5.Substring(num4);
								text4 = text5.Substring(num4 - 2, 2);
							}
						}
						else if (lines_with_headers.Count > 1)
						{
							int num5 = Array.FindIndex<string>(lines_with_headers.ToArray(), (string x) => x.Length > 5 && x.IndexOf(cmd_response, start_searching_response_from_position, StringComparison.Ordinal) >= 0);
							if (num5 >= 0)
							{
								int num6 = lines_with_headers[num5].IndexOf(cmd_response, start_searching_response_from_position, StringComparison.Ordinal);
								if (num6 >= 0)
								{
									StringBuilder stringBuilder = new StringBuilder(lines_with_headers.Count);
									int num7 = 0;
									for (int j = num5; j < lines_with_headers.Count; j++)
									{
										string text6;
										if (j == num5)
										{
											num7++;
											text6 = lines_with_headers[j].Substring(num6);
											text4 = lines_with_headers[j].Substring(num6 - 3, 3);
										}
										else
										{
											string text7 = lines_with_headers[j];
											if (text7[num6 - 4] != '2' || text7[num6 - 3] != num7.ToString("X1")[0])
											{
												break;
											}
											text6 = text7.Substring(num6 - 2);
											num7++;
											if (num7 > 15)
											{
												num7 = 0;
											}
										}
										text6 = this.PrepareResponseLine(text6);
										stringBuilder.Append(text6);
									}
									text3 = stringBuilder.ToString();
								}
							}
						}
						if (text3 != null)
						{
							int num8 = BitHelpers.ConvertHexToInt(text4) * 2;
							int num9 = text3.Length;
							if (text3.Length > num8)
							{
								num9 = num8;
							}
							int num10 = 2;
							num9 -= 2;
							if (text3.Length % 2 != 0)
							{
								num9--;
							}
							if (this.ELMStatus.STNReceiveSegmentation)
							{
								num9 = text3.Length - num10;
							}
							byte[] decoded_bytes = BitHelpers.ConvertHexToBytesX(text3, num10, num9);
							ValueTaskAwaiter<bool> valueTaskAwaiter = this.DecodeMultiResponseCanData(request, timeStamp, header_filter, decoded_bytes).GetAwaiter();
							if (!valueTaskAwaiter.IsCompleted)
							{
								await valueTaskAwaiter;
								ValueTaskAwaiter<bool> valueTaskAwaiter2;
								valueTaskAwaiter = valueTaskAwaiter2;
								valueTaskAwaiter2 = default(ValueTaskAwaiter<bool>);
							}
							return new Tuple<bool, byte[]>(valueTaskAwaiter.GetResult(), decoded_bytes);
						}
					}
				}
				catch (Exception)
				{
				}
				finally
				{
					ListPool<string>.Return(lines_with_headers);
				}
				tuple = new Tuple<bool, byte[]>(false, null);
			}
			return tuple;
		}

		// Token: 0x0600252F RID: 9519 RVA: 0x001C4FBC File Offset: 0x001C31BC
		public async ValueTask<Tuple<bool, byte[]>> DecodeCAN11bit(string data, OBDRequest req, TimeSpan timeStamp, string override_header_filter = null)
		{
			Tuple<bool, byte[]> tuple;
			if (data == null || data == "" || data.Contains("NO DATA"))
			{
				tuple = new Tuple<bool, byte[]>(false, new byte[0]);
			}
			else
			{
				string command = req.Command;
				List<string> lines_with_headers = ListPool<string>.Rent();
				try
				{
					if (this.ELMStatus.STNReceiveSegmentation && data != null && data.Contains("<DATA ERROR"))
					{
						if (req.CheckLength)
						{
							return new Tuple<bool, byte[]>(false, null);
						}
						int num = data.IndexOf("<");
						data = data.Substring(0, num);
					}
					data = OBDDataReader.FilterHexAndNewLineOnly(data);
					string text = null;
					byte[] decoded_bytes = null;
					string header_filter;
					if (!string.IsNullOrEmpty(this.ELM327_LastSentHeader) || (req.BeforeCommands != null && req.BeforeCommands.Length != 0))
					{
						header_filter = CAN11bitHelper.GetPossibleResponseHeader(this.ELM327_LastSentHeader, SharedSettings.Current.SelectedBrand, req);
					}
					else
					{
						header_filter = this.ECUHeaders[this.SelectedECU].Id;
					}
					if (override_header_filter != null)
					{
						header_filter = override_header_filter;
					}
					string text2 = null;
					string cmd_response = req.ResponseMarker;
					if (data.IndexOf(' ') >= 0)
					{
						data = data.Replace(" ", "");
					}
					string[] array = data.Split(OBDDataReader.line_splitter, StringSplitOptions.RemoveEmptyEntries);
					bool flag = false;
					int start_searching_response_from_position = 5;
					if (this.ELMStatus.STNReceiveSegmentation || (command.Length == 6 && command.StartsWith("AA")))
					{
						req.CheckLength = false;
						start_searching_response_from_position = 3;
					}
					foreach (string text3 in array)
					{
						int num2 = text3.IndexOf(header_filter, StringComparison.Ordinal);
						if (num2 >= 0 && num2 <= 1 && !text3.Substring(num2 + 3).StartsWith("037F" + req.Command.Substring(0, 2)))
						{
							lines_with_headers.Add(text3);
							if (!flag && text3.IndexOf(cmd_response, start_searching_response_from_position, StringComparison.Ordinal) >= 0)
							{
								flag = true;
							}
						}
					}
					if (!flag && req.ResponseMarker != null && req.ResponseMarker.Length > 2 && SharedSettings.Current.UseServiceResponseForPositiveResponseMarker)
					{
						string text4 = req.ResponseMarker.Substring(0, 2);
						lines_with_headers.Clear();
						foreach (string text5 in array)
						{
							int num3 = text5.IndexOf(header_filter, StringComparison.Ordinal);
							if (num3 >= 0 && num3 <= 1 && !text5.Substring(num3 + 3).StartsWith("037F" + req.Command.Substring(0, 2)))
							{
								lines_with_headers.Add(text5);
								if (!flag && text5.IndexOf(text4, start_searching_response_from_position, StringComparison.Ordinal) >= 0)
								{
									flag = true;
									cmd_response = text4;
								}
							}
						}
					}
					if (override_header_filter == null && !flag)
					{
						lines_with_headers.Clear();
						List<string> headers = this.GetHeaders(cmd_response, array, req.ELMFormat);
						if (headers.Count > 0)
						{
							header_filter = headers.FirstOrDefault((string x) => x != header_filter);
							if (header_filter == null)
							{
								return new Tuple<bool, byte[]>(false, null);
							}
							foreach (string text6 in array)
							{
								int num4 = text6.IndexOf(header_filter, StringComparison.Ordinal);
								if (num4 >= 0 && num4 <= 1)
								{
									lines_with_headers.Add(text6);
								}
							}
						}
					}
					if (lines_with_headers != null && lines_with_headers.Count > 0)
					{
						if (lines_with_headers.Count == 1)
						{
							string text7 = lines_with_headers[0];
							int num5 = text7.IndexOf(cmd_response, start_searching_response_from_position, StringComparison.Ordinal);
							if (num5 >= 0)
							{
								text = text7.Substring(num5);
								text2 = text7.Substring(num5 - 2, 2);
							}
						}
						else if (lines_with_headers.Count > 1)
						{
							int num6 = lines_with_headers.FindIndex((string x) => x != null && x.Length > 5 && x.IndexOf(cmd_response, start_searching_response_from_position, StringComparison.Ordinal) >= 0);
							if (num6 >= 0)
							{
								int num7 = lines_with_headers[num6].IndexOf(cmd_response, start_searching_response_from_position, StringComparison.Ordinal);
								if (num7 >= 0)
								{
									StringBuilder stringBuilder = new StringBuilder(lines_with_headers.Count);
									int num8 = 0;
									for (int j = num6; j < lines_with_headers.Count; j++)
									{
										string text8;
										if (j == num6)
										{
											num8++;
											text8 = lines_with_headers[j].Substring(num7);
											int num9 = num7 - 3;
											text2 = lines_with_headers[j].Substring(num9, 3);
											if (lines_with_headers[j][num9 - 1] != '1' || ((num9 != 4 || text8.Length != 12) && (num9 != 6 || text8.Length != 10)))
											{
												text2 = lines_with_headers[j].Substring(num9 + 1, 2);
												stringBuilder.Append(text8);
												break;
											}
										}
										else
										{
											string text9 = lines_with_headers[j];
											if (text9[num7 - 4] != '2' || text9[num7 - 3] != num8.ToString("X1")[0])
											{
												break;
											}
											text8 = text9.Substring(num7 - 2);
											num8++;
											if (num8 > 15)
											{
												num8 = 0;
											}
										}
										stringBuilder.Append(text8);
									}
									text = stringBuilder.ToString();
								}
							}
						}
						if (text != null)
						{
							int num10 = BitHelpers.ConvertHexToInt(text2);
							if (this.ELMStatus.STNReceiveSegmentation)
							{
								num10 = text.Length / 2;
							}
							int num11 = num10 * 2;
							if (text.Length > num11)
							{
								text = text.Substring(0, num11);
							}
							if (req.CheckLength && text.Length < num11)
							{
								ELMState elmstatus = this.ELMStatus;
								int i = elmstatus.LostCANMultiframeCounter;
								elmstatus.LostCANMultiframeCounter = i + 1;
								return new Tuple<bool, byte[]>(false, null);
							}
							int num12 = 0;
							int num13 = text.Length;
							if (!command.StartsWith("06", StringComparison.Ordinal))
							{
								if (text.Length > command.Length && !command.StartsWith("2C"))
								{
									if (command.StartsWith("22"))
									{
										num12 = 6;
									}
									else if (command.Length == 6 && command.StartsWith("AA"))
									{
										num12 = cmd_response.Length;
									}
									else if (command.StartsWith("01"))
									{
										num12 = cmd_response.Length;
									}
									else if (command.StartsWith("21") && command.EndsWith("8001"))
									{
										num12 = cmd_response.Length;
									}
									else if (req.OBDMode == OBDDataReader.OBDModes.ReadDTC)
									{
										num12 = req.ResponseMarker.Length;
									}
									else
									{
										num12 = command.Length;
									}
								}
								else if (text.Length > cmd_response.Length)
								{
									num12 = cmd_response.Length;
								}
							}
							num13 = text.Length - num12;
							if (text.Length % 2 != 0)
							{
								num13--;
							}
							decoded_bytes = BitHelpers.ConvertHexToBytesX(text, num12, num13);
							bool flag2;
							if (req.OBDMode == OBDDataReader.OBDModes.ReadDTC)
							{
								flag2 = text != null && text.Length > 0;
								return new Tuple<bool, byte[]>(flag2, decoded_bytes);
							}
							flag2 = await this.CurrentCarData.Decode(command, header_filter, req, decoded_bytes, timeStamp);
							return new Tuple<bool, byte[]>(flag2, decoded_bytes);
						}
					}
					decoded_bytes = null;
				}
				catch
				{
				}
				finally
				{
					ListPool<string>.Return(lines_with_headers);
				}
				tuple = new Tuple<bool, byte[]>(false, null);
			}
			return tuple;
		}

		// Token: 0x06002530 RID: 9520 RVA: 0x001C5020 File Offset: 0x001C3220
		public async ValueTask<Tuple<bool, byte[]>> DecodeCAN29bitOptimized(string data, OBDMultiRequest request, TimeSpan timeStamp, string override_header = null)
		{
			Tuple<bool, byte[]> tuple;
			if (data == null || data == "" || data.Contains("NO DATA"))
			{
				tuple = new Tuple<bool, byte[]>(false, new byte[0]);
			}
			else
			{
				List<string> lines_with_headers = ListPool<string>.Rent();
				try
				{
					if (this.ELMStatus.STNReceiveSegmentation && data != null && data.Contains("<DATA ERROR"))
					{
						if (request.CheckLength)
						{
							return new Tuple<bool, byte[]>(false, null);
						}
						int num = data.IndexOf("<");
						data = data.Substring(0, num);
					}
					data = OBDDataReader.FilterHexAndNewLineOnly(data);
					string[] array = data.Split(OBDDataReader.line_splitter, StringSplitOptions.RemoveEmptyEntries);
					string header_filter;
					if (!string.IsNullOrEmpty(this.ELM327_LastSentHeader))
					{
						header_filter = CAN29bitHelper.GetPossibleResponseHeaderFilter(this.ELM327_LastSentHeader, SharedSettings.Current.BrandForDTC, request);
					}
					else
					{
						header_filter = this.ECUHeaders[this.SelectedECU].Id;
					}
					if (override_header != null)
					{
						header_filter = override_header;
					}
					string key = request.Commands.First<KeyValuePair<string, short>>().Key;
					string responseMarker = request.ResponseMarker;
					bool flag = false;
					int num2 = 10;
					if (this.ELMStatus.STNReceiveSegmentation)
					{
						request.CheckLength = false;
						num2 = 8;
					}
					foreach (string text in array)
					{
						if (text.Length >= 8 && text.Substring(6, 2) == header_filter)
						{
							lines_with_headers.Add(text);
							if (text.IndexOf(responseMarker, num2, StringComparison.Ordinal) >= 0)
							{
								flag = true;
							}
						}
						else if (text.Length >= 8 && header_filter == "??" && text.IndexOf(responseMarker, num2, StringComparison.Ordinal) >= 0)
						{
							flag = true;
							header_filter = text.Substring(6, 2);
							lines_with_headers.Add(text);
						}
					}
					if (header_filter == null && !flag)
					{
						lines_with_headers.Clear();
						List<string> headers = this.GetHeaders(responseMarker, array, request.ELMFormat);
						if (headers.Count > 0)
						{
							header_filter = headers.FirstOrDefault((string x) => x != header_filter);
							if (header_filter == null)
							{
								return new Tuple<bool, byte[]>(false, null);
							}
							foreach (string text2 in array)
							{
								if (text2.Length >= 8 && text2.Substring(6, 2) == header_filter)
								{
									lines_with_headers.Add(text2);
								}
							}
						}
					}
					string text3 = null;
					string text4 = null;
					if (lines_with_headers.Count > 0)
					{
						if (lines_with_headers.Count == 1)
						{
							string text5 = lines_with_headers[0];
							int num3 = text5.IndexOf(responseMarker, num2, StringComparison.Ordinal);
							if (num3 >= 0)
							{
								text3 = text5.Substring(num3);
								text4 = text5.Substring(num3 - 2, 2);
							}
						}
						else if (lines_with_headers.Count > 1)
						{
							int num4 = lines_with_headers[0].IndexOf(responseMarker, num2, StringComparison.Ordinal);
							if (num4 >= 0)
							{
								StringBuilder stringBuilder = new StringBuilder(lines_with_headers.Count);
								for (int j = 0; j < lines_with_headers.Count; j++)
								{
									string text6;
									if (j == 0)
									{
										text6 = lines_with_headers[j].Substring(num4);
										text4 = lines_with_headers[j].Substring(num4 - 2, 2);
									}
									else
									{
										text6 = lines_with_headers[j].Substring(num4 - 2);
									}
									text6 = this.PrepareResponseLine(text6);
									stringBuilder.Append(text6);
								}
								text3 = stringBuilder.ToString();
							}
						}
						if (text3 != null)
						{
							int num5 = BitHelpers.ConvertHexToInt(text4) * 2;
							if (text3.Length > num5 && !this.ELMStatus.STNReceiveSegmentation)
							{
								text3 = text3.Substring(0, num5);
							}
							text3 = text3.Substring(2);
							if (text3.Length % 2 != 0)
							{
								text3 = text3.Substring(0, text3.Length - 1);
							}
							byte[] decoded_bytes = BitHelpers.ConvertHexToBytesX(text3);
							ValueTaskAwaiter<bool> valueTaskAwaiter = this.DecodeMultiResponseCanData(request, timeStamp, header_filter, decoded_bytes).GetAwaiter();
							if (!valueTaskAwaiter.IsCompleted)
							{
								await valueTaskAwaiter;
								ValueTaskAwaiter<bool> valueTaskAwaiter2;
								valueTaskAwaiter = valueTaskAwaiter2;
								valueTaskAwaiter2 = default(ValueTaskAwaiter<bool>);
							}
							return new Tuple<bool, byte[]>(valueTaskAwaiter.GetResult(), decoded_bytes);
						}
					}
				}
				catch
				{
				}
				finally
				{
					ListPool<string>.Return(lines_with_headers);
				}
				tuple = new Tuple<bool, byte[]>(false, null);
			}
			return tuple;
		}

		// Token: 0x06002531 RID: 9521 RVA: 0x001C5084 File Offset: 0x001C3284
		private async ValueTask<bool> DecodeMultiResponseCanData(OBDMultiRequest request, TimeSpan timeStamp, string header_filter, byte[] decoded_bytes)
		{
			bool result = true;
			MemoryStream ms = new MemoryStream(decoded_bytes);
			int counter = 0;
			if (request.Command.StartsWith("01"))
			{
				while (ms.CanRead && ms.Position < ms.Length)
				{
					if (counter >= request.Commands.Count)
					{
						break;
					}
					string text = "01" + ms.ReadByte().ToString("X2", CultureInfo.InvariantCulture);
					int num;
					if (!request.Commands.ContainsKey(text))
					{
						SharedSettings sharedSettings = SharedSettings.Current;
						num = sharedSettings.OptimizedRequestStuckCounter;
						sharedSettings.OptimizedRequestStuckCounter = num + 1;
						return 0;
					}
					int num2 = (int)request.Commands[text];
					byte[] array = new byte[num2];
					if (ms.Length - ms.Position < (long)num2)
					{
						break;
					}
					ms.Read(array, 0, num2);
					ValueTaskAwaiter<bool> valueTaskAwaiter = this.CurrentCarData.Decode(text, header_filter, request, array, timeStamp).GetAwaiter();
					if (!valueTaskAwaiter.IsCompleted)
					{
						await valueTaskAwaiter;
						ValueTaskAwaiter<bool> valueTaskAwaiter2;
						valueTaskAwaiter = valueTaskAwaiter2;
						valueTaskAwaiter2 = default(ValueTaskAwaiter<bool>);
					}
					if (!valueTaskAwaiter.GetResult())
					{
						result = false;
						break;
					}
					num = counter;
					counter = num + 1;
				}
			}
			else if (request.Command.StartsWith("22"))
			{
				while (ms.CanRead && ms.Position < ms.Length && counter < request.Commands.Count)
				{
					int num3 = ms.ReadByte();
					int num4 = ms.ReadByte();
					string text2 = "22" + num3.ToString("X2", CultureInfo.InvariantCulture) + num4.ToString("X2", CultureInfo.InvariantCulture);
					int num;
					if (!request.Commands.ContainsKey(text2))
					{
						SharedSettings sharedSettings2 = SharedSettings.Current;
						num = sharedSettings2.OptimizedRequestStuckCounter;
						sharedSettings2.OptimizedRequestStuckCounter = num + 1;
						return 0;
					}
					int num5 = (int)request.Commands[text2];
					byte[] array2 = new byte[num5];
					if (ms.Length - ms.Position < (long)num5)
					{
						break;
					}
					ms.Read(array2, 0, num5);
					ValueTaskAwaiter<bool> valueTaskAwaiter = this.CurrentCarData.Decode(text2, header_filter, request, array2, timeStamp).GetAwaiter();
					if (!valueTaskAwaiter.IsCompleted)
					{
						await valueTaskAwaiter;
						ValueTaskAwaiter<bool> valueTaskAwaiter2;
						valueTaskAwaiter = valueTaskAwaiter2;
						valueTaskAwaiter2 = default(ValueTaskAwaiter<bool>);
					}
					if (!valueTaskAwaiter.GetResult())
					{
						result = false;
						break;
					}
					num = counter;
					counter = num + 1;
				}
			}
			return result;
		}

		// Token: 0x06002532 RID: 9522 RVA: 0x001C50E8 File Offset: 0x001C32E8
		public async ValueTask<Tuple<bool, byte[]>> DecodeCAN29bit(string data, OBDRequest req, TimeSpan timeStamp, string override_header)
		{
			Tuple<bool, byte[]> tuple;
			if (data == null || data == "" || data.Contains("NO DATA"))
			{
				tuple = new Tuple<bool, byte[]>(false, new byte[0]);
			}
			else
			{
				string command = req.Command;
				List<string> lines_with_headers = ListPool<string>.Rent();
				try
				{
					if (this.ELMStatus.STNReceiveSegmentation && data != null && data.Contains("<DATA ERROR"))
					{
						if (req.CheckLength)
						{
							return new Tuple<bool, byte[]>(false, null);
						}
						int num = data.IndexOf("<");
						data = data.Substring(0, num);
					}
					data = OBDDataReader.FilterHexAndNewLineOnly(data);
					string header_filter;
					if (!string.IsNullOrEmpty(this.ELM327_LastSentHeader))
					{
						header_filter = CAN29bitHelper.GetPossibleResponseHeaderFilter(this.ELM327_LastSentHeader, SharedSettings.Current.BrandForDTC, req);
					}
					else
					{
						header_filter = this.ECUHeaders[this.SelectedECU].Id;
					}
					if (override_header != null)
					{
						header_filter = override_header;
					}
					string text = req.ResponseMarker;
					string[] array = data.Split(OBDDataReader.line_splitter, StringSplitOptions.RemoveEmptyEntries);
					bool flag = false;
					int num2 = 10;
					if (this.ELMStatus.STNReceiveSegmentation)
					{
						req.CheckLength = false;
						num2 = 8;
					}
					foreach (string text2 in array)
					{
						if (text2.Length >= 8 && text2.Substring(6, 2) == header_filter)
						{
							if (!flag && text2.IndexOf(text, num2, StringComparison.Ordinal) >= 0)
							{
								flag = true;
							}
							if (flag)
							{
								lines_with_headers.Add(text2);
							}
						}
						else if (text2.Length >= 8 && header_filter == "??" && text2.IndexOf(text, num2, StringComparison.Ordinal) >= 0)
						{
							flag = true;
							header_filter = text2.Substring(6, 2);
							lines_with_headers.Add(text2);
						}
					}
					if (!flag && req.ResponseMarker != null && req.ResponseMarker.Length > 2 && SharedSettings.Current.UseServiceResponseForPositiveResponseMarker)
					{
						string text3 = req.ResponseMarker.Substring(0, 2);
						lines_with_headers.Clear();
						foreach (string text4 in array)
						{
							if (text4.Length >= 8 && text4.Substring(6, 2) == header_filter)
							{
								if (!flag && text4.IndexOf(text3, num2, StringComparison.Ordinal) >= 0)
								{
									text = text3;
									flag = true;
								}
								if (flag)
								{
									lines_with_headers.Add(text4);
								}
							}
						}
					}
					string text5 = null;
					string text6 = null;
					if (override_header == null && !flag)
					{
						lines_with_headers.Clear();
						List<string> headers = this.GetHeaders(text, array, req.ELMFormat);
						if (headers.Count > 0)
						{
							header_filter = headers.FirstOrDefault((string x) => x != header_filter);
							if (header_filter == null)
							{
								return new Tuple<bool, byte[]>(false, null);
							}
							foreach (string text7 in array)
							{
								if (!flag && text7.IndexOf(text, num2, StringComparison.Ordinal) >= 0)
								{
									flag = true;
								}
								if (flag && text7.Length >= 8 && text7.Substring(6, 2) == header_filter)
								{
									lines_with_headers.Add(text7);
								}
							}
						}
					}
					if (lines_with_headers != null && lines_with_headers.Count > 0)
					{
						if (lines_with_headers.Count == 1)
						{
							string text8 = lines_with_headers[0];
							int num3 = text8.IndexOf(text, num2, StringComparison.Ordinal);
							if (num3 >= 0)
							{
								text6 = text8.Substring(num3);
								text5 = text8.Substring(num3 - 2, 2);
							}
						}
						else if (lines_with_headers.Count > 1)
						{
							int num4 = lines_with_headers[0].IndexOf(text, num2, StringComparison.Ordinal);
							if (num4 >= 0)
							{
								StringBuilder stringBuilder = new StringBuilder(lines_with_headers.Count);
								for (int j = 0; j < lines_with_headers.Count; j++)
								{
									string text9;
									if (j == 0)
									{
										text9 = lines_with_headers[j].Substring(num4);
										text5 = lines_with_headers[j].Substring(num4 - 3, 3);
									}
									else
									{
										text9 = lines_with_headers[j].Substring(num4 - 2);
									}
									text9 = this.PrepareResponseLine(text9);
									stringBuilder.Append(text9);
								}
								text6 = stringBuilder.ToString();
							}
						}
						if (text6 != null)
						{
							int num5 = BitHelpers.ConvertHexToInt(text5) * 2;
							if (this.ELMStatus.STNReceiveSegmentation)
							{
								num5 = text6.Length;
							}
							if (text6.Length > num5)
							{
								text6 = text6.Substring(0, num5);
							}
							if (!command.StartsWith("06", StringComparison.Ordinal))
							{
								text6 = text6.Substring(text.Length);
							}
							if (text6.Length % 2 != 0)
							{
								text6 = text6.Substring(0, text6.Length - 1);
							}
							byte[] decoded_bytes = BitHelpers.ConvertHexToBytesX(text6);
							ValueTaskAwaiter<bool> valueTaskAwaiter = this.CurrentCarData.Decode(command, header_filter, req, decoded_bytes, timeStamp).GetAwaiter();
							if (!valueTaskAwaiter.IsCompleted)
							{
								await valueTaskAwaiter;
								ValueTaskAwaiter<bool> valueTaskAwaiter2;
								valueTaskAwaiter = valueTaskAwaiter2;
								valueTaskAwaiter2 = default(ValueTaskAwaiter<bool>);
							}
							return new Tuple<bool, byte[]>(valueTaskAwaiter.GetResult(), decoded_bytes);
						}
					}
				}
				catch (Exception)
				{
				}
				finally
				{
					ListPool<string>.Return(lines_with_headers);
				}
				tuple = new Tuple<bool, byte[]>(false, null);
			}
			return tuple;
		}

		// Token: 0x06002533 RID: 9523 RVA: 0x000027D4 File Offset: 0x000009D4
		public void DebugWithStopWatch(string str)
		{
		}

		// Token: 0x06002534 RID: 9524 RVA: 0x001C514C File Offset: 0x001C334C
		public async ValueTask<Tuple<bool, byte[]>> DecodeKWP(string data, OBDRequest request, TimeSpan timeStamp, string override_header = null)
		{
			Tuple<bool, byte[]> tuple;
			if (data == null || data == "" || data.Contains("NO DATA"))
			{
				tuple = new Tuple<bool, byte[]>(false, new byte[0]);
			}
			else
			{
				string command = request.Command;
				List<string> lines_with_headers = ListPool<string>.Rent();
				try
				{
					data = OBDDataReader.FilterHexAndNewLineOnly(data);
					string header_filter;
					if (!string.IsNullOrEmpty(this.ELM327_LastSentHeader) && this.ELM327_LastSentHeader.Length >= 6)
					{
						header_filter = this.ELM327_LastSentHeader.Substring(2, 2);
					}
					else
					{
						header_filter = this.ECUHeaders[this.SelectedECU].Id;
					}
					if (override_header != null)
					{
						header_filter = override_header;
					}
					string cmd_response = request.ResponseMarker;
					if (SharedSettings.Current.DaihatsuKLine && cmd_response.Length == 6)
					{
						cmd_response = cmd_response.Substring(0, 4);
					}
					if (data.IndexOf(' ') >= 0)
					{
						data = data.Replace(" ", "");
					}
					string[] array = data.Split(OBDDataReader.line_splitter, StringSplitOptions.RemoveEmptyEntries);
					if (array.Length > 4 && SharedSettings.Current.KWPConcatResponseLines)
					{
						StringBuilder stringBuilder = new StringBuilder(array.Length);
						foreach (string text in array)
						{
							if (text.Length >= 2)
							{
								stringBuilder.Append(text);
							}
						}
						array = new string[] { stringBuilder.ToString() };
					}
					bool flag = false;
					foreach (string text2 in array)
					{
						if (text2.Length >= 6 && text2.Substring(4, 2) == header_filter)
						{
							lines_with_headers.Add(text2);
							if (!flag && text2.IndexOf(cmd_response, 6, StringComparison.Ordinal) >= 0)
							{
								flag = true;
							}
						}
					}
					if (!command.StartsWith(SharedSettings.Current.Mode01Prefix) && lines_with_headers.Count == 1 && array.Length >= 2)
					{
						lines_with_headers = new List<string>(1) { data.Replace("\n", "").Replace("\r", "") };
					}
					if (override_header == null && !flag)
					{
						lines_with_headers.Clear();
						List<string> headers = this.GetHeaders(cmd_response, array, request.ELMFormat);
						if (headers.Count > 0)
						{
							header_filter = headers.FirstOrDefault((string x) => x != header_filter);
							if (header_filter == null)
							{
								return new Tuple<bool, byte[]>(false, null);
							}
							foreach (string text3 in array)
							{
								if (text3.Substring(4, 2) == header_filter)
								{
									lines_with_headers.Add(text3);
								}
							}
						}
					}
					if ((this.CurrentProtocolNumber == 1 || this.CurrentProtocolNumber == 2) && !flag && command.Length >= 2 && lines_with_headers.Count == 0)
					{
						foreach (string text4 in array)
						{
							if (text4.Length >= 8 && text4[6] == command[0] && text4[7] == command[1])
							{
								lines_with_headers.Add(text4);
								cmd_response = command.Substring(0, 2);
							}
						}
					}
					if (lines_with_headers != null && lines_with_headers.Count > 0)
					{
						if (SharedSettings.Current.DecodeMUT2Compatible && command.Length == 4 && !command.StartsWith("0", StringComparison.Ordinal) && command.StartsWith("A", StringComparison.Ordinal))
						{
							cmd_response = cmd_response.Substring(0, 2);
						}
						string text5 = null;
						if (lines_with_headers.Count > 1)
						{
							lines_with_headers = lines_with_headers.SkipWhile((string x) => x.IndexOf(cmd_response, 6, StringComparison.Ordinal) < 0).ToList<string>();
							for (int j = 0; j < lines_with_headers.Count; j++)
							{
								string text6 = lines_with_headers[j];
								int num = text6.IndexOf(cmd_response, 6, StringComparison.Ordinal);
								if (num >= 0)
								{
									if (BitHelpers.GetCheckSummHex(text6.Substring(0, text6.Length - 2)) == text6.Substring(text6.Length - 2))
									{
										text6 = text6.Substring(0, text6.Length - 2);
									}
									lines_with_headers[j] = text6.Substring(num + cmd_response.Length);
								}
							}
							StringBuilder stringBuilder2 = new StringBuilder(lines_with_headers.Count);
							foreach (string text7 in lines_with_headers)
							{
								stringBuilder2.Append(text7);
							}
							text5 = stringBuilder2.ToString();
						}
						else
						{
							int num2 = lines_with_headers[0].IndexOf(cmd_response, 6, StringComparison.Ordinal);
							if (num2 > 0)
							{
								text5 = lines_with_headers[0].Substring(num2 + request.ResponseMarker.Length);
							}
						}
						if (text5 != null)
						{
							byte[] decoded_bytes = BitHelpers.ConvertHexToBytesX(text5);
							ValueTaskAwaiter<bool> valueTaskAwaiter = this.CurrentCarData.Decode(command, header_filter, request, decoded_bytes, timeStamp).GetAwaiter();
							if (!valueTaskAwaiter.IsCompleted)
							{
								await valueTaskAwaiter;
								ValueTaskAwaiter<bool> valueTaskAwaiter2;
								valueTaskAwaiter = valueTaskAwaiter2;
								valueTaskAwaiter2 = default(ValueTaskAwaiter<bool>);
							}
							return new Tuple<bool, byte[]>(valueTaskAwaiter.GetResult(), decoded_bytes);
						}
					}
				}
				catch (Exception)
				{
				}
				finally
				{
					ListPool<string>.Return(lines_with_headers);
				}
				tuple = new Tuple<bool, byte[]>(false, null);
			}
			return tuple;
		}

		// Token: 0x06002535 RID: 9525 RVA: 0x001C51B0 File Offset: 0x001C33B0
		private ELMFormat GetELMFormatFromBeforeCommands(string[] commands)
		{
			if (commands == null)
			{
				return ELMFormat.Unknown;
			}
			foreach (string text in commands)
			{
				if (text != null && !(text == ""))
				{
					string text2 = text.ToUpperInvariant().Replace(" ", "").Trim();
					if (text2 != null)
					{
						int length = text2.Length;
						if (length == 5)
						{
							switch (text2[4])
							{
							case '1':
								if (!(text2 == "ATSP1"))
								{
									goto IL_015B;
								}
								return ELMFormat.KWP;
							case '2':
								if (!(text2 == "ATSP2"))
								{
									goto IL_015B;
								}
								return ELMFormat.KWP;
							case '3':
								if (!(text2 == "ATSP3"))
								{
									goto IL_015B;
								}
								return ELMFormat.KWP;
							case '4':
								if (!(text2 == "ATSP4"))
								{
									goto IL_015B;
								}
								return ELMFormat.KWP;
							case '5':
								if (!(text2 == "ATSP5"))
								{
									goto IL_015B;
								}
								return ELMFormat.KWP;
							case '6':
								if (!(text2 == "ATSP6"))
								{
									goto IL_015B;
								}
								break;
							case '7':
								if (!(text2 == "ATSP7"))
								{
									goto IL_015B;
								}
								return ELMFormat.CAN29bit;
							case '8':
								if (!(text2 == "ATSP8"))
								{
									goto IL_015B;
								}
								break;
							case '9':
								if (!(text2 == "ATSP9"))
								{
									goto IL_015B;
								}
								return ELMFormat.CAN29bit;
							case ':':
							case ';':
							case '<':
							case '=':
							case '>':
							case '?':
							case '@':
							case 'A':
								goto IL_015B;
							case 'B':
								if (!(text2 == "ATSPB"))
								{
									goto IL_015B;
								}
								break;
							default:
								goto IL_015B;
							}
							return ELMFormat.CAN11bit;
						}
					}
				}
				IL_015B:;
			}
			return ELMFormat.Unknown;
		}

		// Token: 0x06002536 RID: 9526 RVA: 0x001C5328 File Offset: 0x001C3528
		public async ValueTask<bool> DecodeData(string data, OBDRequest request, string override_header = null)
		{
			TimeSpan elapsed = this.stopwatch.Elapsed;
			string command = request.Command;
			if (request.ELMFormat == ELMFormat.Unknown)
			{
				if (request.BeforeCommands != null && request.BeforeCommands.Length != 0)
				{
					ELMFormat elmformatFromBeforeCommands = this.GetELMFormatFromBeforeCommands(request.BeforeCommands);
					if (elmformatFromBeforeCommands == ELMFormat.Unknown)
					{
						request.ELMFormat = this.CurrentELMFormat;
					}
					else
					{
						request.ELMFormat = elmformatFromBeforeCommands;
					}
				}
				else
				{
					request.ELMFormat = this.CurrentELMFormat;
				}
			}
			Tuple<bool, byte[]> tuple;
			switch (request.ELMFormat)
			{
			case ELMFormat.Unknown:
				tuple = new Tuple<bool, byte[]>(false, null);
				goto IL_0395;
			case ELMFormat.CAN11bit:
				if (request is OBDMultiRequest && SharedSettings.Current.CANOptimizeRequests)
				{
					tuple = await this.DecodeCAN11bitOptimized(data, request as OBDMultiRequest, elapsed, override_header);
					goto IL_0395;
				}
				tuple = await this.DecodeCAN11bit(data, request, elapsed, override_header);
				goto IL_0395;
			case ELMFormat.CAN29bit:
				if (request is OBDMultiRequest && SharedSettings.Current.CANOptimizeRequests)
				{
					tuple = await this.DecodeCAN29bitOptimized(data, request as OBDMultiRequest, elapsed, override_header);
					goto IL_0395;
				}
				tuple = await this.DecodeCAN29bit(data, request, elapsed, override_header);
				goto IL_0395;
			}
			tuple = await this.DecodeKWP(data, request, elapsed, override_header);
			IL_0395:
			try
			{
				request.OnResponseDecoded(tuple.Item2, tuple.Item1, "");
			}
			catch (Exception)
			{
			}
			return tuple.Item1;
		}

		// Token: 0x06002537 RID: 9527 RVA: 0x001C5384 File Offset: 0x001C3584
		private bool ParseATRV(OBDRequest request, string data)
		{
			OBDDataReader.<>c__DisplayClass197_0 CS$<>8__locals1 = new OBDDataReader.<>c__DisplayClass197_0();
			CS$<>8__locals1.request = request;
			CS$<>8__locals1.<>4__this = this;
			bool flag = false;
			try
			{
				int num = data.IndexOf('.');
				int num2 = data.IndexOf('V');
				OBDDataReader.<>c__DisplayClass197_0 CS$<>8__locals2 = CS$<>8__locals1;
				byte[] array = new byte[3];
				array[0] = byte.Parse(data.Substring(0, num));
				array[1] = byte.Parse(data[num + 1].ToString(CultureInfo.InvariantCulture), CultureInfo.InvariantCulture);
				CS$<>8__locals2.bytes = array;
				if (num2 > num + 2)
				{
					CS$<>8__locals1.bytes[2] = byte.Parse(data[num + 2].ToString(CultureInfo.InvariantCulture), CultureInfo.InvariantCulture);
				}
				Device.BeginInvokeOnMainThread(delegate
				{
					try
					{
						CS$<>8__locals1.request.PIDs[0].Decode(CS$<>8__locals1.bytes, CS$<>8__locals1.<>4__this.stopwatch.Elapsed, CS$<>8__locals1.<>4__this.ECUHeaders[CS$<>8__locals1.<>4__this.SelectedECU].Id);
					}
					catch
					{
					}
				});
				flag = true;
			}
			catch
			{
				flag = false;
			}
			return flag;
		}

		// Token: 0x06002538 RID: 9528 RVA: 0x001C5454 File Offset: 0x001C3654
		public async Task Disconnect(string message)
		{
			this.CurrentStatus = OBDDataReaderStatus.Disconnecting;
			this.DisconnectRequested = true;
			this.BadELM = false;
			try
			{
				await this.ClearRequestQueue();
				await Task.Delay(250);
				await this.Stop("Disconnect:" + message);
				await Task.Delay(500);
				if (this.Connection != null)
				{
					this.Connection.Disconect();
					this.Connection = null;
				}
			}
			catch (Exception)
			{
			}
			this.CurrentStatus = OBDDataReaderStatus.Disconnected;
			try
			{
				this.SelectedECU = 0;
				this.InvokeOnMainThread(delegate
				{
					this.ECUHeaders.Clear();
				});
			}
			catch (Exception)
			{
			}
		}

		// Token: 0x06002539 RID: 9529 RVA: 0x001C54A0 File Offset: 0x001C36A0
		public async Task ReadFreezeFrame(int FreezeFrameNumber = 0)
		{
			this.CurrentCarData.FreezeFrameNumber = FreezeFrameNumber;
			this.CurrentCarData.CreateMode02PIDs();
			await this.ClearRequestQueue();
			List<OBDRequest> requests = new List<OBDRequest>();
			for (int i = 512; i <= 672; i += 32)
			{
				OBDRequest obdrequest = new OBDRequest(i.ToString("X4") + FreezeFrameNumber.ToString("X2"), false);
				this.PreprocessRequest(obdrequest);
				requests.Add(obdrequest);
			}
			this.ReplaceQueue(requests);
			await this.WaitForCommandQueue();
			requests.Clear();
			foreach (PID pid in this.CurrentCarData.Mode02PIDs.Where((PID x) => x.IsAvailable && !(x is PID_SupportedPids)))
			{
				requests.Add(new OBDRequest("02" + pid.Command.Substring(2) + this.CurrentCarData.FreezeFrameNumber.ToString("X2", CultureInfo.InvariantCulture), false));
			}
			this.ReplaceQueue(requests);
			await this.WaitForCommandQueue();
		}

		// Token: 0x0600253A RID: 9530 RVA: 0x001C54EC File Offset: 0x001C36EC
		public async Task PerformAlternativePIDTest()
		{
			this.CurrentMode = OBDDataReader.OBDModes.Mode06;
			this.CurrentCarData.ShouldSetAsAvailable = true;
			foreach (PID pid in this.CurrentCarData.LiveDataPIDs)
			{
				if (!(pid is CalculatedPIDV2) && !(pid is SensorPID))
				{
					this.AddRequestToQueue(pid.Command);
				}
			}
			await this.WaitForCommandQueue();
			await Task.Delay(500);
			this.CurrentMode = OBDDataReader.OBDModes.Universal;
			this.CurrentCarData.ShouldSetAsAvailable = false;
		}

		// Token: 0x0600253B RID: 9531 RVA: 0x001C5530 File Offset: 0x001C3730
		public async Task ReadMode06(IProgress<double> progress)
		{
			this.CurrentMode = OBDDataReader.OBDModes.Mode06;
			await this.ClearRequestQueue();
			await MainThread.InvokeOnMainThreadAsync(delegate
			{
				this.CurrentCarData.Mode06TestCollection.Clear();
			});
			for (int i = 0; i < 255; i++)
			{
				if (i % 32 != 0)
				{
					this.AddRequestToQueue("06" + i.ToString("X2", CultureInfo.InvariantCulture));
				}
			}
			while (this.CommandQueue.Count > 0)
			{
				double p = (double)(243 - this.CommandQueue.Count);
				if (p < 0.0)
				{
					p = 0.0;
				}
				if (p > 243.0)
				{
					p = 243.0;
				}
				p /= 243.0;
				Device.BeginInvokeOnMainThread(delegate
				{
					progress.Report(p);
				});
				await Task.Delay(50);
			}
			await this.WaitForCommandQueue();
			this.CurrentMode = OBDDataReader.OBDModes.Universal;
		}

		// Token: 0x0600253C RID: 9532 RVA: 0x001C557C File Offset: 0x001C377C
		public async Task ReadMode06(IProgress<double> progress, string[] pids)
		{
			this.CurrentMode = OBDDataReader.OBDModes.Mode06;
			await this.ClearRequestQueue();
			await MainThread.InvokeOnMainThreadAsync(delegate
			{
				this.CurrentCarData.Mode06TestCollection.Clear();
			});
			for (int i = 0; i < pids.Length; i++)
			{
				this.AddRequestToQueue(pids[i]);
			}
			while (this.CommandQueue.Count > 0)
			{
				double p = (double)(pids.Length - this.CommandQueue.Count);
				if (p < 0.0)
				{
					p = 0.0;
				}
				if (p > (double)pids.Length)
				{
					p = (double)pids.Length;
				}
				p /= (double)pids.Length;
				MainThread.BeginInvokeOnMainThread(delegate
				{
					progress.Report(p);
				});
				await Task.Delay(50);
			}
			await this.WaitForCommandQueue();
			this.CurrentMode = OBDDataReader.OBDModes.Universal;
		}

		// Token: 0x0600253D RID: 9533 RVA: 0x001C55D0 File Offset: 0x001C37D0
		public bool DecodeDTC_CAN11bitV2(string data, OBDRequest req, TimeSpan timeStamp)
		{
			if (this.ELMStatus.STNReceiveSegmentation && data != null && data.Contains("<DATA ERROR"))
			{
				if (req.CheckLength)
				{
					return false;
				}
				int num = data.IndexOf("<");
				data = data.Substring(0, num);
			}
			string command = req.Command;
			bool flag = false;
			string[] array = (from x in data.Replace(" ", string.Empty).Split(OBDDataReader.line_splitter, StringSplitOptions.RemoveEmptyEntries)
				select x.Trim()).ToArray<string>();
			foreach (string text in this.GetHeaders(req.ResponseMarker, array, req.ELMFormat))
			{
				bool flag2;
				byte[] array2;
				this.DecodeCAN11bit(data, req, timeStamp, text).AsTask().Result.Deconstruct(out flag2, out array2);
				byte[] array3 = array2;
				bool flag3 = this.CurrentCarData.DecodeDTC(command, req, text, array3, timeStamp);
				if (flag3)
				{
					flag = true;
				}
				try
				{
					req.OnResponseDecoded(array3, flag3, text);
				}
				catch (Exception)
				{
				}
			}
			return flag;
		}

		// Token: 0x0600253E RID: 9534 RVA: 0x001C5718 File Offset: 0x001C3918
		public bool DecodeDTC_CAN11bit(string data, OBDRequest req, TimeSpan timeStamp)
		{
			if (SharedSettings.Current.ShowExperimental)
			{
				return this.DecodeDTC_CAN11bitV2(data, req, timeStamp);
			}
			if (this.ELMStatus.STNReceiveSegmentation && data != null && data.Contains("<DATA ERROR"))
			{
				if (req.CheckLength)
				{
					return false;
				}
				int num = data.IndexOf("<");
				data = data.Substring(0, num);
			}
			string command = req.Command;
			bool flag = false;
			string[] array = (from x in data.Replace(" ", string.Empty).Split(OBDDataReader.line_splitter, StringSplitOptions.RemoveEmptyEntries)
				select x.Trim()).ToArray<string>();
			using (List<string>.Enumerator enumerator = this.GetHeaders(req.ResponseMarker, array, req.ELMFormat).GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					string header_filter = enumerator.Current;
					try
					{
						string cmd_response = req.ResponseMarker;
						string[] array2 = array.Where((string x) => !this.LineIsNegativeCAN11bitResponse(x)).Where(delegate(string x)
						{
							int num8 = x.IndexOf(header_filter, StringComparison.Ordinal);
							return num8 >= 0 && num8 <= 1;
						}).ToArray<string>();
						string text = null;
						string text2 = null;
						int start_searching_response_from_position = 5;
						if (this.ELMStatus.STNReceiveSegmentation)
						{
							req.CheckLength = false;
							start_searching_response_from_position = 3;
						}
						if (array2 != null && array2.Length != 0)
						{
							int num2 = Array.FindIndex<string>(array2, (string x) => x.Length > start_searching_response_from_position && x.IndexOf(cmd_response, start_searching_response_from_position, StringComparison.Ordinal) >= 0);
							if (num2 > 0)
							{
								array2 = array2.Skip(num2).ToArray<string>();
							}
							if (array2.Length == 1)
							{
								string text3 = array2.First<string>();
								int num3 = text3.IndexOf(cmd_response, start_searching_response_from_position, StringComparison.Ordinal);
								if (num3 >= 0)
								{
									text = text3.Substring(num3);
									text = this.PrepareResponseLine(text);
									if (num3 > 5 && num3 - 4 == 49)
									{
										text2 = text3.Substring(num3 - 3, 3);
									}
									else
									{
										text2 = text3.Substring(num3 - 2, 2);
									}
								}
							}
							else if (array2.Length > 1)
							{
								num2 = Array.FindIndex<string>(array2, (string x) => x.Length > start_searching_response_from_position && x.IndexOf(cmd_response, 5, StringComparison.Ordinal) >= 0);
								if (num2 >= 0)
								{
									int num4 = array2[num2].IndexOf(cmd_response, start_searching_response_from_position, StringComparison.Ordinal);
									if (num4 >= 0)
									{
										StringBuilder stringBuilder = new StringBuilder(array2.Length);
										int num5 = 0;
										for (int i = num2; i < array2.Length; i++)
										{
											string text4;
											if (i == num2)
											{
												num5++;
												text4 = array2[i].Substring(num4);
												text2 = array2[i].Substring(num4 - 3, 3);
											}
											else
											{
												string text5 = array2[i];
												if (text5[num4 - 4] != '2' || text5[num4 - 3] != num5.ToString("X1")[0])
												{
													break;
												}
												text4 = text5.Substring(num4 - 2);
												num5++;
												if (num5 > 15)
												{
													num5 = 0;
												}
											}
											text4 = this.PrepareResponseLine(text4);
											stringBuilder.Append(text4);
										}
										text = stringBuilder.ToString();
									}
								}
							}
							if (text != null)
							{
								int num6 = BitHelpers.ConvertHexToInt(text2);
								int num7 = num6 * 2;
								if (num6 > 0 && text.Length > num7 && !this.ELMStatus.STNReceiveSegmentation)
								{
									text = text.Substring(0, num6 * 2);
								}
								text = text.Substring(req.ResponseMarker.Length);
								if (text.Length % 2 != 0)
								{
									text = text.Substring(0, text.Length - 1);
								}
								byte[] array3 = BitHelpers.ConvertHexToBytesX(text);
								flag = this.CurrentCarData.DecodeDTC(command, req, header_filter, array3, timeStamp);
								try
								{
									req.OnResponseDecoded(array3, flag, header_filter);
								}
								catch (Exception)
								{
								}
							}
						}
					}
					catch
					{
					}
				}
			}
			return flag;
		}

		// Token: 0x0600253F RID: 9535 RVA: 0x001C5B3C File Offset: 0x001C3D3C
		public bool DecodeDTC_CAN29bit(string data, OBDRequest req, TimeSpan timeStamp)
		{
			if (this.ELMStatus.STNReceiveSegmentation && data != null && data.Contains("<DATA ERROR"))
			{
				if (req.CheckLength)
				{
					return false;
				}
				int num = data.IndexOf("<");
				data = data.Substring(0, num);
			}
			string command = req.Command;
			bool flag = false;
			string[] array = (from x in data.Replace(" ", string.Empty).Split(OBDDataReader.line_splitter, StringSplitOptions.RemoveEmptyEntries)
				select x.Trim()).ToArray<string>();
			using (List<string>.Enumerator enumerator = this.GetHeaders(req.ResponseMarker, array, req.ELMFormat).GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					string header_filter = enumerator.Current;
					try
					{
						string responseMarker = req.ResponseMarker;
						string[] array2 = (from x in array
							where x.Length >= 12
							where x.Substring(6, 2) == header_filter
							select x).ToArray<string>();
						for (int i = 0; i < array2.Length; i++)
						{
							if (array2[i].Length > 24)
							{
								array2[i] = array2[i].Substring(0, 24);
							}
						}
						string text = null;
						string text2 = null;
						if (array2 != null && array2.Length != 0)
						{
							int num2 = 10;
							if (this.ELMStatus.STNReceiveSegmentation)
							{
								req.CheckLength = false;
								num2 = 8;
							}
							if (array2.Length == 1)
							{
								string text3 = array2.First<string>();
								int num3 = text3.IndexOf(responseMarker, num2, StringComparison.Ordinal);
								if (num3 >= 0)
								{
									text = text3.Substring(num3);
									text = this.PrepareResponseLine(text);
									text2 = text3.Substring(num3 - 2, 2);
								}
							}
							else if (array2.Length > 1)
							{
								int num4 = array2[0].IndexOf(responseMarker, num2, StringComparison.Ordinal);
								if (num4 >= 0)
								{
									StringBuilder stringBuilder = new StringBuilder(array2.Length);
									for (int j = 0; j < array2.Length; j++)
									{
										string text4;
										if (j == 0)
										{
											text4 = array2[j].Substring(num4);
											text2 = array2[j].Substring(num4 - 2, 2);
										}
										else
										{
											text4 = array2[j].Substring(num4 - 2);
										}
										text4 = this.PrepareResponseLine(text4);
										stringBuilder.Append(text4);
									}
									text = stringBuilder.ToString();
								}
							}
							if (text != null)
							{
								int num5 = BitHelpers.ConvertHexToInt(text2);
								int num6 = num5 * 2;
								if (num5 > 4 && text.Length > num6 && !this.ELMStatus.STNReceiveSegmentation)
								{
									text = text.Substring(0, num5 * 2);
								}
								text = text.Substring(req.ResponseMarker.Length);
								if (text.Length % 2 != 0)
								{
									text = text.Substring(0, text.Length - 1);
								}
								byte[] array3 = BitHelpers.ConvertHexToBytesX(text);
								flag = this.CurrentCarData.DecodeDTC(command, req, header_filter, array3, timeStamp);
								try
								{
									req.OnResponseDecoded(array3, flag, header_filter);
								}
								catch (Exception)
								{
								}
							}
						}
					}
					catch
					{
					}
				}
			}
			return flag;
		}

		// Token: 0x06002540 RID: 9536 RVA: 0x001C5EA8 File Offset: 0x001C40A8
		public bool DecodeDTC_KWP(string data, OBDRequest request, TimeSpan timeStamp)
		{
			string command = request.Command;
			bool flag = false;
			string[] array = (from x in OBDDataReader.FilterHexAndNewLineOnly(data).Replace(" ", string.Empty).Split(OBDDataReader.line_splitter, StringSplitOptions.RemoveEmptyEntries)
				select x.Trim()).ToArray<string>();
			if (array.Length > 4 && SharedSettings.Current.KWPConcatResponseLines)
			{
				StringBuilder stringBuilder = new StringBuilder(array.Length);
				foreach (string text in array)
				{
					if (text.Length >= 2)
					{
						stringBuilder.Append(text);
					}
				}
				array = new string[] { stringBuilder.ToString() };
			}
			using (List<string>.Enumerator enumerator = this.GetHeaders(request.ResponseMarker, array, request.ELMFormat).GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					string header_filter = enumerator.Current;
					try
					{
						string text2 = request.ResponseMarker;
						data.Replace(" ", string.Empty);
						string[] array3 = array.Where((string x) => x.Length >= 6 && x.Substring(4, 2) == header_filter).ToArray<string>();
						if (array3 != null && array3.Length != 0)
						{
							if (SharedSettings.Current.DecodeMUT2Compatible && command.Length == 4 && !command.StartsWith("0", StringComparison.Ordinal) && command.StartsWith("A", StringComparison.Ordinal))
							{
								text2 = text2.Substring(0, 2);
							}
							string text3 = null;
							if (array3.Length > 1)
							{
								for (int j = 0; j < array3.Length; j++)
								{
									string text4 = this.PrepareResponseLine(array3[j]);
									int num = text4.IndexOf(text2, 6, StringComparison.Ordinal);
									if (num >= 0)
									{
										if (BitHelpers.GetCheckSummHex(text4.Substring(0, text4.Length - 2)) == text4.Substring(text4.Length - 2))
										{
											text4 = text4.Substring(0, text4.Length - 2);
										}
										array3[j] = text4.Substring(num + text2.Length);
									}
								}
								StringBuilder stringBuilder2 = new StringBuilder(array3.Length);
								foreach (string text5 in array3)
								{
									stringBuilder2.Append(text5);
								}
								text3 = stringBuilder2.ToString();
							}
							else
							{
								int num2 = array3[0].IndexOf(text2, 6, StringComparison.Ordinal);
								if (num2 > 0)
								{
									string text6 = array3[0];
									if (BitHelpers.GetCheckSummHex(text6.Substring(0, text6.Length - 2)) == text6.Substring(text6.Length - 2))
									{
										text6 = text6.Substring(0, text6.Length - 2);
									}
									text3 = text6.Substring(num2 + text2.Length);
									text3 = this.PrepareResponseLine(text3);
								}
							}
							if (text3 != null)
							{
								byte[] array4 = BitHelpers.ConvertHexToBytesX(text3);
								flag = this.CurrentCarData.DecodeDTC(command, request, header_filter, array4, timeStamp);
								try
								{
									request.OnResponseDecoded(array4, flag, header_filter);
								}
								catch (Exception)
								{
								}
							}
						}
					}
					catch
					{
					}
				}
			}
			return flag;
		}

		// Token: 0x06002541 RID: 9537 RVA: 0x001C620C File Offset: 0x001C440C
		internal List<string> GetHeaders(string reply_marker, string[] reply_lines, ELMFormat elmFormat)
		{
			if (elmFormat == ELMFormat.Unknown)
			{
				elmFormat = this.CurrentELMFormat;
			}
			if (elmFormat == ELMFormat.KWP && reply_lines.Length > 4 && SharedSettings.Current.KWPConcatResponseLines)
			{
				StringBuilder stringBuilder = new StringBuilder(reply_lines.Length);
				foreach (string text in reply_lines)
				{
					if (text.Length >= 2)
					{
						stringBuilder.Append(text);
					}
				}
				reply_lines = new string[] { stringBuilder.ToString() };
			}
			string[] array2 = reply_lines.Where((string x) => x.Length > 3 && x.Contains(reply_marker)).ToArray<string>();
			if (array2.Length != 0)
			{
				List<string> list = new List<string>();
				foreach (string text2 in array2)
				{
					try
					{
						int num = text2.IndexOf(reply_marker);
						string text3 = text2.Substring(0, num);
						switch (elmFormat)
						{
						case ELMFormat.Unknown:
							return this.GetHeadersUnknownFormat(reply_marker, reply_lines);
						case ELMFormat.KWP:
							text3 = text3.Substring(4, 2);
							break;
						case ELMFormat.CAN11bit:
						{
							string text4 = text3.Substring(0, 3);
							text2.Substring(num);
							if (text2.Length % 2 == 0 && text2[1] == '7')
							{
								text4 = text3.Substring(1, 3);
							}
							text3 = text4;
							break;
						}
						case ELMFormat.CAN29bit:
							if (num == 10 || num == 12 || (this.ELMStatus.STNReceiveSegmentation && num == 8) || num == 10)
							{
								text3 = text3.Substring(6, 2);
							}
							else
							{
								text3 = "";
							}
							break;
						}
						if (!string.IsNullOrEmpty(text3))
						{
							list.Add(text3);
						}
					}
					catch (Exception)
					{
					}
				}
				if (list.Any((string x) => x.Length == 3))
				{
					if (list.Any((string x) => x.Length != 3))
					{
						list.RemoveAll((string x) => x.Length != 3);
					}
				}
				list.Sort();
				return list;
			}
			return new List<string>();
		}

		// Token: 0x06002542 RID: 9538 RVA: 0x001C6454 File Offset: 0x001C4654
		public List<string> GetHeadersUnknownFormat(string reply_marker, string[] reply0100_lines)
		{
			string[] array = reply0100_lines.Where((string x) => x.Contains(reply_marker)).ToArray<string>();
			List<string> list = new List<string>();
			foreach (string text in array)
			{
				int num = text.IndexOf(reply_marker);
				if (num > 0)
				{
					string text2 = text.Substring(0, num);
					int length = text2.Length;
					if (length != 5)
					{
						if (length != 10)
						{
							text2 = text2.Substring(text2.Length - 2);
						}
						else
						{
							int num2 = BitHelpers.ConvertHexToInt(text2.Substring(text2.Length - 2));
							string text3 = text.Substring(num);
							if (num2 == text3.Length / 2)
							{
								text2 = text2.Substring(text2.Length - 4, 2);
							}
							else
							{
								text2 = text2.Substring(text2.Length - 2);
							}
						}
					}
					else
					{
						text2 = text2.Substring(0, 3);
					}
					list.Add(text2);
				}
			}
			list.Sort();
			if (list.Any((string x) => x.Length == 3))
			{
				if (list.Any((string x) => x.Length != 3))
				{
					list.RemoveAll((string x) => x.Length != 3);
				}
			}
			return list;
		}

		// Token: 0x06002543 RID: 9539 RVA: 0x001C65D4 File Offset: 0x001C47D4
		public bool DecodeDTC(string data, OBDRequest request)
		{
			if (request.DoNotDecode)
			{
				EventHandler<int> dtcreadingQueueProgress = this.DTCReadingQueueProgress;
				if (dtcreadingQueueProgress != null)
				{
					dtcreadingQueueProgress(this, this.CommandQueue.Count);
				}
				return false;
			}
			TimeSpan elapsed = this.stopwatch.Elapsed;
			if (request.ELMFormat == ELMFormat.Unknown)
			{
				request.ELMFormat = this.CurrentELMFormat;
			}
			string command = request.Command;
			if (command == "1003" || command == "10C0" || command == "1090" || command == "20")
			{
				EventHandler<int> dtcreadingQueueProgress2 = this.DTCReadingQueueProgress;
				if (dtcreadingQueueProgress2 != null)
				{
					dtcreadingQueueProgress2(this, this.CommandQueue.Count);
				}
				return true;
			}
			if ((data != null && data.Contains("NO DATA")) || data == null)
			{
				EventHandler<int> dtcreadingQueueProgress3 = this.DTCReadingQueueProgress;
				if (dtcreadingQueueProgress3 != null)
				{
					dtcreadingQueueProgress3(this, this.CommandQueue.Count);
				}
				return false;
			}
			data = data.TrimStart();
			if (request.ELMFormat == ELMFormat.CAN11bit && data.Length >= 7 && data[5] == '7' && (data[6] == 'F' || data[6] == 'f') && (data[9] != '7' || data[10] != '8') && (data[9] != '2' || data[10] != '1') && !data.Contains(request.ResponseMarker))
			{
				EventHandler<int> dtcreadingQueueProgress4 = this.DTCReadingQueueProgress;
				if (dtcreadingQueueProgress4 != null)
				{
					dtcreadingQueueProgress4(this, this.CommandQueue.Count);
				}
				return false;
			}
			bool flag;
			switch (request.ELMFormat)
			{
			case ELMFormat.CAN11bit:
				flag = this.DecodeDTC_CAN11bit(data, request, elapsed);
				goto IL_01AF;
			case ELMFormat.CAN29bit:
				flag = this.DecodeDTC_CAN29bit(data, request, elapsed);
				goto IL_01AF;
			}
			flag = this.DecodeDTC_KWP(data, request, elapsed);
			IL_01AF:
			EventHandler<int> dtcreadingQueueProgress5 = this.DTCReadingQueueProgress;
			if (dtcreadingQueueProgress5 != null)
			{
				dtcreadingQueueProgress5(this, this.CommandQueue.Count);
			}
			return flag;
		}

		// Token: 0x06002544 RID: 9540 RVA: 0x001C67AE File Offset: 0x001C49AE
		private bool LineIsNegativeCAN11bitResponse(string data)
		{
			return this.CurrentELMFormat == ELMFormat.CAN11bit && data.Length >= 7 && data[5] == '7' && (data[6] == 'F' || data[6] == 'f');
		}

		// Token: 0x06002545 RID: 9541 RVA: 0x001C67E8 File Offset: 0x001C49E8
		public async Task<bool> CheckSupportedPIDsV2()
		{
			OBDRequestQueueOptimizer.MainECUSupportedItems.Clear();
			foreach (PID pid in this.CurrentCarData.LiveDataPIDs)
			{
				if (!(pid is CustomPID) && !pid.Command.StartsWith("09"))
				{
					pid.IsAvailable = false;
				}
			}
			if (SharedSettings.Current.UseOBD2)
			{
				try
				{
					if (!SharedSettings.Current.PerformSensorsScanByTesting)
					{
						PID mafpid;
						ValueTaskAwaiter<bool> valueTaskAwaiter2;
						if (this.IsNissanConsult2Protocol)
						{
							if (this.CurrentProtocolNumber == 43)
							{
								await this.CheckSupportedPIDsClassicTestV2("2211{0}", "7E0", "", "");
								await this.CheckSupportedPIDsClassicTestV2("2212{0}", "7E0", "", "");
								await this.CheckSupportedPIDsClassicTestV2("2213{0}", "7E0", "", "");
							}
							else
							{
								await this.CheckSupportedPIDsClassicTestV2("2211{0}", "", "", "");
								await this.CheckSupportedPIDsClassicTestV2("2212{0}", "", "", "");
								await this.CheckSupportedPIDsClassicTestV2("2213{0}", "", "", "");
							}
							try
							{
								try
								{
									mafpid = this.CurrentCarData.LiveDataPIDs.FirstOrDefault((PID x) => x.Role == Roles.MAF && x.Command == "221209");
									if (mafpid != null && !mafpid.IsAvailable)
									{
										OBDRequest req = new OBDRequest(mafpid.Command, false, mafpid);
										this.CurrentCarData.ShouldSetAsAvailable = true;
										await this.SendRequest(req);
										ValueTaskAwaiter<bool> valueTaskAwaiter = this.DecodeData(await this.ReadData(2500, null, -1), req, null).GetAwaiter();
										if (!valueTaskAwaiter.IsCompleted)
										{
											await valueTaskAwaiter;
											valueTaskAwaiter = valueTaskAwaiter2;
											valueTaskAwaiter2 = default(ValueTaskAwaiter<bool>);
										}
										if (valueTaskAwaiter.GetResult())
										{
											mafpid.IsAvailable = true;
										}
										req = null;
									}
									mafpid = null;
								}
								catch (Exception)
								{
								}
								goto IL_13CF;
							}
							finally
							{
								this.CurrentCarData.ShouldSetAsAvailable = false;
							}
						}
						string text = SharedSettings.Current.Mode01Prefix + "{0}";
						if (SharedSettings.Current.DaihatsuKLine)
						{
							text = "21{0}01";
						}
						await this.CheckSupportedPIDsClassicTestV2(text, "", "", "");
						PIDWithFloatValueFormula.ResetScalingToDefaults();
						mafpid = this.CurrentCarData.LiveDataPIDs.First((PID x) => x.Id == 103);
						if (mafpid.IsAvailable)
						{
							OBDRequest req = new OBDRequest(mafpid.Command, false, new List<PID> { mafpid });
							await this.SendString(mafpid.Command);
							await this.DecodeData(await this.ReadData(2500, null, -1), req, null);
							req = null;
						}
						PID pid2 = this.CurrentCarData.LiveDataPIDs.First((PID x) => x.Id == 104);
						if (pid2.IsAvailable)
						{
							OBDRequest req = new OBDRequest(pid2.Command, false, new List<PID> { mafpid });
							await this.SendString(pid2.Command);
							await this.DecodeData(await this.ReadData(2500, null, -1), req, null);
							req = null;
						}
						await this.CheckPidsByTestings(new string[]
						{
							"0165", "0166", "0167", "0168", "0169", "016A", "016B", "016C", "016D", "016E",
							"016F", "0170", "0171", "0172", "0173", "0173", "0174", "0175", "0176", "0177",
							"0178", "0179", "017A", "017B", "017C", "017D", "017E", "017F", "0181", "0182",
							"0183", "0185", "0186", "0187", "0189", "018A", "018B", "018C", "018F", "0192",
							"0193", "0194", "0195", "0196", "0197", "0198", "0199", "019A", "019B", "019C",
							"019F", "01A1", "01A3", "01A4", "01A5", "01A7", "01A8", "01A9", "01AB", "01AC",
							"01AD", "01AE", "01B1", "01B3", "01B4", "01B5", "01B6", "01B7", "01BB", "01BC",
							"01BD", "01BE", "01BF", "01C0", "01C1", "01C2", "01C3", "01C5", "01C6", "01C9",
							"01CA", "01CB", "01CC", "01CD"
						}.Intersect((from x in this.CurrentCarData.LiveDataPIDs
							where x.IsAvailable && (x is IPIDFloatValue || x is IPIDWithStringValue)
							select x.Command).ToArray<string>()).ToList<string>());
						if ((this.CurrentELMFormat == ELMFormat.CAN11bit || this.CurrentELMFormat == ELMFormat.CAN29bit) && (SharedSettings.Current.SelectedBrand == "Nissan" || SharedSettings.Current.SelectedBrand == "Infiniti") && SharedSettings.Current.AddNissanConsult3Pids)
						{
							string ecm_header = "7E0";
							if (this.CurrentELMFormat == ELMFormat.CAN29bit)
							{
								ecm_header = "DA15F1";
							}
							OBDRequest obdrequest = new OBDRequest("10C0", ecm_header, "", "", false);
							OBDRequest req = new OBDRequest("1081", ecm_header, "", "", false);
							if (SharedSettings.Current.NissanConsult3OpenCloseSession)
							{
								await this.SendRequest(obdrequest);
								await this.ReadData(2500, null, -1);
							}
							await this.CheckSupportedPIDsClassicTestV2("2211{0}", ecm_header, "", "");
							await this.CheckSupportedPIDsClassicTestV2("2212{0}", ecm_header, "", "");
							await this.CheckSupportedPIDsClassicTestV2("2213{0}", ecm_header, "", "");
							if (SharedSettings.Current.NissanConsult3OpenCloseSession)
							{
								await this.SendRequest(req);
								await this.ReadData(2500, null, -1);
							}
							try
							{
								PID mafpid2 = this.CurrentCarData.LiveDataPIDs.FirstOrDefault((PID x) => x.Role == Roles.MAF && x.Command == "221209");
								if (mafpid2 != null && !mafpid2.IsAvailable)
								{
									List<OBDRequest> list = new List<OBDRequest>(1);
									LiveDataPIDModel.GetRequests(mafpid2, list, null, "");
									if (list.Count > 0)
									{
										OBDRequest req2 = list[0];
										this.CurrentCarData.ShouldSetAsAvailable = true;
										await this.SendRequest(req2);
										ValueTaskAwaiter<bool> valueTaskAwaiter = this.DecodeData(await this.ReadData(2500, null, -1), req2, null).GetAwaiter();
										if (!valueTaskAwaiter.IsCompleted)
										{
											await valueTaskAwaiter;
											valueTaskAwaiter = valueTaskAwaiter2;
											valueTaskAwaiter2 = default(ValueTaskAwaiter<bool>);
										}
										if (valueTaskAwaiter.GetResult())
										{
											mafpid2.IsAvailable = true;
										}
										req2 = null;
									}
								}
								mafpid2 = null;
							}
							catch (Exception)
							{
							}
							finally
							{
								this.CurrentCarData.ShouldSetAsAvailable = false;
							}
							ecm_header = null;
							req = null;
						}
						mafpid = null;
					}
					else
					{
						await this.CheckSupportedPIDsScanTest();
					}
					IL_13CF:
					await Task.Delay(500);
					this.CurrentCarData.InitializeCalculatedPIDs();
					return true;
				}
				catch (Exception)
				{
					return false;
				}
			}
			this.CurrentCarData.InitializeCalculatedPIDs();
			return true;
		}

		// Token: 0x06002546 RID: 9542 RVA: 0x001C682C File Offset: 0x001C4A2C
		public async Task CheckSupportedPIDsClassicTestV2(string pid_template, string requestHeader = "", string beforeCommands = "", string afterCommands = "")
		{
			bool finished = false;
			int idx = 0;
			PID_SupportedPids pid = null;
			do
			{
				string text = string.Format(pid_template, idx.ToString("X2"));
				pid = new PID_SupportedPids(text, this.CurrentCarData.LiveDataPIDs, requestHeader);
				OBDRequest req = new OBDRequest(text, requestHeader, beforeCommands, afterCommands, false, pid);
				await this.SendRequest(req);
				string data = await this.ReadData(2500, null, -1);
				string[] array = OBDDataReader.FilterHexAndNewLineOnly(data).Split(OBDDataReader.line_splitter, StringSplitOptions.RemoveEmptyEntries);
				List<string> headers = this.GetHeaders(req.ResponseMarker, array, ELMFormat.Unknown);
				finished = true;
				foreach (string text2 in headers)
				{
					await this.DecodeData(data, req, text2);
					if (pid.Value[pid.Value.Length - 1])
					{
						finished = false;
					}
				}
				List<string>.Enumerator enumerator = default(List<string>.Enumerator);
				idx += 32;
				req = null;
				data = null;
			}
			while (!finished);
		}

		// Token: 0x06002547 RID: 9543 RVA: 0x001C6890 File Offset: 0x001C4A90
		public async Task CheckSupportedPIDsScanTest()
		{
			this.CurrentMode = OBDDataReader.OBDModes.Mode06;
			object obj = null;
			try
			{
				foreach (PID pid2 in this.CurrentCarData.LiveDataPIDs)
				{
					if (!(pid2 is CalculatedPIDV2) && !(pid2 is SensorPID))
					{
						pid2.IsAvailable = false;
					}
				}
				List<string> list = (from pid in this.CurrentCarData.LiveDataPIDs
					where !(pid is CalculatedPIDV2) && !(pid is SensorPID) && !(pid is PID_SupportedPids) && !string.IsNullOrEmpty(pid.Command) && !pid.Command.StartsWith("09")
					select pid into x
					select x.Command).Distinct<string>().ToList<string>();
				if ((this.IsNissanConsult2Protocol && this.CurrentProtocolNumber == 11) || (this.IsNissanConsult2Protocol && this.CurrentProtocolNumber == 43 && !SharedSettings.Current.UseOBD2))
				{
					list = list.Where((string x) => x.Length > 4).ToList<string>();
				}
				this.CurrentCarData.ShouldSetAsAvailable = true;
				await this.CheckPidsByTestings(list);
			}
			catch (object obj)
			{
			}
			await Task.Delay(500);
			this.CurrentMode = OBDDataReader.OBDModes.Universal;
			this.CurrentCarData.ShouldSetAsAvailable = false;
			object obj2 = obj;
			if (obj2 != null)
			{
				Exception ex = obj2 as Exception;
				if (ex == null)
				{
					throw obj2;
				}
				ExceptionDispatchInfo.Capture(ex).Throw();
			}
			obj = null;
		}

		// Token: 0x06002548 RID: 9544 RVA: 0x001C68D4 File Offset: 0x001C4AD4
		private async Task CheckPidsByTestings(List<string> pidList)
		{
			foreach (string text in pidList)
			{
				OBDRequest req = new OBDRequest(text, false);
				await this.SendRequest(req);
				string text2 = await this.ReadData(2500, null, -1);
				await this.DecodeData(text2, req, null);
				foreach (PID pid in req.PIDs)
				{
					if (pid is IPIDFloatValue && double.IsNaN((pid as IPIDFloatValue).Value))
					{
						pid.IsAvailable = false;
						LiveDataPIDModel._PIDCollection.Remove(pid);
					}
					else if (pid is IPIDWithStringValue && (pid as IPIDWithStringValue).Value == "n/a")
					{
						pid.IsAvailable = false;
						LiveDataPIDModel._PIDCollection.Remove(pid);
					}
				}
				req = null;
			}
			List<string>.Enumerator enumerator = default(List<string>.Enumerator);
		}

		// Token: 0x06002549 RID: 9545 RVA: 0x001C6920 File Offset: 0x001C4B20
		public async Task ChangeECU()
		{
			await this.Stop("ChangeECU");
			TaskAwaiter<bool> taskAwaiter = this.CheckSupportedPIDsV2().GetAwaiter();
			if (!taskAwaiter.IsCompleted)
			{
				await taskAwaiter;
				TaskAwaiter<bool> taskAwaiter2;
				taskAwaiter = taskAwaiter2;
				taskAwaiter2 = default(TaskAwaiter<bool>);
			}
			if (!taskAwaiter.GetResult())
			{
				this.OnDisconnectDetected(OBDDataReader.DisconnectReason.ELMStuck);
			}
			else
			{
				LiveDataPIDModel.UpdatePIDCollection(this);
				this.Start("FromChangeECU");
			}
		}

		// Token: 0x0600254A RID: 9546 RVA: 0x001C6964 File Offset: 0x001C4B64
		public async Task<bool> CheckECUConnectionWhileRunning(bool ForceSendHeader = false)
		{
			OBDRequest checkECUCommand;
			if (SharedSettings.Current.UseDefaultInit)
			{
				if (this.IsNissanConsult2Protocol)
				{
					checkECUCommand = new OBDRequest("221201", "", "", "", false);
				}
				else
				{
					checkECUCommand = new OBDRequest(SharedSettings.Current.Mode01Prefix + "00", "", "", "", false);
				}
			}
			else if (string.IsNullOrEmpty(SharedSettings.Current.DetectECUConnectionPID))
			{
				checkECUCommand = this.GetDefaultPidRequest();
			}
			else
			{
				checkECUCommand = new OBDRequest(SharedSettings.Current.DetectECUConnectionPID, this.GetDefaultHeader(), false);
			}
			bool? DecodeResult = null;
			checkECUCommand.ResponseReceived += delegate(OBDRequest req, string data)
			{
				if (string.IsNullOrEmpty(data))
				{
					return;
				}
				if (string.IsNullOrEmpty(req.ResponseMarker))
				{
					return;
				}
				if (req.ResponseMarker.Length < 2)
				{
					return;
				}
				string text = req.ResponseMarker.Substring(0, 2);
				foreach (string text2 in OBDDataReader.FilterHexAndNewLineOnly(data).Split(OBDDataReader.line_splitter))
				{
					if (text2 != null && text2.Length >= 7 && text2.IndexOf(text, 5) >= 0)
					{
						DecodeResult = new bool?(true);
						return;
					}
				}
			};
			checkECUCommand.ResponseDecoded += delegate(OBDRequest request, byte[] data, bool decodeResult, string responseHeader)
			{
				if (DecodeResult.GetValueOrDefault())
				{
					return;
				}
				DecodeResult = new bool?(decodeResult);
			};
			await Task.Delay(200);
			if (ForceSendHeader)
			{
				this.ELM327_LastSentHeader = this.GetDefaultHeader();
			}
			App.OBDReader.ReplaceQueue(new List<OBDRequest>(1) { checkECUCommand });
			while (DecodeResult == null)
			{
				await Task.Delay(50);
			}
			bool flag = DecodeResult.Value;
			if (!flag && !ForceSendHeader)
			{
				flag = await this.CheckECUConnectionWhileRunning(true);
			}
			return flag;
		}

		// Token: 0x0600254B RID: 9547 RVA: 0x001C69B0 File Offset: 0x001C4BB0
		public byte[] LineToByte(string line)
		{
			int length = line.Length;
			StringBuilder stringBuilder = new StringBuilder(line.Length);
			foreach (char c in line)
			{
				if (c != ' ')
				{
					if ((c < '0' || c > '9') && (c < 'A' || c > 'F') && (c < 'a' || c > 'f'))
					{
						break;
					}
					stringBuilder.Append(c);
				}
			}
			return BitHelpers.ConvertHexToBytesX(stringBuilder.ToString());
		}

		// Token: 0x0600254C RID: 9548 RVA: 0x000027D4 File Offset: 0x000009D4
		private void DecodeToCANFrames(string data)
		{
		}

		// Token: 0x0600254D RID: 9549 RVA: 0x001C6A1F File Offset: 0x001C4C1F
		// Note: this type is marked as 'beforefieldinit'.
		static OBDDataReader()
		{
		}

		// Token: 0x0600254E RID: 9550 RVA: 0x001C6A42 File Offset: 0x001C4C42
		[CompilerGenerated]
		private void <GetViecarDeviceId>b__130_0(object sender, DeviceEventArgs e)
		{
			this.btle_devices.Add(e.Device);
		}

		// Token: 0x0600254F RID: 9551 RVA: 0x001C6A55 File Offset: 0x001C4C55
		[CompilerGenerated]
		private void <ResetVars>b__167_0()
		{
			this.ECUHeaders.Clear();
		}

		// Token: 0x06002550 RID: 9552 RVA: 0x001C6A55 File Offset: 0x001C4C55
		[CompilerGenerated]
		private void <Initialize>b__168_1()
		{
			this.ECUHeaders.Clear();
		}

		// Token: 0x06002551 RID: 9553 RVA: 0x001C6A64 File Offset: 0x001C4C64
		[CompilerGenerated]
		private async void <InitializeCustomInitString>b__170_0()
		{
			await this.DebugWrite("\r\n[InitializeCustomInitString:LengthTooBigHandler]\r\n");
		}

		// Token: 0x06002552 RID: 9554 RVA: 0x001C6A9C File Offset: 0x001C4C9C
		[CompilerGenerated]
		private async void <Stop>b__183_0()
		{
			await this.WaitForCommandQueue();
		}

		// Token: 0x06002553 RID: 9555 RVA: 0x001C6A55 File Offset: 0x001C4C55
		[CompilerGenerated]
		private void <Disconnect>b__198_0()
		{
			this.ECUHeaders.Clear();
		}

		// Token: 0x06002554 RID: 9556 RVA: 0x001C6AD3 File Offset: 0x001C4CD3
		[CompilerGenerated]
		private bool <DecodeDTC_CAN11bit>b__204_1(string x)
		{
			return !this.LineIsNegativeCAN11bitResponse(x);
		}

		// Token: 0x04001234 RID: 4660
		private long lastTimeConnected;

		// Token: 0x04001235 RID: 4661
		private IVWTPManager _vwTPManager;

		// Token: 0x04001236 RID: 4662
		[CompilerGenerated]
		private ELMState <ELMStatus>k__BackingField;

		// Token: 0x04001237 RID: 4663
		[CompilerGenerated]
		private EventHandler<int> DTCReadingQueueProgress;

		// Token: 0x04001238 RID: 4664
		private volatile OBDDataReader.RemoteDeviceLastAction LastAction;

		// Token: 0x04001239 RID: 4665
		private string ELM327_LastSentHeader = "";

		// Token: 0x0400123A RID: 4666
		private string[] ELM327_LastSentBeforeCommands = new string[0];

		// Token: 0x0400123B RID: 4667
		private string[] ELM327_PendingAfterCommands = new string[0];

		// Token: 0x0400123C RID: 4668
		[CompilerGenerated]
		private bool <STCommandsStupported>k__BackingField;

		// Token: 0x0400123D RID: 4669
		[CompilerGenerated]
		private bool <VTCommandsStupported>k__BackingField;

		// Token: 0x0400123E RID: 4670
		private long CommandsCounter;

		// Token: 0x0400123F RID: 4671
		private object curModeLock = new object();

		// Token: 0x04001240 RID: 4672
		private OBDDataReader.OBDModes _CurrentMode;

		// Token: 0x04001241 RID: 4673
		protected long LastReadOrWriteTicks;

		// Token: 0x04001242 RID: 4674
		public int SendDelay;

		// Token: 0x04001243 RID: 4675
		public int NO_DATA_Counter;

		// Token: 0x04001244 RID: 4676
		public ELMFormat CurrentELMFormat;

		// Token: 0x04001245 RID: 4677
		public volatile bool DisconnectRequested;

		// Token: 0x04001246 RID: 4678
		[CompilerGenerated]
		private EventHandler QueueCompleted;

		// Token: 0x04001247 RID: 4679
		protected volatile ConcurrentQueue<OBDRequest> _CommandQueue = new ConcurrentQueue<OBDRequest>();

		// Token: 0x04001248 RID: 4680
		protected IOBDConnection Connection;

		// Token: 0x04001249 RID: 4681
		private volatile bool _Running;

		// Token: 0x0400124A RID: 4682
		protected volatile bool RWCycleEnded = true;

		// Token: 0x0400124B RID: 4683
		protected OBDDataReaderStatus _CurrentStatus;

		// Token: 0x0400124C RID: 4684
		[CompilerGenerated]
		private CurrentStatusChangedEvent StatusChanged;

		// Token: 0x0400124D RID: 4685
		private string[] EmptyStringArray = new string[0];

		// Token: 0x0400124E RID: 4686
		[CompilerGenerated]
		private int <CurrentProtocolNumber>k__BackingField;

		// Token: 0x0400124F RID: 4687
		protected ObservableCollection<ECUHeader> _ECUHeaders;

		// Token: 0x04001250 RID: 4688
		protected string LastDeviceID = "";

		// Token: 0x04001251 RID: 4689
		protected ConnectionTypes LastUsedConnectionType;

		// Token: 0x04001252 RID: 4690
		public long LastReadTimeStampTicks;

		// Token: 0x04001253 RID: 4691
		private bool DroidWiFiV2Failed;

		// Token: 0x04001254 RID: 4692
		private int initFailCounterOnAT;

		// Token: 0x04001255 RID: 4693
		private bool _BadELM;

		// Token: 0x04001256 RID: 4694
		protected int _ECU_ID;

		// Token: 0x04001257 RID: 4695
		protected TimeSpan _ReadDelay;

		// Token: 0x04001258 RID: 4696
		[CompilerGenerated]
		private PropertyChangedEventHandler PropertyChanged;

		// Token: 0x04001259 RID: 4697
		private bool _ECUSelectorVisible;

		// Token: 0x0400125A RID: 4698
		private SemaphoreSlim waitingForQueueSemaphore;

		// Token: 0x0400125B RID: 4699
		[CompilerGenerated]
		private Stopwatch <stopwatch>k__BackingField;

		// Token: 0x0400125C RID: 4700
		private CarData _CurrentCarData;

		// Token: 0x0400125D RID: 4701
		private ObservableCollection<IDevice> btle_devices;

		// Token: 0x0400125E RID: 4702
		private Queue<string> readBufferQueue = new Queue<string>();

		// Token: 0x0400125F RID: 4703
		private string unfinishedReadLine = "";

		// Token: 0x04001260 RID: 4704
		private StringBuilder readSb = new StringBuilder(64);

		// Token: 0x04001261 RID: 4705
		private Stopwatch swReadData = new Stopwatch();

		// Token: 0x04001262 RID: 4706
		private int loopId;

		// Token: 0x04001263 RID: 4707
		private object lock_obj = new object();

		// Token: 0x04001264 RID: 4708
		private volatile bool WorkingWithQueue;

		// Token: 0x04001265 RID: 4709
		private static StringBuilder filterSb = new StringBuilder(64);

		// Token: 0x04001266 RID: 4710
		public static char[] line_splitter = new char[] { '\r', '\n' };

		// Token: 0x04001267 RID: 4711
		private ulong decodeCounter;

		// Token: 0x02000329 RID: 809
		public enum RemoteDeviceLastAction
		{
			// Token: 0x04001269 RID: 4713
			Read,
			// Token: 0x0400126A RID: 4714
			Write
		}

		// Token: 0x0200032A RID: 810
		public enum OBDModes
		{
			// Token: 0x0400126C RID: 4716
			Universal,
			// Token: 0x0400126D RID: 4717
			Mode06,
			// Token: 0x0400126E RID: 4718
			ReadDTC,
			// Token: 0x0400126F RID: 4719
			ClearDTC,
			// Token: 0x04001270 RID: 4720
			Terminal
		}

		// Token: 0x0200032B RID: 811
		public enum InitModes
		{
			// Token: 0x04001272 RID: 4722
			Default,
			// Token: 0x04001273 RID: 4723
			RestoreConnection
		}

		// Token: 0x0200032C RID: 812
		private enum OBDReadingModes
		{
			// Token: 0x04001275 RID: 4725
			Mode01PIDS,
			// Token: 0x04001276 RID: 4726
			Mode02PIDs,
			// Token: 0x04001277 RID: 4727
			DTC03,
			// Token: 0x04001278 RID: 4728
			ResetDTC04,
			// Token: 0x04001279 RID: 4729
			DTC07
		}

		// Token: 0x0200032D RID: 813
		private enum DisconnectReason
		{
			// Token: 0x0400127B RID: 4731
			Unknown,
			// Token: 0x0400127C RID: 4732
			UserRequested,
			// Token: 0x0400127D RID: 4733
			NoDataReceived,
			// Token: 0x0400127E RID: 4734
			ELMStuck
		}

		// Token: 0x0200032E RID: 814
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <<InitializeCustomInitString>b__170_0>d : IAsyncStateMachine
		{
			// Token: 0x06002555 RID: 9557 RVA: 0x001C6AE0 File Offset: 0x001C4CE0
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				OBDDataReader obddataReader = this;
				try
				{
					TaskAwaiter taskAwaiter;
					if (num != 0)
					{
						taskAwaiter = obddataReader.DebugWrite("\r\n[InitializeCustomInitString:LengthTooBigHandler]\r\n").GetAwaiter();
						if (!taskAwaiter.IsCompleted)
						{
							num2 = 0;
							TaskAwaiter taskAwaiter2 = taskAwaiter;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, OBDDataReader.<<InitializeCustomInitString>b__170_0>d>(ref taskAwaiter, ref this);
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
				}
				catch (Exception ex)
				{
					num2 = -2;
					this.<>t__builder.SetException(ex);
					return;
				}
				num2 = -2;
				this.<>t__builder.SetResult();
			}

			// Token: 0x06002556 RID: 9558 RVA: 0x001C6B98 File Offset: 0x001C4D98
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x0400127F RID: 4735
			public int <>1__state;

			// Token: 0x04001280 RID: 4736
			public AsyncVoidMethodBuilder <>t__builder;

			// Token: 0x04001281 RID: 4737
			public OBDDataReader <>4__this;

			// Token: 0x04001282 RID: 4738
			private TaskAwaiter <>u__1;
		}

		// Token: 0x0200032F RID: 815
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <<Stop>b__183_0>d : IAsyncStateMachine
		{
			// Token: 0x06002557 RID: 9559 RVA: 0x001C6BA8 File Offset: 0x001C4DA8
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				OBDDataReader obddataReader = this;
				try
				{
					TaskAwaiter taskAwaiter;
					if (num != 0)
					{
						taskAwaiter = obddataReader.WaitForCommandQueue().GetAwaiter();
						if (!taskAwaiter.IsCompleted)
						{
							num2 = 0;
							TaskAwaiter taskAwaiter2 = taskAwaiter;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, OBDDataReader.<<Stop>b__183_0>d>(ref taskAwaiter, ref this);
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
				}
				catch (Exception ex)
				{
					num2 = -2;
					this.<>t__builder.SetException(ex);
					return;
				}
				num2 = -2;
				this.<>t__builder.SetResult();
			}

			// Token: 0x06002558 RID: 9560 RVA: 0x001C6C5C File Offset: 0x001C4E5C
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x04001283 RID: 4739
			public int <>1__state;

			// Token: 0x04001284 RID: 4740
			public AsyncVoidMethodBuilder <>t__builder;

			// Token: 0x04001285 RID: 4741
			public OBDDataReader <>4__this;

			// Token: 0x04001286 RID: 4742
			private TaskAwaiter <>u__1;
		}

		// Token: 0x02000330 RID: 816
		[CompilerGenerated]
		[Serializable]
		private sealed class <>c
		{
			// Token: 0x06002559 RID: 9561 RVA: 0x001C6C6A File Offset: 0x001C4E6A
			// Note: this type is marked as 'beforefieldinit'.
			static <>c()
			{
			}

			// Token: 0x0600255A RID: 9562 RVA: 0x00002050 File Offset: 0x00000250
			public <>c()
			{
			}

			// Token: 0x0600255B RID: 9563 RVA: 0x001C6C76 File Offset: 0x001C4E76
			internal void <OnStatusChanged>b__104_0()
			{
				IScreenKeeper screenKeeper = DependencyService.Get<IScreenKeeper>(0);
				if (screenKeeper == null)
				{
					return;
				}
				screenKeeper.LetScreenOff();
			}

			// Token: 0x0600255C RID: 9564 RVA: 0x001C6C88 File Offset: 0x001C4E88
			internal void <OnStatusChanged>b__104_1()
			{
				try
				{
					IScreenKeeper screenKeeper = DependencyService.Get<IScreenKeeper>(0);
					if (screenKeeper != null)
					{
						screenKeeper.KeepScreenOn();
					}
				}
				catch (Exception)
				{
				}
			}

			// Token: 0x0600255D RID: 9565 RVA: 0x001C6CBC File Offset: 0x001C4EBC
			internal bool <GetViecarDeviceId>b__130_1(IDevice x)
			{
				return x.Name.ToLower().Contains("viecar");
			}

			// Token: 0x0600255E RID: 9566 RVA: 0x001C6CD3 File Offset: 0x001C4ED3
			internal bool <CheckResponseForELM327Reset>b__144_0(char c)
			{
				return c == ' ';
			}

			// Token: 0x0600255F RID: 9567 RVA: 0x001C6CDA File Offset: 0x001C4EDA
			internal bool <ReadDataWithManualFlowControl_CAN>b__152_0(string x)
			{
				return x.StartsWith("ATCEA") && x != "ATCEA";
			}

			// Token: 0x06002560 RID: 9568 RVA: 0x001C6CF6 File Offset: 0x001C4EF6
			internal bool <ReadDataWithManualFlowControl_CAN>b__152_1(string x)
			{
				return x != null && !x.Contains("ERROR") && !x.Contains("BUFFER");
			}

			// Token: 0x06002561 RID: 9569 RVA: 0x001C6D18 File Offset: 0x001C4F18
			internal bool <ReadDataWithManualFlowControl_CAN>b__152_3(CANFrame x)
			{
				return x.Type == CANFrame.CANFrameTypes.MultiFrameFirstFrame;
			}

			// Token: 0x06002562 RID: 9570 RVA: 0x001C6D23 File Offset: 0x001C4F23
			internal bool <SendLongCanRequest>b__153_0(string x)
			{
				return x.Contains("ATCEA") && x != "ATCEA";
			}

			// Token: 0x06002563 RID: 9571 RVA: 0x001C6D3F File Offset: 0x001C4F3F
			internal bool <GetDefaultPidCommand>b__159_0(CustomPID x)
			{
				return x.IsAvailable;
			}

			// Token: 0x06002564 RID: 9572 RVA: 0x001C6D3F File Offset: 0x001C4F3F
			internal bool <GetDefaultPidCommand>b__159_1(CustomPID x)
			{
				return x.IsAvailable;
			}

			// Token: 0x06002565 RID: 9573 RVA: 0x001C6D48 File Offset: 0x001C4F48
			internal bool <Initialize>b__168_0(string x)
			{
				return (PlatformHelper.AppMarket == Markets.RUS && x.Equals("Lrc7gArwidVPdp9SPHGRH0v9GbM=")) || x.Equals("Y5kpo3aphxDnrKsp2if0BMjSUvo=") || x.Equals("VWq6xreY9eTH4u5UC5tJ0t2GYuI=") || x.Equals("Y5kpo3aphxDnrKsp2if0BMjSUvo=") || x.Equals("VWq6xreY9eTH4u5UC5tJ0t2GYuI=") || x.Equals("W8RPrUKpDE0NIvMkG9bDYSf4D1E=") || x.Equals("v9gdPRvdsuLO6CXYxBIIJ5wEhjQ=");
			}

			// Token: 0x06002566 RID: 9574 RVA: 0x001C6CD3 File Offset: 0x001C4ED3
			internal bool <InitializeCustomInitString>b__170_2(char c)
			{
				return c == ' ';
			}

			// Token: 0x06002567 RID: 9575 RVA: 0x001C6CD3 File Offset: 0x001C4ED3
			internal bool <InitializeDefaultInitString>b__175_1(char c)
			{
				return c == ' ';
			}

			// Token: 0x06002568 RID: 9576 RVA: 0x001C6DBA File Offset: 0x001C4FBA
			internal bool <InitializeDefaultInitString>b__175_2(char c)
			{
				return c == '\r';
			}

			// Token: 0x06002569 RID: 9577 RVA: 0x001C6DC1 File Offset: 0x001C4FC1
			internal bool <InitializeDefaultInitString>b__175_3(string line)
			{
				return line.Length == 2;
			}

			// Token: 0x0600256A RID: 9578 RVA: 0x001C6DC1 File Offset: 0x001C4FC1
			internal bool <InitializeDefaultInitString>b__175_4(string x)
			{
				return x.Length == 2;
			}

			// Token: 0x0600256B RID: 9579 RVA: 0x0012443D File Offset: 0x0012263D
			internal string <ParseECUHeaders>b__181_1(string x)
			{
				return x.Trim();
			}

			// Token: 0x0600256C RID: 9580 RVA: 0x000650C7 File Offset: 0x000632C7
			internal bool <ParseECUHeaders>b__181_3(string x)
			{
				return x.Length == 3;
			}

			// Token: 0x0600256D RID: 9581 RVA: 0x001C6DCC File Offset: 0x001C4FCC
			internal bool <ParseECUHeaders>b__181_4(string x)
			{
				return x.Length != 3;
			}

			// Token: 0x0600256E RID: 9582 RVA: 0x001C6DCC File Offset: 0x001C4FCC
			internal bool <ParseECUHeaders>b__181_5(string x)
			{
				return x.Length != 3;
			}

			// Token: 0x0600256F RID: 9583 RVA: 0x0012443D File Offset: 0x0012263D
			internal string <ParseECUHeadersOld>b__182_1(string x)
			{
				return x.Trim();
			}

			// Token: 0x06002570 RID: 9584 RVA: 0x000650C7 File Offset: 0x000632C7
			internal bool <ParseECUHeadersOld>b__182_3(string x)
			{
				return x.Length == 3;
			}

			// Token: 0x06002571 RID: 9585 RVA: 0x001C6DCC File Offset: 0x001C4FCC
			internal bool <ParseECUHeadersOld>b__182_4(string x)
			{
				return x.Length != 3;
			}

			// Token: 0x06002572 RID: 9586 RVA: 0x001C6DCC File Offset: 0x001C4FCC
			internal bool <ParseECUHeadersOld>b__182_5(string x)
			{
				return x.Length != 3;
			}

			// Token: 0x06002573 RID: 9587 RVA: 0x0012443D File Offset: 0x0012263D
			internal string <DecodeMode06Data>b__185_0(string x)
			{
				return x.Trim();
			}

			// Token: 0x06002574 RID: 9588 RVA: 0x0012443D File Offset: 0x0012263D
			internal string <DecodeMode06Data>b__185_2(string x)
			{
				return x.Trim();
			}

			// Token: 0x06002575 RID: 9589 RVA: 0x00016849 File Offset: 0x00014A49
			internal string <DecodeMode06Data>b__185_4(string x)
			{
				return x;
			}

			// Token: 0x06002576 RID: 9590 RVA: 0x00056949 File Offset: 0x00054B49
			internal bool <ReadFreezeFrame>b__199_0(PID x)
			{
				return x.IsAvailable && !(x is PID_SupportedPids);
			}

			// Token: 0x06002577 RID: 9591 RVA: 0x0012443D File Offset: 0x0012263D
			internal string <DecodeDTC_CAN11bitV2>b__203_0(string x)
			{
				return x.Trim();
			}

			// Token: 0x06002578 RID: 9592 RVA: 0x0012443D File Offset: 0x0012263D
			internal string <DecodeDTC_CAN11bit>b__204_0(string x)
			{
				return x.Trim();
			}

			// Token: 0x06002579 RID: 9593 RVA: 0x0012443D File Offset: 0x0012263D
			internal string <DecodeDTC_CAN29bit>b__205_0(string x)
			{
				return x.Trim();
			}

			// Token: 0x0600257A RID: 9594 RVA: 0x001C6DDA File Offset: 0x001C4FDA
			internal bool <DecodeDTC_CAN29bit>b__205_1(string x)
			{
				return x.Length >= 12;
			}

			// Token: 0x0600257B RID: 9595 RVA: 0x0012443D File Offset: 0x0012263D
			internal string <DecodeDTC_KWP>b__206_0(string x)
			{
				return x.Trim();
			}

			// Token: 0x0600257C RID: 9596 RVA: 0x000650C7 File Offset: 0x000632C7
			internal bool <GetHeaders>b__207_1(string x)
			{
				return x.Length == 3;
			}

			// Token: 0x0600257D RID: 9597 RVA: 0x001C6DCC File Offset: 0x001C4FCC
			internal bool <GetHeaders>b__207_2(string x)
			{
				return x.Length != 3;
			}

			// Token: 0x0600257E RID: 9598 RVA: 0x001C6DCC File Offset: 0x001C4FCC
			internal bool <GetHeaders>b__207_3(string x)
			{
				return x.Length != 3;
			}

			// Token: 0x0600257F RID: 9599 RVA: 0x000650C7 File Offset: 0x000632C7
			internal bool <GetHeadersUnknownFormat>b__208_1(string x)
			{
				return x.Length == 3;
			}

			// Token: 0x06002580 RID: 9600 RVA: 0x001C6DCC File Offset: 0x001C4FCC
			internal bool <GetHeadersUnknownFormat>b__208_2(string x)
			{
				return x.Length != 3;
			}

			// Token: 0x06002581 RID: 9601 RVA: 0x001C6DCC File Offset: 0x001C4FCC
			internal bool <GetHeadersUnknownFormat>b__208_3(string x)
			{
				return x.Length != 3;
			}

			// Token: 0x06002582 RID: 9602 RVA: 0x001C6DE9 File Offset: 0x001C4FE9
			internal bool <CheckSupportedPIDsV2>b__211_0(PID x)
			{
				return x.Role == Roles.MAF && x.Command == "221209";
			}

			// Token: 0x06002583 RID: 9603 RVA: 0x001C6E06 File Offset: 0x001C5006
			internal bool <CheckSupportedPIDsV2>b__211_1(PID x)
			{
				return x.Id == 103;
			}

			// Token: 0x06002584 RID: 9604 RVA: 0x001C6E12 File Offset: 0x001C5012
			internal bool <CheckSupportedPIDsV2>b__211_2(PID x)
			{
				return x.Id == 104;
			}

			// Token: 0x06002585 RID: 9605 RVA: 0x001C6E1E File Offset: 0x001C501E
			internal bool <CheckSupportedPIDsV2>b__211_3(PID x)
			{
				return x.IsAvailable && (x is IPIDFloatValue || x is IPIDWithStringValue);
			}

			// Token: 0x06002586 RID: 9606 RVA: 0x001C6E3D File Offset: 0x001C503D
			internal string <CheckSupportedPIDsV2>b__211_4(PID x)
			{
				return x.Command;
			}

			// Token: 0x06002587 RID: 9607 RVA: 0x001C6DE9 File Offset: 0x001C4FE9
			internal bool <CheckSupportedPIDsV2>b__211_5(PID x)
			{
				return x.Role == Roles.MAF && x.Command == "221209";
			}

			// Token: 0x06002588 RID: 9608 RVA: 0x001C6E45 File Offset: 0x001C5045
			internal bool <CheckSupportedPIDsScanTest>b__213_0(PID pid)
			{
				return !(pid is CalculatedPIDV2) && !(pid is SensorPID) && !(pid is PID_SupportedPids) && !string.IsNullOrEmpty(pid.Command) && !pid.Command.StartsWith("09");
			}

			// Token: 0x06002589 RID: 9609 RVA: 0x001C6E3D File Offset: 0x001C503D
			internal string <CheckSupportedPIDsScanTest>b__213_1(PID x)
			{
				return x.Command;
			}

			// Token: 0x0600258A RID: 9610 RVA: 0x001C6E81 File Offset: 0x001C5081
			internal bool <CheckSupportedPIDsScanTest>b__213_2(string x)
			{
				return x.Length > 4;
			}

			// Token: 0x04001287 RID: 4743
			public static readonly OBDDataReader.<>c <>9 = new OBDDataReader.<>c();

			// Token: 0x04001288 RID: 4744
			public static Action <>9__104_0;

			// Token: 0x04001289 RID: 4745
			public static Action <>9__104_1;

			// Token: 0x0400128A RID: 4746
			public static Func<IDevice, bool> <>9__130_1;

			// Token: 0x0400128B RID: 4747
			public static Func<char, bool> <>9__144_0;

			// Token: 0x0400128C RID: 4748
			public static Func<string, bool> <>9__152_0;

			// Token: 0x0400128D RID: 4749
			public static Func<string, bool> <>9__152_1;

			// Token: 0x0400128E RID: 4750
			public static Func<CANFrame, bool> <>9__152_3;

			// Token: 0x0400128F RID: 4751
			public static Func<string, bool> <>9__153_0;

			// Token: 0x04001290 RID: 4752
			public static Func<CustomPID, bool> <>9__159_0;

			// Token: 0x04001291 RID: 4753
			public static Func<CustomPID, bool> <>9__159_1;

			// Token: 0x04001292 RID: 4754
			public static Func<string, bool> <>9__168_0;

			// Token: 0x04001293 RID: 4755
			public static Func<char, bool> <>9__170_2;

			// Token: 0x04001294 RID: 4756
			public static Func<char, bool> <>9__175_1;

			// Token: 0x04001295 RID: 4757
			public static Func<char, bool> <>9__175_2;

			// Token: 0x04001296 RID: 4758
			public static Func<string, bool> <>9__175_3;

			// Token: 0x04001297 RID: 4759
			public static Func<string, bool> <>9__175_4;

			// Token: 0x04001298 RID: 4760
			public static Func<string, string> <>9__181_1;

			// Token: 0x04001299 RID: 4761
			public static Func<string, bool> <>9__181_3;

			// Token: 0x0400129A RID: 4762
			public static Func<string, bool> <>9__181_4;

			// Token: 0x0400129B RID: 4763
			public static Predicate<string> <>9__181_5;

			// Token: 0x0400129C RID: 4764
			public static Func<string, string> <>9__182_1;

			// Token: 0x0400129D RID: 4765
			public static Func<string, bool> <>9__182_3;

			// Token: 0x0400129E RID: 4766
			public static Func<string, bool> <>9__182_4;

			// Token: 0x0400129F RID: 4767
			public static Predicate<string> <>9__182_5;

			// Token: 0x040012A0 RID: 4768
			public static Func<string, string> <>9__185_0;

			// Token: 0x040012A1 RID: 4769
			public static Func<string, string> <>9__185_2;

			// Token: 0x040012A2 RID: 4770
			public static Func<string, string> <>9__185_4;

			// Token: 0x040012A3 RID: 4771
			public static Func<PID, bool> <>9__199_0;

			// Token: 0x040012A4 RID: 4772
			public static Func<string, string> <>9__203_0;

			// Token: 0x040012A5 RID: 4773
			public static Func<string, string> <>9__204_0;

			// Token: 0x040012A6 RID: 4774
			public static Func<string, string> <>9__205_0;

			// Token: 0x040012A7 RID: 4775
			public static Func<string, bool> <>9__205_1;

			// Token: 0x040012A8 RID: 4776
			public static Func<string, string> <>9__206_0;

			// Token: 0x040012A9 RID: 4777
			public static Func<string, bool> <>9__207_1;

			// Token: 0x040012AA RID: 4778
			public static Func<string, bool> <>9__207_2;

			// Token: 0x040012AB RID: 4779
			public static Predicate<string> <>9__207_3;

			// Token: 0x040012AC RID: 4780
			public static Func<string, bool> <>9__208_1;

			// Token: 0x040012AD RID: 4781
			public static Func<string, bool> <>9__208_2;

			// Token: 0x040012AE RID: 4782
			public static Predicate<string> <>9__208_3;

			// Token: 0x040012AF RID: 4783
			public static Func<PID, bool> <>9__211_0;

			// Token: 0x040012B0 RID: 4784
			public static Func<PID, bool> <>9__211_1;

			// Token: 0x040012B1 RID: 4785
			public static Func<PID, bool> <>9__211_2;

			// Token: 0x040012B2 RID: 4786
			public static Func<PID, bool> <>9__211_3;

			// Token: 0x040012B3 RID: 4787
			public static Func<PID, string> <>9__211_4;

			// Token: 0x040012B4 RID: 4788
			public static Func<PID, bool> <>9__211_5;

			// Token: 0x040012B5 RID: 4789
			public static Func<PID, bool> <>9__213_0;

			// Token: 0x040012B6 RID: 4790
			public static Func<PID, string> <>9__213_1;

			// Token: 0x040012B7 RID: 4791
			public static Func<string, bool> <>9__213_2;
		}

		// Token: 0x02000331 RID: 817
		[CompilerGenerated]
		private sealed class <>c__DisplayClass104_0
		{
			// Token: 0x0600258B RID: 9611 RVA: 0x00002050 File Offset: 0x00000250
			public <>c__DisplayClass104_0()
			{
			}

			// Token: 0x0600258C RID: 9612 RVA: 0x001C6E8C File Offset: 0x001C508C
			internal void <OnStatusChanged>b__2()
			{
				this.Event(this.status);
			}

			// Token: 0x0600258D RID: 9613 RVA: 0x001C6E9F File Offset: 0x001C509F
			internal void <OnStatusChanged>b__3()
			{
				CarPlayManager instance = CarPlayManager.Instance;
				if (instance == null)
				{
					return;
				}
				instance.OnOBDStatusChanged(this.newStatus);
			}

			// Token: 0x040012B8 RID: 4792
			public CurrentStatusChangedEvent Event;

			// Token: 0x040012B9 RID: 4793
			public OBDDataReaderStatus status;

			// Token: 0x040012BA RID: 4794
			public OBDDataReaderStatus newStatus;
		}

		// Token: 0x02000332 RID: 818
		[CompilerGenerated]
		private sealed class <>c__DisplayClass107_0
		{
			// Token: 0x0600258E RID: 9614 RVA: 0x00002050 File Offset: 0x00000250
			public <>c__DisplayClass107_0()
			{
			}

			// Token: 0x0600258F RID: 9615 RVA: 0x001C6EB6 File Offset: 0x001C50B6
			internal bool <CheckIfRequestInQueue>b__0(OBDRequest x)
			{
				return x.Equals(this.request);
			}

			// Token: 0x040012BB RID: 4795
			public OBDRequest request;
		}

		// Token: 0x02000333 RID: 819
		[CompilerGenerated]
		private sealed class <>c__DisplayClass137_0
		{
			// Token: 0x06002590 RID: 9616 RVA: 0x00002050 File Offset: 0x00000250
			public <>c__DisplayClass137_0()
			{
			}

			// Token: 0x06002591 RID: 9617 RVA: 0x001C6EC4 File Offset: 0x001C50C4
			internal void <Start>b__0()
			{
				this.<>4__this.StartLoopV3(this.predstavsa_mraz);
			}

			// Token: 0x040012BC RID: 4796
			public OBDDataReader <>4__this;

			// Token: 0x040012BD RID: 4797
			public string predstavsa_mraz;
		}

		// Token: 0x02000334 RID: 820
		[CompilerGenerated]
		private sealed class <>c__DisplayClass143_0
		{
			// Token: 0x06002592 RID: 9618 RVA: 0x00002050 File Offset: 0x00000250
			public <>c__DisplayClass143_0()
			{
			}

			// Token: 0x040012BE RID: 4798
			public bool hasFinishCharacter;
		}

		// Token: 0x02000335 RID: 821
		[CompilerGenerated]
		private sealed class <>c__DisplayClass143_1
		{
			// Token: 0x06002593 RID: 9619 RVA: 0x00002050 File Offset: 0x00000250
			public <>c__DisplayClass143_1()
			{
			}

			// Token: 0x06002594 RID: 9620 RVA: 0x001C6ED8 File Offset: 0x001C50D8
			internal unsafe void <ReadData>b__0(Span<char> span, byte[] src)
			{
				int num = 0;
				foreach (byte b in src)
				{
					if (b != 0)
					{
						*span[num++] = (char)b;
					}
					if (b == 62)
					{
						this.CS$<>8__locals1.hasFinishCharacter = true;
					}
					if (b == 13 || b == 10)
					{
						int num2 = this.partLineCounter;
						this.partLineCounter = num2 + 1;
					}
				}
			}

			// Token: 0x040012BF RID: 4799
			public int partLineCounter;

			// Token: 0x040012C0 RID: 4800
			public OBDDataReader.<>c__DisplayClass143_0 CS$<>8__locals1;
		}

		// Token: 0x02000336 RID: 822
		[CompilerGenerated]
		private sealed class <>c__DisplayClass147_0
		{
			// Token: 0x06002595 RID: 9621 RVA: 0x00002050 File Offset: 0x00000250
			public <>c__DisplayClass147_0()
			{
			}

			// Token: 0x06002596 RID: 9622 RVA: 0x001C6F35 File Offset: 0x001C5135
			internal bool <StartLoopV3>b__0(OBDRequest x)
			{
				return x == this.request;
			}

			// Token: 0x040012C1 RID: 4801
			public OBDRequest request;

			// Token: 0x040012C2 RID: 4802
			public Predicate<OBDRequest> <>9__0;
		}

		// Token: 0x02000337 RID: 823
		[CompilerGenerated]
		private sealed class <>c__DisplayClass152_0
		{
			// Token: 0x06002597 RID: 9623 RVA: 0x00002050 File Offset: 0x00000250
			public <>c__DisplayClass152_0()
			{
			}

			// Token: 0x06002598 RID: 9624 RVA: 0x001C6F40 File Offset: 0x001C5140
			internal CANFrame <ReadDataWithManualFlowControl_CAN>b__2(string x)
			{
				return new CANFrame(x, this.request.ELMFormat, this.has_extended_address);
			}

			// Token: 0x06002599 RID: 9625 RVA: 0x001C6F40 File Offset: 0x001C5140
			internal CANFrame <ReadDataWithManualFlowControl_CAN>b__4(string x)
			{
				return new CANFrame(x, this.request.ELMFormat, this.has_extended_address);
			}

			// Token: 0x040012C3 RID: 4803
			public OBDRequest request;

			// Token: 0x040012C4 RID: 4804
			public bool has_extended_address;

			// Token: 0x040012C5 RID: 4805
			public Func<string, CANFrame> <>9__4;
		}

		// Token: 0x02000338 RID: 824
		[CompilerGenerated]
		private sealed class <>c__DisplayClass158_0
		{
			// Token: 0x0600259A RID: 9626 RVA: 0x00002050 File Offset: 0x00000250
			public <>c__DisplayClass158_0()
			{
			}

			// Token: 0x0600259B RID: 9627 RVA: 0x001C6F59 File Offset: 0x001C5159
			internal void <PushDataToTerminalPage>b__0()
			{
				if (TerminalPage.Instance != null)
				{
					TerminalPage.Instance.OnResponseReceived(this.reply);
				}
			}

			// Token: 0x040012C6 RID: 4806
			public string reply;
		}

		// Token: 0x02000339 RID: 825
		[CompilerGenerated]
		private sealed class <>c__DisplayClass170_0
		{
			// Token: 0x0600259C RID: 9628 RVA: 0x00002050 File Offset: 0x00000250
			public <>c__DisplayClass170_0()
			{
			}

			// Token: 0x0600259D RID: 9629 RVA: 0x001C6F72 File Offset: 0x001C5172
			internal bool <InitializeCustomInitString>b__1(PID x)
			{
				return x.Command == this.cmd.Command;
			}

			// Token: 0x040012C7 RID: 4807
			public OBDRequest cmd;
		}

		// Token: 0x0200033A RID: 826
		[CompilerGenerated]
		private sealed class <>c__DisplayClass175_0
		{
			// Token: 0x0600259E RID: 9630 RVA: 0x00002050 File Offset: 0x00000250
			public <>c__DisplayClass175_0()
			{
			}

			// Token: 0x0600259F RID: 9631 RVA: 0x001C6F8C File Offset: 0x001C518C
			internal async void <InitializeDefaultInitString>b__5()
			{
				await this.<>4__this.DebugWrite(string.Format("\r\n[LengthTooBig On InitializeDefaultInitString with initMode={0}]\r\n", this.initMode));
			}

			// Token: 0x040012C8 RID: 4808
			public OBDDataReader <>4__this;

			// Token: 0x040012C9 RID: 4809
			public OBDDataReader.InitModes initMode;

			// Token: 0x040012CA RID: 4810
			public Action <>9__5;

			// Token: 0x0200033B RID: 827
			[StructLayout(LayoutKind.Auto)]
			private struct <<InitializeDefaultInitString>b__5>d : IAsyncStateMachine
			{
				// Token: 0x060025A0 RID: 9632 RVA: 0x001C6FC4 File Offset: 0x001C51C4
				void IAsyncStateMachine.MoveNext()
				{
					int num2;
					int num = num2;
					OBDDataReader.<>c__DisplayClass175_0 CS$<>8__locals1 = this;
					try
					{
						TaskAwaiter taskAwaiter;
						if (num != 0)
						{
							taskAwaiter = CS$<>8__locals1.<>4__this.DebugWrite(string.Format("\r\n[LengthTooBig On InitializeDefaultInitString with initMode={0}]\r\n", CS$<>8__locals1.initMode)).GetAwaiter();
							if (!taskAwaiter.IsCompleted)
							{
								num2 = 0;
								TaskAwaiter taskAwaiter2 = taskAwaiter;
								this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, OBDDataReader.<>c__DisplayClass175_0.<<InitializeDefaultInitString>b__5>d>(ref taskAwaiter, ref this);
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
					}
					catch (Exception ex)
					{
						num2 = -2;
						this.<>t__builder.SetException(ex);
						return;
					}
					num2 = -2;
					this.<>t__builder.SetResult();
				}

				// Token: 0x060025A1 RID: 9633 RVA: 0x001C7090 File Offset: 0x001C5290
				[DebuggerHidden]
				void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
				{
					this.<>t__builder.SetStateMachine(stateMachine);
				}

				// Token: 0x040012CB RID: 4811
				public int <>1__state;

				// Token: 0x040012CC RID: 4812
				public AsyncVoidMethodBuilder <>t__builder;

				// Token: 0x040012CD RID: 4813
				public OBDDataReader.<>c__DisplayClass175_0 <>4__this;

				// Token: 0x040012CE RID: 4814
				private TaskAwaiter <>u__1;
			}
		}

		// Token: 0x0200033C RID: 828
		[CompilerGenerated]
		private sealed class <>c__DisplayClass175_1
		{
			// Token: 0x060025A2 RID: 9634 RVA: 0x00002050 File Offset: 0x00000250
			public <>c__DisplayClass175_1()
			{
			}

			// Token: 0x040012CF RID: 4815
			public string atst;
		}

		// Token: 0x0200033D RID: 829
		[CompilerGenerated]
		private sealed class <>c__DisplayClass175_2
		{
			// Token: 0x060025A3 RID: 9635 RVA: 0x00002050 File Offset: 0x00000250
			public <>c__DisplayClass175_2()
			{
			}

			// Token: 0x060025A4 RID: 9636 RVA: 0x001C70A0 File Offset: 0x001C52A0
			internal void <InitializeDefaultInitString>b__0()
			{
				int num = 72;
				if (int.TryParse(this.CS$<>8__locals1.atst, NumberStyles.HexNumber, CultureInfo.InvariantCulture.NumberFormat, out num))
				{
					if (num <= 50)
					{
						if (num == 22 || num == 50)
						{
							this.CS$<>8__locals1.atst = "08";
						}
					}
					else if (num != 100)
					{
						if (num != 150)
						{
							if (num == 255)
							{
								this.CS$<>8__locals1.atst = "96";
							}
						}
						else
						{
							this.CS$<>8__locals1.atst = "32";
						}
					}
					else
					{
						this.CS$<>8__locals1.atst = "16";
					}
					this.atstChangedBecauseOfLengthTooBigHandler = true;
				}
			}

			// Token: 0x040012D0 RID: 4816
			public bool atstChangedBecauseOfLengthTooBigHandler;

			// Token: 0x040012D1 RID: 4817
			public OBDDataReader.<>c__DisplayClass175_1 CS$<>8__locals1;
		}

		// Token: 0x0200033E RID: 830
		[CompilerGenerated]
		private sealed class <>c__DisplayClass181_0
		{
			// Token: 0x060025A5 RID: 9637 RVA: 0x00002050 File Offset: 0x00000250
			public <>c__DisplayClass181_0()
			{
			}

			// Token: 0x060025A6 RID: 9638 RVA: 0x001C7146 File Offset: 0x001C5346
			internal void <ParseECUHeaders>b__0()
			{
				this.<>4__this._ECUHeaders.Clear();
			}

			// Token: 0x060025A7 RID: 9639 RVA: 0x001C7158 File Offset: 0x001C5358
			internal bool <ParseECUHeaders>b__2(string x)
			{
				return x.Length > 3 && x.Contains(this.reply_marker);
			}

			// Token: 0x040012D2 RID: 4818
			public OBDDataReader <>4__this;

			// Token: 0x040012D3 RID: 4819
			public string reply_marker;
		}

		// Token: 0x0200033F RID: 831
		[CompilerGenerated]
		private sealed class <>c__DisplayClass181_1
		{
			// Token: 0x060025A8 RID: 9640 RVA: 0x00002050 File Offset: 0x00000250
			public <>c__DisplayClass181_1()
			{
			}

			// Token: 0x060025A9 RID: 9641 RVA: 0x001C7174 File Offset: 0x001C5374
			internal void <ParseECUHeaders>b__6()
			{
				foreach (string text in this.tHeaders)
				{
					this.CS$<>8__locals1.<>4__this._ECUHeaders.Add(new ECUHeader(text));
				}
			}

			// Token: 0x040012D4 RID: 4820
			public List<string> tHeaders;

			// Token: 0x040012D5 RID: 4821
			public OBDDataReader.<>c__DisplayClass181_0 CS$<>8__locals1;
		}

		// Token: 0x02000340 RID: 832
		[CompilerGenerated]
		private sealed class <>c__DisplayClass182_0
		{
			// Token: 0x060025AA RID: 9642 RVA: 0x00002050 File Offset: 0x00000250
			public <>c__DisplayClass182_0()
			{
			}

			// Token: 0x060025AB RID: 9643 RVA: 0x001C71DC File Offset: 0x001C53DC
			internal void <ParseECUHeadersOld>b__0()
			{
				this.<>4__this._ECUHeaders.Clear();
			}

			// Token: 0x060025AC RID: 9644 RVA: 0x001C71EE File Offset: 0x001C53EE
			internal bool <ParseECUHeadersOld>b__2(string x)
			{
				return x.Contains(this.reply_marker);
			}

			// Token: 0x060025AD RID: 9645 RVA: 0x001C71FC File Offset: 0x001C53FC
			internal void <ParseECUHeadersOld>b__6()
			{
				foreach (string text in this.tHeaders)
				{
					this.<>4__this._ECUHeaders.Add(new ECUHeader(text));
				}
			}

			// Token: 0x040012D6 RID: 4822
			public OBDDataReader <>4__this;

			// Token: 0x040012D7 RID: 4823
			public string reply_marker;

			// Token: 0x040012D8 RID: 4824
			public List<string> tHeaders;
		}

		// Token: 0x02000341 RID: 833
		[CompilerGenerated]
		private sealed class <>c__DisplayClass184_0
		{
			// Token: 0x060025AE RID: 9646 RVA: 0x00002050 File Offset: 0x00000250
			public <>c__DisplayClass184_0()
			{
			}

			// Token: 0x060025AF RID: 9647 RVA: 0x001C7260 File Offset: 0x001C5460
			internal async void <OnDisconnectDetected>b__0()
			{
				OBDRequest[] oldQueueCopy = this.old_queue;
				DependencyService.Get<IScreenKeeper>(0).LetScreenOff();
				bool flag = App.Instance != null && App.Instance.RootPage != null;
				if (flag)
				{
					flag = await App.GetCurrentPage().DisplayAlert("Car Scanner", Translate.GetString("obdreader_DisconnectedBecauseMaxAttempts_Text"), "OK", Translate.GetString("btnCancel.Content"));
				}
				if (flag)
				{
					if (SimpleMainPage.Instance == null)
					{
						await CarPlayOBDHelper.StartConnectionLight();
					}
					else
					{
						await SimpleMainPage.Instance.StartConnection(true);
					}
					if (App.OBDReader.CurrentStatus == OBDDataReaderStatus.ConnectedToECU)
					{
						App.OBDReader.ReplaceQueue(oldQueueCopy);
					}
				}
			}

			// Token: 0x040012D9 RID: 4825
			public OBDRequest[] old_queue;

			// Token: 0x02000342 RID: 834
			[StructLayout(LayoutKind.Auto)]
			private struct <<OnDisconnectDetected>b__0>d : IAsyncStateMachine
			{
				// Token: 0x060025B0 RID: 9648 RVA: 0x001C7298 File Offset: 0x001C5498
				void IAsyncStateMachine.MoveNext()
				{
					int num2;
					int num = num2;
					OBDDataReader.<>c__DisplayClass184_0 CS$<>8__locals1 = this;
					try
					{
						TaskAwaiter<bool> taskAwaiter;
						TaskAwaiter taskAwaiter3;
						bool flag;
						switch (num)
						{
						case 0:
						{
							TaskAwaiter<bool> taskAwaiter2;
							taskAwaiter = taskAwaiter2;
							taskAwaiter2 = default(TaskAwaiter<bool>);
							num2 = -1;
							break;
						}
						case 1:
						{
							TaskAwaiter taskAwaiter4;
							taskAwaiter3 = taskAwaiter4;
							taskAwaiter4 = default(TaskAwaiter);
							num2 = -1;
							goto IL_0133;
						}
						case 2:
						{
							TaskAwaiter taskAwaiter4;
							taskAwaiter3 = taskAwaiter4;
							taskAwaiter4 = default(TaskAwaiter);
							num2 = -1;
							goto IL_0195;
						}
						default:
							oldQueueCopy = CS$<>8__locals1.old_queue;
							DependencyService.Get<IScreenKeeper>(0).LetScreenOff();
							flag = App.Instance != null && App.Instance.RootPage != null;
							if (!flag)
							{
								goto IL_00D0;
							}
							taskAwaiter = App.GetCurrentPage().DisplayAlert("Car Scanner", Translate.GetString("obdreader_DisconnectedBecauseMaxAttempts_Text"), "OK", Translate.GetString("btnCancel.Content")).GetAwaiter();
							if (!taskAwaiter.IsCompleted)
							{
								num2 = 0;
								TaskAwaiter<bool> taskAwaiter2 = taskAwaiter;
								this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<bool>, OBDDataReader.<>c__DisplayClass184_0.<<OnDisconnectDetected>b__0>d>(ref taskAwaiter, ref this);
								return;
							}
							break;
						}
						flag = taskAwaiter.GetResult();
						IL_00D0:
						if (!flag)
						{
							goto IL_01B9;
						}
						if (SimpleMainPage.Instance == null)
						{
							taskAwaiter3 = CarPlayOBDHelper.StartConnectionLight().GetAwaiter();
							if (!taskAwaiter3.IsCompleted)
							{
								num2 = 1;
								TaskAwaiter taskAwaiter4 = taskAwaiter3;
								this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, OBDDataReader.<>c__DisplayClass184_0.<<OnDisconnectDetected>b__0>d>(ref taskAwaiter3, ref this);
								return;
							}
						}
						else
						{
							taskAwaiter3 = SimpleMainPage.Instance.StartConnection(true).GetAwaiter();
							if (!taskAwaiter3.IsCompleted)
							{
								num2 = 2;
								TaskAwaiter taskAwaiter4 = taskAwaiter3;
								this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, OBDDataReader.<>c__DisplayClass184_0.<<OnDisconnectDetected>b__0>d>(ref taskAwaiter3, ref this);
								return;
							}
							goto IL_0195;
						}
						IL_0133:
						taskAwaiter3.GetResult();
						goto IL_019C;
						IL_0195:
						taskAwaiter3.GetResult();
						IL_019C:
						if (App.OBDReader.CurrentStatus == OBDDataReaderStatus.ConnectedToECU)
						{
							App.OBDReader.ReplaceQueue(oldQueueCopy);
						}
						IL_01B9:;
					}
					catch (Exception ex)
					{
						num2 = -2;
						oldQueueCopy = null;
						this.<>t__builder.SetException(ex);
						return;
					}
					num2 = -2;
					oldQueueCopy = null;
					this.<>t__builder.SetResult();
				}

				// Token: 0x060025B1 RID: 9649 RVA: 0x001C74B8 File Offset: 0x001C56B8
				[DebuggerHidden]
				void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
				{
					this.<>t__builder.SetStateMachine(stateMachine);
				}

				// Token: 0x040012DA RID: 4826
				public int <>1__state;

				// Token: 0x040012DB RID: 4827
				public AsyncVoidMethodBuilder <>t__builder;

				// Token: 0x040012DC RID: 4828
				public OBDDataReader.<>c__DisplayClass184_0 <>4__this;

				// Token: 0x040012DD RID: 4829
				private OBDRequest[] <oldQueueCopy>5__2;

				// Token: 0x040012DE RID: 4830
				private TaskAwaiter<bool> <>u__1;

				// Token: 0x040012DF RID: 4831
				private TaskAwaiter <>u__2;
			}
		}

		// Token: 0x02000343 RID: 835
		[CompilerGenerated]
		private sealed class <>c__DisplayClass185_0
		{
			// Token: 0x060025B2 RID: 9650 RVA: 0x00002050 File Offset: 0x00000250
			public <>c__DisplayClass185_0()
			{
			}

			// Token: 0x060025B3 RID: 9651 RVA: 0x001C74C6 File Offset: 0x001C56C6
			internal bool <DecodeMode06Data>b__1(string x)
			{
				return x.StartsWith(this.header_filter);
			}

			// Token: 0x060025B4 RID: 9652 RVA: 0x001C74D4 File Offset: 0x001C56D4
			internal bool <DecodeMode06Data>b__3(string x)
			{
				int num = x.IndexOf(this.header_filter);
				if (num % 2 != 0)
				{
					num = x.IndexOf(this.header_filter, num + 1);
				}
				if (num >= 6 && num <= 8 && num <= x.Length - this.header_filter.Length - 2)
				{
					this.IsCAN = true;
					return true;
				}
				return num == 4 && num <= x.Length - this.header_filter.Length - 2;
			}

			// Token: 0x040012E0 RID: 4832
			public string header_filter;

			// Token: 0x040012E1 RID: 4833
			public bool IsCAN;
		}

		// Token: 0x02000344 RID: 836
		[CompilerGenerated]
		private sealed class <>c__DisplayClass187_0
		{
			// Token: 0x060025B5 RID: 9653 RVA: 0x00002050 File Offset: 0x00000250
			public <>c__DisplayClass187_0()
			{
			}

			// Token: 0x060025B6 RID: 9654 RVA: 0x001C7549 File Offset: 0x001C5749
			internal bool <DecodeCAN11bitOptimized>b__0(string x)
			{
				return x != this.header_filter;
			}

			// Token: 0x060025B7 RID: 9655 RVA: 0x001C7557 File Offset: 0x001C5757
			internal bool <DecodeCAN11bitOptimized>b__1(string x)
			{
				return x.Length > 5 && x.IndexOf(this.cmd_response, this.start_searching_response_from_position, StringComparison.Ordinal) >= 0;
			}

			// Token: 0x040012E2 RID: 4834
			public string header_filter;

			// Token: 0x040012E3 RID: 4835
			public string cmd_response;

			// Token: 0x040012E4 RID: 4836
			public int start_searching_response_from_position;
		}

		// Token: 0x02000345 RID: 837
		[CompilerGenerated]
		private sealed class <>c__DisplayClass188_0
		{
			// Token: 0x060025B8 RID: 9656 RVA: 0x00002050 File Offset: 0x00000250
			public <>c__DisplayClass188_0()
			{
			}

			// Token: 0x060025B9 RID: 9657 RVA: 0x001C757D File Offset: 0x001C577D
			internal bool <DecodeCAN11bit>b__0(string x)
			{
				return x != this.header_filter;
			}

			// Token: 0x060025BA RID: 9658 RVA: 0x001C758B File Offset: 0x001C578B
			internal bool <DecodeCAN11bit>b__1(string x)
			{
				return x != null && x.Length > 5 && x.IndexOf(this.cmd_response, this.start_searching_response_from_position, StringComparison.Ordinal) >= 0;
			}

			// Token: 0x040012E5 RID: 4837
			public string header_filter;

			// Token: 0x040012E6 RID: 4838
			public string cmd_response;

			// Token: 0x040012E7 RID: 4839
			public int start_searching_response_from_position;
		}

		// Token: 0x02000346 RID: 838
		[CompilerGenerated]
		private sealed class <>c__DisplayClass189_0
		{
			// Token: 0x060025BB RID: 9659 RVA: 0x00002050 File Offset: 0x00000250
			public <>c__DisplayClass189_0()
			{
			}

			// Token: 0x060025BC RID: 9660 RVA: 0x001C75B4 File Offset: 0x001C57B4
			internal bool <DecodeCAN29bitOptimized>b__0(string x)
			{
				return x != this.header_filter;
			}

			// Token: 0x040012E8 RID: 4840
			public string header_filter;
		}

		// Token: 0x02000347 RID: 839
		[CompilerGenerated]
		private sealed class <>c__DisplayClass191_0
		{
			// Token: 0x060025BD RID: 9661 RVA: 0x00002050 File Offset: 0x00000250
			public <>c__DisplayClass191_0()
			{
			}

			// Token: 0x060025BE RID: 9662 RVA: 0x001C75C2 File Offset: 0x001C57C2
			internal bool <DecodeCAN29bit>b__0(string x)
			{
				return x != this.header_filter;
			}

			// Token: 0x040012E9 RID: 4841
			public string header_filter;
		}

		// Token: 0x02000348 RID: 840
		[CompilerGenerated]
		private sealed class <>c__DisplayClass193_0
		{
			// Token: 0x060025BF RID: 9663 RVA: 0x00002050 File Offset: 0x00000250
			public <>c__DisplayClass193_0()
			{
			}

			// Token: 0x060025C0 RID: 9664 RVA: 0x001C75D0 File Offset: 0x001C57D0
			internal bool <DecodeKWP>b__0(string x)
			{
				return x != this.header_filter;
			}

			// Token: 0x060025C1 RID: 9665 RVA: 0x001C75DE File Offset: 0x001C57DE
			internal bool <DecodeKWP>b__1(string x)
			{
				return x.IndexOf(this.cmd_response, 6, StringComparison.Ordinal) < 0;
			}

			// Token: 0x040012EA RID: 4842
			public string header_filter;

			// Token: 0x040012EB RID: 4843
			public string cmd_response;
		}

		// Token: 0x02000349 RID: 841
		[CompilerGenerated]
		private sealed class <>c__DisplayClass197_0
		{
			// Token: 0x060025C2 RID: 9666 RVA: 0x00002050 File Offset: 0x00000250
			public <>c__DisplayClass197_0()
			{
			}

			// Token: 0x060025C3 RID: 9667 RVA: 0x001C75F4 File Offset: 0x001C57F4
			internal void <ParseATRV>b__0()
			{
				try
				{
					this.request.PIDs[0].Decode(this.bytes, this.<>4__this.stopwatch.Elapsed, this.<>4__this.ECUHeaders[this.<>4__this.SelectedECU].Id);
				}
				catch
				{
				}
			}

			// Token: 0x040012EC RID: 4844
			public OBDRequest request;

			// Token: 0x040012ED RID: 4845
			public OBDDataReader <>4__this;

			// Token: 0x040012EE RID: 4846
			public byte[] bytes;
		}

		// Token: 0x0200034A RID: 842
		[CompilerGenerated]
		private sealed class <>c__DisplayClass201_0
		{
			// Token: 0x060025C4 RID: 9668 RVA: 0x00002050 File Offset: 0x00000250
			public <>c__DisplayClass201_0()
			{
			}

			// Token: 0x060025C5 RID: 9669 RVA: 0x001C7664 File Offset: 0x001C5864
			internal void <ReadMode06>b__0()
			{
				this.<>4__this.CurrentCarData.Mode06TestCollection.Clear();
			}

			// Token: 0x040012EF RID: 4847
			public OBDDataReader <>4__this;

			// Token: 0x040012F0 RID: 4848
			public IProgress<double> progress;
		}

		// Token: 0x0200034B RID: 843
		[CompilerGenerated]
		private sealed class <>c__DisplayClass201_1
		{
			// Token: 0x060025C6 RID: 9670 RVA: 0x00002050 File Offset: 0x00000250
			public <>c__DisplayClass201_1()
			{
			}

			// Token: 0x060025C7 RID: 9671 RVA: 0x001C767B File Offset: 0x001C587B
			internal void <ReadMode06>b__1()
			{
				this.CS$<>8__locals1.progress.Report(this.p);
			}

			// Token: 0x040012F1 RID: 4849
			public double p;

			// Token: 0x040012F2 RID: 4850
			public OBDDataReader.<>c__DisplayClass201_0 CS$<>8__locals1;
		}

		// Token: 0x0200034C RID: 844
		[CompilerGenerated]
		private sealed class <>c__DisplayClass202_0
		{
			// Token: 0x060025C8 RID: 9672 RVA: 0x00002050 File Offset: 0x00000250
			public <>c__DisplayClass202_0()
			{
			}

			// Token: 0x060025C9 RID: 9673 RVA: 0x001C7693 File Offset: 0x001C5893
			internal void <ReadMode06>b__0()
			{
				this.<>4__this.CurrentCarData.Mode06TestCollection.Clear();
			}

			// Token: 0x040012F3 RID: 4851
			public OBDDataReader <>4__this;

			// Token: 0x040012F4 RID: 4852
			public IProgress<double> progress;
		}

		// Token: 0x0200034D RID: 845
		[CompilerGenerated]
		private sealed class <>c__DisplayClass202_1
		{
			// Token: 0x060025CA RID: 9674 RVA: 0x00002050 File Offset: 0x00000250
			public <>c__DisplayClass202_1()
			{
			}

			// Token: 0x060025CB RID: 9675 RVA: 0x001C76AA File Offset: 0x001C58AA
			internal void <ReadMode06>b__1()
			{
				this.CS$<>8__locals1.progress.Report(this.p);
			}

			// Token: 0x040012F5 RID: 4853
			public double p;

			// Token: 0x040012F6 RID: 4854
			public OBDDataReader.<>c__DisplayClass202_0 CS$<>8__locals1;
		}

		// Token: 0x0200034E RID: 846
		[CompilerGenerated]
		private sealed class <>c__DisplayClass204_0
		{
			// Token: 0x060025CC RID: 9676 RVA: 0x00002050 File Offset: 0x00000250
			public <>c__DisplayClass204_0()
			{
			}

			// Token: 0x060025CD RID: 9677 RVA: 0x001C76C4 File Offset: 0x001C58C4
			internal bool <DecodeDTC_CAN11bit>b__2(string x)
			{
				int num = x.IndexOf(this.header_filter, StringComparison.Ordinal);
				return num >= 0 && num <= 1;
			}

			// Token: 0x040012F7 RID: 4855
			public string header_filter;
		}

		// Token: 0x0200034F RID: 847
		[CompilerGenerated]
		private sealed class <>c__DisplayClass204_1
		{
			// Token: 0x060025CE RID: 9678 RVA: 0x00002050 File Offset: 0x00000250
			public <>c__DisplayClass204_1()
			{
			}

			// Token: 0x060025CF RID: 9679 RVA: 0x001C76EA File Offset: 0x001C58EA
			internal bool <DecodeDTC_CAN11bit>b__3(string x)
			{
				return x.Length > this.start_searching_response_from_position && x.IndexOf(this.cmd_response, this.start_searching_response_from_position, StringComparison.Ordinal) >= 0;
			}

			// Token: 0x060025D0 RID: 9680 RVA: 0x001C7715 File Offset: 0x001C5915
			internal bool <DecodeDTC_CAN11bit>b__4(string x)
			{
				return x.Length > this.start_searching_response_from_position && x.IndexOf(this.cmd_response, 5, StringComparison.Ordinal) >= 0;
			}

			// Token: 0x040012F8 RID: 4856
			public int start_searching_response_from_position;

			// Token: 0x040012F9 RID: 4857
			public string cmd_response;
		}

		// Token: 0x02000350 RID: 848
		[CompilerGenerated]
		private sealed class <>c__DisplayClass205_0
		{
			// Token: 0x060025D1 RID: 9681 RVA: 0x00002050 File Offset: 0x00000250
			public <>c__DisplayClass205_0()
			{
			}

			// Token: 0x060025D2 RID: 9682 RVA: 0x001C773B File Offset: 0x001C593B
			internal bool <DecodeDTC_CAN29bit>b__2(string x)
			{
				return x.Substring(6, 2) == this.header_filter;
			}

			// Token: 0x040012FA RID: 4858
			public string header_filter;
		}

		// Token: 0x02000351 RID: 849
		[CompilerGenerated]
		private sealed class <>c__DisplayClass206_0
		{
			// Token: 0x060025D3 RID: 9683 RVA: 0x00002050 File Offset: 0x00000250
			public <>c__DisplayClass206_0()
			{
			}

			// Token: 0x060025D4 RID: 9684 RVA: 0x001C7750 File Offset: 0x001C5950
			internal bool <DecodeDTC_KWP>b__1(string x)
			{
				return x.Length >= 6 && x.Substring(4, 2) == this.header_filter;
			}

			// Token: 0x040012FB RID: 4859
			public string header_filter;
		}

		// Token: 0x02000352 RID: 850
		[CompilerGenerated]
		private sealed class <>c__DisplayClass207_0
		{
			// Token: 0x060025D5 RID: 9685 RVA: 0x00002050 File Offset: 0x00000250
			public <>c__DisplayClass207_0()
			{
			}

			// Token: 0x060025D6 RID: 9686 RVA: 0x001C7770 File Offset: 0x001C5970
			internal bool <GetHeaders>b__0(string x)
			{
				return x.Length > 3 && x.Contains(this.reply_marker);
			}

			// Token: 0x040012FC RID: 4860
			public string reply_marker;
		}

		// Token: 0x02000353 RID: 851
		[CompilerGenerated]
		private sealed class <>c__DisplayClass208_0
		{
			// Token: 0x060025D7 RID: 9687 RVA: 0x00002050 File Offset: 0x00000250
			public <>c__DisplayClass208_0()
			{
			}

			// Token: 0x060025D8 RID: 9688 RVA: 0x001C7789 File Offset: 0x001C5989
			internal bool <GetHeadersUnknownFormat>b__0(string x)
			{
				return x.Contains(this.reply_marker);
			}

			// Token: 0x040012FD RID: 4861
			public string reply_marker;
		}

		// Token: 0x02000354 RID: 852
		[CompilerGenerated]
		private sealed class <>c__DisplayClass216_0
		{
			// Token: 0x060025D9 RID: 9689 RVA: 0x00002050 File Offset: 0x00000250
			public <>c__DisplayClass216_0()
			{
			}

			// Token: 0x060025DA RID: 9690 RVA: 0x001C7798 File Offset: 0x001C5998
			internal void <CheckECUConnectionWhileRunning>b__0(OBDRequest req, string data)
			{
				if (string.IsNullOrEmpty(data))
				{
					return;
				}
				if (string.IsNullOrEmpty(req.ResponseMarker))
				{
					return;
				}
				if (req.ResponseMarker.Length < 2)
				{
					return;
				}
				string text = req.ResponseMarker.Substring(0, 2);
				foreach (string text2 in OBDDataReader.FilterHexAndNewLineOnly(data).Split(OBDDataReader.line_splitter))
				{
					if (text2 != null && text2.Length >= 7 && text2.IndexOf(text, 5) >= 0)
					{
						this.DecodeResult = new bool?(true);
						return;
					}
				}
			}

			// Token: 0x060025DB RID: 9691 RVA: 0x001C7820 File Offset: 0x001C5A20
			internal void <CheckECUConnectionWhileRunning>b__1(OBDRequest request, byte[] data, bool decodeResult, string responseHeader)
			{
				if (this.DecodeResult.GetValueOrDefault())
				{
					return;
				}
				this.DecodeResult = new bool?(decodeResult);
			}

			// Token: 0x040012FE RID: 4862
			public bool? DecodeResult;
		}

		// Token: 0x02000355 RID: 853
		[CompilerGenerated]
		private sealed class <>c__DisplayClass92_0
		{
			// Token: 0x060025DC RID: 9692 RVA: 0x00002050 File Offset: 0x00000250
			public <>c__DisplayClass92_0()
			{
			}

			// Token: 0x060025DD RID: 9693 RVA: 0x001C783C File Offset: 0x001C5A3C
			internal void <NotifyPropertyChanged>b__0()
			{
				PropertyChangedEventHandler propertyChanged = this.<>4__this.PropertyChanged;
				if (propertyChanged == null)
				{
					return;
				}
				propertyChanged(this.<>4__this, new PropertyChangedEventArgs(this.PropertyName));
			}

			// Token: 0x040012FF RID: 4863
			public OBDDataReader <>4__this;

			// Token: 0x04001300 RID: 4864
			public string PropertyName;
		}

		// Token: 0x02000356 RID: 854
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <ChangeECU>d__215 : IAsyncStateMachine
		{
			// Token: 0x060025DE RID: 9694 RVA: 0x001C7864 File Offset: 0x001C5A64
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				OBDDataReader obddataReader = this;
				try
				{
					TaskAwaiter<bool> taskAwaiter3;
					TaskAwaiter taskAwaiter4;
					if (num != 0)
					{
						if (num == 1)
						{
							taskAwaiter3 = taskAwaiter2;
							taskAwaiter2 = default(TaskAwaiter<bool>);
							num2 = -1;
							goto IL_00C9;
						}
						taskAwaiter4 = obddataReader.Stop("ChangeECU").GetAwaiter();
						if (!taskAwaiter4.IsCompleted)
						{
							num2 = 0;
							TaskAwaiter taskAwaiter5 = taskAwaiter4;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, OBDDataReader.<ChangeECU>d__215>(ref taskAwaiter4, ref this);
							return;
						}
					}
					else
					{
						TaskAwaiter taskAwaiter5;
						taskAwaiter4 = taskAwaiter5;
						taskAwaiter5 = default(TaskAwaiter);
						num2 = -1;
					}
					taskAwaiter4.GetResult();
					taskAwaiter3 = obddataReader.CheckSupportedPIDsV2().GetAwaiter();
					if (!taskAwaiter3.IsCompleted)
					{
						num2 = 1;
						taskAwaiter2 = taskAwaiter3;
						this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<bool>, OBDDataReader.<ChangeECU>d__215>(ref taskAwaiter3, ref this);
						return;
					}
					IL_00C9:
					if (!taskAwaiter3.GetResult())
					{
						obddataReader.OnDisconnectDetected(OBDDataReader.DisconnectReason.ELMStuck);
					}
					else
					{
						LiveDataPIDModel.UpdatePIDCollection(obddataReader);
						obddataReader.Start("FromChangeECU");
					}
				}
				catch (Exception ex)
				{
					num2 = -2;
					this.<>t__builder.SetException(ex);
					return;
				}
				num2 = -2;
				this.<>t__builder.SetResult();
			}

			// Token: 0x060025DF RID: 9695 RVA: 0x001C799C File Offset: 0x001C5B9C
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x04001301 RID: 4865
			public int <>1__state;

			// Token: 0x04001302 RID: 4866
			public AsyncTaskMethodBuilder <>t__builder;

			// Token: 0x04001303 RID: 4867
			public OBDDataReader <>4__this;

			// Token: 0x04001304 RID: 4868
			private TaskAwaiter <>u__1;

			// Token: 0x04001305 RID: 4869
			private TaskAwaiter<bool> <>u__2;
		}

		// Token: 0x02000357 RID: 855
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <CheckECUConnectionWhileRunning>d__216 : IAsyncStateMachine
		{
			// Token: 0x060025E0 RID: 9696 RVA: 0x001C79AC File Offset: 0x001C5BAC
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				OBDDataReader obddataReader = this;
				bool flag2;
				try
				{
					TaskAwaiter taskAwaiter;
					TaskAwaiter<bool> taskAwaiter3;
					switch (num)
					{
					case 0:
					{
						TaskAwaiter taskAwaiter2;
						taskAwaiter = taskAwaiter2;
						taskAwaiter2 = default(TaskAwaiter);
						num2 = -1;
						break;
					}
					case 1:
					{
						TaskAwaiter taskAwaiter2;
						taskAwaiter = taskAwaiter2;
						taskAwaiter2 = default(TaskAwaiter);
						num2 = -1;
						goto IL_0202;
					}
					case 2:
					{
						TaskAwaiter<bool> taskAwaiter4;
						taskAwaiter3 = taskAwaiter4;
						taskAwaiter4 = default(TaskAwaiter<bool>);
						num2 = -1;
						goto IL_028C;
					}
					default:
						CS$<>8__locals1 = new OBDDataReader.<>c__DisplayClass216_0();
						if (SharedSettings.Current.UseDefaultInit)
						{
							if (obddataReader.IsNissanConsult2Protocol)
							{
								checkECUCommand = new OBDRequest("221201", "", "", "", false);
							}
							else
							{
								checkECUCommand = new OBDRequest(SharedSettings.Current.Mode01Prefix + "00", "", "", "", false);
							}
						}
						else if (string.IsNullOrEmpty(SharedSettings.Current.DetectECUConnectionPID))
						{
							checkECUCommand = obddataReader.GetDefaultPidRequest();
						}
						else
						{
							checkECUCommand = new OBDRequest(SharedSettings.Current.DetectECUConnectionPID, obddataReader.GetDefaultHeader(), false);
						}
						CS$<>8__locals1.DecodeResult = null;
						checkECUCommand.ResponseReceived += delegate(OBDRequest req, string data)
						{
							if (string.IsNullOrEmpty(data))
							{
								return;
							}
							if (string.IsNullOrEmpty(req.ResponseMarker))
							{
								return;
							}
							if (req.ResponseMarker.Length < 2)
							{
								return;
							}
							string text = req.ResponseMarker.Substring(0, 2);
							foreach (string text2 in OBDDataReader.FilterHexAndNewLineOnly(data).Split(OBDDataReader.line_splitter))
							{
								if (text2 != null && text2.Length >= 7 && text2.IndexOf(text, 5) >= 0)
								{
									CS$<>8__locals1.DecodeResult = new bool?(true);
									return;
								}
							}
						};
						checkECUCommand.ResponseDecoded += delegate(OBDRequest request, byte[] data, bool decodeResult, string responseHeader)
						{
							if (CS$<>8__locals1.DecodeResult.GetValueOrDefault())
							{
								return;
							}
							CS$<>8__locals1.DecodeResult = new bool?(decodeResult);
						};
						taskAwaiter = Task.Delay(200).GetAwaiter();
						if (!taskAwaiter.IsCompleted)
						{
							num2 = 0;
							TaskAwaiter taskAwaiter2 = taskAwaiter;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, OBDDataReader.<CheckECUConnectionWhileRunning>d__216>(ref taskAwaiter, ref this);
							return;
						}
						break;
					}
					taskAwaiter.GetResult();
					if (ForceSendHeader)
					{
						obddataReader.ELM327_LastSentHeader = obddataReader.GetDefaultHeader();
					}
					App.OBDReader.ReplaceQueue(new List<OBDRequest>(1) { checkECUCommand });
					goto IL_0209;
					IL_0202:
					taskAwaiter.GetResult();
					IL_0209:
					bool flag;
					if (CS$<>8__locals1.DecodeResult != null)
					{
						flag = CS$<>8__locals1.DecodeResult.Value;
						if (flag || ForceSendHeader)
						{
							goto IL_0294;
						}
						taskAwaiter3 = obddataReader.CheckECUConnectionWhileRunning(true).GetAwaiter();
						if (!taskAwaiter3.IsCompleted)
						{
							num2 = 2;
							TaskAwaiter<bool> taskAwaiter4 = taskAwaiter3;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<bool>, OBDDataReader.<CheckECUConnectionWhileRunning>d__216>(ref taskAwaiter3, ref this);
							return;
						}
					}
					else
					{
						taskAwaiter = Task.Delay(50).GetAwaiter();
						if (!taskAwaiter.IsCompleted)
						{
							num2 = 1;
							TaskAwaiter taskAwaiter2 = taskAwaiter;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, OBDDataReader.<CheckECUConnectionWhileRunning>d__216>(ref taskAwaiter, ref this);
							return;
						}
						goto IL_0202;
					}
					IL_028C:
					flag = taskAwaiter3.GetResult();
					IL_0294:
					flag2 = flag;
				}
				catch (Exception ex)
				{
					num2 = -2;
					CS$<>8__locals1 = null;
					checkECUCommand = null;
					this.<>t__builder.SetException(ex);
					return;
				}
				num2 = -2;
				CS$<>8__locals1 = null;
				checkECUCommand = null;
				this.<>t__builder.SetResult(flag2);
			}

			// Token: 0x060025E1 RID: 9697 RVA: 0x001C7CB8 File Offset: 0x001C5EB8
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x04001306 RID: 4870
			public int <>1__state;

			// Token: 0x04001307 RID: 4871
			public AsyncTaskMethodBuilder<bool> <>t__builder;

			// Token: 0x04001308 RID: 4872
			public OBDDataReader <>4__this;

			// Token: 0x04001309 RID: 4873
			public bool ForceSendHeader;

			// Token: 0x0400130A RID: 4874
			private OBDDataReader.<>c__DisplayClass216_0 <>8__1;

			// Token: 0x0400130B RID: 4875
			private OBDRequest <checkECUCommand>5__2;

			// Token: 0x0400130C RID: 4876
			private TaskAwaiter <>u__1;

			// Token: 0x0400130D RID: 4877
			private TaskAwaiter<bool> <>u__2;
		}

		// Token: 0x02000358 RID: 856
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <CheckIsELM327>d__133 : IAsyncStateMachine
		{
			// Token: 0x060025E2 RID: 9698 RVA: 0x001C7CC8 File Offset: 0x001C5EC8
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				OBDDataReader obddataReader = this;
				bool flag;
				try
				{
					try
					{
						TaskAwaiter<string> taskAwaiter3;
						TaskAwaiter taskAwaiter4;
						if (num != 0)
						{
							if (num == 1)
							{
								taskAwaiter3 = taskAwaiter2;
								taskAwaiter2 = default(TaskAwaiter<string>);
								num2 = -1;
								goto IL_00DC;
							}
							if (SharedSettings.Current.NoDelayELM327Init)
							{
								goto IL_0088;
							}
							taskAwaiter4 = Task.Delay(500).GetAwaiter();
							if (!taskAwaiter4.IsCompleted)
							{
								num2 = 0;
								TaskAwaiter taskAwaiter5 = taskAwaiter4;
								this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, OBDDataReader.<CheckIsELM327>d__133>(ref taskAwaiter4, ref this);
								return;
							}
						}
						else
						{
							TaskAwaiter taskAwaiter5;
							taskAwaiter4 = taskAwaiter5;
							taskAwaiter5 = default(TaskAwaiter);
							num2 = -1;
						}
						taskAwaiter4.GetResult();
						IL_0088:
						taskAwaiter3 = obddataReader.SendATZ().GetAwaiter();
						if (!taskAwaiter3.IsCompleted)
						{
							num2 = 1;
							taskAwaiter2 = taskAwaiter3;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<string>, OBDDataReader.<CheckIsELM327>d__133>(ref taskAwaiter3, ref this);
							return;
						}
						IL_00DC:
						if (taskAwaiter3.GetResult().Contains('>'))
						{
							flag = true;
						}
						else
						{
							flag = false;
						}
					}
					catch (Exception)
					{
						flag = false;
					}
				}
				catch (Exception ex)
				{
					num2 = -2;
					this.<>t__builder.SetException(ex);
					return;
				}
				num2 = -2;
				this.<>t__builder.SetResult(flag);
			}

			// Token: 0x060025E3 RID: 9699 RVA: 0x001C7E18 File Offset: 0x001C6018
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x0400130E RID: 4878
			public int <>1__state;

			// Token: 0x0400130F RID: 4879
			public AsyncTaskMethodBuilder<bool> <>t__builder;

			// Token: 0x04001310 RID: 4880
			public OBDDataReader <>4__this;

			// Token: 0x04001311 RID: 4881
			private TaskAwaiter <>u__1;

			// Token: 0x04001312 RID: 4882
			private TaskAwaiter<string> <>u__2;
		}

		// Token: 0x02000359 RID: 857
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <CheckIsSTCommandsSupported>d__134 : IAsyncStateMachine
		{
			// Token: 0x060025E4 RID: 9700 RVA: 0x001C7E28 File Offset: 0x001C6028
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				OBDDataReader obddataReader = this;
				bool flag;
				try
				{
					TaskAwaiter taskAwaiter;
					TaskAwaiter<string> taskAwaiter3;
					switch (num)
					{
					case 0:
					{
						TaskAwaiter taskAwaiter2;
						taskAwaiter = taskAwaiter2;
						taskAwaiter2 = default(TaskAwaiter);
						num2 = -1;
						break;
					}
					case 1:
					{
						TaskAwaiter taskAwaiter2;
						taskAwaiter = taskAwaiter2;
						taskAwaiter2 = default(TaskAwaiter);
						num2 = -1;
						goto IL_018C;
					}
					case 2:
					{
						TaskAwaiter taskAwaiter2;
						taskAwaiter = taskAwaiter2;
						taskAwaiter2 = default(TaskAwaiter);
						num2 = -1;
						goto IL_01F6;
					}
					case 3:
					{
						TaskAwaiter<string> taskAwaiter4;
						taskAwaiter3 = taskAwaiter4;
						taskAwaiter4 = default(TaskAwaiter<string>);
						num2 = -1;
						goto IL_025B;
					}
					case 4:
					{
						TaskAwaiter taskAwaiter2;
						taskAwaiter = taskAwaiter2;
						taskAwaiter2 = default(TaskAwaiter);
						num2 = -1;
						goto IL_02F3;
					}
					case 5:
					{
						TaskAwaiter taskAwaiter2;
						taskAwaiter = taskAwaiter2;
						taskAwaiter2 = default(TaskAwaiter);
						num2 = -1;
						goto IL_0357;
					}
					default:
						if (SharedSettings.Current.ConnectionType == ConnectionTypes.Bluetooth && SharedSettings.Current.BTDeviceName != null && (SharedSettings.Current.BTDeviceName.Contains("OBDLink", StringComparison.InvariantCultureIgnoreCase) || SharedSettings.Current.BTDeviceName.Contains("vlinker", StringComparison.InvariantCultureIgnoreCase)))
						{
							taskAwaiter = obddataReader.DebugWrite("\r\n[STSupported=True(ByName)]").GetAwaiter();
							if (!taskAwaiter.IsCompleted)
							{
								num2 = 0;
								TaskAwaiter taskAwaiter2 = taskAwaiter;
								this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, OBDDataReader.<CheckIsSTCommandsSupported>d__134>(ref taskAwaiter, ref this);
								return;
							}
						}
						else if (SharedSettings.Current.ConnectionType == ConnectionTypes.BluetoothLE && SharedSettings.Current.BTLEDeviceName != null && (SharedSettings.Current.BTLEDeviceName.Contains("OBDLink", StringComparison.InvariantCultureIgnoreCase) || SharedSettings.Current.BTLEDeviceName.Contains("vlinker", StringComparison.InvariantCultureIgnoreCase)))
						{
							taskAwaiter = obddataReader.DebugWrite("\r\n[STSupported=True(ByName)]").GetAwaiter();
							if (!taskAwaiter.IsCompleted)
							{
								num2 = 1;
								TaskAwaiter taskAwaiter2 = taskAwaiter;
								this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, OBDDataReader.<CheckIsSTCommandsSupported>d__134>(ref taskAwaiter, ref this);
								return;
							}
							goto IL_018C;
						}
						else
						{
							taskAwaiter = obddataReader.SendString("STI").GetAwaiter();
							if (!taskAwaiter.IsCompleted)
							{
								num2 = 2;
								TaskAwaiter taskAwaiter2 = taskAwaiter;
								this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, OBDDataReader.<CheckIsSTCommandsSupported>d__134>(ref taskAwaiter, ref this);
								return;
							}
							goto IL_01F6;
						}
						break;
					}
					taskAwaiter.GetResult();
					flag = true;
					goto IL_037B;
					IL_018C:
					taskAwaiter.GetResult();
					flag = true;
					goto IL_037B;
					IL_01F6:
					taskAwaiter.GetResult();
					taskAwaiter3 = obddataReader.ReadData(2500, null, -1).GetAwaiter();
					if (!taskAwaiter3.IsCompleted)
					{
						num2 = 3;
						TaskAwaiter<string> taskAwaiter4 = taskAwaiter3;
						this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<string>, OBDDataReader.<CheckIsSTCommandsSupported>d__134>(ref taskAwaiter3, ref this);
						return;
					}
					IL_025B:
					string result = taskAwaiter3.GetResult();
					if (result != null && (result.Contains("STN") || result.Contains('.')) && !result.Contains('?') && !result.Contains("ELM"))
					{
						taskAwaiter = obddataReader.DebugWrite("\r\n[STSupported=True(ByResponse)]").GetAwaiter();
						if (!taskAwaiter.IsCompleted)
						{
							num2 = 4;
							TaskAwaiter taskAwaiter2 = taskAwaiter;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, OBDDataReader.<CheckIsSTCommandsSupported>d__134>(ref taskAwaiter, ref this);
							return;
						}
					}
					else
					{
						taskAwaiter = obddataReader.DebugWrite("\r\n[STSupported=False(ByResponse)]").GetAwaiter();
						if (!taskAwaiter.IsCompleted)
						{
							num2 = 5;
							TaskAwaiter taskAwaiter2 = taskAwaiter;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, OBDDataReader.<CheckIsSTCommandsSupported>d__134>(ref taskAwaiter, ref this);
							return;
						}
						goto IL_0357;
					}
					IL_02F3:
					taskAwaiter.GetResult();
					flag = true;
					goto IL_037B;
					IL_0357:
					taskAwaiter.GetResult();
					flag = false;
				}
				catch (Exception ex)
				{
					num2 = -2;
					this.<>t__builder.SetException(ex);
					return;
				}
				IL_037B:
				num2 = -2;
				this.<>t__builder.SetResult(flag);
			}

			// Token: 0x060025E5 RID: 9701 RVA: 0x001C81E0 File Offset: 0x001C63E0
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x04001313 RID: 4883
			public int <>1__state;

			// Token: 0x04001314 RID: 4884
			public AsyncValueTaskMethodBuilder<bool> <>t__builder;

			// Token: 0x04001315 RID: 4885
			public OBDDataReader <>4__this;

			// Token: 0x04001316 RID: 4886
			private TaskAwaiter <>u__1;

			// Token: 0x04001317 RID: 4887
			private TaskAwaiter<string> <>u__2;
		}

		// Token: 0x0200035A RID: 858
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <CheckIsVTCommandsSupported>d__135 : IAsyncStateMachine
		{
			// Token: 0x060025E6 RID: 9702 RVA: 0x001C81F0 File Offset: 0x001C63F0
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				OBDDataReader obddataReader = this;
				bool flag;
				try
				{
					TaskAwaiter taskAwaiter;
					TaskAwaiter<string> taskAwaiter3;
					switch (num)
					{
					case 0:
					{
						TaskAwaiter taskAwaiter2;
						taskAwaiter = taskAwaiter2;
						taskAwaiter2 = default(TaskAwaiter);
						num2 = -1;
						break;
					}
					case 1:
					{
						TaskAwaiter taskAwaiter2;
						taskAwaiter = taskAwaiter2;
						taskAwaiter2 = default(TaskAwaiter);
						num2 = -1;
						goto IL_015E;
					}
					case 2:
					{
						TaskAwaiter taskAwaiter2;
						taskAwaiter = taskAwaiter2;
						taskAwaiter2 = default(TaskAwaiter);
						num2 = -1;
						goto IL_01C8;
					}
					case 3:
					{
						TaskAwaiter<string> taskAwaiter4;
						taskAwaiter3 = taskAwaiter4;
						taskAwaiter4 = default(TaskAwaiter<string>);
						num2 = -1;
						goto IL_022D;
					}
					case 4:
					{
						TaskAwaiter taskAwaiter2;
						taskAwaiter = taskAwaiter2;
						taskAwaiter2 = default(TaskAwaiter);
						num2 = -1;
						goto IL_02B8;
					}
					case 5:
					{
						TaskAwaiter taskAwaiter2;
						taskAwaiter = taskAwaiter2;
						taskAwaiter2 = default(TaskAwaiter);
						num2 = -1;
						goto IL_031C;
					}
					default:
						if (SharedSettings.Current.ConnectionType == ConnectionTypes.Bluetooth && SharedSettings.Current.BTDeviceName != null && SharedSettings.Current.BTDeviceName.Contains("vlinker", StringComparison.InvariantCultureIgnoreCase))
						{
							taskAwaiter = obddataReader.DebugWrite("\r\n[VTSupported=True(ByName)]").GetAwaiter();
							if (!taskAwaiter.IsCompleted)
							{
								num2 = 0;
								TaskAwaiter taskAwaiter2 = taskAwaiter;
								this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, OBDDataReader.<CheckIsVTCommandsSupported>d__135>(ref taskAwaiter, ref this);
								return;
							}
						}
						else if (SharedSettings.Current.ConnectionType == ConnectionTypes.BluetoothLE && SharedSettings.Current.BTLEDeviceName != null && SharedSettings.Current.BTLEDeviceName.Contains("vlinker", StringComparison.InvariantCultureIgnoreCase))
						{
							taskAwaiter = obddataReader.DebugWrite("\r\n[VTSupported=True(ByName)]").GetAwaiter();
							if (!taskAwaiter.IsCompleted)
							{
								num2 = 1;
								TaskAwaiter taskAwaiter2 = taskAwaiter;
								this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, OBDDataReader.<CheckIsVTCommandsSupported>d__135>(ref taskAwaiter, ref this);
								return;
							}
							goto IL_015E;
						}
						else
						{
							taskAwaiter = obddataReader.SendString("VTI").GetAwaiter();
							if (!taskAwaiter.IsCompleted)
							{
								num2 = 2;
								TaskAwaiter taskAwaiter2 = taskAwaiter;
								this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, OBDDataReader.<CheckIsVTCommandsSupported>d__135>(ref taskAwaiter, ref this);
								return;
							}
							goto IL_01C8;
						}
						break;
					}
					taskAwaiter.GetResult();
					flag = true;
					goto IL_0340;
					IL_015E:
					taskAwaiter.GetResult();
					flag = true;
					goto IL_0340;
					IL_01C8:
					taskAwaiter.GetResult();
					taskAwaiter3 = obddataReader.ReadData(2500, null, -1).GetAwaiter();
					if (!taskAwaiter3.IsCompleted)
					{
						num2 = 3;
						TaskAwaiter<string> taskAwaiter4 = taskAwaiter3;
						this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<string>, OBDDataReader.<CheckIsVTCommandsSupported>d__135>(ref taskAwaiter3, ref this);
						return;
					}
					IL_022D:
					string result = taskAwaiter3.GetResult();
					if (result != null && result.Contains('.') && !result.Contains('?') && !result.Contains("ELM"))
					{
						taskAwaiter = obddataReader.DebugWrite("\r\n[VTSupported=True(ByResponse)]").GetAwaiter();
						if (!taskAwaiter.IsCompleted)
						{
							num2 = 4;
							TaskAwaiter taskAwaiter2 = taskAwaiter;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, OBDDataReader.<CheckIsVTCommandsSupported>d__135>(ref taskAwaiter, ref this);
							return;
						}
					}
					else
					{
						taskAwaiter = obddataReader.DebugWrite("\r\n[VTSupported=False(ByResponse)]").GetAwaiter();
						if (!taskAwaiter.IsCompleted)
						{
							num2 = 5;
							TaskAwaiter taskAwaiter2 = taskAwaiter;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, OBDDataReader.<CheckIsVTCommandsSupported>d__135>(ref taskAwaiter, ref this);
							return;
						}
						goto IL_031C;
					}
					IL_02B8:
					taskAwaiter.GetResult();
					flag = true;
					goto IL_0340;
					IL_031C:
					taskAwaiter.GetResult();
					flag = false;
				}
				catch (Exception ex)
				{
					num2 = -2;
					this.<>t__builder.SetException(ex);
					return;
				}
				IL_0340:
				num2 = -2;
				this.<>t__builder.SetResult(flag);
			}

			// Token: 0x060025E7 RID: 9703 RVA: 0x001C8570 File Offset: 0x001C6770
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x04001318 RID: 4888
			public int <>1__state;

			// Token: 0x04001319 RID: 4889
			public AsyncValueTaskMethodBuilder<bool> <>t__builder;

			// Token: 0x0400131A RID: 4890
			public OBDDataReader <>4__this;

			// Token: 0x0400131B RID: 4891
			private TaskAwaiter <>u__1;

			// Token: 0x0400131C RID: 4892
			private TaskAwaiter<string> <>u__2;
		}

		// Token: 0x0200035B RID: 859
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <CheckPidsByTestings>d__214 : IAsyncStateMachine
		{
			// Token: 0x060025E8 RID: 9704 RVA: 0x001C8580 File Offset: 0x001C6780
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				OBDDataReader obddataReader = this;
				try
				{
					if (num > 2)
					{
						enumerator = pidList.GetEnumerator();
					}
					try
					{
						TaskAwaiter taskAwaiter;
						TaskAwaiter<string> taskAwaiter3;
						ValueTaskAwaiter<bool> valueTaskAwaiter;
						switch (num)
						{
						case 0:
						{
							TaskAwaiter taskAwaiter2;
							taskAwaiter = taskAwaiter2;
							taskAwaiter2 = default(TaskAwaiter);
							num = (num2 = -1);
							break;
						}
						case 1:
						{
							TaskAwaiter<string> taskAwaiter4;
							taskAwaiter3 = taskAwaiter4;
							taskAwaiter4 = default(TaskAwaiter<string>);
							num = (num2 = -1);
							goto IL_0116;
						}
						case 2:
						{
							ValueTaskAwaiter<bool> valueTaskAwaiter2;
							valueTaskAwaiter = valueTaskAwaiter2;
							valueTaskAwaiter2 = default(ValueTaskAwaiter<bool>);
							num = (num2 = -1);
							goto IL_0181;
						}
						default:
							goto IL_0231;
						}
						IL_00B1:
						taskAwaiter.GetResult();
						taskAwaiter3 = obddataReader.ReadData(2500, null, -1).GetAwaiter();
						if (!taskAwaiter3.IsCompleted)
						{
							num = (num2 = 1);
							TaskAwaiter<string> taskAwaiter4 = taskAwaiter3;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<string>, OBDDataReader.<CheckPidsByTestings>d__214>(ref taskAwaiter3, ref this);
							return;
						}
						IL_0116:
						string result = taskAwaiter3.GetResult();
						valueTaskAwaiter = obddataReader.DecodeData(result, req, null).GetAwaiter();
						if (!valueTaskAwaiter.IsCompleted)
						{
							num = (num2 = 2);
							ValueTaskAwaiter<bool> valueTaskAwaiter2 = valueTaskAwaiter;
							this.<>t__builder.AwaitUnsafeOnCompleted<ValueTaskAwaiter<bool>, OBDDataReader.<CheckPidsByTestings>d__214>(ref valueTaskAwaiter, ref this);
							return;
						}
						IL_0181:
						valueTaskAwaiter.GetResult();
						IEnumerator<PID> enumerator2 = req.PIDs.GetEnumerator();
						try
						{
							while (enumerator2.MoveNext())
							{
								PID pid = enumerator2.Current;
								if (pid is IPIDFloatValue && double.IsNaN((pid as IPIDFloatValue).Value))
								{
									pid.IsAvailable = false;
									LiveDataPIDModel._PIDCollection.Remove(pid);
								}
								else if (pid is IPIDWithStringValue && (pid as IPIDWithStringValue).Value == "n/a")
								{
									pid.IsAvailable = false;
									LiveDataPIDModel._PIDCollection.Remove(pid);
								}
							}
						}
						finally
						{
							if (num < 0 && enumerator2 != null)
							{
								enumerator2.Dispose();
							}
						}
						req = null;
						IL_0231:
						if (enumerator.MoveNext())
						{
							string text = enumerator.Current;
							req = new OBDRequest(text, false);
							taskAwaiter = obddataReader.SendRequest(req).GetAwaiter();
							if (!taskAwaiter.IsCompleted)
							{
								num = (num2 = 0);
								TaskAwaiter taskAwaiter2 = taskAwaiter;
								this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, OBDDataReader.<CheckPidsByTestings>d__214>(ref taskAwaiter, ref this);
								return;
							}
							goto IL_00B1;
						}
					}
					finally
					{
						if (num < 0)
						{
							((IDisposable)enumerator).Dispose();
						}
					}
					enumerator = default(List<string>.Enumerator);
				}
				catch (Exception ex)
				{
					num2 = -2;
					this.<>t__builder.SetException(ex);
					return;
				}
				num2 = -2;
				this.<>t__builder.SetResult();
			}

			// Token: 0x060025E9 RID: 9705 RVA: 0x001C886C File Offset: 0x001C6A6C
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x0400131D RID: 4893
			public int <>1__state;

			// Token: 0x0400131E RID: 4894
			public AsyncTaskMethodBuilder <>t__builder;

			// Token: 0x0400131F RID: 4895
			public List<string> pidList;

			// Token: 0x04001320 RID: 4896
			public OBDDataReader <>4__this;

			// Token: 0x04001321 RID: 4897
			private List<string>.Enumerator <>7__wrap1;

			// Token: 0x04001322 RID: 4898
			private OBDRequest <req>5__3;

			// Token: 0x04001323 RID: 4899
			private TaskAwaiter <>u__1;

			// Token: 0x04001324 RID: 4900
			private TaskAwaiter<string> <>u__2;

			// Token: 0x04001325 RID: 4901
			private ValueTaskAwaiter<bool> <>u__3;
		}

		// Token: 0x0200035C RID: 860
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <CheckSupportedPIDsClassicTestV2>d__212 : IAsyncStateMachine
		{
			// Token: 0x060025EA RID: 9706 RVA: 0x001C887C File Offset: 0x001C6A7C
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				OBDDataReader obddataReader = this;
				try
				{
					TaskAwaiter taskAwaiter;
					TaskAwaiter<string> taskAwaiter3;
					switch (num)
					{
					case 0:
					{
						TaskAwaiter taskAwaiter2;
						taskAwaiter = taskAwaiter2;
						taskAwaiter2 = default(TaskAwaiter);
						num = (num2 = -1);
						goto IL_00F0;
					}
					case 1:
					{
						TaskAwaiter<string> taskAwaiter4;
						taskAwaiter3 = taskAwaiter4;
						taskAwaiter4 = default(TaskAwaiter<string>);
						num = (num2 = -1);
						goto IL_0155;
					}
					case 2:
						IL_01A6:
						try
						{
							if (num != 2)
							{
								goto IL_0252;
							}
							ValueTaskAwaiter<bool> valueTaskAwaiter2;
							ValueTaskAwaiter<bool> valueTaskAwaiter = valueTaskAwaiter2;
							valueTaskAwaiter2 = default(ValueTaskAwaiter<bool>);
							num = (num2 = -1);
							IL_0226:
							valueTaskAwaiter.GetResult();
							if (pid.Value[pid.Value.Length - 1])
							{
								finished = false;
							}
							IL_0252:
							if (enumerator.MoveNext())
							{
								string text = enumerator.Current;
								valueTaskAwaiter = obddataReader.DecodeData(data, req, text).GetAwaiter();
								if (!valueTaskAwaiter.IsCompleted)
								{
									num = (num2 = 2);
									valueTaskAwaiter2 = valueTaskAwaiter;
									this.<>t__builder.AwaitUnsafeOnCompleted<ValueTaskAwaiter<bool>, OBDDataReader.<CheckSupportedPIDsClassicTestV2>d__212>(ref valueTaskAwaiter, ref this);
									return;
								}
								goto IL_0226;
							}
						}
						finally
						{
							if (num < 0)
							{
								((IDisposable)enumerator).Dispose();
							}
						}
						enumerator = default(List<string>.Enumerator);
						idx += 32;
						req = null;
						data = null;
						if (finished)
						{
							goto IL_02D0;
						}
						break;
					default:
						finished = false;
						idx = 0;
						pid = null;
						break;
					}
					string text2 = string.Format(pid_template, idx.ToString("X2"));
					pid = new PID_SupportedPids(text2, obddataReader.CurrentCarData.LiveDataPIDs, requestHeader);
					req = new OBDRequest(text2, requestHeader, beforeCommands, afterCommands, false, pid);
					taskAwaiter = obddataReader.SendRequest(req).GetAwaiter();
					if (!taskAwaiter.IsCompleted)
					{
						num = (num2 = 0);
						TaskAwaiter taskAwaiter2 = taskAwaiter;
						this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, OBDDataReader.<CheckSupportedPIDsClassicTestV2>d__212>(ref taskAwaiter, ref this);
						return;
					}
					IL_00F0:
					taskAwaiter.GetResult();
					taskAwaiter3 = obddataReader.ReadData(2500, null, -1).GetAwaiter();
					if (!taskAwaiter3.IsCompleted)
					{
						num = (num2 = 1);
						TaskAwaiter<string> taskAwaiter4 = taskAwaiter3;
						this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<string>, OBDDataReader.<CheckSupportedPIDsClassicTestV2>d__212>(ref taskAwaiter3, ref this);
						return;
					}
					IL_0155:
					string result = taskAwaiter3.GetResult();
					data = result;
					string[] array = OBDDataReader.FilterHexAndNewLineOnly(data).Split(OBDDataReader.line_splitter, StringSplitOptions.RemoveEmptyEntries);
					List<string> headers = obddataReader.GetHeaders(req.ResponseMarker, array, ELMFormat.Unknown);
					finished = true;
					enumerator = headers.GetEnumerator();
					goto IL_01A6;
				}
				catch (Exception ex)
				{
					num2 = -2;
					pid = null;
					this.<>t__builder.SetException(ex);
					return;
				}
				IL_02D0:
				num2 = -2;
				pid = null;
				this.<>t__builder.SetResult();
			}

			// Token: 0x060025EB RID: 9707 RVA: 0x001C8BA8 File Offset: 0x001C6DA8
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x04001326 RID: 4902
			public int <>1__state;

			// Token: 0x04001327 RID: 4903
			public AsyncTaskMethodBuilder <>t__builder;

			// Token: 0x04001328 RID: 4904
			public string pid_template;

			// Token: 0x04001329 RID: 4905
			public OBDDataReader <>4__this;

			// Token: 0x0400132A RID: 4906
			public string requestHeader;

			// Token: 0x0400132B RID: 4907
			public string beforeCommands;

			// Token: 0x0400132C RID: 4908
			public string afterCommands;

			// Token: 0x0400132D RID: 4909
			private bool <finished>5__2;

			// Token: 0x0400132E RID: 4910
			private int <idx>5__3;

			// Token: 0x0400132F RID: 4911
			private PID_SupportedPids <pid>5__4;

			// Token: 0x04001330 RID: 4912
			private OBDRequest <req>5__5;

			// Token: 0x04001331 RID: 4913
			private string <data>5__6;

			// Token: 0x04001332 RID: 4914
			private TaskAwaiter <>u__1;

			// Token: 0x04001333 RID: 4915
			private TaskAwaiter<string> <>u__2;

			// Token: 0x04001334 RID: 4916
			private List<string>.Enumerator <>7__wrap6;

			// Token: 0x04001335 RID: 4917
			private ValueTaskAwaiter<bool> <>u__3;
		}

		// Token: 0x0200035D RID: 861
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <CheckSupportedPIDsScanTest>d__213 : IAsyncStateMachine
		{
			// Token: 0x060025EC RID: 9708 RVA: 0x001C8BB8 File Offset: 0x001C6DB8
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				OBDDataReader obddataReader = this;
				try
				{
					TaskAwaiter taskAwaiter2;
					TaskAwaiter taskAwaiter;
					if (num != 0)
					{
						if (num == 1)
						{
							taskAwaiter = taskAwaiter2;
							taskAwaiter2 = default(TaskAwaiter);
							num = (num2 = -1);
							goto IL_0214;
						}
						obddataReader.CurrentMode = OBDDataReader.OBDModes.Mode06;
						obj = null;
						int num3 = 0;
					}
					try
					{
						if (num != 0)
						{
							List<PID>.Enumerator enumerator = obddataReader.CurrentCarData.LiveDataPIDs.GetEnumerator();
							try
							{
								while (enumerator.MoveNext())
								{
									PID pid2 = enumerator.Current;
									if (!(pid2 is CalculatedPIDV2) && !(pid2 is SensorPID))
									{
										pid2.IsAvailable = false;
									}
								}
							}
							finally
							{
								if (num < 0)
								{
									((IDisposable)enumerator).Dispose();
								}
							}
							List<string> list = (from pid in obddataReader.CurrentCarData.LiveDataPIDs
								where !(pid is CalculatedPIDV2) && !(pid is SensorPID) && !(pid is PID_SupportedPids) && !string.IsNullOrEmpty(pid.Command) && !pid.Command.StartsWith("09")
								select pid into x
								select x.Command).Distinct<string>().ToList<string>();
							if ((obddataReader.IsNissanConsult2Protocol && obddataReader.CurrentProtocolNumber == 11) || (obddataReader.IsNissanConsult2Protocol && obddataReader.CurrentProtocolNumber == 43 && !SharedSettings.Current.UseOBD2))
							{
								list = list.Where((string x) => x.Length > 4).ToList<string>();
							}
							obddataReader.CurrentCarData.ShouldSetAsAvailable = true;
							taskAwaiter = obddataReader.CheckPidsByTestings(list).GetAwaiter();
							if (!taskAwaiter.IsCompleted)
							{
								num = (num2 = 0);
								taskAwaiter2 = taskAwaiter;
								this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, OBDDataReader.<CheckSupportedPIDsScanTest>d__213>(ref taskAwaiter, ref this);
								return;
							}
						}
						else
						{
							taskAwaiter = taskAwaiter2;
							taskAwaiter2 = default(TaskAwaiter);
							num = (num2 = -1);
						}
						taskAwaiter.GetResult();
					}
					catch (object obj2)
					{
						obj = obj2;
					}
					taskAwaiter = Task.Delay(500).GetAwaiter();
					if (!taskAwaiter.IsCompleted)
					{
						num = (num2 = 1);
						taskAwaiter2 = taskAwaiter;
						this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, OBDDataReader.<CheckSupportedPIDsScanTest>d__213>(ref taskAwaiter, ref this);
						return;
					}
					IL_0214:
					taskAwaiter.GetResult();
					obddataReader.CurrentMode = OBDDataReader.OBDModes.Universal;
					obddataReader.CurrentCarData.ShouldSetAsAvailable = false;
					object obj2 = obj;
					if (obj2 != null)
					{
						Exception ex = obj2 as Exception;
						if (ex == null)
						{
							throw obj2;
						}
						ExceptionDispatchInfo.Capture(ex).Throw();
					}
					obj = null;
				}
				catch (Exception ex2)
				{
					num2 = -2;
					this.<>t__builder.SetException(ex2);
					return;
				}
				num2 = -2;
				this.<>t__builder.SetResult();
			}

			// Token: 0x060025ED RID: 9709 RVA: 0x001C8E98 File Offset: 0x001C7098
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x04001336 RID: 4918
			public int <>1__state;

			// Token: 0x04001337 RID: 4919
			public AsyncTaskMethodBuilder <>t__builder;

			// Token: 0x04001338 RID: 4920
			public OBDDataReader <>4__this;

			// Token: 0x04001339 RID: 4921
			private object <>7__wrap1;

			// Token: 0x0400133A RID: 4922
			private int <>7__wrap2;

			// Token: 0x0400133B RID: 4923
			private TaskAwaiter <>u__1;
		}

		// Token: 0x0200035E RID: 862
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <CheckSupportedPIDsV2>d__211 : IAsyncStateMachine
		{
			// Token: 0x060025EE RID: 9710 RVA: 0x001C8EA8 File Offset: 0x001C70A8
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				OBDDataReader obddataReader = this;
				bool flag;
				try
				{
					if (num > 28)
					{
						OBDRequestQueueOptimizer.MainECUSupportedItems.Clear();
						List<PID>.Enumerator enumerator = obddataReader.CurrentCarData.LiveDataPIDs.GetEnumerator();
						try
						{
							while (enumerator.MoveNext())
							{
								PID pid = enumerator.Current;
								if (!(pid is CustomPID) && !pid.Command.StartsWith("09"))
								{
									pid.IsAvailable = false;
								}
							}
						}
						finally
						{
							if (num < 0)
							{
								((IDisposable)enumerator).Dispose();
							}
						}
						if (!SharedSettings.Current.UseOBD2)
						{
							goto IL_1443;
						}
					}
					try
					{
						TaskAwaiter taskAwaiter;
						TaskAwaiter<string> taskAwaiter3;
						ValueTaskAwaiter<bool> valueTaskAwaiter3;
						switch (num)
						{
						case 0:
						{
							TaskAwaiter taskAwaiter2;
							taskAwaiter = taskAwaiter2;
							taskAwaiter2 = default(TaskAwaiter);
							num = (num2 = -1);
							break;
						}
						case 1:
						{
							TaskAwaiter taskAwaiter2;
							taskAwaiter = taskAwaiter2;
							taskAwaiter2 = default(TaskAwaiter);
							num = (num2 = -1);
							goto IL_0208;
						}
						case 2:
						{
							TaskAwaiter taskAwaiter2;
							taskAwaiter = taskAwaiter2;
							taskAwaiter2 = default(TaskAwaiter);
							num = (num2 = -1);
							goto IL_027A;
						}
						case 3:
						{
							TaskAwaiter taskAwaiter2;
							taskAwaiter = taskAwaiter2;
							taskAwaiter2 = default(TaskAwaiter);
							num = (num2 = -1);
							goto IL_02F1;
						}
						case 4:
						{
							TaskAwaiter taskAwaiter2;
							taskAwaiter = taskAwaiter2;
							taskAwaiter2 = default(TaskAwaiter);
							num = (num2 = -1);
							goto IL_0363;
						}
						case 5:
						{
							TaskAwaiter taskAwaiter2;
							taskAwaiter = taskAwaiter2;
							taskAwaiter2 = default(TaskAwaiter);
							num = (num2 = -1);
							goto IL_03D5;
						}
						case 6:
						case 7:
						case 8:
							IL_03DC:
							try
							{
								try
								{
									switch (num)
									{
									case 6:
									{
										TaskAwaiter taskAwaiter2;
										taskAwaiter = taskAwaiter2;
										taskAwaiter2 = default(TaskAwaiter);
										num = (num2 = -1);
										break;
									}
									case 7:
									{
										TaskAwaiter<string> taskAwaiter4;
										taskAwaiter3 = taskAwaiter4;
										taskAwaiter4 = default(TaskAwaiter<string>);
										num = (num2 = -1);
										goto IL_052C;
									}
									case 8:
										valueTaskAwaiter3 = valueTaskAwaiter2;
										valueTaskAwaiter2 = default(ValueTaskAwaiter<bool>);
										num = (num2 = -1);
										goto IL_0599;
									default:
										mafpid = obddataReader.CurrentCarData.LiveDataPIDs.FirstOrDefault((PID x) => x.Role == Roles.MAF && x.Command == "221209");
										if (mafpid == null || mafpid.IsAvailable)
										{
											goto IL_05B5;
										}
										req = new OBDRequest(mafpid.Command, false, mafpid);
										obddataReader.CurrentCarData.ShouldSetAsAvailable = true;
										taskAwaiter = obddataReader.SendRequest(req).GetAwaiter();
										if (!taskAwaiter.IsCompleted)
										{
											num = (num2 = 6);
											TaskAwaiter taskAwaiter2 = taskAwaiter;
											this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, OBDDataReader.<CheckSupportedPIDsV2>d__211>(ref taskAwaiter, ref this);
											return;
										}
										break;
									}
									taskAwaiter.GetResult();
									taskAwaiter3 = obddataReader.ReadData(2500, null, -1).GetAwaiter();
									if (!taskAwaiter3.IsCompleted)
									{
										num = (num2 = 7);
										TaskAwaiter<string> taskAwaiter4 = taskAwaiter3;
										this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<string>, OBDDataReader.<CheckSupportedPIDsV2>d__211>(ref taskAwaiter3, ref this);
										return;
									}
									IL_052C:
									string result = taskAwaiter3.GetResult();
									valueTaskAwaiter3 = obddataReader.DecodeData(result, req, null).GetAwaiter();
									if (!valueTaskAwaiter3.IsCompleted)
									{
										num = (num2 = 8);
										valueTaskAwaiter2 = valueTaskAwaiter3;
										this.<>t__builder.AwaitUnsafeOnCompleted<ValueTaskAwaiter<bool>, OBDDataReader.<CheckSupportedPIDsV2>d__211>(ref valueTaskAwaiter3, ref this);
										return;
									}
									IL_0599:
									if (valueTaskAwaiter3.GetResult())
									{
										mafpid.IsAvailable = true;
									}
									req = null;
									IL_05B5:
									mafpid = null;
								}
								catch (Exception)
								{
								}
								goto IL_13CF;
							}
							finally
							{
								if (num < 0)
								{
									obddataReader.CurrentCarData.ShouldSetAsAvailable = false;
								}
							}
							goto IL_05D7;
						case 9:
						{
							TaskAwaiter taskAwaiter2;
							taskAwaiter = taskAwaiter2;
							taskAwaiter2 = default(TaskAwaiter);
							num = (num2 = -1);
							goto IL_0669;
						}
						case 10:
						{
							TaskAwaiter taskAwaiter2;
							taskAwaiter = taskAwaiter2;
							taskAwaiter2 = default(TaskAwaiter);
							num = (num2 = -1);
							goto IL_0745;
						}
						case 11:
						{
							TaskAwaiter<string> taskAwaiter4;
							taskAwaiter3 = taskAwaiter4;
							taskAwaiter4 = default(TaskAwaiter<string>);
							num = (num2 = -1);
							goto IL_07AB;
						}
						case 12:
							valueTaskAwaiter3 = valueTaskAwaiter2;
							valueTaskAwaiter2 = default(ValueTaskAwaiter<bool>);
							num = (num2 = -1);
							goto IL_0819;
						case 13:
						{
							TaskAwaiter taskAwaiter2;
							taskAwaiter = taskAwaiter2;
							taskAwaiter2 = default(TaskAwaiter);
							num = (num2 = -1);
							goto IL_08E8;
						}
						case 14:
						{
							TaskAwaiter<string> taskAwaiter4;
							taskAwaiter3 = taskAwaiter4;
							taskAwaiter4 = default(TaskAwaiter<string>);
							num = (num2 = -1);
							goto IL_094E;
						}
						case 15:
							valueTaskAwaiter3 = valueTaskAwaiter2;
							valueTaskAwaiter2 = default(ValueTaskAwaiter<bool>);
							num = (num2 = -1);
							goto IL_09BC;
						case 16:
						{
							TaskAwaiter taskAwaiter2;
							taskAwaiter = taskAwaiter2;
							taskAwaiter2 = default(TaskAwaiter);
							num = (num2 = -1);
							goto IL_0D7F;
						}
						case 17:
						{
							TaskAwaiter taskAwaiter2;
							taskAwaiter = taskAwaiter2;
							taskAwaiter2 = default(TaskAwaiter);
							num = (num2 = -1);
							goto IL_0E9F;
						}
						case 18:
						{
							TaskAwaiter<string> taskAwaiter4;
							taskAwaiter3 = taskAwaiter4;
							taskAwaiter4 = default(TaskAwaiter<string>);
							num = (num2 = -1);
							goto IL_0F05;
						}
						case 19:
						{
							TaskAwaiter taskAwaiter2;
							taskAwaiter = taskAwaiter2;
							taskAwaiter2 = default(TaskAwaiter);
							num = (num2 = -1);
							goto IL_0F7A;
						}
						case 20:
						{
							TaskAwaiter taskAwaiter2;
							taskAwaiter = taskAwaiter2;
							taskAwaiter2 = default(TaskAwaiter);
							num = (num2 = -1);
							goto IL_0FEE;
						}
						case 21:
						{
							TaskAwaiter taskAwaiter2;
							taskAwaiter = taskAwaiter2;
							taskAwaiter2 = default(TaskAwaiter);
							num = (num2 = -1);
							goto IL_1062;
						}
						case 22:
						{
							TaskAwaiter taskAwaiter2;
							taskAwaiter = taskAwaiter2;
							taskAwaiter2 = default(TaskAwaiter);
							num = (num2 = -1);
							goto IL_10D6;
						}
						case 23:
						{
							TaskAwaiter<string> taskAwaiter4;
							taskAwaiter3 = taskAwaiter4;
							taskAwaiter4 = default(TaskAwaiter<string>);
							num = (num2 = -1);
							goto IL_113C;
						}
						case 24:
						case 25:
						case 26:
							IL_1144:
							try
							{
								switch (num)
								{
								case 24:
								{
									TaskAwaiter taskAwaiter2;
									taskAwaiter = taskAwaiter2;
									taskAwaiter2 = default(TaskAwaiter);
									num = (num2 = -1);
									break;
								}
								case 25:
								{
									TaskAwaiter<string> taskAwaiter4;
									taskAwaiter3 = taskAwaiter4;
									taskAwaiter4 = default(TaskAwaiter<string>);
									num = (num2 = -1);
									goto IL_12B0;
								}
								case 26:
									valueTaskAwaiter3 = valueTaskAwaiter2;
									valueTaskAwaiter2 = default(ValueTaskAwaiter<bool>);
									num = (num2 = -1);
									goto IL_131E;
								default:
								{
									mafpid2 = obddataReader.CurrentCarData.LiveDataPIDs.FirstOrDefault((PID x) => x.Role == Roles.MAF && x.Command == "221209");
									if (mafpid2 == null || mafpid2.IsAvailable)
									{
										goto IL_133A;
									}
									List<OBDRequest> list = new List<OBDRequest>(1);
									LiveDataPIDModel.GetRequests(mafpid2, list, null, "");
									if (list.Count <= 0)
									{
										goto IL_133A;
									}
									req2 = list[0];
									obddataReader.CurrentCarData.ShouldSetAsAvailable = true;
									taskAwaiter = obddataReader.SendRequest(req2).GetAwaiter();
									if (!taskAwaiter.IsCompleted)
									{
										num = (num2 = 24);
										TaskAwaiter taskAwaiter2 = taskAwaiter;
										this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, OBDDataReader.<CheckSupportedPIDsV2>d__211>(ref taskAwaiter, ref this);
										return;
									}
									break;
								}
								}
								taskAwaiter.GetResult();
								taskAwaiter3 = obddataReader.ReadData(2500, null, -1).GetAwaiter();
								if (!taskAwaiter3.IsCompleted)
								{
									num = (num2 = 25);
									TaskAwaiter<string> taskAwaiter4 = taskAwaiter3;
									this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<string>, OBDDataReader.<CheckSupportedPIDsV2>d__211>(ref taskAwaiter3, ref this);
									return;
								}
								IL_12B0:
								string result2 = taskAwaiter3.GetResult();
								valueTaskAwaiter3 = obddataReader.DecodeData(result2, req2, null).GetAwaiter();
								if (!valueTaskAwaiter3.IsCompleted)
								{
									num = (num2 = 26);
									valueTaskAwaiter2 = valueTaskAwaiter3;
									this.<>t__builder.AwaitUnsafeOnCompleted<ValueTaskAwaiter<bool>, OBDDataReader.<CheckSupportedPIDsV2>d__211>(ref valueTaskAwaiter3, ref this);
									return;
								}
								IL_131E:
								if (valueTaskAwaiter3.GetResult())
								{
									mafpid2.IsAvailable = true;
								}
								req2 = null;
								IL_133A:
								mafpid2 = null;
							}
							catch (Exception)
							{
							}
							finally
							{
								if (num < 0)
								{
									obddataReader.CurrentCarData.ShouldSetAsAvailable = false;
								}
							}
							ecm_header = null;
							req = null;
							goto IL_1367;
						case 27:
						{
							TaskAwaiter taskAwaiter2;
							taskAwaiter = taskAwaiter2;
							taskAwaiter2 = default(TaskAwaiter);
							num = (num2 = -1);
							goto IL_13C8;
						}
						case 28:
						{
							TaskAwaiter taskAwaiter2;
							taskAwaiter = taskAwaiter2;
							taskAwaiter2 = default(TaskAwaiter);
							num = (num2 = -1);
							goto IL_1428;
						}
						default:
							if (!SharedSettings.Current.PerformSensorsScanByTesting)
							{
								if (!obddataReader.IsNissanConsult2Protocol)
								{
									goto IL_05D7;
								}
								if (obddataReader.CurrentProtocolNumber == 43)
								{
									taskAwaiter = obddataReader.CheckSupportedPIDsClassicTestV2("2211{0}", "7E0", "", "").GetAwaiter();
									if (!taskAwaiter.IsCompleted)
									{
										num = (num2 = 0);
										TaskAwaiter taskAwaiter2 = taskAwaiter;
										this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, OBDDataReader.<CheckSupportedPIDsV2>d__211>(ref taskAwaiter, ref this);
										return;
									}
								}
								else
								{
									taskAwaiter = obddataReader.CheckSupportedPIDsClassicTestV2("2211{0}", "", "", "").GetAwaiter();
									if (!taskAwaiter.IsCompleted)
									{
										num = (num2 = 3);
										TaskAwaiter taskAwaiter2 = taskAwaiter;
										this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, OBDDataReader.<CheckSupportedPIDsV2>d__211>(ref taskAwaiter, ref this);
										return;
									}
									goto IL_02F1;
								}
							}
							else
							{
								taskAwaiter = obddataReader.CheckSupportedPIDsScanTest().GetAwaiter();
								if (!taskAwaiter.IsCompleted)
								{
									num = (num2 = 27);
									TaskAwaiter taskAwaiter2 = taskAwaiter;
									this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, OBDDataReader.<CheckSupportedPIDsV2>d__211>(ref taskAwaiter, ref this);
									return;
								}
								goto IL_13C8;
							}
							break;
						}
						taskAwaiter.GetResult();
						taskAwaiter = obddataReader.CheckSupportedPIDsClassicTestV2("2212{0}", "7E0", "", "").GetAwaiter();
						if (!taskAwaiter.IsCompleted)
						{
							num = (num2 = 1);
							TaskAwaiter taskAwaiter2 = taskAwaiter;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, OBDDataReader.<CheckSupportedPIDsV2>d__211>(ref taskAwaiter, ref this);
							return;
						}
						IL_0208:
						taskAwaiter.GetResult();
						taskAwaiter = obddataReader.CheckSupportedPIDsClassicTestV2("2213{0}", "7E0", "", "").GetAwaiter();
						if (!taskAwaiter.IsCompleted)
						{
							num = (num2 = 2);
							TaskAwaiter taskAwaiter2 = taskAwaiter;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, OBDDataReader.<CheckSupportedPIDsV2>d__211>(ref taskAwaiter, ref this);
							return;
						}
						IL_027A:
						taskAwaiter.GetResult();
						goto IL_03DC;
						IL_02F1:
						taskAwaiter.GetResult();
						taskAwaiter = obddataReader.CheckSupportedPIDsClassicTestV2("2212{0}", "", "", "").GetAwaiter();
						if (!taskAwaiter.IsCompleted)
						{
							num = (num2 = 4);
							TaskAwaiter taskAwaiter2 = taskAwaiter;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, OBDDataReader.<CheckSupportedPIDsV2>d__211>(ref taskAwaiter, ref this);
							return;
						}
						IL_0363:
						taskAwaiter.GetResult();
						taskAwaiter = obddataReader.CheckSupportedPIDsClassicTestV2("2213{0}", "", "", "").GetAwaiter();
						if (!taskAwaiter.IsCompleted)
						{
							num = (num2 = 5);
							TaskAwaiter taskAwaiter2 = taskAwaiter;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, OBDDataReader.<CheckSupportedPIDsV2>d__211>(ref taskAwaiter, ref this);
							return;
						}
						IL_03D5:
						taskAwaiter.GetResult();
						goto IL_03DC;
						IL_05D7:
						string text = SharedSettings.Current.Mode01Prefix + "{0}";
						if (SharedSettings.Current.DaihatsuKLine)
						{
							text = "21{0}01";
						}
						taskAwaiter = obddataReader.CheckSupportedPIDsClassicTestV2(text, "", "", "").GetAwaiter();
						if (!taskAwaiter.IsCompleted)
						{
							num = (num2 = 9);
							TaskAwaiter taskAwaiter2 = taskAwaiter;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, OBDDataReader.<CheckSupportedPIDsV2>d__211>(ref taskAwaiter, ref this);
							return;
						}
						IL_0669:
						taskAwaiter.GetResult();
						PIDWithFloatValueFormula.ResetScalingToDefaults();
						mafpid = obddataReader.CurrentCarData.LiveDataPIDs.First((PID x) => x.Id == 103);
						if (!mafpid.IsAvailable)
						{
							goto IL_0828;
						}
						req = new OBDRequest(mafpid.Command, false, new List<PID> { mafpid });
						taskAwaiter = obddataReader.SendString(mafpid.Command).GetAwaiter();
						if (!taskAwaiter.IsCompleted)
						{
							num = (num2 = 10);
							TaskAwaiter taskAwaiter2 = taskAwaiter;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, OBDDataReader.<CheckSupportedPIDsV2>d__211>(ref taskAwaiter, ref this);
							return;
						}
						IL_0745:
						taskAwaiter.GetResult();
						taskAwaiter3 = obddataReader.ReadData(2500, null, -1).GetAwaiter();
						if (!taskAwaiter3.IsCompleted)
						{
							num = (num2 = 11);
							TaskAwaiter<string> taskAwaiter4 = taskAwaiter3;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<string>, OBDDataReader.<CheckSupportedPIDsV2>d__211>(ref taskAwaiter3, ref this);
							return;
						}
						IL_07AB:
						string result3 = taskAwaiter3.GetResult();
						valueTaskAwaiter3 = obddataReader.DecodeData(result3, req, null).GetAwaiter();
						if (!valueTaskAwaiter3.IsCompleted)
						{
							num = (num2 = 12);
							valueTaskAwaiter2 = valueTaskAwaiter3;
							this.<>t__builder.AwaitUnsafeOnCompleted<ValueTaskAwaiter<bool>, OBDDataReader.<CheckSupportedPIDsV2>d__211>(ref valueTaskAwaiter3, ref this);
							return;
						}
						IL_0819:
						valueTaskAwaiter3.GetResult();
						req = null;
						IL_0828:
						PID pid2 = obddataReader.CurrentCarData.LiveDataPIDs.First((PID x) => x.Id == 104);
						if (!pid2.IsAvailable)
						{
							goto IL_09CB;
						}
						req = new OBDRequest(pid2.Command, false, new List<PID> { mafpid });
						taskAwaiter = obddataReader.SendString(pid2.Command).GetAwaiter();
						if (!taskAwaiter.IsCompleted)
						{
							num = (num2 = 13);
							TaskAwaiter taskAwaiter2 = taskAwaiter;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, OBDDataReader.<CheckSupportedPIDsV2>d__211>(ref taskAwaiter, ref this);
							return;
						}
						IL_08E8:
						taskAwaiter.GetResult();
						taskAwaiter3 = obddataReader.ReadData(2500, null, -1).GetAwaiter();
						if (!taskAwaiter3.IsCompleted)
						{
							num = (num2 = 14);
							TaskAwaiter<string> taskAwaiter4 = taskAwaiter3;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<string>, OBDDataReader.<CheckSupportedPIDsV2>d__211>(ref taskAwaiter3, ref this);
							return;
						}
						IL_094E:
						string result4 = taskAwaiter3.GetResult();
						valueTaskAwaiter3 = obddataReader.DecodeData(result4, req, null).GetAwaiter();
						if (!valueTaskAwaiter3.IsCompleted)
						{
							num = (num2 = 15);
							valueTaskAwaiter2 = valueTaskAwaiter3;
							this.<>t__builder.AwaitUnsafeOnCompleted<ValueTaskAwaiter<bool>, OBDDataReader.<CheckSupportedPIDsV2>d__211>(ref valueTaskAwaiter3, ref this);
							return;
						}
						IL_09BC:
						valueTaskAwaiter3.GetResult();
						req = null;
						IL_09CB:
						IEnumerable<string> enumerable = new string[]
						{
							"0165", "0166", "0167", "0168", "0169", "016A", "016B", "016C", "016D", "016E",
							"016F", "0170", "0171", "0172", "0173", "0173", "0174", "0175", "0176", "0177",
							"0178", "0179", "017A", "017B", "017C", "017D", "017E", "017F", "0181", "0182",
							"0183", "0185", "0186", "0187", "0189", "018A", "018B", "018C", "018F", "0192",
							"0193", "0194", "0195", "0196", "0197", "0198", "0199", "019A", "019B", "019C",
							"019F", "01A1", "01A3", "01A4", "01A5", "01A7", "01A8", "01A9", "01AB", "01AC",
							"01AD", "01AE", "01B1", "01B3", "01B4", "01B5", "01B6", "01B7", "01BB", "01BC",
							"01BD", "01BE", "01BF", "01C0", "01C1", "01C2", "01C3", "01C5", "01C6", "01C9",
							"01CA", "01CB", "01CC", "01CD"
						};
						string[] array = (from x in obddataReader.CurrentCarData.LiveDataPIDs
							where x.IsAvailable && (x is IPIDFloatValue || x is IPIDWithStringValue)
							select x.Command).ToArray<string>();
						List<string> list2 = enumerable.Intersect(array).ToList<string>();
						taskAwaiter = obddataReader.CheckPidsByTestings(list2).GetAwaiter();
						if (!taskAwaiter.IsCompleted)
						{
							num = (num2 = 16);
							TaskAwaiter taskAwaiter2 = taskAwaiter;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, OBDDataReader.<CheckSupportedPIDsV2>d__211>(ref taskAwaiter, ref this);
							return;
						}
						IL_0D7F:
						taskAwaiter.GetResult();
						if ((obddataReader.CurrentELMFormat != ELMFormat.CAN11bit && obddataReader.CurrentELMFormat != ELMFormat.CAN29bit) || (!(SharedSettings.Current.SelectedBrand == "Nissan") && !(SharedSettings.Current.SelectedBrand == "Infiniti")) || !SharedSettings.Current.AddNissanConsult3Pids)
						{
							goto IL_1367;
						}
						ecm_header = "7E0";
						if (obddataReader.CurrentELMFormat == ELMFormat.CAN29bit)
						{
							ecm_header = "DA15F1";
						}
						OBDRequest obdrequest = new OBDRequest("10C0", ecm_header, "", "", false);
						req = new OBDRequest("1081", ecm_header, "", "", false);
						if (!SharedSettings.Current.NissanConsult3OpenCloseSession)
						{
							goto IL_0F0D;
						}
						taskAwaiter = obddataReader.SendRequest(obdrequest).GetAwaiter();
						if (!taskAwaiter.IsCompleted)
						{
							num = (num2 = 17);
							TaskAwaiter taskAwaiter2 = taskAwaiter;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, OBDDataReader.<CheckSupportedPIDsV2>d__211>(ref taskAwaiter, ref this);
							return;
						}
						IL_0E9F:
						taskAwaiter.GetResult();
						taskAwaiter3 = obddataReader.ReadData(2500, null, -1).GetAwaiter();
						if (!taskAwaiter3.IsCompleted)
						{
							num = (num2 = 18);
							TaskAwaiter<string> taskAwaiter4 = taskAwaiter3;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<string>, OBDDataReader.<CheckSupportedPIDsV2>d__211>(ref taskAwaiter3, ref this);
							return;
						}
						IL_0F05:
						taskAwaiter3.GetResult();
						IL_0F0D:
						taskAwaiter = obddataReader.CheckSupportedPIDsClassicTestV2("2211{0}", ecm_header, "", "").GetAwaiter();
						if (!taskAwaiter.IsCompleted)
						{
							num = (num2 = 19);
							TaskAwaiter taskAwaiter2 = taskAwaiter;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, OBDDataReader.<CheckSupportedPIDsV2>d__211>(ref taskAwaiter, ref this);
							return;
						}
						IL_0F7A:
						taskAwaiter.GetResult();
						taskAwaiter = obddataReader.CheckSupportedPIDsClassicTestV2("2212{0}", ecm_header, "", "").GetAwaiter();
						if (!taskAwaiter.IsCompleted)
						{
							num = (num2 = 20);
							TaskAwaiter taskAwaiter2 = taskAwaiter;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, OBDDataReader.<CheckSupportedPIDsV2>d__211>(ref taskAwaiter, ref this);
							return;
						}
						IL_0FEE:
						taskAwaiter.GetResult();
						taskAwaiter = obddataReader.CheckSupportedPIDsClassicTestV2("2213{0}", ecm_header, "", "").GetAwaiter();
						if (!taskAwaiter.IsCompleted)
						{
							num = (num2 = 21);
							TaskAwaiter taskAwaiter2 = taskAwaiter;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, OBDDataReader.<CheckSupportedPIDsV2>d__211>(ref taskAwaiter, ref this);
							return;
						}
						IL_1062:
						taskAwaiter.GetResult();
						if (!SharedSettings.Current.NissanConsult3OpenCloseSession)
						{
							goto IL_1144;
						}
						taskAwaiter = obddataReader.SendRequest(req).GetAwaiter();
						if (!taskAwaiter.IsCompleted)
						{
							num = (num2 = 22);
							TaskAwaiter taskAwaiter2 = taskAwaiter;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, OBDDataReader.<CheckSupportedPIDsV2>d__211>(ref taskAwaiter, ref this);
							return;
						}
						IL_10D6:
						taskAwaiter.GetResult();
						taskAwaiter3 = obddataReader.ReadData(2500, null, -1).GetAwaiter();
						if (!taskAwaiter3.IsCompleted)
						{
							num = (num2 = 23);
							TaskAwaiter<string> taskAwaiter4 = taskAwaiter3;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<string>, OBDDataReader.<CheckSupportedPIDsV2>d__211>(ref taskAwaiter3, ref this);
							return;
						}
						IL_113C:
						taskAwaiter3.GetResult();
						goto IL_1144;
						IL_1367:
						mafpid = null;
						goto IL_13CF;
						IL_13C8:
						taskAwaiter.GetResult();
						IL_13CF:
						taskAwaiter = Task.Delay(500).GetAwaiter();
						if (!taskAwaiter.IsCompleted)
						{
							num = (num2 = 28);
							TaskAwaiter taskAwaiter2 = taskAwaiter;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, OBDDataReader.<CheckSupportedPIDsV2>d__211>(ref taskAwaiter, ref this);
							return;
						}
						IL_1428:
						taskAwaiter.GetResult();
						obddataReader.CurrentCarData.InitializeCalculatedPIDs();
						flag = true;
						goto IL_146B;
					}
					catch (Exception)
					{
						flag = false;
						goto IL_146B;
					}
					IL_1443:
					obddataReader.CurrentCarData.InitializeCalculatedPIDs();
					flag = true;
				}
				catch (Exception ex)
				{
					num2 = -2;
					this.<>t__builder.SetException(ex);
					return;
				}
				IL_146B:
				num2 = -2;
				this.<>t__builder.SetResult(flag);
			}

			// Token: 0x060025EF RID: 9711 RVA: 0x001CA3E0 File Offset: 0x001C85E0
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x0400133C RID: 4924
			public int <>1__state;

			// Token: 0x0400133D RID: 4925
			public AsyncTaskMethodBuilder<bool> <>t__builder;

			// Token: 0x0400133E RID: 4926
			public OBDDataReader <>4__this;

			// Token: 0x0400133F RID: 4927
			private TaskAwaiter <>u__1;

			// Token: 0x04001340 RID: 4928
			private PID <mafpid>5__2;

			// Token: 0x04001341 RID: 4929
			private OBDRequest <req>5__3;

			// Token: 0x04001342 RID: 4930
			private TaskAwaiter<string> <>u__2;

			// Token: 0x04001343 RID: 4931
			private ValueTaskAwaiter<bool> <>u__3;

			// Token: 0x04001344 RID: 4932
			private string <ecm_header>5__4;

			// Token: 0x04001345 RID: 4933
			private PID <mafpid>5__5;

			// Token: 0x04001346 RID: 4934
			private OBDRequest <req>5__6;
		}

		// Token: 0x0200035F RID: 863
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <ClearRequestQueue>d__162 : IAsyncStateMachine
		{
			// Token: 0x060025F0 RID: 9712 RVA: 0x001CA3F0 File Offset: 0x001C85F0
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				OBDDataReader obddataReader = this;
				try
				{
					OBDRequest obdrequest;
					TaskAwaiter taskAwaiter;
					if (num != 0)
					{
						while (obddataReader.CommandQueue.TryDequeue(out obdrequest))
						{
						}
						taskAwaiter = Task.Delay(50).GetAwaiter();
						if (!taskAwaiter.IsCompleted)
						{
							num2 = 0;
							TaskAwaiter taskAwaiter2 = taskAwaiter;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, OBDDataReader.<ClearRequestQueue>d__162>(ref taskAwaiter, ref this);
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
					while (obddataReader.CommandQueue.TryDequeue(out obdrequest))
					{
					}
				}
				catch (Exception ex)
				{
					num2 = -2;
					this.<>t__builder.SetException(ex);
					return;
				}
				num2 = -2;
				this.<>t__builder.SetResult();
			}

			// Token: 0x060025F1 RID: 9713 RVA: 0x001CA4C4 File Offset: 0x001C86C4
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x04001347 RID: 4935
			public int <>1__state;

			// Token: 0x04001348 RID: 4936
			public AsyncTaskMethodBuilder <>t__builder;

			// Token: 0x04001349 RID: 4937
			public OBDDataReader <>4__this;

			// Token: 0x0400134A RID: 4938
			private TaskAwaiter <>u__1;
		}

		// Token: 0x02000360 RID: 864
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <Connect>d__131 : IAsyncStateMachine
		{
			// Token: 0x060025F2 RID: 9714 RVA: 0x001CA4D4 File Offset: 0x001C86D4
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				OBDDataReader obddataReader = this;
				bool flag;
				try
				{
					TaskAwaiter taskAwaiter3;
					int num3;
					switch (num)
					{
					case 0:
					case 1:
					case 2:
					case 3:
					case 4:
					case 5:
					case 6:
					case 7:
					case 8:
					case 9:
					case 10:
					case 11:
					case 12:
					case 13:
					case 14:
					case 15:
					{
						IL_00AC:
						try
						{
							TaskAwaiter<bool> taskAwaiter5;
							switch (num)
							{
							case 0:
							{
								TaskAwaiter taskAwaiter4;
								taskAwaiter3 = taskAwaiter4;
								taskAwaiter4 = default(TaskAwaiter);
								num = (num2 = -1);
								break;
							}
							case 1:
							{
								TaskAwaiter taskAwaiter4;
								taskAwaiter3 = taskAwaiter4;
								taskAwaiter4 = default(TaskAwaiter);
								num = (num2 = -1);
								goto IL_0296;
							}
							case 2:
							{
								TaskAwaiter taskAwaiter4;
								taskAwaiter3 = taskAwaiter4;
								taskAwaiter4 = default(TaskAwaiter);
								num = (num2 = -1);
								goto IL_033D;
							}
							case 3:
							{
								TaskAwaiter taskAwaiter4;
								taskAwaiter3 = taskAwaiter4;
								taskAwaiter4 = default(TaskAwaiter);
								num = (num2 = -1);
								goto IL_03CF;
							}
							case 4:
							{
								TaskAwaiter taskAwaiter4;
								taskAwaiter3 = taskAwaiter4;
								taskAwaiter4 = default(TaskAwaiter);
								num = (num2 = -1);
								goto IL_057B;
							}
							case 5:
							{
								TaskAwaiter taskAwaiter4;
								taskAwaiter3 = taskAwaiter4;
								taskAwaiter4 = default(TaskAwaiter);
								num = (num2 = -1);
								goto IL_05F7;
							}
							case 6:
							{
								TaskAwaiter taskAwaiter4;
								taskAwaiter3 = taskAwaiter4;
								taskAwaiter4 = default(TaskAwaiter);
								num = (num2 = -1);
								goto IL_06AB;
							}
							case 7:
							{
								TaskAwaiter taskAwaiter4;
								taskAwaiter3 = taskAwaiter4;
								taskAwaiter4 = default(TaskAwaiter);
								num = (num2 = -1);
								goto IL_0725;
							}
							case 8:
							{
								TaskAwaiter taskAwaiter4;
								taskAwaiter3 = taskAwaiter4;
								taskAwaiter4 = default(TaskAwaiter);
								num = (num2 = -1);
								goto IL_0796;
							}
							case 9:
							{
								TaskAwaiter taskAwaiter4;
								taskAwaiter3 = taskAwaiter4;
								taskAwaiter4 = default(TaskAwaiter);
								num = (num2 = -1);
								goto IL_0816;
							}
							case 10:
								taskAwaiter5 = taskAwaiter2;
								taskAwaiter2 = default(TaskAwaiter<bool>);
								num = (num2 = -1);
								goto IL_088C;
							case 11:
							{
								TaskAwaiter taskAwaiter4;
								taskAwaiter3 = taskAwaiter4;
								taskAwaiter4 = default(TaskAwaiter);
								num = (num2 = -1);
								goto IL_0905;
							}
							case 12:
							{
								TaskAwaiter taskAwaiter4;
								taskAwaiter3 = taskAwaiter4;
								taskAwaiter4 = default(TaskAwaiter);
								num = (num2 = -1);
								goto IL_0976;
							}
							case 13:
								taskAwaiter5 = taskAwaiter2;
								taskAwaiter2 = default(TaskAwaiter<bool>);
								num = (num2 = -1);
								goto IL_0A0D;
							case 14:
							{
								TaskAwaiter taskAwaiter4;
								taskAwaiter3 = taskAwaiter4;
								taskAwaiter4 = default(TaskAwaiter);
								num = (num2 = -1);
								goto IL_0A80;
							}
							case 15:
							{
								TaskAwaiter taskAwaiter4;
								taskAwaiter3 = taskAwaiter4;
								taskAwaiter4 = default(TaskAwaiter);
								num = (num2 = -1);
								goto IL_0B56;
							}
							default:
								if (obddataReader.DisconnectRequested)
								{
									obddataReader.CurrentStatus = OBDDataReaderStatus.Disconnected;
									flag = false;
									goto IL_0D22;
								}
								obddataReader.CurrentStatus = OBDDataReaderStatus.ConnectingToELM;
								if (Attempt <= 0)
								{
									goto IL_018E;
								}
								taskAwaiter3 = Task.Delay(1000).GetAwaiter();
								if (!taskAwaiter3.IsCompleted)
								{
									num = (num2 = 0);
									TaskAwaiter taskAwaiter4 = taskAwaiter3;
									this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, OBDDataReader.<Connect>d__131>(ref taskAwaiter3, ref this);
									return;
								}
								break;
							}
							taskAwaiter3.GetResult();
							if (obddataReader.DisconnectRequested)
							{
								flag = false;
								goto IL_0D22;
							}
							IL_018E:
							DateTime nowSafe = DateTimeNowHelper.NowSafe;
							byte[] bytes = Encoding.UTF8.GetBytes(string.Concat(new string[]
							{
								"\r\n===== LOG STARTED: ",
								nowSafe.ToString("dd.MM.yyyy HH:mm:ss", CultureInfo.InvariantCulture),
								"=====\r\n=====Version=",
								App.Version,
								" Build=",
								App.Build,
								" P=",
								SharedSettings.Current.AdsProductPurchased.ToString(),
								" NoDelayELM327Init=",
								SharedSettings.Current.NoDelayELM327Init.ToString(),
								"\r\n====Connection profile=",
								SharedSettings.Current.BrandAndProfile
							}));
							taskAwaiter3 = obddataReader.DebugWrite(bytes).GetAwaiter();
							if (!taskAwaiter3.IsCompleted)
							{
								num = (num2 = 1);
								TaskAwaiter taskAwaiter4 = taskAwaiter3;
								this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, OBDDataReader.<Connect>d__131>(ref taskAwaiter3, ref this);
								return;
							}
							IL_0296:
							taskAwaiter3.GetResult();
							if (obddataReader.Connection != null)
							{
								try
								{
									obddataReader.Connection.Disconect();
								}
								catch (Exception)
								{
								}
							}
							switch (SharedSettings.Current.ConnectionType)
							{
							case ConnectionTypes.WiFi:
								taskAwaiter3 = obddataReader.DebugWrite("\r\n[Connection type: WIFI]\r\n").GetAwaiter();
								if (!taskAwaiter3.IsCompleted)
								{
									num = (num2 = 2);
									TaskAwaiter taskAwaiter4 = taskAwaiter3;
									this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, OBDDataReader.<Connect>d__131>(ref taskAwaiter3, ref this);
									return;
								}
								break;
							case ConnectionTypes.BluetoothLE:
								taskAwaiter3 = obddataReader.DebugWrite("\r\n[Connection type: BluetoothLE; DeviceName=" + SharedSettings.Current.BTLEDeviceName + "]\r\n").GetAwaiter();
								if (!taskAwaiter3.IsCompleted)
								{
									num = (num2 = 5);
									TaskAwaiter taskAwaiter4 = taskAwaiter3;
									this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, OBDDataReader.<Connect>d__131>(ref taskAwaiter3, ref this);
									return;
								}
								goto IL_05F7;
							case ConnectionTypes.Bluetooth:
								taskAwaiter3 = obddataReader.DebugWrite("\r\n[Connection type: Bluetooth; DeviceName=" + SharedSettings.Current.BTDeviceName + "]\r\n").GetAwaiter();
								if (!taskAwaiter3.IsCompleted)
								{
									num = (num2 = 6);
									TaskAwaiter taskAwaiter4 = taskAwaiter3;
									this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, OBDDataReader.<Connect>d__131>(ref taskAwaiter3, ref this);
									return;
								}
								goto IL_06AB;
							case ConnectionTypes.USB:
								goto IL_07A9;
							case ConnectionTypes.MFI_OBDLinkMXPlus:
								taskAwaiter3 = obddataReader.DebugWrite("\r\nConnection type: Bluetooth\r\n").GetAwaiter();
								if (!taskAwaiter3.IsCompleted)
								{
									num = (num2 = 8);
									TaskAwaiter taskAwaiter4 = taskAwaiter3;
									this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, OBDDataReader.<Connect>d__131>(ref taskAwaiter3, ref this);
									return;
								}
								goto IL_0796;
							default:
								goto IL_07A9;
							}
							IL_033D:
							taskAwaiter3.GetResult();
							string text = null;
							IWiFiHelper wiFiHelper = DependencyService.Get<IWiFiHelper>(0);
							if (wiFiHelper != null)
							{
								text = wiFiHelper.GetWiFiName();
							}
							if (text == null)
							{
								text = "<null>";
							}
							taskAwaiter3 = obddataReader.DebugWrite("\r\n[WiFi name: " + text + "]\r\n").GetAwaiter();
							if (!taskAwaiter3.IsCompleted)
							{
								num = (num2 = 3);
								TaskAwaiter taskAwaiter4 = taskAwaiter3;
								this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, OBDDataReader.<Connect>d__131>(ref taskAwaiter3, ref this);
								return;
							}
							IL_03CF:
							taskAwaiter3.GetResult();
							if (Device.RuntimePlatform == "Android")
							{
								switch (SharedSettings.Current.AndroidWiFiMode)
								{
								case AndroidWiFiConnectionModes.ModernBind:
								{
									ITCPConnectionV2Droid itcpconnectionV2Droid = DependencyService.Get<ITCPConnectionV2Droid>(1);
									itcpconnectionV2Droid.Bind = true;
									obddataReader.Connection = itcpconnectionV2Droid;
									goto IL_04EE;
								}
								case AndroidWiFiConnectionModes.LegacyBind:
								{
									ITCPConnectionV1Droid itcpconnectionV1Droid = DependencyService.Get<ITCPConnectionV1Droid>(1);
									itcpconnectionV1Droid.Bind = true;
									obddataReader.Connection = itcpconnectionV1Droid;
									goto IL_04EE;
								}
								case AndroidWiFiConnectionModes.ModernDontBind:
								{
									ITCPConnectionV2Droid itcpconnectionV2Droid2 = DependencyService.Get<ITCPConnectionV2Droid>(1);
									itcpconnectionV2Droid2.Bind = false;
									obddataReader.Connection = itcpconnectionV2Droid2;
									goto IL_04EE;
								}
								case AndroidWiFiConnectionModes.LegacyDontBind:
								{
									ITCPConnectionV1Droid itcpconnectionV1Droid2 = DependencyService.Get<ITCPConnectionV1Droid>(1);
									itcpconnectionV1Droid2.Bind = false;
									obddataReader.Connection = itcpconnectionV1Droid2;
									goto IL_04EE;
								}
								}
								if (obddataReader.DroidWiFiV2Failed)
								{
									ITCPConnectionV1Droid itcpconnectionV1Droid3 = DependencyService.Get<ITCPConnectionV1Droid>(1);
									itcpconnectionV1Droid3.Bind = true;
									obddataReader.Connection = itcpconnectionV1Droid3;
								}
								else
								{
									ITCPConnectionV2Droid itcpconnectionV2Droid3 = DependencyService.Get<ITCPConnectionV2Droid>(1);
									itcpconnectionV2Droid3.Bind = true;
									obddataReader.Connection = itcpconnectionV2Droid3;
								}
							}
							else if (Device.RuntimePlatform == "iOS" || Device.RuntimePlatform == "macOS")
							{
								obddataReader.Connection = DependencyService.Get<ITCPConnection>(1);
							}
							IL_04EE:
							if (obddataReader.Connection != null)
							{
								goto IL_07A9;
							}
							if (Device.RuntimePlatform == "Android")
							{
								DependencyService.Get<ITCPConnectionV2Droid>(1);
							}
							else
							{
								obddataReader.Connection = DependencyService.Get<ITCPConnection>(1);
							}
							taskAwaiter3 = obddataReader.DebugWrite("[Connection==null]").GetAwaiter();
							if (!taskAwaiter3.IsCompleted)
							{
								num = (num2 = 4);
								TaskAwaiter taskAwaiter4 = taskAwaiter3;
								this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, OBDDataReader.<Connect>d__131>(ref taskAwaiter3, ref this);
								return;
							}
							IL_057B:
							taskAwaiter3.GetResult();
							goto IL_07A9;
							IL_05F7:
							taskAwaiter3.GetResult();
							obddataReader.Connection = new BTLEConnection(new Guid(SharedSettings.Current.BTLEServiceID), new Guid(SharedSettings.Current.BTLEInputID), new Guid(SharedSettings.Current.BTLEOutputID));
							goto IL_07A9;
							IL_06AB:
							taskAwaiter3.GetResult();
							obddataReader.Connection = DependencyService.Get<IBluetoothConnection>(1);
							if (obddataReader.Connection != null)
							{
								goto IL_07A9;
							}
							taskAwaiter3 = obddataReader.DebugWrite("Connection==null").GetAwaiter();
							if (!taskAwaiter3.IsCompleted)
							{
								num = (num2 = 7);
								TaskAwaiter taskAwaiter4 = taskAwaiter3;
								this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, OBDDataReader.<Connect>d__131>(ref taskAwaiter3, ref this);
								return;
							}
							IL_0725:
							taskAwaiter3.GetResult();
							obddataReader.Connection = DependencyService.Get<IBluetoothConnection>(1);
							goto IL_07A9;
							IL_0796:
							taskAwaiter3.GetResult();
							obddataReader.Connection = DependencyService.Get<IBluetoothConnection>(1);
							IL_07A9:
							taskAwaiter3 = obddataReader.DebugWrite("\r\n[Connecting to " + DeviceID + "]\r\n").GetAwaiter();
							if (!taskAwaiter3.IsCompleted)
							{
								num = (num2 = 9);
								TaskAwaiter taskAwaiter4 = taskAwaiter3;
								this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, OBDDataReader.<Connect>d__131>(ref taskAwaiter3, ref this);
								return;
							}
							IL_0816:
							taskAwaiter3.GetResult();
							obddataReader.initFailCounterOnAT = 0;
							taskAwaiter5 = obddataReader.Connection.ConnectAsync(DeviceID, PCLDebugStream.CurrentInstance).GetAwaiter();
							if (!taskAwaiter5.IsCompleted)
							{
								num = (num2 = 10);
								taskAwaiter2 = taskAwaiter5;
								this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<bool>, OBDDataReader.<Connect>d__131>(ref taskAwaiter5, ref this);
								return;
							}
							IL_088C:
							if (taskAwaiter5.GetResult())
							{
								taskAwaiter3 = obddataReader.DebugWrite("\r\n[Connected to " + DeviceID + "]\r\n").GetAwaiter();
								if (!taskAwaiter3.IsCompleted)
								{
									num = (num2 = 11);
									TaskAwaiter taskAwaiter4 = taskAwaiter3;
									this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, OBDDataReader.<Connect>d__131>(ref taskAwaiter3, ref this);
									return;
								}
							}
							else
							{
								if (Device.RuntimePlatform == "Android")
								{
									obddataReader.DroidWiFiV2Failed = !obddataReader.DroidWiFiV2Failed;
								}
								taskAwaiter3 = obddataReader.DebugWrite("\r\nConnection failed.\r\n").GetAwaiter();
								if (!taskAwaiter3.IsCompleted)
								{
									num = (num2 = 15);
									TaskAwaiter taskAwaiter4 = taskAwaiter3;
									this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, OBDDataReader.<Connect>d__131>(ref taskAwaiter3, ref this);
									return;
								}
								goto IL_0B56;
							}
							IL_0905:
							taskAwaiter3.GetResult();
							if (obddataReader.DisconnectRequested)
							{
								taskAwaiter3 = obddataReader.DebugWrite("\r\n[Disconnect requested at connection stage]\r\n").GetAwaiter();
								if (!taskAwaiter3.IsCompleted)
								{
									num = (num2 = 12);
									TaskAwaiter taskAwaiter4 = taskAwaiter3;
									this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, OBDDataReader.<Connect>d__131>(ref taskAwaiter3, ref this);
									return;
								}
							}
							else
							{
								if (!checkELM)
								{
									flag = true;
									goto IL_0D22;
								}
								taskAwaiter5 = obddataReader.CheckIsELM327().GetAwaiter();
								if (!taskAwaiter5.IsCompleted)
								{
									num = (num2 = 13);
									taskAwaiter2 = taskAwaiter5;
									this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<bool>, OBDDataReader.<Connect>d__131>(ref taskAwaiter5, ref this);
									return;
								}
								goto IL_0A0D;
							}
							IL_0976:
							taskAwaiter3.GetResult();
							try
							{
								if (obddataReader.Connection != null)
								{
									obddataReader.Connection.Disconect();
									obddataReader.Connection = null;
								}
							}
							catch (Exception)
							{
							}
							obddataReader.CurrentStatus = OBDDataReaderStatus.Disconnected;
							flag = false;
							goto IL_0D22;
							IL_0A0D:
							bool result = taskAwaiter5.GetResult();
							if (obddataReader.DisconnectRequested)
							{
								taskAwaiter3 = obddataReader.DebugWrite("\r\n[Disconnect requested at connection stage]\r\n").GetAwaiter();
								if (!taskAwaiter3.IsCompleted)
								{
									num = (num2 = 14);
									TaskAwaiter taskAwaiter4 = taskAwaiter3;
									this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, OBDDataReader.<Connect>d__131>(ref taskAwaiter3, ref this);
									return;
								}
							}
							else
							{
								if (result)
								{
									obddataReader.CurrentStatus = OBDDataReaderStatus.ConnectedToELM;
									flag = true;
									goto IL_0D22;
								}
								obddataReader.CurrentStatus = OBDDataReaderStatus.Disconnected;
								goto IL_0C5F;
							}
							IL_0A80:
							taskAwaiter3.GetResult();
							try
							{
								if (obddataReader.Connection != null)
								{
									obddataReader.Connection.Disconect();
									obddataReader.Connection = null;
								}
							}
							catch (Exception)
							{
							}
							obddataReader.CurrentStatus = OBDDataReaderStatus.Disconnected;
							flag = false;
							goto IL_0D22;
							IL_0B56:
							taskAwaiter3.GetResult();
							obddataReader.CurrentStatus = OBDDataReaderStatus.Disconnected;
							goto IL_0C5F;
						}
						catch (Exception obj)
						{
							num3 = 1;
						}
						if (num3 != 1)
						{
							goto IL_0C5F;
						}
						object obj;
						Exception ex = (Exception)obj;
						taskAwaiter3 = obddataReader.DebugWrite("\r\nConnection error: " + ex.ToString() + "\r\n").GetAwaiter();
						if (!taskAwaiter3.IsCompleted)
						{
							num = (num2 = 16);
							TaskAwaiter taskAwaiter4 = taskAwaiter3;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, OBDDataReader.<Connect>d__131>(ref taskAwaiter3, ref this);
							return;
						}
						break;
					}
					case 16:
					{
						TaskAwaiter taskAwaiter4;
						taskAwaiter3 = taskAwaiter4;
						taskAwaiter4 = default(TaskAwaiter);
						num = (num2 = -1);
						break;
					}
					case 17:
					{
						TaskAwaiter taskAwaiter4;
						taskAwaiter3 = taskAwaiter4;
						taskAwaiter4 = default(TaskAwaiter);
						num = (num2 = -1);
						goto IL_0C51;
					}
					case 18:
					{
						TaskAwaiter taskAwaiter4;
						taskAwaiter3 = taskAwaiter4;
						taskAwaiter4 = default(TaskAwaiter);
						num = (num2 = -1);
						goto IL_0CF0;
					}
					default:
						obddataReader.LastAction = OBDDataReader.RemoteDeviceLastAction.Read;
						DeviceID = obddataReader.GetDeviceIdForConnection();
						obddataReader.BadELM = false;
						obddataReader.SendDelay = SharedSettings.Current.SendDelay;
						Attempt = 0;
						goto IL_0C71;
					}
					taskAwaiter3.GetResult();
					taskAwaiter3 = obddataReader.DebugWrite("\r\nConnection failed.\r\n").GetAwaiter();
					if (!taskAwaiter3.IsCompleted)
					{
						num = (num2 = 17);
						TaskAwaiter taskAwaiter4 = taskAwaiter3;
						this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, OBDDataReader.<Connect>d__131>(ref taskAwaiter3, ref this);
						return;
					}
					IL_0C51:
					taskAwaiter3.GetResult();
					obddataReader.CurrentStatus = OBDDataReaderStatus.Disconnected;
					IL_0C5F:
					num3 = Attempt;
					Attempt = num3 + 1;
					IL_0C71:
					if (Attempt >= SharedSettings.Current.ELM327ConnectionAttempts && SharedSettings.Current.ELM327ConnectionAttempts > 0)
					{
						taskAwaiter3 = obddataReader.DebugWrite("Connection attempts failed\r\n").GetAwaiter();
						if (!taskAwaiter3.IsCompleted)
						{
							num = (num2 = 18);
							TaskAwaiter taskAwaiter4 = taskAwaiter3;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, OBDDataReader.<Connect>d__131>(ref taskAwaiter3, ref this);
							return;
						}
					}
					else
					{
						if (obddataReader.DisconnectRequested)
						{
							flag = false;
							goto IL_0D22;
						}
						num3 = 0;
						goto IL_00AC;
					}
					IL_0CF0:
					taskAwaiter3.GetResult();
					obddataReader.CurrentStatus = OBDDataReaderStatus.Disconnected;
					flag = false;
				}
				catch (Exception ex2)
				{
					num2 = -2;
					DeviceID = null;
					this.<>t__builder.SetException(ex2);
					return;
				}
				IL_0D22:
				num2 = -2;
				DeviceID = null;
				this.<>t__builder.SetResult(flag);
			}

			// Token: 0x060025F3 RID: 9715 RVA: 0x001CB29C File Offset: 0x001C949C
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x0400134B RID: 4939
			public int <>1__state;

			// Token: 0x0400134C RID: 4940
			public AsyncTaskMethodBuilder<bool> <>t__builder;

			// Token: 0x0400134D RID: 4941
			public OBDDataReader <>4__this;

			// Token: 0x0400134E RID: 4942
			public bool checkELM;

			// Token: 0x0400134F RID: 4943
			private string <DeviceID>5__2;

			// Token: 0x04001350 RID: 4944
			private int <Attempt>5__3;

			// Token: 0x04001351 RID: 4945
			private TaskAwaiter <>u__1;

			// Token: 0x04001352 RID: 4946
			private TaskAwaiter<bool> <>u__2;
		}

		// Token: 0x02000361 RID: 865
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <DebugWrite>d__81 : IAsyncStateMachine
		{
			// Token: 0x060025F4 RID: 9716 RVA: 0x001CB2AC File Offset: 0x001C94AC
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				try
				{
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
								goto IL_00DC;
							}
							taskAwaiter = PCLDebugStream.CurrentInstance.WriteAsync(data).GetAwaiter();
							if (!taskAwaiter.IsCompleted)
							{
								num2 = 0;
								TaskAwaiter taskAwaiter2 = taskAwaiter;
								this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, OBDDataReader.<DebugWrite>d__81>(ref taskAwaiter, ref this);
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
						if (!SharedSettings.Current.FlushLog)
						{
							goto IL_00E3;
						}
						taskAwaiter = PCLDebugStream.CurrentInstance.Flush().GetAwaiter();
						if (!taskAwaiter.IsCompleted)
						{
							num2 = 1;
							TaskAwaiter taskAwaiter2 = taskAwaiter;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, OBDDataReader.<DebugWrite>d__81>(ref taskAwaiter, ref this);
							return;
						}
						IL_00DC:
						taskAwaiter.GetResult();
						IL_00E3:;
					}
					catch (Exception)
					{
					}
				}
				catch (Exception ex)
				{
					num2 = -2;
					this.<>t__builder.SetException(ex);
					return;
				}
				num2 = -2;
				this.<>t__builder.SetResult();
			}

			// Token: 0x060025F5 RID: 9717 RVA: 0x001CB3EC File Offset: 0x001C95EC
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x04001353 RID: 4947
			public int <>1__state;

			// Token: 0x04001354 RID: 4948
			public AsyncTaskMethodBuilder <>t__builder;

			// Token: 0x04001355 RID: 4949
			public byte[] data;

			// Token: 0x04001356 RID: 4950
			private TaskAwaiter <>u__1;
		}

		// Token: 0x02000362 RID: 866
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <DebugWrite>d__82 : IAsyncStateMachine
		{
			// Token: 0x060025F6 RID: 9718 RVA: 0x001CB3FC File Offset: 0x001C95FC
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				try
				{
					try
					{
						TaskAwaiter taskAwaiter;
						if (num != 0)
						{
							PCLDebugStream.CurrentInstance.WriteByte(data);
							if (!SharedSettings.Current.FlushLog)
							{
								goto IL_0085;
							}
							taskAwaiter = PCLDebugStream.CurrentInstance.Flush().GetAwaiter();
							if (!taskAwaiter.IsCompleted)
							{
								num2 = 0;
								TaskAwaiter taskAwaiter2 = taskAwaiter;
								this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, OBDDataReader.<DebugWrite>d__82>(ref taskAwaiter, ref this);
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
						IL_0085:;
					}
					catch (Exception)
					{
					}
				}
				catch (Exception ex)
				{
					num2 = -2;
					this.<>t__builder.SetException(ex);
					return;
				}
				num2 = -2;
				this.<>t__builder.SetResult();
			}

			// Token: 0x060025F7 RID: 9719 RVA: 0x001CB4DC File Offset: 0x001C96DC
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x04001357 RID: 4951
			public int <>1__state;

			// Token: 0x04001358 RID: 4952
			public AsyncTaskMethodBuilder <>t__builder;

			// Token: 0x04001359 RID: 4953
			public byte data;

			// Token: 0x0400135A RID: 4954
			private TaskAwaiter <>u__1;
		}

		// Token: 0x02000363 RID: 867
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <DebugWrite>d__83 : IAsyncStateMachine
		{
			// Token: 0x060025F8 RID: 9720 RVA: 0x001CB4EC File Offset: 0x001C96EC
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				OBDDataReader obddataReader = this;
				try
				{
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
								goto IL_00EB;
							}
							byte[] bytes = Encoding.UTF8.GetBytes(data);
							taskAwaiter = obddataReader.DebugWrite(bytes).GetAwaiter();
							if (!taskAwaiter.IsCompleted)
							{
								num2 = 0;
								TaskAwaiter taskAwaiter2 = taskAwaiter;
								this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, OBDDataReader.<DebugWrite>d__83>(ref taskAwaiter, ref this);
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
						if (!SharedSettings.Current.FlushLog)
						{
							goto IL_00F2;
						}
						taskAwaiter = PCLDebugStream.CurrentInstance.Flush().GetAwaiter();
						if (!taskAwaiter.IsCompleted)
						{
							num2 = 1;
							TaskAwaiter taskAwaiter2 = taskAwaiter;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, OBDDataReader.<DebugWrite>d__83>(ref taskAwaiter, ref this);
							return;
						}
						IL_00EB:
						taskAwaiter.GetResult();
						IL_00F2:;
					}
					catch (Exception)
					{
					}
				}
				catch (Exception ex)
				{
					num2 = -2;
					this.<>t__builder.SetException(ex);
					return;
				}
				num2 = -2;
				this.<>t__builder.SetResult();
			}

			// Token: 0x060025F9 RID: 9721 RVA: 0x001CB63C File Offset: 0x001C983C
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x0400135B RID: 4955
			public int <>1__state;

			// Token: 0x0400135C RID: 4956
			public AsyncTaskMethodBuilder <>t__builder;

			// Token: 0x0400135D RID: 4957
			public string data;

			// Token: 0x0400135E RID: 4958
			public OBDDataReader <>4__this;

			// Token: 0x0400135F RID: 4959
			private TaskAwaiter <>u__1;
		}

		// Token: 0x02000364 RID: 868
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <DecodeCAN11bit>d__188 : IAsyncStateMachine
		{
			// Token: 0x060025FA RID: 9722 RVA: 0x001CB64C File Offset: 0x001C984C
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				OBDDataReader obddataReader = this;
				Tuple<bool, byte[]> tuple;
				try
				{
					string command;
					if (num != 0)
					{
						if (data == null || data == "" || data.Contains("NO DATA"))
						{
							tuple = new Tuple<bool, byte[]>(false, new byte[0]);
							goto IL_0947;
						}
						command = req.Command;
						lines_with_headers = ListPool<string>.Rent();
					}
					try
					{
						try
						{
							bool flag2;
							ValueTaskAwaiter<bool> valueTaskAwaiter;
							ValueTaskAwaiter<bool> valueTaskAwaiter2;
							if (num != 0)
							{
								OBDDataReader.<>c__DisplayClass188_0 CS$<>8__locals1 = new OBDDataReader.<>c__DisplayClass188_0();
								if (obddataReader.ELMStatus.STNReceiveSegmentation && data != null && data.Contains("<DATA ERROR"))
								{
									if (req.CheckLength)
									{
										tuple = new Tuple<bool, byte[]>(false, null);
										goto IL_0947;
									}
									int num3 = data.IndexOf("<");
									data = data.Substring(0, num3);
								}
								data = OBDDataReader.FilterHexAndNewLineOnly(data);
								string text = null;
								decoded_bytes = null;
								if (!string.IsNullOrEmpty(obddataReader.ELM327_LastSentHeader) || (req.BeforeCommands != null && req.BeforeCommands.Length != 0))
								{
									CS$<>8__locals1.header_filter = CAN11bitHelper.GetPossibleResponseHeader(obddataReader.ELM327_LastSentHeader, SharedSettings.Current.SelectedBrand, req);
								}
								else
								{
									CS$<>8__locals1.header_filter = obddataReader.ECUHeaders[obddataReader.SelectedECU].Id;
								}
								if (override_header_filter != null)
								{
									CS$<>8__locals1.header_filter = override_header_filter;
								}
								string text2 = null;
								CS$<>8__locals1.cmd_response = req.ResponseMarker;
								if (data.IndexOf(' ') >= 0)
								{
									data = data.Replace(" ", "");
								}
								string[] array = data.Split(OBDDataReader.line_splitter, StringSplitOptions.RemoveEmptyEntries);
								bool flag = false;
								CS$<>8__locals1.start_searching_response_from_position = 5;
								if (obddataReader.ELMStatus.STNReceiveSegmentation || (command.Length == 6 && command.StartsWith("AA")))
								{
									req.CheckLength = false;
									CS$<>8__locals1.start_searching_response_from_position = 3;
								}
								foreach (string text3 in array)
								{
									int num4 = text3.IndexOf(CS$<>8__locals1.header_filter, StringComparison.Ordinal);
									if (num4 >= 0 && num4 <= 1 && !text3.Substring(num4 + 3).StartsWith("037F" + req.Command.Substring(0, 2)))
									{
										lines_with_headers.Add(text3);
										if (!flag && text3.IndexOf(CS$<>8__locals1.cmd_response, CS$<>8__locals1.start_searching_response_from_position, StringComparison.Ordinal) >= 0)
										{
											flag = true;
										}
									}
								}
								if (!flag && req.ResponseMarker != null && req.ResponseMarker.Length > 2 && SharedSettings.Current.UseServiceResponseForPositiveResponseMarker)
								{
									string text4 = req.ResponseMarker.Substring(0, 2);
									lines_with_headers.Clear();
									foreach (string text5 in array)
									{
										int num5 = text5.IndexOf(CS$<>8__locals1.header_filter, StringComparison.Ordinal);
										if (num5 >= 0 && num5 <= 1 && !text5.Substring(num5 + 3).StartsWith("037F" + req.Command.Substring(0, 2)))
										{
											lines_with_headers.Add(text5);
											if (!flag && text5.IndexOf(text4, CS$<>8__locals1.start_searching_response_from_position, StringComparison.Ordinal) >= 0)
											{
												flag = true;
												CS$<>8__locals1.cmd_response = text4;
											}
										}
									}
								}
								if (override_header_filter == null && !flag)
								{
									lines_with_headers.Clear();
									List<string> headers = obddataReader.GetHeaders(CS$<>8__locals1.cmd_response, array, req.ELMFormat);
									if (headers.Count > 0)
									{
										CS$<>8__locals1.header_filter = headers.FirstOrDefault((string x) => x != CS$<>8__locals1.header_filter);
										if (CS$<>8__locals1.header_filter == null)
										{
											tuple = new Tuple<bool, byte[]>(false, null);
											goto IL_0947;
										}
										foreach (string text6 in array)
										{
											int num6 = text6.IndexOf(CS$<>8__locals1.header_filter, StringComparison.Ordinal);
											if (num6 >= 0 && num6 <= 1)
											{
												lines_with_headers.Add(text6);
											}
										}
									}
								}
								if (lines_with_headers != null && lines_with_headers.Count > 0)
								{
									if (lines_with_headers.Count == 1)
									{
										string text7 = lines_with_headers[0];
										int num7 = text7.IndexOf(CS$<>8__locals1.cmd_response, CS$<>8__locals1.start_searching_response_from_position, StringComparison.Ordinal);
										if (num7 >= 0)
										{
											text = text7.Substring(num7);
											text2 = text7.Substring(num7 - 2, 2);
										}
									}
									else if (lines_with_headers.Count > 1)
									{
										int num8 = lines_with_headers.FindIndex((string x) => x != null && x.Length > 5 && x.IndexOf(CS$<>8__locals1.cmd_response, CS$<>8__locals1.start_searching_response_from_position, StringComparison.Ordinal) >= 0);
										if (num8 >= 0)
										{
											int num9 = lines_with_headers[num8].IndexOf(CS$<>8__locals1.cmd_response, CS$<>8__locals1.start_searching_response_from_position, StringComparison.Ordinal);
											if (num9 >= 0)
											{
												StringBuilder stringBuilder = new StringBuilder(lines_with_headers.Count);
												int num10 = 0;
												for (int j = num8; j < lines_with_headers.Count; j++)
												{
													string text8;
													if (j == num8)
													{
														num10++;
														text8 = lines_with_headers[j].Substring(num9);
														int num11 = num9 - 3;
														text2 = lines_with_headers[j].Substring(num11, 3);
														if (lines_with_headers[j][num11 - 1] != '1' || ((num11 != 4 || text8.Length != 12) && (num11 != 6 || text8.Length != 10)))
														{
															text2 = lines_with_headers[j].Substring(num11 + 1, 2);
															stringBuilder.Append(text8);
															break;
														}
													}
													else
													{
														string text9 = lines_with_headers[j];
														if (text9[num9 - 4] != '2' || text9[num9 - 3] != num10.ToString("X1")[0])
														{
															break;
														}
														text8 = text9.Substring(num9 - 2);
														num10++;
														if (num10 > 15)
														{
															num10 = 0;
														}
													}
													stringBuilder.Append(text8);
												}
												text = stringBuilder.ToString();
											}
										}
									}
									if (text != null)
									{
										int num12 = BitHelpers.ConvertHexToInt(text2);
										if (obddataReader.ELMStatus.STNReceiveSegmentation)
										{
											num12 = text.Length / 2;
										}
										int num13 = num12 * 2;
										if (text.Length > num13)
										{
											text = text.Substring(0, num13);
										}
										if (req.CheckLength && text.Length < num13)
										{
											ELMState elmstatus = obddataReader.ELMStatus;
											int i = elmstatus.LostCANMultiframeCounter;
											elmstatus.LostCANMultiframeCounter = i + 1;
											tuple = new Tuple<bool, byte[]>(false, null);
											goto IL_0947;
										}
										int num14 = 0;
										int num15 = text.Length;
										if (!command.StartsWith("06", StringComparison.Ordinal))
										{
											if (text.Length > command.Length && !command.StartsWith("2C"))
											{
												if (command.StartsWith("22"))
												{
													num14 = 6;
												}
												else if (command.Length == 6 && command.StartsWith("AA"))
												{
													num14 = CS$<>8__locals1.cmd_response.Length;
												}
												else if (command.StartsWith("01"))
												{
													num14 = CS$<>8__locals1.cmd_response.Length;
												}
												else if (command.StartsWith("21") && command.EndsWith("8001"))
												{
													num14 = CS$<>8__locals1.cmd_response.Length;
												}
												else if (req.OBDMode == OBDDataReader.OBDModes.ReadDTC)
												{
													num14 = req.ResponseMarker.Length;
												}
												else
												{
													num14 = command.Length;
												}
											}
											else if (text.Length > CS$<>8__locals1.cmd_response.Length)
											{
												num14 = CS$<>8__locals1.cmd_response.Length;
											}
										}
										num15 = text.Length - num14;
										if (text.Length % 2 != 0)
										{
											num15--;
										}
										decoded_bytes = BitHelpers.ConvertHexToBytesX(text, num14, num15);
										if (req.OBDMode == OBDDataReader.OBDModes.ReadDTC)
										{
											flag2 = text != null && text.Length > 0;
											tuple = new Tuple<bool, byte[]>(flag2, decoded_bytes);
											goto IL_0947;
										}
										valueTaskAwaiter = obddataReader.CurrentCarData.Decode(command, CS$<>8__locals1.header_filter, req, decoded_bytes, timeStamp).GetAwaiter();
										if (!valueTaskAwaiter.IsCompleted)
										{
											num = (num2 = 0);
											valueTaskAwaiter2 = valueTaskAwaiter;
											this.<>t__builder.AwaitUnsafeOnCompleted<ValueTaskAwaiter<bool>, OBDDataReader.<DecodeCAN11bit>d__188>(ref valueTaskAwaiter, ref this);
											return;
										}
										goto IL_08E6;
									}
								}
								decoded_bytes = null;
								goto IL_090B;
							}
							valueTaskAwaiter = valueTaskAwaiter2;
							valueTaskAwaiter2 = default(ValueTaskAwaiter<bool>);
							num = (num2 = -1);
							IL_08E6:
							flag2 = valueTaskAwaiter.GetResult();
							tuple = new Tuple<bool, byte[]>(flag2, decoded_bytes);
							goto IL_0947;
						}
						catch
						{
						}
						IL_090B:;
					}
					finally
					{
						if (num < 0)
						{
							ListPool<string>.Return(lines_with_headers);
						}
					}
					tuple = new Tuple<bool, byte[]>(false, null);
				}
				catch (Exception ex)
				{
					num2 = -2;
					lines_with_headers = null;
					this.<>t__builder.SetException(ex);
					return;
				}
				IL_0947:
				num2 = -2;
				lines_with_headers = null;
				this.<>t__builder.SetResult(tuple);
			}

			// Token: 0x060025FB RID: 9723 RVA: 0x001CC008 File Offset: 0x001CA208
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x04001360 RID: 4960
			public int <>1__state;

			// Token: 0x04001361 RID: 4961
			public AsyncValueTaskMethodBuilder<Tuple<bool, byte[]>> <>t__builder;

			// Token: 0x04001362 RID: 4962
			public string data;

			// Token: 0x04001363 RID: 4963
			public OBDRequest req;

			// Token: 0x04001364 RID: 4964
			public OBDDataReader <>4__this;

			// Token: 0x04001365 RID: 4965
			public string override_header_filter;

			// Token: 0x04001366 RID: 4966
			public TimeSpan timeStamp;

			// Token: 0x04001367 RID: 4967
			private List<string> <lines_with_headers>5__2;

			// Token: 0x04001368 RID: 4968
			private byte[] <decoded_bytes>5__3;

			// Token: 0x04001369 RID: 4969
			private ValueTaskAwaiter<bool> <>u__1;
		}

		// Token: 0x02000365 RID: 869
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <DecodeCAN11bitOptimized>d__187 : IAsyncStateMachine
		{
			// Token: 0x060025FC RID: 9724 RVA: 0x001CC018 File Offset: 0x001CA218
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				OBDDataReader obddataReader = this;
				Tuple<bool, byte[]> tuple;
				try
				{
					if (num != 0)
					{
						if (data == null || data == "" || data.Contains("NO DATA"))
						{
							tuple = new Tuple<bool, byte[]>(false, new byte[0]);
							goto IL_0597;
						}
						lines_with_headers = ListPool<string>.Rent();
					}
					try
					{
						try
						{
							ValueTaskAwaiter<bool> valueTaskAwaiter3;
							if (num != 0)
							{
								OBDDataReader.<>c__DisplayClass187_0 CS$<>8__locals1 = new OBDDataReader.<>c__DisplayClass187_0();
								if (obddataReader.ELMStatus.STNReceiveSegmentation && data != null && data.Contains("<DATA ERROR"))
								{
									if (request.CheckLength)
									{
										tuple = new Tuple<bool, byte[]>(false, null);
										goto IL_0597;
									}
									int num3 = data.IndexOf("<");
									data = data.Substring(0, num3);
								}
								data = OBDDataReader.FilterHexAndNewLineOnly(data);
								if (!string.IsNullOrEmpty(obddataReader.ELM327_LastSentHeader))
								{
									CS$<>8__locals1.header_filter = CAN11bitHelper.GetPossibleResponseHeader(obddataReader.ELM327_LastSentHeader, SharedSettings.Current.SelectedBrand, request);
								}
								else
								{
									CS$<>8__locals1.header_filter = obddataReader.ECUHeaders[obddataReader.SelectedECU].Id;
								}
								if (override_header != null)
								{
									CS$<>8__locals1.header_filter = override_header;
								}
								CS$<>8__locals1.cmd_response = request.ResponseMarker;
								string[] array = data.Split(OBDDataReader.line_splitter, StringSplitOptions.RemoveEmptyEntries);
								bool flag = false;
								CS$<>8__locals1.start_searching_response_from_position = 5;
								if (obddataReader.ELMStatus.STNReceiveSegmentation)
								{
									request.CheckLength = false;
									CS$<>8__locals1.start_searching_response_from_position = 3;
								}
								foreach (string text in array)
								{
									int num4 = text.IndexOf(CS$<>8__locals1.header_filter, StringComparison.Ordinal);
									if (num4 >= 0 && num4 <= 1)
									{
										lines_with_headers.Add(text);
										if (!flag && text.IndexOf(CS$<>8__locals1.cmd_response, CS$<>8__locals1.start_searching_response_from_position, StringComparison.Ordinal) >= 0)
										{
											flag = true;
										}
									}
								}
								if (override_header == null && !flag)
								{
									lines_with_headers.Clear();
									List<string> headers = obddataReader.GetHeaders(CS$<>8__locals1.cmd_response, array, request.ELMFormat);
									if (headers.Count > 0)
									{
										CS$<>8__locals1.header_filter = headers.FirstOrDefault((string x) => x != CS$<>8__locals1.header_filter);
										if (CS$<>8__locals1.header_filter == null)
										{
											tuple = new Tuple<bool, byte[]>(false, null);
											goto IL_0597;
										}
										foreach (string text2 in array)
										{
											int num5 = text2.IndexOf(CS$<>8__locals1.header_filter, StringComparison.Ordinal);
											if (num5 >= 0 && num5 <= 1)
											{
												lines_with_headers.Add(text2);
											}
										}
									}
								}
								string text3 = null;
								string text4 = null;
								if (lines_with_headers.Count > 0)
								{
									if (lines_with_headers.Count == 1)
									{
										string text5 = lines_with_headers[0];
										int num6 = text5.IndexOf(CS$<>8__locals1.cmd_response, CS$<>8__locals1.start_searching_response_from_position, StringComparison.Ordinal);
										if (num6 >= 0)
										{
											text3 = text5.Substring(num6);
											text4 = text5.Substring(num6 - 2, 2);
										}
									}
									else if (lines_with_headers.Count > 1)
									{
										int num7 = Array.FindIndex<string>(lines_with_headers.ToArray(), (string x) => x.Length > 5 && x.IndexOf(CS$<>8__locals1.cmd_response, CS$<>8__locals1.start_searching_response_from_position, StringComparison.Ordinal) >= 0);
										if (num7 >= 0)
										{
											int num8 = lines_with_headers[num7].IndexOf(CS$<>8__locals1.cmd_response, CS$<>8__locals1.start_searching_response_from_position, StringComparison.Ordinal);
											if (num8 >= 0)
											{
												StringBuilder stringBuilder = new StringBuilder(lines_with_headers.Count);
												int num9 = 0;
												for (int j = num7; j < lines_with_headers.Count; j++)
												{
													string text6;
													if (j == num7)
													{
														num9++;
														text6 = lines_with_headers[j].Substring(num8);
														text4 = lines_with_headers[j].Substring(num8 - 3, 3);
													}
													else
													{
														string text7 = lines_with_headers[j];
														if (text7[num8 - 4] != '2' || text7[num8 - 3] != num9.ToString("X1")[0])
														{
															break;
														}
														text6 = text7.Substring(num8 - 2);
														num9++;
														if (num9 > 15)
														{
															num9 = 0;
														}
													}
													text6 = obddataReader.PrepareResponseLine(text6);
													stringBuilder.Append(text6);
												}
												text3 = stringBuilder.ToString();
											}
										}
									}
									if (text3 != null)
									{
										int num10 = BitHelpers.ConvertHexToInt(text4) * 2;
										int num11 = text3.Length;
										if (text3.Length > num10)
										{
											num11 = num10;
										}
										int num12 = 2;
										num11 -= 2;
										if (text3.Length % 2 != 0)
										{
											num11--;
										}
										if (obddataReader.ELMStatus.STNReceiveSegmentation)
										{
											num11 = text3.Length - num12;
										}
										decoded_bytes = BitHelpers.ConvertHexToBytesX(text3, num12, num11);
										valueTaskAwaiter3 = obddataReader.DecodeMultiResponseCanData(request, timeStamp, CS$<>8__locals1.header_filter, decoded_bytes).GetAwaiter();
										if (!valueTaskAwaiter3.IsCompleted)
										{
											num = (num2 = 0);
											valueTaskAwaiter2 = valueTaskAwaiter3;
											this.<>t__builder.AwaitUnsafeOnCompleted<ValueTaskAwaiter<bool>, OBDDataReader.<DecodeCAN11bitOptimized>d__187>(ref valueTaskAwaiter3, ref this);
											return;
										}
										goto IL_0541;
									}
								}
								goto IL_055B;
							}
							valueTaskAwaiter3 = valueTaskAwaiter2;
							valueTaskAwaiter2 = default(ValueTaskAwaiter<bool>);
							num = (num2 = -1);
							IL_0541:
							tuple = new Tuple<bool, byte[]>(valueTaskAwaiter3.GetResult(), decoded_bytes);
							goto IL_0597;
						}
						catch (Exception)
						{
						}
						IL_055B:;
					}
					finally
					{
						if (num < 0)
						{
							ListPool<string>.Return(lines_with_headers);
						}
					}
					tuple = new Tuple<bool, byte[]>(false, null);
				}
				catch (Exception ex)
				{
					num2 = -2;
					lines_with_headers = null;
					this.<>t__builder.SetException(ex);
					return;
				}
				IL_0597:
				num2 = -2;
				lines_with_headers = null;
				this.<>t__builder.SetResult(tuple);
			}

			// Token: 0x060025FD RID: 9725 RVA: 0x001CC624 File Offset: 0x001CA824
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x0400136A RID: 4970
			public int <>1__state;

			// Token: 0x0400136B RID: 4971
			public AsyncValueTaskMethodBuilder<Tuple<bool, byte[]>> <>t__builder;

			// Token: 0x0400136C RID: 4972
			public string data;

			// Token: 0x0400136D RID: 4973
			public OBDDataReader <>4__this;

			// Token: 0x0400136E RID: 4974
			public OBDMultiRequest request;

			// Token: 0x0400136F RID: 4975
			public string override_header;

			// Token: 0x04001370 RID: 4976
			public TimeSpan timeStamp;

			// Token: 0x04001371 RID: 4977
			private List<string> <lines_with_headers>5__2;

			// Token: 0x04001372 RID: 4978
			private byte[] <decoded_bytes>5__3;

			// Token: 0x04001373 RID: 4979
			private ValueTaskAwaiter<bool> <>u__1;
		}

		// Token: 0x02000366 RID: 870
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <DecodeCAN29bit>d__191 : IAsyncStateMachine
		{
			// Token: 0x060025FE RID: 9726 RVA: 0x001CC634 File Offset: 0x001CA834
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				OBDDataReader obddataReader = this;
				Tuple<bool, byte[]> tuple;
				try
				{
					string command;
					if (num != 0)
					{
						if (data == null || data == "" || data.Contains("NO DATA"))
						{
							tuple = new Tuple<bool, byte[]>(false, new byte[0]);
							goto IL_067A;
						}
						command = req.Command;
						lines_with_headers = ListPool<string>.Rent();
					}
					try
					{
						try
						{
							ValueTaskAwaiter<bool> valueTaskAwaiter3;
							if (num != 0)
							{
								OBDDataReader.<>c__DisplayClass191_0 CS$<>8__locals1 = new OBDDataReader.<>c__DisplayClass191_0();
								if (obddataReader.ELMStatus.STNReceiveSegmentation && data != null && data.Contains("<DATA ERROR"))
								{
									if (req.CheckLength)
									{
										tuple = new Tuple<bool, byte[]>(false, null);
										goto IL_067A;
									}
									int num3 = data.IndexOf("<");
									data = data.Substring(0, num3);
								}
								data = OBDDataReader.FilterHexAndNewLineOnly(data);
								if (!string.IsNullOrEmpty(obddataReader.ELM327_LastSentHeader))
								{
									CS$<>8__locals1.header_filter = CAN29bitHelper.GetPossibleResponseHeaderFilter(obddataReader.ELM327_LastSentHeader, SharedSettings.Current.BrandForDTC, req);
								}
								else
								{
									CS$<>8__locals1.header_filter = obddataReader.ECUHeaders[obddataReader.SelectedECU].Id;
								}
								if (override_header != null)
								{
									CS$<>8__locals1.header_filter = override_header;
								}
								string text = req.ResponseMarker;
								string[] array = data.Split(OBDDataReader.line_splitter, StringSplitOptions.RemoveEmptyEntries);
								bool flag = false;
								int num4 = 10;
								if (obddataReader.ELMStatus.STNReceiveSegmentation)
								{
									req.CheckLength = false;
									num4 = 8;
								}
								foreach (string text2 in array)
								{
									if (text2.Length >= 8 && text2.Substring(6, 2) == CS$<>8__locals1.header_filter)
									{
										if (!flag && text2.IndexOf(text, num4, StringComparison.Ordinal) >= 0)
										{
											flag = true;
										}
										if (flag)
										{
											lines_with_headers.Add(text2);
										}
									}
									else if (text2.Length >= 8 && CS$<>8__locals1.header_filter == "??" && text2.IndexOf(text, num4, StringComparison.Ordinal) >= 0)
									{
										flag = true;
										CS$<>8__locals1.header_filter = text2.Substring(6, 2);
										lines_with_headers.Add(text2);
									}
								}
								if (!flag && req.ResponseMarker != null && req.ResponseMarker.Length > 2 && SharedSettings.Current.UseServiceResponseForPositiveResponseMarker)
								{
									string text3 = req.ResponseMarker.Substring(0, 2);
									lines_with_headers.Clear();
									foreach (string text4 in array)
									{
										if (text4.Length >= 8 && text4.Substring(6, 2) == CS$<>8__locals1.header_filter)
										{
											if (!flag && text4.IndexOf(text3, num4, StringComparison.Ordinal) >= 0)
											{
												text = text3;
												flag = true;
											}
											if (flag)
											{
												lines_with_headers.Add(text4);
											}
										}
									}
								}
								string text5 = null;
								string text6 = null;
								if (override_header == null && !flag)
								{
									lines_with_headers.Clear();
									List<string> headers = obddataReader.GetHeaders(text, array, req.ELMFormat);
									if (headers.Count > 0)
									{
										CS$<>8__locals1.header_filter = headers.FirstOrDefault((string x) => x != CS$<>8__locals1.header_filter);
										if (CS$<>8__locals1.header_filter == null)
										{
											tuple = new Tuple<bool, byte[]>(false, null);
											goto IL_067A;
										}
										foreach (string text7 in array)
										{
											if (!flag && text7.IndexOf(text, num4, StringComparison.Ordinal) >= 0)
											{
												flag = true;
											}
											if (flag && text7.Length >= 8 && text7.Substring(6, 2) == CS$<>8__locals1.header_filter)
											{
												lines_with_headers.Add(text7);
											}
										}
									}
								}
								if (lines_with_headers != null && lines_with_headers.Count > 0)
								{
									if (lines_with_headers.Count == 1)
									{
										string text8 = lines_with_headers[0];
										int num5 = text8.IndexOf(text, num4, StringComparison.Ordinal);
										if (num5 >= 0)
										{
											text6 = text8.Substring(num5);
											text5 = text8.Substring(num5 - 2, 2);
										}
									}
									else if (lines_with_headers.Count > 1)
									{
										int num6 = lines_with_headers[0].IndexOf(text, num4, StringComparison.Ordinal);
										if (num6 >= 0)
										{
											StringBuilder stringBuilder = new StringBuilder(lines_with_headers.Count);
											for (int j = 0; j < lines_with_headers.Count; j++)
											{
												string text9;
												if (j == 0)
												{
													text9 = lines_with_headers[j].Substring(num6);
													text5 = lines_with_headers[j].Substring(num6 - 3, 3);
												}
												else
												{
													text9 = lines_with_headers[j].Substring(num6 - 2);
												}
												text9 = obddataReader.PrepareResponseLine(text9);
												stringBuilder.Append(text9);
											}
											text6 = stringBuilder.ToString();
										}
									}
									if (text6 != null)
									{
										int num7 = BitHelpers.ConvertHexToInt(text5) * 2;
										if (obddataReader.ELMStatus.STNReceiveSegmentation)
										{
											num7 = text6.Length;
										}
										if (text6.Length > num7)
										{
											text6 = text6.Substring(0, num7);
										}
										if (!command.StartsWith("06", StringComparison.Ordinal))
										{
											text6 = text6.Substring(text.Length);
										}
										if (text6.Length % 2 != 0)
										{
											text6 = text6.Substring(0, text6.Length - 1);
										}
										decoded_bytes = BitHelpers.ConvertHexToBytesX(text6);
										valueTaskAwaiter3 = obddataReader.CurrentCarData.Decode(command, CS$<>8__locals1.header_filter, req, decoded_bytes, timeStamp).GetAwaiter();
										if (!valueTaskAwaiter3.IsCompleted)
										{
											num = (num2 = 0);
											valueTaskAwaiter2 = valueTaskAwaiter3;
											this.<>t__builder.AwaitUnsafeOnCompleted<ValueTaskAwaiter<bool>, OBDDataReader.<DecodeCAN29bit>d__191>(ref valueTaskAwaiter3, ref this);
											return;
										}
										goto IL_0624;
									}
								}
								goto IL_063E;
							}
							valueTaskAwaiter3 = valueTaskAwaiter2;
							valueTaskAwaiter2 = default(ValueTaskAwaiter<bool>);
							num = (num2 = -1);
							IL_0624:
							tuple = new Tuple<bool, byte[]>(valueTaskAwaiter3.GetResult(), decoded_bytes);
							goto IL_067A;
						}
						catch (Exception)
						{
						}
						IL_063E:;
					}
					finally
					{
						if (num < 0)
						{
							ListPool<string>.Return(lines_with_headers);
						}
					}
					tuple = new Tuple<bool, byte[]>(false, null);
				}
				catch (Exception ex)
				{
					num2 = -2;
					lines_with_headers = null;
					this.<>t__builder.SetException(ex);
					return;
				}
				IL_067A:
				num2 = -2;
				lines_with_headers = null;
				this.<>t__builder.SetResult(tuple);
			}

			// Token: 0x060025FF RID: 9727 RVA: 0x001CCD24 File Offset: 0x001CAF24
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x04001374 RID: 4980
			public int <>1__state;

			// Token: 0x04001375 RID: 4981
			public AsyncValueTaskMethodBuilder<Tuple<bool, byte[]>> <>t__builder;

			// Token: 0x04001376 RID: 4982
			public string data;

			// Token: 0x04001377 RID: 4983
			public OBDRequest req;

			// Token: 0x04001378 RID: 4984
			public OBDDataReader <>4__this;

			// Token: 0x04001379 RID: 4985
			public string override_header;

			// Token: 0x0400137A RID: 4986
			public TimeSpan timeStamp;

			// Token: 0x0400137B RID: 4987
			private List<string> <lines_with_headers>5__2;

			// Token: 0x0400137C RID: 4988
			private byte[] <decoded_bytes>5__3;

			// Token: 0x0400137D RID: 4989
			private ValueTaskAwaiter<bool> <>u__1;
		}

		// Token: 0x02000367 RID: 871
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <DecodeCAN29bitOptimized>d__189 : IAsyncStateMachine
		{
			// Token: 0x06002600 RID: 9728 RVA: 0x001CCD34 File Offset: 0x001CAF34
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				OBDDataReader obddataReader = this;
				Tuple<bool, byte[]> tuple;
				try
				{
					if (num != 0)
					{
						if (data == null || data == "" || data.Contains("NO DATA"))
						{
							tuple = new Tuple<bool, byte[]>(false, new byte[0]);
							goto IL_0564;
						}
						lines_with_headers = ListPool<string>.Rent();
					}
					try
					{
						try
						{
							ValueTaskAwaiter<bool> valueTaskAwaiter3;
							if (num != 0)
							{
								OBDDataReader.<>c__DisplayClass189_0 CS$<>8__locals1 = new OBDDataReader.<>c__DisplayClass189_0();
								if (obddataReader.ELMStatus.STNReceiveSegmentation && data != null && data.Contains("<DATA ERROR"))
								{
									if (request.CheckLength)
									{
										tuple = new Tuple<bool, byte[]>(false, null);
										goto IL_0564;
									}
									int num3 = data.IndexOf("<");
									data = data.Substring(0, num3);
								}
								data = OBDDataReader.FilterHexAndNewLineOnly(data);
								string[] array = data.Split(OBDDataReader.line_splitter, StringSplitOptions.RemoveEmptyEntries);
								if (!string.IsNullOrEmpty(obddataReader.ELM327_LastSentHeader))
								{
									CS$<>8__locals1.header_filter = CAN29bitHelper.GetPossibleResponseHeaderFilter(obddataReader.ELM327_LastSentHeader, SharedSettings.Current.BrandForDTC, request);
								}
								else
								{
									CS$<>8__locals1.header_filter = obddataReader.ECUHeaders[obddataReader.SelectedECU].Id;
								}
								if (override_header != null)
								{
									CS$<>8__locals1.header_filter = override_header;
								}
								string key = request.Commands.First<KeyValuePair<string, short>>().Key;
								string responseMarker = request.ResponseMarker;
								bool flag = false;
								int num4 = 10;
								if (obddataReader.ELMStatus.STNReceiveSegmentation)
								{
									request.CheckLength = false;
									num4 = 8;
								}
								foreach (string text in array)
								{
									if (text.Length >= 8 && text.Substring(6, 2) == CS$<>8__locals1.header_filter)
									{
										lines_with_headers.Add(text);
										if (text.IndexOf(responseMarker, num4, StringComparison.Ordinal) >= 0)
										{
											flag = true;
										}
									}
									else if (text.Length >= 8 && CS$<>8__locals1.header_filter == "??" && text.IndexOf(responseMarker, num4, StringComparison.Ordinal) >= 0)
									{
										flag = true;
										CS$<>8__locals1.header_filter = text.Substring(6, 2);
										lines_with_headers.Add(text);
									}
								}
								if (CS$<>8__locals1.header_filter == null && !flag)
								{
									lines_with_headers.Clear();
									List<string> headers = obddataReader.GetHeaders(responseMarker, array, request.ELMFormat);
									if (headers.Count > 0)
									{
										CS$<>8__locals1.header_filter = headers.FirstOrDefault((string x) => x != CS$<>8__locals1.header_filter);
										if (CS$<>8__locals1.header_filter == null)
										{
											tuple = new Tuple<bool, byte[]>(false, null);
											goto IL_0564;
										}
										foreach (string text2 in array)
										{
											if (text2.Length >= 8 && text2.Substring(6, 2) == CS$<>8__locals1.header_filter)
											{
												lines_with_headers.Add(text2);
											}
										}
									}
								}
								string text3 = null;
								string text4 = null;
								if (lines_with_headers.Count > 0)
								{
									if (lines_with_headers.Count == 1)
									{
										string text5 = lines_with_headers[0];
										int num5 = text5.IndexOf(responseMarker, num4, StringComparison.Ordinal);
										if (num5 >= 0)
										{
											text3 = text5.Substring(num5);
											text4 = text5.Substring(num5 - 2, 2);
										}
									}
									else if (lines_with_headers.Count > 1)
									{
										int num6 = lines_with_headers[0].IndexOf(responseMarker, num4, StringComparison.Ordinal);
										if (num6 >= 0)
										{
											StringBuilder stringBuilder = new StringBuilder(lines_with_headers.Count);
											for (int j = 0; j < lines_with_headers.Count; j++)
											{
												string text6;
												if (j == 0)
												{
													text6 = lines_with_headers[j].Substring(num6);
													text4 = lines_with_headers[j].Substring(num6 - 2, 2);
												}
												else
												{
													text6 = lines_with_headers[j].Substring(num6 - 2);
												}
												text6 = obddataReader.PrepareResponseLine(text6);
												stringBuilder.Append(text6);
											}
											text3 = stringBuilder.ToString();
										}
									}
									if (text3 != null)
									{
										int num7 = BitHelpers.ConvertHexToInt(text4) * 2;
										if (text3.Length > num7 && !obddataReader.ELMStatus.STNReceiveSegmentation)
										{
											text3 = text3.Substring(0, num7);
										}
										text3 = text3.Substring(2);
										if (text3.Length % 2 != 0)
										{
											text3 = text3.Substring(0, text3.Length - 1);
										}
										decoded_bytes = BitHelpers.ConvertHexToBytesX(text3);
										valueTaskAwaiter3 = obddataReader.DecodeMultiResponseCanData(request, timeStamp, CS$<>8__locals1.header_filter, decoded_bytes).GetAwaiter();
										if (!valueTaskAwaiter3.IsCompleted)
										{
											num = (num2 = 0);
											valueTaskAwaiter2 = valueTaskAwaiter3;
											this.<>t__builder.AwaitUnsafeOnCompleted<ValueTaskAwaiter<bool>, OBDDataReader.<DecodeCAN29bitOptimized>d__189>(ref valueTaskAwaiter3, ref this);
											return;
										}
										goto IL_050E;
									}
								}
								goto IL_0528;
							}
							valueTaskAwaiter3 = valueTaskAwaiter2;
							valueTaskAwaiter2 = default(ValueTaskAwaiter<bool>);
							num = (num2 = -1);
							IL_050E:
							tuple = new Tuple<bool, byte[]>(valueTaskAwaiter3.GetResult(), decoded_bytes);
							goto IL_0564;
						}
						catch
						{
						}
						IL_0528:;
					}
					finally
					{
						if (num < 0)
						{
							ListPool<string>.Return(lines_with_headers);
						}
					}
					tuple = new Tuple<bool, byte[]>(false, null);
				}
				catch (Exception ex)
				{
					num2 = -2;
					lines_with_headers = null;
					this.<>t__builder.SetException(ex);
					return;
				}
				IL_0564:
				num2 = -2;
				lines_with_headers = null;
				this.<>t__builder.SetResult(tuple);
			}

			// Token: 0x06002601 RID: 9729 RVA: 0x001CD30C File Offset: 0x001CB50C
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x0400137E RID: 4990
			public int <>1__state;

			// Token: 0x0400137F RID: 4991
			public AsyncValueTaskMethodBuilder<Tuple<bool, byte[]>> <>t__builder;

			// Token: 0x04001380 RID: 4992
			public string data;

			// Token: 0x04001381 RID: 4993
			public OBDDataReader <>4__this;

			// Token: 0x04001382 RID: 4994
			public OBDMultiRequest request;

			// Token: 0x04001383 RID: 4995
			public string override_header;

			// Token: 0x04001384 RID: 4996
			public TimeSpan timeStamp;

			// Token: 0x04001385 RID: 4997
			private List<string> <lines_with_headers>5__2;

			// Token: 0x04001386 RID: 4998
			private byte[] <decoded_bytes>5__3;

			// Token: 0x04001387 RID: 4999
			private ValueTaskAwaiter<bool> <>u__1;
		}

		// Token: 0x02000368 RID: 872
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <DecodeData>d__195 : IAsyncStateMachine
		{
			// Token: 0x06002602 RID: 9730 RVA: 0x001CD31C File Offset: 0x001CB51C
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				OBDDataReader obddataReader = this;
				bool item;
				try
				{
					ValueTaskAwaiter<Tuple<bool, byte[]>> valueTaskAwaiter;
					Tuple<bool, byte[]> tuple;
					switch (num)
					{
					case 0:
					{
						ValueTaskAwaiter<Tuple<bool, byte[]>> valueTaskAwaiter2;
						valueTaskAwaiter = valueTaskAwaiter2;
						valueTaskAwaiter2 = default(ValueTaskAwaiter<Tuple<bool, byte[]>>);
						num2 = -1;
						break;
					}
					case 1:
					{
						ValueTaskAwaiter<Tuple<bool, byte[]>> valueTaskAwaiter2;
						valueTaskAwaiter = valueTaskAwaiter2;
						valueTaskAwaiter2 = default(ValueTaskAwaiter<Tuple<bool, byte[]>>);
						num2 = -1;
						goto IL_01E9;
					}
					case 2:
					{
						ValueTaskAwaiter<Tuple<bool, byte[]>> valueTaskAwaiter2;
						valueTaskAwaiter = valueTaskAwaiter2;
						valueTaskAwaiter2 = default(ValueTaskAwaiter<Tuple<bool, byte[]>>);
						num2 = -1;
						goto IL_0289;
					}
					case 3:
					{
						ValueTaskAwaiter<Tuple<bool, byte[]>> valueTaskAwaiter2;
						valueTaskAwaiter = valueTaskAwaiter2;
						valueTaskAwaiter2 = default(ValueTaskAwaiter<Tuple<bool, byte[]>>);
						num2 = -1;
						goto IL_0305;
					}
					case 4:
					{
						ValueTaskAwaiter<Tuple<bool, byte[]>> valueTaskAwaiter2;
						valueTaskAwaiter = valueTaskAwaiter2;
						valueTaskAwaiter2 = default(ValueTaskAwaiter<Tuple<bool, byte[]>>);
						num2 = -1;
						goto IL_038C;
					}
					default:
					{
						TimeSpan elapsed = obddataReader.stopwatch.Elapsed;
						string command = request.Command;
						if (request.ELMFormat == ELMFormat.Unknown)
						{
							if (request.BeforeCommands != null && request.BeforeCommands.Length != 0)
							{
								ELMFormat elmformatFromBeforeCommands = obddataReader.GetELMFormatFromBeforeCommands(request.BeforeCommands);
								if (elmformatFromBeforeCommands == ELMFormat.Unknown)
								{
									request.ELMFormat = obddataReader.CurrentELMFormat;
								}
								else
								{
									request.ELMFormat = elmformatFromBeforeCommands;
								}
							}
							else
							{
								request.ELMFormat = obddataReader.CurrentELMFormat;
							}
						}
						switch (request.ELMFormat)
						{
						case ELMFormat.Unknown:
							tuple = new Tuple<bool, byte[]>(false, null);
							goto IL_0395;
						case ELMFormat.CAN11bit:
							if (request is OBDMultiRequest && SharedSettings.Current.CANOptimizeRequests)
							{
								valueTaskAwaiter = obddataReader.DecodeCAN11bitOptimized(data, request as OBDMultiRequest, elapsed, override_header).GetAwaiter();
								if (!valueTaskAwaiter.IsCompleted)
								{
									num2 = 0;
									ValueTaskAwaiter<Tuple<bool, byte[]>> valueTaskAwaiter2 = valueTaskAwaiter;
									this.<>t__builder.AwaitUnsafeOnCompleted<ValueTaskAwaiter<Tuple<bool, byte[]>>, OBDDataReader.<DecodeData>d__195>(ref valueTaskAwaiter, ref this);
									return;
								}
								goto IL_016D;
							}
							else
							{
								valueTaskAwaiter = obddataReader.DecodeCAN11bit(data, request, elapsed, override_header).GetAwaiter();
								if (!valueTaskAwaiter.IsCompleted)
								{
									num2 = 1;
									ValueTaskAwaiter<Tuple<bool, byte[]>> valueTaskAwaiter2 = valueTaskAwaiter;
									this.<>t__builder.AwaitUnsafeOnCompleted<ValueTaskAwaiter<Tuple<bool, byte[]>>, OBDDataReader.<DecodeData>d__195>(ref valueTaskAwaiter, ref this);
									return;
								}
								goto IL_01E9;
							}
							break;
						case ELMFormat.CAN29bit:
							if (request is OBDMultiRequest && SharedSettings.Current.CANOptimizeRequests)
							{
								valueTaskAwaiter = obddataReader.DecodeCAN29bitOptimized(data, request as OBDMultiRequest, elapsed, override_header).GetAwaiter();
								if (!valueTaskAwaiter.IsCompleted)
								{
									num2 = 2;
									ValueTaskAwaiter<Tuple<bool, byte[]>> valueTaskAwaiter2 = valueTaskAwaiter;
									this.<>t__builder.AwaitUnsafeOnCompleted<ValueTaskAwaiter<Tuple<bool, byte[]>>, OBDDataReader.<DecodeData>d__195>(ref valueTaskAwaiter, ref this);
									return;
								}
								goto IL_0289;
							}
							else
							{
								valueTaskAwaiter = obddataReader.DecodeCAN29bit(data, request, elapsed, override_header).GetAwaiter();
								if (!valueTaskAwaiter.IsCompleted)
								{
									num2 = 3;
									ValueTaskAwaiter<Tuple<bool, byte[]>> valueTaskAwaiter2 = valueTaskAwaiter;
									this.<>t__builder.AwaitUnsafeOnCompleted<ValueTaskAwaiter<Tuple<bool, byte[]>>, OBDDataReader.<DecodeData>d__195>(ref valueTaskAwaiter, ref this);
									return;
								}
								goto IL_0305;
							}
							break;
						}
						valueTaskAwaiter = obddataReader.DecodeKWP(data, request, elapsed, override_header).GetAwaiter();
						if (!valueTaskAwaiter.IsCompleted)
						{
							num2 = 4;
							ValueTaskAwaiter<Tuple<bool, byte[]>> valueTaskAwaiter2 = valueTaskAwaiter;
							this.<>t__builder.AwaitUnsafeOnCompleted<ValueTaskAwaiter<Tuple<bool, byte[]>>, OBDDataReader.<DecodeData>d__195>(ref valueTaskAwaiter, ref this);
							return;
						}
						goto IL_038C;
					}
					}
					IL_016D:
					tuple = valueTaskAwaiter.GetResult();
					goto IL_0395;
					IL_01E9:
					tuple = valueTaskAwaiter.GetResult();
					goto IL_0395;
					IL_0289:
					tuple = valueTaskAwaiter.GetResult();
					goto IL_0395;
					IL_0305:
					tuple = valueTaskAwaiter.GetResult();
					goto IL_0395;
					IL_038C:
					tuple = valueTaskAwaiter.GetResult();
					IL_0395:
					try
					{
						request.OnResponseDecoded(tuple.Item2, tuple.Item1, "");
					}
					catch (Exception)
					{
					}
					item = tuple.Item1;
				}
				catch (Exception ex)
				{
					num2 = -2;
					this.<>t__builder.SetException(ex);
					return;
				}
				num2 = -2;
				this.<>t__builder.SetResult(item);
			}

			// Token: 0x06002603 RID: 9731 RVA: 0x001CD750 File Offset: 0x001CB950
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x04001388 RID: 5000
			public int <>1__state;

			// Token: 0x04001389 RID: 5001
			public AsyncValueTaskMethodBuilder<bool> <>t__builder;

			// Token: 0x0400138A RID: 5002
			public OBDDataReader <>4__this;

			// Token: 0x0400138B RID: 5003
			public OBDRequest request;

			// Token: 0x0400138C RID: 5004
			public string data;

			// Token: 0x0400138D RID: 5005
			public string override_header;

			// Token: 0x0400138E RID: 5006
			private ValueTaskAwaiter<Tuple<bool, byte[]>> <>u__1;
		}

		// Token: 0x02000369 RID: 873
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <DecodeKWP>d__193 : IAsyncStateMachine
		{
			// Token: 0x06002604 RID: 9732 RVA: 0x001CD760 File Offset: 0x001CB960
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				OBDDataReader obddataReader = this;
				Tuple<bool, byte[]> tuple;
				try
				{
					string command;
					if (num != 0)
					{
						if (data == null || data == "" || data.Contains("NO DATA"))
						{
							tuple = new Tuple<bool, byte[]>(false, new byte[0]);
							goto IL_06A7;
						}
						command = request.Command;
						lines_with_headers = ListPool<string>.Rent();
					}
					try
					{
						try
						{
							ValueTaskAwaiter<bool> valueTaskAwaiter3;
							if (num != 0)
							{
								OBDDataReader.<>c__DisplayClass193_0 CS$<>8__locals1 = new OBDDataReader.<>c__DisplayClass193_0();
								data = OBDDataReader.FilterHexAndNewLineOnly(data);
								if (!string.IsNullOrEmpty(obddataReader.ELM327_LastSentHeader) && obddataReader.ELM327_LastSentHeader.Length >= 6)
								{
									CS$<>8__locals1.header_filter = obddataReader.ELM327_LastSentHeader.Substring(2, 2);
								}
								else
								{
									CS$<>8__locals1.header_filter = obddataReader.ECUHeaders[obddataReader.SelectedECU].Id;
								}
								if (override_header != null)
								{
									CS$<>8__locals1.header_filter = override_header;
								}
								CS$<>8__locals1.cmd_response = request.ResponseMarker;
								if (SharedSettings.Current.DaihatsuKLine && CS$<>8__locals1.cmd_response.Length == 6)
								{
									CS$<>8__locals1.cmd_response = CS$<>8__locals1.cmd_response.Substring(0, 4);
								}
								if (data.IndexOf(' ') >= 0)
								{
									data = data.Replace(" ", "");
								}
								string[] array = data.Split(OBDDataReader.line_splitter, StringSplitOptions.RemoveEmptyEntries);
								if (array.Length > 4 && SharedSettings.Current.KWPConcatResponseLines)
								{
									StringBuilder stringBuilder = new StringBuilder(array.Length);
									foreach (string text in array)
									{
										if (text.Length >= 2)
										{
											stringBuilder.Append(text);
										}
									}
									array = new string[] { stringBuilder.ToString() };
								}
								bool flag = false;
								foreach (string text2 in array)
								{
									if (text2.Length >= 6 && text2.Substring(4, 2) == CS$<>8__locals1.header_filter)
									{
										lines_with_headers.Add(text2);
										if (!flag && text2.IndexOf(CS$<>8__locals1.cmd_response, 6, StringComparison.Ordinal) >= 0)
										{
											flag = true;
										}
									}
								}
								if (!command.StartsWith(SharedSettings.Current.Mode01Prefix) && lines_with_headers.Count == 1 && array.Length >= 2)
								{
									lines_with_headers = new List<string>(1) { data.Replace("\n", "").Replace("\r", "") };
								}
								if (override_header == null && !flag)
								{
									lines_with_headers.Clear();
									List<string> headers = obddataReader.GetHeaders(CS$<>8__locals1.cmd_response, array, request.ELMFormat);
									if (headers.Count > 0)
									{
										CS$<>8__locals1.header_filter = headers.FirstOrDefault((string x) => x != CS$<>8__locals1.header_filter);
										if (CS$<>8__locals1.header_filter == null)
										{
											tuple = new Tuple<bool, byte[]>(false, null);
											goto IL_06A7;
										}
										foreach (string text3 in array)
										{
											if (text3.Substring(4, 2) == CS$<>8__locals1.header_filter)
											{
												lines_with_headers.Add(text3);
											}
										}
									}
								}
								if ((obddataReader.CurrentProtocolNumber == 1 || obddataReader.CurrentProtocolNumber == 2) && !flag && command.Length >= 2 && lines_with_headers.Count == 0)
								{
									foreach (string text4 in array)
									{
										if (text4.Length >= 8 && text4[6] == command[0] && text4[7] == command[1])
										{
											lines_with_headers.Add(text4);
											CS$<>8__locals1.cmd_response = command.Substring(0, 2);
										}
									}
								}
								if (lines_with_headers != null && lines_with_headers.Count > 0)
								{
									if (SharedSettings.Current.DecodeMUT2Compatible && command.Length == 4 && !command.StartsWith("0", StringComparison.Ordinal) && command.StartsWith("A", StringComparison.Ordinal))
									{
										CS$<>8__locals1.cmd_response = CS$<>8__locals1.cmd_response.Substring(0, 2);
									}
									string text5 = null;
									if (lines_with_headers.Count > 1)
									{
										lines_with_headers = lines_with_headers.SkipWhile((string x) => x.IndexOf(CS$<>8__locals1.cmd_response, 6, StringComparison.Ordinal) < 0).ToList<string>();
										for (int j = 0; j < lines_with_headers.Count; j++)
										{
											string text6 = lines_with_headers[j];
											int num3 = text6.IndexOf(CS$<>8__locals1.cmd_response, 6, StringComparison.Ordinal);
											if (num3 >= 0)
											{
												if (BitHelpers.GetCheckSummHex(text6.Substring(0, text6.Length - 2)) == text6.Substring(text6.Length - 2))
												{
													text6 = text6.Substring(0, text6.Length - 2);
												}
												lines_with_headers[j] = text6.Substring(num3 + CS$<>8__locals1.cmd_response.Length);
											}
										}
										StringBuilder stringBuilder2 = new StringBuilder(lines_with_headers.Count);
										List<string>.Enumerator enumerator = lines_with_headers.GetEnumerator();
										try
										{
											while (enumerator.MoveNext())
											{
												string text7 = enumerator.Current;
												stringBuilder2.Append(text7);
											}
										}
										finally
										{
											if (num < 0)
											{
												((IDisposable)enumerator).Dispose();
											}
										}
										text5 = stringBuilder2.ToString();
									}
									else
									{
										int num4 = lines_with_headers[0].IndexOf(CS$<>8__locals1.cmd_response, 6, StringComparison.Ordinal);
										if (num4 > 0)
										{
											text5 = lines_with_headers[0].Substring(num4 + request.ResponseMarker.Length);
										}
									}
									if (text5 != null)
									{
										decoded_bytes = BitHelpers.ConvertHexToBytesX(text5);
										valueTaskAwaiter3 = obddataReader.CurrentCarData.Decode(command, CS$<>8__locals1.header_filter, request, decoded_bytes, timeStamp).GetAwaiter();
										if (!valueTaskAwaiter3.IsCompleted)
										{
											num = (num2 = 0);
											valueTaskAwaiter2 = valueTaskAwaiter3;
											this.<>t__builder.AwaitUnsafeOnCompleted<ValueTaskAwaiter<bool>, OBDDataReader.<DecodeKWP>d__193>(ref valueTaskAwaiter3, ref this);
											return;
										}
										goto IL_0651;
									}
								}
								goto IL_066B;
							}
							valueTaskAwaiter3 = valueTaskAwaiter2;
							valueTaskAwaiter2 = default(ValueTaskAwaiter<bool>);
							num = (num2 = -1);
							IL_0651:
							tuple = new Tuple<bool, byte[]>(valueTaskAwaiter3.GetResult(), decoded_bytes);
							goto IL_06A7;
						}
						catch (Exception)
						{
						}
						IL_066B:;
					}
					finally
					{
						if (num < 0)
						{
							ListPool<string>.Return(lines_with_headers);
						}
					}
					tuple = new Tuple<bool, byte[]>(false, null);
				}
				catch (Exception ex)
				{
					num2 = -2;
					lines_with_headers = null;
					this.<>t__builder.SetException(ex);
					return;
				}
				IL_06A7:
				num2 = -2;
				lines_with_headers = null;
				this.<>t__builder.SetResult(tuple);
			}

			// Token: 0x06002605 RID: 9733 RVA: 0x001CDE94 File Offset: 0x001CC094
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x0400138F RID: 5007
			public int <>1__state;

			// Token: 0x04001390 RID: 5008
			public AsyncValueTaskMethodBuilder<Tuple<bool, byte[]>> <>t__builder;

			// Token: 0x04001391 RID: 5009
			public string data;

			// Token: 0x04001392 RID: 5010
			public OBDRequest request;

			// Token: 0x04001393 RID: 5011
			public OBDDataReader <>4__this;

			// Token: 0x04001394 RID: 5012
			public string override_header;

			// Token: 0x04001395 RID: 5013
			public TimeSpan timeStamp;

			// Token: 0x04001396 RID: 5014
			private List<string> <lines_with_headers>5__2;

			// Token: 0x04001397 RID: 5015
			private byte[] <decoded_bytes>5__3;

			// Token: 0x04001398 RID: 5016
			private ValueTaskAwaiter<bool> <>u__1;
		}

		// Token: 0x0200036A RID: 874
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <DecodeMode06Data>d__185 : IAsyncStateMachine
		{
			// Token: 0x06002606 RID: 9734 RVA: 0x001CDEA4 File Offset: 0x001CC0A4
			void IAsyncStateMachine.MoveNext()
			{
				OBDDataReader obddataReader = this;
				bool flag2;
				try
				{
					int length = cmd.Length;
					bool flag = false;
					try
					{
						OBDDataReader.<>c__DisplayClass185_0 CS$<>8__locals1 = new OBDDataReader.<>c__DisplayClass185_0();
						if (data.Length < 2 || data.Contains("NO DATA") || data.Contains("ELM"))
						{
							flag = false;
							flag2 = flag;
							goto IL_035C;
						}
						CS$<>8__locals1.header_filter = obddataReader.ECUHeaders[obddataReader.SelectedECU].Id;
						string text = "4" + cmd.Substring(1);
						CS$<>8__locals1.IsCAN = false;
						string[] array;
						if (CS$<>8__locals1.header_filter.Length == 3)
						{
							array = (from x in data.Replace(" ", string.Empty).Split(OBDDataReader.line_splitter, StringSplitOptions.RemoveEmptyEntries)
								select x.Trim() into x
								where x.StartsWith(CS$<>8__locals1.header_filter)
								select x).ToArray<string>();
							CS$<>8__locals1.IsCAN = true;
						}
						else
						{
							array = (from x in data.Replace(" ", string.Empty).Split(OBDDataReader.line_splitter, StringSplitOptions.RemoveEmptyEntries)
								select x.Trim()).Where(delegate(string x)
							{
								int num3 = x.IndexOf(CS$<>8__locals1.header_filter);
								if (num3 % 2 != 0)
								{
									num3 = x.IndexOf(CS$<>8__locals1.header_filter, num3 + 1);
								}
								if (num3 >= 6 && num3 <= 8 && num3 <= x.Length - CS$<>8__locals1.header_filter.Length - 2)
								{
									CS$<>8__locals1.IsCAN = true;
									return true;
								}
								return num3 == 4 && num3 <= x.Length - CS$<>8__locals1.header_filter.Length - 2;
							}).ToArray<string>();
						}
						if (array.Length == 0)
						{
							flag = false;
						}
						else
						{
							StringBuilder stringBuilder = new StringBuilder(array.Length);
							for (int i = 0; i < array.Length; i++)
							{
								string text2 = obddataReader.PrepareResponseLine(array[i]);
								int num = text2.IndexOf(CS$<>8__locals1.header_filter);
								if (num != 0 && num % 2 != 0)
								{
									num = text2.IndexOf(CS$<>8__locals1.header_filter, num + 1);
								}
								array[i] = text2.Substring(num + CS$<>8__locals1.header_filter.Length);
							}
							array = array.OrderBy((string x) => x).ToArray<string>();
							for (int j = 0; j < array.Length; j++)
							{
								string text3;
								if (j == 0)
								{
									text3 = "1";
								}
								else
								{
									text3 = "2";
								}
								if (array[j].Substring(0, 1) == text3)
								{
									array[j] = array[j].Substring(2);
								}
								stringBuilder.Append(array[j]);
							}
							string text4 = stringBuilder.ToString();
							int num2 = text4.IndexOf(text, StringComparison.Ordinal);
							if (num2 >= 0)
							{
								if (cmd.StartsWith("06", StringComparison.Ordinal))
								{
									text4 = text4.Substring(num2 + 2);
								}
								else
								{
									text4 = text4.Substring(num2 + cmd.Length);
								}
							}
							if (string.IsNullOrEmpty(text4))
							{
								flag = false;
							}
							else
							{
								byte[] array2 = BitHelpers.ConvertHexToBytesX(text4);
								if (cmd.StartsWith("06", StringComparison.Ordinal))
								{
									if (obddataReader.CurrentMode == OBDDataReader.OBDModes.Mode06)
									{
										flag = obddataReader.CurrentCarData.DecodeMode06(cmd, array2, CS$<>8__locals1.IsCAN);
									}
									else
									{
										flag = obddataReader.CurrentCarData.DecodeWithoutRequest(cmd, array2, obddataReader.stopwatch.Elapsed, "", CS$<>8__locals1.header_filter);
									}
								}
							}
						}
					}
					catch (Exception)
					{
						flag = false;
					}
					flag2 = flag;
				}
				catch (Exception ex)
				{
					this.<>1__state = -2;
					this.<>t__builder.SetException(ex);
					return;
				}
				IL_035C:
				this.<>1__state = -2;
				this.<>t__builder.SetResult(flag2);
			}

			// Token: 0x06002607 RID: 9735 RVA: 0x001CE258 File Offset: 0x001CC458
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x04001399 RID: 5017
			public int <>1__state;

			// Token: 0x0400139A RID: 5018
			public AsyncTaskMethodBuilder<bool> <>t__builder;

			// Token: 0x0400139B RID: 5019
			public string cmd;

			// Token: 0x0400139C RID: 5020
			public string data;

			// Token: 0x0400139D RID: 5021
			public OBDDataReader <>4__this;
		}

		// Token: 0x0200036B RID: 875
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <DecodeMultiResponseCanData>d__190 : IAsyncStateMachine
		{
			// Token: 0x06002608 RID: 9736 RVA: 0x001CE268 File Offset: 0x001CC468
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				OBDDataReader obddataReader = this;
				bool flag;
				try
				{
					ValueTaskAwaiter<bool> valueTaskAwaiter3;
					int num3;
					if (num != 0)
					{
						if (num != 1)
						{
							result = true;
							ms = new MemoryStream(decoded_bytes);
							counter = 0;
							if (request.Command.StartsWith("01"))
							{
								goto IL_019D;
							}
							if (request.Command.StartsWith("22"))
							{
								goto IL_0367;
							}
							goto IL_03A7;
						}
						else
						{
							valueTaskAwaiter3 = valueTaskAwaiter2;
							valueTaskAwaiter2 = default(ValueTaskAwaiter<bool>);
							num2 = -1;
						}
						IL_0343:
						if (!valueTaskAwaiter3.GetResult())
						{
							result = false;
							goto IL_03A7;
						}
						num3 = counter;
						counter = num3 + 1;
						IL_0367:
						if (!ms.CanRead || ms.Position >= ms.Length || counter >= request.Commands.Count)
						{
							goto IL_03A7;
						}
						int num4 = ms.ReadByte();
						int num5 = ms.ReadByte();
						string text = "22" + num4.ToString("X2", CultureInfo.InvariantCulture) + num5.ToString("X2", CultureInfo.InvariantCulture);
						if (!request.Commands.ContainsKey(text))
						{
							SharedSettings sharedSettings = SharedSettings.Current;
							num3 = sharedSettings.OptimizedRequestStuckCounter;
							sharedSettings.OptimizedRequestStuckCounter = num3 + 1;
							flag = false;
							goto IL_03D0;
						}
						int num6 = (int)request.Commands[text];
						byte[] array = new byte[num6];
						if (ms.Length - ms.Position < (long)num6)
						{
							goto IL_03A7;
						}
						ms.Read(array, 0, num6);
						valueTaskAwaiter3 = obddataReader.CurrentCarData.Decode(text, header_filter, request, array, timeStamp).GetAwaiter();
						if (!valueTaskAwaiter3.IsCompleted)
						{
							num2 = 1;
							valueTaskAwaiter2 = valueTaskAwaiter3;
							this.<>t__builder.AwaitUnsafeOnCompleted<ValueTaskAwaiter<bool>, OBDDataReader.<DecodeMultiResponseCanData>d__190>(ref valueTaskAwaiter3, ref this);
							return;
						}
						goto IL_0343;
					}
					else
					{
						valueTaskAwaiter3 = valueTaskAwaiter2;
						valueTaskAwaiter2 = default(ValueTaskAwaiter<bool>);
						num2 = -1;
					}
					IL_0176:
					if (!valueTaskAwaiter3.GetResult())
					{
						result = false;
						goto IL_03A7;
					}
					num3 = counter;
					counter = num3 + 1;
					IL_019D:
					if (ms.CanRead && ms.Position < ms.Length)
					{
						if (counter < request.Commands.Count)
						{
							string text2 = "01" + ms.ReadByte().ToString("X2", CultureInfo.InvariantCulture);
							if (!request.Commands.ContainsKey(text2))
							{
								SharedSettings sharedSettings2 = SharedSettings.Current;
								num3 = sharedSettings2.OptimizedRequestStuckCounter;
								sharedSettings2.OptimizedRequestStuckCounter = num3 + 1;
								flag = false;
								goto IL_03D0;
							}
							int num7 = (int)request.Commands[text2];
							byte[] array2 = new byte[num7];
							if (ms.Length - ms.Position >= (long)num7)
							{
								ms.Read(array2, 0, num7);
								valueTaskAwaiter3 = obddataReader.CurrentCarData.Decode(text2, header_filter, request, array2, timeStamp).GetAwaiter();
								if (!valueTaskAwaiter3.IsCompleted)
								{
									num2 = 0;
									valueTaskAwaiter2 = valueTaskAwaiter3;
									this.<>t__builder.AwaitUnsafeOnCompleted<ValueTaskAwaiter<bool>, OBDDataReader.<DecodeMultiResponseCanData>d__190>(ref valueTaskAwaiter3, ref this);
									return;
								}
								goto IL_0176;
							}
						}
					}
					IL_03A7:
					flag = result;
				}
				catch (Exception ex)
				{
					num2 = -2;
					ms = null;
					this.<>t__builder.SetException(ex);
					return;
				}
				IL_03D0:
				num2 = -2;
				ms = null;
				this.<>t__builder.SetResult(flag);
			}

			// Token: 0x06002609 RID: 9737 RVA: 0x001CE67C File Offset: 0x001CC87C
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x0400139E RID: 5022
			public int <>1__state;

			// Token: 0x0400139F RID: 5023
			public AsyncValueTaskMethodBuilder<bool> <>t__builder;

			// Token: 0x040013A0 RID: 5024
			public byte[] decoded_bytes;

			// Token: 0x040013A1 RID: 5025
			public OBDMultiRequest request;

			// Token: 0x040013A2 RID: 5026
			public OBDDataReader <>4__this;

			// Token: 0x040013A3 RID: 5027
			public string header_filter;

			// Token: 0x040013A4 RID: 5028
			public TimeSpan timeStamp;

			// Token: 0x040013A5 RID: 5029
			private bool <result>5__2;

			// Token: 0x040013A6 RID: 5030
			private MemoryStream <ms>5__3;

			// Token: 0x040013A7 RID: 5031
			private int <counter>5__4;

			// Token: 0x040013A8 RID: 5032
			private ValueTaskAwaiter<bool> <>u__1;
		}

		// Token: 0x0200036C RID: 876
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <DecodeVWTPData>d__148 : IAsyncStateMachine
		{
			// Token: 0x0600260A RID: 9738 RVA: 0x001CE68C File Offset: 0x001CC88C
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				OBDDataReader obddataReader = this;
				bool flag;
				try
				{
					TimeSpan elapsed;
					if (num != 0)
					{
						elapsed = obddataReader.stopwatch.Elapsed;
						if (string.IsNullOrEmpty(request_result) || request_result.Contains("NO DATA"))
						{
							request.OnResponseDecoded(EmptyArrays.EmptyByteArray, false, "");
							flag = false;
							goto IL_0184;
						}
					}
					try
					{
						ValueTaskAwaiter<bool> valueTaskAwaiter;
						if (num != 0)
						{
							byte[] array = BitHelpers.ConvertHexToBytesX(request_result);
							int num3 = request.ResponseMarker.Length / 2 + 2;
							dataWithoutMarker = new byte[array.Length - num3];
							Array.Copy(array, num3, dataWithoutMarker, 0, dataWithoutMarker.Length);
							valueTaskAwaiter = obddataReader.CurrentCarData.Decode(request.Command, "000", request, dataWithoutMarker, elapsed).GetAwaiter();
							if (!valueTaskAwaiter.IsCompleted)
							{
								num2 = 0;
								ValueTaskAwaiter<bool> valueTaskAwaiter2 = valueTaskAwaiter;
								this.<>t__builder.AwaitUnsafeOnCompleted<ValueTaskAwaiter<bool>, OBDDataReader.<DecodeVWTPData>d__148>(ref valueTaskAwaiter, ref this);
								return;
							}
						}
						else
						{
							ValueTaskAwaiter<bool> valueTaskAwaiter2;
							valueTaskAwaiter = valueTaskAwaiter2;
							valueTaskAwaiter2 = default(ValueTaskAwaiter<bool>);
							num2 = -1;
						}
						bool result = valueTaskAwaiter.GetResult();
						request.OnResponseDecoded(dataWithoutMarker, result, "");
						flag = result;
					}
					catch (Exception)
					{
						request.OnResponseDecoded(EmptyArrays.EmptyByteArray, false, "");
						flag = false;
					}
				}
				catch (Exception ex)
				{
					num2 = -2;
					this.<>t__builder.SetException(ex);
					return;
				}
				IL_0184:
				num2 = -2;
				this.<>t__builder.SetResult(flag);
			}

			// Token: 0x0600260B RID: 9739 RVA: 0x001CE868 File Offset: 0x001CCA68
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x040013A9 RID: 5033
			public int <>1__state;

			// Token: 0x040013AA RID: 5034
			public AsyncValueTaskMethodBuilder<bool> <>t__builder;

			// Token: 0x040013AB RID: 5035
			public OBDDataReader <>4__this;

			// Token: 0x040013AC RID: 5036
			public string request_result;

			// Token: 0x040013AD RID: 5037
			public OBDRequest request;

			// Token: 0x040013AE RID: 5038
			private byte[] <dataWithoutMarker>5__2;

			// Token: 0x040013AF RID: 5039
			private ValueTaskAwaiter<bool> <>u__1;
		}

		// Token: 0x0200036D RID: 877
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <Disconnect>d__198 : IAsyncStateMachine
		{
			// Token: 0x0600260C RID: 9740 RVA: 0x001CE878 File Offset: 0x001CCA78
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				OBDDataReader obddataReader = this;
				try
				{
					if (num > 3)
					{
						obddataReader.CurrentStatus = OBDDataReaderStatus.Disconnecting;
						obddataReader.DisconnectRequested = true;
						obddataReader.BadELM = false;
					}
					try
					{
						TaskAwaiter taskAwaiter;
						switch (num)
						{
						case 0:
						{
							TaskAwaiter taskAwaiter2;
							taskAwaiter = taskAwaiter2;
							taskAwaiter2 = default(TaskAwaiter);
							num2 = -1;
							break;
						}
						case 1:
						{
							TaskAwaiter taskAwaiter2;
							taskAwaiter = taskAwaiter2;
							taskAwaiter2 = default(TaskAwaiter);
							num2 = -1;
							goto IL_00F3;
						}
						case 2:
						{
							TaskAwaiter taskAwaiter2;
							taskAwaiter = taskAwaiter2;
							taskAwaiter2 = default(TaskAwaiter);
							num2 = -1;
							goto IL_015E;
						}
						case 3:
						{
							TaskAwaiter taskAwaiter2;
							taskAwaiter = taskAwaiter2;
							taskAwaiter2 = default(TaskAwaiter);
							num2 = -1;
							goto IL_01BD;
						}
						default:
							taskAwaiter = obddataReader.ClearRequestQueue().GetAwaiter();
							if (!taskAwaiter.IsCompleted)
							{
								num2 = 0;
								TaskAwaiter taskAwaiter2 = taskAwaiter;
								this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, OBDDataReader.<Disconnect>d__198>(ref taskAwaiter, ref this);
								return;
							}
							break;
						}
						taskAwaiter.GetResult();
						taskAwaiter = Task.Delay(250).GetAwaiter();
						if (!taskAwaiter.IsCompleted)
						{
							num2 = 1;
							TaskAwaiter taskAwaiter2 = taskAwaiter;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, OBDDataReader.<Disconnect>d__198>(ref taskAwaiter, ref this);
							return;
						}
						IL_00F3:
						taskAwaiter.GetResult();
						taskAwaiter = obddataReader.Stop("Disconnect:" + message).GetAwaiter();
						if (!taskAwaiter.IsCompleted)
						{
							num2 = 2;
							TaskAwaiter taskAwaiter2 = taskAwaiter;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, OBDDataReader.<Disconnect>d__198>(ref taskAwaiter, ref this);
							return;
						}
						IL_015E:
						taskAwaiter.GetResult();
						taskAwaiter = Task.Delay(500).GetAwaiter();
						if (!taskAwaiter.IsCompleted)
						{
							num2 = 3;
							TaskAwaiter taskAwaiter2 = taskAwaiter;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, OBDDataReader.<Disconnect>d__198>(ref taskAwaiter, ref this);
							return;
						}
						IL_01BD:
						taskAwaiter.GetResult();
						if (obddataReader.Connection != null)
						{
							obddataReader.Connection.Disconect();
							obddataReader.Connection = null;
						}
					}
					catch (Exception)
					{
					}
					obddataReader.CurrentStatus = OBDDataReaderStatus.Disconnected;
					try
					{
						obddataReader.SelectedECU = 0;
						obddataReader.InvokeOnMainThread(delegate
						{
							base.ECUHeaders.Clear();
						});
					}
					catch (Exception)
					{
					}
				}
				catch (Exception ex)
				{
					num2 = -2;
					this.<>t__builder.SetException(ex);
					return;
				}
				num2 = -2;
				this.<>t__builder.SetResult();
			}

			// Token: 0x0600260D RID: 9741 RVA: 0x001CEB08 File Offset: 0x001CCD08
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x040013B0 RID: 5040
			public int <>1__state;

			// Token: 0x040013B1 RID: 5041
			public AsyncTaskMethodBuilder <>t__builder;

			// Token: 0x040013B2 RID: 5042
			public OBDDataReader <>4__this;

			// Token: 0x040013B3 RID: 5043
			public string message;

			// Token: 0x040013B4 RID: 5044
			private TaskAwaiter <>u__1;
		}

		// Token: 0x0200036E RID: 878
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <GetCurrentProtocolNumber>d__108 : IAsyncStateMachine
		{
			// Token: 0x0600260E RID: 9742 RVA: 0x001CEB18 File Offset: 0x001CCD18
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				OBDDataReader obddataReader = this;
				int num3;
				try
				{
					TaskAwaiter<string> taskAwaiter;
					TaskAwaiter taskAwaiter3;
					if (num != 0)
					{
						if (num == 1)
						{
							TaskAwaiter<string> taskAwaiter2;
							taskAwaiter = taskAwaiter2;
							taskAwaiter2 = default(TaskAwaiter<string>);
							num2 = -1;
							goto IL_00D3;
						}
						taskAwaiter3 = obddataReader.SendString("ATDPN").GetAwaiter();
						if (!taskAwaiter3.IsCompleted)
						{
							num2 = 0;
							TaskAwaiter taskAwaiter4 = taskAwaiter3;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, OBDDataReader.<GetCurrentProtocolNumber>d__108>(ref taskAwaiter3, ref this);
							return;
						}
					}
					else
					{
						TaskAwaiter taskAwaiter4;
						taskAwaiter3 = taskAwaiter4;
						taskAwaiter4 = default(TaskAwaiter);
						num2 = -1;
					}
					taskAwaiter3.GetResult();
					taskAwaiter = obddataReader.ReadData(2500, null, -1).GetAwaiter();
					if (!taskAwaiter.IsCompleted)
					{
						num2 = 1;
						TaskAwaiter<string> taskAwaiter2 = taskAwaiter;
						this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<string>, OBDDataReader.<GetCurrentProtocolNumber>d__108>(ref taskAwaiter, ref this);
						return;
					}
					IL_00D3:
					num3 = OBDResponseAnalyzer.ParseProtocolNumberResponseString(taskAwaiter.GetResult());
				}
				catch (Exception ex)
				{
					num2 = -2;
					this.<>t__builder.SetException(ex);
					return;
				}
				num2 = -2;
				this.<>t__builder.SetResult(num3);
			}

			// Token: 0x0600260F RID: 9743 RVA: 0x001CEC44 File Offset: 0x001CCE44
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x040013B5 RID: 5045
			public int <>1__state;

			// Token: 0x040013B6 RID: 5046
			public AsyncTaskMethodBuilder<int> <>t__builder;

			// Token: 0x040013B7 RID: 5047
			public OBDDataReader <>4__this;

			// Token: 0x040013B8 RID: 5048
			private TaskAwaiter <>u__1;

			// Token: 0x040013B9 RID: 5049
			private TaskAwaiter<string> <>u__2;
		}

		// Token: 0x0200036F RID: 879
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <GetViecarDeviceId>d__130 : IAsyncStateMachine
		{
			// Token: 0x06002610 RID: 9744 RVA: 0x001CEC54 File Offset: 0x001CCE54
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				OBDDataReader obddataReader = this;
				string text;
				try
				{
					TaskAwaiter taskAwaiter;
					switch (num)
					{
					case 0:
					{
						TaskAwaiter taskAwaiter2;
						taskAwaiter = taskAwaiter2;
						taskAwaiter2 = default(TaskAwaiter);
						num2 = -1;
						break;
					}
					case 1:
					{
						TaskAwaiter taskAwaiter2;
						taskAwaiter = taskAwaiter2;
						taskAwaiter2 = default(TaskAwaiter);
						num2 = -1;
						goto IL_0160;
					}
					case 2:
					{
						TaskAwaiter taskAwaiter2;
						taskAwaiter = taskAwaiter2;
						taskAwaiter2 = default(TaskAwaiter);
						num2 = -1;
						goto IL_01D6;
					}
					default:
						obddataReader.btle_devices = new ObservableCollection<IDevice>();
						CrossBluetoothLE.Current.Adapter.DeviceDiscovered += delegate(object sender, DeviceEventArgs e)
						{
							obddataReader.btle_devices.Add(e.Device);
						};
						taskAwaiter = CrossBluetoothLE.Current.Adapter.StartScanningForDevicesAsync(null, null, false, default(CancellationToken)).GetAwaiter();
						if (!taskAwaiter.IsCompleted)
						{
							num2 = 0;
							TaskAwaiter taskAwaiter2 = taskAwaiter;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, OBDDataReader.<GetViecarDeviceId>d__130>(ref taskAwaiter, ref this);
							return;
						}
						break;
					}
					taskAwaiter.GetResult();
					IL_00B7:
					viecardevice = obddataReader.btle_devices.FirstOrDefault((IDevice x) => x.Name.ToLower().Contains("viecar"));
					if (viecardevice != null)
					{
						if (!CrossBluetoothLE.Current.Adapter.IsScanning)
						{
							goto IL_0167;
						}
						taskAwaiter = CrossBluetoothLE.Current.Adapter.StopScanningForDevicesAsync().GetAwaiter();
						if (!taskAwaiter.IsCompleted)
						{
							num2 = 1;
							TaskAwaiter taskAwaiter2 = taskAwaiter;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, OBDDataReader.<GetViecarDeviceId>d__130>(ref taskAwaiter, ref this);
							return;
						}
					}
					else
					{
						taskAwaiter = Task.Delay(50).GetAwaiter();
						if (!taskAwaiter.IsCompleted)
						{
							num2 = 2;
							TaskAwaiter taskAwaiter2 = taskAwaiter;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, OBDDataReader.<GetViecarDeviceId>d__130>(ref taskAwaiter, ref this);
							return;
						}
						goto IL_01D6;
					}
					IL_0160:
					taskAwaiter.GetResult();
					IL_0167:
					text = viecardevice.Id.ToString();
					goto IL_0202;
					IL_01D6:
					taskAwaiter.GetResult();
					viecardevice = null;
					goto IL_00B7;
				}
				catch (Exception ex)
				{
					num2 = -2;
					this.<>t__builder.SetException(ex);
					return;
				}
				IL_0202:
				num2 = -2;
				this.<>t__builder.SetResult(text);
			}

			// Token: 0x06002611 RID: 9745 RVA: 0x001CEE94 File Offset: 0x001CD094
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x040013BA RID: 5050
			public int <>1__state;

			// Token: 0x040013BB RID: 5051
			public AsyncTaskMethodBuilder<string> <>t__builder;

			// Token: 0x040013BC RID: 5052
			public OBDDataReader <>4__this;

			// Token: 0x040013BD RID: 5053
			private TaskAwaiter <>u__1;

			// Token: 0x040013BE RID: 5054
			private IDevice <viecardevice>5__2;
		}

		// Token: 0x02000370 RID: 880
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <Initialize>d__168 : IAsyncStateMachine
		{
			// Token: 0x06002612 RID: 9746 RVA: 0x001CEEA4 File Offset: 0x001CD0A4
			void IAsyncStateMachine.MoveNext()
			{
				int num4;
				int num3 = num4;
				OBDDataReader obddataReader = this;
				bool flag2;
				try
				{
					TaskAwaiter taskAwaiter3;
					int num6;
					switch (num3)
					{
					case 0:
					{
						TaskAwaiter taskAwaiter4;
						taskAwaiter3 = taskAwaiter4;
						taskAwaiter4 = default(TaskAwaiter);
						num3 = (num4 = -1);
						break;
					}
					case 1:
					case 2:
					case 3:
					case 4:
					case 5:
					case 6:
					case 7:
					case 8:
					case 9:
					case 10:
					case 11:
					{
						IL_00FD:
						try
						{
							TaskAwaiter<bool> taskAwaiter5;
							IProgress<string> progress;
							int num5;
							TaskAwaiter<int> taskAwaiter6;
							switch (num3)
							{
							case 1:
							{
								TaskAwaiter taskAwaiter4;
								taskAwaiter3 = taskAwaiter4;
								taskAwaiter4 = default(TaskAwaiter);
								num3 = (num4 = -1);
								break;
							}
							case 2:
							{
								TaskAwaiter taskAwaiter4;
								taskAwaiter3 = taskAwaiter4;
								taskAwaiter4 = default(TaskAwaiter);
								num3 = (num4 = -1);
								goto IL_03B5;
							}
							case 3:
							{
								TaskAwaiter taskAwaiter4;
								taskAwaiter3 = taskAwaiter4;
								taskAwaiter4 = default(TaskAwaiter);
								num3 = (num4 = -1);
								goto IL_0429;
							}
							case 4:
							{
								TaskAwaiter taskAwaiter4;
								taskAwaiter3 = taskAwaiter4;
								taskAwaiter4 = default(TaskAwaiter);
								num3 = (num4 = -1);
								goto IL_04B8;
							}
							case 5:
							case 6:
							case 7:
							case 8:
							{
								IL_04F0:
								try
								{
									switch (num3)
									{
									case 5:
										taskAwaiter5 = taskAwaiter2;
										taskAwaiter2 = default(TaskAwaiter<bool>);
										num3 = (num4 = -1);
										break;
									case 6:
										taskAwaiter5 = taskAwaiter2;
										taskAwaiter2 = default(TaskAwaiter<bool>);
										num3 = (num4 = -1);
										goto IL_060D;
									case 7:
										taskAwaiter5 = taskAwaiter2;
										taskAwaiter2 = default(TaskAwaiter<bool>);
										num3 = (num4 = -1);
										goto IL_069B;
									case 8:
										taskAwaiter5 = taskAwaiter2;
										taskAwaiter2 = default(TaskAwaiter<bool>);
										num3 = (num4 = -1);
										goto IL_0764;
									default:
										if (UseCustom)
										{
											taskAwaiter5 = obddataReader.InitializeCustomInitString(CustomInitString, initMode, true).GetAwaiter();
											if (!taskAwaiter5.IsCompleted)
											{
												num3 = (num4 = 5);
												taskAwaiter2 = taskAwaiter5;
												this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<bool>, OBDDataReader.<Initialize>d__168>(ref taskAwaiter5, ref this);
												return;
											}
										}
										else if (i == Attempts - 1 && !SharedSettings.Current.ForceOnlyOneProtocol)
										{
											taskAwaiter5 = obddataReader.InitializeDefaultInitString(0, initMode, false, true).GetAwaiter();
											if (!taskAwaiter5.IsCompleted)
											{
												num3 = (num4 = 6);
												taskAwaiter2 = taskAwaiter5;
												this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<bool>, OBDDataReader.<Initialize>d__168>(ref taskAwaiter5, ref this);
												return;
											}
											goto IL_060D;
										}
										else
										{
											taskAwaiter5 = obddataReader.InitializeDefaultInitString(DefaultProtocol, initMode, false, true).GetAwaiter();
											if (!taskAwaiter5.IsCompleted)
											{
												num3 = (num4 = 7);
												taskAwaiter2 = taskAwaiter5;
												this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<bool>, OBDDataReader.<Initialize>d__168>(ref taskAwaiter5, ref this);
												return;
											}
											goto IL_069B;
										}
										break;
									}
									bool flag = taskAwaiter5.GetResult();
									initSuccess = flag;
									goto IL_06AC;
									IL_060D:
									flag = taskAwaiter5.GetResult();
									initSuccess = flag;
									if (initSuccess)
									{
										SharedSettings.Current.ProtocolNumber = 0;
										goto IL_06AC;
									}
									goto IL_06AC;
									IL_069B:
									flag = taskAwaiter5.GetResult();
									initSuccess = flag;
									IL_06AC:
									if (!initSuccess)
									{
										goto IL_07E5;
									}
									if (obddataReader.DisconnectRequested)
									{
										obddataReader.CurrentStatus = OBDDataReaderStatus.Disconnected;
										flag2 = false;
										goto IL_0B83;
									}
									old_progress = progressString;
									progressString = Translate.GetString("ios_RequestingPIDs");
									progress = progress;
									if (progress != null)
									{
										progress.Report(progressString);
									}
									if (initMode != OBDDataReader.InitModes.Default)
									{
										goto IL_07A1;
									}
									taskAwaiter5 = obddataReader.CheckSupportedPIDsV2().GetAwaiter();
									if (!taskAwaiter5.IsCompleted)
									{
										num3 = (num4 = 8);
										taskAwaiter2 = taskAwaiter5;
										this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<bool>, OBDDataReader.<Initialize>d__168>(ref taskAwaiter5, ref this);
										return;
									}
									IL_0764:
									if (!taskAwaiter5.GetResult())
									{
										progressString = old_progress;
										IProgress<string> progress2 = progress;
										if (progress2 != null)
										{
											progress2.Report(progressString);
										}
										throw new Exception("Error requesting supported parameters");
									}
									LiveDataPIDModel.UpdatePIDCollection(obddataReader);
									IL_07A1:
									if (obddataReader.DisconnectRequested)
									{
										obddataReader.CurrentStatus = OBDDataReaderStatus.Disconnected;
										flag2 = false;
										goto IL_0B83;
									}
									obddataReader.Start("From Initialize_initSuccess");
									obddataReader.CurrentStatus = OBDDataReaderStatus.ConnectedToECU;
									goto IL_08B3;
								}
								catch (Exception ex)
								{
									obj = ex;
									num2 = 1;
								}
								IL_07E5:
								num5 = num2;
								if (num5 != 1)
								{
									goto IL_0882;
								}
								Exception ex2 = (Exception)obj;
								if (obddataReader.DisconnectRequested)
								{
									obddataReader.CurrentStatus = OBDDataReaderStatus.Disconnected;
									flag2 = false;
									goto IL_0B83;
								}
								taskAwaiter5 = obddataReader.Connect(true).GetAwaiter();
								if (!taskAwaiter5.IsCompleted)
								{
									num3 = (num4 = 9);
									taskAwaiter2 = taskAwaiter5;
									this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<bool>, OBDDataReader.<Initialize>d__168>(ref taskAwaiter5, ref this);
									return;
								}
								goto IL_0872;
							}
							case 9:
								taskAwaiter5 = taskAwaiter2;
								taskAwaiter2 = default(TaskAwaiter<bool>);
								num3 = (num4 = -1);
								goto IL_0872;
							case 10:
							{
								TaskAwaiter<int> taskAwaiter7;
								taskAwaiter6 = taskAwaiter7;
								taskAwaiter7 = default(TaskAwaiter<int>);
								num3 = (num4 = -1);
								goto IL_0955;
							}
							case 11:
								taskAwaiter5 = taskAwaiter2;
								taskAwaiter2 = default(TaskAwaiter<bool>);
								num3 = (num4 = -1);
								goto IL_09EC;
							default:
								initSuccess = false;
								if (obddataReader.DisconnectRequested)
								{
									obddataReader.CurrentStatus = OBDDataReaderStatus.Disconnected;
									flag2 = false;
									goto IL_0B83;
								}
								obddataReader.CurrentStatus = OBDDataReaderStatus.ConnectingToECU;
								i = 0;
								goto IL_08A2;
							}
							IL_0356:
							taskAwaiter3.GetResult();
							taskAwaiter3 = Task.Delay(5000).GetAwaiter();
							if (!taskAwaiter3.IsCompleted)
							{
								num3 = (num4 = 2);
								TaskAwaiter taskAwaiter4 = taskAwaiter3;
								this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, OBDDataReader.<Initialize>d__168>(ref taskAwaiter3, ref this);
								return;
							}
							IL_03B5:
							taskAwaiter3.GetResult();
							obddataReader.CurrentStatus = OBDDataReaderStatus.ConnectedToELM;
							flag2 = false;
							goto IL_0B83;
							IL_0429:
							taskAwaiter3.GetResult();
							obddataReader.CurrentCarData.ResetAvailablePids();
							IL_0460:
							taskAwaiter3 = Task.Delay(300).GetAwaiter();
							if (!taskAwaiter3.IsCompleted)
							{
								num3 = (num4 = 4);
								TaskAwaiter taskAwaiter4 = taskAwaiter3;
								this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, OBDDataReader.<Initialize>d__168>(ref taskAwaiter3, ref this);
								return;
							}
							IL_04B8:
							taskAwaiter3.GetResult();
							obddataReader.BadELM = false;
							if (obddataReader.DisconnectRequested)
							{
								obddataReader.CurrentStatus = OBDDataReaderStatus.Disconnected;
								flag2 = false;
								goto IL_0B83;
							}
							if (obddataReader.CurrentStatus != OBDDataReaderStatus.Disconnected)
							{
								num2 = 0;
								goto IL_04F0;
							}
							goto IL_08B3;
							IL_0872:
							if (!taskAwaiter5.GetResult())
							{
								initSuccess = false;
							}
							IL_0882:
							obj = null;
							progressString = null;
							num5 = i;
							i = num5 + 1;
							IL_08A2:
							if (i < Attempts)
							{
								if (obddataReader.DisconnectRequested)
								{
									obddataReader.CurrentStatus = OBDDataReaderStatus.Disconnected;
									flag2 = false;
									goto IL_0B83;
								}
								progressString = "";
								try
								{
									if (UseCustom)
									{
										progressString = string.Concat(new string[]
										{
											Translate.GetString("ios_ECUinitProgressStage1_Advanced"),
											"\n",
											Translate.GetString("Settings_Control_tbChooseProfile.Text"),
											" ",
											SharedSettings.Current.BrandAndProfile
										});
										progressString = string.Format(progressString, (i + 1).ToString(), Attempts.ToString());
									}
									else
									{
										progressString = string.Concat(new string[]
										{
											Translate.GetString("ios_ECUinitProgressStage1"),
											"\n",
											Translate.GetString("Settings_Control_tbChooseProfile.Text"),
											" ",
											SharedSettings.Current.BrandAndProfile
										});
										progressString = string.Format(progressString, StaticLists.Protocols[DefaultProtocol], (i + 1).ToString(), Attempts.ToString());
									}
									IProgress<string> progress3 = progress;
									if (progress3 != null)
									{
										progress3.Report(progressString);
									}
								}
								catch (Exception)
								{
								}
								if (initMode != OBDDataReader.InitModes.Default)
								{
									obddataReader.ELM327_LastSentHeader = "";
									obddataReader.ELM327_LastSentBeforeCommands = new string[0];
									obddataReader.ELM327_PendingAfterCommands = new string[0];
									goto IL_0460;
								}
								if (Device.RuntimePlatform == "Android")
								{
									if (!DependencyService.Get<ISignatureChecker>(0).GetSignaturesStrings().Any((string x) => (PlatformHelper.AppMarket == Markets.RUS && x.Equals("Lrc7gArwidVPdp9SPHGRH0v9GbM=")) || x.Equals("Y5kpo3aphxDnrKsp2if0BMjSUvo=") || x.Equals("VWq6xreY9eTH4u5UC5tJ0t2GYuI=") || x.Equals("Y5kpo3aphxDnrKsp2if0BMjSUvo=") || x.Equals("VWq6xreY9eTH4u5UC5tJ0t2GYuI=") || x.Equals("W8RPrUKpDE0NIvMkG9bDYSf4D1E=") || x.Equals("v9gdPRvdsuLO6CXYxBIIJ5wEhjQ=")))
									{
										taskAwaiter3 = obddataReader.DebugWrite("\n\rATSF\n\r>").GetAwaiter();
										if (!taskAwaiter3.IsCompleted)
										{
											num3 = (num4 = 1);
											TaskAwaiter taskAwaiter4 = taskAwaiter3;
											this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, OBDDataReader.<Initialize>d__168>(ref taskAwaiter3, ref this);
											return;
										}
										goto IL_0356;
									}
								}
								taskAwaiter3 = MainThread.InvokeOnMainThreadAsync(new Action(obddataReader.ResetVars)).GetAwaiter();
								if (!taskAwaiter3.IsCompleted)
								{
									num3 = (num4 = 3);
									TaskAwaiter taskAwaiter4 = taskAwaiter3;
									this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, OBDDataReader.<Initialize>d__168>(ref taskAwaiter3, ref this);
									return;
								}
								goto IL_0429;
							}
							IL_08B3:
							if (initSuccess || UseCustom || DefaultProtocol != 0 || initMode != OBDDataReader.InitModes.Default)
							{
								goto IL_0A11;
							}
							if (obddataReader.DisconnectRequested)
							{
								obddataReader.CurrentStatus = OBDDataReaderStatus.Disconnected;
								flag2 = false;
								goto IL_0B83;
							}
							taskAwaiter6 = obddataReader.SearchForProtocol(progress).GetAwaiter();
							if (!taskAwaiter6.IsCompleted)
							{
								num3 = (num4 = 10);
								TaskAwaiter<int> taskAwaiter7 = taskAwaiter6;
								this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<int>, OBDDataReader.<Initialize>d__168>(ref taskAwaiter6, ref this);
								return;
							}
							IL_0955:
							int result = taskAwaiter6.GetResult();
							if (result <= 0)
							{
								goto IL_0A11;
							}
							initSuccess = true;
							SharedSettings.Current.ProtocolNumber = result;
							IProgress<string> progress4 = progress;
							if (progress4 != null)
							{
								progress4.Report(Translate.GetString("ios_RequestingPIDs"));
							}
							taskAwaiter5 = obddataReader.CheckSupportedPIDsV2().GetAwaiter();
							if (!taskAwaiter5.IsCompleted)
							{
								num3 = (num4 = 11);
								taskAwaiter2 = taskAwaiter5;
								this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<bool>, OBDDataReader.<Initialize>d__168>(ref taskAwaiter5, ref this);
								return;
							}
							IL_09EC:
							if (!taskAwaiter5.GetResult())
							{
								throw new Exception("Error requesting supported parameters");
							}
							LiveDataPIDModel.UpdatePIDCollection(obddataReader);
							obddataReader.Start("From Initialize_initSuccess_after_SearchingForProtocol");
							IL_0A11:
							if (!initSuccess)
							{
								if (obddataReader.DisconnectRequested)
								{
									obddataReader.CurrentStatus = OBDDataReaderStatus.Disconnected;
									flag2 = false;
									goto IL_0B83;
								}
								obddataReader.CurrentStatus = OBDDataReaderStatus.ConnectedToELM;
							}
							OBDRequestQueueOptimizer.RefreshOBD2Dictionary();
							if (obddataReader.DisconnectRequested)
							{
								obddataReader.CurrentStatus = OBDDataReaderStatus.Disconnected;
								flag2 = false;
								goto IL_0B83;
							}
							flag2 = initSuccess;
							goto IL_0B83;
						}
						catch (Exception obj2)
						{
							num6 = 1;
						}
						if (num6 != 1)
						{
							CustomInitString = null;
							goto IL_0B83;
						}
						object obj2;
						Exception ex3 = (Exception)obj2;
						taskAwaiter3 = obddataReader.DebugWrite(Encoding.UTF8.GetBytes(ex3.ToString())).GetAwaiter();
						if (!taskAwaiter3.IsCompleted)
						{
							num3 = (num4 = 12);
							TaskAwaiter taskAwaiter4 = taskAwaiter3;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, OBDDataReader.<Initialize>d__168>(ref taskAwaiter3, ref this);
							return;
						}
						goto IL_0ADF;
					}
					case 12:
					{
						TaskAwaiter taskAwaiter4;
						taskAwaiter3 = taskAwaiter4;
						taskAwaiter4 = default(TaskAwaiter);
						num3 = (num4 = -1);
						goto IL_0ADF;
					}
					case 13:
					{
						TaskAwaiter taskAwaiter4;
						taskAwaiter3 = taskAwaiter4;
						taskAwaiter4 = default(TaskAwaiter);
						num3 = (num4 = -1);
						goto IL_0B3D;
					}
					default:
						taskAwaiter3 = obddataReader.DebugWrite("\nInitialize(initMode=" + initMode.ToString() + ")\n").GetAwaiter();
						if (!taskAwaiter3.IsCompleted)
						{
							num3 = (num4 = 0);
							TaskAwaiter taskAwaiter4 = taskAwaiter3;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, OBDDataReader.<Initialize>d__168>(ref taskAwaiter3, ref this);
							return;
						}
						break;
					}
					taskAwaiter3.GetResult();
					DefaultProtocol = SharedSettings.Current.ProtocolNumber;
					UseCustom = !SharedSettings.Current.UseDefaultInit;
					CustomInitString = SharedSettings.Current.CustomInitString;
					num6 = 0;
					goto IL_00FD;
					IL_0ADF:
					taskAwaiter3.GetResult();
					taskAwaiter3 = obddataReader.Disconnect("Initialize result:false").GetAwaiter();
					if (!taskAwaiter3.IsCompleted)
					{
						num3 = (num4 = 13);
						TaskAwaiter taskAwaiter4 = taskAwaiter3;
						this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, OBDDataReader.<Initialize>d__168>(ref taskAwaiter3, ref this);
						return;
					}
					IL_0B3D:
					taskAwaiter3.GetResult();
					obddataReader.InvokeOnMainThread(delegate
					{
						base.ECUHeaders.Clear();
					});
					flag2 = false;
				}
				catch (Exception ex4)
				{
					num4 = -2;
					CustomInitString = null;
					this.<>t__builder.SetException(ex4);
					return;
				}
				IL_0B83:
				num4 = -2;
				CustomInitString = null;
				this.<>t__builder.SetResult(flag2);
			}

			// Token: 0x06002613 RID: 9747 RVA: 0x001CFAB4 File Offset: 0x001CDCB4
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x040013BF RID: 5055
			public int <>1__state;

			// Token: 0x040013C0 RID: 5056
			public AsyncTaskMethodBuilder<bool> <>t__builder;

			// Token: 0x040013C1 RID: 5057
			public OBDDataReader <>4__this;

			// Token: 0x040013C2 RID: 5058
			public OBDDataReader.InitModes initMode;

			// Token: 0x040013C3 RID: 5059
			public int Attempts;

			// Token: 0x040013C4 RID: 5060
			public IProgress<string> progress;

			// Token: 0x040013C5 RID: 5061
			private int <DefaultProtocol>5__2;

			// Token: 0x040013C6 RID: 5062
			private bool <UseCustom>5__3;

			// Token: 0x040013C7 RID: 5063
			private string <CustomInitString>5__4;

			// Token: 0x040013C8 RID: 5064
			private TaskAwaiter <>u__1;

			// Token: 0x040013C9 RID: 5065
			private bool <initSuccess>5__5;

			// Token: 0x040013CA RID: 5066
			private int <i>5__6;

			// Token: 0x040013CB RID: 5067
			private string <progressString>5__7;

			// Token: 0x040013CC RID: 5068
			private object <>7__wrap7;

			// Token: 0x040013CD RID: 5069
			private int <>7__wrap8;

			// Token: 0x040013CE RID: 5070
			private TaskAwaiter<bool> <>u__2;

			// Token: 0x040013CF RID: 5071
			private string <old_progress>5__10;

			// Token: 0x040013D0 RID: 5072
			private TaskAwaiter<int> <>u__3;
		}

		// Token: 0x02000371 RID: 881
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <InitializeCustomInitString>d__170 : IAsyncStateMachine
		{
			// Token: 0x06002614 RID: 9748 RVA: 0x001CFAC4 File Offset: 0x001CDCC4
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				OBDDataReader obddataReader = this;
				bool flag3;
				try
				{
					TaskAwaiter taskAwaiter;
					TaskAwaiter taskAwaiter2;
					if (num != 0)
					{
						if (num - 1 <= 20)
						{
							goto IL_009D;
						}
						taskAwaiter = obddataReader.DebugWrite("\r\nInitializeCustomInitString(initMode=" + initMode.ToString() + ")\r\n").GetAwaiter();
						if (!taskAwaiter.IsCompleted)
						{
							num = (num2 = 0);
							taskAwaiter2 = taskAwaiter;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, OBDDataReader.<InitializeCustomInitString>d__170>(ref taskAwaiter, ref this);
							return;
						}
					}
					else
					{
						taskAwaiter = taskAwaiter2;
						taskAwaiter2 = default(TaskAwaiter);
						num = (num2 = -1);
					}
					taskAwaiter.GetResult();
					initSuccess = false;
					IL_009D:
					try
					{
						TaskAwaiter<string> taskAwaiter3;
						ValueTaskAwaiter<bool> valueTaskAwaiter;
						bool flag;
						ValueTaskAwaiter valueTaskAwaiter3;
						switch (num)
						{
						case 1:
						{
							TaskAwaiter<string> taskAwaiter4;
							taskAwaiter3 = taskAwaiter4;
							taskAwaiter4 = default(TaskAwaiter<string>);
							num = (num2 = -1);
							break;
						}
						case 2:
							taskAwaiter = taskAwaiter2;
							taskAwaiter2 = default(TaskAwaiter);
							num = (num2 = -1);
							goto IL_02B3;
						case 3:
							taskAwaiter = taskAwaiter2;
							taskAwaiter2 = default(TaskAwaiter);
							num = (num2 = -1);
							goto IL_0312;
						case 4:
						{
							TaskAwaiter<string> taskAwaiter4;
							taskAwaiter3 = taskAwaiter4;
							taskAwaiter4 = default(TaskAwaiter<string>);
							num = (num2 = -1);
							goto IL_0377;
						}
						case 5:
							taskAwaiter = taskAwaiter2;
							taskAwaiter2 = default(TaskAwaiter);
							num = (num2 = -1);
							goto IL_03D8;
						case 6:
							taskAwaiter = taskAwaiter2;
							taskAwaiter2 = default(TaskAwaiter);
							num = (num2 = -1);
							goto IL_0437;
						case 7:
						{
							TaskAwaiter<string> taskAwaiter4;
							taskAwaiter3 = taskAwaiter4;
							taskAwaiter4 = default(TaskAwaiter<string>);
							num = (num2 = -1);
							goto IL_049C;
						}
						case 8:
						{
							ValueTaskAwaiter<bool> valueTaskAwaiter2;
							valueTaskAwaiter = valueTaskAwaiter2;
							valueTaskAwaiter2 = default(ValueTaskAwaiter<bool>);
							num = (num2 = -1);
							goto IL_04FF;
						}
						case 9:
						{
							ValueTaskAwaiter<bool> valueTaskAwaiter2;
							valueTaskAwaiter = valueTaskAwaiter2;
							valueTaskAwaiter2 = default(ValueTaskAwaiter<bool>);
							num = (num2 = -1);
							goto IL_056C;
						}
						case 10:
							taskAwaiter = taskAwaiter2;
							taskAwaiter2 = default(TaskAwaiter);
							num = (num2 = -1);
							goto IL_062B;
						case 11:
							taskAwaiter = taskAwaiter2;
							taskAwaiter2 = default(TaskAwaiter);
							num = (num2 = -1);
							goto IL_06BA;
						case 12:
							taskAwaiter = taskAwaiter2;
							taskAwaiter2 = default(TaskAwaiter);
							num = (num2 = -1);
							goto IL_0721;
						case 13:
						{
							TaskAwaiter<string> taskAwaiter4;
							taskAwaiter3 = taskAwaiter4;
							taskAwaiter4 = default(TaskAwaiter<string>);
							num = (num2 = -1);
							goto IL_07D8;
						}
						case 14:
						{
							TaskAwaiter<string> taskAwaiter4;
							taskAwaiter3 = taskAwaiter4;
							taskAwaiter4 = default(TaskAwaiter<string>);
							num = (num2 = -1);
							goto IL_0842;
						}
						case 15:
							taskAwaiter = taskAwaiter2;
							taskAwaiter2 = default(TaskAwaiter);
							num = (num2 = -1);
							goto IL_0977;
						case 16:
						{
							TaskAwaiter<string> taskAwaiter4;
							taskAwaiter3 = taskAwaiter4;
							taskAwaiter4 = default(TaskAwaiter<string>);
							num = (num2 = -1);
							goto IL_09E8;
						}
						case 17:
						case 18:
							IL_0A22:
							try
							{
								int num3;
								TaskAwaiter<int> taskAwaiter5;
								if (num != 17)
								{
									if (num == 18)
									{
										ValueTaskAwaiter<bool> valueTaskAwaiter2;
										valueTaskAwaiter = valueTaskAwaiter2;
										valueTaskAwaiter2 = default(ValueTaskAwaiter<bool>);
										num = (num2 = -1);
										goto IL_0C4F;
									}
									num3 = OBDResponseAnalyzer.GetProtocolNumberFromInitString(InitString);
									if (num3 > 0 && num3 < 10)
									{
										goto IL_0AA9;
									}
									taskAwaiter5 = obddataReader.GetCurrentProtocolNumber().GetAwaiter();
									if (!taskAwaiter5.IsCompleted)
									{
										num = (num2 = 17);
										TaskAwaiter<int> taskAwaiter6 = taskAwaiter5;
										this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<int>, OBDDataReader.<InitializeCustomInitString>d__170>(ref taskAwaiter5, ref this);
										return;
									}
								}
								else
								{
									TaskAwaiter<int> taskAwaiter6;
									taskAwaiter5 = taskAwaiter6;
									taskAwaiter6 = default(TaskAwaiter<int>);
									num = (num2 = -1);
								}
								num3 = taskAwaiter5.GetResult();
								IL_0AA9:
								if (num3 >= 1 && num3 <= 10)
								{
									obddataReader.CurrentProtocolNumber = num3;
									obddataReader.CurrentELMFormat = OBDResponseAnalyzer.GetELMFormatFromProtocolNumber(num3);
								}
								else
								{
									obddataReader.CurrentELMFormat = OBDResponseAnalyzer.AnalyzeResponse(CS$<>8__locals1.cmd.Command, s);
									switch (obddataReader.CurrentELMFormat)
									{
									case ELMFormat.KWP:
										obddataReader.CurrentProtocolNumber = 4;
										break;
									case ELMFormat.CAN11bit:
										obddataReader.CurrentProtocolNumber = 6;
										break;
									case ELMFormat.CAN29bit:
										obddataReader.CurrentProtocolNumber = 7;
										break;
									}
								}
								obddataReader.ParseECUHeaders(CS$<>8__locals1.cmd.Command, s);
								List<PID> list = (from x in obddataReader.CurrentCarData.LiveDataPIDs.Concat(CustomPIDViewModel.CurrentProfile.PidCollection).Concat(CustomPIDViewModel.CurrentCustom.PidCollection)
									where x.Command == CS$<>8__locals1.cmd.Command
									select x).ToList<PID>();
								if (SharedSettings.Current.CheckOnlyPositiveResponseMarker && s != null)
								{
									string responseMarkerFromCommand = OBDRequest.GetResponseMarkerFromCommand(CS$<>8__locals1.cmd.Command);
									if (OBDDataReader.FilterHexAndNewLineOnly(s).Contains(responseMarkerFromCommand))
									{
										DecodeResult = true;
										goto IL_0C60;
									}
									goto IL_0C60;
								}
								else
								{
									valueTaskAwaiter = obddataReader.DecodeData(s, new OBDRequest(CS$<>8__locals1.cmd.Command, false, list), null).GetAwaiter();
									if (!valueTaskAwaiter.IsCompleted)
									{
										num = (num2 = 18);
										ValueTaskAwaiter<bool> valueTaskAwaiter2 = valueTaskAwaiter;
										this.<>t__builder.AwaitUnsafeOnCompleted<ValueTaskAwaiter<bool>, OBDDataReader.<InitializeCustomInitString>d__170>(ref valueTaskAwaiter, ref this);
										return;
									}
								}
								IL_0C4F:
								flag = valueTaskAwaiter.GetResult();
								DecodeResult = flag;
								IL_0C60:
								if (DecodeResult)
								{
									if (s != null)
									{
										if (s.Count((char c) => c == ' ') >= 1)
										{
											obddataReader.ELMStatus.ELMSupportsATS0 = false;
										}
									}
									goto IL_0D61;
								}
								goto IL_0D35;
							}
							catch (Exception)
							{
								goto IL_0D35;
							}
							goto IL_0CB6;
						case 19:
						{
							ValueTaskAwaiter<bool> valueTaskAwaiter2;
							valueTaskAwaiter = valueTaskAwaiter2;
							valueTaskAwaiter2 = default(ValueTaskAwaiter<bool>);
							num = (num2 = -1);
							goto IL_0D24;
						}
						case 20:
						{
							ValueTaskAwaiter valueTaskAwaiter4;
							valueTaskAwaiter3 = valueTaskAwaiter4;
							valueTaskAwaiter4 = default(ValueTaskAwaiter);
							num = (num2 = -1);
							goto IL_0DD2;
						}
						case 21:
						{
							ValueTaskAwaiter valueTaskAwaiter4;
							valueTaskAwaiter3 = valueTaskAwaiter4;
							valueTaskAwaiter4 = default(ValueTaskAwaiter);
							num = (num2 = -1);
							goto IL_0E3C;
						}
						default:
						{
							if (InitString.IndexOf("ATE0", StringComparison.OrdinalIgnoreCase) < 0)
							{
								InitString += "\nATE0";
							}
							if (InitString.IndexOf("ATH1", StringComparison.OrdinalIgnoreCase) < 0)
							{
								InitString += "\nATH1";
							}
							if (InitString.IndexOf("ATH1", StringComparison.OrdinalIgnoreCase) < 0)
							{
								InitString += "\nATS0";
							}
							StringArray = InitString.Split(new string[] { "\n", "\r", "\\n", ";" }, StringSplitOptions.RemoveEmptyEntries);
							for (int j = 0; j < StringArray.Length; j++)
							{
								StringArray[j] = StringArray[j].Trim().Replace("\r", "").ToUpperInvariant();
							}
							if (initMode != OBDDataReader.InitModes.RestoreConnection)
							{
								goto IL_025A;
							}
							taskAwaiter3 = obddataReader.SendATZ().GetAwaiter();
							if (!taskAwaiter3.IsCompleted)
							{
								num = (num2 = 1);
								TaskAwaiter<string> taskAwaiter4 = taskAwaiter3;
								this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<string>, OBDDataReader.<InitializeCustomInitString>d__170>(ref taskAwaiter3, ref this);
								return;
							}
							break;
						}
						}
						taskAwaiter3.GetResult();
						IL_025A:
						taskAwaiter = obddataReader.SendString("ATE0").GetAwaiter();
						if (!taskAwaiter.IsCompleted)
						{
							num = (num2 = 2);
							taskAwaiter2 = taskAwaiter;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, OBDDataReader.<InitializeCustomInitString>d__170>(ref taskAwaiter, ref this);
							return;
						}
						IL_02B3:
						taskAwaiter.GetResult();
						taskAwaiter = Task.Delay(300).GetAwaiter();
						if (!taskAwaiter.IsCompleted)
						{
							num = (num2 = 3);
							taskAwaiter2 = taskAwaiter;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, OBDDataReader.<InitializeCustomInitString>d__170>(ref taskAwaiter, ref this);
							return;
						}
						IL_0312:
						taskAwaiter.GetResult();
						taskAwaiter3 = obddataReader.ReadData(2500, null, -1).GetAwaiter();
						if (!taskAwaiter3.IsCompleted)
						{
							num = (num2 = 4);
							TaskAwaiter<string> taskAwaiter4 = taskAwaiter3;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<string>, OBDDataReader.<InitializeCustomInitString>d__170>(ref taskAwaiter3, ref this);
							return;
						}
						IL_0377:
						taskAwaiter3.GetResult();
						taskAwaiter = obddataReader.SendString("ATE0").GetAwaiter();
						if (!taskAwaiter.IsCompleted)
						{
							num = (num2 = 5);
							taskAwaiter2 = taskAwaiter;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, OBDDataReader.<InitializeCustomInitString>d__170>(ref taskAwaiter, ref this);
							return;
						}
						IL_03D8:
						taskAwaiter.GetResult();
						taskAwaiter = Task.Delay(300).GetAwaiter();
						if (!taskAwaiter.IsCompleted)
						{
							num = (num2 = 6);
							taskAwaiter2 = taskAwaiter;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, OBDDataReader.<InitializeCustomInitString>d__170>(ref taskAwaiter, ref this);
							return;
						}
						IL_0437:
						taskAwaiter.GetResult();
						taskAwaiter3 = obddataReader.ReadData(2500, null, -1).GetAwaiter();
						if (!taskAwaiter3.IsCompleted)
						{
							num = (num2 = 7);
							TaskAwaiter<string> taskAwaiter4 = taskAwaiter3;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<string>, OBDDataReader.<InitializeCustomInitString>d__170>(ref taskAwaiter3, ref this);
							return;
						}
						IL_049C:
						taskAwaiter3.GetResult();
						valueTaskAwaiter = obddataReader.CheckIsSTCommandsSupported().GetAwaiter();
						if (!valueTaskAwaiter.IsCompleted)
						{
							num = (num2 = 8);
							ValueTaskAwaiter<bool> valueTaskAwaiter2 = valueTaskAwaiter;
							this.<>t__builder.AwaitUnsafeOnCompleted<ValueTaskAwaiter<bool>, OBDDataReader.<InitializeCustomInitString>d__170>(ref valueTaskAwaiter, ref this);
							return;
						}
						IL_04FF:
						flag = valueTaskAwaiter.GetResult();
						obddataReader.STCommandsStupported = flag;
						valueTaskAwaiter = obddataReader.CheckIsVTCommandsSupported().GetAwaiter();
						if (!valueTaskAwaiter.IsCompleted)
						{
							num = (num2 = 9);
							ValueTaskAwaiter<bool> valueTaskAwaiter2 = valueTaskAwaiter;
							this.<>t__builder.AwaitUnsafeOnCompleted<ValueTaskAwaiter<bool>, OBDDataReader.<InitializeCustomInitString>d__170>(ref valueTaskAwaiter, ref this);
							return;
						}
						IL_056C:
						flag = valueTaskAwaiter.GetResult();
						obddataReader.VTCommandsStupported = flag;
						array = StringArray;
						k = 0;
						goto IL_086E;
						IL_062B:
						taskAwaiter.GetResult();
						goto IL_0860;
						IL_06BA:
						taskAwaiter.GetResult();
						goto IL_0860;
						IL_0721:
						taskAwaiter.GetResult();
						bool flag2 = !s.StartsWith("AT") || (s.Contains("ATFI") || s.Contains("ATSI"));
						if (flag2)
						{
							taskAwaiter3 = obddataReader.ReadData(15000, null, -1).GetAwaiter();
							if (!taskAwaiter3.IsCompleted)
							{
								num = (num2 = 13);
								TaskAwaiter<string> taskAwaiter4 = taskAwaiter3;
								this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<string>, OBDDataReader.<InitializeCustomInitString>d__170>(ref taskAwaiter3, ref this);
								return;
							}
						}
						else
						{
							taskAwaiter3 = obddataReader.ReadData(2500, null, -1).GetAwaiter();
							if (!taskAwaiter3.IsCompleted)
							{
								num = (num2 = 14);
								TaskAwaiter<string> taskAwaiter4 = taskAwaiter3;
								this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<string>, OBDDataReader.<InitializeCustomInitString>d__170>(ref taskAwaiter3, ref this);
								return;
							}
							goto IL_0842;
						}
						IL_07D8:
						string text = taskAwaiter3.GetResult();
						goto IL_084B;
						IL_0842:
						text = taskAwaiter3.GetResult();
						IL_084B:
						obddataReader.CheckReplyForBadELM(s, text);
						s = null;
						IL_0860:
						k++;
						IL_086E:
						if (k >= array.Length)
						{
							array = null;
							if (!Decode)
							{
								flag3 = true;
								goto IL_0E76;
							}
							DecodeResult = false;
							k = 0;
							goto IL_0D55;
						}
						else
						{
							s = array[k];
							if (s.StartsWith("DELAY"))
							{
								string text2 = s.Substring(5);
								int num4 = 0;
								if (!int.TryParse(text2, out num4))
								{
									goto IL_0860;
								}
								taskAwaiter = Task.Delay(num4).GetAwaiter();
								if (!taskAwaiter.IsCompleted)
								{
									num = (num2 = 10);
									taskAwaiter2 = taskAwaiter;
									this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, OBDDataReader.<InitializeCustomInitString>d__170>(ref taskAwaiter, ref this);
									return;
								}
								goto IL_062B;
							}
							else if (s.StartsWith("WAIT"))
							{
								string text3 = s.Substring(4);
								int num5 = 0;
								if (!int.TryParse(text3, out num5))
								{
									goto IL_0860;
								}
								taskAwaiter = Task.Delay(num5).GetAwaiter();
								if (!taskAwaiter.IsCompleted)
								{
									num = (num2 = 11);
									taskAwaiter2 = taskAwaiter;
									this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, OBDDataReader.<InitializeCustomInitString>d__170>(ref taskAwaiter, ref this);
									return;
								}
								goto IL_06BA;
							}
							else
							{
								taskAwaiter = obddataReader.SendString(s).GetAwaiter();
								if (!taskAwaiter.IsCompleted)
								{
									num = (num2 = 12);
									taskAwaiter2 = taskAwaiter;
									this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, OBDDataReader.<InitializeCustomInitString>d__170>(ref taskAwaiter, ref this);
									return;
								}
								goto IL_0721;
							}
						}
						IL_0977:
						taskAwaiter.GetResult();
						taskAwaiter3 = obddataReader.ReadData(15000, delegate
						{
							OBDDataReader.<<InitializeCustomInitString>b__170_0>d <<InitializeCustomInitString>b__170_0>d;
							<<InitializeCustomInitString>b__170_0>d.<>t__builder = AsyncVoidMethodBuilder.Create();
							<<InitializeCustomInitString>b__170_0>d.<>4__this = obddataReader;
							<<InitializeCustomInitString>b__170_0>d.<>1__state = -1;
							<<InitializeCustomInitString>b__170_0>d.<>t__builder.Start<OBDDataReader.<<InitializeCustomInitString>b__170_0>d>(ref <<InitializeCustomInitString>b__170_0>d);
						}, -1).GetAwaiter();
						if (!taskAwaiter3.IsCompleted)
						{
							num = (num2 = 16);
							TaskAwaiter<string> taskAwaiter4 = taskAwaiter3;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<string>, OBDDataReader.<InitializeCustomInitString>d__170>(ref taskAwaiter3, ref this);
							return;
						}
						IL_09E8:
						string result = taskAwaiter3.GetResult();
						s = result;
						int num6 = i;
						i = num6 + 1;
						IL_0A0B:
						if (i >= 2)
						{
							if (initMode == OBDDataReader.InitModes.Default)
							{
								goto IL_0A22;
							}
						}
						else
						{
							taskAwaiter = obddataReader.SendRequest(CS$<>8__locals1.cmd).GetAwaiter();
							if (!taskAwaiter.IsCompleted)
							{
								num = (num2 = 15);
								taskAwaiter2 = taskAwaiter;
								this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, OBDDataReader.<InitializeCustomInitString>d__170>(ref taskAwaiter, ref this);
								return;
							}
							goto IL_0977;
						}
						IL_0CB6:
						valueTaskAwaiter = obddataReader.DecodeData(s, CS$<>8__locals1.cmd, null).GetAwaiter();
						if (!valueTaskAwaiter.IsCompleted)
						{
							num = (num2 = 19);
							ValueTaskAwaiter<bool> valueTaskAwaiter2 = valueTaskAwaiter;
							this.<>t__builder.AwaitUnsafeOnCompleted<ValueTaskAwaiter<bool>, OBDDataReader.<InitializeCustomInitString>d__170>(ref valueTaskAwaiter, ref this);
							return;
						}
						IL_0D24:
						flag = valueTaskAwaiter.GetResult();
						DecodeResult = flag;
						IL_0D35:
						CS$<>8__locals1 = null;
						s = null;
						num6 = k;
						k = num6 + 1;
						IL_0D55:
						if (k < 2)
						{
							CS$<>8__locals1 = new OBDDataReader.<>c__DisplayClass170_0();
							CS$<>8__locals1.cmd = null;
							if (string.IsNullOrEmpty(SharedSettings.Current.DetectECUConnectionPID))
							{
								CS$<>8__locals1.cmd = obddataReader.GetDefaultPidRequest();
							}
							else
							{
								CS$<>8__locals1.cmd = new OBDRequest(SharedSettings.Current.DetectECUConnectionPID, false);
							}
							s = "";
							i = 0;
							goto IL_0A0B;
						}
						IL_0D61:
						if (!DecodeResult)
						{
							goto IL_0E4A;
						}
						valueTaskAwaiter3 = obddataReader.SetSTNLevelCANTransmitSegmentation(SharedSettings.Current.CANRequestSegmentationSTNLevel).GetAwaiter();
						if (!valueTaskAwaiter3.IsCompleted)
						{
							num = (num2 = 20);
							ValueTaskAwaiter valueTaskAwaiter4 = valueTaskAwaiter3;
							this.<>t__builder.AwaitUnsafeOnCompleted<ValueTaskAwaiter, OBDDataReader.<InitializeCustomInitString>d__170>(ref valueTaskAwaiter3, ref this);
							return;
						}
						IL_0DD2:
						valueTaskAwaiter3.GetResult();
						valueTaskAwaiter3 = obddataReader.SetSTNLevelCANReceiveSegmentation(SharedSettings.Current.CANResponseSegmentationSTNLevel).GetAwaiter();
						if (!valueTaskAwaiter3.IsCompleted)
						{
							num = (num2 = 21);
							ValueTaskAwaiter valueTaskAwaiter4 = valueTaskAwaiter3;
							this.<>t__builder.AwaitUnsafeOnCompleted<ValueTaskAwaiter, OBDDataReader.<InitializeCustomInitString>d__170>(ref valueTaskAwaiter3, ref this);
							return;
						}
						IL_0E3C:
						valueTaskAwaiter3.GetResult();
						initSuccess = true;
						IL_0E4A:
						StringArray = null;
					}
					catch (Exception ex)
					{
						throw ex;
					}
					flag3 = initSuccess;
				}
				catch (Exception ex2)
				{
					num2 = -2;
					this.<>t__builder.SetException(ex2);
					return;
				}
				IL_0E76:
				num2 = -2;
				this.<>t__builder.SetResult(flag3);
			}

			// Token: 0x06002615 RID: 9749 RVA: 0x001D09A8 File Offset: 0x001CEBA8
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x040013D1 RID: 5073
			public int <>1__state;

			// Token: 0x040013D2 RID: 5074
			public AsyncTaskMethodBuilder<bool> <>t__builder;

			// Token: 0x040013D3 RID: 5075
			public OBDDataReader <>4__this;

			// Token: 0x040013D4 RID: 5076
			public OBDDataReader.InitModes initMode;

			// Token: 0x040013D5 RID: 5077
			public string InitString;

			// Token: 0x040013D6 RID: 5078
			public bool Decode;

			// Token: 0x040013D7 RID: 5079
			private OBDDataReader.<>c__DisplayClass170_0 <>8__1;

			// Token: 0x040013D8 RID: 5080
			private bool <initSuccess>5__2;

			// Token: 0x040013D9 RID: 5081
			private TaskAwaiter <>u__1;

			// Token: 0x040013DA RID: 5082
			private string[] <StringArray>5__3;

			// Token: 0x040013DB RID: 5083
			private bool <DecodeResult>5__4;

			// Token: 0x040013DC RID: 5084
			private TaskAwaiter<string> <>u__2;

			// Token: 0x040013DD RID: 5085
			private ValueTaskAwaiter<bool> <>u__3;

			// Token: 0x040013DE RID: 5086
			private string[] <>7__wrap4;

			// Token: 0x040013DF RID: 5087
			private int <>7__wrap5;

			// Token: 0x040013E0 RID: 5088
			private string <s>5__7;

			// Token: 0x040013E1 RID: 5089
			private int <k>5__8;

			// Token: 0x040013E2 RID: 5090
			private TaskAwaiter<int> <>u__4;

			// Token: 0x040013E3 RID: 5091
			private ValueTaskAwaiter <>u__5;
		}

		// Token: 0x02000372 RID: 882
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <InitializeDefaultInitString>d__175 : IAsyncStateMachine
		{
			// Token: 0x06002616 RID: 9750 RVA: 0x001D09B8 File Offset: 0x001CEBB8
			void IAsyncStateMachine.MoveNext()
			{
				int num4;
				int num3 = num4;
				OBDDataReader obddataReader = this;
				bool flag;
				try
				{
					TaskAwaiter taskAwaiter;
					switch (num3)
					{
					case 0:
					{
						TaskAwaiter taskAwaiter2;
						taskAwaiter = taskAwaiter2;
						taskAwaiter2 = default(TaskAwaiter);
						num3 = (num4 = -1);
						break;
					}
					case 1:
					case 2:
					case 3:
					case 4:
					case 5:
					case 6:
					case 7:
					case 8:
					case 9:
					case 10:
					case 11:
					case 12:
					case 13:
					case 14:
					case 15:
					case 16:
					case 17:
					case 18:
					case 19:
					case 20:
					case 21:
					case 22:
					case 23:
					case 24:
					case 25:
					case 26:
					case 27:
					case 28:
					case 29:
					case 30:
					case 31:
					case 32:
					case 33:
					case 34:
					case 35:
					case 36:
					case 37:
					case 38:
					case 39:
					case 40:
					{
						IL_02FC:
						int num5;
						try
						{
							TaskAwaiter<string> taskAwaiter3;
							ValueTaskAwaiter<bool> valueTaskAwaiter;
							ValueTaskAwaiter valueTaskAwaiter3;
							bool flag2;
							switch (num3)
							{
							case 1:
							{
								TaskAwaiter<string> taskAwaiter4;
								taskAwaiter3 = taskAwaiter4;
								taskAwaiter4 = default(TaskAwaiter<string>);
								num3 = (num4 = -1);
								break;
							}
							case 2:
							{
								TaskAwaiter<string> taskAwaiter4;
								taskAwaiter3 = taskAwaiter4;
								taskAwaiter4 = default(TaskAwaiter<string>);
								num3 = (num4 = -1);
								goto IL_04E5;
							}
							case 3:
							{
								TaskAwaiter<string> taskAwaiter4;
								taskAwaiter3 = taskAwaiter4;
								taskAwaiter4 = default(TaskAwaiter<string>);
								num3 = (num4 = -1);
								goto IL_0568;
							}
							case 4:
							{
								TaskAwaiter<string> taskAwaiter4;
								taskAwaiter3 = taskAwaiter4;
								taskAwaiter4 = default(TaskAwaiter<string>);
								num3 = (num4 = -1);
								goto IL_05D0;
							}
							case 5:
							{
								TaskAwaiter<string> taskAwaiter4;
								taskAwaiter3 = taskAwaiter4;
								taskAwaiter4 = default(TaskAwaiter<string>);
								num3 = (num4 = -1);
								goto IL_06A5;
							}
							case 6:
							{
								TaskAwaiter taskAwaiter2;
								taskAwaiter = taskAwaiter2;
								taskAwaiter2 = default(TaskAwaiter);
								num3 = (num4 = -1);
								goto IL_0706;
							}
							case 7:
							{
								TaskAwaiter taskAwaiter2;
								taskAwaiter = taskAwaiter2;
								taskAwaiter2 = default(TaskAwaiter);
								num3 = (num4 = -1);
								goto IL_0765;
							}
							case 8:
							{
								TaskAwaiter<string> taskAwaiter4;
								taskAwaiter3 = taskAwaiter4;
								taskAwaiter4 = default(TaskAwaiter<string>);
								num3 = (num4 = -1);
								goto IL_07CA;
							}
							case 9:
							{
								TaskAwaiter taskAwaiter2;
								taskAwaiter = taskAwaiter2;
								taskAwaiter2 = default(TaskAwaiter);
								num3 = (num4 = -1);
								goto IL_082C;
							}
							case 10:
							{
								TaskAwaiter taskAwaiter2;
								taskAwaiter = taskAwaiter2;
								taskAwaiter2 = default(TaskAwaiter);
								num3 = (num4 = -1);
								goto IL_088C;
							}
							case 11:
							{
								TaskAwaiter<string> taskAwaiter4;
								taskAwaiter3 = taskAwaiter4;
								taskAwaiter4 = default(TaskAwaiter<string>);
								num3 = (num4 = -1);
								goto IL_08F2;
							}
							case 12:
							{
								ValueTaskAwaiter<bool> valueTaskAwaiter2;
								valueTaskAwaiter = valueTaskAwaiter2;
								valueTaskAwaiter2 = default(ValueTaskAwaiter<bool>);
								num3 = (num4 = -1);
								goto IL_0956;
							}
							case 13:
							{
								ValueTaskAwaiter<bool> valueTaskAwaiter2;
								valueTaskAwaiter = valueTaskAwaiter2;
								valueTaskAwaiter2 = default(ValueTaskAwaiter<bool>);
								num3 = (num4 = -1);
								goto IL_09C3;
							}
							case 14:
							{
								TaskAwaiter<string> taskAwaiter4;
								taskAwaiter3 = taskAwaiter4;
								taskAwaiter4 = default(TaskAwaiter<string>);
								num3 = (num4 = -1);
								goto IL_0A56;
							}
							case 15:
							{
								TaskAwaiter taskAwaiter2;
								taskAwaiter = taskAwaiter2;
								taskAwaiter2 = default(TaskAwaiter);
								num3 = (num4 = -1);
								goto IL_0B07;
							}
							case 16:
							{
								TaskAwaiter<string> taskAwaiter4;
								taskAwaiter3 = taskAwaiter4;
								taskAwaiter4 = default(TaskAwaiter<string>);
								num3 = (num4 = -1);
								goto IL_0B6D;
							}
							case 17:
								IL_0BC2:
								try
								{
									if (num3 != 17)
									{
										goto IL_0C3A;
									}
									TaskAwaiter<string> taskAwaiter4;
									taskAwaiter3 = taskAwaiter4;
									taskAwaiter4 = default(TaskAwaiter<string>);
									num3 = (num4 = -1);
									IL_0C32:
									taskAwaiter3.GetResult();
									IL_0C3A:
									if (enumerator.MoveNext())
									{
										string text = enumerator.Current;
										taskAwaiter3 = obddataReader.SendInitCommand(text, -1).GetAwaiter();
										if (!taskAwaiter3.IsCompleted)
										{
											num3 = (num4 = 17);
											taskAwaiter4 = taskAwaiter3;
											this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<string>, OBDDataReader.<InitializeDefaultInitString>d__175>(ref taskAwaiter3, ref this);
											return;
										}
										goto IL_0C32;
									}
								}
								finally
								{
									if (num3 < 0)
									{
										((IDisposable)enumerator).Dispose();
									}
								}
								enumerator = default(List<string>.Enumerator);
								goto IL_0C6B;
							case 18:
							{
								TaskAwaiter<string> taskAwaiter4;
								taskAwaiter3 = taskAwaiter4;
								taskAwaiter4 = default(TaskAwaiter<string>);
								num3 = (num4 = -1);
								goto IL_0CED;
							}
							case 19:
							{
								ValueTaskAwaiter valueTaskAwaiter4;
								valueTaskAwaiter3 = valueTaskAwaiter4;
								valueTaskAwaiter4 = default(ValueTaskAwaiter);
								num3 = (num4 = -1);
								goto IL_0D83;
							}
							case 20:
							{
								ValueTaskAwaiter valueTaskAwaiter4;
								valueTaskAwaiter3 = valueTaskAwaiter4;
								valueTaskAwaiter4 = default(ValueTaskAwaiter);
								num3 = (num4 = -1);
								goto IL_0DF0;
							}
							case 21:
							{
								TaskAwaiter taskAwaiter2;
								taskAwaiter = taskAwaiter2;
								taskAwaiter2 = default(TaskAwaiter);
								num3 = (num4 = -1);
								goto IL_0EDC;
							}
							case 22:
							{
								TaskAwaiter<string> taskAwaiter4;
								taskAwaiter3 = taskAwaiter4;
								taskAwaiter4 = default(TaskAwaiter<string>);
								num3 = (num4 = -1);
								goto IL_0F42;
							}
							case 23:
							{
								TaskAwaiter taskAwaiter2;
								taskAwaiter = taskAwaiter2;
								taskAwaiter2 = default(TaskAwaiter);
								num3 = (num4 = -1);
								goto IL_1071;
							}
							case 24:
							{
								TaskAwaiter<string> taskAwaiter4;
								taskAwaiter3 = taskAwaiter4;
								taskAwaiter4 = default(TaskAwaiter<string>);
								num3 = (num4 = -1);
								goto IL_10EE;
							}
							case 25:
							{
								TaskAwaiter taskAwaiter2;
								taskAwaiter = taskAwaiter2;
								taskAwaiter2 = default(TaskAwaiter);
								num3 = (num4 = -1);
								goto IL_1172;
							}
							case 26:
							{
								TaskAwaiter taskAwaiter2;
								taskAwaiter = taskAwaiter2;
								taskAwaiter2 = default(TaskAwaiter);
								num3 = (num4 = -1);
								goto IL_11F8;
							}
							case 27:
							{
								TaskAwaiter<string> taskAwaiter4;
								taskAwaiter3 = taskAwaiter4;
								taskAwaiter4 = default(TaskAwaiter<string>);
								num3 = (num4 = -1);
								goto IL_125E;
							}
							case 28:
							{
								TaskAwaiter taskAwaiter2;
								taskAwaiter = taskAwaiter2;
								taskAwaiter2 = default(TaskAwaiter);
								num3 = (num4 = -1);
								goto IL_12D5;
							}
							case 29:
							{
								TaskAwaiter<string> taskAwaiter4;
								taskAwaiter3 = taskAwaiter4;
								taskAwaiter4 = default(TaskAwaiter<string>);
								num3 = (num4 = -1);
								goto IL_133B;
							}
							case 30:
							case 31:
							case 32:
							case 33:
							case 34:
							case 35:
							case 36:
							{
								IL_143F:
								try
								{
									TaskAwaiter<int> taskAwaiter5;
									TaskAwaiter<bool> taskAwaiter7;
									switch (num3)
									{
									case 30:
									{
										TaskAwaiter<int> taskAwaiter6;
										taskAwaiter5 = taskAwaiter6;
										taskAwaiter6 = default(TaskAwaiter<int>);
										num3 = (num4 = -1);
										break;
									}
									case 31:
									{
										TaskAwaiter taskAwaiter2;
										taskAwaiter = taskAwaiter2;
										taskAwaiter2 = default(TaskAwaiter);
										num3 = (num4 = -1);
										goto IL_154F;
									}
									case 32:
									{
										TaskAwaiter<bool> taskAwaiter8;
										taskAwaiter7 = taskAwaiter8;
										taskAwaiter8 = default(TaskAwaiter<bool>);
										num3 = (num4 = -1);
										goto IL_1711;
									}
									case 33:
									{
										TaskAwaiter taskAwaiter2;
										taskAwaiter = taskAwaiter2;
										taskAwaiter2 = default(TaskAwaiter);
										num3 = (num4 = -1);
										goto IL_1816;
									}
									case 34:
									{
										ValueTaskAwaiter<bool> valueTaskAwaiter2;
										valueTaskAwaiter = valueTaskAwaiter2;
										valueTaskAwaiter2 = default(ValueTaskAwaiter<bool>);
										num3 = (num4 = -1);
										goto IL_188C;
									}
									case 35:
									{
										TaskAwaiter taskAwaiter2;
										taskAwaiter = taskAwaiter2;
										taskAwaiter2 = default(TaskAwaiter);
										num3 = (num4 = -1);
										goto IL_1907;
									}
									case 36:
									{
										ValueTaskAwaiter<bool> valueTaskAwaiter2;
										valueTaskAwaiter = valueTaskAwaiter2;
										valueTaskAwaiter2 = default(ValueTaskAwaiter<bool>);
										num3 = (num4 = -1);
										goto IL_1AEE;
									}
									default:
										_protocol = ProtocolNumber;
										if (_protocol != 0)
										{
											goto IL_1556;
										}
										taskAwaiter5 = obddataReader.GetCurrentProtocolNumber().GetAwaiter();
										if (!taskAwaiter5.IsCompleted)
										{
											num3 = (num4 = 30);
											TaskAwaiter<int> taskAwaiter6 = taskAwaiter5;
											this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<int>, OBDDataReader.<InitializeDefaultInitString>d__175>(ref taskAwaiter5, ref this);
											return;
										}
										break;
									}
									num5 = taskAwaiter5.GetResult();
									_protocol = num5;
									taskAwaiter = obddataReader.DebugWrite(string.Format("\r\nProtocolFromATDPN={0}\r\n", _protocol)).GetAwaiter();
									if (!taskAwaiter.IsCompleted)
									{
										num3 = (num4 = 31);
										TaskAwaiter taskAwaiter2 = taskAwaiter;
										this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, OBDDataReader.<InitializeDefaultInitString>d__175>(ref taskAwaiter, ref this);
										return;
									}
									IL_154F:
									taskAwaiter.GetResult();
									IL_1556:
									if ((_protocol >= 1 && _protocol <= 11) || _protocol == 43)
									{
										obddataReader.CurrentProtocolNumber = _protocol;
										obddataReader.CurrentELMFormat = OBDResponseAnalyzer.GetELMFormatFromProtocolNumber(_protocol);
									}
									else
									{
										obddataReader.CurrentELMFormat = OBDResponseAnalyzer.AnalyzeResponse(cmd, cmd_reply);
										switch (obddataReader.CurrentELMFormat)
										{
										case ELMFormat.KWP:
											obddataReader.CurrentProtocolNumber = 4;
											break;
										case ELMFormat.CAN11bit:
											obddataReader.CurrentProtocolNumber = 6;
											break;
										case ELMFormat.CAN29bit:
											obddataReader.CurrentProtocolNumber = 7;
											break;
										}
									}
									if (obddataReader.CurrentELMFormat == ELMFormat.CAN11bit && (SharedSettings.Current.SelectedBrand == "Toyota" || SharedSettings.Current.SelectedBrand == "Lexus") && cmd == "0100" && cmd_reply != null && !cmd_reply.Contains("4100") && cmd_reply.Contains("7E8037F0111"))
									{
										SharedSettings.Current.ProfileUpdateAlias = "ad685c0700ea4f348b9078befe8d5709";
										new ProfileV2Model().GetProfileForUpdate("ad685c0700ea4f348b9078befe8d5709").Apply(SharedSettings.Current.SelectedBrand);
										obddataReader.CurrentCarData.CreateEmptyPIDS();
										taskAwaiter7 = obddataReader.InitializeCustomInitString(SharedSettings.Current.CustomInitString, CS$<>8__locals1.initMode, Decode).GetAwaiter();
										if (!taskAwaiter7.IsCompleted)
										{
											num3 = (num4 = 32);
											TaskAwaiter<bool> taskAwaiter8 = taskAwaiter7;
											this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<bool>, OBDDataReader.<InitializeDefaultInitString>d__175>(ref taskAwaiter7, ref this);
											return;
										}
									}
									else
									{
										obddataReader.ParseECUHeaders(cmd, cmd_reply);
										if (SharedSettings.Current.CheckOnlyPositiveResponseMarker)
										{
											if (cmd_reply == null)
											{
												goto IL_189D;
											}
											string responseMarkerFromCommand = OBDRequest.GetResponseMarkerFromCommand(cmd);
											if (cmd_reply.Replace(" ", "").Contains(responseMarkerFromCommand))
											{
												DecodeResult = true;
												goto IL_189D;
											}
											goto IL_189D;
										}
										else
										{
											taskAwaiter = obddataReader.DebugWrite(string.Concat(new string[]
											{
												"\r\n[Trying to decode:\r\n",
												reply,
												"\r\nELMFormat=",
												obddataReader.CurrentELMFormat.ToString(),
												"]\r\n"
											})).GetAwaiter();
											if (!taskAwaiter.IsCompleted)
											{
												num3 = (num4 = 33);
												TaskAwaiter taskAwaiter2 = taskAwaiter;
												this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, OBDDataReader.<InitializeDefaultInitString>d__175>(ref taskAwaiter, ref this);
												return;
											}
											goto IL_1816;
										}
									}
									IL_1711:
									flag = taskAwaiter7.GetResult();
									goto IL_1F34;
									IL_1816:
									taskAwaiter.GetResult();
									valueTaskAwaiter = obddataReader.DecodeData(cmd_reply, new OBDRequest(cmd, false), null).GetAwaiter();
									if (!valueTaskAwaiter.IsCompleted)
									{
										num3 = (num4 = 34);
										ValueTaskAwaiter<bool> valueTaskAwaiter2 = valueTaskAwaiter;
										this.<>t__builder.AwaitUnsafeOnCompleted<ValueTaskAwaiter<bool>, OBDDataReader.<InitializeDefaultInitString>d__175>(ref valueTaskAwaiter, ref this);
										return;
									}
									IL_188C:
									flag2 = valueTaskAwaiter.GetResult();
									DecodeResult = flag2;
									IL_189D:
									taskAwaiter = obddataReader.DebugWrite(string.Format("\r\n[DecodeResult={0}]\r\n", DecodeResult)).GetAwaiter();
									if (!taskAwaiter.IsCompleted)
									{
										num3 = (num4 = 35);
										TaskAwaiter taskAwaiter2 = taskAwaiter;
										this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, OBDDataReader.<InitializeDefaultInitString>d__175>(ref taskAwaiter, ref this);
										return;
									}
									IL_1907:
									taskAwaiter.GetResult();
									if (DecodeResult && cmd_reply != null)
									{
										if (cmd_reply.Count((char c) => c == ' ') >= 1)
										{
											obddataReader.ELMStatus.ELMSupportsATS0 = false;
										}
									}
									if (DecodeResult || obddataReader.CurrentELMFormat != ELMFormat.KWP || _protocol < 1 || _protocol > 5 || cmd_reply == null)
									{
										goto IL_1AFF;
									}
									if (cmd_reply.Count((char c) => c == '\r') < 9)
									{
										goto IL_1AFF;
									}
									string[] array2 = cmd_reply.Split(OBDDataReader.line_splitter, StringSplitOptions.RemoveEmptyEntries);
									if (array2.Count((string line) => line.Length == 2) < 9)
									{
										goto IL_1AFF;
									}
									int num6 = EnumerableExtensions.IndexOf<string>(array2, (string x) => x.Length == 2);
									StringBuilder stringBuilder = new StringBuilder(12);
									for (int k = num6; k < array2.Length; k++)
									{
										if (array2[k].Length == 2)
										{
											stringBuilder.Append(array2[k]);
										}
									}
									string text2 = stringBuilder.ToString();
									obddataReader.ParseECUHeaders(cmd, text2);
									SharedSettings.Current.KWPConcatResponseLines = true;
									valueTaskAwaiter = obddataReader.DecodeData(cmd_reply, new OBDRequest(cmd, false), null).GetAwaiter();
									if (!valueTaskAwaiter.IsCompleted)
									{
										num3 = (num4 = 36);
										ValueTaskAwaiter<bool> valueTaskAwaiter2 = valueTaskAwaiter;
										this.<>t__builder.AwaitUnsafeOnCompleted<ValueTaskAwaiter<bool>, OBDDataReader.<InitializeDefaultInitString>d__175>(ref valueTaskAwaiter, ref this);
										return;
									}
									IL_1AEE:
									flag2 = valueTaskAwaiter.GetResult();
									DecodeResult = flag2;
									IL_1AFF:;
								}
								catch (Exception ex)
								{
									obj = ex;
									num2 = 1;
								}
								num5 = num2;
								if (num5 != 1)
								{
									goto IL_1BD1;
								}
								Exception ex2 = (Exception)obj;
								taskAwaiter = obddataReader.DebugWrite(Encoding.UTF8.GetBytes(string.Format("\r\nInit decode attempt #{0} failed! Command: {1}, cmd_reply:\r\n {2} \r\nException:\r\n {3} \r\n", new object[]
								{
									i,
									cmd,
									cmd_reply,
									ex2.ToString()
								}))).GetAwaiter();
								if (!taskAwaiter.IsCompleted)
								{
									num3 = (num4 = 37);
									TaskAwaiter taskAwaiter2 = taskAwaiter;
									this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, OBDDataReader.<InitializeDefaultInitString>d__175>(ref taskAwaiter, ref this);
									return;
								}
								goto IL_1BCA;
							}
							case 37:
							{
								TaskAwaiter taskAwaiter2;
								taskAwaiter = taskAwaiter2;
								taskAwaiter2 = default(TaskAwaiter);
								num3 = (num4 = -1);
								goto IL_1BCA;
							}
							case 38:
							{
								TaskAwaiter taskAwaiter2;
								taskAwaiter = taskAwaiter2;
								taskAwaiter2 = default(TaskAwaiter);
								num3 = (num4 = -1);
								goto IL_1C99;
							}
							case 39:
							{
								TaskAwaiter<string> taskAwaiter4;
								taskAwaiter3 = taskAwaiter4;
								taskAwaiter4 = default(TaskAwaiter<string>);
								num3 = (num4 = -1);
								goto IL_1D2E;
							}
							case 40:
							{
								ValueTaskAwaiter<bool> valueTaskAwaiter2;
								valueTaskAwaiter = valueTaskAwaiter2;
								valueTaskAwaiter2 = default(ValueTaskAwaiter<bool>);
								num3 = (num4 = -1);
								goto IL_1DA8;
							}
							default:
								CS$<>8__locals2 = new OBDDataReader.<>c__DisplayClass175_1();
								if (ProtocolNumber == 11)
								{
									if (CS$<>8__locals1.initMode == OBDDataReader.InitModes.Default)
									{
										obddataReader.CurrentCarData.AddOrRemoveNissanConsultPIDsV2(true);
									}
									if (CS$<>8__locals1.initMode != OBDDataReader.InitModes.RestoreConnection)
									{
										goto IL_0443;
									}
									taskAwaiter3 = obddataReader.SendATZ().GetAwaiter();
									if (!taskAwaiter3.IsCompleted)
									{
										num3 = (num4 = 1);
										TaskAwaiter<string> taskAwaiter4 = taskAwaiter3;
										this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<string>, OBDDataReader.<InitializeDefaultInitString>d__175>(ref taskAwaiter3, ref this);
										return;
									}
								}
								else
								{
									if (CS$<>8__locals1.initMode == OBDDataReader.InitModes.Default)
									{
										bool flag3 = false;
										if (ProtocolNumber == 43 || ProtocolNumber == 46)
										{
											flag3 = true;
										}
										obddataReader.CurrentCarData.AddOrRemoveNissanConsultPIDsV2(flag3);
									}
									if (CS$<>8__locals1.initMode != OBDDataReader.InitModes.RestoreConnection)
									{
										goto IL_06AD;
									}
									taskAwaiter3 = obddataReader.SendATZ().GetAwaiter();
									if (!taskAwaiter3.IsCompleted)
									{
										num3 = (num4 = 5);
										TaskAwaiter<string> taskAwaiter4 = taskAwaiter3;
										this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<string>, OBDDataReader.<InitializeDefaultInitString>d__175>(ref taskAwaiter3, ref this);
										return;
									}
									goto IL_06A5;
								}
								break;
							}
							taskAwaiter3.GetResult();
							IL_0443:
							array = nc2_init;
							j = 0;
							goto IL_05ED;
							IL_04E5:
							string result = taskAwaiter3.GetResult();
							if (result == null || !result.Contains("BUS INIT: ERROR"))
							{
								goto IL_05D8;
							}
							taskAwaiter3 = obddataReader.SendInitCommand(cmd, 10000).GetAwaiter();
							if (!taskAwaiter3.IsCompleted)
							{
								num3 = (num4 = 3);
								TaskAwaiter<string> taskAwaiter4 = taskAwaiter3;
								this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<string>, OBDDataReader.<InitializeDefaultInitString>d__175>(ref taskAwaiter3, ref this);
								return;
							}
							IL_0568:
							taskAwaiter3.GetResult();
							goto IL_05D8;
							IL_05D0:
							taskAwaiter3.GetResult();
							IL_05D8:
							cmd = null;
							j++;
							IL_05ED:
							if (j >= array.Length)
							{
								array = null;
								goto IL_0DF7;
							}
							cmd = array[j];
							if (cmd == "2212010401")
							{
								taskAwaiter3 = obddataReader.SendInitCommand(cmd, 10000).GetAwaiter();
								if (!taskAwaiter3.IsCompleted)
								{
									num3 = (num4 = 2);
									TaskAwaiter<string> taskAwaiter4 = taskAwaiter3;
									this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<string>, OBDDataReader.<InitializeDefaultInitString>d__175>(ref taskAwaiter3, ref this);
									return;
								}
								goto IL_04E5;
							}
							else
							{
								taskAwaiter3 = obddataReader.SendInitCommand(cmd, -1).GetAwaiter();
								if (!taskAwaiter3.IsCompleted)
								{
									num3 = (num4 = 4);
									TaskAwaiter<string> taskAwaiter4 = taskAwaiter3;
									this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<string>, OBDDataReader.<InitializeDefaultInitString>d__175>(ref taskAwaiter3, ref this);
									return;
								}
								goto IL_05D0;
							}
							IL_06A5:
							taskAwaiter3.GetResult();
							IL_06AD:
							taskAwaiter = obddataReader.SendString("ATE0").GetAwaiter();
							if (!taskAwaiter.IsCompleted)
							{
								num3 = (num4 = 6);
								TaskAwaiter taskAwaiter2 = taskAwaiter;
								this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, OBDDataReader.<InitializeDefaultInitString>d__175>(ref taskAwaiter, ref this);
								return;
							}
							IL_0706:
							taskAwaiter.GetResult();
							taskAwaiter = Task.Delay(300).GetAwaiter();
							if (!taskAwaiter.IsCompleted)
							{
								num3 = (num4 = 7);
								TaskAwaiter taskAwaiter2 = taskAwaiter;
								this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, OBDDataReader.<InitializeDefaultInitString>d__175>(ref taskAwaiter, ref this);
								return;
							}
							IL_0765:
							taskAwaiter.GetResult();
							taskAwaiter3 = obddataReader.ReadData(2500, null, -1).GetAwaiter();
							if (!taskAwaiter3.IsCompleted)
							{
								num3 = (num4 = 8);
								TaskAwaiter<string> taskAwaiter4 = taskAwaiter3;
								this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<string>, OBDDataReader.<InitializeDefaultInitString>d__175>(ref taskAwaiter3, ref this);
								return;
							}
							IL_07CA:
							taskAwaiter3.GetResult();
							taskAwaiter = obddataReader.SendString("ATE0").GetAwaiter();
							if (!taskAwaiter.IsCompleted)
							{
								num3 = (num4 = 9);
								TaskAwaiter taskAwaiter2 = taskAwaiter;
								this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, OBDDataReader.<InitializeDefaultInitString>d__175>(ref taskAwaiter, ref this);
								return;
							}
							IL_082C:
							taskAwaiter.GetResult();
							taskAwaiter = Task.Delay(300).GetAwaiter();
							if (!taskAwaiter.IsCompleted)
							{
								num3 = (num4 = 10);
								TaskAwaiter taskAwaiter2 = taskAwaiter;
								this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, OBDDataReader.<InitializeDefaultInitString>d__175>(ref taskAwaiter, ref this);
								return;
							}
							IL_088C:
							taskAwaiter.GetResult();
							taskAwaiter3 = obddataReader.ReadData(2500, null, -1).GetAwaiter();
							if (!taskAwaiter3.IsCompleted)
							{
								num3 = (num4 = 11);
								TaskAwaiter<string> taskAwaiter4 = taskAwaiter3;
								this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<string>, OBDDataReader.<InitializeDefaultInitString>d__175>(ref taskAwaiter3, ref this);
								return;
							}
							IL_08F2:
							taskAwaiter3.GetResult();
							valueTaskAwaiter = obddataReader.CheckIsSTCommandsSupported().GetAwaiter();
							if (!valueTaskAwaiter.IsCompleted)
							{
								num3 = (num4 = 12);
								ValueTaskAwaiter<bool> valueTaskAwaiter2 = valueTaskAwaiter;
								this.<>t__builder.AwaitUnsafeOnCompleted<ValueTaskAwaiter<bool>, OBDDataReader.<InitializeDefaultInitString>d__175>(ref valueTaskAwaiter, ref this);
								return;
							}
							IL_0956:
							flag2 = valueTaskAwaiter.GetResult();
							obddataReader.STCommandsStupported = flag2;
							valueTaskAwaiter = obddataReader.CheckIsVTCommandsSupported().GetAwaiter();
							if (!valueTaskAwaiter.IsCompleted)
							{
								num3 = (num4 = 13);
								ValueTaskAwaiter<bool> valueTaskAwaiter2 = valueTaskAwaiter;
								this.<>t__builder.AwaitUnsafeOnCompleted<ValueTaskAwaiter<bool>, OBDDataReader.<InitializeDefaultInitString>d__175>(ref valueTaskAwaiter, ref this);
								return;
							}
							IL_09C3:
							flag2 = valueTaskAwaiter.GetResult();
							obddataReader.VTCommandsStupported = flag2;
							array = default_init;
							j = 0;
							goto IL_0A6C;
							IL_0A56:
							taskAwaiter3.GetResult();
							j++;
							IL_0A6C:
							if (j >= array.Length)
							{
								array = null;
								if (ProtocolNumber >= 11)
								{
									goto IL_0B9B;
								}
								taskAwaiter = obddataReader.SendString("ATSP" + ProtocolNumber.ToString("X1", CultureInfo.InvariantCulture)).GetAwaiter();
								if (!taskAwaiter.IsCompleted)
								{
									num3 = (num4 = 15);
									TaskAwaiter taskAwaiter2 = taskAwaiter;
									this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, OBDDataReader.<InitializeDefaultInitString>d__175>(ref taskAwaiter, ref this);
									return;
								}
							}
							else
							{
								string text3 = array[j];
								taskAwaiter3 = obddataReader.SendInitCommand(text3, -1).GetAwaiter();
								if (!taskAwaiter3.IsCompleted)
								{
									num3 = (num4 = 14);
									TaskAwaiter<string> taskAwaiter4 = taskAwaiter3;
									this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<string>, OBDDataReader.<InitializeDefaultInitString>d__175>(ref taskAwaiter3, ref this);
									return;
								}
								goto IL_0A56;
							}
							IL_0B07:
							taskAwaiter.GetResult();
							taskAwaiter3 = obddataReader.ReadData(2500, null, -1).GetAwaiter();
							if (!taskAwaiter3.IsCompleted)
							{
								num3 = (num4 = 16);
								TaskAwaiter<string> taskAwaiter4 = taskAwaiter3;
								this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<string>, OBDDataReader.<InitializeDefaultInitString>d__175>(ref taskAwaiter3, ref this);
								return;
							}
							IL_0B6D:
							string text4 = taskAwaiter3.GetResult();
							reply = text4;
							string.IsNullOrEmpty(reply);
							obddataReader.CheckReplyForBadELM("ATSP", reply);
							IL_0B9B:
							if (ProtocolNumber > 11)
							{
								List<string> additionalInit = OBDDataReader.GetAdditionalInit(ProtocolNumber);
								enumerator = additionalInit.GetEnumerator();
								goto IL_0BC2;
							}
							IL_0C6B:
							array = post_init;
							j = 0;
							goto IL_0D03;
							IL_0CED:
							taskAwaiter3.GetResult();
							j++;
							IL_0D03:
							if (j >= array.Length)
							{
								array = null;
								valueTaskAwaiter3 = obddataReader.SetSTNLevelCANReceiveSegmentation(SharedSettings.Current.CANResponseSegmentationSTNLevel).GetAwaiter();
								if (!valueTaskAwaiter3.IsCompleted)
								{
									num3 = (num4 = 19);
									ValueTaskAwaiter valueTaskAwaiter4 = valueTaskAwaiter3;
									this.<>t__builder.AwaitUnsafeOnCompleted<ValueTaskAwaiter, OBDDataReader.<InitializeDefaultInitString>d__175>(ref valueTaskAwaiter3, ref this);
									return;
								}
							}
							else
							{
								string text5 = array[j];
								taskAwaiter3 = obddataReader.SendInitCommand(text5, -1).GetAwaiter();
								if (!taskAwaiter3.IsCompleted)
								{
									num3 = (num4 = 18);
									TaskAwaiter<string> taskAwaiter4 = taskAwaiter3;
									this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<string>, OBDDataReader.<InitializeDefaultInitString>d__175>(ref taskAwaiter3, ref this);
									return;
								}
								goto IL_0CED;
							}
							IL_0D83:
							valueTaskAwaiter3.GetResult();
							valueTaskAwaiter3 = obddataReader.SetSTNLevelCANTransmitSegmentation(SharedSettings.Current.CANRequestSegmentationSTNLevel).GetAwaiter();
							if (!valueTaskAwaiter3.IsCompleted)
							{
								num3 = (num4 = 20);
								ValueTaskAwaiter valueTaskAwaiter4 = valueTaskAwaiter3;
								this.<>t__builder.AwaitUnsafeOnCompleted<ValueTaskAwaiter, OBDDataReader.<InitializeDefaultInitString>d__175>(ref valueTaskAwaiter3, ref this);
								return;
							}
							IL_0DF0:
							valueTaskAwaiter3.GetResult();
							IL_0DF7:
							if (!Decode)
							{
								flag = true;
								goto IL_1F34;
							}
							DecodeResult = false;
							CS$<>8__locals2.atst = "64";
							if (useATST96)
							{
								CS$<>8__locals2.atst = "96";
							}
							else
							{
								CS$<>8__locals2.atst = SharedSettings.Current.GetATST();
							}
							j = 0;
							goto IL_1DD9;
							IL_0EDC:
							taskAwaiter.GetResult();
							taskAwaiter3 = obddataReader.ReadData(2500, null, -1).GetAwaiter();
							if (!taskAwaiter3.IsCompleted)
							{
								num3 = (num4 = 22);
								TaskAwaiter<string> taskAwaiter4 = taskAwaiter3;
								this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<string>, OBDDataReader.<InitializeDefaultInitString>d__175>(ref taskAwaiter3, ref this);
								return;
							}
							IL_0F42:
							taskAwaiter3.GetResult();
							IL_0F4A:
							cmd = "";
							if (ProtocolNumber == 11 || ProtocolNumber == 43)
							{
								cmd = "221201";
							}
							else
							{
								cmd = SharedSettings.Current.Mode01Prefix + "00";
							}
							timeout = 10;
							if ((ProtocolNumber >= 6 && ProtocolNumber <= 9) || ProtocolNumber == 43)
							{
								timeout = 10;
							}
							else
							{
								timeout = 15;
							}
							if (CS$<>8__locals1.initMode == OBDDataReader.InitModes.Default)
							{
								cmd_reply = "";
								i = 0;
								goto IL_1BF9;
							}
							request = new OBDRequest(cmd, obddataReader.GetDefaultHeader(), "", "", false);
							taskAwaiter = obddataReader.SendRequest(request).GetAwaiter();
							if (!taskAwaiter.IsCompleted)
							{
								num3 = (num4 = 38);
								TaskAwaiter taskAwaiter2 = taskAwaiter;
								this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, OBDDataReader.<InitializeDefaultInitString>d__175>(ref taskAwaiter, ref this);
								return;
							}
							goto IL_1C99;
							IL_1071:
							taskAwaiter.GetResult();
							taskAwaiter3 = obddataReader.ReadData(timeout * 1000, delegate
							{
								int num8 = 72;
								if (int.TryParse(CS$<>8__locals3.CS$<>8__locals1.atst, NumberStyles.HexNumber, CultureInfo.InvariantCulture.NumberFormat, out num8))
								{
									if (num8 <= 50)
									{
										if (num8 == 22 || num8 == 50)
										{
											CS$<>8__locals3.CS$<>8__locals1.atst = "08";
										}
									}
									else if (num8 != 100)
									{
										if (num8 != 150)
										{
											if (num8 == 255)
											{
												CS$<>8__locals3.CS$<>8__locals1.atst = "96";
											}
										}
										else
										{
											CS$<>8__locals3.CS$<>8__locals1.atst = "32";
										}
									}
									else
									{
										CS$<>8__locals3.CS$<>8__locals1.atst = "16";
									}
									CS$<>8__locals3.atstChangedBecauseOfLengthTooBigHandler = true;
								}
							}, -1).GetAwaiter();
							if (!taskAwaiter3.IsCompleted)
							{
								num3 = (num4 = 24);
								TaskAwaiter<string> taskAwaiter4 = taskAwaiter3;
								this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<string>, OBDDataReader.<InitializeDefaultInitString>d__175>(ref taskAwaiter3, ref this);
								return;
							}
							IL_10EE:
							text4 = taskAwaiter3.GetResult();
							cmd_reply = text4;
							taskAwaiter = obddataReader.DebugWrite("\r\n[cmd_reply=] " + (cmd_reply ?? "null") + "\r\n").GetAwaiter();
							if (!taskAwaiter.IsCompleted)
							{
								num3 = (num4 = 25);
								TaskAwaiter taskAwaiter2 = taskAwaiter;
								this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, OBDDataReader.<InitializeDefaultInitString>d__175>(ref taskAwaiter, ref this);
								return;
							}
							IL_1172:
							taskAwaiter.GetResult();
							if (!CS$<>8__locals3.atstChangedBecauseOfLengthTooBigHandler)
							{
								goto IL_13F7;
							}
							taskAwaiter = obddataReader.SendString("ATST" + CS$<>8__locals3.CS$<>8__locals1.atst).GetAwaiter();
							if (!taskAwaiter.IsCompleted)
							{
								num3 = (num4 = 26);
								TaskAwaiter taskAwaiter2 = taskAwaiter;
								this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, OBDDataReader.<InitializeDefaultInitString>d__175>(ref taskAwaiter, ref this);
								return;
							}
							IL_11F8:
							taskAwaiter.GetResult();
							taskAwaiter3 = obddataReader.ReadData(2500, null, -1).GetAwaiter();
							if (!taskAwaiter3.IsCompleted)
							{
								num3 = (num4 = 27);
								TaskAwaiter<string> taskAwaiter4 = taskAwaiter3;
								this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<string>, OBDDataReader.<InitializeDefaultInitString>d__175>(ref taskAwaiter3, ref this);
								return;
							}
							IL_125E:
							taskAwaiter3.GetResult();
							taskAwaiter = obddataReader.SendString("ATST" + CS$<>8__locals3.CS$<>8__locals1.atst).GetAwaiter();
							if (!taskAwaiter.IsCompleted)
							{
								num3 = (num4 = 28);
								TaskAwaiter taskAwaiter2 = taskAwaiter;
								this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, OBDDataReader.<InitializeDefaultInitString>d__175>(ref taskAwaiter, ref this);
								return;
							}
							IL_12D5:
							taskAwaiter.GetResult();
							taskAwaiter3 = obddataReader.ReadData(2500, null, -1).GetAwaiter();
							if (!taskAwaiter3.IsCompleted)
							{
								num3 = (num4 = 29);
								TaskAwaiter<string> taskAwaiter4 = taskAwaiter3;
								this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<string>, OBDDataReader.<InitializeDefaultInitString>d__175>(ref taskAwaiter3, ref this);
								return;
							}
							IL_133B:
							taskAwaiter3.GetResult();
							text4 = CS$<>8__locals3.CS$<>8__locals1.atst;
							if (!(text4 == "08"))
							{
								if (!(text4 == "16"))
								{
									if (!(text4 == "32"))
									{
										if (!(text4 == "48"))
										{
											if (!(text4 == "64"))
											{
												if (text4 == "96")
												{
													SharedSettings.Current.ATSTIdx = 7;
												}
											}
											else
											{
												SharedSettings.Current.ATSTIdx = 6;
											}
										}
										else
										{
											SharedSettings.Current.ATSTIdx = 5;
										}
									}
									else
									{
										SharedSettings.Current.ATSTIdx = 4;
									}
								}
								else
								{
									SharedSettings.Current.ATSTIdx = 3;
								}
							}
							else
							{
								SharedSettings.Current.ATSTIdx = 2;
							}
							IL_13F7:
							if (cmd_reply != null)
							{
								cmd_reply = cmd_reply.Replace("SEARCHING", "").Replace("UNABLE TO CONNECT", "").Replace("CAN ERROR", "");
							}
							num2 = 0;
							goto IL_143F;
							IL_1BCA:
							taskAwaiter.GetResult();
							IL_1BD1:
							obj = null;
							if (DecodeResult)
							{
								goto IL_1C05;
							}
							CS$<>8__locals3 = null;
							num5 = i;
							i = num5 + 1;
							IL_1BF9:
							if (i < 4)
							{
								CS$<>8__locals3 = new OBDDataReader.<>c__DisplayClass175_2();
								CS$<>8__locals3.CS$<>8__locals1 = CS$<>8__locals2;
								CS$<>8__locals3.atstChangedBecauseOfLengthTooBigHandler = false;
								taskAwaiter = obddataReader.SendString(cmd).GetAwaiter();
								if (!taskAwaiter.IsCompleted)
								{
									num3 = (num4 = 23);
									TaskAwaiter taskAwaiter2 = taskAwaiter;
									this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, OBDDataReader.<InitializeDefaultInitString>d__175>(ref taskAwaiter, ref this);
									return;
								}
								goto IL_1071;
							}
							IL_1C05:
							if (!DecodeResult)
							{
								cmd_reply = null;
								goto IL_1DC0;
							}
							goto IL_1DE5;
							IL_1C99:
							taskAwaiter.GetResult();
							OBDDataReader obddataReader2 = obddataReader;
							int num7 = 15000;
							Action action;
							if ((action = CS$<>8__locals1.<>9__5) == null)
							{
								action = (CS$<>8__locals1.<>9__5 = delegate
								{
									OBDDataReader.<>c__DisplayClass175_0.<<InitializeDefaultInitString>b__5>d <<InitializeDefaultInitString>b__5>d;
									<<InitializeDefaultInitString>b__5>d.<>t__builder = AsyncVoidMethodBuilder.Create();
									<<InitializeDefaultInitString>b__5>d.<>4__this = CS$<>8__locals1;
									<<InitializeDefaultInitString>b__5>d.<>1__state = -1;
									<<InitializeDefaultInitString>b__5>d.<>t__builder.Start<OBDDataReader.<>c__DisplayClass175_0.<<InitializeDefaultInitString>b__5>d>(ref <<InitializeDefaultInitString>b__5>d);
								});
							}
							taskAwaiter3 = obddataReader2.ReadData(num7, action, -1).GetAwaiter();
							if (!taskAwaiter3.IsCompleted)
							{
								num3 = (num4 = 39);
								TaskAwaiter<string> taskAwaiter4 = taskAwaiter3;
								this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<string>, OBDDataReader.<InitializeDefaultInitString>d__175>(ref taskAwaiter3, ref this);
								return;
							}
							IL_1D2E:
							text4 = taskAwaiter3.GetResult();
							reply = text4;
							valueTaskAwaiter = obddataReader.DecodeData(reply, request, null).GetAwaiter();
							if (!valueTaskAwaiter.IsCompleted)
							{
								num3 = (num4 = 40);
								ValueTaskAwaiter<bool> valueTaskAwaiter2 = valueTaskAwaiter;
								this.<>t__builder.AwaitUnsafeOnCompleted<ValueTaskAwaiter<bool>, OBDDataReader.<InitializeDefaultInitString>d__175>(ref valueTaskAwaiter, ref this);
								return;
							}
							IL_1DA8:
							flag2 = valueTaskAwaiter.GetResult();
							DecodeResult = flag2;
							request = null;
							IL_1DC0:
							cmd = null;
							num5 = j;
							j = num5 + 1;
							IL_1DD9:
							if (j < 2)
							{
								if (!(CS$<>8__locals2.atst != ""))
								{
									goto IL_0F4A;
								}
								taskAwaiter = obddataReader.SendString("ATST" + CS$<>8__locals2.atst).GetAwaiter();
								if (!taskAwaiter.IsCompleted)
								{
									num3 = (num4 = 21);
									TaskAwaiter taskAwaiter2 = taskAwaiter;
									this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, OBDDataReader.<InitializeDefaultInitString>d__175>(ref taskAwaiter, ref this);
									return;
								}
								goto IL_0EDC;
							}
							IL_1DE5:
							if (DecodeResult)
							{
								initSuccess = true;
								SharedSettings.Current.LastSuccessfulProtocol = ProtocolNumber;
							}
							CS$<>8__locals2 = null;
						}
						catch (Exception ex3)
						{
							obj2 = ex3;
							num = 1;
						}
						num5 = num;
						if (num5 != 1)
						{
							obj2 = null;
							flag = initSuccess;
							goto IL_1F34;
						}
						exc = (Exception)obj2;
						taskAwaiter = obddataReader.DebugWrite(Encoding.UTF8.GetBytes(string.Concat(new string[]
						{
							"\r\nInit error!\r\nData:\r\n",
							reply,
							"\r\nException:\r\n",
							exc.ToString(),
							"\r\n"
						}))).GetAwaiter();
						if (!taskAwaiter.IsCompleted)
						{
							num3 = (num4 = 41);
							TaskAwaiter taskAwaiter2 = taskAwaiter;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, OBDDataReader.<InitializeDefaultInitString>d__175>(ref taskAwaiter, ref this);
							return;
						}
						goto IL_1EDA;
					}
					case 41:
					{
						TaskAwaiter taskAwaiter2;
						taskAwaiter = taskAwaiter2;
						taskAwaiter2 = default(TaskAwaiter);
						num3 = (num4 = -1);
						goto IL_1EDA;
					}
					default:
						CS$<>8__locals1 = new OBDDataReader.<>c__DisplayClass175_0();
						CS$<>8__locals1.<>4__this = this;
						CS$<>8__locals1.initMode = initMode;
						taskAwaiter = obddataReader.DebugWrite(string.Concat(new string[]
						{
							"\r\n\nInitializeDefaultInitString(ProtocolNumber=",
							ProtocolNumber.ToString(),
							",initMode=",
							CS$<>8__locals1.initMode.ToString(),
							",useATST96=",
							useATST96.ToString(),
							")\r\n"
						})).GetAwaiter();
						if (!taskAwaiter.IsCompleted)
						{
							num3 = (num4 = 0);
							TaskAwaiter taskAwaiter2 = taskAwaiter;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, OBDDataReader.<InitializeDefaultInitString>d__175>(ref taskAwaiter, ref this);
							return;
						}
						break;
					}
					taskAwaiter.GetResult();
					initSuccess = false;
					reply = string.Empty;
					default_init = new string[] { "ATD", "ATD0", "ATE0", "ATH1" };
					post_init = new string[]
					{
						"ATE0",
						"ATH1",
						"ATM0",
						"ATS0",
						"ATAT" + SharedSettings.Current.AdaptiveTimings.ToString(CultureInfo.InvariantCulture),
						"ATAL"
					};
					nc2_init = new string[]
					{
						"ATD",
						"ATD0",
						"ATE0",
						"ATH1",
						"ATM0",
						"ATS0",
						"ATAT" + SharedSettings.Current.AdaptiveTimings.ToString(CultureInfo.InvariantCulture),
						"ATSP5",
						"ATAL",
						"ATIB10",
						"ATSH8110FC",
						"ATST20",
						"ATSW05",
						"2212010401",
						"221201",
						"ATSW05",
						"ATWM221201"
					};
					num = 0;
					goto IL_02FC;
					IL_1EDA:
					taskAwaiter.GetResult();
					throw exc;
				}
				catch (Exception ex4)
				{
					num4 = -2;
					CS$<>8__locals1 = null;
					reply = null;
					default_init = null;
					post_init = null;
					nc2_init = null;
					this.<>t__builder.SetException(ex4);
					return;
				}
				IL_1F34:
				num4 = -2;
				CS$<>8__locals1 = null;
				reply = null;
				default_init = null;
				post_init = null;
				nc2_init = null;
				this.<>t__builder.SetResult(flag);
			}

			// Token: 0x06002617 RID: 9751 RVA: 0x001D2994 File Offset: 0x001D0B94
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x040013E4 RID: 5092
			public int <>1__state;

			// Token: 0x040013E5 RID: 5093
			public AsyncTaskMethodBuilder<bool> <>t__builder;

			// Token: 0x040013E6 RID: 5094
			public OBDDataReader <>4__this;

			// Token: 0x040013E7 RID: 5095
			public OBDDataReader.InitModes initMode;

			// Token: 0x040013E8 RID: 5096
			public int ProtocolNumber;

			// Token: 0x040013E9 RID: 5097
			public bool useATST96;

			// Token: 0x040013EA RID: 5098
			private OBDDataReader.<>c__DisplayClass175_0 <>8__1;

			// Token: 0x040013EB RID: 5099
			public bool Decode;

			// Token: 0x040013EC RID: 5100
			private OBDDataReader.<>c__DisplayClass175_1 <>8__2;

			// Token: 0x040013ED RID: 5101
			private OBDDataReader.<>c__DisplayClass175_2 <>8__3;

			// Token: 0x040013EE RID: 5102
			private bool <initSuccess>5__2;

			// Token: 0x040013EF RID: 5103
			private string <reply>5__3;

			// Token: 0x040013F0 RID: 5104
			private string[] <default_init>5__4;

			// Token: 0x040013F1 RID: 5105
			private string[] <post_init>5__5;

			// Token: 0x040013F2 RID: 5106
			private string[] <nc2_init>5__6;

			// Token: 0x040013F3 RID: 5107
			private TaskAwaiter <>u__1;

			// Token: 0x040013F4 RID: 5108
			private object <>7__wrap6;

			// Token: 0x040013F5 RID: 5109
			private int <>7__wrap7;

			// Token: 0x040013F6 RID: 5110
			private bool <DecodeResult>5__9;

			// Token: 0x040013F7 RID: 5111
			private TaskAwaiter<string> <>u__2;

			// Token: 0x040013F8 RID: 5112
			private string[] <>7__wrap9;

			// Token: 0x040013F9 RID: 5113
			private int <>7__wrap10;

			// Token: 0x040013FA RID: 5114
			private string <cmd>5__12;

			// Token: 0x040013FB RID: 5115
			private ValueTaskAwaiter<bool> <>u__3;

			// Token: 0x040013FC RID: 5116
			private List<string>.Enumerator <>7__wrap12;

			// Token: 0x040013FD RID: 5117
			private ValueTaskAwaiter <>u__4;

			// Token: 0x040013FE RID: 5118
			private int <timeout>5__14;

			// Token: 0x040013FF RID: 5119
			private string <cmd_reply>5__15;

			// Token: 0x04001400 RID: 5120
			private int <k>5__16;

			// Token: 0x04001401 RID: 5121
			private object <>7__wrap16;

			// Token: 0x04001402 RID: 5122
			private int <>7__wrap17;

			// Token: 0x04001403 RID: 5123
			private int <_protocol>5__19;

			// Token: 0x04001404 RID: 5124
			private TaskAwaiter<int> <>u__5;

			// Token: 0x04001405 RID: 5125
			private TaskAwaiter<bool> <>u__6;

			// Token: 0x04001406 RID: 5126
			private OBDRequest <request>5__20;

			// Token: 0x04001407 RID: 5127
			private Exception <exc>5__21;
		}

		// Token: 0x02000373 RID: 883
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <OnDisconnectDetected>d__184 : IAsyncStateMachine
		{
			// Token: 0x06002618 RID: 9752 RVA: 0x001D29A4 File Offset: 0x001D0BA4
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				OBDDataReader obddataReader = this;
				try
				{
					TaskAwaiter taskAwaiter3;
					TaskAwaiter<bool> taskAwaiter5;
					switch (num)
					{
					case 0:
					{
						TaskAwaiter taskAwaiter4;
						taskAwaiter3 = taskAwaiter4;
						taskAwaiter4 = default(TaskAwaiter);
						num = (num2 = -1);
						break;
					}
					case 1:
					{
						TaskAwaiter taskAwaiter4;
						taskAwaiter3 = taskAwaiter4;
						taskAwaiter4 = default(TaskAwaiter);
						num = (num2 = -1);
						goto IL_0156;
					}
					case 2:
					{
						TaskAwaiter taskAwaiter4;
						taskAwaiter3 = taskAwaiter4;
						taskAwaiter4 = default(TaskAwaiter);
						num = (num2 = -1);
						goto IL_02E6;
					}
					case 3:
					{
						TaskAwaiter taskAwaiter4;
						taskAwaiter3 = taskAwaiter4;
						taskAwaiter4 = default(TaskAwaiter);
						num = (num2 = -1);
						goto IL_0352;
					}
					case 4:
					{
						TaskAwaiter taskAwaiter4;
						taskAwaiter3 = taskAwaiter4;
						taskAwaiter4 = default(TaskAwaiter);
						num = (num2 = -1);
						goto IL_03BC;
					}
					case 5:
						IL_03E0:
						try
						{
							if (num != 5)
							{
								obddataReader.Connection.Disconect();
								taskAwaiter3 = Task.Delay(500).GetAwaiter();
								if (!taskAwaiter3.IsCompleted)
								{
									num = (num2 = 5);
									TaskAwaiter taskAwaiter4 = taskAwaiter3;
									this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, OBDDataReader.<OnDisconnectDetected>d__184>(ref taskAwaiter3, ref this);
									return;
								}
							}
							else
							{
								TaskAwaiter taskAwaiter4;
								taskAwaiter3 = taskAwaiter4;
								taskAwaiter4 = default(TaskAwaiter);
								num = (num2 = -1);
							}
							taskAwaiter3.GetResult();
						}
						catch (Exception)
						{
						}
						goto IL_0454;
					case 6:
						taskAwaiter5 = taskAwaiter2;
						taskAwaiter2 = default(TaskAwaiter<bool>);
						num = (num2 = -1);
						goto IL_04B0;
					case 7:
					{
						TaskAwaiter taskAwaiter4;
						taskAwaiter3 = taskAwaiter4;
						taskAwaiter4 = default(TaskAwaiter);
						num = (num2 = -1);
						goto IL_051F;
					}
					case 8:
						taskAwaiter5 = taskAwaiter2;
						taskAwaiter2 = default(TaskAwaiter<bool>);
						num = (num2 = -1);
						goto IL_058D;
					default:
						CS$<>8__locals1 = new OBDDataReader.<>c__DisplayClass184_0();
						DriveCycle.SaveAndReset();
						taskAwaiter3 = obddataReader.DebugWrite(string.Concat(new string[]
						{
							"\r\nOnDisconnectDetected, reason",
							reason.ToString(),
							" ",
							DateTimeNowHelper.NowSafe.ToString("dd.MM.yyyy HH:mm:ss", CultureInfo.InvariantCulture),
							"\r\n"
						})).GetAwaiter();
						if (!taskAwaiter3.IsCompleted)
						{
							num = (num2 = 0);
							TaskAwaiter taskAwaiter4 = taskAwaiter3;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, OBDDataReader.<OnDisconnectDetected>d__184>(ref taskAwaiter3, ref this);
							return;
						}
						break;
					}
					taskAwaiter3.GetResult();
					if (reason == OBDDataReader.DisconnectReason.UserRequested)
					{
						taskAwaiter3 = obddataReader.Disconnect("OnDisconnectDetected-UserRequested#1").GetAwaiter();
						if (!taskAwaiter3.IsCompleted)
						{
							num = (num2 = 1);
							TaskAwaiter taskAwaiter4 = taskAwaiter3;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, OBDDataReader.<OnDisconnectDetected>d__184>(ref taskAwaiter3, ref this);
							return;
						}
					}
					else
					{
						if (obddataReader.LastUsedConnectionType == ConnectionTypes.WiFi && obddataReader.CommandsCounter == 1L && SharedSettings.Current.AndroidWiFiMode == AndroidWiFiConnectionModes.Auto)
						{
							obddataReader.DroidWiFiV2Failed = !obddataReader.DroidWiFiV2Failed;
						}
						CS$<>8__locals1.old_queue = obddataReader.CommandQueue.ToArray();
						old_recorder = App.OBDReader.CurrentCarData.Recorder;
						if (old_recorder != null)
						{
							App.OBDReader.StatusChanged -= DataRecorderV2.StopRecording;
							old_recorder.Save();
							goto IL_01E3;
						}
						goto IL_01E3;
					}
					IL_0156:
					taskAwaiter3.GetResult();
					goto IL_0650;
					IL_01E3:
					if (obddataReader.DisconnectRequested)
					{
						goto IL_0650;
					}
					if (SharedSettings.Current.StopConnectionAttemptsAfterFailsMinutes > 0 && (obddataReader.stopwatch.Elapsed - new TimeSpan(obddataReader.lastTimeConnected)).TotalMinutes > (double)SharedSettings.Current.StopConnectionAttemptsAfterFailsMinutes)
					{
						obddataReader.Disconnect("StopConnectionAttemptsAfterFailsMinutes");
						if (Device.RuntimePlatform == "Android")
						{
							IAndroidHelperImplementation androidHelperImplementation = DependencyService.Get<IAndroidHelperImplementation>(0);
							if (androidHelperImplementation != null)
							{
								androidHelperImplementation.StopService();
							}
						}
						obddataReader.InvokeOnMainThread(delegate
						{
							OBDDataReader.<>c__DisplayClass184_0.<<OnDisconnectDetected>b__0>d <<OnDisconnectDetected>b__0>d;
							<<OnDisconnectDetected>b__0>d.<>t__builder = AsyncVoidMethodBuilder.Create();
							<<OnDisconnectDetected>b__0>d.<>4__this = CS$<>8__locals1;
							<<OnDisconnectDetected>b__0>d.<>1__state = -1;
							<<OnDisconnectDetected>b__0>d.<>t__builder.Start<OBDDataReader.<>c__DisplayClass184_0.<<OnDisconnectDetected>b__0>d>(ref <<OnDisconnectDetected>b__0>d);
						});
						goto IL_0650;
					}
					obddataReader.CurrentStatus = OBDDataReaderStatus.ConnectingToELM;
					if (SharedSettings.Current.DelayBeforeReconnect <= 0)
					{
						taskAwaiter3 = Task.Delay(1000).GetAwaiter();
						if (!taskAwaiter3.IsCompleted)
						{
							num = (num2 = 2);
							TaskAwaiter taskAwaiter4 = taskAwaiter3;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, OBDDataReader.<OnDisconnectDetected>d__184>(ref taskAwaiter3, ref this);
							return;
						}
					}
					else
					{
						taskAwaiter3 = Task.Delay(TimeSpan.FromSeconds((double)SharedSettings.Current.DelayBeforeReconnect)).GetAwaiter();
						if (!taskAwaiter3.IsCompleted)
						{
							num = (num2 = 3);
							TaskAwaiter taskAwaiter4 = taskAwaiter3;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, OBDDataReader.<OnDisconnectDetected>d__184>(ref taskAwaiter3, ref this);
							return;
						}
						goto IL_0352;
					}
					IL_02E6:
					taskAwaiter3.GetResult();
					goto IL_0359;
					IL_0352:
					taskAwaiter3.GetResult();
					IL_0359:
					if (obddataReader.DisconnectRequested)
					{
						taskAwaiter3 = obddataReader.Disconnect("OnDisconnectDetected-UserRequested#2").GetAwaiter();
						if (!taskAwaiter3.IsCompleted)
						{
							num = (num2 = 4);
							TaskAwaiter taskAwaiter4 = taskAwaiter3;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, OBDDataReader.<OnDisconnectDetected>d__184>(ref taskAwaiter3, ref this);
							return;
						}
					}
					else
					{
						if (obddataReader.Connection != null && obddataReader.Connection.Connected)
						{
							goto IL_03E0;
						}
						goto IL_0454;
					}
					IL_03BC:
					taskAwaiter3.GetResult();
					goto IL_0650;
					IL_0454:
					taskAwaiter5 = App.OBDReader.Connect(true).GetAwaiter();
					if (!taskAwaiter5.IsCompleted)
					{
						num = (num2 = 6);
						taskAwaiter2 = taskAwaiter5;
						this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<bool>, OBDDataReader.<OnDisconnectDetected>d__184>(ref taskAwaiter5, ref this);
						return;
					}
					IL_04B0:
					if (!taskAwaiter5.GetResult())
					{
						goto IL_060A;
					}
					if (obddataReader.DisconnectRequested)
					{
						taskAwaiter3 = obddataReader.Disconnect("OnDisconnectDetected-UserRequested#2").GetAwaiter();
						if (!taskAwaiter3.IsCompleted)
						{
							num = (num2 = 7);
							TaskAwaiter taskAwaiter4 = taskAwaiter3;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, OBDDataReader.<OnDisconnectDetected>d__184>(ref taskAwaiter3, ref this);
							return;
						}
					}
					else
					{
						taskAwaiter5 = App.OBDReader.Initialize(3, OBDDataReader.InitModes.RestoreConnection, new Progress<string>()).GetAwaiter();
						if (!taskAwaiter5.IsCompleted)
						{
							num = (num2 = 8);
							taskAwaiter2 = taskAwaiter5;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<bool>, OBDDataReader.<OnDisconnectDetected>d__184>(ref taskAwaiter5, ref this);
							return;
						}
						goto IL_058D;
					}
					IL_051F:
					taskAwaiter3.GetResult();
					goto IL_0650;
					IL_058D:
					if (taskAwaiter5.GetResult())
					{
						OBDRequestQueueOptimizer.RefreshOBD2Dictionary();
						obddataReader.ReplaceQueue(CS$<>8__locals1.old_queue);
						if (old_recorder != null)
						{
							App.OBDReader.StatusChanged -= DataRecorderV2.StopRecording;
							App.OBDReader.StatusChanged += DataRecorderV2.StopRecording;
							App.OBDReader.CurrentCarData.Recorder = old_recorder;
						}
						if (!obddataReader.Running)
						{
							obddataReader.Start("Start from OnDisconnectRequestedAfterReconnect #4316");
						}
						goto IL_0650;
					}
					IL_060A:
					if (App.OBDReader.CurrentStatus != OBDDataReaderStatus.ConnectedToECU || obddataReader.DisconnectRequested)
					{
						goto IL_01E3;
					}
				}
				catch (Exception ex)
				{
					num2 = -2;
					CS$<>8__locals1 = null;
					old_recorder = null;
					this.<>t__builder.SetException(ex);
					return;
				}
				IL_0650:
				num2 = -2;
				CS$<>8__locals1 = null;
				old_recorder = null;
				this.<>t__builder.SetResult();
			}

			// Token: 0x06002619 RID: 9753 RVA: 0x001D3058 File Offset: 0x001D1258
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x04001408 RID: 5128
			public int <>1__state;

			// Token: 0x04001409 RID: 5129
			public AsyncTaskMethodBuilder <>t__builder;

			// Token: 0x0400140A RID: 5130
			public OBDDataReader <>4__this;

			// Token: 0x0400140B RID: 5131
			public OBDDataReader.DisconnectReason reason;

			// Token: 0x0400140C RID: 5132
			private OBDDataReader.<>c__DisplayClass184_0 <>8__1;

			// Token: 0x0400140D RID: 5133
			private DataRecorderV2 <old_recorder>5__2;

			// Token: 0x0400140E RID: 5134
			private TaskAwaiter <>u__1;

			// Token: 0x0400140F RID: 5135
			private TaskAwaiter<bool> <>u__2;
		}

		// Token: 0x02000374 RID: 884
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <OnStatusChanged>d__104 : IAsyncStateMachine
		{
			// Token: 0x0600261A RID: 9754 RVA: 0x001D3068 File Offset: 0x001D1268
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				OBDDataReader obddataReader = this;
				try
				{
					TaskAwaiter taskAwaiter;
					if (num != 0)
					{
						CS$<>8__locals1 = new OBDDataReader.<>c__DisplayClass104_0();
						CS$<>8__locals1.newStatus = newStatus;
						if (CS$<>8__locals1.newStatus == OBDDataReaderStatus.ConnectedToECU)
						{
							SharedSettings.Current.FlushLog = false;
						}
						else
						{
							SharedSettings.Current.FlushLog = true;
						}
						CS$<>8__locals1.status = obddataReader.CurrentStatus;
						if (CS$<>8__locals1.status == OBDDataReaderStatus.Disconnected)
						{
							try
							{
								obddataReader.DebugWrite("\r\n===== DISCONNECTED AT: " + DateTimeNowHelper.NowSafe.ToString("dd.MM.yyyy HH:mm:ss"));
							}
							catch (Exception)
							{
							}
							obddataReader.InvokeOnMainThread(delegate
							{
								IScreenKeeper screenKeeper = DependencyService.Get<IScreenKeeper>(0);
								if (screenKeeper == null)
								{
									return;
								}
								screenKeeper.LetScreenOff();
							});
						}
						else
						{
							obddataReader.InvokeOnMainThread(delegate
							{
								try
								{
									IScreenKeeper screenKeeper2 = DependencyService.Get<IScreenKeeper>(0);
									if (screenKeeper2 != null)
									{
										screenKeeper2.KeepScreenOn();
									}
								}
								catch (Exception)
								{
								}
							});
						}
						DriveCycle.SaveAndReset();
						CS$<>8__locals1.Event = obddataReader.StatusChanged;
						if (CS$<>8__locals1.Event != null)
						{
							Device.BeginInvokeOnMainThread(delegate
							{
								CS$<>8__locals1.Event(CS$<>8__locals1.status);
							});
						}
						if (CS$<>8__locals1.status != OBDDataReaderStatus.Disconnected)
						{
							goto IL_018D;
						}
						taskAwaiter = PCLDebugStream.CurrentInstance.Flush().GetAwaiter();
						if (!taskAwaiter.IsCompleted)
						{
							num2 = 0;
							TaskAwaiter taskAwaiter2 = taskAwaiter;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, OBDDataReader.<OnStatusChanged>d__104>(ref taskAwaiter, ref this);
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
					IL_018D:
					if (CS$<>8__locals1.status == OBDDataReaderStatus.ConnectedToECU)
					{
						obddataReader.lastTimeConnected = obddataReader.stopwatch.ElapsedTicks;
					}
					Droid_IBackgroundService instance = Droid_BackgroundService.Instance;
					if (instance != null)
					{
						instance.UpdateStatus(CS$<>8__locals1.newStatus);
					}
					MainThreadHelper.InvokeOnMainThread(delegate
					{
						CarPlayManager instance2 = CarPlayManager.Instance;
						if (instance2 == null)
						{
							return;
						}
						instance2.OnOBDStatusChanged(CS$<>8__locals1.newStatus);
					});
					if (SharedSettings.Current.ShowConnectionStatusOverlay)
					{
						ToastHelper.DisplayOBDStatusToast(CS$<>8__locals1.status);
					}
				}
				catch (Exception ex)
				{
					num2 = -2;
					CS$<>8__locals1 = null;
					this.<>t__builder.SetException(ex);
					return;
				}
				num2 = -2;
				CS$<>8__locals1 = null;
				this.<>t__builder.SetResult();
			}

			// Token: 0x0600261B RID: 9755 RVA: 0x001D32E0 File Offset: 0x001D14E0
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x04001410 RID: 5136
			public int <>1__state;

			// Token: 0x04001411 RID: 5137
			public AsyncVoidMethodBuilder <>t__builder;

			// Token: 0x04001412 RID: 5138
			public OBDDataReaderStatus newStatus;

			// Token: 0x04001413 RID: 5139
			public OBDDataReader <>4__this;

			// Token: 0x04001414 RID: 5140
			private OBDDataReader.<>c__DisplayClass104_0 <>8__1;

			// Token: 0x04001415 RID: 5141
			private TaskAwaiter <>u__1;
		}

		// Token: 0x02000375 RID: 885
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <PerformAlternativePIDTest>d__200 : IAsyncStateMachine
		{
			// Token: 0x0600261C RID: 9756 RVA: 0x001D32F0 File Offset: 0x001D14F0
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				OBDDataReader obddataReader = this;
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
							goto IL_0138;
						}
						obddataReader.CurrentMode = OBDDataReader.OBDModes.Mode06;
						obddataReader.CurrentCarData.ShouldSetAsAvailable = true;
						List<PID>.Enumerator enumerator = obddataReader.CurrentCarData.LiveDataPIDs.GetEnumerator();
						try
						{
							while (enumerator.MoveNext())
							{
								PID pid = enumerator.Current;
								if (!(pid is CalculatedPIDV2) && !(pid is SensorPID))
								{
									obddataReader.AddRequestToQueue(pid.Command);
								}
							}
						}
						finally
						{
							if (num < 0)
							{
								((IDisposable)enumerator).Dispose();
							}
						}
						taskAwaiter = obddataReader.WaitForCommandQueue().GetAwaiter();
						if (!taskAwaiter.IsCompleted)
						{
							num = (num2 = 0);
							TaskAwaiter taskAwaiter2 = taskAwaiter;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, OBDDataReader.<PerformAlternativePIDTest>d__200>(ref taskAwaiter, ref this);
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
					taskAwaiter = Task.Delay(500).GetAwaiter();
					if (!taskAwaiter.IsCompleted)
					{
						num = (num2 = 1);
						TaskAwaiter taskAwaiter2 = taskAwaiter;
						this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, OBDDataReader.<PerformAlternativePIDTest>d__200>(ref taskAwaiter, ref this);
						return;
					}
					IL_0138:
					taskAwaiter.GetResult();
					obddataReader.CurrentMode = OBDDataReader.OBDModes.Universal;
					obddataReader.CurrentCarData.ShouldSetAsAvailable = false;
				}
				catch (Exception ex)
				{
					num2 = -2;
					this.<>t__builder.SetException(ex);
					return;
				}
				num2 = -2;
				this.<>t__builder.SetResult();
			}

			// Token: 0x0600261D RID: 9757 RVA: 0x001D34B4 File Offset: 0x001D16B4
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x04001416 RID: 5142
			public int <>1__state;

			// Token: 0x04001417 RID: 5143
			public AsyncTaskMethodBuilder <>t__builder;

			// Token: 0x04001418 RID: 5144
			public OBDDataReader <>4__this;

			// Token: 0x04001419 RID: 5145
			private TaskAwaiter <>u__1;
		}

		// Token: 0x02000376 RID: 886
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <ReadData>d__143 : IAsyncStateMachine
		{
			// Token: 0x0600261E RID: 9758 RVA: 0x001D34C4 File Offset: 0x001D16C4
			unsafe void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				OBDDataReader obddataReader = this;
				string text;
				try
				{
					TaskAwaiter taskAwaiter3;
					TaskAwaiter<bool> taskAwaiter5;
					switch (num)
					{
					case 0:
					{
						TaskAwaiter taskAwaiter4;
						taskAwaiter3 = taskAwaiter4;
						taskAwaiter4 = default(TaskAwaiter);
						num = (num2 = -1);
						break;
					}
					case 1:
					case 2:
						IL_0189:
						try
						{
							ValueTaskAwaiter<byte[]> valueTaskAwaiter;
							if (num != 1)
							{
								if (num == 2)
								{
									TaskAwaiter taskAwaiter4;
									taskAwaiter3 = taskAwaiter4;
									taskAwaiter4 = default(TaskAwaiter);
									num = (num2 = -1);
									goto IL_0267;
								}
								valueTaskAwaiter = obddataReader.Connection.ReadBytesAsync().GetAwaiter();
								if (!valueTaskAwaiter.IsCompleted)
								{
									num = (num2 = 1);
									ValueTaskAwaiter<byte[]> valueTaskAwaiter2 = valueTaskAwaiter;
									this.<>t__builder.AwaitUnsafeOnCompleted<ValueTaskAwaiter<byte[]>, OBDDataReader.<ReadData>d__143>(ref valueTaskAwaiter, ref this);
									return;
								}
							}
							else
							{
								ValueTaskAwaiter<byte[]> valueTaskAwaiter2;
								valueTaskAwaiter = valueTaskAwaiter2;
								valueTaskAwaiter2 = default(ValueTaskAwaiter<byte[]>);
								num = (num2 = -1);
							}
							byte[] result = valueTaskAwaiter.GetResult();
							bytes = result;
							if (bytes.Length != 0)
							{
								goto IL_028D;
							}
							taskAwaiter3 = Task.Delay(10).GetAwaiter();
							if (!taskAwaiter3.IsCompleted)
							{
								num = (num2 = 2);
								TaskAwaiter taskAwaiter4 = taskAwaiter3;
								this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, OBDDataReader.<ReadData>d__143>(ref taskAwaiter3, ref this);
								return;
							}
							IL_0267:
							taskAwaiter3.GetResult();
							goto IL_0430;
						}
						catch (Exception ex)
						{
							obddataReader.LastAction = OBDDataReader.RemoteDeviceLastAction.Read;
							throw new GeneralReadingException("General read exception", ex);
						}
						IL_028D:
						if (bytes == null || bytes.Length == 0)
						{
							goto IL_0302;
						}
						taskAwaiter3 = obddataReader.DebugWrite(bytes).GetAwaiter();
						if (!taskAwaiter3.IsCompleted)
						{
							num = (num2 = 3);
							TaskAwaiter taskAwaiter4 = taskAwaiter3;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, OBDDataReader.<ReadData>d__143>(ref taskAwaiter3, ref this);
							return;
						}
						goto IL_02FB;
					case 3:
					{
						TaskAwaiter taskAwaiter4;
						taskAwaiter3 = taskAwaiter4;
						taskAwaiter4 = default(TaskAwaiter);
						num = (num2 = -1);
						goto IL_02FB;
					}
					case 4:
					{
						TaskAwaiter taskAwaiter4;
						taskAwaiter3 = taskAwaiter4;
						taskAwaiter4 = default(TaskAwaiter);
						num = (num2 = -1);
						goto IL_0595;
					}
					case 5:
						taskAwaiter5 = taskAwaiter2;
						taskAwaiter2 = default(TaskAwaiter<bool>);
						num = (num2 = -1);
						goto IL_06B0;
					default:
					{
						CS$<>8__locals1 = new OBDDataReader.<>c__DisplayClass143_0();
						maxLengthBytes = 2048;
						wasRunningWhenLaunched = obddataReader.Running;
						if (obddataReader.SendDelay <= 0)
						{
							goto IL_0113;
						}
						TimeSpan timeSpan = new TimeSpan(obddataReader.stopwatch.ElapsedTicks - obddataReader.LastReadOrWriteTicks);
						if ((int)timeSpan.TotalMilliseconds <= 0)
						{
							goto IL_0113;
						}
						int num3 = obddataReader.SendDelay / 2 - (int)timeSpan.TotalMilliseconds;
						if (num3 <= 0)
						{
							goto IL_0113;
						}
						taskAwaiter3 = Task.Delay(num3).GetAwaiter();
						if (!taskAwaiter3.IsCompleted)
						{
							num = (num2 = 0);
							TaskAwaiter taskAwaiter4 = taskAwaiter3;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, OBDDataReader.<ReadData>d__143>(ref taskAwaiter3, ref this);
							return;
						}
						break;
					}
					}
					taskAwaiter3.GetResult();
					if (wasRunningWhenLaunched && !obddataReader.Running)
					{
						text = "";
						goto IL_0726;
					}
					IL_0113:
					obddataReader.readSb.Clear();
					dataCount = 0;
					CS$<>8__locals1.hasFinishCharacter = false;
					obddataReader.swReadData.Reset();
					obddataReader.swReadData.Start();
					loopcounter = 0;
					receivedLines = 0;
					goto IL_0430;
					IL_02FB:
					taskAwaiter3.GetResult();
					IL_0302:
					bool flag = false;
					int num4 = 0;
					for (int i = 0; i < bytes.Length; i++)
					{
						if (bytes[i] != 0)
						{
							num4++;
						}
					}
					CS$<>8__locals2.partLineCounter = 0;
					string text2 = string.Create<byte[]>(num4, bytes, delegate(Span<char> span, byte[] src)
					{
						int num6 = 0;
						foreach (byte b in src)
						{
							if (b != 0)
							{
								*span[num6++] = (char)b;
							}
							if (b == 62)
							{
								CS$<>8__locals2.CS$<>8__locals1.hasFinishCharacter = true;
							}
							if (b == 13 || b == 10)
							{
								int partLineCounter = CS$<>8__locals2.partLineCounter;
								CS$<>8__locals2.partLineCounter = partLineCounter + 1;
							}
						}
					});
					if (!string.IsNullOrEmpty(text2))
					{
						obddataReader.readSb.Append(text2);
						flag = true;
					}
					if (CS$<>8__locals2.CS$<>8__locals1.hasFinishCharacter)
					{
						obddataReader.swReadData.Stop();
						goto IL_0447;
					}
					if (maxLines > 0 && !CS$<>8__locals2.CS$<>8__locals1.hasFinishCharacter)
					{
						receivedLines += CS$<>8__locals2.partLineCounter;
						if (receivedLines > maxLines)
						{
							goto IL_0447;
						}
					}
					if (lengthTooBigHandler != null && dataCount > maxLengthBytes)
					{
						Action action = lengthTooBigHandler;
						if (action != null)
						{
							action();
						}
						CS$<>8__locals2.CS$<>8__locals1.hasFinishCharacter = true;
						goto IL_0447;
					}
					if (flag)
					{
						obddataReader.swReadData.Restart();
					}
					CS$<>8__locals2 = null;
					bytes = null;
					IL_0430:
					if (obddataReader.swReadData.ElapsedMilliseconds < (long)timeout_ms)
					{
						CS$<>8__locals2 = new OBDDataReader.<>c__DisplayClass143_1();
						CS$<>8__locals2.CS$<>8__locals1 = CS$<>8__locals1;
						int num5 = loopcounter;
						loopcounter = num5 + 1;
						goto IL_0189;
					}
					IL_0447:
					res = obddataReader.readSb.ToString();
					if (!CS$<>8__locals1.hasFinishCharacter && timeout_ms == 2500 && res.Contains("SEARCHING", StringComparison.OrdinalIgnoreCase))
					{
						timeout_ms = 7500;
						goto IL_0430;
					}
					obddataReader.swReadData.Stop();
					if (CS$<>8__locals1.hasFinishCharacter)
					{
						obddataReader.initFailCounterOnAT = 0;
						obddataReader.LastAction = OBDDataReader.RemoteDeviceLastAction.Read;
						obddataReader.ELMStatus.CheckForErrors(res);
						text = res;
						goto IL_0726;
					}
					taskAwaiter3 = obddataReader.DebugWrite(string.Concat(new string[]
					{
						"\n[NoFinishCharacter, timeElapsed=",
						obddataReader.swReadData.ElapsedMilliseconds.ToString(),
						" loopCounter=",
						loopcounter.ToString(),
						" res.length=",
						res.Length.ToString(),
						"]\n"
					})).GetAwaiter();
					if (!taskAwaiter3.IsCompleted)
					{
						num = (num2 = 4);
						TaskAwaiter taskAwaiter4 = taskAwaiter3;
						this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, OBDDataReader.<ReadData>d__143>(ref taskAwaiter3, ref this);
						return;
					}
					IL_0595:
					taskAwaiter3.GetResult();
					obddataReader.ELMStatus.CheckForErrors(res);
					switch (SharedSettings.Current.ReadPartialErrorAction)
					{
					case ReadPartialErrorActions.ResetConnection:
						obddataReader.Connection.Disconect();
						taskAwaiter5 = obddataReader.Connect(false).GetAwaiter();
						if (!taskAwaiter5.IsCompleted)
						{
							num = (num2 = 5);
							taskAwaiter2 = taskAwaiter5;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<bool>, OBDDataReader.<ReadData>d__143>(ref taskAwaiter5, ref this);
							return;
						}
						goto IL_06B0;
					case ReadPartialErrorActions.Ignore:
						obddataReader.initFailCounterOnAT++;
						if (obddataReader.initFailCounterOnAT > SharedSettings.Current.NoDataLimit)
						{
							throw new GeneralReadingException("initFailCounterOnAT>" + SharedSettings.Current.NoDataLimit.ToString());
						}
						obddataReader.LastAction = OBDDataReader.RemoteDeviceLastAction.Read;
						if (res == null)
						{
							res = "";
						}
						res = res.Trim();
						text = res;
						goto IL_0726;
					}
					throw new GeneralReadingException("Data packet not finished: [" + res + "]");
					IL_06B0:
					if (!taskAwaiter5.GetResult())
					{
						obddataReader.CurrentStatus = OBDDataReaderStatus.ConnectedToECU;
						throw new GeneralReadingException("Fast reconnect failed");
					}
					obddataReader.CurrentStatus = OBDDataReaderStatus.ConnectedToECU;
					obddataReader.LastAction = OBDDataReader.RemoteDeviceLastAction.Read;
					text = res;
				}
				catch (Exception ex2)
				{
					num2 = -2;
					CS$<>8__locals1 = null;
					res = null;
					this.<>t__builder.SetException(ex2);
					return;
				}
				IL_0726:
				num2 = -2;
				CS$<>8__locals1 = null;
				res = null;
				this.<>t__builder.SetResult(text);
			}

			// Token: 0x0600261F RID: 9759 RVA: 0x001D3C50 File Offset: 0x001D1E50
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x0400141A RID: 5146
			public int <>1__state;

			// Token: 0x0400141B RID: 5147
			public AsyncTaskMethodBuilder<string> <>t__builder;

			// Token: 0x0400141C RID: 5148
			public OBDDataReader <>4__this;

			// Token: 0x0400141D RID: 5149
			private OBDDataReader.<>c__DisplayClass143_0 <>8__1;

			// Token: 0x0400141E RID: 5150
			private OBDDataReader.<>c__DisplayClass143_1 <>8__2;

			// Token: 0x0400141F RID: 5151
			public int maxLines;

			// Token: 0x04001420 RID: 5152
			public Action lengthTooBigHandler;

			// Token: 0x04001421 RID: 5153
			public int timeout_ms;

			// Token: 0x04001422 RID: 5154
			private int <maxLengthBytes>5__2;

			// Token: 0x04001423 RID: 5155
			private bool <wasRunningWhenLaunched>5__3;

			// Token: 0x04001424 RID: 5156
			private int <dataCount>5__4;

			// Token: 0x04001425 RID: 5157
			private int <loopcounter>5__5;

			// Token: 0x04001426 RID: 5158
			private int <receivedLines>5__6;

			// Token: 0x04001427 RID: 5159
			private string <res>5__7;

			// Token: 0x04001428 RID: 5160
			private TaskAwaiter <>u__1;

			// Token: 0x04001429 RID: 5161
			private byte[] <bytes>5__8;

			// Token: 0x0400142A RID: 5162
			private ValueTaskAwaiter<byte[]> <>u__2;

			// Token: 0x0400142B RID: 5163
			private TaskAwaiter<bool> <>u__3;
		}

		// Token: 0x02000377 RID: 887
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <ReadDataWithManualFlowControl_CAN>d__152 : IAsyncStateMachine
		{
			// Token: 0x06002620 RID: 9760 RVA: 0x001D3C60 File Offset: 0x001D1E60
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				OBDDataReader obddataReader = this;
				string text3;
				try
				{
					TaskAwaiter taskAwaiter;
					TaskAwaiter<string> taskAwaiter3;
					switch (num)
					{
					case 0:
					{
						TaskAwaiter taskAwaiter2;
						taskAwaiter = taskAwaiter2;
						taskAwaiter2 = default(TaskAwaiter);
						num = (num2 = -1);
						break;
					}
					case 1:
					{
						TaskAwaiter<string> taskAwaiter4;
						taskAwaiter3 = taskAwaiter4;
						taskAwaiter4 = default(TaskAwaiter<string>);
						num = (num2 = -1);
						goto IL_0146;
					}
					case 2:
					{
						TaskAwaiter taskAwaiter2;
						taskAwaiter = taskAwaiter2;
						taskAwaiter2 = default(TaskAwaiter);
						num = (num2 = -1);
						goto IL_025F;
					}
					case 3:
					{
						TaskAwaiter<string> taskAwaiter4;
						taskAwaiter3 = taskAwaiter4;
						taskAwaiter4 = default(TaskAwaiter<string>);
						num = (num2 = -1);
						goto IL_02C4;
					}
					case 4:
					{
						TaskAwaiter taskAwaiter2;
						taskAwaiter = taskAwaiter2;
						taskAwaiter2 = default(TaskAwaiter);
						num = (num2 = -1);
						goto IL_0328;
					}
					case 5:
					{
						TaskAwaiter<string> taskAwaiter4;
						taskAwaiter3 = taskAwaiter4;
						taskAwaiter4 = default(TaskAwaiter<string>);
						num = (num2 = -1);
						goto IL_038D;
					}
					case 6:
					{
						TaskAwaiter taskAwaiter2;
						taskAwaiter = taskAwaiter2;
						taskAwaiter2 = default(TaskAwaiter);
						num = (num2 = -1);
						goto IL_03F1;
					}
					case 7:
					{
						TaskAwaiter<string> taskAwaiter4;
						taskAwaiter3 = taskAwaiter4;
						taskAwaiter4 = default(TaskAwaiter<string>);
						num = (num2 = -1);
						goto IL_0456;
					}
					case 8:
					{
						TaskAwaiter taskAwaiter2;
						taskAwaiter = taskAwaiter2;
						taskAwaiter2 = default(TaskAwaiter);
						num = (num2 = -1);
						goto IL_04F3;
					}
					case 9:
					{
						TaskAwaiter<string> taskAwaiter4;
						taskAwaiter3 = taskAwaiter4;
						taskAwaiter4 = default(TaskAwaiter<string>);
						num = (num2 = -1);
						goto IL_0559;
					}
					case 10:
					{
						TaskAwaiter taskAwaiter2;
						taskAwaiter = taskAwaiter2;
						taskAwaiter2 = default(TaskAwaiter);
						num = (num2 = -1);
						goto IL_06D5;
					}
					case 11:
					{
						TaskAwaiter<string> taskAwaiter4;
						taskAwaiter3 = taskAwaiter4;
						taskAwaiter4 = default(TaskAwaiter<string>);
						num = (num2 = -1);
						goto IL_073B;
					}
					case 12:
					{
						TaskAwaiter taskAwaiter2;
						taskAwaiter = taskAwaiter2;
						taskAwaiter2 = default(TaskAwaiter);
						num = (num2 = -1);
						goto IL_0919;
					}
					case 13:
					{
						TaskAwaiter<string> taskAwaiter4;
						taskAwaiter3 = taskAwaiter4;
						taskAwaiter4 = default(TaskAwaiter<string>);
						num = (num2 = -1);
						goto IL_097F;
					}
					case 14:
					{
						TaskAwaiter taskAwaiter2;
						taskAwaiter = taskAwaiter2;
						taskAwaiter2 = default(TaskAwaiter);
						num = (num2 = -1);
						goto IL_09E4;
					}
					case 15:
					{
						TaskAwaiter<string> taskAwaiter4;
						taskAwaiter3 = taskAwaiter4;
						taskAwaiter4 = default(TaskAwaiter<string>);
						num = (num2 = -1);
						goto IL_0A4A;
					}
					default:
						CS$<>8__locals1 = new OBDDataReader.<>c__DisplayClass152_0();
						CS$<>8__locals1.request = request;
						if (obddataReader.CurrentMode != OBDDataReader.OBDModes.ReadDTC && obddataReader.CurrentMode != OBDDataReader.OBDModes.ClearDTC)
						{
							goto IL_014E;
						}
						taskAwaiter = obddataReader.SendString("3E").GetAwaiter();
						if (!taskAwaiter.IsCompleted)
						{
							num = (num2 = 0);
							TaskAwaiter taskAwaiter2 = taskAwaiter;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, OBDDataReader.<ReadDataWithManualFlowControl_CAN>d__152>(ref taskAwaiter, ref this);
							return;
						}
						break;
					}
					taskAwaiter.GetResult();
					taskAwaiter3 = obddataReader.ReadData(2500, null, -1).GetAwaiter();
					if (!taskAwaiter3.IsCompleted)
					{
						num = (num2 = 1);
						TaskAwaiter<string> taskAwaiter4 = taskAwaiter3;
						this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<string>, OBDDataReader.<ReadDataWithManualFlowControl_CAN>d__152>(ref taskAwaiter3, ref this);
						return;
					}
					IL_0146:
					taskAwaiter3.GetResult();
					IL_014E:
					if (CS$<>8__locals1.request.ELMFormat == ELMFormat.Unknown)
					{
						CS$<>8__locals1.request.ELMFormat = obddataReader.CurrentELMFormat;
					}
					max_frames = 8;
					atfcsd = "30" + max_frames.ToString("X2") + "00";
					CS$<>8__locals1.has_extended_address = false;
					string text = CS$<>8__locals1.request.BeforeCommands.FirstOrDefault((string x) => x.StartsWith("ATCEA") && x != "ATCEA");
					if (text != null)
					{
						text.Replace("ATCEA", "");
						CS$<>8__locals1.has_extended_address = true;
					}
					taskAwaiter = obddataReader.SendString("ATCAF0").GetAwaiter();
					if (!taskAwaiter.IsCompleted)
					{
						num = (num2 = 2);
						TaskAwaiter taskAwaiter2 = taskAwaiter;
						this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, OBDDataReader.<ReadDataWithManualFlowControl_CAN>d__152>(ref taskAwaiter, ref this);
						return;
					}
					IL_025F:
					taskAwaiter.GetResult();
					taskAwaiter3 = obddataReader.ReadData(2500, null, -1).GetAwaiter();
					if (!taskAwaiter3.IsCompleted)
					{
						num = (num2 = 3);
						TaskAwaiter<string> taskAwaiter4 = taskAwaiter3;
						this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<string>, OBDDataReader.<ReadDataWithManualFlowControl_CAN>d__152>(ref taskAwaiter3, ref this);
						return;
					}
					IL_02C4:
					taskAwaiter3.GetResult();
					taskAwaiter = obddataReader.SendString("ATCFC0").GetAwaiter();
					if (!taskAwaiter.IsCompleted)
					{
						num = (num2 = 4);
						TaskAwaiter taskAwaiter2 = taskAwaiter;
						this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, OBDDataReader.<ReadDataWithManualFlowControl_CAN>d__152>(ref taskAwaiter, ref this);
						return;
					}
					IL_0328:
					taskAwaiter.GetResult();
					taskAwaiter3 = obddataReader.ReadData(2500, null, -1).GetAwaiter();
					if (!taskAwaiter3.IsCompleted)
					{
						num = (num2 = 5);
						TaskAwaiter<string> taskAwaiter4 = taskAwaiter3;
						this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<string>, OBDDataReader.<ReadDataWithManualFlowControl_CAN>d__152>(ref taskAwaiter3, ref this);
						return;
					}
					IL_038D:
					taskAwaiter3.GetResult();
					taskAwaiter = obddataReader.SendString("ATAL").GetAwaiter();
					if (!taskAwaiter.IsCompleted)
					{
						num = (num2 = 6);
						TaskAwaiter taskAwaiter2 = taskAwaiter;
						this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, OBDDataReader.<ReadDataWithManualFlowControl_CAN>d__152>(ref taskAwaiter, ref this);
						return;
					}
					IL_03F1:
					taskAwaiter.GetResult();
					taskAwaiter3 = obddataReader.ReadData(2500, null, -1).GetAwaiter();
					if (!taskAwaiter3.IsCompleted)
					{
						num = (num2 = 7);
						TaskAwaiter<string> taskAwaiter4 = taskAwaiter3;
						this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<string>, OBDDataReader.<ReadDataWithManualFlowControl_CAN>d__152>(ref taskAwaiter3, ref this);
						return;
					}
					IL_0456:
					taskAwaiter3.GetResult();
					string text2 = (CS$<>8__locals1.request.Command.Length / 2).ToString("X2") + CS$<>8__locals1.request.Command;
					taskAwaiter = obddataReader.SendString(text2).GetAwaiter();
					if (!taskAwaiter.IsCompleted)
					{
						num = (num2 = 8);
						TaskAwaiter taskAwaiter2 = taskAwaiter;
						this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, OBDDataReader.<ReadDataWithManualFlowControl_CAN>d__152>(ref taskAwaiter, ref this);
						return;
					}
					IL_04F3:
					taskAwaiter.GetResult();
					taskAwaiter3 = obddataReader.ReadData(2500, null, -1).GetAwaiter();
					if (!taskAwaiter3.IsCompleted)
					{
						num = (num2 = 9);
						TaskAwaiter<string> taskAwaiter4 = taskAwaiter3;
						this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<string>, OBDDataReader.<ReadDataWithManualFlowControl_CAN>d__152>(ref taskAwaiter3, ref this);
						return;
					}
					IL_0559:
					string[] array = OBDDataReader.FilterHexAndNewLineOnly(taskAwaiter3.GetResult()).Split(OBDDataReader.line_splitter, StringSplitOptions.RemoveEmptyEntries);
					resultFrames = (from x in array
						where x != null && !x.Contains("ERROR") && !x.Contains("BUFFER")
						select new CANFrame(x, CS$<>8__locals1.request.ELMFormat, CS$<>8__locals1.has_extended_address)).ToList<CANFrame>();
					CANFrame canframe = resultFrames.FirstOrDefault((CANFrame x) => x.Type == CANFrame.CANFrameTypes.MultiFrameFirstFrame);
					IL_05E5:
					int num7;
					if (canframe != null)
					{
						int num3 = (CS$<>8__locals1.has_extended_address ? 5 : 6);
						int num4 = (CS$<>8__locals1.has_extended_address ? 6 : 7);
						int num5 = canframe.ExpectedLength - num3;
						int num6 = num5 / num4;
						if (num5 % num4 != 0)
						{
							num6++;
						}
						read_cycles = num6 / max_frames;
						if (read_cycles == 0)
						{
							read_cycles = 1;
						}
						else if (num6 % max_frames > 0)
						{
							num7 = read_cycles;
							read_cycles = num7 + 1;
						}
						i = 0;
						goto IL_0874;
					}
					goto IL_08BC;
					IL_06D5:
					taskAwaiter.GetResult();
					taskAwaiter3 = obddataReader.ReadData(2500, null, -1).GetAwaiter();
					if (!taskAwaiter3.IsCompleted)
					{
						num = (num2 = 11);
						TaskAwaiter<string> taskAwaiter4 = taskAwaiter3;
						this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<string>, OBDDataReader.<ReadDataWithManualFlowControl_CAN>d__152>(ref taskAwaiter3, ref this);
						return;
					}
					IL_073B:
					string result = taskAwaiter3.GetResult();
					if (result.Contains("NO DATA"))
					{
						goto IL_08BC;
					}
					IEnumerable<string> enumerable = OBDDataReader.FilterHexAndNewLineOnly(result).Split(OBDDataReader.line_splitter, StringSplitOptions.RemoveEmptyEntries);
					Func<string, CANFrame> func;
					if ((func = CS$<>8__locals1.<>9__4) == null)
					{
						func = (CS$<>8__locals1.<>9__4 = (string x) => new CANFrame(x, CS$<>8__locals1.request.ELMFormat, CS$<>8__locals1.has_extended_address));
					}
					CANFrame[] array2 = enumerable.Select(func).ToArray<CANFrame>();
					if (array2.Length == 1 && array2[0].Data.Length >= 3 && array2[0].Data[0] == 3 && array2[0].Data[1] == 127 && (int)array2[0].Data[2] == max_frames && array2[0].Data[3] == 34)
					{
						SharedSettings.Current.ForceUseManualFlowControlWhileReadingData = false;
						CS$<>8__locals1.request.ForceManualFlowControl = false;
						goto IL_08BC;
					}
					resultFrames.AddRange(array2);
					IProgress<string> progress = CS$<>8__locals1.request.Progress;
					if (progress != null)
					{
						progress.Report(string.Format("{0}/{1}", i + 1, read_cycles));
					}
					num7 = i;
					i = num7 + 1;
					IL_0874:
					if (i >= read_cycles)
					{
						if (resultFrames.Count > 1)
						{
							CANFrame canframe2 = resultFrames.Last<CANFrame>();
							if (canframe2.Type == CANFrame.CANFrameTypes.SingleFrame || canframe2.Type == CANFrame.CANFrameTypes.MultiFrameFirstFrame)
							{
								canframe = canframe2;
								goto IL_05E5;
							}
						}
					}
					else
					{
						taskAwaiter = obddataReader.SendString(atfcsd).GetAwaiter();
						if (!taskAwaiter.IsCompleted)
						{
							num = (num2 = 10);
							TaskAwaiter taskAwaiter2 = taskAwaiter;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, OBDDataReader.<ReadDataWithManualFlowControl_CAN>d__152>(ref taskAwaiter, ref this);
							return;
						}
						goto IL_06D5;
					}
					IL_08BC:
					taskAwaiter = obddataReader.SendString("ATCAF1").GetAwaiter();
					if (!taskAwaiter.IsCompleted)
					{
						num = (num2 = 12);
						TaskAwaiter taskAwaiter2 = taskAwaiter;
						this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, OBDDataReader.<ReadDataWithManualFlowControl_CAN>d__152>(ref taskAwaiter, ref this);
						return;
					}
					IL_0919:
					taskAwaiter.GetResult();
					taskAwaiter3 = obddataReader.ReadData(2500, null, -1).GetAwaiter();
					if (!taskAwaiter3.IsCompleted)
					{
						num = (num2 = 13);
						TaskAwaiter<string> taskAwaiter4 = taskAwaiter3;
						this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<string>, OBDDataReader.<ReadDataWithManualFlowControl_CAN>d__152>(ref taskAwaiter3, ref this);
						return;
					}
					IL_097F:
					taskAwaiter3.GetResult();
					taskAwaiter = obddataReader.SendString("ATCFC1").GetAwaiter();
					if (!taskAwaiter.IsCompleted)
					{
						num = (num2 = 14);
						TaskAwaiter taskAwaiter2 = taskAwaiter;
						this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, OBDDataReader.<ReadDataWithManualFlowControl_CAN>d__152>(ref taskAwaiter, ref this);
						return;
					}
					IL_09E4:
					taskAwaiter.GetResult();
					taskAwaiter3 = obddataReader.ReadData(2500, null, -1).GetAwaiter();
					if (!taskAwaiter3.IsCompleted)
					{
						num = (num2 = 15);
						TaskAwaiter<string> taskAwaiter4 = taskAwaiter3;
						this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<string>, OBDDataReader.<ReadDataWithManualFlowControl_CAN>d__152>(ref taskAwaiter3, ref this);
						return;
					}
					IL_0A4A:
					taskAwaiter3.GetResult();
					StringBuilder stringBuilder = new StringBuilder(resultFrames.Count * 2 + 1);
					List<CANFrame>.Enumerator enumerator = resultFrames.GetEnumerator();
					try
					{
						while (enumerator.MoveNext())
						{
							CANFrame canframe3 = enumerator.Current;
							stringBuilder.Append(canframe3.ToString());
							stringBuilder.Append('\r');
						}
					}
					finally
					{
						if (num < 0)
						{
							((IDisposable)enumerator).Dispose();
						}
					}
					text3 = stringBuilder.ToString();
				}
				catch (Exception ex)
				{
					num2 = -2;
					CS$<>8__locals1 = null;
					atfcsd = null;
					resultFrames = null;
					this.<>t__builder.SetException(ex);
					return;
				}
				num2 = -2;
				CS$<>8__locals1 = null;
				atfcsd = null;
				resultFrames = null;
				this.<>t__builder.SetResult(text3);
			}

			// Token: 0x06002621 RID: 9761 RVA: 0x001D47B8 File Offset: 0x001D29B8
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x0400142C RID: 5164
			public int <>1__state;

			// Token: 0x0400142D RID: 5165
			public AsyncTaskMethodBuilder<string> <>t__builder;

			// Token: 0x0400142E RID: 5166
			public OBDRequest request;

			// Token: 0x0400142F RID: 5167
			public OBDDataReader <>4__this;

			// Token: 0x04001430 RID: 5168
			private OBDDataReader.<>c__DisplayClass152_0 <>8__1;

			// Token: 0x04001431 RID: 5169
			private int <max_frames>5__2;

			// Token: 0x04001432 RID: 5170
			private string <atfcsd>5__3;

			// Token: 0x04001433 RID: 5171
			private List<CANFrame> <resultFrames>5__4;

			// Token: 0x04001434 RID: 5172
			private TaskAwaiter <>u__1;

			// Token: 0x04001435 RID: 5173
			private TaskAwaiter<string> <>u__2;

			// Token: 0x04001436 RID: 5174
			private int <read_cycles>5__5;

			// Token: 0x04001437 RID: 5175
			private int <i>5__6;
		}

		// Token: 0x02000378 RID: 888
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <ReadFreezeFrame>d__199 : IAsyncStateMachine
		{
			// Token: 0x06002622 RID: 9762 RVA: 0x001D47C8 File Offset: 0x001D29C8
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				OBDDataReader obddataReader = this;
				try
				{
					TaskAwaiter taskAwaiter;
					switch (num)
					{
					case 0:
					{
						TaskAwaiter taskAwaiter2;
						taskAwaiter = taskAwaiter2;
						taskAwaiter2 = default(TaskAwaiter);
						num = (num2 = -1);
						break;
					}
					case 1:
					{
						TaskAwaiter taskAwaiter2;
						taskAwaiter = taskAwaiter2;
						taskAwaiter2 = default(TaskAwaiter);
						num = (num2 = -1);
						goto IL_0156;
					}
					case 2:
					{
						TaskAwaiter taskAwaiter2;
						taskAwaiter = taskAwaiter2;
						taskAwaiter2 = default(TaskAwaiter);
						num = (num2 = -1);
						goto IL_026B;
					}
					default:
						obddataReader.CurrentCarData.FreezeFrameNumber = FreezeFrameNumber;
						obddataReader.CurrentCarData.CreateMode02PIDs();
						taskAwaiter = obddataReader.ClearRequestQueue().GetAwaiter();
						if (!taskAwaiter.IsCompleted)
						{
							num = (num2 = 0);
							TaskAwaiter taskAwaiter2 = taskAwaiter;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, OBDDataReader.<ReadFreezeFrame>d__199>(ref taskAwaiter, ref this);
							return;
						}
						break;
					}
					taskAwaiter.GetResult();
					requests = new List<OBDRequest>();
					for (int i = 512; i <= 672; i += 32)
					{
						OBDRequest obdrequest = new OBDRequest(i.ToString("X4") + FreezeFrameNumber.ToString("X2"), false);
						obddataReader.PreprocessRequest(obdrequest);
						requests.Add(obdrequest);
					}
					obddataReader.ReplaceQueue(requests);
					taskAwaiter = obddataReader.WaitForCommandQueue().GetAwaiter();
					if (!taskAwaiter.IsCompleted)
					{
						num = (num2 = 1);
						TaskAwaiter taskAwaiter2 = taskAwaiter;
						this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, OBDDataReader.<ReadFreezeFrame>d__199>(ref taskAwaiter, ref this);
						return;
					}
					IL_0156:
					taskAwaiter.GetResult();
					requests.Clear();
					IEnumerator<PID> enumerator = obddataReader.CurrentCarData.Mode02PIDs.Where((PID x) => x.IsAvailable && !(x is PID_SupportedPids)).GetEnumerator();
					try
					{
						while (enumerator.MoveNext())
						{
							PID pid = enumerator.Current;
							string text = "02" + pid.Command.Substring(2) + obddataReader.CurrentCarData.FreezeFrameNumber.ToString("X2", CultureInfo.InvariantCulture);
							requests.Add(new OBDRequest(text, false));
						}
					}
					finally
					{
						if (num < 0 && enumerator != null)
						{
							enumerator.Dispose();
						}
					}
					obddataReader.ReplaceQueue(requests);
					taskAwaiter = obddataReader.WaitForCommandQueue().GetAwaiter();
					if (!taskAwaiter.IsCompleted)
					{
						num = (num2 = 2);
						TaskAwaiter taskAwaiter2 = taskAwaiter;
						this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, OBDDataReader.<ReadFreezeFrame>d__199>(ref taskAwaiter, ref this);
						return;
					}
					IL_026B:
					taskAwaiter.GetResult();
				}
				catch (Exception ex)
				{
					num2 = -2;
					requests = null;
					this.<>t__builder.SetException(ex);
					return;
				}
				num2 = -2;
				requests = null;
				this.<>t__builder.SetResult();
			}

			// Token: 0x06002623 RID: 9763 RVA: 0x001D4AB8 File Offset: 0x001D2CB8
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x04001438 RID: 5176
			public int <>1__state;

			// Token: 0x04001439 RID: 5177
			public AsyncTaskMethodBuilder <>t__builder;

			// Token: 0x0400143A RID: 5178
			public OBDDataReader <>4__this;

			// Token: 0x0400143B RID: 5179
			public int FreezeFrameNumber;

			// Token: 0x0400143C RID: 5180
			private List<OBDRequest> <requests>5__2;

			// Token: 0x0400143D RID: 5181
			private TaskAwaiter <>u__1;
		}

		// Token: 0x02000379 RID: 889
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <ReadMode06>d__201 : IAsyncStateMachine
		{
			// Token: 0x06002624 RID: 9764 RVA: 0x001D4AC8 File Offset: 0x001D2CC8
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				OBDDataReader obddataReader = this;
				try
				{
					TaskAwaiter taskAwaiter;
					switch (num)
					{
					case 0:
					{
						TaskAwaiter taskAwaiter2;
						taskAwaiter = taskAwaiter2;
						taskAwaiter2 = default(TaskAwaiter);
						num2 = -1;
						break;
					}
					case 1:
					{
						TaskAwaiter taskAwaiter2;
						taskAwaiter = taskAwaiter2;
						taskAwaiter2 = default(TaskAwaiter);
						num2 = -1;
						goto IL_0117;
					}
					case 2:
					{
						TaskAwaiter taskAwaiter2;
						taskAwaiter = taskAwaiter2;
						taskAwaiter2 = default(TaskAwaiter);
						num2 = -1;
						goto IL_024E;
					}
					case 3:
					{
						TaskAwaiter taskAwaiter2;
						taskAwaiter = taskAwaiter2;
						taskAwaiter2 = default(TaskAwaiter);
						num2 = -1;
						goto IL_02B7;
					}
					default:
						CS$<>8__locals1 = new OBDDataReader.<>c__DisplayClass201_0();
						CS$<>8__locals1.<>4__this = this;
						CS$<>8__locals1.progress = progress;
						obddataReader.CurrentMode = OBDDataReader.OBDModes.Mode06;
						taskAwaiter = obddataReader.ClearRequestQueue().GetAwaiter();
						if (!taskAwaiter.IsCompleted)
						{
							num2 = 0;
							TaskAwaiter taskAwaiter2 = taskAwaiter;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, OBDDataReader.<ReadMode06>d__201>(ref taskAwaiter, ref this);
							return;
						}
						break;
					}
					taskAwaiter.GetResult();
					taskAwaiter = MainThread.InvokeOnMainThreadAsync(delegate
					{
						CS$<>8__locals1.<>4__this.CurrentCarData.Mode06TestCollection.Clear();
					}).GetAwaiter();
					if (!taskAwaiter.IsCompleted)
					{
						num2 = 1;
						TaskAwaiter taskAwaiter2 = taskAwaiter;
						this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, OBDDataReader.<ReadMode06>d__201>(ref taskAwaiter, ref this);
						return;
					}
					IL_0117:
					taskAwaiter.GetResult();
					for (int i = 0; i < 255; i++)
					{
						if (i % 32 != 0)
						{
							string text = "06" + i.ToString("X2", CultureInfo.InvariantCulture);
							obddataReader.AddRequestToQueue(text);
						}
					}
					goto IL_0255;
					IL_024E:
					taskAwaiter.GetResult();
					IL_0255:
					if (obddataReader.CommandQueue.Count <= 0)
					{
						taskAwaiter = obddataReader.WaitForCommandQueue().GetAwaiter();
						if (!taskAwaiter.IsCompleted)
						{
							num2 = 3;
							TaskAwaiter taskAwaiter2 = taskAwaiter;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, OBDDataReader.<ReadMode06>d__201>(ref taskAwaiter, ref this);
							return;
						}
					}
					else
					{
						OBDDataReader.<>c__DisplayClass201_1 CS$<>8__locals2 = new OBDDataReader.<>c__DisplayClass201_1();
						CS$<>8__locals2.CS$<>8__locals1 = CS$<>8__locals1;
						CS$<>8__locals2.p = (double)(243 - obddataReader.CommandQueue.Count);
						if (CS$<>8__locals2.p < 0.0)
						{
							CS$<>8__locals2.p = 0.0;
						}
						if (CS$<>8__locals2.p > 243.0)
						{
							CS$<>8__locals2.p = 243.0;
						}
						CS$<>8__locals2.p /= 243.0;
						Device.BeginInvokeOnMainThread(delegate
						{
							CS$<>8__locals2.CS$<>8__locals1.progress.Report(CS$<>8__locals2.p);
						});
						taskAwaiter = Task.Delay(50).GetAwaiter();
						if (!taskAwaiter.IsCompleted)
						{
							num2 = 2;
							TaskAwaiter taskAwaiter2 = taskAwaiter;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, OBDDataReader.<ReadMode06>d__201>(ref taskAwaiter, ref this);
							return;
						}
						goto IL_024E;
					}
					IL_02B7:
					taskAwaiter.GetResult();
					obddataReader.CurrentMode = OBDDataReader.OBDModes.Universal;
				}
				catch (Exception ex)
				{
					num2 = -2;
					CS$<>8__locals1 = null;
					this.<>t__builder.SetException(ex);
					return;
				}
				num2 = -2;
				CS$<>8__locals1 = null;
				this.<>t__builder.SetResult();
			}

			// Token: 0x06002625 RID: 9765 RVA: 0x001D4DF4 File Offset: 0x001D2FF4
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x0400143E RID: 5182
			public int <>1__state;

			// Token: 0x0400143F RID: 5183
			public AsyncTaskMethodBuilder <>t__builder;

			// Token: 0x04001440 RID: 5184
			public OBDDataReader <>4__this;

			// Token: 0x04001441 RID: 5185
			public IProgress<double> progress;

			// Token: 0x04001442 RID: 5186
			private OBDDataReader.<>c__DisplayClass201_0 <>8__1;

			// Token: 0x04001443 RID: 5187
			private TaskAwaiter <>u__1;
		}

		// Token: 0x0200037A RID: 890
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <ReadMode06>d__202 : IAsyncStateMachine
		{
			// Token: 0x06002626 RID: 9766 RVA: 0x001D4E04 File Offset: 0x001D3004
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				OBDDataReader obddataReader = this;
				try
				{
					TaskAwaiter taskAwaiter;
					switch (num)
					{
					case 0:
					{
						TaskAwaiter taskAwaiter2;
						taskAwaiter = taskAwaiter2;
						taskAwaiter2 = default(TaskAwaiter);
						num2 = -1;
						break;
					}
					case 1:
					{
						TaskAwaiter taskAwaiter2;
						taskAwaiter = taskAwaiter2;
						taskAwaiter2 = default(TaskAwaiter);
						num2 = -1;
						goto IL_0117;
					}
					case 2:
					{
						TaskAwaiter taskAwaiter2;
						taskAwaiter = taskAwaiter2;
						taskAwaiter2 = default(TaskAwaiter);
						num2 = -1;
						goto IL_023D;
					}
					case 3:
					{
						TaskAwaiter taskAwaiter2;
						taskAwaiter = taskAwaiter2;
						taskAwaiter2 = default(TaskAwaiter);
						num2 = -1;
						goto IL_02A6;
					}
					default:
						CS$<>8__locals1 = new OBDDataReader.<>c__DisplayClass202_0();
						CS$<>8__locals1.<>4__this = this;
						CS$<>8__locals1.progress = progress;
						obddataReader.CurrentMode = OBDDataReader.OBDModes.Mode06;
						taskAwaiter = obddataReader.ClearRequestQueue().GetAwaiter();
						if (!taskAwaiter.IsCompleted)
						{
							num2 = 0;
							TaskAwaiter taskAwaiter2 = taskAwaiter;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, OBDDataReader.<ReadMode06>d__202>(ref taskAwaiter, ref this);
							return;
						}
						break;
					}
					taskAwaiter.GetResult();
					taskAwaiter = MainThread.InvokeOnMainThreadAsync(delegate
					{
						CS$<>8__locals1.<>4__this.CurrentCarData.Mode06TestCollection.Clear();
					}).GetAwaiter();
					if (!taskAwaiter.IsCompleted)
					{
						num2 = 1;
						TaskAwaiter taskAwaiter2 = taskAwaiter;
						this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, OBDDataReader.<ReadMode06>d__202>(ref taskAwaiter, ref this);
						return;
					}
					IL_0117:
					taskAwaiter.GetResult();
					foreach (string text in pids)
					{
						obddataReader.AddRequestToQueue(text);
					}
					goto IL_0244;
					IL_023D:
					taskAwaiter.GetResult();
					IL_0244:
					if (obddataReader.CommandQueue.Count <= 0)
					{
						taskAwaiter = obddataReader.WaitForCommandQueue().GetAwaiter();
						if (!taskAwaiter.IsCompleted)
						{
							num2 = 3;
							TaskAwaiter taskAwaiter2 = taskAwaiter;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, OBDDataReader.<ReadMode06>d__202>(ref taskAwaiter, ref this);
							return;
						}
					}
					else
					{
						OBDDataReader.<>c__DisplayClass202_1 CS$<>8__locals2 = new OBDDataReader.<>c__DisplayClass202_1();
						CS$<>8__locals2.CS$<>8__locals1 = CS$<>8__locals1;
						CS$<>8__locals2.p = (double)(pids.Length - obddataReader.CommandQueue.Count);
						if (CS$<>8__locals2.p < 0.0)
						{
							CS$<>8__locals2.p = 0.0;
						}
						if (CS$<>8__locals2.p > (double)pids.Length)
						{
							CS$<>8__locals2.p = (double)pids.Length;
						}
						CS$<>8__locals2.p /= (double)pids.Length;
						MainThread.BeginInvokeOnMainThread(delegate
						{
							CS$<>8__locals2.CS$<>8__locals1.progress.Report(CS$<>8__locals2.p);
						});
						taskAwaiter = Task.Delay(50).GetAwaiter();
						if (!taskAwaiter.IsCompleted)
						{
							num2 = 2;
							TaskAwaiter taskAwaiter2 = taskAwaiter;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, OBDDataReader.<ReadMode06>d__202>(ref taskAwaiter, ref this);
							return;
						}
						goto IL_023D;
					}
					IL_02A6:
					taskAwaiter.GetResult();
					obddataReader.CurrentMode = OBDDataReader.OBDModes.Universal;
				}
				catch (Exception ex)
				{
					num2 = -2;
					CS$<>8__locals1 = null;
					this.<>t__builder.SetException(ex);
					return;
				}
				num2 = -2;
				CS$<>8__locals1 = null;
				this.<>t__builder.SetResult();
			}

			// Token: 0x06002627 RID: 9767 RVA: 0x001D5120 File Offset: 0x001D3320
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x04001444 RID: 5188
			public int <>1__state;

			// Token: 0x04001445 RID: 5189
			public AsyncTaskMethodBuilder <>t__builder;

			// Token: 0x04001446 RID: 5190
			public OBDDataReader <>4__this;

			// Token: 0x04001447 RID: 5191
			public IProgress<double> progress;

			// Token: 0x04001448 RID: 5192
			private OBDDataReader.<>c__DisplayClass202_0 <>8__1;

			// Token: 0x04001449 RID: 5193
			public string[] pids;

			// Token: 0x0400144A RID: 5194
			private TaskAwaiter <>u__1;
		}

		// Token: 0x0200037B RID: 891
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <ReinitializeConnectionToECU>d__157 : IAsyncStateMachine
		{
			// Token: 0x06002628 RID: 9768 RVA: 0x001D5130 File Offset: 0x001D3330
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				OBDDataReader obddataReader = this;
				bool flag;
				try
				{
					TaskAwaiter taskAwaiter;
					int num3;
					switch (num)
					{
					case 0:
					{
						TaskAwaiter taskAwaiter2;
						taskAwaiter = taskAwaiter2;
						taskAwaiter2 = default(TaskAwaiter);
						num = (num2 = -1);
						break;
					}
					case 1:
					case 2:
					{
						IL_009B:
						try
						{
							TaskAwaiter<bool> taskAwaiter3;
							TaskAwaiter<bool> taskAwaiter4;
							if (num != 1)
							{
								if (num != 2)
								{
									obddataReader.CurrentStatus = OBDDataReaderStatus.ConnectingToECU;
									DriveCycle.SaveAndReset();
									if (SharedSettings.Current.UseDefaultInit)
									{
										taskAwaiter3 = obddataReader.InitializeDefaultInitString(SharedSettings.Current.ProtocolNumber, OBDDataReader.InitModes.RestoreConnection, false, true).GetAwaiter();
										if (!taskAwaiter3.IsCompleted)
										{
											num2 = 1;
											taskAwaiter4 = taskAwaiter3;
											this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<bool>, OBDDataReader.<ReinitializeConnectionToECU>d__157>(ref taskAwaiter3, ref this);
											return;
										}
										goto IL_0123;
									}
									else
									{
										taskAwaiter3 = obddataReader.InitializeCustomInitString(SharedSettings.Current.CustomInitString, OBDDataReader.InitModes.RestoreConnection, true).GetAwaiter();
										if (!taskAwaiter3.IsCompleted)
										{
											num2 = 2;
											taskAwaiter4 = taskAwaiter3;
											this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<bool>, OBDDataReader.<ReinitializeConnectionToECU>d__157>(ref taskAwaiter3, ref this);
											return;
										}
									}
								}
								else
								{
									taskAwaiter3 = taskAwaiter4;
									taskAwaiter4 = default(TaskAwaiter<bool>);
									num2 = -1;
								}
								taskAwaiter3.GetResult();
								goto IL_0198;
							}
							taskAwaiter3 = taskAwaiter4;
							taskAwaiter4 = default(TaskAwaiter<bool>);
							num2 = -1;
							IL_0123:
							taskAwaiter3.GetResult();
							IL_0198:
							obddataReader.CurrentStatus = OBDDataReaderStatus.ConnectedToECU;
							obddataReader.ELM327_LastSentHeader = "";
							obddataReader.ELM327_LastSentBeforeCommands = new string[0];
							obddataReader.ELM327_PendingAfterCommands = new string[0];
							obddataReader.Start("From ReinitializeConnectionToECU_Success");
							flag = true;
							goto IL_0309;
						}
						catch (Exception obj)
						{
							num3 = 1;
						}
						if (num3 != 1)
						{
							goto IL_02EE;
						}
						object obj;
						Exception ex = (Exception)obj;
						obddataReader.CurrentStatus = OBDDataReaderStatus.ConnectingToECU;
						obddataReader.SetRunning(false, "From ReinitializeConnectionToECU #2239");
						obddataReader.ELM327_LastSentHeader = "";
						obddataReader.ELM327_LastSentBeforeCommands = new string[0];
						obddataReader.ELM327_PendingAfterCommands = new string[0];
						taskAwaiter = obddataReader.OnDisconnectDetected(OBDDataReader.DisconnectReason.ELMStuck).GetAwaiter();
						if (!taskAwaiter.IsCompleted)
						{
							num2 = 3;
							TaskAwaiter taskAwaiter2 = taskAwaiter;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, OBDDataReader.<ReinitializeConnectionToECU>d__157>(ref taskAwaiter, ref this);
							return;
						}
						goto IL_0276;
					}
					case 3:
					{
						TaskAwaiter taskAwaiter2;
						taskAwaiter = taskAwaiter2;
						taskAwaiter2 = default(TaskAwaiter);
						num2 = -1;
						goto IL_0276;
					}
					case 4:
					{
						TaskAwaiter taskAwaiter2;
						taskAwaiter = taskAwaiter2;
						taskAwaiter2 = default(TaskAwaiter);
						num2 = -1;
						goto IL_02D2;
					}
					default:
						taskAwaiter = obddataReader.DebugWrite("\nReinitializeConnectionToECU(" + predstavsa_mraz + ")\n").GetAwaiter();
						if (!taskAwaiter.IsCompleted)
						{
							num2 = 0;
							TaskAwaiter taskAwaiter2 = taskAwaiter;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, OBDDataReader.<ReinitializeConnectionToECU>d__157>(ref taskAwaiter, ref this);
							return;
						}
						break;
					}
					taskAwaiter.GetResult();
					num3 = 0;
					goto IL_009B;
					IL_0276:
					taskAwaiter.GetResult();
					taskAwaiter = Task.Delay(500).GetAwaiter();
					if (!taskAwaiter.IsCompleted)
					{
						num2 = 4;
						TaskAwaiter taskAwaiter2 = taskAwaiter;
						this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, OBDDataReader.<ReinitializeConnectionToECU>d__157>(ref taskAwaiter, ref this);
						return;
					}
					IL_02D2:
					taskAwaiter.GetResult();
					if (!obddataReader.Running)
					{
						obddataReader.Start("From ReinitializeConnectionToECU_Exception");
					}
					flag = false;
					IL_02EE:;
				}
				catch (Exception ex2)
				{
					num2 = -2;
					this.<>t__builder.SetException(ex2);
					return;
				}
				IL_0309:
				num2 = -2;
				this.<>t__builder.SetResult(flag);
			}

			// Token: 0x06002629 RID: 9769 RVA: 0x001D5490 File Offset: 0x001D3690
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x0400144B RID: 5195
			public int <>1__state;

			// Token: 0x0400144C RID: 5196
			public AsyncTaskMethodBuilder<bool> <>t__builder;

			// Token: 0x0400144D RID: 5197
			public OBDDataReader <>4__this;

			// Token: 0x0400144E RID: 5198
			public string predstavsa_mraz;

			// Token: 0x0400144F RID: 5199
			private TaskAwaiter <>u__1;

			// Token: 0x04001450 RID: 5200
			private TaskAwaiter<bool> <>u__2;
		}

		// Token: 0x0200037C RID: 892
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <SearchForProtocol>d__178 : IAsyncStateMachine
		{
			// Token: 0x0600262A RID: 9770 RVA: 0x001D54A0 File Offset: 0x001D36A0
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				OBDDataReader obddataReader = this;
				int num4;
				try
				{
					TaskAwaiter<bool> taskAwaiter3;
					if (num != 0)
					{
						if (num != 1)
						{
							obddataReader.CurrentStatus = OBDDataReaderStatus.ConnectingToECU;
							int[] array = new int[]
							{
								0, 6, 7, 4, 5, 3, 1, 2, 8, 9,
								10, 11, 37, 33, 43, 44, 45
							};
							int[] array2 = new int[]
							{
								0, 6, 7, 4, 5, 3, 37, 33, 44, 12,
								13, 14, 15, 16, 17, 18, 19, 20, 21, 22,
								23, 24, 25, 26, 27, 28, 29, 30, 31, 32,
								34, 35, 36, 38, 39, 40, 41, 42, 45
							};
							protocols = array;
							if (SharedSettings.Current.SelectedBrand == "Toyota" || SharedSettings.Current.SelectedBrand == "Lexus")
							{
								protocols = array2;
							}
							i = 1;
							goto IL_02C1;
						}
						taskAwaiter3 = taskAwaiter2;
						taskAwaiter2 = default(TaskAwaiter<bool>);
						num = (num2 = -1);
						goto IL_0252;
					}
					IL_0165:
					int num3;
					try
					{
						if (num != 0)
						{
							taskAwaiter3 = obddataReader.InitializeDefaultInitString(protocols[i], OBDDataReader.InitModes.Default, true, true).GetAwaiter();
							if (!taskAwaiter3.IsCompleted)
							{
								num = (num2 = 0);
								taskAwaiter2 = taskAwaiter3;
								this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<bool>, OBDDataReader.<SearchForProtocol>d__178>(ref taskAwaiter3, ref this);
								return;
							}
						}
						else
						{
							taskAwaiter3 = taskAwaiter2;
							taskAwaiter2 = default(TaskAwaiter<bool>);
							num = (num2 = -1);
						}
						bool result2 = taskAwaiter3.GetResult();
						result = result2;
						goto IL_0285;
					}
					catch (Exception obj)
					{
						num3 = 1;
					}
					if (num3 != 1)
					{
						goto IL_026B;
					}
					object obj;
					Exception ex = (Exception)obj;
					taskAwaiter3 = obddataReader.Connect(true).GetAwaiter();
					if (!taskAwaiter3.IsCompleted)
					{
						num = (num2 = 1);
						taskAwaiter2 = taskAwaiter3;
						this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<bool>, OBDDataReader.<SearchForProtocol>d__178>(ref taskAwaiter3, ref this);
						return;
					}
					IL_0252:
					if (!taskAwaiter3.GetResult() && attempts == 2)
					{
						num4 = 0;
						goto IL_02F8;
					}
					IL_026B:
					if (obddataReader.DisconnectRequested)
					{
						num4 = 0;
						goto IL_02F8;
					}
					IL_0279:
					if (attempts < 3)
					{
						if (obddataReader.DisconnectRequested)
						{
							num4 = 0;
							goto IL_02F8;
						}
						num3 = attempts;
						attempts = num3 + 1;
						num3 = 0;
						goto IL_0165;
					}
					IL_0285:
					if (result)
					{
						obddataReader.CurrentStatus = OBDDataReaderStatus.ConnectedToECU;
						SharedSettings.Current.ATSTIdx = 7;
						num4 = protocols[i];
						goto IL_02F8;
					}
					num3 = i;
					i = num3 + 1;
					IL_02C1:
					if (i >= protocols.Length)
					{
						num4 = 0;
					}
					else
					{
						if (!obddataReader.DisconnectRequested)
						{
							string[] array3 = new string[6];
							array3[0] = Translate.GetString("ios_ECUinitProgressStage2");
							array3[1] = "\n[";
							array3[2] = i.ToString();
							array3[3] = "/";
							int num5 = 4;
							num3 = protocols.Length;
							array3[num5] = num3.ToString();
							array3[5] = "]";
							string text = string.Concat(array3);
							string text2 = StaticLists.Protocols[protocols[i]];
							text = string.Format(text, text2);
							IProgress<string> progress = progress;
							if (progress != null)
							{
								progress.Report(text);
							}
							result = false;
							attempts = 0;
							goto IL_0279;
						}
						num4 = 0;
					}
				}
				catch (Exception ex2)
				{
					num2 = -2;
					protocols = null;
					this.<>t__builder.SetException(ex2);
					return;
				}
				IL_02F8:
				num2 = -2;
				protocols = null;
				this.<>t__builder.SetResult(num4);
			}

			// Token: 0x0600262B RID: 9771 RVA: 0x001D57F4 File Offset: 0x001D39F4
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x04001451 RID: 5201
			public int <>1__state;

			// Token: 0x04001452 RID: 5202
			public AsyncTaskMethodBuilder<int> <>t__builder;

			// Token: 0x04001453 RID: 5203
			public OBDDataReader <>4__this;

			// Token: 0x04001454 RID: 5204
			public IProgress<string> progress;

			// Token: 0x04001455 RID: 5205
			private int[] <protocols>5__2;

			// Token: 0x04001456 RID: 5206
			private int <i>5__3;

			// Token: 0x04001457 RID: 5207
			private bool <result>5__4;

			// Token: 0x04001458 RID: 5208
			private int <attempts>5__5;

			// Token: 0x04001459 RID: 5209
			private TaskAwaiter<bool> <>u__1;
		}

		// Token: 0x0200037D RID: 893
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <SendATZ>d__176 : IAsyncStateMachine
		{
			// Token: 0x0600262C RID: 9772 RVA: 0x001D5804 File Offset: 0x001D3A04
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				OBDDataReader obddataReader = this;
				string text;
				try
				{
					TaskAwaiter taskAwaiter;
					TaskAwaiter<string> taskAwaiter3;
					ValueTaskAwaiter<byte[]> valueTaskAwaiter;
					switch (num)
					{
					case 0:
					{
						TaskAwaiter taskAwaiter2;
						taskAwaiter = taskAwaiter2;
						taskAwaiter2 = default(TaskAwaiter);
						num2 = -1;
						break;
					}
					case 1:
					{
						TaskAwaiter taskAwaiter2;
						taskAwaiter = taskAwaiter2;
						taskAwaiter2 = default(TaskAwaiter);
						num2 = -1;
						goto IL_0102;
					}
					case 2:
					{
						TaskAwaiter taskAwaiter2;
						taskAwaiter = taskAwaiter2;
						taskAwaiter2 = default(TaskAwaiter);
						num2 = -1;
						goto IL_0188;
					}
					case 3:
					{
						TaskAwaiter<string> taskAwaiter4;
						taskAwaiter3 = taskAwaiter4;
						taskAwaiter4 = default(TaskAwaiter<string>);
						num2 = -1;
						goto IL_01ED;
					}
					case 4:
					{
						ValueTaskAwaiter<byte[]> valueTaskAwaiter2;
						valueTaskAwaiter = valueTaskAwaiter2;
						valueTaskAwaiter2 = default(ValueTaskAwaiter<byte[]>);
						num2 = -1;
						goto IL_026D;
					}
					case 5:
					{
						TaskAwaiter taskAwaiter2;
						taskAwaiter = taskAwaiter2;
						taskAwaiter2 = default(TaskAwaiter);
						num2 = -1;
						goto IL_0303;
					}
					case 6:
					{
						TaskAwaiter taskAwaiter2;
						taskAwaiter = taskAwaiter2;
						taskAwaiter2 = default(TaskAwaiter);
						num2 = -1;
						goto IL_0361;
					}
					default:
						if (!SharedSettings.Current.SendATZATE)
						{
							taskAwaiter = obddataReader.SendString("ATZ").GetAwaiter();
							if (!taskAwaiter.IsCompleted)
							{
								num2 = 0;
								TaskAwaiter taskAwaiter2 = taskAwaiter;
								this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, OBDDataReader.<SendATZ>d__176>(ref taskAwaiter, ref this);
								return;
							}
						}
						else
						{
							taskAwaiter = obddataReader.SendString("ATZ\rATE0\r\r\r\r\r\r\r\r\r\r").GetAwaiter();
							if (!taskAwaiter.IsCompleted)
							{
								num2 = 1;
								TaskAwaiter taskAwaiter2 = taskAwaiter;
								this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, OBDDataReader.<SendATZ>d__176>(ref taskAwaiter, ref this);
								return;
							}
							goto IL_0102;
						}
						break;
					}
					taskAwaiter.GetResult();
					obddataReader.ELMStatus.Reset();
					goto IL_0124;
					IL_0102:
					taskAwaiter.GetResult();
					obddataReader.ELMStatus.Reset();
					obddataReader.ELMStatus.UpdateStatusFromCommand("ATE0");
					IL_0124:
					if (SharedSettings.Current.NoDelayELM327Init)
					{
						goto IL_018F;
					}
					taskAwaiter = Task.Delay(400).GetAwaiter();
					if (!taskAwaiter.IsCompleted)
					{
						num2 = 2;
						TaskAwaiter taskAwaiter2 = taskAwaiter;
						this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, OBDDataReader.<SendATZ>d__176>(ref taskAwaiter, ref this);
						return;
					}
					IL_0188:
					taskAwaiter.GetResult();
					IL_018F:
					taskAwaiter3 = obddataReader.ReadData(6000, null, -1).GetAwaiter();
					if (!taskAwaiter3.IsCompleted)
					{
						num2 = 3;
						TaskAwaiter<string> taskAwaiter4 = taskAwaiter3;
						this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<string>, OBDDataReader.<SendATZ>d__176>(ref taskAwaiter3, ref this);
						return;
					}
					IL_01ED:
					string result = taskAwaiter3.GetResult();
					reply = result;
					if (!SharedSettings.Current.ShowExperimental)
					{
						goto IL_036F;
					}
					valueTaskAwaiter = obddataReader.Connection.ReadBytesAsync().GetAwaiter();
					if (!valueTaskAwaiter.IsCompleted)
					{
						num2 = 4;
						ValueTaskAwaiter<byte[]> valueTaskAwaiter2 = valueTaskAwaiter;
						this.<>t__builder.AwaitUnsafeOnCompleted<ValueTaskAwaiter<byte[]>, OBDDataReader.<SendATZ>d__176>(ref valueTaskAwaiter, ref this);
						return;
					}
					IL_026D:
					byte[] result2 = valueTaskAwaiter.GetResult();
					discardBuffer = result2;
					if (discardBuffer == null || discardBuffer.Length == 0)
					{
						goto IL_0368;
					}
					taskAwaiter = obddataReader.DebugWrite("[DiscardBuffer=" + BitHelpers.ByteArrayToHexString(discardBuffer) + "]").GetAwaiter();
					if (!taskAwaiter.IsCompleted)
					{
						num2 = 5;
						TaskAwaiter taskAwaiter2 = taskAwaiter;
						this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, OBDDataReader.<SendATZ>d__176>(ref taskAwaiter, ref this);
						return;
					}
					IL_0303:
					taskAwaiter.GetResult();
					taskAwaiter = obddataReader.DebugWrite(discardBuffer).GetAwaiter();
					if (!taskAwaiter.IsCompleted)
					{
						num2 = 6;
						TaskAwaiter taskAwaiter2 = taskAwaiter;
						this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, OBDDataReader.<SendATZ>d__176>(ref taskAwaiter, ref this);
						return;
					}
					IL_0361:
					taskAwaiter.GetResult();
					IL_0368:
					discardBuffer = null;
					IL_036F:
					text = reply;
				}
				catch (Exception ex)
				{
					num2 = -2;
					reply = null;
					this.<>t__builder.SetException(ex);
					return;
				}
				num2 = -2;
				reply = null;
				this.<>t__builder.SetResult(text);
			}

			// Token: 0x0600262D RID: 9773 RVA: 0x001D5BE0 File Offset: 0x001D3DE0
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x0400145A RID: 5210
			public int <>1__state;

			// Token: 0x0400145B RID: 5211
			public AsyncTaskMethodBuilder<string> <>t__builder;

			// Token: 0x0400145C RID: 5212
			public OBDDataReader <>4__this;

			// Token: 0x0400145D RID: 5213
			private string <reply>5__2;

			// Token: 0x0400145E RID: 5214
			private TaskAwaiter <>u__1;

			// Token: 0x0400145F RID: 5215
			private TaskAwaiter<string> <>u__2;

			// Token: 0x04001460 RID: 5216
			private byte[] <discardBuffer>5__3;

			// Token: 0x04001461 RID: 5217
			private ValueTaskAwaiter<byte[]> <>u__3;
		}

		// Token: 0x0200037E RID: 894
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <SendBeforeOrAfterCommands>d__150 : IAsyncStateMachine
		{
			// Token: 0x0600262E RID: 9774 RVA: 0x001D5BF0 File Offset: 0x001D3DF0
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				OBDDataReader obddataReader = this;
				try
				{
					TaskAwaiter<bool> taskAwaiter;
					TaskAwaiter taskAwaiter3;
					TaskAwaiter<string> taskAwaiter5;
					switch (num)
					{
					case 0:
					{
						TaskAwaiter<bool> taskAwaiter2;
						taskAwaiter = taskAwaiter2;
						taskAwaiter2 = default(TaskAwaiter<bool>);
						num2 = -1;
						break;
					}
					case 1:
					{
						TaskAwaiter<bool> taskAwaiter2;
						taskAwaiter = taskAwaiter2;
						taskAwaiter2 = default(TaskAwaiter<bool>);
						num2 = -1;
						goto IL_01A7;
					}
					case 2:
					{
						TaskAwaiter taskAwaiter4;
						taskAwaiter3 = taskAwaiter4;
						taskAwaiter4 = default(TaskAwaiter);
						num2 = -1;
						goto IL_028E;
					}
					case 3:
					{
						TaskAwaiter taskAwaiter4;
						taskAwaiter3 = taskAwaiter4;
						taskAwaiter4 = default(TaskAwaiter);
						num2 = -1;
						goto IL_031C;
					}
					case 4:
					{
						TaskAwaiter taskAwaiter4;
						taskAwaiter3 = taskAwaiter4;
						taskAwaiter4 = default(TaskAwaiter);
						num2 = -1;
						goto IL_03C9;
					}
					case 5:
					{
						TaskAwaiter taskAwaiter4;
						taskAwaiter3 = taskAwaiter4;
						taskAwaiter4 = default(TaskAwaiter);
						num2 = -1;
						goto IL_04D1;
					}
					case 6:
					{
						TaskAwaiter<string> taskAwaiter6;
						taskAwaiter5 = taskAwaiter6;
						taskAwaiter6 = default(TaskAwaiter<string>);
						num2 = -1;
						goto IL_0536;
					}
					case 7:
					{
						TaskAwaiter taskAwaiter4;
						taskAwaiter3 = taskAwaiter4;
						taskAwaiter4 = default(TaskAwaiter);
						num2 = -1;
						goto IL_05D7;
					}
					case 8:
					{
						TaskAwaiter taskAwaiter4;
						taskAwaiter3 = taskAwaiter4;
						taskAwaiter4 = default(TaskAwaiter);
						num2 = -1;
						goto IL_07C5;
					}
					case 9:
					{
						TaskAwaiter taskAwaiter4;
						taskAwaiter3 = taskAwaiter4;
						taskAwaiter4 = default(TaskAwaiter);
						num2 = -1;
						goto IL_089E;
					}
					case 10:
					{
						TaskAwaiter taskAwaiter4;
						taskAwaiter3 = taskAwaiter4;
						taskAwaiter4 = default(TaskAwaiter);
						num2 = -1;
						goto IL_0908;
					}
					case 11:
					{
						TaskAwaiter<string> taskAwaiter6;
						taskAwaiter5 = taskAwaiter6;
						taskAwaiter6 = default(TaskAwaiter<string>);
						num2 = -1;
						goto IL_0984;
					}
					case 12:
					{
						TaskAwaiter taskAwaiter4;
						taskAwaiter3 = taskAwaiter4;
						taskAwaiter4 = default(TaskAwaiter);
						num2 = -1;
						goto IL_09FD;
					}
					case 13:
					{
						TaskAwaiter taskAwaiter4;
						taskAwaiter3 = taskAwaiter4;
						taskAwaiter4 = default(TaskAwaiter);
						num2 = -1;
						goto IL_0AAD;
					}
					case 14:
					{
						TaskAwaiter taskAwaiter4;
						taskAwaiter3 = taskAwaiter4;
						taskAwaiter4 = default(TaskAwaiter);
						num2 = -1;
						goto IL_0B6A;
					}
					case 15:
					{
						TaskAwaiter taskAwaiter4;
						taskAwaiter3 = taskAwaiter4;
						taskAwaiter4 = default(TaskAwaiter);
						num2 = -1;
						goto IL_0C64;
					}
					case 16:
					{
						TaskAwaiter taskAwaiter4;
						taskAwaiter3 = taskAwaiter4;
						taskAwaiter4 = default(TaskAwaiter);
						num2 = -1;
						goto IL_0CCE;
					}
					case 17:
					{
						TaskAwaiter taskAwaiter4;
						taskAwaiter3 = taskAwaiter4;
						taskAwaiter4 = default(TaskAwaiter);
						num2 = -1;
						goto IL_0D98;
					}
					case 18:
					{
						TaskAwaiter taskAwaiter4;
						taskAwaiter3 = taskAwaiter4;
						taskAwaiter4 = default(TaskAwaiter);
						num2 = -1;
						goto IL_0E5C;
					}
					case 19:
					{
						TaskAwaiter taskAwaiter4;
						taskAwaiter3 = taskAwaiter4;
						taskAwaiter4 = default(TaskAwaiter);
						num2 = -1;
						goto IL_0F0B;
					}
					case 20:
					{
						TaskAwaiter taskAwaiter4;
						taskAwaiter3 = taskAwaiter4;
						taskAwaiter4 = default(TaskAwaiter);
						num2 = -1;
						goto IL_1004;
					}
					case 21:
					{
						TaskAwaiter taskAwaiter4;
						taskAwaiter3 = taskAwaiter4;
						taskAwaiter4 = default(TaskAwaiter);
						num2 = -1;
						goto IL_10D4;
					}
					case 22:
					{
						TaskAwaiter taskAwaiter4;
						taskAwaiter3 = taskAwaiter4;
						taskAwaiter4 = default(TaskAwaiter);
						num2 = -1;
						goto IL_113B;
					}
					case 23:
					{
						TaskAwaiter<string> taskAwaiter6;
						taskAwaiter5 = taskAwaiter6;
						taskAwaiter6 = default(TaskAwaiter<string>);
						num2 = -1;
						goto IL_11A1;
					}
					default:
						if (commandsToSend == null)
						{
							goto IL_11F3;
						}
						atOptimization = SharedSettings.Current.ATCommandStateOptimization;
						array = commandsToSend;
						i = 0;
						goto IL_11BE;
					}
					IL_013A:
					taskAwaiter.GetResult();
					goto IL_11B0;
					IL_01A7:
					taskAwaiter.GetResult();
					goto IL_11B0;
					IL_028E:
					taskAwaiter3.GetResult();
					goto IL_1142;
					IL_031C:
					taskAwaiter3.GetResult();
					goto IL_1142;
					IL_03C9:
					taskAwaiter3.GetResult();
					goto IL_1142;
					IL_04D1:
					taskAwaiter3.GetResult();
					taskAwaiter5 = obddataReader.ReadData(2500, null, -1).GetAwaiter();
					if (!taskAwaiter5.IsCompleted)
					{
						num2 = 6;
						TaskAwaiter<string> taskAwaiter6 = taskAwaiter5;
						this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<string>, OBDDataReader.<SendBeforeOrAfterCommands>d__150>(ref taskAwaiter5, ref this);
						return;
					}
					IL_0536:
					taskAwaiter5.GetResult();
					IL_053E:
					if (obddataReader.ELMStatus.HasSTFlowControlPair(fcheader, rcvheader) || obddataReader.VTCommandsStupported)
					{
						goto IL_11B0;
					}
					taskAwaiter3 = obddataReader.SendString("STCFCPA" + fcheader + "," + rcvheader).GetAwaiter();
					if (!taskAwaiter3.IsCompleted)
					{
						num2 = 7;
						TaskAwaiter taskAwaiter4 = taskAwaiter3;
						this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, OBDDataReader.<SendBeforeOrAfterCommands>d__150>(ref taskAwaiter3, ref this);
						return;
					}
					IL_05D7:
					taskAwaiter3.GetResult();
					fcheader = null;
					rcvheader = null;
					goto IL_1142;
					IL_07C5:
					taskAwaiter3.GetResult();
					goto IL_1142;
					IL_089E:
					taskAwaiter3.GetResult();
					goto IL_1142;
					IL_0908:
					taskAwaiter3.GetResult();
					if (!obddataReader.STCommandsStupported || !obddataReader.VTCommandsStupported)
					{
						goto IL_1142;
					}
					taskAwaiter5 = obddataReader.ReadData(2500, null, -1).GetAwaiter();
					if (!taskAwaiter5.IsCompleted)
					{
						num2 = 11;
						TaskAwaiter<string> taskAwaiter6 = taskAwaiter5;
						this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<string>, OBDDataReader.<SendBeforeOrAfterCommands>d__150>(ref taskAwaiter5, ref this);
						return;
					}
					IL_0984:
					taskAwaiter5.GetResult();
					string text = fcmd.Replace("ATTA", "ATCER");
					taskAwaiter3 = obddataReader.SendString(text).GetAwaiter();
					if (!taskAwaiter3.IsCompleted)
					{
						num2 = 12;
						TaskAwaiter taskAwaiter4 = taskAwaiter3;
						this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, OBDDataReader.<SendBeforeOrAfterCommands>d__150>(ref taskAwaiter3, ref this);
						return;
					}
					IL_09FD:
					taskAwaiter3.GetResult();
					goto IL_1142;
					IL_0AAD:
					taskAwaiter3.GetResult();
					goto IL_1142;
					IL_0B6A:
					taskAwaiter3.GetResult();
					goto IL_1142;
					IL_0C64:
					taskAwaiter3.GetResult();
					goto IL_1142;
					IL_0CCE:
					taskAwaiter3.GetResult();
					goto IL_1142;
					IL_0D98:
					taskAwaiter3.GetResult();
					goto IL_1142;
					IL_0E5C:
					taskAwaiter3.GetResult();
					goto IL_1142;
					IL_0F0B:
					taskAwaiter3.GetResult();
					goto IL_1142;
					IL_1004:
					taskAwaiter3.GetResult();
					goto IL_1142;
					IL_10D4:
					taskAwaiter3.GetResult();
					goto IL_1142;
					IL_113B:
					taskAwaiter3.GetResult();
					IL_1142:
					taskAwaiter5 = obddataReader.ReadData(2500, null, -1).GetAwaiter();
					if (!taskAwaiter5.IsCompleted)
					{
						num2 = 23;
						TaskAwaiter<string> taskAwaiter6 = taskAwaiter5;
						this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<string>, OBDDataReader.<SendBeforeOrAfterCommands>d__150>(ref taskAwaiter5, ref this);
						return;
					}
					IL_11A1:
					taskAwaiter5.GetResult();
					fcmd = null;
					IL_11B0:
					i++;
					IL_11BE:
					if (i >= array.Length)
					{
						array = null;
					}
					else
					{
						string text2 = array[i];
						if (text2 == null)
						{
							goto IL_11B0;
						}
						if (text2 == "REINIT")
						{
							if (SharedSettings.Current.UseDefaultInit)
							{
								taskAwaiter = obddataReader.InitializeDefaultInitString(SharedSettings.Current.ProtocolNumber, OBDDataReader.InitModes.RestoreConnection, false, true).GetAwaiter();
								if (!taskAwaiter.IsCompleted)
								{
									num2 = 0;
									TaskAwaiter<bool> taskAwaiter2 = taskAwaiter;
									this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<bool>, OBDDataReader.<SendBeforeOrAfterCommands>d__150>(ref taskAwaiter, ref this);
									return;
								}
								goto IL_013A;
							}
							else
							{
								taskAwaiter = obddataReader.InitializeCustomInitString(SharedSettings.Current.CustomInitString, OBDDataReader.InitModes.RestoreConnection, true).GetAwaiter();
								if (!taskAwaiter.IsCompleted)
								{
									num2 = 1;
									TaskAwaiter<bool> taskAwaiter2 = taskAwaiter;
									this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<bool>, OBDDataReader.<SendBeforeOrAfterCommands>d__150>(ref taskAwaiter, ref this);
									return;
								}
								goto IL_01A7;
							}
						}
						else
						{
							fcmd = text2;
							if (fcmd.IndexOf(' ') >= 0)
							{
								fcmd = fcmd.Replace(" ", "");
							}
							if (SharedSettings.Current.ExpectedResponseCountOptimization && ((fcmd.StartsWith("10") && fcmd.Length == 4) || fcmd == "20"))
							{
								taskAwaiter3 = obddataReader.SendString(fcmd + "1").GetAwaiter();
								if (!taskAwaiter3.IsCompleted)
								{
									num2 = 2;
									TaskAwaiter taskAwaiter4 = taskAwaiter3;
									this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, OBDDataReader.<SendBeforeOrAfterCommands>d__150>(ref taskAwaiter3, ref this);
									return;
								}
								goto IL_028E;
							}
							else if (obddataReader.STCommandsStupported && !obddataReader.VTCommandsStupported && "ATCEA".Equals(fcmd, StringComparison.OrdinalIgnoreCase))
							{
								taskAwaiter3 = obddataReader.SendString("STCAF0").GetAwaiter();
								if (!taskAwaiter3.IsCompleted)
								{
									num2 = 3;
									TaskAwaiter taskAwaiter4 = taskAwaiter3;
									this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, OBDDataReader.<SendBeforeOrAfterCommands>d__150>(ref taskAwaiter3, ref this);
									return;
								}
								goto IL_031C;
							}
							else if (obddataReader.STCommandsStupported && !obddataReader.VTCommandsStupported && fcmd.StartsWith("ATCEA", StringComparison.OrdinalIgnoreCase))
							{
								string text3 = fcmd.Substring(5);
								string text4 = "STCAF1," + text3;
								taskAwaiter3 = obddataReader.SendString(text4).GetAwaiter();
								if (!taskAwaiter3.IsCompleted)
								{
									num2 = 4;
									TaskAwaiter taskAwaiter4 = taskAwaiter3;
									this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, OBDDataReader.<SendBeforeOrAfterCommands>d__150>(ref taskAwaiter3, ref this);
									return;
								}
								goto IL_03C9;
							}
							else if (obddataReader.STCommandsStupported && fcmd.StartsWith("ATFCSH", StringComparison.OrdinalIgnoreCase))
							{
								fcheader = "";
								rcvheader = "";
								request.Keys.TryGetValue("ST_FC_REQ", out fcheader);
								request.Keys.TryGetValue("ST_FC_RES", out rcvheader);
								if (fcheader != null)
								{
									string text5 = rcvheader;
								}
								if (!(obddataReader.ELMStatus.FlowControlHeader != fcmd.Substring(6)))
								{
									goto IL_053E;
								}
								taskAwaiter3 = obddataReader.SendString(fcmd).GetAwaiter();
								if (!taskAwaiter3.IsCompleted)
								{
									num2 = 5;
									TaskAwaiter taskAwaiter4 = taskAwaiter3;
									this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, OBDDataReader.<SendBeforeOrAfterCommands>d__150>(ref taskAwaiter3, ref this);
									return;
								}
								goto IL_04D1;
							}
							else
							{
								if ((atOptimization && fcmd == "ATCFC1" && obddataReader.ELMStatus.CANFlowControl_CFC) || (atOptimization && fcmd == "ATCFC0" && !obddataReader.ELMStatus.CANFlowControl_CFC) || (atOptimization && fcmd == "ATCAF1" && obddataReader.ELMStatus.CANAutoFormat_CAF) || (atOptimization && fcmd == "ATCAF0" && !obddataReader.ELMStatus.CANAutoFormat_CAF) || (atOptimization && fcmd == "ATFCSM0" && obddataReader.ELMStatus.FlowControlMode == ELMState.FlowControlModes.Auto_0) || (atOptimization && fcmd == "ATFCSM1" && obddataReader.ELMStatus.FlowControlMode == ELMState.FlowControlModes.UserDefinedFull_1) || (atOptimization && fcmd == "ATFCSM2" && obddataReader.ELMStatus.FlowControlMode == ELMState.FlowControlModes.UserDefinedData_2))
								{
									goto IL_11B0;
								}
								if (atOptimization && fcmd == "ATFCSM0")
								{
									if (!isBeforeCommands && request.BeforeCommands != null && request.BeforeCommands.Contains("ATFCSM1"))
									{
										goto IL_11B0;
									}
									taskAwaiter3 = obddataReader.SendString(fcmd).GetAwaiter();
									if (!taskAwaiter3.IsCompleted)
									{
										num2 = 8;
										TaskAwaiter taskAwaiter4 = taskAwaiter3;
										this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, OBDDataReader.<SendBeforeOrAfterCommands>d__150>(ref taskAwaiter3, ref this);
										return;
									}
									goto IL_07C5;
								}
								else if (fcmd.StartsWith("ATTA", StringComparison.OrdinalIgnoreCase) && fcmd.Length == 6)
								{
									if (atOptimization && fcmd.Substring(4) == obddataReader.ELMStatus.TesterAddress)
									{
										goto IL_11B0;
									}
									if (SharedSettings.Current.ReplaceATTAWithATCER)
									{
										string text6 = fcmd.Replace("ATTA", "ATCER");
										taskAwaiter3 = obddataReader.SendString(text6).GetAwaiter();
										if (!taskAwaiter3.IsCompleted)
										{
											num2 = 9;
											TaskAwaiter taskAwaiter4 = taskAwaiter3;
											this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, OBDDataReader.<SendBeforeOrAfterCommands>d__150>(ref taskAwaiter3, ref this);
											return;
										}
										goto IL_089E;
									}
									else
									{
										taskAwaiter3 = obddataReader.SendString(fcmd).GetAwaiter();
										if (!taskAwaiter3.IsCompleted)
										{
											num2 = 10;
											TaskAwaiter taskAwaiter4 = taskAwaiter3;
											this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, OBDDataReader.<SendBeforeOrAfterCommands>d__150>(ref taskAwaiter3, ref this);
											return;
										}
										goto IL_0908;
									}
								}
								else if (atOptimization && fcmd.StartsWith("ATFCSH", StringComparison.OrdinalIgnoreCase))
								{
									string text7 = fcmd.Substring(6);
									if (obddataReader.ELMStatus.FlowControlHeader == text7)
									{
										goto IL_11B0;
									}
									taskAwaiter3 = obddataReader.SendString(fcmd).GetAwaiter();
									if (!taskAwaiter3.IsCompleted)
									{
										num2 = 13;
										TaskAwaiter taskAwaiter4 = taskAwaiter3;
										this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, OBDDataReader.<SendBeforeOrAfterCommands>d__150>(ref taskAwaiter3, ref this);
										return;
									}
									goto IL_0AAD;
								}
								else if (atOptimization && fcmd.StartsWith("ATFCSD", StringComparison.OrdinalIgnoreCase) && fcmd.Length > 6)
								{
									if (fcmd.Substring(6) == obddataReader.ELMStatus.FlowControlData)
									{
										goto IL_11B0;
									}
									taskAwaiter3 = obddataReader.SendString(fcmd).GetAwaiter();
									if (!taskAwaiter3.IsCompleted)
									{
										num2 = 14;
										TaskAwaiter taskAwaiter4 = taskAwaiter3;
										this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, OBDDataReader.<SendBeforeOrAfterCommands>d__150>(ref taskAwaiter3, ref this);
										return;
									}
									goto IL_0B6A;
								}
								else if (atOptimization && fcmd.StartsWith("ATCRA"))
								{
									string text8 = fcmd.Substring(5);
									if (obddataReader.ELMStatus.ATCRA == text8 || (SharedSettings.Current.ATCRAOptimization && (obddataReader.ELMStatus.ATCM == "000" || obddataReader.ELMStatus.ATCM == "00000000")))
									{
										goto IL_11B0;
									}
									if (SharedSettings.Current.ATCRAOptimization)
									{
										taskAwaiter3 = obddataReader.SendString("ATCM000").GetAwaiter();
										if (!taskAwaiter3.IsCompleted)
										{
											num2 = 15;
											TaskAwaiter taskAwaiter4 = taskAwaiter3;
											this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, OBDDataReader.<SendBeforeOrAfterCommands>d__150>(ref taskAwaiter3, ref this);
											return;
										}
										goto IL_0C64;
									}
									else
									{
										taskAwaiter3 = obddataReader.SendString(fcmd).GetAwaiter();
										if (!taskAwaiter3.IsCompleted)
										{
											num2 = 16;
											TaskAwaiter taskAwaiter4 = taskAwaiter3;
											this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, OBDDataReader.<SendBeforeOrAfterCommands>d__150>(ref taskAwaiter3, ref this);
											return;
										}
										goto IL_0CCE;
									}
								}
								else if (atOptimization && fcmd == "ATAR")
								{
									if (SharedSettings.Current.ATCRAOptimization && (obddataReader.ELMStatus.ATCM == "000" || obddataReader.ELMStatus.ATCM == "00000000"))
									{
										goto IL_11B0;
									}
									taskAwaiter3 = obddataReader.SendString(fcmd).GetAwaiter();
									if (!taskAwaiter3.IsCompleted)
									{
										num2 = 17;
										TaskAwaiter taskAwaiter4 = taskAwaiter3;
										this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, OBDDataReader.<SendBeforeOrAfterCommands>d__150>(ref taskAwaiter3, ref this);
										return;
									}
									goto IL_0D98;
								}
								else if (atOptimization && fcmd != "ATCEA" && fcmd.StartsWith("ATCEA"))
								{
									string text9 = fcmd.Substring(5);
									if (obddataReader.ELMStatus.ATCEA == text9)
									{
										goto IL_11B0;
									}
									taskAwaiter3 = obddataReader.SendString(fcmd).GetAwaiter();
									if (!taskAwaiter3.IsCompleted)
									{
										num2 = 18;
										TaskAwaiter taskAwaiter4 = taskAwaiter3;
										this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, OBDDataReader.<SendBeforeOrAfterCommands>d__150>(ref taskAwaiter3, ref this);
										return;
									}
									goto IL_0E5C;
								}
								else if (atOptimization && fcmd.StartsWith("ATCP"))
								{
									string text10 = fcmd.Substring(4);
									if (obddataReader.ELMStatus.CAN29bitPriority == text10)
									{
										goto IL_11B0;
									}
									taskAwaiter3 = obddataReader.SendString(fcmd).GetAwaiter();
									if (!taskAwaiter3.IsCompleted)
									{
										num2 = 19;
										TaskAwaiter taskAwaiter4 = taskAwaiter3;
										this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, OBDDataReader.<SendBeforeOrAfterCommands>d__150>(ref taskAwaiter3, ref this);
										return;
									}
									goto IL_0F0B;
								}
								else if (atOptimization && fcmd.StartsWith("ATSP") && fcmd.Length == 5 && fcmd != "ATSP0" && !fcmd.StartsWith("ATSPA"))
								{
									int num3;
									if (!int.TryParse(fcmd.Substring(4), NumberStyles.HexNumber, CultureInfo.InvariantCulture, out num3))
									{
										goto IL_1142;
									}
									if (num3 == obddataReader.ELMStatus.Protocol)
									{
										goto IL_11B0;
									}
									taskAwaiter3 = obddataReader.SendString(fcmd).GetAwaiter();
									if (!taskAwaiter3.IsCompleted)
									{
										num2 = 20;
										TaskAwaiter taskAwaiter4 = taskAwaiter3;
										this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, OBDDataReader.<SendBeforeOrAfterCommands>d__150>(ref taskAwaiter3, ref this);
										return;
									}
									goto IL_1004;
								}
								else if (atOptimization && fcmd.StartsWith("STP") && fcmd.Length == 5)
								{
									int num4;
									if (!int.TryParse(fcmd.Substring(3, 2), NumberStyles.HexNumber, CultureInfo.InvariantCulture, out num4))
									{
										goto IL_1142;
									}
									if (num4 == obddataReader.ELMStatus.Protocol)
									{
										goto IL_11B0;
									}
									taskAwaiter3 = obddataReader.SendString(fcmd).GetAwaiter();
									if (!taskAwaiter3.IsCompleted)
									{
										num2 = 21;
										TaskAwaiter taskAwaiter4 = taskAwaiter3;
										this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, OBDDataReader.<SendBeforeOrAfterCommands>d__150>(ref taskAwaiter3, ref this);
										return;
									}
									goto IL_10D4;
								}
								else
								{
									taskAwaiter3 = obddataReader.SendString(fcmd).GetAwaiter();
									if (!taskAwaiter3.IsCompleted)
									{
										num2 = 22;
										TaskAwaiter taskAwaiter4 = taskAwaiter3;
										this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, OBDDataReader.<SendBeforeOrAfterCommands>d__150>(ref taskAwaiter3, ref this);
										return;
									}
									goto IL_113B;
								}
							}
						}
					}
				}
				catch (Exception ex)
				{
					num2 = -2;
					this.<>t__builder.SetException(ex);
					return;
				}
				IL_11F3:
				num2 = -2;
				this.<>t__builder.SetResult();
			}

			// Token: 0x0600262F RID: 9775 RVA: 0x001D6E20 File Offset: 0x001D5020
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x04001462 RID: 5218
			public int <>1__state;

			// Token: 0x04001463 RID: 5219
			public AsyncValueTaskMethodBuilder <>t__builder;

			// Token: 0x04001464 RID: 5220
			public string[] commandsToSend;

			// Token: 0x04001465 RID: 5221
			public OBDDataReader <>4__this;

			// Token: 0x04001466 RID: 5222
			public OBDRequest request;

			// Token: 0x04001467 RID: 5223
			public bool isBeforeCommands;

			// Token: 0x04001468 RID: 5224
			private bool <atOptimization>5__2;

			// Token: 0x04001469 RID: 5225
			private string[] <>7__wrap2;

			// Token: 0x0400146A RID: 5226
			private int <>7__wrap3;

			// Token: 0x0400146B RID: 5227
			private TaskAwaiter<bool> <>u__1;

			// Token: 0x0400146C RID: 5228
			private string <fcmd>5__5;

			// Token: 0x0400146D RID: 5229
			private TaskAwaiter <>u__2;

			// Token: 0x0400146E RID: 5230
			private string <fcheader>5__6;

			// Token: 0x0400146F RID: 5231
			private string <rcvheader>5__7;

			// Token: 0x04001470 RID: 5232
			private TaskAwaiter<string> <>u__3;
		}

		// Token: 0x0200037F RID: 895
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <SendInitCommand>d__174 : IAsyncStateMachine
		{
			// Token: 0x06002630 RID: 9776 RVA: 0x001D6E30 File Offset: 0x001D5030
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				OBDDataReader obddataReader = this;
				string text;
				try
				{
					TaskAwaiter taskAwaiter;
					TaskAwaiter<string> taskAwaiter3;
					switch (num)
					{
					case 0:
					{
						TaskAwaiter taskAwaiter2;
						taskAwaiter = taskAwaiter2;
						taskAwaiter2 = default(TaskAwaiter);
						num2 = -1;
						break;
					}
					case 1:
					{
						TaskAwaiter taskAwaiter2;
						taskAwaiter = taskAwaiter2;
						taskAwaiter2 = default(TaskAwaiter);
						num2 = -1;
						goto IL_017B;
					}
					case 2:
					{
						TaskAwaiter taskAwaiter2;
						taskAwaiter = taskAwaiter2;
						taskAwaiter2 = default(TaskAwaiter);
						num2 = -1;
						goto IL_01EA;
					}
					case 3:
					{
						TaskAwaiter<string> taskAwaiter4;
						taskAwaiter3 = taskAwaiter4;
						taskAwaiter4 = default(TaskAwaiter<string>);
						num2 = -1;
						goto IL_0260;
					}
					case 4:
					{
						TaskAwaiter<string> taskAwaiter4;
						taskAwaiter3 = taskAwaiter4;
						taskAwaiter4 = default(TaskAwaiter<string>);
						num2 = -1;
						goto IL_02C9;
					}
					case 5:
					{
						TaskAwaiter taskAwaiter2;
						taskAwaiter = taskAwaiter2;
						taskAwaiter2 = default(TaskAwaiter);
						num2 = -1;
						goto IL_0347;
					}
					case 6:
					{
						TaskAwaiter taskAwaiter2;
						taskAwaiter = taskAwaiter2;
						taskAwaiter2 = default(TaskAwaiter);
						num2 = -1;
						goto IL_03A9;
					}
					case 7:
					{
						TaskAwaiter<string> taskAwaiter4;
						taskAwaiter3 = taskAwaiter4;
						taskAwaiter4 = default(TaskAwaiter<string>);
						num2 = -1;
						goto IL_0418;
					}
					case 8:
					{
						TaskAwaiter<string> taskAwaiter4;
						taskAwaiter3 = taskAwaiter4;
						taskAwaiter4 = default(TaskAwaiter<string>);
						num2 = -1;
						goto IL_047E;
					}
					default:
						if (string.IsNullOrEmpty(command))
						{
							text = "";
							goto IL_04B3;
						}
						command = command.ToUpperInvariant();
						if (command.StartsWith("DELAY"))
						{
							string text2 = command.Substring(5);
							int num3 = 0;
							if (!int.TryParse(text2, out num3))
							{
								goto IL_00EB;
							}
							taskAwaiter = Task.Delay(num3).GetAwaiter();
							if (!taskAwaiter.IsCompleted)
							{
								num2 = 0;
								TaskAwaiter taskAwaiter2 = taskAwaiter;
								this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, OBDDataReader.<SendInitCommand>d__174>(ref taskAwaiter, ref this);
								return;
							}
						}
						else if (command.StartsWith("WAIT"))
						{
							string text3 = command.Substring(4);
							int num4 = 0;
							if (!int.TryParse(text3, out num4))
							{
								goto IL_0182;
							}
							taskAwaiter = Task.Delay(num4).GetAwaiter();
							if (!taskAwaiter.IsCompleted)
							{
								num2 = 1;
								TaskAwaiter taskAwaiter2 = taskAwaiter;
								this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, OBDDataReader.<SendInitCommand>d__174>(ref taskAwaiter, ref this);
								return;
							}
							goto IL_017B;
						}
						else
						{
							taskAwaiter = obddataReader.SendString(command).GetAwaiter();
							if (!taskAwaiter.IsCompleted)
							{
								num2 = 2;
								TaskAwaiter taskAwaiter2 = taskAwaiter;
								this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, OBDDataReader.<SendInitCommand>d__174>(ref taskAwaiter, ref this);
								return;
							}
							goto IL_01EA;
						}
						break;
					}
					taskAwaiter.GetResult();
					IL_00EB:
					text = "";
					goto IL_04B3;
					IL_017B:
					taskAwaiter.GetResult();
					IL_0182:
					text = "";
					goto IL_04B3;
					IL_01EA:
					taskAwaiter.GetResult();
					if (customReadTimeout_ms > 0)
					{
						taskAwaiter3 = obddataReader.ReadData(customReadTimeout_ms, null, -1).GetAwaiter();
						if (!taskAwaiter3.IsCompleted)
						{
							num2 = 3;
							TaskAwaiter<string> taskAwaiter4 = taskAwaiter3;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<string>, OBDDataReader.<SendInitCommand>d__174>(ref taskAwaiter3, ref this);
							return;
						}
					}
					else
					{
						taskAwaiter3 = obddataReader.ReadData(2500, null, -1).GetAwaiter();
						if (!taskAwaiter3.IsCompleted)
						{
							num2 = 4;
							TaskAwaiter<string> taskAwaiter4 = taskAwaiter3;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<string>, OBDDataReader.<SendInitCommand>d__174>(ref taskAwaiter3, ref this);
							return;
						}
						goto IL_02C9;
					}
					IL_0260:
					string text4 = taskAwaiter3.GetResult();
					goto IL_02D2;
					IL_02C9:
					text4 = taskAwaiter3.GetResult();
					IL_02D2:
					if (text4 == null || !text4.Contains("STOPPED"))
					{
						goto IL_0487;
					}
					taskAwaiter = obddataReader.SendString(command).GetAwaiter();
					if (!taskAwaiter.IsCompleted)
					{
						num2 = 5;
						TaskAwaiter taskAwaiter2 = taskAwaiter;
						this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, OBDDataReader.<SendInitCommand>d__174>(ref taskAwaiter, ref this);
						return;
					}
					IL_0347:
					taskAwaiter.GetResult();
					taskAwaiter = Task.Delay(200).GetAwaiter();
					if (!taskAwaiter.IsCompleted)
					{
						num2 = 6;
						TaskAwaiter taskAwaiter2 = taskAwaiter;
						this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, OBDDataReader.<SendInitCommand>d__174>(ref taskAwaiter, ref this);
						return;
					}
					IL_03A9:
					taskAwaiter.GetResult();
					if (customReadTimeout_ms > 0)
					{
						taskAwaiter3 = obddataReader.ReadData(customReadTimeout_ms, null, -1).GetAwaiter();
						if (!taskAwaiter3.IsCompleted)
						{
							num2 = 7;
							TaskAwaiter<string> taskAwaiter4 = taskAwaiter3;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<string>, OBDDataReader.<SendInitCommand>d__174>(ref taskAwaiter3, ref this);
							return;
						}
					}
					else
					{
						taskAwaiter3 = obddataReader.ReadData(2500, null, -1).GetAwaiter();
						if (!taskAwaiter3.IsCompleted)
						{
							num2 = 8;
							TaskAwaiter<string> taskAwaiter4 = taskAwaiter3;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<string>, OBDDataReader.<SendInitCommand>d__174>(ref taskAwaiter3, ref this);
							return;
						}
						goto IL_047E;
					}
					IL_0418:
					text4 = taskAwaiter3.GetResult();
					goto IL_0487;
					IL_047E:
					text4 = taskAwaiter3.GetResult();
					IL_0487:
					obddataReader.CheckReplyForBadELM(command, text4);
					text = text4;
				}
				catch (Exception ex)
				{
					num2 = -2;
					this.<>t__builder.SetException(ex);
					return;
				}
				IL_04B3:
				num2 = -2;
				this.<>t__builder.SetResult(text);
			}

			// Token: 0x06002631 RID: 9777 RVA: 0x001D7320 File Offset: 0x001D5520
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x04001471 RID: 5233
			public int <>1__state;

			// Token: 0x04001472 RID: 5234
			public AsyncTaskMethodBuilder<string> <>t__builder;

			// Token: 0x04001473 RID: 5235
			public string command;

			// Token: 0x04001474 RID: 5236
			public OBDDataReader <>4__this;

			// Token: 0x04001475 RID: 5237
			public int customReadTimeout_ms;

			// Token: 0x04001476 RID: 5238
			private TaskAwaiter <>u__1;

			// Token: 0x04001477 RID: 5239
			private TaskAwaiter<string> <>u__2;
		}

		// Token: 0x02000380 RID: 896
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <SendLongCANMultiRequest>d__155 : IAsyncStateMachine
		{
			// Token: 0x06002632 RID: 9778 RVA: 0x001D7330 File Offset: 0x001D5530
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				OBDDataReader obddataReader = this;
				string text;
				try
				{
					TaskAwaiter<string> taskAwaiter;
					TaskAwaiter taskAwaiter3;
					switch (num)
					{
					case 0:
					{
						TaskAwaiter<string> taskAwaiter2;
						taskAwaiter = taskAwaiter2;
						taskAwaiter2 = default(TaskAwaiter<string>);
						num2 = -1;
						break;
					}
					case 1:
					{
						TaskAwaiter taskAwaiter4;
						taskAwaiter3 = taskAwaiter4;
						taskAwaiter4 = default(TaskAwaiter);
						num2 = -1;
						goto IL_0163;
					}
					case 2:
					{
						TaskAwaiter<string> taskAwaiter2;
						taskAwaiter = taskAwaiter2;
						taskAwaiter2 = default(TaskAwaiter<string>);
						num2 = -1;
						goto IL_01C5;
					}
					case 3:
					{
						TaskAwaiter taskAwaiter4;
						taskAwaiter3 = taskAwaiter4;
						taskAwaiter4 = default(TaskAwaiter);
						num2 = -1;
						goto IL_0248;
					}
					case 4:
					{
						TaskAwaiter<string> taskAwaiter2;
						taskAwaiter = taskAwaiter2;
						taskAwaiter2 = default(TaskAwaiter<string>);
						num2 = -1;
						goto IL_02AA;
					}
					case 5:
					{
						TaskAwaiter taskAwaiter4;
						taskAwaiter3 = taskAwaiter4;
						taskAwaiter4 = default(TaskAwaiter);
						num2 = -1;
						goto IL_0319;
					}
					case 6:
					{
						TaskAwaiter taskAwaiter4;
						taskAwaiter3 = taskAwaiter4;
						taskAwaiter4 = default(TaskAwaiter);
						num2 = -1;
						goto IL_0389;
					}
					case 7:
					{
						TaskAwaiter<string> taskAwaiter2;
						taskAwaiter = taskAwaiter2;
						taskAwaiter2 = default(TaskAwaiter<string>);
						num2 = -1;
						goto IL_03FD;
					}
					default:
						if (obddataReader.STCommandsStupported && SharedSettings.Current.CANRequestSegmentationSTNLevel && request.Command.Length < 500)
						{
							taskAwaiter = obddataReader.SendLongCanRequestSTN(request).GetAwaiter();
							if (!taskAwaiter.IsCompleted)
							{
								num2 = 0;
								TaskAwaiter<string> taskAwaiter2 = taskAwaiter;
								this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<string>, OBDDataReader.<SendLongCANMultiRequest>d__155>(ref taskAwaiter, ref this);
								return;
							}
						}
						else
						{
							delay = 0;
							frameIdx = 0;
							data = "";
							elmFormat = request.ELMFormat;
							if (elmFormat == ELMFormat.Unknown)
							{
								elmFormat = obddataReader.CurrentELMFormat;
							}
							taskAwaiter3 = obddataReader.SendString("ATCFC0").GetAwaiter();
							if (!taskAwaiter3.IsCompleted)
							{
								num2 = 1;
								TaskAwaiter taskAwaiter4 = taskAwaiter3;
								this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, OBDDataReader.<SendLongCANMultiRequest>d__155>(ref taskAwaiter3, ref this);
								return;
							}
							goto IL_0163;
						}
						break;
					}
					text = taskAwaiter.GetResult();
					goto IL_04A5;
					IL_0163:
					taskAwaiter3.GetResult();
					taskAwaiter = obddataReader.ReadData(2500, null, -1).GetAwaiter();
					if (!taskAwaiter.IsCompleted)
					{
						num2 = 2;
						TaskAwaiter<string> taskAwaiter2 = taskAwaiter;
						this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<string>, OBDDataReader.<SendLongCANMultiRequest>d__155>(ref taskAwaiter, ref this);
						return;
					}
					IL_01C5:
					taskAwaiter.GetResult();
					goto IL_0464;
					IL_0248:
					taskAwaiter3.GetResult();
					taskAwaiter = obddataReader.ReadData(2500, null, -1).GetAwaiter();
					if (!taskAwaiter.IsCompleted)
					{
						num2 = 4;
						TaskAwaiter<string> taskAwaiter2 = taskAwaiter;
						this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<string>, OBDDataReader.<SendLongCANMultiRequest>d__155>(ref taskAwaiter, ref this);
						return;
					}
					IL_02AA:
					taskAwaiter.GetResult();
					goto IL_0320;
					IL_0319:
					taskAwaiter3.GetResult();
					IL_0320:
					taskAwaiter3 = obddataReader.SendString(request.CanFrames[frameIdx]).GetAwaiter();
					if (!taskAwaiter3.IsCompleted)
					{
						num2 = 6;
						TaskAwaiter taskAwaiter4 = taskAwaiter3;
						this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, OBDDataReader.<SendLongCANMultiRequest>d__155>(ref taskAwaiter3, ref this);
						return;
					}
					IL_0389:
					taskAwaiter3.GetResult();
					int num3 = frameIdx;
					frameIdx = num3 + 1;
					taskAwaiter = obddataReader.ReadData(2500, null, -1).GetAwaiter();
					if (!taskAwaiter.IsCompleted)
					{
						num2 = 7;
						TaskAwaiter<string> taskAwaiter2 = taskAwaiter;
						this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<string>, OBDDataReader.<SendLongCANMultiRequest>d__155>(ref taskAwaiter, ref this);
						return;
					}
					IL_03FD:
					string result = taskAwaiter.GetResult();
					data = result;
					if (obddataReader.DisconnectRequested || !obddataReader.Running)
					{
						text = data;
						goto IL_04A5;
					}
					try
					{
						int num4;
						int num5;
						if (frameIdx < request.CanFrames.Length && obddataReader.ParseFlowControlFrame(data, elmFormat, out num4, out num5))
						{
							delay = num5;
						}
					}
					catch (Exception)
					{
					}
					IL_0464:
					if (frameIdx >= request.CanFrames.Length)
					{
						text = data;
					}
					else if (frameIdx == request.CanFrames.Length - 1)
					{
						taskAwaiter3 = obddataReader.SendString("ATCFC1").GetAwaiter();
						if (!taskAwaiter3.IsCompleted)
						{
							num2 = 3;
							TaskAwaiter taskAwaiter4 = taskAwaiter3;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, OBDDataReader.<SendLongCANMultiRequest>d__155>(ref taskAwaiter3, ref this);
							return;
						}
						goto IL_0248;
					}
					else
					{
						if (delay <= 0)
						{
							goto IL_0320;
						}
						taskAwaiter3 = Task.Delay(delay).GetAwaiter();
						if (!taskAwaiter3.IsCompleted)
						{
							num2 = 5;
							TaskAwaiter taskAwaiter4 = taskAwaiter3;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, OBDDataReader.<SendLongCANMultiRequest>d__155>(ref taskAwaiter3, ref this);
							return;
						}
						goto IL_0319;
					}
				}
				catch (Exception ex)
				{
					num2 = -2;
					data = null;
					this.<>t__builder.SetException(ex);
					return;
				}
				IL_04A5:
				num2 = -2;
				data = null;
				this.<>t__builder.SetResult(text);
			}

			// Token: 0x06002633 RID: 9779 RVA: 0x001D7834 File Offset: 0x001D5A34
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x04001478 RID: 5240
			public int <>1__state;

			// Token: 0x04001479 RID: 5241
			public AsyncTaskMethodBuilder<string> <>t__builder;

			// Token: 0x0400147A RID: 5242
			public OBDDataReader <>4__this;

			// Token: 0x0400147B RID: 5243
			public OBDMultiRequest request;

			// Token: 0x0400147C RID: 5244
			private int <delay>5__2;

			// Token: 0x0400147D RID: 5245
			private int <frameIdx>5__3;

			// Token: 0x0400147E RID: 5246
			private string <data>5__4;

			// Token: 0x0400147F RID: 5247
			private ELMFormat <elmFormat>5__5;

			// Token: 0x04001480 RID: 5248
			private TaskAwaiter<string> <>u__1;

			// Token: 0x04001481 RID: 5249
			private TaskAwaiter <>u__2;
		}

		// Token: 0x02000381 RID: 897
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <SendLongCanRequest>d__153 : IAsyncStateMachine
		{
			// Token: 0x06002634 RID: 9780 RVA: 0x001D7844 File Offset: 0x001D5A44
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				OBDDataReader obddataReader = this;
				string text;
				try
				{
					TaskAwaiter<string> taskAwaiter;
					TaskAwaiter taskAwaiter3;
					switch (num)
					{
					case 0:
					{
						TaskAwaiter<string> taskAwaiter2;
						taskAwaiter = taskAwaiter2;
						taskAwaiter2 = default(TaskAwaiter<string>);
						num2 = -1;
						break;
					}
					case 1:
					{
						TaskAwaiter taskAwaiter4;
						taskAwaiter3 = taskAwaiter4;
						taskAwaiter4 = default(TaskAwaiter);
						num2 = -1;
						goto IL_019F;
					}
					case 2:
					{
						TaskAwaiter<string> taskAwaiter2;
						taskAwaiter = taskAwaiter2;
						taskAwaiter2 = default(TaskAwaiter<string>);
						num2 = -1;
						goto IL_0204;
					}
					case 3:
					{
						TaskAwaiter taskAwaiter4;
						taskAwaiter3 = taskAwaiter4;
						taskAwaiter4 = default(TaskAwaiter);
						num2 = -1;
						goto IL_0268;
					}
					case 4:
					{
						TaskAwaiter<string> taskAwaiter2;
						taskAwaiter = taskAwaiter2;
						taskAwaiter2 = default(TaskAwaiter<string>);
						num2 = -1;
						goto IL_02CD;
					}
					case 5:
					{
						TaskAwaiter taskAwaiter4;
						taskAwaiter3 = taskAwaiter4;
						taskAwaiter4 = default(TaskAwaiter);
						num2 = -1;
						goto IL_0331;
					}
					case 6:
					{
						TaskAwaiter<string> taskAwaiter2;
						taskAwaiter = taskAwaiter2;
						taskAwaiter2 = default(TaskAwaiter<string>);
						num2 = -1;
						goto IL_0396;
					}
					case 7:
					{
						TaskAwaiter taskAwaiter4;
						taskAwaiter3 = taskAwaiter4;
						taskAwaiter4 = default(TaskAwaiter);
						num2 = -1;
						goto IL_040C;
					}
					case 8:
					{
						TaskAwaiter<string> taskAwaiter2;
						taskAwaiter = taskAwaiter2;
						taskAwaiter2 = default(TaskAwaiter<string>);
						num2 = -1;
						goto IL_0471;
					}
					case 9:
					{
						TaskAwaiter taskAwaiter4;
						taskAwaiter3 = taskAwaiter4;
						taskAwaiter4 = default(TaskAwaiter);
						num2 = -1;
						goto IL_064B;
					}
					case 10:
					{
						TaskAwaiter taskAwaiter4;
						taskAwaiter3 = taskAwaiter4;
						taskAwaiter4 = default(TaskAwaiter);
						num2 = -1;
						goto IL_06AF;
					}
					case 11:
					{
						TaskAwaiter taskAwaiter4;
						taskAwaiter3 = taskAwaiter4;
						taskAwaiter4 = default(TaskAwaiter);
						num2 = -1;
						goto IL_0742;
					}
					case 12:
					{
						TaskAwaiter<string> taskAwaiter2;
						taskAwaiter = taskAwaiter2;
						taskAwaiter2 = default(TaskAwaiter<string>);
						num2 = -1;
						goto IL_07A8;
					}
					case 13:
					{
						TaskAwaiter taskAwaiter4;
						taskAwaiter3 = taskAwaiter4;
						taskAwaiter4 = default(TaskAwaiter);
						num2 = -1;
						goto IL_083C;
					}
					case 14:
					{
						TaskAwaiter taskAwaiter4;
						taskAwaiter3 = taskAwaiter4;
						taskAwaiter4 = default(TaskAwaiter);
						num2 = -1;
						goto IL_08DA;
					}
					case 15:
					{
						TaskAwaiter taskAwaiter4;
						taskAwaiter3 = taskAwaiter4;
						taskAwaiter4 = default(TaskAwaiter);
						num2 = -1;
						goto IL_093E;
					}
					case 16:
					{
						TaskAwaiter taskAwaiter4;
						taskAwaiter3 = taskAwaiter4;
						taskAwaiter4 = default(TaskAwaiter);
						num2 = -1;
						goto IL_09D1;
					}
					case 17:
					{
						TaskAwaiter<string> taskAwaiter2;
						taskAwaiter = taskAwaiter2;
						taskAwaiter2 = default(TaskAwaiter<string>);
						num2 = -1;
						goto IL_0A37;
					}
					case 18:
					{
						TaskAwaiter taskAwaiter4;
						taskAwaiter3 = taskAwaiter4;
						taskAwaiter4 = default(TaskAwaiter);
						num2 = -1;
						goto IL_0ACB;
					}
					case 19:
					{
						TaskAwaiter taskAwaiter4;
						taskAwaiter3 = taskAwaiter4;
						taskAwaiter4 = default(TaskAwaiter);
						num2 = -1;
						goto IL_0B40;
					}
					case 20:
					{
						TaskAwaiter taskAwaiter4;
						taskAwaiter3 = taskAwaiter4;
						taskAwaiter4 = default(TaskAwaiter);
						num2 = -1;
						goto IL_0BD3;
					}
					case 21:
					{
						TaskAwaiter taskAwaiter4;
						taskAwaiter3 = taskAwaiter4;
						taskAwaiter4 = default(TaskAwaiter);
						num2 = -1;
						goto IL_0C3F;
					}
					case 22:
					{
						TaskAwaiter taskAwaiter4;
						taskAwaiter3 = taskAwaiter4;
						taskAwaiter4 = default(TaskAwaiter);
						num2 = -1;
						goto IL_0CF0;
					}
					case 23:
					{
						TaskAwaiter<string> taskAwaiter2;
						taskAwaiter = taskAwaiter2;
						taskAwaiter2 = default(TaskAwaiter<string>);
						num2 = -1;
						goto IL_0D56;
					}
					case 24:
					{
						TaskAwaiter taskAwaiter4;
						taskAwaiter3 = taskAwaiter4;
						taskAwaiter4 = default(TaskAwaiter);
						num2 = -1;
						goto IL_0DF3;
					}
					case 25:
					{
						TaskAwaiter taskAwaiter4;
						taskAwaiter3 = taskAwaiter4;
						taskAwaiter4 = default(TaskAwaiter);
						num2 = -1;
						goto IL_0EB5;
					}
					case 26:
					{
						TaskAwaiter<string> taskAwaiter2;
						taskAwaiter = taskAwaiter2;
						taskAwaiter2 = default(TaskAwaiter<string>);
						num2 = -1;
						goto IL_0F1B;
					}
					case 27:
					{
						TaskAwaiter taskAwaiter4;
						taskAwaiter3 = taskAwaiter4;
						taskAwaiter4 = default(TaskAwaiter);
						num2 = -1;
						goto IL_1093;
					}
					case 28:
					{
						TaskAwaiter taskAwaiter4;
						taskAwaiter3 = taskAwaiter4;
						taskAwaiter4 = default(TaskAwaiter);
						num2 = -1;
						goto IL_10F7;
					}
					case 29:
					{
						TaskAwaiter taskAwaiter4;
						taskAwaiter3 = taskAwaiter4;
						taskAwaiter4 = default(TaskAwaiter);
						num2 = -1;
						goto IL_118A;
					}
					case 30:
					{
						TaskAwaiter<string> taskAwaiter2;
						taskAwaiter = taskAwaiter2;
						taskAwaiter2 = default(TaskAwaiter<string>);
						num2 = -1;
						goto IL_11F0;
					}
					case 31:
					{
						TaskAwaiter taskAwaiter4;
						taskAwaiter3 = taskAwaiter4;
						taskAwaiter4 = default(TaskAwaiter);
						num2 = -1;
						goto IL_1284;
					}
					case 32:
					{
						TaskAwaiter taskAwaiter4;
						taskAwaiter3 = taskAwaiter4;
						taskAwaiter4 = default(TaskAwaiter);
						num2 = -1;
						goto IL_1329;
					}
					case 33:
					{
						TaskAwaiter taskAwaiter4;
						taskAwaiter3 = taskAwaiter4;
						taskAwaiter4 = default(TaskAwaiter);
						num2 = -1;
						goto IL_138D;
					}
					case 34:
					{
						TaskAwaiter taskAwaiter4;
						taskAwaiter3 = taskAwaiter4;
						taskAwaiter4 = default(TaskAwaiter);
						num2 = -1;
						goto IL_1420;
					}
					case 35:
					{
						TaskAwaiter<string> taskAwaiter2;
						taskAwaiter = taskAwaiter2;
						taskAwaiter2 = default(TaskAwaiter<string>);
						num2 = -1;
						goto IL_1486;
					}
					case 36:
					{
						TaskAwaiter taskAwaiter4;
						taskAwaiter3 = taskAwaiter4;
						taskAwaiter4 = default(TaskAwaiter);
						num2 = -1;
						goto IL_151A;
					}
					default:
						if (obddataReader.STCommandsStupported && SharedSettings.Current.CANRequestSegmentationSTNLevel && request.Command.Length < 1000)
						{
							taskAwaiter = obddataReader.SendLongCanRequestSTN(request).GetAwaiter();
							if (!taskAwaiter.IsCompleted)
							{
								num2 = 0;
								TaskAwaiter<string> taskAwaiter2 = taskAwaiter;
								this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<string>, OBDDataReader.<SendLongCanRequest>d__153>(ref taskAwaiter, ref this);
								return;
							}
						}
						else
						{
							taskAwaiter3 = obddataReader.SendString("ATCAF0").GetAwaiter();
							if (!taskAwaiter3.IsCompleted)
							{
								num2 = 1;
								TaskAwaiter taskAwaiter4 = taskAwaiter3;
								this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, OBDDataReader.<SendLongCanRequest>d__153>(ref taskAwaiter3, ref this);
								return;
							}
							goto IL_019F;
						}
						break;
					}
					text = taskAwaiter.GetResult();
					goto IL_155F;
					IL_019F:
					taskAwaiter3.GetResult();
					taskAwaiter = obddataReader.ReadData(2500, null, -1).GetAwaiter();
					if (!taskAwaiter.IsCompleted)
					{
						num2 = 2;
						TaskAwaiter<string> taskAwaiter2 = taskAwaiter;
						this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<string>, OBDDataReader.<SendLongCanRequest>d__153>(ref taskAwaiter, ref this);
						return;
					}
					IL_0204:
					taskAwaiter.GetResult();
					taskAwaiter3 = obddataReader.SendString("ATCFC0").GetAwaiter();
					if (!taskAwaiter3.IsCompleted)
					{
						num2 = 3;
						TaskAwaiter taskAwaiter4 = taskAwaiter3;
						this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, OBDDataReader.<SendLongCanRequest>d__153>(ref taskAwaiter3, ref this);
						return;
					}
					IL_0268:
					taskAwaiter3.GetResult();
					taskAwaiter = obddataReader.ReadData(2500, null, -1).GetAwaiter();
					if (!taskAwaiter.IsCompleted)
					{
						num2 = 4;
						TaskAwaiter<string> taskAwaiter2 = taskAwaiter;
						this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<string>, OBDDataReader.<SendLongCanRequest>d__153>(ref taskAwaiter, ref this);
						return;
					}
					IL_02CD:
					taskAwaiter.GetResult();
					taskAwaiter3 = obddataReader.SendString("ATAL").GetAwaiter();
					if (!taskAwaiter3.IsCompleted)
					{
						num2 = 5;
						TaskAwaiter taskAwaiter4 = taskAwaiter3;
						this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, OBDDataReader.<SendLongCanRequest>d__153>(ref taskAwaiter3, ref this);
						return;
					}
					IL_0331:
					taskAwaiter3.GetResult();
					taskAwaiter = obddataReader.ReadData(2500, null, -1).GetAwaiter();
					if (!taskAwaiter.IsCompleted)
					{
						num2 = 6;
						TaskAwaiter<string> taskAwaiter2 = taskAwaiter;
						this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<string>, OBDDataReader.<SendLongCanRequest>d__153>(ref taskAwaiter, ref this);
						return;
					}
					IL_0396:
					taskAwaiter.GetResult();
					atr0 = true;
					if (!atr0)
					{
						goto IL_0479;
					}
					taskAwaiter3 = obddataReader.SendString("ATR0").GetAwaiter();
					if (!taskAwaiter3.IsCompleted)
					{
						num2 = 7;
						TaskAwaiter taskAwaiter4 = taskAwaiter3;
						this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, OBDDataReader.<SendLongCanRequest>d__153>(ref taskAwaiter3, ref this);
						return;
					}
					IL_040C:
					taskAwaiter3.GetResult();
					taskAwaiter = obddataReader.ReadData(2500, null, -1).GetAwaiter();
					if (!taskAwaiter.IsCompleted)
					{
						num2 = 8;
						TaskAwaiter<string> taskAwaiter2 = taskAwaiter;
						this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<string>, OBDDataReader.<SendLongCanRequest>d__153>(ref taskAwaiter, ref this);
						return;
					}
					IL_0471:
					taskAwaiter.GetResult();
					IL_0479:
					bool flag = request.BeforeCommands.Any((string x) => x.Contains("ATCEA") && x != "ATCEA");
					if (request is OBDMultiRequest)
					{
						frames = (request as OBDMultiRequest).CanFrames;
					}
					else
					{
						frames = OBDMultiRequest.PrepareCanFrames(request.Command, flag);
					}
					delay = 0;
					frameIdx = 0;
					data = "";
					sent_atcfc = false;
					tp = new string[0];
					string text2 = "";
					if (request.Keys.TryGetValue("TesterPresent", out text2) && !string.IsNullOrEmpty(text2))
					{
						tp = text2.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
					}
					elmFormat = request.ELMFormat;
					if (elmFormat == ELMFormat.Unknown)
					{
						elmFormat = obddataReader.CurrentELMFormat;
					}
					sendTPPeriod = SharedSettings.Current.SendTesterPresentWhileLongUploadTimeMs;
					sw = new Stopwatch();
					sw.Start();
					goto IL_0FF4;
					IL_064B:
					taskAwaiter3.GetResult();
					taskAwaiter3 = obddataReader.SendString("ATCFC1").GetAwaiter();
					if (!taskAwaiter3.IsCompleted)
					{
						num2 = 10;
						TaskAwaiter taskAwaiter4 = taskAwaiter3;
						this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, OBDDataReader.<SendLongCanRequest>d__153>(ref taskAwaiter3, ref this);
						return;
					}
					IL_06AF:
					taskAwaiter3.GetResult();
					taskAwaiter3 = App.OBDReader.DebugWrite("\n[" + App.OBDReader.stopwatch.Elapsed.TotalSeconds.ToString() + "]\n").GetAwaiter();
					if (!taskAwaiter3.IsCompleted)
					{
						num2 = 11;
						TaskAwaiter taskAwaiter4 = taskAwaiter3;
						this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, OBDDataReader.<SendLongCanRequest>d__153>(ref taskAwaiter3, ref this);
						return;
					}
					IL_0742:
					taskAwaiter3.GetResult();
					taskAwaiter = obddataReader.ReadData(2500, null, -1).GetAwaiter();
					if (!taskAwaiter.IsCompleted)
					{
						num2 = 12;
						TaskAwaiter<string> taskAwaiter2 = taskAwaiter;
						this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<string>, OBDDataReader.<SendLongCanRequest>d__153>(ref taskAwaiter, ref this);
						return;
					}
					IL_07A8:
					taskAwaiter.GetResult();
					taskAwaiter3 = App.OBDReader.DebugWrite("\n[" + App.OBDReader.stopwatch.Elapsed.TotalSeconds.ToString() + "]\n").GetAwaiter();
					if (!taskAwaiter3.IsCompleted)
					{
						num2 = 13;
						TaskAwaiter taskAwaiter4 = taskAwaiter3;
						this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, OBDDataReader.<SendLongCanRequest>d__153>(ref taskAwaiter3, ref this);
						return;
					}
					IL_083C:
					taskAwaiter3.GetResult();
					if (!atr0)
					{
						goto IL_0AD2;
					}
					taskAwaiter3 = App.OBDReader.DebugWrite("\n[" + App.OBDReader.stopwatch.Elapsed.TotalSeconds.ToString() + "]\n").GetAwaiter();
					if (!taskAwaiter3.IsCompleted)
					{
						num2 = 14;
						TaskAwaiter taskAwaiter4 = taskAwaiter3;
						this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, OBDDataReader.<SendLongCanRequest>d__153>(ref taskAwaiter3, ref this);
						return;
					}
					IL_08DA:
					taskAwaiter3.GetResult();
					taskAwaiter3 = obddataReader.SendString("ATR1").GetAwaiter();
					if (!taskAwaiter3.IsCompleted)
					{
						num2 = 15;
						TaskAwaiter taskAwaiter4 = taskAwaiter3;
						this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, OBDDataReader.<SendLongCanRequest>d__153>(ref taskAwaiter3, ref this);
						return;
					}
					IL_093E:
					taskAwaiter3.GetResult();
					taskAwaiter3 = App.OBDReader.DebugWrite("\n[" + App.OBDReader.stopwatch.Elapsed.TotalSeconds.ToString() + "]\n").GetAwaiter();
					if (!taskAwaiter3.IsCompleted)
					{
						num2 = 16;
						TaskAwaiter taskAwaiter4 = taskAwaiter3;
						this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, OBDDataReader.<SendLongCanRequest>d__153>(ref taskAwaiter3, ref this);
						return;
					}
					IL_09D1:
					taskAwaiter3.GetResult();
					taskAwaiter = obddataReader.ReadData(2500, null, -1).GetAwaiter();
					if (!taskAwaiter.IsCompleted)
					{
						num2 = 17;
						TaskAwaiter<string> taskAwaiter2 = taskAwaiter;
						this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<string>, OBDDataReader.<SendLongCanRequest>d__153>(ref taskAwaiter, ref this);
						return;
					}
					IL_0A37:
					taskAwaiter.GetResult();
					taskAwaiter3 = App.OBDReader.DebugWrite("\n[" + App.OBDReader.stopwatch.Elapsed.TotalSeconds.ToString() + "]\n").GetAwaiter();
					if (!taskAwaiter3.IsCompleted)
					{
						num2 = 18;
						TaskAwaiter taskAwaiter4 = taskAwaiter3;
						this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, OBDDataReader.<SendLongCanRequest>d__153>(ref taskAwaiter3, ref this);
						return;
					}
					IL_0ACB:
					taskAwaiter3.GetResult();
					IL_0AD2:
					if (!atr0 || delay <= 0)
					{
						goto IL_0B47;
					}
					taskAwaiter3 = Task.Delay(delay).GetAwaiter();
					if (!taskAwaiter3.IsCompleted)
					{
						num2 = 19;
						TaskAwaiter taskAwaiter4 = taskAwaiter3;
						this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, OBDDataReader.<SendLongCanRequest>d__153>(ref taskAwaiter3, ref this);
						return;
					}
					IL_0B40:
					taskAwaiter3.GetResult();
					IL_0B47:
					taskAwaiter3 = App.OBDReader.DebugWrite("\n[" + App.OBDReader.stopwatch.Elapsed.TotalSeconds.ToString() + "]\n").GetAwaiter();
					if (!taskAwaiter3.IsCompleted)
					{
						num2 = 20;
						TaskAwaiter taskAwaiter4 = taskAwaiter3;
						this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, OBDDataReader.<SendLongCanRequest>d__153>(ref taskAwaiter3, ref this);
						return;
					}
					IL_0BD3:
					taskAwaiter3.GetResult();
					taskAwaiter3 = obddataReader.SendString(frames[frameIdx]).GetAwaiter();
					if (!taskAwaiter3.IsCompleted)
					{
						num2 = 21;
						TaskAwaiter taskAwaiter4 = taskAwaiter3;
						this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, OBDDataReader.<SendLongCanRequest>d__153>(ref taskAwaiter3, ref this);
						return;
					}
					IL_0C3F:
					taskAwaiter3.GetResult();
					if (obddataReader.DisconnectRequested || !obddataReader.Running)
					{
						text = data;
						goto IL_155F;
					}
					taskAwaiter3 = App.OBDReader.DebugWrite("\n[" + App.OBDReader.stopwatch.Elapsed.TotalSeconds.ToString() + "]\n").GetAwaiter();
					if (!taskAwaiter3.IsCompleted)
					{
						num2 = 22;
						TaskAwaiter taskAwaiter4 = taskAwaiter3;
						this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, OBDDataReader.<SendLongCanRequest>d__153>(ref taskAwaiter3, ref this);
						return;
					}
					IL_0CF0:
					taskAwaiter3.GetResult();
					taskAwaiter = obddataReader.ReadData(2500, null, -1).GetAwaiter();
					if (!taskAwaiter.IsCompleted)
					{
						num2 = 23;
						TaskAwaiter<string> taskAwaiter2 = taskAwaiter;
						this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<string>, OBDDataReader.<SendLongCanRequest>d__153>(ref taskAwaiter, ref this);
						return;
					}
					IL_0D56:
					string result = taskAwaiter.GetResult();
					data = result;
					taskAwaiter3 = App.OBDReader.DebugWrite("\n[" + App.OBDReader.stopwatch.Elapsed.TotalSeconds.ToString() + "]\n").GetAwaiter();
					if (!taskAwaiter3.IsCompleted)
					{
						num2 = 24;
						TaskAwaiter taskAwaiter4 = taskAwaiter3;
						this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, OBDDataReader.<SendLongCanRequest>d__153>(ref taskAwaiter3, ref this);
						return;
					}
					IL_0DF3:
					taskAwaiter3.GetResult();
					if (sendTPPeriod > 0 && tp != null && tp.Length != 0 && sw.ElapsedMilliseconds > (long)sendTPPeriod)
					{
						array = tp;
						i = 0;
						goto IL_0F31;
					}
					goto IL_0F56;
					IL_0EB5:
					taskAwaiter3.GetResult();
					taskAwaiter = obddataReader.ReadData(2500, null, -1).GetAwaiter();
					if (!taskAwaiter.IsCompleted)
					{
						num2 = 26;
						TaskAwaiter<string> taskAwaiter2 = taskAwaiter;
						this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<string>, OBDDataReader.<SendLongCanRequest>d__153>(ref taskAwaiter, ref this);
						return;
					}
					IL_0F1B:
					taskAwaiter.GetResult();
					i++;
					IL_0F31:
					if (i >= array.Length)
					{
						array = null;
						sw.Restart();
					}
					else
					{
						string text3 = array[i];
						taskAwaiter3 = obddataReader.SendString(text3).GetAwaiter();
						if (!taskAwaiter3.IsCompleted)
						{
							num2 = 25;
							TaskAwaiter taskAwaiter4 = taskAwaiter3;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, OBDDataReader.<SendLongCanRequest>d__153>(ref taskAwaiter3, ref this);
							return;
						}
						goto IL_0EB5;
					}
					IL_0F56:
					if (obddataReader.DisconnectRequested || !obddataReader.Running)
					{
						text = data;
						goto IL_155F;
					}
					int num3 = frameIdx;
					frameIdx = num3 + 1;
					IProgress<string> progress = request.Progress;
					if (progress != null)
					{
						progress.Report(string.Format("{0}/{1}", frameIdx, frames.Length));
					}
					try
					{
						int num4;
						int num5;
						if (frameIdx < frames.Length && obddataReader.ParseFlowControlFrame(data, elmFormat, out num4, out num5))
						{
							delay = num5;
						}
					}
					catch (Exception)
					{
					}
					IL_0FF4:
					if (frameIdx >= frames.Length)
					{
						taskAwaiter3 = App.OBDReader.DebugWrite("\n[" + App.OBDReader.stopwatch.Elapsed.TotalSeconds.ToString() + "]\n").GetAwaiter();
						if (!taskAwaiter3.IsCompleted)
						{
							num2 = 27;
							TaskAwaiter taskAwaiter4 = taskAwaiter3;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, OBDDataReader.<SendLongCanRequest>d__153>(ref taskAwaiter3, ref this);
							return;
						}
					}
					else
					{
						if (frameIdx != frames.Length - 1)
						{
							goto IL_0AD2;
						}
						sent_atcfc = true;
						taskAwaiter3 = App.OBDReader.DebugWrite("\n[" + App.OBDReader.stopwatch.Elapsed.TotalSeconds.ToString() + "]\n").GetAwaiter();
						if (!taskAwaiter3.IsCompleted)
						{
							num2 = 9;
							TaskAwaiter taskAwaiter4 = taskAwaiter3;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, OBDDataReader.<SendLongCanRequest>d__153>(ref taskAwaiter3, ref this);
							return;
						}
						goto IL_064B;
					}
					IL_1093:
					taskAwaiter3.GetResult();
					taskAwaiter3 = obddataReader.SendString("ATCAF1").GetAwaiter();
					if (!taskAwaiter3.IsCompleted)
					{
						num2 = 28;
						TaskAwaiter taskAwaiter4 = taskAwaiter3;
						this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, OBDDataReader.<SendLongCanRequest>d__153>(ref taskAwaiter3, ref this);
						return;
					}
					IL_10F7:
					taskAwaiter3.GetResult();
					taskAwaiter3 = App.OBDReader.DebugWrite("\n[" + App.OBDReader.stopwatch.Elapsed.TotalSeconds.ToString() + "]\n").GetAwaiter();
					if (!taskAwaiter3.IsCompleted)
					{
						num2 = 29;
						TaskAwaiter taskAwaiter4 = taskAwaiter3;
						this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, OBDDataReader.<SendLongCanRequest>d__153>(ref taskAwaiter3, ref this);
						return;
					}
					IL_118A:
					taskAwaiter3.GetResult();
					taskAwaiter = obddataReader.ReadData(2500, null, -1).GetAwaiter();
					if (!taskAwaiter.IsCompleted)
					{
						num2 = 30;
						TaskAwaiter<string> taskAwaiter2 = taskAwaiter;
						this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<string>, OBDDataReader.<SendLongCanRequest>d__153>(ref taskAwaiter, ref this);
						return;
					}
					IL_11F0:
					taskAwaiter.GetResult();
					taskAwaiter3 = App.OBDReader.DebugWrite("\n[" + App.OBDReader.stopwatch.Elapsed.TotalSeconds.ToString() + "]\n").GetAwaiter();
					if (!taskAwaiter3.IsCompleted)
					{
						num2 = 31;
						TaskAwaiter taskAwaiter4 = taskAwaiter3;
						this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, OBDDataReader.<SendLongCanRequest>d__153>(ref taskAwaiter3, ref this);
						return;
					}
					IL_1284:
					taskAwaiter3.GetResult();
					if (sent_atcfc)
					{
						goto IL_1521;
					}
					sent_atcfc = true;
					taskAwaiter3 = App.OBDReader.DebugWrite("\n[" + App.OBDReader.stopwatch.Elapsed.TotalSeconds.ToString() + "]\n").GetAwaiter();
					if (!taskAwaiter3.IsCompleted)
					{
						num2 = 32;
						TaskAwaiter taskAwaiter4 = taskAwaiter3;
						this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, OBDDataReader.<SendLongCanRequest>d__153>(ref taskAwaiter3, ref this);
						return;
					}
					IL_1329:
					taskAwaiter3.GetResult();
					taskAwaiter3 = obddataReader.SendString("ATCFC1").GetAwaiter();
					if (!taskAwaiter3.IsCompleted)
					{
						num2 = 33;
						TaskAwaiter taskAwaiter4 = taskAwaiter3;
						this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, OBDDataReader.<SendLongCanRequest>d__153>(ref taskAwaiter3, ref this);
						return;
					}
					IL_138D:
					taskAwaiter3.GetResult();
					taskAwaiter3 = App.OBDReader.DebugWrite("\n[" + App.OBDReader.stopwatch.Elapsed.TotalSeconds.ToString() + "]\n").GetAwaiter();
					if (!taskAwaiter3.IsCompleted)
					{
						num2 = 34;
						TaskAwaiter taskAwaiter4 = taskAwaiter3;
						this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, OBDDataReader.<SendLongCanRequest>d__153>(ref taskAwaiter3, ref this);
						return;
					}
					IL_1420:
					taskAwaiter3.GetResult();
					taskAwaiter = obddataReader.ReadData(2500, null, -1).GetAwaiter();
					if (!taskAwaiter.IsCompleted)
					{
						num2 = 35;
						TaskAwaiter<string> taskAwaiter2 = taskAwaiter;
						this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<string>, OBDDataReader.<SendLongCanRequest>d__153>(ref taskAwaiter, ref this);
						return;
					}
					IL_1486:
					taskAwaiter.GetResult();
					taskAwaiter3 = App.OBDReader.DebugWrite("\n[" + App.OBDReader.stopwatch.Elapsed.TotalSeconds.ToString() + "]\n").GetAwaiter();
					if (!taskAwaiter3.IsCompleted)
					{
						num2 = 36;
						TaskAwaiter taskAwaiter4 = taskAwaiter3;
						this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, OBDDataReader.<SendLongCanRequest>d__153>(ref taskAwaiter3, ref this);
						return;
					}
					IL_151A:
					taskAwaiter3.GetResult();
					IL_1521:
					text = data;
				}
				catch (Exception ex)
				{
					num2 = -2;
					frames = null;
					data = null;
					tp = null;
					sw = null;
					this.<>t__builder.SetException(ex);
					return;
				}
				IL_155F:
				num2 = -2;
				frames = null;
				data = null;
				tp = null;
				sw = null;
				this.<>t__builder.SetResult(text);
			}

			// Token: 0x06002635 RID: 9781 RVA: 0x001D8E14 File Offset: 0x001D7014
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x04001482 RID: 5250
			public int <>1__state;

			// Token: 0x04001483 RID: 5251
			public AsyncTaskMethodBuilder<string> <>t__builder;

			// Token: 0x04001484 RID: 5252
			public OBDDataReader <>4__this;

			// Token: 0x04001485 RID: 5253
			public OBDRequest request;

			// Token: 0x04001486 RID: 5254
			private bool <atr0>5__2;

			// Token: 0x04001487 RID: 5255
			private string[] <frames>5__3;

			// Token: 0x04001488 RID: 5256
			private int <delay>5__4;

			// Token: 0x04001489 RID: 5257
			private int <frameIdx>5__5;

			// Token: 0x0400148A RID: 5258
			private string <data>5__6;

			// Token: 0x0400148B RID: 5259
			private bool <sent_atcfc1>5__7;

			// Token: 0x0400148C RID: 5260
			private string[] <tp>5__8;

			// Token: 0x0400148D RID: 5261
			private ELMFormat <elmFormat>5__9;

			// Token: 0x0400148E RID: 5262
			private int <sendTPPeriod>5__10;

			// Token: 0x0400148F RID: 5263
			private Stopwatch <sw>5__11;

			// Token: 0x04001490 RID: 5264
			private TaskAwaiter<string> <>u__1;

			// Token: 0x04001491 RID: 5265
			private TaskAwaiter <>u__2;

			// Token: 0x04001492 RID: 5266
			private string[] <>7__wrap11;

			// Token: 0x04001493 RID: 5267
			private int <>7__wrap12;
		}

		// Token: 0x02000382 RID: 898
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <SendLongCanRequestSTN>d__154 : IAsyncStateMachine
		{
			// Token: 0x06002636 RID: 9782 RVA: 0x001D8E24 File Offset: 0x001D7024
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				OBDDataReader obddataReader = this;
				string result;
				try
				{
					ValueTaskAwaiter valueTaskAwaiter;
					TaskAwaiter taskAwaiter;
					TaskAwaiter<string> taskAwaiter3;
					switch (num)
					{
					case 0:
					{
						ValueTaskAwaiter valueTaskAwaiter2;
						valueTaskAwaiter = valueTaskAwaiter2;
						valueTaskAwaiter2 = default(ValueTaskAwaiter);
						num2 = -1;
						break;
					}
					case 1:
					{
						TaskAwaiter taskAwaiter2;
						taskAwaiter = taskAwaiter2;
						taskAwaiter2 = default(TaskAwaiter);
						num2 = -1;
						goto IL_00EF;
					}
					case 2:
					{
						TaskAwaiter<string> taskAwaiter4;
						taskAwaiter3 = taskAwaiter4;
						taskAwaiter4 = default(TaskAwaiter<string>);
						num2 = -1;
						goto IL_0151;
					}
					default:
						if (obddataReader.ELMStatus.STNTransmitSegmentation)
						{
							goto IL_008D;
						}
						valueTaskAwaiter = obddataReader.SetSTNLevelCANTransmitSegmentation(true).GetAwaiter();
						if (!valueTaskAwaiter.IsCompleted)
						{
							num2 = 0;
							ValueTaskAwaiter valueTaskAwaiter2 = valueTaskAwaiter;
							this.<>t__builder.AwaitUnsafeOnCompleted<ValueTaskAwaiter, OBDDataReader.<SendLongCanRequestSTN>d__154>(ref valueTaskAwaiter, ref this);
							return;
						}
						break;
					}
					valueTaskAwaiter.GetResult();
					IL_008D:
					taskAwaiter = obddataReader.SendString(request.Command).GetAwaiter();
					if (!taskAwaiter.IsCompleted)
					{
						num2 = 1;
						TaskAwaiter taskAwaiter2 = taskAwaiter;
						this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, OBDDataReader.<SendLongCanRequestSTN>d__154>(ref taskAwaiter, ref this);
						return;
					}
					IL_00EF:
					taskAwaiter.GetResult();
					taskAwaiter3 = obddataReader.ReadData(2500, null, -1).GetAwaiter();
					if (!taskAwaiter3.IsCompleted)
					{
						num2 = 2;
						TaskAwaiter<string> taskAwaiter4 = taskAwaiter3;
						this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<string>, OBDDataReader.<SendLongCanRequestSTN>d__154>(ref taskAwaiter3, ref this);
						return;
					}
					IL_0151:
					result = taskAwaiter3.GetResult();
				}
				catch (Exception ex)
				{
					num2 = -2;
					this.<>t__builder.SetException(ex);
					return;
				}
				num2 = -2;
				this.<>t__builder.SetResult(result);
			}

			// Token: 0x06002637 RID: 9783 RVA: 0x001D8FD8 File Offset: 0x001D71D8
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x04001494 RID: 5268
			public int <>1__state;

			// Token: 0x04001495 RID: 5269
			public AsyncTaskMethodBuilder<string> <>t__builder;

			// Token: 0x04001496 RID: 5270
			public OBDDataReader <>4__this;

			// Token: 0x04001497 RID: 5271
			public OBDRequest request;

			// Token: 0x04001498 RID: 5272
			private ValueTaskAwaiter <>u__1;

			// Token: 0x04001499 RID: 5273
			private TaskAwaiter <>u__2;

			// Token: 0x0400149A RID: 5274
			private TaskAwaiter<string> <>u__3;
		}

		// Token: 0x02000383 RID: 899
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <SendRequest>d__127 : IAsyncStateMachine
		{
			// Token: 0x06002638 RID: 9784 RVA: 0x001D8FE8 File Offset: 0x001D71E8
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				OBDDataReader obddataReader = this;
				try
				{
					TaskAwaiter taskAwaiter;
					TaskAwaiter<string> taskAwaiter3;
					switch (num)
					{
					case 0:
					{
						TaskAwaiter taskAwaiter2;
						taskAwaiter = taskAwaiter2;
						taskAwaiter2 = default(TaskAwaiter);
						num2 = -1;
						break;
					}
					case 1:
					{
						TaskAwaiter<string> taskAwaiter4;
						taskAwaiter3 = taskAwaiter4;
						taskAwaiter4 = default(TaskAwaiter<string>);
						num2 = -1;
						goto IL_0167;
					}
					case 2:
					{
						TaskAwaiter taskAwaiter2;
						taskAwaiter = taskAwaiter2;
						taskAwaiter2 = default(TaskAwaiter);
						num2 = -1;
						goto IL_0244;
					}
					case 3:
					{
						TaskAwaiter<string> taskAwaiter4;
						taskAwaiter3 = taskAwaiter4;
						taskAwaiter4 = default(TaskAwaiter<string>);
						num2 = -1;
						goto IL_02A9;
					}
					case 4:
					{
						TaskAwaiter taskAwaiter2;
						taskAwaiter = taskAwaiter2;
						taskAwaiter2 = default(TaskAwaiter);
						num2 = -1;
						goto IL_0363;
					}
					case 5:
					{
						TaskAwaiter<string> taskAwaiter4;
						taskAwaiter3 = taskAwaiter4;
						taskAwaiter4 = default(TaskAwaiter<string>);
						num2 = -1;
						goto IL_03C8;
					}
					case 6:
					{
						TaskAwaiter taskAwaiter2;
						taskAwaiter = taskAwaiter2;
						taskAwaiter2 = default(TaskAwaiter);
						num2 = -1;
						goto IL_0457;
					}
					default:
						if (!(request.Header != obddataReader.ELM327_LastSentHeader) || (string.IsNullOrEmpty(request.Header) && string.IsNullOrEmpty(obddataReader.ELM327_LastSentHeader)))
						{
							goto IL_02BD;
						}
						if (obddataReader.ELM327_PendingAfterCommands != null && obddataReader.ELM327_PendingAfterCommands.Length != 0)
						{
							array = obddataReader.ELM327_PendingAfterCommands;
							i = 0;
							goto IL_017D;
						}
						goto IL_01A3;
					}
					IL_0102:
					taskAwaiter.GetResult();
					taskAwaiter3 = obddataReader.ReadData(2500, null, -1).GetAwaiter();
					if (!taskAwaiter3.IsCompleted)
					{
						num2 = 1;
						TaskAwaiter<string> taskAwaiter4 = taskAwaiter3;
						this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<string>, OBDDataReader.<SendRequest>d__127>(ref taskAwaiter3, ref this);
						return;
					}
					IL_0167:
					taskAwaiter3.GetResult();
					i++;
					IL_017D:
					if (i >= array.Length)
					{
						array = null;
						obddataReader.ELM327_PendingAfterCommands = new string[0];
					}
					else
					{
						string text = array[i];
						taskAwaiter = obddataReader.SendString(text).GetAwaiter();
						if (!taskAwaiter.IsCompleted)
						{
							num2 = 0;
							TaskAwaiter taskAwaiter2 = taskAwaiter;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, OBDDataReader.<SendRequest>d__127>(ref taskAwaiter, ref this);
							return;
						}
						goto IL_0102;
					}
					IL_01A3:
					string text2;
					if (string.IsNullOrEmpty(request.Header))
					{
						text2 = obddataReader.GetDefaultHeader();
						obddataReader.ELM327_LastSentHeader = "";
					}
					else
					{
						text2 = request.Header;
						obddataReader.ELM327_LastSentHeader = text2;
					}
					taskAwaiter = obddataReader.SendString("ATSH" + text2).GetAwaiter();
					if (!taskAwaiter.IsCompleted)
					{
						num2 = 2;
						TaskAwaiter taskAwaiter2 = taskAwaiter;
						this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, OBDDataReader.<SendRequest>d__127>(ref taskAwaiter, ref this);
						return;
					}
					IL_0244:
					taskAwaiter.GetResult();
					taskAwaiter3 = obddataReader.ReadData(2500, null, -1).GetAwaiter();
					if (!taskAwaiter3.IsCompleted)
					{
						num2 = 3;
						TaskAwaiter<string> taskAwaiter4 = taskAwaiter3;
						this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<string>, OBDDataReader.<SendRequest>d__127>(ref taskAwaiter3, ref this);
						return;
					}
					IL_02A9:
					taskAwaiter3.GetResult();
					obddataReader.ELM327_LastSentBeforeCommands = new string[0];
					IL_02BD:
					if (request.BeforeCommands != null && request.BeforeCommands.Length != 0)
					{
						array = request.BeforeCommands;
						i = 0;
						goto IL_03DE;
					}
					goto IL_03F8;
					IL_0363:
					taskAwaiter.GetResult();
					taskAwaiter3 = obddataReader.ReadData(2500, null, -1).GetAwaiter();
					if (!taskAwaiter3.IsCompleted)
					{
						num2 = 5;
						TaskAwaiter<string> taskAwaiter4 = taskAwaiter3;
						this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<string>, OBDDataReader.<SendRequest>d__127>(ref taskAwaiter3, ref this);
						return;
					}
					IL_03C8:
					taskAwaiter3.GetResult();
					i++;
					IL_03DE:
					if (i >= array.Length)
					{
						array = null;
					}
					else
					{
						string text3 = array[i];
						taskAwaiter = obddataReader.SendString(text3).GetAwaiter();
						if (!taskAwaiter.IsCompleted)
						{
							num2 = 4;
							TaskAwaiter taskAwaiter2 = taskAwaiter;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, OBDDataReader.<SendRequest>d__127>(ref taskAwaiter, ref this);
							return;
						}
						goto IL_0363;
					}
					IL_03F8:
					taskAwaiter = obddataReader.SendString(request.Command).GetAwaiter();
					if (!taskAwaiter.IsCompleted)
					{
						num2 = 6;
						TaskAwaiter taskAwaiter2 = taskAwaiter;
						this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, OBDDataReader.<SendRequest>d__127>(ref taskAwaiter, ref this);
						return;
					}
					IL_0457:
					taskAwaiter.GetResult();
					obddataReader.ELM327_PendingAfterCommands = request.AfterCommands;
				}
				catch (Exception ex)
				{
					num2 = -2;
					this.<>t__builder.SetException(ex);
					return;
				}
				num2 = -2;
				this.<>t__builder.SetResult();
			}

			// Token: 0x06002639 RID: 9785 RVA: 0x001D94B0 File Offset: 0x001D76B0
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x0400149B RID: 5275
			public int <>1__state;

			// Token: 0x0400149C RID: 5276
			public AsyncTaskMethodBuilder <>t__builder;

			// Token: 0x0400149D RID: 5277
			public OBDRequest request;

			// Token: 0x0400149E RID: 5278
			public OBDDataReader <>4__this;

			// Token: 0x0400149F RID: 5279
			private string[] <>7__wrap1;

			// Token: 0x040014A0 RID: 5280
			private int <>7__wrap2;

			// Token: 0x040014A1 RID: 5281
			private TaskAwaiter <>u__1;

			// Token: 0x040014A2 RID: 5282
			private TaskAwaiter<string> <>u__2;
		}

		// Token: 0x02000384 RID: 900
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <SendString>d__138 : IAsyncStateMachine
		{
			// Token: 0x0600263A RID: 9786 RVA: 0x001D94C0 File Offset: 0x001D76C0
			void IAsyncStateMachine.MoveNext()
			{
				int num4;
				int num3 = num4;
				OBDDataReader obddataReader = this;
				try
				{
					TaskAwaiter taskAwaiter2;
					TaskAwaiter taskAwaiter;
					switch (num3)
					{
					case 0:
					case 1:
					case 2:
						break;
					case 3:
						taskAwaiter = taskAwaiter2;
						taskAwaiter2 = default(TaskAwaiter);
						num4 = -1;
						goto IL_030E;
					case 4:
						taskAwaiter = taskAwaiter2;
						taskAwaiter2 = default(TaskAwaiter);
						num4 = -1;
						goto IL_03A9;
					default:
					{
						obddataReader.ELMStatus.UpdateStatusFromCommand(data);
						req_bytes = new byte[data.Length + 1];
						Encoding.ASCII.GetBytes(data, 0, data.Length, req_bytes, 0);
						req_bytes[req_bytes.Length - 1] = 13;
						int num5 = 5;
						if (data.Length > 17)
						{
							num5 = 15;
						}
						timeout = new TimeSpan(0, 0, num5);
						num2 = 0;
						break;
					}
					}
					try
					{
						switch (num3)
						{
						case 0:
							taskAwaiter = taskAwaiter2;
							taskAwaiter2 = default(TaskAwaiter);
							num4 = -1;
							break;
						case 1:
							taskAwaiter = taskAwaiter2;
							taskAwaiter2 = default(TaskAwaiter);
							num4 = -1;
							goto IL_01B6;
						case 2:
							taskAwaiter = taskAwaiter2;
							taskAwaiter2 = default(TaskAwaiter);
							num4 = -1;
							goto IL_0240;
						default:
							if (obddataReader.LastAction == OBDDataReader.RemoteDeviceLastAction.Write)
							{
								goto IL_03F3;
							}
							taskAwaiter = obddataReader.Connection.WriteBytesAsync(req_bytes).TimeoutAfter(timeout).GetAwaiter();
							if (!taskAwaiter.IsCompleted)
							{
								num4 = 0;
								taskAwaiter2 = taskAwaiter;
								this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, OBDDataReader.<SendString>d__138>(ref taskAwaiter, ref this);
								return;
							}
							break;
						}
						taskAwaiter.GetResult();
						if (!obddataReader.Connection.NeedFlush)
						{
							goto IL_01BD;
						}
						taskAwaiter = obddataReader.Connection.FlushAsync().TimeoutAfter(timeout).GetAwaiter();
						if (!taskAwaiter.IsCompleted)
						{
							num4 = 1;
							taskAwaiter2 = taskAwaiter;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, OBDDataReader.<SendString>d__138>(ref taskAwaiter, ref this);
							return;
						}
						IL_01B6:
						taskAwaiter.GetResult();
						IL_01BD:
						obddataReader.LastAction = OBDDataReader.RemoteDeviceLastAction.Write;
						obddataReader.LastReadOrWriteTicks = obddataReader.stopwatch.ElapsedTicks;
						obddataReader.CommandsCounter += 1L;
						taskAwaiter = obddataReader.DebugWrite(req_bytes).GetAwaiter();
						if (!taskAwaiter.IsCompleted)
						{
							num4 = 2;
							taskAwaiter2 = taskAwaiter;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, OBDDataReader.<SendString>d__138>(ref taskAwaiter, ref this);
							return;
						}
						IL_0240:
						taskAwaiter.GetResult();
					}
					catch (TimeoutException ex)
					{
						obj = ex;
						num2 = 1;
					}
					catch (Exception ex2)
					{
						obj = ex2;
						num2 = 2;
					}
					int num6 = num2;
					if (num6 != 1)
					{
						if (num6 != 2)
						{
							obj = null;
							goto IL_03F3;
						}
						exc = (Exception)obj;
						taskAwaiter = obddataReader.DebugWrite("\r\nWriteDataTimeoutException(Write timeout exception)\r\n" + exc.ToString()).GetAwaiter();
						if (!taskAwaiter.IsCompleted)
						{
							num4 = 4;
							taskAwaiter2 = taskAwaiter;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, OBDDataReader.<SendString>d__138>(ref taskAwaiter, ref this);
							return;
						}
						goto IL_03A9;
					}
					else
					{
						texc = (TimeoutException)obj;
						taskAwaiter = obddataReader.DebugWrite("\r\nWriteDataTimeoutException(Write timeout exception) for data: " + data + "\r\n" + texc.ToString()).GetAwaiter();
						if (!taskAwaiter.IsCompleted)
						{
							num4 = 3;
							taskAwaiter2 = taskAwaiter;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, OBDDataReader.<SendString>d__138>(ref taskAwaiter, ref this);
							return;
						}
					}
					IL_030E:
					taskAwaiter.GetResult();
					obddataReader.LastAction = OBDDataReader.RemoteDeviceLastAction.Write;
					throw new WriteDataTimeoutException("Write timeout exception", texc);
					IL_03A9:
					taskAwaiter.GetResult();
					obddataReader.LastAction = OBDDataReader.RemoteDeviceLastAction.Write;
					throw new GeneralWriteException("Write general exception", exc);
				}
				catch (Exception ex3)
				{
					num4 = -2;
					req_bytes = null;
					this.<>t__builder.SetException(ex3);
					return;
				}
				IL_03F3:
				num4 = -2;
				req_bytes = null;
				this.<>t__builder.SetResult();
			}

			// Token: 0x0600263B RID: 9787 RVA: 0x001D9928 File Offset: 0x001D7B28
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x040014A3 RID: 5283
			public int <>1__state;

			// Token: 0x040014A4 RID: 5284
			public AsyncTaskMethodBuilder <>t__builder;

			// Token: 0x040014A5 RID: 5285
			public OBDDataReader <>4__this;

			// Token: 0x040014A6 RID: 5286
			public string data;

			// Token: 0x040014A7 RID: 5287
			private byte[] <req_bytes>5__2;

			// Token: 0x040014A8 RID: 5288
			private TimeSpan <timeout>5__3;

			// Token: 0x040014A9 RID: 5289
			private object <>7__wrap3;

			// Token: 0x040014AA RID: 5290
			private int <>7__wrap4;

			// Token: 0x040014AB RID: 5291
			private TaskAwaiter <>u__1;

			// Token: 0x040014AC RID: 5292
			private TimeoutException <texc>5__6;

			// Token: 0x040014AD RID: 5293
			private Exception <exc>5__7;
		}

		// Token: 0x02000385 RID: 901
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <SetSTNLevelCANReceiveSegmentation>d__172 : IAsyncStateMachine
		{
			// Token: 0x0600263C RID: 9788 RVA: 0x001D9938 File Offset: 0x001D7B38
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				OBDDataReader obddataReader = this;
				try
				{
					TaskAwaiter taskAwaiter;
					TaskAwaiter<string> taskAwaiter3;
					switch (num)
					{
					case 0:
					{
						TaskAwaiter taskAwaiter2;
						taskAwaiter = taskAwaiter2;
						taskAwaiter2 = default(TaskAwaiter);
						num2 = -1;
						break;
					}
					case 1:
					{
						TaskAwaiter taskAwaiter2;
						taskAwaiter = taskAwaiter2;
						taskAwaiter2 = default(TaskAwaiter);
						num2 = -1;
						goto IL_00FD;
					}
					case 2:
					{
						TaskAwaiter<string> taskAwaiter4;
						taskAwaiter3 = taskAwaiter4;
						taskAwaiter4 = default(TaskAwaiter<string>);
						num2 = -1;
						goto IL_0162;
					}
					default:
						if (!obddataReader.STCommandsStupported || !SharedSettings.Current.CANResponseSegmentationSTNLevel)
						{
							goto IL_0198;
						}
						if (value)
						{
							taskAwaiter = obddataReader.SendString("STCSEGR1").GetAwaiter();
							if (!taskAwaiter.IsCompleted)
							{
								num2 = 0;
								TaskAwaiter taskAwaiter2 = taskAwaiter;
								this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, OBDDataReader.<SetSTNLevelCANReceiveSegmentation>d__172>(ref taskAwaiter, ref this);
								return;
							}
						}
						else
						{
							taskAwaiter = obddataReader.SendString("STCSEGR0").GetAwaiter();
							if (!taskAwaiter.IsCompleted)
							{
								num2 = 1;
								TaskAwaiter taskAwaiter2 = taskAwaiter;
								this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, OBDDataReader.<SetSTNLevelCANReceiveSegmentation>d__172>(ref taskAwaiter, ref this);
								return;
							}
							goto IL_00FD;
						}
						break;
					}
					taskAwaiter.GetResult();
					goto IL_0104;
					IL_00FD:
					taskAwaiter.GetResult();
					IL_0104:
					taskAwaiter3 = obddataReader.ReadData(2500, null, -1).GetAwaiter();
					if (!taskAwaiter3.IsCompleted)
					{
						num2 = 2;
						TaskAwaiter<string> taskAwaiter4 = taskAwaiter3;
						this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<string>, OBDDataReader.<SetSTNLevelCANReceiveSegmentation>d__172>(ref taskAwaiter3, ref this);
						return;
					}
					IL_0162:
					string result = taskAwaiter3.GetResult();
					if (result != null && result.Contains("OK"))
					{
						obddataReader.ELMStatus.SetSTNReceiveSegmentation(value);
					}
					else
					{
						SharedSettings.Current.CANResponseSegmentationSTNLevel = false;
					}
					IL_0198:;
				}
				catch (Exception ex)
				{
					num2 = -2;
					this.<>t__builder.SetException(ex);
					return;
				}
				num2 = -2;
				this.<>t__builder.SetResult();
			}

			// Token: 0x0600263D RID: 9789 RVA: 0x001D9B28 File Offset: 0x001D7D28
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x040014AE RID: 5294
			public int <>1__state;

			// Token: 0x040014AF RID: 5295
			public AsyncValueTaskMethodBuilder <>t__builder;

			// Token: 0x040014B0 RID: 5296
			public OBDDataReader <>4__this;

			// Token: 0x040014B1 RID: 5297
			public bool value;

			// Token: 0x040014B2 RID: 5298
			private TaskAwaiter <>u__1;

			// Token: 0x040014B3 RID: 5299
			private TaskAwaiter<string> <>u__2;
		}

		// Token: 0x02000386 RID: 902
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <SetSTNLevelCANTransmitSegmentation>d__171 : IAsyncStateMachine
		{
			// Token: 0x0600263E RID: 9790 RVA: 0x001D9B38 File Offset: 0x001D7D38
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				OBDDataReader obddataReader = this;
				try
				{
					TaskAwaiter taskAwaiter;
					TaskAwaiter<string> taskAwaiter3;
					switch (num)
					{
					case 0:
					{
						TaskAwaiter taskAwaiter2;
						taskAwaiter = taskAwaiter2;
						taskAwaiter2 = default(TaskAwaiter);
						num2 = -1;
						break;
					}
					case 1:
					{
						TaskAwaiter taskAwaiter2;
						taskAwaiter = taskAwaiter2;
						taskAwaiter2 = default(TaskAwaiter);
						num2 = -1;
						goto IL_00FD;
					}
					case 2:
					{
						TaskAwaiter<string> taskAwaiter4;
						taskAwaiter3 = taskAwaiter4;
						taskAwaiter4 = default(TaskAwaiter<string>);
						num2 = -1;
						goto IL_0162;
					}
					default:
						if (!obddataReader.STCommandsStupported || !SharedSettings.Current.CANRequestSegmentationSTNLevel)
						{
							goto IL_0198;
						}
						if (value)
						{
							taskAwaiter = obddataReader.SendString("STCSEGT1").GetAwaiter();
							if (!taskAwaiter.IsCompleted)
							{
								num2 = 0;
								TaskAwaiter taskAwaiter2 = taskAwaiter;
								this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, OBDDataReader.<SetSTNLevelCANTransmitSegmentation>d__171>(ref taskAwaiter, ref this);
								return;
							}
						}
						else
						{
							taskAwaiter = obddataReader.SendString("STCSEGT0").GetAwaiter();
							if (!taskAwaiter.IsCompleted)
							{
								num2 = 1;
								TaskAwaiter taskAwaiter2 = taskAwaiter;
								this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, OBDDataReader.<SetSTNLevelCANTransmitSegmentation>d__171>(ref taskAwaiter, ref this);
								return;
							}
							goto IL_00FD;
						}
						break;
					}
					taskAwaiter.GetResult();
					goto IL_0104;
					IL_00FD:
					taskAwaiter.GetResult();
					IL_0104:
					taskAwaiter3 = obddataReader.ReadData(2500, null, -1).GetAwaiter();
					if (!taskAwaiter3.IsCompleted)
					{
						num2 = 2;
						TaskAwaiter<string> taskAwaiter4 = taskAwaiter3;
						this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<string>, OBDDataReader.<SetSTNLevelCANTransmitSegmentation>d__171>(ref taskAwaiter3, ref this);
						return;
					}
					IL_0162:
					string result = taskAwaiter3.GetResult();
					if (result != null && result.Contains("OK"))
					{
						obddataReader.ELMStatus.SetSTNTransmitSegmentation(value);
					}
					else
					{
						SharedSettings.Current.CANRequestSegmentationSTNLevel = false;
					}
					IL_0198:;
				}
				catch (Exception ex)
				{
					num2 = -2;
					this.<>t__builder.SetException(ex);
					return;
				}
				num2 = -2;
				this.<>t__builder.SetResult();
			}

			// Token: 0x0600263F RID: 9791 RVA: 0x001D9D28 File Offset: 0x001D7F28
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x040014B4 RID: 5300
			public int <>1__state;

			// Token: 0x040014B5 RID: 5301
			public AsyncValueTaskMethodBuilder <>t__builder;

			// Token: 0x040014B6 RID: 5302
			public OBDDataReader <>4__this;

			// Token: 0x040014B7 RID: 5303
			public bool value;

			// Token: 0x040014B8 RID: 5304
			private TaskAwaiter <>u__1;

			// Token: 0x040014B9 RID: 5305
			private TaskAwaiter<string> <>u__2;
		}

		// Token: 0x02000387 RID: 903
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <StartLoopV3>d__147 : IAsyncStateMachine
		{
			// Token: 0x06002640 RID: 9792 RVA: 0x001D9D38 File Offset: 0x001D7F38
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				OBDDataReader obddataReader = this;
				try
				{
					TaskAwaiter taskAwaiter;
					switch (num)
					{
					case 0:
					{
						TaskAwaiter taskAwaiter2;
						taskAwaiter = taskAwaiter2;
						taskAwaiter2 = default(TaskAwaiter);
						num = (num2 = -1);
						break;
					}
					case 1:
					{
						TaskAwaiter taskAwaiter2;
						taskAwaiter = taskAwaiter2;
						taskAwaiter2 = default(TaskAwaiter);
						num = (num2 = -1);
						goto IL_0217;
					}
					case 2:
					{
						TaskAwaiter taskAwaiter2;
						taskAwaiter = taskAwaiter2;
						taskAwaiter2 = default(TaskAwaiter);
						num = (num2 = -1);
						goto IL_02C3;
					}
					case 3:
					case 4:
					case 5:
					case 6:
					case 7:
					case 8:
					case 9:
					case 10:
					case 11:
					case 12:
					case 13:
					case 14:
					case 15:
					case 16:
					case 17:
					case 18:
					case 19:
					case 20:
					case 21:
					case 22:
					case 23:
					case 24:
					case 25:
					case 26:
					case 27:
					case 28:
					case 29:
						IL_02E0:
						try
						{
							TaskAwaiter taskAwaiter2;
							ValueTaskAwaiter valueTaskAwaiter;
							TaskAwaiter<string> taskAwaiter3;
							ValueTaskAwaiter<string> valueTaskAwaiter3;
							TaskAwaiter<bool> taskAwaiter5;
							ValueTaskAwaiter<bool> valueTaskAwaiter5;
							switch (num)
							{
							case 3:
								break;
							case 4:
								taskAwaiter = taskAwaiter2;
								taskAwaiter2 = default(TaskAwaiter);
								num = (num2 = -1);
								goto IL_04AD;
							case 5:
								taskAwaiter = taskAwaiter2;
								taskAwaiter2 = default(TaskAwaiter);
								num = (num2 = -1);
								goto IL_050E;
							case 6:
							{
								ValueTaskAwaiter valueTaskAwaiter2;
								valueTaskAwaiter = valueTaskAwaiter2;
								valueTaskAwaiter2 = default(ValueTaskAwaiter);
								num = (num2 = -1);
								goto IL_07D9;
							}
							case 7:
								taskAwaiter = taskAwaiter2;
								taskAwaiter2 = default(TaskAwaiter);
								num = (num2 = -1);
								goto IL_08BF;
							case 8:
							{
								TaskAwaiter<string> taskAwaiter4;
								taskAwaiter3 = taskAwaiter4;
								taskAwaiter4 = default(TaskAwaiter<string>);
								num = (num2 = -1);
								goto IL_0924;
							}
							case 9:
							{
								ValueTaskAwaiter valueTaskAwaiter2;
								valueTaskAwaiter = valueTaskAwaiter2;
								valueTaskAwaiter2 = default(ValueTaskAwaiter);
								num = (num2 = -1);
								goto IL_0A3E;
							}
							case 10:
							{
								TaskAwaiter<string> taskAwaiter4;
								taskAwaiter3 = taskAwaiter4;
								taskAwaiter4 = default(TaskAwaiter<string>);
								num = (num2 = -1);
								goto IL_0B91;
							}
							case 11:
							{
								TaskAwaiter<string> taskAwaiter4;
								taskAwaiter3 = taskAwaiter4;
								taskAwaiter4 = default(TaskAwaiter<string>);
								num = (num2 = -1);
								goto IL_0C0A;
							}
							case 12:
							{
								TaskAwaiter<string> taskAwaiter4;
								taskAwaiter3 = taskAwaiter4;
								taskAwaiter4 = default(TaskAwaiter<string>);
								num = (num2 = -1);
								goto IL_0CBB;
							}
							case 13:
							{
								ValueTaskAwaiter<string> valueTaskAwaiter4;
								valueTaskAwaiter3 = valueTaskAwaiter4;
								valueTaskAwaiter4 = default(ValueTaskAwaiter<string>);
								num = (num2 = -1);
								goto IL_0D77;
							}
							case 14:
								taskAwaiter = taskAwaiter2;
								taskAwaiter2 = default(TaskAwaiter);
								num = (num2 = -1);
								goto IL_0E3B;
							case 15:
							{
								TaskAwaiter<string> taskAwaiter4;
								taskAwaiter3 = taskAwaiter4;
								taskAwaiter4 = default(TaskAwaiter<string>);
								num = (num2 = -1);
								goto IL_0EB0;
							}
							case 16:
							{
								TaskAwaiter<string> taskAwaiter4;
								taskAwaiter3 = taskAwaiter4;
								taskAwaiter4 = default(TaskAwaiter<string>);
								num = (num2 = -1);
								goto IL_0F5A;
							}
							case 17:
								taskAwaiter = taskAwaiter2;
								taskAwaiter2 = default(TaskAwaiter);
								num = (num2 = -1);
								goto IL_100E;
							case 18:
							{
								TaskAwaiter<string> taskAwaiter4;
								taskAwaiter3 = taskAwaiter4;
								taskAwaiter4 = default(TaskAwaiter<string>);
								num = (num2 = -1);
								goto IL_1074;
							}
							case 19:
								taskAwaiter = taskAwaiter2;
								taskAwaiter2 = default(TaskAwaiter);
								num = (num2 = -1);
								goto IL_10D6;
							case 20:
							{
								TaskAwaiter<string> taskAwaiter4;
								taskAwaiter3 = taskAwaiter4;
								taskAwaiter4 = default(TaskAwaiter<string>);
								num = (num2 = -1);
								goto IL_113C;
							}
							case 21:
								taskAwaiter = taskAwaiter2;
								taskAwaiter2 = default(TaskAwaiter);
								num = (num2 = -1);
								goto IL_119E;
							case 22:
							{
								TaskAwaiter<string> taskAwaiter4;
								taskAwaiter3 = taskAwaiter4;
								taskAwaiter4 = default(TaskAwaiter<string>);
								num = (num2 = -1);
								goto IL_1204;
							}
							case 23:
							{
								TaskAwaiter<bool> taskAwaiter6;
								taskAwaiter5 = taskAwaiter6;
								taskAwaiter6 = default(TaskAwaiter<bool>);
								num = (num2 = -1);
								goto IL_1432;
							}
							case 24:
							{
								TaskAwaiter<bool> taskAwaiter6;
								taskAwaiter5 = taskAwaiter6;
								taskAwaiter6 = default(TaskAwaiter<bool>);
								num = (num2 = -1);
								goto IL_1530;
							}
							case 25:
								taskAwaiter = taskAwaiter2;
								taskAwaiter2 = default(TaskAwaiter);
								num = (num2 = -1);
								goto IL_1604;
							case 26:
							{
								TaskAwaiter<string> taskAwaiter4;
								taskAwaiter3 = taskAwaiter4;
								taskAwaiter4 = default(TaskAwaiter<string>);
								num = (num2 = -1);
								goto IL_166A;
							}
							case 27:
							{
								ValueTaskAwaiter<bool> valueTaskAwaiter6;
								valueTaskAwaiter5 = valueTaskAwaiter6;
								valueTaskAwaiter6 = default(ValueTaskAwaiter<bool>);
								num = (num2 = -1);
								goto IL_1720;
							}
							case 28:
							{
								ValueTaskAwaiter<bool> valueTaskAwaiter6;
								valueTaskAwaiter5 = valueTaskAwaiter6;
								valueTaskAwaiter6 = default(ValueTaskAwaiter<bool>);
								num = (num2 = -1);
								goto IL_1799;
							}
							case 29:
								IL_1829:
								try
								{
									if (num != 29)
									{
										if (obddataReader.waitingForQueueSemaphore == null)
										{
											goto IL_18A9;
										}
										SemaphoreSlim waitingForQueueSemaphore = obddataReader.waitingForQueueSemaphore;
										if (waitingForQueueSemaphore != null)
										{
											waitingForQueueSemaphore.Release();
										}
										taskAwaiter = Task.Delay(150).GetAwaiter();
										if (!taskAwaiter.IsCompleted)
										{
											num = (num2 = 29);
											taskAwaiter2 = taskAwaiter;
											this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, OBDDataReader.<StartLoopV3>d__147>(ref taskAwaiter, ref this);
											return;
										}
									}
									else
									{
										taskAwaiter = taskAwaiter2;
										taskAwaiter2 = default(TaskAwaiter);
										num = (num2 = -1);
									}
									taskAwaiter.GetResult();
									IL_18A9:;
								}
								catch (Exception)
								{
								}
								goto IL_18AE;
							default:
								if (obddataReader.CommandQueue.TryDequeue(out CS$<>8__locals1.request))
								{
									goto IL_051A;
								}
								break;
							}
							try
							{
								if (num != 3)
								{
									if (obddataReader.waitingForQueueSemaphore == null)
									{
										goto IL_03EE;
									}
									SemaphoreSlim waitingForQueueSemaphore2 = obddataReader.waitingForQueueSemaphore;
									if (waitingForQueueSemaphore2 != null)
									{
										waitingForQueueSemaphore2.Release();
									}
									taskAwaiter = Task.Delay(150).GetAwaiter();
									if (!taskAwaiter.IsCompleted)
									{
										num = (num2 = 3);
										taskAwaiter2 = taskAwaiter;
										this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, OBDDataReader.<StartLoopV3>d__147>(ref taskAwaiter, ref this);
										return;
									}
								}
								else
								{
									taskAwaiter = taskAwaiter2;
									taskAwaiter2 = default(TaskAwaiter);
									num = (num2 = -1);
								}
								taskAwaiter.GetResult();
								IL_03EE:;
							}
							catch (Exception)
							{
							}
							if (SharedSettings.Current.AlwaysPingECU && obddataReader.CurrentStatus == OBDDataReaderStatus.ConnectedToECU)
							{
								if ((double)obddataReader.stopwatch.ElapsedMilliseconds - lastPing.TotalMilliseconds > 200.0)
								{
									CS$<>8__locals1.request = pingrequest;
									lastPing = obddataReader.stopwatch.Elapsed;
									goto IL_051A;
								}
								taskAwaiter = Task.Delay(50).GetAwaiter();
								if (!taskAwaiter.IsCompleted)
								{
									num = (num2 = 4);
									taskAwaiter2 = taskAwaiter;
									this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, OBDDataReader.<StartLoopV3>d__147>(ref taskAwaiter, ref this);
									return;
								}
							}
							else
							{
								taskAwaiter = Task.Delay(50).GetAwaiter();
								if (!taskAwaiter.IsCompleted)
								{
									num = (num2 = 5);
									taskAwaiter2 = taskAwaiter;
									this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, OBDDataReader.<StartLoopV3>d__147>(ref taskAwaiter, ref this);
									return;
								}
								goto IL_050E;
							}
							IL_04AD:
							taskAwaiter.GetResult();
							goto IL_1A61;
							IL_050E:
							taskAwaiter.GetResult();
							goto IL_1A61;
							IL_051A:
							if (string.IsNullOrEmpty(CS$<>8__locals1.request.Command))
							{
								goto IL_1A61;
							}
							if (CS$<>8__locals1.request.Repeat)
							{
								obddataReader.CommandQueue.Enqueue(CS$<>8__locals1.request);
							}
							if (CS$<>8__locals1.request.SkipCyclesTarget > 0 && obddataReader.CommandQueue.Count > 1)
							{
								int num3;
								if (CS$<>8__locals1.request.SkippedCycles != 0)
								{
									OBDRequest request = CS$<>8__locals1.request;
									num3 = request.SkippedCycles;
									request.SkippedCycles = num3 + 1;
									if (CS$<>8__locals1.request.SkippedCycles > CS$<>8__locals1.request.SkipCyclesTarget)
									{
										CS$<>8__locals1.request.SkippedCycles = 0;
									}
									goto IL_1A61;
								}
								OBDRequest request2 = CS$<>8__locals1.request;
								num3 = request2.SkippedCycles;
								request2.SkippedCycles = num3 + 1;
							}
							obddataReader.RWCycleEnded = false;
							if ((!(CS$<>8__locals1.request.Header != obddataReader.ELM327_LastSentHeader) || (string.IsNullOrEmpty(CS$<>8__locals1.request.Header) && string.IsNullOrEmpty(obddataReader.ELM327_LastSentHeader))) && (!(CS$<>8__locals1.request.Header == obddataReader.ELM327_LastSentHeader) || obddataReader.ELM327_PendingAfterCommands == null || !obddataReader.ELM327_PendingAfterCommands.Contains("ATCAF1") || CS$<>8__locals1.request.BeforeCommands == null || CS$<>8__locals1.request.BeforeCommands.Contains("ATCAF0") || EnumerableExtensions.IndexOf<string>(obddataReader.ELM327_PendingAfterCommands, "ATCAF0") <= EnumerableExtensions.IndexOf<string>(obddataReader.ELM327_PendingAfterCommands, "ATCAF1")) && (!(CS$<>8__locals1.request.Header == obddataReader.ELM327_LastSentHeader) || obddataReader.ELM327_PendingAfterCommands == null || !obddataReader.ELM327_PendingAfterCommands.Contains("ATAR") || ArrayHelpers.ArrayEquals<string>(obddataReader.ELM327_PendingAfterCommands, CS$<>8__locals1.request.AfterCommands)))
							{
								goto IL_0938;
							}
							if (obddataReader.ELM327_PendingAfterCommands == null || obddataReader.ELM327_PendingAfterCommands.Length == 0)
							{
								goto IL_07E0;
							}
							string[] array = obddataReader.ELM327_PendingAfterCommands.ToArray<string>();
							obddataReader.ELM327_PendingAfterCommands = new string[0];
							valueTaskAwaiter = obddataReader.SendBeforeOrAfterCommands(CS$<>8__locals1.request, array, false).GetAwaiter();
							if (!valueTaskAwaiter.IsCompleted)
							{
								num = (num2 = 6);
								ValueTaskAwaiter valueTaskAwaiter2 = valueTaskAwaiter;
								this.<>t__builder.AwaitUnsafeOnCompleted<ValueTaskAwaiter, OBDDataReader.<StartLoopV3>d__147>(ref valueTaskAwaiter, ref this);
								return;
							}
							IL_07D9:
							valueTaskAwaiter.GetResult();
							IL_07E0:
							string text;
							if (string.IsNullOrEmpty(CS$<>8__locals1.request.Header))
							{
								text = obddataReader.GetDefaultHeader();
								obddataReader.ELM327_LastSentHeader = "";
							}
							else
							{
								text = CS$<>8__locals1.request.Header;
								obddataReader.ELM327_LastSentHeader = text;
							}
							if (CS$<>8__locals1.request.ELMFormat == ELMFormat.VwTp20 && CS$<>8__locals1.request.Header == "000")
							{
								goto IL_0938;
							}
							taskAwaiter = obddataReader.SendString("ATSH" + text).GetAwaiter();
							if (!taskAwaiter.IsCompleted)
							{
								num = (num2 = 7);
								taskAwaiter2 = taskAwaiter;
								this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, OBDDataReader.<StartLoopV3>d__147>(ref taskAwaiter, ref this);
								return;
							}
							IL_08BF:
							taskAwaiter.GetResult();
							taskAwaiter3 = obddataReader.ReadData(2500, null, -1).GetAwaiter();
							if (!taskAwaiter3.IsCompleted)
							{
								num = (num2 = 8);
								TaskAwaiter<string> taskAwaiter4 = taskAwaiter3;
								this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<string>, OBDDataReader.<StartLoopV3>d__147>(ref taskAwaiter3, ref this);
								return;
							}
							IL_0924:
							taskAwaiter3.GetResult();
							obddataReader.ELM327_LastSentBeforeCommands = new string[0];
							IL_0938:
							if (ArrayHelpers.ArrayEquals<string>(obddataReader.ELM327_LastSentBeforeCommands, CS$<>8__locals1.request.BeforeCommands))
							{
								goto IL_0A5B;
							}
							if (CS$<>8__locals1.request.BeforeCommands == null || CS$<>8__locals1.request.BeforeCommands.Length == 0)
							{
								goto IL_0A45;
							}
							bool flag = OBDDataReader.CheckIfNewCommandsContainsAllExistingCommands(obddataReader.ELM327_LastSentBeforeCommands, CS$<>8__locals1.request.BeforeCommands);
							string[] array2 = CS$<>8__locals1.request.BeforeCommands;
							if (flag)
							{
								array2 = CS$<>8__locals1.request.BeforeCommands.Except(obddataReader.ELM327_LastSentBeforeCommands).ToArray<string>();
							}
							valueTaskAwaiter = obddataReader.SendBeforeOrAfterCommands(CS$<>8__locals1.request, array2, true).GetAwaiter();
							if (!valueTaskAwaiter.IsCompleted)
							{
								num = (num2 = 9);
								ValueTaskAwaiter valueTaskAwaiter2 = valueTaskAwaiter;
								this.<>t__builder.AwaitUnsafeOnCompleted<ValueTaskAwaiter, OBDDataReader.<StartLoopV3>d__147>(ref valueTaskAwaiter, ref this);
								return;
							}
							IL_0A3E:
							valueTaskAwaiter.GetResult();
							IL_0A45:
							obddataReader.ELM327_LastSentBeforeCommands = CS$<>8__locals1.request.BeforeCommands;
							IL_0A5B:
							if (CS$<>8__locals1.request.AfterCommands != null && CS$<>8__locals1.request.AfterCommands.Length != 0)
							{
								obddataReader.ELM327_PendingAfterCommands = CS$<>8__locals1.request.AfterCommands;
							}
							requestElmFormat = CS$<>8__locals1.request.ELMFormat;
							if (requestElmFormat == ELMFormat.Unknown)
							{
								requestElmFormat = obddataReader.CurrentELMFormat;
							}
							int num4 = 14;
							if (obddataReader.ELMStatus.ATCEA != "")
							{
								num4 = 12;
							}
							if (CS$<>8__locals1.request.Command.Length > num4 && (requestElmFormat == ELMFormat.CAN11bit || CS$<>8__locals1.request.ELMFormat == ELMFormat.CAN29bit))
							{
								if (CS$<>8__locals1.request is OBDMultiRequest)
								{
									taskAwaiter3 = obddataReader.SendLongCANMultiRequest(CS$<>8__locals1.request as OBDMultiRequest).GetAwaiter();
									if (!taskAwaiter3.IsCompleted)
									{
										num = (num2 = 10);
										TaskAwaiter<string> taskAwaiter4 = taskAwaiter3;
										this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<string>, OBDDataReader.<StartLoopV3>d__147>(ref taskAwaiter3, ref this);
										return;
									}
								}
								else
								{
									taskAwaiter3 = obddataReader.SendLongCanRequest(CS$<>8__locals1.request).GetAwaiter();
									if (!taskAwaiter3.IsCompleted)
									{
										num = (num2 = 11);
										TaskAwaiter<string> taskAwaiter4 = taskAwaiter3;
										this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<string>, OBDDataReader.<StartLoopV3>d__147>(ref taskAwaiter3, ref this);
										return;
									}
									goto IL_0C0A;
								}
							}
							else if (requestElmFormat == ELMFormat.CAN11bit && CS$<>8__locals1.request.ForceManualFlowControl && !string.IsNullOrEmpty(CS$<>8__locals1.request.Header))
							{
								taskAwaiter3 = obddataReader.ReadDataWithManualFlowControl_CAN(CS$<>8__locals1.request).GetAwaiter();
								if (!taskAwaiter3.IsCompleted)
								{
									num = (num2 = 12);
									TaskAwaiter<string> taskAwaiter4 = taskAwaiter3;
									this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<string>, OBDDataReader.<StartLoopV3>d__147>(ref taskAwaiter3, ref this);
									return;
								}
								goto IL_0CBB;
							}
							else if (CS$<>8__locals1.request.ELMFormat == ELMFormat.VwTp20 && CS$<>8__locals1.request.Header == "000")
							{
								valueTaskAwaiter3 = obddataReader.vwTPManager.SendCommand(CS$<>8__locals1.request.Command).GetAwaiter();
								if (!valueTaskAwaiter3.IsCompleted)
								{
									num = (num2 = 13);
									ValueTaskAwaiter<string> valueTaskAwaiter4 = valueTaskAwaiter3;
									this.<>t__builder.AwaitUnsafeOnCompleted<ValueTaskAwaiter<string>, OBDDataReader.<StartLoopV3>d__147>(ref valueTaskAwaiter3, ref this);
									return;
								}
								goto IL_0D77;
							}
							else
							{
								string text2 = OBDRequestQueueOptimizer.SetExpectedCounterForRequestCommand(CS$<>8__locals1.request);
								if (SharedSettings.Current.ExpectedResponseCountOptimization && text2.Length == 4 && text2.StartsWith("10") && CS$<>8__locals1.request.ELMFormat != ELMFormat.VwTp20)
								{
									text2 += "1";
								}
								taskAwaiter = obddataReader.SendString(text2).GetAwaiter();
								if (!taskAwaiter.IsCompleted)
								{
									num = (num2 = 14);
									taskAwaiter2 = taskAwaiter;
									this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, OBDDataReader.<StartLoopV3>d__147>(ref taskAwaiter, ref this);
									return;
								}
								goto IL_0E3B;
							}
							IL_0B91:
							string text3 = taskAwaiter3.GetResult();
							request_result = text3;
							goto IL_0F7C;
							IL_0C0A:
							text3 = taskAwaiter3.GetResult();
							request_result = text3;
							goto IL_0F7C;
							IL_0CBB:
							text3 = taskAwaiter3.GetResult();
							request_result = text3;
							goto IL_0F7C;
							IL_0D77:
							text3 = valueTaskAwaiter3.GetResult();
							request_result = text3;
							goto IL_0F7C;
							IL_0E3B:
							taskAwaiter.GetResult();
							taskAwaiter3 = obddataReader.ReadData(2500, null, CS$<>8__locals1.request.MaxLines).GetAwaiter();
							if (!taskAwaiter3.IsCompleted)
							{
								num = (num2 = 15);
								TaskAwaiter<string> taskAwaiter4 = taskAwaiter3;
								this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<string>, OBDDataReader.<StartLoopV3>d__147>(ref taskAwaiter3, ref this);
								return;
							}
							IL_0EB0:
							text3 = taskAwaiter3.GetResult();
							request_result = text3;
							if (requestElmFormat != ELMFormat.CAN11bit || !CS$<>8__locals1.request.CheckLength || !request_result.Contains("BUFFER FULL"))
							{
								goto IL_0F7C;
							}
							taskAwaiter3 = obddataReader.ReadDataWithManualFlowControl_CAN(CS$<>8__locals1.request).GetAwaiter();
							if (!taskAwaiter3.IsCompleted)
							{
								num = (num2 = 16);
								TaskAwaiter<string> taskAwaiter4 = taskAwaiter3;
								this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<string>, OBDDataReader.<StartLoopV3>d__147>(ref taskAwaiter3, ref this);
								return;
							}
							IL_0F5A:
							text3 = taskAwaiter3.GetResult();
							request_result = text3;
							CS$<>8__locals1.request.ForceManualFlowControl = true;
							IL_0F7C:
							if (!obddataReader.CheckResponseForELM327Reset(CS$<>8__locals1.request, request_result) || !obddataReader.ELMStatus.ELMSupportsATS0)
							{
								goto IL_120C;
							}
							obddataReader.ELMStatus.ELMUnexpectedResetDetected = true;
							taskAwaiter = obddataReader.SendString("ATE0").GetAwaiter();
							if (!taskAwaiter.IsCompleted)
							{
								num = (num2 = 17);
								taskAwaiter2 = taskAwaiter;
								this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, OBDDataReader.<StartLoopV3>d__147>(ref taskAwaiter, ref this);
								return;
							}
							IL_100E:
							taskAwaiter.GetResult();
							taskAwaiter3 = obddataReader.ReadData(2500, null, -1).GetAwaiter();
							if (!taskAwaiter3.IsCompleted)
							{
								num = (num2 = 18);
								TaskAwaiter<string> taskAwaiter4 = taskAwaiter3;
								this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<string>, OBDDataReader.<StartLoopV3>d__147>(ref taskAwaiter3, ref this);
								return;
							}
							IL_1074:
							taskAwaiter3.GetResult();
							taskAwaiter = obddataReader.SendString("ATS0").GetAwaiter();
							if (!taskAwaiter.IsCompleted)
							{
								num = (num2 = 19);
								taskAwaiter2 = taskAwaiter;
								this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, OBDDataReader.<StartLoopV3>d__147>(ref taskAwaiter, ref this);
								return;
							}
							IL_10D6:
							taskAwaiter.GetResult();
							taskAwaiter3 = obddataReader.ReadData(2500, null, -1).GetAwaiter();
							if (!taskAwaiter3.IsCompleted)
							{
								num = (num2 = 20);
								TaskAwaiter<string> taskAwaiter4 = taskAwaiter3;
								this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<string>, OBDDataReader.<StartLoopV3>d__147>(ref taskAwaiter3, ref this);
								return;
							}
							IL_113C:
							taskAwaiter3.GetResult();
							taskAwaiter = obddataReader.SendString("ATH1").GetAwaiter();
							if (!taskAwaiter.IsCompleted)
							{
								num = (num2 = 21);
								taskAwaiter2 = taskAwaiter;
								this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, OBDDataReader.<StartLoopV3>d__147>(ref taskAwaiter, ref this);
								return;
							}
							IL_119E:
							taskAwaiter.GetResult();
							taskAwaiter3 = obddataReader.ReadData(2500, null, -1).GetAwaiter();
							if (!taskAwaiter3.IsCompleted)
							{
								num = (num2 = 22);
								TaskAwaiter<string> taskAwaiter4 = taskAwaiter3;
								this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<string>, OBDDataReader.<StartLoopV3>d__147>(ref taskAwaiter3, ref this);
								return;
							}
							IL_1204:
							taskAwaiter3.GetResult();
							IL_120C:
							if (string.IsNullOrEmpty(request_result) && SharedSettings.Current.ReadPartialErrorAction != ReadPartialErrorActions.ResetConnection)
							{
								OBDRequest request3 = CS$<>8__locals1.request;
								int num3 = request3.FailCounter;
								request3.FailCounter = num3 + 1;
								obddataReader.NO_DATA_Counter++;
								if (CS$<>8__locals1.request is OBDMultiRequest)
								{
									obddataReader.DisassembleFailedMultiRequest(CS$<>8__locals1.request);
								}
							}
							else
							{
								CS$<>8__locals1.request.FailCounter = 0;
							}
							obddataReader.RWCycleEnded = true;
							try
							{
								CS$<>8__locals1.request.OnResponseReceived(request_result);
							}
							catch (Exception)
							{
							}
							bool flag2;
							if (CS$<>8__locals1.request.DoNotDecode)
							{
								if (CS$<>8__locals1.request == pingrequest && obddataReader.CheckNoData(request_result) && obddataReader.NO_DATA_Counter > SharedSettings.Current.NoDataLimit)
								{
									obddataReader.NO_DATA_Counter = 0;
									obddataReader.SetRunning(false, "LOOPV3_NO_DATA_LIMIT_REACHED");
									obddataReader.ReinitializeConnectionToECU("LOOPV3_NO_DATA_LIMIT_REACHED");
									goto IL_1AB4;
								}
								if (obddataReader.CurrentMode != OBDDataReader.OBDModes.ReadDTC && obddataReader.CurrentMode != OBDDataReader.OBDModes.ClearDTC)
								{
									goto IL_1819;
								}
								EventHandler<int> dtcreadingQueueProgress = obddataReader.DTCReadingQueueProgress;
								if (dtcreadingQueueProgress == null)
								{
									goto IL_1819;
								}
								dtcreadingQueueProgress(obddataReader, obddataReader.CommandQueue.Count);
								goto IL_1819;
							}
							else
							{
								OBDDataReader.OBDModes obdmodes = CS$<>8__locals1.request.OBDMode;
								if (obddataReader.CurrentMode != OBDDataReader.OBDModes.Universal)
								{
									obdmodes = obddataReader.CurrentMode;
								}
								switch (obdmodes)
								{
								case OBDDataReader.OBDModes.Mode06:
									taskAwaiter5 = obddataReader.DecodeMode06Data(request_result, CS$<>8__locals1.request.Command).GetAwaiter();
									if (!taskAwaiter5.IsCompleted)
									{
										num = (num2 = 23);
										TaskAwaiter<bool> taskAwaiter6 = taskAwaiter5;
										this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<bool>, OBDDataReader.<StartLoopV3>d__147>(ref taskAwaiter5, ref this);
										return;
									}
									goto IL_1432;
								case OBDDataReader.OBDModes.ReadDTC:
									flag2 = obddataReader.DecodeDTC(request_result, CS$<>8__locals1.request);
									goto IL_1819;
								case OBDDataReader.OBDModes.ClearDTC:
									goto IL_1819;
								case OBDDataReader.OBDModes.Terminal:
									obddataReader.PushDataToTerminalPage(request_result);
									goto IL_1819;
								}
								if (obddataReader.CheckNoData(request_result))
								{
									obddataReader.DisassembleFailedMultiRequest(CS$<>8__locals1.request);
									if (obddataReader.NO_DATA_Counter > SharedSettings.Current.NoDataLimit)
									{
										obddataReader.NO_DATA_Counter = 0;
										obddataReader.SetRunning(false, string.Format("LOOPV3_NO_DATA_LIMIT_REACHED, LOOP_ID {0}", local_loopId));
										obddataReader.ReinitializeConnectionToECU("LOOPV3_NO_DATA_LIMIT_REACHED");
										goto IL_1AB4;
									}
								}
								if (CS$<>8__locals1.request.Command.StartsWith("06", StringComparison.Ordinal))
								{
									taskAwaiter5 = obddataReader.DecodeMode06Data(request_result, CS$<>8__locals1.request.Command).GetAwaiter();
									if (!taskAwaiter5.IsCompleted)
									{
										num = (num2 = 24);
										TaskAwaiter<bool> taskAwaiter6 = taskAwaiter5;
										this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<bool>, OBDDataReader.<StartLoopV3>d__147>(ref taskAwaiter5, ref this);
										return;
									}
									goto IL_1530;
								}
								else if (CS$<>8__locals1.request.Command == "ATRV")
								{
									obddataReader.ParseATRV(CS$<>8__locals1.request, request_result);
									if (!SharedSettings.Current.AlwaysPingECU || !CS$<>8__locals1.request.Repeat || obddataReader.CommandQueue.Count != 1)
									{
										goto IL_1819;
									}
									taskAwaiter = obddataReader.SendString(ping_cmd).GetAwaiter();
									if (!taskAwaiter.IsCompleted)
									{
										num = (num2 = 25);
										taskAwaiter2 = taskAwaiter;
										this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, OBDDataReader.<StartLoopV3>d__147>(ref taskAwaiter, ref this);
										return;
									}
									goto IL_1604;
								}
								else if (CS$<>8__locals1.request.ELMFormat == ELMFormat.VwTp20 && CS$<>8__locals1.request.Header == "000")
								{
									valueTaskAwaiter5 = obddataReader.DecodeVWTPData(request_result, CS$<>8__locals1.request).GetAwaiter();
									if (!valueTaskAwaiter5.IsCompleted)
									{
										num = (num2 = 27);
										ValueTaskAwaiter<bool> valueTaskAwaiter6 = valueTaskAwaiter5;
										this.<>t__builder.AwaitUnsafeOnCompleted<ValueTaskAwaiter<bool>, OBDDataReader.<StartLoopV3>d__147>(ref valueTaskAwaiter5, ref this);
										return;
									}
									goto IL_1720;
								}
								else
								{
									valueTaskAwaiter5 = obddataReader.DecodeData(request_result, CS$<>8__locals1.request, null).GetAwaiter();
									if (!valueTaskAwaiter5.IsCompleted)
									{
										num = (num2 = 28);
										ValueTaskAwaiter<bool> valueTaskAwaiter6 = valueTaskAwaiter5;
										this.<>t__builder.AwaitUnsafeOnCompleted<ValueTaskAwaiter<bool>, OBDDataReader.<StartLoopV3>d__147>(ref valueTaskAwaiter5, ref this);
										return;
									}
									goto IL_1799;
								}
							}
							IL_1432:
							flag2 = taskAwaiter5.GetResult();
							goto IL_1819;
							IL_1530:
							taskAwaiter5.GetResult();
							goto IL_1819;
							IL_1604:
							taskAwaiter.GetResult();
							taskAwaiter3 = obddataReader.ReadData(2500, null, -1).GetAwaiter();
							if (!taskAwaiter3.IsCompleted)
							{
								num = (num2 = 26);
								TaskAwaiter<string> taskAwaiter4 = taskAwaiter3;
								this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<string>, OBDDataReader.<StartLoopV3>d__147>(ref taskAwaiter3, ref this);
								return;
							}
							IL_166A:
							string result = taskAwaiter3.GetResult();
							obddataReader.CheckNoData(result);
							goto IL_1819;
							IL_1720:
							flag2 = valueTaskAwaiter5.GetResult();
							goto IL_17A2;
							IL_1799:
							flag2 = valueTaskAwaiter5.GetResult();
							IL_17A2:
							if (flag2)
							{
								CS$<>8__locals1.request.FailCounter = 0;
							}
							if (CS$<>8__locals1.request is OBDMultiRequest)
							{
								if (flag2 && SharedSettings.Current.OptimizedRequestStuckCounter > 0)
								{
									SharedSettings.Current.OptimizedRequestStuckCounter = 0;
								}
								else if (!flag2 && SharedSettings.Current.DisableOptimizationIfItFails)
								{
									CS$<>8__locals1.request.FailCounter = 2;
									obddataReader.DisassembleFailedMultiRequest(CS$<>8__locals1.request);
								}
							}
							IL_1819:
							if (obddataReader.CommandQueue.Count == 0)
							{
								goto IL_1829;
							}
							IL_18AE:
							request_result = null;
						}
						catch (Exception ex)
						{
							obddataReader.SetRunning(false, "StartLoopV3_ReadDataTimeoutException");
							if (CS$<>8__locals1.request.ELMFormat == ELMFormat.VwTp20 && CS$<>8__locals1.request.Header != "000")
							{
								CS$<>8__locals1.request.OnResponseReceived("ELMFAILTP20");
							}
							if (ex is ReadDataTimeoutException && CS$<>8__locals1.request is OBDMultiRequest)
							{
								obddataReader.DisassembleFailedMultiRequest(CS$<>8__locals1.request);
							}
							CS$<>8__locals1.request.FailCounter = CS$<>8__locals1.request.FailCounter + 1;
							OBDRequest obdrequest;
							if (CS$<>8__locals1.request.FailCounter >= 2 && obddataReader.CommandQueue.TryPeek(out obdrequest) && CS$<>8__locals1.request == obdrequest)
							{
								List<OBDRequest> list = obddataReader.CommandQueue.ToList<OBDRequest>();
								List<OBDRequest> list2 = list;
								Predicate<OBDRequest> predicate;
								if ((predicate = CS$<>8__locals1.<>9__0) == null)
								{
									predicate = (CS$<>8__locals1.<>9__0 = (OBDRequest x) => x == CS$<>8__locals1.request);
								}
								list2.RemoveAll(predicate);
								obddataReader.ReplaceQueue(list);
							}
							else if (CS$<>8__locals1.request != pingrequest && CS$<>8__locals1.request.FailCounter < 2)
							{
								List<OBDRequest> list3 = obddataReader.CommandQueue.ToList<OBDRequest>();
								if (!list3.Contains(CS$<>8__locals1.request))
								{
									list3.Insert(0, CS$<>8__locals1.request);
									obddataReader.ReplaceQueue(list3);
								}
							}
							if (local_loopId == obddataReader.loopId)
							{
								obddataReader.lastTimeConnected = obddataReader.stopwatch.ElapsedTicks;
								obddataReader.OnDisconnectDetected(OBDDataReader.DisconnectReason.ELMStuck);
							}
							goto IL_1AB4;
						}
						goto IL_1A61;
					default:
						CS$<>8__locals1 = new OBDDataReader.<>c__DisplayClass147_0();
						lastPing = TimeSpan.Zero;
						if (SharedSettings.Current.UseDefaultInit)
						{
							if (obddataReader.IsNissanConsult2Protocol)
							{
								ping_cmd = "221201";
							}
							else
							{
								ping_cmd = SharedSettings.Current.Mode01Prefix + "00";
							}
						}
						else
						{
							ping_cmd = SharedSettings.Current.TesterPresentCommand;
							if (string.IsNullOrEmpty(ping_cmd))
							{
								ping_cmd = obddataReader.GetDefaultPidCommand();
							}
						}
						pingrequest = new OBDRequest(ping_cmd, "", "", "", false, new List<PID>(0))
						{
							DoNotDecode = true
						};
						CS$<>8__locals1.request = null;
						if (!obddataReader.Running)
						{
							obddataReader.SetRunning(true, "From Start #2518");
							goto IL_022F;
						}
						taskAwaiter = Task.Delay(1000).GetAwaiter();
						if (!taskAwaiter.IsCompleted)
						{
							num = (num2 = 0);
							TaskAwaiter taskAwaiter2 = taskAwaiter;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, OBDDataReader.<StartLoopV3>d__147>(ref taskAwaiter, ref this);
							return;
						}
						break;
					}
					taskAwaiter.GetResult();
					if (!obddataReader.Running && !obddataReader.DisconnectRequested)
					{
						goto IL_022F;
					}
					taskAwaiter = obddataReader.DebugWrite("\r\n[STARTLOOP_ALREADY_RUNNING. STOPPING START_LOOP_ID]").GetAwaiter();
					if (!taskAwaiter.IsCompleted)
					{
						num = (num2 = 1);
						TaskAwaiter taskAwaiter2 = taskAwaiter;
						this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, OBDDataReader.<StartLoopV3>d__147>(ref taskAwaiter, ref this);
						return;
					}
					IL_0217:
					taskAwaiter.GetResult();
					goto IL_1AB4;
					IL_022F:
					local_loopId = new Random().Next();
					obddataReader.loopId = local_loopId;
					goto IL_1A61;
					IL_02C3:
					taskAwaiter.GetResult();
					goto IL_1A84;
					IL_1A61:
					if (obddataReader.Running && !obddataReader.DisconnectRequested && local_loopId == obddataReader.loopId)
					{
						if (obddataReader.DisconnectRequested)
						{
							taskAwaiter = obddataReader.DebugWrite(string.Format("\r\n[START_LOOP:DisconnectRequested. STOPPING START_LOOP_ID {0}]", local_loopId)).GetAwaiter();
							if (!taskAwaiter.IsCompleted)
							{
								num = (num2 = 2);
								TaskAwaiter taskAwaiter2 = taskAwaiter;
								this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, OBDDataReader.<StartLoopV3>d__147>(ref taskAwaiter, ref this);
								return;
							}
							goto IL_02C3;
						}
						else if (local_loopId == obddataReader.loopId)
						{
							goto IL_02E0;
						}
					}
					IL_1A84:;
				}
				catch (Exception ex2)
				{
					num2 = -2;
					CS$<>8__locals1 = null;
					ping_cmd = null;
					pingrequest = null;
					this.<>t__builder.SetException(ex2);
					return;
				}
				IL_1AB4:
				num2 = -2;
				CS$<>8__locals1 = null;
				ping_cmd = null;
				pingrequest = null;
				this.<>t__builder.SetResult();
			}

			// Token: 0x06002641 RID: 9793 RVA: 0x001DB8A0 File Offset: 0x001D9AA0
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x040014BA RID: 5306
			public int <>1__state;

			// Token: 0x040014BB RID: 5307
			public AsyncVoidMethodBuilder <>t__builder;

			// Token: 0x040014BC RID: 5308
			public OBDDataReader <>4__this;

			// Token: 0x040014BD RID: 5309
			private OBDDataReader.<>c__DisplayClass147_0 <>8__1;

			// Token: 0x040014BE RID: 5310
			private TimeSpan <lastPing>5__2;

			// Token: 0x040014BF RID: 5311
			private string <ping_cmd>5__3;

			// Token: 0x040014C0 RID: 5312
			private OBDRequest <pingrequest>5__4;

			// Token: 0x040014C1 RID: 5313
			private int <local_loopId>5__5;

			// Token: 0x040014C2 RID: 5314
			private TaskAwaiter <>u__1;

			// Token: 0x040014C3 RID: 5315
			private string <request_result>5__6;

			// Token: 0x040014C4 RID: 5316
			private ELMFormat <requestElmFormat>5__7;

			// Token: 0x040014C5 RID: 5317
			private ValueTaskAwaiter <>u__2;

			// Token: 0x040014C6 RID: 5318
			private TaskAwaiter<string> <>u__3;

			// Token: 0x040014C7 RID: 5319
			private ValueTaskAwaiter<string> <>u__4;

			// Token: 0x040014C8 RID: 5320
			private TaskAwaiter<bool> <>u__5;

			// Token: 0x040014C9 RID: 5321
			private ValueTaskAwaiter<bool> <>u__6;
		}

		// Token: 0x02000388 RID: 904
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <Stop>d__183 : IAsyncStateMachine
		{
			// Token: 0x06002642 RID: 9794 RVA: 0x001DB8B0 File Offset: 0x001D9AB0
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				OBDDataReader obddataReader = this;
				try
				{
					TaskAwaiter taskAwaiter;
					TaskAwaiter<Task> taskAwaiter3;
					switch (num)
					{
					case 0:
					{
						TaskAwaiter taskAwaiter2;
						taskAwaiter = taskAwaiter2;
						taskAwaiter2 = default(TaskAwaiter);
						num2 = -1;
						break;
					}
					case 1:
					{
						TaskAwaiter<Task> taskAwaiter4;
						taskAwaiter3 = taskAwaiter4;
						taskAwaiter4 = default(TaskAwaiter<Task>);
						num2 = -1;
						goto IL_0129;
					}
					case 2:
					{
						TaskAwaiter taskAwaiter2;
						taskAwaiter = taskAwaiter2;
						taskAwaiter2 = default(TaskAwaiter);
						num2 = -1;
						goto IL_018A;
					}
					default:
						taskAwaiter = obddataReader.DebugWrite("\nStop(" + message + ")\n").GetAwaiter();
						if (!taskAwaiter.IsCompleted)
						{
							num2 = 0;
							TaskAwaiter taskAwaiter2 = taskAwaiter;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, OBDDataReader.<Stop>d__183>(ref taskAwaiter, ref this);
							return;
						}
						break;
					}
					taskAwaiter.GetResult();
					obddataReader.SetRunning(false, "From Stop #3640");
					Task task = new Task(delegate
					{
						OBDDataReader.<<Stop>b__183_0>d <<Stop>b__183_0>d;
						<<Stop>b__183_0>d.<>t__builder = AsyncVoidMethodBuilder.Create();
						<<Stop>b__183_0>d.<>4__this = obddataReader;
						<<Stop>b__183_0>d.<>1__state = -1;
						<<Stop>b__183_0>d.<>t__builder.Start<OBDDataReader.<<Stop>b__183_0>d>(ref <<Stop>b__183_0>d);
					});
					Task task2 = Task.Delay(TimeSpan.FromSeconds(2.0));
					taskAwaiter3 = Task.WhenAny(new Task[] { task, task2 }).GetAwaiter();
					if (!taskAwaiter3.IsCompleted)
					{
						num2 = 1;
						TaskAwaiter<Task> taskAwaiter4 = taskAwaiter3;
						this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<Task>, OBDDataReader.<Stop>d__183>(ref taskAwaiter3, ref this);
						return;
					}
					IL_0129:
					taskAwaiter3.GetResult();
					taskAwaiter = obddataReader.DebugWrite("\nStopped\n").GetAwaiter();
					if (!taskAwaiter.IsCompleted)
					{
						num2 = 2;
						TaskAwaiter taskAwaiter2 = taskAwaiter;
						this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, OBDDataReader.<Stop>d__183>(ref taskAwaiter, ref this);
						return;
					}
					IL_018A:
					taskAwaiter.GetResult();
				}
				catch (Exception ex)
				{
					num2 = -2;
					this.<>t__builder.SetException(ex);
					return;
				}
				num2 = -2;
				this.<>t__builder.SetResult();
			}

			// Token: 0x06002643 RID: 9795 RVA: 0x001DBA98 File Offset: 0x001D9C98
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x040014CA RID: 5322
			public int <>1__state;

			// Token: 0x040014CB RID: 5323
			public AsyncTaskMethodBuilder <>t__builder;

			// Token: 0x040014CC RID: 5324
			public OBDDataReader <>4__this;

			// Token: 0x040014CD RID: 5325
			public string message;

			// Token: 0x040014CE RID: 5326
			private TaskAwaiter <>u__1;

			// Token: 0x040014CF RID: 5327
			private TaskAwaiter<Task> <>u__2;
		}

		// Token: 0x02000389 RID: 905
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <WaitForCommandQueue>d__112 : IAsyncStateMachine
		{
			// Token: 0x06002644 RID: 9796 RVA: 0x001DBAA8 File Offset: 0x001D9CA8
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				OBDDataReader obddataReader = this;
				try
				{
					TaskAwaiter taskAwaiter;
					if (num != 0)
					{
						obddataReader.waitingForQueueSemaphore = new SemaphoreSlim(0, 1);
						taskAwaiter = obddataReader.waitingForQueueSemaphore.WaitAsync().GetAwaiter();
						if (!taskAwaiter.IsCompleted)
						{
							num2 = 0;
							TaskAwaiter taskAwaiter2 = taskAwaiter;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, OBDDataReader.<WaitForCommandQueue>d__112>(ref taskAwaiter, ref this);
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
					obddataReader.waitingForQueueSemaphore = null;
				}
				catch (Exception ex)
				{
					num2 = -2;
					this.<>t__builder.SetException(ex);
					return;
				}
				num2 = -2;
				this.<>t__builder.SetResult();
			}

			// Token: 0x06002645 RID: 9797 RVA: 0x001DBB74 File Offset: 0x001D9D74
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x040014D0 RID: 5328
			public int <>1__state;

			// Token: 0x040014D1 RID: 5329
			public AsyncTaskMethodBuilder <>t__builder;

			// Token: 0x040014D2 RID: 5330
			public OBDDataReader <>4__this;

			// Token: 0x040014D3 RID: 5331
			private TaskAwaiter <>u__1;
		}
	}
}
