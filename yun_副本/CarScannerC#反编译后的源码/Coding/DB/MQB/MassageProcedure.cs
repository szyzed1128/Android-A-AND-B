using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using CarScannerXamarinForms.OBD2;

namespace CarScannerXamarinForms.Coding.DB.MQB
{
	// Token: 0x02000AE9 RID: 2793
	internal class MassageProcedure : IServiceProcedure, ICodingContainer, INotifyPropertyChanged
	{
		// Token: 0x0600579C RID: 22428 RVA: 0x0041C9C0 File Offset: 0x0041ABC0
		public static MassageProcedure MassageRoutineDriverSeat()
		{
			return new MassageProcedure(Translate.GetString("mqb_MassageDriver"), Translate.GetString("mqb_MassageDescription"), "", "74C", "7B6");
		}

		// Token: 0x0600579D RID: 22429 RVA: 0x0041C9EA File Offset: 0x0041ABEA
		public static MassageProcedure MassageRoutinePassengerSeat()
		{
			return new MassageProcedure(Translate.GetString("mqb_MassagePassenger"), Translate.GetString("mqb_MassageDescription"), "", "74D", "7B7");
		}

		// Token: 0x0600579E RID: 22430 RVA: 0x0041CA14 File Offset: 0x0041AC14
		protected MassageProcedure(string name, string description, string innerDescription, string requestHeader, string responseHeader)
		{
			this.Name = name;
			this.Description = description;
			this.InnerDescription = innerDescription;
			this.RequestHeader = requestHeader;
			this.ResponseHeader = responseHeader;
			MQBAdaptationOption mqbadaptationOption = new MQBAdaptationOption(Translate.GetString("coding_Start"), "03");
			MQBAdaptationOption mqbadaptationOption2 = new MQBAdaptationOption(Translate.GetString("coding_Stop"), "00");
			this.Options.Add(mqbadaptationOption);
			this.Options.Add(mqbadaptationOption2);
		}

		// Token: 0x17001812 RID: 6162
		// (get) Token: 0x0600579F RID: 22431 RVA: 0x0001941D File Offset: 0x0001761D
		// (set) Token: 0x060057A0 RID: 22432 RVA: 0x000027D4 File Offset: 0x000009D4
		public CodingGroup Group
		{
			get
			{
				return CodingGroup.Seats;
			}
			set
			{
			}
		}

		// Token: 0x17001813 RID: 6163
		// (get) Token: 0x060057A1 RID: 22433 RVA: 0x0041CABA File Offset: 0x0041ACBA
		// (set) Token: 0x060057A2 RID: 22434 RVA: 0x0041CAC2 File Offset: 0x0041ACC2
		public virtual string Name
		{
			[CompilerGenerated]
			get
			{
				return this.<Name>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<Name>k__BackingField = value;
			}
		}

		// Token: 0x17001814 RID: 6164
		// (get) Token: 0x060057A3 RID: 22435 RVA: 0x0041CACB File Offset: 0x0041ACCB
		// (set) Token: 0x060057A4 RID: 22436 RVA: 0x0041CAD3 File Offset: 0x0041ACD3
		public string Description
		{
			[CompilerGenerated]
			get
			{
				return this.<Description>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<Description>k__BackingField = value;
			}
		}

		// Token: 0x17001815 RID: 6165
		// (get) Token: 0x060057A5 RID: 22437 RVA: 0x0041CADC File Offset: 0x0041ACDC
		// (set) Token: 0x060057A6 RID: 22438 RVA: 0x0041CAE4 File Offset: 0x0041ACE4
		public string InnerDescription
		{
			[CompilerGenerated]
			get
			{
				return this.<InnerDescription>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<InnerDescription>k__BackingField = value;
			}
		}

		// Token: 0x17001816 RID: 6166
		// (get) Token: 0x060057A7 RID: 22439 RVA: 0x0041CAED File Offset: 0x0041ACED
		// (set) Token: 0x060057A8 RID: 22440 RVA: 0x0041CAF5 File Offset: 0x0041ACF5
		public string CurrentState
		{
			get
			{
				return this._CurrentState;
			}
			private set
			{
				this._CurrentState = value;
				PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
				if (propertyChanged == null)
				{
					return;
				}
				propertyChanged(this, new PropertyChangedEventArgs("CurrentState"));
			}
		}

		// Token: 0x17001817 RID: 6167
		// (get) Token: 0x060057A9 RID: 22441 RVA: 0x00002076 File Offset: 0x00000276
		public bool PasswordVisible
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17001818 RID: 6168
		// (get) Token: 0x060057AA RID: 22442 RVA: 0x000A8D6F File Offset: 0x000A6F6F
		// (set) Token: 0x060057AB RID: 22443 RVA: 0x000027D4 File Offset: 0x000009D4
		public bool HasCurrentState
		{
			get
			{
				return true;
			}
			set
			{
			}
		}

		// Token: 0x17001819 RID: 6169
		// (get) Token: 0x060057AC RID: 22444 RVA: 0x001ECB01 File Offset: 0x001EAD01
		// (set) Token: 0x060057AD RID: 22445 RVA: 0x000027D4 File Offset: 0x000009D4
		public string Password
		{
			get
			{
				return "";
			}
			set
			{
			}
		}

		// Token: 0x1700181A RID: 6170
		// (get) Token: 0x060057AE RID: 22446 RVA: 0x001ECB01 File Offset: 0x001EAD01
		public string PasswordHint
		{
			get
			{
				return "";
			}
		}

		// Token: 0x1700181B RID: 6171
		// (get) Token: 0x060057AF RID: 22447 RVA: 0x0041CB19 File Offset: 0x0041AD19
		// (set) Token: 0x060057B0 RID: 22448 RVA: 0x0041CB21 File Offset: 0x0041AD21
		public ObservableCollection<MQBAdaptationOption> Options
		{
			[CompilerGenerated]
			get
			{
				return this.<Options>k__BackingField;
			}
			[CompilerGenerated]
			private set
			{
				this.<Options>k__BackingField = value;
			}
		} = new ObservableCollection<MQBAdaptationOption>();

		// Token: 0x1700181C RID: 6172
		// (get) Token: 0x060057B1 RID: 22449 RVA: 0x00002076 File Offset: 0x00000276
		// (set) Token: 0x060057B2 RID: 22450 RVA: 0x000027D4 File Offset: 0x000009D4
		public bool RequiresPro
		{
			get
			{
				return false;
			}
			set
			{
			}
		}

		// Token: 0x1700181D RID: 6173
		// (get) Token: 0x060057B3 RID: 22451 RVA: 0x00002076 File Offset: 0x00000276
		public AdaptationValueTypes ValueType
		{
			get
			{
				return AdaptationValueTypes.OptionType;
			}
		}

		// Token: 0x14000066 RID: 102
		// (add) Token: 0x060057B4 RID: 22452 RVA: 0x0041CB2C File Offset: 0x0041AD2C
		// (remove) Token: 0x060057B5 RID: 22453 RVA: 0x0041CB64 File Offset: 0x0041AD64
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

		// Token: 0x060057B6 RID: 22454 RVA: 0x0041CB9C File Offset: 0x0041AD9C
		public async Task<CodingRequestResult> Execute(string password, string value, string UserFriendlyValue, IProgress<string> progress, byte[] originalData = null, bool skipIfTheSameData = false)
		{
			CodingRequestResult codingRequestResult;
			if (value == "03")
			{
				TaskAwaiter<bool> taskAwaiter = this.StartLoop(progress).GetAwaiter();
				if (!taskAwaiter.IsCompleted)
				{
					await taskAwaiter;
					TaskAwaiter<bool> taskAwaiter2;
					taskAwaiter = taskAwaiter2;
					taskAwaiter2 = default(TaskAwaiter<bool>);
				}
				if (taskAwaiter.GetResult())
				{
					codingRequestResult = CodingRequestResult.Success;
				}
				else
				{
					codingRequestResult = CodingRequestResult.NotSupported;
				}
			}
			else
			{
				this.IsRunning = false;
				codingRequestResult = CodingRequestResult.Success;
			}
			return codingRequestResult;
		}

		// Token: 0x060057B7 RID: 22455 RVA: 0x0041CBF0 File Offset: 0x0041ADF0
		public async Task<CodingRequestResult> UpdateCurrentState(string password, IProgress<string> progress = null)
		{
			if (MassageProcedure.runningInstance != null)
			{
				MassageProcedure.runningInstance.IsRunning = false;
				MassageProcedure.runningInstance = null;
			}
			SemaphoreSlim semaphore = new SemaphoreSlim(0, 1);
			OBDRequest obdrequest = this.BuildCheckStatusRequest();
			bool supported = false;
			obdrequest.ResponseReceived += delegate(OBDRequest request, string data)
			{
				if (string.IsNullOrEmpty(data) || !data.Contains(request.ResponseMarker))
				{
					supported = false;
				}
				else
				{
					supported = true;
				}
				request.DoNotDecode = true;
				App.OBDReader.ReplaceQueue(new OBDRequest[0]);
				semaphore.Release();
			};
			App.OBDReader.ReplaceQueue(new OBDRequest[] { obdrequest });
			await semaphore.WaitAsync();
			CodingRequestResult codingRequestResult;
			if (!supported)
			{
				this.CurrentState = MQBAdaptationTemplate.UNSUPPORTED_TITLE;
				codingRequestResult = CodingRequestResult.NotSupported;
			}
			else
			{
				this.CurrentState = "";
				codingRequestResult = CodingRequestResult.Success;
			}
			return codingRequestResult;
		}

		// Token: 0x1700181E RID: 6174
		// (get) Token: 0x060057B8 RID: 22456 RVA: 0x0041CC33 File Offset: 0x0041AE33
		// (set) Token: 0x060057B9 RID: 22457 RVA: 0x0041CC3B File Offset: 0x0041AE3B
		protected string RequestHeader
		{
			[CompilerGenerated]
			get
			{
				return this.<RequestHeader>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<RequestHeader>k__BackingField = value;
			}
		} = "";

		// Token: 0x1700181F RID: 6175
		// (get) Token: 0x060057BA RID: 22458 RVA: 0x0041CC44 File Offset: 0x0041AE44
		// (set) Token: 0x060057BB RID: 22459 RVA: 0x0041CC4C File Offset: 0x0041AE4C
		protected string ResponseHeader
		{
			[CompilerGenerated]
			get
			{
				return this.<ResponseHeader>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<ResponseHeader>k__BackingField = value;
			}
		} = "";

		// Token: 0x060057BC RID: 22460 RVA: 0x0041CC58 File Offset: 0x0041AE58
		protected async Task<bool> SendCommandAndWaitForStatus(OBDRequest commandRequest, OBDRequest checkStatusRequest, Func<byte[], bool> waitForNextStatusUpdateDelegate)
		{
			SemaphoreSlim semaphore = new SemaphoreSlim(0, 1);
			commandRequest.Repeat = false;
			checkStatusRequest.Repeat = false;
			bool result = false;
			ResponseReceivedDelegate responseReceivedDelegate = delegate(OBDRequest request, string data)
			{
				if (string.IsNullOrEmpty(data))
				{
					result = false;
					request.DoNotDecode = true;
					App.OBDReader.ReplaceQueue(new OBDRequest[0]);
					semaphore.Release();
				}
				if (!data.Contains(request.ResponseMarker))
				{
					result = false;
					App.OBDReader.ReplaceQueue(new OBDRequest[0]);
					semaphore.Release();
				}
			};
			checkStatusRequest.ResponseReceived -= responseReceivedDelegate;
			checkStatusRequest.ResponseReceived += responseReceivedDelegate;
			checkStatusRequest.ResponseDecoded += delegate(OBDRequest request, byte[] data, bool decodeResult, string responseHeader)
			{
				if (data != null && data.Length != 0)
				{
					if (waitForNextStatusUpdateDelegate(data))
					{
						result = true;
						semaphore.Release();
						return;
					}
					App.OBDReader.AddRequestToQueue(checkStatusRequest);
				}
			};
			commandRequest.ResponseReceived -= responseReceivedDelegate;
			commandRequest.ResponseReceived += responseReceivedDelegate;
			App.OBDReader.ReplaceQueue(new OBDRequest[] { commandRequest, checkStatusRequest });
			await semaphore.WaitAsync();
			return result;
		}

		// Token: 0x060057BD RID: 22461 RVA: 0x0041CCAB File Offset: 0x0041AEAB
		private bool checkForStatusCompleted(byte[] data)
		{
			return data != null && data.Length != 0 && data[0] != 192;
		}

		// Token: 0x060057BE RID: 22462 RVA: 0x0041CCC3 File Offset: 0x0041AEC3
		private OBDRequest BuildRequest(string command)
		{
			return new OBDRequest(command, this.RequestHeader, "1003;ATCRA" + this.ResponseHeader, "ATAR", false);
		}

		// Token: 0x060057BF RID: 22463 RVA: 0x0041CCE7 File Offset: 0x0041AEE7
		private OBDRequest BuildCheckStatusRequest()
		{
			return this.BuildRequest("220100");
		}

		// Token: 0x060057C0 RID: 22464 RVA: 0x0041CCF4 File Offset: 0x0041AEF4
		private OBDRequest BuildRequestForMovement(MassageProcedure.MovementDirections direction, int durationSeconds)
		{
			string text = "2F";
			if (direction > MassageProcedure.MovementDirections.Backward)
			{
				if (direction - MassageProcedure.MovementDirections.Up <= 1)
				{
					text += "09A8";
				}
			}
			else
			{
				text += "09A7";
			}
			text += "03";
			text += (durationSeconds & 255).ToString("X2");
			switch (direction)
			{
			case MassageProcedure.MovementDirections.Forward:
			case MassageProcedure.MovementDirections.Up:
				text += "00";
				break;
			case MassageProcedure.MovementDirections.Backward:
			case MassageProcedure.MovementDirections.Down:
				text += "01";
				break;
			}
			return this.BuildRequest(text);
		}

		// Token: 0x060057C1 RID: 22465 RVA: 0x0041CD8C File Offset: 0x0041AF8C
		private async Task<bool> MoveToDirection(MassageProcedure.MovementDirections direction, int durationSeconds, IProgress<string> progress)
		{
			OBDRequest obdrequest = this.BuildRequestForMovement(direction, durationSeconds);
			OBDRequest obdrequest2 = this.BuildCheckStatusRequest();
			if (progress != null)
			{
				progress.Report(string.Format("Moving to: {0} ({1})", direction, durationSeconds));
			}
			return await this.SendCommandAndWaitForStatus(obdrequest, obdrequest2, new Func<byte[], bool>(this.checkForStatusCompleted));
		}

		// Token: 0x060057C2 RID: 22466 RVA: 0x0041CDE8 File Offset: 0x0041AFE8
		private async Task<bool> StartLoop(IProgress<string> progress)
		{
			bool result = true;
			TimeSpan pauseBetween = TimeSpan.FromSeconds(0.7);
			List<ValueTuple<MassageProcedure.MovementDirections, int>> list = new List<ValueTuple<MassageProcedure.MovementDirections, int>>
			{
				new ValueTuple<MassageProcedure.MovementDirections, int>(MassageProcedure.MovementDirections.Up, 5),
				new ValueTuple<MassageProcedure.MovementDirections, int>(MassageProcedure.MovementDirections.Forward, 5),
				new ValueTuple<MassageProcedure.MovementDirections, int>(MassageProcedure.MovementDirections.Backward, 5),
				new ValueTuple<MassageProcedure.MovementDirections, int>(MassageProcedure.MovementDirections.Forward, 5),
				new ValueTuple<MassageProcedure.MovementDirections, int>(MassageProcedure.MovementDirections.Down, 5),
				new ValueTuple<MassageProcedure.MovementDirections, int>(MassageProcedure.MovementDirections.Backward, 5),
				new ValueTuple<MassageProcedure.MovementDirections, int>(MassageProcedure.MovementDirections.Forward, 5),
				new ValueTuple<MassageProcedure.MovementDirections, int>(MassageProcedure.MovementDirections.Up, 5),
				new ValueTuple<MassageProcedure.MovementDirections, int>(MassageProcedure.MovementDirections.Backward, 5),
				new ValueTuple<MassageProcedure.MovementDirections, int>(MassageProcedure.MovementDirections.Down, 5),
				new ValueTuple<MassageProcedure.MovementDirections, int>(MassageProcedure.MovementDirections.Forward, 5),
				new ValueTuple<MassageProcedure.MovementDirections, int>(MassageProcedure.MovementDirections.Up, 5),
				new ValueTuple<MassageProcedure.MovementDirections, int>(MassageProcedure.MovementDirections.Down, 5),
				new ValueTuple<MassageProcedure.MovementDirections, int>(MassageProcedure.MovementDirections.Up, 1),
				new ValueTuple<MassageProcedure.MovementDirections, int>(MassageProcedure.MovementDirections.Backward, 5),
				new ValueTuple<MassageProcedure.MovementDirections, int>(MassageProcedure.MovementDirections.Forward, 5),
				new ValueTuple<MassageProcedure.MovementDirections, int>(MassageProcedure.MovementDirections.Backward, 5),
				new ValueTuple<MassageProcedure.MovementDirections, int>(MassageProcedure.MovementDirections.Forward, 5),
				new ValueTuple<MassageProcedure.MovementDirections, int>(MassageProcedure.MovementDirections.Backward, 5),
				new ValueTuple<MassageProcedure.MovementDirections, int>(MassageProcedure.MovementDirections.Forward, 5),
				new ValueTuple<MassageProcedure.MovementDirections, int>(MassageProcedure.MovementDirections.Up, 1),
				new ValueTuple<MassageProcedure.MovementDirections, int>(MassageProcedure.MovementDirections.Backward, 5),
				new ValueTuple<MassageProcedure.MovementDirections, int>(MassageProcedure.MovementDirections.Forward, 5),
				new ValueTuple<MassageProcedure.MovementDirections, int>(MassageProcedure.MovementDirections.Backward, 5),
				new ValueTuple<MassageProcedure.MovementDirections, int>(MassageProcedure.MovementDirections.Forward, 5),
				new ValueTuple<MassageProcedure.MovementDirections, int>(MassageProcedure.MovementDirections.Backward, 5),
				new ValueTuple<MassageProcedure.MovementDirections, int>(MassageProcedure.MovementDirections.Forward, 5),
				new ValueTuple<MassageProcedure.MovementDirections, int>(MassageProcedure.MovementDirections.Up, 1),
				new ValueTuple<MassageProcedure.MovementDirections, int>(MassageProcedure.MovementDirections.Backward, 5),
				new ValueTuple<MassageProcedure.MovementDirections, int>(MassageProcedure.MovementDirections.Forward, 5),
				new ValueTuple<MassageProcedure.MovementDirections, int>(MassageProcedure.MovementDirections.Backward, 5),
				new ValueTuple<MassageProcedure.MovementDirections, int>(MassageProcedure.MovementDirections.Forward, 5),
				new ValueTuple<MassageProcedure.MovementDirections, int>(MassageProcedure.MovementDirections.Backward, 5),
				new ValueTuple<MassageProcedure.MovementDirections, int>(MassageProcedure.MovementDirections.Forward, 5),
				new ValueTuple<MassageProcedure.MovementDirections, int>(MassageProcedure.MovementDirections.Up, 1),
				new ValueTuple<MassageProcedure.MovementDirections, int>(MassageProcedure.MovementDirections.Backward, 5),
				new ValueTuple<MassageProcedure.MovementDirections, int>(MassageProcedure.MovementDirections.Forward, 5),
				new ValueTuple<MassageProcedure.MovementDirections, int>(MassageProcedure.MovementDirections.Backward, 5),
				new ValueTuple<MassageProcedure.MovementDirections, int>(MassageProcedure.MovementDirections.Forward, 5),
				new ValueTuple<MassageProcedure.MovementDirections, int>(MassageProcedure.MovementDirections.Backward, 5),
				new ValueTuple<MassageProcedure.MovementDirections, int>(MassageProcedure.MovementDirections.Forward, 5),
				new ValueTuple<MassageProcedure.MovementDirections, int>(MassageProcedure.MovementDirections.Up, 1),
				new ValueTuple<MassageProcedure.MovementDirections, int>(MassageProcedure.MovementDirections.Backward, 5),
				new ValueTuple<MassageProcedure.MovementDirections, int>(MassageProcedure.MovementDirections.Forward, 5),
				new ValueTuple<MassageProcedure.MovementDirections, int>(MassageProcedure.MovementDirections.Backward, 5),
				new ValueTuple<MassageProcedure.MovementDirections, int>(MassageProcedure.MovementDirections.Forward, 5),
				new ValueTuple<MassageProcedure.MovementDirections, int>(MassageProcedure.MovementDirections.Backward, 5),
				new ValueTuple<MassageProcedure.MovementDirections, int>(MassageProcedure.MovementDirections.Forward, 5),
				new ValueTuple<MassageProcedure.MovementDirections, int>(MassageProcedure.MovementDirections.Up, 1),
				new ValueTuple<MassageProcedure.MovementDirections, int>(MassageProcedure.MovementDirections.Backward, 5),
				new ValueTuple<MassageProcedure.MovementDirections, int>(MassageProcedure.MovementDirections.Forward, 5),
				new ValueTuple<MassageProcedure.MovementDirections, int>(MassageProcedure.MovementDirections.Backward, 5),
				new ValueTuple<MassageProcedure.MovementDirections, int>(MassageProcedure.MovementDirections.Forward, 5),
				new ValueTuple<MassageProcedure.MovementDirections, int>(MassageProcedure.MovementDirections.Backward, 5),
				new ValueTuple<MassageProcedure.MovementDirections, int>(MassageProcedure.MovementDirections.Forward, 5),
				new ValueTuple<MassageProcedure.MovementDirections, int>(MassageProcedure.MovementDirections.Backward, 5),
				new ValueTuple<MassageProcedure.MovementDirections, int>(MassageProcedure.MovementDirections.Down, 1),
				new ValueTuple<MassageProcedure.MovementDirections, int>(MassageProcedure.MovementDirections.Forward, 5),
				new ValueTuple<MassageProcedure.MovementDirections, int>(MassageProcedure.MovementDirections.Backward, 5),
				new ValueTuple<MassageProcedure.MovementDirections, int>(MassageProcedure.MovementDirections.Forward, 5),
				new ValueTuple<MassageProcedure.MovementDirections, int>(MassageProcedure.MovementDirections.Backward, 5),
				new ValueTuple<MassageProcedure.MovementDirections, int>(MassageProcedure.MovementDirections.Forward, 5),
				new ValueTuple<MassageProcedure.MovementDirections, int>(MassageProcedure.MovementDirections.Backward, 5),
				new ValueTuple<MassageProcedure.MovementDirections, int>(MassageProcedure.MovementDirections.Down, 1),
				new ValueTuple<MassageProcedure.MovementDirections, int>(MassageProcedure.MovementDirections.Forward, 5),
				new ValueTuple<MassageProcedure.MovementDirections, int>(MassageProcedure.MovementDirections.Backward, 5),
				new ValueTuple<MassageProcedure.MovementDirections, int>(MassageProcedure.MovementDirections.Forward, 5),
				new ValueTuple<MassageProcedure.MovementDirections, int>(MassageProcedure.MovementDirections.Backward, 5),
				new ValueTuple<MassageProcedure.MovementDirections, int>(MassageProcedure.MovementDirections.Forward, 5),
				new ValueTuple<MassageProcedure.MovementDirections, int>(MassageProcedure.MovementDirections.Backward, 5),
				new ValueTuple<MassageProcedure.MovementDirections, int>(MassageProcedure.MovementDirections.Down, 1),
				new ValueTuple<MassageProcedure.MovementDirections, int>(MassageProcedure.MovementDirections.Forward, 5),
				new ValueTuple<MassageProcedure.MovementDirections, int>(MassageProcedure.MovementDirections.Backward, 5),
				new ValueTuple<MassageProcedure.MovementDirections, int>(MassageProcedure.MovementDirections.Forward, 5),
				new ValueTuple<MassageProcedure.MovementDirections, int>(MassageProcedure.MovementDirections.Backward, 5),
				new ValueTuple<MassageProcedure.MovementDirections, int>(MassageProcedure.MovementDirections.Forward, 5),
				new ValueTuple<MassageProcedure.MovementDirections, int>(MassageProcedure.MovementDirections.Backward, 5),
				new ValueTuple<MassageProcedure.MovementDirections, int>(MassageProcedure.MovementDirections.Down, 1),
				new ValueTuple<MassageProcedure.MovementDirections, int>(MassageProcedure.MovementDirections.Forward, 5),
				new ValueTuple<MassageProcedure.MovementDirections, int>(MassageProcedure.MovementDirections.Backward, 5),
				new ValueTuple<MassageProcedure.MovementDirections, int>(MassageProcedure.MovementDirections.Forward, 5),
				new ValueTuple<MassageProcedure.MovementDirections, int>(MassageProcedure.MovementDirections.Backward, 5),
				new ValueTuple<MassageProcedure.MovementDirections, int>(MassageProcedure.MovementDirections.Forward, 5),
				new ValueTuple<MassageProcedure.MovementDirections, int>(MassageProcedure.MovementDirections.Backward, 5),
				new ValueTuple<MassageProcedure.MovementDirections, int>(MassageProcedure.MovementDirections.Down, 1),
				new ValueTuple<MassageProcedure.MovementDirections, int>(MassageProcedure.MovementDirections.Forward, 5),
				new ValueTuple<MassageProcedure.MovementDirections, int>(MassageProcedure.MovementDirections.Backward, 5),
				new ValueTuple<MassageProcedure.MovementDirections, int>(MassageProcedure.MovementDirections.Forward, 5),
				new ValueTuple<MassageProcedure.MovementDirections, int>(MassageProcedure.MovementDirections.Backward, 5),
				new ValueTuple<MassageProcedure.MovementDirections, int>(MassageProcedure.MovementDirections.Forward, 5),
				new ValueTuple<MassageProcedure.MovementDirections, int>(MassageProcedure.MovementDirections.Backward, 5),
				new ValueTuple<MassageProcedure.MovementDirections, int>(MassageProcedure.MovementDirections.Down, 1),
				new ValueTuple<MassageProcedure.MovementDirections, int>(MassageProcedure.MovementDirections.Forward, 5),
				new ValueTuple<MassageProcedure.MovementDirections, int>(MassageProcedure.MovementDirections.Backward, 5),
				new ValueTuple<MassageProcedure.MovementDirections, int>(MassageProcedure.MovementDirections.Forward, 5),
				new ValueTuple<MassageProcedure.MovementDirections, int>(MassageProcedure.MovementDirections.Backward, 5),
				new ValueTuple<MassageProcedure.MovementDirections, int>(MassageProcedure.MovementDirections.Forward, 5),
				new ValueTuple<MassageProcedure.MovementDirections, int>(MassageProcedure.MovementDirections.Backward, 5)
			};
			Queue<ValueTuple<MassageProcedure.MovementDirections, int>> queue = new Queue<ValueTuple<MassageProcedure.MovementDirections, int>>(list);
			this.IsRunning = true;
			MassageProcedure.runningInstance = this;
			this.CurrentState = "Starting";
			while (this.IsRunning)
			{
				ValueTuple<MassageProcedure.MovementDirections, int> valueTuple = queue.Dequeue();
				queue.Enqueue(valueTuple);
				ValueTuple<MassageProcedure.MovementDirections, int> valueTuple2 = valueTuple;
				MassageProcedure.MovementDirections item = valueTuple2.Item1;
				int item2 = valueTuple2.Item2;
				bool flag = await this.MoveToDirection(item, item2, progress);
				bool movementResult = flag;
				await Task.Delay(pauseBetween);
				if (!movementResult)
				{
					this.CurrentState = MQBAdaptationTemplate.UNSUPPORTED_TITLE;
					result = false;
					break;
				}
			}
			MassageProcedure.runningInstance = null;
			return result;
		}

		// Token: 0x04003618 RID: 13848
		[CompilerGenerated]
		private string <Name>k__BackingField;

		// Token: 0x04003619 RID: 13849
		[CompilerGenerated]
		private string <Description>k__BackingField;

		// Token: 0x0400361A RID: 13850
		[CompilerGenerated]
		private string <InnerDescription>k__BackingField;

		// Token: 0x0400361B RID: 13851
		private string _CurrentState = "";

		// Token: 0x0400361C RID: 13852
		[CompilerGenerated]
		private ObservableCollection<MQBAdaptationOption> <Options>k__BackingField;

		// Token: 0x0400361D RID: 13853
		[CompilerGenerated]
		private PropertyChangedEventHandler PropertyChanged;

		// Token: 0x0400361E RID: 13854
		[CompilerGenerated]
		private string <RequestHeader>k__BackingField;

		// Token: 0x0400361F RID: 13855
		[CompilerGenerated]
		private string <ResponseHeader>k__BackingField;

		// Token: 0x04003620 RID: 13856
		private bool IsRunning;

		// Token: 0x04003621 RID: 13857
		private static MassageProcedure runningInstance;

		// Token: 0x02000AEA RID: 2794
		private enum MovementDirections
		{
			// Token: 0x04003623 RID: 13859
			Forward,
			// Token: 0x04003624 RID: 13860
			Backward,
			// Token: 0x04003625 RID: 13861
			Up,
			// Token: 0x04003626 RID: 13862
			Down
		}

		// Token: 0x02000AEB RID: 2795
		[CompilerGenerated]
		private sealed class <>c__DisplayClass45_0
		{
			// Token: 0x060057C3 RID: 22467 RVA: 0x00002050 File Offset: 0x00000250
			public <>c__DisplayClass45_0()
			{
			}

			// Token: 0x060057C4 RID: 22468 RVA: 0x0041CE34 File Offset: 0x0041B034
			internal void <UpdateCurrentState>b__0(OBDRequest request, string data)
			{
				if (string.IsNullOrEmpty(data) || !data.Contains(request.ResponseMarker))
				{
					this.supported = false;
				}
				else
				{
					this.supported = true;
				}
				request.DoNotDecode = true;
				App.OBDReader.ReplaceQueue(new OBDRequest[0]);
				this.semaphore.Release();
			}

			// Token: 0x04003627 RID: 13863
			public bool supported;

			// Token: 0x04003628 RID: 13864
			public SemaphoreSlim semaphore;
		}

		// Token: 0x02000AEC RID: 2796
		[CompilerGenerated]
		private sealed class <>c__DisplayClass54_0
		{
			// Token: 0x060057C5 RID: 22469 RVA: 0x00002050 File Offset: 0x00000250
			public <>c__DisplayClass54_0()
			{
			}

			// Token: 0x060057C6 RID: 22470 RVA: 0x0041CE8C File Offset: 0x0041B08C
			internal void <SendCommandAndWaitForStatus>b__0(OBDRequest request, string data)
			{
				if (string.IsNullOrEmpty(data))
				{
					this.result = false;
					request.DoNotDecode = true;
					App.OBDReader.ReplaceQueue(new OBDRequest[0]);
					this.semaphore.Release();
				}
				if (!data.Contains(request.ResponseMarker))
				{
					this.result = false;
					App.OBDReader.ReplaceQueue(new OBDRequest[0]);
					this.semaphore.Release();
				}
			}

			// Token: 0x060057C7 RID: 22471 RVA: 0x0041CEFC File Offset: 0x0041B0FC
			internal void <SendCommandAndWaitForStatus>b__1(OBDRequest request, byte[] data, bool decodeResult, string responseHeader)
			{
				if (data != null && data.Length != 0)
				{
					if (this.waitForNextStatusUpdateDelegate(data))
					{
						this.result = true;
						this.semaphore.Release();
						return;
					}
					App.OBDReader.AddRequestToQueue(this.checkStatusRequest);
				}
			}

			// Token: 0x04003629 RID: 13865
			public bool result;

			// Token: 0x0400362A RID: 13866
			public SemaphoreSlim semaphore;

			// Token: 0x0400362B RID: 13867
			public Func<byte[], bool> waitForNextStatusUpdateDelegate;

			// Token: 0x0400362C RID: 13868
			public OBDRequest checkStatusRequest;
		}

		// Token: 0x02000AED RID: 2797
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <Execute>d__44 : IAsyncStateMachine
		{
			// Token: 0x060057C8 RID: 22472 RVA: 0x0041CF38 File Offset: 0x0041B138
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				MassageProcedure massageProcedure = this;
				CodingRequestResult codingRequestResult;
				try
				{
					TaskAwaiter<bool> taskAwaiter3;
					if (num != 0)
					{
						if (!(value == "03"))
						{
							massageProcedure.IsRunning = false;
							codingRequestResult = CodingRequestResult.Success;
							goto IL_00B0;
						}
						taskAwaiter3 = massageProcedure.StartLoop(progress).GetAwaiter();
						if (!taskAwaiter3.IsCompleted)
						{
							num2 = 0;
							taskAwaiter2 = taskAwaiter3;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<bool>, MassageProcedure.<Execute>d__44>(ref taskAwaiter3, ref this);
							return;
						}
					}
					else
					{
						taskAwaiter3 = taskAwaiter2;
						taskAwaiter2 = default(TaskAwaiter<bool>);
						num2 = -1;
					}
					if (taskAwaiter3.GetResult())
					{
						codingRequestResult = CodingRequestResult.Success;
					}
					else
					{
						codingRequestResult = CodingRequestResult.NotSupported;
					}
				}
				catch (Exception ex)
				{
					num2 = -2;
					this.<>t__builder.SetException(ex);
					return;
				}
				IL_00B0:
				num2 = -2;
				this.<>t__builder.SetResult(codingRequestResult);
			}

			// Token: 0x060057C9 RID: 22473 RVA: 0x0041D01C File Offset: 0x0041B21C
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x0400362D RID: 13869
			public int <>1__state;

			// Token: 0x0400362E RID: 13870
			public AsyncTaskMethodBuilder<CodingRequestResult> <>t__builder;

			// Token: 0x0400362F RID: 13871
			public string value;

			// Token: 0x04003630 RID: 13872
			public MassageProcedure <>4__this;

			// Token: 0x04003631 RID: 13873
			public IProgress<string> progress;

			// Token: 0x04003632 RID: 13874
			private TaskAwaiter<bool> <>u__1;
		}

		// Token: 0x02000AEE RID: 2798
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <MoveToDirection>d__60 : IAsyncStateMachine
		{
			// Token: 0x060057CA RID: 22474 RVA: 0x0041D02C File Offset: 0x0041B22C
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				MassageProcedure massageProcedure = this;
				bool result;
				try
				{
					TaskAwaiter<bool> taskAwaiter;
					if (num != 0)
					{
						OBDRequest obdrequest = massageProcedure.BuildRequestForMovement(direction, durationSeconds);
						OBDRequest obdrequest2 = massageProcedure.BuildCheckStatusRequest();
						IProgress<string> progress = progress;
						if (progress != null)
						{
							progress.Report(string.Format("Moving to: {0} ({1})", direction, durationSeconds));
						}
						taskAwaiter = massageProcedure.SendCommandAndWaitForStatus(obdrequest, obdrequest2, new Func<byte[], bool>(massageProcedure.checkForStatusCompleted)).GetAwaiter();
						if (!taskAwaiter.IsCompleted)
						{
							num2 = 0;
							TaskAwaiter<bool> taskAwaiter2 = taskAwaiter;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<bool>, MassageProcedure.<MoveToDirection>d__60>(ref taskAwaiter, ref this);
							return;
						}
					}
					else
					{
						TaskAwaiter<bool> taskAwaiter2;
						taskAwaiter = taskAwaiter2;
						taskAwaiter2 = default(TaskAwaiter<bool>);
						num2 = -1;
					}
					result = taskAwaiter.GetResult();
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

			// Token: 0x060057CB RID: 22475 RVA: 0x0041D144 File Offset: 0x0041B344
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x04003633 RID: 13875
			public int <>1__state;

			// Token: 0x04003634 RID: 13876
			public AsyncTaskMethodBuilder<bool> <>t__builder;

			// Token: 0x04003635 RID: 13877
			public MassageProcedure <>4__this;

			// Token: 0x04003636 RID: 13878
			public MassageProcedure.MovementDirections direction;

			// Token: 0x04003637 RID: 13879
			public int durationSeconds;

			// Token: 0x04003638 RID: 13880
			public IProgress<string> progress;

			// Token: 0x04003639 RID: 13881
			private TaskAwaiter<bool> <>u__1;
		}

		// Token: 0x02000AEF RID: 2799
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <SendCommandAndWaitForStatus>d__54 : IAsyncStateMachine
		{
			// Token: 0x060057CC RID: 22476 RVA: 0x0041D154 File Offset: 0x0041B354
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				bool result;
				try
				{
					TaskAwaiter taskAwaiter;
					if (num != 0)
					{
						CS$<>8__locals1 = new MassageProcedure.<>c__DisplayClass54_0();
						CS$<>8__locals1.waitForNextStatusUpdateDelegate = waitForNextStatusUpdateDelegate;
						CS$<>8__locals1.checkStatusRequest = checkStatusRequest;
						CS$<>8__locals1.semaphore = new SemaphoreSlim(0, 1);
						commandRequest.Repeat = false;
						CS$<>8__locals1.checkStatusRequest.Repeat = false;
						CS$<>8__locals1.result = false;
						ResponseReceivedDelegate responseReceivedDelegate = delegate(OBDRequest request, string data)
						{
							if (string.IsNullOrEmpty(data))
							{
								CS$<>8__locals1.result = false;
								request.DoNotDecode = true;
								App.OBDReader.ReplaceQueue(new OBDRequest[0]);
								CS$<>8__locals1.semaphore.Release();
							}
							if (!data.Contains(request.ResponseMarker))
							{
								CS$<>8__locals1.result = false;
								App.OBDReader.ReplaceQueue(new OBDRequest[0]);
								CS$<>8__locals1.semaphore.Release();
							}
						};
						CS$<>8__locals1.checkStatusRequest.ResponseReceived -= responseReceivedDelegate;
						CS$<>8__locals1.checkStatusRequest.ResponseReceived += responseReceivedDelegate;
						CS$<>8__locals1.checkStatusRequest.ResponseDecoded += delegate(OBDRequest request, byte[] data, bool decodeResult, string responseHeader)
						{
							if (data != null && data.Length != 0)
							{
								if (CS$<>8__locals1.waitForNextStatusUpdateDelegate(data))
								{
									CS$<>8__locals1.result = true;
									CS$<>8__locals1.semaphore.Release();
									return;
								}
								App.OBDReader.AddRequestToQueue(CS$<>8__locals1.checkStatusRequest);
							}
						};
						commandRequest.ResponseReceived -= responseReceivedDelegate;
						commandRequest.ResponseReceived += responseReceivedDelegate;
						App.OBDReader.ReplaceQueue(new OBDRequest[] { commandRequest, CS$<>8__locals1.checkStatusRequest });
						taskAwaiter = CS$<>8__locals1.semaphore.WaitAsync().GetAwaiter();
						if (!taskAwaiter.IsCompleted)
						{
							num2 = 0;
							TaskAwaiter taskAwaiter2 = taskAwaiter;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, MassageProcedure.<SendCommandAndWaitForStatus>d__54>(ref taskAwaiter, ref this);
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
					result = CS$<>8__locals1.result;
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
				this.<>t__builder.SetResult(result);
			}

			// Token: 0x060057CD RID: 22477 RVA: 0x0041D334 File Offset: 0x0041B534
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x0400363A RID: 13882
			public int <>1__state;

			// Token: 0x0400363B RID: 13883
			public AsyncTaskMethodBuilder<bool> <>t__builder;

			// Token: 0x0400363C RID: 13884
			public Func<byte[], bool> waitForNextStatusUpdateDelegate;

			// Token: 0x0400363D RID: 13885
			public OBDRequest checkStatusRequest;

			// Token: 0x0400363E RID: 13886
			public OBDRequest commandRequest;

			// Token: 0x0400363F RID: 13887
			private MassageProcedure.<>c__DisplayClass54_0 <>8__1;

			// Token: 0x04003640 RID: 13888
			private TaskAwaiter <>u__1;
		}

		// Token: 0x02000AF0 RID: 2800
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <StartLoop>d__63 : IAsyncStateMachine
		{
			// Token: 0x060057CE RID: 22478 RVA: 0x0041D344 File Offset: 0x0041B544
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				MassageProcedure massageProcedure = this;
				bool flag;
				try
				{
					TaskAwaiter taskAwaiter;
					TaskAwaiter<bool> taskAwaiter3;
					if (num != 0)
					{
						if (num != 1)
						{
							result = true;
							pauseBetween = TimeSpan.FromSeconds(0.7);
							List<ValueTuple<MassageProcedure.MovementDirections, int>> list = new List<ValueTuple<MassageProcedure.MovementDirections, int>>
							{
								new ValueTuple<MassageProcedure.MovementDirections, int>(MassageProcedure.MovementDirections.Up, 5),
								new ValueTuple<MassageProcedure.MovementDirections, int>(MassageProcedure.MovementDirections.Forward, 5),
								new ValueTuple<MassageProcedure.MovementDirections, int>(MassageProcedure.MovementDirections.Backward, 5),
								new ValueTuple<MassageProcedure.MovementDirections, int>(MassageProcedure.MovementDirections.Forward, 5),
								new ValueTuple<MassageProcedure.MovementDirections, int>(MassageProcedure.MovementDirections.Down, 5),
								new ValueTuple<MassageProcedure.MovementDirections, int>(MassageProcedure.MovementDirections.Backward, 5),
								new ValueTuple<MassageProcedure.MovementDirections, int>(MassageProcedure.MovementDirections.Forward, 5),
								new ValueTuple<MassageProcedure.MovementDirections, int>(MassageProcedure.MovementDirections.Up, 5),
								new ValueTuple<MassageProcedure.MovementDirections, int>(MassageProcedure.MovementDirections.Backward, 5),
								new ValueTuple<MassageProcedure.MovementDirections, int>(MassageProcedure.MovementDirections.Down, 5),
								new ValueTuple<MassageProcedure.MovementDirections, int>(MassageProcedure.MovementDirections.Forward, 5),
								new ValueTuple<MassageProcedure.MovementDirections, int>(MassageProcedure.MovementDirections.Up, 5),
								new ValueTuple<MassageProcedure.MovementDirections, int>(MassageProcedure.MovementDirections.Down, 5),
								new ValueTuple<MassageProcedure.MovementDirections, int>(MassageProcedure.MovementDirections.Up, 1),
								new ValueTuple<MassageProcedure.MovementDirections, int>(MassageProcedure.MovementDirections.Backward, 5),
								new ValueTuple<MassageProcedure.MovementDirections, int>(MassageProcedure.MovementDirections.Forward, 5),
								new ValueTuple<MassageProcedure.MovementDirections, int>(MassageProcedure.MovementDirections.Backward, 5),
								new ValueTuple<MassageProcedure.MovementDirections, int>(MassageProcedure.MovementDirections.Forward, 5),
								new ValueTuple<MassageProcedure.MovementDirections, int>(MassageProcedure.MovementDirections.Backward, 5),
								new ValueTuple<MassageProcedure.MovementDirections, int>(MassageProcedure.MovementDirections.Forward, 5),
								new ValueTuple<MassageProcedure.MovementDirections, int>(MassageProcedure.MovementDirections.Up, 1),
								new ValueTuple<MassageProcedure.MovementDirections, int>(MassageProcedure.MovementDirections.Backward, 5),
								new ValueTuple<MassageProcedure.MovementDirections, int>(MassageProcedure.MovementDirections.Forward, 5),
								new ValueTuple<MassageProcedure.MovementDirections, int>(MassageProcedure.MovementDirections.Backward, 5),
								new ValueTuple<MassageProcedure.MovementDirections, int>(MassageProcedure.MovementDirections.Forward, 5),
								new ValueTuple<MassageProcedure.MovementDirections, int>(MassageProcedure.MovementDirections.Backward, 5),
								new ValueTuple<MassageProcedure.MovementDirections, int>(MassageProcedure.MovementDirections.Forward, 5),
								new ValueTuple<MassageProcedure.MovementDirections, int>(MassageProcedure.MovementDirections.Up, 1),
								new ValueTuple<MassageProcedure.MovementDirections, int>(MassageProcedure.MovementDirections.Backward, 5),
								new ValueTuple<MassageProcedure.MovementDirections, int>(MassageProcedure.MovementDirections.Forward, 5),
								new ValueTuple<MassageProcedure.MovementDirections, int>(MassageProcedure.MovementDirections.Backward, 5),
								new ValueTuple<MassageProcedure.MovementDirections, int>(MassageProcedure.MovementDirections.Forward, 5),
								new ValueTuple<MassageProcedure.MovementDirections, int>(MassageProcedure.MovementDirections.Backward, 5),
								new ValueTuple<MassageProcedure.MovementDirections, int>(MassageProcedure.MovementDirections.Forward, 5),
								new ValueTuple<MassageProcedure.MovementDirections, int>(MassageProcedure.MovementDirections.Up, 1),
								new ValueTuple<MassageProcedure.MovementDirections, int>(MassageProcedure.MovementDirections.Backward, 5),
								new ValueTuple<MassageProcedure.MovementDirections, int>(MassageProcedure.MovementDirections.Forward, 5),
								new ValueTuple<MassageProcedure.MovementDirections, int>(MassageProcedure.MovementDirections.Backward, 5),
								new ValueTuple<MassageProcedure.MovementDirections, int>(MassageProcedure.MovementDirections.Forward, 5),
								new ValueTuple<MassageProcedure.MovementDirections, int>(MassageProcedure.MovementDirections.Backward, 5),
								new ValueTuple<MassageProcedure.MovementDirections, int>(MassageProcedure.MovementDirections.Forward, 5),
								new ValueTuple<MassageProcedure.MovementDirections, int>(MassageProcedure.MovementDirections.Up, 1),
								new ValueTuple<MassageProcedure.MovementDirections, int>(MassageProcedure.MovementDirections.Backward, 5),
								new ValueTuple<MassageProcedure.MovementDirections, int>(MassageProcedure.MovementDirections.Forward, 5),
								new ValueTuple<MassageProcedure.MovementDirections, int>(MassageProcedure.MovementDirections.Backward, 5),
								new ValueTuple<MassageProcedure.MovementDirections, int>(MassageProcedure.MovementDirections.Forward, 5),
								new ValueTuple<MassageProcedure.MovementDirections, int>(MassageProcedure.MovementDirections.Backward, 5),
								new ValueTuple<MassageProcedure.MovementDirections, int>(MassageProcedure.MovementDirections.Forward, 5),
								new ValueTuple<MassageProcedure.MovementDirections, int>(MassageProcedure.MovementDirections.Up, 1),
								new ValueTuple<MassageProcedure.MovementDirections, int>(MassageProcedure.MovementDirections.Backward, 5),
								new ValueTuple<MassageProcedure.MovementDirections, int>(MassageProcedure.MovementDirections.Forward, 5),
								new ValueTuple<MassageProcedure.MovementDirections, int>(MassageProcedure.MovementDirections.Backward, 5),
								new ValueTuple<MassageProcedure.MovementDirections, int>(MassageProcedure.MovementDirections.Forward, 5),
								new ValueTuple<MassageProcedure.MovementDirections, int>(MassageProcedure.MovementDirections.Backward, 5),
								new ValueTuple<MassageProcedure.MovementDirections, int>(MassageProcedure.MovementDirections.Forward, 5),
								new ValueTuple<MassageProcedure.MovementDirections, int>(MassageProcedure.MovementDirections.Backward, 5),
								new ValueTuple<MassageProcedure.MovementDirections, int>(MassageProcedure.MovementDirections.Down, 1),
								new ValueTuple<MassageProcedure.MovementDirections, int>(MassageProcedure.MovementDirections.Forward, 5),
								new ValueTuple<MassageProcedure.MovementDirections, int>(MassageProcedure.MovementDirections.Backward, 5),
								new ValueTuple<MassageProcedure.MovementDirections, int>(MassageProcedure.MovementDirections.Forward, 5),
								new ValueTuple<MassageProcedure.MovementDirections, int>(MassageProcedure.MovementDirections.Backward, 5),
								new ValueTuple<MassageProcedure.MovementDirections, int>(MassageProcedure.MovementDirections.Forward, 5),
								new ValueTuple<MassageProcedure.MovementDirections, int>(MassageProcedure.MovementDirections.Backward, 5),
								new ValueTuple<MassageProcedure.MovementDirections, int>(MassageProcedure.MovementDirections.Down, 1),
								new ValueTuple<MassageProcedure.MovementDirections, int>(MassageProcedure.MovementDirections.Forward, 5),
								new ValueTuple<MassageProcedure.MovementDirections, int>(MassageProcedure.MovementDirections.Backward, 5),
								new ValueTuple<MassageProcedure.MovementDirections, int>(MassageProcedure.MovementDirections.Forward, 5),
								new ValueTuple<MassageProcedure.MovementDirections, int>(MassageProcedure.MovementDirections.Backward, 5),
								new ValueTuple<MassageProcedure.MovementDirections, int>(MassageProcedure.MovementDirections.Forward, 5),
								new ValueTuple<MassageProcedure.MovementDirections, int>(MassageProcedure.MovementDirections.Backward, 5),
								new ValueTuple<MassageProcedure.MovementDirections, int>(MassageProcedure.MovementDirections.Down, 1),
								new ValueTuple<MassageProcedure.MovementDirections, int>(MassageProcedure.MovementDirections.Forward, 5),
								new ValueTuple<MassageProcedure.MovementDirections, int>(MassageProcedure.MovementDirections.Backward, 5),
								new ValueTuple<MassageProcedure.MovementDirections, int>(MassageProcedure.MovementDirections.Forward, 5),
								new ValueTuple<MassageProcedure.MovementDirections, int>(MassageProcedure.MovementDirections.Backward, 5),
								new ValueTuple<MassageProcedure.MovementDirections, int>(MassageProcedure.MovementDirections.Forward, 5),
								new ValueTuple<MassageProcedure.MovementDirections, int>(MassageProcedure.MovementDirections.Backward, 5),
								new ValueTuple<MassageProcedure.MovementDirections, int>(MassageProcedure.MovementDirections.Down, 1),
								new ValueTuple<MassageProcedure.MovementDirections, int>(MassageProcedure.MovementDirections.Forward, 5),
								new ValueTuple<MassageProcedure.MovementDirections, int>(MassageProcedure.MovementDirections.Backward, 5),
								new ValueTuple<MassageProcedure.MovementDirections, int>(MassageProcedure.MovementDirections.Forward, 5),
								new ValueTuple<MassageProcedure.MovementDirections, int>(MassageProcedure.MovementDirections.Backward, 5),
								new ValueTuple<MassageProcedure.MovementDirections, int>(MassageProcedure.MovementDirections.Forward, 5),
								new ValueTuple<MassageProcedure.MovementDirections, int>(MassageProcedure.MovementDirections.Backward, 5),
								new ValueTuple<MassageProcedure.MovementDirections, int>(MassageProcedure.MovementDirections.Down, 1),
								new ValueTuple<MassageProcedure.MovementDirections, int>(MassageProcedure.MovementDirections.Forward, 5),
								new ValueTuple<MassageProcedure.MovementDirections, int>(MassageProcedure.MovementDirections.Backward, 5),
								new ValueTuple<MassageProcedure.MovementDirections, int>(MassageProcedure.MovementDirections.Forward, 5),
								new ValueTuple<MassageProcedure.MovementDirections, int>(MassageProcedure.MovementDirections.Backward, 5),
								new ValueTuple<MassageProcedure.MovementDirections, int>(MassageProcedure.MovementDirections.Forward, 5),
								new ValueTuple<MassageProcedure.MovementDirections, int>(MassageProcedure.MovementDirections.Backward, 5),
								new ValueTuple<MassageProcedure.MovementDirections, int>(MassageProcedure.MovementDirections.Down, 1),
								new ValueTuple<MassageProcedure.MovementDirections, int>(MassageProcedure.MovementDirections.Forward, 5),
								new ValueTuple<MassageProcedure.MovementDirections, int>(MassageProcedure.MovementDirections.Backward, 5),
								new ValueTuple<MassageProcedure.MovementDirections, int>(MassageProcedure.MovementDirections.Forward, 5),
								new ValueTuple<MassageProcedure.MovementDirections, int>(MassageProcedure.MovementDirections.Backward, 5),
								new ValueTuple<MassageProcedure.MovementDirections, int>(MassageProcedure.MovementDirections.Forward, 5),
								new ValueTuple<MassageProcedure.MovementDirections, int>(MassageProcedure.MovementDirections.Backward, 5)
							};
							queue = new Queue<ValueTuple<MassageProcedure.MovementDirections, int>>(list);
							massageProcedure.IsRunning = true;
							MassageProcedure.runningInstance = massageProcedure;
							massageProcedure.CurrentState = "Starting";
							goto IL_067B;
						}
						TaskAwaiter taskAwaiter2;
						taskAwaiter = taskAwaiter2;
						taskAwaiter2 = default(TaskAwaiter);
						num2 = -1;
						goto IL_0658;
					}
					else
					{
						TaskAwaiter<bool> taskAwaiter4;
						taskAwaiter3 = taskAwaiter4;
						taskAwaiter4 = default(TaskAwaiter<bool>);
						num2 = -1;
					}
					IL_05EB:
					bool result2 = taskAwaiter3.GetResult();
					movementResult = result2;
					taskAwaiter = Task.Delay(pauseBetween).GetAwaiter();
					if (!taskAwaiter.IsCompleted)
					{
						num2 = 1;
						TaskAwaiter taskAwaiter2 = taskAwaiter;
						this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, MassageProcedure.<StartLoop>d__63>(ref taskAwaiter, ref this);
						return;
					}
					IL_0658:
					taskAwaiter.GetResult();
					if (!movementResult)
					{
						massageProcedure.CurrentState = MQBAdaptationTemplate.UNSUPPORTED_TITLE;
						result = false;
						goto IL_0686;
					}
					IL_067B:
					if (massageProcedure.IsRunning)
					{
						ValueTuple<MassageProcedure.MovementDirections, int> valueTuple = queue.Dequeue();
						queue.Enqueue(valueTuple);
						ValueTuple<MassageProcedure.MovementDirections, int> valueTuple2 = valueTuple;
						MassageProcedure.MovementDirections item = valueTuple2.Item1;
						int item2 = valueTuple2.Item2;
						taskAwaiter3 = massageProcedure.MoveToDirection(item, item2, progress).GetAwaiter();
						if (!taskAwaiter3.IsCompleted)
						{
							num2 = 0;
							TaskAwaiter<bool> taskAwaiter4 = taskAwaiter3;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<bool>, MassageProcedure.<StartLoop>d__63>(ref taskAwaiter3, ref this);
							return;
						}
						goto IL_05EB;
					}
					IL_0686:
					MassageProcedure.runningInstance = null;
					flag = result;
				}
				catch (Exception ex)
				{
					num2 = -2;
					queue = null;
					this.<>t__builder.SetException(ex);
					return;
				}
				num2 = -2;
				queue = null;
				this.<>t__builder.SetResult(flag);
			}

			// Token: 0x060057CF RID: 22479 RVA: 0x0041DA40 File Offset: 0x0041BC40
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x04003641 RID: 13889
			public int <>1__state;

			// Token: 0x04003642 RID: 13890
			public AsyncTaskMethodBuilder<bool> <>t__builder;

			// Token: 0x04003643 RID: 13891
			public MassageProcedure <>4__this;

			// Token: 0x04003644 RID: 13892
			public IProgress<string> progress;

			// Token: 0x04003645 RID: 13893
			private bool <result>5__2;

			// Token: 0x04003646 RID: 13894
			private TimeSpan <pauseBetween>5__3;

			// Token: 0x04003647 RID: 13895
			private Queue<ValueTuple<MassageProcedure.MovementDirections, int>> <queue>5__4;

			// Token: 0x04003648 RID: 13896
			private bool <movementResult>5__5;

			// Token: 0x04003649 RID: 13897
			private TaskAwaiter<bool> <>u__1;

			// Token: 0x0400364A RID: 13898
			private TaskAwaiter <>u__2;
		}

		// Token: 0x02000AF1 RID: 2801
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <UpdateCurrentState>d__45 : IAsyncStateMachine
		{
			// Token: 0x060057D0 RID: 22480 RVA: 0x0041DA50 File Offset: 0x0041BC50
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				MassageProcedure massageProcedure = this;
				CodingRequestResult codingRequestResult;
				try
				{
					TaskAwaiter taskAwaiter;
					if (num != 0)
					{
						CS$<>8__locals1 = new MassageProcedure.<>c__DisplayClass45_0();
						if (MassageProcedure.runningInstance != null)
						{
							MassageProcedure.runningInstance.IsRunning = false;
							MassageProcedure.runningInstance = null;
						}
						CS$<>8__locals1.semaphore = new SemaphoreSlim(0, 1);
						OBDRequest obdrequest = massageProcedure.BuildCheckStatusRequest();
						CS$<>8__locals1.supported = false;
						obdrequest.ResponseReceived += delegate(OBDRequest request, string data)
						{
							if (string.IsNullOrEmpty(data) || !data.Contains(request.ResponseMarker))
							{
								CS$<>8__locals1.supported = false;
							}
							else
							{
								CS$<>8__locals1.supported = true;
							}
							request.DoNotDecode = true;
							App.OBDReader.ReplaceQueue(new OBDRequest[0]);
							CS$<>8__locals1.semaphore.Release();
						};
						App.OBDReader.ReplaceQueue(new OBDRequest[] { obdrequest });
						taskAwaiter = CS$<>8__locals1.semaphore.WaitAsync().GetAwaiter();
						if (!taskAwaiter.IsCompleted)
						{
							num2 = 0;
							TaskAwaiter taskAwaiter2 = taskAwaiter;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, MassageProcedure.<UpdateCurrentState>d__45>(ref taskAwaiter, ref this);
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
					if (!CS$<>8__locals1.supported)
					{
						massageProcedure.CurrentState = MQBAdaptationTemplate.UNSUPPORTED_TITLE;
						codingRequestResult = CodingRequestResult.NotSupported;
					}
					else
					{
						massageProcedure.CurrentState = "";
						codingRequestResult = CodingRequestResult.Success;
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
				this.<>t__builder.SetResult(codingRequestResult);
			}

			// Token: 0x060057D1 RID: 22481 RVA: 0x0041DBD0 File Offset: 0x0041BDD0
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x0400364B RID: 13899
			public int <>1__state;

			// Token: 0x0400364C RID: 13900
			public AsyncTaskMethodBuilder<CodingRequestResult> <>t__builder;

			// Token: 0x0400364D RID: 13901
			public MassageProcedure <>4__this;

			// Token: 0x0400364E RID: 13902
			private MassageProcedure.<>c__DisplayClass45_0 <>8__1;

			// Token: 0x0400364F RID: 13903
			private TaskAwaiter <>u__1;
		}
	}
}
