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
	// Token: 0x020005DE RID: 1502
	[XamlCompilation(2)]
	[XamlFilePath("UserControls\\SettingsCellWithPicker.xaml")]
	public class SettingsCellWithPicker : CustomCell
	{
		// Token: 0x060035C1 RID: 13761 RVA: 0x0026955B File Offset: 0x0026775B
		public SettingsCellWithPicker()
		{
			this.InitializeComponent();
			base.Tapped += this.SettingsCellWithPicker_Tapped;
		}

		// Token: 0x060035C2 RID: 13762 RVA: 0x0026957B File Offset: 0x0026777B
		private void SettingsCellWithPicker_Tapped(object sender, EventArgs e)
		{
			this.picker.Focus();
		}

		// Token: 0x1700136C RID: 4972
		// (get) Token: 0x060035C3 RID: 13763 RVA: 0x00269589 File Offset: 0x00267789
		// (set) Token: 0x060035C4 RID: 13764 RVA: 0x00269596 File Offset: 0x00267796
		public object SelectedItem
		{
			get
			{
				return base.GetValue(SettingsCellWithPicker.SelectedItemProperty);
			}
			set
			{
				base.SetValue(SettingsCellWithPicker.SelectedItemProperty, value);
			}
		}

		// Token: 0x1700136D RID: 4973
		// (get) Token: 0x060035C5 RID: 13765 RVA: 0x002695A4 File Offset: 0x002677A4
		// (set) Token: 0x060035C6 RID: 13766 RVA: 0x002695B6 File Offset: 0x002677B6
		public int SelectedIndex
		{
			get
			{
				return (int)base.GetValue(SettingsCellWithPicker.SelectedIndexProperty);
			}
			set
			{
				base.SetValue(SettingsCellWithPicker.SelectedIndexProperty, value);
			}
		}

		// Token: 0x1700136E RID: 4974
		// (get) Token: 0x060035C7 RID: 13767 RVA: 0x002695C9 File Offset: 0x002677C9
		// (set) Token: 0x060035C8 RID: 13768 RVA: 0x002695DB File Offset: 0x002677DB
		public IList ItemsSource
		{
			get
			{
				return (IList)base.GetValue(SettingsCellWithPicker.ItemsSourceProperty);
			}
			set
			{
				base.SetValue(SettingsCellWithPicker.ItemsSourceProperty, value);
			}
		}

		// Token: 0x060035C9 RID: 13769 RVA: 0x002695EC File Offset: 0x002677EC
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private void InitializeComponent()
		{
			if (ResourceLoader.IsEnabled && ResourceLoader.CanProvideContentFor(new ResourceLoader.ResourceLoadingQuery
			{
				AssemblyName = typeof(SettingsCellWithPicker).GetTypeInfo().Assembly.GetName(),
				ResourcePath = "UserControls/SettingsCellWithPicker.xaml",
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
			ColumnDefinition columnDefinition;
			VisualDiagnostics.RegisterSourceInfo(columnDefinition = new ColumnDefinition(), new Uri("UserControls\\SettingsCellWithPicker.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 11, 18);
			ColumnDefinition columnDefinition2;
			VisualDiagnostics.RegisterSourceInfo(columnDefinition2 = new ColumnDefinition(), new Uri("UserControls\\SettingsCellWithPicker.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 12, 18);
			StaticResourceExtension staticResourceExtension;
			VisualDiagnostics.RegisterSourceInfo(staticResourceExtension = new StaticResourceExtension(), new Uri("UserControls\\SettingsCellWithPicker.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 16, 17);
			ReferenceExtension referenceExtension;
			VisualDiagnostics.RegisterSourceInfo(referenceExtension = new ReferenceExtension(), new Uri("UserControls\\SettingsCellWithPicker.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 17, 17);
			BindingExtension bindingExtension;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension = new BindingExtension(), new Uri("UserControls\\SettingsCellWithPicker.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 17, 17);
			DynamicResourceExtension dynamicResourceExtension;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension = new DynamicResourceExtension(), new Uri("UserControls\\SettingsCellWithPicker.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 18, 17);
			Label label;
			VisualDiagnostics.RegisterSourceInfo(label = new Label(), new Uri("UserControls\\SettingsCellWithPicker.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 14, 14);
			ReferenceExtension referenceExtension2;
			VisualDiagnostics.RegisterSourceInfo(referenceExtension2 = new ReferenceExtension(), new Uri("UserControls\\SettingsCellWithPicker.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 25, 17);
			BindingExtension bindingExtension2;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension2 = new BindingExtension(), new Uri("UserControls\\SettingsCellWithPicker.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 25, 17);
			ReferenceExtension referenceExtension3;
			VisualDiagnostics.RegisterSourceInfo(referenceExtension3 = new ReferenceExtension(), new Uri("UserControls\\SettingsCellWithPicker.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 26, 17);
			BindingExtension bindingExtension3;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension3 = new BindingExtension(), new Uri("UserControls\\SettingsCellWithPicker.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 26, 17);
			ReferenceExtension referenceExtension4;
			VisualDiagnostics.RegisterSourceInfo(referenceExtension4 = new ReferenceExtension(), new Uri("UserControls\\SettingsCellWithPicker.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 27, 17);
			BindingExtension bindingExtension4;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension4 = new BindingExtension(), new Uri("UserControls\\SettingsCellWithPicker.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 27, 17);
			Picker picker;
			VisualDiagnostics.RegisterSourceInfo(picker = new Picker(), new Uri("UserControls\\SettingsCellWithPicker.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 21, 14);
			Grid grid;
			VisualDiagnostics.RegisterSourceInfo(grid = new Grid(), new Uri("UserControls\\SettingsCellWithPicker.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 9, 10);
			VisualDiagnostics.RegisterSourceInfo(this, new Uri("UserControls\\SettingsCellWithPicker.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 2, 2);
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
			this.me = this;
			this.picker = picker;
			columnDefinition.SetValue(ColumnDefinition.WidthProperty, new GridLengthTypeConverter().ConvertFromInvariantString("auto"));
			grid.GetValue(Grid.ColumnDefinitionsProperty).Add(columnDefinition);
			columnDefinition2.SetValue(ColumnDefinition.WidthProperty, new GridLengthTypeConverter().ConvertFromInvariantString("*"));
			grid.GetValue(Grid.ColumnDefinitionsProperty).Add(columnDefinition2);
			label.SetValue(Grid.ColumnProperty, 0);
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
			xamlServiceProvider.Add(typeFromHandle2, new XamlTypeResolver(xmlNamespaceResolver, typeof(SettingsCellWithPicker).GetTypeInfo().Assembly));
			xamlServiceProvider.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(16, 17)));
			object obj2 = markupExtension.ProvideValue(xamlServiceProvider);
			label.FontSize = (double)obj2;
			referenceExtension.Name = "me";
			IMarkupExtension markupExtension2 = referenceExtension;
			XamlServiceProvider xamlServiceProvider2 = new XamlServiceProvider();
			Type typeFromHandle3 = typeof(IProvideValueTarget);
			object[] array2 = new object[0 + 4];
			array2[0] = bindingExtension;
			array2[1] = label;
			array2[2] = grid;
			array2[3] = this;
			object obj3;
			xamlServiceProvider2.Add(typeFromHandle3, obj3 = new SimpleValueTargetProvider(array2, typeof(BindingExtension).GetRuntimeProperty("Source"), nameScope));
			xamlServiceProvider2.Add(typeof(IReferenceProvider), obj3);
			Type typeFromHandle4 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver2 = new XmlNamespaceResolver();
			xmlNamespaceResolver2.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver2.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver2.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xamlServiceProvider2.Add(typeFromHandle4, new XamlTypeResolver(xmlNamespaceResolver2, typeof(SettingsCellWithPicker).GetTypeInfo().Assembly));
			xamlServiceProvider2.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(17, 17)));
			object obj4 = markupExtension2.ProvideValue(xamlServiceProvider2);
			bindingExtension.Source = obj4;
			bindingExtension.Path = "Title";
			BindingBase bindingBase = bindingExtension.ProvideValue(null);
			label.SetBinding(Label.TextProperty, bindingBase);
			dynamicResourceExtension.Key = "SettingsCellTitleColor";
			IMarkupExtension<DynamicResource> markupExtension3 = dynamicResourceExtension;
			XamlServiceProvider xamlServiceProvider3 = new XamlServiceProvider();
			Type typeFromHandle5 = typeof(IProvideValueTarget);
			object[] array3 = new object[0 + 3];
			array3[0] = label;
			array3[1] = grid;
			array3[2] = this;
			object obj5;
			xamlServiceProvider3.Add(typeFromHandle5, obj5 = new SimpleValueTargetProvider(array3, Label.TextColorProperty, nameScope));
			xamlServiceProvider3.Add(typeof(IReferenceProvider), obj5);
			Type typeFromHandle6 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver3 = new XmlNamespaceResolver();
			xmlNamespaceResolver3.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver3.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver3.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xamlServiceProvider3.Add(typeFromHandle6, new XamlTypeResolver(xmlNamespaceResolver3, typeof(SettingsCellWithPicker).GetTypeInfo().Assembly));
			xamlServiceProvider3.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(18, 17)));
			DynamicResource dynamicResource = markupExtension3.ProvideValue(xamlServiceProvider3);
			label.SetDynamicResource(Label.TextColorProperty, dynamicResource.Key);
			label.SetValue(View.VerticalOptionsProperty, LayoutOptions.Center);
			grid.Children.Add(label);
			picker.SetValue(Grid.ColumnProperty, 1);
			picker.SetValue(View.HorizontalOptionsProperty, LayoutOptions.End);
			bindingExtension2.Mode = 2;
			referenceExtension2.Name = "me";
			IMarkupExtension markupExtension4 = referenceExtension2;
			XamlServiceProvider xamlServiceProvider4 = new XamlServiceProvider();
			Type typeFromHandle7 = typeof(IProvideValueTarget);
			object[] array4 = new object[0 + 4];
			array4[0] = bindingExtension2;
			array4[1] = picker;
			array4[2] = grid;
			array4[3] = this;
			object obj6;
			xamlServiceProvider4.Add(typeFromHandle7, obj6 = new SimpleValueTargetProvider(array4, typeof(BindingExtension).GetRuntimeProperty("Source"), nameScope));
			xamlServiceProvider4.Add(typeof(IReferenceProvider), obj6);
			Type typeFromHandle8 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver4 = new XmlNamespaceResolver();
			xmlNamespaceResolver4.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver4.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver4.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xamlServiceProvider4.Add(typeFromHandle8, new XamlTypeResolver(xmlNamespaceResolver4, typeof(SettingsCellWithPicker).GetTypeInfo().Assembly));
			xamlServiceProvider4.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(25, 17)));
			object obj7 = markupExtension4.ProvideValue(xamlServiceProvider4);
			bindingExtension2.Source = obj7;
			bindingExtension2.Path = "ItemsSource";
			BindingBase bindingBase2 = bindingExtension2.ProvideValue(null);
			picker.SetBinding(Picker.ItemsSourceProperty, bindingBase2);
			bindingExtension3.Mode = 1;
			referenceExtension3.Name = "me";
			IMarkupExtension markupExtension5 = referenceExtension3;
			XamlServiceProvider xamlServiceProvider5 = new XamlServiceProvider();
			Type typeFromHandle9 = typeof(IProvideValueTarget);
			object[] array5 = new object[0 + 4];
			array5[0] = bindingExtension3;
			array5[1] = picker;
			array5[2] = grid;
			array5[3] = this;
			object obj8;
			xamlServiceProvider5.Add(typeFromHandle9, obj8 = new SimpleValueTargetProvider(array5, typeof(BindingExtension).GetRuntimeProperty("Source"), nameScope));
			xamlServiceProvider5.Add(typeof(IReferenceProvider), obj8);
			Type typeFromHandle10 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver5 = new XmlNamespaceResolver();
			xmlNamespaceResolver5.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver5.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver5.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xamlServiceProvider5.Add(typeFromHandle10, new XamlTypeResolver(xmlNamespaceResolver5, typeof(SettingsCellWithPicker).GetTypeInfo().Assembly));
			xamlServiceProvider5.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(26, 17)));
			object obj9 = markupExtension5.ProvideValue(xamlServiceProvider5);
			bindingExtension3.Source = obj9;
			bindingExtension3.Path = "SelectedIndex";
			BindingBase bindingBase3 = bindingExtension3.ProvideValue(null);
			picker.SetBinding(Picker.SelectedIndexProperty, bindingBase3);
			bindingExtension4.Mode = 1;
			referenceExtension4.Name = "me";
			IMarkupExtension markupExtension6 = referenceExtension4;
			XamlServiceProvider xamlServiceProvider6 = new XamlServiceProvider();
			Type typeFromHandle11 = typeof(IProvideValueTarget);
			object[] array6 = new object[0 + 4];
			array6[0] = bindingExtension4;
			array6[1] = picker;
			array6[2] = grid;
			array6[3] = this;
			object obj10;
			xamlServiceProvider6.Add(typeFromHandle11, obj10 = new SimpleValueTargetProvider(array6, typeof(BindingExtension).GetRuntimeProperty("Source"), nameScope));
			xamlServiceProvider6.Add(typeof(IReferenceProvider), obj10);
			Type typeFromHandle12 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver6 = new XmlNamespaceResolver();
			xmlNamespaceResolver6.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver6.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver6.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xamlServiceProvider6.Add(typeFromHandle12, new XamlTypeResolver(xmlNamespaceResolver6, typeof(SettingsCellWithPicker).GetTypeInfo().Assembly));
			xamlServiceProvider6.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(27, 17)));
			object obj11 = markupExtension6.ProvideValue(xamlServiceProvider6);
			bindingExtension4.Source = obj11;
			bindingExtension4.Path = "SelectedItem";
			BindingBase bindingBase4 = bindingExtension4.ProvideValue(null);
			picker.SetBinding(Picker.SelectedItemProperty, bindingBase4);
			picker.SetValue(View.VerticalOptionsProperty, LayoutOptions.Center);
			grid.Children.Add(picker);
			this.SetValue(CustomCell.ContentProperty, grid);
		}

		// Token: 0x060035CA RID: 13770 RVA: 0x0026A0D8 File Offset: 0x002682D8
		// Note: this type is marked as 'beforefieldinit'.
		static SettingsCellWithPicker()
		{
		}

		// Token: 0x060035CB RID: 13771 RVA: 0x0026A168 File Offset: 0x00268368
		private void __InitComponentRuntime()
		{
			Extensions.LoadFromXaml<SettingsCellWithPicker>(this, typeof(SettingsCellWithPicker));
			this.me = NameScopeExtensions.FindByName<CustomCell>(this, "me");
			this.picker = NameScopeExtensions.FindByName<Picker>(this, "picker");
		}

		// Token: 0x0400200A RID: 8202
		public static BindableProperty SelectedItemProperty = BindableProperty.Create("SelectedItem", typeof(object), typeof(SettingsCellWithPicker), null, 1, null, null, null, null, null);

		// Token: 0x0400200B RID: 8203
		public static BindableProperty SelectedIndexProperty = BindableProperty.Create("SelectedIndex", typeof(int), typeof(SettingsCellWithPicker), 0, 1, null, null, null, null, null);

		// Token: 0x0400200C RID: 8204
		public static BindableProperty ItemsSourceProperty = BindableProperty.Create("ItemsSource", typeof(IList), typeof(SettingsCellWithPicker), null, 2, null, null, null, null, null);

		// Token: 0x0400200D RID: 8205
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private CustomCell me;

		// Token: 0x0400200E RID: 8206
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private Picker picker;
	}
}
