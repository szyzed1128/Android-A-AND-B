using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using CarScannerXamarinForms.Common;
using CarScannerXamarinForms.OBD2;
using CarScannerXamarinForms.Settings;

namespace CarScannerXamarinForms.Coding
{
	// Token: 0x020008A4 RID: 2212
	internal class MQBCustomizableMode22Coding : MQBAdaptationTemplate
	{
		// Token: 0x170016F7 RID: 5879
		// (get) Token: 0x06004B41 RID: 19265 RVA: 0x00382A86 File Offset: 0x00380C86
		// (set) Token: 0x06004B42 RID: 19266 RVA: 0x00382A8E File Offset: 0x00380C8E
		public string PreReadCommands
		{
			get
			{
				return this._PreReadCommands;
			}
			set
			{
				this._PreReadCommands = value;
				base.OnPropertyChanged("_PreReadCommands");
			}
		}

		// Token: 0x170016F8 RID: 5880
		// (get) Token: 0x06004B43 RID: 19267 RVA: 0x00382AA2 File Offset: 0x00380CA2
		// (set) Token: 0x06004B44 RID: 19268 RVA: 0x00382AAA File Offset: 0x00380CAA
		public string ReadMode
		{
			[CompilerGenerated]
			get
			{
				return this.<ReadMode>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<ReadMode>k__BackingField = value;
			}
		} = "22";

		// Token: 0x170016F9 RID: 5881
		// (get) Token: 0x06004B45 RID: 19269 RVA: 0x00382AB3 File Offset: 0x00380CB3
		// (set) Token: 0x06004B46 RID: 19270 RVA: 0x00382ABB File Offset: 0x00380CBB
		public string WriteMode
		{
			[CompilerGenerated]
			get
			{
				return this.<WriteMode>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<WriteMode>k__BackingField = value;
			}
		} = "2E";

		// Token: 0x170016FA RID: 5882
		// (get) Token: 0x06004B47 RID: 19271 RVA: 0x00382AC4 File Offset: 0x00380CC4
		// (set) Token: 0x06004B48 RID: 19272 RVA: 0x00382ACC File Offset: 0x00380CCC
		public string ATST
		{
			get
			{
				return this._ATST;
			}
			set
			{
				this._ATST = value;
				base.OnPropertyChanged("ATST");
			}
		}

		// Token: 0x06004B49 RID: 19273 RVA: 0x00382AE0 File Offset: 0x00380CE0
		protected override void BuildDefaultBeforeAndAfterCommands()
		{
			this.BeforeCommands = string.Concat(new string[] { "ATFCSH", base.RequestHeader, ";ATFCSD300000;ATFCSM1;ATAL;ATCRA", base.ResponseHeader, ";ATST", this.ATST });
			this.AfterCommands = "ATFCSM0;ATD;ATSP6;ATE0;ATH1;ATS0;ATSTDEF";
		}

		// Token: 0x06004B4A RID: 19274 RVA: 0x00382B3C File Offset: 0x00380D3C
		public override async Task<Tuple<byte[], CodingRequestResult>> GetCurrentStateRawData(string password)
		{
			this.BuildDefaultBeforeAndAfterCommands();
			CodingRequestResult requestResult = CodingRequestResult.UnknownError;
			List<OBDRequest> list = new List<OBDRequest>();
			list.AddRange(MQBParametrizeBase.GetRequestsFromStringCommands(this.PreReadCommands, password, base.RequestHeader, this.BeforeCommands, this.AfterCommands));
			string text = this.ReadMode + base.Address;
			string readServicePositiveResponse = (int.Parse(this.ReadMode, NumberStyles.HexNumber) + 64).ToString("X2");
			byte[] resultBytes = new byte[0];
			OBDRequest obdrequest = new OBDRequest(text, base.RequestHeader, this.BeforeCommands, this.AfterCommands, false)
			{
				CheckLength = true,
				ELMFormat = ELMFormat.CAN11bit,
				ForceManualFlowControl = SharedSettings.Current.ForceUseManualFlowControlForCodingOperations
			};
			obdrequest.ResponseReceived += delegate(OBDRequest readRequest2, string lastRequestData)
			{
				if (lastRequestData != null)
				{
					string text2 = OBDDataReader.FilterHexAndNewLineOnly(lastRequestData);
					if (!text2.Contains("7F" + this.ReadMode + "78") && !text2.Contains(readServicePositiveResponse))
					{
						if (text2.Length >= 11 && text2.Contains("7F" + this.ReadMode))
						{
							int num = text2.IndexOf("7F" + this.ReadMode);
							int num2 = int.Parse(text2.Substring(num + 4, 2), NumberStyles.HexNumber);
							if (num2 >= 128 || num2 == 34)
							{
								requestResult = CodingRequestResult.WrongConditions;
							}
							else if (num2 == 51)
							{
								requestResult = CodingRequestResult.WrongAccessKey;
							}
							else if (num2 == 49)
							{
								requestResult = CodingRequestResult.NotSupported;
							}
							else
							{
								requestResult = CodingRequestResult.UnknownError;
							}
						}
						else
						{
							requestResult = CodingRequestResult.UnknownError;
						}
						App.OBDReader.ReplaceQueue(new OBDRequest[0]);
						return;
					}
				}
				else
				{
					requestResult = CodingRequestResult.NoData;
				}
			};
			obdrequest.ResponseDecoded += delegate(OBDRequest getDataRequest2, byte[] getDataRequestData, bool decodeResult, string responseHeader)
			{
				if (getDataRequestData != null && getDataRequestData.Length != 0)
				{
					resultBytes = getDataRequestData;
					requestResult = CodingRequestResult.Success;
				}
			};
			list.Add(obdrequest);
			App.OBDReader.ReplaceQueue(list);
			await App.OBDReader.WaitForCommandQueue();
			return new Tuple<byte[], CodingRequestResult>(resultBytes, requestResult);
		}

		// Token: 0x06004B4B RID: 19275 RVA: 0x00382B88 File Offset: 0x00380D88
		protected override async Task<CodingRequestResult> WriteDataToECU(string password, string UserFriendlyValue, IProgress<string> progress, byte[] originalData, string checked_value)
		{
			this.BuildDefaultBeforeAndAfterCommands();
			string writeServicePositiveResponse = (int.Parse(this.WriteMode, NumberStyles.HexNumber) + 64).ToString("X2");
			CodingRequestResult requestResult = CodingRequestResult.UnknownError;
			List<OBDRequest> list = new List<OBDRequest>();
			list.AddRange(MQBParametrizeBase.GetRequestsFromStringCommands(base.PreWriteCommands, password, base.RequestHeader, this.BeforeCommands, this.AfterCommands));
			OBDRequest obdrequest = new OBDRequest(this.WriteMode + base.Address + checked_value, base.RequestHeader, this.BeforeCommands, this.AfterCommands, false);
			obdrequest.ResponseReceived += delegate(OBDRequest writeRequest2, string writeRequestData)
			{
				if (writeRequestData != null)
				{
					string text = OBDDataReader.FilterHexAndNewLineOnly(writeRequestData);
					if (text.Contains("7F" + this.WriteMode + "78") || text.Contains(writeServicePositiveResponse))
					{
						IProgress<string> progress2 = progress;
						if (progress2 != null)
						{
							progress2.Report(Translate.GetString("coding_progress_DataAccepted"));
						}
						requestResult = CodingRequestResult.Success;
						return;
					}
					if (text.Length >= 11 && text.Contains("7F" + this.WriteMode))
					{
						int num = text.IndexOf("7F" + this.WriteMode);
						int num2 = int.Parse(text.Substring(num + 4, 2), NumberStyles.HexNumber);
						if (num2 >= 128 || num2 == 34)
						{
							requestResult = CodingRequestResult.WrongConditions;
						}
						else if (num2 == 51)
						{
							requestResult = CodingRequestResult.WrongAccessKey;
						}
						else if (num2 == 49)
						{
							requestResult = CodingRequestResult.NotSupported;
						}
						else
						{
							requestResult = CodingRequestResult.UnknownError;
						}
					}
					else
					{
						requestResult = CodingRequestResult.UnknownError;
					}
					App.OBDReader.ReplaceQueue(new OBDRequest[0]);
				}
			};
			obdrequest.ELMFormat = ELMFormat.CAN11bit;
			obdrequest.Keys.Add("TesterPresent", "ATSH700;023E80;ATSH" + base.RequestHeader);
			list.Add(obdrequest);
			list.AddRange(MQBParametrizeBase.GetRequestsFromStringCommands(base.PostWriteCommands, password, base.RequestHeader, this.BeforeCommands, this.AfterCommands));
			App.OBDReader.ReplaceQueue(list);
			await App.OBDReader.WaitForCommandQueue();
			if (requestResult == CodingRequestResult.Success && originalData != null)
			{
				Dictionary<string, string> dictionary = new Dictionary<string, string>();
				CodingLogItem.RecordToLog(base.Name, UserFriendlyValue, CodingLogItem.CodingTypes.MQBCustomizableMode22Coding, base.Address, BitHelpers.ByteArrayToHexString(originalData), checked_value, password, base.RequestHeader, base.ResponseHeader, this.PreReadCommands, base.PreWriteCommands, base.PostWriteCommands, this.ATST, dictionary, "");
			}
			return requestResult;
		}

		// Token: 0x06004B4C RID: 19276 RVA: 0x00382BF5 File Offset: 0x00380DF5
		public MQBCustomizableMode22Coding()
		{
		}

		// Token: 0x04002BEE RID: 11246
		private string _PreReadCommands = "1003";

		// Token: 0x04002BEF RID: 11247
		[CompilerGenerated]
		private string <ReadMode>k__BackingField;

		// Token: 0x04002BF0 RID: 11248
		[CompilerGenerated]
		private string <WriteMode>k__BackingField;

		// Token: 0x04002BF1 RID: 11249
		private string _ATST = "64";

		// Token: 0x020008A5 RID: 2213
		[CompilerGenerated]
		private sealed class <>c__DisplayClass17_0
		{
			// Token: 0x06004B4D RID: 19277 RVA: 0x00002050 File Offset: 0x00000250
			public <>c__DisplayClass17_0()
			{
			}

			// Token: 0x06004B4E RID: 19278 RVA: 0x00382C2C File Offset: 0x00380E2C
			internal void <GetCurrentStateRawData>b__0(OBDRequest readRequest2, string lastRequestData)
			{
				if (lastRequestData != null)
				{
					string text = OBDDataReader.FilterHexAndNewLineOnly(lastRequestData);
					if (!text.Contains("7F" + this.<>4__this.ReadMode + "78") && !text.Contains(this.readServicePositiveResponse))
					{
						if (text.Length >= 11 && text.Contains("7F" + this.<>4__this.ReadMode))
						{
							int num = text.IndexOf("7F" + this.<>4__this.ReadMode);
							int num2 = int.Parse(text.Substring(num + 4, 2), NumberStyles.HexNumber);
							if (num2 >= 128 || num2 == 34)
							{
								this.requestResult = CodingRequestResult.WrongConditions;
							}
							else if (num2 == 51)
							{
								this.requestResult = CodingRequestResult.WrongAccessKey;
							}
							else if (num2 == 49)
							{
								this.requestResult = CodingRequestResult.NotSupported;
							}
							else
							{
								this.requestResult = CodingRequestResult.UnknownError;
							}
						}
						else
						{
							this.requestResult = CodingRequestResult.UnknownError;
						}
						App.OBDReader.ReplaceQueue(new OBDRequest[0]);
						return;
					}
				}
				else
				{
					this.requestResult = CodingRequestResult.NoData;
				}
			}

			// Token: 0x06004B4F RID: 19279 RVA: 0x00382D33 File Offset: 0x00380F33
			internal void <GetCurrentStateRawData>b__1(OBDRequest getDataRequest2, byte[] getDataRequestData, bool decodeResult, string responseHeader)
			{
				if (getDataRequestData != null && getDataRequestData.Length != 0)
				{
					this.resultBytes = getDataRequestData;
					this.requestResult = CodingRequestResult.Success;
				}
			}

			// Token: 0x04002BF2 RID: 11250
			public MQBCustomizableMode22Coding <>4__this;

			// Token: 0x04002BF3 RID: 11251
			public string readServicePositiveResponse;

			// Token: 0x04002BF4 RID: 11252
			public CodingRequestResult requestResult;

			// Token: 0x04002BF5 RID: 11253
			public byte[] resultBytes;
		}

		// Token: 0x020008A6 RID: 2214
		[CompilerGenerated]
		private sealed class <>c__DisplayClass18_0
		{
			// Token: 0x06004B50 RID: 19280 RVA: 0x00002050 File Offset: 0x00000250
			public <>c__DisplayClass18_0()
			{
			}

			// Token: 0x06004B51 RID: 19281 RVA: 0x00382D4C File Offset: 0x00380F4C
			internal void <WriteDataToECU>b__0(OBDRequest writeRequest2, string writeRequestData)
			{
				if (writeRequestData != null)
				{
					string text = OBDDataReader.FilterHexAndNewLineOnly(writeRequestData);
					if (text.Contains("7F" + this.<>4__this.WriteMode + "78") || text.Contains(this.writeServicePositiveResponse))
					{
						IProgress<string> progress = this.progress;
						if (progress != null)
						{
							progress.Report(Translate.GetString("coding_progress_DataAccepted"));
						}
						this.requestResult = CodingRequestResult.Success;
						return;
					}
					if (text.Length >= 11 && text.Contains("7F" + this.<>4__this.WriteMode))
					{
						int num = text.IndexOf("7F" + this.<>4__this.WriteMode);
						int num2 = int.Parse(text.Substring(num + 4, 2), NumberStyles.HexNumber);
						if (num2 >= 128 || num2 == 34)
						{
							this.requestResult = CodingRequestResult.WrongConditions;
						}
						else if (num2 == 51)
						{
							this.requestResult = CodingRequestResult.WrongAccessKey;
						}
						else if (num2 == 49)
						{
							this.requestResult = CodingRequestResult.NotSupported;
						}
						else
						{
							this.requestResult = CodingRequestResult.UnknownError;
						}
					}
					else
					{
						this.requestResult = CodingRequestResult.UnknownError;
					}
					App.OBDReader.ReplaceQueue(new OBDRequest[0]);
				}
			}

			// Token: 0x04002BF6 RID: 11254
			public MQBCustomizableMode22Coding <>4__this;

			// Token: 0x04002BF7 RID: 11255
			public string writeServicePositiveResponse;

			// Token: 0x04002BF8 RID: 11256
			public IProgress<string> progress;

			// Token: 0x04002BF9 RID: 11257
			public CodingRequestResult requestResult;
		}

		// Token: 0x020008A7 RID: 2215
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <GetCurrentStateRawData>d__17 : IAsyncStateMachine
		{
			// Token: 0x06004B52 RID: 19282 RVA: 0x00382E68 File Offset: 0x00381068
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				MQBCustomizableMode22Coding mqbcustomizableMode22Coding = this;
				Tuple<byte[], CodingRequestResult> tuple;
				try
				{
					TaskAwaiter taskAwaiter;
					if (num != 0)
					{
						CS$<>8__locals1 = new MQBCustomizableMode22Coding.<>c__DisplayClass17_0();
						CS$<>8__locals1.<>4__this = this;
						mqbcustomizableMode22Coding.BuildDefaultBeforeAndAfterCommands();
						CS$<>8__locals1.requestResult = CodingRequestResult.UnknownError;
						List<OBDRequest> list = new List<OBDRequest>();
						list.AddRange(MQBParametrizeBase.GetRequestsFromStringCommands(mqbcustomizableMode22Coding.PreReadCommands, password, mqbcustomizableMode22Coding.RequestHeader, mqbcustomizableMode22Coding.BeforeCommands, mqbcustomizableMode22Coding.AfterCommands));
						string text = mqbcustomizableMode22Coding.ReadMode + mqbcustomizableMode22Coding.Address;
						CS$<>8__locals1.readServicePositiveResponse = (int.Parse(mqbcustomizableMode22Coding.ReadMode, NumberStyles.HexNumber) + 64).ToString("X2");
						CS$<>8__locals1.resultBytes = new byte[0];
						OBDRequest obdrequest = new OBDRequest(text, mqbcustomizableMode22Coding.RequestHeader, mqbcustomizableMode22Coding.BeforeCommands, mqbcustomizableMode22Coding.AfterCommands, false)
						{
							CheckLength = true,
							ELMFormat = ELMFormat.CAN11bit,
							ForceManualFlowControl = SharedSettings.Current.ForceUseManualFlowControlForCodingOperations
						};
						obdrequest.ResponseReceived += delegate(OBDRequest readRequest2, string lastRequestData)
						{
							if (lastRequestData != null)
							{
								string text2 = OBDDataReader.FilterHexAndNewLineOnly(lastRequestData);
								if (!text2.Contains("7F" + CS$<>8__locals1.<>4__this.ReadMode + "78") && !text2.Contains(CS$<>8__locals1.readServicePositiveResponse))
								{
									if (text2.Length >= 11 && text2.Contains("7F" + CS$<>8__locals1.<>4__this.ReadMode))
									{
										int num3 = text2.IndexOf("7F" + CS$<>8__locals1.<>4__this.ReadMode);
										int num4 = int.Parse(text2.Substring(num3 + 4, 2), NumberStyles.HexNumber);
										if (num4 >= 128 || num4 == 34)
										{
											CS$<>8__locals1.requestResult = CodingRequestResult.WrongConditions;
										}
										else if (num4 == 51)
										{
											CS$<>8__locals1.requestResult = CodingRequestResult.WrongAccessKey;
										}
										else if (num4 == 49)
										{
											CS$<>8__locals1.requestResult = CodingRequestResult.NotSupported;
										}
										else
										{
											CS$<>8__locals1.requestResult = CodingRequestResult.UnknownError;
										}
									}
									else
									{
										CS$<>8__locals1.requestResult = CodingRequestResult.UnknownError;
									}
									App.OBDReader.ReplaceQueue(new OBDRequest[0]);
									return;
								}
							}
							else
							{
								CS$<>8__locals1.requestResult = CodingRequestResult.NoData;
							}
						};
						obdrequest.ResponseDecoded += delegate(OBDRequest getDataRequest2, byte[] getDataRequestData, bool decodeResult, string responseHeader)
						{
							if (getDataRequestData != null && getDataRequestData.Length != 0)
							{
								CS$<>8__locals1.resultBytes = getDataRequestData;
								CS$<>8__locals1.requestResult = CodingRequestResult.Success;
							}
						};
						list.Add(obdrequest);
						App.OBDReader.ReplaceQueue(list);
						taskAwaiter = App.OBDReader.WaitForCommandQueue().GetAwaiter();
						if (!taskAwaiter.IsCompleted)
						{
							num2 = 0;
							TaskAwaiter taskAwaiter2 = taskAwaiter;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, MQBCustomizableMode22Coding.<GetCurrentStateRawData>d__17>(ref taskAwaiter, ref this);
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
					tuple = new Tuple<byte[], CodingRequestResult>(CS$<>8__locals1.resultBytes, CS$<>8__locals1.requestResult);
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

			// Token: 0x06004B53 RID: 19283 RVA: 0x00383084 File Offset: 0x00381284
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x04002BFA RID: 11258
			public int <>1__state;

			// Token: 0x04002BFB RID: 11259
			public AsyncTaskMethodBuilder<Tuple<byte[], CodingRequestResult>> <>t__builder;

			// Token: 0x04002BFC RID: 11260
			public MQBCustomizableMode22Coding <>4__this;

			// Token: 0x04002BFD RID: 11261
			public string password;

			// Token: 0x04002BFE RID: 11262
			private MQBCustomizableMode22Coding.<>c__DisplayClass17_0 <>8__1;

			// Token: 0x04002BFF RID: 11263
			private TaskAwaiter <>u__1;
		}

		// Token: 0x020008A8 RID: 2216
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <WriteDataToECU>d__18 : IAsyncStateMachine
		{
			// Token: 0x06004B54 RID: 19284 RVA: 0x00383094 File Offset: 0x00381294
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				MQBCustomizableMode22Coding mqbcustomizableMode22Coding = this;
				CodingRequestResult requestResult;
				try
				{
					TaskAwaiter taskAwaiter;
					if (num != 0)
					{
						CS$<>8__locals1 = new MQBCustomizableMode22Coding.<>c__DisplayClass18_0();
						CS$<>8__locals1.<>4__this = this;
						CS$<>8__locals1.progress = progress;
						mqbcustomizableMode22Coding.BuildDefaultBeforeAndAfterCommands();
						CS$<>8__locals1.writeServicePositiveResponse = (int.Parse(mqbcustomizableMode22Coding.WriteMode, NumberStyles.HexNumber) + 64).ToString("X2");
						CS$<>8__locals1.requestResult = CodingRequestResult.UnknownError;
						List<OBDRequest> list = new List<OBDRequest>();
						list.AddRange(MQBParametrizeBase.GetRequestsFromStringCommands(mqbcustomizableMode22Coding.PreWriteCommands, password, mqbcustomizableMode22Coding.RequestHeader, mqbcustomizableMode22Coding.BeforeCommands, mqbcustomizableMode22Coding.AfterCommands));
						OBDRequest obdrequest = new OBDRequest(mqbcustomizableMode22Coding.WriteMode + mqbcustomizableMode22Coding.Address + checked_value, mqbcustomizableMode22Coding.RequestHeader, mqbcustomizableMode22Coding.BeforeCommands, mqbcustomizableMode22Coding.AfterCommands, false);
						obdrequest.ResponseReceived += delegate(OBDRequest writeRequest2, string writeRequestData)
						{
							if (writeRequestData != null)
							{
								string text = OBDDataReader.FilterHexAndNewLineOnly(writeRequestData);
								if (text.Contains("7F" + CS$<>8__locals1.<>4__this.WriteMode + "78") || text.Contains(CS$<>8__locals1.writeServicePositiveResponse))
								{
									IProgress<string> progress = CS$<>8__locals1.progress;
									if (progress != null)
									{
										progress.Report(Translate.GetString("coding_progress_DataAccepted"));
									}
									CS$<>8__locals1.requestResult = CodingRequestResult.Success;
									return;
								}
								if (text.Length >= 11 && text.Contains("7F" + CS$<>8__locals1.<>4__this.WriteMode))
								{
									int num3 = text.IndexOf("7F" + CS$<>8__locals1.<>4__this.WriteMode);
									int num4 = int.Parse(text.Substring(num3 + 4, 2), NumberStyles.HexNumber);
									if (num4 >= 128 || num4 == 34)
									{
										CS$<>8__locals1.requestResult = CodingRequestResult.WrongConditions;
									}
									else if (num4 == 51)
									{
										CS$<>8__locals1.requestResult = CodingRequestResult.WrongAccessKey;
									}
									else if (num4 == 49)
									{
										CS$<>8__locals1.requestResult = CodingRequestResult.NotSupported;
									}
									else
									{
										CS$<>8__locals1.requestResult = CodingRequestResult.UnknownError;
									}
								}
								else
								{
									CS$<>8__locals1.requestResult = CodingRequestResult.UnknownError;
								}
								App.OBDReader.ReplaceQueue(new OBDRequest[0]);
							}
						};
						obdrequest.ELMFormat = ELMFormat.CAN11bit;
						obdrequest.Keys.Add("TesterPresent", "ATSH700;023E80;ATSH" + mqbcustomizableMode22Coding.RequestHeader);
						list.Add(obdrequest);
						list.AddRange(MQBParametrizeBase.GetRequestsFromStringCommands(mqbcustomizableMode22Coding.PostWriteCommands, password, mqbcustomizableMode22Coding.RequestHeader, mqbcustomizableMode22Coding.BeforeCommands, mqbcustomizableMode22Coding.AfterCommands));
						App.OBDReader.ReplaceQueue(list);
						taskAwaiter = App.OBDReader.WaitForCommandQueue().GetAwaiter();
						if (!taskAwaiter.IsCompleted)
						{
							num2 = 0;
							TaskAwaiter taskAwaiter2 = taskAwaiter;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, MQBCustomizableMode22Coding.<WriteDataToECU>d__18>(ref taskAwaiter, ref this);
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
					if (CS$<>8__locals1.requestResult == CodingRequestResult.Success && originalData != null)
					{
						Dictionary<string, string> dictionary = new Dictionary<string, string>();
						CodingLogItem.RecordToLog(mqbcustomizableMode22Coding.Name, UserFriendlyValue, CodingLogItem.CodingTypes.MQBCustomizableMode22Coding, mqbcustomizableMode22Coding.Address, BitHelpers.ByteArrayToHexString(originalData), checked_value, password, mqbcustomizableMode22Coding.RequestHeader, mqbcustomizableMode22Coding.ResponseHeader, mqbcustomizableMode22Coding.PreReadCommands, mqbcustomizableMode22Coding.PreWriteCommands, mqbcustomizableMode22Coding.PostWriteCommands, mqbcustomizableMode22Coding.ATST, dictionary, "");
					}
					requestResult = CS$<>8__locals1.requestResult;
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
				this.<>t__builder.SetResult(requestResult);
			}

			// Token: 0x06004B55 RID: 19285 RVA: 0x0038333C File Offset: 0x0038153C
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x04002C00 RID: 11264
			public int <>1__state;

			// Token: 0x04002C01 RID: 11265
			public AsyncTaskMethodBuilder<CodingRequestResult> <>t__builder;

			// Token: 0x04002C02 RID: 11266
			public MQBCustomizableMode22Coding <>4__this;

			// Token: 0x04002C03 RID: 11267
			public IProgress<string> progress;

			// Token: 0x04002C04 RID: 11268
			public string password;

			// Token: 0x04002C05 RID: 11269
			public string checked_value;

			// Token: 0x04002C06 RID: 11270
			private MQBCustomizableMode22Coding.<>c__DisplayClass18_0 <>8__1;

			// Token: 0x04002C07 RID: 11271
			public byte[] originalData;

			// Token: 0x04002C08 RID: 11272
			public string UserFriendlyValue;

			// Token: 0x04002C09 RID: 11273
			private TaskAwaiter <>u__1;
		}
	}
}
