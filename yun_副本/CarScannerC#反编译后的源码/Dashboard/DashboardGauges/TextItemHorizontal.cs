using System;
using System.CodeDom.Compiler;
using System.Reflection;
using System.Runtime.CompilerServices;
using CarScannerXamarinForms.Common.XAMLConverters;
using CarScannerXamarinForms.Settings;
using CarScannerXamarinForms.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Internals;
using Xamarin.Forms.Xaml;
using Xamarin.Forms.Xaml.Diagnostics;
using Xamarin.Forms.Xaml.Internals;

namespace CarScannerXamarinForms.Dashboard.DashboardGauges
{
	// Token: 0x020007B8 RID: 1976
	[XamlCompilation(2)]
	[XamlFilePath("Dashboard\\DashboardGauges\\TextItemHorizontal.xaml")]
	public class TextItemHorizontal : ContentView
	{
		// Token: 0x060045FB RID: 17915 RVA: 0x00366B2F File Offset: 0x00364D2F
		public TextItemHorizontal()
		{
			this.InitializeComponent();
		}

		// Token: 0x060045FC RID: 17916 RVA: 0x00366B40 File Offset: 0x00364D40
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private void InitializeComponent()
		{
			if (ResourceLoader.IsEnabled && ResourceLoader.CanProvideContentFor(new ResourceLoader.ResourceLoadingQuery
			{
				AssemblyName = typeof(TextItemHorizontal).GetTypeInfo().Assembly.GetName(),
				ResourcePath = "Dashboard/DashboardGauges/TextItemHorizontal.xaml",
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
			BoolToNegativeConverter boolToNegativeConverter;
			VisualDiagnostics.RegisterSourceInfo(boolToNegativeConverter = new BoolToNegativeConverter(), new Uri("Dashboard\\DashboardGauges\\TextItemHorizontal.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 20, 14);
			FloatValueToColorConverter floatValueToColorConverter;
			VisualDiagnostics.RegisterSourceInfo(floatValueToColorConverter = new FloatValueToColorConverter(), new Uri("Dashboard\\DashboardGauges\\TextItemHorizontal.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 21, 14);
			TransparentColorToFalseConverter transparentColorToFalseConverter;
			VisualDiagnostics.RegisterSourceInfo(transparentColorToFalseConverter = new TransparentColorToFalseConverter(), new Uri("Dashboard\\DashboardGauges\\TextItemHorizontal.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 22, 14);
			CornerRadiusToThicknessConverter cornerRadiusToThicknessConverter;
			VisualDiagnostics.RegisterSourceInfo(cornerRadiusToThicknessConverter = new CornerRadiusToThicknessConverter(), new Uri("Dashboard\\DashboardGauges\\TextItemHorizontal.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 23, 14);
			ResourceDictionary resourceDictionary;
			VisualDiagnostics.RegisterSourceInfo(resourceDictionary = new ResourceDictionary(), new Uri("Dashboard\\DashboardGauges\\TextItemHorizontal.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 19, 10);
			ColumnDefinition columnDefinition;
			VisualDiagnostics.RegisterSourceInfo(columnDefinition = new ColumnDefinition(), new Uri("Dashboard\\DashboardGauges\\TextItemHorizontal.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 56, 18);
			ColumnDefinition columnDefinition2;
			VisualDiagnostics.RegisterSourceInfo(columnDefinition2 = new ColumnDefinition(), new Uri("Dashboard\\DashboardGauges\\TextItemHorizontal.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 57, 18);
			BindingExtension bindingExtension;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension = new BindingExtension(), new Uri("Dashboard\\DashboardGauges\\TextItemHorizontal.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 63, 17);
			GaugeBackground gaugeBackground;
			VisualDiagnostics.RegisterSourceInfo(gaugeBackground = new GaugeBackground(), new Uri("Dashboard\\DashboardGauges\\TextItemHorizontal.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 60, 14);
			StaticResourceExtension staticResourceExtension;
			VisualDiagnostics.RegisterSourceInfo(staticResourceExtension = new StaticResourceExtension(), new Uri("Dashboard\\DashboardGauges\\TextItemHorizontal.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 67, 17);
			BindingExtension bindingExtension2;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension2 = new BindingExtension(), new Uri("Dashboard\\DashboardGauges\\TextItemHorizontal.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 67, 17);
			StaticResourceExtension staticResourceExtension2;
			VisualDiagnostics.RegisterSourceInfo(staticResourceExtension2 = new StaticResourceExtension(), new Uri("Dashboard\\DashboardGauges\\TextItemHorizontal.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 69, 17);
			BindingExtension bindingExtension3;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension3 = new BindingExtension(), new Uri("Dashboard\\DashboardGauges\\TextItemHorizontal.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 69, 17);
			BindingExtension bindingExtension4;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension4 = new BindingExtension(), new Uri("Dashboard\\DashboardGauges\\TextItemHorizontal.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 71, 17);
			BoxView boxView;
			VisualDiagnostics.RegisterSourceInfo(boxView = new BoxView(), new Uri("Dashboard\\DashboardGauges\\TextItemHorizontal.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 65, 14);
			BindingExtension bindingExtension5;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension5 = new BindingExtension(), new Uri("Dashboard\\DashboardGauges\\TextItemHorizontal.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 75, 17);
			BindingExtension bindingExtension6;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension6 = new BindingExtension(), new Uri("Dashboard\\DashboardGauges\\TextItemHorizontal.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 77, 17);
			BindingExtension bindingExtension7;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension7 = new BindingExtension(), new Uri("Dashboard\\DashboardGauges\\TextItemHorizontal.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 78, 17);
			Label label;
			VisualDiagnostics.RegisterSourceInfo(label = new Label(), new Uri("Dashboard\\DashboardGauges\\TextItemHorizontal.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 73, 14);
			BindingExtension bindingExtension8;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension8 = new BindingExtension(), new Uri("Dashboard\\DashboardGauges\\TextItemHorizontal.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 88, 29);
			BindingExtension bindingExtension9;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension9 = new BindingExtension(), new Uri("Dashboard\\DashboardGauges\\TextItemHorizontal.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 89, 29);
			BindingExtension bindingExtension10;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension10 = new BindingExtension(), new Uri("Dashboard\\DashboardGauges\\TextItemHorizontal.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 90, 29);
			BindingExtension bindingExtension11;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension11 = new BindingExtension(), new Uri("Dashboard\\DashboardGauges\\TextItemHorizontal.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 91, 29);
			Span span;
			VisualDiagnostics.RegisterSourceInfo(span = new Span(), new Uri("Dashboard\\DashboardGauges\\TextItemHorizontal.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 87, 26);
			BindingExtension bindingExtension12;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension12 = new BindingExtension(), new Uri("Dashboard\\DashboardGauges\\TextItemHorizontal.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 93, 29);
			BindingExtension bindingExtension13;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension13 = new BindingExtension(), new Uri("Dashboard\\DashboardGauges\\TextItemHorizontal.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 94, 29);
			BindingExtension bindingExtension14;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension14 = new BindingExtension(), new Uri("Dashboard\\DashboardGauges\\TextItemHorizontal.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 95, 29);
			Span span2;
			VisualDiagnostics.RegisterSourceInfo(span2 = new Span(), new Uri("Dashboard\\DashboardGauges\\TextItemHorizontal.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 92, 26);
			FormattedString formattedString;
			VisualDiagnostics.RegisterSourceInfo(formattedString = new FormattedString(), new Uri("Dashboard\\DashboardGauges\\TextItemHorizontal.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 86, 22);
			Label label2;
			VisualDiagnostics.RegisterSourceInfo(label2 = new Label(), new Uri("Dashboard\\DashboardGauges\\TextItemHorizontal.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 81, 14);
			DynamicResourceExtension dynamicResourceExtension;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension = new DynamicResourceExtension(), new Uri("Dashboard\\DashboardGauges\\TextItemHorizontal.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 175, 17);
			SharedSettings sharedSettings;
			VisualDiagnostics.RegisterSourceInfo(sharedSettings = SharedSettings.Current, new Uri("Dashboard\\DashboardGauges\\TextItemHorizontal.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 177, 17);
			BindingExtension bindingExtension15;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension15 = new BindingExtension(), new Uri("Dashboard\\DashboardGauges\\TextItemHorizontal.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 177, 17);
			BindingExtension bindingExtension16;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension16 = new BindingExtension(), new Uri("Dashboard\\DashboardGauges\\TextItemHorizontal.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 182, 31);
			Span span3;
			VisualDiagnostics.RegisterSourceInfo(span3 = new Span(), new Uri("Dashboard\\DashboardGauges\\TextItemHorizontal.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 182, 26);
			Span span4;
			VisualDiagnostics.RegisterSourceInfo(span4 = new Span(), new Uri("Dashboard\\DashboardGauges\\TextItemHorizontal.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 183, 26);
			FormattedString formattedString2;
			VisualDiagnostics.RegisterSourceInfo(formattedString2 = new FormattedString(), new Uri("Dashboard\\DashboardGauges\\TextItemHorizontal.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 181, 22);
			Label label3;
			VisualDiagnostics.RegisterSourceInfo(label3 = new Label(), new Uri("Dashboard\\DashboardGauges\\TextItemHorizontal.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 171, 14);
			BindingExtension bindingExtension17;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension17 = new BindingExtension(), new Uri("Dashboard\\DashboardGauges\\TextItemHorizontal.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 230, 17);
			BindingExtension bindingExtension18;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension18 = new BindingExtension(), new Uri("Dashboard\\DashboardGauges\\TextItemHorizontal.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 232, 17);
			BindingExtension bindingExtension19;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension19 = new BindingExtension(), new Uri("Dashboard\\DashboardGauges\\TextItemHorizontal.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 233, 17);
			Span span5;
			VisualDiagnostics.RegisterSourceInfo(span5 = new Span(), new Uri("Dashboard\\DashboardGauges\\TextItemHorizontal.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 237, 26);
			BindingExtension bindingExtension20;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension20 = new BindingExtension(), new Uri("Dashboard\\DashboardGauges\\TextItemHorizontal.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 238, 31);
			Span span6;
			VisualDiagnostics.RegisterSourceInfo(span6 = new Span(), new Uri("Dashboard\\DashboardGauges\\TextItemHorizontal.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 238, 26);
			FormattedString formattedString3;
			VisualDiagnostics.RegisterSourceInfo(formattedString3 = new FormattedString(), new Uri("Dashboard\\DashboardGauges\\TextItemHorizontal.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 236, 22);
			Label label4;
			VisualDiagnostics.RegisterSourceInfo(label4 = new Label(), new Uri("Dashboard\\DashboardGauges\\TextItemHorizontal.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 227, 14);
			BindingExtension bindingExtension21;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension21 = new BindingExtension(), new Uri("Dashboard\\DashboardGauges\\TextItemHorizontal.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 248, 17);
			BindingExtension bindingExtension22;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension22 = new BindingExtension(), new Uri("Dashboard\\DashboardGauges\\TextItemHorizontal.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 250, 17);
			BindingExtension bindingExtension23;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension23 = new BindingExtension(), new Uri("Dashboard\\DashboardGauges\\TextItemHorizontal.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 251, 17);
			Span span7;
			VisualDiagnostics.RegisterSourceInfo(span7 = new Span(), new Uri("Dashboard\\DashboardGauges\\TextItemHorizontal.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 255, 26);
			BindingExtension bindingExtension24;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension24 = new BindingExtension(), new Uri("Dashboard\\DashboardGauges\\TextItemHorizontal.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 256, 31);
			Span span8;
			VisualDiagnostics.RegisterSourceInfo(span8 = new Span(), new Uri("Dashboard\\DashboardGauges\\TextItemHorizontal.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 256, 26);
			FormattedString formattedString4;
			VisualDiagnostics.RegisterSourceInfo(formattedString4 = new FormattedString(), new Uri("Dashboard\\DashboardGauges\\TextItemHorizontal.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 254, 22);
			Label label5;
			VisualDiagnostics.RegisterSourceInfo(label5 = new Label(), new Uri("Dashboard\\DashboardGauges\\TextItemHorizontal.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 245, 14);
			BindingExtension bindingExtension25;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension25 = new BindingExtension(), new Uri("Dashboard\\DashboardGauges\\TextItemHorizontal.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 264, 17);
			BindingExtension bindingExtension26;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension26 = new BindingExtension(), new Uri("Dashboard\\DashboardGauges\\TextItemHorizontal.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 266, 17);
			BindingExtension bindingExtension27;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension27 = new BindingExtension(), new Uri("Dashboard\\DashboardGauges\\TextItemHorizontal.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 267, 17);
			Span span9;
			VisualDiagnostics.RegisterSourceInfo(span9 = new Span(), new Uri("Dashboard\\DashboardGauges\\TextItemHorizontal.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 271, 26);
			BindingExtension bindingExtension28;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension28 = new BindingExtension(), new Uri("Dashboard\\DashboardGauges\\TextItemHorizontal.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 272, 31);
			Span span10;
			VisualDiagnostics.RegisterSourceInfo(span10 = new Span(), new Uri("Dashboard\\DashboardGauges\\TextItemHorizontal.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 272, 26);
			FormattedString formattedString5;
			VisualDiagnostics.RegisterSourceInfo(formattedString5 = new FormattedString(), new Uri("Dashboard\\DashboardGauges\\TextItemHorizontal.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 270, 22);
			Label label6;
			VisualDiagnostics.RegisterSourceInfo(label6 = new Label(), new Uri("Dashboard\\DashboardGauges\\TextItemHorizontal.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 261, 14);
			Grid grid;
			VisualDiagnostics.RegisterSourceInfo(grid = new Grid(), new Uri("Dashboard\\DashboardGauges\\TextItemHorizontal.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 50, 10);
			VisualDiagnostics.RegisterSourceInfo(this, new Uri("Dashboard\\DashboardGauges\\TextItemHorizontal.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 2, 2);
			NameScope nameScope = NameScope.GetNameScope(this) ?? new NameScope();
			NameScope.SetNameScope(this, nameScope);
			nameScope.RegisterName("bindingHackName", this);
			if (this.StyleId == null)
			{
				this.StyleId = "bindingHackName";
			}
			this.bindingHackName = this;
			this.Resources = resourceDictionary;
			resourceDictionary.Add("BoolToNegativeConverter", boolToNegativeConverter);
			resourceDictionary.Add("FloatValueToColorConverter", floatValueToColorConverter);
			resourceDictionary.Add("TransparentColorToFalseConverter", transparentColorToFalseConverter);
			resourceDictionary.Add("CornerRadiusToThicknessConverter", cornerRadiusToThicknessConverter);
			this.SetValue(View.MarginProperty, new Thickness(0.0));
			this.SetValue(Layout.PaddingProperty, new Thickness(0.0));
			this.SetValue(View.HorizontalOptionsProperty, LayoutOptions.Fill);
			this.SetValue(View.VerticalOptionsProperty, LayoutOptions.Fill);
			this.Resources = resourceDictionary;
			grid.SetValue(View.MarginProperty, new Thickness(0.0));
			grid.SetValue(Layout.PaddingProperty, new Thickness(0.0));
			grid.SetValue(View.HorizontalOptionsProperty, LayoutOptions.Fill);
			grid.SetValue(View.VerticalOptionsProperty, LayoutOptions.Fill);
			columnDefinition.SetValue(ColumnDefinition.WidthProperty, new GridLengthTypeConverter().ConvertFromInvariantString("*"));
			grid.GetValue(Grid.ColumnDefinitionsProperty).Add(columnDefinition);
			columnDefinition2.SetValue(ColumnDefinition.WidthProperty, new GridLengthTypeConverter().ConvertFromInvariantString("auto"));
			grid.GetValue(Grid.ColumnDefinitionsProperty).Add(columnDefinition2);
			gaugeBackground.SetValue(Grid.ColumnProperty, 0);
			gaugeBackground.SetValue(Grid.ColumnSpanProperty, 2);
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
			xmlNamespaceResolver.Add("grad", "clr-namespace:Xamarin.Forms.Essentials.Controls");
			xmlNamespaceResolver.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver.Add("settings", "clr-namespace:CarScannerXamarinForms.Settings");
			xamlServiceProvider.Add(typeFromHandle2, new XamlTypeResolver(xmlNamespaceResolver, typeof(TextItemHorizontal).GetTypeInfo().Assembly));
			xamlServiceProvider.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(67, 17)));
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
			xmlNamespaceResolver2.Add("grad", "clr-namespace:Xamarin.Forms.Essentials.Controls");
			xmlNamespaceResolver2.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver2.Add("settings", "clr-namespace:CarScannerXamarinForms.Settings");
			xamlServiceProvider2.Add(typeFromHandle4, new XamlTypeResolver(xmlNamespaceResolver2, typeof(TextItemHorizontal).GetTypeInfo().Assembly));
			xamlServiceProvider2.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(69, 17)));
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
			label.SetValue(Grid.ColumnProperty, 0);
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
			label.SetValue(Label.HorizontalTextAlignmentProperty, new TextAlignmentConverter().ConvertFromInvariantString("Start"));
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
			label.SetValue(Label.VerticalTextAlignmentProperty, new TextAlignmentConverter().ConvertFromInvariantString("Center"));
			grid.Children.Add(label);
			label2.SetValue(Grid.ColumnProperty, 1);
			label2.SetValue(Label.HorizontalTextAlignmentProperty, new TextAlignmentConverter().ConvertFromInvariantString("End"));
			label2.SetValue(Label.VerticalTextAlignmentProperty, new TextAlignmentConverter().ConvertFromInvariantString("Center"));
			bindingExtension8.Mode = 2;
			bindingExtension8.Path = "FontName";
			bindingExtension8.TypedBinding = new TypedBinding<DashboardItem, string>(delegate(DashboardItem A_0)
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
			BindingBase bindingBase8 = bindingExtension8.ProvideValue(null);
			span.SetBinding(Span.FontFamilyProperty, bindingBase8);
			bindingExtension9.Mode = 2;
			bindingExtension9.Path = "ValueFontSize";
			bindingExtension9.TypedBinding = new TypedBinding<DashboardItem, double>(delegate(DashboardItem A_0)
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
			BindingBase bindingBase9 = bindingExtension9.ProvideValue(null);
			span.SetBinding(Span.FontSizeProperty, bindingBase9);
			bindingExtension10.Mode = 2;
			bindingExtension10.Path = "Model.TextValue";
			bindingExtension10.TypedBinding = new TypedBinding<DashboardItem, string>(delegate(DashboardItem A_0)
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
			}, null, new Tuple<Func<DashboardItem, object>, string>[]
			{
				new Tuple<Func<DashboardItem, object>, string>((DashboardItem A_0) => A_0, "Model"),
				new Tuple<Func<DashboardItem, object>, string>((DashboardItem A_0) => A_0.Model, "TextValue")
			});
			BindingBase bindingBase10 = bindingExtension10.ProvideValue(null);
			span.SetBinding(Span.TextProperty, bindingBase10);
			bindingExtension11.Mode = 2;
			bindingExtension11.Path = "ValueTextColor";
			bindingExtension11.TypedBinding = new TypedBinding<DashboardItem, Color>(delegate(DashboardItem A_0)
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
			BindingBase bindingBase11 = bindingExtension11.ProvideValue(null);
			span.SetBinding(Span.TextColorProperty, bindingBase11);
			formattedString.Spans.Add(span);
			bindingExtension12.Mode = 2;
			bindingExtension12.Path = "UnitsFontSize";
			bindingExtension12.TypedBinding = new TypedBinding<DashboardItem, double>(delegate(DashboardItem A_0)
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
			BindingBase bindingBase12 = bindingExtension12.ProvideValue(null);
			span2.SetBinding(Span.FontSizeProperty, bindingBase12);
			bindingExtension13.Mode = 2;
			bindingExtension13.Path = "Model.Units";
			bindingExtension13.TypedBinding = new TypedBinding<DashboardItem, string>(delegate(DashboardItem A_0)
			{
				if (A_0 != null)
				{
					LiveDataPIDModel model2 = A_0.Model;
					if (model2 != null)
					{
						return new ValueTuple<string, bool>(model2.Units, true);
					}
				}
				return default(ValueTuple<string, bool>);
			}, null, new Tuple<Func<DashboardItem, object>, string>[]
			{
				new Tuple<Func<DashboardItem, object>, string>((DashboardItem A_0) => A_0, "Model"),
				new Tuple<Func<DashboardItem, object>, string>((DashboardItem A_0) => A_0.Model, "Units")
			});
			BindingBase bindingBase13 = bindingExtension13.ProvideValue(null);
			span2.SetBinding(Span.TextProperty, bindingBase13);
			bindingExtension14.Mode = 2;
			bindingExtension14.Path = "UnitsTextColor";
			bindingExtension14.TypedBinding = new TypedBinding<DashboardItem, Color>(delegate(DashboardItem A_0)
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
			BindingBase bindingBase14 = bindingExtension14.ProvideValue(null);
			span2.SetBinding(Span.TextColorProperty, bindingBase14);
			formattedString.Spans.Add(span2);
			label2.SetValue(Label.FormattedTextProperty, formattedString);
			grid.Children.Add(label2);
			label3.SetValue(Grid.ColumnProperty, 1);
			label3.SetValue(View.MarginProperty, new Thickness(0.0));
			label3.SetValue(Label.FontAttributesProperty, new FontAttributesConverter().ConvertFromInvariantString("Bold"));
			dynamicResourceExtension.Key = "BaseFontSize--";
			IMarkupExtension<DynamicResource> markupExtension3 = dynamicResourceExtension;
			XamlServiceProvider xamlServiceProvider3 = new XamlServiceProvider();
			Type typeFromHandle5 = typeof(IProvideValueTarget);
			object[] array3 = new object[0 + 3];
			array3[0] = label3;
			array3[1] = grid;
			array3[2] = this;
			object obj5;
			xamlServiceProvider3.Add(typeFromHandle5, obj5 = new SimpleValueTargetProvider(array3, Label.FontSizeProperty, nameScope));
			xamlServiceProvider3.Add(typeof(IReferenceProvider), obj5);
			Type typeFromHandle6 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver3 = new XmlNamespaceResolver();
			xmlNamespaceResolver3.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver3.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver3.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver3.Add("dash", "clr-namespace:CarScannerXamarinForms.Dashboard");
			xmlNamespaceResolver3.Add("grad", "clr-namespace:Xamarin.Forms.Essentials.Controls");
			xmlNamespaceResolver3.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver3.Add("settings", "clr-namespace:CarScannerXamarinForms.Settings");
			xamlServiceProvider3.Add(typeFromHandle6, new XamlTypeResolver(xmlNamespaceResolver3, typeof(TextItemHorizontal).GetTypeInfo().Assembly));
			xamlServiceProvider3.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(175, 17)));
			DynamicResource dynamicResource = markupExtension3.ProvideValue(xamlServiceProvider3);
			label3.SetDynamicResource(Label.FontSizeProperty, dynamicResource.Key);
			label3.SetValue(Label.HorizontalTextAlignmentProperty, new TextAlignmentConverter().ConvertFromInvariantString("End"));
			bindingExtension15.Mode = 2;
			bindingExtension15.Source = sharedSettings;
			bindingExtension15.Path = "ShowPing";
			BindingBase bindingBase15 = bindingExtension15.ProvideValue(null);
			label3.SetBinding(VisualElement.IsVisibleProperty, bindingBase15);
			label3.SetValue(Label.TextColorProperty, Color.Red);
			label3.SetValue(Label.VerticalTextAlignmentProperty, new TextAlignmentConverter().ConvertFromInvariantString("End"));
			bindingExtension16.Mode = 2;
			bindingExtension16.Path = "Model.Ping";
			bindingExtension16.TypedBinding = new TypedBinding<DashboardItem, long>(delegate(DashboardItem A_0)
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
			BindingBase bindingBase16 = bindingExtension16.ProvideValue(null);
			span3.SetBinding(Span.TextProperty, bindingBase16);
			formattedString2.Spans.Add(span3);
			span4.SetValue(Span.TextProperty, "ms");
			formattedString2.Spans.Add(span4);
			label3.SetValue(Label.FormattedTextProperty, formattedString2);
			grid.Children.Add(label3);
			label4.SetValue(Grid.ColumnProperty, 1);
			label4.SetValue(Label.FontAttributesProperty, new FontAttributesConverter().ConvertFromInvariantString("Bold"));
			bindingExtension17.Mode = 2;
			bindingExtension17.Path = "MinMaxAvgFontSize";
			bindingExtension17.TypedBinding = new TypedBinding<DashboardItem, double>(delegate(DashboardItem A_0)
			{
				if (A_0 != null)
				{
					return new ValueTuple<double, bool>(A_0.MinMaxAvgFontSize, true);
				}
				return default(ValueTuple<double, bool>);
			}, null, new Tuple<Func<DashboardItem, object>, string>[]
			{
				new Tuple<Func<DashboardItem, object>, string>((DashboardItem A_0) => A_0, "MinMaxAvgFontSize")
			});
			BindingBase bindingBase17 = bindingExtension17.ProvideValue(null);
			label4.SetBinding(Label.FontSizeProperty, bindingBase17);
			label4.SetValue(Label.HorizontalTextAlignmentProperty, new TextAlignmentConverter().ConvertFromInvariantString("End"));
			bindingExtension18.Mode = 2;
			bindingExtension18.Path = "ShowAvg";
			bindingExtension18.TypedBinding = new TypedBinding<DashboardItem, bool>(delegate(DashboardItem A_0)
			{
				if (A_0 != null)
				{
					return new ValueTuple<bool, bool>(A_0.ShowAvg, true);
				}
				return default(ValueTuple<bool, bool>);
			}, null, new Tuple<Func<DashboardItem, object>, string>[]
			{
				new Tuple<Func<DashboardItem, object>, string>((DashboardItem A_0) => A_0, "ShowAvg")
			});
			BindingBase bindingBase18 = bindingExtension18.ProvideValue(null);
			label4.SetBinding(VisualElement.IsVisibleProperty, bindingBase18);
			bindingExtension19.Mode = 2;
			bindingExtension19.Path = "MinMaxAvgColor";
			bindingExtension19.TypedBinding = new TypedBinding<DashboardItem, Color>(delegate(DashboardItem A_0)
			{
				if (A_0 != null)
				{
					return new ValueTuple<Color, bool>(A_0.MinMaxAvgColor, true);
				}
				return default(ValueTuple<Color, bool>);
			}, null, new Tuple<Func<DashboardItem, object>, string>[]
			{
				new Tuple<Func<DashboardItem, object>, string>((DashboardItem A_0) => A_0, "MinMaxAvgColor")
			});
			BindingBase bindingBase19 = bindingExtension19.ProvideValue(null);
			label4.SetBinding(Label.TextColorProperty, bindingBase19);
			label4.SetValue(Label.VerticalTextAlignmentProperty, new TextAlignmentConverter().ConvertFromInvariantString("Start"));
			span5.SetValue(Span.TextProperty, "Avg: ");
			formattedString3.Spans.Add(span5);
			bindingExtension20.Mode = 2;
			bindingExtension20.Path = "Model.AverageTextValue";
			bindingExtension20.TypedBinding = new TypedBinding<DashboardItem, string>(delegate(DashboardItem A_0)
			{
				if (A_0 != null)
				{
					LiveDataPIDModel model4 = A_0.Model;
					if (model4 != null)
					{
						return new ValueTuple<string, bool>(model4.AverageTextValue, true);
					}
				}
				return default(ValueTuple<string, bool>);
			}, null, new Tuple<Func<DashboardItem, object>, string>[]
			{
				new Tuple<Func<DashboardItem, object>, string>((DashboardItem A_0) => A_0, "Model"),
				new Tuple<Func<DashboardItem, object>, string>((DashboardItem A_0) => A_0.Model, "AverageTextValue")
			});
			BindingBase bindingBase20 = bindingExtension20.ProvideValue(null);
			span6.SetBinding(Span.TextProperty, bindingBase20);
			formattedString3.Spans.Add(span6);
			label4.SetValue(Label.FormattedTextProperty, formattedString3);
			grid.Children.Add(label4);
			label5.SetValue(Grid.ColumnProperty, 0);
			label5.SetValue(Label.FontAttributesProperty, new FontAttributesConverter().ConvertFromInvariantString("Bold"));
			bindingExtension21.Mode = 2;
			bindingExtension21.Path = "MinMaxAvgFontSize";
			bindingExtension21.TypedBinding = new TypedBinding<DashboardItem, double>(delegate(DashboardItem A_0)
			{
				if (A_0 != null)
				{
					return new ValueTuple<double, bool>(A_0.MinMaxAvgFontSize, true);
				}
				return default(ValueTuple<double, bool>);
			}, null, new Tuple<Func<DashboardItem, object>, string>[]
			{
				new Tuple<Func<DashboardItem, object>, string>((DashboardItem A_0) => A_0, "MinMaxAvgFontSize")
			});
			BindingBase bindingBase21 = bindingExtension21.ProvideValue(null);
			label5.SetBinding(Label.FontSizeProperty, bindingBase21);
			label5.SetValue(Label.HorizontalTextAlignmentProperty, new TextAlignmentConverter().ConvertFromInvariantString("Start"));
			bindingExtension22.Mode = 2;
			bindingExtension22.Path = "ShowMinMax";
			bindingExtension22.TypedBinding = new TypedBinding<DashboardItem, bool>(delegate(DashboardItem A_0)
			{
				if (A_0 != null)
				{
					return new ValueTuple<bool, bool>(A_0.ShowMinMax, true);
				}
				return default(ValueTuple<bool, bool>);
			}, null, new Tuple<Func<DashboardItem, object>, string>[]
			{
				new Tuple<Func<DashboardItem, object>, string>((DashboardItem A_0) => A_0, "ShowMinMax")
			});
			BindingBase bindingBase22 = bindingExtension22.ProvideValue(null);
			label5.SetBinding(VisualElement.IsVisibleProperty, bindingBase22);
			bindingExtension23.Mode = 2;
			bindingExtension23.Path = "MinMaxAvgColor";
			bindingExtension23.TypedBinding = new TypedBinding<DashboardItem, Color>(delegate(DashboardItem A_0)
			{
				if (A_0 != null)
				{
					return new ValueTuple<Color, bool>(A_0.MinMaxAvgColor, true);
				}
				return default(ValueTuple<Color, bool>);
			}, null, new Tuple<Func<DashboardItem, object>, string>[]
			{
				new Tuple<Func<DashboardItem, object>, string>((DashboardItem A_0) => A_0, "MinMaxAvgColor")
			});
			BindingBase bindingBase23 = bindingExtension23.ProvideValue(null);
			label5.SetBinding(Label.TextColorProperty, bindingBase23);
			label5.SetValue(Label.VerticalTextAlignmentProperty, new TextAlignmentConverter().ConvertFromInvariantString("Start"));
			span7.SetValue(Span.TextProperty, "Max: ");
			formattedString4.Spans.Add(span7);
			bindingExtension24.Mode = 2;
			bindingExtension24.Path = "Model.MaximumAchievedText";
			bindingExtension24.TypedBinding = new TypedBinding<DashboardItem, string>(delegate(DashboardItem A_0)
			{
				if (A_0 != null)
				{
					LiveDataPIDModel model5 = A_0.Model;
					if (model5 != null)
					{
						return new ValueTuple<string, bool>(model5.MaximumAchievedText, true);
					}
				}
				return default(ValueTuple<string, bool>);
			}, null, new Tuple<Func<DashboardItem, object>, string>[]
			{
				new Tuple<Func<DashboardItem, object>, string>((DashboardItem A_0) => A_0, "Model"),
				new Tuple<Func<DashboardItem, object>, string>((DashboardItem A_0) => A_0.Model, "MaximumAchievedText")
			});
			BindingBase bindingBase24 = bindingExtension24.ProvideValue(null);
			span8.SetBinding(Span.TextProperty, bindingBase24);
			formattedString4.Spans.Add(span8);
			label5.SetValue(Label.FormattedTextProperty, formattedString4);
			grid.Children.Add(label5);
			label6.SetValue(Grid.ColumnProperty, 0);
			label6.SetValue(Label.FontAttributesProperty, new FontAttributesConverter().ConvertFromInvariantString("Bold"));
			bindingExtension25.Mode = 2;
			bindingExtension25.Path = "MinMaxAvgFontSize";
			bindingExtension25.TypedBinding = new TypedBinding<DashboardItem, double>(delegate(DashboardItem A_0)
			{
				if (A_0 != null)
				{
					return new ValueTuple<double, bool>(A_0.MinMaxAvgFontSize, true);
				}
				return default(ValueTuple<double, bool>);
			}, null, new Tuple<Func<DashboardItem, object>, string>[]
			{
				new Tuple<Func<DashboardItem, object>, string>((DashboardItem A_0) => A_0, "MinMaxAvgFontSize")
			});
			BindingBase bindingBase25 = bindingExtension25.ProvideValue(null);
			label6.SetBinding(Label.FontSizeProperty, bindingBase25);
			label6.SetValue(Label.HorizontalTextAlignmentProperty, new TextAlignmentConverter().ConvertFromInvariantString("Start"));
			bindingExtension26.Mode = 2;
			bindingExtension26.Path = "ShowMinMax";
			bindingExtension26.TypedBinding = new TypedBinding<DashboardItem, bool>(delegate(DashboardItem A_0)
			{
				if (A_0 != null)
				{
					return new ValueTuple<bool, bool>(A_0.ShowMinMax, true);
				}
				return default(ValueTuple<bool, bool>);
			}, null, new Tuple<Func<DashboardItem, object>, string>[]
			{
				new Tuple<Func<DashboardItem, object>, string>((DashboardItem A_0) => A_0, "ShowMinMax")
			});
			BindingBase bindingBase26 = bindingExtension26.ProvideValue(null);
			label6.SetBinding(VisualElement.IsVisibleProperty, bindingBase26);
			bindingExtension27.Mode = 2;
			bindingExtension27.Path = "MinMaxAvgColor";
			bindingExtension27.TypedBinding = new TypedBinding<DashboardItem, Color>(delegate(DashboardItem A_0)
			{
				if (A_0 != null)
				{
					return new ValueTuple<Color, bool>(A_0.MinMaxAvgColor, true);
				}
				return default(ValueTuple<Color, bool>);
			}, null, new Tuple<Func<DashboardItem, object>, string>[]
			{
				new Tuple<Func<DashboardItem, object>, string>((DashboardItem A_0) => A_0, "MinMaxAvgColor")
			});
			BindingBase bindingBase27 = bindingExtension27.ProvideValue(null);
			label6.SetBinding(Label.TextColorProperty, bindingBase27);
			label6.SetValue(Label.VerticalTextAlignmentProperty, new TextAlignmentConverter().ConvertFromInvariantString("End"));
			span9.SetValue(Span.TextProperty, "Min: ");
			formattedString5.Spans.Add(span9);
			bindingExtension28.Mode = 2;
			bindingExtension28.Path = "Model.MinimumAchievedText";
			bindingExtension28.TypedBinding = new TypedBinding<DashboardItem, string>(delegate(DashboardItem A_0)
			{
				if (A_0 != null)
				{
					LiveDataPIDModel model6 = A_0.Model;
					if (model6 != null)
					{
						return new ValueTuple<string, bool>(model6.MinimumAchievedText, true);
					}
				}
				return default(ValueTuple<string, bool>);
			}, null, new Tuple<Func<DashboardItem, object>, string>[]
			{
				new Tuple<Func<DashboardItem, object>, string>((DashboardItem A_0) => A_0, "Model"),
				new Tuple<Func<DashboardItem, object>, string>((DashboardItem A_0) => A_0.Model, "MinimumAchievedText")
			});
			BindingBase bindingBase28 = bindingExtension28.ProvideValue(null);
			span10.SetBinding(Span.TextProperty, bindingBase28);
			formattedString5.Spans.Add(span10);
			label6.SetValue(Label.FormattedTextProperty, formattedString5);
			grid.Children.Add(label6);
			this.SetValue(ContentView.ContentProperty, grid);
		}

		// Token: 0x060045FD RID: 17917 RVA: 0x00368BD6 File Offset: 0x00366DD6
		private void __InitComponentRuntime()
		{
			Extensions.LoadFromXaml<TextItemHorizontal>(this, typeof(TextItemHorizontal));
			this.bindingHackName = NameScopeExtensions.FindByName<ContentView>(this, "bindingHackName");
		}

		// Token: 0x060045FE RID: 17918 RVA: 0x00368BFC File Offset: 0x00366DFC
		[CompilerGenerated]
		private static ValueTuple<bool, bool> <InitializeComponent>typedBindingsM__981(DashboardItem A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<bool, bool>(A_0.ShowDefaultBackground, true);
			}
			return default(ValueTuple<bool, bool>);
		}

		// Token: 0x060045FF RID: 17919 RVA: 0x00368C2C File Offset: 0x00366E2C
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__982(DashboardItem A_0)
		{
			return A_0;
		}

		// Token: 0x06004600 RID: 17920 RVA: 0x00368C3C File Offset: 0x00366E3C
		[CompilerGenerated]
		private static ValueTuple<Thickness, bool> <InitializeComponent>typedBindingsM__983(DashboardItem A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<Thickness, bool>(A_0.CornerRadius, true);
			}
			return default(ValueTuple<Thickness, bool>);
		}

		// Token: 0x06004601 RID: 17921 RVA: 0x00368C6C File Offset: 0x00366E6C
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__984(DashboardItem A_0)
		{
			return A_0;
		}

		// Token: 0x06004602 RID: 17922 RVA: 0x00368C7C File Offset: 0x00366E7C
		[CompilerGenerated]
		private static ValueTuple<Color, bool> <InitializeComponent>typedBindingsM__985(DashboardItem A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<Color, bool>(A_0.IndicatorBackgroundColor, true);
			}
			return default(ValueTuple<Color, bool>);
		}

		// Token: 0x06004603 RID: 17923 RVA: 0x00368CAC File Offset: 0x00366EAC
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__986(DashboardItem A_0)
		{
			return A_0;
		}

		// Token: 0x06004604 RID: 17924 RVA: 0x00368CBC File Offset: 0x00366EBC
		[CompilerGenerated]
		private static ValueTuple<Color, bool> <InitializeComponent>typedBindingsM__987(DashboardItem A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<Color, bool>(A_0.IndicatorBackgroundColor, true);
			}
			return default(ValueTuple<Color, bool>);
		}

		// Token: 0x06004605 RID: 17925 RVA: 0x00368CEC File Offset: 0x00366EEC
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__988(DashboardItem A_0)
		{
			return A_0;
		}

		// Token: 0x06004606 RID: 17926 RVA: 0x00368CFC File Offset: 0x00366EFC
		[CompilerGenerated]
		private static ValueTuple<double, bool> <InitializeComponent>typedBindingsM__989(DashboardItem A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<double, bool>(A_0.TitleFontSize, true);
			}
			return default(ValueTuple<double, bool>);
		}

		// Token: 0x06004607 RID: 17927 RVA: 0x00368D2C File Offset: 0x00366F2C
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__990(DashboardItem A_0)
		{
			return A_0;
		}

		// Token: 0x06004608 RID: 17928 RVA: 0x00368D3C File Offset: 0x00366F3C
		[CompilerGenerated]
		private static ValueTuple<string, bool> <InitializeComponent>typedBindingsM__991(DashboardItem A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<string, bool>(A_0.PIDName, true);
			}
			return default(ValueTuple<string, bool>);
		}

		// Token: 0x06004609 RID: 17929 RVA: 0x00368D6C File Offset: 0x00366F6C
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__992(DashboardItem A_0)
		{
			return A_0;
		}

		// Token: 0x0600460A RID: 17930 RVA: 0x00368D7C File Offset: 0x00366F7C
		[CompilerGenerated]
		private static ValueTuple<Color, bool> <InitializeComponent>typedBindingsM__993(DashboardItem A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<Color, bool>(A_0.TitleTextColor, true);
			}
			return default(ValueTuple<Color, bool>);
		}

		// Token: 0x0600460B RID: 17931 RVA: 0x00368DAC File Offset: 0x00366FAC
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__994(DashboardItem A_0)
		{
			return A_0;
		}

		// Token: 0x0600460C RID: 17932 RVA: 0x00368DBC File Offset: 0x00366FBC
		[CompilerGenerated]
		private static ValueTuple<string, bool> <InitializeComponent>typedBindingsM__995(DashboardItem A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<string, bool>(A_0.FontName, true);
			}
			return default(ValueTuple<string, bool>);
		}

		// Token: 0x0600460D RID: 17933 RVA: 0x00368DEC File Offset: 0x00366FEC
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__996(DashboardItem A_0)
		{
			return A_0;
		}

		// Token: 0x0600460E RID: 17934 RVA: 0x00368DFC File Offset: 0x00366FFC
		[CompilerGenerated]
		private static ValueTuple<double, bool> <InitializeComponent>typedBindingsM__997(DashboardItem A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<double, bool>(A_0.ValueFontSize, true);
			}
			return default(ValueTuple<double, bool>);
		}

		// Token: 0x0600460F RID: 17935 RVA: 0x00368E2C File Offset: 0x0036702C
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__998(DashboardItem A_0)
		{
			return A_0;
		}

		// Token: 0x06004610 RID: 17936 RVA: 0x00368E3C File Offset: 0x0036703C
		[CompilerGenerated]
		private static ValueTuple<string, bool> <InitializeComponent>typedBindingsM__999(DashboardItem A_0)
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

		// Token: 0x06004611 RID: 17937 RVA: 0x00368E74 File Offset: 0x00367074
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__1000(DashboardItem A_0)
		{
			return A_0;
		}

		// Token: 0x06004612 RID: 17938 RVA: 0x00368E84 File Offset: 0x00367084
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__1001(DashboardItem A_0)
		{
			return A_0.Model;
		}

		// Token: 0x06004613 RID: 17939 RVA: 0x00368E98 File Offset: 0x00367098
		[CompilerGenerated]
		private static ValueTuple<Color, bool> <InitializeComponent>typedBindingsM__1002(DashboardItem A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<Color, bool>(A_0.ValueTextColor, true);
			}
			return default(ValueTuple<Color, bool>);
		}

		// Token: 0x06004614 RID: 17940 RVA: 0x00368EC8 File Offset: 0x003670C8
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__1003(DashboardItem A_0)
		{
			return A_0;
		}

		// Token: 0x06004615 RID: 17941 RVA: 0x00368ED8 File Offset: 0x003670D8
		[CompilerGenerated]
		private static ValueTuple<double, bool> <InitializeComponent>typedBindingsM__1004(DashboardItem A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<double, bool>(A_0.UnitsFontSize, true);
			}
			return default(ValueTuple<double, bool>);
		}

		// Token: 0x06004616 RID: 17942 RVA: 0x00368F08 File Offset: 0x00367108
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__1005(DashboardItem A_0)
		{
			return A_0;
		}

		// Token: 0x06004617 RID: 17943 RVA: 0x00368F18 File Offset: 0x00367118
		[CompilerGenerated]
		private static ValueTuple<string, bool> <InitializeComponent>typedBindingsM__1006(DashboardItem A_0)
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

		// Token: 0x06004618 RID: 17944 RVA: 0x00368F50 File Offset: 0x00367150
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__1007(DashboardItem A_0)
		{
			return A_0;
		}

		// Token: 0x06004619 RID: 17945 RVA: 0x00368F60 File Offset: 0x00367160
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__1008(DashboardItem A_0)
		{
			return A_0.Model;
		}

		// Token: 0x0600461A RID: 17946 RVA: 0x00368F74 File Offset: 0x00367174
		[CompilerGenerated]
		private static ValueTuple<Color, bool> <InitializeComponent>typedBindingsM__1009(DashboardItem A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<Color, bool>(A_0.UnitsTextColor, true);
			}
			return default(ValueTuple<Color, bool>);
		}

		// Token: 0x0600461B RID: 17947 RVA: 0x00368FA4 File Offset: 0x003671A4
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__1010(DashboardItem A_0)
		{
			return A_0;
		}

		// Token: 0x0600461C RID: 17948 RVA: 0x00368FB4 File Offset: 0x003671B4
		[CompilerGenerated]
		private static ValueTuple<long, bool> <InitializeComponent>typedBindingsM__1011(DashboardItem A_0)
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

		// Token: 0x0600461D RID: 17949 RVA: 0x00368FEC File Offset: 0x003671EC
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__1012(DashboardItem A_0)
		{
			return A_0;
		}

		// Token: 0x0600461E RID: 17950 RVA: 0x00368FFC File Offset: 0x003671FC
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__1013(DashboardItem A_0)
		{
			return A_0.Model;
		}

		// Token: 0x0600461F RID: 17951 RVA: 0x00369010 File Offset: 0x00367210
		[CompilerGenerated]
		private static ValueTuple<double, bool> <InitializeComponent>typedBindingsM__1014(DashboardItem A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<double, bool>(A_0.MinMaxAvgFontSize, true);
			}
			return default(ValueTuple<double, bool>);
		}

		// Token: 0x06004620 RID: 17952 RVA: 0x00369040 File Offset: 0x00367240
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__1015(DashboardItem A_0)
		{
			return A_0;
		}

		// Token: 0x06004621 RID: 17953 RVA: 0x00369050 File Offset: 0x00367250
		[CompilerGenerated]
		private static ValueTuple<bool, bool> <InitializeComponent>typedBindingsM__1016(DashboardItem A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<bool, bool>(A_0.ShowAvg, true);
			}
			return default(ValueTuple<bool, bool>);
		}

		// Token: 0x06004622 RID: 17954 RVA: 0x00369080 File Offset: 0x00367280
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__1017(DashboardItem A_0)
		{
			return A_0;
		}

		// Token: 0x06004623 RID: 17955 RVA: 0x00369090 File Offset: 0x00367290
		[CompilerGenerated]
		private static ValueTuple<Color, bool> <InitializeComponent>typedBindingsM__1018(DashboardItem A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<Color, bool>(A_0.MinMaxAvgColor, true);
			}
			return default(ValueTuple<Color, bool>);
		}

		// Token: 0x06004624 RID: 17956 RVA: 0x003690C0 File Offset: 0x003672C0
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__1019(DashboardItem A_0)
		{
			return A_0;
		}

		// Token: 0x06004625 RID: 17957 RVA: 0x003690D0 File Offset: 0x003672D0
		[CompilerGenerated]
		private static ValueTuple<string, bool> <InitializeComponent>typedBindingsM__1020(DashboardItem A_0)
		{
			if (A_0 != null)
			{
				LiveDataPIDModel model = A_0.Model;
				if (model != null)
				{
					return new ValueTuple<string, bool>(model.AverageTextValue, true);
				}
			}
			return default(ValueTuple<string, bool>);
		}

		// Token: 0x06004626 RID: 17958 RVA: 0x00369108 File Offset: 0x00367308
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__1021(DashboardItem A_0)
		{
			return A_0;
		}

		// Token: 0x06004627 RID: 17959 RVA: 0x00369118 File Offset: 0x00367318
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__1022(DashboardItem A_0)
		{
			return A_0.Model;
		}

		// Token: 0x06004628 RID: 17960 RVA: 0x0036912C File Offset: 0x0036732C
		[CompilerGenerated]
		private static ValueTuple<double, bool> <InitializeComponent>typedBindingsM__1023(DashboardItem A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<double, bool>(A_0.MinMaxAvgFontSize, true);
			}
			return default(ValueTuple<double, bool>);
		}

		// Token: 0x06004629 RID: 17961 RVA: 0x0036915C File Offset: 0x0036735C
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__1024(DashboardItem A_0)
		{
			return A_0;
		}

		// Token: 0x0600462A RID: 17962 RVA: 0x0036916C File Offset: 0x0036736C
		[CompilerGenerated]
		private static ValueTuple<bool, bool> <InitializeComponent>typedBindingsM__1025(DashboardItem A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<bool, bool>(A_0.ShowMinMax, true);
			}
			return default(ValueTuple<bool, bool>);
		}

		// Token: 0x0600462B RID: 17963 RVA: 0x0036919C File Offset: 0x0036739C
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__1026(DashboardItem A_0)
		{
			return A_0;
		}

		// Token: 0x0600462C RID: 17964 RVA: 0x003691AC File Offset: 0x003673AC
		[CompilerGenerated]
		private static ValueTuple<Color, bool> <InitializeComponent>typedBindingsM__1027(DashboardItem A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<Color, bool>(A_0.MinMaxAvgColor, true);
			}
			return default(ValueTuple<Color, bool>);
		}

		// Token: 0x0600462D RID: 17965 RVA: 0x003691DC File Offset: 0x003673DC
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__1028(DashboardItem A_0)
		{
			return A_0;
		}

		// Token: 0x0600462E RID: 17966 RVA: 0x003691EC File Offset: 0x003673EC
		[CompilerGenerated]
		private static ValueTuple<string, bool> <InitializeComponent>typedBindingsM__1029(DashboardItem A_0)
		{
			if (A_0 != null)
			{
				LiveDataPIDModel model = A_0.Model;
				if (model != null)
				{
					return new ValueTuple<string, bool>(model.MaximumAchievedText, true);
				}
			}
			return default(ValueTuple<string, bool>);
		}

		// Token: 0x0600462F RID: 17967 RVA: 0x00369224 File Offset: 0x00367424
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__1030(DashboardItem A_0)
		{
			return A_0;
		}

		// Token: 0x06004630 RID: 17968 RVA: 0x00369234 File Offset: 0x00367434
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__1031(DashboardItem A_0)
		{
			return A_0.Model;
		}

		// Token: 0x06004631 RID: 17969 RVA: 0x00369248 File Offset: 0x00367448
		[CompilerGenerated]
		private static ValueTuple<double, bool> <InitializeComponent>typedBindingsM__1032(DashboardItem A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<double, bool>(A_0.MinMaxAvgFontSize, true);
			}
			return default(ValueTuple<double, bool>);
		}

		// Token: 0x06004632 RID: 17970 RVA: 0x00369278 File Offset: 0x00367478
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__1033(DashboardItem A_0)
		{
			return A_0;
		}

		// Token: 0x06004633 RID: 17971 RVA: 0x00369288 File Offset: 0x00367488
		[CompilerGenerated]
		private static ValueTuple<bool, bool> <InitializeComponent>typedBindingsM__1034(DashboardItem A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<bool, bool>(A_0.ShowMinMax, true);
			}
			return default(ValueTuple<bool, bool>);
		}

		// Token: 0x06004634 RID: 17972 RVA: 0x003692B8 File Offset: 0x003674B8
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__1035(DashboardItem A_0)
		{
			return A_0;
		}

		// Token: 0x06004635 RID: 17973 RVA: 0x003692C8 File Offset: 0x003674C8
		[CompilerGenerated]
		private static ValueTuple<Color, bool> <InitializeComponent>typedBindingsM__1036(DashboardItem A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<Color, bool>(A_0.MinMaxAvgColor, true);
			}
			return default(ValueTuple<Color, bool>);
		}

		// Token: 0x06004636 RID: 17974 RVA: 0x003692F8 File Offset: 0x003674F8
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__1037(DashboardItem A_0)
		{
			return A_0;
		}

		// Token: 0x06004637 RID: 17975 RVA: 0x00369308 File Offset: 0x00367508
		[CompilerGenerated]
		private static ValueTuple<string, bool> <InitializeComponent>typedBindingsM__1038(DashboardItem A_0)
		{
			if (A_0 != null)
			{
				LiveDataPIDModel model = A_0.Model;
				if (model != null)
				{
					return new ValueTuple<string, bool>(model.MinimumAchievedText, true);
				}
			}
			return default(ValueTuple<string, bool>);
		}

		// Token: 0x06004638 RID: 17976 RVA: 0x00369340 File Offset: 0x00367540
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__1039(DashboardItem A_0)
		{
			return A_0;
		}

		// Token: 0x06004639 RID: 17977 RVA: 0x00369350 File Offset: 0x00367550
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__1040(DashboardItem A_0)
		{
			return A_0.Model;
		}

		// Token: 0x040028DB RID: 10459
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private ContentView bindingHackName;
	}
}
