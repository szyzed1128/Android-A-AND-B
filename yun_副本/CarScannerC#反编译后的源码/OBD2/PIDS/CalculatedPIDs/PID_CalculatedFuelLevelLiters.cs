using System;
using CarScannerXamarinForms.Settings;

namespace CarScannerXamarinForms.OBD2.PIDS.CalculatedPIDs
{
	// Token: 0x02000449 RID: 1097
	internal class PID_CalculatedFuelLevelLiters : CalculatedPIDV2
	{
		// Token: 0x06002E1C RID: 11804 RVA: 0x00202848 File Offset: 0x00200A48
		public PID_CalculatedFuelLevelLiters()
			: base(Translate.GetString("PID_012F") + " (V)", "", UnitsHelper.Units.liters, 0.0, 80.0, Roles.FuelLevelInputLiters)
		{
			base.ShortName = base.Name;
			base.Id = 63;
		}

		// Token: 0x06002E1D RID: 11805 RVA: 0x002028A0 File Offset: 0x00200AA0
		public override void Initialize()
		{
			this.pidFuelPercent = App.OBDReader.CurrentCarData.FindPIDByRole(Roles.FuelLevelInputPercent);
			base.DependencyPID = this.pidFuelPercent;
			this.requiredPIDs.Clear();
			this.requiredPIDs.Add(this.pidFuelPercent);
			base.Initialize();
		}

		// Token: 0x06002E1E RID: 11806 RVA: 0x002028F4 File Offset: 0x00200AF4
		protected override bool Calculate(IPIDFloatValue pid, out double result)
		{
			if (this.pidFuelPercent != null && pid == this.pidFuelPercent)
			{
				double num;
				if (SharedSettings.Current.UseLitersForVolume)
				{
					num = SharedSettings.Current.FuelTankCapacity;
				}
				else if (SharedSettings.Current.UseUSGallon)
				{
					num = SharedSettings.Current.FuelTankCapacity * 3.78541178;
				}
				else
				{
					num = SharedSettings.Current.FuelTankCapacity * 4.5461;
				}
				result = num * this.pidFuelPercent.Value / 100.0;
				return true;
			}
			result = 0.0;
			return false;
		}

		// Token: 0x04001A01 RID: 6657
		private IPIDFloatValue pidFuelPercent;
	}
}
