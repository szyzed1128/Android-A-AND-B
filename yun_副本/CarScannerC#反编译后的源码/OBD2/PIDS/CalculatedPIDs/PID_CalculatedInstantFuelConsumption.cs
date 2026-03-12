using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using CarScannerXamarinForms.Settings;
using CarScannerXamarinForms.ViewModels;

namespace CarScannerXamarinForms.OBD2.PIDS.CalculatedPIDs
{
	// Token: 0x0200044A RID: 1098
	internal class PID_CalculatedInstantFuelConsumption : CalculatedPIDV2
	{
		// Token: 0x06002E1F RID: 11807 RVA: 0x0020298C File Offset: 0x00200B8C
		public PID_CalculatedInstantFuelConsumption(PID_CalculatedInstantFuelRate fuelRate)
			: base(PID.GetResourceString("PID_CalculatedInstantFuelConsumption"), "INSTANT_FUEL_CONSUMPTION", UnitsHelper.Units.liters100km, 0.0, 50.0, Roles.CALC_InstantFuelConsumption)
		{
			base.Minimum = 0.0;
			base.Maximum = 50.0;
			this.Command = "INSTANT_FUEL_CONSUMPTION";
			base.ShortName = Translate.GetString("PID_CalculatedInstantFuelConsumption_Short");
			this.FuelRate = fuelRate;
			base.Id = 231;
		}

		// Token: 0x06002E20 RID: 11808 RVA: 0x00202A10 File Offset: 0x00200C10
		public override void Initialize()
		{
			List<PID> liveDataPIDs = App.OBDReader.CurrentCarData.LiveDataPIDs;
			this.requiredPIDs.Clear();
			this.lowPriorityRequiredPIDs.Clear();
			PID pid = liveDataPIDs.FirstOrDefault((PID x) => x is PID_GPSSpeed);
			if (SharedSettings.Current.UseGPSForFuelConsumption && SharedSettings.Current.UseGPS && pid != null && pid.IsAvailable)
			{
				this.VSS = pid as IPIDFloatValue;
			}
			else
			{
				this.VSS = App.OBDReader.CurrentCarData.FindPIDByRole(Roles.Speed);
				if (this.VSS == null)
				{
					if (App.OBDReader.IsNissanConsult2Protocol)
					{
						this.VSS = liveDataPIDs.FirstOrDefault((PID x) => x.Command == "2212A1") as PIDWithFloatValueFormula;
					}
					else
					{
						this.VSS = liveDataPIDs.FirstOrDefault((PID x) => x.Id == 13) as PIDWithFloatValueFormula;
					}
					if (this.VSS == null)
					{
						CustomPID customPID = CustomPIDViewModel.CurrentProfile.PidCollection.FirstOrDefault((CustomPID x) => x != null && ((IPIDFloatValue)x).Units == UnitsHelper.Units.kmh);
						if (customPID != null)
						{
							this.VSS = customPID;
						}
					}
				}
			}
			if (this.FuelRate != null && this.VSS != null && this.FuelRate.IsAvailable && this.VSS.IsAvailable)
			{
				this.requiredPIDs.Add(this.FuelRate);
				this.requiredPIDs.Add(this.VSS);
				base.DependencyPID = this.VSS;
			}
			else
			{
				this.IsAvailable = false;
			}
			base.Initialize();
			string text = ((this.VSS == null) ? "null" : string.Format("Id={0}:{1}:{2}", this.VSS.Id, this.VSS.Name, this.VSS.IsAvailable));
			string text2 = "\r\n[InstantFuelConsumption VSS=" + text + "]\r\n";
			App.OBDReader.DebugWrite(text2);
		}

		// Token: 0x06002E21 RID: 11809 RVA: 0x00202C3C File Offset: 0x00200E3C
		protected override bool Calculate(IPIDFloatValue pid, out double result)
		{
			if (Math.Abs(this.VSS.Value) < 1E-05)
			{
				result = double.PositiveInfinity;
			}
			else
			{
				double value = this.VSS.Value;
				double value2 = this.FuelRate.Value;
				double num = 100.0 / this.VSS.Value;
				if (double.IsFinite(num) && double.IsFinite(value2))
				{
					result = num * this.FuelRate.Value;
				}
				else
				{
					result = double.PositiveInfinity;
				}
			}
			return true;
		}

		// Token: 0x04001A02 RID: 6658
		private IPIDFloatValue VSS;

		// Token: 0x04001A03 RID: 6659
		private IPIDFloatValue FuelRate;

		// Token: 0x0200044B RID: 1099
		[CompilerGenerated]
		[Serializable]
		private sealed class <>c
		{
			// Token: 0x06002E22 RID: 11810 RVA: 0x00202CCC File Offset: 0x00200ECC
			// Note: this type is marked as 'beforefieldinit'.
			static <>c()
			{
			}

			// Token: 0x06002E23 RID: 11811 RVA: 0x00002050 File Offset: 0x00000250
			public <>c()
			{
			}

			// Token: 0x06002E24 RID: 11812 RVA: 0x0014DCF9 File Offset: 0x0014BEF9
			internal bool <Initialize>b__3_0(PID x)
			{
				return x is PID_GPSSpeed;
			}

			// Token: 0x06002E25 RID: 11813 RVA: 0x00202CD8 File Offset: 0x00200ED8
			internal bool <Initialize>b__3_1(PID x)
			{
				return x.Command == "2212A1";
			}

			// Token: 0x06002E26 RID: 11814 RVA: 0x0009AE64 File Offset: 0x00099064
			internal bool <Initialize>b__3_2(PID x)
			{
				return x.Id == 13;
			}

			// Token: 0x06002E27 RID: 11815 RVA: 0x00202CEA File Offset: 0x00200EEA
			internal bool <Initialize>b__3_3(CustomPID x)
			{
				return x != null && ((IPIDFloatValue)x).Units == UnitsHelper.Units.kmh;
			}

			// Token: 0x04001A04 RID: 6660
			public static readonly PID_CalculatedInstantFuelConsumption.<>c <>9 = new PID_CalculatedInstantFuelConsumption.<>c();

			// Token: 0x04001A05 RID: 6661
			public static Func<PID, bool> <>9__3_0;

			// Token: 0x04001A06 RID: 6662
			public static Func<PID, bool> <>9__3_1;

			// Token: 0x04001A07 RID: 6663
			public static Func<PID, bool> <>9__3_2;

			// Token: 0x04001A08 RID: 6664
			public static Func<CustomPID, bool> <>9__3_3;
		}
	}
}
