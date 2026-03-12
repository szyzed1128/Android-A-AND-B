using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using CarScannerXamarinForms.Dashboard;
using CarScannerXamarinForms.OBD2;
using CarScannerXamarinForms.PlatformAdapters;
using CarScannerXamarinForms.Settings;
using CarScannerXamarinForms.UserControls;
using Syncfusion.XForms.Buttons;
using Xamarin.Forms;
using Xamarin.Forms.Internals;
using Xamarin.Forms.PlatformConfiguration.iOSSpecific;
using Xamarin.Forms.Xaml;
using Xamarin.Forms.Xaml.Diagnostics;
using Xamarin.Forms.Xaml.Internals;

namespace CarScannerXamarinForms
{
	// Token: 0x020001E0 RID: 480
	[XamlFilePath("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Settings\\SettingsInterfacePage.xaml")]
	public class SettingsInterfacePage : ContentPage
	{
		// Token: 0x06001935 RID: 6453 RVA: 0x000FD804 File Offset: 0x000FBA04
		public SettingsInterfacePage()
		{
			try
			{
				this.InitializeComponent();
				if (PlatformHelper.AppMarket == Markets.RUS)
				{
					this.panelLanguage.IsVisible = false;
				}
				string[] array = new string[]
				{
					Translate.GetString("Settings_ChartStyle_FastLine"),
					Translate.GetString("Settings_ChartStyle_Area"),
					Translate.GetString("Settings_ChartStyle_Spline"),
					Translate.GetString("Settings_ChartStyle_SplineArea")
				};
				this.chartStylePicker.ItemsSource = array;
			}
			catch (Exception)
			{
			}
			base.BindingContext = SharedSettings.Current;
		}

		// Token: 0x06001936 RID: 6454 RVA: 0x000FD898 File Offset: 0x000FBA98
		private async void btnApplyLanguage_Clicked(object sender, EventArgs e)
		{
			if (App.OBDSimulator.IsActive)
			{
				App.OBDSimulator.Stop();
			}
			if (App.OBDReader.CurrentStatus != OBDDataReaderStatus.Disconnected)
			{
				await App.OBDReader.Disconnect("btnApplyLanguage_Clicked");
			}
			App.Instance.ChangeLanguage();
			SharedSettings.Current.LanguageChanged = false;
		}

		// Token: 0x06001937 RID: 6455 RVA: 0x000E988B File Offset: 0x000E7A8B
		private void DarkThemeSwitch_Toggled(object sender, ToggledEventArgs e)
		{
			base.ApplyBindings();
		}

		// Token: 0x06001938 RID: 6456 RVA: 0x000FD8C8 File Offset: 0x000FBAC8
		private async void btnResetDashboard_Clicked(object sender, EventArgs e)
		{
			TaskAwaiter<bool> taskAwaiter = base.DisplayAlert(Translate.GetString("ios_ResetDashboard_Title"), Translate.GetString("ios_ResetDashboard_Text"), Translate.GetString("ios_Yes"), Translate.GetString("ios_No")).GetAwaiter();
			if (!taskAwaiter.IsCompleted)
			{
				await taskAwaiter;
				TaskAwaiter<bool> taskAwaiter2;
				taskAwaiter = taskAwaiter2;
				taskAwaiter2 = default(TaskAwaiter<bool>);
			}
			if (taskAwaiter.GetResult())
			{
				DashboardListViewModel.Current.ClearDashboard();
			}
		}

		// Token: 0x06001939 RID: 6457 RVA: 0x000FD900 File Offset: 0x000FBB00
		private async void btnApplyDashboardTheme_Clicked(object sender, EventArgs e)
		{
			DashboardListViewModel.Current.LoadDashboardFromSettings();
			foreach (DashboardPage dashboardPage in DashboardListViewModel.Current.Pages)
			{
				foreach (DashboardItem dashboardItem in dashboardPage.Items)
				{
					try
					{
						DashboardItem.ApplyThemeToItem(dashboardItem);
						dashboardItem.SelectAndAddControl();
					}
					catch (Exception ex)
					{
						ObjectDisposedException ex2 = ex as ObjectDisposedException;
					}
				}
			}
			DashboardListViewModel.Current.SaveDashboardToSettings();
			base.DisplayAlert(Translate.GetString("ios_DashboardThemeApplyed"), "", "OK");
		}

		// Token: 0x0600193A RID: 6458 RVA: 0x000FD937 File Offset: 0x000FBB37
		private void btnDarkTheme_Clicked(object sender, EventArgs e)
		{
			SharedSettings.Current.DarkMode = true;
		}

		// Token: 0x0600193B RID: 6459 RVA: 0x000FD944 File Offset: 0x000FBB44
		private void btnLightTheme_Clicked(object sender, EventArgs e)
		{
			SharedSettings.Current.DarkMode = false;
		}

		// Token: 0x0600193C RID: 6460 RVA: 0x000FD951 File Offset: 0x000FBB51
		private void btnMainScreenConfiguration_Clicked(object sender, EventArgs e)
		{
			base.Navigation.PushAsync(new MainPageConfigurationScreen());
		}

		// Token: 0x0600193D RID: 6461 RVA: 0x000FD964 File Offset: 0x000FBB64
		private void btnFontSizePlus_Tapped(object sender, EventArgs e)
		{
			SharedSettings sharedSettings = SharedSettings.Current;
			int fontSizePatch = sharedSettings.FontSizePatch;
			sharedSettings.FontSizePatch = fontSizePatch + 1;
		}

		// Token: 0x0600193E RID: 6462 RVA: 0x000FD988 File Offset: 0x000FBB88
		private void btnFontSizeMinus_Tapped(object sender, EventArgs e)
		{
			SharedSettings sharedSettings = SharedSettings.Current;
			int fontSizePatch = sharedSettings.FontSizePatch;
			sharedSettings.FontSizePatch = fontSizePatch - 1;
		}

		// Token: 0x0600193F RID: 6463 RVA: 0x000FD9AC File Offset: 0x000FBBAC
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private void InitializeComponent()
		{
			if (ResourceLoader.IsEnabled && ResourceLoader.CanProvideContentFor(new ResourceLoader.ResourceLoadingQuery
			{
				AssemblyName = typeof(SettingsInterfacePage).GetTypeInfo().Assembly.GetName(),
				ResourcePath = "Settings/SettingsInterfacePage.xaml",
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
			VisualDiagnostics.RegisterSourceInfo(translate = new Translate(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Settings\\SettingsInterfacePage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 13, 5);
			DynamicResourceExtension dynamicResourceExtension;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension = new DynamicResourceExtension(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Settings\\SettingsInterfacePage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 16, 5);
			BoolToNegativeConverter boolToNegativeConverter;
			VisualDiagnostics.RegisterSourceInfo(boolToNegativeConverter = new BoolToNegativeConverter(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Settings\\SettingsInterfacePage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 19, 14);
			TimeSpanTotalIntSecondsToDoubleConverter timeSpanTotalIntSecondsToDoubleConverter;
			VisualDiagnostics.RegisterSourceInfo(timeSpanTotalIntSecondsToDoubleConverter = new TimeSpanTotalIntSecondsToDoubleConverter(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Settings\\SettingsInterfacePage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 20, 14);
			EnumToIntConverter enumToIntConverter;
			VisualDiagnostics.RegisterSourceInfo(enumToIntConverter = new EnumToIntConverter(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Settings\\SettingsInterfacePage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 21, 14);
			ResourceDictionary resourceDictionary;
			VisualDiagnostics.RegisterSourceInfo(resourceDictionary = new ResourceDictionary(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Settings\\SettingsInterfacePage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 18, 10);
			OnPlatform<Thickness> onPlatform;
			VisualDiagnostics.RegisterSourceInfo(onPlatform = new OnPlatform<Thickness>(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Settings\\SettingsInterfacePage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 32, 22);
			Translate translate2;
			VisualDiagnostics.RegisterSourceInfo(translate2 = new Translate(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Settings\\SettingsInterfacePage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 40, 28);
			Label label;
			VisualDiagnostics.RegisterSourceInfo(label = new Label(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Settings\\SettingsInterfacePage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 40, 22);
			List<string> languageList;
			VisualDiagnostics.RegisterSourceInfo(languageList = StaticLists.LanguageList, new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Settings\\SettingsInterfacePage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 43, 25);
			BindingExtension bindingExtension;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension = new BindingExtension(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Settings\\SettingsInterfacePage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 43, 25);
			BindingExtension bindingExtension2;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension2 = new BindingExtension(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Settings\\SettingsInterfacePage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 44, 25);
			Picker picker;
			VisualDiagnostics.RegisterSourceInfo(picker = new Picker(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Settings\\SettingsInterfacePage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 41, 22);
			Button button;
			VisualDiagnostics.RegisterSourceInfo(button = new Button(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Settings\\SettingsInterfacePage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 45, 22);
			StackLayout stackLayout;
			VisualDiagnostics.RegisterSourceInfo(stackLayout = new StackLayout(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Settings\\SettingsInterfacePage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 39, 18);
			BindingExtension bindingExtension3;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension3 = new BindingExtension(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Settings\\SettingsInterfacePage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 52, 21);
			BindingExtension bindingExtension4;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension4 = new BindingExtension(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Settings\\SettingsInterfacePage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 53, 21);
			Translate translate3;
			VisualDiagnostics.RegisterSourceInfo(translate3 = new Translate(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Settings\\SettingsInterfacePage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 54, 21);
			LabelSwitch labelSwitch;
			VisualDiagnostics.RegisterSourceInfo(labelSwitch = new LabelSwitch(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Settings\\SettingsInterfacePage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 51, 18);
			Translate translate4;
			VisualDiagnostics.RegisterSourceInfo(translate4 = new Translate(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Settings\\SettingsInterfacePage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 60, 35);
			Span span;
			VisualDiagnostics.RegisterSourceInfo(span = new Span(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Settings\\SettingsInterfacePage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 60, 30);
			Span span2;
			VisualDiagnostics.RegisterSourceInfo(span2 = new Span(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Settings\\SettingsInterfacePage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 61, 30);
			BindingExtension bindingExtension5;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension5 = new BindingExtension(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Settings\\SettingsInterfacePage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 62, 35);
			Span span3;
			VisualDiagnostics.RegisterSourceInfo(span3 = new Span(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Settings\\SettingsInterfacePage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 62, 30);
			FormattedString formattedString;
			VisualDiagnostics.RegisterSourceInfo(formattedString = new FormattedString(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Settings\\SettingsInterfacePage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 59, 26);
			Label label2;
			VisualDiagnostics.RegisterSourceInfo(label2 = new Label(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Settings\\SettingsInterfacePage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 57, 18);
			Translate translate5;
			VisualDiagnostics.RegisterSourceInfo(translate5 = new Translate(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Settings\\SettingsInterfacePage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 69, 21);
			Button button2;
			VisualDiagnostics.RegisterSourceInfo(button2 = new Button(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Settings\\SettingsInterfacePage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 66, 18);
			Translate translate6;
			VisualDiagnostics.RegisterSourceInfo(translate6 = new Translate(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Settings\\SettingsInterfacePage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 73, 21);
			Button button3;
			VisualDiagnostics.RegisterSourceInfo(button3 = new Button(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Settings\\SettingsInterfacePage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 70, 18);
			StaticResourceExtension staticResourceExtension;
			VisualDiagnostics.RegisterSourceInfo(staticResourceExtension = new StaticResourceExtension(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Settings\\SettingsInterfacePage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 78, 21);
			BindingExtension bindingExtension6;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension6 = new BindingExtension(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Settings\\SettingsInterfacePage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 78, 21);
			Translate translate7;
			VisualDiagnostics.RegisterSourceInfo(translate7 = new Translate(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Settings\\SettingsInterfacePage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 80, 28);
			Label label3;
			VisualDiagnostics.RegisterSourceInfo(label3 = new Label(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Settings\\SettingsInterfacePage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 80, 22);
			DynamicResourceExtension dynamicResourceExtension2;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension2 = new DynamicResourceExtension(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Settings\\SettingsInterfacePage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 83, 25);
			StaticResourceExtension staticResourceExtension2;
			VisualDiagnostics.RegisterSourceInfo(staticResourceExtension2 = new StaticResourceExtension(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Settings\\SettingsInterfacePage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 84, 25);
			BindingExtension bindingExtension7;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension7 = new BindingExtension(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Settings\\SettingsInterfacePage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 84, 25);
			Translate translate8;
			VisualDiagnostics.RegisterSourceInfo(translate8 = new Translate(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Settings\\SettingsInterfacePage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 86, 25);
			DynamicResourceExtension dynamicResourceExtension3;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension3 = new DynamicResourceExtension(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Settings\\SettingsInterfacePage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 87, 25);
			DynamicResourceExtension dynamicResourceExtension4;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension4 = new DynamicResourceExtension(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Settings\\SettingsInterfacePage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 88, 25);
			SfRadioButton sfRadioButton;
			VisualDiagnostics.RegisterSourceInfo(sfRadioButton = new SfRadioButton(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Settings\\SettingsInterfacePage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 81, 22);
			DynamicResourceExtension dynamicResourceExtension5;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension5 = new DynamicResourceExtension(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Settings\\SettingsInterfacePage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 91, 25);
			BindingExtension bindingExtension8;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension8 = new BindingExtension(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Settings\\SettingsInterfacePage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 92, 25);
			Translate translate9;
			VisualDiagnostics.RegisterSourceInfo(translate9 = new Translate(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Settings\\SettingsInterfacePage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 94, 25);
			DynamicResourceExtension dynamicResourceExtension6;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension6 = new DynamicResourceExtension(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Settings\\SettingsInterfacePage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 95, 25);
			DynamicResourceExtension dynamicResourceExtension7;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension7 = new DynamicResourceExtension(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Settings\\SettingsInterfacePage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 96, 25);
			SfRadioButton sfRadioButton2;
			VisualDiagnostics.RegisterSourceInfo(sfRadioButton2 = new SfRadioButton(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Settings\\SettingsInterfacePage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 89, 22);
			SfRadioGroup sfRadioGroup;
			VisualDiagnostics.RegisterSourceInfo(sfRadioGroup = new SfRadioGroup(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Settings\\SettingsInterfacePage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 76, 18);
			BindingExtension bindingExtension9;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension9 = new BindingExtension(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Settings\\SettingsInterfacePage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 105, 21);
			Translate translate10;
			VisualDiagnostics.RegisterSourceInfo(translate10 = new Translate(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Settings\\SettingsInterfacePage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 106, 28);
			Label label4;
			VisualDiagnostics.RegisterSourceInfo(label4 = new Label(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Settings\\SettingsInterfacePage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 106, 22);
			Translate translate11;
			VisualDiagnostics.RegisterSourceInfo(translate11 = new Translate(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Settings\\SettingsInterfacePage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 110, 25);
			StaticResourceExtension staticResourceExtension3;
			VisualDiagnostics.RegisterSourceInfo(staticResourceExtension3 = new StaticResourceExtension(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Settings\\SettingsInterfacePage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 112, 25);
			RadioButtonWithColor radioButtonWithColor;
			VisualDiagnostics.RegisterSourceInfo(radioButtonWithColor = new RadioButtonWithColor(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Settings\\SettingsInterfacePage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 109, 22);
			Translate translate12;
			VisualDiagnostics.RegisterSourceInfo(translate12 = new Translate(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Settings\\SettingsInterfacePage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 114, 25);
			StaticResourceExtension staticResourceExtension4;
			VisualDiagnostics.RegisterSourceInfo(staticResourceExtension4 = new StaticResourceExtension(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Settings\\SettingsInterfacePage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 116, 25);
			RadioButtonWithColor radioButtonWithColor2;
			VisualDiagnostics.RegisterSourceInfo(radioButtonWithColor2 = new RadioButtonWithColor(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Settings\\SettingsInterfacePage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 113, 22);
			StackLayout stackLayout2;
			VisualDiagnostics.RegisterSourceInfo(stackLayout2 = new StackLayout(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Settings\\SettingsInterfacePage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 102, 18);
			BindingExtension bindingExtension10;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension10 = new BindingExtension(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Settings\\SettingsInterfacePage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 121, 36);
			Translate translate13;
			VisualDiagnostics.RegisterSourceInfo(translate13 = new Translate(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Settings\\SettingsInterfacePage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 121, 92);
			OnPlatform<bool> onPlatform2;
			VisualDiagnostics.RegisterSourceInfo(onPlatform2 = new OnPlatform<bool>(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Settings\\SettingsInterfacePage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 123, 26);
			LabelSwitch labelSwitch2;
			VisualDiagnostics.RegisterSourceInfo(labelSwitch2 = new LabelSwitch(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Settings\\SettingsInterfacePage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 121, 18);
			Translate translate14;
			VisualDiagnostics.RegisterSourceInfo(translate14 = new Translate(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Settings\\SettingsInterfacePage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 133, 21);
			Button button4;
			VisualDiagnostics.RegisterSourceInfo(button4 = new Button(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Settings\\SettingsInterfacePage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 130, 18);
			Translate translate15;
			VisualDiagnostics.RegisterSourceInfo(translate15 = new Translate(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Settings\\SettingsInterfacePage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 138, 28);
			Label label5;
			VisualDiagnostics.RegisterSourceInfo(label5 = new Label(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Settings\\SettingsInterfacePage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 138, 22);
			List<string> chartViewSelector;
			VisualDiagnostics.RegisterSourceInfo(chartViewSelector = StaticLists.ChartViewSelector, new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Settings\\SettingsInterfacePage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 141, 25);
			BindingExtension bindingExtension11;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension11 = new BindingExtension(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Settings\\SettingsInterfacePage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 141, 25);
			BindingExtension bindingExtension12;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension12 = new BindingExtension(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Settings\\SettingsInterfacePage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 142, 25);
			Picker picker2;
			VisualDiagnostics.RegisterSourceInfo(picker2 = new Picker(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Settings\\SettingsInterfacePage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 139, 22);
			StackLayout stackLayout3;
			VisualDiagnostics.RegisterSourceInfo(stackLayout3 = new StackLayout(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Settings\\SettingsInterfacePage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 137, 18);
			ColumnDefinition columnDefinition;
			VisualDiagnostics.RegisterSourceInfo(columnDefinition = new ColumnDefinition(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Settings\\SettingsInterfacePage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 147, 26);
			ColumnDefinition columnDefinition2;
			VisualDiagnostics.RegisterSourceInfo(columnDefinition2 = new ColumnDefinition(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Settings\\SettingsInterfacePage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 148, 26);
			Translate translate16;
			VisualDiagnostics.RegisterSourceInfo(translate16 = new Translate(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Settings\\SettingsInterfacePage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 154, 25);
			Label label6;
			VisualDiagnostics.RegisterSourceInfo(label6 = new Label(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Settings\\SettingsInterfacePage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 150, 22);
			BindingExtension bindingExtension13;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension13 = new BindingExtension(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Settings\\SettingsInterfacePage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 160, 25);
			Label label7;
			VisualDiagnostics.RegisterSourceInfo(label7 = new Label(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Settings\\SettingsInterfacePage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 155, 22);
			Grid grid;
			VisualDiagnostics.RegisterSourceInfo(grid = new Grid(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Settings\\SettingsInterfacePage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 145, 18);
			BindingExtension bindingExtension14;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension14 = new BindingExtension(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Settings\\SettingsInterfacePage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 170, 21);
			Stepper stepper;
			VisualDiagnostics.RegisterSourceInfo(stepper = new Stepper(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Settings\\SettingsInterfacePage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 163, 18);
			Translate translate17;
			VisualDiagnostics.RegisterSourceInfo(translate17 = new Translate(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Settings\\SettingsInterfacePage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 173, 24);
			Label label8;
			VisualDiagnostics.RegisterSourceInfo(label8 = new Label(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Settings\\SettingsInterfacePage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 173, 18);
			StaticResourceExtension staticResourceExtension5;
			VisualDiagnostics.RegisterSourceInfo(staticResourceExtension5 = new StaticResourceExtension(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Settings\\SettingsInterfacePage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 174, 51);
			BindingExtension bindingExtension15;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension15 = new BindingExtension(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Settings\\SettingsInterfacePage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 174, 51);
			Picker picker3;
			VisualDiagnostics.RegisterSourceInfo(picker3 = new Picker(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Settings\\SettingsInterfacePage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 174, 18);
			ColumnDefinition columnDefinition3;
			VisualDiagnostics.RegisterSourceInfo(columnDefinition3 = new ColumnDefinition(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Settings\\SettingsInterfacePage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 194, 26);
			ColumnDefinition columnDefinition4;
			VisualDiagnostics.RegisterSourceInfo(columnDefinition4 = new ColumnDefinition(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Settings\\SettingsInterfacePage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 195, 26);
			RowDefinition rowDefinition;
			VisualDiagnostics.RegisterSourceInfo(rowDefinition = new RowDefinition(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Settings\\SettingsInterfacePage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 198, 26);
			RowDefinition rowDefinition2;
			VisualDiagnostics.RegisterSourceInfo(rowDefinition2 = new RowDefinition(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Settings\\SettingsInterfacePage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 199, 26);
			BindingExtension bindingExtension16;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension16 = new BindingExtension(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Settings\\SettingsInterfacePage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 205, 25);
			Translate translate18;
			VisualDiagnostics.RegisterSourceInfo(translate18 = new Translate(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Settings\\SettingsInterfacePage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 206, 25);
			Label label9;
			VisualDiagnostics.RegisterSourceInfo(label9 = new Label(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Settings\\SettingsInterfacePage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 201, 22);
			StaticResourceExtension staticResourceExtension6;
			VisualDiagnostics.RegisterSourceInfo(staticResourceExtension6 = new StaticResourceExtension(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Settings\\SettingsInterfacePage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 212, 25);
			BindingExtension bindingExtension17;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension17 = new BindingExtension(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Settings\\SettingsInterfacePage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 212, 25);
			Translate translate19;
			VisualDiagnostics.RegisterSourceInfo(translate19 = new Translate(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Settings\\SettingsInterfacePage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 213, 25);
			Label label10;
			VisualDiagnostics.RegisterSourceInfo(label10 = new Label(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Settings\\SettingsInterfacePage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 208, 22);
			Translate translate20;
			VisualDiagnostics.RegisterSourceInfo(translate20 = new Translate(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Settings\\SettingsInterfacePage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 218, 25);
			Label label11;
			VisualDiagnostics.RegisterSourceInfo(label11 = new Label(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Settings\\SettingsInterfacePage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 215, 22);
			BindingExtension bindingExtension18;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension18 = new BindingExtension(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Settings\\SettingsInterfacePage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 222, 25);
			CheckSwitch checkSwitch;
			VisualDiagnostics.RegisterSourceInfo(checkSwitch = new CheckSwitch(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Settings\\SettingsInterfacePage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 219, 22);
			Grid grid2;
			VisualDiagnostics.RegisterSourceInfo(grid2 = new Grid(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Settings\\SettingsInterfacePage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 192, 18);
			BindingExtension bindingExtension19;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension19 = new BindingExtension(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Settings\\SettingsInterfacePage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 224, 36);
			Translate translate21;
			VisualDiagnostics.RegisterSourceInfo(translate21 = new Translate(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Settings\\SettingsInterfacePage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 224, 93);
			LabelSwitch labelSwitch3;
			VisualDiagnostics.RegisterSourceInfo(labelSwitch3 = new LabelSwitch(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Settings\\SettingsInterfacePage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 224, 18);
			BindingExtension bindingExtension20;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension20 = new BindingExtension(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Settings\\SettingsInterfacePage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 225, 36);
			Translate translate22;
			VisualDiagnostics.RegisterSourceInfo(translate22 = new Translate(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Settings\\SettingsInterfacePage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 225, 88);
			LabelSwitch labelSwitch4;
			VisualDiagnostics.RegisterSourceInfo(labelSwitch4 = new LabelSwitch(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Settings\\SettingsInterfacePage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 225, 18);
			BindingExtension bindingExtension21;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension21 = new BindingExtension(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Settings\\SettingsInterfacePage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 226, 36);
			Translate translate23;
			VisualDiagnostics.RegisterSourceInfo(translate23 = new Translate(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Settings\\SettingsInterfacePage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 226, 93);
			LabelSwitch labelSwitch5;
			VisualDiagnostics.RegisterSourceInfo(labelSwitch5 = new LabelSwitch(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Settings\\SettingsInterfacePage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 226, 18);
			BindingExtension bindingExtension22;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension22 = new BindingExtension(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Settings\\SettingsInterfacePage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 230, 36);
			Translate translate24;
			VisualDiagnostics.RegisterSourceInfo(translate24 = new Translate(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Settings\\SettingsInterfacePage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 230, 101);
			On on;
			VisualDiagnostics.RegisterSourceInfo(on = new On(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Settings\\SettingsInterfacePage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 233, 30);
			On on2;
			VisualDiagnostics.RegisterSourceInfo(on2 = new On(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Settings\\SettingsInterfacePage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 234, 30);
			OnPlatform<bool> onPlatform3;
			VisualDiagnostics.RegisterSourceInfo(onPlatform3 = new OnPlatform<bool>(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Settings\\SettingsInterfacePage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 232, 26);
			LabelSwitch labelSwitch6;
			VisualDiagnostics.RegisterSourceInfo(labelSwitch6 = new LabelSwitch(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Settings\\SettingsInterfacePage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 230, 18);
			BindingExtension bindingExtension23;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension23 = new BindingExtension(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Settings\\SettingsInterfacePage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 240, 36);
			Translate translate25;
			VisualDiagnostics.RegisterSourceInfo(translate25 = new Translate(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Settings\\SettingsInterfacePage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 240, 80);
			LabelSwitch labelSwitch7;
			VisualDiagnostics.RegisterSourceInfo(labelSwitch7 = new LabelSwitch(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Settings\\SettingsInterfacePage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 240, 18);
			ColumnDefinition columnDefinition5;
			VisualDiagnostics.RegisterSourceInfo(columnDefinition5 = new ColumnDefinition(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Settings\\SettingsInterfacePage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 246, 26);
			ColumnDefinition columnDefinition6;
			VisualDiagnostics.RegisterSourceInfo(columnDefinition6 = new ColumnDefinition(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Settings\\SettingsInterfacePage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 247, 26);
			Translate translate26;
			VisualDiagnostics.RegisterSourceInfo(translate26 = new Translate(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Settings\\SettingsInterfacePage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 253, 25);
			Label label12;
			VisualDiagnostics.RegisterSourceInfo(label12 = new Label(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Settings\\SettingsInterfacePage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 249, 22);
			BindingExtension bindingExtension24;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension24 = new BindingExtension(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Settings\\SettingsInterfacePage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 259, 25);
			Label label13;
			VisualDiagnostics.RegisterSourceInfo(label13 = new Label(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Settings\\SettingsInterfacePage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 254, 22);
			Grid grid3;
			VisualDiagnostics.RegisterSourceInfo(grid3 = new Grid(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Settings\\SettingsInterfacePage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 244, 18);
			BindingExtension bindingExtension25;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension25 = new BindingExtension(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Settings\\SettingsInterfacePage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 269, 21);
			Stepper stepper2;
			VisualDiagnostics.RegisterSourceInfo(stepper2 = new Stepper(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Settings\\SettingsInterfacePage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 262, 18);
			ColumnDefinition columnDefinition7;
			VisualDiagnostics.RegisterSourceInfo(columnDefinition7 = new ColumnDefinition(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Settings\\SettingsInterfacePage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 275, 26);
			ColumnDefinition columnDefinition8;
			VisualDiagnostics.RegisterSourceInfo(columnDefinition8 = new ColumnDefinition(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Settings\\SettingsInterfacePage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 276, 26);
			RowDefinition rowDefinition3;
			VisualDiagnostics.RegisterSourceInfo(rowDefinition3 = new RowDefinition(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Settings\\SettingsInterfacePage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 279, 26);
			RowDefinition rowDefinition4;
			VisualDiagnostics.RegisterSourceInfo(rowDefinition4 = new RowDefinition(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Settings\\SettingsInterfacePage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 280, 26);
			BindingExtension bindingExtension26;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension26 = new BindingExtension(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Settings\\SettingsInterfacePage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 286, 25);
			Translate translate27;
			VisualDiagnostics.RegisterSourceInfo(translate27 = new Translate(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Settings\\SettingsInterfacePage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 287, 25);
			Label label14;
			VisualDiagnostics.RegisterSourceInfo(label14 = new Label(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Settings\\SettingsInterfacePage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 282, 22);
			StaticResourceExtension staticResourceExtension7;
			VisualDiagnostics.RegisterSourceInfo(staticResourceExtension7 = new StaticResourceExtension(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Settings\\SettingsInterfacePage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 293, 25);
			BindingExtension bindingExtension27;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension27 = new BindingExtension(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Settings\\SettingsInterfacePage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 293, 25);
			Translate translate28;
			VisualDiagnostics.RegisterSourceInfo(translate28 = new Translate(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Settings\\SettingsInterfacePage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 294, 25);
			Label label15;
			VisualDiagnostics.RegisterSourceInfo(label15 = new Label(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Settings\\SettingsInterfacePage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 289, 22);
			Translate translate29;
			VisualDiagnostics.RegisterSourceInfo(translate29 = new Translate(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Settings\\SettingsInterfacePage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 300, 25);
			Label label16;
			VisualDiagnostics.RegisterSourceInfo(label16 = new Label(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Settings\\SettingsInterfacePage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 297, 22);
			BindingExtension bindingExtension28;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension28 = new BindingExtension(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Settings\\SettingsInterfacePage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 304, 25);
			Switch @switch;
			VisualDiagnostics.RegisterSourceInfo(@switch = new Switch(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Settings\\SettingsInterfacePage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 301, 22);
			Grid grid4;
			VisualDiagnostics.RegisterSourceInfo(grid4 = new Grid(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Settings\\SettingsInterfacePage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 273, 18);
			Translate translate30;
			VisualDiagnostics.RegisterSourceInfo(translate30 = new Translate(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Settings\\SettingsInterfacePage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 311, 21);
			Label label17;
			VisualDiagnostics.RegisterSourceInfo(label17 = new Label(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Settings\\SettingsInterfacePage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 308, 18);
			List<string> fuelConsumptionUnits;
			VisualDiagnostics.RegisterSourceInfo(fuelConsumptionUnits = StaticLists.FuelConsumptionUnits, new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Settings\\SettingsInterfacePage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 312, 25);
			BindingExtension bindingExtension29;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension29 = new BindingExtension(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Settings\\SettingsInterfacePage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 312, 25);
			StaticResourceExtension staticResourceExtension8;
			VisualDiagnostics.RegisterSourceInfo(staticResourceExtension8 = new StaticResourceExtension(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Settings\\SettingsInterfacePage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 312, 106);
			BindingExtension bindingExtension30;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension30 = new BindingExtension(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Settings\\SettingsInterfacePage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 312, 106);
			Picker picker4;
			VisualDiagnostics.RegisterSourceInfo(picker4 = new Picker(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Settings\\SettingsInterfacePage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 312, 18);
			ColumnDefinition columnDefinition9;
			VisualDiagnostics.RegisterSourceInfo(columnDefinition9 = new ColumnDefinition(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Settings\\SettingsInterfacePage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 336, 26);
			ColumnDefinition columnDefinition10;
			VisualDiagnostics.RegisterSourceInfo(columnDefinition10 = new ColumnDefinition(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Settings\\SettingsInterfacePage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 337, 26);
			RowDefinition rowDefinition5;
			VisualDiagnostics.RegisterSourceInfo(rowDefinition5 = new RowDefinition(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Settings\\SettingsInterfacePage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 340, 26);
			RowDefinition rowDefinition6;
			VisualDiagnostics.RegisterSourceInfo(rowDefinition6 = new RowDefinition(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Settings\\SettingsInterfacePage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 341, 26);
			BindingExtension bindingExtension31;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension31 = new BindingExtension(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Settings\\SettingsInterfacePage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 347, 25);
			Translate translate31;
			VisualDiagnostics.RegisterSourceInfo(translate31 = new Translate(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Settings\\SettingsInterfacePage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 348, 25);
			Label label18;
			VisualDiagnostics.RegisterSourceInfo(label18 = new Label(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Settings\\SettingsInterfacePage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 343, 22);
			StaticResourceExtension staticResourceExtension9;
			VisualDiagnostics.RegisterSourceInfo(staticResourceExtension9 = new StaticResourceExtension(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Settings\\SettingsInterfacePage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 354, 25);
			BindingExtension bindingExtension32;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension32 = new BindingExtension(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Settings\\SettingsInterfacePage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 354, 25);
			Translate translate32;
			VisualDiagnostics.RegisterSourceInfo(translate32 = new Translate(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Settings\\SettingsInterfacePage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 355, 25);
			Label label19;
			VisualDiagnostics.RegisterSourceInfo(label19 = new Label(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Settings\\SettingsInterfacePage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 350, 22);
			Translate translate33;
			VisualDiagnostics.RegisterSourceInfo(translate33 = new Translate(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Settings\\SettingsInterfacePage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 361, 25);
			Label label20;
			VisualDiagnostics.RegisterSourceInfo(label20 = new Label(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Settings\\SettingsInterfacePage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 358, 22);
			BindingExtension bindingExtension33;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension33 = new BindingExtension(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Settings\\SettingsInterfacePage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 365, 25);
			Switch switch2;
			VisualDiagnostics.RegisterSourceInfo(switch2 = new Switch(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Settings\\SettingsInterfacePage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 362, 22);
			Grid grid5;
			VisualDiagnostics.RegisterSourceInfo(grid5 = new Grid(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Settings\\SettingsInterfacePage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 334, 18);
			BindingExtension bindingExtension34;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension34 = new BindingExtension(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Settings\\SettingsInterfacePage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 372, 21);
			ColumnDefinition columnDefinition11;
			VisualDiagnostics.RegisterSourceInfo(columnDefinition11 = new ColumnDefinition(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Settings\\SettingsInterfacePage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 375, 26);
			ColumnDefinition columnDefinition12;
			VisualDiagnostics.RegisterSourceInfo(columnDefinition12 = new ColumnDefinition(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Settings\\SettingsInterfacePage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 376, 26);
			RowDefinition rowDefinition7;
			VisualDiagnostics.RegisterSourceInfo(rowDefinition7 = new RowDefinition(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Settings\\SettingsInterfacePage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 379, 26);
			RowDefinition rowDefinition8;
			VisualDiagnostics.RegisterSourceInfo(rowDefinition8 = new RowDefinition(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Settings\\SettingsInterfacePage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 380, 26);
			BindingExtension bindingExtension35;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension35 = new BindingExtension(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Settings\\SettingsInterfacePage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 386, 25);
			Translate translate34;
			VisualDiagnostics.RegisterSourceInfo(translate34 = new Translate(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Settings\\SettingsInterfacePage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 387, 25);
			Label label21;
			VisualDiagnostics.RegisterSourceInfo(label21 = new Label(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Settings\\SettingsInterfacePage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 382, 22);
			StaticResourceExtension staticResourceExtension10;
			VisualDiagnostics.RegisterSourceInfo(staticResourceExtension10 = new StaticResourceExtension(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Settings\\SettingsInterfacePage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 393, 25);
			BindingExtension bindingExtension36;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension36 = new BindingExtension(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Settings\\SettingsInterfacePage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 393, 25);
			Translate translate35;
			VisualDiagnostics.RegisterSourceInfo(translate35 = new Translate(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Settings\\SettingsInterfacePage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 394, 25);
			Label label22;
			VisualDiagnostics.RegisterSourceInfo(label22 = new Label(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Settings\\SettingsInterfacePage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 389, 22);
			Translate translate36;
			VisualDiagnostics.RegisterSourceInfo(translate36 = new Translate(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Settings\\SettingsInterfacePage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 400, 25);
			Label label23;
			VisualDiagnostics.RegisterSourceInfo(label23 = new Label(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Settings\\SettingsInterfacePage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 397, 22);
			BindingExtension bindingExtension37;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension37 = new BindingExtension(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Settings\\SettingsInterfacePage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 404, 25);
			Switch switch3;
			VisualDiagnostics.RegisterSourceInfo(switch3 = new Switch(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Settings\\SettingsInterfacePage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 401, 22);
			Grid grid6;
			VisualDiagnostics.RegisterSourceInfo(grid6 = new Grid(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Settings\\SettingsInterfacePage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 370, 18);
			ColumnDefinition columnDefinition13;
			VisualDiagnostics.RegisterSourceInfo(columnDefinition13 = new ColumnDefinition(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Settings\\SettingsInterfacePage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 410, 26);
			ColumnDefinition columnDefinition14;
			VisualDiagnostics.RegisterSourceInfo(columnDefinition14 = new ColumnDefinition(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Settings\\SettingsInterfacePage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 411, 26);
			RowDefinition rowDefinition9;
			VisualDiagnostics.RegisterSourceInfo(rowDefinition9 = new RowDefinition(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Settings\\SettingsInterfacePage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 414, 26);
			RowDefinition rowDefinition10;
			VisualDiagnostics.RegisterSourceInfo(rowDefinition10 = new RowDefinition(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Settings\\SettingsInterfacePage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 415, 26);
			BindingExtension bindingExtension38;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension38 = new BindingExtension(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Settings\\SettingsInterfacePage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 421, 25);
			Translate translate37;
			VisualDiagnostics.RegisterSourceInfo(translate37 = new Translate(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Settings\\SettingsInterfacePage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 422, 25);
			Label label24;
			VisualDiagnostics.RegisterSourceInfo(label24 = new Label(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Settings\\SettingsInterfacePage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 417, 22);
			StaticResourceExtension staticResourceExtension11;
			VisualDiagnostics.RegisterSourceInfo(staticResourceExtension11 = new StaticResourceExtension(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Settings\\SettingsInterfacePage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 428, 25);
			BindingExtension bindingExtension39;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension39 = new BindingExtension(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Settings\\SettingsInterfacePage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 428, 25);
			Translate translate38;
			VisualDiagnostics.RegisterSourceInfo(translate38 = new Translate(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Settings\\SettingsInterfacePage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 429, 25);
			Label label25;
			VisualDiagnostics.RegisterSourceInfo(label25 = new Label(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Settings\\SettingsInterfacePage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 424, 22);
			Translate translate39;
			VisualDiagnostics.RegisterSourceInfo(translate39 = new Translate(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Settings\\SettingsInterfacePage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 435, 25);
			Label label26;
			VisualDiagnostics.RegisterSourceInfo(label26 = new Label(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Settings\\SettingsInterfacePage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 432, 22);
			BindingExtension bindingExtension40;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension40 = new BindingExtension(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Settings\\SettingsInterfacePage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 439, 25);
			Switch switch4;
			VisualDiagnostics.RegisterSourceInfo(switch4 = new Switch(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Settings\\SettingsInterfacePage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 436, 22);
			Grid grid7;
			VisualDiagnostics.RegisterSourceInfo(grid7 = new Grid(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Settings\\SettingsInterfacePage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 408, 18);
			ColumnDefinition columnDefinition15;
			VisualDiagnostics.RegisterSourceInfo(columnDefinition15 = new ColumnDefinition(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Settings\\SettingsInterfacePage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 445, 26);
			ColumnDefinition columnDefinition16;
			VisualDiagnostics.RegisterSourceInfo(columnDefinition16 = new ColumnDefinition(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Settings\\SettingsInterfacePage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 446, 26);
			RowDefinition rowDefinition11;
			VisualDiagnostics.RegisterSourceInfo(rowDefinition11 = new RowDefinition(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Settings\\SettingsInterfacePage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 449, 26);
			RowDefinition rowDefinition12;
			VisualDiagnostics.RegisterSourceInfo(rowDefinition12 = new RowDefinition(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Settings\\SettingsInterfacePage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 450, 26);
			BindingExtension bindingExtension41;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension41 = new BindingExtension(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Settings\\SettingsInterfacePage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 456, 25);
			Translate translate40;
			VisualDiagnostics.RegisterSourceInfo(translate40 = new Translate(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Settings\\SettingsInterfacePage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 457, 25);
			Label label27;
			VisualDiagnostics.RegisterSourceInfo(label27 = new Label(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Settings\\SettingsInterfacePage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 452, 22);
			StaticResourceExtension staticResourceExtension12;
			VisualDiagnostics.RegisterSourceInfo(staticResourceExtension12 = new StaticResourceExtension(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Settings\\SettingsInterfacePage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 463, 25);
			BindingExtension bindingExtension42;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension42 = new BindingExtension(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Settings\\SettingsInterfacePage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 463, 25);
			Translate translate41;
			VisualDiagnostics.RegisterSourceInfo(translate41 = new Translate(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Settings\\SettingsInterfacePage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 464, 25);
			Label label28;
			VisualDiagnostics.RegisterSourceInfo(label28 = new Label(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Settings\\SettingsInterfacePage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 459, 22);
			Translate translate42;
			VisualDiagnostics.RegisterSourceInfo(translate42 = new Translate(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Settings\\SettingsInterfacePage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 470, 25);
			Label label29;
			VisualDiagnostics.RegisterSourceInfo(label29 = new Label(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Settings\\SettingsInterfacePage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 467, 22);
			BindingExtension bindingExtension43;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension43 = new BindingExtension(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Settings\\SettingsInterfacePage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 474, 25);
			Switch switch5;
			VisualDiagnostics.RegisterSourceInfo(switch5 = new Switch(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Settings\\SettingsInterfacePage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 471, 22);
			Grid grid8;
			VisualDiagnostics.RegisterSourceInfo(grid8 = new Grid(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Settings\\SettingsInterfacePage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 443, 18);
			ColumnDefinition columnDefinition17;
			VisualDiagnostics.RegisterSourceInfo(columnDefinition17 = new ColumnDefinition(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Settings\\SettingsInterfacePage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 479, 26);
			ColumnDefinition columnDefinition18;
			VisualDiagnostics.RegisterSourceInfo(columnDefinition18 = new ColumnDefinition(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Settings\\SettingsInterfacePage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 480, 26);
			RowDefinition rowDefinition13;
			VisualDiagnostics.RegisterSourceInfo(rowDefinition13 = new RowDefinition(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Settings\\SettingsInterfacePage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 483, 26);
			RowDefinition rowDefinition14;
			VisualDiagnostics.RegisterSourceInfo(rowDefinition14 = new RowDefinition(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Settings\\SettingsInterfacePage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 484, 26);
			BindingExtension bindingExtension44;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension44 = new BindingExtension(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Settings\\SettingsInterfacePage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 490, 25);
			Translate translate43;
			VisualDiagnostics.RegisterSourceInfo(translate43 = new Translate(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Settings\\SettingsInterfacePage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 491, 25);
			Label label30;
			VisualDiagnostics.RegisterSourceInfo(label30 = new Label(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Settings\\SettingsInterfacePage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 486, 22);
			StaticResourceExtension staticResourceExtension13;
			VisualDiagnostics.RegisterSourceInfo(staticResourceExtension13 = new StaticResourceExtension(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Settings\\SettingsInterfacePage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 497, 25);
			BindingExtension bindingExtension45;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension45 = new BindingExtension(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Settings\\SettingsInterfacePage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 497, 25);
			Translate translate44;
			VisualDiagnostics.RegisterSourceInfo(translate44 = new Translate(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Settings\\SettingsInterfacePage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 498, 25);
			Label label31;
			VisualDiagnostics.RegisterSourceInfo(label31 = new Label(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Settings\\SettingsInterfacePage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 493, 22);
			Translate translate45;
			VisualDiagnostics.RegisterSourceInfo(translate45 = new Translate(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Settings\\SettingsInterfacePage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 504, 25);
			Label label32;
			VisualDiagnostics.RegisterSourceInfo(label32 = new Label(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Settings\\SettingsInterfacePage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 501, 22);
			BindingExtension bindingExtension46;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension46 = new BindingExtension(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Settings\\SettingsInterfacePage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 508, 25);
			Switch switch6;
			VisualDiagnostics.RegisterSourceInfo(switch6 = new Switch(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Settings\\SettingsInterfacePage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 505, 22);
			Grid grid9;
			VisualDiagnostics.RegisterSourceInfo(grid9 = new Grid(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Settings\\SettingsInterfacePage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 477, 18);
			ColumnDefinition columnDefinition19;
			VisualDiagnostics.RegisterSourceInfo(columnDefinition19 = new ColumnDefinition(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Settings\\SettingsInterfacePage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 515, 26);
			ColumnDefinition columnDefinition20;
			VisualDiagnostics.RegisterSourceInfo(columnDefinition20 = new ColumnDefinition(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Settings\\SettingsInterfacePage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 516, 26);
			RowDefinition rowDefinition15;
			VisualDiagnostics.RegisterSourceInfo(rowDefinition15 = new RowDefinition(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Settings\\SettingsInterfacePage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 519, 26);
			RowDefinition rowDefinition16;
			VisualDiagnostics.RegisterSourceInfo(rowDefinition16 = new RowDefinition(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Settings\\SettingsInterfacePage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 520, 26);
			BindingExtension bindingExtension47;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension47 = new BindingExtension(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Settings\\SettingsInterfacePage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 526, 25);
			Label label33;
			VisualDiagnostics.RegisterSourceInfo(label33 = new Label(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Settings\\SettingsInterfacePage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 522, 22);
			StaticResourceExtension staticResourceExtension14;
			VisualDiagnostics.RegisterSourceInfo(staticResourceExtension14 = new StaticResourceExtension(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Settings\\SettingsInterfacePage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 533, 25);
			BindingExtension bindingExtension48;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension48 = new BindingExtension(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Settings\\SettingsInterfacePage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 533, 25);
			Label label34;
			VisualDiagnostics.RegisterSourceInfo(label34 = new Label(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Settings\\SettingsInterfacePage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 529, 22);
			Translate translate46;
			VisualDiagnostics.RegisterSourceInfo(translate46 = new Translate(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Settings\\SettingsInterfacePage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 540, 25);
			Label label35;
			VisualDiagnostics.RegisterSourceInfo(label35 = new Label(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Settings\\SettingsInterfacePage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 537, 22);
			BindingExtension bindingExtension49;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension49 = new BindingExtension(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Settings\\SettingsInterfacePage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 544, 25);
			Switch switch7;
			VisualDiagnostics.RegisterSourceInfo(switch7 = new Switch(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Settings\\SettingsInterfacePage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 541, 22);
			Grid grid10;
			VisualDiagnostics.RegisterSourceInfo(grid10 = new Grid(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Settings\\SettingsInterfacePage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 513, 18);
			ColumnDefinition columnDefinition21;
			VisualDiagnostics.RegisterSourceInfo(columnDefinition21 = new ColumnDefinition(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Settings\\SettingsInterfacePage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 555, 26);
			ColumnDefinition columnDefinition22;
			VisualDiagnostics.RegisterSourceInfo(columnDefinition22 = new ColumnDefinition(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Settings\\SettingsInterfacePage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 556, 26);
			RowDefinition rowDefinition17;
			VisualDiagnostics.RegisterSourceInfo(rowDefinition17 = new RowDefinition(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Settings\\SettingsInterfacePage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 559, 26);
			RowDefinition rowDefinition18;
			VisualDiagnostics.RegisterSourceInfo(rowDefinition18 = new RowDefinition(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Settings\\SettingsInterfacePage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 560, 26);
			BindingExtension bindingExtension50;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension50 = new BindingExtension(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Settings\\SettingsInterfacePage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 566, 25);
			Label label36;
			VisualDiagnostics.RegisterSourceInfo(label36 = new Label(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Settings\\SettingsInterfacePage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 562, 22);
			StaticResourceExtension staticResourceExtension15;
			VisualDiagnostics.RegisterSourceInfo(staticResourceExtension15 = new StaticResourceExtension(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Settings\\SettingsInterfacePage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 573, 25);
			BindingExtension bindingExtension51;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension51 = new BindingExtension(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Settings\\SettingsInterfacePage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 573, 25);
			Label label37;
			VisualDiagnostics.RegisterSourceInfo(label37 = new Label(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Settings\\SettingsInterfacePage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 569, 22);
			Translate translate47;
			VisualDiagnostics.RegisterSourceInfo(translate47 = new Translate(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Settings\\SettingsInterfacePage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 580, 25);
			Label label38;
			VisualDiagnostics.RegisterSourceInfo(label38 = new Label(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Settings\\SettingsInterfacePage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 577, 22);
			BindingExtension bindingExtension52;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension52 = new BindingExtension(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Settings\\SettingsInterfacePage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 584, 25);
			Switch switch8;
			VisualDiagnostics.RegisterSourceInfo(switch8 = new Switch(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Settings\\SettingsInterfacePage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 581, 22);
			Grid grid11;
			VisualDiagnostics.RegisterSourceInfo(grid11 = new Grid(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Settings\\SettingsInterfacePage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 553, 18);
			ColumnDefinition columnDefinition23;
			VisualDiagnostics.RegisterSourceInfo(columnDefinition23 = new ColumnDefinition(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Settings\\SettingsInterfacePage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 590, 26);
			ColumnDefinition columnDefinition24;
			VisualDiagnostics.RegisterSourceInfo(columnDefinition24 = new ColumnDefinition(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Settings\\SettingsInterfacePage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 591, 26);
			RowDefinition rowDefinition19;
			VisualDiagnostics.RegisterSourceInfo(rowDefinition19 = new RowDefinition(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Settings\\SettingsInterfacePage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 594, 26);
			RowDefinition rowDefinition20;
			VisualDiagnostics.RegisterSourceInfo(rowDefinition20 = new RowDefinition(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Settings\\SettingsInterfacePage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 595, 26);
			BindingExtension bindingExtension53;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension53 = new BindingExtension(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Settings\\SettingsInterfacePage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 601, 25);
			Label label39;
			VisualDiagnostics.RegisterSourceInfo(label39 = new Label(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Settings\\SettingsInterfacePage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 597, 22);
			StaticResourceExtension staticResourceExtension16;
			VisualDiagnostics.RegisterSourceInfo(staticResourceExtension16 = new StaticResourceExtension(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Settings\\SettingsInterfacePage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 608, 25);
			BindingExtension bindingExtension54;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension54 = new BindingExtension(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Settings\\SettingsInterfacePage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 608, 25);
			Label label40;
			VisualDiagnostics.RegisterSourceInfo(label40 = new Label(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Settings\\SettingsInterfacePage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 604, 22);
			Translate translate48;
			VisualDiagnostics.RegisterSourceInfo(translate48 = new Translate(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Settings\\SettingsInterfacePage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 615, 25);
			Label label41;
			VisualDiagnostics.RegisterSourceInfo(label41 = new Label(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Settings\\SettingsInterfacePage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 612, 22);
			BindingExtension bindingExtension55;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension55 = new BindingExtension(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Settings\\SettingsInterfacePage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 619, 25);
			Switch switch9;
			VisualDiagnostics.RegisterSourceInfo(switch9 = new Switch(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Settings\\SettingsInterfacePage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 616, 22);
			Grid grid12;
			VisualDiagnostics.RegisterSourceInfo(grid12 = new Grid(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Settings\\SettingsInterfacePage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 588, 18);
			StackLayout stackLayout4;
			VisualDiagnostics.RegisterSourceInfo(stackLayout4 = new StackLayout(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Settings\\SettingsInterfacePage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 30, 14);
			ScrollView scrollView;
			VisualDiagnostics.RegisterSourceInfo(scrollView = new ScrollView(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Settings\\SettingsInterfacePage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 29, 10);
			VisualDiagnostics.RegisterSourceInfo(this, new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Settings\\SettingsInterfacePage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 2, 2);
			NameScope nameScope = NameScope.GetNameScope(this) ?? new NameScope();
			NameScope.SetNameScope(this, nameScope);
			nameScope.RegisterName("panelLanguage", stackLayout);
			if (stackLayout.StyleId == null)
			{
				stackLayout.StyleId = "panelLanguage";
			}
			nameScope.RegisterName("btnApplyLangugage", button);
			if (button.StyleId == null)
			{
				button.StyleId = "btnApplyLangugage";
			}
			nameScope.RegisterName("btnFontSizePlus", button2);
			if (button2.StyleId == null)
			{
				button2.StyleId = "btnFontSizePlus";
			}
			nameScope.RegisterName("btnFontSizeMinus", button3);
			if (button3.StyleId == null)
			{
				button3.StyleId = "btnFontSizeMinus";
			}
			nameScope.RegisterName("interfaceThemeGroup", sfRadioGroup);
			if (sfRadioGroup.StyleId == null)
			{
				sfRadioGroup.StyleId = "interfaceThemeGroup";
			}
			nameScope.RegisterName("btnLight", sfRadioButton);
			if (sfRadioButton.StyleId == null)
			{
				sfRadioButton.StyleId = "btnLight";
			}
			nameScope.RegisterName("btnDark", sfRadioButton2);
			if (sfRadioButton2.StyleId == null)
			{
				sfRadioButton2.StyleId = "btnDark";
			}
			nameScope.RegisterName("btnMainScreenConfiguration", button4);
			if (button4.StyleId == null)
			{
				button4.StyleId = "btnMainScreenConfiguration";
			}
			nameScope.RegisterName("chartStylePicker", picker3);
			if (picker3.StyleId == null)
			{
				picker3.StyleId = "chartStylePicker";
			}
			this.panelLanguage = stackLayout;
			this.btnApplyLangugage = button;
			this.btnFontSizePlus = button2;
			this.btnFontSizeMinus = button3;
			this.interfaceThemeGroup = sfRadioGroup;
			this.btnLight = sfRadioButton;
			this.btnDark = sfRadioButton2;
			this.btnMainScreenConfiguration = button4;
			this.chartStylePicker = picker3;
			this.Resources = resourceDictionary;
			resourceDictionary.Add("BoolToNegativeConverter", boolToNegativeConverter);
			resourceDictionary.Add("TimeSpanTotalIntSecondsToDoubleConverter", timeSpanTotalIntSecondsToDoubleConverter);
			resourceDictionary.Add("EnumToIntConverter", enumToIntConverter);
			translate.Text = "SettingsPage_itemInterface.Content";
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
			xmlNamespaceResolver.Add("dash", "clr-namespace:CarScannerXamarinForms.Dashboard");
			xmlNamespaceResolver.Add("input", "clr-namespace:Plugin.InputKit.Shared.Controls;assembly=Plugin.InputKit");
			xmlNamespaceResolver.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver.Add("syncfusion", "clr-namespace:Syncfusion.XForms.Buttons;assembly=Syncfusion.Buttons.XForms");
			xmlNamespaceResolver.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider.Add(typeFromHandle2, new XamlTypeResolver(xmlNamespaceResolver, typeof(SettingsInterfacePage).GetTypeInfo().Assembly));
			xamlServiceProvider.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(13, 5)));
			object obj2 = markupExtension.ProvideValue(xamlServiceProvider);
			this.Title = obj2;
			this.SetValue(Page.PaddingProperty, new Thickness(0.0));
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
			xmlNamespaceResolver2.Add("dash", "clr-namespace:CarScannerXamarinForms.Dashboard");
			xmlNamespaceResolver2.Add("input", "clr-namespace:Plugin.InputKit.Shared.Controls;assembly=Plugin.InputKit");
			xmlNamespaceResolver2.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver2.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver2.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver2.Add("syncfusion", "clr-namespace:Syncfusion.XForms.Buttons;assembly=Syncfusion.Buttons.XForms");
			xmlNamespaceResolver2.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider2.Add(typeFromHandle4, new XamlTypeResolver(xmlNamespaceResolver2, typeof(SettingsInterfacePage).GetTypeInfo().Assembly));
			xamlServiceProvider2.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(16, 5)));
			DynamicResource dynamicResource = markupExtension2.ProvideValue(xamlServiceProvider2);
			this.SetDynamicResource(VisualElement.BackgroundColorProperty, dynamicResource.Key);
			this.Resources = resourceDictionary;
			scrollView.SetValue(ScrollView.OrientationProperty, 0);
			stackLayout4.SetValue(StackLayout.OrientationProperty, 0);
			onPlatform.Android = new Thickness(5.0, 0.0, 5.0, 0.0);
			onPlatform.WinPhone = new Thickness(0.0);
			onPlatform.iOS = new Thickness(5.0, 0.0, 5.0, 0.0);
			stackLayout4.SetValue(View.MarginProperty, onPlatform);
			stackLayout.SetValue(StackLayout.OrientationProperty, 0);
			translate2.Text = "Settings_Control_tbLanguage.Text";
			IMarkupExtension markupExtension3 = translate2;
			XamlServiceProvider xamlServiceProvider3 = new XamlServiceProvider();
			Type typeFromHandle5 = typeof(IProvideValueTarget);
			object[] array3 = new object[0 + 5];
			array3[0] = label;
			array3[1] = stackLayout;
			array3[2] = stackLayout4;
			array3[3] = scrollView;
			array3[4] = this;
			object obj4;
			xamlServiceProvider3.Add(typeFromHandle5, obj4 = new SimpleValueTargetProvider(array3, Label.TextProperty, nameScope));
			xamlServiceProvider3.Add(typeof(IReferenceProvider), obj4);
			Type typeFromHandle6 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver3 = new XmlNamespaceResolver();
			xmlNamespaceResolver3.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver3.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver3.Add("dash", "clr-namespace:CarScannerXamarinForms.Dashboard");
			xmlNamespaceResolver3.Add("input", "clr-namespace:Plugin.InputKit.Shared.Controls;assembly=Plugin.InputKit");
			xmlNamespaceResolver3.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver3.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver3.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver3.Add("syncfusion", "clr-namespace:Syncfusion.XForms.Buttons;assembly=Syncfusion.Buttons.XForms");
			xmlNamespaceResolver3.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider3.Add(typeFromHandle6, new XamlTypeResolver(xmlNamespaceResolver3, typeof(SettingsInterfacePage).GetTypeInfo().Assembly));
			xamlServiceProvider3.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(40, 28)));
			object obj5 = markupExtension3.ProvideValue(xamlServiceProvider3);
			label.Text = obj5;
			stackLayout.Children.Add(label);
			picker.SetValue(View.MarginProperty, new Thickness(0.0, 0.0, 0.0, 10.0));
			bindingExtension.Source = languageList;
			BindingBase bindingBase = bindingExtension.ProvideValue(null);
			picker.SetBinding(Picker.ItemsSourceProperty, bindingBase);
			bindingExtension2.Mode = 1;
			bindingExtension2.Path = "Language";
			bindingExtension2.TypedBinding = new TypedBinding<SharedSettings, int>(delegate(SharedSettings A_0)
			{
				if (A_0 != null)
				{
					return new ValueTuple<int, bool>(A_0.Language, true);
				}
				return default(ValueTuple<int, bool>);
			}, delegate(SharedSettings A_0, int A_1)
			{
				if (A_0 != null)
				{
					A_0.Language = A_1;
					return;
				}
			}, new Tuple<Func<SharedSettings, object>, string>[]
			{
				new Tuple<Func<SharedSettings, object>, string>((SharedSettings A_0) => A_0, "Language")
			});
			BindingBase bindingBase2 = bindingExtension2.ProvideValue(null);
			picker.SetBinding(Picker.SelectedIndexProperty, bindingBase2);
			stackLayout.Children.Add(picker);
			button.Clicked += this.btnApplyLanguage_Clicked;
			button.SetValue(Button.TextProperty, "OK");
			stackLayout.Children.Add(button);
			stackLayout4.Children.Add(stackLayout);
			bindingExtension3.Mode = 1;
			bindingExtension3.Path = "AutomaticallySwitchTheme";
			bindingExtension3.TypedBinding = new TypedBinding<SharedSettings, bool>(delegate(SharedSettings A_0)
			{
				if (A_0 != null)
				{
					return new ValueTuple<bool, bool>(A_0.AutomaticallySwitchTheme, true);
				}
				return default(ValueTuple<bool, bool>);
			}, delegate(SharedSettings A_0, bool A_1)
			{
				if (A_0 != null)
				{
					A_0.AutomaticallySwitchTheme = A_1;
					return;
				}
			}, new Tuple<Func<SharedSettings, object>, string>[]
			{
				new Tuple<Func<SharedSettings, object>, string>((SharedSettings A_0) => A_0, "AutomaticallySwitchTheme")
			});
			BindingBase bindingBase3 = bindingExtension3.ProvideValue(null);
			labelSwitch.SetBinding(LabelSwitch.IsToggledProperty, bindingBase3);
			bindingExtension4.Mode = 2;
			bindingExtension4.Path = "AutomaticallySwitchThemeAvailable";
			bindingExtension4.TypedBinding = new TypedBinding<SharedSettings, bool>(delegate(SharedSettings A_0)
			{
				if (A_0 != null)
				{
					return new ValueTuple<bool, bool>(A_0.AutomaticallySwitchThemeAvailable, true);
				}
				return default(ValueTuple<bool, bool>);
			}, null, new Tuple<Func<SharedSettings, object>, string>[]
			{
				new Tuple<Func<SharedSettings, object>, string>((SharedSettings A_0) => A_0, "AutomaticallySwitchThemeAvailable")
			});
			BindingBase bindingBase4 = bindingExtension4.ProvideValue(null);
			labelSwitch.SetBinding(VisualElement.IsVisibleProperty, bindingBase4);
			translate3.Text = "ios_AutomaticallySwitchTheme";
			IMarkupExtension markupExtension4 = translate3;
			XamlServiceProvider xamlServiceProvider4 = new XamlServiceProvider();
			Type typeFromHandle7 = typeof(IProvideValueTarget);
			object[] array4 = new object[0 + 4];
			array4[0] = labelSwitch;
			array4[1] = stackLayout4;
			array4[2] = scrollView;
			array4[3] = this;
			object obj6;
			xamlServiceProvider4.Add(typeFromHandle7, obj6 = new SimpleValueTargetProvider(array4, LabelSwitch.TextProperty, nameScope));
			xamlServiceProvider4.Add(typeof(IReferenceProvider), obj6);
			Type typeFromHandle8 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver4 = new XmlNamespaceResolver();
			xmlNamespaceResolver4.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver4.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver4.Add("dash", "clr-namespace:CarScannerXamarinForms.Dashboard");
			xmlNamespaceResolver4.Add("input", "clr-namespace:Plugin.InputKit.Shared.Controls;assembly=Plugin.InputKit");
			xmlNamespaceResolver4.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver4.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver4.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver4.Add("syncfusion", "clr-namespace:Syncfusion.XForms.Buttons;assembly=Syncfusion.Buttons.XForms");
			xmlNamespaceResolver4.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider4.Add(typeFromHandle8, new XamlTypeResolver(xmlNamespaceResolver4, typeof(SettingsInterfacePage).GetTypeInfo().Assembly));
			xamlServiceProvider4.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(54, 21)));
			object obj7 = markupExtension4.ProvideValue(xamlServiceProvider4);
			labelSwitch.Text = obj7;
			stackLayout4.Children.Add(labelSwitch);
			translate4.Text = "Settings_AdjustFontSize";
			IMarkupExtension markupExtension5 = translate4;
			XamlServiceProvider xamlServiceProvider5 = new XamlServiceProvider();
			Type typeFromHandle9 = typeof(IProvideValueTarget);
			object[] array5 = new object[0 + 6];
			array5[0] = span;
			array5[1] = formattedString;
			array5[2] = label2;
			array5[3] = stackLayout4;
			array5[4] = scrollView;
			array5[5] = this;
			object obj8;
			xamlServiceProvider5.Add(typeFromHandle9, obj8 = new SimpleValueTargetProvider(array5, Span.TextProperty, nameScope));
			xamlServiceProvider5.Add(typeof(IReferenceProvider), obj8);
			Type typeFromHandle10 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver5 = new XmlNamespaceResolver();
			xmlNamespaceResolver5.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver5.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver5.Add("dash", "clr-namespace:CarScannerXamarinForms.Dashboard");
			xmlNamespaceResolver5.Add("input", "clr-namespace:Plugin.InputKit.Shared.Controls;assembly=Plugin.InputKit");
			xmlNamespaceResolver5.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver5.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver5.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver5.Add("syncfusion", "clr-namespace:Syncfusion.XForms.Buttons;assembly=Syncfusion.Buttons.XForms");
			xmlNamespaceResolver5.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider5.Add(typeFromHandle10, new XamlTypeResolver(xmlNamespaceResolver5, typeof(SettingsInterfacePage).GetTypeInfo().Assembly));
			xamlServiceProvider5.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(60, 35)));
			object obj9 = markupExtension5.ProvideValue(xamlServiceProvider5);
			span.Text = obj9;
			formattedString.Spans.Add(span);
			span2.SetValue(Span.TextProperty, " ");
			formattedString.Spans.Add(span2);
			bindingExtension5.Mode = 2;
			bindingExtension5.StringFormat = "{0:+#;-#;0}";
			bindingExtension5.Path = "FontSizePatch";
			bindingExtension5.TypedBinding = new TypedBinding<SharedSettings, int>(delegate(SharedSettings A_0)
			{
				if (A_0 != null)
				{
					return new ValueTuple<int, bool>(A_0.FontSizePatch, true);
				}
				return default(ValueTuple<int, bool>);
			}, null, new Tuple<Func<SharedSettings, object>, string>[]
			{
				new Tuple<Func<SharedSettings, object>, string>((SharedSettings A_0) => A_0, "FontSizePatch")
			});
			BindingBase bindingBase5 = bindingExtension5.ProvideValue(null);
			span3.SetBinding(Span.TextProperty, bindingBase5);
			formattedString.Spans.Add(span3);
			label2.SetValue(Label.FormattedTextProperty, formattedString);
			stackLayout4.Children.Add(label2);
			button2.Clicked += this.btnFontSizePlus_Tapped;
			translate5.Text = "Settings_IncreaseFont";
			IMarkupExtension markupExtension6 = translate5;
			XamlServiceProvider xamlServiceProvider6 = new XamlServiceProvider();
			Type typeFromHandle11 = typeof(IProvideValueTarget);
			object[] array6 = new object[0 + 4];
			array6[0] = button2;
			array6[1] = stackLayout4;
			array6[2] = scrollView;
			array6[3] = this;
			object obj10;
			xamlServiceProvider6.Add(typeFromHandle11, obj10 = new SimpleValueTargetProvider(array6, Button.TextProperty, nameScope));
			xamlServiceProvider6.Add(typeof(IReferenceProvider), obj10);
			Type typeFromHandle12 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver6 = new XmlNamespaceResolver();
			xmlNamespaceResolver6.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver6.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver6.Add("dash", "clr-namespace:CarScannerXamarinForms.Dashboard");
			xmlNamespaceResolver6.Add("input", "clr-namespace:Plugin.InputKit.Shared.Controls;assembly=Plugin.InputKit");
			xmlNamespaceResolver6.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver6.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver6.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver6.Add("syncfusion", "clr-namespace:Syncfusion.XForms.Buttons;assembly=Syncfusion.Buttons.XForms");
			xmlNamespaceResolver6.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider6.Add(typeFromHandle12, new XamlTypeResolver(xmlNamespaceResolver6, typeof(SettingsInterfacePage).GetTypeInfo().Assembly));
			xamlServiceProvider6.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(69, 21)));
			object obj11 = markupExtension6.ProvideValue(xamlServiceProvider6);
			button2.Text = obj11;
			stackLayout4.Children.Add(button2);
			button3.Clicked += this.btnFontSizeMinus_Tapped;
			translate6.Text = "Settings_DecreaseFont";
			IMarkupExtension markupExtension7 = translate6;
			XamlServiceProvider xamlServiceProvider7 = new XamlServiceProvider();
			Type typeFromHandle13 = typeof(IProvideValueTarget);
			object[] array7 = new object[0 + 4];
			array7[0] = button3;
			array7[1] = stackLayout4;
			array7[2] = scrollView;
			array7[3] = this;
			object obj12;
			xamlServiceProvider7.Add(typeFromHandle13, obj12 = new SimpleValueTargetProvider(array7, Button.TextProperty, nameScope));
			xamlServiceProvider7.Add(typeof(IReferenceProvider), obj12);
			Type typeFromHandle14 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver7 = new XmlNamespaceResolver();
			xmlNamespaceResolver7.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver7.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver7.Add("dash", "clr-namespace:CarScannerXamarinForms.Dashboard");
			xmlNamespaceResolver7.Add("input", "clr-namespace:Plugin.InputKit.Shared.Controls;assembly=Plugin.InputKit");
			xmlNamespaceResolver7.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver7.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver7.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver7.Add("syncfusion", "clr-namespace:Syncfusion.XForms.Buttons;assembly=Syncfusion.Buttons.XForms");
			xmlNamespaceResolver7.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider7.Add(typeFromHandle14, new XamlTypeResolver(xmlNamespaceResolver7, typeof(SettingsInterfacePage).GetTypeInfo().Assembly));
			xamlServiceProvider7.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(73, 21)));
			object obj13 = markupExtension7.ProvideValue(xamlServiceProvider7);
			button3.Text = obj13;
			stackLayout4.Children.Add(button3);
			bindingExtension6.Mode = 2;
			staticResourceExtension.Key = "BoolToNegativeConverter";
			IMarkupExtension markupExtension8 = staticResourceExtension;
			XamlServiceProvider xamlServiceProvider8 = new XamlServiceProvider();
			Type typeFromHandle15 = typeof(IProvideValueTarget);
			object[] array8 = new object[0 + 5];
			array8[0] = bindingExtension6;
			array8[1] = sfRadioGroup;
			array8[2] = stackLayout4;
			array8[3] = scrollView;
			array8[4] = this;
			object obj14;
			xamlServiceProvider8.Add(typeFromHandle15, obj14 = new SimpleValueTargetProvider(array8, typeof(BindingExtension).GetRuntimeProperty("Converter"), nameScope));
			xamlServiceProvider8.Add(typeof(IReferenceProvider), obj14);
			Type typeFromHandle16 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver8 = new XmlNamespaceResolver();
			xmlNamespaceResolver8.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver8.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver8.Add("dash", "clr-namespace:CarScannerXamarinForms.Dashboard");
			xmlNamespaceResolver8.Add("input", "clr-namespace:Plugin.InputKit.Shared.Controls;assembly=Plugin.InputKit");
			xmlNamespaceResolver8.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver8.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver8.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver8.Add("syncfusion", "clr-namespace:Syncfusion.XForms.Buttons;assembly=Syncfusion.Buttons.XForms");
			xmlNamespaceResolver8.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider8.Add(typeFromHandle16, new XamlTypeResolver(xmlNamespaceResolver8, typeof(SettingsInterfacePage).GetTypeInfo().Assembly));
			xamlServiceProvider8.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(78, 21)));
			object obj15 = markupExtension8.ProvideValue(xamlServiceProvider8);
			bindingExtension6.Converter = obj15;
			bindingExtension6.Path = "AutomaticallySwitchTheme";
			bindingExtension6.TypedBinding = new TypedBinding<SharedSettings, bool>(delegate(SharedSettings A_0)
			{
				if (A_0 != null)
				{
					return new ValueTuple<bool, bool>(A_0.AutomaticallySwitchTheme, true);
				}
				return default(ValueTuple<bool, bool>);
			}, null, new Tuple<Func<SharedSettings, object>, string>[]
			{
				new Tuple<Func<SharedSettings, object>, string>((SharedSettings A_0) => A_0, "AutomaticallySwitchTheme")
			});
			BindingBase bindingBase6 = bindingExtension6.ProvideValue(null);
			sfRadioGroup.SetBinding(VisualElement.IsVisibleProperty, bindingBase6);
			sfRadioGroup.SetValue(StackLayout.OrientationProperty, 0);
			translate7.Text = "ios_InterfaceTheme";
			IMarkupExtension markupExtension9 = translate7;
			XamlServiceProvider xamlServiceProvider9 = new XamlServiceProvider();
			Type typeFromHandle17 = typeof(IProvideValueTarget);
			object[] array9 = new object[0 + 5];
			array9[0] = label3;
			array9[1] = sfRadioGroup;
			array9[2] = stackLayout4;
			array9[3] = scrollView;
			array9[4] = this;
			object obj16;
			xamlServiceProvider9.Add(typeFromHandle17, obj16 = new SimpleValueTargetProvider(array9, Label.TextProperty, nameScope));
			xamlServiceProvider9.Add(typeof(IReferenceProvider), obj16);
			Type typeFromHandle18 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver9 = new XmlNamespaceResolver();
			xmlNamespaceResolver9.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver9.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver9.Add("dash", "clr-namespace:CarScannerXamarinForms.Dashboard");
			xmlNamespaceResolver9.Add("input", "clr-namespace:Plugin.InputKit.Shared.Controls;assembly=Plugin.InputKit");
			xmlNamespaceResolver9.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver9.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver9.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver9.Add("syncfusion", "clr-namespace:Syncfusion.XForms.Buttons;assembly=Syncfusion.Buttons.XForms");
			xmlNamespaceResolver9.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider9.Add(typeFromHandle18, new XamlTypeResolver(xmlNamespaceResolver9, typeof(SettingsInterfacePage).GetTypeInfo().Assembly));
			xamlServiceProvider9.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(80, 28)));
			object obj17 = markupExtension9.ProvideValue(xamlServiceProvider9);
			label3.Text = obj17;
			sfRadioGroup.Children.Add(label3);
			dynamicResourceExtension2.Key = "TextColor";
			IMarkupExtension<DynamicResource> markupExtension10 = dynamicResourceExtension2;
			XamlServiceProvider xamlServiceProvider10 = new XamlServiceProvider();
			Type typeFromHandle19 = typeof(IProvideValueTarget);
			object[] array10 = new object[0 + 5];
			array10[0] = sfRadioButton;
			array10[1] = sfRadioGroup;
			array10[2] = stackLayout4;
			array10[3] = scrollView;
			array10[4] = this;
			object obj18;
			xamlServiceProvider10.Add(typeFromHandle19, obj18 = new SimpleValueTargetProvider(array10, ToggleButton.CheckedColorProperty, nameScope));
			xamlServiceProvider10.Add(typeof(IReferenceProvider), obj18);
			Type typeFromHandle20 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver10 = new XmlNamespaceResolver();
			xmlNamespaceResolver10.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver10.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver10.Add("dash", "clr-namespace:CarScannerXamarinForms.Dashboard");
			xmlNamespaceResolver10.Add("input", "clr-namespace:Plugin.InputKit.Shared.Controls;assembly=Plugin.InputKit");
			xmlNamespaceResolver10.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver10.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver10.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver10.Add("syncfusion", "clr-namespace:Syncfusion.XForms.Buttons;assembly=Syncfusion.Buttons.XForms");
			xmlNamespaceResolver10.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider10.Add(typeFromHandle20, new XamlTypeResolver(xmlNamespaceResolver10, typeof(SettingsInterfacePage).GetTypeInfo().Assembly));
			xamlServiceProvider10.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(83, 25)));
			DynamicResource dynamicResource2 = markupExtension10.ProvideValue(xamlServiceProvider10);
			sfRadioButton.SetDynamicResource(ToggleButton.CheckedColorProperty, dynamicResource2.Key);
			bindingExtension7.Mode = 1;
			staticResourceExtension2.Key = "BoolToNegativeConverter";
			IMarkupExtension markupExtension11 = staticResourceExtension2;
			XamlServiceProvider xamlServiceProvider11 = new XamlServiceProvider();
			Type typeFromHandle21 = typeof(IProvideValueTarget);
			object[] array11 = new object[0 + 6];
			array11[0] = bindingExtension7;
			array11[1] = sfRadioButton;
			array11[2] = sfRadioGroup;
			array11[3] = stackLayout4;
			array11[4] = scrollView;
			array11[5] = this;
			object obj19;
			xamlServiceProvider11.Add(typeFromHandle21, obj19 = new SimpleValueTargetProvider(array11, typeof(BindingExtension).GetRuntimeProperty("Converter"), nameScope));
			xamlServiceProvider11.Add(typeof(IReferenceProvider), obj19);
			Type typeFromHandle22 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver11 = new XmlNamespaceResolver();
			xmlNamespaceResolver11.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver11.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver11.Add("dash", "clr-namespace:CarScannerXamarinForms.Dashboard");
			xmlNamespaceResolver11.Add("input", "clr-namespace:Plugin.InputKit.Shared.Controls;assembly=Plugin.InputKit");
			xmlNamespaceResolver11.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver11.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver11.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver11.Add("syncfusion", "clr-namespace:Syncfusion.XForms.Buttons;assembly=Syncfusion.Buttons.XForms");
			xmlNamespaceResolver11.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider11.Add(typeFromHandle22, new XamlTypeResolver(xmlNamespaceResolver11, typeof(SettingsInterfacePage).GetTypeInfo().Assembly));
			xamlServiceProvider11.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(84, 25)));
			object obj20 = markupExtension11.ProvideValue(xamlServiceProvider11);
			bindingExtension7.Converter = obj20;
			bindingExtension7.Path = "DarkMode";
			bindingExtension7.TypedBinding = new TypedBinding<SharedSettings, bool>(delegate(SharedSettings A_0)
			{
				if (A_0 != null)
				{
					return new ValueTuple<bool, bool>(A_0.DarkMode, true);
				}
				return default(ValueTuple<bool, bool>);
			}, delegate(SharedSettings A_0, bool A_1)
			{
				if (A_0 != null)
				{
					A_0.DarkMode = A_1;
					return;
				}
			}, new Tuple<Func<SharedSettings, object>, string>[]
			{
				new Tuple<Func<SharedSettings, object>, string>((SharedSettings A_0) => A_0, "DarkMode")
			});
			BindingBase bindingBase7 = bindingExtension7.ProvideValue(null);
			sfRadioButton.SetBinding(ToggleButton.IsCheckedProperty, bindingBase7);
			sfRadioButton.SetValue(ToggleButton.IsThreeStateProperty, false);
			translate8.Text = "ios_DashboardThemeLight";
			IMarkupExtension markupExtension12 = translate8;
			XamlServiceProvider xamlServiceProvider12 = new XamlServiceProvider();
			Type typeFromHandle23 = typeof(IProvideValueTarget);
			object[] array12 = new object[0 + 5];
			array12[0] = sfRadioButton;
			array12[1] = sfRadioGroup;
			array12[2] = stackLayout4;
			array12[3] = scrollView;
			array12[4] = this;
			object obj21;
			xamlServiceProvider12.Add(typeFromHandle23, obj21 = new SimpleValueTargetProvider(array12, ToggleButton.TextProperty, nameScope));
			xamlServiceProvider12.Add(typeof(IReferenceProvider), obj21);
			Type typeFromHandle24 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver12 = new XmlNamespaceResolver();
			xmlNamespaceResolver12.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver12.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver12.Add("dash", "clr-namespace:CarScannerXamarinForms.Dashboard");
			xmlNamespaceResolver12.Add("input", "clr-namespace:Plugin.InputKit.Shared.Controls;assembly=Plugin.InputKit");
			xmlNamespaceResolver12.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver12.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver12.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver12.Add("syncfusion", "clr-namespace:Syncfusion.XForms.Buttons;assembly=Syncfusion.Buttons.XForms");
			xmlNamespaceResolver12.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider12.Add(typeFromHandle24, new XamlTypeResolver(xmlNamespaceResolver12, typeof(SettingsInterfacePage).GetTypeInfo().Assembly));
			xamlServiceProvider12.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(86, 25)));
			object obj22 = markupExtension12.ProvideValue(xamlServiceProvider12);
			sfRadioButton.Text = obj22;
			dynamicResourceExtension3.Key = "TextColor";
			IMarkupExtension<DynamicResource> markupExtension13 = dynamicResourceExtension3;
			XamlServiceProvider xamlServiceProvider13 = new XamlServiceProvider();
			Type typeFromHandle25 = typeof(IProvideValueTarget);
			object[] array13 = new object[0 + 5];
			array13[0] = sfRadioButton;
			array13[1] = sfRadioGroup;
			array13[2] = stackLayout4;
			array13[3] = scrollView;
			array13[4] = this;
			object obj23;
			xamlServiceProvider13.Add(typeFromHandle25, obj23 = new SimpleValueTargetProvider(array13, ToggleButton.TextColorProperty, nameScope));
			xamlServiceProvider13.Add(typeof(IReferenceProvider), obj23);
			Type typeFromHandle26 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver13 = new XmlNamespaceResolver();
			xmlNamespaceResolver13.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver13.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver13.Add("dash", "clr-namespace:CarScannerXamarinForms.Dashboard");
			xmlNamespaceResolver13.Add("input", "clr-namespace:Plugin.InputKit.Shared.Controls;assembly=Plugin.InputKit");
			xmlNamespaceResolver13.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver13.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver13.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver13.Add("syncfusion", "clr-namespace:Syncfusion.XForms.Buttons;assembly=Syncfusion.Buttons.XForms");
			xmlNamespaceResolver13.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider13.Add(typeFromHandle26, new XamlTypeResolver(xmlNamespaceResolver13, typeof(SettingsInterfacePage).GetTypeInfo().Assembly));
			xamlServiceProvider13.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(87, 25)));
			DynamicResource dynamicResource3 = markupExtension13.ProvideValue(xamlServiceProvider13);
			sfRadioButton.SetDynamicResource(ToggleButton.TextColorProperty, dynamicResource3.Key);
			dynamicResourceExtension4.Key = "TextColor";
			IMarkupExtension<DynamicResource> markupExtension14 = dynamicResourceExtension4;
			XamlServiceProvider xamlServiceProvider14 = new XamlServiceProvider();
			Type typeFromHandle27 = typeof(IProvideValueTarget);
			object[] array14 = new object[0 + 5];
			array14[0] = sfRadioButton;
			array14[1] = sfRadioGroup;
			array14[2] = stackLayout4;
			array14[3] = scrollView;
			array14[4] = this;
			object obj24;
			xamlServiceProvider14.Add(typeFromHandle27, obj24 = new SimpleValueTargetProvider(array14, ToggleButton.UncheckedColorProperty, nameScope));
			xamlServiceProvider14.Add(typeof(IReferenceProvider), obj24);
			Type typeFromHandle28 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver14 = new XmlNamespaceResolver();
			xmlNamespaceResolver14.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver14.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver14.Add("dash", "clr-namespace:CarScannerXamarinForms.Dashboard");
			xmlNamespaceResolver14.Add("input", "clr-namespace:Plugin.InputKit.Shared.Controls;assembly=Plugin.InputKit");
			xmlNamespaceResolver14.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver14.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver14.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver14.Add("syncfusion", "clr-namespace:Syncfusion.XForms.Buttons;assembly=Syncfusion.Buttons.XForms");
			xmlNamespaceResolver14.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider14.Add(typeFromHandle28, new XamlTypeResolver(xmlNamespaceResolver14, typeof(SettingsInterfacePage).GetTypeInfo().Assembly));
			xamlServiceProvider14.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(88, 25)));
			DynamicResource dynamicResource4 = markupExtension14.ProvideValue(xamlServiceProvider14);
			sfRadioButton.SetDynamicResource(ToggleButton.UncheckedColorProperty, dynamicResource4.Key);
			sfRadioGroup.Children.Add(sfRadioButton);
			dynamicResourceExtension5.Key = "TextColor";
			IMarkupExtension<DynamicResource> markupExtension15 = dynamicResourceExtension5;
			XamlServiceProvider xamlServiceProvider15 = new XamlServiceProvider();
			Type typeFromHandle29 = typeof(IProvideValueTarget);
			object[] array15 = new object[0 + 5];
			array15[0] = sfRadioButton2;
			array15[1] = sfRadioGroup;
			array15[2] = stackLayout4;
			array15[3] = scrollView;
			array15[4] = this;
			object obj25;
			xamlServiceProvider15.Add(typeFromHandle29, obj25 = new SimpleValueTargetProvider(array15, ToggleButton.CheckedColorProperty, nameScope));
			xamlServiceProvider15.Add(typeof(IReferenceProvider), obj25);
			Type typeFromHandle30 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver15 = new XmlNamespaceResolver();
			xmlNamespaceResolver15.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver15.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver15.Add("dash", "clr-namespace:CarScannerXamarinForms.Dashboard");
			xmlNamespaceResolver15.Add("input", "clr-namespace:Plugin.InputKit.Shared.Controls;assembly=Plugin.InputKit");
			xmlNamespaceResolver15.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver15.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver15.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver15.Add("syncfusion", "clr-namespace:Syncfusion.XForms.Buttons;assembly=Syncfusion.Buttons.XForms");
			xmlNamespaceResolver15.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider15.Add(typeFromHandle30, new XamlTypeResolver(xmlNamespaceResolver15, typeof(SettingsInterfacePage).GetTypeInfo().Assembly));
			xamlServiceProvider15.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(91, 25)));
			DynamicResource dynamicResource5 = markupExtension15.ProvideValue(xamlServiceProvider15);
			sfRadioButton2.SetDynamicResource(ToggleButton.CheckedColorProperty, dynamicResource5.Key);
			bindingExtension8.Mode = 1;
			bindingExtension8.Path = "DarkMode";
			bindingExtension8.TypedBinding = new TypedBinding<SharedSettings, bool>(delegate(SharedSettings A_0)
			{
				if (A_0 != null)
				{
					return new ValueTuple<bool, bool>(A_0.DarkMode, true);
				}
				return default(ValueTuple<bool, bool>);
			}, delegate(SharedSettings A_0, bool A_1)
			{
				if (A_0 != null)
				{
					A_0.DarkMode = A_1;
					return;
				}
			}, new Tuple<Func<SharedSettings, object>, string>[]
			{
				new Tuple<Func<SharedSettings, object>, string>((SharedSettings A_0) => A_0, "DarkMode")
			});
			BindingBase bindingBase8 = bindingExtension8.ProvideValue(null);
			sfRadioButton2.SetBinding(ToggleButton.IsCheckedProperty, bindingBase8);
			sfRadioButton2.SetValue(ToggleButton.IsThreeStateProperty, false);
			translate9.Text = "ios_DashboardThemeDark";
			IMarkupExtension markupExtension16 = translate9;
			XamlServiceProvider xamlServiceProvider16 = new XamlServiceProvider();
			Type typeFromHandle31 = typeof(IProvideValueTarget);
			object[] array16 = new object[0 + 5];
			array16[0] = sfRadioButton2;
			array16[1] = sfRadioGroup;
			array16[2] = stackLayout4;
			array16[3] = scrollView;
			array16[4] = this;
			object obj26;
			xamlServiceProvider16.Add(typeFromHandle31, obj26 = new SimpleValueTargetProvider(array16, ToggleButton.TextProperty, nameScope));
			xamlServiceProvider16.Add(typeof(IReferenceProvider), obj26);
			Type typeFromHandle32 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver16 = new XmlNamespaceResolver();
			xmlNamespaceResolver16.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver16.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver16.Add("dash", "clr-namespace:CarScannerXamarinForms.Dashboard");
			xmlNamespaceResolver16.Add("input", "clr-namespace:Plugin.InputKit.Shared.Controls;assembly=Plugin.InputKit");
			xmlNamespaceResolver16.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver16.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver16.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver16.Add("syncfusion", "clr-namespace:Syncfusion.XForms.Buttons;assembly=Syncfusion.Buttons.XForms");
			xmlNamespaceResolver16.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider16.Add(typeFromHandle32, new XamlTypeResolver(xmlNamespaceResolver16, typeof(SettingsInterfacePage).GetTypeInfo().Assembly));
			xamlServiceProvider16.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(94, 25)));
			object obj27 = markupExtension16.ProvideValue(xamlServiceProvider16);
			sfRadioButton2.Text = obj27;
			dynamicResourceExtension6.Key = "TextColor";
			IMarkupExtension<DynamicResource> markupExtension17 = dynamicResourceExtension6;
			XamlServiceProvider xamlServiceProvider17 = new XamlServiceProvider();
			Type typeFromHandle33 = typeof(IProvideValueTarget);
			object[] array17 = new object[0 + 5];
			array17[0] = sfRadioButton2;
			array17[1] = sfRadioGroup;
			array17[2] = stackLayout4;
			array17[3] = scrollView;
			array17[4] = this;
			object obj28;
			xamlServiceProvider17.Add(typeFromHandle33, obj28 = new SimpleValueTargetProvider(array17, ToggleButton.TextColorProperty, nameScope));
			xamlServiceProvider17.Add(typeof(IReferenceProvider), obj28);
			Type typeFromHandle34 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver17 = new XmlNamespaceResolver();
			xmlNamespaceResolver17.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver17.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver17.Add("dash", "clr-namespace:CarScannerXamarinForms.Dashboard");
			xmlNamespaceResolver17.Add("input", "clr-namespace:Plugin.InputKit.Shared.Controls;assembly=Plugin.InputKit");
			xmlNamespaceResolver17.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver17.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver17.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver17.Add("syncfusion", "clr-namespace:Syncfusion.XForms.Buttons;assembly=Syncfusion.Buttons.XForms");
			xmlNamespaceResolver17.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider17.Add(typeFromHandle34, new XamlTypeResolver(xmlNamespaceResolver17, typeof(SettingsInterfacePage).GetTypeInfo().Assembly));
			xamlServiceProvider17.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(95, 25)));
			DynamicResource dynamicResource6 = markupExtension17.ProvideValue(xamlServiceProvider17);
			sfRadioButton2.SetDynamicResource(ToggleButton.TextColorProperty, dynamicResource6.Key);
			dynamicResourceExtension7.Key = "TextColor";
			IMarkupExtension<DynamicResource> markupExtension18 = dynamicResourceExtension7;
			XamlServiceProvider xamlServiceProvider18 = new XamlServiceProvider();
			Type typeFromHandle35 = typeof(IProvideValueTarget);
			object[] array18 = new object[0 + 5];
			array18[0] = sfRadioButton2;
			array18[1] = sfRadioGroup;
			array18[2] = stackLayout4;
			array18[3] = scrollView;
			array18[4] = this;
			object obj29;
			xamlServiceProvider18.Add(typeFromHandle35, obj29 = new SimpleValueTargetProvider(array18, ToggleButton.UncheckedColorProperty, nameScope));
			xamlServiceProvider18.Add(typeof(IReferenceProvider), obj29);
			Type typeFromHandle36 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver18 = new XmlNamespaceResolver();
			xmlNamespaceResolver18.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver18.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver18.Add("dash", "clr-namespace:CarScannerXamarinForms.Dashboard");
			xmlNamespaceResolver18.Add("input", "clr-namespace:Plugin.InputKit.Shared.Controls;assembly=Plugin.InputKit");
			xmlNamespaceResolver18.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver18.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver18.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver18.Add("syncfusion", "clr-namespace:Syncfusion.XForms.Buttons;assembly=Syncfusion.Buttons.XForms");
			xmlNamespaceResolver18.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider18.Add(typeFromHandle36, new XamlTypeResolver(xmlNamespaceResolver18, typeof(SettingsInterfacePage).GetTypeInfo().Assembly));
			xamlServiceProvider18.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(96, 25)));
			DynamicResource dynamicResource7 = markupExtension18.ProvideValue(xamlServiceProvider18);
			sfRadioButton2.SetDynamicResource(ToggleButton.UncheckedColorProperty, dynamicResource7.Key);
			sfRadioGroup.Children.Add(sfRadioButton2);
			stackLayout4.Children.Add(sfRadioGroup);
			stackLayout2.SetValue(StackLayout.OrientationProperty, 0);
			stackLayout2.SetValue(RadioButtonGroup.GroupNameProperty, "UpdateOnlyVsiible");
			bindingExtension9.Mode = 1;
			bindingExtension9.Path = "LiveDataListPageUpdateOnlyVisible";
			bindingExtension9.TypedBinding = new TypedBinding<SharedSettings, bool>(delegate(SharedSettings A_0)
			{
				if (A_0 != null)
				{
					return new ValueTuple<bool, bool>(A_0.LiveDataListPageUpdateOnlyVisible, true);
				}
				return default(ValueTuple<bool, bool>);
			}, delegate(SharedSettings A_0, bool A_1)
			{
				if (A_0 != null)
				{
					A_0.LiveDataListPageUpdateOnlyVisible = A_1;
					return;
				}
			}, new Tuple<Func<SharedSettings, object>, string>[]
			{
				new Tuple<Func<SharedSettings, object>, string>((SharedSettings A_0) => A_0, "LiveDataListPageUpdateOnlyVisible")
			});
			BindingBase bindingBase9 = bindingExtension9.ProvideValue(null);
			stackLayout2.SetBinding(RadioButtonGroup.SelectedValueProperty, bindingBase9);
			translate10.Text = "settings_LiveDataListPageUpdateOnlyVisible_Title";
			IMarkupExtension markupExtension19 = translate10;
			XamlServiceProvider xamlServiceProvider19 = new XamlServiceProvider();
			Type typeFromHandle37 = typeof(IProvideValueTarget);
			object[] array19 = new object[0 + 5];
			array19[0] = label4;
			array19[1] = stackLayout2;
			array19[2] = stackLayout4;
			array19[3] = scrollView;
			array19[4] = this;
			object obj30;
			xamlServiceProvider19.Add(typeFromHandle37, obj30 = new SimpleValueTargetProvider(array19, Label.TextProperty, nameScope));
			xamlServiceProvider19.Add(typeof(IReferenceProvider), obj30);
			Type typeFromHandle38 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver19 = new XmlNamespaceResolver();
			xmlNamespaceResolver19.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver19.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver19.Add("dash", "clr-namespace:CarScannerXamarinForms.Dashboard");
			xmlNamespaceResolver19.Add("input", "clr-namespace:Plugin.InputKit.Shared.Controls;assembly=Plugin.InputKit");
			xmlNamespaceResolver19.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver19.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver19.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver19.Add("syncfusion", "clr-namespace:Syncfusion.XForms.Buttons;assembly=Syncfusion.Buttons.XForms");
			xmlNamespaceResolver19.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider19.Add(typeFromHandle38, new XamlTypeResolver(xmlNamespaceResolver19, typeof(SettingsInterfacePage).GetTypeInfo().Assembly));
			xamlServiceProvider19.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(106, 28)));
			object obj31 = markupExtension19.ProvideValue(xamlServiceProvider19);
			label4.Text = obj31;
			stackLayout2.Children.Add(label4);
			translate11.Text = "settings_LiveDataListPageUpdateOnlyVisible";
			IMarkupExtension markupExtension20 = translate11;
			XamlServiceProvider xamlServiceProvider20 = new XamlServiceProvider();
			Type typeFromHandle39 = typeof(IProvideValueTarget);
			object[] array20 = new object[0 + 5];
			array20[0] = radioButtonWithColor;
			array20[1] = stackLayout2;
			array20[2] = stackLayout4;
			array20[3] = scrollView;
			array20[4] = this;
			object obj32;
			xamlServiceProvider20.Add(typeFromHandle39, obj32 = new SimpleValueTargetProvider(array20, RadioButton.ContentProperty, nameScope));
			xamlServiceProvider20.Add(typeof(IReferenceProvider), obj32);
			Type typeFromHandle40 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver20 = new XmlNamespaceResolver();
			xmlNamespaceResolver20.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver20.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver20.Add("dash", "clr-namespace:CarScannerXamarinForms.Dashboard");
			xmlNamespaceResolver20.Add("input", "clr-namespace:Plugin.InputKit.Shared.Controls;assembly=Plugin.InputKit");
			xmlNamespaceResolver20.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver20.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver20.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver20.Add("syncfusion", "clr-namespace:Syncfusion.XForms.Buttons;assembly=Syncfusion.Buttons.XForms");
			xmlNamespaceResolver20.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider20.Add(typeFromHandle40, new XamlTypeResolver(xmlNamespaceResolver20, typeof(SettingsInterfacePage).GetTypeInfo().Assembly));
			xamlServiceProvider20.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(110, 25)));
			object obj33 = markupExtension20.ProvideValue(xamlServiceProvider20);
			radioButtonWithColor.SetValue(RadioButton.ContentProperty, obj33);
			radioButtonWithColor.SetValue(RadioButton.GroupNameProperty, "UpdateOnlyVsiible");
			staticResourceExtension3.Key = "TrueValue";
			IMarkupExtension markupExtension21 = staticResourceExtension3;
			XamlServiceProvider xamlServiceProvider21 = new XamlServiceProvider();
			Type typeFromHandle41 = typeof(IProvideValueTarget);
			object[] array21 = new object[0 + 5];
			array21[0] = radioButtonWithColor;
			array21[1] = stackLayout2;
			array21[2] = stackLayout4;
			array21[3] = scrollView;
			array21[4] = this;
			object obj34;
			xamlServiceProvider21.Add(typeFromHandle41, obj34 = new SimpleValueTargetProvider(array21, RadioButton.ValueProperty, nameScope));
			xamlServiceProvider21.Add(typeof(IReferenceProvider), obj34);
			Type typeFromHandle42 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver21 = new XmlNamespaceResolver();
			xmlNamespaceResolver21.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver21.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver21.Add("dash", "clr-namespace:CarScannerXamarinForms.Dashboard");
			xmlNamespaceResolver21.Add("input", "clr-namespace:Plugin.InputKit.Shared.Controls;assembly=Plugin.InputKit");
			xmlNamespaceResolver21.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver21.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver21.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver21.Add("syncfusion", "clr-namespace:Syncfusion.XForms.Buttons;assembly=Syncfusion.Buttons.XForms");
			xmlNamespaceResolver21.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider21.Add(typeFromHandle42, new XamlTypeResolver(xmlNamespaceResolver21, typeof(SettingsInterfacePage).GetTypeInfo().Assembly));
			xamlServiceProvider21.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(112, 25)));
			object obj35 = markupExtension21.ProvideValue(xamlServiceProvider21);
			radioButtonWithColor.SetValue(RadioButton.ValueProperty, obj35);
			stackLayout2.Children.Add(radioButtonWithColor);
			translate12.Text = "settings_LiveDataListPageUpdateAllSensors";
			IMarkupExtension markupExtension22 = translate12;
			XamlServiceProvider xamlServiceProvider22 = new XamlServiceProvider();
			Type typeFromHandle43 = typeof(IProvideValueTarget);
			object[] array22 = new object[0 + 5];
			array22[0] = radioButtonWithColor2;
			array22[1] = stackLayout2;
			array22[2] = stackLayout4;
			array22[3] = scrollView;
			array22[4] = this;
			object obj36;
			xamlServiceProvider22.Add(typeFromHandle43, obj36 = new SimpleValueTargetProvider(array22, RadioButton.ContentProperty, nameScope));
			xamlServiceProvider22.Add(typeof(IReferenceProvider), obj36);
			Type typeFromHandle44 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver22 = new XmlNamespaceResolver();
			xmlNamespaceResolver22.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver22.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver22.Add("dash", "clr-namespace:CarScannerXamarinForms.Dashboard");
			xmlNamespaceResolver22.Add("input", "clr-namespace:Plugin.InputKit.Shared.Controls;assembly=Plugin.InputKit");
			xmlNamespaceResolver22.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver22.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver22.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver22.Add("syncfusion", "clr-namespace:Syncfusion.XForms.Buttons;assembly=Syncfusion.Buttons.XForms");
			xmlNamespaceResolver22.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider22.Add(typeFromHandle44, new XamlTypeResolver(xmlNamespaceResolver22, typeof(SettingsInterfacePage).GetTypeInfo().Assembly));
			xamlServiceProvider22.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(114, 25)));
			object obj37 = markupExtension22.ProvideValue(xamlServiceProvider22);
			radioButtonWithColor2.SetValue(RadioButton.ContentProperty, obj37);
			radioButtonWithColor2.SetValue(RadioButton.GroupNameProperty, "UpdateOnlyVsiible");
			staticResourceExtension4.Key = "FalseValue";
			IMarkupExtension markupExtension23 = staticResourceExtension4;
			XamlServiceProvider xamlServiceProvider23 = new XamlServiceProvider();
			Type typeFromHandle45 = typeof(IProvideValueTarget);
			object[] array23 = new object[0 + 5];
			array23[0] = radioButtonWithColor2;
			array23[1] = stackLayout2;
			array23[2] = stackLayout4;
			array23[3] = scrollView;
			array23[4] = this;
			object obj38;
			xamlServiceProvider23.Add(typeFromHandle45, obj38 = new SimpleValueTargetProvider(array23, RadioButton.ValueProperty, nameScope));
			xamlServiceProvider23.Add(typeof(IReferenceProvider), obj38);
			Type typeFromHandle46 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver23 = new XmlNamespaceResolver();
			xmlNamespaceResolver23.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver23.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver23.Add("dash", "clr-namespace:CarScannerXamarinForms.Dashboard");
			xmlNamespaceResolver23.Add("input", "clr-namespace:Plugin.InputKit.Shared.Controls;assembly=Plugin.InputKit");
			xmlNamespaceResolver23.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver23.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver23.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver23.Add("syncfusion", "clr-namespace:Syncfusion.XForms.Buttons;assembly=Syncfusion.Buttons.XForms");
			xmlNamespaceResolver23.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider23.Add(typeFromHandle46, new XamlTypeResolver(xmlNamespaceResolver23, typeof(SettingsInterfacePage).GetTypeInfo().Assembly));
			xamlServiceProvider23.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(116, 25)));
			object obj39 = markupExtension23.ProvideValue(xamlServiceProvider23);
			radioButtonWithColor2.SetValue(RadioButton.ValueProperty, obj39);
			stackLayout2.Children.Add(radioButtonWithColor2);
			stackLayout4.Children.Add(stackLayout2);
			bindingExtension10.Mode = 1;
			bindingExtension10.Path = "AndroidRecolorNavBar";
			bindingExtension10.TypedBinding = new TypedBinding<SharedSettings, bool>(delegate(SharedSettings A_0)
			{
				if (A_0 != null)
				{
					return new ValueTuple<bool, bool>(A_0.AndroidRecolorNavBar, true);
				}
				return default(ValueTuple<bool, bool>);
			}, delegate(SharedSettings A_0, bool A_1)
			{
				if (A_0 != null)
				{
					A_0.AndroidRecolorNavBar = A_1;
					return;
				}
			}, new Tuple<Func<SharedSettings, object>, string>[]
			{
				new Tuple<Func<SharedSettings, object>, string>((SharedSettings A_0) => A_0, "AndroidRecolorNavBar")
			});
			BindingBase bindingBase10 = bindingExtension10.ProvideValue(null);
			labelSwitch2.SetBinding(LabelSwitch.IsToggledProperty, bindingBase10);
			translate13.Text = "droid_ChangeNavigationBarColor";
			IMarkupExtension markupExtension24 = translate13;
			XamlServiceProvider xamlServiceProvider24 = new XamlServiceProvider();
			Type typeFromHandle47 = typeof(IProvideValueTarget);
			object[] array24 = new object[0 + 4];
			array24[0] = labelSwitch2;
			array24[1] = stackLayout4;
			array24[2] = scrollView;
			array24[3] = this;
			object obj40;
			xamlServiceProvider24.Add(typeFromHandle47, obj40 = new SimpleValueTargetProvider(array24, LabelSwitch.TextProperty, nameScope));
			xamlServiceProvider24.Add(typeof(IReferenceProvider), obj40);
			Type typeFromHandle48 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver24 = new XmlNamespaceResolver();
			xmlNamespaceResolver24.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver24.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver24.Add("dash", "clr-namespace:CarScannerXamarinForms.Dashboard");
			xmlNamespaceResolver24.Add("input", "clr-namespace:Plugin.InputKit.Shared.Controls;assembly=Plugin.InputKit");
			xmlNamespaceResolver24.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver24.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver24.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver24.Add("syncfusion", "clr-namespace:Syncfusion.XForms.Buttons;assembly=Syncfusion.Buttons.XForms");
			xmlNamespaceResolver24.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider24.Add(typeFromHandle48, new XamlTypeResolver(xmlNamespaceResolver24, typeof(SettingsInterfacePage).GetTypeInfo().Assembly));
			xamlServiceProvider24.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(121, 92)));
			object obj41 = markupExtension24.ProvideValue(xamlServiceProvider24);
			labelSwitch2.Text = obj41;
			onPlatform2.Android = true;
			onPlatform2.iOS = false;
			labelSwitch2.SetValue(VisualElement.IsVisibleProperty, onPlatform2);
			stackLayout4.Children.Add(labelSwitch2);
			button4.Clicked += this.btnMainScreenConfiguration_Clicked;
			translate14.Text = "ios_MainScreen";
			IMarkupExtension markupExtension25 = translate14;
			XamlServiceProvider xamlServiceProvider25 = new XamlServiceProvider();
			Type typeFromHandle49 = typeof(IProvideValueTarget);
			object[] array25 = new object[0 + 4];
			array25[0] = button4;
			array25[1] = stackLayout4;
			array25[2] = scrollView;
			array25[3] = this;
			object obj42;
			xamlServiceProvider25.Add(typeFromHandle49, obj42 = new SimpleValueTargetProvider(array25, Button.TextProperty, nameScope));
			xamlServiceProvider25.Add(typeof(IReferenceProvider), obj42);
			Type typeFromHandle50 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver25 = new XmlNamespaceResolver();
			xmlNamespaceResolver25.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver25.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver25.Add("dash", "clr-namespace:CarScannerXamarinForms.Dashboard");
			xmlNamespaceResolver25.Add("input", "clr-namespace:Plugin.InputKit.Shared.Controls;assembly=Plugin.InputKit");
			xmlNamespaceResolver25.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver25.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver25.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver25.Add("syncfusion", "clr-namespace:Syncfusion.XForms.Buttons;assembly=Syncfusion.Buttons.XForms");
			xmlNamespaceResolver25.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider25.Add(typeFromHandle50, new XamlTypeResolver(xmlNamespaceResolver25, typeof(SettingsInterfacePage).GetTypeInfo().Assembly));
			xamlServiceProvider25.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(133, 21)));
			object obj43 = markupExtension25.ProvideValue(xamlServiceProvider25);
			button4.Text = obj43;
			stackLayout4.Children.Add(button4);
			stackLayout3.SetValue(StackLayout.OrientationProperty, 0);
			translate15.Text = "Settings_Control_tbChartsMode";
			IMarkupExtension markupExtension26 = translate15;
			XamlServiceProvider xamlServiceProvider26 = new XamlServiceProvider();
			Type typeFromHandle51 = typeof(IProvideValueTarget);
			object[] array26 = new object[0 + 5];
			array26[0] = label5;
			array26[1] = stackLayout3;
			array26[2] = stackLayout4;
			array26[3] = scrollView;
			array26[4] = this;
			object obj44;
			xamlServiceProvider26.Add(typeFromHandle51, obj44 = new SimpleValueTargetProvider(array26, Label.TextProperty, nameScope));
			xamlServiceProvider26.Add(typeof(IReferenceProvider), obj44);
			Type typeFromHandle52 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver26 = new XmlNamespaceResolver();
			xmlNamespaceResolver26.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver26.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver26.Add("dash", "clr-namespace:CarScannerXamarinForms.Dashboard");
			xmlNamespaceResolver26.Add("input", "clr-namespace:Plugin.InputKit.Shared.Controls;assembly=Plugin.InputKit");
			xmlNamespaceResolver26.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver26.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver26.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver26.Add("syncfusion", "clr-namespace:Syncfusion.XForms.Buttons;assembly=Syncfusion.Buttons.XForms");
			xmlNamespaceResolver26.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider26.Add(typeFromHandle52, new XamlTypeResolver(xmlNamespaceResolver26, typeof(SettingsInterfacePage).GetTypeInfo().Assembly));
			xamlServiceProvider26.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(138, 28)));
			object obj45 = markupExtension26.ProvideValue(xamlServiceProvider26);
			label5.Text = obj45;
			stackLayout3.Children.Add(label5);
			picker2.SetValue(View.MarginProperty, new Thickness(0.0, 0.0, 0.0, 10.0));
			bindingExtension11.Source = chartViewSelector;
			BindingBase bindingBase11 = bindingExtension11.ProvideValue(null);
			picker2.SetBinding(Picker.ItemsSourceProperty, bindingBase11);
			bindingExtension12.Mode = 1;
			bindingExtension12.Path = "ChartsView";
			bindingExtension12.TypedBinding = new TypedBinding<SharedSettings, int>(delegate(SharedSettings A_0)
			{
				if (A_0 != null)
				{
					return new ValueTuple<int, bool>(A_0.ChartsView, true);
				}
				return default(ValueTuple<int, bool>);
			}, delegate(SharedSettings A_0, int A_1)
			{
				if (A_0 != null)
				{
					A_0.ChartsView = A_1;
					return;
				}
			}, new Tuple<Func<SharedSettings, object>, string>[]
			{
				new Tuple<Func<SharedSettings, object>, string>((SharedSettings A_0) => A_0, "ChartsView")
			});
			BindingBase bindingBase12 = bindingExtension12.ProvideValue(null);
			picker2.SetBinding(Picker.SelectedIndexProperty, bindingBase12);
			stackLayout3.Children.Add(picker2);
			stackLayout4.Children.Add(stackLayout3);
			grid.SetValue(View.MarginProperty, new Thickness(0.0, 0.0, 0.0, 10.0));
			grid.SetValue(Grid.RowSpacingProperty, 0.0);
			columnDefinition.SetValue(ColumnDefinition.WidthProperty, new GridLengthTypeConverter().ConvertFromInvariantString("*"));
			grid.GetValue(Grid.ColumnDefinitionsProperty).Add(columnDefinition);
			columnDefinition2.SetValue(ColumnDefinition.WidthProperty, new GridLengthTypeConverter().ConvertFromInvariantString("auto"));
			grid.GetValue(Grid.ColumnDefinitionsProperty).Add(columnDefinition2);
			label6.SetValue(Grid.RowProperty, 0);
			label6.SetValue(Grid.ColumnProperty, 0);
			label6.SetValue(Label.LineBreakModeProperty, 1);
			translate16.Text = "Settings_Control_tbChartVisibleTime.Text";
			IMarkupExtension markupExtension27 = translate16;
			XamlServiceProvider xamlServiceProvider27 = new XamlServiceProvider();
			Type typeFromHandle53 = typeof(IProvideValueTarget);
			object[] array27 = new object[0 + 5];
			array27[0] = label6;
			array27[1] = grid;
			array27[2] = stackLayout4;
			array27[3] = scrollView;
			array27[4] = this;
			object obj46;
			xamlServiceProvider27.Add(typeFromHandle53, obj46 = new SimpleValueTargetProvider(array27, Label.TextProperty, nameScope));
			xamlServiceProvider27.Add(typeof(IReferenceProvider), obj46);
			Type typeFromHandle54 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver27 = new XmlNamespaceResolver();
			xmlNamespaceResolver27.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver27.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver27.Add("dash", "clr-namespace:CarScannerXamarinForms.Dashboard");
			xmlNamespaceResolver27.Add("input", "clr-namespace:Plugin.InputKit.Shared.Controls;assembly=Plugin.InputKit");
			xmlNamespaceResolver27.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver27.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver27.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver27.Add("syncfusion", "clr-namespace:Syncfusion.XForms.Buttons;assembly=Syncfusion.Buttons.XForms");
			xmlNamespaceResolver27.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider27.Add(typeFromHandle54, new XamlTypeResolver(xmlNamespaceResolver27, typeof(SettingsInterfacePage).GetTypeInfo().Assembly));
			xamlServiceProvider27.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(154, 25)));
			object obj47 = markupExtension27.ProvideValue(xamlServiceProvider27);
			label6.Text = obj47;
			grid.Children.Add(label6);
			label7.SetValue(Grid.RowProperty, 0);
			label7.SetValue(Grid.ColumnProperty, 1);
			label7.SetValue(Label.FontAttributesProperty, new FontAttributesConverter().ConvertFromInvariantString("Bold"));
			label7.SetValue(Label.LineBreakModeProperty, 0);
			bindingExtension13.Mode = 2;
			bindingExtension13.Path = "LiveDataShowTime";
			bindingExtension13.TypedBinding = new TypedBinding<SharedSettings, int>(delegate(SharedSettings A_0)
			{
				if (A_0 != null)
				{
					return new ValueTuple<int, bool>(A_0.LiveDataShowTime, true);
				}
				return default(ValueTuple<int, bool>);
			}, null, new Tuple<Func<SharedSettings, object>, string>[]
			{
				new Tuple<Func<SharedSettings, object>, string>((SharedSettings A_0) => A_0, "LiveDataShowTime")
			});
			BindingBase bindingBase13 = bindingExtension13.ProvideValue(null);
			label7.SetBinding(Label.TextProperty, bindingBase13);
			label7.SetValue(View.VerticalOptionsProperty, LayoutOptions.Center);
			grid.Children.Add(label7);
			stackLayout4.Children.Add(grid);
			stepper.SetValue(Grid.RowProperty, 1);
			stepper.SetValue(Grid.ColumnProperty, 1);
			stepper.SetValue(View.HorizontalOptionsProperty, LayoutOptions.EndAndExpand);
			stepper.SetValue(Stepper.IncrementProperty, 5.0);
			stepper.SetValue(Stepper.MaximumProperty, 10000.0);
			stepper.SetValue(Stepper.MinimumProperty, 5.0);
			bindingExtension14.Mode = 1;
			bindingExtension14.Path = "LiveDataShowTime";
			bindingExtension14.TypedBinding = new TypedBinding<SharedSettings, int>(delegate(SharedSettings A_0)
			{
				if (A_0 != null)
				{
					return new ValueTuple<int, bool>(A_0.LiveDataShowTime, true);
				}
				return default(ValueTuple<int, bool>);
			}, delegate(SharedSettings A_0, int A_1)
			{
				if (A_0 != null)
				{
					A_0.LiveDataShowTime = A_1;
					return;
				}
			}, new Tuple<Func<SharedSettings, object>, string>[]
			{
				new Tuple<Func<SharedSettings, object>, string>((SharedSettings A_0) => A_0, "LiveDataShowTime")
			});
			BindingBase bindingBase14 = bindingExtension14.ProvideValue(null);
			stepper.SetBinding(Stepper.ValueProperty, bindingBase14);
			stackLayout4.Children.Add(stepper);
			translate17.Text = "Settings_ChartStyle";
			IMarkupExtension markupExtension28 = translate17;
			XamlServiceProvider xamlServiceProvider28 = new XamlServiceProvider();
			Type typeFromHandle55 = typeof(IProvideValueTarget);
			object[] array28 = new object[0 + 4];
			array28[0] = label8;
			array28[1] = stackLayout4;
			array28[2] = scrollView;
			array28[3] = this;
			object obj48;
			xamlServiceProvider28.Add(typeFromHandle55, obj48 = new SimpleValueTargetProvider(array28, Label.TextProperty, nameScope));
			xamlServiceProvider28.Add(typeof(IReferenceProvider), obj48);
			Type typeFromHandle56 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver28 = new XmlNamespaceResolver();
			xmlNamespaceResolver28.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver28.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver28.Add("dash", "clr-namespace:CarScannerXamarinForms.Dashboard");
			xmlNamespaceResolver28.Add("input", "clr-namespace:Plugin.InputKit.Shared.Controls;assembly=Plugin.InputKit");
			xmlNamespaceResolver28.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver28.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver28.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver28.Add("syncfusion", "clr-namespace:Syncfusion.XForms.Buttons;assembly=Syncfusion.Buttons.XForms");
			xmlNamespaceResolver28.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider28.Add(typeFromHandle56, new XamlTypeResolver(xmlNamespaceResolver28, typeof(SettingsInterfacePage).GetTypeInfo().Assembly));
			xamlServiceProvider28.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(173, 24)));
			object obj49 = markupExtension28.ProvideValue(xamlServiceProvider28);
			label8.Text = obj49;
			stackLayout4.Children.Add(label8);
			staticResourceExtension5.Key = "EnumToIntConverter";
			IMarkupExtension markupExtension29 = staticResourceExtension5;
			XamlServiceProvider xamlServiceProvider29 = new XamlServiceProvider();
			Type typeFromHandle57 = typeof(IProvideValueTarget);
			object[] array29 = new object[0 + 5];
			array29[0] = bindingExtension15;
			array29[1] = picker3;
			array29[2] = stackLayout4;
			array29[3] = scrollView;
			array29[4] = this;
			object obj50;
			xamlServiceProvider29.Add(typeFromHandle57, obj50 = new SimpleValueTargetProvider(array29, typeof(BindingExtension).GetRuntimeProperty("Converter"), nameScope));
			xamlServiceProvider29.Add(typeof(IReferenceProvider), obj50);
			Type typeFromHandle58 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver29 = new XmlNamespaceResolver();
			xmlNamespaceResolver29.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver29.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver29.Add("dash", "clr-namespace:CarScannerXamarinForms.Dashboard");
			xmlNamespaceResolver29.Add("input", "clr-namespace:Plugin.InputKit.Shared.Controls;assembly=Plugin.InputKit");
			xmlNamespaceResolver29.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver29.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver29.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver29.Add("syncfusion", "clr-namespace:Syncfusion.XForms.Buttons;assembly=Syncfusion.Buttons.XForms");
			xmlNamespaceResolver29.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider29.Add(typeFromHandle58, new XamlTypeResolver(xmlNamespaceResolver29, typeof(SettingsInterfacePage).GetTypeInfo().Assembly));
			xamlServiceProvider29.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(174, 51)));
			object obj51 = markupExtension29.ProvideValue(xamlServiceProvider29);
			bindingExtension15.Converter = obj51;
			bindingExtension15.Mode = 1;
			bindingExtension15.Path = "ChartDisplayStyle";
			bindingExtension15.TypedBinding = new TypedBinding<SharedSettings, ChartItemTypes>(delegate(SharedSettings A_0)
			{
				if (A_0 != null)
				{
					return new ValueTuple<ChartItemTypes, bool>(A_0.ChartDisplayStyle, true);
				}
				return default(ValueTuple<ChartItemTypes, bool>);
			}, delegate(SharedSettings A_0, ChartItemTypes A_1)
			{
				if (A_0 != null)
				{
					A_0.ChartDisplayStyle = A_1;
					return;
				}
			}, new Tuple<Func<SharedSettings, object>, string>[]
			{
				new Tuple<Func<SharedSettings, object>, string>((SharedSettings A_0) => A_0, "ChartDisplayStyle")
			});
			BindingBase bindingBase15 = bindingExtension15.ProvideValue(null);
			picker3.SetBinding(Picker.SelectedIndexProperty, bindingBase15);
			stackLayout4.Children.Add(picker3);
			grid2.SetValue(View.MarginProperty, new Thickness(0.0, 0.0, 0.0, 10.0));
			grid2.SetValue(Grid.RowSpacingProperty, 0.0);
			columnDefinition3.SetValue(ColumnDefinition.WidthProperty, new GridLengthTypeConverter().ConvertFromInvariantString("*"));
			grid2.GetValue(Grid.ColumnDefinitionsProperty).Add(columnDefinition3);
			columnDefinition4.SetValue(ColumnDefinition.WidthProperty, new GridLengthTypeConverter().ConvertFromInvariantString("auto"));
			grid2.GetValue(Grid.ColumnDefinitionsProperty).Add(columnDefinition4);
			rowDefinition.SetValue(RowDefinition.HeightProperty, new GridLengthTypeConverter().ConvertFromInvariantString("auto"));
			grid2.GetValue(Grid.RowDefinitionsProperty).Add(rowDefinition);
			rowDefinition2.SetValue(RowDefinition.HeightProperty, new GridLengthTypeConverter().ConvertFromInvariantString("auto"));
			grid2.GetValue(Grid.RowDefinitionsProperty).Add(rowDefinition2);
			label9.SetValue(Grid.RowProperty, 1);
			label9.SetValue(Grid.ColumnProperty, 0);
			label9.SetValue(Label.FontAttributesProperty, new FontAttributesConverter().ConvertFromInvariantString("Italic"));
			bindingExtension16.Mode = 2;
			bindingExtension16.Path = "SetChartMinMaxOnlyVisibleArea";
			bindingExtension16.TypedBinding = new TypedBinding<SharedSettings, bool>(delegate(SharedSettings A_0)
			{
				if (A_0 != null)
				{
					return new ValueTuple<bool, bool>(A_0.SetChartMinMaxOnlyVisibleArea, true);
				}
				return default(ValueTuple<bool, bool>);
			}, null, new Tuple<Func<SharedSettings, object>, string>[]
			{
				new Tuple<Func<SharedSettings, object>, string>((SharedSettings A_0) => A_0, "SetChartMinMaxOnlyVisibleArea")
			});
			BindingBase bindingBase16 = bindingExtension16.ProvideValue(null);
			label9.SetBinding(VisualElement.IsVisibleProperty, bindingBase16);
			translate18.Text = "Settings_Control_ToggleSetChartMinMaxOnlyVisibleArea.OnContent";
			IMarkupExtension markupExtension30 = translate18;
			XamlServiceProvider xamlServiceProvider30 = new XamlServiceProvider();
			Type typeFromHandle59 = typeof(IProvideValueTarget);
			object[] array30 = new object[0 + 5];
			array30[0] = label9;
			array30[1] = grid2;
			array30[2] = stackLayout4;
			array30[3] = scrollView;
			array30[4] = this;
			object obj52;
			xamlServiceProvider30.Add(typeFromHandle59, obj52 = new SimpleValueTargetProvider(array30, Label.TextProperty, nameScope));
			xamlServiceProvider30.Add(typeof(IReferenceProvider), obj52);
			Type typeFromHandle60 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver30 = new XmlNamespaceResolver();
			xmlNamespaceResolver30.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver30.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver30.Add("dash", "clr-namespace:CarScannerXamarinForms.Dashboard");
			xmlNamespaceResolver30.Add("input", "clr-namespace:Plugin.InputKit.Shared.Controls;assembly=Plugin.InputKit");
			xmlNamespaceResolver30.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver30.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver30.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver30.Add("syncfusion", "clr-namespace:Syncfusion.XForms.Buttons;assembly=Syncfusion.Buttons.XForms");
			xmlNamespaceResolver30.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider30.Add(typeFromHandle60, new XamlTypeResolver(xmlNamespaceResolver30, typeof(SettingsInterfacePage).GetTypeInfo().Assembly));
			xamlServiceProvider30.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(206, 25)));
			object obj53 = markupExtension30.ProvideValue(xamlServiceProvider30);
			label9.Text = obj53;
			label9.SetValue(View.VerticalOptionsProperty, LayoutOptions.Center);
			grid2.Children.Add(label9);
			label10.SetValue(Grid.RowProperty, 1);
			label10.SetValue(Grid.ColumnProperty, 0);
			label10.SetValue(Label.FontAttributesProperty, new FontAttributesConverter().ConvertFromInvariantString("Italic"));
			bindingExtension17.Mode = 2;
			staticResourceExtension6.Key = "BoolToNegativeConverter";
			IMarkupExtension markupExtension31 = staticResourceExtension6;
			XamlServiceProvider xamlServiceProvider31 = new XamlServiceProvider();
			Type typeFromHandle61 = typeof(IProvideValueTarget);
			object[] array31 = new object[0 + 6];
			array31[0] = bindingExtension17;
			array31[1] = label10;
			array31[2] = grid2;
			array31[3] = stackLayout4;
			array31[4] = scrollView;
			array31[5] = this;
			object obj54;
			xamlServiceProvider31.Add(typeFromHandle61, obj54 = new SimpleValueTargetProvider(array31, typeof(BindingExtension).GetRuntimeProperty("Converter"), nameScope));
			xamlServiceProvider31.Add(typeof(IReferenceProvider), obj54);
			Type typeFromHandle62 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver31 = new XmlNamespaceResolver();
			xmlNamespaceResolver31.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver31.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver31.Add("dash", "clr-namespace:CarScannerXamarinForms.Dashboard");
			xmlNamespaceResolver31.Add("input", "clr-namespace:Plugin.InputKit.Shared.Controls;assembly=Plugin.InputKit");
			xmlNamespaceResolver31.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver31.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver31.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver31.Add("syncfusion", "clr-namespace:Syncfusion.XForms.Buttons;assembly=Syncfusion.Buttons.XForms");
			xmlNamespaceResolver31.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider31.Add(typeFromHandle62, new XamlTypeResolver(xmlNamespaceResolver31, typeof(SettingsInterfacePage).GetTypeInfo().Assembly));
			xamlServiceProvider31.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(212, 25)));
			object obj55 = markupExtension31.ProvideValue(xamlServiceProvider31);
			bindingExtension17.Converter = obj55;
			bindingExtension17.Path = "SetChartMinMaxOnlyVisibleArea";
			bindingExtension17.TypedBinding = new TypedBinding<SharedSettings, bool>(delegate(SharedSettings A_0)
			{
				if (A_0 != null)
				{
					return new ValueTuple<bool, bool>(A_0.SetChartMinMaxOnlyVisibleArea, true);
				}
				return default(ValueTuple<bool, bool>);
			}, null, new Tuple<Func<SharedSettings, object>, string>[]
			{
				new Tuple<Func<SharedSettings, object>, string>((SharedSettings A_0) => A_0, "SetChartMinMaxOnlyVisibleArea")
			});
			BindingBase bindingBase17 = bindingExtension17.ProvideValue(null);
			label10.SetBinding(VisualElement.IsVisibleProperty, bindingBase17);
			translate19.Text = "Settings_Control_ToggleSetChartMinMaxOnlyVisibleArea.OffContent";
			IMarkupExtension markupExtension32 = translate19;
			XamlServiceProvider xamlServiceProvider32 = new XamlServiceProvider();
			Type typeFromHandle63 = typeof(IProvideValueTarget);
			object[] array32 = new object[0 + 5];
			array32[0] = label10;
			array32[1] = grid2;
			array32[2] = stackLayout4;
			array32[3] = scrollView;
			array32[4] = this;
			object obj56;
			xamlServiceProvider32.Add(typeFromHandle63, obj56 = new SimpleValueTargetProvider(array32, Label.TextProperty, nameScope));
			xamlServiceProvider32.Add(typeof(IReferenceProvider), obj56);
			Type typeFromHandle64 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver32 = new XmlNamespaceResolver();
			xmlNamespaceResolver32.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver32.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver32.Add("dash", "clr-namespace:CarScannerXamarinForms.Dashboard");
			xmlNamespaceResolver32.Add("input", "clr-namespace:Plugin.InputKit.Shared.Controls;assembly=Plugin.InputKit");
			xmlNamespaceResolver32.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver32.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver32.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver32.Add("syncfusion", "clr-namespace:Syncfusion.XForms.Buttons;assembly=Syncfusion.Buttons.XForms");
			xmlNamespaceResolver32.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider32.Add(typeFromHandle64, new XamlTypeResolver(xmlNamespaceResolver32, typeof(SettingsInterfacePage).GetTypeInfo().Assembly));
			xamlServiceProvider32.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(213, 25)));
			object obj57 = markupExtension32.ProvideValue(xamlServiceProvider32);
			label10.Text = obj57;
			label10.SetValue(View.VerticalOptionsProperty, LayoutOptions.Center);
			grid2.Children.Add(label10);
			label11.SetValue(Grid.RowProperty, 0);
			label11.SetValue(Grid.ColumnProperty, 0);
			translate20.Text = "Settings_Control_ToggleSetChartMinMaxOnlyVisibleArea.Header";
			IMarkupExtension markupExtension33 = translate20;
			XamlServiceProvider xamlServiceProvider33 = new XamlServiceProvider();
			Type typeFromHandle65 = typeof(IProvideValueTarget);
			object[] array33 = new object[0 + 5];
			array33[0] = label11;
			array33[1] = grid2;
			array33[2] = stackLayout4;
			array33[3] = scrollView;
			array33[4] = this;
			object obj58;
			xamlServiceProvider33.Add(typeFromHandle65, obj58 = new SimpleValueTargetProvider(array33, Label.TextProperty, nameScope));
			xamlServiceProvider33.Add(typeof(IReferenceProvider), obj58);
			Type typeFromHandle66 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver33 = new XmlNamespaceResolver();
			xmlNamespaceResolver33.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver33.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver33.Add("dash", "clr-namespace:CarScannerXamarinForms.Dashboard");
			xmlNamespaceResolver33.Add("input", "clr-namespace:Plugin.InputKit.Shared.Controls;assembly=Plugin.InputKit");
			xmlNamespaceResolver33.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver33.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver33.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver33.Add("syncfusion", "clr-namespace:Syncfusion.XForms.Buttons;assembly=Syncfusion.Buttons.XForms");
			xmlNamespaceResolver33.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider33.Add(typeFromHandle66, new XamlTypeResolver(xmlNamespaceResolver33, typeof(SettingsInterfacePage).GetTypeInfo().Assembly));
			xamlServiceProvider33.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(218, 25)));
			object obj59 = markupExtension33.ProvideValue(xamlServiceProvider33);
			label11.Text = obj59;
			grid2.Children.Add(label11);
			checkSwitch.SetValue(Grid.RowProperty, 1);
			checkSwitch.SetValue(Grid.ColumnProperty, 1);
			bindingExtension18.Mode = 1;
			bindingExtension18.Path = "SetChartMinMaxOnlyVisibleArea";
			bindingExtension18.TypedBinding = new TypedBinding<SharedSettings, bool>(delegate(SharedSettings A_0)
			{
				if (A_0 != null)
				{
					return new ValueTuple<bool, bool>(A_0.SetChartMinMaxOnlyVisibleArea, true);
				}
				return default(ValueTuple<bool, bool>);
			}, delegate(SharedSettings A_0, bool A_1)
			{
				if (A_0 != null)
				{
					A_0.SetChartMinMaxOnlyVisibleArea = A_1;
					return;
				}
			}, new Tuple<Func<SharedSettings, object>, string>[]
			{
				new Tuple<Func<SharedSettings, object>, string>((SharedSettings A_0) => A_0, "SetChartMinMaxOnlyVisibleArea")
			});
			BindingBase bindingBase18 = bindingExtension18.ProvideValue(null);
			checkSwitch.SetBinding(CheckSwitch.IsToggledProperty, bindingBase18);
			grid2.Children.Add(checkSwitch);
			stackLayout4.Children.Add(grid2);
			bindingExtension19.Mode = 1;
			bindingExtension19.Path = "ChartShowAverageValue";
			bindingExtension19.TypedBinding = new TypedBinding<SharedSettings, bool>(delegate(SharedSettings A_0)
			{
				if (A_0 != null)
				{
					return new ValueTuple<bool, bool>(A_0.ChartShowAverageValue, true);
				}
				return default(ValueTuple<bool, bool>);
			}, delegate(SharedSettings A_0, bool A_1)
			{
				if (A_0 != null)
				{
					A_0.ChartShowAverageValue = A_1;
					return;
				}
			}, new Tuple<Func<SharedSettings, object>, string>[]
			{
				new Tuple<Func<SharedSettings, object>, string>((SharedSettings A_0) => A_0, "ChartShowAverageValue")
			});
			BindingBase bindingBase19 = bindingExtension19.ProvideValue(null);
			labelSwitch3.SetBinding(LabelSwitch.IsToggledProperty, bindingBase19);
			translate21.Text = "Settings_Control_ToggleShowAverageOnChart.Header";
			IMarkupExtension markupExtension34 = translate21;
			XamlServiceProvider xamlServiceProvider34 = new XamlServiceProvider();
			Type typeFromHandle67 = typeof(IProvideValueTarget);
			object[] array34 = new object[0 + 4];
			array34[0] = labelSwitch3;
			array34[1] = stackLayout4;
			array34[2] = scrollView;
			array34[3] = this;
			object obj60;
			xamlServiceProvider34.Add(typeFromHandle67, obj60 = new SimpleValueTargetProvider(array34, LabelSwitch.TextProperty, nameScope));
			xamlServiceProvider34.Add(typeof(IReferenceProvider), obj60);
			Type typeFromHandle68 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver34 = new XmlNamespaceResolver();
			xmlNamespaceResolver34.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver34.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver34.Add("dash", "clr-namespace:CarScannerXamarinForms.Dashboard");
			xmlNamespaceResolver34.Add("input", "clr-namespace:Plugin.InputKit.Shared.Controls;assembly=Plugin.InputKit");
			xmlNamespaceResolver34.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver34.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver34.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver34.Add("syncfusion", "clr-namespace:Syncfusion.XForms.Buttons;assembly=Syncfusion.Buttons.XForms");
			xmlNamespaceResolver34.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider34.Add(typeFromHandle68, new XamlTypeResolver(xmlNamespaceResolver34, typeof(SettingsInterfacePage).GetTypeInfo().Assembly));
			xamlServiceProvider34.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(224, 93)));
			object obj61 = markupExtension34.ProvideValue(xamlServiceProvider34);
			labelSwitch3.Text = obj61;
			stackLayout4.Children.Add(labelSwitch3);
			bindingExtension20.Mode = 1;
			bindingExtension20.Path = "ShowMinMaxValues";
			bindingExtension20.TypedBinding = new TypedBinding<SharedSettings, bool>(delegate(SharedSettings A_0)
			{
				if (A_0 != null)
				{
					return new ValueTuple<bool, bool>(A_0.ShowMinMaxValues, true);
				}
				return default(ValueTuple<bool, bool>);
			}, delegate(SharedSettings A_0, bool A_1)
			{
				if (A_0 != null)
				{
					A_0.ShowMinMaxValues = A_1;
					return;
				}
			}, new Tuple<Func<SharedSettings, object>, string>[]
			{
				new Tuple<Func<SharedSettings, object>, string>((SharedSettings A_0) => A_0, "ShowMinMaxValues")
			});
			BindingBase bindingBase20 = bindingExtension20.ProvideValue(null);
			labelSwitch4.SetBinding(LabelSwitch.IsToggledProperty, bindingBase20);
			translate22.Text = "Dashboard_DisplayMinMax";
			IMarkupExtension markupExtension35 = translate22;
			XamlServiceProvider xamlServiceProvider35 = new XamlServiceProvider();
			Type typeFromHandle69 = typeof(IProvideValueTarget);
			object[] array35 = new object[0 + 4];
			array35[0] = labelSwitch4;
			array35[1] = stackLayout4;
			array35[2] = scrollView;
			array35[3] = this;
			object obj62;
			xamlServiceProvider35.Add(typeFromHandle69, obj62 = new SimpleValueTargetProvider(array35, LabelSwitch.TextProperty, nameScope));
			xamlServiceProvider35.Add(typeof(IReferenceProvider), obj62);
			Type typeFromHandle70 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver35 = new XmlNamespaceResolver();
			xmlNamespaceResolver35.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver35.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver35.Add("dash", "clr-namespace:CarScannerXamarinForms.Dashboard");
			xmlNamespaceResolver35.Add("input", "clr-namespace:Plugin.InputKit.Shared.Controls;assembly=Plugin.InputKit");
			xmlNamespaceResolver35.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver35.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver35.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver35.Add("syncfusion", "clr-namespace:Syncfusion.XForms.Buttons;assembly=Syncfusion.Buttons.XForms");
			xmlNamespaceResolver35.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider35.Add(typeFromHandle70, new XamlTypeResolver(xmlNamespaceResolver35, typeof(SettingsInterfacePage).GetTypeInfo().Assembly));
			xamlServiceProvider35.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(225, 88)));
			object obj63 = markupExtension35.ProvideValue(xamlServiceProvider35);
			labelSwitch4.Text = obj63;
			stackLayout4.Children.Add(labelSwitch4);
			bindingExtension21.Mode = 1;
			bindingExtension21.Path = "MultiChartPauseHidden";
			bindingExtension21.TypedBinding = new TypedBinding<SharedSettings, bool>(delegate(SharedSettings A_0)
			{
				if (A_0 != null)
				{
					return new ValueTuple<bool, bool>(A_0.MultiChartPauseHidden, true);
				}
				return default(ValueTuple<bool, bool>);
			}, delegate(SharedSettings A_0, bool A_1)
			{
				if (A_0 != null)
				{
					A_0.MultiChartPauseHidden = A_1;
					return;
				}
			}, new Tuple<Func<SharedSettings, object>, string>[]
			{
				new Tuple<Func<SharedSettings, object>, string>((SharedSettings A_0) => A_0, "MultiChartPauseHidden")
			});
			BindingBase bindingBase21 = bindingExtension21.ProvideValue(null);
			labelSwitch5.SetBinding(LabelSwitch.IsToggledProperty, bindingBase21);
			translate23.Text = "ios_Settings_MultiChart_PauseRequests";
			IMarkupExtension markupExtension36 = translate23;
			XamlServiceProvider xamlServiceProvider36 = new XamlServiceProvider();
			Type typeFromHandle71 = typeof(IProvideValueTarget);
			object[] array36 = new object[0 + 4];
			array36[0] = labelSwitch5;
			array36[1] = stackLayout4;
			array36[2] = scrollView;
			array36[3] = this;
			object obj64;
			xamlServiceProvider36.Add(typeFromHandle71, obj64 = new SimpleValueTargetProvider(array36, LabelSwitch.TextProperty, nameScope));
			xamlServiceProvider36.Add(typeof(IReferenceProvider), obj64);
			Type typeFromHandle72 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver36 = new XmlNamespaceResolver();
			xmlNamespaceResolver36.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver36.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver36.Add("dash", "clr-namespace:CarScannerXamarinForms.Dashboard");
			xmlNamespaceResolver36.Add("input", "clr-namespace:Plugin.InputKit.Shared.Controls;assembly=Plugin.InputKit");
			xmlNamespaceResolver36.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver36.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver36.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver36.Add("syncfusion", "clr-namespace:Syncfusion.XForms.Buttons;assembly=Syncfusion.Buttons.XForms");
			xmlNamespaceResolver36.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider36.Add(typeFromHandle72, new XamlTypeResolver(xmlNamespaceResolver36, typeof(SettingsInterfacePage).GetTypeInfo().Assembly));
			xamlServiceProvider36.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(226, 93)));
			object obj65 = markupExtension36.ProvideValue(xamlServiceProvider36);
			labelSwitch5.Text = obj65;
			stackLayout4.Children.Add(labelSwitch5);
			bindingExtension22.Mode = 1;
			bindingExtension22.Path = "AndroidChartRenderingSafeMode";
			bindingExtension22.TypedBinding = new TypedBinding<SharedSettings, bool>(delegate(SharedSettings A_0)
			{
				if (A_0 != null)
				{
					return new ValueTuple<bool, bool>(A_0.AndroidChartRenderingSafeMode, true);
				}
				return default(ValueTuple<bool, bool>);
			}, delegate(SharedSettings A_0, bool A_1)
			{
				if (A_0 != null)
				{
					A_0.AndroidChartRenderingSafeMode = A_1;
					return;
				}
			}, new Tuple<Func<SharedSettings, object>, string>[]
			{
				new Tuple<Func<SharedSettings, object>, string>((SharedSettings A_0) => A_0, "AndroidChartRenderingSafeMode")
			});
			BindingBase bindingBase22 = bindingExtension22.ProvideValue(null);
			labelSwitch6.SetBinding(LabelSwitch.IsToggledProperty, bindingBase22);
			translate24.Text = "droid_ChartRenderingSafeMode";
			IMarkupExtension markupExtension37 = translate24;
			XamlServiceProvider xamlServiceProvider37 = new XamlServiceProvider();
			Type typeFromHandle73 = typeof(IProvideValueTarget);
			object[] array37 = new object[0 + 4];
			array37[0] = labelSwitch6;
			array37[1] = stackLayout4;
			array37[2] = scrollView;
			array37[3] = this;
			object obj66;
			xamlServiceProvider37.Add(typeFromHandle73, obj66 = new SimpleValueTargetProvider(array37, LabelSwitch.TextProperty, nameScope));
			xamlServiceProvider37.Add(typeof(IReferenceProvider), obj66);
			Type typeFromHandle74 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver37 = new XmlNamespaceResolver();
			xmlNamespaceResolver37.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver37.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver37.Add("dash", "clr-namespace:CarScannerXamarinForms.Dashboard");
			xmlNamespaceResolver37.Add("input", "clr-namespace:Plugin.InputKit.Shared.Controls;assembly=Plugin.InputKit");
			xmlNamespaceResolver37.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver37.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver37.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver37.Add("syncfusion", "clr-namespace:Syncfusion.XForms.Buttons;assembly=Syncfusion.Buttons.XForms");
			xmlNamespaceResolver37.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider37.Add(typeFromHandle74, new XamlTypeResolver(xmlNamespaceResolver37, typeof(SettingsInterfacePage).GetTypeInfo().Assembly));
			xamlServiceProvider37.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(230, 101)));
			object obj67 = markupExtension37.ProvideValue(xamlServiceProvider37);
			labelSwitch6.Text = obj67;
			on.Platform = new List<string>(1) { "Android" };
			on.Value = "True";
			onPlatform3.Platforms.Add(on);
			on2.Platform = new List<string>(1) { "iOS" };
			on2.Value = "False";
			onPlatform3.Platforms.Add(on2);
			labelSwitch6.SetValue(VisualElement.IsVisibleProperty, onPlatform3);
			stackLayout4.Children.Add(labelSwitch6);
			bindingExtension23.Mode = 1;
			bindingExtension23.Path = "ShowPing";
			bindingExtension23.TypedBinding = new TypedBinding<SharedSettings, bool>(delegate(SharedSettings A_0)
			{
				if (A_0 != null)
				{
					return new ValueTuple<bool, bool>(A_0.ShowPing, true);
				}
				return default(ValueTuple<bool, bool>);
			}, delegate(SharedSettings A_0, bool A_1)
			{
				if (A_0 != null)
				{
					A_0.ShowPing = A_1;
					return;
				}
			}, new Tuple<Func<SharedSettings, object>, string>[]
			{
				new Tuple<Func<SharedSettings, object>, string>((SharedSettings A_0) => A_0, "ShowPing")
			});
			BindingBase bindingBase23 = bindingExtension23.ProvideValue(null);
			labelSwitch7.SetBinding(LabelSwitch.IsToggledProperty, bindingBase23);
			translate25.Text = "Settings_Control_tbShowPing.Text";
			IMarkupExtension markupExtension38 = translate25;
			XamlServiceProvider xamlServiceProvider38 = new XamlServiceProvider();
			Type typeFromHandle75 = typeof(IProvideValueTarget);
			object[] array38 = new object[0 + 4];
			array38[0] = labelSwitch7;
			array38[1] = stackLayout4;
			array38[2] = scrollView;
			array38[3] = this;
			object obj68;
			xamlServiceProvider38.Add(typeFromHandle75, obj68 = new SimpleValueTargetProvider(array38, LabelSwitch.TextProperty, nameScope));
			xamlServiceProvider38.Add(typeof(IReferenceProvider), obj68);
			Type typeFromHandle76 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver38 = new XmlNamespaceResolver();
			xmlNamespaceResolver38.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver38.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver38.Add("dash", "clr-namespace:CarScannerXamarinForms.Dashboard");
			xmlNamespaceResolver38.Add("input", "clr-namespace:Plugin.InputKit.Shared.Controls;assembly=Plugin.InputKit");
			xmlNamespaceResolver38.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver38.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver38.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver38.Add("syncfusion", "clr-namespace:Syncfusion.XForms.Buttons;assembly=Syncfusion.Buttons.XForms");
			xmlNamespaceResolver38.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider38.Add(typeFromHandle76, new XamlTypeResolver(xmlNamespaceResolver38, typeof(SettingsInterfacePage).GetTypeInfo().Assembly));
			xamlServiceProvider38.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(240, 80)));
			object obj69 = markupExtension38.ProvideValue(xamlServiceProvider38);
			labelSwitch7.Text = obj69;
			stackLayout4.Children.Add(labelSwitch7);
			grid3.SetValue(View.MarginProperty, new Thickness(0.0, 0.0, 0.0, 10.0));
			grid3.SetValue(Grid.RowSpacingProperty, 0.0);
			columnDefinition5.SetValue(ColumnDefinition.WidthProperty, new GridLengthTypeConverter().ConvertFromInvariantString("*"));
			grid3.GetValue(Grid.ColumnDefinitionsProperty).Add(columnDefinition5);
			columnDefinition6.SetValue(ColumnDefinition.WidthProperty, new GridLengthTypeConverter().ConvertFromInvariantString("auto"));
			grid3.GetValue(Grid.ColumnDefinitionsProperty).Add(columnDefinition6);
			label12.SetValue(Grid.RowProperty, 0);
			label12.SetValue(Grid.ColumnProperty, 0);
			label12.SetValue(Label.LineBreakModeProperty, 1);
			translate26.Text = "settings_DataRecordLineSplitterTime";
			IMarkupExtension markupExtension39 = translate26;
			XamlServiceProvider xamlServiceProvider39 = new XamlServiceProvider();
			Type typeFromHandle77 = typeof(IProvideValueTarget);
			object[] array39 = new object[0 + 5];
			array39[0] = label12;
			array39[1] = grid3;
			array39[2] = stackLayout4;
			array39[3] = scrollView;
			array39[4] = this;
			object obj70;
			xamlServiceProvider39.Add(typeFromHandle77, obj70 = new SimpleValueTargetProvider(array39, Label.TextProperty, nameScope));
			xamlServiceProvider39.Add(typeof(IReferenceProvider), obj70);
			Type typeFromHandle78 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver39 = new XmlNamespaceResolver();
			xmlNamespaceResolver39.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver39.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver39.Add("dash", "clr-namespace:CarScannerXamarinForms.Dashboard");
			xmlNamespaceResolver39.Add("input", "clr-namespace:Plugin.InputKit.Shared.Controls;assembly=Plugin.InputKit");
			xmlNamespaceResolver39.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver39.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver39.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver39.Add("syncfusion", "clr-namespace:Syncfusion.XForms.Buttons;assembly=Syncfusion.Buttons.XForms");
			xmlNamespaceResolver39.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider39.Add(typeFromHandle78, new XamlTypeResolver(xmlNamespaceResolver39, typeof(SettingsInterfacePage).GetTypeInfo().Assembly));
			xamlServiceProvider39.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(253, 25)));
			object obj71 = markupExtension39.ProvideValue(xamlServiceProvider39);
			label12.Text = obj71;
			grid3.Children.Add(label12);
			label13.SetValue(Grid.RowProperty, 0);
			label13.SetValue(Grid.ColumnProperty, 1);
			label13.SetValue(Label.FontAttributesProperty, new FontAttributesConverter().ConvertFromInvariantString("Bold"));
			label13.SetValue(Label.LineBreakModeProperty, 0);
			bindingExtension24.Mode = 2;
			bindingExtension24.Path = "DataRecordLineSplitterTime";
			bindingExtension24.TypedBinding = new TypedBinding<SharedSettings, int>(delegate(SharedSettings A_0)
			{
				if (A_0 != null)
				{
					return new ValueTuple<int, bool>(A_0.DataRecordLineSplitterTime, true);
				}
				return default(ValueTuple<int, bool>);
			}, null, new Tuple<Func<SharedSettings, object>, string>[]
			{
				new Tuple<Func<SharedSettings, object>, string>((SharedSettings A_0) => A_0, "DataRecordLineSplitterTime")
			});
			BindingBase bindingBase24 = bindingExtension24.ProvideValue(null);
			label13.SetBinding(Label.TextProperty, bindingBase24);
			label13.SetValue(View.VerticalOptionsProperty, LayoutOptions.Center);
			grid3.Children.Add(label13);
			stackLayout4.Children.Add(grid3);
			stepper2.SetValue(Grid.RowProperty, 1);
			stepper2.SetValue(Grid.ColumnProperty, 1);
			stepper2.SetValue(View.HorizontalOptionsProperty, LayoutOptions.EndAndExpand);
			stepper2.SetValue(Stepper.IncrementProperty, 5.0);
			stepper2.SetValue(Stepper.MaximumProperty, 10000.0);
			stepper2.SetValue(Stepper.MinimumProperty, 5.0);
			bindingExtension25.Mode = 1;
			bindingExtension25.Path = "DataRecordLineSplitterTime";
			bindingExtension25.TypedBinding = new TypedBinding<SharedSettings, int>(delegate(SharedSettings A_0)
			{
				if (A_0 != null)
				{
					return new ValueTuple<int, bool>(A_0.DataRecordLineSplitterTime, true);
				}
				return default(ValueTuple<int, bool>);
			}, delegate(SharedSettings A_0, int A_1)
			{
				if (A_0 != null)
				{
					A_0.DataRecordLineSplitterTime = A_1;
					return;
				}
			}, new Tuple<Func<SharedSettings, object>, string>[]
			{
				new Tuple<Func<SharedSettings, object>, string>((SharedSettings A_0) => A_0, "DataRecordLineSplitterTime")
			});
			BindingBase bindingBase25 = bindingExtension25.ProvideValue(null);
			stepper2.SetBinding(Stepper.ValueProperty, bindingBase25);
			stackLayout4.Children.Add(stepper2);
			grid4.SetValue(View.MarginProperty, new Thickness(0.0, 0.0, 0.0, 10.0));
			grid4.SetValue(Grid.RowSpacingProperty, 0.0);
			columnDefinition7.SetValue(ColumnDefinition.WidthProperty, new GridLengthTypeConverter().ConvertFromInvariantString("*"));
			grid4.GetValue(Grid.ColumnDefinitionsProperty).Add(columnDefinition7);
			columnDefinition8.SetValue(ColumnDefinition.WidthProperty, new GridLengthTypeConverter().ConvertFromInvariantString("auto"));
			grid4.GetValue(Grid.ColumnDefinitionsProperty).Add(columnDefinition8);
			rowDefinition3.SetValue(RowDefinition.HeightProperty, new GridLengthTypeConverter().ConvertFromInvariantString("auto"));
			grid4.GetValue(Grid.RowDefinitionsProperty).Add(rowDefinition3);
			rowDefinition4.SetValue(RowDefinition.HeightProperty, new GridLengthTypeConverter().ConvertFromInvariantString("auto"));
			grid4.GetValue(Grid.RowDefinitionsProperty).Add(rowDefinition4);
			label14.SetValue(Grid.RowProperty, 1);
			label14.SetValue(Grid.ColumnProperty, 0);
			label14.SetValue(Label.FontAttributesProperty, new FontAttributesConverter().ConvertFromInvariantString("Italic"));
			bindingExtension26.Mode = 2;
			bindingExtension26.Path = "Use_km";
			bindingExtension26.TypedBinding = new TypedBinding<SharedSettings, bool>(delegate(SharedSettings A_0)
			{
				if (A_0 != null)
				{
					return new ValueTuple<bool, bool>(A_0.Use_km, true);
				}
				return default(ValueTuple<bool, bool>);
			}, null, new Tuple<Func<SharedSettings, object>, string>[]
			{
				new Tuple<Func<SharedSettings, object>, string>((SharedSettings A_0) => A_0, "Use_km")
			});
			BindingBase bindingBase26 = bindingExtension26.ProvideValue(null);
			label14.SetBinding(VisualElement.IsVisibleProperty, bindingBase26);
			translate27.Text = "Settings_Control_ToggleSpeedAndDistance.OnContent";
			IMarkupExtension markupExtension40 = translate27;
			XamlServiceProvider xamlServiceProvider40 = new XamlServiceProvider();
			Type typeFromHandle79 = typeof(IProvideValueTarget);
			object[] array40 = new object[0 + 5];
			array40[0] = label14;
			array40[1] = grid4;
			array40[2] = stackLayout4;
			array40[3] = scrollView;
			array40[4] = this;
			object obj72;
			xamlServiceProvider40.Add(typeFromHandle79, obj72 = new SimpleValueTargetProvider(array40, Label.TextProperty, nameScope));
			xamlServiceProvider40.Add(typeof(IReferenceProvider), obj72);
			Type typeFromHandle80 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver40 = new XmlNamespaceResolver();
			xmlNamespaceResolver40.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver40.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver40.Add("dash", "clr-namespace:CarScannerXamarinForms.Dashboard");
			xmlNamespaceResolver40.Add("input", "clr-namespace:Plugin.InputKit.Shared.Controls;assembly=Plugin.InputKit");
			xmlNamespaceResolver40.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver40.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver40.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver40.Add("syncfusion", "clr-namespace:Syncfusion.XForms.Buttons;assembly=Syncfusion.Buttons.XForms");
			xmlNamespaceResolver40.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider40.Add(typeFromHandle80, new XamlTypeResolver(xmlNamespaceResolver40, typeof(SettingsInterfacePage).GetTypeInfo().Assembly));
			xamlServiceProvider40.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(287, 25)));
			object obj73 = markupExtension40.ProvideValue(xamlServiceProvider40);
			label14.Text = obj73;
			label14.SetValue(View.VerticalOptionsProperty, LayoutOptions.Center);
			grid4.Children.Add(label14);
			label15.SetValue(Grid.RowProperty, 1);
			label15.SetValue(Grid.ColumnProperty, 0);
			label15.SetValue(Label.FontAttributesProperty, new FontAttributesConverter().ConvertFromInvariantString("Italic"));
			bindingExtension27.Mode = 2;
			staticResourceExtension7.Key = "BoolToNegativeConverter";
			IMarkupExtension markupExtension41 = staticResourceExtension7;
			XamlServiceProvider xamlServiceProvider41 = new XamlServiceProvider();
			Type typeFromHandle81 = typeof(IProvideValueTarget);
			object[] array41 = new object[0 + 6];
			array41[0] = bindingExtension27;
			array41[1] = label15;
			array41[2] = grid4;
			array41[3] = stackLayout4;
			array41[4] = scrollView;
			array41[5] = this;
			object obj74;
			xamlServiceProvider41.Add(typeFromHandle81, obj74 = new SimpleValueTargetProvider(array41, typeof(BindingExtension).GetRuntimeProperty("Converter"), nameScope));
			xamlServiceProvider41.Add(typeof(IReferenceProvider), obj74);
			Type typeFromHandle82 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver41 = new XmlNamespaceResolver();
			xmlNamespaceResolver41.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver41.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver41.Add("dash", "clr-namespace:CarScannerXamarinForms.Dashboard");
			xmlNamespaceResolver41.Add("input", "clr-namespace:Plugin.InputKit.Shared.Controls;assembly=Plugin.InputKit");
			xmlNamespaceResolver41.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver41.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver41.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver41.Add("syncfusion", "clr-namespace:Syncfusion.XForms.Buttons;assembly=Syncfusion.Buttons.XForms");
			xmlNamespaceResolver41.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider41.Add(typeFromHandle82, new XamlTypeResolver(xmlNamespaceResolver41, typeof(SettingsInterfacePage).GetTypeInfo().Assembly));
			xamlServiceProvider41.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(293, 25)));
			object obj75 = markupExtension41.ProvideValue(xamlServiceProvider41);
			bindingExtension27.Converter = obj75;
			bindingExtension27.Path = "Use_km";
			bindingExtension27.TypedBinding = new TypedBinding<SharedSettings, bool>(delegate(SharedSettings A_0)
			{
				if (A_0 != null)
				{
					return new ValueTuple<bool, bool>(A_0.Use_km, true);
				}
				return default(ValueTuple<bool, bool>);
			}, null, new Tuple<Func<SharedSettings, object>, string>[]
			{
				new Tuple<Func<SharedSettings, object>, string>((SharedSettings A_0) => A_0, "Use_km")
			});
			BindingBase bindingBase27 = bindingExtension27.ProvideValue(null);
			label15.SetBinding(VisualElement.IsVisibleProperty, bindingBase27);
			translate28.Text = "Settings_Control_ToggleSpeedAndDistance.OffContent";
			IMarkupExtension markupExtension42 = translate28;
			XamlServiceProvider xamlServiceProvider42 = new XamlServiceProvider();
			Type typeFromHandle83 = typeof(IProvideValueTarget);
			object[] array42 = new object[0 + 5];
			array42[0] = label15;
			array42[1] = grid4;
			array42[2] = stackLayout4;
			array42[3] = scrollView;
			array42[4] = this;
			object obj76;
			xamlServiceProvider42.Add(typeFromHandle83, obj76 = new SimpleValueTargetProvider(array42, Label.TextProperty, nameScope));
			xamlServiceProvider42.Add(typeof(IReferenceProvider), obj76);
			Type typeFromHandle84 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver42 = new XmlNamespaceResolver();
			xmlNamespaceResolver42.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver42.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver42.Add("dash", "clr-namespace:CarScannerXamarinForms.Dashboard");
			xmlNamespaceResolver42.Add("input", "clr-namespace:Plugin.InputKit.Shared.Controls;assembly=Plugin.InputKit");
			xmlNamespaceResolver42.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver42.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver42.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver42.Add("syncfusion", "clr-namespace:Syncfusion.XForms.Buttons;assembly=Syncfusion.Buttons.XForms");
			xmlNamespaceResolver42.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider42.Add(typeFromHandle84, new XamlTypeResolver(xmlNamespaceResolver42, typeof(SettingsInterfacePage).GetTypeInfo().Assembly));
			xamlServiceProvider42.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(294, 25)));
			object obj77 = markupExtension42.ProvideValue(xamlServiceProvider42);
			label15.Text = obj77;
			label15.SetValue(View.VerticalOptionsProperty, LayoutOptions.Center);
			grid4.Children.Add(label15);
			label16.SetValue(Grid.RowProperty, 0);
			label16.SetValue(Grid.ColumnProperty, 0);
			translate29.Text = "Settings_Control_tbSpeedDistanceTemperatureVolume.Text";
			IMarkupExtension markupExtension43 = translate29;
			XamlServiceProvider xamlServiceProvider43 = new XamlServiceProvider();
			Type typeFromHandle85 = typeof(IProvideValueTarget);
			object[] array43 = new object[0 + 5];
			array43[0] = label16;
			array43[1] = grid4;
			array43[2] = stackLayout4;
			array43[3] = scrollView;
			array43[4] = this;
			object obj78;
			xamlServiceProvider43.Add(typeFromHandle85, obj78 = new SimpleValueTargetProvider(array43, Label.TextProperty, nameScope));
			xamlServiceProvider43.Add(typeof(IReferenceProvider), obj78);
			Type typeFromHandle86 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver43 = new XmlNamespaceResolver();
			xmlNamespaceResolver43.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver43.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver43.Add("dash", "clr-namespace:CarScannerXamarinForms.Dashboard");
			xmlNamespaceResolver43.Add("input", "clr-namespace:Plugin.InputKit.Shared.Controls;assembly=Plugin.InputKit");
			xmlNamespaceResolver43.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver43.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver43.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver43.Add("syncfusion", "clr-namespace:Syncfusion.XForms.Buttons;assembly=Syncfusion.Buttons.XForms");
			xmlNamespaceResolver43.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider43.Add(typeFromHandle86, new XamlTypeResolver(xmlNamespaceResolver43, typeof(SettingsInterfacePage).GetTypeInfo().Assembly));
			xamlServiceProvider43.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(300, 25)));
			object obj79 = markupExtension43.ProvideValue(xamlServiceProvider43);
			label16.Text = obj79;
			grid4.Children.Add(label16);
			@switch.SetValue(Grid.RowProperty, 1);
			@switch.SetValue(Grid.ColumnProperty, 1);
			bindingExtension28.Mode = 1;
			bindingExtension28.Path = "Use_km";
			bindingExtension28.TypedBinding = new TypedBinding<SharedSettings, bool>(delegate(SharedSettings A_0)
			{
				if (A_0 != null)
				{
					return new ValueTuple<bool, bool>(A_0.Use_km, true);
				}
				return default(ValueTuple<bool, bool>);
			}, delegate(SharedSettings A_0, bool A_1)
			{
				if (A_0 != null)
				{
					A_0.Use_km = A_1;
					return;
				}
			}, new Tuple<Func<SharedSettings, object>, string>[]
			{
				new Tuple<Func<SharedSettings, object>, string>((SharedSettings A_0) => A_0, "Use_km")
			});
			BindingBase bindingBase28 = bindingExtension28.ProvideValue(null);
			@switch.SetBinding(Switch.IsToggledProperty, bindingBase28);
			grid4.Children.Add(@switch);
			stackLayout4.Children.Add(grid4);
			label17.SetValue(Grid.RowProperty, 0);
			label17.SetValue(Grid.ColumnProperty, 0);
			translate30.Text = "ios_UnitsForFuel";
			IMarkupExtension markupExtension44 = translate30;
			XamlServiceProvider xamlServiceProvider44 = new XamlServiceProvider();
			Type typeFromHandle87 = typeof(IProvideValueTarget);
			object[] array44 = new object[0 + 4];
			array44[0] = label17;
			array44[1] = stackLayout4;
			array44[2] = scrollView;
			array44[3] = this;
			object obj80;
			xamlServiceProvider44.Add(typeFromHandle87, obj80 = new SimpleValueTargetProvider(array44, Label.TextProperty, nameScope));
			xamlServiceProvider44.Add(typeof(IReferenceProvider), obj80);
			Type typeFromHandle88 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver44 = new XmlNamespaceResolver();
			xmlNamespaceResolver44.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver44.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver44.Add("dash", "clr-namespace:CarScannerXamarinForms.Dashboard");
			xmlNamespaceResolver44.Add("input", "clr-namespace:Plugin.InputKit.Shared.Controls;assembly=Plugin.InputKit");
			xmlNamespaceResolver44.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver44.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver44.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver44.Add("syncfusion", "clr-namespace:Syncfusion.XForms.Buttons;assembly=Syncfusion.Buttons.XForms");
			xmlNamespaceResolver44.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider44.Add(typeFromHandle88, new XamlTypeResolver(xmlNamespaceResolver44, typeof(SettingsInterfacePage).GetTypeInfo().Assembly));
			xamlServiceProvider44.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(311, 21)));
			object obj81 = markupExtension44.ProvideValue(xamlServiceProvider44);
			label17.Text = obj81;
			stackLayout4.Children.Add(label17);
			bindingExtension29.Source = fuelConsumptionUnits;
			BindingBase bindingBase29 = bindingExtension29.ProvideValue(null);
			picker4.SetBinding(Picker.ItemsSourceProperty, bindingBase29);
			bindingExtension30.Mode = 1;
			staticResourceExtension8.Key = "EnumToIntConverter";
			IMarkupExtension markupExtension45 = staticResourceExtension8;
			XamlServiceProvider xamlServiceProvider45 = new XamlServiceProvider();
			Type typeFromHandle89 = typeof(IProvideValueTarget);
			object[] array45 = new object[0 + 5];
			array45[0] = bindingExtension30;
			array45[1] = picker4;
			array45[2] = stackLayout4;
			array45[3] = scrollView;
			array45[4] = this;
			object obj82;
			xamlServiceProvider45.Add(typeFromHandle89, obj82 = new SimpleValueTargetProvider(array45, typeof(BindingExtension).GetRuntimeProperty("Converter"), nameScope));
			xamlServiceProvider45.Add(typeof(IReferenceProvider), obj82);
			Type typeFromHandle90 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver45 = new XmlNamespaceResolver();
			xmlNamespaceResolver45.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver45.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver45.Add("dash", "clr-namespace:CarScannerXamarinForms.Dashboard");
			xmlNamespaceResolver45.Add("input", "clr-namespace:Plugin.InputKit.Shared.Controls;assembly=Plugin.InputKit");
			xmlNamespaceResolver45.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver45.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver45.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver45.Add("syncfusion", "clr-namespace:Syncfusion.XForms.Buttons;assembly=Syncfusion.Buttons.XForms");
			xmlNamespaceResolver45.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider45.Add(typeFromHandle90, new XamlTypeResolver(xmlNamespaceResolver45, typeof(SettingsInterfacePage).GetTypeInfo().Assembly));
			xamlServiceProvider45.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(312, 106)));
			object obj83 = markupExtension45.ProvideValue(xamlServiceProvider45);
			bindingExtension30.Converter = obj83;
			bindingExtension30.Path = "FuelConsumptionUnit";
			bindingExtension30.TypedBinding = new TypedBinding<SharedSettings, FuelConsumptionUnits>(delegate(SharedSettings A_0)
			{
				if (A_0 != null)
				{
					return new ValueTuple<FuelConsumptionUnits, bool>(A_0.FuelConsumptionUnit, true);
				}
				return default(ValueTuple<FuelConsumptionUnits, bool>);
			}, delegate(SharedSettings A_0, FuelConsumptionUnits A_1)
			{
				if (A_0 != null)
				{
					A_0.FuelConsumptionUnit = A_1;
					return;
				}
			}, new Tuple<Func<SharedSettings, object>, string>[]
			{
				new Tuple<Func<SharedSettings, object>, string>((SharedSettings A_0) => A_0, "FuelConsumptionUnit")
			});
			BindingBase bindingBase30 = bindingExtension30.ProvideValue(null);
			picker4.SetBinding(Picker.SelectedIndexProperty, bindingBase30);
			stackLayout4.Children.Add(picker4);
			grid5.SetValue(View.MarginProperty, new Thickness(0.0, 0.0, 0.0, 10.0));
			grid5.SetValue(Grid.RowSpacingProperty, 0.0);
			columnDefinition9.SetValue(ColumnDefinition.WidthProperty, new GridLengthTypeConverter().ConvertFromInvariantString("*"));
			grid5.GetValue(Grid.ColumnDefinitionsProperty).Add(columnDefinition9);
			columnDefinition10.SetValue(ColumnDefinition.WidthProperty, new GridLengthTypeConverter().ConvertFromInvariantString("auto"));
			grid5.GetValue(Grid.ColumnDefinitionsProperty).Add(columnDefinition10);
			rowDefinition5.SetValue(RowDefinition.HeightProperty, new GridLengthTypeConverter().ConvertFromInvariantString("auto"));
			grid5.GetValue(Grid.RowDefinitionsProperty).Add(rowDefinition5);
			rowDefinition6.SetValue(RowDefinition.HeightProperty, new GridLengthTypeConverter().ConvertFromInvariantString("auto"));
			grid5.GetValue(Grid.RowDefinitionsProperty).Add(rowDefinition6);
			label18.SetValue(Grid.RowProperty, 1);
			label18.SetValue(Grid.ColumnProperty, 0);
			label18.SetValue(Label.FontAttributesProperty, new FontAttributesConverter().ConvertFromInvariantString("Italic"));
			bindingExtension31.Mode = 2;
			bindingExtension31.Path = "UseLitersForVolume";
			bindingExtension31.TypedBinding = new TypedBinding<SharedSettings, bool>(delegate(SharedSettings A_0)
			{
				if (A_0 != null)
				{
					return new ValueTuple<bool, bool>(A_0.UseLitersForVolume, true);
				}
				return default(ValueTuple<bool, bool>);
			}, null, new Tuple<Func<SharedSettings, object>, string>[]
			{
				new Tuple<Func<SharedSettings, object>, string>((SharedSettings A_0) => A_0, "UseLitersForVolume")
			});
			BindingBase bindingBase31 = bindingExtension31.ProvideValue(null);
			label18.SetBinding(VisualElement.IsVisibleProperty, bindingBase31);
			translate31.Text = "ios_Liters";
			IMarkupExtension markupExtension46 = translate31;
			XamlServiceProvider xamlServiceProvider46 = new XamlServiceProvider();
			Type typeFromHandle91 = typeof(IProvideValueTarget);
			object[] array46 = new object[0 + 5];
			array46[0] = label18;
			array46[1] = grid5;
			array46[2] = stackLayout4;
			array46[3] = scrollView;
			array46[4] = this;
			object obj84;
			xamlServiceProvider46.Add(typeFromHandle91, obj84 = new SimpleValueTargetProvider(array46, Label.TextProperty, nameScope));
			xamlServiceProvider46.Add(typeof(IReferenceProvider), obj84);
			Type typeFromHandle92 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver46 = new XmlNamespaceResolver();
			xmlNamespaceResolver46.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver46.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver46.Add("dash", "clr-namespace:CarScannerXamarinForms.Dashboard");
			xmlNamespaceResolver46.Add("input", "clr-namespace:Plugin.InputKit.Shared.Controls;assembly=Plugin.InputKit");
			xmlNamespaceResolver46.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver46.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver46.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver46.Add("syncfusion", "clr-namespace:Syncfusion.XForms.Buttons;assembly=Syncfusion.Buttons.XForms");
			xmlNamespaceResolver46.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider46.Add(typeFromHandle92, new XamlTypeResolver(xmlNamespaceResolver46, typeof(SettingsInterfacePage).GetTypeInfo().Assembly));
			xamlServiceProvider46.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(348, 25)));
			object obj85 = markupExtension46.ProvideValue(xamlServiceProvider46);
			label18.Text = obj85;
			label18.SetValue(View.VerticalOptionsProperty, LayoutOptions.Center);
			grid5.Children.Add(label18);
			label19.SetValue(Grid.RowProperty, 1);
			label19.SetValue(Grid.ColumnProperty, 0);
			label19.SetValue(Label.FontAttributesProperty, new FontAttributesConverter().ConvertFromInvariantString("Italic"));
			bindingExtension32.Mode = 2;
			staticResourceExtension9.Key = "BoolToNegativeConverter";
			IMarkupExtension markupExtension47 = staticResourceExtension9;
			XamlServiceProvider xamlServiceProvider47 = new XamlServiceProvider();
			Type typeFromHandle93 = typeof(IProvideValueTarget);
			object[] array47 = new object[0 + 6];
			array47[0] = bindingExtension32;
			array47[1] = label19;
			array47[2] = grid5;
			array47[3] = stackLayout4;
			array47[4] = scrollView;
			array47[5] = this;
			object obj86;
			xamlServiceProvider47.Add(typeFromHandle93, obj86 = new SimpleValueTargetProvider(array47, typeof(BindingExtension).GetRuntimeProperty("Converter"), nameScope));
			xamlServiceProvider47.Add(typeof(IReferenceProvider), obj86);
			Type typeFromHandle94 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver47 = new XmlNamespaceResolver();
			xmlNamespaceResolver47.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver47.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver47.Add("dash", "clr-namespace:CarScannerXamarinForms.Dashboard");
			xmlNamespaceResolver47.Add("input", "clr-namespace:Plugin.InputKit.Shared.Controls;assembly=Plugin.InputKit");
			xmlNamespaceResolver47.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver47.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver47.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver47.Add("syncfusion", "clr-namespace:Syncfusion.XForms.Buttons;assembly=Syncfusion.Buttons.XForms");
			xmlNamespaceResolver47.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider47.Add(typeFromHandle94, new XamlTypeResolver(xmlNamespaceResolver47, typeof(SettingsInterfacePage).GetTypeInfo().Assembly));
			xamlServiceProvider47.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(354, 25)));
			object obj87 = markupExtension47.ProvideValue(xamlServiceProvider47);
			bindingExtension32.Converter = obj87;
			bindingExtension32.Path = "UseLitersForVolume";
			bindingExtension32.TypedBinding = new TypedBinding<SharedSettings, bool>(delegate(SharedSettings A_0)
			{
				if (A_0 != null)
				{
					return new ValueTuple<bool, bool>(A_0.UseLitersForVolume, true);
				}
				return default(ValueTuple<bool, bool>);
			}, null, new Tuple<Func<SharedSettings, object>, string>[]
			{
				new Tuple<Func<SharedSettings, object>, string>((SharedSettings A_0) => A_0, "UseLitersForVolume")
			});
			BindingBase bindingBase32 = bindingExtension32.ProvideValue(null);
			label19.SetBinding(VisualElement.IsVisibleProperty, bindingBase32);
			translate32.Text = "ios_Gallons";
			IMarkupExtension markupExtension48 = translate32;
			XamlServiceProvider xamlServiceProvider48 = new XamlServiceProvider();
			Type typeFromHandle95 = typeof(IProvideValueTarget);
			object[] array48 = new object[0 + 5];
			array48[0] = label19;
			array48[1] = grid5;
			array48[2] = stackLayout4;
			array48[3] = scrollView;
			array48[4] = this;
			object obj88;
			xamlServiceProvider48.Add(typeFromHandle95, obj88 = new SimpleValueTargetProvider(array48, Label.TextProperty, nameScope));
			xamlServiceProvider48.Add(typeof(IReferenceProvider), obj88);
			Type typeFromHandle96 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver48 = new XmlNamespaceResolver();
			xmlNamespaceResolver48.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver48.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver48.Add("dash", "clr-namespace:CarScannerXamarinForms.Dashboard");
			xmlNamespaceResolver48.Add("input", "clr-namespace:Plugin.InputKit.Shared.Controls;assembly=Plugin.InputKit");
			xmlNamespaceResolver48.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver48.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver48.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver48.Add("syncfusion", "clr-namespace:Syncfusion.XForms.Buttons;assembly=Syncfusion.Buttons.XForms");
			xmlNamespaceResolver48.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider48.Add(typeFromHandle96, new XamlTypeResolver(xmlNamespaceResolver48, typeof(SettingsInterfacePage).GetTypeInfo().Assembly));
			xamlServiceProvider48.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(355, 25)));
			object obj89 = markupExtension48.ProvideValue(xamlServiceProvider48);
			label19.Text = obj89;
			label19.SetValue(View.VerticalOptionsProperty, LayoutOptions.Center);
			grid5.Children.Add(label19);
			label20.SetValue(Grid.RowProperty, 0);
			label20.SetValue(Grid.ColumnProperty, 0);
			translate33.Text = "ios_VolumeUnits";
			IMarkupExtension markupExtension49 = translate33;
			XamlServiceProvider xamlServiceProvider49 = new XamlServiceProvider();
			Type typeFromHandle97 = typeof(IProvideValueTarget);
			object[] array49 = new object[0 + 5];
			array49[0] = label20;
			array49[1] = grid5;
			array49[2] = stackLayout4;
			array49[3] = scrollView;
			array49[4] = this;
			object obj90;
			xamlServiceProvider49.Add(typeFromHandle97, obj90 = new SimpleValueTargetProvider(array49, Label.TextProperty, nameScope));
			xamlServiceProvider49.Add(typeof(IReferenceProvider), obj90);
			Type typeFromHandle98 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver49 = new XmlNamespaceResolver();
			xmlNamespaceResolver49.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver49.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver49.Add("dash", "clr-namespace:CarScannerXamarinForms.Dashboard");
			xmlNamespaceResolver49.Add("input", "clr-namespace:Plugin.InputKit.Shared.Controls;assembly=Plugin.InputKit");
			xmlNamespaceResolver49.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver49.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver49.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver49.Add("syncfusion", "clr-namespace:Syncfusion.XForms.Buttons;assembly=Syncfusion.Buttons.XForms");
			xmlNamespaceResolver49.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider49.Add(typeFromHandle98, new XamlTypeResolver(xmlNamespaceResolver49, typeof(SettingsInterfacePage).GetTypeInfo().Assembly));
			xamlServiceProvider49.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(361, 25)));
			object obj91 = markupExtension49.ProvideValue(xamlServiceProvider49);
			label20.Text = obj91;
			grid5.Children.Add(label20);
			switch2.SetValue(Grid.RowProperty, 1);
			switch2.SetValue(Grid.ColumnProperty, 1);
			bindingExtension33.Mode = 1;
			bindingExtension33.Path = "UseLitersForVolume";
			bindingExtension33.TypedBinding = new TypedBinding<SharedSettings, bool>(delegate(SharedSettings A_0)
			{
				if (A_0 != null)
				{
					return new ValueTuple<bool, bool>(A_0.UseLitersForVolume, true);
				}
				return default(ValueTuple<bool, bool>);
			}, delegate(SharedSettings A_0, bool A_1)
			{
				if (A_0 != null)
				{
					A_0.UseLitersForVolume = A_1;
					return;
				}
			}, new Tuple<Func<SharedSettings, object>, string>[]
			{
				new Tuple<Func<SharedSettings, object>, string>((SharedSettings A_0) => A_0, "UseLitersForVolume")
			});
			BindingBase bindingBase33 = bindingExtension33.ProvideValue(null);
			switch2.SetBinding(Switch.IsToggledProperty, bindingBase33);
			grid5.Children.Add(switch2);
			stackLayout4.Children.Add(grid5);
			grid6.SetValue(View.MarginProperty, new Thickness(0.0, 0.0, 0.0, 10.0));
			bindingExtension34.Path = "ShowUSGallonSelector";
			bindingExtension34.TypedBinding = new TypedBinding<SharedSettings, bool>(delegate(SharedSettings A_0)
			{
				if (A_0 != null)
				{
					return new ValueTuple<bool, bool>(A_0.ShowUSGallonSelector, true);
				}
				return default(ValueTuple<bool, bool>);
			}, null, new Tuple<Func<SharedSettings, object>, string>[]
			{
				new Tuple<Func<SharedSettings, object>, string>((SharedSettings A_0) => A_0, "ShowUSGallonSelector")
			});
			BindingBase bindingBase34 = bindingExtension34.ProvideValue(null);
			grid6.SetBinding(VisualElement.IsVisibleProperty, bindingBase34);
			grid6.SetValue(Grid.RowSpacingProperty, 0.0);
			columnDefinition11.SetValue(ColumnDefinition.WidthProperty, new GridLengthTypeConverter().ConvertFromInvariantString("*"));
			grid6.GetValue(Grid.ColumnDefinitionsProperty).Add(columnDefinition11);
			columnDefinition12.SetValue(ColumnDefinition.WidthProperty, new GridLengthTypeConverter().ConvertFromInvariantString("auto"));
			grid6.GetValue(Grid.ColumnDefinitionsProperty).Add(columnDefinition12);
			rowDefinition7.SetValue(RowDefinition.HeightProperty, new GridLengthTypeConverter().ConvertFromInvariantString("auto"));
			grid6.GetValue(Grid.RowDefinitionsProperty).Add(rowDefinition7);
			rowDefinition8.SetValue(RowDefinition.HeightProperty, new GridLengthTypeConverter().ConvertFromInvariantString("auto"));
			grid6.GetValue(Grid.RowDefinitionsProperty).Add(rowDefinition8);
			label21.SetValue(Grid.RowProperty, 1);
			label21.SetValue(Grid.ColumnProperty, 0);
			label21.SetValue(Label.FontAttributesProperty, new FontAttributesConverter().ConvertFromInvariantString("Italic"));
			bindingExtension35.Mode = 2;
			bindingExtension35.Path = "UseUSGallon";
			bindingExtension35.TypedBinding = new TypedBinding<SharedSettings, bool>(delegate(SharedSettings A_0)
			{
				if (A_0 != null)
				{
					return new ValueTuple<bool, bool>(A_0.UseUSGallon, true);
				}
				return default(ValueTuple<bool, bool>);
			}, null, new Tuple<Func<SharedSettings, object>, string>[]
			{
				new Tuple<Func<SharedSettings, object>, string>((SharedSettings A_0) => A_0, "UseUSGallon")
			});
			BindingBase bindingBase35 = bindingExtension35.ProvideValue(null);
			label21.SetBinding(VisualElement.IsVisibleProperty, bindingBase35);
			translate34.Text = "Settings_Control_ToggleUSGallon.OnContent";
			IMarkupExtension markupExtension50 = translate34;
			XamlServiceProvider xamlServiceProvider50 = new XamlServiceProvider();
			Type typeFromHandle99 = typeof(IProvideValueTarget);
			object[] array50 = new object[0 + 5];
			array50[0] = label21;
			array50[1] = grid6;
			array50[2] = stackLayout4;
			array50[3] = scrollView;
			array50[4] = this;
			object obj92;
			xamlServiceProvider50.Add(typeFromHandle99, obj92 = new SimpleValueTargetProvider(array50, Label.TextProperty, nameScope));
			xamlServiceProvider50.Add(typeof(IReferenceProvider), obj92);
			Type typeFromHandle100 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver50 = new XmlNamespaceResolver();
			xmlNamespaceResolver50.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver50.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver50.Add("dash", "clr-namespace:CarScannerXamarinForms.Dashboard");
			xmlNamespaceResolver50.Add("input", "clr-namespace:Plugin.InputKit.Shared.Controls;assembly=Plugin.InputKit");
			xmlNamespaceResolver50.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver50.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver50.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver50.Add("syncfusion", "clr-namespace:Syncfusion.XForms.Buttons;assembly=Syncfusion.Buttons.XForms");
			xmlNamespaceResolver50.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider50.Add(typeFromHandle100, new XamlTypeResolver(xmlNamespaceResolver50, typeof(SettingsInterfacePage).GetTypeInfo().Assembly));
			xamlServiceProvider50.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(387, 25)));
			object obj93 = markupExtension50.ProvideValue(xamlServiceProvider50);
			label21.Text = obj93;
			label21.SetValue(View.VerticalOptionsProperty, LayoutOptions.Center);
			grid6.Children.Add(label21);
			label22.SetValue(Grid.RowProperty, 1);
			label22.SetValue(Grid.ColumnProperty, 0);
			label22.SetValue(Label.FontAttributesProperty, new FontAttributesConverter().ConvertFromInvariantString("Italic"));
			bindingExtension36.Mode = 2;
			staticResourceExtension10.Key = "BoolToNegativeConverter";
			IMarkupExtension markupExtension51 = staticResourceExtension10;
			XamlServiceProvider xamlServiceProvider51 = new XamlServiceProvider();
			Type typeFromHandle101 = typeof(IProvideValueTarget);
			object[] array51 = new object[0 + 6];
			array51[0] = bindingExtension36;
			array51[1] = label22;
			array51[2] = grid6;
			array51[3] = stackLayout4;
			array51[4] = scrollView;
			array51[5] = this;
			object obj94;
			xamlServiceProvider51.Add(typeFromHandle101, obj94 = new SimpleValueTargetProvider(array51, typeof(BindingExtension).GetRuntimeProperty("Converter"), nameScope));
			xamlServiceProvider51.Add(typeof(IReferenceProvider), obj94);
			Type typeFromHandle102 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver51 = new XmlNamespaceResolver();
			xmlNamespaceResolver51.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver51.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver51.Add("dash", "clr-namespace:CarScannerXamarinForms.Dashboard");
			xmlNamespaceResolver51.Add("input", "clr-namespace:Plugin.InputKit.Shared.Controls;assembly=Plugin.InputKit");
			xmlNamespaceResolver51.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver51.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver51.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver51.Add("syncfusion", "clr-namespace:Syncfusion.XForms.Buttons;assembly=Syncfusion.Buttons.XForms");
			xmlNamespaceResolver51.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider51.Add(typeFromHandle102, new XamlTypeResolver(xmlNamespaceResolver51, typeof(SettingsInterfacePage).GetTypeInfo().Assembly));
			xamlServiceProvider51.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(393, 25)));
			object obj95 = markupExtension51.ProvideValue(xamlServiceProvider51);
			bindingExtension36.Converter = obj95;
			bindingExtension36.Path = "UseUSGallon";
			bindingExtension36.TypedBinding = new TypedBinding<SharedSettings, bool>(delegate(SharedSettings A_0)
			{
				if (A_0 != null)
				{
					return new ValueTuple<bool, bool>(A_0.UseUSGallon, true);
				}
				return default(ValueTuple<bool, bool>);
			}, null, new Tuple<Func<SharedSettings, object>, string>[]
			{
				new Tuple<Func<SharedSettings, object>, string>((SharedSettings A_0) => A_0, "UseUSGallon")
			});
			BindingBase bindingBase36 = bindingExtension36.ProvideValue(null);
			label22.SetBinding(VisualElement.IsVisibleProperty, bindingBase36);
			translate35.Text = "Settings_Control_ToggleUSGallon.OffContent";
			IMarkupExtension markupExtension52 = translate35;
			XamlServiceProvider xamlServiceProvider52 = new XamlServiceProvider();
			Type typeFromHandle103 = typeof(IProvideValueTarget);
			object[] array52 = new object[0 + 5];
			array52[0] = label22;
			array52[1] = grid6;
			array52[2] = stackLayout4;
			array52[3] = scrollView;
			array52[4] = this;
			object obj96;
			xamlServiceProvider52.Add(typeFromHandle103, obj96 = new SimpleValueTargetProvider(array52, Label.TextProperty, nameScope));
			xamlServiceProvider52.Add(typeof(IReferenceProvider), obj96);
			Type typeFromHandle104 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver52 = new XmlNamespaceResolver();
			xmlNamespaceResolver52.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver52.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver52.Add("dash", "clr-namespace:CarScannerXamarinForms.Dashboard");
			xmlNamespaceResolver52.Add("input", "clr-namespace:Plugin.InputKit.Shared.Controls;assembly=Plugin.InputKit");
			xmlNamespaceResolver52.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver52.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver52.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver52.Add("syncfusion", "clr-namespace:Syncfusion.XForms.Buttons;assembly=Syncfusion.Buttons.XForms");
			xmlNamespaceResolver52.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider52.Add(typeFromHandle104, new XamlTypeResolver(xmlNamespaceResolver52, typeof(SettingsInterfacePage).GetTypeInfo().Assembly));
			xamlServiceProvider52.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(394, 25)));
			object obj97 = markupExtension52.ProvideValue(xamlServiceProvider52);
			label22.Text = obj97;
			label22.SetValue(View.VerticalOptionsProperty, LayoutOptions.Center);
			grid6.Children.Add(label22);
			label23.SetValue(Grid.RowProperty, 0);
			label23.SetValue(Grid.ColumnProperty, 0);
			translate36.Text = "Settings_Control_ToggleUSGallon.Header";
			IMarkupExtension markupExtension53 = translate36;
			XamlServiceProvider xamlServiceProvider53 = new XamlServiceProvider();
			Type typeFromHandle105 = typeof(IProvideValueTarget);
			object[] array53 = new object[0 + 5];
			array53[0] = label23;
			array53[1] = grid6;
			array53[2] = stackLayout4;
			array53[3] = scrollView;
			array53[4] = this;
			object obj98;
			xamlServiceProvider53.Add(typeFromHandle105, obj98 = new SimpleValueTargetProvider(array53, Label.TextProperty, nameScope));
			xamlServiceProvider53.Add(typeof(IReferenceProvider), obj98);
			Type typeFromHandle106 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver53 = new XmlNamespaceResolver();
			xmlNamespaceResolver53.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver53.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver53.Add("dash", "clr-namespace:CarScannerXamarinForms.Dashboard");
			xmlNamespaceResolver53.Add("input", "clr-namespace:Plugin.InputKit.Shared.Controls;assembly=Plugin.InputKit");
			xmlNamespaceResolver53.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver53.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver53.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver53.Add("syncfusion", "clr-namespace:Syncfusion.XForms.Buttons;assembly=Syncfusion.Buttons.XForms");
			xmlNamespaceResolver53.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider53.Add(typeFromHandle106, new XamlTypeResolver(xmlNamespaceResolver53, typeof(SettingsInterfacePage).GetTypeInfo().Assembly));
			xamlServiceProvider53.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(400, 25)));
			object obj99 = markupExtension53.ProvideValue(xamlServiceProvider53);
			label23.Text = obj99;
			grid6.Children.Add(label23);
			switch3.SetValue(Grid.RowProperty, 1);
			switch3.SetValue(Grid.ColumnProperty, 1);
			bindingExtension37.Mode = 1;
			bindingExtension37.Path = "UseUSGallon";
			bindingExtension37.TypedBinding = new TypedBinding<SharedSettings, bool>(delegate(SharedSettings A_0)
			{
				if (A_0 != null)
				{
					return new ValueTuple<bool, bool>(A_0.UseUSGallon, true);
				}
				return default(ValueTuple<bool, bool>);
			}, delegate(SharedSettings A_0, bool A_1)
			{
				if (A_0 != null)
				{
					A_0.UseUSGallon = A_1;
					return;
				}
			}, new Tuple<Func<SharedSettings, object>, string>[]
			{
				new Tuple<Func<SharedSettings, object>, string>((SharedSettings A_0) => A_0, "UseUSGallon")
			});
			BindingBase bindingBase37 = bindingExtension37.ProvideValue(null);
			switch3.SetBinding(Switch.IsToggledProperty, bindingBase37);
			grid6.Children.Add(switch3);
			stackLayout4.Children.Add(grid6);
			grid7.SetValue(View.MarginProperty, new Thickness(0.0, 0.0, 0.0, 10.0));
			grid7.SetValue(Grid.RowSpacingProperty, 0.0);
			columnDefinition13.SetValue(ColumnDefinition.WidthProperty, new GridLengthTypeConverter().ConvertFromInvariantString("*"));
			grid7.GetValue(Grid.ColumnDefinitionsProperty).Add(columnDefinition13);
			columnDefinition14.SetValue(ColumnDefinition.WidthProperty, new GridLengthTypeConverter().ConvertFromInvariantString("auto"));
			grid7.GetValue(Grid.ColumnDefinitionsProperty).Add(columnDefinition14);
			rowDefinition9.SetValue(RowDefinition.HeightProperty, new GridLengthTypeConverter().ConvertFromInvariantString("auto"));
			grid7.GetValue(Grid.RowDefinitionsProperty).Add(rowDefinition9);
			rowDefinition10.SetValue(RowDefinition.HeightProperty, new GridLengthTypeConverter().ConvertFromInvariantString("auto"));
			grid7.GetValue(Grid.RowDefinitionsProperty).Add(rowDefinition10);
			label24.SetValue(Grid.RowProperty, 1);
			label24.SetValue(Grid.ColumnProperty, 0);
			label24.SetValue(Label.FontAttributesProperty, new FontAttributesConverter().ConvertFromInvariantString("Italic"));
			bindingExtension38.Mode = 2;
			bindingExtension38.Path = "Pressure_use_kpa";
			bindingExtension38.TypedBinding = new TypedBinding<SharedSettings, bool>(delegate(SharedSettings A_0)
			{
				if (A_0 != null)
				{
					return new ValueTuple<bool, bool>(A_0.Pressure_use_kpa, true);
				}
				return default(ValueTuple<bool, bool>);
			}, null, new Tuple<Func<SharedSettings, object>, string>[]
			{
				new Tuple<Func<SharedSettings, object>, string>((SharedSettings A_0) => A_0, "Pressure_use_kpa")
			});
			BindingBase bindingBase38 = bindingExtension38.ProvideValue(null);
			label24.SetBinding(VisualElement.IsVisibleProperty, bindingBase38);
			translate37.Text = "Settings_Control_TogglePressureUnits.OnContent";
			IMarkupExtension markupExtension54 = translate37;
			XamlServiceProvider xamlServiceProvider54 = new XamlServiceProvider();
			Type typeFromHandle107 = typeof(IProvideValueTarget);
			object[] array54 = new object[0 + 5];
			array54[0] = label24;
			array54[1] = grid7;
			array54[2] = stackLayout4;
			array54[3] = scrollView;
			array54[4] = this;
			object obj100;
			xamlServiceProvider54.Add(typeFromHandle107, obj100 = new SimpleValueTargetProvider(array54, Label.TextProperty, nameScope));
			xamlServiceProvider54.Add(typeof(IReferenceProvider), obj100);
			Type typeFromHandle108 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver54 = new XmlNamespaceResolver();
			xmlNamespaceResolver54.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver54.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver54.Add("dash", "clr-namespace:CarScannerXamarinForms.Dashboard");
			xmlNamespaceResolver54.Add("input", "clr-namespace:Plugin.InputKit.Shared.Controls;assembly=Plugin.InputKit");
			xmlNamespaceResolver54.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver54.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver54.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver54.Add("syncfusion", "clr-namespace:Syncfusion.XForms.Buttons;assembly=Syncfusion.Buttons.XForms");
			xmlNamespaceResolver54.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider54.Add(typeFromHandle108, new XamlTypeResolver(xmlNamespaceResolver54, typeof(SettingsInterfacePage).GetTypeInfo().Assembly));
			xamlServiceProvider54.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(422, 25)));
			object obj101 = markupExtension54.ProvideValue(xamlServiceProvider54);
			label24.Text = obj101;
			label24.SetValue(View.VerticalOptionsProperty, LayoutOptions.Center);
			grid7.Children.Add(label24);
			label25.SetValue(Grid.RowProperty, 1);
			label25.SetValue(Grid.ColumnProperty, 0);
			label25.SetValue(Label.FontAttributesProperty, new FontAttributesConverter().ConvertFromInvariantString("Italic"));
			bindingExtension39.Mode = 2;
			staticResourceExtension11.Key = "BoolToNegativeConverter";
			IMarkupExtension markupExtension55 = staticResourceExtension11;
			XamlServiceProvider xamlServiceProvider55 = new XamlServiceProvider();
			Type typeFromHandle109 = typeof(IProvideValueTarget);
			object[] array55 = new object[0 + 6];
			array55[0] = bindingExtension39;
			array55[1] = label25;
			array55[2] = grid7;
			array55[3] = stackLayout4;
			array55[4] = scrollView;
			array55[5] = this;
			object obj102;
			xamlServiceProvider55.Add(typeFromHandle109, obj102 = new SimpleValueTargetProvider(array55, typeof(BindingExtension).GetRuntimeProperty("Converter"), nameScope));
			xamlServiceProvider55.Add(typeof(IReferenceProvider), obj102);
			Type typeFromHandle110 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver55 = new XmlNamespaceResolver();
			xmlNamespaceResolver55.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver55.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver55.Add("dash", "clr-namespace:CarScannerXamarinForms.Dashboard");
			xmlNamespaceResolver55.Add("input", "clr-namespace:Plugin.InputKit.Shared.Controls;assembly=Plugin.InputKit");
			xmlNamespaceResolver55.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver55.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver55.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver55.Add("syncfusion", "clr-namespace:Syncfusion.XForms.Buttons;assembly=Syncfusion.Buttons.XForms");
			xmlNamespaceResolver55.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider55.Add(typeFromHandle110, new XamlTypeResolver(xmlNamespaceResolver55, typeof(SettingsInterfacePage).GetTypeInfo().Assembly));
			xamlServiceProvider55.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(428, 25)));
			object obj103 = markupExtension55.ProvideValue(xamlServiceProvider55);
			bindingExtension39.Converter = obj103;
			bindingExtension39.Path = "Pressure_use_kpa";
			bindingExtension39.TypedBinding = new TypedBinding<SharedSettings, bool>(delegate(SharedSettings A_0)
			{
				if (A_0 != null)
				{
					return new ValueTuple<bool, bool>(A_0.Pressure_use_kpa, true);
				}
				return default(ValueTuple<bool, bool>);
			}, null, new Tuple<Func<SharedSettings, object>, string>[]
			{
				new Tuple<Func<SharedSettings, object>, string>((SharedSettings A_0) => A_0, "Pressure_use_kpa")
			});
			BindingBase bindingBase39 = bindingExtension39.ProvideValue(null);
			label25.SetBinding(VisualElement.IsVisibleProperty, bindingBase39);
			translate38.Text = "Settings_Control_TogglePressureUnits.OffContent";
			IMarkupExtension markupExtension56 = translate38;
			XamlServiceProvider xamlServiceProvider56 = new XamlServiceProvider();
			Type typeFromHandle111 = typeof(IProvideValueTarget);
			object[] array56 = new object[0 + 5];
			array56[0] = label25;
			array56[1] = grid7;
			array56[2] = stackLayout4;
			array56[3] = scrollView;
			array56[4] = this;
			object obj104;
			xamlServiceProvider56.Add(typeFromHandle111, obj104 = new SimpleValueTargetProvider(array56, Label.TextProperty, nameScope));
			xamlServiceProvider56.Add(typeof(IReferenceProvider), obj104);
			Type typeFromHandle112 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver56 = new XmlNamespaceResolver();
			xmlNamespaceResolver56.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver56.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver56.Add("dash", "clr-namespace:CarScannerXamarinForms.Dashboard");
			xmlNamespaceResolver56.Add("input", "clr-namespace:Plugin.InputKit.Shared.Controls;assembly=Plugin.InputKit");
			xmlNamespaceResolver56.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver56.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver56.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver56.Add("syncfusion", "clr-namespace:Syncfusion.XForms.Buttons;assembly=Syncfusion.Buttons.XForms");
			xmlNamespaceResolver56.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider56.Add(typeFromHandle112, new XamlTypeResolver(xmlNamespaceResolver56, typeof(SettingsInterfacePage).GetTypeInfo().Assembly));
			xamlServiceProvider56.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(429, 25)));
			object obj105 = markupExtension56.ProvideValue(xamlServiceProvider56);
			label25.Text = obj105;
			label25.SetValue(View.VerticalOptionsProperty, LayoutOptions.Center);
			grid7.Children.Add(label25);
			label26.SetValue(Grid.RowProperty, 0);
			label26.SetValue(Grid.ColumnProperty, 0);
			translate39.Text = "Settings_Control_TogglePressureUnits.Header";
			IMarkupExtension markupExtension57 = translate39;
			XamlServiceProvider xamlServiceProvider57 = new XamlServiceProvider();
			Type typeFromHandle113 = typeof(IProvideValueTarget);
			object[] array57 = new object[0 + 5];
			array57[0] = label26;
			array57[1] = grid7;
			array57[2] = stackLayout4;
			array57[3] = scrollView;
			array57[4] = this;
			object obj106;
			xamlServiceProvider57.Add(typeFromHandle113, obj106 = new SimpleValueTargetProvider(array57, Label.TextProperty, nameScope));
			xamlServiceProvider57.Add(typeof(IReferenceProvider), obj106);
			Type typeFromHandle114 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver57 = new XmlNamespaceResolver();
			xmlNamespaceResolver57.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver57.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver57.Add("dash", "clr-namespace:CarScannerXamarinForms.Dashboard");
			xmlNamespaceResolver57.Add("input", "clr-namespace:Plugin.InputKit.Shared.Controls;assembly=Plugin.InputKit");
			xmlNamespaceResolver57.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver57.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver57.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver57.Add("syncfusion", "clr-namespace:Syncfusion.XForms.Buttons;assembly=Syncfusion.Buttons.XForms");
			xmlNamespaceResolver57.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider57.Add(typeFromHandle114, new XamlTypeResolver(xmlNamespaceResolver57, typeof(SettingsInterfacePage).GetTypeInfo().Assembly));
			xamlServiceProvider57.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(435, 25)));
			object obj107 = markupExtension57.ProvideValue(xamlServiceProvider57);
			label26.Text = obj107;
			grid7.Children.Add(label26);
			switch4.SetValue(Grid.RowProperty, 1);
			switch4.SetValue(Grid.ColumnProperty, 1);
			bindingExtension40.Mode = 1;
			bindingExtension40.Path = "Pressure_use_kpa";
			bindingExtension40.TypedBinding = new TypedBinding<SharedSettings, bool>(delegate(SharedSettings A_0)
			{
				if (A_0 != null)
				{
					return new ValueTuple<bool, bool>(A_0.Pressure_use_kpa, true);
				}
				return default(ValueTuple<bool, bool>);
			}, delegate(SharedSettings A_0, bool A_1)
			{
				if (A_0 != null)
				{
					A_0.Pressure_use_kpa = A_1;
					return;
				}
			}, new Tuple<Func<SharedSettings, object>, string>[]
			{
				new Tuple<Func<SharedSettings, object>, string>((SharedSettings A_0) => A_0, "Pressure_use_kpa")
			});
			BindingBase bindingBase40 = bindingExtension40.ProvideValue(null);
			switch4.SetBinding(Switch.IsToggledProperty, bindingBase40);
			grid7.Children.Add(switch4);
			stackLayout4.Children.Add(grid7);
			grid8.SetValue(View.MarginProperty, new Thickness(0.0, 0.0, 0.0, 10.0));
			grid8.SetValue(Grid.RowSpacingProperty, 0.0);
			columnDefinition15.SetValue(ColumnDefinition.WidthProperty, new GridLengthTypeConverter().ConvertFromInvariantString("*"));
			grid8.GetValue(Grid.ColumnDefinitionsProperty).Add(columnDefinition15);
			columnDefinition16.SetValue(ColumnDefinition.WidthProperty, new GridLengthTypeConverter().ConvertFromInvariantString("auto"));
			grid8.GetValue(Grid.ColumnDefinitionsProperty).Add(columnDefinition16);
			rowDefinition11.SetValue(RowDefinition.HeightProperty, new GridLengthTypeConverter().ConvertFromInvariantString("auto"));
			grid8.GetValue(Grid.RowDefinitionsProperty).Add(rowDefinition11);
			rowDefinition12.SetValue(RowDefinition.HeightProperty, new GridLengthTypeConverter().ConvertFromInvariantString("auto"));
			grid8.GetValue(Grid.RowDefinitionsProperty).Add(rowDefinition12);
			label27.SetValue(Grid.RowProperty, 1);
			label27.SetValue(Grid.ColumnProperty, 0);
			label27.SetValue(Label.FontAttributesProperty, new FontAttributesConverter().ConvertFromInvariantString("Italic"));
			bindingExtension41.Mode = 2;
			bindingExtension41.Path = "Flow_use_grams_sec";
			bindingExtension41.TypedBinding = new TypedBinding<SharedSettings, bool>(delegate(SharedSettings A_0)
			{
				if (A_0 != null)
				{
					return new ValueTuple<bool, bool>(A_0.Flow_use_grams_sec, true);
				}
				return default(ValueTuple<bool, bool>);
			}, null, new Tuple<Func<SharedSettings, object>, string>[]
			{
				new Tuple<Func<SharedSettings, object>, string>((SharedSettings A_0) => A_0, "Flow_use_grams_sec")
			});
			BindingBase bindingBase41 = bindingExtension41.ProvideValue(null);
			label27.SetBinding(VisualElement.IsVisibleProperty, bindingBase41);
			translate40.Text = "Settings_Control_ToggleFlowUnits.OnContent";
			IMarkupExtension markupExtension58 = translate40;
			XamlServiceProvider xamlServiceProvider58 = new XamlServiceProvider();
			Type typeFromHandle115 = typeof(IProvideValueTarget);
			object[] array58 = new object[0 + 5];
			array58[0] = label27;
			array58[1] = grid8;
			array58[2] = stackLayout4;
			array58[3] = scrollView;
			array58[4] = this;
			object obj108;
			xamlServiceProvider58.Add(typeFromHandle115, obj108 = new SimpleValueTargetProvider(array58, Label.TextProperty, nameScope));
			xamlServiceProvider58.Add(typeof(IReferenceProvider), obj108);
			Type typeFromHandle116 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver58 = new XmlNamespaceResolver();
			xmlNamespaceResolver58.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver58.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver58.Add("dash", "clr-namespace:CarScannerXamarinForms.Dashboard");
			xmlNamespaceResolver58.Add("input", "clr-namespace:Plugin.InputKit.Shared.Controls;assembly=Plugin.InputKit");
			xmlNamespaceResolver58.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver58.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver58.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver58.Add("syncfusion", "clr-namespace:Syncfusion.XForms.Buttons;assembly=Syncfusion.Buttons.XForms");
			xmlNamespaceResolver58.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider58.Add(typeFromHandle116, new XamlTypeResolver(xmlNamespaceResolver58, typeof(SettingsInterfacePage).GetTypeInfo().Assembly));
			xamlServiceProvider58.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(457, 25)));
			object obj109 = markupExtension58.ProvideValue(xamlServiceProvider58);
			label27.Text = obj109;
			label27.SetValue(View.VerticalOptionsProperty, LayoutOptions.Center);
			grid8.Children.Add(label27);
			label28.SetValue(Grid.RowProperty, 1);
			label28.SetValue(Grid.ColumnProperty, 0);
			label28.SetValue(Label.FontAttributesProperty, new FontAttributesConverter().ConvertFromInvariantString("Italic"));
			bindingExtension42.Mode = 2;
			staticResourceExtension12.Key = "BoolToNegativeConverter";
			IMarkupExtension markupExtension59 = staticResourceExtension12;
			XamlServiceProvider xamlServiceProvider59 = new XamlServiceProvider();
			Type typeFromHandle117 = typeof(IProvideValueTarget);
			object[] array59 = new object[0 + 6];
			array59[0] = bindingExtension42;
			array59[1] = label28;
			array59[2] = grid8;
			array59[3] = stackLayout4;
			array59[4] = scrollView;
			array59[5] = this;
			object obj110;
			xamlServiceProvider59.Add(typeFromHandle117, obj110 = new SimpleValueTargetProvider(array59, typeof(BindingExtension).GetRuntimeProperty("Converter"), nameScope));
			xamlServiceProvider59.Add(typeof(IReferenceProvider), obj110);
			Type typeFromHandle118 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver59 = new XmlNamespaceResolver();
			xmlNamespaceResolver59.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver59.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver59.Add("dash", "clr-namespace:CarScannerXamarinForms.Dashboard");
			xmlNamespaceResolver59.Add("input", "clr-namespace:Plugin.InputKit.Shared.Controls;assembly=Plugin.InputKit");
			xmlNamespaceResolver59.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver59.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver59.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver59.Add("syncfusion", "clr-namespace:Syncfusion.XForms.Buttons;assembly=Syncfusion.Buttons.XForms");
			xmlNamespaceResolver59.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider59.Add(typeFromHandle118, new XamlTypeResolver(xmlNamespaceResolver59, typeof(SettingsInterfacePage).GetTypeInfo().Assembly));
			xamlServiceProvider59.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(463, 25)));
			object obj111 = markupExtension59.ProvideValue(xamlServiceProvider59);
			bindingExtension42.Converter = obj111;
			bindingExtension42.Path = "Flow_use_grams_sec";
			bindingExtension42.TypedBinding = new TypedBinding<SharedSettings, bool>(delegate(SharedSettings A_0)
			{
				if (A_0 != null)
				{
					return new ValueTuple<bool, bool>(A_0.Flow_use_grams_sec, true);
				}
				return default(ValueTuple<bool, bool>);
			}, null, new Tuple<Func<SharedSettings, object>, string>[]
			{
				new Tuple<Func<SharedSettings, object>, string>((SharedSettings A_0) => A_0, "Flow_use_grams_sec")
			});
			BindingBase bindingBase42 = bindingExtension42.ProvideValue(null);
			label28.SetBinding(VisualElement.IsVisibleProperty, bindingBase42);
			translate41.Text = "Settings_Control_ToggleFlowUnits.OffContent";
			IMarkupExtension markupExtension60 = translate41;
			XamlServiceProvider xamlServiceProvider60 = new XamlServiceProvider();
			Type typeFromHandle119 = typeof(IProvideValueTarget);
			object[] array60 = new object[0 + 5];
			array60[0] = label28;
			array60[1] = grid8;
			array60[2] = stackLayout4;
			array60[3] = scrollView;
			array60[4] = this;
			object obj112;
			xamlServiceProvider60.Add(typeFromHandle119, obj112 = new SimpleValueTargetProvider(array60, Label.TextProperty, nameScope));
			xamlServiceProvider60.Add(typeof(IReferenceProvider), obj112);
			Type typeFromHandle120 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver60 = new XmlNamespaceResolver();
			xmlNamespaceResolver60.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver60.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver60.Add("dash", "clr-namespace:CarScannerXamarinForms.Dashboard");
			xmlNamespaceResolver60.Add("input", "clr-namespace:Plugin.InputKit.Shared.Controls;assembly=Plugin.InputKit");
			xmlNamespaceResolver60.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver60.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver60.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver60.Add("syncfusion", "clr-namespace:Syncfusion.XForms.Buttons;assembly=Syncfusion.Buttons.XForms");
			xmlNamespaceResolver60.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider60.Add(typeFromHandle120, new XamlTypeResolver(xmlNamespaceResolver60, typeof(SettingsInterfacePage).GetTypeInfo().Assembly));
			xamlServiceProvider60.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(464, 25)));
			object obj113 = markupExtension60.ProvideValue(xamlServiceProvider60);
			label28.Text = obj113;
			label28.SetValue(View.VerticalOptionsProperty, LayoutOptions.Center);
			grid8.Children.Add(label28);
			label29.SetValue(Grid.RowProperty, 0);
			label29.SetValue(Grid.ColumnProperty, 0);
			translate42.Text = "Settings_Control_ToggleFlowUnits.Header";
			IMarkupExtension markupExtension61 = translate42;
			XamlServiceProvider xamlServiceProvider61 = new XamlServiceProvider();
			Type typeFromHandle121 = typeof(IProvideValueTarget);
			object[] array61 = new object[0 + 5];
			array61[0] = label29;
			array61[1] = grid8;
			array61[2] = stackLayout4;
			array61[3] = scrollView;
			array61[4] = this;
			object obj114;
			xamlServiceProvider61.Add(typeFromHandle121, obj114 = new SimpleValueTargetProvider(array61, Label.TextProperty, nameScope));
			xamlServiceProvider61.Add(typeof(IReferenceProvider), obj114);
			Type typeFromHandle122 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver61 = new XmlNamespaceResolver();
			xmlNamespaceResolver61.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver61.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver61.Add("dash", "clr-namespace:CarScannerXamarinForms.Dashboard");
			xmlNamespaceResolver61.Add("input", "clr-namespace:Plugin.InputKit.Shared.Controls;assembly=Plugin.InputKit");
			xmlNamespaceResolver61.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver61.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver61.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver61.Add("syncfusion", "clr-namespace:Syncfusion.XForms.Buttons;assembly=Syncfusion.Buttons.XForms");
			xmlNamespaceResolver61.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider61.Add(typeFromHandle122, new XamlTypeResolver(xmlNamespaceResolver61, typeof(SettingsInterfacePage).GetTypeInfo().Assembly));
			xamlServiceProvider61.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(470, 25)));
			object obj115 = markupExtension61.ProvideValue(xamlServiceProvider61);
			label29.Text = obj115;
			grid8.Children.Add(label29);
			switch5.SetValue(Grid.RowProperty, 1);
			switch5.SetValue(Grid.ColumnProperty, 1);
			bindingExtension43.Mode = 1;
			bindingExtension43.Path = "Flow_use_grams_sec";
			bindingExtension43.TypedBinding = new TypedBinding<SharedSettings, bool>(delegate(SharedSettings A_0)
			{
				if (A_0 != null)
				{
					return new ValueTuple<bool, bool>(A_0.Flow_use_grams_sec, true);
				}
				return default(ValueTuple<bool, bool>);
			}, delegate(SharedSettings A_0, bool A_1)
			{
				if (A_0 != null)
				{
					A_0.Flow_use_grams_sec = A_1;
					return;
				}
			}, new Tuple<Func<SharedSettings, object>, string>[]
			{
				new Tuple<Func<SharedSettings, object>, string>((SharedSettings A_0) => A_0, "Flow_use_grams_sec")
			});
			BindingBase bindingBase43 = bindingExtension43.ProvideValue(null);
			switch5.SetBinding(Switch.IsToggledProperty, bindingBase43);
			grid8.Children.Add(switch5);
			stackLayout4.Children.Add(grid8);
			grid9.SetValue(View.MarginProperty, new Thickness(0.0, 0.0, 0.0, 10.0));
			grid9.SetValue(Grid.RowSpacingProperty, 0.0);
			columnDefinition17.SetValue(ColumnDefinition.WidthProperty, new GridLengthTypeConverter().ConvertFromInvariantString("*"));
			grid9.GetValue(Grid.ColumnDefinitionsProperty).Add(columnDefinition17);
			columnDefinition18.SetValue(ColumnDefinition.WidthProperty, new GridLengthTypeConverter().ConvertFromInvariantString("auto"));
			grid9.GetValue(Grid.ColumnDefinitionsProperty).Add(columnDefinition18);
			rowDefinition13.SetValue(RowDefinition.HeightProperty, new GridLengthTypeConverter().ConvertFromInvariantString("auto"));
			grid9.GetValue(Grid.RowDefinitionsProperty).Add(rowDefinition13);
			rowDefinition14.SetValue(RowDefinition.HeightProperty, new GridLengthTypeConverter().ConvertFromInvariantString("auto"));
			grid9.GetValue(Grid.RowDefinitionsProperty).Add(rowDefinition14);
			label30.SetValue(Grid.RowProperty, 1);
			label30.SetValue(Grid.ColumnProperty, 0);
			label30.SetValue(Label.FontAttributesProperty, new FontAttributesConverter().ConvertFromInvariantString("Italic"));
			bindingExtension44.Mode = 2;
			bindingExtension44.Path = "Use_celcium";
			bindingExtension44.TypedBinding = new TypedBinding<SharedSettings, bool>(delegate(SharedSettings A_0)
			{
				if (A_0 != null)
				{
					return new ValueTuple<bool, bool>(A_0.Use_celcium, true);
				}
				return default(ValueTuple<bool, bool>);
			}, null, new Tuple<Func<SharedSettings, object>, string>[]
			{
				new Tuple<Func<SharedSettings, object>, string>((SharedSettings A_0) => A_0, "Use_celcium")
			});
			BindingBase bindingBase44 = bindingExtension44.ProvideValue(null);
			label30.SetBinding(VisualElement.IsVisibleProperty, bindingBase44);
			translate43.Text = "Settings_Control_ToggleTemperatureUnits.OnContent";
			IMarkupExtension markupExtension62 = translate43;
			XamlServiceProvider xamlServiceProvider62 = new XamlServiceProvider();
			Type typeFromHandle123 = typeof(IProvideValueTarget);
			object[] array62 = new object[0 + 5];
			array62[0] = label30;
			array62[1] = grid9;
			array62[2] = stackLayout4;
			array62[3] = scrollView;
			array62[4] = this;
			object obj116;
			xamlServiceProvider62.Add(typeFromHandle123, obj116 = new SimpleValueTargetProvider(array62, Label.TextProperty, nameScope));
			xamlServiceProvider62.Add(typeof(IReferenceProvider), obj116);
			Type typeFromHandle124 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver62 = new XmlNamespaceResolver();
			xmlNamespaceResolver62.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver62.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver62.Add("dash", "clr-namespace:CarScannerXamarinForms.Dashboard");
			xmlNamespaceResolver62.Add("input", "clr-namespace:Plugin.InputKit.Shared.Controls;assembly=Plugin.InputKit");
			xmlNamespaceResolver62.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver62.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver62.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver62.Add("syncfusion", "clr-namespace:Syncfusion.XForms.Buttons;assembly=Syncfusion.Buttons.XForms");
			xmlNamespaceResolver62.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider62.Add(typeFromHandle124, new XamlTypeResolver(xmlNamespaceResolver62, typeof(SettingsInterfacePage).GetTypeInfo().Assembly));
			xamlServiceProvider62.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(491, 25)));
			object obj117 = markupExtension62.ProvideValue(xamlServiceProvider62);
			label30.Text = obj117;
			label30.SetValue(View.VerticalOptionsProperty, LayoutOptions.Center);
			grid9.Children.Add(label30);
			label31.SetValue(Grid.RowProperty, 1);
			label31.SetValue(Grid.ColumnProperty, 0);
			label31.SetValue(Label.FontAttributesProperty, new FontAttributesConverter().ConvertFromInvariantString("Italic"));
			bindingExtension45.Mode = 2;
			staticResourceExtension13.Key = "BoolToNegativeConverter";
			IMarkupExtension markupExtension63 = staticResourceExtension13;
			XamlServiceProvider xamlServiceProvider63 = new XamlServiceProvider();
			Type typeFromHandle125 = typeof(IProvideValueTarget);
			object[] array63 = new object[0 + 6];
			array63[0] = bindingExtension45;
			array63[1] = label31;
			array63[2] = grid9;
			array63[3] = stackLayout4;
			array63[4] = scrollView;
			array63[5] = this;
			object obj118;
			xamlServiceProvider63.Add(typeFromHandle125, obj118 = new SimpleValueTargetProvider(array63, typeof(BindingExtension).GetRuntimeProperty("Converter"), nameScope));
			xamlServiceProvider63.Add(typeof(IReferenceProvider), obj118);
			Type typeFromHandle126 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver63 = new XmlNamespaceResolver();
			xmlNamespaceResolver63.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver63.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver63.Add("dash", "clr-namespace:CarScannerXamarinForms.Dashboard");
			xmlNamespaceResolver63.Add("input", "clr-namespace:Plugin.InputKit.Shared.Controls;assembly=Plugin.InputKit");
			xmlNamespaceResolver63.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver63.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver63.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver63.Add("syncfusion", "clr-namespace:Syncfusion.XForms.Buttons;assembly=Syncfusion.Buttons.XForms");
			xmlNamespaceResolver63.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider63.Add(typeFromHandle126, new XamlTypeResolver(xmlNamespaceResolver63, typeof(SettingsInterfacePage).GetTypeInfo().Assembly));
			xamlServiceProvider63.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(497, 25)));
			object obj119 = markupExtension63.ProvideValue(xamlServiceProvider63);
			bindingExtension45.Converter = obj119;
			bindingExtension45.Path = "Use_celcium";
			bindingExtension45.TypedBinding = new TypedBinding<SharedSettings, bool>(delegate(SharedSettings A_0)
			{
				if (A_0 != null)
				{
					return new ValueTuple<bool, bool>(A_0.Use_celcium, true);
				}
				return default(ValueTuple<bool, bool>);
			}, null, new Tuple<Func<SharedSettings, object>, string>[]
			{
				new Tuple<Func<SharedSettings, object>, string>((SharedSettings A_0) => A_0, "Use_celcium")
			});
			BindingBase bindingBase45 = bindingExtension45.ProvideValue(null);
			label31.SetBinding(VisualElement.IsVisibleProperty, bindingBase45);
			translate44.Text = "Settings_Control_ToggleTemperatureUnits.OffContent";
			IMarkupExtension markupExtension64 = translate44;
			XamlServiceProvider xamlServiceProvider64 = new XamlServiceProvider();
			Type typeFromHandle127 = typeof(IProvideValueTarget);
			object[] array64 = new object[0 + 5];
			array64[0] = label31;
			array64[1] = grid9;
			array64[2] = stackLayout4;
			array64[3] = scrollView;
			array64[4] = this;
			object obj120;
			xamlServiceProvider64.Add(typeFromHandle127, obj120 = new SimpleValueTargetProvider(array64, Label.TextProperty, nameScope));
			xamlServiceProvider64.Add(typeof(IReferenceProvider), obj120);
			Type typeFromHandle128 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver64 = new XmlNamespaceResolver();
			xmlNamespaceResolver64.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver64.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver64.Add("dash", "clr-namespace:CarScannerXamarinForms.Dashboard");
			xmlNamespaceResolver64.Add("input", "clr-namespace:Plugin.InputKit.Shared.Controls;assembly=Plugin.InputKit");
			xmlNamespaceResolver64.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver64.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver64.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver64.Add("syncfusion", "clr-namespace:Syncfusion.XForms.Buttons;assembly=Syncfusion.Buttons.XForms");
			xmlNamespaceResolver64.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider64.Add(typeFromHandle128, new XamlTypeResolver(xmlNamespaceResolver64, typeof(SettingsInterfacePage).GetTypeInfo().Assembly));
			xamlServiceProvider64.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(498, 25)));
			object obj121 = markupExtension64.ProvideValue(xamlServiceProvider64);
			label31.Text = obj121;
			label31.SetValue(View.VerticalOptionsProperty, LayoutOptions.Center);
			grid9.Children.Add(label31);
			label32.SetValue(Grid.RowProperty, 0);
			label32.SetValue(Grid.ColumnProperty, 0);
			translate45.Text = "Settings_Control_ToggleTemperatureUnits.Header";
			IMarkupExtension markupExtension65 = translate45;
			XamlServiceProvider xamlServiceProvider65 = new XamlServiceProvider();
			Type typeFromHandle129 = typeof(IProvideValueTarget);
			object[] array65 = new object[0 + 5];
			array65[0] = label32;
			array65[1] = grid9;
			array65[2] = stackLayout4;
			array65[3] = scrollView;
			array65[4] = this;
			object obj122;
			xamlServiceProvider65.Add(typeFromHandle129, obj122 = new SimpleValueTargetProvider(array65, Label.TextProperty, nameScope));
			xamlServiceProvider65.Add(typeof(IReferenceProvider), obj122);
			Type typeFromHandle130 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver65 = new XmlNamespaceResolver();
			xmlNamespaceResolver65.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver65.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver65.Add("dash", "clr-namespace:CarScannerXamarinForms.Dashboard");
			xmlNamespaceResolver65.Add("input", "clr-namespace:Plugin.InputKit.Shared.Controls;assembly=Plugin.InputKit");
			xmlNamespaceResolver65.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver65.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver65.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver65.Add("syncfusion", "clr-namespace:Syncfusion.XForms.Buttons;assembly=Syncfusion.Buttons.XForms");
			xmlNamespaceResolver65.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider65.Add(typeFromHandle130, new XamlTypeResolver(xmlNamespaceResolver65, typeof(SettingsInterfacePage).GetTypeInfo().Assembly));
			xamlServiceProvider65.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(504, 25)));
			object obj123 = markupExtension65.ProvideValue(xamlServiceProvider65);
			label32.Text = obj123;
			grid9.Children.Add(label32);
			switch6.SetValue(Grid.RowProperty, 1);
			switch6.SetValue(Grid.ColumnProperty, 1);
			bindingExtension46.Mode = 1;
			bindingExtension46.Path = "Use_celcium";
			bindingExtension46.TypedBinding = new TypedBinding<SharedSettings, bool>(delegate(SharedSettings A_0)
			{
				if (A_0 != null)
				{
					return new ValueTuple<bool, bool>(A_0.Use_celcium, true);
				}
				return default(ValueTuple<bool, bool>);
			}, delegate(SharedSettings A_0, bool A_1)
			{
				if (A_0 != null)
				{
					A_0.Use_celcium = A_1;
					return;
				}
			}, new Tuple<Func<SharedSettings, object>, string>[]
			{
				new Tuple<Func<SharedSettings, object>, string>((SharedSettings A_0) => A_0, "Use_celcium")
			});
			BindingBase bindingBase46 = bindingExtension46.ProvideValue(null);
			switch6.SetBinding(Switch.IsToggledProperty, bindingBase46);
			grid9.Children.Add(switch6);
			stackLayout4.Children.Add(grid9);
			grid10.SetValue(View.MarginProperty, new Thickness(0.0, 0.0, 0.0, 10.0));
			grid10.SetValue(Grid.RowSpacingProperty, 0.0);
			columnDefinition19.SetValue(ColumnDefinition.WidthProperty, new GridLengthTypeConverter().ConvertFromInvariantString("*"));
			grid10.GetValue(Grid.ColumnDefinitionsProperty).Add(columnDefinition19);
			columnDefinition20.SetValue(ColumnDefinition.WidthProperty, new GridLengthTypeConverter().ConvertFromInvariantString("auto"));
			grid10.GetValue(Grid.ColumnDefinitionsProperty).Add(columnDefinition20);
			rowDefinition15.SetValue(RowDefinition.HeightProperty, new GridLengthTypeConverter().ConvertFromInvariantString("auto"));
			grid10.GetValue(Grid.RowDefinitionsProperty).Add(rowDefinition15);
			rowDefinition16.SetValue(RowDefinition.HeightProperty, new GridLengthTypeConverter().ConvertFromInvariantString("auto"));
			grid10.GetValue(Grid.RowDefinitionsProperty).Add(rowDefinition16);
			label33.SetValue(Grid.RowProperty, 1);
			label33.SetValue(Grid.ColumnProperty, 0);
			label33.SetValue(Label.FontAttributesProperty, new FontAttributesConverter().ConvertFromInvariantString("Italic"));
			bindingExtension47.Mode = 2;
			bindingExtension47.Path = "AccelerationUseG";
			bindingExtension47.TypedBinding = new TypedBinding<SharedSettings, bool>(delegate(SharedSettings A_0)
			{
				if (A_0 != null)
				{
					return new ValueTuple<bool, bool>(A_0.AccelerationUseG, true);
				}
				return default(ValueTuple<bool, bool>);
			}, null, new Tuple<Func<SharedSettings, object>, string>[]
			{
				new Tuple<Func<SharedSettings, object>, string>((SharedSettings A_0) => A_0, "AccelerationUseG")
			});
			BindingBase bindingBase47 = bindingExtension47.ProvideValue(null);
			label33.SetBinding(VisualElement.IsVisibleProperty, bindingBase47);
			label33.SetValue(Label.TextProperty, "g");
			label33.SetValue(View.VerticalOptionsProperty, LayoutOptions.Center);
			grid10.Children.Add(label33);
			label34.SetValue(Grid.RowProperty, 1);
			label34.SetValue(Grid.ColumnProperty, 0);
			label34.SetValue(Label.FontAttributesProperty, new FontAttributesConverter().ConvertFromInvariantString("Italic"));
			bindingExtension48.Mode = 2;
			staticResourceExtension14.Key = "BoolToNegativeConverter";
			IMarkupExtension markupExtension66 = staticResourceExtension14;
			XamlServiceProvider xamlServiceProvider66 = new XamlServiceProvider();
			Type typeFromHandle131 = typeof(IProvideValueTarget);
			object[] array66 = new object[0 + 6];
			array66[0] = bindingExtension48;
			array66[1] = label34;
			array66[2] = grid10;
			array66[3] = stackLayout4;
			array66[4] = scrollView;
			array66[5] = this;
			object obj124;
			xamlServiceProvider66.Add(typeFromHandle131, obj124 = new SimpleValueTargetProvider(array66, typeof(BindingExtension).GetRuntimeProperty("Converter"), nameScope));
			xamlServiceProvider66.Add(typeof(IReferenceProvider), obj124);
			Type typeFromHandle132 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver66 = new XmlNamespaceResolver();
			xmlNamespaceResolver66.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver66.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver66.Add("dash", "clr-namespace:CarScannerXamarinForms.Dashboard");
			xmlNamespaceResolver66.Add("input", "clr-namespace:Plugin.InputKit.Shared.Controls;assembly=Plugin.InputKit");
			xmlNamespaceResolver66.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver66.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver66.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver66.Add("syncfusion", "clr-namespace:Syncfusion.XForms.Buttons;assembly=Syncfusion.Buttons.XForms");
			xmlNamespaceResolver66.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider66.Add(typeFromHandle132, new XamlTypeResolver(xmlNamespaceResolver66, typeof(SettingsInterfacePage).GetTypeInfo().Assembly));
			xamlServiceProvider66.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(533, 25)));
			object obj125 = markupExtension66.ProvideValue(xamlServiceProvider66);
			bindingExtension48.Converter = obj125;
			bindingExtension48.Path = "AccelerationUseG";
			bindingExtension48.TypedBinding = new TypedBinding<SharedSettings, bool>(delegate(SharedSettings A_0)
			{
				if (A_0 != null)
				{
					return new ValueTuple<bool, bool>(A_0.AccelerationUseG, true);
				}
				return default(ValueTuple<bool, bool>);
			}, null, new Tuple<Func<SharedSettings, object>, string>[]
			{
				new Tuple<Func<SharedSettings, object>, string>((SharedSettings A_0) => A_0, "AccelerationUseG")
			});
			BindingBase bindingBase48 = bindingExtension48.ProvideValue(null);
			label34.SetBinding(VisualElement.IsVisibleProperty, bindingBase48);
			label34.SetValue(Label.TextProperty, "m/sec^2");
			label34.SetValue(View.VerticalOptionsProperty, LayoutOptions.Center);
			grid10.Children.Add(label34);
			label35.SetValue(Grid.RowProperty, 0);
			label35.SetValue(Grid.ColumnProperty, 0);
			translate46.Text = "Settings_Control_ToggleAccelerationUnits.Header";
			IMarkupExtension markupExtension67 = translate46;
			XamlServiceProvider xamlServiceProvider67 = new XamlServiceProvider();
			Type typeFromHandle133 = typeof(IProvideValueTarget);
			object[] array67 = new object[0 + 5];
			array67[0] = label35;
			array67[1] = grid10;
			array67[2] = stackLayout4;
			array67[3] = scrollView;
			array67[4] = this;
			object obj126;
			xamlServiceProvider67.Add(typeFromHandle133, obj126 = new SimpleValueTargetProvider(array67, Label.TextProperty, nameScope));
			xamlServiceProvider67.Add(typeof(IReferenceProvider), obj126);
			Type typeFromHandle134 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver67 = new XmlNamespaceResolver();
			xmlNamespaceResolver67.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver67.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver67.Add("dash", "clr-namespace:CarScannerXamarinForms.Dashboard");
			xmlNamespaceResolver67.Add("input", "clr-namespace:Plugin.InputKit.Shared.Controls;assembly=Plugin.InputKit");
			xmlNamespaceResolver67.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver67.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver67.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver67.Add("syncfusion", "clr-namespace:Syncfusion.XForms.Buttons;assembly=Syncfusion.Buttons.XForms");
			xmlNamespaceResolver67.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider67.Add(typeFromHandle134, new XamlTypeResolver(xmlNamespaceResolver67, typeof(SettingsInterfacePage).GetTypeInfo().Assembly));
			xamlServiceProvider67.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(540, 25)));
			object obj127 = markupExtension67.ProvideValue(xamlServiceProvider67);
			label35.Text = obj127;
			grid10.Children.Add(label35);
			switch7.SetValue(Grid.RowProperty, 1);
			switch7.SetValue(Grid.ColumnProperty, 1);
			bindingExtension49.Mode = 1;
			bindingExtension49.Path = "AccelerationUseG";
			bindingExtension49.TypedBinding = new TypedBinding<SharedSettings, bool>(delegate(SharedSettings A_0)
			{
				if (A_0 != null)
				{
					return new ValueTuple<bool, bool>(A_0.AccelerationUseG, true);
				}
				return default(ValueTuple<bool, bool>);
			}, delegate(SharedSettings A_0, bool A_1)
			{
				if (A_0 != null)
				{
					A_0.AccelerationUseG = A_1;
					return;
				}
			}, new Tuple<Func<SharedSettings, object>, string>[]
			{
				new Tuple<Func<SharedSettings, object>, string>((SharedSettings A_0) => A_0, "AccelerationUseG")
			});
			BindingBase bindingBase49 = bindingExtension49.ProvideValue(null);
			switch7.SetBinding(Switch.IsToggledProperty, bindingBase49);
			grid10.Children.Add(switch7);
			stackLayout4.Children.Add(grid10);
			grid11.SetValue(View.MarginProperty, new Thickness(0.0, 0.0, 0.0, 10.0));
			grid11.SetValue(Grid.RowSpacingProperty, 0.0);
			columnDefinition21.SetValue(ColumnDefinition.WidthProperty, new GridLengthTypeConverter().ConvertFromInvariantString("*"));
			grid11.GetValue(Grid.ColumnDefinitionsProperty).Add(columnDefinition21);
			columnDefinition22.SetValue(ColumnDefinition.WidthProperty, new GridLengthTypeConverter().ConvertFromInvariantString("auto"));
			grid11.GetValue(Grid.ColumnDefinitionsProperty).Add(columnDefinition22);
			rowDefinition17.SetValue(RowDefinition.HeightProperty, new GridLengthTypeConverter().ConvertFromInvariantString("auto"));
			grid11.GetValue(Grid.RowDefinitionsProperty).Add(rowDefinition17);
			rowDefinition18.SetValue(RowDefinition.HeightProperty, new GridLengthTypeConverter().ConvertFromInvariantString("auto"));
			grid11.GetValue(Grid.RowDefinitionsProperty).Add(rowDefinition18);
			label36.SetValue(Grid.RowProperty, 1);
			label36.SetValue(Grid.ColumnProperty, 0);
			label36.SetValue(Label.FontAttributesProperty, new FontAttributesConverter().ConvertFromInvariantString("Italic"));
			bindingExtension50.Mode = 2;
			bindingExtension50.Path = "UseHoursePower";
			bindingExtension50.TypedBinding = new TypedBinding<SharedSettings, bool>(delegate(SharedSettings A_0)
			{
				if (A_0 != null)
				{
					return new ValueTuple<bool, bool>(A_0.UseHoursePower, true);
				}
				return default(ValueTuple<bool, bool>);
			}, null, new Tuple<Func<SharedSettings, object>, string>[]
			{
				new Tuple<Func<SharedSettings, object>, string>((SharedSettings A_0) => A_0, "UseHoursePower")
			});
			BindingBase bindingBase50 = bindingExtension50.ProvideValue(null);
			label36.SetBinding(VisualElement.IsVisibleProperty, bindingBase50);
			label36.SetValue(Label.TextProperty, "hp");
			label36.SetValue(View.VerticalOptionsProperty, LayoutOptions.Center);
			grid11.Children.Add(label36);
			label37.SetValue(Grid.RowProperty, 1);
			label37.SetValue(Grid.ColumnProperty, 0);
			label37.SetValue(Label.FontAttributesProperty, new FontAttributesConverter().ConvertFromInvariantString("Italic"));
			bindingExtension51.Mode = 2;
			staticResourceExtension15.Key = "BoolToNegativeConverter";
			IMarkupExtension markupExtension68 = staticResourceExtension15;
			XamlServiceProvider xamlServiceProvider68 = new XamlServiceProvider();
			Type typeFromHandle135 = typeof(IProvideValueTarget);
			object[] array68 = new object[0 + 6];
			array68[0] = bindingExtension51;
			array68[1] = label37;
			array68[2] = grid11;
			array68[3] = stackLayout4;
			array68[4] = scrollView;
			array68[5] = this;
			object obj128;
			xamlServiceProvider68.Add(typeFromHandle135, obj128 = new SimpleValueTargetProvider(array68, typeof(BindingExtension).GetRuntimeProperty("Converter"), nameScope));
			xamlServiceProvider68.Add(typeof(IReferenceProvider), obj128);
			Type typeFromHandle136 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver68 = new XmlNamespaceResolver();
			xmlNamespaceResolver68.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver68.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver68.Add("dash", "clr-namespace:CarScannerXamarinForms.Dashboard");
			xmlNamespaceResolver68.Add("input", "clr-namespace:Plugin.InputKit.Shared.Controls;assembly=Plugin.InputKit");
			xmlNamespaceResolver68.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver68.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver68.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver68.Add("syncfusion", "clr-namespace:Syncfusion.XForms.Buttons;assembly=Syncfusion.Buttons.XForms");
			xmlNamespaceResolver68.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider68.Add(typeFromHandle136, new XamlTypeResolver(xmlNamespaceResolver68, typeof(SettingsInterfacePage).GetTypeInfo().Assembly));
			xamlServiceProvider68.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(573, 25)));
			object obj129 = markupExtension68.ProvideValue(xamlServiceProvider68);
			bindingExtension51.Converter = obj129;
			bindingExtension51.Path = "UseHoursePower";
			bindingExtension51.TypedBinding = new TypedBinding<SharedSettings, bool>(delegate(SharedSettings A_0)
			{
				if (A_0 != null)
				{
					return new ValueTuple<bool, bool>(A_0.UseHoursePower, true);
				}
				return default(ValueTuple<bool, bool>);
			}, null, new Tuple<Func<SharedSettings, object>, string>[]
			{
				new Tuple<Func<SharedSettings, object>, string>((SharedSettings A_0) => A_0, "UseHoursePower")
			});
			BindingBase bindingBase51 = bindingExtension51.ProvideValue(null);
			label37.SetBinding(VisualElement.IsVisibleProperty, bindingBase51);
			label37.SetValue(Label.TextProperty, "kW");
			label37.SetValue(View.VerticalOptionsProperty, LayoutOptions.Center);
			grid11.Children.Add(label37);
			label38.SetValue(Grid.RowProperty, 0);
			label38.SetValue(Grid.ColumnProperty, 0);
			translate47.Text = "Settings_Control_TogglePowerUnits.Header";
			IMarkupExtension markupExtension69 = translate47;
			XamlServiceProvider xamlServiceProvider69 = new XamlServiceProvider();
			Type typeFromHandle137 = typeof(IProvideValueTarget);
			object[] array69 = new object[0 + 5];
			array69[0] = label38;
			array69[1] = grid11;
			array69[2] = stackLayout4;
			array69[3] = scrollView;
			array69[4] = this;
			object obj130;
			xamlServiceProvider69.Add(typeFromHandle137, obj130 = new SimpleValueTargetProvider(array69, Label.TextProperty, nameScope));
			xamlServiceProvider69.Add(typeof(IReferenceProvider), obj130);
			Type typeFromHandle138 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver69 = new XmlNamespaceResolver();
			xmlNamespaceResolver69.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver69.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver69.Add("dash", "clr-namespace:CarScannerXamarinForms.Dashboard");
			xmlNamespaceResolver69.Add("input", "clr-namespace:Plugin.InputKit.Shared.Controls;assembly=Plugin.InputKit");
			xmlNamespaceResolver69.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver69.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver69.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver69.Add("syncfusion", "clr-namespace:Syncfusion.XForms.Buttons;assembly=Syncfusion.Buttons.XForms");
			xmlNamespaceResolver69.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider69.Add(typeFromHandle138, new XamlTypeResolver(xmlNamespaceResolver69, typeof(SettingsInterfacePage).GetTypeInfo().Assembly));
			xamlServiceProvider69.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(580, 25)));
			object obj131 = markupExtension69.ProvideValue(xamlServiceProvider69);
			label38.Text = obj131;
			grid11.Children.Add(label38);
			switch8.SetValue(Grid.RowProperty, 1);
			switch8.SetValue(Grid.ColumnProperty, 1);
			bindingExtension52.Mode = 1;
			bindingExtension52.Path = "UseHoursePower";
			bindingExtension52.TypedBinding = new TypedBinding<SharedSettings, bool>(delegate(SharedSettings A_0)
			{
				if (A_0 != null)
				{
					return new ValueTuple<bool, bool>(A_0.UseHoursePower, true);
				}
				return default(ValueTuple<bool, bool>);
			}, delegate(SharedSettings A_0, bool A_1)
			{
				if (A_0 != null)
				{
					A_0.UseHoursePower = A_1;
					return;
				}
			}, new Tuple<Func<SharedSettings, object>, string>[]
			{
				new Tuple<Func<SharedSettings, object>, string>((SharedSettings A_0) => A_0, "UseHoursePower")
			});
			BindingBase bindingBase52 = bindingExtension52.ProvideValue(null);
			switch8.SetBinding(Switch.IsToggledProperty, bindingBase52);
			grid11.Children.Add(switch8);
			stackLayout4.Children.Add(grid11);
			grid12.SetValue(View.MarginProperty, new Thickness(0.0, 0.0, 0.0, 10.0));
			grid12.SetValue(Grid.RowSpacingProperty, 0.0);
			columnDefinition23.SetValue(ColumnDefinition.WidthProperty, new GridLengthTypeConverter().ConvertFromInvariantString("*"));
			grid12.GetValue(Grid.ColumnDefinitionsProperty).Add(columnDefinition23);
			columnDefinition24.SetValue(ColumnDefinition.WidthProperty, new GridLengthTypeConverter().ConvertFromInvariantString("auto"));
			grid12.GetValue(Grid.ColumnDefinitionsProperty).Add(columnDefinition24);
			rowDefinition19.SetValue(RowDefinition.HeightProperty, new GridLengthTypeConverter().ConvertFromInvariantString("auto"));
			grid12.GetValue(Grid.RowDefinitionsProperty).Add(rowDefinition19);
			rowDefinition20.SetValue(RowDefinition.HeightProperty, new GridLengthTypeConverter().ConvertFromInvariantString("auto"));
			grid12.GetValue(Grid.RowDefinitionsProperty).Add(rowDefinition20);
			label39.SetValue(Grid.RowProperty, 1);
			label39.SetValue(Grid.ColumnProperty, 0);
			label39.SetValue(Label.FontAttributesProperty, new FontAttributesConverter().ConvertFromInvariantString("Italic"));
			bindingExtension53.Mode = 2;
			bindingExtension53.Path = "UseNmForTorque";
			bindingExtension53.TypedBinding = new TypedBinding<SharedSettings, bool>(delegate(SharedSettings A_0)
			{
				if (A_0 != null)
				{
					return new ValueTuple<bool, bool>(A_0.UseNmForTorque, true);
				}
				return default(ValueTuple<bool, bool>);
			}, null, new Tuple<Func<SharedSettings, object>, string>[]
			{
				new Tuple<Func<SharedSettings, object>, string>((SharedSettings A_0) => A_0, "UseNmForTorque")
			});
			BindingBase bindingBase53 = bindingExtension53.ProvideValue(null);
			label39.SetBinding(VisualElement.IsVisibleProperty, bindingBase53);
			label39.SetValue(Label.TextProperty, "Nm");
			label39.SetValue(View.VerticalOptionsProperty, LayoutOptions.Center);
			grid12.Children.Add(label39);
			label40.SetValue(Grid.RowProperty, 1);
			label40.SetValue(Grid.ColumnProperty, 0);
			label40.SetValue(Label.FontAttributesProperty, new FontAttributesConverter().ConvertFromInvariantString("Italic"));
			bindingExtension54.Mode = 2;
			staticResourceExtension16.Key = "BoolToNegativeConverter";
			IMarkupExtension markupExtension70 = staticResourceExtension16;
			XamlServiceProvider xamlServiceProvider70 = new XamlServiceProvider();
			Type typeFromHandle139 = typeof(IProvideValueTarget);
			object[] array70 = new object[0 + 6];
			array70[0] = bindingExtension54;
			array70[1] = label40;
			array70[2] = grid12;
			array70[3] = stackLayout4;
			array70[4] = scrollView;
			array70[5] = this;
			object obj132;
			xamlServiceProvider70.Add(typeFromHandle139, obj132 = new SimpleValueTargetProvider(array70, typeof(BindingExtension).GetRuntimeProperty("Converter"), nameScope));
			xamlServiceProvider70.Add(typeof(IReferenceProvider), obj132);
			Type typeFromHandle140 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver70 = new XmlNamespaceResolver();
			xmlNamespaceResolver70.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver70.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver70.Add("dash", "clr-namespace:CarScannerXamarinForms.Dashboard");
			xmlNamespaceResolver70.Add("input", "clr-namespace:Plugin.InputKit.Shared.Controls;assembly=Plugin.InputKit");
			xmlNamespaceResolver70.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver70.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver70.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver70.Add("syncfusion", "clr-namespace:Syncfusion.XForms.Buttons;assembly=Syncfusion.Buttons.XForms");
			xmlNamespaceResolver70.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider70.Add(typeFromHandle140, new XamlTypeResolver(xmlNamespaceResolver70, typeof(SettingsInterfacePage).GetTypeInfo().Assembly));
			xamlServiceProvider70.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(608, 25)));
			object obj133 = markupExtension70.ProvideValue(xamlServiceProvider70);
			bindingExtension54.Converter = obj133;
			bindingExtension54.Path = "UseNmForTorque";
			bindingExtension54.TypedBinding = new TypedBinding<SharedSettings, bool>(delegate(SharedSettings A_0)
			{
				if (A_0 != null)
				{
					return new ValueTuple<bool, bool>(A_0.UseNmForTorque, true);
				}
				return default(ValueTuple<bool, bool>);
			}, null, new Tuple<Func<SharedSettings, object>, string>[]
			{
				new Tuple<Func<SharedSettings, object>, string>((SharedSettings A_0) => A_0, "UseNmForTorque")
			});
			BindingBase bindingBase54 = bindingExtension54.ProvideValue(null);
			label40.SetBinding(VisualElement.IsVisibleProperty, bindingBase54);
			label40.SetValue(Label.TextProperty, "ft*lbs");
			label40.SetValue(View.VerticalOptionsProperty, LayoutOptions.Center);
			grid12.Children.Add(label40);
			label41.SetValue(Grid.RowProperty, 0);
			label41.SetValue(Grid.ColumnProperty, 0);
			translate48.Text = "Settings_Control_ToggleTorqueUnits.Header";
			IMarkupExtension markupExtension71 = translate48;
			XamlServiceProvider xamlServiceProvider71 = new XamlServiceProvider();
			Type typeFromHandle141 = typeof(IProvideValueTarget);
			object[] array71 = new object[0 + 5];
			array71[0] = label41;
			array71[1] = grid12;
			array71[2] = stackLayout4;
			array71[3] = scrollView;
			array71[4] = this;
			object obj134;
			xamlServiceProvider71.Add(typeFromHandle141, obj134 = new SimpleValueTargetProvider(array71, Label.TextProperty, nameScope));
			xamlServiceProvider71.Add(typeof(IReferenceProvider), obj134);
			Type typeFromHandle142 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver71 = new XmlNamespaceResolver();
			xmlNamespaceResolver71.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver71.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver71.Add("dash", "clr-namespace:CarScannerXamarinForms.Dashboard");
			xmlNamespaceResolver71.Add("input", "clr-namespace:Plugin.InputKit.Shared.Controls;assembly=Plugin.InputKit");
			xmlNamespaceResolver71.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver71.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver71.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver71.Add("syncfusion", "clr-namespace:Syncfusion.XForms.Buttons;assembly=Syncfusion.Buttons.XForms");
			xmlNamespaceResolver71.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider71.Add(typeFromHandle142, new XamlTypeResolver(xmlNamespaceResolver71, typeof(SettingsInterfacePage).GetTypeInfo().Assembly));
			xamlServiceProvider71.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(615, 25)));
			object obj135 = markupExtension71.ProvideValue(xamlServiceProvider71);
			label41.Text = obj135;
			grid12.Children.Add(label41);
			switch9.SetValue(Grid.RowProperty, 1);
			switch9.SetValue(Grid.ColumnProperty, 1);
			bindingExtension55.Mode = 1;
			bindingExtension55.Path = "UseNmForTorque";
			bindingExtension55.TypedBinding = new TypedBinding<SharedSettings, bool>(delegate(SharedSettings A_0)
			{
				if (A_0 != null)
				{
					return new ValueTuple<bool, bool>(A_0.UseNmForTorque, true);
				}
				return default(ValueTuple<bool, bool>);
			}, delegate(SharedSettings A_0, bool A_1)
			{
				if (A_0 != null)
				{
					A_0.UseNmForTorque = A_1;
					return;
				}
			}, new Tuple<Func<SharedSettings, object>, string>[]
			{
				new Tuple<Func<SharedSettings, object>, string>((SharedSettings A_0) => A_0, "UseNmForTorque")
			});
			BindingBase bindingBase55 = bindingExtension55.ProvideValue(null);
			switch9.SetBinding(Switch.IsToggledProperty, bindingBase55);
			grid12.Children.Add(switch9);
			stackLayout4.Children.Add(grid12);
			scrollView.Content = stackLayout4;
			this.SetValue(ContentPage.ContentProperty, scrollView);
		}

		// Token: 0x06001940 RID: 6464 RVA: 0x0010A8C0 File Offset: 0x00108AC0
		private void __InitComponentRuntime()
		{
			Extensions.LoadFromXaml<SettingsInterfacePage>(this, typeof(SettingsInterfacePage));
			this.panelLanguage = NameScopeExtensions.FindByName<StackLayout>(this, "panelLanguage");
			this.btnApplyLangugage = NameScopeExtensions.FindByName<Button>(this, "btnApplyLangugage");
			this.btnFontSizePlus = NameScopeExtensions.FindByName<Button>(this, "btnFontSizePlus");
			this.btnFontSizeMinus = NameScopeExtensions.FindByName<Button>(this, "btnFontSizeMinus");
			this.interfaceThemeGroup = NameScopeExtensions.FindByName<SfRadioGroup>(this, "interfaceThemeGroup");
			this.btnLight = NameScopeExtensions.FindByName<SfRadioButton>(this, "btnLight");
			this.btnDark = NameScopeExtensions.FindByName<SfRadioButton>(this, "btnDark");
			this.btnMainScreenConfiguration = NameScopeExtensions.FindByName<Button>(this, "btnMainScreenConfiguration");
			this.chartStylePicker = NameScopeExtensions.FindByName<Picker>(this, "chartStylePicker");
		}

		// Token: 0x06001941 RID: 6465 RVA: 0x0010A978 File Offset: 0x00108B78
		[CompilerGenerated]
		private static ValueTuple<int, bool> <InitializeComponent>typedBindingsM__2539(SharedSettings A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<int, bool>(A_0.Language, true);
			}
			return default(ValueTuple<int, bool>);
		}

		// Token: 0x06001942 RID: 6466 RVA: 0x0010A9A8 File Offset: 0x00108BA8
		[CompilerGenerated]
		private static void <InitializeComponent>typedBindingsM__2540(SharedSettings A_0, int A_1)
		{
			if (A_0 != null)
			{
				A_0.Language = A_1;
				return;
			}
		}

		// Token: 0x06001943 RID: 6467 RVA: 0x0010A9C4 File Offset: 0x00108BC4
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__2541(SharedSettings A_0)
		{
			return A_0;
		}

		// Token: 0x06001944 RID: 6468 RVA: 0x0010A9D4 File Offset: 0x00108BD4
		[CompilerGenerated]
		private static ValueTuple<bool, bool> <InitializeComponent>typedBindingsM__2542(SharedSettings A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<bool, bool>(A_0.AutomaticallySwitchTheme, true);
			}
			return default(ValueTuple<bool, bool>);
		}

		// Token: 0x06001945 RID: 6469 RVA: 0x0010AA04 File Offset: 0x00108C04
		[CompilerGenerated]
		private static void <InitializeComponent>typedBindingsM__2543(SharedSettings A_0, bool A_1)
		{
			if (A_0 != null)
			{
				A_0.AutomaticallySwitchTheme = A_1;
				return;
			}
		}

		// Token: 0x06001946 RID: 6470 RVA: 0x0010AA20 File Offset: 0x00108C20
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__2544(SharedSettings A_0)
		{
			return A_0;
		}

		// Token: 0x06001947 RID: 6471 RVA: 0x0010AA30 File Offset: 0x00108C30
		[CompilerGenerated]
		private static ValueTuple<bool, bool> <InitializeComponent>typedBindingsM__2545(SharedSettings A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<bool, bool>(A_0.AutomaticallySwitchThemeAvailable, true);
			}
			return default(ValueTuple<bool, bool>);
		}

		// Token: 0x06001948 RID: 6472 RVA: 0x0010AA60 File Offset: 0x00108C60
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__2546(SharedSettings A_0)
		{
			return A_0;
		}

		// Token: 0x06001949 RID: 6473 RVA: 0x0010AA70 File Offset: 0x00108C70
		[CompilerGenerated]
		private static ValueTuple<int, bool> <InitializeComponent>typedBindingsM__2547(SharedSettings A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<int, bool>(A_0.FontSizePatch, true);
			}
			return default(ValueTuple<int, bool>);
		}

		// Token: 0x0600194A RID: 6474 RVA: 0x0010AAA0 File Offset: 0x00108CA0
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__2548(SharedSettings A_0)
		{
			return A_0;
		}

		// Token: 0x0600194B RID: 6475 RVA: 0x0010AAB0 File Offset: 0x00108CB0
		[CompilerGenerated]
		private static ValueTuple<bool, bool> <InitializeComponent>typedBindingsM__2549(SharedSettings A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<bool, bool>(A_0.AutomaticallySwitchTheme, true);
			}
			return default(ValueTuple<bool, bool>);
		}

		// Token: 0x0600194C RID: 6476 RVA: 0x0010AAE0 File Offset: 0x00108CE0
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__2550(SharedSettings A_0)
		{
			return A_0;
		}

		// Token: 0x0600194D RID: 6477 RVA: 0x0010AAF0 File Offset: 0x00108CF0
		[CompilerGenerated]
		private static ValueTuple<bool, bool> <InitializeComponent>typedBindingsM__2551(SharedSettings A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<bool, bool>(A_0.DarkMode, true);
			}
			return default(ValueTuple<bool, bool>);
		}

		// Token: 0x0600194E RID: 6478 RVA: 0x0010AB20 File Offset: 0x00108D20
		[CompilerGenerated]
		private static void <InitializeComponent>typedBindingsM__2552(SharedSettings A_0, bool A_1)
		{
			if (A_0 != null)
			{
				A_0.DarkMode = A_1;
				return;
			}
		}

		// Token: 0x0600194F RID: 6479 RVA: 0x0010AB3C File Offset: 0x00108D3C
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__2553(SharedSettings A_0)
		{
			return A_0;
		}

		// Token: 0x06001950 RID: 6480 RVA: 0x0010AB4C File Offset: 0x00108D4C
		[CompilerGenerated]
		private static ValueTuple<bool, bool> <InitializeComponent>typedBindingsM__2554(SharedSettings A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<bool, bool>(A_0.DarkMode, true);
			}
			return default(ValueTuple<bool, bool>);
		}

		// Token: 0x06001951 RID: 6481 RVA: 0x0010AB7C File Offset: 0x00108D7C
		[CompilerGenerated]
		private static void <InitializeComponent>typedBindingsM__2555(SharedSettings A_0, bool A_1)
		{
			if (A_0 != null)
			{
				A_0.DarkMode = A_1;
				return;
			}
		}

		// Token: 0x06001952 RID: 6482 RVA: 0x0010AB98 File Offset: 0x00108D98
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__2556(SharedSettings A_0)
		{
			return A_0;
		}

		// Token: 0x06001953 RID: 6483 RVA: 0x0010ABA8 File Offset: 0x00108DA8
		[CompilerGenerated]
		private static ValueTuple<bool, bool> <InitializeComponent>typedBindingsM__2557(SharedSettings A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<bool, bool>(A_0.LiveDataListPageUpdateOnlyVisible, true);
			}
			return default(ValueTuple<bool, bool>);
		}

		// Token: 0x06001954 RID: 6484 RVA: 0x0010ABD8 File Offset: 0x00108DD8
		[CompilerGenerated]
		private static void <InitializeComponent>typedBindingsM__2558(SharedSettings A_0, bool A_1)
		{
			if (A_0 != null)
			{
				A_0.LiveDataListPageUpdateOnlyVisible = A_1;
				return;
			}
		}

		// Token: 0x06001955 RID: 6485 RVA: 0x0010ABF4 File Offset: 0x00108DF4
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__2559(SharedSettings A_0)
		{
			return A_0;
		}

		// Token: 0x06001956 RID: 6486 RVA: 0x0010AC04 File Offset: 0x00108E04
		[CompilerGenerated]
		private static ValueTuple<bool, bool> <InitializeComponent>typedBindingsM__2560(SharedSettings A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<bool, bool>(A_0.AndroidRecolorNavBar, true);
			}
			return default(ValueTuple<bool, bool>);
		}

		// Token: 0x06001957 RID: 6487 RVA: 0x0010AC34 File Offset: 0x00108E34
		[CompilerGenerated]
		private static void <InitializeComponent>typedBindingsM__2561(SharedSettings A_0, bool A_1)
		{
			if (A_0 != null)
			{
				A_0.AndroidRecolorNavBar = A_1;
				return;
			}
		}

		// Token: 0x06001958 RID: 6488 RVA: 0x0010AC50 File Offset: 0x00108E50
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__2562(SharedSettings A_0)
		{
			return A_0;
		}

		// Token: 0x06001959 RID: 6489 RVA: 0x0010AC60 File Offset: 0x00108E60
		[CompilerGenerated]
		private static ValueTuple<int, bool> <InitializeComponent>typedBindingsM__2563(SharedSettings A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<int, bool>(A_0.ChartsView, true);
			}
			return default(ValueTuple<int, bool>);
		}

		// Token: 0x0600195A RID: 6490 RVA: 0x0010AC90 File Offset: 0x00108E90
		[CompilerGenerated]
		private static void <InitializeComponent>typedBindingsM__2564(SharedSettings A_0, int A_1)
		{
			if (A_0 != null)
			{
				A_0.ChartsView = A_1;
				return;
			}
		}

		// Token: 0x0600195B RID: 6491 RVA: 0x0010ACAC File Offset: 0x00108EAC
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__2565(SharedSettings A_0)
		{
			return A_0;
		}

		// Token: 0x0600195C RID: 6492 RVA: 0x0010ACBC File Offset: 0x00108EBC
		[CompilerGenerated]
		private static ValueTuple<int, bool> <InitializeComponent>typedBindingsM__2566(SharedSettings A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<int, bool>(A_0.LiveDataShowTime, true);
			}
			return default(ValueTuple<int, bool>);
		}

		// Token: 0x0600195D RID: 6493 RVA: 0x0010ACEC File Offset: 0x00108EEC
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__2567(SharedSettings A_0)
		{
			return A_0;
		}

		// Token: 0x0600195E RID: 6494 RVA: 0x0010ACFC File Offset: 0x00108EFC
		[CompilerGenerated]
		private static ValueTuple<int, bool> <InitializeComponent>typedBindingsM__2568(SharedSettings A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<int, bool>(A_0.LiveDataShowTime, true);
			}
			return default(ValueTuple<int, bool>);
		}

		// Token: 0x0600195F RID: 6495 RVA: 0x0010AD2C File Offset: 0x00108F2C
		[CompilerGenerated]
		private static void <InitializeComponent>typedBindingsM__2569(SharedSettings A_0, int A_1)
		{
			if (A_0 != null)
			{
				A_0.LiveDataShowTime = A_1;
				return;
			}
		}

		// Token: 0x06001960 RID: 6496 RVA: 0x0010AD48 File Offset: 0x00108F48
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__2570(SharedSettings A_0)
		{
			return A_0;
		}

		// Token: 0x06001961 RID: 6497 RVA: 0x0010AD58 File Offset: 0x00108F58
		[CompilerGenerated]
		private static ValueTuple<ChartItemTypes, bool> <InitializeComponent>typedBindingsM__2571(SharedSettings A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<ChartItemTypes, bool>(A_0.ChartDisplayStyle, true);
			}
			return default(ValueTuple<ChartItemTypes, bool>);
		}

		// Token: 0x06001962 RID: 6498 RVA: 0x0010AD88 File Offset: 0x00108F88
		[CompilerGenerated]
		private static void <InitializeComponent>typedBindingsM__2572(SharedSettings A_0, ChartItemTypes A_1)
		{
			if (A_0 != null)
			{
				A_0.ChartDisplayStyle = A_1;
				return;
			}
		}

		// Token: 0x06001963 RID: 6499 RVA: 0x0010ADA4 File Offset: 0x00108FA4
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__2573(SharedSettings A_0)
		{
			return A_0;
		}

		// Token: 0x06001964 RID: 6500 RVA: 0x0010ADB4 File Offset: 0x00108FB4
		[CompilerGenerated]
		private static ValueTuple<bool, bool> <InitializeComponent>typedBindingsM__2574(SharedSettings A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<bool, bool>(A_0.SetChartMinMaxOnlyVisibleArea, true);
			}
			return default(ValueTuple<bool, bool>);
		}

		// Token: 0x06001965 RID: 6501 RVA: 0x0010ADE4 File Offset: 0x00108FE4
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__2575(SharedSettings A_0)
		{
			return A_0;
		}

		// Token: 0x06001966 RID: 6502 RVA: 0x0010ADF4 File Offset: 0x00108FF4
		[CompilerGenerated]
		private static ValueTuple<bool, bool> <InitializeComponent>typedBindingsM__2576(SharedSettings A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<bool, bool>(A_0.SetChartMinMaxOnlyVisibleArea, true);
			}
			return default(ValueTuple<bool, bool>);
		}

		// Token: 0x06001967 RID: 6503 RVA: 0x0010AE24 File Offset: 0x00109024
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__2577(SharedSettings A_0)
		{
			return A_0;
		}

		// Token: 0x06001968 RID: 6504 RVA: 0x0010AE34 File Offset: 0x00109034
		[CompilerGenerated]
		private static ValueTuple<bool, bool> <InitializeComponent>typedBindingsM__2578(SharedSettings A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<bool, bool>(A_0.SetChartMinMaxOnlyVisibleArea, true);
			}
			return default(ValueTuple<bool, bool>);
		}

		// Token: 0x06001969 RID: 6505 RVA: 0x0010AE64 File Offset: 0x00109064
		[CompilerGenerated]
		private static void <InitializeComponent>typedBindingsM__2579(SharedSettings A_0, bool A_1)
		{
			if (A_0 != null)
			{
				A_0.SetChartMinMaxOnlyVisibleArea = A_1;
				return;
			}
		}

		// Token: 0x0600196A RID: 6506 RVA: 0x0010AE80 File Offset: 0x00109080
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__2580(SharedSettings A_0)
		{
			return A_0;
		}

		// Token: 0x0600196B RID: 6507 RVA: 0x0010AE90 File Offset: 0x00109090
		[CompilerGenerated]
		private static ValueTuple<bool, bool> <InitializeComponent>typedBindingsM__2581(SharedSettings A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<bool, bool>(A_0.ChartShowAverageValue, true);
			}
			return default(ValueTuple<bool, bool>);
		}

		// Token: 0x0600196C RID: 6508 RVA: 0x0010AEC0 File Offset: 0x001090C0
		[CompilerGenerated]
		private static void <InitializeComponent>typedBindingsM__2582(SharedSettings A_0, bool A_1)
		{
			if (A_0 != null)
			{
				A_0.ChartShowAverageValue = A_1;
				return;
			}
		}

		// Token: 0x0600196D RID: 6509 RVA: 0x0010AEDC File Offset: 0x001090DC
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__2583(SharedSettings A_0)
		{
			return A_0;
		}

		// Token: 0x0600196E RID: 6510 RVA: 0x0010AEEC File Offset: 0x001090EC
		[CompilerGenerated]
		private static ValueTuple<bool, bool> <InitializeComponent>typedBindingsM__2584(SharedSettings A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<bool, bool>(A_0.ShowMinMaxValues, true);
			}
			return default(ValueTuple<bool, bool>);
		}

		// Token: 0x0600196F RID: 6511 RVA: 0x0010AF1C File Offset: 0x0010911C
		[CompilerGenerated]
		private static void <InitializeComponent>typedBindingsM__2585(SharedSettings A_0, bool A_1)
		{
			if (A_0 != null)
			{
				A_0.ShowMinMaxValues = A_1;
				return;
			}
		}

		// Token: 0x06001970 RID: 6512 RVA: 0x0010AF38 File Offset: 0x00109138
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__2586(SharedSettings A_0)
		{
			return A_0;
		}

		// Token: 0x06001971 RID: 6513 RVA: 0x0010AF48 File Offset: 0x00109148
		[CompilerGenerated]
		private static ValueTuple<bool, bool> <InitializeComponent>typedBindingsM__2587(SharedSettings A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<bool, bool>(A_0.MultiChartPauseHidden, true);
			}
			return default(ValueTuple<bool, bool>);
		}

		// Token: 0x06001972 RID: 6514 RVA: 0x0010AF78 File Offset: 0x00109178
		[CompilerGenerated]
		private static void <InitializeComponent>typedBindingsM__2588(SharedSettings A_0, bool A_1)
		{
			if (A_0 != null)
			{
				A_0.MultiChartPauseHidden = A_1;
				return;
			}
		}

		// Token: 0x06001973 RID: 6515 RVA: 0x0010AF94 File Offset: 0x00109194
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__2589(SharedSettings A_0)
		{
			return A_0;
		}

		// Token: 0x06001974 RID: 6516 RVA: 0x0010AFA4 File Offset: 0x001091A4
		[CompilerGenerated]
		private static ValueTuple<bool, bool> <InitializeComponent>typedBindingsM__2590(SharedSettings A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<bool, bool>(A_0.AndroidChartRenderingSafeMode, true);
			}
			return default(ValueTuple<bool, bool>);
		}

		// Token: 0x06001975 RID: 6517 RVA: 0x0010AFD4 File Offset: 0x001091D4
		[CompilerGenerated]
		private static void <InitializeComponent>typedBindingsM__2591(SharedSettings A_0, bool A_1)
		{
			if (A_0 != null)
			{
				A_0.AndroidChartRenderingSafeMode = A_1;
				return;
			}
		}

		// Token: 0x06001976 RID: 6518 RVA: 0x0010AFF0 File Offset: 0x001091F0
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__2592(SharedSettings A_0)
		{
			return A_0;
		}

		// Token: 0x06001977 RID: 6519 RVA: 0x0010B000 File Offset: 0x00109200
		[CompilerGenerated]
		private static ValueTuple<bool, bool> <InitializeComponent>typedBindingsM__2593(SharedSettings A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<bool, bool>(A_0.ShowPing, true);
			}
			return default(ValueTuple<bool, bool>);
		}

		// Token: 0x06001978 RID: 6520 RVA: 0x0010B030 File Offset: 0x00109230
		[CompilerGenerated]
		private static void <InitializeComponent>typedBindingsM__2594(SharedSettings A_0, bool A_1)
		{
			if (A_0 != null)
			{
				A_0.ShowPing = A_1;
				return;
			}
		}

		// Token: 0x06001979 RID: 6521 RVA: 0x0010B04C File Offset: 0x0010924C
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__2595(SharedSettings A_0)
		{
			return A_0;
		}

		// Token: 0x0600197A RID: 6522 RVA: 0x0010B05C File Offset: 0x0010925C
		[CompilerGenerated]
		private static ValueTuple<int, bool> <InitializeComponent>typedBindingsM__2596(SharedSettings A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<int, bool>(A_0.DataRecordLineSplitterTime, true);
			}
			return default(ValueTuple<int, bool>);
		}

		// Token: 0x0600197B RID: 6523 RVA: 0x0010B08C File Offset: 0x0010928C
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__2597(SharedSettings A_0)
		{
			return A_0;
		}

		// Token: 0x0600197C RID: 6524 RVA: 0x0010B09C File Offset: 0x0010929C
		[CompilerGenerated]
		private static ValueTuple<int, bool> <InitializeComponent>typedBindingsM__2598(SharedSettings A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<int, bool>(A_0.DataRecordLineSplitterTime, true);
			}
			return default(ValueTuple<int, bool>);
		}

		// Token: 0x0600197D RID: 6525 RVA: 0x0010B0CC File Offset: 0x001092CC
		[CompilerGenerated]
		private static void <InitializeComponent>typedBindingsM__2599(SharedSettings A_0, int A_1)
		{
			if (A_0 != null)
			{
				A_0.DataRecordLineSplitterTime = A_1;
				return;
			}
		}

		// Token: 0x0600197E RID: 6526 RVA: 0x0010B0E8 File Offset: 0x001092E8
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__2600(SharedSettings A_0)
		{
			return A_0;
		}

		// Token: 0x0600197F RID: 6527 RVA: 0x0010B0F8 File Offset: 0x001092F8
		[CompilerGenerated]
		private static ValueTuple<bool, bool> <InitializeComponent>typedBindingsM__2601(SharedSettings A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<bool, bool>(A_0.Use_km, true);
			}
			return default(ValueTuple<bool, bool>);
		}

		// Token: 0x06001980 RID: 6528 RVA: 0x0010B128 File Offset: 0x00109328
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__2602(SharedSettings A_0)
		{
			return A_0;
		}

		// Token: 0x06001981 RID: 6529 RVA: 0x0010B138 File Offset: 0x00109338
		[CompilerGenerated]
		private static ValueTuple<bool, bool> <InitializeComponent>typedBindingsM__2603(SharedSettings A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<bool, bool>(A_0.Use_km, true);
			}
			return default(ValueTuple<bool, bool>);
		}

		// Token: 0x06001982 RID: 6530 RVA: 0x0010B168 File Offset: 0x00109368
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__2604(SharedSettings A_0)
		{
			return A_0;
		}

		// Token: 0x06001983 RID: 6531 RVA: 0x0010B178 File Offset: 0x00109378
		[CompilerGenerated]
		private static ValueTuple<bool, bool> <InitializeComponent>typedBindingsM__2605(SharedSettings A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<bool, bool>(A_0.Use_km, true);
			}
			return default(ValueTuple<bool, bool>);
		}

		// Token: 0x06001984 RID: 6532 RVA: 0x0010B1A8 File Offset: 0x001093A8
		[CompilerGenerated]
		private static void <InitializeComponent>typedBindingsM__2606(SharedSettings A_0, bool A_1)
		{
			if (A_0 != null)
			{
				A_0.Use_km = A_1;
				return;
			}
		}

		// Token: 0x06001985 RID: 6533 RVA: 0x0010B1C4 File Offset: 0x001093C4
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__2607(SharedSettings A_0)
		{
			return A_0;
		}

		// Token: 0x06001986 RID: 6534 RVA: 0x0010B1D4 File Offset: 0x001093D4
		[CompilerGenerated]
		private static ValueTuple<FuelConsumptionUnits, bool> <InitializeComponent>typedBindingsM__2608(SharedSettings A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<FuelConsumptionUnits, bool>(A_0.FuelConsumptionUnit, true);
			}
			return default(ValueTuple<FuelConsumptionUnits, bool>);
		}

		// Token: 0x06001987 RID: 6535 RVA: 0x0010B204 File Offset: 0x00109404
		[CompilerGenerated]
		private static void <InitializeComponent>typedBindingsM__2609(SharedSettings A_0, FuelConsumptionUnits A_1)
		{
			if (A_0 != null)
			{
				A_0.FuelConsumptionUnit = A_1;
				return;
			}
		}

		// Token: 0x06001988 RID: 6536 RVA: 0x0010B220 File Offset: 0x00109420
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__2610(SharedSettings A_0)
		{
			return A_0;
		}

		// Token: 0x06001989 RID: 6537 RVA: 0x0010B230 File Offset: 0x00109430
		[CompilerGenerated]
		private static ValueTuple<bool, bool> <InitializeComponent>typedBindingsM__2611(SharedSettings A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<bool, bool>(A_0.UseLitersForVolume, true);
			}
			return default(ValueTuple<bool, bool>);
		}

		// Token: 0x0600198A RID: 6538 RVA: 0x0010B260 File Offset: 0x00109460
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__2612(SharedSettings A_0)
		{
			return A_0;
		}

		// Token: 0x0600198B RID: 6539 RVA: 0x0010B270 File Offset: 0x00109470
		[CompilerGenerated]
		private static ValueTuple<bool, bool> <InitializeComponent>typedBindingsM__2613(SharedSettings A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<bool, bool>(A_0.UseLitersForVolume, true);
			}
			return default(ValueTuple<bool, bool>);
		}

		// Token: 0x0600198C RID: 6540 RVA: 0x0010B2A0 File Offset: 0x001094A0
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__2614(SharedSettings A_0)
		{
			return A_0;
		}

		// Token: 0x0600198D RID: 6541 RVA: 0x0010B2B0 File Offset: 0x001094B0
		[CompilerGenerated]
		private static ValueTuple<bool, bool> <InitializeComponent>typedBindingsM__2615(SharedSettings A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<bool, bool>(A_0.UseLitersForVolume, true);
			}
			return default(ValueTuple<bool, bool>);
		}

		// Token: 0x0600198E RID: 6542 RVA: 0x0010B2E0 File Offset: 0x001094E0
		[CompilerGenerated]
		private static void <InitializeComponent>typedBindingsM__2616(SharedSettings A_0, bool A_1)
		{
			if (A_0 != null)
			{
				A_0.UseLitersForVolume = A_1;
				return;
			}
		}

		// Token: 0x0600198F RID: 6543 RVA: 0x0010B2FC File Offset: 0x001094FC
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__2617(SharedSettings A_0)
		{
			return A_0;
		}

		// Token: 0x06001990 RID: 6544 RVA: 0x0010B30C File Offset: 0x0010950C
		[CompilerGenerated]
		private static ValueTuple<bool, bool> <InitializeComponent>typedBindingsM__2618(SharedSettings A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<bool, bool>(A_0.ShowUSGallonSelector, true);
			}
			return default(ValueTuple<bool, bool>);
		}

		// Token: 0x06001991 RID: 6545 RVA: 0x0010B33C File Offset: 0x0010953C
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__2620(SharedSettings A_0)
		{
			return A_0;
		}

		// Token: 0x06001992 RID: 6546 RVA: 0x0010B34C File Offset: 0x0010954C
		[CompilerGenerated]
		private static ValueTuple<bool, bool> <InitializeComponent>typedBindingsM__2621(SharedSettings A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<bool, bool>(A_0.UseUSGallon, true);
			}
			return default(ValueTuple<bool, bool>);
		}

		// Token: 0x06001993 RID: 6547 RVA: 0x0010B37C File Offset: 0x0010957C
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__2622(SharedSettings A_0)
		{
			return A_0;
		}

		// Token: 0x06001994 RID: 6548 RVA: 0x0010B38C File Offset: 0x0010958C
		[CompilerGenerated]
		private static ValueTuple<bool, bool> <InitializeComponent>typedBindingsM__2623(SharedSettings A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<bool, bool>(A_0.UseUSGallon, true);
			}
			return default(ValueTuple<bool, bool>);
		}

		// Token: 0x06001995 RID: 6549 RVA: 0x0010B3BC File Offset: 0x001095BC
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__2624(SharedSettings A_0)
		{
			return A_0;
		}

		// Token: 0x06001996 RID: 6550 RVA: 0x0010B3CC File Offset: 0x001095CC
		[CompilerGenerated]
		private static ValueTuple<bool, bool> <InitializeComponent>typedBindingsM__2625(SharedSettings A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<bool, bool>(A_0.UseUSGallon, true);
			}
			return default(ValueTuple<bool, bool>);
		}

		// Token: 0x06001997 RID: 6551 RVA: 0x0010B3FC File Offset: 0x001095FC
		[CompilerGenerated]
		private static void <InitializeComponent>typedBindingsM__2626(SharedSettings A_0, bool A_1)
		{
			if (A_0 != null)
			{
				A_0.UseUSGallon = A_1;
				return;
			}
		}

		// Token: 0x06001998 RID: 6552 RVA: 0x0010B418 File Offset: 0x00109618
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__2627(SharedSettings A_0)
		{
			return A_0;
		}

		// Token: 0x06001999 RID: 6553 RVA: 0x0010B428 File Offset: 0x00109628
		[CompilerGenerated]
		private static ValueTuple<bool, bool> <InitializeComponent>typedBindingsM__2628(SharedSettings A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<bool, bool>(A_0.Pressure_use_kpa, true);
			}
			return default(ValueTuple<bool, bool>);
		}

		// Token: 0x0600199A RID: 6554 RVA: 0x0010B458 File Offset: 0x00109658
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__2629(SharedSettings A_0)
		{
			return A_0;
		}

		// Token: 0x0600199B RID: 6555 RVA: 0x0010B468 File Offset: 0x00109668
		[CompilerGenerated]
		private static ValueTuple<bool, bool> <InitializeComponent>typedBindingsM__2630(SharedSettings A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<bool, bool>(A_0.Pressure_use_kpa, true);
			}
			return default(ValueTuple<bool, bool>);
		}

		// Token: 0x0600199C RID: 6556 RVA: 0x0010B498 File Offset: 0x00109698
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__2631(SharedSettings A_0)
		{
			return A_0;
		}

		// Token: 0x0600199D RID: 6557 RVA: 0x0010B4A8 File Offset: 0x001096A8
		[CompilerGenerated]
		private static ValueTuple<bool, bool> <InitializeComponent>typedBindingsM__2632(SharedSettings A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<bool, bool>(A_0.Pressure_use_kpa, true);
			}
			return default(ValueTuple<bool, bool>);
		}

		// Token: 0x0600199E RID: 6558 RVA: 0x0010B4D8 File Offset: 0x001096D8
		[CompilerGenerated]
		private static void <InitializeComponent>typedBindingsM__2633(SharedSettings A_0, bool A_1)
		{
			if (A_0 != null)
			{
				A_0.Pressure_use_kpa = A_1;
				return;
			}
		}

		// Token: 0x0600199F RID: 6559 RVA: 0x0010B4F4 File Offset: 0x001096F4
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__2634(SharedSettings A_0)
		{
			return A_0;
		}

		// Token: 0x060019A0 RID: 6560 RVA: 0x0010B504 File Offset: 0x00109704
		[CompilerGenerated]
		private static ValueTuple<bool, bool> <InitializeComponent>typedBindingsM__2635(SharedSettings A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<bool, bool>(A_0.Flow_use_grams_sec, true);
			}
			return default(ValueTuple<bool, bool>);
		}

		// Token: 0x060019A1 RID: 6561 RVA: 0x0010B534 File Offset: 0x00109734
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__2636(SharedSettings A_0)
		{
			return A_0;
		}

		// Token: 0x060019A2 RID: 6562 RVA: 0x0010B544 File Offset: 0x00109744
		[CompilerGenerated]
		private static ValueTuple<bool, bool> <InitializeComponent>typedBindingsM__2637(SharedSettings A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<bool, bool>(A_0.Flow_use_grams_sec, true);
			}
			return default(ValueTuple<bool, bool>);
		}

		// Token: 0x060019A3 RID: 6563 RVA: 0x0010B574 File Offset: 0x00109774
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__2638(SharedSettings A_0)
		{
			return A_0;
		}

		// Token: 0x060019A4 RID: 6564 RVA: 0x0010B584 File Offset: 0x00109784
		[CompilerGenerated]
		private static ValueTuple<bool, bool> <InitializeComponent>typedBindingsM__2639(SharedSettings A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<bool, bool>(A_0.Flow_use_grams_sec, true);
			}
			return default(ValueTuple<bool, bool>);
		}

		// Token: 0x060019A5 RID: 6565 RVA: 0x0010B5B4 File Offset: 0x001097B4
		[CompilerGenerated]
		private static void <InitializeComponent>typedBindingsM__2640(SharedSettings A_0, bool A_1)
		{
			if (A_0 != null)
			{
				A_0.Flow_use_grams_sec = A_1;
				return;
			}
		}

		// Token: 0x060019A6 RID: 6566 RVA: 0x0010B5D0 File Offset: 0x001097D0
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__2641(SharedSettings A_0)
		{
			return A_0;
		}

		// Token: 0x060019A7 RID: 6567 RVA: 0x0010B5E0 File Offset: 0x001097E0
		[CompilerGenerated]
		private static ValueTuple<bool, bool> <InitializeComponent>typedBindingsM__2642(SharedSettings A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<bool, bool>(A_0.Use_celcium, true);
			}
			return default(ValueTuple<bool, bool>);
		}

		// Token: 0x060019A8 RID: 6568 RVA: 0x0010B610 File Offset: 0x00109810
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__2643(SharedSettings A_0)
		{
			return A_0;
		}

		// Token: 0x060019A9 RID: 6569 RVA: 0x0010B620 File Offset: 0x00109820
		[CompilerGenerated]
		private static ValueTuple<bool, bool> <InitializeComponent>typedBindingsM__2644(SharedSettings A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<bool, bool>(A_0.Use_celcium, true);
			}
			return default(ValueTuple<bool, bool>);
		}

		// Token: 0x060019AA RID: 6570 RVA: 0x0010B650 File Offset: 0x00109850
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__2645(SharedSettings A_0)
		{
			return A_0;
		}

		// Token: 0x060019AB RID: 6571 RVA: 0x0010B660 File Offset: 0x00109860
		[CompilerGenerated]
		private static ValueTuple<bool, bool> <InitializeComponent>typedBindingsM__2646(SharedSettings A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<bool, bool>(A_0.Use_celcium, true);
			}
			return default(ValueTuple<bool, bool>);
		}

		// Token: 0x060019AC RID: 6572 RVA: 0x0010B690 File Offset: 0x00109890
		[CompilerGenerated]
		private static void <InitializeComponent>typedBindingsM__2647(SharedSettings A_0, bool A_1)
		{
			if (A_0 != null)
			{
				A_0.Use_celcium = A_1;
				return;
			}
		}

		// Token: 0x060019AD RID: 6573 RVA: 0x0010B6AC File Offset: 0x001098AC
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__2648(SharedSettings A_0)
		{
			return A_0;
		}

		// Token: 0x060019AE RID: 6574 RVA: 0x0010B6BC File Offset: 0x001098BC
		[CompilerGenerated]
		private static ValueTuple<bool, bool> <InitializeComponent>typedBindingsM__2649(SharedSettings A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<bool, bool>(A_0.AccelerationUseG, true);
			}
			return default(ValueTuple<bool, bool>);
		}

		// Token: 0x060019AF RID: 6575 RVA: 0x0010B6EC File Offset: 0x001098EC
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__2650(SharedSettings A_0)
		{
			return A_0;
		}

		// Token: 0x060019B0 RID: 6576 RVA: 0x0010B6FC File Offset: 0x001098FC
		[CompilerGenerated]
		private static ValueTuple<bool, bool> <InitializeComponent>typedBindingsM__2651(SharedSettings A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<bool, bool>(A_0.AccelerationUseG, true);
			}
			return default(ValueTuple<bool, bool>);
		}

		// Token: 0x060019B1 RID: 6577 RVA: 0x0010B72C File Offset: 0x0010992C
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__2652(SharedSettings A_0)
		{
			return A_0;
		}

		// Token: 0x060019B2 RID: 6578 RVA: 0x0010B73C File Offset: 0x0010993C
		[CompilerGenerated]
		private static ValueTuple<bool, bool> <InitializeComponent>typedBindingsM__2653(SharedSettings A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<bool, bool>(A_0.AccelerationUseG, true);
			}
			return default(ValueTuple<bool, bool>);
		}

		// Token: 0x060019B3 RID: 6579 RVA: 0x0010B76C File Offset: 0x0010996C
		[CompilerGenerated]
		private static void <InitializeComponent>typedBindingsM__2654(SharedSettings A_0, bool A_1)
		{
			if (A_0 != null)
			{
				A_0.AccelerationUseG = A_1;
				return;
			}
		}

		// Token: 0x060019B4 RID: 6580 RVA: 0x0010B788 File Offset: 0x00109988
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__2655(SharedSettings A_0)
		{
			return A_0;
		}

		// Token: 0x060019B5 RID: 6581 RVA: 0x0010B798 File Offset: 0x00109998
		[CompilerGenerated]
		private static ValueTuple<bool, bool> <InitializeComponent>typedBindingsM__2656(SharedSettings A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<bool, bool>(A_0.UseHoursePower, true);
			}
			return default(ValueTuple<bool, bool>);
		}

		// Token: 0x060019B6 RID: 6582 RVA: 0x0010B7C8 File Offset: 0x001099C8
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__2657(SharedSettings A_0)
		{
			return A_0;
		}

		// Token: 0x060019B7 RID: 6583 RVA: 0x0010B7D8 File Offset: 0x001099D8
		[CompilerGenerated]
		private static ValueTuple<bool, bool> <InitializeComponent>typedBindingsM__2658(SharedSettings A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<bool, bool>(A_0.UseHoursePower, true);
			}
			return default(ValueTuple<bool, bool>);
		}

		// Token: 0x060019B8 RID: 6584 RVA: 0x0010B808 File Offset: 0x00109A08
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__2659(SharedSettings A_0)
		{
			return A_0;
		}

		// Token: 0x060019B9 RID: 6585 RVA: 0x0010B818 File Offset: 0x00109A18
		[CompilerGenerated]
		private static ValueTuple<bool, bool> <InitializeComponent>typedBindingsM__2660(SharedSettings A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<bool, bool>(A_0.UseHoursePower, true);
			}
			return default(ValueTuple<bool, bool>);
		}

		// Token: 0x060019BA RID: 6586 RVA: 0x0010B848 File Offset: 0x00109A48
		[CompilerGenerated]
		private static void <InitializeComponent>typedBindingsM__2661(SharedSettings A_0, bool A_1)
		{
			if (A_0 != null)
			{
				A_0.UseHoursePower = A_1;
				return;
			}
		}

		// Token: 0x060019BB RID: 6587 RVA: 0x0010B864 File Offset: 0x00109A64
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__2662(SharedSettings A_0)
		{
			return A_0;
		}

		// Token: 0x060019BC RID: 6588 RVA: 0x0010B874 File Offset: 0x00109A74
		[CompilerGenerated]
		private static ValueTuple<bool, bool> <InitializeComponent>typedBindingsM__2663(SharedSettings A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<bool, bool>(A_0.UseNmForTorque, true);
			}
			return default(ValueTuple<bool, bool>);
		}

		// Token: 0x060019BD RID: 6589 RVA: 0x0010B8A4 File Offset: 0x00109AA4
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__2664(SharedSettings A_0)
		{
			return A_0;
		}

		// Token: 0x060019BE RID: 6590 RVA: 0x0010B8B4 File Offset: 0x00109AB4
		[CompilerGenerated]
		private static ValueTuple<bool, bool> <InitializeComponent>typedBindingsM__2665(SharedSettings A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<bool, bool>(A_0.UseNmForTorque, true);
			}
			return default(ValueTuple<bool, bool>);
		}

		// Token: 0x060019BF RID: 6591 RVA: 0x0010B8E4 File Offset: 0x00109AE4
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__2666(SharedSettings A_0)
		{
			return A_0;
		}

		// Token: 0x060019C0 RID: 6592 RVA: 0x0010B8F4 File Offset: 0x00109AF4
		[CompilerGenerated]
		private static ValueTuple<bool, bool> <InitializeComponent>typedBindingsM__2667(SharedSettings A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<bool, bool>(A_0.UseNmForTorque, true);
			}
			return default(ValueTuple<bool, bool>);
		}

		// Token: 0x060019C1 RID: 6593 RVA: 0x0010B924 File Offset: 0x00109B24
		[CompilerGenerated]
		private static void <InitializeComponent>typedBindingsM__2668(SharedSettings A_0, bool A_1)
		{
			if (A_0 != null)
			{
				A_0.UseNmForTorque = A_1;
				return;
			}
		}

		// Token: 0x060019C2 RID: 6594 RVA: 0x0010B940 File Offset: 0x00109B40
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__2669(SharedSettings A_0)
		{
			return A_0;
		}

		// Token: 0x04000B01 RID: 2817
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private StackLayout panelLanguage;

		// Token: 0x04000B02 RID: 2818
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private Button btnApplyLangugage;

		// Token: 0x04000B03 RID: 2819
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private Button btnFontSizePlus;

		// Token: 0x04000B04 RID: 2820
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private Button btnFontSizeMinus;

		// Token: 0x04000B05 RID: 2821
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private SfRadioGroup interfaceThemeGroup;

		// Token: 0x04000B06 RID: 2822
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private SfRadioButton btnLight;

		// Token: 0x04000B07 RID: 2823
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private SfRadioButton btnDark;

		// Token: 0x04000B08 RID: 2824
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private Button btnMainScreenConfiguration;

		// Token: 0x04000B09 RID: 2825
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private Picker chartStylePicker;

		// Token: 0x020001E1 RID: 481
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <btnApplyDashboardTheme_Clicked>d__4 : IAsyncStateMachine
		{
			// Token: 0x060019C3 RID: 6595 RVA: 0x0010B950 File Offset: 0x00109B50
			void IAsyncStateMachine.MoveNext()
			{
				int num = this.<>1__state;
				SettingsInterfacePage settingsInterfacePage = this;
				try
				{
					DashboardListViewModel.Current.LoadDashboardFromSettings();
					IEnumerator<DashboardPage> enumerator = DashboardListViewModel.Current.Pages.GetEnumerator();
					try
					{
						while (enumerator.MoveNext())
						{
							DashboardPage dashboardPage = enumerator.Current;
							IEnumerator<DashboardItem> enumerator2 = dashboardPage.Items.GetEnumerator();
							try
							{
								while (enumerator2.MoveNext())
								{
									DashboardItem dashboardItem = enumerator2.Current;
									try
									{
										DashboardItem.ApplyThemeToItem(dashboardItem);
										dashboardItem.SelectAndAddControl();
									}
									catch (Exception ex)
									{
										ObjectDisposedException ex2 = ex as ObjectDisposedException;
									}
								}
							}
							finally
							{
								if (num < 0 && enumerator2 != null)
								{
									enumerator2.Dispose();
								}
							}
						}
					}
					finally
					{
						if (num < 0 && enumerator != null)
						{
							enumerator.Dispose();
						}
					}
					DashboardListViewModel.Current.SaveDashboardToSettings();
					settingsInterfacePage.DisplayAlert(Translate.GetString("ios_DashboardThemeApplyed"), "", "OK");
				}
				catch (Exception ex3)
				{
					this.<>1__state = -2;
					this.<>t__builder.SetException(ex3);
					return;
				}
				this.<>1__state = -2;
				this.<>t__builder.SetResult();
			}

			// Token: 0x060019C4 RID: 6596 RVA: 0x0010BA74 File Offset: 0x00109C74
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x04000B0A RID: 2826
			public int <>1__state;

			// Token: 0x04000B0B RID: 2827
			public AsyncVoidMethodBuilder <>t__builder;

			// Token: 0x04000B0C RID: 2828
			public SettingsInterfacePage <>4__this;
		}

		// Token: 0x020001E2 RID: 482
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <btnApplyLanguage_Clicked>d__1 : IAsyncStateMachine
		{
			// Token: 0x060019C5 RID: 6597 RVA: 0x0010BA84 File Offset: 0x00109C84
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				try
				{
					TaskAwaiter taskAwaiter;
					if (num != 0)
					{
						if (App.OBDSimulator.IsActive)
						{
							App.OBDSimulator.Stop();
						}
						if (App.OBDReader.CurrentStatus == OBDDataReaderStatus.Disconnected)
						{
							goto IL_008D;
						}
						taskAwaiter = App.OBDReader.Disconnect("btnApplyLanguage_Clicked").GetAwaiter();
						if (!taskAwaiter.IsCompleted)
						{
							num2 = 0;
							TaskAwaiter taskAwaiter2 = taskAwaiter;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, SettingsInterfacePage.<btnApplyLanguage_Clicked>d__1>(ref taskAwaiter, ref this);
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
					IL_008D:
					App.Instance.ChangeLanguage();
					SharedSettings.Current.LanguageChanged = false;
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

			// Token: 0x060019C6 RID: 6598 RVA: 0x0010BB70 File Offset: 0x00109D70
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x04000B0D RID: 2829
			public int <>1__state;

			// Token: 0x04000B0E RID: 2830
			public AsyncVoidMethodBuilder <>t__builder;

			// Token: 0x04000B0F RID: 2831
			private TaskAwaiter <>u__1;
		}

		// Token: 0x020001E3 RID: 483
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <btnResetDashboard_Clicked>d__3 : IAsyncStateMachine
		{
			// Token: 0x060019C7 RID: 6599 RVA: 0x0010BB80 File Offset: 0x00109D80
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				SettingsInterfacePage settingsInterfacePage = this;
				try
				{
					TaskAwaiter<bool> taskAwaiter3;
					if (num != 0)
					{
						taskAwaiter3 = settingsInterfacePage.DisplayAlert(Translate.GetString("ios_ResetDashboard_Title"), Translate.GetString("ios_ResetDashboard_Text"), Translate.GetString("ios_Yes"), Translate.GetString("ios_No")).GetAwaiter();
						if (!taskAwaiter3.IsCompleted)
						{
							num2 = 0;
							taskAwaiter2 = taskAwaiter3;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<bool>, SettingsInterfacePage.<btnResetDashboard_Clicked>d__3>(ref taskAwaiter3, ref this);
							return;
						}
					}
					else
					{
						taskAwaiter3 = taskAwaiter2;
						taskAwaiter2 = default(TaskAwaiter<bool>);
						num2 = -1;
					}
					if (taskAwaiter3.GetResult())
					{
						DashboardListViewModel.Current.ClearDashboard();
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

			// Token: 0x060019C8 RID: 6600 RVA: 0x0010BC68 File Offset: 0x00109E68
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x04000B10 RID: 2832
			public int <>1__state;

			// Token: 0x04000B11 RID: 2833
			public AsyncVoidMethodBuilder <>t__builder;

			// Token: 0x04000B12 RID: 2834
			public SettingsInterfacePage <>4__this;

			// Token: 0x04000B13 RID: 2835
			private TaskAwaiter<bool> <>u__1;
		}
	}
}
