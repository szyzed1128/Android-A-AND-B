using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using CarScannerXamarinForms.Common;
using CarScannerXamarinForms.OBD2;
using CarScannerXamarinForms.OBD2.PIDS;
using CarScannerXamarinForms.Settings;
using Newtonsoft.Json;

namespace CarScannerXamarinForms.Coding
{
	// Token: 0x02000883 RID: 2179
	internal class KWP2000Coding : MQBAdaptationTemplate
	{
		// Token: 0x06004A29 RID: 18985 RVA: 0x0037C6EC File Offset: 0x0037A8EC
		[JsonConstructor]
		public KWP2000Coding(int UID, string Name, string Description, string Password, string PasswordHint, IEnumerable<TranslationItem> Translations, AdaptationValueTypes ValueType, string Address, string RequestHeader, string ResponseHeader, int StartByteId, int DataLength, double Multiplier, double Offset, bool IsSigned, bool ReversedByteSet, bool RequiresPro, IEnumerable<MQBAdaptationOption> Options)
		{
			base.Name = Name;
			base.UID = UID;
			base.Description = Description;
			this.Password = Password;
			base.PasswordHint = PasswordHint;
			base.Translations = new ObservableCollection<TranslationItem>(Translations);
			base.ValueType = ValueType;
			base.Address = Address;
			base.RequestHeader = RequestHeader;
			base.ResponseHeader = ResponseHeader;
			base.StartByteId = StartByteId;
			base.DataLength = DataLength;
			base.Multiplier = Multiplier;
			base.Offset = Offset;
			base.IsSigned = IsSigned;
			base.ReversedByteSet = ReversedByteSet;
			this.RequiresPro = RequiresPro;
			base.Options = new ObservableCollection<MQBAdaptationOption>(Options);
			if (base.RequestHeader.Contains('-') || base.RequestHeader.Contains(":"))
			{
				string[] array = base.RequestHeader.Split(new char[] { '-', ':' }, StringSplitOptions.RemoveEmptyEntries);
				base.RequestHeader = array[0];
				this.ExtendedAddress = array[1];
			}
		}

		// Token: 0x170016B1 RID: 5809
		// (get) Token: 0x06004A2A RID: 18986 RVA: 0x0037C7F1 File Offset: 0x0037A9F1
		protected virtual string ReadService
		{
			get
			{
				return "21";
			}
		}

		// Token: 0x170016B2 RID: 5810
		// (get) Token: 0x06004A2B RID: 18987 RVA: 0x00002076 File Offset: 0x00000276
		// (set) Token: 0x06004A2C RID: 18988 RVA: 0x0037C7F8 File Offset: 0x0037A9F8
		public override bool PasswordVisible
		{
			get
			{
				return false;
			}
			set
			{
				base.PasswordVisible = value;
			}
		}

		// Token: 0x170016B3 RID: 5811
		// (get) Token: 0x06004A2D RID: 18989 RVA: 0x000A8D6F File Offset: 0x000A6F6F
		protected virtual CodingLogItem.CodingTypes CodingType
		{
			get
			{
				return CodingLogItem.CodingTypes.KWP2000;
			}
		}

		// Token: 0x170016B4 RID: 5812
		// (get) Token: 0x06004A2E RID: 18990 RVA: 0x0037C801 File Offset: 0x0037AA01
		protected virtual string WriteCommand
		{
			get
			{
				return "3B";
			}
		}

		// Token: 0x170016B5 RID: 5813
		// (get) Token: 0x06004A2F RID: 18991 RVA: 0x00017A6F File Offset: 0x00015C6F
		protected virtual string OpenSessionCommand
		{
			get
			{
				throw new NotImplementedException();
			}
		}

		// Token: 0x170016B6 RID: 5814
		// (get) Token: 0x06004A30 RID: 18992 RVA: 0x00017A6F File Offset: 0x00015C6F
		protected virtual string CloseSessionCommand
		{
			get
			{
				throw new NotImplementedException();
			}
		}

		// Token: 0x170016B7 RID: 5815
		// (get) Token: 0x06004A31 RID: 18993 RVA: 0x0037C808 File Offset: 0x0037AA08
		// (set) Token: 0x06004A32 RID: 18994 RVA: 0x0037C810 File Offset: 0x0037AA10
		protected virtual string ExtendedAddress
		{
			[CompilerGenerated]
			get
			{
				return this.<ExtendedAddress>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<ExtendedAddress>k__BackingField = value;
			}
		} = "";

		// Token: 0x06004A33 RID: 18995 RVA: 0x00218FB1 File Offset: 0x002171B1
		protected virtual OBDRequest[] GetAccessKeysRequests(string password, IProgress<string> progress, Action wrongPasswordCallback)
		{
			return new OBDRequest[0];
		}

		// Token: 0x06004A34 RID: 18996 RVA: 0x0037C81C File Offset: 0x0037AA1C
		protected override void BuildDefaultBeforeAndAfterCommands()
		{
			if (string.IsNullOrEmpty(this.ExtendedAddress))
			{
				this.BeforeCommands = "ATFCSH" + base.RequestHeader + ";ATFCSD300000;ATFCSM1;ATAL;ATCRA" + base.ResponseHeader;
				this.AfterCommands = "ATFCSM0;ATD;ATSP6;ATE0;ATH1;ATS0";
				return;
			}
			this.BeforeCommands = string.Concat(new string[] { "ATFCSH", base.RequestHeader, ";ATFCSD300000;ATFCSM1;ATAL;ATCRA", base.ResponseHeader, ";ATCEA", this.ExtendedAddress, ";ATTA", this.ExtendedAddress });
			this.AfterCommands = "ATFCSM0;ATD;ATSP6;ATE0;ATH1;ATS0";
		}

		// Token: 0x06004A35 RID: 18997 RVA: 0x0037C8C4 File Offset: 0x0037AAC4
		public override async Task<Tuple<byte[], CodingRequestResult>> GetCurrentStateRawData(string password)
		{
			CodingRequestResult codingResult = CodingRequestResult.UnknownError;
			SemaphoreSlim semaphore = new SemaphoreSlim(0, 1);
			string text = SharedSettings.Current.GetATST();
			if (string.IsNullOrEmpty(text))
			{
				text = "32";
			}
			this.BuildDefaultBeforeAndAfterCommands();
			OBDRequest getStateReqest = new OBDRequest(this.ReadService + base.Address, base.RequestHeader, this.BeforeCommands, this.AfterCommands, false);
			OBDRequest getStateReqestforNR78 = new OBDRequest(this.ReadService + base.Address, base.RequestHeader, "ATAT0;ATSTFF;" + this.BeforeCommands, string.Concat(new string[]
			{
				"ATAT",
				SharedSettings.Current.AdaptiveTimings.ToString(),
				";ATST",
				text,
				";",
				this.AfterCommands
			}), false);
			List<OBDRequest> list = new List<OBDRequest>();
			if (!string.IsNullOrEmpty(this.OpenSessionCommand))
			{
				OBDRequest[] array = (from x in this.OpenSessionCommand.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries)
					select new OBDRequest(x, this.RequestHeader, this.BeforeCommands, this.AfterCommands, false)
					{
						DoNotDecode = true
					}).ToArray<OBDRequest>();
				list.AddRange(array);
			}
			List<OBDRequest> closeDiagnosticSessionRequests = new List<OBDRequest>();
			if (!string.IsNullOrEmpty(this.CloseSessionCommand))
			{
				OBDRequest[] array2 = (from x in this.CloseSessionCommand.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries)
					select new OBDRequest(x, this.RequestHeader, this.BeforeCommands, this.AfterCommands, false)
					{
						DoNotDecode = true
					}).ToArray<OBDRequest>();
				if (array2.Length != 0)
				{
					array2[array2.Length - 1].ResponseReceived += delegate(OBDRequest request, string data)
					{
						semaphore.Release();
					};
				}
				closeDiagnosticSessionRequests.AddRange(array2);
			}
			getStateReqest.ResponseReceived += delegate(OBDRequest request, string data)
			{
				string text2 = "7F" + request.Command.Substring(0, 2) + "78";
				if (data.IndexOf(text2) >= 0)
				{
					data = OBDDataReader.FilterHexAndNewLineOnly(data);
					if (data.Split(new char[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries).Length == 1)
					{
						getStateReqest.DoNotDecode = true;
						List<OBDRequest> queueCopy = App.OBDReader.GetQueueCopy();
						queueCopy.Insert(0, getStateReqestforNR78);
						App.OBDReader.ReplaceQueue(queueCopy);
					}
				}
			};
			getStateReqest.ELMFormat = ELMFormat.CAN11bit;
			getStateReqest.ForceManualFlowControl = SharedSettings.Current.ForceUseManualFlowControlForCodingOperations;
			byte[] result = null;
			ResponseDecodedDelegate responseDecodedDelegate = delegate(OBDRequest request, byte[] data, bool decodeResult, string responseHeader)
			{
				if (data == null || data.Length == 0)
				{
					codingResult = CodingRequestResult.NotSupported;
				}
				else
				{
					codingResult = CodingRequestResult.Success;
				}
				result = data;
				if (closeDiagnosticSessionRequests.Count == 0)
				{
					semaphore.Release();
				}
			};
			getStateReqest.ResponseDecoded += responseDecodedDelegate;
			getStateReqestforNR78.ResponseDecoded += responseDecodedDelegate;
			List<OBDRequest> list2 = new List<OBDRequest>();
			if (list.Count > 0)
			{
				list2.AddRange(list);
			}
			list2.Add(getStateReqest);
			if (closeDiagnosticSessionRequests.Count > 0)
			{
				list2.AddRange(closeDiagnosticSessionRequests);
			}
			foreach (OBDRequest obdrequest in list2)
			{
				obdrequest.ELMFormat = ELMFormat.CAN11bit;
			}
			App.OBDReader.ReplaceQueue(list2);
			await semaphore.WaitAsync();
			if (result == null)
			{
				result = new byte[0];
			}
			if (result.Length != 0 && base.DataLength == 0)
			{
				base.DataLength = result.Length;
			}
			return new Tuple<byte[], CodingRequestResult>(result, codingResult);
		}

		// Token: 0x06004A36 RID: 18998 RVA: 0x0037C908 File Offset: 0x0037AB08
		protected override async Task<CodingRequestResult> WriteDataToECU(string password, string UserFriendlyValue, IProgress<string> progress, byte[] originalData, string checked_value)
		{
			CodingRequestResult codingResult = CodingRequestResult.UnknownError;
			this.BuildDefaultBeforeAndAfterCommands();
			SemaphoreSlim semaphore = new SemaphoreSlim(0, 1);
			List<OBDRequest> list = new List<OBDRequest>();
			if (!string.IsNullOrEmpty(this.OpenSessionCommand))
			{
				OBDRequest[] array = (from x in this.OpenSessionCommand.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries)
					select new OBDRequest(x, this.RequestHeader, this.BeforeCommands, this.AfterCommands, false)
					{
						DoNotDecode = true
					}).ToArray<OBDRequest>();
				list.AddRange(array);
			}
			List<OBDRequest> closeDiagnosticSessionRequests = new List<OBDRequest>();
			if (!string.IsNullOrEmpty(this.CloseSessionCommand))
			{
				OBDRequest[] array2 = (from x in this.CloseSessionCommand.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries)
					select new OBDRequest(x, this.RequestHeader, this.BeforeCommands, this.AfterCommands, false)
					{
						DoNotDecode = true
					}).ToArray<OBDRequest>();
				if (array2.Length != 0)
				{
					array2[array2.Length - 1].ResponseReceived += delegate(OBDRequest request, string data)
					{
						semaphore.Release();
					};
				}
				closeDiagnosticSessionRequests.AddRange(array2);
			}
			OBDRequest obdrequest = new OBDRequest(this.WriteCommand + base.Address + checked_value, base.RequestHeader, this.BeforeCommands, this.AfterCommands, false);
			obdrequest.ResponseReceived += delegate(OBDRequest request, string data)
			{
				string text = (int.Parse(this.WriteCommand, NumberStyles.HexNumber) + 64).ToString("X2");
				if (data.Contains(text + this.Address) || data.Contains("7F" + this.WriteCommand + "78"))
				{
					IProgress<string> progress2 = progress;
					if (progress2 != null)
					{
						progress2.Report(Translate.GetString("coding_progress_DataAccepted"));
					}
					codingResult = CodingRequestResult.Success;
				}
				else
				{
					IProgress<string> progress3 = progress;
					if (progress3 != null)
					{
						progress3.Report(Translate.GetString("coding_progress_DataRejected"));
					}
					if (data == null)
					{
						codingResult = CodingRequestResult.UnknownError;
					}
					else
					{
						data = OBDDataReader.FilterHexAndNewLineOnly(data);
						if (data.Length >= 11 && data.Contains("7F" + this.WriteCommand))
						{
							int num = data.IndexOf("7F" + this.WriteCommand);
							int num2 = int.Parse(data.Substring(num + 4, 2), NumberStyles.HexNumber);
							if (num2 >= 128 || num2 == 34)
							{
								codingResult = CodingRequestResult.WrongConditions;
							}
							else if (num2 == 51)
							{
								codingResult = CodingRequestResult.WrongAccessKey;
							}
							else if (num2 == 49)
							{
								codingResult = CodingRequestResult.NotSupported;
							}
							else
							{
								codingResult = CodingRequestResult.UnknownError;
							}
						}
						else
						{
							codingResult = CodingRequestResult.UnknownError;
						}
					}
				}
				if (closeDiagnosticSessionRequests.Count == 0)
				{
					semaphore.Release();
				}
			};
			List<OBDRequest> list2 = new List<OBDRequest>();
			if (list.Count > 0)
			{
				list2.AddRange(list);
			}
			OBDRequest[] accessKeysRequests = this.GetAccessKeysRequests(password, progress, delegate
			{
				if (!SharedSettings.Current.IgnoreCodingErrors)
				{
					codingResult = CodingRequestResult.WrongAccessKey;
					App.OBDReader.ReplaceQueue(new OBDRequest[0]);
					semaphore.Release();
				}
			});
			if (accessKeysRequests != null)
			{
				list2.AddRange(accessKeysRequests);
			}
			list2.Add(obdrequest);
			if (closeDiagnosticSessionRequests.Count > 0)
			{
				list2.AddRange(closeDiagnosticSessionRequests);
			}
			foreach (OBDRequest obdrequest2 in list2)
			{
				obdrequest2.ELMFormat = ELMFormat.CAN11bit;
			}
			CodingLogItem.RecordToLog(base.Name, UserFriendlyValue, this.CodingType, base.Address, BitHelpers.ByteArrayToHexString(originalData), checked_value, password, string.IsNullOrEmpty(this.ExtendedAddress) ? base.RequestHeader : (base.RequestHeader + "-" + this.ExtendedAddress), base.ResponseHeader, "", "", "", "96", null, "");
			App.OBDReader.ReplaceQueue(list2);
			await semaphore.WaitAsync();
			return codingResult;
		}

		// Token: 0x04002B0E RID: 11022
		[CompilerGenerated]
		private string <ExtendedAddress>k__BackingField;

		// Token: 0x02000884 RID: 2180
		[CompilerGenerated]
		private sealed class <>c__DisplayClass20_0
		{
			// Token: 0x06004A37 RID: 18999 RVA: 0x00002050 File Offset: 0x00000250
			public <>c__DisplayClass20_0()
			{
			}

			// Token: 0x06004A38 RID: 19000 RVA: 0x0037C975 File Offset: 0x0037AB75
			internal OBDRequest <GetCurrentStateRawData>b__2(string x)
			{
				return new OBDRequest(x, this.<>4__this.RequestHeader, this.<>4__this.BeforeCommands, this.<>4__this.AfterCommands, false)
				{
					DoNotDecode = true
				};
			}

			// Token: 0x06004A39 RID: 19001 RVA: 0x0037C975 File Offset: 0x0037AB75
			internal OBDRequest <GetCurrentStateRawData>b__3(string x)
			{
				return new OBDRequest(x, this.<>4__this.RequestHeader, this.<>4__this.BeforeCommands, this.<>4__this.AfterCommands, false)
				{
					DoNotDecode = true
				};
			}

			// Token: 0x06004A3A RID: 19002 RVA: 0x0037C9A6 File Offset: 0x0037ABA6
			internal void <GetCurrentStateRawData>b__4(OBDRequest request, string data)
			{
				this.semaphore.Release();
			}

			// Token: 0x06004A3B RID: 19003 RVA: 0x0037C9B4 File Offset: 0x0037ABB4
			internal void <GetCurrentStateRawData>b__0(OBDRequest request, string data)
			{
				string text = "7F" + request.Command.Substring(0, 2) + "78";
				if (data.IndexOf(text) >= 0)
				{
					data = OBDDataReader.FilterHexAndNewLineOnly(data);
					if (data.Split(new char[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries).Length == 1)
					{
						this.getStateReqest.DoNotDecode = true;
						List<OBDRequest> queueCopy = App.OBDReader.GetQueueCopy();
						queueCopy.Insert(0, this.getStateReqestforNR78);
						App.OBDReader.ReplaceQueue(queueCopy);
					}
				}
			}

			// Token: 0x06004A3C RID: 19004 RVA: 0x0037CA3B File Offset: 0x0037AC3B
			internal void <GetCurrentStateRawData>b__1(OBDRequest request, byte[] data, bool decodeResult, string responseHeader)
			{
				if (data == null || data.Length == 0)
				{
					this.codingResult = CodingRequestResult.NotSupported;
				}
				else
				{
					this.codingResult = CodingRequestResult.Success;
				}
				this.result = data;
				if (this.closeDiagnosticSessionRequests.Count == 0)
				{
					this.semaphore.Release();
				}
			}

			// Token: 0x04002B0F RID: 11023
			public KWP2000Coding <>4__this;

			// Token: 0x04002B10 RID: 11024
			public SemaphoreSlim semaphore;

			// Token: 0x04002B11 RID: 11025
			public OBDRequest getStateReqest;

			// Token: 0x04002B12 RID: 11026
			public OBDRequest getStateReqestforNR78;

			// Token: 0x04002B13 RID: 11027
			public CodingRequestResult codingResult;

			// Token: 0x04002B14 RID: 11028
			public byte[] result;

			// Token: 0x04002B15 RID: 11029
			public List<OBDRequest> closeDiagnosticSessionRequests;
		}

		// Token: 0x02000885 RID: 2181
		[CompilerGenerated]
		private sealed class <>c__DisplayClass21_0
		{
			// Token: 0x06004A3D RID: 19005 RVA: 0x00002050 File Offset: 0x00000250
			public <>c__DisplayClass21_0()
			{
			}

			// Token: 0x06004A3E RID: 19006 RVA: 0x0037CA75 File Offset: 0x0037AC75
			internal OBDRequest <WriteDataToECU>b__2(string x)
			{
				return new OBDRequest(x, this.<>4__this.RequestHeader, this.<>4__this.BeforeCommands, this.<>4__this.AfterCommands, false)
				{
					DoNotDecode = true
				};
			}

			// Token: 0x06004A3F RID: 19007 RVA: 0x0037CA75 File Offset: 0x0037AC75
			internal OBDRequest <WriteDataToECU>b__3(string x)
			{
				return new OBDRequest(x, this.<>4__this.RequestHeader, this.<>4__this.BeforeCommands, this.<>4__this.AfterCommands, false)
				{
					DoNotDecode = true
				};
			}

			// Token: 0x06004A40 RID: 19008 RVA: 0x0037CAA6 File Offset: 0x0037ACA6
			internal void <WriteDataToECU>b__4(OBDRequest request, string data)
			{
				this.semaphore.Release();
			}

			// Token: 0x06004A41 RID: 19009 RVA: 0x0037CAB4 File Offset: 0x0037ACB4
			internal void <WriteDataToECU>b__0(OBDRequest request, string data)
			{
				string text = (int.Parse(this.<>4__this.WriteCommand, NumberStyles.HexNumber) + 64).ToString("X2");
				if (data.Contains(text + this.<>4__this.Address) || data.Contains("7F" + this.<>4__this.WriteCommand + "78"))
				{
					IProgress<string> progress = this.progress;
					if (progress != null)
					{
						progress.Report(Translate.GetString("coding_progress_DataAccepted"));
					}
					this.codingResult = CodingRequestResult.Success;
				}
				else
				{
					IProgress<string> progress2 = this.progress;
					if (progress2 != null)
					{
						progress2.Report(Translate.GetString("coding_progress_DataRejected"));
					}
					if (data == null)
					{
						this.codingResult = CodingRequestResult.UnknownError;
					}
					else
					{
						data = OBDDataReader.FilterHexAndNewLineOnly(data);
						if (data.Length >= 11 && data.Contains("7F" + this.<>4__this.WriteCommand))
						{
							int num = data.IndexOf("7F" + this.<>4__this.WriteCommand);
							int num2 = int.Parse(data.Substring(num + 4, 2), NumberStyles.HexNumber);
							if (num2 >= 128 || num2 == 34)
							{
								this.codingResult = CodingRequestResult.WrongConditions;
							}
							else if (num2 == 51)
							{
								this.codingResult = CodingRequestResult.WrongAccessKey;
							}
							else if (num2 == 49)
							{
								this.codingResult = CodingRequestResult.NotSupported;
							}
							else
							{
								this.codingResult = CodingRequestResult.UnknownError;
							}
						}
						else
						{
							this.codingResult = CodingRequestResult.UnknownError;
						}
					}
				}
				if (this.closeDiagnosticSessionRequests.Count == 0)
				{
					this.semaphore.Release();
				}
			}

			// Token: 0x06004A42 RID: 19010 RVA: 0x0037CC32 File Offset: 0x0037AE32
			internal void <WriteDataToECU>b__1()
			{
				if (!SharedSettings.Current.IgnoreCodingErrors)
				{
					this.codingResult = CodingRequestResult.WrongAccessKey;
					App.OBDReader.ReplaceQueue(new OBDRequest[0]);
					this.semaphore.Release();
				}
			}

			// Token: 0x04002B16 RID: 11030
			public KWP2000Coding <>4__this;

			// Token: 0x04002B17 RID: 11031
			public SemaphoreSlim semaphore;

			// Token: 0x04002B18 RID: 11032
			public IProgress<string> progress;

			// Token: 0x04002B19 RID: 11033
			public CodingRequestResult codingResult;

			// Token: 0x04002B1A RID: 11034
			public List<OBDRequest> closeDiagnosticSessionRequests;
		}

		// Token: 0x02000886 RID: 2182
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <GetCurrentStateRawData>d__20 : IAsyncStateMachine
		{
			// Token: 0x06004A43 RID: 19011 RVA: 0x0037CC64 File Offset: 0x0037AE64
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				KWP2000Coding kwp2000Coding = this;
				Tuple<byte[], CodingRequestResult> tuple;
				try
				{
					TaskAwaiter taskAwaiter;
					if (num != 0)
					{
						CS$<>8__locals1 = new KWP2000Coding.<>c__DisplayClass20_0();
						CS$<>8__locals1.<>4__this = this;
						CS$<>8__locals1.codingResult = CodingRequestResult.UnknownError;
						CS$<>8__locals1.semaphore = new SemaphoreSlim(0, 1);
						string text = SharedSettings.Current.GetATST();
						if (string.IsNullOrEmpty(text))
						{
							text = "32";
						}
						kwp2000Coding.BuildDefaultBeforeAndAfterCommands();
						CS$<>8__locals1.getStateReqest = new OBDRequest(kwp2000Coding.ReadService + kwp2000Coding.Address, kwp2000Coding.RequestHeader, kwp2000Coding.BeforeCommands, kwp2000Coding.AfterCommands, false);
						CS$<>8__locals1.getStateReqestforNR78 = new OBDRequest(kwp2000Coding.ReadService + kwp2000Coding.Address, kwp2000Coding.RequestHeader, "ATAT0;ATSTFF;" + kwp2000Coding.BeforeCommands, string.Concat(new string[]
						{
							"ATAT",
							SharedSettings.Current.AdaptiveTimings.ToString(),
							";ATST",
							text,
							";",
							kwp2000Coding.AfterCommands
						}), false);
						List<OBDRequest> list = new List<OBDRequest>();
						if (!string.IsNullOrEmpty(kwp2000Coding.OpenSessionCommand))
						{
							OBDRequest[] array = (from x in kwp2000Coding.OpenSessionCommand.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries)
								select new OBDRequest(x, CS$<>8__locals1.<>4__this.RequestHeader, CS$<>8__locals1.<>4__this.BeforeCommands, CS$<>8__locals1.<>4__this.AfterCommands, false)
								{
									DoNotDecode = true
								}).ToArray<OBDRequest>();
							list.AddRange(array);
						}
						CS$<>8__locals1.closeDiagnosticSessionRequests = new List<OBDRequest>();
						if (!string.IsNullOrEmpty(kwp2000Coding.CloseSessionCommand))
						{
							OBDRequest[] array2 = (from x in kwp2000Coding.CloseSessionCommand.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries)
								select new OBDRequest(x, CS$<>8__locals1.<>4__this.RequestHeader, CS$<>8__locals1.<>4__this.BeforeCommands, CS$<>8__locals1.<>4__this.AfterCommands, false)
								{
									DoNotDecode = true
								}).ToArray<OBDRequest>();
							if (array2.Length != 0)
							{
								array2[array2.Length - 1].ResponseReceived += delegate(OBDRequest request, string data)
								{
									CS$<>8__locals1.semaphore.Release();
								};
							}
							CS$<>8__locals1.closeDiagnosticSessionRequests.AddRange(array2);
						}
						CS$<>8__locals1.getStateReqest.ResponseReceived += delegate(OBDRequest request, string data)
						{
							string text2 = "7F" + request.Command.Substring(0, 2) + "78";
							if (data.IndexOf(text2) >= 0)
							{
								data = OBDDataReader.FilterHexAndNewLineOnly(data);
								if (data.Split(new char[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries).Length == 1)
								{
									CS$<>8__locals1.getStateReqest.DoNotDecode = true;
									List<OBDRequest> queueCopy = App.OBDReader.GetQueueCopy();
									queueCopy.Insert(0, CS$<>8__locals1.getStateReqestforNR78);
									App.OBDReader.ReplaceQueue(queueCopy);
								}
							}
						};
						CS$<>8__locals1.getStateReqest.ELMFormat = ELMFormat.CAN11bit;
						CS$<>8__locals1.getStateReqest.ForceManualFlowControl = SharedSettings.Current.ForceUseManualFlowControlForCodingOperations;
						CS$<>8__locals1.result = null;
						ResponseDecodedDelegate responseDecodedDelegate = delegate(OBDRequest request, byte[] data, bool decodeResult, string responseHeader)
						{
							if (data == null || data.Length == 0)
							{
								CS$<>8__locals1.codingResult = CodingRequestResult.NotSupported;
							}
							else
							{
								CS$<>8__locals1.codingResult = CodingRequestResult.Success;
							}
							CS$<>8__locals1.result = data;
							if (CS$<>8__locals1.closeDiagnosticSessionRequests.Count == 0)
							{
								CS$<>8__locals1.semaphore.Release();
							}
						};
						CS$<>8__locals1.getStateReqest.ResponseDecoded += responseDecodedDelegate;
						CS$<>8__locals1.getStateReqestforNR78.ResponseDecoded += responseDecodedDelegate;
						List<OBDRequest> list2 = new List<OBDRequest>();
						if (list.Count > 0)
						{
							list2.AddRange(list);
						}
						list2.Add(CS$<>8__locals1.getStateReqest);
						if (CS$<>8__locals1.closeDiagnosticSessionRequests.Count > 0)
						{
							list2.AddRange(CS$<>8__locals1.closeDiagnosticSessionRequests);
						}
						List<OBDRequest>.Enumerator enumerator = list2.GetEnumerator();
						try
						{
							while (enumerator.MoveNext())
							{
								OBDRequest obdrequest = enumerator.Current;
								obdrequest.ELMFormat = ELMFormat.CAN11bit;
							}
						}
						finally
						{
							if (num < 0)
							{
								((IDisposable)enumerator).Dispose();
							}
						}
						App.OBDReader.ReplaceQueue(list2);
						taskAwaiter = CS$<>8__locals1.semaphore.WaitAsync().GetAwaiter();
						if (!taskAwaiter.IsCompleted)
						{
							num = (num2 = 0);
							TaskAwaiter taskAwaiter2 = taskAwaiter;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, KWP2000Coding.<GetCurrentStateRawData>d__20>(ref taskAwaiter, ref this);
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
					if (CS$<>8__locals1.result == null)
					{
						CS$<>8__locals1.result = new byte[0];
					}
					if (CS$<>8__locals1.result.Length != 0 && kwp2000Coding.DataLength == 0)
					{
						kwp2000Coding.DataLength = CS$<>8__locals1.result.Length;
					}
					tuple = new Tuple<byte[], CodingRequestResult>(CS$<>8__locals1.result, CS$<>8__locals1.codingResult);
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
				this.<>t__builder.SetResult(tuple);
			}

			// Token: 0x06004A44 RID: 19012 RVA: 0x0037D0C8 File Offset: 0x0037B2C8
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x04002B1B RID: 11035
			public int <>1__state;

			// Token: 0x04002B1C RID: 11036
			public AsyncTaskMethodBuilder<Tuple<byte[], CodingRequestResult>> <>t__builder;

			// Token: 0x04002B1D RID: 11037
			public KWP2000Coding <>4__this;

			// Token: 0x04002B1E RID: 11038
			private KWP2000Coding.<>c__DisplayClass20_0 <>8__1;

			// Token: 0x04002B1F RID: 11039
			private TaskAwaiter <>u__1;
		}

		// Token: 0x02000887 RID: 2183
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <WriteDataToECU>d__21 : IAsyncStateMachine
		{
			// Token: 0x06004A45 RID: 19013 RVA: 0x0037D0D8 File Offset: 0x0037B2D8
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				KWP2000Coding kwp2000Coding = this;
				CodingRequestResult codingResult;
				try
				{
					TaskAwaiter taskAwaiter;
					if (num != 0)
					{
						CS$<>8__locals1 = new KWP2000Coding.<>c__DisplayClass21_0();
						CS$<>8__locals1.<>4__this = this;
						CS$<>8__locals1.progress = progress;
						CS$<>8__locals1.codingResult = CodingRequestResult.UnknownError;
						kwp2000Coding.BuildDefaultBeforeAndAfterCommands();
						CS$<>8__locals1.semaphore = new SemaphoreSlim(0, 1);
						List<OBDRequest> list = new List<OBDRequest>();
						if (!string.IsNullOrEmpty(kwp2000Coding.OpenSessionCommand))
						{
							OBDRequest[] array = (from x in kwp2000Coding.OpenSessionCommand.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries)
								select new OBDRequest(x, CS$<>8__locals1.<>4__this.RequestHeader, CS$<>8__locals1.<>4__this.BeforeCommands, CS$<>8__locals1.<>4__this.AfterCommands, false)
								{
									DoNotDecode = true
								}).ToArray<OBDRequest>();
							list.AddRange(array);
						}
						CS$<>8__locals1.closeDiagnosticSessionRequests = new List<OBDRequest>();
						if (!string.IsNullOrEmpty(kwp2000Coding.CloseSessionCommand))
						{
							OBDRequest[] array2 = (from x in kwp2000Coding.CloseSessionCommand.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries)
								select new OBDRequest(x, CS$<>8__locals1.<>4__this.RequestHeader, CS$<>8__locals1.<>4__this.BeforeCommands, CS$<>8__locals1.<>4__this.AfterCommands, false)
								{
									DoNotDecode = true
								}).ToArray<OBDRequest>();
							if (array2.Length != 0)
							{
								array2[array2.Length - 1].ResponseReceived += delegate(OBDRequest request, string data)
								{
									CS$<>8__locals1.semaphore.Release();
								};
							}
							CS$<>8__locals1.closeDiagnosticSessionRequests.AddRange(array2);
						}
						OBDRequest obdrequest = new OBDRequest(kwp2000Coding.WriteCommand + kwp2000Coding.Address + checked_value, kwp2000Coding.RequestHeader, kwp2000Coding.BeforeCommands, kwp2000Coding.AfterCommands, false);
						obdrequest.ResponseReceived += delegate(OBDRequest request, string data)
						{
							string text = (int.Parse(CS$<>8__locals1.<>4__this.WriteCommand, NumberStyles.HexNumber) + 64).ToString("X2");
							if (data.Contains(text + CS$<>8__locals1.<>4__this.Address) || data.Contains("7F" + CS$<>8__locals1.<>4__this.WriteCommand + "78"))
							{
								IProgress<string> progress = CS$<>8__locals1.progress;
								if (progress != null)
								{
									progress.Report(Translate.GetString("coding_progress_DataAccepted"));
								}
								CS$<>8__locals1.codingResult = CodingRequestResult.Success;
							}
							else
							{
								IProgress<string> progress2 = CS$<>8__locals1.progress;
								if (progress2 != null)
								{
									progress2.Report(Translate.GetString("coding_progress_DataRejected"));
								}
								if (data == null)
								{
									CS$<>8__locals1.codingResult = CodingRequestResult.UnknownError;
								}
								else
								{
									data = OBDDataReader.FilterHexAndNewLineOnly(data);
									if (data.Length >= 11 && data.Contains("7F" + CS$<>8__locals1.<>4__this.WriteCommand))
									{
										int num3 = data.IndexOf("7F" + CS$<>8__locals1.<>4__this.WriteCommand);
										int num4 = int.Parse(data.Substring(num3 + 4, 2), NumberStyles.HexNumber);
										if (num4 >= 128 || num4 == 34)
										{
											CS$<>8__locals1.codingResult = CodingRequestResult.WrongConditions;
										}
										else if (num4 == 51)
										{
											CS$<>8__locals1.codingResult = CodingRequestResult.WrongAccessKey;
										}
										else if (num4 == 49)
										{
											CS$<>8__locals1.codingResult = CodingRequestResult.NotSupported;
										}
										else
										{
											CS$<>8__locals1.codingResult = CodingRequestResult.UnknownError;
										}
									}
									else
									{
										CS$<>8__locals1.codingResult = CodingRequestResult.UnknownError;
									}
								}
							}
							if (CS$<>8__locals1.closeDiagnosticSessionRequests.Count == 0)
							{
								CS$<>8__locals1.semaphore.Release();
							}
						};
						List<OBDRequest> list2 = new List<OBDRequest>();
						if (list.Count > 0)
						{
							list2.AddRange(list);
						}
						OBDRequest[] accessKeysRequests = kwp2000Coding.GetAccessKeysRequests(password, CS$<>8__locals1.progress, delegate
						{
							if (!SharedSettings.Current.IgnoreCodingErrors)
							{
								CS$<>8__locals1.codingResult = CodingRequestResult.WrongAccessKey;
								App.OBDReader.ReplaceQueue(new OBDRequest[0]);
								CS$<>8__locals1.semaphore.Release();
							}
						});
						if (accessKeysRequests != null)
						{
							list2.AddRange(accessKeysRequests);
						}
						list2.Add(obdrequest);
						if (CS$<>8__locals1.closeDiagnosticSessionRequests.Count > 0)
						{
							list2.AddRange(CS$<>8__locals1.closeDiagnosticSessionRequests);
						}
						List<OBDRequest>.Enumerator enumerator = list2.GetEnumerator();
						try
						{
							while (enumerator.MoveNext())
							{
								OBDRequest obdrequest2 = enumerator.Current;
								obdrequest2.ELMFormat = ELMFormat.CAN11bit;
							}
						}
						finally
						{
							if (num < 0)
							{
								((IDisposable)enumerator).Dispose();
							}
						}
						CodingLogItem.RecordToLog(kwp2000Coding.Name, UserFriendlyValue, kwp2000Coding.CodingType, kwp2000Coding.Address, BitHelpers.ByteArrayToHexString(originalData), checked_value, password, string.IsNullOrEmpty(kwp2000Coding.ExtendedAddress) ? kwp2000Coding.RequestHeader : (kwp2000Coding.RequestHeader + "-" + kwp2000Coding.ExtendedAddress), kwp2000Coding.ResponseHeader, "", "", "", "96", null, "");
						App.OBDReader.ReplaceQueue(list2);
						taskAwaiter = CS$<>8__locals1.semaphore.WaitAsync().GetAwaiter();
						if (!taskAwaiter.IsCompleted)
						{
							num = (num2 = 0);
							TaskAwaiter taskAwaiter2 = taskAwaiter;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, KWP2000Coding.<WriteDataToECU>d__21>(ref taskAwaiter, ref this);
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
					codingResult = CS$<>8__locals1.codingResult;
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
				this.<>t__builder.SetResult(codingResult);
			}

			// Token: 0x06004A46 RID: 19014 RVA: 0x0037D488 File Offset: 0x0037B688
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x04002B20 RID: 11040
			public int <>1__state;

			// Token: 0x04002B21 RID: 11041
			public AsyncTaskMethodBuilder<CodingRequestResult> <>t__builder;

			// Token: 0x04002B22 RID: 11042
			public KWP2000Coding <>4__this;

			// Token: 0x04002B23 RID: 11043
			public IProgress<string> progress;

			// Token: 0x04002B24 RID: 11044
			public string checked_value;

			// Token: 0x04002B25 RID: 11045
			public string password;

			// Token: 0x04002B26 RID: 11046
			public string UserFriendlyValue;

			// Token: 0x04002B27 RID: 11047
			public byte[] originalData;

			// Token: 0x04002B28 RID: 11048
			private KWP2000Coding.<>c__DisplayClass21_0 <>8__1;

			// Token: 0x04002B29 RID: 11049
			private TaskAwaiter <>u__1;
		}
	}
}
