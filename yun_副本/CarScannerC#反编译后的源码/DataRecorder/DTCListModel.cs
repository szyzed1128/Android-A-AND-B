using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using CarScannerXamarinForms.DTC;
using CarScannerXamarinForms.Settings;

namespace CarScannerXamarinForms.DataRecorder
{
	// Token: 0x020006E9 RID: 1769
	internal class DTCListModel : ObservableCollection<DTCItemV2>
	{
		// Token: 0x06003C43 RID: 15427 RVA: 0x003188D0 File Offset: 0x00316AD0
		public DTCListModel(List<DTCItemV2> fullList)
		{
			this.fullList = fullList;
			SharedSettings.Current.PropertyChanged += this.SharedSettings_PropertyChanged;
			this.UpdateCollection();
		}

		// Token: 0x06003C44 RID: 15428 RVA: 0x003188FC File Offset: 0x00316AFC
		private void UpdateCollection()
		{
			List<DTCItemV2> list = this.fullList.ToList<DTCItemV2>();
			if (SharedSettings.Current.HideArchiveDTC)
			{
				list = list.Where((DTCItemV2 x) => !x.IsArchive).ToList<DTCItemV2>();
			}
			if (SharedSettings.Current.HideDTCWithUncomplitedTests)
			{
				list = list.Where((DTCItemV2 x) => !x.OnlyTestNotComplitedDTC).ToList<DTCItemV2>();
			}
			base.Clear();
			foreach (DTCItemV2 dtcitemV in list.ToArray())
			{
				base.Add(dtcitemV);
			}
		}

		// Token: 0x06003C45 RID: 15429 RVA: 0x003189A9 File Offset: 0x00316BA9
		private void SharedSettings_PropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			if (e.PropertyName == "HideArchiveDTC" || e.PropertyName == "HideDTCWithUncomplitedTests")
			{
				this.UpdateCollection();
			}
		}

		// Token: 0x040024E2 RID: 9442
		private List<DTCItemV2> fullList;

		// Token: 0x020006EA RID: 1770
		[CompilerGenerated]
		[Serializable]
		private sealed class <>c
		{
			// Token: 0x06003C46 RID: 15430 RVA: 0x003189D5 File Offset: 0x00316BD5
			// Note: this type is marked as 'beforefieldinit'.
			static <>c()
			{
			}

			// Token: 0x06003C47 RID: 15431 RVA: 0x00002050 File Offset: 0x00000250
			public <>c()
			{
			}

			// Token: 0x06003C48 RID: 15432 RVA: 0x00216CF9 File Offset: 0x00214EF9
			internal bool <UpdateCollection>b__2_0(DTCItemV2 x)
			{
				return !x.IsArchive;
			}

			// Token: 0x06003C49 RID: 15433 RVA: 0x00216D04 File Offset: 0x00214F04
			internal bool <UpdateCollection>b__2_1(DTCItemV2 x)
			{
				return !x.OnlyTestNotComplitedDTC;
			}

			// Token: 0x040024E3 RID: 9443
			public static readonly DTCListModel.<>c <>9 = new DTCListModel.<>c();

			// Token: 0x040024E4 RID: 9444
			public static Func<DTCItemV2, bool> <>9__2_0;

			// Token: 0x040024E5 RID: 9445
			public static Func<DTCItemV2, bool> <>9__2_1;
		}
	}
}
