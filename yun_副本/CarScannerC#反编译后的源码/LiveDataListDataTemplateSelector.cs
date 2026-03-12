using System;
using System.Runtime.CompilerServices;
using Xamarin.Forms;

namespace CarScannerXamarinForms
{
	// Token: 0x02000054 RID: 84
	public class LiveDataListDataTemplateSelector : DataTemplateSelector
	{
		// Token: 0x060001F5 RID: 501 RVA: 0x00016A2D File Offset: 0x00014C2D
		public LiveDataListDataTemplateSelector()
		{
		}

		// Token: 0x17000047 RID: 71
		// (get) Token: 0x060001F6 RID: 502 RVA: 0x00016A35 File Offset: 0x00014C35
		// (set) Token: 0x060001F7 RID: 503 RVA: 0x00016A3D File Offset: 0x00014C3D
		public DataTemplate MobileTemplate
		{
			[CompilerGenerated]
			get
			{
				return this.<MobileTemplate>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<MobileTemplate>k__BackingField = value;
			}
		}

		// Token: 0x17000048 RID: 72
		// (get) Token: 0x060001F8 RID: 504 RVA: 0x00016A46 File Offset: 0x00014C46
		// (set) Token: 0x060001F9 RID: 505 RVA: 0x00016A4E File Offset: 0x00014C4E
		public DataTemplate TabletTemplate
		{
			[CompilerGenerated]
			get
			{
				return this.<TabletTemplate>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<TabletTemplate>k__BackingField = value;
			}
		}

		// Token: 0x060001FA RID: 506 RVA: 0x00016A57 File Offset: 0x00014C57
		protected override DataTemplate OnSelectTemplate(object item, BindableObject container)
		{
			if (Device.Idiom == 2)
			{
				return this.TabletTemplate;
			}
			return this.MobileTemplate;
		}

		// Token: 0x04000195 RID: 405
		[CompilerGenerated]
		private DataTemplate <MobileTemplate>k__BackingField;

		// Token: 0x04000196 RID: 406
		[CompilerGenerated]
		private DataTemplate <TabletTemplate>k__BackingField;
	}
}
