using System;
using System.Runtime.CompilerServices;
using CarScannerXamarinForms.Dashboard.DashboardPages;
using Xamarin.Forms;

namespace CarScannerXamarinForms.Pages.Dashboard
{
	// Token: 0x020006C4 RID: 1732
	internal class DashboardPageSettingsPopupDataTemplateSelector : DataTemplateSelector
	{
		// Token: 0x17001399 RID: 5017
		// (get) Token: 0x06003B11 RID: 15121 RVA: 0x003125B1 File Offset: 0x003107B1
		// (set) Token: 0x06003B12 RID: 15122 RVA: 0x003125B9 File Offset: 0x003107B9
		public DataTemplate CustomPageTemplate
		{
			[CompilerGenerated]
			get
			{
				return this.<CustomPageTemplate>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<CustomPageTemplate>k__BackingField = value;
			}
		}

		// Token: 0x1700139A RID: 5018
		// (get) Token: 0x06003B13 RID: 15123 RVA: 0x003125C2 File Offset: 0x003107C2
		// (set) Token: 0x06003B14 RID: 15124 RVA: 0x003125CA File Offset: 0x003107CA
		public DataTemplate TemplatedPageTemplate
		{
			[CompilerGenerated]
			get
			{
				return this.<TemplatedPageTemplate>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<TemplatedPageTemplate>k__BackingField = value;
			}
		}

		// Token: 0x06003B15 RID: 15125 RVA: 0x003125D4 File Offset: 0x003107D4
		protected override DataTemplate OnSelectTemplate(object item, BindableObject container)
		{
			DataTemplate dataTemplate;
			try
			{
				DashboardXamlPage instance = DashboardXamlPage.Instance;
				if (instance.Pages[instance.CurrentPage] is Dash_CustomPage)
				{
					dataTemplate = this.CustomPageTemplate;
				}
				else
				{
					dataTemplate = this.TemplatedPageTemplate;
				}
			}
			catch (Exception)
			{
				dataTemplate = this.TemplatedPageTemplate;
			}
			return dataTemplate;
		}

		// Token: 0x06003B16 RID: 15126 RVA: 0x00016A2D File Offset: 0x00014C2D
		public DashboardPageSettingsPopupDataTemplateSelector()
		{
		}

		// Token: 0x0400242A RID: 9258
		[CompilerGenerated]
		private DataTemplate <CustomPageTemplate>k__BackingField;

		// Token: 0x0400242B RID: 9259
		[CompilerGenerated]
		private DataTemplate <TemplatedPageTemplate>k__BackingField;
	}
}
