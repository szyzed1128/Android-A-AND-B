using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Input;
using CarScannerXamarinForms.Common;
using CarScannerXamarinForms.DriveCycles;
using CarScannerXamarinForms.InApp;
using CarScannerXamarinForms.OBD2;
using CarScannerXamarinForms.OBD2.PIDS;
using CarScannerXamarinForms.Pages;
using CarScannerXamarinForms.Settings;
using Xamarin.Forms;

namespace CarScannerXamarinForms.ViewModels
{
	// Token: 0x02000718 RID: 1816
	public class DriveCycleViewModel : INotifyPropertyChanged
	{
		// Token: 0x17001428 RID: 5160
		// (get) Token: 0x06003DA9 RID: 15785 RVA: 0x003290E6 File Offset: 0x003272E6
		public static DriveCycleViewModel Current
		{
			get
			{
				if (DriveCycleViewModel._Current == null)
				{
					DriveCycleViewModel._Current = new DriveCycleViewModel();
				}
				return DriveCycleViewModel._Current;
			}
		}

		// Token: 0x06003DAA RID: 15786 RVA: 0x003290FE File Offset: 0x003272FE
		public static void Reload()
		{
			DriveCycleViewModel._Current = null;
		}

		// Token: 0x06003DAB RID: 15787 RVA: 0x00329106 File Offset: 0x00327306
		private DriveCycleViewModel()
		{
			this.Initialize();
		}

		// Token: 0x06003DAC RID: 15788 RVA: 0x0032912C File Offset: 0x0032732C
		private void Initialize()
		{
			this.PeriodStartDate = default(DateTime);
			this.PeriodEndDate = DateTimeNowHelper.NowSafe;
			this.LoadFromFile();
			this.CalculateTotal();
		}

		// Token: 0x06003DAD RID: 15789 RVA: 0x0032915F File Offset: 0x0032735F
		public void SetCustomPeriod(DateTime start, DateTime end)
		{
			this.PeriodStartDate = start;
			this.PeriodEndDate = end;
			this.RefreshPeriodForStatisticsScreen();
		}

		// Token: 0x06003DAE RID: 15790 RVA: 0x00329175 File Offset: 0x00327375
		public void RefreshPeriodForStatisticsScreenWithoutRebuildingDays()
		{
			this.CalculatePeriod(this.PeriodStartDate, this.PeriodEndDate);
		}

		// Token: 0x06003DAF RID: 15791 RVA: 0x0032918C File Offset: 0x0032738C
		public void RefreshPeriodForStatisticsScreen()
		{
			IEnumerable<DriveCycle> enumerable = this.DriveCycles.Where((DriveCycle dc) => this.PeriodStartDate <= dc.TimeStarted.Date && dc.TimeStarted.Date <= this.PeriodEndDate);
			IEnumerable<DayDriveCycle> enumerable2 = this.SeparateByDays(enumerable);
			this.DayCycles.Clear();
			foreach (DayDriveCycle dayDriveCycle in enumerable2)
			{
				this.DayCycles.Add(dayDriveCycle);
			}
			this.CalculatePeriod(this.PeriodStartDate, this.PeriodEndDate);
		}

		// Token: 0x06003DB0 RID: 15792 RVA: 0x00329214 File Offset: 0x00327414
		public void Reset()
		{
			this.TotalAvgFuelConsumption = 0.0;
			this.TotalAvgSpeed = 0.0;
			this.TotalDistance = 0.0;
			this.TotalFuelUsed = 0.0;
			this.TotalFuelPrice = 0m;
			this.DayCycles.Clear();
			this._DriveCycles.Clear();
			DriveCycle driveCycle = DriveCycle.Current;
			if (driveCycle != null)
			{
				driveCycle.Distance = 0.0;
				driveCycle.FuelUsed = 0.0;
				driveCycle.TimeStarted = DateTimeNowHelper.NowSafe;
			}
			this.DeleteFiles();
			this._DriveCycles.Clear();
			this.Initialize();
		}

		// Token: 0x06003DB1 RID: 15793 RVA: 0x003292C8 File Offset: 0x003274C8
		private void DeleteFiles()
		{
			string localFilePath = FileSystemHelper.GetLocalFilePath("drivecycles.bin");
			if (File.Exists(localFilePath))
			{
				File.Delete(localFilePath);
			}
		}

		// Token: 0x06003DB2 RID: 15794 RVA: 0x003292F0 File Offset: 0x003274F0
		protected void OnPropertyChanged(string propertyName)
		{
			PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
			if (propertyChanged != null)
			{
				propertyChanged(this, new PropertyChangedEventArgs(propertyName));
			}
		}

		// Token: 0x17001429 RID: 5161
		// (get) Token: 0x06003DB3 RID: 15795 RVA: 0x00329314 File Offset: 0x00327514
		public IReadOnlyList<DriveCycle> DriveCycles
		{
			get
			{
				return this._DriveCycles;
			}
		}

		// Token: 0x06003DB4 RID: 15796 RVA: 0x0032931C File Offset: 0x0032751C
		public void AddDriveCycle(DriveCycle dc)
		{
			this._DriveCycles.Add(dc);
			this.CalculateTotal();
		}

		// Token: 0x06003DB5 RID: 15797 RVA: 0x00329330 File Offset: 0x00327530
		public void DeleteDriveCycle(DriveCycle dc)
		{
			this._DriveCycles.Remove(dc);
			this.CalculateTotal();
		}

		// Token: 0x06003DB6 RID: 15798 RVA: 0x00329348 File Offset: 0x00327548
		public void DeleteDriveCycles(IEnumerable<DriveCycle> dcs)
		{
			foreach (DriveCycle driveCycle in dcs)
			{
				this._DriveCycles.Remove(driveCycle);
			}
			this.CalculateTotal();
			DriveCycle.SaveRange(this._DriveCycles);
		}

		// Token: 0x1700142A RID: 5162
		// (get) Token: 0x06003DB7 RID: 15799 RVA: 0x003293A8 File Offset: 0x003275A8
		public SmartCollection<DayDriveCycle> DayCycles
		{
			get
			{
				return this._DayCycles;
			}
		}

		// Token: 0x1700142B RID: 5163
		// (get) Token: 0x06003DB8 RID: 15800 RVA: 0x003293B0 File Offset: 0x003275B0
		public ICommand PeriodAllTime
		{
			get
			{
				return new Command(delegate
				{
					this.PeriodEndDate = DateTime.MaxValue;
					this.PeriodStartDate = new DateTime(2001, 1, 1, 0, 0, 1);
					this.RefreshPeriodForStatisticsScreen();
				});
			}
		}

		// Token: 0x1700142C RID: 5164
		// (get) Token: 0x06003DB9 RID: 15801 RVA: 0x003293C3 File Offset: 0x003275C3
		public ICommand PeriodToday
		{
			get
			{
				return new Command(async delegate
				{
					if (!SharedSettings.Current.AdsProductPurchased)
					{
						page = FuelStatisticsPage.Instance;
						if (page != null)
						{
							TaskAwaiter<bool> taskAwaiter3 = page.DisplayAlert("Car Scanner Pro", Translate.GetString("FeatureLocked"), Translate.GetString("ios_Yes"), Translate.GetString("ios_No")).GetAwaiter();
							if (!taskAwaiter3.IsCompleted)
							{
								await taskAwaiter3;
								taskAwaiter3 = taskAwaiter2;
								taskAwaiter2 = default(TaskAwaiter<bool>);
							}
							if (taskAwaiter3.GetResult())
							{
								await page.Navigation.PushAsync(InAppManager.GetInAppPage());
							}
						}
					}
					else
					{
						this.PeriodEndDate = DateTimeNowHelper.NowSafe;
						this.PeriodStartDate = DateTimeNowHelper.NowSafe.Date;
						this.RefreshPeriodForStatisticsScreen();
					}
				});
			}
		}

		// Token: 0x1700142D RID: 5165
		// (get) Token: 0x06003DBA RID: 15802 RVA: 0x003293D6 File Offset: 0x003275D6
		public ICommand Period7Days
		{
			get
			{
				return new Command(async delegate
				{
					if (!SharedSettings.Current.AdsProductPurchased)
					{
						page = FuelStatisticsPage.Instance;
						if (page != null)
						{
							TaskAwaiter<bool> taskAwaiter3 = page.DisplayAlert("Car Scanner Pro", Translate.GetString("FeatureLocked"), Translate.GetString("ios_Yes"), Translate.GetString("ios_No")).GetAwaiter();
							if (!taskAwaiter3.IsCompleted)
							{
								await taskAwaiter3;
								taskAwaiter3 = taskAwaiter2;
								taskAwaiter2 = default(TaskAwaiter<bool>);
							}
							if (taskAwaiter3.GetResult())
							{
								await page.Navigation.PushAsync(InAppManager.GetInAppPage());
							}
						}
					}
					else
					{
						TimeSpan timeSpan = new TimeSpan(DateTimeNowHelper.NowSafe.Date.Ticks);
						TimeSpan timeSpan2 = timeSpan - TimeSpan.FromDays(7.0);
						this.PeriodEndDate = new DateTime(timeSpan.Ticks);
						this.PeriodStartDate = new DateTime(timeSpan2.Ticks);
						this.RefreshPeriodForStatisticsScreen();
					}
				});
			}
		}

		// Token: 0x1700142E RID: 5166
		// (get) Token: 0x06003DBB RID: 15803 RVA: 0x003293E9 File Offset: 0x003275E9
		public ICommand Period14Days
		{
			get
			{
				return new Command(async delegate
				{
					if (!SharedSettings.Current.AdsProductPurchased)
					{
						page = FuelStatisticsPage.Instance;
						if (page != null)
						{
							TaskAwaiter<bool> taskAwaiter3 = page.DisplayAlert("Car Scanner Pro", Translate.GetString("FeatureLocked"), Translate.GetString("ios_Yes"), Translate.GetString("ios_No")).GetAwaiter();
							if (!taskAwaiter3.IsCompleted)
							{
								await taskAwaiter3;
								taskAwaiter3 = taskAwaiter2;
								taskAwaiter2 = default(TaskAwaiter<bool>);
							}
							if (taskAwaiter3.GetResult())
							{
								await page.Navigation.PushAsync(InAppManager.GetInAppPage());
							}
						}
					}
					else
					{
						TimeSpan timeSpan = new TimeSpan(DateTimeNowHelper.NowSafe.Date.Ticks);
						TimeSpan timeSpan2 = timeSpan - TimeSpan.FromDays(14.0);
						this.PeriodEndDate = new DateTime(timeSpan.Ticks);
						this.PeriodStartDate = new DateTime(timeSpan2.Ticks);
						this.RefreshPeriodForStatisticsScreen();
					}
				});
			}
		}

		// Token: 0x1700142F RID: 5167
		// (get) Token: 0x06003DBC RID: 15804 RVA: 0x003293FC File Offset: 0x003275FC
		public ICommand Period30Days
		{
			get
			{
				return new Command(async delegate
				{
					if (!SharedSettings.Current.AdsProductPurchased)
					{
						page = FuelStatisticsPage.Instance;
						if (page != null)
						{
							TaskAwaiter<bool> taskAwaiter3 = page.DisplayAlert("Car Scanner Pro", Translate.GetString("FeatureLocked"), Translate.GetString("ios_Yes"), Translate.GetString("ios_No")).GetAwaiter();
							if (!taskAwaiter3.IsCompleted)
							{
								await taskAwaiter3;
								taskAwaiter3 = taskAwaiter2;
								taskAwaiter2 = default(TaskAwaiter<bool>);
							}
							if (taskAwaiter3.GetResult())
							{
								await page.Navigation.PushAsync(InAppManager.GetInAppPage());
							}
						}
					}
					else
					{
						TimeSpan timeSpan = new TimeSpan(DateTimeNowHelper.NowSafe.Date.Ticks);
						TimeSpan timeSpan2 = timeSpan - TimeSpan.FromDays(30.0);
						this.PeriodEndDate = new DateTime(timeSpan.Ticks);
						this.PeriodStartDate = new DateTime(timeSpan2.Ticks);
						this.RefreshPeriodForStatisticsScreen();
					}
				});
			}
		}

		// Token: 0x17001430 RID: 5168
		// (get) Token: 0x06003DBD RID: 15805 RVA: 0x0032940F File Offset: 0x0032760F
		public ICommand Period90Days
		{
			get
			{
				return new Command(async delegate
				{
					if (!SharedSettings.Current.AdsProductPurchased)
					{
						page = FuelStatisticsPage.Instance;
						if (page != null)
						{
							TaskAwaiter<bool> taskAwaiter3 = page.DisplayAlert("Car Scanner Pro", Translate.GetString("FeatureLocked"), Translate.GetString("ios_Yes"), Translate.GetString("ios_No")).GetAwaiter();
							if (!taskAwaiter3.IsCompleted)
							{
								await taskAwaiter3;
								taskAwaiter3 = taskAwaiter2;
								taskAwaiter2 = default(TaskAwaiter<bool>);
							}
							if (taskAwaiter3.GetResult())
							{
								await page.Navigation.PushAsync(InAppManager.GetInAppPage());
							}
						}
					}
					else
					{
						TimeSpan timeSpan = new TimeSpan(DateTimeNowHelper.NowSafe.Date.Ticks);
						TimeSpan timeSpan2 = timeSpan - TimeSpan.FromDays(90.0);
						this.PeriodEndDate = new DateTime(timeSpan.Ticks);
						this.PeriodStartDate = new DateTime(timeSpan2.Ticks);
						this.RefreshPeriodForStatisticsScreen();
					}
				});
			}
		}

		// Token: 0x17001431 RID: 5169
		// (get) Token: 0x06003DBE RID: 15806 RVA: 0x00329422 File Offset: 0x00327622
		public ICommand Period180Days
		{
			get
			{
				return new Command(async delegate
				{
					if (!SharedSettings.Current.AdsProductPurchased)
					{
						page = FuelStatisticsPage.Instance;
						if (page != null)
						{
							TaskAwaiter<bool> taskAwaiter3 = page.DisplayAlert("Car Scanner Pro", Translate.GetString("FeatureLocked"), Translate.GetString("ios_Yes"), Translate.GetString("ios_No")).GetAwaiter();
							if (!taskAwaiter3.IsCompleted)
							{
								await taskAwaiter3;
								taskAwaiter3 = taskAwaiter2;
								taskAwaiter2 = default(TaskAwaiter<bool>);
							}
							if (taskAwaiter3.GetResult())
							{
								await page.Navigation.PushAsync(InAppManager.GetInAppPage());
							}
						}
					}
					else
					{
						TimeSpan timeSpan = new TimeSpan(DateTimeNowHelper.NowSafe.Date.Ticks);
						TimeSpan timeSpan2 = timeSpan - TimeSpan.FromDays(180.0);
						this.PeriodEndDate = new DateTime(timeSpan.Ticks);
						this.PeriodStartDate = new DateTime(timeSpan2.Ticks);
						this.RefreshPeriodForStatisticsScreen();
					}
				});
			}
		}

		// Token: 0x17001432 RID: 5170
		// (get) Token: 0x06003DBF RID: 15807 RVA: 0x00329435 File Offset: 0x00327635
		public ICommand Period360Days
		{
			get
			{
				return new Command(async delegate
				{
					if (!SharedSettings.Current.AdsProductPurchased)
					{
						page = FuelStatisticsPage.Instance;
						if (page != null)
						{
							TaskAwaiter<bool> taskAwaiter3 = page.DisplayAlert("Car Scanner Pro", Translate.GetString("FeatureLocked"), Translate.GetString("ios_Yes"), Translate.GetString("ios_No")).GetAwaiter();
							if (!taskAwaiter3.IsCompleted)
							{
								await taskAwaiter3;
								taskAwaiter3 = taskAwaiter2;
								taskAwaiter2 = default(TaskAwaiter<bool>);
							}
							if (taskAwaiter3.GetResult())
							{
								await page.Navigation.PushAsync(InAppManager.GetInAppPage());
							}
						}
					}
					else
					{
						TimeSpan timeSpan = new TimeSpan(DateTimeNowHelper.NowSafe.Date.Ticks);
						TimeSpan timeSpan2 = timeSpan - TimeSpan.FromDays(360.0);
						this.PeriodEndDate = new DateTime(timeSpan.Ticks);
						this.PeriodStartDate = new DateTime(timeSpan2.Ticks);
						this.RefreshPeriodForStatisticsScreen();
					}
				});
			}
		}

		// Token: 0x17001433 RID: 5171
		// (get) Token: 0x06003DC0 RID: 15808 RVA: 0x00329448 File Offset: 0x00327648
		// (set) Token: 0x06003DC1 RID: 15809 RVA: 0x00329450 File Offset: 0x00327650
		public DateTime PeriodStartDate
		{
			[CompilerGenerated]
			get
			{
				return this.<PeriodStartDate>k__BackingField;
			}
			[CompilerGenerated]
			private set
			{
				this.<PeriodStartDate>k__BackingField = value;
			}
		}

		// Token: 0x17001434 RID: 5172
		// (get) Token: 0x06003DC2 RID: 15810 RVA: 0x00329459 File Offset: 0x00327659
		// (set) Token: 0x06003DC3 RID: 15811 RVA: 0x00329461 File Offset: 0x00327661
		public DateTime PeriodEndDate
		{
			[CompilerGenerated]
			get
			{
				return this.<PeriodEndDate>k__BackingField;
			}
			[CompilerGenerated]
			private set
			{
				this.<PeriodEndDate>k__BackingField = value;
			}
		}

		// Token: 0x06003DC4 RID: 15812 RVA: 0x0032946C File Offset: 0x0032766C
		private void LoadFromFile()
		{
			this._DriveCycles.Clear();
			Stopwatch stopwatch = new Stopwatch();
			stopwatch.Start();
			List<DriveCycle> list = DriveCycle.LoadDriveCycles();
			long elapsedMilliseconds = stopwatch.ElapsedMilliseconds;
			foreach (DriveCycle driveCycle in list)
			{
				if (driveCycle != null && !double.IsInfinity(driveCycle.FuelUsed) && !double.IsNaN(driveCycle.FuelUsed) && !double.IsInfinity(driveCycle.Distance) && !double.IsNaN(driveCycle.Distance) && (driveCycle.FuelUsed > 0.0 || (SharedSettings.Current.FuelHybridCar && driveCycle.Distance > 0.0)))
				{
					this._DriveCycles.Add(driveCycle);
				}
			}
			stopwatch.Stop();
			long elapsedMilliseconds2 = stopwatch.ElapsedMilliseconds;
		}

		// Token: 0x1400003C RID: 60
		// (add) Token: 0x06003DC5 RID: 15813 RVA: 0x00329564 File Offset: 0x00327764
		// (remove) Token: 0x06003DC6 RID: 15814 RVA: 0x0032959C File Offset: 0x0032779C
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

		// Token: 0x17001435 RID: 5173
		// (get) Token: 0x06003DC7 RID: 15815 RVA: 0x003295D1 File Offset: 0x003277D1
		// (set) Token: 0x06003DC8 RID: 15816 RVA: 0x003295D9 File Offset: 0x003277D9
		public double PeriodFuelUsed
		{
			get
			{
				return this._PeriodFuelUsed;
			}
			set
			{
				if (Math.Abs(value - this._PeriodFuelUsed) > 5E-324)
				{
					this._PeriodFuelUsed = value;
					this.OnPropertyChanged("PeriodFuelUsed");
				}
			}
		}

		// Token: 0x17001436 RID: 5174
		// (get) Token: 0x06003DC9 RID: 15817 RVA: 0x00329605 File Offset: 0x00327805
		// (set) Token: 0x06003DCA RID: 15818 RVA: 0x0032960D File Offset: 0x0032780D
		public decimal PeriodFuelPrice
		{
			get
			{
				return this._PeriodFuelPrice;
			}
			set
			{
				if (value != this._PeriodFuelPrice)
				{
					this._PeriodFuelPrice = value;
					this.OnPropertyChanged("PeriodFuelPrice");
				}
			}
		}

		// Token: 0x17001437 RID: 5175
		// (get) Token: 0x06003DCB RID: 15819 RVA: 0x0032962F File Offset: 0x0032782F
		// (set) Token: 0x06003DCC RID: 15820 RVA: 0x00329637 File Offset: 0x00327837
		public double PeriodDistance
		{
			get
			{
				return this._PeriodDistance;
			}
			set
			{
				if (Math.Abs(value - this._PeriodDistance) > 5E-324)
				{
					this._PeriodDistance = value;
					this.OnPropertyChanged("PeriodDistance");
					this.OnPropertyChanged("PeriodMotorHours");
				}
			}
		}

		// Token: 0x17001438 RID: 5176
		// (get) Token: 0x06003DCD RID: 15821 RVA: 0x0032966E File Offset: 0x0032786E
		// (set) Token: 0x06003DCE RID: 15822 RVA: 0x0032969A File Offset: 0x0032789A
		public double PeriodAvgFuelConsumption
		{
			get
			{
				if (double.IsInfinity(this._PeriodAvgFuelConsumption) || double.IsNaN(this._PeriodAvgFuelConsumption))
				{
					return 0.0;
				}
				return this._PeriodAvgFuelConsumption;
			}
			set
			{
				if (Math.Abs(value - this._PeriodAvgFuelConsumption) > 5E-324)
				{
					this._PeriodAvgFuelConsumption = value;
					this.OnPropertyChanged("PeriodAvgFuelConsumption");
				}
			}
		}

		// Token: 0x17001439 RID: 5177
		// (get) Token: 0x06003DCF RID: 15823 RVA: 0x003296C8 File Offset: 0x003278C8
		public double PeriodMotorHours
		{
			get
			{
				double num = Math.Round(this.PeriodDistance / this.PeriodAvgSpeed, 1);
				if (double.IsInfinity(num) || double.IsNaN(num))
				{
					return 0.0;
				}
				return num;
			}
		}

		// Token: 0x1700143A RID: 5178
		// (get) Token: 0x06003DD0 RID: 15824 RVA: 0x00329704 File Offset: 0x00327904
		// (set) Token: 0x06003DD1 RID: 15825 RVA: 0x0032970C File Offset: 0x0032790C
		public double PeriodAvgSpeed
		{
			get
			{
				return this._PeriodAvgSpeed;
			}
			set
			{
				if (Math.Abs(value - this._PeriodAvgSpeed) > 5E-324)
				{
					this._PeriodAvgSpeed = value;
					this.OnPropertyChanged("PeriodAvgSpeed");
					this.OnPropertyChanged("PeriodMotorHours");
				}
			}
		}

		// Token: 0x1700143B RID: 5179
		// (get) Token: 0x06003DD2 RID: 15826 RVA: 0x00329743 File Offset: 0x00327943
		// (set) Token: 0x06003DD3 RID: 15827 RVA: 0x0032974B File Offset: 0x0032794B
		public TimeSpan PeriodDuration
		{
			get
			{
				return this._PeriodDuration;
			}
			set
			{
				if (this._PeriodDuration != value)
				{
					this._PeriodDuration = value;
					this.OnPropertyChanged("PeriodDuration");
				}
			}
		}

		// Token: 0x1700143C RID: 5180
		// (get) Token: 0x06003DD4 RID: 15828 RVA: 0x0032976D File Offset: 0x0032796D
		// (set) Token: 0x06003DD5 RID: 15829 RVA: 0x00329775 File Offset: 0x00327975
		public double TotalFuelUsed
		{
			get
			{
				return this._TotalFuelUsed;
			}
			set
			{
				if (Math.Abs(value - this._TotalFuelUsed) > 5E-324)
				{
					this._TotalFuelUsed = value;
					this.OnPropertyChanged("TotalFuelUsed");
				}
			}
		}

		// Token: 0x1700143D RID: 5181
		// (get) Token: 0x06003DD6 RID: 15830 RVA: 0x003297A1 File Offset: 0x003279A1
		// (set) Token: 0x06003DD7 RID: 15831 RVA: 0x003297A9 File Offset: 0x003279A9
		public decimal TotalFuelPrice
		{
			get
			{
				return this._TotalFuelPrice;
			}
			set
			{
				if (value != this._TotalFuelPrice)
				{
					this._TotalFuelPrice = value;
					this.OnPropertyChanged("TotalFuelPrice");
				}
			}
		}

		// Token: 0x1700143E RID: 5182
		// (get) Token: 0x06003DD8 RID: 15832 RVA: 0x003297CB File Offset: 0x003279CB
		// (set) Token: 0x06003DD9 RID: 15833 RVA: 0x003297D3 File Offset: 0x003279D3
		public double TotalDistance
		{
			get
			{
				return this._TotalDistance;
			}
			set
			{
				if (Math.Abs(value - this._TotalDistance) > 5E-324)
				{
					this._TotalDistance = value;
					this.OnPropertyChanged("TotalDistance");
					this.OnPropertyChanged("TotalMotorHours");
				}
			}
		}

		// Token: 0x1700143F RID: 5183
		// (get) Token: 0x06003DDA RID: 15834 RVA: 0x0032980A File Offset: 0x00327A0A
		// (set) Token: 0x06003DDB RID: 15835 RVA: 0x00329836 File Offset: 0x00327A36
		public double TotalAvgFuelConsumption
		{
			get
			{
				if (double.IsInfinity(this._TotalAvgFuelConsumption) || double.IsNaN(this._TotalAvgFuelConsumption))
				{
					return 0.0;
				}
				return this._TotalAvgFuelConsumption;
			}
			set
			{
				if (Math.Abs(value - this._TotalAvgFuelConsumption) > 5E-324)
				{
					this._TotalAvgFuelConsumption = value;
					this.OnPropertyChanged("TotalAvgFuelConsumption");
				}
			}
		}

		// Token: 0x17001440 RID: 5184
		// (get) Token: 0x06003DDC RID: 15836 RVA: 0x00329864 File Offset: 0x00327A64
		public double TotalMotorHours
		{
			get
			{
				double num = Math.Round(this.TotalDistance / this.TotalAvgSpeed, 1);
				if (double.IsInfinity(num) || double.IsNaN(num))
				{
					return 0.0;
				}
				return num;
			}
		}

		// Token: 0x17001441 RID: 5185
		// (get) Token: 0x06003DDD RID: 15837 RVA: 0x003298A0 File Offset: 0x00327AA0
		// (set) Token: 0x06003DDE RID: 15838 RVA: 0x003298A8 File Offset: 0x00327AA8
		public double TotalAvgSpeed
		{
			get
			{
				return this._TotalAvgSpeed;
			}
			set
			{
				if (Math.Abs(value - this._TotalAvgSpeed) > 5E-324)
				{
					this._TotalAvgSpeed = value;
					this.OnPropertyChanged("TotalAvgSpeed");
					this.OnPropertyChanged("TotalMotorHours");
				}
			}
		}

		// Token: 0x17001442 RID: 5186
		// (get) Token: 0x06003DDF RID: 15839 RVA: 0x003298DF File Offset: 0x00327ADF
		// (set) Token: 0x06003DE0 RID: 15840 RVA: 0x003298E7 File Offset: 0x00327AE7
		public TimeSpan TotalDuration
		{
			get
			{
				return this._TotalDuration;
			}
			set
			{
				if (this._TotalDuration != value)
				{
					this._TotalDuration = value;
					this.OnPropertyChanged("TotalDuration");
				}
			}
		}

		// Token: 0x17001443 RID: 5187
		// (get) Token: 0x06003DE1 RID: 15841 RVA: 0x0026161F File Offset: 0x0025F81F
		public string DistanceUnits
		{
			get
			{
				return UnitsHelper.GetCaption(UnitsHelper.Units.km);
			}
		}

		// Token: 0x17001444 RID: 5188
		// (get) Token: 0x06003DE2 RID: 15842 RVA: 0x00261627 File Offset: 0x0025F827
		public string FuelUsedUnits
		{
			get
			{
				return UnitsHelper.GetCaption(UnitsHelper.Units.liters);
			}
		}

		// Token: 0x17001445 RID: 5189
		// (get) Token: 0x06003DE3 RID: 15843 RVA: 0x00261630 File Offset: 0x0025F830
		public string FuelConsumptionsUnits
		{
			get
			{
				return UnitsHelper.GetCaption(UnitsHelper.Units.liters100km);
			}
		}

		// Token: 0x17001446 RID: 5190
		// (get) Token: 0x06003DE4 RID: 15844 RVA: 0x00261639 File Offset: 0x0025F839
		public string SpeedUnits
		{
			get
			{
				return UnitsHelper.GetCaption(UnitsHelper.Units.kmh);
			}
		}

		// Token: 0x17001447 RID: 5191
		// (get) Token: 0x06003DE5 RID: 15845 RVA: 0x00261641 File Offset: 0x0025F841
		public string Currency
		{
			get
			{
				return SharedSettings.Current.Currency;
			}
		}

		// Token: 0x06003DE6 RID: 15846 RVA: 0x0032990C File Offset: 0x00327B0C
		public void CalculateTotal()
		{
			List<DriveCycle> driveCycles = this._DriveCycles;
			this.TotalDistance = 0.0;
			this.TotalAvgSpeed = 0.0;
			this.TotalAvgFuelConsumption = 0.0;
			this.TotalFuelUsed = 0.0;
			this.TotalDuration = default(TimeSpan);
			this.TotalFuelPrice = 0m;
			foreach (DriveCycle driveCycle in driveCycles)
			{
				try
				{
					if (!double.IsInfinity(driveCycle.Distance) && !double.IsNaN(driveCycle.Distance))
					{
						this.TotalDistance += driveCycle.Distance;
					}
					if (!double.IsInfinity(driveCycle.FuelUsed) && !double.IsNaN(driveCycle.FuelUsed))
					{
						this.TotalFuelUsed += driveCycle.FuelUsed;
						this.TotalFuelPrice += (decimal)driveCycle.FuelUsed * driveCycle.FuelPricePerL;
					}
					this.TotalDuration += driveCycle.Duration;
				}
				catch (Exception)
				{
				}
			}
			this.TotalFuelPrice = Math.Round(this.TotalFuelPrice, 2);
			double num = this.TotalDistance / this.TotalDuration.TotalHours;
			num = UnitsHelper.GetValue(num, UnitsHelper.Units.kmh);
			this.TotalAvgSpeed = Math.Round(num, 2);
			double num2 = this.TotalFuelUsed / this.TotalDistance;
			this.TotalAvgFuelConsumption = num2 * 100.0;
		}

		// Token: 0x06003DE7 RID: 15847 RVA: 0x00329AC4 File Offset: 0x00327CC4
		private void CalculatePeriod(DateTime startTime, DateTime endTime)
		{
			List<DriveCycle> list = this._DriveCycles.Where((DriveCycle x) => x.TimeStarted.Date >= startTime && x.TimeFinished.Date <= endTime).ToList<DriveCycle>();
			this.PeriodDistance = 0.0;
			this.PeriodAvgSpeed = 0.0;
			this.PeriodAvgFuelConsumption = 0.0;
			this.PeriodFuelUsed = 0.0;
			this.PeriodDuration = default(TimeSpan);
			this.PeriodFuelPrice = 0m;
			double num = 0.0;
			double num2 = 0.0;
			decimal num3 = 0m;
			TimeSpan timeSpan = default(TimeSpan);
			foreach (DriveCycle driveCycle in list)
			{
				try
				{
					if (driveCycle.Distance != 0.0 || driveCycle.FuelUsed != 0.0)
					{
						if (!double.IsInfinity(driveCycle.Distance) && !double.IsNaN(driveCycle.Distance))
						{
							num += driveCycle.Distance;
						}
						if (!double.IsInfinity(driveCycle.FuelUsed) && !double.IsNaN(driveCycle.FuelUsed))
						{
							num2 += driveCycle.FuelUsed;
							num3 += (decimal)driveCycle.FuelUsed * driveCycle.FuelPricePerL;
						}
						timeSpan += driveCycle.Duration;
					}
				}
				catch (Exception)
				{
				}
			}
			DriveCycle driveCycle2 = DriveCycle.Current;
			if (driveCycle2 != null && double.IsFinite(driveCycle2.FuelUsed) && driveCycle2.TimeStarted >= startTime && driveCycle2.TimeStarted <= endTime && App.OBDReader.CurrentStatus == OBDDataReaderStatus.ConnectedToECU && !App.OBDSimulator.IsActive)
			{
				if (double.IsFinite(driveCycle2.Distance))
				{
					num += driveCycle2.Distance;
				}
				num2 += driveCycle2.FuelUsed;
				num3 += (decimal)driveCycle2.FuelUsed * SharedSettings.Current.FuelPriceForLitre;
				TimeSpan timeSpan2 = DateTimeNowHelper.NowSafe - driveCycle2.TimeStarted;
				timeSpan += timeSpan2;
			}
			this.PeriodFuelPrice = Math.Round(num3, 2);
			double num4 = num / timeSpan.TotalHours;
			num4 = UnitsHelper.GetValue(num4, UnitsHelper.Units.kmh);
			this.PeriodAvgSpeed = Math.Round(num4, 2);
			this.PeriodDuration = timeSpan;
			double num5 = num2 / num;
			this.PeriodAvgFuelConsumption = Math.Round(UnitsHelper.GetValue(num5 * 100.0, UnitsHelper.Units.liters100km), 2);
			this.PeriodDistance = Math.Round(UnitsHelper.GetValue(num, UnitsHelper.Units.km), 2);
			this.PeriodFuelUsed = Math.Round(UnitsHelper.GetValue(num2, UnitsHelper.Units.liters), 2);
		}

		// Token: 0x06003DE8 RID: 15848 RVA: 0x00329DBC File Offset: 0x00327FBC
		public void CalculatePeriodForCalibration(DateTime startTime)
		{
			IEnumerable<DriveCycle> enumerable = this.DriveCycles.Where((DriveCycle x) => x.TimeStarted >= startTime);
			this.PeriodDistance = 0.0;
			this.PeriodAvgSpeed = 0.0;
			this.PeriodAvgFuelConsumption = 0.0;
			this.PeriodFuelUsed = 0.0;
			this.PeriodDuration = default(TimeSpan);
			this.PeriodFuelPrice = 0m;
			foreach (DriveCycle driveCycle in enumerable)
			{
				try
				{
					if (!double.IsInfinity(driveCycle.Distance) && !double.IsNaN(driveCycle.Distance))
					{
						this.PeriodDistance += driveCycle.Distance;
					}
					if (!double.IsInfinity(driveCycle.FuelUsed) && !double.IsNaN(driveCycle.FuelUsed))
					{
						this.PeriodFuelUsed += driveCycle.FuelUsed;
						this.PeriodFuelPrice += (decimal)driveCycle.FuelUsed * driveCycle.FuelPricePerL;
					}
					this.PeriodDuration += driveCycle.Duration;
				}
				catch (Exception)
				{
				}
			}
			this.PeriodAvgSpeed = this.PeriodDistance / this.PeriodDuration.TotalHours;
			double num = this.PeriodFuelUsed / this.PeriodDistance;
			this.PeriodAvgFuelConsumption = num * 100.0;
		}

		// Token: 0x06003DE9 RID: 15849 RVA: 0x00329F6C File Offset: 0x0032816C
		private IEnumerable<DayDriveCycle> SeparateByDays(IEnumerable<DriveCycle> dcs)
		{
			List<DayDriveCycle> list = new List<DayDriveCycle>();
			foreach (DriveCycle driveCycle in dcs)
			{
				if (driveCycle.FuelUsed > 0.0 && !double.IsNaN(driveCycle.FuelUsed) && !double.IsInfinity(driveCycle.FuelUsed))
				{
					DayDriveCycle dayDriveCycle = null;
					try
					{
						DateTime dateTime = driveCycle.TimeStarted.Date;
						if (driveCycle.TimeStarted.Date != driveCycle.TimeFinished.Date)
						{
							TimeSpan timeSpan = new TimeSpan(driveCycle.TimeFinished.Date.Ticks);
							long num = Math.Abs(timeSpan.Ticks - driveCycle.TimeStarted.Ticks);
							if (Math.Abs(driveCycle.TimeFinished.Ticks - timeSpan.Ticks) > num)
							{
								dateTime = driveCycle.TimeFinished.Date;
							}
						}
						int num2 = list.Count - 1;
						if (num2 >= 0)
						{
							DayDriveCycle dayDriveCycle2 = list[num2];
							if (dayDriveCycle2.TimeStarted == dateTime)
							{
								dayDriveCycle = dayDriveCycle2;
							}
						}
						if (dayDriveCycle == null)
						{
							dayDriveCycle = new DayDriveCycle();
							dayDriveCycle.TimeStarted = dateTime;
							dayDriveCycle.TimeFinished = dateTime;
							list.Add(dayDriveCycle);
						}
						dayDriveCycle.Distance += driveCycle.Distance;
						dayDriveCycle.FuelUsed += driveCycle.FuelUsed;
						dayDriveCycle.Duration += driveCycle.Duration;
						dayDriveCycle.FuelPricePerL += driveCycle.FuelPricePerL;
						dayDriveCycle.counter++;
						dayDriveCycle.TotalFuelPrice += driveCycle.TotalFuelPrice;
					}
					catch (Exception)
					{
					}
				}
			}
			foreach (DayDriveCycle dayDriveCycle3 in list)
			{
				dayDriveCycle3.FuelPricePerL /= dayDriveCycle3.counter;
				dayDriveCycle3.Distance = UnitsHelper.GetValue(dayDriveCycle3.Distance, UnitsHelper.Units.km);
			}
			return list.Where((DayDriveCycle x) => x.TimeStarted >= this.PeriodStartDate && x.TimeFinished <= this.PeriodEndDate);
		}

		// Token: 0x06003DEA RID: 15850 RVA: 0x0032A20C File Offset: 0x0032840C
		public List<CombinedDriveCycle> GetCombinedDriveCycles()
		{
			if (SharedSettings.Current.MergeDriveCyclesTime == 0)
			{
				return this.DriveCycles.Select((DriveCycle x) => new CombinedDriveCycle(x)).ToList<CombinedDriveCycle>();
			}
			List<CombinedDriveCycle> list = new List<CombinedDriveCycle>();
			for (int i = 0; i < this.DriveCycles.Count; i++)
			{
				DriveCycle driveCycle = this.DriveCycles[i];
				if (i == 0)
				{
					list.Add(new CombinedDriveCycle(driveCycle));
				}
				else
				{
					CombinedDriveCycle combinedDriveCycle = list[list.Count - 1];
					TimeSpan timeSpan = new TimeSpan(driveCycle.TimeStarted.Ticks - combinedDriveCycle.TimeFinished.Ticks);
					if (combinedDriveCycle.FuelPricePerL == driveCycle.FuelPricePerL && timeSpan.TotalSeconds <= (double)SharedSettings.Current.MergeDriveCyclesTime)
					{
						combinedDriveCycle.AddCycle(driveCycle);
					}
					else
					{
						list.Add(new CombinedDriveCycle(driveCycle));
					}
				}
			}
			return list;
		}

		// Token: 0x06003DEB RID: 15851 RVA: 0x0032A308 File Offset: 0x00328508
		[CompilerGenerated]
		private bool <RefreshPeriodForStatisticsScreen>b__8_0(DriveCycle dc)
		{
			return this.PeriodStartDate <= dc.TimeStarted.Date && dc.TimeStarted.Date <= this.PeriodEndDate;
		}

		// Token: 0x06003DEC RID: 15852 RVA: 0x0032A34B File Offset: 0x0032854B
		[CompilerGenerated]
		private void <get_PeriodAllTime>b__22_0()
		{
			this.PeriodEndDate = DateTime.MaxValue;
			this.PeriodStartDate = new DateTime(2001, 1, 1, 0, 0, 1);
			this.RefreshPeriodForStatisticsScreen();
		}

		// Token: 0x06003DED RID: 15853 RVA: 0x0032A374 File Offset: 0x00328574
		[CompilerGenerated]
		private async void <get_PeriodToday>b__24_0()
		{
			if (!SharedSettings.Current.AdsProductPurchased)
			{
				FuelStatisticsPage page = FuelStatisticsPage.Instance;
				if (page != null)
				{
					TaskAwaiter<bool> taskAwaiter = page.DisplayAlert("Car Scanner Pro", Translate.GetString("FeatureLocked"), Translate.GetString("ios_Yes"), Translate.GetString("ios_No")).GetAwaiter();
					if (!taskAwaiter.IsCompleted)
					{
						await taskAwaiter;
						TaskAwaiter<bool> taskAwaiter2;
						taskAwaiter = taskAwaiter2;
						taskAwaiter2 = default(TaskAwaiter<bool>);
					}
					if (taskAwaiter.GetResult())
					{
						await page.Navigation.PushAsync(InAppManager.GetInAppPage());
					}
				}
			}
			else
			{
				this.PeriodEndDate = DateTimeNowHelper.NowSafe;
				this.PeriodStartDate = DateTimeNowHelper.NowSafe.Date;
				this.RefreshPeriodForStatisticsScreen();
			}
		}

		// Token: 0x06003DEE RID: 15854 RVA: 0x0032A3AC File Offset: 0x003285AC
		[CompilerGenerated]
		private async void <get_Period7Days>b__26_0()
		{
			if (!SharedSettings.Current.AdsProductPurchased)
			{
				FuelStatisticsPage page = FuelStatisticsPage.Instance;
				if (page != null)
				{
					TaskAwaiter<bool> taskAwaiter = page.DisplayAlert("Car Scanner Pro", Translate.GetString("FeatureLocked"), Translate.GetString("ios_Yes"), Translate.GetString("ios_No")).GetAwaiter();
					if (!taskAwaiter.IsCompleted)
					{
						await taskAwaiter;
						TaskAwaiter<bool> taskAwaiter2;
						taskAwaiter = taskAwaiter2;
						taskAwaiter2 = default(TaskAwaiter<bool>);
					}
					if (taskAwaiter.GetResult())
					{
						await page.Navigation.PushAsync(InAppManager.GetInAppPage());
					}
				}
			}
			else
			{
				TimeSpan timeSpan = new TimeSpan(DateTimeNowHelper.NowSafe.Date.Ticks);
				TimeSpan timeSpan2 = timeSpan - TimeSpan.FromDays(7.0);
				this.PeriodEndDate = new DateTime(timeSpan.Ticks);
				this.PeriodStartDate = new DateTime(timeSpan2.Ticks);
				this.RefreshPeriodForStatisticsScreen();
			}
		}

		// Token: 0x06003DEF RID: 15855 RVA: 0x0032A3E4 File Offset: 0x003285E4
		[CompilerGenerated]
		private async void <get_Period14Days>b__28_0()
		{
			if (!SharedSettings.Current.AdsProductPurchased)
			{
				FuelStatisticsPage page = FuelStatisticsPage.Instance;
				if (page != null)
				{
					TaskAwaiter<bool> taskAwaiter = page.DisplayAlert("Car Scanner Pro", Translate.GetString("FeatureLocked"), Translate.GetString("ios_Yes"), Translate.GetString("ios_No")).GetAwaiter();
					if (!taskAwaiter.IsCompleted)
					{
						await taskAwaiter;
						TaskAwaiter<bool> taskAwaiter2;
						taskAwaiter = taskAwaiter2;
						taskAwaiter2 = default(TaskAwaiter<bool>);
					}
					if (taskAwaiter.GetResult())
					{
						await page.Navigation.PushAsync(InAppManager.GetInAppPage());
					}
				}
			}
			else
			{
				TimeSpan timeSpan = new TimeSpan(DateTimeNowHelper.NowSafe.Date.Ticks);
				TimeSpan timeSpan2 = timeSpan - TimeSpan.FromDays(14.0);
				this.PeriodEndDate = new DateTime(timeSpan.Ticks);
				this.PeriodStartDate = new DateTime(timeSpan2.Ticks);
				this.RefreshPeriodForStatisticsScreen();
			}
		}

		// Token: 0x06003DF0 RID: 15856 RVA: 0x0032A41C File Offset: 0x0032861C
		[CompilerGenerated]
		private async void <get_Period30Days>b__30_0()
		{
			if (!SharedSettings.Current.AdsProductPurchased)
			{
				FuelStatisticsPage page = FuelStatisticsPage.Instance;
				if (page != null)
				{
					TaskAwaiter<bool> taskAwaiter = page.DisplayAlert("Car Scanner Pro", Translate.GetString("FeatureLocked"), Translate.GetString("ios_Yes"), Translate.GetString("ios_No")).GetAwaiter();
					if (!taskAwaiter.IsCompleted)
					{
						await taskAwaiter;
						TaskAwaiter<bool> taskAwaiter2;
						taskAwaiter = taskAwaiter2;
						taskAwaiter2 = default(TaskAwaiter<bool>);
					}
					if (taskAwaiter.GetResult())
					{
						await page.Navigation.PushAsync(InAppManager.GetInAppPage());
					}
				}
			}
			else
			{
				TimeSpan timeSpan = new TimeSpan(DateTimeNowHelper.NowSafe.Date.Ticks);
				TimeSpan timeSpan2 = timeSpan - TimeSpan.FromDays(30.0);
				this.PeriodEndDate = new DateTime(timeSpan.Ticks);
				this.PeriodStartDate = new DateTime(timeSpan2.Ticks);
				this.RefreshPeriodForStatisticsScreen();
			}
		}

		// Token: 0x06003DF1 RID: 15857 RVA: 0x0032A454 File Offset: 0x00328654
		[CompilerGenerated]
		private async void <get_Period90Days>b__32_0()
		{
			if (!SharedSettings.Current.AdsProductPurchased)
			{
				FuelStatisticsPage page = FuelStatisticsPage.Instance;
				if (page != null)
				{
					TaskAwaiter<bool> taskAwaiter = page.DisplayAlert("Car Scanner Pro", Translate.GetString("FeatureLocked"), Translate.GetString("ios_Yes"), Translate.GetString("ios_No")).GetAwaiter();
					if (!taskAwaiter.IsCompleted)
					{
						await taskAwaiter;
						TaskAwaiter<bool> taskAwaiter2;
						taskAwaiter = taskAwaiter2;
						taskAwaiter2 = default(TaskAwaiter<bool>);
					}
					if (taskAwaiter.GetResult())
					{
						await page.Navigation.PushAsync(InAppManager.GetInAppPage());
					}
				}
			}
			else
			{
				TimeSpan timeSpan = new TimeSpan(DateTimeNowHelper.NowSafe.Date.Ticks);
				TimeSpan timeSpan2 = timeSpan - TimeSpan.FromDays(90.0);
				this.PeriodEndDate = new DateTime(timeSpan.Ticks);
				this.PeriodStartDate = new DateTime(timeSpan2.Ticks);
				this.RefreshPeriodForStatisticsScreen();
			}
		}

		// Token: 0x06003DF2 RID: 15858 RVA: 0x0032A48C File Offset: 0x0032868C
		[CompilerGenerated]
		private async void <get_Period180Days>b__34_0()
		{
			if (!SharedSettings.Current.AdsProductPurchased)
			{
				FuelStatisticsPage page = FuelStatisticsPage.Instance;
				if (page != null)
				{
					TaskAwaiter<bool> taskAwaiter = page.DisplayAlert("Car Scanner Pro", Translate.GetString("FeatureLocked"), Translate.GetString("ios_Yes"), Translate.GetString("ios_No")).GetAwaiter();
					if (!taskAwaiter.IsCompleted)
					{
						await taskAwaiter;
						TaskAwaiter<bool> taskAwaiter2;
						taskAwaiter = taskAwaiter2;
						taskAwaiter2 = default(TaskAwaiter<bool>);
					}
					if (taskAwaiter.GetResult())
					{
						await page.Navigation.PushAsync(InAppManager.GetInAppPage());
					}
				}
			}
			else
			{
				TimeSpan timeSpan = new TimeSpan(DateTimeNowHelper.NowSafe.Date.Ticks);
				TimeSpan timeSpan2 = timeSpan - TimeSpan.FromDays(180.0);
				this.PeriodEndDate = new DateTime(timeSpan.Ticks);
				this.PeriodStartDate = new DateTime(timeSpan2.Ticks);
				this.RefreshPeriodForStatisticsScreen();
			}
		}

		// Token: 0x06003DF3 RID: 15859 RVA: 0x0032A4C4 File Offset: 0x003286C4
		[CompilerGenerated]
		private async void <get_Period360Days>b__36_0()
		{
			if (!SharedSettings.Current.AdsProductPurchased)
			{
				FuelStatisticsPage page = FuelStatisticsPage.Instance;
				if (page != null)
				{
					TaskAwaiter<bool> taskAwaiter = page.DisplayAlert("Car Scanner Pro", Translate.GetString("FeatureLocked"), Translate.GetString("ios_Yes"), Translate.GetString("ios_No")).GetAwaiter();
					if (!taskAwaiter.IsCompleted)
					{
						await taskAwaiter;
						TaskAwaiter<bool> taskAwaiter2;
						taskAwaiter = taskAwaiter2;
						taskAwaiter2 = default(TaskAwaiter<bool>);
					}
					if (taskAwaiter.GetResult())
					{
						await page.Navigation.PushAsync(InAppManager.GetInAppPage());
					}
				}
			}
			else
			{
				TimeSpan timeSpan = new TimeSpan(DateTimeNowHelper.NowSafe.Date.Ticks);
				TimeSpan timeSpan2 = timeSpan - TimeSpan.FromDays(360.0);
				this.PeriodEndDate = new DateTime(timeSpan.Ticks);
				this.PeriodStartDate = new DateTime(timeSpan2.Ticks);
				this.RefreshPeriodForStatisticsScreen();
			}
		}

		// Token: 0x06003DF4 RID: 15860 RVA: 0x0032A4FB File Offset: 0x003286FB
		[CompilerGenerated]
		private bool <SeparateByDays>b__114_0(DayDriveCycle x)
		{
			return x.TimeStarted >= this.PeriodStartDate && x.TimeFinished <= this.PeriodEndDate;
		}

		// Token: 0x040025CB RID: 9675
		private static DriveCycleViewModel _Current;

		// Token: 0x040025CC RID: 9676
		private List<DriveCycle> _DriveCycles = new List<DriveCycle>();

		// Token: 0x040025CD RID: 9677
		private SmartCollection<DayDriveCycle> _DayCycles = new SmartCollection<DayDriveCycle>();

		// Token: 0x040025CE RID: 9678
		[CompilerGenerated]
		private DateTime <PeriodStartDate>k__BackingField;

		// Token: 0x040025CF RID: 9679
		[CompilerGenerated]
		private DateTime <PeriodEndDate>k__BackingField;

		// Token: 0x040025D0 RID: 9680
		[CompilerGenerated]
		private PropertyChangedEventHandler PropertyChanged;

		// Token: 0x040025D1 RID: 9681
		private double _PeriodFuelUsed;

		// Token: 0x040025D2 RID: 9682
		private decimal _PeriodFuelPrice;

		// Token: 0x040025D3 RID: 9683
		private double _PeriodDistance;

		// Token: 0x040025D4 RID: 9684
		private double _PeriodAvgFuelConsumption;

		// Token: 0x040025D5 RID: 9685
		private double _PeriodAvgSpeed;

		// Token: 0x040025D6 RID: 9686
		private TimeSpan _PeriodDuration;

		// Token: 0x040025D7 RID: 9687
		private double _TotalFuelUsed;

		// Token: 0x040025D8 RID: 9688
		private decimal _TotalFuelPrice;

		// Token: 0x040025D9 RID: 9689
		private double _TotalDistance;

		// Token: 0x040025DA RID: 9690
		private double _TotalAvgFuelConsumption;

		// Token: 0x040025DB RID: 9691
		private double _TotalAvgSpeed;

		// Token: 0x040025DC RID: 9692
		private TimeSpan _TotalDuration;

		// Token: 0x02000719 RID: 1817
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <<get_Period14Days>b__28_0>d : IAsyncStateMachine
		{
			// Token: 0x06003DF5 RID: 15861 RVA: 0x0032A524 File Offset: 0x00328724
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				DriveCycleViewModel driveCycleViewModel = this;
				try
				{
					TaskAwaiter taskAwaiter3;
					TaskAwaiter<bool> taskAwaiter5;
					if (num != 0)
					{
						if (num == 1)
						{
							TaskAwaiter taskAwaiter4;
							taskAwaiter3 = taskAwaiter4;
							taskAwaiter4 = default(TaskAwaiter);
							num2 = -1;
							goto IL_012E;
						}
						if (SharedSettings.Current.AdsProductPurchased)
						{
							TimeSpan timeSpan = new TimeSpan(DateTimeNowHelper.NowSafe.Date.Ticks);
							TimeSpan timeSpan2 = timeSpan - TimeSpan.FromDays(14.0);
							driveCycleViewModel.PeriodEndDate = new DateTime(timeSpan.Ticks);
							driveCycleViewModel.PeriodStartDate = new DateTime(timeSpan2.Ticks);
							driveCycleViewModel.RefreshPeriodForStatisticsScreen();
							goto IL_01AF;
						}
						page = FuelStatisticsPage.Instance;
						if (page == null)
						{
							goto IL_0135;
						}
						taskAwaiter5 = page.DisplayAlert("Car Scanner Pro", Translate.GetString("FeatureLocked"), Translate.GetString("ios_Yes"), Translate.GetString("ios_No")).GetAwaiter();
						if (!taskAwaiter5.IsCompleted)
						{
							num2 = 0;
							taskAwaiter2 = taskAwaiter5;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<bool>, DriveCycleViewModel.<<get_Period14Days>b__28_0>d>(ref taskAwaiter5, ref this);
							return;
						}
					}
					else
					{
						taskAwaiter5 = taskAwaiter2;
						taskAwaiter2 = default(TaskAwaiter<bool>);
						num2 = -1;
					}
					if (!taskAwaiter5.GetResult())
					{
						goto IL_0135;
					}
					taskAwaiter3 = page.Navigation.PushAsync(InAppManager.GetInAppPage()).GetAwaiter();
					if (!taskAwaiter3.IsCompleted)
					{
						num2 = 1;
						TaskAwaiter taskAwaiter4 = taskAwaiter3;
						this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, DriveCycleViewModel.<<get_Period14Days>b__28_0>d>(ref taskAwaiter3, ref this);
						return;
					}
					IL_012E:
					taskAwaiter3.GetResult();
					IL_0135:;
				}
				catch (Exception ex)
				{
					num2 = -2;
					this.<>t__builder.SetException(ex);
					return;
				}
				IL_01AF:
				num2 = -2;
				this.<>t__builder.SetResult();
			}

			// Token: 0x06003DF6 RID: 15862 RVA: 0x0032A710 File Offset: 0x00328910
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x040025DD RID: 9693
			public int <>1__state;

			// Token: 0x040025DE RID: 9694
			public AsyncVoidMethodBuilder <>t__builder;

			// Token: 0x040025DF RID: 9695
			public DriveCycleViewModel <>4__this;

			// Token: 0x040025E0 RID: 9696
			private FuelStatisticsPage <page>5__2;

			// Token: 0x040025E1 RID: 9697
			private TaskAwaiter<bool> <>u__1;

			// Token: 0x040025E2 RID: 9698
			private TaskAwaiter <>u__2;
		}

		// Token: 0x0200071A RID: 1818
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <<get_Period180Days>b__34_0>d : IAsyncStateMachine
		{
			// Token: 0x06003DF7 RID: 15863 RVA: 0x0032A720 File Offset: 0x00328920
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				DriveCycleViewModel driveCycleViewModel = this;
				try
				{
					TaskAwaiter taskAwaiter3;
					TaskAwaiter<bool> taskAwaiter5;
					if (num != 0)
					{
						if (num == 1)
						{
							TaskAwaiter taskAwaiter4;
							taskAwaiter3 = taskAwaiter4;
							taskAwaiter4 = default(TaskAwaiter);
							num2 = -1;
							goto IL_012E;
						}
						if (SharedSettings.Current.AdsProductPurchased)
						{
							TimeSpan timeSpan = new TimeSpan(DateTimeNowHelper.NowSafe.Date.Ticks);
							TimeSpan timeSpan2 = timeSpan - TimeSpan.FromDays(180.0);
							driveCycleViewModel.PeriodEndDate = new DateTime(timeSpan.Ticks);
							driveCycleViewModel.PeriodStartDate = new DateTime(timeSpan2.Ticks);
							driveCycleViewModel.RefreshPeriodForStatisticsScreen();
							goto IL_01AF;
						}
						page = FuelStatisticsPage.Instance;
						if (page == null)
						{
							goto IL_0135;
						}
						taskAwaiter5 = page.DisplayAlert("Car Scanner Pro", Translate.GetString("FeatureLocked"), Translate.GetString("ios_Yes"), Translate.GetString("ios_No")).GetAwaiter();
						if (!taskAwaiter5.IsCompleted)
						{
							num2 = 0;
							taskAwaiter2 = taskAwaiter5;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<bool>, DriveCycleViewModel.<<get_Period180Days>b__34_0>d>(ref taskAwaiter5, ref this);
							return;
						}
					}
					else
					{
						taskAwaiter5 = taskAwaiter2;
						taskAwaiter2 = default(TaskAwaiter<bool>);
						num2 = -1;
					}
					if (!taskAwaiter5.GetResult())
					{
						goto IL_0135;
					}
					taskAwaiter3 = page.Navigation.PushAsync(InAppManager.GetInAppPage()).GetAwaiter();
					if (!taskAwaiter3.IsCompleted)
					{
						num2 = 1;
						TaskAwaiter taskAwaiter4 = taskAwaiter3;
						this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, DriveCycleViewModel.<<get_Period180Days>b__34_0>d>(ref taskAwaiter3, ref this);
						return;
					}
					IL_012E:
					taskAwaiter3.GetResult();
					IL_0135:;
				}
				catch (Exception ex)
				{
					num2 = -2;
					this.<>t__builder.SetException(ex);
					return;
				}
				IL_01AF:
				num2 = -2;
				this.<>t__builder.SetResult();
			}

			// Token: 0x06003DF8 RID: 15864 RVA: 0x0032A90C File Offset: 0x00328B0C
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x040025E3 RID: 9699
			public int <>1__state;

			// Token: 0x040025E4 RID: 9700
			public AsyncVoidMethodBuilder <>t__builder;

			// Token: 0x040025E5 RID: 9701
			public DriveCycleViewModel <>4__this;

			// Token: 0x040025E6 RID: 9702
			private FuelStatisticsPage <page>5__2;

			// Token: 0x040025E7 RID: 9703
			private TaskAwaiter<bool> <>u__1;

			// Token: 0x040025E8 RID: 9704
			private TaskAwaiter <>u__2;
		}

		// Token: 0x0200071B RID: 1819
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <<get_Period30Days>b__30_0>d : IAsyncStateMachine
		{
			// Token: 0x06003DF9 RID: 15865 RVA: 0x0032A91C File Offset: 0x00328B1C
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				DriveCycleViewModel driveCycleViewModel = this;
				try
				{
					TaskAwaiter taskAwaiter3;
					TaskAwaiter<bool> taskAwaiter5;
					if (num != 0)
					{
						if (num == 1)
						{
							TaskAwaiter taskAwaiter4;
							taskAwaiter3 = taskAwaiter4;
							taskAwaiter4 = default(TaskAwaiter);
							num2 = -1;
							goto IL_012E;
						}
						if (SharedSettings.Current.AdsProductPurchased)
						{
							TimeSpan timeSpan = new TimeSpan(DateTimeNowHelper.NowSafe.Date.Ticks);
							TimeSpan timeSpan2 = timeSpan - TimeSpan.FromDays(30.0);
							driveCycleViewModel.PeriodEndDate = new DateTime(timeSpan.Ticks);
							driveCycleViewModel.PeriodStartDate = new DateTime(timeSpan2.Ticks);
							driveCycleViewModel.RefreshPeriodForStatisticsScreen();
							goto IL_01AF;
						}
						page = FuelStatisticsPage.Instance;
						if (page == null)
						{
							goto IL_0135;
						}
						taskAwaiter5 = page.DisplayAlert("Car Scanner Pro", Translate.GetString("FeatureLocked"), Translate.GetString("ios_Yes"), Translate.GetString("ios_No")).GetAwaiter();
						if (!taskAwaiter5.IsCompleted)
						{
							num2 = 0;
							taskAwaiter2 = taskAwaiter5;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<bool>, DriveCycleViewModel.<<get_Period30Days>b__30_0>d>(ref taskAwaiter5, ref this);
							return;
						}
					}
					else
					{
						taskAwaiter5 = taskAwaiter2;
						taskAwaiter2 = default(TaskAwaiter<bool>);
						num2 = -1;
					}
					if (!taskAwaiter5.GetResult())
					{
						goto IL_0135;
					}
					taskAwaiter3 = page.Navigation.PushAsync(InAppManager.GetInAppPage()).GetAwaiter();
					if (!taskAwaiter3.IsCompleted)
					{
						num2 = 1;
						TaskAwaiter taskAwaiter4 = taskAwaiter3;
						this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, DriveCycleViewModel.<<get_Period30Days>b__30_0>d>(ref taskAwaiter3, ref this);
						return;
					}
					IL_012E:
					taskAwaiter3.GetResult();
					IL_0135:;
				}
				catch (Exception ex)
				{
					num2 = -2;
					this.<>t__builder.SetException(ex);
					return;
				}
				IL_01AF:
				num2 = -2;
				this.<>t__builder.SetResult();
			}

			// Token: 0x06003DFA RID: 15866 RVA: 0x0032AB08 File Offset: 0x00328D08
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x040025E9 RID: 9705
			public int <>1__state;

			// Token: 0x040025EA RID: 9706
			public AsyncVoidMethodBuilder <>t__builder;

			// Token: 0x040025EB RID: 9707
			public DriveCycleViewModel <>4__this;

			// Token: 0x040025EC RID: 9708
			private FuelStatisticsPage <page>5__2;

			// Token: 0x040025ED RID: 9709
			private TaskAwaiter<bool> <>u__1;

			// Token: 0x040025EE RID: 9710
			private TaskAwaiter <>u__2;
		}

		// Token: 0x0200071C RID: 1820
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <<get_Period360Days>b__36_0>d : IAsyncStateMachine
		{
			// Token: 0x06003DFB RID: 15867 RVA: 0x0032AB18 File Offset: 0x00328D18
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				DriveCycleViewModel driveCycleViewModel = this;
				try
				{
					TaskAwaiter taskAwaiter3;
					TaskAwaiter<bool> taskAwaiter5;
					if (num != 0)
					{
						if (num == 1)
						{
							TaskAwaiter taskAwaiter4;
							taskAwaiter3 = taskAwaiter4;
							taskAwaiter4 = default(TaskAwaiter);
							num2 = -1;
							goto IL_012E;
						}
						if (SharedSettings.Current.AdsProductPurchased)
						{
							TimeSpan timeSpan = new TimeSpan(DateTimeNowHelper.NowSafe.Date.Ticks);
							TimeSpan timeSpan2 = timeSpan - TimeSpan.FromDays(360.0);
							driveCycleViewModel.PeriodEndDate = new DateTime(timeSpan.Ticks);
							driveCycleViewModel.PeriodStartDate = new DateTime(timeSpan2.Ticks);
							driveCycleViewModel.RefreshPeriodForStatisticsScreen();
							goto IL_01AF;
						}
						page = FuelStatisticsPage.Instance;
						if (page == null)
						{
							goto IL_0135;
						}
						taskAwaiter5 = page.DisplayAlert("Car Scanner Pro", Translate.GetString("FeatureLocked"), Translate.GetString("ios_Yes"), Translate.GetString("ios_No")).GetAwaiter();
						if (!taskAwaiter5.IsCompleted)
						{
							num2 = 0;
							taskAwaiter2 = taskAwaiter5;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<bool>, DriveCycleViewModel.<<get_Period360Days>b__36_0>d>(ref taskAwaiter5, ref this);
							return;
						}
					}
					else
					{
						taskAwaiter5 = taskAwaiter2;
						taskAwaiter2 = default(TaskAwaiter<bool>);
						num2 = -1;
					}
					if (!taskAwaiter5.GetResult())
					{
						goto IL_0135;
					}
					taskAwaiter3 = page.Navigation.PushAsync(InAppManager.GetInAppPage()).GetAwaiter();
					if (!taskAwaiter3.IsCompleted)
					{
						num2 = 1;
						TaskAwaiter taskAwaiter4 = taskAwaiter3;
						this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, DriveCycleViewModel.<<get_Period360Days>b__36_0>d>(ref taskAwaiter3, ref this);
						return;
					}
					IL_012E:
					taskAwaiter3.GetResult();
					IL_0135:;
				}
				catch (Exception ex)
				{
					num2 = -2;
					this.<>t__builder.SetException(ex);
					return;
				}
				IL_01AF:
				num2 = -2;
				this.<>t__builder.SetResult();
			}

			// Token: 0x06003DFC RID: 15868 RVA: 0x0032AD04 File Offset: 0x00328F04
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x040025EF RID: 9711
			public int <>1__state;

			// Token: 0x040025F0 RID: 9712
			public AsyncVoidMethodBuilder <>t__builder;

			// Token: 0x040025F1 RID: 9713
			public DriveCycleViewModel <>4__this;

			// Token: 0x040025F2 RID: 9714
			private FuelStatisticsPage <page>5__2;

			// Token: 0x040025F3 RID: 9715
			private TaskAwaiter<bool> <>u__1;

			// Token: 0x040025F4 RID: 9716
			private TaskAwaiter <>u__2;
		}

		// Token: 0x0200071D RID: 1821
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <<get_Period7Days>b__26_0>d : IAsyncStateMachine
		{
			// Token: 0x06003DFD RID: 15869 RVA: 0x0032AD14 File Offset: 0x00328F14
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				DriveCycleViewModel driveCycleViewModel = this;
				try
				{
					TaskAwaiter taskAwaiter3;
					TaskAwaiter<bool> taskAwaiter5;
					if (num != 0)
					{
						if (num == 1)
						{
							TaskAwaiter taskAwaiter4;
							taskAwaiter3 = taskAwaiter4;
							taskAwaiter4 = default(TaskAwaiter);
							num2 = -1;
							goto IL_012E;
						}
						if (SharedSettings.Current.AdsProductPurchased)
						{
							TimeSpan timeSpan = new TimeSpan(DateTimeNowHelper.NowSafe.Date.Ticks);
							TimeSpan timeSpan2 = timeSpan - TimeSpan.FromDays(7.0);
							driveCycleViewModel.PeriodEndDate = new DateTime(timeSpan.Ticks);
							driveCycleViewModel.PeriodStartDate = new DateTime(timeSpan2.Ticks);
							driveCycleViewModel.RefreshPeriodForStatisticsScreen();
							goto IL_01AF;
						}
						page = FuelStatisticsPage.Instance;
						if (page == null)
						{
							goto IL_0135;
						}
						taskAwaiter5 = page.DisplayAlert("Car Scanner Pro", Translate.GetString("FeatureLocked"), Translate.GetString("ios_Yes"), Translate.GetString("ios_No")).GetAwaiter();
						if (!taskAwaiter5.IsCompleted)
						{
							num2 = 0;
							taskAwaiter2 = taskAwaiter5;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<bool>, DriveCycleViewModel.<<get_Period7Days>b__26_0>d>(ref taskAwaiter5, ref this);
							return;
						}
					}
					else
					{
						taskAwaiter5 = taskAwaiter2;
						taskAwaiter2 = default(TaskAwaiter<bool>);
						num2 = -1;
					}
					if (!taskAwaiter5.GetResult())
					{
						goto IL_0135;
					}
					taskAwaiter3 = page.Navigation.PushAsync(InAppManager.GetInAppPage()).GetAwaiter();
					if (!taskAwaiter3.IsCompleted)
					{
						num2 = 1;
						TaskAwaiter taskAwaiter4 = taskAwaiter3;
						this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, DriveCycleViewModel.<<get_Period7Days>b__26_0>d>(ref taskAwaiter3, ref this);
						return;
					}
					IL_012E:
					taskAwaiter3.GetResult();
					IL_0135:;
				}
				catch (Exception ex)
				{
					num2 = -2;
					this.<>t__builder.SetException(ex);
					return;
				}
				IL_01AF:
				num2 = -2;
				this.<>t__builder.SetResult();
			}

			// Token: 0x06003DFE RID: 15870 RVA: 0x0032AF00 File Offset: 0x00329100
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x040025F5 RID: 9717
			public int <>1__state;

			// Token: 0x040025F6 RID: 9718
			public AsyncVoidMethodBuilder <>t__builder;

			// Token: 0x040025F7 RID: 9719
			public DriveCycleViewModel <>4__this;

			// Token: 0x040025F8 RID: 9720
			private FuelStatisticsPage <page>5__2;

			// Token: 0x040025F9 RID: 9721
			private TaskAwaiter<bool> <>u__1;

			// Token: 0x040025FA RID: 9722
			private TaskAwaiter <>u__2;
		}

		// Token: 0x0200071E RID: 1822
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <<get_Period90Days>b__32_0>d : IAsyncStateMachine
		{
			// Token: 0x06003DFF RID: 15871 RVA: 0x0032AF10 File Offset: 0x00329110
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				DriveCycleViewModel driveCycleViewModel = this;
				try
				{
					TaskAwaiter taskAwaiter3;
					TaskAwaiter<bool> taskAwaiter5;
					if (num != 0)
					{
						if (num == 1)
						{
							TaskAwaiter taskAwaiter4;
							taskAwaiter3 = taskAwaiter4;
							taskAwaiter4 = default(TaskAwaiter);
							num2 = -1;
							goto IL_012E;
						}
						if (SharedSettings.Current.AdsProductPurchased)
						{
							TimeSpan timeSpan = new TimeSpan(DateTimeNowHelper.NowSafe.Date.Ticks);
							TimeSpan timeSpan2 = timeSpan - TimeSpan.FromDays(90.0);
							driveCycleViewModel.PeriodEndDate = new DateTime(timeSpan.Ticks);
							driveCycleViewModel.PeriodStartDate = new DateTime(timeSpan2.Ticks);
							driveCycleViewModel.RefreshPeriodForStatisticsScreen();
							goto IL_01AF;
						}
						page = FuelStatisticsPage.Instance;
						if (page == null)
						{
							goto IL_0135;
						}
						taskAwaiter5 = page.DisplayAlert("Car Scanner Pro", Translate.GetString("FeatureLocked"), Translate.GetString("ios_Yes"), Translate.GetString("ios_No")).GetAwaiter();
						if (!taskAwaiter5.IsCompleted)
						{
							num2 = 0;
							taskAwaiter2 = taskAwaiter5;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<bool>, DriveCycleViewModel.<<get_Period90Days>b__32_0>d>(ref taskAwaiter5, ref this);
							return;
						}
					}
					else
					{
						taskAwaiter5 = taskAwaiter2;
						taskAwaiter2 = default(TaskAwaiter<bool>);
						num2 = -1;
					}
					if (!taskAwaiter5.GetResult())
					{
						goto IL_0135;
					}
					taskAwaiter3 = page.Navigation.PushAsync(InAppManager.GetInAppPage()).GetAwaiter();
					if (!taskAwaiter3.IsCompleted)
					{
						num2 = 1;
						TaskAwaiter taskAwaiter4 = taskAwaiter3;
						this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, DriveCycleViewModel.<<get_Period90Days>b__32_0>d>(ref taskAwaiter3, ref this);
						return;
					}
					IL_012E:
					taskAwaiter3.GetResult();
					IL_0135:;
				}
				catch (Exception ex)
				{
					num2 = -2;
					this.<>t__builder.SetException(ex);
					return;
				}
				IL_01AF:
				num2 = -2;
				this.<>t__builder.SetResult();
			}

			// Token: 0x06003E00 RID: 15872 RVA: 0x0032B0FC File Offset: 0x003292FC
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x040025FB RID: 9723
			public int <>1__state;

			// Token: 0x040025FC RID: 9724
			public AsyncVoidMethodBuilder <>t__builder;

			// Token: 0x040025FD RID: 9725
			public DriveCycleViewModel <>4__this;

			// Token: 0x040025FE RID: 9726
			private FuelStatisticsPage <page>5__2;

			// Token: 0x040025FF RID: 9727
			private TaskAwaiter<bool> <>u__1;

			// Token: 0x04002600 RID: 9728
			private TaskAwaiter <>u__2;
		}

		// Token: 0x0200071F RID: 1823
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <<get_PeriodToday>b__24_0>d : IAsyncStateMachine
		{
			// Token: 0x06003E01 RID: 15873 RVA: 0x0032B10C File Offset: 0x0032930C
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				DriveCycleViewModel driveCycleViewModel = this;
				try
				{
					TaskAwaiter taskAwaiter3;
					TaskAwaiter<bool> taskAwaiter5;
					if (num != 0)
					{
						if (num == 1)
						{
							TaskAwaiter taskAwaiter4;
							taskAwaiter3 = taskAwaiter4;
							taskAwaiter4 = default(TaskAwaiter);
							num2 = -1;
							goto IL_0125;
						}
						if (SharedSettings.Current.AdsProductPurchased)
						{
							driveCycleViewModel.PeriodEndDate = DateTimeNowHelper.NowSafe;
							driveCycleViewModel.PeriodStartDate = DateTimeNowHelper.NowSafe.Date;
							driveCycleViewModel.RefreshPeriodForStatisticsScreen();
							goto IL_016E;
						}
						page = FuelStatisticsPage.Instance;
						if (page == null)
						{
							goto IL_012C;
						}
						taskAwaiter5 = page.DisplayAlert("Car Scanner Pro", Translate.GetString("FeatureLocked"), Translate.GetString("ios_Yes"), Translate.GetString("ios_No")).GetAwaiter();
						if (!taskAwaiter5.IsCompleted)
						{
							num2 = 0;
							taskAwaiter2 = taskAwaiter5;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<bool>, DriveCycleViewModel.<<get_PeriodToday>b__24_0>d>(ref taskAwaiter5, ref this);
							return;
						}
					}
					else
					{
						taskAwaiter5 = taskAwaiter2;
						taskAwaiter2 = default(TaskAwaiter<bool>);
						num2 = -1;
					}
					if (!taskAwaiter5.GetResult())
					{
						goto IL_012C;
					}
					taskAwaiter3 = page.Navigation.PushAsync(InAppManager.GetInAppPage()).GetAwaiter();
					if (!taskAwaiter3.IsCompleted)
					{
						num2 = 1;
						TaskAwaiter taskAwaiter4 = taskAwaiter3;
						this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, DriveCycleViewModel.<<get_PeriodToday>b__24_0>d>(ref taskAwaiter3, ref this);
						return;
					}
					IL_0125:
					taskAwaiter3.GetResult();
					IL_012C:;
				}
				catch (Exception ex)
				{
					num2 = -2;
					this.<>t__builder.SetException(ex);
					return;
				}
				IL_016E:
				num2 = -2;
				this.<>t__builder.SetResult();
			}

			// Token: 0x06003E02 RID: 15874 RVA: 0x0032B2B8 File Offset: 0x003294B8
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x04002601 RID: 9729
			public int <>1__state;

			// Token: 0x04002602 RID: 9730
			public AsyncVoidMethodBuilder <>t__builder;

			// Token: 0x04002603 RID: 9731
			public DriveCycleViewModel <>4__this;

			// Token: 0x04002604 RID: 9732
			private FuelStatisticsPage <page>5__2;

			// Token: 0x04002605 RID: 9733
			private TaskAwaiter<bool> <>u__1;

			// Token: 0x04002606 RID: 9734
			private TaskAwaiter <>u__2;
		}

		// Token: 0x02000720 RID: 1824
		[CompilerGenerated]
		[Serializable]
		private sealed class <>c
		{
			// Token: 0x06003E03 RID: 15875 RVA: 0x0032B2C6 File Offset: 0x003294C6
			// Note: this type is marked as 'beforefieldinit'.
			static <>c()
			{
			}

			// Token: 0x06003E04 RID: 15876 RVA: 0x00002050 File Offset: 0x00000250
			public <>c()
			{
			}

			// Token: 0x06003E05 RID: 15877 RVA: 0x0032B2D2 File Offset: 0x003294D2
			internal CombinedDriveCycle <GetCombinedDriveCycles>b__115_0(DriveCycle x)
			{
				return new CombinedDriveCycle(x);
			}

			// Token: 0x04002607 RID: 9735
			public static readonly DriveCycleViewModel.<>c <>9 = new DriveCycleViewModel.<>c();

			// Token: 0x04002608 RID: 9736
			public static Func<DriveCycle, CombinedDriveCycle> <>9__115_0;
		}

		// Token: 0x02000721 RID: 1825
		[CompilerGenerated]
		private sealed class <>c__DisplayClass112_0
		{
			// Token: 0x06003E06 RID: 15878 RVA: 0x00002050 File Offset: 0x00000250
			public <>c__DisplayClass112_0()
			{
			}

			// Token: 0x06003E07 RID: 15879 RVA: 0x0032B2DC File Offset: 0x003294DC
			internal bool <CalculatePeriod>b__0(DriveCycle x)
			{
				return x.TimeStarted.Date >= this.startTime && x.TimeFinished.Date <= this.endTime;
			}

			// Token: 0x04002609 RID: 9737
			public DateTime startTime;

			// Token: 0x0400260A RID: 9738
			public DateTime endTime;
		}

		// Token: 0x02000722 RID: 1826
		[CompilerGenerated]
		private sealed class <>c__DisplayClass113_0
		{
			// Token: 0x06003E08 RID: 15880 RVA: 0x00002050 File Offset: 0x00000250
			public <>c__DisplayClass113_0()
			{
			}

			// Token: 0x06003E09 RID: 15881 RVA: 0x0032B31F File Offset: 0x0032951F
			internal bool <CalculatePeriodForCalibration>b__0(DriveCycle x)
			{
				return x.TimeStarted >= this.startTime;
			}

			// Token: 0x0400260B RID: 9739
			public DateTime startTime;
		}
	}
}
