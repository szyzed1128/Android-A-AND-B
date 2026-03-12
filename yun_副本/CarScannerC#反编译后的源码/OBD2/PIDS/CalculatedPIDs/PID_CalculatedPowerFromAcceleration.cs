using System;
using System.Linq;
using System.Runtime.CompilerServices;
using CarScannerXamarinForms.Settings;

namespace CarScannerXamarinForms.OBD2.PIDS.CalculatedPIDs
{
	// Token: 0x0200044E RID: 1102
	public class PID_CalculatedPowerFromAcceleration : CalculatedPIDV2
	{
		// Token: 0x06002E47 RID: 11847 RVA: 0x002045EC File Offset: 0x002027EC
		public PID_CalculatedPowerFromAcceleration(IPIDFloatValue PID_CalculatedAccelerationFromSpeed)
			: base(PID.GetResourceString("PID_POWER_FROM_ACCELERATION"), "POWER_FROM_ACCELERATION", UnitsHelper.Units.kW, 0.0, 1.0, Roles.None)
		{
			base.Minimum = 0.0;
			base.Maximum = 200.0;
			this.Command = "POWER_FROM_ACCELERATION";
			base.ShortName = PID.GetResourceString("PID_POWER_FROM_ACCELERATION_SHORT");
			this.LowPriorityInterval = 500;
			this.ACCELERATION_FROM_SPEED = PID_CalculatedAccelerationFromSpeed;
			base.Id = 239;
		}

		// Token: 0x06002E48 RID: 11848 RVA: 0x00204688 File Offset: 0x00202888
		public override void Initialize()
		{
			this.FUEL_INPUT = App.OBDReader.CurrentCarData.FindPIDByRole(Roles.FuelLevelInputLiters, App.OBDReader.CurrentCarData.LiveDataPIDs.FirstOrDefault((PID x) => x.Id == 63) as IPIDFloatValue);
			this.BARO = App.OBDReader.CurrentCarData.FindPIDByRole(Roles.BARO, App.OBDReader.CurrentCarData.LiveDataPIDs.FirstOrDefault((PID x) => x.Id == 67) as IPIDFloatValue);
			this.AMBIENT_TEMP = App.OBDReader.CurrentCarData.FindPIDByRole(Roles.AMBIENT_TEMP, App.OBDReader.CurrentCarData.LiveDataPIDs.FirstOrDefault((PID x) => x.Id == 94) as IPIDFloatValue);
			this.lowPriorityRequiredPIDs.Add(this.FUEL_INPUT);
			this.lowPriorityRequiredPIDs.Add(this.BARO);
			this.lowPriorityRequiredPIDs.Add(this.AMBIENT_TEMP);
			this.SpeedPID = App.OBDReader.CurrentCarData.FindPIDByRole(Roles.Speed);
			this.requiredPIDs.Add(this.SpeedPID);
			this.requiredPIDs.Add(this.ACCELERATION_FROM_SPEED);
			base.DependencyPID = this.ACCELERATION_FROM_SPEED;
			base.Initialize();
		}

		// Token: 0x06002E49 RID: 11849 RVA: 0x00204800 File Offset: 0x00202A00
		protected override bool Calculate(IPIDFloatValue pid, out double result)
		{
			double num = this.GetEnginePower() + this.GetPowerLossDueToAirResistance() + this.GetPowerLossDueToTireRollingResistance();
			result = num / 1000.0;
			return true;
		}

		// Token: 0x06002E4A RID: 11850 RVA: 0x00204830 File Offset: 0x00202A30
		private double GetEnginePower()
		{
			return this.GetTotalWeight() * this.ACCELERATION_FROM_SPEED.Value * this.SpeedPID.Value * 1000.0 / 3600.0;
		}

		// Token: 0x06002E4B RID: 11851 RVA: 0x00204864 File Offset: 0x00202A64
		private double GetTotalWeight()
		{
			double num = SharedSettings.Current.CurbWeight;
			if (SharedSettings.Current.Use_km)
			{
				num *= 0.45359237;
			}
			num += this.GetDriverWeight();
			num -= this.GetFuelWeight();
			double num2 = SharedSettings.Current.PassengersWeight;
			if (SharedSettings.Current.Use_km)
			{
				num2 *= 0.45359237;
			}
			num += num2;
			double num3 = SharedSettings.Current.AdditionalWeight;
			if (SharedSettings.Current.Use_km)
			{
				num3 *= 0.45359237;
			}
			return num + num3;
		}

		// Token: 0x06002E4C RID: 11852 RVA: 0x002048F8 File Offset: 0x00202AF8
		private double GetPowerLossDueToAirResistance()
		{
			return 0.5 * this.GetAirDensity() * Math.Pow(this.SpeedPID.Value, 3.0) * SharedSettings.Current.DragCoefficient * SharedSettings.Current.DragArea;
		}

		// Token: 0x06002E4D RID: 11853 RVA: 0x00204945 File Offset: 0x00202B45
		private double GetPowerLossDueToTireRollingResistance()
		{
			return SharedSettings.Current.TireResistance * this.GetTotalWeight() * this.GRAV * this.SpeedPID.Value;
		}

		// Token: 0x06002E4E RID: 11854 RVA: 0x0020496C File Offset: 0x00202B6C
		private double GetAirDensity()
		{
			double num = SharedSettings.Current.BarometricPressure;
			double num2 = SharedSettings.Current.AmbientTemperature;
			if (!SharedSettings.Current.Use_celcium)
			{
				num2 = (num2 - 32.0) * 5.0 / 9.0;
			}
			if ((this.BARO as PID).IsAvailable)
			{
				num = SharedSettings.Current.BarometricPressure;
			}
			if ((this.AMBIENT_TEMP as PID).IsAvailable)
			{
				num2 = SharedSettings.Current.AmbientTemperature;
			}
			return PID_CalculatedPowerFromAcceleration.GetAirDensity(num, num2);
		}

		// Token: 0x06002E4F RID: 11855 RVA: 0x002049FC File Offset: 0x00202BFC
		private double GetFuelWeight()
		{
			double num;
			if ((this.FUEL_INPUT as PID).IsAvailable)
			{
				num = SharedSettings.Current.FuelTankCapacity - this.FUEL_INPUT.Value;
			}
			else
			{
				num = SharedSettings.Current.FuelTankCapacity * SharedSettings.Current.FuelInTankVolume / 100.0;
				if (!SharedSettings.Current.UseLitersForVolume)
				{
					if (SharedSettings.Current.UseUSGallon)
					{
						num *= 3.78541;
					}
					else
					{
						num *= 4.54609;
					}
				}
			}
			double num2 = 0.755;
			FuelTypes fuelType = SharedSettings.Current.FuelType;
			if (fuelType != FuelTypes.Gasoline)
			{
				if (fuelType == FuelTypes.Diesel)
				{
					num2 = 0.832;
				}
			}
			else
			{
				num2 = 0.755;
			}
			return num * num2;
		}

		// Token: 0x06002E50 RID: 11856 RVA: 0x00204AC7 File Offset: 0x00202CC7
		public static double GetAirDensity(double baro_pressure, double ambient_temp)
		{
			return baro_pressure * 1000.0 / (287.058 * (ambient_temp + 273.15));
		}

		// Token: 0x06002E51 RID: 11857 RVA: 0x00204AEC File Offset: 0x00202CEC
		public double GetDriverWeight()
		{
			double num = SharedSettings.Current.DriverWeight;
			if (SharedSettings.Current.Use_km)
			{
				num *= 0.45359237;
			}
			return num - 75.0;
		}

		// Token: 0x04001A33 RID: 6707
		private IPIDFloatValue SpeedPID;

		// Token: 0x04001A34 RID: 6708
		private IPIDFloatValue BARO;

		// Token: 0x04001A35 RID: 6709
		private IPIDFloatValue AMBIENT_TEMP;

		// Token: 0x04001A36 RID: 6710
		private IPIDFloatValue FUEL_INPUT;

		// Token: 0x04001A37 RID: 6711
		private IPIDFloatValue ACCELERATION_FROM_SPEED;

		// Token: 0x04001A38 RID: 6712
		private double GRAV = 6.672E-08;

		// Token: 0x0200044F RID: 1103
		[CompilerGenerated]
		[Serializable]
		private sealed class <>c
		{
			// Token: 0x06002E52 RID: 11858 RVA: 0x00204B29 File Offset: 0x00202D29
			// Note: this type is marked as 'beforefieldinit'.
			static <>c()
			{
			}

			// Token: 0x06002E53 RID: 11859 RVA: 0x00002050 File Offset: 0x00000250
			public <>c()
			{
			}

			// Token: 0x06002E54 RID: 11860 RVA: 0x00204B35 File Offset: 0x00202D35
			internal bool <Initialize>b__7_0(PID x)
			{
				return x.Id == 63;
			}

			// Token: 0x06002E55 RID: 11861 RVA: 0x002026F1 File Offset: 0x002008F1
			internal bool <Initialize>b__7_1(PID x)
			{
				return x.Id == 67;
			}

			// Token: 0x06002E56 RID: 11862 RVA: 0x002026FD File Offset: 0x002008FD
			internal bool <Initialize>b__7_2(PID x)
			{
				return x.Id == 94;
			}

			// Token: 0x04001A39 RID: 6713
			public static readonly PID_CalculatedPowerFromAcceleration.<>c <>9 = new PID_CalculatedPowerFromAcceleration.<>c();

			// Token: 0x04001A3A RID: 6714
			public static Func<PID, bool> <>9__7_0;

			// Token: 0x04001A3B RID: 6715
			public static Func<PID, bool> <>9__7_1;

			// Token: 0x04001A3C RID: 6716
			public static Func<PID, bool> <>9__7_2;
		}
	}
}
