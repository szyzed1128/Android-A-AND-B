using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace CarScannerXamarinForms.Common
{
	// Token: 0x020007F4 RID: 2036
	public static class OnlineTime
	{
		// Token: 0x06004735 RID: 18229 RVA: 0x0036D4E0 File Offset: 0x0036B6E0
		public static async Task<DateTime> GetCurrentTimeUTC()
		{
			Func<string, string, bool> func = delegate(string uri, string verifyResult)
			{
				long num2;
				return long.TryParse(verifyResult, out num2);
			};
			string text = await HttpDownloader.Get(new string[] { "https://currentmillis.com/time/seconds-since-unix-epoch.php", "https://www.carscanner.info/time.php" }, 10, func, true);
			DateTime dateTime;
			long num;
			if (text == null || text == "")
			{
				dateTime = DateTimeNowHelper.NowSafe.ToUniversalTime();
			}
			else if (long.TryParse(text, out num))
			{
				DateTime utcDateTime = DateTimeOffset.FromUnixTimeSeconds(num).UtcDateTime;
				OnlineTime._LastUtcTime = new DateTime?(utcDateTime);
				dateTime = utcDateTime;
			}
			else
			{
				dateTime = DateTimeNowHelper.NowSafe.ToUniversalTime();
			}
			return dateTime;
		}

		// Token: 0x17001620 RID: 5664
		// (get) Token: 0x06004736 RID: 18230 RVA: 0x0036D51B File Offset: 0x0036B71B
		public static DateTime LastUtcTime
		{
			get
			{
				if (OnlineTime._LastUtcTime == null)
				{
					return DateTimeNowHelper.NowSafe;
				}
				return OnlineTime._LastUtcTime.Value;
			}
		}

		// Token: 0x04002985 RID: 10629
		private static DateTime? _LastUtcTime;

		// Token: 0x020007F5 RID: 2037
		[CompilerGenerated]
		[Serializable]
		private sealed class <>c
		{
			// Token: 0x06004737 RID: 18231 RVA: 0x0036D539 File Offset: 0x0036B739
			// Note: this type is marked as 'beforefieldinit'.
			static <>c()
			{
			}

			// Token: 0x06004738 RID: 18232 RVA: 0x00002050 File Offset: 0x00000250
			public <>c()
			{
			}

			// Token: 0x06004739 RID: 18233 RVA: 0x0036D548 File Offset: 0x0036B748
			internal bool <GetCurrentTimeUTC>b__0_0(string uri, string verifyResult)
			{
				long num;
				return long.TryParse(verifyResult, out num);
			}

			// Token: 0x04002986 RID: 10630
			public static readonly OnlineTime.<>c <>9 = new OnlineTime.<>c();

			// Token: 0x04002987 RID: 10631
			public static Func<string, string, bool> <>9__0_0;
		}

		// Token: 0x020007F6 RID: 2038
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <GetCurrentTimeUTC>d__0 : IAsyncStateMachine
		{
			// Token: 0x0600473A RID: 18234 RVA: 0x0036D564 File Offset: 0x0036B764
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				DateTime dateTime;
				try
				{
					TaskAwaiter<string> taskAwaiter;
					if (num != 0)
					{
						Func<string, string, bool> func = delegate(string uri, string verifyResult)
						{
							long num4;
							return long.TryParse(verifyResult, out num4);
						};
						taskAwaiter = HttpDownloader.Get(new string[] { "https://currentmillis.com/time/seconds-since-unix-epoch.php", "https://www.carscanner.info/time.php" }, 10, func, true).GetAwaiter();
						if (!taskAwaiter.IsCompleted)
						{
							num2 = 0;
							TaskAwaiter<string> taskAwaiter2 = taskAwaiter;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<string>, OnlineTime.<GetCurrentTimeUTC>d__0>(ref taskAwaiter, ref this);
							return;
						}
					}
					else
					{
						TaskAwaiter<string> taskAwaiter2;
						taskAwaiter = taskAwaiter2;
						taskAwaiter2 = default(TaskAwaiter<string>);
						num2 = -1;
					}
					string result = taskAwaiter.GetResult();
					long num3;
					if (result == null || result == "")
					{
						dateTime = DateTimeNowHelper.NowSafe.ToUniversalTime();
					}
					else if (long.TryParse(result, out num3))
					{
						DateTime utcDateTime = DateTimeOffset.FromUnixTimeSeconds(num3).UtcDateTime;
						OnlineTime._LastUtcTime = new DateTime?(utcDateTime);
						dateTime = utcDateTime;
					}
					else
					{
						dateTime = DateTimeNowHelper.NowSafe.ToUniversalTime();
					}
				}
				catch (Exception ex)
				{
					num2 = -2;
					this.<>t__builder.SetException(ex);
					return;
				}
				num2 = -2;
				this.<>t__builder.SetResult(dateTime);
			}

			// Token: 0x0600473B RID: 18235 RVA: 0x0036D6B0 File Offset: 0x0036B8B0
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x04002988 RID: 10632
			public int <>1__state;

			// Token: 0x04002989 RID: 10633
			public AsyncTaskMethodBuilder<DateTime> <>t__builder;

			// Token: 0x0400298A RID: 10634
			private TaskAwaiter<string> <>u__1;
		}
	}
}
