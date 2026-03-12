using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using CarScannerXamarinForms.OBD2;

namespace CarScannerXamarinForms.Coding.DB.Renault
{
	// Token: 0x02000A08 RID: 2568
	internal abstract class RenaultService31Base : CustomizableCodingTemplate, IServiceProcedure, ICodingContainer, INotifyPropertyChanged
	{
		// Token: 0x06005208 RID: 21000 RVA: 0x003F67BC File Offset: 0x003F49BC
		public override async Task<CodingRequestResult> UpdateCurrentState(string password, IProgress<string> progress = null)
		{
			this.BuildDefaultBeforeAndAfterCommands();
			base.CurrentState = "";
			this.OnStatusUpdateRequested();
			return CodingRequestResult.Success;
		}

		// Token: 0x170017BE RID: 6078
		// (get) Token: 0x06005209 RID: 21001 RVA: 0x000A8D6F File Offset: 0x000A6F6F
		// (set) Token: 0x0600520A RID: 21002 RVA: 0x003F0A8F File Offset: 0x003EEC8F
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

		// Token: 0x0600520B RID: 21003 RVA: 0x000027D4 File Offset: 0x000009D4
		protected virtual void OnStatusUpdateRequested()
		{
		}

		// Token: 0x0600520C RID: 21004 RVA: 0x003F6800 File Offset: 0x003F4A00
		public override async Task<CodingRequestResult> Execute(string password, string value, string UserFriendlyValue, IProgress<string> progress, byte[] originalData = null, bool skipIfTheSameData = false)
		{
			this.BuildDefaultBeforeAndAfterCommands();
			OBDRequest obdrequest = new OBDRequest(value, base.RequestHeader, this.BeforeCommands, this.AfterCommands, false);
			App.OBDReader.ReplaceQueue(obdrequest);
			await App.OBDReader.WaitForCommandQueue();
			return CodingRequestResult.OperationInProgress;
		}

		// Token: 0x0600520D RID: 21005 RVA: 0x003F684C File Offset: 0x003F4A4C
		protected override void BuildDefaultBeforeAndAfterCommands()
		{
			if (string.IsNullOrEmpty(base.ExtendedAddress))
			{
				this.BeforeCommands = string.Concat(new string[] { "ATFCSH", base.RequestHeader, ";ATFCSD300000;ATFCSM1;ATAL;ATCRA", base.ResponseHeader, ";ATST", base.ATST });
				this.AfterCommands = "ATFCSM0;ATSTDEF;ATAR";
				return;
			}
			this.BeforeCommands = string.Concat(new string[] { "ATFCSH", base.RequestHeader, ";ATFCSD300000;ATFCSM1;ATAL;ATCRA", base.ResponseHeader, ";ATCEA", base.ExtendedAddress, ";ATTA", base.ExtendedAddress, ";ATST", base.ATST });
			this.AfterCommands = "ATFCSM0;ATSTDEF;ATAR;ATCEA";
		}

		// Token: 0x0600520E RID: 21006 RVA: 0x003F6929 File Offset: 0x003F4B29
		protected RenaultService31Base()
		{
		}

		// Token: 0x02000A09 RID: 2569
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <Execute>d__5 : IAsyncStateMachine
		{
			// Token: 0x0600520F RID: 21007 RVA: 0x003F6934 File Offset: 0x003F4B34
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				RenaultService31Base renaultService31Base = this;
				CodingRequestResult codingRequestResult;
				try
				{
					TaskAwaiter taskAwaiter;
					if (num != 0)
					{
						renaultService31Base.BuildDefaultBeforeAndAfterCommands();
						OBDRequest obdrequest = new OBDRequest(value, renaultService31Base.RequestHeader, renaultService31Base.BeforeCommands, renaultService31Base.AfterCommands, false);
						App.OBDReader.ReplaceQueue(obdrequest);
						taskAwaiter = App.OBDReader.WaitForCommandQueue().GetAwaiter();
						if (!taskAwaiter.IsCompleted)
						{
							num2 = 0;
							TaskAwaiter taskAwaiter2 = taskAwaiter;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, RenaultService31Base.<Execute>d__5>(ref taskAwaiter, ref this);
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
					codingRequestResult = CodingRequestResult.OperationInProgress;
				}
				catch (Exception ex)
				{
					num2 = -2;
					this.<>t__builder.SetException(ex);
					return;
				}
				num2 = -2;
				this.<>t__builder.SetResult(codingRequestResult);
			}

			// Token: 0x06005210 RID: 21008 RVA: 0x003F6A24 File Offset: 0x003F4C24
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x040031F2 RID: 12786
			public int <>1__state;

			// Token: 0x040031F3 RID: 12787
			public AsyncTaskMethodBuilder<CodingRequestResult> <>t__builder;

			// Token: 0x040031F4 RID: 12788
			public RenaultService31Base <>4__this;

			// Token: 0x040031F5 RID: 12789
			public string value;

			// Token: 0x040031F6 RID: 12790
			private TaskAwaiter <>u__1;
		}

		// Token: 0x02000A0A RID: 2570
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <UpdateCurrentState>d__0 : IAsyncStateMachine
		{
			// Token: 0x06005211 RID: 21009 RVA: 0x003F6A34 File Offset: 0x003F4C34
			void IAsyncStateMachine.MoveNext()
			{
				RenaultService31Base renaultService31Base = this;
				CodingRequestResult codingRequestResult;
				try
				{
					renaultService31Base.BuildDefaultBeforeAndAfterCommands();
					renaultService31Base.CurrentState = "";
					renaultService31Base.OnStatusUpdateRequested();
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

			// Token: 0x06005212 RID: 21010 RVA: 0x003F6AA0 File Offset: 0x003F4CA0
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x040031F7 RID: 12791
			public int <>1__state;

			// Token: 0x040031F8 RID: 12792
			public AsyncTaskMethodBuilder<CodingRequestResult> <>t__builder;

			// Token: 0x040031F9 RID: 12793
			public RenaultService31Base <>4__this;
		}
	}
}
