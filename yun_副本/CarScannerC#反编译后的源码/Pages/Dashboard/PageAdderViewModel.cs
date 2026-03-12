using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using CarScannerXamarinForms.Dashboard;
using CarScannerXamarinForms.Dashboard.ProxySettings;
using CarScannerXamarinForms.OBD2.PIDS;
using CarScannerXamarinForms.OBD2.PIDS.CalculatedPIDs;
using CarScannerXamarinForms.Settings;
using CarScannerXamarinForms.ViewModels;
using Xamarin.Forms;

namespace CarScannerXamarinForms.Pages.Dashboard
{
	// Token: 0x020006D1 RID: 1745
	internal class PageAdderViewModel : INotifyPropertyChanged
	{
		// Token: 0x06003B5F RID: 15199 RVA: 0x003148E8 File Offset: 0x00312AE8
		public PageAdderViewModel(DescriptionPage page)
		{
			this.Page = page;
			this.SelectedPIDs.CollectionChanged += this.SelectedPIDs_CollectionChanged;
			this.AvailablePIDs = new List<IPID>(LiveDataPIDModel._PIDCollection);
			this.UpdateFilter();
		}

		// Token: 0x14000038 RID: 56
		// (add) Token: 0x06003B60 RID: 15200 RVA: 0x00314970 File Offset: 0x00312B70
		// (remove) Token: 0x06003B61 RID: 15201 RVA: 0x003149A8 File Offset: 0x00312BA8
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

		// Token: 0x06003B62 RID: 15202 RVA: 0x003149DD File Offset: 0x00312BDD
		private void SelectedPIDs_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
		{
			if (this.SelectedPIDs.Count < this.MaxItems)
			{
				this.CanAdd = true;
			}
			else
			{
				this.CanAdd = false;
			}
			PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
			if (propertyChanged == null)
			{
				return;
			}
			propertyChanged(this, new PropertyChangedEventArgs("CanAdd"));
		}

		// Token: 0x06003B63 RID: 15203 RVA: 0x00314A1D File Offset: 0x00312C1D
		public void AddPidToSelected(IPID pid, DashboardItemTypes itemType)
		{
			if (this.CanAdd)
			{
				this.SelectedPIDs.Add(pid);
				this.SelectedPIDsItemTypes.Add(itemType);
				this.AvailablePIDs.Remove(pid);
				this.UpdateFilter();
			}
		}

		// Token: 0x06003B64 RID: 15204 RVA: 0x00314A54 File Offset: 0x00312C54
		public void RemovePidFromSelected(IPID pid)
		{
			int num = this.SelectedPIDs.IndexOf(pid);
			this.SelectedPIDs.Remove(pid);
			this.SelectedPIDsItemTypes.RemoveAt(num);
			this.AvailablePIDs.Add(pid);
			this.UpdateFilter();
		}

		// Token: 0x170013A1 RID: 5025
		// (get) Token: 0x06003B65 RID: 15205 RVA: 0x00314A99 File Offset: 0x00312C99
		// (set) Token: 0x06003B66 RID: 15206 RVA: 0x00314AA1 File Offset: 0x00312CA1
		public string Filter
		{
			get
			{
				return this._Filter;
			}
			set
			{
				this._Filter = value;
				this.UpdateFilter();
			}
		}

		// Token: 0x06003B67 RID: 15207 RVA: 0x00314AB0 File Offset: 0x00312CB0
		private void UpdateFilter()
		{
			if (string.IsNullOrEmpty(this.Filter))
			{
				IEnumerable<IPID> enumerable = this.Sort(this.AvailablePIDs);
				this.FilteredPidList.Reset(enumerable);
				return;
			}
			string text = this.Filter.Trim();
			IEnumerable<IPID> enumerable2 = this.AvailablePIDs;
			if (!string.IsNullOrEmpty(text))
			{
				string[] array = text.Split(new char[] { ' ' });
				for (int i = 0; i < array.Length; i++)
				{
					string word = array[i];
					enumerable2 = enumerable2.Where((IPID x) => (!string.IsNullOrEmpty(x.Name) && x.Name.IndexOf(word, StringComparison.OrdinalIgnoreCase) >= 0) || (!string.IsNullOrEmpty(x.ShortName) && x.ShortName.IndexOf(word, StringComparison.OrdinalIgnoreCase) >= 0));
				}
			}
			IEnumerable<IPID> enumerable3 = this.Sort(enumerable2);
			this.FilteredPidList.Reset(enumerable3);
		}

		// Token: 0x06003B68 RID: 15208 RVA: 0x00314B60 File Offset: 0x00312D60
		private IEnumerable<IPID> Sort(IEnumerable<IPID> collection)
		{
			switch (SharedSettings.Current.PIDSortingMode)
			{
			case SharedSettings.PIDSortingModes.NameAsc:
				return collection.OrderBy((IPID x) => x.Name);
			case SharedSettings.PIDSortingModes.NameDesc:
				return collection.OrderByDescending((IPID x) => x.Name);
			}
			return from x in collection
				orderby x is CalculatedPIDV2, x.Id
				select x;
		}

		// Token: 0x170013A2 RID: 5026
		// (get) Token: 0x06003B69 RID: 15209 RVA: 0x00314C21 File Offset: 0x00312E21
		// (set) Token: 0x06003B6A RID: 15210 RVA: 0x00314C29 File Offset: 0x00312E29
		public SmartCollection<IPID> FilteredPidList
		{
			[CompilerGenerated]
			get
			{
				return this.<FilteredPidList>k__BackingField;
			}
			[CompilerGenerated]
			private set
			{
				this.<FilteredPidList>k__BackingField = value;
			}
		} = new SmartCollection<IPID>();

		// Token: 0x170013A3 RID: 5027
		// (get) Token: 0x06003B6B RID: 15211 RVA: 0x00314C32 File Offset: 0x00312E32
		// (set) Token: 0x06003B6C RID: 15212 RVA: 0x00314C3A File Offset: 0x00312E3A
		private List<IPID> AvailablePIDs
		{
			[CompilerGenerated]
			get
			{
				return this.<AvailablePIDs>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<AvailablePIDs>k__BackingField = value;
			}
		}

		// Token: 0x170013A4 RID: 5028
		// (get) Token: 0x06003B6D RID: 15213 RVA: 0x00314C43 File Offset: 0x00312E43
		// (set) Token: 0x06003B6E RID: 15214 RVA: 0x00314C4B File Offset: 0x00312E4B
		public ObservableCollection<IPID> SelectedPIDs
		{
			[CompilerGenerated]
			get
			{
				return this.<SelectedPIDs>k__BackingField;
			}
			[CompilerGenerated]
			private set
			{
				this.<SelectedPIDs>k__BackingField = value;
			}
		} = new ObservableCollection<IPID>();

		// Token: 0x170013A5 RID: 5029
		// (get) Token: 0x06003B6F RID: 15215 RVA: 0x00314C54 File Offset: 0x00312E54
		public int MaxItems
		{
			get
			{
				return this.Page.PlacesCount;
			}
		}

		// Token: 0x170013A6 RID: 5030
		// (get) Token: 0x06003B70 RID: 15216 RVA: 0x00314C61 File Offset: 0x00312E61
		// (set) Token: 0x06003B71 RID: 15217 RVA: 0x00314C69 File Offset: 0x00312E69
		public bool CanAdd
		{
			[CompilerGenerated]
			get
			{
				return this.<CanAdd>k__BackingField;
			}
			[CompilerGenerated]
			private set
			{
				this.<CanAdd>k__BackingField = value;
			}
		} = true;

		// Token: 0x06003B72 RID: 15218 RVA: 0x00314C74 File Offset: 0x00312E74
		internal void BuildPage()
		{
			DashboardPage dashboardPageFromDashboardType = DashboardPage.GetDashboardPageFromDashboardType(this.Page.DashboardType);
			dashboardPageFromDashboardType.CreateGrid();
			dashboardPageFromDashboardType.RebuildItems();
			dashboardPageFromDashboardType.Title = Translate.GetString("ios_DashboardPage") + " " + (DashboardListViewModel.Current.Pages.Count + 1).ToString(CultureInfo.InvariantCulture);
			DashboardListViewModel.Current.Pages.Add(dashboardPageFromDashboardType);
			for (int i = 0; i < this.SelectedPIDs.Count; i++)
			{
				dashboardPageFromDashboardType.Items[i].PID_Id = this.SelectedPIDs[i].Id;
				dashboardPageFromDashboardType.Items[i].Minimum = this.SelectedPIDs[i].Minimum;
				dashboardPageFromDashboardType.Items[i].Maximum = this.SelectedPIDs[i].Maximum;
				dashboardPageFromDashboardType.Items[i].CustomName = this.SelectedPIDs[i].ShortName;
				dashboardPageFromDashboardType.Items[i].ValueFontSize = Device.GetNamedSize(4, typeof(Label)) * 1.5;
				dashboardPageFromDashboardType.Items[i].ItemType = this.SelectedPIDsItemTypes[i];
			}
			DashboardListViewModel.Current.SaveDashboardToSettings();
			SharedSettings.Current.DashboardLastPage = DashboardListViewModel.Current.Pages.Count - 1;
			DashboardXamlPage.Instance.ShouldLoadDashboardFromSettings = true;
		}

		// Token: 0x04002463 RID: 9315
		private DescriptionPage Page;

		// Token: 0x04002464 RID: 9316
		[CompilerGenerated]
		private PropertyChangedEventHandler PropertyChanged;

		// Token: 0x04002465 RID: 9317
		private string _Filter = "";

		// Token: 0x04002466 RID: 9318
		private string filtertext = "";

		// Token: 0x04002467 RID: 9319
		[CompilerGenerated]
		private SmartCollection<IPID> <FilteredPidList>k__BackingField;

		// Token: 0x04002468 RID: 9320
		[CompilerGenerated]
		private List<IPID> <AvailablePIDs>k__BackingField;

		// Token: 0x04002469 RID: 9321
		[CompilerGenerated]
		private ObservableCollection<IPID> <SelectedPIDs>k__BackingField;

		// Token: 0x0400246A RID: 9322
		private List<DashboardItemTypes> SelectedPIDsItemTypes = new List<DashboardItemTypes>();

		// Token: 0x0400246B RID: 9323
		[CompilerGenerated]
		private bool <CanAdd>k__BackingField;

		// Token: 0x020006D2 RID: 1746
		[CompilerGenerated]
		[Serializable]
		private sealed class <>c
		{
			// Token: 0x06003B73 RID: 15219 RVA: 0x00314E02 File Offset: 0x00313002
			// Note: this type is marked as 'beforefieldinit'.
			static <>c()
			{
			}

			// Token: 0x06003B74 RID: 15220 RVA: 0x00002050 File Offset: 0x00000250
			public <>c()
			{
			}

			// Token: 0x06003B75 RID: 15221 RVA: 0x00314E0E File Offset: 0x0031300E
			internal string <Sort>b__14_0(IPID x)
			{
				return x.Name;
			}

			// Token: 0x06003B76 RID: 15222 RVA: 0x00314E0E File Offset: 0x0031300E
			internal string <Sort>b__14_1(IPID x)
			{
				return x.Name;
			}

			// Token: 0x06003B77 RID: 15223 RVA: 0x000ABFE4 File Offset: 0x000AA1E4
			internal bool <Sort>b__14_2(IPID x)
			{
				return x is CalculatedPIDV2;
			}

			// Token: 0x06003B78 RID: 15224 RVA: 0x000D6EE1 File Offset: 0x000D50E1
			internal int <Sort>b__14_3(IPID x)
			{
				return x.Id;
			}

			// Token: 0x0400246C RID: 9324
			public static readonly PageAdderViewModel.<>c <>9 = new PageAdderViewModel.<>c();

			// Token: 0x0400246D RID: 9325
			public static Func<IPID, string> <>9__14_0;

			// Token: 0x0400246E RID: 9326
			public static Func<IPID, string> <>9__14_1;

			// Token: 0x0400246F RID: 9327
			public static Func<IPID, bool> <>9__14_2;

			// Token: 0x04002470 RID: 9328
			public static Func<IPID, int> <>9__14_3;
		}

		// Token: 0x020006D3 RID: 1747
		[CompilerGenerated]
		private sealed class <>c__DisplayClass13_0
		{
			// Token: 0x06003B79 RID: 15225 RVA: 0x00002050 File Offset: 0x00000250
			public <>c__DisplayClass13_0()
			{
			}

			// Token: 0x06003B7A RID: 15226 RVA: 0x00314E18 File Offset: 0x00313018
			internal bool <UpdateFilter>b__0(IPID x)
			{
				return (!string.IsNullOrEmpty(x.Name) && x.Name.IndexOf(this.word, StringComparison.OrdinalIgnoreCase) >= 0) || (!string.IsNullOrEmpty(x.ShortName) && x.ShortName.IndexOf(this.word, StringComparison.OrdinalIgnoreCase) >= 0);
			}

			// Token: 0x04002471 RID: 9329
			public string word;
		}
	}
}
