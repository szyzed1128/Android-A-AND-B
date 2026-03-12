using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using CarScannerXamarinForms.Common;
using CarScannerXamarinForms.OBD2;
using CarScannerXamarinForms.Settings;

namespace CarScannerXamarinForms.Coding.DB.PQ26_NewRapid2020
{
	// Token: 0x02000A35 RID: 2613
	internal class MQBDataSet2EWriteOnly : MQBCustomizableMode22Coding
	{
		// Token: 0x060052EC RID: 21228 RVA: 0x003FAF90 File Offset: 0x003F9190
		public MQBDataSet2EWriteOnly(string address, CodingGroup group, string name, string description, string innerDescription, string password, string preReadCommands, string preWriteCommands, string postWriteCommands, string unit, string path, string identsAddress, Func<byte[], MQBDataSet2EWriteOnly, string> getCurrentStateDelegate)
		{
			base.Address = address;
			base.Group = group;
			base.Name = name;
			base.Description = description;
			base.InnerDescription = innerDescription;
			base.ValueType = AdaptationValueTypes.OptionType;
			base.RequestHeader = VagUnitHelper.GetRequestHeaderForMQBUnit(unit);
			base.ResponseHeader = VagUnitHelper.GetResponseHeaderForMQBUnit(unit);
			this.Password = password;
			base.PreReadCommands = preReadCommands;
			base.PreWriteCommands = preWriteCommands;
			base.PostWriteCommands = postWriteCommands;
			this.GetCurrentStateDelegate = getCurrentStateDelegate;
			this.GetCurrentStateAddress = identsAddress;
			this.HasCurrentState = false;
			string[] array = (from x in PackageFileReader.GetFilesInDirectory(path)
				orderby x
				select x).ToArray<string>();
			for (int i = 0; i < array.Length; i++)
			{
				MQBAdaptationOption mqbadaptationOption = new MQBAdaptationOption(string.Format(Translate.GetString("coding_Variant"), i + 1), path + "." + array[i]);
				base.Options.Add(mqbadaptationOption);
			}
		}

		// Token: 0x170017D8 RID: 6104
		// (get) Token: 0x060052ED RID: 21229 RVA: 0x003FB097 File Offset: 0x003F9297
		// (set) Token: 0x060052EE RID: 21230 RVA: 0x003FB09F File Offset: 0x003F929F
		protected virtual string GetCurrentStateAddress
		{
			[CompilerGenerated]
			get
			{
				return this.<GetCurrentStateAddress>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<GetCurrentStateAddress>k__BackingField = value;
			}
		}

		// Token: 0x170017D9 RID: 6105
		// (get) Token: 0x060052EF RID: 21231 RVA: 0x003FB0A8 File Offset: 0x003F92A8
		// (set) Token: 0x060052F0 RID: 21232 RVA: 0x003FB0B0 File Offset: 0x003F92B0
		protected virtual Func<byte[], MQBDataSet2EWriteOnly, string> GetCurrentStateDelegate
		{
			[CompilerGenerated]
			get
			{
				return this.<GetCurrentStateDelegate>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<GetCurrentStateDelegate>k__BackingField = value;
			}
		}

		// Token: 0x060052F1 RID: 21233 RVA: 0x003FB0BC File Offset: 0x003F92BC
		protected void LoadOptionValue(MQBAdaptationOption opt)
		{
			if (opt.Value.Contains('.'))
			{
				string text = PackageFileReader.ReadFileToString(opt.Value);
				opt.Value = text;
			}
		}

		// Token: 0x060052F2 RID: 21234 RVA: 0x003FB0EC File Offset: 0x003F92EC
		public override async Task<CodingRequestResult> UpdateCurrentState(string password, IProgress<string> progress = null)
		{
			CodingRequestResult codingRequestResult;
			if (this.GetCurrentStateDelegate == null)
			{
				base.CurrentState = MQBAdaptationTemplate.UNKNOWN_TITLE;
				codingRequestResult = CodingRequestResult.Success;
			}
			else
			{
				foreach (MQBAdaptationOption mqbadaptationOption in base.Options)
				{
					this.LoadOptionValue(mqbadaptationOption);
				}
				if (string.IsNullOrEmpty(password))
				{
					password = this.Password;
				}
				Tuple<byte[], CodingRequestResult> tuple = await this.GetCurrentStateRawData(password);
				CodingRequestResult item = tuple.Item2;
				byte[] item2 = tuple.Item1;
				if (item != CodingRequestResult.Success)
				{
					base.CurrentState = MQBAdaptationTemplate.CodingRequestResultToString(item);
					codingRequestResult = item;
				}
				else
				{
					base.CurrentState = this.GetCurrentStateDelegate(item2, this);
					codingRequestResult = item;
				}
			}
			return codingRequestResult;
		}

		// Token: 0x060052F3 RID: 21235 RVA: 0x003FB138 File Offset: 0x003F9338
		public override async Task<Tuple<byte[], CodingRequestResult>> GetCurrentStateRawData(string password)
		{
			this.BuildDefaultBeforeAndAfterCommands();
			CodingRequestResult requestResult = CodingRequestResult.UnknownError;
			List<OBDRequest> list = new List<OBDRequest>();
			list.AddRange(MQBParametrizeBase.GetRequestsFromStringCommands(base.PreReadCommands, password, base.RequestHeader, this.BeforeCommands, this.AfterCommands));
			base.ReadMode + base.Address;
			string readServicePositiveResponse = (int.Parse(base.ReadMode, NumberStyles.HexNumber) + 64).ToString("X2");
			byte[] resultBytes = new byte[0];
			OBDRequest obdrequest = new OBDRequest("22" + this.GetCurrentStateAddress, base.RequestHeader, this.BeforeCommands, this.AfterCommands, false)
			{
				CheckLength = true,
				ELMFormat = ELMFormat.CAN11bit,
				ForceManualFlowControl = SharedSettings.Current.ForceUseManualFlowControlForCodingOperations
			};
			obdrequest.ResponseReceived += delegate(OBDRequest readRequest2, string lastRequestData)
			{
				if (lastRequestData != null)
				{
					string text = OBDDataReader.FilterHexAndNewLineOnly(lastRequestData);
					if (!text.Contains("7F" + this.ReadMode + "78") && !text.Contains(readServicePositiveResponse))
					{
						if (text.Length >= 11 && text.Contains("7F" + this.ReadMode))
						{
							int num = text.IndexOf("7F" + this.ReadMode);
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

		// Token: 0x060052F4 RID: 21236 RVA: 0x003FB184 File Offset: 0x003F9384
		public override async Task<CodingRequestResult> Execute(string password, string value, string UserFriendlyValue, IProgress<string> progress, byte[] originalData = null, bool skipIfTheSameData = false)
		{
			OBDRequest obdrequest = new OBDRequest("010C", false);
			bool engineIsRunning = false;
			obdrequest.ResponseDecoded += delegate(OBDRequest rpm_req2, byte[] data, bool decodeResult, string responseHeader)
			{
				if (data != null && data.Length >= 2 && ((int)data[0] * 256 + (int)data[1]) / 4 > 0)
				{
					engineIsRunning = true;
				}
			};
			App.OBDReader.ReplaceQueue(new OBDRequest[] { obdrequest });
			await App.OBDReader.WaitForCommandQueue();
			CodingRequestResult codingRequestResult;
			if (engineIsRunning)
			{
				codingRequestResult = CodingRequestResult.WrongConditions;
			}
			else
			{
				string text = "";
				MQBAdaptationOption mqbadaptationOption = base.Options.FirstOrDefault((MQBAdaptationOption x) => x.Value == value);
				if (mqbadaptationOption != null)
				{
					text = mqbadaptationOption.Title;
				}
				codingRequestResult = await this.WriteDataToECU(password, text, progress, null, value);
			}
			return codingRequestResult;
		}

		// Token: 0x040032B5 RID: 12981
		[CompilerGenerated]
		private string <GetCurrentStateAddress>k__BackingField;

		// Token: 0x040032B6 RID: 12982
		[CompilerGenerated]
		private Func<byte[], MQBDataSet2EWriteOnly, string> <GetCurrentStateDelegate>k__BackingField;

		// Token: 0x02000A36 RID: 2614
		[CompilerGenerated]
		[Serializable]
		private sealed class <>c
		{
			// Token: 0x060052F5 RID: 21237 RVA: 0x003FB1E0 File Offset: 0x003F93E0
			// Note: this type is marked as 'beforefieldinit'.
			static <>c()
			{
			}

			// Token: 0x060052F6 RID: 21238 RVA: 0x00002050 File Offset: 0x00000250
			public <>c()
			{
			}

			// Token: 0x060052F7 RID: 21239 RVA: 0x00016849 File Offset: 0x00014A49
			internal string <.ctor>b__0_0(string x)
			{
				return x;
			}

			// Token: 0x040032B7 RID: 12983
			public static readonly MQBDataSet2EWriteOnly.<>c <>9 = new MQBDataSet2EWriteOnly.<>c();

			// Token: 0x040032B8 RID: 12984
			public static Func<string, string> <>9__0_0;
		}

		// Token: 0x02000A37 RID: 2615
		[CompilerGenerated]
		private sealed class <>c__DisplayClass11_0
		{
			// Token: 0x060052F8 RID: 21240 RVA: 0x00002050 File Offset: 0x00000250
			public <>c__DisplayClass11_0()
			{
			}

			// Token: 0x060052F9 RID: 21241 RVA: 0x003FB1EC File Offset: 0x003F93EC
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

			// Token: 0x060052FA RID: 21242 RVA: 0x003FB2F3 File Offset: 0x003F94F3
			internal void <GetCurrentStateRawData>b__1(OBDRequest getDataRequest2, byte[] getDataRequestData, bool decodeResult, string responseHeader)
			{
				if (getDataRequestData != null && getDataRequestData.Length != 0)
				{
					this.resultBytes = getDataRequestData;
					this.requestResult = CodingRequestResult.Success;
				}
			}

			// Token: 0x040032B9 RID: 12985
			public MQBDataSet2EWriteOnly <>4__this;

			// Token: 0x040032BA RID: 12986
			public string readServicePositiveResponse;

			// Token: 0x040032BB RID: 12987
			public CodingRequestResult requestResult;

			// Token: 0x040032BC RID: 12988
			public byte[] resultBytes;
		}

		// Token: 0x02000A38 RID: 2616
		[CompilerGenerated]
		private sealed class <>c__DisplayClass12_0
		{
			// Token: 0x060052FB RID: 21243 RVA: 0x00002050 File Offset: 0x00000250
			public <>c__DisplayClass12_0()
			{
			}

			// Token: 0x060052FC RID: 21244 RVA: 0x003FB30A File Offset: 0x003F950A
			internal void <Execute>b__0(OBDRequest rpm_req2, byte[] data, bool decodeResult, string responseHeader)
			{
				if (data != null && data.Length >= 2 && ((int)data[0] * 256 + (int)data[1]) / 4 > 0)
				{
					this.engineIsRunning = true;
				}
			}

			// Token: 0x060052FD RID: 21245 RVA: 0x003FB32E File Offset: 0x003F952E
			internal bool <Execute>b__1(MQBAdaptationOption x)
			{
				return x.Value == this.value;
			}

			// Token: 0x040032BD RID: 12989
			public bool engineIsRunning;

			// Token: 0x040032BE RID: 12990
			public string value;
		}

		// Token: 0x02000A39 RID: 2617
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <Execute>d__12 : IAsyncStateMachine
		{
			// Token: 0x060052FE RID: 21246 RVA: 0x003FB344 File Offset: 0x003F9544
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				MQBDataSet2EWriteOnly mqbdataSet2EWriteOnly = this;
				CodingRequestResult codingRequestResult;
				try
				{
					TaskAwaiter<CodingRequestResult> taskAwaiter;
					TaskAwaiter taskAwaiter3;
					if (num != 0)
					{
						if (num == 1)
						{
							TaskAwaiter<CodingRequestResult> taskAwaiter2;
							taskAwaiter = taskAwaiter2;
							taskAwaiter2 = default(TaskAwaiter<CodingRequestResult>);
							num2 = -1;
							goto IL_0190;
						}
						CS$<>8__locals1 = new MQBDataSet2EWriteOnly.<>c__DisplayClass12_0();
						CS$<>8__locals1.value = value;
						OBDRequest obdrequest = new OBDRequest("010C", false);
						CS$<>8__locals1.engineIsRunning = false;
						obdrequest.ResponseDecoded += delegate(OBDRequest rpm_req2, byte[] data, bool decodeResult, string responseHeader)
						{
							if (data != null && data.Length >= 2 && ((int)data[0] * 256 + (int)data[1]) / 4 > 0)
							{
								CS$<>8__locals1.engineIsRunning = true;
							}
						};
						App.OBDReader.ReplaceQueue(new OBDRequest[] { obdrequest });
						taskAwaiter3 = App.OBDReader.WaitForCommandQueue().GetAwaiter();
						if (!taskAwaiter3.IsCompleted)
						{
							num2 = 0;
							TaskAwaiter taskAwaiter4 = taskAwaiter3;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, MQBDataSet2EWriteOnly.<Execute>d__12>(ref taskAwaiter3, ref this);
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
					if (CS$<>8__locals1.engineIsRunning)
					{
						codingRequestResult = CodingRequestResult.WrongConditions;
						goto IL_01BA;
					}
					string text = "";
					MQBAdaptationOption mqbadaptationOption = mqbdataSet2EWriteOnly.Options.FirstOrDefault((MQBAdaptationOption x) => x.Value == CS$<>8__locals1.value);
					if (mqbadaptationOption != null)
					{
						text = mqbadaptationOption.Title;
					}
					taskAwaiter = mqbdataSet2EWriteOnly.WriteDataToECU(password, text, progress, null, CS$<>8__locals1.value).GetAwaiter();
					if (!taskAwaiter.IsCompleted)
					{
						num2 = 1;
						TaskAwaiter<CodingRequestResult> taskAwaiter2 = taskAwaiter;
						this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<CodingRequestResult>, MQBDataSet2EWriteOnly.<Execute>d__12>(ref taskAwaiter, ref this);
						return;
					}
					IL_0190:
					codingRequestResult = taskAwaiter.GetResult();
				}
				catch (Exception ex)
				{
					num2 = -2;
					CS$<>8__locals1 = null;
					this.<>t__builder.SetException(ex);
					return;
				}
				IL_01BA:
				num2 = -2;
				CS$<>8__locals1 = null;
				this.<>t__builder.SetResult(codingRequestResult);
			}

			// Token: 0x060052FF RID: 21247 RVA: 0x003FB544 File Offset: 0x003F9744
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x040032BF RID: 12991
			public int <>1__state;

			// Token: 0x040032C0 RID: 12992
			public AsyncTaskMethodBuilder<CodingRequestResult> <>t__builder;

			// Token: 0x040032C1 RID: 12993
			public string value;

			// Token: 0x040032C2 RID: 12994
			private MQBDataSet2EWriteOnly.<>c__DisplayClass12_0 <>8__1;

			// Token: 0x040032C3 RID: 12995
			public MQBDataSet2EWriteOnly <>4__this;

			// Token: 0x040032C4 RID: 12996
			public string password;

			// Token: 0x040032C5 RID: 12997
			public IProgress<string> progress;

			// Token: 0x040032C6 RID: 12998
			private TaskAwaiter <>u__1;

			// Token: 0x040032C7 RID: 12999
			private TaskAwaiter<CodingRequestResult> <>u__2;
		}

		// Token: 0x02000A3A RID: 2618
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <GetCurrentStateRawData>d__11 : IAsyncStateMachine
		{
			// Token: 0x06005300 RID: 21248 RVA: 0x003FB554 File Offset: 0x003F9754
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				MQBDataSet2EWriteOnly mqbdataSet2EWriteOnly = this;
				Tuple<byte[], CodingRequestResult> tuple;
				try
				{
					TaskAwaiter taskAwaiter;
					if (num != 0)
					{
						CS$<>8__locals1 = new MQBDataSet2EWriteOnly.<>c__DisplayClass11_0();
						CS$<>8__locals1.<>4__this = this;
						mqbdataSet2EWriteOnly.BuildDefaultBeforeAndAfterCommands();
						CS$<>8__locals1.requestResult = CodingRequestResult.UnknownError;
						List<OBDRequest> list = new List<OBDRequest>();
						list.AddRange(MQBParametrizeBase.GetRequestsFromStringCommands(mqbdataSet2EWriteOnly.PreReadCommands, password, mqbdataSet2EWriteOnly.RequestHeader, mqbdataSet2EWriteOnly.BeforeCommands, mqbdataSet2EWriteOnly.AfterCommands));
						mqbdataSet2EWriteOnly.ReadMode + mqbdataSet2EWriteOnly.Address;
						CS$<>8__locals1.readServicePositiveResponse = (int.Parse(mqbdataSet2EWriteOnly.ReadMode, NumberStyles.HexNumber) + 64).ToString("X2");
						CS$<>8__locals1.resultBytes = new byte[0];
						OBDRequest obdrequest = new OBDRequest("22" + mqbdataSet2EWriteOnly.GetCurrentStateAddress, mqbdataSet2EWriteOnly.RequestHeader, mqbdataSet2EWriteOnly.BeforeCommands, mqbdataSet2EWriteOnly.AfterCommands, false)
						{
							CheckLength = true,
							ELMFormat = ELMFormat.CAN11bit,
							ForceManualFlowControl = SharedSettings.Current.ForceUseManualFlowControlForCodingOperations
						};
						obdrequest.ResponseReceived += delegate(OBDRequest readRequest2, string lastRequestData)
						{
							if (lastRequestData != null)
							{
								string text = OBDDataReader.FilterHexAndNewLineOnly(lastRequestData);
								if (!text.Contains("7F" + CS$<>8__locals1.<>4__this.ReadMode + "78") && !text.Contains(CS$<>8__locals1.readServicePositiveResponse))
								{
									if (text.Length >= 11 && text.Contains("7F" + CS$<>8__locals1.<>4__this.ReadMode))
									{
										int num3 = text.IndexOf("7F" + CS$<>8__locals1.<>4__this.ReadMode);
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
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, MQBDataSet2EWriteOnly.<GetCurrentStateRawData>d__11>(ref taskAwaiter, ref this);
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

			// Token: 0x06005301 RID: 21249 RVA: 0x003FB780 File Offset: 0x003F9980
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x040032C8 RID: 13000
			public int <>1__state;

			// Token: 0x040032C9 RID: 13001
			public AsyncTaskMethodBuilder<Tuple<byte[], CodingRequestResult>> <>t__builder;

			// Token: 0x040032CA RID: 13002
			public MQBDataSet2EWriteOnly <>4__this;

			// Token: 0x040032CB RID: 13003
			public string password;

			// Token: 0x040032CC RID: 13004
			private MQBDataSet2EWriteOnly.<>c__DisplayClass11_0 <>8__1;

			// Token: 0x040032CD RID: 13005
			private TaskAwaiter <>u__1;
		}

		// Token: 0x02000A3B RID: 2619
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <UpdateCurrentState>d__10 : IAsyncStateMachine
		{
			// Token: 0x06005302 RID: 21250 RVA: 0x003FB790 File Offset: 0x003F9990
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				MQBDataSet2EWriteOnly mqbdataSet2EWriteOnly = this;
				CodingRequestResult codingRequestResult;
				try
				{
					TaskAwaiter<Tuple<byte[], CodingRequestResult>> taskAwaiter;
					if (num != 0)
					{
						if (mqbdataSet2EWriteOnly.GetCurrentStateDelegate == null)
						{
							mqbdataSet2EWriteOnly.CurrentState = MQBAdaptationTemplate.UNKNOWN_TITLE;
							codingRequestResult = CodingRequestResult.Success;
							goto IL_0138;
						}
						IEnumerator<MQBAdaptationOption> enumerator = mqbdataSet2EWriteOnly.Options.GetEnumerator();
						try
						{
							while (enumerator.MoveNext())
							{
								MQBAdaptationOption mqbadaptationOption = enumerator.Current;
								mqbdataSet2EWriteOnly.LoadOptionValue(mqbadaptationOption);
							}
						}
						finally
						{
							if (num < 0 && enumerator != null)
							{
								enumerator.Dispose();
							}
						}
						if (string.IsNullOrEmpty(password))
						{
							password = mqbdataSet2EWriteOnly.Password;
						}
						taskAwaiter = mqbdataSet2EWriteOnly.GetCurrentStateRawData(password).GetAwaiter();
						if (!taskAwaiter.IsCompleted)
						{
							num = (num2 = 0);
							TaskAwaiter<Tuple<byte[], CodingRequestResult>> taskAwaiter2 = taskAwaiter;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<Tuple<byte[], CodingRequestResult>>, MQBDataSet2EWriteOnly.<UpdateCurrentState>d__10>(ref taskAwaiter, ref this);
							return;
						}
					}
					else
					{
						TaskAwaiter<Tuple<byte[], CodingRequestResult>> taskAwaiter2;
						taskAwaiter = taskAwaiter2;
						taskAwaiter2 = default(TaskAwaiter<Tuple<byte[], CodingRequestResult>>);
						num = (num2 = -1);
					}
					Tuple<byte[], CodingRequestResult> result = taskAwaiter.GetResult();
					CodingRequestResult item = result.Item2;
					byte[] item2 = result.Item1;
					if (item != CodingRequestResult.Success)
					{
						mqbdataSet2EWriteOnly.CurrentState = MQBAdaptationTemplate.CodingRequestResultToString(item);
						codingRequestResult = item;
					}
					else
					{
						mqbdataSet2EWriteOnly.CurrentState = mqbdataSet2EWriteOnly.GetCurrentStateDelegate(item2, mqbdataSet2EWriteOnly);
						codingRequestResult = item;
					}
				}
				catch (Exception ex)
				{
					num2 = -2;
					this.<>t__builder.SetException(ex);
					return;
				}
				IL_0138:
				num2 = -2;
				this.<>t__builder.SetResult(codingRequestResult);
			}

			// Token: 0x06005303 RID: 21251 RVA: 0x003FB920 File Offset: 0x003F9B20
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x040032CE RID: 13006
			public int <>1__state;

			// Token: 0x040032CF RID: 13007
			public AsyncTaskMethodBuilder<CodingRequestResult> <>t__builder;

			// Token: 0x040032D0 RID: 13008
			public MQBDataSet2EWriteOnly <>4__this;

			// Token: 0x040032D1 RID: 13009
			public string password;

			// Token: 0x040032D2 RID: 13010
			private TaskAwaiter<Tuple<byte[], CodingRequestResult>> <>u__1;
		}
	}
}
