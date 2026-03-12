using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
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
	// Token: 0x020007B3 RID: 1971
	[XamlFilePath("Dashboard\\DashboardGauges\\LinearProgressItemiOS.xaml")]
	public class LinearProgressItemiOS : ContentView
	{
		// Token: 0x060044CC RID: 17612 RVA: 0x00359D5F File Offset: 0x00357F5F
		public LinearProgressItemiOS()
		{
			this.InitializeComponent();
		}

		// Token: 0x060044CD RID: 17613 RVA: 0x00359D70 File Offset: 0x00357F70
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private void InitializeComponent()
		{
			if (ResourceLoader.IsEnabled && ResourceLoader.CanProvideContentFor(new ResourceLoader.ResourceLoadingQuery
			{
				AssemblyName = typeof(LinearProgressItemiOS).GetTypeInfo().Assembly.GetName(),
				ResourcePath = "Dashboard/DashboardGauges/LinearProgressItemiOS.xaml",
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
			VisualDiagnostics.RegisterSourceInfo(excludeInfinityFromDoubleConverter = new ExcludeInfinityFromDoubleConverter(), new Uri("Dashboard\\DashboardGauges\\LinearProgressItemiOS.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 15, 14);
			BoolToNegativeConverter boolToNegativeConverter;
			VisualDiagnostics.RegisterSourceInfo(boolToNegativeConverter = new BoolToNegativeConverter(), new Uri("Dashboard\\DashboardGauges\\LinearProgressItemiOS.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 16, 14);
			DoubleToPercentConverter doubleToPercentConverter;
			VisualDiagnostics.RegisterSourceInfo(doubleToPercentConverter = new DoubleToPercentConverter(), new Uri("Dashboard\\DashboardGauges\\LinearProgressItemiOS.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 17, 14);
			FloatItemToLineBreakModeConverter floatItemToLineBreakModeConverter;
			VisualDiagnostics.RegisterSourceInfo(floatItemToLineBreakModeConverter = new FloatItemToLineBreakModeConverter(), new Uri("Dashboard\\DashboardGauges\\LinearProgressItemiOS.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 18, 14);
			BoolToSFGaugeOrientationConverter boolToSFGaugeOrientationConverter;
			VisualDiagnostics.RegisterSourceInfo(boolToSFGaugeOrientationConverter = new BoolToSFGaugeOrientationConverter(), new Uri("Dashboard\\DashboardGauges\\LinearProgressItemiOS.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 19, 14);
			ProgressBarWidthToPaddingConverter progressBarWidthToPaddingConverter;
			VisualDiagnostics.RegisterSourceInfo(progressBarWidthToPaddingConverter = new ProgressBarWidthToPaddingConverter(), new Uri("Dashboard\\DashboardGauges\\LinearProgressItemiOS.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 20, 14);
			TransparentColorToFalseConverter transparentColorToFalseConverter;
			VisualDiagnostics.RegisterSourceInfo(transparentColorToFalseConverter = new TransparentColorToFalseConverter(), new Uri("Dashboard\\DashboardGauges\\LinearProgressItemiOS.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 21, 14);
			CornerRadiusToThicknessConverter cornerRadiusToThicknessConverter;
			VisualDiagnostics.RegisterSourceInfo(cornerRadiusToThicknessConverter = new CornerRadiusToThicknessConverter(), new Uri("Dashboard\\DashboardGauges\\LinearProgressItemiOS.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 22, 14);
			ResourceDictionary resourceDictionary;
			VisualDiagnostics.RegisterSourceInfo(resourceDictionary = new ResourceDictionary(), new Uri("Dashboard\\DashboardGauges\\LinearProgressItemiOS.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 14, 10);
			RowDefinition rowDefinition;
			VisualDiagnostics.RegisterSourceInfo(rowDefinition = new RowDefinition(), new Uri("Dashboard\\DashboardGauges\\LinearProgressItemiOS.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 36, 18);
			RowDefinition rowDefinition2;
			VisualDiagnostics.RegisterSourceInfo(rowDefinition2 = new RowDefinition(), new Uri("Dashboard\\DashboardGauges\\LinearProgressItemiOS.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 37, 18);
			RowDefinition rowDefinition3;
			VisualDiagnostics.RegisterSourceInfo(rowDefinition3 = new RowDefinition(), new Uri("Dashboard\\DashboardGauges\\LinearProgressItemiOS.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 38, 18);
			BindingExtension bindingExtension;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension = new BindingExtension(), new Uri("Dashboard\\DashboardGauges\\LinearProgressItemiOS.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 44, 17);
			GaugeBackground gaugeBackground;
			VisualDiagnostics.RegisterSourceInfo(gaugeBackground = new GaugeBackground(), new Uri("Dashboard\\DashboardGauges\\LinearProgressItemiOS.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 41, 14);
			StaticResourceExtension staticResourceExtension;
			VisualDiagnostics.RegisterSourceInfo(staticResourceExtension = new StaticResourceExtension(), new Uri("Dashboard\\DashboardGauges\\LinearProgressItemiOS.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 48, 17);
			BindingExtension bindingExtension2;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension2 = new BindingExtension(), new Uri("Dashboard\\DashboardGauges\\LinearProgressItemiOS.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 48, 17);
			StaticResourceExtension staticResourceExtension2;
			VisualDiagnostics.RegisterSourceInfo(staticResourceExtension2 = new StaticResourceExtension(), new Uri("Dashboard\\DashboardGauges\\LinearProgressItemiOS.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 50, 17);
			BindingExtension bindingExtension3;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension3 = new BindingExtension(), new Uri("Dashboard\\DashboardGauges\\LinearProgressItemiOS.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 50, 17);
			BindingExtension bindingExtension4;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension4 = new BindingExtension(), new Uri("Dashboard\\DashboardGauges\\LinearProgressItemiOS.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 52, 17);
			BoxView boxView;
			VisualDiagnostics.RegisterSourceInfo(boxView = new BoxView(), new Uri("Dashboard\\DashboardGauges\\LinearProgressItemiOS.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 46, 14);
			BindingExtension bindingExtension5;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension5 = new BindingExtension(), new Uri("Dashboard\\DashboardGauges\\LinearProgressItemiOS.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 56, 17);
			BindingExtension bindingExtension6;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension6 = new BindingExtension(), new Uri("Dashboard\\DashboardGauges\\LinearProgressItemiOS.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 59, 17);
			BindingExtension bindingExtension7;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension7 = new BindingExtension(), new Uri("Dashboard\\DashboardGauges\\LinearProgressItemiOS.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 60, 17);
			Label label;
			VisualDiagnostics.RegisterSourceInfo(label = new Label(), new Uri("Dashboard\\DashboardGauges\\LinearProgressItemiOS.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 54, 14);
			BindingExtension bindingExtension8;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension8 = new BindingExtension(), new Uri("Dashboard\\DashboardGauges\\LinearProgressItemiOS.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 66, 17);
			BindingExtension bindingExtension9;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension9 = new BindingExtension(), new Uri("Dashboard\\DashboardGauges\\LinearProgressItemiOS.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 70, 21);
			BindingExtension bindingExtension10;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension10 = new BindingExtension(), new Uri("Dashboard\\DashboardGauges\\LinearProgressItemiOS.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 72, 21);
			BindingExtension bindingExtension11;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension11 = new BindingExtension(), new Uri("Dashboard\\DashboardGauges\\LinearProgressItemiOS.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 73, 21);
			StaticResourceExtension staticResourceExtension3;
			VisualDiagnostics.RegisterSourceInfo(staticResourceExtension3 = new StaticResourceExtension(), new Uri("Dashboard\\DashboardGauges\\LinearProgressItemiOS.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 74, 21);
			BindingExtension bindingExtension12;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension12 = new BindingExtension(), new Uri("Dashboard\\DashboardGauges\\LinearProgressItemiOS.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 74, 21);
			BindingExtension bindingExtension13;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension13 = new BindingExtension(), new Uri("Dashboard\\DashboardGauges\\LinearProgressItemiOS.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 75, 21);
			BindingExtension bindingExtension14;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension14 = new BindingExtension(), new Uri("Dashboard\\DashboardGauges\\LinearProgressItemiOS.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 76, 21);
			BindingExtension bindingExtension15;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension15 = new BindingExtension(), new Uri("Dashboard\\DashboardGauges\\LinearProgressItemiOS.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 77, 21);
			BindingExtension bindingExtension16;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension16 = new BindingExtension(), new Uri("Dashboard\\DashboardGauges\\LinearProgressItemiOS.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 78, 21);
			StaticResourceExtension staticResourceExtension4;
			VisualDiagnostics.RegisterSourceInfo(staticResourceExtension4 = new StaticResourceExtension(), new Uri("Dashboard\\DashboardGauges\\LinearProgressItemiOS.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 81, 29);
			On on;
			VisualDiagnostics.RegisterSourceInfo(on = new On(), new Uri("Dashboard\\DashboardGauges\\LinearProgressItemiOS.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 86, 38);
			On on2;
			VisualDiagnostics.RegisterSourceInfo(on2 = new On(), new Uri("Dashboard\\DashboardGauges\\LinearProgressItemiOS.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 87, 38);
			OnPlatform<BindingMode> onPlatform;
			VisualDiagnostics.RegisterSourceInfo(onPlatform = new OnPlatform<BindingMode>(), new Uri("Dashboard\\DashboardGauges\\LinearProgressItemiOS.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 85, 34);
			double num = 20.0;
			BindingExtension bindingExtension17;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension17 = new BindingExtension(), new Uri("Dashboard\\DashboardGauges\\LinearProgressItemiOS.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 80, 26);
			SfLinearProgressBar sfLinearProgressBar;
			VisualDiagnostics.RegisterSourceInfo(sfLinearProgressBar = new SfLinearProgressBar(), new Uri("Dashboard\\DashboardGauges\\LinearProgressItemiOS.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 68, 18);
			Grid grid;
			VisualDiagnostics.RegisterSourceInfo(grid = new Grid(), new Uri("Dashboard\\DashboardGauges\\LinearProgressItemiOS.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 64, 14);
			BindingExtension bindingExtension18;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension18 = new BindingExtension(), new Uri("Dashboard\\DashboardGauges\\LinearProgressItemiOS.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 104, 17);
			BindingExtension bindingExtension19;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension19 = new BindingExtension(), new Uri("Dashboard\\DashboardGauges\\LinearProgressItemiOS.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 109, 21);
			BindingExtension bindingExtension20;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension20 = new BindingExtension(), new Uri("Dashboard\\DashboardGauges\\LinearProgressItemiOS.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 110, 21);
			StaticResourceExtension staticResourceExtension5;
			VisualDiagnostics.RegisterSourceInfo(staticResourceExtension5 = new StaticResourceExtension(), new Uri("Dashboard\\DashboardGauges\\LinearProgressItemiOS.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 113, 21);
			BindingExtension bindingExtension21;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension21 = new BindingExtension(), new Uri("Dashboard\\DashboardGauges\\LinearProgressItemiOS.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 113, 21);
			BindingExtension bindingExtension22;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension22 = new BindingExtension(), new Uri("Dashboard\\DashboardGauges\\LinearProgressItemiOS.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 114, 21);
			BindingExtension bindingExtension23;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension23 = new BindingExtension(), new Uri("Dashboard\\DashboardGauges\\LinearProgressItemiOS.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 115, 21);
			Label label2;
			VisualDiagnostics.RegisterSourceInfo(label2 = new Label(), new Uri("Dashboard\\DashboardGauges\\LinearProgressItemiOS.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 107, 18);
			BindingExtension bindingExtension24;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension24 = new BindingExtension(), new Uri("Dashboard\\DashboardGauges\\LinearProgressItemiOS.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 120, 21);
			BindingExtension bindingExtension25;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension25 = new BindingExtension(), new Uri("Dashboard\\DashboardGauges\\LinearProgressItemiOS.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 124, 21);
			BindingExtension bindingExtension26;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension26 = new BindingExtension(), new Uri("Dashboard\\DashboardGauges\\LinearProgressItemiOS.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 125, 21);
			Label label3;
			VisualDiagnostics.RegisterSourceInfo(label3 = new Label(), new Uri("Dashboard\\DashboardGauges\\LinearProgressItemiOS.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 119, 18);
			StackLayout stackLayout;
			VisualDiagnostics.RegisterSourceInfo(stackLayout = new StackLayout(), new Uri("Dashboard\\DashboardGauges\\LinearProgressItemiOS.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 101, 14);
			BindingExtension bindingExtension27;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension27 = new BindingExtension(), new Uri("Dashboard\\DashboardGauges\\LinearProgressItemiOS.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 135, 17);
			DynamicResourceExtension dynamicResourceExtension;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension = new DynamicResourceExtension(), new Uri("Dashboard\\DashboardGauges\\LinearProgressItemiOS.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 141, 21);
			BindingExtension bindingExtension28;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension28 = new BindingExtension(), new Uri("Dashboard\\DashboardGauges\\LinearProgressItemiOS.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 142, 21);
			Label label4;
			VisualDiagnostics.RegisterSourceInfo(label4 = new Label(), new Uri("Dashboard\\DashboardGauges\\LinearProgressItemiOS.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 139, 18);
			DynamicResourceExtension dynamicResourceExtension2;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension2 = new DynamicResourceExtension(), new Uri("Dashboard\\DashboardGauges\\LinearProgressItemiOS.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 146, 21);
			Label label5;
			VisualDiagnostics.RegisterSourceInfo(label5 = new Label(), new Uri("Dashboard\\DashboardGauges\\LinearProgressItemiOS.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 144, 18);
			StackLayout stackLayout2;
			VisualDiagnostics.RegisterSourceInfo(stackLayout2 = new StackLayout(), new Uri("Dashboard\\DashboardGauges\\LinearProgressItemiOS.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 131, 14);
			Grid grid2;
			VisualDiagnostics.RegisterSourceInfo(grid2 = new Grid(), new Uri("Dashboard\\DashboardGauges\\LinearProgressItemiOS.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 29, 10);
			VisualDiagnostics.RegisterSourceInfo(this, new Uri("Dashboard\\DashboardGauges\\LinearProgressItemiOS.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 2, 2);
			NameScope nameScope = NameScope.GetNameScope(this) ?? new NameScope();
			NameScope.SetNameScope(this, nameScope);
			nameScope.RegisterName("labelValue", label2);
			if (label2.StyleId == null)
			{
				label2.StyleId = "labelValue";
			}
			this.labelValue = label2;
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
			grid2.SetValue(View.MarginProperty, new Thickness(0.0));
			grid2.SetValue(Layout.PaddingProperty, new Thickness(0.0));
			grid2.SetValue(View.HorizontalOptionsProperty, LayoutOptions.Fill);
			grid2.SetValue(View.VerticalOptionsProperty, LayoutOptions.Fill);
			rowDefinition.SetValue(RowDefinition.HeightProperty, new GridLengthTypeConverter().ConvertFromInvariantString("*"));
			grid2.GetValue(Grid.RowDefinitionsProperty).Add(rowDefinition);
			rowDefinition2.SetValue(RowDefinition.HeightProperty, new GridLengthTypeConverter().ConvertFromInvariantString("auto"));
			grid2.GetValue(Grid.RowDefinitionsProperty).Add(rowDefinition2);
			rowDefinition3.SetValue(RowDefinition.HeightProperty, new GridLengthTypeConverter().ConvertFromInvariantString("*"));
			grid2.GetValue(Grid.RowDefinitionsProperty).Add(rowDefinition3);
			gaugeBackground.SetValue(Grid.RowProperty, 0);
			gaugeBackground.SetValue(Grid.RowSpanProperty, 3);
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
			grid2.Children.Add(gaugeBackground);
			boxView.SetValue(Grid.RowProperty, 0);
			bindingExtension2.Mode = 2;
			staticResourceExtension.Key = "CornerRadiusToThicknessConverter";
			IMarkupExtension markupExtension = staticResourceExtension;
			XamlServiceProvider xamlServiceProvider = new XamlServiceProvider();
			Type typeFromHandle = typeof(IProvideValueTarget);
			object[] array = new object[0 + 4];
			array[0] = bindingExtension2;
			array[1] = boxView;
			array[2] = grid2;
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
			xmlNamespaceResolver.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver.Add("progressBar", "clr-namespace:Syncfusion.XForms.ProgressBar;assembly=Syncfusion.SfProgressBar.XForms");
			xamlServiceProvider.Add(typeFromHandle2, new XamlTypeResolver(xmlNamespaceResolver, typeof(LinearProgressItemiOS).GetTypeInfo().Assembly));
			xamlServiceProvider.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(48, 17)));
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
			array2[2] = grid2;
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
			xmlNamespaceResolver2.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver2.Add("progressBar", "clr-namespace:Syncfusion.XForms.ProgressBar;assembly=Syncfusion.SfProgressBar.XForms");
			xamlServiceProvider2.Add(typeFromHandle4, new XamlTypeResolver(xmlNamespaceResolver2, typeof(LinearProgressItemiOS).GetTypeInfo().Assembly));
			xamlServiceProvider2.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(50, 17)));
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
			grid2.Children.Add(boxView);
			label.SetValue(Grid.RowProperty, 0);
			bindingExtension5.Mode = 2;
			bindingExtension5.Path = "TitleFontSize";
			bindingExtension5.TypedBinding = new TypedBinding<DashboardItem, double>(delegate(DashboardItem A_0)
			{
				if (A_0 != null)
				{
					return new ValueTuple<double, bool>(A_0.TitleFontSize, true);
				}
				return default(ValueTuple<double, bool>);
			}, null, new Tuple<Func<DashboardItem, object>, string>[]
			{
				new Tuple<Func<DashboardItem, object>, string>((DashboardItem A_0) => A_0, "TitleFontSize")
			});
			BindingBase bindingBase5 = bindingExtension5.ProvideValue(null);
			label.SetBinding(Label.FontSizeProperty, bindingBase5);
			label.SetValue(View.HorizontalOptionsProperty, LayoutOptions.FillAndExpand);
			label.SetValue(Label.HorizontalTextAlignmentProperty, new TextAlignmentConverter().ConvertFromInvariantString("Center"));
			bindingExtension6.Mode = 2;
			bindingExtension6.Path = "PIDName";
			bindingExtension6.TypedBinding = new TypedBinding<DashboardItem, string>(delegate(DashboardItem A_0)
			{
				if (A_0 != null)
				{
					return new ValueTuple<string, bool>(A_0.PIDName, true);
				}
				return default(ValueTuple<string, bool>);
			}, null, new Tuple<Func<DashboardItem, object>, string>[]
			{
				new Tuple<Func<DashboardItem, object>, string>((DashboardItem A_0) => A_0, "PIDName")
			});
			BindingBase bindingBase6 = bindingExtension6.ProvideValue(null);
			label.SetBinding(Label.TextProperty, bindingBase6);
			bindingExtension7.Mode = 2;
			bindingExtension7.Path = "TitleTextColor";
			bindingExtension7.TypedBinding = new TypedBinding<DashboardItem, Color>(delegate(DashboardItem A_0)
			{
				if (A_0 != null)
				{
					return new ValueTuple<Color, bool>(A_0.TitleTextColor, true);
				}
				return default(ValueTuple<Color, bool>);
			}, null, new Tuple<Func<DashboardItem, object>, string>[]
			{
				new Tuple<Func<DashboardItem, object>, string>((DashboardItem A_0) => A_0, "TitleTextColor")
			});
			BindingBase bindingBase7 = bindingExtension7.ProvideValue(null);
			label.SetBinding(Label.TextColorProperty, bindingBase7);
			label.SetValue(View.VerticalOptionsProperty, LayoutOptions.Start);
			label.SetValue(Label.VerticalTextAlignmentProperty, new TextAlignmentConverter().ConvertFromInvariantString("Start"));
			grid2.Children.Add(label);
			grid.SetValue(Grid.RowProperty, 1);
			bindingExtension8.Mode = 2;
			bindingExtension8.Path = "LinearScaleSize";
			bindingExtension8.TypedBinding = new TypedBinding<DashboardItem, double>(delegate(DashboardItem A_0)
			{
				if (A_0 != null)
				{
					return new ValueTuple<double, bool>(A_0.LinearScaleSize, true);
				}
				return default(ValueTuple<double, bool>);
			}, null, new Tuple<Func<DashboardItem, object>, string>[]
			{
				new Tuple<Func<DashboardItem, object>, string>((DashboardItem A_0) => A_0, "LinearScaleSize")
			});
			BindingBase bindingBase8 = bindingExtension8.ProvideValue(null);
			grid.SetBinding(VisualElement.HeightRequestProperty, bindingBase8);
			grid.SetValue(View.VerticalOptionsProperty, LayoutOptions.Center);
			sfLinearProgressBar.SetValue(ProgressBarBase.EasingEffectProperty, 5);
			bindingExtension9.Mode = 2;
			bindingExtension9.Path = "Interval";
			bindingExtension9.TypedBinding = new TypedBinding<DashboardItem, double>(delegate(DashboardItem A_0)
			{
				if (A_0 != null)
				{
					return new ValueTuple<double, bool>(A_0.Interval, true);
				}
				return default(ValueTuple<double, bool>);
			}, null, new Tuple<Func<DashboardItem, object>, string>[]
			{
				new Tuple<Func<DashboardItem, object>, string>((DashboardItem A_0) => A_0, "Interval")
			});
			BindingBase bindingBase9 = bindingExtension9.ProvideValue(null);
			sfLinearProgressBar.SetBinding(ProgressBarBase.GapWidthProperty, bindingBase9);
			sfLinearProgressBar.SetValue(ProgressBarBase.IsIndeterminateProperty, false);
			bindingExtension10.Mode = 2;
			bindingExtension10.Path = "Maximum";
			bindingExtension10.TypedBinding = new TypedBinding<DashboardItem, double>(delegate(DashboardItem A_0)
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
			BindingBase bindingBase10 = bindingExtension10.ProvideValue(null);
			sfLinearProgressBar.SetBinding(ProgressBarBase.MaximumProperty, bindingBase10);
			bindingExtension11.Mode = 2;
			bindingExtension11.Path = "Minimum";
			bindingExtension11.TypedBinding = new TypedBinding<DashboardItem, double>(delegate(DashboardItem A_0)
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
			BindingBase bindingBase11 = bindingExtension11.ProvideValue(null);
			sfLinearProgressBar.SetBinding(ProgressBarBase.MinimumProperty, bindingBase11);
			bindingExtension12.Mode = 2;
			staticResourceExtension3.Key = "ExcludeInfinityFromDoubleConverter";
			IMarkupExtension markupExtension3 = staticResourceExtension3;
			XamlServiceProvider xamlServiceProvider3 = new XamlServiceProvider();
			Type typeFromHandle5 = typeof(IProvideValueTarget);
			object[] array3 = new object[0 + 5];
			array3[0] = bindingExtension12;
			array3[1] = sfLinearProgressBar;
			array3[2] = grid;
			array3[3] = grid2;
			array3[4] = this;
			object obj5;
			xamlServiceProvider3.Add(typeFromHandle5, obj5 = new SimpleValueTargetProvider(array3, typeof(BindingExtension).GetRuntimeProperty("Converter"), nameScope));
			xamlServiceProvider3.Add(typeof(IReferenceProvider), obj5);
			Type typeFromHandle6 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver3 = new XmlNamespaceResolver();
			xmlNamespaceResolver3.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver3.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver3.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver3.Add("dash", "clr-namespace:CarScannerXamarinForms.Dashboard");
			xmlNamespaceResolver3.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver3.Add("progressBar", "clr-namespace:Syncfusion.XForms.ProgressBar;assembly=Syncfusion.SfProgressBar.XForms");
			xamlServiceProvider3.Add(typeFromHandle6, new XamlTypeResolver(xmlNamespaceResolver3, typeof(LinearProgressItemiOS).GetTypeInfo().Assembly));
			xamlServiceProvider3.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(74, 21)));
			object obj6 = markupExtension3.ProvideValue(xamlServiceProvider3);
			bindingExtension12.Converter = obj6;
			bindingExtension12.Path = "Model.FloatValue";
			bindingExtension12.TypedBinding = new TypedBinding<DashboardItem, double>(delegate(DashboardItem A_0)
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
			BindingBase bindingBase12 = bindingExtension12.ProvideValue(null);
			sfLinearProgressBar.SetBinding(ProgressBarBase.ProgressProperty, bindingBase12);
			bindingExtension13.Mode = 2;
			bindingExtension13.Path = "GaugePointerColor";
			bindingExtension13.TypedBinding = new TypedBinding<DashboardItem, Color>(delegate(DashboardItem A_0)
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
			BindingBase bindingBase13 = bindingExtension13.ProvideValue(null);
			sfLinearProgressBar.SetBinding(ProgressBarBase.ProgressColorProperty, bindingBase13);
			bindingExtension14.Mode = 2;
			bindingExtension14.Path = "SegmentCount";
			bindingExtension14.TypedBinding = new TypedBinding<DashboardItem, int>(delegate(DashboardItem A_0)
			{
				if (A_0 != null)
				{
					return new ValueTuple<int, bool>(A_0.SegmentCount, true);
				}
				return default(ValueTuple<int, bool>);
			}, null, new Tuple<Func<DashboardItem, object>, string>[]
			{
				new Tuple<Func<DashboardItem, object>, string>((DashboardItem A_0) => A_0, "SegmentCount")
			});
			BindingBase bindingBase14 = bindingExtension14.ProvideValue(null);
			sfLinearProgressBar.SetBinding(ProgressBarBase.SegmentCountProperty, bindingBase14);
			bindingExtension15.Mode = 2;
			bindingExtension15.Path = "GaugeRimColor";
			bindingExtension15.TypedBinding = new TypedBinding<DashboardItem, Color>(delegate(DashboardItem A_0)
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
			BindingBase bindingBase15 = bindingExtension15.ProvideValue(null);
			sfLinearProgressBar.SetBinding(ProgressBarBase.TrackColorProperty, bindingBase15);
			bindingExtension16.Mode = 2;
			bindingExtension16.Path = "LinearScaleSize";
			bindingExtension16.TypedBinding = new TypedBinding<DashboardItem, double>(delegate(DashboardItem A_0)
			{
				if (A_0 != null)
				{
					return new ValueTuple<double, bool>(A_0.LinearScaleSize, true);
				}
				return default(ValueTuple<double, bool>);
			}, null, new Tuple<Func<DashboardItem, object>, string>[]
			{
				new Tuple<Func<DashboardItem, object>, string>((DashboardItem A_0) => A_0, "LinearScaleSize")
			});
			BindingBase bindingBase16 = bindingExtension16.ProvideValue(null);
			sfLinearProgressBar.SetBinding(SfLinearProgressBar.TrackHeightProperty, bindingBase16);
			staticResourceExtension4.Key = "ProgressBarWidthToPaddingConverter";
			IMarkupExtension markupExtension4 = staticResourceExtension4;
			XamlServiceProvider xamlServiceProvider4 = new XamlServiceProvider();
			Type typeFromHandle7 = typeof(IProvideValueTarget);
			object[] array4 = new object[0 + 5];
			array4[0] = bindingExtension17;
			array4[1] = sfLinearProgressBar;
			array4[2] = grid;
			array4[3] = grid2;
			array4[4] = this;
			object obj7;
			xamlServiceProvider4.Add(typeFromHandle7, obj7 = new SimpleValueTargetProvider(array4, typeof(BindingExtension).GetRuntimeProperty("Converter"), nameScope));
			xamlServiceProvider4.Add(typeof(IReferenceProvider), obj7);
			Type typeFromHandle8 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver4 = new XmlNamespaceResolver();
			xmlNamespaceResolver4.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver4.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver4.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver4.Add("dash", "clr-namespace:CarScannerXamarinForms.Dashboard");
			xmlNamespaceResolver4.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver4.Add("progressBar", "clr-namespace:Syncfusion.XForms.ProgressBar;assembly=Syncfusion.SfProgressBar.XForms");
			xamlServiceProvider4.Add(typeFromHandle8, new XamlTypeResolver(xmlNamespaceResolver4, typeof(LinearProgressItemiOS).GetTypeInfo().Assembly));
			xamlServiceProvider4.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(81, 29)));
			object obj8 = markupExtension4.ProvideValue(xamlServiceProvider4);
			bindingExtension17.Converter = obj8;
			bindingExtension17.Mode = 4;
			bindingExtension17.Path = "LinearScaleSize";
			on.Platform = new List<string>(1) { "iOS" };
			on.Value = "OneWay";
			onPlatform.Platforms.Add(on);
			on2.Platform = new List<string>(1) { "Android" };
			on2.Value = "OneWay";
			onPlatform.Platforms.Add(on2);
			bindingExtension17.Mode = onPlatform;
			bindingExtension17.ConverterParameter = num;
			bindingExtension17.TypedBinding = new TypedBinding<DashboardItem, double>(delegate(DashboardItem A_0)
			{
				if (A_0 != null)
				{
					return new ValueTuple<double, bool>(A_0.LinearScaleSize, true);
				}
				return default(ValueTuple<double, bool>);
			}, null, null);
			BindingBase bindingBase17 = bindingExtension17.ProvideValue(null);
			sfLinearProgressBar.SetBinding(SfLinearProgressBar.PaddingProperty, bindingBase17);
			grid.Children.Add(sfLinearProgressBar);
			grid2.Children.Add(grid);
			stackLayout.SetValue(Grid.RowProperty, 2);
			stackLayout.SetValue(View.HorizontalOptionsProperty, LayoutOptions.Center);
			bindingExtension18.Mode = 2;
			bindingExtension18.Path = "ShowValue";
			bindingExtension18.TypedBinding = new TypedBinding<DashboardItem, bool>(delegate(DashboardItem A_0)
			{
				if (A_0 != null)
				{
					return new ValueTuple<bool, bool>(A_0.ShowValue, true);
				}
				return default(ValueTuple<bool, bool>);
			}, null, new Tuple<Func<DashboardItem, object>, string>[]
			{
				new Tuple<Func<DashboardItem, object>, string>((DashboardItem A_0) => A_0, "ShowValue")
			});
			BindingBase bindingBase18 = bindingExtension18.ProvideValue(null);
			stackLayout.SetBinding(VisualElement.IsVisibleProperty, bindingBase18);
			stackLayout.SetValue(StackLayout.OrientationProperty, 1);
			stackLayout.SetValue(View.VerticalOptionsProperty, LayoutOptions.End);
			bindingExtension19.Mode = 2;
			bindingExtension19.Path = "FontName";
			bindingExtension19.TypedBinding = new TypedBinding<DashboardItem, string>(delegate(DashboardItem A_0)
			{
				if (A_0 != null)
				{
					return new ValueTuple<string, bool>(A_0.FontName, true);
				}
				return default(ValueTuple<string, bool>);
			}, null, new Tuple<Func<DashboardItem, object>, string>[]
			{
				new Tuple<Func<DashboardItem, object>, string>((DashboardItem A_0) => A_0, "FontName")
			});
			BindingBase bindingBase19 = bindingExtension19.ProvideValue(null);
			label2.SetBinding(Label.FontFamilyProperty, bindingBase19);
			bindingExtension20.Mode = 2;
			bindingExtension20.Path = "ValueFontSize";
			bindingExtension20.TypedBinding = new TypedBinding<DashboardItem, double>(delegate(DashboardItem A_0)
			{
				if (A_0 != null)
				{
					return new ValueTuple<double, bool>(A_0.ValueFontSize, true);
				}
				return default(ValueTuple<double, bool>);
			}, null, new Tuple<Func<DashboardItem, object>, string>[]
			{
				new Tuple<Func<DashboardItem, object>, string>((DashboardItem A_0) => A_0, "ValueFontSize")
			});
			BindingBase bindingBase20 = bindingExtension20.ProvideValue(null);
			label2.SetBinding(Label.FontSizeProperty, bindingBase20);
			label2.SetValue(View.HorizontalOptionsProperty, LayoutOptions.Center);
			label2.SetValue(Label.HorizontalTextAlignmentProperty, new TextAlignmentConverter().ConvertFromInvariantString("Center"));
			staticResourceExtension5.Key = "FloatItemToLineBreakModeConverter";
			IMarkupExtension markupExtension5 = staticResourceExtension5;
			XamlServiceProvider xamlServiceProvider5 = new XamlServiceProvider();
			Type typeFromHandle9 = typeof(IProvideValueTarget);
			object[] array5 = new object[0 + 5];
			array5[0] = bindingExtension21;
			array5[1] = label2;
			array5[2] = stackLayout;
			array5[3] = grid2;
			array5[4] = this;
			object obj9;
			xamlServiceProvider5.Add(typeFromHandle9, obj9 = new SimpleValueTargetProvider(array5, typeof(BindingExtension).GetRuntimeProperty("Converter"), nameScope));
			xamlServiceProvider5.Add(typeof(IReferenceProvider), obj9);
			Type typeFromHandle10 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver5 = new XmlNamespaceResolver();
			xmlNamespaceResolver5.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver5.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver5.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver5.Add("dash", "clr-namespace:CarScannerXamarinForms.Dashboard");
			xmlNamespaceResolver5.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver5.Add("progressBar", "clr-namespace:Syncfusion.XForms.ProgressBar;assembly=Syncfusion.SfProgressBar.XForms");
			xamlServiceProvider5.Add(typeFromHandle10, new XamlTypeResolver(xmlNamespaceResolver5, typeof(LinearProgressItemiOS).GetTypeInfo().Assembly));
			xamlServiceProvider5.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(113, 21)));
			object obj10 = markupExtension5.ProvideValue(xamlServiceProvider5);
			bindingExtension21.Converter = obj10;
			bindingExtension21.Path = "Model.ChartVisible";
			bindingExtension21.TypedBinding = new TypedBinding<DashboardItem, bool>(delegate(DashboardItem A_0)
			{
				if (A_0 != null)
				{
					LiveDataPIDModel model2 = A_0.Model;
					if (model2 != null)
					{
						return new ValueTuple<bool, bool>(model2.ChartVisible, true);
					}
				}
				return default(ValueTuple<bool, bool>);
			}, delegate(DashboardItem A_0, bool A_1)
			{
				if (A_0 != null)
				{
					LiveDataPIDModel model3 = A_0.Model;
					if (model3 != null)
					{
						model3.ChartVisible = A_1;
						return;
					}
				}
			}, new Tuple<Func<DashboardItem, object>, string>[]
			{
				new Tuple<Func<DashboardItem, object>, string>((DashboardItem A_0) => A_0, "Model"),
				new Tuple<Func<DashboardItem, object>, string>((DashboardItem A_0) => A_0.Model, "ChartVisible")
			});
			BindingBase bindingBase21 = bindingExtension21.ProvideValue(null);
			label2.SetBinding(Label.LineBreakModeProperty, bindingBase21);
			bindingExtension22.Mode = 2;
			bindingExtension22.Path = "Model.TextValue";
			bindingExtension22.TypedBinding = new TypedBinding<DashboardItem, string>(delegate(DashboardItem A_0)
			{
				if (A_0 != null)
				{
					LiveDataPIDModel model4 = A_0.Model;
					if (model4 != null)
					{
						return new ValueTuple<string, bool>(model4.TextValue, true);
					}
				}
				return default(ValueTuple<string, bool>);
			}, null, new Tuple<Func<DashboardItem, object>, string>[]
			{
				new Tuple<Func<DashboardItem, object>, string>((DashboardItem A_0) => A_0, "Model"),
				new Tuple<Func<DashboardItem, object>, string>((DashboardItem A_0) => A_0.Model, "TextValue")
			});
			BindingBase bindingBase22 = bindingExtension22.ProvideValue(null);
			label2.SetBinding(Label.TextProperty, bindingBase22);
			bindingExtension23.Mode = 2;
			bindingExtension23.Path = "ValueTextColor";
			bindingExtension23.TypedBinding = new TypedBinding<DashboardItem, Color>(delegate(DashboardItem A_0)
			{
				if (A_0 != null)
				{
					return new ValueTuple<Color, bool>(A_0.ValueTextColor, true);
				}
				return default(ValueTuple<Color, bool>);
			}, null, new Tuple<Func<DashboardItem, object>, string>[]
			{
				new Tuple<Func<DashboardItem, object>, string>((DashboardItem A_0) => A_0, "ValueTextColor")
			});
			BindingBase bindingBase23 = bindingExtension23.ProvideValue(null);
			label2.SetBinding(Label.TextColorProperty, bindingBase23);
			label2.SetValue(View.VerticalOptionsProperty, LayoutOptions.Center);
			label2.SetValue(Label.VerticalTextAlignmentProperty, new TextAlignmentConverter().ConvertFromInvariantString("Center"));
			stackLayout.Children.Add(label2);
			bindingExtension24.Mode = 2;
			bindingExtension24.Path = "UnitsFontSize";
			bindingExtension24.TypedBinding = new TypedBinding<DashboardItem, double>(delegate(DashboardItem A_0)
			{
				if (A_0 != null)
				{
					return new ValueTuple<double, bool>(A_0.UnitsFontSize, true);
				}
				return default(ValueTuple<double, bool>);
			}, null, new Tuple<Func<DashboardItem, object>, string>[]
			{
				new Tuple<Func<DashboardItem, object>, string>((DashboardItem A_0) => A_0, "UnitsFontSize")
			});
			BindingBase bindingBase24 = bindingExtension24.ProvideValue(null);
			label3.SetBinding(Label.FontSizeProperty, bindingBase24);
			label3.SetValue(View.HorizontalOptionsProperty, LayoutOptions.CenterAndExpand);
			label3.SetValue(Label.HorizontalTextAlignmentProperty, new TextAlignmentConverter().ConvertFromInvariantString("Center"));
			label3.SetValue(Label.LineBreakModeProperty, 0);
			bindingExtension25.Mode = 2;
			bindingExtension25.Path = "Model.Units";
			bindingExtension25.TypedBinding = new TypedBinding<DashboardItem, string>(delegate(DashboardItem A_0)
			{
				if (A_0 != null)
				{
					LiveDataPIDModel model5 = A_0.Model;
					if (model5 != null)
					{
						return new ValueTuple<string, bool>(model5.Units, true);
					}
				}
				return default(ValueTuple<string, bool>);
			}, null, new Tuple<Func<DashboardItem, object>, string>[]
			{
				new Tuple<Func<DashboardItem, object>, string>((DashboardItem A_0) => A_0, "Model"),
				new Tuple<Func<DashboardItem, object>, string>((DashboardItem A_0) => A_0.Model, "Units")
			});
			BindingBase bindingBase25 = bindingExtension25.ProvideValue(null);
			label3.SetBinding(Label.TextProperty, bindingBase25);
			bindingExtension26.Mode = 2;
			bindingExtension26.Path = "UnitsTextColor";
			bindingExtension26.TypedBinding = new TypedBinding<DashboardItem, Color>(delegate(DashboardItem A_0)
			{
				if (A_0 != null)
				{
					return new ValueTuple<Color, bool>(A_0.UnitsTextColor, true);
				}
				return default(ValueTuple<Color, bool>);
			}, null, new Tuple<Func<DashboardItem, object>, string>[]
			{
				new Tuple<Func<DashboardItem, object>, string>((DashboardItem A_0) => A_0, "UnitsTextColor")
			});
			BindingBase bindingBase26 = bindingExtension26.ProvideValue(null);
			label3.SetBinding(Label.TextColorProperty, bindingBase26);
			label3.SetValue(View.VerticalOptionsProperty, LayoutOptions.Center);
			label3.SetValue(Label.VerticalTextAlignmentProperty, new TextAlignmentConverter().ConvertFromInvariantString("Center"));
			stackLayout.Children.Add(label3);
			grid2.Children.Add(stackLayout);
			stackLayout2.SetValue(Grid.RowProperty, 1);
			stackLayout2.SetValue(View.MarginProperty, new Thickness(0.0));
			stackLayout2.SetValue(View.HorizontalOptionsProperty, LayoutOptions.End);
			bindingExtension27.Mode = 2;
			bindingExtension27.Path = "Model.Settings.ShowPing";
			bindingExtension27.TypedBinding = new TypedBinding<DashboardItem, bool>(delegate(DashboardItem A_0)
			{
				if (A_0 != null)
				{
					LiveDataPIDModel model6 = A_0.Model;
					if (model6 != null)
					{
						SharedSettings settings = model6.Settings;
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
			BindingBase bindingBase27 = bindingExtension27.ProvideValue(null);
			stackLayout2.SetBinding(VisualElement.IsVisibleProperty, bindingBase27);
			stackLayout2.SetValue(StackLayout.OrientationProperty, 1);
			stackLayout2.SetValue(StackLayout.SpacingProperty, 0.0);
			stackLayout2.SetValue(View.VerticalOptionsProperty, LayoutOptions.End);
			label4.SetValue(Label.FontAttributesProperty, new FontAttributesConverter().ConvertFromInvariantString("Bold"));
			dynamicResourceExtension.Key = "BaseFontSize--";
			IMarkupExtension<DynamicResource> markupExtension6 = dynamicResourceExtension;
			XamlServiceProvider xamlServiceProvider6 = new XamlServiceProvider();
			Type typeFromHandle11 = typeof(IProvideValueTarget);
			object[] array6 = new object[0 + 4];
			array6[0] = label4;
			array6[1] = stackLayout2;
			array6[2] = grid2;
			array6[3] = this;
			object obj11;
			xamlServiceProvider6.Add(typeFromHandle11, obj11 = new SimpleValueTargetProvider(array6, Label.FontSizeProperty, nameScope));
			xamlServiceProvider6.Add(typeof(IReferenceProvider), obj11);
			Type typeFromHandle12 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver6 = new XmlNamespaceResolver();
			xmlNamespaceResolver6.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver6.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver6.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver6.Add("dash", "clr-namespace:CarScannerXamarinForms.Dashboard");
			xmlNamespaceResolver6.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver6.Add("progressBar", "clr-namespace:Syncfusion.XForms.ProgressBar;assembly=Syncfusion.SfProgressBar.XForms");
			xamlServiceProvider6.Add(typeFromHandle12, new XamlTypeResolver(xmlNamespaceResolver6, typeof(LinearProgressItemiOS).GetTypeInfo().Assembly));
			xamlServiceProvider6.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(141, 21)));
			DynamicResource dynamicResource = markupExtension6.ProvideValue(xamlServiceProvider6);
			label4.SetDynamicResource(Label.FontSizeProperty, dynamicResource.Key);
			bindingExtension28.Mode = 2;
			bindingExtension28.Path = "Model.Ping";
			bindingExtension28.TypedBinding = new TypedBinding<DashboardItem, long>(delegate(DashboardItem A_0)
			{
				if (A_0 != null)
				{
					LiveDataPIDModel model7 = A_0.Model;
					if (model7 != null)
					{
						return new ValueTuple<long, bool>(model7.Ping, true);
					}
				}
				return default(ValueTuple<long, bool>);
			}, null, new Tuple<Func<DashboardItem, object>, string>[]
			{
				new Tuple<Func<DashboardItem, object>, string>((DashboardItem A_0) => A_0, "Model"),
				new Tuple<Func<DashboardItem, object>, string>((DashboardItem A_0) => A_0.Model, "Ping")
			});
			BindingBase bindingBase28 = bindingExtension28.ProvideValue(null);
			label4.SetBinding(Label.TextProperty, bindingBase28);
			label4.SetValue(Label.TextColorProperty, Color.Red);
			stackLayout2.Children.Add(label4);
			label5.SetValue(Label.FontAttributesProperty, new FontAttributesConverter().ConvertFromInvariantString("Bold"));
			dynamicResourceExtension2.Key = "BaseFontSize--";
			IMarkupExtension<DynamicResource> markupExtension7 = dynamicResourceExtension2;
			XamlServiceProvider xamlServiceProvider7 = new XamlServiceProvider();
			Type typeFromHandle13 = typeof(IProvideValueTarget);
			object[] array7 = new object[0 + 4];
			array7[0] = label5;
			array7[1] = stackLayout2;
			array7[2] = grid2;
			array7[3] = this;
			object obj12;
			xamlServiceProvider7.Add(typeFromHandle13, obj12 = new SimpleValueTargetProvider(array7, Label.FontSizeProperty, nameScope));
			xamlServiceProvider7.Add(typeof(IReferenceProvider), obj12);
			Type typeFromHandle14 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver7 = new XmlNamespaceResolver();
			xmlNamespaceResolver7.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver7.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver7.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver7.Add("dash", "clr-namespace:CarScannerXamarinForms.Dashboard");
			xmlNamespaceResolver7.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver7.Add("progressBar", "clr-namespace:Syncfusion.XForms.ProgressBar;assembly=Syncfusion.SfProgressBar.XForms");
			xamlServiceProvider7.Add(typeFromHandle14, new XamlTypeResolver(xmlNamespaceResolver7, typeof(LinearProgressItemiOS).GetTypeInfo().Assembly));
			xamlServiceProvider7.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(146, 21)));
			DynamicResource dynamicResource2 = markupExtension7.ProvideValue(xamlServiceProvider7);
			label5.SetDynamicResource(Label.FontSizeProperty, dynamicResource2.Key);
			label5.SetValue(Label.TextProperty, "ms");
			label5.SetValue(Label.TextColorProperty, Color.Red);
			stackLayout2.Children.Add(label5);
			grid2.Children.Add(stackLayout2);
			this.SetValue(ContentView.ContentProperty, grid2);
		}

		// Token: 0x060044CE RID: 17614 RVA: 0x0035C2B5 File Offset: 0x0035A4B5
		private void __InitComponentRuntime()
		{
			Extensions.LoadFromXaml<LinearProgressItemiOS>(this, typeof(LinearProgressItemiOS));
			this.labelValue = NameScopeExtensions.FindByName<Label>(this, "labelValue");
		}

		// Token: 0x060044CF RID: 17615 RVA: 0x0035C2DC File Offset: 0x0035A4DC
		[CompilerGenerated]
		private static ValueTuple<bool, bool> <InitializeComponent>typedBindingsM__699(DashboardItem A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<bool, bool>(A_0.ShowDefaultBackground, true);
			}
			return default(ValueTuple<bool, bool>);
		}

		// Token: 0x060044D0 RID: 17616 RVA: 0x0035C30C File Offset: 0x0035A50C
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__700(DashboardItem A_0)
		{
			return A_0;
		}

		// Token: 0x060044D1 RID: 17617 RVA: 0x0035C31C File Offset: 0x0035A51C
		[CompilerGenerated]
		private static ValueTuple<Thickness, bool> <InitializeComponent>typedBindingsM__701(DashboardItem A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<Thickness, bool>(A_0.CornerRadius, true);
			}
			return default(ValueTuple<Thickness, bool>);
		}

		// Token: 0x060044D2 RID: 17618 RVA: 0x0035C34C File Offset: 0x0035A54C
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__702(DashboardItem A_0)
		{
			return A_0;
		}

		// Token: 0x060044D3 RID: 17619 RVA: 0x0035C35C File Offset: 0x0035A55C
		[CompilerGenerated]
		private static ValueTuple<Color, bool> <InitializeComponent>typedBindingsM__703(DashboardItem A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<Color, bool>(A_0.IndicatorBackgroundColor, true);
			}
			return default(ValueTuple<Color, bool>);
		}

		// Token: 0x060044D4 RID: 17620 RVA: 0x0035C38C File Offset: 0x0035A58C
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__704(DashboardItem A_0)
		{
			return A_0;
		}

		// Token: 0x060044D5 RID: 17621 RVA: 0x0035C39C File Offset: 0x0035A59C
		[CompilerGenerated]
		private static ValueTuple<Color, bool> <InitializeComponent>typedBindingsM__705(DashboardItem A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<Color, bool>(A_0.IndicatorBackgroundColor, true);
			}
			return default(ValueTuple<Color, bool>);
		}

		// Token: 0x060044D6 RID: 17622 RVA: 0x0035C3CC File Offset: 0x0035A5CC
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__706(DashboardItem A_0)
		{
			return A_0;
		}

		// Token: 0x060044D7 RID: 17623 RVA: 0x0035C3DC File Offset: 0x0035A5DC
		[CompilerGenerated]
		private static ValueTuple<double, bool> <InitializeComponent>typedBindingsM__707(DashboardItem A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<double, bool>(A_0.TitleFontSize, true);
			}
			return default(ValueTuple<double, bool>);
		}

		// Token: 0x060044D8 RID: 17624 RVA: 0x0035C40C File Offset: 0x0035A60C
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__708(DashboardItem A_0)
		{
			return A_0;
		}

		// Token: 0x060044D9 RID: 17625 RVA: 0x0035C41C File Offset: 0x0035A61C
		[CompilerGenerated]
		private static ValueTuple<string, bool> <InitializeComponent>typedBindingsM__709(DashboardItem A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<string, bool>(A_0.PIDName, true);
			}
			return default(ValueTuple<string, bool>);
		}

		// Token: 0x060044DA RID: 17626 RVA: 0x0035C44C File Offset: 0x0035A64C
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__710(DashboardItem A_0)
		{
			return A_0;
		}

		// Token: 0x060044DB RID: 17627 RVA: 0x0035C45C File Offset: 0x0035A65C
		[CompilerGenerated]
		private static ValueTuple<Color, bool> <InitializeComponent>typedBindingsM__711(DashboardItem A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<Color, bool>(A_0.TitleTextColor, true);
			}
			return default(ValueTuple<Color, bool>);
		}

		// Token: 0x060044DC RID: 17628 RVA: 0x0035C48C File Offset: 0x0035A68C
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__712(DashboardItem A_0)
		{
			return A_0;
		}

		// Token: 0x060044DD RID: 17629 RVA: 0x0035C49C File Offset: 0x0035A69C
		[CompilerGenerated]
		private static ValueTuple<double, bool> <InitializeComponent>typedBindingsM__713(DashboardItem A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<double, bool>(A_0.LinearScaleSize, true);
			}
			return default(ValueTuple<double, bool>);
		}

		// Token: 0x060044DE RID: 17630 RVA: 0x0035C4CC File Offset: 0x0035A6CC
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__714(DashboardItem A_0)
		{
			return A_0;
		}

		// Token: 0x060044DF RID: 17631 RVA: 0x0035C4DC File Offset: 0x0035A6DC
		[CompilerGenerated]
		private static ValueTuple<double, bool> <InitializeComponent>typedBindingsM__715(DashboardItem A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<double, bool>(A_0.Interval, true);
			}
			return default(ValueTuple<double, bool>);
		}

		// Token: 0x060044E0 RID: 17632 RVA: 0x0035C50C File Offset: 0x0035A70C
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__716(DashboardItem A_0)
		{
			return A_0;
		}

		// Token: 0x060044E1 RID: 17633 RVA: 0x0035C51C File Offset: 0x0035A71C
		[CompilerGenerated]
		private static ValueTuple<double, bool> <InitializeComponent>typedBindingsM__717(DashboardItem A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<double, bool>(A_0.Maximum, true);
			}
			return default(ValueTuple<double, bool>);
		}

		// Token: 0x060044E2 RID: 17634 RVA: 0x0035C54C File Offset: 0x0035A74C
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__718(DashboardItem A_0)
		{
			return A_0;
		}

		// Token: 0x060044E3 RID: 17635 RVA: 0x0035C55C File Offset: 0x0035A75C
		[CompilerGenerated]
		private static ValueTuple<double, bool> <InitializeComponent>typedBindingsM__719(DashboardItem A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<double, bool>(A_0.Minimum, true);
			}
			return default(ValueTuple<double, bool>);
		}

		// Token: 0x060044E4 RID: 17636 RVA: 0x0035C58C File Offset: 0x0035A78C
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__720(DashboardItem A_0)
		{
			return A_0;
		}

		// Token: 0x060044E5 RID: 17637 RVA: 0x0035C59C File Offset: 0x0035A79C
		[CompilerGenerated]
		private static ValueTuple<double, bool> <InitializeComponent>typedBindingsM__721(DashboardItem A_0)
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

		// Token: 0x060044E6 RID: 17638 RVA: 0x0035C5D4 File Offset: 0x0035A7D4
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__722(DashboardItem A_0)
		{
			return A_0;
		}

		// Token: 0x060044E7 RID: 17639 RVA: 0x0035C5E4 File Offset: 0x0035A7E4
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__723(DashboardItem A_0)
		{
			return A_0.Model;
		}

		// Token: 0x060044E8 RID: 17640 RVA: 0x0035C5F8 File Offset: 0x0035A7F8
		[CompilerGenerated]
		private static ValueTuple<Color, bool> <InitializeComponent>typedBindingsM__724(DashboardItem A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<Color, bool>(A_0.GaugePointerColor, true);
			}
			return default(ValueTuple<Color, bool>);
		}

		// Token: 0x060044E9 RID: 17641 RVA: 0x0035C628 File Offset: 0x0035A828
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__725(DashboardItem A_0)
		{
			return A_0;
		}

		// Token: 0x060044EA RID: 17642 RVA: 0x0035C638 File Offset: 0x0035A838
		[CompilerGenerated]
		private static ValueTuple<int, bool> <InitializeComponent>typedBindingsM__726(DashboardItem A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<int, bool>(A_0.SegmentCount, true);
			}
			return default(ValueTuple<int, bool>);
		}

		// Token: 0x060044EB RID: 17643 RVA: 0x0035C668 File Offset: 0x0035A868
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__727(DashboardItem A_0)
		{
			return A_0;
		}

		// Token: 0x060044EC RID: 17644 RVA: 0x0035C678 File Offset: 0x0035A878
		[CompilerGenerated]
		private static ValueTuple<Color, bool> <InitializeComponent>typedBindingsM__728(DashboardItem A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<Color, bool>(A_0.GaugeRimColor, true);
			}
			return default(ValueTuple<Color, bool>);
		}

		// Token: 0x060044ED RID: 17645 RVA: 0x0035C6A8 File Offset: 0x0035A8A8
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__729(DashboardItem A_0)
		{
			return A_0;
		}

		// Token: 0x060044EE RID: 17646 RVA: 0x0035C6B8 File Offset: 0x0035A8B8
		[CompilerGenerated]
		private static ValueTuple<double, bool> <InitializeComponent>typedBindingsM__730(DashboardItem A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<double, bool>(A_0.LinearScaleSize, true);
			}
			return default(ValueTuple<double, bool>);
		}

		// Token: 0x060044EF RID: 17647 RVA: 0x0035C6E8 File Offset: 0x0035A8E8
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__731(DashboardItem A_0)
		{
			return A_0;
		}

		// Token: 0x060044F0 RID: 17648 RVA: 0x0035C6F8 File Offset: 0x0035A8F8
		[CompilerGenerated]
		private static ValueTuple<double, bool> <InitializeComponent>typedBindingsM__732(DashboardItem A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<double, bool>(A_0.LinearScaleSize, true);
			}
			return default(ValueTuple<double, bool>);
		}

		// Token: 0x060044F1 RID: 17649 RVA: 0x0035C728 File Offset: 0x0035A928
		[CompilerGenerated]
		private static ValueTuple<bool, bool> <InitializeComponent>typedBindingsM__733(DashboardItem A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<bool, bool>(A_0.ShowValue, true);
			}
			return default(ValueTuple<bool, bool>);
		}

		// Token: 0x060044F2 RID: 17650 RVA: 0x0035C758 File Offset: 0x0035A958
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__734(DashboardItem A_0)
		{
			return A_0;
		}

		// Token: 0x060044F3 RID: 17651 RVA: 0x0035C768 File Offset: 0x0035A968
		[CompilerGenerated]
		private static ValueTuple<string, bool> <InitializeComponent>typedBindingsM__735(DashboardItem A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<string, bool>(A_0.FontName, true);
			}
			return default(ValueTuple<string, bool>);
		}

		// Token: 0x060044F4 RID: 17652 RVA: 0x0035C798 File Offset: 0x0035A998
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__736(DashboardItem A_0)
		{
			return A_0;
		}

		// Token: 0x060044F5 RID: 17653 RVA: 0x0035C7A8 File Offset: 0x0035A9A8
		[CompilerGenerated]
		private static ValueTuple<double, bool> <InitializeComponent>typedBindingsM__737(DashboardItem A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<double, bool>(A_0.ValueFontSize, true);
			}
			return default(ValueTuple<double, bool>);
		}

		// Token: 0x060044F6 RID: 17654 RVA: 0x0035C7D8 File Offset: 0x0035A9D8
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__738(DashboardItem A_0)
		{
			return A_0;
		}

		// Token: 0x060044F7 RID: 17655 RVA: 0x0035C7E8 File Offset: 0x0035A9E8
		[CompilerGenerated]
		private static ValueTuple<bool, bool> <InitializeComponent>typedBindingsM__739(DashboardItem A_0)
		{
			if (A_0 != null)
			{
				LiveDataPIDModel model = A_0.Model;
				if (model != null)
				{
					return new ValueTuple<bool, bool>(model.ChartVisible, true);
				}
			}
			return default(ValueTuple<bool, bool>);
		}

		// Token: 0x060044F8 RID: 17656 RVA: 0x0035C820 File Offset: 0x0035AA20
		[CompilerGenerated]
		private static void <InitializeComponent>typedBindingsM__740(DashboardItem A_0, bool A_1)
		{
			if (A_0 != null)
			{
				LiveDataPIDModel model = A_0.Model;
				if (model != null)
				{
					model.ChartVisible = A_1;
					return;
				}
			}
		}

		// Token: 0x060044F9 RID: 17657 RVA: 0x0035C848 File Offset: 0x0035AA48
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__741(DashboardItem A_0)
		{
			return A_0;
		}

		// Token: 0x060044FA RID: 17658 RVA: 0x0035C858 File Offset: 0x0035AA58
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__742(DashboardItem A_0)
		{
			return A_0.Model;
		}

		// Token: 0x060044FB RID: 17659 RVA: 0x0035C86C File Offset: 0x0035AA6C
		[CompilerGenerated]
		private static ValueTuple<string, bool> <InitializeComponent>typedBindingsM__743(DashboardItem A_0)
		{
			if (A_0 != null)
			{
				LiveDataPIDModel model = A_0.Model;
				if (model != null)
				{
					return new ValueTuple<string, bool>(model.TextValue, true);
				}
			}
			return default(ValueTuple<string, bool>);
		}

		// Token: 0x060044FC RID: 17660 RVA: 0x0035C8A4 File Offset: 0x0035AAA4
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__744(DashboardItem A_0)
		{
			return A_0;
		}

		// Token: 0x060044FD RID: 17661 RVA: 0x0035C8B4 File Offset: 0x0035AAB4
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__745(DashboardItem A_0)
		{
			return A_0.Model;
		}

		// Token: 0x060044FE RID: 17662 RVA: 0x0035C8C8 File Offset: 0x0035AAC8
		[CompilerGenerated]
		private static ValueTuple<Color, bool> <InitializeComponent>typedBindingsM__746(DashboardItem A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<Color, bool>(A_0.ValueTextColor, true);
			}
			return default(ValueTuple<Color, bool>);
		}

		// Token: 0x060044FF RID: 17663 RVA: 0x0035C8F8 File Offset: 0x0035AAF8
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__747(DashboardItem A_0)
		{
			return A_0;
		}

		// Token: 0x06004500 RID: 17664 RVA: 0x0035C908 File Offset: 0x0035AB08
		[CompilerGenerated]
		private static ValueTuple<double, bool> <InitializeComponent>typedBindingsM__748(DashboardItem A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<double, bool>(A_0.UnitsFontSize, true);
			}
			return default(ValueTuple<double, bool>);
		}

		// Token: 0x06004501 RID: 17665 RVA: 0x0035C938 File Offset: 0x0035AB38
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__749(DashboardItem A_0)
		{
			return A_0;
		}

		// Token: 0x06004502 RID: 17666 RVA: 0x0035C948 File Offset: 0x0035AB48
		[CompilerGenerated]
		private static ValueTuple<string, bool> <InitializeComponent>typedBindingsM__750(DashboardItem A_0)
		{
			if (A_0 != null)
			{
				LiveDataPIDModel model = A_0.Model;
				if (model != null)
				{
					return new ValueTuple<string, bool>(model.Units, true);
				}
			}
			return default(ValueTuple<string, bool>);
		}

		// Token: 0x06004503 RID: 17667 RVA: 0x0035C980 File Offset: 0x0035AB80
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__751(DashboardItem A_0)
		{
			return A_0;
		}

		// Token: 0x06004504 RID: 17668 RVA: 0x0035C990 File Offset: 0x0035AB90
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__752(DashboardItem A_0)
		{
			return A_0.Model;
		}

		// Token: 0x06004505 RID: 17669 RVA: 0x0035C9A4 File Offset: 0x0035ABA4
		[CompilerGenerated]
		private static ValueTuple<Color, bool> <InitializeComponent>typedBindingsM__753(DashboardItem A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<Color, bool>(A_0.UnitsTextColor, true);
			}
			return default(ValueTuple<Color, bool>);
		}

		// Token: 0x06004506 RID: 17670 RVA: 0x0035C9D4 File Offset: 0x0035ABD4
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__754(DashboardItem A_0)
		{
			return A_0;
		}

		// Token: 0x06004507 RID: 17671 RVA: 0x0035C9E4 File Offset: 0x0035ABE4
		[CompilerGenerated]
		private static ValueTuple<bool, bool> <InitializeComponent>typedBindingsM__755(DashboardItem A_0)
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

		// Token: 0x06004508 RID: 17672 RVA: 0x0035CA28 File Offset: 0x0035AC28
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__756(DashboardItem A_0)
		{
			return A_0;
		}

		// Token: 0x06004509 RID: 17673 RVA: 0x0035CA38 File Offset: 0x0035AC38
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__757(DashboardItem A_0)
		{
			return A_0.Model;
		}

		// Token: 0x0600450A RID: 17674 RVA: 0x0035CA4C File Offset: 0x0035AC4C
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__758(DashboardItem A_0)
		{
			return A_0.Model.Settings;
		}

		// Token: 0x0600450B RID: 17675 RVA: 0x0035CA64 File Offset: 0x0035AC64
		[CompilerGenerated]
		private static ValueTuple<long, bool> <InitializeComponent>typedBindingsM__759(DashboardItem A_0)
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

		// Token: 0x0600450C RID: 17676 RVA: 0x0035CA9C File Offset: 0x0035AC9C
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__760(DashboardItem A_0)
		{
			return A_0;
		}

		// Token: 0x0600450D RID: 17677 RVA: 0x0035CAAC File Offset: 0x0035ACAC
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__761(DashboardItem A_0)
		{
			return A_0.Model;
		}

		// Token: 0x040028D3 RID: 10451
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private Label labelValue;
	}
}
