using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using CarScannerXamarinForms.Dashboard;
using CarScannerXamarinForms.Dashboard.DashboardPages;
using CarScannerXamarinForms.Dashboard.ProxySettings;
using CarScannerXamarinForms.OBD2.PIDS;
using CarScannerXamarinForms.Pages.Dashboard;
using CarScannerXamarinForms.PlatformAdapters;
using CarScannerXamarinForms.Settings;
using CarScannerXamarinForms.UserControls;
using CarScannerXamarinForms.ViewModels;
using FFImageLoading.Forms;
using Syncfusion.ListView.XForms;
using Xamarin.Forms;
using Xamarin.Forms.Internals;
using Xamarin.Forms.PlatformConfiguration.iOSSpecific;
using Xamarin.Forms.Xaml;
using Xamarin.Forms.Xaml.Diagnostics;
using Xamarin.Forms.Xaml.Internals;

namespace CarScannerXamarinForms
{
	// Token: 0x020000AD RID: 173
	[XamlFilePath("Pages\\Dashboard\\DashboardPageAdder.xaml")]
	public class DashboardPageAdder : ContentPage
	{
		// Token: 0x06000365 RID: 869 RVA: 0x0002226E File Offset: 0x0002046E
		public DashboardPageAdder()
		{
			this.InitializeComponent();
			if (PlatformHelper.IsiOS)
			{
				Page.SetPrefersHomeIndicatorAutoHidden(this, false);
			}
		}

		// Token: 0x06000366 RID: 870 RVA: 0x00022295 File Offset: 0x00020495
		private void EntrySearch_Completed(object sender, EventArgs e)
		{
			PlatformHelper.CommonService.HideKeyboard();
		}

		// Token: 0x06000367 RID: 871 RVA: 0x000222A4 File Offset: 0x000204A4
		private async void LvAvailablePids_ItemSelected(object sender, SelectedItemChangedEventArgs e)
		{
			if (this.lvAvailablePids.SelectedItem != null)
			{
				PlatformHelper.CommonService.HideKeyboard();
				IPID pid = this.lvAvailablePids.SelectedItem as IPID;
				this.lvAvailablePids.SelectedItem = null;
				if (this.model.CanAdd)
				{
					string cancel = Translate.GetString("btnCancel.Content");
					string text = await this.DisplayActionSheetCustom(Translate.GetString("ios_DashboardItemEditor_DisplayType"), cancel, null, StaticLists.DashboardItemTypesList.ToArray());
					if (!(text == cancel))
					{
						if (StaticLists.DashboardItemTypesList.Contains(text))
						{
							DashboardItemTypes dashboardItemTypes = (DashboardItemTypes)StaticLists.DashboardItemTypesList.IndexOf(text);
							this.model.AddPidToSelected(pid, dashboardItemTypes);
							this.lvAvailablePids.SelectedItem = null;
							this.UpdatebtnOK();
						}
						pid = null;
						cancel = null;
					}
				}
			}
		}

		// Token: 0x06000368 RID: 872 RVA: 0x000222DC File Offset: 0x000204DC
		private void LvSelectedPids_ItemSelected(object sender, SelectedItemChangedEventArgs e)
		{
			PlatformHelper.CommonService.HideKeyboard();
			if (this.lvSelectedPids.SelectedItem != null)
			{
				IPID ipid = this.lvSelectedPids.SelectedItem as IPID;
				this.model.RemovePidFromSelected(ipid);
				this.lvSelectedPids.SelectedItem = null;
				this.UpdatebtnOK();
			}
		}

		// Token: 0x06000369 RID: 873 RVA: 0x00022330 File Offset: 0x00020530
		private async void entrySearch_TextChanged(object sender, TextChangedEventArgs e)
		{
			this.filtertext = this.entrySearch.Text;
			await Task.Delay(500);
			if (this.filtertext == this.entrySearch.Text)
			{
				try
				{
					this.model.Filter = this.filtertext;
				}
				catch
				{
				}
			}
		}

		// Token: 0x0600036A RID: 874 RVA: 0x000027D4 File Offset: 0x000009D4
		private void Handle_SizeChanged(object sender, EventArgs e)
		{
		}

		// Token: 0x0600036B RID: 875 RVA: 0x00022367 File Offset: 0x00020567
		protected override bool OnBackButtonPressed()
		{
			this.btnBack_Clicked(this.btnBack, null);
			return true;
		}

		// Token: 0x0600036C RID: 876 RVA: 0x00022378 File Offset: 0x00020578
		private void btnBack_Clicked(object sender, EventArgs e)
		{
			if (this.panelStep1.IsVisible)
			{
				base.Navigation.PopAsync();
				return;
			}
			this.panelStep2.IsVisible = false;
			this.panelStep1.IsVisible = true;
			this.lbTitle.Text = Translate.GetString("ios_ChooseDashboardPageType");
		}

		// Token: 0x0600036D RID: 877 RVA: 0x000223CC File Offset: 0x000205CC
		private void btnDel_Clicked(object sender, EventArgs e)
		{
			IPID ipid = (sender as View).BindingContext as IPID;
			this.model.RemovePidFromSelected(ipid);
			this.UpdatebtnOK();
		}

		// Token: 0x0600036E RID: 878 RVA: 0x000223FC File Offset: 0x000205FC
		private void UpdatebtnOK()
		{
			if (!this.model.CanAdd)
			{
				this.btnOK.IsEnabled = true;
				this.btnOK.TextColor = Color.White;
				this.btnOK.BackgroundColor = Color.Green;
				return;
			}
			this.btnOK.IsEnabled = false;
			this.btnOK.TextColor = Color.White;
			this.btnOK.BackgroundColor = Color.DarkGray;
		}

		// Token: 0x0600036F RID: 879 RVA: 0x00022470 File Offset: 0x00020670
		private async void lv_ItemTapped(object sender, ItemTappedEventArgs e)
		{
			DescriptionPage descriptionPage = e.ItemData as DescriptionPage;
			if (descriptionPage.DashboardType == DashboardTypes.Custom)
			{
				DashboardListViewModel.Current.Pages.Add(new Dash_CustomPage());
				DashboardListViewModel.Current.SaveDashboardToSettings();
				SharedSettings.Current.DashboardLastPage = DashboardListViewModel.Current.Pages.Count - 1;
				DashboardXamlPage.Instance.ShouldLoadDashboardFromSettings = true;
				await base.Navigation.PopAsync();
			}
			else
			{
				this.model = new PageAdderViewModel(descriptionPage);
				this.panelStep1.IsVisible = false;
				this.panelStep2.IsVisible = true;
				this.panelStep2.BindingContext = this.model;
				this.btnOK.IsEnabled = false;
				this.btnOK.TextColor = Color.White;
				this.btnOK.BackgroundColor = Color.DarkGray;
				this.lbTitle.Text = Translate.GetString("ios_ChooseSensors");
			}
		}

		// Token: 0x06000370 RID: 880 RVA: 0x000224AF File Offset: 0x000206AF
		private void btnOK_Clicked(object sender, EventArgs e)
		{
			this.model.BuildPage();
			base.Navigation.PopAsync();
		}

		// Token: 0x06000371 RID: 881 RVA: 0x000224C8 File Offset: 0x000206C8
		private List<int> GetOtherModelsPID_Ids()
		{
			List<int> list = new List<int>();
			foreach (DashboardPage dashboardPage in DashboardListViewModel.Current.Pages)
			{
				foreach (DashboardItem dashboardItem in dashboardPage.Items)
				{
					list.Add(dashboardItem.PID_Id);
				}
			}
			return list;
		}

		// Token: 0x06000372 RID: 882 RVA: 0x00022558 File Offset: 0x00020758
		private void SetPids(DashboardPage dashPage)
		{
			int itemsCount = dashPage.ItemsCount;
			List<int> other_pages = this.GetOtherModelsPID_Ids();
			PID[] array = LiveDataPIDModel._PIDCollection.Where((PID x) => x is IPIDFloatValue && !other_pages.Contains(x.Id)).ToArray<PID>();
			if (array.Length >= itemsCount)
			{
				array = array.Take(itemsCount).ToArray<PID>();
				for (int i = 0; i < dashPage.ItemsCount; i++)
				{
					dashPage.Items[i].PID_Id = array[i].Id;
					dashPage.Items[i].Minimum = array[i].Minimum;
					dashPage.Items[i].Maximum = array[i].Maximum;
				}
			}
			else
			{
				for (int j = 0; j < array.Length; j++)
				{
					dashPage.Items[j].PID_Id = array[j].Id;
					dashPage.Items[j].Minimum = array[j].Minimum;
					dashPage.Items[j].Maximum = array[j].Maximum;
				}
			}
			foreach (DashboardItem dashboardItem in dashPage.Items)
			{
				dashboardItem.ValueFontSize = Device.GetNamedSize(4, typeof(Label)) * 1.5;
			}
		}

		// Token: 0x06000373 RID: 883 RVA: 0x000226CC File Offset: 0x000208CC
		private void btnInfo_Clicked(object sender, EventArgs e)
		{
			base.DisplayAlert(Translate.GetString("ios_ChooseDashboardPageType"), Translate.GetString("ios_ChooseDashboardPageType_InfoText"), "OK");
		}

		// Token: 0x06000374 RID: 884 RVA: 0x000226EE File Offset: 0x000208EE
		private void btnSkip_Clicked(object sender, EventArgs e)
		{
			while (this.model.CanAdd)
			{
				this.model.AddPidToSelected(PID.Empty, DashboardItemTypes.Text);
			}
			this.model.BuildPage();
			base.Navigation.PopAsync();
		}

		// Token: 0x06000375 RID: 885 RVA: 0x00022728 File Offset: 0x00020928
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private void InitializeComponent()
		{
			if (ResourceLoader.IsEnabled && ResourceLoader.CanProvideContentFor(new ResourceLoader.ResourceLoadingQuery
			{
				AssemblyName = typeof(DashboardPageAdder).GetTypeInfo().Assembly.GetName(),
				ResourcePath = "Pages/Dashboard/DashboardPageAdder.xaml",
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
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension = new DynamicResourceExtension(), new Uri("Pages\\Dashboard\\DashboardPageAdder.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 14, 5);
			BoolToNegativeConverter boolToNegativeConverter;
			VisualDiagnostics.RegisterSourceInfo(boolToNegativeConverter = new BoolToNegativeConverter(), new Uri("Pages\\Dashboard\\DashboardPageAdder.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 20, 14);
			ResourceDictionary resourceDictionary;
			VisualDiagnostics.RegisterSourceInfo(resourceDictionary = new ResourceDictionary(), new Uri("Pages\\Dashboard\\DashboardPageAdder.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 19, 10);
			ColumnDefinition columnDefinition;
			VisualDiagnostics.RegisterSourceInfo(columnDefinition = new ColumnDefinition(), new Uri("Pages\\Dashboard\\DashboardPageAdder.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 30, 18);
			ColumnDefinition columnDefinition2;
			VisualDiagnostics.RegisterSourceInfo(columnDefinition2 = new ColumnDefinition(), new Uri("Pages\\Dashboard\\DashboardPageAdder.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 31, 18);
			ColumnDefinition columnDefinition3;
			VisualDiagnostics.RegisterSourceInfo(columnDefinition3 = new ColumnDefinition(), new Uri("Pages\\Dashboard\\DashboardPageAdder.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 32, 18);
			DynamicResourceExtension dynamicResourceExtension2;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension2 = new DynamicResourceExtension(), new Uri("Pages\\Dashboard\\DashboardPageAdder.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 39, 17);
			Translate translate;
			VisualDiagnostics.RegisterSourceInfo(translate = new Translate(), new Uri("Pages\\Dashboard\\DashboardPageAdder.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 40, 17);
			OnPlatform<bool> onPlatform;
			VisualDiagnostics.RegisterSourceInfo(onPlatform = new OnPlatform<bool>(), new Uri("Pages\\Dashboard\\DashboardPageAdder.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 42, 22);
			LinkButton linkButton;
			VisualDiagnostics.RegisterSourceInfo(linkButton = new LinkButton(), new Uri("Pages\\Dashboard\\DashboardPageAdder.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 34, 14);
			DynamicResourceExtension dynamicResourceExtension3;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension3 = new DynamicResourceExtension(), new Uri("Pages\\Dashboard\\DashboardPageAdder.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 53, 17);
			Translate translate2;
			VisualDiagnostics.RegisterSourceInfo(translate2 = new Translate(), new Uri("Pages\\Dashboard\\DashboardPageAdder.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 54, 17);
			NonScalableLabel nonScalableLabel;
			VisualDiagnostics.RegisterSourceInfo(nonScalableLabel = new NonScalableLabel(), new Uri("Pages\\Dashboard\\DashboardPageAdder.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 49, 14);
			DynamicResourceExtension dynamicResourceExtension4;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension4 = new DynamicResourceExtension(), new Uri("Pages\\Dashboard\\DashboardPageAdder.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 60, 17);
			DynamicResourceExtension dynamicResourceExtension5;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension5 = new DynamicResourceExtension(), new Uri("Pages\\Dashboard\\DashboardPageAdder.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 61, 17);
			On on;
			VisualDiagnostics.RegisterSourceInfo(on = new On(), new Uri("Pages\\Dashboard\\DashboardPageAdder.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 64, 26);
			On on2;
			VisualDiagnostics.RegisterSourceInfo(on2 = new On(), new Uri("Pages\\Dashboard\\DashboardPageAdder.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 65, 26);
			OnPlatform<Thickness> onPlatform2;
			VisualDiagnostics.RegisterSourceInfo(onPlatform2 = new OnPlatform<Thickness>(), new Uri("Pages\\Dashboard\\DashboardPageAdder.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 63, 22);
			LinkButton linkButton2;
			VisualDiagnostics.RegisterSourceInfo(linkButton2 = new LinkButton(), new Uri("Pages\\Dashboard\\DashboardPageAdder.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 56, 14);
			Grid grid;
			VisualDiagnostics.RegisterSourceInfo(grid = new Grid(), new Uri("Pages\\Dashboard\\DashboardPageAdder.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 24, 10);
			OnPlatform<Thickness> onPlatform3;
			VisualDiagnostics.RegisterSourceInfo(onPlatform3 = new OnPlatform<Thickness>(), new Uri("Pages\\Dashboard\\DashboardPageAdder.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 78, 18);
			RowDefinition rowDefinition;
			VisualDiagnostics.RegisterSourceInfo(rowDefinition = new RowDefinition(), new Uri("Pages\\Dashboard\\DashboardPageAdder.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 85, 22);
			RowDefinition rowDefinition2;
			VisualDiagnostics.RegisterSourceInfo(rowDefinition2 = new RowDefinition(), new Uri("Pages\\Dashboard\\DashboardPageAdder.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 86, 22);
			RowDefinition rowDefinition3;
			VisualDiagnostics.RegisterSourceInfo(rowDefinition3 = new RowDefinition(), new Uri("Pages\\Dashboard\\DashboardPageAdder.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 87, 22);
			DynamicResourceExtension dynamicResourceExtension6;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension6 = new DynamicResourceExtension(), new Uri("Pages\\Dashboard\\DashboardPageAdder.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 94, 21);
			List<DescriptionPage> dashboardPageTypes;
			VisualDiagnostics.RegisterSourceInfo(dashboardPageTypes = StaticLists.DashboardPageTypes, new Uri("Pages\\Dashboard\\DashboardPageAdder.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 97, 21);
			BindingExtension bindingExtension;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension = new BindingExtension(), new Uri("Pages\\Dashboard\\DashboardPageAdder.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 97, 21);
			DynamicResourceExtension dynamicResourceExtension7;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension7 = new DynamicResourceExtension(), new Uri("Pages\\Dashboard\\DashboardPageAdder.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 98, 21);
			GridLayout gridLayout;
			VisualDiagnostics.RegisterSourceInfo(gridLayout = new GridLayout(), new Uri("Pages\\Dashboard\\DashboardPageAdder.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 100, 26);
			DataTemplate dataTemplate;
			VisualDiagnostics.RegisterSourceInfo(dataTemplate = new DataTemplate(), new Uri("Pages\\Dashboard\\DashboardPageAdder.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 103, 26);
			SfListView sfListView;
			VisualDiagnostics.RegisterSourceInfo(sfListView = new SfListView(), new Uri("Pages\\Dashboard\\DashboardPageAdder.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 90, 18);
			Translate translate3;
			VisualDiagnostics.RegisterSourceInfo(translate3 = new Translate(), new Uri("Pages\\Dashboard\\DashboardPageAdder.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 122, 21);
			ActivityFrame activityFrame;
			VisualDiagnostics.RegisterSourceInfo(activityFrame = new ActivityFrame(), new Uri("Pages\\Dashboard\\DashboardPageAdder.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 118, 18);
			Grid grid2;
			VisualDiagnostics.RegisterSourceInfo(grid2 = new Grid(), new Uri("Pages\\Dashboard\\DashboardPageAdder.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 83, 14);
			RowDefinition rowDefinition4;
			VisualDiagnostics.RegisterSourceInfo(rowDefinition4 = new RowDefinition(), new Uri("Pages\\Dashboard\\DashboardPageAdder.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 128, 22);
			RowDefinition rowDefinition5;
			VisualDiagnostics.RegisterSourceInfo(rowDefinition5 = new RowDefinition(), new Uri("Pages\\Dashboard\\DashboardPageAdder.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 129, 22);
			RowDefinition rowDefinition6;
			VisualDiagnostics.RegisterSourceInfo(rowDefinition6 = new RowDefinition(), new Uri("Pages\\Dashboard\\DashboardPageAdder.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 130, 22);
			RowDefinition rowDefinition7;
			VisualDiagnostics.RegisterSourceInfo(rowDefinition7 = new RowDefinition(), new Uri("Pages\\Dashboard\\DashboardPageAdder.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 131, 22);
			RowDefinition rowDefinition8;
			VisualDiagnostics.RegisterSourceInfo(rowDefinition8 = new RowDefinition(), new Uri("Pages\\Dashboard\\DashboardPageAdder.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 132, 22);
			ColumnDefinition columnDefinition4;
			VisualDiagnostics.RegisterSourceInfo(columnDefinition4 = new ColumnDefinition(), new Uri("Pages\\Dashboard\\DashboardPageAdder.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 136, 26);
			ColumnDefinition columnDefinition5;
			VisualDiagnostics.RegisterSourceInfo(columnDefinition5 = new ColumnDefinition(), new Uri("Pages\\Dashboard\\DashboardPageAdder.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 137, 26);
			Entry entry;
			VisualDiagnostics.RegisterSourceInfo(entry = new Entry(), new Uri("Pages\\Dashboard\\DashboardPageAdder.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 145, 22);
			Grid grid3;
			VisualDiagnostics.RegisterSourceInfo(grid3 = new Grid(), new Uri("Pages\\Dashboard\\DashboardPageAdder.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 134, 18);
			BindingExtension bindingExtension2;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension2 = new BindingExtension(), new Uri("Pages\\Dashboard\\DashboardPageAdder.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 160, 21);
			DataTemplate dataTemplate2;
			VisualDiagnostics.RegisterSourceInfo(dataTemplate2 = new DataTemplate(), new Uri("Pages\\Dashboard\\DashboardPageAdder.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 162, 26);
			ListView listView;
			VisualDiagnostics.RegisterSourceInfo(listView = new ListView(), new Uri("Pages\\Dashboard\\DashboardPageAdder.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 155, 18);
			DynamicResourceExtension dynamicResourceExtension8;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension8 = new DynamicResourceExtension(), new Uri("Pages\\Dashboard\\DashboardPageAdder.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 187, 21);
			Translate translate4;
			VisualDiagnostics.RegisterSourceInfo(translate4 = new Translate(), new Uri("Pages\\Dashboard\\DashboardPageAdder.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 190, 35);
			Span span;
			VisualDiagnostics.RegisterSourceInfo(span = new Span(), new Uri("Pages\\Dashboard\\DashboardPageAdder.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 190, 30);
			Span span2;
			VisualDiagnostics.RegisterSourceInfo(span2 = new Span(), new Uri("Pages\\Dashboard\\DashboardPageAdder.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 191, 30);
			BindingExtension bindingExtension3;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension3 = new BindingExtension(), new Uri("Pages\\Dashboard\\DashboardPageAdder.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 192, 35);
			Span span3;
			VisualDiagnostics.RegisterSourceInfo(span3 = new Span(), new Uri("Pages\\Dashboard\\DashboardPageAdder.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 192, 30);
			Span span4;
			VisualDiagnostics.RegisterSourceInfo(span4 = new Span(), new Uri("Pages\\Dashboard\\DashboardPageAdder.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 193, 30);
			BindingExtension bindingExtension4;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension4 = new BindingExtension(), new Uri("Pages\\Dashboard\\DashboardPageAdder.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 194, 35);
			Span span5;
			VisualDiagnostics.RegisterSourceInfo(span5 = new Span(), new Uri("Pages\\Dashboard\\DashboardPageAdder.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 194, 30);
			FormattedString formattedString;
			VisualDiagnostics.RegisterSourceInfo(formattedString = new FormattedString(), new Uri("Pages\\Dashboard\\DashboardPageAdder.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 189, 26);
			Label label;
			VisualDiagnostics.RegisterSourceInfo(label = new Label(), new Uri("Pages\\Dashboard\\DashboardPageAdder.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 184, 18);
			BindingExtension bindingExtension5;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension5 = new BindingExtension(), new Uri("Pages\\Dashboard\\DashboardPageAdder.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 203, 21);
			DataTemplate dataTemplate3;
			VisualDiagnostics.RegisterSourceInfo(dataTemplate3 = new DataTemplate(), new Uri("Pages\\Dashboard\\DashboardPageAdder.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 205, 26);
			ListView listView2;
			VisualDiagnostics.RegisterSourceInfo(listView2 = new ListView(), new Uri("Pages\\Dashboard\\DashboardPageAdder.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 198, 18);
			StaticResourceExtension staticResourceExtension;
			VisualDiagnostics.RegisterSourceInfo(staticResourceExtension = new StaticResourceExtension(), new Uri("Pages\\Dashboard\\DashboardPageAdder.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 232, 25);
			BindingExtension bindingExtension6;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension6 = new BindingExtension(), new Uri("Pages\\Dashboard\\DashboardPageAdder.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 232, 25);
			Button button;
			VisualDiagnostics.RegisterSourceInfo(button = new Button(), new Uri("Pages\\Dashboard\\DashboardPageAdder.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 228, 22);
			Translate translate5;
			VisualDiagnostics.RegisterSourceInfo(translate5 = new Translate(), new Uri("Pages\\Dashboard\\DashboardPageAdder.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 239, 25);
			Button button2;
			VisualDiagnostics.RegisterSourceInfo(button2 = new Button(), new Uri("Pages\\Dashboard\\DashboardPageAdder.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 235, 22);
			Grid grid4;
			VisualDiagnostics.RegisterSourceInfo(grid4 = new Grid(), new Uri("Pages\\Dashboard\\DashboardPageAdder.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 227, 18);
			Grid grid5;
			VisualDiagnostics.RegisterSourceInfo(grid5 = new Grid(), new Uri("Pages\\Dashboard\\DashboardPageAdder.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 126, 14);
			Grid grid6;
			VisualDiagnostics.RegisterSourceInfo(grid6 = new Grid(), new Uri("Pages\\Dashboard\\DashboardPageAdder.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 76, 10);
			VisualDiagnostics.RegisterSourceInfo(this, new Uri("Pages\\Dashboard\\DashboardPageAdder.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 2, 2);
			NameScope nameScope = NameScope.GetNameScope(this) ?? new NameScope();
			NameScope.SetNameScope(this, nameScope);
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
			nameScope.RegisterName("lbTitle", nonScalableLabel);
			if (nonScalableLabel.StyleId == null)
			{
				nonScalableLabel.StyleId = "lbTitle";
			}
			nameScope.RegisterName("LayoutRoot", grid6);
			if (grid6.StyleId == null)
			{
				grid6.StyleId = "LayoutRoot";
			}
			nameScope.RegisterName("panelStep1", grid2);
			if (grid2.StyleId == null)
			{
				grid2.StyleId = "panelStep1";
			}
			nameScope.RegisterName("lv", sfListView);
			if (sfListView.StyleId == null)
			{
				sfListView.StyleId = "lv";
			}
			nameScope.RegisterName("activityFrame", activityFrame);
			if (activityFrame.StyleId == null)
			{
				activityFrame.StyleId = "activityFrame";
			}
			nameScope.RegisterName("panelStep2", grid5);
			if (grid5.StyleId == null)
			{
				grid5.StyleId = "panelStep2";
			}
			nameScope.RegisterName("entrySearch", entry);
			if (entry.StyleId == null)
			{
				entry.StyleId = "entrySearch";
			}
			nameScope.RegisterName("lvAvailablePids", listView);
			if (listView.StyleId == null)
			{
				listView.StyleId = "lvAvailablePids";
			}
			nameScope.RegisterName("lvSelectedPids", listView2);
			if (listView2.StyleId == null)
			{
				listView2.StyleId = "lvSelectedPids";
			}
			nameScope.RegisterName("btnOK", button);
			if (button.StyleId == null)
			{
				button.StyleId = "btnOK";
			}
			nameScope.RegisterName("btnSkip", button2);
			if (button2.StyleId == null)
			{
				button2.StyleId = "btnSkip";
			}
			this.gridButtons = grid;
			this.btnBack = linkButton;
			this.lbTitle = nonScalableLabel;
			this.LayoutRoot = grid6;
			this.panelStep1 = grid2;
			this.lv = sfListView;
			this.activityFrame = activityFrame;
			this.panelStep2 = grid5;
			this.entrySearch = entry;
			this.lvAvailablePids = listView;
			this.lvSelectedPids = listView2;
			this.btnOK = button;
			this.btnSkip = button2;
			this.Resources = resourceDictionary;
			resourceDictionary.Add("BoolToNegativeConverter", boolToNegativeConverter);
			this.SetValue(Page.PaddingProperty, new Thickness(5.0, 20.0, 5.0, 5.0));
			this.SetValue(Page.PrefersStatusBarHiddenProperty, 0);
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
			xmlNamespaceResolver.Add("ffimageloading", "clr-namespace:FFImageLoading.Forms;assembly=FFImageLoading.Forms");
			xmlNamespaceResolver.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver.Add("syncfusion", "clr-namespace:Syncfusion.ListView.XForms;assembly=Syncfusion.SfListView.XForms");
			xmlNamespaceResolver.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider.Add(typeFromHandle2, new XamlTypeResolver(xmlNamespaceResolver, typeof(DashboardPageAdder).GetTypeInfo().Assembly));
			xamlServiceProvider.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(14, 5)));
			DynamicResource dynamicResource = markupExtension.ProvideValue(xamlServiceProvider);
			this.SetDynamicResource(VisualElement.BackgroundColorProperty, dynamicResource.Key);
			this.SetValue(NavigationPage.HasBackButtonProperty, false);
			this.SetValue(NavigationPage.HasNavigationBarProperty, true);
			this.SizeChanged += this.Handle_SizeChanged;
			this.Resources = resourceDictionary;
			grid.SetValue(Grid.RowProperty, 0);
			grid.SetValue(Layout.PaddingProperty, new Thickness(0.0));
			grid.SetValue(Grid.RowSpacingProperty, 0.0);
			columnDefinition.SetValue(ColumnDefinition.WidthProperty, new GridLengthTypeConverter().ConvertFromInvariantString("auto"));
			grid.GetValue(Grid.ColumnDefinitionsProperty).Add(columnDefinition);
			columnDefinition2.SetValue(ColumnDefinition.WidthProperty, new GridLengthTypeConverter().ConvertFromInvariantString("*"));
			grid.GetValue(Grid.ColumnDefinitionsProperty).Add(columnDefinition2);
			columnDefinition3.SetValue(ColumnDefinition.WidthProperty, new GridLengthTypeConverter().ConvertFromInvariantString("auto"));
			grid.GetValue(Grid.ColumnDefinitionsProperty).Add(columnDefinition3);
			linkButton.SetValue(Grid.ColumnProperty, 0);
			linkButton.Clicked += this.btnBack_Clicked;
			linkButton.SetValue(View.HorizontalOptionsProperty, LayoutOptions.Start);
			dynamicResourceExtension2.Key = "NavigationBarButton";
			IMarkupExtension<DynamicResource> markupExtension2 = dynamicResourceExtension2;
			XamlServiceProvider xamlServiceProvider2 = new XamlServiceProvider();
			Type typeFromHandle3 = typeof(IProvideValueTarget);
			object[] array2 = new object[0 + 3];
			array2[0] = linkButton;
			array2[1] = grid;
			array2[2] = this;
			object obj2;
			xamlServiceProvider2.Add(typeFromHandle3, obj2 = new SimpleValueTargetProvider(array2, VisualElement.StyleProperty, nameScope));
			xamlServiceProvider2.Add(typeof(IReferenceProvider), obj2);
			Type typeFromHandle4 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver2 = new XmlNamespaceResolver();
			xmlNamespaceResolver2.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver2.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver2.Add("ffimageloading", "clr-namespace:FFImageLoading.Forms;assembly=FFImageLoading.Forms");
			xmlNamespaceResolver2.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver2.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver2.Add("syncfusion", "clr-namespace:Syncfusion.ListView.XForms;assembly=Syncfusion.SfListView.XForms");
			xmlNamespaceResolver2.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider2.Add(typeFromHandle4, new XamlTypeResolver(xmlNamespaceResolver2, typeof(DashboardPageAdder).GetTypeInfo().Assembly));
			xamlServiceProvider2.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(39, 17)));
			DynamicResource dynamicResource2 = markupExtension2.ProvideValue(xamlServiceProvider2);
			linkButton.SetDynamicResource(VisualElement.StyleProperty, dynamicResource2.Key);
			translate.Text = "ios_Cancel";
			IMarkupExtension markupExtension3 = translate;
			XamlServiceProvider xamlServiceProvider3 = new XamlServiceProvider();
			Type typeFromHandle5 = typeof(IProvideValueTarget);
			object[] array3 = new object[0 + 3];
			array3[0] = linkButton;
			array3[1] = grid;
			array3[2] = this;
			object obj3;
			xamlServiceProvider3.Add(typeFromHandle5, obj3 = new SimpleValueTargetProvider(array3, Button.TextProperty, nameScope));
			xamlServiceProvider3.Add(typeof(IReferenceProvider), obj3);
			Type typeFromHandle6 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver3 = new XmlNamespaceResolver();
			xmlNamespaceResolver3.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver3.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver3.Add("ffimageloading", "clr-namespace:FFImageLoading.Forms;assembly=FFImageLoading.Forms");
			xmlNamespaceResolver3.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver3.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver3.Add("syncfusion", "clr-namespace:Syncfusion.ListView.XForms;assembly=Syncfusion.SfListView.XForms");
			xmlNamespaceResolver3.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider3.Add(typeFromHandle6, new XamlTypeResolver(xmlNamespaceResolver3, typeof(DashboardPageAdder).GetTypeInfo().Assembly));
			xamlServiceProvider3.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(40, 17)));
			object obj4 = markupExtension3.ProvideValue(xamlServiceProvider3);
			linkButton.Text = obj4;
			onPlatform.iOS = true;
			onPlatform.Android = true;
			linkButton.SetValue(VisualElement.IsVisibleProperty, onPlatform);
			grid.Children.Add(linkButton);
			nonScalableLabel.SetValue(Grid.ColumnProperty, 1);
			nonScalableLabel.SetValue(View.HorizontalOptionsProperty, LayoutOptions.Center);
			dynamicResourceExtension3.Key = "NavigationBarLabel";
			IMarkupExtension<DynamicResource> markupExtension4 = dynamicResourceExtension3;
			XamlServiceProvider xamlServiceProvider4 = new XamlServiceProvider();
			Type typeFromHandle7 = typeof(IProvideValueTarget);
			object[] array4 = new object[0 + 3];
			array4[0] = nonScalableLabel;
			array4[1] = grid;
			array4[2] = this;
			object obj5;
			xamlServiceProvider4.Add(typeFromHandle7, obj5 = new SimpleValueTargetProvider(array4, VisualElement.StyleProperty, nameScope));
			xamlServiceProvider4.Add(typeof(IReferenceProvider), obj5);
			Type typeFromHandle8 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver4 = new XmlNamespaceResolver();
			xmlNamespaceResolver4.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver4.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver4.Add("ffimageloading", "clr-namespace:FFImageLoading.Forms;assembly=FFImageLoading.Forms");
			xmlNamespaceResolver4.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver4.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver4.Add("syncfusion", "clr-namespace:Syncfusion.ListView.XForms;assembly=Syncfusion.SfListView.XForms");
			xmlNamespaceResolver4.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider4.Add(typeFromHandle8, new XamlTypeResolver(xmlNamespaceResolver4, typeof(DashboardPageAdder).GetTypeInfo().Assembly));
			xamlServiceProvider4.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(53, 17)));
			DynamicResource dynamicResource3 = markupExtension4.ProvideValue(xamlServiceProvider4);
			nonScalableLabel.SetDynamicResource(VisualElement.StyleProperty, dynamicResource3.Key);
			translate2.Text = "ios_ChooseDashboardPageType";
			IMarkupExtension markupExtension5 = translate2;
			XamlServiceProvider xamlServiceProvider5 = new XamlServiceProvider();
			Type typeFromHandle9 = typeof(IProvideValueTarget);
			object[] array5 = new object[0 + 3];
			array5[0] = nonScalableLabel;
			array5[1] = grid;
			array5[2] = this;
			object obj6;
			xamlServiceProvider5.Add(typeFromHandle9, obj6 = new SimpleValueTargetProvider(array5, Label.TextProperty, nameScope));
			xamlServiceProvider5.Add(typeof(IReferenceProvider), obj6);
			Type typeFromHandle10 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver5 = new XmlNamespaceResolver();
			xmlNamespaceResolver5.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver5.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver5.Add("ffimageloading", "clr-namespace:FFImageLoading.Forms;assembly=FFImageLoading.Forms");
			xmlNamespaceResolver5.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver5.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver5.Add("syncfusion", "clr-namespace:Syncfusion.ListView.XForms;assembly=Syncfusion.SfListView.XForms");
			xmlNamespaceResolver5.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider5.Add(typeFromHandle10, new XamlTypeResolver(xmlNamespaceResolver5, typeof(DashboardPageAdder).GetTypeInfo().Assembly));
			xamlServiceProvider5.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(54, 17)));
			object obj7 = markupExtension5.ProvideValue(xamlServiceProvider5);
			nonScalableLabel.Text = obj7;
			grid.Children.Add(nonScalableLabel);
			linkButton2.SetValue(Grid.ColumnProperty, 2);
			linkButton2.Clicked += this.btnInfo_Clicked;
			linkButton2.SetValue(View.HorizontalOptionsProperty, LayoutOptions.End);
			dynamicResourceExtension4.Key = "InfoImageNavigationBarTextColor";
			IMarkupExtension<DynamicResource> markupExtension6 = dynamicResourceExtension4;
			XamlServiceProvider xamlServiceProvider6 = new XamlServiceProvider();
			Type typeFromHandle11 = typeof(IProvideValueTarget);
			object[] array6 = new object[0 + 3];
			array6[0] = linkButton2;
			array6[1] = grid;
			array6[2] = this;
			object obj8;
			xamlServiceProvider6.Add(typeFromHandle11, obj8 = new SimpleValueTargetProvider(array6, Button.ImageProperty, nameScope));
			xamlServiceProvider6.Add(typeof(IReferenceProvider), obj8);
			Type typeFromHandle12 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver6 = new XmlNamespaceResolver();
			xmlNamespaceResolver6.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver6.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver6.Add("ffimageloading", "clr-namespace:FFImageLoading.Forms;assembly=FFImageLoading.Forms");
			xmlNamespaceResolver6.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver6.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver6.Add("syncfusion", "clr-namespace:Syncfusion.ListView.XForms;assembly=Syncfusion.SfListView.XForms");
			xmlNamespaceResolver6.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider6.Add(typeFromHandle12, new XamlTypeResolver(xmlNamespaceResolver6, typeof(DashboardPageAdder).GetTypeInfo().Assembly));
			xamlServiceProvider6.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(60, 17)));
			DynamicResource dynamicResource4 = markupExtension6.ProvideValue(xamlServiceProvider6);
			linkButton2.SetDynamicResource(Button.ImageProperty, dynamicResource4.Key);
			dynamicResourceExtension5.Key = "NavigationBarButton";
			IMarkupExtension<DynamicResource> markupExtension7 = dynamicResourceExtension5;
			XamlServiceProvider xamlServiceProvider7 = new XamlServiceProvider();
			Type typeFromHandle13 = typeof(IProvideValueTarget);
			object[] array7 = new object[0 + 3];
			array7[0] = linkButton2;
			array7[1] = grid;
			array7[2] = this;
			object obj9;
			xamlServiceProvider7.Add(typeFromHandle13, obj9 = new SimpleValueTargetProvider(array7, VisualElement.StyleProperty, nameScope));
			xamlServiceProvider7.Add(typeof(IReferenceProvider), obj9);
			Type typeFromHandle14 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver7 = new XmlNamespaceResolver();
			xmlNamespaceResolver7.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver7.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver7.Add("ffimageloading", "clr-namespace:FFImageLoading.Forms;assembly=FFImageLoading.Forms");
			xmlNamespaceResolver7.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver7.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver7.Add("syncfusion", "clr-namespace:Syncfusion.ListView.XForms;assembly=Syncfusion.SfListView.XForms");
			xmlNamespaceResolver7.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider7.Add(typeFromHandle14, new XamlTypeResolver(xmlNamespaceResolver7, typeof(DashboardPageAdder).GetTypeInfo().Assembly));
			xamlServiceProvider7.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(61, 17)));
			DynamicResource dynamicResource5 = markupExtension7.ProvideValue(xamlServiceProvider7);
			linkButton2.SetDynamicResource(VisualElement.StyleProperty, dynamicResource5.Key);
			on.Platform = new List<string>(1) { "iOS" };
			on.Value = "0,0,5,0";
			onPlatform2.Platforms.Add(on);
			on2.Platform = new List<string>(1) { "Android" };
			on2.Value = "0,0,5,0";
			onPlatform2.Platforms.Add(on2);
			linkButton2.SetValue(View.MarginProperty, onPlatform2);
			grid.Children.Add(linkButton2);
			this.SetValue(NavigationPage.TitleViewProperty, grid);
			onPlatform3.Android = new Thickness(5.0, 0.0, 5.0, 0.0);
			onPlatform3.iOS = new Thickness(5.0, 0.0, 5.0, 0.0);
			grid6.SetValue(View.MarginProperty, onPlatform3);
			rowDefinition.SetValue(RowDefinition.HeightProperty, new GridLengthTypeConverter().ConvertFromInvariantString("auto"));
			grid2.GetValue(Grid.RowDefinitionsProperty).Add(rowDefinition);
			rowDefinition2.SetValue(RowDefinition.HeightProperty, new GridLengthTypeConverter().ConvertFromInvariantString("*"));
			grid2.GetValue(Grid.RowDefinitionsProperty).Add(rowDefinition2);
			rowDefinition3.SetValue(RowDefinition.HeightProperty, new GridLengthTypeConverter().ConvertFromInvariantString("auto"));
			grid2.GetValue(Grid.RowDefinitionsProperty).Add(rowDefinition3);
			sfListView.SetValue(Grid.RowProperty, 1);
			sfListView.SetValue(SfListView.AutoFitModeProperty, 1);
			dynamicResourceExtension6.Key = "BackgroundColor";
			IMarkupExtension<DynamicResource> markupExtension8 = dynamicResourceExtension6;
			XamlServiceProvider xamlServiceProvider8 = new XamlServiceProvider();
			Type typeFromHandle15 = typeof(IProvideValueTarget);
			object[] array8 = new object[0 + 4];
			array8[0] = sfListView;
			array8[1] = grid2;
			array8[2] = grid6;
			array8[3] = this;
			object obj10;
			xamlServiceProvider8.Add(typeFromHandle15, obj10 = new SimpleValueTargetProvider(array8, VisualElement.BackgroundColorProperty, nameScope));
			xamlServiceProvider8.Add(typeof(IReferenceProvider), obj10);
			Type typeFromHandle16 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver8 = new XmlNamespaceResolver();
			xmlNamespaceResolver8.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver8.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver8.Add("ffimageloading", "clr-namespace:FFImageLoading.Forms;assembly=FFImageLoading.Forms");
			xmlNamespaceResolver8.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver8.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver8.Add("syncfusion", "clr-namespace:Syncfusion.ListView.XForms;assembly=Syncfusion.SfListView.XForms");
			xmlNamespaceResolver8.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider8.Add(typeFromHandle16, new XamlTypeResolver(xmlNamespaceResolver8, typeof(DashboardPageAdder).GetTypeInfo().Assembly));
			xamlServiceProvider8.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(94, 21)));
			DynamicResource dynamicResource6 = markupExtension8.ProvideValue(xamlServiceProvider8);
			sfListView.SetDynamicResource(VisualElement.BackgroundColorProperty, dynamicResource6.Key);
			sfListView.SetValue(SfListView.ItemSizeProperty, 150.0);
			sfListView.ItemTapped += new ItemTappedEventHandler(this.lv_ItemTapped);
			bindingExtension.Source = dashboardPageTypes;
			BindingBase bindingBase = bindingExtension.ProvideValue(null);
			sfListView.SetBinding(SfListView.ItemsSourceProperty, bindingBase);
			dynamicResourceExtension7.Key = "BackgroundColor";
			IMarkupExtension<DynamicResource> markupExtension9 = dynamicResourceExtension7;
			XamlServiceProvider xamlServiceProvider9 = new XamlServiceProvider();
			Type typeFromHandle17 = typeof(IProvideValueTarget);
			object[] array9 = new object[0 + 4];
			array9[0] = sfListView;
			array9[1] = grid2;
			array9[2] = grid6;
			array9[3] = this;
			object obj11;
			xamlServiceProvider9.Add(typeFromHandle17, obj11 = new SimpleValueTargetProvider(array9, SfListView.SelectionBackgroundColorProperty, nameScope));
			xamlServiceProvider9.Add(typeof(IReferenceProvider), obj11);
			Type typeFromHandle18 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver9 = new XmlNamespaceResolver();
			xmlNamespaceResolver9.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver9.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver9.Add("ffimageloading", "clr-namespace:FFImageLoading.Forms;assembly=FFImageLoading.Forms");
			xmlNamespaceResolver9.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver9.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver9.Add("syncfusion", "clr-namespace:Syncfusion.ListView.XForms;assembly=Syncfusion.SfListView.XForms");
			xmlNamespaceResolver9.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider9.Add(typeFromHandle18, new XamlTypeResolver(xmlNamespaceResolver9, typeof(DashboardPageAdder).GetTypeInfo().Assembly));
			xamlServiceProvider9.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(98, 21)));
			DynamicResource dynamicResource7 = markupExtension9.ProvideValue(xamlServiceProvider9);
			sfListView.SetDynamicResource(SfListView.SelectionBackgroundColorProperty, dynamicResource7.Key);
			gridLayout.SetValue(GridLayout.SpanCountProperty, 2);
			sfListView.SetValue(SfListView.LayoutManagerProperty, gridLayout);
			IDataTemplate dataTemplate4 = dataTemplate;
			DashboardPageAdder.<InitializeComponent>_anonXamlCDataTemplate_40 <InitializeComponent>_anonXamlCDataTemplate_ = new DashboardPageAdder.<InitializeComponent>_anonXamlCDataTemplate_40();
			object[] array10 = new object[0 + 5];
			array10[0] = dataTemplate;
			array10[1] = sfListView;
			array10[2] = grid2;
			array10[3] = grid6;
			array10[4] = this;
			<InitializeComponent>_anonXamlCDataTemplate_.parentValues = array10;
			<InitializeComponent>_anonXamlCDataTemplate_.root = this;
			dataTemplate4.LoadTemplate = new Func<object>(<InitializeComponent>_anonXamlCDataTemplate_.LoadDataTemplate);
			sfListView.SetValue(SfListView.ItemTemplateProperty, dataTemplate);
			grid2.Children.Add(sfListView);
			activityFrame.SetValue(Grid.RowProperty, 1);
			activityFrame.SetValue(VisualElement.IsVisibleProperty, new VisualElement.VisibilityConverter().ConvertFromInvariantString("false"));
			translate3.Text = "ios_SavingDashboard";
			IMarkupExtension markupExtension10 = translate3;
			XamlServiceProvider xamlServiceProvider10 = new XamlServiceProvider();
			Type typeFromHandle19 = typeof(IProvideValueTarget);
			object[] array11 = new object[0 + 4];
			array11[0] = activityFrame;
			array11[1] = grid2;
			array11[2] = grid6;
			array11[3] = this;
			object obj12;
			xamlServiceProvider10.Add(typeFromHandle19, obj12 = new SimpleValueTargetProvider(array11, ActivityFrame.TextProperty, nameScope));
			xamlServiceProvider10.Add(typeof(IReferenceProvider), obj12);
			Type typeFromHandle20 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver10 = new XmlNamespaceResolver();
			xmlNamespaceResolver10.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver10.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver10.Add("ffimageloading", "clr-namespace:FFImageLoading.Forms;assembly=FFImageLoading.Forms");
			xmlNamespaceResolver10.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver10.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver10.Add("syncfusion", "clr-namespace:Syncfusion.ListView.XForms;assembly=Syncfusion.SfListView.XForms");
			xmlNamespaceResolver10.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider10.Add(typeFromHandle20, new XamlTypeResolver(xmlNamespaceResolver10, typeof(DashboardPageAdder).GetTypeInfo().Assembly));
			xamlServiceProvider10.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(122, 21)));
			object obj13 = markupExtension10.ProvideValue(xamlServiceProvider10);
			activityFrame.Text = obj13;
			activityFrame.SetValue(View.VerticalOptionsProperty, LayoutOptions.Start);
			grid2.Children.Add(activityFrame);
			grid6.Children.Add(grid2);
			grid5.SetValue(VisualElement.IsVisibleProperty, new VisualElement.VisibilityConverter().ConvertFromInvariantString("False"));
			rowDefinition4.SetValue(RowDefinition.HeightProperty, new GridLengthTypeConverter().ConvertFromInvariantString("auto"));
			grid5.GetValue(Grid.RowDefinitionsProperty).Add(rowDefinition4);
			rowDefinition5.SetValue(RowDefinition.HeightProperty, new GridLengthTypeConverter().ConvertFromInvariantString("0.65*"));
			grid5.GetValue(Grid.RowDefinitionsProperty).Add(rowDefinition5);
			rowDefinition6.SetValue(RowDefinition.HeightProperty, new GridLengthTypeConverter().ConvertFromInvariantString("auto"));
			grid5.GetValue(Grid.RowDefinitionsProperty).Add(rowDefinition6);
			rowDefinition7.SetValue(RowDefinition.HeightProperty, new GridLengthTypeConverter().ConvertFromInvariantString("0.35*"));
			grid5.GetValue(Grid.RowDefinitionsProperty).Add(rowDefinition7);
			rowDefinition8.SetValue(RowDefinition.HeightProperty, new GridLengthTypeConverter().ConvertFromInvariantString("auto"));
			grid5.GetValue(Grid.RowDefinitionsProperty).Add(rowDefinition8);
			grid3.SetValue(Grid.RowProperty, 0);
			columnDefinition4.SetValue(ColumnDefinition.WidthProperty, new GridLengthTypeConverter().ConvertFromInvariantString("auto"));
			grid3.GetValue(Grid.ColumnDefinitionsProperty).Add(columnDefinition4);
			columnDefinition5.SetValue(ColumnDefinition.WidthProperty, new GridLengthTypeConverter().ConvertFromInvariantString("*"));
			grid3.GetValue(Grid.ColumnDefinitionsProperty).Add(columnDefinition5);
			entry.SetValue(Grid.ColumnProperty, 1);
			entry.Completed += this.EntrySearch_Completed;
			entry.SetValue(InputView.IsSpellCheckEnabledProperty, false);
			entry.SetValue(Entry.IsTextPredictionEnabledProperty, false);
			entry.SetValue(Entry.PlaceholderProperty, "\ud83d\udd0e");
			entry.TextChanged += this.entrySearch_TextChanged;
			entry.SetValue(View.VerticalOptionsProperty, LayoutOptions.Center);
			grid3.Children.Add(entry);
			grid5.Children.Add(grid3);
			listView.SetValue(Grid.RowProperty, 1);
			listView.SetValue(ListView.HasUnevenRowsProperty, true);
			listView.ItemSelected += this.LvAvailablePids_ItemSelected;
			bindingExtension2.Path = "FilteredPidList";
			BindingBase bindingBase2 = bindingExtension2.ProvideValue(null);
			listView.SetBinding(ItemsView<Cell>.ItemsSourceProperty, bindingBase2);
			IDataTemplate dataTemplate5 = dataTemplate2;
			DashboardPageAdder.<InitializeComponent>_anonXamlCDataTemplate_41 <InitializeComponent>_anonXamlCDataTemplate_2 = new DashboardPageAdder.<InitializeComponent>_anonXamlCDataTemplate_41();
			object[] array12 = new object[0 + 5];
			array12[0] = dataTemplate2;
			array12[1] = listView;
			array12[2] = grid5;
			array12[3] = grid6;
			array12[4] = this;
			<InitializeComponent>_anonXamlCDataTemplate_2.parentValues = array12;
			<InitializeComponent>_anonXamlCDataTemplate_2.root = this;
			dataTemplate5.LoadTemplate = new Func<object>(<InitializeComponent>_anonXamlCDataTemplate_2.LoadDataTemplate);
			listView.SetValue(ItemsView<Cell>.ItemTemplateProperty, dataTemplate2);
			grid5.Children.Add(listView);
			label.SetValue(Grid.RowProperty, 2);
			label.SetValue(Label.FontAttributesProperty, new FontAttributesConverter().ConvertFromInvariantString("Bold"));
			dynamicResourceExtension8.Key = "GreenTextColor";
			IMarkupExtension<DynamicResource> markupExtension11 = dynamicResourceExtension8;
			XamlServiceProvider xamlServiceProvider11 = new XamlServiceProvider();
			Type typeFromHandle21 = typeof(IProvideValueTarget);
			object[] array13 = new object[0 + 4];
			array13[0] = label;
			array13[1] = grid5;
			array13[2] = grid6;
			array13[3] = this;
			object obj14;
			xamlServiceProvider11.Add(typeFromHandle21, obj14 = new SimpleValueTargetProvider(array13, Label.TextColorProperty, nameScope));
			xamlServiceProvider11.Add(typeof(IReferenceProvider), obj14);
			Type typeFromHandle22 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver11 = new XmlNamespaceResolver();
			xmlNamespaceResolver11.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver11.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver11.Add("ffimageloading", "clr-namespace:FFImageLoading.Forms;assembly=FFImageLoading.Forms");
			xmlNamespaceResolver11.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver11.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver11.Add("syncfusion", "clr-namespace:Syncfusion.ListView.XForms;assembly=Syncfusion.SfListView.XForms");
			xmlNamespaceResolver11.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider11.Add(typeFromHandle22, new XamlTypeResolver(xmlNamespaceResolver11, typeof(DashboardPageAdder).GetTypeInfo().Assembly));
			xamlServiceProvider11.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(187, 21)));
			DynamicResource dynamicResource8 = markupExtension11.ProvideValue(xamlServiceProvider11);
			label.SetDynamicResource(Label.TextColorProperty, dynamicResource8.Key);
			translate4.Text = "ios_SelectedItems";
			IMarkupExtension markupExtension12 = translate4;
			XamlServiceProvider xamlServiceProvider12 = new XamlServiceProvider();
			Type typeFromHandle23 = typeof(IProvideValueTarget);
			object[] array14 = new object[0 + 6];
			array14[0] = span;
			array14[1] = formattedString;
			array14[2] = label;
			array14[3] = grid5;
			array14[4] = grid6;
			array14[5] = this;
			object obj15;
			xamlServiceProvider12.Add(typeFromHandle23, obj15 = new SimpleValueTargetProvider(array14, Span.TextProperty, nameScope));
			xamlServiceProvider12.Add(typeof(IReferenceProvider), obj15);
			Type typeFromHandle24 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver12 = new XmlNamespaceResolver();
			xmlNamespaceResolver12.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver12.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver12.Add("ffimageloading", "clr-namespace:FFImageLoading.Forms;assembly=FFImageLoading.Forms");
			xmlNamespaceResolver12.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver12.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver12.Add("syncfusion", "clr-namespace:Syncfusion.ListView.XForms;assembly=Syncfusion.SfListView.XForms");
			xmlNamespaceResolver12.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider12.Add(typeFromHandle24, new XamlTypeResolver(xmlNamespaceResolver12, typeof(DashboardPageAdder).GetTypeInfo().Assembly));
			xamlServiceProvider12.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(190, 35)));
			object obj16 = markupExtension12.ProvideValue(xamlServiceProvider12);
			span.Text = obj16;
			formattedString.Spans.Add(span);
			span2.SetValue(Span.TextProperty, " ");
			formattedString.Spans.Add(span2);
			bindingExtension3.Path = "SelectedPIDs.Count";
			BindingBase bindingBase3 = bindingExtension3.ProvideValue(null);
			span3.SetBinding(Span.TextProperty, bindingBase3);
			formattedString.Spans.Add(span3);
			span4.SetValue(Span.TextProperty, " / ");
			formattedString.Spans.Add(span4);
			bindingExtension4.Path = "MaxItems";
			BindingBase bindingBase4 = bindingExtension4.ProvideValue(null);
			span5.SetBinding(Span.TextProperty, bindingBase4);
			formattedString.Spans.Add(span5);
			label.SetValue(Label.FormattedTextProperty, formattedString);
			grid5.Children.Add(label);
			listView2.SetValue(Grid.RowProperty, 3);
			listView2.SetValue(ListView.HasUnevenRowsProperty, true);
			listView2.ItemSelected += this.LvSelectedPids_ItemSelected;
			bindingExtension5.Path = "SelectedPIDs";
			BindingBase bindingBase5 = bindingExtension5.ProvideValue(null);
			listView2.SetBinding(ItemsView<Cell>.ItemsSourceProperty, bindingBase5);
			IDataTemplate dataTemplate6 = dataTemplate3;
			DashboardPageAdder.<InitializeComponent>_anonXamlCDataTemplate_42 <InitializeComponent>_anonXamlCDataTemplate_3 = new DashboardPageAdder.<InitializeComponent>_anonXamlCDataTemplate_42();
			object[] array15 = new object[0 + 5];
			array15[0] = dataTemplate3;
			array15[1] = listView2;
			array15[2] = grid5;
			array15[3] = grid6;
			array15[4] = this;
			<InitializeComponent>_anonXamlCDataTemplate_3.parentValues = array15;
			<InitializeComponent>_anonXamlCDataTemplate_3.root = this;
			dataTemplate6.LoadTemplate = new Func<object>(<InitializeComponent>_anonXamlCDataTemplate_3.LoadDataTemplate);
			listView2.SetValue(ItemsView<Cell>.ItemTemplateProperty, dataTemplate3);
			grid5.Children.Add(listView2);
			grid4.SetValue(Grid.RowProperty, 4);
			grid4.SetValue(Grid.ColumnDefinitionsProperty, new ColumnDefinitionCollectionTypeConverter().ConvertFromInvariantString("*,*"));
			button.SetValue(Grid.ColumnProperty, 1);
			button.Clicked += this.btnOK_Clicked;
			staticResourceExtension.Key = "BoolToNegativeConverter";
			IMarkupExtension markupExtension13 = staticResourceExtension;
			XamlServiceProvider xamlServiceProvider13 = new XamlServiceProvider();
			Type typeFromHandle25 = typeof(IProvideValueTarget);
			object[] array16 = new object[0 + 6];
			array16[0] = bindingExtension6;
			array16[1] = button;
			array16[2] = grid4;
			array16[3] = grid5;
			array16[4] = grid6;
			array16[5] = this;
			object obj17;
			xamlServiceProvider13.Add(typeFromHandle25, obj17 = new SimpleValueTargetProvider(array16, typeof(BindingExtension).GetRuntimeProperty("Converter"), nameScope));
			xamlServiceProvider13.Add(typeof(IReferenceProvider), obj17);
			Type typeFromHandle26 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver13 = new XmlNamespaceResolver();
			xmlNamespaceResolver13.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver13.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver13.Add("ffimageloading", "clr-namespace:FFImageLoading.Forms;assembly=FFImageLoading.Forms");
			xmlNamespaceResolver13.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver13.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver13.Add("syncfusion", "clr-namespace:Syncfusion.ListView.XForms;assembly=Syncfusion.SfListView.XForms");
			xmlNamespaceResolver13.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider13.Add(typeFromHandle26, new XamlTypeResolver(xmlNamespaceResolver13, typeof(DashboardPageAdder).GetTypeInfo().Assembly));
			xamlServiceProvider13.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(232, 25)));
			object obj18 = markupExtension13.ProvideValue(xamlServiceProvider13);
			bindingExtension6.Converter = obj18;
			bindingExtension6.Path = "CanAdd";
			BindingBase bindingBase6 = bindingExtension6.ProvideValue(null);
			button.SetBinding(VisualElement.IsEnabledProperty, bindingBase6);
			button.SetValue(Button.TextProperty, "OK");
			grid4.Children.Add(button);
			button2.SetValue(Grid.ColumnProperty, 0);
			button2.Clicked += this.btnSkip_Clicked;
			translate5.Text = "ios_Skip";
			IMarkupExtension markupExtension14 = translate5;
			XamlServiceProvider xamlServiceProvider14 = new XamlServiceProvider();
			Type typeFromHandle27 = typeof(IProvideValueTarget);
			object[] array17 = new object[0 + 5];
			array17[0] = button2;
			array17[1] = grid4;
			array17[2] = grid5;
			array17[3] = grid6;
			array17[4] = this;
			object obj19;
			xamlServiceProvider14.Add(typeFromHandle27, obj19 = new SimpleValueTargetProvider(array17, Button.TextProperty, nameScope));
			xamlServiceProvider14.Add(typeof(IReferenceProvider), obj19);
			Type typeFromHandle28 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver14 = new XmlNamespaceResolver();
			xmlNamespaceResolver14.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver14.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver14.Add("ffimageloading", "clr-namespace:FFImageLoading.Forms;assembly=FFImageLoading.Forms");
			xmlNamespaceResolver14.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver14.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver14.Add("syncfusion", "clr-namespace:Syncfusion.ListView.XForms;assembly=Syncfusion.SfListView.XForms");
			xmlNamespaceResolver14.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider14.Add(typeFromHandle28, new XamlTypeResolver(xmlNamespaceResolver14, typeof(DashboardPageAdder).GetTypeInfo().Assembly));
			xamlServiceProvider14.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(239, 25)));
			object obj20 = markupExtension14.ProvideValue(xamlServiceProvider14);
			button2.Text = obj20;
			grid4.Children.Add(button2);
			grid5.Children.Add(grid4);
			grid6.Children.Add(grid5);
			this.SetValue(ContentPage.ContentProperty, grid6);
		}

		// Token: 0x06000376 RID: 886 RVA: 0x00025174 File Offset: 0x00023374
		private void __InitComponentRuntime()
		{
			Extensions.LoadFromXaml<DashboardPageAdder>(this, typeof(DashboardPageAdder));
			this.gridButtons = NameScopeExtensions.FindByName<Grid>(this, "gridButtons");
			this.btnBack = NameScopeExtensions.FindByName<LinkButton>(this, "btnBack");
			this.lbTitle = NameScopeExtensions.FindByName<NonScalableLabel>(this, "lbTitle");
			this.LayoutRoot = NameScopeExtensions.FindByName<Grid>(this, "LayoutRoot");
			this.panelStep1 = NameScopeExtensions.FindByName<Grid>(this, "panelStep1");
			this.lv = NameScopeExtensions.FindByName<SfListView>(this, "lv");
			this.activityFrame = NameScopeExtensions.FindByName<ActivityFrame>(this, "activityFrame");
			this.panelStep2 = NameScopeExtensions.FindByName<Grid>(this, "panelStep2");
			this.entrySearch = NameScopeExtensions.FindByName<Entry>(this, "entrySearch");
			this.lvAvailablePids = NameScopeExtensions.FindByName<ListView>(this, "lvAvailablePids");
			this.lvSelectedPids = NameScopeExtensions.FindByName<ListView>(this, "lvSelectedPids");
			this.btnOK = NameScopeExtensions.FindByName<Button>(this, "btnOK");
			this.btnSkip = NameScopeExtensions.FindByName<Button>(this, "btnSkip");
		}

		// Token: 0x04000237 RID: 567
		private PageAdderViewModel model;

		// Token: 0x04000238 RID: 568
		private string filtertext = "";

		// Token: 0x04000239 RID: 569
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private Grid gridButtons;

		// Token: 0x0400023A RID: 570
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private LinkButton btnBack;

		// Token: 0x0400023B RID: 571
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private NonScalableLabel lbTitle;

		// Token: 0x0400023C RID: 572
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private Grid LayoutRoot;

		// Token: 0x0400023D RID: 573
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private Grid panelStep1;

		// Token: 0x0400023E RID: 574
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private SfListView lv;

		// Token: 0x0400023F RID: 575
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private ActivityFrame activityFrame;

		// Token: 0x04000240 RID: 576
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private Grid panelStep2;

		// Token: 0x04000241 RID: 577
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private Entry entrySearch;

		// Token: 0x04000242 RID: 578
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private ListView lvAvailablePids;

		// Token: 0x04000243 RID: 579
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private ListView lvSelectedPids;

		// Token: 0x04000244 RID: 580
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private Button btnOK;

		// Token: 0x04000245 RID: 581
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private Button btnSkip;

		// Token: 0x020000AE RID: 174
		[CompilerGenerated]
		private sealed class <>c__DisplayClass15_0
		{
			// Token: 0x06000377 RID: 887 RVA: 0x00002050 File Offset: 0x00000250
			public <>c__DisplayClass15_0()
			{
			}

			// Token: 0x06000378 RID: 888 RVA: 0x0002526F File Offset: 0x0002346F
			internal bool <SetPids>b__0(PID x)
			{
				return x is IPIDFloatValue && !this.other_pages.Contains(x.Id);
			}

			// Token: 0x04000246 RID: 582
			public List<int> other_pages;
		}

		// Token: 0x020000AF RID: 175
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <LvAvailablePids_ItemSelected>d__2 : IAsyncStateMachine
		{
			// Token: 0x06000379 RID: 889 RVA: 0x00025290 File Offset: 0x00023490
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				DashboardPageAdder dashboardPageAdder = this;
				try
				{
					TaskAwaiter<string> taskAwaiter;
					if (num != 0)
					{
						if (dashboardPageAdder.lvAvailablePids.SelectedItem == null)
						{
							goto IL_0146;
						}
						PlatformHelper.CommonService.HideKeyboard();
						pid = dashboardPageAdder.lvAvailablePids.SelectedItem as IPID;
						dashboardPageAdder.lvAvailablePids.SelectedItem = null;
						if (!dashboardPageAdder.model.CanAdd)
						{
							goto IL_0161;
						}
						cancel = Translate.GetString("btnCancel.Content");
						taskAwaiter = dashboardPageAdder.DisplayActionSheetCustom(Translate.GetString("ios_DashboardItemEditor_DisplayType"), cancel, null, StaticLists.DashboardItemTypesList.ToArray()).GetAwaiter();
						if (!taskAwaiter.IsCompleted)
						{
							num2 = 0;
							TaskAwaiter<string> taskAwaiter2 = taskAwaiter;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<string>, DashboardPageAdder.<LvAvailablePids_ItemSelected>d__2>(ref taskAwaiter, ref this);
							return;
						}
					}
					else
					{
						TaskAwaiter<string> taskAwaiter2;
						taskAwaiter = taskAwaiter2;
						taskAwaiter2 = default(TaskAwaiter<string>);
						num2 = -1;
					}
					string result = taskAwaiter.GetResult();
					if (!(result == cancel))
					{
						if (StaticLists.DashboardItemTypesList.Contains(result))
						{
							DashboardItemTypes dashboardItemTypes = (DashboardItemTypes)StaticLists.DashboardItemTypesList.IndexOf(result);
							dashboardPageAdder.model.AddPidToSelected(pid, dashboardItemTypes);
							dashboardPageAdder.lvAvailablePids.SelectedItem = null;
							dashboardPageAdder.UpdatebtnOK();
						}
						pid = null;
						cancel = null;
					}
					IL_0146:;
				}
				catch (Exception ex)
				{
					num2 = -2;
					this.<>t__builder.SetException(ex);
					return;
				}
				IL_0161:
				num2 = -2;
				this.<>t__builder.SetResult();
			}

			// Token: 0x0600037A RID: 890 RVA: 0x00025430 File Offset: 0x00023630
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x04000247 RID: 583
			public int <>1__state;

			// Token: 0x04000248 RID: 584
			public AsyncVoidMethodBuilder <>t__builder;

			// Token: 0x04000249 RID: 585
			public DashboardPageAdder <>4__this;

			// Token: 0x0400024A RID: 586
			private IPID <pid>5__2;

			// Token: 0x0400024B RID: 587
			private string <cancel>5__3;

			// Token: 0x0400024C RID: 588
			private TaskAwaiter<string> <>u__1;
		}

		// Token: 0x020000B0 RID: 176
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <entrySearch_TextChanged>d__6 : IAsyncStateMachine
		{
			// Token: 0x0600037B RID: 891 RVA: 0x00025440 File Offset: 0x00023640
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				DashboardPageAdder dashboardPageAdder = this;
				try
				{
					TaskAwaiter taskAwaiter;
					if (num != 0)
					{
						dashboardPageAdder.filtertext = dashboardPageAdder.entrySearch.Text;
						taskAwaiter = Task.Delay(500).GetAwaiter();
						if (!taskAwaiter.IsCompleted)
						{
							num2 = 0;
							TaskAwaiter taskAwaiter2 = taskAwaiter;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, DashboardPageAdder.<entrySearch_TextChanged>d__6>(ref taskAwaiter, ref this);
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
					if (dashboardPageAdder.filtertext == dashboardPageAdder.entrySearch.Text)
					{
						try
						{
							dashboardPageAdder.model.Filter = dashboardPageAdder.filtertext;
						}
						catch
						{
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

			// Token: 0x0600037C RID: 892 RVA: 0x00025544 File Offset: 0x00023744
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x0400024D RID: 589
			public int <>1__state;

			// Token: 0x0400024E RID: 590
			public AsyncVoidMethodBuilder <>t__builder;

			// Token: 0x0400024F RID: 591
			public DashboardPageAdder <>4__this;

			// Token: 0x04000250 RID: 592
			private TaskAwaiter <>u__1;
		}

		// Token: 0x020000B1 RID: 177
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <lv_ItemTapped>d__12 : IAsyncStateMachine
		{
			// Token: 0x0600037D RID: 893 RVA: 0x00025554 File Offset: 0x00023754
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				DashboardPageAdder dashboardPageAdder = this;
				try
				{
					TaskAwaiter<Page> taskAwaiter;
					if (num != 0)
					{
						DescriptionPage descriptionPage = e.ItemData as DescriptionPage;
						if (descriptionPage.DashboardType != DashboardTypes.Custom)
						{
							dashboardPageAdder.model = new PageAdderViewModel(descriptionPage);
							dashboardPageAdder.panelStep1.IsVisible = false;
							dashboardPageAdder.panelStep2.IsVisible = true;
							dashboardPageAdder.panelStep2.BindingContext = dashboardPageAdder.model;
							dashboardPageAdder.btnOK.IsEnabled = false;
							dashboardPageAdder.btnOK.TextColor = Color.White;
							dashboardPageAdder.btnOK.BackgroundColor = Color.DarkGray;
							dashboardPageAdder.lbTitle.Text = Translate.GetString("ios_ChooseSensors");
							goto IL_014E;
						}
						DashboardListViewModel.Current.Pages.Add(new Dash_CustomPage());
						DashboardListViewModel.Current.SaveDashboardToSettings();
						SharedSettings.Current.DashboardLastPage = DashboardListViewModel.Current.Pages.Count - 1;
						DashboardXamlPage.Instance.ShouldLoadDashboardFromSettings = true;
						taskAwaiter = dashboardPageAdder.Navigation.PopAsync().GetAwaiter();
						if (!taskAwaiter.IsCompleted)
						{
							num2 = 0;
							TaskAwaiter<Page> taskAwaiter2 = taskAwaiter;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<Page>, DashboardPageAdder.<lv_ItemTapped>d__12>(ref taskAwaiter, ref this);
							return;
						}
					}
					else
					{
						TaskAwaiter<Page> taskAwaiter2;
						taskAwaiter = taskAwaiter2;
						taskAwaiter2 = default(TaskAwaiter<Page>);
						num2 = -1;
					}
					taskAwaiter.GetResult();
					IL_014E:;
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

			// Token: 0x0600037E RID: 894 RVA: 0x000256FC File Offset: 0x000238FC
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x04000251 RID: 593
			public int <>1__state;

			// Token: 0x04000252 RID: 594
			public AsyncVoidMethodBuilder <>t__builder;

			// Token: 0x04000253 RID: 595
			public ItemTappedEventArgs e;

			// Token: 0x04000254 RID: 596
			public DashboardPageAdder <>4__this;

			// Token: 0x04000255 RID: 597
			private TaskAwaiter<Page> <>u__1;
		}

		// Token: 0x020000B2 RID: 178
		[CompilerGenerated]
		private sealed class <InitializeComponent>_anonXamlCDataTemplate_40
		{
			// Token: 0x0600037F RID: 895 RVA: 0x0002570C File Offset: 0x0002390C
			public <InitializeComponent>_anonXamlCDataTemplate_40()
			{
			}

			// Token: 0x06000380 RID: 896 RVA: 0x00025720 File Offset: 0x00023920
			internal object LoadDataTemplate()
			{
				BindingExtension bindingExtension;
				VisualDiagnostics.RegisterSourceInfo(bindingExtension = new BindingExtension(), new Uri("Pages\\Dashboard\\DashboardPageAdder.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 112, 41);
				CachedImage cachedImage;
				VisualDiagnostics.RegisterSourceInfo(cachedImage = new CachedImage(), new Uri("Pages\\Dashboard\\DashboardPageAdder.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 106, 38);
				ContentView contentView;
				VisualDiagnostics.RegisterSourceInfo(contentView = new ContentView(), new Uri("Pages\\Dashboard\\DashboardPageAdder.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 105, 34);
				ViewCell viewCell;
				VisualDiagnostics.RegisterSourceInfo(viewCell = new ViewCell(), new Uri("Pages\\Dashboard\\DashboardPageAdder.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 104, 30);
				NameScope nameScope = new NameScope();
				NameScope.SetNameScope(viewCell, nameScope);
				cachedImage.SetValue(View.MarginProperty, new Thickness(10.0));
				cachedImage.SetValue(CachedImage.DownsampleToViewSizeProperty, true);
				cachedImage.SetValue(VisualElement.HeightRequestProperty, 150.0);
				cachedImage.SetValue(View.HorizontalOptionsProperty, LayoutOptions.Center);
				cachedImage.SetValue(VisualElement.MinimumHeightRequestProperty, 150.0);
				bindingExtension.Path = "PreviewFile";
				BindingBase bindingBase = bindingExtension.ProvideValue(null);
				cachedImage.SetBinding(CachedImage.SourceProperty, bindingBase);
				contentView.SetValue(ContentView.ContentProperty, cachedImage);
				viewCell.View = contentView;
				return viewCell;
			}

			// Token: 0x04000256 RID: 598
			internal object[] parentValues;

			// Token: 0x04000257 RID: 599
			internal DashboardPageAdder root;
		}

		// Token: 0x020000B3 RID: 179
		[CompilerGenerated]
		private sealed class <InitializeComponent>_anonXamlCDataTemplate_41
		{
			// Token: 0x06000381 RID: 897 RVA: 0x00025890 File Offset: 0x00023A90
			public <InitializeComponent>_anonXamlCDataTemplate_41()
			{
			}

			// Token: 0x06000382 RID: 898 RVA: 0x000258A4 File Offset: 0x00023AA4
			internal object LoadDataTemplate()
			{
				ColumnDefinition columnDefinition;
				VisualDiagnostics.RegisterSourceInfo(columnDefinition = new ColumnDefinition(), new Uri("Pages\\Dashboard\\DashboardPageAdder.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 166, 42);
				ColumnDefinition columnDefinition2;
				VisualDiagnostics.RegisterSourceInfo(columnDefinition2 = new ColumnDefinition(), new Uri("Pages\\Dashboard\\DashboardPageAdder.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 167, 42);
				DynamicResourceExtension dynamicResourceExtension;
				VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension = new DynamicResourceExtension(), new Uri("Pages\\Dashboard\\DashboardPageAdder.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 171, 41);
				BindingExtension bindingExtension;
				VisualDiagnostics.RegisterSourceInfo(bindingExtension = new BindingExtension(), new Uri("Pages\\Dashboard\\DashboardPageAdder.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 172, 41);
				Label label;
				VisualDiagnostics.RegisterSourceInfo(label = new Label(), new Uri("Pages\\Dashboard\\DashboardPageAdder.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 169, 38);
				DynamicResourceExtension dynamicResourceExtension2;
				VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension2 = new DynamicResourceExtension(), new Uri("Pages\\Dashboard\\DashboardPageAdder.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 175, 41);
				Label label2;
				VisualDiagnostics.RegisterSourceInfo(label2 = new Label(), new Uri("Pages\\Dashboard\\DashboardPageAdder.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 173, 38);
				Grid grid;
				VisualDiagnostics.RegisterSourceInfo(grid = new Grid(), new Uri("Pages\\Dashboard\\DashboardPageAdder.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 164, 34);
				ViewCell viewCell;
				VisualDiagnostics.RegisterSourceInfo(viewCell = new ViewCell(), new Uri("Pages\\Dashboard\\DashboardPageAdder.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 163, 30);
				NameScope nameScope = new NameScope();
				NameScope.SetNameScope(viewCell, nameScope);
				grid.SetValue(View.MarginProperty, new Thickness(0.0, 5.0, 0.0, 5.0));
				columnDefinition.SetValue(ColumnDefinition.WidthProperty, new GridLengthTypeConverter().ConvertFromInvariantString("*"));
				grid.GetValue(Grid.ColumnDefinitionsProperty).Add(columnDefinition);
				columnDefinition2.SetValue(ColumnDefinition.WidthProperty, new GridLengthTypeConverter().ConvertFromInvariantString("auto"));
				grid.GetValue(Grid.ColumnDefinitionsProperty).Add(columnDefinition2);
				label.SetValue(Grid.ColumnProperty, 0);
				dynamicResourceExtension.Key = "BaseFontSize++";
				IMarkupExtension<DynamicResource> markupExtension = dynamicResourceExtension;
				XamlServiceProvider xamlServiceProvider = new XamlServiceProvider();
				Type typeFromHandle = typeof(IProvideValueTarget);
				int num;
				object[] array = new object[(num = this.parentValues.Length) + 3];
				Array.Copy(this.parentValues, 0, array, 3, num);
				object[] array2 = array;
				array2[0] = label;
				array2[1] = grid;
				array2[2] = viewCell;
				object obj;
				xamlServiceProvider.Add(typeFromHandle, obj = new SimpleValueTargetProvider(array2, Label.FontSizeProperty, nameScope));
				xamlServiceProvider.Add(typeof(IReferenceProvider), obj);
				Type typeFromHandle2 = typeof(IXamlTypeResolver);
				XmlNamespaceResolver xmlNamespaceResolver = new XmlNamespaceResolver();
				xmlNamespaceResolver.Add("", "http://xamarin.com/schemas/2014/forms");
				xmlNamespaceResolver.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
				xmlNamespaceResolver.Add("ffimageloading", "clr-namespace:FFImageLoading.Forms;assembly=FFImageLoading.Forms");
				xmlNamespaceResolver.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
				xmlNamespaceResolver.Add("local", "clr-namespace:CarScannerXamarinForms");
				xmlNamespaceResolver.Add("syncfusion", "clr-namespace:Syncfusion.ListView.XForms;assembly=Syncfusion.SfListView.XForms");
				xmlNamespaceResolver.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
				xamlServiceProvider.Add(typeFromHandle2, new XamlTypeResolver(xmlNamespaceResolver, typeof(DashboardPageAdder.<InitializeComponent>_anonXamlCDataTemplate_41).GetTypeInfo().Assembly));
				xamlServiceProvider.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(171, 41)));
				DynamicResource dynamicResource = markupExtension.ProvideValue(xamlServiceProvider);
				label.SetDynamicResource(Label.FontSizeProperty, dynamicResource.Key);
				bindingExtension.Path = "Name";
				BindingBase bindingBase = bindingExtension.ProvideValue(null);
				label.SetBinding(Label.TextProperty, bindingBase);
				grid.Children.Add(label);
				label2.SetValue(Grid.ColumnProperty, 1);
				dynamicResourceExtension2.Key = "BaseFontSize+";
				IMarkupExtension<DynamicResource> markupExtension2 = dynamicResourceExtension2;
				XamlServiceProvider xamlServiceProvider2 = new XamlServiceProvider();
				Type typeFromHandle3 = typeof(IProvideValueTarget);
				int num2;
				object[] array3 = new object[(num2 = this.parentValues.Length) + 3];
				Array.Copy(this.parentValues, 0, array3, 3, num2);
				object[] array4 = array3;
				array4[0] = label2;
				array4[1] = grid;
				array4[2] = viewCell;
				object obj2;
				xamlServiceProvider2.Add(typeFromHandle3, obj2 = new SimpleValueTargetProvider(array4, Label.FontSizeProperty, nameScope));
				xamlServiceProvider2.Add(typeof(IReferenceProvider), obj2);
				Type typeFromHandle4 = typeof(IXamlTypeResolver);
				XmlNamespaceResolver xmlNamespaceResolver2 = new XmlNamespaceResolver();
				xmlNamespaceResolver2.Add("", "http://xamarin.com/schemas/2014/forms");
				xmlNamespaceResolver2.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
				xmlNamespaceResolver2.Add("ffimageloading", "clr-namespace:FFImageLoading.Forms;assembly=FFImageLoading.Forms");
				xmlNamespaceResolver2.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
				xmlNamespaceResolver2.Add("local", "clr-namespace:CarScannerXamarinForms");
				xmlNamespaceResolver2.Add("syncfusion", "clr-namespace:Syncfusion.ListView.XForms;assembly=Syncfusion.SfListView.XForms");
				xmlNamespaceResolver2.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
				xamlServiceProvider2.Add(typeFromHandle4, new XamlTypeResolver(xmlNamespaceResolver2, typeof(DashboardPageAdder.<InitializeComponent>_anonXamlCDataTemplate_41).GetTypeInfo().Assembly));
				xamlServiceProvider2.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(175, 41)));
				DynamicResource dynamicResource2 = markupExtension2.ProvideValue(xamlServiceProvider2);
				label2.SetDynamicResource(Label.FontSizeProperty, dynamicResource2.Key);
				label2.SetValue(Label.TextProperty, "➕");
				label2.SetValue(Label.TextColorProperty, Color.DarkGreen);
				grid.Children.Add(label2);
				viewCell.View = grid;
				return viewCell;
			}

			// Token: 0x04000258 RID: 600
			internal object[] parentValues;

			// Token: 0x04000259 RID: 601
			internal DashboardPageAdder root;
		}

		// Token: 0x020000B4 RID: 180
		[CompilerGenerated]
		private sealed class <InitializeComponent>_anonXamlCDataTemplate_42
		{
			// Token: 0x06000383 RID: 899 RVA: 0x00025E28 File Offset: 0x00024028
			public <InitializeComponent>_anonXamlCDataTemplate_42()
			{
			}

			// Token: 0x06000384 RID: 900 RVA: 0x00025E3C File Offset: 0x0002403C
			internal object LoadDataTemplate()
			{
				ColumnDefinition columnDefinition;
				VisualDiagnostics.RegisterSourceInfo(columnDefinition = new ColumnDefinition(), new Uri("Pages\\Dashboard\\DashboardPageAdder.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 209, 42);
				ColumnDefinition columnDefinition2;
				VisualDiagnostics.RegisterSourceInfo(columnDefinition2 = new ColumnDefinition(), new Uri("Pages\\Dashboard\\DashboardPageAdder.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 210, 42);
				DynamicResourceExtension dynamicResourceExtension;
				VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension = new DynamicResourceExtension(), new Uri("Pages\\Dashboard\\DashboardPageAdder.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 214, 41);
				BindingExtension bindingExtension;
				VisualDiagnostics.RegisterSourceInfo(bindingExtension = new BindingExtension(), new Uri("Pages\\Dashboard\\DashboardPageAdder.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 215, 41);
				Label label;
				VisualDiagnostics.RegisterSourceInfo(label = new Label(), new Uri("Pages\\Dashboard\\DashboardPageAdder.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 212, 38);
				DynamicResourceExtension dynamicResourceExtension2;
				VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension2 = new DynamicResourceExtension(), new Uri("Pages\\Dashboard\\DashboardPageAdder.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 218, 41);
				Label label2;
				VisualDiagnostics.RegisterSourceInfo(label2 = new Label(), new Uri("Pages\\Dashboard\\DashboardPageAdder.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 216, 38);
				Grid grid;
				VisualDiagnostics.RegisterSourceInfo(grid = new Grid(), new Uri("Pages\\Dashboard\\DashboardPageAdder.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 207, 34);
				ViewCell viewCell;
				VisualDiagnostics.RegisterSourceInfo(viewCell = new ViewCell(), new Uri("Pages\\Dashboard\\DashboardPageAdder.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 206, 30);
				NameScope nameScope = new NameScope();
				NameScope.SetNameScope(viewCell, nameScope);
				grid.SetValue(View.MarginProperty, new Thickness(0.0, 5.0));
				columnDefinition.SetValue(ColumnDefinition.WidthProperty, new GridLengthTypeConverter().ConvertFromInvariantString("*"));
				grid.GetValue(Grid.ColumnDefinitionsProperty).Add(columnDefinition);
				columnDefinition2.SetValue(ColumnDefinition.WidthProperty, new GridLengthTypeConverter().ConvertFromInvariantString("auto"));
				grid.GetValue(Grid.ColumnDefinitionsProperty).Add(columnDefinition2);
				label.SetValue(Grid.ColumnProperty, 0);
				dynamicResourceExtension.Key = "BaseFontSize++";
				IMarkupExtension<DynamicResource> markupExtension = dynamicResourceExtension;
				XamlServiceProvider xamlServiceProvider = new XamlServiceProvider();
				Type typeFromHandle = typeof(IProvideValueTarget);
				int num;
				object[] array = new object[(num = this.parentValues.Length) + 3];
				Array.Copy(this.parentValues, 0, array, 3, num);
				object[] array2 = array;
				array2[0] = label;
				array2[1] = grid;
				array2[2] = viewCell;
				object obj;
				xamlServiceProvider.Add(typeFromHandle, obj = new SimpleValueTargetProvider(array2, Label.FontSizeProperty, nameScope));
				xamlServiceProvider.Add(typeof(IReferenceProvider), obj);
				Type typeFromHandle2 = typeof(IXamlTypeResolver);
				XmlNamespaceResolver xmlNamespaceResolver = new XmlNamespaceResolver();
				xmlNamespaceResolver.Add("", "http://xamarin.com/schemas/2014/forms");
				xmlNamespaceResolver.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
				xmlNamespaceResolver.Add("ffimageloading", "clr-namespace:FFImageLoading.Forms;assembly=FFImageLoading.Forms");
				xmlNamespaceResolver.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
				xmlNamespaceResolver.Add("local", "clr-namespace:CarScannerXamarinForms");
				xmlNamespaceResolver.Add("syncfusion", "clr-namespace:Syncfusion.ListView.XForms;assembly=Syncfusion.SfListView.XForms");
				xmlNamespaceResolver.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
				xamlServiceProvider.Add(typeFromHandle2, new XamlTypeResolver(xmlNamespaceResolver, typeof(DashboardPageAdder.<InitializeComponent>_anonXamlCDataTemplate_42).GetTypeInfo().Assembly));
				xamlServiceProvider.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(214, 41)));
				DynamicResource dynamicResource = markupExtension.ProvideValue(xamlServiceProvider);
				label.SetDynamicResource(Label.FontSizeProperty, dynamicResource.Key);
				bindingExtension.Path = "Name";
				BindingBase bindingBase = bindingExtension.ProvideValue(null);
				label.SetBinding(Label.TextProperty, bindingBase);
				grid.Children.Add(label);
				label2.SetValue(Grid.ColumnProperty, 1);
				dynamicResourceExtension2.Key = "BaseFontSize+";
				IMarkupExtension<DynamicResource> markupExtension2 = dynamicResourceExtension2;
				XamlServiceProvider xamlServiceProvider2 = new XamlServiceProvider();
				Type typeFromHandle3 = typeof(IProvideValueTarget);
				int num2;
				object[] array3 = new object[(num2 = this.parentValues.Length) + 3];
				Array.Copy(this.parentValues, 0, array3, 3, num2);
				object[] array4 = array3;
				array4[0] = label2;
				array4[1] = grid;
				array4[2] = viewCell;
				object obj2;
				xamlServiceProvider2.Add(typeFromHandle3, obj2 = new SimpleValueTargetProvider(array4, Label.FontSizeProperty, nameScope));
				xamlServiceProvider2.Add(typeof(IReferenceProvider), obj2);
				Type typeFromHandle4 = typeof(IXamlTypeResolver);
				XmlNamespaceResolver xmlNamespaceResolver2 = new XmlNamespaceResolver();
				xmlNamespaceResolver2.Add("", "http://xamarin.com/schemas/2014/forms");
				xmlNamespaceResolver2.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
				xmlNamespaceResolver2.Add("ffimageloading", "clr-namespace:FFImageLoading.Forms;assembly=FFImageLoading.Forms");
				xmlNamespaceResolver2.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
				xmlNamespaceResolver2.Add("local", "clr-namespace:CarScannerXamarinForms");
				xmlNamespaceResolver2.Add("syncfusion", "clr-namespace:Syncfusion.ListView.XForms;assembly=Syncfusion.SfListView.XForms");
				xmlNamespaceResolver2.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
				xamlServiceProvider2.Add(typeFromHandle4, new XamlTypeResolver(xmlNamespaceResolver2, typeof(DashboardPageAdder.<InitializeComponent>_anonXamlCDataTemplate_42).GetTypeInfo().Assembly));
				xamlServiceProvider2.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(218, 41)));
				DynamicResource dynamicResource2 = markupExtension2.ProvideValue(xamlServiceProvider2);
				label2.SetDynamicResource(Label.FontSizeProperty, dynamicResource2.Key);
				label2.SetValue(Label.TextProperty, "➖");
				label2.SetValue(Label.TextColorProperty, Color.Red);
				grid.Children.Add(label2);
				viewCell.View = grid;
				return viewCell;
			}

			// Token: 0x0400025A RID: 602
			internal object[] parentValues;

			// Token: 0x0400025B RID: 603
			internal DashboardPageAdder root;
		}
	}
}
