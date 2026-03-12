using System;
using CarScannerXamarinForms.Settings;

namespace CarScannerXamarinForms.OBD2.PIDS.CalculatedPIDs
{
	// Token: 0x02000448 RID: 1096
	internal class PID_CalculatedFreeSpaceInFuelTank : CalculatedPIDV2
	{
		// Token: 0x06002E19 RID: 11801 RVA: 0x00202718 File Offset: 0x00200918
		public PID_CalculatedFreeSpaceInFuelTank()
			: base(Translate.GetString("PID_FreeSpaceInFuelTank"), "", UnitsHelper.Units.liters, 0.0, 80.0, Roles.FreeSpaceInTankCalculated)
		{
			base.ShortName = base.Name;
			base.Id = 901;
		}

		// Token: 0x06002E1A RID: 11802 RVA: 0x00202768 File Offset: 0x00200968
		public override void Initialize()
		{
			this.pidFuelLevelLiters = App.OBDReader.CurrentCarData.FindPIDByRole(Roles.FuelLevelInputLiters);
			base.DependencyPID = this.pidFuelLevelLiters;
			this.requiredPIDs.Clear();
			this.requiredPIDs.Add(this.pidFuelLevelLiters);
			base.Initialize();
		}

		// Token: 0x06002E1B RID: 11803 RVA: 0x002027BC File Offset: 0x002009BC
		protected override bool Calculate(IPIDFloatValue pid, out double result)
		{
			if (this.pidFuelLevelLiters != null && pid == this.pidFuelLevelLiters)
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
				result = num - this.pidFuelLevelLiters.Value;
				return true;
			}
			result = 0.0;
			return false;
		}

		// Token: 0x04001A00 RID: 6656
		private IPIDFloatValue pidFuelLevelLiters;
	}
}
