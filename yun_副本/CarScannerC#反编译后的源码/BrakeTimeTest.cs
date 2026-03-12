using System;
using CarScannerXamarinForms.OBD2;
using CarScannerXamarinForms.OBD2.PIDS;
using CarScannerXamarinForms.Settings;

namespace CarScannerXamarinForms
{
	// Token: 0x02000183 RID: 387
	internal class BrakeTimeTest : SpeedTestV1
	{
		// Token: 0x060015B1 RID: 5553 RVA: 0x00099D9F File Offset: 0x00097F9F
		public BrakeTimeTest(OBDDataReader Reader, string Name, double StartSpeed, double EndSpeed)
			: base(Reader, Name, StartSpeed, EndSpeed)
		{
		}

		// Token: 0x060015B2 RID: 5554 RVA: 0x00099DAC File Offset: 0x00097FAC
		protected override void OnMeasuringCheck(IPIDFloatValue fpid)
		{
			double num = (SharedSettings.Current.Use_km ? fpid.Value : (fpid.Value * 0.621371));
			if (num >= this.StartSpeed)
			{
				base.CurrentState = SpeedTestV1.TestStates.StartConditionNotReady;
				this.BeforeStartFlag = true;
				this.sw.Restart();
				base.CurrentTime = default(TimeSpan);
				return;
			}
			base.CurrentTime = this.sw.Elapsed;
			if (num <= this.EndSpeed)
			{
				base.CurrentState = SpeedTestV1.TestStates.Finished;
				this.OnTestCompleted();
			}
		}

		// Token: 0x060015B3 RID: 5555 RVA: 0x00099E38 File Offset: 0x00098038
		protected override void OnStartConditionNotReadyCheck(IPIDFloatValue fpid)
		{
			double num = (SharedSettings.Current.Use_km ? fpid.Value : (fpid.Value * 0.621371));
			if (num >= this.StartSpeed)
			{
				this.BeforeStartFlag = true;
				this.sw.Restart();
				base.CurrentTime = default(TimeSpan);
			}
			if (num < this.StartSpeed && this.BeforeStartFlag)
			{
				base.CurrentState = SpeedTestV1.TestStates.Measuring;
				base.CurrentTime = default(TimeSpan);
				this.sw.Restart();
			}
		}
	}
}
