using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using CarScannerXamarinForms.Common;
using CarScannerXamarinForms.OBD2;
using CarScannerXamarinForms.OBD2.PIDS;
using CarScannerXamarinForms.Settings;
using CarScannerXamarinForms.ViewModels;

namespace CarScannerXamarinForms.ProfilesV2
{
	// Token: 0x020002C9 RID: 713
	internal static class ProfileSupportedPidsTester
	{
		// Token: 0x17001103 RID: 4355
		// (get) Token: 0x0600228C RID: 8844 RVA: 0x001AB672 File Offset: 0x001A9872
		// (set) Token: 0x0600228D RID: 8845 RVA: 0x001AB679 File Offset: 0x001A9879
		public static bool IsTesting
		{
			[CompilerGenerated]
			get
			{
				return ProfileSupportedPidsTester.<IsTesting>k__BackingField;
			}
			[CompilerGenerated]
			private set
			{
				ProfileSupportedPidsTester.<IsTesting>k__BackingField = value;
			}
		}

		// Token: 0x0600228E RID: 8846 RVA: 0x001AB684 File Offset: 0x001A9884
		public static async Task<string> PerformTest(IProgress<int> progress, CancellationToken cancellationToken)
		{
			ProfileSupportedPidsTester.<>c__DisplayClass4_0 CS$<>8__locals1 = new ProfileSupportedPidsTester.<>c__DisplayClass4_0();
			CS$<>8__locals1.cancellationToken = cancellationToken;
			CS$<>8__locals1.progress = progress;
			ProfileSupportedPidsTester.IsTesting = true;
			SharedSettings.Current.ShouldCheckProfilePIDs = false;
			CS$<>8__locals1.lock_object = new object();
			if (!CustomPIDViewModel.DisabledProfile.Loaded)
			{
				CustomPIDViewModel.DisabledProfile.Load();
			}
			if (!CustomPIDViewModel.CurrentProfile.Loaded)
			{
				CustomPIDViewModel.CurrentProfile.Load();
			}
			CS$<>8__locals1.semaphore = new SemaphoreSlim(0, 1);
			List<CustomPID> pids_for_test = CustomPIDViewModel.CurrentProfile.PidCollection.Concat(CustomPIDViewModel.DisabledProfile.PidCollection).ToList<CustomPID>();
			string text;
			if (pids_for_test.Count == 0)
			{
				ProfileSupportedPidsTester.IsTesting = false;
				text = "";
			}
			else
			{
				CS$<>8__locals1.max_disconnect_counter = 3;
				List<OBDRequest> requests = new List<OBDRequest>(pids_for_test.Count);
				App.OBDReader.NO_DATA_Counter = 0;
				CS$<>8__locals1.tooMuchErrorsDetected = false;
				CS$<>8__locals1.supported_pids = new List<CustomPID>();
				new List<CustomPID>();
				List<CustomPID> action_pids = pids_for_test.Where((CustomPID x) => x.IsAction).ToList<CustomPID>();
				foreach (CustomPID customPID in action_pids)
				{
					pids_for_test.Remove(customPID);
				}
				CS$<>8__locals1.disconnectCounter = 0;
				CurrentStatusChangedEvent obdReaderStatusChanged = delegate(OBDDataReaderStatus NewStatus)
				{
					if (CS$<>8__locals1.cancellationToken.IsCancellationRequested)
					{
						try
						{
							if (CS$<>8__locals1.semaphore.CurrentCount == 0)
							{
								CS$<>8__locals1.semaphore.Release();
							}
						}
						catch
						{
						}
						return;
					}
					if (NewStatus == OBDDataReaderStatus.ConnectingToELM)
					{
						int disconnectCounter = CS$<>8__locals1.disconnectCounter;
						CS$<>8__locals1.disconnectCounter = disconnectCounter + 1;
						if (CS$<>8__locals1.disconnectCounter <= CS$<>8__locals1.max_disconnect_counter)
						{
							return;
						}
						try
						{
							if (CS$<>8__locals1.semaphore.CurrentCount == 0)
							{
								CS$<>8__locals1.semaphore.Release();
							}
							return;
						}
						catch
						{
							return;
						}
					}
					if (NewStatus == OBDDataReaderStatus.Disconnected)
					{
						try
						{
							if (CS$<>8__locals1.semaphore.CurrentCount == 0)
							{
								CS$<>8__locals1.semaphore.Release();
							}
						}
						catch
						{
						}
					}
				};
				App.OBDReader.StatusChanged -= obdReaderStatusChanged;
				App.OBDReader.StatusChanged += obdReaderStatusChanged;
				EventHandler<PID> pidValueChanged = delegate(object pid_sender, PID pid_e)
				{
					object lock_object = CS$<>8__locals1.lock_object;
					lock (lock_object)
					{
						if (pid_e != null)
						{
							CS$<>8__locals1.supported_pids.Add((CustomPID)pid_e);
						}
					}
				};
				LiveDataPIDModel liveDataPIDModel = new LiveDataPIDModel();
				foreach (CustomPID customPID2 in pids_for_test)
				{
					customPID2.ValueChanged -= pidValueChanged;
					customPID2.ValueChanged += pidValueChanged;
					liveDataPIDModel.SelectedPID = customPID2;
					liveDataPIDModel.GetRequests(requests, null, "");
				}
				liveDataPIDModel.SelectedPID = PID.Empty;
				CS$<>8__locals1.total = requests.Count;
				CS$<>8__locals1.current = 0;
				CS$<>8__locals1.responseReceivedCounter = 0;
				ResponseReceivedDelegate responseReceivedDelegate = delegate(OBDRequest req, string data)
				{
					int num = CS$<>8__locals1.responseReceivedCounter;
					CS$<>8__locals1.responseReceivedCounter = num + 1;
					App.OBDReader.NO_DATA_Counter = 0;
					if (CS$<>8__locals1.cancellationToken.IsCancellationRequested)
					{
						App.OBDReader.ReplaceQueue(new OBDRequest[0]);
					}
					num = CS$<>8__locals1.current;
					CS$<>8__locals1.current = num + 1;
					Action action;
					if ((action = CS$<>8__locals1.<>9__6) == null)
					{
						action = (CS$<>8__locals1.<>9__6 = delegate
						{
							double num2 = (double)CS$<>8__locals1.current * 100.0 / (double)CS$<>8__locals1.total;
							IProgress<int> progress2 = CS$<>8__locals1.progress;
							if (progress2 == null)
							{
								return;
							}
							progress2.Report((int)num2);
						});
					}
					MainThreadHelper.InvokeOnMainThread(action);
					if (App.OBDReader.ELMStatus.NoFinishCharacterErrors > 10 || App.OBDReader.ELMStatus.WrongNewLineCharactersCounter > 10 || CS$<>8__locals1.disconnectCounter > 3)
					{
						CS$<>8__locals1.tooMuchErrorsDetected = true;
						App.OBDReader.ReplaceQueue(new OBDRequest[0]);
					}
					foreach (string text2 in App.OBDReader.ELMStatus.ATCommandsNotSupported)
					{
						if (text2.StartsWith("ATFC") || text2.StartsWith("ATSH") || text2.StartsWith("ATSP") || text2.StartsWith("ATCP"))
						{
							CS$<>8__locals1.tooMuchErrorsDetected = true;
							App.OBDReader.ReplaceQueue(new OBDRequest[0]);
							App.OBDReader.BadELM = true;
						}
					}
				};
				foreach (OBDRequest obdrequest in requests)
				{
					obdrequest.Repeat = false;
					obdrequest.ResponseReceived -= responseReceivedDelegate;
					obdrequest.ResponseReceived += responseReceivedDelegate;
				}
				App.OBDReader.ReplaceQueue(requests);
				Task.Run(delegate
				{
					ProfileSupportedPidsTester.<>c__DisplayClass4_0.<<PerformTest>b__4>d <<PerformTest>b__4>d;
					<<PerformTest>b__4>d.<>t__builder = AsyncTaskMethodBuilder.Create();
					<<PerformTest>b__4>d.<>4__this = CS$<>8__locals1;
					<<PerformTest>b__4>d.<>1__state = -1;
					<<PerformTest>b__4>d.<>t__builder.Start<ProfileSupportedPidsTester.<>c__DisplayClass4_0.<<PerformTest>b__4>d>(ref <<PerformTest>b__4>d);
					return <<PerformTest>b__4>d.<>t__builder.Task;
				});
				await CS$<>8__locals1.semaphore.WaitAsync();
				await Task.Delay(1000);
				if (CS$<>8__locals1.responseReceivedCounter + CS$<>8__locals1.disconnectCounter < requests.Count)
				{
					ProfileSupportedPidsTester.IsTesting = false;
					text = "";
				}
				else
				{
					App.OBDReader.StatusChanged -= obdReaderStatusChanged;
					foreach (CustomPID customPID3 in pids_for_test)
					{
						customPID3.ValueChanged -= pidValueChanged;
					}
					foreach (OBDRequest obdrequest2 in requests)
					{
						obdrequest2.ResponseReceived -= responseReceivedDelegate;
					}
					int count = App.OBDReader.ELMStatus.ATCommandsNotSupported.Count;
					if (CS$<>8__locals1.tooMuchErrorsDetected)
					{
						ProfileSupportedPidsTester.IsTesting = false;
						text = Translate.GetString("profile_PidDetectionInterruptedDueToBadELM");
					}
					else if (CS$<>8__locals1.cancellationToken.IsCancellationRequested)
					{
						ProfileSupportedPidsTester.IsTesting = false;
						text = "";
					}
					else if (CS$<>8__locals1.supported_pids.Count == 0)
					{
						ProfileSupportedPidsTester.IsTesting = false;
						text = Translate.GetString("profile_PidDetectionZeroPidsSupported");
					}
					else
					{
						CustomPIDViewModel.CurrentProfile.PidCollection.Clear();
						foreach (CustomPID customPID4 in CS$<>8__locals1.supported_pids)
						{
							CustomPIDViewModel.CurrentProfile.PidCollection.Add(customPID4);
							if (!LiveDataPIDModel._PIDCollection.Contains(customPID4))
							{
								LiveDataPIDModel._PIDCollection.Add(customPID4);
							}
						}
						foreach (CustomPID customPID5 in action_pids)
						{
							CustomPIDViewModel.CurrentProfile.PidCollection.Add(customPID5);
						}
						CustomPIDViewModel.CurrentProfile.Save();
						List<CustomPID> list = (from x in pids_for_test.Except(CS$<>8__locals1.supported_pids)
							where x != null
							select x).ToList<CustomPID>();
						CustomPIDViewModel.DisabledProfile.PidCollection.Clear();
						foreach (CustomPID customPID6 in list)
						{
							CustomPIDViewModel.DisabledProfile.PidCollection.Add(customPID6);
							LiveDataPIDModel._PIDCollection.Remove(customPID6);
						}
						CustomPIDViewModel.DisabledProfile.Save();
						ProfileSupportedPidsTester.IsTesting = false;
						text = "";
					}
				}
			}
			return text;
		}

		// Token: 0x04001089 RID: 4233
		[CompilerGenerated]
		private static bool <IsTesting>k__BackingField;

		// Token: 0x020002CA RID: 714
		[CompilerGenerated]
		[Serializable]
		private sealed class <>c
		{
			// Token: 0x0600228F RID: 8847 RVA: 0x001AB6CF File Offset: 0x001A98CF
			// Note: this type is marked as 'beforefieldinit'.
			static <>c()
			{
			}

			// Token: 0x06002290 RID: 8848 RVA: 0x00002050 File Offset: 0x00000250
			public <>c()
			{
			}

			// Token: 0x06002291 RID: 8849 RVA: 0x001AB6DB File Offset: 0x001A98DB
			internal bool <PerformTest>b__4_0(CustomPID x)
			{
				return x.IsAction;
			}

			// Token: 0x06002292 RID: 8850 RVA: 0x001AB6E3 File Offset: 0x001A98E3
			internal bool <PerformTest>b__4_5(CustomPID x)
			{
				return x != null;
			}

			// Token: 0x0400108A RID: 4234
			public static readonly ProfileSupportedPidsTester.<>c <>9 = new ProfileSupportedPidsTester.<>c();

			// Token: 0x0400108B RID: 4235
			public static Func<CustomPID, bool> <>9__4_0;

			// Token: 0x0400108C RID: 4236
			public static Func<CustomPID, bool> <>9__4_5;
		}

		// Token: 0x020002CB RID: 715
		[CompilerGenerated]
		private sealed class <>c__DisplayClass4_0
		{
			// Token: 0x06002293 RID: 8851 RVA: 0x00002050 File Offset: 0x00000250
			public <>c__DisplayClass4_0()
			{
			}

			// Token: 0x06002294 RID: 8852 RVA: 0x001AB6EC File Offset: 0x001A98EC
			internal void <PerformTest>b__1(OBDDataReaderStatus NewStatus)
			{
				if (this.cancellationToken.IsCancellationRequested)
				{
					try
					{
						if (this.semaphore.CurrentCount == 0)
						{
							this.semaphore.Release();
						}
					}
					catch
					{
					}
					return;
				}
				if (NewStatus == OBDDataReaderStatus.ConnectingToELM)
				{
					int num = this.disconnectCounter;
					this.disconnectCounter = num + 1;
					if (this.disconnectCounter <= this.max_disconnect_counter)
					{
						return;
					}
					try
					{
						if (this.semaphore.CurrentCount == 0)
						{
							this.semaphore.Release();
						}
						return;
					}
					catch
					{
						return;
					}
				}
				if (NewStatus == OBDDataReaderStatus.Disconnected)
				{
					try
					{
						if (this.semaphore.CurrentCount == 0)
						{
							this.semaphore.Release();
						}
					}
					catch
					{
					}
				}
			}

			// Token: 0x06002295 RID: 8853 RVA: 0x001AB7B0 File Offset: 0x001A99B0
			internal void <PerformTest>b__2(object pid_sender, PID pid_e)
			{
				object obj = this.lock_object;
				lock (obj)
				{
					if (pid_e != null)
					{
						this.supported_pids.Add((CustomPID)pid_e);
					}
				}
			}

			// Token: 0x06002296 RID: 8854 RVA: 0x001AB800 File Offset: 0x001A9A00
			internal void <PerformTest>b__3(OBDRequest req, string data)
			{
				int num = this.responseReceivedCounter;
				this.responseReceivedCounter = num + 1;
				App.OBDReader.NO_DATA_Counter = 0;
				if (this.cancellationToken.IsCancellationRequested)
				{
					App.OBDReader.ReplaceQueue(new OBDRequest[0]);
				}
				num = this.current;
				this.current = num + 1;
				Action action;
				if ((action = this.<>9__6) == null)
				{
					action = (this.<>9__6 = delegate
					{
						double num2 = (double)this.current * 100.0 / (double)this.total;
						IProgress<int> progress = this.progress;
						if (progress == null)
						{
							return;
						}
						progress.Report((int)num2);
					});
				}
				MainThreadHelper.InvokeOnMainThread(action);
				if (App.OBDReader.ELMStatus.NoFinishCharacterErrors > 10 || App.OBDReader.ELMStatus.WrongNewLineCharactersCounter > 10 || this.disconnectCounter > 3)
				{
					this.tooMuchErrorsDetected = true;
					App.OBDReader.ReplaceQueue(new OBDRequest[0]);
				}
				foreach (string text in App.OBDReader.ELMStatus.ATCommandsNotSupported)
				{
					if (text.StartsWith("ATFC") || text.StartsWith("ATSH") || text.StartsWith("ATSP") || text.StartsWith("ATCP"))
					{
						this.tooMuchErrorsDetected = true;
						App.OBDReader.ReplaceQueue(new OBDRequest[0]);
						App.OBDReader.BadELM = true;
					}
				}
			}

			// Token: 0x06002297 RID: 8855 RVA: 0x001AB960 File Offset: 0x001A9B60
			internal void <PerformTest>b__6()
			{
				double num = (double)this.current * 100.0 / (double)this.total;
				IProgress<int> progress = this.progress;
				if (progress == null)
				{
					return;
				}
				progress.Report((int)num);
			}

			// Token: 0x06002298 RID: 8856 RVA: 0x001AB99C File Offset: 0x001A9B9C
			internal async Task <PerformTest>b__4()
			{
				await App.OBDReader.WaitForCommandQueue();
				await Task.Delay(1000);
				try
				{
					if (this.semaphore.CurrentCount == 0)
					{
						this.semaphore.Release();
					}
				}
				catch
				{
				}
			}

			// Token: 0x0400108D RID: 4237
			public CancellationToken cancellationToken;

			// Token: 0x0400108E RID: 4238
			public SemaphoreSlim semaphore;

			// Token: 0x0400108F RID: 4239
			public int disconnectCounter;

			// Token: 0x04001090 RID: 4240
			public int max_disconnect_counter;

			// Token: 0x04001091 RID: 4241
			public object lock_object;

			// Token: 0x04001092 RID: 4242
			public List<CustomPID> supported_pids;

			// Token: 0x04001093 RID: 4243
			public int responseReceivedCounter;

			// Token: 0x04001094 RID: 4244
			public int current;

			// Token: 0x04001095 RID: 4245
			public int total;

			// Token: 0x04001096 RID: 4246
			public IProgress<int> progress;

			// Token: 0x04001097 RID: 4247
			public bool tooMuchErrorsDetected;

			// Token: 0x04001098 RID: 4248
			public Action <>9__6;

			// Token: 0x020002CC RID: 716
			[StructLayout(LayoutKind.Auto)]
			private struct <<PerformTest>b__4>d : IAsyncStateMachine
			{
				// Token: 0x06002299 RID: 8857 RVA: 0x001AB9E0 File Offset: 0x001A9BE0
				void IAsyncStateMachine.MoveNext()
				{
					int num2;
					int num = num2;
					ProfileSupportedPidsTester.<>c__DisplayClass4_0 CS$<>8__locals1 = this;
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
								num2 = -1;
								goto IL_00CC;
							}
							taskAwaiter = App.OBDReader.WaitForCommandQueue().GetAwaiter();
							if (!taskAwaiter.IsCompleted)
							{
								num2 = 0;
								TaskAwaiter taskAwaiter2 = taskAwaiter;
								this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, ProfileSupportedPidsTester.<>c__DisplayClass4_0.<<PerformTest>b__4>d>(ref taskAwaiter, ref this);
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
						taskAwaiter = Task.Delay(1000).GetAwaiter();
						if (!taskAwaiter.IsCompleted)
						{
							num2 = 1;
							TaskAwaiter taskAwaiter2 = taskAwaiter;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, ProfileSupportedPidsTester.<>c__DisplayClass4_0.<<PerformTest>b__4>d>(ref taskAwaiter, ref this);
							return;
						}
						IL_00CC:
						taskAwaiter.GetResult();
						try
						{
							if (CS$<>8__locals1.semaphore.CurrentCount == 0)
							{
								CS$<>8__locals1.semaphore.Release();
							}
						}
						catch
						{
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

				// Token: 0x0600229A RID: 8858 RVA: 0x001ABB28 File Offset: 0x001A9D28
				[DebuggerHidden]
				void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
				{
					this.<>t__builder.SetStateMachine(stateMachine);
				}

				// Token: 0x04001099 RID: 4249
				public int <>1__state;

				// Token: 0x0400109A RID: 4250
				public AsyncTaskMethodBuilder <>t__builder;

				// Token: 0x0400109B RID: 4251
				public ProfileSupportedPidsTester.<>c__DisplayClass4_0 <>4__this;

				// Token: 0x0400109C RID: 4252
				private TaskAwaiter <>u__1;
			}
		}

		// Token: 0x020002CD RID: 717
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <PerformTest>d__4 : IAsyncStateMachine
		{
			// Token: 0x0600229B RID: 8859 RVA: 0x001ABB38 File Offset: 0x001A9D38
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				string text;
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
							goto IL_03FB;
						}
						CS$<>8__locals1 = new ProfileSupportedPidsTester.<>c__DisplayClass4_0();
						CS$<>8__locals1.cancellationToken = cancellationToken;
						CS$<>8__locals1.progress = progress;
						ProfileSupportedPidsTester.IsTesting = true;
						SharedSettings.Current.ShouldCheckProfilePIDs = false;
						CS$<>8__locals1.lock_object = new object();
						if (!CustomPIDViewModel.DisabledProfile.Loaded)
						{
							CustomPIDViewModel.DisabledProfile.Load();
						}
						if (!CustomPIDViewModel.CurrentProfile.Loaded)
						{
							CustomPIDViewModel.CurrentProfile.Load();
						}
						CS$<>8__locals1.semaphore = new SemaphoreSlim(0, 1);
						pids_for_test = CustomPIDViewModel.CurrentProfile.PidCollection.Concat(CustomPIDViewModel.DisabledProfile.PidCollection).ToList<CustomPID>();
						if (pids_for_test.Count == 0)
						{
							ProfileSupportedPidsTester.IsTesting = false;
							text = "";
							goto IL_0703;
						}
						CS$<>8__locals1.max_disconnect_counter = 3;
						requests = new List<OBDRequest>(pids_for_test.Count);
						App.OBDReader.NO_DATA_Counter = 0;
						CS$<>8__locals1.tooMuchErrorsDetected = false;
						CS$<>8__locals1.supported_pids = new List<CustomPID>();
						new List<CustomPID>();
						action_pids = pids_for_test.Where((CustomPID x) => x.IsAction).ToList<CustomPID>();
						List<CustomPID>.Enumerator enumerator = action_pids.GetEnumerator();
						try
						{
							while (enumerator.MoveNext())
							{
								CustomPID customPID = enumerator.Current;
								pids_for_test.Remove(customPID);
							}
						}
						finally
						{
							if (num < 0)
							{
								((IDisposable)enumerator).Dispose();
							}
						}
						CS$<>8__locals1.disconnectCounter = 0;
						obdReaderStatusChanged = delegate(OBDDataReaderStatus NewStatus)
						{
							if (CS$<>8__locals1.cancellationToken.IsCancellationRequested)
							{
								try
								{
									if (CS$<>8__locals1.semaphore.CurrentCount == 0)
									{
										CS$<>8__locals1.semaphore.Release();
									}
								}
								catch
								{
								}
								return;
							}
							if (NewStatus == OBDDataReaderStatus.ConnectingToELM)
							{
								int disconnectCounter = CS$<>8__locals1.disconnectCounter;
								CS$<>8__locals1.disconnectCounter = disconnectCounter + 1;
								if (CS$<>8__locals1.disconnectCounter <= CS$<>8__locals1.max_disconnect_counter)
								{
									return;
								}
								try
								{
									if (CS$<>8__locals1.semaphore.CurrentCount == 0)
									{
										CS$<>8__locals1.semaphore.Release();
									}
									return;
								}
								catch
								{
									return;
								}
							}
							if (NewStatus == OBDDataReaderStatus.Disconnected)
							{
								try
								{
									if (CS$<>8__locals1.semaphore.CurrentCount == 0)
									{
										CS$<>8__locals1.semaphore.Release();
									}
								}
								catch
								{
								}
							}
						};
						App.OBDReader.StatusChanged -= obdReaderStatusChanged;
						App.OBDReader.StatusChanged += obdReaderStatusChanged;
						pidValueChanged = delegate(object pid_sender, PID pid_e)
						{
							object lock_object = CS$<>8__locals1.lock_object;
							lock (lock_object)
							{
								if (pid_e != null)
								{
									CS$<>8__locals1.supported_pids.Add((CustomPID)pid_e);
								}
							}
						};
						LiveDataPIDModel liveDataPIDModel = new LiveDataPIDModel();
						enumerator = pids_for_test.GetEnumerator();
						try
						{
							while (enumerator.MoveNext())
							{
								CustomPID customPID2 = enumerator.Current;
								customPID2.ValueChanged -= pidValueChanged;
								customPID2.ValueChanged += pidValueChanged;
								liveDataPIDModel.SelectedPID = customPID2;
								liveDataPIDModel.GetRequests(requests, null, "");
							}
						}
						finally
						{
							if (num < 0)
							{
								((IDisposable)enumerator).Dispose();
							}
						}
						liveDataPIDModel.SelectedPID = PID.Empty;
						CS$<>8__locals1.total = requests.Count;
						CS$<>8__locals1.current = 0;
						CS$<>8__locals1.responseReceivedCounter = 0;
						responseReceivedDelegate = delegate(OBDRequest req, string data)
						{
							int num3 = CS$<>8__locals1.responseReceivedCounter;
							CS$<>8__locals1.responseReceivedCounter = num3 + 1;
							App.OBDReader.NO_DATA_Counter = 0;
							if (CS$<>8__locals1.cancellationToken.IsCancellationRequested)
							{
								App.OBDReader.ReplaceQueue(new OBDRequest[0]);
							}
							num3 = CS$<>8__locals1.current;
							CS$<>8__locals1.current = num3 + 1;
							Action action;
							if ((action = CS$<>8__locals1.<>9__6) == null)
							{
								action = (CS$<>8__locals1.<>9__6 = delegate
								{
									double num4 = (double)CS$<>8__locals1.current * 100.0 / (double)CS$<>8__locals1.total;
									IProgress<int> progress = CS$<>8__locals1.progress;
									if (progress == null)
									{
										return;
									}
									progress.Report((int)num4);
								});
							}
							MainThreadHelper.InvokeOnMainThread(action);
							if (App.OBDReader.ELMStatus.NoFinishCharacterErrors > 10 || App.OBDReader.ELMStatus.WrongNewLineCharactersCounter > 10 || CS$<>8__locals1.disconnectCounter > 3)
							{
								CS$<>8__locals1.tooMuchErrorsDetected = true;
								App.OBDReader.ReplaceQueue(new OBDRequest[0]);
							}
							foreach (string text2 in App.OBDReader.ELMStatus.ATCommandsNotSupported)
							{
								if (text2.StartsWith("ATFC") || text2.StartsWith("ATSH") || text2.StartsWith("ATSP") || text2.StartsWith("ATCP"))
								{
									CS$<>8__locals1.tooMuchErrorsDetected = true;
									App.OBDReader.ReplaceQueue(new OBDRequest[0]);
									App.OBDReader.BadELM = true;
								}
							}
						};
						List<OBDRequest>.Enumerator enumerator2 = requests.GetEnumerator();
						try
						{
							while (enumerator2.MoveNext())
							{
								OBDRequest obdrequest = enumerator2.Current;
								obdrequest.Repeat = false;
								obdrequest.ResponseReceived -= responseReceivedDelegate;
								obdrequest.ResponseReceived += responseReceivedDelegate;
							}
						}
						finally
						{
							if (num < 0)
							{
								((IDisposable)enumerator2).Dispose();
							}
						}
						App.OBDReader.ReplaceQueue(requests);
						Task.Run(delegate
						{
							ProfileSupportedPidsTester.<>c__DisplayClass4_0.<<PerformTest>b__4>d <<PerformTest>b__4>d;
							<<PerformTest>b__4>d.<>t__builder = AsyncTaskMethodBuilder.Create();
							<<PerformTest>b__4>d.<>4__this = CS$<>8__locals1;
							<<PerformTest>b__4>d.<>1__state = -1;
							<<PerformTest>b__4>d.<>t__builder.Start<ProfileSupportedPidsTester.<>c__DisplayClass4_0.<<PerformTest>b__4>d>(ref <<PerformTest>b__4>d);
							return <<PerformTest>b__4>d.<>t__builder.Task;
						});
						taskAwaiter = CS$<>8__locals1.semaphore.WaitAsync().GetAwaiter();
						if (!taskAwaiter.IsCompleted)
						{
							num = (num2 = 0);
							TaskAwaiter taskAwaiter2 = taskAwaiter;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, ProfileSupportedPidsTester.<PerformTest>d__4>(ref taskAwaiter, ref this);
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
						this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, ProfileSupportedPidsTester.<PerformTest>d__4>(ref taskAwaiter, ref this);
						return;
					}
					IL_03FB:
					taskAwaiter.GetResult();
					if (CS$<>8__locals1.responseReceivedCounter + CS$<>8__locals1.disconnectCounter < requests.Count)
					{
						ProfileSupportedPidsTester.IsTesting = false;
						text = "";
					}
					else
					{
						App.OBDReader.StatusChanged -= obdReaderStatusChanged;
						List<CustomPID>.Enumerator enumerator = pids_for_test.GetEnumerator();
						try
						{
							while (enumerator.MoveNext())
							{
								CustomPID customPID3 = enumerator.Current;
								customPID3.ValueChanged -= pidValueChanged;
							}
						}
						finally
						{
							if (num < 0)
							{
								((IDisposable)enumerator).Dispose();
							}
						}
						List<OBDRequest>.Enumerator enumerator2 = requests.GetEnumerator();
						try
						{
							while (enumerator2.MoveNext())
							{
								OBDRequest obdrequest2 = enumerator2.Current;
								obdrequest2.ResponseReceived -= responseReceivedDelegate;
							}
						}
						finally
						{
							if (num < 0)
							{
								((IDisposable)enumerator2).Dispose();
							}
						}
						int count = App.OBDReader.ELMStatus.ATCommandsNotSupported.Count;
						if (CS$<>8__locals1.tooMuchErrorsDetected)
						{
							ProfileSupportedPidsTester.IsTesting = false;
							text = Translate.GetString("profile_PidDetectionInterruptedDueToBadELM");
						}
						else if (CS$<>8__locals1.cancellationToken.IsCancellationRequested)
						{
							ProfileSupportedPidsTester.IsTesting = false;
							text = "";
						}
						else if (CS$<>8__locals1.supported_pids.Count == 0)
						{
							ProfileSupportedPidsTester.IsTesting = false;
							text = Translate.GetString("profile_PidDetectionZeroPidsSupported");
						}
						else
						{
							CustomPIDViewModel.CurrentProfile.PidCollection.Clear();
							enumerator = CS$<>8__locals1.supported_pids.GetEnumerator();
							try
							{
								while (enumerator.MoveNext())
								{
									CustomPID customPID4 = enumerator.Current;
									CustomPIDViewModel.CurrentProfile.PidCollection.Add(customPID4);
									if (!LiveDataPIDModel._PIDCollection.Contains(customPID4))
									{
										LiveDataPIDModel._PIDCollection.Add(customPID4);
									}
								}
							}
							finally
							{
								if (num < 0)
								{
									((IDisposable)enumerator).Dispose();
								}
							}
							enumerator = action_pids.GetEnumerator();
							try
							{
								while (enumerator.MoveNext())
								{
									CustomPID customPID5 = enumerator.Current;
									CustomPIDViewModel.CurrentProfile.PidCollection.Add(customPID5);
								}
							}
							finally
							{
								if (num < 0)
								{
									((IDisposable)enumerator).Dispose();
								}
							}
							CustomPIDViewModel.CurrentProfile.Save();
							List<CustomPID> list = (from x in pids_for_test.Except(CS$<>8__locals1.supported_pids)
								where x != null
								select x).ToList<CustomPID>();
							CustomPIDViewModel.DisabledProfile.PidCollection.Clear();
							enumerator = list.GetEnumerator();
							try
							{
								while (enumerator.MoveNext())
								{
									CustomPID customPID6 = enumerator.Current;
									CustomPIDViewModel.DisabledProfile.PidCollection.Add(customPID6);
									LiveDataPIDModel._PIDCollection.Remove(customPID6);
								}
							}
							finally
							{
								if (num < 0)
								{
									((IDisposable)enumerator).Dispose();
								}
							}
							CustomPIDViewModel.DisabledProfile.Save();
							ProfileSupportedPidsTester.IsTesting = false;
							text = "";
						}
					}
				}
				catch (Exception ex)
				{
					num2 = -2;
					CS$<>8__locals1 = null;
					pids_for_test = null;
					requests = null;
					action_pids = null;
					obdReaderStatusChanged = null;
					pidValueChanged = null;
					responseReceivedDelegate = null;
					this.<>t__builder.SetException(ex);
					return;
				}
				IL_0703:
				num2 = -2;
				CS$<>8__locals1 = null;
				pids_for_test = null;
				requests = null;
				action_pids = null;
				obdReaderStatusChanged = null;
				pidValueChanged = null;
				responseReceivedDelegate = null;
				this.<>t__builder.SetResult(text);
			}

			// Token: 0x0600229C RID: 8860 RVA: 0x001AC36C File Offset: 0x001AA56C
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x0400109D RID: 4253
			public int <>1__state;

			// Token: 0x0400109E RID: 4254
			public AsyncTaskMethodBuilder<string> <>t__builder;

			// Token: 0x0400109F RID: 4255
			public CancellationToken cancellationToken;

			// Token: 0x040010A0 RID: 4256
			public IProgress<int> progress;

			// Token: 0x040010A1 RID: 4257
			private ProfileSupportedPidsTester.<>c__DisplayClass4_0 <>8__1;

			// Token: 0x040010A2 RID: 4258
			private List<CustomPID> <pids_for_test>5__2;

			// Token: 0x040010A3 RID: 4259
			private List<OBDRequest> <requests>5__3;

			// Token: 0x040010A4 RID: 4260
			private List<CustomPID> <action_pids>5__4;

			// Token: 0x040010A5 RID: 4261
			private CurrentStatusChangedEvent <obdReaderStatusChanged>5__5;

			// Token: 0x040010A6 RID: 4262
			private EventHandler<PID> <pidValueChanged>5__6;

			// Token: 0x040010A7 RID: 4263
			private ResponseReceivedDelegate <responseReceivedDelegate>5__7;

			// Token: 0x040010A8 RID: 4264
			private TaskAwaiter <>u__1;
		}
	}
}
