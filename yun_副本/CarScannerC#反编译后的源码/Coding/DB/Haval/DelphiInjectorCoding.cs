using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using CarScannerXamarinForms.Common;
using CarScannerXamarinForms.OBD2;
using CarScannerXamarinForms.OBD2.PIDS;
using CarScannerXamarinForms.Settings;

namespace CarScannerXamarinForms.Coding.DB.Haval
{
	// Token: 0x02000BD8 RID: 3032
	internal class DelphiInjectorCoding : CustomizableCodingTemplate
	{
		// Token: 0x06005B46 RID: 23366 RVA: 0x00438070 File Offset: 0x00436270
		public DelphiInjectorCoding(int number, string readAddress, string writeAddress)
		{
			base.Name = "4D20 Delphi Injector code #" + number.ToString();
			base.Description = "WARNING! Not tested! Use at your own risk!";
			base.Translations.Add(new TranslationItem("ru", "4D20 Delphi Код форсунки " + number.ToString(), "ВНИМАНИЕ! Не проверено! Используйте на свой страх и риск!", ""));
			this.ReadModeAndAddress = readAddress;
			base.WriteModeAndAddress = writeAddress;
			base.MakeChangesToInitialData = true;
			base.Protocol = "6";
			base.ATST = "32";
			base.RequestHeader = "7E0";
			base.ResponseHeader = "7E8";
			this.ValueType = AdaptationValueTypes.InputTextType;
			base.PreWriteCommands = "2701;2702";
			base.OpenSessionCommand = "1003";
			this.InjectorNumber = number;
			this.PasswordVisible = false;
			this.Group = CodingGroup.EngineAndPowertrain;
		}

		// Token: 0x06005B47 RID: 23367 RVA: 0x00438148 File Offset: 0x00436348
		protected override void BuildDefaultBeforeAndAfterCommands()
		{
			string text = (string.IsNullOrEmpty(base.Protocol) ? "" : ("ATSP" + base.Protocol + ";"));
			string text2 = ((base.RequestHeader.Length == 8) ? ("ATCP" + base.RequestHeader.Substring(0, 2) + ";") : "");
			if (string.IsNullOrEmpty(base.ExtendedAddress))
			{
				this.BeforeCommands = string.Concat(new string[] { text, text2, "ATFCSH", base.RequestHeader, ";ATFCSD300000;ATFCSM0;ATAL;ATCRA", base.ResponseHeader, ";ATST", base.ATST });
				this.AfterCommands = string.Format("ATFCSM0;ATD;ATSP{0};ATE0;ATH1;ATS0;ATSTDEF", SharedSettings.Current.ProtocolNumber);
			}
			else
			{
				this.BeforeCommands = string.Concat(new string[]
				{
					text, text2, "ATFCSH", base.RequestHeader, ";ATFCSD300000;ATFCSM0;ATAL;ATCRA", base.ResponseHeader, ";ATCEA", base.ExtendedAddress, ";ATTA", base.ExtendedAddress,
					";ATST", base.ATST
				});
				this.AfterCommands = string.Format("ATFCSM0;ATD;ATSP{0};ATE0;ATH1;ATS0;ATSTDEF", SharedSettings.Current.ProtocolNumber);
			}
			if (SharedSettings.Current.ShowExperimental && App.OBDReader.CurrentELMFormat == ELMFormat.CAN11bit && (base.Protocol == "" || base.Protocol == "6"))
			{
				if (string.IsNullOrEmpty(base.ExtendedAddress))
				{
					this.AfterCommands = "ATFCSM0;ATCAF1;ATAR;ATSTDEF";
					return;
				}
				this.AfterCommands = "ATFCSM0;ATCAF1;ATAR;ATCEA;ATSTDEF";
			}
		}

		// Token: 0x06005B48 RID: 23368 RVA: 0x00438320 File Offset: 0x00436520
		public override async Task<CodingRequestResult> Execute(string password, string value, string UserFriendlyValue, IProgress<string> progress, byte[] originalData = null, bool skipIfTheSameData = false)
		{
			CodingRequestResult codingRequestResult;
			if (value.Length != 20)
			{
				codingRequestResult = CodingRequestResult.WrongInputValue;
			}
			else
			{
				byte[] array = DelphiC3iCodeConverter.CodeToBytes(value);
				string checked_value = BitHelpers.ByteArrayToHexString(array);
				if (base.MakeChangesToInitialData && originalData == null)
				{
					if (progress != null)
					{
						progress.Report(Translate.GetString("coding_progress_RequestingOriginalData"));
					}
					Tuple<byte[], CodingRequestResult> tuple = await this.GetCurrentStateRawData("");
					if (tuple.Item2 != CodingRequestResult.Success)
					{
						return tuple.Item2;
					}
					originalData = tuple.Item1;
				}
				if (skipIfTheSameData && base.MakeChangesToInitialData && originalData != null && ArrayHelpers.ArrayEquals<byte>(BitHelpers.ConvertHexToBytesX(checked_value), originalData))
				{
					Dictionary<string, string> dictionary = new Dictionary<string, string>();
					dictionary.Add("Protocol", base.Protocol);
					dictionary.Add("ExtendedAddress", base.ExtendedAddress);
					dictionary.Add("TesterAddress", base.TesterAddress);
					CodingLogItem.RecordToLog(base.Name, UserFriendlyValue, CodingLogItem.CodingTypes.CustomizableCodingTemplate, base.WriteModeAndAddress, BitHelpers.ByteArrayToHexString(originalData), checked_value, password, base.RequestHeader, base.ResponseHeader, base.OpenSessionCommand, base.PreWriteCommands, base.PostWriteCommands, base.ATST, dictionary, base.PreReadCommands);
					codingRequestResult = CodingRequestResult.Success;
				}
				else
				{
					codingRequestResult = await this.WriteDataToECU(password, UserFriendlyValue, progress, originalData, checked_value);
				}
			}
			return codingRequestResult;
		}

		// Token: 0x06005B49 RID: 23369 RVA: 0x00438398 File Offset: 0x00436598
		public override async Task<CodingRequestResult> UpdateCurrentState(string password, IProgress<string> progress = null)
		{
			CodingRequestResult codingRequestResult;
			if (string.IsNullOrEmpty(this.ReadModeAndAddress))
			{
				codingRequestResult = CodingRequestResult.Success;
			}
			else
			{
				Tuple<byte[], CodingRequestResult> tuple = await this.GetCurrentStateRawData(password);
				CodingRequestResult item = tuple.Item2;
				byte[] item2 = tuple.Item1;
				if (item != CodingRequestResult.Success)
				{
					base.CurrentState = CustomizableCodingTemplate.CodingRequestResultToString(item);
					codingRequestResult = item;
				}
				else if (item2.Length == 21)
				{
					byte[] array = new byte[item2.Length - 1];
					Array.Copy(item2, 1, array, 0, array.Length);
					base.CurrentState = DelphiC3iCodeConverter.BytesToCode(array);
					codingRequestResult = CodingRequestResult.Success;
				}
				else
				{
					base.CurrentState = MQBAdaptationTemplate.UNKNOWN_TITLE;
					codingRequestResult = CodingRequestResult.InitialDataIncorrect;
				}
			}
			return codingRequestResult;
		}

		// Token: 0x0400398D RID: 14733
		private int InjectorNumber;

		// Token: 0x02000BD9 RID: 3033
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <Execute>d__3 : IAsyncStateMachine
		{
			// Token: 0x06005B4A RID: 23370 RVA: 0x004383E4 File Offset: 0x004365E4
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				DelphiInjectorCoding delphiInjectorCoding = this;
				CodingRequestResult codingRequestResult;
				try
				{
					TaskAwaiter<CodingRequestResult> taskAwaiter;
					IProgress<string> progress;
					TaskAwaiter<Tuple<byte[], CodingRequestResult>> taskAwaiter3;
					if (num != 0)
					{
						if (num == 1)
						{
							TaskAwaiter<CodingRequestResult> taskAwaiter2;
							taskAwaiter = taskAwaiter2;
							taskAwaiter2 = default(TaskAwaiter<CodingRequestResult>);
							num2 = -1;
							goto IL_0250;
						}
						if (value.Length != 20)
						{
							codingRequestResult = CodingRequestResult.WrongInputValue;
							goto IL_027A;
						}
						byte[] array = DelphiC3iCodeConverter.CodeToBytes(value);
						checked_value = BitHelpers.ByteArrayToHexString(array);
						if (!delphiInjectorCoding.MakeChangesToInitialData || originalData != null)
						{
							goto IL_0103;
						}
						progress = progress;
						if (progress != null)
						{
							progress.Report(Translate.GetString("coding_progress_RequestingOriginalData"));
						}
						taskAwaiter3 = delphiInjectorCoding.GetCurrentStateRawData("").GetAwaiter();
						if (!taskAwaiter3.IsCompleted)
						{
							num2 = 0;
							TaskAwaiter<Tuple<byte[], CodingRequestResult>> taskAwaiter4 = taskAwaiter3;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<Tuple<byte[], CodingRequestResult>>, DelphiInjectorCoding.<Execute>d__3>(ref taskAwaiter3, ref this);
							return;
						}
					}
					else
					{
						TaskAwaiter<Tuple<byte[], CodingRequestResult>> taskAwaiter4;
						taskAwaiter3 = taskAwaiter4;
						taskAwaiter4 = default(TaskAwaiter<Tuple<byte[], CodingRequestResult>>);
						num2 = -1;
					}
					Tuple<byte[], CodingRequestResult> result = taskAwaiter3.GetResult();
					if (result.Item2 != CodingRequestResult.Success)
					{
						codingRequestResult = result.Item2;
						goto IL_027A;
					}
					originalData = result.Item1;
					IL_0103:
					if (skipIfTheSameData && delphiInjectorCoding.MakeChangesToInitialData && originalData != null && ArrayHelpers.ArrayEquals<byte>(BitHelpers.ConvertHexToBytesX(checked_value), originalData))
					{
						Dictionary<string, string> dictionary = new Dictionary<string, string>();
						dictionary.Add("Protocol", delphiInjectorCoding.Protocol);
						dictionary.Add("ExtendedAddress", delphiInjectorCoding.ExtendedAddress);
						dictionary.Add("TesterAddress", delphiInjectorCoding.TesterAddress);
						CodingLogItem.RecordToLog(delphiInjectorCoding.Name, UserFriendlyValue, CodingLogItem.CodingTypes.CustomizableCodingTemplate, delphiInjectorCoding.WriteModeAndAddress, BitHelpers.ByteArrayToHexString(originalData), checked_value, password, delphiInjectorCoding.RequestHeader, delphiInjectorCoding.ResponseHeader, delphiInjectorCoding.OpenSessionCommand, delphiInjectorCoding.PreWriteCommands, delphiInjectorCoding.PostWriteCommands, delphiInjectorCoding.ATST, dictionary, delphiInjectorCoding.PreReadCommands);
						codingRequestResult = CodingRequestResult.Success;
						goto IL_027A;
					}
					taskAwaiter = delphiInjectorCoding.WriteDataToECU(password, UserFriendlyValue, progress, originalData, checked_value).GetAwaiter();
					if (!taskAwaiter.IsCompleted)
					{
						num2 = 1;
						TaskAwaiter<CodingRequestResult> taskAwaiter2 = taskAwaiter;
						this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<CodingRequestResult>, DelphiInjectorCoding.<Execute>d__3>(ref taskAwaiter, ref this);
						return;
					}
					IL_0250:
					codingRequestResult = taskAwaiter.GetResult();
				}
				catch (Exception ex)
				{
					num2 = -2;
					checked_value = null;
					this.<>t__builder.SetException(ex);
					return;
				}
				IL_027A:
				num2 = -2;
				checked_value = null;
				this.<>t__builder.SetResult(codingRequestResult);
			}

			// Token: 0x06005B4B RID: 23371 RVA: 0x004386A4 File Offset: 0x004368A4
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x0400398E RID: 14734
			public int <>1__state;

			// Token: 0x0400398F RID: 14735
			public AsyncTaskMethodBuilder<CodingRequestResult> <>t__builder;

			// Token: 0x04003990 RID: 14736
			public string value;

			// Token: 0x04003991 RID: 14737
			public DelphiInjectorCoding <>4__this;

			// Token: 0x04003992 RID: 14738
			public byte[] originalData;

			// Token: 0x04003993 RID: 14739
			public IProgress<string> progress;

			// Token: 0x04003994 RID: 14740
			public bool skipIfTheSameData;

			// Token: 0x04003995 RID: 14741
			public string UserFriendlyValue;

			// Token: 0x04003996 RID: 14742
			public string password;

			// Token: 0x04003997 RID: 14743
			private string <checked_value>5__2;

			// Token: 0x04003998 RID: 14744
			private TaskAwaiter<Tuple<byte[], CodingRequestResult>> <>u__1;

			// Token: 0x04003999 RID: 14745
			private TaskAwaiter<CodingRequestResult> <>u__2;
		}

		// Token: 0x02000BDA RID: 3034
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <UpdateCurrentState>d__4 : IAsyncStateMachine
		{
			// Token: 0x06005B4C RID: 23372 RVA: 0x004386B4 File Offset: 0x004368B4
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				DelphiInjectorCoding delphiInjectorCoding = this;
				CodingRequestResult codingRequestResult;
				try
				{
					TaskAwaiter<Tuple<byte[], CodingRequestResult>> taskAwaiter;
					if (num != 0)
					{
						if (string.IsNullOrEmpty(delphiInjectorCoding.ReadModeAndAddress))
						{
							codingRequestResult = CodingRequestResult.Success;
							goto IL_0107;
						}
						taskAwaiter = delphiInjectorCoding.GetCurrentStateRawData(password).GetAwaiter();
						if (!taskAwaiter.IsCompleted)
						{
							num2 = 0;
							TaskAwaiter<Tuple<byte[], CodingRequestResult>> taskAwaiter2 = taskAwaiter;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<Tuple<byte[], CodingRequestResult>>, DelphiInjectorCoding.<UpdateCurrentState>d__4>(ref taskAwaiter, ref this);
							return;
						}
					}
					else
					{
						TaskAwaiter<Tuple<byte[], CodingRequestResult>> taskAwaiter2;
						taskAwaiter = taskAwaiter2;
						taskAwaiter2 = default(TaskAwaiter<Tuple<byte[], CodingRequestResult>>);
						num2 = -1;
					}
					Tuple<byte[], CodingRequestResult> result = taskAwaiter.GetResult();
					CodingRequestResult item = result.Item2;
					byte[] item2 = result.Item1;
					if (item != CodingRequestResult.Success)
					{
						delphiInjectorCoding.CurrentState = CustomizableCodingTemplate.CodingRequestResultToString(item);
						codingRequestResult = item;
					}
					else if (item2.Length == 21)
					{
						byte[] array = new byte[item2.Length - 1];
						Array.Copy(item2, 1, array, 0, array.Length);
						delphiInjectorCoding.CurrentState = DelphiC3iCodeConverter.BytesToCode(array);
						codingRequestResult = CodingRequestResult.Success;
					}
					else
					{
						delphiInjectorCoding.CurrentState = MQBAdaptationTemplate.UNKNOWN_TITLE;
						codingRequestResult = CodingRequestResult.InitialDataIncorrect;
					}
				}
				catch (Exception ex)
				{
					num2 = -2;
					this.<>t__builder.SetException(ex);
					return;
				}
				IL_0107:
				num2 = -2;
				this.<>t__builder.SetResult(codingRequestResult);
			}

			// Token: 0x06005B4D RID: 23373 RVA: 0x004387EC File Offset: 0x004369EC
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x0400399A RID: 14746
			public int <>1__state;

			// Token: 0x0400399B RID: 14747
			public AsyncTaskMethodBuilder<CodingRequestResult> <>t__builder;

			// Token: 0x0400399C RID: 14748
			public DelphiInjectorCoding <>4__this;

			// Token: 0x0400399D RID: 14749
			public string password;

			// Token: 0x0400399E RID: 14750
			private TaskAwaiter<Tuple<byte[], CodingRequestResult>> <>u__1;
		}
	}
}
