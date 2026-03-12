using System;
using System.Globalization;
using System.Text;
using CarScannerXamarinForms.Settings;
using CarScannerXamarinForms.ViewModels;
using Xamarin.Forms;

namespace CarScannerXamarinForms
{
	// Token: 0x0200007C RID: 124
	public class ModelFloatValueToTextConverter : IValueConverter
	{
		// Token: 0x06000294 RID: 660 RVA: 0x0001869C File Offset: 0x0001689C
		public ModelFloatValueToTextConverter(LiveDataPIDModel model)
		{
			this.model = model;
		}

		// Token: 0x06000295 RID: 661 RVA: 0x000186AC File Offset: 0x000168AC
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			if (this.model != null && this.model.SelectedPID != null)
			{
				StringBuilder stringBuilder;
				if (SharedSettings.Current.ChartShowAverageValue)
				{
					stringBuilder = new StringBuilder(7);
					stringBuilder.Append(this.model.SelectedPID.ShortName);
					stringBuilder.Append(" (");
					stringBuilder.Append(this.model.TextValue);
					stringBuilder.Append("/~");
					stringBuilder.Append(this.model.AverageTextValue);
					stringBuilder.Append(this.model.Units);
					stringBuilder.Append(")");
				}
				else
				{
					stringBuilder = new StringBuilder(5);
					stringBuilder.Append(this.model.SelectedPID.ShortName);
					stringBuilder.Append(" (");
					stringBuilder.Append(this.model.TextValue);
					stringBuilder.Append(this.model.Units);
					stringBuilder.Append(")");
				}
				return stringBuilder.ToString();
			}
			return "";
		}

		// Token: 0x06000296 RID: 662 RVA: 0x00017A6F File Offset: 0x00015C6F
		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}

		// Token: 0x040001AB RID: 427
		private LiveDataPIDModel model;
	}
}
