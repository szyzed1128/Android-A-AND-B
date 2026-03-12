using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Windows.Input;
using CarScannerXamarinForms.Common.XAMLConverters;
using DLToolkit.Forms.Controls;
using Xamarin.Forms;
using Xamarin.Forms.Internals;
using Xamarin.Forms.PlatformConfiguration.iOSSpecific;
using Xamarin.Forms.Xaml;
using Xamarin.Forms.Xaml.Diagnostics;
using Xamarin.Forms.Xaml.Internals;

namespace CarScannerXamarinForms
{
	// Token: 0x0200018F RID: 399
	[XamlFilePath("UserControls\\ColorSelectorPage.xaml")]
	public class ColorSelectorPage : ContentPage
	{
		// Token: 0x17000F5E RID: 3934
		// (get) Token: 0x06001617 RID: 5655 RVA: 0x0009C03B File Offset: 0x0009A23B
		// (set) Token: 0x06001618 RID: 5656 RVA: 0x0009C043 File Offset: 0x0009A243
		public ICommand ItemTappedCommand
		{
			[CompilerGenerated]
			get
			{
				return this.<ItemTappedCommand>k__BackingField;
			}
			[CompilerGenerated]
			private set
			{
				this.<ItemTappedCommand>k__BackingField = value;
			}
		}

		// Token: 0x06001619 RID: 5657 RVA: 0x0009C04C File Offset: 0x0009A24C
		public ColorSelectorPage(string PropertyName, object Item)
		{
			ColorSelectorPage <>4__this = this;
			this.Item = Item;
			this.PropertyName = PropertyName;
			if (this._Colors == null)
			{
				this.CreateColors();
			}
			this.ItemTappedCommand = new Command(delegate(object param)
			{
				Item.GetType().GetProperty(PropertyName).SetValue(Item, (param as ColorItem).Color);
				<>4__this.Navigation.PopAsync(true);
			});
			this.InitializeComponent();
			base.Disappearing += this.Page_Disappearing;
			base.BindingContext = this;
			object value = Item.GetType().GetProperty(PropertyName).GetValue(Item);
			if (value == null || !(value is Color))
			{
				this.lvColors.SelectedItem = this.Colors.FirstOrDefault<ColorItem>();
				return;
			}
			Color c = (Color)value;
			ColorItem colorItem = this.Colors.FirstOrDefault((ColorItem x) => x.Color == c);
			if (colorItem == null)
			{
				this.lvColors.SelectedItem = this.Colors.FirstOrDefault<ColorItem>();
			}
			else
			{
				this.lvColors.SelectedItem = colorItem;
			}
			try
			{
				this.lvColors.FlowScrollTo(colorItem, 2, false);
			}
			catch
			{
			}
		}

		// Token: 0x0600161A RID: 5658 RVA: 0x000027D4 File Offset: 0x000009D4
		private void Page_Disappearing(object sender, EventArgs e)
		{
		}

		// Token: 0x0600161B RID: 5659 RVA: 0x0009C194 File Offset: 0x0009A394
		private void CreateColors()
		{
			int num = 48;
			this._Colors = new List<ColorItem>(256 / num * 3 + 12);
			for (int i = 0; i <= 256; i += num)
			{
				for (int j = 0; j <= 256; j += num)
				{
					for (int k = 0; k <= 256; k += num)
					{
						double num2 = (double)i / 256.0;
						double num3 = (double)j / 256.0;
						double num4 = (double)k / 256.0;
						Color color;
						color..ctor(num2, num3, num4);
						ColorItem colorItem = new ColorItem
						{
							Color = color
						};
						this._Colors.Add(colorItem);
					}
				}
			}
			this._Colors.Sort((ColorItem x, ColorItem y) => ColorSelectorPage.CompareColors(x, y));
			this._Colors.Insert(0, new ColorItem
			{
				Color = Color.Transparent
			});
		}

		// Token: 0x0600161C RID: 5660 RVA: 0x0009C284 File Offset: 0x0009A484
		private static int CompareColors(object x, object y)
		{
			Color color = ((ColorItem)x).Color;
			Color color2 = ((ColorItem)y).Color;
			double saturation = color.Saturation;
			double saturation2 = color2.Saturation;
			double hue = color.Hue;
			double hue2 = color2.Hue;
			double brightness = ColorSelectorPage.GetBrightness(color);
			double brightness2 = ColorSelectorPage.GetBrightness(color2);
			if (hue < hue2)
			{
				return -1;
			}
			if (hue > hue2)
			{
				return 1;
			}
			if (saturation < saturation2)
			{
				return -1;
			}
			if (saturation > saturation2)
			{
				return 1;
			}
			if (brightness < brightness2)
			{
				return -1;
			}
			if (brightness > brightness2)
			{
				return 1;
			}
			return 0;
		}

		// Token: 0x0600161D RID: 5661 RVA: 0x0009C308 File Offset: 0x0009A508
		private static double GetBrightness(Color c)
		{
			double num = c.R / 255.0;
			double num2 = c.G / 255.0;
			double num3 = c.B / 255.0;
			double num4 = num;
			double num5 = num;
			if (num2 > num4)
			{
				num4 = num2;
			}
			if (num3 > num4)
			{
				num4 = num3;
			}
			if (num2 < num5)
			{
				num5 = num2;
			}
			if (num3 < num5)
			{
				num5 = num3;
			}
			return (num4 + num5) / 2.0;
		}

		// Token: 0x17000F5F RID: 3935
		// (get) Token: 0x0600161E RID: 5662 RVA: 0x0009C375 File Offset: 0x0009A575
		public List<ColorItem> Colors
		{
			get
			{
				return this._Colors;
			}
		}

		// Token: 0x0600161F RID: 5663 RVA: 0x0009C380 File Offset: 0x0009A580
		private async void lvColors_ItemTapped(object sender, ItemTappedEventArgs e)
		{
		}

		// Token: 0x06001620 RID: 5664 RVA: 0x000027D4 File Offset: 0x000009D4
		private void boxView_Tapped(object sender, EventArgs e)
		{
		}

		// Token: 0x06001621 RID: 5665 RVA: 0x0009C3B0 File Offset: 0x0009A5B0
		private async void btnCancel_Clicked(object sender, EventArgs e)
		{
			await base.Navigation.PopAsync(true);
		}

		// Token: 0x06001622 RID: 5666 RVA: 0x0009C3E8 File Offset: 0x0009A5E8
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private void InitializeComponent()
		{
			if (ResourceLoader.IsEnabled && ResourceLoader.CanProvideContentFor(new ResourceLoader.ResourceLoadingQuery
			{
				AssemblyName = typeof(ColorSelectorPage).GetTypeInfo().Assembly.GetName(),
				ResourcePath = "UserControls/ColorSelectorPage.xaml",
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
			DynamicResourceExtension dynamicResourceExtension;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension = new DynamicResourceExtension(), new Uri("UserControls\\ColorSelectorPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 12, 5);
			TranparentColorToTrueConverter tranparentColorToTrueConverter;
			VisualDiagnostics.RegisterSourceInfo(tranparentColorToTrueConverter = new TranparentColorToTrueConverter(), new Uri("UserControls\\ColorSelectorPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 17, 14);
			TransparentColorToTransparentImagePathConverter transparentColorToTransparentImagePathConverter;
			VisualDiagnostics.RegisterSourceInfo(transparentColorToTransparentImagePathConverter = new TransparentColorToTransparentImagePathConverter(), new Uri("UserControls\\ColorSelectorPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 18, 14);
			ResourceDictionary resourceDictionary;
			VisualDiagnostics.RegisterSourceInfo(resourceDictionary = new ResourceDictionary(), new Uri("UserControls\\ColorSelectorPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 16, 10);
			OnPlatform<Thickness> onPlatform;
			VisualDiagnostics.RegisterSourceInfo(onPlatform = new OnPlatform<Thickness>(), new Uri("UserControls\\ColorSelectorPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 22, 10);
			ColumnDefinition columnDefinition;
			VisualDiagnostics.RegisterSourceInfo(columnDefinition = new ColumnDefinition(), new Uri("UserControls\\ColorSelectorPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 31, 18);
			ColumnDefinition columnDefinition2;
			VisualDiagnostics.RegisterSourceInfo(columnDefinition2 = new ColumnDefinition(), new Uri("UserControls\\ColorSelectorPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 32, 18);
			ColumnDefinition columnDefinition3;
			VisualDiagnostics.RegisterSourceInfo(columnDefinition3 = new ColumnDefinition(), new Uri("UserControls\\ColorSelectorPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 33, 18);
			DynamicResourceExtension dynamicResourceExtension2;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension2 = new DynamicResourceExtension(), new Uri("UserControls\\ColorSelectorPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 40, 17);
			Translate translate;
			VisualDiagnostics.RegisterSourceInfo(translate = new Translate(), new Uri("UserControls\\ColorSelectorPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 41, 17);
			OnPlatform<bool> onPlatform2;
			VisualDiagnostics.RegisterSourceInfo(onPlatform2 = new OnPlatform<bool>(), new Uri("UserControls\\ColorSelectorPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 43, 22);
			LinkButton linkButton;
			VisualDiagnostics.RegisterSourceInfo(linkButton = new LinkButton(), new Uri("UserControls\\ColorSelectorPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 35, 14);
			DynamicResourceExtension dynamicResourceExtension3;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension3 = new DynamicResourceExtension(), new Uri("UserControls\\ColorSelectorPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 54, 17);
			Translate translate2;
			VisualDiagnostics.RegisterSourceInfo(translate2 = new Translate(), new Uri("UserControls\\ColorSelectorPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 55, 17);
			Label label;
			VisualDiagnostics.RegisterSourceInfo(label = new Label(), new Uri("UserControls\\ColorSelectorPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 50, 14);
			Grid grid;
			VisualDiagnostics.RegisterSourceInfo(grid = new Grid(), new Uri("UserControls\\ColorSelectorPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 29, 10);
			RowDefinition rowDefinition;
			VisualDiagnostics.RegisterSourceInfo(rowDefinition = new RowDefinition(), new Uri("UserControls\\ColorSelectorPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 63, 18);
			RowDefinition rowDefinition2;
			VisualDiagnostics.RegisterSourceInfo(rowDefinition2 = new RowDefinition(), new Uri("UserControls\\ColorSelectorPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 64, 18);
			ColumnDefinition columnDefinition4;
			VisualDiagnostics.RegisterSourceInfo(columnDefinition4 = new ColumnDefinition(), new Uri("UserControls\\ColorSelectorPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 68, 18);
			ColumnDefinition columnDefinition5;
			VisualDiagnostics.RegisterSourceInfo(columnDefinition5 = new ColumnDefinition(), new Uri("UserControls\\ColorSelectorPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 69, 18);
			ColumnDefinition columnDefinition6;
			VisualDiagnostics.RegisterSourceInfo(columnDefinition6 = new ColumnDefinition(), new Uri("UserControls\\ColorSelectorPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 70, 18);
			BindingExtension bindingExtension;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension = new BindingExtension(), new Uri("UserControls\\ColorSelectorPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 81, 17);
			BindingExtension bindingExtension2;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension2 = new BindingExtension(), new Uri("UserControls\\ColorSelectorPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 82, 17);
			DataTemplate dataTemplate;
			VisualDiagnostics.RegisterSourceInfo(dataTemplate = new DataTemplate(), new Uri("UserControls\\ColorSelectorPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 89, 22);
			FlowListView flowListView;
			VisualDiagnostics.RegisterSourceInfo(flowListView = new FlowListView(), new Uri("UserControls\\ColorSelectorPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 74, 14);
			Grid grid2;
			VisualDiagnostics.RegisterSourceInfo(grid2 = new Grid(), new Uri("UserControls\\ColorSelectorPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 61, 10);
			VisualDiagnostics.RegisterSourceInfo(this, new Uri("UserControls\\ColorSelectorPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 2, 2);
			NameScope nameScope = NameScope.GetNameScope(this) ?? new NameScope();
			NameScope.SetNameScope(this, nameScope);
			nameScope.RegisterName("lvColors", flowListView);
			if (flowListView.StyleId == null)
			{
				flowListView.StyleId = "lvColors";
			}
			this.lvColors = flowListView;
			this.Resources = resourceDictionary;
			resourceDictionary.Add("TranparentColorToTrueConverter", tranparentColorToTrueConverter);
			resourceDictionary.Add("TransparentColorToTransparentImagePathConverter", transparentColorToTransparentImagePathConverter);
			this.SetValue(Page.PrefersStatusBarHiddenProperty, 2);
			this.SetValue(Page.UseSafeAreaProperty, true);
			dynamicResourceExtension.Key = "BackgroundColor";
			IMarkupExtension<DynamicResource> markupExtension = dynamicResourceExtension;
			XamlServiceProvider xamlServiceProvider = new XamlServiceProvider();
			Type typeFromHandle = typeof(IProvideValueTarget);
			object[] array = new object[0 + 1];
			array[0] = this;
			object obj;
			xamlServiceProvider.Add(typeFromHandle, obj = new SimpleValueTargetProvider(array, VisualElement.BackgroundColorProperty, nameScope));
			xamlServiceProvider.Add(typeof(IReferenceProvider), obj);
			Type typeFromHandle2 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver = new XmlNamespaceResolver();
			xmlNamespaceResolver.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver.Add("flv", "clr-namespace:DLToolkit.Forms.Controls;assembly=DLToolkit.Forms.Controls.FlowListView");
			xmlNamespaceResolver.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver.Add("local", "clr-namespace:CarScannerXamarinForms");
			xamlServiceProvider.Add(typeFromHandle2, new XamlTypeResolver(xmlNamespaceResolver, typeof(ColorSelectorPage).GetTypeInfo().Assembly));
			xamlServiceProvider.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(12, 5)));
			DynamicResource dynamicResource = markupExtension.ProvideValue(xamlServiceProvider);
			this.SetDynamicResource(VisualElement.BackgroundColorProperty, dynamicResource.Key);
			this.SetValue(NavigationPage.HasBackButtonProperty, true);
			this.SetValue(NavigationPage.HasNavigationBarProperty, true);
			this.Resources = resourceDictionary;
			onPlatform.Android = new Thickness(5.0, 0.0, 5.0, 0.0);
			onPlatform.iOS = new Thickness(5.0, 0.0, 5.0, 0.0);
			this.SetValue(Page.PaddingProperty, onPlatform);
			columnDefinition.SetValue(ColumnDefinition.WidthProperty, new GridLengthTypeConverter().ConvertFromInvariantString("*"));
			grid.GetValue(Grid.ColumnDefinitionsProperty).Add(columnDefinition);
			columnDefinition2.SetValue(ColumnDefinition.WidthProperty, new GridLengthTypeConverter().ConvertFromInvariantString("auto"));
			grid.GetValue(Grid.ColumnDefinitionsProperty).Add(columnDefinition2);
			columnDefinition3.SetValue(ColumnDefinition.WidthProperty, new GridLengthTypeConverter().ConvertFromInvariantString("*"));
			grid.GetValue(Grid.ColumnDefinitionsProperty).Add(columnDefinition3);
			linkButton.SetValue(Grid.RowProperty, 0);
			linkButton.SetValue(Grid.ColumnProperty, 0);
			linkButton.Clicked += this.btnCancel_Clicked;
			linkButton.SetValue(View.HorizontalOptionsProperty, LayoutOptions.Start);
			dynamicResourceExtension2.Key = "NavigationBarButton";
			IMarkupExtension<DynamicResource> markupExtension2 = dynamicResourceExtension2;
			XamlServiceProvider xamlServiceProvider2 = new XamlServiceProvider();
			Type typeFromHandle3 = typeof(IProvideValueTarget);
			object[] array2 = new object[0 + 3];
			array2[0] = linkButton;
			array2[1] = grid;
			array2[2] = this;
			object obj2;
			xamlServiceProvider2.Add(typeFromHandle3, obj2 = new SimpleValueTargetProvider(array2, VisualElement.StyleProperty, nameScope));
			xamlServiceProvider2.Add(typeof(IReferenceProvider), obj2);
			Type typeFromHandle4 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver2 = new XmlNamespaceResolver();
			xmlNamespaceResolver2.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver2.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver2.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver2.Add("flv", "clr-namespace:DLToolkit.Forms.Controls;assembly=DLToolkit.Forms.Controls.FlowListView");
			xmlNamespaceResolver2.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver2.Add("local", "clr-namespace:CarScannerXamarinForms");
			xamlServiceProvider2.Add(typeFromHandle4, new XamlTypeResolver(xmlNamespaceResolver2, typeof(ColorSelectorPage).GetTypeInfo().Assembly));
			xamlServiceProvider2.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(40, 17)));
			DynamicResource dynamicResource2 = markupExtension2.ProvideValue(xamlServiceProvider2);
			linkButton.SetDynamicResource(VisualElement.StyleProperty, dynamicResource2.Key);
			translate.Text = "ios_Cancel";
			IMarkupExtension markupExtension3 = translate;
			XamlServiceProvider xamlServiceProvider3 = new XamlServiceProvider();
			Type typeFromHandle5 = typeof(IProvideValueTarget);
			object[] array3 = new object[0 + 3];
			array3[0] = linkButton;
			array3[1] = grid;
			array3[2] = this;
			object obj3;
			xamlServiceProvider3.Add(typeFromHandle5, obj3 = new SimpleValueTargetProvider(array3, Button.TextProperty, nameScope));
			xamlServiceProvider3.Add(typeof(IReferenceProvider), obj3);
			Type typeFromHandle6 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver3 = new XmlNamespaceResolver();
			xmlNamespaceResolver3.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver3.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver3.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver3.Add("flv", "clr-namespace:DLToolkit.Forms.Controls;assembly=DLToolkit.Forms.Controls.FlowListView");
			xmlNamespaceResolver3.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver3.Add("local", "clr-namespace:CarScannerXamarinForms");
			xamlServiceProvider3.Add(typeFromHandle6, new XamlTypeResolver(xmlNamespaceResolver3, typeof(ColorSelectorPage).GetTypeInfo().Assembly));
			xamlServiceProvider3.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(41, 17)));
			object obj4 = markupExtension3.ProvideValue(xamlServiceProvider3);
			linkButton.Text = obj4;
			onPlatform2.iOS = true;
			onPlatform2.Android = false;
			linkButton.SetValue(VisualElement.IsVisibleProperty, onPlatform2);
			grid.Children.Add(linkButton);
			label.SetValue(Grid.RowProperty, 0);
			label.SetValue(Grid.ColumnProperty, 1);
			label.SetValue(View.HorizontalOptionsProperty, LayoutOptions.Center);
			dynamicResourceExtension3.Key = "NavigationBarLabel";
			IMarkupExtension<DynamicResource> markupExtension4 = dynamicResourceExtension3;
			XamlServiceProvider xamlServiceProvider4 = new XamlServiceProvider();
			Type typeFromHandle7 = typeof(IProvideValueTarget);
			object[] array4 = new object[0 + 3];
			array4[0] = label;
			array4[1] = grid;
			array4[2] = this;
			object obj5;
			xamlServiceProvider4.Add(typeFromHandle7, obj5 = new SimpleValueTargetProvider(array4, VisualElement.StyleProperty, nameScope));
			xamlServiceProvider4.Add(typeof(IReferenceProvider), obj5);
			Type typeFromHandle8 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver4 = new XmlNamespaceResolver();
			xmlNamespaceResolver4.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver4.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver4.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver4.Add("flv", "clr-namespace:DLToolkit.Forms.Controls;assembly=DLToolkit.Forms.Controls.FlowListView");
			xmlNamespaceResolver4.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver4.Add("local", "clr-namespace:CarScannerXamarinForms");
			xamlServiceProvider4.Add(typeFromHandle8, new XamlTypeResolver(xmlNamespaceResolver4, typeof(ColorSelectorPage).GetTypeInfo().Assembly));
			xamlServiceProvider4.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(54, 17)));
			DynamicResource dynamicResource3 = markupExtension4.ProvideValue(xamlServiceProvider4);
			label.SetDynamicResource(VisualElement.StyleProperty, dynamicResource3.Key);
			translate2.Text = "ios_SelectColor";
			IMarkupExtension markupExtension5 = translate2;
			XamlServiceProvider xamlServiceProvider5 = new XamlServiceProvider();
			Type typeFromHandle9 = typeof(IProvideValueTarget);
			object[] array5 = new object[0 + 3];
			array5[0] = label;
			array5[1] = grid;
			array5[2] = this;
			object obj6;
			xamlServiceProvider5.Add(typeFromHandle9, obj6 = new SimpleValueTargetProvider(array5, Label.TextProperty, nameScope));
			xamlServiceProvider5.Add(typeof(IReferenceProvider), obj6);
			Type typeFromHandle10 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver5 = new XmlNamespaceResolver();
			xmlNamespaceResolver5.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver5.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver5.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver5.Add("flv", "clr-namespace:DLToolkit.Forms.Controls;assembly=DLToolkit.Forms.Controls.FlowListView");
			xmlNamespaceResolver5.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver5.Add("local", "clr-namespace:CarScannerXamarinForms");
			xamlServiceProvider5.Add(typeFromHandle10, new XamlTypeResolver(xmlNamespaceResolver5, typeof(ColorSelectorPage).GetTypeInfo().Assembly));
			xamlServiceProvider5.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(55, 17)));
			object obj7 = markupExtension5.ProvideValue(xamlServiceProvider5);
			label.Text = obj7;
			label.SetValue(View.VerticalOptionsProperty, LayoutOptions.Center);
			grid.Children.Add(label);
			this.SetValue(NavigationPage.TitleViewProperty, grid);
			grid2.SetValue(View.MarginProperty, new Thickness(5.0, 0.0, 5.0, 0.0));
			rowDefinition.SetValue(RowDefinition.HeightProperty, new GridLengthTypeConverter().ConvertFromInvariantString("auto"));
			grid2.GetValue(Grid.RowDefinitionsProperty).Add(rowDefinition);
			rowDefinition2.SetValue(RowDefinition.HeightProperty, new GridLengthTypeConverter().ConvertFromInvariantString("*"));
			grid2.GetValue(Grid.RowDefinitionsProperty).Add(rowDefinition2);
			columnDefinition4.SetValue(ColumnDefinition.WidthProperty, new GridLengthTypeConverter().ConvertFromInvariantString("*"));
			grid2.GetValue(Grid.ColumnDefinitionsProperty).Add(columnDefinition4);
			columnDefinition5.SetValue(ColumnDefinition.WidthProperty, new GridLengthTypeConverter().ConvertFromInvariantString("auto"));
			grid2.GetValue(Grid.ColumnDefinitionsProperty).Add(columnDefinition5);
			columnDefinition6.SetValue(ColumnDefinition.WidthProperty, new GridLengthTypeConverter().ConvertFromInvariantString("*"));
			grid2.GetValue(Grid.ColumnDefinitionsProperty).Add(columnDefinition6);
			flowListView.SetValue(Grid.RowProperty, 1);
			flowListView.SetValue(Grid.ColumnProperty, 0);
			flowListView.SetValue(Grid.ColumnSpanProperty, 3);
			flowListView.SetValue(FlowListView.FlowColumnCountProperty, new int?(5));
			flowListView.FlowItemTapped += this.lvColors_ItemTapped;
			bindingExtension.Path = "ItemTappedCommand";
			BindingBase bindingBase = bindingExtension.ProvideValue(null);
			flowListView.SetBinding(FlowListView.FlowItemTappedCommandProperty, bindingBase);
			bindingExtension2.Path = "Colors";
			BindingBase bindingBase2 = bindingExtension2.ProvideValue(null);
			flowListView.SetBinding(FlowListView.FlowItemsSourceProperty, bindingBase2);
			flowListView.SetValue(ListView.HasUnevenRowsProperty, false);
			flowListView.ItemTapped += this.lvColors_ItemTapped;
			flowListView.SetValue(ListView.SeparatorColorProperty, Color.Black);
			flowListView.SetValue(ListView.SeparatorVisibilityProperty, 1);
			IDataTemplate dataTemplate2 = dataTemplate;
			ColorSelectorPage.<InitializeComponent>_anonXamlCDataTemplate_98 <InitializeComponent>_anonXamlCDataTemplate_ = new ColorSelectorPage.<InitializeComponent>_anonXamlCDataTemplate_98();
			object[] array6 = new object[0 + 4];
			array6[0] = dataTemplate;
			array6[1] = flowListView;
			array6[2] = grid2;
			array6[3] = this;
			<InitializeComponent>_anonXamlCDataTemplate_.parentValues = array6;
			<InitializeComponent>_anonXamlCDataTemplate_.root = this;
			dataTemplate2.LoadTemplate = new Func<object>(<InitializeComponent>_anonXamlCDataTemplate_.LoadDataTemplate);
			flowListView.SetValue(FlowListView.FlowColumnTemplateProperty, dataTemplate);
			grid2.Children.Add(flowListView);
			this.SetValue(ContentPage.ContentProperty, grid2);
		}

		// Token: 0x06001623 RID: 5667 RVA: 0x0009D390 File Offset: 0x0009B590
		private void __InitComponentRuntime()
		{
			Extensions.LoadFromXaml<ColorSelectorPage>(this, typeof(ColorSelectorPage));
			this.lvColors = NameScopeExtensions.FindByName<FlowListView>(this, "lvColors");
		}

		// Token: 0x04000663 RID: 1635
		private object Item;

		// Token: 0x04000664 RID: 1636
		private string PropertyName;

		// Token: 0x04000665 RID: 1637
		[CompilerGenerated]
		private ICommand <ItemTappedCommand>k__BackingField;

		// Token: 0x04000666 RID: 1638
		private List<ColorItem> _Colors;

		// Token: 0x04000667 RID: 1639
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private FlowListView lvColors;

		// Token: 0x02000190 RID: 400
		[CompilerGenerated]
		[Serializable]
		private sealed class <>c
		{
			// Token: 0x06001624 RID: 5668 RVA: 0x0009D3B4 File Offset: 0x0009B5B4
			// Note: this type is marked as 'beforefieldinit'.
			static <>c()
			{
			}

			// Token: 0x06001625 RID: 5669 RVA: 0x00002050 File Offset: 0x00000250
			public <>c()
			{
			}

			// Token: 0x06001626 RID: 5670 RVA: 0x0009D3C0 File Offset: 0x0009B5C0
			internal int <CreateColors>b__8_0(ColorItem x, ColorItem y)
			{
				return ColorSelectorPage.CompareColors(x, y);
			}

			// Token: 0x04000668 RID: 1640
			public static readonly ColorSelectorPage.<>c <>9 = new ColorSelectorPage.<>c();

			// Token: 0x04000669 RID: 1641
			public static Comparison<ColorItem> <>9__8_0;
		}

		// Token: 0x02000191 RID: 401
		[CompilerGenerated]
		private sealed class <>c__DisplayClass6_0
		{
			// Token: 0x06001627 RID: 5671 RVA: 0x00002050 File Offset: 0x00000250
			public <>c__DisplayClass6_0()
			{
			}

			// Token: 0x06001628 RID: 5672 RVA: 0x0009D3CC File Offset: 0x0009B5CC
			internal void <.ctor>b__0(object param)
			{
				this.Item.GetType().GetProperty(this.PropertyName).SetValue(this.Item, (param as ColorItem).Color);
				this.<>4__this.Navigation.PopAsync(true);
			}

			// Token: 0x0400066A RID: 1642
			public object Item;

			// Token: 0x0400066B RID: 1643
			public string PropertyName;

			// Token: 0x0400066C RID: 1644
			public ColorSelectorPage <>4__this;
		}

		// Token: 0x02000192 RID: 402
		[CompilerGenerated]
		private sealed class <>c__DisplayClass6_1
		{
			// Token: 0x06001629 RID: 5673 RVA: 0x00002050 File Offset: 0x00000250
			public <>c__DisplayClass6_1()
			{
			}

			// Token: 0x0600162A RID: 5674 RVA: 0x0009D41C File Offset: 0x0009B61C
			internal bool <.ctor>b__1(ColorItem x)
			{
				return x.Color == this.c;
			}

			// Token: 0x0400066D RID: 1645
			public Color c;
		}

		// Token: 0x02000193 RID: 403
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <btnCancel_Clicked>d__16 : IAsyncStateMachine
		{
			// Token: 0x0600162B RID: 5675 RVA: 0x0009D430 File Offset: 0x0009B630
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				ColorSelectorPage colorSelectorPage = this;
				try
				{
					TaskAwaiter<Page> taskAwaiter;
					if (num != 0)
					{
						taskAwaiter = colorSelectorPage.Navigation.PopAsync(true).GetAwaiter();
						if (!taskAwaiter.IsCompleted)
						{
							num2 = 0;
							TaskAwaiter<Page> taskAwaiter2 = taskAwaiter;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<Page>, ColorSelectorPage.<btnCancel_Clicked>d__16>(ref taskAwaiter, ref this);
							return;
						}
					}
					else
					{
						TaskAwaiter<Page> taskAwaiter2;
						taskAwaiter = taskAwaiter2;
						taskAwaiter2 = default(TaskAwaiter<Page>);
						num2 = -1;
					}
					taskAwaiter.GetResult();
				}
				catch (Exception ex)
				{
					num2 = -2;
					this.<>t__builder.SetException(ex);
					return;
				}
				num2 = -2;
				this.<>t__builder.SetResult();
			}

			// Token: 0x0600162C RID: 5676 RVA: 0x0009D4EC File Offset: 0x0009B6EC
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x0400066E RID: 1646
			public int <>1__state;

			// Token: 0x0400066F RID: 1647
			public AsyncVoidMethodBuilder <>t__builder;

			// Token: 0x04000670 RID: 1648
			public ColorSelectorPage <>4__this;

			// Token: 0x04000671 RID: 1649
			private TaskAwaiter<Page> <>u__1;
		}

		// Token: 0x02000194 RID: 404
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <lvColors_ItemTapped>d__14 : IAsyncStateMachine
		{
			// Token: 0x0600162D RID: 5677 RVA: 0x0009D4FC File Offset: 0x0009B6FC
			void IAsyncStateMachine.MoveNext()
			{
				try
				{
				}
				catch (Exception ex)
				{
					this.<>1__state = -2;
					this.<>t__builder.SetException(ex);
					return;
				}
				this.<>1__state = -2;
				this.<>t__builder.SetResult();
			}

			// Token: 0x0600162E RID: 5678 RVA: 0x0009D548 File Offset: 0x0009B748
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x04000672 RID: 1650
			public int <>1__state;

			// Token: 0x04000673 RID: 1651
			public AsyncVoidMethodBuilder <>t__builder;
		}

		// Token: 0x02000195 RID: 405
		[CompilerGenerated]
		private sealed class <InitializeComponent>_anonXamlCDataTemplate_98
		{
			// Token: 0x0600162F RID: 5679 RVA: 0x0009D558 File Offset: 0x0009B758
			public <InitializeComponent>_anonXamlCDataTemplate_98()
			{
			}

			// Token: 0x06001630 RID: 5680 RVA: 0x0009D56C File Offset: 0x0009B76C
			internal object LoadDataTemplate()
			{
				DynamicResourceExtension dynamicResourceExtension;
				VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension = new DynamicResourceExtension(), new Uri("UserControls\\ColorSelectorPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 92, 29);
				DynamicResourceExtension dynamicResourceExtension2;
				VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension2 = new DynamicResourceExtension(), new Uri("UserControls\\ColorSelectorPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 95, 29);
				RowDefinition rowDefinition;
				VisualDiagnostics.RegisterSourceInfo(rowDefinition = new RowDefinition(), new Uri("UserControls\\ColorSelectorPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 98, 38);
				BindingExtension bindingExtension;
				VisualDiagnostics.RegisterSourceInfo(bindingExtension = new BindingExtension(), new Uri("UserControls\\ColorSelectorPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 106, 37);
				BoxView boxView;
				VisualDiagnostics.RegisterSourceInfo(boxView = new BoxView(), new Uri("UserControls\\ColorSelectorPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 100, 34);
				StaticResourceExtension staticResourceExtension;
				VisualDiagnostics.RegisterSourceInfo(staticResourceExtension = new StaticResourceExtension(), new Uri("UserControls\\ColorSelectorPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 112, 37);
				BindingExtension bindingExtension2;
				VisualDiagnostics.RegisterSourceInfo(bindingExtension2 = new BindingExtension(), new Uri("UserControls\\ColorSelectorPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 112, 37);
				StaticResourceExtension staticResourceExtension2;
				VisualDiagnostics.RegisterSourceInfo(staticResourceExtension2 = new StaticResourceExtension(), new Uri("UserControls\\ColorSelectorPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 113, 37);
				BindingExtension bindingExtension3;
				VisualDiagnostics.RegisterSourceInfo(bindingExtension3 = new BindingExtension(), new Uri("UserControls\\ColorSelectorPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 113, 37);
				Image image;
				VisualDiagnostics.RegisterSourceInfo(image = new Image(), new Uri("UserControls\\ColorSelectorPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 107, 34);
				Grid grid;
				VisualDiagnostics.RegisterSourceInfo(grid = new Grid(), new Uri("UserControls\\ColorSelectorPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 96, 30);
				Frame frame;
				VisualDiagnostics.RegisterSourceInfo(frame = new Frame(), new Uri("UserControls\\ColorSelectorPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 90, 26);
				NameScope nameScope = new NameScope();
				NameScope.SetNameScope(frame, nameScope);
				frame.SetValue(Layout.PaddingProperty, new Thickness(3.0));
				dynamicResourceExtension.Key = "TextColor";
				IMarkupExtension<DynamicResource> markupExtension = dynamicResourceExtension;
				XamlServiceProvider xamlServiceProvider = new XamlServiceProvider();
				Type typeFromHandle = typeof(IProvideValueTarget);
				int num;
				object[] array = new object[(num = this.parentValues.Length) + 1];
				Array.Copy(this.parentValues, 0, array, 1, num);
				object[] array2 = array;
				array2[0] = frame;
				object obj;
				xamlServiceProvider.Add(typeFromHandle, obj = new SimpleValueTargetProvider(array2, VisualElement.BackgroundColorProperty, nameScope));
				xamlServiceProvider.Add(typeof(IReferenceProvider), obj);
				Type typeFromHandle2 = typeof(IXamlTypeResolver);
				XmlNamespaceResolver xmlNamespaceResolver = new XmlNamespaceResolver();
				xmlNamespaceResolver.Add("", "http://xamarin.com/schemas/2014/forms");
				xmlNamespaceResolver.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
				xmlNamespaceResolver.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
				xmlNamespaceResolver.Add("flv", "clr-namespace:DLToolkit.Forms.Controls;assembly=DLToolkit.Forms.Controls.FlowListView");
				xmlNamespaceResolver.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
				xmlNamespaceResolver.Add("local", "clr-namespace:CarScannerXamarinForms");
				xamlServiceProvider.Add(typeFromHandle2, new XamlTypeResolver(xmlNamespaceResolver, typeof(ColorSelectorPage.<InitializeComponent>_anonXamlCDataTemplate_98).GetTypeInfo().Assembly));
				xamlServiceProvider.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(92, 29)));
				DynamicResource dynamicResource = markupExtension.ProvideValue(xamlServiceProvider);
				frame.SetDynamicResource(VisualElement.BackgroundColorProperty, dynamicResource.Key);
				frame.SetValue(Frame.CornerRadiusProperty, 0f);
				frame.SetValue(Frame.HasShadowProperty, false);
				dynamicResourceExtension2.Key = "TextColor";
				IMarkupExtension<DynamicResource> markupExtension2 = dynamicResourceExtension2;
				XamlServiceProvider xamlServiceProvider2 = new XamlServiceProvider();
				Type typeFromHandle3 = typeof(IProvideValueTarget);
				int num2;
				object[] array3 = new object[(num2 = this.parentValues.Length) + 1];
				Array.Copy(this.parentValues, 0, array3, 1, num2);
				object[] array4 = array3;
				array4[0] = frame;
				object obj2;
				xamlServiceProvider2.Add(typeFromHandle3, obj2 = new SimpleValueTargetProvider(array4, Frame.OutlineColorProperty, nameScope));
				xamlServiceProvider2.Add(typeof(IReferenceProvider), obj2);
				Type typeFromHandle4 = typeof(IXamlTypeResolver);
				XmlNamespaceResolver xmlNamespaceResolver2 = new XmlNamespaceResolver();
				xmlNamespaceResolver2.Add("", "http://xamarin.com/schemas/2014/forms");
				xmlNamespaceResolver2.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
				xmlNamespaceResolver2.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
				xmlNamespaceResolver2.Add("flv", "clr-namespace:DLToolkit.Forms.Controls;assembly=DLToolkit.Forms.Controls.FlowListView");
				xmlNamespaceResolver2.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
				xmlNamespaceResolver2.Add("local", "clr-namespace:CarScannerXamarinForms");
				xamlServiceProvider2.Add(typeFromHandle4, new XamlTypeResolver(xmlNamespaceResolver2, typeof(ColorSelectorPage.<InitializeComponent>_anonXamlCDataTemplate_98).GetTypeInfo().Assembly));
				xamlServiceProvider2.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(95, 29)));
				DynamicResource dynamicResource2 = markupExtension2.ProvideValue(xamlServiceProvider2);
				frame.SetDynamicResource(Frame.OutlineColorProperty, dynamicResource2.Key);
				rowDefinition.SetValue(RowDefinition.HeightProperty, new GridLengthTypeConverter().ConvertFromInvariantString("auto"));
				grid.GetValue(Grid.RowDefinitionsProperty).Add(rowDefinition);
				boxView.SetValue(Grid.RowProperty, 0);
				boxView.SetValue(View.MarginProperty, new Thickness(0.0));
				boxView.SetValue(VisualElement.HeightRequestProperty, 50.0);
				boxView.SetValue(View.HorizontalOptionsProperty, LayoutOptions.Fill);
				boxView.SetValue(View.VerticalOptionsProperty, LayoutOptions.Fill);
				bindingExtension.Path = "Color";
				BindingBase bindingBase = bindingExtension.ProvideValue(null);
				boxView.SetBinding(BoxView.ColorProperty, bindingBase);
				grid.Children.Add(boxView);
				image.SetValue(Grid.RowProperty, 0);
				image.SetValue(Image.AspectProperty, 2);
				image.SetValue(VisualElement.HeightRequestProperty, 50.0);
				image.SetValue(VisualElement.InputTransparentProperty, true);
				staticResourceExtension.Key = "TranparentColorToTrueConverter";
				IMarkupExtension markupExtension3 = staticResourceExtension;
				XamlServiceProvider xamlServiceProvider3 = new XamlServiceProvider();
				Type typeFromHandle5 = typeof(IProvideValueTarget);
				int num3;
				object[] array5 = new object[(num3 = this.parentValues.Length) + 4];
				Array.Copy(this.parentValues, 0, array5, 4, num3);
				object[] array6 = array5;
				array6[0] = bindingExtension2;
				array6[1] = image;
				array6[2] = grid;
				array6[3] = frame;
				object obj3;
				xamlServiceProvider3.Add(typeFromHandle5, obj3 = new SimpleValueTargetProvider(array6, typeof(BindingExtension).GetRuntimeProperty("Converter"), nameScope));
				xamlServiceProvider3.Add(typeof(IReferenceProvider), obj3);
				Type typeFromHandle6 = typeof(IXamlTypeResolver);
				XmlNamespaceResolver xmlNamespaceResolver3 = new XmlNamespaceResolver();
				xmlNamespaceResolver3.Add("", "http://xamarin.com/schemas/2014/forms");
				xmlNamespaceResolver3.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
				xmlNamespaceResolver3.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
				xmlNamespaceResolver3.Add("flv", "clr-namespace:DLToolkit.Forms.Controls;assembly=DLToolkit.Forms.Controls.FlowListView");
				xmlNamespaceResolver3.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
				xmlNamespaceResolver3.Add("local", "clr-namespace:CarScannerXamarinForms");
				xamlServiceProvider3.Add(typeFromHandle6, new XamlTypeResolver(xmlNamespaceResolver3, typeof(ColorSelectorPage.<InitializeComponent>_anonXamlCDataTemplate_98).GetTypeInfo().Assembly));
				xamlServiceProvider3.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(112, 37)));
				object obj4 = markupExtension3.ProvideValue(xamlServiceProvider3);
				bindingExtension2.Converter = obj4;
				bindingExtension2.Path = "Color";
				BindingBase bindingBase2 = bindingExtension2.ProvideValue(null);
				image.SetBinding(VisualElement.IsVisibleProperty, bindingBase2);
				staticResourceExtension2.Key = "TransparentColorToTransparentImagePathConverter";
				IMarkupExtension markupExtension4 = staticResourceExtension2;
				XamlServiceProvider xamlServiceProvider4 = new XamlServiceProvider();
				Type typeFromHandle7 = typeof(IProvideValueTarget);
				int num4;
				object[] array7 = new object[(num4 = this.parentValues.Length) + 4];
				Array.Copy(this.parentValues, 0, array7, 4, num4);
				object[] array8 = array7;
				array8[0] = bindingExtension3;
				array8[1] = image;
				array8[2] = grid;
				array8[3] = frame;
				object obj5;
				xamlServiceProvider4.Add(typeFromHandle7, obj5 = new SimpleValueTargetProvider(array8, typeof(BindingExtension).GetRuntimeProperty("Converter"), nameScope));
				xamlServiceProvider4.Add(typeof(IReferenceProvider), obj5);
				Type typeFromHandle8 = typeof(IXamlTypeResolver);
				XmlNamespaceResolver xmlNamespaceResolver4 = new XmlNamespaceResolver();
				xmlNamespaceResolver4.Add("", "http://xamarin.com/schemas/2014/forms");
				xmlNamespaceResolver4.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
				xmlNamespaceResolver4.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
				xmlNamespaceResolver4.Add("flv", "clr-namespace:DLToolkit.Forms.Controls;assembly=DLToolkit.Forms.Controls.FlowListView");
				xmlNamespaceResolver4.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
				xmlNamespaceResolver4.Add("local", "clr-namespace:CarScannerXamarinForms");
				xamlServiceProvider4.Add(typeFromHandle8, new XamlTypeResolver(xmlNamespaceResolver4, typeof(ColorSelectorPage.<InitializeComponent>_anonXamlCDataTemplate_98).GetTypeInfo().Assembly));
				xamlServiceProvider4.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(113, 37)));
				object obj6 = markupExtension4.ProvideValue(xamlServiceProvider4);
				bindingExtension3.Converter = obj6;
				bindingExtension3.Path = "Color";
				BindingBase bindingBase3 = bindingExtension3.ProvideValue(null);
				image.SetBinding(Image.SourceProperty, bindingBase3);
				grid.Children.Add(image);
				frame.SetValue(ContentView.ContentProperty, grid);
				return frame;
			}

			// Token: 0x04000674 RID: 1652
			internal object[] parentValues;

			// Token: 0x04000675 RID: 1653
			internal ColorSelectorPage root;
		}
	}
}
