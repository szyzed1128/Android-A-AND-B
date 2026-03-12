using System;
using System.CodeDom.Compiler;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using CarScannerXamarinForms.Common.XAMLConverters;
using CarScannerXamarinForms.DTC;
using CarScannerXamarinForms.ECUModels;
using CarScannerXamarinForms.OBD2;
using CarScannerXamarinForms.Settings;
using CarScannerXamarinForms.UserControls;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Internals;
using Xamarin.Forms.PlatformConfiguration.iOSSpecific;
using Xamarin.Forms.Xaml;
using Xamarin.Forms.Xaml.Diagnostics;
using Xamarin.Forms.Xaml.Internals;

namespace CarScannerXamarinForms.DTCv2
{
	// Token: 0x020005A2 RID: 1442
	[XamlCompilation(2)]
	[XamlFilePath("DTCv2\\DTCv2Page.xaml")]
	public class DTCv2Page : ContentPage, IDTCvXPage
	{
		// Token: 0x06003473 RID: 13427 RVA: 0x0024A77C File Offset: 0x0024897C
		public DTCv2Page()
		{
			this.InitializeComponent();
			if (SharedSettings.Current.AdsProductPurchased)
			{
				this.ad.IsVisible = false;
			}
			else
			{
				this.ad.IsVisible = true;
			}
			if (App.OBDReader.CurrentELMFormat == ELMFormat.KWP)
			{
				this.btnToOldStyle.IsVisible = true;
				return;
			}
			this.btnToOldStyle.IsVisible = SharedSettings.Current.ShowExperimental;
		}

		// Token: 0x06003474 RID: 13428 RVA: 0x0024A7EC File Offset: 0x002489EC
		public async Task LoadModel()
		{
			this.Model = new DTCv2Model();
			base.BindingContext = this.Model;
		}

		// Token: 0x06003475 RID: 13429 RVA: 0x0024A82F File Offset: 0x00248A2F
		private void LvECUs_ItemSelected(object sender, SelectedItemChangedEventArgs e)
		{
			if (this.lvECUs.SelectedItem == null)
			{
				return;
			}
			IECU iecu = this.lvECUs.SelectedItem as IECU;
			iecu.IsSelected = !iecu.IsSelected;
			this.lvECUs.SelectedItem = null;
		}

		// Token: 0x06003476 RID: 13430 RVA: 0x0024A869 File Offset: 0x00248A69
		private void LvDTCs_ItemSelected(object sender, SelectedItemChangedEventArgs e)
		{
			if (this.lvDTCs.SelectedItem == null)
			{
				return;
			}
			DTCItemV2 dtcitemV = this.lvDTCs.SelectedItem as DTCItemV2;
			this.lvDTCs.SelectedItem = null;
			DTCWorker.GoogleForDTC(dtcitemV.Code);
		}

		// Token: 0x06003477 RID: 13431 RVA: 0x0024A89F File Offset: 0x00248A9F
		private void btnExpand_Clicked(object sender, EventArgs e)
		{
			IECU iecu = (sender as VisualElement).BindingContext as IECU;
			iecu.Expanded = !iecu.Expanded;
		}

		// Token: 0x06003478 RID: 13432 RVA: 0x0024A8BF File Offset: 0x00248ABF
		protected override bool OnBackButtonPressed()
		{
			this.Model.BackButtonCommand.Execute(null);
			return true;
		}

		// Token: 0x06003479 RID: 13433 RVA: 0x0024A8D4 File Offset: 0x00248AD4
		private async void BtnClearECUDTC_Clicked(object sender, EventArgs e)
		{
			if (!this.Model.IsBusy)
			{
				IECU iecu = (sender as VisualElement).BindingContext as IECU;
				await this.Model.ClearDTCForOneECU(iecu);
			}
		}

		// Token: 0x0600347A RID: 13434 RVA: 0x0024A913 File Offset: 0x00248B13
		private void LabelBadDTCTapGestureRecognizer_Tapped(object sender, EventArgs e)
		{
			Launcher.TryOpenAsync("https://www.carscanner.info/choosing-obdii-adapter/");
		}

		// Token: 0x0600347B RID: 13435 RVA: 0x0024A920 File Offset: 0x00248B20
		private async void btnToOldStyle_Clicked(object sender, EventArgs e)
		{
			await App.OBDReader.DebugWrite("\r\n[USER_SELECTED_OLD_STYLE_DTC]\r\n");
			this.btnToOldStyle.IsEnabled = false;
			DTCWizardStartPage dtcwizardStartPage = new DTCWizardStartPage();
			await base.Navigation.PushAsync(dtcwizardStartPage);
			this.btnToOldStyle.IsEnabled = true;
		}

		// Token: 0x0600347C RID: 13436 RVA: 0x0024A958 File Offset: 0x00248B58
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private void InitializeComponent()
		{
			if (ResourceLoader.IsEnabled && ResourceLoader.CanProvideContentFor(new ResourceLoader.ResourceLoadingQuery
			{
				AssemblyName = typeof(DTCv2Page).GetTypeInfo().Assembly.GetName(),
				ResourcePath = "DTCv2/DTCv2Page.xaml",
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
			VisualDiagnostics.RegisterSourceInfo(translate = new Translate(), new Uri("DTCv2\\DTCv2Page.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 14, 5);
			DynamicResourceExtension dynamicResourceExtension;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension = new DynamicResourceExtension(), new Uri("DTCv2\\DTCv2Page.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 17, 5);
			DTCStatusCollectionToStringConverter dtcstatusCollectionToStringConverter;
			VisualDiagnostics.RegisterSourceInfo(dtcstatusCollectionToStringConverter = new DTCStatusCollectionToStringConverter(), new Uri("DTCv2\\DTCv2Page.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 22, 14);
			DTCDescriptionCollectionToStringConverter dtcdescriptionCollectionToStringConverter;
			VisualDiagnostics.RegisterSourceInfo(dtcdescriptionCollectionToStringConverter = new DTCDescriptionCollectionToStringConverter(), new Uri("DTCv2\\DTCv2Page.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 23, 14);
			DTCCodeToStringConverter dtccodeToStringConverter;
			VisualDiagnostics.RegisterSourceInfo(dtccodeToStringConverter = new DTCCodeToStringConverter(), new Uri("DTCv2\\DTCv2Page.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 24, 14);
			EmptyStringToFalseConverter emptyStringToFalseConverter;
			VisualDiagnostics.RegisterSourceInfo(emptyStringToFalseConverter = new EmptyStringToFalseConverter(), new Uri("DTCv2\\DTCv2Page.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 25, 14);
			BoolToNegativeConverter boolToNegativeConverter;
			VisualDiagnostics.RegisterSourceInfo(boolToNegativeConverter = new BoolToNegativeConverter(), new Uri("DTCv2\\DTCv2Page.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 26, 14);
			TrueToRedColorConverter trueToRedColorConverter;
			VisualDiagnostics.RegisterSourceInfo(trueToRedColorConverter = new TrueToRedColorConverter(), new Uri("DTCv2\\DTCv2Page.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 27, 14);
			ResourceDictionary resourceDictionary;
			VisualDiagnostics.RegisterSourceInfo(resourceDictionary = new ResourceDictionary(), new Uri("DTCv2\\DTCv2Page.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 21, 10);
			ColumnDefinition columnDefinition;
			VisualDiagnostics.RegisterSourceInfo(columnDefinition = new ColumnDefinition(), new Uri("DTCv2\\DTCv2Page.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 33, 18);
			ColumnDefinition columnDefinition2;
			VisualDiagnostics.RegisterSourceInfo(columnDefinition2 = new ColumnDefinition(), new Uri("DTCv2\\DTCv2Page.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 35, 18);
			ColumnDefinition columnDefinition3;
			VisualDiagnostics.RegisterSourceInfo(columnDefinition3 = new ColumnDefinition(), new Uri("DTCv2\\DTCv2Page.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 37, 18);
			ColumnDefinition columnDefinition4;
			VisualDiagnostics.RegisterSourceInfo(columnDefinition4 = new ColumnDefinition(), new Uri("DTCv2\\DTCv2Page.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 38, 18);
			BindingExtension bindingExtension;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension = new BindingExtension(), new Uri("DTCv2\\DTCv2Page.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 42, 17);
			DynamicResourceExtension dynamicResourceExtension2;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension2 = new DynamicResourceExtension(), new Uri("DTCv2\\DTCv2Page.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 44, 17);
			Translate translate2;
			VisualDiagnostics.RegisterSourceInfo(translate2 = new Translate(), new Uri("DTCv2\\DTCv2Page.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 45, 17);
			LinkButton linkButton;
			VisualDiagnostics.RegisterSourceInfo(linkButton = new LinkButton(), new Uri("DTCv2\\DTCv2Page.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 40, 14);
			DynamicResourceExtension dynamicResourceExtension3;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension3 = new DynamicResourceExtension(), new Uri("DTCv2\\DTCv2Page.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 51, 17);
			Translate translate3;
			VisualDiagnostics.RegisterSourceInfo(translate3 = new Translate(), new Uri("DTCv2\\DTCv2Page.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 52, 17);
			NonScalableLabel nonScalableLabel;
			VisualDiagnostics.RegisterSourceInfo(nonScalableLabel = new NonScalableLabel(), new Uri("DTCv2\\DTCv2Page.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 46, 14);
			BindingExtension bindingExtension2;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension2 = new BindingExtension(), new Uri("DTCv2\\DTCv2Page.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 59, 17);
			DynamicResourceExtension dynamicResourceExtension4;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension4 = new DynamicResourceExtension(), new Uri("DTCv2\\DTCv2Page.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 60, 17);
			BindingExtension bindingExtension3;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension3 = new BindingExtension(), new Uri("DTCv2\\DTCv2Page.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 62, 43);
			TapGestureRecognizer tapGestureRecognizer;
			VisualDiagnostics.RegisterSourceInfo(tapGestureRecognizer = new TapGestureRecognizer(), new Uri("DTCv2\\DTCv2Page.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 62, 22);
			Image image;
			VisualDiagnostics.RegisterSourceInfo(image = new Image(), new Uri("DTCv2\\DTCv2Page.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 54, 14);
			BindingExtension bindingExtension4;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension4 = new BindingExtension(), new Uri("DTCv2\\DTCv2Page.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 70, 17);
			DynamicResourceExtension dynamicResourceExtension5;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension5 = new DynamicResourceExtension(), new Uri("DTCv2\\DTCv2Page.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 71, 17);
			BindingExtension bindingExtension5;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension5 = new BindingExtension(), new Uri("DTCv2\\DTCv2Page.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 73, 43);
			TapGestureRecognizer tapGestureRecognizer2;
			VisualDiagnostics.RegisterSourceInfo(tapGestureRecognizer2 = new TapGestureRecognizer(), new Uri("DTCv2\\DTCv2Page.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 73, 22);
			Image image2;
			VisualDiagnostics.RegisterSourceInfo(image2 = new Image(), new Uri("DTCv2\\DTCv2Page.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 65, 14);
			BindingExtension bindingExtension6;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension6 = new BindingExtension(), new Uri("DTCv2\\DTCv2Page.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 79, 17);
			DynamicResourceExtension dynamicResourceExtension6;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension6 = new DynamicResourceExtension(), new Uri("DTCv2\\DTCv2Page.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 80, 17);
			BindingExtension bindingExtension7;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension7 = new BindingExtension(), new Uri("DTCv2\\DTCv2Page.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 82, 43);
			TapGestureRecognizer tapGestureRecognizer3;
			VisualDiagnostics.RegisterSourceInfo(tapGestureRecognizer3 = new TapGestureRecognizer(), new Uri("DTCv2\\DTCv2Page.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 82, 22);
			Image image3;
			VisualDiagnostics.RegisterSourceInfo(image3 = new Image(), new Uri("DTCv2\\DTCv2Page.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 76, 14);
			Grid grid;
			VisualDiagnostics.RegisterSourceInfo(grid = new Grid(), new Uri("DTCv2\\DTCv2Page.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 31, 10);
			RowDefinition rowDefinition;
			VisualDiagnostics.RegisterSourceInfo(rowDefinition = new RowDefinition(), new Uri("DTCv2\\DTCv2Page.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 93, 18);
			RowDefinition rowDefinition2;
			VisualDiagnostics.RegisterSourceInfo(rowDefinition2 = new RowDefinition(), new Uri("DTCv2\\DTCv2Page.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 94, 18);
			BindingExtension bindingExtension8;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension8 = new BindingExtension(), new Uri("DTCv2\\DTCv2Page.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 99, 17);
			RowDefinition rowDefinition3;
			VisualDiagnostics.RegisterSourceInfo(rowDefinition3 = new RowDefinition(), new Uri("DTCv2\\DTCv2Page.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 101, 22);
			RowDefinition rowDefinition4;
			VisualDiagnostics.RegisterSourceInfo(rowDefinition4 = new RowDefinition(), new Uri("DTCv2\\DTCv2Page.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 103, 22);
			RowDefinition rowDefinition5;
			VisualDiagnostics.RegisterSourceInfo(rowDefinition5 = new RowDefinition(), new Uri("DTCv2\\DTCv2Page.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 105, 22);
			RowDefinition rowDefinition6;
			VisualDiagnostics.RegisterSourceInfo(rowDefinition6 = new RowDefinition(), new Uri("DTCv2\\DTCv2Page.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 107, 22);
			BindingExtension bindingExtension9;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension9 = new BindingExtension(), new Uri("DTCv2\\DTCv2Page.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 119, 21);
			DataTemplate dataTemplate;
			VisualDiagnostics.RegisterSourceInfo(dataTemplate = new DataTemplate(), new Uri("DTCv2\\DTCv2Page.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 121, 26);
			DynamicResourceExtension dynamicResourceExtension7;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension7 = new DynamicResourceExtension(), new Uri("DTCv2\\DTCv2Page.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 135, 33);
			Translate translate4;
			VisualDiagnostics.RegisterSourceInfo(translate4 = new Translate(), new Uri("DTCv2\\DTCv2Page.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 138, 33);
			Label label;
			VisualDiagnostics.RegisterSourceInfo(label = new Label(), new Uri("DTCv2\\DTCv2Page.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 133, 30);
			DynamicResourceExtension dynamicResourceExtension8;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension8 = new DynamicResourceExtension(), new Uri("DTCv2\\DTCv2Page.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 141, 33);
			StaticResourceExtension staticResourceExtension;
			VisualDiagnostics.RegisterSourceInfo(staticResourceExtension = new StaticResourceExtension(), new Uri("DTCv2\\DTCv2Page.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 144, 33);
			BindingExtension bindingExtension10;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension10 = new BindingExtension(), new Uri("DTCv2\\DTCv2Page.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 144, 33);
			Translate translate5;
			VisualDiagnostics.RegisterSourceInfo(translate5 = new Translate(), new Uri("DTCv2\\DTCv2Page.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 145, 33);
			Label label2;
			VisualDiagnostics.RegisterSourceInfo(label2 = new Label(), new Uri("DTCv2\\DTCv2Page.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 139, 30);
			BindingExtension bindingExtension11;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension11 = new BindingExtension(), new Uri("DTCv2\\DTCv2Page.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 150, 33);
			BindingExtension bindingExtension12;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension12 = new BindingExtension(), new Uri("DTCv2\\DTCv2Page.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 152, 33);
			DynamicResourceExtension dynamicResourceExtension9;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension9 = new DynamicResourceExtension(), new Uri("DTCv2\\DTCv2Page.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 156, 37);
			DynamicResourceExtension dynamicResourceExtension10;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension10 = new DynamicResourceExtension(), new Uri("DTCv2\\DTCv2Page.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 157, 37);
			Translate translate6;
			VisualDiagnostics.RegisterSourceInfo(translate6 = new Translate(), new Uri("DTCv2\\DTCv2Page.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 158, 37);
			StaticResourceExtension staticResourceExtension2;
			VisualDiagnostics.RegisterSourceInfo(staticResourceExtension2 = new StaticResourceExtension(), new Uri("DTCv2\\DTCv2Page.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 159, 37);
			RadioButtonWithColor radioButtonWithColor;
			VisualDiagnostics.RegisterSourceInfo(radioButtonWithColor = new RadioButtonWithColor(), new Uri("DTCv2\\DTCv2Page.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 153, 34);
			DynamicResourceExtension dynamicResourceExtension11;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension11 = new DynamicResourceExtension(), new Uri("DTCv2\\DTCv2Page.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 163, 37);
			DynamicResourceExtension dynamicResourceExtension12;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension12 = new DynamicResourceExtension(), new Uri("DTCv2\\DTCv2Page.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 164, 37);
			Translate translate7;
			VisualDiagnostics.RegisterSourceInfo(translate7 = new Translate(), new Uri("DTCv2\\DTCv2Page.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 165, 37);
			StaticResourceExtension staticResourceExtension3;
			VisualDiagnostics.RegisterSourceInfo(staticResourceExtension3 = new StaticResourceExtension(), new Uri("DTCv2\\DTCv2Page.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 166, 37);
			RadioButtonWithColor radioButtonWithColor2;
			VisualDiagnostics.RegisterSourceInfo(radioButtonWithColor2 = new RadioButtonWithColor(), new Uri("DTCv2\\DTCv2Page.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 160, 34);
			Grid grid2;
			VisualDiagnostics.RegisterSourceInfo(grid2 = new Grid(), new Uri("DTCv2\\DTCv2Page.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 147, 30);
			StackLayout stackLayout;
			VisualDiagnostics.RegisterSourceInfo(stackLayout = new StackLayout(), new Uri("DTCv2\\DTCv2Page.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 132, 26);
			Translate translate8;
			VisualDiagnostics.RegisterSourceInfo(translate8 = new Translate(), new Uri("DTCv2\\DTCv2Page.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 176, 58);
			Label label3;
			VisualDiagnostics.RegisterSourceInfo(label3 = new Label(), new Uri("DTCv2\\DTCv2Page.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 176, 30);
			SharedSettings sharedSettings;
			VisualDiagnostics.RegisterSourceInfo(sharedSettings = SharedSettings.Current, new Uri("DTCv2\\DTCv2Page.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 181, 33);
			BindingExtension bindingExtension13;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension13 = new BindingExtension(), new Uri("DTCv2\\DTCv2Page.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 182, 33);
			Translate translate9;
			VisualDiagnostics.RegisterSourceInfo(translate9 = new Translate(), new Uri("DTCv2\\DTCv2Page.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 183, 33);
			LabelSwitch labelSwitch;
			VisualDiagnostics.RegisterSourceInfo(labelSwitch = new LabelSwitch(), new Uri("DTCv2\\DTCv2Page.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 177, 30);
			SharedSettings sharedSettings2;
			VisualDiagnostics.RegisterSourceInfo(sharedSettings2 = SharedSettings.Current, new Uri("DTCv2\\DTCv2Page.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 188, 33);
			BindingExtension bindingExtension14;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension14 = new BindingExtension(), new Uri("DTCv2\\DTCv2Page.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 189, 33);
			Translate translate10;
			VisualDiagnostics.RegisterSourceInfo(translate10 = new Translate(), new Uri("DTCv2\\DTCv2Page.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 190, 33);
			LabelSwitch labelSwitch2;
			VisualDiagnostics.RegisterSourceInfo(labelSwitch2 = new LabelSwitch(), new Uri("DTCv2\\DTCv2Page.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 184, 30);
			Translate translate11;
			VisualDiagnostics.RegisterSourceInfo(translate11 = new Translate(), new Uri("DTCv2\\DTCv2Page.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 194, 33);
			Button button;
			VisualDiagnostics.RegisterSourceInfo(button = new Button(), new Uri("DTCv2\\DTCv2Page.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 191, 30);
			StackLayout stackLayout2;
			VisualDiagnostics.RegisterSourceInfo(stackLayout2 = new StackLayout(), new Uri("DTCv2\\DTCv2Page.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 174, 26);
			ListView listView;
			VisualDiagnostics.RegisterSourceInfo(listView = new ListView(), new Uri("DTCv2\\DTCv2Page.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 112, 18);
			ColumnDefinition columnDefinition5;
			VisualDiagnostics.RegisterSourceInfo(columnDefinition5 = new ColumnDefinition(), new Uri("DTCv2\\DTCv2Page.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 200, 26);
			ColumnDefinition columnDefinition6;
			VisualDiagnostics.RegisterSourceInfo(columnDefinition6 = new ColumnDefinition(), new Uri("DTCv2\\DTCv2Page.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 201, 26);
			BindingExtension bindingExtension15;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension15 = new BindingExtension(), new Uri("DTCv2\\DTCv2Page.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 206, 25);
			Translate translate12;
			VisualDiagnostics.RegisterSourceInfo(translate12 = new Translate(), new Uri("DTCv2\\DTCv2Page.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 208, 25);
			Button button2;
			VisualDiagnostics.RegisterSourceInfo(button2 = new Button(), new Uri("DTCv2\\DTCv2Page.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 203, 22);
			BindingExtension bindingExtension16;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension16 = new BindingExtension(), new Uri("DTCv2\\DTCv2Page.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 212, 25);
			Translate translate13;
			VisualDiagnostics.RegisterSourceInfo(translate13 = new Translate(), new Uri("DTCv2\\DTCv2Page.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 214, 25);
			Button button3;
			VisualDiagnostics.RegisterSourceInfo(button3 = new Button(), new Uri("DTCv2\\DTCv2Page.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 209, 22);
			Grid grid3;
			VisualDiagnostics.RegisterSourceInfo(grid3 = new Grid(), new Uri("DTCv2\\DTCv2Page.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 198, 18);
			BindingExtension bindingExtension17;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension17 = new BindingExtension(), new Uri("DTCv2\\DTCv2Page.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 220, 21);
			BindingExtension bindingExtension18;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension18 = new BindingExtension(), new Uri("DTCv2\\DTCv2Page.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 221, 21);
			ActivityFrame activityFrame;
			VisualDiagnostics.RegisterSourceInfo(activityFrame = new ActivityFrame(), new Uri("DTCv2\\DTCv2Page.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 216, 18);
			Grid grid4;
			VisualDiagnostics.RegisterSourceInfo(grid4 = new Grid(), new Uri("DTCv2\\DTCv2Page.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 96, 14);
			BindingExtension bindingExtension19;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension19 = new BindingExtension(), new Uri("DTCv2\\DTCv2Page.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 228, 17);
			RowDefinition rowDefinition7;
			VisualDiagnostics.RegisterSourceInfo(rowDefinition7 = new RowDefinition(), new Uri("DTCv2\\DTCv2Page.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 231, 22);
			RowDefinition rowDefinition8;
			VisualDiagnostics.RegisterSourceInfo(rowDefinition8 = new RowDefinition(), new Uri("DTCv2\\DTCv2Page.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 233, 22);
			RowDefinition rowDefinition9;
			VisualDiagnostics.RegisterSourceInfo(rowDefinition9 = new RowDefinition(), new Uri("DTCv2\\DTCv2Page.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 235, 22);
			RowDefinition rowDefinition10;
			VisualDiagnostics.RegisterSourceInfo(rowDefinition10 = new RowDefinition(), new Uri("DTCv2\\DTCv2Page.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 237, 22);
			RowDefinition rowDefinition11;
			VisualDiagnostics.RegisterSourceInfo(rowDefinition11 = new RowDefinition(), new Uri("DTCv2\\DTCv2Page.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 239, 22);
			DynamicResourceExtension dynamicResourceExtension13;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension13 = new DynamicResourceExtension(), new Uri("DTCv2\\DTCv2Page.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 246, 21);
			DynamicResourceExtension dynamicResourceExtension14;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension14 = new DynamicResourceExtension(), new Uri("DTCv2\\DTCv2Page.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 247, 21);
			BindingExtension bindingExtension20;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension20 = new BindingExtension(), new Uri("DTCv2\\DTCv2Page.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 249, 21);
			SharedSettings sharedSettings3;
			VisualDiagnostics.RegisterSourceInfo(sharedSettings3 = SharedSettings.Current, new Uri("DTCv2\\DTCv2Page.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 252, 29);
			BindingExtension bindingExtension21;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension21 = new BindingExtension(), new Uri("DTCv2\\DTCv2Page.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 253, 29);
			Translate translate14;
			VisualDiagnostics.RegisterSourceInfo(translate14 = new Translate(), new Uri("DTCv2\\DTCv2Page.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 254, 29);
			LabelSwitch labelSwitch3;
			VisualDiagnostics.RegisterSourceInfo(labelSwitch3 = new LabelSwitch(), new Uri("DTCv2\\DTCv2Page.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 251, 26);
			SharedSettings sharedSettings4;
			VisualDiagnostics.RegisterSourceInfo(sharedSettings4 = SharedSettings.Current, new Uri("DTCv2\\DTCv2Page.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 256, 29);
			BindingExtension bindingExtension22;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension22 = new BindingExtension(), new Uri("DTCv2\\DTCv2Page.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 257, 29);
			Translate translate15;
			VisualDiagnostics.RegisterSourceInfo(translate15 = new Translate(), new Uri("DTCv2\\DTCv2Page.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 258, 29);
			LabelSwitch labelSwitch4;
			VisualDiagnostics.RegisterSourceInfo(labelSwitch4 = new LabelSwitch(), new Uri("DTCv2\\DTCv2Page.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 255, 26);
			StackLayout stackLayout3;
			VisualDiagnostics.RegisterSourceInfo(stackLayout3 = new StackLayout(), new Uri("DTCv2\\DTCv2Page.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 250, 22);
			Frame frame;
			VisualDiagnostics.RegisterSourceInfo(frame = new Frame(), new Uri("DTCv2\\DTCv2Page.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 242, 18);
			StaticResourceExtension staticResourceExtension4;
			VisualDiagnostics.RegisterSourceInfo(staticResourceExtension4 = new StaticResourceExtension(), new Uri("DTCv2\\DTCv2Page.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 264, 21);
			BindingExtension bindingExtension23;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension23 = new BindingExtension(), new Uri("DTCv2\\DTCv2Page.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 264, 21);
			Translate translate16;
			VisualDiagnostics.RegisterSourceInfo(translate16 = new Translate(), new Uri("DTCv2\\DTCv2Page.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 266, 50);
			Label label4;
			VisualDiagnostics.RegisterSourceInfo(label4 = new Label(), new Uri("DTCv2\\DTCv2Page.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 266, 22);
			Translate translate17;
			VisualDiagnostics.RegisterSourceInfo(translate17 = new Translate(), new Uri("DTCv2\\DTCv2Page.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 267, 28);
			Label label5;
			VisualDiagnostics.RegisterSourceInfo(label5 = new Label(), new Uri("DTCv2\\DTCv2Page.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 267, 22);
			BindingExtension bindingExtension24;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension24 = new BindingExtension(), new Uri("DTCv2\\DTCv2Page.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 270, 25);
			Translate translate18;
			VisualDiagnostics.RegisterSourceInfo(translate18 = new Translate(), new Uri("DTCv2\\DTCv2Page.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 271, 25);
			DynamicResourceExtension dynamicResourceExtension15;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension15 = new DynamicResourceExtension(), new Uri("DTCv2\\DTCv2Page.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 272, 25);
			TapGestureRecognizer tapGestureRecognizer4;
			VisualDiagnostics.RegisterSourceInfo(tapGestureRecognizer4 = new TapGestureRecognizer(), new Uri("DTCv2\\DTCv2Page.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 274, 30);
			Label label6;
			VisualDiagnostics.RegisterSourceInfo(label6 = new Label(), new Uri("DTCv2\\DTCv2Page.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 268, 22);
			StackLayout stackLayout4;
			VisualDiagnostics.RegisterSourceInfo(stackLayout4 = new StackLayout(), new Uri("DTCv2\\DTCv2Page.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 262, 18);
			BindingExtension bindingExtension25;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension25 = new BindingExtension(), new Uri("DTCv2\\DTCv2Page.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 284, 21);
			BindingExtension bindingExtension26;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension26 = new BindingExtension(), new Uri("DTCv2\\DTCv2Page.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 286, 21);
			DataTemplate dataTemplate2;
			VisualDiagnostics.RegisterSourceInfo(dataTemplate2 = new DataTemplate(), new Uri("DTCv2\\DTCv2Page.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 295, 26);
			DataTemplate dataTemplate3;
			VisualDiagnostics.RegisterSourceInfo(dataTemplate3 = new DataTemplate(), new Uri("DTCv2\\DTCv2Page.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 345, 26);
			ListView listView2;
			VisualDiagnostics.RegisterSourceInfo(listView2 = new ListView(), new Uri("DTCv2\\DTCv2Page.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 279, 18);
			BindingExtension bindingExtension27;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension27 = new BindingExtension(), new Uri("DTCv2\\DTCv2Page.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 424, 21);
			BindingExtension bindingExtension28;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension28 = new BindingExtension(), new Uri("DTCv2\\DTCv2Page.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 425, 21);
			ActivityFrame activityFrame2;
			VisualDiagnostics.RegisterSourceInfo(activityFrame2 = new ActivityFrame(), new Uri("DTCv2\\DTCv2Page.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 420, 18);
			BindingExtension bindingExtension29;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension29 = new BindingExtension(), new Uri("DTCv2\\DTCv2Page.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 430, 21);
			BindingExtension bindingExtension30;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension30 = new BindingExtension(), new Uri("DTCv2\\DTCv2Page.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 431, 21);
			BindingExtension bindingExtension31;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension31 = new BindingExtension(), new Uri("DTCv2\\DTCv2Page.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 432, 21);
			Translate translate19;
			VisualDiagnostics.RegisterSourceInfo(translate19 = new Translate(), new Uri("DTCv2\\DTCv2Page.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 433, 21);
			Button button4;
			VisualDiagnostics.RegisterSourceInfo(button4 = new Button(), new Uri("DTCv2\\DTCv2Page.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 427, 18);
			StaticResourceExtension staticResourceExtension5;
			VisualDiagnostics.RegisterSourceInfo(staticResourceExtension5 = new StaticResourceExtension(), new Uri("DTCv2\\DTCv2Page.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 435, 36);
			BindingExtension bindingExtension32;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension32 = new BindingExtension(), new Uri("DTCv2\\DTCv2Page.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 435, 36);
			BindingExtension bindingExtension33;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension33 = new BindingExtension(), new Uri("DTCv2\\DTCv2Page.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 437, 40);
			ColumnDefinition columnDefinition7;
			VisualDiagnostics.RegisterSourceInfo(columnDefinition7 = new ColumnDefinition(), new Uri("DTCv2\\DTCv2Page.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 439, 30);
			ColumnDefinition columnDefinition8;
			VisualDiagnostics.RegisterSourceInfo(columnDefinition8 = new ColumnDefinition(), new Uri("DTCv2\\DTCv2Page.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 440, 30);
			BindingExtension bindingExtension34;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension34 = new BindingExtension(), new Uri("DTCv2\\DTCv2Page.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 444, 29);
			Translate translate20;
			VisualDiagnostics.RegisterSourceInfo(translate20 = new Translate(), new Uri("DTCv2\\DTCv2Page.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 445, 29);
			Button button5;
			VisualDiagnostics.RegisterSourceInfo(button5 = new Button(), new Uri("DTCv2\\DTCv2Page.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 442, 26);
			BindingExtension bindingExtension35;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension35 = new BindingExtension(), new Uri("DTCv2\\DTCv2Page.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 448, 29);
			Translate translate21;
			VisualDiagnostics.RegisterSourceInfo(translate21 = new Translate(), new Uri("DTCv2\\DTCv2Page.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 449, 29);
			Button button6;
			VisualDiagnostics.RegisterSourceInfo(button6 = new Button(), new Uri("DTCv2\\DTCv2Page.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 446, 26);
			Grid grid5;
			VisualDiagnostics.RegisterSourceInfo(grid5 = new Grid(), new Uri("DTCv2\\DTCv2Page.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 437, 22);
			BindingExtension bindingExtension36;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension36 = new BindingExtension(), new Uri("DTCv2\\DTCv2Page.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 454, 25);
			StaticResourceExtension staticResourceExtension6;
			VisualDiagnostics.RegisterSourceInfo(staticResourceExtension6 = new StaticResourceExtension(), new Uri("DTCv2\\DTCv2Page.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 455, 25);
			BindingExtension bindingExtension37;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension37 = new BindingExtension(), new Uri("DTCv2\\DTCv2Page.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 455, 25);
			Translate translate22;
			VisualDiagnostics.RegisterSourceInfo(translate22 = new Translate(), new Uri("DTCv2\\DTCv2Page.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 456, 25);
			Button button7;
			VisualDiagnostics.RegisterSourceInfo(button7 = new Button(), new Uri("DTCv2\\DTCv2Page.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 452, 22);
			Grid grid6;
			VisualDiagnostics.RegisterSourceInfo(grid6 = new Grid(), new Uri("DTCv2\\DTCv2Page.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 435, 18);
			BindingExtension bindingExtension38;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension38 = new BindingExtension(), new Uri("DTCv2\\DTCv2Page.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 462, 21);
			DynamicResourceExtension dynamicResourceExtension16;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension16 = new DynamicResourceExtension(), new Uri("DTCv2\\DTCv2Page.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 466, 25);
			BindingExtension bindingExtension39;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension39 = new BindingExtension(), new Uri("DTCv2\\DTCv2Page.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 467, 25);
			Translate translate23;
			VisualDiagnostics.RegisterSourceInfo(translate23 = new Translate(), new Uri("DTCv2\\DTCv2Page.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 468, 25);
			DynamicResourceExtension dynamicResourceExtension17;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension17 = new DynamicResourceExtension(), new Uri("DTCv2\\DTCv2Page.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 469, 25);
			TapGestureRecognizer tapGestureRecognizer5;
			VisualDiagnostics.RegisterSourceInfo(tapGestureRecognizer5 = new TapGestureRecognizer(), new Uri("DTCv2\\DTCv2Page.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 471, 30);
			Label label7;
			VisualDiagnostics.RegisterSourceInfo(label7 = new Label(), new Uri("DTCv2\\DTCv2Page.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 464, 22);
			StackLayout stackLayout5;
			VisualDiagnostics.RegisterSourceInfo(stackLayout5 = new StackLayout(), new Uri("DTCv2\\DTCv2Page.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 460, 18);
			Grid grid7;
			VisualDiagnostics.RegisterSourceInfo(grid7 = new Grid(), new Uri("DTCv2\\DTCv2Page.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 225, 14);
			ComplexAdView complexAdView;
			VisualDiagnostics.RegisterSourceInfo(complexAdView = new ComplexAdView(), new Uri("DTCv2\\DTCv2Page.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 478, 14);
			Grid grid8;
			VisualDiagnostics.RegisterSourceInfo(grid8 = new Grid(), new Uri("DTCv2\\DTCv2Page.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 91, 10);
			VisualDiagnostics.RegisterSourceInfo(this, new Uri("DTCv2\\DTCv2Page.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 2, 2);
			NameScope nameScope = NameScope.GetNameScope(this) ?? new NameScope();
			NameScope.SetNameScope(this, nameScope);
			nameScope.RegisterName("gridButtons", grid);
			if (grid.StyleId == null)
			{
				grid.StyleId = "gridButtons";
			}
			nameScope.RegisterName("btnCheckAll", image);
			if (image.StyleId == null)
			{
				image.StyleId = "btnCheckAll";
			}
			nameScope.RegisterName("btnUncheckAll", image2);
			if (image2.StyleId == null)
			{
				image2.StyleId = "btnUncheckAll";
			}
			nameScope.RegisterName("btnFilter", image3);
			if (image3.StyleId == null)
			{
				image3.StyleId = "btnFilter";
			}
			nameScope.RegisterName("layoutRoot", grid8);
			if (grid8.StyleId == null)
			{
				grid8.StyleId = "layoutRoot";
			}
			nameScope.RegisterName("gridSetup", grid4);
			if (grid4.StyleId == null)
			{
				grid4.StyleId = "gridSetup";
			}
			nameScope.RegisterName("lvECUs", listView);
			if (listView.StyleId == null)
			{
				listView.StyleId = "lvECUs";
			}
			nameScope.RegisterName("gridAllOrSupportedSelector", grid2);
			if (grid2.StyleId == null)
			{
				grid2.StyleId = "gridAllOrSupportedSelector";
			}
			nameScope.RegisterName("rbDetected", radioButtonWithColor);
			if (radioButtonWithColor.StyleId == null)
			{
				radioButtonWithColor.StyleId = "rbDetected";
			}
			nameScope.RegisterName("rbAll", radioButtonWithColor2);
			if (radioButtonWithColor2.StyleId == null)
			{
				radioButtonWithColor2.StyleId = "rbAll";
			}
			nameScope.RegisterName("swHideArchiveDTC2", labelSwitch);
			if (labelSwitch.StyleId == null)
			{
				labelSwitch.StyleId = "swHideArchiveDTC2";
			}
			nameScope.RegisterName("swHideUncomplitedTests2", labelSwitch2);
			if (labelSwitch2.StyleId == null)
			{
				labelSwitch2.StyleId = "swHideUncomplitedTests2";
			}
			nameScope.RegisterName("btnToOldStyle", button);
			if (button.StyleId == null)
			{
				button.StyleId = "btnToOldStyle";
			}
			nameScope.RegisterName("gridSetupButtons", grid3);
			if (grid3.StyleId == null)
			{
				grid3.StyleId = "gridSetupButtons";
			}
			nameScope.RegisterName("btnReadDTC", button2);
			if (button2.StyleId == null)
			{
				button2.StyleId = "btnReadDTC";
			}
			nameScope.RegisterName("btnClearDTC", button3);
			if (button3.StyleId == null)
			{
				button3.StyleId = "btnClearDTC";
			}
			nameScope.RegisterName("activityFrame2", activityFrame);
			if (activityFrame.StyleId == null)
			{
				activityFrame.StyleId = "activityFrame2";
			}
			nameScope.RegisterName("gridResults", grid7);
			if (grid7.StyleId == null)
			{
				grid7.StyleId = "gridResults";
			}
			nameScope.RegisterName("stackFilter", stackLayout3);
			if (stackLayout3.StyleId == null)
			{
				stackLayout3.StyleId = "stackFilter";
			}
			nameScope.RegisterName("lvDTCs", listView2);
			if (listView2.StyleId == null)
			{
				listView2.StyleId = "lvDTCs";
			}
			nameScope.RegisterName("activityFrame", activityFrame2);
			if (activityFrame2.StyleId == null)
			{
				activityFrame2.StyleId = "activityFrame";
			}
			nameScope.RegisterName("btnCancel", button4);
			if (button4.StyleId == null)
			{
				button4.StyleId = "btnCancel";
			}
			nameScope.RegisterName("ad", complexAdView);
			if (complexAdView.StyleId == null)
			{
				complexAdView.StyleId = "ad";
			}
			this.gridButtons = grid;
			this.btnCheckAll = image;
			this.btnUncheckAll = image2;
			this.btnFilter = image3;
			this.layoutRoot = grid8;
			this.gridSetup = grid4;
			this.lvECUs = listView;
			this.gridAllOrSupportedSelector = grid2;
			this.rbDetected = radioButtonWithColor;
			this.rbAll = radioButtonWithColor2;
			this.swHideArchiveDTC2 = labelSwitch;
			this.swHideUncomplitedTests2 = labelSwitch2;
			this.btnToOldStyle = button;
			this.gridSetupButtons = grid3;
			this.btnReadDTC = button2;
			this.btnClearDTC = button3;
			this.activityFrame2 = activityFrame;
			this.gridResults = grid7;
			this.stackFilter = stackLayout3;
			this.lvDTCs = listView2;
			this.activityFrame = activityFrame2;
			this.btnCancel = button4;
			this.ad = complexAdView;
			this.Resources = resourceDictionary;
			resourceDictionary.Add("DTCStatusCollectionToStringConverter", dtcstatusCollectionToStringConverter);
			resourceDictionary.Add("DTCDescriptionCollectionToStringConverter", dtcdescriptionCollectionToStringConverter);
			resourceDictionary.Add("DTCCodeToStringConverter", dtccodeToStringConverter);
			resourceDictionary.Add("EmptyStringToFalseConverter", emptyStringToFalseConverter);
			resourceDictionary.Add("BoolToNegativeConverter", boolToNegativeConverter);
			resourceDictionary.Add("TrueToRedColorConverter", trueToRedColorConverter);
			translate.Text = "ios_MainPage_TileDtcErrors";
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
			xmlNamespaceResolver.Add("dtcv2", "clr-namespace:CarScannerXamarinForms.DTCv2");
			xmlNamespaceResolver.Add("ecuModels", "clr-namespace:CarScannerXamarinForms.ECUModels");
			xmlNamespaceResolver.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver.Add("settings", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver.Add("syncfusion", "clr-namespace:Syncfusion.XForms.Buttons;assembly=Syncfusion.Buttons.XForms");
			xmlNamespaceResolver.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider.Add(typeFromHandle2, new XamlTypeResolver(xmlNamespaceResolver, typeof(DTCv2Page).GetTypeInfo().Assembly));
			xamlServiceProvider.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(14, 5)));
			object obj2 = markupExtension.ProvideValue(xamlServiceProvider);
			this.Title = obj2;
			this.SetValue(Page.PrefersStatusBarHiddenProperty, 2);
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
			xmlNamespaceResolver2.Add("dtcv2", "clr-namespace:CarScannerXamarinForms.DTCv2");
			xmlNamespaceResolver2.Add("ecuModels", "clr-namespace:CarScannerXamarinForms.ECUModels");
			xmlNamespaceResolver2.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver2.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver2.Add("settings", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver2.Add("syncfusion", "clr-namespace:Syncfusion.XForms.Buttons;assembly=Syncfusion.Buttons.XForms");
			xmlNamespaceResolver2.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider2.Add(typeFromHandle4, new XamlTypeResolver(xmlNamespaceResolver2, typeof(DTCv2Page).GetTypeInfo().Assembly));
			xamlServiceProvider2.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(17, 5)));
			DynamicResource dynamicResource = markupExtension2.ProvideValue(xamlServiceProvider2);
			this.SetDynamicResource(VisualElement.BackgroundColorProperty, dynamicResource.Key);
			this.SetValue(NavigationPage.HasBackButtonProperty, false);
			this.SetValue(NavigationPage.HasNavigationBarProperty, true);
			this.Resources = resourceDictionary;
			columnDefinition.SetValue(ColumnDefinition.WidthProperty, new GridLengthTypeConverter().ConvertFromInvariantString("auto"));
			grid.GetValue(Grid.ColumnDefinitionsProperty).Add(columnDefinition);
			columnDefinition2.SetValue(ColumnDefinition.WidthProperty, new GridLengthTypeConverter().ConvertFromInvariantString("*"));
			grid.GetValue(Grid.ColumnDefinitionsProperty).Add(columnDefinition2);
			columnDefinition3.SetValue(ColumnDefinition.WidthProperty, new GridLengthTypeConverter().ConvertFromInvariantString("auto"));
			grid.GetValue(Grid.ColumnDefinitionsProperty).Add(columnDefinition3);
			columnDefinition4.SetValue(ColumnDefinition.WidthProperty, new GridLengthTypeConverter().ConvertFromInvariantString("auto"));
			grid.GetValue(Grid.ColumnDefinitionsProperty).Add(columnDefinition4);
			linkButton.SetValue(Grid.ColumnProperty, 0);
			bindingExtension.Path = "BackButtonCommand";
			BindingBase bindingBase = bindingExtension.ProvideValue(null);
			linkButton.SetBinding(Button.CommandProperty, bindingBase);
			linkButton.SetValue(View.HorizontalOptionsProperty, LayoutOptions.Start);
			dynamicResourceExtension2.Key = "NavigationBarButton";
			IMarkupExtension<DynamicResource> markupExtension3 = dynamicResourceExtension2;
			XamlServiceProvider xamlServiceProvider3 = new XamlServiceProvider();
			Type typeFromHandle5 = typeof(IProvideValueTarget);
			object[] array3 = new object[0 + 3];
			array3[0] = linkButton;
			array3[1] = grid;
			array3[2] = this;
			object obj4;
			xamlServiceProvider3.Add(typeFromHandle5, obj4 = new SimpleValueTargetProvider(array3, VisualElement.StyleProperty, nameScope));
			xamlServiceProvider3.Add(typeof(IReferenceProvider), obj4);
			Type typeFromHandle6 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver3 = new XmlNamespaceResolver();
			xmlNamespaceResolver3.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver3.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver3.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver3.Add("dtcv2", "clr-namespace:CarScannerXamarinForms.DTCv2");
			xmlNamespaceResolver3.Add("ecuModels", "clr-namespace:CarScannerXamarinForms.ECUModels");
			xmlNamespaceResolver3.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver3.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver3.Add("settings", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver3.Add("syncfusion", "clr-namespace:Syncfusion.XForms.Buttons;assembly=Syncfusion.Buttons.XForms");
			xmlNamespaceResolver3.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider3.Add(typeFromHandle6, new XamlTypeResolver(xmlNamespaceResolver3, typeof(DTCv2Page).GetTypeInfo().Assembly));
			xamlServiceProvider3.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(44, 17)));
			DynamicResource dynamicResource2 = markupExtension3.ProvideValue(xamlServiceProvider3);
			linkButton.SetDynamicResource(VisualElement.StyleProperty, dynamicResource2.Key);
			translate2.Text = "ios_Back";
			IMarkupExtension markupExtension4 = translate2;
			XamlServiceProvider xamlServiceProvider4 = new XamlServiceProvider();
			Type typeFromHandle7 = typeof(IProvideValueTarget);
			object[] array4 = new object[0 + 3];
			array4[0] = linkButton;
			array4[1] = grid;
			array4[2] = this;
			object obj5;
			xamlServiceProvider4.Add(typeFromHandle7, obj5 = new SimpleValueTargetProvider(array4, Button.TextProperty, nameScope));
			xamlServiceProvider4.Add(typeof(IReferenceProvider), obj5);
			Type typeFromHandle8 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver4 = new XmlNamespaceResolver();
			xmlNamespaceResolver4.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver4.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver4.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver4.Add("dtcv2", "clr-namespace:CarScannerXamarinForms.DTCv2");
			xmlNamespaceResolver4.Add("ecuModels", "clr-namespace:CarScannerXamarinForms.ECUModels");
			xmlNamespaceResolver4.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver4.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver4.Add("settings", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver4.Add("syncfusion", "clr-namespace:Syncfusion.XForms.Buttons;assembly=Syncfusion.Buttons.XForms");
			xmlNamespaceResolver4.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider4.Add(typeFromHandle8, new XamlTypeResolver(xmlNamespaceResolver4, typeof(DTCv2Page).GetTypeInfo().Assembly));
			xamlServiceProvider4.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(45, 17)));
			object obj6 = markupExtension4.ProvideValue(xamlServiceProvider4);
			linkButton.Text = obj6;
			grid.Children.Add(linkButton);
			nonScalableLabel.SetValue(Grid.ColumnProperty, 1);
			nonScalableLabel.SetValue(View.HorizontalOptionsProperty, LayoutOptions.Center);
			nonScalableLabel.SetValue(Label.HorizontalTextAlignmentProperty, new TextAlignmentConverter().ConvertFromInvariantString("Center"));
			nonScalableLabel.SetValue(Label.LineBreakModeProperty, 4);
			dynamicResourceExtension3.Key = "NavigationBarNonScalableLabel";
			IMarkupExtension<DynamicResource> markupExtension5 = dynamicResourceExtension3;
			XamlServiceProvider xamlServiceProvider5 = new XamlServiceProvider();
			Type typeFromHandle9 = typeof(IProvideValueTarget);
			object[] array5 = new object[0 + 3];
			array5[0] = nonScalableLabel;
			array5[1] = grid;
			array5[2] = this;
			object obj7;
			xamlServiceProvider5.Add(typeFromHandle9, obj7 = new SimpleValueTargetProvider(array5, VisualElement.StyleProperty, nameScope));
			xamlServiceProvider5.Add(typeof(IReferenceProvider), obj7);
			Type typeFromHandle10 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver5 = new XmlNamespaceResolver();
			xmlNamespaceResolver5.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver5.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver5.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver5.Add("dtcv2", "clr-namespace:CarScannerXamarinForms.DTCv2");
			xmlNamespaceResolver5.Add("ecuModels", "clr-namespace:CarScannerXamarinForms.ECUModels");
			xmlNamespaceResolver5.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver5.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver5.Add("settings", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver5.Add("syncfusion", "clr-namespace:Syncfusion.XForms.Buttons;assembly=Syncfusion.Buttons.XForms");
			xmlNamespaceResolver5.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider5.Add(typeFromHandle10, new XamlTypeResolver(xmlNamespaceResolver5, typeof(DTCv2Page).GetTypeInfo().Assembly));
			xamlServiceProvider5.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(51, 17)));
			DynamicResource dynamicResource3 = markupExtension5.ProvideValue(xamlServiceProvider5);
			nonScalableLabel.SetDynamicResource(VisualElement.StyleProperty, dynamicResource3.Key);
			translate3.Text = "ios_MainPage_TileDtcErrors";
			IMarkupExtension markupExtension6 = translate3;
			XamlServiceProvider xamlServiceProvider6 = new XamlServiceProvider();
			Type typeFromHandle11 = typeof(IProvideValueTarget);
			object[] array6 = new object[0 + 3];
			array6[0] = nonScalableLabel;
			array6[1] = grid;
			array6[2] = this;
			object obj8;
			xamlServiceProvider6.Add(typeFromHandle11, obj8 = new SimpleValueTargetProvider(array6, Label.TextProperty, nameScope));
			xamlServiceProvider6.Add(typeof(IReferenceProvider), obj8);
			Type typeFromHandle12 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver6 = new XmlNamespaceResolver();
			xmlNamespaceResolver6.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver6.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver6.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver6.Add("dtcv2", "clr-namespace:CarScannerXamarinForms.DTCv2");
			xmlNamespaceResolver6.Add("ecuModels", "clr-namespace:CarScannerXamarinForms.ECUModels");
			xmlNamespaceResolver6.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver6.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver6.Add("settings", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver6.Add("syncfusion", "clr-namespace:Syncfusion.XForms.Buttons;assembly=Syncfusion.Buttons.XForms");
			xmlNamespaceResolver6.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider6.Add(typeFromHandle12, new XamlTypeResolver(xmlNamespaceResolver6, typeof(DTCv2Page).GetTypeInfo().Assembly));
			xamlServiceProvider6.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(52, 17)));
			object obj9 = markupExtension6.ProvideValue(xamlServiceProvider6);
			nonScalableLabel.Text = obj9;
			grid.Children.Add(nonScalableLabel);
			image.SetValue(Grid.ColumnProperty, 2);
			image.SetValue(View.MarginProperty, new Thickness(0.0));
			image.SetValue(VisualElement.BackgroundColorProperty, Color.Transparent);
			bindingExtension2.Mode = 2;
			bindingExtension2.Path = "IsSetup";
			BindingBase bindingBase2 = bindingExtension2.ProvideValue(null);
			image.SetBinding(VisualElement.IsVisibleProperty, bindingBase2);
			dynamicResourceExtension4.Key = "NB_check";
			IMarkupExtension<DynamicResource> markupExtension7 = dynamicResourceExtension4;
			XamlServiceProvider xamlServiceProvider7 = new XamlServiceProvider();
			Type typeFromHandle13 = typeof(IProvideValueTarget);
			object[] array7 = new object[0 + 3];
			array7[0] = image;
			array7[1] = grid;
			array7[2] = this;
			object obj10;
			xamlServiceProvider7.Add(typeFromHandle13, obj10 = new SimpleValueTargetProvider(array7, Image.SourceProperty, nameScope));
			xamlServiceProvider7.Add(typeof(IReferenceProvider), obj10);
			Type typeFromHandle14 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver7 = new XmlNamespaceResolver();
			xmlNamespaceResolver7.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver7.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver7.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver7.Add("dtcv2", "clr-namespace:CarScannerXamarinForms.DTCv2");
			xmlNamespaceResolver7.Add("ecuModels", "clr-namespace:CarScannerXamarinForms.ECUModels");
			xmlNamespaceResolver7.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver7.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver7.Add("settings", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver7.Add("syncfusion", "clr-namespace:Syncfusion.XForms.Buttons;assembly=Syncfusion.Buttons.XForms");
			xmlNamespaceResolver7.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider7.Add(typeFromHandle14, new XamlTypeResolver(xmlNamespaceResolver7, typeof(DTCv2Page).GetTypeInfo().Assembly));
			xamlServiceProvider7.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(60, 17)));
			DynamicResource dynamicResource4 = markupExtension7.ProvideValue(xamlServiceProvider7);
			image.SetDynamicResource(Image.SourceProperty, dynamicResource4.Key);
			bindingExtension3.Path = "CheckAllCommand";
			BindingBase bindingBase3 = bindingExtension3.ProvideValue(null);
			tapGestureRecognizer.SetBinding(TapGestureRecognizer.CommandProperty, bindingBase3);
			tapGestureRecognizer.SetValue(TapGestureRecognizer.NumberOfTapsRequiredProperty, 1);
			image.GestureRecognizers.Add(tapGestureRecognizer);
			grid.Children.Add(image);
			image2.SetValue(Grid.ColumnProperty, 3);
			image2.SetValue(View.MarginProperty, new Thickness(0.0, 0.0, 5.0, 0.0));
			image2.SetValue(VisualElement.BackgroundColorProperty, Color.Transparent);
			bindingExtension4.Mode = 2;
			bindingExtension4.Path = "IsSetup";
			BindingBase bindingBase4 = bindingExtension4.ProvideValue(null);
			image2.SetBinding(VisualElement.IsVisibleProperty, bindingBase4);
			dynamicResourceExtension5.Key = "NB_uncheck";
			IMarkupExtension<DynamicResource> markupExtension8 = dynamicResourceExtension5;
			XamlServiceProvider xamlServiceProvider8 = new XamlServiceProvider();
			Type typeFromHandle15 = typeof(IProvideValueTarget);
			object[] array8 = new object[0 + 3];
			array8[0] = image2;
			array8[1] = grid;
			array8[2] = this;
			object obj11;
			xamlServiceProvider8.Add(typeFromHandle15, obj11 = new SimpleValueTargetProvider(array8, Image.SourceProperty, nameScope));
			xamlServiceProvider8.Add(typeof(IReferenceProvider), obj11);
			Type typeFromHandle16 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver8 = new XmlNamespaceResolver();
			xmlNamespaceResolver8.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver8.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver8.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver8.Add("dtcv2", "clr-namespace:CarScannerXamarinForms.DTCv2");
			xmlNamespaceResolver8.Add("ecuModels", "clr-namespace:CarScannerXamarinForms.ECUModels");
			xmlNamespaceResolver8.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver8.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver8.Add("settings", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver8.Add("syncfusion", "clr-namespace:Syncfusion.XForms.Buttons;assembly=Syncfusion.Buttons.XForms");
			xmlNamespaceResolver8.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider8.Add(typeFromHandle16, new XamlTypeResolver(xmlNamespaceResolver8, typeof(DTCv2Page).GetTypeInfo().Assembly));
			xamlServiceProvider8.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(71, 17)));
			DynamicResource dynamicResource5 = markupExtension8.ProvideValue(xamlServiceProvider8);
			image2.SetDynamicResource(Image.SourceProperty, dynamicResource5.Key);
			bindingExtension5.Path = "CheckNoneCommand";
			BindingBase bindingBase5 = bindingExtension5.ProvideValue(null);
			tapGestureRecognizer2.SetBinding(TapGestureRecognizer.CommandProperty, bindingBase5);
			tapGestureRecognizer2.SetValue(TapGestureRecognizer.NumberOfTapsRequiredProperty, 1);
			image2.GestureRecognizers.Add(tapGestureRecognizer2);
			grid.Children.Add(image2);
			image3.SetValue(Grid.ColumnProperty, 3);
			bindingExtension6.Mode = 2;
			bindingExtension6.Path = "IsResults";
			BindingBase bindingBase6 = bindingExtension6.ProvideValue(null);
			image3.SetBinding(VisualElement.IsVisibleProperty, bindingBase6);
			dynamicResourceExtension6.Key = "NB_filter";
			IMarkupExtension<DynamicResource> markupExtension9 = dynamicResourceExtension6;
			XamlServiceProvider xamlServiceProvider9 = new XamlServiceProvider();
			Type typeFromHandle17 = typeof(IProvideValueTarget);
			object[] array9 = new object[0 + 3];
			array9[0] = image3;
			array9[1] = grid;
			array9[2] = this;
			object obj12;
			xamlServiceProvider9.Add(typeFromHandle17, obj12 = new SimpleValueTargetProvider(array9, Image.SourceProperty, nameScope));
			xamlServiceProvider9.Add(typeof(IReferenceProvider), obj12);
			Type typeFromHandle18 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver9 = new XmlNamespaceResolver();
			xmlNamespaceResolver9.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver9.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver9.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver9.Add("dtcv2", "clr-namespace:CarScannerXamarinForms.DTCv2");
			xmlNamespaceResolver9.Add("ecuModels", "clr-namespace:CarScannerXamarinForms.ECUModels");
			xmlNamespaceResolver9.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver9.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver9.Add("settings", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver9.Add("syncfusion", "clr-namespace:Syncfusion.XForms.Buttons;assembly=Syncfusion.Buttons.XForms");
			xmlNamespaceResolver9.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider9.Add(typeFromHandle18, new XamlTypeResolver(xmlNamespaceResolver9, typeof(DTCv2Page).GetTypeInfo().Assembly));
			xamlServiceProvider9.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(80, 17)));
			DynamicResource dynamicResource6 = markupExtension9.ProvideValue(xamlServiceProvider9);
			image3.SetDynamicResource(Image.SourceProperty, dynamicResource6.Key);
			bindingExtension7.Path = "FilterSwitchCommand";
			BindingBase bindingBase7 = bindingExtension7.ProvideValue(null);
			tapGestureRecognizer3.SetBinding(TapGestureRecognizer.CommandProperty, bindingBase7);
			tapGestureRecognizer3.SetValue(TapGestureRecognizer.NumberOfTapsRequiredProperty, 1);
			image3.GestureRecognizers.Add(tapGestureRecognizer3);
			grid.Children.Add(image3);
			this.SetValue(NavigationPage.TitleViewProperty, grid);
			grid8.SetValue(View.MarginProperty, new Thickness(5.0, 0.0));
			rowDefinition.SetValue(RowDefinition.HeightProperty, new GridLengthTypeConverter().ConvertFromInvariantString("*"));
			grid8.GetValue(Grid.RowDefinitionsProperty).Add(rowDefinition);
			rowDefinition2.SetValue(RowDefinition.HeightProperty, new GridLengthTypeConverter().ConvertFromInvariantString("auto"));
			grid8.GetValue(Grid.RowDefinitionsProperty).Add(rowDefinition2);
			grid4.SetValue(Grid.RowProperty, 0);
			bindingExtension8.Mode = 2;
			bindingExtension8.Path = "IsSetup";
			BindingBase bindingBase8 = bindingExtension8.ProvideValue(null);
			grid4.SetBinding(VisualElement.IsVisibleProperty, bindingBase8);
			rowDefinition3.SetValue(RowDefinition.HeightProperty, new GridLengthTypeConverter().ConvertFromInvariantString("auto"));
			grid4.GetValue(Grid.RowDefinitionsProperty).Add(rowDefinition3);
			rowDefinition4.SetValue(RowDefinition.HeightProperty, new GridLengthTypeConverter().ConvertFromInvariantString("*"));
			grid4.GetValue(Grid.RowDefinitionsProperty).Add(rowDefinition4);
			rowDefinition5.SetValue(RowDefinition.HeightProperty, new GridLengthTypeConverter().ConvertFromInvariantString("auto"));
			grid4.GetValue(Grid.RowDefinitionsProperty).Add(rowDefinition5);
			rowDefinition6.SetValue(RowDefinition.HeightProperty, new GridLengthTypeConverter().ConvertFromInvariantString("auto"));
			grid4.GetValue(Grid.RowDefinitionsProperty).Add(rowDefinition6);
			listView.SetValue(Grid.RowProperty, 1);
			listView.SetValue(ListView.HasUnevenRowsProperty, true);
			listView.SetValue(ListView.IsGroupingEnabledProperty, false);
			listView.ItemSelected += this.LvECUs_ItemSelected;
			bindingExtension9.Path = "ECUListToDisplay";
			bindingExtension9.TypedBinding = new TypedBinding<DTCv2Model, ObservableCollection<IECU>>(delegate(DTCv2Model A_0)
			{
				if (A_0 != null)
				{
					return new ValueTuple<ObservableCollection<IECU>, bool>(A_0.ECUListToDisplay, true);
				}
				return default(ValueTuple<ObservableCollection<IECU>, bool>);
			}, null, new Tuple<Func<DTCv2Model, object>, string>[]
			{
				new Tuple<Func<DTCv2Model, object>, string>((DTCv2Model A_0) => A_0, "ECUListToDisplay")
			});
			BindingBase bindingBase9 = bindingExtension9.ProvideValue(null);
			listView.SetBinding(ItemsView<Cell>.ItemsSourceProperty, bindingBase9);
			IDataTemplate dataTemplate4 = dataTemplate;
			DTCv2Page.<InitializeComponent>_anonXamlCDataTemplate_26 <InitializeComponent>_anonXamlCDataTemplate_ = new DTCv2Page.<InitializeComponent>_anonXamlCDataTemplate_26();
			object[] array10 = new object[0 + 5];
			array10[0] = dataTemplate;
			array10[1] = listView;
			array10[2] = grid4;
			array10[3] = grid8;
			array10[4] = this;
			<InitializeComponent>_anonXamlCDataTemplate_.parentValues = array10;
			<InitializeComponent>_anonXamlCDataTemplate_.root = this;
			dataTemplate4.LoadTemplate = new Func<object>(<InitializeComponent>_anonXamlCDataTemplate_.LoadDataTemplate);
			listView.SetValue(ItemsView<Cell>.ItemTemplateProperty, dataTemplate);
			stackLayout.SetValue(StackLayout.OrientationProperty, 0);
			label.SetValue(Label.FontAttributesProperty, new FontAttributesConverter().ConvertFromInvariantString("Bold"));
			dynamicResourceExtension7.Key = "BaseFontSize+";
			IMarkupExtension<DynamicResource> markupExtension10 = dynamicResourceExtension7;
			XamlServiceProvider xamlServiceProvider10 = new XamlServiceProvider();
			Type typeFromHandle19 = typeof(IProvideValueTarget);
			object[] array11 = new object[0 + 6];
			array11[0] = label;
			array11[1] = stackLayout;
			array11[2] = listView;
			array11[3] = grid4;
			array11[4] = grid8;
			array11[5] = this;
			object obj13;
			xamlServiceProvider10.Add(typeFromHandle19, obj13 = new SimpleValueTargetProvider(array11, Label.FontSizeProperty, nameScope));
			xamlServiceProvider10.Add(typeof(IReferenceProvider), obj13);
			Type typeFromHandle20 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver10 = new XmlNamespaceResolver();
			xmlNamespaceResolver10.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver10.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver10.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver10.Add("dtcv2", "clr-namespace:CarScannerXamarinForms.DTCv2");
			xmlNamespaceResolver10.Add("ecuModels", "clr-namespace:CarScannerXamarinForms.ECUModels");
			xmlNamespaceResolver10.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver10.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver10.Add("settings", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver10.Add("syncfusion", "clr-namespace:Syncfusion.XForms.Buttons;assembly=Syncfusion.Buttons.XForms");
			xmlNamespaceResolver10.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider10.Add(typeFromHandle20, new XamlTypeResolver(xmlNamespaceResolver10, typeof(DTCv2Page).GetTypeInfo().Assembly));
			xamlServiceProvider10.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(135, 33)));
			DynamicResource dynamicResource7 = markupExtension10.ProvideValue(xamlServiceProvider10);
			label.SetDynamicResource(Label.FontSizeProperty, dynamicResource7.Key);
			label.SetValue(View.HorizontalOptionsProperty, LayoutOptions.Center);
			label.SetValue(Label.HorizontalTextAlignmentProperty, new TextAlignmentConverter().ConvertFromInvariantString("Center"));
			translate4.Text = "dtc_ChooseECULabel";
			IMarkupExtension markupExtension11 = translate4;
			XamlServiceProvider xamlServiceProvider11 = new XamlServiceProvider();
			Type typeFromHandle21 = typeof(IProvideValueTarget);
			object[] array12 = new object[0 + 6];
			array12[0] = label;
			array12[1] = stackLayout;
			array12[2] = listView;
			array12[3] = grid4;
			array12[4] = grid8;
			array12[5] = this;
			object obj14;
			xamlServiceProvider11.Add(typeFromHandle21, obj14 = new SimpleValueTargetProvider(array12, Label.TextProperty, nameScope));
			xamlServiceProvider11.Add(typeof(IReferenceProvider), obj14);
			Type typeFromHandle22 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver11 = new XmlNamespaceResolver();
			xmlNamespaceResolver11.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver11.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver11.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver11.Add("dtcv2", "clr-namespace:CarScannerXamarinForms.DTCv2");
			xmlNamespaceResolver11.Add("ecuModels", "clr-namespace:CarScannerXamarinForms.ECUModels");
			xmlNamespaceResolver11.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver11.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver11.Add("settings", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver11.Add("syncfusion", "clr-namespace:Syncfusion.XForms.Buttons;assembly=Syncfusion.Buttons.XForms");
			xmlNamespaceResolver11.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider11.Add(typeFromHandle22, new XamlTypeResolver(xmlNamespaceResolver11, typeof(DTCv2Page).GetTypeInfo().Assembly));
			xamlServiceProvider11.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(138, 33)));
			object obj15 = markupExtension11.ProvideValue(xamlServiceProvider11);
			label.Text = obj15;
			stackLayout.Children.Add(label);
			label2.SetValue(Label.FontAttributesProperty, new FontAttributesConverter().ConvertFromInvariantString("Bold"));
			dynamicResourceExtension8.Key = "BaseFontSize";
			IMarkupExtension<DynamicResource> markupExtension12 = dynamicResourceExtension8;
			XamlServiceProvider xamlServiceProvider12 = new XamlServiceProvider();
			Type typeFromHandle23 = typeof(IProvideValueTarget);
			object[] array13 = new object[0 + 6];
			array13[0] = label2;
			array13[1] = stackLayout;
			array13[2] = listView;
			array13[3] = grid4;
			array13[4] = grid8;
			array13[5] = this;
			object obj16;
			xamlServiceProvider12.Add(typeFromHandle23, obj16 = new SimpleValueTargetProvider(array13, Label.FontSizeProperty, nameScope));
			xamlServiceProvider12.Add(typeof(IReferenceProvider), obj16);
			Type typeFromHandle24 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver12 = new XmlNamespaceResolver();
			xmlNamespaceResolver12.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver12.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver12.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver12.Add("dtcv2", "clr-namespace:CarScannerXamarinForms.DTCv2");
			xmlNamespaceResolver12.Add("ecuModels", "clr-namespace:CarScannerXamarinForms.ECUModels");
			xmlNamespaceResolver12.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver12.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver12.Add("settings", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver12.Add("syncfusion", "clr-namespace:Syncfusion.XForms.Buttons;assembly=Syncfusion.Buttons.XForms");
			xmlNamespaceResolver12.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider12.Add(typeFromHandle24, new XamlTypeResolver(xmlNamespaceResolver12, typeof(DTCv2Page).GetTypeInfo().Assembly));
			xamlServiceProvider12.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(141, 33)));
			DynamicResource dynamicResource8 = markupExtension12.ProvideValue(xamlServiceProvider12);
			label2.SetDynamicResource(Label.FontSizeProperty, dynamicResource8.Key);
			label2.SetValue(View.HorizontalOptionsProperty, LayoutOptions.Center);
			label2.SetValue(Label.HorizontalTextAlignmentProperty, new TextAlignmentConverter().ConvertFromInvariantString("Center"));
			bindingExtension10.Mode = 2;
			staticResourceExtension.Key = "BoolToNegativeConverter";
			IMarkupExtension markupExtension13 = staticResourceExtension;
			XamlServiceProvider xamlServiceProvider13 = new XamlServiceProvider();
			Type typeFromHandle25 = typeof(IProvideValueTarget);
			object[] array14 = new object[0 + 7];
			array14[0] = bindingExtension10;
			array14[1] = label2;
			array14[2] = stackLayout;
			array14[3] = listView;
			array14[4] = grid4;
			array14[5] = grid8;
			array14[6] = this;
			object obj17;
			xamlServiceProvider13.Add(typeFromHandle25, obj17 = new SimpleValueTargetProvider(array14, typeof(BindingExtension).GetRuntimeProperty("Converter"), nameScope));
			xamlServiceProvider13.Add(typeof(IReferenceProvider), obj17);
			Type typeFromHandle26 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver13 = new XmlNamespaceResolver();
			xmlNamespaceResolver13.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver13.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver13.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver13.Add("dtcv2", "clr-namespace:CarScannerXamarinForms.DTCv2");
			xmlNamespaceResolver13.Add("ecuModels", "clr-namespace:CarScannerXamarinForms.ECUModels");
			xmlNamespaceResolver13.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver13.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver13.Add("settings", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver13.Add("syncfusion", "clr-namespace:Syncfusion.XForms.Buttons;assembly=Syncfusion.Buttons.XForms");
			xmlNamespaceResolver13.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider13.Add(typeFromHandle26, new XamlTypeResolver(xmlNamespaceResolver13, typeof(DTCv2Page).GetTypeInfo().Assembly));
			xamlServiceProvider13.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(144, 33)));
			object obj18 = markupExtension13.ProvideValue(xamlServiceProvider13);
			bindingExtension10.Converter = obj18;
			bindingExtension10.Path = "DisplayAllOrDetectedSelector";
			bindingExtension10.TypedBinding = new TypedBinding<DTCv2Model, bool>(delegate(DTCv2Model A_0)
			{
				if (A_0 != null)
				{
					return new ValueTuple<bool, bool>(A_0.DisplayAllOrDetectedSelector, true);
				}
				return default(ValueTuple<bool, bool>);
			}, null, new Tuple<Func<DTCv2Model, object>, string>[]
			{
				new Tuple<Func<DTCv2Model, object>, string>((DTCv2Model A_0) => A_0, "DisplayAllOrDetectedSelector")
			});
			BindingBase bindingBase10 = bindingExtension10.ProvideValue(null);
			label2.SetBinding(VisualElement.IsVisibleProperty, bindingBase10);
			translate5.Text = "dtc_ECUListHint";
			IMarkupExtension markupExtension14 = translate5;
			XamlServiceProvider xamlServiceProvider14 = new XamlServiceProvider();
			Type typeFromHandle27 = typeof(IProvideValueTarget);
			object[] array15 = new object[0 + 6];
			array15[0] = label2;
			array15[1] = stackLayout;
			array15[2] = listView;
			array15[3] = grid4;
			array15[4] = grid8;
			array15[5] = this;
			object obj19;
			xamlServiceProvider14.Add(typeFromHandle27, obj19 = new SimpleValueTargetProvider(array15, Label.TextProperty, nameScope));
			xamlServiceProvider14.Add(typeof(IReferenceProvider), obj19);
			Type typeFromHandle28 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver14 = new XmlNamespaceResolver();
			xmlNamespaceResolver14.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver14.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver14.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver14.Add("dtcv2", "clr-namespace:CarScannerXamarinForms.DTCv2");
			xmlNamespaceResolver14.Add("ecuModels", "clr-namespace:CarScannerXamarinForms.ECUModels");
			xmlNamespaceResolver14.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver14.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver14.Add("settings", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver14.Add("syncfusion", "clr-namespace:Syncfusion.XForms.Buttons;assembly=Syncfusion.Buttons.XForms");
			xmlNamespaceResolver14.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider14.Add(typeFromHandle28, new XamlTypeResolver(xmlNamespaceResolver14, typeof(DTCv2Page).GetTypeInfo().Assembly));
			xamlServiceProvider14.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(145, 33)));
			object obj20 = markupExtension14.ProvideValue(xamlServiceProvider14);
			label2.Text = obj20;
			stackLayout.Children.Add(label2);
			grid2.SetValue(Grid.ColumnDefinitionsProperty, new ColumnDefinitionCollectionTypeConverter().ConvertFromInvariantString("*,*"));
			bindingExtension11.Mode = 2;
			bindingExtension11.Path = "DisplayAllOrDetectedSelector";
			bindingExtension11.TypedBinding = new TypedBinding<DTCv2Model, bool>(delegate(DTCv2Model A_0)
			{
				if (A_0 != null)
				{
					return new ValueTuple<bool, bool>(A_0.DisplayAllOrDetectedSelector, true);
				}
				return default(ValueTuple<bool, bool>);
			}, null, new Tuple<Func<DTCv2Model, object>, string>[]
			{
				new Tuple<Func<DTCv2Model, object>, string>((DTCv2Model A_0) => A_0, "DisplayAllOrDetectedSelector")
			});
			BindingBase bindingBase11 = bindingExtension11.ProvideValue(null);
			grid2.SetBinding(VisualElement.IsVisibleProperty, bindingBase11);
			grid2.SetValue(RadioButtonGroup.GroupNameProperty, "AllOrSupportedSelector");
			bindingExtension12.Mode = 1;
			bindingExtension12.Path = "DisplayDetected";
			bindingExtension12.TypedBinding = new TypedBinding<DTCv2Model, bool>(delegate(DTCv2Model A_0)
			{
				if (A_0 != null)
				{
					return new ValueTuple<bool, bool>(A_0.DisplayDetected, true);
				}
				return default(ValueTuple<bool, bool>);
			}, delegate(DTCv2Model A_0, bool A_1)
			{
				if (A_0 != null)
				{
					A_0.DisplayDetected = A_1;
					return;
				}
			}, new Tuple<Func<DTCv2Model, object>, string>[]
			{
				new Tuple<Func<DTCv2Model, object>, string>((DTCv2Model A_0) => A_0, "DisplayDetected")
			});
			BindingBase bindingBase12 = bindingExtension12.ProvideValue(null);
			grid2.SetBinding(RadioButtonGroup.SelectedValueProperty, bindingBase12);
			radioButtonWithColor.SetValue(Grid.ColumnProperty, 0);
			dynamicResourceExtension9.Key = "BackgroundColor";
			IMarkupExtension<DynamicResource> markupExtension15 = dynamicResourceExtension9;
			XamlServiceProvider xamlServiceProvider15 = new XamlServiceProvider();
			Type typeFromHandle29 = typeof(IProvideValueTarget);
			object[] array16 = new object[0 + 7];
			array16[0] = radioButtonWithColor;
			array16[1] = grid2;
			array16[2] = stackLayout;
			array16[3] = listView;
			array16[4] = grid4;
			array16[5] = grid8;
			array16[6] = this;
			object obj21;
			xamlServiceProvider15.Add(typeFromHandle29, obj21 = new SimpleValueTargetProvider(array16, VisualElement.BackgroundColorProperty, nameScope));
			xamlServiceProvider15.Add(typeof(IReferenceProvider), obj21);
			Type typeFromHandle30 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver15 = new XmlNamespaceResolver();
			xmlNamespaceResolver15.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver15.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver15.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver15.Add("dtcv2", "clr-namespace:CarScannerXamarinForms.DTCv2");
			xmlNamespaceResolver15.Add("ecuModels", "clr-namespace:CarScannerXamarinForms.ECUModels");
			xmlNamespaceResolver15.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver15.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver15.Add("settings", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver15.Add("syncfusion", "clr-namespace:Syncfusion.XForms.Buttons;assembly=Syncfusion.Buttons.XForms");
			xmlNamespaceResolver15.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider15.Add(typeFromHandle30, new XamlTypeResolver(xmlNamespaceResolver15, typeof(DTCv2Page).GetTypeInfo().Assembly));
			xamlServiceProvider15.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(156, 37)));
			DynamicResource dynamicResource9 = markupExtension15.ProvideValue(xamlServiceProvider15);
			radioButtonWithColor.SetDynamicResource(VisualElement.BackgroundColorProperty, dynamicResource9.Key);
			dynamicResourceExtension10.Key = "ButtonAccentColor";
			IMarkupExtension<DynamicResource> markupExtension16 = dynamicResourceExtension10;
			XamlServiceProvider xamlServiceProvider16 = new XamlServiceProvider();
			Type typeFromHandle31 = typeof(IProvideValueTarget);
			object[] array17 = new object[0 + 7];
			array17[0] = radioButtonWithColor;
			array17[1] = grid2;
			array17[2] = stackLayout;
			array17[3] = listView;
			array17[4] = grid4;
			array17[5] = grid8;
			array17[6] = this;
			object obj22;
			xamlServiceProvider16.Add(typeFromHandle31, obj22 = new SimpleValueTargetProvider(array17, RadioButton.BorderColorProperty, nameScope));
			xamlServiceProvider16.Add(typeof(IReferenceProvider), obj22);
			Type typeFromHandle32 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver16 = new XmlNamespaceResolver();
			xmlNamespaceResolver16.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver16.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver16.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver16.Add("dtcv2", "clr-namespace:CarScannerXamarinForms.DTCv2");
			xmlNamespaceResolver16.Add("ecuModels", "clr-namespace:CarScannerXamarinForms.ECUModels");
			xmlNamespaceResolver16.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver16.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver16.Add("settings", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver16.Add("syncfusion", "clr-namespace:Syncfusion.XForms.Buttons;assembly=Syncfusion.Buttons.XForms");
			xmlNamespaceResolver16.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider16.Add(typeFromHandle32, new XamlTypeResolver(xmlNamespaceResolver16, typeof(DTCv2Page).GetTypeInfo().Assembly));
			xamlServiceProvider16.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(157, 37)));
			DynamicResource dynamicResource10 = markupExtension16.ProvideValue(xamlServiceProvider16);
			radioButtonWithColor.SetDynamicResource(RadioButton.BorderColorProperty, dynamicResource10.Key);
			translate6.Text = "dtc_FoundUnits";
			IMarkupExtension markupExtension17 = translate6;
			XamlServiceProvider xamlServiceProvider17 = new XamlServiceProvider();
			Type typeFromHandle33 = typeof(IProvideValueTarget);
			object[] array18 = new object[0 + 7];
			array18[0] = radioButtonWithColor;
			array18[1] = grid2;
			array18[2] = stackLayout;
			array18[3] = listView;
			array18[4] = grid4;
			array18[5] = grid8;
			array18[6] = this;
			object obj23;
			xamlServiceProvider17.Add(typeFromHandle33, obj23 = new SimpleValueTargetProvider(array18, RadioButton.ContentProperty, nameScope));
			xamlServiceProvider17.Add(typeof(IReferenceProvider), obj23);
			Type typeFromHandle34 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver17 = new XmlNamespaceResolver();
			xmlNamespaceResolver17.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver17.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver17.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver17.Add("dtcv2", "clr-namespace:CarScannerXamarinForms.DTCv2");
			xmlNamespaceResolver17.Add("ecuModels", "clr-namespace:CarScannerXamarinForms.ECUModels");
			xmlNamespaceResolver17.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver17.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver17.Add("settings", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver17.Add("syncfusion", "clr-namespace:Syncfusion.XForms.Buttons;assembly=Syncfusion.Buttons.XForms");
			xmlNamespaceResolver17.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider17.Add(typeFromHandle34, new XamlTypeResolver(xmlNamespaceResolver17, typeof(DTCv2Page).GetTypeInfo().Assembly));
			xamlServiceProvider17.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(158, 37)));
			object obj24 = markupExtension17.ProvideValue(xamlServiceProvider17);
			radioButtonWithColor.SetValue(RadioButton.ContentProperty, obj24);
			staticResourceExtension2.Key = "TrueValue";
			IMarkupExtension markupExtension18 = staticResourceExtension2;
			XamlServiceProvider xamlServiceProvider18 = new XamlServiceProvider();
			Type typeFromHandle35 = typeof(IProvideValueTarget);
			object[] array19 = new object[0 + 7];
			array19[0] = radioButtonWithColor;
			array19[1] = grid2;
			array19[2] = stackLayout;
			array19[3] = listView;
			array19[4] = grid4;
			array19[5] = grid8;
			array19[6] = this;
			object obj25;
			xamlServiceProvider18.Add(typeFromHandle35, obj25 = new SimpleValueTargetProvider(array19, RadioButton.ValueProperty, nameScope));
			xamlServiceProvider18.Add(typeof(IReferenceProvider), obj25);
			Type typeFromHandle36 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver18 = new XmlNamespaceResolver();
			xmlNamespaceResolver18.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver18.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver18.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver18.Add("dtcv2", "clr-namespace:CarScannerXamarinForms.DTCv2");
			xmlNamespaceResolver18.Add("ecuModels", "clr-namespace:CarScannerXamarinForms.ECUModels");
			xmlNamespaceResolver18.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver18.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver18.Add("settings", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver18.Add("syncfusion", "clr-namespace:Syncfusion.XForms.Buttons;assembly=Syncfusion.Buttons.XForms");
			xmlNamespaceResolver18.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider18.Add(typeFromHandle36, new XamlTypeResolver(xmlNamespaceResolver18, typeof(DTCv2Page).GetTypeInfo().Assembly));
			xamlServiceProvider18.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(159, 37)));
			object obj26 = markupExtension18.ProvideValue(xamlServiceProvider18);
			radioButtonWithColor.SetValue(RadioButton.ValueProperty, obj26);
			grid2.Children.Add(radioButtonWithColor);
			radioButtonWithColor2.SetValue(Grid.ColumnProperty, 1);
			dynamicResourceExtension11.Key = "BackgroundColor";
			IMarkupExtension<DynamicResource> markupExtension19 = dynamicResourceExtension11;
			XamlServiceProvider xamlServiceProvider19 = new XamlServiceProvider();
			Type typeFromHandle37 = typeof(IProvideValueTarget);
			object[] array20 = new object[0 + 7];
			array20[0] = radioButtonWithColor2;
			array20[1] = grid2;
			array20[2] = stackLayout;
			array20[3] = listView;
			array20[4] = grid4;
			array20[5] = grid8;
			array20[6] = this;
			object obj27;
			xamlServiceProvider19.Add(typeFromHandle37, obj27 = new SimpleValueTargetProvider(array20, VisualElement.BackgroundColorProperty, nameScope));
			xamlServiceProvider19.Add(typeof(IReferenceProvider), obj27);
			Type typeFromHandle38 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver19 = new XmlNamespaceResolver();
			xmlNamespaceResolver19.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver19.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver19.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver19.Add("dtcv2", "clr-namespace:CarScannerXamarinForms.DTCv2");
			xmlNamespaceResolver19.Add("ecuModels", "clr-namespace:CarScannerXamarinForms.ECUModels");
			xmlNamespaceResolver19.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver19.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver19.Add("settings", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver19.Add("syncfusion", "clr-namespace:Syncfusion.XForms.Buttons;assembly=Syncfusion.Buttons.XForms");
			xmlNamespaceResolver19.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider19.Add(typeFromHandle38, new XamlTypeResolver(xmlNamespaceResolver19, typeof(DTCv2Page).GetTypeInfo().Assembly));
			xamlServiceProvider19.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(163, 37)));
			DynamicResource dynamicResource11 = markupExtension19.ProvideValue(xamlServiceProvider19);
			radioButtonWithColor2.SetDynamicResource(VisualElement.BackgroundColorProperty, dynamicResource11.Key);
			dynamicResourceExtension12.Key = "ButtonAccentColor";
			IMarkupExtension<DynamicResource> markupExtension20 = dynamicResourceExtension12;
			XamlServiceProvider xamlServiceProvider20 = new XamlServiceProvider();
			Type typeFromHandle39 = typeof(IProvideValueTarget);
			object[] array21 = new object[0 + 7];
			array21[0] = radioButtonWithColor2;
			array21[1] = grid2;
			array21[2] = stackLayout;
			array21[3] = listView;
			array21[4] = grid4;
			array21[5] = grid8;
			array21[6] = this;
			object obj28;
			xamlServiceProvider20.Add(typeFromHandle39, obj28 = new SimpleValueTargetProvider(array21, RadioButton.BorderColorProperty, nameScope));
			xamlServiceProvider20.Add(typeof(IReferenceProvider), obj28);
			Type typeFromHandle40 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver20 = new XmlNamespaceResolver();
			xmlNamespaceResolver20.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver20.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver20.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver20.Add("dtcv2", "clr-namespace:CarScannerXamarinForms.DTCv2");
			xmlNamespaceResolver20.Add("ecuModels", "clr-namespace:CarScannerXamarinForms.ECUModels");
			xmlNamespaceResolver20.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver20.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver20.Add("settings", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver20.Add("syncfusion", "clr-namespace:Syncfusion.XForms.Buttons;assembly=Syncfusion.Buttons.XForms");
			xmlNamespaceResolver20.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider20.Add(typeFromHandle40, new XamlTypeResolver(xmlNamespaceResolver20, typeof(DTCv2Page).GetTypeInfo().Assembly));
			xamlServiceProvider20.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(164, 37)));
			DynamicResource dynamicResource12 = markupExtension20.ProvideValue(xamlServiceProvider20);
			radioButtonWithColor2.SetDynamicResource(RadioButton.BorderColorProperty, dynamicResource12.Key);
			translate7.Text = "dtc_AllUnits";
			IMarkupExtension markupExtension21 = translate7;
			XamlServiceProvider xamlServiceProvider21 = new XamlServiceProvider();
			Type typeFromHandle41 = typeof(IProvideValueTarget);
			object[] array22 = new object[0 + 7];
			array22[0] = radioButtonWithColor2;
			array22[1] = grid2;
			array22[2] = stackLayout;
			array22[3] = listView;
			array22[4] = grid4;
			array22[5] = grid8;
			array22[6] = this;
			object obj29;
			xamlServiceProvider21.Add(typeFromHandle41, obj29 = new SimpleValueTargetProvider(array22, RadioButton.ContentProperty, nameScope));
			xamlServiceProvider21.Add(typeof(IReferenceProvider), obj29);
			Type typeFromHandle42 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver21 = new XmlNamespaceResolver();
			xmlNamespaceResolver21.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver21.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver21.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver21.Add("dtcv2", "clr-namespace:CarScannerXamarinForms.DTCv2");
			xmlNamespaceResolver21.Add("ecuModels", "clr-namespace:CarScannerXamarinForms.ECUModels");
			xmlNamespaceResolver21.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver21.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver21.Add("settings", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver21.Add("syncfusion", "clr-namespace:Syncfusion.XForms.Buttons;assembly=Syncfusion.Buttons.XForms");
			xmlNamespaceResolver21.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider21.Add(typeFromHandle42, new XamlTypeResolver(xmlNamespaceResolver21, typeof(DTCv2Page).GetTypeInfo().Assembly));
			xamlServiceProvider21.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(165, 37)));
			object obj30 = markupExtension21.ProvideValue(xamlServiceProvider21);
			radioButtonWithColor2.SetValue(RadioButton.ContentProperty, obj30);
			staticResourceExtension3.Key = "FalseValue";
			IMarkupExtension markupExtension22 = staticResourceExtension3;
			XamlServiceProvider xamlServiceProvider22 = new XamlServiceProvider();
			Type typeFromHandle43 = typeof(IProvideValueTarget);
			object[] array23 = new object[0 + 7];
			array23[0] = radioButtonWithColor2;
			array23[1] = grid2;
			array23[2] = stackLayout;
			array23[3] = listView;
			array23[4] = grid4;
			array23[5] = grid8;
			array23[6] = this;
			object obj31;
			xamlServiceProvider22.Add(typeFromHandle43, obj31 = new SimpleValueTargetProvider(array23, RadioButton.ValueProperty, nameScope));
			xamlServiceProvider22.Add(typeof(IReferenceProvider), obj31);
			Type typeFromHandle44 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver22 = new XmlNamespaceResolver();
			xmlNamespaceResolver22.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver22.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver22.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver22.Add("dtcv2", "clr-namespace:CarScannerXamarinForms.DTCv2");
			xmlNamespaceResolver22.Add("ecuModels", "clr-namespace:CarScannerXamarinForms.ECUModels");
			xmlNamespaceResolver22.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver22.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver22.Add("settings", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver22.Add("syncfusion", "clr-namespace:Syncfusion.XForms.Buttons;assembly=Syncfusion.Buttons.XForms");
			xmlNamespaceResolver22.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider22.Add(typeFromHandle44, new XamlTypeResolver(xmlNamespaceResolver22, typeof(DTCv2Page).GetTypeInfo().Assembly));
			xamlServiceProvider22.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(166, 37)));
			object obj32 = markupExtension22.ProvideValue(xamlServiceProvider22);
			radioButtonWithColor2.SetValue(RadioButton.ValueProperty, obj32);
			grid2.Children.Add(radioButtonWithColor2);
			stackLayout.Children.Add(grid2);
			listView.SetValue(ListView.HeaderProperty, stackLayout);
			stackLayout2.SetValue(StackLayout.OrientationProperty, 0);
			label3.SetValue(Label.FontAttributesProperty, new FontAttributesConverter().ConvertFromInvariantString("Bold"));
			translate8.Text = "dtc_FilteringOptions";
			IMarkupExtension markupExtension23 = translate8;
			XamlServiceProvider xamlServiceProvider23 = new XamlServiceProvider();
			Type typeFromHandle45 = typeof(IProvideValueTarget);
			object[] array24 = new object[0 + 6];
			array24[0] = label3;
			array24[1] = stackLayout2;
			array24[2] = listView;
			array24[3] = grid4;
			array24[4] = grid8;
			array24[5] = this;
			object obj33;
			xamlServiceProvider23.Add(typeFromHandle45, obj33 = new SimpleValueTargetProvider(array24, Label.TextProperty, nameScope));
			xamlServiceProvider23.Add(typeof(IReferenceProvider), obj33);
			Type typeFromHandle46 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver23 = new XmlNamespaceResolver();
			xmlNamespaceResolver23.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver23.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver23.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver23.Add("dtcv2", "clr-namespace:CarScannerXamarinForms.DTCv2");
			xmlNamespaceResolver23.Add("ecuModels", "clr-namespace:CarScannerXamarinForms.ECUModels");
			xmlNamespaceResolver23.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver23.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver23.Add("settings", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver23.Add("syncfusion", "clr-namespace:Syncfusion.XForms.Buttons;assembly=Syncfusion.Buttons.XForms");
			xmlNamespaceResolver23.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider23.Add(typeFromHandle46, new XamlTypeResolver(xmlNamespaceResolver23, typeof(DTCv2Page).GetTypeInfo().Assembly));
			xamlServiceProvider23.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(176, 58)));
			object obj34 = markupExtension23.ProvideValue(xamlServiceProvider23);
			label3.Text = obj34;
			stackLayout2.Children.Add(label3);
			labelSwitch.SetValue(Grid.RowProperty, 0);
			labelSwitch.SetValue(BindableObject.BindingContextProperty, sharedSettings);
			bindingExtension13.Mode = 1;
			bindingExtension13.Path = "HideArchiveDTC";
			bindingExtension13.TypedBinding = new TypedBinding<SharedSettings, bool>(delegate(SharedSettings A_0)
			{
				if (A_0 != null)
				{
					return new ValueTuple<bool, bool>(A_0.HideArchiveDTC, true);
				}
				return default(ValueTuple<bool, bool>);
			}, delegate(SharedSettings A_0, bool A_1)
			{
				if (A_0 != null)
				{
					A_0.HideArchiveDTC = A_1;
					return;
				}
			}, new Tuple<Func<SharedSettings, object>, string>[]
			{
				new Tuple<Func<SharedSettings, object>, string>((SharedSettings A_0) => A_0, "HideArchiveDTC")
			});
			BindingBase bindingBase13 = bindingExtension13.ProvideValue(null);
			labelSwitch.SetBinding(LabelSwitch.IsToggledProperty, bindingBase13);
			translate9.Text = "ios_SkipArchiveDTC";
			IMarkupExtension markupExtension24 = translate9;
			XamlServiceProvider xamlServiceProvider24 = new XamlServiceProvider();
			Type typeFromHandle47 = typeof(IProvideValueTarget);
			object[] array25 = new object[0 + 6];
			array25[0] = labelSwitch;
			array25[1] = stackLayout2;
			array25[2] = listView;
			array25[3] = grid4;
			array25[4] = grid8;
			array25[5] = this;
			object obj35;
			xamlServiceProvider24.Add(typeFromHandle47, obj35 = new SimpleValueTargetProvider(array25, LabelSwitch.TextProperty, nameScope));
			xamlServiceProvider24.Add(typeof(IReferenceProvider), obj35);
			Type typeFromHandle48 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver24 = new XmlNamespaceResolver();
			xmlNamespaceResolver24.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver24.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver24.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver24.Add("dtcv2", "clr-namespace:CarScannerXamarinForms.DTCv2");
			xmlNamespaceResolver24.Add("ecuModels", "clr-namespace:CarScannerXamarinForms.ECUModels");
			xmlNamespaceResolver24.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver24.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver24.Add("settings", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver24.Add("syncfusion", "clr-namespace:Syncfusion.XForms.Buttons;assembly=Syncfusion.Buttons.XForms");
			xmlNamespaceResolver24.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider24.Add(typeFromHandle48, new XamlTypeResolver(xmlNamespaceResolver24, typeof(DTCv2Page).GetTypeInfo().Assembly));
			xamlServiceProvider24.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(183, 33)));
			object obj36 = markupExtension24.ProvideValue(xamlServiceProvider24);
			labelSwitch.Text = obj36;
			stackLayout2.Children.Add(labelSwitch);
			labelSwitch2.SetValue(Grid.RowProperty, 1);
			labelSwitch2.SetValue(BindableObject.BindingContextProperty, sharedSettings2);
			bindingExtension14.Mode = 1;
			bindingExtension14.Path = "HideDTCWithUncomplitedTests";
			bindingExtension14.TypedBinding = new TypedBinding<SharedSettings, bool>(delegate(SharedSettings A_0)
			{
				if (A_0 != null)
				{
					return new ValueTuple<bool, bool>(A_0.HideDTCWithUncomplitedTests, true);
				}
				return default(ValueTuple<bool, bool>);
			}, delegate(SharedSettings A_0, bool A_1)
			{
				if (A_0 != null)
				{
					A_0.HideDTCWithUncomplitedTests = A_1;
					return;
				}
			}, new Tuple<Func<SharedSettings, object>, string>[]
			{
				new Tuple<Func<SharedSettings, object>, string>((SharedSettings A_0) => A_0, "HideDTCWithUncomplitedTests")
			});
			BindingBase bindingBase14 = bindingExtension14.ProvideValue(null);
			labelSwitch2.SetBinding(LabelSwitch.IsToggledProperty, bindingBase14);
			translate10.Text = "ios_HideDTCWithUncomplitedTests";
			IMarkupExtension markupExtension25 = translate10;
			XamlServiceProvider xamlServiceProvider25 = new XamlServiceProvider();
			Type typeFromHandle49 = typeof(IProvideValueTarget);
			object[] array26 = new object[0 + 6];
			array26[0] = labelSwitch2;
			array26[1] = stackLayout2;
			array26[2] = listView;
			array26[3] = grid4;
			array26[4] = grid8;
			array26[5] = this;
			object obj37;
			xamlServiceProvider25.Add(typeFromHandle49, obj37 = new SimpleValueTargetProvider(array26, LabelSwitch.TextProperty, nameScope));
			xamlServiceProvider25.Add(typeof(IReferenceProvider), obj37);
			Type typeFromHandle50 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver25 = new XmlNamespaceResolver();
			xmlNamespaceResolver25.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver25.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver25.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver25.Add("dtcv2", "clr-namespace:CarScannerXamarinForms.DTCv2");
			xmlNamespaceResolver25.Add("ecuModels", "clr-namespace:CarScannerXamarinForms.ECUModels");
			xmlNamespaceResolver25.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver25.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver25.Add("settings", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver25.Add("syncfusion", "clr-namespace:Syncfusion.XForms.Buttons;assembly=Syncfusion.Buttons.XForms");
			xmlNamespaceResolver25.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider25.Add(typeFromHandle50, new XamlTypeResolver(xmlNamespaceResolver25, typeof(DTCv2Page).GetTypeInfo().Assembly));
			xamlServiceProvider25.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(190, 33)));
			object obj38 = markupExtension25.ProvideValue(xamlServiceProvider25);
			labelSwitch2.Text = obj38;
			stackLayout2.Children.Add(labelSwitch2);
			button.Clicked += this.btnToOldStyle_Clicked;
			translate11.Text = "dtc_UseOldStyle";
			IMarkupExtension markupExtension26 = translate11;
			XamlServiceProvider xamlServiceProvider26 = new XamlServiceProvider();
			Type typeFromHandle51 = typeof(IProvideValueTarget);
			object[] array27 = new object[0 + 6];
			array27[0] = button;
			array27[1] = stackLayout2;
			array27[2] = listView;
			array27[3] = grid4;
			array27[4] = grid8;
			array27[5] = this;
			object obj39;
			xamlServiceProvider26.Add(typeFromHandle51, obj39 = new SimpleValueTargetProvider(array27, Button.TextProperty, nameScope));
			xamlServiceProvider26.Add(typeof(IReferenceProvider), obj39);
			Type typeFromHandle52 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver26 = new XmlNamespaceResolver();
			xmlNamespaceResolver26.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver26.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver26.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver26.Add("dtcv2", "clr-namespace:CarScannerXamarinForms.DTCv2");
			xmlNamespaceResolver26.Add("ecuModels", "clr-namespace:CarScannerXamarinForms.ECUModels");
			xmlNamespaceResolver26.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver26.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver26.Add("settings", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver26.Add("syncfusion", "clr-namespace:Syncfusion.XForms.Buttons;assembly=Syncfusion.Buttons.XForms");
			xmlNamespaceResolver26.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider26.Add(typeFromHandle52, new XamlTypeResolver(xmlNamespaceResolver26, typeof(DTCv2Page).GetTypeInfo().Assembly));
			xamlServiceProvider26.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(194, 33)));
			object obj40 = markupExtension26.ProvideValue(xamlServiceProvider26);
			button.Text = obj40;
			stackLayout2.Children.Add(button);
			listView.SetValue(ListView.FooterProperty, stackLayout2);
			grid4.Children.Add(listView);
			grid3.SetValue(Grid.RowProperty, 2);
			columnDefinition5.SetValue(ColumnDefinition.WidthProperty, new GridLengthTypeConverter().ConvertFromInvariantString("*"));
			grid3.GetValue(Grid.ColumnDefinitionsProperty).Add(columnDefinition5);
			columnDefinition6.SetValue(ColumnDefinition.WidthProperty, new GridLengthTypeConverter().ConvertFromInvariantString("*"));
			grid3.GetValue(Grid.ColumnDefinitionsProperty).Add(columnDefinition6);
			button2.SetValue(Grid.ColumnProperty, 0);
			bindingExtension15.Path = "ReadDTCCommand";
			BindingBase bindingBase15 = bindingExtension15.ProvideValue(null);
			button2.SetBinding(Button.CommandProperty, bindingBase15);
			button2.SetValue(VisualElement.IsEnabledProperty, false);
			translate12.Text = "DtcPage_btnRead.Content";
			IMarkupExtension markupExtension27 = translate12;
			XamlServiceProvider xamlServiceProvider27 = new XamlServiceProvider();
			Type typeFromHandle53 = typeof(IProvideValueTarget);
			object[] array28 = new object[0 + 5];
			array28[0] = button2;
			array28[1] = grid3;
			array28[2] = grid4;
			array28[3] = grid8;
			array28[4] = this;
			object obj41;
			xamlServiceProvider27.Add(typeFromHandle53, obj41 = new SimpleValueTargetProvider(array28, Button.TextProperty, nameScope));
			xamlServiceProvider27.Add(typeof(IReferenceProvider), obj41);
			Type typeFromHandle54 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver27 = new XmlNamespaceResolver();
			xmlNamespaceResolver27.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver27.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver27.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver27.Add("dtcv2", "clr-namespace:CarScannerXamarinForms.DTCv2");
			xmlNamespaceResolver27.Add("ecuModels", "clr-namespace:CarScannerXamarinForms.ECUModels");
			xmlNamespaceResolver27.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver27.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver27.Add("settings", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver27.Add("syncfusion", "clr-namespace:Syncfusion.XForms.Buttons;assembly=Syncfusion.Buttons.XForms");
			xmlNamespaceResolver27.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider27.Add(typeFromHandle54, new XamlTypeResolver(xmlNamespaceResolver27, typeof(DTCv2Page).GetTypeInfo().Assembly));
			xamlServiceProvider27.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(208, 25)));
			object obj42 = markupExtension27.ProvideValue(xamlServiceProvider27);
			button2.Text = obj42;
			grid3.Children.Add(button2);
			button3.SetValue(Grid.ColumnProperty, 1);
			bindingExtension16.Path = "ClearDTCCommand";
			BindingBase bindingBase16 = bindingExtension16.ProvideValue(null);
			button3.SetBinding(Button.CommandProperty, bindingBase16);
			button3.SetValue(VisualElement.IsEnabledProperty, false);
			translate13.Text = "DtcPage_btnClear.Content";
			IMarkupExtension markupExtension28 = translate13;
			XamlServiceProvider xamlServiceProvider28 = new XamlServiceProvider();
			Type typeFromHandle55 = typeof(IProvideValueTarget);
			object[] array29 = new object[0 + 5];
			array29[0] = button3;
			array29[1] = grid3;
			array29[2] = grid4;
			array29[3] = grid8;
			array29[4] = this;
			object obj43;
			xamlServiceProvider28.Add(typeFromHandle55, obj43 = new SimpleValueTargetProvider(array29, Button.TextProperty, nameScope));
			xamlServiceProvider28.Add(typeof(IReferenceProvider), obj43);
			Type typeFromHandle56 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver28 = new XmlNamespaceResolver();
			xmlNamespaceResolver28.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver28.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver28.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver28.Add("dtcv2", "clr-namespace:CarScannerXamarinForms.DTCv2");
			xmlNamespaceResolver28.Add("ecuModels", "clr-namespace:CarScannerXamarinForms.ECUModels");
			xmlNamespaceResolver28.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver28.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver28.Add("settings", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver28.Add("syncfusion", "clr-namespace:Syncfusion.XForms.Buttons;assembly=Syncfusion.Buttons.XForms");
			xmlNamespaceResolver28.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider28.Add(typeFromHandle56, new XamlTypeResolver(xmlNamespaceResolver28, typeof(DTCv2Page).GetTypeInfo().Assembly));
			xamlServiceProvider28.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(214, 25)));
			object obj44 = markupExtension28.ProvideValue(xamlServiceProvider28);
			button3.Text = obj44;
			grid3.Children.Add(button3);
			grid4.Children.Add(grid3);
			activityFrame.SetValue(Grid.RowProperty, 1);
			activityFrame.SetValue(View.MarginProperty, new Thickness(0.0));
			bindingExtension17.Path = "IsBusy";
			BindingBase bindingBase17 = bindingExtension17.ProvideValue(null);
			activityFrame.SetBinding(VisualElement.IsVisibleProperty, bindingBase17);
			bindingExtension18.Path = "StatusText";
			BindingBase bindingBase18 = bindingExtension18.ProvideValue(null);
			activityFrame.SetBinding(ActivityFrame.TextProperty, bindingBase18);
			activityFrame.SetValue(View.VerticalOptionsProperty, LayoutOptions.Center);
			grid4.Children.Add(activityFrame);
			grid8.Children.Add(grid4);
			grid7.SetValue(Grid.RowProperty, 0);
			bindingExtension19.Mode = 2;
			bindingExtension19.Path = "IsResults";
			BindingBase bindingBase19 = bindingExtension19.ProvideValue(null);
			grid7.SetBinding(VisualElement.IsVisibleProperty, bindingBase19);
			rowDefinition7.SetValue(RowDefinition.HeightProperty, new GridLengthTypeConverter().ConvertFromInvariantString("auto"));
			grid7.GetValue(Grid.RowDefinitionsProperty).Add(rowDefinition7);
			rowDefinition8.SetValue(RowDefinition.HeightProperty, new GridLengthTypeConverter().ConvertFromInvariantString("*"));
			grid7.GetValue(Grid.RowDefinitionsProperty).Add(rowDefinition8);
			rowDefinition9.SetValue(RowDefinition.HeightProperty, new GridLengthTypeConverter().ConvertFromInvariantString("auto"));
			grid7.GetValue(Grid.RowDefinitionsProperty).Add(rowDefinition9);
			rowDefinition10.SetValue(RowDefinition.HeightProperty, new GridLengthTypeConverter().ConvertFromInvariantString("auto"));
			grid7.GetValue(Grid.RowDefinitionsProperty).Add(rowDefinition10);
			rowDefinition11.SetValue(RowDefinition.HeightProperty, new GridLengthTypeConverter().ConvertFromInvariantString("auto"));
			grid7.GetValue(Grid.RowDefinitionsProperty).Add(rowDefinition11);
			frame.SetValue(Grid.RowProperty, 0);
			frame.SetValue(View.MarginProperty, new Thickness(0.0, 0.0, 0.0, 5.0));
			frame.SetValue(Layout.PaddingProperty, new Thickness(5.0));
			dynamicResourceExtension13.Key = "BackgroundColor";
			IMarkupExtension<DynamicResource> markupExtension29 = dynamicResourceExtension13;
			XamlServiceProvider xamlServiceProvider29 = new XamlServiceProvider();
			Type typeFromHandle57 = typeof(IProvideValueTarget);
			object[] array30 = new object[0 + 4];
			array30[0] = frame;
			array30[1] = grid7;
			array30[2] = grid8;
			array30[3] = this;
			object obj45;
			xamlServiceProvider29.Add(typeFromHandle57, obj45 = new SimpleValueTargetProvider(array30, VisualElement.BackgroundColorProperty, nameScope));
			xamlServiceProvider29.Add(typeof(IReferenceProvider), obj45);
			Type typeFromHandle58 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver29 = new XmlNamespaceResolver();
			xmlNamespaceResolver29.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver29.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver29.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver29.Add("dtcv2", "clr-namespace:CarScannerXamarinForms.DTCv2");
			xmlNamespaceResolver29.Add("ecuModels", "clr-namespace:CarScannerXamarinForms.ECUModels");
			xmlNamespaceResolver29.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver29.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver29.Add("settings", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver29.Add("syncfusion", "clr-namespace:Syncfusion.XForms.Buttons;assembly=Syncfusion.Buttons.XForms");
			xmlNamespaceResolver29.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider29.Add(typeFromHandle58, new XamlTypeResolver(xmlNamespaceResolver29, typeof(DTCv2Page).GetTypeInfo().Assembly));
			xamlServiceProvider29.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(246, 21)));
			DynamicResource dynamicResource13 = markupExtension29.ProvideValue(xamlServiceProvider29);
			frame.SetDynamicResource(VisualElement.BackgroundColorProperty, dynamicResource13.Key);
			dynamicResourceExtension14.Key = "TextColor";
			IMarkupExtension<DynamicResource> markupExtension30 = dynamicResourceExtension14;
			XamlServiceProvider xamlServiceProvider30 = new XamlServiceProvider();
			Type typeFromHandle59 = typeof(IProvideValueTarget);
			object[] array31 = new object[0 + 4];
			array31[0] = frame;
			array31[1] = grid7;
			array31[2] = grid8;
			array31[3] = this;
			object obj46;
			xamlServiceProvider30.Add(typeFromHandle59, obj46 = new SimpleValueTargetProvider(array31, Frame.BorderColorProperty, nameScope));
			xamlServiceProvider30.Add(typeof(IReferenceProvider), obj46);
			Type typeFromHandle60 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver30 = new XmlNamespaceResolver();
			xmlNamespaceResolver30.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver30.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver30.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver30.Add("dtcv2", "clr-namespace:CarScannerXamarinForms.DTCv2");
			xmlNamespaceResolver30.Add("ecuModels", "clr-namespace:CarScannerXamarinForms.ECUModels");
			xmlNamespaceResolver30.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver30.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver30.Add("settings", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver30.Add("syncfusion", "clr-namespace:Syncfusion.XForms.Buttons;assembly=Syncfusion.Buttons.XForms");
			xmlNamespaceResolver30.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider30.Add(typeFromHandle60, new XamlTypeResolver(xmlNamespaceResolver30, typeof(DTCv2Page).GetTypeInfo().Assembly));
			xamlServiceProvider30.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(247, 21)));
			DynamicResource dynamicResource14 = markupExtension30.ProvideValue(xamlServiceProvider30);
			frame.SetDynamicResource(Frame.BorderColorProperty, dynamicResource14.Key);
			frame.SetValue(Frame.HasShadowProperty, false);
			bindingExtension20.Mode = 2;
			bindingExtension20.Path = "IsFilterVisible";
			BindingBase bindingBase20 = bindingExtension20.ProvideValue(null);
			frame.SetBinding(VisualElement.IsVisibleProperty, bindingBase20);
			stackLayout3.SetValue(StackLayout.OrientationProperty, 0);
			labelSwitch3.SetValue(BindableObject.BindingContextProperty, sharedSettings3);
			bindingExtension21.Mode = 1;
			bindingExtension21.Path = "HideArchiveDTC";
			BindingBase bindingBase21 = bindingExtension21.ProvideValue(null);
			labelSwitch3.SetBinding(LabelSwitch.IsToggledProperty, bindingBase21);
			translate14.Text = "ios_SkipArchiveDTC";
			IMarkupExtension markupExtension31 = translate14;
			XamlServiceProvider xamlServiceProvider31 = new XamlServiceProvider();
			Type typeFromHandle61 = typeof(IProvideValueTarget);
			object[] array32 = new object[0 + 6];
			array32[0] = labelSwitch3;
			array32[1] = stackLayout3;
			array32[2] = frame;
			array32[3] = grid7;
			array32[4] = grid8;
			array32[5] = this;
			object obj47;
			xamlServiceProvider31.Add(typeFromHandle61, obj47 = new SimpleValueTargetProvider(array32, LabelSwitch.TextProperty, nameScope));
			xamlServiceProvider31.Add(typeof(IReferenceProvider), obj47);
			Type typeFromHandle62 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver31 = new XmlNamespaceResolver();
			xmlNamespaceResolver31.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver31.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver31.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver31.Add("dtcv2", "clr-namespace:CarScannerXamarinForms.DTCv2");
			xmlNamespaceResolver31.Add("ecuModels", "clr-namespace:CarScannerXamarinForms.ECUModels");
			xmlNamespaceResolver31.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver31.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver31.Add("settings", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver31.Add("syncfusion", "clr-namespace:Syncfusion.XForms.Buttons;assembly=Syncfusion.Buttons.XForms");
			xmlNamespaceResolver31.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider31.Add(typeFromHandle62, new XamlTypeResolver(xmlNamespaceResolver31, typeof(DTCv2Page).GetTypeInfo().Assembly));
			xamlServiceProvider31.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(254, 29)));
			object obj48 = markupExtension31.ProvideValue(xamlServiceProvider31);
			labelSwitch3.Text = obj48;
			stackLayout3.Children.Add(labelSwitch3);
			labelSwitch4.SetValue(BindableObject.BindingContextProperty, sharedSettings4);
			bindingExtension22.Mode = 1;
			bindingExtension22.Path = "HideDTCWithUncomplitedTests";
			BindingBase bindingBase22 = bindingExtension22.ProvideValue(null);
			labelSwitch4.SetBinding(LabelSwitch.IsToggledProperty, bindingBase22);
			translate15.Text = "ios_HideDTCWithUncomplitedTests";
			IMarkupExtension markupExtension32 = translate15;
			XamlServiceProvider xamlServiceProvider32 = new XamlServiceProvider();
			Type typeFromHandle63 = typeof(IProvideValueTarget);
			object[] array33 = new object[0 + 6];
			array33[0] = labelSwitch4;
			array33[1] = stackLayout3;
			array33[2] = frame;
			array33[3] = grid7;
			array33[4] = grid8;
			array33[5] = this;
			object obj49;
			xamlServiceProvider32.Add(typeFromHandle63, obj49 = new SimpleValueTargetProvider(array33, LabelSwitch.TextProperty, nameScope));
			xamlServiceProvider32.Add(typeof(IReferenceProvider), obj49);
			Type typeFromHandle64 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver32 = new XmlNamespaceResolver();
			xmlNamespaceResolver32.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver32.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver32.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver32.Add("dtcv2", "clr-namespace:CarScannerXamarinForms.DTCv2");
			xmlNamespaceResolver32.Add("ecuModels", "clr-namespace:CarScannerXamarinForms.ECUModels");
			xmlNamespaceResolver32.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver32.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver32.Add("settings", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver32.Add("syncfusion", "clr-namespace:Syncfusion.XForms.Buttons;assembly=Syncfusion.Buttons.XForms");
			xmlNamespaceResolver32.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider32.Add(typeFromHandle64, new XamlTypeResolver(xmlNamespaceResolver32, typeof(DTCv2Page).GetTypeInfo().Assembly));
			xamlServiceProvider32.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(258, 29)));
			object obj50 = markupExtension32.ProvideValue(xamlServiceProvider32);
			labelSwitch4.Text = obj50;
			stackLayout3.Children.Add(labelSwitch4);
			frame.SetValue(ContentView.ContentProperty, stackLayout3);
			grid7.Children.Add(frame);
			stackLayout4.SetValue(Grid.RowProperty, 1);
			bindingExtension23.Mode = 2;
			staticResourceExtension4.Key = "BoolToNegativeConverter";
			IMarkupExtension markupExtension33 = staticResourceExtension4;
			XamlServiceProvider xamlServiceProvider33 = new XamlServiceProvider();
			Type typeFromHandle65 = typeof(IProvideValueTarget);
			object[] array34 = new object[0 + 5];
			array34[0] = bindingExtension23;
			array34[1] = stackLayout4;
			array34[2] = grid7;
			array34[3] = grid8;
			array34[4] = this;
			object obj51;
			xamlServiceProvider33.Add(typeFromHandle65, obj51 = new SimpleValueTargetProvider(array34, typeof(BindingExtension).GetRuntimeProperty("Converter"), nameScope));
			xamlServiceProvider33.Add(typeof(IReferenceProvider), obj51);
			Type typeFromHandle66 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver33 = new XmlNamespaceResolver();
			xmlNamespaceResolver33.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver33.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver33.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver33.Add("dtcv2", "clr-namespace:CarScannerXamarinForms.DTCv2");
			xmlNamespaceResolver33.Add("ecuModels", "clr-namespace:CarScannerXamarinForms.ECUModels");
			xmlNamespaceResolver33.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver33.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver33.Add("settings", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver33.Add("syncfusion", "clr-namespace:Syncfusion.XForms.Buttons;assembly=Syncfusion.Buttons.XForms");
			xmlNamespaceResolver33.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider33.Add(typeFromHandle66, new XamlTypeResolver(xmlNamespaceResolver33, typeof(DTCv2Page).GetTypeInfo().Assembly));
			xamlServiceProvider33.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(264, 21)));
			object obj52 = markupExtension33.ProvideValue(xamlServiceProvider33);
			bindingExtension23.Converter = obj52;
			bindingExtension23.Path = "HasDTCToDisplay";
			BindingBase bindingBase23 = bindingExtension23.ProvideValue(null);
			stackLayout4.SetBinding(VisualElement.IsVisibleProperty, bindingBase23);
			stackLayout4.SetValue(StackLayout.OrientationProperty, 0);
			label4.SetValue(Label.FontAttributesProperty, new FontAttributesConverter().ConvertFromInvariantString("Bold"));
			translate16.Text = "ios_NoDTC";
			IMarkupExtension markupExtension34 = translate16;
			XamlServiceProvider xamlServiceProvider34 = new XamlServiceProvider();
			Type typeFromHandle67 = typeof(IProvideValueTarget);
			object[] array35 = new object[0 + 5];
			array35[0] = label4;
			array35[1] = stackLayout4;
			array35[2] = grid7;
			array35[3] = grid8;
			array35[4] = this;
			object obj53;
			xamlServiceProvider34.Add(typeFromHandle67, obj53 = new SimpleValueTargetProvider(array35, Label.TextProperty, nameScope));
			xamlServiceProvider34.Add(typeof(IReferenceProvider), obj53);
			Type typeFromHandle68 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver34 = new XmlNamespaceResolver();
			xmlNamespaceResolver34.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver34.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver34.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver34.Add("dtcv2", "clr-namespace:CarScannerXamarinForms.DTCv2");
			xmlNamespaceResolver34.Add("ecuModels", "clr-namespace:CarScannerXamarinForms.ECUModels");
			xmlNamespaceResolver34.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver34.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver34.Add("settings", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver34.Add("syncfusion", "clr-namespace:Syncfusion.XForms.Buttons;assembly=Syncfusion.Buttons.XForms");
			xmlNamespaceResolver34.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider34.Add(typeFromHandle68, new XamlTypeResolver(xmlNamespaceResolver34, typeof(DTCv2Page).GetTypeInfo().Assembly));
			xamlServiceProvider34.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(266, 50)));
			object obj54 = markupExtension34.ProvideValue(xamlServiceProvider34);
			label4.Text = obj54;
			stackLayout4.Children.Add(label4);
			translate17.Text = "dtc_NoDTCFilterHint";
			IMarkupExtension markupExtension35 = translate17;
			XamlServiceProvider xamlServiceProvider35 = new XamlServiceProvider();
			Type typeFromHandle69 = typeof(IProvideValueTarget);
			object[] array36 = new object[0 + 5];
			array36[0] = label5;
			array36[1] = stackLayout4;
			array36[2] = grid7;
			array36[3] = grid8;
			array36[4] = this;
			object obj55;
			xamlServiceProvider35.Add(typeFromHandle69, obj55 = new SimpleValueTargetProvider(array36, Label.TextProperty, nameScope));
			xamlServiceProvider35.Add(typeof(IReferenceProvider), obj55);
			Type typeFromHandle70 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver35 = new XmlNamespaceResolver();
			xmlNamespaceResolver35.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver35.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver35.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver35.Add("dtcv2", "clr-namespace:CarScannerXamarinForms.DTCv2");
			xmlNamespaceResolver35.Add("ecuModels", "clr-namespace:CarScannerXamarinForms.ECUModels");
			xmlNamespaceResolver35.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver35.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver35.Add("settings", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver35.Add("syncfusion", "clr-namespace:Syncfusion.XForms.Buttons;assembly=Syncfusion.Buttons.XForms");
			xmlNamespaceResolver35.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider35.Add(typeFromHandle70, new XamlTypeResolver(xmlNamespaceResolver35, typeof(DTCv2Page).GetTypeInfo().Assembly));
			xamlServiceProvider35.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(267, 28)));
			object obj56 = markupExtension35.ProvideValue(xamlServiceProvider35);
			label5.Text = obj56;
			stackLayout4.Children.Add(label5);
			label6.SetValue(Label.FontAttributesProperty, new FontAttributesConverter().ConvertFromInvariantString("Italic"));
			bindingExtension24.Mode = 2;
			bindingExtension24.Path = "BadELMDetected";
			BindingBase bindingBase24 = bindingExtension24.ProvideValue(null);
			label6.SetBinding(VisualElement.IsVisibleProperty, bindingBase24);
			translate18.Text = "dtc_BadELMDetected";
			IMarkupExtension markupExtension36 = translate18;
			XamlServiceProvider xamlServiceProvider36 = new XamlServiceProvider();
			Type typeFromHandle71 = typeof(IProvideValueTarget);
			object[] array37 = new object[0 + 5];
			array37[0] = label6;
			array37[1] = stackLayout4;
			array37[2] = grid7;
			array37[3] = grid8;
			array37[4] = this;
			object obj57;
			xamlServiceProvider36.Add(typeFromHandle71, obj57 = new SimpleValueTargetProvider(array37, Label.TextProperty, nameScope));
			xamlServiceProvider36.Add(typeof(IReferenceProvider), obj57);
			Type typeFromHandle72 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver36 = new XmlNamespaceResolver();
			xmlNamespaceResolver36.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver36.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver36.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver36.Add("dtcv2", "clr-namespace:CarScannerXamarinForms.DTCv2");
			xmlNamespaceResolver36.Add("ecuModels", "clr-namespace:CarScannerXamarinForms.ECUModels");
			xmlNamespaceResolver36.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver36.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver36.Add("settings", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver36.Add("syncfusion", "clr-namespace:Syncfusion.XForms.Buttons;assembly=Syncfusion.Buttons.XForms");
			xmlNamespaceResolver36.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider36.Add(typeFromHandle72, new XamlTypeResolver(xmlNamespaceResolver36, typeof(DTCv2Page).GetTypeInfo().Assembly));
			xamlServiceProvider36.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(271, 25)));
			object obj58 = markupExtension36.ProvideValue(xamlServiceProvider36);
			label6.Text = obj58;
			dynamicResourceExtension15.Key = "RedTextColor";
			IMarkupExtension<DynamicResource> markupExtension37 = dynamicResourceExtension15;
			XamlServiceProvider xamlServiceProvider37 = new XamlServiceProvider();
			Type typeFromHandle73 = typeof(IProvideValueTarget);
			object[] array38 = new object[0 + 5];
			array38[0] = label6;
			array38[1] = stackLayout4;
			array38[2] = grid7;
			array38[3] = grid8;
			array38[4] = this;
			object obj59;
			xamlServiceProvider37.Add(typeFromHandle73, obj59 = new SimpleValueTargetProvider(array38, Label.TextColorProperty, nameScope));
			xamlServiceProvider37.Add(typeof(IReferenceProvider), obj59);
			Type typeFromHandle74 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver37 = new XmlNamespaceResolver();
			xmlNamespaceResolver37.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver37.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver37.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver37.Add("dtcv2", "clr-namespace:CarScannerXamarinForms.DTCv2");
			xmlNamespaceResolver37.Add("ecuModels", "clr-namespace:CarScannerXamarinForms.ECUModels");
			xmlNamespaceResolver37.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver37.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver37.Add("settings", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver37.Add("syncfusion", "clr-namespace:Syncfusion.XForms.Buttons;assembly=Syncfusion.Buttons.XForms");
			xmlNamespaceResolver37.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider37.Add(typeFromHandle74, new XamlTypeResolver(xmlNamespaceResolver37, typeof(DTCv2Page).GetTypeInfo().Assembly));
			xamlServiceProvider37.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(272, 25)));
			DynamicResource dynamicResource15 = markupExtension37.ProvideValue(xamlServiceProvider37);
			label6.SetDynamicResource(Label.TextColorProperty, dynamicResource15.Key);
			tapGestureRecognizer4.SetValue(TapGestureRecognizer.NumberOfTapsRequiredProperty, 1);
			tapGestureRecognizer4.Tapped += this.LabelBadDTCTapGestureRecognizer_Tapped;
			label6.GestureRecognizers.Add(tapGestureRecognizer4);
			stackLayout4.Children.Add(label6);
			grid7.Children.Add(stackLayout4);
			listView2.SetValue(Grid.RowProperty, 1);
			listView2.SetValue(ListView.HasUnevenRowsProperty, true);
			listView2.SetValue(ListView.IsGroupingEnabledProperty, true);
			bindingExtension25.Mode = 2;
			bindingExtension25.Path = "HasDTCToDisplay";
			BindingBase bindingBase25 = bindingExtension25.ProvideValue(null);
			listView2.SetBinding(VisualElement.IsVisibleProperty, bindingBase25);
			listView2.ItemSelected += this.LvDTCs_ItemSelected;
			bindingExtension26.Path = "ECUListFound";
			BindingBase bindingBase26 = bindingExtension26.ProvideValue(null);
			listView2.SetBinding(ItemsView<Cell>.ItemsSourceProperty, bindingBase26);
			IDataTemplate dataTemplate5 = dataTemplate2;
			DTCv2Page.<InitializeComponent>_anonXamlCDataTemplate_27 <InitializeComponent>_anonXamlCDataTemplate_2 = new DTCv2Page.<InitializeComponent>_anonXamlCDataTemplate_27();
			object[] array39 = new object[0 + 5];
			array39[0] = dataTemplate2;
			array39[1] = listView2;
			array39[2] = grid7;
			array39[3] = grid8;
			array39[4] = this;
			<InitializeComponent>_anonXamlCDataTemplate_2.parentValues = array39;
			<InitializeComponent>_anonXamlCDataTemplate_2.root = this;
			dataTemplate5.LoadTemplate = new Func<object>(<InitializeComponent>_anonXamlCDataTemplate_2.LoadDataTemplate);
			listView2.SetValue(ListView.GroupHeaderTemplateProperty, dataTemplate2);
			IDataTemplate dataTemplate6 = dataTemplate3;
			DTCv2Page.<InitializeComponent>_anonXamlCDataTemplate_28 <InitializeComponent>_anonXamlCDataTemplate_3 = new DTCv2Page.<InitializeComponent>_anonXamlCDataTemplate_28();
			object[] array40 = new object[0 + 5];
			array40[0] = dataTemplate3;
			array40[1] = listView2;
			array40[2] = grid7;
			array40[3] = grid8;
			array40[4] = this;
			<InitializeComponent>_anonXamlCDataTemplate_3.parentValues = array40;
			<InitializeComponent>_anonXamlCDataTemplate_3.root = this;
			dataTemplate6.LoadTemplate = new Func<object>(<InitializeComponent>_anonXamlCDataTemplate_3.LoadDataTemplate);
			listView2.SetValue(ItemsView<Cell>.ItemTemplateProperty, dataTemplate3);
			grid7.Children.Add(listView2);
			activityFrame2.SetValue(Grid.RowProperty, 2);
			activityFrame2.SetValue(View.MarginProperty, new Thickness(0.0));
			bindingExtension27.Path = "IsBusy";
			BindingBase bindingBase27 = bindingExtension27.ProvideValue(null);
			activityFrame2.SetBinding(VisualElement.IsVisibleProperty, bindingBase27);
			bindingExtension28.Path = "StatusText";
			BindingBase bindingBase28 = bindingExtension28.ProvideValue(null);
			activityFrame2.SetBinding(ActivityFrame.TextProperty, bindingBase28);
			activityFrame2.SetValue(View.VerticalOptionsProperty, LayoutOptions.Center);
			grid7.Children.Add(activityFrame2);
			button4.SetValue(Grid.RowProperty, 3);
			bindingExtension29.Path = "CancelCommand";
			BindingBase bindingBase29 = bindingExtension29.ProvideValue(null);
			button4.SetBinding(Button.CommandProperty, bindingBase29);
			bindingExtension30.Mode = 2;
			bindingExtension30.Path = "IsCancelButtonEnabled";
			BindingBase bindingBase30 = bindingExtension30.ProvideValue(null);
			button4.SetBinding(VisualElement.IsEnabledProperty, bindingBase30);
			bindingExtension31.Path = "IsBusy";
			BindingBase bindingBase31 = bindingExtension31.ProvideValue(null);
			button4.SetBinding(VisualElement.IsVisibleProperty, bindingBase31);
			translate19.Text = "ios_Cancel";
			IMarkupExtension markupExtension38 = translate19;
			XamlServiceProvider xamlServiceProvider38 = new XamlServiceProvider();
			Type typeFromHandle75 = typeof(IProvideValueTarget);
			object[] array41 = new object[0 + 4];
			array41[0] = button4;
			array41[1] = grid7;
			array41[2] = grid8;
			array41[3] = this;
			object obj60;
			xamlServiceProvider38.Add(typeFromHandle75, obj60 = new SimpleValueTargetProvider(array41, Button.TextProperty, nameScope));
			xamlServiceProvider38.Add(typeof(IReferenceProvider), obj60);
			Type typeFromHandle76 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver38 = new XmlNamespaceResolver();
			xmlNamespaceResolver38.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver38.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver38.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver38.Add("dtcv2", "clr-namespace:CarScannerXamarinForms.DTCv2");
			xmlNamespaceResolver38.Add("ecuModels", "clr-namespace:CarScannerXamarinForms.ECUModels");
			xmlNamespaceResolver38.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver38.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver38.Add("settings", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver38.Add("syncfusion", "clr-namespace:Syncfusion.XForms.Buttons;assembly=Syncfusion.Buttons.XForms");
			xmlNamespaceResolver38.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider38.Add(typeFromHandle76, new XamlTypeResolver(xmlNamespaceResolver38, typeof(DTCv2Page).GetTypeInfo().Assembly));
			xamlServiceProvider38.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(433, 21)));
			object obj61 = markupExtension38.ProvideValue(xamlServiceProvider38);
			button4.Text = obj61;
			grid7.Children.Add(button4);
			grid6.SetValue(Grid.RowProperty, 3);
			staticResourceExtension5.Key = "BoolToNegativeConverter";
			IMarkupExtension markupExtension39 = staticResourceExtension5;
			XamlServiceProvider xamlServiceProvider39 = new XamlServiceProvider();
			Type typeFromHandle77 = typeof(IProvideValueTarget);
			object[] array42 = new object[0 + 5];
			array42[0] = bindingExtension32;
			array42[1] = grid6;
			array42[2] = grid7;
			array42[3] = grid8;
			array42[4] = this;
			object obj62;
			xamlServiceProvider39.Add(typeFromHandle77, obj62 = new SimpleValueTargetProvider(array42, typeof(BindingExtension).GetRuntimeProperty("Converter"), nameScope));
			xamlServiceProvider39.Add(typeof(IReferenceProvider), obj62);
			Type typeFromHandle78 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver39 = new XmlNamespaceResolver();
			xmlNamespaceResolver39.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver39.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver39.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver39.Add("dtcv2", "clr-namespace:CarScannerXamarinForms.DTCv2");
			xmlNamespaceResolver39.Add("ecuModels", "clr-namespace:CarScannerXamarinForms.ECUModels");
			xmlNamespaceResolver39.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver39.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver39.Add("settings", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver39.Add("syncfusion", "clr-namespace:Syncfusion.XForms.Buttons;assembly=Syncfusion.Buttons.XForms");
			xmlNamespaceResolver39.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider39.Add(typeFromHandle78, new XamlTypeResolver(xmlNamespaceResolver39, typeof(DTCv2Page).GetTypeInfo().Assembly));
			xamlServiceProvider39.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(435, 36)));
			object obj63 = markupExtension39.ProvideValue(xamlServiceProvider39);
			bindingExtension32.Converter = obj63;
			bindingExtension32.Path = "IsBusy";
			BindingBase bindingBase32 = bindingExtension32.ProvideValue(null);
			grid6.SetBinding(VisualElement.IsVisibleProperty, bindingBase32);
			grid5.SetValue(Grid.RowProperty, 0);
			bindingExtension33.Mode = 2;
			bindingExtension33.Path = "HasDTCToDisplay";
			BindingBase bindingBase33 = bindingExtension33.ProvideValue(null);
			grid5.SetBinding(VisualElement.IsVisibleProperty, bindingBase33);
			columnDefinition7.SetValue(ColumnDefinition.WidthProperty, new GridLengthTypeConverter().ConvertFromInvariantString("*"));
			grid5.GetValue(Grid.ColumnDefinitionsProperty).Add(columnDefinition7);
			columnDefinition8.SetValue(ColumnDefinition.WidthProperty, new GridLengthTypeConverter().ConvertFromInvariantString("*"));
			grid5.GetValue(Grid.ColumnDefinitionsProperty).Add(columnDefinition8);
			button5.SetValue(Grid.ColumnProperty, 0);
			bindingExtension34.Path = "ClearDTCCommand";
			BindingBase bindingBase34 = bindingExtension34.ProvideValue(null);
			button5.SetBinding(Button.CommandProperty, bindingBase34);
			translate20.Text = "DtcPage_btnClear.Content";
			IMarkupExtension markupExtension40 = translate20;
			XamlServiceProvider xamlServiceProvider40 = new XamlServiceProvider();
			Type typeFromHandle79 = typeof(IProvideValueTarget);
			object[] array43 = new object[0 + 6];
			array43[0] = button5;
			array43[1] = grid5;
			array43[2] = grid6;
			array43[3] = grid7;
			array43[4] = grid8;
			array43[5] = this;
			object obj64;
			xamlServiceProvider40.Add(typeFromHandle79, obj64 = new SimpleValueTargetProvider(array43, Button.TextProperty, nameScope));
			xamlServiceProvider40.Add(typeof(IReferenceProvider), obj64);
			Type typeFromHandle80 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver40 = new XmlNamespaceResolver();
			xmlNamespaceResolver40.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver40.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver40.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver40.Add("dtcv2", "clr-namespace:CarScannerXamarinForms.DTCv2");
			xmlNamespaceResolver40.Add("ecuModels", "clr-namespace:CarScannerXamarinForms.ECUModels");
			xmlNamespaceResolver40.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver40.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver40.Add("settings", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver40.Add("syncfusion", "clr-namespace:Syncfusion.XForms.Buttons;assembly=Syncfusion.Buttons.XForms");
			xmlNamespaceResolver40.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider40.Add(typeFromHandle80, new XamlTypeResolver(xmlNamespaceResolver40, typeof(DTCv2Page).GetTypeInfo().Assembly));
			xamlServiceProvider40.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(445, 29)));
			object obj65 = markupExtension40.ProvideValue(xamlServiceProvider40);
			button5.Text = obj65;
			grid5.Children.Add(button5);
			button6.SetValue(Grid.ColumnProperty, 1);
			bindingExtension35.Path = "ShareCommand";
			BindingBase bindingBase35 = bindingExtension35.ProvideValue(null);
			button6.SetBinding(Button.CommandProperty, bindingBase35);
			translate21.Text = "ios_Share";
			IMarkupExtension markupExtension41 = translate21;
			XamlServiceProvider xamlServiceProvider41 = new XamlServiceProvider();
			Type typeFromHandle81 = typeof(IProvideValueTarget);
			object[] array44 = new object[0 + 6];
			array44[0] = button6;
			array44[1] = grid5;
			array44[2] = grid6;
			array44[3] = grid7;
			array44[4] = grid8;
			array44[5] = this;
			object obj66;
			xamlServiceProvider41.Add(typeFromHandle81, obj66 = new SimpleValueTargetProvider(array44, Button.TextProperty, nameScope));
			xamlServiceProvider41.Add(typeof(IReferenceProvider), obj66);
			Type typeFromHandle82 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver41 = new XmlNamespaceResolver();
			xmlNamespaceResolver41.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver41.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver41.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver41.Add("dtcv2", "clr-namespace:CarScannerXamarinForms.DTCv2");
			xmlNamespaceResolver41.Add("ecuModels", "clr-namespace:CarScannerXamarinForms.ECUModels");
			xmlNamespaceResolver41.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver41.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver41.Add("settings", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver41.Add("syncfusion", "clr-namespace:Syncfusion.XForms.Buttons;assembly=Syncfusion.Buttons.XForms");
			xmlNamespaceResolver41.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider41.Add(typeFromHandle82, new XamlTypeResolver(xmlNamespaceResolver41, typeof(DTCv2Page).GetTypeInfo().Assembly));
			xamlServiceProvider41.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(449, 29)));
			object obj67 = markupExtension41.ProvideValue(xamlServiceProvider41);
			button6.Text = obj67;
			grid5.Children.Add(button6);
			grid6.Children.Add(grid5);
			button7.SetValue(Grid.RowProperty, 0);
			bindingExtension36.Path = "ClearDTCCommand";
			BindingBase bindingBase36 = bindingExtension36.ProvideValue(null);
			button7.SetBinding(Button.CommandProperty, bindingBase36);
			bindingExtension37.Mode = 2;
			staticResourceExtension6.Key = "BoolToNegativeConverter";
			IMarkupExtension markupExtension42 = staticResourceExtension6;
			XamlServiceProvider xamlServiceProvider42 = new XamlServiceProvider();
			Type typeFromHandle83 = typeof(IProvideValueTarget);
			object[] array45 = new object[0 + 6];
			array45[0] = bindingExtension37;
			array45[1] = button7;
			array45[2] = grid6;
			array45[3] = grid7;
			array45[4] = grid8;
			array45[5] = this;
			object obj68;
			xamlServiceProvider42.Add(typeFromHandle83, obj68 = new SimpleValueTargetProvider(array45, typeof(BindingExtension).GetRuntimeProperty("Converter"), nameScope));
			xamlServiceProvider42.Add(typeof(IReferenceProvider), obj68);
			Type typeFromHandle84 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver42 = new XmlNamespaceResolver();
			xmlNamespaceResolver42.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver42.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver42.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver42.Add("dtcv2", "clr-namespace:CarScannerXamarinForms.DTCv2");
			xmlNamespaceResolver42.Add("ecuModels", "clr-namespace:CarScannerXamarinForms.ECUModels");
			xmlNamespaceResolver42.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver42.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver42.Add("settings", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver42.Add("syncfusion", "clr-namespace:Syncfusion.XForms.Buttons;assembly=Syncfusion.Buttons.XForms");
			xmlNamespaceResolver42.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider42.Add(typeFromHandle84, new XamlTypeResolver(xmlNamespaceResolver42, typeof(DTCv2Page).GetTypeInfo().Assembly));
			xamlServiceProvider42.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(455, 25)));
			object obj69 = markupExtension42.ProvideValue(xamlServiceProvider42);
			bindingExtension37.Converter = obj69;
			bindingExtension37.Path = "HasDTCToDisplay";
			BindingBase bindingBase37 = bindingExtension37.ProvideValue(null);
			button7.SetBinding(VisualElement.IsVisibleProperty, bindingBase37);
			translate22.Text = "DtcPage_btnClear.Content";
			IMarkupExtension markupExtension43 = translate22;
			XamlServiceProvider xamlServiceProvider43 = new XamlServiceProvider();
			Type typeFromHandle85 = typeof(IProvideValueTarget);
			object[] array46 = new object[0 + 5];
			array46[0] = button7;
			array46[1] = grid6;
			array46[2] = grid7;
			array46[3] = grid8;
			array46[4] = this;
			object obj70;
			xamlServiceProvider43.Add(typeFromHandle85, obj70 = new SimpleValueTargetProvider(array46, Button.TextProperty, nameScope));
			xamlServiceProvider43.Add(typeof(IReferenceProvider), obj70);
			Type typeFromHandle86 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver43 = new XmlNamespaceResolver();
			xmlNamespaceResolver43.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver43.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver43.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver43.Add("dtcv2", "clr-namespace:CarScannerXamarinForms.DTCv2");
			xmlNamespaceResolver43.Add("ecuModels", "clr-namespace:CarScannerXamarinForms.ECUModels");
			xmlNamespaceResolver43.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver43.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver43.Add("settings", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver43.Add("syncfusion", "clr-namespace:Syncfusion.XForms.Buttons;assembly=Syncfusion.Buttons.XForms");
			xmlNamespaceResolver43.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider43.Add(typeFromHandle86, new XamlTypeResolver(xmlNamespaceResolver43, typeof(DTCv2Page).GetTypeInfo().Assembly));
			xamlServiceProvider43.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(456, 25)));
			object obj71 = markupExtension43.ProvideValue(xamlServiceProvider43);
			button7.Text = obj71;
			grid6.Children.Add(button7);
			grid7.Children.Add(grid6);
			stackLayout5.SetValue(Grid.RowProperty, 4);
			bindingExtension38.Mode = 2;
			bindingExtension38.Path = "HasDTCToDisplay";
			BindingBase bindingBase38 = bindingExtension38.ProvideValue(null);
			stackLayout5.SetBinding(VisualElement.IsVisibleProperty, bindingBase38);
			stackLayout5.SetValue(StackLayout.OrientationProperty, 0);
			label7.SetValue(Label.FontAttributesProperty, new FontAttributesConverter().ConvertFromInvariantString("Italic"));
			dynamicResourceExtension16.Key = "BaseFontSize";
			IMarkupExtension<DynamicResource> markupExtension44 = dynamicResourceExtension16;
			XamlServiceProvider xamlServiceProvider44 = new XamlServiceProvider();
			Type typeFromHandle87 = typeof(IProvideValueTarget);
			object[] array47 = new object[0 + 5];
			array47[0] = label7;
			array47[1] = stackLayout5;
			array47[2] = grid7;
			array47[3] = grid8;
			array47[4] = this;
			object obj72;
			xamlServiceProvider44.Add(typeFromHandle87, obj72 = new SimpleValueTargetProvider(array47, Label.FontSizeProperty, nameScope));
			xamlServiceProvider44.Add(typeof(IReferenceProvider), obj72);
			Type typeFromHandle88 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver44 = new XmlNamespaceResolver();
			xmlNamespaceResolver44.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver44.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver44.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver44.Add("dtcv2", "clr-namespace:CarScannerXamarinForms.DTCv2");
			xmlNamespaceResolver44.Add("ecuModels", "clr-namespace:CarScannerXamarinForms.ECUModels");
			xmlNamespaceResolver44.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver44.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver44.Add("settings", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver44.Add("syncfusion", "clr-namespace:Syncfusion.XForms.Buttons;assembly=Syncfusion.Buttons.XForms");
			xmlNamespaceResolver44.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider44.Add(typeFromHandle88, new XamlTypeResolver(xmlNamespaceResolver44, typeof(DTCv2Page).GetTypeInfo().Assembly));
			xamlServiceProvider44.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(466, 25)));
			DynamicResource dynamicResource16 = markupExtension44.ProvideValue(xamlServiceProvider44);
			label7.SetDynamicResource(Label.FontSizeProperty, dynamicResource16.Key);
			bindingExtension39.Mode = 2;
			bindingExtension39.Path = "BadELMDetected";
			BindingBase bindingBase39 = bindingExtension39.ProvideValue(null);
			label7.SetBinding(VisualElement.IsVisibleProperty, bindingBase39);
			translate23.Text = "dtc_BadELMDetected";
			IMarkupExtension markupExtension45 = translate23;
			XamlServiceProvider xamlServiceProvider45 = new XamlServiceProvider();
			Type typeFromHandle89 = typeof(IProvideValueTarget);
			object[] array48 = new object[0 + 5];
			array48[0] = label7;
			array48[1] = stackLayout5;
			array48[2] = grid7;
			array48[3] = grid8;
			array48[4] = this;
			object obj73;
			xamlServiceProvider45.Add(typeFromHandle89, obj73 = new SimpleValueTargetProvider(array48, Label.TextProperty, nameScope));
			xamlServiceProvider45.Add(typeof(IReferenceProvider), obj73);
			Type typeFromHandle90 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver45 = new XmlNamespaceResolver();
			xmlNamespaceResolver45.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver45.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver45.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver45.Add("dtcv2", "clr-namespace:CarScannerXamarinForms.DTCv2");
			xmlNamespaceResolver45.Add("ecuModels", "clr-namespace:CarScannerXamarinForms.ECUModels");
			xmlNamespaceResolver45.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver45.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver45.Add("settings", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver45.Add("syncfusion", "clr-namespace:Syncfusion.XForms.Buttons;assembly=Syncfusion.Buttons.XForms");
			xmlNamespaceResolver45.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider45.Add(typeFromHandle90, new XamlTypeResolver(xmlNamespaceResolver45, typeof(DTCv2Page).GetTypeInfo().Assembly));
			xamlServiceProvider45.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(468, 25)));
			object obj74 = markupExtension45.ProvideValue(xamlServiceProvider45);
			label7.Text = obj74;
			dynamicResourceExtension17.Key = "RedTextColor";
			IMarkupExtension<DynamicResource> markupExtension46 = dynamicResourceExtension17;
			XamlServiceProvider xamlServiceProvider46 = new XamlServiceProvider();
			Type typeFromHandle91 = typeof(IProvideValueTarget);
			object[] array49 = new object[0 + 5];
			array49[0] = label7;
			array49[1] = stackLayout5;
			array49[2] = grid7;
			array49[3] = grid8;
			array49[4] = this;
			object obj75;
			xamlServiceProvider46.Add(typeFromHandle91, obj75 = new SimpleValueTargetProvider(array49, Label.TextColorProperty, nameScope));
			xamlServiceProvider46.Add(typeof(IReferenceProvider), obj75);
			Type typeFromHandle92 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver46 = new XmlNamespaceResolver();
			xmlNamespaceResolver46.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver46.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver46.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver46.Add("dtcv2", "clr-namespace:CarScannerXamarinForms.DTCv2");
			xmlNamespaceResolver46.Add("ecuModels", "clr-namespace:CarScannerXamarinForms.ECUModels");
			xmlNamespaceResolver46.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver46.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver46.Add("settings", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver46.Add("syncfusion", "clr-namespace:Syncfusion.XForms.Buttons;assembly=Syncfusion.Buttons.XForms");
			xmlNamespaceResolver46.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider46.Add(typeFromHandle92, new XamlTypeResolver(xmlNamespaceResolver46, typeof(DTCv2Page).GetTypeInfo().Assembly));
			xamlServiceProvider46.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(469, 25)));
			DynamicResource dynamicResource17 = markupExtension46.ProvideValue(xamlServiceProvider46);
			label7.SetDynamicResource(Label.TextColorProperty, dynamicResource17.Key);
			tapGestureRecognizer5.SetValue(TapGestureRecognizer.NumberOfTapsRequiredProperty, 1);
			tapGestureRecognizer5.Tapped += this.LabelBadDTCTapGestureRecognizer_Tapped;
			label7.GestureRecognizers.Add(tapGestureRecognizer5);
			stackLayout5.Children.Add(label7);
			grid7.Children.Add(stackLayout5);
			grid8.Children.Add(grid7);
			complexAdView.SetValue(Grid.RowProperty, 1);
			complexAdView.SetValue(View.MarginProperty, new Thickness(0.0, 0.0, 0.0, 0.0));
			complexAdView.SetValue(VisualElement.HeightRequestProperty, 55.0);
			complexAdView.SetValue(VisualElement.IsVisibleProperty, new VisualElement.VisibilityConverter().ConvertFromInvariantString("true"));
			complexAdView.SetValue(View.VerticalOptionsProperty, LayoutOptions.End);
			grid8.Children.Add(complexAdView);
			this.SetValue(ContentPage.ContentProperty, grid8);
		}

		// Token: 0x0600347D RID: 13437 RVA: 0x002523AC File Offset: 0x002505AC
		private void __InitComponentRuntime()
		{
			Extensions.LoadFromXaml<DTCv2Page>(this, typeof(DTCv2Page));
			this.gridButtons = NameScopeExtensions.FindByName<Grid>(this, "gridButtons");
			this.btnCheckAll = NameScopeExtensions.FindByName<Image>(this, "btnCheckAll");
			this.btnUncheckAll = NameScopeExtensions.FindByName<Image>(this, "btnUncheckAll");
			this.btnFilter = NameScopeExtensions.FindByName<Image>(this, "btnFilter");
			this.layoutRoot = NameScopeExtensions.FindByName<Grid>(this, "layoutRoot");
			this.gridSetup = NameScopeExtensions.FindByName<Grid>(this, "gridSetup");
			this.lvECUs = NameScopeExtensions.FindByName<ListView>(this, "lvECUs");
			this.gridAllOrSupportedSelector = NameScopeExtensions.FindByName<Grid>(this, "gridAllOrSupportedSelector");
			this.rbDetected = NameScopeExtensions.FindByName<RadioButtonWithColor>(this, "rbDetected");
			this.rbAll = NameScopeExtensions.FindByName<RadioButtonWithColor>(this, "rbAll");
			this.swHideArchiveDTC2 = NameScopeExtensions.FindByName<LabelSwitch>(this, "swHideArchiveDTC2");
			this.swHideUncomplitedTests2 = NameScopeExtensions.FindByName<LabelSwitch>(this, "swHideUncomplitedTests2");
			this.btnToOldStyle = NameScopeExtensions.FindByName<Button>(this, "btnToOldStyle");
			this.gridSetupButtons = NameScopeExtensions.FindByName<Grid>(this, "gridSetupButtons");
			this.btnReadDTC = NameScopeExtensions.FindByName<Button>(this, "btnReadDTC");
			this.btnClearDTC = NameScopeExtensions.FindByName<Button>(this, "btnClearDTC");
			this.activityFrame2 = NameScopeExtensions.FindByName<ActivityFrame>(this, "activityFrame2");
			this.gridResults = NameScopeExtensions.FindByName<Grid>(this, "gridResults");
			this.stackFilter = NameScopeExtensions.FindByName<StackLayout>(this, "stackFilter");
			this.lvDTCs = NameScopeExtensions.FindByName<ListView>(this, "lvDTCs");
			this.activityFrame = NameScopeExtensions.FindByName<ActivityFrame>(this, "activityFrame");
			this.btnCancel = NameScopeExtensions.FindByName<Button>(this, "btnCancel");
			this.ad = NameScopeExtensions.FindByName<ComplexAdView>(this, "ad");
		}

		// Token: 0x0600347E RID: 13438 RVA: 0x00252554 File Offset: 0x00250754
		[CompilerGenerated]
		private static ValueTuple<ObservableCollection<IECU>, bool> <InitializeComponent>typedBindingsM__1041(DTCv2Model A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<ObservableCollection<IECU>, bool>(A_0.ECUListToDisplay, true);
			}
			return default(ValueTuple<ObservableCollection<IECU>, bool>);
		}

		// Token: 0x0600347F RID: 13439 RVA: 0x00252584 File Offset: 0x00250784
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__1043(DTCv2Model A_0)
		{
			return A_0;
		}

		// Token: 0x06003480 RID: 13440 RVA: 0x00252594 File Offset: 0x00250794
		[CompilerGenerated]
		private static ValueTuple<bool, bool> <InitializeComponent>typedBindingsM__1052(DTCv2Model A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<bool, bool>(A_0.DisplayAllOrDetectedSelector, true);
			}
			return default(ValueTuple<bool, bool>);
		}

		// Token: 0x06003481 RID: 13441 RVA: 0x002525C4 File Offset: 0x002507C4
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__1053(DTCv2Model A_0)
		{
			return A_0;
		}

		// Token: 0x06003482 RID: 13442 RVA: 0x002525D4 File Offset: 0x002507D4
		[CompilerGenerated]
		private static ValueTuple<bool, bool> <InitializeComponent>typedBindingsM__1054(DTCv2Model A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<bool, bool>(A_0.DisplayAllOrDetectedSelector, true);
			}
			return default(ValueTuple<bool, bool>);
		}

		// Token: 0x06003483 RID: 13443 RVA: 0x00252604 File Offset: 0x00250804
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__1055(DTCv2Model A_0)
		{
			return A_0;
		}

		// Token: 0x06003484 RID: 13444 RVA: 0x00252614 File Offset: 0x00250814
		[CompilerGenerated]
		private static ValueTuple<bool, bool> <InitializeComponent>typedBindingsM__1056(DTCv2Model A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<bool, bool>(A_0.DisplayDetected, true);
			}
			return default(ValueTuple<bool, bool>);
		}

		// Token: 0x06003485 RID: 13445 RVA: 0x00252644 File Offset: 0x00250844
		[CompilerGenerated]
		private static void <InitializeComponent>typedBindingsM__1057(DTCv2Model A_0, bool A_1)
		{
			if (A_0 != null)
			{
				A_0.DisplayDetected = A_1;
				return;
			}
		}

		// Token: 0x06003486 RID: 13446 RVA: 0x00252660 File Offset: 0x00250860
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__1058(DTCv2Model A_0)
		{
			return A_0;
		}

		// Token: 0x06003487 RID: 13447 RVA: 0x00252670 File Offset: 0x00250870
		[CompilerGenerated]
		private static ValueTuple<bool, bool> <InitializeComponent>typedBindingsM__1059(SharedSettings A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<bool, bool>(A_0.HideArchiveDTC, true);
			}
			return default(ValueTuple<bool, bool>);
		}

		// Token: 0x06003488 RID: 13448 RVA: 0x002526A0 File Offset: 0x002508A0
		[CompilerGenerated]
		private static void <InitializeComponent>typedBindingsM__1060(SharedSettings A_0, bool A_1)
		{
			if (A_0 != null)
			{
				A_0.HideArchiveDTC = A_1;
				return;
			}
		}

		// Token: 0x06003489 RID: 13449 RVA: 0x002526BC File Offset: 0x002508BC
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__1061(SharedSettings A_0)
		{
			return A_0;
		}

		// Token: 0x0600348A RID: 13450 RVA: 0x002526CC File Offset: 0x002508CC
		[CompilerGenerated]
		private static ValueTuple<bool, bool> <InitializeComponent>typedBindingsM__1062(SharedSettings A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<bool, bool>(A_0.HideDTCWithUncomplitedTests, true);
			}
			return default(ValueTuple<bool, bool>);
		}

		// Token: 0x0600348B RID: 13451 RVA: 0x002526FC File Offset: 0x002508FC
		[CompilerGenerated]
		private static void <InitializeComponent>typedBindingsM__1063(SharedSettings A_0, bool A_1)
		{
			if (A_0 != null)
			{
				A_0.HideDTCWithUncomplitedTests = A_1;
				return;
			}
		}

		// Token: 0x0600348C RID: 13452 RVA: 0x00252718 File Offset: 0x00250918
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__1064(SharedSettings A_0)
		{
			return A_0;
		}

		// Token: 0x04001F0D RID: 7949
		public DTCv2Model Model;

		// Token: 0x04001F0E RID: 7950
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private Grid gridButtons;

		// Token: 0x04001F0F RID: 7951
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private Image btnCheckAll;

		// Token: 0x04001F10 RID: 7952
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private Image btnUncheckAll;

		// Token: 0x04001F11 RID: 7953
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private Image btnFilter;

		// Token: 0x04001F12 RID: 7954
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private Grid layoutRoot;

		// Token: 0x04001F13 RID: 7955
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private Grid gridSetup;

		// Token: 0x04001F14 RID: 7956
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private ListView lvECUs;

		// Token: 0x04001F15 RID: 7957
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private Grid gridAllOrSupportedSelector;

		// Token: 0x04001F16 RID: 7958
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private RadioButtonWithColor rbDetected;

		// Token: 0x04001F17 RID: 7959
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private RadioButtonWithColor rbAll;

		// Token: 0x04001F18 RID: 7960
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private LabelSwitch swHideArchiveDTC2;

		// Token: 0x04001F19 RID: 7961
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private LabelSwitch swHideUncomplitedTests2;

		// Token: 0x04001F1A RID: 7962
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private Button btnToOldStyle;

		// Token: 0x04001F1B RID: 7963
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private Grid gridSetupButtons;

		// Token: 0x04001F1C RID: 7964
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private Button btnReadDTC;

		// Token: 0x04001F1D RID: 7965
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private Button btnClearDTC;

		// Token: 0x04001F1E RID: 7966
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private ActivityFrame activityFrame2;

		// Token: 0x04001F1F RID: 7967
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private Grid gridResults;

		// Token: 0x04001F20 RID: 7968
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private StackLayout stackFilter;

		// Token: 0x04001F21 RID: 7969
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private ListView lvDTCs;

		// Token: 0x04001F22 RID: 7970
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private ActivityFrame activityFrame;

		// Token: 0x04001F23 RID: 7971
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private Button btnCancel;

		// Token: 0x04001F24 RID: 7972
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private ComplexAdView ad;

		// Token: 0x020005A3 RID: 1443
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <BtnClearECUDTC_Clicked>d__7 : IAsyncStateMachine
		{
			// Token: 0x0600348D RID: 13453 RVA: 0x00252728 File Offset: 0x00250928
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				DTCv2Page dtcv2Page = this;
				try
				{
					TaskAwaiter taskAwaiter;
					if (num != 0)
					{
						if (dtcv2Page.Model.IsBusy)
						{
							goto IL_00B2;
						}
						IECU iecu = (sender as VisualElement).BindingContext as IECU;
						taskAwaiter = dtcv2Page.Model.ClearDTCForOneECU(iecu).GetAwaiter();
						if (!taskAwaiter.IsCompleted)
						{
							num2 = 0;
							TaskAwaiter taskAwaiter2 = taskAwaiter;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, DTCv2Page.<BtnClearECUDTC_Clicked>d__7>(ref taskAwaiter, ref this);
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
				IL_00B2:
				num2 = -2;
				this.<>t__builder.SetResult();
			}

			// Token: 0x0600348E RID: 13454 RVA: 0x0025280C File Offset: 0x00250A0C
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x04001F25 RID: 7973
			public int <>1__state;

			// Token: 0x04001F26 RID: 7974
			public AsyncVoidMethodBuilder <>t__builder;

			// Token: 0x04001F27 RID: 7975
			public DTCv2Page <>4__this;

			// Token: 0x04001F28 RID: 7976
			public object sender;

			// Token: 0x04001F29 RID: 7977
			private TaskAwaiter <>u__1;
		}

		// Token: 0x020005A4 RID: 1444
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <LoadModel>d__1 : IAsyncStateMachine
		{
			// Token: 0x0600348F RID: 13455 RVA: 0x0025281C File Offset: 0x00250A1C
			void IAsyncStateMachine.MoveNext()
			{
				DTCv2Page dtcv2Page = this;
				try
				{
					dtcv2Page.Model = new DTCv2Model();
					dtcv2Page.BindingContext = dtcv2Page.Model;
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

			// Token: 0x06003490 RID: 13456 RVA: 0x00252884 File Offset: 0x00250A84
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x04001F2A RID: 7978
			public int <>1__state;

			// Token: 0x04001F2B RID: 7979
			public AsyncTaskMethodBuilder <>t__builder;

			// Token: 0x04001F2C RID: 7980
			public DTCv2Page <>4__this;
		}

		// Token: 0x020005A5 RID: 1445
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <btnToOldStyle_Clicked>d__9 : IAsyncStateMachine
		{
			// Token: 0x06003491 RID: 13457 RVA: 0x00252894 File Offset: 0x00250A94
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				DTCv2Page dtcv2Page = this;
				try
				{
					TaskAwaiter taskAwaiter;
					if (num != 0)
					{
						if (num == 1)
						{
							TaskAwaiter taskAwaiter2;
							taskAwaiter = taskAwaiter2;
							taskAwaiter2 = default(TaskAwaiter);
							num2 = -1;
							goto IL_00E5;
						}
						taskAwaiter = App.OBDReader.DebugWrite("\r\n[USER_SELECTED_OLD_STYLE_DTC]\r\n").GetAwaiter();
						if (!taskAwaiter.IsCompleted)
						{
							num2 = 0;
							TaskAwaiter taskAwaiter2 = taskAwaiter;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, DTCv2Page.<btnToOldStyle_Clicked>d__9>(ref taskAwaiter, ref this);
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
					dtcv2Page.btnToOldStyle.IsEnabled = false;
					DTCWizardStartPage dtcwizardStartPage = new DTCWizardStartPage();
					taskAwaiter = dtcv2Page.Navigation.PushAsync(dtcwizardStartPage).GetAwaiter();
					if (!taskAwaiter.IsCompleted)
					{
						num2 = 1;
						TaskAwaiter taskAwaiter2 = taskAwaiter;
						this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, DTCv2Page.<btnToOldStyle_Clicked>d__9>(ref taskAwaiter, ref this);
						return;
					}
					IL_00E5:
					taskAwaiter.GetResult();
					dtcv2Page.btnToOldStyle.IsEnabled = true;
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

			// Token: 0x06003492 RID: 13458 RVA: 0x002529D8 File Offset: 0x00250BD8
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x04001F2D RID: 7981
			public int <>1__state;

			// Token: 0x04001F2E RID: 7982
			public AsyncVoidMethodBuilder <>t__builder;

			// Token: 0x04001F2F RID: 7983
			public DTCv2Page <>4__this;

			// Token: 0x04001F30 RID: 7984
			private TaskAwaiter <>u__1;
		}

		// Token: 0x020005A6 RID: 1446
		[CompilerGenerated]
		private sealed class <InitializeComponent>_anonXamlCDataTemplate_26
		{
			// Token: 0x06003493 RID: 13459 RVA: 0x002529E8 File Offset: 0x00250BE8
			public <InitializeComponent>_anonXamlCDataTemplate_26()
			{
			}

			// Token: 0x06003494 RID: 13460 RVA: 0x002529FC File Offset: 0x00250BFC
			internal object LoadDataTemplate()
			{
				BindingExtension bindingExtension;
				VisualDiagnostics.RegisterSourceInfo(bindingExtension = new BindingExtension(), new Uri("DTCv2\\DTCv2Page.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 125, 37);
				BindingExtension bindingExtension2;
				VisualDiagnostics.RegisterSourceInfo(bindingExtension2 = new BindingExtension(), new Uri("DTCv2\\DTCv2Page.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 126, 37);
				StaticResourceExtension staticResourceExtension;
				VisualDiagnostics.RegisterSourceInfo(staticResourceExtension = new StaticResourceExtension(), new Uri("DTCv2\\DTCv2Page.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 127, 37);
				BindingExtension bindingExtension3;
				VisualDiagnostics.RegisterSourceInfo(bindingExtension3 = new BindingExtension(), new Uri("DTCv2\\DTCv2Page.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 127, 37);
				LabelSwitch labelSwitch;
				VisualDiagnostics.RegisterSourceInfo(labelSwitch = new LabelSwitch(), new Uri("DTCv2\\DTCv2Page.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 123, 34);
				ViewCell viewCell;
				VisualDiagnostics.RegisterSourceInfo(viewCell = new ViewCell(), new Uri("DTCv2\\DTCv2Page.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 122, 30);
				NameScope nameScope = new NameScope();
				NameScope.SetNameScope(viewCell, nameScope);
				labelSwitch.SetValue(View.MarginProperty, new Thickness(0.0, 5.0));
				bindingExtension.Mode = 1;
				bindingExtension.Path = "IsSelected";
				bindingExtension.TypedBinding = new TypedBinding<IECU, bool>(delegate(IECU A_0)
				{
					if (A_0 != null)
					{
						return new ValueTuple<bool, bool>(A_0.IsSelected, true);
					}
					return default(ValueTuple<bool, bool>);
				}, delegate(IECU A_0, bool A_1)
				{
					if (A_0 != null)
					{
						A_0.IsSelected = A_1;
						return;
					}
				}, new Tuple<Func<IECU, object>, string>[]
				{
					new Tuple<Func<IECU, object>, string>((IECU A_0) => A_0, "IsSelected")
				});
				BindingBase bindingBase = bindingExtension.ProvideValue(null);
				labelSwitch.SetBinding(LabelSwitch.IsToggledProperty, bindingBase);
				bindingExtension2.Path = "Name";
				bindingExtension2.TypedBinding = new TypedBinding<IECU, string>(delegate(IECU A_0)
				{
					if (A_0 != null)
					{
						return new ValueTuple<string, bool>(A_0.Name, true);
					}
					return default(ValueTuple<string, bool>);
				}, delegate(IECU A_0, string A_1)
				{
					if (A_0 != null)
					{
						A_0.Name = A_1;
						return;
					}
				}, new Tuple<Func<IECU, object>, string>[]
				{
					new Tuple<Func<IECU, object>, string>((IECU A_0) => A_0, "Name")
				});
				BindingBase bindingBase2 = bindingExtension2.ProvideValue(null);
				labelSwitch.SetBinding(LabelSwitch.TextProperty, bindingBase2);
				bindingExtension3.Mode = 2;
				staticResourceExtension.Key = "TrueToRedColorConverter";
				IMarkupExtension markupExtension = staticResourceExtension;
				XamlServiceProvider xamlServiceProvider = new XamlServiceProvider();
				Type typeFromHandle = typeof(IProvideValueTarget);
				int num;
				object[] array = new object[(num = this.parentValues.Length) + 3];
				Array.Copy(this.parentValues, 0, array, 3, num);
				object[] array2 = array;
				array2[0] = bindingExtension3;
				array2[1] = labelSwitch;
				array2[2] = viewCell;
				object obj;
				xamlServiceProvider.Add(typeFromHandle, obj = new SimpleValueTargetProvider(array2, typeof(BindingExtension).GetRuntimeProperty("Converter"), nameScope));
				xamlServiceProvider.Add(typeof(IReferenceProvider), obj);
				Type typeFromHandle2 = typeof(IXamlTypeResolver);
				XmlNamespaceResolver xmlNamespaceResolver = new XmlNamespaceResolver();
				xmlNamespaceResolver.Add("", "http://xamarin.com/schemas/2014/forms");
				xmlNamespaceResolver.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
				xmlNamespaceResolver.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
				xmlNamespaceResolver.Add("dtcv2", "clr-namespace:CarScannerXamarinForms.DTCv2");
				xmlNamespaceResolver.Add("ecuModels", "clr-namespace:CarScannerXamarinForms.ECUModels");
				xmlNamespaceResolver.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
				xmlNamespaceResolver.Add("local", "clr-namespace:CarScannerXamarinForms");
				xmlNamespaceResolver.Add("settings", "clr-namespace:CarScannerXamarinForms.Settings");
				xmlNamespaceResolver.Add("syncfusion", "clr-namespace:Syncfusion.XForms.Buttons;assembly=Syncfusion.Buttons.XForms");
				xmlNamespaceResolver.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
				xamlServiceProvider.Add(typeFromHandle2, new XamlTypeResolver(xmlNamespaceResolver, typeof(DTCv2Page.<InitializeComponent>_anonXamlCDataTemplate_26).GetTypeInfo().Assembly));
				xamlServiceProvider.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(127, 37)));
				object obj2 = markupExtension.ProvideValue(xamlServiceProvider);
				bindingExtension3.Converter = obj2;
				bindingExtension3.Path = "Highlighted";
				bindingExtension3.TypedBinding = new TypedBinding<IECU, bool>(delegate(IECU A_0)
				{
					if (A_0 != null)
					{
						return new ValueTuple<bool, bool>(A_0.Highlighted, true);
					}
					return default(ValueTuple<bool, bool>);
				}, null, new Tuple<Func<IECU, object>, string>[]
				{
					new Tuple<Func<IECU, object>, string>((IECU A_0) => A_0, "Highlighted")
				});
				BindingBase bindingBase3 = bindingExtension3.ProvideValue(null);
				labelSwitch.SetBinding(LabelSwitch.TextColorProperty, bindingBase3);
				viewCell.View = labelSwitch;
				return viewCell;
			}

			// Token: 0x06003495 RID: 13461 RVA: 0x00252DFC File Offset: 0x00250FFC
			[CompilerGenerated]
			private static ValueTuple<bool, bool> <LoadDataTemplate>typedBindingsM__1044(IECU A_0)
			{
				if (A_0 != null)
				{
					return new ValueTuple<bool, bool>(A_0.IsSelected, true);
				}
				return default(ValueTuple<bool, bool>);
			}

			// Token: 0x06003496 RID: 13462 RVA: 0x00252E2C File Offset: 0x0025102C
			[CompilerGenerated]
			private static void <LoadDataTemplate>typedBindingsM__1045(IECU A_0, bool A_1)
			{
				if (A_0 != null)
				{
					A_0.IsSelected = A_1;
					return;
				}
			}

			// Token: 0x06003497 RID: 13463 RVA: 0x00252E48 File Offset: 0x00251048
			[CompilerGenerated]
			private static object <LoadDataTemplate>typedBindingsM__1046(IECU A_0)
			{
				return A_0;
			}

			// Token: 0x06003498 RID: 13464 RVA: 0x00252E58 File Offset: 0x00251058
			[CompilerGenerated]
			private static ValueTuple<string, bool> <LoadDataTemplate>typedBindingsM__1047(IECU A_0)
			{
				if (A_0 != null)
				{
					return new ValueTuple<string, bool>(A_0.Name, true);
				}
				return default(ValueTuple<string, bool>);
			}

			// Token: 0x06003499 RID: 13465 RVA: 0x00252E88 File Offset: 0x00251088
			[CompilerGenerated]
			private static void <LoadDataTemplate>typedBindingsM__1048(IECU A_0, string A_1)
			{
				if (A_0 != null)
				{
					A_0.Name = A_1;
					return;
				}
			}

			// Token: 0x0600349A RID: 13466 RVA: 0x00252EA4 File Offset: 0x002510A4
			[CompilerGenerated]
			private static object <LoadDataTemplate>typedBindingsM__1049(IECU A_0)
			{
				return A_0;
			}

			// Token: 0x0600349B RID: 13467 RVA: 0x00252EB4 File Offset: 0x002510B4
			[CompilerGenerated]
			private static ValueTuple<bool, bool> <LoadDataTemplate>typedBindingsM__1050(IECU A_0)
			{
				if (A_0 != null)
				{
					return new ValueTuple<bool, bool>(A_0.Highlighted, true);
				}
				return default(ValueTuple<bool, bool>);
			}

			// Token: 0x0600349C RID: 13468 RVA: 0x00252EE4 File Offset: 0x002510E4
			[CompilerGenerated]
			private static object <LoadDataTemplate>typedBindingsM__1051(IECU A_0)
			{
				return A_0;
			}

			// Token: 0x04001F31 RID: 7985
			internal object[] parentValues;

			// Token: 0x04001F32 RID: 7986
			internal DTCv2Page root;
		}

		// Token: 0x020005A7 RID: 1447
		[CompilerGenerated]
		private sealed class <InitializeComponent>_anonXamlCDataTemplate_27
		{
			// Token: 0x0600349D RID: 13469 RVA: 0x00252EF4 File Offset: 0x002510F4
			public <InitializeComponent>_anonXamlCDataTemplate_27()
			{
			}

			// Token: 0x0600349E RID: 13470 RVA: 0x00252F08 File Offset: 0x00251108
			internal object LoadDataTemplate()
			{
				TapGestureRecognizer tapGestureRecognizer;
				VisualDiagnostics.RegisterSourceInfo(tapGestureRecognizer = new TapGestureRecognizer(), new Uri("DTCv2\\DTCv2Page.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 299, 42);
				ColumnDefinition columnDefinition;
				VisualDiagnostics.RegisterSourceInfo(columnDefinition = new ColumnDefinition(), new Uri("DTCv2\\DTCv2Page.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 302, 42);
				ColumnDefinition columnDefinition2;
				VisualDiagnostics.RegisterSourceInfo(columnDefinition2 = new ColumnDefinition(), new Uri("DTCv2\\DTCv2Page.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 303, 42);
				BindingExtension bindingExtension;
				VisualDiagnostics.RegisterSourceInfo(bindingExtension = new BindingExtension(), new Uri("DTCv2\\DTCv2Page.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 310, 41);
				DynamicResourceExtension dynamicResourceExtension;
				VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension = new DynamicResourceExtension(), new Uri("DTCv2\\DTCv2Page.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 311, 41);
				Label label;
				VisualDiagnostics.RegisterSourceInfo(label = new Label(), new Uri("DTCv2\\DTCv2Page.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 305, 38);
				DynamicResourceExtension dynamicResourceExtension2;
				VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension2 = new DynamicResourceExtension(), new Uri("DTCv2\\DTCv2Page.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 333, 41);
				Button button;
				VisualDiagnostics.RegisterSourceInfo(button = new Button(), new Uri("DTCv2\\DTCv2Page.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 330, 38);
				Grid grid;
				VisualDiagnostics.RegisterSourceInfo(grid = new Grid(), new Uri("DTCv2\\DTCv2Page.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 297, 34);
				ViewCell viewCell;
				VisualDiagnostics.RegisterSourceInfo(viewCell = new ViewCell(), new Uri("DTCv2\\DTCv2Page.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 296, 30);
				NameScope nameScope = new NameScope();
				NameScope.SetNameScope(viewCell, nameScope);
				nameScope.RegisterName("btnClearEcu", button);
				if (button.StyleId == null)
				{
					button.StyleId = "btnClearEcu";
				}
				grid.SetValue(Grid.ColumnSpacingProperty, 0.0);
				tapGestureRecognizer.SetValue(TapGestureRecognizer.NumberOfTapsRequiredProperty, 1);
				tapGestureRecognizer.Tapped += this.root.btnExpand_Clicked;
				grid.GestureRecognizers.Add(tapGestureRecognizer);
				columnDefinition.SetValue(ColumnDefinition.WidthProperty, new GridLengthTypeConverter().ConvertFromInvariantString("*"));
				grid.GetValue(Grid.ColumnDefinitionsProperty).Add(columnDefinition);
				columnDefinition2.SetValue(ColumnDefinition.WidthProperty, new GridLengthTypeConverter().ConvertFromInvariantString("auto"));
				grid.GetValue(Grid.ColumnDefinitionsProperty).Add(columnDefinition2);
				label.SetValue(Grid.ColumnProperty, 0);
				label.SetValue(Label.FontAttributesProperty, new FontAttributesConverter().ConvertFromInvariantString("Bold"));
				label.SetValue(View.HorizontalOptionsProperty, LayoutOptions.Center);
				label.SetValue(Label.HorizontalTextAlignmentProperty, new TextAlignmentConverter().ConvertFromInvariantString("Center"));
				bindingExtension.Path = "Name";
				BindingBase bindingBase = bindingExtension.ProvideValue(null);
				label.SetBinding(Label.TextProperty, bindingBase);
				dynamicResourceExtension.Key = "ButtonAccentColor";
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
				xamlServiceProvider.Add(typeFromHandle, obj = new SimpleValueTargetProvider(array2, Label.TextColorProperty, nameScope));
				xamlServiceProvider.Add(typeof(IReferenceProvider), obj);
				Type typeFromHandle2 = typeof(IXamlTypeResolver);
				XmlNamespaceResolver xmlNamespaceResolver = new XmlNamespaceResolver();
				xmlNamespaceResolver.Add("", "http://xamarin.com/schemas/2014/forms");
				xmlNamespaceResolver.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
				xmlNamespaceResolver.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
				xmlNamespaceResolver.Add("dtcv2", "clr-namespace:CarScannerXamarinForms.DTCv2");
				xmlNamespaceResolver.Add("ecuModels", "clr-namespace:CarScannerXamarinForms.ECUModels");
				xmlNamespaceResolver.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
				xmlNamespaceResolver.Add("local", "clr-namespace:CarScannerXamarinForms");
				xmlNamespaceResolver.Add("settings", "clr-namespace:CarScannerXamarinForms.Settings");
				xmlNamespaceResolver.Add("syncfusion", "clr-namespace:Syncfusion.XForms.Buttons;assembly=Syncfusion.Buttons.XForms");
				xmlNamespaceResolver.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
				xamlServiceProvider.Add(typeFromHandle2, new XamlTypeResolver(xmlNamespaceResolver, typeof(DTCv2Page.<InitializeComponent>_anonXamlCDataTemplate_27).GetTypeInfo().Assembly));
				xamlServiceProvider.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(311, 41)));
				DynamicResource dynamicResource = markupExtension.ProvideValue(xamlServiceProvider);
				label.SetDynamicResource(Label.TextColorProperty, dynamicResource.Key);
				label.SetValue(View.VerticalOptionsProperty, LayoutOptions.Center);
				label.SetValue(Label.VerticalTextAlignmentProperty, new TextAlignmentConverter().ConvertFromInvariantString("Center"));
				grid.Children.Add(label);
				button.SetValue(Grid.ColumnProperty, 1);
				dynamicResourceExtension2.Key = "ButtonRedColor";
				IMarkupExtension<DynamicResource> markupExtension2 = dynamicResourceExtension2;
				XamlServiceProvider xamlServiceProvider2 = new XamlServiceProvider();
				Type typeFromHandle3 = typeof(IProvideValueTarget);
				int num2;
				object[] array3 = new object[(num2 = this.parentValues.Length) + 3];
				Array.Copy(this.parentValues, 0, array3, 3, num2);
				object[] array4 = array3;
				array4[0] = button;
				array4[1] = grid;
				array4[2] = viewCell;
				object obj2;
				xamlServiceProvider2.Add(typeFromHandle3, obj2 = new SimpleValueTargetProvider(array4, VisualElement.BackgroundColorProperty, nameScope));
				xamlServiceProvider2.Add(typeof(IReferenceProvider), obj2);
				Type typeFromHandle4 = typeof(IXamlTypeResolver);
				XmlNamespaceResolver xmlNamespaceResolver2 = new XmlNamespaceResolver();
				xmlNamespaceResolver2.Add("", "http://xamarin.com/schemas/2014/forms");
				xmlNamespaceResolver2.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
				xmlNamespaceResolver2.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
				xmlNamespaceResolver2.Add("dtcv2", "clr-namespace:CarScannerXamarinForms.DTCv2");
				xmlNamespaceResolver2.Add("ecuModels", "clr-namespace:CarScannerXamarinForms.ECUModels");
				xmlNamespaceResolver2.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
				xmlNamespaceResolver2.Add("local", "clr-namespace:CarScannerXamarinForms");
				xmlNamespaceResolver2.Add("settings", "clr-namespace:CarScannerXamarinForms.Settings");
				xmlNamespaceResolver2.Add("syncfusion", "clr-namespace:Syncfusion.XForms.Buttons;assembly=Syncfusion.Buttons.XForms");
				xmlNamespaceResolver2.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
				xamlServiceProvider2.Add(typeFromHandle4, new XamlTypeResolver(xmlNamespaceResolver2, typeof(DTCv2Page.<InitializeComponent>_anonXamlCDataTemplate_27).GetTypeInfo().Assembly));
				xamlServiceProvider2.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(333, 41)));
				DynamicResource dynamicResource2 = markupExtension2.ProvideValue(xamlServiceProvider2);
				button.SetDynamicResource(VisualElement.BackgroundColorProperty, dynamicResource2.Key);
				button.Clicked += this.root.BtnClearECUDTC_Clicked;
				button.SetValue(Button.ImageSourceProperty, new ImageSourceConverter().ConvertFromInvariantString("icons8_trash_filled_inverted.png"));
				grid.Children.Add(button);
				viewCell.View = grid;
				return viewCell;
			}

			// Token: 0x04001F33 RID: 7987
			internal object[] parentValues;

			// Token: 0x04001F34 RID: 7988
			internal DTCv2Page root;
		}

		// Token: 0x020005A8 RID: 1448
		[CompilerGenerated]
		private sealed class <InitializeComponent>_anonXamlCDataTemplate_28
		{
			// Token: 0x0600349F RID: 13471 RVA: 0x002535DC File Offset: 0x002517DC
			public <InitializeComponent>_anonXamlCDataTemplate_28()
			{
			}

			// Token: 0x060034A0 RID: 13472 RVA: 0x002535F0 File Offset: 0x002517F0
			internal object LoadDataTemplate()
			{
				RowDefinition rowDefinition;
				VisualDiagnostics.RegisterSourceInfo(rowDefinition = new RowDefinition(), new Uri("DTCv2\\DTCv2Page.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 349, 42);
				RowDefinition rowDefinition2;
				VisualDiagnostics.RegisterSourceInfo(rowDefinition2 = new RowDefinition(), new Uri("DTCv2\\DTCv2Page.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 350, 42);
				DynamicResourceExtension dynamicResourceExtension;
				VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension = new DynamicResourceExtension(), new Uri("DTCv2\\DTCv2Page.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 357, 45);
				StaticResourceExtension staticResourceExtension;
				VisualDiagnostics.RegisterSourceInfo(staticResourceExtension = new StaticResourceExtension(), new Uri("DTCv2\\DTCv2Page.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 358, 45);
				BindingExtension bindingExtension;
				VisualDiagnostics.RegisterSourceInfo(bindingExtension = new BindingExtension(), new Uri("DTCv2\\DTCv2Page.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 358, 45);
				Label label;
				VisualDiagnostics.RegisterSourceInfo(label = new Label(), new Uri("DTCv2\\DTCv2Page.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 353, 42);
				DynamicResourceExtension dynamicResourceExtension2;
				VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension2 = new DynamicResourceExtension(), new Uri("DTCv2\\DTCv2Page.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 363, 45);
				BindingExtension bindingExtension2;
				VisualDiagnostics.RegisterSourceInfo(bindingExtension2 = new BindingExtension(), new Uri("DTCv2\\DTCv2Page.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 364, 45);
				Translate translate;
				VisualDiagnostics.RegisterSourceInfo(translate = new Translate(), new Uri("DTCv2\\DTCv2Page.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 365, 45);
				Label label2;
				VisualDiagnostics.RegisterSourceInfo(label2 = new Label(), new Uri("DTCv2\\DTCv2Page.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 360, 42);
				StackLayout stackLayout;
				VisualDiagnostics.RegisterSourceInfo(stackLayout = new StackLayout(), new Uri("DTCv2\\DTCv2Page.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 352, 38);
				ColumnDefinition columnDefinition;
				VisualDiagnostics.RegisterSourceInfo(columnDefinition = new ColumnDefinition(), new Uri("DTCv2\\DTCv2Page.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 372, 46);
				ColumnDefinition columnDefinition2;
				VisualDiagnostics.RegisterSourceInfo(columnDefinition2 = new ColumnDefinition(), new Uri("DTCv2\\DTCv2Page.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 373, 46);
				DynamicResourceExtension dynamicResourceExtension3;
				VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension3 = new DynamicResourceExtension(), new Uri("DTCv2\\DTCv2Page.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 389, 49);
				StaticResourceExtension staticResourceExtension2;
				VisualDiagnostics.RegisterSourceInfo(staticResourceExtension2 = new StaticResourceExtension(), new Uri("DTCv2\\DTCv2Page.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 390, 49);
				BindingExtension bindingExtension3;
				VisualDiagnostics.RegisterSourceInfo(bindingExtension3 = new BindingExtension(), new Uri("DTCv2\\DTCv2Page.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 390, 49);
				BindingExtension bindingExtension4;
				VisualDiagnostics.RegisterSourceInfo(bindingExtension4 = new BindingExtension(), new Uri("DTCv2\\DTCv2Page.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 391, 49);
				Label label3;
				VisualDiagnostics.RegisterSourceInfo(label3 = new Label(), new Uri("DTCv2\\DTCv2Page.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 388, 46);
				DynamicResourceExtension dynamicResourceExtension4;
				VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension4 = new DynamicResourceExtension(), new Uri("DTCv2\\DTCv2Page.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 395, 49);
				StaticResourceExtension staticResourceExtension3;
				VisualDiagnostics.RegisterSourceInfo(staticResourceExtension3 = new StaticResourceExtension(), new Uri("DTCv2\\DTCv2Page.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 396, 49);
				BindingExtension bindingExtension5;
				VisualDiagnostics.RegisterSourceInfo(bindingExtension5 = new BindingExtension(), new Uri("DTCv2\\DTCv2Page.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 396, 49);
				BindingExtension bindingExtension6;
				VisualDiagnostics.RegisterSourceInfo(bindingExtension6 = new BindingExtension(), new Uri("DTCv2\\DTCv2Page.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 397, 49);
				Label label4;
				VisualDiagnostics.RegisterSourceInfo(label4 = new Label(), new Uri("DTCv2\\DTCv2Page.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 394, 46);
				DynamicResourceExtension dynamicResourceExtension5;
				VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension5 = new DynamicResourceExtension(), new Uri("DTCv2\\DTCv2Page.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 401, 49);
				StaticResourceExtension staticResourceExtension4;
				VisualDiagnostics.RegisterSourceInfo(staticResourceExtension4 = new StaticResourceExtension(), new Uri("DTCv2\\DTCv2Page.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 402, 49);
				BindingExtension bindingExtension7;
				VisualDiagnostics.RegisterSourceInfo(bindingExtension7 = new BindingExtension(), new Uri("DTCv2\\DTCv2Page.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 402, 49);
				BindingExtension bindingExtension8;
				VisualDiagnostics.RegisterSourceInfo(bindingExtension8 = new BindingExtension(), new Uri("DTCv2\\DTCv2Page.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 404, 49);
				Label label5;
				VisualDiagnostics.RegisterSourceInfo(label5 = new Label(), new Uri("DTCv2\\DTCv2Page.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 399, 46);
				DynamicResourceExtension dynamicResourceExtension6;
				VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension6 = new DynamicResourceExtension(), new Uri("DTCv2\\DTCv2Page.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 407, 49);
				Translate translate2;
				VisualDiagnostics.RegisterSourceInfo(translate2 = new Translate(), new Uri("DTCv2\\DTCv2Page.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 409, 49);
				Label label6;
				VisualDiagnostics.RegisterSourceInfo(label6 = new Label(), new Uri("DTCv2\\DTCv2Page.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 405, 46);
				StackLayout stackLayout2;
				VisualDiagnostics.RegisterSourceInfo(stackLayout2 = new StackLayout(), new Uri("DTCv2\\DTCv2Page.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 375, 42);
				Grid grid;
				VisualDiagnostics.RegisterSourceInfo(grid = new Grid(), new Uri("DTCv2\\DTCv2Page.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 370, 38);
				Grid grid2;
				VisualDiagnostics.RegisterSourceInfo(grid2 = new Grid(), new Uri("DTCv2\\DTCv2Page.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 347, 34);
				ViewCell viewCell;
				VisualDiagnostics.RegisterSourceInfo(viewCell = new ViewCell(), new Uri("DTCv2\\DTCv2Page.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 346, 30);
				NameScope nameScope = new NameScope();
				NameScope.SetNameScope(viewCell, nameScope);
				nameScope.RegisterName("labelCode", label);
				if (label.StyleId == null)
				{
					label.StyleId = "labelCode";
				}
				grid2.SetValue(View.MarginProperty, new Thickness(0.0, 0.0, 0.0, 5.0));
				grid2.SetValue(Grid.RowSpacingProperty, 0.0);
				rowDefinition.SetValue(RowDefinition.HeightProperty, new GridLengthTypeConverter().ConvertFromInvariantString("auto"));
				grid2.GetValue(Grid.RowDefinitionsProperty).Add(rowDefinition);
				rowDefinition2.SetValue(RowDefinition.HeightProperty, new GridLengthTypeConverter().ConvertFromInvariantString("auto"));
				grid2.GetValue(Grid.RowDefinitionsProperty).Add(rowDefinition2);
				stackLayout.SetValue(StackLayout.OrientationProperty, 1);
				label.SetValue(Grid.RowProperty, 0);
				label.SetValue(Label.FontAttributesProperty, new FontAttributesConverter().ConvertFromInvariantString("Bold"));
				dynamicResourceExtension.Key = "BaseFontSize+";
				IMarkupExtension<DynamicResource> markupExtension = dynamicResourceExtension;
				XamlServiceProvider xamlServiceProvider = new XamlServiceProvider();
				Type typeFromHandle = typeof(IProvideValueTarget);
				int num;
				object[] array = new object[(num = this.parentValues.Length) + 4];
				Array.Copy(this.parentValues, 0, array, 4, num);
				object[] array2 = array;
				array2[0] = label;
				array2[1] = stackLayout;
				array2[2] = grid2;
				array2[3] = viewCell;
				object obj;
				xamlServiceProvider.Add(typeFromHandle, obj = new SimpleValueTargetProvider(array2, Label.FontSizeProperty, nameScope));
				xamlServiceProvider.Add(typeof(IReferenceProvider), obj);
				Type typeFromHandle2 = typeof(IXamlTypeResolver);
				XmlNamespaceResolver xmlNamespaceResolver = new XmlNamespaceResolver();
				xmlNamespaceResolver.Add("", "http://xamarin.com/schemas/2014/forms");
				xmlNamespaceResolver.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
				xmlNamespaceResolver.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
				xmlNamespaceResolver.Add("dtcv2", "clr-namespace:CarScannerXamarinForms.DTCv2");
				xmlNamespaceResolver.Add("ecuModels", "clr-namespace:CarScannerXamarinForms.ECUModels");
				xmlNamespaceResolver.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
				xmlNamespaceResolver.Add("local", "clr-namespace:CarScannerXamarinForms");
				xmlNamespaceResolver.Add("settings", "clr-namespace:CarScannerXamarinForms.Settings");
				xmlNamespaceResolver.Add("syncfusion", "clr-namespace:Syncfusion.XForms.Buttons;assembly=Syncfusion.Buttons.XForms");
				xmlNamespaceResolver.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
				xamlServiceProvider.Add(typeFromHandle2, new XamlTypeResolver(xmlNamespaceResolver, typeof(DTCv2Page.<InitializeComponent>_anonXamlCDataTemplate_28).GetTypeInfo().Assembly));
				xamlServiceProvider.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(357, 45)));
				DynamicResource dynamicResource = markupExtension.ProvideValue(xamlServiceProvider);
				label.SetDynamicResource(Label.FontSizeProperty, dynamicResource.Key);
				staticResourceExtension.Key = "DTCCodeToStringConverter";
				IMarkupExtension markupExtension2 = staticResourceExtension;
				XamlServiceProvider xamlServiceProvider2 = new XamlServiceProvider();
				Type typeFromHandle3 = typeof(IProvideValueTarget);
				int num2;
				object[] array3 = new object[(num2 = this.parentValues.Length) + 5];
				Array.Copy(this.parentValues, 0, array3, 5, num2);
				object[] array4 = array3;
				array4[0] = bindingExtension;
				array4[1] = label;
				array4[2] = stackLayout;
				array4[3] = grid2;
				array4[4] = viewCell;
				object obj2;
				xamlServiceProvider2.Add(typeFromHandle3, obj2 = new SimpleValueTargetProvider(array4, typeof(BindingExtension).GetRuntimeProperty("Converter"), nameScope));
				xamlServiceProvider2.Add(typeof(IReferenceProvider), obj2);
				Type typeFromHandle4 = typeof(IXamlTypeResolver);
				XmlNamespaceResolver xmlNamespaceResolver2 = new XmlNamespaceResolver();
				xmlNamespaceResolver2.Add("", "http://xamarin.com/schemas/2014/forms");
				xmlNamespaceResolver2.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
				xmlNamespaceResolver2.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
				xmlNamespaceResolver2.Add("dtcv2", "clr-namespace:CarScannerXamarinForms.DTCv2");
				xmlNamespaceResolver2.Add("ecuModels", "clr-namespace:CarScannerXamarinForms.ECUModels");
				xmlNamespaceResolver2.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
				xmlNamespaceResolver2.Add("local", "clr-namespace:CarScannerXamarinForms");
				xmlNamespaceResolver2.Add("settings", "clr-namespace:CarScannerXamarinForms.Settings");
				xmlNamespaceResolver2.Add("syncfusion", "clr-namespace:Syncfusion.XForms.Buttons;assembly=Syncfusion.Buttons.XForms");
				xmlNamespaceResolver2.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
				xamlServiceProvider2.Add(typeFromHandle4, new XamlTypeResolver(xmlNamespaceResolver2, typeof(DTCv2Page.<InitializeComponent>_anonXamlCDataTemplate_28).GetTypeInfo().Assembly));
				xamlServiceProvider2.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(358, 45)));
				object obj3 = markupExtension2.ProvideValue(xamlServiceProvider2);
				bindingExtension.Converter = obj3;
				bindingExtension.Path = "Code";
				BindingBase bindingBase = bindingExtension.ProvideValue(null);
				label.SetBinding(Label.TextProperty, bindingBase);
				stackLayout.Children.Add(label);
				label2.SetValue(View.MarginProperty, new Thickness(5.0, 0.0, 0.0, 0.0));
				label2.SetValue(Label.FontAttributesProperty, new FontAttributesConverter().ConvertFromInvariantString("Bold"));
				dynamicResourceExtension2.Key = "BaseFontSize";
				IMarkupExtension<DynamicResource> markupExtension3 = dynamicResourceExtension2;
				XamlServiceProvider xamlServiceProvider3 = new XamlServiceProvider();
				Type typeFromHandle5 = typeof(IProvideValueTarget);
				int num3;
				object[] array5 = new object[(num3 = this.parentValues.Length) + 4];
				Array.Copy(this.parentValues, 0, array5, 4, num3);
				object[] array6 = array5;
				array6[0] = label2;
				array6[1] = stackLayout;
				array6[2] = grid2;
				array6[3] = viewCell;
				object obj4;
				xamlServiceProvider3.Add(typeFromHandle5, obj4 = new SimpleValueTargetProvider(array6, Label.FontSizeProperty, nameScope));
				xamlServiceProvider3.Add(typeof(IReferenceProvider), obj4);
				Type typeFromHandle6 = typeof(IXamlTypeResolver);
				XmlNamespaceResolver xmlNamespaceResolver3 = new XmlNamespaceResolver();
				xmlNamespaceResolver3.Add("", "http://xamarin.com/schemas/2014/forms");
				xmlNamespaceResolver3.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
				xmlNamespaceResolver3.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
				xmlNamespaceResolver3.Add("dtcv2", "clr-namespace:CarScannerXamarinForms.DTCv2");
				xmlNamespaceResolver3.Add("ecuModels", "clr-namespace:CarScannerXamarinForms.ECUModels");
				xmlNamespaceResolver3.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
				xmlNamespaceResolver3.Add("local", "clr-namespace:CarScannerXamarinForms");
				xmlNamespaceResolver3.Add("settings", "clr-namespace:CarScannerXamarinForms.Settings");
				xmlNamespaceResolver3.Add("syncfusion", "clr-namespace:Syncfusion.XForms.Buttons;assembly=Syncfusion.Buttons.XForms");
				xmlNamespaceResolver3.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
				xamlServiceProvider3.Add(typeFromHandle6, new XamlTypeResolver(xmlNamespaceResolver3, typeof(DTCv2Page.<InitializeComponent>_anonXamlCDataTemplate_28).GetTypeInfo().Assembly));
				xamlServiceProvider3.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(363, 45)));
				DynamicResource dynamicResource2 = markupExtension3.ProvideValue(xamlServiceProvider3);
				label2.SetDynamicResource(Label.FontSizeProperty, dynamicResource2.Key);
				bindingExtension2.Path = "IsArchive";
				BindingBase bindingBase2 = bindingExtension2.ProvideValue(null);
				label2.SetBinding(VisualElement.IsVisibleProperty, bindingBase2);
				translate.Text = "ios_Archive";
				IMarkupExtension markupExtension4 = translate;
				XamlServiceProvider xamlServiceProvider4 = new XamlServiceProvider();
				Type typeFromHandle7 = typeof(IProvideValueTarget);
				int num4;
				object[] array7 = new object[(num4 = this.parentValues.Length) + 4];
				Array.Copy(this.parentValues, 0, array7, 4, num4);
				object[] array8 = array7;
				array8[0] = label2;
				array8[1] = stackLayout;
				array8[2] = grid2;
				array8[3] = viewCell;
				object obj5;
				xamlServiceProvider4.Add(typeFromHandle7, obj5 = new SimpleValueTargetProvider(array8, Label.TextProperty, nameScope));
				xamlServiceProvider4.Add(typeof(IReferenceProvider), obj5);
				Type typeFromHandle8 = typeof(IXamlTypeResolver);
				XmlNamespaceResolver xmlNamespaceResolver4 = new XmlNamespaceResolver();
				xmlNamespaceResolver4.Add("", "http://xamarin.com/schemas/2014/forms");
				xmlNamespaceResolver4.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
				xmlNamespaceResolver4.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
				xmlNamespaceResolver4.Add("dtcv2", "clr-namespace:CarScannerXamarinForms.DTCv2");
				xmlNamespaceResolver4.Add("ecuModels", "clr-namespace:CarScannerXamarinForms.ECUModels");
				xmlNamespaceResolver4.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
				xmlNamespaceResolver4.Add("local", "clr-namespace:CarScannerXamarinForms");
				xmlNamespaceResolver4.Add("settings", "clr-namespace:CarScannerXamarinForms.Settings");
				xmlNamespaceResolver4.Add("syncfusion", "clr-namespace:Syncfusion.XForms.Buttons;assembly=Syncfusion.Buttons.XForms");
				xmlNamespaceResolver4.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
				xamlServiceProvider4.Add(typeFromHandle8, new XamlTypeResolver(xmlNamespaceResolver4, typeof(DTCv2Page.<InitializeComponent>_anonXamlCDataTemplate_28).GetTypeInfo().Assembly));
				xamlServiceProvider4.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(365, 45)));
				object obj6 = markupExtension4.ProvideValue(xamlServiceProvider4);
				label2.Text = obj6;
				label2.SetValue(View.VerticalOptionsProperty, LayoutOptions.Center);
				label2.SetValue(Label.VerticalTextAlignmentProperty, new TextAlignmentConverter().ConvertFromInvariantString("Center"));
				stackLayout.Children.Add(label2);
				grid2.Children.Add(stackLayout);
				grid.SetValue(Grid.RowProperty, 1);
				grid.SetValue(Grid.RowSpacingProperty, 0.0);
				columnDefinition.SetValue(ColumnDefinition.WidthProperty, new GridLengthTypeConverter().ConvertFromInvariantString("*"));
				grid.GetValue(Grid.ColumnDefinitionsProperty).Add(columnDefinition);
				columnDefinition2.SetValue(ColumnDefinition.WidthProperty, new GridLengthTypeConverter().ConvertFromInvariantString("auto"));
				grid.GetValue(Grid.ColumnDefinitionsProperty).Add(columnDefinition2);
				stackLayout2.SetValue(Grid.ColumnProperty, 0);
				stackLayout2.SetValue(StackLayout.OrientationProperty, 0);
				stackLayout2.SetValue(StackLayout.SpacingProperty, 0.0);
				dynamicResourceExtension3.Key = "BaseFontSize+";
				IMarkupExtension<DynamicResource> markupExtension5 = dynamicResourceExtension3;
				XamlServiceProvider xamlServiceProvider5 = new XamlServiceProvider();
				Type typeFromHandle9 = typeof(IProvideValueTarget);
				int num5;
				object[] array9 = new object[(num5 = this.parentValues.Length) + 5];
				Array.Copy(this.parentValues, 0, array9, 5, num5);
				object[] array10 = array9;
				array10[0] = label3;
				array10[1] = stackLayout2;
				array10[2] = grid;
				array10[3] = grid2;
				array10[4] = viewCell;
				object obj7;
				xamlServiceProvider5.Add(typeFromHandle9, obj7 = new SimpleValueTargetProvider(array10, Label.FontSizeProperty, nameScope));
				xamlServiceProvider5.Add(typeof(IReferenceProvider), obj7);
				Type typeFromHandle10 = typeof(IXamlTypeResolver);
				XmlNamespaceResolver xmlNamespaceResolver5 = new XmlNamespaceResolver();
				xmlNamespaceResolver5.Add("", "http://xamarin.com/schemas/2014/forms");
				xmlNamespaceResolver5.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
				xmlNamespaceResolver5.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
				xmlNamespaceResolver5.Add("dtcv2", "clr-namespace:CarScannerXamarinForms.DTCv2");
				xmlNamespaceResolver5.Add("ecuModels", "clr-namespace:CarScannerXamarinForms.ECUModels");
				xmlNamespaceResolver5.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
				xmlNamespaceResolver5.Add("local", "clr-namespace:CarScannerXamarinForms");
				xmlNamespaceResolver5.Add("settings", "clr-namespace:CarScannerXamarinForms.Settings");
				xmlNamespaceResolver5.Add("syncfusion", "clr-namespace:Syncfusion.XForms.Buttons;assembly=Syncfusion.Buttons.XForms");
				xmlNamespaceResolver5.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
				xamlServiceProvider5.Add(typeFromHandle10, new XamlTypeResolver(xmlNamespaceResolver5, typeof(DTCv2Page.<InitializeComponent>_anonXamlCDataTemplate_28).GetTypeInfo().Assembly));
				xamlServiceProvider5.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(389, 49)));
				DynamicResource dynamicResource3 = markupExtension5.ProvideValue(xamlServiceProvider5);
				label3.SetDynamicResource(Label.FontSizeProperty, dynamicResource3.Key);
				staticResourceExtension2.Key = "DTCDescriptionCollectionToStringConverter";
				IMarkupExtension markupExtension6 = staticResourceExtension2;
				XamlServiceProvider xamlServiceProvider6 = new XamlServiceProvider();
				Type typeFromHandle11 = typeof(IProvideValueTarget);
				int num6;
				object[] array11 = new object[(num6 = this.parentValues.Length) + 6];
				Array.Copy(this.parentValues, 0, array11, 6, num6);
				object[] array12 = array11;
				array12[0] = bindingExtension3;
				array12[1] = label3;
				array12[2] = stackLayout2;
				array12[3] = grid;
				array12[4] = grid2;
				array12[5] = viewCell;
				object obj8;
				xamlServiceProvider6.Add(typeFromHandle11, obj8 = new SimpleValueTargetProvider(array12, typeof(BindingExtension).GetRuntimeProperty("Converter"), nameScope));
				xamlServiceProvider6.Add(typeof(IReferenceProvider), obj8);
				Type typeFromHandle12 = typeof(IXamlTypeResolver);
				XmlNamespaceResolver xmlNamespaceResolver6 = new XmlNamespaceResolver();
				xmlNamespaceResolver6.Add("", "http://xamarin.com/schemas/2014/forms");
				xmlNamespaceResolver6.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
				xmlNamespaceResolver6.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
				xmlNamespaceResolver6.Add("dtcv2", "clr-namespace:CarScannerXamarinForms.DTCv2");
				xmlNamespaceResolver6.Add("ecuModels", "clr-namespace:CarScannerXamarinForms.ECUModels");
				xmlNamespaceResolver6.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
				xmlNamespaceResolver6.Add("local", "clr-namespace:CarScannerXamarinForms");
				xmlNamespaceResolver6.Add("settings", "clr-namespace:CarScannerXamarinForms.Settings");
				xmlNamespaceResolver6.Add("syncfusion", "clr-namespace:Syncfusion.XForms.Buttons;assembly=Syncfusion.Buttons.XForms");
				xmlNamespaceResolver6.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
				xamlServiceProvider6.Add(typeFromHandle12, new XamlTypeResolver(xmlNamespaceResolver6, typeof(DTCv2Page.<InitializeComponent>_anonXamlCDataTemplate_28).GetTypeInfo().Assembly));
				xamlServiceProvider6.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(390, 49)));
				object obj9 = markupExtension6.ProvideValue(xamlServiceProvider6);
				bindingExtension3.Converter = obj9;
				bindingExtension3.Path = "Descriptions";
				BindingBase bindingBase3 = bindingExtension3.ProvideValue(null);
				label3.SetBinding(Label.FormattedTextProperty, bindingBase3);
				bindingExtension4.Path = "DescriptionsVisible";
				BindingBase bindingBase4 = bindingExtension4.ProvideValue(null);
				label3.SetBinding(VisualElement.IsVisibleProperty, bindingBase4);
				label3.SetValue(Label.LineBreakModeProperty, 1);
				stackLayout2.Children.Add(label3);
				dynamicResourceExtension4.Key = "BaseFontSize";
				IMarkupExtension<DynamicResource> markupExtension7 = dynamicResourceExtension4;
				XamlServiceProvider xamlServiceProvider7 = new XamlServiceProvider();
				Type typeFromHandle13 = typeof(IProvideValueTarget);
				int num7;
				object[] array13 = new object[(num7 = this.parentValues.Length) + 5];
				Array.Copy(this.parentValues, 0, array13, 5, num7);
				object[] array14 = array13;
				array14[0] = label4;
				array14[1] = stackLayout2;
				array14[2] = grid;
				array14[3] = grid2;
				array14[4] = viewCell;
				object obj10;
				xamlServiceProvider7.Add(typeFromHandle13, obj10 = new SimpleValueTargetProvider(array14, Label.FontSizeProperty, nameScope));
				xamlServiceProvider7.Add(typeof(IReferenceProvider), obj10);
				Type typeFromHandle14 = typeof(IXamlTypeResolver);
				XmlNamespaceResolver xmlNamespaceResolver7 = new XmlNamespaceResolver();
				xmlNamespaceResolver7.Add("", "http://xamarin.com/schemas/2014/forms");
				xmlNamespaceResolver7.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
				xmlNamespaceResolver7.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
				xmlNamespaceResolver7.Add("dtcv2", "clr-namespace:CarScannerXamarinForms.DTCv2");
				xmlNamespaceResolver7.Add("ecuModels", "clr-namespace:CarScannerXamarinForms.ECUModels");
				xmlNamespaceResolver7.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
				xmlNamespaceResolver7.Add("local", "clr-namespace:CarScannerXamarinForms");
				xmlNamespaceResolver7.Add("settings", "clr-namespace:CarScannerXamarinForms.Settings");
				xmlNamespaceResolver7.Add("syncfusion", "clr-namespace:Syncfusion.XForms.Buttons;assembly=Syncfusion.Buttons.XForms");
				xmlNamespaceResolver7.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
				xamlServiceProvider7.Add(typeFromHandle14, new XamlTypeResolver(xmlNamespaceResolver7, typeof(DTCv2Page.<InitializeComponent>_anonXamlCDataTemplate_28).GetTypeInfo().Assembly));
				xamlServiceProvider7.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(395, 49)));
				DynamicResource dynamicResource4 = markupExtension7.ProvideValue(xamlServiceProvider7);
				label4.SetDynamicResource(Label.FontSizeProperty, dynamicResource4.Key);
				staticResourceExtension3.Key = "DTCStatusCollectionToStringConverter";
				IMarkupExtension markupExtension8 = staticResourceExtension3;
				XamlServiceProvider xamlServiceProvider8 = new XamlServiceProvider();
				Type typeFromHandle15 = typeof(IProvideValueTarget);
				int num8;
				object[] array15 = new object[(num8 = this.parentValues.Length) + 6];
				Array.Copy(this.parentValues, 0, array15, 6, num8);
				object[] array16 = array15;
				array16[0] = bindingExtension5;
				array16[1] = label4;
				array16[2] = stackLayout2;
				array16[3] = grid;
				array16[4] = grid2;
				array16[5] = viewCell;
				object obj11;
				xamlServiceProvider8.Add(typeFromHandle15, obj11 = new SimpleValueTargetProvider(array16, typeof(BindingExtension).GetRuntimeProperty("Converter"), nameScope));
				xamlServiceProvider8.Add(typeof(IReferenceProvider), obj11);
				Type typeFromHandle16 = typeof(IXamlTypeResolver);
				XmlNamespaceResolver xmlNamespaceResolver8 = new XmlNamespaceResolver();
				xmlNamespaceResolver8.Add("", "http://xamarin.com/schemas/2014/forms");
				xmlNamespaceResolver8.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
				xmlNamespaceResolver8.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
				xmlNamespaceResolver8.Add("dtcv2", "clr-namespace:CarScannerXamarinForms.DTCv2");
				xmlNamespaceResolver8.Add("ecuModels", "clr-namespace:CarScannerXamarinForms.ECUModels");
				xmlNamespaceResolver8.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
				xmlNamespaceResolver8.Add("local", "clr-namespace:CarScannerXamarinForms");
				xmlNamespaceResolver8.Add("settings", "clr-namespace:CarScannerXamarinForms.Settings");
				xmlNamespaceResolver8.Add("syncfusion", "clr-namespace:Syncfusion.XForms.Buttons;assembly=Syncfusion.Buttons.XForms");
				xmlNamespaceResolver8.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
				xamlServiceProvider8.Add(typeFromHandle16, new XamlTypeResolver(xmlNamespaceResolver8, typeof(DTCv2Page.<InitializeComponent>_anonXamlCDataTemplate_28).GetTypeInfo().Assembly));
				xamlServiceProvider8.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(396, 49)));
				object obj12 = markupExtension8.ProvideValue(xamlServiceProvider8);
				bindingExtension5.Converter = obj12;
				bindingExtension5.Path = "Statuses";
				BindingBase bindingBase5 = bindingExtension5.ProvideValue(null);
				label4.SetBinding(Label.FormattedTextProperty, bindingBase5);
				bindingExtension6.Path = "StatusVisible";
				BindingBase bindingBase6 = bindingExtension6.ProvideValue(null);
				label4.SetBinding(VisualElement.IsVisibleProperty, bindingBase6);
				label4.SetValue(Label.LineBreakModeProperty, 1);
				stackLayout2.Children.Add(label4);
				label5.SetValue(Label.FontAttributesProperty, new FontAttributesConverter().ConvertFromInvariantString("Italic"));
				dynamicResourceExtension5.Key = "BaseFontSize";
				IMarkupExtension<DynamicResource> markupExtension9 = dynamicResourceExtension5;
				XamlServiceProvider xamlServiceProvider9 = new XamlServiceProvider();
				Type typeFromHandle17 = typeof(IProvideValueTarget);
				int num9;
				object[] array17 = new object[(num9 = this.parentValues.Length) + 5];
				Array.Copy(this.parentValues, 0, array17, 5, num9);
				object[] array18 = array17;
				array18[0] = label5;
				array18[1] = stackLayout2;
				array18[2] = grid;
				array18[3] = grid2;
				array18[4] = viewCell;
				object obj13;
				xamlServiceProvider9.Add(typeFromHandle17, obj13 = new SimpleValueTargetProvider(array18, Label.FontSizeProperty, nameScope));
				xamlServiceProvider9.Add(typeof(IReferenceProvider), obj13);
				Type typeFromHandle18 = typeof(IXamlTypeResolver);
				XmlNamespaceResolver xmlNamespaceResolver9 = new XmlNamespaceResolver();
				xmlNamespaceResolver9.Add("", "http://xamarin.com/schemas/2014/forms");
				xmlNamespaceResolver9.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
				xmlNamespaceResolver9.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
				xmlNamespaceResolver9.Add("dtcv2", "clr-namespace:CarScannerXamarinForms.DTCv2");
				xmlNamespaceResolver9.Add("ecuModels", "clr-namespace:CarScannerXamarinForms.ECUModels");
				xmlNamespaceResolver9.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
				xmlNamespaceResolver9.Add("local", "clr-namespace:CarScannerXamarinForms");
				xmlNamespaceResolver9.Add("settings", "clr-namespace:CarScannerXamarinForms.Settings");
				xmlNamespaceResolver9.Add("syncfusion", "clr-namespace:Syncfusion.XForms.Buttons;assembly=Syncfusion.Buttons.XForms");
				xmlNamespaceResolver9.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
				xamlServiceProvider9.Add(typeFromHandle18, new XamlTypeResolver(xmlNamespaceResolver9, typeof(DTCv2Page.<InitializeComponent>_anonXamlCDataTemplate_28).GetTypeInfo().Assembly));
				xamlServiceProvider9.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(401, 49)));
				DynamicResource dynamicResource5 = markupExtension9.ProvideValue(xamlServiceProvider9);
				label5.SetDynamicResource(Label.FontSizeProperty, dynamicResource5.Key);
				bindingExtension7.Mode = 2;
				staticResourceExtension4.Key = "EmptyStringToFalseConverter";
				IMarkupExtension markupExtension10 = staticResourceExtension4;
				XamlServiceProvider xamlServiceProvider10 = new XamlServiceProvider();
				Type typeFromHandle19 = typeof(IProvideValueTarget);
				int num10;
				object[] array19 = new object[(num10 = this.parentValues.Length) + 6];
				Array.Copy(this.parentValues, 0, array19, 6, num10);
				object[] array20 = array19;
				array20[0] = bindingExtension7;
				array20[1] = label5;
				array20[2] = stackLayout2;
				array20[3] = grid;
				array20[4] = grid2;
				array20[5] = viewCell;
				object obj14;
				xamlServiceProvider10.Add(typeFromHandle19, obj14 = new SimpleValueTargetProvider(array20, typeof(BindingExtension).GetRuntimeProperty("Converter"), nameScope));
				xamlServiceProvider10.Add(typeof(IReferenceProvider), obj14);
				Type typeFromHandle20 = typeof(IXamlTypeResolver);
				XmlNamespaceResolver xmlNamespaceResolver10 = new XmlNamespaceResolver();
				xmlNamespaceResolver10.Add("", "http://xamarin.com/schemas/2014/forms");
				xmlNamespaceResolver10.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
				xmlNamespaceResolver10.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
				xmlNamespaceResolver10.Add("dtcv2", "clr-namespace:CarScannerXamarinForms.DTCv2");
				xmlNamespaceResolver10.Add("ecuModels", "clr-namespace:CarScannerXamarinForms.ECUModels");
				xmlNamespaceResolver10.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
				xmlNamespaceResolver10.Add("local", "clr-namespace:CarScannerXamarinForms");
				xmlNamespaceResolver10.Add("settings", "clr-namespace:CarScannerXamarinForms.Settings");
				xmlNamespaceResolver10.Add("syncfusion", "clr-namespace:Syncfusion.XForms.Buttons;assembly=Syncfusion.Buttons.XForms");
				xmlNamespaceResolver10.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
				xamlServiceProvider10.Add(typeFromHandle20, new XamlTypeResolver(xmlNamespaceResolver10, typeof(DTCv2Page.<InitializeComponent>_anonXamlCDataTemplate_28).GetTypeInfo().Assembly));
				xamlServiceProvider10.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(402, 49)));
				object obj15 = markupExtension10.ProvideValue(xamlServiceProvider10);
				bindingExtension7.Converter = obj15;
				bindingExtension7.Path = "Payload";
				BindingBase bindingBase7 = bindingExtension7.ProvideValue(null);
				label5.SetBinding(VisualElement.IsVisibleProperty, bindingBase7);
				label5.SetValue(Label.LineBreakModeProperty, 1);
				bindingExtension8.Mode = 2;
				bindingExtension8.Path = "Payload";
				BindingBase bindingBase8 = bindingExtension8.ProvideValue(null);
				label5.SetBinding(Label.TextProperty, bindingBase8);
				stackLayout2.Children.Add(label5);
				label6.SetValue(Label.FontAttributesProperty, new FontAttributesConverter().ConvertFromInvariantString("Italic"));
				dynamicResourceExtension6.Key = "BaseFontSize";
				IMarkupExtension<DynamicResource> markupExtension11 = dynamicResourceExtension6;
				XamlServiceProvider xamlServiceProvider11 = new XamlServiceProvider();
				Type typeFromHandle21 = typeof(IProvideValueTarget);
				int num11;
				object[] array21 = new object[(num11 = this.parentValues.Length) + 5];
				Array.Copy(this.parentValues, 0, array21, 5, num11);
				object[] array22 = array21;
				array22[0] = label6;
				array22[1] = stackLayout2;
				array22[2] = grid;
				array22[3] = grid2;
				array22[4] = viewCell;
				object obj16;
				xamlServiceProvider11.Add(typeFromHandle21, obj16 = new SimpleValueTargetProvider(array22, Label.FontSizeProperty, nameScope));
				xamlServiceProvider11.Add(typeof(IReferenceProvider), obj16);
				Type typeFromHandle22 = typeof(IXamlTypeResolver);
				XmlNamespaceResolver xmlNamespaceResolver11 = new XmlNamespaceResolver();
				xmlNamespaceResolver11.Add("", "http://xamarin.com/schemas/2014/forms");
				xmlNamespaceResolver11.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
				xmlNamespaceResolver11.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
				xmlNamespaceResolver11.Add("dtcv2", "clr-namespace:CarScannerXamarinForms.DTCv2");
				xmlNamespaceResolver11.Add("ecuModels", "clr-namespace:CarScannerXamarinForms.ECUModels");
				xmlNamespaceResolver11.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
				xmlNamespaceResolver11.Add("local", "clr-namespace:CarScannerXamarinForms");
				xmlNamespaceResolver11.Add("settings", "clr-namespace:CarScannerXamarinForms.Settings");
				xmlNamespaceResolver11.Add("syncfusion", "clr-namespace:Syncfusion.XForms.Buttons;assembly=Syncfusion.Buttons.XForms");
				xmlNamespaceResolver11.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
				xamlServiceProvider11.Add(typeFromHandle22, new XamlTypeResolver(xmlNamespaceResolver11, typeof(DTCv2Page.<InitializeComponent>_anonXamlCDataTemplate_28).GetTypeInfo().Assembly));
				xamlServiceProvider11.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(407, 49)));
				DynamicResource dynamicResource6 = markupExtension11.ProvideValue(xamlServiceProvider11);
				label6.SetDynamicResource(Label.FontSizeProperty, dynamicResource6.Key);
				label6.SetValue(Label.LineBreakModeProperty, 1);
				translate2.Text = "ios_TapToGetDescription";
				IMarkupExtension markupExtension12 = translate2;
				XamlServiceProvider xamlServiceProvider12 = new XamlServiceProvider();
				Type typeFromHandle23 = typeof(IProvideValueTarget);
				int num12;
				object[] array23 = new object[(num12 = this.parentValues.Length) + 5];
				Array.Copy(this.parentValues, 0, array23, 5, num12);
				object[] array24 = array23;
				array24[0] = label6;
				array24[1] = stackLayout2;
				array24[2] = grid;
				array24[3] = grid2;
				array24[4] = viewCell;
				object obj17;
				xamlServiceProvider12.Add(typeFromHandle23, obj17 = new SimpleValueTargetProvider(array24, Label.TextProperty, nameScope));
				xamlServiceProvider12.Add(typeof(IReferenceProvider), obj17);
				Type typeFromHandle24 = typeof(IXamlTypeResolver);
				XmlNamespaceResolver xmlNamespaceResolver12 = new XmlNamespaceResolver();
				xmlNamespaceResolver12.Add("", "http://xamarin.com/schemas/2014/forms");
				xmlNamespaceResolver12.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
				xmlNamespaceResolver12.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
				xmlNamespaceResolver12.Add("dtcv2", "clr-namespace:CarScannerXamarinForms.DTCv2");
				xmlNamespaceResolver12.Add("ecuModels", "clr-namespace:CarScannerXamarinForms.ECUModels");
				xmlNamespaceResolver12.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
				xmlNamespaceResolver12.Add("local", "clr-namespace:CarScannerXamarinForms");
				xmlNamespaceResolver12.Add("settings", "clr-namespace:CarScannerXamarinForms.Settings");
				xmlNamespaceResolver12.Add("syncfusion", "clr-namespace:Syncfusion.XForms.Buttons;assembly=Syncfusion.Buttons.XForms");
				xmlNamespaceResolver12.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
				xamlServiceProvider12.Add(typeFromHandle24, new XamlTypeResolver(xmlNamespaceResolver12, typeof(DTCv2Page.<InitializeComponent>_anonXamlCDataTemplate_28).GetTypeInfo().Assembly));
				xamlServiceProvider12.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(409, 49)));
				object obj18 = markupExtension12.ProvideValue(xamlServiceProvider12);
				label6.Text = obj18;
				stackLayout2.Children.Add(label6);
				grid.Children.Add(stackLayout2);
				grid2.Children.Add(grid);
				viewCell.View = grid2;
				return viewCell;
			}

			// Token: 0x04001F35 RID: 7989
			internal object[] parentValues;

			// Token: 0x04001F36 RID: 7990
			internal DTCv2Page root;
		}
	}
}
