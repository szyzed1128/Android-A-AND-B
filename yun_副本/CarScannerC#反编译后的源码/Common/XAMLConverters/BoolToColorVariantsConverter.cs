using System;
using System.Globalization;
using System.Runtime.CompilerServices;
using Xamarin.Forms;

namespace CarScannerXamarinForms.Common.XAMLConverters
{
	// Token: 0x02000813 RID: 2067
	internal class BoolToColorVariantsConverter : IValueConverter
	{
		// Token: 0x1700163C RID: 5692
		// (get) Token: 0x060047CA RID: 18378 RVA: 0x0036F81F File Offset: 0x0036DA1F
		// (set) Token: 0x060047CB RID: 18379 RVA: 0x0036F827 File Offset: 0x0036DA27
		public Color TrueValue
		{
			[CompilerGenerated]
			get
			{
				return this.<TrueValue>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<TrueValue>k__BackingField = value;
			}
		} = Color.Green;

		// Token: 0x1700163D RID: 5693
		// (get) Token: 0x060047CC RID: 18380 RVA: 0x0036F830 File Offset: 0x0036DA30
		// (set) Token: 0x060047CD RID: 18381 RVA: 0x0036F838 File Offset: 0x0036DA38
		public Color FalseValue
		{
			[CompilerGenerated]
			get
			{
				return this.<FalseValue>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<FalseValue>k__BackingField = value;
			}
		} = Color.Red;

		// Token: 0x060047CE RID: 18382 RVA: 0x0036F844 File Offset: 0x0036DA44
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			if (!(value is bool))
			{
				return this.FalseValue;
			}
			bool flag = (bool)value;
			if (flag)
			{
				return this.TrueValue;
			}
			return this.FalseValue;
		}

		// Token: 0x060047CF RID: 18383 RVA: 0x00017A6F File Offset: 0x00015C6F
		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}

		// Token: 0x060047D0 RID: 18384 RVA: 0x0036F886 File Offset: 0x0036DA86
		public BoolToColorVariantsConverter()
		{
		}

		// Token: 0x040029D4 RID: 10708
		[CompilerGenerated]
		private Color <TrueValue>k__BackingField;

		// Token: 0x040029D5 RID: 10709
		[CompilerGenerated]
		private Color <FalseValue>k__BackingField;
	}
}
