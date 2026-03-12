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
	// Token: 0x020007B7 RID: 1975
	[XamlFilePath("Dashboard\\DashboardGauges\\SimpleTextItem.xaml")]
	public class SimpleTextItem : ContentView
	{
		// Token: 0x060045B8 RID: 17848 RVA: 0x00363F2B File Offset: 0x0036212B
		public SimpleTextItem()
		{
			this.InitializeComponent();
		}

		// Token: 0x060045B9 RID: 17849 RVA: 0x00363F3C File Offset: 0x0036213C
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private void InitializeComponent()
		{
			if (ResourceLoader.IsEnabled && ResourceLoader.CanProvideContentFor(new ResourceLoader.ResourceLoadingQuery
			{
				AssemblyName = typeof(SimpleTextItem).GetTypeInfo().Assembly.GetName(),
				ResourcePath = "Dashboard/DashboardGauges/SimpleTextItem.xaml",
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
			VisualDiagnostics.RegisterSourceInfo(boolToNegativeConverter = new BoolToNegativeConverter(), new Uri("Dashboard\\DashboardGauges\\SimpleTextItem.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 20, 14);
			TransparentColorToFalseConverter transparentColorToFalseConverter;
			VisualDiagnostics.RegisterSourceInfo(transparentColorToFalseConverter = new TransparentColorToFalseConverter(), new Uri("Dashboard\\DashboardGauges\\SimpleTextItem.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 21, 14);
			CornerRadiusToThicknessConverter cornerRadiusToThicknessConverter;
			VisualDiagnostics.RegisterSourceInfo(cornerRadiusToThicknessConverter = new CornerRadiusToThicknessConverter(), new Uri("Dashboard\\DashboardGauges\\SimpleTextItem.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 22, 14);
			PIDToValueLineBreakConverter pidtoValueLineBreakConverter;
			VisualDiagnostics.RegisterSourceInfo(pidtoValueLineBreakConverter = new PIDToValueLineBreakConverter(), new Uri("Dashboard\\DashboardGauges\\SimpleTextItem.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 23, 14);
			ResourceDictionary resourceDictionary;
			VisualDiagnostics.RegisterSourceInfo(resourceDictionary = new ResourceDictionary(), new Uri("Dashboard\\DashboardGauges\\SimpleTextItem.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 19, 10);
			RowDefinition rowDefinition;
			VisualDiagnostics.RegisterSourceInfo(rowDefinition = new RowDefinition(), new Uri("Dashboard\\DashboardGauges\\SimpleTextItem.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 39, 18);
			BindingExtension bindingExtension;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension = new BindingExtension(), new Uri("Dashboard\\DashboardGauges\\SimpleTextItem.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 46, 17);
			GaugeBackground gaugeBackground;
			VisualDiagnostics.RegisterSourceInfo(gaugeBackground = new GaugeBackground(), new Uri("Dashboard\\DashboardGauges\\SimpleTextItem.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 43, 14);
			StaticResourceExtension staticResourceExtension;
			VisualDiagnostics.RegisterSourceInfo(staticResourceExtension = new StaticResourceExtension(), new Uri("Dashboard\\DashboardGauges\\SimpleTextItem.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 51, 17);
			BindingExtension bindingExtension2;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension2 = new BindingExtension(), new Uri("Dashboard\\DashboardGauges\\SimpleTextItem.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 51, 17);
			StaticResourceExtension staticResourceExtension2;
			VisualDiagnostics.RegisterSourceInfo(staticResourceExtension2 = new StaticResourceExtension(), new Uri("Dashboard\\DashboardGauges\\SimpleTextItem.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 53, 17);
			BindingExtension bindingExtension3;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension3 = new BindingExtension(), new Uri("Dashboard\\DashboardGauges\\SimpleTextItem.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 53, 17);
			BindingExtension bindingExtension4;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension4 = new BindingExtension(), new Uri("Dashboard\\DashboardGauges\\SimpleTextItem.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 55, 17);
			BoxView boxView;
			VisualDiagnostics.RegisterSourceInfo(boxView = new BoxView(), new Uri("Dashboard\\DashboardGauges\\SimpleTextItem.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 49, 14);
			BindingExtension bindingExtension5;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension5 = new BindingExtension(), new Uri("Dashboard\\DashboardGauges\\SimpleTextItem.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 60, 17);
			BindingExtension bindingExtension6;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension6 = new BindingExtension(), new Uri("Dashboard\\DashboardGauges\\SimpleTextItem.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 63, 17);
			BindingExtension bindingExtension7;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension7 = new BindingExtension(), new Uri("Dashboard\\DashboardGauges\\SimpleTextItem.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 64, 17);
			Label label;
			VisualDiagnostics.RegisterSourceInfo(label = new Label(), new Uri("Dashboard\\DashboardGauges\\SimpleTextItem.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 57, 14);
			BindingExtension bindingExtension8;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension8 = new BindingExtension(), new Uri("Dashboard\\DashboardGauges\\SimpleTextItem.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 72, 17);
			BindingExtension bindingExtension9;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension9 = new BindingExtension(), new Uri("Dashboard\\DashboardGauges\\SimpleTextItem.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 73, 17);
			StaticResourceExtension staticResourceExtension3;
			VisualDiagnostics.RegisterSourceInfo(staticResourceExtension3 = new StaticResourceExtension(), new Uri("Dashboard\\DashboardGauges\\SimpleTextItem.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 76, 17);
			BindingExtension bindingExtension10;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension10 = new BindingExtension(), new Uri("Dashboard\\DashboardGauges\\SimpleTextItem.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 76, 17);
			BindingExtension bindingExtension11;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension11 = new BindingExtension(), new Uri("Dashboard\\DashboardGauges\\SimpleTextItem.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 77, 17);
			BindingExtension bindingExtension12;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension12 = new BindingExtension(), new Uri("Dashboard\\DashboardGauges\\SimpleTextItem.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 78, 17);
			Label label2;
			VisualDiagnostics.RegisterSourceInfo(label2 = new Label(), new Uri("Dashboard\\DashboardGauges\\SimpleTextItem.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 68, 14);
			BindingExtension bindingExtension13;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension13 = new BindingExtension(), new Uri("Dashboard\\DashboardGauges\\SimpleTextItem.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 85, 17);
			BindingExtension bindingExtension14;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension14 = new BindingExtension(), new Uri("Dashboard\\DashboardGauges\\SimpleTextItem.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 89, 17);
			BindingExtension bindingExtension15;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension15 = new BindingExtension(), new Uri("Dashboard\\DashboardGauges\\SimpleTextItem.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 90, 17);
			Label label3;
			VisualDiagnostics.RegisterSourceInfo(label3 = new Label(), new Uri("Dashboard\\DashboardGauges\\SimpleTextItem.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 82, 14);
			BindingExtension bindingExtension16;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension16 = new BindingExtension(), new Uri("Dashboard\\DashboardGauges\\SimpleTextItem.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 104, 21);
			BindingExtension bindingExtension17;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension17 = new BindingExtension(), new Uri("Dashboard\\DashboardGauges\\SimpleTextItem.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 106, 21);
			BindingExtension bindingExtension18;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension18 = new BindingExtension(), new Uri("Dashboard\\DashboardGauges\\SimpleTextItem.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 107, 21);
			Span span;
			VisualDiagnostics.RegisterSourceInfo(span = new Span(), new Uri("Dashboard\\DashboardGauges\\SimpleTextItem.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 110, 30);
			BindingExtension bindingExtension19;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension19 = new BindingExtension(), new Uri("Dashboard\\DashboardGauges\\SimpleTextItem.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 111, 35);
			Span span2;
			VisualDiagnostics.RegisterSourceInfo(span2 = new Span(), new Uri("Dashboard\\DashboardGauges\\SimpleTextItem.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 111, 30);
			FormattedString formattedString;
			VisualDiagnostics.RegisterSourceInfo(formattedString = new FormattedString(), new Uri("Dashboard\\DashboardGauges\\SimpleTextItem.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 109, 26);
			Label label4;
			VisualDiagnostics.RegisterSourceInfo(label4 = new Label(), new Uri("Dashboard\\DashboardGauges\\SimpleTextItem.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 102, 18);
			DynamicResourceExtension dynamicResourceExtension;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension = new DynamicResourceExtension(), new Uri("Dashboard\\DashboardGauges\\SimpleTextItem.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 122, 21);
			SharedSettings sharedSettings;
			VisualDiagnostics.RegisterSourceInfo(sharedSettings = SharedSettings.Current, new Uri("Dashboard\\DashboardGauges\\SimpleTextItem.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 124, 21);
			BindingExtension bindingExtension20;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension20 = new BindingExtension(), new Uri("Dashboard\\DashboardGauges\\SimpleTextItem.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 124, 21);
			BindingExtension bindingExtension21;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension21 = new BindingExtension(), new Uri("Dashboard\\DashboardGauges\\SimpleTextItem.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 128, 35);
			Span span3;
			VisualDiagnostics.RegisterSourceInfo(span3 = new Span(), new Uri("Dashboard\\DashboardGauges\\SimpleTextItem.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 128, 30);
			Span span4;
			VisualDiagnostics.RegisterSourceInfo(span4 = new Span(), new Uri("Dashboard\\DashboardGauges\\SimpleTextItem.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 129, 30);
			FormattedString formattedString2;
			VisualDiagnostics.RegisterSourceInfo(formattedString2 = new FormattedString(), new Uri("Dashboard\\DashboardGauges\\SimpleTextItem.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 127, 26);
			Label label5;
			VisualDiagnostics.RegisterSourceInfo(label5 = new Label(), new Uri("Dashboard\\DashboardGauges\\SimpleTextItem.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 119, 18);
			StackLayout stackLayout;
			VisualDiagnostics.RegisterSourceInfo(stackLayout = new StackLayout(), new Uri("Dashboard\\DashboardGauges\\SimpleTextItem.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 94, 14);
			BindingExtension bindingExtension22;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension22 = new BindingExtension(), new Uri("Dashboard\\DashboardGauges\\SimpleTextItem.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 148, 21);
			BindingExtension bindingExtension23;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension23 = new BindingExtension(), new Uri("Dashboard\\DashboardGauges\\SimpleTextItem.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 149, 21);
			BindingExtension bindingExtension24;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension24 = new BindingExtension(), new Uri("Dashboard\\DashboardGauges\\SimpleTextItem.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 150, 21);
			Span span5;
			VisualDiagnostics.RegisterSourceInfo(span5 = new Span(), new Uri("Dashboard\\DashboardGauges\\SimpleTextItem.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 153, 30);
			BindingExtension bindingExtension25;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension25 = new BindingExtension(), new Uri("Dashboard\\DashboardGauges\\SimpleTextItem.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 154, 35);
			Span span6;
			VisualDiagnostics.RegisterSourceInfo(span6 = new Span(), new Uri("Dashboard\\DashboardGauges\\SimpleTextItem.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 154, 30);
			FormattedString formattedString3;
			VisualDiagnostics.RegisterSourceInfo(formattedString3 = new FormattedString(), new Uri("Dashboard\\DashboardGauges\\SimpleTextItem.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 152, 26);
			Label label6;
			VisualDiagnostics.RegisterSourceInfo(label6 = new Label(), new Uri("Dashboard\\DashboardGauges\\SimpleTextItem.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 146, 18);
			BindingExtension bindingExtension26;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension26 = new BindingExtension(), new Uri("Dashboard\\DashboardGauges\\SimpleTextItem.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 161, 21);
			BindingExtension bindingExtension27;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension27 = new BindingExtension(), new Uri("Dashboard\\DashboardGauges\\SimpleTextItem.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 162, 21);
			BindingExtension bindingExtension28;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension28 = new BindingExtension(), new Uri("Dashboard\\DashboardGauges\\SimpleTextItem.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 163, 21);
			Span span7;
			VisualDiagnostics.RegisterSourceInfo(span7 = new Span(), new Uri("Dashboard\\DashboardGauges\\SimpleTextItem.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 166, 30);
			BindingExtension bindingExtension29;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension29 = new BindingExtension(), new Uri("Dashboard\\DashboardGauges\\SimpleTextItem.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 167, 35);
			Span span8;
			VisualDiagnostics.RegisterSourceInfo(span8 = new Span(), new Uri("Dashboard\\DashboardGauges\\SimpleTextItem.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 167, 30);
			FormattedString formattedString4;
			VisualDiagnostics.RegisterSourceInfo(formattedString4 = new FormattedString(), new Uri("Dashboard\\DashboardGauges\\SimpleTextItem.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 165, 26);
			Label label7;
			VisualDiagnostics.RegisterSourceInfo(label7 = new Label(), new Uri("Dashboard\\DashboardGauges\\SimpleTextItem.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 159, 18);
			StackLayout stackLayout2;
			VisualDiagnostics.RegisterSourceInfo(stackLayout2 = new StackLayout(), new Uri("Dashboard\\DashboardGauges\\SimpleTextItem.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 139, 14);
			Grid grid;
			VisualDiagnostics.RegisterSourceInfo(grid = new Grid(), new Uri("Dashboard\\DashboardGauges\\SimpleTextItem.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 29, 10);
			VisualDiagnostics.RegisterSourceInfo(this, new Uri("Dashboard\\DashboardGauges\\SimpleTextItem.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 2, 2);
			NameScope nameScope = NameScope.GetNameScope(this) ?? new NameScope();
			NameScope.SetNameScope(this, nameScope);
			nameScope.RegisterName("bindingHackName", this);
			if (this.StyleId == null)
			{
				this.StyleId = "bindingHackName";
			}
			nameScope.RegisterName("labelValue", label2);
			if (label2.StyleId == null)
			{
				label2.StyleId = "labelValue";
			}
			this.bindingHackName = this;
			this.labelValue = label2;
			this.Resources = resourceDictionary;
			resourceDictionary.Add("BoolToNegativeConverter", boolToNegativeConverter);
			resourceDictionary.Add("TransparentColorToFalseConverter", transparentColorToFalseConverter);
			resourceDictionary.Add("CornerRadiusToThicknessConverter", cornerRadiusToThicknessConverter);
			resourceDictionary.Add("PIDToValueLineBreakConverter", pidtoValueLineBreakConverter);
			this.SetValue(View.MarginProperty, new Thickness(0.0));
			this.SetValue(Layout.PaddingProperty, new Thickness(0.0));
			this.SetValue(View.HorizontalOptionsProperty, LayoutOptions.Fill);
			this.SetValue(View.VerticalOptionsProperty, LayoutOptions.Fill);
			this.Resources = resourceDictionary;
			grid.SetValue(View.MarginProperty, new Thickness(0.0));
			grid.SetValue(Layout.PaddingProperty, new Thickness(0.0));
			grid.SetValue(VisualElement.BackgroundColorProperty, Color.Transparent);
			grid.SetValue(Grid.ColumnSpacingProperty, 0.0);
			grid.SetValue(View.HorizontalOptionsProperty, LayoutOptions.Fill);
			grid.SetValue(Grid.RowSpacingProperty, 0.0);
			grid.SetValue(View.VerticalOptionsProperty, LayoutOptions.Fill);
			rowDefinition.SetValue(RowDefinition.HeightProperty, new GridLengthTypeConverter().ConvertFromInvariantString("*"));
			grid.GetValue(Grid.RowDefinitionsProperty).Add(rowDefinition);
			gaugeBackground.SetValue(Grid.RowProperty, 0);
			gaugeBackground.SetValue(View.HorizontalOptionsProperty, LayoutOptions.Fill);
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
			gaugeBackground.SetValue(View.VerticalOptionsProperty, LayoutOptions.Fill);
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
			xamlServiceProvider.Add(typeFromHandle2, new XamlTypeResolver(xmlNamespaceResolver, typeof(SimpleTextItem).GetTypeInfo().Assembly));
			xamlServiceProvider.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(51, 17)));
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
			xamlServiceProvider2.Add(typeFromHandle4, new XamlTypeResolver(xmlNamespaceResolver2, typeof(SimpleTextItem).GetTypeInfo().Assembly));
			xamlServiceProvider2.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(53, 17)));
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
			label.SetValue(Grid.RowProperty, 0);
			label.SetValue(VisualElement.BackgroundColorProperty, Color.Transparent);
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
			grid.Children.Add(label);
			label2.SetValue(Grid.RowProperty, 0);
			label2.SetValue(VisualElement.BackgroundColorProperty, Color.Transparent);
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
			label2.SetBinding(Label.FontFamilyProperty, bindingBase8);
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
			label2.SetBinding(Label.FontSizeProperty, bindingBase9);
			label2.SetValue(View.HorizontalOptionsProperty, LayoutOptions.FillAndExpand);
			label2.SetValue(Label.HorizontalTextAlignmentProperty, new TextAlignmentConverter().ConvertFromInvariantString("Center"));
			staticResourceExtension3.Key = "PIDToValueLineBreakConverter";
			IMarkupExtension markupExtension3 = staticResourceExtension3;
			XamlServiceProvider xamlServiceProvider3 = new XamlServiceProvider();
			Type typeFromHandle5 = typeof(IProvideValueTarget);
			object[] array3 = new object[0 + 4];
			array3[0] = bindingExtension10;
			array3[1] = label2;
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
			xmlNamespaceResolver3.Add("grad", "clr-namespace:Xamarin.Forms.Essentials.Controls");
			xmlNamespaceResolver3.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver3.Add("settings", "clr-namespace:CarScannerXamarinForms.Settings");
			xamlServiceProvider3.Add(typeFromHandle6, new XamlTypeResolver(xmlNamespaceResolver3, typeof(SimpleTextItem).GetTypeInfo().Assembly));
			xamlServiceProvider3.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(76, 17)));
			object obj6 = markupExtension3.ProvideValue(xamlServiceProvider3);
			bindingExtension10.Converter = obj6;
			bindingExtension10.Path = "Model.ChartVisible";
			bindingExtension10.TypedBinding = new TypedBinding<DashboardItem, bool>(delegate(DashboardItem A_0)
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
			}, delegate(DashboardItem A_0, bool A_1)
			{
				if (A_0 != null)
				{
					LiveDataPIDModel model2 = A_0.Model;
					if (model2 != null)
					{
						model2.ChartVisible = A_1;
						return;
					}
				}
			}, new Tuple<Func<DashboardItem, object>, string>[]
			{
				new Tuple<Func<DashboardItem, object>, string>((DashboardItem A_0) => A_0, "Model"),
				new Tuple<Func<DashboardItem, object>, string>((DashboardItem A_0) => A_0.Model, "ChartVisible")
			});
			BindingBase bindingBase10 = bindingExtension10.ProvideValue(null);
			label2.SetBinding(Label.LineBreakModeProperty, bindingBase10);
			bindingExtension11.Mode = 2;
			bindingExtension11.Path = "Model.TextValue";
			bindingExtension11.TypedBinding = new TypedBinding<DashboardItem, string>(delegate(DashboardItem A_0)
			{
				if (A_0 != null)
				{
					LiveDataPIDModel model3 = A_0.Model;
					if (model3 != null)
					{
						return new ValueTuple<string, bool>(model3.TextValue, true);
					}
				}
				return default(ValueTuple<string, bool>);
			}, null, new Tuple<Func<DashboardItem, object>, string>[]
			{
				new Tuple<Func<DashboardItem, object>, string>((DashboardItem A_0) => A_0, "Model"),
				new Tuple<Func<DashboardItem, object>, string>((DashboardItem A_0) => A_0.Model, "TextValue")
			});
			BindingBase bindingBase11 = bindingExtension11.ProvideValue(null);
			label2.SetBinding(Label.TextProperty, bindingBase11);
			bindingExtension12.Mode = 2;
			bindingExtension12.Path = "ValueTextColor";
			bindingExtension12.TypedBinding = new TypedBinding<DashboardItem, Color>(delegate(DashboardItem A_0)
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
			BindingBase bindingBase12 = bindingExtension12.ProvideValue(null);
			label2.SetBinding(Label.TextColorProperty, bindingBase12);
			label2.SetValue(View.VerticalOptionsProperty, LayoutOptions.Center);
			label2.SetValue(Label.VerticalTextAlignmentProperty, new TextAlignmentConverter().ConvertFromInvariantString("Center"));
			grid.Children.Add(label2);
			label3.SetValue(Grid.RowProperty, 0);
			label3.SetValue(VisualElement.BackgroundColorProperty, Color.Transparent);
			bindingExtension13.Mode = 2;
			bindingExtension13.Path = "UnitsFontSize";
			bindingExtension13.TypedBinding = new TypedBinding<DashboardItem, double>(delegate(DashboardItem A_0)
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
			BindingBase bindingBase13 = bindingExtension13.ProvideValue(null);
			label3.SetBinding(Label.FontSizeProperty, bindingBase13);
			label3.SetValue(View.HorizontalOptionsProperty, LayoutOptions.FillAndExpand);
			label3.SetValue(Label.HorizontalTextAlignmentProperty, new TextAlignmentConverter().ConvertFromInvariantString("Center"));
			label3.SetValue(Label.LineBreakModeProperty, 0);
			bindingExtension14.Mode = 2;
			bindingExtension14.Path = "Model.Units";
			bindingExtension14.TypedBinding = new TypedBinding<DashboardItem, string>(delegate(DashboardItem A_0)
			{
				if (A_0 != null)
				{
					LiveDataPIDModel model4 = A_0.Model;
					if (model4 != null)
					{
						return new ValueTuple<string, bool>(model4.Units, true);
					}
				}
				return default(ValueTuple<string, bool>);
			}, null, new Tuple<Func<DashboardItem, object>, string>[]
			{
				new Tuple<Func<DashboardItem, object>, string>((DashboardItem A_0) => A_0, "Model"),
				new Tuple<Func<DashboardItem, object>, string>((DashboardItem A_0) => A_0.Model, "Units")
			});
			BindingBase bindingBase14 = bindingExtension14.ProvideValue(null);
			label3.SetBinding(Label.TextProperty, bindingBase14);
			bindingExtension15.Mode = 2;
			bindingExtension15.Path = "UnitsTextColor";
			bindingExtension15.TypedBinding = new TypedBinding<DashboardItem, Color>(delegate(DashboardItem A_0)
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
			BindingBase bindingBase15 = bindingExtension15.ProvideValue(null);
			label3.SetBinding(Label.TextColorProperty, bindingBase15);
			label3.SetValue(View.VerticalOptionsProperty, LayoutOptions.End);
			label3.SetValue(Label.VerticalTextAlignmentProperty, new TextAlignmentConverter().ConvertFromInvariantString("End"));
			grid.Children.Add(label3);
			stackLayout.SetValue(Grid.RowProperty, 0);
			stackLayout.SetValue(View.MarginProperty, new Thickness(0.0));
			stackLayout.SetValue(View.HorizontalOptionsProperty, LayoutOptions.End);
			stackLayout.SetValue(StackLayout.OrientationProperty, 0);
			stackLayout.SetValue(View.VerticalOptionsProperty, LayoutOptions.End);
			label4.SetValue(Label.FontAttributesProperty, new FontAttributesConverter().ConvertFromInvariantString("Bold"));
			bindingExtension16.Mode = 2;
			bindingExtension16.Path = "MinMaxAvgFontSize";
			bindingExtension16.TypedBinding = new TypedBinding<DashboardItem, double>(delegate(DashboardItem A_0)
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
			BindingBase bindingBase16 = bindingExtension16.ProvideValue(null);
			label4.SetBinding(Label.FontSizeProperty, bindingBase16);
			label4.SetValue(Label.HorizontalTextAlignmentProperty, new TextAlignmentConverter().ConvertFromInvariantString("End"));
			bindingExtension17.Mode = 2;
			bindingExtension17.Path = "ShowAvg";
			bindingExtension17.TypedBinding = new TypedBinding<DashboardItem, bool>(delegate(DashboardItem A_0)
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
			BindingBase bindingBase17 = bindingExtension17.ProvideValue(null);
			label4.SetBinding(VisualElement.IsVisibleProperty, bindingBase17);
			bindingExtension18.Mode = 2;
			bindingExtension18.Path = "MinMaxAvgColor";
			bindingExtension18.TypedBinding = new TypedBinding<DashboardItem, Color>(delegate(DashboardItem A_0)
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
			BindingBase bindingBase18 = bindingExtension18.ProvideValue(null);
			label4.SetBinding(Label.TextColorProperty, bindingBase18);
			span.SetValue(Span.TextProperty, "Avg: ");
			formattedString.Spans.Add(span);
			bindingExtension19.Mode = 2;
			bindingExtension19.Path = "Model.AverageTextValue";
			bindingExtension19.TypedBinding = new TypedBinding<DashboardItem, string>(delegate(DashboardItem A_0)
			{
				if (A_0 != null)
				{
					LiveDataPIDModel model5 = A_0.Model;
					if (model5 != null)
					{
						return new ValueTuple<string, bool>(model5.AverageTextValue, true);
					}
				}
				return default(ValueTuple<string, bool>);
			}, null, new Tuple<Func<DashboardItem, object>, string>[]
			{
				new Tuple<Func<DashboardItem, object>, string>((DashboardItem A_0) => A_0, "Model"),
				new Tuple<Func<DashboardItem, object>, string>((DashboardItem A_0) => A_0.Model, "AverageTextValue")
			});
			BindingBase bindingBase19 = bindingExtension19.ProvideValue(null);
			span2.SetBinding(Span.TextProperty, bindingBase19);
			formattedString.Spans.Add(span2);
			label4.SetValue(Label.FormattedTextProperty, formattedString);
			stackLayout.Children.Add(label4);
			label5.SetValue(View.MarginProperty, new Thickness(0.0));
			label5.SetValue(Label.FontAttributesProperty, new FontAttributesConverter().ConvertFromInvariantString("Bold"));
			dynamicResourceExtension.Key = "BaseFontSize--";
			IMarkupExtension<DynamicResource> markupExtension4 = dynamicResourceExtension;
			XamlServiceProvider xamlServiceProvider4 = new XamlServiceProvider();
			Type typeFromHandle7 = typeof(IProvideValueTarget);
			object[] array4 = new object[0 + 4];
			array4[0] = label5;
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
			xmlNamespaceResolver4.Add("grad", "clr-namespace:Xamarin.Forms.Essentials.Controls");
			xmlNamespaceResolver4.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver4.Add("settings", "clr-namespace:CarScannerXamarinForms.Settings");
			xamlServiceProvider4.Add(typeFromHandle8, new XamlTypeResolver(xmlNamespaceResolver4, typeof(SimpleTextItem).GetTypeInfo().Assembly));
			xamlServiceProvider4.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(122, 21)));
			DynamicResource dynamicResource = markupExtension4.ProvideValue(xamlServiceProvider4);
			label5.SetDynamicResource(Label.FontSizeProperty, dynamicResource.Key);
			label5.SetValue(Label.HorizontalTextAlignmentProperty, new TextAlignmentConverter().ConvertFromInvariantString("End"));
			bindingExtension20.Mode = 2;
			bindingExtension20.Source = sharedSettings;
			bindingExtension20.Path = "ShowPing";
			BindingBase bindingBase20 = bindingExtension20.ProvideValue(null);
			label5.SetBinding(VisualElement.IsVisibleProperty, bindingBase20);
			label5.SetValue(Label.TextColorProperty, Color.Red);
			bindingExtension21.Mode = 2;
			bindingExtension21.Path = "Model.Ping";
			bindingExtension21.TypedBinding = new TypedBinding<DashboardItem, long>(delegate(DashboardItem A_0)
			{
				if (A_0 != null)
				{
					LiveDataPIDModel model6 = A_0.Model;
					if (model6 != null)
					{
						return new ValueTuple<long, bool>(model6.Ping, true);
					}
				}
				return default(ValueTuple<long, bool>);
			}, null, new Tuple<Func<DashboardItem, object>, string>[]
			{
				new Tuple<Func<DashboardItem, object>, string>((DashboardItem A_0) => A_0, "Model"),
				new Tuple<Func<DashboardItem, object>, string>((DashboardItem A_0) => A_0.Model, "Ping")
			});
			BindingBase bindingBase21 = bindingExtension21.ProvideValue(null);
			span3.SetBinding(Span.TextProperty, bindingBase21);
			formattedString2.Spans.Add(span3);
			span4.SetValue(Span.TextProperty, "ms");
			formattedString2.Spans.Add(span4);
			label5.SetValue(Label.FormattedTextProperty, formattedString2);
			stackLayout.Children.Add(label5);
			grid.Children.Add(stackLayout);
			stackLayout2.SetValue(Grid.RowProperty, 0);
			stackLayout2.SetValue(View.MarginProperty, new Thickness(0.0));
			stackLayout2.SetValue(View.HorizontalOptionsProperty, LayoutOptions.Start);
			stackLayout2.SetValue(StackLayout.OrientationProperty, 0);
			stackLayout2.SetValue(View.VerticalOptionsProperty, LayoutOptions.End);
			label6.SetValue(Label.FontAttributesProperty, new FontAttributesConverter().ConvertFromInvariantString("Bold"));
			bindingExtension22.Mode = 2;
			bindingExtension22.Path = "MinMaxAvgFontSize";
			bindingExtension22.TypedBinding = new TypedBinding<DashboardItem, double>(delegate(DashboardItem A_0)
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
			BindingBase bindingBase22 = bindingExtension22.ProvideValue(null);
			label6.SetBinding(Label.FontSizeProperty, bindingBase22);
			bindingExtension23.Mode = 2;
			bindingExtension23.Path = "ShowMinMax";
			bindingExtension23.TypedBinding = new TypedBinding<DashboardItem, bool>(delegate(DashboardItem A_0)
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
			BindingBase bindingBase23 = bindingExtension23.ProvideValue(null);
			label6.SetBinding(VisualElement.IsVisibleProperty, bindingBase23);
			bindingExtension24.Mode = 2;
			bindingExtension24.Path = "MinMaxAvgColor";
			bindingExtension24.TypedBinding = new TypedBinding<DashboardItem, Color>(delegate(DashboardItem A_0)
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
			BindingBase bindingBase24 = bindingExtension24.ProvideValue(null);
			label6.SetBinding(Label.TextColorProperty, bindingBase24);
			span5.SetValue(Span.TextProperty, "Max: ");
			formattedString3.Spans.Add(span5);
			bindingExtension25.Mode = 2;
			bindingExtension25.Path = "Model.MaximumAchievedText";
			bindingExtension25.TypedBinding = new TypedBinding<DashboardItem, string>(delegate(DashboardItem A_0)
			{
				if (A_0 != null)
				{
					LiveDataPIDModel model7 = A_0.Model;
					if (model7 != null)
					{
						return new ValueTuple<string, bool>(model7.MaximumAchievedText, true);
					}
				}
				return default(ValueTuple<string, bool>);
			}, null, new Tuple<Func<DashboardItem, object>, string>[]
			{
				new Tuple<Func<DashboardItem, object>, string>((DashboardItem A_0) => A_0, "Model"),
				new Tuple<Func<DashboardItem, object>, string>((DashboardItem A_0) => A_0.Model, "MaximumAchievedText")
			});
			BindingBase bindingBase25 = bindingExtension25.ProvideValue(null);
			span6.SetBinding(Span.TextProperty, bindingBase25);
			formattedString3.Spans.Add(span6);
			label6.SetValue(Label.FormattedTextProperty, formattedString3);
			stackLayout2.Children.Add(label6);
			label7.SetValue(Label.FontAttributesProperty, new FontAttributesConverter().ConvertFromInvariantString("Bold"));
			bindingExtension26.Mode = 2;
			bindingExtension26.Path = "MinMaxAvgFontSize";
			bindingExtension26.TypedBinding = new TypedBinding<DashboardItem, double>(delegate(DashboardItem A_0)
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
			BindingBase bindingBase26 = bindingExtension26.ProvideValue(null);
			label7.SetBinding(Label.FontSizeProperty, bindingBase26);
			bindingExtension27.Mode = 2;
			bindingExtension27.Path = "ShowMinMax";
			bindingExtension27.TypedBinding = new TypedBinding<DashboardItem, bool>(delegate(DashboardItem A_0)
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
			BindingBase bindingBase27 = bindingExtension27.ProvideValue(null);
			label7.SetBinding(VisualElement.IsVisibleProperty, bindingBase27);
			bindingExtension28.Mode = 2;
			bindingExtension28.Path = "MinMaxAvgColor";
			bindingExtension28.TypedBinding = new TypedBinding<DashboardItem, Color>(delegate(DashboardItem A_0)
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
			BindingBase bindingBase28 = bindingExtension28.ProvideValue(null);
			label7.SetBinding(Label.TextColorProperty, bindingBase28);
			span7.SetValue(Span.TextProperty, "Min: ");
			formattedString4.Spans.Add(span7);
			bindingExtension29.Mode = 2;
			bindingExtension29.Path = "Model.MinimumAchievedText";
			bindingExtension29.TypedBinding = new TypedBinding<DashboardItem, string>(delegate(DashboardItem A_0)
			{
				if (A_0 != null)
				{
					LiveDataPIDModel model8 = A_0.Model;
					if (model8 != null)
					{
						return new ValueTuple<string, bool>(model8.MinimumAchievedText, true);
					}
				}
				return default(ValueTuple<string, bool>);
			}, null, new Tuple<Func<DashboardItem, object>, string>[]
			{
				new Tuple<Func<DashboardItem, object>, string>((DashboardItem A_0) => A_0, "Model"),
				new Tuple<Func<DashboardItem, object>, string>((DashboardItem A_0) => A_0.Model, "MinimumAchievedText")
			});
			BindingBase bindingBase29 = bindingExtension29.ProvideValue(null);
			span8.SetBinding(Span.TextProperty, bindingBase29);
			formattedString4.Spans.Add(span8);
			label7.SetValue(Label.FormattedTextProperty, formattedString4);
			stackLayout2.Children.Add(label7);
			grid.Children.Add(stackLayout2);
			this.SetValue(ContentView.ContentProperty, grid);
		}

		// Token: 0x060045BA RID: 17850 RVA: 0x0036630C File Offset: 0x0036450C
		private void __InitComponentRuntime()
		{
			Extensions.LoadFromXaml<SimpleTextItem>(this, typeof(SimpleTextItem));
			this.bindingHackName = NameScopeExtensions.FindByName<ContentView>(this, "bindingHackName");
			this.labelValue = NameScopeExtensions.FindByName<Label>(this, "labelValue");
		}

		// Token: 0x060045BB RID: 17851 RVA: 0x00366344 File Offset: 0x00364544
		[CompilerGenerated]
		private static ValueTuple<bool, bool> <InitializeComponent>typedBindingsM__917(DashboardItem A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<bool, bool>(A_0.ShowDefaultBackground, true);
			}
			return default(ValueTuple<bool, bool>);
		}

		// Token: 0x060045BC RID: 17852 RVA: 0x00366374 File Offset: 0x00364574
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__918(DashboardItem A_0)
		{
			return A_0;
		}

		// Token: 0x060045BD RID: 17853 RVA: 0x00366384 File Offset: 0x00364584
		[CompilerGenerated]
		private static ValueTuple<Thickness, bool> <InitializeComponent>typedBindingsM__919(DashboardItem A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<Thickness, bool>(A_0.CornerRadius, true);
			}
			return default(ValueTuple<Thickness, bool>);
		}

		// Token: 0x060045BE RID: 17854 RVA: 0x003663B4 File Offset: 0x003645B4
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__920(DashboardItem A_0)
		{
			return A_0;
		}

		// Token: 0x060045BF RID: 17855 RVA: 0x003663C4 File Offset: 0x003645C4
		[CompilerGenerated]
		private static ValueTuple<Color, bool> <InitializeComponent>typedBindingsM__921(DashboardItem A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<Color, bool>(A_0.IndicatorBackgroundColor, true);
			}
			return default(ValueTuple<Color, bool>);
		}

		// Token: 0x060045C0 RID: 17856 RVA: 0x003663F4 File Offset: 0x003645F4
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__922(DashboardItem A_0)
		{
			return A_0;
		}

		// Token: 0x060045C1 RID: 17857 RVA: 0x00366404 File Offset: 0x00364604
		[CompilerGenerated]
		private static ValueTuple<Color, bool> <InitializeComponent>typedBindingsM__923(DashboardItem A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<Color, bool>(A_0.IndicatorBackgroundColor, true);
			}
			return default(ValueTuple<Color, bool>);
		}

		// Token: 0x060045C2 RID: 17858 RVA: 0x00366434 File Offset: 0x00364634
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__924(DashboardItem A_0)
		{
			return A_0;
		}

		// Token: 0x060045C3 RID: 17859 RVA: 0x00366444 File Offset: 0x00364644
		[CompilerGenerated]
		private static ValueTuple<double, bool> <InitializeComponent>typedBindingsM__925(DashboardItem A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<double, bool>(A_0.TitleFontSize, true);
			}
			return default(ValueTuple<double, bool>);
		}

		// Token: 0x060045C4 RID: 17860 RVA: 0x00366474 File Offset: 0x00364674
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__926(DashboardItem A_0)
		{
			return A_0;
		}

		// Token: 0x060045C5 RID: 17861 RVA: 0x00366484 File Offset: 0x00364684
		[CompilerGenerated]
		private static ValueTuple<string, bool> <InitializeComponent>typedBindingsM__927(DashboardItem A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<string, bool>(A_0.PIDName, true);
			}
			return default(ValueTuple<string, bool>);
		}

		// Token: 0x060045C6 RID: 17862 RVA: 0x003664B4 File Offset: 0x003646B4
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__928(DashboardItem A_0)
		{
			return A_0;
		}

		// Token: 0x060045C7 RID: 17863 RVA: 0x003664C4 File Offset: 0x003646C4
		[CompilerGenerated]
		private static ValueTuple<Color, bool> <InitializeComponent>typedBindingsM__929(DashboardItem A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<Color, bool>(A_0.TitleTextColor, true);
			}
			return default(ValueTuple<Color, bool>);
		}

		// Token: 0x060045C8 RID: 17864 RVA: 0x003664F4 File Offset: 0x003646F4
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__930(DashboardItem A_0)
		{
			return A_0;
		}

		// Token: 0x060045C9 RID: 17865 RVA: 0x00366504 File Offset: 0x00364704
		[CompilerGenerated]
		private static ValueTuple<string, bool> <InitializeComponent>typedBindingsM__931(DashboardItem A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<string, bool>(A_0.FontName, true);
			}
			return default(ValueTuple<string, bool>);
		}

		// Token: 0x060045CA RID: 17866 RVA: 0x00366534 File Offset: 0x00364734
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__932(DashboardItem A_0)
		{
			return A_0;
		}

		// Token: 0x060045CB RID: 17867 RVA: 0x00366544 File Offset: 0x00364744
		[CompilerGenerated]
		private static ValueTuple<double, bool> <InitializeComponent>typedBindingsM__933(DashboardItem A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<double, bool>(A_0.ValueFontSize, true);
			}
			return default(ValueTuple<double, bool>);
		}

		// Token: 0x060045CC RID: 17868 RVA: 0x00366574 File Offset: 0x00364774
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__934(DashboardItem A_0)
		{
			return A_0;
		}

		// Token: 0x060045CD RID: 17869 RVA: 0x00366584 File Offset: 0x00364784
		[CompilerGenerated]
		private static ValueTuple<bool, bool> <InitializeComponent>typedBindingsM__935(DashboardItem A_0)
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

		// Token: 0x060045CE RID: 17870 RVA: 0x003665BC File Offset: 0x003647BC
		[CompilerGenerated]
		private static void <InitializeComponent>typedBindingsM__936(DashboardItem A_0, bool A_1)
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

		// Token: 0x060045CF RID: 17871 RVA: 0x003665E4 File Offset: 0x003647E4
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__937(DashboardItem A_0)
		{
			return A_0;
		}

		// Token: 0x060045D0 RID: 17872 RVA: 0x003665F4 File Offset: 0x003647F4
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__938(DashboardItem A_0)
		{
			return A_0.Model;
		}

		// Token: 0x060045D1 RID: 17873 RVA: 0x00366608 File Offset: 0x00364808
		[CompilerGenerated]
		private static ValueTuple<string, bool> <InitializeComponent>typedBindingsM__939(DashboardItem A_0)
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

		// Token: 0x060045D2 RID: 17874 RVA: 0x00366640 File Offset: 0x00364840
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__940(DashboardItem A_0)
		{
			return A_0;
		}

		// Token: 0x060045D3 RID: 17875 RVA: 0x00366650 File Offset: 0x00364850
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__941(DashboardItem A_0)
		{
			return A_0.Model;
		}

		// Token: 0x060045D4 RID: 17876 RVA: 0x00366664 File Offset: 0x00364864
		[CompilerGenerated]
		private static ValueTuple<Color, bool> <InitializeComponent>typedBindingsM__942(DashboardItem A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<Color, bool>(A_0.ValueTextColor, true);
			}
			return default(ValueTuple<Color, bool>);
		}

		// Token: 0x060045D5 RID: 17877 RVA: 0x00366694 File Offset: 0x00364894
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__943(DashboardItem A_0)
		{
			return A_0;
		}

		// Token: 0x060045D6 RID: 17878 RVA: 0x003666A4 File Offset: 0x003648A4
		[CompilerGenerated]
		private static ValueTuple<double, bool> <InitializeComponent>typedBindingsM__944(DashboardItem A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<double, bool>(A_0.UnitsFontSize, true);
			}
			return default(ValueTuple<double, bool>);
		}

		// Token: 0x060045D7 RID: 17879 RVA: 0x003666D4 File Offset: 0x003648D4
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__945(DashboardItem A_0)
		{
			return A_0;
		}

		// Token: 0x060045D8 RID: 17880 RVA: 0x003666E4 File Offset: 0x003648E4
		[CompilerGenerated]
		private static ValueTuple<string, bool> <InitializeComponent>typedBindingsM__946(DashboardItem A_0)
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

		// Token: 0x060045D9 RID: 17881 RVA: 0x0036671C File Offset: 0x0036491C
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__947(DashboardItem A_0)
		{
			return A_0;
		}

		// Token: 0x060045DA RID: 17882 RVA: 0x0036672C File Offset: 0x0036492C
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__948(DashboardItem A_0)
		{
			return A_0.Model;
		}

		// Token: 0x060045DB RID: 17883 RVA: 0x00366740 File Offset: 0x00364940
		[CompilerGenerated]
		private static ValueTuple<Color, bool> <InitializeComponent>typedBindingsM__949(DashboardItem A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<Color, bool>(A_0.UnitsTextColor, true);
			}
			return default(ValueTuple<Color, bool>);
		}

		// Token: 0x060045DC RID: 17884 RVA: 0x00366770 File Offset: 0x00364970
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__950(DashboardItem A_0)
		{
			return A_0;
		}

		// Token: 0x060045DD RID: 17885 RVA: 0x00366780 File Offset: 0x00364980
		[CompilerGenerated]
		private static ValueTuple<double, bool> <InitializeComponent>typedBindingsM__951(DashboardItem A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<double, bool>(A_0.MinMaxAvgFontSize, true);
			}
			return default(ValueTuple<double, bool>);
		}

		// Token: 0x060045DE RID: 17886 RVA: 0x003667B0 File Offset: 0x003649B0
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__952(DashboardItem A_0)
		{
			return A_0;
		}

		// Token: 0x060045DF RID: 17887 RVA: 0x003667C0 File Offset: 0x003649C0
		[CompilerGenerated]
		private static ValueTuple<bool, bool> <InitializeComponent>typedBindingsM__953(DashboardItem A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<bool, bool>(A_0.ShowAvg, true);
			}
			return default(ValueTuple<bool, bool>);
		}

		// Token: 0x060045E0 RID: 17888 RVA: 0x003667F0 File Offset: 0x003649F0
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__954(DashboardItem A_0)
		{
			return A_0;
		}

		// Token: 0x060045E1 RID: 17889 RVA: 0x00366800 File Offset: 0x00364A00
		[CompilerGenerated]
		private static ValueTuple<Color, bool> <InitializeComponent>typedBindingsM__955(DashboardItem A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<Color, bool>(A_0.MinMaxAvgColor, true);
			}
			return default(ValueTuple<Color, bool>);
		}

		// Token: 0x060045E2 RID: 17890 RVA: 0x00366830 File Offset: 0x00364A30
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__956(DashboardItem A_0)
		{
			return A_0;
		}

		// Token: 0x060045E3 RID: 17891 RVA: 0x00366840 File Offset: 0x00364A40
		[CompilerGenerated]
		private static ValueTuple<string, bool> <InitializeComponent>typedBindingsM__957(DashboardItem A_0)
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

		// Token: 0x060045E4 RID: 17892 RVA: 0x00366878 File Offset: 0x00364A78
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__958(DashboardItem A_0)
		{
			return A_0;
		}

		// Token: 0x060045E5 RID: 17893 RVA: 0x00366888 File Offset: 0x00364A88
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__959(DashboardItem A_0)
		{
			return A_0.Model;
		}

		// Token: 0x060045E6 RID: 17894 RVA: 0x0036689C File Offset: 0x00364A9C
		[CompilerGenerated]
		private static ValueTuple<long, bool> <InitializeComponent>typedBindingsM__960(DashboardItem A_0)
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

		// Token: 0x060045E7 RID: 17895 RVA: 0x003668D4 File Offset: 0x00364AD4
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__961(DashboardItem A_0)
		{
			return A_0;
		}

		// Token: 0x060045E8 RID: 17896 RVA: 0x003668E4 File Offset: 0x00364AE4
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__962(DashboardItem A_0)
		{
			return A_0.Model;
		}

		// Token: 0x060045E9 RID: 17897 RVA: 0x003668F8 File Offset: 0x00364AF8
		[CompilerGenerated]
		private static ValueTuple<double, bool> <InitializeComponent>typedBindingsM__963(DashboardItem A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<double, bool>(A_0.MinMaxAvgFontSize, true);
			}
			return default(ValueTuple<double, bool>);
		}

		// Token: 0x060045EA RID: 17898 RVA: 0x00366928 File Offset: 0x00364B28
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__964(DashboardItem A_0)
		{
			return A_0;
		}

		// Token: 0x060045EB RID: 17899 RVA: 0x00366938 File Offset: 0x00364B38
		[CompilerGenerated]
		private static ValueTuple<bool, bool> <InitializeComponent>typedBindingsM__965(DashboardItem A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<bool, bool>(A_0.ShowMinMax, true);
			}
			return default(ValueTuple<bool, bool>);
		}

		// Token: 0x060045EC RID: 17900 RVA: 0x00366968 File Offset: 0x00364B68
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__966(DashboardItem A_0)
		{
			return A_0;
		}

		// Token: 0x060045ED RID: 17901 RVA: 0x00366978 File Offset: 0x00364B78
		[CompilerGenerated]
		private static ValueTuple<Color, bool> <InitializeComponent>typedBindingsM__967(DashboardItem A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<Color, bool>(A_0.MinMaxAvgColor, true);
			}
			return default(ValueTuple<Color, bool>);
		}

		// Token: 0x060045EE RID: 17902 RVA: 0x003669A8 File Offset: 0x00364BA8
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__968(DashboardItem A_0)
		{
			return A_0;
		}

		// Token: 0x060045EF RID: 17903 RVA: 0x003669B8 File Offset: 0x00364BB8
		[CompilerGenerated]
		private static ValueTuple<string, bool> <InitializeComponent>typedBindingsM__969(DashboardItem A_0)
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

		// Token: 0x060045F0 RID: 17904 RVA: 0x003669F0 File Offset: 0x00364BF0
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__970(DashboardItem A_0)
		{
			return A_0;
		}

		// Token: 0x060045F1 RID: 17905 RVA: 0x00366A00 File Offset: 0x00364C00
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__971(DashboardItem A_0)
		{
			return A_0.Model;
		}

		// Token: 0x060045F2 RID: 17906 RVA: 0x00366A14 File Offset: 0x00364C14
		[CompilerGenerated]
		private static ValueTuple<double, bool> <InitializeComponent>typedBindingsM__972(DashboardItem A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<double, bool>(A_0.MinMaxAvgFontSize, true);
			}
			return default(ValueTuple<double, bool>);
		}

		// Token: 0x060045F3 RID: 17907 RVA: 0x00366A44 File Offset: 0x00364C44
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__973(DashboardItem A_0)
		{
			return A_0;
		}

		// Token: 0x060045F4 RID: 17908 RVA: 0x00366A54 File Offset: 0x00364C54
		[CompilerGenerated]
		private static ValueTuple<bool, bool> <InitializeComponent>typedBindingsM__974(DashboardItem A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<bool, bool>(A_0.ShowMinMax, true);
			}
			return default(ValueTuple<bool, bool>);
		}

		// Token: 0x060045F5 RID: 17909 RVA: 0x00366A84 File Offset: 0x00364C84
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__975(DashboardItem A_0)
		{
			return A_0;
		}

		// Token: 0x060045F6 RID: 17910 RVA: 0x00366A94 File Offset: 0x00364C94
		[CompilerGenerated]
		private static ValueTuple<Color, bool> <InitializeComponent>typedBindingsM__976(DashboardItem A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<Color, bool>(A_0.MinMaxAvgColor, true);
			}
			return default(ValueTuple<Color, bool>);
		}

		// Token: 0x060045F7 RID: 17911 RVA: 0x00366AC4 File Offset: 0x00364CC4
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__977(DashboardItem A_0)
		{
			return A_0;
		}

		// Token: 0x060045F8 RID: 17912 RVA: 0x00366AD4 File Offset: 0x00364CD4
		[CompilerGenerated]
		private static ValueTuple<string, bool> <InitializeComponent>typedBindingsM__978(DashboardItem A_0)
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

		// Token: 0x060045F9 RID: 17913 RVA: 0x00366B0C File Offset: 0x00364D0C
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__979(DashboardItem A_0)
		{
			return A_0;
		}

		// Token: 0x060045FA RID: 17914 RVA: 0x00366B1C File Offset: 0x00364D1C
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__980(DashboardItem A_0)
		{
			return A_0.Model;
		}

		// Token: 0x040028D9 RID: 10457
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private ContentView bindingHackName;

		// Token: 0x040028DA RID: 10458
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private Label labelValue;
	}
}
