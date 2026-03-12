using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using CarScannerXamarinForms.Common;
using CarScannerXamarinForms.DataRecorder;
using CarScannerXamarinForms.OBD2;
using CarScannerXamarinForms.OBD2.RequestProducers;
using CarScannerXamarinForms.Pages;
using CarScannerXamarinForms.PlatformAdapters;
using CarScannerXamarinForms.Settings;
using CarScannerXamarinForms.ViewModels;
using Xamarin.Forms;

namespace CarScannerXamarinForms.CarPlay
{
	// Token: 0x02000BE4 RID: 3044
	public class CarPlayManager : ICarPlayManager, INotifyPropertyChanged
	{
		// Token: 0x1700185D RID: 6237
		// (get) Token: 0x06005B89 RID: 23433 RVA: 0x00439705 File Offset: 0x00437905
		public static CarPlayManager Instance
		{
			get
			{
				if (CarPlayManager._instance == null)
				{
					CarPlayManager._instance = new CarPlayManager();
				}
				return CarPlayManager._instance;
			}
		}

		// Token: 0x06005B8A RID: 23434 RVA: 0x0043971D File Offset: 0x0043791D
		public static void AddRequests(List<OBDRequest> requests)
		{
			if (App.OBDSimulator.IsActive)
			{
				return;
			}
			CarPlayManager instance = CarPlayManager.Instance;
			if (instance == null)
			{
				return;
			}
			instance.UpdateRequests(requests);
		}

		// Token: 0x06005B8B RID: 23435 RVA: 0x0043973C File Offset: 0x0043793C
		protected CarPlayManager()
		{
		}

		// Token: 0x1400006B RID: 107
		// (add) Token: 0x06005B8C RID: 23436 RVA: 0x00439760 File Offset: 0x00437960
		// (remove) Token: 0x06005B8D RID: 23437 RVA: 0x00439798 File Offset: 0x00437998
		public event EventHandler Connected
		{
			[CompilerGenerated]
			add
			{
				EventHandler eventHandler = this.Connected;
				EventHandler eventHandler2;
				do
				{
					eventHandler2 = eventHandler;
					EventHandler eventHandler3 = (EventHandler)Delegate.Combine(eventHandler2, value);
					eventHandler = Interlocked.CompareExchange<EventHandler>(ref this.Connected, eventHandler3, eventHandler2);
				}
				while (eventHandler != eventHandler2);
			}
			[CompilerGenerated]
			remove
			{
				EventHandler eventHandler = this.Connected;
				EventHandler eventHandler2;
				do
				{
					eventHandler2 = eventHandler;
					EventHandler eventHandler3 = (EventHandler)Delegate.Remove(eventHandler2, value);
					eventHandler = Interlocked.CompareExchange<EventHandler>(ref this.Connected, eventHandler3, eventHandler2);
				}
				while (eventHandler != eventHandler2);
			}
		}

		// Token: 0x1400006C RID: 108
		// (add) Token: 0x06005B8E RID: 23438 RVA: 0x004397D0 File Offset: 0x004379D0
		// (remove) Token: 0x06005B8F RID: 23439 RVA: 0x00439808 File Offset: 0x00437A08
		public event EventHandler Disconnected
		{
			[CompilerGenerated]
			add
			{
				EventHandler eventHandler = this.Disconnected;
				EventHandler eventHandler2;
				do
				{
					eventHandler2 = eventHandler;
					EventHandler eventHandler3 = (EventHandler)Delegate.Combine(eventHandler2, value);
					eventHandler = Interlocked.CompareExchange<EventHandler>(ref this.Disconnected, eventHandler3, eventHandler2);
				}
				while (eventHandler != eventHandler2);
			}
			[CompilerGenerated]
			remove
			{
				EventHandler eventHandler = this.Disconnected;
				EventHandler eventHandler2;
				do
				{
					eventHandler2 = eventHandler;
					EventHandler eventHandler3 = (EventHandler)Delegate.Remove(eventHandler2, value);
					eventHandler = Interlocked.CompareExchange<EventHandler>(ref this.Disconnected, eventHandler3, eventHandler2);
				}
				while (eventHandler != eventHandler2);
			}
		}

		// Token: 0x1400006D RID: 109
		// (add) Token: 0x06005B90 RID: 23440 RVA: 0x00439840 File Offset: 0x00437A40
		// (remove) Token: 0x06005B91 RID: 23441 RVA: 0x00439878 File Offset: 0x00437A78
		public event EventHandler<CarPlayPages> CarPlayPageChanged
		{
			[CompilerGenerated]
			add
			{
				EventHandler<CarPlayPages> eventHandler = this.CarPlayPageChanged;
				EventHandler<CarPlayPages> eventHandler2;
				do
				{
					eventHandler2 = eventHandler;
					EventHandler<CarPlayPages> eventHandler3 = (EventHandler<CarPlayPages>)Delegate.Combine(eventHandler2, value);
					eventHandler = Interlocked.CompareExchange<EventHandler<CarPlayPages>>(ref this.CarPlayPageChanged, eventHandler3, eventHandler2);
				}
				while (eventHandler != eventHandler2);
			}
			[CompilerGenerated]
			remove
			{
				EventHandler<CarPlayPages> eventHandler = this.CarPlayPageChanged;
				EventHandler<CarPlayPages> eventHandler2;
				do
				{
					eventHandler2 = eventHandler;
					EventHandler<CarPlayPages> eventHandler3 = (EventHandler<CarPlayPages>)Delegate.Remove(eventHandler2, value);
					eventHandler = Interlocked.CompareExchange<EventHandler<CarPlayPages>>(ref this.CarPlayPageChanged, eventHandler3, eventHandler2);
				}
				while (eventHandler != eventHandler2);
			}
		}

		// Token: 0x1400006E RID: 110
		// (add) Token: 0x06005B92 RID: 23442 RVA: 0x004398B0 File Offset: 0x00437AB0
		// (remove) Token: 0x06005B93 RID: 23443 RVA: 0x004398E8 File Offset: 0x00437AE8
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

		// Token: 0x1700185E RID: 6238
		// (get) Token: 0x06005B94 RID: 23444 RVA: 0x0043991D File Offset: 0x00437B1D
		// (set) Token: 0x06005B95 RID: 23445 RVA: 0x00439925 File Offset: 0x00437B25
		public virtual CarPlayPages CurrentPage
		{
			[CompilerGenerated]
			get
			{
				return this.<CurrentPage>k__BackingField;
			}
			[CompilerGenerated]
			protected set
			{
				this.<CurrentPage>k__BackingField = value;
			}
		}

		// Token: 0x1700185F RID: 6239
		// (get) Token: 0x06005B96 RID: 23446 RVA: 0x0043992E File Offset: 0x00437B2E
		// (set) Token: 0x06005B97 RID: 23447 RVA: 0x00439936 File Offset: 0x00437B36
		public virtual bool IsConnected
		{
			[CompilerGenerated]
			get
			{
				return this.<IsConnected>k__BackingField;
			}
			[CompilerGenerated]
			protected set
			{
				this.<IsConnected>k__BackingField = value;
			}
		}

		// Token: 0x06005B98 RID: 23448 RVA: 0x0043993F File Offset: 0x00437B3F
		protected void OnPropertyChanged(string propertyName)
		{
			PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
			if (propertyChanged == null)
			{
				return;
			}
			propertyChanged(this, new PropertyChangedEventArgs(propertyName));
		}

		// Token: 0x17001860 RID: 6240
		// (get) Token: 0x06005B99 RID: 23449 RVA: 0x00439958 File Offset: 0x00437B58
		public virtual CarPlayDashboardModel DashboardModel
		{
			get
			{
				if (this._DashboardModel == null)
				{
					this._DashboardModel = new CarPlayDashboardModel((this._cpDelegate == null) ? 10 : this._cpDelegate.MaxListItems, (this._cpDelegate == null) ? 10 : this._cpDelegate.MaxTextItems);
				}
				return this._DashboardModel;
			}
		}

		// Token: 0x06005B9A RID: 23450 RVA: 0x004399AC File Offset: 0x00437BAC
		public virtual void OnAccelerationConfigurationChanged()
		{
			this.UpdateAccelerationPage();
		}

		// Token: 0x06005B9B RID: 23451 RVA: 0x004399B4 File Offset: 0x00437BB4
		protected void UpdateAccelerationPage()
		{
			try
			{
				List<KeyValuePair<string, string>> accelerationValues = CarPlayManager.GetAccelerationValues();
				ICarPlayDelegate cpDelegate = this._cpDelegate;
				if (cpDelegate != null)
				{
					cpDelegate.UpdateAcceleration(accelerationValues, string.Concat(new string[]
					{
						SpeedTestViewModel.Instance.SpeedPID.ShortName,
						": ",
						SpeedTestViewModel.Instance.Speed,
						" ",
						SpeedTestViewModel.Instance.Units
					}));
				}
			}
			catch (Exception)
			{
			}
		}

		// Token: 0x06005B9C RID: 23452 RVA: 0x00439A38 File Offset: 0x00437C38
		public static List<KeyValuePair<string, string>> GetAccelerationValues()
		{
			if (!SpeedTestViewModel.Instance.WasLoaded)
			{
				SpeedTestViewModel.Instance.LoadFromFile();
			}
			List<KeyValuePair<string, string>> list = new List<KeyValuePair<string, string>>(SpeedTestViewModel.Instance.TestCollection.Count + 1);
			if (PlatformHelper.IsiOS && SpeedTestViewModel.Instance.SpeedPID != null && SpeedTestViewModel.Instance.Units != null)
			{
				KeyValuePair<string, string> keyValuePair = new KeyValuePair<string, string>(SpeedTestViewModel.Instance.SpeedPID.ShortName, SpeedTestViewModel.Instance.Speed + " " + SpeedTestViewModel.Instance.Units);
				list.Add(keyValuePair);
			}
			for (int i = 0; i < SpeedTestViewModel.Instance.TestCollection.Count; i++)
			{
				ISpeedTest speedTest = SpeedTestViewModel.Instance.TestCollection[i];
				KeyValuePair<string, string> keyValuePair2 = new KeyValuePair<string, string>(speedTest.Name, speedTest.Value);
				list.Add(keyValuePair2);
			}
			return list;
		}

		// Token: 0x06005B9D RID: 23453 RVA: 0x00439B13 File Offset: 0x00437D13
		public virtual void OnDashboardConfigurationUpdated()
		{
			if (!this.IsConnected)
			{
				return;
			}
			this.DashboardModel.LoadFromSettings();
			MainThreadHelper.InvokeOnMainThread(delegate
			{
				ICarPlayDelegate cpDelegate = this._cpDelegate;
				if (cpDelegate != null)
				{
					cpDelegate.UpdateDashboardPagesList(this.DashboardModel.ListOfPages.GetCurrentPage());
				}
				this.UpdateDashboard();
			});
		}

		// Token: 0x06005B9E RID: 23454 RVA: 0x00439B3A File Offset: 0x00437D3A
		public virtual void OnOBDStatusChanged(OBDDataReaderStatus status)
		{
			if (this._cpDelegate == null)
			{
				return;
			}
			this.UpdateStatusPage();
		}

		// Token: 0x06005B9F RID: 23455 RVA: 0x00439B4C File Offset: 0x00437D4C
		protected virtual async void Loop(int loopId)
		{
			while (this.IsConnected)
			{
				if (this._loopCurrentId == loopId)
				{
					if (this._cpDelegate != null)
					{
						CarPlayPages currentPage = this._cpDelegate.GetCurrentPage();
						if (currentPage != this.CurrentPage)
						{
							this.OnPageChanged(currentPage);
						}
						switch (this.CurrentPage)
						{
						case CarPlayPages.Acceleration:
							this.UpdateAccelerationPage();
							break;
						case CarPlayPages.Dashboard:
							this.UpdateDashboard();
							break;
						}
						await Task.Delay(SharedSettings.Current.CarPlayUpdateInterval);
						if (this._loopCurrentId == loopId)
						{
							continue;
						}
					}
				}
				return;
			}
		}

		// Token: 0x06005BA0 RID: 23456 RVA: 0x00439B8C File Offset: 0x00437D8C
		protected virtual void OnPageChanged(CarPlayPages newPage)
		{
			if (!this.IsDelegateConnected)
			{
				return;
			}
			if (this.CurrentPage == CarPlayPages.Acceleration && App.Instance != null)
			{
				RootNavigationPage rootPage = App.Instance.RootPage;
				if (rootPage != null)
				{
					if (!rootPage.Navigation.NavigationStack.Any((Page x) => x.GetType() == typeof(SpeedTestPage)))
					{
						SpeedTestViewModel.Instance.Stop();
					}
				}
			}
			this.CurrentPage = newPage;
			switch (newPage)
			{
			case CarPlayPages.ConnectionStatus:
				this.UpdateStatusPage();
				break;
			case CarPlayPages.Acceleration:
				SpeedTestViewModel.Instance.Initialize();
				if (!SpeedTestViewModel.Instance.IsStarted)
				{
					SpeedTestViewModel.Instance.Start();
				}
				if (!SpeedTestViewModel.Instance.WasLoaded)
				{
					SpeedTestViewModel.Instance.LoadFromFile();
				}
				this.UpdateAccelerationPage();
				break;
			case CarPlayPages.DashboardPagesList:
				this.UpdateDashboardPagesList();
				break;
			case CarPlayPages.Dashboard:
				this.UpdateDashboard();
				break;
			}
			RequestProducerStatic.UpdateOBDReaderRequests();
		}

		// Token: 0x17001861 RID: 6241
		// (get) Token: 0x06005BA1 RID: 23457 RVA: 0x00439C79 File Offset: 0x00437E79
		public bool IsDelegateConnected
		{
			get
			{
				return this._cpDelegate != null;
			}
		}

		// Token: 0x17001862 RID: 6242
		// (get) Token: 0x06005BA2 RID: 23458 RVA: 0x00439C86 File Offset: 0x00437E86
		// (set) Token: 0x06005BA3 RID: 23459 RVA: 0x00439C8E File Offset: 0x00437E8E
		public bool NonDismissableAlertRequested
		{
			[CompilerGenerated]
			get
			{
				return this.<NonDismissableAlertRequested>k__BackingField;
			}
			[CompilerGenerated]
			private set
			{
				this.<NonDismissableAlertRequested>k__BackingField = value;
			}
		}

		// Token: 0x17001863 RID: 6243
		// (get) Token: 0x06005BA4 RID: 23460 RVA: 0x00439C97 File Offset: 0x00437E97
		public ICarPlayDelegate CarPlayDelegate
		{
			get
			{
				return this._cpDelegate;
			}
		}

		// Token: 0x17001864 RID: 6244
		// (get) Token: 0x06005BA5 RID: 23461 RVA: 0x00439C9F File Offset: 0x00437E9F
		// (set) Token: 0x06005BA6 RID: 23462 RVA: 0x00439CA7 File Offset: 0x00437EA7
		public string NonDismissableMessage
		{
			[CompilerGenerated]
			get
			{
				return this.<NonDismissableMessage>k__BackingField;
			}
			[CompilerGenerated]
			private set
			{
				this.<NonDismissableMessage>k__BackingField = value;
			}
		} = "";

		// Token: 0x06005BA7 RID: 23463 RVA: 0x00439CB0 File Offset: 0x00437EB0
		public async void OnDelegateConnected(ICarPlayDelegate appDelegate)
		{
			if (appDelegate != null)
			{
				this._cpDelegate = appDelegate;
				this._cpDelegate.UserSelectedDashboardPageIndex -= this.OnDashboardPageSelectedIndex;
				this._cpDelegate.UserSelectedDashboardPageIndex += this.OnDashboardPageSelectedIndex;
				this.IsConnected = true;
				this.OnPropertyChanged("IsDelegateConnected");
				EventHandler connected = this.Connected;
				if (connected != null)
				{
					connected(this, EventArgs.Empty);
				}
				if (App.OBDReader != null)
				{
					this.UpdateStatusPage();
				}
				this._DashboardModel = new CarPlayDashboardModel((this._cpDelegate == null) ? 10 : this._cpDelegate.MaxListItems, (this._cpDelegate == null) ? 10 : this._cpDelegate.MaxTextItems);
				this.DashboardModel.LoadFromSettings();
				if (PlatformHelper.IsiOS && PlatformHelper.IsPlatformVersionNewerOrEqual(18, 5))
				{
					this.UpdateDashboardPagesList();
					this.UpdateAccelerationPage();
				}
				if (this.NonDismissableAlertRequested)
				{
					ICarPlayDelegate cpDelegate = this._cpDelegate;
					if (cpDelegate != null)
					{
						cpDelegate.ShowNonDismissableAlert(this.NonDismissableMessage);
					}
				}
				Task.Run(delegate
				{
					Random random = new Random();
					this._loopCurrentId = random.Next();
					this.Loop(this._loopCurrentId);
				});
				if (SimpleMainPage.Instance == null && SharedSettings.Current.ConnectOnLaunch)
				{
					await this.Connect();
					if (SharedSettings.Current.OpenDashboardOnLaunch && App.OBDReader != null && App.OBDReader.CurrentStatus == OBDDataReaderStatus.ConnectedToECU)
					{
						this.UpdateDashboardPagesList();
						this.OnDashboardPageSelectedIndex(this, SharedSettings.Current.CarPlayDashboardSelectedIndex);
					}
				}
			}
		}

		// Token: 0x06005BA8 RID: 23464 RVA: 0x00439CF0 File Offset: 0x00437EF0
		public void OnDelegateDisconnected(ICarPlayDelegate appDelegate)
		{
			this.IsConnected = false;
			if (this._cpDelegate != null)
			{
				this._cpDelegate.UserSelectedDashboardPageIndex -= this.OnDashboardPageSelectedIndex;
			}
			this._cpDelegate = null;
			this.OnPropertyChanged("IsDelegateConnected");
			EventHandler disconnected = this.Disconnected;
			if (disconnected != null)
			{
				disconnected(this, EventArgs.Empty);
			}
			if (App.OBDReader != null)
			{
				List<OBDRequest> queueCopy = App.OBDReader.GetQueueCopy();
				queueCopy.RemoveAll((OBDRequest x) => x.Keys.ContainsKey("CarPlay"));
				App.OBDReader.ReplaceQueue(queueCopy);
			}
			if (this.DashboardModel != null)
			{
				foreach (CarPlayDashboardPage carPlayDashboardPage in this.DashboardModel)
				{
					foreach (CarPlayDashboardItem carPlayDashboardItem in carPlayDashboardPage)
					{
						LiveDataPIDModel model = carPlayDashboardItem.Model;
						if (model != null)
						{
							model.Unsubscribe();
						}
					}
				}
			}
		}

		// Token: 0x06005BA9 RID: 23465 RVA: 0x00439E1C File Offset: 0x0043801C
		public virtual void UpdateRequests(List<OBDRequest> requests)
		{
			if (!this.IsDelegateConnected)
			{
				return;
			}
			switch (this.CurrentPage)
			{
			case CarPlayPages.ConnectionStatus:
				return;
			case CarPlayPages.Acceleration:
				SpeedTestViewModel.Instance.Initialize();
				LiveDataPIDModel.GetRequests(SpeedTestViewModel.Instance.SpeedPID, requests, "CarPlay", "");
				return;
			case CarPlayPages.CodingAndServiceOpened:
			case CarPlayPages.DashboardPagesList:
				break;
			case CarPlayPages.Dashboard:
			{
				CarPlayDashboardPage dashPage = this.DashboardModel.GetCurrentPage();
				CarPlayDashboardPage dashPage2 = dashPage;
				if (dashPage2 != null)
				{
					dashPage2.GetRequests(requests);
				}
				try
				{
					foreach (CarPlayDashboardPage carPlayDashboardPage in this.DashboardModel.Where((CarPlayDashboardPage x) => x != null && x.UpdateInBackground && x != dashPage).ToList<CarPlayDashboardPage>())
					{
						carPlayDashboardPage.GetRequests(requests);
					}
				}
				catch (Exception)
				{
				}
				break;
			}
			default:
				return;
			}
		}

		// Token: 0x06005BAA RID: 23466 RVA: 0x00439F14 File Offset: 0x00438114
		protected virtual void UpdateDashboard()
		{
			if (!this.IsConnected)
			{
				return;
			}
			try
			{
				KeyValuePair<string, string>[] dashboardValues = this.GetDashboardValues();
				ICarPlayDelegate cpDelegate = this._cpDelegate;
				if (cpDelegate != null)
				{
					cpDelegate.UpdateDashboardValues(dashboardValues);
				}
			}
			catch (Exception)
			{
			}
		}

		// Token: 0x06005BAB RID: 23467 RVA: 0x00439F5C File Offset: 0x0043815C
		public KeyValuePair<string, string>[] GetDashboardValues()
		{
			CarPlayDashboardPage currentPage = this.DashboardModel.GetCurrentPage();
			KeyValuePair<string, string>[] array = new KeyValuePair<string, string>[currentPage.Count];
			for (int i = 0; i < currentPage.Count; i++)
			{
				CarPlayDashboardItem carPlayDashboardItem = currentPage[i];
				string text = carPlayDashboardItem.Title;
				if (string.IsNullOrEmpty(carPlayDashboardItem.Title))
				{
					if (carPlayDashboardItem.Model != null && carPlayDashboardItem.Model.SelectedPID != null && !string.IsNullOrEmpty(carPlayDashboardItem.Model.SelectedPID.ShortName))
					{
						text = carPlayDashboardItem.Model.SelectedPID.ShortName;
					}
					else
					{
						text = "#" + i.ToString();
					}
				}
				KeyValuePair<string, string> keyValuePair = new KeyValuePair<string, string>(text, carPlayDashboardItem.Value);
				array[i] = keyValuePair;
			}
			return array;
		}

		// Token: 0x06005BAC RID: 23468 RVA: 0x0043A024 File Offset: 0x00438224
		protected virtual void UpdateStatusPage()
		{
			try
			{
				KeyValuePair<string, string>[] array;
				ConnectButtonVisible connectButtonVisible;
				CarPlayManager.GetOBDStatusValues(out array, out connectButtonVisible);
				ICarPlayDelegate cpDelegate = this._cpDelegate;
				if (cpDelegate != null)
				{
					cpDelegate.UpdateStatusTab(array, connectButtonVisible);
				}
			}
			catch (Exception)
			{
			}
		}

		// Token: 0x06005BAD RID: 23469 RVA: 0x0043A064 File Offset: 0x00438264
		public static void GetOBDStatusValues(out KeyValuePair<string, string>[] values, out ConnectButtonVisible button)
		{
			string @string = Translate.GetString("MainPage_StatusDisconnected");
			string string2 = Translate.GetString("MainPage_StatusConnected");
			string string3 = Translate.GetString("MainPage_StatusConnecting");
			string text = "";
			string text2 = "";
			if (App.OBDReader == null)
			{
				text = @string;
				text2 = @string;
			}
			else
			{
				switch (App.OBDReader.CurrentStatus)
				{
				case OBDDataReaderStatus.Disconnected:
					text = @string;
					text2 = @string;
					break;
				case OBDDataReaderStatus.ConnectingToELM:
					text = string3;
					text2 = @string;
					break;
				case OBDDataReaderStatus.ConnectedToELM:
					text = string2;
					text2 = @string;
					break;
				case OBDDataReaderStatus.ConnectingToECU:
					text = string2;
					text2 = string3;
					break;
				case OBDDataReaderStatus.ConnectedToECU:
					text = string2;
					text2 = string2;
					break;
				}
			}
			KeyValuePair<string, string> keyValuePair = new KeyValuePair<string, string>(Translate.GetString("MainPage_ELM_Connection.Text"), text);
			KeyValuePair<string, string> keyValuePair2 = new KeyValuePair<string, string>(Translate.GetString("MainPage_ECU_Connection.Text"), text2);
			if (App.OBDReader != null && App.OBDReader.CurrentStatus == OBDDataReaderStatus.ConnectedToECU && CarInfoViewModel.Instance != null && CarInfoViewModel.Instance.IsVINAvailable)
			{
				KeyValuePair<string, string> keyValuePair3 = new KeyValuePair<string, string>("VIN", CarInfoViewModel.Instance.VIN);
				values = new KeyValuePair<string, string>[] { keyValuePair, keyValuePair2, keyValuePair3 };
			}
			else
			{
				values = new KeyValuePair<string, string>[] { keyValuePair, keyValuePair2 };
			}
			button = ConnectButtonVisible.None;
			if (App.OBDReader == null)
			{
				button = ConnectButtonVisible.Connect;
				return;
			}
			switch (App.OBDReader.CurrentStatus)
			{
			case OBDDataReaderStatus.Disconnected:
				button = ConnectButtonVisible.Connect;
				return;
			case OBDDataReaderStatus.ConnectingToELM:
			case OBDDataReaderStatus.ConnectedToELM:
			case OBDDataReaderStatus.ConnectingToECU:
			case OBDDataReaderStatus.ConnectedToECU:
				button = ConnectButtonVisible.Disconnect;
				return;
			case OBDDataReaderStatus.Disconnecting:
				button = ConnectButtonVisible.None;
				return;
			default:
				return;
			}
		}

		// Token: 0x06005BAE RID: 23470 RVA: 0x0043A1E0 File Offset: 0x004383E0
		public void OnDisconnectTapped()
		{
			if (App.OBDReader.CurrentStatus == OBDDataReaderStatus.Disconnecting || App.OBDReader.CurrentStatus == OBDDataReaderStatus.Disconnected || App.OBDReader.DisconnectRequested)
			{
				return;
			}
			if (App.OBDSimulator != null && App.OBDSimulator.IsActive)
			{
				App.OBDReader.SetStatusForTest(OBDDataReaderStatus.Disconnected);
				App.OBDSimulator.Stop();
				LiveDataPIDModel._PIDCollection.Clear();
				SimpleMainPage instance = SimpleMainPage.Instance;
				if (instance != null)
				{
					instance.UpdateVINInfo();
				}
			}
			if (SimpleMainPage.Instance != null)
			{
				SimpleMainPage.Instance.btnDisconnect_Clicked(null, EventArgs.Empty);
			}
			else
			{
				App.OBDReader.DisconnectRequested = true;
				App.OBDReader.Disconnect("UserClickbtnDisconnectCarPlay");
				DataRecorderV2.StopRecording(OBDDataReaderStatus.Disconnected);
				if (PlatformHelper.IsAndroid)
				{
					PlatformHelper.DroidService.AndroidHelper_StopService();
				}
			}
			this.UpdateStatusPage();
		}

		// Token: 0x06005BAF RID: 23471 RVA: 0x0043A2AC File Offset: 0x004384AC
		public async void OnConnectTapped()
		{
			await this.Connect();
		}

		// Token: 0x06005BB0 RID: 23472 RVA: 0x0043A2E4 File Offset: 0x004384E4
		private async Task Connect()
		{
			if (App.OBDReader.CurrentStatus != OBDDataReaderStatus.Disconnecting)
			{
				if (!SharedSettings.Current.FirstConnectionAttempted || string.IsNullOrEmpty(SharedSettings.Current.SelectedProfileV2Name))
				{
					ICarPlayDelegate cpDelegate = this._cpDelegate;
					if (cpDelegate != null)
					{
						cpDelegate.DisplayAlert(new string[] { Translate.GetString("carPlay_FirstConnectionRequired") });
					}
				}
				else if (SimpleMainPage.Instance == null)
				{
					this.InitOBDReaderIfNull();
					await CarPlayOBDHelper.StartConnectionLight();
				}
				else
				{
					SimpleMainPage.Instance.btnConnect_Clicked(null, EventArgs.Empty);
				}
			}
		}

		// Token: 0x06005BB1 RID: 23473 RVA: 0x0043A328 File Offset: 0x00438528
		public void ResetAcceleration()
		{
			if (SpeedTestViewModel.Instance.TestCollection != null)
			{
				foreach (ISpeedTest speedTest in SpeedTestViewModel.Instance.TestCollection)
				{
					speedTest.Cancel();
					speedTest.Start();
				}
			}
		}

		// Token: 0x06005BB2 RID: 23474 RVA: 0x0043A388 File Offset: 0x00438588
		protected void UpdateDashboardPagesList()
		{
			ICarPlayDelegate cpDelegate = this._cpDelegate;
			if (cpDelegate == null)
			{
				return;
			}
			cpDelegate.UpdateDashboardPagesList(this.DashboardModel.ListOfPages.GetCurrentPage());
		}

		// Token: 0x06005BB3 RID: 23475 RVA: 0x0043A3AA File Offset: 0x004385AA
		public void DisplayAlert(string message)
		{
			ICarPlayDelegate cpDelegate = this._cpDelegate;
			if (cpDelegate == null)
			{
				return;
			}
			cpDelegate.DisplayAlert(new string[] { message });
		}

		// Token: 0x06005BB4 RID: 23476 RVA: 0x0043A3C8 File Offset: 0x004385C8
		public void OnDashboardPageSelectedIndex(object sender, int idx)
		{
			if (idx == -2147483648)
			{
				this.DashboardModel.ListOfPages.PrevPage();
				this.UpdateDashboardPagesList();
				return;
			}
			if (idx == 2147483647)
			{
				this.DashboardModel.ListOfPages.NextPage();
				this.UpdateDashboardPagesList();
				return;
			}
			if (App.OBDReader.CurrentStatus == OBDDataReaderStatus.ConnectingToECU || App.OBDReader.CurrentStatus == OBDDataReaderStatus.ConnectingToELM || App.OBDReader.CurrentStatus == OBDDataReaderStatus.Disconnecting)
			{
				ICarPlayDelegate cpDelegate = this._cpDelegate;
				if (cpDelegate == null)
				{
					return;
				}
				cpDelegate.DisplayAlert(new string[] { Translate.GetString("ios_ConnectionInProgressText") });
				return;
			}
			else if (App.OBDReader.CurrentStatus == OBDDataReaderStatus.ConnectedToELM)
			{
				ICarPlayDelegate cpDelegate2 = this._cpDelegate;
				if (cpDelegate2 == null)
				{
					return;
				}
				cpDelegate2.DisplayAlert(new string[] { Translate.GetString("ios_ConnectedToElmOnlyTitle") });
				return;
			}
			else if (App.OBDReader.CurrentStatus == OBDDataReaderStatus.Disconnected)
			{
				ICarPlayDelegate cpDelegate3 = this._cpDelegate;
				if (cpDelegate3 == null)
				{
					return;
				}
				cpDelegate3.DisplayAlert(new string[]
				{
					Translate.GetString("ios_DisconnectedFromELMTitle"),
					Translate.GetString("ios_DisconnectedFromELMText")
				});
				return;
			}
			else if (!SharedSettings.Current.AdsProductPurchased && idx > 2)
			{
				ICarPlayDelegate cpDelegate4 = this._cpDelegate;
				if (cpDelegate4 == null)
				{
					return;
				}
				cpDelegate4.DisplayAlert(new string[] { Translate.GetString("carPlay_DashboardOnlyFirst3AvailableInFree") });
				return;
			}
			else
			{
				if (!this.DashboardModel.IsLoaded)
				{
					this.DashboardModel.LoadFromSettings();
				}
				this.DashboardModel.CurrentPageIndex = idx;
				CarPlayDashboardPage currentPage = this.DashboardModel.GetCurrentPage();
				currentPage.Initialize();
				this.UpdateDashboard();
				ICarPlayDelegate cpDelegate5 = this._cpDelegate;
				if (cpDelegate5 == null)
				{
					return;
				}
				cpDelegate5.OpenDashboard(currentPage.Title);
				return;
			}
		}

		// Token: 0x06005BB5 RID: 23477 RVA: 0x0043A556 File Offset: 0x00438756
		public void OnVINLoaded(string VIN)
		{
			this.UpdateStatusPage();
		}

		// Token: 0x06005BB6 RID: 23478 RVA: 0x0043A55E File Offset: 0x0043875E
		public void DisplayNonDismissableAlert(string message)
		{
			if (PlatformHelper.IsAndroid && message != null)
			{
				message = message.Replace("CarPlay", "Android Auto");
			}
			this.NonDismissableAlertRequested = true;
			this.NonDismissableMessage = message;
			ICarPlayDelegate cpDelegate = this._cpDelegate;
			if (cpDelegate == null)
			{
				return;
			}
			cpDelegate.ShowNonDismissableAlert(message);
		}

		// Token: 0x06005BB7 RID: 23479 RVA: 0x0043A59C File Offset: 0x0043879C
		public void HideNonDismissableAlert()
		{
			this.NonDismissableAlertRequested = false;
			this.NonDismissableMessage = "";
			ICarPlayDelegate cpDelegate = this._cpDelegate;
			if (cpDelegate == null)
			{
				return;
			}
			cpDelegate.HideNonDismissableAlert();
		}

		// Token: 0x06005BB8 RID: 23480 RVA: 0x0043A5C1 File Offset: 0x004387C1
		private void InitOBDReaderIfNull()
		{
			if (App.OBDReader == null)
			{
				App.OBDReader = new OBDDataReader();
				CarPlayOBDHelper.Initialize();
			}
		}

		// Token: 0x06005BB9 RID: 23481 RVA: 0x0043A5D9 File Offset: 0x004387D9
		[CompilerGenerated]
		private void <OnDashboardConfigurationUpdated>b__33_0()
		{
			ICarPlayDelegate cpDelegate = this._cpDelegate;
			if (cpDelegate != null)
			{
				cpDelegate.UpdateDashboardPagesList(this.DashboardModel.ListOfPages.GetCurrentPage());
			}
			this.UpdateDashboard();
		}

		// Token: 0x06005BBA RID: 23482 RVA: 0x0043A604 File Offset: 0x00438804
		[CompilerGenerated]
		private void <OnDelegateConnected>b__50_0()
		{
			Random random = new Random();
			this._loopCurrentId = random.Next();
			this.Loop(this._loopCurrentId);
		}

		// Token: 0x040039C3 RID: 14787
		private static CarPlayManager _instance;

		// Token: 0x040039C4 RID: 14788
		private ICarPlayDelegate _cpDelegate;

		// Token: 0x040039C5 RID: 14789
		[CompilerGenerated]
		private EventHandler Connected;

		// Token: 0x040039C6 RID: 14790
		[CompilerGenerated]
		private EventHandler Disconnected;

		// Token: 0x040039C7 RID: 14791
		[CompilerGenerated]
		private EventHandler<CarPlayPages> CarPlayPageChanged;

		// Token: 0x040039C8 RID: 14792
		[CompilerGenerated]
		private PropertyChangedEventHandler PropertyChanged;

		// Token: 0x040039C9 RID: 14793
		[CompilerGenerated]
		private CarPlayPages <CurrentPage>k__BackingField;

		// Token: 0x040039CA RID: 14794
		[CompilerGenerated]
		private bool <IsConnected>k__BackingField;

		// Token: 0x040039CB RID: 14795
		protected CarPlayDashboardModel _DashboardModel = new CarPlayDashboardModel(10, 10);

		// Token: 0x040039CC RID: 14796
		private int _loopCurrentId;

		// Token: 0x040039CD RID: 14797
		[CompilerGenerated]
		private bool <NonDismissableAlertRequested>k__BackingField;

		// Token: 0x040039CE RID: 14798
		[CompilerGenerated]
		private string <NonDismissableMessage>k__BackingField;

		// Token: 0x02000BE5 RID: 3045
		[CompilerGenerated]
		[Serializable]
		private sealed class <>c
		{
			// Token: 0x06005BBB RID: 23483 RVA: 0x0043A62F File Offset: 0x0043882F
			// Note: this type is marked as 'beforefieldinit'.
			static <>c()
			{
			}

			// Token: 0x06005BBC RID: 23484 RVA: 0x00002050 File Offset: 0x00000250
			public <>c()
			{
			}

			// Token: 0x06005BBD RID: 23485 RVA: 0x0043A63B File Offset: 0x0043883B
			internal bool <OnPageChanged>b__37_0(Page x)
			{
				return x.GetType() == typeof(SpeedTestPage);
			}

			// Token: 0x06005BBE RID: 23486 RVA: 0x0043A652 File Offset: 0x00438852
			internal bool <OnDelegateDisconnected>b__51_0(OBDRequest x)
			{
				return x.Keys.ContainsKey("CarPlay");
			}

			// Token: 0x040039CF RID: 14799
			public static readonly CarPlayManager.<>c <>9 = new CarPlayManager.<>c();

			// Token: 0x040039D0 RID: 14800
			public static Func<Page, bool> <>9__37_0;

			// Token: 0x040039D1 RID: 14801
			public static Predicate<OBDRequest> <>9__51_0;
		}

		// Token: 0x02000BE6 RID: 3046
		[CompilerGenerated]
		private sealed class <>c__DisplayClass52_0
		{
			// Token: 0x06005BBF RID: 23487 RVA: 0x00002050 File Offset: 0x00000250
			public <>c__DisplayClass52_0()
			{
			}

			// Token: 0x06005BC0 RID: 23488 RVA: 0x0043A664 File Offset: 0x00438864
			internal bool <UpdateRequests>b__0(CarPlayDashboardPage x)
			{
				return x != null && x.UpdateInBackground && x != this.dashPage;
			}

			// Token: 0x040039D2 RID: 14802
			public CarPlayDashboardPage dashPage;
		}

		// Token: 0x02000BE7 RID: 3047
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <Connect>d__59 : IAsyncStateMachine
		{
			// Token: 0x06005BC1 RID: 23489 RVA: 0x0043A680 File Offset: 0x00438880
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				CarPlayManager carPlayManager = this;
				try
				{
					TaskAwaiter taskAwaiter;
					if (num != 0)
					{
						if (App.OBDReader.CurrentStatus == OBDDataReaderStatus.Disconnecting)
						{
							goto IL_00FC;
						}
						if (!SharedSettings.Current.FirstConnectionAttempted || string.IsNullOrEmpty(SharedSettings.Current.SelectedProfileV2Name))
						{
							ICarPlayDelegate cpDelegate = carPlayManager._cpDelegate;
							if (cpDelegate != null)
							{
								cpDelegate.DisplayAlert(new string[] { Translate.GetString("carPlay_FirstConnectionRequired") });
							}
							goto IL_00FC;
						}
						if (SimpleMainPage.Instance != null)
						{
							SimpleMainPage.Instance.btnConnect_Clicked(null, EventArgs.Empty);
							goto IL_00E3;
						}
						carPlayManager.InitOBDReaderIfNull();
						taskAwaiter = CarPlayOBDHelper.StartConnectionLight().GetAwaiter();
						if (!taskAwaiter.IsCompleted)
						{
							num2 = 0;
							TaskAwaiter taskAwaiter2 = taskAwaiter;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, CarPlayManager.<Connect>d__59>(ref taskAwaiter, ref this);
							return;
						}
					}
					else
					{
						TaskAwaiter taskAwaiter2;
						taskAwaiter = taskAwaiter2;
						taskAwaiter2 = default(TaskAwaiter);
						num2 = -1;
					}
					taskAwaiter.GetResult();
					IL_00E3:;
				}
				catch (Exception ex)
				{
					num2 = -2;
					this.<>t__builder.SetException(ex);
					return;
				}
				IL_00FC:
				num2 = -2;
				this.<>t__builder.SetResult();
			}

			// Token: 0x06005BC2 RID: 23490 RVA: 0x0043A7AC File Offset: 0x004389AC
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x040039D3 RID: 14803
			public int <>1__state;

			// Token: 0x040039D4 RID: 14804
			public AsyncTaskMethodBuilder <>t__builder;

			// Token: 0x040039D5 RID: 14805
			public CarPlayManager <>4__this;

			// Token: 0x040039D6 RID: 14806
			private TaskAwaiter <>u__1;
		}

		// Token: 0x02000BE8 RID: 3048
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <Loop>d__36 : IAsyncStateMachine
		{
			// Token: 0x06005BC3 RID: 23491 RVA: 0x0043A7BC File Offset: 0x004389BC
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				CarPlayManager carPlayManager = this;
				try
				{
					if (num != 0)
					{
						goto IL_00FC;
					}
					TaskAwaiter taskAwaiter2;
					TaskAwaiter taskAwaiter = taskAwaiter2;
					taskAwaiter2 = default(TaskAwaiter);
					num2 = -1;
					IL_00E5:
					taskAwaiter.GetResult();
					if (carPlayManager._loopCurrentId != loopId)
					{
						goto IL_0122;
					}
					IL_00FC:
					if (carPlayManager.IsConnected)
					{
						if (carPlayManager._loopCurrentId == loopId)
						{
							if (carPlayManager._cpDelegate != null)
							{
								CarPlayPages currentPage = carPlayManager._cpDelegate.GetCurrentPage();
								if (currentPage != carPlayManager.CurrentPage)
								{
									carPlayManager.OnPageChanged(currentPage);
								}
								switch (carPlayManager.CurrentPage)
								{
								case CarPlayPages.Acceleration:
									carPlayManager.UpdateAccelerationPage();
									break;
								case CarPlayPages.Dashboard:
									carPlayManager.UpdateDashboard();
									break;
								}
								taskAwaiter = Task.Delay(SharedSettings.Current.CarPlayUpdateInterval).GetAwaiter();
								if (!taskAwaiter.IsCompleted)
								{
									num2 = 0;
									taskAwaiter2 = taskAwaiter;
									this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, CarPlayManager.<Loop>d__36>(ref taskAwaiter, ref this);
									return;
								}
								goto IL_00E5;
							}
						}
					}
				}
				catch (Exception ex)
				{
					num2 = -2;
					this.<>t__builder.SetException(ex);
					return;
				}
				IL_0122:
				num2 = -2;
				this.<>t__builder.SetResult();
			}

			// Token: 0x06005BC4 RID: 23492 RVA: 0x0043A910 File Offset: 0x00438B10
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x040039D7 RID: 14807
			public int <>1__state;

			// Token: 0x040039D8 RID: 14808
			public AsyncVoidMethodBuilder <>t__builder;

			// Token: 0x040039D9 RID: 14809
			public CarPlayManager <>4__this;

			// Token: 0x040039DA RID: 14810
			public int loopId;

			// Token: 0x040039DB RID: 14811
			private TaskAwaiter <>u__1;
		}

		// Token: 0x02000BE9 RID: 3049
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <OnConnectTapped>d__58 : IAsyncStateMachine
		{
			// Token: 0x06005BC5 RID: 23493 RVA: 0x0043A920 File Offset: 0x00438B20
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				CarPlayManager carPlayManager = this;
				try
				{
					TaskAwaiter taskAwaiter;
					if (num != 0)
					{
						taskAwaiter = carPlayManager.Connect().GetAwaiter();
						if (!taskAwaiter.IsCompleted)
						{
							num2 = 0;
							TaskAwaiter taskAwaiter2 = taskAwaiter;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, CarPlayManager.<OnConnectTapped>d__58>(ref taskAwaiter, ref this);
							return;
						}
					}
					else
					{
						TaskAwaiter taskAwaiter2;
						taskAwaiter = taskAwaiter2;
						taskAwaiter2 = default(TaskAwaiter);
						num2 = -1;
					}
					taskAwaiter.GetResult();
				}
				catch (Exception ex)
				{
					num2 = -2;
					this.<>t__builder.SetException(ex);
					return;
				}
				num2 = -2;
				this.<>t__builder.SetResult();
			}

			// Token: 0x06005BC6 RID: 23494 RVA: 0x0043A9D4 File Offset: 0x00438BD4
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x040039DC RID: 14812
			public int <>1__state;

			// Token: 0x040039DD RID: 14813
			public AsyncVoidMethodBuilder <>t__builder;

			// Token: 0x040039DE RID: 14814
			public CarPlayManager <>4__this;

			// Token: 0x040039DF RID: 14815
			private TaskAwaiter <>u__1;
		}

		// Token: 0x02000BEA RID: 3050
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <OnDelegateConnected>d__50 : IAsyncStateMachine
		{
			// Token: 0x06005BC7 RID: 23495 RVA: 0x0043A9E4 File Offset: 0x00438BE4
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				CarPlayManager carPlayManager = this;
				try
				{
					TaskAwaiter taskAwaiter;
					if (num != 0)
					{
						if (appDelegate == null)
						{
							goto IL_01EA;
						}
						carPlayManager._cpDelegate = appDelegate;
						carPlayManager._cpDelegate.UserSelectedDashboardPageIndex -= carPlayManager.OnDashboardPageSelectedIndex;
						carPlayManager._cpDelegate.UserSelectedDashboardPageIndex += carPlayManager.OnDashboardPageSelectedIndex;
						carPlayManager.IsConnected = true;
						carPlayManager.OnPropertyChanged("IsDelegateConnected");
						EventHandler connected = carPlayManager.Connected;
						if (connected != null)
						{
							connected(carPlayManager, EventArgs.Empty);
						}
						if (App.OBDReader != null)
						{
							carPlayManager.UpdateStatusPage();
						}
						carPlayManager._DashboardModel = new CarPlayDashboardModel((carPlayManager._cpDelegate == null) ? 10 : carPlayManager._cpDelegate.MaxListItems, (carPlayManager._cpDelegate == null) ? 10 : carPlayManager._cpDelegate.MaxTextItems);
						carPlayManager.DashboardModel.LoadFromSettings();
						if (PlatformHelper.IsiOS && PlatformHelper.IsPlatformVersionNewerOrEqual(18, 5))
						{
							carPlayManager.UpdateDashboardPagesList();
							carPlayManager.UpdateAccelerationPage();
						}
						if (carPlayManager.NonDismissableAlertRequested)
						{
							ICarPlayDelegate cpDelegate = carPlayManager._cpDelegate;
							if (cpDelegate != null)
							{
								cpDelegate.ShowNonDismissableAlert(carPlayManager.NonDismissableMessage);
							}
						}
						Task.Run(delegate
						{
							Random random = new Random();
							carPlayManager._loopCurrentId = random.Next();
							carPlayManager.Loop(carPlayManager._loopCurrentId);
						});
						if (SimpleMainPage.Instance != null || !SharedSettings.Current.ConnectOnLaunch)
						{
							goto IL_01D1;
						}
						taskAwaiter = carPlayManager.Connect().GetAwaiter();
						if (!taskAwaiter.IsCompleted)
						{
							num2 = 0;
							TaskAwaiter taskAwaiter2 = taskAwaiter;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, CarPlayManager.<OnDelegateConnected>d__50>(ref taskAwaiter, ref this);
							return;
						}
					}
					else
					{
						TaskAwaiter taskAwaiter2;
						taskAwaiter = taskAwaiter2;
						taskAwaiter2 = default(TaskAwaiter);
						num2 = -1;
					}
					taskAwaiter.GetResult();
					if (SharedSettings.Current.OpenDashboardOnLaunch && App.OBDReader != null && App.OBDReader.CurrentStatus == OBDDataReaderStatus.ConnectedToECU)
					{
						carPlayManager.UpdateDashboardPagesList();
						carPlayManager.OnDashboardPageSelectedIndex(carPlayManager, SharedSettings.Current.CarPlayDashboardSelectedIndex);
					}
					IL_01D1:;
				}
				catch (Exception ex)
				{
					num2 = -2;
					this.<>t__builder.SetException(ex);
					return;
				}
				IL_01EA:
				num2 = -2;
				this.<>t__builder.SetResult();
			}

			// Token: 0x06005BC8 RID: 23496 RVA: 0x0043AC0C File Offset: 0x00438E0C
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x040039E0 RID: 14816
			public int <>1__state;

			// Token: 0x040039E1 RID: 14817
			public AsyncVoidMethodBuilder <>t__builder;

			// Token: 0x040039E2 RID: 14818
			public ICarPlayDelegate appDelegate;

			// Token: 0x040039E3 RID: 14819
			public CarPlayManager <>4__this;

			// Token: 0x040039E4 RID: 14820
			private TaskAwaiter <>u__1;
		}
	}
}
