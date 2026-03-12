using System;
using System.CodeDom.Compiler;
using System.Reflection;
using System.Runtime.CompilerServices;
using CarScannerXamarinForms.Common.XAMLConverters;
using CarScannerXamarinForms.Settings;
using CarScannerXamarinForms.ViewModels;
using Syncfusion.XForms.ProgressBar;
using Xamarin.Forms;
using Xamarin.Forms.Internals;
using Xamarin.Forms.Xaml;
using Xamarin.Forms.Xaml.Diagnostics;
using Xamarin.Forms.Xaml.Internals;

namespace CarScannerXamarinForms.Dashboard.DashboardGauges
{
	// Token: 0x020007AA RID: 1962
	[XamlFilePath("Dashboard\\DashboardGauges\\CircularProgressItem.xaml")]
	public class CircularProgressItem : ContentView
	{
		// Token: 0x0600436C RID: 17260 RVA: 0x00343D13 File Offset: 0x00341F13
		public CircularProgressItem()
		{
			this.InitializeComponent();
		}

		// Token: 0x0600436D RID: 17261 RVA: 0x00343D24 File Offset: 0x00341F24
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private void InitializeComponent()
		{
			if (ResourceLoader.IsEnabled && ResourceLoader.CanProvideContentFor(new ResourceLoader.ResourceLoadingQuery
			{
				AssemblyName = typeof(CircularProgressItem).GetTypeInfo().Assembly.GetName(),
				ResourcePath = "Dashboard/DashboardGauges/CircularProgressItem.xaml",
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
			ExcludeInfinityFromDoubleConverter excludeInfinityFromDoubleConverter;
			VisualDiagnostics.RegisterSourceInfo(excludeInfinityFromDoubleConverter = new ExcludeInfinityFromDoubleConverter(), new Uri("Dashboard\\DashboardGauges\\CircularProgressItem.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 16, 14);
			BoolToNegativeConverter boolToNegativeConverter;
			VisualDiagnostics.RegisterSourceInfo(boolToNegativeConverter = new BoolToNegativeConverter(), new Uri("Dashboard\\DashboardGauges\\CircularProgressItem.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 17, 14);
			DoubleToPercentConverter doubleToPercentConverter;
			VisualDiagnostics.RegisterSourceInfo(doubleToPercentConverter = new DoubleToPercentConverter(), new Uri("Dashboard\\DashboardGauges\\CircularProgressItem.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 18, 14);
			FloatItemToLineBreakModeConverter floatItemToLineBreakModeConverter;
			VisualDiagnostics.RegisterSourceInfo(floatItemToLineBreakModeConverter = new FloatItemToLineBreakModeConverter(), new Uri("Dashboard\\DashboardGauges\\CircularProgressItem.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 19, 14);
			BoolToSFGaugeOrientationConverter boolToSFGaugeOrientationConverter;
			VisualDiagnostics.RegisterSourceInfo(boolToSFGaugeOrientationConverter = new BoolToSFGaugeOrientationConverter(), new Uri("Dashboard\\DashboardGauges\\CircularProgressItem.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 20, 14);
			ProgressBarWidthToPaddingConverter progressBarWidthToPaddingConverter;
			VisualDiagnostics.RegisterSourceInfo(progressBarWidthToPaddingConverter = new ProgressBarWidthToPaddingConverter(), new Uri("Dashboard\\DashboardGauges\\CircularProgressItem.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 21, 14);
			TransparentColorToFalseConverter transparentColorToFalseConverter;
			VisualDiagnostics.RegisterSourceInfo(transparentColorToFalseConverter = new TransparentColorToFalseConverter(), new Uri("Dashboard\\DashboardGauges\\CircularProgressItem.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 22, 14);
			CornerRadiusToThicknessConverter cornerRadiusToThicknessConverter;
			VisualDiagnostics.RegisterSourceInfo(cornerRadiusToThicknessConverter = new CornerRadiusToThicknessConverter(), new Uri("Dashboard\\DashboardGauges\\CircularProgressItem.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 23, 14);
			ResourceDictionary resourceDictionary;
			VisualDiagnostics.RegisterSourceInfo(resourceDictionary = new ResourceDictionary(), new Uri("Dashboard\\DashboardGauges\\CircularProgressItem.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 15, 10);
			RowDefinition rowDefinition;
			VisualDiagnostics.RegisterSourceInfo(rowDefinition = new RowDefinition(), new Uri("Dashboard\\DashboardGauges\\CircularProgressItem.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 37, 18);
			BindingExtension bindingExtension;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension = new BindingExtension(), new Uri("Dashboard\\DashboardGauges\\CircularProgressItem.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 41, 49);
			GaugeBackground gaugeBackground;
			VisualDiagnostics.RegisterSourceInfo(gaugeBackground = new GaugeBackground(), new Uri("Dashboard\\DashboardGauges\\CircularProgressItem.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 41, 14);
			StaticResourceExtension staticResourceExtension;
			VisualDiagnostics.RegisterSourceInfo(staticResourceExtension = new StaticResourceExtension(), new Uri("Dashboard\\DashboardGauges\\CircularProgressItem.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 44, 17);
			BindingExtension bindingExtension2;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension2 = new BindingExtension(), new Uri("Dashboard\\DashboardGauges\\CircularProgressItem.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 44, 17);
			StaticResourceExtension staticResourceExtension2;
			VisualDiagnostics.RegisterSourceInfo(staticResourceExtension2 = new StaticResourceExtension(), new Uri("Dashboard\\DashboardGauges\\CircularProgressItem.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 46, 17);
			BindingExtension bindingExtension3;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension3 = new BindingExtension(), new Uri("Dashboard\\DashboardGauges\\CircularProgressItem.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 46, 17);
			BindingExtension bindingExtension4;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension4 = new BindingExtension(), new Uri("Dashboard\\DashboardGauges\\CircularProgressItem.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 48, 17);
			BoxView boxView;
			VisualDiagnostics.RegisterSourceInfo(boxView = new BoxView(), new Uri("Dashboard\\DashboardGauges\\CircularProgressItem.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 42, 14);
			BindingExtension bindingExtension5;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension5 = new BindingExtension(), new Uri("Dashboard\\DashboardGauges\\CircularProgressItem.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 60, 17);
			BindingExtension bindingExtension6;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension6 = new BindingExtension(), new Uri("Dashboard\\DashboardGauges\\CircularProgressItem.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 61, 17);
			StaticResourceExtension staticResourceExtension3;
			VisualDiagnostics.RegisterSourceInfo(staticResourceExtension3 = new StaticResourceExtension(), new Uri("Dashboard\\DashboardGauges\\CircularProgressItem.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 62, 17);
			BindingExtension bindingExtension7;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension7 = new BindingExtension(), new Uri("Dashboard\\DashboardGauges\\CircularProgressItem.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 62, 17);
			BindingExtension bindingExtension8;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension8 = new BindingExtension(), new Uri("Dashboard\\DashboardGauges\\CircularProgressItem.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 63, 17);
			BindingExtension bindingExtension9;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension9 = new BindingExtension(), new Uri("Dashboard\\DashboardGauges\\CircularProgressItem.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 65, 17);
			SimpleTextItem simpleTextItem;
			VisualDiagnostics.RegisterSourceInfo(simpleTextItem = new SimpleTextItem(), new Uri("Dashboard\\DashboardGauges\\CircularProgressItem.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 70, 22);
			SfCircularProgressBar sfCircularProgressBar;
			VisualDiagnostics.RegisterSourceInfo(sfCircularProgressBar = new SfCircularProgressBar(), new Uri("Dashboard\\DashboardGauges\\CircularProgressItem.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 53, 14);
			BindingExtension bindingExtension10;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension10 = new BindingExtension(), new Uri("Dashboard\\DashboardGauges\\CircularProgressItem.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 100, 17);
			DynamicResourceExtension dynamicResourceExtension;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension = new DynamicResourceExtension(), new Uri("Dashboard\\DashboardGauges\\CircularProgressItem.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 106, 21);
			BindingExtension bindingExtension11;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension11 = new BindingExtension(), new Uri("Dashboard\\DashboardGauges\\CircularProgressItem.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 107, 21);
			Label label;
			VisualDiagnostics.RegisterSourceInfo(label = new Label(), new Uri("Dashboard\\DashboardGauges\\CircularProgressItem.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 104, 18);
			DynamicResourceExtension dynamicResourceExtension2;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension2 = new DynamicResourceExtension(), new Uri("Dashboard\\DashboardGauges\\CircularProgressItem.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 111, 21);
			Label label2;
			VisualDiagnostics.RegisterSourceInfo(label2 = new Label(), new Uri("Dashboard\\DashboardGauges\\CircularProgressItem.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 109, 18);
			StackLayout stackLayout;
			VisualDiagnostics.RegisterSourceInfo(stackLayout = new StackLayout(), new Uri("Dashboard\\DashboardGauges\\CircularProgressItem.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 96, 14);
			Grid grid;
			VisualDiagnostics.RegisterSourceInfo(grid = new Grid(), new Uri("Dashboard\\DashboardGauges\\CircularProgressItem.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 30, 10);
			VisualDiagnostics.RegisterSourceInfo(this, new Uri("Dashboard\\DashboardGauges\\CircularProgressItem.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 2, 2);
			NameScope nameScope = NameScope.GetNameScope(this) ?? new NameScope();
			NameScope.SetNameScope(this, nameScope);
			this.Resources = resourceDictionary;
			resourceDictionary.Add("ExcludeInfinityFromDoubleConverter", excludeInfinityFromDoubleConverter);
			resourceDictionary.Add("BoolToNegativeConverter", boolToNegativeConverter);
			resourceDictionary.Add("DoubleToPercentConverter", doubleToPercentConverter);
			resourceDictionary.Add("FloatItemToLineBreakModeConverter", floatItemToLineBreakModeConverter);
			resourceDictionary.Add("BoolToSFGaugeOrientationConverter", boolToSFGaugeOrientationConverter);
			resourceDictionary.Add("ProgressBarWidthToPaddingConverter", progressBarWidthToPaddingConverter);
			resourceDictionary.Add("TransparentColorToFalseConverter", transparentColorToFalseConverter);
			resourceDictionary.Add("CornerRadiusToThicknessConverter", cornerRadiusToThicknessConverter);
			this.SetValue(View.MarginProperty, new Thickness(0.0));
			this.SetValue(Layout.PaddingProperty, new Thickness(0.0));
			this.Resources = resourceDictionary;
			grid.SetValue(View.MarginProperty, new Thickness(0.0));
			grid.SetValue(Layout.PaddingProperty, new Thickness(0.0));
			grid.SetValue(View.HorizontalOptionsProperty, LayoutOptions.Fill);
			grid.SetValue(View.VerticalOptionsProperty, LayoutOptions.Fill);
			rowDefinition.SetValue(RowDefinition.HeightProperty, new GridLengthTypeConverter().ConvertFromInvariantString("*"));
			grid.GetValue(Grid.RowDefinitionsProperty).Add(rowDefinition);
			gaugeBackground.SetValue(Grid.RowProperty, 0);
			bindingExtension.Mode = 2;
			bindingExtension.Path = "ShowDefaultBackground";
			bindingExtension.TypedBinding = new TypedBinding<DashboardItem, bool>(delegate(DashboardItem A_0)
			{
				if (A_0 != null)
				{
					return new ValueTuple<bool, bool>(A_0.ShowDefaultBackground, true);
				}
				return default(ValueTuple<bool, bool>);
			}, null, new Tuple<Func<DashboardItem, object>, string>[]
			{
				new Tuple<Func<DashboardItem, object>, string>((DashboardItem A_0) => A_0, "ShowDefaultBackground")
			});
			BindingBase bindingBase = bindingExtension.ProvideValue(null);
			gaugeBackground.SetBinding(VisualElement.IsVisibleProperty, bindingBase);
			grid.Children.Add(gaugeBackground);
			boxView.SetValue(Grid.RowProperty, 0);
			bindingExtension2.Mode = 2;
			staticResourceExtension.Key = "CornerRadiusToThicknessConverter";
			IMarkupExtension markupExtension = staticResourceExtension;
			XamlServiceProvider xamlServiceProvider = new XamlServiceProvider();
			Type typeFromHandle = typeof(IProvideValueTarget);
			object[] array = new object[0 + 4];
			array[0] = bindingExtension2;
			array[1] = boxView;
			array[2] = grid;
			array[3] = this;
			object obj;
			xamlServiceProvider.Add(typeFromHandle, obj = new SimpleValueTargetProvider(array, typeof(BindingExtension).GetRuntimeProperty("Converter"), nameScope));
			xamlServiceProvider.Add(typeof(IReferenceProvider), obj);
			Type typeFromHandle2 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver = new XmlNamespaceResolver();
			xmlNamespaceResolver.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver.Add("dash", "clr-namespace:CarScannerXamarinForms.Dashboard");
			xmlNamespaceResolver.Add("gauges", "clr-namespace:CarScannerXamarinForms.Dashboard.DashboardGauges");
			xmlNamespaceResolver.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver.Add("progressBar", "clr-namespace:Syncfusion.XForms.ProgressBar;assembly=Syncfusion.SfProgressBar.XForms");
			xamlServiceProvider.Add(typeFromHandle2, new XamlTypeResolver(xmlNamespaceResolver, typeof(CircularProgressItem).GetTypeInfo().Assembly));
			xamlServiceProvider.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(44, 17)));
			object obj2 = markupExtension.ProvideValue(xamlServiceProvider);
			bindingExtension2.Converter = obj2;
			bindingExtension2.Path = "CornerRadius";
			bindingExtension2.TypedBinding = new TypedBinding<DashboardItem, Thickness>(delegate(DashboardItem A_0)
			{
				if (A_0 != null)
				{
					return new ValueTuple<Thickness, bool>(A_0.CornerRadius, true);
				}
				return default(ValueTuple<Thickness, bool>);
			}, null, new Tuple<Func<DashboardItem, object>, string>[]
			{
				new Tuple<Func<DashboardItem, object>, string>((DashboardItem A_0) => A_0, "CornerRadius")
			});
			BindingBase bindingBase2 = bindingExtension2.ProvideValue(null);
			boxView.SetBinding(BoxView.CornerRadiusProperty, bindingBase2);
			boxView.SetValue(View.HorizontalOptionsProperty, LayoutOptions.Fill);
			bindingExtension3.Mode = 2;
			staticResourceExtension2.Key = "TransparentColorToFalseConverter";
			IMarkupExtension markupExtension2 = staticResourceExtension2;
			XamlServiceProvider xamlServiceProvider2 = new XamlServiceProvider();
			Type typeFromHandle3 = typeof(IProvideValueTarget);
			object[] array2 = new object[0 + 4];
			array2[0] = bindingExtension3;
			array2[1] = boxView;
			array2[2] = grid;
			array2[3] = this;
			object obj3;
			xamlServiceProvider2.Add(typeFromHandle3, obj3 = new SimpleValueTargetProvider(array2, typeof(BindingExtension).GetRuntimeProperty("Converter"), nameScope));
			xamlServiceProvider2.Add(typeof(IReferenceProvider), obj3);
			Type typeFromHandle4 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver2 = new XmlNamespaceResolver();
			xmlNamespaceResolver2.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver2.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver2.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver2.Add("dash", "clr-namespace:CarScannerXamarinForms.Dashboard");
			xmlNamespaceResolver2.Add("gauges", "clr-namespace:CarScannerXamarinForms.Dashboard.DashboardGauges");
			xmlNamespaceResolver2.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver2.Add("progressBar", "clr-namespace:Syncfusion.XForms.ProgressBar;assembly=Syncfusion.SfProgressBar.XForms");
			xamlServiceProvider2.Add(typeFromHandle4, new XamlTypeResolver(xmlNamespaceResolver2, typeof(CircularProgressItem).GetTypeInfo().Assembly));
			xamlServiceProvider2.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(46, 17)));
			object obj4 = markupExtension2.ProvideValue(xamlServiceProvider2);
			bindingExtension3.Converter = obj4;
			bindingExtension3.Path = "IndicatorBackgroundColor";
			bindingExtension3.TypedBinding = new TypedBinding<DashboardItem, Color>(delegate(DashboardItem A_0)
			{
				if (A_0 != null)
				{
					return new ValueTuple<Color, bool>(A_0.IndicatorBackgroundColor, true);
				}
				return default(ValueTuple<Color, bool>);
			}, null, new Tuple<Func<DashboardItem, object>, string>[]
			{
				new Tuple<Func<DashboardItem, object>, string>((DashboardItem A_0) => A_0, "IndicatorBackgroundColor")
			});
			BindingBase bindingBase3 = bindingExtension3.ProvideValue(null);
			boxView.SetBinding(VisualElement.IsVisibleProperty, bindingBase3);
			boxView.SetValue(View.VerticalOptionsProperty, LayoutOptions.Fill);
			bindingExtension4.Mode = 2;
			bindingExtension4.Path = "IndicatorBackgroundColor";
			bindingExtension4.TypedBinding = new TypedBinding<DashboardItem, Color>(delegate(DashboardItem A_0)
			{
				if (A_0 != null)
				{
					return new ValueTuple<Color, bool>(A_0.IndicatorBackgroundColor, true);
				}
				return default(ValueTuple<Color, bool>);
			}, null, new Tuple<Func<DashboardItem, object>, string>[]
			{
				new Tuple<Func<DashboardItem, object>, string>((DashboardItem A_0) => A_0, "IndicatorBackgroundColor")
			});
			BindingBase bindingBase4 = bindingExtension4.ProvideValue(null);
			boxView.SetBinding(BoxView.ColorProperty, bindingBase4);
			grid.Children.Add(boxView);
			sfCircularProgressBar.SetValue(Grid.RowProperty, 0);
			sfCircularProgressBar.SetValue(ProgressBarBase.EasingEffectProperty, 5);
			sfCircularProgressBar.SetValue(VisualElement.HeightRequestProperty, 60.0);
			sfCircularProgressBar.SetValue(SfCircularProgressBar.IndicatorInnerRadiusProperty, 50.0);
			sfCircularProgressBar.SetValue(SfCircularProgressBar.IndicatorOuterRadiusProperty, 55.0);
			sfCircularProgressBar.SetValue(ProgressBarBase.IsIndeterminateProperty, false);
			bindingExtension5.Mode = 2;
			bindingExtension5.Path = "Maximum";
			bindingExtension5.TypedBinding = new TypedBinding<DashboardItem, double>(delegate(DashboardItem A_0)
			{
				if (A_0 != null)
				{
					return new ValueTuple<double, bool>(A_0.Maximum, true);
				}
				return default(ValueTuple<double, bool>);
			}, null, new Tuple<Func<DashboardItem, object>, string>[]
			{
				new Tuple<Func<DashboardItem, object>, string>((DashboardItem A_0) => A_0, "Maximum")
			});
			BindingBase bindingBase5 = bindingExtension5.ProvideValue(null);
			sfCircularProgressBar.SetBinding(ProgressBarBase.MaximumProperty, bindingBase5);
			bindingExtension6.Mode = 2;
			bindingExtension6.Path = "Minimum";
			bindingExtension6.TypedBinding = new TypedBinding<DashboardItem, double>(delegate(DashboardItem A_0)
			{
				if (A_0 != null)
				{
					return new ValueTuple<double, bool>(A_0.Minimum, true);
				}
				return default(ValueTuple<double, bool>);
			}, null, new Tuple<Func<DashboardItem, object>, string>[]
			{
				new Tuple<Func<DashboardItem, object>, string>((DashboardItem A_0) => A_0, "Minimum")
			});
			BindingBase bindingBase6 = bindingExtension6.ProvideValue(null);
			sfCircularProgressBar.SetBinding(ProgressBarBase.MinimumProperty, bindingBase6);
			bindingExtension7.Mode = 2;
			staticResourceExtension3.Key = "ExcludeInfinityFromDoubleConverter";
			IMarkupExtension markupExtension3 = staticResourceExtension3;
			XamlServiceProvider xamlServiceProvider3 = new XamlServiceProvider();
			Type typeFromHandle5 = typeof(IProvideValueTarget);
			object[] array3 = new object[0 + 4];
			array3[0] = bindingExtension7;
			array3[1] = sfCircularProgressBar;
			array3[2] = grid;
			array3[3] = this;
			object obj5;
			xamlServiceProvider3.Add(typeFromHandle5, obj5 = new SimpleValueTargetProvider(array3, typeof(BindingExtension).GetRuntimeProperty("Converter"), nameScope));
			xamlServiceProvider3.Add(typeof(IReferenceProvider), obj5);
			Type typeFromHandle6 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver3 = new XmlNamespaceResolver();
			xmlNamespaceResolver3.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver3.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver3.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver3.Add("dash", "clr-namespace:CarScannerXamarinForms.Dashboard");
			xmlNamespaceResolver3.Add("gauges", "clr-namespace:CarScannerXamarinForms.Dashboard.DashboardGauges");
			xmlNamespaceResolver3.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver3.Add("progressBar", "clr-namespace:Syncfusion.XForms.ProgressBar;assembly=Syncfusion.SfProgressBar.XForms");
			xamlServiceProvider3.Add(typeFromHandle6, new XamlTypeResolver(xmlNamespaceResolver3, typeof(CircularProgressItem).GetTypeInfo().Assembly));
			xamlServiceProvider3.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(62, 17)));
			object obj6 = markupExtension3.ProvideValue(xamlServiceProvider3);
			bindingExtension7.Converter = obj6;
			bindingExtension7.Path = "Model.FloatValue";
			bindingExtension7.TypedBinding = new TypedBinding<DashboardItem, double>(delegate(DashboardItem A_0)
			{
				if (A_0 != null)
				{
					LiveDataPIDModel model = A_0.Model;
					if (model != null)
					{
						return new ValueTuple<double, bool>(model.FloatValue, true);
					}
				}
				return default(ValueTuple<double, bool>);
			}, null, new Tuple<Func<DashboardItem, object>, string>[]
			{
				new Tuple<Func<DashboardItem, object>, string>((DashboardItem A_0) => A_0, "Model"),
				new Tuple<Func<DashboardItem, object>, string>((DashboardItem A_0) => A_0.Model, "FloatValue")
			});
			BindingBase bindingBase7 = bindingExtension7.ProvideValue(null);
			sfCircularProgressBar.SetBinding(ProgressBarBase.ProgressProperty, bindingBase7);
			bindingExtension8.Mode = 2;
			bindingExtension8.Path = "GaugePointerColor";
			bindingExtension8.TypedBinding = new TypedBinding<DashboardItem, Color>(delegate(DashboardItem A_0)
			{
				if (A_0 != null)
				{
					return new ValueTuple<Color, bool>(A_0.GaugePointerColor, true);
				}
				return default(ValueTuple<Color, bool>);
			}, null, new Tuple<Func<DashboardItem, object>, string>[]
			{
				new Tuple<Func<DashboardItem, object>, string>((DashboardItem A_0) => A_0, "GaugePointerColor")
			});
			BindingBase bindingBase8 = bindingExtension8.ProvideValue(null);
			sfCircularProgressBar.SetBinding(ProgressBarBase.ProgressColorProperty, bindingBase8);
			sfCircularProgressBar.SetValue(ProgressBarBase.SegmentCountProperty, 4);
			bindingExtension9.Mode = 2;
			bindingExtension9.Path = "GaugeRimColor";
			bindingExtension9.TypedBinding = new TypedBinding<DashboardItem, Color>(delegate(DashboardItem A_0)
			{
				if (A_0 != null)
				{
					return new ValueTuple<Color, bool>(A_0.GaugeRimColor, true);
				}
				return default(ValueTuple<Color, bool>);
			}, null, new Tuple<Func<DashboardItem, object>, string>[]
			{
				new Tuple<Func<DashboardItem, object>, string>((DashboardItem A_0) => A_0, "GaugeRimColor")
			});
			BindingBase bindingBase9 = bindingExtension9.ProvideValue(null);
			sfCircularProgressBar.SetBinding(ProgressBarBase.TrackColorProperty, bindingBase9);
			sfCircularProgressBar.SetValue(SfCircularProgressBar.TrackInnerRadiusProperty, 50.0);
			sfCircularProgressBar.SetValue(SfCircularProgressBar.TrackOuterRadiusProperty, 55.0);
			sfCircularProgressBar.SetValue(VisualElement.WidthRequestProperty, 60.0);
			sfCircularProgressBar.SetValue(SfCircularProgressBar.ContentProperty, simpleTextItem);
			grid.Children.Add(sfCircularProgressBar);
			stackLayout.SetValue(Grid.RowProperty, 1);
			stackLayout.SetValue(View.MarginProperty, new Thickness(0.0));
			stackLayout.SetValue(View.HorizontalOptionsProperty, LayoutOptions.End);
			bindingExtension10.Mode = 2;
			bindingExtension10.Path = "Model.Settings.ShowPing";
			bindingExtension10.TypedBinding = new TypedBinding<DashboardItem, bool>(delegate(DashboardItem A_0)
			{
				if (A_0 != null)
				{
					LiveDataPIDModel model2 = A_0.Model;
					if (model2 != null)
					{
						SharedSettings settings = model2.Settings;
						if (settings != null)
						{
							return new ValueTuple<bool, bool>(settings.ShowPing, true);
						}
					}
				}
				return default(ValueTuple<bool, bool>);
			}, null, new Tuple<Func<DashboardItem, object>, string>[]
			{
				new Tuple<Func<DashboardItem, object>, string>((DashboardItem A_0) => A_0, "Model"),
				new Tuple<Func<DashboardItem, object>, string>((DashboardItem A_0) => A_0.Model, "Settings"),
				new Tuple<Func<DashboardItem, object>, string>((DashboardItem A_0) => A_0.Model.Settings, "ShowPing")
			});
			BindingBase bindingBase10 = bindingExtension10.ProvideValue(null);
			stackLayout.SetBinding(VisualElement.IsVisibleProperty, bindingBase10);
			stackLayout.SetValue(StackLayout.OrientationProperty, 1);
			stackLayout.SetValue(StackLayout.SpacingProperty, 0.0);
			stackLayout.SetValue(View.VerticalOptionsProperty, LayoutOptions.End);
			label.SetValue(Label.FontAttributesProperty, new FontAttributesConverter().ConvertFromInvariantString("Bold"));
			dynamicResourceExtension.Key = "BaseFontSize--";
			IMarkupExtension<DynamicResource> markupExtension4 = dynamicResourceExtension;
			XamlServiceProvider xamlServiceProvider4 = new XamlServiceProvider();
			Type typeFromHandle7 = typeof(IProvideValueTarget);
			object[] array4 = new object[0 + 4];
			array4[0] = label;
			array4[1] = stackLayout;
			array4[2] = grid;
			array4[3] = this;
			object obj7;
			xamlServiceProvider4.Add(typeFromHandle7, obj7 = new SimpleValueTargetProvider(array4, Label.FontSizeProperty, nameScope));
			xamlServiceProvider4.Add(typeof(IReferenceProvider), obj7);
			Type typeFromHandle8 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver4 = new XmlNamespaceResolver();
			xmlNamespaceResolver4.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver4.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver4.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver4.Add("dash", "clr-namespace:CarScannerXamarinForms.Dashboard");
			xmlNamespaceResolver4.Add("gauges", "clr-namespace:CarScannerXamarinForms.Dashboard.DashboardGauges");
			xmlNamespaceResolver4.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver4.Add("progressBar", "clr-namespace:Syncfusion.XForms.ProgressBar;assembly=Syncfusion.SfProgressBar.XForms");
			xamlServiceProvider4.Add(typeFromHandle8, new XamlTypeResolver(xmlNamespaceResolver4, typeof(CircularProgressItem).GetTypeInfo().Assembly));
			xamlServiceProvider4.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(106, 21)));
			DynamicResource dynamicResource = markupExtension4.ProvideValue(xamlServiceProvider4);
			label.SetDynamicResource(Label.FontSizeProperty, dynamicResource.Key);
			bindingExtension11.Mode = 2;
			bindingExtension11.Path = "Model.Ping";
			bindingExtension11.TypedBinding = new TypedBinding<DashboardItem, long>(delegate(DashboardItem A_0)
			{
				if (A_0 != null)
				{
					LiveDataPIDModel model3 = A_0.Model;
					if (model3 != null)
					{
						return new ValueTuple<long, bool>(model3.Ping, true);
					}
				}
				return default(ValueTuple<long, bool>);
			}, null, new Tuple<Func<DashboardItem, object>, string>[]
			{
				new Tuple<Func<DashboardItem, object>, string>((DashboardItem A_0) => A_0, "Model"),
				new Tuple<Func<DashboardItem, object>, string>((DashboardItem A_0) => A_0.Model, "Ping")
			});
			BindingBase bindingBase11 = bindingExtension11.ProvideValue(null);
			label.SetBinding(Label.TextProperty, bindingBase11);
			label.SetValue(Label.TextColorProperty, Color.Red);
			stackLayout.Children.Add(label);
			label2.SetValue(Label.FontAttributesProperty, new FontAttributesConverter().ConvertFromInvariantString("Bold"));
			dynamicResourceExtension2.Key = "BaseFontSize--";
			IMarkupExtension<DynamicResource> markupExtension5 = dynamicResourceExtension2;
			XamlServiceProvider xamlServiceProvider5 = new XamlServiceProvider();
			Type typeFromHandle9 = typeof(IProvideValueTarget);
			object[] array5 = new object[0 + 4];
			array5[0] = label2;
			array5[1] = stackLayout;
			array5[2] = grid;
			array5[3] = this;
			object obj8;
			xamlServiceProvider5.Add(typeFromHandle9, obj8 = new SimpleValueTargetProvider(array5, Label.FontSizeProperty, nameScope));
			xamlServiceProvider5.Add(typeof(IReferenceProvider), obj8);
			Type typeFromHandle10 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver5 = new XmlNamespaceResolver();
			xmlNamespaceResolver5.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver5.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver5.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver5.Add("dash", "clr-namespace:CarScannerXamarinForms.Dashboard");
			xmlNamespaceResolver5.Add("gauges", "clr-namespace:CarScannerXamarinForms.Dashboard.DashboardGauges");
			xmlNamespaceResolver5.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver5.Add("progressBar", "clr-namespace:Syncfusion.XForms.ProgressBar;assembly=Syncfusion.SfProgressBar.XForms");
			xamlServiceProvider5.Add(typeFromHandle10, new XamlTypeResolver(xmlNamespaceResolver5, typeof(CircularProgressItem).GetTypeInfo().Assembly));
			xamlServiceProvider5.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(111, 21)));
			DynamicResource dynamicResource2 = markupExtension5.ProvideValue(xamlServiceProvider5);
			label2.SetDynamicResource(Label.FontSizeProperty, dynamicResource2.Key);
			label2.SetValue(Label.TextProperty, "ms");
			label2.SetValue(Label.TextColorProperty, Color.Red);
			stackLayout.Children.Add(label2);
			grid.Children.Add(stackLayout);
			this.SetValue(ContentView.ContentProperty, grid);
		}

		// Token: 0x0600436E RID: 17262 RVA: 0x00345252 File Offset: 0x00343452
		private void __InitComponentRuntime()
		{
			Extensions.LoadFromXaml<CircularProgressItem>(this, typeof(CircularProgressItem));
		}

		// Token: 0x0600436F RID: 17263 RVA: 0x00345268 File Offset: 0x00343468
		[CompilerGenerated]
		private static ValueTuple<bool, bool> <InitializeComponent>typedBindingsM__370(DashboardItem A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<bool, bool>(A_0.ShowDefaultBackground, true);
			}
			return default(ValueTuple<bool, bool>);
		}

		// Token: 0x06004370 RID: 17264 RVA: 0x00345298 File Offset: 0x00343498
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__371(DashboardItem A_0)
		{
			return A_0;
		}

		// Token: 0x06004371 RID: 17265 RVA: 0x003452A8 File Offset: 0x003434A8
		[CompilerGenerated]
		private static ValueTuple<Thickness, bool> <InitializeComponent>typedBindingsM__372(DashboardItem A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<Thickness, bool>(A_0.CornerRadius, true);
			}
			return default(ValueTuple<Thickness, bool>);
		}

		// Token: 0x06004372 RID: 17266 RVA: 0x003452D8 File Offset: 0x003434D8
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__373(DashboardItem A_0)
		{
			return A_0;
		}

		// Token: 0x06004373 RID: 17267 RVA: 0x003452E8 File Offset: 0x003434E8
		[CompilerGenerated]
		private static ValueTuple<Color, bool> <InitializeComponent>typedBindingsM__374(DashboardItem A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<Color, bool>(A_0.IndicatorBackgroundColor, true);
			}
			return default(ValueTuple<Color, bool>);
		}

		// Token: 0x06004374 RID: 17268 RVA: 0x00345318 File Offset: 0x00343518
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__375(DashboardItem A_0)
		{
			return A_0;
		}

		// Token: 0x06004375 RID: 17269 RVA: 0x00345328 File Offset: 0x00343528
		[CompilerGenerated]
		private static ValueTuple<Color, bool> <InitializeComponent>typedBindingsM__376(DashboardItem A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<Color, bool>(A_0.IndicatorBackgroundColor, true);
			}
			return default(ValueTuple<Color, bool>);
		}

		// Token: 0x06004376 RID: 17270 RVA: 0x00345358 File Offset: 0x00343558
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__377(DashboardItem A_0)
		{
			return A_0;
		}

		// Token: 0x06004377 RID: 17271 RVA: 0x00345368 File Offset: 0x00343568
		[CompilerGenerated]
		private static ValueTuple<double, bool> <InitializeComponent>typedBindingsM__378(DashboardItem A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<double, bool>(A_0.Maximum, true);
			}
			return default(ValueTuple<double, bool>);
		}

		// Token: 0x06004378 RID: 17272 RVA: 0x00345398 File Offset: 0x00343598
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__379(DashboardItem A_0)
		{
			return A_0;
		}

		// Token: 0x06004379 RID: 17273 RVA: 0x003453A8 File Offset: 0x003435A8
		[CompilerGenerated]
		private static ValueTuple<double, bool> <InitializeComponent>typedBindingsM__380(DashboardItem A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<double, bool>(A_0.Minimum, true);
			}
			return default(ValueTuple<double, bool>);
		}

		// Token: 0x0600437A RID: 17274 RVA: 0x003453D8 File Offset: 0x003435D8
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__381(DashboardItem A_0)
		{
			return A_0;
		}

		// Token: 0x0600437B RID: 17275 RVA: 0x003453E8 File Offset: 0x003435E8
		[CompilerGenerated]
		private static ValueTuple<double, bool> <InitializeComponent>typedBindingsM__382(DashboardItem A_0)
		{
			if (A_0 != null)
			{
				LiveDataPIDModel model = A_0.Model;
				if (model != null)
				{
					return new ValueTuple<double, bool>(model.FloatValue, true);
				}
			}
			return default(ValueTuple<double, bool>);
		}

		// Token: 0x0600437C RID: 17276 RVA: 0x00345420 File Offset: 0x00343620
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__383(DashboardItem A_0)
		{
			return A_0;
		}

		// Token: 0x0600437D RID: 17277 RVA: 0x00345430 File Offset: 0x00343630
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__384(DashboardItem A_0)
		{
			return A_0.Model;
		}

		// Token: 0x0600437E RID: 17278 RVA: 0x00345444 File Offset: 0x00343644
		[CompilerGenerated]
		private static ValueTuple<Color, bool> <InitializeComponent>typedBindingsM__385(DashboardItem A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<Color, bool>(A_0.GaugePointerColor, true);
			}
			return default(ValueTuple<Color, bool>);
		}

		// Token: 0x0600437F RID: 17279 RVA: 0x00345474 File Offset: 0x00343674
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__386(DashboardItem A_0)
		{
			return A_0;
		}

		// Token: 0x06004380 RID: 17280 RVA: 0x00345484 File Offset: 0x00343684
		[CompilerGenerated]
		private static ValueTuple<Color, bool> <InitializeComponent>typedBindingsM__387(DashboardItem A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<Color, bool>(A_0.GaugeRimColor, true);
			}
			return default(ValueTuple<Color, bool>);
		}

		// Token: 0x06004381 RID: 17281 RVA: 0x003454B4 File Offset: 0x003436B4
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__388(DashboardItem A_0)
		{
			return A_0;
		}

		// Token: 0x06004382 RID: 17282 RVA: 0x003454C4 File Offset: 0x003436C4
		[CompilerGenerated]
		private static ValueTuple<bool, bool> <InitializeComponent>typedBindingsM__389(DashboardItem A_0)
		{
			if (A_0 != null)
			{
				LiveDataPIDModel model = A_0.Model;
				if (model != null)
				{
					SharedSettings settings = model.Settings;
					if (settings != null)
					{
						return new ValueTuple<bool, bool>(settings.ShowPing, true);
					}
				}
			}
			return default(ValueTuple<bool, bool>);
		}

		// Token: 0x06004383 RID: 17283 RVA: 0x00345508 File Offset: 0x00343708
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__390(DashboardItem A_0)
		{
			return A_0;
		}

		// Token: 0x06004384 RID: 17284 RVA: 0x00345518 File Offset: 0x00343718
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__391(DashboardItem A_0)
		{
			return A_0.Model;
		}

		// Token: 0x06004385 RID: 17285 RVA: 0x0034552C File Offset: 0x0034372C
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__392(DashboardItem A_0)
		{
			return A_0.Model.Settings;
		}

		// Token: 0x06004386 RID: 17286 RVA: 0x00345544 File Offset: 0x00343744
		[CompilerGenerated]
		private static ValueTuple<long, bool> <InitializeComponent>typedBindingsM__393(DashboardItem A_0)
		{
			if (A_0 != null)
			{
				LiveDataPIDModel model = A_0.Model;
				if (model != null)
				{
					return new ValueTuple<long, bool>(model.Ping, true);
				}
			}
			return default(ValueTuple<long, bool>);
		}

		// Token: 0x06004387 RID: 17287 RVA: 0x0034557C File Offset: 0x0034377C
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__394(DashboardItem A_0)
		{
			return A_0;
		}

		// Token: 0x06004388 RID: 17288 RVA: 0x0034558C File Offset: 0x0034378C
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__395(DashboardItem A_0)
		{
			return A_0.Model;
		}
	}
}
