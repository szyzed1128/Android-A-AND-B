using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using CarScannerXamarinForms.OBD2.PIDS;
using CarScannerXamarinForms.Settings;

namespace CarScannerXamarinForms.Pages.Dashboard
{
	// Token: 0x020006CD RID: 1741
	internal class MultiPIDSelectorModel : INotifyPropertyChanged
	{
		// Token: 0x06003B46 RID: 15174 RVA: 0x00314530 File Offset: 0x00312730
		public MultiPIDSelectorModel(IEnumerable<IPID> initialPids, List<int> alreadySelectedPids)
		{
			MultiPIDSelectorModel <>4__this = this;
			this.AvailablePIDs = initialPids.Select((IPID x) => new ProxyPidForSelector(x, <>4__this.SetAsSelected(x, alreadySelectedPids))).ToList<ProxyPidForSelector>();
			this.UpdateFilter();
		}

		// Token: 0x06003B47 RID: 15175 RVA: 0x0031459B File Offset: 0x0031279B
		private bool SetAsSelected(IPID pid, List<int> alreadySelectedPids)
		{
			return alreadySelectedPids.Contains(pid.Id);
		}

		// Token: 0x1700139E RID: 5022
		// (get) Token: 0x06003B48 RID: 15176 RVA: 0x003145AE File Offset: 0x003127AE
		// (set) Token: 0x06003B49 RID: 15177 RVA: 0x003145B6 File Offset: 0x003127B6
		public SmartCollection<ProxyPidForSelector> FilteredPidList
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
		} = new SmartCollection<ProxyPidForSelector>();

		// Token: 0x1700139F RID: 5023
		// (get) Token: 0x06003B4A RID: 15178 RVA: 0x003145BF File Offset: 0x003127BF
		// (set) Token: 0x06003B4B RID: 15179 RVA: 0x003145C7 File Offset: 0x003127C7
		private List<ProxyPidForSelector> AvailablePIDs
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

		// Token: 0x170013A0 RID: 5024
		// (get) Token: 0x06003B4C RID: 15180 RVA: 0x003145D0 File Offset: 0x003127D0
		// (set) Token: 0x06003B4D RID: 15181 RVA: 0x003145D8 File Offset: 0x003127D8
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

		// Token: 0x14000037 RID: 55
		// (add) Token: 0x06003B4E RID: 15182 RVA: 0x003145E8 File Offset: 0x003127E8
		// (remove) Token: 0x06003B4F RID: 15183 RVA: 0x00314620 File Offset: 0x00312820
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

		// Token: 0x06003B50 RID: 15184 RVA: 0x00314658 File Offset: 0x00312858
		private void UpdateFilter()
		{
			if (string.IsNullOrEmpty(this.Filter))
			{
				IEnumerable<ProxyPidForSelector> enumerable = this.Sort(this.AvailablePIDs);
				this.FilteredPidList.Reset(enumerable);
				return;
			}
			string text = this.Filter.Trim();
			IEnumerable<ProxyPidForSelector> enumerable2 = this.AvailablePIDs;
			if (!string.IsNullOrEmpty(text))
			{
				string[] array = text.Split(new char[] { ' ' });
				for (int i = 0; i < array.Length; i++)
				{
					string word = array[i];
					enumerable2 = enumerable2.Where((ProxyPidForSelector x) => (!string.IsNullOrEmpty(x.Pid.Name) && x.Pid.Name.IndexOf(word, StringComparison.OrdinalIgnoreCase) >= 0) || (!string.IsNullOrEmpty(x.Pid.ShortName) && x.Pid.ShortName.IndexOf(word, StringComparison.OrdinalIgnoreCase) >= 0));
				}
			}
			IEnumerable<ProxyPidForSelector> enumerable3 = this.Sort(enumerable2);
			this.FilteredPidList.Reset(enumerable3);
		}

		// Token: 0x06003B51 RID: 15185 RVA: 0x00314708 File Offset: 0x00312908
		private IEnumerable<ProxyPidForSelector> Sort(IEnumerable<ProxyPidForSelector> collection)
		{
			switch (SharedSettings.Current.PIDSortingMode)
			{
			case SharedSettings.PIDSortingModes.NameAsc:
				return collection.OrderBy((ProxyPidForSelector x) => x.Pid.Name);
			case SharedSettings.PIDSortingModes.NameDesc:
				return collection.OrderByDescending((ProxyPidForSelector x) => x.Pid.Name);
			}
			return from x in collection
				orderby false, x.Pid.Id
				select x;
		}

		// Token: 0x06003B52 RID: 15186 RVA: 0x003147CC File Offset: 0x003129CC
		public List<IPID> GetSelected()
		{
			return (from x in this.AvailablePIDs
				where x.IsSelected
				select x.Pid).ToList<IPID>();
		}

		// Token: 0x04002454 RID: 9300
		[CompilerGenerated]
		private SmartCollection<ProxyPidForSelector> <FilteredPidList>k__BackingField;

		// Token: 0x04002455 RID: 9301
		[CompilerGenerated]
		private List<ProxyPidForSelector> <AvailablePIDs>k__BackingField;

		// Token: 0x04002456 RID: 9302
		private string _Filter = "";

		// Token: 0x04002457 RID: 9303
		private string filtertext = "";

		// Token: 0x04002458 RID: 9304
		[CompilerGenerated]
		private PropertyChangedEventHandler PropertyChanged;

		// Token: 0x020006CE RID: 1742
		[CompilerGenerated]
		[Serializable]
		private sealed class <>c
		{
			// Token: 0x06003B53 RID: 15187 RVA: 0x0031482C File Offset: 0x00312A2C
			// Note: this type is marked as 'beforefieldinit'.
			static <>c()
			{
			}

			// Token: 0x06003B54 RID: 15188 RVA: 0x00002050 File Offset: 0x00000250
			public <>c()
			{
			}

			// Token: 0x06003B55 RID: 15189 RVA: 0x00314838 File Offset: 0x00312A38
			internal string <Sort>b__19_0(ProxyPidForSelector x)
			{
				return x.Pid.Name;
			}

			// Token: 0x06003B56 RID: 15190 RVA: 0x00314838 File Offset: 0x00312A38
			internal string <Sort>b__19_1(ProxyPidForSelector x)
			{
				return x.Pid.Name;
			}

			// Token: 0x06003B57 RID: 15191 RVA: 0x00002076 File Offset: 0x00000276
			internal bool <Sort>b__19_2(ProxyPidForSelector x)
			{
				return false;
			}

			// Token: 0x06003B58 RID: 15192 RVA: 0x00314845 File Offset: 0x00312A45
			internal int <Sort>b__19_3(ProxyPidForSelector x)
			{
				return x.Pid.Id;
			}

			// Token: 0x06003B59 RID: 15193 RVA: 0x00314852 File Offset: 0x00312A52
			internal bool <GetSelected>b__20_0(ProxyPidForSelector x)
			{
				return x.IsSelected;
			}

			// Token: 0x06003B5A RID: 15194 RVA: 0x0031485A File Offset: 0x00312A5A
			internal IPID <GetSelected>b__20_1(ProxyPidForSelector x)
			{
				return x.Pid;
			}

			// Token: 0x04002459 RID: 9305
			public static readonly MultiPIDSelectorModel.<>c <>9 = new MultiPIDSelectorModel.<>c();

			// Token: 0x0400245A RID: 9306
			public static Func<ProxyPidForSelector, string> <>9__19_0;

			// Token: 0x0400245B RID: 9307
			public static Func<ProxyPidForSelector, string> <>9__19_1;

			// Token: 0x0400245C RID: 9308
			public static Func<ProxyPidForSelector, bool> <>9__19_2;

			// Token: 0x0400245D RID: 9309
			public static Func<ProxyPidForSelector, int> <>9__19_3;

			// Token: 0x0400245E RID: 9310
			public static Func<ProxyPidForSelector, bool> <>9__20_0;

			// Token: 0x0400245F RID: 9311
			public static Func<ProxyPidForSelector, IPID> <>9__20_1;
		}

		// Token: 0x020006CF RID: 1743
		[CompilerGenerated]
		private sealed class <>c__DisplayClass0_0
		{
			// Token: 0x06003B5B RID: 15195 RVA: 0x00002050 File Offset: 0x00000250
			public <>c__DisplayClass0_0()
			{
			}

			// Token: 0x06003B5C RID: 15196 RVA: 0x00314862 File Offset: 0x00312A62
			internal ProxyPidForSelector <.ctor>b__0(IPID x)
			{
				return new ProxyPidForSelector(x, this.<>4__this.SetAsSelected(x, this.alreadySelectedPids));
			}

			// Token: 0x04002460 RID: 9312
			public MultiPIDSelectorModel <>4__this;

			// Token: 0x04002461 RID: 9313
			public List<int> alreadySelectedPids;
		}

		// Token: 0x020006D0 RID: 1744
		[CompilerGenerated]
		private sealed class <>c__DisplayClass18_0
		{
			// Token: 0x06003B5D RID: 15197 RVA: 0x00002050 File Offset: 0x00000250
			public <>c__DisplayClass18_0()
			{
			}

			// Token: 0x06003B5E RID: 15198 RVA: 0x0031487C File Offset: 0x00312A7C
			internal bool <UpdateFilter>b__0(ProxyPidForSelector x)
			{
				return (!string.IsNullOrEmpty(x.Pid.Name) && x.Pid.Name.IndexOf(this.word, StringComparison.OrdinalIgnoreCase) >= 0) || (!string.IsNullOrEmpty(x.Pid.ShortName) && x.Pid.ShortName.IndexOf(this.word, StringComparison.OrdinalIgnoreCase) >= 0);
			}

			// Token: 0x04002462 RID: 9314
			public string word;
		}
	}
}
