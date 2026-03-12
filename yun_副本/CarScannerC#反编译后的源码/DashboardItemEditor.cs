using System;
using System.CodeDom.Compiler;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using CarScannerXamarinForms.Common.XAMLConverters;
using CarScannerXamarinForms.Dashboard;
using CarScannerXamarinForms.Dashboard.ProxySettings;
using CarScannerXamarinForms.OBD2;
using CarScannerXamarinForms.OBD2.PIDS;
using CarScannerXamarinForms.Pages;
using CarScannerXamarinForms.Pages.Dashboard;
using CarScannerXamarinForms.PlatformAdapters;
using CarScannerXamarinForms.Settings;
using CarScannerXamarinForms.Settings.SettingsV3;
using CarScannerXamarinForms.UserControls;
using CarScannerXamarinForms.ViewModels;
using Sounds.FormsPlugin.Abstractions;
using Syncfusion.XForms.Expander;
using Xamarin.CommunityToolkit.Converters;
using Xamarin.Forms;
using Xamarin.Forms.Internals;
using Xamarin.Forms.PlatformConfiguration.iOSSpecific;
using Xamarin.Forms.Xaml;
using Xamarin.Forms.Xaml.Diagnostics;
using Xamarin.Forms.Xaml.Internals;

namespace CarScannerXamarinForms
{
	// Token: 0x020001C2 RID: 450
	[XamlFilePath("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\Dashboard\\DashboardItemEditor.xaml")]
	public class DashboardItemEditor : ContentPage
	{
		// Token: 0x0600175D RID: 5981 RVA: 0x000B3550 File Offset: 0x000B1750
		public DashboardItemEditor(DashboardItem originalItem, double width, double height)
		{
			this.originalItem = originalItem;
			this._item = new DashboardItem();
			new ProxyItem(originalItem).ApplySettingsToRealItem(this._item, false);
			this._item.SelectAndAddControl();
			this.orig_height = height;
			this.orig_width = width;
			this._item.HorizontalOptions = LayoutOptions.Center;
			this._item.Model = new LiveDataPIDModel
			{
				DoubleFormat = this._item.ValueFormat
			};
			this._item.Start();
			this.InitializeComponent();
			string[] array = new string[]
			{
				Translate.GetString("Settings_ChartStyle_FastLine"),
				Translate.GetString("Settings_ChartStyle_Area"),
				Translate.GetString("Settings_ChartStyle_Spline"),
				Translate.GetString("Settings_ChartStyle_SplineArea")
			};
			this.chartStylePicker.ItemsSource = array;
			if (PlatformHelper.IsiOS)
			{
				Page.SetPrefersHomeIndicatorAutoHidden(this, false);
			}
		}

		// Token: 0x0600175E RID: 5982 RVA: 0x000B3637 File Offset: 0x000B1837
		private void CircularGaugeWidthEntry_Completed(object sender, EventArgs e)
		{
			this._item.SelectAndAddControl();
		}

		// Token: 0x0600175F RID: 5983 RVA: 0x000B3644 File Offset: 0x000B1844
		private void Handle_SizeChanged(object sender, EventArgs e)
		{
			if (this.PreviewGrid != null)
			{
				this.PreviewGrid.HorizontalOptions = LayoutOptions.Fill;
				this.PreviewGrid.HorizontalOptions = LayoutOptions.FillAndExpand;
			}
		}

		// Token: 0x06001760 RID: 5984 RVA: 0x000B3670 File Offset: 0x000B1870
		private async void Handle_Appearing(object sender, EventArgs e)
		{
			if (this.PreviewGrid.Children.Count == 0)
			{
				if (this.shouldStartItem)
				{
					this._item.Start();
				}
				Grid.SetRow(this._item, 0);
				Grid.SetColumn(this._item, 1);
				Grid.SetRowSpan(this._item, 1);
				Grid.SetColumnSpan(this._item, 1);
				double width = this.originalItem.Width;
				double height = this.originalItem.Height;
				if (this.orig_height < height)
				{
					this._item.WidthRequest = this.orig_width;
					this._item.HeightRequest = this.orig_height;
				}
				else
				{
					double num = height / this.orig_height;
					double num2 = this.orig_height / this.orig_width;
					this._item.WidthRequest = height / num2;
				}
				this.PreviewGrid.Children.Add(this._item);
			}
			base.BindingContext = null;
			base.BindingContext = this._item;
		}

		// Token: 0x06001761 RID: 5985 RVA: 0x000B36A7 File Offset: 0x000B18A7
		private void Entry_BindingContextChanged(object sender, EventArgs e)
		{
			if (sender is Entry)
			{
				this.numericEntry_SizeChanged(sender as Entry, e);
			}
		}

		// Token: 0x06001762 RID: 5986 RVA: 0x000B36BE File Offset: 0x000B18BE
		public static IEnumerable<T> GetControlsOfType<T>(View root) where T : View
		{
			DashboardItemEditor.<GetControlsOfType>d__9<T> <GetControlsOfType>d__ = new DashboardItemEditor.<GetControlsOfType>d__9<T>(-2);
			<GetControlsOfType>d__.<>3__root = root;
			return <GetControlsOfType>d__;
		}

		// Token: 0x06001763 RID: 5987 RVA: 0x000B36CE File Offset: 0x000B18CE
		protected override bool OnBackButtonPressed()
		{
			this.btnBack_Clicked(this.btnBack, null);
			return true;
		}

		// Token: 0x06001764 RID: 5988 RVA: 0x000B36E0 File Offset: 0x000B18E0
		private async void btnBack_Clicked(object sender, EventArgs e)
		{
			try
			{
				(sender as Button).IsEnabled = false;
				this.activityFrame.IsVisible = true;
				await Task.Delay(100);
				base.IsEnabled = false;
				this.PreviewGrid.Children.Clear();
				DashboardItem item = this._item;
				if (item != null)
				{
					item.Stop();
				}
				DashboardXamlPage.Instance.ShouldLoadDashboardFromSettings = true;
				DashboardPage dashboardPage = DashboardListViewModel.Current.Pages.FirstOrDefault((DashboardPage x) => x.Items.Contains(this.originalItem));
				if (dashboardPage != null)
				{
					int num = DashboardListViewModel.Current.Pages.IndexOf(dashboardPage);
					int num2 = dashboardPage.Items.IndexOf(this.originalItem);
					ProxyPage proxyPage = new ProxyPage(dashboardPage);
					proxyPage.Items[num2] = new ProxyItem(this._item);
					List<ProxyPage> list = new List<ProxyPage>(DashboardListViewModel.Current.Pages.Count);
					foreach (DashboardPage dashboardPage2 in DashboardListViewModel.Current.Pages)
					{
						list.Add((dashboardPage2 != dashboardPage) ? new ProxyPage(dashboardPage2) : proxyPage);
					}
					DashboardListViewModel.Current.SaveDashboardToSettings(list);
					Page page = base.Navigation.NavigationStack.FirstOrDefault((Page x) => x is DashboardEditorPage);
					if (page != null)
					{
						DashboardListViewModel.Current.LoadDashboardFromSettings();
						page.BindingContext = DashboardListViewModel.Current.Pages[num];
					}
				}
				this.activityFrame.IsVisible = false;
				base.Navigation.PopAsync();
			}
			catch (Exception)
			{
			}
		}

		// Token: 0x06001765 RID: 5989 RVA: 0x000B3720 File Offset: 0x000B1920
		private async void btnSelectPID_Clicked(object sender, EventArgs e)
		{
			IPID ipid;
			if (this._item.ItemType == DashboardItemTypes.Action)
			{
				ipid = await PIDSelector.SelectPIDAsync(this._item.Model.SelectedPID, (PID x) => (x != null && x == PID.Empty) || (x is CustomPID && (x as CustomPID).IsAction));
			}
			else
			{
				ipid = await PIDSelector.SelectPIDAsync(this._item.Model.SelectedPID, (PID x) => x != null && (!(x is CustomPID) || !(x as CustomPID).IsAction));
			}
			if (ipid != null)
			{
				this._item.PID_Id = ipid.Id;
				if (ipid.Minimum < ipid.Maximum)
				{
					this._item.Minimum = ipid.Minimum;
					this._item.Maximum = ipid.Maximum;
				}
				else if (double.IsFinite(ipid.Minimum))
				{
					this._item.Minimum = ipid.Minimum;
					this._item.Maximum = ipid.Minimum + 1.0;
				}
				else if (double.IsFinite(ipid.Maximum))
				{
					this._item.Maximum = ipid.Maximum;
					this._item.Minimum = ipid.Maximum - 1.0;
				}
				else
				{
					this._item.Minimum = 0.0;
					this._item.Maximum = 100.0;
				}
				this._item.CustomName = ipid.ShortName;
				this._item.Start();
				List<OBDRequest> list = new List<OBDRequest>();
				this._item.Model.GetRequests(list, null, "");
				foreach (OBDRequest obdrequest in list)
				{
					App.OBDReader.AddRequestToQueue(obdrequest);
				}
			}
		}

		// Token: 0x06001766 RID: 5990 RVA: 0x000B3758 File Offset: 0x000B1958
		private void SelectColor(string propertyName)
		{
			ColorSelectorPageV2 colorSelectorPageV = new ColorSelectorPageV2((Color)base.BindingContext.GetType().GetProperty(propertyName).GetValue(base.BindingContext), delegate(Color new_color)
			{
				this.BindingContext.GetType().GetProperty(propertyName).SetValue(this.BindingContext, new_color);
			});
			base.Navigation.PushAsync(colorSelectorPageV, true);
		}

		// Token: 0x06001767 RID: 5991 RVA: 0x000B37BF File Offset: 0x000B19BF
		private void TitleColor_Tapped(object sender, EventArgs e)
		{
			this.SelectColor("TitleTextColor");
		}

		// Token: 0x06001768 RID: 5992 RVA: 0x000B37CC File Offset: 0x000B19CC
		private void BackgroundColor_Tapped(object sender, EventArgs e)
		{
			this.SelectColor("BackgroundColor");
		}

		// Token: 0x06001769 RID: 5993 RVA: 0x000B37D9 File Offset: 0x000B19D9
		private void ValueNormalTextColor_Tapped(object sender, EventArgs e)
		{
			this.SelectColor("ValueNormalTextColor");
		}

		// Token: 0x0600176A RID: 5994 RVA: 0x000B37E6 File Offset: 0x000B19E6
		private void UnitsTextColor_Tapped(object sender, EventArgs e)
		{
			this.SelectColor("UnitsTextColor");
		}

		// Token: 0x0600176B RID: 5995 RVA: 0x000B37F3 File Offset: 0x000B19F3
		private void FrameColor_Tapped(object sender, EventArgs e)
		{
			this.SelectColor("FrameColor");
		}

		// Token: 0x0600176C RID: 5996 RVA: 0x000B3800 File Offset: 0x000B1A00
		private void ColorBox_Tapped(object sender, EventArgs e)
		{
			View view = sender as View;
			this.SelectColor(view.ClassId);
		}

		// Token: 0x0600176D RID: 5997 RVA: 0x000B3820 File Offset: 0x000B1A20
		private void ItemTypePicker_SelectedIndexChanged(object sender, EventArgs e)
		{
			switch (this._item.ItemType)
			{
			case DashboardItemTypes.Text:
			case DashboardItemTypes.Action:
			case DashboardItemTypes.TextHorizontal:
				this.panelGauge.IsVisible = false;
				this.panelChart.IsVisible = false;
				this.panelTextItem.IsVisible = true;
				this.panelLinearGauge.IsVisible = false;
				this.panelGaugeVar2.IsVisible = false;
				this.panelGaugeVar3.IsVisible = false;
				return;
			case DashboardItemTypes.Chart:
				this.panelGauge.IsVisible = false;
				this.panelChart.IsVisible = true;
				this.panelTextItem.IsVisible = true;
				this.panelLinearGauge.IsVisible = false;
				this.panelGaugeVar2.IsVisible = false;
				this.panelGaugeVar3.IsVisible = false;
				return;
			case DashboardItemTypes.Gauge:
			case DashboardItemTypes.MultiPidBarChart:
				this.panelGauge.IsVisible = true;
				this.panelChart.IsVisible = false;
				this.panelTextItem.IsVisible = true;
				this.panelLinearGauge.IsVisible = false;
				this.panelGaugeVar2.IsVisible = false;
				this.panelGaugeVar3.IsVisible = false;
				return;
			case DashboardItemTypes.LinearGauge:
				this.panelGauge.IsVisible = false;
				this.panelChart.IsVisible = false;
				this.panelTextItem.IsVisible = true;
				this.panelLinearGauge.IsVisible = true;
				this.panelGaugeVar2.IsVisible = false;
				this.panelGaugeVar3.IsVisible = false;
				if (this._item.GaugeRimColor == this._item.GaugePointerColor && this._item != null && base.BindingContext != null)
				{
					Color color;
					color..ctor(1.0 - this._item.GaugeRimColor.R, 1.0 - this._item.GaugeRimColor.G, 1.0 - this._item.GaugeRimColor.B);
					base.BindingContext.GetType().GetProperty("GaugePointerColor").SetValue(base.BindingContext, color);
				}
				break;
			case DashboardItemTypes.GaugeVar2:
				this.panelGauge.IsVisible = false;
				this.panelChart.IsVisible = false;
				this.panelTextItem.IsVisible = true;
				this.panelLinearGauge.IsVisible = false;
				this.panelGaugeVar2.IsVisible = true;
				this.panelGaugeVar3.IsVisible = false;
				if (this._item.GaugeRimColor == this._item.GaugePointerColor && this._item != null && base.BindingContext != null)
				{
					Color color2;
					color2..ctor(1.0 - this._item.GaugeRimColor.R, 1.0 - this._item.GaugeRimColor.G, 1.0 - this._item.GaugeRimColor.B);
					base.BindingContext.GetType().GetProperty("GaugePointerColor").SetValue(base.BindingContext, color2);
					return;
				}
				break;
			case DashboardItemTypes.GaugeVar3:
				this.panelGauge.IsVisible = false;
				this.panelChart.IsVisible = false;
				this.panelTextItem.IsVisible = true;
				this.panelLinearGauge.IsVisible = false;
				this.panelGaugeVar2.IsVisible = false;
				this.panelGaugeVar3.IsVisible = true;
				return;
			default:
				return;
			}
		}

		// Token: 0x0600176E RID: 5998 RVA: 0x000B3B90 File Offset: 0x000B1D90
		private void ApplyCurrentPage_Clicked(object sender, EventArgs e)
		{
			DashboardPage dashboardPage = DashboardListViewModel.Current.Pages.FirstOrDefault((DashboardPage x) => x.Items.Contains(this.originalItem));
			if (dashboardPage != null)
			{
				ProxyItem proxyItem = new ProxyItem(this._item);
				int num = DashboardListViewModel.Current.Pages.IndexOf(dashboardPage);
				int num2 = dashboardPage.Items.IndexOf(this.originalItem);
				List<ProxyPage> list = DashboardListViewModel.Current.Pages.Select((DashboardPage x) => new ProxyPage(x)).ToList<ProxyPage>();
				list[num].Items[num2] = proxyItem;
				foreach (ProxyItem proxyItem2 in list[num].Items)
				{
					if (proxyItem.ItemType == proxyItem2.ItemType && proxyItem2 != proxyItem)
					{
						proxyItem.ApplySettingsToProxyItem(proxyItem2, true);
					}
				}
				DashboardListViewModel.Current.SaveDashboardToSettings(list);
				DashboardListViewModel.Current.LoadDashboardFromSettings();
				Page page = base.Navigation.NavigationStack.FirstOrDefault((Page x) => x is DashboardEditorPage);
				if (page != null)
				{
					DashboardListViewModel.Current.LoadDashboardFromSettings();
					page.BindingContext = DashboardListViewModel.Current.Pages[num];
				}
			}
			this.activityFrame.IsVisible = false;
			base.DisplayAlert(Translate.GetString("ios_Done"), "", "OK");
		}

		// Token: 0x0600176F RID: 5999 RVA: 0x000B3D30 File Offset: 0x000B1F30
		private void ApplyColorsCurrentPage_Clicked(object sender, EventArgs e)
		{
			DashboardPage dashboardPage = DashboardListViewModel.Current.Pages.FirstOrDefault((DashboardPage x) => x.Items.Contains(this.originalItem));
			if (dashboardPage != null)
			{
				ProxyItem proxyItem = new ProxyItem(this._item);
				int num = DashboardListViewModel.Current.Pages.IndexOf(dashboardPage);
				int num2 = dashboardPage.Items.IndexOf(this.originalItem);
				List<ProxyPage> list = DashboardListViewModel.Current.Pages.Select((DashboardPage x) => new ProxyPage(x)).ToList<ProxyPage>();
				list[num].Items[num2] = proxyItem;
				foreach (ProxyItem proxyItem2 in list[num].Items)
				{
					if (proxyItem.ItemType == proxyItem2.ItemType && proxyItem2 != proxyItem)
					{
						proxyItem.ApplyColorSettingsToProxyItem(proxyItem2);
					}
				}
				DashboardListViewModel.Current.SaveDashboardToSettings(list);
				DashboardListViewModel.Current.LoadDashboardFromSettings();
				Page page = base.Navigation.NavigationStack.FirstOrDefault((Page x) => x is DashboardEditorPage);
				if (page != null)
				{
					DashboardListViewModel.Current.LoadDashboardFromSettings();
					page.BindingContext = DashboardListViewModel.Current.Pages[num];
				}
			}
			this.activityFrame.IsVisible = false;
			base.DisplayAlert(Translate.GetString("ios_Done"), "", "OK");
		}

		// Token: 0x06001770 RID: 6000 RVA: 0x000B3ED0 File Offset: 0x000B20D0
		private void ApplyCurrentPageAllTypes_Clicked(object sender, EventArgs e)
		{
			DashboardPage dashboardPage = DashboardListViewModel.Current.Pages.FirstOrDefault((DashboardPage x) => x.Items.Contains(this.originalItem));
			if (dashboardPage != null)
			{
				ProxyItem proxyItem = new ProxyItem(this._item);
				int num = DashboardListViewModel.Current.Pages.IndexOf(dashboardPage);
				int num2 = dashboardPage.Items.IndexOf(this.originalItem);
				List<ProxyPage> list = DashboardListViewModel.Current.Pages.Select((DashboardPage x) => new ProxyPage(x)).ToList<ProxyPage>();
				list[num].Items[num2] = proxyItem;
				foreach (ProxyItem proxyItem2 in list[num].Items)
				{
					if (proxyItem2 != proxyItem)
					{
						proxyItem.ApplySettingsToProxyItem(proxyItem2, true);
					}
				}
				DashboardListViewModel.Current.SaveDashboardToSettings(list);
				DashboardListViewModel.Current.LoadDashboardFromSettings();
				Page page = base.Navigation.NavigationStack.FirstOrDefault((Page x) => x is DashboardEditorPage);
				if (page != null)
				{
					DashboardListViewModel.Current.LoadDashboardFromSettings();
					page.BindingContext = DashboardListViewModel.Current.Pages[num];
				}
			}
			this.activityFrame.IsVisible = false;
			base.DisplayAlert(Translate.GetString("ios_Done"), "", "OK");
		}

		// Token: 0x06001771 RID: 6001 RVA: 0x000B4060 File Offset: 0x000B2260
		private void ApplyColorCurrentPageAllTypes_Clicked(object sender, EventArgs e)
		{
			DashboardPage dashboardPage = DashboardListViewModel.Current.Pages.FirstOrDefault((DashboardPage x) => x.Items.Contains(this.originalItem));
			if (dashboardPage != null)
			{
				ProxyItem proxyItem = new ProxyItem(this._item);
				int num = DashboardListViewModel.Current.Pages.IndexOf(dashboardPage);
				int num2 = dashboardPage.Items.IndexOf(this.originalItem);
				List<ProxyPage> list = DashboardListViewModel.Current.Pages.Select((DashboardPage x) => new ProxyPage(x)).ToList<ProxyPage>();
				list[num].Items[num2] = proxyItem;
				foreach (ProxyItem proxyItem2 in list[num].Items)
				{
					if (proxyItem2 != proxyItem)
					{
						proxyItem.ApplyColorSettingsToProxyItem(proxyItem2);
					}
				}
				DashboardListViewModel.Current.SaveDashboardToSettings(list);
				DashboardListViewModel.Current.LoadDashboardFromSettings();
				Page page = base.Navigation.NavigationStack.FirstOrDefault((Page x) => x is DashboardEditorPage);
				if (page != null)
				{
					DashboardListViewModel.Current.LoadDashboardFromSettings();
					page.BindingContext = DashboardListViewModel.Current.Pages[num];
				}
			}
			this.activityFrame.IsVisible = false;
			base.DisplayAlert(Translate.GetString("ios_Done"), "", "OK");
		}

		// Token: 0x06001772 RID: 6002 RVA: 0x000B41F0 File Offset: 0x000B23F0
		private void ApplyAllPages_Clicked(object sender, EventArgs e)
		{
			DashboardPage dashboardPage = DashboardListViewModel.Current.Pages.FirstOrDefault((DashboardPage x) => x.Items.Contains(this.originalItem));
			if (dashboardPage != null)
			{
				ProxyItem proxyItem = new ProxyItem(this._item);
				int num = DashboardListViewModel.Current.Pages.IndexOf(dashboardPage);
				int num2 = dashboardPage.Items.IndexOf(this.originalItem);
				List<ProxyPage> list = DashboardListViewModel.Current.Pages.Select((DashboardPage x) => new ProxyPage(x)).ToList<ProxyPage>();
				list[num].Items[num2] = proxyItem;
				foreach (ProxyPage proxyPage in list)
				{
					foreach (ProxyItem proxyItem2 in proxyPage.Items)
					{
						if (proxyItem.ItemType == proxyItem2.ItemType && proxyItem2 != proxyItem)
						{
							proxyItem.ApplySettingsToProxyItem(proxyItem2, true);
						}
					}
				}
				DashboardListViewModel.Current.SaveDashboardToSettings(list);
				DashboardListViewModel.Current.LoadDashboardFromSettings();
				Page page = base.Navigation.NavigationStack.FirstOrDefault((Page x) => x is DashboardEditorPage);
				if (page != null)
				{
					DashboardListViewModel.Current.LoadDashboardFromSettings();
					page.BindingContext = DashboardListViewModel.Current.Pages[num];
				}
			}
			this.activityFrame.IsVisible = false;
			base.DisplayAlert(Translate.GetString("ios_Done"), "", "OK");
		}

		// Token: 0x06001773 RID: 6003 RVA: 0x000B43C0 File Offset: 0x000B25C0
		private void ApplyColorAllPagesSameType_Clicked(object sender, EventArgs e)
		{
			DashboardPage dashboardPage = DashboardListViewModel.Current.Pages.FirstOrDefault((DashboardPage x) => x.Items.Contains(this.originalItem));
			if (dashboardPage != null)
			{
				ProxyItem proxyItem = new ProxyItem(this._item);
				int num = DashboardListViewModel.Current.Pages.IndexOf(dashboardPage);
				int num2 = dashboardPage.Items.IndexOf(this.originalItem);
				List<ProxyPage> list = DashboardListViewModel.Current.Pages.Select((DashboardPage x) => new ProxyPage(x)).ToList<ProxyPage>();
				list[num].Items[num2] = proxyItem;
				foreach (ProxyPage proxyPage in list)
				{
					foreach (ProxyItem proxyItem2 in proxyPage.Items)
					{
						if (proxyItem.ItemType == proxyItem2.ItemType && proxyItem2 != proxyItem)
						{
							proxyItem.ApplyColorSettingsToProxyItem(proxyItem2);
						}
					}
				}
				DashboardListViewModel.Current.SaveDashboardToSettings(list);
				DashboardListViewModel.Current.LoadDashboardFromSettings();
				Page page = base.Navigation.NavigationStack.FirstOrDefault((Page x) => x is DashboardEditorPage);
				if (page != null)
				{
					DashboardListViewModel.Current.LoadDashboardFromSettings();
					page.BindingContext = DashboardListViewModel.Current.Pages[num];
				}
			}
			this.activityFrame.IsVisible = false;
			base.DisplayAlert(Translate.GetString("ios_Done"), "", "OK");
		}

		// Token: 0x06001774 RID: 6004 RVA: 0x000B4590 File Offset: 0x000B2790
		private void ApplyColorAllPages_Clicked(object sender, EventArgs e)
		{
			DashboardPage dashboardPage = DashboardListViewModel.Current.Pages.FirstOrDefault((DashboardPage x) => x.Items.Contains(this.originalItem));
			if (dashboardPage != null)
			{
				ProxyItem proxyItem = new ProxyItem(this._item);
				int num = DashboardListViewModel.Current.Pages.IndexOf(dashboardPage);
				int num2 = dashboardPage.Items.IndexOf(this.originalItem);
				List<ProxyPage> list = DashboardListViewModel.Current.Pages.Select((DashboardPage x) => new ProxyPage(x)).ToList<ProxyPage>();
				list[num].Items[num2] = proxyItem;
				foreach (ProxyPage proxyPage in list)
				{
					foreach (ProxyItem proxyItem2 in proxyPage.Items)
					{
						proxyItem.ApplyColorSettingsToProxyItem(proxyItem2);
					}
				}
				DashboardListViewModel.Current.SaveDashboardToSettings(list);
				DashboardListViewModel.Current.LoadDashboardFromSettings();
				Page page = base.Navigation.NavigationStack.FirstOrDefault((Page x) => x is DashboardEditorPage);
				if (page != null)
				{
					DashboardListViewModel.Current.LoadDashboardFromSettings();
					page.BindingContext = DashboardListViewModel.Current.Pages[num];
				}
			}
			this.activityFrame.IsVisible = false;
			base.DisplayAlert(Translate.GetString("ios_Done"), "", "OK");
		}

		// Token: 0x06001775 RID: 6005 RVA: 0x000B474C File Offset: 0x000B294C
		private void PIDPicker_SelectedIndexChanged(object sender, EventArgs e)
		{
			((sender as Picker).BindingContext as DashboardItem).Start();
		}

		// Token: 0x06001776 RID: 6006 RVA: 0x000B4764 File Offset: 0x000B2964
		private void numericEntry_TextChanged(object sender, TextChangedEventArgs e)
		{
			string text = e.NewTextValue.Replace(".", CultureInfo.CurrentUICulture.NumberFormat.NumberDecimalSeparator).Replace(",", CultureInfo.CurrentUICulture.NumberFormat.NumberDecimalSeparator);
			Entry entry = sender as Entry;
			DashboardItem dashboardItem = entry.BindingContext as DashboardItem;
			PropertyInfo property = dashboardItem.GetType().GetProperty(entry.ClassId);
			double num = 0.0;
			if (e.NewTextValue == "" || e.NewTextValue == "-")
			{
				property.SetValue(dashboardItem, 0);
				return;
			}
			if (double.TryParse(text, out num))
			{
				property.SetValue(dashboardItem, num);
				return;
			}
			if (!double.TryParse(e.OldTextValue, out num))
			{
				num = 0.0;
			}
			property.SetValue(dashboardItem, num);
			entry.Text = e.OldTextValue;
		}

		// Token: 0x06001777 RID: 6007 RVA: 0x000B485C File Offset: 0x000B2A5C
		private void numericEntry_SizeChanged(object sender, EventArgs e)
		{
			Entry entry = sender as Entry;
			DashboardItem dashboardItem = entry.BindingContext as DashboardItem;
			if (dashboardItem == null || string.IsNullOrEmpty(entry.ClassId))
			{
				return;
			}
			object value = dashboardItem.GetType().GetProperty(entry.ClassId).GetValue(dashboardItem);
			entry.Text = value.ToString();
		}

		// Token: 0x06001778 RID: 6008 RVA: 0x000B48B1 File Offset: 0x000B2AB1
		private void btnInfo_Clicked(object sender, EventArgs e)
		{
			base.DisplayAlert(Translate.GetString("ios_DashboardItemEditor"), Translate.GetString("ios_DashboardItemEditor_InfoText"), "OK");
		}

		// Token: 0x06001779 RID: 6009 RVA: 0x000B48D3 File Offset: 0x000B2AD3
		private void play_Tapped(object sender, EventArgs e)
		{
			ISoundManager soundManager = PlatformHelper.CommonService.SoundManager;
			soundManager.Filename = this._item.SoundName;
			soundManager.Play();
		}

		// Token: 0x0600177A RID: 6010 RVA: 0x000B48F6 File Offset: 0x000B2AF6
		private void playLow_Tapped(object sender, EventArgs e)
		{
			ISoundManager soundManager = PlatformHelper.CommonService.SoundManager;
			soundManager.Filename = this._item.SoundNameLow;
			soundManager.Play();
		}

		// Token: 0x0600177B RID: 6011 RVA: 0x000B4919 File Offset: 0x000B2B19
		private void pickerValueFormat_SelectedIndexChanged(object sender, EventArgs e)
		{
			this._item.ValueFormat = (sender as Picker).SelectedIndex;
		}

		// Token: 0x0600177C RID: 6012 RVA: 0x000B4934 File Offset: 0x000B2B34
		private void btnRotateGradient_Clicked(object sender, EventArgs e)
		{
			int num = (int)this._item.GradientStartPoint.X;
			int num2 = (int)this._item.GradientStartPoint.Y;
			if (num == 0 && num2 == 0)
			{
				Point point;
				point..ctor(1.0, 0.0);
				Point point2;
				point2..ctor(0.0, 1.0);
				this._item.GradientStartPoint = point;
				this._item.GradientEndPoint = point2;
				return;
			}
			if (num == 1 && num2 == 0)
			{
				Point point3;
				point3..ctor(1.0, 1.0);
				Point point4;
				point4..ctor(0.0, 0.0);
				this._item.GradientStartPoint = point3;
				this._item.GradientEndPoint = point4;
				return;
			}
			if (num == 1 && num2 == 1)
			{
				Point point5;
				point5..ctor(0.0, 1.0);
				Point point6;
				point6..ctor(1.0, 0.0);
				this._item.GradientStartPoint = point5;
				this._item.GradientEndPoint = point6;
				return;
			}
			if (num == 0 && num2 == 1)
			{
				Point point7;
				point7..ctor(0.0, 0.0);
				Point point8;
				point8..ctor(1.0, 1.0);
				this._item.GradientStartPoint = point7;
				this._item.GradientEndPoint = point8;
			}
		}

		// Token: 0x0600177D RID: 6013 RVA: 0x000B4ABC File Offset: 0x000B2CBC
		private async void btnChangeSensor_Clicked(object sender, EventArgs e)
		{
			object obj = new PID_IDToPIDConverter().Convert(this._item.PID_Id, typeof(PID), null, CultureInfo.InvariantCulture);
			if (obj != null)
			{
				if (obj != PID.Empty)
				{
					if (obj is CustomPID && CustomPIDViewModel.CurrentCustom.PidCollection.Contains(obj))
					{
						Page page = new CustomPIDsEditorPage(obj as CustomPID);
						this.PreviewGrid.Children.Clear();
						DashboardItem item = this._item;
						if (item != null)
						{
							item.Stop();
						}
						this.shouldStartItem = true;
						await base.Navigation.PushAsync(page);
					}
					else
					{
						Page page2 = new SettingsPIDOverrideEditorPageV3((IPID)obj);
						if (App.UseLegacyUI)
						{
							page2 = new SettingsPIDOverridePage((IPID)obj);
						}
						this.PreviewGrid.Children.Clear();
						DashboardItem item2 = this._item;
						if (item2 != null)
						{
							item2.Stop();
						}
						this.shouldStartItem = true;
						await base.Navigation.PushAsync(page2);
					}
				}
			}
		}

		// Token: 0x0600177E RID: 6014 RVA: 0x000B4AF4 File Offset: 0x000B2CF4
		private async void btnSelectMultiplePIDs_Clicked(object sender, EventArgs e)
		{
			DashboardMultiPidSelector dashboardMultiPidSelector = new DashboardMultiPidSelector(delegate(List<IPID> newPids)
			{
				if (newPids == null || newPids.Count == 0)
				{
					return;
				}
				this._item.PID_IDs.Clear();
				this._item.PID_IDs.AddRange(newPids.Select((IPID x) => x.Id).ToArray<int>());
				MultiplePIDViewModel multiplePIDViewModel = this._item.Model as MultiplePIDViewModel;
				if (multiplePIDViewModel != null)
				{
					multiplePIDViewModel.SetPIDs(this._item.PID_IDs);
					this._item.OverrideName = true;
					this._item.CustomName = newPids[0].ShortName;
				}
			}, this._item.PID_IDs);
			await base.Navigation.PushAsync(dashboardMultiPidSelector);
		}

		// Token: 0x0600177F RID: 6015 RVA: 0x000B4B2C File Offset: 0x000B2D2C
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private void InitializeComponent()
		{
			if (ResourceLoader.IsEnabled && ResourceLoader.CanProvideContentFor(new ResourceLoader.ResourceLoadingQuery
			{
				AssemblyName = typeof(DashboardItemEditor).GetTypeInfo().Assembly.GetName(),
				ResourcePath = "Pages/Dashboard/DashboardItemEditor.xaml",
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
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension = new DynamicResourceExtension(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\Dashboard\\DashboardItemEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 17, 5);
			PID_IDToPIDConverter pid_IDToPIDConverter;
			VisualDiagnostics.RegisterSourceInfo(pid_IDToPIDConverter = new PID_IDToPIDConverter(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\Dashboard\\DashboardItemEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 24, 14);
			PID_IDToPIDNameConverter pid_IDToPIDNameConverter;
			VisualDiagnostics.RegisterSourceInfo(pid_IDToPIDNameConverter = new PID_IDToPIDNameConverter(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\Dashboard\\DashboardItemEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 25, 14);
			DoubleToStringConverter doubleToStringConverter;
			VisualDiagnostics.RegisterSourceInfo(doubleToStringConverter = new DoubleToStringConverter(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\Dashboard\\DashboardItemEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 26, 14);
			ItemTypeToIndexConverter itemTypeToIndexConverter;
			VisualDiagnostics.RegisterSourceInfo(itemTypeToIndexConverter = new ItemTypeToIndexConverter(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\Dashboard\\DashboardItemEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 27, 14);
			TextSizeToPickerIdxConverter textSizeToPickerIdxConverter;
			VisualDiagnostics.RegisterSourceInfo(textSizeToPickerIdxConverter = new TextSizeToPickerIdxConverter(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\Dashboard\\DashboardItemEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 28, 14);
			BoolToNegativeConverter boolToNegativeConverter;
			VisualDiagnostics.RegisterSourceInfo(boolToNegativeConverter = new BoolToNegativeConverter(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\Dashboard\\DashboardItemEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 29, 14);
			ThicknessToDoubleConverter thicknessToDoubleConverter;
			VisualDiagnostics.RegisterSourceInfo(thicknessToDoubleConverter = new ThicknessToDoubleConverter(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\Dashboard\\DashboardItemEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 30, 14);
			PidIdToPidConverter pidIdToPidConverter;
			VisualDiagnostics.RegisterSourceInfo(pidIdToPidConverter = new PidIdToPidConverter(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\Dashboard\\DashboardItemEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 31, 14);
			EnumToIntConverter enumToIntConverter;
			VisualDiagnostics.RegisterSourceInfo(enumToIntConverter = new EnumToIntConverter(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\Dashboard\\DashboardItemEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 32, 14);
			DashboardItemTypes dashboardItemTypes = DashboardItemTypes.Text;
			DashboardItemTypes dashboardItemTypes2 = DashboardItemTypes.Chart;
			DashboardItemTypes dashboardItemTypes3 = DashboardItemTypes.Gauge;
			DashboardItemTypes dashboardItemTypes4 = DashboardItemTypes.LinearGauge;
			DashboardItemTypes dashboardItemTypes5 = DashboardItemTypes.TextHorizontal;
			DashboardItemTypes dashboardItemTypes6 = DashboardItemTypes.GaugeVar2;
			DashboardItemTypes dashboardItemTypes7 = DashboardItemTypes.GaugeVar3;
			DashboardItemTypes dashboardItemTypes8 = DashboardItemTypes.Action;
			EnumToBoolConverter enumToBoolConverter;
			VisualDiagnostics.RegisterSourceInfo(enumToBoolConverter = new EnumToBoolConverter(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\Dashboard\\DashboardItemEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 34, 14);
			DashboardItemTypes dashboardItemTypes9 = DashboardItemTypes.MultiPidBarChart;
			DashboardItemTypes dashboardItemTypes10 = DashboardItemTypes.MultiPidBarChart;
			EnumToBoolConverter enumToBoolConverter2;
			VisualDiagnostics.RegisterSourceInfo(enumToBoolConverter2 = new EnumToBoolConverter(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\Dashboard\\DashboardItemEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 47, 14);
			DynamicResourceExtension dynamicResourceExtension2;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension2 = new DynamicResourceExtension(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\Dashboard\\DashboardItemEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 57, 52);
			Setter setter;
			VisualDiagnostics.RegisterSourceInfo(setter = new Setter(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\Dashboard\\DashboardItemEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 57, 18);
			Setter setter2;
			VisualDiagnostics.RegisterSourceInfo(setter2 = new Setter(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\Dashboard\\DashboardItemEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 58, 18);
			Setter setter3;
			VisualDiagnostics.RegisterSourceInfo(setter3 = new Setter(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\Dashboard\\DashboardItemEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 59, 18);
			Style style;
			VisualDiagnostics.RegisterSourceInfo(style = new Style(typeof(Frame)), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\Dashboard\\DashboardItemEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 56, 14);
			DynamicResourceExtension dynamicResourceExtension3;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension3 = new DynamicResourceExtension(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\Dashboard\\DashboardItemEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 63, 49);
			Setter setter4;
			VisualDiagnostics.RegisterSourceInfo(setter4 = new Setter(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\Dashboard\\DashboardItemEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 63, 18);
			Setter setter5;
			VisualDiagnostics.RegisterSourceInfo(setter5 = new Setter(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\Dashboard\\DashboardItemEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 64, 18);
			Setter setter6;
			VisualDiagnostics.RegisterSourceInfo(setter6 = new Setter(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\Dashboard\\DashboardItemEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 65, 18);
			Setter setter7;
			VisualDiagnostics.RegisterSourceInfo(setter7 = new Setter(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\Dashboard\\DashboardItemEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 66, 18);
			Style style2;
			VisualDiagnostics.RegisterSourceInfo(style2 = new Style(typeof(Frame)), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\Dashboard\\DashboardItemEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 62, 14);
			DynamicResourceExtension dynamicResourceExtension4;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension4 = new DynamicResourceExtension(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\Dashboard\\DashboardItemEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 70, 47);
			Setter setter8;
			VisualDiagnostics.RegisterSourceInfo(setter8 = new Setter(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\Dashboard\\DashboardItemEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 70, 18);
			DynamicResourceExtension dynamicResourceExtension5;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension5 = new DynamicResourceExtension(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\Dashboard\\DashboardItemEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 71, 52);
			Setter setter9;
			VisualDiagnostics.RegisterSourceInfo(setter9 = new Setter(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\Dashboard\\DashboardItemEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 71, 18);
			Style style3;
			VisualDiagnostics.RegisterSourceInfo(style3 = new Style(typeof(ExtendedSlider)), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\Dashboard\\DashboardItemEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 69, 14);
			ResourceDictionary resourceDictionary;
			VisualDiagnostics.RegisterSourceInfo(resourceDictionary = new ResourceDictionary(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\Dashboard\\DashboardItemEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 23, 10);
			ColumnDefinition columnDefinition;
			VisualDiagnostics.RegisterSourceInfo(columnDefinition = new ColumnDefinition(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\Dashboard\\DashboardItemEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 83, 18);
			ColumnDefinition columnDefinition2;
			VisualDiagnostics.RegisterSourceInfo(columnDefinition2 = new ColumnDefinition(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\Dashboard\\DashboardItemEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 84, 18);
			ColumnDefinition columnDefinition3;
			VisualDiagnostics.RegisterSourceInfo(columnDefinition3 = new ColumnDefinition(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\Dashboard\\DashboardItemEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 85, 18);
			DynamicResourceExtension dynamicResourceExtension6;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension6 = new DynamicResourceExtension(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\Dashboard\\DashboardItemEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 92, 17);
			Translate translate;
			VisualDiagnostics.RegisterSourceInfo(translate = new Translate(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\Dashboard\\DashboardItemEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 93, 17);
			OnPlatform<bool> onPlatform;
			VisualDiagnostics.RegisterSourceInfo(onPlatform = new OnPlatform<bool>(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\Dashboard\\DashboardItemEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 95, 22);
			LinkButton linkButton;
			VisualDiagnostics.RegisterSourceInfo(linkButton = new LinkButton(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\Dashboard\\DashboardItemEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 87, 14);
			DynamicResourceExtension dynamicResourceExtension7;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension7 = new DynamicResourceExtension(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\Dashboard\\DashboardItemEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 105, 17);
			Translate translate2;
			VisualDiagnostics.RegisterSourceInfo(translate2 = new Translate(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\Dashboard\\DashboardItemEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 106, 17);
			NonScalableLabel nonScalableLabel;
			VisualDiagnostics.RegisterSourceInfo(nonScalableLabel = new NonScalableLabel(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\Dashboard\\DashboardItemEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 102, 14);
			DynamicResourceExtension dynamicResourceExtension8;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension8 = new DynamicResourceExtension(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\Dashboard\\DashboardItemEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 111, 17);
			DynamicResourceExtension dynamicResourceExtension9;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension9 = new DynamicResourceExtension(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\Dashboard\\DashboardItemEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 112, 17);
			On on;
			VisualDiagnostics.RegisterSourceInfo(on = new On(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\Dashboard\\DashboardItemEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 115, 26);
			On on2;
			VisualDiagnostics.RegisterSourceInfo(on2 = new On(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\Dashboard\\DashboardItemEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 116, 26);
			OnPlatform<Thickness> onPlatform2;
			VisualDiagnostics.RegisterSourceInfo(onPlatform2 = new OnPlatform<Thickness>(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\Dashboard\\DashboardItemEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 114, 22);
			LinkButton linkButton2;
			VisualDiagnostics.RegisterSourceInfo(linkButton2 = new LinkButton(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\Dashboard\\DashboardItemEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 107, 14);
			Grid grid;
			VisualDiagnostics.RegisterSourceInfo(grid = new Grid(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\Dashboard\\DashboardItemEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 78, 10);
			OnPlatform<Thickness> onPlatform3;
			VisualDiagnostics.RegisterSourceInfo(onPlatform3 = new OnPlatform<Thickness>(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\Dashboard\\DashboardItemEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 128, 18);
			RowDefinition rowDefinition;
			VisualDiagnostics.RegisterSourceInfo(rowDefinition = new RowDefinition(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\Dashboard\\DashboardItemEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 134, 18);
			RowDefinition rowDefinition2;
			VisualDiagnostics.RegisterSourceInfo(rowDefinition2 = new RowDefinition(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\Dashboard\\DashboardItemEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 135, 18);
			RowDefinition rowDefinition3;
			VisualDiagnostics.RegisterSourceInfo(rowDefinition3 = new RowDefinition(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\Dashboard\\DashboardItemEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 136, 18);
			RowDefinition rowDefinition4;
			VisualDiagnostics.RegisterSourceInfo(rowDefinition4 = new RowDefinition(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\Dashboard\\DashboardItemEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 145, 22);
			ColumnDefinition columnDefinition4;
			VisualDiagnostics.RegisterSourceInfo(columnDefinition4 = new ColumnDefinition(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\Dashboard\\DashboardItemEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 148, 22);
			ColumnDefinition columnDefinition5;
			VisualDiagnostics.RegisterSourceInfo(columnDefinition5 = new ColumnDefinition(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\Dashboard\\DashboardItemEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 149, 22);
			ColumnDefinition columnDefinition6;
			VisualDiagnostics.RegisterSourceInfo(columnDefinition6 = new ColumnDefinition(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\Dashboard\\DashboardItemEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 150, 22);
			Grid grid2;
			VisualDiagnostics.RegisterSourceInfo(grid2 = new Grid(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\Dashboard\\DashboardItemEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 139, 14);
			Translate translate3;
			VisualDiagnostics.RegisterSourceInfo(translate3 = new Translate(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\Dashboard\\DashboardItemEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 157, 17);
			ActivityFrame activityFrame;
			VisualDiagnostics.RegisterSourceInfo(activityFrame = new ActivityFrame(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\Dashboard\\DashboardItemEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 153, 14);
			DynamicResourceExtension dynamicResourceExtension10;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension10 = new DynamicResourceExtension(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\Dashboard\\DashboardItemEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 165, 17);
			Translate translate4;
			VisualDiagnostics.RegisterSourceInfo(translate4 = new Translate(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\Dashboard\\DashboardItemEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 171, 58);
			Label label;
			VisualDiagnostics.RegisterSourceInfo(label = new Label(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\Dashboard\\DashboardItemEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 171, 30);
			StaticResourceExtension staticResourceExtension;
			VisualDiagnostics.RegisterSourceInfo(staticResourceExtension = new StaticResourceExtension(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\Dashboard\\DashboardItemEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 178, 33);
			BindingExtension bindingExtension;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension = new BindingExtension(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\Dashboard\\DashboardItemEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 178, 33);
			StaticResourceExtension staticResourceExtension2;
			VisualDiagnostics.RegisterSourceInfo(staticResourceExtension2 = new StaticResourceExtension(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\Dashboard\\DashboardItemEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 179, 33);
			BindingExtension bindingExtension2;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension2 = new BindingExtension(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\Dashboard\\DashboardItemEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 179, 33);
			DynamicResourceExtension dynamicResourceExtension11;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension11 = new DynamicResourceExtension(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\Dashboard\\DashboardItemEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 180, 33);
			LinkButton linkButton3;
			VisualDiagnostics.RegisterSourceInfo(linkButton3 = new LinkButton(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\Dashboard\\DashboardItemEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 172, 30);
			StaticResourceExtension staticResourceExtension3;
			VisualDiagnostics.RegisterSourceInfo(staticResourceExtension3 = new StaticResourceExtension(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\Dashboard\\DashboardItemEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 188, 33);
			BindingExtension bindingExtension3;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension3 = new BindingExtension(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\Dashboard\\DashboardItemEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 188, 33);
			Translate translate5;
			VisualDiagnostics.RegisterSourceInfo(translate5 = new Translate(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\Dashboard\\DashboardItemEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 189, 33);
			DynamicResourceExtension dynamicResourceExtension12;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension12 = new DynamicResourceExtension(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\Dashboard\\DashboardItemEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 190, 33);
			LinkButton linkButton4;
			VisualDiagnostics.RegisterSourceInfo(linkButton4 = new LinkButton(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\Dashboard\\DashboardItemEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 182, 30);
			BindingExtension bindingExtension4;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension4 = new BindingExtension(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\Dashboard\\DashboardItemEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 195, 33);
			Translate translate6;
			VisualDiagnostics.RegisterSourceInfo(translate6 = new Translate(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\Dashboard\\DashboardItemEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 196, 33);
			LabelSwitch labelSwitch;
			VisualDiagnostics.RegisterSourceInfo(labelSwitch = new LabelSwitch(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\Dashboard\\DashboardItemEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 193, 30);
			BindingExtension bindingExtension5;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension5 = new BindingExtension(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\Dashboard\\DashboardItemEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 197, 36);
			BindingExtension bindingExtension6;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension6 = new BindingExtension(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\Dashboard\\DashboardItemEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 197, 84);
			Entry entry;
			VisualDiagnostics.RegisterSourceInfo(entry = new Entry(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\Dashboard\\DashboardItemEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 197, 30);
			Translate translate7;
			VisualDiagnostics.RegisterSourceInfo(translate7 = new Translate(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\Dashboard\\DashboardItemEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 202, 33);
			Button button;
			VisualDiagnostics.RegisterSourceInfo(button = new Button(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\Dashboard\\DashboardItemEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 199, 30);
			StackLayout stackLayout;
			VisualDiagnostics.RegisterSourceInfo(stackLayout = new StackLayout(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\Dashboard\\DashboardItemEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 170, 26);
			Frame frame;
			VisualDiagnostics.RegisterSourceInfo(frame = new Frame(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\Dashboard\\DashboardItemEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 169, 22);
			ColumnDefinition columnDefinition7;
			VisualDiagnostics.RegisterSourceInfo(columnDefinition7 = new ColumnDefinition(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\Dashboard\\DashboardItemEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 213, 38);
			ColumnDefinition columnDefinition8;
			VisualDiagnostics.RegisterSourceInfo(columnDefinition8 = new ColumnDefinition(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\Dashboard\\DashboardItemEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 214, 38);
			Translate translate8;
			VisualDiagnostics.RegisterSourceInfo(translate8 = new Translate(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\Dashboard\\DashboardItemEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 219, 37);
			Label label2;
			VisualDiagnostics.RegisterSourceInfo(label2 = new Label(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\Dashboard\\DashboardItemEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 216, 34);
			List<string> dashboardItemTypesList;
			VisualDiagnostics.RegisterSourceInfo(dashboardItemTypesList = StaticLists.DashboardItemTypesList, new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\Dashboard\\DashboardItemEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 224, 37);
			BindingExtension bindingExtension7;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension7 = new BindingExtension(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\Dashboard\\DashboardItemEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 224, 37);
			StaticResourceExtension staticResourceExtension4;
			VisualDiagnostics.RegisterSourceInfo(staticResourceExtension4 = new StaticResourceExtension(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\Dashboard\\DashboardItemEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 225, 37);
			BindingExtension bindingExtension8;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension8 = new BindingExtension(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\Dashboard\\DashboardItemEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 225, 37);
			Picker picker;
			VisualDiagnostics.RegisterSourceInfo(picker = new Picker(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\Dashboard\\DashboardItemEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 222, 34);
			Grid grid3;
			VisualDiagnostics.RegisterSourceInfo(grid3 = new Grid(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\Dashboard\\DashboardItemEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 211, 30);
			StackLayout stackLayout2;
			VisualDiagnostics.RegisterSourceInfo(stackLayout2 = new StackLayout(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\Dashboard\\DashboardItemEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 210, 26);
			Frame frame2;
			VisualDiagnostics.RegisterSourceInfo(frame2 = new Frame(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\Dashboard\\DashboardItemEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 209, 22);
			DynamicResourceExtension dynamicResourceExtension13;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension13 = new DynamicResourceExtension(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\Dashboard\\DashboardItemEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 243, 25);
			DynamicResourceExtension dynamicResourceExtension14;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension14 = new DynamicResourceExtension(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\Dashboard\\DashboardItemEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 244, 25);
			Translate translate9;
			VisualDiagnostics.RegisterSourceInfo(translate9 = new Translate(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\Dashboard\\DashboardItemEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 251, 37);
			DynamicResourceExtension dynamicResourceExtension15;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension15 = new DynamicResourceExtension(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\Dashboard\\DashboardItemEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 252, 37);
			Label label3;
			VisualDiagnostics.RegisterSourceInfo(label3 = new Label(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\Dashboard\\DashboardItemEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 248, 34);
			Grid grid4;
			VisualDiagnostics.RegisterSourceInfo(grid4 = new Grid(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\Dashboard\\DashboardItemEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 247, 30);
			BindingExtension bindingExtension9;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension9 = new BindingExtension(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\Dashboard\\DashboardItemEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 263, 56);
			Translate translate10;
			VisualDiagnostics.RegisterSourceInfo(translate10 = new Translate(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\Dashboard\\DashboardItemEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 263, 113);
			LabelSwitch labelSwitch2;
			VisualDiagnostics.RegisterSourceInfo(labelSwitch2 = new LabelSwitch(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\Dashboard\\DashboardItemEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 263, 38);
			StaticResourceExtension staticResourceExtension5;
			VisualDiagnostics.RegisterSourceInfo(staticResourceExtension5 = new StaticResourceExtension(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\Dashboard\\DashboardItemEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 270, 41);
			BindingExtension bindingExtension10;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension10 = new BindingExtension(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\Dashboard\\DashboardItemEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 270, 41);
			Translate translate11;
			VisualDiagnostics.RegisterSourceInfo(translate11 = new Translate(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\Dashboard\\DashboardItemEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 272, 41);
			BindingExtension bindingExtension11;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension11 = new BindingExtension(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\Dashboard\\DashboardItemEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 273, 41);
			DashboardItemColorBox dashboardItemColorBox;
			VisualDiagnostics.RegisterSourceInfo(dashboardItemColorBox = new DashboardItemColorBox(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\Dashboard\\DashboardItemEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 269, 38);
			BindingExtension bindingExtension12;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension12 = new BindingExtension(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\Dashboard\\DashboardItemEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 274, 50);
			Translate translate12;
			VisualDiagnostics.RegisterSourceInfo(translate12 = new Translate(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\Dashboard\\DashboardItemEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 275, 48);
			Label label4;
			VisualDiagnostics.RegisterSourceInfo(label4 = new Label(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\Dashboard\\DashboardItemEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 275, 42);
			BindingExtension bindingExtension13;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension13 = new BindingExtension(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\Dashboard\\DashboardItemEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 280, 45);
			DashboardItemColorBox dashboardItemColorBox2;
			VisualDiagnostics.RegisterSourceInfo(dashboardItemColorBox2 = new DashboardItemColorBox(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\Dashboard\\DashboardItemEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 276, 42);
			BindingExtension bindingExtension14;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension14 = new BindingExtension(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\Dashboard\\DashboardItemEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 285, 45);
			DashboardItemColorBox dashboardItemColorBox3;
			VisualDiagnostics.RegisterSourceInfo(dashboardItemColorBox3 = new DashboardItemColorBox(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\Dashboard\\DashboardItemEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 281, 42);
			Translate translate13;
			VisualDiagnostics.RegisterSourceInfo(translate13 = new Translate(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\Dashboard\\DashboardItemEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 286, 48);
			Label label5;
			VisualDiagnostics.RegisterSourceInfo(label5 = new Label(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\Dashboard\\DashboardItemEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 286, 42);
			BindingExtension bindingExtension15;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension15 = new BindingExtension(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\Dashboard\\DashboardItemEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 296, 45);
			ExtendedSlider extendedSlider;
			VisualDiagnostics.RegisterSourceInfo(extendedSlider = new ExtendedSlider(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\Dashboard\\DashboardItemEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 289, 42);
			Label label6;
			VisualDiagnostics.RegisterSourceInfo(label6 = new Label(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\Dashboard\\DashboardItemEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 297, 42);
			BindingExtension bindingExtension16;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension16 = new BindingExtension(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\Dashboard\\DashboardItemEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 306, 45);
			ExtendedSlider extendedSlider2;
			VisualDiagnostics.RegisterSourceInfo(extendedSlider2 = new ExtendedSlider(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\Dashboard\\DashboardItemEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 299, 42);
			Translate translate14;
			VisualDiagnostics.RegisterSourceInfo(translate14 = new Translate(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\Dashboard\\DashboardItemEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 310, 45);
			Button button2;
			VisualDiagnostics.RegisterSourceInfo(button2 = new Button(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\Dashboard\\DashboardItemEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 307, 42);
			StackLayout stackLayout3;
			VisualDiagnostics.RegisterSourceInfo(stackLayout3 = new StackLayout(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\Dashboard\\DashboardItemEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 274, 38);
			Translate translate15;
			VisualDiagnostics.RegisterSourceInfo(translate15 = new Translate(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\Dashboard\\DashboardItemEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 317, 41);
			BindingExtension bindingExtension17;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension17 = new BindingExtension(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\Dashboard\\DashboardItemEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 318, 41);
			DashboardItemColorBox dashboardItemColorBox4;
			VisualDiagnostics.RegisterSourceInfo(dashboardItemColorBox4 = new DashboardItemColorBox(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\Dashboard\\DashboardItemEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 315, 38);
			ColumnDefinition columnDefinition9;
			VisualDiagnostics.RegisterSourceInfo(columnDefinition9 = new ColumnDefinition(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\Dashboard\\DashboardItemEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 326, 46);
			ColumnDefinition columnDefinition10;
			VisualDiagnostics.RegisterSourceInfo(columnDefinition10 = new ColumnDefinition(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\Dashboard\\DashboardItemEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 327, 46);
			RowDefinition rowDefinition5;
			VisualDiagnostics.RegisterSourceInfo(rowDefinition5 = new RowDefinition(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\Dashboard\\DashboardItemEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 330, 46);
			RowDefinition rowDefinition6;
			VisualDiagnostics.RegisterSourceInfo(rowDefinition6 = new RowDefinition(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\Dashboard\\DashboardItemEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 331, 46);
			Translate translate16;
			VisualDiagnostics.RegisterSourceInfo(translate16 = new Translate(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\Dashboard\\DashboardItemEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 336, 45);
			Label label7;
			VisualDiagnostics.RegisterSourceInfo(label7 = new Label(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\Dashboard\\DashboardItemEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 333, 42);
			BindingExtension bindingExtension18;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension18 = new BindingExtension(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\Dashboard\\DashboardItemEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 341, 45);
			Label label8;
			VisualDiagnostics.RegisterSourceInfo(label8 = new Label(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\Dashboard\\DashboardItemEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 337, 42);
			BindingExtension bindingExtension19;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension19 = new BindingExtension(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\Dashboard\\DashboardItemEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 350, 45);
			ExtendedSlider extendedSlider3;
			VisualDiagnostics.RegisterSourceInfo(extendedSlider3 = new ExtendedSlider(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\Dashboard\\DashboardItemEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 343, 42);
			Grid grid5;
			VisualDiagnostics.RegisterSourceInfo(grid5 = new Grid(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\Dashboard\\DashboardItemEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 324, 38);
			ColumnDefinition columnDefinition11;
			VisualDiagnostics.RegisterSourceInfo(columnDefinition11 = new ColumnDefinition(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\Dashboard\\DashboardItemEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 356, 46);
			ColumnDefinition columnDefinition12;
			VisualDiagnostics.RegisterSourceInfo(columnDefinition12 = new ColumnDefinition(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\Dashboard\\DashboardItemEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 357, 46);
			RowDefinition rowDefinition7;
			VisualDiagnostics.RegisterSourceInfo(rowDefinition7 = new RowDefinition(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\Dashboard\\DashboardItemEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 360, 46);
			RowDefinition rowDefinition8;
			VisualDiagnostics.RegisterSourceInfo(rowDefinition8 = new RowDefinition(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\Dashboard\\DashboardItemEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 361, 46);
			Translate translate17;
			VisualDiagnostics.RegisterSourceInfo(translate17 = new Translate(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\Dashboard\\DashboardItemEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 366, 45);
			Label label9;
			VisualDiagnostics.RegisterSourceInfo(label9 = new Label(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\Dashboard\\DashboardItemEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 363, 42);
			StaticResourceExtension staticResourceExtension6;
			VisualDiagnostics.RegisterSourceInfo(staticResourceExtension6 = new StaticResourceExtension(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\Dashboard\\DashboardItemEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 371, 45);
			BindingExtension bindingExtension20;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension20 = new BindingExtension(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\Dashboard\\DashboardItemEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 371, 45);
			Label label10;
			VisualDiagnostics.RegisterSourceInfo(label10 = new Label(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\Dashboard\\DashboardItemEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 367, 42);
			StaticResourceExtension staticResourceExtension7;
			VisualDiagnostics.RegisterSourceInfo(staticResourceExtension7 = new StaticResourceExtension(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\Dashboard\\DashboardItemEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 380, 45);
			BindingExtension bindingExtension21;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension21 = new BindingExtension(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\Dashboard\\DashboardItemEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 380, 45);
			ExtendedSlider extendedSlider4;
			VisualDiagnostics.RegisterSourceInfo(extendedSlider4 = new ExtendedSlider(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\Dashboard\\DashboardItemEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 373, 42);
			Grid grid6;
			VisualDiagnostics.RegisterSourceInfo(grid6 = new Grid(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\Dashboard\\DashboardItemEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 354, 38);
			StackLayout stackLayout4;
			VisualDiagnostics.RegisterSourceInfo(stackLayout4 = new StackLayout(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\Dashboard\\DashboardItemEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 259, 34);
			Frame frame3;
			VisualDiagnostics.RegisterSourceInfo(frame3 = new Frame(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\Dashboard\\DashboardItemEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 258, 30);
			SfExpander sfExpander;
			VisualDiagnostics.RegisterSourceInfo(sfExpander = new SfExpander(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\Dashboard\\DashboardItemEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 239, 22);
			DynamicResourceExtension dynamicResourceExtension16;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension16 = new DynamicResourceExtension(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\Dashboard\\DashboardItemEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 397, 25);
			DynamicResourceExtension dynamicResourceExtension17;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension17 = new DynamicResourceExtension(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\Dashboard\\DashboardItemEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 398, 25);
			Translate translate18;
			VisualDiagnostics.RegisterSourceInfo(translate18 = new Translate(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\Dashboard\\DashboardItemEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 405, 37);
			DynamicResourceExtension dynamicResourceExtension18;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension18 = new DynamicResourceExtension(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\Dashboard\\DashboardItemEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 406, 37);
			Label label11;
			VisualDiagnostics.RegisterSourceInfo(label11 = new Label(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\Dashboard\\DashboardItemEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 402, 34);
			Grid grid7;
			VisualDiagnostics.RegisterSourceInfo(grid7 = new Grid(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\Dashboard\\DashboardItemEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 401, 30);
			Translate translate19;
			VisualDiagnostics.RegisterSourceInfo(translate19 = new Translate(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\Dashboard\\DashboardItemEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 419, 41);
			BindingExtension bindingExtension22;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension22 = new BindingExtension(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\Dashboard\\DashboardItemEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 420, 41);
			DashboardItemColorBox dashboardItemColorBox5;
			VisualDiagnostics.RegisterSourceInfo(dashboardItemColorBox5 = new DashboardItemColorBox(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\Dashboard\\DashboardItemEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 417, 38);
			ColumnDefinition columnDefinition13;
			VisualDiagnostics.RegisterSourceInfo(columnDefinition13 = new ColumnDefinition(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\Dashboard\\DashboardItemEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 425, 46);
			ColumnDefinition columnDefinition14;
			VisualDiagnostics.RegisterSourceInfo(columnDefinition14 = new ColumnDefinition(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\Dashboard\\DashboardItemEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 426, 46);
			RowDefinition rowDefinition9;
			VisualDiagnostics.RegisterSourceInfo(rowDefinition9 = new RowDefinition(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\Dashboard\\DashboardItemEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 429, 46);
			RowDefinition rowDefinition10;
			VisualDiagnostics.RegisterSourceInfo(rowDefinition10 = new RowDefinition(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\Dashboard\\DashboardItemEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 430, 46);
			Translate translate20;
			VisualDiagnostics.RegisterSourceInfo(translate20 = new Translate(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\Dashboard\\DashboardItemEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 435, 45);
			Label label12;
			VisualDiagnostics.RegisterSourceInfo(label12 = new Label(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\Dashboard\\DashboardItemEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 432, 42);
			BindingExtension bindingExtension23;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension23 = new BindingExtension(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\Dashboard\\DashboardItemEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 440, 45);
			Label label13;
			VisualDiagnostics.RegisterSourceInfo(label13 = new Label(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\Dashboard\\DashboardItemEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 436, 42);
			BindingExtension bindingExtension24;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension24 = new BindingExtension(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\Dashboard\\DashboardItemEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 449, 45);
			ExtendedSlider extendedSlider5;
			VisualDiagnostics.RegisterSourceInfo(extendedSlider5 = new ExtendedSlider(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\Dashboard\\DashboardItemEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 442, 42);
			Grid grid8;
			VisualDiagnostics.RegisterSourceInfo(grid8 = new Grid(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\Dashboard\\DashboardItemEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 423, 38);
			Translate translate21;
			VisualDiagnostics.RegisterSourceInfo(translate21 = new Translate(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\Dashboard\\DashboardItemEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 455, 41);
			BindingExtension bindingExtension25;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension25 = new BindingExtension(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\Dashboard\\DashboardItemEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 456, 41);
			DashboardItemColorBox dashboardItemColorBox6;
			VisualDiagnostics.RegisterSourceInfo(dashboardItemColorBox6 = new DashboardItemColorBox(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\Dashboard\\DashboardItemEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 453, 38);
			ColumnDefinition columnDefinition15;
			VisualDiagnostics.RegisterSourceInfo(columnDefinition15 = new ColumnDefinition(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\Dashboard\\DashboardItemEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 460, 46);
			ColumnDefinition columnDefinition16;
			VisualDiagnostics.RegisterSourceInfo(columnDefinition16 = new ColumnDefinition(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\Dashboard\\DashboardItemEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 461, 46);
			RowDefinition rowDefinition11;
			VisualDiagnostics.RegisterSourceInfo(rowDefinition11 = new RowDefinition(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\Dashboard\\DashboardItemEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 464, 46);
			RowDefinition rowDefinition12;
			VisualDiagnostics.RegisterSourceInfo(rowDefinition12 = new RowDefinition(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\Dashboard\\DashboardItemEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 465, 46);
			Translate translate22;
			VisualDiagnostics.RegisterSourceInfo(translate22 = new Translate(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\Dashboard\\DashboardItemEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 470, 45);
			Label label14;
			VisualDiagnostics.RegisterSourceInfo(label14 = new Label(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\Dashboard\\DashboardItemEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 467, 42);
			BindingExtension bindingExtension26;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension26 = new BindingExtension(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\Dashboard\\DashboardItemEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 475, 45);
			Label label15;
			VisualDiagnostics.RegisterSourceInfo(label15 = new Label(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\Dashboard\\DashboardItemEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 471, 42);
			BindingExtension bindingExtension27;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension27 = new BindingExtension(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\Dashboard\\DashboardItemEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 484, 45);
			ExtendedSlider extendedSlider6;
			VisualDiagnostics.RegisterSourceInfo(extendedSlider6 = new ExtendedSlider(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\Dashboard\\DashboardItemEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 477, 42);
			Grid grid9;
			VisualDiagnostics.RegisterSourceInfo(grid9 = new Grid(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\Dashboard\\DashboardItemEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 458, 38);
			BindingExtension bindingExtension28;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension28 = new BindingExtension(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\Dashboard\\DashboardItemEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 487, 56);
			Translate translate23;
			VisualDiagnostics.RegisterSourceInfo(translate23 = new Translate(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\Dashboard\\DashboardItemEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 487, 107);
			LabelSwitch labelSwitch3;
			VisualDiagnostics.RegisterSourceInfo(labelSwitch3 = new LabelSwitch(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\Dashboard\\DashboardItemEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 487, 38);
			Translate translate24;
			VisualDiagnostics.RegisterSourceInfo(translate24 = new Translate(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\Dashboard\\DashboardItemEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 490, 44);
			Label label16;
			VisualDiagnostics.RegisterSourceInfo(label16 = new Label(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\Dashboard\\DashboardItemEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 490, 38);
			List<string> doubleFormats;
			VisualDiagnostics.RegisterSourceInfo(doubleFormats = StaticLists.DoubleFormats, new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\Dashboard\\DashboardItemEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 491, 45);
			BindingExtension bindingExtension29;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension29 = new BindingExtension(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\Dashboard\\DashboardItemEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 491, 45);
			BindingExtension bindingExtension30;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension30 = new BindingExtension(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\Dashboard\\DashboardItemEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 491, 119);
			Picker picker2;
			VisualDiagnostics.RegisterSourceInfo(picker2 = new Picker(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\Dashboard\\DashboardItemEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 491, 38);
			Translate translate25;
			VisualDiagnostics.RegisterSourceInfo(translate25 = new Translate(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\Dashboard\\DashboardItemEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 496, 41);
			BindingExtension bindingExtension31;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension31 = new BindingExtension(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\Dashboard\\DashboardItemEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 497, 41);
			DashboardItemColorBox dashboardItemColorBox7;
			VisualDiagnostics.RegisterSourceInfo(dashboardItemColorBox7 = new DashboardItemColorBox(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\Dashboard\\DashboardItemEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 494, 38);
			ColumnDefinition columnDefinition17;
			VisualDiagnostics.RegisterSourceInfo(columnDefinition17 = new ColumnDefinition(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\Dashboard\\DashboardItemEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 502, 46);
			ColumnDefinition columnDefinition18;
			VisualDiagnostics.RegisterSourceInfo(columnDefinition18 = new ColumnDefinition(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\Dashboard\\DashboardItemEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 503, 46);
			RowDefinition rowDefinition13;
			VisualDiagnostics.RegisterSourceInfo(rowDefinition13 = new RowDefinition(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\Dashboard\\DashboardItemEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 506, 46);
			RowDefinition rowDefinition14;
			VisualDiagnostics.RegisterSourceInfo(rowDefinition14 = new RowDefinition(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\Dashboard\\DashboardItemEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 507, 46);
			Translate translate26;
			VisualDiagnostics.RegisterSourceInfo(translate26 = new Translate(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\Dashboard\\DashboardItemEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 512, 45);
			Label label17;
			VisualDiagnostics.RegisterSourceInfo(label17 = new Label(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\Dashboard\\DashboardItemEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 509, 42);
			BindingExtension bindingExtension32;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension32 = new BindingExtension(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\Dashboard\\DashboardItemEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 517, 45);
			Label label18;
			VisualDiagnostics.RegisterSourceInfo(label18 = new Label(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\Dashboard\\DashboardItemEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 513, 42);
			BindingExtension bindingExtension33;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension33 = new BindingExtension(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\Dashboard\\DashboardItemEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 526, 45);
			ExtendedSlider extendedSlider7;
			VisualDiagnostics.RegisterSourceInfo(extendedSlider7 = new ExtendedSlider(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\Dashboard\\DashboardItemEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 519, 42);
			Grid grid10;
			VisualDiagnostics.RegisterSourceInfo(grid10 = new Grid(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\Dashboard\\DashboardItemEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 500, 38);
			StackLayout stackLayout5;
			VisualDiagnostics.RegisterSourceInfo(stackLayout5 = new StackLayout(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\Dashboard\\DashboardItemEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 414, 34);
			Frame frame4;
			VisualDiagnostics.RegisterSourceInfo(frame4 = new Frame(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\Dashboard\\DashboardItemEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 413, 30);
			SfExpander sfExpander2;
			VisualDiagnostics.RegisterSourceInfo(sfExpander2 = new SfExpander(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\Dashboard\\DashboardItemEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 393, 22);
			DynamicResourceExtension dynamicResourceExtension19;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension19 = new DynamicResourceExtension(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\Dashboard\\DashboardItemEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 542, 25);
			DynamicResourceExtension dynamicResourceExtension20;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension20 = new DynamicResourceExtension(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\Dashboard\\DashboardItemEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 543, 25);
			Translate translate27;
			VisualDiagnostics.RegisterSourceInfo(translate27 = new Translate(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\Dashboard\\DashboardItemEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 550, 37);
			DynamicResourceExtension dynamicResourceExtension21;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension21 = new DynamicResourceExtension(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\Dashboard\\DashboardItemEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 551, 37);
			Label label19;
			VisualDiagnostics.RegisterSourceInfo(label19 = new Label(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\Dashboard\\DashboardItemEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 547, 34);
			Grid grid11;
			VisualDiagnostics.RegisterSourceInfo(grid11 = new Grid(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\Dashboard\\DashboardItemEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 546, 30);
			Translate translate28;
			VisualDiagnostics.RegisterSourceInfo(translate28 = new Translate(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\Dashboard\\DashboardItemEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 559, 44);
			Label label20;
			VisualDiagnostics.RegisterSourceInfo(label20 = new Label(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\Dashboard\\DashboardItemEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 559, 38);
			BindingExtension bindingExtension34;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension34 = new BindingExtension(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\Dashboard\\DashboardItemEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 560, 56);
			NumericEntryV3 numericEntryV;
			VisualDiagnostics.RegisterSourceInfo(numericEntryV = new NumericEntryV3(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\Dashboard\\DashboardItemEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 560, 38);
			Translate translate29;
			VisualDiagnostics.RegisterSourceInfo(translate29 = new Translate(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\Dashboard\\DashboardItemEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 561, 44);
			Label label21;
			VisualDiagnostics.RegisterSourceInfo(label21 = new Label(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\Dashboard\\DashboardItemEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 561, 38);
			BindingExtension bindingExtension35;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension35 = new BindingExtension(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\Dashboard\\DashboardItemEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 562, 56);
			NumericEntryV3 numericEntryV2;
			VisualDiagnostics.RegisterSourceInfo(numericEntryV2 = new NumericEntryV3(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\Dashboard\\DashboardItemEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 562, 38);
			BindingExtension bindingExtension36;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension36 = new BindingExtension(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\Dashboard\\DashboardItemEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 566, 56);
			Translate translate30;
			VisualDiagnostics.RegisterSourceInfo(translate30 = new Translate(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\Dashboard\\DashboardItemEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 566, 109);
			LabelSwitch labelSwitch4;
			VisualDiagnostics.RegisterSourceInfo(labelSwitch4 = new LabelSwitch(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\Dashboard\\DashboardItemEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 566, 38);
			BindingExtension bindingExtension37;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension37 = new BindingExtension(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\Dashboard\\DashboardItemEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 567, 50);
			Translate translate31;
			VisualDiagnostics.RegisterSourceInfo(translate31 = new Translate(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\Dashboard\\DashboardItemEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 568, 48);
			Label label22;
			VisualDiagnostics.RegisterSourceInfo(label22 = new Label(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\Dashboard\\DashboardItemEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 568, 42);
			BindingExtension bindingExtension38;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension38 = new BindingExtension(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\Dashboard\\DashboardItemEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 569, 60);
			NumericEntryV3 numericEntryV3;
			VisualDiagnostics.RegisterSourceInfo(numericEntryV3 = new NumericEntryV3(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\Dashboard\\DashboardItemEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 569, 42);
			Translate translate32;
			VisualDiagnostics.RegisterSourceInfo(translate32 = new Translate(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\Dashboard\\DashboardItemEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 571, 48);
			Label label23;
			VisualDiagnostics.RegisterSourceInfo(label23 = new Label(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\Dashboard\\DashboardItemEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 571, 42);
			BindingExtension bindingExtension39;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension39 = new BindingExtension(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\Dashboard\\DashboardItemEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 572, 60);
			NumericEntryV3 numericEntryV4;
			VisualDiagnostics.RegisterSourceInfo(numericEntryV4 = new NumericEntryV3(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\Dashboard\\DashboardItemEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 572, 42);
			Translate translate33;
			VisualDiagnostics.RegisterSourceInfo(translate33 = new Translate(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\Dashboard\\DashboardItemEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 576, 45);
			BindingExtension bindingExtension40;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension40 = new BindingExtension(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\Dashboard\\DashboardItemEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 577, 45);
			DashboardItemColorBox dashboardItemColorBox8;
			VisualDiagnostics.RegisterSourceInfo(dashboardItemColorBox8 = new DashboardItemColorBox(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\Dashboard\\DashboardItemEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 573, 42);
			StackLayout stackLayout6;
			VisualDiagnostics.RegisterSourceInfo(stackLayout6 = new StackLayout(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\Dashboard\\DashboardItemEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 567, 38);
			BindingExtension bindingExtension41;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension41 = new BindingExtension(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\Dashboard\\DashboardItemEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 582, 56);
			Translate translate34;
			VisualDiagnostics.RegisterSourceInfo(translate34 = new Translate(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\Dashboard\\DashboardItemEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 582, 108);
			LabelSwitch labelSwitch5;
			VisualDiagnostics.RegisterSourceInfo(labelSwitch5 = new LabelSwitch(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\Dashboard\\DashboardItemEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 582, 38);
			BindingExtension bindingExtension42;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension42 = new BindingExtension(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\Dashboard\\DashboardItemEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 583, 50);
			Translate translate35;
			VisualDiagnostics.RegisterSourceInfo(translate35 = new Translate(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\Dashboard\\DashboardItemEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 584, 48);
			Label label24;
			VisualDiagnostics.RegisterSourceInfo(label24 = new Label(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\Dashboard\\DashboardItemEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 584, 42);
			BindingExtension bindingExtension43;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension43 = new BindingExtension(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\Dashboard\\DashboardItemEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 585, 60);
			NumericEntryV3 numericEntryV5;
			VisualDiagnostics.RegisterSourceInfo(numericEntryV5 = new NumericEntryV3(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\Dashboard\\DashboardItemEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 585, 42);
			Translate translate36;
			VisualDiagnostics.RegisterSourceInfo(translate36 = new Translate(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\Dashboard\\DashboardItemEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 587, 48);
			Label label25;
			VisualDiagnostics.RegisterSourceInfo(label25 = new Label(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\Dashboard\\DashboardItemEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 587, 42);
			BindingExtension bindingExtension44;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension44 = new BindingExtension(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\Dashboard\\DashboardItemEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 588, 60);
			NumericEntryV3 numericEntryV6;
			VisualDiagnostics.RegisterSourceInfo(numericEntryV6 = new NumericEntryV3(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\Dashboard\\DashboardItemEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 588, 42);
			Translate translate37;
			VisualDiagnostics.RegisterSourceInfo(translate37 = new Translate(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\Dashboard\\DashboardItemEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 592, 45);
			BindingExtension bindingExtension45;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension45 = new BindingExtension(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\Dashboard\\DashboardItemEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 593, 45);
			DashboardItemColorBox dashboardItemColorBox9;
			VisualDiagnostics.RegisterSourceInfo(dashboardItemColorBox9 = new DashboardItemColorBox(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\Dashboard\\DashboardItemEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 589, 42);
			StackLayout stackLayout7;
			VisualDiagnostics.RegisterSourceInfo(stackLayout7 = new StackLayout(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\Dashboard\\DashboardItemEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 583, 38);
			Translate translate38;
			VisualDiagnostics.RegisterSourceInfo(translate38 = new Translate(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\Dashboard\\DashboardItemEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 599, 41);
			BindingExtension bindingExtension46;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension46 = new BindingExtension(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\Dashboard\\DashboardItemEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 600, 41);
			DashboardItemColorBox dashboardItemColorBox10;
			VisualDiagnostics.RegisterSourceInfo(dashboardItemColorBox10 = new DashboardItemColorBox(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\Dashboard\\DashboardItemEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 596, 38);
			Translate translate39;
			VisualDiagnostics.RegisterSourceInfo(translate39 = new Translate(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\Dashboard\\DashboardItemEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 605, 41);
			BindingExtension bindingExtension47;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension47 = new BindingExtension(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\Dashboard\\DashboardItemEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 606, 41);
			DashboardItemColorBox dashboardItemColorBox11;
			VisualDiagnostics.RegisterSourceInfo(dashboardItemColorBox11 = new DashboardItemColorBox(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\Dashboard\\DashboardItemEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 602, 38);
			Translate translate40;
			VisualDiagnostics.RegisterSourceInfo(translate40 = new Translate(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\Dashboard\\DashboardItemEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 613, 41);
			BindingExtension bindingExtension48;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension48 = new BindingExtension(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\Dashboard\\DashboardItemEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 614, 41);
			DashboardItemColorBox dashboardItemColorBox12;
			VisualDiagnostics.RegisterSourceInfo(dashboardItemColorBox12 = new DashboardItemColorBox(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\Dashboard\\DashboardItemEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 610, 38);
			Translate translate41;
			VisualDiagnostics.RegisterSourceInfo(translate41 = new Translate(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\Dashboard\\DashboardItemEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 619, 41);
			BindingExtension bindingExtension49;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension49 = new BindingExtension(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\Dashboard\\DashboardItemEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 620, 41);
			DashboardItemColorBox dashboardItemColorBox13;
			VisualDiagnostics.RegisterSourceInfo(dashboardItemColorBox13 = new DashboardItemColorBox(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\Dashboard\\DashboardItemEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 616, 38);
			Translate translate42;
			VisualDiagnostics.RegisterSourceInfo(translate42 = new Translate(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\Dashboard\\DashboardItemEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 627, 41);
			BindingExtension bindingExtension50;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension50 = new BindingExtension(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\Dashboard\\DashboardItemEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 628, 41);
			DashboardItemColorBox dashboardItemColorBox14;
			VisualDiagnostics.RegisterSourceInfo(dashboardItemColorBox14 = new DashboardItemColorBox(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\Dashboard\\DashboardItemEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 624, 38);
			StackLayout stackLayout8;
			VisualDiagnostics.RegisterSourceInfo(stackLayout8 = new StackLayout(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\Dashboard\\DashboardItemEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 558, 34);
			Frame frame5;
			VisualDiagnostics.RegisterSourceInfo(frame5 = new Frame(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\Dashboard\\DashboardItemEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 557, 30);
			SfExpander sfExpander3;
			VisualDiagnostics.RegisterSourceInfo(sfExpander3 = new SfExpander(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\Dashboard\\DashboardItemEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 538, 22);
			DynamicResourceExtension dynamicResourceExtension22;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension22 = new DynamicResourceExtension(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\Dashboard\\DashboardItemEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 642, 25);
			DynamicResourceExtension dynamicResourceExtension23;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension23 = new DynamicResourceExtension(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\Dashboard\\DashboardItemEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 643, 25);
			Translate translate43;
			VisualDiagnostics.RegisterSourceInfo(translate43 = new Translate(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\Dashboard\\DashboardItemEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 650, 37);
			DynamicResourceExtension dynamicResourceExtension24;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension24 = new DynamicResourceExtension(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\Dashboard\\DashboardItemEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 651, 37);
			Label label26;
			VisualDiagnostics.RegisterSourceInfo(label26 = new Label(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\Dashboard\\DashboardItemEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 647, 34);
			Grid grid12;
			VisualDiagnostics.RegisterSourceInfo(grid12 = new Grid(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\Dashboard\\DashboardItemEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 646, 30);
			Label label27;
			VisualDiagnostics.RegisterSourceInfo(label27 = new Label(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\Dashboard\\DashboardItemEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 659, 38);
			BindingExtension bindingExtension51;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension51 = new BindingExtension(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\Dashboard\\DashboardItemEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 660, 102);
			NumericEntryV3 numericEntryV7;
			VisualDiagnostics.RegisterSourceInfo(numericEntryV7 = new NumericEntryV3(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\Dashboard\\DashboardItemEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 660, 38);
			Translate translate44;
			VisualDiagnostics.RegisterSourceInfo(translate44 = new Translate(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\Dashboard\\DashboardItemEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 662, 44);
			Label label28;
			VisualDiagnostics.RegisterSourceInfo(label28 = new Label(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\Dashboard\\DashboardItemEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 662, 38);
			BindingExtension bindingExtension52;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension52 = new BindingExtension(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\Dashboard\\DashboardItemEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 663, 56);
			NumericEntryV3 numericEntryV8;
			VisualDiagnostics.RegisterSourceInfo(numericEntryV8 = new NumericEntryV3(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\Dashboard\\DashboardItemEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 663, 38);
			Translate translate45;
			VisualDiagnostics.RegisterSourceInfo(translate45 = new Translate(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\Dashboard\\DashboardItemEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 664, 44);
			Label label29;
			VisualDiagnostics.RegisterSourceInfo(label29 = new Label(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\Dashboard\\DashboardItemEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 664, 38);
			BindingExtension bindingExtension53;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension53 = new BindingExtension(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\Dashboard\\DashboardItemEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 665, 56);
			NumericEntryV3 numericEntryV9;
			VisualDiagnostics.RegisterSourceInfo(numericEntryV9 = new NumericEntryV3(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\Dashboard\\DashboardItemEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 665, 38);
			Translate translate46;
			VisualDiagnostics.RegisterSourceInfo(translate46 = new Translate(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\Dashboard\\DashboardItemEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 670, 41);
			BindingExtension bindingExtension54;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension54 = new BindingExtension(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\Dashboard\\DashboardItemEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 671, 41);
			DashboardItemColorBox dashboardItemColorBox15;
			VisualDiagnostics.RegisterSourceInfo(dashboardItemColorBox15 = new DashboardItemColorBox(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\Dashboard\\DashboardItemEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 667, 38);
			Translate translate47;
			VisualDiagnostics.RegisterSourceInfo(translate47 = new Translate(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\Dashboard\\DashboardItemEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 676, 41);
			BindingExtension bindingExtension55;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension55 = new BindingExtension(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\Dashboard\\DashboardItemEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 677, 41);
			DashboardItemColorBox dashboardItemColorBox16;
			VisualDiagnostics.RegisterSourceInfo(dashboardItemColorBox16 = new DashboardItemColorBox(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\Dashboard\\DashboardItemEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 673, 38);
			BindingExtension bindingExtension56;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension56 = new BindingExtension(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\Dashboard\\DashboardItemEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 696, 56);
			Translate translate48;
			VisualDiagnostics.RegisterSourceInfo(translate48 = new Translate(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\Dashboard\\DashboardItemEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 696, 109);
			LabelSwitch labelSwitch6;
			VisualDiagnostics.RegisterSourceInfo(labelSwitch6 = new LabelSwitch(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\Dashboard\\DashboardItemEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 696, 38);
			BindingExtension bindingExtension57;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension57 = new BindingExtension(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\Dashboard\\DashboardItemEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 697, 50);
			Translate translate49;
			VisualDiagnostics.RegisterSourceInfo(translate49 = new Translate(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\Dashboard\\DashboardItemEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 698, 48);
			Label label30;
			VisualDiagnostics.RegisterSourceInfo(label30 = new Label(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\Dashboard\\DashboardItemEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 698, 42);
			BindingExtension bindingExtension58;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension58 = new BindingExtension(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\Dashboard\\DashboardItemEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 699, 60);
			NumericEntryV3 numericEntryV10;
			VisualDiagnostics.RegisterSourceInfo(numericEntryV10 = new NumericEntryV3(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\Dashboard\\DashboardItemEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 699, 42);
			Translate translate50;
			VisualDiagnostics.RegisterSourceInfo(translate50 = new Translate(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\Dashboard\\DashboardItemEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 701, 48);
			Label label31;
			VisualDiagnostics.RegisterSourceInfo(label31 = new Label(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\Dashboard\\DashboardItemEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 701, 42);
			BindingExtension bindingExtension59;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension59 = new BindingExtension(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\Dashboard\\DashboardItemEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 702, 60);
			NumericEntryV3 numericEntryV11;
			VisualDiagnostics.RegisterSourceInfo(numericEntryV11 = new NumericEntryV3(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\Dashboard\\DashboardItemEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 702, 42);
			Translate translate51;
			VisualDiagnostics.RegisterSourceInfo(translate51 = new Translate(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\Dashboard\\DashboardItemEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 706, 45);
			BindingExtension bindingExtension60;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension60 = new BindingExtension(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\Dashboard\\DashboardItemEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 707, 45);
			DashboardItemColorBox dashboardItemColorBox17;
			VisualDiagnostics.RegisterSourceInfo(dashboardItemColorBox17 = new DashboardItemColorBox(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\Dashboard\\DashboardItemEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 703, 42);
			StackLayout stackLayout9;
			VisualDiagnostics.RegisterSourceInfo(stackLayout9 = new StackLayout(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\Dashboard\\DashboardItemEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 697, 38);
			BindingExtension bindingExtension61;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension61 = new BindingExtension(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\Dashboard\\DashboardItemEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 711, 56);
			Translate translate52;
			VisualDiagnostics.RegisterSourceInfo(translate52 = new Translate(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\Dashboard\\DashboardItemEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 711, 108);
			LabelSwitch labelSwitch7;
			VisualDiagnostics.RegisterSourceInfo(labelSwitch7 = new LabelSwitch(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\Dashboard\\DashboardItemEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 711, 38);
			BindingExtension bindingExtension62;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension62 = new BindingExtension(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\Dashboard\\DashboardItemEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 712, 50);
			Translate translate53;
			VisualDiagnostics.RegisterSourceInfo(translate53 = new Translate(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\Dashboard\\DashboardItemEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 713, 48);
			Label label32;
			VisualDiagnostics.RegisterSourceInfo(label32 = new Label(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\Dashboard\\DashboardItemEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 713, 42);
			BindingExtension bindingExtension63;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension63 = new BindingExtension(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\Dashboard\\DashboardItemEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 714, 60);
			NumericEntryV3 numericEntryV12;
			VisualDiagnostics.RegisterSourceInfo(numericEntryV12 = new NumericEntryV3(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\Dashboard\\DashboardItemEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 714, 42);
			Translate translate54;
			VisualDiagnostics.RegisterSourceInfo(translate54 = new Translate(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\Dashboard\\DashboardItemEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 716, 48);
			Label label33;
			VisualDiagnostics.RegisterSourceInfo(label33 = new Label(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\Dashboard\\DashboardItemEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 716, 42);
			BindingExtension bindingExtension64;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension64 = new BindingExtension(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\Dashboard\\DashboardItemEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 717, 60);
			NumericEntryV3 numericEntryV13;
			VisualDiagnostics.RegisterSourceInfo(numericEntryV13 = new NumericEntryV3(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\Dashboard\\DashboardItemEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 717, 42);
			Translate translate55;
			VisualDiagnostics.RegisterSourceInfo(translate55 = new Translate(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\Dashboard\\DashboardItemEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 721, 45);
			BindingExtension bindingExtension65;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension65 = new BindingExtension(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\Dashboard\\DashboardItemEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 722, 45);
			DashboardItemColorBox dashboardItemColorBox18;
			VisualDiagnostics.RegisterSourceInfo(dashboardItemColorBox18 = new DashboardItemColorBox(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\Dashboard\\DashboardItemEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 718, 42);
			StackLayout stackLayout10;
			VisualDiagnostics.RegisterSourceInfo(stackLayout10 = new StackLayout(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\Dashboard\\DashboardItemEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 712, 38);
			StackLayout stackLayout11;
			VisualDiagnostics.RegisterSourceInfo(stackLayout11 = new StackLayout(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\Dashboard\\DashboardItemEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 658, 34);
			Frame frame6;
			VisualDiagnostics.RegisterSourceInfo(frame6 = new Frame(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\Dashboard\\DashboardItemEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 657, 30);
			SfExpander sfExpander4;
			VisualDiagnostics.RegisterSourceInfo(sfExpander4 = new SfExpander(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\Dashboard\\DashboardItemEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 638, 22);
			DynamicResourceExtension dynamicResourceExtension25;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension25 = new DynamicResourceExtension(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\Dashboard\\DashboardItemEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 740, 25);
			DynamicResourceExtension dynamicResourceExtension26;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension26 = new DynamicResourceExtension(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\Dashboard\\DashboardItemEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 741, 25);
			Translate translate56;
			VisualDiagnostics.RegisterSourceInfo(translate56 = new Translate(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\Dashboard\\DashboardItemEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 748, 37);
			DynamicResourceExtension dynamicResourceExtension27;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension27 = new DynamicResourceExtension(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\Dashboard\\DashboardItemEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 749, 37);
			Label label34;
			VisualDiagnostics.RegisterSourceInfo(label34 = new Label(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\Dashboard\\DashboardItemEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 745, 34);
			Grid grid13;
			VisualDiagnostics.RegisterSourceInfo(grid13 = new Grid(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\Dashboard\\DashboardItemEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 744, 30);
			Label label35;
			VisualDiagnostics.RegisterSourceInfo(label35 = new Label(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\Dashboard\\DashboardItemEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 757, 38);
			BindingExtension bindingExtension66;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension66 = new BindingExtension(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\Dashboard\\DashboardItemEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 758, 102);
			NumericEntryV3 numericEntryV14;
			VisualDiagnostics.RegisterSourceInfo(numericEntryV14 = new NumericEntryV3(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\Dashboard\\DashboardItemEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 758, 38);
			Translate translate57;
			VisualDiagnostics.RegisterSourceInfo(translate57 = new Translate(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\Dashboard\\DashboardItemEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 760, 44);
			Label label36;
			VisualDiagnostics.RegisterSourceInfo(label36 = new Label(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\Dashboard\\DashboardItemEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 760, 38);
			BindingExtension bindingExtension67;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension67 = new BindingExtension(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\Dashboard\\DashboardItemEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 761, 56);
			NumericEntryV3 numericEntryV15;
			VisualDiagnostics.RegisterSourceInfo(numericEntryV15 = new NumericEntryV3(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\Dashboard\\DashboardItemEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 761, 38);
			Translate translate58;
			VisualDiagnostics.RegisterSourceInfo(translate58 = new Translate(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\Dashboard\\DashboardItemEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 762, 44);
			Label label37;
			VisualDiagnostics.RegisterSourceInfo(label37 = new Label(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\Dashboard\\DashboardItemEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 762, 38);
			BindingExtension bindingExtension68;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension68 = new BindingExtension(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\Dashboard\\DashboardItemEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 763, 56);
			NumericEntryV3 numericEntryV16;
			VisualDiagnostics.RegisterSourceInfo(numericEntryV16 = new NumericEntryV3(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\Dashboard\\DashboardItemEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 763, 38);
			Translate translate59;
			VisualDiagnostics.RegisterSourceInfo(translate59 = new Translate(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\Dashboard\\DashboardItemEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 768, 41);
			BindingExtension bindingExtension69;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension69 = new BindingExtension(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\Dashboard\\DashboardItemEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 769, 41);
			DashboardItemColorBox dashboardItemColorBox19;
			VisualDiagnostics.RegisterSourceInfo(dashboardItemColorBox19 = new DashboardItemColorBox(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\Dashboard\\DashboardItemEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 765, 38);
			StackLayout stackLayout12;
			VisualDiagnostics.RegisterSourceInfo(stackLayout12 = new StackLayout(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\Dashboard\\DashboardItemEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 756, 34);
			Frame frame7;
			VisualDiagnostics.RegisterSourceInfo(frame7 = new Frame(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\Dashboard\\DashboardItemEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 755, 30);
			SfExpander sfExpander5;
			VisualDiagnostics.RegisterSourceInfo(sfExpander5 = new SfExpander(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\Dashboard\\DashboardItemEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 736, 22);
			DynamicResourceExtension dynamicResourceExtension28;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension28 = new DynamicResourceExtension(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\Dashboard\\DashboardItemEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 833, 25);
			DynamicResourceExtension dynamicResourceExtension29;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension29 = new DynamicResourceExtension(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\Dashboard\\DashboardItemEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 834, 25);
			Translate translate60;
			VisualDiagnostics.RegisterSourceInfo(translate60 = new Translate(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\Dashboard\\DashboardItemEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 841, 37);
			DynamicResourceExtension dynamicResourceExtension30;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension30 = new DynamicResourceExtension(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\Dashboard\\DashboardItemEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 842, 37);
			Label label38;
			VisualDiagnostics.RegisterSourceInfo(label38 = new Label(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\Dashboard\\DashboardItemEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 838, 34);
			Grid grid14;
			VisualDiagnostics.RegisterSourceInfo(grid14 = new Grid(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\Dashboard\\DashboardItemEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 837, 30);
			Translate translate61;
			VisualDiagnostics.RegisterSourceInfo(translate61 = new Translate(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\Dashboard\\DashboardItemEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 850, 44);
			Label label39;
			VisualDiagnostics.RegisterSourceInfo(label39 = new Label(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\Dashboard\\DashboardItemEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 850, 38);
			Translate translate62;
			VisualDiagnostics.RegisterSourceInfo(translate62 = new Translate(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\Dashboard\\DashboardItemEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 852, 41);
			StaticResourceExtension staticResourceExtension8;
			VisualDiagnostics.RegisterSourceInfo(staticResourceExtension8 = new StaticResourceExtension(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\Dashboard\\DashboardItemEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 854, 41);
			BindingExtension bindingExtension70;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension70 = new BindingExtension(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\Dashboard\\DashboardItemEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 854, 41);
			RadioButton radioButton;
			VisualDiagnostics.RegisterSourceInfo(radioButton = new RadioButton(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\Dashboard\\DashboardItemEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 851, 38);
			Translate translate63;
			VisualDiagnostics.RegisterSourceInfo(translate63 = new Translate(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\Dashboard\\DashboardItemEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 856, 41);
			BindingExtension bindingExtension71;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension71 = new BindingExtension(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\Dashboard\\DashboardItemEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 858, 41);
			RadioButton radioButton2;
			VisualDiagnostics.RegisterSourceInfo(radioButton2 = new RadioButton(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\Dashboard\\DashboardItemEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 855, 38);
			Translate translate64;
			VisualDiagnostics.RegisterSourceInfo(translate64 = new Translate(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\Dashboard\\DashboardItemEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 859, 44);
			Label label40;
			VisualDiagnostics.RegisterSourceInfo(label40 = new Label(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\Dashboard\\DashboardItemEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 859, 38);
			StaticResourceExtension staticResourceExtension9;
			VisualDiagnostics.RegisterSourceInfo(staticResourceExtension9 = new StaticResourceExtension(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\Dashboard\\DashboardItemEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 860, 71);
			BindingExtension bindingExtension72;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension72 = new BindingExtension(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\Dashboard\\DashboardItemEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 860, 71);
			Picker picker3;
			VisualDiagnostics.RegisterSourceInfo(picker3 = new Picker(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\Dashboard\\DashboardItemEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 860, 38);
			Translate translate65;
			VisualDiagnostics.RegisterSourceInfo(translate65 = new Translate(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\Dashboard\\DashboardItemEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 862, 44);
			Label label41;
			VisualDiagnostics.RegisterSourceInfo(label41 = new Label(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\Dashboard\\DashboardItemEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 862, 38);
			BindingExtension bindingExtension73;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension73 = new BindingExtension(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\Dashboard\\DashboardItemEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 863, 56);
			NumericEntryV3 numericEntryV17;
			VisualDiagnostics.RegisterSourceInfo(numericEntryV17 = new NumericEntryV3(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\Dashboard\\DashboardItemEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 863, 38);
			Translate translate66;
			VisualDiagnostics.RegisterSourceInfo(translate66 = new Translate(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\Dashboard\\DashboardItemEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 865, 44);
			Label label42;
			VisualDiagnostics.RegisterSourceInfo(label42 = new Label(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\Dashboard\\DashboardItemEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 865, 38);
			BindingExtension bindingExtension74;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension74 = new BindingExtension(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\Dashboard\\DashboardItemEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 866, 73);
			NumericEntryV3 numericEntryV18;
			VisualDiagnostics.RegisterSourceInfo(numericEntryV18 = new NumericEntryV3(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\Dashboard\\DashboardItemEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 866, 38);
			Translate translate67;
			VisualDiagnostics.RegisterSourceInfo(translate67 = new Translate(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\Dashboard\\DashboardItemEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 873, 41);
			BindingExtension bindingExtension75;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension75 = new BindingExtension(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\Dashboard\\DashboardItemEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 874, 41);
			DashboardItemColorBox dashboardItemColorBox20;
			VisualDiagnostics.RegisterSourceInfo(dashboardItemColorBox20 = new DashboardItemColorBox(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\Dashboard\\DashboardItemEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 870, 38);
			Translate translate68;
			VisualDiagnostics.RegisterSourceInfo(translate68 = new Translate(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\Dashboard\\DashboardItemEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 879, 41);
			BindingExtension bindingExtension76;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension76 = new BindingExtension(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\Dashboard\\DashboardItemEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 880, 41);
			DashboardItemColorBox dashboardItemColorBox21;
			VisualDiagnostics.RegisterSourceInfo(dashboardItemColorBox21 = new DashboardItemColorBox(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\Dashboard\\DashboardItemEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 876, 38);
			BindingExtension bindingExtension77;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension77 = new BindingExtension(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\Dashboard\\DashboardItemEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 883, 56);
			Translate translate69;
			VisualDiagnostics.RegisterSourceInfo(translate69 = new Translate(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\Dashboard\\DashboardItemEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 883, 107);
			LabelSwitch labelSwitch8;
			VisualDiagnostics.RegisterSourceInfo(labelSwitch8 = new LabelSwitch(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\Dashboard\\DashboardItemEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 883, 38);
			BindingExtension bindingExtension78;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension78 = new BindingExtension(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\Dashboard\\DashboardItemEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 886, 50);
			Translate translate70;
			VisualDiagnostics.RegisterSourceInfo(translate70 = new Translate(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\Dashboard\\DashboardItemEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 887, 48);
			Label label43;
			VisualDiagnostics.RegisterSourceInfo(label43 = new Label(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\Dashboard\\DashboardItemEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 887, 42);
			BindingExtension bindingExtension79;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension79 = new BindingExtension(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\Dashboard\\DashboardItemEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 888, 60);
			NumericEntryV3 numericEntryV19;
			VisualDiagnostics.RegisterSourceInfo(numericEntryV19 = new NumericEntryV3(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\Dashboard\\DashboardItemEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 888, 42);
			Translate translate71;
			VisualDiagnostics.RegisterSourceInfo(translate71 = new Translate(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\Dashboard\\DashboardItemEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 889, 48);
			Label label44;
			VisualDiagnostics.RegisterSourceInfo(label44 = new Label(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\Dashboard\\DashboardItemEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 889, 42);
			BindingExtension bindingExtension80;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension80 = new BindingExtension(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\Dashboard\\DashboardItemEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 890, 60);
			NumericEntryV3 numericEntryV20;
			VisualDiagnostics.RegisterSourceInfo(numericEntryV20 = new NumericEntryV3(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\Dashboard\\DashboardItemEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 890, 42);
			StackLayout stackLayout13;
			VisualDiagnostics.RegisterSourceInfo(stackLayout13 = new StackLayout(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\Dashboard\\DashboardItemEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 886, 38);
			BindingExtension bindingExtension81;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension81 = new BindingExtension(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\Dashboard\\DashboardItemEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 893, 56);
			Translate translate72;
			VisualDiagnostics.RegisterSourceInfo(translate72 = new Translate(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\Dashboard\\DashboardItemEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 893, 109);
			LabelSwitch labelSwitch9;
			VisualDiagnostics.RegisterSourceInfo(labelSwitch9 = new LabelSwitch(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\Dashboard\\DashboardItemEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 893, 38);
			BindingExtension bindingExtension82;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension82 = new BindingExtension(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\Dashboard\\DashboardItemEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 895, 50);
			Translate translate73;
			VisualDiagnostics.RegisterSourceInfo(translate73 = new Translate(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\Dashboard\\DashboardItemEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 896, 48);
			Label label45;
			VisualDiagnostics.RegisterSourceInfo(label45 = new Label(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\Dashboard\\DashboardItemEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 896, 42);
			BindingExtension bindingExtension83;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension83 = new BindingExtension(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\Dashboard\\DashboardItemEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 897, 60);
			NumericEntryV3 numericEntryV21;
			VisualDiagnostics.RegisterSourceInfo(numericEntryV21 = new NumericEntryV3(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\Dashboard\\DashboardItemEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 897, 42);
			StackLayout stackLayout14;
			VisualDiagnostics.RegisterSourceInfo(stackLayout14 = new StackLayout(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\Dashboard\\DashboardItemEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 895, 38);
			StackLayout stackLayout15;
			VisualDiagnostics.RegisterSourceInfo(stackLayout15 = new StackLayout(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\Dashboard\\DashboardItemEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 849, 34);
			Frame frame8;
			VisualDiagnostics.RegisterSourceInfo(frame8 = new Frame(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\Dashboard\\DashboardItemEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 848, 30);
			SfExpander sfExpander6;
			VisualDiagnostics.RegisterSourceInfo(sfExpander6 = new SfExpander(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\Dashboard\\DashboardItemEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 829, 22);
			DynamicResourceExtension dynamicResourceExtension31;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension31 = new DynamicResourceExtension(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\Dashboard\\DashboardItemEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 910, 25);
			DynamicResourceExtension dynamicResourceExtension32;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension32 = new DynamicResourceExtension(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\Dashboard\\DashboardItemEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 911, 25);
			Translate translate74;
			VisualDiagnostics.RegisterSourceInfo(translate74 = new Translate(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\Dashboard\\DashboardItemEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 918, 37);
			DynamicResourceExtension dynamicResourceExtension33;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension33 = new DynamicResourceExtension(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\Dashboard\\DashboardItemEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 919, 37);
			Label label46;
			VisualDiagnostics.RegisterSourceInfo(label46 = new Label(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\Dashboard\\DashboardItemEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 915, 34);
			Grid grid15;
			VisualDiagnostics.RegisterSourceInfo(grid15 = new Grid(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\Dashboard\\DashboardItemEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 914, 30);
			Translate translate75;
			VisualDiagnostics.RegisterSourceInfo(translate75 = new Translate(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\Dashboard\\DashboardItemEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 927, 44);
			Label label47;
			VisualDiagnostics.RegisterSourceInfo(label47 = new Label(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\Dashboard\\DashboardItemEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 927, 38);
			BindingExtension bindingExtension84;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension84 = new BindingExtension(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\Dashboard\\DashboardItemEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 929, 56);
			NumericEntryV3 numericEntryV22;
			VisualDiagnostics.RegisterSourceInfo(numericEntryV22 = new NumericEntryV3(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\Dashboard\\DashboardItemEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 929, 38);
			Translate translate76;
			VisualDiagnostics.RegisterSourceInfo(translate76 = new Translate(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\Dashboard\\DashboardItemEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 930, 44);
			Label label48;
			VisualDiagnostics.RegisterSourceInfo(label48 = new Label(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\Dashboard\\DashboardItemEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 930, 38);
			BindingExtension bindingExtension85;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension85 = new BindingExtension(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\Dashboard\\DashboardItemEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 931, 56);
			NumericEntryV3 numericEntryV23;
			VisualDiagnostics.RegisterSourceInfo(numericEntryV23 = new NumericEntryV3(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\Dashboard\\DashboardItemEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 931, 38);
			ColumnDefinition columnDefinition19;
			VisualDiagnostics.RegisterSourceInfo(columnDefinition19 = new ColumnDefinition(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\Dashboard\\DashboardItemEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 937, 46);
			ColumnDefinition columnDefinition20;
			VisualDiagnostics.RegisterSourceInfo(columnDefinition20 = new ColumnDefinition(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\Dashboard\\DashboardItemEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 938, 46);
			RowDefinition rowDefinition15;
			VisualDiagnostics.RegisterSourceInfo(rowDefinition15 = new RowDefinition(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\Dashboard\\DashboardItemEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 941, 46);
			RowDefinition rowDefinition16;
			VisualDiagnostics.RegisterSourceInfo(rowDefinition16 = new RowDefinition(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\Dashboard\\DashboardItemEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 942, 46);
			Translate translate77;
			VisualDiagnostics.RegisterSourceInfo(translate77 = new Translate(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\Dashboard\\DashboardItemEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 947, 45);
			Label label49;
			VisualDiagnostics.RegisterSourceInfo(label49 = new Label(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\Dashboard\\DashboardItemEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 944, 42);
			BindingExtension bindingExtension86;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension86 = new BindingExtension(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\Dashboard\\DashboardItemEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 952, 45);
			Label label50;
			VisualDiagnostics.RegisterSourceInfo(label50 = new Label(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\Dashboard\\DashboardItemEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 948, 42);
			BindingExtension bindingExtension87;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension87 = new BindingExtension(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\Dashboard\\DashboardItemEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 961, 45);
			ExtendedSlider extendedSlider8;
			VisualDiagnostics.RegisterSourceInfo(extendedSlider8 = new ExtendedSlider(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\Dashboard\\DashboardItemEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 954, 42);
			Grid grid16;
			VisualDiagnostics.RegisterSourceInfo(grid16 = new Grid(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\Dashboard\\DashboardItemEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 935, 38);
			Translate translate78;
			VisualDiagnostics.RegisterSourceInfo(translate78 = new Translate(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\Dashboard\\DashboardItemEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 967, 41);
			BindingExtension bindingExtension88;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension88 = new BindingExtension(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\Dashboard\\DashboardItemEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 968, 41);
			DashboardItemColorBox dashboardItemColorBox22;
			VisualDiagnostics.RegisterSourceInfo(dashboardItemColorBox22 = new DashboardItemColorBox(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\Dashboard\\DashboardItemEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 964, 38);
			Translate translate79;
			VisualDiagnostics.RegisterSourceInfo(translate79 = new Translate(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\Dashboard\\DashboardItemEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 975, 41);
			BindingExtension bindingExtension89;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension89 = new BindingExtension(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\Dashboard\\DashboardItemEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 976, 41);
			DashboardItemColorBox dashboardItemColorBox23;
			VisualDiagnostics.RegisterSourceInfo(dashboardItemColorBox23 = new DashboardItemColorBox(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\Dashboard\\DashboardItemEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 972, 38);
			Translate translate80;
			VisualDiagnostics.RegisterSourceInfo(translate80 = new Translate(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\Dashboard\\DashboardItemEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 982, 41);
			BindingExtension bindingExtension90;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension90 = new BindingExtension(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\Dashboard\\DashboardItemEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 983, 41);
			DashboardItemColorBox dashboardItemColorBox24;
			VisualDiagnostics.RegisterSourceInfo(dashboardItemColorBox24 = new DashboardItemColorBox(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\Dashboard\\DashboardItemEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 979, 38);
			BindingExtension bindingExtension91;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension91 = new BindingExtension(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\Dashboard\\DashboardItemEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 985, 56);
			Translate translate81;
			VisualDiagnostics.RegisterSourceInfo(translate81 = new Translate(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\Dashboard\\DashboardItemEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 985, 101);
			LabelSwitch labelSwitch10;
			VisualDiagnostics.RegisterSourceInfo(labelSwitch10 = new LabelSwitch(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\Dashboard\\DashboardItemEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 985, 38);
			On on3;
			VisualDiagnostics.RegisterSourceInfo(on3 = new On(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\Dashboard\\DashboardItemEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 992, 50);
			On on4;
			VisualDiagnostics.RegisterSourceInfo(on4 = new On(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\Dashboard\\DashboardItemEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 993, 50);
			OnPlatform<bool> onPlatform4;
			VisualDiagnostics.RegisterSourceInfo(onPlatform4 = new OnPlatform<bool>(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\Dashboard\\DashboardItemEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 991, 46);
			ColumnDefinition columnDefinition21;
			VisualDiagnostics.RegisterSourceInfo(columnDefinition21 = new ColumnDefinition(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\Dashboard\\DashboardItemEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 997, 46);
			ColumnDefinition columnDefinition22;
			VisualDiagnostics.RegisterSourceInfo(columnDefinition22 = new ColumnDefinition(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\Dashboard\\DashboardItemEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 998, 46);
			BindingExtension bindingExtension92;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension92 = new BindingExtension(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\Dashboard\\DashboardItemEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 1002, 45);
			Translate translate82;
			VisualDiagnostics.RegisterSourceInfo(translate82 = new Translate(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\Dashboard\\DashboardItemEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 1003, 45);
			Label label51;
			VisualDiagnostics.RegisterSourceInfo(label51 = new Label(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\Dashboard\\DashboardItemEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 1000, 42);
			StaticResourceExtension staticResourceExtension10;
			VisualDiagnostics.RegisterSourceInfo(staticResourceExtension10 = new StaticResourceExtension(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\Dashboard\\DashboardItemEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 1007, 45);
			BindingExtension bindingExtension93;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension93 = new BindingExtension(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\Dashboard\\DashboardItemEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 1007, 45);
			Translate translate83;
			VisualDiagnostics.RegisterSourceInfo(translate83 = new Translate(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\Dashboard\\DashboardItemEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 1008, 45);
			Label label52;
			VisualDiagnostics.RegisterSourceInfo(label52 = new Label(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\Dashboard\\DashboardItemEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 1005, 42);
			BindingExtension bindingExtension94;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension94 = new BindingExtension(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\Dashboard\\DashboardItemEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 1013, 45);
			Switch @switch;
			VisualDiagnostics.RegisterSourceInfo(@switch = new Switch(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\Dashboard\\DashboardItemEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 1010, 42);
			Grid grid17;
			VisualDiagnostics.RegisterSourceInfo(grid17 = new Grid(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\Dashboard\\DashboardItemEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 989, 38);
			StackLayout stackLayout16;
			VisualDiagnostics.RegisterSourceInfo(stackLayout16 = new StackLayout(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\Dashboard\\DashboardItemEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 926, 34);
			Frame frame9;
			VisualDiagnostics.RegisterSourceInfo(frame9 = new Frame(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\Dashboard\\DashboardItemEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 925, 30);
			SfExpander sfExpander7;
			VisualDiagnostics.RegisterSourceInfo(sfExpander7 = new SfExpander(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\Dashboard\\DashboardItemEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 906, 22);
			DynamicResourceExtension dynamicResourceExtension34;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension34 = new DynamicResourceExtension(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\Dashboard\\DashboardItemEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 1027, 25);
			DynamicResourceExtension dynamicResourceExtension35;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension35 = new DynamicResourceExtension(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\Dashboard\\DashboardItemEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 1028, 25);
			Translate translate84;
			VisualDiagnostics.RegisterSourceInfo(translate84 = new Translate(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\Dashboard\\DashboardItemEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 1035, 37);
			DynamicResourceExtension dynamicResourceExtension36;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension36 = new DynamicResourceExtension(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\Dashboard\\DashboardItemEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 1036, 37);
			Label label53;
			VisualDiagnostics.RegisterSourceInfo(label53 = new Label(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\Dashboard\\DashboardItemEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 1032, 34);
			Grid grid18;
			VisualDiagnostics.RegisterSourceInfo(grid18 = new Grid(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\Dashboard\\DashboardItemEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 1031, 30);
			BindingExtension bindingExtension95;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension95 = new BindingExtension(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\Dashboard\\DashboardItemEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 1047, 56);
			Translate translate85;
			VisualDiagnostics.RegisterSourceInfo(translate85 = new Translate(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\Dashboard\\DashboardItemEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 1047, 102);
			LabelSwitch labelSwitch11;
			VisualDiagnostics.RegisterSourceInfo(labelSwitch11 = new LabelSwitch(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\Dashboard\\DashboardItemEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 1047, 38);
			BindingExtension bindingExtension96;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension96 = new BindingExtension(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\Dashboard\\DashboardItemEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 1048, 56);
			Translate translate86;
			VisualDiagnostics.RegisterSourceInfo(translate86 = new Translate(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\Dashboard\\DashboardItemEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 1048, 99);
			LabelSwitch labelSwitch12;
			VisualDiagnostics.RegisterSourceInfo(labelSwitch12 = new LabelSwitch(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\Dashboard\\DashboardItemEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 1048, 38);
			Translate translate87;
			VisualDiagnostics.RegisterSourceInfo(translate87 = new Translate(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\Dashboard\\DashboardItemEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 1052, 41);
			BindingExtension bindingExtension97;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension97 = new BindingExtension(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\Dashboard\\DashboardItemEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 1053, 41);
			DashboardItemColorBox dashboardItemColorBox25;
			VisualDiagnostics.RegisterSourceInfo(dashboardItemColorBox25 = new DashboardItemColorBox(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\Dashboard\\DashboardItemEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 1049, 38);
			ColumnDefinition columnDefinition23;
			VisualDiagnostics.RegisterSourceInfo(columnDefinition23 = new ColumnDefinition(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\Dashboard\\DashboardItemEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 1058, 46);
			ColumnDefinition columnDefinition24;
			VisualDiagnostics.RegisterSourceInfo(columnDefinition24 = new ColumnDefinition(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\Dashboard\\DashboardItemEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 1059, 46);
			RowDefinition rowDefinition17;
			VisualDiagnostics.RegisterSourceInfo(rowDefinition17 = new RowDefinition(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\Dashboard\\DashboardItemEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 1062, 46);
			RowDefinition rowDefinition18;
			VisualDiagnostics.RegisterSourceInfo(rowDefinition18 = new RowDefinition(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\Dashboard\\DashboardItemEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 1063, 46);
			Translate translate88;
			VisualDiagnostics.RegisterSourceInfo(translate88 = new Translate(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\Dashboard\\DashboardItemEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 1068, 45);
			Label label54;
			VisualDiagnostics.RegisterSourceInfo(label54 = new Label(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\Dashboard\\DashboardItemEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 1065, 42);
			BindingExtension bindingExtension98;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension98 = new BindingExtension(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\Dashboard\\DashboardItemEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 1073, 45);
			Label label55;
			VisualDiagnostics.RegisterSourceInfo(label55 = new Label(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\Dashboard\\DashboardItemEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 1069, 42);
			BindingExtension bindingExtension99;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension99 = new BindingExtension(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\Dashboard\\DashboardItemEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 1082, 45);
			ExtendedSlider extendedSlider9;
			VisualDiagnostics.RegisterSourceInfo(extendedSlider9 = new ExtendedSlider(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\Dashboard\\DashboardItemEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 1075, 42);
			Grid grid19;
			VisualDiagnostics.RegisterSourceInfo(grid19 = new Grid(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\Dashboard\\DashboardItemEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 1056, 38);
			BindingExtension bindingExtension100;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension100 = new BindingExtension(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\Dashboard\\DashboardItemEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 1087, 56);
			Translate translate89;
			VisualDiagnostics.RegisterSourceInfo(translate89 = new Translate(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\Dashboard\\DashboardItemEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 1087, 110);
			LabelSwitch labelSwitch13;
			VisualDiagnostics.RegisterSourceInfo(labelSwitch13 = new LabelSwitch(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\Dashboard\\DashboardItemEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 1087, 38);
			Translate translate90;
			VisualDiagnostics.RegisterSourceInfo(translate90 = new Translate(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\Dashboard\\DashboardItemEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 1091, 41);
			BindingExtension bindingExtension101;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension101 = new BindingExtension(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\Dashboard\\DashboardItemEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 1092, 41);
			DashboardItemColorBox dashboardItemColorBox26;
			VisualDiagnostics.RegisterSourceInfo(dashboardItemColorBox26 = new DashboardItemColorBox(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\Dashboard\\DashboardItemEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 1088, 38);
			ColumnDefinition columnDefinition25;
			VisualDiagnostics.RegisterSourceInfo(columnDefinition25 = new ColumnDefinition(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\Dashboard\\DashboardItemEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 1096, 46);
			ColumnDefinition columnDefinition26;
			VisualDiagnostics.RegisterSourceInfo(columnDefinition26 = new ColumnDefinition(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\Dashboard\\DashboardItemEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 1097, 46);
			RowDefinition rowDefinition19;
			VisualDiagnostics.RegisterSourceInfo(rowDefinition19 = new RowDefinition(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\Dashboard\\DashboardItemEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 1100, 46);
			RowDefinition rowDefinition20;
			VisualDiagnostics.RegisterSourceInfo(rowDefinition20 = new RowDefinition(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\Dashboard\\DashboardItemEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 1101, 46);
			BindingExtension bindingExtension102;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension102 = new BindingExtension(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\Dashboard\\DashboardItemEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 1107, 45);
			Translate translate91;
			VisualDiagnostics.RegisterSourceInfo(translate91 = new Translate(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\Dashboard\\DashboardItemEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 1108, 45);
			Label label56;
			VisualDiagnostics.RegisterSourceInfo(label56 = new Label(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\Dashboard\\DashboardItemEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 1103, 42);
			StaticResourceExtension staticResourceExtension11;
			VisualDiagnostics.RegisterSourceInfo(staticResourceExtension11 = new StaticResourceExtension(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\Dashboard\\DashboardItemEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 1114, 45);
			BindingExtension bindingExtension103;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension103 = new BindingExtension(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\Dashboard\\DashboardItemEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 1114, 45);
			Translate translate92;
			VisualDiagnostics.RegisterSourceInfo(translate92 = new Translate(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\Dashboard\\DashboardItemEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 1115, 45);
			Label label57;
			VisualDiagnostics.RegisterSourceInfo(label57 = new Label(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\Dashboard\\DashboardItemEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 1110, 42);
			Translate translate93;
			VisualDiagnostics.RegisterSourceInfo(translate93 = new Translate(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\Dashboard\\DashboardItemEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 1120, 45);
			Label label58;
			VisualDiagnostics.RegisterSourceInfo(label58 = new Label(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\Dashboard\\DashboardItemEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 1117, 42);
			BindingExtension bindingExtension104;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension104 = new BindingExtension(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\Dashboard\\DashboardItemEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 1124, 45);
			CheckSwitch checkSwitch;
			VisualDiagnostics.RegisterSourceInfo(checkSwitch = new CheckSwitch(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\Dashboard\\DashboardItemEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 1121, 42);
			Grid grid20;
			VisualDiagnostics.RegisterSourceInfo(grid20 = new Grid(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\Dashboard\\DashboardItemEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 1094, 38);
			StackLayout stackLayout17;
			VisualDiagnostics.RegisterSourceInfo(stackLayout17 = new StackLayout(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\Dashboard\\DashboardItemEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 1044, 34);
			Frame frame10;
			VisualDiagnostics.RegisterSourceInfo(frame10 = new Frame(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\Dashboard\\DashboardItemEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 1043, 30);
			SfExpander sfExpander8;
			VisualDiagnostics.RegisterSourceInfo(sfExpander8 = new SfExpander(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\Dashboard\\DashboardItemEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 1023, 22);
			DynamicResourceExtension dynamicResourceExtension37;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension37 = new DynamicResourceExtension(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\Dashboard\\DashboardItemEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 1136, 25);
			DynamicResourceExtension dynamicResourceExtension38;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension38 = new DynamicResourceExtension(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\Dashboard\\DashboardItemEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 1137, 25);
			Translate translate94;
			VisualDiagnostics.RegisterSourceInfo(translate94 = new Translate(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\Dashboard\\DashboardItemEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 1144, 37);
			DynamicResourceExtension dynamicResourceExtension39;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension39 = new DynamicResourceExtension(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\Dashboard\\DashboardItemEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 1145, 37);
			Label label59;
			VisualDiagnostics.RegisterSourceInfo(label59 = new Label(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\Dashboard\\DashboardItemEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 1141, 34);
			Grid grid21;
			VisualDiagnostics.RegisterSourceInfo(grid21 = new Grid(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\Dashboard\\DashboardItemEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 1140, 30);
			BindingExtension bindingExtension105;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension105 = new BindingExtension(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\Dashboard\\DashboardItemEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 1155, 56);
			Translate translate95;
			VisualDiagnostics.RegisterSourceInfo(translate95 = new Translate(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\Dashboard\\DashboardItemEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 1155, 108);
			LabelSwitch labelSwitch14;
			VisualDiagnostics.RegisterSourceInfo(labelSwitch14 = new LabelSwitch(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\Dashboard\\DashboardItemEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 1155, 38);
			BindingExtension bindingExtension106;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension106 = new BindingExtension(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\Dashboard\\DashboardItemEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 1158, 50);
			Translate translate96;
			VisualDiagnostics.RegisterSourceInfo(translate96 = new Translate(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\Dashboard\\DashboardItemEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 1159, 48);
			Label label60;
			VisualDiagnostics.RegisterSourceInfo(label60 = new Label(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\Dashboard\\DashboardItemEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 1159, 42);
			BindingExtension bindingExtension107;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension107 = new BindingExtension(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\Dashboard\\DashboardItemEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 1161, 60);
			NumericEntryV3 numericEntryV24;
			VisualDiagnostics.RegisterSourceInfo(numericEntryV24 = new NumericEntryV3(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\Dashboard\\DashboardItemEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 1161, 42);
			Translate translate97;
			VisualDiagnostics.RegisterSourceInfo(translate97 = new Translate(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\Dashboard\\DashboardItemEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 1166, 45);
			BindingExtension bindingExtension108;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension108 = new BindingExtension(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\Dashboard\\DashboardItemEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 1167, 45);
			DashboardItemColorBox dashboardItemColorBox27;
			VisualDiagnostics.RegisterSourceInfo(dashboardItemColorBox27 = new DashboardItemColorBox(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\Dashboard\\DashboardItemEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 1163, 42);
			StackLayout stackLayout18;
			VisualDiagnostics.RegisterSourceInfo(stackLayout18 = new StackLayout(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\Dashboard\\DashboardItemEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 1158, 38);
			BindingExtension bindingExtension109;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension109 = new BindingExtension(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\Dashboard\\DashboardItemEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 1173, 56);
			Translate translate98;
			VisualDiagnostics.RegisterSourceInfo(translate98 = new Translate(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\Dashboard\\DashboardItemEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 1173, 101);
			LabelSwitch labelSwitch15;
			VisualDiagnostics.RegisterSourceInfo(labelSwitch15 = new LabelSwitch(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\Dashboard\\DashboardItemEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 1173, 38);
			BindingExtension bindingExtension110;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension110 = new BindingExtension(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\Dashboard\\DashboardItemEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 1176, 50);
			Translate translate99;
			VisualDiagnostics.RegisterSourceInfo(translate99 = new Translate(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\Dashboard\\DashboardItemEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 1177, 48);
			Label label61;
			VisualDiagnostics.RegisterSourceInfo(label61 = new Label(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\Dashboard\\DashboardItemEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 1177, 42);
			BindingExtension bindingExtension111;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension111 = new BindingExtension(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\Dashboard\\DashboardItemEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 1179, 60);
			NumericEntryV3 numericEntryV25;
			VisualDiagnostics.RegisterSourceInfo(numericEntryV25 = new NumericEntryV3(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\Dashboard\\DashboardItemEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 1179, 42);
			Translate translate100;
			VisualDiagnostics.RegisterSourceInfo(translate100 = new Translate(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\Dashboard\\DashboardItemEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 1181, 48);
			Label label62;
			VisualDiagnostics.RegisterSourceInfo(label62 = new Label(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\Dashboard\\DashboardItemEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 1181, 42);
			ColumnDefinition columnDefinition27;
			VisualDiagnostics.RegisterSourceInfo(columnDefinition27 = new ColumnDefinition(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\Dashboard\\DashboardItemEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 1184, 50);
			ColumnDefinition columnDefinition28;
			VisualDiagnostics.RegisterSourceInfo(columnDefinition28 = new ColumnDefinition(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\Dashboard\\DashboardItemEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 1185, 50);
			List<string> soundsList;
			VisualDiagnostics.RegisterSourceInfo(soundsList = StaticLists.SoundsList, new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\Dashboard\\DashboardItemEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 1189, 49);
			BindingExtension bindingExtension112;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension112 = new BindingExtension(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\Dashboard\\DashboardItemEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 1189, 49);
			BindingExtension bindingExtension113;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension113 = new BindingExtension(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\Dashboard\\DashboardItemEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 1190, 49);
			Picker picker4;
			VisualDiagnostics.RegisterSourceInfo(picker4 = new Picker(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\Dashboard\\DashboardItemEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 1187, 46);
			TapGestureRecognizer tapGestureRecognizer;
			VisualDiagnostics.RegisterSourceInfo(tapGestureRecognizer = new TapGestureRecognizer(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\Dashboard\\DashboardItemEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 1196, 54);
			Image image;
			VisualDiagnostics.RegisterSourceInfo(image = new Image(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\Dashboard\\DashboardItemEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 1191, 46);
			Grid grid22;
			VisualDiagnostics.RegisterSourceInfo(grid22 = new Grid(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\Dashboard\\DashboardItemEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 1182, 42);
			StackLayout stackLayout19;
			VisualDiagnostics.RegisterSourceInfo(stackLayout19 = new StackLayout(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\Dashboard\\DashboardItemEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 1176, 38);
			BindingExtension bindingExtension114;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension114 = new BindingExtension(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\Dashboard\\DashboardItemEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 1204, 56);
			Translate translate101;
			VisualDiagnostics.RegisterSourceInfo(translate101 = new Translate(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\Dashboard\\DashboardItemEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 1204, 106);
			LabelSwitch labelSwitch16;
			VisualDiagnostics.RegisterSourceInfo(labelSwitch16 = new LabelSwitch(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\Dashboard\\DashboardItemEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 1204, 38);
			BindingExtension bindingExtension115;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension115 = new BindingExtension(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\Dashboard\\DashboardItemEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 1208, 50);
			Translate translate102;
			VisualDiagnostics.RegisterSourceInfo(translate102 = new Translate(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\Dashboard\\DashboardItemEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 1209, 48);
			Label label63;
			VisualDiagnostics.RegisterSourceInfo(label63 = new Label(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\Dashboard\\DashboardItemEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 1209, 42);
			BindingExtension bindingExtension116;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension116 = new BindingExtension(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\Dashboard\\DashboardItemEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 1210, 60);
			NumericEntryV3 numericEntryV26;
			VisualDiagnostics.RegisterSourceInfo(numericEntryV26 = new NumericEntryV3(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\Dashboard\\DashboardItemEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 1210, 42);
			Translate translate103;
			VisualDiagnostics.RegisterSourceInfo(translate103 = new Translate(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\Dashboard\\DashboardItemEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 1215, 45);
			BindingExtension bindingExtension117;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension117 = new BindingExtension(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\Dashboard\\DashboardItemEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 1216, 45);
			DashboardItemColorBox dashboardItemColorBox28;
			VisualDiagnostics.RegisterSourceInfo(dashboardItemColorBox28 = new DashboardItemColorBox(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\Dashboard\\DashboardItemEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 1212, 42);
			StackLayout stackLayout20;
			VisualDiagnostics.RegisterSourceInfo(stackLayout20 = new StackLayout(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\Dashboard\\DashboardItemEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 1208, 38);
			BindingExtension bindingExtension118;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension118 = new BindingExtension(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\Dashboard\\DashboardItemEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 1222, 56);
			Translate translate104;
			VisualDiagnostics.RegisterSourceInfo(translate104 = new Translate(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\Dashboard\\DashboardItemEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 1222, 104);
			LabelSwitch labelSwitch17;
			VisualDiagnostics.RegisterSourceInfo(labelSwitch17 = new LabelSwitch(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\Dashboard\\DashboardItemEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 1222, 38);
			BindingExtension bindingExtension119;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension119 = new BindingExtension(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\Dashboard\\DashboardItemEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 1225, 50);
			Translate translate105;
			VisualDiagnostics.RegisterSourceInfo(translate105 = new Translate(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\Dashboard\\DashboardItemEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 1226, 48);
			Label label64;
			VisualDiagnostics.RegisterSourceInfo(label64 = new Label(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\Dashboard\\DashboardItemEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 1226, 42);
			BindingExtension bindingExtension120;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension120 = new BindingExtension(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\Dashboard\\DashboardItemEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 1227, 60);
			NumericEntryV3 numericEntryV27;
			VisualDiagnostics.RegisterSourceInfo(numericEntryV27 = new NumericEntryV3(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\Dashboard\\DashboardItemEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 1227, 42);
			Translate translate106;
			VisualDiagnostics.RegisterSourceInfo(translate106 = new Translate(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\Dashboard\\DashboardItemEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 1229, 48);
			Label label65;
			VisualDiagnostics.RegisterSourceInfo(label65 = new Label(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\Dashboard\\DashboardItemEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 1229, 42);
			ColumnDefinition columnDefinition29;
			VisualDiagnostics.RegisterSourceInfo(columnDefinition29 = new ColumnDefinition(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\Dashboard\\DashboardItemEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 1232, 50);
			ColumnDefinition columnDefinition30;
			VisualDiagnostics.RegisterSourceInfo(columnDefinition30 = new ColumnDefinition(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\Dashboard\\DashboardItemEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 1233, 50);
			List<string> soundsList2;
			VisualDiagnostics.RegisterSourceInfo(soundsList2 = StaticLists.SoundsList, new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\Dashboard\\DashboardItemEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 1237, 49);
			BindingExtension bindingExtension121;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension121 = new BindingExtension(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\Dashboard\\DashboardItemEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 1237, 49);
			BindingExtension bindingExtension122;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension122 = new BindingExtension(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\Dashboard\\DashboardItemEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 1238, 49);
			Picker picker5;
			VisualDiagnostics.RegisterSourceInfo(picker5 = new Picker(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\Dashboard\\DashboardItemEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 1235, 46);
			TapGestureRecognizer tapGestureRecognizer2;
			VisualDiagnostics.RegisterSourceInfo(tapGestureRecognizer2 = new TapGestureRecognizer(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\Dashboard\\DashboardItemEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 1244, 54);
			Image image2;
			VisualDiagnostics.RegisterSourceInfo(image2 = new Image(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\Dashboard\\DashboardItemEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 1239, 46);
			Grid grid23;
			VisualDiagnostics.RegisterSourceInfo(grid23 = new Grid(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\Dashboard\\DashboardItemEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 1230, 42);
			StackLayout stackLayout21;
			VisualDiagnostics.RegisterSourceInfo(stackLayout21 = new StackLayout(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\Dashboard\\DashboardItemEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 1225, 38);
			StackLayout stackLayout22;
			VisualDiagnostics.RegisterSourceInfo(stackLayout22 = new StackLayout(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\Dashboard\\DashboardItemEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 1153, 34);
			Frame frame11;
			VisualDiagnostics.RegisterSourceInfo(frame11 = new Frame(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\Dashboard\\DashboardItemEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 1152, 30);
			SfExpander sfExpander9;
			VisualDiagnostics.RegisterSourceInfo(sfExpander9 = new SfExpander(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\Dashboard\\DashboardItemEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 1132, 22);
			DynamicResourceExtension dynamicResourceExtension40;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension40 = new DynamicResourceExtension(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\Dashboard\\DashboardItemEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 1262, 25);
			DynamicResourceExtension dynamicResourceExtension41;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension41 = new DynamicResourceExtension(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\Dashboard\\DashboardItemEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 1263, 25);
			Translate translate107;
			VisualDiagnostics.RegisterSourceInfo(translate107 = new Translate(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\Dashboard\\DashboardItemEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 1270, 37);
			DynamicResourceExtension dynamicResourceExtension42;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension42 = new DynamicResourceExtension(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\Dashboard\\DashboardItemEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 1271, 37);
			Label label66;
			VisualDiagnostics.RegisterSourceInfo(label66 = new Label(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\Dashboard\\DashboardItemEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 1267, 34);
			Grid grid24;
			VisualDiagnostics.RegisterSourceInfo(grid24 = new Grid(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\Dashboard\\DashboardItemEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 1266, 30);
			Translate translate108;
			VisualDiagnostics.RegisterSourceInfo(translate108 = new Translate(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\Dashboard\\DashboardItemEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 1279, 44);
			Label label67;
			VisualDiagnostics.RegisterSourceInfo(label67 = new Label(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\Dashboard\\DashboardItemEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 1279, 38);
			Translate translate109;
			VisualDiagnostics.RegisterSourceInfo(translate109 = new Translate(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\Dashboard\\DashboardItemEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 1280, 80);
			Button button3;
			VisualDiagnostics.RegisterSourceInfo(button3 = new Button(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\Dashboard\\DashboardItemEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 1280, 38);
			Translate translate110;
			VisualDiagnostics.RegisterSourceInfo(translate110 = new Translate(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\Dashboard\\DashboardItemEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 1282, 44);
			Label label68;
			VisualDiagnostics.RegisterSourceInfo(label68 = new Label(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\Dashboard\\DashboardItemEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 1282, 38);
			Translate translate111;
			VisualDiagnostics.RegisterSourceInfo(translate111 = new Translate(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\Dashboard\\DashboardItemEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 1283, 88);
			Button button4;
			VisualDiagnostics.RegisterSourceInfo(button4 = new Button(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\Dashboard\\DashboardItemEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 1283, 38);
			Translate translate112;
			VisualDiagnostics.RegisterSourceInfo(translate112 = new Translate(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\Dashboard\\DashboardItemEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 1285, 44);
			Label label69;
			VisualDiagnostics.RegisterSourceInfo(label69 = new Label(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\Dashboard\\DashboardItemEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 1285, 38);
			Translate translate113;
			VisualDiagnostics.RegisterSourceInfo(translate113 = new Translate(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\Dashboard\\DashboardItemEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 1286, 77);
			Button button5;
			VisualDiagnostics.RegisterSourceInfo(button5 = new Button(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\Dashboard\\DashboardItemEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 1286, 38);
			Translate translate114;
			VisualDiagnostics.RegisterSourceInfo(translate114 = new Translate(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\Dashboard\\DashboardItemEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 1288, 44);
			Label label70;
			VisualDiagnostics.RegisterSourceInfo(label70 = new Label(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\Dashboard\\DashboardItemEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 1288, 38);
			Translate translate115;
			VisualDiagnostics.RegisterSourceInfo(translate115 = new Translate(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\Dashboard\\DashboardItemEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 1289, 93);
			Button button6;
			VisualDiagnostics.RegisterSourceInfo(button6 = new Button(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\Dashboard\\DashboardItemEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 1289, 38);
			Translate translate116;
			VisualDiagnostics.RegisterSourceInfo(translate116 = new Translate(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\Dashboard\\DashboardItemEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 1291, 44);
			Label label71;
			VisualDiagnostics.RegisterSourceInfo(label71 = new Label(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\Dashboard\\DashboardItemEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 1291, 38);
			Translate translate117;
			VisualDiagnostics.RegisterSourceInfo(translate117 = new Translate(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\Dashboard\\DashboardItemEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 1292, 86);
			Button button7;
			VisualDiagnostics.RegisterSourceInfo(button7 = new Button(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\Dashboard\\DashboardItemEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 1292, 38);
			Translate translate118;
			VisualDiagnostics.RegisterSourceInfo(translate118 = new Translate(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\Dashboard\\DashboardItemEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 1294, 44);
			Label label72;
			VisualDiagnostics.RegisterSourceInfo(label72 = new Label(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\Dashboard\\DashboardItemEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 1294, 38);
			Translate translate119;
			VisualDiagnostics.RegisterSourceInfo(translate119 = new Translate(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\Dashboard\\DashboardItemEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 1295, 90);
			Button button8;
			VisualDiagnostics.RegisterSourceInfo(button8 = new Button(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\Dashboard\\DashboardItemEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 1295, 38);
			Translate translate120;
			VisualDiagnostics.RegisterSourceInfo(translate120 = new Translate(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\Dashboard\\DashboardItemEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 1297, 44);
			Label label73;
			VisualDiagnostics.RegisterSourceInfo(label73 = new Label(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\Dashboard\\DashboardItemEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 1297, 38);
			Translate translate121;
			VisualDiagnostics.RegisterSourceInfo(translate121 = new Translate(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\Dashboard\\DashboardItemEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 1298, 82);
			Button button9;
			VisualDiagnostics.RegisterSourceInfo(button9 = new Button(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\Dashboard\\DashboardItemEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 1298, 38);
			StackLayout stackLayout23;
			VisualDiagnostics.RegisterSourceInfo(stackLayout23 = new StackLayout(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\Dashboard\\DashboardItemEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 1278, 34);
			Frame frame12;
			VisualDiagnostics.RegisterSourceInfo(frame12 = new Frame(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\Dashboard\\DashboardItemEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 1277, 30);
			SfExpander sfExpander10;
			VisualDiagnostics.RegisterSourceInfo(sfExpander10 = new SfExpander(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\Dashboard\\DashboardItemEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 1258, 22);
			StackLayout stackLayout24;
			VisualDiagnostics.RegisterSourceInfo(stackLayout24 = new StackLayout(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\Dashboard\\DashboardItemEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 167, 18);
			ScrollView scrollView;
			VisualDiagnostics.RegisterSourceInfo(scrollView = new ScrollView(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\Dashboard\\DashboardItemEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 163, 14);
			Grid grid25;
			VisualDiagnostics.RegisterSourceInfo(grid25 = new Grid(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\Dashboard\\DashboardItemEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 126, 10);
			VisualDiagnostics.RegisterSourceInfo(this, new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Pages\\Dashboard\\DashboardItemEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 2, 2);
			NameScope nameScope = NameScope.GetNameScope(this) ?? new NameScope();
			NameScope.SetNameScope(this, nameScope);
			NameScope nameScope2 = new NameScope();
			NameScope nameScope3 = new NameScope();
			NameScope nameScope4 = new NameScope();
			NameScope nameScope5 = new NameScope();
			NameScope nameScope6 = new NameScope();
			NameScope nameScope7 = new NameScope();
			NameScope nameScope8 = new NameScope();
			NameScope nameScope9 = new NameScope();
			NameScope nameScope10 = new NameScope();
			nameScope.RegisterName("gridButtons", grid);
			if (grid.StyleId == null)
			{
				grid.StyleId = "gridButtons";
			}
			nameScope.RegisterName("btnBack", linkButton);
			if (linkButton.StyleId == null)
			{
				linkButton.StyleId = "btnBack";
			}
			nameScope.RegisterName("LayoutRoot", grid25);
			if (grid25.StyleId == null)
			{
				grid25.StyleId = "LayoutRoot";
			}
			nameScope.RegisterName("PreviewGrid", grid2);
			if (grid2.StyleId == null)
			{
				grid2.StyleId = "PreviewGrid";
			}
			nameScope.RegisterName("activityFrame", activityFrame);
			if (activityFrame.StyleId == null)
			{
				activityFrame.StyleId = "activityFrame";
			}
			nameScope.RegisterName("btnChangeSensor", button);
			if (button.StyleId == null)
			{
				button.StyleId = "btnChangeSensor";
			}
			nameScope.RegisterName("panelCommon", sfExpander);
			if (sfExpander.StyleId == null)
			{
				sfExpander.StyleId = "panelCommon";
			}
			nameScope.RegisterName("btnRotateGradient", button2);
			if (button2.StyleId == null)
			{
				button2.StyleId = "btnRotateGradient";
			}
			nameScope.RegisterName("panelTextItem", sfExpander2);
			if (sfExpander2.StyleId == null)
			{
				sfExpander2.StyleId = "panelTextItem";
			}
			nameScope.RegisterName("panelGauge", sfExpander3);
			if (sfExpander3.StyleId == null)
			{
				sfExpander3.StyleId = "panelGauge";
			}
			nameScope.RegisterName("panelGaugeVar2", sfExpander4);
			if (sfExpander4.StyleId == null)
			{
				sfExpander4.StyleId = "panelGaugeVar2";
			}
			nameScope.RegisterName("panelGaugeVar3", sfExpander5);
			if (sfExpander5.StyleId == null)
			{
				sfExpander5.StyleId = "panelGaugeVar3";
			}
			nameScope.RegisterName("panelChart", sfExpander6);
			if (sfExpander6.StyleId == null)
			{
				sfExpander6.StyleId = "panelChart";
			}
			nameScope.RegisterName("chartStylePicker", picker3);
			if (picker3.StyleId == null)
			{
				picker3.StyleId = "chartStylePicker";
			}
			nameScope.RegisterName("panelLinearGauge", sfExpander7);
			if (sfExpander7.StyleId == null)
			{
				sfExpander7.StyleId = "panelLinearGauge";
			}
			nameScope.RegisterName("panelMinMaxAvg", sfExpander8);
			if (sfExpander8.StyleId == null)
			{
				sfExpander8.StyleId = "panelMinMaxAvg";
			}
			nameScope.RegisterName("panelWarningAndSound", sfExpander9);
			if (sfExpander9.StyleId == null)
			{
				sfExpander9.StyleId = "panelWarningAndSound";
			}
			nameScope.RegisterName("panelApplyTo", sfExpander10);
			if (sfExpander10.StyleId == null)
			{
				sfExpander10.StyleId = "panelApplyTo";
			}
			this.gridButtons = grid;
			this.btnBack = linkButton;
			this.LayoutRoot = grid25;
			this.PreviewGrid = grid2;
			this.activityFrame = activityFrame;
			this.btnChangeSensor = button;
			this.panelCommon = sfExpander;
			this.btnRotateGradient = button2;
			this.panelTextItem = sfExpander2;
			this.panelGauge = sfExpander3;
			this.panelGaugeVar2 = sfExpander4;
			this.panelGaugeVar3 = sfExpander5;
			this.panelChart = sfExpander6;
			this.chartStylePicker = picker3;
			this.panelLinearGauge = sfExpander7;
			this.panelMinMaxAvg = sfExpander8;
			this.panelWarningAndSound = sfExpander9;
			this.panelApplyTo = sfExpander10;
			this.Resources = resourceDictionary;
			resourceDictionary.Add("PID_IDToPIDConverter", pid_IDToPIDConverter);
			resourceDictionary.Add("PID_IDToPIDNameConverter", pid_IDToPIDNameConverter);
			resourceDictionary.Add("DoubleToStringConverter", doubleToStringConverter);
			resourceDictionary.Add("ItemTypeToIndexConverter", itemTypeToIndexConverter);
			resourceDictionary.Add("TextSizeToPickerIdxConverter", textSizeToPickerIdxConverter);
			resourceDictionary.Add("BoolToNegativeConverter", boolToNegativeConverter);
			resourceDictionary.Add("ThicknessToDoubleConverter", thicknessToDoubleConverter);
			resourceDictionary.Add("PidIdToPidConverter", pidIdToPidConverter);
			resourceDictionary.Add("EnumToIntConverter", enumToIntConverter);
			enumToBoolConverter.TrueValues.Add(dashboardItemTypes);
			enumToBoolConverter.TrueValues.Add(dashboardItemTypes2);
			enumToBoolConverter.TrueValues.Add(dashboardItemTypes3);
			enumToBoolConverter.TrueValues.Add(dashboardItemTypes4);
			enumToBoolConverter.TrueValues.Add(dashboardItemTypes5);
			enumToBoolConverter.TrueValues.Add(dashboardItemTypes6);
			enumToBoolConverter.TrueValues.Add(dashboardItemTypes7);
			enumToBoolConverter.TrueValues.Add(dashboardItemTypes8);
			IMarkupExtension<IValueConverter> markupExtension = enumToBoolConverter;
			XamlServiceProvider xamlServiceProvider = new XamlServiceProvider();
			Type typeFromHandle = typeof(IProvideValueTarget);
			object[] array = new object[0 + 2];
			array[0] = resourceDictionary;
			array[1] = this;
			object obj;
			xamlServiceProvider.Add(typeFromHandle, obj = new SimpleValueTargetProvider(array, null, nameScope));
			xamlServiceProvider.Add(typeof(IReferenceProvider), obj);
			Type typeFromHandle2 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver = new XmlNamespaceResolver();
			xmlNamespaceResolver.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver.Add("dash", "clr-namespace:CarScannerXamarinForms.Dashboard");
			xmlNamespaceResolver.Add("expander", "clr-namespace:Syncfusion.XForms.Expander;assembly=Syncfusion.Expander.XForms");
			xmlNamespaceResolver.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider.Add(typeFromHandle2, new XamlTypeResolver(xmlNamespaceResolver, typeof(DashboardItemEditor).GetTypeInfo().Assembly));
			xamlServiceProvider.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(34, 14)));
			IValueConverter valueConverter = markupExtension.ProvideValue(xamlServiceProvider);
			resourceDictionary.Add("VisibleConverterItemsWithSingleSensor", valueConverter);
			enumToBoolConverter2.TrueValues.Add(dashboardItemTypes9);
			enumToBoolConverter2.TrueValues.Add(dashboardItemTypes10);
			IMarkupExtension<IValueConverter> markupExtension2 = enumToBoolConverter2;
			XamlServiceProvider xamlServiceProvider2 = new XamlServiceProvider();
			Type typeFromHandle3 = typeof(IProvideValueTarget);
			object[] array2 = new object[0 + 2];
			array2[0] = resourceDictionary;
			array2[1] = this;
			object obj2;
			xamlServiceProvider2.Add(typeFromHandle3, obj2 = new SimpleValueTargetProvider(array2, null, nameScope));
			xamlServiceProvider2.Add(typeof(IReferenceProvider), obj2);
			Type typeFromHandle4 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver2 = new XmlNamespaceResolver();
			xmlNamespaceResolver2.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver2.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver2.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver2.Add("dash", "clr-namespace:CarScannerXamarinForms.Dashboard");
			xmlNamespaceResolver2.Add("expander", "clr-namespace:Syncfusion.XForms.Expander;assembly=Syncfusion.Expander.XForms");
			xmlNamespaceResolver2.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver2.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver2.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver2.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider2.Add(typeFromHandle4, new XamlTypeResolver(xmlNamespaceResolver2, typeof(DashboardItemEditor).GetTypeInfo().Assembly));
			xamlServiceProvider2.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(47, 14)));
			IValueConverter valueConverter2 = markupExtension2.ProvideValue(xamlServiceProvider2);
			resourceDictionary.Add("VisibleConverterItemsWithMultipleSensors", valueConverter2);
			setter.Property = VisualElement.BackgroundColorProperty;
			dynamicResourceExtension2.Key = "BackgroundColor";
			IMarkupExtension<DynamicResource> markupExtension3 = dynamicResourceExtension2;
			XamlServiceProvider xamlServiceProvider3 = new XamlServiceProvider();
			Type typeFromHandle5 = typeof(IProvideValueTarget);
			object[] array3 = new object[0 + 4];
			array3[0] = setter;
			array3[1] = style;
			array3[2] = resourceDictionary;
			array3[3] = this;
			object obj3;
			xamlServiceProvider3.Add(typeFromHandle5, obj3 = new SimpleValueTargetProvider(array3, typeof(Setter).GetRuntimeProperty("Value"), nameScope2));
			xamlServiceProvider3.Add(typeof(IReferenceProvider), obj3);
			Type typeFromHandle6 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver3 = new XmlNamespaceResolver();
			xmlNamespaceResolver3.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver3.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver3.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver3.Add("dash", "clr-namespace:CarScannerXamarinForms.Dashboard");
			xmlNamespaceResolver3.Add("expander", "clr-namespace:Syncfusion.XForms.Expander;assembly=Syncfusion.Expander.XForms");
			xmlNamespaceResolver3.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver3.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver3.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver3.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider3.Add(typeFromHandle6, new XamlTypeResolver(xmlNamespaceResolver3, typeof(DashboardItemEditor).GetTypeInfo().Assembly));
			xamlServiceProvider3.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(57, 52)));
			DynamicResource dynamicResource = markupExtension3.ProvideValue(xamlServiceProvider3);
			setter.Value = dynamicResource;
			style.Setters.Add(setter);
			setter2.Property = View.MarginProperty;
			setter2.Value = "5,10";
			setter2.Value = new Thickness(5.0, 10.0);
			style.Setters.Add(setter2);
			setter3.Property = Layout.PaddingProperty;
			setter3.Value = "5,10";
			setter3.Value = new Thickness(5.0, 10.0);
			style.Setters.Add(setter3);
			resourceDictionary.Add(style);
			setter4.Property = Frame.OutlineColorProperty;
			dynamicResourceExtension3.Key = "TextColor";
			IMarkupExtension<DynamicResource> markupExtension4 = dynamicResourceExtension3;
			XamlServiceProvider xamlServiceProvider4 = new XamlServiceProvider();
			Type typeFromHandle7 = typeof(IProvideValueTarget);
			object[] array4 = new object[0 + 4];
			array4[0] = setter4;
			array4[1] = style2;
			array4[2] = resourceDictionary;
			array4[3] = this;
			object obj4;
			xamlServiceProvider4.Add(typeFromHandle7, obj4 = new SimpleValueTargetProvider(array4, typeof(Setter).GetRuntimeProperty("Value"), nameScope5));
			xamlServiceProvider4.Add(typeof(IReferenceProvider), obj4);
			Type typeFromHandle8 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver4 = new XmlNamespaceResolver();
			xmlNamespaceResolver4.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver4.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver4.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver4.Add("dash", "clr-namespace:CarScannerXamarinForms.Dashboard");
			xmlNamespaceResolver4.Add("expander", "clr-namespace:Syncfusion.XForms.Expander;assembly=Syncfusion.Expander.XForms");
			xmlNamespaceResolver4.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver4.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver4.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver4.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider4.Add(typeFromHandle8, new XamlTypeResolver(xmlNamespaceResolver4, typeof(DashboardItemEditor).GetTypeInfo().Assembly));
			xamlServiceProvider4.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(63, 49)));
			DynamicResource dynamicResource2 = markupExtension4.ProvideValue(xamlServiceProvider4);
			setter4.Value = dynamicResource2;
			style2.Setters.Add(setter4);
			setter5.Property = Layout.PaddingProperty;
			setter5.Value = "2";
			setter5.Value = new Thickness(2.0);
			style2.Setters.Add(setter5);
			setter6.Property = View.MarginProperty;
			setter6.Value = "0";
			setter6.Value = new Thickness(0.0);
			style2.Setters.Add(setter6);
			setter7.Property = VisualElement.BackgroundColorProperty;
			setter7.Value = "Transparent";
			setter7.Value = Color.Transparent;
			style2.Setters.Add(setter7);
			resourceDictionary.Add("ColorPickerFrame", style2);
			setter8.Property = Slider.ThumbColorProperty;
			dynamicResourceExtension4.Key = "SwitchOnColor";
			IMarkupExtension<DynamicResource> markupExtension5 = dynamicResourceExtension4;
			XamlServiceProvider xamlServiceProvider5 = new XamlServiceProvider();
			Type typeFromHandle9 = typeof(IProvideValueTarget);
			object[] array5 = new object[0 + 4];
			array5[0] = setter8;
			array5[1] = style3;
			array5[2] = resourceDictionary;
			array5[3] = this;
			object obj5;
			xamlServiceProvider5.Add(typeFromHandle9, obj5 = new SimpleValueTargetProvider(array5, typeof(Setter).GetRuntimeProperty("Value"), nameScope9));
			xamlServiceProvider5.Add(typeof(IReferenceProvider), obj5);
			Type typeFromHandle10 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver5 = new XmlNamespaceResolver();
			xmlNamespaceResolver5.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver5.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver5.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver5.Add("dash", "clr-namespace:CarScannerXamarinForms.Dashboard");
			xmlNamespaceResolver5.Add("expander", "clr-namespace:Syncfusion.XForms.Expander;assembly=Syncfusion.Expander.XForms");
			xmlNamespaceResolver5.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver5.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver5.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver5.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider5.Add(typeFromHandle10, new XamlTypeResolver(xmlNamespaceResolver5, typeof(DashboardItemEditor).GetTypeInfo().Assembly));
			xamlServiceProvider5.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(70, 47)));
			DynamicResource dynamicResource3 = markupExtension5.ProvideValue(xamlServiceProvider5);
			setter8.Value = dynamicResource3;
			style3.Setters.Add(setter8);
			setter9.Property = VisualElement.BackgroundColorProperty;
			dynamicResourceExtension5.Key = "SliderBackgroundColor";
			IMarkupExtension<DynamicResource> markupExtension6 = dynamicResourceExtension5;
			XamlServiceProvider xamlServiceProvider6 = new XamlServiceProvider();
			Type typeFromHandle11 = typeof(IProvideValueTarget);
			object[] array6 = new object[0 + 4];
			array6[0] = setter9;
			array6[1] = style3;
			array6[2] = resourceDictionary;
			array6[3] = this;
			object obj6;
			xamlServiceProvider6.Add(typeFromHandle11, obj6 = new SimpleValueTargetProvider(array6, typeof(Setter).GetRuntimeProperty("Value"), nameScope10));
			xamlServiceProvider6.Add(typeof(IReferenceProvider), obj6);
			Type typeFromHandle12 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver6 = new XmlNamespaceResolver();
			xmlNamespaceResolver6.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver6.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver6.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver6.Add("dash", "clr-namespace:CarScannerXamarinForms.Dashboard");
			xmlNamespaceResolver6.Add("expander", "clr-namespace:Syncfusion.XForms.Expander;assembly=Syncfusion.Expander.XForms");
			xmlNamespaceResolver6.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver6.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver6.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver6.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider6.Add(typeFromHandle12, new XamlTypeResolver(xmlNamespaceResolver6, typeof(DashboardItemEditor).GetTypeInfo().Assembly));
			xamlServiceProvider6.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(71, 52)));
			DynamicResource dynamicResource4 = markupExtension6.ProvideValue(xamlServiceProvider6);
			setter9.Value = dynamicResource4;
			style3.Setters.Add(setter9);
			resourceDictionary.Add(style3);
			this.SetValue(Page.PrefersStatusBarHiddenProperty, 0);
			this.SetValue(Page.UseSafeAreaProperty, true);
			this.Appearing += this.Handle_Appearing;
			dynamicResourceExtension.Key = "BackgroundColor";
			IMarkupExtension<DynamicResource> markupExtension7 = dynamicResourceExtension;
			XamlServiceProvider xamlServiceProvider7 = new XamlServiceProvider();
			Type typeFromHandle13 = typeof(IProvideValueTarget);
			object[] array7 = new object[0 + 1];
			array7[0] = this;
			object obj7;
			xamlServiceProvider7.Add(typeFromHandle13, obj7 = new SimpleValueTargetProvider(array7, VisualElement.BackgroundColorProperty, nameScope));
			xamlServiceProvider7.Add(typeof(IReferenceProvider), obj7);
			Type typeFromHandle14 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver7 = new XmlNamespaceResolver();
			xmlNamespaceResolver7.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver7.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver7.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver7.Add("dash", "clr-namespace:CarScannerXamarinForms.Dashboard");
			xmlNamespaceResolver7.Add("expander", "clr-namespace:Syncfusion.XForms.Expander;assembly=Syncfusion.Expander.XForms");
			xmlNamespaceResolver7.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver7.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver7.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver7.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider7.Add(typeFromHandle14, new XamlTypeResolver(xmlNamespaceResolver7, typeof(DashboardItemEditor).GetTypeInfo().Assembly));
			xamlServiceProvider7.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(17, 5)));
			DynamicResource dynamicResource5 = markupExtension7.ProvideValue(xamlServiceProvider7);
			this.SetDynamicResource(VisualElement.BackgroundColorProperty, dynamicResource5.Key);
			this.SetValue(NavigationPage.HasBackButtonProperty, false);
			this.SetValue(NavigationPage.HasNavigationBarProperty, true);
			this.SizeChanged += this.Handle_SizeChanged;
			this.Resources = resourceDictionary;
			grid.SetValue(Grid.RowProperty, 0);
			grid.SetValue(View.MarginProperty, new Thickness(5.0, 0.0, 5.0, 0.0));
			columnDefinition.SetValue(ColumnDefinition.WidthProperty, new GridLengthTypeConverter().ConvertFromInvariantString("*"));
			grid.GetValue(Grid.ColumnDefinitionsProperty).Add(columnDefinition);
			columnDefinition2.SetValue(ColumnDefinition.WidthProperty, new GridLengthTypeConverter().ConvertFromInvariantString("auto"));
			grid.GetValue(Grid.ColumnDefinitionsProperty).Add(columnDefinition2);
			columnDefinition3.SetValue(ColumnDefinition.WidthProperty, new GridLengthTypeConverter().ConvertFromInvariantString("*"));
			grid.GetValue(Grid.ColumnDefinitionsProperty).Add(columnDefinition3);
			linkButton.SetValue(Grid.ColumnProperty, 0);
			linkButton.Clicked += this.btnBack_Clicked;
			linkButton.SetValue(View.HorizontalOptionsProperty, LayoutOptions.Start);
			dynamicResourceExtension6.Key = "NavigationBarButton";
			IMarkupExtension<DynamicResource> markupExtension8 = dynamicResourceExtension6;
			XamlServiceProvider xamlServiceProvider8 = new XamlServiceProvider();
			Type typeFromHandle15 = typeof(IProvideValueTarget);
			object[] array8 = new object[0 + 3];
			array8[0] = linkButton;
			array8[1] = grid;
			array8[2] = this;
			object obj8;
			xamlServiceProvider8.Add(typeFromHandle15, obj8 = new SimpleValueTargetProvider(array8, VisualElement.StyleProperty, nameScope));
			xamlServiceProvider8.Add(typeof(IReferenceProvider), obj8);
			Type typeFromHandle16 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver8 = new XmlNamespaceResolver();
			xmlNamespaceResolver8.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver8.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver8.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver8.Add("dash", "clr-namespace:CarScannerXamarinForms.Dashboard");
			xmlNamespaceResolver8.Add("expander", "clr-namespace:Syncfusion.XForms.Expander;assembly=Syncfusion.Expander.XForms");
			xmlNamespaceResolver8.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver8.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver8.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver8.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider8.Add(typeFromHandle16, new XamlTypeResolver(xmlNamespaceResolver8, typeof(DashboardItemEditor).GetTypeInfo().Assembly));
			xamlServiceProvider8.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(92, 17)));
			DynamicResource dynamicResource6 = markupExtension8.ProvideValue(xamlServiceProvider8);
			linkButton.SetDynamicResource(VisualElement.StyleProperty, dynamicResource6.Key);
			translate.Text = "ios_Back";
			IMarkupExtension markupExtension9 = translate;
			XamlServiceProvider xamlServiceProvider9 = new XamlServiceProvider();
			Type typeFromHandle17 = typeof(IProvideValueTarget);
			object[] array9 = new object[0 + 3];
			array9[0] = linkButton;
			array9[1] = grid;
			array9[2] = this;
			object obj9;
			xamlServiceProvider9.Add(typeFromHandle17, obj9 = new SimpleValueTargetProvider(array9, Button.TextProperty, nameScope));
			xamlServiceProvider9.Add(typeof(IReferenceProvider), obj9);
			Type typeFromHandle18 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver9 = new XmlNamespaceResolver();
			xmlNamespaceResolver9.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver9.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver9.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver9.Add("dash", "clr-namespace:CarScannerXamarinForms.Dashboard");
			xmlNamespaceResolver9.Add("expander", "clr-namespace:Syncfusion.XForms.Expander;assembly=Syncfusion.Expander.XForms");
			xmlNamespaceResolver9.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver9.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver9.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver9.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider9.Add(typeFromHandle18, new XamlTypeResolver(xmlNamespaceResolver9, typeof(DashboardItemEditor).GetTypeInfo().Assembly));
			xamlServiceProvider9.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(93, 17)));
			object obj10 = markupExtension9.ProvideValue(xamlServiceProvider9);
			linkButton.Text = obj10;
			onPlatform.iOS = true;
			onPlatform.Android = true;
			linkButton.SetValue(VisualElement.IsVisibleProperty, onPlatform);
			grid.Children.Add(linkButton);
			nonScalableLabel.SetValue(Grid.ColumnProperty, 1);
			nonScalableLabel.SetValue(View.HorizontalOptionsProperty, LayoutOptions.Center);
			dynamicResourceExtension7.Key = "NavigationBarLabel";
			IMarkupExtension<DynamicResource> markupExtension10 = dynamicResourceExtension7;
			XamlServiceProvider xamlServiceProvider10 = new XamlServiceProvider();
			Type typeFromHandle19 = typeof(IProvideValueTarget);
			object[] array10 = new object[0 + 3];
			array10[0] = nonScalableLabel;
			array10[1] = grid;
			array10[2] = this;
			object obj11;
			xamlServiceProvider10.Add(typeFromHandle19, obj11 = new SimpleValueTargetProvider(array10, VisualElement.StyleProperty, nameScope));
			xamlServiceProvider10.Add(typeof(IReferenceProvider), obj11);
			Type typeFromHandle20 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver10 = new XmlNamespaceResolver();
			xmlNamespaceResolver10.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver10.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver10.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver10.Add("dash", "clr-namespace:CarScannerXamarinForms.Dashboard");
			xmlNamespaceResolver10.Add("expander", "clr-namespace:Syncfusion.XForms.Expander;assembly=Syncfusion.Expander.XForms");
			xmlNamespaceResolver10.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver10.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver10.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver10.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider10.Add(typeFromHandle20, new XamlTypeResolver(xmlNamespaceResolver10, typeof(DashboardItemEditor).GetTypeInfo().Assembly));
			xamlServiceProvider10.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(105, 17)));
			DynamicResource dynamicResource7 = markupExtension10.ProvideValue(xamlServiceProvider10);
			nonScalableLabel.SetDynamicResource(VisualElement.StyleProperty, dynamicResource7.Key);
			translate2.Text = "ios_DashboardItemEditor";
			IMarkupExtension markupExtension11 = translate2;
			XamlServiceProvider xamlServiceProvider11 = new XamlServiceProvider();
			Type typeFromHandle21 = typeof(IProvideValueTarget);
			object[] array11 = new object[0 + 3];
			array11[0] = nonScalableLabel;
			array11[1] = grid;
			array11[2] = this;
			object obj12;
			xamlServiceProvider11.Add(typeFromHandle21, obj12 = new SimpleValueTargetProvider(array11, Label.TextProperty, nameScope));
			xamlServiceProvider11.Add(typeof(IReferenceProvider), obj12);
			Type typeFromHandle22 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver11 = new XmlNamespaceResolver();
			xmlNamespaceResolver11.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver11.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver11.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver11.Add("dash", "clr-namespace:CarScannerXamarinForms.Dashboard");
			xmlNamespaceResolver11.Add("expander", "clr-namespace:Syncfusion.XForms.Expander;assembly=Syncfusion.Expander.XForms");
			xmlNamespaceResolver11.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver11.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver11.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver11.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider11.Add(typeFromHandle22, new XamlTypeResolver(xmlNamespaceResolver11, typeof(DashboardItemEditor).GetTypeInfo().Assembly));
			xamlServiceProvider11.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(106, 17)));
			object obj13 = markupExtension11.ProvideValue(xamlServiceProvider11);
			nonScalableLabel.Text = obj13;
			grid.Children.Add(nonScalableLabel);
			linkButton2.SetValue(Grid.ColumnProperty, 2);
			linkButton2.Clicked += this.btnInfo_Clicked;
			linkButton2.SetValue(View.HorizontalOptionsProperty, LayoutOptions.End);
			dynamicResourceExtension8.Key = "InfoImageNavigationBarTextColor";
			IMarkupExtension<DynamicResource> markupExtension12 = dynamicResourceExtension8;
			XamlServiceProvider xamlServiceProvider12 = new XamlServiceProvider();
			Type typeFromHandle23 = typeof(IProvideValueTarget);
			object[] array12 = new object[0 + 3];
			array12[0] = linkButton2;
			array12[1] = grid;
			array12[2] = this;
			object obj14;
			xamlServiceProvider12.Add(typeFromHandle23, obj14 = new SimpleValueTargetProvider(array12, Button.ImageProperty, nameScope));
			xamlServiceProvider12.Add(typeof(IReferenceProvider), obj14);
			Type typeFromHandle24 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver12 = new XmlNamespaceResolver();
			xmlNamespaceResolver12.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver12.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver12.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver12.Add("dash", "clr-namespace:CarScannerXamarinForms.Dashboard");
			xmlNamespaceResolver12.Add("expander", "clr-namespace:Syncfusion.XForms.Expander;assembly=Syncfusion.Expander.XForms");
			xmlNamespaceResolver12.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver12.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver12.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver12.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider12.Add(typeFromHandle24, new XamlTypeResolver(xmlNamespaceResolver12, typeof(DashboardItemEditor).GetTypeInfo().Assembly));
			xamlServiceProvider12.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(111, 17)));
			DynamicResource dynamicResource8 = markupExtension12.ProvideValue(xamlServiceProvider12);
			linkButton2.SetDynamicResource(Button.ImageProperty, dynamicResource8.Key);
			dynamicResourceExtension9.Key = "NavigationBarButton";
			IMarkupExtension<DynamicResource> markupExtension13 = dynamicResourceExtension9;
			XamlServiceProvider xamlServiceProvider13 = new XamlServiceProvider();
			Type typeFromHandle25 = typeof(IProvideValueTarget);
			object[] array13 = new object[0 + 3];
			array13[0] = linkButton2;
			array13[1] = grid;
			array13[2] = this;
			object obj15;
			xamlServiceProvider13.Add(typeFromHandle25, obj15 = new SimpleValueTargetProvider(array13, VisualElement.StyleProperty, nameScope));
			xamlServiceProvider13.Add(typeof(IReferenceProvider), obj15);
			Type typeFromHandle26 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver13 = new XmlNamespaceResolver();
			xmlNamespaceResolver13.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver13.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver13.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver13.Add("dash", "clr-namespace:CarScannerXamarinForms.Dashboard");
			xmlNamespaceResolver13.Add("expander", "clr-namespace:Syncfusion.XForms.Expander;assembly=Syncfusion.Expander.XForms");
			xmlNamespaceResolver13.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver13.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver13.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver13.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider13.Add(typeFromHandle26, new XamlTypeResolver(xmlNamespaceResolver13, typeof(DashboardItemEditor).GetTypeInfo().Assembly));
			xamlServiceProvider13.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(112, 17)));
			DynamicResource dynamicResource9 = markupExtension13.ProvideValue(xamlServiceProvider13);
			linkButton2.SetDynamicResource(VisualElement.StyleProperty, dynamicResource9.Key);
			on.Platform = new List<string>(1) { "iOS" };
			on.Value = "0,0,5,0";
			onPlatform2.Platforms.Add(on);
			on2.Platform = new List<string>(1) { "Android" };
			on2.Value = "0,0,5,0";
			onPlatform2.Platforms.Add(on2);
			linkButton2.SetValue(View.MarginProperty, onPlatform2);
			grid.Children.Add(linkButton2);
			this.SetValue(NavigationPage.TitleViewProperty, grid);
			onPlatform3.Android = new Thickness(0.0);
			onPlatform3.iOS = new Thickness(0.0);
			grid25.SetValue(View.MarginProperty, onPlatform3);
			rowDefinition.SetValue(RowDefinition.HeightProperty, new GridLengthTypeConverter().ConvertFromInvariantString("auto"));
			grid25.GetValue(Grid.RowDefinitionsProperty).Add(rowDefinition);
			rowDefinition2.SetValue(RowDefinition.HeightProperty, new GridLengthTypeConverter().ConvertFromInvariantString("0.3*"));
			grid25.GetValue(Grid.RowDefinitionsProperty).Add(rowDefinition2);
			rowDefinition3.SetValue(RowDefinition.HeightProperty, new GridLengthTypeConverter().ConvertFromInvariantString("0.7*"));
			grid25.GetValue(Grid.RowDefinitionsProperty).Add(rowDefinition3);
			grid2.SetValue(Grid.RowProperty, 1);
			grid2.SetValue(View.HorizontalOptionsProperty, LayoutOptions.FillAndExpand);
			rowDefinition4.SetValue(RowDefinition.HeightProperty, new GridLengthTypeConverter().ConvertFromInvariantString("*"));
			grid2.GetValue(Grid.RowDefinitionsProperty).Add(rowDefinition4);
			columnDefinition4.SetValue(ColumnDefinition.WidthProperty, new GridLengthTypeConverter().ConvertFromInvariantString("*"));
			grid2.GetValue(Grid.ColumnDefinitionsProperty).Add(columnDefinition4);
			columnDefinition5.SetValue(ColumnDefinition.WidthProperty, new GridLengthTypeConverter().ConvertFromInvariantString("auto"));
			grid2.GetValue(Grid.ColumnDefinitionsProperty).Add(columnDefinition5);
			columnDefinition6.SetValue(ColumnDefinition.WidthProperty, new GridLengthTypeConverter().ConvertFromInvariantString("*"));
			grid2.GetValue(Grid.ColumnDefinitionsProperty).Add(columnDefinition6);
			grid25.Children.Add(grid2);
			activityFrame.SetValue(Grid.RowProperty, 1);
			activityFrame.SetValue(VisualElement.IsVisibleProperty, new VisualElement.VisibilityConverter().ConvertFromInvariantString("false"));
			translate3.Text = "ios_SavingDashboard";
			IMarkupExtension markupExtension14 = translate3;
			XamlServiceProvider xamlServiceProvider14 = new XamlServiceProvider();
			Type typeFromHandle27 = typeof(IProvideValueTarget);
			object[] array14 = new object[0 + 3];
			array14[0] = activityFrame;
			array14[1] = grid25;
			array14[2] = this;
			object obj16;
			xamlServiceProvider14.Add(typeFromHandle27, obj16 = new SimpleValueTargetProvider(array14, ActivityFrame.TextProperty, nameScope));
			xamlServiceProvider14.Add(typeof(IReferenceProvider), obj16);
			Type typeFromHandle28 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver14 = new XmlNamespaceResolver();
			xmlNamespaceResolver14.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver14.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver14.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver14.Add("dash", "clr-namespace:CarScannerXamarinForms.Dashboard");
			xmlNamespaceResolver14.Add("expander", "clr-namespace:Syncfusion.XForms.Expander;assembly=Syncfusion.Expander.XForms");
			xmlNamespaceResolver14.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver14.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver14.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver14.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider14.Add(typeFromHandle28, new XamlTypeResolver(xmlNamespaceResolver14, typeof(DashboardItemEditor).GetTypeInfo().Assembly));
			xamlServiceProvider14.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(157, 17)));
			object obj17 = markupExtension14.ProvideValue(xamlServiceProvider14);
			activityFrame.Text = obj17;
			activityFrame.SetValue(View.VerticalOptionsProperty, LayoutOptions.Start);
			grid25.Children.Add(activityFrame);
			scrollView.SetValue(Grid.RowProperty, 2);
			dynamicResourceExtension10.Key = "DashboardItemEditorBackground";
			IMarkupExtension<DynamicResource> markupExtension15 = dynamicResourceExtension10;
			XamlServiceProvider xamlServiceProvider15 = new XamlServiceProvider();
			Type typeFromHandle29 = typeof(IProvideValueTarget);
			object[] array15 = new object[0 + 3];
			array15[0] = scrollView;
			array15[1] = grid25;
			array15[2] = this;
			object obj18;
			xamlServiceProvider15.Add(typeFromHandle29, obj18 = new SimpleValueTargetProvider(array15, VisualElement.BackgroundColorProperty, nameScope));
			xamlServiceProvider15.Add(typeof(IReferenceProvider), obj18);
			Type typeFromHandle30 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver15 = new XmlNamespaceResolver();
			xmlNamespaceResolver15.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver15.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver15.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver15.Add("dash", "clr-namespace:CarScannerXamarinForms.Dashboard");
			xmlNamespaceResolver15.Add("expander", "clr-namespace:Syncfusion.XForms.Expander;assembly=Syncfusion.Expander.XForms");
			xmlNamespaceResolver15.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver15.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver15.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver15.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider15.Add(typeFromHandle30, new XamlTypeResolver(xmlNamespaceResolver15, typeof(DashboardItemEditor).GetTypeInfo().Assembly));
			xamlServiceProvider15.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(165, 17)));
			DynamicResource dynamicResource10 = markupExtension15.ProvideValue(xamlServiceProvider15);
			scrollView.SetDynamicResource(VisualElement.BackgroundColorProperty, dynamicResource10.Key);
			scrollView.SetValue(ScrollView.OrientationProperty, 0);
			stackLayout24.SetValue(StackLayout.OrientationProperty, 0);
			stackLayout.SetValue(StackLayout.OrientationProperty, 0);
			label.SetValue(Label.FontAttributesProperty, new FontAttributesConverter().ConvertFromInvariantString("Bold"));
			translate4.Text = "ios_DashboardItemEditor_Sensor";
			IMarkupExtension markupExtension16 = translate4;
			XamlServiceProvider xamlServiceProvider16 = new XamlServiceProvider();
			Type typeFromHandle31 = typeof(IProvideValueTarget);
			object[] array16 = new object[0 + 7];
			array16[0] = label;
			array16[1] = stackLayout;
			array16[2] = frame;
			array16[3] = stackLayout24;
			array16[4] = scrollView;
			array16[5] = grid25;
			array16[6] = this;
			object obj19;
			xamlServiceProvider16.Add(typeFromHandle31, obj19 = new SimpleValueTargetProvider(array16, Label.TextProperty, nameScope));
			xamlServiceProvider16.Add(typeof(IReferenceProvider), obj19);
			Type typeFromHandle32 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver16 = new XmlNamespaceResolver();
			xmlNamespaceResolver16.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver16.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver16.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver16.Add("dash", "clr-namespace:CarScannerXamarinForms.Dashboard");
			xmlNamespaceResolver16.Add("expander", "clr-namespace:Syncfusion.XForms.Expander;assembly=Syncfusion.Expander.XForms");
			xmlNamespaceResolver16.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver16.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver16.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver16.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider16.Add(typeFromHandle32, new XamlTypeResolver(xmlNamespaceResolver16, typeof(DashboardItemEditor).GetTypeInfo().Assembly));
			xamlServiceProvider16.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(171, 58)));
			object obj20 = markupExtension16.ProvideValue(xamlServiceProvider16);
			label.Text = obj20;
			stackLayout.Children.Add(label);
			linkButton3.SetValue(View.MarginProperty, new Thickness(0.0, 0.0, 0.0, 5.0));
			linkButton3.SetValue(VisualElement.BackgroundColorProperty, Color.Transparent);
			linkButton3.SetValue(Button.BorderColorProperty, Color.Transparent);
			linkButton3.Clicked += this.btnSelectPID_Clicked;
			linkButton3.SetValue(View.HorizontalOptionsProperty, LayoutOptions.CenterAndExpand);
			staticResourceExtension.Key = "VisibleConverterItemsWithSingleSensor";
			IMarkupExtension markupExtension17 = staticResourceExtension;
			XamlServiceProvider xamlServiceProvider17 = new XamlServiceProvider();
			Type typeFromHandle33 = typeof(IProvideValueTarget);
			object[] array17 = new object[0 + 8];
			array17[0] = bindingExtension;
			array17[1] = linkButton3;
			array17[2] = stackLayout;
			array17[3] = frame;
			array17[4] = stackLayout24;
			array17[5] = scrollView;
			array17[6] = grid25;
			array17[7] = this;
			object obj21;
			xamlServiceProvider17.Add(typeFromHandle33, obj21 = new SimpleValueTargetProvider(array17, typeof(BindingExtension).GetRuntimeProperty("Converter"), nameScope));
			xamlServiceProvider17.Add(typeof(IReferenceProvider), obj21);
			Type typeFromHandle34 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver17 = new XmlNamespaceResolver();
			xmlNamespaceResolver17.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver17.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver17.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver17.Add("dash", "clr-namespace:CarScannerXamarinForms.Dashboard");
			xmlNamespaceResolver17.Add("expander", "clr-namespace:Syncfusion.XForms.Expander;assembly=Syncfusion.Expander.XForms");
			xmlNamespaceResolver17.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver17.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver17.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver17.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider17.Add(typeFromHandle34, new XamlTypeResolver(xmlNamespaceResolver17, typeof(DashboardItemEditor).GetTypeInfo().Assembly));
			xamlServiceProvider17.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(178, 33)));
			object obj22 = markupExtension17.ProvideValue(xamlServiceProvider17);
			bindingExtension.Converter = obj22;
			bindingExtension.Path = "ItemType";
			bindingExtension.TypedBinding = new TypedBinding<DashboardItem, DashboardItemTypes>(delegate(DashboardItem A_0)
			{
				if (A_0 != null)
				{
					return new ValueTuple<DashboardItemTypes, bool>(A_0.ItemType, true);
				}
				return default(ValueTuple<DashboardItemTypes, bool>);
			}, delegate(DashboardItem A_0, DashboardItemTypes A_1)
			{
				if (A_0 != null)
				{
					A_0.ItemType = A_1;
					return;
				}
			}, new Tuple<Func<DashboardItem, object>, string>[]
			{
				new Tuple<Func<DashboardItem, object>, string>((DashboardItem A_0) => A_0, "ItemType")
			});
			BindingBase bindingBase = bindingExtension.ProvideValue(null);
			linkButton3.SetBinding(VisualElement.IsVisibleProperty, bindingBase);
			bindingExtension2.Mode = 2;
			staticResourceExtension2.Key = "PID_IDToPIDNameConverter";
			IMarkupExtension markupExtension18 = staticResourceExtension2;
			XamlServiceProvider xamlServiceProvider18 = new XamlServiceProvider();
			Type typeFromHandle35 = typeof(IProvideValueTarget);
			object[] array18 = new object[0 + 8];
			array18[0] = bindingExtension2;
			array18[1] = linkButton3;
			array18[2] = stackLayout;
			array18[3] = frame;
			array18[4] = stackLayout24;
			array18[5] = scrollView;
			array18[6] = grid25;
			array18[7] = this;
			object obj23;
			xamlServiceProvider18.Add(typeFromHandle35, obj23 = new SimpleValueTargetProvider(array18, typeof(BindingExtension).GetRuntimeProperty("Converter"), nameScope));
			xamlServiceProvider18.Add(typeof(IReferenceProvider), obj23);
			Type typeFromHandle36 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver18 = new XmlNamespaceResolver();
			xmlNamespaceResolver18.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver18.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver18.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver18.Add("dash", "clr-namespace:CarScannerXamarinForms.Dashboard");
			xmlNamespaceResolver18.Add("expander", "clr-namespace:Syncfusion.XForms.Expander;assembly=Syncfusion.Expander.XForms");
			xmlNamespaceResolver18.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver18.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver18.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver18.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider18.Add(typeFromHandle36, new XamlTypeResolver(xmlNamespaceResolver18, typeof(DashboardItemEditor).GetTypeInfo().Assembly));
			xamlServiceProvider18.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(179, 33)));
			object obj24 = markupExtension18.ProvideValue(xamlServiceProvider18);
			bindingExtension2.Converter = obj24;
			bindingExtension2.Path = "PID_Id";
			bindingExtension2.TypedBinding = new TypedBinding<DashboardItem, int>(delegate(DashboardItem A_0)
			{
				if (A_0 != null)
				{
					return new ValueTuple<int, bool>(A_0.PID_Id, true);
				}
				return default(ValueTuple<int, bool>);
			}, null, new Tuple<Func<DashboardItem, object>, string>[]
			{
				new Tuple<Func<DashboardItem, object>, string>((DashboardItem A_0) => A_0, "PID_Id")
			});
			BindingBase bindingBase2 = bindingExtension2.ProvideValue(null);
			linkButton3.SetBinding(Button.TextProperty, bindingBase2);
			dynamicResourceExtension11.Key = "ButtonAccentColor";
			IMarkupExtension<DynamicResource> markupExtension19 = dynamicResourceExtension11;
			XamlServiceProvider xamlServiceProvider19 = new XamlServiceProvider();
			Type typeFromHandle37 = typeof(IProvideValueTarget);
			object[] array19 = new object[0 + 7];
			array19[0] = linkButton3;
			array19[1] = stackLayout;
			array19[2] = frame;
			array19[3] = stackLayout24;
			array19[4] = scrollView;
			array19[5] = grid25;
			array19[6] = this;
			object obj25;
			xamlServiceProvider19.Add(typeFromHandle37, obj25 = new SimpleValueTargetProvider(array19, Button.TextColorProperty, nameScope));
			xamlServiceProvider19.Add(typeof(IReferenceProvider), obj25);
			Type typeFromHandle38 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver19 = new XmlNamespaceResolver();
			xmlNamespaceResolver19.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver19.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver19.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver19.Add("dash", "clr-namespace:CarScannerXamarinForms.Dashboard");
			xmlNamespaceResolver19.Add("expander", "clr-namespace:Syncfusion.XForms.Expander;assembly=Syncfusion.Expander.XForms");
			xmlNamespaceResolver19.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver19.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver19.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver19.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider19.Add(typeFromHandle38, new XamlTypeResolver(xmlNamespaceResolver19, typeof(DashboardItemEditor).GetTypeInfo().Assembly));
			xamlServiceProvider19.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(180, 33)));
			DynamicResource dynamicResource11 = markupExtension19.ProvideValue(xamlServiceProvider19);
			linkButton3.SetDynamicResource(Button.TextColorProperty, dynamicResource11.Key);
			stackLayout.Children.Add(linkButton3);
			linkButton4.SetValue(View.MarginProperty, new Thickness(0.0, 0.0, 0.0, 5.0));
			linkButton4.SetValue(VisualElement.BackgroundColorProperty, Color.Transparent);
			linkButton4.SetValue(Button.BorderColorProperty, Color.Transparent);
			linkButton4.Clicked += this.btnSelectMultiplePIDs_Clicked;
			linkButton4.SetValue(View.HorizontalOptionsProperty, LayoutOptions.CenterAndExpand);
			staticResourceExtension3.Key = "VisibleConverterItemsWithMultipleSensors";
			IMarkupExtension markupExtension20 = staticResourceExtension3;
			XamlServiceProvider xamlServiceProvider20 = new XamlServiceProvider();
			Type typeFromHandle39 = typeof(IProvideValueTarget);
			object[] array20 = new object[0 + 8];
			array20[0] = bindingExtension3;
			array20[1] = linkButton4;
			array20[2] = stackLayout;
			array20[3] = frame;
			array20[4] = stackLayout24;
			array20[5] = scrollView;
			array20[6] = grid25;
			array20[7] = this;
			object obj26;
			xamlServiceProvider20.Add(typeFromHandle39, obj26 = new SimpleValueTargetProvider(array20, typeof(BindingExtension).GetRuntimeProperty("Converter"), nameScope));
			xamlServiceProvider20.Add(typeof(IReferenceProvider), obj26);
			Type typeFromHandle40 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver20 = new XmlNamespaceResolver();
			xmlNamespaceResolver20.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver20.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver20.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver20.Add("dash", "clr-namespace:CarScannerXamarinForms.Dashboard");
			xmlNamespaceResolver20.Add("expander", "clr-namespace:Syncfusion.XForms.Expander;assembly=Syncfusion.Expander.XForms");
			xmlNamespaceResolver20.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver20.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver20.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver20.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider20.Add(typeFromHandle40, new XamlTypeResolver(xmlNamespaceResolver20, typeof(DashboardItemEditor).GetTypeInfo().Assembly));
			xamlServiceProvider20.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(188, 33)));
			object obj27 = markupExtension20.ProvideValue(xamlServiceProvider20);
			bindingExtension3.Converter = obj27;
			bindingExtension3.Path = "ItemType";
			bindingExtension3.TypedBinding = new TypedBinding<DashboardItem, DashboardItemTypes>(delegate(DashboardItem A_0)
			{
				if (A_0 != null)
				{
					return new ValueTuple<DashboardItemTypes, bool>(A_0.ItemType, true);
				}
				return default(ValueTuple<DashboardItemTypes, bool>);
			}, delegate(DashboardItem A_0, DashboardItemTypes A_1)
			{
				if (A_0 != null)
				{
					A_0.ItemType = A_1;
					return;
				}
			}, new Tuple<Func<DashboardItem, object>, string>[]
			{
				new Tuple<Func<DashboardItem, object>, string>((DashboardItem A_0) => A_0, "ItemType")
			});
			BindingBase bindingBase3 = bindingExtension3.ProvideValue(null);
			linkButton4.SetBinding(VisualElement.IsVisibleProperty, bindingBase3);
			translate5.Text = "dash_SelectMultiple";
			IMarkupExtension markupExtension21 = translate5;
			XamlServiceProvider xamlServiceProvider21 = new XamlServiceProvider();
			Type typeFromHandle41 = typeof(IProvideValueTarget);
			object[] array21 = new object[0 + 7];
			array21[0] = linkButton4;
			array21[1] = stackLayout;
			array21[2] = frame;
			array21[3] = stackLayout24;
			array21[4] = scrollView;
			array21[5] = grid25;
			array21[6] = this;
			object obj28;
			xamlServiceProvider21.Add(typeFromHandle41, obj28 = new SimpleValueTargetProvider(array21, Button.TextProperty, nameScope));
			xamlServiceProvider21.Add(typeof(IReferenceProvider), obj28);
			Type typeFromHandle42 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver21 = new XmlNamespaceResolver();
			xmlNamespaceResolver21.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver21.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver21.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver21.Add("dash", "clr-namespace:CarScannerXamarinForms.Dashboard");
			xmlNamespaceResolver21.Add("expander", "clr-namespace:Syncfusion.XForms.Expander;assembly=Syncfusion.Expander.XForms");
			xmlNamespaceResolver21.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver21.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver21.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver21.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider21.Add(typeFromHandle42, new XamlTypeResolver(xmlNamespaceResolver21, typeof(DashboardItemEditor).GetTypeInfo().Assembly));
			xamlServiceProvider21.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(189, 33)));
			object obj29 = markupExtension21.ProvideValue(xamlServiceProvider21);
			linkButton4.Text = obj29;
			dynamicResourceExtension12.Key = "ButtonAccentColor";
			IMarkupExtension<DynamicResource> markupExtension22 = dynamicResourceExtension12;
			XamlServiceProvider xamlServiceProvider22 = new XamlServiceProvider();
			Type typeFromHandle43 = typeof(IProvideValueTarget);
			object[] array22 = new object[0 + 7];
			array22[0] = linkButton4;
			array22[1] = stackLayout;
			array22[2] = frame;
			array22[3] = stackLayout24;
			array22[4] = scrollView;
			array22[5] = grid25;
			array22[6] = this;
			object obj30;
			xamlServiceProvider22.Add(typeFromHandle43, obj30 = new SimpleValueTargetProvider(array22, Button.TextColorProperty, nameScope));
			xamlServiceProvider22.Add(typeof(IReferenceProvider), obj30);
			Type typeFromHandle44 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver22 = new XmlNamespaceResolver();
			xmlNamespaceResolver22.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver22.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver22.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver22.Add("dash", "clr-namespace:CarScannerXamarinForms.Dashboard");
			xmlNamespaceResolver22.Add("expander", "clr-namespace:Syncfusion.XForms.Expander;assembly=Syncfusion.Expander.XForms");
			xmlNamespaceResolver22.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver22.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver22.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver22.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider22.Add(typeFromHandle44, new XamlTypeResolver(xmlNamespaceResolver22, typeof(DashboardItemEditor).GetTypeInfo().Assembly));
			xamlServiceProvider22.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(190, 33)));
			DynamicResource dynamicResource12 = markupExtension22.ProvideValue(xamlServiceProvider22);
			linkButton4.SetDynamicResource(Button.TextColorProperty, dynamicResource12.Key);
			stackLayout.Children.Add(linkButton4);
			labelSwitch.SetValue(Layout.PaddingProperty, new Thickness(0.0, 5.0, 0.0, 0.0));
			bindingExtension4.Mode = 1;
			bindingExtension4.Path = "OverrideName";
			bindingExtension4.TypedBinding = new TypedBinding<DashboardItem, bool>(delegate(DashboardItem A_0)
			{
				if (A_0 != null)
				{
					return new ValueTuple<bool, bool>(A_0.OverrideName, true);
				}
				return default(ValueTuple<bool, bool>);
			}, delegate(DashboardItem A_0, bool A_1)
			{
				if (A_0 != null)
				{
					A_0.OverrideName = A_1;
					return;
				}
			}, new Tuple<Func<DashboardItem, object>, string>[]
			{
				new Tuple<Func<DashboardItem, object>, string>((DashboardItem A_0) => A_0, "OverrideName")
			});
			BindingBase bindingBase4 = bindingExtension4.ProvideValue(null);
			labelSwitch.SetBinding(LabelSwitch.IsToggledProperty, bindingBase4);
			translate6.Text = "ios_Dashboard_OverrideName";
			IMarkupExtension markupExtension23 = translate6;
			XamlServiceProvider xamlServiceProvider23 = new XamlServiceProvider();
			Type typeFromHandle45 = typeof(IProvideValueTarget);
			object[] array23 = new object[0 + 7];
			array23[0] = labelSwitch;
			array23[1] = stackLayout;
			array23[2] = frame;
			array23[3] = stackLayout24;
			array23[4] = scrollView;
			array23[5] = grid25;
			array23[6] = this;
			object obj31;
			xamlServiceProvider23.Add(typeFromHandle45, obj31 = new SimpleValueTargetProvider(array23, LabelSwitch.TextProperty, nameScope));
			xamlServiceProvider23.Add(typeof(IReferenceProvider), obj31);
			Type typeFromHandle46 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver23 = new XmlNamespaceResolver();
			xmlNamespaceResolver23.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver23.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver23.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver23.Add("dash", "clr-namespace:CarScannerXamarinForms.Dashboard");
			xmlNamespaceResolver23.Add("expander", "clr-namespace:Syncfusion.XForms.Expander;assembly=Syncfusion.Expander.XForms");
			xmlNamespaceResolver23.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver23.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver23.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver23.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider23.Add(typeFromHandle46, new XamlTypeResolver(xmlNamespaceResolver23, typeof(DashboardItemEditor).GetTypeInfo().Assembly));
			xamlServiceProvider23.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(196, 33)));
			object obj32 = markupExtension23.ProvideValue(xamlServiceProvider23);
			labelSwitch.Text = obj32;
			stackLayout.Children.Add(labelSwitch);
			bindingExtension5.Mode = 2;
			bindingExtension5.Path = "OverrideName";
			bindingExtension5.TypedBinding = new TypedBinding<DashboardItem, bool>(delegate(DashboardItem A_0)
			{
				if (A_0 != null)
				{
					return new ValueTuple<bool, bool>(A_0.OverrideName, true);
				}
				return default(ValueTuple<bool, bool>);
			}, null, new Tuple<Func<DashboardItem, object>, string>[]
			{
				new Tuple<Func<DashboardItem, object>, string>((DashboardItem A_0) => A_0, "OverrideName")
			});
			BindingBase bindingBase5 = bindingExtension5.ProvideValue(null);
			entry.SetBinding(VisualElement.IsVisibleProperty, bindingBase5);
			bindingExtension6.Mode = 1;
			bindingExtension6.Path = "CustomName";
			bindingExtension6.TypedBinding = new TypedBinding<DashboardItem, string>(delegate(DashboardItem A_0)
			{
				if (A_0 != null)
				{
					return new ValueTuple<string, bool>(A_0.CustomName, true);
				}
				return default(ValueTuple<string, bool>);
			}, delegate(DashboardItem A_0, string A_1)
			{
				if (A_0 != null)
				{
					A_0.CustomName = A_1;
					return;
				}
			}, new Tuple<Func<DashboardItem, object>, string>[]
			{
				new Tuple<Func<DashboardItem, object>, string>((DashboardItem A_0) => A_0, "CustomName")
			});
			BindingBase bindingBase6 = bindingExtension6.ProvideValue(null);
			entry.SetBinding(Entry.TextProperty, bindingBase6);
			stackLayout.Children.Add(entry);
			button.Clicked += this.btnChangeSensor_Clicked;
			translate7.Text = "dashboardItem_ChangeSensorProperties";
			IMarkupExtension markupExtension24 = translate7;
			XamlServiceProvider xamlServiceProvider24 = new XamlServiceProvider();
			Type typeFromHandle47 = typeof(IProvideValueTarget);
			object[] array24 = new object[0 + 7];
			array24[0] = button;
			array24[1] = stackLayout;
			array24[2] = frame;
			array24[3] = stackLayout24;
			array24[4] = scrollView;
			array24[5] = grid25;
			array24[6] = this;
			object obj33;
			xamlServiceProvider24.Add(typeFromHandle47, obj33 = new SimpleValueTargetProvider(array24, Button.TextProperty, nameScope));
			xamlServiceProvider24.Add(typeof(IReferenceProvider), obj33);
			Type typeFromHandle48 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver24 = new XmlNamespaceResolver();
			xmlNamespaceResolver24.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver24.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver24.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver24.Add("dash", "clr-namespace:CarScannerXamarinForms.Dashboard");
			xmlNamespaceResolver24.Add("expander", "clr-namespace:Syncfusion.XForms.Expander;assembly=Syncfusion.Expander.XForms");
			xmlNamespaceResolver24.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver24.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver24.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver24.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider24.Add(typeFromHandle48, new XamlTypeResolver(xmlNamespaceResolver24, typeof(DashboardItemEditor).GetTypeInfo().Assembly));
			xamlServiceProvider24.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(202, 33)));
			object obj34 = markupExtension24.ProvideValue(xamlServiceProvider24);
			button.Text = obj34;
			stackLayout.Children.Add(button);
			frame.SetValue(ContentView.ContentProperty, stackLayout);
			stackLayout24.Children.Add(frame);
			stackLayout2.SetValue(StackLayout.OrientationProperty, 0);
			columnDefinition7.SetValue(ColumnDefinition.WidthProperty, new GridLengthTypeConverter().ConvertFromInvariantString("auto"));
			grid3.GetValue(Grid.ColumnDefinitionsProperty).Add(columnDefinition7);
			columnDefinition8.SetValue(ColumnDefinition.WidthProperty, new GridLengthTypeConverter().ConvertFromInvariantString("*"));
			grid3.GetValue(Grid.ColumnDefinitionsProperty).Add(columnDefinition8);
			label2.SetValue(Grid.ColumnProperty, 0);
			label2.SetValue(Label.FontAttributesProperty, new FontAttributesConverter().ConvertFromInvariantString("Bold"));
			translate8.Text = "ios_DashboardItemEditor_DisplayType";
			IMarkupExtension markupExtension25 = translate8;
			XamlServiceProvider xamlServiceProvider25 = new XamlServiceProvider();
			Type typeFromHandle49 = typeof(IProvideValueTarget);
			object[] array25 = new object[0 + 8];
			array25[0] = label2;
			array25[1] = grid3;
			array25[2] = stackLayout2;
			array25[3] = frame2;
			array25[4] = stackLayout24;
			array25[5] = scrollView;
			array25[6] = grid25;
			array25[7] = this;
			object obj35;
			xamlServiceProvider25.Add(typeFromHandle49, obj35 = new SimpleValueTargetProvider(array25, Label.TextProperty, nameScope));
			xamlServiceProvider25.Add(typeof(IReferenceProvider), obj35);
			Type typeFromHandle50 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver25 = new XmlNamespaceResolver();
			xmlNamespaceResolver25.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver25.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver25.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver25.Add("dash", "clr-namespace:CarScannerXamarinForms.Dashboard");
			xmlNamespaceResolver25.Add("expander", "clr-namespace:Syncfusion.XForms.Expander;assembly=Syncfusion.Expander.XForms");
			xmlNamespaceResolver25.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver25.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver25.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver25.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider25.Add(typeFromHandle50, new XamlTypeResolver(xmlNamespaceResolver25, typeof(DashboardItemEditor).GetTypeInfo().Assembly));
			xamlServiceProvider25.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(219, 37)));
			object obj36 = markupExtension25.ProvideValue(xamlServiceProvider25);
			label2.Text = obj36;
			label2.SetValue(View.VerticalOptionsProperty, LayoutOptions.Center);
			label2.SetValue(Label.VerticalTextAlignmentProperty, new TextAlignmentConverter().ConvertFromInvariantString("Center"));
			grid3.Children.Add(label2);
			picker.SetValue(Grid.ColumnProperty, 1);
			bindingExtension7.Source = dashboardItemTypesList;
			BindingBase bindingBase7 = bindingExtension7.ProvideValue(null);
			picker.SetBinding(Picker.ItemsSourceProperty, bindingBase7);
			bindingExtension8.Mode = 1;
			staticResourceExtension4.Key = "ItemTypeToIndexConverter";
			IMarkupExtension markupExtension26 = staticResourceExtension4;
			XamlServiceProvider xamlServiceProvider26 = new XamlServiceProvider();
			Type typeFromHandle51 = typeof(IProvideValueTarget);
			object[] array26 = new object[0 + 9];
			array26[0] = bindingExtension8;
			array26[1] = picker;
			array26[2] = grid3;
			array26[3] = stackLayout2;
			array26[4] = frame2;
			array26[5] = stackLayout24;
			array26[6] = scrollView;
			array26[7] = grid25;
			array26[8] = this;
			object obj37;
			xamlServiceProvider26.Add(typeFromHandle51, obj37 = new SimpleValueTargetProvider(array26, typeof(BindingExtension).GetRuntimeProperty("Converter"), nameScope));
			xamlServiceProvider26.Add(typeof(IReferenceProvider), obj37);
			Type typeFromHandle52 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver26 = new XmlNamespaceResolver();
			xmlNamespaceResolver26.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver26.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver26.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver26.Add("dash", "clr-namespace:CarScannerXamarinForms.Dashboard");
			xmlNamespaceResolver26.Add("expander", "clr-namespace:Syncfusion.XForms.Expander;assembly=Syncfusion.Expander.XForms");
			xmlNamespaceResolver26.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver26.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver26.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver26.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider26.Add(typeFromHandle52, new XamlTypeResolver(xmlNamespaceResolver26, typeof(DashboardItemEditor).GetTypeInfo().Assembly));
			xamlServiceProvider26.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(225, 37)));
			object obj38 = markupExtension26.ProvideValue(xamlServiceProvider26);
			bindingExtension8.Converter = obj38;
			bindingExtension8.Path = "ItemType";
			bindingExtension8.TypedBinding = new TypedBinding<DashboardItem, DashboardItemTypes>(delegate(DashboardItem A_0)
			{
				if (A_0 != null)
				{
					return new ValueTuple<DashboardItemTypes, bool>(A_0.ItemType, true);
				}
				return default(ValueTuple<DashboardItemTypes, bool>);
			}, delegate(DashboardItem A_0, DashboardItemTypes A_1)
			{
				if (A_0 != null)
				{
					A_0.ItemType = A_1;
					return;
				}
			}, new Tuple<Func<DashboardItem, object>, string>[]
			{
				new Tuple<Func<DashboardItem, object>, string>((DashboardItem A_0) => A_0, "ItemType")
			});
			BindingBase bindingBase8 = bindingExtension8.ProvideValue(null);
			picker.SetBinding(Picker.SelectedIndexProperty, bindingBase8);
			picker.SelectedIndexChanged += this.ItemTypePicker_SelectedIndexChanged;
			grid3.Children.Add(picker);
			stackLayout2.Children.Add(grid3);
			frame2.SetValue(ContentView.ContentProperty, stackLayout2);
			stackLayout24.Children.Add(frame2);
			sfExpander.SetValue(SfExpander.AnimationDurationProperty, 250.0);
			sfExpander.SetValue(SfExpander.AnimationEasingProperty, 3);
			dynamicResourceExtension13.Key = "NavigationBarBackgroundColor";
			IMarkupExtension<DynamicResource> markupExtension27 = dynamicResourceExtension13;
			XamlServiceProvider xamlServiceProvider27 = new XamlServiceProvider();
			Type typeFromHandle53 = typeof(IProvideValueTarget);
			object[] array27 = new object[0 + 5];
			array27[0] = sfExpander;
			array27[1] = stackLayout24;
			array27[2] = scrollView;
			array27[3] = grid25;
			array27[4] = this;
			object obj39;
			xamlServiceProvider27.Add(typeFromHandle53, obj39 = new SimpleValueTargetProvider(array27, SfExpander.HeaderBackgroundColorProperty, nameScope));
			xamlServiceProvider27.Add(typeof(IReferenceProvider), obj39);
			Type typeFromHandle54 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver27 = new XmlNamespaceResolver();
			xmlNamespaceResolver27.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver27.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver27.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver27.Add("dash", "clr-namespace:CarScannerXamarinForms.Dashboard");
			xmlNamespaceResolver27.Add("expander", "clr-namespace:Syncfusion.XForms.Expander;assembly=Syncfusion.Expander.XForms");
			xmlNamespaceResolver27.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver27.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver27.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver27.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider27.Add(typeFromHandle54, new XamlTypeResolver(xmlNamespaceResolver27, typeof(DashboardItemEditor).GetTypeInfo().Assembly));
			xamlServiceProvider27.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(243, 25)));
			DynamicResource dynamicResource13 = markupExtension27.ProvideValue(xamlServiceProvider27);
			sfExpander.SetDynamicResource(SfExpander.HeaderBackgroundColorProperty, dynamicResource13.Key);
			dynamicResourceExtension14.Key = "NavigationBarButtonColor";
			IMarkupExtension<DynamicResource> markupExtension28 = dynamicResourceExtension14;
			XamlServiceProvider xamlServiceProvider28 = new XamlServiceProvider();
			Type typeFromHandle55 = typeof(IProvideValueTarget);
			object[] array28 = new object[0 + 5];
			array28[0] = sfExpander;
			array28[1] = stackLayout24;
			array28[2] = scrollView;
			array28[3] = grid25;
			array28[4] = this;
			object obj40;
			xamlServiceProvider28.Add(typeFromHandle55, obj40 = new SimpleValueTargetProvider(array28, SfExpander.IconColorProperty, nameScope));
			xamlServiceProvider28.Add(typeof(IReferenceProvider), obj40);
			Type typeFromHandle56 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver28 = new XmlNamespaceResolver();
			xmlNamespaceResolver28.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver28.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver28.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver28.Add("dash", "clr-namespace:CarScannerXamarinForms.Dashboard");
			xmlNamespaceResolver28.Add("expander", "clr-namespace:Syncfusion.XForms.Expander;assembly=Syncfusion.Expander.XForms");
			xmlNamespaceResolver28.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver28.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver28.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver28.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider28.Add(typeFromHandle56, new XamlTypeResolver(xmlNamespaceResolver28, typeof(DashboardItemEditor).GetTypeInfo().Assembly));
			xamlServiceProvider28.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(244, 25)));
			DynamicResource dynamicResource14 = markupExtension28.ProvideValue(xamlServiceProvider28);
			sfExpander.SetDynamicResource(SfExpander.IconColorProperty, dynamicResource14.Key);
			sfExpander.SetValue(SfExpander.IsExpandedProperty, false);
			label3.SetValue(View.MarginProperty, new Thickness(5.0));
			label3.SetValue(Label.FontAttributesProperty, new FontAttributesConverter().ConvertFromInvariantString("Bold"));
			translate9.Text = "DashItem_BackgroundAndBorder";
			IMarkupExtension markupExtension29 = translate9;
			XamlServiceProvider xamlServiceProvider29 = new XamlServiceProvider();
			Type typeFromHandle57 = typeof(IProvideValueTarget);
			object[] array29 = new object[0 + 7];
			array29[0] = label3;
			array29[1] = grid4;
			array29[2] = sfExpander;
			array29[3] = stackLayout24;
			array29[4] = scrollView;
			array29[5] = grid25;
			array29[6] = this;
			object obj41;
			xamlServiceProvider29.Add(typeFromHandle57, obj41 = new SimpleValueTargetProvider(array29, Label.TextProperty, nameScope));
			xamlServiceProvider29.Add(typeof(IReferenceProvider), obj41);
			Type typeFromHandle58 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver29 = new XmlNamespaceResolver();
			xmlNamespaceResolver29.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver29.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver29.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver29.Add("dash", "clr-namespace:CarScannerXamarinForms.Dashboard");
			xmlNamespaceResolver29.Add("expander", "clr-namespace:Syncfusion.XForms.Expander;assembly=Syncfusion.Expander.XForms");
			xmlNamespaceResolver29.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver29.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver29.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver29.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider29.Add(typeFromHandle58, new XamlTypeResolver(xmlNamespaceResolver29, typeof(DashboardItemEditor).GetTypeInfo().Assembly));
			xamlServiceProvider29.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(251, 37)));
			object obj42 = markupExtension29.ProvideValue(xamlServiceProvider29);
			label3.Text = obj42;
			dynamicResourceExtension15.Key = "NavigationBarTextColor";
			IMarkupExtension<DynamicResource> markupExtension30 = dynamicResourceExtension15;
			XamlServiceProvider xamlServiceProvider30 = new XamlServiceProvider();
			Type typeFromHandle59 = typeof(IProvideValueTarget);
			object[] array30 = new object[0 + 7];
			array30[0] = label3;
			array30[1] = grid4;
			array30[2] = sfExpander;
			array30[3] = stackLayout24;
			array30[4] = scrollView;
			array30[5] = grid25;
			array30[6] = this;
			object obj43;
			xamlServiceProvider30.Add(typeFromHandle59, obj43 = new SimpleValueTargetProvider(array30, Label.TextColorProperty, nameScope));
			xamlServiceProvider30.Add(typeof(IReferenceProvider), obj43);
			Type typeFromHandle60 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver30 = new XmlNamespaceResolver();
			xmlNamespaceResolver30.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver30.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver30.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver30.Add("dash", "clr-namespace:CarScannerXamarinForms.Dashboard");
			xmlNamespaceResolver30.Add("expander", "clr-namespace:Syncfusion.XForms.Expander;assembly=Syncfusion.Expander.XForms");
			xmlNamespaceResolver30.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver30.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver30.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver30.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider30.Add(typeFromHandle60, new XamlTypeResolver(xmlNamespaceResolver30, typeof(DashboardItemEditor).GetTypeInfo().Assembly));
			xamlServiceProvider30.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(252, 37)));
			DynamicResource dynamicResource15 = markupExtension30.ProvideValue(xamlServiceProvider30);
			label3.SetDynamicResource(Label.TextColorProperty, dynamicResource15.Key);
			label3.SetValue(View.VerticalOptionsProperty, LayoutOptions.Center);
			label3.SetValue(Label.VerticalTextAlignmentProperty, new TextAlignmentConverter().ConvertFromInvariantString("Center"));
			grid4.Children.Add(label3);
			sfExpander.SetValue(SfExpander.HeaderProperty, grid4);
			stackLayout4.SetValue(StackLayout.OrientationProperty, 0);
			bindingExtension9.Mode = 1;
			bindingExtension9.Path = "ShowDefaultBackground";
			bindingExtension9.TypedBinding = new TypedBinding<DashboardItem, bool>(delegate(DashboardItem A_0)
			{
				if (A_0 != null)
				{
					return new ValueTuple<bool, bool>(A_0.ShowDefaultBackground, true);
				}
				return default(ValueTuple<bool, bool>);
			}, delegate(DashboardItem A_0, bool A_1)
			{
				if (A_0 != null)
				{
					A_0.ShowDefaultBackground = A_1;
					return;
				}
			}, new Tuple<Func<DashboardItem, object>, string>[]
			{
				new Tuple<Func<DashboardItem, object>, string>((DashboardItem A_0) => A_0, "ShowDefaultBackground")
			});
			BindingBase bindingBase9 = bindingExtension9.ProvideValue(null);
			labelSwitch2.SetBinding(LabelSwitch.IsToggledProperty, bindingBase9);
			translate10.Text = "ios_DashboardItemEditor_UseDefaultBackground";
			IMarkupExtension markupExtension31 = translate10;
			XamlServiceProvider xamlServiceProvider31 = new XamlServiceProvider();
			Type typeFromHandle61 = typeof(IProvideValueTarget);
			object[] array31 = new object[0 + 8];
			array31[0] = labelSwitch2;
			array31[1] = stackLayout4;
			array31[2] = frame3;
			array31[3] = sfExpander;
			array31[4] = stackLayout24;
			array31[5] = scrollView;
			array31[6] = grid25;
			array31[7] = this;
			object obj44;
			xamlServiceProvider31.Add(typeFromHandle61, obj44 = new SimpleValueTargetProvider(array31, LabelSwitch.TextProperty, nameScope));
			xamlServiceProvider31.Add(typeof(IReferenceProvider), obj44);
			Type typeFromHandle62 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver31 = new XmlNamespaceResolver();
			xmlNamespaceResolver31.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver31.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver31.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver31.Add("dash", "clr-namespace:CarScannerXamarinForms.Dashboard");
			xmlNamespaceResolver31.Add("expander", "clr-namespace:Syncfusion.XForms.Expander;assembly=Syncfusion.Expander.XForms");
			xmlNamespaceResolver31.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver31.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver31.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver31.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider31.Add(typeFromHandle62, new XamlTypeResolver(xmlNamespaceResolver31, typeof(DashboardItemEditor).GetTypeInfo().Assembly));
			xamlServiceProvider31.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(263, 113)));
			object obj45 = markupExtension31.ProvideValue(xamlServiceProvider31);
			labelSwitch2.Text = obj45;
			stackLayout4.Children.Add(labelSwitch2);
			bindingExtension10.Mode = 2;
			staticResourceExtension5.Key = "BoolToNegativeConverter";
			IMarkupExtension markupExtension32 = staticResourceExtension5;
			XamlServiceProvider xamlServiceProvider32 = new XamlServiceProvider();
			Type typeFromHandle63 = typeof(IProvideValueTarget);
			object[] array32 = new object[0 + 9];
			array32[0] = bindingExtension10;
			array32[1] = dashboardItemColorBox;
			array32[2] = stackLayout4;
			array32[3] = frame3;
			array32[4] = sfExpander;
			array32[5] = stackLayout24;
			array32[6] = scrollView;
			array32[7] = grid25;
			array32[8] = this;
			object obj46;
			xamlServiceProvider32.Add(typeFromHandle63, obj46 = new SimpleValueTargetProvider(array32, typeof(BindingExtension).GetRuntimeProperty("Converter"), nameScope));
			xamlServiceProvider32.Add(typeof(IReferenceProvider), obj46);
			Type typeFromHandle64 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver32 = new XmlNamespaceResolver();
			xmlNamespaceResolver32.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver32.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver32.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver32.Add("dash", "clr-namespace:CarScannerXamarinForms.Dashboard");
			xmlNamespaceResolver32.Add("expander", "clr-namespace:Syncfusion.XForms.Expander;assembly=Syncfusion.Expander.XForms");
			xmlNamespaceResolver32.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver32.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver32.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver32.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider32.Add(typeFromHandle64, new XamlTypeResolver(xmlNamespaceResolver32, typeof(DashboardItemEditor).GetTypeInfo().Assembly));
			xamlServiceProvider32.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(270, 41)));
			object obj47 = markupExtension32.ProvideValue(xamlServiceProvider32);
			bindingExtension10.Converter = obj47;
			bindingExtension10.Path = "ShowDefaultBackground";
			bindingExtension10.TypedBinding = new TypedBinding<DashboardItem, bool>(delegate(DashboardItem A_0)
			{
				if (A_0 != null)
				{
					return new ValueTuple<bool, bool>(A_0.ShowDefaultBackground, true);
				}
				return default(ValueTuple<bool, bool>);
			}, null, new Tuple<Func<DashboardItem, object>, string>[]
			{
				new Tuple<Func<DashboardItem, object>, string>((DashboardItem A_0) => A_0, "ShowDefaultBackground")
			});
			BindingBase bindingBase10 = bindingExtension10.ProvideValue(null);
			dashboardItemColorBox.SetBinding(VisualElement.IsVisibleProperty, bindingBase10);
			dashboardItemColorBox.Tapped += this.BackgroundColor_Tapped;
			translate11.Text = "ios_DashboardItemEditor_BackgroundColor";
			IMarkupExtension markupExtension33 = translate11;
			XamlServiceProvider xamlServiceProvider33 = new XamlServiceProvider();
			Type typeFromHandle65 = typeof(IProvideValueTarget);
			object[] array33 = new object[0 + 8];
			array33[0] = dashboardItemColorBox;
			array33[1] = stackLayout4;
			array33[2] = frame3;
			array33[3] = sfExpander;
			array33[4] = stackLayout24;
			array33[5] = scrollView;
			array33[6] = grid25;
			array33[7] = this;
			object obj48;
			xamlServiceProvider33.Add(typeFromHandle65, obj48 = new SimpleValueTargetProvider(array33, DashboardItemColorBox.TextProperty, nameScope));
			xamlServiceProvider33.Add(typeof(IReferenceProvider), obj48);
			Type typeFromHandle66 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver33 = new XmlNamespaceResolver();
			xmlNamespaceResolver33.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver33.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver33.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver33.Add("dash", "clr-namespace:CarScannerXamarinForms.Dashboard");
			xmlNamespaceResolver33.Add("expander", "clr-namespace:Syncfusion.XForms.Expander;assembly=Syncfusion.Expander.XForms");
			xmlNamespaceResolver33.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver33.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver33.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver33.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider33.Add(typeFromHandle66, new XamlTypeResolver(xmlNamespaceResolver33, typeof(DashboardItemEditor).GetTypeInfo().Assembly));
			xamlServiceProvider33.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(272, 41)));
			object obj49 = markupExtension33.ProvideValue(xamlServiceProvider33);
			dashboardItemColorBox.Text = obj49;
			bindingExtension11.Mode = 2;
			bindingExtension11.Path = "BackgroundColor";
			bindingExtension11.TypedBinding = new TypedBinding<DashboardItem, Color>(delegate(DashboardItem A_0)
			{
				if (A_0 != null)
				{
					return new ValueTuple<Color, bool>(A_0.BackgroundColor, true);
				}
				return default(ValueTuple<Color, bool>);
			}, null, new Tuple<Func<DashboardItem, object>, string>[]
			{
				new Tuple<Func<DashboardItem, object>, string>((DashboardItem A_0) => A_0, "BackgroundColor")
			});
			BindingBase bindingBase11 = bindingExtension11.ProvideValue(null);
			dashboardItemColorBox.SetBinding(DashboardItemColorBox.ColorProperty, bindingBase11);
			stackLayout4.Children.Add(dashboardItemColorBox);
			bindingExtension12.Mode = 2;
			bindingExtension12.Path = "ShowDefaultBackground";
			bindingExtension12.TypedBinding = new TypedBinding<DashboardItem, bool>(delegate(DashboardItem A_0)
			{
				if (A_0 != null)
				{
					return new ValueTuple<bool, bool>(A_0.ShowDefaultBackground, true);
				}
				return default(ValueTuple<bool, bool>);
			}, null, new Tuple<Func<DashboardItem, object>, string>[]
			{
				new Tuple<Func<DashboardItem, object>, string>((DashboardItem A_0) => A_0, "ShowDefaultBackground")
			});
			BindingBase bindingBase12 = bindingExtension12.ProvideValue(null);
			stackLayout3.SetBinding(VisualElement.IsVisibleProperty, bindingBase12);
			stackLayout3.SetValue(StackLayout.OrientationProperty, 0);
			translate12.Text = "ios_GradientColors";
			IMarkupExtension markupExtension34 = translate12;
			XamlServiceProvider xamlServiceProvider34 = new XamlServiceProvider();
			Type typeFromHandle67 = typeof(IProvideValueTarget);
			object[] array34 = new object[0 + 9];
			array34[0] = label4;
			array34[1] = stackLayout3;
			array34[2] = stackLayout4;
			array34[3] = frame3;
			array34[4] = sfExpander;
			array34[5] = stackLayout24;
			array34[6] = scrollView;
			array34[7] = grid25;
			array34[8] = this;
			object obj50;
			xamlServiceProvider34.Add(typeFromHandle67, obj50 = new SimpleValueTargetProvider(array34, Label.TextProperty, nameScope));
			xamlServiceProvider34.Add(typeof(IReferenceProvider), obj50);
			Type typeFromHandle68 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver34 = new XmlNamespaceResolver();
			xmlNamespaceResolver34.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver34.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver34.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver34.Add("dash", "clr-namespace:CarScannerXamarinForms.Dashboard");
			xmlNamespaceResolver34.Add("expander", "clr-namespace:Syncfusion.XForms.Expander;assembly=Syncfusion.Expander.XForms");
			xmlNamespaceResolver34.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver34.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver34.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver34.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider34.Add(typeFromHandle68, new XamlTypeResolver(xmlNamespaceResolver34, typeof(DashboardItemEditor).GetTypeInfo().Assembly));
			xamlServiceProvider34.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(275, 48)));
			object obj51 = markupExtension34.ProvideValue(xamlServiceProvider34);
			label4.Text = obj51;
			stackLayout3.Children.Add(label4);
			dashboardItemColorBox2.SetValue(Element.ClassIdProperty, "GradientColor1");
			dashboardItemColorBox2.Tapped += this.ColorBox_Tapped;
			dashboardItemColorBox2.SetValue(DashboardItemColorBox.TextProperty, "#1");
			bindingExtension13.Mode = 1;
			bindingExtension13.Path = "GradientColor1";
			bindingExtension13.TypedBinding = new TypedBinding<DashboardItem, Color>(delegate(DashboardItem A_0)
			{
				if (A_0 != null)
				{
					return new ValueTuple<Color, bool>(A_0.GradientColor1, true);
				}
				return default(ValueTuple<Color, bool>);
			}, delegate(DashboardItem A_0, Color A_1)
			{
				if (A_0 != null)
				{
					A_0.GradientColor1 = A_1;
					return;
				}
			}, new Tuple<Func<DashboardItem, object>, string>[]
			{
				new Tuple<Func<DashboardItem, object>, string>((DashboardItem A_0) => A_0, "GradientColor1")
			});
			BindingBase bindingBase13 = bindingExtension13.ProvideValue(null);
			dashboardItemColorBox2.SetBinding(DashboardItemColorBox.ColorProperty, bindingBase13);
			stackLayout3.Children.Add(dashboardItemColorBox2);
			dashboardItemColorBox3.SetValue(Element.ClassIdProperty, "GradientColor2");
			dashboardItemColorBox3.Tapped += this.ColorBox_Tapped;
			dashboardItemColorBox3.SetValue(DashboardItemColorBox.TextProperty, "#2");
			bindingExtension14.Mode = 1;
			bindingExtension14.Path = "GradientColor2";
			bindingExtension14.TypedBinding = new TypedBinding<DashboardItem, Color>(delegate(DashboardItem A_0)
			{
				if (A_0 != null)
				{
					return new ValueTuple<Color, bool>(A_0.GradientColor2, true);
				}
				return default(ValueTuple<Color, bool>);
			}, delegate(DashboardItem A_0, Color A_1)
			{
				if (A_0 != null)
				{
					A_0.GradientColor2 = A_1;
					return;
				}
			}, new Tuple<Func<DashboardItem, object>, string>[]
			{
				new Tuple<Func<DashboardItem, object>, string>((DashboardItem A_0) => A_0, "GradientColor2")
			});
			BindingBase bindingBase14 = bindingExtension14.ProvideValue(null);
			dashboardItemColorBox3.SetBinding(DashboardItemColorBox.ColorProperty, bindingBase14);
			stackLayout3.Children.Add(dashboardItemColorBox3);
			translate13.Text = "ios_GradientPoints";
			IMarkupExtension markupExtension35 = translate13;
			XamlServiceProvider xamlServiceProvider35 = new XamlServiceProvider();
			Type typeFromHandle69 = typeof(IProvideValueTarget);
			object[] array35 = new object[0 + 9];
			array35[0] = label5;
			array35[1] = stackLayout3;
			array35[2] = stackLayout4;
			array35[3] = frame3;
			array35[4] = sfExpander;
			array35[5] = stackLayout24;
			array35[6] = scrollView;
			array35[7] = grid25;
			array35[8] = this;
			object obj52;
			xamlServiceProvider35.Add(typeFromHandle69, obj52 = new SimpleValueTargetProvider(array35, Label.TextProperty, nameScope));
			xamlServiceProvider35.Add(typeof(IReferenceProvider), obj52);
			Type typeFromHandle70 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver35 = new XmlNamespaceResolver();
			xmlNamespaceResolver35.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver35.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver35.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver35.Add("dash", "clr-namespace:CarScannerXamarinForms.Dashboard");
			xmlNamespaceResolver35.Add("expander", "clr-namespace:Syncfusion.XForms.Expander;assembly=Syncfusion.Expander.XForms");
			xmlNamespaceResolver35.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver35.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver35.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver35.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider35.Add(typeFromHandle70, new XamlTypeResolver(xmlNamespaceResolver35, typeof(DashboardItemEditor).GetTypeInfo().Assembly));
			xamlServiceProvider35.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(286, 48)));
			object obj53 = markupExtension35.ProvideValue(xamlServiceProvider35);
			label5.Text = obj53;
			stackLayout3.Children.Add(label5);
			extendedSlider.SetValue(Grid.RowProperty, 1);
			extendedSlider.SetValue(Grid.ColumnProperty, 0);
			extendedSlider.SetValue(Grid.ColumnSpanProperty, 2);
			extendedSlider.SetValue(Slider.MaximumProperty, 1.0);
			extendedSlider.SetValue(Slider.MinimumProperty, 0.0);
			extendedSlider.StepValue = 0.01;
			bindingExtension15.Mode = 1;
			bindingExtension15.Path = "GradientOffsetPoint1";
			bindingExtension15.TypedBinding = new TypedBinding<DashboardItem, double>(delegate(DashboardItem A_0)
			{
				if (A_0 != null)
				{
					return new ValueTuple<double, bool>(A_0.GradientOffsetPoint1, true);
				}
				return default(ValueTuple<double, bool>);
			}, delegate(DashboardItem A_0, double A_1)
			{
				if (A_0 != null)
				{
					A_0.GradientOffsetPoint1 = A_1;
					return;
				}
			}, new Tuple<Func<DashboardItem, object>, string>[]
			{
				new Tuple<Func<DashboardItem, object>, string>((DashboardItem A_0) => A_0, "GradientOffsetPoint1")
			});
			BindingBase bindingBase15 = bindingExtension15.ProvideValue(null);
			extendedSlider.SetBinding(Slider.ValueProperty, bindingBase15);
			stackLayout3.Children.Add(extendedSlider);
			label6.SetValue(Label.TextProperty, "Gradient offset 2:");
			stackLayout3.Children.Add(label6);
			extendedSlider2.SetValue(Grid.RowProperty, 1);
			extendedSlider2.SetValue(Grid.ColumnProperty, 0);
			extendedSlider2.SetValue(Grid.ColumnSpanProperty, 2);
			extendedSlider2.SetValue(Slider.MaximumProperty, 1.0);
			extendedSlider2.SetValue(Slider.MinimumProperty, 0.0);
			extendedSlider2.StepValue = 0.01;
			bindingExtension16.Mode = 1;
			bindingExtension16.Path = "GradientOffsetPoint2";
			bindingExtension16.TypedBinding = new TypedBinding<DashboardItem, double>(delegate(DashboardItem A_0)
			{
				if (A_0 != null)
				{
					return new ValueTuple<double, bool>(A_0.GradientOffsetPoint2, true);
				}
				return default(ValueTuple<double, bool>);
			}, delegate(DashboardItem A_0, double A_1)
			{
				if (A_0 != null)
				{
					A_0.GradientOffsetPoint2 = A_1;
					return;
				}
			}, new Tuple<Func<DashboardItem, object>, string>[]
			{
				new Tuple<Func<DashboardItem, object>, string>((DashboardItem A_0) => A_0, "GradientOffsetPoint2")
			});
			BindingBase bindingBase16 = bindingExtension16.ProvideValue(null);
			extendedSlider2.SetBinding(Slider.ValueProperty, bindingBase16);
			stackLayout3.Children.Add(extendedSlider2);
			button2.Clicked += this.btnRotateGradient_Clicked;
			translate14.Text = "ios_GradientRotate";
			IMarkupExtension markupExtension36 = translate14;
			XamlServiceProvider xamlServiceProvider36 = new XamlServiceProvider();
			Type typeFromHandle71 = typeof(IProvideValueTarget);
			object[] array36 = new object[0 + 9];
			array36[0] = button2;
			array36[1] = stackLayout3;
			array36[2] = stackLayout4;
			array36[3] = frame3;
			array36[4] = sfExpander;
			array36[5] = stackLayout24;
			array36[6] = scrollView;
			array36[7] = grid25;
			array36[8] = this;
			object obj54;
			xamlServiceProvider36.Add(typeFromHandle71, obj54 = new SimpleValueTargetProvider(array36, Button.TextProperty, nameScope));
			xamlServiceProvider36.Add(typeof(IReferenceProvider), obj54);
			Type typeFromHandle72 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver36 = new XmlNamespaceResolver();
			xmlNamespaceResolver36.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver36.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver36.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver36.Add("dash", "clr-namespace:CarScannerXamarinForms.Dashboard");
			xmlNamespaceResolver36.Add("expander", "clr-namespace:Syncfusion.XForms.Expander;assembly=Syncfusion.Expander.XForms");
			xmlNamespaceResolver36.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver36.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver36.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver36.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider36.Add(typeFromHandle72, new XamlTypeResolver(xmlNamespaceResolver36, typeof(DashboardItemEditor).GetTypeInfo().Assembly));
			xamlServiceProvider36.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(310, 45)));
			object obj55 = markupExtension36.ProvideValue(xamlServiceProvider36);
			button2.Text = obj55;
			stackLayout3.Children.Add(button2);
			stackLayout4.Children.Add(stackLayout3);
			dashboardItemColorBox4.Tapped += this.FrameColor_Tapped;
			translate15.Text = "ios_DashboardItemEditor_BorderColor";
			IMarkupExtension markupExtension37 = translate15;
			XamlServiceProvider xamlServiceProvider37 = new XamlServiceProvider();
			Type typeFromHandle73 = typeof(IProvideValueTarget);
			object[] array37 = new object[0 + 8];
			array37[0] = dashboardItemColorBox4;
			array37[1] = stackLayout4;
			array37[2] = frame3;
			array37[3] = sfExpander;
			array37[4] = stackLayout24;
			array37[5] = scrollView;
			array37[6] = grid25;
			array37[7] = this;
			object obj56;
			xamlServiceProvider37.Add(typeFromHandle73, obj56 = new SimpleValueTargetProvider(array37, DashboardItemColorBox.TextProperty, nameScope));
			xamlServiceProvider37.Add(typeof(IReferenceProvider), obj56);
			Type typeFromHandle74 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver37 = new XmlNamespaceResolver();
			xmlNamespaceResolver37.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver37.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver37.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver37.Add("dash", "clr-namespace:CarScannerXamarinForms.Dashboard");
			xmlNamespaceResolver37.Add("expander", "clr-namespace:Syncfusion.XForms.Expander;assembly=Syncfusion.Expander.XForms");
			xmlNamespaceResolver37.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver37.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver37.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver37.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider37.Add(typeFromHandle74, new XamlTypeResolver(xmlNamespaceResolver37, typeof(DashboardItemEditor).GetTypeInfo().Assembly));
			xamlServiceProvider37.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(317, 41)));
			object obj57 = markupExtension37.ProvideValue(xamlServiceProvider37);
			dashboardItemColorBox4.Text = obj57;
			bindingExtension17.Mode = 2;
			bindingExtension17.Path = "FrameColor";
			bindingExtension17.TypedBinding = new TypedBinding<DashboardItem, Color>(delegate(DashboardItem A_0)
			{
				if (A_0 != null)
				{
					return new ValueTuple<Color, bool>(A_0.FrameColor, true);
				}
				return default(ValueTuple<Color, bool>);
			}, null, new Tuple<Func<DashboardItem, object>, string>[]
			{
				new Tuple<Func<DashboardItem, object>, string>((DashboardItem A_0) => A_0, "FrameColor")
			});
			BindingBase bindingBase17 = bindingExtension17.ProvideValue(null);
			dashboardItemColorBox4.SetBinding(DashboardItemColorBox.ColorProperty, bindingBase17);
			stackLayout4.Children.Add(dashboardItemColorBox4);
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
			label7.SetValue(Grid.RowProperty, 0);
			label7.SetValue(Grid.ColumnProperty, 0);
			translate16.Text = "ios_DashboardItemEditor_BorderSize";
			IMarkupExtension markupExtension38 = translate16;
			XamlServiceProvider xamlServiceProvider38 = new XamlServiceProvider();
			Type typeFromHandle75 = typeof(IProvideValueTarget);
			object[] array38 = new object[0 + 9];
			array38[0] = label7;
			array38[1] = grid5;
			array38[2] = stackLayout4;
			array38[3] = frame3;
			array38[4] = sfExpander;
			array38[5] = stackLayout24;
			array38[6] = scrollView;
			array38[7] = grid25;
			array38[8] = this;
			object obj58;
			xamlServiceProvider38.Add(typeFromHandle75, obj58 = new SimpleValueTargetProvider(array38, Label.TextProperty, nameScope));
			xamlServiceProvider38.Add(typeof(IReferenceProvider), obj58);
			Type typeFromHandle76 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver38 = new XmlNamespaceResolver();
			xmlNamespaceResolver38.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver38.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver38.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver38.Add("dash", "clr-namespace:CarScannerXamarinForms.Dashboard");
			xmlNamespaceResolver38.Add("expander", "clr-namespace:Syncfusion.XForms.Expander;assembly=Syncfusion.Expander.XForms");
			xmlNamespaceResolver38.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver38.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver38.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver38.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider38.Add(typeFromHandle76, new XamlTypeResolver(xmlNamespaceResolver38, typeof(DashboardItemEditor).GetTypeInfo().Assembly));
			xamlServiceProvider38.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(336, 45)));
			object obj59 = markupExtension38.ProvideValue(xamlServiceProvider38);
			label7.Text = obj59;
			grid5.Children.Add(label7);
			label8.SetValue(Grid.RowProperty, 0);
			label8.SetValue(Grid.ColumnProperty, 1);
			label8.SetValue(Label.FontAttributesProperty, new FontAttributesConverter().ConvertFromInvariantString("Bold"));
			bindingExtension18.Mode = 2;
			bindingExtension18.Path = "FrameSize";
			bindingExtension18.TypedBinding = new TypedBinding<DashboardItem, double>(delegate(DashboardItem A_0)
			{
				if (A_0 != null)
				{
					return new ValueTuple<double, bool>(A_0.FrameSize, true);
				}
				return default(ValueTuple<double, bool>);
			}, null, new Tuple<Func<DashboardItem, object>, string>[]
			{
				new Tuple<Func<DashboardItem, object>, string>((DashboardItem A_0) => A_0, "FrameSize")
			});
			BindingBase bindingBase18 = bindingExtension18.ProvideValue(null);
			label8.SetBinding(Label.TextProperty, bindingBase18);
			label8.SetValue(View.VerticalOptionsProperty, LayoutOptions.Center);
			grid5.Children.Add(label8);
			extendedSlider3.SetValue(Grid.RowProperty, 1);
			extendedSlider3.SetValue(Grid.ColumnProperty, 0);
			extendedSlider3.SetValue(Grid.ColumnSpanProperty, 2);
			extendedSlider3.SetValue(Slider.MaximumProperty, 50.0);
			extendedSlider3.SetValue(Slider.MinimumProperty, 0.0);
			extendedSlider3.StepValue = 1.0;
			bindingExtension19.Mode = 1;
			bindingExtension19.Path = "FrameSize";
			bindingExtension19.TypedBinding = new TypedBinding<DashboardItem, double>(delegate(DashboardItem A_0)
			{
				if (A_0 != null)
				{
					return new ValueTuple<double, bool>(A_0.FrameSize, true);
				}
				return default(ValueTuple<double, bool>);
			}, delegate(DashboardItem A_0, double A_1)
			{
				if (A_0 != null)
				{
					A_0.FrameSize = A_1;
					return;
				}
			}, new Tuple<Func<DashboardItem, object>, string>[]
			{
				new Tuple<Func<DashboardItem, object>, string>((DashboardItem A_0) => A_0, "FrameSize")
			});
			BindingBase bindingBase19 = bindingExtension19.ProvideValue(null);
			extendedSlider3.SetBinding(Slider.ValueProperty, bindingBase19);
			grid5.Children.Add(extendedSlider3);
			stackLayout4.Children.Add(grid5);
			grid6.SetValue(View.MarginProperty, new Thickness(0.0, 0.0, 0.0, 10.0));
			grid6.SetValue(Grid.RowSpacingProperty, 0.0);
			columnDefinition11.SetValue(ColumnDefinition.WidthProperty, new GridLengthTypeConverter().ConvertFromInvariantString("*"));
			grid6.GetValue(Grid.ColumnDefinitionsProperty).Add(columnDefinition11);
			columnDefinition12.SetValue(ColumnDefinition.WidthProperty, new GridLengthTypeConverter().ConvertFromInvariantString("auto"));
			grid6.GetValue(Grid.ColumnDefinitionsProperty).Add(columnDefinition12);
			rowDefinition7.SetValue(RowDefinition.HeightProperty, new GridLengthTypeConverter().ConvertFromInvariantString("auto"));
			grid6.GetValue(Grid.RowDefinitionsProperty).Add(rowDefinition7);
			rowDefinition8.SetValue(RowDefinition.HeightProperty, new GridLengthTypeConverter().ConvertFromInvariantString("auto"));
			grid6.GetValue(Grid.RowDefinitionsProperty).Add(rowDefinition8);
			label9.SetValue(Grid.RowProperty, 0);
			label9.SetValue(Grid.ColumnProperty, 0);
			translate17.Text = "ios_CornerRadius";
			IMarkupExtension markupExtension39 = translate17;
			XamlServiceProvider xamlServiceProvider39 = new XamlServiceProvider();
			Type typeFromHandle77 = typeof(IProvideValueTarget);
			object[] array39 = new object[0 + 9];
			array39[0] = label9;
			array39[1] = grid6;
			array39[2] = stackLayout4;
			array39[3] = frame3;
			array39[4] = sfExpander;
			array39[5] = stackLayout24;
			array39[6] = scrollView;
			array39[7] = grid25;
			array39[8] = this;
			object obj60;
			xamlServiceProvider39.Add(typeFromHandle77, obj60 = new SimpleValueTargetProvider(array39, Label.TextProperty, nameScope));
			xamlServiceProvider39.Add(typeof(IReferenceProvider), obj60);
			Type typeFromHandle78 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver39 = new XmlNamespaceResolver();
			xmlNamespaceResolver39.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver39.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver39.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver39.Add("dash", "clr-namespace:CarScannerXamarinForms.Dashboard");
			xmlNamespaceResolver39.Add("expander", "clr-namespace:Syncfusion.XForms.Expander;assembly=Syncfusion.Expander.XForms");
			xmlNamespaceResolver39.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver39.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver39.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver39.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider39.Add(typeFromHandle78, new XamlTypeResolver(xmlNamespaceResolver39, typeof(DashboardItemEditor).GetTypeInfo().Assembly));
			xamlServiceProvider39.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(366, 45)));
			object obj61 = markupExtension39.ProvideValue(xamlServiceProvider39);
			label9.Text = obj61;
			grid6.Children.Add(label9);
			label10.SetValue(Grid.RowProperty, 0);
			label10.SetValue(Grid.ColumnProperty, 1);
			label10.SetValue(Label.FontAttributesProperty, new FontAttributesConverter().ConvertFromInvariantString("Bold"));
			bindingExtension20.Mode = 2;
			staticResourceExtension6.Key = "ThicknessToDoubleConverter";
			IMarkupExtension markupExtension40 = staticResourceExtension6;
			XamlServiceProvider xamlServiceProvider40 = new XamlServiceProvider();
			Type typeFromHandle79 = typeof(IProvideValueTarget);
			object[] array40 = new object[0 + 10];
			array40[0] = bindingExtension20;
			array40[1] = label10;
			array40[2] = grid6;
			array40[3] = stackLayout4;
			array40[4] = frame3;
			array40[5] = sfExpander;
			array40[6] = stackLayout24;
			array40[7] = scrollView;
			array40[8] = grid25;
			array40[9] = this;
			object obj62;
			xamlServiceProvider40.Add(typeFromHandle79, obj62 = new SimpleValueTargetProvider(array40, typeof(BindingExtension).GetRuntimeProperty("Converter"), nameScope));
			xamlServiceProvider40.Add(typeof(IReferenceProvider), obj62);
			Type typeFromHandle80 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver40 = new XmlNamespaceResolver();
			xmlNamespaceResolver40.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver40.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver40.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver40.Add("dash", "clr-namespace:CarScannerXamarinForms.Dashboard");
			xmlNamespaceResolver40.Add("expander", "clr-namespace:Syncfusion.XForms.Expander;assembly=Syncfusion.Expander.XForms");
			xmlNamespaceResolver40.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver40.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver40.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver40.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider40.Add(typeFromHandle80, new XamlTypeResolver(xmlNamespaceResolver40, typeof(DashboardItemEditor).GetTypeInfo().Assembly));
			xamlServiceProvider40.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(371, 45)));
			object obj63 = markupExtension40.ProvideValue(xamlServiceProvider40);
			bindingExtension20.Converter = obj63;
			bindingExtension20.Path = "CornerRadius";
			bindingExtension20.TypedBinding = new TypedBinding<DashboardItem, Thickness>(delegate(DashboardItem A_0)
			{
				if (A_0 != null)
				{
					return new ValueTuple<Thickness, bool>(A_0.CornerRadius, true);
				}
				return default(ValueTuple<Thickness, bool>);
			}, null, new Tuple<Func<DashboardItem, object>, string>[]
			{
				new Tuple<Func<DashboardItem, object>, string>((DashboardItem A_0) => A_0, "CornerRadius")
			});
			BindingBase bindingBase20 = bindingExtension20.ProvideValue(null);
			label10.SetBinding(Label.TextProperty, bindingBase20);
			label10.SetValue(View.VerticalOptionsProperty, LayoutOptions.Center);
			grid6.Children.Add(label10);
			extendedSlider4.SetValue(Grid.RowProperty, 1);
			extendedSlider4.SetValue(Grid.ColumnProperty, 0);
			extendedSlider4.SetValue(Grid.ColumnSpanProperty, 2);
			extendedSlider4.SetValue(Slider.MaximumProperty, 360.0);
			extendedSlider4.SetValue(Slider.MinimumProperty, 0.0);
			extendedSlider4.StepValue = 1.0;
			bindingExtension21.Mode = 1;
			staticResourceExtension7.Key = "ThicknessToDoubleConverter";
			IMarkupExtension markupExtension41 = staticResourceExtension7;
			XamlServiceProvider xamlServiceProvider41 = new XamlServiceProvider();
			Type typeFromHandle81 = typeof(IProvideValueTarget);
			object[] array41 = new object[0 + 10];
			array41[0] = bindingExtension21;
			array41[1] = extendedSlider4;
			array41[2] = grid6;
			array41[3] = stackLayout4;
			array41[4] = frame3;
			array41[5] = sfExpander;
			array41[6] = stackLayout24;
			array41[7] = scrollView;
			array41[8] = grid25;
			array41[9] = this;
			object obj64;
			xamlServiceProvider41.Add(typeFromHandle81, obj64 = new SimpleValueTargetProvider(array41, typeof(BindingExtension).GetRuntimeProperty("Converter"), nameScope));
			xamlServiceProvider41.Add(typeof(IReferenceProvider), obj64);
			Type typeFromHandle82 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver41 = new XmlNamespaceResolver();
			xmlNamespaceResolver41.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver41.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver41.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver41.Add("dash", "clr-namespace:CarScannerXamarinForms.Dashboard");
			xmlNamespaceResolver41.Add("expander", "clr-namespace:Syncfusion.XForms.Expander;assembly=Syncfusion.Expander.XForms");
			xmlNamespaceResolver41.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver41.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver41.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver41.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider41.Add(typeFromHandle82, new XamlTypeResolver(xmlNamespaceResolver41, typeof(DashboardItemEditor).GetTypeInfo().Assembly));
			xamlServiceProvider41.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(380, 45)));
			object obj65 = markupExtension41.ProvideValue(xamlServiceProvider41);
			bindingExtension21.Converter = obj65;
			bindingExtension21.Path = "CornerRadius";
			bindingExtension21.TypedBinding = new TypedBinding<DashboardItem, Thickness>(delegate(DashboardItem A_0)
			{
				if (A_0 != null)
				{
					return new ValueTuple<Thickness, bool>(A_0.CornerRadius, true);
				}
				return default(ValueTuple<Thickness, bool>);
			}, delegate(DashboardItem A_0, Thickness A_1)
			{
				if (A_0 != null)
				{
					A_0.CornerRadius = A_1;
					return;
				}
			}, new Tuple<Func<DashboardItem, object>, string>[]
			{
				new Tuple<Func<DashboardItem, object>, string>((DashboardItem A_0) => A_0, "CornerRadius")
			});
			BindingBase bindingBase21 = bindingExtension21.ProvideValue(null);
			extendedSlider4.SetBinding(Slider.ValueProperty, bindingBase21);
			grid6.Children.Add(extendedSlider4);
			stackLayout4.Children.Add(grid6);
			frame3.SetValue(ContentView.ContentProperty, stackLayout4);
			sfExpander.SetValue(SfExpander.ContentProperty, frame3);
			stackLayout24.Children.Add(sfExpander);
			sfExpander2.SetValue(SfExpander.AnimationDurationProperty, 250.0);
			sfExpander2.SetValue(SfExpander.AnimationEasingProperty, 3);
			dynamicResourceExtension16.Key = "NavigationBarBackgroundColor";
			IMarkupExtension<DynamicResource> markupExtension42 = dynamicResourceExtension16;
			XamlServiceProvider xamlServiceProvider42 = new XamlServiceProvider();
			Type typeFromHandle83 = typeof(IProvideValueTarget);
			object[] array42 = new object[0 + 5];
			array42[0] = sfExpander2;
			array42[1] = stackLayout24;
			array42[2] = scrollView;
			array42[3] = grid25;
			array42[4] = this;
			object obj66;
			xamlServiceProvider42.Add(typeFromHandle83, obj66 = new SimpleValueTargetProvider(array42, SfExpander.HeaderBackgroundColorProperty, nameScope));
			xamlServiceProvider42.Add(typeof(IReferenceProvider), obj66);
			Type typeFromHandle84 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver42 = new XmlNamespaceResolver();
			xmlNamespaceResolver42.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver42.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver42.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver42.Add("dash", "clr-namespace:CarScannerXamarinForms.Dashboard");
			xmlNamespaceResolver42.Add("expander", "clr-namespace:Syncfusion.XForms.Expander;assembly=Syncfusion.Expander.XForms");
			xmlNamespaceResolver42.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver42.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver42.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver42.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider42.Add(typeFromHandle84, new XamlTypeResolver(xmlNamespaceResolver42, typeof(DashboardItemEditor).GetTypeInfo().Assembly));
			xamlServiceProvider42.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(397, 25)));
			DynamicResource dynamicResource16 = markupExtension42.ProvideValue(xamlServiceProvider42);
			sfExpander2.SetDynamicResource(SfExpander.HeaderBackgroundColorProperty, dynamicResource16.Key);
			dynamicResourceExtension17.Key = "NavigationBarButtonColor";
			IMarkupExtension<DynamicResource> markupExtension43 = dynamicResourceExtension17;
			XamlServiceProvider xamlServiceProvider43 = new XamlServiceProvider();
			Type typeFromHandle85 = typeof(IProvideValueTarget);
			object[] array43 = new object[0 + 5];
			array43[0] = sfExpander2;
			array43[1] = stackLayout24;
			array43[2] = scrollView;
			array43[3] = grid25;
			array43[4] = this;
			object obj67;
			xamlServiceProvider43.Add(typeFromHandle85, obj67 = new SimpleValueTargetProvider(array43, SfExpander.IconColorProperty, nameScope));
			xamlServiceProvider43.Add(typeof(IReferenceProvider), obj67);
			Type typeFromHandle86 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver43 = new XmlNamespaceResolver();
			xmlNamespaceResolver43.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver43.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver43.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver43.Add("dash", "clr-namespace:CarScannerXamarinForms.Dashboard");
			xmlNamespaceResolver43.Add("expander", "clr-namespace:Syncfusion.XForms.Expander;assembly=Syncfusion.Expander.XForms");
			xmlNamespaceResolver43.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver43.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver43.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver43.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider43.Add(typeFromHandle86, new XamlTypeResolver(xmlNamespaceResolver43, typeof(DashboardItemEditor).GetTypeInfo().Assembly));
			xamlServiceProvider43.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(398, 25)));
			DynamicResource dynamicResource17 = markupExtension43.ProvideValue(xamlServiceProvider43);
			sfExpander2.SetDynamicResource(SfExpander.IconColorProperty, dynamicResource17.Key);
			sfExpander2.SetValue(SfExpander.IsExpandedProperty, false);
			label11.SetValue(View.MarginProperty, new Thickness(5.0));
			label11.SetValue(Label.FontAttributesProperty, new FontAttributesConverter().ConvertFromInvariantString("Bold"));
			translate18.Text = "DashItem_TextSizeAndColor";
			IMarkupExtension markupExtension44 = translate18;
			XamlServiceProvider xamlServiceProvider44 = new XamlServiceProvider();
			Type typeFromHandle87 = typeof(IProvideValueTarget);
			object[] array44 = new object[0 + 7];
			array44[0] = label11;
			array44[1] = grid7;
			array44[2] = sfExpander2;
			array44[3] = stackLayout24;
			array44[4] = scrollView;
			array44[5] = grid25;
			array44[6] = this;
			object obj68;
			xamlServiceProvider44.Add(typeFromHandle87, obj68 = new SimpleValueTargetProvider(array44, Label.TextProperty, nameScope));
			xamlServiceProvider44.Add(typeof(IReferenceProvider), obj68);
			Type typeFromHandle88 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver44 = new XmlNamespaceResolver();
			xmlNamespaceResolver44.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver44.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver44.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver44.Add("dash", "clr-namespace:CarScannerXamarinForms.Dashboard");
			xmlNamespaceResolver44.Add("expander", "clr-namespace:Syncfusion.XForms.Expander;assembly=Syncfusion.Expander.XForms");
			xmlNamespaceResolver44.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver44.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver44.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver44.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider44.Add(typeFromHandle88, new XamlTypeResolver(xmlNamespaceResolver44, typeof(DashboardItemEditor).GetTypeInfo().Assembly));
			xamlServiceProvider44.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(405, 37)));
			object obj69 = markupExtension44.ProvideValue(xamlServiceProvider44);
			label11.Text = obj69;
			dynamicResourceExtension18.Key = "NavigationBarTextColor";
			IMarkupExtension<DynamicResource> markupExtension45 = dynamicResourceExtension18;
			XamlServiceProvider xamlServiceProvider45 = new XamlServiceProvider();
			Type typeFromHandle89 = typeof(IProvideValueTarget);
			object[] array45 = new object[0 + 7];
			array45[0] = label11;
			array45[1] = grid7;
			array45[2] = sfExpander2;
			array45[3] = stackLayout24;
			array45[4] = scrollView;
			array45[5] = grid25;
			array45[6] = this;
			object obj70;
			xamlServiceProvider45.Add(typeFromHandle89, obj70 = new SimpleValueTargetProvider(array45, Label.TextColorProperty, nameScope));
			xamlServiceProvider45.Add(typeof(IReferenceProvider), obj70);
			Type typeFromHandle90 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver45 = new XmlNamespaceResolver();
			xmlNamespaceResolver45.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver45.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver45.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver45.Add("dash", "clr-namespace:CarScannerXamarinForms.Dashboard");
			xmlNamespaceResolver45.Add("expander", "clr-namespace:Syncfusion.XForms.Expander;assembly=Syncfusion.Expander.XForms");
			xmlNamespaceResolver45.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver45.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver45.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver45.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider45.Add(typeFromHandle90, new XamlTypeResolver(xmlNamespaceResolver45, typeof(DashboardItemEditor).GetTypeInfo().Assembly));
			xamlServiceProvider45.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(406, 37)));
			DynamicResource dynamicResource18 = markupExtension45.ProvideValue(xamlServiceProvider45);
			label11.SetDynamicResource(Label.TextColorProperty, dynamicResource18.Key);
			label11.SetValue(View.VerticalOptionsProperty, LayoutOptions.Center);
			label11.SetValue(Label.VerticalTextAlignmentProperty, new TextAlignmentConverter().ConvertFromInvariantString("Center"));
			grid7.Children.Add(label11);
			sfExpander2.SetValue(SfExpander.HeaderProperty, grid7);
			stackLayout5.SetValue(StackLayout.OrientationProperty, 0);
			dashboardItemColorBox5.Tapped += this.TitleColor_Tapped;
			translate19.Text = "ios_DashboardItemEditor_TitleTextColor";
			IMarkupExtension markupExtension46 = translate19;
			XamlServiceProvider xamlServiceProvider46 = new XamlServiceProvider();
			Type typeFromHandle91 = typeof(IProvideValueTarget);
			object[] array46 = new object[0 + 8];
			array46[0] = dashboardItemColorBox5;
			array46[1] = stackLayout5;
			array46[2] = frame4;
			array46[3] = sfExpander2;
			array46[4] = stackLayout24;
			array46[5] = scrollView;
			array46[6] = grid25;
			array46[7] = this;
			object obj71;
			xamlServiceProvider46.Add(typeFromHandle91, obj71 = new SimpleValueTargetProvider(array46, DashboardItemColorBox.TextProperty, nameScope));
			xamlServiceProvider46.Add(typeof(IReferenceProvider), obj71);
			Type typeFromHandle92 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver46 = new XmlNamespaceResolver();
			xmlNamespaceResolver46.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver46.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver46.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver46.Add("dash", "clr-namespace:CarScannerXamarinForms.Dashboard");
			xmlNamespaceResolver46.Add("expander", "clr-namespace:Syncfusion.XForms.Expander;assembly=Syncfusion.Expander.XForms");
			xmlNamespaceResolver46.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver46.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver46.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver46.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider46.Add(typeFromHandle92, new XamlTypeResolver(xmlNamespaceResolver46, typeof(DashboardItemEditor).GetTypeInfo().Assembly));
			xamlServiceProvider46.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(419, 41)));
			object obj72 = markupExtension46.ProvideValue(xamlServiceProvider46);
			dashboardItemColorBox5.Text = obj72;
			bindingExtension22.Mode = 2;
			bindingExtension22.Path = "TitleTextColor";
			bindingExtension22.TypedBinding = new TypedBinding<DashboardItem, Color>(delegate(DashboardItem A_0)
			{
				if (A_0 != null)
				{
					return new ValueTuple<Color, bool>(A_0.TitleTextColor, true);
				}
				return default(ValueTuple<Color, bool>);
			}, null, new Tuple<Func<DashboardItem, object>, string>[]
			{
				new Tuple<Func<DashboardItem, object>, string>((DashboardItem A_0) => A_0, "TitleTextColor")
			});
			BindingBase bindingBase22 = bindingExtension22.ProvideValue(null);
			dashboardItemColorBox5.SetBinding(DashboardItemColorBox.ColorProperty, bindingBase22);
			stackLayout5.Children.Add(dashboardItemColorBox5);
			grid8.SetValue(View.MarginProperty, new Thickness(0.0, 0.0, 0.0, 10.0));
			grid8.SetValue(Grid.RowSpacingProperty, 0.0);
			columnDefinition13.SetValue(ColumnDefinition.WidthProperty, new GridLengthTypeConverter().ConvertFromInvariantString("*"));
			grid8.GetValue(Grid.ColumnDefinitionsProperty).Add(columnDefinition13);
			columnDefinition14.SetValue(ColumnDefinition.WidthProperty, new GridLengthTypeConverter().ConvertFromInvariantString("auto"));
			grid8.GetValue(Grid.ColumnDefinitionsProperty).Add(columnDefinition14);
			rowDefinition9.SetValue(RowDefinition.HeightProperty, new GridLengthTypeConverter().ConvertFromInvariantString("auto"));
			grid8.GetValue(Grid.RowDefinitionsProperty).Add(rowDefinition9);
			rowDefinition10.SetValue(RowDefinition.HeightProperty, new GridLengthTypeConverter().ConvertFromInvariantString("auto"));
			grid8.GetValue(Grid.RowDefinitionsProperty).Add(rowDefinition10);
			label12.SetValue(Grid.RowProperty, 0);
			label12.SetValue(Grid.ColumnProperty, 0);
			translate20.Text = "ios_DashboardItemEditor_TitleTextSize";
			IMarkupExtension markupExtension47 = translate20;
			XamlServiceProvider xamlServiceProvider47 = new XamlServiceProvider();
			Type typeFromHandle93 = typeof(IProvideValueTarget);
			object[] array47 = new object[0 + 9];
			array47[0] = label12;
			array47[1] = grid8;
			array47[2] = stackLayout5;
			array47[3] = frame4;
			array47[4] = sfExpander2;
			array47[5] = stackLayout24;
			array47[6] = scrollView;
			array47[7] = grid25;
			array47[8] = this;
			object obj73;
			xamlServiceProvider47.Add(typeFromHandle93, obj73 = new SimpleValueTargetProvider(array47, Label.TextProperty, nameScope));
			xamlServiceProvider47.Add(typeof(IReferenceProvider), obj73);
			Type typeFromHandle94 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver47 = new XmlNamespaceResolver();
			xmlNamespaceResolver47.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver47.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver47.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver47.Add("dash", "clr-namespace:CarScannerXamarinForms.Dashboard");
			xmlNamespaceResolver47.Add("expander", "clr-namespace:Syncfusion.XForms.Expander;assembly=Syncfusion.Expander.XForms");
			xmlNamespaceResolver47.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver47.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver47.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver47.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider47.Add(typeFromHandle94, new XamlTypeResolver(xmlNamespaceResolver47, typeof(DashboardItemEditor).GetTypeInfo().Assembly));
			xamlServiceProvider47.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(435, 45)));
			object obj74 = markupExtension47.ProvideValue(xamlServiceProvider47);
			label12.Text = obj74;
			grid8.Children.Add(label12);
			label13.SetValue(Grid.RowProperty, 0);
			label13.SetValue(Grid.ColumnProperty, 1);
			label13.SetValue(Label.FontAttributesProperty, new FontAttributesConverter().ConvertFromInvariantString("Bold"));
			bindingExtension23.Mode = 2;
			bindingExtension23.Path = "TitleFontSize";
			bindingExtension23.TypedBinding = new TypedBinding<DashboardItem, double>(delegate(DashboardItem A_0)
			{
				if (A_0 != null)
				{
					return new ValueTuple<double, bool>(A_0.TitleFontSize, true);
				}
				return default(ValueTuple<double, bool>);
			}, null, new Tuple<Func<DashboardItem, object>, string>[]
			{
				new Tuple<Func<DashboardItem, object>, string>((DashboardItem A_0) => A_0, "TitleFontSize")
			});
			BindingBase bindingBase23 = bindingExtension23.ProvideValue(null);
			label13.SetBinding(Label.TextProperty, bindingBase23);
			label13.SetValue(View.VerticalOptionsProperty, LayoutOptions.Center);
			grid8.Children.Add(label13);
			extendedSlider5.SetValue(Grid.RowProperty, 1);
			extendedSlider5.SetValue(Grid.ColumnProperty, 0);
			extendedSlider5.SetValue(Grid.ColumnSpanProperty, 2);
			extendedSlider5.SetValue(Slider.MaximumProperty, 200.0);
			extendedSlider5.SetValue(Slider.MinimumProperty, 0.0);
			extendedSlider5.StepValue = 4.0;
			bindingExtension24.Mode = 1;
			bindingExtension24.Path = "TitleFontSize";
			bindingExtension24.TypedBinding = new TypedBinding<DashboardItem, double>(delegate(DashboardItem A_0)
			{
				if (A_0 != null)
				{
					return new ValueTuple<double, bool>(A_0.TitleFontSize, true);
				}
				return default(ValueTuple<double, bool>);
			}, delegate(DashboardItem A_0, double A_1)
			{
				if (A_0 != null)
				{
					A_0.TitleFontSize = A_1;
					return;
				}
			}, new Tuple<Func<DashboardItem, object>, string>[]
			{
				new Tuple<Func<DashboardItem, object>, string>((DashboardItem A_0) => A_0, "TitleFontSize")
			});
			BindingBase bindingBase24 = bindingExtension24.ProvideValue(null);
			extendedSlider5.SetBinding(Slider.ValueProperty, bindingBase24);
			grid8.Children.Add(extendedSlider5);
			stackLayout5.Children.Add(grid8);
			dashboardItemColorBox6.Tapped += this.ValueNormalTextColor_Tapped;
			translate21.Text = "ios_DashboardItemEditor_ValueTextColor";
			IMarkupExtension markupExtension48 = translate21;
			XamlServiceProvider xamlServiceProvider48 = new XamlServiceProvider();
			Type typeFromHandle95 = typeof(IProvideValueTarget);
			object[] array48 = new object[0 + 8];
			array48[0] = dashboardItemColorBox6;
			array48[1] = stackLayout5;
			array48[2] = frame4;
			array48[3] = sfExpander2;
			array48[4] = stackLayout24;
			array48[5] = scrollView;
			array48[6] = grid25;
			array48[7] = this;
			object obj75;
			xamlServiceProvider48.Add(typeFromHandle95, obj75 = new SimpleValueTargetProvider(array48, DashboardItemColorBox.TextProperty, nameScope));
			xamlServiceProvider48.Add(typeof(IReferenceProvider), obj75);
			Type typeFromHandle96 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver48 = new XmlNamespaceResolver();
			xmlNamespaceResolver48.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver48.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver48.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver48.Add("dash", "clr-namespace:CarScannerXamarinForms.Dashboard");
			xmlNamespaceResolver48.Add("expander", "clr-namespace:Syncfusion.XForms.Expander;assembly=Syncfusion.Expander.XForms");
			xmlNamespaceResolver48.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver48.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver48.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver48.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider48.Add(typeFromHandle96, new XamlTypeResolver(xmlNamespaceResolver48, typeof(DashboardItemEditor).GetTypeInfo().Assembly));
			xamlServiceProvider48.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(455, 41)));
			object obj76 = markupExtension48.ProvideValue(xamlServiceProvider48);
			dashboardItemColorBox6.Text = obj76;
			bindingExtension25.Mode = 2;
			bindingExtension25.Path = "ValueNormalTextColor";
			bindingExtension25.TypedBinding = new TypedBinding<DashboardItem, Color>(delegate(DashboardItem A_0)
			{
				if (A_0 != null)
				{
					return new ValueTuple<Color, bool>(A_0.ValueNormalTextColor, true);
				}
				return default(ValueTuple<Color, bool>);
			}, null, new Tuple<Func<DashboardItem, object>, string>[]
			{
				new Tuple<Func<DashboardItem, object>, string>((DashboardItem A_0) => A_0, "ValueNormalTextColor")
			});
			BindingBase bindingBase25 = bindingExtension25.ProvideValue(null);
			dashboardItemColorBox6.SetBinding(DashboardItemColorBox.ColorProperty, bindingBase25);
			stackLayout5.Children.Add(dashboardItemColorBox6);
			grid9.SetValue(View.MarginProperty, new Thickness(0.0, 0.0, 0.0, 10.0));
			grid9.SetValue(Grid.RowSpacingProperty, 0.0);
			columnDefinition15.SetValue(ColumnDefinition.WidthProperty, new GridLengthTypeConverter().ConvertFromInvariantString("*"));
			grid9.GetValue(Grid.ColumnDefinitionsProperty).Add(columnDefinition15);
			columnDefinition16.SetValue(ColumnDefinition.WidthProperty, new GridLengthTypeConverter().ConvertFromInvariantString("auto"));
			grid9.GetValue(Grid.ColumnDefinitionsProperty).Add(columnDefinition16);
			rowDefinition11.SetValue(RowDefinition.HeightProperty, new GridLengthTypeConverter().ConvertFromInvariantString("auto"));
			grid9.GetValue(Grid.RowDefinitionsProperty).Add(rowDefinition11);
			rowDefinition12.SetValue(RowDefinition.HeightProperty, new GridLengthTypeConverter().ConvertFromInvariantString("auto"));
			grid9.GetValue(Grid.RowDefinitionsProperty).Add(rowDefinition12);
			label14.SetValue(Grid.RowProperty, 0);
			label14.SetValue(Grid.ColumnProperty, 0);
			translate22.Text = "ios_DashboardItemEditor_ValueTextSize";
			IMarkupExtension markupExtension49 = translate22;
			XamlServiceProvider xamlServiceProvider49 = new XamlServiceProvider();
			Type typeFromHandle97 = typeof(IProvideValueTarget);
			object[] array49 = new object[0 + 9];
			array49[0] = label14;
			array49[1] = grid9;
			array49[2] = stackLayout5;
			array49[3] = frame4;
			array49[4] = sfExpander2;
			array49[5] = stackLayout24;
			array49[6] = scrollView;
			array49[7] = grid25;
			array49[8] = this;
			object obj77;
			xamlServiceProvider49.Add(typeFromHandle97, obj77 = new SimpleValueTargetProvider(array49, Label.TextProperty, nameScope));
			xamlServiceProvider49.Add(typeof(IReferenceProvider), obj77);
			Type typeFromHandle98 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver49 = new XmlNamespaceResolver();
			xmlNamespaceResolver49.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver49.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver49.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver49.Add("dash", "clr-namespace:CarScannerXamarinForms.Dashboard");
			xmlNamespaceResolver49.Add("expander", "clr-namespace:Syncfusion.XForms.Expander;assembly=Syncfusion.Expander.XForms");
			xmlNamespaceResolver49.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver49.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver49.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver49.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider49.Add(typeFromHandle98, new XamlTypeResolver(xmlNamespaceResolver49, typeof(DashboardItemEditor).GetTypeInfo().Assembly));
			xamlServiceProvider49.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(470, 45)));
			object obj78 = markupExtension49.ProvideValue(xamlServiceProvider49);
			label14.Text = obj78;
			grid9.Children.Add(label14);
			label15.SetValue(Grid.RowProperty, 0);
			label15.SetValue(Grid.ColumnProperty, 1);
			label15.SetValue(Label.FontAttributesProperty, new FontAttributesConverter().ConvertFromInvariantString("Bold"));
			bindingExtension26.Mode = 2;
			bindingExtension26.Path = "ValueFontSize";
			bindingExtension26.TypedBinding = new TypedBinding<DashboardItem, double>(delegate(DashboardItem A_0)
			{
				if (A_0 != null)
				{
					return new ValueTuple<double, bool>(A_0.ValueFontSize, true);
				}
				return default(ValueTuple<double, bool>);
			}, null, new Tuple<Func<DashboardItem, object>, string>[]
			{
				new Tuple<Func<DashboardItem, object>, string>((DashboardItem A_0) => A_0, "ValueFontSize")
			});
			BindingBase bindingBase26 = bindingExtension26.ProvideValue(null);
			label15.SetBinding(Label.TextProperty, bindingBase26);
			label15.SetValue(View.VerticalOptionsProperty, LayoutOptions.Center);
			grid9.Children.Add(label15);
			extendedSlider6.SetValue(Grid.RowProperty, 1);
			extendedSlider6.SetValue(Grid.ColumnProperty, 0);
			extendedSlider6.SetValue(Grid.ColumnSpanProperty, 2);
			extendedSlider6.SetValue(Slider.MaximumProperty, 200.0);
			extendedSlider6.SetValue(Slider.MinimumProperty, 0.0);
			extendedSlider6.StepValue = 4.0;
			bindingExtension27.Mode = 1;
			bindingExtension27.Path = "ValueFontSize";
			bindingExtension27.TypedBinding = new TypedBinding<DashboardItem, double>(delegate(DashboardItem A_0)
			{
				if (A_0 != null)
				{
					return new ValueTuple<double, bool>(A_0.ValueFontSize, true);
				}
				return default(ValueTuple<double, bool>);
			}, delegate(DashboardItem A_0, double A_1)
			{
				if (A_0 != null)
				{
					A_0.ValueFontSize = A_1;
					return;
				}
			}, new Tuple<Func<DashboardItem, object>, string>[]
			{
				new Tuple<Func<DashboardItem, object>, string>((DashboardItem A_0) => A_0, "ValueFontSize")
			});
			BindingBase bindingBase27 = bindingExtension27.ProvideValue(null);
			extendedSlider6.SetBinding(Slider.ValueProperty, bindingBase27);
			grid9.Children.Add(extendedSlider6);
			stackLayout5.Children.Add(grid9);
			bindingExtension28.Mode = 1;
			bindingExtension28.Path = "ValueUseLCDFont";
			bindingExtension28.TypedBinding = new TypedBinding<DashboardItem, bool>(delegate(DashboardItem A_0)
			{
				if (A_0 != null)
				{
					return new ValueTuple<bool, bool>(A_0.ValueUseLCDFont, true);
				}
				return default(ValueTuple<bool, bool>);
			}, delegate(DashboardItem A_0, bool A_1)
			{
				if (A_0 != null)
				{
					A_0.ValueUseLCDFont = A_1;
					return;
				}
			}, new Tuple<Func<DashboardItem, object>, string>[]
			{
				new Tuple<Func<DashboardItem, object>, string>((DashboardItem A_0) => A_0, "ValueUseLCDFont")
			});
			BindingBase bindingBase28 = bindingExtension28.ProvideValue(null);
			labelSwitch3.SetBinding(LabelSwitch.IsToggledProperty, bindingBase28);
			translate23.Text = "ValueUseLCDFont";
			IMarkupExtension markupExtension50 = translate23;
			XamlServiceProvider xamlServiceProvider50 = new XamlServiceProvider();
			Type typeFromHandle99 = typeof(IProvideValueTarget);
			object[] array50 = new object[0 + 8];
			array50[0] = labelSwitch3;
			array50[1] = stackLayout5;
			array50[2] = frame4;
			array50[3] = sfExpander2;
			array50[4] = stackLayout24;
			array50[5] = scrollView;
			array50[6] = grid25;
			array50[7] = this;
			object obj79;
			xamlServiceProvider50.Add(typeFromHandle99, obj79 = new SimpleValueTargetProvider(array50, LabelSwitch.TextProperty, nameScope));
			xamlServiceProvider50.Add(typeof(IReferenceProvider), obj79);
			Type typeFromHandle100 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver50 = new XmlNamespaceResolver();
			xmlNamespaceResolver50.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver50.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver50.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver50.Add("dash", "clr-namespace:CarScannerXamarinForms.Dashboard");
			xmlNamespaceResolver50.Add("expander", "clr-namespace:Syncfusion.XForms.Expander;assembly=Syncfusion.Expander.XForms");
			xmlNamespaceResolver50.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver50.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver50.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver50.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider50.Add(typeFromHandle100, new XamlTypeResolver(xmlNamespaceResolver50, typeof(DashboardItemEditor).GetTypeInfo().Assembly));
			xamlServiceProvider50.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(487, 107)));
			object obj80 = markupExtension50.ProvideValue(xamlServiceProvider50);
			labelSwitch3.Text = obj80;
			stackLayout5.Children.Add(labelSwitch3);
			translate24.Text = "DashboardEditor_ValueFormat";
			IMarkupExtension markupExtension51 = translate24;
			XamlServiceProvider xamlServiceProvider51 = new XamlServiceProvider();
			Type typeFromHandle101 = typeof(IProvideValueTarget);
			object[] array51 = new object[0 + 8];
			array51[0] = label16;
			array51[1] = stackLayout5;
			array51[2] = frame4;
			array51[3] = sfExpander2;
			array51[4] = stackLayout24;
			array51[5] = scrollView;
			array51[6] = grid25;
			array51[7] = this;
			object obj81;
			xamlServiceProvider51.Add(typeFromHandle101, obj81 = new SimpleValueTargetProvider(array51, Label.TextProperty, nameScope));
			xamlServiceProvider51.Add(typeof(IReferenceProvider), obj81);
			Type typeFromHandle102 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver51 = new XmlNamespaceResolver();
			xmlNamespaceResolver51.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver51.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver51.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver51.Add("dash", "clr-namespace:CarScannerXamarinForms.Dashboard");
			xmlNamespaceResolver51.Add("expander", "clr-namespace:Syncfusion.XForms.Expander;assembly=Syncfusion.Expander.XForms");
			xmlNamespaceResolver51.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver51.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver51.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver51.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider51.Add(typeFromHandle102, new XamlTypeResolver(xmlNamespaceResolver51, typeof(DashboardItemEditor).GetTypeInfo().Assembly));
			xamlServiceProvider51.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(490, 44)));
			object obj82 = markupExtension51.ProvideValue(xamlServiceProvider51);
			label16.Text = obj82;
			stackLayout5.Children.Add(label16);
			bindingExtension29.Source = doubleFormats;
			BindingBase bindingBase29 = bindingExtension29.ProvideValue(null);
			picker2.SetBinding(Picker.ItemsSourceProperty, bindingBase29);
			bindingExtension30.Mode = 1;
			bindingExtension30.Path = "ValueFormat";
			bindingExtension30.TypedBinding = new TypedBinding<DashboardItem, int>(delegate(DashboardItem A_0)
			{
				if (A_0 != null)
				{
					return new ValueTuple<int, bool>(A_0.ValueFormat, true);
				}
				return default(ValueTuple<int, bool>);
			}, delegate(DashboardItem A_0, int A_1)
			{
				if (A_0 != null)
				{
					A_0.ValueFormat = A_1;
					return;
				}
			}, new Tuple<Func<DashboardItem, object>, string>[]
			{
				new Tuple<Func<DashboardItem, object>, string>((DashboardItem A_0) => A_0, "ValueFormat")
			});
			BindingBase bindingBase30 = bindingExtension30.ProvideValue(null);
			picker2.SetBinding(Picker.SelectedIndexProperty, bindingBase30);
			stackLayout5.Children.Add(picker2);
			dashboardItemColorBox7.Tapped += this.UnitsTextColor_Tapped;
			translate25.Text = "ios_DashboardItemEditor_UnitsTextColor";
			IMarkupExtension markupExtension52 = translate25;
			XamlServiceProvider xamlServiceProvider52 = new XamlServiceProvider();
			Type typeFromHandle103 = typeof(IProvideValueTarget);
			object[] array52 = new object[0 + 8];
			array52[0] = dashboardItemColorBox7;
			array52[1] = stackLayout5;
			array52[2] = frame4;
			array52[3] = sfExpander2;
			array52[4] = stackLayout24;
			array52[5] = scrollView;
			array52[6] = grid25;
			array52[7] = this;
			object obj83;
			xamlServiceProvider52.Add(typeFromHandle103, obj83 = new SimpleValueTargetProvider(array52, DashboardItemColorBox.TextProperty, nameScope));
			xamlServiceProvider52.Add(typeof(IReferenceProvider), obj83);
			Type typeFromHandle104 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver52 = new XmlNamespaceResolver();
			xmlNamespaceResolver52.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver52.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver52.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver52.Add("dash", "clr-namespace:CarScannerXamarinForms.Dashboard");
			xmlNamespaceResolver52.Add("expander", "clr-namespace:Syncfusion.XForms.Expander;assembly=Syncfusion.Expander.XForms");
			xmlNamespaceResolver52.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver52.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver52.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver52.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider52.Add(typeFromHandle104, new XamlTypeResolver(xmlNamespaceResolver52, typeof(DashboardItemEditor).GetTypeInfo().Assembly));
			xamlServiceProvider52.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(496, 41)));
			object obj84 = markupExtension52.ProvideValue(xamlServiceProvider52);
			dashboardItemColorBox7.Text = obj84;
			bindingExtension31.Mode = 2;
			bindingExtension31.Path = "UnitsTextColor";
			bindingExtension31.TypedBinding = new TypedBinding<DashboardItem, Color>(delegate(DashboardItem A_0)
			{
				if (A_0 != null)
				{
					return new ValueTuple<Color, bool>(A_0.UnitsTextColor, true);
				}
				return default(ValueTuple<Color, bool>);
			}, null, new Tuple<Func<DashboardItem, object>, string>[]
			{
				new Tuple<Func<DashboardItem, object>, string>((DashboardItem A_0) => A_0, "UnitsTextColor")
			});
			BindingBase bindingBase31 = bindingExtension31.ProvideValue(null);
			dashboardItemColorBox7.SetBinding(DashboardItemColorBox.ColorProperty, bindingBase31);
			stackLayout5.Children.Add(dashboardItemColorBox7);
			grid10.SetValue(View.MarginProperty, new Thickness(0.0, 0.0, 0.0, 10.0));
			grid10.SetValue(Grid.RowSpacingProperty, 0.0);
			columnDefinition17.SetValue(ColumnDefinition.WidthProperty, new GridLengthTypeConverter().ConvertFromInvariantString("*"));
			grid10.GetValue(Grid.ColumnDefinitionsProperty).Add(columnDefinition17);
			columnDefinition18.SetValue(ColumnDefinition.WidthProperty, new GridLengthTypeConverter().ConvertFromInvariantString("auto"));
			grid10.GetValue(Grid.ColumnDefinitionsProperty).Add(columnDefinition18);
			rowDefinition13.SetValue(RowDefinition.HeightProperty, new GridLengthTypeConverter().ConvertFromInvariantString("auto"));
			grid10.GetValue(Grid.RowDefinitionsProperty).Add(rowDefinition13);
			rowDefinition14.SetValue(RowDefinition.HeightProperty, new GridLengthTypeConverter().ConvertFromInvariantString("auto"));
			grid10.GetValue(Grid.RowDefinitionsProperty).Add(rowDefinition14);
			label17.SetValue(Grid.RowProperty, 0);
			label17.SetValue(Grid.ColumnProperty, 0);
			translate26.Text = "ios_DashboardItemEditor_UnitsTextSize";
			IMarkupExtension markupExtension53 = translate26;
			XamlServiceProvider xamlServiceProvider53 = new XamlServiceProvider();
			Type typeFromHandle105 = typeof(IProvideValueTarget);
			object[] array53 = new object[0 + 9];
			array53[0] = label17;
			array53[1] = grid10;
			array53[2] = stackLayout5;
			array53[3] = frame4;
			array53[4] = sfExpander2;
			array53[5] = stackLayout24;
			array53[6] = scrollView;
			array53[7] = grid25;
			array53[8] = this;
			object obj85;
			xamlServiceProvider53.Add(typeFromHandle105, obj85 = new SimpleValueTargetProvider(array53, Label.TextProperty, nameScope));
			xamlServiceProvider53.Add(typeof(IReferenceProvider), obj85);
			Type typeFromHandle106 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver53 = new XmlNamespaceResolver();
			xmlNamespaceResolver53.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver53.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver53.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver53.Add("dash", "clr-namespace:CarScannerXamarinForms.Dashboard");
			xmlNamespaceResolver53.Add("expander", "clr-namespace:Syncfusion.XForms.Expander;assembly=Syncfusion.Expander.XForms");
			xmlNamespaceResolver53.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver53.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver53.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver53.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider53.Add(typeFromHandle106, new XamlTypeResolver(xmlNamespaceResolver53, typeof(DashboardItemEditor).GetTypeInfo().Assembly));
			xamlServiceProvider53.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(512, 45)));
			object obj86 = markupExtension53.ProvideValue(xamlServiceProvider53);
			label17.Text = obj86;
			grid10.Children.Add(label17);
			label18.SetValue(Grid.RowProperty, 0);
			label18.SetValue(Grid.ColumnProperty, 1);
			label18.SetValue(Label.FontAttributesProperty, new FontAttributesConverter().ConvertFromInvariantString("Bold"));
			bindingExtension32.Mode = 2;
			bindingExtension32.Path = "UnitsFontSize";
			bindingExtension32.TypedBinding = new TypedBinding<DashboardItem, double>(delegate(DashboardItem A_0)
			{
				if (A_0 != null)
				{
					return new ValueTuple<double, bool>(A_0.UnitsFontSize, true);
				}
				return default(ValueTuple<double, bool>);
			}, null, new Tuple<Func<DashboardItem, object>, string>[]
			{
				new Tuple<Func<DashboardItem, object>, string>((DashboardItem A_0) => A_0, "UnitsFontSize")
			});
			BindingBase bindingBase32 = bindingExtension32.ProvideValue(null);
			label18.SetBinding(Label.TextProperty, bindingBase32);
			label18.SetValue(View.VerticalOptionsProperty, LayoutOptions.Center);
			grid10.Children.Add(label18);
			extendedSlider7.SetValue(Grid.RowProperty, 1);
			extendedSlider7.SetValue(Grid.ColumnProperty, 0);
			extendedSlider7.SetValue(Grid.ColumnSpanProperty, 2);
			extendedSlider7.SetValue(Slider.MaximumProperty, 200.0);
			extendedSlider7.SetValue(Slider.MinimumProperty, 0.0);
			extendedSlider7.StepValue = 4.0;
			bindingExtension33.Mode = 1;
			bindingExtension33.Path = "UnitsFontSize";
			bindingExtension33.TypedBinding = new TypedBinding<DashboardItem, double>(delegate(DashboardItem A_0)
			{
				if (A_0 != null)
				{
					return new ValueTuple<double, bool>(A_0.UnitsFontSize, true);
				}
				return default(ValueTuple<double, bool>);
			}, delegate(DashboardItem A_0, double A_1)
			{
				if (A_0 != null)
				{
					A_0.UnitsFontSize = A_1;
					return;
				}
			}, new Tuple<Func<DashboardItem, object>, string>[]
			{
				new Tuple<Func<DashboardItem, object>, string>((DashboardItem A_0) => A_0, "UnitsFontSize")
			});
			BindingBase bindingBase33 = bindingExtension33.ProvideValue(null);
			extendedSlider7.SetBinding(Slider.ValueProperty, bindingBase33);
			grid10.Children.Add(extendedSlider7);
			stackLayout5.Children.Add(grid10);
			frame4.SetValue(ContentView.ContentProperty, stackLayout5);
			sfExpander2.SetValue(SfExpander.ContentProperty, frame4);
			stackLayout24.Children.Add(sfExpander2);
			sfExpander3.SetValue(SfExpander.AnimationDurationProperty, 250.0);
			sfExpander3.SetValue(SfExpander.AnimationEasingProperty, 3);
			dynamicResourceExtension19.Key = "NavigationBarBackgroundColor";
			IMarkupExtension<DynamicResource> markupExtension54 = dynamicResourceExtension19;
			XamlServiceProvider xamlServiceProvider54 = new XamlServiceProvider();
			Type typeFromHandle107 = typeof(IProvideValueTarget);
			object[] array54 = new object[0 + 5];
			array54[0] = sfExpander3;
			array54[1] = stackLayout24;
			array54[2] = scrollView;
			array54[3] = grid25;
			array54[4] = this;
			object obj87;
			xamlServiceProvider54.Add(typeFromHandle107, obj87 = new SimpleValueTargetProvider(array54, SfExpander.HeaderBackgroundColorProperty, nameScope));
			xamlServiceProvider54.Add(typeof(IReferenceProvider), obj87);
			Type typeFromHandle108 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver54 = new XmlNamespaceResolver();
			xmlNamespaceResolver54.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver54.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver54.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver54.Add("dash", "clr-namespace:CarScannerXamarinForms.Dashboard");
			xmlNamespaceResolver54.Add("expander", "clr-namespace:Syncfusion.XForms.Expander;assembly=Syncfusion.Expander.XForms");
			xmlNamespaceResolver54.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver54.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver54.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver54.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider54.Add(typeFromHandle108, new XamlTypeResolver(xmlNamespaceResolver54, typeof(DashboardItemEditor).GetTypeInfo().Assembly));
			xamlServiceProvider54.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(542, 25)));
			DynamicResource dynamicResource19 = markupExtension54.ProvideValue(xamlServiceProvider54);
			sfExpander3.SetDynamicResource(SfExpander.HeaderBackgroundColorProperty, dynamicResource19.Key);
			dynamicResourceExtension20.Key = "NavigationBarButtonColor";
			IMarkupExtension<DynamicResource> markupExtension55 = dynamicResourceExtension20;
			XamlServiceProvider xamlServiceProvider55 = new XamlServiceProvider();
			Type typeFromHandle109 = typeof(IProvideValueTarget);
			object[] array55 = new object[0 + 5];
			array55[0] = sfExpander3;
			array55[1] = stackLayout24;
			array55[2] = scrollView;
			array55[3] = grid25;
			array55[4] = this;
			object obj88;
			xamlServiceProvider55.Add(typeFromHandle109, obj88 = new SimpleValueTargetProvider(array55, SfExpander.IconColorProperty, nameScope));
			xamlServiceProvider55.Add(typeof(IReferenceProvider), obj88);
			Type typeFromHandle110 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver55 = new XmlNamespaceResolver();
			xmlNamespaceResolver55.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver55.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver55.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver55.Add("dash", "clr-namespace:CarScannerXamarinForms.Dashboard");
			xmlNamespaceResolver55.Add("expander", "clr-namespace:Syncfusion.XForms.Expander;assembly=Syncfusion.Expander.XForms");
			xmlNamespaceResolver55.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver55.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver55.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver55.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider55.Add(typeFromHandle110, new XamlTypeResolver(xmlNamespaceResolver55, typeof(DashboardItemEditor).GetTypeInfo().Assembly));
			xamlServiceProvider55.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(543, 25)));
			DynamicResource dynamicResource20 = markupExtension55.ProvideValue(xamlServiceProvider55);
			sfExpander3.SetDynamicResource(SfExpander.IconColorProperty, dynamicResource20.Key);
			sfExpander3.SetValue(SfExpander.IsExpandedProperty, false);
			label19.SetValue(View.MarginProperty, new Thickness(5.0));
			label19.SetValue(Label.FontAttributesProperty, new FontAttributesConverter().ConvertFromInvariantString("Bold"));
			translate27.Text = "DashItem_GaugeOptions";
			IMarkupExtension markupExtension56 = translate27;
			XamlServiceProvider xamlServiceProvider56 = new XamlServiceProvider();
			Type typeFromHandle111 = typeof(IProvideValueTarget);
			object[] array56 = new object[0 + 7];
			array56[0] = label19;
			array56[1] = grid11;
			array56[2] = sfExpander3;
			array56[3] = stackLayout24;
			array56[4] = scrollView;
			array56[5] = grid25;
			array56[6] = this;
			object obj89;
			xamlServiceProvider56.Add(typeFromHandle111, obj89 = new SimpleValueTargetProvider(array56, Label.TextProperty, nameScope));
			xamlServiceProvider56.Add(typeof(IReferenceProvider), obj89);
			Type typeFromHandle112 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver56 = new XmlNamespaceResolver();
			xmlNamespaceResolver56.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver56.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver56.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver56.Add("dash", "clr-namespace:CarScannerXamarinForms.Dashboard");
			xmlNamespaceResolver56.Add("expander", "clr-namespace:Syncfusion.XForms.Expander;assembly=Syncfusion.Expander.XForms");
			xmlNamespaceResolver56.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver56.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver56.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver56.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider56.Add(typeFromHandle112, new XamlTypeResolver(xmlNamespaceResolver56, typeof(DashboardItemEditor).GetTypeInfo().Assembly));
			xamlServiceProvider56.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(550, 37)));
			object obj90 = markupExtension56.ProvideValue(xamlServiceProvider56);
			label19.Text = obj90;
			dynamicResourceExtension21.Key = "NavigationBarTextColor";
			IMarkupExtension<DynamicResource> markupExtension57 = dynamicResourceExtension21;
			XamlServiceProvider xamlServiceProvider57 = new XamlServiceProvider();
			Type typeFromHandle113 = typeof(IProvideValueTarget);
			object[] array57 = new object[0 + 7];
			array57[0] = label19;
			array57[1] = grid11;
			array57[2] = sfExpander3;
			array57[3] = stackLayout24;
			array57[4] = scrollView;
			array57[5] = grid25;
			array57[6] = this;
			object obj91;
			xamlServiceProvider57.Add(typeFromHandle113, obj91 = new SimpleValueTargetProvider(array57, Label.TextColorProperty, nameScope));
			xamlServiceProvider57.Add(typeof(IReferenceProvider), obj91);
			Type typeFromHandle114 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver57 = new XmlNamespaceResolver();
			xmlNamespaceResolver57.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver57.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver57.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver57.Add("dash", "clr-namespace:CarScannerXamarinForms.Dashboard");
			xmlNamespaceResolver57.Add("expander", "clr-namespace:Syncfusion.XForms.Expander;assembly=Syncfusion.Expander.XForms");
			xmlNamespaceResolver57.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver57.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver57.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver57.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider57.Add(typeFromHandle114, new XamlTypeResolver(xmlNamespaceResolver57, typeof(DashboardItemEditor).GetTypeInfo().Assembly));
			xamlServiceProvider57.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(551, 37)));
			DynamicResource dynamicResource21 = markupExtension57.ProvideValue(xamlServiceProvider57);
			label19.SetDynamicResource(Label.TextColorProperty, dynamicResource21.Key);
			label19.SetValue(View.VerticalOptionsProperty, LayoutOptions.Center);
			label19.SetValue(Label.VerticalTextAlignmentProperty, new TextAlignmentConverter().ConvertFromInvariantString("Center"));
			grid11.Children.Add(label19);
			sfExpander3.SetValue(SfExpander.HeaderProperty, grid11);
			stackLayout8.SetValue(StackLayout.OrientationProperty, 0);
			translate28.Text = "ios_DashboardItemEditor_Minimum";
			IMarkupExtension markupExtension58 = translate28;
			XamlServiceProvider xamlServiceProvider58 = new XamlServiceProvider();
			Type typeFromHandle115 = typeof(IProvideValueTarget);
			object[] array58 = new object[0 + 8];
			array58[0] = label20;
			array58[1] = stackLayout8;
			array58[2] = frame5;
			array58[3] = sfExpander3;
			array58[4] = stackLayout24;
			array58[5] = scrollView;
			array58[6] = grid25;
			array58[7] = this;
			object obj92;
			xamlServiceProvider58.Add(typeFromHandle115, obj92 = new SimpleValueTargetProvider(array58, Label.TextProperty, nameScope));
			xamlServiceProvider58.Add(typeof(IReferenceProvider), obj92);
			Type typeFromHandle116 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver58 = new XmlNamespaceResolver();
			xmlNamespaceResolver58.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver58.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver58.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver58.Add("dash", "clr-namespace:CarScannerXamarinForms.Dashboard");
			xmlNamespaceResolver58.Add("expander", "clr-namespace:Syncfusion.XForms.Expander;assembly=Syncfusion.Expander.XForms");
			xmlNamespaceResolver58.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver58.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver58.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver58.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider58.Add(typeFromHandle116, new XamlTypeResolver(xmlNamespaceResolver58, typeof(DashboardItemEditor).GetTypeInfo().Assembly));
			xamlServiceProvider58.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(559, 44)));
			object obj93 = markupExtension58.ProvideValue(xamlServiceProvider58);
			label20.Text = obj93;
			stackLayout8.Children.Add(label20);
			bindingExtension34.Mode = 1;
			bindingExtension34.Path = "Minimum";
			bindingExtension34.TypedBinding = new TypedBinding<DashboardItem, double>(delegate(DashboardItem A_0)
			{
				if (A_0 != null)
				{
					return new ValueTuple<double, bool>(A_0.Minimum, true);
				}
				return default(ValueTuple<double, bool>);
			}, delegate(DashboardItem A_0, double A_1)
			{
				if (A_0 != null)
				{
					A_0.Minimum = A_1;
					return;
				}
			}, new Tuple<Func<DashboardItem, object>, string>[]
			{
				new Tuple<Func<DashboardItem, object>, string>((DashboardItem A_0) => A_0, "Minimum")
			});
			BindingBase bindingBase34 = bindingExtension34.ProvideValue(null);
			numericEntryV.SetBinding(NumericEntryV3.ValueProperty, bindingBase34);
			stackLayout8.Children.Add(numericEntryV);
			translate29.Text = "ios_DashboardItemEditor_Maximum";
			IMarkupExtension markupExtension59 = translate29;
			XamlServiceProvider xamlServiceProvider59 = new XamlServiceProvider();
			Type typeFromHandle117 = typeof(IProvideValueTarget);
			object[] array59 = new object[0 + 8];
			array59[0] = label21;
			array59[1] = stackLayout8;
			array59[2] = frame5;
			array59[3] = sfExpander3;
			array59[4] = stackLayout24;
			array59[5] = scrollView;
			array59[6] = grid25;
			array59[7] = this;
			object obj94;
			xamlServiceProvider59.Add(typeFromHandle117, obj94 = new SimpleValueTargetProvider(array59, Label.TextProperty, nameScope));
			xamlServiceProvider59.Add(typeof(IReferenceProvider), obj94);
			Type typeFromHandle118 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver59 = new XmlNamespaceResolver();
			xmlNamespaceResolver59.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver59.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver59.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver59.Add("dash", "clr-namespace:CarScannerXamarinForms.Dashboard");
			xmlNamespaceResolver59.Add("expander", "clr-namespace:Syncfusion.XForms.Expander;assembly=Syncfusion.Expander.XForms");
			xmlNamespaceResolver59.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver59.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver59.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver59.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider59.Add(typeFromHandle118, new XamlTypeResolver(xmlNamespaceResolver59, typeof(DashboardItemEditor).GetTypeInfo().Assembly));
			xamlServiceProvider59.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(561, 44)));
			object obj95 = markupExtension59.ProvideValue(xamlServiceProvider59);
			label21.Text = obj95;
			stackLayout8.Children.Add(label21);
			bindingExtension35.Mode = 1;
			bindingExtension35.Path = "Maximum";
			bindingExtension35.TypedBinding = new TypedBinding<DashboardItem, double>(delegate(DashboardItem A_0)
			{
				if (A_0 != null)
				{
					return new ValueTuple<double, bool>(A_0.Maximum, true);
				}
				return default(ValueTuple<double, bool>);
			}, delegate(DashboardItem A_0, double A_1)
			{
				if (A_0 != null)
				{
					A_0.Maximum = A_1;
					return;
				}
			}, new Tuple<Func<DashboardItem, object>, string>[]
			{
				new Tuple<Func<DashboardItem, object>, string>((DashboardItem A_0) => A_0, "Maximum")
			});
			BindingBase bindingBase35 = bindingExtension35.ProvideValue(null);
			numericEntryV2.SetBinding(NumericEntryV3.ValueProperty, bindingBase35);
			stackLayout8.Children.Add(numericEntryV2);
			bindingExtension36.Mode = 1;
			bindingExtension36.Path = "GaugeShowBlueLine";
			bindingExtension36.TypedBinding = new TypedBinding<DashboardItem, bool>(delegate(DashboardItem A_0)
			{
				if (A_0 != null)
				{
					return new ValueTuple<bool, bool>(A_0.GaugeShowBlueLine, true);
				}
				return default(ValueTuple<bool, bool>);
			}, delegate(DashboardItem A_0, bool A_1)
			{
				if (A_0 != null)
				{
					A_0.GaugeShowBlueLine = A_1;
					return;
				}
			}, new Tuple<Func<DashboardItem, object>, string>[]
			{
				new Tuple<Func<DashboardItem, object>, string>((DashboardItem A_0) => A_0, "GaugeShowBlueLine")
			});
			BindingBase bindingBase36 = bindingExtension36.ProvideValue(null);
			labelSwitch4.SetBinding(LabelSwitch.IsToggledProperty, bindingBase36);
			translate30.Text = "ios_DashboardItemEditor_GaugeShowBlueLine";
			IMarkupExtension markupExtension60 = translate30;
			XamlServiceProvider xamlServiceProvider60 = new XamlServiceProvider();
			Type typeFromHandle119 = typeof(IProvideValueTarget);
			object[] array60 = new object[0 + 8];
			array60[0] = labelSwitch4;
			array60[1] = stackLayout8;
			array60[2] = frame5;
			array60[3] = sfExpander3;
			array60[4] = stackLayout24;
			array60[5] = scrollView;
			array60[6] = grid25;
			array60[7] = this;
			object obj96;
			xamlServiceProvider60.Add(typeFromHandle119, obj96 = new SimpleValueTargetProvider(array60, LabelSwitch.TextProperty, nameScope));
			xamlServiceProvider60.Add(typeof(IReferenceProvider), obj96);
			Type typeFromHandle120 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver60 = new XmlNamespaceResolver();
			xmlNamespaceResolver60.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver60.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver60.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver60.Add("dash", "clr-namespace:CarScannerXamarinForms.Dashboard");
			xmlNamespaceResolver60.Add("expander", "clr-namespace:Syncfusion.XForms.Expander;assembly=Syncfusion.Expander.XForms");
			xmlNamespaceResolver60.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver60.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver60.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver60.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider60.Add(typeFromHandle120, new XamlTypeResolver(xmlNamespaceResolver60, typeof(DashboardItemEditor).GetTypeInfo().Assembly));
			xamlServiceProvider60.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(566, 109)));
			object obj97 = markupExtension60.ProvideValue(xamlServiceProvider60);
			labelSwitch4.Text = obj97;
			stackLayout8.Children.Add(labelSwitch4);
			bindingExtension37.Mode = 2;
			bindingExtension37.Path = "GaugeShowBlueLine";
			bindingExtension37.TypedBinding = new TypedBinding<DashboardItem, bool>(delegate(DashboardItem A_0)
			{
				if (A_0 != null)
				{
					return new ValueTuple<bool, bool>(A_0.GaugeShowBlueLine, true);
				}
				return default(ValueTuple<bool, bool>);
			}, null, new Tuple<Func<DashboardItem, object>, string>[]
			{
				new Tuple<Func<DashboardItem, object>, string>((DashboardItem A_0) => A_0, "GaugeShowBlueLine")
			});
			BindingBase bindingBase37 = bindingExtension37.ProvideValue(null);
			stackLayout6.SetBinding(VisualElement.IsVisibleProperty, bindingBase37);
			stackLayout6.SetValue(StackLayout.OrientationProperty, 0);
			translate31.Text = "ios_DashboardItemEditor_GaugeBlueLineStart";
			IMarkupExtension markupExtension61 = translate31;
			XamlServiceProvider xamlServiceProvider61 = new XamlServiceProvider();
			Type typeFromHandle121 = typeof(IProvideValueTarget);
			object[] array61 = new object[0 + 9];
			array61[0] = label22;
			array61[1] = stackLayout6;
			array61[2] = stackLayout8;
			array61[3] = frame5;
			array61[4] = sfExpander3;
			array61[5] = stackLayout24;
			array61[6] = scrollView;
			array61[7] = grid25;
			array61[8] = this;
			object obj98;
			xamlServiceProvider61.Add(typeFromHandle121, obj98 = new SimpleValueTargetProvider(array61, Label.TextProperty, nameScope));
			xamlServiceProvider61.Add(typeof(IReferenceProvider), obj98);
			Type typeFromHandle122 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver61 = new XmlNamespaceResolver();
			xmlNamespaceResolver61.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver61.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver61.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver61.Add("dash", "clr-namespace:CarScannerXamarinForms.Dashboard");
			xmlNamespaceResolver61.Add("expander", "clr-namespace:Syncfusion.XForms.Expander;assembly=Syncfusion.Expander.XForms");
			xmlNamespaceResolver61.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver61.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver61.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver61.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider61.Add(typeFromHandle122, new XamlTypeResolver(xmlNamespaceResolver61, typeof(DashboardItemEditor).GetTypeInfo().Assembly));
			xamlServiceProvider61.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(568, 48)));
			object obj99 = markupExtension61.ProvideValue(xamlServiceProvider61);
			label22.Text = obj99;
			stackLayout6.Children.Add(label22);
			bindingExtension38.Mode = 1;
			bindingExtension38.Path = "GaugeBlueLineStart";
			bindingExtension38.TypedBinding = new TypedBinding<DashboardItem, double>(delegate(DashboardItem A_0)
			{
				if (A_0 != null)
				{
					return new ValueTuple<double, bool>(A_0.GaugeBlueLineStart, true);
				}
				return default(ValueTuple<double, bool>);
			}, delegate(DashboardItem A_0, double A_1)
			{
				if (A_0 != null)
				{
					A_0.GaugeBlueLineStart = A_1;
					return;
				}
			}, new Tuple<Func<DashboardItem, object>, string>[]
			{
				new Tuple<Func<DashboardItem, object>, string>((DashboardItem A_0) => A_0, "GaugeBlueLineStart")
			});
			BindingBase bindingBase38 = bindingExtension38.ProvideValue(null);
			numericEntryV3.SetBinding(NumericEntryV3.ValueProperty, bindingBase38);
			stackLayout6.Children.Add(numericEntryV3);
			translate32.Text = "ios_DashboardItemEditor_GaugeBlueLineFinish";
			IMarkupExtension markupExtension62 = translate32;
			XamlServiceProvider xamlServiceProvider62 = new XamlServiceProvider();
			Type typeFromHandle123 = typeof(IProvideValueTarget);
			object[] array62 = new object[0 + 9];
			array62[0] = label23;
			array62[1] = stackLayout6;
			array62[2] = stackLayout8;
			array62[3] = frame5;
			array62[4] = sfExpander3;
			array62[5] = stackLayout24;
			array62[6] = scrollView;
			array62[7] = grid25;
			array62[8] = this;
			object obj100;
			xamlServiceProvider62.Add(typeFromHandle123, obj100 = new SimpleValueTargetProvider(array62, Label.TextProperty, nameScope));
			xamlServiceProvider62.Add(typeof(IReferenceProvider), obj100);
			Type typeFromHandle124 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver62 = new XmlNamespaceResolver();
			xmlNamespaceResolver62.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver62.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver62.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver62.Add("dash", "clr-namespace:CarScannerXamarinForms.Dashboard");
			xmlNamespaceResolver62.Add("expander", "clr-namespace:Syncfusion.XForms.Expander;assembly=Syncfusion.Expander.XForms");
			xmlNamespaceResolver62.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver62.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver62.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver62.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider62.Add(typeFromHandle124, new XamlTypeResolver(xmlNamespaceResolver62, typeof(DashboardItemEditor).GetTypeInfo().Assembly));
			xamlServiceProvider62.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(571, 48)));
			object obj101 = markupExtension62.ProvideValue(xamlServiceProvider62);
			label23.Text = obj101;
			stackLayout6.Children.Add(label23);
			bindingExtension39.Mode = 1;
			bindingExtension39.Path = "GaugeBlueLineFinish";
			bindingExtension39.TypedBinding = new TypedBinding<DashboardItem, double>(delegate(DashboardItem A_0)
			{
				if (A_0 != null)
				{
					return new ValueTuple<double, bool>(A_0.GaugeBlueLineFinish, true);
				}
				return default(ValueTuple<double, bool>);
			}, delegate(DashboardItem A_0, double A_1)
			{
				if (A_0 != null)
				{
					A_0.GaugeBlueLineFinish = A_1;
					return;
				}
			}, new Tuple<Func<DashboardItem, object>, string>[]
			{
				new Tuple<Func<DashboardItem, object>, string>((DashboardItem A_0) => A_0, "GaugeBlueLineFinish")
			});
			BindingBase bindingBase39 = bindingExtension39.ProvideValue(null);
			numericEntryV4.SetBinding(NumericEntryV3.ValueProperty, bindingBase39);
			stackLayout6.Children.Add(numericEntryV4);
			dashboardItemColorBox8.SetValue(Element.ClassIdProperty, "GaugeBlueLineColor");
			dashboardItemColorBox8.Tapped += this.ColorBox_Tapped;
			translate33.Text = "ios_DashboardItemEditor_GaugeBlueLineColor";
			IMarkupExtension markupExtension63 = translate33;
			XamlServiceProvider xamlServiceProvider63 = new XamlServiceProvider();
			Type typeFromHandle125 = typeof(IProvideValueTarget);
			object[] array63 = new object[0 + 9];
			array63[0] = dashboardItemColorBox8;
			array63[1] = stackLayout6;
			array63[2] = stackLayout8;
			array63[3] = frame5;
			array63[4] = sfExpander3;
			array63[5] = stackLayout24;
			array63[6] = scrollView;
			array63[7] = grid25;
			array63[8] = this;
			object obj102;
			xamlServiceProvider63.Add(typeFromHandle125, obj102 = new SimpleValueTargetProvider(array63, DashboardItemColorBox.TextProperty, nameScope));
			xamlServiceProvider63.Add(typeof(IReferenceProvider), obj102);
			Type typeFromHandle126 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver63 = new XmlNamespaceResolver();
			xmlNamespaceResolver63.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver63.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver63.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver63.Add("dash", "clr-namespace:CarScannerXamarinForms.Dashboard");
			xmlNamespaceResolver63.Add("expander", "clr-namespace:Syncfusion.XForms.Expander;assembly=Syncfusion.Expander.XForms");
			xmlNamespaceResolver63.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver63.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver63.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver63.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider63.Add(typeFromHandle126, new XamlTypeResolver(xmlNamespaceResolver63, typeof(DashboardItemEditor).GetTypeInfo().Assembly));
			xamlServiceProvider63.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(576, 45)));
			object obj103 = markupExtension63.ProvideValue(xamlServiceProvider63);
			dashboardItemColorBox8.Text = obj103;
			bindingExtension40.Mode = 2;
			bindingExtension40.Path = "GaugeBlueLineColor";
			bindingExtension40.TypedBinding = new TypedBinding<DashboardItem, Color>(delegate(DashboardItem A_0)
			{
				if (A_0 != null)
				{
					return new ValueTuple<Color, bool>(A_0.GaugeBlueLineColor, true);
				}
				return default(ValueTuple<Color, bool>);
			}, null, new Tuple<Func<DashboardItem, object>, string>[]
			{
				new Tuple<Func<DashboardItem, object>, string>((DashboardItem A_0) => A_0, "GaugeBlueLineColor")
			});
			BindingBase bindingBase40 = bindingExtension40.ProvideValue(null);
			dashboardItemColorBox8.SetBinding(DashboardItemColorBox.ColorProperty, bindingBase40);
			stackLayout6.Children.Add(dashboardItemColorBox8);
			stackLayout8.Children.Add(stackLayout6);
			bindingExtension41.Mode = 1;
			bindingExtension41.Path = "GaugeShowRedLine";
			bindingExtension41.TypedBinding = new TypedBinding<DashboardItem, bool>(delegate(DashboardItem A_0)
			{
				if (A_0 != null)
				{
					return new ValueTuple<bool, bool>(A_0.GaugeShowRedLine, true);
				}
				return default(ValueTuple<bool, bool>);
			}, delegate(DashboardItem A_0, bool A_1)
			{
				if (A_0 != null)
				{
					A_0.GaugeShowRedLine = A_1;
					return;
				}
			}, new Tuple<Func<DashboardItem, object>, string>[]
			{
				new Tuple<Func<DashboardItem, object>, string>((DashboardItem A_0) => A_0, "GaugeShowRedLine")
			});
			BindingBase bindingBase41 = bindingExtension41.ProvideValue(null);
			labelSwitch5.SetBinding(LabelSwitch.IsToggledProperty, bindingBase41);
			translate34.Text = "ios_DashboardItemEditor_GaugeShowRedLine";
			IMarkupExtension markupExtension64 = translate34;
			XamlServiceProvider xamlServiceProvider64 = new XamlServiceProvider();
			Type typeFromHandle127 = typeof(IProvideValueTarget);
			object[] array64 = new object[0 + 8];
			array64[0] = labelSwitch5;
			array64[1] = stackLayout8;
			array64[2] = frame5;
			array64[3] = sfExpander3;
			array64[4] = stackLayout24;
			array64[5] = scrollView;
			array64[6] = grid25;
			array64[7] = this;
			object obj104;
			xamlServiceProvider64.Add(typeFromHandle127, obj104 = new SimpleValueTargetProvider(array64, LabelSwitch.TextProperty, nameScope));
			xamlServiceProvider64.Add(typeof(IReferenceProvider), obj104);
			Type typeFromHandle128 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver64 = new XmlNamespaceResolver();
			xmlNamespaceResolver64.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver64.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver64.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver64.Add("dash", "clr-namespace:CarScannerXamarinForms.Dashboard");
			xmlNamespaceResolver64.Add("expander", "clr-namespace:Syncfusion.XForms.Expander;assembly=Syncfusion.Expander.XForms");
			xmlNamespaceResolver64.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver64.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver64.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver64.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider64.Add(typeFromHandle128, new XamlTypeResolver(xmlNamespaceResolver64, typeof(DashboardItemEditor).GetTypeInfo().Assembly));
			xamlServiceProvider64.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(582, 108)));
			object obj105 = markupExtension64.ProvideValue(xamlServiceProvider64);
			labelSwitch5.Text = obj105;
			stackLayout8.Children.Add(labelSwitch5);
			bindingExtension42.Mode = 2;
			bindingExtension42.Path = "GaugeShowRedLine";
			bindingExtension42.TypedBinding = new TypedBinding<DashboardItem, bool>(delegate(DashboardItem A_0)
			{
				if (A_0 != null)
				{
					return new ValueTuple<bool, bool>(A_0.GaugeShowRedLine, true);
				}
				return default(ValueTuple<bool, bool>);
			}, null, new Tuple<Func<DashboardItem, object>, string>[]
			{
				new Tuple<Func<DashboardItem, object>, string>((DashboardItem A_0) => A_0, "GaugeShowRedLine")
			});
			BindingBase bindingBase42 = bindingExtension42.ProvideValue(null);
			stackLayout7.SetBinding(VisualElement.IsVisibleProperty, bindingBase42);
			stackLayout7.SetValue(StackLayout.OrientationProperty, 0);
			translate35.Text = "ios_DashboardItemEditor_GaugeRedLineStart";
			IMarkupExtension markupExtension65 = translate35;
			XamlServiceProvider xamlServiceProvider65 = new XamlServiceProvider();
			Type typeFromHandle129 = typeof(IProvideValueTarget);
			object[] array65 = new object[0 + 9];
			array65[0] = label24;
			array65[1] = stackLayout7;
			array65[2] = stackLayout8;
			array65[3] = frame5;
			array65[4] = sfExpander3;
			array65[5] = stackLayout24;
			array65[6] = scrollView;
			array65[7] = grid25;
			array65[8] = this;
			object obj106;
			xamlServiceProvider65.Add(typeFromHandle129, obj106 = new SimpleValueTargetProvider(array65, Label.TextProperty, nameScope));
			xamlServiceProvider65.Add(typeof(IReferenceProvider), obj106);
			Type typeFromHandle130 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver65 = new XmlNamespaceResolver();
			xmlNamespaceResolver65.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver65.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver65.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver65.Add("dash", "clr-namespace:CarScannerXamarinForms.Dashboard");
			xmlNamespaceResolver65.Add("expander", "clr-namespace:Syncfusion.XForms.Expander;assembly=Syncfusion.Expander.XForms");
			xmlNamespaceResolver65.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver65.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver65.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver65.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider65.Add(typeFromHandle130, new XamlTypeResolver(xmlNamespaceResolver65, typeof(DashboardItemEditor).GetTypeInfo().Assembly));
			xamlServiceProvider65.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(584, 48)));
			object obj107 = markupExtension65.ProvideValue(xamlServiceProvider65);
			label24.Text = obj107;
			stackLayout7.Children.Add(label24);
			bindingExtension43.Mode = 1;
			bindingExtension43.Path = "GaugeRedLineStart";
			bindingExtension43.TypedBinding = new TypedBinding<DashboardItem, double>(delegate(DashboardItem A_0)
			{
				if (A_0 != null)
				{
					return new ValueTuple<double, bool>(A_0.GaugeRedLineStart, true);
				}
				return default(ValueTuple<double, bool>);
			}, delegate(DashboardItem A_0, double A_1)
			{
				if (A_0 != null)
				{
					A_0.GaugeRedLineStart = A_1;
					return;
				}
			}, new Tuple<Func<DashboardItem, object>, string>[]
			{
				new Tuple<Func<DashboardItem, object>, string>((DashboardItem A_0) => A_0, "GaugeRedLineStart")
			});
			BindingBase bindingBase43 = bindingExtension43.ProvideValue(null);
			numericEntryV5.SetBinding(NumericEntryV3.ValueProperty, bindingBase43);
			stackLayout7.Children.Add(numericEntryV5);
			translate36.Text = "ios_DashboardItemEditor_GaugeRedLineFinish";
			IMarkupExtension markupExtension66 = translate36;
			XamlServiceProvider xamlServiceProvider66 = new XamlServiceProvider();
			Type typeFromHandle131 = typeof(IProvideValueTarget);
			object[] array66 = new object[0 + 9];
			array66[0] = label25;
			array66[1] = stackLayout7;
			array66[2] = stackLayout8;
			array66[3] = frame5;
			array66[4] = sfExpander3;
			array66[5] = stackLayout24;
			array66[6] = scrollView;
			array66[7] = grid25;
			array66[8] = this;
			object obj108;
			xamlServiceProvider66.Add(typeFromHandle131, obj108 = new SimpleValueTargetProvider(array66, Label.TextProperty, nameScope));
			xamlServiceProvider66.Add(typeof(IReferenceProvider), obj108);
			Type typeFromHandle132 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver66 = new XmlNamespaceResolver();
			xmlNamespaceResolver66.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver66.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver66.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver66.Add("dash", "clr-namespace:CarScannerXamarinForms.Dashboard");
			xmlNamespaceResolver66.Add("expander", "clr-namespace:Syncfusion.XForms.Expander;assembly=Syncfusion.Expander.XForms");
			xmlNamespaceResolver66.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver66.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver66.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver66.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider66.Add(typeFromHandle132, new XamlTypeResolver(xmlNamespaceResolver66, typeof(DashboardItemEditor).GetTypeInfo().Assembly));
			xamlServiceProvider66.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(587, 48)));
			object obj109 = markupExtension66.ProvideValue(xamlServiceProvider66);
			label25.Text = obj109;
			stackLayout7.Children.Add(label25);
			bindingExtension44.Mode = 1;
			bindingExtension44.Path = "GaugeRedLineFinish";
			bindingExtension44.TypedBinding = new TypedBinding<DashboardItem, double>(delegate(DashboardItem A_0)
			{
				if (A_0 != null)
				{
					return new ValueTuple<double, bool>(A_0.GaugeRedLineFinish, true);
				}
				return default(ValueTuple<double, bool>);
			}, delegate(DashboardItem A_0, double A_1)
			{
				if (A_0 != null)
				{
					A_0.GaugeRedLineFinish = A_1;
					return;
				}
			}, new Tuple<Func<DashboardItem, object>, string>[]
			{
				new Tuple<Func<DashboardItem, object>, string>((DashboardItem A_0) => A_0, "GaugeRedLineFinish")
			});
			BindingBase bindingBase44 = bindingExtension44.ProvideValue(null);
			numericEntryV6.SetBinding(NumericEntryV3.ValueProperty, bindingBase44);
			stackLayout7.Children.Add(numericEntryV6);
			dashboardItemColorBox9.SetValue(Element.ClassIdProperty, "GaugeRedLineColor");
			dashboardItemColorBox9.Tapped += this.ColorBox_Tapped;
			translate37.Text = "ios_DashboardItemEditor_GaugeRedLineColor";
			IMarkupExtension markupExtension67 = translate37;
			XamlServiceProvider xamlServiceProvider67 = new XamlServiceProvider();
			Type typeFromHandle133 = typeof(IProvideValueTarget);
			object[] array67 = new object[0 + 9];
			array67[0] = dashboardItemColorBox9;
			array67[1] = stackLayout7;
			array67[2] = stackLayout8;
			array67[3] = frame5;
			array67[4] = sfExpander3;
			array67[5] = stackLayout24;
			array67[6] = scrollView;
			array67[7] = grid25;
			array67[8] = this;
			object obj110;
			xamlServiceProvider67.Add(typeFromHandle133, obj110 = new SimpleValueTargetProvider(array67, DashboardItemColorBox.TextProperty, nameScope));
			xamlServiceProvider67.Add(typeof(IReferenceProvider), obj110);
			Type typeFromHandle134 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver67 = new XmlNamespaceResolver();
			xmlNamespaceResolver67.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver67.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver67.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver67.Add("dash", "clr-namespace:CarScannerXamarinForms.Dashboard");
			xmlNamespaceResolver67.Add("expander", "clr-namespace:Syncfusion.XForms.Expander;assembly=Syncfusion.Expander.XForms");
			xmlNamespaceResolver67.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver67.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver67.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver67.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider67.Add(typeFromHandle134, new XamlTypeResolver(xmlNamespaceResolver67, typeof(DashboardItemEditor).GetTypeInfo().Assembly));
			xamlServiceProvider67.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(592, 45)));
			object obj111 = markupExtension67.ProvideValue(xamlServiceProvider67);
			dashboardItemColorBox9.Text = obj111;
			bindingExtension45.Mode = 2;
			bindingExtension45.Path = "GaugeRedLineColor";
			bindingExtension45.TypedBinding = new TypedBinding<DashboardItem, Color>(delegate(DashboardItem A_0)
			{
				if (A_0 != null)
				{
					return new ValueTuple<Color, bool>(A_0.GaugeRedLineColor, true);
				}
				return default(ValueTuple<Color, bool>);
			}, null, new Tuple<Func<DashboardItem, object>, string>[]
			{
				new Tuple<Func<DashboardItem, object>, string>((DashboardItem A_0) => A_0, "GaugeRedLineColor")
			});
			BindingBase bindingBase45 = bindingExtension45.ProvideValue(null);
			dashboardItemColorBox9.SetBinding(DashboardItemColorBox.ColorProperty, bindingBase45);
			stackLayout7.Children.Add(dashboardItemColorBox9);
			stackLayout8.Children.Add(stackLayout7);
			dashboardItemColorBox10.SetValue(Element.ClassIdProperty, "GaugeLabelColor");
			dashboardItemColorBox10.Tapped += this.ColorBox_Tapped;
			translate38.Text = "ios_DashboardItemEditor_GaugeLabelsColor";
			IMarkupExtension markupExtension68 = translate38;
			XamlServiceProvider xamlServiceProvider68 = new XamlServiceProvider();
			Type typeFromHandle135 = typeof(IProvideValueTarget);
			object[] array68 = new object[0 + 8];
			array68[0] = dashboardItemColorBox10;
			array68[1] = stackLayout8;
			array68[2] = frame5;
			array68[3] = sfExpander3;
			array68[4] = stackLayout24;
			array68[5] = scrollView;
			array68[6] = grid25;
			array68[7] = this;
			object obj112;
			xamlServiceProvider68.Add(typeFromHandle135, obj112 = new SimpleValueTargetProvider(array68, DashboardItemColorBox.TextProperty, nameScope));
			xamlServiceProvider68.Add(typeof(IReferenceProvider), obj112);
			Type typeFromHandle136 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver68 = new XmlNamespaceResolver();
			xmlNamespaceResolver68.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver68.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver68.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver68.Add("dash", "clr-namespace:CarScannerXamarinForms.Dashboard");
			xmlNamespaceResolver68.Add("expander", "clr-namespace:Syncfusion.XForms.Expander;assembly=Syncfusion.Expander.XForms");
			xmlNamespaceResolver68.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver68.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver68.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver68.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider68.Add(typeFromHandle136, new XamlTypeResolver(xmlNamespaceResolver68, typeof(DashboardItemEditor).GetTypeInfo().Assembly));
			xamlServiceProvider68.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(599, 41)));
			object obj113 = markupExtension68.ProvideValue(xamlServiceProvider68);
			dashboardItemColorBox10.Text = obj113;
			bindingExtension46.Mode = 2;
			bindingExtension46.Path = "GaugeLabelColor";
			bindingExtension46.TypedBinding = new TypedBinding<DashboardItem, Color>(delegate(DashboardItem A_0)
			{
				if (A_0 != null)
				{
					return new ValueTuple<Color, bool>(A_0.GaugeLabelColor, true);
				}
				return default(ValueTuple<Color, bool>);
			}, null, new Tuple<Func<DashboardItem, object>, string>[]
			{
				new Tuple<Func<DashboardItem, object>, string>((DashboardItem A_0) => A_0, "GaugeLabelColor")
			});
			BindingBase bindingBase46 = bindingExtension46.ProvideValue(null);
			dashboardItemColorBox10.SetBinding(DashboardItemColorBox.ColorProperty, bindingBase46);
			stackLayout8.Children.Add(dashboardItemColorBox10);
			dashboardItemColorBox11.SetValue(Element.ClassIdProperty, "GaugeRimColor");
			dashboardItemColorBox11.Tapped += this.ColorBox_Tapped;
			translate39.Text = "ios_DashboardItemEditor_GaugeRimColor";
			IMarkupExtension markupExtension69 = translate39;
			XamlServiceProvider xamlServiceProvider69 = new XamlServiceProvider();
			Type typeFromHandle137 = typeof(IProvideValueTarget);
			object[] array69 = new object[0 + 8];
			array69[0] = dashboardItemColorBox11;
			array69[1] = stackLayout8;
			array69[2] = frame5;
			array69[3] = sfExpander3;
			array69[4] = stackLayout24;
			array69[5] = scrollView;
			array69[6] = grid25;
			array69[7] = this;
			object obj114;
			xamlServiceProvider69.Add(typeFromHandle137, obj114 = new SimpleValueTargetProvider(array69, DashboardItemColorBox.TextProperty, nameScope));
			xamlServiceProvider69.Add(typeof(IReferenceProvider), obj114);
			Type typeFromHandle138 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver69 = new XmlNamespaceResolver();
			xmlNamespaceResolver69.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver69.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver69.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver69.Add("dash", "clr-namespace:CarScannerXamarinForms.Dashboard");
			xmlNamespaceResolver69.Add("expander", "clr-namespace:Syncfusion.XForms.Expander;assembly=Syncfusion.Expander.XForms");
			xmlNamespaceResolver69.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver69.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver69.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver69.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider69.Add(typeFromHandle138, new XamlTypeResolver(xmlNamespaceResolver69, typeof(DashboardItemEditor).GetTypeInfo().Assembly));
			xamlServiceProvider69.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(605, 41)));
			object obj115 = markupExtension69.ProvideValue(xamlServiceProvider69);
			dashboardItemColorBox11.Text = obj115;
			bindingExtension47.Mode = 2;
			bindingExtension47.Path = "GaugeRimColor";
			bindingExtension47.TypedBinding = new TypedBinding<DashboardItem, Color>(delegate(DashboardItem A_0)
			{
				if (A_0 != null)
				{
					return new ValueTuple<Color, bool>(A_0.GaugeRimColor, true);
				}
				return default(ValueTuple<Color, bool>);
			}, null, new Tuple<Func<DashboardItem, object>, string>[]
			{
				new Tuple<Func<DashboardItem, object>, string>((DashboardItem A_0) => A_0, "GaugeRimColor")
			});
			BindingBase bindingBase47 = bindingExtension47.ProvideValue(null);
			dashboardItemColorBox11.SetBinding(DashboardItemColorBox.ColorProperty, bindingBase47);
			stackLayout8.Children.Add(dashboardItemColorBox11);
			dashboardItemColorBox12.SetValue(Element.ClassIdProperty, "GaugeTickColor");
			dashboardItemColorBox12.Tapped += this.ColorBox_Tapped;
			translate40.Text = "ios_DashboardItemEditor_GaugeTicksColor";
			IMarkupExtension markupExtension70 = translate40;
			XamlServiceProvider xamlServiceProvider70 = new XamlServiceProvider();
			Type typeFromHandle139 = typeof(IProvideValueTarget);
			object[] array70 = new object[0 + 8];
			array70[0] = dashboardItemColorBox12;
			array70[1] = stackLayout8;
			array70[2] = frame5;
			array70[3] = sfExpander3;
			array70[4] = stackLayout24;
			array70[5] = scrollView;
			array70[6] = grid25;
			array70[7] = this;
			object obj116;
			xamlServiceProvider70.Add(typeFromHandle139, obj116 = new SimpleValueTargetProvider(array70, DashboardItemColorBox.TextProperty, nameScope));
			xamlServiceProvider70.Add(typeof(IReferenceProvider), obj116);
			Type typeFromHandle140 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver70 = new XmlNamespaceResolver();
			xmlNamespaceResolver70.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver70.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver70.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver70.Add("dash", "clr-namespace:CarScannerXamarinForms.Dashboard");
			xmlNamespaceResolver70.Add("expander", "clr-namespace:Syncfusion.XForms.Expander;assembly=Syncfusion.Expander.XForms");
			xmlNamespaceResolver70.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver70.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver70.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver70.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider70.Add(typeFromHandle140, new XamlTypeResolver(xmlNamespaceResolver70, typeof(DashboardItemEditor).GetTypeInfo().Assembly));
			xamlServiceProvider70.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(613, 41)));
			object obj117 = markupExtension70.ProvideValue(xamlServiceProvider70);
			dashboardItemColorBox12.Text = obj117;
			bindingExtension48.Mode = 2;
			bindingExtension48.Path = "GaugeTickColor";
			bindingExtension48.TypedBinding = new TypedBinding<DashboardItem, Color>(delegate(DashboardItem A_0)
			{
				if (A_0 != null)
				{
					return new ValueTuple<Color, bool>(A_0.GaugeTickColor, true);
				}
				return default(ValueTuple<Color, bool>);
			}, null, new Tuple<Func<DashboardItem, object>, string>[]
			{
				new Tuple<Func<DashboardItem, object>, string>((DashboardItem A_0) => A_0, "GaugeTickColor")
			});
			BindingBase bindingBase48 = bindingExtension48.ProvideValue(null);
			dashboardItemColorBox12.SetBinding(DashboardItemColorBox.ColorProperty, bindingBase48);
			stackLayout8.Children.Add(dashboardItemColorBox12);
			dashboardItemColorBox13.SetValue(Element.ClassIdProperty, "GaugePointerColor");
			dashboardItemColorBox13.Tapped += this.ColorBox_Tapped;
			translate41.Text = "ios_DashboardItemEditor_GaugePointerColor";
			IMarkupExtension markupExtension71 = translate41;
			XamlServiceProvider xamlServiceProvider71 = new XamlServiceProvider();
			Type typeFromHandle141 = typeof(IProvideValueTarget);
			object[] array71 = new object[0 + 8];
			array71[0] = dashboardItemColorBox13;
			array71[1] = stackLayout8;
			array71[2] = frame5;
			array71[3] = sfExpander3;
			array71[4] = stackLayout24;
			array71[5] = scrollView;
			array71[6] = grid25;
			array71[7] = this;
			object obj118;
			xamlServiceProvider71.Add(typeFromHandle141, obj118 = new SimpleValueTargetProvider(array71, DashboardItemColorBox.TextProperty, nameScope));
			xamlServiceProvider71.Add(typeof(IReferenceProvider), obj118);
			Type typeFromHandle142 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver71 = new XmlNamespaceResolver();
			xmlNamespaceResolver71.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver71.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver71.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver71.Add("dash", "clr-namespace:CarScannerXamarinForms.Dashboard");
			xmlNamespaceResolver71.Add("expander", "clr-namespace:Syncfusion.XForms.Expander;assembly=Syncfusion.Expander.XForms");
			xmlNamespaceResolver71.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver71.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver71.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver71.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider71.Add(typeFromHandle142, new XamlTypeResolver(xmlNamespaceResolver71, typeof(DashboardItemEditor).GetTypeInfo().Assembly));
			xamlServiceProvider71.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(619, 41)));
			object obj119 = markupExtension71.ProvideValue(xamlServiceProvider71);
			dashboardItemColorBox13.Text = obj119;
			bindingExtension49.Mode = 2;
			bindingExtension49.Path = "GaugePointerColor";
			bindingExtension49.TypedBinding = new TypedBinding<DashboardItem, Color>(delegate(DashboardItem A_0)
			{
				if (A_0 != null)
				{
					return new ValueTuple<Color, bool>(A_0.GaugePointerColor, true);
				}
				return default(ValueTuple<Color, bool>);
			}, null, new Tuple<Func<DashboardItem, object>, string>[]
			{
				new Tuple<Func<DashboardItem, object>, string>((DashboardItem A_0) => A_0, "GaugePointerColor")
			});
			BindingBase bindingBase49 = bindingExtension49.ProvideValue(null);
			dashboardItemColorBox13.SetBinding(DashboardItemColorBox.ColorProperty, bindingBase49);
			stackLayout8.Children.Add(dashboardItemColorBox13);
			dashboardItemColorBox14.SetValue(Element.ClassIdProperty, "GaugeKnobColor");
			dashboardItemColorBox14.Tapped += this.ColorBox_Tapped;
			translate42.Text = "ios_DashboardItemEditor_GaugeKnobColor";
			IMarkupExtension markupExtension72 = translate42;
			XamlServiceProvider xamlServiceProvider72 = new XamlServiceProvider();
			Type typeFromHandle143 = typeof(IProvideValueTarget);
			object[] array72 = new object[0 + 8];
			array72[0] = dashboardItemColorBox14;
			array72[1] = stackLayout8;
			array72[2] = frame5;
			array72[3] = sfExpander3;
			array72[4] = stackLayout24;
			array72[5] = scrollView;
			array72[6] = grid25;
			array72[7] = this;
			object obj120;
			xamlServiceProvider72.Add(typeFromHandle143, obj120 = new SimpleValueTargetProvider(array72, DashboardItemColorBox.TextProperty, nameScope));
			xamlServiceProvider72.Add(typeof(IReferenceProvider), obj120);
			Type typeFromHandle144 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver72 = new XmlNamespaceResolver();
			xmlNamespaceResolver72.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver72.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver72.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver72.Add("dash", "clr-namespace:CarScannerXamarinForms.Dashboard");
			xmlNamespaceResolver72.Add("expander", "clr-namespace:Syncfusion.XForms.Expander;assembly=Syncfusion.Expander.XForms");
			xmlNamespaceResolver72.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver72.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver72.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver72.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider72.Add(typeFromHandle144, new XamlTypeResolver(xmlNamespaceResolver72, typeof(DashboardItemEditor).GetTypeInfo().Assembly));
			xamlServiceProvider72.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(627, 41)));
			object obj121 = markupExtension72.ProvideValue(xamlServiceProvider72);
			dashboardItemColorBox14.Text = obj121;
			bindingExtension50.Mode = 2;
			bindingExtension50.Path = "GaugeKnobColor";
			bindingExtension50.TypedBinding = new TypedBinding<DashboardItem, Color>(delegate(DashboardItem A_0)
			{
				if (A_0 != null)
				{
					return new ValueTuple<Color, bool>(A_0.GaugeKnobColor, true);
				}
				return default(ValueTuple<Color, bool>);
			}, null, new Tuple<Func<DashboardItem, object>, string>[]
			{
				new Tuple<Func<DashboardItem, object>, string>((DashboardItem A_0) => A_0, "GaugeKnobColor")
			});
			BindingBase bindingBase50 = bindingExtension50.ProvideValue(null);
			dashboardItemColorBox14.SetBinding(DashboardItemColorBox.ColorProperty, bindingBase50);
			stackLayout8.Children.Add(dashboardItemColorBox14);
			frame5.SetValue(ContentView.ContentProperty, stackLayout8);
			sfExpander3.SetValue(SfExpander.ContentProperty, frame5);
			stackLayout24.Children.Add(sfExpander3);
			sfExpander4.SetValue(SfExpander.AnimationDurationProperty, 250.0);
			sfExpander4.SetValue(SfExpander.AnimationEasingProperty, 0);
			dynamicResourceExtension22.Key = "NavigationBarBackgroundColor";
			IMarkupExtension<DynamicResource> markupExtension73 = dynamicResourceExtension22;
			XamlServiceProvider xamlServiceProvider73 = new XamlServiceProvider();
			Type typeFromHandle145 = typeof(IProvideValueTarget);
			object[] array73 = new object[0 + 5];
			array73[0] = sfExpander4;
			array73[1] = stackLayout24;
			array73[2] = scrollView;
			array73[3] = grid25;
			array73[4] = this;
			object obj122;
			xamlServiceProvider73.Add(typeFromHandle145, obj122 = new SimpleValueTargetProvider(array73, SfExpander.HeaderBackgroundColorProperty, nameScope));
			xamlServiceProvider73.Add(typeof(IReferenceProvider), obj122);
			Type typeFromHandle146 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver73 = new XmlNamespaceResolver();
			xmlNamespaceResolver73.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver73.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver73.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver73.Add("dash", "clr-namespace:CarScannerXamarinForms.Dashboard");
			xmlNamespaceResolver73.Add("expander", "clr-namespace:Syncfusion.XForms.Expander;assembly=Syncfusion.Expander.XForms");
			xmlNamespaceResolver73.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver73.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver73.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver73.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider73.Add(typeFromHandle146, new XamlTypeResolver(xmlNamespaceResolver73, typeof(DashboardItemEditor).GetTypeInfo().Assembly));
			xamlServiceProvider73.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(642, 25)));
			DynamicResource dynamicResource22 = markupExtension73.ProvideValue(xamlServiceProvider73);
			sfExpander4.SetDynamicResource(SfExpander.HeaderBackgroundColorProperty, dynamicResource22.Key);
			dynamicResourceExtension23.Key = "NavigationBarButtonColor";
			IMarkupExtension<DynamicResource> markupExtension74 = dynamicResourceExtension23;
			XamlServiceProvider xamlServiceProvider74 = new XamlServiceProvider();
			Type typeFromHandle147 = typeof(IProvideValueTarget);
			object[] array74 = new object[0 + 5];
			array74[0] = sfExpander4;
			array74[1] = stackLayout24;
			array74[2] = scrollView;
			array74[3] = grid25;
			array74[4] = this;
			object obj123;
			xamlServiceProvider74.Add(typeFromHandle147, obj123 = new SimpleValueTargetProvider(array74, SfExpander.IconColorProperty, nameScope));
			xamlServiceProvider74.Add(typeof(IReferenceProvider), obj123);
			Type typeFromHandle148 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver74 = new XmlNamespaceResolver();
			xmlNamespaceResolver74.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver74.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver74.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver74.Add("dash", "clr-namespace:CarScannerXamarinForms.Dashboard");
			xmlNamespaceResolver74.Add("expander", "clr-namespace:Syncfusion.XForms.Expander;assembly=Syncfusion.Expander.XForms");
			xmlNamespaceResolver74.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver74.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver74.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver74.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider74.Add(typeFromHandle148, new XamlTypeResolver(xmlNamespaceResolver74, typeof(DashboardItemEditor).GetTypeInfo().Assembly));
			xamlServiceProvider74.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(643, 25)));
			DynamicResource dynamicResource23 = markupExtension74.ProvideValue(xamlServiceProvider74);
			sfExpander4.SetDynamicResource(SfExpander.IconColorProperty, dynamicResource23.Key);
			sfExpander4.SetValue(SfExpander.IsExpandedProperty, false);
			label26.SetValue(View.MarginProperty, new Thickness(5.0));
			label26.SetValue(Label.FontAttributesProperty, new FontAttributesConverter().ConvertFromInvariantString("Bold"));
			translate43.Text = "DashItem_GaugeOptions";
			IMarkupExtension markupExtension75 = translate43;
			XamlServiceProvider xamlServiceProvider75 = new XamlServiceProvider();
			Type typeFromHandle149 = typeof(IProvideValueTarget);
			object[] array75 = new object[0 + 7];
			array75[0] = label26;
			array75[1] = grid12;
			array75[2] = sfExpander4;
			array75[3] = stackLayout24;
			array75[4] = scrollView;
			array75[5] = grid25;
			array75[6] = this;
			object obj124;
			xamlServiceProvider75.Add(typeFromHandle149, obj124 = new SimpleValueTargetProvider(array75, Label.TextProperty, nameScope));
			xamlServiceProvider75.Add(typeof(IReferenceProvider), obj124);
			Type typeFromHandle150 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver75 = new XmlNamespaceResolver();
			xmlNamespaceResolver75.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver75.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver75.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver75.Add("dash", "clr-namespace:CarScannerXamarinForms.Dashboard");
			xmlNamespaceResolver75.Add("expander", "clr-namespace:Syncfusion.XForms.Expander;assembly=Syncfusion.Expander.XForms");
			xmlNamespaceResolver75.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver75.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver75.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver75.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider75.Add(typeFromHandle150, new XamlTypeResolver(xmlNamespaceResolver75, typeof(DashboardItemEditor).GetTypeInfo().Assembly));
			xamlServiceProvider75.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(650, 37)));
			object obj125 = markupExtension75.ProvideValue(xamlServiceProvider75);
			label26.Text = obj125;
			dynamicResourceExtension24.Key = "NavigationBarTextColor";
			IMarkupExtension<DynamicResource> markupExtension76 = dynamicResourceExtension24;
			XamlServiceProvider xamlServiceProvider76 = new XamlServiceProvider();
			Type typeFromHandle151 = typeof(IProvideValueTarget);
			object[] array76 = new object[0 + 7];
			array76[0] = label26;
			array76[1] = grid12;
			array76[2] = sfExpander4;
			array76[3] = stackLayout24;
			array76[4] = scrollView;
			array76[5] = grid25;
			array76[6] = this;
			object obj126;
			xamlServiceProvider76.Add(typeFromHandle151, obj126 = new SimpleValueTargetProvider(array76, Label.TextColorProperty, nameScope));
			xamlServiceProvider76.Add(typeof(IReferenceProvider), obj126);
			Type typeFromHandle152 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver76 = new XmlNamespaceResolver();
			xmlNamespaceResolver76.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver76.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver76.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver76.Add("dash", "clr-namespace:CarScannerXamarinForms.Dashboard");
			xmlNamespaceResolver76.Add("expander", "clr-namespace:Syncfusion.XForms.Expander;assembly=Syncfusion.Expander.XForms");
			xmlNamespaceResolver76.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver76.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver76.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver76.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider76.Add(typeFromHandle152, new XamlTypeResolver(xmlNamespaceResolver76, typeof(DashboardItemEditor).GetTypeInfo().Assembly));
			xamlServiceProvider76.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(651, 37)));
			DynamicResource dynamicResource24 = markupExtension76.ProvideValue(xamlServiceProvider76);
			label26.SetDynamicResource(Label.TextColorProperty, dynamicResource24.Key);
			label26.SetValue(View.VerticalOptionsProperty, LayoutOptions.Center);
			label26.SetValue(Label.VerticalTextAlignmentProperty, new TextAlignmentConverter().ConvertFromInvariantString("Center"));
			grid12.Children.Add(label26);
			sfExpander4.SetValue(SfExpander.HeaderProperty, grid12);
			stackLayout11.SetValue(StackLayout.OrientationProperty, 0);
			label27.SetValue(Label.TextProperty, "Gauge and pointer width:");
			stackLayout11.Children.Add(label27);
			numericEntryV7.Completed += this.CircularGaugeWidthEntry_Completed;
			bindingExtension51.Mode = 1;
			bindingExtension51.Path = "CiruclarGaugeWidth";
			bindingExtension51.TypedBinding = new TypedBinding<DashboardItem, double>(delegate(DashboardItem A_0)
			{
				if (A_0 != null)
				{
					return new ValueTuple<double, bool>(A_0.CiruclarGaugeWidth, true);
				}
				return default(ValueTuple<double, bool>);
			}, delegate(DashboardItem A_0, double A_1)
			{
				if (A_0 != null)
				{
					A_0.CiruclarGaugeWidth = A_1;
					return;
				}
			}, new Tuple<Func<DashboardItem, object>, string>[]
			{
				new Tuple<Func<DashboardItem, object>, string>((DashboardItem A_0) => A_0, "CiruclarGaugeWidth")
			});
			BindingBase bindingBase51 = bindingExtension51.ProvideValue(null);
			numericEntryV7.SetBinding(NumericEntryV3.ValueProperty, bindingBase51);
			stackLayout11.Children.Add(numericEntryV7);
			translate44.Text = "ios_DashboardItemEditor_Minimum";
			IMarkupExtension markupExtension77 = translate44;
			XamlServiceProvider xamlServiceProvider77 = new XamlServiceProvider();
			Type typeFromHandle153 = typeof(IProvideValueTarget);
			object[] array77 = new object[0 + 8];
			array77[0] = label28;
			array77[1] = stackLayout11;
			array77[2] = frame6;
			array77[3] = sfExpander4;
			array77[4] = stackLayout24;
			array77[5] = scrollView;
			array77[6] = grid25;
			array77[7] = this;
			object obj127;
			xamlServiceProvider77.Add(typeFromHandle153, obj127 = new SimpleValueTargetProvider(array77, Label.TextProperty, nameScope));
			xamlServiceProvider77.Add(typeof(IReferenceProvider), obj127);
			Type typeFromHandle154 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver77 = new XmlNamespaceResolver();
			xmlNamespaceResolver77.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver77.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver77.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver77.Add("dash", "clr-namespace:CarScannerXamarinForms.Dashboard");
			xmlNamespaceResolver77.Add("expander", "clr-namespace:Syncfusion.XForms.Expander;assembly=Syncfusion.Expander.XForms");
			xmlNamespaceResolver77.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver77.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver77.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver77.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider77.Add(typeFromHandle154, new XamlTypeResolver(xmlNamespaceResolver77, typeof(DashboardItemEditor).GetTypeInfo().Assembly));
			xamlServiceProvider77.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(662, 44)));
			object obj128 = markupExtension77.ProvideValue(xamlServiceProvider77);
			label28.Text = obj128;
			stackLayout11.Children.Add(label28);
			bindingExtension52.Mode = 1;
			bindingExtension52.Path = "Minimum";
			bindingExtension52.TypedBinding = new TypedBinding<DashboardItem, double>(delegate(DashboardItem A_0)
			{
				if (A_0 != null)
				{
					return new ValueTuple<double, bool>(A_0.Minimum, true);
				}
				return default(ValueTuple<double, bool>);
			}, delegate(DashboardItem A_0, double A_1)
			{
				if (A_0 != null)
				{
					A_0.Minimum = A_1;
					return;
				}
			}, new Tuple<Func<DashboardItem, object>, string>[]
			{
				new Tuple<Func<DashboardItem, object>, string>((DashboardItem A_0) => A_0, "Minimum")
			});
			BindingBase bindingBase52 = bindingExtension52.ProvideValue(null);
			numericEntryV8.SetBinding(NumericEntryV3.ValueProperty, bindingBase52);
			stackLayout11.Children.Add(numericEntryV8);
			translate45.Text = "ios_DashboardItemEditor_Maximum";
			IMarkupExtension markupExtension78 = translate45;
			XamlServiceProvider xamlServiceProvider78 = new XamlServiceProvider();
			Type typeFromHandle155 = typeof(IProvideValueTarget);
			object[] array78 = new object[0 + 8];
			array78[0] = label29;
			array78[1] = stackLayout11;
			array78[2] = frame6;
			array78[3] = sfExpander4;
			array78[4] = stackLayout24;
			array78[5] = scrollView;
			array78[6] = grid25;
			array78[7] = this;
			object obj129;
			xamlServiceProvider78.Add(typeFromHandle155, obj129 = new SimpleValueTargetProvider(array78, Label.TextProperty, nameScope));
			xamlServiceProvider78.Add(typeof(IReferenceProvider), obj129);
			Type typeFromHandle156 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver78 = new XmlNamespaceResolver();
			xmlNamespaceResolver78.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver78.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver78.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver78.Add("dash", "clr-namespace:CarScannerXamarinForms.Dashboard");
			xmlNamespaceResolver78.Add("expander", "clr-namespace:Syncfusion.XForms.Expander;assembly=Syncfusion.Expander.XForms");
			xmlNamespaceResolver78.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver78.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver78.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver78.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider78.Add(typeFromHandle156, new XamlTypeResolver(xmlNamespaceResolver78, typeof(DashboardItemEditor).GetTypeInfo().Assembly));
			xamlServiceProvider78.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(664, 44)));
			object obj130 = markupExtension78.ProvideValue(xamlServiceProvider78);
			label29.Text = obj130;
			stackLayout11.Children.Add(label29);
			bindingExtension53.Mode = 1;
			bindingExtension53.Path = "Maximum";
			bindingExtension53.TypedBinding = new TypedBinding<DashboardItem, double>(delegate(DashboardItem A_0)
			{
				if (A_0 != null)
				{
					return new ValueTuple<double, bool>(A_0.Maximum, true);
				}
				return default(ValueTuple<double, bool>);
			}, delegate(DashboardItem A_0, double A_1)
			{
				if (A_0 != null)
				{
					A_0.Maximum = A_1;
					return;
				}
			}, new Tuple<Func<DashboardItem, object>, string>[]
			{
				new Tuple<Func<DashboardItem, object>, string>((DashboardItem A_0) => A_0, "Maximum")
			});
			BindingBase bindingBase53 = bindingExtension53.ProvideValue(null);
			numericEntryV9.SetBinding(NumericEntryV3.ValueProperty, bindingBase53);
			stackLayout11.Children.Add(numericEntryV9);
			dashboardItemColorBox15.SetValue(Element.ClassIdProperty, "GaugePointerColor");
			dashboardItemColorBox15.Tapped += this.ColorBox_Tapped;
			translate46.Text = "ios_DashboardItemEditor_GaugePointerColor";
			IMarkupExtension markupExtension79 = translate46;
			XamlServiceProvider xamlServiceProvider79 = new XamlServiceProvider();
			Type typeFromHandle157 = typeof(IProvideValueTarget);
			object[] array79 = new object[0 + 8];
			array79[0] = dashboardItemColorBox15;
			array79[1] = stackLayout11;
			array79[2] = frame6;
			array79[3] = sfExpander4;
			array79[4] = stackLayout24;
			array79[5] = scrollView;
			array79[6] = grid25;
			array79[7] = this;
			object obj131;
			xamlServiceProvider79.Add(typeFromHandle157, obj131 = new SimpleValueTargetProvider(array79, DashboardItemColorBox.TextProperty, nameScope));
			xamlServiceProvider79.Add(typeof(IReferenceProvider), obj131);
			Type typeFromHandle158 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver79 = new XmlNamespaceResolver();
			xmlNamespaceResolver79.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver79.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver79.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver79.Add("dash", "clr-namespace:CarScannerXamarinForms.Dashboard");
			xmlNamespaceResolver79.Add("expander", "clr-namespace:Syncfusion.XForms.Expander;assembly=Syncfusion.Expander.XForms");
			xmlNamespaceResolver79.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver79.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver79.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver79.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider79.Add(typeFromHandle158, new XamlTypeResolver(xmlNamespaceResolver79, typeof(DashboardItemEditor).GetTypeInfo().Assembly));
			xamlServiceProvider79.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(670, 41)));
			object obj132 = markupExtension79.ProvideValue(xamlServiceProvider79);
			dashboardItemColorBox15.Text = obj132;
			bindingExtension54.Mode = 2;
			bindingExtension54.Path = "GaugePointerColor";
			bindingExtension54.TypedBinding = new TypedBinding<DashboardItem, Color>(delegate(DashboardItem A_0)
			{
				if (A_0 != null)
				{
					return new ValueTuple<Color, bool>(A_0.GaugePointerColor, true);
				}
				return default(ValueTuple<Color, bool>);
			}, null, new Tuple<Func<DashboardItem, object>, string>[]
			{
				new Tuple<Func<DashboardItem, object>, string>((DashboardItem A_0) => A_0, "GaugePointerColor")
			});
			BindingBase bindingBase54 = bindingExtension54.ProvideValue(null);
			dashboardItemColorBox15.SetBinding(DashboardItemColorBox.ColorProperty, bindingBase54);
			stackLayout11.Children.Add(dashboardItemColorBox15);
			dashboardItemColorBox16.SetValue(Element.ClassIdProperty, "GaugeRimColor");
			dashboardItemColorBox16.Tapped += this.ColorBox_Tapped;
			translate47.Text = "ios_DashboardItemEditor_GaugeRimColor";
			IMarkupExtension markupExtension80 = translate47;
			XamlServiceProvider xamlServiceProvider80 = new XamlServiceProvider();
			Type typeFromHandle159 = typeof(IProvideValueTarget);
			object[] array80 = new object[0 + 8];
			array80[0] = dashboardItemColorBox16;
			array80[1] = stackLayout11;
			array80[2] = frame6;
			array80[3] = sfExpander4;
			array80[4] = stackLayout24;
			array80[5] = scrollView;
			array80[6] = grid25;
			array80[7] = this;
			object obj133;
			xamlServiceProvider80.Add(typeFromHandle159, obj133 = new SimpleValueTargetProvider(array80, DashboardItemColorBox.TextProperty, nameScope));
			xamlServiceProvider80.Add(typeof(IReferenceProvider), obj133);
			Type typeFromHandle160 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver80 = new XmlNamespaceResolver();
			xmlNamespaceResolver80.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver80.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver80.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver80.Add("dash", "clr-namespace:CarScannerXamarinForms.Dashboard");
			xmlNamespaceResolver80.Add("expander", "clr-namespace:Syncfusion.XForms.Expander;assembly=Syncfusion.Expander.XForms");
			xmlNamespaceResolver80.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver80.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver80.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver80.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider80.Add(typeFromHandle160, new XamlTypeResolver(xmlNamespaceResolver80, typeof(DashboardItemEditor).GetTypeInfo().Assembly));
			xamlServiceProvider80.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(676, 41)));
			object obj134 = markupExtension80.ProvideValue(xamlServiceProvider80);
			dashboardItemColorBox16.Text = obj134;
			bindingExtension55.Mode = 2;
			bindingExtension55.Path = "GaugeRimColor";
			bindingExtension55.TypedBinding = new TypedBinding<DashboardItem, Color>(delegate(DashboardItem A_0)
			{
				if (A_0 != null)
				{
					return new ValueTuple<Color, bool>(A_0.GaugeRimColor, true);
				}
				return default(ValueTuple<Color, bool>);
			}, null, new Tuple<Func<DashboardItem, object>, string>[]
			{
				new Tuple<Func<DashboardItem, object>, string>((DashboardItem A_0) => A_0, "GaugeRimColor")
			});
			BindingBase bindingBase55 = bindingExtension55.ProvideValue(null);
			dashboardItemColorBox16.SetBinding(DashboardItemColorBox.ColorProperty, bindingBase55);
			stackLayout11.Children.Add(dashboardItemColorBox16);
			bindingExtension56.Mode = 1;
			bindingExtension56.Path = "GaugeShowBlueLine";
			bindingExtension56.TypedBinding = new TypedBinding<DashboardItem, bool>(delegate(DashboardItem A_0)
			{
				if (A_0 != null)
				{
					return new ValueTuple<bool, bool>(A_0.GaugeShowBlueLine, true);
				}
				return default(ValueTuple<bool, bool>);
			}, delegate(DashboardItem A_0, bool A_1)
			{
				if (A_0 != null)
				{
					A_0.GaugeShowBlueLine = A_1;
					return;
				}
			}, new Tuple<Func<DashboardItem, object>, string>[]
			{
				new Tuple<Func<DashboardItem, object>, string>((DashboardItem A_0) => A_0, "GaugeShowBlueLine")
			});
			BindingBase bindingBase56 = bindingExtension56.ProvideValue(null);
			labelSwitch6.SetBinding(LabelSwitch.IsToggledProperty, bindingBase56);
			translate48.Text = "ios_DashboardItemEditor_GaugeShowBlueLine";
			IMarkupExtension markupExtension81 = translate48;
			XamlServiceProvider xamlServiceProvider81 = new XamlServiceProvider();
			Type typeFromHandle161 = typeof(IProvideValueTarget);
			object[] array81 = new object[0 + 8];
			array81[0] = labelSwitch6;
			array81[1] = stackLayout11;
			array81[2] = frame6;
			array81[3] = sfExpander4;
			array81[4] = stackLayout24;
			array81[5] = scrollView;
			array81[6] = grid25;
			array81[7] = this;
			object obj135;
			xamlServiceProvider81.Add(typeFromHandle161, obj135 = new SimpleValueTargetProvider(array81, LabelSwitch.TextProperty, nameScope));
			xamlServiceProvider81.Add(typeof(IReferenceProvider), obj135);
			Type typeFromHandle162 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver81 = new XmlNamespaceResolver();
			xmlNamespaceResolver81.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver81.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver81.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver81.Add("dash", "clr-namespace:CarScannerXamarinForms.Dashboard");
			xmlNamespaceResolver81.Add("expander", "clr-namespace:Syncfusion.XForms.Expander;assembly=Syncfusion.Expander.XForms");
			xmlNamespaceResolver81.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver81.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver81.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver81.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider81.Add(typeFromHandle162, new XamlTypeResolver(xmlNamespaceResolver81, typeof(DashboardItemEditor).GetTypeInfo().Assembly));
			xamlServiceProvider81.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(696, 109)));
			object obj136 = markupExtension81.ProvideValue(xamlServiceProvider81);
			labelSwitch6.Text = obj136;
			stackLayout11.Children.Add(labelSwitch6);
			bindingExtension57.Mode = 2;
			bindingExtension57.Path = "GaugeShowBlueLine";
			bindingExtension57.TypedBinding = new TypedBinding<DashboardItem, bool>(delegate(DashboardItem A_0)
			{
				if (A_0 != null)
				{
					return new ValueTuple<bool, bool>(A_0.GaugeShowBlueLine, true);
				}
				return default(ValueTuple<bool, bool>);
			}, null, new Tuple<Func<DashboardItem, object>, string>[]
			{
				new Tuple<Func<DashboardItem, object>, string>((DashboardItem A_0) => A_0, "GaugeShowBlueLine")
			});
			BindingBase bindingBase57 = bindingExtension57.ProvideValue(null);
			stackLayout9.SetBinding(VisualElement.IsVisibleProperty, bindingBase57);
			stackLayout9.SetValue(StackLayout.OrientationProperty, 0);
			translate49.Text = "ios_DashboardItemEditor_GaugeBlueLineStart";
			IMarkupExtension markupExtension82 = translate49;
			XamlServiceProvider xamlServiceProvider82 = new XamlServiceProvider();
			Type typeFromHandle163 = typeof(IProvideValueTarget);
			object[] array82 = new object[0 + 9];
			array82[0] = label30;
			array82[1] = stackLayout9;
			array82[2] = stackLayout11;
			array82[3] = frame6;
			array82[4] = sfExpander4;
			array82[5] = stackLayout24;
			array82[6] = scrollView;
			array82[7] = grid25;
			array82[8] = this;
			object obj137;
			xamlServiceProvider82.Add(typeFromHandle163, obj137 = new SimpleValueTargetProvider(array82, Label.TextProperty, nameScope));
			xamlServiceProvider82.Add(typeof(IReferenceProvider), obj137);
			Type typeFromHandle164 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver82 = new XmlNamespaceResolver();
			xmlNamespaceResolver82.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver82.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver82.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver82.Add("dash", "clr-namespace:CarScannerXamarinForms.Dashboard");
			xmlNamespaceResolver82.Add("expander", "clr-namespace:Syncfusion.XForms.Expander;assembly=Syncfusion.Expander.XForms");
			xmlNamespaceResolver82.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver82.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver82.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver82.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider82.Add(typeFromHandle164, new XamlTypeResolver(xmlNamespaceResolver82, typeof(DashboardItemEditor).GetTypeInfo().Assembly));
			xamlServiceProvider82.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(698, 48)));
			object obj138 = markupExtension82.ProvideValue(xamlServiceProvider82);
			label30.Text = obj138;
			stackLayout9.Children.Add(label30);
			bindingExtension58.Mode = 1;
			bindingExtension58.Path = "GaugeBlueLineStart";
			bindingExtension58.TypedBinding = new TypedBinding<DashboardItem, double>(delegate(DashboardItem A_0)
			{
				if (A_0 != null)
				{
					return new ValueTuple<double, bool>(A_0.GaugeBlueLineStart, true);
				}
				return default(ValueTuple<double, bool>);
			}, delegate(DashboardItem A_0, double A_1)
			{
				if (A_0 != null)
				{
					A_0.GaugeBlueLineStart = A_1;
					return;
				}
			}, new Tuple<Func<DashboardItem, object>, string>[]
			{
				new Tuple<Func<DashboardItem, object>, string>((DashboardItem A_0) => A_0, "GaugeBlueLineStart")
			});
			BindingBase bindingBase58 = bindingExtension58.ProvideValue(null);
			numericEntryV10.SetBinding(NumericEntryV3.ValueProperty, bindingBase58);
			stackLayout9.Children.Add(numericEntryV10);
			translate50.Text = "ios_DashboardItemEditor_GaugeBlueLineFinish";
			IMarkupExtension markupExtension83 = translate50;
			XamlServiceProvider xamlServiceProvider83 = new XamlServiceProvider();
			Type typeFromHandle165 = typeof(IProvideValueTarget);
			object[] array83 = new object[0 + 9];
			array83[0] = label31;
			array83[1] = stackLayout9;
			array83[2] = stackLayout11;
			array83[3] = frame6;
			array83[4] = sfExpander4;
			array83[5] = stackLayout24;
			array83[6] = scrollView;
			array83[7] = grid25;
			array83[8] = this;
			object obj139;
			xamlServiceProvider83.Add(typeFromHandle165, obj139 = new SimpleValueTargetProvider(array83, Label.TextProperty, nameScope));
			xamlServiceProvider83.Add(typeof(IReferenceProvider), obj139);
			Type typeFromHandle166 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver83 = new XmlNamespaceResolver();
			xmlNamespaceResolver83.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver83.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver83.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver83.Add("dash", "clr-namespace:CarScannerXamarinForms.Dashboard");
			xmlNamespaceResolver83.Add("expander", "clr-namespace:Syncfusion.XForms.Expander;assembly=Syncfusion.Expander.XForms");
			xmlNamespaceResolver83.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver83.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver83.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver83.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider83.Add(typeFromHandle166, new XamlTypeResolver(xmlNamespaceResolver83, typeof(DashboardItemEditor).GetTypeInfo().Assembly));
			xamlServiceProvider83.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(701, 48)));
			object obj140 = markupExtension83.ProvideValue(xamlServiceProvider83);
			label31.Text = obj140;
			stackLayout9.Children.Add(label31);
			bindingExtension59.Mode = 1;
			bindingExtension59.Path = "GaugeBlueLineFinish";
			bindingExtension59.TypedBinding = new TypedBinding<DashboardItem, double>(delegate(DashboardItem A_0)
			{
				if (A_0 != null)
				{
					return new ValueTuple<double, bool>(A_0.GaugeBlueLineFinish, true);
				}
				return default(ValueTuple<double, bool>);
			}, delegate(DashboardItem A_0, double A_1)
			{
				if (A_0 != null)
				{
					A_0.GaugeBlueLineFinish = A_1;
					return;
				}
			}, new Tuple<Func<DashboardItem, object>, string>[]
			{
				new Tuple<Func<DashboardItem, object>, string>((DashboardItem A_0) => A_0, "GaugeBlueLineFinish")
			});
			BindingBase bindingBase59 = bindingExtension59.ProvideValue(null);
			numericEntryV11.SetBinding(NumericEntryV3.ValueProperty, bindingBase59);
			stackLayout9.Children.Add(numericEntryV11);
			dashboardItemColorBox17.SetValue(Element.ClassIdProperty, "GaugeBlueLineColor");
			dashboardItemColorBox17.Tapped += this.ColorBox_Tapped;
			translate51.Text = "ios_DashboardItemEditor_GaugeBlueLineColor";
			IMarkupExtension markupExtension84 = translate51;
			XamlServiceProvider xamlServiceProvider84 = new XamlServiceProvider();
			Type typeFromHandle167 = typeof(IProvideValueTarget);
			object[] array84 = new object[0 + 9];
			array84[0] = dashboardItemColorBox17;
			array84[1] = stackLayout9;
			array84[2] = stackLayout11;
			array84[3] = frame6;
			array84[4] = sfExpander4;
			array84[5] = stackLayout24;
			array84[6] = scrollView;
			array84[7] = grid25;
			array84[8] = this;
			object obj141;
			xamlServiceProvider84.Add(typeFromHandle167, obj141 = new SimpleValueTargetProvider(array84, DashboardItemColorBox.TextProperty, nameScope));
			xamlServiceProvider84.Add(typeof(IReferenceProvider), obj141);
			Type typeFromHandle168 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver84 = new XmlNamespaceResolver();
			xmlNamespaceResolver84.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver84.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver84.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver84.Add("dash", "clr-namespace:CarScannerXamarinForms.Dashboard");
			xmlNamespaceResolver84.Add("expander", "clr-namespace:Syncfusion.XForms.Expander;assembly=Syncfusion.Expander.XForms");
			xmlNamespaceResolver84.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver84.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver84.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver84.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider84.Add(typeFromHandle168, new XamlTypeResolver(xmlNamespaceResolver84, typeof(DashboardItemEditor).GetTypeInfo().Assembly));
			xamlServiceProvider84.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(706, 45)));
			object obj142 = markupExtension84.ProvideValue(xamlServiceProvider84);
			dashboardItemColorBox17.Text = obj142;
			bindingExtension60.Mode = 2;
			bindingExtension60.Path = "GaugeBlueLineColor";
			bindingExtension60.TypedBinding = new TypedBinding<DashboardItem, Color>(delegate(DashboardItem A_0)
			{
				if (A_0 != null)
				{
					return new ValueTuple<Color, bool>(A_0.GaugeBlueLineColor, true);
				}
				return default(ValueTuple<Color, bool>);
			}, null, new Tuple<Func<DashboardItem, object>, string>[]
			{
				new Tuple<Func<DashboardItem, object>, string>((DashboardItem A_0) => A_0, "GaugeBlueLineColor")
			});
			BindingBase bindingBase60 = bindingExtension60.ProvideValue(null);
			dashboardItemColorBox17.SetBinding(DashboardItemColorBox.ColorProperty, bindingBase60);
			stackLayout9.Children.Add(dashboardItemColorBox17);
			stackLayout11.Children.Add(stackLayout9);
			bindingExtension61.Mode = 1;
			bindingExtension61.Path = "GaugeShowRedLine";
			bindingExtension61.TypedBinding = new TypedBinding<DashboardItem, bool>(delegate(DashboardItem A_0)
			{
				if (A_0 != null)
				{
					return new ValueTuple<bool, bool>(A_0.GaugeShowRedLine, true);
				}
				return default(ValueTuple<bool, bool>);
			}, delegate(DashboardItem A_0, bool A_1)
			{
				if (A_0 != null)
				{
					A_0.GaugeShowRedLine = A_1;
					return;
				}
			}, new Tuple<Func<DashboardItem, object>, string>[]
			{
				new Tuple<Func<DashboardItem, object>, string>((DashboardItem A_0) => A_0, "GaugeShowRedLine")
			});
			BindingBase bindingBase61 = bindingExtension61.ProvideValue(null);
			labelSwitch7.SetBinding(LabelSwitch.IsToggledProperty, bindingBase61);
			translate52.Text = "ios_DashboardItemEditor_GaugeShowRedLine";
			IMarkupExtension markupExtension85 = translate52;
			XamlServiceProvider xamlServiceProvider85 = new XamlServiceProvider();
			Type typeFromHandle169 = typeof(IProvideValueTarget);
			object[] array85 = new object[0 + 8];
			array85[0] = labelSwitch7;
			array85[1] = stackLayout11;
			array85[2] = frame6;
			array85[3] = sfExpander4;
			array85[4] = stackLayout24;
			array85[5] = scrollView;
			array85[6] = grid25;
			array85[7] = this;
			object obj143;
			xamlServiceProvider85.Add(typeFromHandle169, obj143 = new SimpleValueTargetProvider(array85, LabelSwitch.TextProperty, nameScope));
			xamlServiceProvider85.Add(typeof(IReferenceProvider), obj143);
			Type typeFromHandle170 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver85 = new XmlNamespaceResolver();
			xmlNamespaceResolver85.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver85.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver85.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver85.Add("dash", "clr-namespace:CarScannerXamarinForms.Dashboard");
			xmlNamespaceResolver85.Add("expander", "clr-namespace:Syncfusion.XForms.Expander;assembly=Syncfusion.Expander.XForms");
			xmlNamespaceResolver85.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver85.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver85.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver85.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider85.Add(typeFromHandle170, new XamlTypeResolver(xmlNamespaceResolver85, typeof(DashboardItemEditor).GetTypeInfo().Assembly));
			xamlServiceProvider85.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(711, 108)));
			object obj144 = markupExtension85.ProvideValue(xamlServiceProvider85);
			labelSwitch7.Text = obj144;
			stackLayout11.Children.Add(labelSwitch7);
			bindingExtension62.Mode = 2;
			bindingExtension62.Path = "GaugeShowRedLine";
			bindingExtension62.TypedBinding = new TypedBinding<DashboardItem, bool>(delegate(DashboardItem A_0)
			{
				if (A_0 != null)
				{
					return new ValueTuple<bool, bool>(A_0.GaugeShowRedLine, true);
				}
				return default(ValueTuple<bool, bool>);
			}, null, new Tuple<Func<DashboardItem, object>, string>[]
			{
				new Tuple<Func<DashboardItem, object>, string>((DashboardItem A_0) => A_0, "GaugeShowRedLine")
			});
			BindingBase bindingBase62 = bindingExtension62.ProvideValue(null);
			stackLayout10.SetBinding(VisualElement.IsVisibleProperty, bindingBase62);
			stackLayout10.SetValue(StackLayout.OrientationProperty, 0);
			translate53.Text = "ios_DashboardItemEditor_GaugeRedLineStart";
			IMarkupExtension markupExtension86 = translate53;
			XamlServiceProvider xamlServiceProvider86 = new XamlServiceProvider();
			Type typeFromHandle171 = typeof(IProvideValueTarget);
			object[] array86 = new object[0 + 9];
			array86[0] = label32;
			array86[1] = stackLayout10;
			array86[2] = stackLayout11;
			array86[3] = frame6;
			array86[4] = sfExpander4;
			array86[5] = stackLayout24;
			array86[6] = scrollView;
			array86[7] = grid25;
			array86[8] = this;
			object obj145;
			xamlServiceProvider86.Add(typeFromHandle171, obj145 = new SimpleValueTargetProvider(array86, Label.TextProperty, nameScope));
			xamlServiceProvider86.Add(typeof(IReferenceProvider), obj145);
			Type typeFromHandle172 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver86 = new XmlNamespaceResolver();
			xmlNamespaceResolver86.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver86.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver86.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver86.Add("dash", "clr-namespace:CarScannerXamarinForms.Dashboard");
			xmlNamespaceResolver86.Add("expander", "clr-namespace:Syncfusion.XForms.Expander;assembly=Syncfusion.Expander.XForms");
			xmlNamespaceResolver86.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver86.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver86.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver86.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider86.Add(typeFromHandle172, new XamlTypeResolver(xmlNamespaceResolver86, typeof(DashboardItemEditor).GetTypeInfo().Assembly));
			xamlServiceProvider86.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(713, 48)));
			object obj146 = markupExtension86.ProvideValue(xamlServiceProvider86);
			label32.Text = obj146;
			stackLayout10.Children.Add(label32);
			bindingExtension63.Mode = 1;
			bindingExtension63.Path = "GaugeRedLineStart";
			bindingExtension63.TypedBinding = new TypedBinding<DashboardItem, double>(delegate(DashboardItem A_0)
			{
				if (A_0 != null)
				{
					return new ValueTuple<double, bool>(A_0.GaugeRedLineStart, true);
				}
				return default(ValueTuple<double, bool>);
			}, delegate(DashboardItem A_0, double A_1)
			{
				if (A_0 != null)
				{
					A_0.GaugeRedLineStart = A_1;
					return;
				}
			}, new Tuple<Func<DashboardItem, object>, string>[]
			{
				new Tuple<Func<DashboardItem, object>, string>((DashboardItem A_0) => A_0, "GaugeRedLineStart")
			});
			BindingBase bindingBase63 = bindingExtension63.ProvideValue(null);
			numericEntryV12.SetBinding(NumericEntryV3.ValueProperty, bindingBase63);
			stackLayout10.Children.Add(numericEntryV12);
			translate54.Text = "ios_DashboardItemEditor_GaugeRedLineFinish";
			IMarkupExtension markupExtension87 = translate54;
			XamlServiceProvider xamlServiceProvider87 = new XamlServiceProvider();
			Type typeFromHandle173 = typeof(IProvideValueTarget);
			object[] array87 = new object[0 + 9];
			array87[0] = label33;
			array87[1] = stackLayout10;
			array87[2] = stackLayout11;
			array87[3] = frame6;
			array87[4] = sfExpander4;
			array87[5] = stackLayout24;
			array87[6] = scrollView;
			array87[7] = grid25;
			array87[8] = this;
			object obj147;
			xamlServiceProvider87.Add(typeFromHandle173, obj147 = new SimpleValueTargetProvider(array87, Label.TextProperty, nameScope));
			xamlServiceProvider87.Add(typeof(IReferenceProvider), obj147);
			Type typeFromHandle174 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver87 = new XmlNamespaceResolver();
			xmlNamespaceResolver87.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver87.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver87.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver87.Add("dash", "clr-namespace:CarScannerXamarinForms.Dashboard");
			xmlNamespaceResolver87.Add("expander", "clr-namespace:Syncfusion.XForms.Expander;assembly=Syncfusion.Expander.XForms");
			xmlNamespaceResolver87.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver87.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver87.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver87.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider87.Add(typeFromHandle174, new XamlTypeResolver(xmlNamespaceResolver87, typeof(DashboardItemEditor).GetTypeInfo().Assembly));
			xamlServiceProvider87.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(716, 48)));
			object obj148 = markupExtension87.ProvideValue(xamlServiceProvider87);
			label33.Text = obj148;
			stackLayout10.Children.Add(label33);
			bindingExtension64.Mode = 1;
			bindingExtension64.Path = "GaugeRedLineFinish";
			bindingExtension64.TypedBinding = new TypedBinding<DashboardItem, double>(delegate(DashboardItem A_0)
			{
				if (A_0 != null)
				{
					return new ValueTuple<double, bool>(A_0.GaugeRedLineFinish, true);
				}
				return default(ValueTuple<double, bool>);
			}, delegate(DashboardItem A_0, double A_1)
			{
				if (A_0 != null)
				{
					A_0.GaugeRedLineFinish = A_1;
					return;
				}
			}, new Tuple<Func<DashboardItem, object>, string>[]
			{
				new Tuple<Func<DashboardItem, object>, string>((DashboardItem A_0) => A_0, "GaugeRedLineFinish")
			});
			BindingBase bindingBase64 = bindingExtension64.ProvideValue(null);
			numericEntryV13.SetBinding(NumericEntryV3.ValueProperty, bindingBase64);
			stackLayout10.Children.Add(numericEntryV13);
			dashboardItemColorBox18.SetValue(Element.ClassIdProperty, "GaugeRedLineColor");
			dashboardItemColorBox18.Tapped += this.ColorBox_Tapped;
			translate55.Text = "ios_DashboardItemEditor_GaugeRedLineColor";
			IMarkupExtension markupExtension88 = translate55;
			XamlServiceProvider xamlServiceProvider88 = new XamlServiceProvider();
			Type typeFromHandle175 = typeof(IProvideValueTarget);
			object[] array88 = new object[0 + 9];
			array88[0] = dashboardItemColorBox18;
			array88[1] = stackLayout10;
			array88[2] = stackLayout11;
			array88[3] = frame6;
			array88[4] = sfExpander4;
			array88[5] = stackLayout24;
			array88[6] = scrollView;
			array88[7] = grid25;
			array88[8] = this;
			object obj149;
			xamlServiceProvider88.Add(typeFromHandle175, obj149 = new SimpleValueTargetProvider(array88, DashboardItemColorBox.TextProperty, nameScope));
			xamlServiceProvider88.Add(typeof(IReferenceProvider), obj149);
			Type typeFromHandle176 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver88 = new XmlNamespaceResolver();
			xmlNamespaceResolver88.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver88.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver88.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver88.Add("dash", "clr-namespace:CarScannerXamarinForms.Dashboard");
			xmlNamespaceResolver88.Add("expander", "clr-namespace:Syncfusion.XForms.Expander;assembly=Syncfusion.Expander.XForms");
			xmlNamespaceResolver88.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver88.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver88.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver88.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider88.Add(typeFromHandle176, new XamlTypeResolver(xmlNamespaceResolver88, typeof(DashboardItemEditor).GetTypeInfo().Assembly));
			xamlServiceProvider88.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(721, 45)));
			object obj150 = markupExtension88.ProvideValue(xamlServiceProvider88);
			dashboardItemColorBox18.Text = obj150;
			bindingExtension65.Mode = 2;
			bindingExtension65.Path = "GaugeRedLineColor";
			bindingExtension65.TypedBinding = new TypedBinding<DashboardItem, Color>(delegate(DashboardItem A_0)
			{
				if (A_0 != null)
				{
					return new ValueTuple<Color, bool>(A_0.GaugeRedLineColor, true);
				}
				return default(ValueTuple<Color, bool>);
			}, null, new Tuple<Func<DashboardItem, object>, string>[]
			{
				new Tuple<Func<DashboardItem, object>, string>((DashboardItem A_0) => A_0, "GaugeRedLineColor")
			});
			BindingBase bindingBase65 = bindingExtension65.ProvideValue(null);
			dashboardItemColorBox18.SetBinding(DashboardItemColorBox.ColorProperty, bindingBase65);
			stackLayout10.Children.Add(dashboardItemColorBox18);
			stackLayout11.Children.Add(stackLayout10);
			frame6.SetValue(ContentView.ContentProperty, stackLayout11);
			sfExpander4.SetValue(SfExpander.ContentProperty, frame6);
			stackLayout24.Children.Add(sfExpander4);
			sfExpander5.SetValue(SfExpander.AnimationDurationProperty, 250.0);
			sfExpander5.SetValue(SfExpander.AnimationEasingProperty, 0);
			dynamicResourceExtension25.Key = "NavigationBarBackgroundColor";
			IMarkupExtension<DynamicResource> markupExtension89 = dynamicResourceExtension25;
			XamlServiceProvider xamlServiceProvider89 = new XamlServiceProvider();
			Type typeFromHandle177 = typeof(IProvideValueTarget);
			object[] array89 = new object[0 + 5];
			array89[0] = sfExpander5;
			array89[1] = stackLayout24;
			array89[2] = scrollView;
			array89[3] = grid25;
			array89[4] = this;
			object obj151;
			xamlServiceProvider89.Add(typeFromHandle177, obj151 = new SimpleValueTargetProvider(array89, SfExpander.HeaderBackgroundColorProperty, nameScope));
			xamlServiceProvider89.Add(typeof(IReferenceProvider), obj151);
			Type typeFromHandle178 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver89 = new XmlNamespaceResolver();
			xmlNamespaceResolver89.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver89.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver89.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver89.Add("dash", "clr-namespace:CarScannerXamarinForms.Dashboard");
			xmlNamespaceResolver89.Add("expander", "clr-namespace:Syncfusion.XForms.Expander;assembly=Syncfusion.Expander.XForms");
			xmlNamespaceResolver89.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver89.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver89.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver89.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider89.Add(typeFromHandle178, new XamlTypeResolver(xmlNamespaceResolver89, typeof(DashboardItemEditor).GetTypeInfo().Assembly));
			xamlServiceProvider89.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(740, 25)));
			DynamicResource dynamicResource25 = markupExtension89.ProvideValue(xamlServiceProvider89);
			sfExpander5.SetDynamicResource(SfExpander.HeaderBackgroundColorProperty, dynamicResource25.Key);
			dynamicResourceExtension26.Key = "NavigationBarButtonColor";
			IMarkupExtension<DynamicResource> markupExtension90 = dynamicResourceExtension26;
			XamlServiceProvider xamlServiceProvider90 = new XamlServiceProvider();
			Type typeFromHandle179 = typeof(IProvideValueTarget);
			object[] array90 = new object[0 + 5];
			array90[0] = sfExpander5;
			array90[1] = stackLayout24;
			array90[2] = scrollView;
			array90[3] = grid25;
			array90[4] = this;
			object obj152;
			xamlServiceProvider90.Add(typeFromHandle179, obj152 = new SimpleValueTargetProvider(array90, SfExpander.IconColorProperty, nameScope));
			xamlServiceProvider90.Add(typeof(IReferenceProvider), obj152);
			Type typeFromHandle180 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver90 = new XmlNamespaceResolver();
			xmlNamespaceResolver90.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver90.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver90.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver90.Add("dash", "clr-namespace:CarScannerXamarinForms.Dashboard");
			xmlNamespaceResolver90.Add("expander", "clr-namespace:Syncfusion.XForms.Expander;assembly=Syncfusion.Expander.XForms");
			xmlNamespaceResolver90.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver90.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver90.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver90.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider90.Add(typeFromHandle180, new XamlTypeResolver(xmlNamespaceResolver90, typeof(DashboardItemEditor).GetTypeInfo().Assembly));
			xamlServiceProvider90.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(741, 25)));
			DynamicResource dynamicResource26 = markupExtension90.ProvideValue(xamlServiceProvider90);
			sfExpander5.SetDynamicResource(SfExpander.IconColorProperty, dynamicResource26.Key);
			sfExpander5.SetValue(SfExpander.IsExpandedProperty, false);
			label34.SetValue(View.MarginProperty, new Thickness(5.0));
			label34.SetValue(Label.FontAttributesProperty, new FontAttributesConverter().ConvertFromInvariantString("Bold"));
			translate56.Text = "DashItem_GaugeOptions";
			IMarkupExtension markupExtension91 = translate56;
			XamlServiceProvider xamlServiceProvider91 = new XamlServiceProvider();
			Type typeFromHandle181 = typeof(IProvideValueTarget);
			object[] array91 = new object[0 + 7];
			array91[0] = label34;
			array91[1] = grid13;
			array91[2] = sfExpander5;
			array91[3] = stackLayout24;
			array91[4] = scrollView;
			array91[5] = grid25;
			array91[6] = this;
			object obj153;
			xamlServiceProvider91.Add(typeFromHandle181, obj153 = new SimpleValueTargetProvider(array91, Label.TextProperty, nameScope));
			xamlServiceProvider91.Add(typeof(IReferenceProvider), obj153);
			Type typeFromHandle182 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver91 = new XmlNamespaceResolver();
			xmlNamespaceResolver91.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver91.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver91.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver91.Add("dash", "clr-namespace:CarScannerXamarinForms.Dashboard");
			xmlNamespaceResolver91.Add("expander", "clr-namespace:Syncfusion.XForms.Expander;assembly=Syncfusion.Expander.XForms");
			xmlNamespaceResolver91.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver91.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver91.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver91.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider91.Add(typeFromHandle182, new XamlTypeResolver(xmlNamespaceResolver91, typeof(DashboardItemEditor).GetTypeInfo().Assembly));
			xamlServiceProvider91.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(748, 37)));
			object obj154 = markupExtension91.ProvideValue(xamlServiceProvider91);
			label34.Text = obj154;
			dynamicResourceExtension27.Key = "NavigationBarTextColor";
			IMarkupExtension<DynamicResource> markupExtension92 = dynamicResourceExtension27;
			XamlServiceProvider xamlServiceProvider92 = new XamlServiceProvider();
			Type typeFromHandle183 = typeof(IProvideValueTarget);
			object[] array92 = new object[0 + 7];
			array92[0] = label34;
			array92[1] = grid13;
			array92[2] = sfExpander5;
			array92[3] = stackLayout24;
			array92[4] = scrollView;
			array92[5] = grid25;
			array92[6] = this;
			object obj155;
			xamlServiceProvider92.Add(typeFromHandle183, obj155 = new SimpleValueTargetProvider(array92, Label.TextColorProperty, nameScope));
			xamlServiceProvider92.Add(typeof(IReferenceProvider), obj155);
			Type typeFromHandle184 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver92 = new XmlNamespaceResolver();
			xmlNamespaceResolver92.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver92.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver92.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver92.Add("dash", "clr-namespace:CarScannerXamarinForms.Dashboard");
			xmlNamespaceResolver92.Add("expander", "clr-namespace:Syncfusion.XForms.Expander;assembly=Syncfusion.Expander.XForms");
			xmlNamespaceResolver92.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver92.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver92.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver92.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider92.Add(typeFromHandle184, new XamlTypeResolver(xmlNamespaceResolver92, typeof(DashboardItemEditor).GetTypeInfo().Assembly));
			xamlServiceProvider92.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(749, 37)));
			DynamicResource dynamicResource27 = markupExtension92.ProvideValue(xamlServiceProvider92);
			label34.SetDynamicResource(Label.TextColorProperty, dynamicResource27.Key);
			label34.SetValue(View.VerticalOptionsProperty, LayoutOptions.Center);
			label34.SetValue(Label.VerticalTextAlignmentProperty, new TextAlignmentConverter().ConvertFromInvariantString("Center"));
			grid13.Children.Add(label34);
			sfExpander5.SetValue(SfExpander.HeaderProperty, grid13);
			stackLayout12.SetValue(StackLayout.OrientationProperty, 0);
			label35.SetValue(Label.TextProperty, "Gauge and pointer width:");
			stackLayout12.Children.Add(label35);
			numericEntryV14.Completed += this.CircularGaugeWidthEntry_Completed;
			bindingExtension66.Mode = 1;
			bindingExtension66.Path = "CiruclarGaugeWidth";
			bindingExtension66.TypedBinding = new TypedBinding<DashboardItem, double>(delegate(DashboardItem A_0)
			{
				if (A_0 != null)
				{
					return new ValueTuple<double, bool>(A_0.CiruclarGaugeWidth, true);
				}
				return default(ValueTuple<double, bool>);
			}, delegate(DashboardItem A_0, double A_1)
			{
				if (A_0 != null)
				{
					A_0.CiruclarGaugeWidth = A_1;
					return;
				}
			}, new Tuple<Func<DashboardItem, object>, string>[]
			{
				new Tuple<Func<DashboardItem, object>, string>((DashboardItem A_0) => A_0, "CiruclarGaugeWidth")
			});
			BindingBase bindingBase66 = bindingExtension66.ProvideValue(null);
			numericEntryV14.SetBinding(NumericEntryV3.ValueProperty, bindingBase66);
			stackLayout12.Children.Add(numericEntryV14);
			translate57.Text = "ios_DashboardItemEditor_Minimum";
			IMarkupExtension markupExtension93 = translate57;
			XamlServiceProvider xamlServiceProvider93 = new XamlServiceProvider();
			Type typeFromHandle185 = typeof(IProvideValueTarget);
			object[] array93 = new object[0 + 8];
			array93[0] = label36;
			array93[1] = stackLayout12;
			array93[2] = frame7;
			array93[3] = sfExpander5;
			array93[4] = stackLayout24;
			array93[5] = scrollView;
			array93[6] = grid25;
			array93[7] = this;
			object obj156;
			xamlServiceProvider93.Add(typeFromHandle185, obj156 = new SimpleValueTargetProvider(array93, Label.TextProperty, nameScope));
			xamlServiceProvider93.Add(typeof(IReferenceProvider), obj156);
			Type typeFromHandle186 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver93 = new XmlNamespaceResolver();
			xmlNamespaceResolver93.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver93.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver93.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver93.Add("dash", "clr-namespace:CarScannerXamarinForms.Dashboard");
			xmlNamespaceResolver93.Add("expander", "clr-namespace:Syncfusion.XForms.Expander;assembly=Syncfusion.Expander.XForms");
			xmlNamespaceResolver93.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver93.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver93.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver93.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider93.Add(typeFromHandle186, new XamlTypeResolver(xmlNamespaceResolver93, typeof(DashboardItemEditor).GetTypeInfo().Assembly));
			xamlServiceProvider93.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(760, 44)));
			object obj157 = markupExtension93.ProvideValue(xamlServiceProvider93);
			label36.Text = obj157;
			stackLayout12.Children.Add(label36);
			bindingExtension67.Mode = 1;
			bindingExtension67.Path = "Minimum";
			bindingExtension67.TypedBinding = new TypedBinding<DashboardItem, double>(delegate(DashboardItem A_0)
			{
				if (A_0 != null)
				{
					return new ValueTuple<double, bool>(A_0.Minimum, true);
				}
				return default(ValueTuple<double, bool>);
			}, delegate(DashboardItem A_0, double A_1)
			{
				if (A_0 != null)
				{
					A_0.Minimum = A_1;
					return;
				}
			}, new Tuple<Func<DashboardItem, object>, string>[]
			{
				new Tuple<Func<DashboardItem, object>, string>((DashboardItem A_0) => A_0, "Minimum")
			});
			BindingBase bindingBase67 = bindingExtension67.ProvideValue(null);
			numericEntryV15.SetBinding(NumericEntryV3.ValueProperty, bindingBase67);
			stackLayout12.Children.Add(numericEntryV15);
			translate58.Text = "ios_DashboardItemEditor_Maximum";
			IMarkupExtension markupExtension94 = translate58;
			XamlServiceProvider xamlServiceProvider94 = new XamlServiceProvider();
			Type typeFromHandle187 = typeof(IProvideValueTarget);
			object[] array94 = new object[0 + 8];
			array94[0] = label37;
			array94[1] = stackLayout12;
			array94[2] = frame7;
			array94[3] = sfExpander5;
			array94[4] = stackLayout24;
			array94[5] = scrollView;
			array94[6] = grid25;
			array94[7] = this;
			object obj158;
			xamlServiceProvider94.Add(typeFromHandle187, obj158 = new SimpleValueTargetProvider(array94, Label.TextProperty, nameScope));
			xamlServiceProvider94.Add(typeof(IReferenceProvider), obj158);
			Type typeFromHandle188 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver94 = new XmlNamespaceResolver();
			xmlNamespaceResolver94.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver94.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver94.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver94.Add("dash", "clr-namespace:CarScannerXamarinForms.Dashboard");
			xmlNamespaceResolver94.Add("expander", "clr-namespace:Syncfusion.XForms.Expander;assembly=Syncfusion.Expander.XForms");
			xmlNamespaceResolver94.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver94.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver94.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver94.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider94.Add(typeFromHandle188, new XamlTypeResolver(xmlNamespaceResolver94, typeof(DashboardItemEditor).GetTypeInfo().Assembly));
			xamlServiceProvider94.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(762, 44)));
			object obj159 = markupExtension94.ProvideValue(xamlServiceProvider94);
			label37.Text = obj159;
			stackLayout12.Children.Add(label37);
			bindingExtension68.Mode = 1;
			bindingExtension68.Path = "Maximum";
			bindingExtension68.TypedBinding = new TypedBinding<DashboardItem, double>(delegate(DashboardItem A_0)
			{
				if (A_0 != null)
				{
					return new ValueTuple<double, bool>(A_0.Maximum, true);
				}
				return default(ValueTuple<double, bool>);
			}, delegate(DashboardItem A_0, double A_1)
			{
				if (A_0 != null)
				{
					A_0.Maximum = A_1;
					return;
				}
			}, new Tuple<Func<DashboardItem, object>, string>[]
			{
				new Tuple<Func<DashboardItem, object>, string>((DashboardItem A_0) => A_0, "Maximum")
			});
			BindingBase bindingBase68 = bindingExtension68.ProvideValue(null);
			numericEntryV16.SetBinding(NumericEntryV3.ValueProperty, bindingBase68);
			stackLayout12.Children.Add(numericEntryV16);
			dashboardItemColorBox19.SetValue(Element.ClassIdProperty, "GaugePointerColor");
			dashboardItemColorBox19.Tapped += this.ColorBox_Tapped;
			translate59.Text = "ios_DashboardItemEditor_GaugePointerColor";
			IMarkupExtension markupExtension95 = translate59;
			XamlServiceProvider xamlServiceProvider95 = new XamlServiceProvider();
			Type typeFromHandle189 = typeof(IProvideValueTarget);
			object[] array95 = new object[0 + 8];
			array95[0] = dashboardItemColorBox19;
			array95[1] = stackLayout12;
			array95[2] = frame7;
			array95[3] = sfExpander5;
			array95[4] = stackLayout24;
			array95[5] = scrollView;
			array95[6] = grid25;
			array95[7] = this;
			object obj160;
			xamlServiceProvider95.Add(typeFromHandle189, obj160 = new SimpleValueTargetProvider(array95, DashboardItemColorBox.TextProperty, nameScope));
			xamlServiceProvider95.Add(typeof(IReferenceProvider), obj160);
			Type typeFromHandle190 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver95 = new XmlNamespaceResolver();
			xmlNamespaceResolver95.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver95.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver95.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver95.Add("dash", "clr-namespace:CarScannerXamarinForms.Dashboard");
			xmlNamespaceResolver95.Add("expander", "clr-namespace:Syncfusion.XForms.Expander;assembly=Syncfusion.Expander.XForms");
			xmlNamespaceResolver95.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver95.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver95.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver95.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider95.Add(typeFromHandle190, new XamlTypeResolver(xmlNamespaceResolver95, typeof(DashboardItemEditor).GetTypeInfo().Assembly));
			xamlServiceProvider95.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(768, 41)));
			object obj161 = markupExtension95.ProvideValue(xamlServiceProvider95);
			dashboardItemColorBox19.Text = obj161;
			bindingExtension69.Mode = 2;
			bindingExtension69.Path = "GaugePointerColor";
			bindingExtension69.TypedBinding = new TypedBinding<DashboardItem, Color>(delegate(DashboardItem A_0)
			{
				if (A_0 != null)
				{
					return new ValueTuple<Color, bool>(A_0.GaugePointerColor, true);
				}
				return default(ValueTuple<Color, bool>);
			}, null, new Tuple<Func<DashboardItem, object>, string>[]
			{
				new Tuple<Func<DashboardItem, object>, string>((DashboardItem A_0) => A_0, "GaugePointerColor")
			});
			BindingBase bindingBase69 = bindingExtension69.ProvideValue(null);
			dashboardItemColorBox19.SetBinding(DashboardItemColorBox.ColorProperty, bindingBase69);
			stackLayout12.Children.Add(dashboardItemColorBox19);
			frame7.SetValue(ContentView.ContentProperty, stackLayout12);
			sfExpander5.SetValue(SfExpander.ContentProperty, frame7);
			stackLayout24.Children.Add(sfExpander5);
			sfExpander6.SetValue(SfExpander.AnimationDurationProperty, 250.0);
			sfExpander6.SetValue(SfExpander.AnimationEasingProperty, 3);
			dynamicResourceExtension28.Key = "NavigationBarBackgroundColor";
			IMarkupExtension<DynamicResource> markupExtension96 = dynamicResourceExtension28;
			XamlServiceProvider xamlServiceProvider96 = new XamlServiceProvider();
			Type typeFromHandle191 = typeof(IProvideValueTarget);
			object[] array96 = new object[0 + 5];
			array96[0] = sfExpander6;
			array96[1] = stackLayout24;
			array96[2] = scrollView;
			array96[3] = grid25;
			array96[4] = this;
			object obj162;
			xamlServiceProvider96.Add(typeFromHandle191, obj162 = new SimpleValueTargetProvider(array96, SfExpander.HeaderBackgroundColorProperty, nameScope));
			xamlServiceProvider96.Add(typeof(IReferenceProvider), obj162);
			Type typeFromHandle192 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver96 = new XmlNamespaceResolver();
			xmlNamespaceResolver96.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver96.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver96.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver96.Add("dash", "clr-namespace:CarScannerXamarinForms.Dashboard");
			xmlNamespaceResolver96.Add("expander", "clr-namespace:Syncfusion.XForms.Expander;assembly=Syncfusion.Expander.XForms");
			xmlNamespaceResolver96.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver96.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver96.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver96.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider96.Add(typeFromHandle192, new XamlTypeResolver(xmlNamespaceResolver96, typeof(DashboardItemEditor).GetTypeInfo().Assembly));
			xamlServiceProvider96.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(833, 25)));
			DynamicResource dynamicResource28 = markupExtension96.ProvideValue(xamlServiceProvider96);
			sfExpander6.SetDynamicResource(SfExpander.HeaderBackgroundColorProperty, dynamicResource28.Key);
			dynamicResourceExtension29.Key = "NavigationBarButtonColor";
			IMarkupExtension<DynamicResource> markupExtension97 = dynamicResourceExtension29;
			XamlServiceProvider xamlServiceProvider97 = new XamlServiceProvider();
			Type typeFromHandle193 = typeof(IProvideValueTarget);
			object[] array97 = new object[0 + 5];
			array97[0] = sfExpander6;
			array97[1] = stackLayout24;
			array97[2] = scrollView;
			array97[3] = grid25;
			array97[4] = this;
			object obj163;
			xamlServiceProvider97.Add(typeFromHandle193, obj163 = new SimpleValueTargetProvider(array97, SfExpander.IconColorProperty, nameScope));
			xamlServiceProvider97.Add(typeof(IReferenceProvider), obj163);
			Type typeFromHandle194 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver97 = new XmlNamespaceResolver();
			xmlNamespaceResolver97.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver97.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver97.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver97.Add("dash", "clr-namespace:CarScannerXamarinForms.Dashboard");
			xmlNamespaceResolver97.Add("expander", "clr-namespace:Syncfusion.XForms.Expander;assembly=Syncfusion.Expander.XForms");
			xmlNamespaceResolver97.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver97.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver97.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver97.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider97.Add(typeFromHandle194, new XamlTypeResolver(xmlNamespaceResolver97, typeof(DashboardItemEditor).GetTypeInfo().Assembly));
			xamlServiceProvider97.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(834, 25)));
			DynamicResource dynamicResource29 = markupExtension97.ProvideValue(xamlServiceProvider97);
			sfExpander6.SetDynamicResource(SfExpander.IconColorProperty, dynamicResource29.Key);
			sfExpander6.SetValue(SfExpander.IsExpandedProperty, false);
			label38.SetValue(View.MarginProperty, new Thickness(5.0));
			label38.SetValue(Label.FontAttributesProperty, new FontAttributesConverter().ConvertFromInvariantString("Bold"));
			translate60.Text = "DashItem_ChartOptions";
			IMarkupExtension markupExtension98 = translate60;
			XamlServiceProvider xamlServiceProvider98 = new XamlServiceProvider();
			Type typeFromHandle195 = typeof(IProvideValueTarget);
			object[] array98 = new object[0 + 7];
			array98[0] = label38;
			array98[1] = grid14;
			array98[2] = sfExpander6;
			array98[3] = stackLayout24;
			array98[4] = scrollView;
			array98[5] = grid25;
			array98[6] = this;
			object obj164;
			xamlServiceProvider98.Add(typeFromHandle195, obj164 = new SimpleValueTargetProvider(array98, Label.TextProperty, nameScope));
			xamlServiceProvider98.Add(typeof(IReferenceProvider), obj164);
			Type typeFromHandle196 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver98 = new XmlNamespaceResolver();
			xmlNamespaceResolver98.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver98.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver98.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver98.Add("dash", "clr-namespace:CarScannerXamarinForms.Dashboard");
			xmlNamespaceResolver98.Add("expander", "clr-namespace:Syncfusion.XForms.Expander;assembly=Syncfusion.Expander.XForms");
			xmlNamespaceResolver98.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver98.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver98.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver98.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider98.Add(typeFromHandle196, new XamlTypeResolver(xmlNamespaceResolver98, typeof(DashboardItemEditor).GetTypeInfo().Assembly));
			xamlServiceProvider98.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(841, 37)));
			object obj165 = markupExtension98.ProvideValue(xamlServiceProvider98);
			label38.Text = obj165;
			dynamicResourceExtension30.Key = "NavigationBarTextColor";
			IMarkupExtension<DynamicResource> markupExtension99 = dynamicResourceExtension30;
			XamlServiceProvider xamlServiceProvider99 = new XamlServiceProvider();
			Type typeFromHandle197 = typeof(IProvideValueTarget);
			object[] array99 = new object[0 + 7];
			array99[0] = label38;
			array99[1] = grid14;
			array99[2] = sfExpander6;
			array99[3] = stackLayout24;
			array99[4] = scrollView;
			array99[5] = grid25;
			array99[6] = this;
			object obj166;
			xamlServiceProvider99.Add(typeFromHandle197, obj166 = new SimpleValueTargetProvider(array99, Label.TextColorProperty, nameScope));
			xamlServiceProvider99.Add(typeof(IReferenceProvider), obj166);
			Type typeFromHandle198 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver99 = new XmlNamespaceResolver();
			xmlNamespaceResolver99.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver99.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver99.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver99.Add("dash", "clr-namespace:CarScannerXamarinForms.Dashboard");
			xmlNamespaceResolver99.Add("expander", "clr-namespace:Syncfusion.XForms.Expander;assembly=Syncfusion.Expander.XForms");
			xmlNamespaceResolver99.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver99.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver99.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver99.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider99.Add(typeFromHandle198, new XamlTypeResolver(xmlNamespaceResolver99, typeof(DashboardItemEditor).GetTypeInfo().Assembly));
			xamlServiceProvider99.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(842, 37)));
			DynamicResource dynamicResource30 = markupExtension99.ProvideValue(xamlServiceProvider99);
			label38.SetDynamicResource(Label.TextColorProperty, dynamicResource30.Key);
			label38.SetValue(View.VerticalOptionsProperty, LayoutOptions.Center);
			label38.SetValue(Label.VerticalTextAlignmentProperty, new TextAlignmentConverter().ConvertFromInvariantString("Center"));
			grid14.Children.Add(label38);
			sfExpander6.SetValue(SfExpander.HeaderProperty, grid14);
			stackLayout15.SetValue(StackLayout.OrientationProperty, 0);
			translate61.Text = "dash_ChartValuePosition";
			IMarkupExtension markupExtension100 = translate61;
			XamlServiceProvider xamlServiceProvider100 = new XamlServiceProvider();
			Type typeFromHandle199 = typeof(IProvideValueTarget);
			object[] array100 = new object[0 + 8];
			array100[0] = label39;
			array100[1] = stackLayout15;
			array100[2] = frame8;
			array100[3] = sfExpander6;
			array100[4] = stackLayout24;
			array100[5] = scrollView;
			array100[6] = grid25;
			array100[7] = this;
			object obj167;
			xamlServiceProvider100.Add(typeFromHandle199, obj167 = new SimpleValueTargetProvider(array100, Label.TextProperty, nameScope));
			xamlServiceProvider100.Add(typeof(IReferenceProvider), obj167);
			Type typeFromHandle200 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver100 = new XmlNamespaceResolver();
			xmlNamespaceResolver100.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver100.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver100.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver100.Add("dash", "clr-namespace:CarScannerXamarinForms.Dashboard");
			xmlNamespaceResolver100.Add("expander", "clr-namespace:Syncfusion.XForms.Expander;assembly=Syncfusion.Expander.XForms");
			xmlNamespaceResolver100.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver100.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver100.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver100.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider100.Add(typeFromHandle200, new XamlTypeResolver(xmlNamespaceResolver100, typeof(DashboardItemEditor).GetTypeInfo().Assembly));
			xamlServiceProvider100.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(850, 44)));
			object obj168 = markupExtension100.ProvideValue(xamlServiceProvider100);
			label39.Text = obj168;
			stackLayout15.Children.Add(label39);
			translate62.Text = "dash_ChartValuePosition_TopRightCorner";
			IMarkupExtension markupExtension101 = translate62;
			XamlServiceProvider xamlServiceProvider101 = new XamlServiceProvider();
			Type typeFromHandle201 = typeof(IProvideValueTarget);
			object[] array101 = new object[0 + 8];
			array101[0] = radioButton;
			array101[1] = stackLayout15;
			array101[2] = frame8;
			array101[3] = sfExpander6;
			array101[4] = stackLayout24;
			array101[5] = scrollView;
			array101[6] = grid25;
			array101[7] = this;
			object obj169;
			xamlServiceProvider101.Add(typeFromHandle201, obj169 = new SimpleValueTargetProvider(array101, RadioButton.ContentProperty, nameScope));
			xamlServiceProvider101.Add(typeof(IReferenceProvider), obj169);
			Type typeFromHandle202 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver101 = new XmlNamespaceResolver();
			xmlNamespaceResolver101.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver101.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver101.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver101.Add("dash", "clr-namespace:CarScannerXamarinForms.Dashboard");
			xmlNamespaceResolver101.Add("expander", "clr-namespace:Syncfusion.XForms.Expander;assembly=Syncfusion.Expander.XForms");
			xmlNamespaceResolver101.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver101.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver101.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver101.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider101.Add(typeFromHandle202, new XamlTypeResolver(xmlNamespaceResolver101, typeof(DashboardItemEditor).GetTypeInfo().Assembly));
			xamlServiceProvider101.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(852, 41)));
			object obj170 = markupExtension101.ProvideValue(xamlServiceProvider101);
			radioButton.SetValue(RadioButton.ContentProperty, obj170);
			radioButton.SetValue(RadioButton.GroupNameProperty, "ChartValuePositionCenter");
			bindingExtension70.Mode = 1;
			staticResourceExtension8.Key = "BoolToNegativeConverter";
			IMarkupExtension markupExtension102 = staticResourceExtension8;
			XamlServiceProvider xamlServiceProvider102 = new XamlServiceProvider();
			Type typeFromHandle203 = typeof(IProvideValueTarget);
			object[] array102 = new object[0 + 9];
			array102[0] = bindingExtension70;
			array102[1] = radioButton;
			array102[2] = stackLayout15;
			array102[3] = frame8;
			array102[4] = sfExpander6;
			array102[5] = stackLayout24;
			array102[6] = scrollView;
			array102[7] = grid25;
			array102[8] = this;
			object obj171;
			xamlServiceProvider102.Add(typeFromHandle203, obj171 = new SimpleValueTargetProvider(array102, typeof(BindingExtension).GetRuntimeProperty("Converter"), nameScope));
			xamlServiceProvider102.Add(typeof(IReferenceProvider), obj171);
			Type typeFromHandle204 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver102 = new XmlNamespaceResolver();
			xmlNamespaceResolver102.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver102.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver102.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver102.Add("dash", "clr-namespace:CarScannerXamarinForms.Dashboard");
			xmlNamespaceResolver102.Add("expander", "clr-namespace:Syncfusion.XForms.Expander;assembly=Syncfusion.Expander.XForms");
			xmlNamespaceResolver102.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver102.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver102.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver102.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider102.Add(typeFromHandle204, new XamlTypeResolver(xmlNamespaceResolver102, typeof(DashboardItemEditor).GetTypeInfo().Assembly));
			xamlServiceProvider102.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(854, 41)));
			object obj172 = markupExtension102.ProvideValue(xamlServiceProvider102);
			bindingExtension70.Converter = obj172;
			bindingExtension70.Path = "ChartValuePositionCenter";
			bindingExtension70.TypedBinding = new TypedBinding<DashboardItem, bool>(delegate(DashboardItem A_0)
			{
				if (A_0 != null)
				{
					return new ValueTuple<bool, bool>(A_0.ChartValuePositionCenter, true);
				}
				return default(ValueTuple<bool, bool>);
			}, delegate(DashboardItem A_0, bool A_1)
			{
				if (A_0 != null)
				{
					A_0.ChartValuePositionCenter = A_1;
					return;
				}
			}, new Tuple<Func<DashboardItem, object>, string>[]
			{
				new Tuple<Func<DashboardItem, object>, string>((DashboardItem A_0) => A_0, "ChartValuePositionCenter")
			});
			BindingBase bindingBase70 = bindingExtension70.ProvideValue(null);
			radioButton.SetBinding(RadioButton.IsCheckedProperty, bindingBase70);
			stackLayout15.Children.Add(radioButton);
			translate63.Text = "dash_ChartValuePosition_Center";
			IMarkupExtension markupExtension103 = translate63;
			XamlServiceProvider xamlServiceProvider103 = new XamlServiceProvider();
			Type typeFromHandle205 = typeof(IProvideValueTarget);
			object[] array103 = new object[0 + 8];
			array103[0] = radioButton2;
			array103[1] = stackLayout15;
			array103[2] = frame8;
			array103[3] = sfExpander6;
			array103[4] = stackLayout24;
			array103[5] = scrollView;
			array103[6] = grid25;
			array103[7] = this;
			object obj173;
			xamlServiceProvider103.Add(typeFromHandle205, obj173 = new SimpleValueTargetProvider(array103, RadioButton.ContentProperty, nameScope));
			xamlServiceProvider103.Add(typeof(IReferenceProvider), obj173);
			Type typeFromHandle206 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver103 = new XmlNamespaceResolver();
			xmlNamespaceResolver103.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver103.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver103.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver103.Add("dash", "clr-namespace:CarScannerXamarinForms.Dashboard");
			xmlNamespaceResolver103.Add("expander", "clr-namespace:Syncfusion.XForms.Expander;assembly=Syncfusion.Expander.XForms");
			xmlNamespaceResolver103.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver103.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver103.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver103.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider103.Add(typeFromHandle206, new XamlTypeResolver(xmlNamespaceResolver103, typeof(DashboardItemEditor).GetTypeInfo().Assembly));
			xamlServiceProvider103.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(856, 41)));
			object obj174 = markupExtension103.ProvideValue(xamlServiceProvider103);
			radioButton2.SetValue(RadioButton.ContentProperty, obj174);
			radioButton2.SetValue(RadioButton.GroupNameProperty, "ChartValuePositionCenter");
			bindingExtension71.Mode = 1;
			bindingExtension71.Path = "ChartValuePositionCenter";
			bindingExtension71.TypedBinding = new TypedBinding<DashboardItem, bool>(delegate(DashboardItem A_0)
			{
				if (A_0 != null)
				{
					return new ValueTuple<bool, bool>(A_0.ChartValuePositionCenter, true);
				}
				return default(ValueTuple<bool, bool>);
			}, delegate(DashboardItem A_0, bool A_1)
			{
				if (A_0 != null)
				{
					A_0.ChartValuePositionCenter = A_1;
					return;
				}
			}, new Tuple<Func<DashboardItem, object>, string>[]
			{
				new Tuple<Func<DashboardItem, object>, string>((DashboardItem A_0) => A_0, "ChartValuePositionCenter")
			});
			BindingBase bindingBase71 = bindingExtension71.ProvideValue(null);
			radioButton2.SetBinding(RadioButton.IsCheckedProperty, bindingBase71);
			stackLayout15.Children.Add(radioButton2);
			translate64.Text = "Settings_ChartStyle";
			IMarkupExtension markupExtension104 = translate64;
			XamlServiceProvider xamlServiceProvider104 = new XamlServiceProvider();
			Type typeFromHandle207 = typeof(IProvideValueTarget);
			object[] array104 = new object[0 + 8];
			array104[0] = label40;
			array104[1] = stackLayout15;
			array104[2] = frame8;
			array104[3] = sfExpander6;
			array104[4] = stackLayout24;
			array104[5] = scrollView;
			array104[6] = grid25;
			array104[7] = this;
			object obj175;
			xamlServiceProvider104.Add(typeFromHandle207, obj175 = new SimpleValueTargetProvider(array104, Label.TextProperty, nameScope));
			xamlServiceProvider104.Add(typeof(IReferenceProvider), obj175);
			Type typeFromHandle208 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver104 = new XmlNamespaceResolver();
			xmlNamespaceResolver104.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver104.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver104.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver104.Add("dash", "clr-namespace:CarScannerXamarinForms.Dashboard");
			xmlNamespaceResolver104.Add("expander", "clr-namespace:Syncfusion.XForms.Expander;assembly=Syncfusion.Expander.XForms");
			xmlNamespaceResolver104.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver104.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver104.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver104.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider104.Add(typeFromHandle208, new XamlTypeResolver(xmlNamespaceResolver104, typeof(DashboardItemEditor).GetTypeInfo().Assembly));
			xamlServiceProvider104.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(859, 44)));
			object obj176 = markupExtension104.ProvideValue(xamlServiceProvider104);
			label40.Text = obj176;
			stackLayout15.Children.Add(label40);
			staticResourceExtension9.Key = "EnumToIntConverter";
			IMarkupExtension markupExtension105 = staticResourceExtension9;
			XamlServiceProvider xamlServiceProvider105 = new XamlServiceProvider();
			Type typeFromHandle209 = typeof(IProvideValueTarget);
			object[] array105 = new object[0 + 9];
			array105[0] = bindingExtension72;
			array105[1] = picker3;
			array105[2] = stackLayout15;
			array105[3] = frame8;
			array105[4] = sfExpander6;
			array105[5] = stackLayout24;
			array105[6] = scrollView;
			array105[7] = grid25;
			array105[8] = this;
			object obj177;
			xamlServiceProvider105.Add(typeFromHandle209, obj177 = new SimpleValueTargetProvider(array105, typeof(BindingExtension).GetRuntimeProperty("Converter"), nameScope));
			xamlServiceProvider105.Add(typeof(IReferenceProvider), obj177);
			Type typeFromHandle210 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver105 = new XmlNamespaceResolver();
			xmlNamespaceResolver105.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver105.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver105.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver105.Add("dash", "clr-namespace:CarScannerXamarinForms.Dashboard");
			xmlNamespaceResolver105.Add("expander", "clr-namespace:Syncfusion.XForms.Expander;assembly=Syncfusion.Expander.XForms");
			xmlNamespaceResolver105.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver105.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver105.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver105.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider105.Add(typeFromHandle210, new XamlTypeResolver(xmlNamespaceResolver105, typeof(DashboardItemEditor).GetTypeInfo().Assembly));
			xamlServiceProvider105.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(860, 71)));
			object obj178 = markupExtension105.ProvideValue(xamlServiceProvider105);
			bindingExtension72.Converter = obj178;
			bindingExtension72.Mode = 1;
			bindingExtension72.Path = "ChartItemType";
			bindingExtension72.TypedBinding = new TypedBinding<DashboardItem, ChartItemTypes>(delegate(DashboardItem A_0)
			{
				if (A_0 != null)
				{
					return new ValueTuple<ChartItemTypes, bool>(A_0.ChartItemType, true);
				}
				return default(ValueTuple<ChartItemTypes, bool>);
			}, delegate(DashboardItem A_0, ChartItemTypes A_1)
			{
				if (A_0 != null)
				{
					A_0.ChartItemType = A_1;
					return;
				}
			}, new Tuple<Func<DashboardItem, object>, string>[]
			{
				new Tuple<Func<DashboardItem, object>, string>((DashboardItem A_0) => A_0, "ChartItemType")
			});
			BindingBase bindingBase72 = bindingExtension72.ProvideValue(null);
			picker3.SetBinding(Picker.SelectedIndexProperty, bindingBase72);
			stackLayout15.Children.Add(picker3);
			translate65.Text = "Settings_Control_tbChartVisibleTime.Text";
			IMarkupExtension markupExtension106 = translate65;
			XamlServiceProvider xamlServiceProvider106 = new XamlServiceProvider();
			Type typeFromHandle211 = typeof(IProvideValueTarget);
			object[] array106 = new object[0 + 8];
			array106[0] = label41;
			array106[1] = stackLayout15;
			array106[2] = frame8;
			array106[3] = sfExpander6;
			array106[4] = stackLayout24;
			array106[5] = scrollView;
			array106[6] = grid25;
			array106[7] = this;
			object obj179;
			xamlServiceProvider106.Add(typeFromHandle211, obj179 = new SimpleValueTargetProvider(array106, Label.TextProperty, nameScope));
			xamlServiceProvider106.Add(typeof(IReferenceProvider), obj179);
			Type typeFromHandle212 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver106 = new XmlNamespaceResolver();
			xmlNamespaceResolver106.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver106.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver106.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver106.Add("dash", "clr-namespace:CarScannerXamarinForms.Dashboard");
			xmlNamespaceResolver106.Add("expander", "clr-namespace:Syncfusion.XForms.Expander;assembly=Syncfusion.Expander.XForms");
			xmlNamespaceResolver106.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver106.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver106.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver106.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider106.Add(typeFromHandle212, new XamlTypeResolver(xmlNamespaceResolver106, typeof(DashboardItemEditor).GetTypeInfo().Assembly));
			xamlServiceProvider106.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(862, 44)));
			object obj180 = markupExtension106.ProvideValue(xamlServiceProvider106);
			label41.Text = obj180;
			stackLayout15.Children.Add(label41);
			bindingExtension73.Mode = 1;
			bindingExtension73.Path = "LiveDataShowTime";
			bindingExtension73.TypedBinding = new TypedBinding<DashboardItem, int>(delegate(DashboardItem A_0)
			{
				if (A_0 != null)
				{
					return new ValueTuple<int, bool>(A_0.LiveDataShowTime, true);
				}
				return default(ValueTuple<int, bool>);
			}, delegate(DashboardItem A_0, int A_1)
			{
				if (A_0 != null)
				{
					A_0.LiveDataShowTime = A_1;
					return;
				}
			}, new Tuple<Func<DashboardItem, object>, string>[]
			{
				new Tuple<Func<DashboardItem, object>, string>((DashboardItem A_0) => A_0, "LiveDataShowTime")
			});
			BindingBase bindingBase73 = bindingExtension73.ProvideValue(null);
			numericEntryV17.SetBinding(NumericEntryV3.ValueProperty, bindingBase73);
			stackLayout15.Children.Add(numericEntryV17);
			translate66.Text = "dash_ChartLineWidth";
			IMarkupExtension markupExtension107 = translate66;
			XamlServiceProvider xamlServiceProvider107 = new XamlServiceProvider();
			Type typeFromHandle213 = typeof(IProvideValueTarget);
			object[] array107 = new object[0 + 8];
			array107[0] = label42;
			array107[1] = stackLayout15;
			array107[2] = frame8;
			array107[3] = sfExpander6;
			array107[4] = stackLayout24;
			array107[5] = scrollView;
			array107[6] = grid25;
			array107[7] = this;
			object obj181;
			xamlServiceProvider107.Add(typeFromHandle213, obj181 = new SimpleValueTargetProvider(array107, Label.TextProperty, nameScope));
			xamlServiceProvider107.Add(typeof(IReferenceProvider), obj181);
			Type typeFromHandle214 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver107 = new XmlNamespaceResolver();
			xmlNamespaceResolver107.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver107.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver107.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver107.Add("dash", "clr-namespace:CarScannerXamarinForms.Dashboard");
			xmlNamespaceResolver107.Add("expander", "clr-namespace:Syncfusion.XForms.Expander;assembly=Syncfusion.Expander.XForms");
			xmlNamespaceResolver107.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver107.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver107.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver107.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider107.Add(typeFromHandle214, new XamlTypeResolver(xmlNamespaceResolver107, typeof(DashboardItemEditor).GetTypeInfo().Assembly));
			xamlServiceProvider107.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(865, 44)));
			object obj182 = markupExtension107.ProvideValue(xamlServiceProvider107);
			label42.Text = obj182;
			stackLayout15.Children.Add(label42);
			numericEntryV18.SetValue(NumericEntryV3.DoubleFormatProperty, "0");
			bindingExtension74.Mode = 1;
			bindingExtension74.Path = "ChartLineWidth";
			bindingExtension74.TypedBinding = new TypedBinding<DashboardItem, int>(delegate(DashboardItem A_0)
			{
				if (A_0 != null)
				{
					return new ValueTuple<int, bool>(A_0.ChartLineWidth, true);
				}
				return default(ValueTuple<int, bool>);
			}, delegate(DashboardItem A_0, int A_1)
			{
				if (A_0 != null)
				{
					A_0.ChartLineWidth = A_1;
					return;
				}
			}, new Tuple<Func<DashboardItem, object>, string>[]
			{
				new Tuple<Func<DashboardItem, object>, string>((DashboardItem A_0) => A_0, "ChartLineWidth")
			});
			BindingBase bindingBase74 = bindingExtension74.ProvideValue(null);
			numericEntryV18.SetBinding(NumericEntryV3.ValueProperty, bindingBase74);
			stackLayout15.Children.Add(numericEntryV18);
			dashboardItemColorBox20.SetValue(Element.ClassIdProperty, "GaugeLabelColor");
			dashboardItemColorBox20.Tapped += this.ColorBox_Tapped;
			translate67.Text = "ios_DashboardItemEditor_ChartLabelsColor";
			IMarkupExtension markupExtension108 = translate67;
			XamlServiceProvider xamlServiceProvider108 = new XamlServiceProvider();
			Type typeFromHandle215 = typeof(IProvideValueTarget);
			object[] array108 = new object[0 + 8];
			array108[0] = dashboardItemColorBox20;
			array108[1] = stackLayout15;
			array108[2] = frame8;
			array108[3] = sfExpander6;
			array108[4] = stackLayout24;
			array108[5] = scrollView;
			array108[6] = grid25;
			array108[7] = this;
			object obj183;
			xamlServiceProvider108.Add(typeFromHandle215, obj183 = new SimpleValueTargetProvider(array108, DashboardItemColorBox.TextProperty, nameScope));
			xamlServiceProvider108.Add(typeof(IReferenceProvider), obj183);
			Type typeFromHandle216 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver108 = new XmlNamespaceResolver();
			xmlNamespaceResolver108.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver108.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver108.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver108.Add("dash", "clr-namespace:CarScannerXamarinForms.Dashboard");
			xmlNamespaceResolver108.Add("expander", "clr-namespace:Syncfusion.XForms.Expander;assembly=Syncfusion.Expander.XForms");
			xmlNamespaceResolver108.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver108.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver108.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver108.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider108.Add(typeFromHandle216, new XamlTypeResolver(xmlNamespaceResolver108, typeof(DashboardItemEditor).GetTypeInfo().Assembly));
			xamlServiceProvider108.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(873, 41)));
			object obj184 = markupExtension108.ProvideValue(xamlServiceProvider108);
			dashboardItemColorBox20.Text = obj184;
			bindingExtension75.Mode = 2;
			bindingExtension75.Path = "GaugeLabelColor";
			bindingExtension75.TypedBinding = new TypedBinding<DashboardItem, Color>(delegate(DashboardItem A_0)
			{
				if (A_0 != null)
				{
					return new ValueTuple<Color, bool>(A_0.GaugeLabelColor, true);
				}
				return default(ValueTuple<Color, bool>);
			}, null, new Tuple<Func<DashboardItem, object>, string>[]
			{
				new Tuple<Func<DashboardItem, object>, string>((DashboardItem A_0) => A_0, "GaugeLabelColor")
			});
			BindingBase bindingBase75 = bindingExtension75.ProvideValue(null);
			dashboardItemColorBox20.SetBinding(DashboardItemColorBox.ColorProperty, bindingBase75);
			stackLayout15.Children.Add(dashboardItemColorBox20);
			dashboardItemColorBox21.SetValue(Element.ClassIdProperty, "ChartLineColor");
			dashboardItemColorBox21.Tapped += this.ColorBox_Tapped;
			translate68.Text = "ios_DashboardItemEditor_ChartValueLineColor";
			IMarkupExtension markupExtension109 = translate68;
			XamlServiceProvider xamlServiceProvider109 = new XamlServiceProvider();
			Type typeFromHandle217 = typeof(IProvideValueTarget);
			object[] array109 = new object[0 + 8];
			array109[0] = dashboardItemColorBox21;
			array109[1] = stackLayout15;
			array109[2] = frame8;
			array109[3] = sfExpander6;
			array109[4] = stackLayout24;
			array109[5] = scrollView;
			array109[6] = grid25;
			array109[7] = this;
			object obj185;
			xamlServiceProvider109.Add(typeFromHandle217, obj185 = new SimpleValueTargetProvider(array109, DashboardItemColorBox.TextProperty, nameScope));
			xamlServiceProvider109.Add(typeof(IReferenceProvider), obj185);
			Type typeFromHandle218 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver109 = new XmlNamespaceResolver();
			xmlNamespaceResolver109.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver109.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver109.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver109.Add("dash", "clr-namespace:CarScannerXamarinForms.Dashboard");
			xmlNamespaceResolver109.Add("expander", "clr-namespace:Syncfusion.XForms.Expander;assembly=Syncfusion.Expander.XForms");
			xmlNamespaceResolver109.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver109.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver109.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver109.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider109.Add(typeFromHandle218, new XamlTypeResolver(xmlNamespaceResolver109, typeof(DashboardItemEditor).GetTypeInfo().Assembly));
			xamlServiceProvider109.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(879, 41)));
			object obj186 = markupExtension109.ProvideValue(xamlServiceProvider109);
			dashboardItemColorBox21.Text = obj186;
			bindingExtension76.Mode = 2;
			bindingExtension76.Path = "ChartLineColor";
			bindingExtension76.TypedBinding = new TypedBinding<DashboardItem, Color>(delegate(DashboardItem A_0)
			{
				if (A_0 != null)
				{
					return new ValueTuple<Color, bool>(A_0.ChartLineColor, true);
				}
				return default(ValueTuple<Color, bool>);
			}, null, new Tuple<Func<DashboardItem, object>, string>[]
			{
				new Tuple<Func<DashboardItem, object>, string>((DashboardItem A_0) => A_0, "ChartLineColor")
			});
			BindingBase bindingBase76 = bindingExtension76.ProvideValue(null);
			dashboardItemColorBox21.SetBinding(DashboardItemColorBox.ColorProperty, bindingBase76);
			stackLayout15.Children.Add(dashboardItemColorBox21);
			bindingExtension77.Mode = 1;
			bindingExtension77.Path = "UseCustomMinMax";
			bindingExtension77.TypedBinding = new TypedBinding<DashboardItem, bool>(delegate(DashboardItem A_0)
			{
				if (A_0 != null)
				{
					return new ValueTuple<bool, bool>(A_0.UseCustomMinMax, true);
				}
				return default(ValueTuple<bool, bool>);
			}, delegate(DashboardItem A_0, bool A_1)
			{
				if (A_0 != null)
				{
					A_0.UseCustomMinMax = A_1;
					return;
				}
			}, new Tuple<Func<DashboardItem, object>, string>[]
			{
				new Tuple<Func<DashboardItem, object>, string>((DashboardItem A_0) => A_0, "UseCustomMinMax")
			});
			BindingBase bindingBase77 = bindingExtension77.ProvideValue(null);
			labelSwitch8.SetBinding(LabelSwitch.IsToggledProperty, bindingBase77);
			translate69.Text = "ios_DashboardItemEditor_UseCustomMinMax";
			IMarkupExtension markupExtension110 = translate69;
			XamlServiceProvider xamlServiceProvider110 = new XamlServiceProvider();
			Type typeFromHandle219 = typeof(IProvideValueTarget);
			object[] array110 = new object[0 + 8];
			array110[0] = labelSwitch8;
			array110[1] = stackLayout15;
			array110[2] = frame8;
			array110[3] = sfExpander6;
			array110[4] = stackLayout24;
			array110[5] = scrollView;
			array110[6] = grid25;
			array110[7] = this;
			object obj187;
			xamlServiceProvider110.Add(typeFromHandle219, obj187 = new SimpleValueTargetProvider(array110, LabelSwitch.TextProperty, nameScope));
			xamlServiceProvider110.Add(typeof(IReferenceProvider), obj187);
			Type typeFromHandle220 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver110 = new XmlNamespaceResolver();
			xmlNamespaceResolver110.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver110.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver110.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver110.Add("dash", "clr-namespace:CarScannerXamarinForms.Dashboard");
			xmlNamespaceResolver110.Add("expander", "clr-namespace:Syncfusion.XForms.Expander;assembly=Syncfusion.Expander.XForms");
			xmlNamespaceResolver110.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver110.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver110.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver110.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider110.Add(typeFromHandle220, new XamlTypeResolver(xmlNamespaceResolver110, typeof(DashboardItemEditor).GetTypeInfo().Assembly));
			xamlServiceProvider110.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(883, 107)));
			object obj188 = markupExtension110.ProvideValue(xamlServiceProvider110);
			labelSwitch8.Text = obj188;
			stackLayout15.Children.Add(labelSwitch8);
			bindingExtension78.Mode = 2;
			bindingExtension78.Path = "UseCustomMinMax";
			bindingExtension78.TypedBinding = new TypedBinding<DashboardItem, bool>(delegate(DashboardItem A_0)
			{
				if (A_0 != null)
				{
					return new ValueTuple<bool, bool>(A_0.UseCustomMinMax, true);
				}
				return default(ValueTuple<bool, bool>);
			}, null, new Tuple<Func<DashboardItem, object>, string>[]
			{
				new Tuple<Func<DashboardItem, object>, string>((DashboardItem A_0) => A_0, "UseCustomMinMax")
			});
			BindingBase bindingBase78 = bindingExtension78.ProvideValue(null);
			stackLayout13.SetBinding(VisualElement.IsVisibleProperty, bindingBase78);
			stackLayout13.SetValue(StackLayout.OrientationProperty, 0);
			translate70.Text = "ios_DashboardItemEditor_Minimum";
			IMarkupExtension markupExtension111 = translate70;
			XamlServiceProvider xamlServiceProvider111 = new XamlServiceProvider();
			Type typeFromHandle221 = typeof(IProvideValueTarget);
			object[] array111 = new object[0 + 9];
			array111[0] = label43;
			array111[1] = stackLayout13;
			array111[2] = stackLayout15;
			array111[3] = frame8;
			array111[4] = sfExpander6;
			array111[5] = stackLayout24;
			array111[6] = scrollView;
			array111[7] = grid25;
			array111[8] = this;
			object obj189;
			xamlServiceProvider111.Add(typeFromHandle221, obj189 = new SimpleValueTargetProvider(array111, Label.TextProperty, nameScope));
			xamlServiceProvider111.Add(typeof(IReferenceProvider), obj189);
			Type typeFromHandle222 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver111 = new XmlNamespaceResolver();
			xmlNamespaceResolver111.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver111.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver111.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver111.Add("dash", "clr-namespace:CarScannerXamarinForms.Dashboard");
			xmlNamespaceResolver111.Add("expander", "clr-namespace:Syncfusion.XForms.Expander;assembly=Syncfusion.Expander.XForms");
			xmlNamespaceResolver111.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver111.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver111.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver111.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider111.Add(typeFromHandle222, new XamlTypeResolver(xmlNamespaceResolver111, typeof(DashboardItemEditor).GetTypeInfo().Assembly));
			xamlServiceProvider111.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(887, 48)));
			object obj190 = markupExtension111.ProvideValue(xamlServiceProvider111);
			label43.Text = obj190;
			stackLayout13.Children.Add(label43);
			bindingExtension79.Mode = 1;
			bindingExtension79.Path = "Minimum";
			bindingExtension79.TypedBinding = new TypedBinding<DashboardItem, double>(delegate(DashboardItem A_0)
			{
				if (A_0 != null)
				{
					return new ValueTuple<double, bool>(A_0.Minimum, true);
				}
				return default(ValueTuple<double, bool>);
			}, delegate(DashboardItem A_0, double A_1)
			{
				if (A_0 != null)
				{
					A_0.Minimum = A_1;
					return;
				}
			}, new Tuple<Func<DashboardItem, object>, string>[]
			{
				new Tuple<Func<DashboardItem, object>, string>((DashboardItem A_0) => A_0, "Minimum")
			});
			BindingBase bindingBase79 = bindingExtension79.ProvideValue(null);
			numericEntryV19.SetBinding(NumericEntryV3.ValueProperty, bindingBase79);
			stackLayout13.Children.Add(numericEntryV19);
			translate71.Text = "ios_DashboardItemEditor_Maximum";
			IMarkupExtension markupExtension112 = translate71;
			XamlServiceProvider xamlServiceProvider112 = new XamlServiceProvider();
			Type typeFromHandle223 = typeof(IProvideValueTarget);
			object[] array112 = new object[0 + 9];
			array112[0] = label44;
			array112[1] = stackLayout13;
			array112[2] = stackLayout15;
			array112[3] = frame8;
			array112[4] = sfExpander6;
			array112[5] = stackLayout24;
			array112[6] = scrollView;
			array112[7] = grid25;
			array112[8] = this;
			object obj191;
			xamlServiceProvider112.Add(typeFromHandle223, obj191 = new SimpleValueTargetProvider(array112, Label.TextProperty, nameScope));
			xamlServiceProvider112.Add(typeof(IReferenceProvider), obj191);
			Type typeFromHandle224 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver112 = new XmlNamespaceResolver();
			xmlNamespaceResolver112.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver112.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver112.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver112.Add("dash", "clr-namespace:CarScannerXamarinForms.Dashboard");
			xmlNamespaceResolver112.Add("expander", "clr-namespace:Syncfusion.XForms.Expander;assembly=Syncfusion.Expander.XForms");
			xmlNamespaceResolver112.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver112.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver112.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver112.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider112.Add(typeFromHandle224, new XamlTypeResolver(xmlNamespaceResolver112, typeof(DashboardItemEditor).GetTypeInfo().Assembly));
			xamlServiceProvider112.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(889, 48)));
			object obj192 = markupExtension112.ProvideValue(xamlServiceProvider112);
			label44.Text = obj192;
			stackLayout13.Children.Add(label44);
			bindingExtension80.Mode = 1;
			bindingExtension80.Path = "Maximum";
			bindingExtension80.TypedBinding = new TypedBinding<DashboardItem, double>(delegate(DashboardItem A_0)
			{
				if (A_0 != null)
				{
					return new ValueTuple<double, bool>(A_0.Maximum, true);
				}
				return default(ValueTuple<double, bool>);
			}, delegate(DashboardItem A_0, double A_1)
			{
				if (A_0 != null)
				{
					A_0.Maximum = A_1;
					return;
				}
			}, new Tuple<Func<DashboardItem, object>, string>[]
			{
				new Tuple<Func<DashboardItem, object>, string>((DashboardItem A_0) => A_0, "Maximum")
			});
			BindingBase bindingBase80 = bindingExtension80.ProvideValue(null);
			numericEntryV20.SetBinding(NumericEntryV3.ValueProperty, bindingBase80);
			stackLayout13.Children.Add(numericEntryV20);
			stackLayout15.Children.Add(stackLayout13);
			bindingExtension81.Mode = 1;
			bindingExtension81.Path = "UseCustomInterval";
			bindingExtension81.TypedBinding = new TypedBinding<DashboardItem, bool>(delegate(DashboardItem A_0)
			{
				if (A_0 != null)
				{
					return new ValueTuple<bool, bool>(A_0.UseCustomInterval, true);
				}
				return default(ValueTuple<bool, bool>);
			}, delegate(DashboardItem A_0, bool A_1)
			{
				if (A_0 != null)
				{
					A_0.UseCustomInterval = A_1;
					return;
				}
			}, new Tuple<Func<DashboardItem, object>, string>[]
			{
				new Tuple<Func<DashboardItem, object>, string>((DashboardItem A_0) => A_0, "UseCustomInterval")
			});
			BindingBase bindingBase81 = bindingExtension81.ProvideValue(null);
			labelSwitch9.SetBinding(LabelSwitch.IsToggledProperty, bindingBase81);
			translate72.Text = "ios_UseCustomInterval";
			IMarkupExtension markupExtension113 = translate72;
			XamlServiceProvider xamlServiceProvider113 = new XamlServiceProvider();
			Type typeFromHandle225 = typeof(IProvideValueTarget);
			object[] array113 = new object[0 + 8];
			array113[0] = labelSwitch9;
			array113[1] = stackLayout15;
			array113[2] = frame8;
			array113[3] = sfExpander6;
			array113[4] = stackLayout24;
			array113[5] = scrollView;
			array113[6] = grid25;
			array113[7] = this;
			object obj193;
			xamlServiceProvider113.Add(typeFromHandle225, obj193 = new SimpleValueTargetProvider(array113, LabelSwitch.TextProperty, nameScope));
			xamlServiceProvider113.Add(typeof(IReferenceProvider), obj193);
			Type typeFromHandle226 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver113 = new XmlNamespaceResolver();
			xmlNamespaceResolver113.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver113.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver113.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver113.Add("dash", "clr-namespace:CarScannerXamarinForms.Dashboard");
			xmlNamespaceResolver113.Add("expander", "clr-namespace:Syncfusion.XForms.Expander;assembly=Syncfusion.Expander.XForms");
			xmlNamespaceResolver113.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver113.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver113.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver113.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider113.Add(typeFromHandle226, new XamlTypeResolver(xmlNamespaceResolver113, typeof(DashboardItemEditor).GetTypeInfo().Assembly));
			xamlServiceProvider113.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(893, 109)));
			object obj194 = markupExtension113.ProvideValue(xamlServiceProvider113);
			labelSwitch9.Text = obj194;
			stackLayout15.Children.Add(labelSwitch9);
			bindingExtension82.Mode = 2;
			bindingExtension82.Path = "UseCustomInterval";
			bindingExtension82.TypedBinding = new TypedBinding<DashboardItem, bool>(delegate(DashboardItem A_0)
			{
				if (A_0 != null)
				{
					return new ValueTuple<bool, bool>(A_0.UseCustomInterval, true);
				}
				return default(ValueTuple<bool, bool>);
			}, null, new Tuple<Func<DashboardItem, object>, string>[]
			{
				new Tuple<Func<DashboardItem, object>, string>((DashboardItem A_0) => A_0, "UseCustomInterval")
			});
			BindingBase bindingBase82 = bindingExtension82.ProvideValue(null);
			stackLayout14.SetBinding(VisualElement.IsVisibleProperty, bindingBase82);
			stackLayout14.SetValue(StackLayout.OrientationProperty, 0);
			translate73.Text = "ios_CustomInterval";
			IMarkupExtension markupExtension114 = translate73;
			XamlServiceProvider xamlServiceProvider114 = new XamlServiceProvider();
			Type typeFromHandle227 = typeof(IProvideValueTarget);
			object[] array114 = new object[0 + 9];
			array114[0] = label45;
			array114[1] = stackLayout14;
			array114[2] = stackLayout15;
			array114[3] = frame8;
			array114[4] = sfExpander6;
			array114[5] = stackLayout24;
			array114[6] = scrollView;
			array114[7] = grid25;
			array114[8] = this;
			object obj195;
			xamlServiceProvider114.Add(typeFromHandle227, obj195 = new SimpleValueTargetProvider(array114, Label.TextProperty, nameScope));
			xamlServiceProvider114.Add(typeof(IReferenceProvider), obj195);
			Type typeFromHandle228 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver114 = new XmlNamespaceResolver();
			xmlNamespaceResolver114.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver114.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver114.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver114.Add("dash", "clr-namespace:CarScannerXamarinForms.Dashboard");
			xmlNamespaceResolver114.Add("expander", "clr-namespace:Syncfusion.XForms.Expander;assembly=Syncfusion.Expander.XForms");
			xmlNamespaceResolver114.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver114.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver114.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver114.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider114.Add(typeFromHandle228, new XamlTypeResolver(xmlNamespaceResolver114, typeof(DashboardItemEditor).GetTypeInfo().Assembly));
			xamlServiceProvider114.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(896, 48)));
			object obj196 = markupExtension114.ProvideValue(xamlServiceProvider114);
			label45.Text = obj196;
			stackLayout14.Children.Add(label45);
			bindingExtension83.Mode = 1;
			bindingExtension83.Path = "CustomInterval";
			bindingExtension83.TypedBinding = new TypedBinding<DashboardItem, double>(delegate(DashboardItem A_0)
			{
				if (A_0 != null)
				{
					return new ValueTuple<double, bool>(A_0.CustomInterval, true);
				}
				return default(ValueTuple<double, bool>);
			}, delegate(DashboardItem A_0, double A_1)
			{
				if (A_0 != null)
				{
					A_0.CustomInterval = A_1;
					return;
				}
			}, new Tuple<Func<DashboardItem, object>, string>[]
			{
				new Tuple<Func<DashboardItem, object>, string>((DashboardItem A_0) => A_0, "CustomInterval")
			});
			BindingBase bindingBase83 = bindingExtension83.ProvideValue(null);
			numericEntryV21.SetBinding(NumericEntryV3.ValueProperty, bindingBase83);
			stackLayout14.Children.Add(numericEntryV21);
			stackLayout15.Children.Add(stackLayout14);
			frame8.SetValue(ContentView.ContentProperty, stackLayout15);
			sfExpander6.SetValue(SfExpander.ContentProperty, frame8);
			stackLayout24.Children.Add(sfExpander6);
			sfExpander7.SetValue(SfExpander.AnimationDurationProperty, 250.0);
			sfExpander7.SetValue(SfExpander.AnimationEasingProperty, 3);
			dynamicResourceExtension31.Key = "NavigationBarBackgroundColor";
			IMarkupExtension<DynamicResource> markupExtension115 = dynamicResourceExtension31;
			XamlServiceProvider xamlServiceProvider115 = new XamlServiceProvider();
			Type typeFromHandle229 = typeof(IProvideValueTarget);
			object[] array115 = new object[0 + 5];
			array115[0] = sfExpander7;
			array115[1] = stackLayout24;
			array115[2] = scrollView;
			array115[3] = grid25;
			array115[4] = this;
			object obj197;
			xamlServiceProvider115.Add(typeFromHandle229, obj197 = new SimpleValueTargetProvider(array115, SfExpander.HeaderBackgroundColorProperty, nameScope));
			xamlServiceProvider115.Add(typeof(IReferenceProvider), obj197);
			Type typeFromHandle230 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver115 = new XmlNamespaceResolver();
			xmlNamespaceResolver115.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver115.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver115.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver115.Add("dash", "clr-namespace:CarScannerXamarinForms.Dashboard");
			xmlNamespaceResolver115.Add("expander", "clr-namespace:Syncfusion.XForms.Expander;assembly=Syncfusion.Expander.XForms");
			xmlNamespaceResolver115.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver115.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver115.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver115.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider115.Add(typeFromHandle230, new XamlTypeResolver(xmlNamespaceResolver115, typeof(DashboardItemEditor).GetTypeInfo().Assembly));
			xamlServiceProvider115.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(910, 25)));
			DynamicResource dynamicResource31 = markupExtension115.ProvideValue(xamlServiceProvider115);
			sfExpander7.SetDynamicResource(SfExpander.HeaderBackgroundColorProperty, dynamicResource31.Key);
			dynamicResourceExtension32.Key = "NavigationBarButtonColor";
			IMarkupExtension<DynamicResource> markupExtension116 = dynamicResourceExtension32;
			XamlServiceProvider xamlServiceProvider116 = new XamlServiceProvider();
			Type typeFromHandle231 = typeof(IProvideValueTarget);
			object[] array116 = new object[0 + 5];
			array116[0] = sfExpander7;
			array116[1] = stackLayout24;
			array116[2] = scrollView;
			array116[3] = grid25;
			array116[4] = this;
			object obj198;
			xamlServiceProvider116.Add(typeFromHandle231, obj198 = new SimpleValueTargetProvider(array116, SfExpander.IconColorProperty, nameScope));
			xamlServiceProvider116.Add(typeof(IReferenceProvider), obj198);
			Type typeFromHandle232 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver116 = new XmlNamespaceResolver();
			xmlNamespaceResolver116.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver116.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver116.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver116.Add("dash", "clr-namespace:CarScannerXamarinForms.Dashboard");
			xmlNamespaceResolver116.Add("expander", "clr-namespace:Syncfusion.XForms.Expander;assembly=Syncfusion.Expander.XForms");
			xmlNamespaceResolver116.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver116.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver116.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver116.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider116.Add(typeFromHandle232, new XamlTypeResolver(xmlNamespaceResolver116, typeof(DashboardItemEditor).GetTypeInfo().Assembly));
			xamlServiceProvider116.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(911, 25)));
			DynamicResource dynamicResource32 = markupExtension116.ProvideValue(xamlServiceProvider116);
			sfExpander7.SetDynamicResource(SfExpander.IconColorProperty, dynamicResource32.Key);
			sfExpander7.SetValue(SfExpander.IsExpandedProperty, false);
			label46.SetValue(View.MarginProperty, new Thickness(5.0));
			label46.SetValue(Label.FontAttributesProperty, new FontAttributesConverter().ConvertFromInvariantString("Bold"));
			translate74.Text = "DashItem_LinearOptions";
			IMarkupExtension markupExtension117 = translate74;
			XamlServiceProvider xamlServiceProvider117 = new XamlServiceProvider();
			Type typeFromHandle233 = typeof(IProvideValueTarget);
			object[] array117 = new object[0 + 7];
			array117[0] = label46;
			array117[1] = grid15;
			array117[2] = sfExpander7;
			array117[3] = stackLayout24;
			array117[4] = scrollView;
			array117[5] = grid25;
			array117[6] = this;
			object obj199;
			xamlServiceProvider117.Add(typeFromHandle233, obj199 = new SimpleValueTargetProvider(array117, Label.TextProperty, nameScope));
			xamlServiceProvider117.Add(typeof(IReferenceProvider), obj199);
			Type typeFromHandle234 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver117 = new XmlNamespaceResolver();
			xmlNamespaceResolver117.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver117.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver117.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver117.Add("dash", "clr-namespace:CarScannerXamarinForms.Dashboard");
			xmlNamespaceResolver117.Add("expander", "clr-namespace:Syncfusion.XForms.Expander;assembly=Syncfusion.Expander.XForms");
			xmlNamespaceResolver117.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver117.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver117.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver117.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider117.Add(typeFromHandle234, new XamlTypeResolver(xmlNamespaceResolver117, typeof(DashboardItemEditor).GetTypeInfo().Assembly));
			xamlServiceProvider117.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(918, 37)));
			object obj200 = markupExtension117.ProvideValue(xamlServiceProvider117);
			label46.Text = obj200;
			dynamicResourceExtension33.Key = "NavigationBarTextColor";
			IMarkupExtension<DynamicResource> markupExtension118 = dynamicResourceExtension33;
			XamlServiceProvider xamlServiceProvider118 = new XamlServiceProvider();
			Type typeFromHandle235 = typeof(IProvideValueTarget);
			object[] array118 = new object[0 + 7];
			array118[0] = label46;
			array118[1] = grid15;
			array118[2] = sfExpander7;
			array118[3] = stackLayout24;
			array118[4] = scrollView;
			array118[5] = grid25;
			array118[6] = this;
			object obj201;
			xamlServiceProvider118.Add(typeFromHandle235, obj201 = new SimpleValueTargetProvider(array118, Label.TextColorProperty, nameScope));
			xamlServiceProvider118.Add(typeof(IReferenceProvider), obj201);
			Type typeFromHandle236 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver118 = new XmlNamespaceResolver();
			xmlNamespaceResolver118.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver118.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver118.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver118.Add("dash", "clr-namespace:CarScannerXamarinForms.Dashboard");
			xmlNamespaceResolver118.Add("expander", "clr-namespace:Syncfusion.XForms.Expander;assembly=Syncfusion.Expander.XForms");
			xmlNamespaceResolver118.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver118.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver118.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver118.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider118.Add(typeFromHandle236, new XamlTypeResolver(xmlNamespaceResolver118, typeof(DashboardItemEditor).GetTypeInfo().Assembly));
			xamlServiceProvider118.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(919, 37)));
			DynamicResource dynamicResource33 = markupExtension118.ProvideValue(xamlServiceProvider118);
			label46.SetDynamicResource(Label.TextColorProperty, dynamicResource33.Key);
			label46.SetValue(View.VerticalOptionsProperty, LayoutOptions.Center);
			label46.SetValue(Label.VerticalTextAlignmentProperty, new TextAlignmentConverter().ConvertFromInvariantString("Center"));
			grid15.Children.Add(label46);
			sfExpander7.SetValue(SfExpander.HeaderProperty, grid15);
			stackLayout16.SetValue(StackLayout.OrientationProperty, 0);
			translate75.Text = "ios_DashboardItemEditor_Minimum";
			IMarkupExtension markupExtension119 = translate75;
			XamlServiceProvider xamlServiceProvider119 = new XamlServiceProvider();
			Type typeFromHandle237 = typeof(IProvideValueTarget);
			object[] array119 = new object[0 + 8];
			array119[0] = label47;
			array119[1] = stackLayout16;
			array119[2] = frame9;
			array119[3] = sfExpander7;
			array119[4] = stackLayout24;
			array119[5] = scrollView;
			array119[6] = grid25;
			array119[7] = this;
			object obj202;
			xamlServiceProvider119.Add(typeFromHandle237, obj202 = new SimpleValueTargetProvider(array119, Label.TextProperty, nameScope));
			xamlServiceProvider119.Add(typeof(IReferenceProvider), obj202);
			Type typeFromHandle238 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver119 = new XmlNamespaceResolver();
			xmlNamespaceResolver119.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver119.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver119.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver119.Add("dash", "clr-namespace:CarScannerXamarinForms.Dashboard");
			xmlNamespaceResolver119.Add("expander", "clr-namespace:Syncfusion.XForms.Expander;assembly=Syncfusion.Expander.XForms");
			xmlNamespaceResolver119.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver119.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver119.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver119.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider119.Add(typeFromHandle238, new XamlTypeResolver(xmlNamespaceResolver119, typeof(DashboardItemEditor).GetTypeInfo().Assembly));
			xamlServiceProvider119.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(927, 44)));
			object obj203 = markupExtension119.ProvideValue(xamlServiceProvider119);
			label47.Text = obj203;
			stackLayout16.Children.Add(label47);
			bindingExtension84.Mode = 1;
			bindingExtension84.Path = "Minimum";
			bindingExtension84.TypedBinding = new TypedBinding<DashboardItem, double>(delegate(DashboardItem A_0)
			{
				if (A_0 != null)
				{
					return new ValueTuple<double, bool>(A_0.Minimum, true);
				}
				return default(ValueTuple<double, bool>);
			}, delegate(DashboardItem A_0, double A_1)
			{
				if (A_0 != null)
				{
					A_0.Minimum = A_1;
					return;
				}
			}, new Tuple<Func<DashboardItem, object>, string>[]
			{
				new Tuple<Func<DashboardItem, object>, string>((DashboardItem A_0) => A_0, "Minimum")
			});
			BindingBase bindingBase84 = bindingExtension84.ProvideValue(null);
			numericEntryV22.SetBinding(NumericEntryV3.ValueProperty, bindingBase84);
			stackLayout16.Children.Add(numericEntryV22);
			translate76.Text = "ios_DashboardItemEditor_Maximum";
			IMarkupExtension markupExtension120 = translate76;
			XamlServiceProvider xamlServiceProvider120 = new XamlServiceProvider();
			Type typeFromHandle239 = typeof(IProvideValueTarget);
			object[] array120 = new object[0 + 8];
			array120[0] = label48;
			array120[1] = stackLayout16;
			array120[2] = frame9;
			array120[3] = sfExpander7;
			array120[4] = stackLayout24;
			array120[5] = scrollView;
			array120[6] = grid25;
			array120[7] = this;
			object obj204;
			xamlServiceProvider120.Add(typeFromHandle239, obj204 = new SimpleValueTargetProvider(array120, Label.TextProperty, nameScope));
			xamlServiceProvider120.Add(typeof(IReferenceProvider), obj204);
			Type typeFromHandle240 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver120 = new XmlNamespaceResolver();
			xmlNamespaceResolver120.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver120.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver120.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver120.Add("dash", "clr-namespace:CarScannerXamarinForms.Dashboard");
			xmlNamespaceResolver120.Add("expander", "clr-namespace:Syncfusion.XForms.Expander;assembly=Syncfusion.Expander.XForms");
			xmlNamespaceResolver120.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver120.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver120.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver120.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider120.Add(typeFromHandle240, new XamlTypeResolver(xmlNamespaceResolver120, typeof(DashboardItemEditor).GetTypeInfo().Assembly));
			xamlServiceProvider120.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(930, 44)));
			object obj205 = markupExtension120.ProvideValue(xamlServiceProvider120);
			label48.Text = obj205;
			stackLayout16.Children.Add(label48);
			bindingExtension85.Mode = 1;
			bindingExtension85.Path = "Maximum";
			bindingExtension85.TypedBinding = new TypedBinding<DashboardItem, double>(delegate(DashboardItem A_0)
			{
				if (A_0 != null)
				{
					return new ValueTuple<double, bool>(A_0.Maximum, true);
				}
				return default(ValueTuple<double, bool>);
			}, delegate(DashboardItem A_0, double A_1)
			{
				if (A_0 != null)
				{
					A_0.Maximum = A_1;
					return;
				}
			}, new Tuple<Func<DashboardItem, object>, string>[]
			{
				new Tuple<Func<DashboardItem, object>, string>((DashboardItem A_0) => A_0, "Maximum")
			});
			BindingBase bindingBase85 = bindingExtension85.ProvideValue(null);
			numericEntryV23.SetBinding(NumericEntryV3.ValueProperty, bindingBase85);
			stackLayout16.Children.Add(numericEntryV23);
			grid16.SetValue(View.MarginProperty, new Thickness(0.0, 0.0, 0.0, 10.0));
			grid16.SetValue(Grid.RowSpacingProperty, 0.0);
			columnDefinition19.SetValue(ColumnDefinition.WidthProperty, new GridLengthTypeConverter().ConvertFromInvariantString("*"));
			grid16.GetValue(Grid.ColumnDefinitionsProperty).Add(columnDefinition19);
			columnDefinition20.SetValue(ColumnDefinition.WidthProperty, new GridLengthTypeConverter().ConvertFromInvariantString("auto"));
			grid16.GetValue(Grid.ColumnDefinitionsProperty).Add(columnDefinition20);
			rowDefinition15.SetValue(RowDefinition.HeightProperty, new GridLengthTypeConverter().ConvertFromInvariantString("auto"));
			grid16.GetValue(Grid.RowDefinitionsProperty).Add(rowDefinition15);
			rowDefinition16.SetValue(RowDefinition.HeightProperty, new GridLengthTypeConverter().ConvertFromInvariantString("auto"));
			grid16.GetValue(Grid.RowDefinitionsProperty).Add(rowDefinition16);
			label49.SetValue(Grid.RowProperty, 0);
			label49.SetValue(Grid.ColumnProperty, 0);
			translate77.Text = "ios_DashboardItemEditor_LinearGaugeScaleSize";
			IMarkupExtension markupExtension121 = translate77;
			XamlServiceProvider xamlServiceProvider121 = new XamlServiceProvider();
			Type typeFromHandle241 = typeof(IProvideValueTarget);
			object[] array121 = new object[0 + 9];
			array121[0] = label49;
			array121[1] = grid16;
			array121[2] = stackLayout16;
			array121[3] = frame9;
			array121[4] = sfExpander7;
			array121[5] = stackLayout24;
			array121[6] = scrollView;
			array121[7] = grid25;
			array121[8] = this;
			object obj206;
			xamlServiceProvider121.Add(typeFromHandle241, obj206 = new SimpleValueTargetProvider(array121, Label.TextProperty, nameScope));
			xamlServiceProvider121.Add(typeof(IReferenceProvider), obj206);
			Type typeFromHandle242 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver121 = new XmlNamespaceResolver();
			xmlNamespaceResolver121.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver121.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver121.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver121.Add("dash", "clr-namespace:CarScannerXamarinForms.Dashboard");
			xmlNamespaceResolver121.Add("expander", "clr-namespace:Syncfusion.XForms.Expander;assembly=Syncfusion.Expander.XForms");
			xmlNamespaceResolver121.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver121.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver121.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver121.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider121.Add(typeFromHandle242, new XamlTypeResolver(xmlNamespaceResolver121, typeof(DashboardItemEditor).GetTypeInfo().Assembly));
			xamlServiceProvider121.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(947, 45)));
			object obj207 = markupExtension121.ProvideValue(xamlServiceProvider121);
			label49.Text = obj207;
			grid16.Children.Add(label49);
			label50.SetValue(Grid.RowProperty, 0);
			label50.SetValue(Grid.ColumnProperty, 1);
			label50.SetValue(Label.FontAttributesProperty, new FontAttributesConverter().ConvertFromInvariantString("Bold"));
			bindingExtension86.Mode = 2;
			bindingExtension86.Path = "LinearScaleSize";
			bindingExtension86.TypedBinding = new TypedBinding<DashboardItem, double>(delegate(DashboardItem A_0)
			{
				if (A_0 != null)
				{
					return new ValueTuple<double, bool>(A_0.LinearScaleSize, true);
				}
				return default(ValueTuple<double, bool>);
			}, null, new Tuple<Func<DashboardItem, object>, string>[]
			{
				new Tuple<Func<DashboardItem, object>, string>((DashboardItem A_0) => A_0, "LinearScaleSize")
			});
			BindingBase bindingBase86 = bindingExtension86.ProvideValue(null);
			label50.SetBinding(Label.TextProperty, bindingBase86);
			label50.SetValue(View.VerticalOptionsProperty, LayoutOptions.Center);
			grid16.Children.Add(label50);
			extendedSlider8.SetValue(Grid.RowProperty, 1);
			extendedSlider8.SetValue(Grid.ColumnProperty, 0);
			extendedSlider8.SetValue(Grid.ColumnSpanProperty, 2);
			extendedSlider8.SetValue(Slider.MaximumProperty, 100.0);
			extendedSlider8.SetValue(Slider.MinimumProperty, 0.0);
			extendedSlider8.StepValue = 1.0;
			bindingExtension87.Mode = 1;
			bindingExtension87.Path = "LinearScaleSize";
			bindingExtension87.TypedBinding = new TypedBinding<DashboardItem, double>(delegate(DashboardItem A_0)
			{
				if (A_0 != null)
				{
					return new ValueTuple<double, bool>(A_0.LinearScaleSize, true);
				}
				return default(ValueTuple<double, bool>);
			}, delegate(DashboardItem A_0, double A_1)
			{
				if (A_0 != null)
				{
					A_0.LinearScaleSize = A_1;
					return;
				}
			}, new Tuple<Func<DashboardItem, object>, string>[]
			{
				new Tuple<Func<DashboardItem, object>, string>((DashboardItem A_0) => A_0, "LinearScaleSize")
			});
			BindingBase bindingBase87 = bindingExtension87.ProvideValue(null);
			extendedSlider8.SetBinding(Slider.ValueProperty, bindingBase87);
			grid16.Children.Add(extendedSlider8);
			stackLayout16.Children.Add(grid16);
			dashboardItemColorBox22.SetValue(Element.ClassIdProperty, "GaugeRimColor");
			dashboardItemColorBox22.Tapped += this.ColorBox_Tapped;
			translate78.Text = "ios_DashboardItemEditor_GaugeRimColor";
			IMarkupExtension markupExtension122 = translate78;
			XamlServiceProvider xamlServiceProvider122 = new XamlServiceProvider();
			Type typeFromHandle243 = typeof(IProvideValueTarget);
			object[] array122 = new object[0 + 8];
			array122[0] = dashboardItemColorBox22;
			array122[1] = stackLayout16;
			array122[2] = frame9;
			array122[3] = sfExpander7;
			array122[4] = stackLayout24;
			array122[5] = scrollView;
			array122[6] = grid25;
			array122[7] = this;
			object obj208;
			xamlServiceProvider122.Add(typeFromHandle243, obj208 = new SimpleValueTargetProvider(array122, DashboardItemColorBox.TextProperty, nameScope));
			xamlServiceProvider122.Add(typeof(IReferenceProvider), obj208);
			Type typeFromHandle244 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver122 = new XmlNamespaceResolver();
			xmlNamespaceResolver122.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver122.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver122.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver122.Add("dash", "clr-namespace:CarScannerXamarinForms.Dashboard");
			xmlNamespaceResolver122.Add("expander", "clr-namespace:Syncfusion.XForms.Expander;assembly=Syncfusion.Expander.XForms");
			xmlNamespaceResolver122.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver122.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver122.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver122.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider122.Add(typeFromHandle244, new XamlTypeResolver(xmlNamespaceResolver122, typeof(DashboardItemEditor).GetTypeInfo().Assembly));
			xamlServiceProvider122.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(967, 41)));
			object obj209 = markupExtension122.ProvideValue(xamlServiceProvider122);
			dashboardItemColorBox22.Text = obj209;
			bindingExtension88.Mode = 2;
			bindingExtension88.Path = "GaugeRimColor";
			bindingExtension88.TypedBinding = new TypedBinding<DashboardItem, Color>(delegate(DashboardItem A_0)
			{
				if (A_0 != null)
				{
					return new ValueTuple<Color, bool>(A_0.GaugeRimColor, true);
				}
				return default(ValueTuple<Color, bool>);
			}, null, new Tuple<Func<DashboardItem, object>, string>[]
			{
				new Tuple<Func<DashboardItem, object>, string>((DashboardItem A_0) => A_0, "GaugeRimColor")
			});
			BindingBase bindingBase88 = bindingExtension88.ProvideValue(null);
			dashboardItemColorBox22.SetBinding(DashboardItemColorBox.ColorProperty, bindingBase88);
			stackLayout16.Children.Add(dashboardItemColorBox22);
			dashboardItemColorBox23.SetValue(Element.ClassIdProperty, "GaugeLabelColor");
			dashboardItemColorBox23.Tapped += this.ColorBox_Tapped;
			translate79.Text = "ios_DashboardItemEditor_GaugeLabelsColor";
			IMarkupExtension markupExtension123 = translate79;
			XamlServiceProvider xamlServiceProvider123 = new XamlServiceProvider();
			Type typeFromHandle245 = typeof(IProvideValueTarget);
			object[] array123 = new object[0 + 8];
			array123[0] = dashboardItemColorBox23;
			array123[1] = stackLayout16;
			array123[2] = frame9;
			array123[3] = sfExpander7;
			array123[4] = stackLayout24;
			array123[5] = scrollView;
			array123[6] = grid25;
			array123[7] = this;
			object obj210;
			xamlServiceProvider123.Add(typeFromHandle245, obj210 = new SimpleValueTargetProvider(array123, DashboardItemColorBox.TextProperty, nameScope));
			xamlServiceProvider123.Add(typeof(IReferenceProvider), obj210);
			Type typeFromHandle246 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver123 = new XmlNamespaceResolver();
			xmlNamespaceResolver123.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver123.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver123.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver123.Add("dash", "clr-namespace:CarScannerXamarinForms.Dashboard");
			xmlNamespaceResolver123.Add("expander", "clr-namespace:Syncfusion.XForms.Expander;assembly=Syncfusion.Expander.XForms");
			xmlNamespaceResolver123.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver123.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver123.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver123.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider123.Add(typeFromHandle246, new XamlTypeResolver(xmlNamespaceResolver123, typeof(DashboardItemEditor).GetTypeInfo().Assembly));
			xamlServiceProvider123.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(975, 41)));
			object obj211 = markupExtension123.ProvideValue(xamlServiceProvider123);
			dashboardItemColorBox23.Text = obj211;
			bindingExtension89.Mode = 2;
			bindingExtension89.Path = "GaugeLabelColor";
			bindingExtension89.TypedBinding = new TypedBinding<DashboardItem, Color>(delegate(DashboardItem A_0)
			{
				if (A_0 != null)
				{
					return new ValueTuple<Color, bool>(A_0.GaugeLabelColor, true);
				}
				return default(ValueTuple<Color, bool>);
			}, null, new Tuple<Func<DashboardItem, object>, string>[]
			{
				new Tuple<Func<DashboardItem, object>, string>((DashboardItem A_0) => A_0, "GaugeLabelColor")
			});
			BindingBase bindingBase89 = bindingExtension89.ProvideValue(null);
			dashboardItemColorBox23.SetBinding(DashboardItemColorBox.ColorProperty, bindingBase89);
			stackLayout16.Children.Add(dashboardItemColorBox23);
			dashboardItemColorBox24.SetValue(Element.ClassIdProperty, "GaugePointerColor");
			dashboardItemColorBox24.Tapped += this.ColorBox_Tapped;
			translate80.Text = "ios_DashboardItemEditor_GaugePointerColor";
			IMarkupExtension markupExtension124 = translate80;
			XamlServiceProvider xamlServiceProvider124 = new XamlServiceProvider();
			Type typeFromHandle247 = typeof(IProvideValueTarget);
			object[] array124 = new object[0 + 8];
			array124[0] = dashboardItemColorBox24;
			array124[1] = stackLayout16;
			array124[2] = frame9;
			array124[3] = sfExpander7;
			array124[4] = stackLayout24;
			array124[5] = scrollView;
			array124[6] = grid25;
			array124[7] = this;
			object obj212;
			xamlServiceProvider124.Add(typeFromHandle247, obj212 = new SimpleValueTargetProvider(array124, DashboardItemColorBox.TextProperty, nameScope));
			xamlServiceProvider124.Add(typeof(IReferenceProvider), obj212);
			Type typeFromHandle248 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver124 = new XmlNamespaceResolver();
			xmlNamespaceResolver124.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver124.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver124.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver124.Add("dash", "clr-namespace:CarScannerXamarinForms.Dashboard");
			xmlNamespaceResolver124.Add("expander", "clr-namespace:Syncfusion.XForms.Expander;assembly=Syncfusion.Expander.XForms");
			xmlNamespaceResolver124.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver124.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver124.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver124.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider124.Add(typeFromHandle248, new XamlTypeResolver(xmlNamespaceResolver124, typeof(DashboardItemEditor).GetTypeInfo().Assembly));
			xamlServiceProvider124.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(982, 41)));
			object obj213 = markupExtension124.ProvideValue(xamlServiceProvider124);
			dashboardItemColorBox24.Text = obj213;
			bindingExtension90.Mode = 2;
			bindingExtension90.Path = "GaugePointerColor";
			bindingExtension90.TypedBinding = new TypedBinding<DashboardItem, Color>(delegate(DashboardItem A_0)
			{
				if (A_0 != null)
				{
					return new ValueTuple<Color, bool>(A_0.GaugePointerColor, true);
				}
				return default(ValueTuple<Color, bool>);
			}, null, new Tuple<Func<DashboardItem, object>, string>[]
			{
				new Tuple<Func<DashboardItem, object>, string>((DashboardItem A_0) => A_0, "GaugePointerColor")
			});
			BindingBase bindingBase90 = bindingExtension90.ProvideValue(null);
			dashboardItemColorBox24.SetBinding(DashboardItemColorBox.ColorProperty, bindingBase90);
			stackLayout16.Children.Add(dashboardItemColorBox24);
			bindingExtension91.Mode = 1;
			bindingExtension91.Path = "ShowValue";
			bindingExtension91.TypedBinding = new TypedBinding<DashboardItem, bool>(delegate(DashboardItem A_0)
			{
				if (A_0 != null)
				{
					return new ValueTuple<bool, bool>(A_0.ShowValue, true);
				}
				return default(ValueTuple<bool, bool>);
			}, delegate(DashboardItem A_0, bool A_1)
			{
				if (A_0 != null)
				{
					A_0.ShowValue = A_1;
					return;
				}
			}, new Tuple<Func<DashboardItem, object>, string>[]
			{
				new Tuple<Func<DashboardItem, object>, string>((DashboardItem A_0) => A_0, "ShowValue")
			});
			BindingBase bindingBase91 = bindingExtension91.ProvideValue(null);
			labelSwitch10.SetBinding(LabelSwitch.IsToggledProperty, bindingBase91);
			translate81.Text = "ios_ShowValue";
			IMarkupExtension markupExtension125 = translate81;
			XamlServiceProvider xamlServiceProvider125 = new XamlServiceProvider();
			Type typeFromHandle249 = typeof(IProvideValueTarget);
			object[] array125 = new object[0 + 8];
			array125[0] = labelSwitch10;
			array125[1] = stackLayout16;
			array125[2] = frame9;
			array125[3] = sfExpander7;
			array125[4] = stackLayout24;
			array125[5] = scrollView;
			array125[6] = grid25;
			array125[7] = this;
			object obj214;
			xamlServiceProvider125.Add(typeFromHandle249, obj214 = new SimpleValueTargetProvider(array125, LabelSwitch.TextProperty, nameScope));
			xamlServiceProvider125.Add(typeof(IReferenceProvider), obj214);
			Type typeFromHandle250 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver125 = new XmlNamespaceResolver();
			xmlNamespaceResolver125.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver125.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver125.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver125.Add("dash", "clr-namespace:CarScannerXamarinForms.Dashboard");
			xmlNamespaceResolver125.Add("expander", "clr-namespace:Syncfusion.XForms.Expander;assembly=Syncfusion.Expander.XForms");
			xmlNamespaceResolver125.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver125.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver125.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver125.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider125.Add(typeFromHandle250, new XamlTypeResolver(xmlNamespaceResolver125, typeof(DashboardItemEditor).GetTypeInfo().Assembly));
			xamlServiceProvider125.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(985, 101)));
			object obj215 = markupExtension125.ProvideValue(xamlServiceProvider125);
			labelSwitch10.Text = obj215;
			stackLayout16.Children.Add(labelSwitch10);
			on3.Platform = new List<string>(1) { "iOS" };
			on3.Value = "false";
			onPlatform4.Platforms.Add(on3);
			on4.Platform = new List<string>(1) { "Android" };
			on4.Value = "true";
			onPlatform4.Platforms.Add(on4);
			grid17.SetValue(VisualElement.IsVisibleProperty, onPlatform4);
			columnDefinition21.SetValue(ColumnDefinition.WidthProperty, new GridLengthTypeConverter().ConvertFromInvariantString("*"));
			grid17.GetValue(Grid.ColumnDefinitionsProperty).Add(columnDefinition21);
			columnDefinition22.SetValue(ColumnDefinition.WidthProperty, new GridLengthTypeConverter().ConvertFromInvariantString("auto"));
			grid17.GetValue(Grid.ColumnDefinitionsProperty).Add(columnDefinition22);
			label51.SetValue(Grid.ColumnProperty, 0);
			bindingExtension92.Mode = 1;
			bindingExtension92.Path = "LinearOrientationHorizontal";
			bindingExtension92.TypedBinding = new TypedBinding<DashboardItem, bool>(delegate(DashboardItem A_0)
			{
				if (A_0 != null)
				{
					return new ValueTuple<bool, bool>(A_0.LinearOrientationHorizontal, true);
				}
				return default(ValueTuple<bool, bool>);
			}, delegate(DashboardItem A_0, bool A_1)
			{
				if (A_0 != null)
				{
					A_0.LinearOrientationHorizontal = A_1;
					return;
				}
			}, new Tuple<Func<DashboardItem, object>, string>[]
			{
				new Tuple<Func<DashboardItem, object>, string>((DashboardItem A_0) => A_0, "LinearOrientationHorizontal")
			});
			BindingBase bindingBase92 = bindingExtension92.ProvideValue(null);
			label51.SetBinding(VisualElement.IsVisibleProperty, bindingBase92);
			translate82.Text = "ios_LinearOrientation_Horizontal";
			IMarkupExtension markupExtension126 = translate82;
			XamlServiceProvider xamlServiceProvider126 = new XamlServiceProvider();
			Type typeFromHandle251 = typeof(IProvideValueTarget);
			object[] array126 = new object[0 + 9];
			array126[0] = label51;
			array126[1] = grid17;
			array126[2] = stackLayout16;
			array126[3] = frame9;
			array126[4] = sfExpander7;
			array126[5] = stackLayout24;
			array126[6] = scrollView;
			array126[7] = grid25;
			array126[8] = this;
			object obj216;
			xamlServiceProvider126.Add(typeFromHandle251, obj216 = new SimpleValueTargetProvider(array126, Label.TextProperty, nameScope));
			xamlServiceProvider126.Add(typeof(IReferenceProvider), obj216);
			Type typeFromHandle252 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver126 = new XmlNamespaceResolver();
			xmlNamespaceResolver126.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver126.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver126.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver126.Add("dash", "clr-namespace:CarScannerXamarinForms.Dashboard");
			xmlNamespaceResolver126.Add("expander", "clr-namespace:Syncfusion.XForms.Expander;assembly=Syncfusion.Expander.XForms");
			xmlNamespaceResolver126.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver126.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver126.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver126.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider126.Add(typeFromHandle252, new XamlTypeResolver(xmlNamespaceResolver126, typeof(DashboardItemEditor).GetTypeInfo().Assembly));
			xamlServiceProvider126.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(1003, 45)));
			object obj217 = markupExtension126.ProvideValue(xamlServiceProvider126);
			label51.Text = obj217;
			label51.SetValue(View.VerticalOptionsProperty, LayoutOptions.Center);
			grid17.Children.Add(label51);
			label52.SetValue(Grid.ColumnProperty, 0);
			bindingExtension93.Mode = 1;
			staticResourceExtension10.Key = "BoolToNegativeConverter";
			IMarkupExtension markupExtension127 = staticResourceExtension10;
			XamlServiceProvider xamlServiceProvider127 = new XamlServiceProvider();
			Type typeFromHandle253 = typeof(IProvideValueTarget);
			object[] array127 = new object[0 + 10];
			array127[0] = bindingExtension93;
			array127[1] = label52;
			array127[2] = grid17;
			array127[3] = stackLayout16;
			array127[4] = frame9;
			array127[5] = sfExpander7;
			array127[6] = stackLayout24;
			array127[7] = scrollView;
			array127[8] = grid25;
			array127[9] = this;
			object obj218;
			xamlServiceProvider127.Add(typeFromHandle253, obj218 = new SimpleValueTargetProvider(array127, typeof(BindingExtension).GetRuntimeProperty("Converter"), nameScope));
			xamlServiceProvider127.Add(typeof(IReferenceProvider), obj218);
			Type typeFromHandle254 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver127 = new XmlNamespaceResolver();
			xmlNamespaceResolver127.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver127.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver127.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver127.Add("dash", "clr-namespace:CarScannerXamarinForms.Dashboard");
			xmlNamespaceResolver127.Add("expander", "clr-namespace:Syncfusion.XForms.Expander;assembly=Syncfusion.Expander.XForms");
			xmlNamespaceResolver127.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver127.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver127.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver127.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider127.Add(typeFromHandle254, new XamlTypeResolver(xmlNamespaceResolver127, typeof(DashboardItemEditor).GetTypeInfo().Assembly));
			xamlServiceProvider127.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(1007, 45)));
			object obj219 = markupExtension127.ProvideValue(xamlServiceProvider127);
			bindingExtension93.Converter = obj219;
			bindingExtension93.Path = "LinearOrientationHorizontal";
			bindingExtension93.TypedBinding = new TypedBinding<DashboardItem, bool>(delegate(DashboardItem A_0)
			{
				if (A_0 != null)
				{
					return new ValueTuple<bool, bool>(A_0.LinearOrientationHorizontal, true);
				}
				return default(ValueTuple<bool, bool>);
			}, delegate(DashboardItem A_0, bool A_1)
			{
				if (A_0 != null)
				{
					A_0.LinearOrientationHorizontal = A_1;
					return;
				}
			}, new Tuple<Func<DashboardItem, object>, string>[]
			{
				new Tuple<Func<DashboardItem, object>, string>((DashboardItem A_0) => A_0, "LinearOrientationHorizontal")
			});
			BindingBase bindingBase93 = bindingExtension93.ProvideValue(null);
			label52.SetBinding(VisualElement.IsVisibleProperty, bindingBase93);
			translate83.Text = "ios_LinearOrientation_Vertical";
			IMarkupExtension markupExtension128 = translate83;
			XamlServiceProvider xamlServiceProvider128 = new XamlServiceProvider();
			Type typeFromHandle255 = typeof(IProvideValueTarget);
			object[] array128 = new object[0 + 9];
			array128[0] = label52;
			array128[1] = grid17;
			array128[2] = stackLayout16;
			array128[3] = frame9;
			array128[4] = sfExpander7;
			array128[5] = stackLayout24;
			array128[6] = scrollView;
			array128[7] = grid25;
			array128[8] = this;
			object obj220;
			xamlServiceProvider128.Add(typeFromHandle255, obj220 = new SimpleValueTargetProvider(array128, Label.TextProperty, nameScope));
			xamlServiceProvider128.Add(typeof(IReferenceProvider), obj220);
			Type typeFromHandle256 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver128 = new XmlNamespaceResolver();
			xmlNamespaceResolver128.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver128.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver128.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver128.Add("dash", "clr-namespace:CarScannerXamarinForms.Dashboard");
			xmlNamespaceResolver128.Add("expander", "clr-namespace:Syncfusion.XForms.Expander;assembly=Syncfusion.Expander.XForms");
			xmlNamespaceResolver128.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver128.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver128.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver128.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider128.Add(typeFromHandle256, new XamlTypeResolver(xmlNamespaceResolver128, typeof(DashboardItemEditor).GetTypeInfo().Assembly));
			xamlServiceProvider128.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(1008, 45)));
			object obj221 = markupExtension128.ProvideValue(xamlServiceProvider128);
			label52.Text = obj221;
			label52.SetValue(View.VerticalOptionsProperty, LayoutOptions.Center);
			grid17.Children.Add(label52);
			@switch.SetValue(Grid.ColumnProperty, 1);
			@switch.SetValue(View.HorizontalOptionsProperty, LayoutOptions.End);
			bindingExtension94.Mode = 1;
			bindingExtension94.Path = "LinearOrientationHorizontal";
			bindingExtension94.TypedBinding = new TypedBinding<DashboardItem, bool>(delegate(DashboardItem A_0)
			{
				if (A_0 != null)
				{
					return new ValueTuple<bool, bool>(A_0.LinearOrientationHorizontal, true);
				}
				return default(ValueTuple<bool, bool>);
			}, delegate(DashboardItem A_0, bool A_1)
			{
				if (A_0 != null)
				{
					A_0.LinearOrientationHorizontal = A_1;
					return;
				}
			}, new Tuple<Func<DashboardItem, object>, string>[]
			{
				new Tuple<Func<DashboardItem, object>, string>((DashboardItem A_0) => A_0, "LinearOrientationHorizontal")
			});
			BindingBase bindingBase94 = bindingExtension94.ProvideValue(null);
			@switch.SetBinding(Switch.IsToggledProperty, bindingBase94);
			grid17.Children.Add(@switch);
			stackLayout16.Children.Add(grid17);
			frame9.SetValue(ContentView.ContentProperty, stackLayout16);
			sfExpander7.SetValue(SfExpander.ContentProperty, frame9);
			stackLayout24.Children.Add(sfExpander7);
			sfExpander8.SetValue(SfExpander.AnimationDurationProperty, 250.0);
			sfExpander8.SetValue(SfExpander.AnimationEasingProperty, 3);
			dynamicResourceExtension34.Key = "NavigationBarBackgroundColor";
			IMarkupExtension<DynamicResource> markupExtension129 = dynamicResourceExtension34;
			XamlServiceProvider xamlServiceProvider129 = new XamlServiceProvider();
			Type typeFromHandle257 = typeof(IProvideValueTarget);
			object[] array129 = new object[0 + 5];
			array129[0] = sfExpander8;
			array129[1] = stackLayout24;
			array129[2] = scrollView;
			array129[3] = grid25;
			array129[4] = this;
			object obj222;
			xamlServiceProvider129.Add(typeFromHandle257, obj222 = new SimpleValueTargetProvider(array129, SfExpander.HeaderBackgroundColorProperty, nameScope));
			xamlServiceProvider129.Add(typeof(IReferenceProvider), obj222);
			Type typeFromHandle258 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver129 = new XmlNamespaceResolver();
			xmlNamespaceResolver129.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver129.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver129.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver129.Add("dash", "clr-namespace:CarScannerXamarinForms.Dashboard");
			xmlNamespaceResolver129.Add("expander", "clr-namespace:Syncfusion.XForms.Expander;assembly=Syncfusion.Expander.XForms");
			xmlNamespaceResolver129.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver129.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver129.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver129.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider129.Add(typeFromHandle258, new XamlTypeResolver(xmlNamespaceResolver129, typeof(DashboardItemEditor).GetTypeInfo().Assembly));
			xamlServiceProvider129.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(1027, 25)));
			DynamicResource dynamicResource34 = markupExtension129.ProvideValue(xamlServiceProvider129);
			sfExpander8.SetDynamicResource(SfExpander.HeaderBackgroundColorProperty, dynamicResource34.Key);
			dynamicResourceExtension35.Key = "NavigationBarButtonColor";
			IMarkupExtension<DynamicResource> markupExtension130 = dynamicResourceExtension35;
			XamlServiceProvider xamlServiceProvider130 = new XamlServiceProvider();
			Type typeFromHandle259 = typeof(IProvideValueTarget);
			object[] array130 = new object[0 + 5];
			array130[0] = sfExpander8;
			array130[1] = stackLayout24;
			array130[2] = scrollView;
			array130[3] = grid25;
			array130[4] = this;
			object obj223;
			xamlServiceProvider130.Add(typeFromHandle259, obj223 = new SimpleValueTargetProvider(array130, SfExpander.IconColorProperty, nameScope));
			xamlServiceProvider130.Add(typeof(IReferenceProvider), obj223);
			Type typeFromHandle260 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver130 = new XmlNamespaceResolver();
			xmlNamespaceResolver130.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver130.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver130.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver130.Add("dash", "clr-namespace:CarScannerXamarinForms.Dashboard");
			xmlNamespaceResolver130.Add("expander", "clr-namespace:Syncfusion.XForms.Expander;assembly=Syncfusion.Expander.XForms");
			xmlNamespaceResolver130.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver130.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver130.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver130.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider130.Add(typeFromHandle260, new XamlTypeResolver(xmlNamespaceResolver130, typeof(DashboardItemEditor).GetTypeInfo().Assembly));
			xamlServiceProvider130.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(1028, 25)));
			DynamicResource dynamicResource35 = markupExtension130.ProvideValue(xamlServiceProvider130);
			sfExpander8.SetDynamicResource(SfExpander.IconColorProperty, dynamicResource35.Key);
			sfExpander8.SetValue(SfExpander.IsExpandedProperty, false);
			label53.SetValue(View.MarginProperty, new Thickness(5.0));
			label53.SetValue(Label.FontAttributesProperty, new FontAttributesConverter().ConvertFromInvariantString("Bold"));
			translate84.Text = "Dashboard_MinMaxAvg";
			IMarkupExtension markupExtension131 = translate84;
			XamlServiceProvider xamlServiceProvider131 = new XamlServiceProvider();
			Type typeFromHandle261 = typeof(IProvideValueTarget);
			object[] array131 = new object[0 + 7];
			array131[0] = label53;
			array131[1] = grid18;
			array131[2] = sfExpander8;
			array131[3] = stackLayout24;
			array131[4] = scrollView;
			array131[5] = grid25;
			array131[6] = this;
			object obj224;
			xamlServiceProvider131.Add(typeFromHandle261, obj224 = new SimpleValueTargetProvider(array131, Label.TextProperty, nameScope));
			xamlServiceProvider131.Add(typeof(IReferenceProvider), obj224);
			Type typeFromHandle262 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver131 = new XmlNamespaceResolver();
			xmlNamespaceResolver131.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver131.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver131.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver131.Add("dash", "clr-namespace:CarScannerXamarinForms.Dashboard");
			xmlNamespaceResolver131.Add("expander", "clr-namespace:Syncfusion.XForms.Expander;assembly=Syncfusion.Expander.XForms");
			xmlNamespaceResolver131.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver131.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver131.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver131.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider131.Add(typeFromHandle262, new XamlTypeResolver(xmlNamespaceResolver131, typeof(DashboardItemEditor).GetTypeInfo().Assembly));
			xamlServiceProvider131.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(1035, 37)));
			object obj225 = markupExtension131.ProvideValue(xamlServiceProvider131);
			label53.Text = obj225;
			dynamicResourceExtension36.Key = "NavigationBarTextColor";
			IMarkupExtension<DynamicResource> markupExtension132 = dynamicResourceExtension36;
			XamlServiceProvider xamlServiceProvider132 = new XamlServiceProvider();
			Type typeFromHandle263 = typeof(IProvideValueTarget);
			object[] array132 = new object[0 + 7];
			array132[0] = label53;
			array132[1] = grid18;
			array132[2] = sfExpander8;
			array132[3] = stackLayout24;
			array132[4] = scrollView;
			array132[5] = grid25;
			array132[6] = this;
			object obj226;
			xamlServiceProvider132.Add(typeFromHandle263, obj226 = new SimpleValueTargetProvider(array132, Label.TextColorProperty, nameScope));
			xamlServiceProvider132.Add(typeof(IReferenceProvider), obj226);
			Type typeFromHandle264 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver132 = new XmlNamespaceResolver();
			xmlNamespaceResolver132.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver132.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver132.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver132.Add("dash", "clr-namespace:CarScannerXamarinForms.Dashboard");
			xmlNamespaceResolver132.Add("expander", "clr-namespace:Syncfusion.XForms.Expander;assembly=Syncfusion.Expander.XForms");
			xmlNamespaceResolver132.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver132.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver132.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver132.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider132.Add(typeFromHandle264, new XamlTypeResolver(xmlNamespaceResolver132, typeof(DashboardItemEditor).GetTypeInfo().Assembly));
			xamlServiceProvider132.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(1036, 37)));
			DynamicResource dynamicResource36 = markupExtension132.ProvideValue(xamlServiceProvider132);
			label53.SetDynamicResource(Label.TextColorProperty, dynamicResource36.Key);
			label53.SetValue(View.VerticalOptionsProperty, LayoutOptions.Center);
			label53.SetValue(Label.VerticalTextAlignmentProperty, new TextAlignmentConverter().ConvertFromInvariantString("Center"));
			grid18.Children.Add(label53);
			sfExpander8.SetValue(SfExpander.HeaderProperty, grid18);
			stackLayout17.SetValue(StackLayout.OrientationProperty, 0);
			bindingExtension95.Mode = 1;
			bindingExtension95.Path = "ShowMinMax";
			bindingExtension95.TypedBinding = new TypedBinding<DashboardItem, bool>(delegate(DashboardItem A_0)
			{
				if (A_0 != null)
				{
					return new ValueTuple<bool, bool>(A_0.ShowMinMax, true);
				}
				return default(ValueTuple<bool, bool>);
			}, delegate(DashboardItem A_0, bool A_1)
			{
				if (A_0 != null)
				{
					A_0.ShowMinMax = A_1;
					return;
				}
			}, new Tuple<Func<DashboardItem, object>, string>[]
			{
				new Tuple<Func<DashboardItem, object>, string>((DashboardItem A_0) => A_0, "ShowMinMax")
			});
			BindingBase bindingBase95 = bindingExtension95.ProvideValue(null);
			labelSwitch11.SetBinding(LabelSwitch.IsToggledProperty, bindingBase95);
			translate85.Text = "Dashboard_DisplayMinMax";
			IMarkupExtension markupExtension133 = translate85;
			XamlServiceProvider xamlServiceProvider133 = new XamlServiceProvider();
			Type typeFromHandle265 = typeof(IProvideValueTarget);
			object[] array133 = new object[0 + 8];
			array133[0] = labelSwitch11;
			array133[1] = stackLayout17;
			array133[2] = frame10;
			array133[3] = sfExpander8;
			array133[4] = stackLayout24;
			array133[5] = scrollView;
			array133[6] = grid25;
			array133[7] = this;
			object obj227;
			xamlServiceProvider133.Add(typeFromHandle265, obj227 = new SimpleValueTargetProvider(array133, LabelSwitch.TextProperty, nameScope));
			xamlServiceProvider133.Add(typeof(IReferenceProvider), obj227);
			Type typeFromHandle266 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver133 = new XmlNamespaceResolver();
			xmlNamespaceResolver133.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver133.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver133.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver133.Add("dash", "clr-namespace:CarScannerXamarinForms.Dashboard");
			xmlNamespaceResolver133.Add("expander", "clr-namespace:Syncfusion.XForms.Expander;assembly=Syncfusion.Expander.XForms");
			xmlNamespaceResolver133.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver133.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver133.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver133.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider133.Add(typeFromHandle266, new XamlTypeResolver(xmlNamespaceResolver133, typeof(DashboardItemEditor).GetTypeInfo().Assembly));
			xamlServiceProvider133.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(1047, 102)));
			object obj228 = markupExtension133.ProvideValue(xamlServiceProvider133);
			labelSwitch11.Text = obj228;
			stackLayout17.Children.Add(labelSwitch11);
			bindingExtension96.Mode = 1;
			bindingExtension96.Path = "ShowAvg";
			bindingExtension96.TypedBinding = new TypedBinding<DashboardItem, bool>(delegate(DashboardItem A_0)
			{
				if (A_0 != null)
				{
					return new ValueTuple<bool, bool>(A_0.ShowAvg, true);
				}
				return default(ValueTuple<bool, bool>);
			}, delegate(DashboardItem A_0, bool A_1)
			{
				if (A_0 != null)
				{
					A_0.ShowAvg = A_1;
					return;
				}
			}, new Tuple<Func<DashboardItem, object>, string>[]
			{
				new Tuple<Func<DashboardItem, object>, string>((DashboardItem A_0) => A_0, "ShowAvg")
			});
			BindingBase bindingBase96 = bindingExtension96.ProvideValue(null);
			labelSwitch12.SetBinding(LabelSwitch.IsToggledProperty, bindingBase96);
			translate86.Text = "Dashboard_DisplayAvg";
			IMarkupExtension markupExtension134 = translate86;
			XamlServiceProvider xamlServiceProvider134 = new XamlServiceProvider();
			Type typeFromHandle267 = typeof(IProvideValueTarget);
			object[] array134 = new object[0 + 8];
			array134[0] = labelSwitch12;
			array134[1] = stackLayout17;
			array134[2] = frame10;
			array134[3] = sfExpander8;
			array134[4] = stackLayout24;
			array134[5] = scrollView;
			array134[6] = grid25;
			array134[7] = this;
			object obj229;
			xamlServiceProvider134.Add(typeFromHandle267, obj229 = new SimpleValueTargetProvider(array134, LabelSwitch.TextProperty, nameScope));
			xamlServiceProvider134.Add(typeof(IReferenceProvider), obj229);
			Type typeFromHandle268 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver134 = new XmlNamespaceResolver();
			xmlNamespaceResolver134.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver134.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver134.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver134.Add("dash", "clr-namespace:CarScannerXamarinForms.Dashboard");
			xmlNamespaceResolver134.Add("expander", "clr-namespace:Syncfusion.XForms.Expander;assembly=Syncfusion.Expander.XForms");
			xmlNamespaceResolver134.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver134.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver134.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver134.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider134.Add(typeFromHandle268, new XamlTypeResolver(xmlNamespaceResolver134, typeof(DashboardItemEditor).GetTypeInfo().Assembly));
			xamlServiceProvider134.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(1048, 99)));
			object obj230 = markupExtension134.ProvideValue(xamlServiceProvider134);
			labelSwitch12.Text = obj230;
			stackLayout17.Children.Add(labelSwitch12);
			dashboardItemColorBox25.SetValue(Element.ClassIdProperty, "MinMaxAvgColor");
			dashboardItemColorBox25.Tapped += this.ColorBox_Tapped;
			translate87.Text = "Dashboard_MinMaxAvgColor";
			IMarkupExtension markupExtension135 = translate87;
			XamlServiceProvider xamlServiceProvider135 = new XamlServiceProvider();
			Type typeFromHandle269 = typeof(IProvideValueTarget);
			object[] array135 = new object[0 + 8];
			array135[0] = dashboardItemColorBox25;
			array135[1] = stackLayout17;
			array135[2] = frame10;
			array135[3] = sfExpander8;
			array135[4] = stackLayout24;
			array135[5] = scrollView;
			array135[6] = grid25;
			array135[7] = this;
			object obj231;
			xamlServiceProvider135.Add(typeFromHandle269, obj231 = new SimpleValueTargetProvider(array135, DashboardItemColorBox.TextProperty, nameScope));
			xamlServiceProvider135.Add(typeof(IReferenceProvider), obj231);
			Type typeFromHandle270 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver135 = new XmlNamespaceResolver();
			xmlNamespaceResolver135.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver135.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver135.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver135.Add("dash", "clr-namespace:CarScannerXamarinForms.Dashboard");
			xmlNamespaceResolver135.Add("expander", "clr-namespace:Syncfusion.XForms.Expander;assembly=Syncfusion.Expander.XForms");
			xmlNamespaceResolver135.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver135.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver135.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver135.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider135.Add(typeFromHandle270, new XamlTypeResolver(xmlNamespaceResolver135, typeof(DashboardItemEditor).GetTypeInfo().Assembly));
			xamlServiceProvider135.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(1052, 41)));
			object obj232 = markupExtension135.ProvideValue(xamlServiceProvider135);
			dashboardItemColorBox25.Text = obj232;
			bindingExtension97.Mode = 2;
			bindingExtension97.Path = "MinMaxAvgColor";
			bindingExtension97.TypedBinding = new TypedBinding<DashboardItem, Color>(delegate(DashboardItem A_0)
			{
				if (A_0 != null)
				{
					return new ValueTuple<Color, bool>(A_0.MinMaxAvgColor, true);
				}
				return default(ValueTuple<Color, bool>);
			}, null, new Tuple<Func<DashboardItem, object>, string>[]
			{
				new Tuple<Func<DashboardItem, object>, string>((DashboardItem A_0) => A_0, "MinMaxAvgColor")
			});
			BindingBase bindingBase97 = bindingExtension97.ProvideValue(null);
			dashboardItemColorBox25.SetBinding(DashboardItemColorBox.ColorProperty, bindingBase97);
			stackLayout17.Children.Add(dashboardItemColorBox25);
			grid19.SetValue(View.MarginProperty, new Thickness(0.0, 0.0, 0.0, 10.0));
			grid19.SetValue(Grid.RowSpacingProperty, 0.0);
			columnDefinition23.SetValue(ColumnDefinition.WidthProperty, new GridLengthTypeConverter().ConvertFromInvariantString("*"));
			grid19.GetValue(Grid.ColumnDefinitionsProperty).Add(columnDefinition23);
			columnDefinition24.SetValue(ColumnDefinition.WidthProperty, new GridLengthTypeConverter().ConvertFromInvariantString("auto"));
			grid19.GetValue(Grid.ColumnDefinitionsProperty).Add(columnDefinition24);
			rowDefinition17.SetValue(RowDefinition.HeightProperty, new GridLengthTypeConverter().ConvertFromInvariantString("auto"));
			grid19.GetValue(Grid.RowDefinitionsProperty).Add(rowDefinition17);
			rowDefinition18.SetValue(RowDefinition.HeightProperty, new GridLengthTypeConverter().ConvertFromInvariantString("auto"));
			grid19.GetValue(Grid.RowDefinitionsProperty).Add(rowDefinition18);
			label54.SetValue(Grid.RowProperty, 0);
			label54.SetValue(Grid.ColumnProperty, 0);
			translate88.Text = "Dashboard_MinMaxAvgFontSize";
			IMarkupExtension markupExtension136 = translate88;
			XamlServiceProvider xamlServiceProvider136 = new XamlServiceProvider();
			Type typeFromHandle271 = typeof(IProvideValueTarget);
			object[] array136 = new object[0 + 9];
			array136[0] = label54;
			array136[1] = grid19;
			array136[2] = stackLayout17;
			array136[3] = frame10;
			array136[4] = sfExpander8;
			array136[5] = stackLayout24;
			array136[6] = scrollView;
			array136[7] = grid25;
			array136[8] = this;
			object obj233;
			xamlServiceProvider136.Add(typeFromHandle271, obj233 = new SimpleValueTargetProvider(array136, Label.TextProperty, nameScope));
			xamlServiceProvider136.Add(typeof(IReferenceProvider), obj233);
			Type typeFromHandle272 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver136 = new XmlNamespaceResolver();
			xmlNamespaceResolver136.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver136.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver136.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver136.Add("dash", "clr-namespace:CarScannerXamarinForms.Dashboard");
			xmlNamespaceResolver136.Add("expander", "clr-namespace:Syncfusion.XForms.Expander;assembly=Syncfusion.Expander.XForms");
			xmlNamespaceResolver136.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver136.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver136.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver136.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider136.Add(typeFromHandle272, new XamlTypeResolver(xmlNamespaceResolver136, typeof(DashboardItemEditor).GetTypeInfo().Assembly));
			xamlServiceProvider136.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(1068, 45)));
			object obj234 = markupExtension136.ProvideValue(xamlServiceProvider136);
			label54.Text = obj234;
			grid19.Children.Add(label54);
			label55.SetValue(Grid.RowProperty, 0);
			label55.SetValue(Grid.ColumnProperty, 1);
			label55.SetValue(Label.FontAttributesProperty, new FontAttributesConverter().ConvertFromInvariantString("Bold"));
			bindingExtension98.Mode = 2;
			bindingExtension98.Path = "MinMaxAvgFontSize";
			bindingExtension98.TypedBinding = new TypedBinding<DashboardItem, double>(delegate(DashboardItem A_0)
			{
				if (A_0 != null)
				{
					return new ValueTuple<double, bool>(A_0.MinMaxAvgFontSize, true);
				}
				return default(ValueTuple<double, bool>);
			}, null, new Tuple<Func<DashboardItem, object>, string>[]
			{
				new Tuple<Func<DashboardItem, object>, string>((DashboardItem A_0) => A_0, "MinMaxAvgFontSize")
			});
			BindingBase bindingBase98 = bindingExtension98.ProvideValue(null);
			label55.SetBinding(Label.TextProperty, bindingBase98);
			label55.SetValue(View.VerticalOptionsProperty, LayoutOptions.Center);
			grid19.Children.Add(label55);
			extendedSlider9.SetValue(Grid.RowProperty, 1);
			extendedSlider9.SetValue(Grid.ColumnProperty, 0);
			extendedSlider9.SetValue(Grid.ColumnSpanProperty, 2);
			extendedSlider9.SetValue(Slider.MaximumProperty, 200.0);
			extendedSlider9.SetValue(Slider.MinimumProperty, 0.0);
			extendedSlider9.StepValue = 4.0;
			bindingExtension99.Mode = 1;
			bindingExtension99.Path = "MinMaxAvgFontSize";
			bindingExtension99.TypedBinding = new TypedBinding<DashboardItem, double>(delegate(DashboardItem A_0)
			{
				if (A_0 != null)
				{
					return new ValueTuple<double, bool>(A_0.MinMaxAvgFontSize, true);
				}
				return default(ValueTuple<double, bool>);
			}, delegate(DashboardItem A_0, double A_1)
			{
				if (A_0 != null)
				{
					A_0.MinMaxAvgFontSize = A_1;
					return;
				}
			}, new Tuple<Func<DashboardItem, object>, string>[]
			{
				new Tuple<Func<DashboardItem, object>, string>((DashboardItem A_0) => A_0, "MinMaxAvgFontSize")
			});
			BindingBase bindingBase99 = bindingExtension99.ProvideValue(null);
			extendedSlider9.SetBinding(Slider.ValueProperty, bindingBase99);
			grid19.Children.Add(extendedSlider9);
			stackLayout17.Children.Add(grid19);
			bindingExtension100.Mode = 1;
			bindingExtension100.Path = "ShowMinMaxPointers";
			bindingExtension100.TypedBinding = new TypedBinding<DashboardItem, bool>(delegate(DashboardItem A_0)
			{
				if (A_0 != null)
				{
					return new ValueTuple<bool, bool>(A_0.ShowMinMaxPointers, true);
				}
				return default(ValueTuple<bool, bool>);
			}, delegate(DashboardItem A_0, bool A_1)
			{
				if (A_0 != null)
				{
					A_0.ShowMinMaxPointers = A_1;
					return;
				}
			}, new Tuple<Func<DashboardItem, object>, string>[]
			{
				new Tuple<Func<DashboardItem, object>, string>((DashboardItem A_0) => A_0, "ShowMinMaxPointers")
			});
			BindingBase bindingBase100 = bindingExtension100.ProvideValue(null);
			labelSwitch13.SetBinding(LabelSwitch.IsToggledProperty, bindingBase100);
			translate89.Text = "Dashboard_DisplayMinMaxPointers";
			IMarkupExtension markupExtension137 = translate89;
			XamlServiceProvider xamlServiceProvider137 = new XamlServiceProvider();
			Type typeFromHandle273 = typeof(IProvideValueTarget);
			object[] array137 = new object[0 + 8];
			array137[0] = labelSwitch13;
			array137[1] = stackLayout17;
			array137[2] = frame10;
			array137[3] = sfExpander8;
			array137[4] = stackLayout24;
			array137[5] = scrollView;
			array137[6] = grid25;
			array137[7] = this;
			object obj235;
			xamlServiceProvider137.Add(typeFromHandle273, obj235 = new SimpleValueTargetProvider(array137, LabelSwitch.TextProperty, nameScope));
			xamlServiceProvider137.Add(typeof(IReferenceProvider), obj235);
			Type typeFromHandle274 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver137 = new XmlNamespaceResolver();
			xmlNamespaceResolver137.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver137.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver137.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver137.Add("dash", "clr-namespace:CarScannerXamarinForms.Dashboard");
			xmlNamespaceResolver137.Add("expander", "clr-namespace:Syncfusion.XForms.Expander;assembly=Syncfusion.Expander.XForms");
			xmlNamespaceResolver137.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver137.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver137.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver137.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider137.Add(typeFromHandle274, new XamlTypeResolver(xmlNamespaceResolver137, typeof(DashboardItemEditor).GetTypeInfo().Assembly));
			xamlServiceProvider137.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(1087, 110)));
			object obj236 = markupExtension137.ProvideValue(xamlServiceProvider137);
			labelSwitch13.Text = obj236;
			stackLayout17.Children.Add(labelSwitch13);
			dashboardItemColorBox26.SetValue(Element.ClassIdProperty, "MinMaxPointersColor");
			dashboardItemColorBox26.Tapped += this.ColorBox_Tapped;
			translate90.Text = "Dashboard_MinMaxPointersColor";
			IMarkupExtension markupExtension138 = translate90;
			XamlServiceProvider xamlServiceProvider138 = new XamlServiceProvider();
			Type typeFromHandle275 = typeof(IProvideValueTarget);
			object[] array138 = new object[0 + 8];
			array138[0] = dashboardItemColorBox26;
			array138[1] = stackLayout17;
			array138[2] = frame10;
			array138[3] = sfExpander8;
			array138[4] = stackLayout24;
			array138[5] = scrollView;
			array138[6] = grid25;
			array138[7] = this;
			object obj237;
			xamlServiceProvider138.Add(typeFromHandle275, obj237 = new SimpleValueTargetProvider(array138, DashboardItemColorBox.TextProperty, nameScope));
			xamlServiceProvider138.Add(typeof(IReferenceProvider), obj237);
			Type typeFromHandle276 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver138 = new XmlNamespaceResolver();
			xmlNamespaceResolver138.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver138.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver138.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver138.Add("dash", "clr-namespace:CarScannerXamarinForms.Dashboard");
			xmlNamespaceResolver138.Add("expander", "clr-namespace:Syncfusion.XForms.Expander;assembly=Syncfusion.Expander.XForms");
			xmlNamespaceResolver138.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver138.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver138.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver138.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider138.Add(typeFromHandle276, new XamlTypeResolver(xmlNamespaceResolver138, typeof(DashboardItemEditor).GetTypeInfo().Assembly));
			xamlServiceProvider138.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(1091, 41)));
			object obj238 = markupExtension138.ProvideValue(xamlServiceProvider138);
			dashboardItemColorBox26.Text = obj238;
			bindingExtension101.Mode = 2;
			bindingExtension101.Path = "MinMaxPointersColor";
			bindingExtension101.TypedBinding = new TypedBinding<DashboardItem, Color>(delegate(DashboardItem A_0)
			{
				if (A_0 != null)
				{
					return new ValueTuple<Color, bool>(A_0.MinMaxPointersColor, true);
				}
				return default(ValueTuple<Color, bool>);
			}, null, new Tuple<Func<DashboardItem, object>, string>[]
			{
				new Tuple<Func<DashboardItem, object>, string>((DashboardItem A_0) => A_0, "MinMaxPointersColor")
			});
			BindingBase bindingBase101 = bindingExtension101.ProvideValue(null);
			dashboardItemColorBox26.SetBinding(DashboardItemColorBox.ColorProperty, bindingBase101);
			stackLayout17.Children.Add(dashboardItemColorBox26);
			grid20.SetValue(View.MarginProperty, new Thickness(0.0, 0.0, 0.0, 10.0));
			grid20.SetValue(Grid.RowSpacingProperty, 0.0);
			columnDefinition25.SetValue(ColumnDefinition.WidthProperty, new GridLengthTypeConverter().ConvertFromInvariantString("*"));
			grid20.GetValue(Grid.ColumnDefinitionsProperty).Add(columnDefinition25);
			columnDefinition26.SetValue(ColumnDefinition.WidthProperty, new GridLengthTypeConverter().ConvertFromInvariantString("auto"));
			grid20.GetValue(Grid.ColumnDefinitionsProperty).Add(columnDefinition26);
			rowDefinition19.SetValue(RowDefinition.HeightProperty, new GridLengthTypeConverter().ConvertFromInvariantString("auto"));
			grid20.GetValue(Grid.RowDefinitionsProperty).Add(rowDefinition19);
			rowDefinition20.SetValue(RowDefinition.HeightProperty, new GridLengthTypeConverter().ConvertFromInvariantString("auto"));
			grid20.GetValue(Grid.RowDefinitionsProperty).Add(rowDefinition20);
			label56.SetValue(Grid.RowProperty, 1);
			label56.SetValue(Grid.ColumnProperty, 0);
			label56.SetValue(Label.FontAttributesProperty, new FontAttributesConverter().ConvertFromInvariantString("Italic"));
			bindingExtension102.Mode = 2;
			bindingExtension102.Path = "SetMinMaxAvgOnlyVisibleArea";
			bindingExtension102.TypedBinding = new TypedBinding<DashboardItem, bool>(delegate(DashboardItem A_0)
			{
				if (A_0 != null)
				{
					return new ValueTuple<bool, bool>(A_0.SetMinMaxAvgOnlyVisibleArea, true);
				}
				return default(ValueTuple<bool, bool>);
			}, null, new Tuple<Func<DashboardItem, object>, string>[]
			{
				new Tuple<Func<DashboardItem, object>, string>((DashboardItem A_0) => A_0, "SetMinMaxAvgOnlyVisibleArea")
			});
			BindingBase bindingBase102 = bindingExtension102.ProvideValue(null);
			label56.SetBinding(VisualElement.IsVisibleProperty, bindingBase102);
			translate91.Text = "Settings_Control_ToggleSetChartMinMaxOnlyVisibleArea.OnContent";
			IMarkupExtension markupExtension139 = translate91;
			XamlServiceProvider xamlServiceProvider139 = new XamlServiceProvider();
			Type typeFromHandle277 = typeof(IProvideValueTarget);
			object[] array139 = new object[0 + 9];
			array139[0] = label56;
			array139[1] = grid20;
			array139[2] = stackLayout17;
			array139[3] = frame10;
			array139[4] = sfExpander8;
			array139[5] = stackLayout24;
			array139[6] = scrollView;
			array139[7] = grid25;
			array139[8] = this;
			object obj239;
			xamlServiceProvider139.Add(typeFromHandle277, obj239 = new SimpleValueTargetProvider(array139, Label.TextProperty, nameScope));
			xamlServiceProvider139.Add(typeof(IReferenceProvider), obj239);
			Type typeFromHandle278 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver139 = new XmlNamespaceResolver();
			xmlNamespaceResolver139.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver139.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver139.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver139.Add("dash", "clr-namespace:CarScannerXamarinForms.Dashboard");
			xmlNamespaceResolver139.Add("expander", "clr-namespace:Syncfusion.XForms.Expander;assembly=Syncfusion.Expander.XForms");
			xmlNamespaceResolver139.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver139.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver139.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver139.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider139.Add(typeFromHandle278, new XamlTypeResolver(xmlNamespaceResolver139, typeof(DashboardItemEditor).GetTypeInfo().Assembly));
			xamlServiceProvider139.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(1108, 45)));
			object obj240 = markupExtension139.ProvideValue(xamlServiceProvider139);
			label56.Text = obj240;
			label56.SetValue(View.VerticalOptionsProperty, LayoutOptions.Center);
			grid20.Children.Add(label56);
			label57.SetValue(Grid.RowProperty, 1);
			label57.SetValue(Grid.ColumnProperty, 0);
			label57.SetValue(Label.FontAttributesProperty, new FontAttributesConverter().ConvertFromInvariantString("Italic"));
			bindingExtension103.Mode = 2;
			staticResourceExtension11.Key = "BoolToNegativeConverter";
			IMarkupExtension markupExtension140 = staticResourceExtension11;
			XamlServiceProvider xamlServiceProvider140 = new XamlServiceProvider();
			Type typeFromHandle279 = typeof(IProvideValueTarget);
			object[] array140 = new object[0 + 10];
			array140[0] = bindingExtension103;
			array140[1] = label57;
			array140[2] = grid20;
			array140[3] = stackLayout17;
			array140[4] = frame10;
			array140[5] = sfExpander8;
			array140[6] = stackLayout24;
			array140[7] = scrollView;
			array140[8] = grid25;
			array140[9] = this;
			object obj241;
			xamlServiceProvider140.Add(typeFromHandle279, obj241 = new SimpleValueTargetProvider(array140, typeof(BindingExtension).GetRuntimeProperty("Converter"), nameScope));
			xamlServiceProvider140.Add(typeof(IReferenceProvider), obj241);
			Type typeFromHandle280 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver140 = new XmlNamespaceResolver();
			xmlNamespaceResolver140.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver140.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver140.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver140.Add("dash", "clr-namespace:CarScannerXamarinForms.Dashboard");
			xmlNamespaceResolver140.Add("expander", "clr-namespace:Syncfusion.XForms.Expander;assembly=Syncfusion.Expander.XForms");
			xmlNamespaceResolver140.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver140.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver140.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver140.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider140.Add(typeFromHandle280, new XamlTypeResolver(xmlNamespaceResolver140, typeof(DashboardItemEditor).GetTypeInfo().Assembly));
			xamlServiceProvider140.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(1114, 45)));
			object obj242 = markupExtension140.ProvideValue(xamlServiceProvider140);
			bindingExtension103.Converter = obj242;
			bindingExtension103.Path = "SetMinMaxAvgOnlyVisibleArea";
			bindingExtension103.TypedBinding = new TypedBinding<DashboardItem, bool>(delegate(DashboardItem A_0)
			{
				if (A_0 != null)
				{
					return new ValueTuple<bool, bool>(A_0.SetMinMaxAvgOnlyVisibleArea, true);
				}
				return default(ValueTuple<bool, bool>);
			}, null, new Tuple<Func<DashboardItem, object>, string>[]
			{
				new Tuple<Func<DashboardItem, object>, string>((DashboardItem A_0) => A_0, "SetMinMaxAvgOnlyVisibleArea")
			});
			BindingBase bindingBase103 = bindingExtension103.ProvideValue(null);
			label57.SetBinding(VisualElement.IsVisibleProperty, bindingBase103);
			translate92.Text = "Settings_Control_ToggleSetChartMinMaxOnlyVisibleArea.OffContent";
			IMarkupExtension markupExtension141 = translate92;
			XamlServiceProvider xamlServiceProvider141 = new XamlServiceProvider();
			Type typeFromHandle281 = typeof(IProvideValueTarget);
			object[] array141 = new object[0 + 9];
			array141[0] = label57;
			array141[1] = grid20;
			array141[2] = stackLayout17;
			array141[3] = frame10;
			array141[4] = sfExpander8;
			array141[5] = stackLayout24;
			array141[6] = scrollView;
			array141[7] = grid25;
			array141[8] = this;
			object obj243;
			xamlServiceProvider141.Add(typeFromHandle281, obj243 = new SimpleValueTargetProvider(array141, Label.TextProperty, nameScope));
			xamlServiceProvider141.Add(typeof(IReferenceProvider), obj243);
			Type typeFromHandle282 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver141 = new XmlNamespaceResolver();
			xmlNamespaceResolver141.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver141.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver141.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver141.Add("dash", "clr-namespace:CarScannerXamarinForms.Dashboard");
			xmlNamespaceResolver141.Add("expander", "clr-namespace:Syncfusion.XForms.Expander;assembly=Syncfusion.Expander.XForms");
			xmlNamespaceResolver141.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver141.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver141.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver141.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider141.Add(typeFromHandle282, new XamlTypeResolver(xmlNamespaceResolver141, typeof(DashboardItemEditor).GetTypeInfo().Assembly));
			xamlServiceProvider141.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(1115, 45)));
			object obj244 = markupExtension141.ProvideValue(xamlServiceProvider141);
			label57.Text = obj244;
			label57.SetValue(View.VerticalOptionsProperty, LayoutOptions.Center);
			grid20.Children.Add(label57);
			label58.SetValue(Grid.RowProperty, 0);
			label58.SetValue(Grid.ColumnProperty, 0);
			translate93.Text = "Settings_Control_ToggleSetChartMinMaxOnlyVisibleArea.Header";
			IMarkupExtension markupExtension142 = translate93;
			XamlServiceProvider xamlServiceProvider142 = new XamlServiceProvider();
			Type typeFromHandle283 = typeof(IProvideValueTarget);
			object[] array142 = new object[0 + 9];
			array142[0] = label58;
			array142[1] = grid20;
			array142[2] = stackLayout17;
			array142[3] = frame10;
			array142[4] = sfExpander8;
			array142[5] = stackLayout24;
			array142[6] = scrollView;
			array142[7] = grid25;
			array142[8] = this;
			object obj245;
			xamlServiceProvider142.Add(typeFromHandle283, obj245 = new SimpleValueTargetProvider(array142, Label.TextProperty, nameScope));
			xamlServiceProvider142.Add(typeof(IReferenceProvider), obj245);
			Type typeFromHandle284 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver142 = new XmlNamespaceResolver();
			xmlNamespaceResolver142.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver142.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver142.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver142.Add("dash", "clr-namespace:CarScannerXamarinForms.Dashboard");
			xmlNamespaceResolver142.Add("expander", "clr-namespace:Syncfusion.XForms.Expander;assembly=Syncfusion.Expander.XForms");
			xmlNamespaceResolver142.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver142.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver142.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver142.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider142.Add(typeFromHandle284, new XamlTypeResolver(xmlNamespaceResolver142, typeof(DashboardItemEditor).GetTypeInfo().Assembly));
			xamlServiceProvider142.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(1120, 45)));
			object obj246 = markupExtension142.ProvideValue(xamlServiceProvider142);
			label58.Text = obj246;
			grid20.Children.Add(label58);
			checkSwitch.SetValue(Grid.RowProperty, 1);
			checkSwitch.SetValue(Grid.ColumnProperty, 1);
			bindingExtension104.Mode = 1;
			bindingExtension104.Path = "SetMinMaxAvgOnlyVisibleArea";
			bindingExtension104.TypedBinding = new TypedBinding<DashboardItem, bool>(delegate(DashboardItem A_0)
			{
				if (A_0 != null)
				{
					return new ValueTuple<bool, bool>(A_0.SetMinMaxAvgOnlyVisibleArea, true);
				}
				return default(ValueTuple<bool, bool>);
			}, delegate(DashboardItem A_0, bool A_1)
			{
				if (A_0 != null)
				{
					A_0.SetMinMaxAvgOnlyVisibleArea = A_1;
					return;
				}
			}, new Tuple<Func<DashboardItem, object>, string>[]
			{
				new Tuple<Func<DashboardItem, object>, string>((DashboardItem A_0) => A_0, "SetMinMaxAvgOnlyVisibleArea")
			});
			BindingBase bindingBase104 = bindingExtension104.ProvideValue(null);
			checkSwitch.SetBinding(CheckSwitch.IsToggledProperty, bindingBase104);
			grid20.Children.Add(checkSwitch);
			stackLayout17.Children.Add(grid20);
			frame10.SetValue(ContentView.ContentProperty, stackLayout17);
			sfExpander8.SetValue(SfExpander.ContentProperty, frame10);
			stackLayout24.Children.Add(sfExpander8);
			sfExpander9.SetValue(SfExpander.AnimationDurationProperty, 250.0);
			sfExpander9.SetValue(SfExpander.AnimationEasingProperty, 3);
			dynamicResourceExtension37.Key = "NavigationBarBackgroundColor";
			IMarkupExtension<DynamicResource> markupExtension143 = dynamicResourceExtension37;
			XamlServiceProvider xamlServiceProvider143 = new XamlServiceProvider();
			Type typeFromHandle285 = typeof(IProvideValueTarget);
			object[] array143 = new object[0 + 5];
			array143[0] = sfExpander9;
			array143[1] = stackLayout24;
			array143[2] = scrollView;
			array143[3] = grid25;
			array143[4] = this;
			object obj247;
			xamlServiceProvider143.Add(typeFromHandle285, obj247 = new SimpleValueTargetProvider(array143, SfExpander.HeaderBackgroundColorProperty, nameScope));
			xamlServiceProvider143.Add(typeof(IReferenceProvider), obj247);
			Type typeFromHandle286 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver143 = new XmlNamespaceResolver();
			xmlNamespaceResolver143.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver143.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver143.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver143.Add("dash", "clr-namespace:CarScannerXamarinForms.Dashboard");
			xmlNamespaceResolver143.Add("expander", "clr-namespace:Syncfusion.XForms.Expander;assembly=Syncfusion.Expander.XForms");
			xmlNamespaceResolver143.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver143.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver143.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver143.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider143.Add(typeFromHandle286, new XamlTypeResolver(xmlNamespaceResolver143, typeof(DashboardItemEditor).GetTypeInfo().Assembly));
			xamlServiceProvider143.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(1136, 25)));
			DynamicResource dynamicResource37 = markupExtension143.ProvideValue(xamlServiceProvider143);
			sfExpander9.SetDynamicResource(SfExpander.HeaderBackgroundColorProperty, dynamicResource37.Key);
			dynamicResourceExtension38.Key = "NavigationBarButtonColor";
			IMarkupExtension<DynamicResource> markupExtension144 = dynamicResourceExtension38;
			XamlServiceProvider xamlServiceProvider144 = new XamlServiceProvider();
			Type typeFromHandle287 = typeof(IProvideValueTarget);
			object[] array144 = new object[0 + 5];
			array144[0] = sfExpander9;
			array144[1] = stackLayout24;
			array144[2] = scrollView;
			array144[3] = grid25;
			array144[4] = this;
			object obj248;
			xamlServiceProvider144.Add(typeFromHandle287, obj248 = new SimpleValueTargetProvider(array144, SfExpander.IconColorProperty, nameScope));
			xamlServiceProvider144.Add(typeof(IReferenceProvider), obj248);
			Type typeFromHandle288 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver144 = new XmlNamespaceResolver();
			xmlNamespaceResolver144.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver144.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver144.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver144.Add("dash", "clr-namespace:CarScannerXamarinForms.Dashboard");
			xmlNamespaceResolver144.Add("expander", "clr-namespace:Syncfusion.XForms.Expander;assembly=Syncfusion.Expander.XForms");
			xmlNamespaceResolver144.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver144.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver144.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver144.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider144.Add(typeFromHandle288, new XamlTypeResolver(xmlNamespaceResolver144, typeof(DashboardItemEditor).GetTypeInfo().Assembly));
			xamlServiceProvider144.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(1137, 25)));
			DynamicResource dynamicResource38 = markupExtension144.ProvideValue(xamlServiceProvider144);
			sfExpander9.SetDynamicResource(SfExpander.IconColorProperty, dynamicResource38.Key);
			sfExpander9.SetValue(SfExpander.IsExpandedProperty, false);
			label59.SetValue(View.MarginProperty, new Thickness(5.0));
			label59.SetValue(Label.FontAttributesProperty, new FontAttributesConverter().ConvertFromInvariantString("Bold"));
			translate94.Text = "Dashboard_WarningsAndSound";
			IMarkupExtension markupExtension145 = translate94;
			XamlServiceProvider xamlServiceProvider145 = new XamlServiceProvider();
			Type typeFromHandle289 = typeof(IProvideValueTarget);
			object[] array145 = new object[0 + 7];
			array145[0] = label59;
			array145[1] = grid21;
			array145[2] = sfExpander9;
			array145[3] = stackLayout24;
			array145[4] = scrollView;
			array145[5] = grid25;
			array145[6] = this;
			object obj249;
			xamlServiceProvider145.Add(typeFromHandle289, obj249 = new SimpleValueTargetProvider(array145, Label.TextProperty, nameScope));
			xamlServiceProvider145.Add(typeof(IReferenceProvider), obj249);
			Type typeFromHandle290 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver145 = new XmlNamespaceResolver();
			xmlNamespaceResolver145.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver145.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver145.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver145.Add("dash", "clr-namespace:CarScannerXamarinForms.Dashboard");
			xmlNamespaceResolver145.Add("expander", "clr-namespace:Syncfusion.XForms.Expander;assembly=Syncfusion.Expander.XForms");
			xmlNamespaceResolver145.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver145.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver145.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver145.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider145.Add(typeFromHandle290, new XamlTypeResolver(xmlNamespaceResolver145, typeof(DashboardItemEditor).GetTypeInfo().Assembly));
			xamlServiceProvider145.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(1144, 37)));
			object obj250 = markupExtension145.ProvideValue(xamlServiceProvider145);
			label59.Text = obj250;
			dynamicResourceExtension39.Key = "NavigationBarTextColor";
			IMarkupExtension<DynamicResource> markupExtension146 = dynamicResourceExtension39;
			XamlServiceProvider xamlServiceProvider146 = new XamlServiceProvider();
			Type typeFromHandle291 = typeof(IProvideValueTarget);
			object[] array146 = new object[0 + 7];
			array146[0] = label59;
			array146[1] = grid21;
			array146[2] = sfExpander9;
			array146[3] = stackLayout24;
			array146[4] = scrollView;
			array146[5] = grid25;
			array146[6] = this;
			object obj251;
			xamlServiceProvider146.Add(typeFromHandle291, obj251 = new SimpleValueTargetProvider(array146, Label.TextColorProperty, nameScope));
			xamlServiceProvider146.Add(typeof(IReferenceProvider), obj251);
			Type typeFromHandle292 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver146 = new XmlNamespaceResolver();
			xmlNamespaceResolver146.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver146.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver146.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver146.Add("dash", "clr-namespace:CarScannerXamarinForms.Dashboard");
			xmlNamespaceResolver146.Add("expander", "clr-namespace:Syncfusion.XForms.Expander;assembly=Syncfusion.Expander.XForms");
			xmlNamespaceResolver146.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver146.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver146.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver146.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider146.Add(typeFromHandle292, new XamlTypeResolver(xmlNamespaceResolver146, typeof(DashboardItemEditor).GetTypeInfo().Assembly));
			xamlServiceProvider146.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(1145, 37)));
			DynamicResource dynamicResource39 = markupExtension146.ProvideValue(xamlServiceProvider146);
			label59.SetDynamicResource(Label.TextColorProperty, dynamicResource39.Key);
			label59.SetValue(View.VerticalOptionsProperty, LayoutOptions.Center);
			label59.SetValue(Label.VerticalTextAlignmentProperty, new TextAlignmentConverter().ConvertFromInvariantString("Center"));
			grid21.Children.Add(label59);
			sfExpander9.SetValue(SfExpander.HeaderProperty, grid21);
			stackLayout22.SetValue(StackLayout.OrientationProperty, 0);
			bindingExtension105.Mode = 1;
			bindingExtension105.Path = "GaugeShowRedLine";
			bindingExtension105.TypedBinding = new TypedBinding<DashboardItem, bool>(delegate(DashboardItem A_0)
			{
				if (A_0 != null)
				{
					return new ValueTuple<bool, bool>(A_0.GaugeShowRedLine, true);
				}
				return default(ValueTuple<bool, bool>);
			}, delegate(DashboardItem A_0, bool A_1)
			{
				if (A_0 != null)
				{
					A_0.GaugeShowRedLine = A_1;
					return;
				}
			}, new Tuple<Func<DashboardItem, object>, string>[]
			{
				new Tuple<Func<DashboardItem, object>, string>((DashboardItem A_0) => A_0, "GaugeShowRedLine")
			});
			BindingBase bindingBase105 = bindingExtension105.ProvideValue(null);
			labelSwitch14.SetBinding(LabelSwitch.IsToggledProperty, bindingBase105);
			translate95.Text = "ios_DashboardItemEditor_ChangeValueTextColor";
			IMarkupExtension markupExtension147 = translate95;
			XamlServiceProvider xamlServiceProvider147 = new XamlServiceProvider();
			Type typeFromHandle293 = typeof(IProvideValueTarget);
			object[] array147 = new object[0 + 8];
			array147[0] = labelSwitch14;
			array147[1] = stackLayout22;
			array147[2] = frame11;
			array147[3] = sfExpander9;
			array147[4] = stackLayout24;
			array147[5] = scrollView;
			array147[6] = grid25;
			array147[7] = this;
			object obj252;
			xamlServiceProvider147.Add(typeFromHandle293, obj252 = new SimpleValueTargetProvider(array147, LabelSwitch.TextProperty, nameScope));
			xamlServiceProvider147.Add(typeof(IReferenceProvider), obj252);
			Type typeFromHandle294 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver147 = new XmlNamespaceResolver();
			xmlNamespaceResolver147.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver147.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver147.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver147.Add("dash", "clr-namespace:CarScannerXamarinForms.Dashboard");
			xmlNamespaceResolver147.Add("expander", "clr-namespace:Syncfusion.XForms.Expander;assembly=Syncfusion.Expander.XForms");
			xmlNamespaceResolver147.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver147.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver147.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver147.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider147.Add(typeFromHandle294, new XamlTypeResolver(xmlNamespaceResolver147, typeof(DashboardItemEditor).GetTypeInfo().Assembly));
			xamlServiceProvider147.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(1155, 108)));
			object obj253 = markupExtension147.ProvideValue(xamlServiceProvider147);
			labelSwitch14.Text = obj253;
			stackLayout22.Children.Add(labelSwitch14);
			bindingExtension106.Mode = 2;
			bindingExtension106.Path = "GaugeShowRedLine";
			bindingExtension106.TypedBinding = new TypedBinding<DashboardItem, bool>(delegate(DashboardItem A_0)
			{
				if (A_0 != null)
				{
					return new ValueTuple<bool, bool>(A_0.GaugeShowRedLine, true);
				}
				return default(ValueTuple<bool, bool>);
			}, null, new Tuple<Func<DashboardItem, object>, string>[]
			{
				new Tuple<Func<DashboardItem, object>, string>((DashboardItem A_0) => A_0, "GaugeShowRedLine")
			});
			BindingBase bindingBase106 = bindingExtension106.ProvideValue(null);
			stackLayout18.SetBinding(VisualElement.IsVisibleProperty, bindingBase106);
			stackLayout18.SetValue(StackLayout.OrientationProperty, 0);
			translate96.Text = "ios_DashboardItemEditor_Threshold";
			IMarkupExtension markupExtension148 = translate96;
			XamlServiceProvider xamlServiceProvider148 = new XamlServiceProvider();
			Type typeFromHandle295 = typeof(IProvideValueTarget);
			object[] array148 = new object[0 + 9];
			array148[0] = label60;
			array148[1] = stackLayout18;
			array148[2] = stackLayout22;
			array148[3] = frame11;
			array148[4] = sfExpander9;
			array148[5] = stackLayout24;
			array148[6] = scrollView;
			array148[7] = grid25;
			array148[8] = this;
			object obj254;
			xamlServiceProvider148.Add(typeFromHandle295, obj254 = new SimpleValueTargetProvider(array148, Label.TextProperty, nameScope));
			xamlServiceProvider148.Add(typeof(IReferenceProvider), obj254);
			Type typeFromHandle296 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver148 = new XmlNamespaceResolver();
			xmlNamespaceResolver148.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver148.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver148.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver148.Add("dash", "clr-namespace:CarScannerXamarinForms.Dashboard");
			xmlNamespaceResolver148.Add("expander", "clr-namespace:Syncfusion.XForms.Expander;assembly=Syncfusion.Expander.XForms");
			xmlNamespaceResolver148.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver148.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver148.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver148.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider148.Add(typeFromHandle296, new XamlTypeResolver(xmlNamespaceResolver148, typeof(DashboardItemEditor).GetTypeInfo().Assembly));
			xamlServiceProvider148.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(1159, 48)));
			object obj255 = markupExtension148.ProvideValue(xamlServiceProvider148);
			label60.Text = obj255;
			stackLayout18.Children.Add(label60);
			bindingExtension107.Mode = 1;
			bindingExtension107.Path = "GaugeRedLineStart";
			bindingExtension107.TypedBinding = new TypedBinding<DashboardItem, double>(delegate(DashboardItem A_0)
			{
				if (A_0 != null)
				{
					return new ValueTuple<double, bool>(A_0.GaugeRedLineStart, true);
				}
				return default(ValueTuple<double, bool>);
			}, delegate(DashboardItem A_0, double A_1)
			{
				if (A_0 != null)
				{
					A_0.GaugeRedLineStart = A_1;
					return;
				}
			}, new Tuple<Func<DashboardItem, object>, string>[]
			{
				new Tuple<Func<DashboardItem, object>, string>((DashboardItem A_0) => A_0, "GaugeRedLineStart")
			});
			BindingBase bindingBase107 = bindingExtension107.ProvideValue(null);
			numericEntryV24.SetBinding(NumericEntryV3.ValueProperty, bindingBase107);
			stackLayout18.Children.Add(numericEntryV24);
			dashboardItemColorBox27.SetValue(Element.ClassIdProperty, "GaugeRedLineColor");
			dashboardItemColorBox27.Tapped += this.ColorBox_Tapped;
			translate97.Text = "ios_DashboardItemEditor_ValueExceedThresholdColor";
			IMarkupExtension markupExtension149 = translate97;
			XamlServiceProvider xamlServiceProvider149 = new XamlServiceProvider();
			Type typeFromHandle297 = typeof(IProvideValueTarget);
			object[] array149 = new object[0 + 9];
			array149[0] = dashboardItemColorBox27;
			array149[1] = stackLayout18;
			array149[2] = stackLayout22;
			array149[3] = frame11;
			array149[4] = sfExpander9;
			array149[5] = stackLayout24;
			array149[6] = scrollView;
			array149[7] = grid25;
			array149[8] = this;
			object obj256;
			xamlServiceProvider149.Add(typeFromHandle297, obj256 = new SimpleValueTargetProvider(array149, DashboardItemColorBox.TextProperty, nameScope));
			xamlServiceProvider149.Add(typeof(IReferenceProvider), obj256);
			Type typeFromHandle298 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver149 = new XmlNamespaceResolver();
			xmlNamespaceResolver149.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver149.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver149.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver149.Add("dash", "clr-namespace:CarScannerXamarinForms.Dashboard");
			xmlNamespaceResolver149.Add("expander", "clr-namespace:Syncfusion.XForms.Expander;assembly=Syncfusion.Expander.XForms");
			xmlNamespaceResolver149.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver149.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver149.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver149.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider149.Add(typeFromHandle298, new XamlTypeResolver(xmlNamespaceResolver149, typeof(DashboardItemEditor).GetTypeInfo().Assembly));
			xamlServiceProvider149.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(1166, 45)));
			object obj257 = markupExtension149.ProvideValue(xamlServiceProvider149);
			dashboardItemColorBox27.Text = obj257;
			bindingExtension108.Mode = 2;
			bindingExtension108.Path = "GaugeRedLineColor";
			bindingExtension108.TypedBinding = new TypedBinding<DashboardItem, Color>(delegate(DashboardItem A_0)
			{
				if (A_0 != null)
				{
					return new ValueTuple<Color, bool>(A_0.GaugeRedLineColor, true);
				}
				return default(ValueTuple<Color, bool>);
			}, null, new Tuple<Func<DashboardItem, object>, string>[]
			{
				new Tuple<Func<DashboardItem, object>, string>((DashboardItem A_0) => A_0, "GaugeRedLineColor")
			});
			BindingBase bindingBase108 = bindingExtension108.ProvideValue(null);
			dashboardItemColorBox27.SetBinding(DashboardItemColorBox.ColorProperty, bindingBase108);
			stackLayout18.Children.Add(dashboardItemColorBox27);
			stackLayout22.Children.Add(stackLayout18);
			bindingExtension109.Mode = 1;
			bindingExtension109.Path = "PlaySound";
			bindingExtension109.TypedBinding = new TypedBinding<DashboardItem, bool>(delegate(DashboardItem A_0)
			{
				if (A_0 != null)
				{
					return new ValueTuple<bool, bool>(A_0.PlaySound, true);
				}
				return default(ValueTuple<bool, bool>);
			}, delegate(DashboardItem A_0, bool A_1)
			{
				if (A_0 != null)
				{
					A_0.PlaySound = A_1;
					return;
				}
			}, new Tuple<Func<DashboardItem, object>, string>[]
			{
				new Tuple<Func<DashboardItem, object>, string>((DashboardItem A_0) => A_0, "PlaySound")
			});
			BindingBase bindingBase109 = bindingExtension109.ProvideValue(null);
			labelSwitch15.SetBinding(LabelSwitch.IsToggledProperty, bindingBase109);
			translate98.Text = "ios_DashboardItemEditor_PlaySound";
			IMarkupExtension markupExtension150 = translate98;
			XamlServiceProvider xamlServiceProvider150 = new XamlServiceProvider();
			Type typeFromHandle299 = typeof(IProvideValueTarget);
			object[] array150 = new object[0 + 8];
			array150[0] = labelSwitch15;
			array150[1] = stackLayout22;
			array150[2] = frame11;
			array150[3] = sfExpander9;
			array150[4] = stackLayout24;
			array150[5] = scrollView;
			array150[6] = grid25;
			array150[7] = this;
			object obj258;
			xamlServiceProvider150.Add(typeFromHandle299, obj258 = new SimpleValueTargetProvider(array150, LabelSwitch.TextProperty, nameScope));
			xamlServiceProvider150.Add(typeof(IReferenceProvider), obj258);
			Type typeFromHandle300 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver150 = new XmlNamespaceResolver();
			xmlNamespaceResolver150.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver150.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver150.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver150.Add("dash", "clr-namespace:CarScannerXamarinForms.Dashboard");
			xmlNamespaceResolver150.Add("expander", "clr-namespace:Syncfusion.XForms.Expander;assembly=Syncfusion.Expander.XForms");
			xmlNamespaceResolver150.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver150.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver150.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver150.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider150.Add(typeFromHandle300, new XamlTypeResolver(xmlNamespaceResolver150, typeof(DashboardItemEditor).GetTypeInfo().Assembly));
			xamlServiceProvider150.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(1173, 101)));
			object obj259 = markupExtension150.ProvideValue(xamlServiceProvider150);
			labelSwitch15.Text = obj259;
			stackLayout22.Children.Add(labelSwitch15);
			bindingExtension110.Mode = 2;
			bindingExtension110.Path = "PlaySound";
			bindingExtension110.TypedBinding = new TypedBinding<DashboardItem, bool>(delegate(DashboardItem A_0)
			{
				if (A_0 != null)
				{
					return new ValueTuple<bool, bool>(A_0.PlaySound, true);
				}
				return default(ValueTuple<bool, bool>);
			}, null, new Tuple<Func<DashboardItem, object>, string>[]
			{
				new Tuple<Func<DashboardItem, object>, string>((DashboardItem A_0) => A_0, "PlaySound")
			});
			BindingBase bindingBase110 = bindingExtension110.ProvideValue(null);
			stackLayout19.SetBinding(VisualElement.IsVisibleProperty, bindingBase110);
			stackLayout19.SetValue(StackLayout.OrientationProperty, 0);
			translate99.Text = "ios_DashboardItemEditor_Threshold";
			IMarkupExtension markupExtension151 = translate99;
			XamlServiceProvider xamlServiceProvider151 = new XamlServiceProvider();
			Type typeFromHandle301 = typeof(IProvideValueTarget);
			object[] array151 = new object[0 + 9];
			array151[0] = label61;
			array151[1] = stackLayout19;
			array151[2] = stackLayout22;
			array151[3] = frame11;
			array151[4] = sfExpander9;
			array151[5] = stackLayout24;
			array151[6] = scrollView;
			array151[7] = grid25;
			array151[8] = this;
			object obj260;
			xamlServiceProvider151.Add(typeFromHandle301, obj260 = new SimpleValueTargetProvider(array151, Label.TextProperty, nameScope));
			xamlServiceProvider151.Add(typeof(IReferenceProvider), obj260);
			Type typeFromHandle302 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver151 = new XmlNamespaceResolver();
			xmlNamespaceResolver151.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver151.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver151.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver151.Add("dash", "clr-namespace:CarScannerXamarinForms.Dashboard");
			xmlNamespaceResolver151.Add("expander", "clr-namespace:Syncfusion.XForms.Expander;assembly=Syncfusion.Expander.XForms");
			xmlNamespaceResolver151.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver151.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver151.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver151.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider151.Add(typeFromHandle302, new XamlTypeResolver(xmlNamespaceResolver151, typeof(DashboardItemEditor).GetTypeInfo().Assembly));
			xamlServiceProvider151.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(1177, 48)));
			object obj261 = markupExtension151.ProvideValue(xamlServiceProvider151);
			label61.Text = obj261;
			stackLayout19.Children.Add(label61);
			bindingExtension111.Mode = 1;
			bindingExtension111.Path = "SoundStart";
			bindingExtension111.TypedBinding = new TypedBinding<DashboardItem, double>(delegate(DashboardItem A_0)
			{
				if (A_0 != null)
				{
					return new ValueTuple<double, bool>(A_0.SoundStart, true);
				}
				return default(ValueTuple<double, bool>);
			}, delegate(DashboardItem A_0, double A_1)
			{
				if (A_0 != null)
				{
					A_0.SoundStart = A_1;
					return;
				}
			}, new Tuple<Func<DashboardItem, object>, string>[]
			{
				new Tuple<Func<DashboardItem, object>, string>((DashboardItem A_0) => A_0, "SoundStart")
			});
			BindingBase bindingBase111 = bindingExtension111.ProvideValue(null);
			numericEntryV25.SetBinding(NumericEntryV3.ValueProperty, bindingBase111);
			stackLayout19.Children.Add(numericEntryV25);
			translate100.Text = "ios_DashboardItemEditor_SoundName";
			IMarkupExtension markupExtension152 = translate100;
			XamlServiceProvider xamlServiceProvider152 = new XamlServiceProvider();
			Type typeFromHandle303 = typeof(IProvideValueTarget);
			object[] array152 = new object[0 + 9];
			array152[0] = label62;
			array152[1] = stackLayout19;
			array152[2] = stackLayout22;
			array152[3] = frame11;
			array152[4] = sfExpander9;
			array152[5] = stackLayout24;
			array152[6] = scrollView;
			array152[7] = grid25;
			array152[8] = this;
			object obj262;
			xamlServiceProvider152.Add(typeFromHandle303, obj262 = new SimpleValueTargetProvider(array152, Label.TextProperty, nameScope));
			xamlServiceProvider152.Add(typeof(IReferenceProvider), obj262);
			Type typeFromHandle304 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver152 = new XmlNamespaceResolver();
			xmlNamespaceResolver152.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver152.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver152.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver152.Add("dash", "clr-namespace:CarScannerXamarinForms.Dashboard");
			xmlNamespaceResolver152.Add("expander", "clr-namespace:Syncfusion.XForms.Expander;assembly=Syncfusion.Expander.XForms");
			xmlNamespaceResolver152.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver152.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver152.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver152.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider152.Add(typeFromHandle304, new XamlTypeResolver(xmlNamespaceResolver152, typeof(DashboardItemEditor).GetTypeInfo().Assembly));
			xamlServiceProvider152.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(1181, 48)));
			object obj263 = markupExtension152.ProvideValue(xamlServiceProvider152);
			label62.Text = obj263;
			stackLayout19.Children.Add(label62);
			columnDefinition27.SetValue(ColumnDefinition.WidthProperty, new GridLengthTypeConverter().ConvertFromInvariantString("*"));
			grid22.GetValue(Grid.ColumnDefinitionsProperty).Add(columnDefinition27);
			columnDefinition28.SetValue(ColumnDefinition.WidthProperty, new GridLengthTypeConverter().ConvertFromInvariantString("auto"));
			grid22.GetValue(Grid.ColumnDefinitionsProperty).Add(columnDefinition28);
			picker4.SetValue(Grid.ColumnProperty, 0);
			bindingExtension112.Source = soundsList;
			BindingBase bindingBase112 = bindingExtension112.ProvideValue(null);
			picker4.SetBinding(Picker.ItemsSourceProperty, bindingBase112);
			bindingExtension113.Mode = 1;
			bindingExtension113.Path = "SoundName";
			bindingExtension113.TypedBinding = new TypedBinding<DashboardItem, string>(delegate(DashboardItem A_0)
			{
				if (A_0 != null)
				{
					return new ValueTuple<string, bool>(A_0.SoundName, true);
				}
				return default(ValueTuple<string, bool>);
			}, delegate(DashboardItem A_0, string A_1)
			{
				if (A_0 != null)
				{
					A_0.SoundName = A_1;
					return;
				}
			}, new Tuple<Func<DashboardItem, object>, string>[]
			{
				new Tuple<Func<DashboardItem, object>, string>((DashboardItem A_0) => A_0, "SoundName")
			});
			BindingBase bindingBase113 = bindingExtension113.ProvideValue(null);
			picker4.SetBinding(Picker.SelectedItemProperty, bindingBase113);
			grid22.Children.Add(picker4);
			image.SetValue(Grid.ColumnProperty, 1);
			image.SetValue(VisualElement.BackgroundColorProperty, Color.White);
			image.SetValue(Image.SourceProperty, new ImageSourceConverter().ConvertFromInvariantString("icons8_play.png"));
			tapGestureRecognizer.SetValue(TapGestureRecognizer.NumberOfTapsRequiredProperty, 1);
			tapGestureRecognizer.Tapped += this.play_Tapped;
			image.GestureRecognizers.Add(tapGestureRecognizer);
			grid22.Children.Add(image);
			stackLayout19.Children.Add(grid22);
			stackLayout22.Children.Add(stackLayout19);
			bindingExtension114.Mode = 1;
			bindingExtension114.Path = "ShowLowWarning";
			bindingExtension114.TypedBinding = new TypedBinding<DashboardItem, bool>(delegate(DashboardItem A_0)
			{
				if (A_0 != null)
				{
					return new ValueTuple<bool, bool>(A_0.ShowLowWarning, true);
				}
				return default(ValueTuple<bool, bool>);
			}, delegate(DashboardItem A_0, bool A_1)
			{
				if (A_0 != null)
				{
					A_0.ShowLowWarning = A_1;
					return;
				}
			}, new Tuple<Func<DashboardItem, object>, string>[]
			{
				new Tuple<Func<DashboardItem, object>, string>((DashboardItem A_0) => A_0, "ShowLowWarning")
			});
			BindingBase bindingBase114 = bindingExtension114.ProvideValue(null);
			labelSwitch16.SetBinding(LabelSwitch.IsToggledProperty, bindingBase114);
			translate101.Text = "ios_DashboardItemEditor_ChangeValueLowTextColor";
			IMarkupExtension markupExtension153 = translate101;
			XamlServiceProvider xamlServiceProvider153 = new XamlServiceProvider();
			Type typeFromHandle305 = typeof(IProvideValueTarget);
			object[] array153 = new object[0 + 8];
			array153[0] = labelSwitch16;
			array153[1] = stackLayout22;
			array153[2] = frame11;
			array153[3] = sfExpander9;
			array153[4] = stackLayout24;
			array153[5] = scrollView;
			array153[6] = grid25;
			array153[7] = this;
			object obj264;
			xamlServiceProvider153.Add(typeFromHandle305, obj264 = new SimpleValueTargetProvider(array153, LabelSwitch.TextProperty, nameScope));
			xamlServiceProvider153.Add(typeof(IReferenceProvider), obj264);
			Type typeFromHandle306 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver153 = new XmlNamespaceResolver();
			xmlNamespaceResolver153.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver153.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver153.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver153.Add("dash", "clr-namespace:CarScannerXamarinForms.Dashboard");
			xmlNamespaceResolver153.Add("expander", "clr-namespace:Syncfusion.XForms.Expander;assembly=Syncfusion.Expander.XForms");
			xmlNamespaceResolver153.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver153.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver153.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver153.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider153.Add(typeFromHandle306, new XamlTypeResolver(xmlNamespaceResolver153, typeof(DashboardItemEditor).GetTypeInfo().Assembly));
			xamlServiceProvider153.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(1204, 106)));
			object obj265 = markupExtension153.ProvideValue(xamlServiceProvider153);
			labelSwitch16.Text = obj265;
			stackLayout22.Children.Add(labelSwitch16);
			bindingExtension115.Mode = 2;
			bindingExtension115.Path = "ShowLowWarning";
			bindingExtension115.TypedBinding = new TypedBinding<DashboardItem, bool>(delegate(DashboardItem A_0)
			{
				if (A_0 != null)
				{
					return new ValueTuple<bool, bool>(A_0.ShowLowWarning, true);
				}
				return default(ValueTuple<bool, bool>);
			}, null, new Tuple<Func<DashboardItem, object>, string>[]
			{
				new Tuple<Func<DashboardItem, object>, string>((DashboardItem A_0) => A_0, "ShowLowWarning")
			});
			BindingBase bindingBase115 = bindingExtension115.ProvideValue(null);
			stackLayout20.SetBinding(VisualElement.IsVisibleProperty, bindingBase115);
			stackLayout20.SetValue(StackLayout.OrientationProperty, 0);
			translate102.Text = "ios_DashboardItemEditor_Threshold";
			IMarkupExtension markupExtension154 = translate102;
			XamlServiceProvider xamlServiceProvider154 = new XamlServiceProvider();
			Type typeFromHandle307 = typeof(IProvideValueTarget);
			object[] array154 = new object[0 + 9];
			array154[0] = label63;
			array154[1] = stackLayout20;
			array154[2] = stackLayout22;
			array154[3] = frame11;
			array154[4] = sfExpander9;
			array154[5] = stackLayout24;
			array154[6] = scrollView;
			array154[7] = grid25;
			array154[8] = this;
			object obj266;
			xamlServiceProvider154.Add(typeFromHandle307, obj266 = new SimpleValueTargetProvider(array154, Label.TextProperty, nameScope));
			xamlServiceProvider154.Add(typeof(IReferenceProvider), obj266);
			Type typeFromHandle308 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver154 = new XmlNamespaceResolver();
			xmlNamespaceResolver154.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver154.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver154.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver154.Add("dash", "clr-namespace:CarScannerXamarinForms.Dashboard");
			xmlNamespaceResolver154.Add("expander", "clr-namespace:Syncfusion.XForms.Expander;assembly=Syncfusion.Expander.XForms");
			xmlNamespaceResolver154.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver154.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver154.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver154.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider154.Add(typeFromHandle308, new XamlTypeResolver(xmlNamespaceResolver154, typeof(DashboardItemEditor).GetTypeInfo().Assembly));
			xamlServiceProvider154.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(1209, 48)));
			object obj267 = markupExtension154.ProvideValue(xamlServiceProvider154);
			label63.Text = obj267;
			stackLayout20.Children.Add(label63);
			bindingExtension116.Mode = 1;
			bindingExtension116.Path = "LowWarningStart";
			bindingExtension116.TypedBinding = new TypedBinding<DashboardItem, double>(delegate(DashboardItem A_0)
			{
				if (A_0 != null)
				{
					return new ValueTuple<double, bool>(A_0.LowWarningStart, true);
				}
				return default(ValueTuple<double, bool>);
			}, delegate(DashboardItem A_0, double A_1)
			{
				if (A_0 != null)
				{
					A_0.LowWarningStart = A_1;
					return;
				}
			}, new Tuple<Func<DashboardItem, object>, string>[]
			{
				new Tuple<Func<DashboardItem, object>, string>((DashboardItem A_0) => A_0, "LowWarningStart")
			});
			BindingBase bindingBase116 = bindingExtension116.ProvideValue(null);
			numericEntryV26.SetBinding(NumericEntryV3.ValueProperty, bindingBase116);
			stackLayout20.Children.Add(numericEntryV26);
			dashboardItemColorBox28.SetValue(Element.ClassIdProperty, "LowWarningColor");
			dashboardItemColorBox28.Tapped += this.ColorBox_Tapped;
			translate103.Text = "ios_DashboardItemEditor_ValueBelowThresholdColor";
			IMarkupExtension markupExtension155 = translate103;
			XamlServiceProvider xamlServiceProvider155 = new XamlServiceProvider();
			Type typeFromHandle309 = typeof(IProvideValueTarget);
			object[] array155 = new object[0 + 9];
			array155[0] = dashboardItemColorBox28;
			array155[1] = stackLayout20;
			array155[2] = stackLayout22;
			array155[3] = frame11;
			array155[4] = sfExpander9;
			array155[5] = stackLayout24;
			array155[6] = scrollView;
			array155[7] = grid25;
			array155[8] = this;
			object obj268;
			xamlServiceProvider155.Add(typeFromHandle309, obj268 = new SimpleValueTargetProvider(array155, DashboardItemColorBox.TextProperty, nameScope));
			xamlServiceProvider155.Add(typeof(IReferenceProvider), obj268);
			Type typeFromHandle310 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver155 = new XmlNamespaceResolver();
			xmlNamespaceResolver155.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver155.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver155.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver155.Add("dash", "clr-namespace:CarScannerXamarinForms.Dashboard");
			xmlNamespaceResolver155.Add("expander", "clr-namespace:Syncfusion.XForms.Expander;assembly=Syncfusion.Expander.XForms");
			xmlNamespaceResolver155.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver155.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver155.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver155.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider155.Add(typeFromHandle310, new XamlTypeResolver(xmlNamespaceResolver155, typeof(DashboardItemEditor).GetTypeInfo().Assembly));
			xamlServiceProvider155.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(1215, 45)));
			object obj269 = markupExtension155.ProvideValue(xamlServiceProvider155);
			dashboardItemColorBox28.Text = obj269;
			bindingExtension117.Mode = 2;
			bindingExtension117.Path = "LowWarningColor";
			bindingExtension117.TypedBinding = new TypedBinding<DashboardItem, Color>(delegate(DashboardItem A_0)
			{
				if (A_0 != null)
				{
					return new ValueTuple<Color, bool>(A_0.LowWarningColor, true);
				}
				return default(ValueTuple<Color, bool>);
			}, null, new Tuple<Func<DashboardItem, object>, string>[]
			{
				new Tuple<Func<DashboardItem, object>, string>((DashboardItem A_0) => A_0, "LowWarningColor")
			});
			BindingBase bindingBase117 = bindingExtension117.ProvideValue(null);
			dashboardItemColorBox28.SetBinding(DashboardItemColorBox.ColorProperty, bindingBase117);
			stackLayout20.Children.Add(dashboardItemColorBox28);
			stackLayout22.Children.Add(stackLayout20);
			bindingExtension118.Mode = 1;
			bindingExtension118.Path = "PlaySoundLow";
			bindingExtension118.TypedBinding = new TypedBinding<DashboardItem, bool>(delegate(DashboardItem A_0)
			{
				if (A_0 != null)
				{
					return new ValueTuple<bool, bool>(A_0.PlaySoundLow, true);
				}
				return default(ValueTuple<bool, bool>);
			}, delegate(DashboardItem A_0, bool A_1)
			{
				if (A_0 != null)
				{
					A_0.PlaySoundLow = A_1;
					return;
				}
			}, new Tuple<Func<DashboardItem, object>, string>[]
			{
				new Tuple<Func<DashboardItem, object>, string>((DashboardItem A_0) => A_0, "PlaySoundLow")
			});
			BindingBase bindingBase118 = bindingExtension118.ProvideValue(null);
			labelSwitch17.SetBinding(LabelSwitch.IsToggledProperty, bindingBase118);
			translate104.Text = "ios_DashboardItemEditor_PlaySoundLow";
			IMarkupExtension markupExtension156 = translate104;
			XamlServiceProvider xamlServiceProvider156 = new XamlServiceProvider();
			Type typeFromHandle311 = typeof(IProvideValueTarget);
			object[] array156 = new object[0 + 8];
			array156[0] = labelSwitch17;
			array156[1] = stackLayout22;
			array156[2] = frame11;
			array156[3] = sfExpander9;
			array156[4] = stackLayout24;
			array156[5] = scrollView;
			array156[6] = grid25;
			array156[7] = this;
			object obj270;
			xamlServiceProvider156.Add(typeFromHandle311, obj270 = new SimpleValueTargetProvider(array156, LabelSwitch.TextProperty, nameScope));
			xamlServiceProvider156.Add(typeof(IReferenceProvider), obj270);
			Type typeFromHandle312 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver156 = new XmlNamespaceResolver();
			xmlNamespaceResolver156.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver156.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver156.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver156.Add("dash", "clr-namespace:CarScannerXamarinForms.Dashboard");
			xmlNamespaceResolver156.Add("expander", "clr-namespace:Syncfusion.XForms.Expander;assembly=Syncfusion.Expander.XForms");
			xmlNamespaceResolver156.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver156.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver156.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver156.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider156.Add(typeFromHandle312, new XamlTypeResolver(xmlNamespaceResolver156, typeof(DashboardItemEditor).GetTypeInfo().Assembly));
			xamlServiceProvider156.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(1222, 104)));
			object obj271 = markupExtension156.ProvideValue(xamlServiceProvider156);
			labelSwitch17.Text = obj271;
			stackLayout22.Children.Add(labelSwitch17);
			bindingExtension119.Mode = 2;
			bindingExtension119.Path = "PlaySoundLow";
			bindingExtension119.TypedBinding = new TypedBinding<DashboardItem, bool>(delegate(DashboardItem A_0)
			{
				if (A_0 != null)
				{
					return new ValueTuple<bool, bool>(A_0.PlaySoundLow, true);
				}
				return default(ValueTuple<bool, bool>);
			}, null, new Tuple<Func<DashboardItem, object>, string>[]
			{
				new Tuple<Func<DashboardItem, object>, string>((DashboardItem A_0) => A_0, "PlaySoundLow")
			});
			BindingBase bindingBase119 = bindingExtension119.ProvideValue(null);
			stackLayout21.SetBinding(VisualElement.IsVisibleProperty, bindingBase119);
			stackLayout21.SetValue(StackLayout.OrientationProperty, 0);
			translate105.Text = "ios_DashboardItemEditor_Threshold";
			IMarkupExtension markupExtension157 = translate105;
			XamlServiceProvider xamlServiceProvider157 = new XamlServiceProvider();
			Type typeFromHandle313 = typeof(IProvideValueTarget);
			object[] array157 = new object[0 + 9];
			array157[0] = label64;
			array157[1] = stackLayout21;
			array157[2] = stackLayout22;
			array157[3] = frame11;
			array157[4] = sfExpander9;
			array157[5] = stackLayout24;
			array157[6] = scrollView;
			array157[7] = grid25;
			array157[8] = this;
			object obj272;
			xamlServiceProvider157.Add(typeFromHandle313, obj272 = new SimpleValueTargetProvider(array157, Label.TextProperty, nameScope));
			xamlServiceProvider157.Add(typeof(IReferenceProvider), obj272);
			Type typeFromHandle314 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver157 = new XmlNamespaceResolver();
			xmlNamespaceResolver157.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver157.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver157.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver157.Add("dash", "clr-namespace:CarScannerXamarinForms.Dashboard");
			xmlNamespaceResolver157.Add("expander", "clr-namespace:Syncfusion.XForms.Expander;assembly=Syncfusion.Expander.XForms");
			xmlNamespaceResolver157.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver157.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver157.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver157.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider157.Add(typeFromHandle314, new XamlTypeResolver(xmlNamespaceResolver157, typeof(DashboardItemEditor).GetTypeInfo().Assembly));
			xamlServiceProvider157.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(1226, 48)));
			object obj273 = markupExtension157.ProvideValue(xamlServiceProvider157);
			label64.Text = obj273;
			stackLayout21.Children.Add(label64);
			bindingExtension120.Mode = 1;
			bindingExtension120.Path = "SoundStartLow";
			bindingExtension120.TypedBinding = new TypedBinding<DashboardItem, double>(delegate(DashboardItem A_0)
			{
				if (A_0 != null)
				{
					return new ValueTuple<double, bool>(A_0.SoundStartLow, true);
				}
				return default(ValueTuple<double, bool>);
			}, delegate(DashboardItem A_0, double A_1)
			{
				if (A_0 != null)
				{
					A_0.SoundStartLow = A_1;
					return;
				}
			}, new Tuple<Func<DashboardItem, object>, string>[]
			{
				new Tuple<Func<DashboardItem, object>, string>((DashboardItem A_0) => A_0, "SoundStartLow")
			});
			BindingBase bindingBase120 = bindingExtension120.ProvideValue(null);
			numericEntryV27.SetBinding(NumericEntryV3.ValueProperty, bindingBase120);
			stackLayout21.Children.Add(numericEntryV27);
			translate106.Text = "ios_DashboardItemEditor_SoundName";
			IMarkupExtension markupExtension158 = translate106;
			XamlServiceProvider xamlServiceProvider158 = new XamlServiceProvider();
			Type typeFromHandle315 = typeof(IProvideValueTarget);
			object[] array158 = new object[0 + 9];
			array158[0] = label65;
			array158[1] = stackLayout21;
			array158[2] = stackLayout22;
			array158[3] = frame11;
			array158[4] = sfExpander9;
			array158[5] = stackLayout24;
			array158[6] = scrollView;
			array158[7] = grid25;
			array158[8] = this;
			object obj274;
			xamlServiceProvider158.Add(typeFromHandle315, obj274 = new SimpleValueTargetProvider(array158, Label.TextProperty, nameScope));
			xamlServiceProvider158.Add(typeof(IReferenceProvider), obj274);
			Type typeFromHandle316 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver158 = new XmlNamespaceResolver();
			xmlNamespaceResolver158.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver158.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver158.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver158.Add("dash", "clr-namespace:CarScannerXamarinForms.Dashboard");
			xmlNamespaceResolver158.Add("expander", "clr-namespace:Syncfusion.XForms.Expander;assembly=Syncfusion.Expander.XForms");
			xmlNamespaceResolver158.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver158.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver158.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver158.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider158.Add(typeFromHandle316, new XamlTypeResolver(xmlNamespaceResolver158, typeof(DashboardItemEditor).GetTypeInfo().Assembly));
			xamlServiceProvider158.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(1229, 48)));
			object obj275 = markupExtension158.ProvideValue(xamlServiceProvider158);
			label65.Text = obj275;
			stackLayout21.Children.Add(label65);
			columnDefinition29.SetValue(ColumnDefinition.WidthProperty, new GridLengthTypeConverter().ConvertFromInvariantString("*"));
			grid23.GetValue(Grid.ColumnDefinitionsProperty).Add(columnDefinition29);
			columnDefinition30.SetValue(ColumnDefinition.WidthProperty, new GridLengthTypeConverter().ConvertFromInvariantString("auto"));
			grid23.GetValue(Grid.ColumnDefinitionsProperty).Add(columnDefinition30);
			picker5.SetValue(Grid.ColumnProperty, 0);
			bindingExtension121.Source = soundsList2;
			BindingBase bindingBase121 = bindingExtension121.ProvideValue(null);
			picker5.SetBinding(Picker.ItemsSourceProperty, bindingBase121);
			bindingExtension122.Mode = 1;
			bindingExtension122.Path = "SoundNameLow";
			bindingExtension122.TypedBinding = new TypedBinding<DashboardItem, string>(delegate(DashboardItem A_0)
			{
				if (A_0 != null)
				{
					return new ValueTuple<string, bool>(A_0.SoundNameLow, true);
				}
				return default(ValueTuple<string, bool>);
			}, delegate(DashboardItem A_0, string A_1)
			{
				if (A_0 != null)
				{
					A_0.SoundNameLow = A_1;
					return;
				}
			}, new Tuple<Func<DashboardItem, object>, string>[]
			{
				new Tuple<Func<DashboardItem, object>, string>((DashboardItem A_0) => A_0, "SoundNameLow")
			});
			BindingBase bindingBase122 = bindingExtension122.ProvideValue(null);
			picker5.SetBinding(Picker.SelectedItemProperty, bindingBase122);
			grid23.Children.Add(picker5);
			image2.SetValue(Grid.ColumnProperty, 1);
			image2.SetValue(VisualElement.BackgroundColorProperty, Color.White);
			image2.SetValue(Image.SourceProperty, new ImageSourceConverter().ConvertFromInvariantString("icons8_play.png"));
			tapGestureRecognizer2.SetValue(TapGestureRecognizer.NumberOfTapsRequiredProperty, 1);
			tapGestureRecognizer2.Tapped += this.playLow_Tapped;
			image2.GestureRecognizers.Add(tapGestureRecognizer2);
			grid23.Children.Add(image2);
			stackLayout21.Children.Add(grid23);
			stackLayout22.Children.Add(stackLayout21);
			frame11.SetValue(ContentView.ContentProperty, stackLayout22);
			sfExpander9.SetValue(SfExpander.ContentProperty, frame11);
			stackLayout24.Children.Add(sfExpander9);
			sfExpander10.SetValue(SfExpander.AnimationDurationProperty, 250.0);
			sfExpander10.SetValue(SfExpander.AnimationEasingProperty, 3);
			dynamicResourceExtension40.Key = "NavigationBarBackgroundColor";
			IMarkupExtension<DynamicResource> markupExtension159 = dynamicResourceExtension40;
			XamlServiceProvider xamlServiceProvider159 = new XamlServiceProvider();
			Type typeFromHandle317 = typeof(IProvideValueTarget);
			object[] array159 = new object[0 + 5];
			array159[0] = sfExpander10;
			array159[1] = stackLayout24;
			array159[2] = scrollView;
			array159[3] = grid25;
			array159[4] = this;
			object obj276;
			xamlServiceProvider159.Add(typeFromHandle317, obj276 = new SimpleValueTargetProvider(array159, SfExpander.HeaderBackgroundColorProperty, nameScope));
			xamlServiceProvider159.Add(typeof(IReferenceProvider), obj276);
			Type typeFromHandle318 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver159 = new XmlNamespaceResolver();
			xmlNamespaceResolver159.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver159.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver159.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver159.Add("dash", "clr-namespace:CarScannerXamarinForms.Dashboard");
			xmlNamespaceResolver159.Add("expander", "clr-namespace:Syncfusion.XForms.Expander;assembly=Syncfusion.Expander.XForms");
			xmlNamespaceResolver159.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver159.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver159.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver159.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider159.Add(typeFromHandle318, new XamlTypeResolver(xmlNamespaceResolver159, typeof(DashboardItemEditor).GetTypeInfo().Assembly));
			xamlServiceProvider159.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(1262, 25)));
			DynamicResource dynamicResource40 = markupExtension159.ProvideValue(xamlServiceProvider159);
			sfExpander10.SetDynamicResource(SfExpander.HeaderBackgroundColorProperty, dynamicResource40.Key);
			dynamicResourceExtension41.Key = "NavigationBarButtonColor";
			IMarkupExtension<DynamicResource> markupExtension160 = dynamicResourceExtension41;
			XamlServiceProvider xamlServiceProvider160 = new XamlServiceProvider();
			Type typeFromHandle319 = typeof(IProvideValueTarget);
			object[] array160 = new object[0 + 5];
			array160[0] = sfExpander10;
			array160[1] = stackLayout24;
			array160[2] = scrollView;
			array160[3] = grid25;
			array160[4] = this;
			object obj277;
			xamlServiceProvider160.Add(typeFromHandle319, obj277 = new SimpleValueTargetProvider(array160, SfExpander.IconColorProperty, nameScope));
			xamlServiceProvider160.Add(typeof(IReferenceProvider), obj277);
			Type typeFromHandle320 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver160 = new XmlNamespaceResolver();
			xmlNamespaceResolver160.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver160.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver160.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver160.Add("dash", "clr-namespace:CarScannerXamarinForms.Dashboard");
			xmlNamespaceResolver160.Add("expander", "clr-namespace:Syncfusion.XForms.Expander;assembly=Syncfusion.Expander.XForms");
			xmlNamespaceResolver160.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver160.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver160.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver160.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider160.Add(typeFromHandle320, new XamlTypeResolver(xmlNamespaceResolver160, typeof(DashboardItemEditor).GetTypeInfo().Assembly));
			xamlServiceProvider160.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(1263, 25)));
			DynamicResource dynamicResource41 = markupExtension160.ProvideValue(xamlServiceProvider160);
			sfExpander10.SetDynamicResource(SfExpander.IconColorProperty, dynamicResource41.Key);
			sfExpander10.SetValue(SfExpander.IsExpandedProperty, false);
			label66.SetValue(View.MarginProperty, new Thickness(5.0));
			label66.SetValue(Label.FontAttributesProperty, new FontAttributesConverter().ConvertFromInvariantString("Bold"));
			translate107.Text = "DashItem_ApplyTo";
			IMarkupExtension markupExtension161 = translate107;
			XamlServiceProvider xamlServiceProvider161 = new XamlServiceProvider();
			Type typeFromHandle321 = typeof(IProvideValueTarget);
			object[] array161 = new object[0 + 7];
			array161[0] = label66;
			array161[1] = grid24;
			array161[2] = sfExpander10;
			array161[3] = stackLayout24;
			array161[4] = scrollView;
			array161[5] = grid25;
			array161[6] = this;
			object obj278;
			xamlServiceProvider161.Add(typeFromHandle321, obj278 = new SimpleValueTargetProvider(array161, Label.TextProperty, nameScope));
			xamlServiceProvider161.Add(typeof(IReferenceProvider), obj278);
			Type typeFromHandle322 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver161 = new XmlNamespaceResolver();
			xmlNamespaceResolver161.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver161.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver161.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver161.Add("dash", "clr-namespace:CarScannerXamarinForms.Dashboard");
			xmlNamespaceResolver161.Add("expander", "clr-namespace:Syncfusion.XForms.Expander;assembly=Syncfusion.Expander.XForms");
			xmlNamespaceResolver161.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver161.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver161.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver161.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider161.Add(typeFromHandle322, new XamlTypeResolver(xmlNamespaceResolver161, typeof(DashboardItemEditor).GetTypeInfo().Assembly));
			xamlServiceProvider161.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(1270, 37)));
			object obj279 = markupExtension161.ProvideValue(xamlServiceProvider161);
			label66.Text = obj279;
			dynamicResourceExtension42.Key = "NavigationBarTextColor";
			IMarkupExtension<DynamicResource> markupExtension162 = dynamicResourceExtension42;
			XamlServiceProvider xamlServiceProvider162 = new XamlServiceProvider();
			Type typeFromHandle323 = typeof(IProvideValueTarget);
			object[] array162 = new object[0 + 7];
			array162[0] = label66;
			array162[1] = grid24;
			array162[2] = sfExpander10;
			array162[3] = stackLayout24;
			array162[4] = scrollView;
			array162[5] = grid25;
			array162[6] = this;
			object obj280;
			xamlServiceProvider162.Add(typeFromHandle323, obj280 = new SimpleValueTargetProvider(array162, Label.TextColorProperty, nameScope));
			xamlServiceProvider162.Add(typeof(IReferenceProvider), obj280);
			Type typeFromHandle324 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver162 = new XmlNamespaceResolver();
			xmlNamespaceResolver162.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver162.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver162.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver162.Add("dash", "clr-namespace:CarScannerXamarinForms.Dashboard");
			xmlNamespaceResolver162.Add("expander", "clr-namespace:Syncfusion.XForms.Expander;assembly=Syncfusion.Expander.XForms");
			xmlNamespaceResolver162.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver162.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver162.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver162.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider162.Add(typeFromHandle324, new XamlTypeResolver(xmlNamespaceResolver162, typeof(DashboardItemEditor).GetTypeInfo().Assembly));
			xamlServiceProvider162.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(1271, 37)));
			DynamicResource dynamicResource42 = markupExtension162.ProvideValue(xamlServiceProvider162);
			label66.SetDynamicResource(Label.TextColorProperty, dynamicResource42.Key);
			label66.SetValue(View.VerticalOptionsProperty, LayoutOptions.Center);
			label66.SetValue(Label.VerticalTextAlignmentProperty, new TextAlignmentConverter().ConvertFromInvariantString("Center"));
			grid24.Children.Add(label66);
			sfExpander10.SetValue(SfExpander.HeaderProperty, grid24);
			stackLayout23.SetValue(StackLayout.OrientationProperty, 0);
			translate108.Text = "ios_DashboardItemEditor_ApplyToPage";
			IMarkupExtension markupExtension163 = translate108;
			XamlServiceProvider xamlServiceProvider163 = new XamlServiceProvider();
			Type typeFromHandle325 = typeof(IProvideValueTarget);
			object[] array163 = new object[0 + 8];
			array163[0] = label67;
			array163[1] = stackLayout23;
			array163[2] = frame12;
			array163[3] = sfExpander10;
			array163[4] = stackLayout24;
			array163[5] = scrollView;
			array163[6] = grid25;
			array163[7] = this;
			object obj281;
			xamlServiceProvider163.Add(typeFromHandle325, obj281 = new SimpleValueTargetProvider(array163, Label.TextProperty, nameScope));
			xamlServiceProvider163.Add(typeof(IReferenceProvider), obj281);
			Type typeFromHandle326 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver163 = new XmlNamespaceResolver();
			xmlNamespaceResolver163.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver163.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver163.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver163.Add("dash", "clr-namespace:CarScannerXamarinForms.Dashboard");
			xmlNamespaceResolver163.Add("expander", "clr-namespace:Syncfusion.XForms.Expander;assembly=Syncfusion.Expander.XForms");
			xmlNamespaceResolver163.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver163.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver163.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver163.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider163.Add(typeFromHandle326, new XamlTypeResolver(xmlNamespaceResolver163, typeof(DashboardItemEditor).GetTypeInfo().Assembly));
			xamlServiceProvider163.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(1279, 44)));
			object obj282 = markupExtension163.ProvideValue(xamlServiceProvider163);
			label67.Text = obj282;
			stackLayout23.Children.Add(label67);
			button3.Clicked += this.ApplyCurrentPage_Clicked;
			translate109.Text = "ios_DashboardItemEditor_ApplyTo";
			IMarkupExtension markupExtension164 = translate109;
			XamlServiceProvider xamlServiceProvider164 = new XamlServiceProvider();
			Type typeFromHandle327 = typeof(IProvideValueTarget);
			object[] array164 = new object[0 + 8];
			array164[0] = button3;
			array164[1] = stackLayout23;
			array164[2] = frame12;
			array164[3] = sfExpander10;
			array164[4] = stackLayout24;
			array164[5] = scrollView;
			array164[6] = grid25;
			array164[7] = this;
			object obj283;
			xamlServiceProvider164.Add(typeFromHandle327, obj283 = new SimpleValueTargetProvider(array164, Button.TextProperty, nameScope));
			xamlServiceProvider164.Add(typeof(IReferenceProvider), obj283);
			Type typeFromHandle328 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver164 = new XmlNamespaceResolver();
			xmlNamespaceResolver164.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver164.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver164.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver164.Add("dash", "clr-namespace:CarScannerXamarinForms.Dashboard");
			xmlNamespaceResolver164.Add("expander", "clr-namespace:Syncfusion.XForms.Expander;assembly=Syncfusion.Expander.XForms");
			xmlNamespaceResolver164.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver164.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver164.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver164.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider164.Add(typeFromHandle328, new XamlTypeResolver(xmlNamespaceResolver164, typeof(DashboardItemEditor).GetTypeInfo().Assembly));
			xamlServiceProvider164.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(1280, 80)));
			object obj284 = markupExtension164.ProvideValue(xamlServiceProvider164);
			button3.Text = obj284;
			stackLayout23.Children.Add(button3);
			translate110.Text = "ios_DashboardItemEditor_ApplyToPageAllTypes";
			IMarkupExtension markupExtension165 = translate110;
			XamlServiceProvider xamlServiceProvider165 = new XamlServiceProvider();
			Type typeFromHandle329 = typeof(IProvideValueTarget);
			object[] array165 = new object[0 + 8];
			array165[0] = label68;
			array165[1] = stackLayout23;
			array165[2] = frame12;
			array165[3] = sfExpander10;
			array165[4] = stackLayout24;
			array165[5] = scrollView;
			array165[6] = grid25;
			array165[7] = this;
			object obj285;
			xamlServiceProvider165.Add(typeFromHandle329, obj285 = new SimpleValueTargetProvider(array165, Label.TextProperty, nameScope));
			xamlServiceProvider165.Add(typeof(IReferenceProvider), obj285);
			Type typeFromHandle330 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver165 = new XmlNamespaceResolver();
			xmlNamespaceResolver165.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver165.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver165.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver165.Add("dash", "clr-namespace:CarScannerXamarinForms.Dashboard");
			xmlNamespaceResolver165.Add("expander", "clr-namespace:Syncfusion.XForms.Expander;assembly=Syncfusion.Expander.XForms");
			xmlNamespaceResolver165.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver165.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver165.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver165.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider165.Add(typeFromHandle330, new XamlTypeResolver(xmlNamespaceResolver165, typeof(DashboardItemEditor).GetTypeInfo().Assembly));
			xamlServiceProvider165.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(1282, 44)));
			object obj286 = markupExtension165.ProvideValue(xamlServiceProvider165);
			label68.Text = obj286;
			stackLayout23.Children.Add(label68);
			button4.Clicked += this.ApplyCurrentPageAllTypes_Clicked;
			translate111.Text = "ios_DashboardItemEditor_ApplyTo";
			IMarkupExtension markupExtension166 = translate111;
			XamlServiceProvider xamlServiceProvider166 = new XamlServiceProvider();
			Type typeFromHandle331 = typeof(IProvideValueTarget);
			object[] array166 = new object[0 + 8];
			array166[0] = button4;
			array166[1] = stackLayout23;
			array166[2] = frame12;
			array166[3] = sfExpander10;
			array166[4] = stackLayout24;
			array166[5] = scrollView;
			array166[6] = grid25;
			array166[7] = this;
			object obj287;
			xamlServiceProvider166.Add(typeFromHandle331, obj287 = new SimpleValueTargetProvider(array166, Button.TextProperty, nameScope));
			xamlServiceProvider166.Add(typeof(IReferenceProvider), obj287);
			Type typeFromHandle332 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver166 = new XmlNamespaceResolver();
			xmlNamespaceResolver166.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver166.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver166.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver166.Add("dash", "clr-namespace:CarScannerXamarinForms.Dashboard");
			xmlNamespaceResolver166.Add("expander", "clr-namespace:Syncfusion.XForms.Expander;assembly=Syncfusion.Expander.XForms");
			xmlNamespaceResolver166.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver166.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver166.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver166.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider166.Add(typeFromHandle332, new XamlTypeResolver(xmlNamespaceResolver166, typeof(DashboardItemEditor).GetTypeInfo().Assembly));
			xamlServiceProvider166.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(1283, 88)));
			object obj288 = markupExtension166.ProvideValue(xamlServiceProvider166);
			button4.Text = obj288;
			stackLayout23.Children.Add(button4);
			translate112.Text = "ios_DashboardItemEditor_ApplyToAllPages";
			IMarkupExtension markupExtension167 = translate112;
			XamlServiceProvider xamlServiceProvider167 = new XamlServiceProvider();
			Type typeFromHandle333 = typeof(IProvideValueTarget);
			object[] array167 = new object[0 + 8];
			array167[0] = label69;
			array167[1] = stackLayout23;
			array167[2] = frame12;
			array167[3] = sfExpander10;
			array167[4] = stackLayout24;
			array167[5] = scrollView;
			array167[6] = grid25;
			array167[7] = this;
			object obj289;
			xamlServiceProvider167.Add(typeFromHandle333, obj289 = new SimpleValueTargetProvider(array167, Label.TextProperty, nameScope));
			xamlServiceProvider167.Add(typeof(IReferenceProvider), obj289);
			Type typeFromHandle334 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver167 = new XmlNamespaceResolver();
			xmlNamespaceResolver167.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver167.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver167.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver167.Add("dash", "clr-namespace:CarScannerXamarinForms.Dashboard");
			xmlNamespaceResolver167.Add("expander", "clr-namespace:Syncfusion.XForms.Expander;assembly=Syncfusion.Expander.XForms");
			xmlNamespaceResolver167.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver167.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver167.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver167.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider167.Add(typeFromHandle334, new XamlTypeResolver(xmlNamespaceResolver167, typeof(DashboardItemEditor).GetTypeInfo().Assembly));
			xamlServiceProvider167.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(1285, 44)));
			object obj290 = markupExtension167.ProvideValue(xamlServiceProvider167);
			label69.Text = obj290;
			stackLayout23.Children.Add(label69);
			button5.Clicked += this.ApplyAllPages_Clicked;
			translate113.Text = "ios_DashboardItemEditor_ApplyTo";
			IMarkupExtension markupExtension168 = translate113;
			XamlServiceProvider xamlServiceProvider168 = new XamlServiceProvider();
			Type typeFromHandle335 = typeof(IProvideValueTarget);
			object[] array168 = new object[0 + 8];
			array168[0] = button5;
			array168[1] = stackLayout23;
			array168[2] = frame12;
			array168[3] = sfExpander10;
			array168[4] = stackLayout24;
			array168[5] = scrollView;
			array168[6] = grid25;
			array168[7] = this;
			object obj291;
			xamlServiceProvider168.Add(typeFromHandle335, obj291 = new SimpleValueTargetProvider(array168, Button.TextProperty, nameScope));
			xamlServiceProvider168.Add(typeof(IReferenceProvider), obj291);
			Type typeFromHandle336 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver168 = new XmlNamespaceResolver();
			xmlNamespaceResolver168.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver168.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver168.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver168.Add("dash", "clr-namespace:CarScannerXamarinForms.Dashboard");
			xmlNamespaceResolver168.Add("expander", "clr-namespace:Syncfusion.XForms.Expander;assembly=Syncfusion.Expander.XForms");
			xmlNamespaceResolver168.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver168.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver168.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver168.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider168.Add(typeFromHandle336, new XamlTypeResolver(xmlNamespaceResolver168, typeof(DashboardItemEditor).GetTypeInfo().Assembly));
			xamlServiceProvider168.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(1286, 77)));
			object obj292 = markupExtension168.ProvideValue(xamlServiceProvider168);
			button5.Text = obj292;
			stackLayout23.Children.Add(button5);
			translate114.Text = "ios_DashboardItemEditor_ApplyColorToPageAllTypes";
			IMarkupExtension markupExtension169 = translate114;
			XamlServiceProvider xamlServiceProvider169 = new XamlServiceProvider();
			Type typeFromHandle337 = typeof(IProvideValueTarget);
			object[] array169 = new object[0 + 8];
			array169[0] = label70;
			array169[1] = stackLayout23;
			array169[2] = frame12;
			array169[3] = sfExpander10;
			array169[4] = stackLayout24;
			array169[5] = scrollView;
			array169[6] = grid25;
			array169[7] = this;
			object obj293;
			xamlServiceProvider169.Add(typeFromHandle337, obj293 = new SimpleValueTargetProvider(array169, Label.TextProperty, nameScope));
			xamlServiceProvider169.Add(typeof(IReferenceProvider), obj293);
			Type typeFromHandle338 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver169 = new XmlNamespaceResolver();
			xmlNamespaceResolver169.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver169.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver169.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver169.Add("dash", "clr-namespace:CarScannerXamarinForms.Dashboard");
			xmlNamespaceResolver169.Add("expander", "clr-namespace:Syncfusion.XForms.Expander;assembly=Syncfusion.Expander.XForms");
			xmlNamespaceResolver169.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver169.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver169.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver169.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider169.Add(typeFromHandle338, new XamlTypeResolver(xmlNamespaceResolver169, typeof(DashboardItemEditor).GetTypeInfo().Assembly));
			xamlServiceProvider169.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(1288, 44)));
			object obj294 = markupExtension169.ProvideValue(xamlServiceProvider169);
			label70.Text = obj294;
			stackLayout23.Children.Add(label70);
			button6.Clicked += this.ApplyColorCurrentPageAllTypes_Clicked;
			translate115.Text = "ios_DashboardItemEditor_ApplyTo";
			IMarkupExtension markupExtension170 = translate115;
			XamlServiceProvider xamlServiceProvider170 = new XamlServiceProvider();
			Type typeFromHandle339 = typeof(IProvideValueTarget);
			object[] array170 = new object[0 + 8];
			array170[0] = button6;
			array170[1] = stackLayout23;
			array170[2] = frame12;
			array170[3] = sfExpander10;
			array170[4] = stackLayout24;
			array170[5] = scrollView;
			array170[6] = grid25;
			array170[7] = this;
			object obj295;
			xamlServiceProvider170.Add(typeFromHandle339, obj295 = new SimpleValueTargetProvider(array170, Button.TextProperty, nameScope));
			xamlServiceProvider170.Add(typeof(IReferenceProvider), obj295);
			Type typeFromHandle340 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver170 = new XmlNamespaceResolver();
			xmlNamespaceResolver170.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver170.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver170.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver170.Add("dash", "clr-namespace:CarScannerXamarinForms.Dashboard");
			xmlNamespaceResolver170.Add("expander", "clr-namespace:Syncfusion.XForms.Expander;assembly=Syncfusion.Expander.XForms");
			xmlNamespaceResolver170.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver170.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver170.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver170.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider170.Add(typeFromHandle340, new XamlTypeResolver(xmlNamespaceResolver170, typeof(DashboardItemEditor).GetTypeInfo().Assembly));
			xamlServiceProvider170.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(1289, 93)));
			object obj296 = markupExtension170.ProvideValue(xamlServiceProvider170);
			button6.Text = obj296;
			stackLayout23.Children.Add(button6);
			translate116.Text = "ios_DashboardItemEditor_ApplyColorToPage";
			IMarkupExtension markupExtension171 = translate116;
			XamlServiceProvider xamlServiceProvider171 = new XamlServiceProvider();
			Type typeFromHandle341 = typeof(IProvideValueTarget);
			object[] array171 = new object[0 + 8];
			array171[0] = label71;
			array171[1] = stackLayout23;
			array171[2] = frame12;
			array171[3] = sfExpander10;
			array171[4] = stackLayout24;
			array171[5] = scrollView;
			array171[6] = grid25;
			array171[7] = this;
			object obj297;
			xamlServiceProvider171.Add(typeFromHandle341, obj297 = new SimpleValueTargetProvider(array171, Label.TextProperty, nameScope));
			xamlServiceProvider171.Add(typeof(IReferenceProvider), obj297);
			Type typeFromHandle342 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver171 = new XmlNamespaceResolver();
			xmlNamespaceResolver171.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver171.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver171.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver171.Add("dash", "clr-namespace:CarScannerXamarinForms.Dashboard");
			xmlNamespaceResolver171.Add("expander", "clr-namespace:Syncfusion.XForms.Expander;assembly=Syncfusion.Expander.XForms");
			xmlNamespaceResolver171.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver171.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver171.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver171.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider171.Add(typeFromHandle342, new XamlTypeResolver(xmlNamespaceResolver171, typeof(DashboardItemEditor).GetTypeInfo().Assembly));
			xamlServiceProvider171.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(1291, 44)));
			object obj298 = markupExtension171.ProvideValue(xamlServiceProvider171);
			label71.Text = obj298;
			stackLayout23.Children.Add(label71);
			button7.Clicked += this.ApplyColorsCurrentPage_Clicked;
			translate117.Text = "ios_DashboardItemEditor_ApplyTo";
			IMarkupExtension markupExtension172 = translate117;
			XamlServiceProvider xamlServiceProvider172 = new XamlServiceProvider();
			Type typeFromHandle343 = typeof(IProvideValueTarget);
			object[] array172 = new object[0 + 8];
			array172[0] = button7;
			array172[1] = stackLayout23;
			array172[2] = frame12;
			array172[3] = sfExpander10;
			array172[4] = stackLayout24;
			array172[5] = scrollView;
			array172[6] = grid25;
			array172[7] = this;
			object obj299;
			xamlServiceProvider172.Add(typeFromHandle343, obj299 = new SimpleValueTargetProvider(array172, Button.TextProperty, nameScope));
			xamlServiceProvider172.Add(typeof(IReferenceProvider), obj299);
			Type typeFromHandle344 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver172 = new XmlNamespaceResolver();
			xmlNamespaceResolver172.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver172.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver172.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver172.Add("dash", "clr-namespace:CarScannerXamarinForms.Dashboard");
			xmlNamespaceResolver172.Add("expander", "clr-namespace:Syncfusion.XForms.Expander;assembly=Syncfusion.Expander.XForms");
			xmlNamespaceResolver172.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver172.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver172.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver172.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider172.Add(typeFromHandle344, new XamlTypeResolver(xmlNamespaceResolver172, typeof(DashboardItemEditor).GetTypeInfo().Assembly));
			xamlServiceProvider172.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(1292, 86)));
			object obj300 = markupExtension172.ProvideValue(xamlServiceProvider172);
			button7.Text = obj300;
			stackLayout23.Children.Add(button7);
			translate118.Text = "ios_DashboardItemEditor_ApplyColorToAllPagesSameType";
			IMarkupExtension markupExtension173 = translate118;
			XamlServiceProvider xamlServiceProvider173 = new XamlServiceProvider();
			Type typeFromHandle345 = typeof(IProvideValueTarget);
			object[] array173 = new object[0 + 8];
			array173[0] = label72;
			array173[1] = stackLayout23;
			array173[2] = frame12;
			array173[3] = sfExpander10;
			array173[4] = stackLayout24;
			array173[5] = scrollView;
			array173[6] = grid25;
			array173[7] = this;
			object obj301;
			xamlServiceProvider173.Add(typeFromHandle345, obj301 = new SimpleValueTargetProvider(array173, Label.TextProperty, nameScope));
			xamlServiceProvider173.Add(typeof(IReferenceProvider), obj301);
			Type typeFromHandle346 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver173 = new XmlNamespaceResolver();
			xmlNamespaceResolver173.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver173.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver173.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver173.Add("dash", "clr-namespace:CarScannerXamarinForms.Dashboard");
			xmlNamespaceResolver173.Add("expander", "clr-namespace:Syncfusion.XForms.Expander;assembly=Syncfusion.Expander.XForms");
			xmlNamespaceResolver173.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver173.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver173.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver173.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider173.Add(typeFromHandle346, new XamlTypeResolver(xmlNamespaceResolver173, typeof(DashboardItemEditor).GetTypeInfo().Assembly));
			xamlServiceProvider173.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(1294, 44)));
			object obj302 = markupExtension173.ProvideValue(xamlServiceProvider173);
			label72.Text = obj302;
			stackLayout23.Children.Add(label72);
			button8.Clicked += this.ApplyColorAllPagesSameType_Clicked;
			translate119.Text = "ios_DashboardItemEditor_ApplyTo";
			IMarkupExtension markupExtension174 = translate119;
			XamlServiceProvider xamlServiceProvider174 = new XamlServiceProvider();
			Type typeFromHandle347 = typeof(IProvideValueTarget);
			object[] array174 = new object[0 + 8];
			array174[0] = button8;
			array174[1] = stackLayout23;
			array174[2] = frame12;
			array174[3] = sfExpander10;
			array174[4] = stackLayout24;
			array174[5] = scrollView;
			array174[6] = grid25;
			array174[7] = this;
			object obj303;
			xamlServiceProvider174.Add(typeFromHandle347, obj303 = new SimpleValueTargetProvider(array174, Button.TextProperty, nameScope));
			xamlServiceProvider174.Add(typeof(IReferenceProvider), obj303);
			Type typeFromHandle348 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver174 = new XmlNamespaceResolver();
			xmlNamespaceResolver174.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver174.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver174.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver174.Add("dash", "clr-namespace:CarScannerXamarinForms.Dashboard");
			xmlNamespaceResolver174.Add("expander", "clr-namespace:Syncfusion.XForms.Expander;assembly=Syncfusion.Expander.XForms");
			xmlNamespaceResolver174.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver174.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver174.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver174.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider174.Add(typeFromHandle348, new XamlTypeResolver(xmlNamespaceResolver174, typeof(DashboardItemEditor).GetTypeInfo().Assembly));
			xamlServiceProvider174.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(1295, 90)));
			object obj304 = markupExtension174.ProvideValue(xamlServiceProvider174);
			button8.Text = obj304;
			stackLayout23.Children.Add(button8);
			translate120.Text = "ios_DashboardItemEditor_ApplyColorToAllPages";
			IMarkupExtension markupExtension175 = translate120;
			XamlServiceProvider xamlServiceProvider175 = new XamlServiceProvider();
			Type typeFromHandle349 = typeof(IProvideValueTarget);
			object[] array175 = new object[0 + 8];
			array175[0] = label73;
			array175[1] = stackLayout23;
			array175[2] = frame12;
			array175[3] = sfExpander10;
			array175[4] = stackLayout24;
			array175[5] = scrollView;
			array175[6] = grid25;
			array175[7] = this;
			object obj305;
			xamlServiceProvider175.Add(typeFromHandle349, obj305 = new SimpleValueTargetProvider(array175, Label.TextProperty, nameScope));
			xamlServiceProvider175.Add(typeof(IReferenceProvider), obj305);
			Type typeFromHandle350 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver175 = new XmlNamespaceResolver();
			xmlNamespaceResolver175.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver175.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver175.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver175.Add("dash", "clr-namespace:CarScannerXamarinForms.Dashboard");
			xmlNamespaceResolver175.Add("expander", "clr-namespace:Syncfusion.XForms.Expander;assembly=Syncfusion.Expander.XForms");
			xmlNamespaceResolver175.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver175.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver175.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver175.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider175.Add(typeFromHandle350, new XamlTypeResolver(xmlNamespaceResolver175, typeof(DashboardItemEditor).GetTypeInfo().Assembly));
			xamlServiceProvider175.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(1297, 44)));
			object obj306 = markupExtension175.ProvideValue(xamlServiceProvider175);
			label73.Text = obj306;
			stackLayout23.Children.Add(label73);
			button9.Clicked += this.ApplyColorAllPages_Clicked;
			translate121.Text = "ios_DashboardItemEditor_ApplyTo";
			IMarkupExtension markupExtension176 = translate121;
			XamlServiceProvider xamlServiceProvider176 = new XamlServiceProvider();
			Type typeFromHandle351 = typeof(IProvideValueTarget);
			object[] array176 = new object[0 + 8];
			array176[0] = button9;
			array176[1] = stackLayout23;
			array176[2] = frame12;
			array176[3] = sfExpander10;
			array176[4] = stackLayout24;
			array176[5] = scrollView;
			array176[6] = grid25;
			array176[7] = this;
			object obj307;
			xamlServiceProvider176.Add(typeFromHandle351, obj307 = new SimpleValueTargetProvider(array176, Button.TextProperty, nameScope));
			xamlServiceProvider176.Add(typeof(IReferenceProvider), obj307);
			Type typeFromHandle352 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver176 = new XmlNamespaceResolver();
			xmlNamespaceResolver176.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver176.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver176.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver176.Add("dash", "clr-namespace:CarScannerXamarinForms.Dashboard");
			xmlNamespaceResolver176.Add("expander", "clr-namespace:Syncfusion.XForms.Expander;assembly=Syncfusion.Expander.XForms");
			xmlNamespaceResolver176.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver176.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver176.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver176.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider176.Add(typeFromHandle352, new XamlTypeResolver(xmlNamespaceResolver176, typeof(DashboardItemEditor).GetTypeInfo().Assembly));
			xamlServiceProvider176.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(1298, 82)));
			object obj308 = markupExtension176.ProvideValue(xamlServiceProvider176);
			button9.Text = obj308;
			stackLayout23.Children.Add(button9);
			frame12.SetValue(ContentView.ContentProperty, stackLayout23);
			sfExpander10.SetValue(SfExpander.ContentProperty, frame12);
			stackLayout24.Children.Add(sfExpander10);
			scrollView.Content = stackLayout24;
			grid25.Children.Add(scrollView);
			this.SetValue(ContentPage.ContentProperty, grid25);
		}

		// Token: 0x06001780 RID: 6016 RVA: 0x000D4776 File Offset: 0x000D2976
		[CompilerGenerated]
		private bool <btnBack_Clicked>b__11_0(DashboardPage x)
		{
			return x.Items.Contains(this.originalItem);
		}

		// Token: 0x06001781 RID: 6017 RVA: 0x000D4776 File Offset: 0x000D2976
		[CompilerGenerated]
		private bool <ApplyCurrentPage_Clicked>b__21_0(DashboardPage x)
		{
			return x.Items.Contains(this.originalItem);
		}

		// Token: 0x06001782 RID: 6018 RVA: 0x000D4776 File Offset: 0x000D2976
		[CompilerGenerated]
		private bool <ApplyColorsCurrentPage_Clicked>b__22_0(DashboardPage x)
		{
			return x.Items.Contains(this.originalItem);
		}

		// Token: 0x06001783 RID: 6019 RVA: 0x000D4776 File Offset: 0x000D2976
		[CompilerGenerated]
		private bool <ApplyCurrentPageAllTypes_Clicked>b__23_0(DashboardPage x)
		{
			return x.Items.Contains(this.originalItem);
		}

		// Token: 0x06001784 RID: 6020 RVA: 0x000D4776 File Offset: 0x000D2976
		[CompilerGenerated]
		private bool <ApplyColorCurrentPageAllTypes_Clicked>b__24_0(DashboardPage x)
		{
			return x.Items.Contains(this.originalItem);
		}

		// Token: 0x06001785 RID: 6021 RVA: 0x000D4776 File Offset: 0x000D2976
		[CompilerGenerated]
		private bool <ApplyAllPages_Clicked>b__25_0(DashboardPage x)
		{
			return x.Items.Contains(this.originalItem);
		}

		// Token: 0x06001786 RID: 6022 RVA: 0x000D4776 File Offset: 0x000D2976
		[CompilerGenerated]
		private bool <ApplyColorAllPagesSameType_Clicked>b__26_0(DashboardPage x)
		{
			return x.Items.Contains(this.originalItem);
		}

		// Token: 0x06001787 RID: 6023 RVA: 0x000D4776 File Offset: 0x000D2976
		[CompilerGenerated]
		private bool <ApplyColorAllPages_Clicked>b__27_0(DashboardPage x)
		{
			return x.Items.Contains(this.originalItem);
		}

		// Token: 0x06001788 RID: 6024 RVA: 0x000D478C File Offset: 0x000D298C
		[CompilerGenerated]
		private void <btnSelectMultiplePIDs_Clicked>b__38_0(List<IPID> newPids)
		{
			if (newPids == null || newPids.Count == 0)
			{
				return;
			}
			this._item.PID_IDs.Clear();
			this._item.PID_IDs.AddRange(newPids.Select((IPID x) => x.Id).ToArray<int>());
			MultiplePIDViewModel multiplePIDViewModel = this._item.Model as MultiplePIDViewModel;
			if (multiplePIDViewModel != null)
			{
				multiplePIDViewModel.SetPIDs(this._item.PID_IDs);
				this._item.OverrideName = true;
				this._item.CustomName = newPids[0].ShortName;
			}
		}

		// Token: 0x06001789 RID: 6025 RVA: 0x000D4838 File Offset: 0x000D2A38
		private void __InitComponentRuntime()
		{
			Extensions.LoadFromXaml<DashboardItemEditor>(this, typeof(DashboardItemEditor));
			this.gridButtons = NameScopeExtensions.FindByName<Grid>(this, "gridButtons");
			this.btnBack = NameScopeExtensions.FindByName<LinkButton>(this, "btnBack");
			this.LayoutRoot = NameScopeExtensions.FindByName<Grid>(this, "LayoutRoot");
			this.PreviewGrid = NameScopeExtensions.FindByName<Grid>(this, "PreviewGrid");
			this.activityFrame = NameScopeExtensions.FindByName<ActivityFrame>(this, "activityFrame");
			this.btnChangeSensor = NameScopeExtensions.FindByName<Button>(this, "btnChangeSensor");
			this.panelCommon = NameScopeExtensions.FindByName<SfExpander>(this, "panelCommon");
			this.btnRotateGradient = NameScopeExtensions.FindByName<Button>(this, "btnRotateGradient");
			this.panelTextItem = NameScopeExtensions.FindByName<SfExpander>(this, "panelTextItem");
			this.panelGauge = NameScopeExtensions.FindByName<SfExpander>(this, "panelGauge");
			this.panelGaugeVar2 = NameScopeExtensions.FindByName<SfExpander>(this, "panelGaugeVar2");
			this.panelGaugeVar3 = NameScopeExtensions.FindByName<SfExpander>(this, "panelGaugeVar3");
			this.panelChart = NameScopeExtensions.FindByName<SfExpander>(this, "panelChart");
			this.chartStylePicker = NameScopeExtensions.FindByName<Picker>(this, "chartStylePicker");
			this.panelLinearGauge = NameScopeExtensions.FindByName<SfExpander>(this, "panelLinearGauge");
			this.panelMinMaxAvg = NameScopeExtensions.FindByName<SfExpander>(this, "panelMinMaxAvg");
			this.panelWarningAndSound = NameScopeExtensions.FindByName<SfExpander>(this, "panelWarningAndSound");
			this.panelApplyTo = NameScopeExtensions.FindByName<SfExpander>(this, "panelApplyTo");
		}

		// Token: 0x0600178A RID: 6026 RVA: 0x000D4988 File Offset: 0x000D2B88
		[CompilerGenerated]
		private static ValueTuple<DashboardItemTypes, bool> <InitializeComponent>typedBindingsM__2234(DashboardItem A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<DashboardItemTypes, bool>(A_0.ItemType, true);
			}
			return default(ValueTuple<DashboardItemTypes, bool>);
		}

		// Token: 0x0600178B RID: 6027 RVA: 0x000D49B8 File Offset: 0x000D2BB8
		[CompilerGenerated]
		private static void <InitializeComponent>typedBindingsM__2235(DashboardItem A_0, DashboardItemTypes A_1)
		{
			if (A_0 != null)
			{
				A_0.ItemType = A_1;
				return;
			}
		}

		// Token: 0x0600178C RID: 6028 RVA: 0x000D49D4 File Offset: 0x000D2BD4
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__2236(DashboardItem A_0)
		{
			return A_0;
		}

		// Token: 0x0600178D RID: 6029 RVA: 0x000D49E4 File Offset: 0x000D2BE4
		[CompilerGenerated]
		private static ValueTuple<int, bool> <InitializeComponent>typedBindingsM__2237(DashboardItem A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<int, bool>(A_0.PID_Id, true);
			}
			return default(ValueTuple<int, bool>);
		}

		// Token: 0x0600178E RID: 6030 RVA: 0x000D4A14 File Offset: 0x000D2C14
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__2238(DashboardItem A_0)
		{
			return A_0;
		}

		// Token: 0x0600178F RID: 6031 RVA: 0x000D4A24 File Offset: 0x000D2C24
		[CompilerGenerated]
		private static ValueTuple<DashboardItemTypes, bool> <InitializeComponent>typedBindingsM__2239(DashboardItem A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<DashboardItemTypes, bool>(A_0.ItemType, true);
			}
			return default(ValueTuple<DashboardItemTypes, bool>);
		}

		// Token: 0x06001790 RID: 6032 RVA: 0x000D4A54 File Offset: 0x000D2C54
		[CompilerGenerated]
		private static void <InitializeComponent>typedBindingsM__2240(DashboardItem A_0, DashboardItemTypes A_1)
		{
			if (A_0 != null)
			{
				A_0.ItemType = A_1;
				return;
			}
		}

		// Token: 0x06001791 RID: 6033 RVA: 0x000D4A70 File Offset: 0x000D2C70
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__2241(DashboardItem A_0)
		{
			return A_0;
		}

		// Token: 0x06001792 RID: 6034 RVA: 0x000D4A80 File Offset: 0x000D2C80
		[CompilerGenerated]
		private static ValueTuple<bool, bool> <InitializeComponent>typedBindingsM__2242(DashboardItem A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<bool, bool>(A_0.OverrideName, true);
			}
			return default(ValueTuple<bool, bool>);
		}

		// Token: 0x06001793 RID: 6035 RVA: 0x000D4AB0 File Offset: 0x000D2CB0
		[CompilerGenerated]
		private static void <InitializeComponent>typedBindingsM__2243(DashboardItem A_0, bool A_1)
		{
			if (A_0 != null)
			{
				A_0.OverrideName = A_1;
				return;
			}
		}

		// Token: 0x06001794 RID: 6036 RVA: 0x000D4ACC File Offset: 0x000D2CCC
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__2244(DashboardItem A_0)
		{
			return A_0;
		}

		// Token: 0x06001795 RID: 6037 RVA: 0x000D4ADC File Offset: 0x000D2CDC
		[CompilerGenerated]
		private static ValueTuple<bool, bool> <InitializeComponent>typedBindingsM__2245(DashboardItem A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<bool, bool>(A_0.OverrideName, true);
			}
			return default(ValueTuple<bool, bool>);
		}

		// Token: 0x06001796 RID: 6038 RVA: 0x000D4B0C File Offset: 0x000D2D0C
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__2246(DashboardItem A_0)
		{
			return A_0;
		}

		// Token: 0x06001797 RID: 6039 RVA: 0x000D4B1C File Offset: 0x000D2D1C
		[CompilerGenerated]
		private static ValueTuple<string, bool> <InitializeComponent>typedBindingsM__2247(DashboardItem A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<string, bool>(A_0.CustomName, true);
			}
			return default(ValueTuple<string, bool>);
		}

		// Token: 0x06001798 RID: 6040 RVA: 0x000D4B4C File Offset: 0x000D2D4C
		[CompilerGenerated]
		private static void <InitializeComponent>typedBindingsM__2248(DashboardItem A_0, string A_1)
		{
			if (A_0 != null)
			{
				A_0.CustomName = A_1;
				return;
			}
		}

		// Token: 0x06001799 RID: 6041 RVA: 0x000D4B68 File Offset: 0x000D2D68
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__2249(DashboardItem A_0)
		{
			return A_0;
		}

		// Token: 0x0600179A RID: 6042 RVA: 0x000D4B78 File Offset: 0x000D2D78
		[CompilerGenerated]
		private static ValueTuple<DashboardItemTypes, bool> <InitializeComponent>typedBindingsM__2250(DashboardItem A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<DashboardItemTypes, bool>(A_0.ItemType, true);
			}
			return default(ValueTuple<DashboardItemTypes, bool>);
		}

		// Token: 0x0600179B RID: 6043 RVA: 0x000D4BA8 File Offset: 0x000D2DA8
		[CompilerGenerated]
		private static void <InitializeComponent>typedBindingsM__2251(DashboardItem A_0, DashboardItemTypes A_1)
		{
			if (A_0 != null)
			{
				A_0.ItemType = A_1;
				return;
			}
		}

		// Token: 0x0600179C RID: 6044 RVA: 0x000D4BC4 File Offset: 0x000D2DC4
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__2252(DashboardItem A_0)
		{
			return A_0;
		}

		// Token: 0x0600179D RID: 6045 RVA: 0x000D4BD4 File Offset: 0x000D2DD4
		[CompilerGenerated]
		private static ValueTuple<bool, bool> <InitializeComponent>typedBindingsM__2253(DashboardItem A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<bool, bool>(A_0.ShowDefaultBackground, true);
			}
			return default(ValueTuple<bool, bool>);
		}

		// Token: 0x0600179E RID: 6046 RVA: 0x000D4C04 File Offset: 0x000D2E04
		[CompilerGenerated]
		private static void <InitializeComponent>typedBindingsM__2254(DashboardItem A_0, bool A_1)
		{
			if (A_0 != null)
			{
				A_0.ShowDefaultBackground = A_1;
				return;
			}
		}

		// Token: 0x0600179F RID: 6047 RVA: 0x000D4C20 File Offset: 0x000D2E20
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__2255(DashboardItem A_0)
		{
			return A_0;
		}

		// Token: 0x060017A0 RID: 6048 RVA: 0x000D4C30 File Offset: 0x000D2E30
		[CompilerGenerated]
		private static ValueTuple<bool, bool> <InitializeComponent>typedBindingsM__2256(DashboardItem A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<bool, bool>(A_0.ShowDefaultBackground, true);
			}
			return default(ValueTuple<bool, bool>);
		}

		// Token: 0x060017A1 RID: 6049 RVA: 0x000D4C60 File Offset: 0x000D2E60
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__2257(DashboardItem A_0)
		{
			return A_0;
		}

		// Token: 0x060017A2 RID: 6050 RVA: 0x000D4C70 File Offset: 0x000D2E70
		[CompilerGenerated]
		private static ValueTuple<Color, bool> <InitializeComponent>typedBindingsM__2258(DashboardItem A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<Color, bool>(A_0.BackgroundColor, true);
			}
			return default(ValueTuple<Color, bool>);
		}

		// Token: 0x060017A3 RID: 6051 RVA: 0x000D4CA0 File Offset: 0x000D2EA0
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__2259(DashboardItem A_0)
		{
			return A_0;
		}

		// Token: 0x060017A4 RID: 6052 RVA: 0x000D4CB0 File Offset: 0x000D2EB0
		[CompilerGenerated]
		private static ValueTuple<bool, bool> <InitializeComponent>typedBindingsM__2260(DashboardItem A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<bool, bool>(A_0.ShowDefaultBackground, true);
			}
			return default(ValueTuple<bool, bool>);
		}

		// Token: 0x060017A5 RID: 6053 RVA: 0x000D4CE0 File Offset: 0x000D2EE0
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__2261(DashboardItem A_0)
		{
			return A_0;
		}

		// Token: 0x060017A6 RID: 6054 RVA: 0x000D4CF0 File Offset: 0x000D2EF0
		[CompilerGenerated]
		private static ValueTuple<Color, bool> <InitializeComponent>typedBindingsM__2262(DashboardItem A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<Color, bool>(A_0.GradientColor1, true);
			}
			return default(ValueTuple<Color, bool>);
		}

		// Token: 0x060017A7 RID: 6055 RVA: 0x000D4D20 File Offset: 0x000D2F20
		[CompilerGenerated]
		private static void <InitializeComponent>typedBindingsM__2263(DashboardItem A_0, Color A_1)
		{
			if (A_0 != null)
			{
				A_0.GradientColor1 = A_1;
				return;
			}
		}

		// Token: 0x060017A8 RID: 6056 RVA: 0x000D4D3C File Offset: 0x000D2F3C
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__2264(DashboardItem A_0)
		{
			return A_0;
		}

		// Token: 0x060017A9 RID: 6057 RVA: 0x000D4D4C File Offset: 0x000D2F4C
		[CompilerGenerated]
		private static ValueTuple<Color, bool> <InitializeComponent>typedBindingsM__2265(DashboardItem A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<Color, bool>(A_0.GradientColor2, true);
			}
			return default(ValueTuple<Color, bool>);
		}

		// Token: 0x060017AA RID: 6058 RVA: 0x000D4D7C File Offset: 0x000D2F7C
		[CompilerGenerated]
		private static void <InitializeComponent>typedBindingsM__2266(DashboardItem A_0, Color A_1)
		{
			if (A_0 != null)
			{
				A_0.GradientColor2 = A_1;
				return;
			}
		}

		// Token: 0x060017AB RID: 6059 RVA: 0x000D4D98 File Offset: 0x000D2F98
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__2267(DashboardItem A_0)
		{
			return A_0;
		}

		// Token: 0x060017AC RID: 6060 RVA: 0x000D4DA8 File Offset: 0x000D2FA8
		[CompilerGenerated]
		private static ValueTuple<double, bool> <InitializeComponent>typedBindingsM__2268(DashboardItem A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<double, bool>(A_0.GradientOffsetPoint1, true);
			}
			return default(ValueTuple<double, bool>);
		}

		// Token: 0x060017AD RID: 6061 RVA: 0x000D4DD8 File Offset: 0x000D2FD8
		[CompilerGenerated]
		private static void <InitializeComponent>typedBindingsM__2269(DashboardItem A_0, double A_1)
		{
			if (A_0 != null)
			{
				A_0.GradientOffsetPoint1 = A_1;
				return;
			}
		}

		// Token: 0x060017AE RID: 6062 RVA: 0x000D4DF4 File Offset: 0x000D2FF4
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__2270(DashboardItem A_0)
		{
			return A_0;
		}

		// Token: 0x060017AF RID: 6063 RVA: 0x000D4E04 File Offset: 0x000D3004
		[CompilerGenerated]
		private static ValueTuple<double, bool> <InitializeComponent>typedBindingsM__2271(DashboardItem A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<double, bool>(A_0.GradientOffsetPoint2, true);
			}
			return default(ValueTuple<double, bool>);
		}

		// Token: 0x060017B0 RID: 6064 RVA: 0x000D4E34 File Offset: 0x000D3034
		[CompilerGenerated]
		private static void <InitializeComponent>typedBindingsM__2272(DashboardItem A_0, double A_1)
		{
			if (A_0 != null)
			{
				A_0.GradientOffsetPoint2 = A_1;
				return;
			}
		}

		// Token: 0x060017B1 RID: 6065 RVA: 0x000D4E50 File Offset: 0x000D3050
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__2273(DashboardItem A_0)
		{
			return A_0;
		}

		// Token: 0x060017B2 RID: 6066 RVA: 0x000D4E60 File Offset: 0x000D3060
		[CompilerGenerated]
		private static ValueTuple<Color, bool> <InitializeComponent>typedBindingsM__2274(DashboardItem A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<Color, bool>(A_0.FrameColor, true);
			}
			return default(ValueTuple<Color, bool>);
		}

		// Token: 0x060017B3 RID: 6067 RVA: 0x000D4E90 File Offset: 0x000D3090
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__2275(DashboardItem A_0)
		{
			return A_0;
		}

		// Token: 0x060017B4 RID: 6068 RVA: 0x000D4EA0 File Offset: 0x000D30A0
		[CompilerGenerated]
		private static ValueTuple<double, bool> <InitializeComponent>typedBindingsM__2276(DashboardItem A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<double, bool>(A_0.FrameSize, true);
			}
			return default(ValueTuple<double, bool>);
		}

		// Token: 0x060017B5 RID: 6069 RVA: 0x000D4ED0 File Offset: 0x000D30D0
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__2277(DashboardItem A_0)
		{
			return A_0;
		}

		// Token: 0x060017B6 RID: 6070 RVA: 0x000D4EE0 File Offset: 0x000D30E0
		[CompilerGenerated]
		private static ValueTuple<double, bool> <InitializeComponent>typedBindingsM__2278(DashboardItem A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<double, bool>(A_0.FrameSize, true);
			}
			return default(ValueTuple<double, bool>);
		}

		// Token: 0x060017B7 RID: 6071 RVA: 0x000D4F10 File Offset: 0x000D3110
		[CompilerGenerated]
		private static void <InitializeComponent>typedBindingsM__2279(DashboardItem A_0, double A_1)
		{
			if (A_0 != null)
			{
				A_0.FrameSize = A_1;
				return;
			}
		}

		// Token: 0x060017B8 RID: 6072 RVA: 0x000D4F2C File Offset: 0x000D312C
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__2280(DashboardItem A_0)
		{
			return A_0;
		}

		// Token: 0x060017B9 RID: 6073 RVA: 0x000D4F3C File Offset: 0x000D313C
		[CompilerGenerated]
		private static ValueTuple<Thickness, bool> <InitializeComponent>typedBindingsM__2281(DashboardItem A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<Thickness, bool>(A_0.CornerRadius, true);
			}
			return default(ValueTuple<Thickness, bool>);
		}

		// Token: 0x060017BA RID: 6074 RVA: 0x000D4F6C File Offset: 0x000D316C
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__2282(DashboardItem A_0)
		{
			return A_0;
		}

		// Token: 0x060017BB RID: 6075 RVA: 0x000D4F7C File Offset: 0x000D317C
		[CompilerGenerated]
		private static ValueTuple<Thickness, bool> <InitializeComponent>typedBindingsM__2283(DashboardItem A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<Thickness, bool>(A_0.CornerRadius, true);
			}
			return default(ValueTuple<Thickness, bool>);
		}

		// Token: 0x060017BC RID: 6076 RVA: 0x000D4FAC File Offset: 0x000D31AC
		[CompilerGenerated]
		private static void <InitializeComponent>typedBindingsM__2284(DashboardItem A_0, Thickness A_1)
		{
			if (A_0 != null)
			{
				A_0.CornerRadius = A_1;
				return;
			}
		}

		// Token: 0x060017BD RID: 6077 RVA: 0x000D4FC8 File Offset: 0x000D31C8
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__2285(DashboardItem A_0)
		{
			return A_0;
		}

		// Token: 0x060017BE RID: 6078 RVA: 0x000D4FD8 File Offset: 0x000D31D8
		[CompilerGenerated]
		private static ValueTuple<Color, bool> <InitializeComponent>typedBindingsM__2286(DashboardItem A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<Color, bool>(A_0.TitleTextColor, true);
			}
			return default(ValueTuple<Color, bool>);
		}

		// Token: 0x060017BF RID: 6079 RVA: 0x000D5008 File Offset: 0x000D3208
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__2287(DashboardItem A_0)
		{
			return A_0;
		}

		// Token: 0x060017C0 RID: 6080 RVA: 0x000D5018 File Offset: 0x000D3218
		[CompilerGenerated]
		private static ValueTuple<double, bool> <InitializeComponent>typedBindingsM__2288(DashboardItem A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<double, bool>(A_0.TitleFontSize, true);
			}
			return default(ValueTuple<double, bool>);
		}

		// Token: 0x060017C1 RID: 6081 RVA: 0x000D5048 File Offset: 0x000D3248
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__2289(DashboardItem A_0)
		{
			return A_0;
		}

		// Token: 0x060017C2 RID: 6082 RVA: 0x000D5058 File Offset: 0x000D3258
		[CompilerGenerated]
		private static ValueTuple<double, bool> <InitializeComponent>typedBindingsM__2290(DashboardItem A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<double, bool>(A_0.TitleFontSize, true);
			}
			return default(ValueTuple<double, bool>);
		}

		// Token: 0x060017C3 RID: 6083 RVA: 0x000D5088 File Offset: 0x000D3288
		[CompilerGenerated]
		private static void <InitializeComponent>typedBindingsM__2291(DashboardItem A_0, double A_1)
		{
			if (A_0 != null)
			{
				A_0.TitleFontSize = A_1;
				return;
			}
		}

		// Token: 0x060017C4 RID: 6084 RVA: 0x000D50A4 File Offset: 0x000D32A4
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__2292(DashboardItem A_0)
		{
			return A_0;
		}

		// Token: 0x060017C5 RID: 6085 RVA: 0x000D50B4 File Offset: 0x000D32B4
		[CompilerGenerated]
		private static ValueTuple<Color, bool> <InitializeComponent>typedBindingsM__2293(DashboardItem A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<Color, bool>(A_0.ValueNormalTextColor, true);
			}
			return default(ValueTuple<Color, bool>);
		}

		// Token: 0x060017C6 RID: 6086 RVA: 0x000D50E4 File Offset: 0x000D32E4
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__2294(DashboardItem A_0)
		{
			return A_0;
		}

		// Token: 0x060017C7 RID: 6087 RVA: 0x000D50F4 File Offset: 0x000D32F4
		[CompilerGenerated]
		private static ValueTuple<double, bool> <InitializeComponent>typedBindingsM__2295(DashboardItem A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<double, bool>(A_0.ValueFontSize, true);
			}
			return default(ValueTuple<double, bool>);
		}

		// Token: 0x060017C8 RID: 6088 RVA: 0x000D5124 File Offset: 0x000D3324
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__2296(DashboardItem A_0)
		{
			return A_0;
		}

		// Token: 0x060017C9 RID: 6089 RVA: 0x000D5134 File Offset: 0x000D3334
		[CompilerGenerated]
		private static ValueTuple<double, bool> <InitializeComponent>typedBindingsM__2297(DashboardItem A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<double, bool>(A_0.ValueFontSize, true);
			}
			return default(ValueTuple<double, bool>);
		}

		// Token: 0x060017CA RID: 6090 RVA: 0x000D5164 File Offset: 0x000D3364
		[CompilerGenerated]
		private static void <InitializeComponent>typedBindingsM__2298(DashboardItem A_0, double A_1)
		{
			if (A_0 != null)
			{
				A_0.ValueFontSize = A_1;
				return;
			}
		}

		// Token: 0x060017CB RID: 6091 RVA: 0x000D5180 File Offset: 0x000D3380
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__2299(DashboardItem A_0)
		{
			return A_0;
		}

		// Token: 0x060017CC RID: 6092 RVA: 0x000D5190 File Offset: 0x000D3390
		[CompilerGenerated]
		private static ValueTuple<bool, bool> <InitializeComponent>typedBindingsM__2300(DashboardItem A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<bool, bool>(A_0.ValueUseLCDFont, true);
			}
			return default(ValueTuple<bool, bool>);
		}

		// Token: 0x060017CD RID: 6093 RVA: 0x000D51C0 File Offset: 0x000D33C0
		[CompilerGenerated]
		private static void <InitializeComponent>typedBindingsM__2301(DashboardItem A_0, bool A_1)
		{
			if (A_0 != null)
			{
				A_0.ValueUseLCDFont = A_1;
				return;
			}
		}

		// Token: 0x060017CE RID: 6094 RVA: 0x000D51DC File Offset: 0x000D33DC
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__2302(DashboardItem A_0)
		{
			return A_0;
		}

		// Token: 0x060017CF RID: 6095 RVA: 0x000D51EC File Offset: 0x000D33EC
		[CompilerGenerated]
		private static ValueTuple<int, bool> <InitializeComponent>typedBindingsM__2303(DashboardItem A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<int, bool>(A_0.ValueFormat, true);
			}
			return default(ValueTuple<int, bool>);
		}

		// Token: 0x060017D0 RID: 6096 RVA: 0x000D521C File Offset: 0x000D341C
		[CompilerGenerated]
		private static void <InitializeComponent>typedBindingsM__2304(DashboardItem A_0, int A_1)
		{
			if (A_0 != null)
			{
				A_0.ValueFormat = A_1;
				return;
			}
		}

		// Token: 0x060017D1 RID: 6097 RVA: 0x000D5238 File Offset: 0x000D3438
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__2305(DashboardItem A_0)
		{
			return A_0;
		}

		// Token: 0x060017D2 RID: 6098 RVA: 0x000D5248 File Offset: 0x000D3448
		[CompilerGenerated]
		private static ValueTuple<Color, bool> <InitializeComponent>typedBindingsM__2306(DashboardItem A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<Color, bool>(A_0.UnitsTextColor, true);
			}
			return default(ValueTuple<Color, bool>);
		}

		// Token: 0x060017D3 RID: 6099 RVA: 0x000D5278 File Offset: 0x000D3478
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__2307(DashboardItem A_0)
		{
			return A_0;
		}

		// Token: 0x060017D4 RID: 6100 RVA: 0x000D5288 File Offset: 0x000D3488
		[CompilerGenerated]
		private static ValueTuple<double, bool> <InitializeComponent>typedBindingsM__2308(DashboardItem A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<double, bool>(A_0.UnitsFontSize, true);
			}
			return default(ValueTuple<double, bool>);
		}

		// Token: 0x060017D5 RID: 6101 RVA: 0x000D52B8 File Offset: 0x000D34B8
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__2309(DashboardItem A_0)
		{
			return A_0;
		}

		// Token: 0x060017D6 RID: 6102 RVA: 0x000D52C8 File Offset: 0x000D34C8
		[CompilerGenerated]
		private static ValueTuple<double, bool> <InitializeComponent>typedBindingsM__2310(DashboardItem A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<double, bool>(A_0.UnitsFontSize, true);
			}
			return default(ValueTuple<double, bool>);
		}

		// Token: 0x060017D7 RID: 6103 RVA: 0x000D52F8 File Offset: 0x000D34F8
		[CompilerGenerated]
		private static void <InitializeComponent>typedBindingsM__2311(DashboardItem A_0, double A_1)
		{
			if (A_0 != null)
			{
				A_0.UnitsFontSize = A_1;
				return;
			}
		}

		// Token: 0x060017D8 RID: 6104 RVA: 0x000D5314 File Offset: 0x000D3514
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__2312(DashboardItem A_0)
		{
			return A_0;
		}

		// Token: 0x060017D9 RID: 6105 RVA: 0x000D5324 File Offset: 0x000D3524
		[CompilerGenerated]
		private static ValueTuple<double, bool> <InitializeComponent>typedBindingsM__2313(DashboardItem A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<double, bool>(A_0.Minimum, true);
			}
			return default(ValueTuple<double, bool>);
		}

		// Token: 0x060017DA RID: 6106 RVA: 0x000D5354 File Offset: 0x000D3554
		[CompilerGenerated]
		private static void <InitializeComponent>typedBindingsM__2314(DashboardItem A_0, double A_1)
		{
			if (A_0 != null)
			{
				A_0.Minimum = A_1;
				return;
			}
		}

		// Token: 0x060017DB RID: 6107 RVA: 0x000D5370 File Offset: 0x000D3570
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__2315(DashboardItem A_0)
		{
			return A_0;
		}

		// Token: 0x060017DC RID: 6108 RVA: 0x000D5380 File Offset: 0x000D3580
		[CompilerGenerated]
		private static ValueTuple<double, bool> <InitializeComponent>typedBindingsM__2316(DashboardItem A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<double, bool>(A_0.Maximum, true);
			}
			return default(ValueTuple<double, bool>);
		}

		// Token: 0x060017DD RID: 6109 RVA: 0x000D53B0 File Offset: 0x000D35B0
		[CompilerGenerated]
		private static void <InitializeComponent>typedBindingsM__2317(DashboardItem A_0, double A_1)
		{
			if (A_0 != null)
			{
				A_0.Maximum = A_1;
				return;
			}
		}

		// Token: 0x060017DE RID: 6110 RVA: 0x000D53CC File Offset: 0x000D35CC
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__2318(DashboardItem A_0)
		{
			return A_0;
		}

		// Token: 0x060017DF RID: 6111 RVA: 0x000D53DC File Offset: 0x000D35DC
		[CompilerGenerated]
		private static ValueTuple<bool, bool> <InitializeComponent>typedBindingsM__2319(DashboardItem A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<bool, bool>(A_0.GaugeShowBlueLine, true);
			}
			return default(ValueTuple<bool, bool>);
		}

		// Token: 0x060017E0 RID: 6112 RVA: 0x000D540C File Offset: 0x000D360C
		[CompilerGenerated]
		private static void <InitializeComponent>typedBindingsM__2320(DashboardItem A_0, bool A_1)
		{
			if (A_0 != null)
			{
				A_0.GaugeShowBlueLine = A_1;
				return;
			}
		}

		// Token: 0x060017E1 RID: 6113 RVA: 0x000D5428 File Offset: 0x000D3628
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__2321(DashboardItem A_0)
		{
			return A_0;
		}

		// Token: 0x060017E2 RID: 6114 RVA: 0x000D5438 File Offset: 0x000D3638
		[CompilerGenerated]
		private static ValueTuple<bool, bool> <InitializeComponent>typedBindingsM__2322(DashboardItem A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<bool, bool>(A_0.GaugeShowBlueLine, true);
			}
			return default(ValueTuple<bool, bool>);
		}

		// Token: 0x060017E3 RID: 6115 RVA: 0x000D5468 File Offset: 0x000D3668
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__2323(DashboardItem A_0)
		{
			return A_0;
		}

		// Token: 0x060017E4 RID: 6116 RVA: 0x000D5478 File Offset: 0x000D3678
		[CompilerGenerated]
		private static ValueTuple<double, bool> <InitializeComponent>typedBindingsM__2324(DashboardItem A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<double, bool>(A_0.GaugeBlueLineStart, true);
			}
			return default(ValueTuple<double, bool>);
		}

		// Token: 0x060017E5 RID: 6117 RVA: 0x000D54A8 File Offset: 0x000D36A8
		[CompilerGenerated]
		private static void <InitializeComponent>typedBindingsM__2325(DashboardItem A_0, double A_1)
		{
			if (A_0 != null)
			{
				A_0.GaugeBlueLineStart = A_1;
				return;
			}
		}

		// Token: 0x060017E6 RID: 6118 RVA: 0x000D54C4 File Offset: 0x000D36C4
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__2326(DashboardItem A_0)
		{
			return A_0;
		}

		// Token: 0x060017E7 RID: 6119 RVA: 0x000D54D4 File Offset: 0x000D36D4
		[CompilerGenerated]
		private static ValueTuple<double, bool> <InitializeComponent>typedBindingsM__2327(DashboardItem A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<double, bool>(A_0.GaugeBlueLineFinish, true);
			}
			return default(ValueTuple<double, bool>);
		}

		// Token: 0x060017E8 RID: 6120 RVA: 0x000D5504 File Offset: 0x000D3704
		[CompilerGenerated]
		private static void <InitializeComponent>typedBindingsM__2328(DashboardItem A_0, double A_1)
		{
			if (A_0 != null)
			{
				A_0.GaugeBlueLineFinish = A_1;
				return;
			}
		}

		// Token: 0x060017E9 RID: 6121 RVA: 0x000D5520 File Offset: 0x000D3720
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__2329(DashboardItem A_0)
		{
			return A_0;
		}

		// Token: 0x060017EA RID: 6122 RVA: 0x000D5530 File Offset: 0x000D3730
		[CompilerGenerated]
		private static ValueTuple<Color, bool> <InitializeComponent>typedBindingsM__2330(DashboardItem A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<Color, bool>(A_0.GaugeBlueLineColor, true);
			}
			return default(ValueTuple<Color, bool>);
		}

		// Token: 0x060017EB RID: 6123 RVA: 0x000D5560 File Offset: 0x000D3760
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__2331(DashboardItem A_0)
		{
			return A_0;
		}

		// Token: 0x060017EC RID: 6124 RVA: 0x000D5570 File Offset: 0x000D3770
		[CompilerGenerated]
		private static ValueTuple<bool, bool> <InitializeComponent>typedBindingsM__2332(DashboardItem A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<bool, bool>(A_0.GaugeShowRedLine, true);
			}
			return default(ValueTuple<bool, bool>);
		}

		// Token: 0x060017ED RID: 6125 RVA: 0x000D55A0 File Offset: 0x000D37A0
		[CompilerGenerated]
		private static void <InitializeComponent>typedBindingsM__2333(DashboardItem A_0, bool A_1)
		{
			if (A_0 != null)
			{
				A_0.GaugeShowRedLine = A_1;
				return;
			}
		}

		// Token: 0x060017EE RID: 6126 RVA: 0x000D55BC File Offset: 0x000D37BC
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__2334(DashboardItem A_0)
		{
			return A_0;
		}

		// Token: 0x060017EF RID: 6127 RVA: 0x000D55CC File Offset: 0x000D37CC
		[CompilerGenerated]
		private static ValueTuple<bool, bool> <InitializeComponent>typedBindingsM__2335(DashboardItem A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<bool, bool>(A_0.GaugeShowRedLine, true);
			}
			return default(ValueTuple<bool, bool>);
		}

		// Token: 0x060017F0 RID: 6128 RVA: 0x000D55FC File Offset: 0x000D37FC
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__2336(DashboardItem A_0)
		{
			return A_0;
		}

		// Token: 0x060017F1 RID: 6129 RVA: 0x000D560C File Offset: 0x000D380C
		[CompilerGenerated]
		private static ValueTuple<double, bool> <InitializeComponent>typedBindingsM__2337(DashboardItem A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<double, bool>(A_0.GaugeRedLineStart, true);
			}
			return default(ValueTuple<double, bool>);
		}

		// Token: 0x060017F2 RID: 6130 RVA: 0x000D563C File Offset: 0x000D383C
		[CompilerGenerated]
		private static void <InitializeComponent>typedBindingsM__2338(DashboardItem A_0, double A_1)
		{
			if (A_0 != null)
			{
				A_0.GaugeRedLineStart = A_1;
				return;
			}
		}

		// Token: 0x060017F3 RID: 6131 RVA: 0x000D5658 File Offset: 0x000D3858
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__2339(DashboardItem A_0)
		{
			return A_0;
		}

		// Token: 0x060017F4 RID: 6132 RVA: 0x000D5668 File Offset: 0x000D3868
		[CompilerGenerated]
		private static ValueTuple<double, bool> <InitializeComponent>typedBindingsM__2340(DashboardItem A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<double, bool>(A_0.GaugeRedLineFinish, true);
			}
			return default(ValueTuple<double, bool>);
		}

		// Token: 0x060017F5 RID: 6133 RVA: 0x000D5698 File Offset: 0x000D3898
		[CompilerGenerated]
		private static void <InitializeComponent>typedBindingsM__2341(DashboardItem A_0, double A_1)
		{
			if (A_0 != null)
			{
				A_0.GaugeRedLineFinish = A_1;
				return;
			}
		}

		// Token: 0x060017F6 RID: 6134 RVA: 0x000D56B4 File Offset: 0x000D38B4
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__2342(DashboardItem A_0)
		{
			return A_0;
		}

		// Token: 0x060017F7 RID: 6135 RVA: 0x000D56C4 File Offset: 0x000D38C4
		[CompilerGenerated]
		private static ValueTuple<Color, bool> <InitializeComponent>typedBindingsM__2343(DashboardItem A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<Color, bool>(A_0.GaugeRedLineColor, true);
			}
			return default(ValueTuple<Color, bool>);
		}

		// Token: 0x060017F8 RID: 6136 RVA: 0x000D56F4 File Offset: 0x000D38F4
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__2344(DashboardItem A_0)
		{
			return A_0;
		}

		// Token: 0x060017F9 RID: 6137 RVA: 0x000D5704 File Offset: 0x000D3904
		[CompilerGenerated]
		private static ValueTuple<Color, bool> <InitializeComponent>typedBindingsM__2345(DashboardItem A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<Color, bool>(A_0.GaugeLabelColor, true);
			}
			return default(ValueTuple<Color, bool>);
		}

		// Token: 0x060017FA RID: 6138 RVA: 0x000D5734 File Offset: 0x000D3934
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__2346(DashboardItem A_0)
		{
			return A_0;
		}

		// Token: 0x060017FB RID: 6139 RVA: 0x000D5744 File Offset: 0x000D3944
		[CompilerGenerated]
		private static ValueTuple<Color, bool> <InitializeComponent>typedBindingsM__2347(DashboardItem A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<Color, bool>(A_0.GaugeRimColor, true);
			}
			return default(ValueTuple<Color, bool>);
		}

		// Token: 0x060017FC RID: 6140 RVA: 0x000D5774 File Offset: 0x000D3974
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__2348(DashboardItem A_0)
		{
			return A_0;
		}

		// Token: 0x060017FD RID: 6141 RVA: 0x000D5784 File Offset: 0x000D3984
		[CompilerGenerated]
		private static ValueTuple<Color, bool> <InitializeComponent>typedBindingsM__2349(DashboardItem A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<Color, bool>(A_0.GaugeTickColor, true);
			}
			return default(ValueTuple<Color, bool>);
		}

		// Token: 0x060017FE RID: 6142 RVA: 0x000D57B4 File Offset: 0x000D39B4
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__2350(DashboardItem A_0)
		{
			return A_0;
		}

		// Token: 0x060017FF RID: 6143 RVA: 0x000D57C4 File Offset: 0x000D39C4
		[CompilerGenerated]
		private static ValueTuple<Color, bool> <InitializeComponent>typedBindingsM__2351(DashboardItem A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<Color, bool>(A_0.GaugePointerColor, true);
			}
			return default(ValueTuple<Color, bool>);
		}

		// Token: 0x06001800 RID: 6144 RVA: 0x000D57F4 File Offset: 0x000D39F4
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__2352(DashboardItem A_0)
		{
			return A_0;
		}

		// Token: 0x06001801 RID: 6145 RVA: 0x000D5804 File Offset: 0x000D3A04
		[CompilerGenerated]
		private static ValueTuple<Color, bool> <InitializeComponent>typedBindingsM__2353(DashboardItem A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<Color, bool>(A_0.GaugeKnobColor, true);
			}
			return default(ValueTuple<Color, bool>);
		}

		// Token: 0x06001802 RID: 6146 RVA: 0x000D5834 File Offset: 0x000D3A34
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__2354(DashboardItem A_0)
		{
			return A_0;
		}

		// Token: 0x06001803 RID: 6147 RVA: 0x000D5844 File Offset: 0x000D3A44
		[CompilerGenerated]
		private static ValueTuple<double, bool> <InitializeComponent>typedBindingsM__2355(DashboardItem A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<double, bool>(A_0.CiruclarGaugeWidth, true);
			}
			return default(ValueTuple<double, bool>);
		}

		// Token: 0x06001804 RID: 6148 RVA: 0x000D5874 File Offset: 0x000D3A74
		[CompilerGenerated]
		private static void <InitializeComponent>typedBindingsM__2356(DashboardItem A_0, double A_1)
		{
			if (A_0 != null)
			{
				A_0.CiruclarGaugeWidth = A_1;
				return;
			}
		}

		// Token: 0x06001805 RID: 6149 RVA: 0x000D5890 File Offset: 0x000D3A90
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__2357(DashboardItem A_0)
		{
			return A_0;
		}

		// Token: 0x06001806 RID: 6150 RVA: 0x000D58A0 File Offset: 0x000D3AA0
		[CompilerGenerated]
		private static ValueTuple<double, bool> <InitializeComponent>typedBindingsM__2358(DashboardItem A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<double, bool>(A_0.Minimum, true);
			}
			return default(ValueTuple<double, bool>);
		}

		// Token: 0x06001807 RID: 6151 RVA: 0x000D58D0 File Offset: 0x000D3AD0
		[CompilerGenerated]
		private static void <InitializeComponent>typedBindingsM__2359(DashboardItem A_0, double A_1)
		{
			if (A_0 != null)
			{
				A_0.Minimum = A_1;
				return;
			}
		}

		// Token: 0x06001808 RID: 6152 RVA: 0x000D58EC File Offset: 0x000D3AEC
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__2360(DashboardItem A_0)
		{
			return A_0;
		}

		// Token: 0x06001809 RID: 6153 RVA: 0x000D58FC File Offset: 0x000D3AFC
		[CompilerGenerated]
		private static ValueTuple<double, bool> <InitializeComponent>typedBindingsM__2361(DashboardItem A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<double, bool>(A_0.Maximum, true);
			}
			return default(ValueTuple<double, bool>);
		}

		// Token: 0x0600180A RID: 6154 RVA: 0x000D592C File Offset: 0x000D3B2C
		[CompilerGenerated]
		private static void <InitializeComponent>typedBindingsM__2362(DashboardItem A_0, double A_1)
		{
			if (A_0 != null)
			{
				A_0.Maximum = A_1;
				return;
			}
		}

		// Token: 0x0600180B RID: 6155 RVA: 0x000D5948 File Offset: 0x000D3B48
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__2363(DashboardItem A_0)
		{
			return A_0;
		}

		// Token: 0x0600180C RID: 6156 RVA: 0x000D5958 File Offset: 0x000D3B58
		[CompilerGenerated]
		private static ValueTuple<Color, bool> <InitializeComponent>typedBindingsM__2364(DashboardItem A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<Color, bool>(A_0.GaugePointerColor, true);
			}
			return default(ValueTuple<Color, bool>);
		}

		// Token: 0x0600180D RID: 6157 RVA: 0x000D5988 File Offset: 0x000D3B88
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__2365(DashboardItem A_0)
		{
			return A_0;
		}

		// Token: 0x0600180E RID: 6158 RVA: 0x000D5998 File Offset: 0x000D3B98
		[CompilerGenerated]
		private static ValueTuple<Color, bool> <InitializeComponent>typedBindingsM__2366(DashboardItem A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<Color, bool>(A_0.GaugeRimColor, true);
			}
			return default(ValueTuple<Color, bool>);
		}

		// Token: 0x0600180F RID: 6159 RVA: 0x000D59C8 File Offset: 0x000D3BC8
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__2367(DashboardItem A_0)
		{
			return A_0;
		}

		// Token: 0x06001810 RID: 6160 RVA: 0x000D59D8 File Offset: 0x000D3BD8
		[CompilerGenerated]
		private static ValueTuple<bool, bool> <InitializeComponent>typedBindingsM__2368(DashboardItem A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<bool, bool>(A_0.GaugeShowBlueLine, true);
			}
			return default(ValueTuple<bool, bool>);
		}

		// Token: 0x06001811 RID: 6161 RVA: 0x000D5A08 File Offset: 0x000D3C08
		[CompilerGenerated]
		private static void <InitializeComponent>typedBindingsM__2369(DashboardItem A_0, bool A_1)
		{
			if (A_0 != null)
			{
				A_0.GaugeShowBlueLine = A_1;
				return;
			}
		}

		// Token: 0x06001812 RID: 6162 RVA: 0x000D5A24 File Offset: 0x000D3C24
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__2370(DashboardItem A_0)
		{
			return A_0;
		}

		// Token: 0x06001813 RID: 6163 RVA: 0x000D5A34 File Offset: 0x000D3C34
		[CompilerGenerated]
		private static ValueTuple<bool, bool> <InitializeComponent>typedBindingsM__2371(DashboardItem A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<bool, bool>(A_0.GaugeShowBlueLine, true);
			}
			return default(ValueTuple<bool, bool>);
		}

		// Token: 0x06001814 RID: 6164 RVA: 0x000D5A64 File Offset: 0x000D3C64
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__2372(DashboardItem A_0)
		{
			return A_0;
		}

		// Token: 0x06001815 RID: 6165 RVA: 0x000D5A74 File Offset: 0x000D3C74
		[CompilerGenerated]
		private static ValueTuple<double, bool> <InitializeComponent>typedBindingsM__2373(DashboardItem A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<double, bool>(A_0.GaugeBlueLineStart, true);
			}
			return default(ValueTuple<double, bool>);
		}

		// Token: 0x06001816 RID: 6166 RVA: 0x000D5AA4 File Offset: 0x000D3CA4
		[CompilerGenerated]
		private static void <InitializeComponent>typedBindingsM__2374(DashboardItem A_0, double A_1)
		{
			if (A_0 != null)
			{
				A_0.GaugeBlueLineStart = A_1;
				return;
			}
		}

		// Token: 0x06001817 RID: 6167 RVA: 0x000D5AC0 File Offset: 0x000D3CC0
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__2375(DashboardItem A_0)
		{
			return A_0;
		}

		// Token: 0x06001818 RID: 6168 RVA: 0x000D5AD0 File Offset: 0x000D3CD0
		[CompilerGenerated]
		private static ValueTuple<double, bool> <InitializeComponent>typedBindingsM__2376(DashboardItem A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<double, bool>(A_0.GaugeBlueLineFinish, true);
			}
			return default(ValueTuple<double, bool>);
		}

		// Token: 0x06001819 RID: 6169 RVA: 0x000D5B00 File Offset: 0x000D3D00
		[CompilerGenerated]
		private static void <InitializeComponent>typedBindingsM__2377(DashboardItem A_0, double A_1)
		{
			if (A_0 != null)
			{
				A_0.GaugeBlueLineFinish = A_1;
				return;
			}
		}

		// Token: 0x0600181A RID: 6170 RVA: 0x000D5B1C File Offset: 0x000D3D1C
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__2378(DashboardItem A_0)
		{
			return A_0;
		}

		// Token: 0x0600181B RID: 6171 RVA: 0x000D5B2C File Offset: 0x000D3D2C
		[CompilerGenerated]
		private static ValueTuple<Color, bool> <InitializeComponent>typedBindingsM__2379(DashboardItem A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<Color, bool>(A_0.GaugeBlueLineColor, true);
			}
			return default(ValueTuple<Color, bool>);
		}

		// Token: 0x0600181C RID: 6172 RVA: 0x000D5B5C File Offset: 0x000D3D5C
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__2380(DashboardItem A_0)
		{
			return A_0;
		}

		// Token: 0x0600181D RID: 6173 RVA: 0x000D5B6C File Offset: 0x000D3D6C
		[CompilerGenerated]
		private static ValueTuple<bool, bool> <InitializeComponent>typedBindingsM__2381(DashboardItem A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<bool, bool>(A_0.GaugeShowRedLine, true);
			}
			return default(ValueTuple<bool, bool>);
		}

		// Token: 0x0600181E RID: 6174 RVA: 0x000D5B9C File Offset: 0x000D3D9C
		[CompilerGenerated]
		private static void <InitializeComponent>typedBindingsM__2382(DashboardItem A_0, bool A_1)
		{
			if (A_0 != null)
			{
				A_0.GaugeShowRedLine = A_1;
				return;
			}
		}

		// Token: 0x0600181F RID: 6175 RVA: 0x000D5BB8 File Offset: 0x000D3DB8
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__2383(DashboardItem A_0)
		{
			return A_0;
		}

		// Token: 0x06001820 RID: 6176 RVA: 0x000D5BC8 File Offset: 0x000D3DC8
		[CompilerGenerated]
		private static ValueTuple<bool, bool> <InitializeComponent>typedBindingsM__2384(DashboardItem A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<bool, bool>(A_0.GaugeShowRedLine, true);
			}
			return default(ValueTuple<bool, bool>);
		}

		// Token: 0x06001821 RID: 6177 RVA: 0x000D5BF8 File Offset: 0x000D3DF8
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__2385(DashboardItem A_0)
		{
			return A_0;
		}

		// Token: 0x06001822 RID: 6178 RVA: 0x000D5C08 File Offset: 0x000D3E08
		[CompilerGenerated]
		private static ValueTuple<double, bool> <InitializeComponent>typedBindingsM__2386(DashboardItem A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<double, bool>(A_0.GaugeRedLineStart, true);
			}
			return default(ValueTuple<double, bool>);
		}

		// Token: 0x06001823 RID: 6179 RVA: 0x000D5C38 File Offset: 0x000D3E38
		[CompilerGenerated]
		private static void <InitializeComponent>typedBindingsM__2387(DashboardItem A_0, double A_1)
		{
			if (A_0 != null)
			{
				A_0.GaugeRedLineStart = A_1;
				return;
			}
		}

		// Token: 0x06001824 RID: 6180 RVA: 0x000D5C54 File Offset: 0x000D3E54
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__2388(DashboardItem A_0)
		{
			return A_0;
		}

		// Token: 0x06001825 RID: 6181 RVA: 0x000D5C64 File Offset: 0x000D3E64
		[CompilerGenerated]
		private static ValueTuple<double, bool> <InitializeComponent>typedBindingsM__2389(DashboardItem A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<double, bool>(A_0.GaugeRedLineFinish, true);
			}
			return default(ValueTuple<double, bool>);
		}

		// Token: 0x06001826 RID: 6182 RVA: 0x000D5C94 File Offset: 0x000D3E94
		[CompilerGenerated]
		private static void <InitializeComponent>typedBindingsM__2390(DashboardItem A_0, double A_1)
		{
			if (A_0 != null)
			{
				A_0.GaugeRedLineFinish = A_1;
				return;
			}
		}

		// Token: 0x06001827 RID: 6183 RVA: 0x000D5CB0 File Offset: 0x000D3EB0
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__2391(DashboardItem A_0)
		{
			return A_0;
		}

		// Token: 0x06001828 RID: 6184 RVA: 0x000D5CC0 File Offset: 0x000D3EC0
		[CompilerGenerated]
		private static ValueTuple<Color, bool> <InitializeComponent>typedBindingsM__2392(DashboardItem A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<Color, bool>(A_0.GaugeRedLineColor, true);
			}
			return default(ValueTuple<Color, bool>);
		}

		// Token: 0x06001829 RID: 6185 RVA: 0x000D5CF0 File Offset: 0x000D3EF0
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__2393(DashboardItem A_0)
		{
			return A_0;
		}

		// Token: 0x0600182A RID: 6186 RVA: 0x000D5D00 File Offset: 0x000D3F00
		[CompilerGenerated]
		private static ValueTuple<double, bool> <InitializeComponent>typedBindingsM__2394(DashboardItem A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<double, bool>(A_0.CiruclarGaugeWidth, true);
			}
			return default(ValueTuple<double, bool>);
		}

		// Token: 0x0600182B RID: 6187 RVA: 0x000D5D30 File Offset: 0x000D3F30
		[CompilerGenerated]
		private static void <InitializeComponent>typedBindingsM__2395(DashboardItem A_0, double A_1)
		{
			if (A_0 != null)
			{
				A_0.CiruclarGaugeWidth = A_1;
				return;
			}
		}

		// Token: 0x0600182C RID: 6188 RVA: 0x000D5D4C File Offset: 0x000D3F4C
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__2396(DashboardItem A_0)
		{
			return A_0;
		}

		// Token: 0x0600182D RID: 6189 RVA: 0x000D5D5C File Offset: 0x000D3F5C
		[CompilerGenerated]
		private static ValueTuple<double, bool> <InitializeComponent>typedBindingsM__2397(DashboardItem A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<double, bool>(A_0.Minimum, true);
			}
			return default(ValueTuple<double, bool>);
		}

		// Token: 0x0600182E RID: 6190 RVA: 0x000D5D8C File Offset: 0x000D3F8C
		[CompilerGenerated]
		private static void <InitializeComponent>typedBindingsM__2398(DashboardItem A_0, double A_1)
		{
			if (A_0 != null)
			{
				A_0.Minimum = A_1;
				return;
			}
		}

		// Token: 0x0600182F RID: 6191 RVA: 0x000D5DA8 File Offset: 0x000D3FA8
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__2399(DashboardItem A_0)
		{
			return A_0;
		}

		// Token: 0x06001830 RID: 6192 RVA: 0x000D5DB8 File Offset: 0x000D3FB8
		[CompilerGenerated]
		private static ValueTuple<double, bool> <InitializeComponent>typedBindingsM__2400(DashboardItem A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<double, bool>(A_0.Maximum, true);
			}
			return default(ValueTuple<double, bool>);
		}

		// Token: 0x06001831 RID: 6193 RVA: 0x000D5DE8 File Offset: 0x000D3FE8
		[CompilerGenerated]
		private static void <InitializeComponent>typedBindingsM__2401(DashboardItem A_0, double A_1)
		{
			if (A_0 != null)
			{
				A_0.Maximum = A_1;
				return;
			}
		}

		// Token: 0x06001832 RID: 6194 RVA: 0x000D5E04 File Offset: 0x000D4004
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__2402(DashboardItem A_0)
		{
			return A_0;
		}

		// Token: 0x06001833 RID: 6195 RVA: 0x000D5E14 File Offset: 0x000D4014
		[CompilerGenerated]
		private static ValueTuple<Color, bool> <InitializeComponent>typedBindingsM__2403(DashboardItem A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<Color, bool>(A_0.GaugePointerColor, true);
			}
			return default(ValueTuple<Color, bool>);
		}

		// Token: 0x06001834 RID: 6196 RVA: 0x000D5E44 File Offset: 0x000D4044
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__2404(DashboardItem A_0)
		{
			return A_0;
		}

		// Token: 0x06001835 RID: 6197 RVA: 0x000D5E54 File Offset: 0x000D4054
		[CompilerGenerated]
		private static ValueTuple<bool, bool> <InitializeComponent>typedBindingsM__2405(DashboardItem A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<bool, bool>(A_0.ChartValuePositionCenter, true);
			}
			return default(ValueTuple<bool, bool>);
		}

		// Token: 0x06001836 RID: 6198 RVA: 0x000D5E84 File Offset: 0x000D4084
		[CompilerGenerated]
		private static void <InitializeComponent>typedBindingsM__2406(DashboardItem A_0, bool A_1)
		{
			if (A_0 != null)
			{
				A_0.ChartValuePositionCenter = A_1;
				return;
			}
		}

		// Token: 0x06001837 RID: 6199 RVA: 0x000D5EA0 File Offset: 0x000D40A0
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__2407(DashboardItem A_0)
		{
			return A_0;
		}

		// Token: 0x06001838 RID: 6200 RVA: 0x000D5EB0 File Offset: 0x000D40B0
		[CompilerGenerated]
		private static ValueTuple<bool, bool> <InitializeComponent>typedBindingsM__2408(DashboardItem A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<bool, bool>(A_0.ChartValuePositionCenter, true);
			}
			return default(ValueTuple<bool, bool>);
		}

		// Token: 0x06001839 RID: 6201 RVA: 0x000D5EE0 File Offset: 0x000D40E0
		[CompilerGenerated]
		private static void <InitializeComponent>typedBindingsM__2409(DashboardItem A_0, bool A_1)
		{
			if (A_0 != null)
			{
				A_0.ChartValuePositionCenter = A_1;
				return;
			}
		}

		// Token: 0x0600183A RID: 6202 RVA: 0x000D5EFC File Offset: 0x000D40FC
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__2410(DashboardItem A_0)
		{
			return A_0;
		}

		// Token: 0x0600183B RID: 6203 RVA: 0x000D5F0C File Offset: 0x000D410C
		[CompilerGenerated]
		private static ValueTuple<ChartItemTypes, bool> <InitializeComponent>typedBindingsM__2411(DashboardItem A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<ChartItemTypes, bool>(A_0.ChartItemType, true);
			}
			return default(ValueTuple<ChartItemTypes, bool>);
		}

		// Token: 0x0600183C RID: 6204 RVA: 0x000D5F3C File Offset: 0x000D413C
		[CompilerGenerated]
		private static void <InitializeComponent>typedBindingsM__2412(DashboardItem A_0, ChartItemTypes A_1)
		{
			if (A_0 != null)
			{
				A_0.ChartItemType = A_1;
				return;
			}
		}

		// Token: 0x0600183D RID: 6205 RVA: 0x000D5F58 File Offset: 0x000D4158
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__2413(DashboardItem A_0)
		{
			return A_0;
		}

		// Token: 0x0600183E RID: 6206 RVA: 0x000D5F68 File Offset: 0x000D4168
		[CompilerGenerated]
		private static ValueTuple<int, bool> <InitializeComponent>typedBindingsM__2414(DashboardItem A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<int, bool>(A_0.LiveDataShowTime, true);
			}
			return default(ValueTuple<int, bool>);
		}

		// Token: 0x0600183F RID: 6207 RVA: 0x000D5F98 File Offset: 0x000D4198
		[CompilerGenerated]
		private static void <InitializeComponent>typedBindingsM__2415(DashboardItem A_0, int A_1)
		{
			if (A_0 != null)
			{
				A_0.LiveDataShowTime = A_1;
				return;
			}
		}

		// Token: 0x06001840 RID: 6208 RVA: 0x000D5FB4 File Offset: 0x000D41B4
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__2416(DashboardItem A_0)
		{
			return A_0;
		}

		// Token: 0x06001841 RID: 6209 RVA: 0x000D5FC4 File Offset: 0x000D41C4
		[CompilerGenerated]
		private static ValueTuple<int, bool> <InitializeComponent>typedBindingsM__2417(DashboardItem A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<int, bool>(A_0.ChartLineWidth, true);
			}
			return default(ValueTuple<int, bool>);
		}

		// Token: 0x06001842 RID: 6210 RVA: 0x000D5FF4 File Offset: 0x000D41F4
		[CompilerGenerated]
		private static void <InitializeComponent>typedBindingsM__2418(DashboardItem A_0, int A_1)
		{
			if (A_0 != null)
			{
				A_0.ChartLineWidth = A_1;
				return;
			}
		}

		// Token: 0x06001843 RID: 6211 RVA: 0x000D6010 File Offset: 0x000D4210
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__2419(DashboardItem A_0)
		{
			return A_0;
		}

		// Token: 0x06001844 RID: 6212 RVA: 0x000D6020 File Offset: 0x000D4220
		[CompilerGenerated]
		private static ValueTuple<Color, bool> <InitializeComponent>typedBindingsM__2420(DashboardItem A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<Color, bool>(A_0.GaugeLabelColor, true);
			}
			return default(ValueTuple<Color, bool>);
		}

		// Token: 0x06001845 RID: 6213 RVA: 0x000D6050 File Offset: 0x000D4250
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__2421(DashboardItem A_0)
		{
			return A_0;
		}

		// Token: 0x06001846 RID: 6214 RVA: 0x000D6060 File Offset: 0x000D4260
		[CompilerGenerated]
		private static ValueTuple<Color, bool> <InitializeComponent>typedBindingsM__2422(DashboardItem A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<Color, bool>(A_0.ChartLineColor, true);
			}
			return default(ValueTuple<Color, bool>);
		}

		// Token: 0x06001847 RID: 6215 RVA: 0x000D6090 File Offset: 0x000D4290
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__2423(DashboardItem A_0)
		{
			return A_0;
		}

		// Token: 0x06001848 RID: 6216 RVA: 0x000D60A0 File Offset: 0x000D42A0
		[CompilerGenerated]
		private static ValueTuple<bool, bool> <InitializeComponent>typedBindingsM__2424(DashboardItem A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<bool, bool>(A_0.UseCustomMinMax, true);
			}
			return default(ValueTuple<bool, bool>);
		}

		// Token: 0x06001849 RID: 6217 RVA: 0x000D60D0 File Offset: 0x000D42D0
		[CompilerGenerated]
		private static void <InitializeComponent>typedBindingsM__2425(DashboardItem A_0, bool A_1)
		{
			if (A_0 != null)
			{
				A_0.UseCustomMinMax = A_1;
				return;
			}
		}

		// Token: 0x0600184A RID: 6218 RVA: 0x000D60EC File Offset: 0x000D42EC
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__2426(DashboardItem A_0)
		{
			return A_0;
		}

		// Token: 0x0600184B RID: 6219 RVA: 0x000D60FC File Offset: 0x000D42FC
		[CompilerGenerated]
		private static ValueTuple<bool, bool> <InitializeComponent>typedBindingsM__2427(DashboardItem A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<bool, bool>(A_0.UseCustomMinMax, true);
			}
			return default(ValueTuple<bool, bool>);
		}

		// Token: 0x0600184C RID: 6220 RVA: 0x000D612C File Offset: 0x000D432C
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__2428(DashboardItem A_0)
		{
			return A_0;
		}

		// Token: 0x0600184D RID: 6221 RVA: 0x000D613C File Offset: 0x000D433C
		[CompilerGenerated]
		private static ValueTuple<double, bool> <InitializeComponent>typedBindingsM__2429(DashboardItem A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<double, bool>(A_0.Minimum, true);
			}
			return default(ValueTuple<double, bool>);
		}

		// Token: 0x0600184E RID: 6222 RVA: 0x000D616C File Offset: 0x000D436C
		[CompilerGenerated]
		private static void <InitializeComponent>typedBindingsM__2430(DashboardItem A_0, double A_1)
		{
			if (A_0 != null)
			{
				A_0.Minimum = A_1;
				return;
			}
		}

		// Token: 0x0600184F RID: 6223 RVA: 0x000D6188 File Offset: 0x000D4388
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__2431(DashboardItem A_0)
		{
			return A_0;
		}

		// Token: 0x06001850 RID: 6224 RVA: 0x000D6198 File Offset: 0x000D4398
		[CompilerGenerated]
		private static ValueTuple<double, bool> <InitializeComponent>typedBindingsM__2432(DashboardItem A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<double, bool>(A_0.Maximum, true);
			}
			return default(ValueTuple<double, bool>);
		}

		// Token: 0x06001851 RID: 6225 RVA: 0x000D61C8 File Offset: 0x000D43C8
		[CompilerGenerated]
		private static void <InitializeComponent>typedBindingsM__2433(DashboardItem A_0, double A_1)
		{
			if (A_0 != null)
			{
				A_0.Maximum = A_1;
				return;
			}
		}

		// Token: 0x06001852 RID: 6226 RVA: 0x000D61E4 File Offset: 0x000D43E4
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__2434(DashboardItem A_0)
		{
			return A_0;
		}

		// Token: 0x06001853 RID: 6227 RVA: 0x000D61F4 File Offset: 0x000D43F4
		[CompilerGenerated]
		private static ValueTuple<bool, bool> <InitializeComponent>typedBindingsM__2435(DashboardItem A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<bool, bool>(A_0.UseCustomInterval, true);
			}
			return default(ValueTuple<bool, bool>);
		}

		// Token: 0x06001854 RID: 6228 RVA: 0x000D6224 File Offset: 0x000D4424
		[CompilerGenerated]
		private static void <InitializeComponent>typedBindingsM__2436(DashboardItem A_0, bool A_1)
		{
			if (A_0 != null)
			{
				A_0.UseCustomInterval = A_1;
				return;
			}
		}

		// Token: 0x06001855 RID: 6229 RVA: 0x000D6240 File Offset: 0x000D4440
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__2437(DashboardItem A_0)
		{
			return A_0;
		}

		// Token: 0x06001856 RID: 6230 RVA: 0x000D6250 File Offset: 0x000D4450
		[CompilerGenerated]
		private static ValueTuple<bool, bool> <InitializeComponent>typedBindingsM__2438(DashboardItem A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<bool, bool>(A_0.UseCustomInterval, true);
			}
			return default(ValueTuple<bool, bool>);
		}

		// Token: 0x06001857 RID: 6231 RVA: 0x000D6280 File Offset: 0x000D4480
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__2439(DashboardItem A_0)
		{
			return A_0;
		}

		// Token: 0x06001858 RID: 6232 RVA: 0x000D6290 File Offset: 0x000D4490
		[CompilerGenerated]
		private static ValueTuple<double, bool> <InitializeComponent>typedBindingsM__2440(DashboardItem A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<double, bool>(A_0.CustomInterval, true);
			}
			return default(ValueTuple<double, bool>);
		}

		// Token: 0x06001859 RID: 6233 RVA: 0x000D62C0 File Offset: 0x000D44C0
		[CompilerGenerated]
		private static void <InitializeComponent>typedBindingsM__2441(DashboardItem A_0, double A_1)
		{
			if (A_0 != null)
			{
				A_0.CustomInterval = A_1;
				return;
			}
		}

		// Token: 0x0600185A RID: 6234 RVA: 0x000D62DC File Offset: 0x000D44DC
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__2442(DashboardItem A_0)
		{
			return A_0;
		}

		// Token: 0x0600185B RID: 6235 RVA: 0x000D62EC File Offset: 0x000D44EC
		[CompilerGenerated]
		private static ValueTuple<double, bool> <InitializeComponent>typedBindingsM__2443(DashboardItem A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<double, bool>(A_0.Minimum, true);
			}
			return default(ValueTuple<double, bool>);
		}

		// Token: 0x0600185C RID: 6236 RVA: 0x000D631C File Offset: 0x000D451C
		[CompilerGenerated]
		private static void <InitializeComponent>typedBindingsM__2444(DashboardItem A_0, double A_1)
		{
			if (A_0 != null)
			{
				A_0.Minimum = A_1;
				return;
			}
		}

		// Token: 0x0600185D RID: 6237 RVA: 0x000D6338 File Offset: 0x000D4538
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__2445(DashboardItem A_0)
		{
			return A_0;
		}

		// Token: 0x0600185E RID: 6238 RVA: 0x000D6348 File Offset: 0x000D4548
		[CompilerGenerated]
		private static ValueTuple<double, bool> <InitializeComponent>typedBindingsM__2446(DashboardItem A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<double, bool>(A_0.Maximum, true);
			}
			return default(ValueTuple<double, bool>);
		}

		// Token: 0x0600185F RID: 6239 RVA: 0x000D6378 File Offset: 0x000D4578
		[CompilerGenerated]
		private static void <InitializeComponent>typedBindingsM__2447(DashboardItem A_0, double A_1)
		{
			if (A_0 != null)
			{
				A_0.Maximum = A_1;
				return;
			}
		}

		// Token: 0x06001860 RID: 6240 RVA: 0x000D6394 File Offset: 0x000D4594
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__2448(DashboardItem A_0)
		{
			return A_0;
		}

		// Token: 0x06001861 RID: 6241 RVA: 0x000D63A4 File Offset: 0x000D45A4
		[CompilerGenerated]
		private static ValueTuple<double, bool> <InitializeComponent>typedBindingsM__2449(DashboardItem A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<double, bool>(A_0.LinearScaleSize, true);
			}
			return default(ValueTuple<double, bool>);
		}

		// Token: 0x06001862 RID: 6242 RVA: 0x000D63D4 File Offset: 0x000D45D4
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__2450(DashboardItem A_0)
		{
			return A_0;
		}

		// Token: 0x06001863 RID: 6243 RVA: 0x000D63E4 File Offset: 0x000D45E4
		[CompilerGenerated]
		private static ValueTuple<double, bool> <InitializeComponent>typedBindingsM__2451(DashboardItem A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<double, bool>(A_0.LinearScaleSize, true);
			}
			return default(ValueTuple<double, bool>);
		}

		// Token: 0x06001864 RID: 6244 RVA: 0x000D6414 File Offset: 0x000D4614
		[CompilerGenerated]
		private static void <InitializeComponent>typedBindingsM__2452(DashboardItem A_0, double A_1)
		{
			if (A_0 != null)
			{
				A_0.LinearScaleSize = A_1;
				return;
			}
		}

		// Token: 0x06001865 RID: 6245 RVA: 0x000D6430 File Offset: 0x000D4630
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__2453(DashboardItem A_0)
		{
			return A_0;
		}

		// Token: 0x06001866 RID: 6246 RVA: 0x000D6440 File Offset: 0x000D4640
		[CompilerGenerated]
		private static ValueTuple<Color, bool> <InitializeComponent>typedBindingsM__2454(DashboardItem A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<Color, bool>(A_0.GaugeRimColor, true);
			}
			return default(ValueTuple<Color, bool>);
		}

		// Token: 0x06001867 RID: 6247 RVA: 0x000D6470 File Offset: 0x000D4670
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__2455(DashboardItem A_0)
		{
			return A_0;
		}

		// Token: 0x06001868 RID: 6248 RVA: 0x000D6480 File Offset: 0x000D4680
		[CompilerGenerated]
		private static ValueTuple<Color, bool> <InitializeComponent>typedBindingsM__2456(DashboardItem A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<Color, bool>(A_0.GaugeLabelColor, true);
			}
			return default(ValueTuple<Color, bool>);
		}

		// Token: 0x06001869 RID: 6249 RVA: 0x000D64B0 File Offset: 0x000D46B0
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__2457(DashboardItem A_0)
		{
			return A_0;
		}

		// Token: 0x0600186A RID: 6250 RVA: 0x000D64C0 File Offset: 0x000D46C0
		[CompilerGenerated]
		private static ValueTuple<Color, bool> <InitializeComponent>typedBindingsM__2458(DashboardItem A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<Color, bool>(A_0.GaugePointerColor, true);
			}
			return default(ValueTuple<Color, bool>);
		}

		// Token: 0x0600186B RID: 6251 RVA: 0x000D64F0 File Offset: 0x000D46F0
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__2459(DashboardItem A_0)
		{
			return A_0;
		}

		// Token: 0x0600186C RID: 6252 RVA: 0x000D6500 File Offset: 0x000D4700
		[CompilerGenerated]
		private static ValueTuple<bool, bool> <InitializeComponent>typedBindingsM__2460(DashboardItem A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<bool, bool>(A_0.ShowValue, true);
			}
			return default(ValueTuple<bool, bool>);
		}

		// Token: 0x0600186D RID: 6253 RVA: 0x000D6530 File Offset: 0x000D4730
		[CompilerGenerated]
		private static void <InitializeComponent>typedBindingsM__2461(DashboardItem A_0, bool A_1)
		{
			if (A_0 != null)
			{
				A_0.ShowValue = A_1;
				return;
			}
		}

		// Token: 0x0600186E RID: 6254 RVA: 0x000D654C File Offset: 0x000D474C
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__2462(DashboardItem A_0)
		{
			return A_0;
		}

		// Token: 0x0600186F RID: 6255 RVA: 0x000D655C File Offset: 0x000D475C
		[CompilerGenerated]
		private static ValueTuple<bool, bool> <InitializeComponent>typedBindingsM__2463(DashboardItem A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<bool, bool>(A_0.LinearOrientationHorizontal, true);
			}
			return default(ValueTuple<bool, bool>);
		}

		// Token: 0x06001870 RID: 6256 RVA: 0x000D658C File Offset: 0x000D478C
		[CompilerGenerated]
		private static void <InitializeComponent>typedBindingsM__2464(DashboardItem A_0, bool A_1)
		{
			if (A_0 != null)
			{
				A_0.LinearOrientationHorizontal = A_1;
				return;
			}
		}

		// Token: 0x06001871 RID: 6257 RVA: 0x000D65A8 File Offset: 0x000D47A8
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__2465(DashboardItem A_0)
		{
			return A_0;
		}

		// Token: 0x06001872 RID: 6258 RVA: 0x000D65B8 File Offset: 0x000D47B8
		[CompilerGenerated]
		private static ValueTuple<bool, bool> <InitializeComponent>typedBindingsM__2466(DashboardItem A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<bool, bool>(A_0.LinearOrientationHorizontal, true);
			}
			return default(ValueTuple<bool, bool>);
		}

		// Token: 0x06001873 RID: 6259 RVA: 0x000D65E8 File Offset: 0x000D47E8
		[CompilerGenerated]
		private static void <InitializeComponent>typedBindingsM__2467(DashboardItem A_0, bool A_1)
		{
			if (A_0 != null)
			{
				A_0.LinearOrientationHorizontal = A_1;
				return;
			}
		}

		// Token: 0x06001874 RID: 6260 RVA: 0x000D6604 File Offset: 0x000D4804
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__2468(DashboardItem A_0)
		{
			return A_0;
		}

		// Token: 0x06001875 RID: 6261 RVA: 0x000D6614 File Offset: 0x000D4814
		[CompilerGenerated]
		private static ValueTuple<bool, bool> <InitializeComponent>typedBindingsM__2469(DashboardItem A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<bool, bool>(A_0.LinearOrientationHorizontal, true);
			}
			return default(ValueTuple<bool, bool>);
		}

		// Token: 0x06001876 RID: 6262 RVA: 0x000D6644 File Offset: 0x000D4844
		[CompilerGenerated]
		private static void <InitializeComponent>typedBindingsM__2470(DashboardItem A_0, bool A_1)
		{
			if (A_0 != null)
			{
				A_0.LinearOrientationHorizontal = A_1;
				return;
			}
		}

		// Token: 0x06001877 RID: 6263 RVA: 0x000D6660 File Offset: 0x000D4860
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__2471(DashboardItem A_0)
		{
			return A_0;
		}

		// Token: 0x06001878 RID: 6264 RVA: 0x000D6670 File Offset: 0x000D4870
		[CompilerGenerated]
		private static ValueTuple<bool, bool> <InitializeComponent>typedBindingsM__2472(DashboardItem A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<bool, bool>(A_0.ShowMinMax, true);
			}
			return default(ValueTuple<bool, bool>);
		}

		// Token: 0x06001879 RID: 6265 RVA: 0x000D66A0 File Offset: 0x000D48A0
		[CompilerGenerated]
		private static void <InitializeComponent>typedBindingsM__2473(DashboardItem A_0, bool A_1)
		{
			if (A_0 != null)
			{
				A_0.ShowMinMax = A_1;
				return;
			}
		}

		// Token: 0x0600187A RID: 6266 RVA: 0x000D66BC File Offset: 0x000D48BC
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__2474(DashboardItem A_0)
		{
			return A_0;
		}

		// Token: 0x0600187B RID: 6267 RVA: 0x000D66CC File Offset: 0x000D48CC
		[CompilerGenerated]
		private static ValueTuple<bool, bool> <InitializeComponent>typedBindingsM__2475(DashboardItem A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<bool, bool>(A_0.ShowAvg, true);
			}
			return default(ValueTuple<bool, bool>);
		}

		// Token: 0x0600187C RID: 6268 RVA: 0x000D66FC File Offset: 0x000D48FC
		[CompilerGenerated]
		private static void <InitializeComponent>typedBindingsM__2476(DashboardItem A_0, bool A_1)
		{
			if (A_0 != null)
			{
				A_0.ShowAvg = A_1;
				return;
			}
		}

		// Token: 0x0600187D RID: 6269 RVA: 0x000D6718 File Offset: 0x000D4918
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__2477(DashboardItem A_0)
		{
			return A_0;
		}

		// Token: 0x0600187E RID: 6270 RVA: 0x000D6728 File Offset: 0x000D4928
		[CompilerGenerated]
		private static ValueTuple<Color, bool> <InitializeComponent>typedBindingsM__2478(DashboardItem A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<Color, bool>(A_0.MinMaxAvgColor, true);
			}
			return default(ValueTuple<Color, bool>);
		}

		// Token: 0x0600187F RID: 6271 RVA: 0x000D6758 File Offset: 0x000D4958
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__2479(DashboardItem A_0)
		{
			return A_0;
		}

		// Token: 0x06001880 RID: 6272 RVA: 0x000D6768 File Offset: 0x000D4968
		[CompilerGenerated]
		private static ValueTuple<double, bool> <InitializeComponent>typedBindingsM__2480(DashboardItem A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<double, bool>(A_0.MinMaxAvgFontSize, true);
			}
			return default(ValueTuple<double, bool>);
		}

		// Token: 0x06001881 RID: 6273 RVA: 0x000D6798 File Offset: 0x000D4998
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__2481(DashboardItem A_0)
		{
			return A_0;
		}

		// Token: 0x06001882 RID: 6274 RVA: 0x000D67A8 File Offset: 0x000D49A8
		[CompilerGenerated]
		private static ValueTuple<double, bool> <InitializeComponent>typedBindingsM__2482(DashboardItem A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<double, bool>(A_0.MinMaxAvgFontSize, true);
			}
			return default(ValueTuple<double, bool>);
		}

		// Token: 0x06001883 RID: 6275 RVA: 0x000D67D8 File Offset: 0x000D49D8
		[CompilerGenerated]
		private static void <InitializeComponent>typedBindingsM__2483(DashboardItem A_0, double A_1)
		{
			if (A_0 != null)
			{
				A_0.MinMaxAvgFontSize = A_1;
				return;
			}
		}

		// Token: 0x06001884 RID: 6276 RVA: 0x000D67F4 File Offset: 0x000D49F4
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__2484(DashboardItem A_0)
		{
			return A_0;
		}

		// Token: 0x06001885 RID: 6277 RVA: 0x000D6804 File Offset: 0x000D4A04
		[CompilerGenerated]
		private static ValueTuple<bool, bool> <InitializeComponent>typedBindingsM__2485(DashboardItem A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<bool, bool>(A_0.ShowMinMaxPointers, true);
			}
			return default(ValueTuple<bool, bool>);
		}

		// Token: 0x06001886 RID: 6278 RVA: 0x000D6834 File Offset: 0x000D4A34
		[CompilerGenerated]
		private static void <InitializeComponent>typedBindingsM__2486(DashboardItem A_0, bool A_1)
		{
			if (A_0 != null)
			{
				A_0.ShowMinMaxPointers = A_1;
				return;
			}
		}

		// Token: 0x06001887 RID: 6279 RVA: 0x000D6850 File Offset: 0x000D4A50
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__2487(DashboardItem A_0)
		{
			return A_0;
		}

		// Token: 0x06001888 RID: 6280 RVA: 0x000D6860 File Offset: 0x000D4A60
		[CompilerGenerated]
		private static ValueTuple<Color, bool> <InitializeComponent>typedBindingsM__2488(DashboardItem A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<Color, bool>(A_0.MinMaxPointersColor, true);
			}
			return default(ValueTuple<Color, bool>);
		}

		// Token: 0x06001889 RID: 6281 RVA: 0x000D6890 File Offset: 0x000D4A90
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__2489(DashboardItem A_0)
		{
			return A_0;
		}

		// Token: 0x0600188A RID: 6282 RVA: 0x000D68A0 File Offset: 0x000D4AA0
		[CompilerGenerated]
		private static ValueTuple<bool, bool> <InitializeComponent>typedBindingsM__2490(DashboardItem A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<bool, bool>(A_0.SetMinMaxAvgOnlyVisibleArea, true);
			}
			return default(ValueTuple<bool, bool>);
		}

		// Token: 0x0600188B RID: 6283 RVA: 0x000D68D0 File Offset: 0x000D4AD0
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__2491(DashboardItem A_0)
		{
			return A_0;
		}

		// Token: 0x0600188C RID: 6284 RVA: 0x000D68E0 File Offset: 0x000D4AE0
		[CompilerGenerated]
		private static ValueTuple<bool, bool> <InitializeComponent>typedBindingsM__2492(DashboardItem A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<bool, bool>(A_0.SetMinMaxAvgOnlyVisibleArea, true);
			}
			return default(ValueTuple<bool, bool>);
		}

		// Token: 0x0600188D RID: 6285 RVA: 0x000D6910 File Offset: 0x000D4B10
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__2493(DashboardItem A_0)
		{
			return A_0;
		}

		// Token: 0x0600188E RID: 6286 RVA: 0x000D6920 File Offset: 0x000D4B20
		[CompilerGenerated]
		private static ValueTuple<bool, bool> <InitializeComponent>typedBindingsM__2494(DashboardItem A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<bool, bool>(A_0.SetMinMaxAvgOnlyVisibleArea, true);
			}
			return default(ValueTuple<bool, bool>);
		}

		// Token: 0x0600188F RID: 6287 RVA: 0x000D6950 File Offset: 0x000D4B50
		[CompilerGenerated]
		private static void <InitializeComponent>typedBindingsM__2495(DashboardItem A_0, bool A_1)
		{
			if (A_0 != null)
			{
				A_0.SetMinMaxAvgOnlyVisibleArea = A_1;
				return;
			}
		}

		// Token: 0x06001890 RID: 6288 RVA: 0x000D696C File Offset: 0x000D4B6C
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__2496(DashboardItem A_0)
		{
			return A_0;
		}

		// Token: 0x06001891 RID: 6289 RVA: 0x000D697C File Offset: 0x000D4B7C
		[CompilerGenerated]
		private static ValueTuple<bool, bool> <InitializeComponent>typedBindingsM__2497(DashboardItem A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<bool, bool>(A_0.GaugeShowRedLine, true);
			}
			return default(ValueTuple<bool, bool>);
		}

		// Token: 0x06001892 RID: 6290 RVA: 0x000D69AC File Offset: 0x000D4BAC
		[CompilerGenerated]
		private static void <InitializeComponent>typedBindingsM__2498(DashboardItem A_0, bool A_1)
		{
			if (A_0 != null)
			{
				A_0.GaugeShowRedLine = A_1;
				return;
			}
		}

		// Token: 0x06001893 RID: 6291 RVA: 0x000D69C8 File Offset: 0x000D4BC8
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__2499(DashboardItem A_0)
		{
			return A_0;
		}

		// Token: 0x06001894 RID: 6292 RVA: 0x000D69D8 File Offset: 0x000D4BD8
		[CompilerGenerated]
		private static ValueTuple<bool, bool> <InitializeComponent>typedBindingsM__2500(DashboardItem A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<bool, bool>(A_0.GaugeShowRedLine, true);
			}
			return default(ValueTuple<bool, bool>);
		}

		// Token: 0x06001895 RID: 6293 RVA: 0x000D6A08 File Offset: 0x000D4C08
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__2501(DashboardItem A_0)
		{
			return A_0;
		}

		// Token: 0x06001896 RID: 6294 RVA: 0x000D6A18 File Offset: 0x000D4C18
		[CompilerGenerated]
		private static ValueTuple<double, bool> <InitializeComponent>typedBindingsM__2502(DashboardItem A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<double, bool>(A_0.GaugeRedLineStart, true);
			}
			return default(ValueTuple<double, bool>);
		}

		// Token: 0x06001897 RID: 6295 RVA: 0x000D6A48 File Offset: 0x000D4C48
		[CompilerGenerated]
		private static void <InitializeComponent>typedBindingsM__2503(DashboardItem A_0, double A_1)
		{
			if (A_0 != null)
			{
				A_0.GaugeRedLineStart = A_1;
				return;
			}
		}

		// Token: 0x06001898 RID: 6296 RVA: 0x000D6A64 File Offset: 0x000D4C64
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__2504(DashboardItem A_0)
		{
			return A_0;
		}

		// Token: 0x06001899 RID: 6297 RVA: 0x000D6A74 File Offset: 0x000D4C74
		[CompilerGenerated]
		private static ValueTuple<Color, bool> <InitializeComponent>typedBindingsM__2505(DashboardItem A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<Color, bool>(A_0.GaugeRedLineColor, true);
			}
			return default(ValueTuple<Color, bool>);
		}

		// Token: 0x0600189A RID: 6298 RVA: 0x000D6AA4 File Offset: 0x000D4CA4
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__2506(DashboardItem A_0)
		{
			return A_0;
		}

		// Token: 0x0600189B RID: 6299 RVA: 0x000D6AB4 File Offset: 0x000D4CB4
		[CompilerGenerated]
		private static ValueTuple<bool, bool> <InitializeComponent>typedBindingsM__2507(DashboardItem A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<bool, bool>(A_0.PlaySound, true);
			}
			return default(ValueTuple<bool, bool>);
		}

		// Token: 0x0600189C RID: 6300 RVA: 0x000D6AE4 File Offset: 0x000D4CE4
		[CompilerGenerated]
		private static void <InitializeComponent>typedBindingsM__2508(DashboardItem A_0, bool A_1)
		{
			if (A_0 != null)
			{
				A_0.PlaySound = A_1;
				return;
			}
		}

		// Token: 0x0600189D RID: 6301 RVA: 0x000D6B00 File Offset: 0x000D4D00
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__2509(DashboardItem A_0)
		{
			return A_0;
		}

		// Token: 0x0600189E RID: 6302 RVA: 0x000D6B10 File Offset: 0x000D4D10
		[CompilerGenerated]
		private static ValueTuple<bool, bool> <InitializeComponent>typedBindingsM__2510(DashboardItem A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<bool, bool>(A_0.PlaySound, true);
			}
			return default(ValueTuple<bool, bool>);
		}

		// Token: 0x0600189F RID: 6303 RVA: 0x000D6B40 File Offset: 0x000D4D40
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__2511(DashboardItem A_0)
		{
			return A_0;
		}

		// Token: 0x060018A0 RID: 6304 RVA: 0x000D6B50 File Offset: 0x000D4D50
		[CompilerGenerated]
		private static ValueTuple<double, bool> <InitializeComponent>typedBindingsM__2512(DashboardItem A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<double, bool>(A_0.SoundStart, true);
			}
			return default(ValueTuple<double, bool>);
		}

		// Token: 0x060018A1 RID: 6305 RVA: 0x000D6B80 File Offset: 0x000D4D80
		[CompilerGenerated]
		private static void <InitializeComponent>typedBindingsM__2513(DashboardItem A_0, double A_1)
		{
			if (A_0 != null)
			{
				A_0.SoundStart = A_1;
				return;
			}
		}

		// Token: 0x060018A2 RID: 6306 RVA: 0x000D6B9C File Offset: 0x000D4D9C
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__2514(DashboardItem A_0)
		{
			return A_0;
		}

		// Token: 0x060018A3 RID: 6307 RVA: 0x000D6BAC File Offset: 0x000D4DAC
		[CompilerGenerated]
		private static ValueTuple<string, bool> <InitializeComponent>typedBindingsM__2515(DashboardItem A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<string, bool>(A_0.SoundName, true);
			}
			return default(ValueTuple<string, bool>);
		}

		// Token: 0x060018A4 RID: 6308 RVA: 0x000D6BDC File Offset: 0x000D4DDC
		[CompilerGenerated]
		private static void <InitializeComponent>typedBindingsM__2516(DashboardItem A_0, string A_1)
		{
			if (A_0 != null)
			{
				A_0.SoundName = A_1;
				return;
			}
		}

		// Token: 0x060018A5 RID: 6309 RVA: 0x000D6BF8 File Offset: 0x000D4DF8
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__2517(DashboardItem A_0)
		{
			return A_0;
		}

		// Token: 0x060018A6 RID: 6310 RVA: 0x000D6C08 File Offset: 0x000D4E08
		[CompilerGenerated]
		private static ValueTuple<bool, bool> <InitializeComponent>typedBindingsM__2518(DashboardItem A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<bool, bool>(A_0.ShowLowWarning, true);
			}
			return default(ValueTuple<bool, bool>);
		}

		// Token: 0x060018A7 RID: 6311 RVA: 0x000D6C38 File Offset: 0x000D4E38
		[CompilerGenerated]
		private static void <InitializeComponent>typedBindingsM__2519(DashboardItem A_0, bool A_1)
		{
			if (A_0 != null)
			{
				A_0.ShowLowWarning = A_1;
				return;
			}
		}

		// Token: 0x060018A8 RID: 6312 RVA: 0x000D6C54 File Offset: 0x000D4E54
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__2520(DashboardItem A_0)
		{
			return A_0;
		}

		// Token: 0x060018A9 RID: 6313 RVA: 0x000D6C64 File Offset: 0x000D4E64
		[CompilerGenerated]
		private static ValueTuple<bool, bool> <InitializeComponent>typedBindingsM__2521(DashboardItem A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<bool, bool>(A_0.ShowLowWarning, true);
			}
			return default(ValueTuple<bool, bool>);
		}

		// Token: 0x060018AA RID: 6314 RVA: 0x000D6C94 File Offset: 0x000D4E94
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__2522(DashboardItem A_0)
		{
			return A_0;
		}

		// Token: 0x060018AB RID: 6315 RVA: 0x000D6CA4 File Offset: 0x000D4EA4
		[CompilerGenerated]
		private static ValueTuple<double, bool> <InitializeComponent>typedBindingsM__2523(DashboardItem A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<double, bool>(A_0.LowWarningStart, true);
			}
			return default(ValueTuple<double, bool>);
		}

		// Token: 0x060018AC RID: 6316 RVA: 0x000D6CD4 File Offset: 0x000D4ED4
		[CompilerGenerated]
		private static void <InitializeComponent>typedBindingsM__2524(DashboardItem A_0, double A_1)
		{
			if (A_0 != null)
			{
				A_0.LowWarningStart = A_1;
				return;
			}
		}

		// Token: 0x060018AD RID: 6317 RVA: 0x000D6CF0 File Offset: 0x000D4EF0
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__2525(DashboardItem A_0)
		{
			return A_0;
		}

		// Token: 0x060018AE RID: 6318 RVA: 0x000D6D00 File Offset: 0x000D4F00
		[CompilerGenerated]
		private static ValueTuple<Color, bool> <InitializeComponent>typedBindingsM__2526(DashboardItem A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<Color, bool>(A_0.LowWarningColor, true);
			}
			return default(ValueTuple<Color, bool>);
		}

		// Token: 0x060018AF RID: 6319 RVA: 0x000D6D30 File Offset: 0x000D4F30
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__2527(DashboardItem A_0)
		{
			return A_0;
		}

		// Token: 0x060018B0 RID: 6320 RVA: 0x000D6D40 File Offset: 0x000D4F40
		[CompilerGenerated]
		private static ValueTuple<bool, bool> <InitializeComponent>typedBindingsM__2528(DashboardItem A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<bool, bool>(A_0.PlaySoundLow, true);
			}
			return default(ValueTuple<bool, bool>);
		}

		// Token: 0x060018B1 RID: 6321 RVA: 0x000D6D70 File Offset: 0x000D4F70
		[CompilerGenerated]
		private static void <InitializeComponent>typedBindingsM__2529(DashboardItem A_0, bool A_1)
		{
			if (A_0 != null)
			{
				A_0.PlaySoundLow = A_1;
				return;
			}
		}

		// Token: 0x060018B2 RID: 6322 RVA: 0x000D6D8C File Offset: 0x000D4F8C
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__2530(DashboardItem A_0)
		{
			return A_0;
		}

		// Token: 0x060018B3 RID: 6323 RVA: 0x000D6D9C File Offset: 0x000D4F9C
		[CompilerGenerated]
		private static ValueTuple<bool, bool> <InitializeComponent>typedBindingsM__2531(DashboardItem A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<bool, bool>(A_0.PlaySoundLow, true);
			}
			return default(ValueTuple<bool, bool>);
		}

		// Token: 0x060018B4 RID: 6324 RVA: 0x000D6DCC File Offset: 0x000D4FCC
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__2532(DashboardItem A_0)
		{
			return A_0;
		}

		// Token: 0x060018B5 RID: 6325 RVA: 0x000D6DDC File Offset: 0x000D4FDC
		[CompilerGenerated]
		private static ValueTuple<double, bool> <InitializeComponent>typedBindingsM__2533(DashboardItem A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<double, bool>(A_0.SoundStartLow, true);
			}
			return default(ValueTuple<double, bool>);
		}

		// Token: 0x060018B6 RID: 6326 RVA: 0x000D6E0C File Offset: 0x000D500C
		[CompilerGenerated]
		private static void <InitializeComponent>typedBindingsM__2534(DashboardItem A_0, double A_1)
		{
			if (A_0 != null)
			{
				A_0.SoundStartLow = A_1;
				return;
			}
		}

		// Token: 0x060018B7 RID: 6327 RVA: 0x000D6E28 File Offset: 0x000D5028
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__2535(DashboardItem A_0)
		{
			return A_0;
		}

		// Token: 0x060018B8 RID: 6328 RVA: 0x000D6E38 File Offset: 0x000D5038
		[CompilerGenerated]
		private static ValueTuple<string, bool> <InitializeComponent>typedBindingsM__2536(DashboardItem A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<string, bool>(A_0.SoundNameLow, true);
			}
			return default(ValueTuple<string, bool>);
		}

		// Token: 0x060018B9 RID: 6329 RVA: 0x000D6E68 File Offset: 0x000D5068
		[CompilerGenerated]
		private static void <InitializeComponent>typedBindingsM__2537(DashboardItem A_0, string A_1)
		{
			if (A_0 != null)
			{
				A_0.SoundNameLow = A_1;
				return;
			}
		}

		// Token: 0x060018BA RID: 6330 RVA: 0x000D6E84 File Offset: 0x000D5084
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__2538(DashboardItem A_0)
		{
			return A_0;
		}

		// Token: 0x04000A45 RID: 2629
		private DashboardItem _item;

		// Token: 0x04000A46 RID: 2630
		private DashboardItem originalItem;

		// Token: 0x04000A47 RID: 2631
		private double orig_width;

		// Token: 0x04000A48 RID: 2632
		private double orig_height;

		// Token: 0x04000A49 RID: 2633
		private bool shouldStartItem;

		// Token: 0x04000A4A RID: 2634
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private Grid gridButtons;

		// Token: 0x04000A4B RID: 2635
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private LinkButton btnBack;

		// Token: 0x04000A4C RID: 2636
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private Grid LayoutRoot;

		// Token: 0x04000A4D RID: 2637
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private Grid PreviewGrid;

		// Token: 0x04000A4E RID: 2638
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private ActivityFrame activityFrame;

		// Token: 0x04000A4F RID: 2639
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private Button btnChangeSensor;

		// Token: 0x04000A50 RID: 2640
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private SfExpander panelCommon;

		// Token: 0x04000A51 RID: 2641
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private Button btnRotateGradient;

		// Token: 0x04000A52 RID: 2642
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private SfExpander panelTextItem;

		// Token: 0x04000A53 RID: 2643
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private SfExpander panelGauge;

		// Token: 0x04000A54 RID: 2644
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private SfExpander panelGaugeVar2;

		// Token: 0x04000A55 RID: 2645
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private SfExpander panelGaugeVar3;

		// Token: 0x04000A56 RID: 2646
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private SfExpander panelChart;

		// Token: 0x04000A57 RID: 2647
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private Picker chartStylePicker;

		// Token: 0x04000A58 RID: 2648
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private SfExpander panelLinearGauge;

		// Token: 0x04000A59 RID: 2649
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private SfExpander panelMinMaxAvg;

		// Token: 0x04000A5A RID: 2650
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private SfExpander panelWarningAndSound;

		// Token: 0x04000A5B RID: 2651
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private SfExpander panelApplyTo;

		// Token: 0x020001C3 RID: 451
		[CompilerGenerated]
		[Serializable]
		private sealed class <>c
		{
			// Token: 0x060018BB RID: 6331 RVA: 0x000D6E92 File Offset: 0x000D5092
			// Note: this type is marked as 'beforefieldinit'.
			static <>c()
			{
			}

			// Token: 0x060018BC RID: 6332 RVA: 0x00002050 File Offset: 0x00000250
			public <>c()
			{
			}

			// Token: 0x060018BD RID: 6333 RVA: 0x00027702 File Offset: 0x00025902
			internal bool <btnBack_Clicked>b__11_1(Page x)
			{
				return x is DashboardEditorPage;
			}

			// Token: 0x060018BE RID: 6334 RVA: 0x000D6E9E File Offset: 0x000D509E
			internal bool <btnSelectPID_Clicked>b__12_0(PID x)
			{
				return (x != null && x == PID.Empty) || (x is CustomPID && (x as CustomPID).IsAction);
			}

			// Token: 0x060018BF RID: 6335 RVA: 0x000D6EC2 File Offset: 0x000D50C2
			internal bool <btnSelectPID_Clicked>b__12_1(PID x)
			{
				return x != null && (!(x is CustomPID) || !(x as CustomPID).IsAction);
			}

			// Token: 0x060018C0 RID: 6336 RVA: 0x000200C4 File Offset: 0x0001E2C4
			internal ProxyPage <ApplyCurrentPage_Clicked>b__21_1(DashboardPage x)
			{
				return new ProxyPage(x);
			}

			// Token: 0x060018C1 RID: 6337 RVA: 0x00027702 File Offset: 0x00025902
			internal bool <ApplyCurrentPage_Clicked>b__21_2(Page x)
			{
				return x is DashboardEditorPage;
			}

			// Token: 0x060018C2 RID: 6338 RVA: 0x000200C4 File Offset: 0x0001E2C4
			internal ProxyPage <ApplyColorsCurrentPage_Clicked>b__22_1(DashboardPage x)
			{
				return new ProxyPage(x);
			}

			// Token: 0x060018C3 RID: 6339 RVA: 0x00027702 File Offset: 0x00025902
			internal bool <ApplyColorsCurrentPage_Clicked>b__22_2(Page x)
			{
				return x is DashboardEditorPage;
			}

			// Token: 0x060018C4 RID: 6340 RVA: 0x000200C4 File Offset: 0x0001E2C4
			internal ProxyPage <ApplyCurrentPageAllTypes_Clicked>b__23_1(DashboardPage x)
			{
				return new ProxyPage(x);
			}

			// Token: 0x060018C5 RID: 6341 RVA: 0x00027702 File Offset: 0x00025902
			internal bool <ApplyCurrentPageAllTypes_Clicked>b__23_2(Page x)
			{
				return x is DashboardEditorPage;
			}

			// Token: 0x060018C6 RID: 6342 RVA: 0x000200C4 File Offset: 0x0001E2C4
			internal ProxyPage <ApplyColorCurrentPageAllTypes_Clicked>b__24_1(DashboardPage x)
			{
				return new ProxyPage(x);
			}

			// Token: 0x060018C7 RID: 6343 RVA: 0x00027702 File Offset: 0x00025902
			internal bool <ApplyColorCurrentPageAllTypes_Clicked>b__24_2(Page x)
			{
				return x is DashboardEditorPage;
			}

			// Token: 0x060018C8 RID: 6344 RVA: 0x000200C4 File Offset: 0x0001E2C4
			internal ProxyPage <ApplyAllPages_Clicked>b__25_1(DashboardPage x)
			{
				return new ProxyPage(x);
			}

			// Token: 0x060018C9 RID: 6345 RVA: 0x00027702 File Offset: 0x00025902
			internal bool <ApplyAllPages_Clicked>b__25_2(Page x)
			{
				return x is DashboardEditorPage;
			}

			// Token: 0x060018CA RID: 6346 RVA: 0x000200C4 File Offset: 0x0001E2C4
			internal ProxyPage <ApplyColorAllPagesSameType_Clicked>b__26_1(DashboardPage x)
			{
				return new ProxyPage(x);
			}

			// Token: 0x060018CB RID: 6347 RVA: 0x00027702 File Offset: 0x00025902
			internal bool <ApplyColorAllPagesSameType_Clicked>b__26_2(Page x)
			{
				return x is DashboardEditorPage;
			}

			// Token: 0x060018CC RID: 6348 RVA: 0x000200C4 File Offset: 0x0001E2C4
			internal ProxyPage <ApplyColorAllPages_Clicked>b__27_1(DashboardPage x)
			{
				return new ProxyPage(x);
			}

			// Token: 0x060018CD RID: 6349 RVA: 0x00027702 File Offset: 0x00025902
			internal bool <ApplyColorAllPages_Clicked>b__27_2(Page x)
			{
				return x is DashboardEditorPage;
			}

			// Token: 0x060018CE RID: 6350 RVA: 0x000D6EE1 File Offset: 0x000D50E1
			internal int <btnSelectMultiplePIDs_Clicked>b__38_1(IPID x)
			{
				return x.Id;
			}

			// Token: 0x04000A5C RID: 2652
			public static readonly DashboardItemEditor.<>c <>9 = new DashboardItemEditor.<>c();

			// Token: 0x04000A5D RID: 2653
			public static Func<Page, bool> <>9__11_1;

			// Token: 0x04000A5E RID: 2654
			public static Func<PID, bool> <>9__12_0;

			// Token: 0x04000A5F RID: 2655
			public static Func<PID, bool> <>9__12_1;

			// Token: 0x04000A60 RID: 2656
			public static Func<DashboardPage, ProxyPage> <>9__21_1;

			// Token: 0x04000A61 RID: 2657
			public static Func<Page, bool> <>9__21_2;

			// Token: 0x04000A62 RID: 2658
			public static Func<DashboardPage, ProxyPage> <>9__22_1;

			// Token: 0x04000A63 RID: 2659
			public static Func<Page, bool> <>9__22_2;

			// Token: 0x04000A64 RID: 2660
			public static Func<DashboardPage, ProxyPage> <>9__23_1;

			// Token: 0x04000A65 RID: 2661
			public static Func<Page, bool> <>9__23_2;

			// Token: 0x04000A66 RID: 2662
			public static Func<DashboardPage, ProxyPage> <>9__24_1;

			// Token: 0x04000A67 RID: 2663
			public static Func<Page, bool> <>9__24_2;

			// Token: 0x04000A68 RID: 2664
			public static Func<DashboardPage, ProxyPage> <>9__25_1;

			// Token: 0x04000A69 RID: 2665
			public static Func<Page, bool> <>9__25_2;

			// Token: 0x04000A6A RID: 2666
			public static Func<DashboardPage, ProxyPage> <>9__26_1;

			// Token: 0x04000A6B RID: 2667
			public static Func<Page, bool> <>9__26_2;

			// Token: 0x04000A6C RID: 2668
			public static Func<DashboardPage, ProxyPage> <>9__27_1;

			// Token: 0x04000A6D RID: 2669
			public static Func<Page, bool> <>9__27_2;

			// Token: 0x04000A6E RID: 2670
			public static Func<IPID, int> <>9__38_1;
		}

		// Token: 0x020001C4 RID: 452
		[CompilerGenerated]
		private sealed class <>c__DisplayClass13_0
		{
			// Token: 0x060018CF RID: 6351 RVA: 0x00002050 File Offset: 0x00000250
			public <>c__DisplayClass13_0()
			{
			}

			// Token: 0x060018D0 RID: 6352 RVA: 0x000D6EE9 File Offset: 0x000D50E9
			internal void <SelectColor>b__0(Color new_color)
			{
				this.<>4__this.BindingContext.GetType().GetProperty(this.propertyName).SetValue(this.<>4__this.BindingContext, new_color);
			}

			// Token: 0x04000A6F RID: 2671
			public DashboardItemEditor <>4__this;

			// Token: 0x04000A70 RID: 2672
			public string propertyName;
		}

		// Token: 0x020001C5 RID: 453
		[CompilerGenerated]
		private sealed class <GetControlsOfType>d__9<T> : IEnumerable<T>, IEnumerable, IEnumerator<T>, IEnumerator, IDisposable where T : View
		{
			// Token: 0x060018D1 RID: 6353 RVA: 0x000D6F1C File Offset: 0x000D511C
			[DebuggerHidden]
			public <GetControlsOfType>d__9(int <>1__state)
			{
				this.<>1__state = <>1__state;
				this.<>l__initialThreadId = Environment.CurrentManagedThreadId;
			}

			// Token: 0x060018D2 RID: 6354 RVA: 0x000D6F38 File Offset: 0x000D5138
			[DebuggerHidden]
			void IDisposable.Dispose()
			{
				int num = this.<>1__state;
				if (num - -4 <= 1 || num == 2)
				{
					try
					{
						if (num == -4 || num == 2)
						{
							try
							{
							}
							finally
							{
								this.<>m__Finally2();
							}
						}
					}
					finally
					{
						this.<>m__Finally1();
					}
				}
				this.<>7__wrap1 = null;
				this.<>7__wrap2 = null;
				this.<>1__state = -2;
			}

			// Token: 0x060018D3 RID: 6355 RVA: 0x000D6FA8 File Offset: 0x000D51A8
			bool IEnumerator.MoveNext()
			{
				bool flag;
				try
				{
					switch (this.<>1__state)
					{
					case 0:
					{
						this.<>1__state = -1;
						T t = root as T;
						if (t != null)
						{
							this.<>2__current = t;
							this.<>1__state = 1;
							return true;
						}
						break;
					}
					case 1:
						this.<>1__state = -1;
						break;
					case 2:
						this.<>1__state = -4;
						goto IL_00DA;
					default:
						return false;
					}
					Layout<View> layout = root as Layout<View>;
					if (layout != null)
					{
						this.<>7__wrap1 = layout.Children.GetEnumerator();
						this.<>1__state = -3;
						goto IL_00F4;
					}
					goto IL_010E;
					IL_00DA:
					if (this.<>7__wrap2.MoveNext())
					{
						T t2 = this.<>7__wrap2.Current;
						this.<>2__current = t2;
						this.<>1__state = 2;
						return true;
					}
					this.<>m__Finally2();
					this.<>7__wrap2 = null;
					IL_00F4:
					if (this.<>7__wrap1.MoveNext())
					{
						View view = this.<>7__wrap1.Current;
						this.<>7__wrap2 = DashboardItemEditor.GetControlsOfType<T>(view).GetEnumerator();
						this.<>1__state = -4;
						goto IL_00DA;
					}
					this.<>m__Finally1();
					this.<>7__wrap1 = null;
					IL_010E:
					flag = false;
				}
				catch
				{
					this.System.IDisposable.Dispose();
					throw;
				}
				return flag;
			}

			// Token: 0x060018D4 RID: 6356 RVA: 0x000D70EC File Offset: 0x000D52EC
			private void <>m__Finally1()
			{
				this.<>1__state = -1;
				if (this.<>7__wrap1 != null)
				{
					this.<>7__wrap1.Dispose();
				}
			}

			// Token: 0x060018D5 RID: 6357 RVA: 0x000D7108 File Offset: 0x000D5308
			private void <>m__Finally2()
			{
				this.<>1__state = -3;
				if (this.<>7__wrap2 != null)
				{
					this.<>7__wrap2.Dispose();
				}
			}

			// Token: 0x17000F8F RID: 3983
			// (get) Token: 0x060018D6 RID: 6358 RVA: 0x000D7125 File Offset: 0x000D5325
			T IEnumerator<T>.Current
			{
				[DebuggerHidden]
				get
				{
					return this.<>2__current;
				}
			}

			// Token: 0x060018D7 RID: 6359 RVA: 0x000D712D File Offset: 0x000D532D
			[DebuggerHidden]
			void IEnumerator.Reset()
			{
				throw new NotSupportedException();
			}

			// Token: 0x17000F90 RID: 3984
			// (get) Token: 0x060018D8 RID: 6360 RVA: 0x000D7134 File Offset: 0x000D5334
			object IEnumerator.Current
			{
				[DebuggerHidden]
				get
				{
					return this.<>2__current;
				}
			}

			// Token: 0x060018D9 RID: 6361 RVA: 0x000D7144 File Offset: 0x000D5344
			[DebuggerHidden]
			IEnumerator<T> IEnumerable<T>.GetEnumerator()
			{
				DashboardItemEditor.<GetControlsOfType>d__9<T> <GetControlsOfType>d__;
				if (this.<>1__state == -2 && this.<>l__initialThreadId == Environment.CurrentManagedThreadId)
				{
					this.<>1__state = 0;
					<GetControlsOfType>d__ = this;
				}
				else
				{
					<GetControlsOfType>d__ = new DashboardItemEditor.<GetControlsOfType>d__9<T>(0);
				}
				<GetControlsOfType>d__.root = root;
				return <GetControlsOfType>d__;
			}

			// Token: 0x060018DA RID: 6362 RVA: 0x000D7187 File Offset: 0x000D5387
			[DebuggerHidden]
			IEnumerator IEnumerable.GetEnumerator()
			{
				return this.System.Collections.Generic.IEnumerable<T>.GetEnumerator();
			}

			// Token: 0x04000A71 RID: 2673
			private int <>1__state;

			// Token: 0x04000A72 RID: 2674
			private T <>2__current;

			// Token: 0x04000A73 RID: 2675
			private int <>l__initialThreadId;

			// Token: 0x04000A74 RID: 2676
			private View root;

			// Token: 0x04000A75 RID: 2677
			public View <>3__root;

			// Token: 0x04000A76 RID: 2678
			private IEnumerator<View> <>7__wrap1;

			// Token: 0x04000A77 RID: 2679
			private IEnumerator<T> <>7__wrap2;
		}

		// Token: 0x020001C6 RID: 454
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <Handle_Appearing>d__7 : IAsyncStateMachine
		{
			// Token: 0x060018DB RID: 6363 RVA: 0x000D7190 File Offset: 0x000D5390
			void IAsyncStateMachine.MoveNext()
			{
				DashboardItemEditor dashboardItemEditor = this;
				try
				{
					if (dashboardItemEditor.PreviewGrid.Children.Count == 0)
					{
						if (dashboardItemEditor.shouldStartItem)
						{
							dashboardItemEditor._item.Start();
						}
						Grid.SetRow(dashboardItemEditor._item, 0);
						Grid.SetColumn(dashboardItemEditor._item, 1);
						Grid.SetRowSpan(dashboardItemEditor._item, 1);
						Grid.SetColumnSpan(dashboardItemEditor._item, 1);
						double width = dashboardItemEditor.originalItem.Width;
						double height = dashboardItemEditor.originalItem.Height;
						if (dashboardItemEditor.orig_height < height)
						{
							dashboardItemEditor._item.WidthRequest = dashboardItemEditor.orig_width;
							dashboardItemEditor._item.HeightRequest = dashboardItemEditor.orig_height;
						}
						else
						{
							double num = height / dashboardItemEditor.orig_height;
							double num2 = dashboardItemEditor.orig_height / dashboardItemEditor.orig_width;
							dashboardItemEditor._item.WidthRequest = height / num2;
						}
						dashboardItemEditor.PreviewGrid.Children.Add(dashboardItemEditor._item);
					}
					dashboardItemEditor.BindingContext = null;
					dashboardItemEditor.BindingContext = dashboardItemEditor._item;
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

			// Token: 0x060018DC RID: 6364 RVA: 0x000D72CC File Offset: 0x000D54CC
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x04000A78 RID: 2680
			public int <>1__state;

			// Token: 0x04000A79 RID: 2681
			public AsyncVoidMethodBuilder <>t__builder;

			// Token: 0x04000A7A RID: 2682
			public DashboardItemEditor <>4__this;
		}

		// Token: 0x020001C7 RID: 455
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <btnBack_Clicked>d__11 : IAsyncStateMachine
		{
			// Token: 0x060018DD RID: 6365 RVA: 0x000D72DC File Offset: 0x000D54DC
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				DashboardItemEditor dashboardItemEditor = this;
				try
				{
					try
					{
						TaskAwaiter taskAwaiter;
						if (num != 0)
						{
							(sender as Button).IsEnabled = false;
							dashboardItemEditor.activityFrame.IsVisible = true;
							taskAwaiter = Task.Delay(100).GetAwaiter();
							if (!taskAwaiter.IsCompleted)
							{
								num = (num2 = 0);
								TaskAwaiter taskAwaiter2 = taskAwaiter;
								this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, DashboardItemEditor.<btnBack_Clicked>d__11>(ref taskAwaiter, ref this);
								return;
							}
						}
						else
						{
							TaskAwaiter taskAwaiter2;
							taskAwaiter = taskAwaiter2;
							taskAwaiter2 = default(TaskAwaiter);
							num = (num2 = -1);
						}
						taskAwaiter.GetResult();
						dashboardItemEditor.IsEnabled = false;
						dashboardItemEditor.PreviewGrid.Children.Clear();
						DashboardItem item = dashboardItemEditor._item;
						if (item != null)
						{
							item.Stop();
						}
						DashboardXamlPage.Instance.ShouldLoadDashboardFromSettings = true;
						DashboardPage dashboardPage = DashboardListViewModel.Current.Pages.FirstOrDefault((DashboardPage x) => x.Items.Contains(dashboardItemEditor.originalItem));
						if (dashboardPage != null)
						{
							int num3 = DashboardListViewModel.Current.Pages.IndexOf(dashboardPage);
							int num4 = dashboardPage.Items.IndexOf(dashboardItemEditor.originalItem);
							ProxyPage proxyPage = new ProxyPage(dashboardPage);
							proxyPage.Items[num4] = new ProxyItem(dashboardItemEditor._item);
							List<ProxyPage> list = new List<ProxyPage>(DashboardListViewModel.Current.Pages.Count);
							IEnumerator<DashboardPage> enumerator = DashboardListViewModel.Current.Pages.GetEnumerator();
							try
							{
								while (enumerator.MoveNext())
								{
									DashboardPage dashboardPage2 = enumerator.Current;
									ProxyPage proxyPage2;
									if (dashboardPage2 == dashboardPage)
									{
										proxyPage2 = proxyPage;
									}
									else
									{
										proxyPage2 = new ProxyPage(dashboardPage2);
									}
									list.Add(proxyPage2);
								}
							}
							finally
							{
								if (num < 0 && enumerator != null)
								{
									enumerator.Dispose();
								}
							}
							DashboardListViewModel.Current.SaveDashboardToSettings(list);
							Page page = dashboardItemEditor.Navigation.NavigationStack.FirstOrDefault((Page x) => x is DashboardEditorPage);
							if (page != null)
							{
								DashboardListViewModel.Current.LoadDashboardFromSettings();
								page.BindingContext = DashboardListViewModel.Current.Pages[num3];
							}
						}
						dashboardItemEditor.activityFrame.IsVisible = false;
						dashboardItemEditor.Navigation.PopAsync();
					}
					catch (Exception)
					{
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

			// Token: 0x060018DE RID: 6366 RVA: 0x000D7578 File Offset: 0x000D5778
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x04000A7B RID: 2683
			public int <>1__state;

			// Token: 0x04000A7C RID: 2684
			public AsyncVoidMethodBuilder <>t__builder;

			// Token: 0x04000A7D RID: 2685
			public object sender;

			// Token: 0x04000A7E RID: 2686
			public DashboardItemEditor <>4__this;

			// Token: 0x04000A7F RID: 2687
			private TaskAwaiter <>u__1;
		}

		// Token: 0x020001C8 RID: 456
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <btnChangeSensor_Clicked>d__37 : IAsyncStateMachine
		{
			// Token: 0x060018DF RID: 6367 RVA: 0x000D7588 File Offset: 0x000D5788
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				DashboardItemEditor dashboardItemEditor = this;
				try
				{
					TaskAwaiter taskAwaiter;
					TaskAwaiter taskAwaiter2;
					if (num != 0)
					{
						if (num != 1)
						{
							object obj = new PID_IDToPIDConverter().Convert(dashboardItemEditor._item.PID_Id, typeof(PID), null, CultureInfo.InvariantCulture);
							if (obj == null)
							{
								goto IL_01DE;
							}
							if (obj == PID.Empty)
							{
								goto IL_01DE;
							}
							if (obj is CustomPID && CustomPIDViewModel.CurrentCustom.PidCollection.Contains(obj))
							{
								Page page = new CustomPIDsEditorPage(obj as CustomPID);
								dashboardItemEditor.PreviewGrid.Children.Clear();
								DashboardItem item = dashboardItemEditor._item;
								if (item != null)
								{
									item.Stop();
								}
								dashboardItemEditor.shouldStartItem = true;
								taskAwaiter = dashboardItemEditor.Navigation.PushAsync(page).GetAwaiter();
								if (!taskAwaiter.IsCompleted)
								{
									num2 = 0;
									taskAwaiter2 = taskAwaiter;
									this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, DashboardItemEditor.<btnChangeSensor_Clicked>d__37>(ref taskAwaiter, ref this);
									return;
								}
								goto IL_010C;
							}
							else
							{
								Page page2 = new SettingsPIDOverrideEditorPageV3((IPID)obj);
								if (App.UseLegacyUI)
								{
									page2 = new SettingsPIDOverridePage((IPID)obj);
								}
								dashboardItemEditor.PreviewGrid.Children.Clear();
								DashboardItem item2 = dashboardItemEditor._item;
								if (item2 != null)
								{
									item2.Stop();
								}
								dashboardItemEditor.shouldStartItem = true;
								taskAwaiter = dashboardItemEditor.Navigation.PushAsync(page2).GetAwaiter();
								if (!taskAwaiter.IsCompleted)
								{
									num2 = 1;
									taskAwaiter2 = taskAwaiter;
									this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, DashboardItemEditor.<btnChangeSensor_Clicked>d__37>(ref taskAwaiter, ref this);
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
						goto IL_01C3;
					}
					taskAwaiter = taskAwaiter2;
					taskAwaiter2 = default(TaskAwaiter);
					num2 = -1;
					IL_010C:
					taskAwaiter.GetResult();
					IL_01C3:;
				}
				catch (Exception ex)
				{
					num2 = -2;
					this.<>t__builder.SetException(ex);
					return;
				}
				IL_01DE:
				num2 = -2;
				this.<>t__builder.SetResult();
			}

			// Token: 0x060018E0 RID: 6368 RVA: 0x000D77A4 File Offset: 0x000D59A4
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x04000A80 RID: 2688
			public int <>1__state;

			// Token: 0x04000A81 RID: 2689
			public AsyncVoidMethodBuilder <>t__builder;

			// Token: 0x04000A82 RID: 2690
			public DashboardItemEditor <>4__this;

			// Token: 0x04000A83 RID: 2691
			private TaskAwaiter <>u__1;
		}

		// Token: 0x020001C9 RID: 457
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <btnSelectMultiplePIDs_Clicked>d__38 : IAsyncStateMachine
		{
			// Token: 0x060018E1 RID: 6369 RVA: 0x000D77B4 File Offset: 0x000D59B4
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				DashboardItemEditor dashboardItemEditor = this;
				try
				{
					TaskAwaiter taskAwaiter;
					if (num != 0)
					{
						DashboardMultiPidSelector dashboardMultiPidSelector = new DashboardMultiPidSelector(delegate(List<IPID> newPids)
						{
							if (newPids == null || newPids.Count == 0)
							{
								return;
							}
							dashboardItemEditor._item.PID_IDs.Clear();
							dashboardItemEditor._item.PID_IDs.AddRange(newPids.Select((IPID x) => x.Id).ToArray<int>());
							MultiplePIDViewModel multiplePIDViewModel = dashboardItemEditor._item.Model as MultiplePIDViewModel;
							if (multiplePIDViewModel != null)
							{
								multiplePIDViewModel.SetPIDs(dashboardItemEditor._item.PID_IDs);
								dashboardItemEditor._item.OverrideName = true;
								dashboardItemEditor._item.CustomName = newPids[0].ShortName;
							}
						}, dashboardItemEditor._item.PID_IDs);
						taskAwaiter = dashboardItemEditor.Navigation.PushAsync(dashboardMultiPidSelector).GetAwaiter();
						if (!taskAwaiter.IsCompleted)
						{
							num2 = 0;
							TaskAwaiter taskAwaiter2 = taskAwaiter;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, DashboardItemEditor.<btnSelectMultiplePIDs_Clicked>d__38>(ref taskAwaiter, ref this);
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

			// Token: 0x060018E2 RID: 6370 RVA: 0x000D788C File Offset: 0x000D5A8C
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x04000A84 RID: 2692
			public int <>1__state;

			// Token: 0x04000A85 RID: 2693
			public AsyncVoidMethodBuilder <>t__builder;

			// Token: 0x04000A86 RID: 2694
			public DashboardItemEditor <>4__this;

			// Token: 0x04000A87 RID: 2695
			private TaskAwaiter <>u__1;
		}

		// Token: 0x020001CA RID: 458
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <btnSelectPID_Clicked>d__12 : IAsyncStateMachine
		{
			// Token: 0x060018E3 RID: 6371 RVA: 0x000D789C File Offset: 0x000D5A9C
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				DashboardItemEditor dashboardItemEditor = this;
				try
				{
					TaskAwaiter<IPID> taskAwaiter;
					TaskAwaiter<IPID> taskAwaiter2;
					IPID ipid;
					if (num != 0)
					{
						if (num != 1)
						{
							if (dashboardItemEditor._item.ItemType == DashboardItemTypes.Action)
							{
								taskAwaiter = PIDSelector.SelectPIDAsync(dashboardItemEditor._item.Model.SelectedPID, (PID x) => (x != null && x == PID.Empty) || (x is CustomPID && (x as CustomPID).IsAction)).GetAwaiter();
								if (!taskAwaiter.IsCompleted)
								{
									num = (num2 = 0);
									taskAwaiter2 = taskAwaiter;
									this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<IPID>, DashboardItemEditor.<btnSelectPID_Clicked>d__12>(ref taskAwaiter, ref this);
									return;
								}
								goto IL_00B0;
							}
							else
							{
								taskAwaiter = PIDSelector.SelectPIDAsync(dashboardItemEditor._item.Model.SelectedPID, (PID x) => x != null && (!(x is CustomPID) || !(x as CustomPID).IsAction)).GetAwaiter();
								if (!taskAwaiter.IsCompleted)
								{
									num = (num2 = 1);
									taskAwaiter2 = taskAwaiter;
									this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<IPID>, DashboardItemEditor.<btnSelectPID_Clicked>d__12>(ref taskAwaiter, ref this);
									return;
								}
							}
						}
						else
						{
							taskAwaiter = taskAwaiter2;
							taskAwaiter2 = default(TaskAwaiter<IPID>);
							num = (num2 = -1);
						}
						ipid = taskAwaiter.GetResult();
						goto IL_0147;
					}
					taskAwaiter = taskAwaiter2;
					taskAwaiter2 = default(TaskAwaiter<IPID>);
					num = (num2 = -1);
					IL_00B0:
					ipid = taskAwaiter.GetResult();
					IL_0147:
					if (ipid != null)
					{
						dashboardItemEditor._item.PID_Id = ipid.Id;
						if (ipid.Minimum < ipid.Maximum)
						{
							dashboardItemEditor._item.Minimum = ipid.Minimum;
							dashboardItemEditor._item.Maximum = ipid.Maximum;
						}
						else if (double.IsFinite(ipid.Minimum))
						{
							dashboardItemEditor._item.Minimum = ipid.Minimum;
							dashboardItemEditor._item.Maximum = ipid.Minimum + 1.0;
						}
						else if (double.IsFinite(ipid.Maximum))
						{
							dashboardItemEditor._item.Maximum = ipid.Maximum;
							dashboardItemEditor._item.Minimum = ipid.Maximum - 1.0;
						}
						else
						{
							dashboardItemEditor._item.Minimum = 0.0;
							dashboardItemEditor._item.Maximum = 100.0;
						}
						dashboardItemEditor._item.CustomName = ipid.ShortName;
						dashboardItemEditor._item.Start();
						List<OBDRequest> list = new List<OBDRequest>();
						dashboardItemEditor._item.Model.GetRequests(list, null, "");
						List<OBDRequest>.Enumerator enumerator = list.GetEnumerator();
						try
						{
							while (enumerator.MoveNext())
							{
								OBDRequest obdrequest = enumerator.Current;
								App.OBDReader.AddRequestToQueue(obdrequest);
							}
						}
						finally
						{
							if (num < 0)
							{
								((IDisposable)enumerator).Dispose();
							}
						}
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

			// Token: 0x060018E4 RID: 6372 RVA: 0x000D7BB4 File Offset: 0x000D5DB4
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x04000A88 RID: 2696
			public int <>1__state;

			// Token: 0x04000A89 RID: 2697
			public AsyncVoidMethodBuilder <>t__builder;

			// Token: 0x04000A8A RID: 2698
			public DashboardItemEditor <>4__this;

			// Token: 0x04000A8B RID: 2699
			private TaskAwaiter<IPID> <>u__1;
		}
	}
}
