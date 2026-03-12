using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using CarScannerXamarinForms.Settings;
using Xamarin.Forms;
using Xamarin.Forms.Internals;
using Xamarin.Forms.PlatformConfiguration.iOSSpecific;
using Xamarin.Forms.Xaml;
using Xamarin.Forms.Xaml.Diagnostics;
using Xamarin.Forms.Xaml.Internals;

namespace CarScannerXamarinForms.Pages
{
	// Token: 0x0200062E RID: 1582
	[XamlFilePath("Pages\\LanguageSelectorPage.xaml")]
	public class LanguageSelectorPage : ContentPage
	{
		// Token: 0x06003731 RID: 14129 RVA: 0x00294532 File Offset: 0x00292732
		public LanguageSelectorPage()
		{
			this.InitializeComponent();
			this.lv.SelectedItem = StaticLists.LanguageList.First<string>();
			this.lv.ScrollTo(this.lv.SelectedItem, 1, false);
		}

		// Token: 0x06003732 RID: 14130 RVA: 0x000027D4 File Offset: 0x000009D4
		private void Handle_SizeChanged(object sender, EventArgs e)
		{
		}

		// Token: 0x06003733 RID: 14131 RVA: 0x00294570 File Offset: 0x00292770
		private async void btnApplyLanguage_Clicked(object sender, EventArgs e)
		{
			try
			{
				RegionInfo currentRegion = RegionInfo.CurrentRegion;
				SharedSettings.Current.Currency = currentRegion.CurrencySymbol;
				if (!currentRegion.IsMetric)
				{
					SharedSettings.Current.Use_km = false;
					SharedSettings.Current.UseLitersForVolume = false;
					SharedSettings.Current.Use_celcium = false;
					SharedSettings.Current.FuelConsumptionUnit = FuelConsumptionUnits.MilesPerGallon;
					if (currentRegion.TwoLetterISORegionName == "US")
					{
						SharedSettings.Current.UseUSGallon = true;
					}
					if (currentRegion.TwoLetterISORegionName == "UK")
					{
						SharedSettings.Current.UseLitersForVolume = true;
					}
				}
				int num = StaticLists.LanguageList.IndexOf((string)this.lv.SelectedItem);
				SharedSettings.Current.Language = num;
			}
			catch (Exception)
			{
				SharedSettings.Current.Currency = "$";
			}
			App.Instance.ChangeLanguageAndGoToWelcomePage();
		}

		// Token: 0x06003734 RID: 14132 RVA: 0x002945A8 File Offset: 0x002927A8
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private void InitializeComponent()
		{
			if (ResourceLoader.IsEnabled && ResourceLoader.CanProvideContentFor(new ResourceLoader.ResourceLoadingQuery
			{
				AssemblyName = typeof(LanguageSelectorPage).GetTypeInfo().Assembly.GetName(),
				ResourcePath = "Pages/LanguageSelectorPage.xaml",
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
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension = new DynamicResourceExtension(), new Uri("Pages\\LanguageSelectorPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 11, 5);
			RowDefinition rowDefinition;
			VisualDiagnostics.RegisterSourceInfo(rowDefinition = new RowDefinition(), new Uri("Pages\\LanguageSelectorPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 16, 18);
			RowDefinition rowDefinition2;
			VisualDiagnostics.RegisterSourceInfo(rowDefinition2 = new RowDefinition(), new Uri("Pages\\LanguageSelectorPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 17, 18);
			RowDefinition rowDefinition3;
			VisualDiagnostics.RegisterSourceInfo(rowDefinition3 = new RowDefinition(), new Uri("Pages\\LanguageSelectorPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 18, 18);
			Translate translate;
			VisualDiagnostics.RegisterSourceInfo(translate = new Translate(), new Uri("Pages\\LanguageSelectorPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 25, 17);
			Label label;
			VisualDiagnostics.RegisterSourceInfo(label = new Label(), new Uri("Pages\\LanguageSelectorPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 20, 14);
			List<string> languageList;
			VisualDiagnostics.RegisterSourceInfo(languageList = StaticLists.LanguageList, new Uri("Pages\\LanguageSelectorPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 30, 17);
			BindingExtension bindingExtension;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension = new BindingExtension(), new Uri("Pages\\LanguageSelectorPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 30, 17);
			DataTemplate dataTemplate;
			VisualDiagnostics.RegisterSourceInfo(dataTemplate = new DataTemplate(), new Uri("Pages\\LanguageSelectorPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 32, 22);
			ListView listView;
			VisualDiagnostics.RegisterSourceInfo(listView = new ListView(), new Uri("Pages\\LanguageSelectorPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 26, 14);
			DynamicResourceExtension dynamicResourceExtension2;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension2 = new DynamicResourceExtension(), new Uri("Pages\\LanguageSelectorPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 39, 17);
			Button button;
			VisualDiagnostics.RegisterSourceInfo(button = new Button(), new Uri("Pages\\LanguageSelectorPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 37, 14);
			Grid grid;
			VisualDiagnostics.RegisterSourceInfo(grid = new Grid(), new Uri("Pages\\LanguageSelectorPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 14, 10);
			VisualDiagnostics.RegisterSourceInfo(this, new Uri("Pages\\LanguageSelectorPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 2, 2);
			NameScope nameScope = NameScope.GetNameScope(this) ?? new NameScope();
			NameScope.SetNameScope(this, nameScope);
			nameScope.RegisterName("lv", listView);
			if (listView.StyleId == null)
			{
				listView.StyleId = "lv";
			}
			this.lv = listView;
			this.SetValue(Page.TitleProperty, "Car Scanner ELM OBD2");
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
			xmlNamespaceResolver.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver.Add("local", "clr-namespace:CarScannerXamarinForms");
			xamlServiceProvider.Add(typeFromHandle2, new XamlTypeResolver(xmlNamespaceResolver, typeof(LanguageSelectorPage).GetTypeInfo().Assembly));
			xamlServiceProvider.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(11, 5)));
			DynamicResource dynamicResource = markupExtension.ProvideValue(xamlServiceProvider);
			this.SetDynamicResource(VisualElement.BackgroundColorProperty, dynamicResource.Key);
			this.SizeChanged += this.Handle_SizeChanged;
			rowDefinition.SetValue(RowDefinition.HeightProperty, new GridLengthTypeConverter().ConvertFromInvariantString("auto"));
			grid.GetValue(Grid.RowDefinitionsProperty).Add(rowDefinition);
			rowDefinition2.SetValue(RowDefinition.HeightProperty, new GridLengthTypeConverter().ConvertFromInvariantString("*"));
			grid.GetValue(Grid.RowDefinitionsProperty).Add(rowDefinition2);
			rowDefinition3.SetValue(RowDefinition.HeightProperty, new GridLengthTypeConverter().ConvertFromInvariantString("auto"));
			grid.GetValue(Grid.RowDefinitionsProperty).Add(rowDefinition3);
			label.SetValue(Grid.RowProperty, 0);
			label.SetValue(Label.FontAttributesProperty, new FontAttributesConverter().ConvertFromInvariantString("Bold"));
			label.SetValue(View.HorizontalOptionsProperty, LayoutOptions.Center);
			label.SetValue(Label.HorizontalTextAlignmentProperty, new TextAlignmentConverter().ConvertFromInvariantString("Center"));
			translate.Text = "welcome_ChooseLanguage";
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
			xamlServiceProvider2.Add(typeFromHandle4, new XamlTypeResolver(xmlNamespaceResolver2, typeof(LanguageSelectorPage).GetTypeInfo().Assembly));
			xamlServiceProvider2.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(25, 17)));
			object obj3 = markupExtension2.ProvideValue(xamlServiceProvider2);
			label.Text = obj3;
			grid.Children.Add(label);
			listView.SetValue(Grid.RowProperty, 1);
			listView.SetValue(VisualElement.BackgroundColorProperty, Color.Transparent);
			bindingExtension.Source = languageList;
			BindingBase bindingBase = bindingExtension.ProvideValue(null);
			listView.SetBinding(ItemsView<Cell>.ItemsSourceProperty, bindingBase);
			IDataTemplate dataTemplate2 = dataTemplate;
			LanguageSelectorPage.<InitializeComponent>_anonXamlCDataTemplate_58 <InitializeComponent>_anonXamlCDataTemplate_ = new LanguageSelectorPage.<InitializeComponent>_anonXamlCDataTemplate_58();
			object[] array3 = new object[0 + 4];
			array3[0] = dataTemplate;
			array3[1] = listView;
			array3[2] = grid;
			array3[3] = this;
			<InitializeComponent>_anonXamlCDataTemplate_.parentValues = array3;
			<InitializeComponent>_anonXamlCDataTemplate_.root = this;
			dataTemplate2.LoadTemplate = new Func<object>(<InitializeComponent>_anonXamlCDataTemplate_.LoadDataTemplate);
			listView.SetValue(ItemsView<Cell>.ItemTemplateProperty, dataTemplate);
			grid.Children.Add(listView);
			button.SetValue(Grid.RowProperty, 2);
			dynamicResourceExtension2.Key = "ButtonGreenColor";
			IMarkupExtension<DynamicResource> markupExtension3 = dynamicResourceExtension2;
			XamlServiceProvider xamlServiceProvider3 = new XamlServiceProvider();
			Type typeFromHandle5 = typeof(IProvideValueTarget);
			object[] array4 = new object[0 + 3];
			array4[0] = button;
			array4[1] = grid;
			array4[2] = this;
			object obj4;
			xamlServiceProvider3.Add(typeFromHandle5, obj4 = new SimpleValueTargetProvider(array4, VisualElement.BackgroundColorProperty, nameScope));
			xamlServiceProvider3.Add(typeof(IReferenceProvider), obj4);
			Type typeFromHandle6 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver3 = new XmlNamespaceResolver();
			xmlNamespaceResolver3.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver3.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver3.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver3.Add("local", "clr-namespace:CarScannerXamarinForms");
			xamlServiceProvider3.Add(typeFromHandle6, new XamlTypeResolver(xmlNamespaceResolver3, typeof(LanguageSelectorPage).GetTypeInfo().Assembly));
			xamlServiceProvider3.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(39, 17)));
			DynamicResource dynamicResource2 = markupExtension3.ProvideValue(xamlServiceProvider3);
			button.SetDynamicResource(VisualElement.BackgroundColorProperty, dynamicResource2.Key);
			button.Clicked += this.btnApplyLanguage_Clicked;
			button.SetValue(Button.TextProperty, "OK");
			button.SetValue(Button.TextColorProperty, Color.White);
			grid.Children.Add(button);
			this.SetValue(ContentPage.ContentProperty, grid);
		}

		// Token: 0x06003735 RID: 14133 RVA: 0x00294DED File Offset: 0x00292FED
		private void __InitComponentRuntime()
		{
			Extensions.LoadFromXaml<LanguageSelectorPage>(this, typeof(LanguageSelectorPage));
			this.lv = NameScopeExtensions.FindByName<ListView>(this, "lv");
		}

		// Token: 0x04002165 RID: 8549
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private ListView lv;

		// Token: 0x0200062F RID: 1583
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <btnApplyLanguage_Clicked>d__2 : IAsyncStateMachine
		{
			// Token: 0x06003736 RID: 14134 RVA: 0x00294E14 File Offset: 0x00293014
			void IAsyncStateMachine.MoveNext()
			{
				LanguageSelectorPage languageSelectorPage = this;
				try
				{
					try
					{
						RegionInfo currentRegion = RegionInfo.CurrentRegion;
						SharedSettings.Current.Currency = currentRegion.CurrencySymbol;
						if (!currentRegion.IsMetric)
						{
							SharedSettings.Current.Use_km = false;
							SharedSettings.Current.UseLitersForVolume = false;
							SharedSettings.Current.Use_celcium = false;
							SharedSettings.Current.FuelConsumptionUnit = FuelConsumptionUnits.MilesPerGallon;
							if (currentRegion.TwoLetterISORegionName == "US")
							{
								SharedSettings.Current.UseUSGallon = true;
							}
							if (currentRegion.TwoLetterISORegionName == "UK")
							{
								SharedSettings.Current.UseLitersForVolume = true;
							}
						}
						int num = StaticLists.LanguageList.IndexOf((string)languageSelectorPage.lv.SelectedItem);
						SharedSettings.Current.Language = num;
					}
					catch (Exception)
					{
						SharedSettings.Current.Currency = "$";
					}
					App.Instance.ChangeLanguageAndGoToWelcomePage();
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

			// Token: 0x06003737 RID: 14135 RVA: 0x00294F38 File Offset: 0x00293138
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x04002166 RID: 8550
			public int <>1__state;

			// Token: 0x04002167 RID: 8551
			public AsyncVoidMethodBuilder <>t__builder;

			// Token: 0x04002168 RID: 8552
			public LanguageSelectorPage <>4__this;
		}

		// Token: 0x02000630 RID: 1584
		[CompilerGenerated]
		private sealed class <InitializeComponent>_anonXamlCDataTemplate_58
		{
			// Token: 0x06003738 RID: 14136 RVA: 0x00294F48 File Offset: 0x00293148
			public <InitializeComponent>_anonXamlCDataTemplate_58()
			{
			}

			// Token: 0x06003739 RID: 14137 RVA: 0x00294F5C File Offset: 0x0029315C
			internal object LoadDataTemplate()
			{
				BindingExtension bindingExtension;
				VisualDiagnostics.RegisterSourceInfo(bindingExtension = new BindingExtension(), new Uri("Pages\\LanguageSelectorPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 33, 35);
				DynamicResourceExtension dynamicResourceExtension;
				VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension = new DynamicResourceExtension(), new Uri("Pages\\LanguageSelectorPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 33, 52);
				TextCell textCell;
				VisualDiagnostics.RegisterSourceInfo(textCell = new TextCell(), new Uri("Pages\\LanguageSelectorPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 33, 26);
				NameScope nameScope = new NameScope();
				NameScope.SetNameScope(textCell, nameScope);
				BindingBase bindingBase = bindingExtension.ProvideValue(null);
				textCell.SetBinding(TextCell.TextProperty, bindingBase);
				dynamicResourceExtension.Key = "TextColor";
				IMarkupExtension<DynamicResource> markupExtension = dynamicResourceExtension;
				XamlServiceProvider xamlServiceProvider = new XamlServiceProvider();
				Type typeFromHandle = typeof(IProvideValueTarget);
				int num;
				object[] array = new object[(num = this.parentValues.Length) + 1];
				Array.Copy(this.parentValues, 0, array, 1, num);
				object[] array2 = array;
				array2[0] = textCell;
				object obj;
				xamlServiceProvider.Add(typeFromHandle, obj = new SimpleValueTargetProvider(array2, TextCell.TextColorProperty, nameScope));
				xamlServiceProvider.Add(typeof(IReferenceProvider), obj);
				Type typeFromHandle2 = typeof(IXamlTypeResolver);
				XmlNamespaceResolver xmlNamespaceResolver = new XmlNamespaceResolver();
				xmlNamespaceResolver.Add("", "http://xamarin.com/schemas/2014/forms");
				xmlNamespaceResolver.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
				xmlNamespaceResolver.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
				xmlNamespaceResolver.Add("local", "clr-namespace:CarScannerXamarinForms");
				xamlServiceProvider.Add(typeFromHandle2, new XamlTypeResolver(xmlNamespaceResolver, typeof(LanguageSelectorPage.<InitializeComponent>_anonXamlCDataTemplate_58).GetTypeInfo().Assembly));
				xamlServiceProvider.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(33, 52)));
				DynamicResource dynamicResource = markupExtension.ProvideValue(xamlServiceProvider);
				textCell.SetDynamicResource(TextCell.TextColorProperty, dynamicResource.Key);
				return textCell;
			}

			// Token: 0x04002169 RID: 8553
			internal object[] parentValues;

			// Token: 0x0400216A RID: 8554
			internal LanguageSelectorPage root;
		}
	}
}
