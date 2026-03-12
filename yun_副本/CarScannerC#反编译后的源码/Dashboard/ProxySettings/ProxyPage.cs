using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using CarScannerXamarinForms.Dashboard.DashboardPages;

namespace CarScannerXamarinForms.Dashboard.ProxySettings
{
	// Token: 0x02000779 RID: 1913
	public class ProxyPage
	{
		// Token: 0x060041E0 RID: 16864 RVA: 0x0033A8D7 File Offset: 0x00338AD7
		public ProxyPage()
		{
			this.Items = new List<ProxyItem>();
		}

		// Token: 0x060041E1 RID: 16865 RVA: 0x0033A8EC File Offset: 0x00338AEC
		public ProxyPage(DashboardPage dashboardPage)
		{
			this.DashboardType = dashboardPage.DashboardType;
			this.Items = new List<ProxyItem>(dashboardPage.Items.Count);
			foreach (DashboardItem dashboardItem in dashboardPage.Items)
			{
				ProxyItem proxyItem = new ProxyItem(dashboardItem);
				this.Items.Add(proxyItem);
			}
			this.Title = dashboardPage.Title;
			this.UpdateInBackground = dashboardPage.UpdateInBackground;
		}

		// Token: 0x17001587 RID: 5511
		// (get) Token: 0x060041E2 RID: 16866 RVA: 0x0033A984 File Offset: 0x00338B84
		// (set) Token: 0x060041E3 RID: 16867 RVA: 0x0033A98C File Offset: 0x00338B8C
		public int ItemsCount
		{
			[CompilerGenerated]
			get
			{
				return this.<ItemsCount>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<ItemsCount>k__BackingField = value;
			}
		}

		// Token: 0x17001588 RID: 5512
		// (get) Token: 0x060041E4 RID: 16868 RVA: 0x0033A995 File Offset: 0x00338B95
		// (set) Token: 0x060041E5 RID: 16869 RVA: 0x0033A99D File Offset: 0x00338B9D
		public List<ProxyItem> Items
		{
			[CompilerGenerated]
			get
			{
				return this.<Items>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<Items>k__BackingField = value;
			}
		}

		// Token: 0x17001589 RID: 5513
		// (get) Token: 0x060041E6 RID: 16870 RVA: 0x0033A9A6 File Offset: 0x00338BA6
		// (set) Token: 0x060041E7 RID: 16871 RVA: 0x0033A9AE File Offset: 0x00338BAE
		public string Title
		{
			[CompilerGenerated]
			get
			{
				return this.<Title>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<Title>k__BackingField = value;
			}
		}

		// Token: 0x1700158A RID: 5514
		// (get) Token: 0x060041E8 RID: 16872 RVA: 0x0033A9B7 File Offset: 0x00338BB7
		// (set) Token: 0x060041E9 RID: 16873 RVA: 0x0033A9BF File Offset: 0x00338BBF
		public DashboardTypes DashboardType
		{
			[CompilerGenerated]
			get
			{
				return this.<DashboardType>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<DashboardType>k__BackingField = value;
			}
		}

		// Token: 0x1700158B RID: 5515
		// (get) Token: 0x060041EA RID: 16874 RVA: 0x0033A9C8 File Offset: 0x00338BC8
		// (set) Token: 0x060041EB RID: 16875 RVA: 0x0033A9D0 File Offset: 0x00338BD0
		public bool UpdateInBackground
		{
			[CompilerGenerated]
			get
			{
				return this.<UpdateInBackground>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<UpdateInBackground>k__BackingField = value;
			}
		}

		// Token: 0x060041EC RID: 16876 RVA: 0x0033A9DC File Offset: 0x00338BDC
		public static DashboardPage GetPageFromProxyPage(ProxyPage proxyPage)
		{
			DashboardPage dashboardPageFromDashboardType = DashboardPage.GetDashboardPageFromDashboardType(proxyPage.DashboardType);
			if (dashboardPageFromDashboardType is Dash_CustomPage)
			{
				foreach (ProxyItem proxyItem in proxyPage.Items)
				{
					try
					{
						DashboardItem dashboardItem = new DashboardItem();
						proxyItem.ApplySettingsToRealItem(dashboardItem, false);
						dashboardPageFromDashboardType.Items.Add(dashboardItem);
					}
					catch (Exception)
					{
					}
				}
				try
				{
					dashboardPageFromDashboardType.CreateGrid();
					goto IL_00C0;
				}
				catch (Exception)
				{
					goto IL_00C0;
				}
			}
			dashboardPageFromDashboardType.CreateGrid();
			for (int i = 0; i < dashboardPageFromDashboardType.Items.Count; i++)
			{
				try
				{
					proxyPage.Items[i].ApplySettingsToRealItem(dashboardPageFromDashboardType.Items[i], false);
				}
				catch (Exception)
				{
				}
			}
			try
			{
				dashboardPageFromDashboardType.RebuildItems();
			}
			catch (Exception)
			{
			}
			IL_00C0:
			dashboardPageFromDashboardType.Title = proxyPage.Title;
			dashboardPageFromDashboardType.UpdateInBackground = proxyPage.UpdateInBackground;
			return dashboardPageFromDashboardType;
		}

		// Token: 0x060041ED RID: 16877 RVA: 0x0033AB04 File Offset: 0x00338D04
		public void ApplyToPage(DashboardPage page)
		{
			if (page.Items.Count == 0)
			{
				page.CreateGrid();
			}
			for (int i = 0; i < page.Items.Count; i++)
			{
				this.Items[i].ApplySettingsToRealItem(page.Items[i], false);
				page.Items[i].SelectAndAddControl();
			}
			page.Title = this.Title;
			page.UpdateInBackground = this.UpdateInBackground;
		}

		// Token: 0x0400286E RID: 10350
		[CompilerGenerated]
		private int <ItemsCount>k__BackingField;

		// Token: 0x0400286F RID: 10351
		[CompilerGenerated]
		private List<ProxyItem> <Items>k__BackingField;

		// Token: 0x04002870 RID: 10352
		[CompilerGenerated]
		private string <Title>k__BackingField;

		// Token: 0x04002871 RID: 10353
		[CompilerGenerated]
		private DashboardTypes <DashboardType>k__BackingField;

		// Token: 0x04002872 RID: 10354
		[CompilerGenerated]
		private bool <UpdateInBackground>k__BackingField;
	}
}
