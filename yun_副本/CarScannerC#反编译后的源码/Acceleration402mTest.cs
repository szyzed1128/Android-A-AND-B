using System;
using System.ComponentModel;
using System.Globalization;
using CarScannerXamarinForms.OBD2;
using CarScannerXamarinForms.OBD2.PIDS;
using CarScannerXamarinForms.Settings;
using CarScannerXamarinForms.SpeedTest;

namespace CarScannerXamarinForms
{
	// Token: 0x02000181 RID: 385
	internal class Acceleration402mTest : SpeedTestV1, ISpeedTest, ISpeedTestBase, INotifyPropertyChanged
	{
		// Token: 0x060015A7 RID: 5543 RVA: 0x00099865 File Offset: 0x00097A65
		public Acceleration402mTest(OBDDataReader Reader, string Name)
			: base(Reader, Name, 0.0, 999.0)
		{
		}

		// Token: 0x060015A8 RID: 5544 RVA: 0x0009988C File Offset: 0x00097A8C
		protected override void OnStartConditionNotReadyCheck(IPIDFloatValue fpid)
		{
			double value = fpid.Value;
			if (value <= this.StartSpeed)
			{
				this.BeforeStartFlag = true;
				this.sw.Restart();
				base.CurrentTime = default(TimeSpan);
				this.distance_km = 0.0;
				this.LastStep = fpid.TimeStamp;
				this.LastSpeed = fpid.Value;
			}
			if (value > this.StartSpeed && this.BeforeStartFlag)
			{
				base.CurrentState = SpeedTestV1.TestStates.Measuring;
				base.CurrentTime = default(TimeSpan);
				double num = value * (fpid.TimeStamp - this.LastStep).TotalHours;
				this.distance_km = num;
				this.LastStep = fpid.TimeStamp;
				this.LastSpeed = fpid.Value;
				this.sw.Restart();
			}
		}

		// Token: 0x060015A9 RID: 5545 RVA: 0x00099960 File Offset: 0x00097B60
		protected override void OnMeasuringCheck(IPIDFloatValue fpid)
		{
			if (fpid.Value <= this.StartSpeed)
			{
				base.CurrentState = SpeedTestV1.TestStates.StartConditionNotReady;
				this.BeforeStartFlag = true;
				this.sw.Restart();
				base.CurrentTime = default(TimeSpan);
				this.distance_km = 0.0;
				this.LastStep = fpid.TimeStamp;
				this.LastSpeed = fpid.Value;
				return;
			}
			base.CurrentTime = this.sw.Elapsed;
			TimeSpan timeStamp = fpid.TimeStamp;
			TimeSpan timeSpan = timeStamp - this.LastStep;
			this.LastStep = timeStamp;
			double num = this.LastSpeed * timeSpan.TotalHours;
			this.LastSpeed = fpid.Value;
			this.distance_km += num;
			if (this.distance_km > 0.402)
			{
				base.CurrentState = SpeedTestV1.TestStates.Finished;
				this.sw.Stop();
				this.distance_km = 0.402;
				this.CalculateTextValue();
				this.OnTestCompleted();
			}
		}

		// Token: 0x060015AA RID: 5546 RVA: 0x00099A60 File Offset: 0x00097C60
		protected override void CalculateTextValue()
		{
			string text = base.CurrentTime.ToString("mm\\:ss\\.fff", CultureInfo.InvariantCulture);
			string text2;
			if (SharedSettings.Current.Use_km)
			{
				text2 = " " + (this.distance_km * 1000.0).ToString("0") + "m";
			}
			else
			{
				text2 = " " + (this.distance_km * 3280.84).ToString("0") + "ft.";
			}
			base.Value = text + text2;
		}

		// Token: 0x0400062A RID: 1578
		private double distance_km;

		// Token: 0x0400062B RID: 1579
		private TimeSpan LastStep = TimeSpan.Zero;

		// Token: 0x0400062C RID: 1580
		private double LastSpeed;
	}
}
