using System;
using System.CodeDom.Compiler;
using System.Reflection;
using Xamarin.Forms;
using Xamarin.Forms.Internals;
using Xamarin.Forms.Xaml;
using Xamarin.Forms.Xaml.Diagnostics;
using Xamarin.Forms.Xaml.Internals;

namespace CarScannerXamarinForms.Styles
{
	// Token: 0x020001EB RID: 491
	[XamlCompilation(2)]
	[XamlFilePath("Styles\\DarkTheme.xaml")]
	public class DarkTheme : ResourceDictionary
	{
		// Token: 0x060019EA RID: 6634 RVA: 0x00111692 File Offset: 0x0010F892
		public DarkTheme()
		{
			this.InitializeComponent();
		}

		// Token: 0x060019EB RID: 6635 RVA: 0x001116A0 File Offset: 0x0010F8A0
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private void InitializeComponent()
		{
			if (ResourceLoader.IsEnabled && ResourceLoader.CanProvideContentFor(new ResourceLoader.ResourceLoadingQuery
			{
				AssemblyName = typeof(DarkTheme).GetTypeInfo().Assembly.GetName(),
				ResourcePath = "Styles/DarkTheme.xaml",
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
			string text = "dark_blue_icons8_speedometer.png";
			string text2 = "dark_icons8_speedometer.png";
			string text3 = "dark_blue_icons8_cosine.png";
			string text4 = "dark_icons8_cosine.png";
			string text5 = "dark_blue_view_details.png";
			string text6 = "dark_view_details.png";
			string text7 = "dark_blue_engine.png";
			string text8 = "dark_engine.png";
			string text9 = "dark_blue_save.png";
			string text10 = "dark_save.png";
			string text11 = "dark_blue_test_passed.png";
			string text12 = "dark_test_passed.png";
			string text13 = "dark_blue_f1_race_car_top_veiw.png";
			string text14 = "dark_f1_race_car_top_veiw.png";
			string text15 = "dark_blue_biomass.png";
			string text16 = "dark_biomass.png";
			string text17 = "dark_main_records.png";
			string text18 = "dark_blue_informatics.png";
			string text19 = "dark_informatics.png";
			string text20 = "dark_blue_versions.png";
			string text21 = "dark_grey_versions.png";
			string text22 = "dark_blue_settings.png";
			string text23 = "dark_blue_purchase.png";
			string text24 = "dark_blue_fuel_statistics.png";
			string text25 = "dark_blue_garage.png";
			string text26 = "dark_garage.png";
			string text27 = "dark_blue_coding.png";
			string text28 = "dark_coding.png";
			string text29 = "icons8_info_invert.png";
			string text30 = "icons8_info.png";
			string text31 = "logo200_white.png";
			string text32 = "icons8_info_invert.png";
			string text33 = "gas_station.png";
			string text34 = "icons8_settings_invert.png";
			string text35 = "icons8_shopping_cart.png";
			string text36 = "icons8_trash_filled_inverted.png";
			string text37 = "icons8_filter_white.png";
			string text38 = "icons8_joyent_inverted.png";
			string text39 = "add_inverted.png";
			string text40 = "icons8_checked_checkbox.png";
			string text41 = "icons8_unchecked_checkbox.png";
			string text42 = "icons8_list.png";
			string text43 = "icons8_info_invert.png";
			string text44 = "icons8_info_invert.png";
			string text45 = "icons8_chevron_up_white.png";
			string text46 = "icons8_chevron_down_white.png";
			string text47 = "icons8_trash_filled_inverted.png";
			string text48 = "icons8_settings_invert.png";
			Color color = new Color(1.0, 1.0, 1.0, 1.0);
			Color color2 = new Color(0.0, 0.0, 0.0, 1.0);
			Color lightGray = Color.LightGray;
			Color color3 = new Color(0.0, 0.0, 0.0, 1.0);
			Color color4 = new Color(0.03921568766236305, 0.5176470875740051, 1.0, 1.0);
			Color color5 = new Color(1.0, 0.0, 0.0, 1.0);
			Color color6 = new Color(0.03921568766236305, 0.5176470875740051, 1.0, 1.0);
			Color color7 = new Color(1.0, 1.0, 1.0, 1.0);
			Color lightGray2 = Color.LightGray;
			Color color8 = new Color(0.3294117748737335, 0.3294117748737335, 0.3294117748737335, 1.0);
			Color color9 = new Color(1.0, 1.0, 1.0, 1.0);
			Color color10 = new Color(1.0, 1.0, 1.0, 1.0);
			Color color11 = new Color(0.250980406999588, 0.250980406999588, 0.250980406999588, 1.0);
			Color color12 = new Color(0.4627451002597809, 0.8392156958580017, 0.4470588266849518, 1.0);
			Color darkGray = Color.DarkGray;
			Color color13 = new Color(0.0, 0.47843137383461, 1.0, 1.0);
			Color white = Color.White;
			Color lightGray3 = Color.LightGray;
			Color color14 = new Color(0.250980406999588, 0.250980406999588, 0.250980406999588, 1.0);
			Color gray = Color.Gray;
			Color color15 = new Color(0.0, 0.6980392336845398, 0.20000000298023224, 1.0);
			Color color16 = new Color(0.9019607901573181, 1.0, 0.09803921729326248, 1.0);
			Color red = Color.Red;
			Color color17 = new Color(0.0, 0.0, 0.0, 1.0);
			Color color18 = new Color(0.10980392247438431, 0.10980392247438431, 0.11764705926179886, 1.0);
			Color color19 = new Color(1.0, 1.0, 1.0, 1.0);
			Color color20 = new Color(0.5960784554481506, 0.5960784554481506, 0.6196078658103943, 1.0);
			Color color21 = new Color(0.5568627715110779, 0.5568627715110779, 0.572549045085907, 1.0);
			Color color22 = new Color(0.5568627715110779, 0.5568627715110779, 0.572549045085907, 1.0);
			VisualDiagnostics.RegisterSourceInfo(this, new Uri("Styles\\DarkTheme.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 2, 2);
			NameScope nameScope = new NameScope();
			this.Add("DashboardActiveImage", text);
			this.Add("DashboardInactiveImage", text2);
			this.Add("LiveDataActiveImage", text3);
			this.Add("LiveDataInactiveImage", text4);
			this.Add("LiveDataListActiveImage", text5);
			this.Add("LiveDataListInactiveImage", text6);
			this.Add("DTCActiveImage", text7);
			this.Add("DTCInactiveImage", text8);
			this.Add("FreezeFrameActiveImage", text9);
			this.Add("FreezeFrameInactiveImage", text10);
			this.Add("Mode06ActiveImage", text11);
			this.Add("Mode06InactiveImage", text12);
			this.Add("SpeedTestActiveImage", text13);
			this.Add("SpeedTestInactiveImage", text14);
			this.Add("EcoTestsActiveImage", text15);
			this.Add("EcoTestsInactiveImage", text16);
			this.Add("RecordsImage", text17);
			this.Add("TerminalActiveImage", text18);
			this.Add("TerminalInactiveImage", text19);
			this.Add("VersionsActiveImage", text20);
			this.Add("VersionsInactiveImage", text21);
			this.Add("SettingsImage", text22);
			this.Add("PurchaseImage", text23);
			this.Add("FuelStatisticsImage", text24);
			this.Add("GarageActiveImage", text25);
			this.Add("GarageInactiveImage", text26);
			this.Add("CodingActiveImage", text27);
			this.Add("CodingInactiveImage", text28);
			this.Add("InfoImageWhite", text29);
			this.Add("InfoImageBlack", text30);
			this.Add("LogoImage", text31);
			this.Add("InfoImageTextColor", text32);
			this.Add("NB_gas_station", text33);
			this.Add("NB_settings", text34);
			this.Add("NB_purchase", text35);
			this.Add("NB_delete", text36);
			this.Add("NB_filter", text37);
			this.Add("NB_add_wcircle", text38);
			this.Add("NB_add", text39);
			this.Add("NB_check", text40);
			this.Add("NB_uncheck", text41);
			this.Add("NB_legend", text42);
			this.Add("InfoImageNavigationBarTextColor", text43);
			this.Add("NB_info", text44);
			this.Add("TC_up_image", text45);
			this.Add("TC_down_image", text46);
			this.Add("TC_del", text47);
			this.Add("TC_settings", text48);
			this.Add("TextColor", color);
			this.Add("TextInverseColor", color2);
			this.Add("GrayedTextColor", lightGray);
			this.Add("BackgroundColor", color3);
			this.Add("ButtonGreenColor", color4);
			this.Add("ButtonRedColor", color5);
			this.Add("ButtonAccentColor", color6);
			this.Add("ElementBorderColor", color7);
			this.Add("ButtonBackgroundColor", lightGray2);
			this.Add("NavigationBarBackgroundColor", color8);
			this.Add("NavigationBarTextColor", color9);
			this.Add("NavigationBarButtonColor", color10);
			this.Add("EntryBackgroundColor", color11);
			this.Add("SwitchOnColor", color12);
			this.Add("DashboardItemEditorBackground", darkGray);
			this.Add("ChartLineColor", color13);
			this.Add("ChartLabelColor", white);
			this.Add("ChartStrokeColor", lightGray3);
			this.Add("SliderBackgroundColor", color14);
			this.Add("ListViewSeparatorColor", gray);
			this.Add("GreenTextColor", color15);
			this.Add("YellowTextColor", color16);
			this.Add("RedTextColor", red);
			this.Add("SettingsBackground", color17);
			this.Add("SettingsCellBackground", color18);
			this.Add("SettingsCellTitleColor", color19);
			this.Add("SettingsCellValueTextColor", color20);
			this.Add("SettingsFooterTextColor", color21);
			this.Add("SettingsHeaderTextColor", color22);
		}

		// Token: 0x060019EC RID: 6636 RVA: 0x00112139 File Offset: 0x00110339
		private void __InitComponentRuntime()
		{
			Extensions.LoadFromXaml<DarkTheme>(this, typeof(DarkTheme));
		}
	}
}
