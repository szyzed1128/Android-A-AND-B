using System;
using System.Globalization;
using CarScannerXamarinForms.OBD2;
using CarScannerXamarinForms.OBD2.PIDS;
using CarScannerXamarinForms.Settings;

namespace CarScannerXamarinForms
{
	// Token: 0x02000182 RID: 386
	internal class BrakeDistanceTest : SpeedTestV1
	{
		// Token: 0x060015AB RID: 5547 RVA: 0x00099B02 File Offset: 0x00097D02
		public BrakeDistanceTest(OBDDataReader Reader, string Name, double StartSpeed, double EndSpeed)
			: base(Reader, Name, StartSpeed, EndSpeed)
		{
		}

		// Token: 0x060015AC RID: 5548 RVA: 0x00099B1A File Offset: 0x00097D1A
		public override void Start()
		{
			this.distance = 0.0;
			base.Start();
			base.Value = "0";
		}

		// Token: 0x060015AD RID: 5549 RVA: 0x00099B3C File Offset: 0x00097D3C
		public override void Cancel()
		{
			this.distance = 0.0;
			base.Cancel();
			base.Value = "0";
		}

		// Token: 0x060015AE RID: 5550 RVA: 0x00099B60 File Offset: 0x00097D60
		protected override void CalculateTextValue()
		{
			if (base.CurrentState == SpeedTestV1.TestStates.Measuring)
			{
				if (SharedSettings.Current.Use_km)
				{
					base.Value = (this.distance * 1000.0).ToString("0.##", CultureInfo.InvariantCulture) + " m";
					return;
				}
				base.Value = (this.distance * 5280.0).ToString("0.##", CultureInfo.InvariantCulture) + " ft.";
			}
		}

		// Token: 0x060015AF RID: 5551 RVA: 0x00099BE8 File Offset: 0x00097DE8
		protected override void OnMeasuringCheck(IPIDFloatValue fpid)
		{
			double num = (SharedSettings.Current.Use_km ? fpid.Value : (fpid.Value * 0.621371));
			if (num >= this.StartSpeed)
			{
				base.CurrentState = SpeedTestV1.TestStates.StartConditionNotReady;
				this.BeforeStartFlag = true;
				base.CurrentTime = default(TimeSpan);
				this.LastStep = fpid.TimeStamp;
				this.LastSpeed = fpid.Value;
				this.distance = 0.0;
				this.sw.Restart();
				return;
			}
			base.CurrentTime = this.sw.Elapsed;
			TimeSpan timeStamp = fpid.TimeStamp;
			TimeSpan timeSpan = timeStamp - this.LastStep;
			this.LastStep = timeStamp;
			double num2 = this.LastSpeed * timeSpan.TotalHours;
			this.LastSpeed = fpid.Value;
			this.distance += num2;
			if (num <= this.EndSpeed)
			{
				base.CurrentState = SpeedTestV1.TestStates.Finished;
				this.sw.Stop();
				this.OnTestCompleted();
			}
		}

		// Token: 0x060015B0 RID: 5552 RVA: 0x00099CEC File Offset: 0x00097EEC
		protected override void OnStartConditionNotReadyCheck(IPIDFloatValue fpid)
		{
			double num = (SharedSettings.Current.Use_km ? fpid.Value : (fpid.Value * 0.621371));
			if (num >= this.StartSpeed)
			{
				this.BeforeStartFlag = true;
				this.sw.Restart();
				base.CurrentTime = default(TimeSpan);
				this.LastStep = fpid.TimeStamp;
				this.LastSpeed = fpid.Value;
				this.distance = 0.0;
			}
			if (num < this.StartSpeed && this.BeforeStartFlag)
			{
				base.CurrentState = SpeedTestV1.TestStates.Measuring;
				base.CurrentTime = default(TimeSpan);
				this.sw.Restart();
			}
		}

		// Token: 0x0400062D RID: 1581
		private double distance;

		// Token: 0x0400062E RID: 1582
		private TimeSpan LastStep = TimeSpan.Zero;

		// Token: 0x0400062F RID: 1583
		private double LastSpeed;
	}
}
