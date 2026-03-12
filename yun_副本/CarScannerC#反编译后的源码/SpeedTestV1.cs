using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Threading;
using CarScannerXamarinForms.DataRecorder;
using CarScannerXamarinForms.OBD2;
using CarScannerXamarinForms.OBD2.PIDS;
using CarScannerXamarinForms.Settings;
using CarScannerXamarinForms.SpeedTest;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace CarScannerXamarinForms
{
	// Token: 0x02000185 RID: 389
	public class SpeedTestV1 : ISpeedTest, ISpeedTestBase, INotifyPropertyChanged, IDisposable
	{
		// Token: 0x17000F48 RID: 3912
		// (get) Token: 0x060015B4 RID: 5556 RVA: 0x00099EC4 File Offset: 0x000980C4
		// (set) Token: 0x060015B5 RID: 5557 RVA: 0x00099ECC File Offset: 0x000980CC
		public virtual double StartSpeed
		{
			[CompilerGenerated]
			get
			{
				return this.<StartSpeed>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<StartSpeed>k__BackingField = value;
			}
		}

		// Token: 0x17000F49 RID: 3913
		// (get) Token: 0x060015B6 RID: 5558 RVA: 0x00099ED5 File Offset: 0x000980D5
		// (set) Token: 0x060015B7 RID: 5559 RVA: 0x00099EDD File Offset: 0x000980DD
		public virtual double EndSpeed
		{
			[CompilerGenerated]
			get
			{
				return this.<EndSpeed>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<EndSpeed>k__BackingField = value;
			}
		}

		// Token: 0x17000F4A RID: 3914
		// (get) Token: 0x060015B8 RID: 5560 RVA: 0x0001DB13 File Offset: 0x0001BD13
		protected OBDDataReader Reader
		{
			get
			{
				return App.OBDReader;
			}
		}

		// Token: 0x17000F4B RID: 3915
		// (get) Token: 0x060015B9 RID: 5561 RVA: 0x00099EE6 File Offset: 0x000980E6
		// (set) Token: 0x060015BA RID: 5562 RVA: 0x00099EEE File Offset: 0x000980EE
		protected TimeSpan CurrentTime
		{
			get
			{
				return this._CurrentTime;
			}
			set
			{
				this._CurrentTime = value;
				this.CalculateTextValue();
			}
		}

		// Token: 0x060015BB RID: 5563 RVA: 0x00099F00 File Offset: 0x00098100
		protected virtual void CalculateTextValue()
		{
			this.Value = this.CurrentTime.ToString("mm\\:ss\\.fff", CultureInfo.InvariantCulture);
		}

		// Token: 0x17000F4C RID: 3916
		// (get) Token: 0x060015BC RID: 5564 RVA: 0x00099F2B File Offset: 0x0009812B
		// (set) Token: 0x060015BD RID: 5565 RVA: 0x00099F33 File Offset: 0x00098133
		protected SpeedTestV1.TestStates CurrentState
		{
			get
			{
				return this._CurrentState;
			}
			set
			{
				this._CurrentState = value;
				if (value == SpeedTestV1.TestStates.Finished)
				{
					this.IsTestFinished = true;
				}
				else
				{
					this.IsTestFinished = false;
				}
				MainThread.BeginInvokeOnMainThread(delegate
				{
					PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
					if (propertyChanged == null)
					{
						return;
					}
					propertyChanged(this, new PropertyChangedEventArgs("IsTestFinished"));
				});
			}
		}

		// Token: 0x060015BE RID: 5566 RVA: 0x00099F61 File Offset: 0x00098161
		public SpeedTestV1()
		{
			this.sw = new Stopwatch();
		}

		// Token: 0x060015BF RID: 5567 RVA: 0x00099F7F File Offset: 0x0009817F
		public SpeedTestV1(OBDDataReader Reader, string Name, double StartSpeed, double EndSpeed)
		{
			this.sw = new Stopwatch();
			this.StartSpeed = StartSpeed;
			this.EndSpeed = EndSpeed;
			this.Name = Name;
			this.BeforeStartFlag = false;
		}

		// Token: 0x060015C0 RID: 5568 RVA: 0x00099FBC File Offset: 0x000981BC
		public virtual void RefreshSpeedPID()
		{
			if (this.SpeedPID != null)
			{
				this.SpeedPID.ValueChanged -= this.SpeedTest_ValueChanged;
			}
			this.SpeedPID = SpeedTestViewModel.Instance.SpeedPID;
			this.SpeedPID.ValueChanged -= this.SpeedTest_ValueChanged;
			this.SpeedPID.ValueChanged += this.SpeedTest_ValueChanged;
		}

		// Token: 0x060015C1 RID: 5569 RVA: 0x0009A028 File Offset: 0x00098228
		public virtual void Start()
		{
			this.CurrentTime = default(TimeSpan);
			this.sw.Restart();
			this.CurrentState = SpeedTestV1.TestStates.StartConditionNotReady;
			this.BeforeStartFlag = false;
			if (this.SpeedPID != null)
			{
				this.SpeedPID.ValueChanged -= this.SpeedTest_ValueChanged;
			}
			this.SpeedPID = SpeedTestViewModel.Instance.SpeedPID;
			this.SpeedPID.ValueChanged -= this.SpeedTest_ValueChanged;
			this.SpeedPID.ValueChanged += this.SpeedTest_ValueChanged;
			OBDRequest obdrequest;
			if (this.SpeedPID is CustomPID)
			{
				CustomPID customPID = this.SpeedPID as CustomPID;
				obdrequest = new OBDRequest(customPID.Command, customPID.Header, customPID.BeforeCommand, customPID.AfterCommand, true);
			}
			else
			{
				obdrequest = new OBDRequest(this.SpeedPID.Command, true);
			}
			this.Reader.AddRequestToQueue(obdrequest);
		}

		// Token: 0x060015C2 RID: 5570 RVA: 0x0009A114 File Offset: 0x00098314
		private void SpeedTest_ValueChanged(object sender, PID e)
		{
			SpeedTestV1.TestStates currentState = this.CurrentState;
			if (currentState == SpeedTestV1.TestStates.StartConditionNotReady)
			{
				this.OnStartConditionNotReadyCheck(this.SpeedPID);
				SpeedTestV1.TestStates currentState2 = this.CurrentState;
				return;
			}
			if (currentState != SpeedTestV1.TestStates.Measuring)
			{
				return;
			}
			this.OnMeasuringCheck(this.SpeedPID);
		}

		// Token: 0x060015C3 RID: 5571 RVA: 0x0009A154 File Offset: 0x00098354
		protected virtual void OnMeasuringCheck(IPIDFloatValue fpid)
		{
			double num = (SharedSettings.Current.Use_km ? fpid.Value : (fpid.Value * 0.621371));
			if (num <= this.StartSpeed)
			{
				this.CurrentState = SpeedTestV1.TestStates.StartConditionNotReady;
				this.BeforeStartFlag = true;
				this.sw.Restart();
				this.CurrentTime = default(TimeSpan);
				return;
			}
			this.CurrentTime = this.sw.Elapsed;
			if (num >= this.EndSpeed)
			{
				this.CurrentState = SpeedTestV1.TestStates.Finished;
				this.sw.Stop();
				this.OnTestCompleted();
				this.CalculateTextValue();
			}
		}

		// Token: 0x060015C4 RID: 5572 RVA: 0x0009A1F0 File Offset: 0x000983F0
		protected virtual void OnStartConditionNotReadyCheck(IPIDFloatValue fpid)
		{
			double num = (SharedSettings.Current.Use_km ? fpid.Value : (fpid.Value * 0.621371));
			if (num <= this.StartSpeed)
			{
				this.BeforeStartFlag = true;
				this.sw.Restart();
				this.CurrentTime = default(TimeSpan);
			}
			if (num > this.StartSpeed && this.BeforeStartFlag)
			{
				this.CurrentState = SpeedTestV1.TestStates.Measuring;
				this.CurrentTime = default(TimeSpan);
			}
		}

		// Token: 0x060015C5 RID: 5573 RVA: 0x0009A274 File Offset: 0x00098474
		public virtual void Cancel()
		{
			if (this.Reader != null && this.Reader.CurrentStatus == OBDDataReaderStatus.ConnectedToECU && this.SpeedPID != null)
			{
				(this.SpeedPID as PID).ValueChanged -= this.SpeedTest_ValueChanged;
			}
			this.CurrentState = SpeedTestV1.TestStates.StartConditionNotReady;
		}

		// Token: 0x14000009 RID: 9
		// (add) Token: 0x060015C6 RID: 5574 RVA: 0x0009A2C4 File Offset: 0x000984C4
		// (remove) Token: 0x060015C7 RID: 5575 RVA: 0x0009A2FC File Offset: 0x000984FC
		public event EventHandler TestCompleted
		{
			[CompilerGenerated]
			add
			{
				EventHandler eventHandler = this.TestCompleted;
				EventHandler eventHandler2;
				do
				{
					eventHandler2 = eventHandler;
					EventHandler eventHandler3 = (EventHandler)Delegate.Combine(eventHandler2, value);
					eventHandler = Interlocked.CompareExchange<EventHandler>(ref this.TestCompleted, eventHandler3, eventHandler2);
				}
				while (eventHandler != eventHandler2);
			}
			[CompilerGenerated]
			remove
			{
				EventHandler eventHandler = this.TestCompleted;
				EventHandler eventHandler2;
				do
				{
					eventHandler2 = eventHandler;
					EventHandler eventHandler3 = (EventHandler)Delegate.Remove(eventHandler2, value);
					eventHandler = Interlocked.CompareExchange<EventHandler>(ref this.TestCompleted, eventHandler3, eventHandler2);
				}
				while (eventHandler != eventHandler2);
			}
		}

		// Token: 0x060015C8 RID: 5576 RVA: 0x0009A334 File Offset: 0x00098534
		protected virtual void OnTestCompleted()
		{
			EventHandler testCompleted = this.TestCompleted;
			if (testCompleted != null)
			{
				testCompleted(this, EventArgs.Empty);
			}
			try
			{
				if (!App.OBDSimulator.IsActive)
				{
					DataRecorderV2 recorder = App.OBDReader.CurrentCarData.Recorder;
					if (recorder != null)
					{
						recorder.Record(this);
					}
				}
			}
			catch (Exception)
			{
			}
		}

		// Token: 0x17000F4D RID: 3917
		// (get) Token: 0x060015C9 RID: 5577 RVA: 0x0009A394 File Offset: 0x00098594
		// (set) Token: 0x060015CA RID: 5578 RVA: 0x0009A39C File Offset: 0x0009859C
		public string Value
		{
			get
			{
				return this._Value;
			}
			set
			{
				this._Value = value;
				this.NotifyPropertyChanged("Value");
			}
		}

		// Token: 0x17000F4E RID: 3918
		// (get) Token: 0x060015CB RID: 5579 RVA: 0x0009A3B0 File Offset: 0x000985B0
		// (set) Token: 0x060015CC RID: 5580 RVA: 0x0009A3B8 File Offset: 0x000985B8
		public string Name
		{
			[CompilerGenerated]
			get
			{
				return this.<Name>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<Name>k__BackingField = value;
			}
		}

		// Token: 0x17000F4F RID: 3919
		// (get) Token: 0x060015CD RID: 5581 RVA: 0x0009A3C1 File Offset: 0x000985C1
		// (set) Token: 0x060015CE RID: 5582 RVA: 0x0009A3C9 File Offset: 0x000985C9
		public string Description
		{
			[CompilerGenerated]
			get
			{
				return this.<Description>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<Description>k__BackingField = value;
			}
		}

		// Token: 0x17000F50 RID: 3920
		// (get) Token: 0x060015CF RID: 5583 RVA: 0x0009A3D2 File Offset: 0x000985D2
		// (set) Token: 0x060015D0 RID: 5584 RVA: 0x0009A3DA File Offset: 0x000985DA
		public bool IsTestFinished
		{
			[CompilerGenerated]
			get
			{
				return this.<IsTestFinished>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<IsTestFinished>k__BackingField = value;
			}
		}

		// Token: 0x1400000A RID: 10
		// (add) Token: 0x060015D1 RID: 5585 RVA: 0x0009A3E4 File Offset: 0x000985E4
		// (remove) Token: 0x060015D2 RID: 5586 RVA: 0x0009A41C File Offset: 0x0009861C
		public event PropertyChangedEventHandler PropertyChanged
		{
			[CompilerGenerated]
			add
			{
				PropertyChangedEventHandler propertyChangedEventHandler = this.PropertyChanged;
				PropertyChangedEventHandler propertyChangedEventHandler2;
				do
				{
					propertyChangedEventHandler2 = propertyChangedEventHandler;
					PropertyChangedEventHandler propertyChangedEventHandler3 = (PropertyChangedEventHandler)Delegate.Combine(propertyChangedEventHandler2, value);
					propertyChangedEventHandler = Interlocked.CompareExchange<PropertyChangedEventHandler>(ref this.PropertyChanged, propertyChangedEventHandler3, propertyChangedEventHandler2);
				}
				while (propertyChangedEventHandler != propertyChangedEventHandler2);
			}
			[CompilerGenerated]
			remove
			{
				PropertyChangedEventHandler propertyChangedEventHandler = this.PropertyChanged;
				PropertyChangedEventHandler propertyChangedEventHandler2;
				do
				{
					propertyChangedEventHandler2 = propertyChangedEventHandler;
					PropertyChangedEventHandler propertyChangedEventHandler3 = (PropertyChangedEventHandler)Delegate.Remove(propertyChangedEventHandler2, value);
					propertyChangedEventHandler = Interlocked.CompareExchange<PropertyChangedEventHandler>(ref this.PropertyChanged, propertyChangedEventHandler3, propertyChangedEventHandler2);
				}
				while (propertyChangedEventHandler != propertyChangedEventHandler2);
			}
		}

		// Token: 0x060015D3 RID: 5587 RVA: 0x0009A451 File Offset: 0x00098651
		public void Dispose()
		{
			if (this.SpeedPID != null)
			{
				(this.SpeedPID as PID).ValueChanged -= this.SpeedTest_ValueChanged;
			}
		}

		// Token: 0x060015D4 RID: 5588 RVA: 0x0009A478 File Offset: 0x00098678
		protected void NotifyPropertyChanged(string PropertyName)
		{
			PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
			if (propertyChanged != null)
			{
				Device.BeginInvokeOnMainThread(delegate
				{
					propertyChanged(this, new PropertyChangedEventArgs(PropertyName));
				});
			}
		}

		// Token: 0x060015D5 RID: 5589 RVA: 0x0009A4BE File Offset: 0x000986BE
		[CompilerGenerated]
		private void <set_CurrentState>b__21_0()
		{
			PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
			if (propertyChanged == null)
			{
				return;
			}
			propertyChanged(this, new PropertyChangedEventArgs("IsTestFinished"));
		}

		// Token: 0x04000630 RID: 1584
		[CompilerGenerated]
		private double <StartSpeed>k__BackingField;

		// Token: 0x04000631 RID: 1585
		[CompilerGenerated]
		private double <EndSpeed>k__BackingField;

		// Token: 0x04000632 RID: 1586
		protected IPIDFloatValue SpeedPID;

		// Token: 0x04000633 RID: 1587
		protected bool BeforeStartFlag;

		// Token: 0x04000634 RID: 1588
		protected Stopwatch sw;

		// Token: 0x04000635 RID: 1589
		private TimeSpan _CurrentTime;

		// Token: 0x04000636 RID: 1590
		private SpeedTestV1.TestStates _CurrentState;

		// Token: 0x04000637 RID: 1591
		[CompilerGenerated]
		private EventHandler TestCompleted;

		// Token: 0x04000638 RID: 1592
		private string _Value = "00:00.000";

		// Token: 0x04000639 RID: 1593
		[CompilerGenerated]
		private string <Name>k__BackingField;

		// Token: 0x0400063A RID: 1594
		[CompilerGenerated]
		private string <Description>k__BackingField;

		// Token: 0x0400063B RID: 1595
		[CompilerGenerated]
		private bool <IsTestFinished>k__BackingField;

		// Token: 0x0400063C RID: 1596
		[CompilerGenerated]
		private PropertyChangedEventHandler PropertyChanged;

		// Token: 0x02000186 RID: 390
		protected enum TestStates
		{
			// Token: 0x0400063E RID: 1598
			StartConditionNotReady,
			// Token: 0x0400063F RID: 1599
			Measuring,
			// Token: 0x04000640 RID: 1600
			Finished
		}

		// Token: 0x02000187 RID: 391
		[CompilerGenerated]
		private sealed class <>c__DisplayClass55_0
		{
			// Token: 0x060015D6 RID: 5590 RVA: 0x00002050 File Offset: 0x00000250
			public <>c__DisplayClass55_0()
			{
			}

			// Token: 0x060015D7 RID: 5591 RVA: 0x0009A4DB File Offset: 0x000986DB
			internal void <NotifyPropertyChanged>b__0()
			{
				this.propertyChanged(this.<>4__this, new PropertyChangedEventArgs(this.PropertyName));
			}

			// Token: 0x04000641 RID: 1601
			public PropertyChangedEventHandler propertyChanged;

			// Token: 0x04000642 RID: 1602
			public SpeedTestV1 <>4__this;

			// Token: 0x04000643 RID: 1603
			public string PropertyName;
		}
	}
}
