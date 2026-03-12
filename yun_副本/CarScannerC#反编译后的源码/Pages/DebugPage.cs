using System;
using System.CodeDom.Compiler;
using System.Diagnostics;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using CarScannerXamarinForms.Settings;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Internals;
using Xamarin.Forms.PlatformConfiguration.iOSSpecific;
using Xamarin.Forms.Xaml;
using Xamarin.Forms.Xaml.Diagnostics;
using Xamarin.Forms.Xaml.Internals;

namespace CarScannerXamarinForms.Pages
{
	// Token: 0x02000619 RID: 1561
	[XamlFilePath("Pages\\DebugPage.xaml")]
	public class DebugPage : ContentPage
	{
		// Token: 0x060036C8 RID: 14024 RVA: 0x0028889E File Offset: 0x00286A9E
		public DebugPage()
		{
			this.InitializeComponent();
			this.editor.Text = SharedSettings.Current.LastException;
		}

		// Token: 0x060036C9 RID: 14025 RVA: 0x000A8D6F File Offset: 0x000A6F6F
		protected override bool OnBackButtonPressed()
		{
			return true;
		}

		// Token: 0x060036CA RID: 14026 RVA: 0x002888C1 File Offset: 0x00286AC1
		private void Handle_Clicked(object sender, EventArgs e)
		{
			SharedSettings.Current.LastException = "";
			App.Instance.ChangeLanguage();
		}

		// Token: 0x060036CB RID: 14027 RVA: 0x002888DC File Offset: 0x00286ADC
		private async void SendEmail_Clicked(object sender, EventArgs e)
		{
			await SharedSettings.Current.SendDebugEmail();
		}

		// Token: 0x060036CC RID: 14028 RVA: 0x0028890B File Offset: 0x00286B0B
		private void btnCopyToClipboard_Clicked(object sender, EventArgs e)
		{
			string text = "admin@carscanner.info\r\nDebug information:";
			Task<string> settingsReport = SharedSettings.Current.GetSettingsReport();
			Clipboard.SetTextAsync(text + ((settingsReport != null) ? settingsReport.ToString() : null) + "\r\n" + SharedSettings.Current.LastException);
		}

		// Token: 0x060036CD RID: 14029 RVA: 0x000027D4 File Offset: 0x000009D4
		private void Handle_SizeChanged(object sender, EventArgs e)
		{
		}

		// Token: 0x060036CE RID: 14030 RVA: 0x00288944 File Offset: 0x00286B44
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private void InitializeComponent()
		{
			if (ResourceLoader.IsEnabled && ResourceLoader.CanProvideContentFor(new ResourceLoader.ResourceLoadingQuery
			{
				AssemblyName = typeof(DebugPage).GetTypeInfo().Assembly.GetName(),
				ResourcePath = "Pages/DebugPage.xaml",
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
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension = new DynamicResourceExtension(), new Uri("Pages\\DebugPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 10, 5);
			RowDefinition rowDefinition;
			VisualDiagnostics.RegisterSourceInfo(rowDefinition = new RowDefinition(), new Uri("Pages\\DebugPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 16, 18);
			RowDefinition rowDefinition2;
			VisualDiagnostics.RegisterSourceInfo(rowDefinition2 = new RowDefinition(), new Uri("Pages\\DebugPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 17, 18);
			RowDefinition rowDefinition3;
			VisualDiagnostics.RegisterSourceInfo(rowDefinition3 = new RowDefinition(), new Uri("Pages\\DebugPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 18, 18);
			RowDefinition rowDefinition4;
			VisualDiagnostics.RegisterSourceInfo(rowDefinition4 = new RowDefinition(), new Uri("Pages\\DebugPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 19, 18);
			RowDefinition rowDefinition5;
			VisualDiagnostics.RegisterSourceInfo(rowDefinition5 = new RowDefinition(), new Uri("Pages\\DebugPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 20, 18);
			RowDefinition rowDefinition6;
			VisualDiagnostics.RegisterSourceInfo(rowDefinition6 = new RowDefinition(), new Uri("Pages\\DebugPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 21, 18);
			RowDefinition rowDefinition7;
			VisualDiagnostics.RegisterSourceInfo(rowDefinition7 = new RowDefinition(), new Uri("Pages\\DebugPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 22, 18);
			RowDefinition rowDefinition8;
			VisualDiagnostics.RegisterSourceInfo(rowDefinition8 = new RowDefinition(), new Uri("Pages\\DebugPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 23, 18);
			Translate translate;
			VisualDiagnostics.RegisterSourceInfo(translate = new Translate(), new Uri("Pages\\DebugPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 30, 17);
			DynamicResourceExtension dynamicResourceExtension2;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension2 = new DynamicResourceExtension(), new Uri("Pages\\DebugPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 31, 17);
			Label label;
			VisualDiagnostics.RegisterSourceInfo(label = new Label(), new Uri("Pages\\DebugPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 25, 14);
			Translate translate2;
			VisualDiagnostics.RegisterSourceInfo(translate2 = new Translate(), new Uri("Pages\\DebugPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 34, 17);
			DynamicResourceExtension dynamicResourceExtension3;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension3 = new DynamicResourceExtension(), new Uri("Pages\\DebugPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 35, 17);
			Label label2;
			VisualDiagnostics.RegisterSourceInfo(label2 = new Label(), new Uri("Pages\\DebugPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 32, 14);
			DynamicResourceExtension dynamicResourceExtension4;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension4 = new DynamicResourceExtension(), new Uri("Pages\\DebugPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 42, 17);
			Label label3;
			VisualDiagnostics.RegisterSourceInfo(label3 = new Label(), new Uri("Pages\\DebugPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 36, 14);
			Translate translate3;
			VisualDiagnostics.RegisterSourceInfo(translate3 = new Translate(), new Uri("Pages\\DebugPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 45, 17);
			DynamicResourceExtension dynamicResourceExtension5;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension5 = new DynamicResourceExtension(), new Uri("Pages\\DebugPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 46, 17);
			Label label4;
			VisualDiagnostics.RegisterSourceInfo(label4 = new Label(), new Uri("Pages\\DebugPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 43, 14);
			DynamicResourceExtension dynamicResourceExtension6;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension6 = new DynamicResourceExtension(), new Uri("Pages\\DebugPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 51, 17);
			DynamicResourceExtension dynamicResourceExtension7;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension7 = new DynamicResourceExtension(), new Uri("Pages\\DebugPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 52, 17);
			Editor editor;
			VisualDiagnostics.RegisterSourceInfo(editor = new Editor(), new Uri("Pages\\DebugPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 48, 14);
			Translate translate4;
			VisualDiagnostics.RegisterSourceInfo(translate4 = new Translate(), new Uri("Pages\\DebugPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 57, 17);
			DynamicResourceExtension dynamicResourceExtension8;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension8 = new DynamicResourceExtension(), new Uri("Pages\\DebugPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 58, 17);
			Button button;
			VisualDiagnostics.RegisterSourceInfo(button = new Button(), new Uri("Pages\\DebugPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 53, 14);
			Translate translate5;
			VisualDiagnostics.RegisterSourceInfo(translate5 = new Translate(), new Uri("Pages\\DebugPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 63, 17);
			DynamicResourceExtension dynamicResourceExtension9;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension9 = new DynamicResourceExtension(), new Uri("Pages\\DebugPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 64, 17);
			Button button2;
			VisualDiagnostics.RegisterSourceInfo(button2 = new Button(), new Uri("Pages\\DebugPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 59, 14);
			Translate translate6;
			VisualDiagnostics.RegisterSourceInfo(translate6 = new Translate(), new Uri("Pages\\DebugPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 69, 17);
			DynamicResourceExtension dynamicResourceExtension10;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension10 = new DynamicResourceExtension(), new Uri("Pages\\DebugPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 70, 17);
			Button button3;
			VisualDiagnostics.RegisterSourceInfo(button3 = new Button(), new Uri("Pages\\DebugPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 65, 14);
			Grid grid;
			VisualDiagnostics.RegisterSourceInfo(grid = new Grid(), new Uri("Pages\\DebugPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 14, 10);
			VisualDiagnostics.RegisterSourceInfo(this, new Uri("Pages\\DebugPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 2, 2);
			NameScope nameScope = NameScope.GetNameScope(this) ?? new NameScope();
			NameScope.SetNameScope(this, nameScope);
			nameScope.RegisterName("editor", editor);
			if (editor.StyleId == null)
			{
				editor.StyleId = "editor";
			}
			nameScope.RegisterName("btnCopyClipboard", button);
			if (button.StyleId == null)
			{
				button.StyleId = "btnCopyClipboard";
			}
			nameScope.RegisterName("btnSendEmail", button2);
			if (button2.StyleId == null)
			{
				button2.StyleId = "btnSendEmail";
			}
			nameScope.RegisterName("btnContinue", button3);
			if (button3.StyleId == null)
			{
				button3.StyleId = "btnContinue";
			}
			this.editor = editor;
			this.btnCopyClipboard = button;
			this.btnSendEmail = button2;
			this.btnContinue = button3;
			this.SetValue(Page.PaddingProperty, new Thickness(10.0, 10.0, 10.0, 10.0));
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
			xmlNamespaceResolver.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver.Add("local", "clr-namespace:CarScannerXamarinForms");
			xamlServiceProvider.Add(typeFromHandle2, new XamlTypeResolver(xmlNamespaceResolver, typeof(DebugPage).GetTypeInfo().Assembly));
			xamlServiceProvider.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(10, 5)));
			DynamicResource dynamicResource = markupExtension.ProvideValue(xamlServiceProvider);
			this.SetDynamicResource(VisualElement.BackgroundColorProperty, dynamicResource.Key);
			this.SizeChanged += this.Handle_SizeChanged;
			rowDefinition.SetValue(RowDefinition.HeightProperty, new GridLengthTypeConverter().ConvertFromInvariantString("auto"));
			grid.GetValue(Grid.RowDefinitionsProperty).Add(rowDefinition);
			rowDefinition2.SetValue(RowDefinition.HeightProperty, new GridLengthTypeConverter().ConvertFromInvariantString("auto"));
			grid.GetValue(Grid.RowDefinitionsProperty).Add(rowDefinition2);
			rowDefinition3.SetValue(RowDefinition.HeightProperty, new GridLengthTypeConverter().ConvertFromInvariantString("auto"));
			grid.GetValue(Grid.RowDefinitionsProperty).Add(rowDefinition3);
			rowDefinition4.SetValue(RowDefinition.HeightProperty, new GridLengthTypeConverter().ConvertFromInvariantString("auto"));
			grid.GetValue(Grid.RowDefinitionsProperty).Add(rowDefinition4);
			rowDefinition5.SetValue(RowDefinition.HeightProperty, new GridLengthTypeConverter().ConvertFromInvariantString("*"));
			grid.GetValue(Grid.RowDefinitionsProperty).Add(rowDefinition5);
			rowDefinition6.SetValue(RowDefinition.HeightProperty, new GridLengthTypeConverter().ConvertFromInvariantString("auto"));
			grid.GetValue(Grid.RowDefinitionsProperty).Add(rowDefinition6);
			rowDefinition7.SetValue(RowDefinition.HeightProperty, new GridLengthTypeConverter().ConvertFromInvariantString("auto"));
			grid.GetValue(Grid.RowDefinitionsProperty).Add(rowDefinition7);
			rowDefinition8.SetValue(RowDefinition.HeightProperty, new GridLengthTypeConverter().ConvertFromInvariantString("auto"));
			grid.GetValue(Grid.RowDefinitionsProperty).Add(rowDefinition8);
			label.SetValue(Grid.RowProperty, 0);
			label.SetValue(Label.FontAttributesProperty, new FontAttributesConverter().ConvertFromInvariantString("Bold"));
			label.SetValue(View.HorizontalOptionsProperty, LayoutOptions.Center);
			label.SetValue(Label.HorizontalTextAlignmentProperty, new TextAlignmentConverter().ConvertFromInvariantString("Center"));
			translate.Text = "debug_Crashed";
			IMarkupExtension markupExtension2 = translate;
			XamlServiceProvider xamlServiceProvider2 = new XamlServiceProvider();
			Type typeFromHandle3 = typeof(IProvideValueTarget);
			object[] array2 = new object[0 + 3];
			array2[0] = label;
			array2[1] = grid;
			array2[2] = this;
			object obj2;
			xamlServiceProvider2.Add(typeFromHandle3, obj2 = new SimpleValueTargetProvider(array2, Label.TextProperty, nameScope));
			xamlServiceProvider2.Add(typeof(IReferenceProvider), obj2);
			Type typeFromHandle4 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver2 = new XmlNamespaceResolver();
			xmlNamespaceResolver2.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver2.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver2.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver2.Add("local", "clr-namespace:CarScannerXamarinForms");
			xamlServiceProvider2.Add(typeFromHandle4, new XamlTypeResolver(xmlNamespaceResolver2, typeof(DebugPage).GetTypeInfo().Assembly));
			xamlServiceProvider2.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(30, 17)));
			object obj3 = markupExtension2.ProvideValue(xamlServiceProvider2);
			label.Text = obj3;
			dynamicResourceExtension2.Key = "TextColor";
			IMarkupExtension<DynamicResource> markupExtension3 = dynamicResourceExtension2;
			XamlServiceProvider xamlServiceProvider3 = new XamlServiceProvider();
			Type typeFromHandle5 = typeof(IProvideValueTarget);
			object[] array3 = new object[0 + 3];
			array3[0] = label;
			array3[1] = grid;
			array3[2] = this;
			object obj4;
			xamlServiceProvider3.Add(typeFromHandle5, obj4 = new SimpleValueTargetProvider(array3, Label.TextColorProperty, nameScope));
			xamlServiceProvider3.Add(typeof(IReferenceProvider), obj4);
			Type typeFromHandle6 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver3 = new XmlNamespaceResolver();
			xmlNamespaceResolver3.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver3.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver3.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver3.Add("local", "clr-namespace:CarScannerXamarinForms");
			xamlServiceProvider3.Add(typeFromHandle6, new XamlTypeResolver(xmlNamespaceResolver3, typeof(DebugPage).GetTypeInfo().Assembly));
			xamlServiceProvider3.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(31, 17)));
			DynamicResource dynamicResource2 = markupExtension3.ProvideValue(xamlServiceProvider3);
			label.SetDynamicResource(Label.TextColorProperty, dynamicResource2.Key);
			grid.Children.Add(label);
			label2.SetValue(Grid.RowProperty, 1);
			translate2.Text = "debug_Text";
			IMarkupExtension markupExtension4 = translate2;
			XamlServiceProvider xamlServiceProvider4 = new XamlServiceProvider();
			Type typeFromHandle7 = typeof(IProvideValueTarget);
			object[] array4 = new object[0 + 3];
			array4[0] = label2;
			array4[1] = grid;
			array4[2] = this;
			object obj5;
			xamlServiceProvider4.Add(typeFromHandle7, obj5 = new SimpleValueTargetProvider(array4, Label.TextProperty, nameScope));
			xamlServiceProvider4.Add(typeof(IReferenceProvider), obj5);
			Type typeFromHandle8 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver4 = new XmlNamespaceResolver();
			xmlNamespaceResolver4.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver4.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver4.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver4.Add("local", "clr-namespace:CarScannerXamarinForms");
			xamlServiceProvider4.Add(typeFromHandle8, new XamlTypeResolver(xmlNamespaceResolver4, typeof(DebugPage).GetTypeInfo().Assembly));
			xamlServiceProvider4.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(34, 17)));
			object obj6 = markupExtension4.ProvideValue(xamlServiceProvider4);
			label2.Text = obj6;
			dynamicResourceExtension3.Key = "TextColor";
			IMarkupExtension<DynamicResource> markupExtension5 = dynamicResourceExtension3;
			XamlServiceProvider xamlServiceProvider5 = new XamlServiceProvider();
			Type typeFromHandle9 = typeof(IProvideValueTarget);
			object[] array5 = new object[0 + 3];
			array5[0] = label2;
			array5[1] = grid;
			array5[2] = this;
			object obj7;
			xamlServiceProvider5.Add(typeFromHandle9, obj7 = new SimpleValueTargetProvider(array5, Label.TextColorProperty, nameScope));
			xamlServiceProvider5.Add(typeof(IReferenceProvider), obj7);
			Type typeFromHandle10 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver5 = new XmlNamespaceResolver();
			xmlNamespaceResolver5.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver5.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver5.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver5.Add("local", "clr-namespace:CarScannerXamarinForms");
			xamlServiceProvider5.Add(typeFromHandle10, new XamlTypeResolver(xmlNamespaceResolver5, typeof(DebugPage).GetTypeInfo().Assembly));
			xamlServiceProvider5.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(35, 17)));
			DynamicResource dynamicResource3 = markupExtension5.ProvideValue(xamlServiceProvider5);
			label2.SetDynamicResource(Label.TextColorProperty, dynamicResource3.Key);
			grid.Children.Add(label2);
			label3.SetValue(Grid.RowProperty, 2);
			label3.SetValue(Label.FontAttributesProperty, new FontAttributesConverter().ConvertFromInvariantString("Bold"));
			label3.SetValue(View.HorizontalOptionsProperty, LayoutOptions.Center);
			label3.SetValue(Label.HorizontalTextAlignmentProperty, new TextAlignmentConverter().ConvertFromInvariantString("Center"));
			label3.SetValue(Label.TextProperty, "admin@carscanner.info");
			dynamicResourceExtension4.Key = "TextColor";
			IMarkupExtension<DynamicResource> markupExtension6 = dynamicResourceExtension4;
			XamlServiceProvider xamlServiceProvider6 = new XamlServiceProvider();
			Type typeFromHandle11 = typeof(IProvideValueTarget);
			object[] array6 = new object[0 + 3];
			array6[0] = label3;
			array6[1] = grid;
			array6[2] = this;
			object obj8;
			xamlServiceProvider6.Add(typeFromHandle11, obj8 = new SimpleValueTargetProvider(array6, Label.TextColorProperty, nameScope));
			xamlServiceProvider6.Add(typeof(IReferenceProvider), obj8);
			Type typeFromHandle12 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver6 = new XmlNamespaceResolver();
			xmlNamespaceResolver6.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver6.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver6.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver6.Add("local", "clr-namespace:CarScannerXamarinForms");
			xamlServiceProvider6.Add(typeFromHandle12, new XamlTypeResolver(xmlNamespaceResolver6, typeof(DebugPage).GetTypeInfo().Assembly));
			xamlServiceProvider6.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(42, 17)));
			DynamicResource dynamicResource4 = markupExtension6.ProvideValue(xamlServiceProvider6);
			label3.SetDynamicResource(Label.TextColorProperty, dynamicResource4.Key);
			grid.Children.Add(label3);
			label4.SetValue(Grid.RowProperty, 3);
			translate3.Text = "debug_DebugInfo";
			IMarkupExtension markupExtension7 = translate3;
			XamlServiceProvider xamlServiceProvider7 = new XamlServiceProvider();
			Type typeFromHandle13 = typeof(IProvideValueTarget);
			object[] array7 = new object[0 + 3];
			array7[0] = label4;
			array7[1] = grid;
			array7[2] = this;
			object obj9;
			xamlServiceProvider7.Add(typeFromHandle13, obj9 = new SimpleValueTargetProvider(array7, Label.TextProperty, nameScope));
			xamlServiceProvider7.Add(typeof(IReferenceProvider), obj9);
			Type typeFromHandle14 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver7 = new XmlNamespaceResolver();
			xmlNamespaceResolver7.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver7.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver7.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver7.Add("local", "clr-namespace:CarScannerXamarinForms");
			xamlServiceProvider7.Add(typeFromHandle14, new XamlTypeResolver(xmlNamespaceResolver7, typeof(DebugPage).GetTypeInfo().Assembly));
			xamlServiceProvider7.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(45, 17)));
			object obj10 = markupExtension7.ProvideValue(xamlServiceProvider7);
			label4.Text = obj10;
			dynamicResourceExtension5.Key = "TextColor";
			IMarkupExtension<DynamicResource> markupExtension8 = dynamicResourceExtension5;
			XamlServiceProvider xamlServiceProvider8 = new XamlServiceProvider();
			Type typeFromHandle15 = typeof(IProvideValueTarget);
			object[] array8 = new object[0 + 3];
			array8[0] = label4;
			array8[1] = grid;
			array8[2] = this;
			object obj11;
			xamlServiceProvider8.Add(typeFromHandle15, obj11 = new SimpleValueTargetProvider(array8, Label.TextColorProperty, nameScope));
			xamlServiceProvider8.Add(typeof(IReferenceProvider), obj11);
			Type typeFromHandle16 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver8 = new XmlNamespaceResolver();
			xmlNamespaceResolver8.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver8.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver8.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver8.Add("local", "clr-namespace:CarScannerXamarinForms");
			xamlServiceProvider8.Add(typeFromHandle16, new XamlTypeResolver(xmlNamespaceResolver8, typeof(DebugPage).GetTypeInfo().Assembly));
			xamlServiceProvider8.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(46, 17)));
			DynamicResource dynamicResource5 = markupExtension8.ProvideValue(xamlServiceProvider8);
			label4.SetDynamicResource(Label.TextColorProperty, dynamicResource5.Key);
			grid.Children.Add(label4);
			editor.SetValue(Grid.RowProperty, 4);
			dynamicResourceExtension6.Key = "EntryBackgroundColor";
			IMarkupExtension<DynamicResource> markupExtension9 = dynamicResourceExtension6;
			XamlServiceProvider xamlServiceProvider9 = new XamlServiceProvider();
			Type typeFromHandle17 = typeof(IProvideValueTarget);
			object[] array9 = new object[0 + 3];
			array9[0] = editor;
			array9[1] = grid;
			array9[2] = this;
			object obj12;
			xamlServiceProvider9.Add(typeFromHandle17, obj12 = new SimpleValueTargetProvider(array9, VisualElement.BackgroundColorProperty, nameScope));
			xamlServiceProvider9.Add(typeof(IReferenceProvider), obj12);
			Type typeFromHandle18 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver9 = new XmlNamespaceResolver();
			xmlNamespaceResolver9.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver9.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver9.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver9.Add("local", "clr-namespace:CarScannerXamarinForms");
			xamlServiceProvider9.Add(typeFromHandle18, new XamlTypeResolver(xmlNamespaceResolver9, typeof(DebugPage).GetTypeInfo().Assembly));
			xamlServiceProvider9.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(51, 17)));
			DynamicResource dynamicResource6 = markupExtension9.ProvideValue(xamlServiceProvider9);
			editor.SetDynamicResource(VisualElement.BackgroundColorProperty, dynamicResource6.Key);
			dynamicResourceExtension7.Key = "TextColor";
			IMarkupExtension<DynamicResource> markupExtension10 = dynamicResourceExtension7;
			XamlServiceProvider xamlServiceProvider10 = new XamlServiceProvider();
			Type typeFromHandle19 = typeof(IProvideValueTarget);
			object[] array10 = new object[0 + 3];
			array10[0] = editor;
			array10[1] = grid;
			array10[2] = this;
			object obj13;
			xamlServiceProvider10.Add(typeFromHandle19, obj13 = new SimpleValueTargetProvider(array10, Editor.TextColorProperty, nameScope));
			xamlServiceProvider10.Add(typeof(IReferenceProvider), obj13);
			Type typeFromHandle20 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver10 = new XmlNamespaceResolver();
			xmlNamespaceResolver10.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver10.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver10.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver10.Add("local", "clr-namespace:CarScannerXamarinForms");
			xamlServiceProvider10.Add(typeFromHandle20, new XamlTypeResolver(xmlNamespaceResolver10, typeof(DebugPage).GetTypeInfo().Assembly));
			xamlServiceProvider10.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(52, 17)));
			DynamicResource dynamicResource7 = markupExtension10.ProvideValue(xamlServiceProvider10);
			editor.SetDynamicResource(Editor.TextColorProperty, dynamicResource7.Key);
			grid.Children.Add(editor);
			button.SetValue(Grid.RowProperty, 5);
			button.Clicked += this.btnCopyToClipboard_Clicked;
			translate4.Text = "debug_Clipboard";
			IMarkupExtension markupExtension11 = translate4;
			XamlServiceProvider xamlServiceProvider11 = new XamlServiceProvider();
			Type typeFromHandle21 = typeof(IProvideValueTarget);
			object[] array11 = new object[0 + 3];
			array11[0] = button;
			array11[1] = grid;
			array11[2] = this;
			object obj14;
			xamlServiceProvider11.Add(typeFromHandle21, obj14 = new SimpleValueTargetProvider(array11, Button.TextProperty, nameScope));
			xamlServiceProvider11.Add(typeof(IReferenceProvider), obj14);
			Type typeFromHandle22 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver11 = new XmlNamespaceResolver();
			xmlNamespaceResolver11.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver11.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver11.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver11.Add("local", "clr-namespace:CarScannerXamarinForms");
			xamlServiceProvider11.Add(typeFromHandle22, new XamlTypeResolver(xmlNamespaceResolver11, typeof(DebugPage).GetTypeInfo().Assembly));
			xamlServiceProvider11.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(57, 17)));
			object obj15 = markupExtension11.ProvideValue(xamlServiceProvider11);
			button.Text = obj15;
			dynamicResourceExtension8.Key = "AccentColor";
			IMarkupExtension<DynamicResource> markupExtension12 = dynamicResourceExtension8;
			XamlServiceProvider xamlServiceProvider12 = new XamlServiceProvider();
			Type typeFromHandle23 = typeof(IProvideValueTarget);
			object[] array12 = new object[0 + 3];
			array12[0] = button;
			array12[1] = grid;
			array12[2] = this;
			object obj16;
			xamlServiceProvider12.Add(typeFromHandle23, obj16 = new SimpleValueTargetProvider(array12, Button.TextColorProperty, nameScope));
			xamlServiceProvider12.Add(typeof(IReferenceProvider), obj16);
			Type typeFromHandle24 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver12 = new XmlNamespaceResolver();
			xmlNamespaceResolver12.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver12.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver12.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver12.Add("local", "clr-namespace:CarScannerXamarinForms");
			xamlServiceProvider12.Add(typeFromHandle24, new XamlTypeResolver(xmlNamespaceResolver12, typeof(DebugPage).GetTypeInfo().Assembly));
			xamlServiceProvider12.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(58, 17)));
			DynamicResource dynamicResource8 = markupExtension12.ProvideValue(xamlServiceProvider12);
			button.SetDynamicResource(Button.TextColorProperty, dynamicResource8.Key);
			grid.Children.Add(button);
			button2.SetValue(Grid.RowProperty, 6);
			button2.Clicked += this.SendEmail_Clicked;
			translate5.Text = "debug_SendEmail";
			IMarkupExtension markupExtension13 = translate5;
			XamlServiceProvider xamlServiceProvider13 = new XamlServiceProvider();
			Type typeFromHandle25 = typeof(IProvideValueTarget);
			object[] array13 = new object[0 + 3];
			array13[0] = button2;
			array13[1] = grid;
			array13[2] = this;
			object obj17;
			xamlServiceProvider13.Add(typeFromHandle25, obj17 = new SimpleValueTargetProvider(array13, Button.TextProperty, nameScope));
			xamlServiceProvider13.Add(typeof(IReferenceProvider), obj17);
			Type typeFromHandle26 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver13 = new XmlNamespaceResolver();
			xmlNamespaceResolver13.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver13.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver13.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver13.Add("local", "clr-namespace:CarScannerXamarinForms");
			xamlServiceProvider13.Add(typeFromHandle26, new XamlTypeResolver(xmlNamespaceResolver13, typeof(DebugPage).GetTypeInfo().Assembly));
			xamlServiceProvider13.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(63, 17)));
			object obj18 = markupExtension13.ProvideValue(xamlServiceProvider13);
			button2.Text = obj18;
			dynamicResourceExtension9.Key = "AccentColor";
			IMarkupExtension<DynamicResource> markupExtension14 = dynamicResourceExtension9;
			XamlServiceProvider xamlServiceProvider14 = new XamlServiceProvider();
			Type typeFromHandle27 = typeof(IProvideValueTarget);
			object[] array14 = new object[0 + 3];
			array14[0] = button2;
			array14[1] = grid;
			array14[2] = this;
			object obj19;
			xamlServiceProvider14.Add(typeFromHandle27, obj19 = new SimpleValueTargetProvider(array14, Button.TextColorProperty, nameScope));
			xamlServiceProvider14.Add(typeof(IReferenceProvider), obj19);
			Type typeFromHandle28 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver14 = new XmlNamespaceResolver();
			xmlNamespaceResolver14.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver14.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver14.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver14.Add("local", "clr-namespace:CarScannerXamarinForms");
			xamlServiceProvider14.Add(typeFromHandle28, new XamlTypeResolver(xmlNamespaceResolver14, typeof(DebugPage).GetTypeInfo().Assembly));
			xamlServiceProvider14.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(64, 17)));
			DynamicResource dynamicResource9 = markupExtension14.ProvideValue(xamlServiceProvider14);
			button2.SetDynamicResource(Button.TextColorProperty, dynamicResource9.Key);
			grid.Children.Add(button2);
			button3.SetValue(Grid.RowProperty, 7);
			button3.Clicked += this.Handle_Clicked;
			translate6.Text = "debug_Continue";
			IMarkupExtension markupExtension15 = translate6;
			XamlServiceProvider xamlServiceProvider15 = new XamlServiceProvider();
			Type typeFromHandle29 = typeof(IProvideValueTarget);
			object[] array15 = new object[0 + 3];
			array15[0] = button3;
			array15[1] = grid;
			array15[2] = this;
			object obj20;
			xamlServiceProvider15.Add(typeFromHandle29, obj20 = new SimpleValueTargetProvider(array15, Button.TextProperty, nameScope));
			xamlServiceProvider15.Add(typeof(IReferenceProvider), obj20);
			Type typeFromHandle30 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver15 = new XmlNamespaceResolver();
			xmlNamespaceResolver15.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver15.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver15.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver15.Add("local", "clr-namespace:CarScannerXamarinForms");
			xamlServiceProvider15.Add(typeFromHandle30, new XamlTypeResolver(xmlNamespaceResolver15, typeof(DebugPage).GetTypeInfo().Assembly));
			xamlServiceProvider15.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(69, 17)));
			object obj21 = markupExtension15.ProvideValue(xamlServiceProvider15);
			button3.Text = obj21;
			dynamicResourceExtension10.Key = "AccentColor";
			IMarkupExtension<DynamicResource> markupExtension16 = dynamicResourceExtension10;
			XamlServiceProvider xamlServiceProvider16 = new XamlServiceProvider();
			Type typeFromHandle31 = typeof(IProvideValueTarget);
			object[] array16 = new object[0 + 3];
			array16[0] = button3;
			array16[1] = grid;
			array16[2] = this;
			object obj22;
			xamlServiceProvider16.Add(typeFromHandle31, obj22 = new SimpleValueTargetProvider(array16, Button.TextColorProperty, nameScope));
			xamlServiceProvider16.Add(typeof(IReferenceProvider), obj22);
			Type typeFromHandle32 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver16 = new XmlNamespaceResolver();
			xmlNamespaceResolver16.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver16.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver16.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver16.Add("local", "clr-namespace:CarScannerXamarinForms");
			xamlServiceProvider16.Add(typeFromHandle32, new XamlTypeResolver(xmlNamespaceResolver16, typeof(DebugPage).GetTypeInfo().Assembly));
			xamlServiceProvider16.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(70, 17)));
			DynamicResource dynamicResource10 = markupExtension16.ProvideValue(xamlServiceProvider16);
			button3.SetDynamicResource(Button.TextColorProperty, dynamicResource10.Key);
			grid.Children.Add(button3);
			this.SetValue(ContentPage.ContentProperty, grid);
		}

		// Token: 0x060036CF RID: 14031 RVA: 0x0028A3BC File Offset: 0x002885BC
		private void __InitComponentRuntime()
		{
			Extensions.LoadFromXaml<DebugPage>(this, typeof(DebugPage));
			this.editor = NameScopeExtensions.FindByName<Editor>(this, "editor");
			this.btnCopyClipboard = NameScopeExtensions.FindByName<Button>(this, "btnCopyClipboard");
			this.btnSendEmail = NameScopeExtensions.FindByName<Button>(this, "btnSendEmail");
			this.btnContinue = NameScopeExtensions.FindByName<Button>(this, "btnContinue");
		}

		// Token: 0x04002112 RID: 8466
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private Editor editor;

		// Token: 0x04002113 RID: 8467
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private Button btnCopyClipboard;

		// Token: 0x04002114 RID: 8468
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private Button btnSendEmail;

		// Token: 0x04002115 RID: 8469
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private Button btnContinue;

		// Token: 0x0200061A RID: 1562
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <SendEmail_Clicked>d__3 : IAsyncStateMachine
		{
			// Token: 0x060036D0 RID: 14032 RVA: 0x0028A420 File Offset: 0x00288620
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				try
				{
					TaskAwaiter taskAwaiter;
					if (num != 0)
					{
						taskAwaiter = SharedSettings.Current.SendDebugEmail().GetAwaiter();
						if (!taskAwaiter.IsCompleted)
						{
							num2 = 0;
							TaskAwaiter taskAwaiter2 = taskAwaiter;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, DebugPage.<SendEmail_Clicked>d__3>(ref taskAwaiter, ref this);
							return;
						}
					}
					else
					{
						TaskAwaiter taskAwaiter2;
						taskAwaiter = taskAwaiter2;
						taskAwaiter2 = default(TaskAwaiter);
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

			// Token: 0x060036D1 RID: 14033 RVA: 0x0028A4D0 File Offset: 0x002886D0
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x04002116 RID: 8470
			public int <>1__state;

			// Token: 0x04002117 RID: 8471
			public AsyncVoidMethodBuilder <>t__builder;

			// Token: 0x04002118 RID: 8472
			private TaskAwaiter <>u__1;
		}
	}
}
