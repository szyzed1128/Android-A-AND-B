using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.CompilerServices;
using AiForms.Renderers;
using CarScannerXamarinForms.Common.XAMLConverters;
using CarScannerXamarinForms.OBD2;
using Xamarin.Forms;
using Xamarin.Forms.Internals;
using Xamarin.Forms.PlatformConfiguration.iOSSpecific;
using Xamarin.Forms.Xaml;
using Xamarin.Forms.Xaml.Diagnostics;
using Xamarin.Forms.Xaml.Internals;

namespace CarScannerXamarinForms.Settings
{
	// Token: 0x020001FE RID: 510
	[XamlCompilation(2)]
	[XamlFilePath("Settings\\ELMFailPage.xaml")]
	public class ELMFailPage : ContentPage
	{
		// Token: 0x06001A3B RID: 6715 RVA: 0x00120EE9 File Offset: 0x0011F0E9
		public ELMFailPage()
		{
			this.InitializeComponent();
			this.settingsLayoutRoot.BindingContext = App.OBDReader.ELMStatus;
		}

		// Token: 0x06001A3C RID: 6716 RVA: 0x00120F0C File Offset: 0x0011F10C
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private void InitializeComponent()
		{
			if (ResourceLoader.IsEnabled && ResourceLoader.CanProvideContentFor(new ResourceLoader.ResourceLoadingQuery
			{
				AssemblyName = typeof(ELMFailPage).GetTypeInfo().Assembly.GetName(),
				ResourcePath = "Settings/ELMFailPage.xaml",
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
			Translate translate;
			VisualDiagnostics.RegisterSourceInfo(translate = new Translate(), new Uri("Settings\\ELMFailPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 13, 5);
			DynamicResourceExtension dynamicResourceExtension;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension = new DynamicResourceExtension(), new Uri("Settings\\ELMFailPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 15, 5);
			StringIENumerableToStringConverter stringIENumerableToStringConverter;
			VisualDiagnostics.RegisterSourceInfo(stringIENumerableToStringConverter = new StringIENumerableToStringConverter(), new Uri("Settings\\ELMFailPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 18, 14);
			ResourceDictionary resourceDictionary;
			VisualDiagnostics.RegisterSourceInfo(resourceDictionary = new ResourceDictionary(), new Uri("Settings\\ELMFailPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 17, 10);
			Translate translate2;
			VisualDiagnostics.RegisterSourceInfo(translate2 = new Translate(), new Uri("Settings\\ELMFailPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 27, 31);
			BindingExtension bindingExtension;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension = new BindingExtension(), new Uri("Settings\\ELMFailPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 27, 101);
			LabelCell labelCell;
			VisualDiagnostics.RegisterSourceInfo(labelCell = new LabelCell(), new Uri("Settings\\ELMFailPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 27, 18);
			Translate translate3;
			VisualDiagnostics.RegisterSourceInfo(translate3 = new Translate(), new Uri("Settings\\ELMFailPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 28, 31);
			BindingExtension bindingExtension2;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension2 = new BindingExtension(), new Uri("Settings\\ELMFailPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 28, 97);
			LabelCell labelCell2;
			VisualDiagnostics.RegisterSourceInfo(labelCell2 = new LabelCell(), new Uri("Settings\\ELMFailPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 28, 18);
			Translate translate4;
			VisualDiagnostics.RegisterSourceInfo(translate4 = new Translate(), new Uri("Settings\\ELMFailPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 29, 31);
			BindingExtension bindingExtension3;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension3 = new BindingExtension(), new Uri("Settings\\ELMFailPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 29, 109);
			LabelCell labelCell3;
			VisualDiagnostics.RegisterSourceInfo(labelCell3 = new LabelCell(), new Uri("Settings\\ELMFailPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 29, 18);
			Section section;
			VisualDiagnostics.RegisterSourceInfo(section = new Section(), new Uri("Settings\\ELMFailPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 26, 14);
			Translate translate5;
			VisualDiagnostics.RegisterSourceInfo(translate5 = new Translate(), new Uri("Settings\\ELMFailPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 31, 25);
			StaticResourceExtension staticResourceExtension;
			VisualDiagnostics.RegisterSourceInfo(staticResourceExtension = new StaticResourceExtension(), new Uri("Settings\\ELMFailPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 33, 28);
			BindingExtension bindingExtension4;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension4 = new BindingExtension(), new Uri("Settings\\ELMFailPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 33, 28);
			Label label;
			VisualDiagnostics.RegisterSourceInfo(label = new Label(), new Uri("Settings\\ELMFailPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 33, 22);
			CustomCell customCell;
			VisualDiagnostics.RegisterSourceInfo(customCell = new CustomCell(), new Uri("Settings\\ELMFailPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 32, 18);
			Section section2;
			VisualDiagnostics.RegisterSourceInfo(section2 = new Section(), new Uri("Settings\\ELMFailPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 31, 14);
			SettingsView settingsView;
			VisualDiagnostics.RegisterSourceInfo(settingsView = new SettingsView(), new Uri("Settings\\ELMFailPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 22, 10);
			VisualDiagnostics.RegisterSourceInfo(this, new Uri("Settings\\ELMFailPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 2, 2);
			NameScope nameScope = NameScope.GetNameScope(this) ?? new NameScope();
			NameScope.SetNameScope(this, nameScope);
			nameScope.RegisterName("settingsLayoutRoot", settingsView);
			if (settingsView.StyleId == null)
			{
				settingsView.StyleId = "settingsLayoutRoot";
			}
			this.settingsLayoutRoot = settingsView;
			this.Resources = resourceDictionary;
			resourceDictionary.Add("StringIENumerableToStringConverter", stringIENumerableToStringConverter);
			translate.Text = "settings_ELM_Errors";
			IMarkupExtension markupExtension = translate;
			XamlServiceProvider xamlServiceProvider = new XamlServiceProvider();
			Type typeFromHandle = typeof(IProvideValueTarget);
			object[] array = new object[0 + 1];
			array[0] = this;
			object obj;
			xamlServiceProvider.Add(typeFromHandle, obj = new SimpleValueTargetProvider(array, Page.TitleProperty, nameScope));
			xamlServiceProvider.Add(typeof(IReferenceProvider), obj);
			Type typeFromHandle2 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver = new XmlNamespaceResolver();
			xmlNamespaceResolver.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver.Add("obd2", "clr-namespace:CarScannerXamarinForms.OBD2");
			xmlNamespaceResolver.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xmlNamespaceResolver.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider.Add(typeFromHandle2, new XamlTypeResolver(xmlNamespaceResolver, typeof(ELMFailPage).GetTypeInfo().Assembly));
			xamlServiceProvider.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(13, 5)));
			object obj2 = markupExtension.ProvideValue(xamlServiceProvider);
			this.Title = obj2;
			this.SetValue(Page.UseSafeAreaProperty, true);
			dynamicResourceExtension.Key = "BackgroundColor";
			IMarkupExtension<DynamicResource> markupExtension2 = dynamicResourceExtension;
			XamlServiceProvider xamlServiceProvider2 = new XamlServiceProvider();
			Type typeFromHandle3 = typeof(IProvideValueTarget);
			object[] array2 = new object[0 + 1];
			array2[0] = this;
			object obj3;
			xamlServiceProvider2.Add(typeFromHandle3, obj3 = new SimpleValueTargetProvider(array2, VisualElement.BackgroundColorProperty, nameScope));
			xamlServiceProvider2.Add(typeof(IReferenceProvider), obj3);
			Type typeFromHandle4 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver2 = new XmlNamespaceResolver();
			xmlNamespaceResolver2.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver2.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver2.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver2.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver2.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver2.Add("obd2", "clr-namespace:CarScannerXamarinForms.OBD2");
			xmlNamespaceResolver2.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver2.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xmlNamespaceResolver2.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider2.Add(typeFromHandle4, new XamlTypeResolver(xmlNamespaceResolver2, typeof(ELMFailPage).GetTypeInfo().Assembly));
			xamlServiceProvider2.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(15, 5)));
			DynamicResource dynamicResource = markupExtension2.ProvideValue(xamlServiceProvider2);
			this.SetDynamicResource(VisualElement.BackgroundColorProperty, dynamicResource.Key);
			this.Resources = resourceDictionary;
			settingsView.SetValue(TableView.HasUnevenRowsProperty, true);
			section.SetValue(SectionBase.TitleProperty, "");
			translate2.Text = "settings_ELM_Errors_NoResponseReceived";
			IMarkupExtension markupExtension3 = translate2;
			XamlServiceProvider xamlServiceProvider3 = new XamlServiceProvider();
			Type typeFromHandle5 = typeof(IProvideValueTarget);
			object[] array3 = new object[0 + 4];
			array3[0] = labelCell;
			array3[1] = section;
			array3[2] = settingsView;
			array3[3] = this;
			object obj4;
			xamlServiceProvider3.Add(typeFromHandle5, obj4 = new SimpleValueTargetProvider(array3, CellBase.TitleProperty, nameScope));
			xamlServiceProvider3.Add(typeof(IReferenceProvider), obj4);
			Type typeFromHandle6 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver3 = new XmlNamespaceResolver();
			xmlNamespaceResolver3.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver3.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver3.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver3.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver3.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver3.Add("obd2", "clr-namespace:CarScannerXamarinForms.OBD2");
			xmlNamespaceResolver3.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver3.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xmlNamespaceResolver3.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider3.Add(typeFromHandle6, new XamlTypeResolver(xmlNamespaceResolver3, typeof(ELMFailPage).GetTypeInfo().Assembly));
			xamlServiceProvider3.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(27, 31)));
			object obj5 = markupExtension3.ProvideValue(xamlServiceProvider3);
			labelCell.Title = obj5;
			bindingExtension.Mode = 2;
			bindingExtension.Path = "NoFinishCharacterErrors";
			bindingExtension.TypedBinding = new TypedBinding<ELMState, int>(delegate(ELMState A_0)
			{
				if (A_0 != null)
				{
					return new ValueTuple<int, bool>(A_0.NoFinishCharacterErrors, true);
				}
				return default(ValueTuple<int, bool>);
			}, null, new Tuple<Func<ELMState, object>, string>[]
			{
				new Tuple<Func<ELMState, object>, string>((ELMState A_0) => A_0, "NoFinishCharacterErrors")
			});
			BindingBase bindingBase = bindingExtension.ProvideValue(null);
			labelCell.SetBinding(LabelCell.ValueTextProperty, bindingBase);
			section.Add(labelCell);
			translate3.Text = "settings_ELM_Errors_LineSymbolFail";
			IMarkupExtension markupExtension4 = translate3;
			XamlServiceProvider xamlServiceProvider4 = new XamlServiceProvider();
			Type typeFromHandle7 = typeof(IProvideValueTarget);
			object[] array4 = new object[0 + 4];
			array4[0] = labelCell2;
			array4[1] = section;
			array4[2] = settingsView;
			array4[3] = this;
			object obj6;
			xamlServiceProvider4.Add(typeFromHandle7, obj6 = new SimpleValueTargetProvider(array4, CellBase.TitleProperty, nameScope));
			xamlServiceProvider4.Add(typeof(IReferenceProvider), obj6);
			Type typeFromHandle8 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver4 = new XmlNamespaceResolver();
			xmlNamespaceResolver4.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver4.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver4.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver4.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver4.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver4.Add("obd2", "clr-namespace:CarScannerXamarinForms.OBD2");
			xmlNamespaceResolver4.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver4.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xmlNamespaceResolver4.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider4.Add(typeFromHandle8, new XamlTypeResolver(xmlNamespaceResolver4, typeof(ELMFailPage).GetTypeInfo().Assembly));
			xamlServiceProvider4.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(28, 31)));
			object obj7 = markupExtension4.ProvideValue(xamlServiceProvider4);
			labelCell2.Title = obj7;
			bindingExtension2.Mode = 2;
			bindingExtension2.Path = "WrongNewLineCharactersCounter";
			bindingExtension2.TypedBinding = new TypedBinding<ELMState, int>(delegate(ELMState A_0)
			{
				if (A_0 != null)
				{
					return new ValueTuple<int, bool>(A_0.WrongNewLineCharactersCounter, true);
				}
				return default(ValueTuple<int, bool>);
			}, null, new Tuple<Func<ELMState, object>, string>[]
			{
				new Tuple<Func<ELMState, object>, string>((ELMState A_0) => A_0, "WrongNewLineCharactersCounter")
			});
			BindingBase bindingBase2 = bindingExtension2.ProvideValue(null);
			labelCell2.SetBinding(LabelCell.ValueTextProperty, bindingBase2);
			section.Add(labelCell2);
			translate4.Text = "settings_ELM_Errors_UnexpectedELMResetDetected";
			IMarkupExtension markupExtension5 = translate4;
			XamlServiceProvider xamlServiceProvider5 = new XamlServiceProvider();
			Type typeFromHandle9 = typeof(IProvideValueTarget);
			object[] array5 = new object[0 + 4];
			array5[0] = labelCell3;
			array5[1] = section;
			array5[2] = settingsView;
			array5[3] = this;
			object obj8;
			xamlServiceProvider5.Add(typeFromHandle9, obj8 = new SimpleValueTargetProvider(array5, CellBase.TitleProperty, nameScope));
			xamlServiceProvider5.Add(typeof(IReferenceProvider), obj8);
			Type typeFromHandle10 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver5 = new XmlNamespaceResolver();
			xmlNamespaceResolver5.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver5.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver5.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver5.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver5.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver5.Add("obd2", "clr-namespace:CarScannerXamarinForms.OBD2");
			xmlNamespaceResolver5.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver5.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xmlNamespaceResolver5.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider5.Add(typeFromHandle10, new XamlTypeResolver(xmlNamespaceResolver5, typeof(ELMFailPage).GetTypeInfo().Assembly));
			xamlServiceProvider5.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(29, 31)));
			object obj9 = markupExtension5.ProvideValue(xamlServiceProvider5);
			labelCell3.Title = obj9;
			bindingExtension3.Mode = 2;
			bindingExtension3.Path = "ELMUnexpectedResetDetected";
			bindingExtension3.TypedBinding = new TypedBinding<ELMState, bool>(delegate(ELMState A_0)
			{
				if (A_0 != null)
				{
					return new ValueTuple<bool, bool>(A_0.ELMUnexpectedResetDetected, true);
				}
				return default(ValueTuple<bool, bool>);
			}, null, new Tuple<Func<ELMState, object>, string>[]
			{
				new Tuple<Func<ELMState, object>, string>((ELMState A_0) => A_0, "ELMUnexpectedResetDetected")
			});
			BindingBase bindingBase3 = bindingExtension3.ProvideValue(null);
			labelCell3.SetBinding(LabelCell.ValueTextProperty, bindingBase3);
			section.Add(labelCell3);
			settingsView.Root.Add(section);
			translate5.Text = "settings_ELM_Errors_ATCommandNotSupported";
			IMarkupExtension markupExtension6 = translate5;
			XamlServiceProvider xamlServiceProvider6 = new XamlServiceProvider();
			Type typeFromHandle11 = typeof(IProvideValueTarget);
			object[] array6 = new object[0 + 3];
			array6[0] = section2;
			array6[1] = settingsView;
			array6[2] = this;
			object obj10;
			xamlServiceProvider6.Add(typeFromHandle11, obj10 = new SimpleValueTargetProvider(array6, SectionBase.TitleProperty, nameScope));
			xamlServiceProvider6.Add(typeof(IReferenceProvider), obj10);
			Type typeFromHandle12 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver6 = new XmlNamespaceResolver();
			xmlNamespaceResolver6.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver6.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver6.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver6.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver6.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver6.Add("obd2", "clr-namespace:CarScannerXamarinForms.OBD2");
			xmlNamespaceResolver6.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver6.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xmlNamespaceResolver6.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider6.Add(typeFromHandle12, new XamlTypeResolver(xmlNamespaceResolver6, typeof(ELMFailPage).GetTypeInfo().Assembly));
			xamlServiceProvider6.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(31, 25)));
			object obj11 = markupExtension6.ProvideValue(xamlServiceProvider6);
			section2.Title = obj11;
			staticResourceExtension.Key = "StringIENumerableToStringConverter";
			IMarkupExtension markupExtension7 = staticResourceExtension;
			XamlServiceProvider xamlServiceProvider7 = new XamlServiceProvider();
			Type typeFromHandle13 = typeof(IProvideValueTarget);
			object[] array7 = new object[0 + 6];
			array7[0] = bindingExtension4;
			array7[1] = label;
			array7[2] = customCell;
			array7[3] = section2;
			array7[4] = settingsView;
			array7[5] = this;
			object obj12;
			xamlServiceProvider7.Add(typeFromHandle13, obj12 = new SimpleValueTargetProvider(array7, typeof(BindingExtension).GetRuntimeProperty("Converter"), nameScope));
			xamlServiceProvider7.Add(typeof(IReferenceProvider), obj12);
			Type typeFromHandle14 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver7 = new XmlNamespaceResolver();
			xmlNamespaceResolver7.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver7.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver7.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver7.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver7.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver7.Add("obd2", "clr-namespace:CarScannerXamarinForms.OBD2");
			xmlNamespaceResolver7.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver7.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xmlNamespaceResolver7.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider7.Add(typeFromHandle14, new XamlTypeResolver(xmlNamespaceResolver7, typeof(ELMFailPage).GetTypeInfo().Assembly));
			xamlServiceProvider7.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(33, 28)));
			object obj13 = markupExtension7.ProvideValue(xamlServiceProvider7);
			bindingExtension4.Converter = obj13;
			bindingExtension4.Path = "ATCommandsNotSupported";
			bindingExtension4.TypedBinding = new TypedBinding<ELMState, HashSet<string>>(delegate(ELMState A_0)
			{
				if (A_0 != null)
				{
					return new ValueTuple<HashSet<string>, bool>(A_0.ATCommandsNotSupported, true);
				}
				return default(ValueTuple<HashSet<string>, bool>);
			}, delegate(ELMState A_0, HashSet<string> A_1)
			{
				if (A_0 != null)
				{
					A_0.ATCommandsNotSupported = A_1;
					return;
				}
			}, new Tuple<Func<ELMState, object>, string>[]
			{
				new Tuple<Func<ELMState, object>, string>((ELMState A_0) => A_0, "ATCommandsNotSupported")
			});
			BindingBase bindingBase4 = bindingExtension4.ProvideValue(null);
			label.SetBinding(Label.TextProperty, bindingBase4);
			customCell.SetValue(CustomCell.ContentProperty, label);
			section2.Add(customCell);
			settingsView.Root.Add(section2);
			this.SetValue(ContentPage.ContentProperty, settingsView);
		}

		// Token: 0x06001A3D RID: 6717 RVA: 0x00121EC2 File Offset: 0x001200C2
		private void __InitComponentRuntime()
		{
			Extensions.LoadFromXaml<ELMFailPage>(this, typeof(ELMFailPage));
			this.settingsLayoutRoot = NameScopeExtensions.FindByName<SettingsView>(this, "settingsLayoutRoot");
		}

		// Token: 0x06001A3E RID: 6718 RVA: 0x00121EE8 File Offset: 0x001200E8
		[CompilerGenerated]
		private static ValueTuple<int, bool> <InitializeComponent>typedBindingsM__1528(ELMState A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<int, bool>(A_0.NoFinishCharacterErrors, true);
			}
			return default(ValueTuple<int, bool>);
		}

		// Token: 0x06001A3F RID: 6719 RVA: 0x00121F18 File Offset: 0x00120118
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__1529(ELMState A_0)
		{
			return A_0;
		}

		// Token: 0x06001A40 RID: 6720 RVA: 0x00121F28 File Offset: 0x00120128
		[CompilerGenerated]
		private static ValueTuple<int, bool> <InitializeComponent>typedBindingsM__1530(ELMState A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<int, bool>(A_0.WrongNewLineCharactersCounter, true);
			}
			return default(ValueTuple<int, bool>);
		}

		// Token: 0x06001A41 RID: 6721 RVA: 0x00121F58 File Offset: 0x00120158
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__1531(ELMState A_0)
		{
			return A_0;
		}

		// Token: 0x06001A42 RID: 6722 RVA: 0x00121F68 File Offset: 0x00120168
		[CompilerGenerated]
		private static ValueTuple<bool, bool> <InitializeComponent>typedBindingsM__1532(ELMState A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<bool, bool>(A_0.ELMUnexpectedResetDetected, true);
			}
			return default(ValueTuple<bool, bool>);
		}

		// Token: 0x06001A43 RID: 6723 RVA: 0x00121F98 File Offset: 0x00120198
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__1533(ELMState A_0)
		{
			return A_0;
		}

		// Token: 0x06001A44 RID: 6724 RVA: 0x00121FA8 File Offset: 0x001201A8
		[CompilerGenerated]
		private static ValueTuple<HashSet<string>, bool> <InitializeComponent>typedBindingsM__1534(ELMState A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<HashSet<string>, bool>(A_0.ATCommandsNotSupported, true);
			}
			return default(ValueTuple<HashSet<string>, bool>);
		}

		// Token: 0x06001A45 RID: 6725 RVA: 0x00121FD8 File Offset: 0x001201D8
		[CompilerGenerated]
		private static void <InitializeComponent>typedBindingsM__1535(ELMState A_0, HashSet<string> A_1)
		{
			if (A_0 != null)
			{
				A_0.ATCommandsNotSupported = A_1;
				return;
			}
		}

		// Token: 0x06001A46 RID: 6726 RVA: 0x00121FF4 File Offset: 0x001201F4
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__1536(ELMState A_0)
		{
			return A_0;
		}

		// Token: 0x04000B97 RID: 2967
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private SettingsView settingsLayoutRoot;
	}
}
