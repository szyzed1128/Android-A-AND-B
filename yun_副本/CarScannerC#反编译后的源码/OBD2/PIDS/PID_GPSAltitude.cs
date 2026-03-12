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
	// Token: 0x020003D5 RID: 981
	public class PID_GPSAltitude : SensorPID
	{
		// Token: 0x060027C9 RID: 10185 RVA: 0x001E8D3C File Offset: 0x001E6F3C
		public PID_GPSAltitude()
			: base(PID.GetResourceString("PID_GPSAltitude"), "GPS_ALTITUDE", UnitsHelper.Units.meters)
		{
			base.Minimum = 0.0;
			base.Maximum = 150.0;
			this.Command = "GPS_ALTITUDE";
			base.Role = Roles.GPS_Altitude;
		}

		// Token: 0x170011A8 RID: 4520
		// (get) Token: 0x060027CA RID: 10186 RVA: 0x001E8D94 File Offset: 0x001E6F94
		// (set) Token: 0x060027CB RID: 10187 RVA: 0x001E8DEC File Offset: 0x001E6FEC
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

		// Token: 0x060027CC RID: 10188 RVA: 0x001E8DF5 File Offset: 0x001E6FF5
		public override void Initialize()
		{
			this.CheckIsEnabled();
		}

		// Token: 0x060027CD RID: 10189 RVA: 0x001E8E00 File Offset: 0x001E7000
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

		// Token: 0x060027CE RID: 10190 RVA: 0x001E8E37 File Offset: 0x001E7037
		private void Current_PositionChanged(object sender, PositionEventArgs e)
		{
			this.Value = e.Position.Altitude;
		}

		// Token: 0x060027CF RID: 10191 RVA: 0x001E8E4A File Offset: 0x001E704A
		public void ReportAltitudeFromOtherSource(double value)
		{
			this.Value = value;
		}

		// Token: 0x060027D0 RID: 10192 RVA: 0x001E8E54 File Offset: 0x001E7054
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

		// Token: 0x060027D1 RID: 10193 RVA: 0x001E8E8C File Offset: 0x001E708C
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

		// Token: 0x04001647 RID: 5703
		private volatile bool isRunning;

		// Token: 0x020003D6 RID: 982
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <CheckIsEnabled>d__6 : IAsyncStateMachine
		{
			// Token: 0x060027D2 RID: 10194 RVA: 0x001E8EC4 File Offset: 0x001E70C4
			void IAsyncStateMachine.MoveNext()
			{
				PID_GPSAltitude pid_GPSAltitude = this;
				try
				{
					if (PlatformHelper.IsAndroid && !PlatformHelper.DroidService.HasLocationPermission)
					{
						pid_GPSAltitude.IsAvailable = false;
					}
					else
					{
						try
						{
							bool isGeolocationAvailable = CrossGeolocator.Current.IsGeolocationAvailable;
							bool isGeolocationEnabled = CrossGeolocator.Current.IsGeolocationEnabled;
							pid_GPSAltitude.IsAvailable = isGeolocationAvailable && isGeolocationEnabled;
							if (pid_GPSAltitude.IsAvailable)
							{
								if (!LiveDataPIDModel._PIDCollection.Contains(pid_GPSAltitude))
								{
									LiveDataPIDModel._PIDCollection.Add(pid_GPSAltitude);
								}
							}
							else if (LiveDataPIDModel._PIDCollection.Contains(pid_GPSAltitude))
							{
								LiveDataPIDModel._PIDCollection.Remove(pid_GPSAltitude);
							}
						}
						catch
						{
							pid_GPSAltitude.IsAvailable = false;
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

			// Token: 0x060027D3 RID: 10195 RVA: 0x001E8FA8 File Offset: 0x001E71A8
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x04001648 RID: 5704
			public int <>1__state;

			// Token: 0x04001649 RID: 5705
			public AsyncVoidMethodBuilder <>t__builder;

			// Token: 0x0400164A RID: 5706
			public PID_GPSAltitude <>4__this;
		}

		// Token: 0x020003D7 RID: 983
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <Start>d__9 : IAsyncStateMachine
		{
			// Token: 0x060027D4 RID: 10196 RVA: 0x001E8FB8 File Offset: 0x001E71B8
			void IAsyncStateMachine.MoveNext()
			{
				PID_GPSAltitude pid_GPSAltitude = this;
				try
				{
					if (!PlatformHelper.IsAndroid || PlatformHelper.DroidService.HasLocationPermission)
					{
						try
						{
							if (pid_GPSAltitude.IsAvailable && !pid_GPSAltitude.isRunning && !CrossGeolocator.Current.IsListening)
							{
								CrossGeolocator.Current.DesiredAccuracy = 1.0;
								CrossGeolocator.Current.PositionChanged -= pid_GPSAltitude.Current_PositionChanged;
								CrossGeolocator.Current.PositionChanged += pid_GPSAltitude.Current_PositionChanged;
								CrossGeolocator.Current.StartListeningAsync(TimeSpan.FromMilliseconds(50.0), 1.0, false, new ListenerSettings
								{
									ActivityType = 1,
									AllowBackgroundUpdates = false,
									PauseLocationUpdatesAutomatically = false,
									DeferLocationUpdates = false,
									ListenForSignificantChanges = false
								});
								pid_GPSAltitude.isRunning = true;
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

			// Token: 0x060027D5 RID: 10197 RVA: 0x001E90EC File Offset: 0x001E72EC
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x0400164B RID: 5707
			public int <>1__state;

			// Token: 0x0400164C RID: 5708
			public AsyncVoidMethodBuilder <>t__builder;

			// Token: 0x0400164D RID: 5709
			public PID_GPSAltitude <>4__this;
		}

		// Token: 0x020003D8 RID: 984
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <Stop>d__10 : IAsyncStateMachine
		{
			// Token: 0x060027D6 RID: 10198 RVA: 0x001E90FC File Offset: 0x001E72FC
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				PID_GPSAltitude pid_GPSAltitude = this;
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
									this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<bool>, PID_GPSAltitude.<Stop>d__10>(ref taskAwaiter, ref this);
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
						pid_GPSAltitude.isRunning = false;
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

			// Token: 0x060027D7 RID: 10199 RVA: 0x001E91EC File Offset: 0x001E73EC
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x0400164E RID: 5710
			public int <>1__state;

			// Token: 0x0400164F RID: 5711
			public AsyncVoidMethodBuilder <>t__builder;

			// Token: 0x04001650 RID: 5712
			public PID_GPSAltitude <>4__this;

			// Token: 0x04001651 RID: 5713
			private TaskAwaiter<bool> <>u__1;
		}
	}
}
