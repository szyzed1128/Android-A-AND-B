using System;
using System.Runtime.CompilerServices;

namespace CarScannerXamarinForms.Dashboard.ProxySettings
{
	// Token: 0x02000777 RID: 1911
	public class DescriptionPage
	{
		// Token: 0x06004135 RID: 16693 RVA: 0x00002050 File Offset: 0x00000250
		public DescriptionPage()
		{
		}

		// Token: 0x06004136 RID: 16694 RVA: 0x003395D6 File Offset: 0x003377D6
		public DescriptionPage(string Title, string PreviewFile, DashboardTypes DashboardType)
		{
			this.Title = Title;
			this.PreviewFile = PreviewFile;
			this.DashboardType = DashboardType;
		}

		// Token: 0x17001535 RID: 5429
		// (get) Token: 0x06004137 RID: 16695 RVA: 0x003395F3 File Offset: 0x003377F3
		// (set) Token: 0x06004138 RID: 16696 RVA: 0x003395FB File Offset: 0x003377FB
		public string PreviewFile
		{
			[CompilerGenerated]
			get
			{
				return this.<PreviewFile>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<PreviewFile>k__BackingField = value;
			}
		}

		// Token: 0x17001536 RID: 5430
		// (get) Token: 0x06004139 RID: 16697 RVA: 0x00339604 File Offset: 0x00337804
		// (set) Token: 0x0600413A RID: 16698 RVA: 0x0033960C File Offset: 0x0033780C
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

		// Token: 0x17001537 RID: 5431
		// (get) Token: 0x0600413B RID: 16699 RVA: 0x00339615 File Offset: 0x00337815
		// (set) Token: 0x0600413C RID: 16700 RVA: 0x0033961D File Offset: 0x0033781D
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

		// Token: 0x17001538 RID: 5432
		// (get) Token: 0x0600413D RID: 16701 RVA: 0x00339626 File Offset: 0x00337826
		public int PlacesCount
		{
			get
			{
				if (this._PlacesCount == 0)
				{
					this._PlacesCount = DashboardPage.GetDashboardPageFromDashboardType(this.DashboardType).ItemsCount;
				}
				return this._PlacesCount;
			}
		}

		// Token: 0x0400281C RID: 10268
		[CompilerGenerated]
		private string <PreviewFile>k__BackingField;

		// Token: 0x0400281D RID: 10269
		[CompilerGenerated]
		private string <Title>k__BackingField;

		// Token: 0x0400281E RID: 10270
		[CompilerGenerated]
		private DashboardTypes <DashboardType>k__BackingField;

		// Token: 0x0400281F RID: 10271
		private int _PlacesCount;
	}
}
