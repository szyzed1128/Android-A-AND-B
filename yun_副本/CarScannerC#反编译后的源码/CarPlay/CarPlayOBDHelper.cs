using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using CarScannerXamarinForms.Common;
using CarScannerXamarinForms.DataRecorder;
using CarScannerXamarinForms.InApp;
using CarScannerXamarinForms.OBD2;
using CarScannerXamarinForms.OBD2.RequestProducers;
using CarScannerXamarinForms.PlatformAdapters;
using CarScannerXamarinForms.Settings;
using CarScannerXamarinForms.ViewModels;

namespace CarScannerXamarinForms.CarPlay
{
	// Token: 0x02000BEB RID: 3051
	public static class CarPlayOBDHelper
	{
		// Token: 0x06005BC9 RID: 23497 RVA: 0x0043AC1C File Offset: 0x00438E1C
		public static async Task StartConnectionLight()
		{
			try
			{
				CarPlayOBDHelper.<>c__DisplayClass0_0 CS$<>8__locals1 = new CarPlayOBDHelper.<>c__DisplayClass0_0();
				if (App.OBDReader.CurrentStatus == OBDDataReaderStatus.Disconnected)
				{
					if (!VersionChecker.IsValidVersion)
					{
						CarPlayManager instance = CarPlayManager.Instance;
						if (instance != null)
						{
							instance.DisplayAlert(Translate.GetString("main_VersionOutdatedTitle") + "\n" + Translate.GetString("main_VersionOutdatedText"));
						}
					}
					else if (SharedSettings.Current.ConnectionType == ConnectionTypes.BluetoothLE && string.IsNullOrEmpty(SharedSettings.Current.BTLEDeviceID))
					{
						CarPlayManager instance2 = CarPlayManager.Instance;
						if (instance2 != null)
						{
							instance2.DisplayAlert(Translate.GetString("ios_NoBTLE_DeviceSelectedTitle") + "\n" + Translate.GetString("ios_NoBTLE_DeviceSelectedText"));
						}
					}
					else if ((SharedSettings.Current.ConnectionType == ConnectionTypes.Bluetooth || SharedSettings.Current.ConnectionType == ConnectionTypes.MFI_OBDLinkMXPlus) && string.IsNullOrEmpty(SharedSettings.Current.BTDeviceID))
					{
						CarPlayManager instance3 = CarPlayManager.Instance;
						if (instance3 != null)
						{
							instance3.DisplayAlert(Translate.GetString("ios_NoBT_DeviceSelectedTitle") + "\n" + Translate.GetString("ios_NoBT_DeviceSelectedText"));
						}
					}
					else
					{
						App.OBDReader.DisconnectRequested = false;
						if (PlatformHelper.IsiOS)
						{
							try
							{
								if (SharedSettings.Current.ConnectionType == ConnectionTypes.BluetoothLE && PlatformHelper.IsPlatformVersionNewerOrEqual(13, 0) && PlatformHelper.IOSService.IsCoreBluetoothAuthorizationStatusDeniedOrRestricted())
								{
									CarPlayManager instance4 = CarPlayManager.Instance;
									if (instance4 != null)
									{
										instance4.DisplayAlert(Translate.GetString("ios_NoBluetoothPermissionTitle") + "\n" + Translate.GetString("ios_NoBluetoothPermissionText"));
									}
								}
							}
							catch
							{
							}
						}
						if (SharedSettings.Current.ForceProfileUpdateScheduled)
						{
							ProfileUpdater.ForceUpdate();
						}
						CS$<>8__locals1.connected = false;
						try
						{
							App.OBDReader.stopwatch.Restart();
							SharedSettings.Current.OptimizedRequestStuckCounter = 0;
							OBDDataReader obdreader = App.OBDReader;
							if (obdreader != null)
							{
								ELMState elmstatus = obdreader.ELMStatus;
								if (elmstatus != null)
								{
									elmstatus.ResetErrorsState();
								}
							}
							if (SharedSettings.Current.SpeedCalibrationTaskPending && SpeedCalibrationModelV2.Instance == null)
							{
								SpeedCalibrationModelV2.Instance = new SpeedCalibrationModelV2();
							}
							OBDRequestQueueOptimizer.ClearResponseCounterDictionary();
							await Task.Run(delegate
							{
								CarPlayOBDHelper.<>c__DisplayClass0_0.<<StartConnectionLight>b__0>d <<StartConnectionLight>b__0>d;
								<<StartConnectionLight>b__0>d.<>t__builder = AsyncTaskMethodBuilder.Create();
								<<StartConnectionLight>b__0>d.<>4__this = CS$<>8__locals1;
								<<StartConnectionLight>b__0>d.<>1__state = -1;
								<<StartConnectionLight>b__0>d.<>t__builder.Start<CarPlayOBDHelper.<>c__DisplayClass0_0.<<StartConnectionLight>b__0>d>(ref <<StartConnectionLight>b__0>d);
								return <<StartConnectionLight>b__0>d.<>t__builder.Task;
							}).ConfigureAwait(true);
						}
						catch (Exception)
						{
						}
						if (SharedSettings.Current.ConnectionType == ConnectionTypes.BluetoothLE && !CS$<>8__locals1.connected && SharedSettings.Current.SearchForBTLEIfConnectionFailed && !App.OBDReader.DisconnectRequested)
						{
							OBDDataReaderStatus prevStatus = App.OBDReader.CurrentStatus;
							App.OBDReader.SetStatusForTest(OBDDataReaderStatus.ConnectingToECU);
							bool flag = await BTLEDeviceSelectorViewModel.FindDeviceWithRandomizedUUID();
							App.OBDReader.SetStatusForTest(prevStatus);
							if (flag)
							{
								try
								{
									OBDRequestQueueOptimizer.ClearResponseCounterDictionary();
									await Task.Run(delegate
									{
										CarPlayOBDHelper.<>c__DisplayClass0_0.<<StartConnectionLight>b__1>d <<StartConnectionLight>b__1>d;
										<<StartConnectionLight>b__1>d.<>t__builder = AsyncTaskMethodBuilder.Create();
										<<StartConnectionLight>b__1>d.<>4__this = CS$<>8__locals1;
										<<StartConnectionLight>b__1>d.<>1__state = -1;
										<<StartConnectionLight>b__1>d.<>t__builder.Start<CarPlayOBDHelper.<>c__DisplayClass0_0.<<StartConnectionLight>b__1>d>(ref <<StartConnectionLight>b__1>d);
										return <<StartConnectionLight>b__1>d.<>t__builder.Task;
									}).ConfigureAwait(true);
								}
								catch (Exception)
								{
								}
							}
						}
						string text = "";
						switch (SharedSettings.Current.ConnectionType)
						{
						case ConnectionTypes.WiFi:
							text = SharedSettings.Current.WiFiServer + ":" + SharedSettings.Current.WiFiPort;
							break;
						case ConnectionTypes.BluetoothLE:
							text = SharedSettings.Current.BTLEDeviceID;
							break;
						case ConnectionTypes.Bluetooth:
							text = SharedSettings.Current.BTDeviceID;
							break;
						}
						ScanXChecker.CheckDeviceAtConnection(text);
						if (CS$<>8__locals1.connected && !App.OBDReader.DisconnectRequested)
						{
							CarPlayOBDHelper.<>c__DisplayClass0_1 CS$<>8__locals2 = new CarPlayOBDHelper.<>c__DisplayClass0_1();
							CS$<>8__locals2.attempts = 3;
							if (SharedSettings.Current.ProtocolNumber == 0 && SharedSettings.Current.UseDefaultInit)
							{
								CS$<>8__locals2.attempts = 2;
							}
							CarInfoViewModel.Instance.Reset();
							CS$<>8__locals2.initResult = false;
							CS$<>8__locals2.progress = new Progress<string>();
							await Task.Run(delegate
							{
								CarPlayOBDHelper.<>c__DisplayClass0_1.<<StartConnectionLight>b__2>d <<StartConnectionLight>b__2>d;
								<<StartConnectionLight>b__2>d.<>t__builder = AsyncTaskMethodBuilder.Create();
								<<StartConnectionLight>b__2>d.<>4__this = CS$<>8__locals2;
								<<StartConnectionLight>b__2>d.<>1__state = -1;
								<<StartConnectionLight>b__2>d.<>t__builder.Start<CarPlayOBDHelper.<>c__DisplayClass0_1.<<StartConnectionLight>b__2>d>(ref <<StartConnectionLight>b__2>d);
								return <<StartConnectionLight>b__2>d.<>t__builder.Task;
							}).ConfigureAwait(true);
							if (CS$<>8__locals2.initResult)
							{
								SharedSettings.Current.FreePeriodGoodConnections++;
								if (PlatformHelper.IsAndroid && SharedSettings.Current.AndroidStartBackgroundService)
								{
									PlatformHelper.DroidService.AndroidHelper_StartService();
								}
								await CarInfoViewModel.Instance.Load(false);
								DataRecorderV2 recorder = App.OBDReader.CurrentCarData.Recorder;
								if (recorder != null)
								{
									recorder.Save();
								}
								if (SharedSettings.Current.RecordData)
								{
									DataRecorderV2.StartRecording();
								}
								else
								{
									App.OBDReader.CurrentCarData.Recorder = null;
								}
								RequestProducerStatic.UpdateOBDReaderRequests();
								SpeedTestViewModel.Instance.Initialize();
							}
							CS$<>8__locals2 = null;
						}
						CS$<>8__locals1 = null;
					}
				}
			}
			catch (Exception)
			{
			}
		}

		// Token: 0x06005BCA RID: 23498 RVA: 0x0043AC58 File Offset: 0x00438E58
		public static void Initialize()
		{
			CarData currentCarData = App.OBDReader.CurrentCarData;
			if (currentCarData != null)
			{
				currentCarData.CreateEmptyPIDS();
			}
			try
			{
				LiveDataPIDModel.LoadCustomPIDS();
			}
			catch (Exception)
			{
			}
		}

		// Token: 0x02000BEC RID: 3052
		[CompilerGenerated]
		private sealed class <>c__DisplayClass0_0
		{
			// Token: 0x06005BCB RID: 23499 RVA: 0x00002050 File Offset: 0x00000250
			public <>c__DisplayClass0_0()
			{
			}

			// Token: 0x06005BCC RID: 23500 RVA: 0x0043AC98 File Offset: 0x00438E98
			internal async Task <StartConnectionLight>b__0()
			{
				bool flag = await App.OBDReader.Connect(true);
				this.connected = flag;
			}

			// Token: 0x06005BCD RID: 23501 RVA: 0x0043ACDC File Offset: 0x00438EDC
			internal async Task <StartConnectionLight>b__1()
			{
				try
				{
					bool flag = await App.OBDReader.Connect(true);
					this.connected = flag;
				}
				catch (Exception)
				{
				}
			}

			// Token: 0x040039E5 RID: 14821
			public bool connected;

			// Token: 0x02000BED RID: 3053
			[StructLayout(LayoutKind.Auto)]
			private struct <<StartConnectionLight>b__0>d : IAsyncStateMachine
			{
				// Token: 0x06005BCE RID: 23502 RVA: 0x0043AD20 File Offset: 0x00438F20
				void IAsyncStateMachine.MoveNext()
				{
					int num2;
					int num = num2;
					CarPlayOBDHelper.<>c__DisplayClass0_0 CS$<>8__locals1 = this;
					try
					{
						TaskAwaiter<bool> taskAwaiter;
						if (num != 0)
						{
							taskAwaiter = App.OBDReader.Connect(true).GetAwaiter();
							if (!taskAwaiter.IsCompleted)
							{
								num2 = 0;
								TaskAwaiter<bool> taskAwaiter2 = taskAwaiter;
								this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<bool>, CarPlayOBDHelper.<>c__DisplayClass0_0.<<StartConnectionLight>b__0>d>(ref taskAwaiter, ref this);
								return;
							}
						}
						else
						{
							TaskAwaiter<bool> taskAwaiter2;
							taskAwaiter = taskAwaiter2;
							taskAwaiter2 = default(TaskAwaiter<bool>);
							num2 = -1;
						}
						bool result = taskAwaiter.GetResult();
						CS$<>8__locals1.connected = result;
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

				// Token: 0x06005BCF RID: 23503 RVA: 0x0043ADE4 File Offset: 0x00438FE4
				[DebuggerHidden]
				void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
				{
					this.<>t__builder.SetStateMachine(stateMachine);
				}

				// Token: 0x040039E6 RID: 14822
				public int <>1__state;

				// Token: 0x040039E7 RID: 14823
				public AsyncTaskMethodBuilder <>t__builder;

				// Token: 0x040039E8 RID: 14824
				public CarPlayOBDHelper.<>c__DisplayClass0_0 <>4__this;

				// Token: 0x040039E9 RID: 14825
				private TaskAwaiter<bool> <>u__1;
			}

			// Token: 0x02000BEE RID: 3054
			[StructLayout(LayoutKind.Auto)]
			private struct <<StartConnectionLight>b__1>d : IAsyncStateMachine
			{
				// Token: 0x06005BD0 RID: 23504 RVA: 0x0043ADF4 File Offset: 0x00438FF4
				void IAsyncStateMachine.MoveNext()
				{
					int num2;
					int num = num2;
					CarPlayOBDHelper.<>c__DisplayClass0_0 CS$<>8__locals1 = this;
					try
					{
						try
						{
							TaskAwaiter<bool> taskAwaiter;
							if (num != 0)
							{
								taskAwaiter = App.OBDReader.Connect(true).GetAwaiter();
								if (!taskAwaiter.IsCompleted)
								{
									num2 = 0;
									TaskAwaiter<bool> taskAwaiter2 = taskAwaiter;
									this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<bool>, CarPlayOBDHelper.<>c__DisplayClass0_0.<<StartConnectionLight>b__1>d>(ref taskAwaiter, ref this);
									return;
								}
							}
							else
							{
								TaskAwaiter<bool> taskAwaiter2;
								taskAwaiter = taskAwaiter2;
								taskAwaiter2 = default(TaskAwaiter<bool>);
								num2 = -1;
							}
							bool result = taskAwaiter.GetResult();
							CS$<>8__locals1.connected = result;
						}
						catch (Exception)
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

				// Token: 0x06005BD1 RID: 23505 RVA: 0x0043AECC File Offset: 0x004390CC
				[DebuggerHidden]
				void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
				{
					this.<>t__builder.SetStateMachine(stateMachine);
				}

				// Token: 0x040039EA RID: 14826
				public int <>1__state;

				// Token: 0x040039EB RID: 14827
				public AsyncTaskMethodBuilder <>t__builder;

				// Token: 0x040039EC RID: 14828
				public CarPlayOBDHelper.<>c__DisplayClass0_0 <>4__this;

				// Token: 0x040039ED RID: 14829
				private TaskAwaiter<bool> <>u__1;
			}
		}

		// Token: 0x02000BEF RID: 3055
		[CompilerGenerated]
		private sealed class <>c__DisplayClass0_1
		{
			// Token: 0x06005BD2 RID: 23506 RVA: 0x00002050 File Offset: 0x00000250
			public <>c__DisplayClass0_1()
			{
			}

			// Token: 0x06005BD3 RID: 23507 RVA: 0x0043AEDC File Offset: 0x004390DC
			internal async Task <StartConnectionLight>b__2()
			{
				bool flag = await App.OBDReader.Initialize(this.attempts, OBDDataReader.InitModes.Default, this.progress);
				this.initResult = flag;
			}

			// Token: 0x040039EE RID: 14830
			public int attempts;

			// Token: 0x040039EF RID: 14831
			public Progress<string> progress;

			// Token: 0x040039F0 RID: 14832
			public bool initResult;

			// Token: 0x02000BF0 RID: 3056
			[StructLayout(LayoutKind.Auto)]
			private struct <<StartConnectionLight>b__2>d : IAsyncStateMachine
			{
				// Token: 0x06005BD4 RID: 23508 RVA: 0x0043AF20 File Offset: 0x00439120
				void IAsyncStateMachine.MoveNext()
				{
					int num2;
					int num = num2;
					CarPlayOBDHelper.<>c__DisplayClass0_1 CS$<>8__locals1 = this;
					try
					{
						TaskAwaiter<bool> taskAwaiter;
						if (num != 0)
						{
							taskAwaiter = App.OBDReader.Initialize(CS$<>8__locals1.attempts, OBDDataReader.InitModes.Default, CS$<>8__locals1.progress).GetAwaiter();
							if (!taskAwaiter.IsCompleted)
							{
								num2 = 0;
								TaskAwaiter<bool> taskAwaiter2 = taskAwaiter;
								this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<bool>, CarPlayOBDHelper.<>c__DisplayClass0_1.<<StartConnectionLight>b__2>d>(ref taskAwaiter, ref this);
								return;
							}
						}
						else
						{
							TaskAwaiter<bool> taskAwaiter2;
							taskAwaiter = taskAwaiter2;
							taskAwaiter2 = default(TaskAwaiter<bool>);
							num2 = -1;
						}
						bool result = taskAwaiter.GetResult();
						CS$<>8__locals1.initResult = result;
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

				// Token: 0x06005BD5 RID: 23509 RVA: 0x0043AFF0 File Offset: 0x004391F0
				[DebuggerHidden]
				void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
				{
					this.<>t__builder.SetStateMachine(stateMachine);
				}

				// Token: 0x040039F1 RID: 14833
				public int <>1__state;

				// Token: 0x040039F2 RID: 14834
				public AsyncTaskMethodBuilder <>t__builder;

				// Token: 0x040039F3 RID: 14835
				public CarPlayOBDHelper.<>c__DisplayClass0_1 <>4__this;

				// Token: 0x040039F4 RID: 14836
				private TaskAwaiter<bool> <>u__1;
			}
		}

		// Token: 0x02000BF1 RID: 3057
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <StartConnectionLight>d__0 : IAsyncStateMachine
		{
			// Token: 0x06005BD6 RID: 23510 RVA: 0x0043B000 File Offset: 0x00439200
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				try
				{
					try
					{
						TaskAwaiter<bool> taskAwaiter;
						ConfiguredTaskAwaitable.ConfiguredTaskAwaiter configuredTaskAwaiter;
						ConfiguredTaskAwaitable.ConfiguredTaskAwaiter configuredTaskAwaiter2;
						TaskAwaiter taskAwaiter3;
						switch (num)
						{
						case 0:
							break;
						case 1:
						{
							TaskAwaiter<bool> taskAwaiter2;
							taskAwaiter = taskAwaiter2;
							taskAwaiter2 = default(TaskAwaiter<bool>);
							num = (num2 = -1);
							goto IL_033A;
						}
						case 2:
							IL_0356:
							try
							{
								if (num != 2)
								{
									OBDRequestQueueOptimizer.ClearResponseCounterDictionary();
									configuredTaskAwaiter = Task.Run(delegate
									{
										CarPlayOBDHelper.<>c__DisplayClass0_0.<<StartConnectionLight>b__1>d <<StartConnectionLight>b__1>d;
										<<StartConnectionLight>b__1>d.<>t__builder = AsyncTaskMethodBuilder.Create();
										<<StartConnectionLight>b__1>d.<>4__this = CS$<>8__locals1;
										<<StartConnectionLight>b__1>d.<>1__state = -1;
										<<StartConnectionLight>b__1>d.<>t__builder.Start<CarPlayOBDHelper.<>c__DisplayClass0_0.<<StartConnectionLight>b__1>d>(ref <<StartConnectionLight>b__1>d);
										return <<StartConnectionLight>b__1>d.<>t__builder.Task;
									}).ConfigureAwait(true).GetAwaiter();
									if (!configuredTaskAwaiter.IsCompleted)
									{
										num = (num2 = 2);
										configuredTaskAwaiter2 = configuredTaskAwaiter;
										this.<>t__builder.AwaitUnsafeOnCompleted<ConfiguredTaskAwaitable.ConfiguredTaskAwaiter, CarPlayOBDHelper.<StartConnectionLight>d__0>(ref configuredTaskAwaiter, ref this);
										return;
									}
								}
								else
								{
									configuredTaskAwaiter = configuredTaskAwaiter2;
									configuredTaskAwaiter2 = default(ConfiguredTaskAwaitable.ConfiguredTaskAwaiter);
									num = (num2 = -1);
								}
								configuredTaskAwaiter.GetResult();
							}
							catch (Exception)
							{
							}
							goto IL_03D9;
						case 3:
							configuredTaskAwaiter = configuredTaskAwaiter2;
							configuredTaskAwaiter2 = default(ConfiguredTaskAwaitable.ConfiguredTaskAwaiter);
							num = (num2 = -1);
							goto IL_052F;
						case 4:
						{
							TaskAwaiter taskAwaiter4;
							taskAwaiter3 = taskAwaiter4;
							taskAwaiter4 = default(TaskAwaiter);
							num = (num2 = -1);
							goto IL_05D5;
						}
						default:
							CS$<>8__locals1 = new CarPlayOBDHelper.<>c__DisplayClass0_0();
							if (App.OBDReader.CurrentStatus != OBDDataReaderStatus.Disconnected)
							{
								goto IL_0658;
							}
							if (!VersionChecker.IsValidVersion)
							{
								CarPlayManager instance = CarPlayManager.Instance;
								if (instance != null)
								{
									instance.DisplayAlert(Translate.GetString("main_VersionOutdatedTitle") + "\n" + Translate.GetString("main_VersionOutdatedText"));
								}
								goto IL_0658;
							}
							if (SharedSettings.Current.ConnectionType == ConnectionTypes.BluetoothLE && string.IsNullOrEmpty(SharedSettings.Current.BTLEDeviceID))
							{
								CarPlayManager instance2 = CarPlayManager.Instance;
								if (instance2 != null)
								{
									instance2.DisplayAlert(Translate.GetString("ios_NoBTLE_DeviceSelectedTitle") + "\n" + Translate.GetString("ios_NoBTLE_DeviceSelectedText"));
								}
								goto IL_0658;
							}
							if ((SharedSettings.Current.ConnectionType == ConnectionTypes.Bluetooth || SharedSettings.Current.ConnectionType == ConnectionTypes.MFI_OBDLinkMXPlus) && string.IsNullOrEmpty(SharedSettings.Current.BTDeviceID))
							{
								CarPlayManager instance3 = CarPlayManager.Instance;
								if (instance3 != null)
								{
									instance3.DisplayAlert(Translate.GetString("ios_NoBT_DeviceSelectedTitle") + "\n" + Translate.GetString("ios_NoBT_DeviceSelectedText"));
								}
								goto IL_0658;
							}
							App.OBDReader.DisconnectRequested = false;
							if (PlatformHelper.IsiOS)
							{
								try
								{
									if (SharedSettings.Current.ConnectionType == ConnectionTypes.BluetoothLE && PlatformHelper.IsPlatformVersionNewerOrEqual(13, 0) && PlatformHelper.IOSService.IsCoreBluetoothAuthorizationStatusDeniedOrRestricted())
									{
										CarPlayManager instance4 = CarPlayManager.Instance;
										if (instance4 != null)
										{
											instance4.DisplayAlert(Translate.GetString("ios_NoBluetoothPermissionTitle") + "\n" + Translate.GetString("ios_NoBluetoothPermissionText"));
										}
									}
								}
								catch
								{
								}
							}
							if (SharedSettings.Current.ForceProfileUpdateScheduled)
							{
								ProfileUpdater.ForceUpdate();
							}
							CS$<>8__locals1.connected = false;
							break;
						}
						try
						{
							if (num != 0)
							{
								App.OBDReader.stopwatch.Restart();
								SharedSettings.Current.OptimizedRequestStuckCounter = 0;
								OBDDataReader obdreader = App.OBDReader;
								if (obdreader != null)
								{
									ELMState elmstatus = obdreader.ELMStatus;
									if (elmstatus != null)
									{
										elmstatus.ResetErrorsState();
									}
								}
								if (SharedSettings.Current.SpeedCalibrationTaskPending && SpeedCalibrationModelV2.Instance == null)
								{
									SpeedCalibrationModelV2.Instance = new SpeedCalibrationModelV2();
								}
								OBDRequestQueueOptimizer.ClearResponseCounterDictionary();
								configuredTaskAwaiter = Task.Run(delegate
								{
									CarPlayOBDHelper.<>c__DisplayClass0_0.<<StartConnectionLight>b__0>d <<StartConnectionLight>b__0>d;
									<<StartConnectionLight>b__0>d.<>t__builder = AsyncTaskMethodBuilder.Create();
									<<StartConnectionLight>b__0>d.<>4__this = CS$<>8__locals1;
									<<StartConnectionLight>b__0>d.<>1__state = -1;
									<<StartConnectionLight>b__0>d.<>t__builder.Start<CarPlayOBDHelper.<>c__DisplayClass0_0.<<StartConnectionLight>b__0>d>(ref <<StartConnectionLight>b__0>d);
									return <<StartConnectionLight>b__0>d.<>t__builder.Task;
								}).ConfigureAwait(true).GetAwaiter();
								if (!configuredTaskAwaiter.IsCompleted)
								{
									num = (num2 = 0);
									configuredTaskAwaiter2 = configuredTaskAwaiter;
									this.<>t__builder.AwaitUnsafeOnCompleted<ConfiguredTaskAwaitable.ConfiguredTaskAwaiter, CarPlayOBDHelper.<StartConnectionLight>d__0>(ref configuredTaskAwaiter, ref this);
									return;
								}
							}
							else
							{
								configuredTaskAwaiter = configuredTaskAwaiter2;
								configuredTaskAwaiter2 = default(ConfiguredTaskAwaitable.ConfiguredTaskAwaiter);
								num = (num2 = -1);
							}
							configuredTaskAwaiter.GetResult();
						}
						catch (Exception)
						{
						}
						if (SharedSettings.Current.ConnectionType != ConnectionTypes.BluetoothLE || CS$<>8__locals1.connected || !SharedSettings.Current.SearchForBTLEIfConnectionFailed || App.OBDReader.DisconnectRequested)
						{
							goto IL_03D9;
						}
						prevStatus = App.OBDReader.CurrentStatus;
						App.OBDReader.SetStatusForTest(OBDDataReaderStatus.ConnectingToECU);
						taskAwaiter = BTLEDeviceSelectorViewModel.FindDeviceWithRandomizedUUID().GetAwaiter();
						if (!taskAwaiter.IsCompleted)
						{
							num = (num2 = 1);
							TaskAwaiter<bool> taskAwaiter2 = taskAwaiter;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<bool>, CarPlayOBDHelper.<StartConnectionLight>d__0>(ref taskAwaiter, ref this);
							return;
						}
						IL_033A:
						bool result = taskAwaiter.GetResult();
						App.OBDReader.SetStatusForTest(prevStatus);
						if (result)
						{
							goto IL_0356;
						}
						IL_03D9:
						string text = "";
						switch (SharedSettings.Current.ConnectionType)
						{
						case ConnectionTypes.WiFi:
							text = SharedSettings.Current.WiFiServer + ":" + SharedSettings.Current.WiFiPort;
							break;
						case ConnectionTypes.BluetoothLE:
							text = SharedSettings.Current.BTLEDeviceID;
							break;
						case ConnectionTypes.Bluetooth:
							text = SharedSettings.Current.BTDeviceID;
							break;
						}
						ScanXChecker.CheckDeviceAtConnection(text);
						if (!CS$<>8__locals1.connected || App.OBDReader.DisconnectRequested)
						{
							goto IL_0631;
						}
						CS$<>8__locals2 = new CarPlayOBDHelper.<>c__DisplayClass0_1();
						CS$<>8__locals2.attempts = 3;
						if (SharedSettings.Current.ProtocolNumber == 0 && SharedSettings.Current.UseDefaultInit)
						{
							CS$<>8__locals2.attempts = 2;
						}
						CarInfoViewModel.Instance.Reset();
						CS$<>8__locals2.initResult = false;
						CS$<>8__locals2.progress = new Progress<string>();
						configuredTaskAwaiter = Task.Run(delegate
						{
							CarPlayOBDHelper.<>c__DisplayClass0_1.<<StartConnectionLight>b__2>d <<StartConnectionLight>b__2>d;
							<<StartConnectionLight>b__2>d.<>t__builder = AsyncTaskMethodBuilder.Create();
							<<StartConnectionLight>b__2>d.<>4__this = CS$<>8__locals2;
							<<StartConnectionLight>b__2>d.<>1__state = -1;
							<<StartConnectionLight>b__2>d.<>t__builder.Start<CarPlayOBDHelper.<>c__DisplayClass0_1.<<StartConnectionLight>b__2>d>(ref <<StartConnectionLight>b__2>d);
							return <<StartConnectionLight>b__2>d.<>t__builder.Task;
						}).ConfigureAwait(true).GetAwaiter();
						if (!configuredTaskAwaiter.IsCompleted)
						{
							num = (num2 = 3);
							configuredTaskAwaiter2 = configuredTaskAwaiter;
							this.<>t__builder.AwaitUnsafeOnCompleted<ConfiguredTaskAwaitable.ConfiguredTaskAwaiter, CarPlayOBDHelper.<StartConnectionLight>d__0>(ref configuredTaskAwaiter, ref this);
							return;
						}
						IL_052F:
						configuredTaskAwaiter.GetResult();
						if (!CS$<>8__locals2.initResult)
						{
							goto IL_062A;
						}
						SharedSettings sharedSettings = SharedSettings.Current;
						int freePeriodGoodConnections = sharedSettings.FreePeriodGoodConnections;
						sharedSettings.FreePeriodGoodConnections = freePeriodGoodConnections + 1;
						if (PlatformHelper.IsAndroid && SharedSettings.Current.AndroidStartBackgroundService)
						{
							PlatformHelper.DroidService.AndroidHelper_StartService();
						}
						taskAwaiter3 = CarInfoViewModel.Instance.Load(false).GetAwaiter();
						if (!taskAwaiter3.IsCompleted)
						{
							num = (num2 = 4);
							TaskAwaiter taskAwaiter4 = taskAwaiter3;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, CarPlayOBDHelper.<StartConnectionLight>d__0>(ref taskAwaiter3, ref this);
							return;
						}
						IL_05D5:
						taskAwaiter3.GetResult();
						DataRecorderV2 recorder = App.OBDReader.CurrentCarData.Recorder;
						if (recorder != null)
						{
							recorder.Save();
						}
						if (SharedSettings.Current.RecordData)
						{
							DataRecorderV2.StartRecording();
						}
						else
						{
							App.OBDReader.CurrentCarData.Recorder = null;
						}
						RequestProducerStatic.UpdateOBDReaderRequests();
						SpeedTestViewModel.Instance.Initialize();
						IL_062A:
						CS$<>8__locals2 = null;
						IL_0631:
						CS$<>8__locals1 = null;
					}
					catch (Exception)
					{
					}
				}
				catch (Exception ex)
				{
					num2 = -2;
					this.<>t__builder.SetException(ex);
					return;
				}
				IL_0658:
				num2 = -2;
				this.<>t__builder.SetResult();
			}

			// Token: 0x06005BD7 RID: 23511 RVA: 0x0043B6F4 File Offset: 0x004398F4
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x040039F5 RID: 14837
			public int <>1__state;

			// Token: 0x040039F6 RID: 14838
			public AsyncTaskMethodBuilder <>t__builder;

			// Token: 0x040039F7 RID: 14839
			private CarPlayOBDHelper.<>c__DisplayClass0_0 <>8__1;

			// Token: 0x040039F8 RID: 14840
			private CarPlayOBDHelper.<>c__DisplayClass0_1 <>8__2;

			// Token: 0x040039F9 RID: 14841
			private ConfiguredTaskAwaitable.ConfiguredTaskAwaiter <>u__1;

			// Token: 0x040039FA RID: 14842
			private OBDDataReaderStatus <prevStatus>5__2;

			// Token: 0x040039FB RID: 14843
			private TaskAwaiter<bool> <>u__2;

			// Token: 0x040039FC RID: 14844
			private TaskAwaiter <>u__3;
		}
	}
}
