using System;
using System.CodeDom.Compiler;
using System.Reflection;
using System.Runtime.CompilerServices;
using CarScannerXamarinForms.Coding.DB.MQB;
using Xamarin.Forms;
using Xamarin.Forms.Internals;
using Xamarin.Forms.PlatformConfiguration.iOSSpecific;
using Xamarin.Forms.Xaml;
using Xamarin.Forms.Xaml.Diagnostics;
using Xamarin.Forms.Xaml.Internals;

namespace CarScannerXamarinForms.Coding.Pages
{
	// Token: 0x02000951 RID: 2385
	[XamlCompilation(2)]
	[XamlFilePath("Coding\\Pages\\FPAProfileEditor.xaml")]
	public class FPAProfileEditor : ContentPage
	{
		// Token: 0x06004E96 RID: 20118 RVA: 0x003B907E File Offset: 0x003B727E
		public FPAProfileEditor(FPAProfile profile)
		{
			this.InitializeComponent();
			base.BindingContext = profile;
		}

		// Token: 0x06004E97 RID: 20119 RVA: 0x003B9094 File Offset: 0x003B7294
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private void InitializeComponent()
		{
			if (ResourceLoader.IsEnabled && ResourceLoader.CanProvideContentFor(new ResourceLoader.ResourceLoadingQuery
			{
				AssemblyName = typeof(FPAProfileEditor).GetTypeInfo().Assembly.GetName(),
				ResourcePath = "Coding/Pages/FPAProfileEditor.xaml",
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
			VisualDiagnostics.RegisterSourceInfo(bindingExtension = new BindingExtension(), new Uri("Coding\\Pages\\FPAProfileEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 10, 5);
			DynamicResourceExtension dynamicResourceExtension;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension = new DynamicResourceExtension(), new Uri("Coding\\Pages\\FPAProfileEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 13, 5);
			Label label;
			VisualDiagnostics.RegisterSourceInfo(label = new Label(), new Uri("Coding\\Pages\\FPAProfileEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 17, 18);
			BindingExtension bindingExtension2;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension2 = new BindingExtension(), new Uri("Coding\\Pages\\FPAProfileEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 18, 25);
			BindingExtension bindingExtension3;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension3 = new BindingExtension(), new Uri("Coding\\Pages\\FPAProfileEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 18, 64);
			Picker picker;
			VisualDiagnostics.RegisterSourceInfo(picker = new Picker(), new Uri("Coding\\Pages\\FPAProfileEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 18, 18);
			Label label2;
			VisualDiagnostics.RegisterSourceInfo(label2 = new Label(), new Uri("Coding\\Pages\\FPAProfileEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 20, 18);
			BindingExtension bindingExtension4;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension4 = new BindingExtension(), new Uri("Coding\\Pages\\FPAProfileEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 21, 48);
			DataTemplate dataTemplate;
			VisualDiagnostics.RegisterSourceInfo(dataTemplate = new DataTemplate(), new Uri("Coding\\Pages\\FPAProfileEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 23, 26);
			ListView listView;
			VisualDiagnostics.RegisterSourceInfo(listView = new ListView(), new Uri("Coding\\Pages\\FPAProfileEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 21, 18);
			StackLayout stackLayout;
			VisualDiagnostics.RegisterSourceInfo(stackLayout = new StackLayout(), new Uri("Coding\\Pages\\FPAProfileEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 16, 14);
			ScrollView scrollView;
			VisualDiagnostics.RegisterSourceInfo(scrollView = new ScrollView(), new Uri("Coding\\Pages\\FPAProfileEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 15, 10);
			VisualDiagnostics.RegisterSourceInfo(this, new Uri("Coding\\Pages\\FPAProfileEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 2, 2);
			NameScope nameScope = NameScope.GetNameScope(this) ?? new NameScope();
			NameScope.SetNameScope(this, nameScope);
			bindingExtension.Path = "Title";
			BindingBase bindingBase = bindingExtension.ProvideValue(null);
			this.SetBinding(Page.TitleProperty, bindingBase);
			this.SetValue(Page.PaddingProperty, new Thickness(5.0, 0.0));
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
			xmlNamespaceResolver.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver.Add("settings", "clr-namespace:CarScannerXamarinForms.Settings");
			xamlServiceProvider.Add(typeFromHandle2, new XamlTypeResolver(xmlNamespaceResolver, typeof(FPAProfileEditor).GetTypeInfo().Assembly));
			xamlServiceProvider.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(13, 5)));
			DynamicResource dynamicResource = markupExtension.ProvideValue(xamlServiceProvider);
			this.SetDynamicResource(VisualElement.BackgroundColorProperty, dynamicResource.Key);
			scrollView.SetValue(ScrollView.OrientationProperty, 0);
			label.SetValue(Label.FontAttributesProperty, new FontAttributesConverter().ConvertFromInvariantString("Bold"));
			label.SetValue(Label.TextProperty, "Return to profile after restart:");
			stackLayout.Children.Add(label);
			bindingExtension2.Path = "ReturnToValues";
			BindingBase bindingBase2 = bindingExtension2.ProvideValue(null);
			picker.SetBinding(Picker.ItemsSourceProperty, bindingBase2);
			bindingExtension3.Mode = 1;
			bindingExtension3.Path = "ReturnAfterRestart";
			BindingBase bindingBase3 = bindingExtension3.ProvideValue(null);
			picker.SetBinding(Picker.SelectedIndexProperty, bindingBase3);
			stackLayout.Children.Add(picker);
			label2.SetValue(Label.FontAttributesProperty, new FontAttributesConverter().ConvertFromInvariantString("Bold"));
			label2.SetValue(Label.TextProperty, "Controls:");
			stackLayout.Children.Add(label2);
			listView.SetValue(ListView.HasUnevenRowsProperty, true);
			bindingExtension4.Path = "Controls";
			BindingBase bindingBase4 = bindingExtension4.ProvideValue(null);
			listView.SetBinding(ItemsView<Cell>.ItemsSourceProperty, bindingBase4);
			IDataTemplate dataTemplate2 = dataTemplate;
			FPAProfileEditor.<InitializeComponent>_anonXamlCDataTemplate_13 <InitializeComponent>_anonXamlCDataTemplate_ = new FPAProfileEditor.<InitializeComponent>_anonXamlCDataTemplate_13();
			object[] array2 = new object[0 + 5];
			array2[0] = dataTemplate;
			array2[1] = listView;
			array2[2] = stackLayout;
			array2[3] = scrollView;
			array2[4] = this;
			<InitializeComponent>_anonXamlCDataTemplate_.parentValues = array2;
			<InitializeComponent>_anonXamlCDataTemplate_.root = this;
			dataTemplate2.LoadTemplate = new Func<object>(<InitializeComponent>_anonXamlCDataTemplate_.LoadDataTemplate);
			listView.SetValue(ItemsView<Cell>.ItemTemplateProperty, dataTemplate);
			stackLayout.Children.Add(listView);
			scrollView.Content = stackLayout;
			this.SetValue(ContentPage.ContentProperty, scrollView);
		}

		// Token: 0x06004E98 RID: 20120 RVA: 0x003B964B File Offset: 0x003B784B
		private void __InitComponentRuntime()
		{
			Extensions.LoadFromXaml<FPAProfileEditor>(this, typeof(FPAProfileEditor));
		}

		// Token: 0x02000952 RID: 2386
		[CompilerGenerated]
		private sealed class <InitializeComponent>_anonXamlCDataTemplate_13
		{
			// Token: 0x06004E99 RID: 20121 RVA: 0x003B9660 File Offset: 0x003B7860
			public <InitializeComponent>_anonXamlCDataTemplate_13()
			{
			}

			// Token: 0x06004E9A RID: 20122 RVA: 0x003B9674 File Offset: 0x003B7874
			internal object LoadDataTemplate()
			{
				BindingExtension bindingExtension;
				VisualDiagnostics.RegisterSourceInfo(bindingExtension = new BindingExtension(), new Uri("Coding\\Pages\\FPAProfileEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 29, 55);
				Span span;
				VisualDiagnostics.RegisterSourceInfo(span = new Span(), new Uri("Coding\\Pages\\FPAProfileEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 29, 50);
				Span span2;
				VisualDiagnostics.RegisterSourceInfo(span2 = new Span(), new Uri("Coding\\Pages\\FPAProfileEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 30, 50);
				FormattedString formattedString;
				VisualDiagnostics.RegisterSourceInfo(formattedString = new FormattedString(), new Uri("Coding\\Pages\\FPAProfileEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 28, 46);
				Label label;
				VisualDiagnostics.RegisterSourceInfo(label = new Label(), new Uri("Coding\\Pages\\FPAProfileEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 26, 38);
				BindingExtension bindingExtension2;
				VisualDiagnostics.RegisterSourceInfo(bindingExtension2 = new BindingExtension(), new Uri("Coding\\Pages\\FPAProfileEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 36, 41);
				BindingExtension bindingExtension3;
				VisualDiagnostics.RegisterSourceInfo(bindingExtension3 = new BindingExtension(), new Uri("Coding\\Pages\\FPAProfileEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 37, 41);
				Picker picker;
				VisualDiagnostics.RegisterSourceInfo(picker = new Picker(), new Uri("Coding\\Pages\\FPAProfileEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 34, 38);
				StackLayout stackLayout;
				VisualDiagnostics.RegisterSourceInfo(stackLayout = new StackLayout(), new Uri("Coding\\Pages\\FPAProfileEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 25, 34);
				ViewCell viewCell;
				VisualDiagnostics.RegisterSourceInfo(viewCell = new ViewCell(), new Uri("Coding\\Pages\\FPAProfileEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 24, 30);
				NameScope nameScope = new NameScope();
				NameScope.SetNameScope(viewCell, nameScope);
				nameScope.RegisterName("pickerControlValues", picker);
				if (picker.StyleId == null)
				{
					picker.StyleId = "pickerControlValues";
				}
				stackLayout.SetValue(StackLayout.OrientationProperty, 0);
				bindingExtension.Path = "Title";
				BindingBase bindingBase = bindingExtension.ProvideValue(null);
				span.SetBinding(Span.TextProperty, bindingBase);
				formattedString.Spans.Add(span);
				span2.SetValue(Span.TextProperty, ":");
				formattedString.Spans.Add(span2);
				label.SetValue(Label.FormattedTextProperty, formattedString);
				stackLayout.Children.Add(label);
				bindingExtension2.Path = "ControlValues";
				BindingBase bindingBase2 = bindingExtension2.ProvideValue(null);
				picker.SetBinding(Picker.ItemsSourceProperty, bindingBase2);
				bindingExtension3.Mode = 1;
				bindingExtension3.Path = "Value";
				BindingBase bindingBase3 = bindingExtension3.ProvideValue(null);
				picker.SetBinding(Picker.SelectedIndexProperty, bindingBase3);
				stackLayout.Children.Add(picker);
				viewCell.View = stackLayout;
				return viewCell;
			}

			// Token: 0x04002F3A RID: 12090
			internal object[] parentValues;

			// Token: 0x04002F3B RID: 12091
			internal FPAProfileEditor root;
		}
	}
}
