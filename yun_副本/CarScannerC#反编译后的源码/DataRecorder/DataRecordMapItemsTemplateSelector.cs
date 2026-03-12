using System;
using System.Runtime.CompilerServices;
using Xamarin.Forms;

namespace CarScannerXamarinForms.DataRecorder
{
	// Token: 0x020006E8 RID: 1768
	internal class DataRecordMapItemsTemplateSelector : DataTemplateSelector
	{
		// Token: 0x170013E0 RID: 5088
		// (get) Token: 0x06003C3D RID: 15421 RVA: 0x00318878 File Offset: 0x00316A78
		// (set) Token: 0x06003C3E RID: 15422 RVA: 0x00318880 File Offset: 0x00316A80
		public DataTemplate MultiValuesTemplate
		{
			[CompilerGenerated]
			get
			{
				return this.<MultiValuesTemplate>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<MultiValuesTemplate>k__BackingField = value;
			}
		}

		// Token: 0x170013E1 RID: 5089
		// (get) Token: 0x06003C3F RID: 15423 RVA: 0x00318889 File Offset: 0x00316A89
		// (set) Token: 0x06003C40 RID: 15424 RVA: 0x00318891 File Offset: 0x00316A91
		public DataTemplate SingleValueTemplate
		{
			[CompilerGenerated]
			get
			{
				return this.<SingleValueTemplate>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<SingleValueTemplate>k__BackingField = value;
			}
		}

		// Token: 0x06003C41 RID: 15425 RVA: 0x0031889C File Offset: 0x00316A9C
		protected override DataTemplate OnSelectTemplate(object item, BindableObject container)
		{
			DataRecordForMapsWithGeoposition dataRecordForMapsWithGeoposition = item as DataRecordForMapsWithGeoposition;
			if (dataRecordForMapsWithGeoposition == null)
			{
				return this.SingleValueTemplate;
			}
			if (dataRecordForMapsWithGeoposition.Count > 1)
			{
				return this.MultiValuesTemplate;
			}
			return this.SingleValueTemplate;
		}

		// Token: 0x06003C42 RID: 15426 RVA: 0x00016A2D File Offset: 0x00014C2D
		public DataRecordMapItemsTemplateSelector()
		{
		}

		// Token: 0x040024E0 RID: 9440
		[CompilerGenerated]
		private DataTemplate <MultiValuesTemplate>k__BackingField;

		// Token: 0x040024E1 RID: 9441
		[CompilerGenerated]
		private DataTemplate <SingleValueTemplate>k__BackingField;
	}
}
