using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Reflection;
using Xamarin.Forms;
using Xamarin.Forms.Internals;
using Xamarin.Forms.Xaml;
using Xamarin.Forms.Xaml.Diagnostics;
using Xamarin.Forms.Xaml.Internals;

namespace CarScannerXamarinForms.Styles
{
	// Token: 0x020001EE RID: 494
	[XamlCompilation(2)]
	[XamlFilePath("Styles\\LightTheme.xaml")]
	public class LightTheme : ResourceDictionary
	{
		// Token: 0x060019F3 RID: 6643 RVA: 0x0011248E File Offset: 0x0011068E
		public LightTheme()
		{
			this.InitializeComponent();
			ICollection<string> keys = base.Keys;
		}

		// Token: 0x060019F4 RID: 6644 RVA: 0x001124A4 File Offset: 0x001106A4
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private void InitializeComponent()
		{
			if (ResourceLoader.IsEnabled && ResourceLoader.CanProvideContentFor(new ResourceLoader.ResourceLoadingQuery
			{
				AssemblyName = typeof(LightTheme).GetTypeInfo().Assembly.GetName(),
				ResourcePath = "Styles/LightTheme.xaml",
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
			string text = "blue_icons8_speedometer.png";
			string text2 = "icons8_speedometer.png";
			string text3 = "blue_icons8_cosine.png";
			string text4 = "icons8_cosine.png";
			string text5 = "blue_view_details.png";
			string text6 = "view_details.png";
			string text7 = "blue_engine.png";
			string text8 = "engine.png";
			string text9 = "blue_save.png";
			string text10 = "save.png";
			string text11 = "blue_test_passed.png";
			string text12 = "test_passed.png";
			string text13 = "blue_f1_race_car_top_veiw.png";
			string text14 = "f1_race_car_top_veiw.png";
			string text15 = "blue_biomass.png";
			string text16 = "biomass.png";
			string text17 = "main_records.png";
			string text18 = "icons8_info_invert.png";
			string text19 = "icons8_info.png";
			string text20 = "logo200.png";
			string text21 = "blue_informatics.png";
			string text22 = "informatics.png";
			string text23 = "blue_versions.png";
			string text24 = "grey_versions.png";
			string text25 = "blue_settings.png";
			string text26 = "blue_purchase.png";
			string text27 = "blue_fuel_statistics.png";
			string text28 = "blue_garage.png";
			string text29 = "garage.png";
			string text30 = "icons8_info.png";
			string text31 = "blue_coding.png";
			string text32 = "coding.png";
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
			string text45 = "icons8_chevron_up.png";
			string text46 = "icons8_chevron_down.png";
			string text47 = "icons8_trash_filled.png";
			string text48 = "icons8_settings.png";
			Color color = new Color(0.0, 0.0, 0.0, 1.0);
			Color color2 = new Color(1.0, 1.0, 1.0, 1.0);
			Color darkGray = Color.DarkGray;
			Color color3 = new Color(1.0, 1.0, 1.0, 1.0);
			Color color4 = new Color(0.0, 0.501960813999176, 0.0, 1.0);
			Color color5 = new Color(1.0, 0.0, 0.0, 1.0);
			Color color6 = new Color(0.0, 0.48235294222831726, 1.0, 1.0);
			Color color7 = new Color(0.0, 0.0, 0.0, 1.0);
			Color lightGray = Color.LightGray;
			Color color8 = new Color(0.09803921729326248, 0.4627451002597809, 0.8235294222831726, 1.0);
			Color white = Color.White;
			Color color9 = new Color(1.0, 1.0, 1.0, 1.0);
			Color white2 = Color.White;
			Color color10 = new Color(0.4627451002597809, 0.8392156958580017, 0.4470588266849518, 1.0);
			Color silver = Color.Silver;
			Color color11 = new Color(0.0, 0.47843137383461, 1.0, 1.0);
			Color black = Color.Black;
			Color darkGray2 = Color.DarkGray;
			Color lightGray2 = Color.LightGray;
			Color darkGray3 = Color.DarkGray;
			Color green = Color.Green;
			Color darkOrange = Color.DarkOrange;
			Color red = Color.Red;
			Color color12 = new Color(0.9490196108818054, 0.9490196108818054, 0.9647058844566345, 1.0);
			Color color13 = new Color(1.0, 1.0, 1.0, 1.0);
			Color color14 = new Color(0.2823529541492462, 0.2823529541492462, 0.2823529541492462, 1.0);
			Color color15 = new Color(0.5411764979362488, 0.5411764979362488, 0.5529412031173706, 1.0);
			Color color16 = new Color(0.4274509847164154, 0.4274509847164154, 0.4431372582912445, 1.0);
			Color color17 = new Color(0.4000000059604645, 0.4000000059604645, 0.4000000059604645, 1.0);
			VisualDiagnostics.RegisterSourceInfo(this, new Uri("Styles\\LightTheme.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 2, 2);
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
			this.Add("InfoImageWhite", text18);
			this.Add("InfoImageBlack", text19);
			this.Add("LogoImage", text20);
			this.Add("TerminalActiveImage", text21);
			this.Add("TerminalInactiveImage", text22);
			this.Add("VersionsActiveImage", text23);
			this.Add("VersionsInactiveImage", text24);
			this.Add("SettingsImage", text25);
			this.Add("PurchaseImage", text26);
			this.Add("FuelStatisticsImage", text27);
			this.Add("GarageActiveImage", text28);
			this.Add("GarageInactiveImage", text29);
			this.Add("InfoImageTextColor", text30);
			this.Add("CodingActiveImage", text31);
			this.Add("CodingInactiveImage", text32);
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
			this.Add("GrayedTextColor", darkGray);
			this.Add("BackgroundColor", color3);
			this.Add("ButtonGreenColor", color4);
			this.Add("ButtonRedColor", color5);
			this.Add("ButtonAccentColor", color6);
			this.Add("ElementBorderColor", color7);
			this.Add("ButtonBackgroundColor", lightGray);
			this.Add("NavigationBarBackgroundColor", color8);
			this.Add("NavigationBarTextColor", white);
			this.Add("NavigationBarButtonColor", color9);
			this.Add("EntryBackgroundColor", white2);
			this.Add("SwitchOnColor", color10);
			this.Add("DashboardItemEditorBackground", silver);
			this.Add("ChartLineColor", color11);
			this.Add("ChartLabelColor", black);
			this.Add("ChartStrokeColor", darkGray2);
			this.Add("SliderBackgroundColor", lightGray2);
			this.Add("ListViewSeparatorColor", darkGray3);
			this.Add("GreenTextColor", green);
			this.Add("YellowTextColor", darkOrange);
			this.Add("RedTextColor", red);
			this.Add("SettingsBackground", color12);
			this.Add("SettingsCellBackground", color13);
			this.Add("SettingsCellTitleColor", color14);
			this.Add("SettingsCellValueTextColor", color15);
			this.Add("SettingsFooterTextColor", color16);
			this.Add("SettingsHeaderTextColor", color17);
		}

		// Token: 0x060019F5 RID: 6645 RVA: 0x00112E89 File Offset: 0x00111089
		private void __InitComponentRuntime()
		{
			Extensions.LoadFromXaml<LightTheme>(this, typeof(LightTheme));
		}
	}
}
