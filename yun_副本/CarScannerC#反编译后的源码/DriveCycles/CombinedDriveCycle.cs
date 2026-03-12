using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using CarScannerXamarinForms.OBD2.PIDS;
using CarScannerXamarinForms.Settings;
using CarScannerXamarinForms.ViewModels;

namespace CarScannerXamarinForms.DriveCycles
{
	// Token: 0x020005BE RID: 1470
	public class CombinedDriveCycle
	{
		// Token: 0x060034FD RID: 13565 RVA: 0x00261362 File Offset: 0x0025F562
		public CombinedDriveCycle(DriveCycle dc)
		{
			this.FuelPricePerL = dc.FuelPricePerL;
			this.AddCycle(dc);
		}

		// Token: 0x060034FE RID: 13566 RVA: 0x00261388 File Offset: 0x0025F588
		public void AddCycle(DriveCycle dc)
		{
			this._Cycles.Add(dc);
			this.Recalculate();
		}

		// Token: 0x060034FF RID: 13567 RVA: 0x0026139C File Offset: 0x0025F59C
		public void AddCycles(IEnumerable<DriveCycle> dcs)
		{
			foreach (DriveCycle driveCycle in dcs)
			{
				this._Cycles.Add(driveCycle);
			}
			this.Recalculate();
		}

		// Token: 0x06003500 RID: 13568 RVA: 0x002613F0 File Offset: 0x0025F5F0
		private void Recalculate()
		{
			double num = 0.0;
			double num2 = 0.0;
			foreach (DriveCycle driveCycle in this._Cycles)
			{
				num += driveCycle.Distance;
				num2 += driveCycle.FuelUsed;
			}
			this.TimeStarted = this._Cycles.Min((DriveCycle x) => x.TimeStarted);
			this.TimeFinished = this._Cycles.Max((DriveCycle x) => x.TimeFinished);
			this.Distance = num;
			this.FuelUsed = num2;
		}

		// Token: 0x17001345 RID: 4933
		// (get) Token: 0x06003501 RID: 13569 RVA: 0x002614D0 File Offset: 0x0025F6D0
		public IReadOnlyList<DriveCycle> Cycles
		{
			get
			{
				return this._Cycles;
			}
		}

		// Token: 0x17001346 RID: 4934
		// (get) Token: 0x06003502 RID: 13570 RVA: 0x002614D8 File Offset: 0x0025F6D8
		// (set) Token: 0x06003503 RID: 13571 RVA: 0x002614E0 File Offset: 0x0025F6E0
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

		// Token: 0x17001347 RID: 4935
		// (get) Token: 0x06003504 RID: 13572 RVA: 0x002614E9 File Offset: 0x0025F6E9
		// (set) Token: 0x06003505 RID: 13573 RVA: 0x002614F1 File Offset: 0x0025F6F1
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

		// Token: 0x17001348 RID: 4936
		// (get) Token: 0x06003506 RID: 13574 RVA: 0x002614FA File Offset: 0x0025F6FA
		// (set) Token: 0x06003507 RID: 13575 RVA: 0x00261502 File Offset: 0x0025F702
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

		// Token: 0x17001349 RID: 4937
		// (get) Token: 0x06003508 RID: 13576 RVA: 0x0026150B File Offset: 0x0025F70B
		// (set) Token: 0x06003509 RID: 13577 RVA: 0x00261513 File Offset: 0x0025F713
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

		// Token: 0x1700134A RID: 4938
		// (get) Token: 0x0600350A RID: 13578 RVA: 0x0026151C File Offset: 0x0025F71C
		// (set) Token: 0x0600350B RID: 13579 RVA: 0x00261524 File Offset: 0x0025F724
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

		// Token: 0x1700134B RID: 4939
		// (get) Token: 0x0600350C RID: 13580 RVA: 0x00261530 File Offset: 0x0025F730
		public double AvgFuelConsumption
		{
			get
			{
				double num = this.FuelUsed * 100.0 / this.Distance;
				if (double.IsInfinity(num))
				{
					return 0.0;
				}
				return num;
			}
		}

		// Token: 0x1700134C RID: 4940
		// (get) Token: 0x0600350D RID: 13581 RVA: 0x00261568 File Offset: 0x0025F768
		public double AvgSpeed
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

		// Token: 0x1700134D RID: 4941
		// (get) Token: 0x0600350E RID: 13582 RVA: 0x0026159E File Offset: 0x0025F79E
		public double AvgSpeedUserUnits
		{
			get
			{
				return UnitsHelper.GetValue(this.AvgSpeed, UnitsHelper.Units.kmh);
			}
		}

		// Token: 0x1700134E RID: 4942
		// (get) Token: 0x0600350F RID: 13583 RVA: 0x002615AC File Offset: 0x0025F7AC
		public double AvgFuelConsumptionUserUnits
		{
			get
			{
				return UnitsHelper.GetValue(this.AvgFuelConsumption, UnitsHelper.Units.liters100km);
			}
		}

		// Token: 0x1700134F RID: 4943
		// (get) Token: 0x06003510 RID: 13584 RVA: 0x002615BB File Offset: 0x0025F7BB
		public double FuelUsedUserUnits
		{
			get
			{
				return UnitsHelper.GetValue(this.FuelUsed, UnitsHelper.Units.liters);
			}
		}

		// Token: 0x17001350 RID: 4944
		// (get) Token: 0x06003511 RID: 13585 RVA: 0x002615CA File Offset: 0x0025F7CA
		public double DistanceUserUnits
		{
			get
			{
				return UnitsHelper.GetValue(this.Distance, UnitsHelper.Units.km);
			}
		}

		// Token: 0x17001351 RID: 4945
		// (get) Token: 0x06003512 RID: 13586 RVA: 0x002615D8 File Offset: 0x0025F7D8
		public decimal TotalFuelPrice
		{
			get
			{
				return (decimal)this.FuelUsed * this.FuelPricePerL;
			}
		}

		// Token: 0x17001352 RID: 4946
		// (get) Token: 0x06003513 RID: 13587 RVA: 0x002615F0 File Offset: 0x0025F7F0
		public virtual TimeSpan Duration
		{
			get
			{
				return new TimeSpan(this.TimeFinished.Ticks - this.TimeStarted.Ticks);
			}
		}

		// Token: 0x17001353 RID: 4947
		// (get) Token: 0x06003514 RID: 13588 RVA: 0x0026161F File Offset: 0x0025F81F
		public string DistanceUnits
		{
			get
			{
				return UnitsHelper.GetCaption(UnitsHelper.Units.km);
			}
		}

		// Token: 0x17001354 RID: 4948
		// (get) Token: 0x06003515 RID: 13589 RVA: 0x00261627 File Offset: 0x0025F827
		public string FuelUsedUnits
		{
			get
			{
				return UnitsHelper.GetCaption(UnitsHelper.Units.liters);
			}
		}

		// Token: 0x17001355 RID: 4949
		// (get) Token: 0x06003516 RID: 13590 RVA: 0x00261630 File Offset: 0x0025F830
		public string FuelConsumptionsUnits
		{
			get
			{
				return UnitsHelper.GetCaption(UnitsHelper.Units.liters100km);
			}
		}

		// Token: 0x17001356 RID: 4950
		// (get) Token: 0x06003517 RID: 13591 RVA: 0x00261639 File Offset: 0x0025F839
		public string SpeedUnits
		{
			get
			{
				return UnitsHelper.GetCaption(UnitsHelper.Units.kmh);
			}
		}

		// Token: 0x17001357 RID: 4951
		// (get) Token: 0x06003518 RID: 13592 RVA: 0x00261641 File Offset: 0x0025F841
		public string FuelPriceUnits
		{
			get
			{
				return SharedSettings.Current.Currency;
			}
		}

		// Token: 0x04001F92 RID: 8082
		private List<DriveCycle> _Cycles = new List<DriveCycle>();

		// Token: 0x04001F93 RID: 8083
		[CompilerGenerated]
		private double <Distance>k__BackingField;

		// Token: 0x04001F94 RID: 8084
		[CompilerGenerated]
		private double <FuelUsed>k__BackingField;

		// Token: 0x04001F95 RID: 8085
		[CompilerGenerated]
		private decimal <FuelPricePerL>k__BackingField;

		// Token: 0x04001F96 RID: 8086
		[CompilerGenerated]
		private DateTime <TimeStarted>k__BackingField;

		// Token: 0x04001F97 RID: 8087
		[CompilerGenerated]
		private DateTime <TimeFinished>k__BackingField;

		// Token: 0x020005BF RID: 1471
		[CompilerGenerated]
		[Serializable]
		private sealed class <>c
		{
			// Token: 0x06003519 RID: 13593 RVA: 0x0026164D File Offset: 0x0025F84D
			// Note: this type is marked as 'beforefieldinit'.
			static <>c()
			{
			}

			// Token: 0x0600351A RID: 13594 RVA: 0x00002050 File Offset: 0x00000250
			public <>c()
			{
			}

			// Token: 0x0600351B RID: 13595 RVA: 0x00261659 File Offset: 0x0025F859
			internal DateTime <Recalculate>b__3_0(DriveCycle x)
			{
				return x.TimeStarted;
			}

			// Token: 0x0600351C RID: 13596 RVA: 0x00261661 File Offset: 0x0025F861
			internal DateTime <Recalculate>b__3_1(DriveCycle x)
			{
				return x.TimeFinished;
			}

			// Token: 0x04001F98 RID: 8088
			public static readonly CombinedDriveCycle.<>c <>9 = new CombinedDriveCycle.<>c();

			// Token: 0x04001F99 RID: 8089
			public static Func<DriveCycle, DateTime> <>9__3_0;

			// Token: 0x04001F9A RID: 8090
			public static Func<DriveCycle, DateTime> <>9__3_1;
		}
	}
}
