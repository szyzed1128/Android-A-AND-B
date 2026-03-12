using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using CarScannerXamarinForms.Common;
using CarScannerXamarinForms.OBD2;
using CarScannerXamarinForms.Settings;

namespace CarScannerXamarinForms.Coding.DB.MQB
{
	// Token: 0x02000B59 RID: 2905
	internal class TailGateParametrizeCustomizationCoding : CustomizableCodingTemplate
	{
		// Token: 0x060059B2 RID: 22962 RVA: 0x0042C368 File Offset: 0x0042A568
		public TailGateParametrizeCustomizationCoding()
		{
			this.Group = CodingGroup.Boot;
			base.Name = "Tailgate dataset customization";
			base.Description = "Compatibility: 5Q0959107 with supported datasets";
			this.ValueType = AdaptationValueTypes.TailgateCustomization;
			base.RequestHeader = "723";
			base.ResponseHeader = "78D";
			this.Password = "20103";
			base.PasswordHint = "20103";
			this.ReadModeAndAddress = "";
			base.WriteModeAndAddress = "2E720D";
			base.OpenSessionCommand = "1040";
			base.MakeChangesToInitialData = true;
			base.PreReadCommands = "";
			base.PreWriteCommands = "1040;22F198;22F199;22F1A0;22F1A1;2703;2704;2EF198;2EF199;2EF1A0;2EF1A1";
			base.PostWriteCommands = "14FFFFFF;1102;1003;14FFFFFF;";
		}

		// Token: 0x17001849 RID: 6217
		// (get) Token: 0x060059B3 RID: 22963 RVA: 0x0042C441 File Offset: 0x0042A641
		// (set) Token: 0x060059B4 RID: 22964 RVA: 0x0042C449 File Offset: 0x0042A649
		public bool DisplayControls
		{
			get
			{
				return this._DisplayControls;
			}
			set
			{
				if (this._DisplayControls != value)
				{
					this._DisplayControls = value;
					base.OnPropertyChanged("DisplayControls");
				}
			}
		}

		// Token: 0x1700184A RID: 6218
		// (get) Token: 0x060059B5 RID: 22965 RVA: 0x000A8D6F File Offset: 0x000A6F6F
		// (set) Token: 0x060059B6 RID: 22966 RVA: 0x003F0A8F File Offset: 0x003EEC8F
		public override bool HasCurrentState
		{
			get
			{
				return true;
			}
			set
			{
				base.HasCurrentState = value;
			}
		}

		// Token: 0x1700184B RID: 6219
		// (get) Token: 0x060059B7 RID: 22967 RVA: 0x0042C466 File Offset: 0x0042A666
		// (set) Token: 0x060059B8 RID: 22968 RVA: 0x0042C46E File Offset: 0x0042A66E
		public string PartNumber
		{
			get
			{
				return this._PartNumber;
			}
			set
			{
				if (value != this._PartNumber)
				{
					this._PartNumber = value;
					base.OnPropertyChanged("PartNumber");
				}
			}
		}

		// Token: 0x1700184C RID: 6220
		// (get) Token: 0x060059B9 RID: 22969 RVA: 0x0042C490 File Offset: 0x0042A690
		// (set) Token: 0x060059BA RID: 22970 RVA: 0x0042C498 File Offset: 0x0042A698
		public string DSVersion
		{
			get
			{
				return this._DSVersion;
			}
			set
			{
				if (value != this._DSVersion)
				{
					this._DSVersion = value;
					base.OnPropertyChanged("DSVersion");
				}
			}
		}

		// Token: 0x1700184D RID: 6221
		// (get) Token: 0x060059BB RID: 22971 RVA: 0x0042C4BA File Offset: 0x0042A6BA
		// (set) Token: 0x060059BC RID: 22972 RVA: 0x0042C4C2 File Offset: 0x0042A6C2
		public ObservableCollection<ProxyFile> AvailableDatasets
		{
			[CompilerGenerated]
			get
			{
				return this.<AvailableDatasets>k__BackingField;
			}
			[CompilerGenerated]
			protected set
			{
				this.<AvailableDatasets>k__BackingField = value;
			}
		} = new ObservableCollection<ProxyFile>();

		// Token: 0x1700184E RID: 6222
		// (get) Token: 0x060059BD RID: 22973 RVA: 0x0042C4CB File Offset: 0x0042A6CB
		// (set) Token: 0x060059BE RID: 22974 RVA: 0x0042C4D3 File Offset: 0x0042A6D3
		public MQB_6D_DatasetBuilderModel Model
		{
			get
			{
				return this._Model;
			}
			set
			{
				this._Model = value;
				base.OnPropertyChanged("Model");
			}
		}

		// Token: 0x060059BF RID: 22975 RVA: 0x0042C4E8 File Offset: 0x0042A6E8
		public override async Task<CodingRequestResult> UpdateCurrentState(string password, IProgress<string> progress = null)
		{
			this.PartNumber = "";
			this.DSVersion = "";
			this.LongCoding = null;
			this.DisplayControls = false;
			base.CurrentState = MQBAdaptationTemplate.UNKNOWN_TITLE;
			this.BuildDefaultBeforeAndAfterCommands();
			OBDRequest obdrequest = new OBDRequest("22F187", base.RequestHeader, this.BeforeCommands, this.AfterCommands, false);
			obdrequest.ResponseDecoded += this.DeviceRequest_ResponseDecoded;
			obdrequest.ForceManualFlowControl = SharedSettings.Current.ForceUseManualFlowControlForCodingOperations;
			OBDRequest obdrequest2 = new OBDRequest("22F1B1", base.RequestHeader, this.BeforeCommands, this.AfterCommands, false);
			obdrequest2.ResponseDecoded += this.DatasetVersionsRequest_ResponseDecoded;
			obdrequest2.ForceManualFlowControl = SharedSettings.Current.ForceUseManualFlowControlForCodingOperations;
			OBDRequest obdrequest3 = new OBDRequest("220600", base.RequestHeader, this.BeforeCommands, this.AfterCommands, false);
			obdrequest3.ResponseDecoded += this.LongCodingRequest_ResponseDecoded;
			obdrequest3.ForceManualFlowControl = SharedSettings.Current.ForceUseManualFlowControlForCodingOperations;
			App.OBDReader.ReplaceQueue(new OBDRequest[] { obdrequest, obdrequest2, obdrequest3 });
			await App.OBDReader.WaitForCommandQueue();
			await Task.Delay(1000);
			CodingRequestResult codingRequestResult;
			if (string.IsNullOrEmpty(this.DSVersion) || string.IsNullOrEmpty(this.PartNumber) || this.LongCoding == null)
			{
				base.CurrentState = MQBAdaptationTemplate.UNKNOWN_TITLE;
				this.DisplayControls = false;
				codingRequestResult = CodingRequestResult.NoData;
			}
			else
			{
				string text = "vag.tailgate6d";
				string[] filesInDirectory = PackageFileReader.GetFilesInDirectory(text + ".");
				string filenamepattern = this.PartNumber + "_" + this.DSVersion;
				string text2 = filesInDirectory.FirstOrDefault((string x) => x.Contains(filenamepattern, StringComparison.OrdinalIgnoreCase));
				if (text2 == null)
				{
					filenamepattern = "_" + this.DSVersion + "_";
					text2 = filesInDirectory.FirstOrDefault((string x) => x.Contains(filenamepattern, StringComparison.OrdinalIgnoreCase));
				}
				if (text2 == null)
				{
					base.CurrentState = MQBAdaptationTemplate.UNSUPPORTED_TITLE;
					codingRequestResult = CodingRequestResult.NotSupported;
				}
				else
				{
					using (Stream stream = PackageFileReader.OpenFileStream(text + "." + text2))
					{
						byte[] array = new byte[stream.Length];
						stream.Read(array, 0, array.Length);
						MQB_6D_DatasetBuilderModel mqb_6D_DatasetBuilderModel = new MQB_6D_DatasetBuilderModel();
						mqb_6D_DatasetBuilderModel.LoadFromBytes(array, this.PartNumber, this.LongCoding);
						this.Model = mqb_6D_DatasetBuilderModel;
						base.CurrentState = this.PartNumber + " / " + this.DSVersion;
						this.DisplayControls = true;
						codingRequestResult = CodingRequestResult.Success;
					}
				}
			}
			return codingRequestResult;
		}

		// Token: 0x060059C0 RID: 22976 RVA: 0x0042C52C File Offset: 0x0042A72C
		public override async Task<CodingRequestResult> Execute(string password, string value, string UserFriendlyValue, IProgress<string> progress, byte[] originalData = null, bool skipIfTheSameData = false)
		{
			CodingRequestResult codingRequestResult;
			if (string.IsNullOrEmpty(this.DSVersion) || string.IsNullOrEmpty(this.PartNumber) || this.LongCoding == null)
			{
				codingRequestResult = CodingRequestResult.NoData;
			}
			else
			{
				byte[] array = this.Model.BuildNewData();
				string checked_value = BitHelpers.ByteArrayToHexString(array);
				originalData = this.Model.GetOriginalData();
				CodingRequestResult codingRequestResult2 = await this.WriteDataToECU(password, string.Concat(new string[] { UserFriendlyValue, " (PN:", this.PartNumber, "; Ver.:", this.DSVersion, ")" }), progress, originalData, checked_value);
				if (codingRequestResult2 == CodingRequestResult.Success)
				{
					Dictionary<string, string> dictionary = new Dictionary<string, string>();
					dictionary.Add("Protocol", base.Protocol);
					CodingLogItem.RecordToLog(base.Name, UserFriendlyValue, CodingLogItem.CodingTypes.CustomizableCodingTemplate, base.WriteModeAndAddress, BitHelpers.ByteArrayToHexString(originalData), checked_value, password, base.RequestHeader, base.ResponseHeader, base.OpenSessionCommand, base.PreWriteCommands, base.PostWriteCommands, base.ATST, dictionary, base.PreReadCommands);
				}
				codingRequestResult = codingRequestResult2;
			}
			return codingRequestResult;
		}

		// Token: 0x060059C1 RID: 22977 RVA: 0x0042C591 File Offset: 0x0042A791
		private void LongCodingRequest_ResponseDecoded(OBDRequest request, byte[] data, bool decodeResult, string responseHeader)
		{
			if (data != null && data.Length != 0)
			{
				this.LongCoding = data;
			}
		}

		// Token: 0x060059C2 RID: 22978 RVA: 0x0042C5A4 File Offset: 0x0042A7A4
		private void DatasetVersionsRequest_ResponseDecoded(OBDRequest request, byte[] data, bool decodeResult, string responseHeader)
		{
			if (data != null && data.Length > 6)
			{
				for (int i = 0; i < data.Length; i += 7)
				{
					string text = data[i].ToString("X2") + data[i + 1].ToString("X2");
					string @string = Encoding.ASCII.GetString(data, i + 2, 4);
					byte b = data[i + 6];
					if (text == "720D")
					{
						this.DSVersion = @string;
						return;
					}
				}
			}
		}

		// Token: 0x060059C3 RID: 22979 RVA: 0x0042C61C File Offset: 0x0042A81C
		private void DeviceRequest_ResponseDecoded(OBDRequest request, byte[] data, bool decodeResult, string responseHeader)
		{
			if (data != null && data.Length != 0)
			{
				string text = new string((from x in data
					select (char)x into ch
					where (byte)ch >= 32 && (byte)ch <= 126
					select ch).ToArray<char>());
				if (text == null)
				{
					text = "";
				}
				text = text.Trim();
				this.PartNumber = text;
			}
		}

		// Token: 0x0400381B RID: 14363
		private bool _DisplayControls;

		// Token: 0x0400381C RID: 14364
		private string _PartNumber = "";

		// Token: 0x0400381D RID: 14365
		private string _DSVersion = "";

		// Token: 0x0400381E RID: 14366
		public byte[] LongCoding;

		// Token: 0x0400381F RID: 14367
		[CompilerGenerated]
		private ObservableCollection<ProxyFile> <AvailableDatasets>k__BackingField;

		// Token: 0x04003820 RID: 14368
		private MQB_6D_DatasetBuilderModel _Model = new MQB_6D_DatasetBuilderModel();

		// Token: 0x02000B5A RID: 2906
		[CompilerGenerated]
		[Serializable]
		private sealed class <>c
		{
			// Token: 0x060059C4 RID: 22980 RVA: 0x0042C69B File Offset: 0x0042A89B
			// Note: this type is marked as 'beforefieldinit'.
			static <>c()
			{
			}

			// Token: 0x060059C5 RID: 22981 RVA: 0x00002050 File Offset: 0x00000250
			public <>c()
			{
			}

			// Token: 0x060059C6 RID: 22982 RVA: 0x00016849 File Offset: 0x00014A49
			internal char <DeviceRequest_ResponseDecoded>b__29_0(byte x)
			{
				return (char)x;
			}

			// Token: 0x060059C7 RID: 22983 RVA: 0x00216CDB File Offset: 0x00214EDB
			internal bool <DeviceRequest_ResponseDecoded>b__29_1(char ch)
			{
				return (byte)ch >= 32 && (byte)ch <= 126;
			}

			// Token: 0x04003821 RID: 14369
			public static readonly TailGateParametrizeCustomizationCoding.<>c <>9 = new TailGateParametrizeCustomizationCoding.<>c();

			// Token: 0x04003822 RID: 14370
			public static Func<byte, char> <>9__29_0;

			// Token: 0x04003823 RID: 14371
			public static Func<char, bool> <>9__29_1;
		}

		// Token: 0x02000B5B RID: 2907
		[CompilerGenerated]
		private sealed class <>c__DisplayClass25_0
		{
			// Token: 0x060059C8 RID: 22984 RVA: 0x00002050 File Offset: 0x00000250
			public <>c__DisplayClass25_0()
			{
			}

			// Token: 0x060059C9 RID: 22985 RVA: 0x0042C6A7 File Offset: 0x0042A8A7
			internal bool <UpdateCurrentState>b__0(string x)
			{
				return x.Contains(this.filenamepattern, StringComparison.OrdinalIgnoreCase);
			}

			// Token: 0x060059CA RID: 22986 RVA: 0x0042C6A7 File Offset: 0x0042A8A7
			internal bool <UpdateCurrentState>b__1(string x)
			{
				return x.Contains(this.filenamepattern, StringComparison.OrdinalIgnoreCase);
			}

			// Token: 0x04003824 RID: 14372
			public string filenamepattern;
		}

		// Token: 0x02000B5C RID: 2908
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <Execute>d__26 : IAsyncStateMachine
		{
			// Token: 0x060059CB RID: 22987 RVA: 0x0042C6B8 File Offset: 0x0042A8B8
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				TailGateParametrizeCustomizationCoding tailGateParametrizeCustomizationCoding = this;
				CodingRequestResult codingRequestResult;
				try
				{
					TaskAwaiter<CodingRequestResult> taskAwaiter;
					if (num != 0)
					{
						if (string.IsNullOrEmpty(tailGateParametrizeCustomizationCoding.DSVersion) || string.IsNullOrEmpty(tailGateParametrizeCustomizationCoding.PartNumber) || tailGateParametrizeCustomizationCoding.LongCoding == null)
						{
							codingRequestResult = CodingRequestResult.NoData;
							goto IL_01B5;
						}
						byte[] array = tailGateParametrizeCustomizationCoding.Model.BuildNewData();
						checked_value = BitHelpers.ByteArrayToHexString(array);
						originalData = tailGateParametrizeCustomizationCoding.Model.GetOriginalData();
						taskAwaiter = tailGateParametrizeCustomizationCoding.WriteDataToECU(password, string.Concat(new string[] { UserFriendlyValue, " (PN:", tailGateParametrizeCustomizationCoding.PartNumber, "; Ver.:", tailGateParametrizeCustomizationCoding.DSVersion, ")" }), progress, originalData, checked_value).GetAwaiter();
						if (!taskAwaiter.IsCompleted)
						{
							num2 = 0;
							TaskAwaiter<CodingRequestResult> taskAwaiter2 = taskAwaiter;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<CodingRequestResult>, TailGateParametrizeCustomizationCoding.<Execute>d__26>(ref taskAwaiter, ref this);
							return;
						}
					}
					else
					{
						TaskAwaiter<CodingRequestResult> taskAwaiter2;
						taskAwaiter = taskAwaiter2;
						taskAwaiter2 = default(TaskAwaiter<CodingRequestResult>);
						num2 = -1;
					}
					CodingRequestResult result = taskAwaiter.GetResult();
					if (result == CodingRequestResult.Success)
					{
						Dictionary<string, string> dictionary = new Dictionary<string, string>();
						dictionary.Add("Protocol", tailGateParametrizeCustomizationCoding.Protocol);
						CodingLogItem.RecordToLog(tailGateParametrizeCustomizationCoding.Name, UserFriendlyValue, CodingLogItem.CodingTypes.CustomizableCodingTemplate, tailGateParametrizeCustomizationCoding.WriteModeAndAddress, BitHelpers.ByteArrayToHexString(originalData), checked_value, password, tailGateParametrizeCustomizationCoding.RequestHeader, tailGateParametrizeCustomizationCoding.ResponseHeader, tailGateParametrizeCustomizationCoding.OpenSessionCommand, tailGateParametrizeCustomizationCoding.PreWriteCommands, tailGateParametrizeCustomizationCoding.PostWriteCommands, tailGateParametrizeCustomizationCoding.ATST, dictionary, tailGateParametrizeCustomizationCoding.PreReadCommands);
					}
					codingRequestResult = result;
				}
				catch (Exception ex)
				{
					num2 = -2;
					checked_value = null;
					this.<>t__builder.SetException(ex);
					return;
				}
				IL_01B5:
				num2 = -2;
				checked_value = null;
				this.<>t__builder.SetResult(codingRequestResult);
			}

			// Token: 0x060059CC RID: 22988 RVA: 0x0042C8B4 File Offset: 0x0042AAB4
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x04003825 RID: 14373
			public int <>1__state;

			// Token: 0x04003826 RID: 14374
			public AsyncTaskMethodBuilder<CodingRequestResult> <>t__builder;

			// Token: 0x04003827 RID: 14375
			public TailGateParametrizeCustomizationCoding <>4__this;

			// Token: 0x04003828 RID: 14376
			public byte[] originalData;

			// Token: 0x04003829 RID: 14377
			public string password;

			// Token: 0x0400382A RID: 14378
			public string UserFriendlyValue;

			// Token: 0x0400382B RID: 14379
			public IProgress<string> progress;

			// Token: 0x0400382C RID: 14380
			private string <checked_value>5__2;

			// Token: 0x0400382D RID: 14381
			private TaskAwaiter<CodingRequestResult> <>u__1;
		}

		// Token: 0x02000B5D RID: 2909
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <UpdateCurrentState>d__25 : IAsyncStateMachine
		{
			// Token: 0x060059CD RID: 22989 RVA: 0x0042C8C4 File Offset: 0x0042AAC4
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				TailGateParametrizeCustomizationCoding tailGateParametrizeCustomizationCoding = this;
				CodingRequestResult codingRequestResult;
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
							goto IL_01FC;
						}
						CS$<>8__locals1 = new TailGateParametrizeCustomizationCoding.<>c__DisplayClass25_0();
						tailGateParametrizeCustomizationCoding.PartNumber = "";
						tailGateParametrizeCustomizationCoding.DSVersion = "";
						tailGateParametrizeCustomizationCoding.LongCoding = null;
						tailGateParametrizeCustomizationCoding.DisplayControls = false;
						tailGateParametrizeCustomizationCoding.CurrentState = MQBAdaptationTemplate.UNKNOWN_TITLE;
						tailGateParametrizeCustomizationCoding.BuildDefaultBeforeAndAfterCommands();
						OBDRequest obdrequest = new OBDRequest("22F187", tailGateParametrizeCustomizationCoding.RequestHeader, tailGateParametrizeCustomizationCoding.BeforeCommands, tailGateParametrizeCustomizationCoding.AfterCommands, false);
						obdrequest.ResponseDecoded += tailGateParametrizeCustomizationCoding.DeviceRequest_ResponseDecoded;
						obdrequest.ForceManualFlowControl = SharedSettings.Current.ForceUseManualFlowControlForCodingOperations;
						OBDRequest obdrequest2 = new OBDRequest("22F1B1", tailGateParametrizeCustomizationCoding.RequestHeader, tailGateParametrizeCustomizationCoding.BeforeCommands, tailGateParametrizeCustomizationCoding.AfterCommands, false);
						obdrequest2.ResponseDecoded += tailGateParametrizeCustomizationCoding.DatasetVersionsRequest_ResponseDecoded;
						obdrequest2.ForceManualFlowControl = SharedSettings.Current.ForceUseManualFlowControlForCodingOperations;
						OBDRequest obdrequest3 = new OBDRequest("220600", tailGateParametrizeCustomizationCoding.RequestHeader, tailGateParametrizeCustomizationCoding.BeforeCommands, tailGateParametrizeCustomizationCoding.AfterCommands, false);
						obdrequest3.ResponseDecoded += tailGateParametrizeCustomizationCoding.LongCodingRequest_ResponseDecoded;
						obdrequest3.ForceManualFlowControl = SharedSettings.Current.ForceUseManualFlowControlForCodingOperations;
						App.OBDReader.ReplaceQueue(new OBDRequest[] { obdrequest, obdrequest2, obdrequest3 });
						taskAwaiter = App.OBDReader.WaitForCommandQueue().GetAwaiter();
						if (!taskAwaiter.IsCompleted)
						{
							num = (num2 = 0);
							TaskAwaiter taskAwaiter2 = taskAwaiter;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, TailGateParametrizeCustomizationCoding.<UpdateCurrentState>d__25>(ref taskAwaiter, ref this);
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
					taskAwaiter = Task.Delay(1000).GetAwaiter();
					if (!taskAwaiter.IsCompleted)
					{
						num = (num2 = 1);
						TaskAwaiter taskAwaiter2 = taskAwaiter;
						this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, TailGateParametrizeCustomizationCoding.<UpdateCurrentState>d__25>(ref taskAwaiter, ref this);
						return;
					}
					IL_01FC:
					taskAwaiter.GetResult();
					if (string.IsNullOrEmpty(tailGateParametrizeCustomizationCoding.DSVersion) || string.IsNullOrEmpty(tailGateParametrizeCustomizationCoding.PartNumber) || tailGateParametrizeCustomizationCoding.LongCoding == null)
					{
						tailGateParametrizeCustomizationCoding.CurrentState = MQBAdaptationTemplate.UNKNOWN_TITLE;
						tailGateParametrizeCustomizationCoding.DisplayControls = false;
						codingRequestResult = CodingRequestResult.NoData;
					}
					else
					{
						string text = "vag.tailgate6d";
						string[] filesInDirectory = PackageFileReader.GetFilesInDirectory(text + ".");
						CS$<>8__locals1.filenamepattern = tailGateParametrizeCustomizationCoding.PartNumber + "_" + tailGateParametrizeCustomizationCoding.DSVersion;
						string text2 = filesInDirectory.FirstOrDefault((string x) => x.Contains(CS$<>8__locals1.filenamepattern, StringComparison.OrdinalIgnoreCase));
						if (text2 == null)
						{
							CS$<>8__locals1.filenamepattern = "_" + tailGateParametrizeCustomizationCoding.DSVersion + "_";
							text2 = filesInDirectory.FirstOrDefault((string x) => x.Contains(CS$<>8__locals1.filenamepattern, StringComparison.OrdinalIgnoreCase));
						}
						if (text2 == null)
						{
							tailGateParametrizeCustomizationCoding.CurrentState = MQBAdaptationTemplate.UNSUPPORTED_TITLE;
							codingRequestResult = CodingRequestResult.NotSupported;
						}
						else
						{
							Stream stream = PackageFileReader.OpenFileStream(text + "." + text2);
							try
							{
								byte[] array = new byte[stream.Length];
								stream.Read(array, 0, array.Length);
								MQB_6D_DatasetBuilderModel mqb_6D_DatasetBuilderModel = new MQB_6D_DatasetBuilderModel();
								mqb_6D_DatasetBuilderModel.LoadFromBytes(array, tailGateParametrizeCustomizationCoding.PartNumber, tailGateParametrizeCustomizationCoding.LongCoding);
								tailGateParametrizeCustomizationCoding.Model = mqb_6D_DatasetBuilderModel;
								tailGateParametrizeCustomizationCoding.CurrentState = tailGateParametrizeCustomizationCoding.PartNumber + " / " + tailGateParametrizeCustomizationCoding.DSVersion;
								tailGateParametrizeCustomizationCoding.DisplayControls = true;
								codingRequestResult = CodingRequestResult.Success;
							}
							finally
							{
								if (num < 0 && stream != null)
								{
									((IDisposable)stream).Dispose();
								}
							}
						}
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

			// Token: 0x060059CE RID: 22990 RVA: 0x0042CCB8 File Offset: 0x0042AEB8
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x0400382E RID: 14382
			public int <>1__state;

			// Token: 0x0400382F RID: 14383
			public AsyncTaskMethodBuilder<CodingRequestResult> <>t__builder;

			// Token: 0x04003830 RID: 14384
			public TailGateParametrizeCustomizationCoding <>4__this;

			// Token: 0x04003831 RID: 14385
			private TailGateParametrizeCustomizationCoding.<>c__DisplayClass25_0 <>8__1;

			// Token: 0x04003832 RID: 14386
			private TaskAwaiter <>u__1;
		}
	}
}
