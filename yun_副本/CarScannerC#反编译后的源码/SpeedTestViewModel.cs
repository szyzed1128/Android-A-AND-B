using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using CarScannerXamarinForms.CarPlay;
using CarScannerXamarinForms.Common;
using CarScannerXamarinForms.OBD2.PIDS;
using CarScannerXamarinForms.Settings;
using CarScannerXamarinForms.ViewModels;
using Xamarin.Forms;

namespace CarScannerXamarinForms
{
	// Token: 0x02000188 RID: 392
	public class SpeedTestViewModel : INotifyPropertyChanged
	{
		// Token: 0x17000F51 RID: 3921
		// (get) Token: 0x060015D8 RID: 5592 RVA: 0x0009A4F9 File Offset: 0x000986F9
		public static SpeedTestViewModel Instance
		{
			get
			{
				if (SpeedTestViewModel._Instance == null)
				{
					SpeedTestViewModel._Instance = new SpeedTestViewModel();
				}
				return SpeedTestViewModel._Instance;
			}
		}

		// Token: 0x17000F52 RID: 3922
		// (get) Token: 0x060015D9 RID: 5593 RVA: 0x0009A511 File Offset: 0x00098711
		// (set) Token: 0x060015DA RID: 5594 RVA: 0x0009A519 File Offset: 0x00098719
		public IPIDFloatValue SpeedPID
		{
			[CompilerGenerated]
			get
			{
				return this.<SpeedPID>k__BackingField;
			}
			[CompilerGenerated]
			private set
			{
				this.<SpeedPID>k__BackingField = value;
			}
		}

		// Token: 0x060015DB RID: 5595 RVA: 0x0009A522 File Offset: 0x00098722
		private SpeedTestViewModel()
		{
			this.Initialize();
		}

		// Token: 0x060015DC RID: 5596 RVA: 0x0009A554 File Offset: 0x00098754
		public void Initialize()
		{
			this.SpeedPID = SpeedTestViewModel.GetSpeedPID();
			if (this.TestCollection != null && this.TestCollection.Count > 0)
			{
				foreach (ISpeedTest speedTest in this.TestCollection)
				{
					speedTest.RefreshSpeedPID();
				}
			}
		}

		// Token: 0x060015DD RID: 5597 RVA: 0x0009A5C0 File Offset: 0x000987C0
		public static IPIDFloatValue GetSpeedPID()
		{
			IPIDFloatValue SpeedPID = App.OBDReader.CurrentCarData.FindPIDByRole(Roles.Speed);
			if (SpeedPID == null)
			{
				SpeedPID = App.OBDReader.CurrentCarData.LiveDataPIDs.FirstOrDefault((PID x) => x.Id == 13) as IPIDFloatValue;
				if (!SpeedPID.IsAvailable)
				{
					string name = SpeedPID.Name;
					CustomPID customPID = CustomPIDViewModel.CurrentProfile.PidCollection.FirstOrDefault((CustomPID x) => x.Name == SpeedPID.Name);
					if (customPID != null)
					{
						SpeedPID = customPID;
					}
				}
			}
			return SpeedPID;
		}

		// Token: 0x17000F53 RID: 3923
		// (get) Token: 0x060015DE RID: 5598 RVA: 0x0009A674 File Offset: 0x00098874
		public ObservableCollection<ISpeedTest> TestCollection
		{
			get
			{
				return this._TestCollection;
			}
		}

		// Token: 0x17000F54 RID: 3924
		// (get) Token: 0x060015DF RID: 5599 RVA: 0x0009A67C File Offset: 0x0009887C
		// (set) Token: 0x060015E0 RID: 5600 RVA: 0x0009A684 File Offset: 0x00098884
		public string Speed
		{
			get
			{
				return this._Speed;
			}
			set
			{
				this._Speed = value;
				this.NotifyPropertyChanged("Speed");
			}
		}

		// Token: 0x17000F55 RID: 3925
		// (get) Token: 0x060015E1 RID: 5601 RVA: 0x0009A698 File Offset: 0x00098898
		// (set) Token: 0x060015E2 RID: 5602 RVA: 0x0009A6A0 File Offset: 0x000988A0
		public string Units
		{
			get
			{
				return this._Units;
			}
			set
			{
				this._Units = value;
				this.NotifyPropertyChanged("Units");
			}
		}

		// Token: 0x17000F56 RID: 3926
		// (get) Token: 0x060015E3 RID: 5603 RVA: 0x0009A6B4 File Offset: 0x000988B4
		// (set) Token: 0x060015E4 RID: 5604 RVA: 0x0009A6BC File Offset: 0x000988BC
		public bool IsStarted
		{
			[CompilerGenerated]
			get
			{
				return this.<IsStarted>k__BackingField;
			}
			[CompilerGenerated]
			private set
			{
				this.<IsStarted>k__BackingField = value;
			}
		}

		// Token: 0x060015E5 RID: 5605 RVA: 0x0009A6C8 File Offset: 0x000988C8
		public void Start()
		{
			this.Initialize();
			foreach (ISpeedTest speedTest in this.TestCollection)
			{
				speedTest.Start();
			}
			this.Speed = "0";
			if (this.SpeedPID == null || this.SpeedPID == null)
			{
				return;
			}
			this.SpeedPID.ValueChanged -= this.SpeedTestViewModel_ValueChanged;
			this.SpeedPID.ValueChanged += this.SpeedTestViewModel_ValueChanged;
			this.Units = UnitsHelper.GetCaption(this.SpeedPID.Units);
			this.IsStarted = true;
		}

		// Token: 0x060015E6 RID: 5606 RVA: 0x0009A780 File Offset: 0x00098980
		public void Stop()
		{
			this.SpeedPID.ValueChanged -= this.SpeedTestViewModel_ValueChanged;
			foreach (ISpeedTest speedTest in this.TestCollection)
			{
				speedTest.Cancel();
			}
			this.IsStarted = false;
		}

		// Token: 0x060015E7 RID: 5607 RVA: 0x0009A7E8 File Offset: 0x000989E8
		private void SpeedTestViewModel_ValueChanged(object sender, PID e)
		{
			IPIDFloatValue ipidfloatValue = e as IPIDFloatValue;
			this.Speed = UnitsHelper.GetValue(ipidfloatValue.Value, ipidfloatValue.Units).ToString("0.##", CultureInfo.InvariantCulture);
		}

		// Token: 0x060015E8 RID: 5608 RVA: 0x0009A828 File Offset: 0x00098A28
		protected virtual void NotifyPropertyChanged(string PropertyName)
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

		// Token: 0x1400000B RID: 11
		// (add) Token: 0x060015E9 RID: 5609 RVA: 0x0009A870 File Offset: 0x00098A70
		// (remove) Token: 0x060015EA RID: 5610 RVA: 0x0009A8A8 File Offset: 0x00098AA8
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

		// Token: 0x17000F57 RID: 3927
		// (get) Token: 0x060015EB RID: 5611 RVA: 0x0009A8DD File Offset: 0x00098ADD
		// (set) Token: 0x060015EC RID: 5612 RVA: 0x0009A8E5 File Offset: 0x00098AE5
		public bool WasLoaded
		{
			[CompilerGenerated]
			get
			{
				return this.<WasLoaded>k__BackingField;
			}
			[CompilerGenerated]
			private set
			{
				this.<WasLoaded>k__BackingField = value;
			}
		}

		// Token: 0x060015ED RID: 5613 RVA: 0x0009A8F0 File Offset: 0x00098AF0
		public void LoadFromFile()
		{
			this.TestCollection.Clear();
			bool flag = false;
			try
			{
				string speedTestDB = SharedSettings.Current.SpeedTestDB;
				if (speedTestDB == null)
				{
					this.TestCollection.Clear();
					if (SharedSettings.Current.Use_km)
					{
						this.TestCollection.Add(new SpeedTestV1(App.OBDReader, Translate.GetString("SpeedTest_AccelerationTime") + " 0-20 km/h", 0.0, 20.0));
						this.TestCollection.Add(new SpeedTestV1(App.OBDReader, Translate.GetString("SpeedTest_AccelerationTime") + " 0-30 km/h", 0.0, 30.0));
						this.TestCollection.Add(new SpeedTestV1(App.OBDReader, Translate.GetString("SpeedTest_AccelerationTime") + " 0-40 km/h", 0.0, 40.0));
						this.TestCollection.Add(new SpeedTestV1(App.OBDReader, Translate.GetString("SpeedTest_AccelerationTime") + " 0-60 km/h", 0.0, 60.0));
						this.TestCollection.Add(new SpeedTestV1(App.OBDReader, Translate.GetString("SpeedTest_AccelerationTime") + " 0-80 km/h", 0.0, 80.0));
						this.TestCollection.Add(new SpeedTestV1(App.OBDReader, Translate.GetString("SpeedTest_AccelerationTime") + " 80-120 km/h", 80.0, 120.0));
						this.TestCollection.Add(new SpeedTestV1(App.OBDReader, Translate.GetString("SpeedTest_AccelerationTime") + " 0-100 km/h", 0.0, 100.0));
						this.TestCollection.Add(new SpeedTestV1(App.OBDReader, Translate.GetString("SpeedTest_AccelerationTime") + " 0-120 km/h", 0.0, 120.0));
						this.TestCollection.Add(new SpeedTestV1(App.OBDReader, Translate.GetString("SpeedTest_AccelerationTime") + " 100-200 km/h", 0.0, 200.0));
						this.TestCollection.Add(new SpeedTestV1(App.OBDReader, Translate.GetString("SpeedTest_AccelerationTime") + " 0-200 km/h", 0.0, 200.0));
					}
					else
					{
						this.TestCollection.Add(new SpeedTestV1(App.OBDReader, Translate.GetString("SpeedTest_AccelerationTime") + " 0-20 mph", 0.0, 20.0));
						this.TestCollection.Add(new SpeedTestV1(App.OBDReader, Translate.GetString("SpeedTest_AccelerationTime") + " 0-40 mph", 0.0, 40.0));
						this.TestCollection.Add(new SpeedTestV1(App.OBDReader, Translate.GetString("SpeedTest_AccelerationTime") + " 0-60 mph", 0.0, 60.0));
						this.TestCollection.Add(new SpeedTestV1(App.OBDReader, Translate.GetString("SpeedTest_AccelerationTime") + " 0-80 mph", 0.0, 80.0));
						this.TestCollection.Add(new SpeedTestV1(App.OBDReader, Translate.GetString("SpeedTest_AccelerationTime") + " 0-100 mph", 0.0, 100.0));
					}
				}
				else
				{
					ProxyTest[] array = (from x in Json.DeserializeObject<List<ProxyTest>>(speedTestDB)
						orderby x.TestType, x.StartSpeed, x.EndSpeed
						select x).ToArray<ProxyTest>();
					for (int i = 0; i < array.Length; i++)
					{
						ISpeedTest speedTest = ProxyTest.GetSpeedTest(array[i]);
						this.TestCollection.Add(speedTest);
						speedTest.Start();
					}
				}
				this.WasLoaded = true;
				flag = true;
			}
			catch (Exception)
			{
				flag = false;
			}
			if (!flag)
			{
				this.TestCollection.Clear();
				this.TestCollection.Add(new SpeedTestV1(App.OBDReader, Translate.GetString("SpeedTest_AccelerationTime"), 0.0, 60.0));
				this.TestCollection.Add(new SpeedTestV1(App.OBDReader, Translate.GetString("SpeedTest_AccelerationTime"), 0.0, 100.0));
				this.WasLoaded = true;
			}
		}

		// Token: 0x060015EE RID: 5614 RVA: 0x0009ADFC File Offset: 0x00098FFC
		public void SaveToFile()
		{
			string text = Json.SerializeObject(this.TestCollection.Select((ISpeedTest x) => ProxyTest.GetProxyTest(x)).ToList<ProxyTest>());
			SharedSettings.Current.SpeedTestDB = text;
			CarPlayManager instance = CarPlayManager.Instance;
			if (instance == null)
			{
				return;
			}
			instance.OnAccelerationConfigurationChanged();
		}

		// Token: 0x04000644 RID: 1604
		private static SpeedTestViewModel _Instance;

		// Token: 0x04000645 RID: 1605
		[CompilerGenerated]
		private IPIDFloatValue <SpeedPID>k__BackingField;

		// Token: 0x04000646 RID: 1606
		private ObservableCollection<ISpeedTest> _TestCollection = new ObservableCollection<ISpeedTest>();

		// Token: 0x04000647 RID: 1607
		private string _Speed = "";

		// Token: 0x04000648 RID: 1608
		private string _Units = "";

		// Token: 0x04000649 RID: 1609
		[CompilerGenerated]
		private bool <IsStarted>k__BackingField;

		// Token: 0x0400064A RID: 1610
		[CompilerGenerated]
		private PropertyChangedEventHandler PropertyChanged;

		// Token: 0x0400064B RID: 1611
		[CompilerGenerated]
		private bool <WasLoaded>k__BackingField;

		// Token: 0x02000189 RID: 393
		[CompilerGenerated]
		[Serializable]
		private sealed class <>c
		{
			// Token: 0x060015EF RID: 5615 RVA: 0x0009AE58 File Offset: 0x00099058
			// Note: this type is marked as 'beforefieldinit'.
			static <>c()
			{
			}

			// Token: 0x060015F0 RID: 5616 RVA: 0x00002050 File Offset: 0x00000250
			public <>c()
			{
			}

			// Token: 0x060015F1 RID: 5617 RVA: 0x0009AE64 File Offset: 0x00099064
			internal bool <GetSpeedPID>b__9_0(PID x)
			{
				return x.Id == 13;
			}

			// Token: 0x060015F2 RID: 5618 RVA: 0x0009AE70 File Offset: 0x00099070
			internal ProxyTest.TestTypes <LoadFromFile>b__36_0(ProxyTest x)
			{
				return x.TestType;
			}

			// Token: 0x060015F3 RID: 5619 RVA: 0x0009AE78 File Offset: 0x00099078
			internal double <LoadFromFile>b__36_1(ProxyTest x)
			{
				return x.StartSpeed;
			}

			// Token: 0x060015F4 RID: 5620 RVA: 0x0009AE80 File Offset: 0x00099080
			internal double <LoadFromFile>b__36_2(ProxyTest x)
			{
				return x.EndSpeed;
			}

			// Token: 0x060015F5 RID: 5621 RVA: 0x0009AE88 File Offset: 0x00099088
			internal ProxyTest <SaveToFile>b__37_0(ISpeedTest x)
			{
				return ProxyTest.GetProxyTest(x);
			}

			// Token: 0x0400064C RID: 1612
			public static readonly SpeedTestViewModel.<>c <>9 = new SpeedTestViewModel.<>c();

			// Token: 0x0400064D RID: 1613
			public static Func<PID, bool> <>9__9_0;

			// Token: 0x0400064E RID: 1614
			public static Func<ProxyTest, ProxyTest.TestTypes> <>9__36_0;

			// Token: 0x0400064F RID: 1615
			public static Func<ProxyTest, double> <>9__36_1;

			// Token: 0x04000650 RID: 1616
			public static Func<ProxyTest, double> <>9__36_2;

			// Token: 0x04000651 RID: 1617
			public static Func<ISpeedTest, ProxyTest> <>9__37_0;
		}

		// Token: 0x0200018A RID: 394
		[CompilerGenerated]
		private sealed class <>c__DisplayClass28_0
		{
			// Token: 0x060015F6 RID: 5622 RVA: 0x00002050 File Offset: 0x00000250
			public <>c__DisplayClass28_0()
			{
			}

			// Token: 0x060015F7 RID: 5623 RVA: 0x0009AE90 File Offset: 0x00099090
			internal void <NotifyPropertyChanged>b__0()
			{
				this.propertyChanged(this.<>4__this, new PropertyChangedEventArgs(this.PropertyName));
			}

			// Token: 0x04000652 RID: 1618
			public PropertyChangedEventHandler propertyChanged;

			// Token: 0x04000653 RID: 1619
			public SpeedTestViewModel <>4__this;

			// Token: 0x04000654 RID: 1620
			public string PropertyName;
		}

		// Token: 0x0200018B RID: 395
		[CompilerGenerated]
		private sealed class <>c__DisplayClass9_0
		{
			// Token: 0x060015F8 RID: 5624 RVA: 0x00002050 File Offset: 0x00000250
			public <>c__DisplayClass9_0()
			{
			}

			// Token: 0x060015F9 RID: 5625 RVA: 0x0009AEAE File Offset: 0x000990AE
			internal bool <GetSpeedPID>b__1(CustomPID x)
			{
				return x.Name == this.SpeedPID.Name;
			}

			// Token: 0x04000655 RID: 1621
			public IPIDFloatValue SpeedPID;
		}
	}
}
