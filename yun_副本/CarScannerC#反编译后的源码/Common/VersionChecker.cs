using System;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using CarScannerXamarinForms.PlatformAdapters;
using CarScannerXamarinForms.Settings;

namespace CarScannerXamarinForms.Common
{
	// Token: 0x0200080F RID: 2063
	internal class VersionChecker
	{
		// Token: 0x060047BD RID: 18365 RVA: 0x0036F3D8 File Offset: 0x0036D5D8
		public static async Task CheckVersion()
		{
			if (!(new TimeSpan(DateTimeNowHelper.NowSafe.Ticks - SharedSettings.Current.LastTimeNewVersionChecked) < TimeSpan.FromHours(1.0)))
			{
				try
				{
					string text = "";
					switch (PlatformHelper.AppMarket)
					{
					case Markets.AppStore:
						text = "mv_ios.txt";
						break;
					case Markets.GooglePlay:
						text = "mv_droid.txt";
						break;
					case Markets.HMS:
						text = "mv_droid_hms.txt";
						break;
					case Markets.Rustore:
						text = "mv_droid_rustore.txt";
						break;
					case Markets.RUS:
						text = "mv_droid_rus.txt";
						break;
					case Markets.Sideload:
						text = "mv_droid_sl.txt";
						break;
					}
					string text2 = await HttpDownloader.Get(new string[]
					{
						"https://node2.carscanner.info/mv/" + text,
						"https://node3.carscanner.info/mv/" + text,
						"https://node4.carscanner.info/mv/" + text
					}, 10, null, true);
					if (text2 != null && text2.Length > 0)
					{
						string[] array = (from x in text2.Split(new char[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries)
							where x != ""
							select x).ToArray<string>();
						if (array.Length >= 4)
						{
							SharedSettings.Current.MinVersion = array[2];
							SharedSettings.Current.AvailableVersion = array[3];
							SharedSettings.Current.LastTimeNewVersionChecked = DateTimeNowHelper.NowSafe.Ticks;
						}
					}
				}
				catch (Exception)
				{
				}
			}
		}

		// Token: 0x1700163A RID: 5690
		// (get) Token: 0x060047BE RID: 18366 RVA: 0x0036F414 File Offset: 0x0036D614
		public static bool IsValidVersion
		{
			get
			{
				return (SharedSettings.Current.BannedVersions == null || !SharedSettings.Current.BannedVersions.Contains(App.Version)) && (string.IsNullOrEmpty(SharedSettings.Current.MinVersion) || SharedSettings.Current.MinVersion == App.Version || VersionChecker.IsVersionNewer(App.Version, SharedSettings.Current.MinVersion));
			}
		}

		// Token: 0x1700163B RID: 5691
		// (get) Token: 0x060047BF RID: 18367 RVA: 0x0036F48C File Offset: 0x0036D68C
		public static bool ShouldShowNewVersionAvailable
		{
			get
			{
				return !(SharedSettings.Current.AvailableVersion == App.Version) && !string.IsNullOrEmpty(SharedSettings.Current.AvailableVersion) && VersionChecker.IsVersionNewer(SharedSettings.Current.AvailableVersion, App.Version);
			}
		}

		// Token: 0x060047C0 RID: 18368 RVA: 0x0036F4E0 File Offset: 0x0036D6E0
		internal static bool IsVersionNewer(string new_ver, string old_ver)
		{
			if (string.IsNullOrEmpty(new_ver))
			{
				return false;
			}
			if (string.IsNullOrEmpty(old_ver))
			{
				return false;
			}
			try
			{
				string[] array = new_ver.Split(new char[] { '.' }, StringSplitOptions.RemoveEmptyEntries);
				string[] array2 = old_ver.Split(new char[] { '.' }, StringSplitOptions.RemoveEmptyEntries);
				int num = int.Parse(array[0]);
				int num2 = int.Parse(array2[0]);
				if (num > num2)
				{
					return true;
				}
				if (num < num2)
				{
					return false;
				}
				int num3 = int.Parse(array[1]);
				int num4 = int.Parse(array2[1]);
				if (num3 > num4)
				{
					return true;
				}
				if (num3 < num4)
				{
					return false;
				}
				int num5 = int.Parse(array[2]);
				int num6 = int.Parse(array2[2]);
				if (num5 > num6)
				{
					return true;
				}
			}
			catch (Exception)
			{
			}
			return false;
		}

		// Token: 0x060047C1 RID: 18369 RVA: 0x00002050 File Offset: 0x00000250
		public VersionChecker()
		{
		}

		// Token: 0x02000810 RID: 2064
		[CompilerGenerated]
		[Serializable]
		private sealed class <>c
		{
			// Token: 0x060047C2 RID: 18370 RVA: 0x0036F5AC File Offset: 0x0036D7AC
			// Note: this type is marked as 'beforefieldinit'.
			static <>c()
			{
			}

			// Token: 0x060047C3 RID: 18371 RVA: 0x00002050 File Offset: 0x00000250
			public <>c()
			{
			}

			// Token: 0x060047C4 RID: 18372 RVA: 0x0036F5B8 File Offset: 0x0036D7B8
			internal bool <CheckVersion>b__0_0(string x)
			{
				return x != "";
			}

			// Token: 0x040029CF RID: 10703
			public static readonly VersionChecker.<>c <>9 = new VersionChecker.<>c();

			// Token: 0x040029D0 RID: 10704
			public static Func<string, bool> <>9__0_0;
		}

		// Token: 0x02000811 RID: 2065
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <CheckVersion>d__0 : IAsyncStateMachine
		{
			// Token: 0x060047C5 RID: 18373 RVA: 0x0036F5C8 File Offset: 0x0036D7C8
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				try
				{
					if (num == 0 || !(new TimeSpan(DateTimeNowHelper.NowSafe.Ticks - SharedSettings.Current.LastTimeNewVersionChecked) < TimeSpan.FromHours(1.0)))
					{
						try
						{
							TaskAwaiter<string> taskAwaiter;
							if (num != 0)
							{
								string text = "";
								switch (PlatformHelper.AppMarket)
								{
								case Markets.AppStore:
									text = "mv_ios.txt";
									break;
								case Markets.GooglePlay:
									text = "mv_droid.txt";
									break;
								case Markets.HMS:
									text = "mv_droid_hms.txt";
									break;
								case Markets.Rustore:
									text = "mv_droid_rustore.txt";
									break;
								case Markets.RUS:
									text = "mv_droid_rus.txt";
									break;
								case Markets.Sideload:
									text = "mv_droid_sl.txt";
									break;
								}
								taskAwaiter = HttpDownloader.Get(new string[]
								{
									"https://node2.carscanner.info/mv/" + text,
									"https://node3.carscanner.info/mv/" + text,
									"https://node4.carscanner.info/mv/" + text
								}, 10, null, true).GetAwaiter();
								if (!taskAwaiter.IsCompleted)
								{
									num2 = 0;
									TaskAwaiter<string> taskAwaiter2 = taskAwaiter;
									this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<string>, VersionChecker.<CheckVersion>d__0>(ref taskAwaiter, ref this);
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
							if (result != null && result.Length > 0)
							{
								string[] array = (from x in result.Split(new char[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries)
									where x != ""
									select x).ToArray<string>();
								if (array.Length >= 4)
								{
									SharedSettings.Current.MinVersion = array[2];
									SharedSettings.Current.AvailableVersion = array[3];
									SharedSettings.Current.LastTimeNewVersionChecked = DateTimeNowHelper.NowSafe.Ticks;
								}
							}
						}
						catch (Exception)
						{
						}
					}
				}
				catch (Exception ex)
				{
					num2 = -2;
					this.<>t__builder.SetException(ex);
					return;
				}
				num2 = -2;
				this.<>t__builder.SetResult();
			}

			// Token: 0x060047C6 RID: 18374 RVA: 0x0036F800 File Offset: 0x0036DA00
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x040029D1 RID: 10705
			public int <>1__state;

			// Token: 0x040029D2 RID: 10706
			public AsyncTaskMethodBuilder <>t__builder;

			// Token: 0x040029D3 RID: 10707
			private TaskAwaiter<string> <>u__1;
		}
	}
}
