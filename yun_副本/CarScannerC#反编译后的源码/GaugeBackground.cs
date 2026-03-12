using System;
using System.CodeDom.Compiler;
using System.Reflection;
using System.Runtime.CompilerServices;
using CarScannerXamarinForms.Dashboard;
using Syncfusion.XForms.Graphics;
using Xamarin.Forms;
using Xamarin.Forms.Internals;
using Xamarin.Forms.Xaml;
using Xamarin.Forms.Xaml.Diagnostics;
using Xamarin.Forms.Xaml.Internals;

namespace CarScannerXamarinForms
{
	// Token: 0x0200008B RID: 139
	[XamlFilePath("Dashboard\\DashboardGauges\\GaugeBackground.xaml")]
	public class GaugeBackground : ContentView
	{
		// Token: 0x060002C2 RID: 706 RVA: 0x00018F21 File Offset: 0x00017121
		public GaugeBackground()
		{
			this.InitializeComponent();
		}

		// Token: 0x060002C3 RID: 707 RVA: 0x00018F30 File Offset: 0x00017130
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private void InitializeComponent()
		{
			if (ResourceLoader.IsEnabled && ResourceLoader.CanProvideContentFor(new ResourceLoader.ResourceLoadingQuery
			{
				AssemblyName = typeof(GaugeBackground).GetTypeInfo().Assembly.GetName(),
				ResourcePath = "Dashboard/DashboardGauges/GaugeBackground.xaml",
				Instance = this
			}))
			{
				this.__InitComponentRuntime();
				return;
			}
			if (XamlLoader.XamlFileProvider != null && XamlLoader.XamlFileProvider(base.GetType()) != null)
			{
				this.__InitComponentRuntime();
				return;
			}
			BindingExtension bindingExtension;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension = new BindingExtension(), new Uri("Dashboard\\DashboardGauges\\GaugeBackground.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 19, 51);
			BindingExtension bindingExtension2;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension2 = new BindingExtension(), new Uri("Dashboard\\DashboardGauges\\GaugeBackground.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 19, 106);
			BindingExtension bindingExtension3;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension3 = new BindingExtension(), new Uri("Dashboard\\DashboardGauges\\GaugeBackground.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 21, 52);
			BindingExtension bindingExtension4;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension4 = new BindingExtension(), new Uri("Dashboard\\DashboardGauges\\GaugeBackground.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 21, 105);
			SfGradientStop sfGradientStop;
			VisualDiagnostics.RegisterSourceInfo(sfGradientStop = new SfGradientStop(), new Uri("Dashboard\\DashboardGauges\\GaugeBackground.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 21, 26);
			BindingExtension bindingExtension5;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension5 = new BindingExtension(), new Uri("Dashboard\\DashboardGauges\\GaugeBackground.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 22, 52);
			BindingExtension bindingExtension6;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension6 = new BindingExtension(), new Uri("Dashboard\\DashboardGauges\\GaugeBackground.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 22, 105);
			SfGradientStop sfGradientStop2;
			VisualDiagnostics.RegisterSourceInfo(sfGradientStop2 = new SfGradientStop(), new Uri("Dashboard\\DashboardGauges\\GaugeBackground.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 22, 26);
			SfLinearGradientBrush sfLinearGradientBrush;
			VisualDiagnostics.RegisterSourceInfo(sfLinearGradientBrush = new SfLinearGradientBrush(), new Uri("Dashboard\\DashboardGauges\\GaugeBackground.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 19, 18);
			SfGradientView sfGradientView;
			VisualDiagnostics.RegisterSourceInfo(sfGradientView = new SfGradientView(), new Uri("Dashboard\\DashboardGauges\\GaugeBackground.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 17, 10);
			VisualDiagnostics.RegisterSourceInfo(this, new Uri("Dashboard\\DashboardGauges\\GaugeBackground.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 2, 2);
			NameScope nameScope = NameScope.GetNameScope(this) ?? new NameScope();
			NameScope.SetNameScope(this, nameScope);
			this.SetValue(View.MarginProperty, new Thickness(0.0));
			this.SetValue(Layout.PaddingProperty, new Thickness(0.0));
			bindingExtension.Mode = 2;
			bindingExtension.Path = "GradientStartPoint";
			bindingExtension.TypedBinding = new TypedBinding<DashboardItem, Point>(delegate(DashboardItem A_0)
			{
				if (A_0 != null)
				{
					return new ValueTuple<Point, bool>(A_0.GradientStartPoint, true);
				}
				return default(ValueTuple<Point, bool>);
			}, null, new Tuple<Func<DashboardItem, object>, string>[]
			{
				new Tuple<Func<DashboardItem, object>, string>((DashboardItem A_0) => A_0, "GradientStartPoint")
			});
			BindingBase bindingBase = bindingExtension.ProvideValue(null);
			sfLinearGradientBrush.SetBinding(SfLinearGradientBrush.StartPointProperty, bindingBase);
			bindingExtension2.Mode = 2;
			bindingExtension2.Path = "GradientEndPoint";
			bindingExtension2.TypedBinding = new TypedBinding<DashboardItem, Point>(delegate(DashboardItem A_0)
			{
				if (A_0 != null)
				{
					return new ValueTuple<Point, bool>(A_0.GradientEndPoint, true);
				}
				return default(ValueTuple<Point, bool>);
			}, null, new Tuple<Func<DashboardItem, object>, string>[]
			{
				new Tuple<Func<DashboardItem, object>, string>((DashboardItem A_0) => A_0, "GradientEndPoint")
			});
			BindingBase bindingBase2 = bindingExtension2.ProvideValue(null);
			sfLinearGradientBrush.SetBinding(SfLinearGradientBrush.EndPointProperty, bindingBase2);
			bindingExtension3.Mode = 2;
			bindingExtension3.Path = "GradientOffsetPoint1";
			BindingBase bindingBase3 = bindingExtension3.ProvideValue(null);
			sfGradientStop.SetBinding(SfGradientStop.OffsetProperty, bindingBase3);
			bindingExtension4.Mode = 2;
			bindingExtension4.Path = "GradientColor1";
			BindingBase bindingBase4 = bindingExtension4.ProvideValue(null);
			sfGradientStop.SetBinding(SfGradientStop.ColorProperty, bindingBase4);
			sfLinearGradientBrush.GetValue(SfGradientBrush.GradientStopsProperty).Add(sfGradientStop);
			bindingExtension5.Mode = 2;
			bindingExtension5.Path = "GradientOffsetPoint2";
			BindingBase bindingBase5 = bindingExtension5.ProvideValue(null);
			sfGradientStop2.SetBinding(SfGradientStop.OffsetProperty, bindingBase5);
			bindingExtension6.Mode = 2;
			bindingExtension6.Path = "GradientColor2";
			BindingBase bindingBase6 = bindingExtension6.ProvideValue(null);
			sfGradientStop2.SetBinding(SfGradientStop.ColorProperty, bindingBase6);
			sfLinearGradientBrush.GetValue(SfGradientBrush.GradientStopsProperty).Add(sfGradientStop2);
			sfGradientView.SetValue(SfGradientView.BackgroundBrushProperty, sfLinearGradientBrush);
			this.SetValue(ContentView.ContentProperty, sfGradientView);
		}

		// Token: 0x060002C4 RID: 708 RVA: 0x0001937F File Offset: 0x0001757F
		private void __InitComponentRuntime()
		{
			Extensions.LoadFromXaml<GaugeBackground>(this, typeof(GaugeBackground));
		}

		// Token: 0x060002C5 RID: 709 RVA: 0x00019394 File Offset: 0x00017594
		[CompilerGenerated]
		private static ValueTuple<Point, bool> <InitializeComponent>typedBindingsM__396(DashboardItem A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<Point, bool>(A_0.GradientStartPoint, true);
			}
			return default(ValueTuple<Point, bool>);
		}

		// Token: 0x060002C6 RID: 710 RVA: 0x000193C4 File Offset: 0x000175C4
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__397(DashboardItem A_0)
		{
			return A_0;
		}

		// Token: 0x060002C7 RID: 711 RVA: 0x000193D4 File Offset: 0x000175D4
		[CompilerGenerated]
		private static ValueTuple<Point, bool> <InitializeComponent>typedBindingsM__398(DashboardItem A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<Point, bool>(A_0.GradientEndPoint, true);
			}
			return default(ValueTuple<Point, bool>);
		}

		// Token: 0x060002C8 RID: 712 RVA: 0x00019404 File Offset: 0x00017604
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__399(DashboardItem A_0)
		{
			return A_0;
		}
	}
}
