using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using CarScannerXamarinForms.OBD2.PIDS;
using CarScannerXamarinForms.Settings;

namespace CarScannerXamarinForms.ViewModels
{
	// Token: 0x02000765 RID: 1893
	public class SpeedCalibrationModelV2
	{
		// Token: 0x170014C3 RID: 5315
		// (get) Token: 0x06003FFD RID: 16381 RVA: 0x00335487 File Offset: 0x00333687
		// (set) Token: 0x06003FFE RID: 16382 RVA: 0x0033548E File Offset: 0x0033368E
		public static SpeedCalibrationModelV2 Instance
		{
			[CompilerGenerated]
			get
			{
				return SpeedCalibrationModelV2.<Instance>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				SpeedCalibrationModelV2.<Instance>k__BackingField = value;
			}
		}

		// Token: 0x06003FFF RID: 16383 RVA: 0x00335498 File Offset: 0x00333698
		public SpeedCalibrationModelV2()
		{
			this.SpeedPid = App.OBDReader.CurrentCarData.FindPIDByRole(Roles.Speed);
		}

		// Token: 0x170014C4 RID: 5316
		// (get) Token: 0x06004000 RID: 16384 RVA: 0x0033550C File Offset: 0x0033370C
		// (set) Token: 0x06004001 RID: 16385 RVA: 0x00335514 File Offset: 0x00333714
		public IPIDFloatValue SpeedPid
		{
			[CompilerGenerated]
			get
			{
				return this.<SpeedPid>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<SpeedPid>k__BackingField = value;
			}
		}

		// Token: 0x170014C5 RID: 5317
		// (get) Token: 0x06004002 RID: 16386 RVA: 0x0033551D File Offset: 0x0033371D
		// (set) Token: 0x06004003 RID: 16387 RVA: 0x00335525 File Offset: 0x00333725
		public double PossibleSpeedFluctuations
		{
			[CompilerGenerated]
			get
			{
				return this.<PossibleSpeedFluctuations>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<PossibleSpeedFluctuations>k__BackingField = value;
			}
		} = 1.0;

		// Token: 0x170014C6 RID: 5318
		// (get) Token: 0x06004004 RID: 16388 RVA: 0x0033552E File Offset: 0x0033372E
		// (set) Token: 0x06004005 RID: 16389 RVA: 0x00335536 File Offset: 0x00333736
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

		// Token: 0x170014C7 RID: 5319
		// (get) Token: 0x06004006 RID: 16390 RVA: 0x0033553F File Offset: 0x0033373F
		// (set) Token: 0x06004007 RID: 16391 RVA: 0x00335547 File Offset: 0x00333747
		public bool IsFinished
		{
			[CompilerGenerated]
			get
			{
				return this.<IsFinished>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<IsFinished>k__BackingField = value;
			}
		}

		// Token: 0x06004008 RID: 16392 RVA: 0x00335550 File Offset: 0x00333750
		public void RecordGPSSpeed(double gpsSpeed, TimeSpan timeStamp)
		{
			if (this.IsFinished)
			{
				return;
			}
			if (this.SpeedPid == null)
			{
				return;
			}
			if (gpsSpeed < (double)this.MinimumSpeed)
			{
				this.Values.Clear();
				return;
			}
			if (this.Values.Count != 0)
			{
				if (Math.Abs(this.LastGPSSpeed - gpsSpeed) <= this.PossibleSpeedFluctuations && Math.Abs(this.LastOBDSpeed - this.SpeedPid.Value) <= this.PossibleSpeedFluctuations)
				{
					if (Math.Abs((this.SpeedPid.TimeStamp - timeStamp).TotalMilliseconds) >= 300.0)
					{
						this.Values.Clear();
						return;
					}
					double value = this.SpeedPid.Value;
					this.lastRecord = timeStamp;
					this.LastGPSSpeed = gpsSpeed;
					this.Values.Add(new ValueTuple<double, double>(gpsSpeed, value));
					if (timeStamp >= this.finishRecord)
					{
						double num = this.Values.Sum((ValueTuple<double, double> x) => x.Item1) / (double)this.Values.Count;
						double num2 = this.Values.Sum((ValueTuple<double, double> x) => x.Item2) / (double)this.Values.Count;
						double num3 = num / num2;
						SharedSettings.Current.SpeedCorrectionFactor = num3;
						this.IsFinished = true;
						SharedSettings.Current.SpeedCalibrationTaskPending = false;
						return;
					}
				}
				else
				{
					this.Values.Clear();
					this.RecordGPSSpeed(gpsSpeed, timeStamp);
				}
				return;
			}
			if (Math.Abs((this.SpeedPid.TimeStamp - timeStamp).TotalMilliseconds) >= 300.0)
			{
				return;
			}
			double value2 = this.SpeedPid.Value;
			this.lastRecord = timeStamp;
			this.firstRecord = timeStamp;
			this.finishRecord = timeStamp + TimeSpan.FromSeconds((double)this.TargetTimeSeconds);
			this.LastGPSSpeed = gpsSpeed;
			this.LastOBDSpeed = value2;
			this.Values.Add(new ValueTuple<double, double>(gpsSpeed, value2));
		}

		// Token: 0x04002757 RID: 10071
		[CompilerGenerated]
		private static SpeedCalibrationModelV2 <Instance>k__BackingField;

		// Token: 0x04002758 RID: 10072
		[CompilerGenerated]
		private IPIDFloatValue <SpeedPid>k__BackingField;

		// Token: 0x04002759 RID: 10073
		public int MinimumSpeed = 80;

		// Token: 0x0400275A RID: 10074
		[CompilerGenerated]
		private double <PossibleSpeedFluctuations>k__BackingField;

		// Token: 0x0400275B RID: 10075
		[CompilerGenerated]
		private int <TargetTimeSeconds>k__BackingField;

		// Token: 0x0400275C RID: 10076
		private List<ValueTuple<double, double>> Values = new List<ValueTuple<double, double>>();

		// Token: 0x0400275D RID: 10077
		private TimeSpan lastRecord = TimeSpan.Zero;

		// Token: 0x0400275E RID: 10078
		private TimeSpan firstRecord = TimeSpan.Zero;

		// Token: 0x0400275F RID: 10079
		private TimeSpan finishRecord = TimeSpan.Zero;

		// Token: 0x04002760 RID: 10080
		private double LastGPSSpeed;

		// Token: 0x04002761 RID: 10081
		private double LastOBDSpeed;

		// Token: 0x04002762 RID: 10082
		[CompilerGenerated]
		private bool <IsFinished>k__BackingField;

		// Token: 0x02000766 RID: 1894
		[CompilerGenerated]
		[Serializable]
		private sealed class <>c
		{
			// Token: 0x06004009 RID: 16393 RVA: 0x00335762 File Offset: 0x00333962
			// Note: this type is marked as 'beforefieldinit'.
			static <>c()
			{
			}

			// Token: 0x0600400A RID: 16394 RVA: 0x00002050 File Offset: 0x00000250
			public <>c()
			{
			}

			// Token: 0x0600400B RID: 16395 RVA: 0x0033576E File Offset: 0x0033396E
			internal double <RecordGPSSpeed>b__28_0(ValueTuple<double, double> x)
			{
				return x.Item1;
			}

			// Token: 0x0600400C RID: 16396 RVA: 0x00335776 File Offset: 0x00333976
			internal double <RecordGPSSpeed>b__28_1(ValueTuple<double, double> x)
			{
				return x.Item2;
			}

			// Token: 0x04002763 RID: 10083
			public static readonly SpeedCalibrationModelV2.<>c <>9 = new SpeedCalibrationModelV2.<>c();

			// Token: 0x04002764 RID: 10084
			public static Func<ValueTuple<double, double>, double> <>9__28_0;

			// Token: 0x04002765 RID: 10085
			public static Func<ValueTuple<double, double>, double> <>9__28_1;
		}
	}
}
