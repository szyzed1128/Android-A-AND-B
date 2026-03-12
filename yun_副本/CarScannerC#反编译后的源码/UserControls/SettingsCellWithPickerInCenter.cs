using System;
using System.CodeDom.Compiler;
using System.Collections;
using System.Reflection;
using AiForms.Renderers;
using Xamarin.Forms;
using Xamarin.Forms.Internals;
using Xamarin.Forms.Xaml;
using Xamarin.Forms.Xaml.Diagnostics;
using Xamarin.Forms.Xaml.Internals;

namespace CarScannerXamarinForms.UserControls
{
	// Token: 0x020005DF RID: 1503
	[XamlCompilation(2)]
	[XamlFilePath("UserControls\\SettingsCellWithPickerInCenter.xaml")]
	public class SettingsCellWithPickerInCenter : CustomCell
	{
		// Token: 0x060035CC RID: 13772 RVA: 0x0026A19D File Offset: 0x0026839D
		public SettingsCellWithPickerInCenter()
		{
			this.InitializeComponent();
			this.picker.BindingContext = this;
			this.label.BindingContext = this;
			base.BindingContextChanged += this.SettingsCellWithPickerInCenter_BindingContextChanged;
		}

		// Token: 0x060035CD RID: 13773 RVA: 0x000027D4 File Offset: 0x000009D4
		private void SettingsCellWithPickerInCenter_BindingContextChanged(object sender, EventArgs e)
		{
		}

		// Token: 0x060035CE RID: 13774 RVA: 0x0026A1D5 File Offset: 0x002683D5
		private void SettingsCellWithPicker_Tapped(object sender, EventArgs e)
		{
			this.picker.Focus();
		}

		// Token: 0x1700136F RID: 4975
		// (get) Token: 0x060035CF RID: 13775 RVA: 0x0026A1E3 File Offset: 0x002683E3
		// (set) Token: 0x060035D0 RID: 13776 RVA: 0x0026A1F0 File Offset: 0x002683F0
		public object SelectedItem
		{
			get
			{
				return base.GetValue(SettingsCellWithPickerInCenter.SelectedItemProperty);
			}
			set
			{
				if (this.ItemsSource != null)
				{
					base.SetValue(SettingsCellWithPickerInCenter.SelectedItemProperty, value);
				}
			}
		}

		// Token: 0x17001370 RID: 4976
		// (get) Token: 0x060035D1 RID: 13777 RVA: 0x0026A206 File Offset: 0x00268406
		// (set) Token: 0x060035D2 RID: 13778 RVA: 0x0026A218 File Offset: 0x00268418
		public int SelectedIndex
		{
			get
			{
				return (int)base.GetValue(SettingsCellWithPickerInCenter.SelectedIndexProperty);
			}
			set
			{
				if (this.ItemsSource != null)
				{
					base.SetValue(SettingsCellWithPickerInCenter.SelectedIndexProperty, value);
				}
			}
		}

		// Token: 0x17001371 RID: 4977
		// (get) Token: 0x060035D3 RID: 13779 RVA: 0x0026A233 File Offset: 0x00268433
		// (set) Token: 0x060035D4 RID: 13780 RVA: 0x0026A248 File Offset: 0x00268448
		public IList ItemsSource
		{
			get
			{
				return (IList)base.GetValue(SettingsCellWithPickerInCenter.ItemsSourceProperty);
			}
			set
			{
				if (value != null)
				{
					IList itemsSource = this.ItemsSource;
				}
				base.SetValue(SettingsCellWithPickerInCenter.ItemsSourceProperty, value);
				this.SelectedIndex = this.SelectedIndex;
				object selectedItem = this.SelectedItem;
				if (selectedItem != null)
				{
					this.SelectedItem = selectedItem;
				}
			}
		}

		// Token: 0x060035D5 RID: 13781 RVA: 0x0026A288 File Offset: 0x00268488
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private void InitializeComponent()
		{
			if (ResourceLoader.IsEnabled && ResourceLoader.CanProvideContentFor(new ResourceLoader.ResourceLoadingQuery
			{
				AssemblyName = typeof(SettingsCellWithPickerInCenter).GetTypeInfo().Assembly.GetName(),
				ResourcePath = "UserControls/SettingsCellWithPickerInCenter.xaml",
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
			VisualDiagnostics.RegisterSourceInfo(bindingExtension = new BindingExtension(), new Uri("UserControls\\SettingsCellWithPickerInCenter.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 17, 17);
			BindingExtension bindingExtension2;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension2 = new BindingExtension(), new Uri("UserControls\\SettingsCellWithPickerInCenter.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 18, 17);
			BindingExtension bindingExtension3;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension3 = new BindingExtension(), new Uri("UserControls\\SettingsCellWithPickerInCenter.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 19, 17);
			Picker picker;
			VisualDiagnostics.RegisterSourceInfo(picker = new Picker(), new Uri("UserControls\\SettingsCellWithPickerInCenter.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 13, 14);
			StaticResourceExtension staticResourceExtension;
			VisualDiagnostics.RegisterSourceInfo(staticResourceExtension = new StaticResourceExtension(), new Uri("UserControls\\SettingsCellWithPickerInCenter.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 24, 17);
			BindingExtension bindingExtension4;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension4 = new BindingExtension(), new Uri("UserControls\\SettingsCellWithPickerInCenter.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 26, 17);
			DynamicResourceExtension dynamicResourceExtension;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension = new DynamicResourceExtension(), new Uri("UserControls\\SettingsCellWithPickerInCenter.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 27, 17);
			Label label;
			VisualDiagnostics.RegisterSourceInfo(label = new Label(), new Uri("UserControls\\SettingsCellWithPickerInCenter.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 22, 14);
			Grid grid;
			VisualDiagnostics.RegisterSourceInfo(grid = new Grid(), new Uri("UserControls\\SettingsCellWithPickerInCenter.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 10, 10);
			VisualDiagnostics.RegisterSourceInfo(this, new Uri("UserControls\\SettingsCellWithPickerInCenter.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 2, 2);
			NameScope nameScope = NameScope.GetNameScope(this) ?? new NameScope();
			NameScope.SetNameScope(this, nameScope);
			nameScope.RegisterName("me", this);
			if (this.StyleId == null)
			{
				this.StyleId = "me";
			}
			nameScope.RegisterName("picker", picker);
			if (picker.StyleId == null)
			{
				picker.StyleId = "picker";
			}
			nameScope.RegisterName("label", label);
			if (label.StyleId == null)
			{
				label.StyleId = "label";
			}
			this.me = this;
			this.picker = picker;
			this.label = label;
			this.Tapped += this.SettingsCellWithPicker_Tapped;
			picker.SetValue(View.HorizontalOptionsProperty, LayoutOptions.CenterAndExpand);
			picker.SetValue(VisualElement.IsVisibleProperty, new VisualElement.VisibilityConverter().ConvertFromInvariantString("False"));
			bindingExtension.Mode = 2;
			bindingExtension.Path = "ItemsSource";
			BindingBase bindingBase = bindingExtension.ProvideValue(null);
			picker.SetBinding(Picker.ItemsSourceProperty, bindingBase);
			bindingExtension2.Mode = 1;
			bindingExtension2.Path = "SelectedIndex";
			BindingBase bindingBase2 = bindingExtension2.ProvideValue(null);
			picker.SetBinding(Picker.SelectedIndexProperty, bindingBase2);
			bindingExtension3.Mode = 1;
			bindingExtension3.Path = "SelectedItem";
			BindingBase bindingBase3 = bindingExtension3.ProvideValue(null);
			picker.SetBinding(Picker.SelectedItemProperty, bindingBase3);
			picker.SetValue(View.VerticalOptionsProperty, LayoutOptions.Center);
			grid.Children.Add(picker);
			staticResourceExtension.Key = "BaseFontSize++";
			IMarkupExtension markupExtension = staticResourceExtension;
			XamlServiceProvider xamlServiceProvider = new XamlServiceProvider();
			Type typeFromHandle = typeof(IProvideValueTarget);
			object[] array = new object[0 + 3];
			array[0] = label;
			array[1] = grid;
			array[2] = this;
			object obj;
			xamlServiceProvider.Add(typeFromHandle, obj = new SimpleValueTargetProvider(array, Label.FontSizeProperty, nameScope));
			xamlServiceProvider.Add(typeof(IReferenceProvider), obj);
			Type typeFromHandle2 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver = new XmlNamespaceResolver();
			xmlNamespaceResolver.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xamlServiceProvider.Add(typeFromHandle2, new XamlTypeResolver(xmlNamespaceResolver, typeof(SettingsCellWithPickerInCenter).GetTypeInfo().Assembly));
			xamlServiceProvider.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(24, 17)));
			object obj2 = markupExtension.ProvideValue(xamlServiceProvider);
			label.FontSize = (double)obj2;
			label.SetValue(Label.HorizontalTextAlignmentProperty, new TextAlignmentConverter().ConvertFromInvariantString("Center"));
			bindingExtension4.Path = "SelectedItem";
			bindingExtension4.Mode = 2;
			BindingBase bindingBase4 = bindingExtension4.ProvideValue(null);
			label.SetBinding(Label.TextProperty, bindingBase4);
			dynamicResourceExtension.Key = "ButtonAccentColor";
			IMarkupExtension<DynamicResource> markupExtension2 = dynamicResourceExtension;
			XamlServiceProvider xamlServiceProvider2 = new XamlServiceProvider();
			Type typeFromHandle3 = typeof(IProvideValueTarget);
			object[] array2 = new object[0 + 3];
			array2[0] = label;
			array2[1] = grid;
			array2[2] = this;
			object obj3;
			xamlServiceProvider2.Add(typeFromHandle3, obj3 = new SimpleValueTargetProvider(array2, Label.TextColorProperty, nameScope));
			xamlServiceProvider2.Add(typeof(IReferenceProvider), obj3);
			Type typeFromHandle4 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver2 = new XmlNamespaceResolver();
			xmlNamespaceResolver2.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver2.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver2.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xamlServiceProvider2.Add(typeFromHandle4, new XamlTypeResolver(xmlNamespaceResolver2, typeof(SettingsCellWithPickerInCenter).GetTypeInfo().Assembly));
			xamlServiceProvider2.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(27, 17)));
			DynamicResource dynamicResource = markupExtension2.ProvideValue(xamlServiceProvider2);
			label.SetDynamicResource(Label.TextColorProperty, dynamicResource.Key);
			grid.Children.Add(label);
			this.SetValue(CustomCell.ContentProperty, grid);
		}

		// Token: 0x060035D6 RID: 13782 RVA: 0x0026A860 File Offset: 0x00268A60
		// Note: this type is marked as 'beforefieldinit'.
		static SettingsCellWithPickerInCenter()
		{
		}

		// Token: 0x060035D7 RID: 13783 RVA: 0x0026A8F0 File Offset: 0x00268AF0
		private void __InitComponentRuntime()
		{
			Extensions.LoadFromXaml<SettingsCellWithPickerInCenter>(this, typeof(SettingsCellWithPickerInCenter));
			this.me = NameScopeExtensions.FindByName<CustomCell>(this, "me");
			this.picker = NameScopeExtensions.FindByName<Picker>(this, "picker");
			this.label = NameScopeExtensions.FindByName<Label>(this, "label");
		}

		// Token: 0x0400200F RID: 8207
		public static BindableProperty SelectedItemProperty = BindableProperty.Create("SelectedItem", typeof(object), typeof(SettingsCellWithPickerInCenter), null, 1, null, null, null, null, null);

		// Token: 0x04002010 RID: 8208
		public static BindableProperty SelectedIndexProperty = BindableProperty.Create("SelectedIndex", typeof(int), typeof(SettingsCellWithPickerInCenter), 0, 1, null, null, null, null, null);

		// Token: 0x04002011 RID: 8209
		public static BindableProperty ItemsSourceProperty = BindableProperty.Create("ItemsSource", typeof(IList), typeof(SettingsCellWithPickerInCenter), null, 2, null, null, null, null, null);

		// Token: 0x04002012 RID: 8210
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private CustomCell me;

		// Token: 0x04002013 RID: 8211
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private Picker picker;

		// Token: 0x04002014 RID: 8212
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private Label label;
	}
}
