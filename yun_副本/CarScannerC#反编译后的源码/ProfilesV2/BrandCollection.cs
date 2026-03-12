using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;

namespace CarScannerXamarinForms.ProfilesV2
{
	// Token: 0x020002BE RID: 702
	public class BrandCollection : ObservableCollection<OBDReaderProfileV2>, INotifyPropertyChanged
	{
		// Token: 0x06002229 RID: 8745 RVA: 0x001A8E7A File Offset: 0x001A707A
		public BrandCollection()
		{
		}

		// Token: 0x0600222A RID: 8746 RVA: 0x001A8E82 File Offset: 0x001A7082
		public BrandCollection(IEnumerable<OBDReaderProfileV2> profiles)
			: base(profiles)
		{
		}

		// Token: 0x0600222B RID: 8747 RVA: 0x001A8E8B File Offset: 0x001A708B
		public BrandCollection(List<OBDReaderProfileV2> profiles)
			: base(profiles)
		{
		}

		// Token: 0x0600222C RID: 8748 RVA: 0x001A8E94 File Offset: 0x001A7094
		public BrandCollection(string name)
			: this()
		{
			this.Name = name;
		}

		// Token: 0x0600222D RID: 8749 RVA: 0x001A8EA3 File Offset: 0x001A70A3
		public BrandCollection(string name, IEnumerable<OBDReaderProfileV2> profiles)
			: this(profiles)
		{
			this.Name = name;
		}

		// Token: 0x0600222E RID: 8750 RVA: 0x001A8EB3 File Offset: 0x001A70B3
		public BrandCollection(string name, List<OBDReaderProfileV2> profiles)
			: this(profiles)
		{
			this.Name = name;
		}

		// Token: 0x170010E6 RID: 4326
		// (get) Token: 0x0600222F RID: 8751 RVA: 0x001A8EC3 File Offset: 0x001A70C3
		// (set) Token: 0x06002230 RID: 8752 RVA: 0x001A8ECB File Offset: 0x001A70CB
		public string Name
		{
			get
			{
				return this._Name;
			}
			set
			{
				this._Name = value;
				this.OnPropertyChanged(new PropertyChangedEventArgs("Name"));
			}
		}

		// Token: 0x06002231 RID: 8753 RVA: 0x001A8EE4 File Offset: 0x001A70E4
		public void Sort()
		{
			this.FirstOrDefault((OBDReaderProfileV2 x) => x.Name == "OBD-II / EOBD");
		}

		// Token: 0x0400104B RID: 4171
		private string _Name;

		// Token: 0x020002BF RID: 703
		[CompilerGenerated]
		[Serializable]
		private sealed class <>c
		{
			// Token: 0x06002232 RID: 8754 RVA: 0x001A8F0C File Offset: 0x001A710C
			// Note: this type is marked as 'beforefieldinit'.
			static <>c()
			{
			}

			// Token: 0x06002233 RID: 8755 RVA: 0x00002050 File Offset: 0x00000250
			public <>c()
			{
			}

			// Token: 0x06002234 RID: 8756 RVA: 0x001A8F18 File Offset: 0x001A7118
			internal bool <Sort>b__10_0(OBDReaderProfileV2 x)
			{
				return x.Name == "OBD-II / EOBD";
			}

			// Token: 0x0400104C RID: 4172
			public static readonly BrandCollection.<>c <>9 = new BrandCollection.<>c();

			// Token: 0x0400104D RID: 4173
			public static Func<OBDReaderProfileV2, bool> <>9__10_0;
		}
	}
}
