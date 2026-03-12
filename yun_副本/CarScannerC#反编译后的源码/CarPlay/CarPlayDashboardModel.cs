using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using CarScannerXamarinForms.Common;
using CarScannerXamarinForms.Dashboard.ProxySettings;
using CarScannerXamarinForms.OBD2;
using CarScannerXamarinForms.OBD2.PIDS;
using CarScannerXamarinForms.OBD2.PIDS.CalculatedPIDs;
using CarScannerXamarinForms.Settings;
using CarScannerXamarinForms.ViewModels;
using Newtonsoft.Json;

namespace CarScannerXamarinForms.CarPlay
{
	// Token: 0x02000BE1 RID: 3041
	public class CarPlayDashboardModel : List<CarPlayDashboardPage>
	{
		// Token: 0x06005B64 RID: 23396 RVA: 0x00438DB2 File Offset: 0x00436FB2
		public CarPlayDashboardModel(int maxItems, int maxTextItems)
		{
			this.MaxItems = maxItems;
			this.ListOfPages = new PagingList(new List<KeyValuePair<string, int>>(0), maxItems, "", "");
		}

		// Token: 0x06005B65 RID: 23397 RVA: 0x00438DF0 File Offset: 0x00436FF0
		public CarPlayDashboardModel(IEnumerable<CarPlayDashboardPage> pages, int maxItems, int maxTextItems)
			: base(pages)
		{
			this.MaxItems = maxItems;
			this.MaxTextItems = maxTextItems;
			this.ListOfPages = new PagingList(new List<KeyValuePair<string, int>>(0), maxItems, "", "");
		}

		// Token: 0x17001856 RID: 6230
		// (get) Token: 0x06005B66 RID: 23398 RVA: 0x00438E3E File Offset: 0x0043703E
		// (set) Token: 0x06005B67 RID: 23399 RVA: 0x00438E46 File Offset: 0x00437046
		public int MaxItems
		{
			[CompilerGenerated]
			get
			{
				return this.<MaxItems>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<MaxItems>k__BackingField = value;
			}
		} = 10;

		// Token: 0x17001857 RID: 6231
		// (get) Token: 0x06005B68 RID: 23400 RVA: 0x00438E4F File Offset: 0x0043704F
		// (set) Token: 0x06005B69 RID: 23401 RVA: 0x00438E57 File Offset: 0x00437057
		public int MaxTextItems
		{
			[CompilerGenerated]
			get
			{
				return this.<MaxTextItems>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<MaxTextItems>k__BackingField = value;
			}
		} = 10;

		// Token: 0x17001858 RID: 6232
		// (get) Token: 0x06005B6A RID: 23402 RVA: 0x00438E60 File Offset: 0x00437060
		// (set) Token: 0x06005B6B RID: 23403 RVA: 0x00438E9F File Offset: 0x0043709F
		public int CurrentPageIndex
		{
			get
			{
				int carPlayDashboardSelectedIndex = SharedSettings.Current.CarPlayDashboardSelectedIndex;
				if (carPlayDashboardSelectedIndex >= base.Count)
				{
					SharedSettings.Current.CarPlayDashboardSelectedIndex = 0;
				}
				if (carPlayDashboardSelectedIndex < 0)
				{
					SharedSettings.Current.CarPlayDashboardSelectedIndex = base.Count - 1;
				}
				return SharedSettings.Current.CarPlayDashboardSelectedIndex;
			}
			set
			{
				if (value >= base.Count)
				{
					value = 0;
				}
				if (value < 0)
				{
					value = base.Count - 1;
				}
				SharedSettings.Current.CarPlayDashboardSelectedIndex = value;
			}
		}

		// Token: 0x06005B6C RID: 23404 RVA: 0x00438EC6 File Offset: 0x004370C6
		public CarPlayDashboardPage GetCurrentPage()
		{
			if (base.Count == 0)
			{
				return new CarPlayDashboardPage(0);
			}
			return base[this.CurrentPageIndex];
		}

		// Token: 0x17001859 RID: 6233
		// (get) Token: 0x06005B6D RID: 23405 RVA: 0x00438EE3 File Offset: 0x004370E3
		// (set) Token: 0x06005B6E RID: 23406 RVA: 0x00438EEB File Offset: 0x004370EB
		public bool IsLoaded
		{
			[CompilerGenerated]
			get
			{
				return this.<IsLoaded>k__BackingField;
			}
			[CompilerGenerated]
			private set
			{
				this.<IsLoaded>k__BackingField = value;
			}
		}

		// Token: 0x1700185A RID: 6234
		// (get) Token: 0x06005B6F RID: 23407 RVA: 0x00438EF4 File Offset: 0x004370F4
		// (set) Token: 0x06005B70 RID: 23408 RVA: 0x00438EFC File Offset: 0x004370FC
		public PagingList ListOfPages
		{
			[CompilerGenerated]
			get
			{
				return this.<ListOfPages>k__BackingField;
			}
			[CompilerGenerated]
			private set
			{
				this.<ListOfPages>k__BackingField = value;
			}
		}

		// Token: 0x06005B71 RID: 23409 RVA: 0x00438F08 File Offset: 0x00437108
		public void LoadFromSettings()
		{
			this.ClearAndUnsubscribe();
			string dashboard = SharedSettings.Current.Dashboard;
			if (string.IsNullOrEmpty(dashboard))
			{
				App.OBDReader.DebugWrite("\r\n[CPDashboard: Empty]\r\n");
				this.LoadDefaultDashboard();
			}
			try
			{
				foreach (ProxyPage proxyPage in JsonConvert.DeserializeObject<List<ProxyPage>>(dashboard))
				{
					if (proxyPage.Items.Count > this.MaxTextItems)
					{
						int num = 1;
						int num2 = proxyPage.Items.Count / this.MaxTextItems;
						if (proxyPage.Items.Count % this.MaxTextItems != 0)
						{
							num2++;
						}
						for (int i = 0; i < proxyPage.Items.Count; i += this.MaxTextItems)
						{
							IEnumerable<ProxyItem> enumerable = proxyPage.Items.Skip(i).Take(this.MaxTextItems);
							CarPlayDashboardPage carPlayDashboardPage = new CarPlayDashboardPage(this.MaxTextItems)
							{
								Title = string.Concat(new string[]
								{
									proxyPage.Title,
									" (",
									num.ToString(),
									"/",
									num2.ToString(),
									")"
								})
							};
							foreach (ProxyItem proxyItem in enumerable)
							{
								CarPlayDashboardItem carPlayDashboardItem = new CarPlayDashboardItem(proxyItem);
								carPlayDashboardPage.Add(carPlayDashboardItem);
							}
							carPlayDashboardPage.UpdateInBackground = proxyPage.UpdateInBackground;
							base.Add(carPlayDashboardPage);
							num++;
						}
					}
					else
					{
						CarPlayDashboardPage carPlayDashboardPage2 = new CarPlayDashboardPage(proxyPage.Items.Count)
						{
							Title = proxyPage.Title
						};
						foreach (ProxyItem proxyItem2 in proxyPage.Items)
						{
							CarPlayDashboardItem carPlayDashboardItem2 = new CarPlayDashboardItem(proxyItem2);
							carPlayDashboardPage2.Add(carPlayDashboardItem2);
						}
						carPlayDashboardPage2.UpdateInBackground = proxyPage.UpdateInBackground;
						base.Add(carPlayDashboardPage2);
					}
				}
			}
			catch (Exception ex)
			{
				App.OBDReader.DebugWrite("\r\n[CPDashboard error exc: " + ex.ToString() + "]\r\n");
				base.Clear();
				this.LoadDefaultDashboard();
			}
			this.CurrentPageIndex = SharedSettings.Current.CarPlayDashboardSelectedIndex;
			List<KeyValuePair<string, int>> list = new List<KeyValuePair<string, int>>();
			for (int j = 0; j < base.Count; j++)
			{
				list.Add(new KeyValuePair<string, int>((j + 1).ToString() + ") " + base[j].Title, j));
			}
			this.ListOfPages = new PagingList(list, this.MaxItems, Translate.GetString("carPlay_PrevPages"), Translate.GetString("carPlay_NextPages"));
			if (App.OBDReader != null && App.OBDReader.CurrentStatus == OBDDataReaderStatus.ConnectedToECU)
			{
				this.IsLoaded = true;
			}
		}

		// Token: 0x06005B72 RID: 23410 RVA: 0x00439254 File Offset: 0x00437454
		public void ClearAndUnsubscribe()
		{
			foreach (CarPlayDashboardPage carPlayDashboardPage in this)
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
			base.Clear();
		}

		// Token: 0x06005B73 RID: 23411 RVA: 0x004392E4 File Offset: 0x004374E4
		private void LoadDefaultDashboard()
		{
			CarPlayDashboardPage carPlayDashboardPage = new CarPlayDashboardPage
			{
				Title = Translate.GetString("ios_DashboardPage") + " 1"
			};
			PID pid = App.OBDReader.CurrentCarData.FindPIDByRole(Roles.RPM) as PID;
			if (pid != null)
			{
				carPlayDashboardPage.Add(new CarPlayDashboardItem(pid));
			}
			PID pid2 = App.OBDReader.CurrentCarData.FindPIDByRole(Roles.Speed) as PID;
			if (pid2 == null)
			{
				pid2 = LiveDataPIDModel._PIDCollection.FirstOrDefault((PID x) => x is IPIDFloatValue && (x as IPIDFloatValue).Units == UnitsHelper.Units.kmh);
			}
			if (pid2 != null)
			{
				carPlayDashboardPage.Add(new CarPlayDashboardItem(pid2));
			}
			PID pid3 = App.OBDReader.CurrentCarData.FindPIDByRole(Roles.Coolant) as PID;
			if (pid3 != null)
			{
				carPlayDashboardPage.Add(new CarPlayDashboardItem(pid3));
			}
			CarPlayDashboardPage carPlayDashboardPage2 = new CarPlayDashboardPage(0)
			{
				Title = Translate.GetString("dash_CustomizablePage")
			};
			CarPlayDashboardPage carPlayDashboardPage3 = new CarPlayDashboardPage(3);
			CalculatedPIDV2[] array = new CalculatedPIDV2[0];
			try
			{
				array = (from x in LiveDataPIDModel._PIDCollection
					where x is CalculatedPIDV2
					select x as CalculatedPIDV2).ToArray<CalculatedPIDV2>();
			}
			catch (Exception)
			{
			}
			PID pid4 = array.FirstOrDefault((CalculatedPIDV2 x) => x is PID_CalculatedInstantFuelRate);
			PID pid5 = array.FirstOrDefault((CalculatedPIDV2 x) => x is PID_CalculatedAVGFuelConsumption);
			PID pid6 = array.FirstOrDefault((CalculatedPIDV2 x) => x is PID_CalculatedInstantFuelConsumption);
			PID pid7 = array.FirstOrDefault((CalculatedPIDV2 x) => x is PID_CalculatedAvgSpeed);
			PID pid8 = array.FirstOrDefault((CalculatedPIDV2 x) => x is PID_TotalDistance);
			PID pid9 = array.FirstOrDefault((CalculatedPIDV2 x) => x is PID_TotalFuelUsed);
			PID[] array2 = new PID[] { pid4, pid5, pid6, pid2, pid, pid7, pid8, pid9 };
			carPlayDashboardPage3.AddRange(from x in array2
				where x != null
				select new CarPlayDashboardItem(x));
			base.Add(carPlayDashboardPage);
			base.Add(carPlayDashboardPage2);
			base.Add(carPlayDashboardPage3);
			carPlayDashboardPage3.Title = Translate.GetString("ios_FuelConsumption");
			if (carPlayDashboardPage3.Title != null)
			{
				carPlayDashboardPage3.Title = carPlayDashboardPage3.Title.Replace(": ", "").Replace(":", "");
			}
		}

		// Token: 0x040039B1 RID: 14769
		[CompilerGenerated]
		private int <MaxItems>k__BackingField;

		// Token: 0x040039B2 RID: 14770
		[CompilerGenerated]
		private int <MaxTextItems>k__BackingField;

		// Token: 0x040039B3 RID: 14771
		[CompilerGenerated]
		private bool <IsLoaded>k__BackingField;

		// Token: 0x040039B4 RID: 14772
		[CompilerGenerated]
		private PagingList <ListOfPages>k__BackingField;

		// Token: 0x02000BE2 RID: 3042
		[CompilerGenerated]
		[Serializable]
		private sealed class <>c
		{
			// Token: 0x06005B74 RID: 23412 RVA: 0x0043960C File Offset: 0x0043780C
			// Note: this type is marked as 'beforefieldinit'.
			static <>c()
			{
			}

			// Token: 0x06005B75 RID: 23413 RVA: 0x00002050 File Offset: 0x00000250
			public <>c()
			{
			}

			// Token: 0x06005B76 RID: 23414 RVA: 0x000AC018 File Offset: 0x000AA218
			internal bool <LoadDefaultDashboard>b__24_0(PID x)
			{
				return x is IPIDFloatValue && (x as IPIDFloatValue).Units == UnitsHelper.Units.kmh;
			}

			// Token: 0x06005B77 RID: 23415 RVA: 0x000ABFE4 File Offset: 0x000AA1E4
			internal bool <LoadDefaultDashboard>b__24_1(PID x)
			{
				return x is CalculatedPIDV2;
			}

			// Token: 0x06005B78 RID: 23416 RVA: 0x000ABFEF File Offset: 0x000AA1EF
			internal CalculatedPIDV2 <LoadDefaultDashboard>b__24_2(PID x)
			{
				return x as CalculatedPIDV2;
			}

			// Token: 0x06005B79 RID: 23417 RVA: 0x000ABFF7 File Offset: 0x000AA1F7
			internal bool <LoadDefaultDashboard>b__24_3(CalculatedPIDV2 x)
			{
				return x is PID_CalculatedInstantFuelRate;
			}

			// Token: 0x06005B7A RID: 23418 RVA: 0x000AC002 File Offset: 0x000AA202
			internal bool <LoadDefaultDashboard>b__24_4(CalculatedPIDV2 x)
			{
				return x is PID_CalculatedAVGFuelConsumption;
			}

			// Token: 0x06005B7B RID: 23419 RVA: 0x000AC00D File Offset: 0x000AA20D
			internal bool <LoadDefaultDashboard>b__24_5(CalculatedPIDV2 x)
			{
				return x is PID_CalculatedInstantFuelConsumption;
			}

			// Token: 0x06005B7C RID: 23420 RVA: 0x000AC032 File Offset: 0x000AA232
			internal bool <LoadDefaultDashboard>b__24_6(CalculatedPIDV2 x)
			{
				return x is PID_CalculatedAvgSpeed;
			}

			// Token: 0x06005B7D RID: 23421 RVA: 0x000AC03D File Offset: 0x000AA23D
			internal bool <LoadDefaultDashboard>b__24_7(CalculatedPIDV2 x)
			{
				return x is PID_TotalDistance;
			}

			// Token: 0x06005B7E RID: 23422 RVA: 0x000AC048 File Offset: 0x000AA248
			internal bool <LoadDefaultDashboard>b__24_8(CalculatedPIDV2 x)
			{
				return x is PID_TotalFuelUsed;
			}

			// Token: 0x06005B7F RID: 23423 RVA: 0x001AB6E3 File Offset: 0x001A98E3
			internal bool <LoadDefaultDashboard>b__24_9(PID x)
			{
				return x != null;
			}

			// Token: 0x06005B80 RID: 23424 RVA: 0x00439618 File Offset: 0x00437818
			internal CarPlayDashboardItem <LoadDefaultDashboard>b__24_10(PID x)
			{
				return new CarPlayDashboardItem(x);
			}

			// Token: 0x040039B5 RID: 14773
			public static readonly CarPlayDashboardModel.<>c <>9 = new CarPlayDashboardModel.<>c();

			// Token: 0x040039B6 RID: 14774
			public static Func<PID, bool> <>9__24_0;

			// Token: 0x040039B7 RID: 14775
			public static Func<PID, bool> <>9__24_1;

			// Token: 0x040039B8 RID: 14776
			public static Func<PID, CalculatedPIDV2> <>9__24_2;

			// Token: 0x040039B9 RID: 14777
			public static Func<CalculatedPIDV2, bool> <>9__24_3;

			// Token: 0x040039BA RID: 14778
			public static Func<CalculatedPIDV2, bool> <>9__24_4;

			// Token: 0x040039BB RID: 14779
			public static Func<CalculatedPIDV2, bool> <>9__24_5;

			// Token: 0x040039BC RID: 14780
			public static Func<CalculatedPIDV2, bool> <>9__24_6;

			// Token: 0x040039BD RID: 14781
			public static Func<CalculatedPIDV2, bool> <>9__24_7;

			// Token: 0x040039BE RID: 14782
			public static Func<CalculatedPIDV2, bool> <>9__24_8;

			// Token: 0x040039BF RID: 14783
			public static Func<PID, bool> <>9__24_9;

			// Token: 0x040039C0 RID: 14784
			public static Func<PID, CarPlayDashboardItem> <>9__24_10;
		}
	}
}
