using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using CarScannerXamarinForms.OBD2;
using CarScannerXamarinForms.OBD2.PIDS;

namespace CarScannerXamarinForms.Coding.DB.Renault
{
	// Token: 0x02000A1A RID: 2586
	internal class VestaRenaultDriveWheelPositionSensor : ICodingContainer, INotifyPropertyChanged, IServiceProcedure
	{
		// Token: 0x06005264 RID: 21092 RVA: 0x003F8B4C File Offset: 0x003F6D4C
		public VestaRenaultDriveWheelPositionSensor(string openSessionCmd)
		{
			this.Group = CodingGroup.ServiceProcedures;
			this.Name = Translate.GetString("codingDB_VestaSteeringWheelPosition_Name");
			this.Description = Translate.GetString("coding_Renault_Warning");
			this.InnerDescription = Translate.GetString("codingDB_VestaSteeringWheelPosition_InnerDescription");
			this.removeShift = new MQBAdaptationOption(Translate.GetString("codingDB_VestaSteeringWheelPosition_ResetCalibration"), "31010000");
			this.setShift = new MQBAdaptationOption(Translate.GetString("codingDB_VestaSteeringWheelPosition_SetCalibration"), "31020080");
			this.Options.Add(this.removeShift);
			this.Options.Add(this.setShift);
			this.OpenSessionCommand = openSessionCmd;
		}

		// Token: 0x170017CB RID: 6091
		// (get) Token: 0x06005265 RID: 21093 RVA: 0x003F8C5A File Offset: 0x003F6E5A
		// (set) Token: 0x06005266 RID: 21094 RVA: 0x003F8C62 File Offset: 0x003F6E62
		public CodingGroup Group
		{
			[CompilerGenerated]
			get
			{
				return this.<Group>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<Group>k__BackingField = value;
			}
		}

		// Token: 0x170017CC RID: 6092
		// (get) Token: 0x06005267 RID: 21095 RVA: 0x003F8C6B File Offset: 0x003F6E6B
		// (set) Token: 0x06005268 RID: 21096 RVA: 0x003F8C73 File Offset: 0x003F6E73
		public string Name
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

		// Token: 0x170017CD RID: 6093
		// (get) Token: 0x06005269 RID: 21097 RVA: 0x003F8C7C File Offset: 0x003F6E7C
		// (set) Token: 0x0600526A RID: 21098 RVA: 0x003F8C84 File Offset: 0x003F6E84
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

		// Token: 0x170017CE RID: 6094
		// (get) Token: 0x0600526B RID: 21099 RVA: 0x003F8C8D File Offset: 0x003F6E8D
		// (set) Token: 0x0600526C RID: 21100 RVA: 0x003F8C95 File Offset: 0x003F6E95
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

		// Token: 0x170017CF RID: 6095
		// (get) Token: 0x0600526D RID: 21101 RVA: 0x003F8C9E File Offset: 0x003F6E9E
		// (set) Token: 0x0600526E RID: 21102 RVA: 0x003F8CA6 File Offset: 0x003F6EA6
		public string OpenSessionCommand
		{
			[CompilerGenerated]
			get
			{
				return this.<OpenSessionCommand>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<OpenSessionCommand>k__BackingField = value;
			}
		} = "10C0";

		// Token: 0x170017D0 RID: 6096
		// (get) Token: 0x0600526F RID: 21103 RVA: 0x003F8CAF File Offset: 0x003F6EAF
		// (set) Token: 0x06005270 RID: 21104 RVA: 0x003F8CB7 File Offset: 0x003F6EB7
		public string CurrentState
		{
			get
			{
				return this._CurrenState;
			}
			set
			{
				this._CurrenState = value;
				PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
				if (propertyChanged == null)
				{
					return;
				}
				propertyChanged(this, new PropertyChangedEventArgs("CurrentState"));
			}
		}

		// Token: 0x170017D1 RID: 6097
		// (get) Token: 0x06005271 RID: 21105 RVA: 0x00002076 File Offset: 0x00000276
		public bool PasswordVisible
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170017D2 RID: 6098
		// (get) Token: 0x06005272 RID: 21106 RVA: 0x003F8CDB File Offset: 0x003F6EDB
		// (set) Token: 0x06005273 RID: 21107 RVA: 0x003F8CE3 File Offset: 0x003F6EE3
		public bool HasCurrentState
		{
			[CompilerGenerated]
			get
			{
				return this.<HasCurrentState>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<HasCurrentState>k__BackingField = value;
			}
		} = true;

		// Token: 0x170017D3 RID: 6099
		// (get) Token: 0x06005274 RID: 21108 RVA: 0x001ECB01 File Offset: 0x001EAD01
		public string PasswordHint
		{
			get
			{
				return "";
			}
		}

		// Token: 0x170017D4 RID: 6100
		// (get) Token: 0x06005275 RID: 21109 RVA: 0x003F8CEC File Offset: 0x003F6EEC
		// (set) Token: 0x06005276 RID: 21110 RVA: 0x003F8CF4 File Offset: 0x003F6EF4
		public ObservableCollection<MQBAdaptationOption> Options
		{
			[CompilerGenerated]
			get
			{
				return this.<Options>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<Options>k__BackingField = value;
			}
		} = new ObservableCollection<MQBAdaptationOption>();

		// Token: 0x170017D5 RID: 6101
		// (get) Token: 0x06005277 RID: 21111 RVA: 0x003F8CFD File Offset: 0x003F6EFD
		// (set) Token: 0x06005278 RID: 21112 RVA: 0x003F8D05 File Offset: 0x003F6F05
		public bool RequiresPro
		{
			[CompilerGenerated]
			get
			{
				return this.<RequiresPro>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<RequiresPro>k__BackingField = value;
			}
		} = true;

		// Token: 0x170017D6 RID: 6102
		// (get) Token: 0x06005279 RID: 21113 RVA: 0x00002076 File Offset: 0x00000276
		public AdaptationValueTypes ValueType
		{
			get
			{
				return AdaptationValueTypes.OptionType;
			}
		}

		// Token: 0x170017D7 RID: 6103
		// (get) Token: 0x0600527A RID: 21114 RVA: 0x003F8D0E File Offset: 0x003F6F0E
		// (set) Token: 0x0600527B RID: 21115 RVA: 0x003F8D16 File Offset: 0x003F6F16
		public string Password
		{
			[CompilerGenerated]
			get
			{
				return this.<Password>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<Password>k__BackingField = value;
			}
		} = "";

		// Token: 0x14000062 RID: 98
		// (add) Token: 0x0600527C RID: 21116 RVA: 0x003F8D20 File Offset: 0x003F6F20
		// (remove) Token: 0x0600527D RID: 21117 RVA: 0x003F8D58 File Offset: 0x003F6F58
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

		// Token: 0x0600527E RID: 21118 RVA: 0x003F8D90 File Offset: 0x003F6F90
		public async Task<CodingRequestResult> Execute(string password, string value, string UserFriendlyValue, IProgress<string> progress, byte[] originalData = null, bool skipIfTheSameData = false)
		{
			CodingRequestResult result = CodingRequestResult.UnknownError;
			string text = string.Concat(new string[] { "ATSH", this.RequestHeader, ";ATFCSH", this.RequestHeader, ";ATFCSD300000;ATFCSM1;ATST", this.ATST, ";ATCRA", this.ResponseHeader });
			string text2 = "ATAR;ATFCSM0";
			OBDRequest obdrequest = new OBDRequest(this.OpenSessionCommand, this.RequestHeader, text, text2, false)
			{
				ELMFormat = ELMFormat.CAN11bit
			};
			OBDRequest resetReq = new OBDRequest("1102", this.RequestHeader, text, text2, false)
			{
				ELMFormat = ELMFormat.CAN11bit
			};
			if (value == this.removeShift.Value)
			{
				"037F31" + "78";
				string finishedSuccessPattern2 = "7101";
				OBDRequest obdrequest2 = new OBDRequest("31010000", this.RequestHeader, text, text2, false);
				Func<string, bool> <>9__1;
				obdrequest2.ResponseReceived += delegate(OBDRequest senderRequest, string data)
				{
					if (data == null)
					{
						result = CodingRequestResult.NoData;
					}
					if (data.Contains("NO DATA") || data.Contains("ERROR"))
					{
						result = CodingRequestResult.NoData;
					}
					IEnumerable<string> enumerable = OBDDataReader.FilterHexAndNewLineOnly(data).Split(new char[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
					Func<string, bool> func;
					if ((func = <>9__1) == null)
					{
						func = (<>9__1 = (string line) => line.Contains(finishedSuccessPattern2));
					}
					if (enumerable.Any(func))
					{
						IProgress<string> progress2 = progress;
						if (progress2 != null)
						{
							progress2.Report("Операция завершена");
						}
						result = CodingRequestResult.Success;
						App.OBDReader.ReplaceQueue(new OBDRequest[] { resetReq });
					}
				};
				App.OBDReader.ReplaceQueue(new OBDRequest[] { obdrequest, obdrequest2 });
				await App.OBDReader.WaitForCommandQueue();
			}
			else if (value == this.setShift.Value)
			{
				string negativePattern = "037F31";
				negativePattern + "78";
				string continueWithRepeatPattern = "710201";
				string finishedSuccessPattern = "710202";
				string negativeCode03Pattern = "710204";
				string finishedWithCode02Pattern = "7102";
				OBDRequest obdrequest3 = new OBDRequest(value, this.RequestHeader, text, text2, false);
				obdrequest3.ELMFormat = ELMFormat.CAN11bit;
				OBDRequest continueReq = new OBDRequest("310201", this.RequestHeader, text, text2, false)
				{
					ELMFormat = ELMFormat.CAN11bit
				};
				Func<string, bool> <>9__3;
				Func<string, bool> <>9__4;
				Func<string, bool> <>9__5;
				Func<string, bool> <>9__6;
				Func<string, bool> <>9__7;
				ResponseReceivedDelegate responseReceivedDelegate = delegate(OBDRequest senderRequest, string data)
				{
					if (this.StopRequested)
					{
						return;
					}
					if (senderRequest.Command.EndsWith("01"))
					{
						Task.Delay(250).Wait();
					}
					if (data == null)
					{
						result = CodingRequestResult.NoData;
					}
					if (data.Contains("NO DATA") || data.Contains("ERROR"))
					{
						result = CodingRequestResult.NoData;
					}
					string[] array = OBDDataReader.FilterHexAndNewLineOnly(data).Split(new char[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
					IEnumerable<string> enumerable2 = array;
					Func<string, bool> func2;
					if ((func2 = <>9__3) == null)
					{
						func2 = (<>9__3 = (string line) => line.Contains(continueWithRepeatPattern));
					}
					if (enumerable2.Any(func2))
					{
						IProgress<string> progress3 = progress;
						if (progress3 != null)
						{
							progress3.Report("Операция выполняется");
						}
						App.OBDReader.ReplaceQueue(new OBDRequest[] { continueReq });
						return;
					}
					IEnumerable<string> enumerable3 = array;
					Func<string, bool> func3;
					if ((func3 = <>9__4) == null)
					{
						func3 = (<>9__4 = (string line) => line.Contains(finishedSuccessPattern) || line.Contains(finishedWithCode02Pattern));
					}
					if (enumerable3.Any(func3))
					{
						IProgress<string> progress4 = progress;
						if (progress4 != null)
						{
							progress4.Report("Операция завершена");
						}
						result = CodingRequestResult.Success;
						App.OBDReader.ReplaceQueue(new OBDRequest[] { resetReq });
						return;
					}
					IEnumerable<string> enumerable4 = array;
					Func<string, bool> func4;
					if ((func4 = <>9__5) == null)
					{
						func4 = (<>9__5 = (string line) => line.Contains(negativeCode03Pattern));
					}
					if (enumerable4.Any(func4))
					{
						IProgress<string> progress5 = progress;
						if (progress5 != null)
						{
							progress5.Report("Выполнение операции невозможно");
						}
						result = CodingRequestResult.UnknownError;
						App.OBDReader.ReplaceQueue(new OBDRequest[0]);
						return;
					}
					IEnumerable<string> enumerable5 = array;
					Func<string, bool> func5;
					if ((func5 = <>9__6) == null)
					{
						func5 = (<>9__6 = (string line) => line.Contains(negativePattern));
					}
					if (enumerable5.Any(func5))
					{
						IEnumerable<string> enumerable6 = array;
						Func<string, bool> func6;
						if ((func6 = <>9__7) == null)
						{
							func6 = (<>9__7 = (string line) => line.Contains(negativePattern));
						}
						string text3 = enumerable6.FirstOrDefault(func6);
						int num = text3.IndexOf(negativePattern);
						string text4 = text3.Substring(num, 8);
						string text5 = text4.Substring(text4.Length - 2);
						IProgress<string> progress6 = progress;
						if (progress6 != null)
						{
							progress6.Report("Выполнение операции невозможно\nКод ошибки: " + text5);
						}
						result = CodingRequestResult.UnknownError;
						App.OBDReader.ReplaceQueue(new OBDRequest[0]);
					}
				};
				obdrequest3.ResponseReceived += responseReceivedDelegate;
				continueReq.ResponseReceived += responseReceivedDelegate;
				App.OBDReader.ReplaceQueue(new OBDRequest[] { obdrequest, obdrequest3 });
				await App.OBDReader.WaitForCommandQueue();
			}
			return result;
		}

		// Token: 0x0600527F RID: 21119 RVA: 0x003F8DE4 File Offset: 0x003F6FE4
		public async Task<CodingRequestResult> UpdateCurrentState(string password, IProgress<string> progress = null)
		{
			this.CurrentState = MQBAdaptationTemplate.UNKNOWN_TITLE;
			string text = string.Concat(new string[] { "ATSH", this.RequestHeader, ";ATFCSH", this.RequestHeader, ";ATFCSD300000;ATFCSM1;ATST", this.ATST, ";ATCRA", this.ResponseHeader });
			string text2 = "ATAR;ATFCSM0";
			if (this.pid == null)
			{
				this.pid = new CustomPID("", "", "220100", "740", "0.1*(A*256+B)-3276.7", UnitsHelper.Units.grads, -100.0, 100.0, text, text2, false, Roles.None, CustomPIDType.Formula, 0, 1, 1.0, 1.0, 0.0, false, false, true, null);
				this.pid.ValueChanged -= this.Pid_ValueChanged;
				this.pid.ValueChanged += this.Pid_ValueChanged;
			}
			OBDRequest obdrequest = new OBDRequest(this.pid.Command, this.RequestHeader, text, text2, true, this.pid);
			OBDRequest obdrequest2 = new OBDRequest(this.OpenSessionCommand, this.RequestHeader, text, text2, false)
			{
				ELMFormat = ELMFormat.CAN11bit
			};
			App.OBDReader.ReplaceQueue(new OBDRequest[] { obdrequest2, obdrequest });
			return CodingRequestResult.Success;
		}

		// Token: 0x06005280 RID: 21120 RVA: 0x003F8E28 File Offset: 0x003F7028
		private void Pid_ValueChanged(object sender, PID e)
		{
			CustomPID customPID = (CustomPID)e;
			this.CurrentState = "Угол рулевого колеса = " + customPID.Value.ToString("0.00");
		}

		// Token: 0x04003249 RID: 12873
		protected string RequestHeader = "740";

		// Token: 0x0400324A RID: 12874
		protected string ResponseHeader = "760";

		// Token: 0x0400324B RID: 12875
		protected string ATST = "FA";

		// Token: 0x0400324C RID: 12876
		protected string ID = "02";

		// Token: 0x0400324D RID: 12877
		private MQBAdaptationOption removeShift;

		// Token: 0x0400324E RID: 12878
		private MQBAdaptationOption setShift;

		// Token: 0x0400324F RID: 12879
		[CompilerGenerated]
		private CodingGroup <Group>k__BackingField;

		// Token: 0x04003250 RID: 12880
		[CompilerGenerated]
		private string <Name>k__BackingField;

		// Token: 0x04003251 RID: 12881
		[CompilerGenerated]
		private string <Description>k__BackingField;

		// Token: 0x04003252 RID: 12882
		[CompilerGenerated]
		private string <InnerDescription>k__BackingField;

		// Token: 0x04003253 RID: 12883
		[CompilerGenerated]
		private string <OpenSessionCommand>k__BackingField;

		// Token: 0x04003254 RID: 12884
		private string _CurrenState = "";

		// Token: 0x04003255 RID: 12885
		[CompilerGenerated]
		private bool <HasCurrentState>k__BackingField;

		// Token: 0x04003256 RID: 12886
		protected bool StopRequested;

		// Token: 0x04003257 RID: 12887
		[CompilerGenerated]
		private ObservableCollection<MQBAdaptationOption> <Options>k__BackingField;

		// Token: 0x04003258 RID: 12888
		[CompilerGenerated]
		private bool <RequiresPro>k__BackingField;

		// Token: 0x04003259 RID: 12889
		[CompilerGenerated]
		private string <Password>k__BackingField;

		// Token: 0x0400325A RID: 12890
		[CompilerGenerated]
		private PropertyChangedEventHandler PropertyChanged;

		// Token: 0x0400325B RID: 12891
		private IPIDFloatValue pid;

		// Token: 0x02000A1B RID: 2587
		[CompilerGenerated]
		private sealed class <>c__DisplayClass57_0
		{
			// Token: 0x06005281 RID: 21121 RVA: 0x00002050 File Offset: 0x00000250
			public <>c__DisplayClass57_0()
			{
			}

			// Token: 0x0400325C RID: 12892
			public CodingRequestResult result;

			// Token: 0x0400325D RID: 12893
			public IProgress<string> progress;

			// Token: 0x0400325E RID: 12894
			public OBDRequest resetReq;

			// Token: 0x0400325F RID: 12895
			public VestaRenaultDriveWheelPositionSensor <>4__this;
		}

		// Token: 0x02000A1C RID: 2588
		[CompilerGenerated]
		private sealed class <>c__DisplayClass57_1
		{
			// Token: 0x06005282 RID: 21122 RVA: 0x00002050 File Offset: 0x00000250
			public <>c__DisplayClass57_1()
			{
			}

			// Token: 0x06005283 RID: 21123 RVA: 0x003F8E60 File Offset: 0x003F7060
			internal void <Execute>b__0(OBDRequest senderRequest, string data)
			{
				if (data == null)
				{
					this.CS$<>8__locals1.result = CodingRequestResult.NoData;
				}
				if (data.Contains("NO DATA") || data.Contains("ERROR"))
				{
					this.CS$<>8__locals1.result = CodingRequestResult.NoData;
				}
				IEnumerable<string> enumerable = OBDDataReader.FilterHexAndNewLineOnly(data).Split(new char[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
				Func<string, bool> func;
				if ((func = this.<>9__1) == null)
				{
					func = (this.<>9__1 = (string line) => line.Contains(this.finishedSuccessPattern));
				}
				if (enumerable.Any(func))
				{
					IProgress<string> progress = this.CS$<>8__locals1.progress;
					if (progress != null)
					{
						progress.Report("Операция завершена");
					}
					this.CS$<>8__locals1.result = CodingRequestResult.Success;
					App.OBDReader.ReplaceQueue(new OBDRequest[] { this.CS$<>8__locals1.resetReq });
				}
			}

			// Token: 0x06005284 RID: 21124 RVA: 0x003F8F2B File Offset: 0x003F712B
			internal bool <Execute>b__1(string line)
			{
				return line.Contains(this.finishedSuccessPattern);
			}

			// Token: 0x04003260 RID: 12896
			public string finishedSuccessPattern;

			// Token: 0x04003261 RID: 12897
			public VestaRenaultDriveWheelPositionSensor.<>c__DisplayClass57_0 CS$<>8__locals1;

			// Token: 0x04003262 RID: 12898
			public Func<string, bool> <>9__1;
		}

		// Token: 0x02000A1D RID: 2589
		[CompilerGenerated]
		private sealed class <>c__DisplayClass57_2
		{
			// Token: 0x06005285 RID: 21125 RVA: 0x00002050 File Offset: 0x00000250
			public <>c__DisplayClass57_2()
			{
			}

			// Token: 0x06005286 RID: 21126 RVA: 0x003F8F3C File Offset: 0x003F713C
			internal void <Execute>b__2(OBDRequest senderRequest, string data)
			{
				if (this.CS$<>8__locals2.<>4__this.StopRequested)
				{
					return;
				}
				if (senderRequest.Command.EndsWith("01"))
				{
					Task.Delay(250).Wait();
				}
				if (data == null)
				{
					this.CS$<>8__locals2.result = CodingRequestResult.NoData;
				}
				if (data.Contains("NO DATA") || data.Contains("ERROR"))
				{
					this.CS$<>8__locals2.result = CodingRequestResult.NoData;
				}
				string[] array = OBDDataReader.FilterHexAndNewLineOnly(data).Split(new char[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
				IEnumerable<string> enumerable = array;
				Func<string, bool> func;
				if ((func = this.<>9__3) == null)
				{
					func = (this.<>9__3 = (string line) => line.Contains(this.continueWithRepeatPattern));
				}
				if (enumerable.Any(func))
				{
					IProgress<string> progress = this.CS$<>8__locals2.progress;
					if (progress != null)
					{
						progress.Report("Операция выполняется");
					}
					App.OBDReader.ReplaceQueue(new OBDRequest[] { this.continueReq });
					return;
				}
				IEnumerable<string> enumerable2 = array;
				Func<string, bool> func2;
				if ((func2 = this.<>9__4) == null)
				{
					func2 = (this.<>9__4 = (string line) => line.Contains(this.finishedSuccessPattern) || line.Contains(this.finishedWithCode02Pattern));
				}
				if (enumerable2.Any(func2))
				{
					IProgress<string> progress2 = this.CS$<>8__locals2.progress;
					if (progress2 != null)
					{
						progress2.Report("Операция завершена");
					}
					this.CS$<>8__locals2.result = CodingRequestResult.Success;
					App.OBDReader.ReplaceQueue(new OBDRequest[] { this.CS$<>8__locals2.resetReq });
					return;
				}
				IEnumerable<string> enumerable3 = array;
				Func<string, bool> func3;
				if ((func3 = this.<>9__5) == null)
				{
					func3 = (this.<>9__5 = (string line) => line.Contains(this.negativeCode03Pattern));
				}
				if (enumerable3.Any(func3))
				{
					IProgress<string> progress3 = this.CS$<>8__locals2.progress;
					if (progress3 != null)
					{
						progress3.Report("Выполнение операции невозможно");
					}
					this.CS$<>8__locals2.result = CodingRequestResult.UnknownError;
					App.OBDReader.ReplaceQueue(new OBDRequest[0]);
					return;
				}
				IEnumerable<string> enumerable4 = array;
				Func<string, bool> func4;
				if ((func4 = this.<>9__6) == null)
				{
					func4 = (this.<>9__6 = (string line) => line.Contains(this.negativePattern));
				}
				if (enumerable4.Any(func4))
				{
					IEnumerable<string> enumerable5 = array;
					Func<string, bool> func5;
					if ((func5 = this.<>9__7) == null)
					{
						func5 = (this.<>9__7 = (string line) => line.Contains(this.negativePattern));
					}
					string text = enumerable5.FirstOrDefault(func5);
					int num = text.IndexOf(this.negativePattern);
					string text2 = text.Substring(num, 8);
					string text3 = text2.Substring(text2.Length - 2);
					IProgress<string> progress4 = this.CS$<>8__locals2.progress;
					if (progress4 != null)
					{
						progress4.Report("Выполнение операции невозможно\nКод ошибки: " + text3);
					}
					this.CS$<>8__locals2.result = CodingRequestResult.UnknownError;
					App.OBDReader.ReplaceQueue(new OBDRequest[0]);
				}
			}

			// Token: 0x06005287 RID: 21127 RVA: 0x003F91A7 File Offset: 0x003F73A7
			internal bool <Execute>b__3(string line)
			{
				return line.Contains(this.continueWithRepeatPattern);
			}

			// Token: 0x06005288 RID: 21128 RVA: 0x003F91B5 File Offset: 0x003F73B5
			internal bool <Execute>b__4(string line)
			{
				return line.Contains(this.finishedSuccessPattern) || line.Contains(this.finishedWithCode02Pattern);
			}

			// Token: 0x06005289 RID: 21129 RVA: 0x003F91D3 File Offset: 0x003F73D3
			internal bool <Execute>b__5(string line)
			{
				return line.Contains(this.negativeCode03Pattern);
			}

			// Token: 0x0600528A RID: 21130 RVA: 0x003F91E1 File Offset: 0x003F73E1
			internal bool <Execute>b__6(string line)
			{
				return line.Contains(this.negativePattern);
			}

			// Token: 0x0600528B RID: 21131 RVA: 0x003F91E1 File Offset: 0x003F73E1
			internal bool <Execute>b__7(string line)
			{
				return line.Contains(this.negativePattern);
			}

			// Token: 0x04003263 RID: 12899
			public string continueWithRepeatPattern;

			// Token: 0x04003264 RID: 12900
			public OBDRequest continueReq;

			// Token: 0x04003265 RID: 12901
			public string finishedSuccessPattern;

			// Token: 0x04003266 RID: 12902
			public string finishedWithCode02Pattern;

			// Token: 0x04003267 RID: 12903
			public string negativeCode03Pattern;

			// Token: 0x04003268 RID: 12904
			public string negativePattern;

			// Token: 0x04003269 RID: 12905
			public VestaRenaultDriveWheelPositionSensor.<>c__DisplayClass57_0 CS$<>8__locals2;

			// Token: 0x0400326A RID: 12906
			public Func<string, bool> <>9__3;

			// Token: 0x0400326B RID: 12907
			public Func<string, bool> <>9__4;

			// Token: 0x0400326C RID: 12908
			public Func<string, bool> <>9__5;

			// Token: 0x0400326D RID: 12909
			public Func<string, bool> <>9__6;

			// Token: 0x0400326E RID: 12910
			public Func<string, bool> <>9__7;
		}

		// Token: 0x02000A1E RID: 2590
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <Execute>d__57 : IAsyncStateMachine
		{
			// Token: 0x0600528C RID: 21132 RVA: 0x003F91F0 File Offset: 0x003F73F0
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				VestaRenaultDriveWheelPositionSensor vestaRenaultDriveWheelPositionSensor = this;
				CodingRequestResult result;
				try
				{
					TaskAwaiter taskAwaiter;
					TaskAwaiter taskAwaiter2;
					if (num != 0)
					{
						if (num != 1)
						{
							CS$<>8__locals1 = new VestaRenaultDriveWheelPositionSensor.<>c__DisplayClass57_0();
							CS$<>8__locals1.progress = progress;
							CS$<>8__locals1.<>4__this = this;
							CS$<>8__locals1.result = CodingRequestResult.UnknownError;
							string text = string.Concat(new string[] { "ATSH", vestaRenaultDriveWheelPositionSensor.RequestHeader, ";ATFCSH", vestaRenaultDriveWheelPositionSensor.RequestHeader, ";ATFCSD300000;ATFCSM1;ATST", vestaRenaultDriveWheelPositionSensor.ATST, ";ATCRA", vestaRenaultDriveWheelPositionSensor.ResponseHeader });
							string text2 = "ATAR;ATFCSM0";
							OBDRequest obdrequest = new OBDRequest(vestaRenaultDriveWheelPositionSensor.OpenSessionCommand, vestaRenaultDriveWheelPositionSensor.RequestHeader, text, text2, false)
							{
								ELMFormat = ELMFormat.CAN11bit
							};
							CS$<>8__locals1.resetReq = new OBDRequest("1102", vestaRenaultDriveWheelPositionSensor.RequestHeader, text, text2, false)
							{
								ELMFormat = ELMFormat.CAN11bit
							};
							if (value == vestaRenaultDriveWheelPositionSensor.removeShift.Value)
							{
								VestaRenaultDriveWheelPositionSensor.<>c__DisplayClass57_1 CS$<>8__locals2 = new VestaRenaultDriveWheelPositionSensor.<>c__DisplayClass57_1();
								CS$<>8__locals2.CS$<>8__locals1 = CS$<>8__locals1;
								"037F31" + "78";
								CS$<>8__locals2.finishedSuccessPattern = "7101";
								OBDRequest obdrequest2 = new OBDRequest("31010000", vestaRenaultDriveWheelPositionSensor.RequestHeader, text, text2, false);
								obdrequest2.ResponseReceived += delegate(OBDRequest senderRequest, string data)
								{
									if (data == null)
									{
										CS$<>8__locals2.CS$<>8__locals1.result = CodingRequestResult.NoData;
									}
									if (data.Contains("NO DATA") || data.Contains("ERROR"))
									{
										CS$<>8__locals2.CS$<>8__locals1.result = CodingRequestResult.NoData;
									}
									IEnumerable<string> enumerable = OBDDataReader.FilterHexAndNewLineOnly(data).Split(new char[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
									Func<string, bool> func;
									if ((func = CS$<>8__locals2.<>9__1) == null)
									{
										func = (CS$<>8__locals2.<>9__1 = (string line) => line.Contains(CS$<>8__locals2.finishedSuccessPattern));
									}
									if (enumerable.Any(func))
									{
										IProgress<string> progress = CS$<>8__locals2.CS$<>8__locals1.progress;
										if (progress != null)
										{
											progress.Report("Операция завершена");
										}
										CS$<>8__locals2.CS$<>8__locals1.result = CodingRequestResult.Success;
										App.OBDReader.ReplaceQueue(new OBDRequest[] { CS$<>8__locals2.CS$<>8__locals1.resetReq });
									}
								};
								App.OBDReader.ReplaceQueue(new OBDRequest[] { obdrequest, obdrequest2 });
								taskAwaiter = App.OBDReader.WaitForCommandQueue().GetAwaiter();
								if (!taskAwaiter.IsCompleted)
								{
									num2 = 0;
									taskAwaiter2 = taskAwaiter;
									this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, VestaRenaultDriveWheelPositionSensor.<Execute>d__57>(ref taskAwaiter, ref this);
									return;
								}
								goto IL_01D9;
							}
							else
							{
								if (!(value == vestaRenaultDriveWheelPositionSensor.setShift.Value))
								{
									goto IL_0334;
								}
								VestaRenaultDriveWheelPositionSensor.<>c__DisplayClass57_2 CS$<>8__locals3 = new VestaRenaultDriveWheelPositionSensor.<>c__DisplayClass57_2();
								CS$<>8__locals3.CS$<>8__locals2 = CS$<>8__locals1;
								CS$<>8__locals3.negativePattern = "037F31";
								CS$<>8__locals3.negativePattern + "78";
								CS$<>8__locals3.continueWithRepeatPattern = "710201";
								CS$<>8__locals3.finishedSuccessPattern = "710202";
								CS$<>8__locals3.negativeCode03Pattern = "710204";
								CS$<>8__locals3.finishedWithCode02Pattern = "7102";
								OBDRequest obdrequest3 = new OBDRequest(value, vestaRenaultDriveWheelPositionSensor.RequestHeader, text, text2, false)
								{
									ELMFormat = ELMFormat.CAN11bit
								};
								CS$<>8__locals3.continueReq = new OBDRequest("310201", vestaRenaultDriveWheelPositionSensor.RequestHeader, text, text2, false)
								{
									ELMFormat = ELMFormat.CAN11bit
								};
								ResponseReceivedDelegate responseReceivedDelegate = delegate(OBDRequest senderRequest, string data)
								{
									if (CS$<>8__locals3.CS$<>8__locals2.<>4__this.StopRequested)
									{
										return;
									}
									if (senderRequest.Command.EndsWith("01"))
									{
										Task.Delay(250).Wait();
									}
									if (data == null)
									{
										CS$<>8__locals3.CS$<>8__locals2.result = CodingRequestResult.NoData;
									}
									if (data.Contains("NO DATA") || data.Contains("ERROR"))
									{
										CS$<>8__locals3.CS$<>8__locals2.result = CodingRequestResult.NoData;
									}
									string[] array = OBDDataReader.FilterHexAndNewLineOnly(data).Split(new char[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
									IEnumerable<string> enumerable2 = array;
									Func<string, bool> func2;
									if ((func2 = CS$<>8__locals3.<>9__3) == null)
									{
										func2 = (CS$<>8__locals3.<>9__3 = (string line) => line.Contains(CS$<>8__locals3.continueWithRepeatPattern));
									}
									if (enumerable2.Any(func2))
									{
										IProgress<string> progress2 = CS$<>8__locals3.CS$<>8__locals2.progress;
										if (progress2 != null)
										{
											progress2.Report("Операция выполняется");
										}
										App.OBDReader.ReplaceQueue(new OBDRequest[] { CS$<>8__locals3.continueReq });
										return;
									}
									IEnumerable<string> enumerable3 = array;
									Func<string, bool> func3;
									if ((func3 = CS$<>8__locals3.<>9__4) == null)
									{
										func3 = (CS$<>8__locals3.<>9__4 = (string line) => line.Contains(CS$<>8__locals3.finishedSuccessPattern) || line.Contains(CS$<>8__locals3.finishedWithCode02Pattern));
									}
									if (enumerable3.Any(func3))
									{
										IProgress<string> progress3 = CS$<>8__locals3.CS$<>8__locals2.progress;
										if (progress3 != null)
										{
											progress3.Report("Операция завершена");
										}
										CS$<>8__locals3.CS$<>8__locals2.result = CodingRequestResult.Success;
										App.OBDReader.ReplaceQueue(new OBDRequest[] { CS$<>8__locals3.CS$<>8__locals2.resetReq });
										return;
									}
									IEnumerable<string> enumerable4 = array;
									Func<string, bool> func4;
									if ((func4 = CS$<>8__locals3.<>9__5) == null)
									{
										func4 = (CS$<>8__locals3.<>9__5 = (string line) => line.Contains(CS$<>8__locals3.negativeCode03Pattern));
									}
									if (enumerable4.Any(func4))
									{
										IProgress<string> progress4 = CS$<>8__locals3.CS$<>8__locals2.progress;
										if (progress4 != null)
										{
											progress4.Report("Выполнение операции невозможно");
										}
										CS$<>8__locals3.CS$<>8__locals2.result = CodingRequestResult.UnknownError;
										App.OBDReader.ReplaceQueue(new OBDRequest[0]);
										return;
									}
									IEnumerable<string> enumerable5 = array;
									Func<string, bool> func5;
									if ((func5 = CS$<>8__locals3.<>9__6) == null)
									{
										func5 = (CS$<>8__locals3.<>9__6 = (string line) => line.Contains(CS$<>8__locals3.negativePattern));
									}
									if (enumerable5.Any(func5))
									{
										IEnumerable<string> enumerable6 = array;
										Func<string, bool> func6;
										if ((func6 = CS$<>8__locals3.<>9__7) == null)
										{
											func6 = (CS$<>8__locals3.<>9__7 = (string line) => line.Contains(CS$<>8__locals3.negativePattern));
										}
										string text3 = enumerable6.FirstOrDefault(func6);
										int num3 = text3.IndexOf(CS$<>8__locals3.negativePattern);
										string text4 = text3.Substring(num3, 8);
										string text5 = text4.Substring(text4.Length - 2);
										IProgress<string> progress5 = CS$<>8__locals3.CS$<>8__locals2.progress;
										if (progress5 != null)
										{
											progress5.Report("Выполнение операции невозможно\nКод ошибки: " + text5);
										}
										CS$<>8__locals3.CS$<>8__locals2.result = CodingRequestResult.UnknownError;
										App.OBDReader.ReplaceQueue(new OBDRequest[0]);
									}
								};
								obdrequest3.ResponseReceived += responseReceivedDelegate;
								CS$<>8__locals3.continueReq.ResponseReceived += responseReceivedDelegate;
								App.OBDReader.ReplaceQueue(new OBDRequest[] { obdrequest, obdrequest3 });
								taskAwaiter = App.OBDReader.WaitForCommandQueue().GetAwaiter();
								if (!taskAwaiter.IsCompleted)
								{
									num2 = 1;
									taskAwaiter2 = taskAwaiter;
									this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, VestaRenaultDriveWheelPositionSensor.<Execute>d__57>(ref taskAwaiter, ref this);
									return;
								}
							}
						}
						else
						{
							taskAwaiter = taskAwaiter2;
							taskAwaiter2 = default(TaskAwaiter);
							num2 = -1;
						}
						taskAwaiter.GetResult();
						goto IL_0334;
					}
					taskAwaiter = taskAwaiter2;
					taskAwaiter2 = default(TaskAwaiter);
					num2 = -1;
					IL_01D9:
					taskAwaiter.GetResult();
					IL_0334:
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

			// Token: 0x0600528D RID: 21133 RVA: 0x003F9598 File Offset: 0x003F7798
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x0400326F RID: 12911
			public int <>1__state;

			// Token: 0x04003270 RID: 12912
			public AsyncTaskMethodBuilder<CodingRequestResult> <>t__builder;

			// Token: 0x04003271 RID: 12913
			public IProgress<string> progress;

			// Token: 0x04003272 RID: 12914
			public VestaRenaultDriveWheelPositionSensor <>4__this;

			// Token: 0x04003273 RID: 12915
			public string value;

			// Token: 0x04003274 RID: 12916
			private VestaRenaultDriveWheelPositionSensor.<>c__DisplayClass57_0 <>8__1;

			// Token: 0x04003275 RID: 12917
			private TaskAwaiter <>u__1;
		}

		// Token: 0x02000A1F RID: 2591
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <UpdateCurrentState>d__59 : IAsyncStateMachine
		{
			// Token: 0x0600528E RID: 21134 RVA: 0x003F95A8 File Offset: 0x003F77A8
			void IAsyncStateMachine.MoveNext()
			{
				VestaRenaultDriveWheelPositionSensor vestaRenaultDriveWheelPositionSensor = this;
				CodingRequestResult codingRequestResult;
				try
				{
					vestaRenaultDriveWheelPositionSensor.CurrentState = MQBAdaptationTemplate.UNKNOWN_TITLE;
					string text = string.Concat(new string[] { "ATSH", vestaRenaultDriveWheelPositionSensor.RequestHeader, ";ATFCSH", vestaRenaultDriveWheelPositionSensor.RequestHeader, ";ATFCSD300000;ATFCSM1;ATST", vestaRenaultDriveWheelPositionSensor.ATST, ";ATCRA", vestaRenaultDriveWheelPositionSensor.ResponseHeader });
					string text2 = "ATAR;ATFCSM0";
					if (vestaRenaultDriveWheelPositionSensor.pid == null)
					{
						vestaRenaultDriveWheelPositionSensor.pid = new CustomPID("", "", "220100", "740", "0.1*(A*256+B)-3276.7", UnitsHelper.Units.grads, -100.0, 100.0, text, text2, false, Roles.None, CustomPIDType.Formula, 0, 1, 1.0, 1.0, 0.0, false, false, true, null);
						vestaRenaultDriveWheelPositionSensor.pid.ValueChanged -= vestaRenaultDriveWheelPositionSensor.Pid_ValueChanged;
						vestaRenaultDriveWheelPositionSensor.pid.ValueChanged += vestaRenaultDriveWheelPositionSensor.Pid_ValueChanged;
					}
					OBDRequest obdrequest = new OBDRequest(vestaRenaultDriveWheelPositionSensor.pid.Command, vestaRenaultDriveWheelPositionSensor.RequestHeader, text, text2, true, vestaRenaultDriveWheelPositionSensor.pid);
					OBDRequest obdrequest2 = new OBDRequest(vestaRenaultDriveWheelPositionSensor.OpenSessionCommand, vestaRenaultDriveWheelPositionSensor.RequestHeader, text, text2, false)
					{
						ELMFormat = ELMFormat.CAN11bit
					};
					App.OBDReader.ReplaceQueue(new OBDRequest[] { obdrequest2, obdrequest });
					codingRequestResult = CodingRequestResult.Success;
				}
				catch (Exception ex)
				{
					this.<>1__state = -2;
					this.<>t__builder.SetException(ex);
					return;
				}
				this.<>1__state = -2;
				this.<>t__builder.SetResult(codingRequestResult);
			}

			// Token: 0x0600528F RID: 21135 RVA: 0x003F9758 File Offset: 0x003F7958
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x04003276 RID: 12918
			public int <>1__state;

			// Token: 0x04003277 RID: 12919
			public AsyncTaskMethodBuilder<CodingRequestResult> <>t__builder;

			// Token: 0x04003278 RID: 12920
			public VestaRenaultDriveWheelPositionSensor <>4__this;
		}
	}
}
