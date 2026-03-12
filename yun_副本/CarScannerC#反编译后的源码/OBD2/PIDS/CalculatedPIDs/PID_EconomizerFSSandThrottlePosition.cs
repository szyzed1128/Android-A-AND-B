using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using CarScannerXamarinForms.Settings;
using CarScannerXamarinForms.ViewModels;

namespace CarScannerXamarinForms.OBD2.PIDS.CalculatedPIDs
{
	// Token: 0x02000456 RID: 1110
	public class PID_EconomizerFSSandThrottlePosition : CalculatedPIDV2
	{
		// Token: 0x06002E6D RID: 11885 RVA: 0x0020539C File Offset: 0x0020359C
		public PID_EconomizerFSSandThrottlePosition(int ECU_ID)
			: base(PID.GetResourceString("PID_EconmizerFSSandThrottlePosition"), "ECONOMIZER", UnitsHelper.Units.None, 0.0, 1.0, Roles.None)
		{
			this.ECU_ID = 0;
			base.Minimum = 0.0;
			base.Maximum = 2.0;
			this.Command = "ECONOMIZER";
			base.Id = 229;
		}

		// Token: 0x06002E6E RID: 11886 RVA: 0x00205410 File Offset: 0x00203610
		protected override bool Calculate(IPIDFloatValue pid, out double result)
		{
			if (pid == this.PercentIndicatorPID)
			{
				if (this.FSSPid.Value[this.ECU_ID] == PID0103_FuelSystemStatus.FuelSystemStatuses.OpenLoopDueToInsufficientEngineTemperature || this.FSSPid.Value[this.ECU_ID] == PID0103_FuelSystemStatus.FuelSystemStatuses.OpenLoopDueToSystemFailure)
				{
					result = -1.0;
					return true;
				}
				if (this.FSSPid.Value[this.ECU_ID] == PID0103_FuelSystemStatus.FuelSystemStatuses.ClosedLoopOK || this.FSSPid.Value[this.ECU_ID] == PID0103_FuelSystemStatus.FuelSystemStatuses.ClosedLoopButFail)
				{
					result = 1.0;
					return true;
				}
				if (this.FSSPid.Value[0] != PID0103_FuelSystemStatus.FuelSystemStatuses.OpenLoopDueToEngineLoadOrFuelCut || this.FSSPid.Value[1] == PID0103_FuelSystemStatus.FuelSystemStatuses.ClosedLoopOK)
				{
					result = 1.0;
					return true;
				}
				if (this.FSSPid.Value[1] == PID0103_FuelSystemStatus.FuelSystemStatuses.OpenLoopDueToEngineLoadOrFuelCut || this.FSSPid.Value[1] == PID0103_FuelSystemStatus.FuelSystemStatuses.None)
				{
					if (this.PercentIndicatorPID.Value >= 20.0)
					{
						result = 2.0;
						return true;
					}
					result = 0.0;
					return true;
				}
			}
			if (pid != this.customZeroPid)
			{
				result = 1.0;
				return false;
			}
			if (SharedSettings.Current.ZeroConsumptionWhenZero)
			{
				if (Math.Abs(this.customZeroPid.Value) < 5E-324)
				{
					result = 0.0;
					return true;
				}
				result = 1.0;
				return true;
			}
			else
			{
				if (this.customZeroPid.Value > 0.0)
				{
					result = 0.0;
					return true;
				}
				result = 1.0;
				return true;
			}
		}

		// Token: 0x06002E6F RID: 11887 RVA: 0x0020559C File Offset: 0x0020379C
		public override void Initialize()
		{
			List<PID> liveDataPIDs = App.OBDReader.CurrentCarData.LiveDataPIDs;
			this.FSSPid = null;
			this.PercentIndicatorPID = null;
			this.IsAvailable = false;
			base.SetValue(1.0);
			this.requiredPIDs.Clear();
			if (SharedSettings.Current.ZeroConsumptionPIDId > 0)
			{
				this.customZeroPid = CustomPIDViewModel.CurrentProfile.PidCollection.FirstOrDefault((CustomPID x) => x.Id == SharedSettings.Current.ZeroConsumptionPIDId);
				if (this.customZeroPid == null)
				{
					this.customZeroPid = CustomPIDViewModel.CurrentCustom.PidCollection.FirstOrDefault((CustomPID x) => x.Id == SharedSettings.Current.ZeroConsumptionPIDId);
				}
				if (this.customZeroPid == null)
				{
					this.customZeroPid = App.OBDReader.CurrentCarData.LiveDataPIDs.FirstOrDefault((PID x) => x.Id == SharedSettings.Current.ZeroConsumptionPIDId) as IPIDFloatValue;
				}
				if (this.customZeroPid is PID_EconomizerFSSandThrottlePosition)
				{
					this.customZeroPid = null;
				}
			}
			this.FSSPid = liveDataPIDs.FirstOrDefault((PID x) => x is PID0103_FuelSystemStatus) as PID0103_FuelSystemStatus;
			this.PercentIndicatorPID = null;
			if (SharedSettings.Current.FuelFlowCalculationScheme == FuelFlowCalculationSchemes.LOAD_ABS)
			{
				IPIDFloatValue ipidfloatValue = App.OBDReader.CurrentCarData.FindPIDByRole(Roles.LOAD_ABS);
				if (ipidfloatValue.IsAvailable)
				{
					this.PercentIndicatorPID = ipidfloatValue;
				}
			}
			if (this.PercentIndicatorPID == null)
			{
				IPIDFloatValue ipidfloatValue2 = App.OBDReader.CurrentCarData.FindPIDByRole(Roles.LOAD_PCT);
				IPIDFloatValue ipidfloatValue3 = App.OBDReader.CurrentCarData.FindPIDByRole(Roles.AcceleratorPedalPosition);
				IPIDFloatValue ipidfloatValue4 = App.OBDReader.CurrentCarData.FindPIDByRole(Roles.Throttle);
				IPIDFloatValue ipidfloatValue5 = App.OBDReader.CurrentCarData.FindPIDByRole(Roles.LOAD_ABS);
				IPIDFloatValue[] array = new IPIDFloatValue[] { ipidfloatValue4, ipidfloatValue2, ipidfloatValue5, ipidfloatValue3 };
				this.PercentIndicatorPID = array.FirstOrDefault((IPIDFloatValue x) => x != null && x.IsAvailable);
			}
			if (SharedSettings.Current.UseCustomPIDForZeroConsumption && this.customZeroPid != null)
			{
				this.requiredPIDs.Add(this.customZeroPid);
				base.DependencyPID = this.customZeroPid;
				this.IsAvailable = true;
			}
			else if (this.FSSPid != null && this.PercentIndicatorPID != null && this.FSSPid.IsAvailable && this.PercentIndicatorPID.IsAvailable)
			{
				this.requiredPIDs.Add(this.FSSPid);
				this.requiredPIDs.Add(this.PercentIndicatorPID);
				base.DependencyPID = this.PercentIndicatorPID;
				this.IsAvailable = true;
			}
			else
			{
				this.IsAvailable = false;
			}
			base.Initialize();
		}

		// Token: 0x04001A4D RID: 6733
		private int ECU_ID;

		// Token: 0x04001A4E RID: 6734
		private IPIDFloatValue PercentIndicatorPID;

		// Token: 0x04001A4F RID: 6735
		private IPIDFloatValue customZeroPid;

		// Token: 0x04001A50 RID: 6736
		private PID0103_FuelSystemStatus FSSPid;

		// Token: 0x02000457 RID: 1111
		[CompilerGenerated]
		[Serializable]
		private sealed class <>c
		{
			// Token: 0x06002E70 RID: 11888 RVA: 0x00205869 File Offset: 0x00203A69
			// Note: this type is marked as 'beforefieldinit'.
			static <>c()
			{
			}

			// Token: 0x06002E71 RID: 11889 RVA: 0x00002050 File Offset: 0x00000250
			public <>c()
			{
			}

			// Token: 0x06002E72 RID: 11890 RVA: 0x00205875 File Offset: 0x00203A75
			internal bool <Initialize>b__6_0(CustomPID x)
			{
				return x.Id == SharedSettings.Current.ZeroConsumptionPIDId;
			}

			// Token: 0x06002E73 RID: 11891 RVA: 0x00205875 File Offset: 0x00203A75
			internal bool <Initialize>b__6_1(CustomPID x)
			{
				return x.Id == SharedSettings.Current.ZeroConsumptionPIDId;
			}

			// Token: 0x06002E74 RID: 11892 RVA: 0x00205875 File Offset: 0x00203A75
			internal bool <Initialize>b__6_2(PID x)
			{
				return x.Id == SharedSettings.Current.ZeroConsumptionPIDId;
			}

			// Token: 0x06002E75 RID: 11893 RVA: 0x00205889 File Offset: 0x00203A89
			internal bool <Initialize>b__6_3(PID x)
			{
				return x is PID0103_FuelSystemStatus;
			}

			// Token: 0x06002E76 RID: 11894 RVA: 0x00205894 File Offset: 0x00203A94
			internal bool <Initialize>b__6_4(IPIDFloatValue x)
			{
				return x != null && x.IsAvailable;
			}

			// Token: 0x04001A51 RID: 6737
			public static readonly PID_EconomizerFSSandThrottlePosition.<>c <>9 = new PID_EconomizerFSSandThrottlePosition.<>c();

			// Token: 0x04001A52 RID: 6738
			public static Func<CustomPID, bool> <>9__6_0;

			// Token: 0x04001A53 RID: 6739
			public static Func<CustomPID, bool> <>9__6_1;

			// Token: 0x04001A54 RID: 6740
			public static Func<PID, bool> <>9__6_2;

			// Token: 0x04001A55 RID: 6741
			public static Func<PID, bool> <>9__6_3;

			// Token: 0x04001A56 RID: 6742
			public static Func<IPIDFloatValue, bool> <>9__6_4;
		}
	}
}
