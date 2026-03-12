using System;
using System.Runtime.CompilerServices;
using CarScannerXamarinForms.OBD2.PIDS;
using CarScannerXamarinForms.Settings;

namespace CarScannerXamarinForms.ViewModels
{
	// Token: 0x02000712 RID: 1810
	public class DayDriveCycle
	{
		// Token: 0x17001408 RID: 5128
		// (get) Token: 0x06003D61 RID: 15713 RVA: 0x00328158 File Offset: 0x00326358
		// (set) Token: 0x06003D62 RID: 15714 RVA: 0x00328160 File Offset: 0x00326360
		public virtual TimeSpan Duration
		{
			get
			{
				return this._Duration;
			}
			set
			{
				this._Duration = value;
			}
		}

		// Token: 0x17001409 RID: 5129
		// (get) Token: 0x06003D63 RID: 15715 RVA: 0x00328169 File Offset: 0x00326369
		// (set) Token: 0x06003D64 RID: 15716 RVA: 0x00328171 File Offset: 0x00326371
		public decimal TotalFuelPrice
		{
			[CompilerGenerated]
			get
			{
				return this.<TotalFuelPrice>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<TotalFuelPrice>k__BackingField = value;
			}
		}

		// Token: 0x1700140A RID: 5130
		// (get) Token: 0x06003D65 RID: 15717 RVA: 0x0032817A File Offset: 0x0032637A
		public virtual double MotorHours
		{
			get
			{
				return this.Distance / this.AvgSpeed;
			}
		}

		// Token: 0x1700140B RID: 5131
		// (get) Token: 0x06003D66 RID: 15718 RVA: 0x0032818C File Offset: 0x0032638C
		public virtual double AvgSpeed
		{
			get
			{
				double num = this.Distance / this.Duration.TotalHours;
				if (double.IsInfinity(num))
				{
					return 0.0;
				}
				return num;
			}
		}

		// Token: 0x1700140C RID: 5132
		// (get) Token: 0x06003D67 RID: 15719 RVA: 0x003281C4 File Offset: 0x003263C4
		public virtual double AvgFuelConsumption
		{
			get
			{
				double fuelUsed = this.FuelUsed;
				double num = this.Distance;
				if (!SharedSettings.Current.Use_km)
				{
					num = UnitsHelper.Convert(num, UnitsHelper.Units.miles, UnitsHelper.Units.km);
				}
				double value = UnitsHelper.GetValue(fuelUsed * 100.0 / num, UnitsHelper.Units.liters100km);
				if (!double.IsFinite(value))
				{
					return double.NaN;
				}
				return value;
			}
		}

		// Token: 0x1700140D RID: 5133
		// (get) Token: 0x06003D68 RID: 15720 RVA: 0x0032821C File Offset: 0x0032641C
		public virtual double AvgFuelConsumptionCurrentUnits
		{
			get
			{
				switch (SharedSettings.Current.FuelConsumptionUnit)
				{
				case FuelConsumptionUnits.KmPerLiter:
					return UnitsHelper.Convert(this.AvgFuelConsumption, UnitsHelper.Units.liters100km, UnitsHelper.Units.km_liter);
				case FuelConsumptionUnits.MilesPerGallon:
					return UnitsHelper.Convert(this.AvgFuelConsumption, UnitsHelper.Units.liters100km, UnitsHelper.Units.MPG);
				}
				return this.AvgFuelConsumption;
			}
		}

		// Token: 0x1700140E RID: 5134
		// (get) Token: 0x06003D69 RID: 15721 RVA: 0x0032826E File Offset: 0x0032646E
		// (set) Token: 0x06003D6A RID: 15722 RVA: 0x00328276 File Offset: 0x00326476
		public double Distance
		{
			[CompilerGenerated]
			get
			{
				return this.<Distance>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<Distance>k__BackingField = value;
			}
		}

		// Token: 0x1700140F RID: 5135
		// (get) Token: 0x06003D6B RID: 15723 RVA: 0x0032827F File Offset: 0x0032647F
		// (set) Token: 0x06003D6C RID: 15724 RVA: 0x00328287 File Offset: 0x00326487
		public double FuelUsed
		{
			[CompilerGenerated]
			get
			{
				return this.<FuelUsed>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<FuelUsed>k__BackingField = value;
			}
		}

		// Token: 0x17001410 RID: 5136
		// (get) Token: 0x06003D6D RID: 15725 RVA: 0x00328290 File Offset: 0x00326490
		public double FuelUsedCurrentUnits
		{
			get
			{
				if (!double.IsFinite(this.FuelUsed))
				{
					return 0.0;
				}
				if (SharedSettings.Current.UseLitersForVolume)
				{
					return this.FuelUsed;
				}
				return UnitsHelper.Convert(this.FuelUsed, UnitsHelper.Units.liters, UnitsHelper.Units.gallons);
			}
		}

		// Token: 0x17001411 RID: 5137
		// (get) Token: 0x06003D6E RID: 15726 RVA: 0x003282CB File Offset: 0x003264CB
		// (set) Token: 0x06003D6F RID: 15727 RVA: 0x003282D3 File Offset: 0x003264D3
		public decimal FuelPricePerL
		{
			[CompilerGenerated]
			get
			{
				return this.<FuelPricePerL>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<FuelPricePerL>k__BackingField = value;
			}
		}

		// Token: 0x17001412 RID: 5138
		// (get) Token: 0x06003D70 RID: 15728 RVA: 0x003282DC File Offset: 0x003264DC
		// (set) Token: 0x06003D71 RID: 15729 RVA: 0x003282E4 File Offset: 0x003264E4
		public DateTime TimeStarted
		{
			[CompilerGenerated]
			get
			{
				return this.<TimeStarted>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<TimeStarted>k__BackingField = value;
			}
		}

		// Token: 0x17001413 RID: 5139
		// (get) Token: 0x06003D72 RID: 15730 RVA: 0x003282ED File Offset: 0x003264ED
		// (set) Token: 0x06003D73 RID: 15731 RVA: 0x003282F5 File Offset: 0x003264F5
		public DateTime TimeFinished
		{
			[CompilerGenerated]
			get
			{
				return this.<TimeFinished>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<TimeFinished>k__BackingField = value;
			}
		}

		// Token: 0x06003D74 RID: 15732 RVA: 0x00002050 File Offset: 0x00000250
		public DayDriveCycle()
		{
		}

		// Token: 0x040025A9 RID: 9641
		[CompilerGenerated]
		private decimal <TotalFuelPrice>k__BackingField;

		// Token: 0x040025AA RID: 9642
		[CompilerGenerated]
		private double <Distance>k__BackingField;

		// Token: 0x040025AB RID: 9643
		[CompilerGenerated]
		private double <FuelUsed>k__BackingField;

		// Token: 0x040025AC RID: 9644
		[CompilerGenerated]
		private decimal <FuelPricePerL>k__BackingField;

		// Token: 0x040025AD RID: 9645
		[CompilerGenerated]
		private DateTime <TimeStarted>k__BackingField;

		// Token: 0x040025AE RID: 9646
		[CompilerGenerated]
		private DateTime <TimeFinished>k__BackingField;

		// Token: 0x040025AF RID: 9647
		private TimeSpan _Duration;

		// Token: 0x040025B0 RID: 9648
		public int counter;
	}
}
