using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using CarScannerXamarinForms.Common;

namespace CarScannerXamarinForms.Coding
{
	// Token: 0x0200086B RID: 2155
	internal class CustomizableCodingContainerBackupBase : CustomizableCodingTemplate
	{
		// Token: 0x06004972 RID: 18802 RVA: 0x00378DAB File Offset: 0x00376FAB
		public CustomizableCodingContainerBackupBase()
		{
			this.Options.Add(new MQBAdaptationOption("CREATE BACKUP", ""));
		}

		// Token: 0x17001676 RID: 5750
		// (get) Token: 0x06004973 RID: 18803 RVA: 0x00002076 File Offset: 0x00000276
		// (set) Token: 0x06004974 RID: 18804 RVA: 0x00378DCD File Offset: 0x00376FCD
		public override AdaptationValueTypes ValueType
		{
			get
			{
				return AdaptationValueTypes.OptionType;
			}
			set
			{
				base.ValueType = value;
			}
		}

		// Token: 0x06004975 RID: 18805 RVA: 0x00378DD8 File Offset: 0x00376FD8
		public override async Task<CodingRequestResult> Execute(string password, string value, string UserFriendlyValue, IProgress<string> progress, byte[] originalData = null, bool skipIfTheSameData = false)
		{
			byte[] array;
			CodingRequestResult codingRequestResult;
			(await this.GetCurrentStateRawData(password)).Deconstruct(out array, out codingRequestResult);
			byte[] array2 = array;
			CodingRequestResult codingRequestResult2 = codingRequestResult;
			if (codingRequestResult2 == CodingRequestResult.Success)
			{
				Dictionary<string, string> dictionary = new Dictionary<string, string>();
				dictionary.Add("Protocol", base.Protocol);
				dictionary.Add("ExtendedAddress", base.ExtendedAddress);
				dictionary.Add("TesterAddress", base.TesterAddress);
				CodingLogItem.RecordToLog(base.Name, UserFriendlyValue, CodingLogItem.CodingTypes.CustomizableCodingTemplate, base.WriteModeAndAddress, BitHelpers.ByteArrayToHexString(array2), BitHelpers.ByteArrayToHexString(array2), password, base.RequestHeader, base.ResponseHeader, base.OpenSessionCommand, base.PreWriteCommands, base.PostWriteCommands, base.ATST, dictionary, base.PreReadCommands);
			}
			return codingRequestResult2;
		}

		// Token: 0x0200086C RID: 2156
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <Execute>d__4 : IAsyncStateMachine
		{
			// Token: 0x06004976 RID: 18806 RVA: 0x00378E2C File Offset: 0x0037702C
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				CustomizableCodingContainerBackupBase customizableCodingContainerBackupBase = this;
				CodingRequestResult codingRequestResult3;
				try
				{
					TaskAwaiter<Tuple<byte[], CodingRequestResult>> taskAwaiter;
					if (num != 0)
					{
						taskAwaiter = customizableCodingContainerBackupBase.GetCurrentStateRawData(password).GetAwaiter();
						if (!taskAwaiter.IsCompleted)
						{
							num2 = 0;
							TaskAwaiter<Tuple<byte[], CodingRequestResult>> taskAwaiter2 = taskAwaiter;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<Tuple<byte[], CodingRequestResult>>, CustomizableCodingContainerBackupBase.<Execute>d__4>(ref taskAwaiter, ref this);
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
					byte[] array;
					CodingRequestResult codingRequestResult;
					taskAwaiter.GetResult().Deconstruct(out array, out codingRequestResult);
					byte[] array2 = array;
					CodingRequestResult codingRequestResult2 = codingRequestResult;
					if (codingRequestResult2 == CodingRequestResult.Success)
					{
						Dictionary<string, string> dictionary = new Dictionary<string, string>();
						dictionary.Add("Protocol", customizableCodingContainerBackupBase.Protocol);
						dictionary.Add("ExtendedAddress", customizableCodingContainerBackupBase.ExtendedAddress);
						dictionary.Add("TesterAddress", customizableCodingContainerBackupBase.TesterAddress);
						CodingLogItem.RecordToLog(customizableCodingContainerBackupBase.Name, UserFriendlyValue, CodingLogItem.CodingTypes.CustomizableCodingTemplate, customizableCodingContainerBackupBase.WriteModeAndAddress, BitHelpers.ByteArrayToHexString(array2), BitHelpers.ByteArrayToHexString(array2), password, customizableCodingContainerBackupBase.RequestHeader, customizableCodingContainerBackupBase.ResponseHeader, customizableCodingContainerBackupBase.OpenSessionCommand, customizableCodingContainerBackupBase.PreWriteCommands, customizableCodingContainerBackupBase.PostWriteCommands, customizableCodingContainerBackupBase.ATST, dictionary, customizableCodingContainerBackupBase.PreReadCommands);
					}
					codingRequestResult3 = codingRequestResult2;
				}
				catch (Exception ex)
				{
					num2 = -2;
					this.<>t__builder.SetException(ex);
					return;
				}
				num2 = -2;
				this.<>t__builder.SetResult(codingRequestResult3);
			}

			// Token: 0x06004977 RID: 18807 RVA: 0x00378FA4 File Offset: 0x003771A4
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x04002A71 RID: 10865
			public int <>1__state;

			// Token: 0x04002A72 RID: 10866
			public AsyncTaskMethodBuilder<CodingRequestResult> <>t__builder;

			// Token: 0x04002A73 RID: 10867
			public CustomizableCodingContainerBackupBase <>4__this;

			// Token: 0x04002A74 RID: 10868
			public string password;

			// Token: 0x04002A75 RID: 10869
			public string UserFriendlyValue;

			// Token: 0x04002A76 RID: 10870
			private TaskAwaiter<Tuple<byte[], CodingRequestResult>> <>u__1;
		}
	}
}
