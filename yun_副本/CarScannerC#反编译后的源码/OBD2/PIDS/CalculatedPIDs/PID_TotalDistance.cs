using System;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using CarScannerXamarinForms.Settings;
using CarScannerXamarinForms.ViewModels;

namespace CarScannerXamarinForms.OBD2.PIDS.CalculatedPIDs
{
	// Token: 0x02000469 RID: 1129
	internal class PID_TotalDistance : CalculatedPIDV2
	{
		// Token: 0x06002EC7 RID: 11975 RVA: 0x00207078 File Offset: 0x00205278
		public PID_TotalDistance()
			: base(PID.GetResourceString("PID_TotalDistance"), "TOTAL_DISTANCE", UnitsHelper.Units.km, 0.0, 150.0, Roles.CALC_Distance)
		{
			this.sw = new Stopwatch();
			base.Id = 235;
		}

		// Token: 0x06002EC8 RID: 11976 RVA: 0x002070C5 File Offset: 0x002052C5
		public override void ResetValues()
		{
			this.S = 0.0;
			this.sw.Restart();
			base.ResetValues();
		}

		// Token: 0x06002EC9 RID: 11977 RVA: 0x002070E8 File Offset: 0x002052E8
		protected override bool Calculate(IPIDFloatValue speedPid, out double result)
		{
			if (!this.sw.IsRunning)
			{
				this.sw.Start();
				result = 0.0;
				return false;
			}
			if (SharedSettings.Current.FilterRPM300 && this.RPM != null && !SharedSettings.Current.FuelHybridCar && this.RPM.Value < 300.0)
			{
				result = base.Value;
				return false;
			}
			TimeSpan elapsed = this.sw.Elapsed;
			this.sw.Restart();
			if (elapsed.TotalSeconds > 15.0)
			{
				result = base.Value;
				return false;
			}
			double num = speedPid.Value * elapsed.TotalHours;
			if (double.IsFinite(num))
			{
				this.S += num;
			}
			result = this.S;
			DriveCycle.Current.RecordDistance(num, speedPid.TimeStamp);
			return true;
		}

		// Token: 0x06002ECA RID: 11978 RVA: 0x002071CC File Offset: 0x002053CC
		public override void Initialize()
		{
			this.S = 0.0;
			this.sw.Reset();
			IPIDFloatValue ipidfloatValue;
			if (SharedSettings.Current.UseGPSForFuelConsumption)
			{
				ipidfloatValue = (IPIDFloatValue)App.OBDReader.CurrentCarData.LiveDataPIDs.FirstOrDefault((PID x) => x is PID_GPSSpeed);
			}
			else
			{
				ipidfloatValue = App.OBDReader.CurrentCarData.FindPIDByRole(Roles.Speed);
				if (ipidfloatValue == null)
				{
					if (App.OBDReader.IsNissanConsult2Protocol)
					{
						ipidfloatValue = App.OBDReader.CurrentCarData.LiveDataPIDs.FirstOrDefault((PID x) => x.Command == "2212A1") as PIDWithFloatValueFormula;
					}
					else
					{
						ipidfloatValue = App.OBDReader.CurrentCarData.LiveDataPIDs.FirstOrDefault((PID x) => x.Id == 13) as PIDWithFloatValueFormula;
					}
					if (ipidfloatValue == null)
					{
						CustomPID customPID = CustomPIDViewModel.CurrentProfile.PidCollection.FirstOrDefault((CustomPID x) => x != null && ((IPIDFloatValue)x).Units == UnitsHelper.Units.kmh);
						if (customPID != null)
						{
							ipidfloatValue = customPID;
						}
					}
				}
			}
			this.requiredPIDs.Clear();
			if (SharedSettings.Current.FilterRPM300)
			{
				this.RPM = App.OBDReader.CurrentCarData.FindPIDByRole(Roles.RPM);
				if (this.RPM != null)
				{
					this.requiredPIDs.Add(this.RPM);
				}
			}
			if (ipidfloatValue != null)
			{
				base.DependencyPID = ipidfloatValue;
			}
			base.Initialize();
		}

		// Token: 0x04001A89 RID: 6793
		private Stopwatch sw;

		// Token: 0x04001A8A RID: 6794
		private double S;

		// Token: 0x04001A8B RID: 6795
		private IPIDFloatValue RPM;

		// Token: 0x0200046A RID: 1130
		[CompilerGenerated]
		[Serializable]
		private sealed class <>c
		{
			// Token: 0x06002ECB RID: 11979 RVA: 0x00207363 File Offset: 0x00205563
			// Note: this type is marked as 'beforefieldinit'.
			static <>c()
			{
			}

			// Token: 0x06002ECC RID: 11980 RVA: 0x00002050 File Offset: 0x00000250
			public <>c()
			{
			}

			// Token: 0x06002ECD RID: 11981 RVA: 0x0014DCF9 File Offset: 0x0014BEF9
			internal bool <Initialize>b__6_0(PID x)
			{
				return x is PID_GPSSpeed;
			}

			// Token: 0x06002ECE RID: 11982 RVA: 0x00202CD8 File Offset: 0x00200ED8
			internal bool <Initialize>b__6_1(PID x)
			{
				return x.Command == "2212A1";
			}

			// Token: 0x06002ECF RID: 11983 RVA: 0x0009AE64 File Offset: 0x00099064
			internal bool <Initialize>b__6_2(PID x)
			{
				return x.Id == 13;
			}

			// Token: 0x06002ED0 RID: 11984 RVA: 0x00202CEA File Offset: 0x00200EEA
			internal bool <Initialize>b__6_3(CustomPID x)
			{
				return x != null && ((IPIDFloatValue)x).Units == UnitsHelper.Units.kmh;
			}

			// Token: 0x04001A8C RID: 6796
			public static readonly PID_TotalDistance.<>c <>9 = new PID_TotalDistance.<>c();

			// Token: 0x04001A8D RID: 6797
			public static Func<PID, bool> <>9__6_0;

			// Token: 0x04001A8E RID: 6798
			public static Func<PID, bool> <>9__6_1;

			// Token: 0x04001A8F RID: 6799
			public static Func<PID, bool> <>9__6_2;

			// Token: 0x04001A90 RID: 6800
			public static Func<CustomPID, bool> <>9__6_3;
		}
	}
}
