using System;
using System.CodeDom.Compiler;
using System.Diagnostics;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using AiForms.Renderers;
using CarScannerXamarinForms.Common;
using Xamarin.Forms;
using Xamarin.Forms.Internals;
using Xamarin.Forms.PlatformConfiguration.iOSSpecific;
using Xamarin.Forms.Xaml;
using Xamarin.Forms.Xaml.Diagnostics;
using Xamarin.Forms.Xaml.Internals;

namespace CarScannerXamarinForms.ProfilesV2
{
	// Token: 0x020002C4 RID: 708
	[XamlCompilation(2)]
	[XamlFilePath("ProfilesV2\\ProfilesUpdatePage.xaml")]
	public class ProfilesUpdatePage : ContentPage
	{
		// Token: 0x0600227B RID: 8827 RVA: 0x001AA111 File Offset: 0x001A8311
		public ProfilesUpdatePage()
		{
			this.InitializeComponent();
			this.Init();
		}

		// Token: 0x0600227C RID: 8828 RVA: 0x001AA134 File Offset: 0x001A8334
		private async Task Init()
		{
			this.cellCurrentVersion.ValueText = ProfileV2Model.GetCurrentVersion();
			ValueTuple<bool, string> valueTuple = await ProfileV2Model.CheckForOnlineUpdatesAvailables();
			bool item = valueTuple.Item1;
			string item2 = valueTuple.Item2;
			this.cellNewVersion.ValueText = item2;
			if (item)
			{
				this.btnUpdate.IsVisible = true;
			}
			if (ProfileV2Model.GetUpdateVersion() != "")
			{
				this.btnDel.IsVisible = true;
				this.btnVerify.IsVisible = true;
			}
			else
			{
				this.btnDel.IsVisible = false;
				this.btnVerify.IsVisible = false;
			}
		}

		// Token: 0x0600227D RID: 8829 RVA: 0x001AA178 File Offset: 0x001A8378
		private async void btnUpdate_Tapped(object sender, EventArgs e)
		{
			this.btnUpdate.IsVisible = false;
			this.btnCancel.IsVisible = true;
			try
			{
				this.cts = new CancellationTokenSource();
				IProgress<string> progress = new Progress<string>(delegate(string s)
				{
					MainThreadHelper.InvokeOnMainThread(delegate
					{
						this.cellProgress.Title = s;
					});
				});
				this.cellProgress.IsVisible = true;
				await ProfileV2Model.DownloadUpdate(progress, this.cts.Token);
			}
			catch (Exception)
			{
				ProfileV2Model.DeleteUpdated();
			}
			this.btnCancel.IsVisible = false;
			this.Init();
		}

		// Token: 0x0600227E RID: 8830 RVA: 0x001AA1AF File Offset: 0x001A83AF
		private void btnDel_Tapped(object sender, EventArgs e)
		{
			ProfileV2Model.DeleteUpdated();
			this.Init();
		}

		// Token: 0x0600227F RID: 8831 RVA: 0x001AA1BD File Offset: 0x001A83BD
		private void btnCancel_Tapped(object sender, EventArgs e)
		{
			this.cts.Cancel();
			this.btnCancel.IsVisible = false;
			this.cellProgress.IsVisible = false;
		}

		// Token: 0x06002280 RID: 8832 RVA: 0x001AA1E4 File Offset: 0x001A83E4
		private async void btnVerify_Tapped(object sender, EventArgs e)
		{
			if (ProfileV2Model.VerifyUpdated())
			{
				await base.DisplayAlert("Verify: OK", "", "OK");
			}
			else
			{
				await base.DisplayAlert("Verify: Fail", "", "OK");
			}
		}

		// Token: 0x06002281 RID: 8833 RVA: 0x001AA21C File Offset: 0x001A841C
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private void InitializeComponent()
		{
			if (ResourceLoader.IsEnabled && ResourceLoader.CanProvideContentFor(new ResourceLoader.ResourceLoadingQuery
			{
				AssemblyName = typeof(ProfilesUpdatePage).GetTypeInfo().Assembly.GetName(),
				ResourcePath = "ProfilesV2/ProfilesUpdatePage.xaml",
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
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension = new DynamicResourceExtension(), new Uri("ProfilesV2\\ProfilesUpdatePage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 14, 5);
			BoolToNegativeConverter boolToNegativeConverter;
			VisualDiagnostics.RegisterSourceInfo(boolToNegativeConverter = new BoolToNegativeConverter(), new Uri("ProfilesV2\\ProfilesUpdatePage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 17, 14);
			ResourceDictionary resourceDictionary;
			VisualDiagnostics.RegisterSourceInfo(resourceDictionary = new ResourceDictionary(), new Uri("ProfilesV2\\ProfilesUpdatePage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 16, 10);
			ReferenceExtension referenceExtension;
			VisualDiagnostics.RegisterSourceInfo(referenceExtension = new ReferenceExtension(), new Uri("ProfilesV2\\ProfilesUpdatePage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 25, 13);
			LabelCell labelCell;
			VisualDiagnostics.RegisterSourceInfo(labelCell = new LabelCell(), new Uri("ProfilesV2\\ProfilesUpdatePage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 29, 18);
			LabelCell labelCell2;
			VisualDiagnostics.RegisterSourceInfo(labelCell2 = new LabelCell(), new Uri("ProfilesV2\\ProfilesUpdatePage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 30, 18);
			DynamicResourceExtension dynamicResourceExtension2;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension2 = new DynamicResourceExtension(), new Uri("ProfilesV2\\ProfilesUpdatePage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 36, 21);
			ButtonCell buttonCell;
			VisualDiagnostics.RegisterSourceInfo(buttonCell = new ButtonCell(), new Uri("ProfilesV2\\ProfilesUpdatePage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 31, 18);
			DynamicResourceExtension dynamicResourceExtension3;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension3 = new DynamicResourceExtension(), new Uri("ProfilesV2\\ProfilesUpdatePage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 41, 21);
			ButtonCell buttonCell2;
			VisualDiagnostics.RegisterSourceInfo(buttonCell2 = new ButtonCell(), new Uri("ProfilesV2\\ProfilesUpdatePage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 37, 18);
			DynamicResourceExtension dynamicResourceExtension4;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension4 = new DynamicResourceExtension(), new Uri("ProfilesV2\\ProfilesUpdatePage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 48, 21);
			ButtonCell buttonCell3;
			VisualDiagnostics.RegisterSourceInfo(buttonCell3 = new ButtonCell(), new Uri("ProfilesV2\\ProfilesUpdatePage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 43, 18);
			DynamicResourceExtension dynamicResourceExtension5;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension5 = new DynamicResourceExtension(), new Uri("ProfilesV2\\ProfilesUpdatePage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 55, 21);
			ButtonCell buttonCell4;
			VisualDiagnostics.RegisterSourceInfo(buttonCell4 = new ButtonCell(), new Uri("ProfilesV2\\ProfilesUpdatePage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 50, 18);
			DynamicResourceExtension dynamicResourceExtension6;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension6 = new DynamicResourceExtension(), new Uri("ProfilesV2\\ProfilesUpdatePage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 62, 21);
			ButtonCell buttonCell5;
			VisualDiagnostics.RegisterSourceInfo(buttonCell5 = new ButtonCell(), new Uri("ProfilesV2\\ProfilesUpdatePage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 57, 18);
			Section section;
			VisualDiagnostics.RegisterSourceInfo(section = new Section(), new Uri("ProfilesV2\\ProfilesUpdatePage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 28, 14);
			SettingsView settingsView;
			VisualDiagnostics.RegisterSourceInfo(settingsView = new SettingsView(), new Uri("ProfilesV2\\ProfilesUpdatePage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 23, 10);
			VisualDiagnostics.RegisterSourceInfo(this, new Uri("ProfilesV2\\ProfilesUpdatePage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 2, 2);
			NameScope nameScope = NameScope.GetNameScope(this) ?? new NameScope();
			NameScope.SetNameScope(this, nameScope);
			nameScope.RegisterName("page", this);
			if (this.StyleId == null)
			{
				this.StyleId = "page";
			}
			nameScope.RegisterName("settingsRoot", settingsView);
			if (settingsView.StyleId == null)
			{
				settingsView.StyleId = "settingsRoot";
			}
			nameScope.RegisterName("cellCurrentVersion", labelCell);
			if (labelCell.StyleId == null)
			{
				labelCell.StyleId = "cellCurrentVersion";
			}
			nameScope.RegisterName("cellNewVersion", labelCell2);
			if (labelCell2.StyleId == null)
			{
				labelCell2.StyleId = "cellNewVersion";
			}
			nameScope.RegisterName("btnUpdate", buttonCell);
			if (buttonCell.StyleId == null)
			{
				buttonCell.StyleId = "btnUpdate";
			}
			nameScope.RegisterName("cellProgress", buttonCell2);
			if (buttonCell2.StyleId == null)
			{
				buttonCell2.StyleId = "cellProgress";
			}
			nameScope.RegisterName("btnCancel", buttonCell3);
			if (buttonCell3.StyleId == null)
			{
				buttonCell3.StyleId = "btnCancel";
			}
			nameScope.RegisterName("btnDel", buttonCell4);
			if (buttonCell4.StyleId == null)
			{
				buttonCell4.StyleId = "btnDel";
			}
			nameScope.RegisterName("btnVerify", buttonCell5);
			if (buttonCell5.StyleId == null)
			{
				buttonCell5.StyleId = "btnVerify";
			}
			this.page = this;
			this.settingsRoot = settingsView;
			this.cellCurrentVersion = labelCell;
			this.cellNewVersion = labelCell2;
			this.btnUpdate = buttonCell;
			this.cellProgress = buttonCell2;
			this.btnCancel = buttonCell3;
			this.btnDel = buttonCell4;
			this.btnVerify = buttonCell5;
			this.Resources = resourceDictionary;
			resourceDictionary.Add("BoolToNegativeConverter", boolToNegativeConverter);
			this.SetValue(Page.TitleProperty, "Profiles update");
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
			xmlNamespaceResolver.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xamlServiceProvider.Add(typeFromHandle2, new XamlTypeResolver(xmlNamespaceResolver, typeof(ProfilesUpdatePage).GetTypeInfo().Assembly));
			xamlServiceProvider.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(14, 5)));
			DynamicResource dynamicResource = markupExtension.ProvideValue(xamlServiceProvider);
			this.SetDynamicResource(VisualElement.BackgroundColorProperty, dynamicResource.Key);
			this.Resources = resourceDictionary;
			referenceExtension.Name = "page";
			IMarkupExtension markupExtension2 = referenceExtension;
			XamlServiceProvider xamlServiceProvider2 = new XamlServiceProvider();
			Type typeFromHandle3 = typeof(IProvideValueTarget);
			object[] array2 = new object[0 + 2];
			array2[0] = settingsView;
			array2[1] = this;
			object obj2;
			xamlServiceProvider2.Add(typeFromHandle3, obj2 = new SimpleValueTargetProvider(array2, BindableObject.BindingContextProperty, nameScope));
			xamlServiceProvider2.Add(typeof(IReferenceProvider), obj2);
			Type typeFromHandle4 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver2 = new XmlNamespaceResolver();
			xmlNamespaceResolver2.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver2.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver2.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver2.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver2.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver2.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver2.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xamlServiceProvider2.Add(typeFromHandle4, new XamlTypeResolver(xmlNamespaceResolver2, typeof(ProfilesUpdatePage).GetTypeInfo().Assembly));
			xamlServiceProvider2.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(25, 13)));
			object obj3 = markupExtension2.ProvideValue(xamlServiceProvider2);
			settingsView.SetValue(BindableObject.BindingContextProperty, obj3);
			settingsView.SetValue(TableView.HasUnevenRowsProperty, true);
			settingsView.SetValue(SettingsView.HeaderHeightProperty, 0.0);
			section.SetValue(SectionBase.TitleProperty, "");
			labelCell.SetValue(CellBase.TitleProperty, "Current version:");
			section.Add(labelCell);
			labelCell2.SetValue(CellBase.TitleProperty, "New version:");
			section.Add(labelCell2);
			buttonCell.SetValue(CellBase.TitleProperty, "Update");
			buttonCell.SetValue(CellBase.IsVisibleProperty, false);
			buttonCell.Tapped += this.btnUpdate_Tapped;
			dynamicResourceExtension2.Key = "ButtonAccentColor";
			IMarkupExtension<DynamicResource> markupExtension3 = dynamicResourceExtension2;
			XamlServiceProvider xamlServiceProvider3 = new XamlServiceProvider();
			Type typeFromHandle5 = typeof(IProvideValueTarget);
			object[] array3 = new object[0 + 4];
			array3[0] = buttonCell;
			array3[1] = section;
			array3[2] = settingsView;
			array3[3] = this;
			object obj4;
			xamlServiceProvider3.Add(typeFromHandle5, obj4 = new SimpleValueTargetProvider(array3, CellBase.TitleColorProperty, nameScope));
			xamlServiceProvider3.Add(typeof(IReferenceProvider), obj4);
			Type typeFromHandle6 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver3 = new XmlNamespaceResolver();
			xmlNamespaceResolver3.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver3.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver3.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver3.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver3.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver3.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver3.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xamlServiceProvider3.Add(typeFromHandle6, new XamlTypeResolver(xmlNamespaceResolver3, typeof(ProfilesUpdatePage).GetTypeInfo().Assembly));
			xamlServiceProvider3.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(36, 21)));
			DynamicResource dynamicResource2 = markupExtension3.ProvideValue(xamlServiceProvider3);
			buttonCell.SetDynamicResource(CellBase.TitleColorProperty, dynamicResource2.Key);
			section.Add(buttonCell);
			buttonCell2.SetValue(CellBase.TitleProperty, "");
			buttonCell2.SetValue(CellBase.IsVisibleProperty, false);
			dynamicResourceExtension3.Key = "SettingsCellTitleColor";
			IMarkupExtension<DynamicResource> markupExtension4 = dynamicResourceExtension3;
			XamlServiceProvider xamlServiceProvider4 = new XamlServiceProvider();
			Type typeFromHandle7 = typeof(IProvideValueTarget);
			object[] array4 = new object[0 + 4];
			array4[0] = buttonCell2;
			array4[1] = section;
			array4[2] = settingsView;
			array4[3] = this;
			object obj5;
			xamlServiceProvider4.Add(typeFromHandle7, obj5 = new SimpleValueTargetProvider(array4, CellBase.TitleColorProperty, nameScope));
			xamlServiceProvider4.Add(typeof(IReferenceProvider), obj5);
			Type typeFromHandle8 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver4 = new XmlNamespaceResolver();
			xmlNamespaceResolver4.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver4.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver4.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver4.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver4.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver4.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver4.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xamlServiceProvider4.Add(typeFromHandle8, new XamlTypeResolver(xmlNamespaceResolver4, typeof(ProfilesUpdatePage).GetTypeInfo().Assembly));
			xamlServiceProvider4.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(41, 21)));
			DynamicResource dynamicResource3 = markupExtension4.ProvideValue(xamlServiceProvider4);
			buttonCell2.SetDynamicResource(CellBase.TitleColorProperty, dynamicResource3.Key);
			section.Add(buttonCell2);
			buttonCell3.SetValue(CellBase.TitleProperty, "Cancel");
			buttonCell3.SetValue(CellBase.IsVisibleProperty, false);
			buttonCell3.Tapped += this.btnCancel_Tapped;
			dynamicResourceExtension4.Key = "ButtonAccentColor";
			IMarkupExtension<DynamicResource> markupExtension5 = dynamicResourceExtension4;
			XamlServiceProvider xamlServiceProvider5 = new XamlServiceProvider();
			Type typeFromHandle9 = typeof(IProvideValueTarget);
			object[] array5 = new object[0 + 4];
			array5[0] = buttonCell3;
			array5[1] = section;
			array5[2] = settingsView;
			array5[3] = this;
			object obj6;
			xamlServiceProvider5.Add(typeFromHandle9, obj6 = new SimpleValueTargetProvider(array5, CellBase.TitleColorProperty, nameScope));
			xamlServiceProvider5.Add(typeof(IReferenceProvider), obj6);
			Type typeFromHandle10 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver5 = new XmlNamespaceResolver();
			xmlNamespaceResolver5.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver5.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver5.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver5.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver5.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver5.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver5.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xamlServiceProvider5.Add(typeFromHandle10, new XamlTypeResolver(xmlNamespaceResolver5, typeof(ProfilesUpdatePage).GetTypeInfo().Assembly));
			xamlServiceProvider5.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(48, 21)));
			DynamicResource dynamicResource4 = markupExtension5.ProvideValue(xamlServiceProvider5);
			buttonCell3.SetDynamicResource(CellBase.TitleColorProperty, dynamicResource4.Key);
			section.Add(buttonCell3);
			buttonCell4.SetValue(CellBase.TitleProperty, "Delete update");
			buttonCell4.SetValue(CellBase.IsVisibleProperty, false);
			buttonCell4.Tapped += this.btnDel_Tapped;
			dynamicResourceExtension5.Key = "ButtonAccentColor";
			IMarkupExtension<DynamicResource> markupExtension6 = dynamicResourceExtension5;
			XamlServiceProvider xamlServiceProvider6 = new XamlServiceProvider();
			Type typeFromHandle11 = typeof(IProvideValueTarget);
			object[] array6 = new object[0 + 4];
			array6[0] = buttonCell4;
			array6[1] = section;
			array6[2] = settingsView;
			array6[3] = this;
			object obj7;
			xamlServiceProvider6.Add(typeFromHandle11, obj7 = new SimpleValueTargetProvider(array6, CellBase.TitleColorProperty, nameScope));
			xamlServiceProvider6.Add(typeof(IReferenceProvider), obj7);
			Type typeFromHandle12 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver6 = new XmlNamespaceResolver();
			xmlNamespaceResolver6.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver6.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver6.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver6.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver6.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver6.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver6.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xamlServiceProvider6.Add(typeFromHandle12, new XamlTypeResolver(xmlNamespaceResolver6, typeof(ProfilesUpdatePage).GetTypeInfo().Assembly));
			xamlServiceProvider6.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(55, 21)));
			DynamicResource dynamicResource5 = markupExtension6.ProvideValue(xamlServiceProvider6);
			buttonCell4.SetDynamicResource(CellBase.TitleColorProperty, dynamicResource5.Key);
			section.Add(buttonCell4);
			buttonCell5.SetValue(CellBase.TitleProperty, "Verify update");
			buttonCell5.SetValue(CellBase.IsVisibleProperty, false);
			buttonCell5.Tapped += this.btnVerify_Tapped;
			dynamicResourceExtension6.Key = "ButtonAccentColor";
			IMarkupExtension<DynamicResource> markupExtension7 = dynamicResourceExtension6;
			XamlServiceProvider xamlServiceProvider7 = new XamlServiceProvider();
			Type typeFromHandle13 = typeof(IProvideValueTarget);
			object[] array7 = new object[0 + 4];
			array7[0] = buttonCell5;
			array7[1] = section;
			array7[2] = settingsView;
			array7[3] = this;
			object obj8;
			xamlServiceProvider7.Add(typeFromHandle13, obj8 = new SimpleValueTargetProvider(array7, CellBase.TitleColorProperty, nameScope));
			xamlServiceProvider7.Add(typeof(IReferenceProvider), obj8);
			Type typeFromHandle14 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver7 = new XmlNamespaceResolver();
			xmlNamespaceResolver7.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver7.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver7.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver7.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver7.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver7.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver7.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xamlServiceProvider7.Add(typeFromHandle14, new XamlTypeResolver(xmlNamespaceResolver7, typeof(ProfilesUpdatePage).GetTypeInfo().Assembly));
			xamlServiceProvider7.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(62, 21)));
			DynamicResource dynamicResource6 = markupExtension7.ProvideValue(xamlServiceProvider7);
			buttonCell5.SetDynamicResource(CellBase.TitleColorProperty, dynamicResource6.Key);
			section.Add(buttonCell5);
			settingsView.Root.Add(section);
			this.SetValue(ContentPage.ContentProperty, settingsView);
		}

		// Token: 0x06002282 RID: 8834 RVA: 0x001AB1B3 File Offset: 0x001A93B3
		[CompilerGenerated]
		private void <btnUpdate_Tapped>b__3_0(string s)
		{
			MainThreadHelper.InvokeOnMainThread(delegate
			{
				this.cellProgress.Title = s;
			});
		}

		// Token: 0x06002283 RID: 8835 RVA: 0x001AB1D8 File Offset: 0x001A93D8
		private void __InitComponentRuntime()
		{
			Extensions.LoadFromXaml<ProfilesUpdatePage>(this, typeof(ProfilesUpdatePage));
			this.page = NameScopeExtensions.FindByName<ContentPage>(this, "page");
			this.settingsRoot = NameScopeExtensions.FindByName<SettingsView>(this, "settingsRoot");
			this.cellCurrentVersion = NameScopeExtensions.FindByName<LabelCell>(this, "cellCurrentVersion");
			this.cellNewVersion = NameScopeExtensions.FindByName<LabelCell>(this, "cellNewVersion");
			this.btnUpdate = NameScopeExtensions.FindByName<ButtonCell>(this, "btnUpdate");
			this.cellProgress = NameScopeExtensions.FindByName<ButtonCell>(this, "cellProgress");
			this.btnCancel = NameScopeExtensions.FindByName<ButtonCell>(this, "btnCancel");
			this.btnDel = NameScopeExtensions.FindByName<ButtonCell>(this, "btnDel");
			this.btnVerify = NameScopeExtensions.FindByName<ButtonCell>(this, "btnVerify");
		}

		// Token: 0x04001071 RID: 4209
		private CancellationTokenSource cts = new CancellationTokenSource();

		// Token: 0x04001072 RID: 4210
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private ContentPage page;

		// Token: 0x04001073 RID: 4211
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private SettingsView settingsRoot;

		// Token: 0x04001074 RID: 4212
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private LabelCell cellCurrentVersion;

		// Token: 0x04001075 RID: 4213
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private LabelCell cellNewVersion;

		// Token: 0x04001076 RID: 4214
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private ButtonCell btnUpdate;

		// Token: 0x04001077 RID: 4215
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private ButtonCell cellProgress;

		// Token: 0x04001078 RID: 4216
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private ButtonCell btnCancel;

		// Token: 0x04001079 RID: 4217
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private ButtonCell btnDel;

		// Token: 0x0400107A RID: 4218
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private ButtonCell btnVerify;

		// Token: 0x020002C5 RID: 709
		[CompilerGenerated]
		private sealed class <>c__DisplayClass3_0
		{
			// Token: 0x06002284 RID: 8836 RVA: 0x00002050 File Offset: 0x00000250
			public <>c__DisplayClass3_0()
			{
			}

			// Token: 0x06002285 RID: 8837 RVA: 0x001AB28F File Offset: 0x001A948F
			internal void <btnUpdate_Tapped>b__1()
			{
				this.<>4__this.cellProgress.Title = this.s;
			}

			// Token: 0x0400107B RID: 4219
			public string s;

			// Token: 0x0400107C RID: 4220
			public ProfilesUpdatePage <>4__this;
		}

		// Token: 0x020002C6 RID: 710
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <Init>d__2 : IAsyncStateMachine
		{
			// Token: 0x06002286 RID: 8838 RVA: 0x001AB2A8 File Offset: 0x001A94A8
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				ProfilesUpdatePage profilesUpdatePage = this;
				try
				{
					TaskAwaiter<ValueTuple<bool, string>> taskAwaiter;
					if (num != 0)
					{
						profilesUpdatePage.cellCurrentVersion.ValueText = ProfileV2Model.GetCurrentVersion();
						taskAwaiter = ProfileV2Model.CheckForOnlineUpdatesAvailables().GetAwaiter();
						if (!taskAwaiter.IsCompleted)
						{
							num2 = 0;
							TaskAwaiter<ValueTuple<bool, string>> taskAwaiter2 = taskAwaiter;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<ValueTuple<bool, string>>, ProfilesUpdatePage.<Init>d__2>(ref taskAwaiter, ref this);
							return;
						}
					}
					else
					{
						TaskAwaiter<ValueTuple<bool, string>> taskAwaiter2;
						taskAwaiter = taskAwaiter2;
						taskAwaiter2 = default(TaskAwaiter<ValueTuple<bool, string>>);
						num2 = -1;
					}
					ValueTuple<bool, string> result = taskAwaiter.GetResult();
					bool item = result.Item1;
					string item2 = result.Item2;
					profilesUpdatePage.cellNewVersion.ValueText = item2;
					if (item)
					{
						profilesUpdatePage.btnUpdate.IsVisible = true;
					}
					if (ProfileV2Model.GetUpdateVersion() != "")
					{
						profilesUpdatePage.btnDel.IsVisible = true;
						profilesUpdatePage.btnVerify.IsVisible = true;
					}
					else
					{
						profilesUpdatePage.btnDel.IsVisible = false;
						profilesUpdatePage.btnVerify.IsVisible = false;
					}
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

			// Token: 0x06002287 RID: 8839 RVA: 0x001AB3DC File Offset: 0x001A95DC
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x0400107D RID: 4221
			public int <>1__state;

			// Token: 0x0400107E RID: 4222
			public AsyncTaskMethodBuilder <>t__builder;

			// Token: 0x0400107F RID: 4223
			public ProfilesUpdatePage <>4__this;

			// Token: 0x04001080 RID: 4224
			private TaskAwaiter<ValueTuple<bool, string>> <>u__1;
		}

		// Token: 0x020002C7 RID: 711
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <btnUpdate_Tapped>d__3 : IAsyncStateMachine
		{
			// Token: 0x06002288 RID: 8840 RVA: 0x001AB3EC File Offset: 0x001A95EC
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				ProfilesUpdatePage profilesUpdatePage = this;
				try
				{
					if (num != 0)
					{
						profilesUpdatePage.btnUpdate.IsVisible = false;
						profilesUpdatePage.btnCancel.IsVisible = true;
					}
					try
					{
						TaskAwaiter<bool> taskAwaiter;
						if (num != 0)
						{
							profilesUpdatePage.cts = new CancellationTokenSource();
							IProgress<string> progress = new Progress<string>(delegate(string s)
							{
								MainThreadHelper.InvokeOnMainThread(new Action(new ProfilesUpdatePage.<>c__DisplayClass3_0
								{
									<>4__this = profilesUpdatePage,
									s = s
								}.<btnUpdate_Tapped>b__1));
							});
							profilesUpdatePage.cellProgress.IsVisible = true;
							taskAwaiter = ProfileV2Model.DownloadUpdate(progress, profilesUpdatePage.cts.Token).GetAwaiter();
							if (!taskAwaiter.IsCompleted)
							{
								num2 = 0;
								TaskAwaiter<bool> taskAwaiter2 = taskAwaiter;
								this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<bool>, ProfilesUpdatePage.<btnUpdate_Tapped>d__3>(ref taskAwaiter, ref this);
								return;
							}
						}
						else
						{
							TaskAwaiter<bool> taskAwaiter2;
							taskAwaiter = taskAwaiter2;
							taskAwaiter2 = default(TaskAwaiter<bool>);
							num2 = -1;
						}
						taskAwaiter.GetResult();
					}
					catch (Exception)
					{
						ProfileV2Model.DeleteUpdated();
					}
					profilesUpdatePage.btnCancel.IsVisible = false;
					profilesUpdatePage.Init();
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

			// Token: 0x06002289 RID: 8841 RVA: 0x001AB518 File Offset: 0x001A9718
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x04001081 RID: 4225
			public int <>1__state;

			// Token: 0x04001082 RID: 4226
			public AsyncVoidMethodBuilder <>t__builder;

			// Token: 0x04001083 RID: 4227
			public ProfilesUpdatePage <>4__this;

			// Token: 0x04001084 RID: 4228
			private TaskAwaiter<bool> <>u__1;
		}

		// Token: 0x020002C8 RID: 712
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <btnVerify_Tapped>d__6 : IAsyncStateMachine
		{
			// Token: 0x0600228A RID: 8842 RVA: 0x001AB528 File Offset: 0x001A9728
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				ProfilesUpdatePage profilesUpdatePage = this;
				try
				{
					TaskAwaiter taskAwaiter;
					TaskAwaiter taskAwaiter2;
					if (num != 0)
					{
						if (num != 1)
						{
							if (ProfileV2Model.VerifyUpdated())
							{
								taskAwaiter = profilesUpdatePage.DisplayAlert("Verify: OK", "", "OK").GetAwaiter();
								if (!taskAwaiter.IsCompleted)
								{
									num2 = 0;
									taskAwaiter2 = taskAwaiter;
									this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, ProfilesUpdatePage.<btnVerify_Tapped>d__6>(ref taskAwaiter, ref this);
									return;
								}
								goto IL_0082;
							}
							else
							{
								taskAwaiter = profilesUpdatePage.DisplayAlert("Verify: Fail", "", "OK").GetAwaiter();
								if (!taskAwaiter.IsCompleted)
								{
									num2 = 1;
									taskAwaiter2 = taskAwaiter;
									this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, ProfilesUpdatePage.<btnVerify_Tapped>d__6>(ref taskAwaiter, ref this);
									return;
								}
							}
						}
						else
						{
							taskAwaiter = taskAwaiter2;
							taskAwaiter2 = default(TaskAwaiter);
							num2 = -1;
						}
						taskAwaiter.GetResult();
						goto IL_00F2;
					}
					taskAwaiter = taskAwaiter2;
					taskAwaiter2 = default(TaskAwaiter);
					num2 = -1;
					IL_0082:
					taskAwaiter.GetResult();
					IL_00F2:;
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

			// Token: 0x0600228B RID: 8843 RVA: 0x001AB664 File Offset: 0x001A9864
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x04001085 RID: 4229
			public int <>1__state;

			// Token: 0x04001086 RID: 4230
			public AsyncVoidMethodBuilder <>t__builder;

			// Token: 0x04001087 RID: 4231
			public ProfilesUpdatePage <>4__this;

			// Token: 0x04001088 RID: 4232
			private TaskAwaiter <>u__1;
		}
	}
}
