using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;

namespace CarScannerXamarinForms.ViewModels
{
	// Token: 0x02000761 RID: 1889
	internal class SpeedCalibrationModel
	{
		// Token: 0x170014C0 RID: 5312
		// (get) Token: 0x06003FE9 RID: 16361 RVA: 0x0033518E File Offset: 0x0033338E
		// (set) Token: 0x06003FEA RID: 16362 RVA: 0x00335196 File Offset: 0x00333396
		public bool CalibrationFinished
		{
			[CompilerGenerated]
			get
			{
				return this.<CalibrationFinished>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<CalibrationFinished>k__BackingField = value;
			}
		}

		// Token: 0x170014C1 RID: 5313
		// (get) Token: 0x06003FEB RID: 16363 RVA: 0x0033519F File Offset: 0x0033339F
		// (set) Token: 0x06003FEC RID: 16364 RVA: 0x003351A7 File Offset: 0x003333A7
		public int TargetTimeSeconds
		{
			[CompilerGenerated]
			get
			{
				return this.<TargetTimeSeconds>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<TargetTimeSeconds>k__BackingField = value;
			}
		} = 10;

		// Token: 0x170014C2 RID: 5314
		// (get) Token: 0x06003FED RID: 16365 RVA: 0x003351B0 File Offset: 0x003333B0
		// (set) Token: 0x06003FEE RID: 16366 RVA: 0x003351B8 File Offset: 0x003333B8
		public int TargetCalibrationMinimumSpeedByGPS
		{
			[CompilerGenerated]
			get
			{
				return this.<TargetCalibrationMinimumSpeedByGPS>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<TargetCalibrationMinimumSpeedByGPS>k__BackingField = value;
			}
		} = 80;

		// Token: 0x06003FEF RID: 16367 RVA: 0x000027D4 File Offset: 0x000009D4
		public void RecordVehicleSpeed(int speed, TimeSpan timeStamp)
		{
		}

		// Token: 0x06003FF0 RID: 16368 RVA: 0x003351C4 File Offset: 0x003333C4
		public void RecordGPSSpeed(int speed, TimeSpan timeStamp)
		{
			if (speed < this.TargetCalibrationMinimumSpeedByGPS)
			{
				this.GPSSpeedCollection.Clear();
				return;
			}
			if (this.GPSSpeedCollection.Count == 0)
			{
				this.GPSSpeedCollection.Add(new ValueTuple<int, TimeSpan>(speed, timeStamp));
				return;
			}
			int item = this.GPSSpeedCollection[this.GPSSpeedCollection.Count - 1].Item1;
			if (item == speed)
			{
				this.GPSSpeedCollection.Add(new ValueTuple<int, TimeSpan>(speed, timeStamp));
				return;
			}
			this.GPSSpeedCollection.Clear();
			this.GPSSpeedCollection.Add(new ValueTuple<int, TimeSpan>(speed, timeStamp));
		}

		// Token: 0x06003FF1 RID: 16369 RVA: 0x0033525C File Offset: 0x0033345C
		private void Analyze()
		{
			ValueTuple<int, TimeSpan> valueTuple = this.GPSSpeedCollection[this.GPSSpeedCollection.Count - 1];
			int lastItemSpeedGps = valueTuple.Item1;
			TimeSpan item = valueTuple.Item2;
			TimeSpan firstItemTimeGps = item - TimeSpan.FromSeconds((double)this.TargetTimeSeconds);
			List<ValueTuple<int, TimeSpan>> list = this.GPSSpeedCollection.ToList<ValueTuple<int, TimeSpan>>();
			list.Reverse();
			List<ValueTuple<int, TimeSpan>> list2 = list.TakeWhile((ValueTuple<int, TimeSpan> x) => x.Item2 >= firstItemTimeGps).ToList<ValueTuple<int, TimeSpan>>();
			if (list2.All((ValueTuple<int, TimeSpan> x) => Math.Abs(x.Item1 - lastItemSpeedGps) <= this.PossibleSpeedFluactuation))
			{
				valueTuple = this.VehicleSpeedCollection[this.GPSSpeedCollection.Count - 1];
				int lastItemSpeedOBD = valueTuple.Item1;
				List<ValueTuple<int, TimeSpan>> list3 = this.VehicleSpeedCollection.ToList<ValueTuple<int, TimeSpan>>();
				list3.Reverse();
				List<ValueTuple<int, TimeSpan>> list4 = list3.TakeWhile((ValueTuple<int, TimeSpan> x) => x.Item2 >= firstItemTimeGps).ToList<ValueTuple<int, TimeSpan>>();
				if (list4.All((ValueTuple<int, TimeSpan> x) => Math.Abs(x.Item1 - lastItemSpeedOBD) <= this.PossibleSpeedFluactuation))
				{
					double num = (double)list2.Sum((ValueTuple<int, TimeSpan> x) => x.Item1) / (double)list2.Count;
					double num2 = (double)list4.Sum((ValueTuple<int, TimeSpan> x) => x.Item1) / (double)list4.Count;
					double num3 = num / num2;
					this.CalibrationFinished = true;
				}
			}
		}

		// Token: 0x06003FF2 RID: 16370 RVA: 0x003353DE File Offset: 0x003335DE
		public SpeedCalibrationModel()
		{
		}

		// Token: 0x04002749 RID: 10057
		[CompilerGenerated]
		private bool <CalibrationFinished>k__BackingField;

		// Token: 0x0400274A RID: 10058
		[CompilerGenerated]
		private int <TargetTimeSeconds>k__BackingField;

		// Token: 0x0400274B RID: 10059
		[CompilerGenerated]
		private int <TargetCalibrationMinimumSpeedByGPS>k__BackingField;

		// Token: 0x0400274C RID: 10060
		public int PossibleSpeedFluactuation = 1;

		// Token: 0x0400274D RID: 10061
		private List<ValueTuple<int, TimeSpan>> GPSSpeedCollection = new List<ValueTuple<int, TimeSpan>>();

		// Token: 0x0400274E RID: 10062
		private List<ValueTuple<int, TimeSpan>> VehicleSpeedCollection = new List<ValueTuple<int, TimeSpan>>();

		// Token: 0x02000762 RID: 1890
		[CompilerGenerated]
		[Serializable]
		private sealed class <>c
		{
			// Token: 0x06003FF3 RID: 16371 RVA: 0x00335413 File Offset: 0x00333613
			// Note: this type is marked as 'beforefieldinit'.
			static <>c()
			{
			}

			// Token: 0x06003FF4 RID: 16372 RVA: 0x00002050 File Offset: 0x00000250
			public <>c()
			{
			}

			// Token: 0x06003FF5 RID: 16373 RVA: 0x0033541F File Offset: 0x0033361F
			internal int <Analyze>b__17_4(ValueTuple<int, TimeSpan> x)
			{
				return x.Item1;
			}

			// Token: 0x06003FF6 RID: 16374 RVA: 0x0033541F File Offset: 0x0033361F
			internal int <Analyze>b__17_5(ValueTuple<int, TimeSpan> x)
			{
				return x.Item1;
			}

			// Token: 0x0400274F RID: 10063
			public static readonly SpeedCalibrationModel.<>c <>9 = new SpeedCalibrationModel.<>c();

			// Token: 0x04002750 RID: 10064
			public static Func<ValueTuple<int, TimeSpan>, int> <>9__17_4;

			// Token: 0x04002751 RID: 10065
			public static Func<ValueTuple<int, TimeSpan>, int> <>9__17_5;
		}

		// Token: 0x02000763 RID: 1891
		[CompilerGenerated]
		private sealed class <>c__DisplayClass17_0
		{
			// Token: 0x06003FF7 RID: 16375 RVA: 0x00002050 File Offset: 0x00000250
			public <>c__DisplayClass17_0()
			{
			}

			// Token: 0x06003FF8 RID: 16376 RVA: 0x00335427 File Offset: 0x00333627
			internal bool <Analyze>b__0(ValueTuple<int, TimeSpan> x)
			{
				return x.Item2 >= this.firstItemTimeGps;
			}

			// Token: 0x06003FF9 RID: 16377 RVA: 0x0033543A File Offset: 0x0033363A
			internal bool <Analyze>b__1(ValueTuple<int, TimeSpan> x)
			{
				return Math.Abs(x.Item1 - this.lastItemSpeedGps) <= this.<>4__this.PossibleSpeedFluactuation;
			}

			// Token: 0x06003FFA RID: 16378 RVA: 0x00335427 File Offset: 0x00333627
			internal bool <Analyze>b__2(ValueTuple<int, TimeSpan> x)
			{
				return x.Item2 >= this.firstItemTimeGps;
			}

			// Token: 0x04002752 RID: 10066
			public TimeSpan firstItemTimeGps;

			// Token: 0x04002753 RID: 10067
			public int lastItemSpeedGps;

			// Token: 0x04002754 RID: 10068
			public SpeedCalibrationModel <>4__this;
		}

		// Token: 0x02000764 RID: 1892
		[CompilerGenerated]
		private sealed class <>c__DisplayClass17_1
		{
			// Token: 0x06003FFB RID: 16379 RVA: 0x00002050 File Offset: 0x00000250
			public <>c__DisplayClass17_1()
			{
			}

			// Token: 0x06003FFC RID: 16380 RVA: 0x0033545E File Offset: 0x0033365E
			internal bool <Analyze>b__3(ValueTuple<int, TimeSpan> x)
			{
				return Math.Abs(x.Item1 - this.lastItemSpeedOBD) <= this.CS$<>8__locals1.<>4__this.PossibleSpeedFluactuation;
			}

			// Token: 0x04002755 RID: 10069
			public int lastItemSpeedOBD;

			// Token: 0x04002756 RID: 10070
			public SpeedCalibrationModel.<>c__DisplayClass17_0 CS$<>8__locals1;
		}
	}
}
