using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using CarScannerXamarinForms.PlatformAdapters;
using CarScannerXamarinForms.Settings;
using CarScannerXamarinForms.ViewModels;
using Plugin.Geolocator;
using Plugin.Geolocator.Abstractions;

namespace CarScannerXamarinForms.OBD2.PIDS
{
	// Token: 0x020003D9 RID: 985
	public class PID_GPSSpeed : SensorPID
	{
		// Token: 0x060027D8 RID: 10200 RVA: 0x001E91FC File Offset: 0x001E73FC
		public PID_GPSSpeed()
			: base(PID.GetResourceString("PID_GPSSpeed"), "GPS_SPEED", UnitsHelper.Units.kmh)
		{
			base.Minimum = 0.0;
			base.Maximum = 150.0;
			this.Command = "GPS_SPEED";
			base.Role = Roles.GPS_Speed;
		}

		// Token: 0x170011A9 RID: 4521
		// (get) Token: 0x060027D9 RID: 10201 RVA: 0x001E9250 File Offset: 0x001E7450
		// (set) Token: 0x060027DA RID: 10202 RVA: 0x001E8DEC File Offset: 0x001E6FEC
		public override bool IsAvailable
		{
			get
			{
				if (PlatformHelper.IsAndroid && !PlatformHelper.DroidService.HasLocationPermission)
				{
					return false;
				}
				bool flag;
				try
				{
					if (!CrossGeolocator.IsSupported)
					{
						flag = false;
					}
					else if (!SharedSettings.Current.UseGPS)
					{
						flag = false;
					}
					else
					{
						flag = true;
					}
				}
				catch (Exception)
				{
					flag = false;
				}
				return flag;
			}
			set
			{
				base.IsAvailable = value;
			}
		}

		// Token: 0x060027DB RID: 10203 RVA: 0x001E92A8 File Offset: 0x001E74A8
		public override void Initialize()
		{
			this.CheckIsEnabled();
		}

		// Token: 0x060027DC RID: 10204 RVA: 0x001E92B0 File Offset: 0x001E74B0
		private async void CheckIsEnabled()
		{
			if (PlatformHelper.IsAndroid && !PlatformHelper.DroidService.HasLocationPermission)
			{
				this.IsAvailable = false;
			}
			else
			{
				try
				{
					bool isGeolocationAvailable = CrossGeolocator.Current.IsGeolocationAvailable;
					bool isGeolocationEnabled = CrossGeolocator.Current.IsGeolocationEnabled;
					this.IsAvailable = isGeolocationAvailable && isGeolocationEnabled;
					if (this.IsAvailable)
					{
						if (!LiveDataPIDModel._PIDCollection.Contains(this))
						{
							LiveDataPIDModel._PIDCollection.Add(this);
						}
					}
					else if (LiveDataPIDModel._PIDCollection.Contains(this))
					{
						LiveDataPIDModel._PIDCollection.Remove(this);
					}
				}
				catch
				{
					this.IsAvailable = false;
				}
			}
		}

		// Token: 0x060027DD RID: 10205 RVA: 0x001E92E7 File Offset: 0x001E74E7
		private void Current_PositionChanged(object sender, PositionEventArgs e)
		{
			this.Value = e.Position.Speed * 3.6;
			SpeedCalibrationModelV2 instance = SpeedCalibrationModelV2.Instance;
			if (instance == null)
			{
				return;
			}
			instance.RecordGPSSpeed(this.Value, base.TimeStamp);
		}

		// Token: 0x060027DE RID: 10206 RVA: 0x001E931F File Offset: 0x001E751F
		public void ReportSpeedFromOtherSource(double value)
		{
			this.Value = value;
			SpeedCalibrationModelV2 instance = SpeedCalibrationModelV2.Instance;
			if (instance == null)
			{
				return;
			}
			instance.RecordGPSSpeed(this.Value, base.TimeStamp);
		}

		// Token: 0x060027DF RID: 10207 RVA: 0x001E9344 File Offset: 0x001E7544
		public override async void Start()
		{
			if (!PlatformHelper.IsAndroid || PlatformHelper.DroidService.HasLocationPermission)
			{
				try
				{
					if (this.IsAvailable && !this.isRunning && !CrossGeolocator.Current.IsListening)
					{
						CrossGeolocator.Current.DesiredAccuracy = 1.0;
						CrossGeolocator.Current.PositionChanged -= this.Current_PositionChanged;
						CrossGeolocator.Current.PositionChanged += this.Current_PositionChanged;
						CrossGeolocator.Current.StartListeningAsync(TimeSpan.FromMilliseconds(50.0), 1.0, false, new ListenerSettings
						{
							ActivityType = 1,
							AllowBackgroundUpdates = false,
							PauseLocationUpdatesAutomatically = false,
							DeferLocationUpdates = false,
							ListenForSignificantChanges = false
						});
						this.isRunning = true;
					}
				}
				catch (Exception)
				{
				}
			}
		}

		// Token: 0x060027E0 RID: 10208 RVA: 0x001E937C File Offset: 0x001E757C
		public override async void Stop()
		{
			if (!PlatformHelper.IsAndroid || PlatformHelper.DroidService.HasLocationPermission)
			{
				try
				{
					await CrossGeolocator.Current.StopListeningAsync();
				}
				catch
				{
				}
				this.isRunning = false;
			}
		}

		// Token: 0x04001652 RID: 5714
		private volatile bool isRunning;

		// Token: 0x020003DA RID: 986
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <CheckIsEnabled>d__6 : IAsyncStateMachine
		{
			// Token: 0x060027E1 RID: 10209 RVA: 0x001E93B4 File Offset: 0x001E75B4
			void IAsyncStateMachine.MoveNext()
			{
				PID_GPSSpeed pid_GPSSpeed = this;
				try
				{
					if (PlatformHelper.IsAndroid && !PlatformHelper.DroidService.HasLocationPermission)
					{
						pid_GPSSpeed.IsAvailable = false;
					}
					else
					{
						try
						{
							bool isGeolocationAvailable = CrossGeolocator.Current.IsGeolocationAvailable;
							bool isGeolocationEnabled = CrossGeolocator.Current.IsGeolocationEnabled;
							pid_GPSSpeed.IsAvailable = isGeolocationAvailable && isGeolocationEnabled;
							if (pid_GPSSpeed.IsAvailable)
							{
								if (!LiveDataPIDModel._PIDCollection.Contains(pid_GPSSpeed))
								{
									LiveDataPIDModel._PIDCollection.Add(pid_GPSSpeed);
								}
							}
							else if (LiveDataPIDModel._PIDCollection.Contains(pid_GPSSpeed))
							{
								LiveDataPIDModel._PIDCollection.Remove(pid_GPSSpeed);
							}
						}
						catch
						{
							pid_GPSSpeed.IsAvailable = false;
						}
					}
				}
				catch (Exception ex)
				{
					this.<>1__state = -2;
					this.<>t__builder.SetException(ex);
					return;
				}
				this.<>1__state = -2;
				this.<>t__builder.SetResult();
			}

			// Token: 0x060027E2 RID: 10210 RVA: 0x001E9498 File Offset: 0x001E7698
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x04001653 RID: 5715
			public int <>1__state;

			// Token: 0x04001654 RID: 5716
			public AsyncVoidMethodBuilder <>t__builder;

			// Token: 0x04001655 RID: 5717
			public PID_GPSSpeed <>4__this;
		}

		// Token: 0x020003DB RID: 987
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <Start>d__9 : IAsyncStateMachine
		{
			// Token: 0x060027E3 RID: 10211 RVA: 0x001E94A8 File Offset: 0x001E76A8
			void IAsyncStateMachine.MoveNext()
			{
				PID_GPSSpeed pid_GPSSpeed = this;
				try
				{
					if (!PlatformHelper.IsAndroid || PlatformHelper.DroidService.HasLocationPermission)
					{
						try
						{
							if (pid_GPSSpeed.IsAvailable && !pid_GPSSpeed.isRunning && !CrossGeolocator.Current.IsListening)
							{
								CrossGeolocator.Current.DesiredAccuracy = 1.0;
								CrossGeolocator.Current.PositionChanged -= pid_GPSSpeed.Current_PositionChanged;
								CrossGeolocator.Current.PositionChanged += pid_GPSSpeed.Current_PositionChanged;
								CrossGeolocator.Current.StartListeningAsync(TimeSpan.FromMilliseconds(50.0), 1.0, false, new ListenerSettings
								{
									ActivityType = 1,
									AllowBackgroundUpdates = false,
									PauseLocationUpdatesAutomatically = false,
									DeferLocationUpdates = false,
									ListenForSignificantChanges = false
								});
								pid_GPSSpeed.isRunning = true;
							}
						}
						catch (Exception)
						{
						}
					}
				}
				catch (Exception ex)
				{
					this.<>1__state = -2;
					this.<>t__builder.SetException(ex);
					return;
				}
				this.<>1__state = -2;
				this.<>t__builder.SetResult();
			}

			// Token: 0x060027E4 RID: 10212 RVA: 0x001E95DC File Offset: 0x001E77DC
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x04001656 RID: 5718
			public int <>1__state;

			// Token: 0x04001657 RID: 5719
			public AsyncVoidMethodBuilder <>t__builder;

			// Token: 0x04001658 RID: 5720
			public PID_GPSSpeed <>4__this;
		}

		// Token: 0x020003DC RID: 988
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <Stop>d__10 : IAsyncStateMachine
		{
			// Token: 0x060027E5 RID: 10213 RVA: 0x001E95EC File Offset: 0x001E77EC
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				PID_GPSSpeed pid_GPSSpeed = this;
				try
				{
					if (num == 0 || !PlatformHelper.IsAndroid || PlatformHelper.DroidService.HasLocationPermission)
					{
						try
						{
							TaskAwaiter<bool> taskAwaiter;
							if (num != 0)
							{
								taskAwaiter = CrossGeolocator.Current.StopListeningAsync().GetAwaiter();
								if (!taskAwaiter.IsCompleted)
								{
									num2 = 0;
									TaskAwaiter<bool> taskAwaiter2 = taskAwaiter;
									this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<bool>, PID_GPSSpeed.<Stop>d__10>(ref taskAwaiter, ref this);
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
							taskAwaiter.GetResult();
						}
						catch
						{
						}
						pid_GPSSpeed.isRunning = false;
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

			// Token: 0x060027E6 RID: 10214 RVA: 0x001E96DC File Offset: 0x001E78DC
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x04001659 RID: 5721
			public int <>1__state;

			// Token: 0x0400165A RID: 5722
			public AsyncVoidMethodBuilder <>t__builder;

			// Token: 0x0400165B RID: 5723
			public PID_GPSSpeed <>4__this;

			// Token: 0x0400165C RID: 5724
			private TaskAwaiter<bool> <>u__1;
		}
	}
}
