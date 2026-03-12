using System;
using System.Globalization;
using System.Runtime.CompilerServices;
using Xamarin.Forms;

namespace CarScannerXamarinForms.Common.XAMLConverters
{
	// Token: 0x02000814 RID: 2068
	internal class BoolToStringsVariantsConverter : IValueConverter
	{
		// Token: 0x1700163E RID: 5694
		// (get) Token: 0x060047D1 RID: 18385 RVA: 0x0036F8A4 File Offset: 0x0036DAA4
		// (set) Token: 0x060047D2 RID: 18386 RVA: 0x0036F8AC File Offset: 0x0036DAAC
		public string TrueValue
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
		} = true.ToString();

		// Token: 0x1700163F RID: 5695
		// (get) Token: 0x060047D3 RID: 18387 RVA: 0x0036F8B5 File Offset: 0x0036DAB5
		// (set) Token: 0x060047D4 RID: 18388 RVA: 0x0036F8BD File Offset: 0x0036DABD
		public string FalseValue
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
		} = false.ToString();

		// Token: 0x060047D5 RID: 18389 RVA: 0x0036F8C8 File Offset: 0x0036DAC8
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

		// Token: 0x060047D6 RID: 18390 RVA: 0x00017A6F File Offset: 0x00015C6F
		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}

		// Token: 0x060047D7 RID: 18391 RVA: 0x0036F8FC File Offset: 0x0036DAFC
		public BoolToStringsVariantsConverter()
		{
		}

		// Token: 0x040029D6 RID: 10710
		[CompilerGenerated]
		private string <TrueValue>k__BackingField;

		// Token: 0x040029D7 RID: 10711
		[CompilerGenerated]
		private string <FalseValue>k__BackingField;
	}
}
