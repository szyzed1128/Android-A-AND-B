using System;
using System.Collections.ObjectModel;

namespace Xamarin.Forms.Essentials.Controls
{
	// Token: 0x02000043 RID: 67
	public class GradientLayer : ContentView
	{
		// Token: 0x17000036 RID: 54
		// (get) Token: 0x0600019C RID: 412 RVA: 0x0000AD41 File Offset: 0x00008F41
		// (set) Token: 0x0600019D RID: 413 RVA: 0x0000AD53 File Offset: 0x00008F53
		public Point StartPoint
		{
			get
			{
				return (Point)base.GetValue(GradientLayer.StartPointProperty);
			}
			set
			{
				base.SetValue(GradientLayer.StartPointProperty, value);
			}
		}

		// Token: 0x17000037 RID: 55
		// (get) Token: 0x0600019E RID: 414 RVA: 0x0000AD66 File Offset: 0x00008F66
		// (set) Token: 0x0600019F RID: 415 RVA: 0x0000AD78 File Offset: 0x00008F78
		public Point EndPoint
		{
			get
			{
				return (Point)base.GetValue(GradientLayer.EndPointProperty);
			}
			set
			{
				base.SetValue(GradientLayer.EndPointProperty, value);
			}
		}

		// Token: 0x17000038 RID: 56
		// (get) Token: 0x060001A0 RID: 416 RVA: 0x0000AD8B File Offset: 0x00008F8B
		// (set) Token: 0x060001A1 RID: 417 RVA: 0x0000AD9D File Offset: 0x00008F9D
		public ObservableCollection<GradientColor> GradientColors
		{
			get
			{
				return (ObservableCollection<GradientColor>)base.GetValue(GradientLayer.GradientColorsProperty);
			}
			set
			{
				base.SetValue(GradientLayer.GradientColorsProperty, value);
			}
		}

		// Token: 0x060001A2 RID: 418 RVA: 0x0000ADAB File Offset: 0x00008FAB
		public GradientLayer()
		{
		}

		// Token: 0x060001A3 RID: 419 RVA: 0x0000ADB4 File Offset: 0x00008FB4
		// Note: this type is marked as 'beforefieldinit'.
		static GradientLayer()
		{
		}

		// Token: 0x04000170 RID: 368
		public static BindableProperty GradientColorsProperty = BindableProperty.Create("GradientColors", typeof(ObservableCollection<GradientColor>), typeof(GradientLayer), new ObservableCollection<GradientColor>(), 1, null, null, null, null, null);

		// Token: 0x04000171 RID: 369
		public static BindableProperty StartPointProperty = BindableProperty.Create("StartPoint", typeof(Point), typeof(GradientLayer), new Point(0.0, 0.0), 1, null, null, null, null, null);

		// Token: 0x04000172 RID: 370
		public static BindableProperty EndPointProperty = BindableProperty.Create("EndPoint", typeof(Point), typeof(GradientLayer), new Point(0.0, 0.0), 1, null, null, null, null, null);
	}
}
