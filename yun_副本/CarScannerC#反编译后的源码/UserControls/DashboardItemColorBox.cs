using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Threading;
using Xamarin.Forms;
using Xamarin.Forms.Internals;
using Xamarin.Forms.Xaml;
using Xamarin.Forms.Xaml.Diagnostics;
using Xamarin.Forms.Xaml.Internals;

namespace CarScannerXamarinForms.UserControls
{
	// Token: 0x020005CA RID: 1482
	[XamlCompilation(2)]
	[XamlFilePath("UserControls\\DashboardItemColorBox.xaml")]
	public class DashboardItemColorBox : ContentView
	{
		// Token: 0x0600354E RID: 13646 RVA: 0x00263B2A File Offset: 0x00261D2A
		public DashboardItemColorBox()
		{
			this.InitializeComponent();
			this.label.Text = this.Text;
			this.boxView.Color = this.Color;
		}

		// Token: 0x14000031 RID: 49
		// (add) Token: 0x0600354F RID: 13647 RVA: 0x00263B5C File Offset: 0x00261D5C
		// (remove) Token: 0x06003550 RID: 13648 RVA: 0x00263B94 File Offset: 0x00261D94
		public event EventHandler Tapped
		{
			[CompilerGenerated]
			add
			{
				EventHandler eventHandler = this.Tapped;
				EventHandler eventHandler2;
				do
				{
					eventHandler2 = eventHandler;
					EventHandler eventHandler3 = (EventHandler)Delegate.Combine(eventHandler2, value);
					eventHandler = Interlocked.CompareExchange<EventHandler>(ref this.Tapped, eventHandler3, eventHandler2);
				}
				while (eventHandler != eventHandler2);
			}
			[CompilerGenerated]
			remove
			{
				EventHandler eventHandler = this.Tapped;
				EventHandler eventHandler2;
				do
				{
					eventHandler2 = eventHandler;
					EventHandler eventHandler3 = (EventHandler)Delegate.Remove(eventHandler2, value);
					eventHandler = Interlocked.CompareExchange<EventHandler>(ref this.Tapped, eventHandler3, eventHandler2);
				}
				while (eventHandler != eventHandler2);
			}
		}

		// Token: 0x06003551 RID: 13649 RVA: 0x00263BC9 File Offset: 0x00261DC9
		private void TapGestureRecognizer_Tapped(object sender, EventArgs e)
		{
			EventHandler tapped = this.Tapped;
			if (tapped == null)
			{
				return;
			}
			tapped(this, e);
		}

		// Token: 0x06003552 RID: 13650 RVA: 0x00263BDD File Offset: 0x00261DDD
		private static void TextPropertyChanged(BindableObject bindable, object oldValue, object newValue)
		{
			((DashboardItemColorBox)bindable).label.Text = (string)newValue;
		}

		// Token: 0x1700135D RID: 4957
		// (get) Token: 0x06003553 RID: 13651 RVA: 0x00263BF5 File Offset: 0x00261DF5
		// (set) Token: 0x06003554 RID: 13652 RVA: 0x00263C07 File Offset: 0x00261E07
		public string Text
		{
			get
			{
				return (string)base.GetValue(DashboardItemColorBox.TextProperty);
			}
			set
			{
				base.SetValue(DashboardItemColorBox.TextProperty, value);
			}
		}

		// Token: 0x06003555 RID: 13653 RVA: 0x00263C18 File Offset: 0x00261E18
		private static void ColorPropertyChanged(BindableObject bindable, object oldValue, object newValue)
		{
			DashboardItemColorBox dashboardItemColorBox = (DashboardItemColorBox)bindable;
			Color color = (Color)newValue;
			dashboardItemColorBox.boxView.Color = color;
			if (color == Color.Transparent)
			{
				dashboardItemColorBox.imgTransparent.IsVisible = true;
				if (dashboardItemColorBox.imgTransparent.Source == null)
				{
					dashboardItemColorBox.imgTransparent.Source = ImageSource.FromFile("transparent.png");
					return;
				}
			}
			else
			{
				dashboardItemColorBox.imgTransparent.IsVisible = false;
			}
		}

		// Token: 0x1700135E RID: 4958
		// (get) Token: 0x06003556 RID: 13654 RVA: 0x00263C87 File Offset: 0x00261E87
		// (set) Token: 0x06003557 RID: 13655 RVA: 0x00263C99 File Offset: 0x00261E99
		public Color Color
		{
			get
			{
				return (Color)base.GetValue(DashboardItemColorBox.ColorProperty);
			}
			set
			{
				base.SetValue(DashboardItemColorBox.ColorProperty, value);
			}
		}

		// Token: 0x06003558 RID: 13656 RVA: 0x00263CAC File Offset: 0x00261EAC
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private void InitializeComponent()
		{
			if (ResourceLoader.IsEnabled && ResourceLoader.CanProvideContentFor(new ResourceLoader.ResourceLoadingQuery
			{
				AssemblyName = typeof(DashboardItemColorBox).GetTypeInfo().Assembly.GetName(),
				ResourcePath = "UserControls/DashboardItemColorBox.xaml",
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
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension = new DynamicResourceExtension(), new Uri("UserControls\\DashboardItemColorBox.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 13, 52);
			Setter setter;
			VisualDiagnostics.RegisterSourceInfo(setter = new Setter(), new Uri("UserControls\\DashboardItemColorBox.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 13, 18);
			Setter setter2;
			VisualDiagnostics.RegisterSourceInfo(setter2 = new Setter(), new Uri("UserControls\\DashboardItemColorBox.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 14, 18);
			Setter setter3;
			VisualDiagnostics.RegisterSourceInfo(setter3 = new Setter(), new Uri("UserControls\\DashboardItemColorBox.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 15, 18);
			Style style;
			VisualDiagnostics.RegisterSourceInfo(style = new Style(typeof(Frame)), new Uri("UserControls\\DashboardItemColorBox.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 12, 14);
			DynamicResourceExtension dynamicResourceExtension2;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension2 = new DynamicResourceExtension(), new Uri("UserControls\\DashboardItemColorBox.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 19, 49);
			Setter setter4;
			VisualDiagnostics.RegisterSourceInfo(setter4 = new Setter(), new Uri("UserControls\\DashboardItemColorBox.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 19, 18);
			Setter setter5;
			VisualDiagnostics.RegisterSourceInfo(setter5 = new Setter(), new Uri("UserControls\\DashboardItemColorBox.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 20, 18);
			Setter setter6;
			VisualDiagnostics.RegisterSourceInfo(setter6 = new Setter(), new Uri("UserControls\\DashboardItemColorBox.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 21, 18);
			Setter setter7;
			VisualDiagnostics.RegisterSourceInfo(setter7 = new Setter(), new Uri("UserControls\\DashboardItemColorBox.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 22, 18);
			Style style2;
			VisualDiagnostics.RegisterSourceInfo(style2 = new Style(typeof(Frame)), new Uri("UserControls\\DashboardItemColorBox.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 18, 14);
			ResourceDictionary resourceDictionary;
			VisualDiagnostics.RegisterSourceInfo(resourceDictionary = new ResourceDictionary(), new Uri("UserControls\\DashboardItemColorBox.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 11, 10);
			ColumnDefinition columnDefinition;
			VisualDiagnostics.RegisterSourceInfo(columnDefinition = new ColumnDefinition(), new Uri("UserControls\\DashboardItemColorBox.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 30, 18);
			ColumnDefinition columnDefinition2;
			VisualDiagnostics.RegisterSourceInfo(columnDefinition2 = new ColumnDefinition(), new Uri("UserControls\\DashboardItemColorBox.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 31, 18);
			Label label;
			VisualDiagnostics.RegisterSourceInfo(label = new Label(), new Uri("UserControls\\DashboardItemColorBox.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 33, 14);
			DynamicResourceExtension dynamicResourceExtension3;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension3 = new DynamicResourceExtension(), new Uri("UserControls\\DashboardItemColorBox.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 38, 36);
			On on;
			VisualDiagnostics.RegisterSourceInfo(on = new On(), new Uri("UserControls\\DashboardItemColorBox.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 41, 26);
			On on2;
			VisualDiagnostics.RegisterSourceInfo(on2 = new On(), new Uri("UserControls\\DashboardItemColorBox.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 42, 26);
			OnPlatform<bool> onPlatform;
			VisualDiagnostics.RegisterSourceInfo(onPlatform = new OnPlatform<bool>(), new Uri("UserControls\\DashboardItemColorBox.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 40, 22);
			RowDefinition rowDefinition;
			VisualDiagnostics.RegisterSourceInfo(rowDefinition = new RowDefinition(), new Uri("UserControls\\DashboardItemColorBox.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 47, 26);
			TapGestureRecognizer tapGestureRecognizer;
			VisualDiagnostics.RegisterSourceInfo(tapGestureRecognizer = new TapGestureRecognizer(), new Uri("UserControls\\DashboardItemColorBox.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 55, 30);
			BoxView boxView;
			VisualDiagnostics.RegisterSourceInfo(boxView = new BoxView(), new Uri("UserControls\\DashboardItemColorBox.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 49, 22);
			Image image;
			VisualDiagnostics.RegisterSourceInfo(image = new Image(), new Uri("UserControls\\DashboardItemColorBox.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 58, 22);
			Grid grid;
			VisualDiagnostics.RegisterSourceInfo(grid = new Grid(), new Uri("UserControls\\DashboardItemColorBox.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 45, 18);
			Frame frame;
			VisualDiagnostics.RegisterSourceInfo(frame = new Frame(), new Uri("UserControls\\DashboardItemColorBox.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 38, 14);
			Grid grid2;
			VisualDiagnostics.RegisterSourceInfo(grid2 = new Grid(), new Uri("UserControls\\DashboardItemColorBox.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 28, 10);
			VisualDiagnostics.RegisterSourceInfo(this, new Uri("UserControls\\DashboardItemColorBox.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 2, 2);
			NameScope nameScope = NameScope.GetNameScope(this) ?? new NameScope();
			NameScope.SetNameScope(this, nameScope);
			NameScope nameScope2 = new NameScope();
			NameScope nameScope3 = new NameScope();
			NameScope nameScope4 = new NameScope();
			NameScope nameScope5 = new NameScope();
			NameScope nameScope6 = new NameScope();
			NameScope nameScope7 = new NameScope();
			NameScope nameScope8 = new NameScope();
			nameScope.RegisterName("label", label);
			if (label.StyleId == null)
			{
				label.StyleId = "label";
			}
			nameScope.RegisterName("boxView", boxView);
			if (boxView.StyleId == null)
			{
				boxView.StyleId = "boxView";
			}
			nameScope.RegisterName("imgTransparent", image);
			if (image.StyleId == null)
			{
				image.StyleId = "imgTransparent";
			}
			this.label = label;
			this.boxView = boxView;
			this.imgTransparent = image;
			this.Resources = resourceDictionary;
			setter.Property = VisualElement.BackgroundColorProperty;
			dynamicResourceExtension.Key = "BackgroundColor";
			IMarkupExtension<DynamicResource> markupExtension = dynamicResourceExtension;
			XamlServiceProvider xamlServiceProvider = new XamlServiceProvider();
			Type typeFromHandle = typeof(IProvideValueTarget);
			object[] array = new object[0 + 4];
			array[0] = setter;
			array[1] = style;
			array[2] = resourceDictionary;
			array[3] = this;
			object obj;
			xamlServiceProvider.Add(typeFromHandle, obj = new SimpleValueTargetProvider(array, typeof(Setter).GetRuntimeProperty("Value"), nameScope2));
			xamlServiceProvider.Add(typeof(IReferenceProvider), obj);
			Type typeFromHandle2 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver = new XmlNamespaceResolver();
			xmlNamespaceResolver.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver.Add("d", "http://xamarin.com/schemas/2014/forms/design");
			xmlNamespaceResolver.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver.Add("mc", "http://schemas.openxmlformats.org/markup-compatibility/2006");
			xamlServiceProvider.Add(typeFromHandle2, new XamlTypeResolver(xmlNamespaceResolver, typeof(DashboardItemColorBox).GetTypeInfo().Assembly));
			xamlServiceProvider.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(13, 52)));
			DynamicResource dynamicResource = markupExtension.ProvideValue(xamlServiceProvider);
			setter.Value = dynamicResource;
			style.Setters.Add(setter);
			setter2.Property = View.MarginProperty;
			setter2.Value = "5,10";
			setter2.Value = new Thickness(5.0, 10.0);
			style.Setters.Add(setter2);
			setter3.Property = Layout.PaddingProperty;
			setter3.Value = "5,10";
			setter3.Value = new Thickness(5.0, 10.0);
			style.Setters.Add(setter3);
			resourceDictionary.Add(style);
			setter4.Property = Frame.OutlineColorProperty;
			dynamicResourceExtension2.Key = "TextColor";
			IMarkupExtension<DynamicResource> markupExtension2 = dynamicResourceExtension2;
			XamlServiceProvider xamlServiceProvider2 = new XamlServiceProvider();
			Type typeFromHandle3 = typeof(IProvideValueTarget);
			object[] array2 = new object[0 + 4];
			array2[0] = setter4;
			array2[1] = style2;
			array2[2] = resourceDictionary;
			array2[3] = this;
			object obj2;
			xamlServiceProvider2.Add(typeFromHandle3, obj2 = new SimpleValueTargetProvider(array2, typeof(Setter).GetRuntimeProperty("Value"), nameScope5));
			xamlServiceProvider2.Add(typeof(IReferenceProvider), obj2);
			Type typeFromHandle4 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver2 = new XmlNamespaceResolver();
			xmlNamespaceResolver2.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver2.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver2.Add("d", "http://xamarin.com/schemas/2014/forms/design");
			xmlNamespaceResolver2.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver2.Add("mc", "http://schemas.openxmlformats.org/markup-compatibility/2006");
			xamlServiceProvider2.Add(typeFromHandle4, new XamlTypeResolver(xmlNamespaceResolver2, typeof(DashboardItemColorBox).GetTypeInfo().Assembly));
			xamlServiceProvider2.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(19, 49)));
			DynamicResource dynamicResource2 = markupExtension2.ProvideValue(xamlServiceProvider2);
			setter4.Value = dynamicResource2;
			style2.Setters.Add(setter4);
			setter5.Property = Layout.PaddingProperty;
			setter5.Value = "2";
			setter5.Value = new Thickness(2.0);
			style2.Setters.Add(setter5);
			setter6.Property = View.MarginProperty;
			setter6.Value = "0";
			setter6.Value = new Thickness(0.0);
			style2.Setters.Add(setter6);
			setter7.Property = VisualElement.BackgroundColorProperty;
			setter7.Value = "Transparent";
			setter7.Value = Color.Transparent;
			style2.Setters.Add(setter7);
			resourceDictionary.Add("ColorPickerFrame", style2);
			this.Resources = resourceDictionary;
			columnDefinition.SetValue(ColumnDefinition.WidthProperty, new GridLengthTypeConverter().ConvertFromInvariantString("0.8*"));
			grid2.GetValue(Grid.ColumnDefinitionsProperty).Add(columnDefinition);
			columnDefinition2.SetValue(ColumnDefinition.WidthProperty, new GridLengthTypeConverter().ConvertFromInvariantString("0.2*"));
			grid2.GetValue(Grid.ColumnDefinitionsProperty).Add(columnDefinition2);
			label.SetValue(Grid.ColumnProperty, 0);
			label.SetValue(View.VerticalOptionsProperty, LayoutOptions.Center);
			label.SetValue(Label.VerticalTextAlignmentProperty, new TextAlignmentConverter().ConvertFromInvariantString("Center"));
			grid2.Children.Add(label);
			frame.SetValue(Grid.ColumnProperty, 1);
			dynamicResourceExtension3.Key = "ColorPickerFrame";
			IMarkupExtension<DynamicResource> markupExtension3 = dynamicResourceExtension3;
			XamlServiceProvider xamlServiceProvider3 = new XamlServiceProvider();
			Type typeFromHandle5 = typeof(IProvideValueTarget);
			object[] array3 = new object[0 + 3];
			array3[0] = frame;
			array3[1] = grid2;
			array3[2] = this;
			object obj3;
			xamlServiceProvider3.Add(typeFromHandle5, obj3 = new SimpleValueTargetProvider(array3, VisualElement.StyleProperty, nameScope));
			xamlServiceProvider3.Add(typeof(IReferenceProvider), obj3);
			Type typeFromHandle6 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver3 = new XmlNamespaceResolver();
			xmlNamespaceResolver3.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver3.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver3.Add("d", "http://xamarin.com/schemas/2014/forms/design");
			xmlNamespaceResolver3.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver3.Add("mc", "http://schemas.openxmlformats.org/markup-compatibility/2006");
			xamlServiceProvider3.Add(typeFromHandle6, new XamlTypeResolver(xmlNamespaceResolver3, typeof(DashboardItemColorBox).GetTypeInfo().Assembly));
			xamlServiceProvider3.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(38, 36)));
			DynamicResource dynamicResource3 = markupExtension3.ProvideValue(xamlServiceProvider3);
			frame.SetDynamicResource(VisualElement.StyleProperty, dynamicResource3.Key);
			on.Platform = new List<string>(1) { "iOS" };
			on.Value = "false";
			onPlatform.Platforms.Add(on);
			on2.Platform = new List<string>(1) { "Android" };
			on2.Value = "true";
			onPlatform.Platforms.Add(on2);
			frame.SetValue(Frame.HasShadowProperty, onPlatform);
			rowDefinition.SetValue(RowDefinition.HeightProperty, new GridLengthTypeConverter().ConvertFromInvariantString("auto"));
			grid.GetValue(Grid.RowDefinitionsProperty).Add(rowDefinition);
			boxView.SetValue(Grid.RowProperty, 0);
			boxView.SetValue(VisualElement.HeightRequestProperty, 40.0);
			boxView.SetValue(View.VerticalOptionsProperty, LayoutOptions.Start);
			tapGestureRecognizer.Tapped += this.TapGestureRecognizer_Tapped;
			boxView.GestureRecognizers.Add(tapGestureRecognizer);
			grid.Children.Add(boxView);
			image.SetValue(Grid.RowProperty, 0);
			image.SetValue(Image.AspectProperty, 2);
			image.SetValue(VisualElement.HeightRequestProperty, 40.0);
			image.SetValue(VisualElement.InputTransparentProperty, true);
			image.SetValue(VisualElement.IsVisibleProperty, new VisualElement.VisibilityConverter().ConvertFromInvariantString("False"));
			grid.Children.Add(image);
			frame.SetValue(ContentView.ContentProperty, grid);
			grid2.Children.Add(frame);
			this.SetValue(ContentView.ContentProperty, grid2);
		}

		// Token: 0x06003559 RID: 13657 RVA: 0x002649FC File Offset: 0x00262BFC
		// Note: this type is marked as 'beforefieldinit'.
		static DashboardItemColorBox()
		{
		}

		// Token: 0x0600355A RID: 13658 RVA: 0x00264A74 File Offset: 0x00262C74
		private void __InitComponentRuntime()
		{
			Extensions.LoadFromXaml<DashboardItemColorBox>(this, typeof(DashboardItemColorBox));
			this.label = NameScopeExtensions.FindByName<Label>(this, "label");
			this.boxView = NameScopeExtensions.FindByName<BoxView>(this, "boxView");
			this.imgTransparent = NameScopeExtensions.FindByName<Image>(this, "imgTransparent");
		}

		// Token: 0x04001FBC RID: 8124
		[CompilerGenerated]
		private EventHandler Tapped;

		// Token: 0x04001FBD RID: 8125
		public static readonly BindableProperty TextProperty = BindableProperty.Create("Text", typeof(string), typeof(DashboardItemColorBox), null, 2, null, new BindableProperty.BindingPropertyChangedDelegate(DashboardItemColorBox.TextPropertyChanged), null, null, null);

		// Token: 0x04001FBE RID: 8126
		public static readonly BindableProperty ColorProperty = BindableProperty.Create("Color", typeof(Color), typeof(DashboardItemColorBox), null, 2, null, new BindableProperty.BindingPropertyChangedDelegate(DashboardItemColorBox.ColorPropertyChanged), null, null, null);

		// Token: 0x04001FBF RID: 8127
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private Label label;

		// Token: 0x04001FC0 RID: 8128
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private BoxView boxView;

		// Token: 0x04001FC1 RID: 8129
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private Image imgTransparent;
	}
}
