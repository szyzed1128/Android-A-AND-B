using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Windows.Input;
using CarScannerXamarinForms.Common;
using CarScannerXamarinForms.DataRecorder;
using CarScannerXamarinForms.OBD2;
using CarScannerXamarinForms.OBD2.PIDS;
using CarScannerXamarinForms.OBD2.PIDS.CalculatedPIDs;
using CarScannerXamarinForms.OBD2.PIDS.InternalActions;
using CarScannerXamarinForms.Settings;
using Xamarin.CommunityToolkit.Extensions;
using Xamarin.Forms;

namespace CarScannerXamarinForms.ViewModels
{
	// Token: 0x02000742 RID: 1858
	public class LiveDataPIDModel : INotifyPropertyChanged, IDisposable
	{
		// Token: 0x06003F1C RID: 16156 RVA: 0x0032EE94 File Offset: 0x0032D094
		public static void LoadCustomPIDS()
		{
			if (!CustomPIDViewModel.CurrentProfile.Loaded)
			{
				CustomPIDViewModel.CurrentProfile.Load();
			}
			if (!CustomPIDViewModel.CurrentCustom.Loaded)
			{
				CustomPIDViewModel.CurrentCustom.Load();
			}
			CustomPIDViewModel.CheckIdIntegrity();
			foreach (CustomPID customPID in CustomPIDViewModel.CurrentProfile.PidCollection.Concat(CustomPIDViewModel.CurrentCustom.PidCollection).ToList<CustomPID>())
			{
				customPID.ReloadFormula();
			}
		}

		// Token: 0x06003F1D RID: 16157 RVA: 0x0032EF30 File Offset: 0x0032D130
		public static void UpdatePIDCollection(OBDDataReader OBDDataReader)
		{
			IEnumerable<PID> enumerable = from x in OBDDataReader.CurrentCarData.LiveDataPIDs
				where x.IsAvailable && !(x is CalculatedPIDV2) && (x is IPIDFloatValue || x is PIDWithStringValue || x is PID0103_FuelSystemStatus || x is PID_Status)
				orderby x is PID_ATRV descending, !(x is InternalActionPID), x.Command
				select x;
			IOrderedEnumerable<PID> orderedEnumerable = from x in App.OBDReader.CurrentCarData.LiveDataPIDs
				where x.IsAvailable && x is CalculatedPIDV2
				orderby x.Name
				select x;
			LiveDataPIDModel._PIDCollection.Clear();
			LiveDataPIDModel._PIDCollection.Add(PID.Empty);
			foreach (PID pid in enumerable)
			{
				LiveDataPIDModel._PIDCollection.Add(pid);
			}
			foreach (PID pid2 in orderedEnumerable)
			{
				LiveDataPIDModel._PIDCollection.Add(pid2);
			}
			foreach (CustomPID customPID in CustomPIDViewModel.CurrentProfile.PidCollection)
			{
				if (App.OBDReader.STCommandsStupported || !customPID.RequiresSTCommands)
				{
					LiveDataPIDModel._PIDCollection.Add(customPID);
					customPID.IsAvailable = true;
				}
			}
			foreach (CustomPID customPID2 in CustomPIDViewModel.CurrentCustom.PidCollection)
			{
				if (App.OBDReader.STCommandsStupported || !customPID2.RequiresSTCommands)
				{
					LiveDataPIDModel._PIDCollection.Add(customPID2);
					customPID2.IsAvailable = true;
				}
			}
			LiveDataPIDModel.SetShouldHideForSelectedPids();
			foreach (PID pid3 in LiveDataPIDModel._PIDCollection.Where((PID x) => x is IPIDFloatValue))
			{
				pid3.ValueChanged -= new EventHandler<PID>(LiveDataPIDModel.PIDValueChangedFireRecordData);
				pid3.ValueChanged += new EventHandler<PID>(LiveDataPIDModel.PIDValueChangedFireRecordData);
			}
			SharedSettings.Current.LastCarAvailableSensors = LiveDataPIDModel.GetAvailableSensorsString();
			if (App.OBDReader != null && App.OBDReader.CurrentStatus == OBDDataReaderStatus.ConnectedToECU && !OBDReaderSimulator.Current.IsActive)
			{
				App.OBDReader.DebugWrite("\r\n[AvailableSensors: " + SharedSettings.Current.LastCarAvailableSensors + "]\r\n");
			}
		}

		// Token: 0x06003F1E RID: 16158 RVA: 0x0032F264 File Offset: 0x0032D464
		internal void ClearResources()
		{
			this.Unsubscribe();
			this.Values.Clear();
			this.Values = new SmartCollection<DoubleValueItem>();
		}

		// Token: 0x06003F1F RID: 16159 RVA: 0x0032F284 File Offset: 0x0032D484
		private static string GetAvailableSensorsString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			for (int i = 0; i < LiveDataPIDModel._PIDCollection.Count; i++)
			{
				if (LiveDataPIDModel._PIDCollection[i] != null && LiveDataPIDModel._PIDCollection[i].IsAvailable)
				{
					stringBuilder.Append(LiveDataPIDModel._PIDCollection[i].Id.ToString());
					stringBuilder.Append(";");
				}
			}
			return stringBuilder.ToString();
		}

		// Token: 0x06003F20 RID: 16160 RVA: 0x0032F2FC File Offset: 0x0032D4FC
		private static void PIDValueChangedFireRecordData(object sender, IPID e)
		{
			IPIDFloatValue ipidfloatValue = e as IPIDFloatValue;
			if (ipidfloatValue != null)
			{
				DataRecorderV2 recorder = App.OBDReader.CurrentCarData.Recorder;
				if (recorder == null)
				{
					return;
				}
				recorder.Record(ipidfloatValue, e.TimeStamp.TotalSeconds);
			}
		}

		// Token: 0x06003F21 RID: 16161 RVA: 0x0032F33C File Offset: 0x0032D53C
		public static void SetShouldHideForSelectedPids()
		{
			if (!SharedSettings.Current.AdsProductPurchased)
			{
				if (SharedSettings.Current.FreePeriodGoodConnections > 15)
				{
					foreach (PID pid in LiveDataPIDModel._PIDCollection)
					{
						pid.ShouldHideValue = false;
					}
					List<PID> list = LiveDataPIDModel._PIDCollection.Where((PID x) => x is CustomPID || x is CalculatedPIDV2).ToList<PID>();
					if (LiveDataPIDModel._PIDCollection.Count - list.Count > 2)
					{
						for (int i = 0; i < list.Count; i++)
						{
							list[i].ShouldHideValue = true;
						}
						return;
					}
					for (int j = list.Count / 2; j < list.Count; j++)
					{
						list[j].ShouldHideValue = true;
					}
					return;
				}
				else
				{
					using (IEnumerator<PID> enumerator = LiveDataPIDModel._PIDCollection.Where((PID x) => x is CustomPID || x is CalculatedPIDV2).GetEnumerator())
					{
						while (enumerator.MoveNext())
						{
							PID pid2 = enumerator.Current;
							pid2.ShouldHideValue = false;
						}
						return;
					}
				}
			}
			foreach (PID pid3 in LiveDataPIDModel._PIDCollection)
			{
				pid3.ShouldHideValue = false;
			}
		}

		// Token: 0x06003F22 RID: 16162 RVA: 0x0032F4C4 File Offset: 0x0032D6C4
		public static void GetSupportedPIDsTEST(OBDDataReader OBDDataReader)
		{
			IOrderedEnumerable<PID> orderedEnumerable = from x in App.OBDReader.CurrentCarData.LiveDataPIDs
				where x.IsAvailable && !(x is CalculatedPIDV2) && (x is IPIDFloatValue || x is PIDWithStringValue || x is PID0103_FuelSystemStatus || x is PID_Status)
				orderby x.Id
				select x;
			IEnumerable<PID> enumerable = from x in App.OBDReader.CurrentCarData.LiveDataPIDs
				where x.IsAvailable && x is CalculatedPIDV2
				orderby x.Name
				select x;
			LiveDataPIDModel._PIDCollection.Clear();
			LiveDataPIDModel._PIDCollection.Add(PID.Empty);
			foreach (PID pid in enumerable)
			{
				LiveDataPIDModel._PIDCollection.Add(pid);
			}
			if (SharedSettings.Current.UseOBD2)
			{
				using (IEnumerator<PID> enumerator = orderedEnumerable.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						PID pid2 = enumerator.Current;
						LiveDataPIDModel._PIDCollection.Add(pid2);
					}
					goto IL_0176;
				}
			}
			PID pid3 = App.OBDReader.CurrentCarData.LiveDataPIDs.FirstOrDefault((PID x) => x is PID_ATRV);
			if (pid3 != null)
			{
				LiveDataPIDModel._PIDCollection.Add(pid3);
			}
			IL_0176:
			foreach (CustomPID customPID in CustomPIDViewModel.CurrentProfile.PidCollection)
			{
				if (customPID.IsAvailable)
				{
					LiveDataPIDModel._PIDCollection.Add(customPID);
				}
			}
			foreach (CustomPID customPID2 in CustomPIDViewModel.CurrentCustom.PidCollection)
			{
				if (customPID2.IsAvailable)
				{
					LiveDataPIDModel._PIDCollection.Add(customPID2);
				}
			}
			LiveDataPIDModel.SetShouldHideForSelectedPids();
			foreach (PID pid4 in LiveDataPIDModel._PIDCollection.Where((PID x) => x is IPIDFloatValue))
			{
				pid4.ValueChanged -= new EventHandler<PID>(LiveDataPIDModel.PIDValueChangedFireRecordData);
				pid4.ValueChanged += new EventHandler<PID>(LiveDataPIDModel.PIDValueChangedFireRecordData);
			}
		}

		// Token: 0x06003F23 RID: 16163 RVA: 0x0032F78C File Offset: 0x0032D98C
		public static void AddFuelConsumptionRequests(List<OBDRequest> requests)
		{
			try
			{
				PID pid = LiveDataPIDModel._PIDCollection.FirstOrDefault((PID x) => x is PID_CalculatedAVGFuelConsumption);
				if (pid != null)
				{
					LiveDataPIDModel.GetRequests(pid, requests, null, "");
				}
			}
			catch
			{
			}
		}

		// Token: 0x06003F24 RID: 16164 RVA: 0x0032F7E8 File Offset: 0x0032D9E8
		public LiveDataPIDModel()
		{
			this.Values = new SmartCollection<DoubleValueItem>();
			this.Values.Add(new DoubleValueItem(double.NaN, default(TimeSpan)));
			this.SetMinMaxAvgOnlyVisibleArea = SharedSettings.Current.SetChartMinMaxOnlyVisibleArea;
			if (this.PIDCollection != null && this.PIDCollection.Any<PID>())
			{
				this.SelectedPID = this.PIDCollection.FirstOrDefault<PID>();
			}
		}

		// Token: 0x14000042 RID: 66
		// (add) Token: 0x06003F25 RID: 16165 RVA: 0x0032F8E4 File Offset: 0x0032DAE4
		// (remove) Token: 0x06003F26 RID: 16166 RVA: 0x0032F91C File Offset: 0x0032DB1C
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

		// Token: 0x06003F27 RID: 16167 RVA: 0x0032F954 File Offset: 0x0032DB54
		protected virtual void NotifyPropertyChanged(string PropertyName)
		{
			try
			{
				PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
				if (propertyChanged != null)
				{
					propertyChanged(this, new PropertyChangedEventArgs(PropertyName));
				}
			}
			catch (Exception)
			{
			}
		}

		// Token: 0x17001496 RID: 5270
		// (get) Token: 0x06003F28 RID: 16168 RVA: 0x0032F990 File Offset: 0x0032DB90
		// (set) Token: 0x06003F29 RID: 16169 RVA: 0x0032F998 File Offset: 0x0032DB98
		public LiveDataModes Mode
		{
			[CompilerGenerated]
			get
			{
				return this.<Mode>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<Mode>k__BackingField = value;
			}
		}

		// Token: 0x17001497 RID: 5271
		// (get) Token: 0x06003F2A RID: 16170 RVA: 0x0032F9A1 File Offset: 0x0032DBA1
		public ObservableCollection<PID> PIDCollection
		{
			get
			{
				return LiveDataPIDModel._PIDCollection;
			}
		}

		// Token: 0x17001498 RID: 5272
		// (get) Token: 0x06003F2B RID: 16171 RVA: 0x0032F9A8 File Offset: 0x0032DBA8
		public SharedSettings Settings
		{
			get
			{
				return SharedSettings.Current;
			}
		}

		// Token: 0x17001499 RID: 5273
		// (get) Token: 0x06003F2C RID: 16172 RVA: 0x0032F9AF File Offset: 0x0032DBAF
		// (set) Token: 0x06003F2D RID: 16173 RVA: 0x0032F9B7 File Offset: 0x0032DBB7
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

		// Token: 0x1700149A RID: 5274
		// (get) Token: 0x06003F2E RID: 16174 RVA: 0x0032F9C0 File Offset: 0x0032DBC0
		// (set) Token: 0x06003F2F RID: 16175 RVA: 0x0032F9C8 File Offset: 0x0032DBC8
		public SmartCollection<DoubleValueItem> Values
		{
			[CompilerGenerated]
			get
			{
				return this.<Values>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<Values>k__BackingField = value;
			}
		}

		// Token: 0x1700149B RID: 5275
		// (get) Token: 0x06003F30 RID: 16176 RVA: 0x0032F9D1 File Offset: 0x0032DBD1
		public bool IsFloatPID
		{
			get
			{
				return this.SelectedPID != null && this.SelectedPID is IPIDFloatValue && (!(this.SelectedPID is CustomPID) || !(this.SelectedPID as CustomPID).IsAction);
			}
		}

		// Token: 0x1700149C RID: 5276
		// (get) Token: 0x06003F31 RID: 16177 RVA: 0x0032FA0E File Offset: 0x0032DC0E
		public bool IsAction
		{
			get
			{
				return this.SelectedPID != null && (this.SelectedPID is CustomPID && (this.SelectedPID as CustomPID).IsAction);
			}
		}

		// Token: 0x1700149D RID: 5277
		// (get) Token: 0x06003F32 RID: 16178 RVA: 0x0032FA3C File Offset: 0x0032DC3C
		// (set) Token: 0x06003F33 RID: 16179 RVA: 0x0032FA44 File Offset: 0x0032DC44
		public double Minimum
		{
			get
			{
				return this._Minimum;
			}
			set
			{
				if (this._Minimum != value)
				{
					this._Minimum = value;
					this.NotifyPropertyChanged("Minimum");
				}
			}
		}

		// Token: 0x1700149E RID: 5278
		// (get) Token: 0x06003F34 RID: 16180 RVA: 0x0032FA61 File Offset: 0x0032DC61
		// (set) Token: 0x06003F35 RID: 16181 RVA: 0x0032FA69 File Offset: 0x0032DC69
		public double Maximum
		{
			get
			{
				return this._Maximum;
			}
			set
			{
				if (this._Maximum != value)
				{
					this._Maximum = value;
					this.NotifyPropertyChanged("Maximum");
				}
			}
		}

		// Token: 0x1700149F RID: 5279
		// (get) Token: 0x06003F36 RID: 16182 RVA: 0x0032FA86 File Offset: 0x0032DC86
		// (set) Token: 0x06003F37 RID: 16183 RVA: 0x0032FA8E File Offset: 0x0032DC8E
		public double Interval
		{
			get
			{
				return this._Interval;
			}
			set
			{
				this._Interval = value;
				this.NotifyPropertyChanged("Interval");
			}
		}

		// Token: 0x14000043 RID: 67
		// (add) Token: 0x06003F38 RID: 16184 RVA: 0x0032FAA4 File Offset: 0x0032DCA4
		// (remove) Token: 0x06003F39 RID: 16185 RVA: 0x0032FADC File Offset: 0x0032DCDC
		public event PIDChangedEvent PIDChanged
		{
			[CompilerGenerated]
			add
			{
				PIDChangedEvent pidchangedEvent = this.PIDChanged;
				PIDChangedEvent pidchangedEvent2;
				do
				{
					pidchangedEvent2 = pidchangedEvent;
					PIDChangedEvent pidchangedEvent3 = (PIDChangedEvent)Delegate.Combine(pidchangedEvent2, value);
					pidchangedEvent = Interlocked.CompareExchange<PIDChangedEvent>(ref this.PIDChanged, pidchangedEvent3, pidchangedEvent2);
				}
				while (pidchangedEvent != pidchangedEvent2);
			}
			[CompilerGenerated]
			remove
			{
				PIDChangedEvent pidchangedEvent = this.PIDChanged;
				PIDChangedEvent pidchangedEvent2;
				do
				{
					pidchangedEvent2 = pidchangedEvent;
					PIDChangedEvent pidchangedEvent3 = (PIDChangedEvent)Delegate.Remove(pidchangedEvent2, value);
					pidchangedEvent = Interlocked.CompareExchange<PIDChangedEvent>(ref this.PIDChanged, pidchangedEvent3, pidchangedEvent2);
				}
				while (pidchangedEvent != pidchangedEvent2);
			}
		}

		// Token: 0x170014A0 RID: 5280
		// (get) Token: 0x06003F3A RID: 16186 RVA: 0x0032FB11 File Offset: 0x0032DD11
		// (set) Token: 0x06003F3B RID: 16187 RVA: 0x0032FB4A File Offset: 0x0032DD4A
		public double FloatValue
		{
			get
			{
				if (this.SelectedPID != null && this.SelectedPID.ShouldHideValue && this.TextValue == LiveDataPIDModel.hideValueString)
				{
					return double.NaN;
				}
				return this._FloatValue;
			}
			set
			{
				this._FloatValue = value;
				this.NotifyPropertyChanged("FloatValue");
			}
		}

		// Token: 0x170014A1 RID: 5281
		// (get) Token: 0x06003F3C RID: 16188 RVA: 0x0032FB5E File Offset: 0x0032DD5E
		// (set) Token: 0x06003F3D RID: 16189 RVA: 0x0032FB66 File Offset: 0x0032DD66
		public string AverageTextValue
		{
			get
			{
				return this._AverageTextValue;
			}
			set
			{
				this._AverageTextValue = value;
				this.NotifyPropertyChanged("AverageTextValue");
			}
		}

		// Token: 0x170014A2 RID: 5282
		// (get) Token: 0x06003F3E RID: 16190 RVA: 0x0032FB7A File Offset: 0x0032DD7A
		// (set) Token: 0x06003F3F RID: 16191 RVA: 0x0032FB82 File Offset: 0x0032DD82
		public bool AverageVisible
		{
			get
			{
				return this._AverageVIsible;
			}
			set
			{
				this._AverageVIsible = value;
				this.NotifyPropertyChanged("AverageVisible");
			}
		}

		// Token: 0x170014A3 RID: 5283
		// (get) Token: 0x06003F40 RID: 16192 RVA: 0x0032FB96 File Offset: 0x0032DD96
		// (set) Token: 0x06003F41 RID: 16193 RVA: 0x0032FBA0 File Offset: 0x0032DDA0
		public string TextValue
		{
			get
			{
				return this._TextValue;
			}
			set
			{
				if (value != "" && this.SelectedPID != null && this.SelectedPID.ShouldHideValue)
				{
					if (SharedSettings.Current.FreePeriodGoodConnections > 25)
					{
						if (this._TextValue != LiveDataPIDModel.hideValueString)
						{
							this._TextValue = LiveDataPIDModel.hideValueString;
							this.NotifyPropertyChanged("TextValue");
						}
						this.NotifyPropertyChanged("Units");
						this.NotifyPropertyChanged("ChartVisible");
						return;
					}
					int num = SharedSettings.Current.FreePeriodGoodConnections - 15;
					if (num > 10)
					{
						if (this._TextValue != LiveDataPIDModel.hideValueString)
						{
							this._TextValue = LiveDataPIDModel.hideValueString;
							this.NotifyPropertyChanged("TextValue");
						}
						this.NotifyPropertyChanged("Units");
						this.NotifyPropertyChanged("ChartVisible");
						return;
					}
					if (num < 3)
					{
						num = 3;
					}
					if ((int)this.lastReadTime.TotalSeconds % 10 <= num)
					{
						if (this._TextValue != LiveDataPIDModel.hideValueString)
						{
							this._TextValue = LiveDataPIDModel.hideValueString;
							this.NotifyPropertyChanged("TextValue");
						}
						this.NotifyPropertyChanged("Units");
						this.NotifyPropertyChanged("ChartVisible");
						return;
					}
					this.NotifyPropertyChanged("Units");
					this.NotifyPropertyChanged("ChartVisible");
				}
				if (this._TextValue == value)
				{
					return;
				}
				this._TextValue = value;
				this.NotifyPropertyChanged("TextValue");
			}
		}

		// Token: 0x170014A4 RID: 5284
		// (get) Token: 0x06003F42 RID: 16194 RVA: 0x0032FD05 File Offset: 0x0032DF05
		// (set) Token: 0x06003F43 RID: 16195 RVA: 0x0032FD3A File Offset: 0x0032DF3A
		public string Units
		{
			get
			{
				if (this.SelectedPID != null && this.SelectedPID.ShouldHideValue && this.TextValue == LiveDataPIDModel.hideValueString)
				{
					return "";
				}
				return this._Units;
			}
			set
			{
				this._Units = value;
				this.NotifyPropertyChanged("Units");
			}
		}

		// Token: 0x170014A5 RID: 5285
		// (get) Token: 0x06003F44 RID: 16196 RVA: 0x0032FD4E File Offset: 0x0032DF4E
		// (set) Token: 0x06003F45 RID: 16197 RVA: 0x0032FD81 File Offset: 0x0032DF81
		public bool ChartVisible
		{
			get
			{
				if (this.SelectedPID != null && this.SelectedPID.ShouldHideValue)
				{
					return !(this.TextValue == LiveDataPIDModel.hideValueString);
				}
				return this._ChartVisible;
			}
			set
			{
				this._ChartVisible = value;
				this.NotifyPropertyChanged("ChartVisible");
			}
		}

		// Token: 0x170014A6 RID: 5286
		// (get) Token: 0x06003F46 RID: 16198 RVA: 0x0032FD95 File Offset: 0x0032DF95
		// (set) Token: 0x06003F47 RID: 16199 RVA: 0x0032FD9D File Offset: 0x0032DF9D
		public bool SetMinMaxAvgOnlyVisibleArea
		{
			get
			{
				return this._SetMinMaxAvgOnlyVisibleArea;
			}
			set
			{
				if (value != this._SetMinMaxAvgOnlyVisibleArea)
				{
					this._SetMinMaxAvgOnlyVisibleArea = value;
					this.NotifyPropertyChanged("SetMinMaxAvgOnlyVisibleArea");
				}
			}
		}

		// Token: 0x170014A7 RID: 5287
		// (get) Token: 0x06003F48 RID: 16200 RVA: 0x0032FDBA File Offset: 0x0032DFBA
		private TimeSpan TargetTimeShift
		{
			get
			{
				return TimeSpan.FromSeconds((double)SharedSettings.Current.LiveDataShowTime);
			}
		}

		// Token: 0x170014A8 RID: 5288
		// (get) Token: 0x06003F49 RID: 16201 RVA: 0x0032FDCC File Offset: 0x0032DFCC
		// (set) Token: 0x06003F4A RID: 16202 RVA: 0x0032FDD4 File Offset: 0x0032DFD4
		public virtual IPID SelectedPID
		{
			get
			{
				return this._SelectedPID;
			}
			set
			{
				this.ChangeCurrentPid(value, this._SelectedPID);
			}
		}

		// Token: 0x170014A9 RID: 5289
		// (get) Token: 0x06003F4B RID: 16203 RVA: 0x0032FDE3 File Offset: 0x0032DFE3
		// (set) Token: 0x06003F4C RID: 16204 RVA: 0x0032FDEB File Offset: 0x0032DFEB
		public long Ping
		{
			get
			{
				return this._Ping;
			}
			set
			{
				this._Ping = value;
				this.NotifyPropertyChanged("Ping");
			}
		}

		// Token: 0x170014AA RID: 5290
		// (get) Token: 0x06003F4D RID: 16205 RVA: 0x0032FDFF File Offset: 0x0032DFFF
		public bool ShowPing
		{
			get
			{
				return SharedSettings.Current.ShowPing;
			}
		}

		// Token: 0x170014AB RID: 5291
		// (get) Token: 0x06003F4E RID: 16206 RVA: 0x0032FE0B File Offset: 0x0032E00B
		// (set) Token: 0x06003F4F RID: 16207 RVA: 0x0032FE13 File Offset: 0x0032E013
		public int DoubleFormat
		{
			get
			{
				return this._DoubleFormat;
			}
			set
			{
				if (value != this._DoubleFormat)
				{
					this._DoubleFormat = value;
					this.NotifyPropertyChanged("DoubleFormat");
				}
			}
		}

		// Token: 0x170014AC RID: 5292
		// (get) Token: 0x06003F50 RID: 16208 RVA: 0x0032FE30 File Offset: 0x0032E030
		// (set) Token: 0x06003F51 RID: 16209 RVA: 0x0032FE38 File Offset: 0x0032E038
		public double MinimumAchieved
		{
			get
			{
				return this._MinimumAchieved;
			}
			set
			{
				if (value != this._MinimumAchieved)
				{
					this._MinimumAchieved = value;
					this.NotifyPropertyChanged("MinimumAchieved");
					this.NotifyPropertyChanged("MinimumAchievedText");
				}
			}
		}

		// Token: 0x170014AD RID: 5293
		// (get) Token: 0x06003F52 RID: 16210 RVA: 0x0032FE60 File Offset: 0x0032E060
		// (set) Token: 0x06003F53 RID: 16211 RVA: 0x0032FE68 File Offset: 0x0032E068
		public double MaximumAchieved
		{
			get
			{
				return this._MaximumAchieved;
			}
			set
			{
				if (value != this._MaximumAchieved)
				{
					this._MaximumAchieved = value;
					this.NotifyPropertyChanged("MaximumAchieved");
					this.NotifyPropertyChanged("MaximumAchievedText");
				}
			}
		}

		// Token: 0x170014AE RID: 5294
		// (get) Token: 0x06003F54 RID: 16212 RVA: 0x0032FE90 File Offset: 0x0032E090
		public string MinimumAchievedText
		{
			get
			{
				if (double.IsNaN(this._MinimumAchieved))
				{
					return "n/a";
				}
				return this._MinimumAchieved.ToString(StaticLists.DoubleFormats[this.DoubleFormat], CultureInfo.InvariantCulture);
			}
		}

		// Token: 0x170014AF RID: 5295
		// (get) Token: 0x06003F55 RID: 16213 RVA: 0x0032FEC5 File Offset: 0x0032E0C5
		public string MaximumAchievedText
		{
			get
			{
				if (double.IsNaN(this._MaximumAchieved))
				{
					return "n/a";
				}
				return this._MaximumAchieved.ToString(StaticLists.DoubleFormats[this.DoubleFormat], CultureInfo.InvariantCulture);
			}
		}

		// Token: 0x06003F56 RID: 16214 RVA: 0x0032FEFC File Offset: 0x0032E0FC
		public void SelectedPID_ValueChanged(object sender, PID e)
		{
			if (e == this.SelectedPID)
			{
				if (this.lastReadTime == e.TimeStamp)
				{
					return;
				}
				if (e != null)
				{
					Device.BeginInvokeOnMainThread(delegate
					{
						this.Calculate(e);
					});
				}
			}
		}

		// Token: 0x06003F57 RID: 16215 RVA: 0x0032FF60 File Offset: 0x0032E160
		private void Calculate(PID e)
		{
			if (!(this.lastReadTime == e.TimeStamp))
			{
				if (this.ShowPing)
				{
					this.Ping = (e.TimeStamp.Ticks - this.lastReadTime.Ticks) / 10000L;
				}
				this.lastReadTime = e.TimeStamp;
				if (this.SelectedPID != null)
				{
					IPIDFloatValue ipidfloatValue = this.SelectedPID as IPIDFloatValue;
					if (ipidfloatValue != null)
					{
						DoubleValueItem doubleValueItem;
						if (this.CustomUnit != UnitsHelper.Units.None)
						{
							double num = UnitsHelper.Convert(ipidfloatValue.Value, ipidfloatValue.Units, this.CustomUnit);
							doubleValueItem = new DoubleValueItem(num, e.TimeStamp);
						}
						else
						{
							doubleValueItem = new DoubleValueItem(UnitsHelper.GetValue(ipidfloatValue.Value, ipidfloatValue.Units), e.TimeStamp);
						}
						if (double.IsFinite(doubleValueItem.Value) && (!e.ShouldHideValue || !(this.TextValue == LiveDataPIDModel.hideValueString)))
						{
							this.Values.AddWithoutNotification(doubleValueItem);
							this.CutValuesToVisibleArea(doubleValueItem.TimeAdded);
							this.SetMinMaxAvg(doubleValueItem);
							this.Values.NotifyCollectionReset();
						}
						this.FloatValue = doubleValueItem.Value;
						if (double.IsInfinity(doubleValueItem.Value))
						{
							this.TextValue = "∞";
							return;
						}
						if (double.IsNaN(doubleValueItem.Value))
						{
							this._TextValue = "n/a";
							return;
						}
						string textValueVariantOrNull = ipidfloatValue.GetTextValueVariantOrNull(doubleValueItem.Value);
						if (textValueVariantOrNull != null)
						{
							this.TextValue = textValueVariantOrNull;
							return;
						}
						this.TextValue = doubleValueItem.Value.ToString(StaticLists.DoubleFormats[this.DoubleFormat], CultureInfo.InvariantCulture);
						return;
					}
				}
				if (this.SelectedPID is PIDWithStringValue)
				{
					PIDWithStringValue pidwithStringValue = this.SelectedPID as PIDWithStringValue;
					this.TextValue = pidwithStringValue.Value;
					return;
				}
				if (this.SelectedPID is PID_Status)
				{
					string text = (this.SelectedPID as PID_Status).Value.ToString();
					this.TextValue = text;
					return;
				}
				if (this.SelectedPID is PID0103_FuelSystemStatus)
				{
					PID0103_FuelSystemStatus pid0103_FuelSystemStatus = this.SelectedPID as PID0103_FuelSystemStatus;
					if (pid0103_FuelSystemStatus.Value[1] == PID0103_FuelSystemStatus.FuelSystemStatuses.None)
					{
						string fullCaption = PID0103_FuelSystemStatus.GetFullCaption(pid0103_FuelSystemStatus.Value[0]);
						this.TextValue = fullCaption;
						return;
					}
					string text2 = "ECU #1: " + PID0103_FuelSystemStatus.GetFullCaption(pid0103_FuelSystemStatus.Value[0]) + "\nECU #2: " + PID0103_FuelSystemStatus.GetFullCaption(pid0103_FuelSystemStatus.Value[1]);
					this.TextValue = text2;
				}
			}
		}

		// Token: 0x170014B0 RID: 5296
		// (get) Token: 0x06003F58 RID: 16216 RVA: 0x003301CE File Offset: 0x0032E3CE
		// (set) Token: 0x06003F59 RID: 16217 RVA: 0x003301D6 File Offset: 0x0032E3D6
		public int LiveDataShowTime
		{
			get
			{
				return this._LiveDataShowTime;
			}
			set
			{
				this._LiveDataShowTime = value;
				PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
				if (propertyChanged == null)
				{
					return;
				}
				propertyChanged(this, new PropertyChangedEventArgs("LiveDataShowTime"));
			}
		}

		// Token: 0x06003F5A RID: 16218 RVA: 0x003301FC File Offset: 0x0032E3FC
		private void CutValuesToVisibleArea(TimeSpan lastTime)
		{
			TimeSpan timeSpan = lastTime.Subtract(TimeSpan.FromSeconds((double)this.LiveDataShowTime));
			int num = -1;
			for (int i = 0; i < this.Values.Count; i++)
			{
				if (this.Values[i].TimeAdded >= timeSpan)
				{
					num = i;
					break;
				}
			}
			if (num > 0)
			{
				List<DoubleValueItem> list = ListPool<DoubleValueItem>.Rent();
				list.AddRange(this.Values.Skip(num));
				this.Values.ResetWithoutNotification(list);
				ListPool<DoubleValueItem>.Return(list);
			}
		}

		// Token: 0x06003F5B RID: 16219 RVA: 0x00330288 File Offset: 0x0032E488
		private void SetMinMaxAvg(DoubleValueItem valItem)
		{
			double value = valItem.Value;
			if (!double.IsFinite(value))
			{
				return;
			}
			TimeSpan timeSpan = valItem.TimeAdded - new TimeSpan(0, 0, this.LiveDataShowTime);
			this.totalSummForAvgCalculations += value;
			this.totalCounterForValues += 1L;
			if (SharedSettings.Current.SetChartMinMaxOnlyVisibleArea && (this.Mode == LiveDataModes.LiveDataChart || this.Mode == LiveDataModes.DashboardChart))
			{
				double num = double.NaN;
				double num2 = double.NaN;
				int num3 = 0;
				double num4 = 0.0;
				for (int i = this.Values.Count - 1; i >= 0; i--)
				{
					DoubleValueItem doubleValueItem = this.Values[i];
					if (doubleValueItem.TimeAdded < timeSpan)
					{
						break;
					}
					if (double.IsFinite(doubleValueItem.Value))
					{
						num3++;
						num4 += doubleValueItem.Value;
						if (double.IsNaN(num) || doubleValueItem.Value < num)
						{
							num = doubleValueItem.Value;
						}
						if (double.IsNaN(num2) || doubleValueItem.Value > num2)
						{
							num2 = doubleValueItem.Value;
						}
					}
				}
				this.MinimumAchieved = num;
				this.MaximumAchieved = num2;
				this.AverageTextValue = (num4 / (double)num3).ToString(StaticLists.DoubleFormats[this.DoubleFormat], CultureInfo.InvariantCulture);
			}
			else
			{
				if (double.IsNaN(this._MinimumAchieved) || value < this._MinimumAchieved)
				{
					this.MinimumAchieved = value;
				}
				if (double.IsNaN(this._MaximumAchieved) || value > this._MaximumAchieved)
				{
					this.MaximumAchieved = value;
				}
				this.AverageTextValue = (this.totalSummForAvgCalculations / (double)this.totalCounterForValues).ToString(StaticLists.DoubleFormats[this.DoubleFormat], CultureInfo.InvariantCulture);
			}
			double num5;
			double num6;
			double num7;
			this.GetChartMinMaxInterval(this._MinimumAchieved, this._MaximumAchieved, out num5, out num6, out num7);
			this.Minimum = num5;
			this.Maximum = num6;
			this.Interval = num7;
		}

		// Token: 0x06003F5C RID: 16220 RVA: 0x00330494 File Offset: 0x0032E694
		private void ChangeCurrentPid(IPID NewPid, IPID OldPid)
		{
			if (OldPid != null)
			{
				OldPid.ValueChanged -= this.SelectedPID_ValueChanged;
				this.RecordNaNOnPIDUnsubscribe(OldPid);
			}
			if (NewPid == null)
			{
				this._SelectedPID = PID.Empty;
			}
			else
			{
				this._SelectedPID = NewPid;
			}
			this.TextValue = "";
			this.Units = "";
			this.Minimum = 0.0;
			this.Maximum = 1.0;
			if (NewPid is IPIDFloatValue)
			{
				if (NewPid is CustomPID && (NewPid as CustomPID).IsAction)
				{
					this.AverageVisible = false;
					this.ChartVisible = false;
					this.TextValue = "";
				}
				else
				{
					if (SharedSettings.Current.ChartShowAverageValue)
					{
						this.AverageVisible = true;
					}
					else
					{
						this.AverageVisible = false;
					}
					IPIDFloatValue ipidfloatValue = NewPid as IPIDFloatValue;
					this.CustomUnit = NewPid.CustomUnit;
					if (this.CustomUnit == UnitsHelper.Units.None)
					{
						this.Units = UnitsHelper.GetCaption(ipidfloatValue.Units);
					}
					else
					{
						this.Units = UnitsHelper.GetCaptionInvariant(this.CustomUnit);
					}
					this.ChartVisible = true;
				}
			}
			else
			{
				this.AverageVisible = false;
				this.ChartVisible = false;
				this.CustomUnit = UnitsHelper.Units.None;
			}
			this.Subscribe();
			this.NotifyPropertyChanged("ChartVisible");
			this.NotifyPropertyChanged("SelectedPID");
			this.NotifyPropertyChanged("IsFloatPID");
			this.NotifyPropertyChanged("ShowMinMaxValues");
			this.NotifyPropertyChanged("ChartShowAverageValue");
			this.Values.Clear();
			this.totalSummForAvgCalculations = 0.0;
			this.OnPIDChanged();
		}

		// Token: 0x06003F5D RID: 16221 RVA: 0x00330619 File Offset: 0x0032E819
		public virtual void Unsubscribe()
		{
			if (this.SelectedPID != null)
			{
				this.SelectedPID.ValueChanged -= this.SelectedPID_ValueChanged;
				this.RecordNaNOnPIDUnsubscribe(this.SelectedPID);
			}
		}

		// Token: 0x06003F5E RID: 16222 RVA: 0x00330648 File Offset: 0x0032E848
		private void RecordNaNOnPIDUnsubscribe(IPID pid)
		{
			IPIDFloatValue ipidfloatValue = pid as IPIDFloatValue;
			if (ipidfloatValue != null && this.Values.Count > 0 && double.IsFinite(this.Values[this.Values.Count - 1].Value))
			{
				OBDDataReader obdreader = App.OBDReader;
				if (obdreader == null)
				{
					return;
				}
				CarData currentCarData = obdreader.CurrentCarData;
				if (currentCarData == null)
				{
					return;
				}
				DataRecorderV2 recorder = currentCarData.Recorder;
				if (recorder == null)
				{
					return;
				}
				recorder.RecordNaN(ipidfloatValue, this.SelectedPID.TimeStamp.TotalSeconds);
			}
		}

		// Token: 0x06003F5F RID: 16223 RVA: 0x003306CC File Offset: 0x0032E8CC
		public virtual void Subscribe()
		{
			if (this.SelectedPID != null && this.SelectedPID != PID.Empty)
			{
				IPID selectedPID = this.SelectedPID;
				lock (selectedPID)
				{
					this.SelectedPID.ValueChanged -= this.SelectedPID_ValueChanged;
					this.SelectedPID.ValueChanged += this.SelectedPID_ValueChanged;
				}
			}
		}

		// Token: 0x170014B1 RID: 5297
		// (get) Token: 0x06003F60 RID: 16224 RVA: 0x0033074C File Offset: 0x0032E94C
		public ICommand Action
		{
			get
			{
				return new Command(delegate
				{
					if (this.SelectedPID != null)
					{
						IPID selectedPID = this.SelectedPID;
						CustomPID cpid = selectedPID as CustomPID;
						if (cpid != null && cpid.IsAction)
						{
							if (cpid.Command.StartsWith("INTERNAL:"))
							{
								if (cpid.Command == "INTERNAL:RESETFUELDISTANCESPEED")
								{
									App.OBDReader.CurrentCarData.ResetFuelDistanceSpeed();
									return;
								}
							}
							else
							{
								OBDRequest[] array = (from cmd in cpid.Command.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries)
									select new OBDRequest(cmd, cpid.Header, cpid.BeforeCommand, cpid.AfterCommand, false, new List<PID> { cpid })).ToArray<OBDRequest>();
								if (array != null && array.Length != 0)
								{
									array[array.Length - 1].ResponseReceived += this.Action_ResponseReceived;
									foreach (OBDRequest obdrequest in array)
									{
										App.OBDReader.AddRequestToQueue(obdrequest);
									}
								}
							}
						}
					}
				});
			}
		}

		// Token: 0x06003F61 RID: 16225 RVA: 0x0033075F File Offset: 0x0032E95F
		private void Action_ResponseReceived(OBDRequest request, string data)
		{
			request.ResponseReceived -= this.Action_ResponseReceived;
			MainThreadHelper.InvokeOnMainThread(delegate
			{
				Page currentPage = App.GetCurrentPage();
				if (currentPage != null)
				{
					VisualElementExtension.DisplayToastAsync(currentPage, "Request:\n" + request.Command + "\nResponse:\n" + data, 5000);
				}
			});
		}

		// Token: 0x06003F62 RID: 16226 RVA: 0x0033079B File Offset: 0x0032E99B
		public virtual void GetRequests(List<OBDRequest> requestsQueue, string addKey = null, string addValue = "")
		{
			if (this.SelectedPID != null && this.SelectedPID != PID.Empty)
			{
				LiveDataPIDModel.GetRequests(this.SelectedPID, requestsQueue, addKey, addValue);
			}
		}

		// Token: 0x06003F63 RID: 16227 RVA: 0x003307C0 File Offset: 0x0032E9C0
		public static void GetRequests(IPID pid, List<OBDRequest> requestsQueue, string addKey = null, string addValue = "")
		{
			if (pid == null || requestsQueue == null)
			{
				return;
			}
			if (pid is SensorPID)
			{
				(pid as SensorPID).Start();
				return;
			}
			if (pid is CalculatedPIDV2)
			{
				using (IEnumerator<IPID> enumerator = (pid as CalculatedPIDV2).RequiredPIDs.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						IPID ipid = enumerator.Current;
						LiveDataPIDModel.GetRequests(ipid, requestsQueue, null, "");
					}
					return;
				}
			}
			CustomPID customPID = pid as CustomPID;
			if (customPID != null)
			{
				if (customPID.IsAction)
				{
					return;
				}
				OBDRequest req2 = null;
				if (pid.Name == "CARSCANNERGENERATEDPID")
				{
					req2 = new OBDRequest(customPID.Command, customPID.Header, customPID.BeforeCommand, customPID.AfterCommand, true, pid);
					req2.SkipCyclesTarget = pid.SkipCycles;
					OBDRequest obdrequest = requestsQueue.FirstOrDefault((OBDRequest x) => x.Equals(req2));
					if (obdrequest == null)
					{
						if (addKey != null)
						{
							req2.Keys[addKey] = addValue;
						}
						requestsQueue.Add(req2);
					}
					else if (obdrequest.SkipCyclesTarget > req2.SkipCyclesTarget)
					{
						obdrequest.SkipCyclesTarget = req2.SkipCyclesTarget;
					}
				}
				else if (!string.IsNullOrEmpty(customPID.Command))
				{
					req2 = new OBDRequest(customPID.Command, customPID.Header, customPID.BeforeCommand, customPID.AfterCommand, true);
					req2.SkipCyclesTarget = pid.SkipCycles;
					req2.CheckLength = SharedSettings.Current.ForceUseManualFlowControlWhileReadingData;
					OBDRequest obdrequest2 = requestsQueue.FirstOrDefault((OBDRequest x) => x.Equals(req2));
					if (obdrequest2 == null)
					{
						if (addKey != null)
						{
							req2.Keys[addKey] = addValue;
						}
						requestsQueue.Add(req2);
					}
					else if (obdrequest2.SkipCyclesTarget > req2.SkipCyclesTarget)
					{
						obdrequest2.SkipCyclesTarget = req2.SkipCyclesTarget;
					}
				}
				using (IEnumerator<IPID> enumerator = customPID.RequiredPIDs.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						IPID ipid2 = enumerator.Current;
						LiveDataPIDModel.GetRequests(ipid2, requestsQueue, null, "");
					}
					return;
				}
			}
			if (!string.IsNullOrEmpty(pid.Command))
			{
				OBDRequest req = new OBDRequest(pid.Command, pid.Header, true);
				req.SkipCyclesTarget = pid.SkipCycles;
				req.CheckLength = SharedSettings.Current.ForceUseManualFlowControlWhileReadingData;
				OBDRequest obdrequest3 = requestsQueue.FirstOrDefault((OBDRequest x) => x.Equals(req));
				if (obdrequest3 == null)
				{
					if (addKey != null)
					{
						req.Keys[addKey] = addValue;
					}
					requestsQueue.Add(req);
					return;
				}
				if (obdrequest3.SkipCyclesTarget > req.SkipCyclesTarget)
				{
					obdrequest3.SkipCyclesTarget = req.SkipCyclesTarget;
				}
			}
		}

		// Token: 0x06003F64 RID: 16228 RVA: 0x00330AD0 File Offset: 0x0032ECD0
		public static bool CheckIfRequestInQueue(OBDRequest request, IEnumerable<OBDRequest> queue)
		{
			return queue.Any((OBDRequest x) => x.Equals(request));
		}

		// Token: 0x06003F65 RID: 16229 RVA: 0x00330AFC File Offset: 0x0032ECFC
		private IEnumerable<DoubleValueItem> GetVisibleValues(TimeSpan TargetTime)
		{
			int num = ((this.Values.Count == 0) ? 0 : (this.Values.Count - 1));
			int num2 = this.Values.Count - 1;
			while (num2 > -1 && this.Values[num2].TimeAdded > TargetTime)
			{
				num = num2;
				num2--;
			}
			return this.Values.Skip(num);
		}

		// Token: 0x06003F66 RID: 16230 RVA: 0x00330B6C File Offset: 0x0032ED6C
		private int GetFirstVisibleValueIndex(TimeSpan TargetTime)
		{
			int num = this.Values.Count;
			for (int i = this.Values.Count - 1; i >= 0; i--)
			{
				num = i;
				if (this.Values[i].TimeAdded < TargetTime)
				{
					break;
				}
			}
			return num;
		}

		// Token: 0x06003F67 RID: 16231 RVA: 0x00330BBC File Offset: 0x0032EDBC
		public virtual void Dispose()
		{
			if (this.SelectedPID != null)
			{
				this.SelectedPID.ValueChanged -= this.SelectedPID_ValueChanged;
			}
		}

		// Token: 0x06003F68 RID: 16232 RVA: 0x00330BE0 File Offset: 0x0032EDE0
		protected void OnPIDChanged()
		{
			PIDChangedEvent pidchanged = this.PIDChanged;
			if (pidchanged != null)
			{
				pidchanged(this.SelectedPID, this);
			}
			PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
			if (propertyChanged != null)
			{
				propertyChanged(this, new PropertyChangedEventArgs("IsFloatPID"));
				propertyChanged(this, new PropertyChangedEventArgs("IsAction"));
			}
		}

		// Token: 0x06003F69 RID: 16233 RVA: 0x00330C30 File Offset: 0x0032EE30
		private double GetInterval(double minimum, double maximum)
		{
			if (minimum == maximum)
			{
				return 1.0;
			}
			double num = Math.Abs(minimum - maximum);
			double num2;
			if (0.0 < num && num <= 1.0)
			{
				num2 = 0.1;
			}
			else if (1.0 < num && num <= 2.0)
			{
				num2 = 0.2;
			}
			else if (2.0 < num && num <= 10.0)
			{
				num2 = 1.0;
			}
			else if (10.0 < num && num <= 20.0)
			{
				num2 = 2.0;
			}
			else if (20.0 < num && num <= 50.0)
			{
				num2 = 5.0;
			}
			else if (50.0 < num && num <= 100.0)
			{
				num2 = 10.0;
			}
			else if (100.0 < num && num <= 220.0)
			{
				num2 = 20.0;
			}
			else if (220.0 < num && num <= 500.0)
			{
				num2 = 25.0;
			}
			else if (500.0 < num && num <= 1000.0)
			{
				num2 = 100.0;
			}
			else if (1000.0 < num && num <= 2000.0)
			{
				num2 = 200.0;
			}
			else if (2000.0 < num && num <= 10000.0)
			{
				num2 = 1000.0;
			}
			else if (10000.0 < num && num <= 20000.0)
			{
				num2 = 2000.0;
			}
			else if (20000.0 < num && num <= 100000.0)
			{
				num2 = 10000.0;
			}
			else
			{
				num2 = num / 10.0;
			}
			return num2;
		}

		// Token: 0x06003F6A RID: 16234 RVA: 0x00330E50 File Offset: 0x0032F050
		private void GetChartMinMaxInterval(double actual_min, double actual_max, out double chart_min, out double chart_max, out double interval)
		{
			interval = this.GetInterval(actual_min, actual_max);
			if (actual_min == actual_max)
			{
				chart_max = Math.Ceiling(actual_max + interval);
				chart_min = Math.Floor(actual_min - interval);
				return;
			}
			chart_min = Math.Floor(actual_min / interval) * interval;
			chart_max = Math.Ceiling(actual_max / interval) * interval;
		}

		// Token: 0x170014B2 RID: 5298
		// (get) Token: 0x06003F6B RID: 16235 RVA: 0x00330EA7 File Offset: 0x0032F0A7
		public bool ShowMinMaxValues
		{
			get
			{
				return this.IsFloatPID && this.Settings.ShowMinMaxValues;
			}
		}

		// Token: 0x170014B3 RID: 5299
		// (get) Token: 0x06003F6C RID: 16236 RVA: 0x00330EBE File Offset: 0x0032F0BE
		public bool ChartShowAverageValue
		{
			get
			{
				return this.IsFloatPID && this.Settings.ChartShowAverageValue;
			}
		}

		// Token: 0x06003F6D RID: 16237 RVA: 0x00330ED5 File Offset: 0x0032F0D5
		// Note: this type is marked as 'beforefieldinit'.
		static LiveDataPIDModel()
		{
		}

		// Token: 0x06003F6E RID: 16238 RVA: 0x00330EF0 File Offset: 0x0032F0F0
		[CompilerGenerated]
		private void <get_Action>b__124_0()
		{
			if (this.SelectedPID != null)
			{
				IPID selectedPID = this.SelectedPID;
				CustomPID cpid = selectedPID as CustomPID;
				if (cpid != null && cpid.IsAction)
				{
					if (cpid.Command.StartsWith("INTERNAL:"))
					{
						if (cpid.Command == "INTERNAL:RESETFUELDISTANCESPEED")
						{
							App.OBDReader.CurrentCarData.ResetFuelDistanceSpeed();
							return;
						}
					}
					else
					{
						OBDRequest[] array = (from cmd in cpid.Command.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries)
							select new OBDRequest(cmd, cpid.Header, cpid.BeforeCommand, cpid.AfterCommand, false, new List<PID> { cpid })).ToArray<OBDRequest>();
						if (array != null && array.Length != 0)
						{
							array[array.Length - 1].ResponseReceived += this.Action_ResponseReceived;
							foreach (OBDRequest obdrequest in array)
							{
								App.OBDReader.AddRequestToQueue(obdrequest);
							}
						}
					}
				}
			}
		}

		// Token: 0x040026AE RID: 9902
		public static ObservableCollection<PID> _PIDCollection = new ObservableCollection<PID>();

		// Token: 0x040026AF RID: 9903
		[CompilerGenerated]
		private PropertyChangedEventHandler PropertyChanged;

		// Token: 0x040026B0 RID: 9904
		[CompilerGenerated]
		private LiveDataModes <Mode>k__BackingField;

		// Token: 0x040026B1 RID: 9905
		[CompilerGenerated]
		private string <Name>k__BackingField;

		// Token: 0x040026B2 RID: 9906
		[CompilerGenerated]
		private SmartCollection<DoubleValueItem> <Values>k__BackingField;

		// Token: 0x040026B3 RID: 9907
		private double _Minimum;

		// Token: 0x040026B4 RID: 9908
		private double _Maximum;

		// Token: 0x040026B5 RID: 9909
		private double _Interval = 1.0;

		// Token: 0x040026B6 RID: 9910
		[CompilerGenerated]
		private PIDChangedEvent PIDChanged;

		// Token: 0x040026B7 RID: 9911
		private double _FloatValue;

		// Token: 0x040026B8 RID: 9912
		private string _AverageTextValue = "";

		// Token: 0x040026B9 RID: 9913
		private bool _AverageVIsible = SharedSettings.Current.ChartShowAverageValue;

		// Token: 0x040026BA RID: 9914
		private static string hideValueString = Translate.GetString("ios_TrialExpired");

		// Token: 0x040026BB RID: 9915
		private string _TextValue = "";

		// Token: 0x040026BC RID: 9916
		private string _Units = "";

		// Token: 0x040026BD RID: 9917
		private UnitsHelper.Units CustomUnit;

		// Token: 0x040026BE RID: 9918
		private bool _ChartVisible = true;

		// Token: 0x040026BF RID: 9919
		private bool _SetMinMaxAvgOnlyVisibleArea = true;

		// Token: 0x040026C0 RID: 9920
		private ObservableCollection<LiveDataPIDModel> OtherModels;

		// Token: 0x040026C1 RID: 9921
		private long _Ping;

		// Token: 0x040026C2 RID: 9922
		private TimeSpan lastReadTime;

		// Token: 0x040026C3 RID: 9923
		private int _DoubleFormat = 2;

		// Token: 0x040026C4 RID: 9924
		private double _MinimumAchieved = double.NaN;

		// Token: 0x040026C5 RID: 9925
		private double _MaximumAchieved = double.NaN;

		// Token: 0x040026C6 RID: 9926
		private double totalSummForAvgCalculations;

		// Token: 0x040026C7 RID: 9927
		private long totalCounterForValues;

		// Token: 0x040026C8 RID: 9928
		private int _LiveDataShowTime = SharedSettings.Current.LiveDataShowTime;

		// Token: 0x040026C9 RID: 9929
		private IPID _SelectedPID;

		// Token: 0x02000743 RID: 1859
		[CompilerGenerated]
		[Serializable]
		private sealed class <>c
		{
			// Token: 0x06003F6F RID: 16239 RVA: 0x00330FF5 File Offset: 0x0032F1F5
			// Note: this type is marked as 'beforefieldinit'.
			static <>c()
			{
			}

			// Token: 0x06003F70 RID: 16240 RVA: 0x00002050 File Offset: 0x00000250
			public <>c()
			{
			}

			// Token: 0x06003F71 RID: 16241 RVA: 0x00331001 File Offset: 0x0032F201
			internal bool <UpdatePIDCollection>b__2_0(PID x)
			{
				return x.IsAvailable && !(x is CalculatedPIDV2) && (x is IPIDFloatValue || x is PIDWithStringValue || x is PID0103_FuelSystemStatus || x is PID_Status);
			}

			// Token: 0x06003F72 RID: 16242 RVA: 0x00331038 File Offset: 0x0032F238
			internal bool <UpdatePIDCollection>b__2_1(PID x)
			{
				return x is PID_ATRV;
			}

			// Token: 0x06003F73 RID: 16243 RVA: 0x00331043 File Offset: 0x0032F243
			internal bool <UpdatePIDCollection>b__2_2(PID x)
			{
				return !(x is InternalActionPID);
			}

			// Token: 0x06003F74 RID: 16244 RVA: 0x001C6E3D File Offset: 0x001C503D
			internal string <UpdatePIDCollection>b__2_3(PID x)
			{
				return x.Command;
			}

			// Token: 0x06003F75 RID: 16245 RVA: 0x00331051 File Offset: 0x0032F251
			internal bool <UpdatePIDCollection>b__2_4(PID x)
			{
				return x.IsAvailable && x is CalculatedPIDV2;
			}

			// Token: 0x06003F76 RID: 16246 RVA: 0x002A2115 File Offset: 0x002A0315
			internal string <UpdatePIDCollection>b__2_5(PID x)
			{
				return x.Name;
			}

			// Token: 0x06003F77 RID: 16247 RVA: 0x00066340 File Offset: 0x00064540
			internal bool <UpdatePIDCollection>b__2_6(PID x)
			{
				return x is IPIDFloatValue;
			}

			// Token: 0x06003F78 RID: 16248 RVA: 0x00331066 File Offset: 0x0032F266
			internal bool <SetShouldHideForSelectedPids>b__6_0(PID x)
			{
				return x is CustomPID || x is CalculatedPIDV2;
			}

			// Token: 0x06003F79 RID: 16249 RVA: 0x00331066 File Offset: 0x0032F266
			internal bool <SetShouldHideForSelectedPids>b__6_1(PID x)
			{
				return x is CustomPID || x is CalculatedPIDV2;
			}

			// Token: 0x06003F7A RID: 16250 RVA: 0x00331001 File Offset: 0x0032F201
			internal bool <GetSupportedPIDsTEST>b__7_0(PID x)
			{
				return x.IsAvailable && !(x is CalculatedPIDV2) && (x is IPIDFloatValue || x is PIDWithStringValue || x is PID0103_FuelSystemStatus || x is PID_Status);
			}

			// Token: 0x06003F7B RID: 16251 RVA: 0x002A211D File Offset: 0x002A031D
			internal int <GetSupportedPIDsTEST>b__7_1(PID x)
			{
				return x.Id;
			}

			// Token: 0x06003F7C RID: 16252 RVA: 0x00331051 File Offset: 0x0032F251
			internal bool <GetSupportedPIDsTEST>b__7_2(PID x)
			{
				return x.IsAvailable && x is CalculatedPIDV2;
			}

			// Token: 0x06003F7D RID: 16253 RVA: 0x002A2115 File Offset: 0x002A0315
			internal string <GetSupportedPIDsTEST>b__7_3(PID x)
			{
				return x.Name;
			}

			// Token: 0x06003F7E RID: 16254 RVA: 0x00331038 File Offset: 0x0032F238
			internal bool <GetSupportedPIDsTEST>b__7_4(PID x)
			{
				return x is PID_ATRV;
			}

			// Token: 0x06003F7F RID: 16255 RVA: 0x00066340 File Offset: 0x00064540
			internal bool <GetSupportedPIDsTEST>b__7_5(PID x)
			{
				return x is IPIDFloatValue;
			}

			// Token: 0x06003F80 RID: 16256 RVA: 0x000AC002 File Offset: 0x000AA202
			internal bool <AddFuelConsumptionRequests>b__8_0(PID x)
			{
				return x is PID_CalculatedAVGFuelConsumption;
			}

			// Token: 0x040026CA RID: 9930
			public static readonly LiveDataPIDModel.<>c <>9 = new LiveDataPIDModel.<>c();

			// Token: 0x040026CB RID: 9931
			public static Func<PID, bool> <>9__2_0;

			// Token: 0x040026CC RID: 9932
			public static Func<PID, bool> <>9__2_1;

			// Token: 0x040026CD RID: 9933
			public static Func<PID, bool> <>9__2_2;

			// Token: 0x040026CE RID: 9934
			public static Func<PID, string> <>9__2_3;

			// Token: 0x040026CF RID: 9935
			public static Func<PID, bool> <>9__2_4;

			// Token: 0x040026D0 RID: 9936
			public static Func<PID, string> <>9__2_5;

			// Token: 0x040026D1 RID: 9937
			public static Func<PID, bool> <>9__2_6;

			// Token: 0x040026D2 RID: 9938
			public static Func<PID, bool> <>9__6_0;

			// Token: 0x040026D3 RID: 9939
			public static Func<PID, bool> <>9__6_1;

			// Token: 0x040026D4 RID: 9940
			public static Func<PID, bool> <>9__7_0;

			// Token: 0x040026D5 RID: 9941
			public static Func<PID, int> <>9__7_1;

			// Token: 0x040026D6 RID: 9942
			public static Func<PID, bool> <>9__7_2;

			// Token: 0x040026D7 RID: 9943
			public static Func<PID, string> <>9__7_3;

			// Token: 0x040026D8 RID: 9944
			public static Func<PID, bool> <>9__7_4;

			// Token: 0x040026D9 RID: 9945
			public static Func<PID, bool> <>9__7_5;

			// Token: 0x040026DA RID: 9946
			public static Func<PID, bool> <>9__8_0;
		}

		// Token: 0x02000744 RID: 1860
		[CompilerGenerated]
		private sealed class <>c__DisplayClass108_0
		{
			// Token: 0x06003F81 RID: 16257 RVA: 0x00002050 File Offset: 0x00000250
			public <>c__DisplayClass108_0()
			{
			}

			// Token: 0x06003F82 RID: 16258 RVA: 0x0033107B File Offset: 0x0032F27B
			internal void <SelectedPID_ValueChanged>b__0()
			{
				this.<>4__this.Calculate(this.e);
			}

			// Token: 0x040026DB RID: 9947
			public LiveDataPIDModel <>4__this;

			// Token: 0x040026DC RID: 9948
			public PID e;
		}

		// Token: 0x02000745 RID: 1861
		[CompilerGenerated]
		private sealed class <>c__DisplayClass124_0
		{
			// Token: 0x06003F83 RID: 16259 RVA: 0x00002050 File Offset: 0x00000250
			public <>c__DisplayClass124_0()
			{
			}

			// Token: 0x06003F84 RID: 16260 RVA: 0x0033108E File Offset: 0x0032F28E
			internal OBDRequest <get_Action>b__1(string cmd)
			{
				return new OBDRequest(cmd, this.cpid.Header, this.cpid.BeforeCommand, this.cpid.AfterCommand, false, new List<PID> { this.cpid });
			}

			// Token: 0x040026DD RID: 9949
			public CustomPID cpid;
		}

		// Token: 0x02000746 RID: 1862
		[CompilerGenerated]
		private sealed class <>c__DisplayClass125_0
		{
			// Token: 0x06003F85 RID: 16261 RVA: 0x00002050 File Offset: 0x00000250
			public <>c__DisplayClass125_0()
			{
			}

			// Token: 0x06003F86 RID: 16262 RVA: 0x003310CC File Offset: 0x0032F2CC
			internal void <Action_ResponseReceived>b__0()
			{
				Page currentPage = App.GetCurrentPage();
				if (currentPage != null)
				{
					VisualElementExtension.DisplayToastAsync(currentPage, "Request:\n" + this.request.Command + "\nResponse:\n" + this.data, 5000);
				}
			}

			// Token: 0x040026DE RID: 9950
			public OBDRequest request;

			// Token: 0x040026DF RID: 9951
			public string data;
		}

		// Token: 0x02000747 RID: 1863
		[CompilerGenerated]
		private sealed class <>c__DisplayClass127_0
		{
			// Token: 0x06003F87 RID: 16263 RVA: 0x00002050 File Offset: 0x00000250
			public <>c__DisplayClass127_0()
			{
			}

			// Token: 0x06003F88 RID: 16264 RVA: 0x0033110E File Offset: 0x0032F30E
			internal bool <GetRequests>b__0(OBDRequest x)
			{
				return x.Equals(this.req);
			}

			// Token: 0x06003F89 RID: 16265 RVA: 0x0033110E File Offset: 0x0032F30E
			internal bool <GetRequests>b__1(OBDRequest x)
			{
				return x.Equals(this.req);
			}

			// Token: 0x040026E0 RID: 9952
			public OBDRequest req;
		}

		// Token: 0x02000748 RID: 1864
		[CompilerGenerated]
		private sealed class <>c__DisplayClass127_1
		{
			// Token: 0x06003F8A RID: 16266 RVA: 0x00002050 File Offset: 0x00000250
			public <>c__DisplayClass127_1()
			{
			}

			// Token: 0x06003F8B RID: 16267 RVA: 0x0033111C File Offset: 0x0032F31C
			internal bool <GetRequests>b__2(OBDRequest x)
			{
				return x.Equals(this.req);
			}

			// Token: 0x040026E1 RID: 9953
			public OBDRequest req;
		}

		// Token: 0x02000749 RID: 1865
		[CompilerGenerated]
		private sealed class <>c__DisplayClass128_0
		{
			// Token: 0x06003F8C RID: 16268 RVA: 0x00002050 File Offset: 0x00000250
			public <>c__DisplayClass128_0()
			{
			}

			// Token: 0x06003F8D RID: 16269 RVA: 0x0033112A File Offset: 0x0032F32A
			internal bool <CheckIfRequestInQueue>b__0(OBDRequest x)
			{
				return x.Equals(this.request);
			}

			// Token: 0x040026E2 RID: 9954
			public OBDRequest request;
		}
	}
}
