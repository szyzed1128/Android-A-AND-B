using System;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using CarScannerXamarinForms.OBD2;
using CarScannerXamarinForms.OBD2.PIDS;
using CarScannerXamarinForms.Settings;
using Xamarin.Essentials;

namespace CarScannerXamarinForms.DataRecorder
{
	// Token: 0x020006EC RID: 1772
	internal static class GPSDataCollector
	{
		// Token: 0x170013E2 RID: 5090
		// (get) Token: 0x06003C52 RID: 15442 RVA: 0x00318A8D File Offset: 0x00316C8D
		// (set) Token: 0x06003C53 RID: 15443 RVA: 0x00318A94 File Offset: 0x00316C94
		public static bool IsUpdating
		{
			[CompilerGenerated]
			get
			{
				return GPSDataCollector.<IsUpdating>k__BackingField;
			}
			[CompilerGenerated]
			private set
			{
				GPSDataCollector.<IsUpdating>k__BackingField = value;
			}
		}

		// Token: 0x06003C54 RID: 15444 RVA: 0x00318A9C File Offset: 0x00316C9C
		public static void StartUpdates()
		{
			Task.Run(async delegate
			{
				timeout = TimeSpan.FromSeconds(5.0);
				if (GPSDataCollector.IsUpdating)
				{
					await Task.Delay(timeout);
					if (GPSDataCollector.IsUpdating)
					{
						return;
					}
					if (App.OBDReader.CurrentStatus != OBDDataReaderStatus.ConnectedToECU)
					{
						return;
					}
				}
				TaskAwaiter<PermissionStatus> taskAwaiter3 = Permissions.CheckStatusAsync<Permissions.LocationWhenInUse>().GetAwaiter();
				if (!taskAwaiter3.IsCompleted)
				{
					await taskAwaiter3;
					taskAwaiter3 = taskAwaiter2;
					taskAwaiter2 = default(TaskAwaiter<PermissionStatus>);
				}
				if (taskAwaiter3.GetResult() == 3)
				{
					GPSDataCollector.IsUpdating = true;
					gpsSpeedPid = App.OBDReader.CurrentCarData.LiveDataPIDs.FirstOrDefault((PID x) => x is PID_GPSSpeed) as PID_GPSSpeed;
					gpsAltPid = App.OBDReader.CurrentCarData.LiveDataPIDs.FirstOrDefault((PID x) => x is PID_GPSAltitude) as PID_GPSAltitude;
					while (SharedSettings.Current.RecordLocationData && GPSDataCollector.IsUpdating)
					{
						try
						{
							Location location = await Geolocation.GetLocationAsync(new GeolocationRequest(5, timeout));
							if (location != null)
							{
								if (!double.IsFinite(location.Latitude) || !double.IsFinite(location.Longitude) || location.Latitude == 0.0 || location.Longitude == 0.0)
								{
									continue;
								}
								if (GPSDataCollector.LastLatitude != location.Latitude || GPSDataCollector.LastLongtitude != location.Longitude)
								{
									GPSDataCollector.LastLatitude = location.Latitude;
									GPSDataCollector.LastLongtitude = location.Longitude;
									GPSDataCollector.LastGPSTimeStamp = location.Timestamp.DateTime;
									DataRecorderV2 recorder = App.OBDReader.CurrentCarData.Recorder;
									if (recorder != null)
									{
										recorder.Record(GPSDataCollector.LastLatitude, GPSDataCollector.LastLongtitude);
									}
								}
								if (location.Speed != null && location.Speed != null && location.Speed.Value != GPSDataCollector.LastGPSSpeed)
								{
									GPSDataCollector.LastGPSSpeed = location.Speed.Value;
									PID_GPSSpeed pid_GPSSpeed = gpsSpeedPid;
									if (pid_GPSSpeed != null)
									{
										pid_GPSSpeed.ReportSpeedFromOtherSource(GPSDataCollector.LastGPSSpeed * 3.6);
									}
								}
								if (location.Altitude != null && location.Altitude != null && location.Altitude.Value != GPSDataCollector.LastAltitude)
								{
									GPSDataCollector.LastAltitude = location.Altitude.Value;
									gpsAltPid.ReportAltitudeFromOtherSource(GPSDataCollector.LastAltitude);
								}
							}
							Task.Delay(100).Wait();
						}
						catch (Exception)
						{
						}
					}
					GPSDataCollector.IsUpdating = false;
					gpsSpeedPid = null;
					gpsAltPid = null;
				}
				else
				{
					SharedSettings.Current.RecordLocationData = false;
				}
			});
		}

		// Token: 0x170013E3 RID: 5091
		// (get) Token: 0x06003C55 RID: 15445 RVA: 0x00318AC3 File Offset: 0x00316CC3
		// (set) Token: 0x06003C56 RID: 15446 RVA: 0x00318ACA File Offset: 0x00316CCA
		public static double LastLongtitude
		{
			[CompilerGenerated]
			get
			{
				return GPSDataCollector.<LastLongtitude>k__BackingField;
			}
			[CompilerGenerated]
			private set
			{
				GPSDataCollector.<LastLongtitude>k__BackingField = value;
			}
		}

		// Token: 0x170013E4 RID: 5092
		// (get) Token: 0x06003C57 RID: 15447 RVA: 0x00318AD2 File Offset: 0x00316CD2
		// (set) Token: 0x06003C58 RID: 15448 RVA: 0x00318AD9 File Offset: 0x00316CD9
		public static double LastLatitude
		{
			[CompilerGenerated]
			get
			{
				return GPSDataCollector.<LastLatitude>k__BackingField;
			}
			[CompilerGenerated]
			private set
			{
				GPSDataCollector.<LastLatitude>k__BackingField = value;
			}
		}

		// Token: 0x170013E5 RID: 5093
		// (get) Token: 0x06003C59 RID: 15449 RVA: 0x00318AE1 File Offset: 0x00316CE1
		// (set) Token: 0x06003C5A RID: 15450 RVA: 0x00318AE8 File Offset: 0x00316CE8
		public static double LastGPSSpeed
		{
			[CompilerGenerated]
			get
			{
				return GPSDataCollector.<LastGPSSpeed>k__BackingField;
			}
			[CompilerGenerated]
			private set
			{
				GPSDataCollector.<LastGPSSpeed>k__BackingField = value;
			}
		}

		// Token: 0x170013E6 RID: 5094
		// (get) Token: 0x06003C5B RID: 15451 RVA: 0x00318AF0 File Offset: 0x00316CF0
		// (set) Token: 0x06003C5C RID: 15452 RVA: 0x00318AF7 File Offset: 0x00316CF7
		public static double LastAltitude
		{
			[CompilerGenerated]
			get
			{
				return GPSDataCollector.<LastAltitude>k__BackingField;
			}
			[CompilerGenerated]
			private set
			{
				GPSDataCollector.<LastAltitude>k__BackingField = value;
			}
		}

		// Token: 0x170013E7 RID: 5095
		// (get) Token: 0x06003C5D RID: 15453 RVA: 0x00318AFF File Offset: 0x00316CFF
		// (set) Token: 0x06003C5E RID: 15454 RVA: 0x00318B06 File Offset: 0x00316D06
		public static DateTime LastGPSTimeStamp
		{
			[CompilerGenerated]
			get
			{
				return GPSDataCollector.<LastGPSTimeStamp>k__BackingField;
			}
			[CompilerGenerated]
			private set
			{
				GPSDataCollector.<LastGPSTimeStamp>k__BackingField = value;
			}
		}

		// Token: 0x06003C5F RID: 15455 RVA: 0x00318B0E File Offset: 0x00316D0E
		internal static void StopUpdates()
		{
			GPSDataCollector.IsUpdating = false;
		}

		// Token: 0x040024E6 RID: 9446
		[CompilerGenerated]
		private static bool <IsUpdating>k__BackingField;

		// Token: 0x040024E7 RID: 9447
		[CompilerGenerated]
		private static double <LastLongtitude>k__BackingField;

		// Token: 0x040024E8 RID: 9448
		[CompilerGenerated]
		private static double <LastLatitude>k__BackingField;

		// Token: 0x040024E9 RID: 9449
		[CompilerGenerated]
		private static double <LastGPSSpeed>k__BackingField;

		// Token: 0x040024EA RID: 9450
		[CompilerGenerated]
		private static double <LastAltitude>k__BackingField;

		// Token: 0x040024EB RID: 9451
		[CompilerGenerated]
		private static DateTime <LastGPSTimeStamp>k__BackingField;

		// Token: 0x020006ED RID: 1773
		[CompilerGenerated]
		[Serializable]
		private sealed class <>c
		{
			// Token: 0x06003C60 RID: 15456 RVA: 0x00318B16 File Offset: 0x00316D16
			// Note: this type is marked as 'beforefieldinit'.
			static <>c()
			{
			}

			// Token: 0x06003C61 RID: 15457 RVA: 0x00002050 File Offset: 0x00000250
			public <>c()
			{
			}

			// Token: 0x06003C62 RID: 15458 RVA: 0x00318B24 File Offset: 0x00316D24
			internal async Task <StartUpdates>b__4_0()
			{
				TimeSpan timeout = TimeSpan.FromSeconds(5.0);
				if (GPSDataCollector.IsUpdating)
				{
					await Task.Delay(timeout);
					if (GPSDataCollector.IsUpdating)
					{
						return;
					}
					if (App.OBDReader.CurrentStatus != OBDDataReaderStatus.ConnectedToECU)
					{
						return;
					}
				}
				TaskAwaiter<PermissionStatus> taskAwaiter = Permissions.CheckStatusAsync<Permissions.LocationWhenInUse>().GetAwaiter();
				if (!taskAwaiter.IsCompleted)
				{
					await taskAwaiter;
					TaskAwaiter<PermissionStatus> taskAwaiter2;
					taskAwaiter = taskAwaiter2;
					taskAwaiter2 = default(TaskAwaiter<PermissionStatus>);
				}
				if (taskAwaiter.GetResult() == 3)
				{
					GPSDataCollector.IsUpdating = true;
					PID_GPSSpeed gpsSpeedPid = App.OBDReader.CurrentCarData.LiveDataPIDs.FirstOrDefault((PID x) => x is PID_GPSSpeed) as PID_GPSSpeed;
					PID_GPSAltitude gpsAltPid = App.OBDReader.CurrentCarData.LiveDataPIDs.FirstOrDefault((PID x) => x is PID_GPSAltitude) as PID_GPSAltitude;
					while (SharedSettings.Current.RecordLocationData && GPSDataCollector.IsUpdating)
					{
						try
						{
							Location location = await Geolocation.GetLocationAsync(new GeolocationRequest(5, timeout));
							if (location != null)
							{
								if (!double.IsFinite(location.Latitude) || !double.IsFinite(location.Longitude) || location.Latitude == 0.0 || location.Longitude == 0.0)
								{
									continue;
								}
								if (GPSDataCollector.LastLatitude != location.Latitude || GPSDataCollector.LastLongtitude != location.Longitude)
								{
									GPSDataCollector.LastLatitude = location.Latitude;
									GPSDataCollector.LastLongtitude = location.Longitude;
									GPSDataCollector.LastGPSTimeStamp = location.Timestamp.DateTime;
									DataRecorderV2 recorder = App.OBDReader.CurrentCarData.Recorder;
									if (recorder != null)
									{
										recorder.Record(GPSDataCollector.LastLatitude, GPSDataCollector.LastLongtitude);
									}
								}
								if (location.Speed != null && location.Speed != null && location.Speed.Value != GPSDataCollector.LastGPSSpeed)
								{
									GPSDataCollector.LastGPSSpeed = location.Speed.Value;
									PID_GPSSpeed pid_GPSSpeed = gpsSpeedPid;
									if (pid_GPSSpeed != null)
									{
										pid_GPSSpeed.ReportSpeedFromOtherSource(GPSDataCollector.LastGPSSpeed * 3.6);
									}
								}
								if (location.Altitude != null && location.Altitude != null && location.Altitude.Value != GPSDataCollector.LastAltitude)
								{
									GPSDataCollector.LastAltitude = location.Altitude.Value;
									gpsAltPid.ReportAltitudeFromOtherSource(GPSDataCollector.LastAltitude);
								}
							}
							Task.Delay(100).Wait();
						}
						catch (Exception)
						{
						}
					}
					GPSDataCollector.IsUpdating = false;
					gpsSpeedPid = null;
					gpsAltPid = null;
				}
				else
				{
					SharedSettings.Current.RecordLocationData = false;
				}
			}

			// Token: 0x06003C63 RID: 15459 RVA: 0x0014DCF9 File Offset: 0x0014BEF9
			internal bool <StartUpdates>b__4_1(PID x)
			{
				return x is PID_GPSSpeed;
			}

			// Token: 0x06003C64 RID: 15460 RVA: 0x0014DD0F File Offset: 0x0014BF0F
			internal bool <StartUpdates>b__4_2(PID x)
			{
				return x is PID_GPSAltitude;
			}

			// Token: 0x040024EC RID: 9452
			public static readonly GPSDataCollector.<>c <>9 = new GPSDataCollector.<>c();

			// Token: 0x040024ED RID: 9453
			public static Func<PID, bool> <>9__4_1;

			// Token: 0x040024EE RID: 9454
			public static Func<PID, bool> <>9__4_2;

			// Token: 0x040024EF RID: 9455
			public static Func<Task> <>9__4_0;

			// Token: 0x020006EE RID: 1774
			[StructLayout(LayoutKind.Auto)]
			private struct <<StartUpdates>b__4_0>d : IAsyncStateMachine
			{
				// Token: 0x06003C65 RID: 15461 RVA: 0x00318B60 File Offset: 0x00316D60
				void IAsyncStateMachine.MoveNext()
				{
					int num2;
					int num = num2;
					try
					{
						TaskAwaiter taskAwaiter3;
						TaskAwaiter<PermissionStatus> taskAwaiter5;
						switch (num)
						{
						case 0:
						{
							TaskAwaiter taskAwaiter4;
							taskAwaiter3 = taskAwaiter4;
							taskAwaiter4 = default(TaskAwaiter);
							num = (num2 = -1);
							break;
						}
						case 1:
							taskAwaiter5 = taskAwaiter2;
							taskAwaiter2 = default(TaskAwaiter<PermissionStatus>);
							num = (num2 = -1);
							goto IL_0105;
						case 2:
							IL_0199:
							try
							{
								TaskAwaiter<Location> taskAwaiter6;
								if (num != 2)
								{
									taskAwaiter6 = Geolocation.GetLocationAsync(new GeolocationRequest(5, timeout)).GetAwaiter();
									if (!taskAwaiter6.IsCompleted)
									{
										num = (num2 = 2);
										TaskAwaiter<Location> taskAwaiter7 = taskAwaiter6;
										this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<Location>, GPSDataCollector.<>c.<<StartUpdates>b__4_0>d>(ref taskAwaiter6, ref this);
										return;
									}
								}
								else
								{
									TaskAwaiter<Location> taskAwaiter7;
									taskAwaiter6 = taskAwaiter7;
									taskAwaiter7 = default(TaskAwaiter<Location>);
									num = (num2 = -1);
								}
								Location result = taskAwaiter6.GetResult();
								if (result != null)
								{
									if (!double.IsFinite(result.Latitude) || !double.IsFinite(result.Longitude) || result.Latitude == 0.0 || result.Longitude == 0.0)
									{
										goto IL_0390;
									}
									if (GPSDataCollector.LastLatitude != result.Latitude || GPSDataCollector.LastLongtitude != result.Longitude)
									{
										GPSDataCollector.LastLatitude = result.Latitude;
										GPSDataCollector.LastLongtitude = result.Longitude;
										GPSDataCollector.LastGPSTimeStamp = result.Timestamp.DateTime;
										DataRecorderV2 recorder = App.OBDReader.CurrentCarData.Recorder;
										if (recorder != null)
										{
											recorder.Record(GPSDataCollector.LastLatitude, GPSDataCollector.LastLongtitude);
										}
									}
									if (result.Speed != null && result.Speed != null && result.Speed.Value != GPSDataCollector.LastGPSSpeed)
									{
										GPSDataCollector.LastGPSSpeed = result.Speed.Value;
										PID_GPSSpeed pid_GPSSpeed = gpsSpeedPid;
										if (pid_GPSSpeed != null)
										{
											pid_GPSSpeed.ReportSpeedFromOtherSource(GPSDataCollector.LastGPSSpeed * 3.6);
										}
									}
									if (result.Altitude != null && result.Altitude != null && result.Altitude.Value != GPSDataCollector.LastAltitude)
									{
										GPSDataCollector.LastAltitude = result.Altitude.Value;
										gpsAltPid.ReportAltitudeFromOtherSource(GPSDataCollector.LastAltitude);
									}
								}
								Task.Delay(100).Wait();
							}
							catch (Exception)
							{
							}
							goto IL_0390;
						default:
							timeout = TimeSpan.FromSeconds(5.0);
							if (!GPSDataCollector.IsUpdating)
							{
								goto IL_00B2;
							}
							taskAwaiter3 = Task.Delay(timeout).GetAwaiter();
							if (!taskAwaiter3.IsCompleted)
							{
								num = (num2 = 0);
								TaskAwaiter taskAwaiter4 = taskAwaiter3;
								this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, GPSDataCollector.<>c.<<StartUpdates>b__4_0>d>(ref taskAwaiter3, ref this);
								return;
							}
							break;
						}
						taskAwaiter3.GetResult();
						if (GPSDataCollector.IsUpdating)
						{
							goto IL_03E2;
						}
						if (App.OBDReader.CurrentStatus != OBDDataReaderStatus.ConnectedToECU)
						{
							goto IL_03E2;
						}
						IL_00B2:
						taskAwaiter5 = Permissions.CheckStatusAsync<Permissions.LocationWhenInUse>().GetAwaiter();
						if (!taskAwaiter5.IsCompleted)
						{
							num = (num2 = 1);
							taskAwaiter2 = taskAwaiter5;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<PermissionStatus>, GPSDataCollector.<>c.<<StartUpdates>b__4_0>d>(ref taskAwaiter5, ref this);
							return;
						}
						IL_0105:
						if (taskAwaiter5.GetResult() != 3)
						{
							SharedSettings.Current.RecordLocationData = false;
							goto IL_03C7;
						}
						GPSDataCollector.IsUpdating = true;
						gpsSpeedPid = App.OBDReader.CurrentCarData.LiveDataPIDs.FirstOrDefault((PID x) => x is PID_GPSSpeed) as PID_GPSSpeed;
						gpsAltPid = App.OBDReader.CurrentCarData.LiveDataPIDs.FirstOrDefault((PID x) => x is PID_GPSAltitude) as PID_GPSAltitude;
						IL_0390:
						if (SharedSettings.Current.RecordLocationData && GPSDataCollector.IsUpdating)
						{
							goto IL_0199;
						}
						GPSDataCollector.IsUpdating = false;
						gpsSpeedPid = null;
						gpsAltPid = null;
						IL_03C7:;
					}
					catch (Exception ex)
					{
						num2 = -2;
						this.<>t__builder.SetException(ex);
						return;
					}
					IL_03E2:
					num2 = -2;
					this.<>t__builder.SetResult();
				}

				// Token: 0x06003C66 RID: 15462 RVA: 0x00318F98 File Offset: 0x00317198
				[DebuggerHidden]
				void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
				{
					this.<>t__builder.SetStateMachine(stateMachine);
				}

				// Token: 0x040024F0 RID: 9456
				public int <>1__state;

				// Token: 0x040024F1 RID: 9457
				public AsyncTaskMethodBuilder <>t__builder;

				// Token: 0x040024F2 RID: 9458
				private TimeSpan <timeout>5__2;

				// Token: 0x040024F3 RID: 9459
				private TaskAwaiter <>u__1;

				// Token: 0x040024F4 RID: 9460
				private TaskAwaiter<PermissionStatus> <>u__2;

				// Token: 0x040024F5 RID: 9461
				private PID_GPSSpeed <gpsSpeedPid>5__3;

				// Token: 0x040024F6 RID: 9462
				private PID_GPSAltitude <gpsAltPid>5__4;

				// Token: 0x040024F7 RID: 9463
				private TaskAwaiter<Location> <>u__3;
			}
		}
	}
}
