using System;
using System.CodeDom.Compiler;
using System.Diagnostics;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using CarScannerXamarinForms.DTCv2;
using CarScannerXamarinForms.ECUModels;
using CarScannerXamarinForms.Settings;
using CarScannerXamarinForms.UserControls;
using Xamarin.Forms;
using Xamarin.Forms.Internals;
using Xamarin.Forms.PlatformConfiguration.iOSSpecific;
using Xamarin.Forms.Xaml;
using Xamarin.Forms.Xaml.Diagnostics;
using Xamarin.Forms.Xaml.Internals;

namespace CarScannerXamarinForms.Pages
{
	// Token: 0x02000626 RID: 1574
	[XamlCompilation(2)]
	[XamlFilePath("Pages\\EcuInfoPageV2.xaml")]
	public class EcuInfoPageV2 : ContentPage
	{
		// Token: 0x06003716 RID: 14102 RVA: 0x0028E864 File Offset: 0x0028CA64
		public EcuInfoPageV2()
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
			this.Model = new DTCv2Model();
			base.BindingContext = this.Model;
			if (this.Model.ECUListFull.Count == 1)
			{
				this.Model.ECUListFull[0].IsSelected = true;
				this.Model.ReadECUInfoCommand.Execute(null);
			}
		}

		// Token: 0x06003717 RID: 14103 RVA: 0x0028E8F5 File Offset: 0x0028CAF5
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

		// Token: 0x06003718 RID: 14104 RVA: 0x000027D4 File Offset: 0x000009D4
		private void LabelBadDTCTapGestureRecognizer_Tapped(object sender, EventArgs e)
		{
		}

		// Token: 0x06003719 RID: 14105 RVA: 0x0028E92F File Offset: 0x0028CB2F
		protected override bool OnBackButtonPressed()
		{
			if (this.Model.ECUListFull.Count == 1)
			{
				base.Navigation.PopAsync();
				return true;
			}
			this.Model.BackButtonCommand.Execute(null);
			return true;
		}

		// Token: 0x0600371A RID: 14106 RVA: 0x0028E964 File Offset: 0x0028CB64
		private async void ButtonBackCancel_Clicked(object sender, EventArgs e)
		{
			(sender as View).IsEnabled = false;
			if (this.Model.ECUListFull.Count == 1)
			{
				await base.Navigation.PopAsync();
			}
			(sender as View).IsEnabled = true;
		}

		// Token: 0x0600371B RID: 14107 RVA: 0x0028E9A4 File Offset: 0x0028CBA4
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private void InitializeComponent()
		{
			if (ResourceLoader.IsEnabled && ResourceLoader.CanProvideContentFor(new ResourceLoader.ResourceLoadingQuery
			{
				AssemblyName = typeof(EcuInfoPageV2).GetTypeInfo().Assembly.GetName(),
				ResourcePath = "Pages/EcuInfoPageV2.xaml",
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
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension = new DynamicResourceExtension(), new Uri("Pages\\EcuInfoPageV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 12, 5);
			DTCStatusCollectionToStringConverter dtcstatusCollectionToStringConverter;
			VisualDiagnostics.RegisterSourceInfo(dtcstatusCollectionToStringConverter = new DTCStatusCollectionToStringConverter(), new Uri("Pages\\EcuInfoPageV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 17, 14);
			DTCDescriptionCollectionToStringConverter dtcdescriptionCollectionToStringConverter;
			VisualDiagnostics.RegisterSourceInfo(dtcdescriptionCollectionToStringConverter = new DTCDescriptionCollectionToStringConverter(), new Uri("Pages\\EcuInfoPageV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 18, 14);
			DTCCodeToStringConverter dtccodeToStringConverter;
			VisualDiagnostics.RegisterSourceInfo(dtccodeToStringConverter = new DTCCodeToStringConverter(), new Uri("Pages\\EcuInfoPageV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 19, 14);
			EmptyStringToFalseConverter emptyStringToFalseConverter;
			VisualDiagnostics.RegisterSourceInfo(emptyStringToFalseConverter = new EmptyStringToFalseConverter(), new Uri("Pages\\EcuInfoPageV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 20, 14);
			BoolToNegativeConverter boolToNegativeConverter;
			VisualDiagnostics.RegisterSourceInfo(boolToNegativeConverter = new BoolToNegativeConverter(), new Uri("Pages\\EcuInfoPageV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 21, 14);
			ResourceDictionary resourceDictionary;
			VisualDiagnostics.RegisterSourceInfo(resourceDictionary = new ResourceDictionary(), new Uri("Pages\\EcuInfoPageV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 16, 10);
			ColumnDefinition columnDefinition;
			VisualDiagnostics.RegisterSourceInfo(columnDefinition = new ColumnDefinition(), new Uri("Pages\\EcuInfoPageV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 27, 18);
			ColumnDefinition columnDefinition2;
			VisualDiagnostics.RegisterSourceInfo(columnDefinition2 = new ColumnDefinition(), new Uri("Pages\\EcuInfoPageV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 29, 18);
			ColumnDefinition columnDefinition3;
			VisualDiagnostics.RegisterSourceInfo(columnDefinition3 = new ColumnDefinition(), new Uri("Pages\\EcuInfoPageV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 31, 18);
			ColumnDefinition columnDefinition4;
			VisualDiagnostics.RegisterSourceInfo(columnDefinition4 = new ColumnDefinition(), new Uri("Pages\\EcuInfoPageV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 32, 18);
			BindingExtension bindingExtension;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension = new BindingExtension(), new Uri("Pages\\EcuInfoPageV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 36, 17);
			DynamicResourceExtension dynamicResourceExtension2;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension2 = new DynamicResourceExtension(), new Uri("Pages\\EcuInfoPageV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 38, 17);
			Translate translate;
			VisualDiagnostics.RegisterSourceInfo(translate = new Translate(), new Uri("Pages\\EcuInfoPageV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 39, 17);
			LinkButton linkButton;
			VisualDiagnostics.RegisterSourceInfo(linkButton = new LinkButton(), new Uri("Pages\\EcuInfoPageV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 34, 14);
			DynamicResourceExtension dynamicResourceExtension3;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension3 = new DynamicResourceExtension(), new Uri("Pages\\EcuInfoPageV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 45, 17);
			Translate translate2;
			VisualDiagnostics.RegisterSourceInfo(translate2 = new Translate(), new Uri("Pages\\EcuInfoPageV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 46, 17);
			NonScalableLabel nonScalableLabel;
			VisualDiagnostics.RegisterSourceInfo(nonScalableLabel = new NonScalableLabel(), new Uri("Pages\\EcuInfoPageV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 40, 14);
			BindingExtension bindingExtension2;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension2 = new BindingExtension(), new Uri("Pages\\EcuInfoPageV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 53, 17);
			DynamicResourceExtension dynamicResourceExtension4;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension4 = new DynamicResourceExtension(), new Uri("Pages\\EcuInfoPageV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 54, 17);
			BindingExtension bindingExtension3;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension3 = new BindingExtension(), new Uri("Pages\\EcuInfoPageV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 56, 43);
			TapGestureRecognizer tapGestureRecognizer;
			VisualDiagnostics.RegisterSourceInfo(tapGestureRecognizer = new TapGestureRecognizer(), new Uri("Pages\\EcuInfoPageV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 56, 22);
			Image image;
			VisualDiagnostics.RegisterSourceInfo(image = new Image(), new Uri("Pages\\EcuInfoPageV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 48, 14);
			BindingExtension bindingExtension4;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension4 = new BindingExtension(), new Uri("Pages\\EcuInfoPageV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 64, 17);
			DynamicResourceExtension dynamicResourceExtension5;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension5 = new DynamicResourceExtension(), new Uri("Pages\\EcuInfoPageV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 65, 17);
			BindingExtension bindingExtension5;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension5 = new BindingExtension(), new Uri("Pages\\EcuInfoPageV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 67, 43);
			TapGestureRecognizer tapGestureRecognizer2;
			VisualDiagnostics.RegisterSourceInfo(tapGestureRecognizer2 = new TapGestureRecognizer(), new Uri("Pages\\EcuInfoPageV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 67, 22);
			Image image2;
			VisualDiagnostics.RegisterSourceInfo(image2 = new Image(), new Uri("Pages\\EcuInfoPageV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 59, 14);
			BindingExtension bindingExtension6;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension6 = new BindingExtension(), new Uri("Pages\\EcuInfoPageV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 73, 17);
			DynamicResourceExtension dynamicResourceExtension6;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension6 = new DynamicResourceExtension(), new Uri("Pages\\EcuInfoPageV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 74, 17);
			BindingExtension bindingExtension7;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension7 = new BindingExtension(), new Uri("Pages\\EcuInfoPageV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 76, 43);
			TapGestureRecognizer tapGestureRecognizer3;
			VisualDiagnostics.RegisterSourceInfo(tapGestureRecognizer3 = new TapGestureRecognizer(), new Uri("Pages\\EcuInfoPageV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 76, 22);
			Image image3;
			VisualDiagnostics.RegisterSourceInfo(image3 = new Image(), new Uri("Pages\\EcuInfoPageV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 70, 14);
			Grid grid;
			VisualDiagnostics.RegisterSourceInfo(grid = new Grid(), new Uri("Pages\\EcuInfoPageV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 25, 10);
			RowDefinition rowDefinition;
			VisualDiagnostics.RegisterSourceInfo(rowDefinition = new RowDefinition(), new Uri("Pages\\EcuInfoPageV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 85, 18);
			RowDefinition rowDefinition2;
			VisualDiagnostics.RegisterSourceInfo(rowDefinition2 = new RowDefinition(), new Uri("Pages\\EcuInfoPageV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 86, 18);
			BindingExtension bindingExtension8;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension8 = new BindingExtension(), new Uri("Pages\\EcuInfoPageV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 91, 17);
			RowDefinition rowDefinition3;
			VisualDiagnostics.RegisterSourceInfo(rowDefinition3 = new RowDefinition(), new Uri("Pages\\EcuInfoPageV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 93, 22);
			RowDefinition rowDefinition4;
			VisualDiagnostics.RegisterSourceInfo(rowDefinition4 = new RowDefinition(), new Uri("Pages\\EcuInfoPageV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 95, 22);
			RowDefinition rowDefinition5;
			VisualDiagnostics.RegisterSourceInfo(rowDefinition5 = new RowDefinition(), new Uri("Pages\\EcuInfoPageV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 97, 22);
			RowDefinition rowDefinition6;
			VisualDiagnostics.RegisterSourceInfo(rowDefinition6 = new RowDefinition(), new Uri("Pages\\EcuInfoPageV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 99, 22);
			BindingExtension bindingExtension9;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension9 = new BindingExtension(), new Uri("Pages\\EcuInfoPageV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 110, 21);
			DataTemplate dataTemplate;
			VisualDiagnostics.RegisterSourceInfo(dataTemplate = new DataTemplate(), new Uri("Pages\\EcuInfoPageV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 112, 26);
			DynamicResourceExtension dynamicResourceExtension7;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension7 = new DynamicResourceExtension(), new Uri("Pages\\EcuInfoPageV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 130, 33);
			StaticResourceExtension staticResourceExtension;
			VisualDiagnostics.RegisterSourceInfo(staticResourceExtension = new StaticResourceExtension(), new Uri("Pages\\EcuInfoPageV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 133, 33);
			BindingExtension bindingExtension10;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension10 = new BindingExtension(), new Uri("Pages\\EcuInfoPageV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 133, 33);
			Translate translate3;
			VisualDiagnostics.RegisterSourceInfo(translate3 = new Translate(), new Uri("Pages\\EcuInfoPageV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 134, 33);
			Label label;
			VisualDiagnostics.RegisterSourceInfo(label = new Label(), new Uri("Pages\\EcuInfoPageV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 128, 30);
			BindingExtension bindingExtension11;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension11 = new BindingExtension(), new Uri("Pages\\EcuInfoPageV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 139, 33);
			BindingExtension bindingExtension12;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension12 = new BindingExtension(), new Uri("Pages\\EcuInfoPageV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 141, 33);
			DynamicResourceExtension dynamicResourceExtension8;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension8 = new DynamicResourceExtension(), new Uri("Pages\\EcuInfoPageV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 145, 37);
			DynamicResourceExtension dynamicResourceExtension9;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension9 = new DynamicResourceExtension(), new Uri("Pages\\EcuInfoPageV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 146, 37);
			Translate translate4;
			VisualDiagnostics.RegisterSourceInfo(translate4 = new Translate(), new Uri("Pages\\EcuInfoPageV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 147, 37);
			StaticResourceExtension staticResourceExtension2;
			VisualDiagnostics.RegisterSourceInfo(staticResourceExtension2 = new StaticResourceExtension(), new Uri("Pages\\EcuInfoPageV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 148, 37);
			RadioButtonWithColor radioButtonWithColor;
			VisualDiagnostics.RegisterSourceInfo(radioButtonWithColor = new RadioButtonWithColor(), new Uri("Pages\\EcuInfoPageV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 142, 34);
			DynamicResourceExtension dynamicResourceExtension10;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension10 = new DynamicResourceExtension(), new Uri("Pages\\EcuInfoPageV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 152, 37);
			DynamicResourceExtension dynamicResourceExtension11;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension11 = new DynamicResourceExtension(), new Uri("Pages\\EcuInfoPageV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 153, 37);
			Translate translate5;
			VisualDiagnostics.RegisterSourceInfo(translate5 = new Translate(), new Uri("Pages\\EcuInfoPageV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 154, 37);
			StaticResourceExtension staticResourceExtension3;
			VisualDiagnostics.RegisterSourceInfo(staticResourceExtension3 = new StaticResourceExtension(), new Uri("Pages\\EcuInfoPageV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 155, 37);
			RadioButtonWithColor radioButtonWithColor2;
			VisualDiagnostics.RegisterSourceInfo(radioButtonWithColor2 = new RadioButtonWithColor(), new Uri("Pages\\EcuInfoPageV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 149, 34);
			Grid grid2;
			VisualDiagnostics.RegisterSourceInfo(grid2 = new Grid(), new Uri("Pages\\EcuInfoPageV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 136, 30);
			StackLayout stackLayout;
			VisualDiagnostics.RegisterSourceInfo(stackLayout = new StackLayout(), new Uri("Pages\\EcuInfoPageV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 122, 26);
			ListView listView;
			VisualDiagnostics.RegisterSourceInfo(listView = new ListView(), new Uri("Pages\\EcuInfoPageV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 104, 18);
			BindingExtension bindingExtension13;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension13 = new BindingExtension(), new Uri("Pages\\EcuInfoPageV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 165, 21);
			Translate translate6;
			VisualDiagnostics.RegisterSourceInfo(translate6 = new Translate(), new Uri("Pages\\EcuInfoPageV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 167, 21);
			Button button;
			VisualDiagnostics.RegisterSourceInfo(button = new Button(), new Uri("Pages\\EcuInfoPageV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 162, 18);
			BindingExtension bindingExtension14;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension14 = new BindingExtension(), new Uri("Pages\\EcuInfoPageV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 172, 21);
			BindingExtension bindingExtension15;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension15 = new BindingExtension(), new Uri("Pages\\EcuInfoPageV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 173, 21);
			ActivityFrame activityFrame;
			VisualDiagnostics.RegisterSourceInfo(activityFrame = new ActivityFrame(), new Uri("Pages\\EcuInfoPageV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 168, 18);
			Grid grid3;
			VisualDiagnostics.RegisterSourceInfo(grid3 = new Grid(), new Uri("Pages\\EcuInfoPageV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 88, 14);
			BindingExtension bindingExtension16;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension16 = new BindingExtension(), new Uri("Pages\\EcuInfoPageV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 180, 17);
			RowDefinition rowDefinition7;
			VisualDiagnostics.RegisterSourceInfo(rowDefinition7 = new RowDefinition(), new Uri("Pages\\EcuInfoPageV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 183, 22);
			RowDefinition rowDefinition8;
			VisualDiagnostics.RegisterSourceInfo(rowDefinition8 = new RowDefinition(), new Uri("Pages\\EcuInfoPageV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 185, 22);
			RowDefinition rowDefinition9;
			VisualDiagnostics.RegisterSourceInfo(rowDefinition9 = new RowDefinition(), new Uri("Pages\\EcuInfoPageV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 187, 22);
			RowDefinition rowDefinition10;
			VisualDiagnostics.RegisterSourceInfo(rowDefinition10 = new RowDefinition(), new Uri("Pages\\EcuInfoPageV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 189, 22);
			RowDefinition rowDefinition11;
			VisualDiagnostics.RegisterSourceInfo(rowDefinition11 = new RowDefinition(), new Uri("Pages\\EcuInfoPageV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 191, 22);
			StaticResourceExtension staticResourceExtension4;
			VisualDiagnostics.RegisterSourceInfo(staticResourceExtension4 = new StaticResourceExtension(), new Uri("Pages\\EcuInfoPageV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 197, 21);
			BindingExtension bindingExtension17;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension17 = new BindingExtension(), new Uri("Pages\\EcuInfoPageV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 197, 21);
			Translate translate7;
			VisualDiagnostics.RegisterSourceInfo(translate7 = new Translate(), new Uri("Pages\\EcuInfoPageV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 199, 50);
			Label label2;
			VisualDiagnostics.RegisterSourceInfo(label2 = new Label(), new Uri("Pages\\EcuInfoPageV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 199, 22);
			BindingExtension bindingExtension18;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension18 = new BindingExtension(), new Uri("Pages\\EcuInfoPageV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 203, 25);
			Translate translate8;
			VisualDiagnostics.RegisterSourceInfo(translate8 = new Translate(), new Uri("Pages\\EcuInfoPageV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 204, 25);
			DynamicResourceExtension dynamicResourceExtension12;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension12 = new DynamicResourceExtension(), new Uri("Pages\\EcuInfoPageV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 205, 25);
			TapGestureRecognizer tapGestureRecognizer4;
			VisualDiagnostics.RegisterSourceInfo(tapGestureRecognizer4 = new TapGestureRecognizer(), new Uri("Pages\\EcuInfoPageV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 207, 30);
			Label label3;
			VisualDiagnostics.RegisterSourceInfo(label3 = new Label(), new Uri("Pages\\EcuInfoPageV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 201, 22);
			StackLayout stackLayout2;
			VisualDiagnostics.RegisterSourceInfo(stackLayout2 = new StackLayout(), new Uri("Pages\\EcuInfoPageV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 195, 18);
			BindingExtension bindingExtension19;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension19 = new BindingExtension(), new Uri("Pages\\EcuInfoPageV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 213, 47);
			Label label4;
			VisualDiagnostics.RegisterSourceInfo(label4 = new Label(), new Uri("Pages\\EcuInfoPageV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 213, 22);
			ScrollView scrollView;
			VisualDiagnostics.RegisterSourceInfo(scrollView = new ScrollView(), new Uri("Pages\\EcuInfoPageV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 212, 18);
			BindingExtension bindingExtension20;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension20 = new BindingExtension(), new Uri("Pages\\EcuInfoPageV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 221, 21);
			BindingExtension bindingExtension21;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension21 = new BindingExtension(), new Uri("Pages\\EcuInfoPageV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 222, 21);
			ActivityFrame activityFrame2;
			VisualDiagnostics.RegisterSourceInfo(activityFrame2 = new ActivityFrame(), new Uri("Pages\\EcuInfoPageV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 217, 18);
			BindingExtension bindingExtension22;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension22 = new BindingExtension(), new Uri("Pages\\EcuInfoPageV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 229, 21);
			BindingExtension bindingExtension23;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension23 = new BindingExtension(), new Uri("Pages\\EcuInfoPageV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 230, 21);
			BindingExtension bindingExtension24;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension24 = new BindingExtension(), new Uri("Pages\\EcuInfoPageV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 231, 21);
			Translate translate9;
			VisualDiagnostics.RegisterSourceInfo(translate9 = new Translate(), new Uri("Pages\\EcuInfoPageV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 232, 21);
			Button button2;
			VisualDiagnostics.RegisterSourceInfo(button2 = new Button(), new Uri("Pages\\EcuInfoPageV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 225, 18);
			StaticResourceExtension staticResourceExtension5;
			VisualDiagnostics.RegisterSourceInfo(staticResourceExtension5 = new StaticResourceExtension(), new Uri("Pages\\EcuInfoPageV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 234, 36);
			BindingExtension bindingExtension25;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension25 = new BindingExtension(), new Uri("Pages\\EcuInfoPageV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 234, 36);
			BindingExtension bindingExtension26;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension26 = new BindingExtension(), new Uri("Pages\\EcuInfoPageV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 236, 25);
			BindingExtension bindingExtension27;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension27 = new BindingExtension(), new Uri("Pages\\EcuInfoPageV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 237, 25);
			Translate translate10;
			VisualDiagnostics.RegisterSourceInfo(translate10 = new Translate(), new Uri("Pages\\EcuInfoPageV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 238, 25);
			Button button3;
			VisualDiagnostics.RegisterSourceInfo(button3 = new Button(), new Uri("Pages\\EcuInfoPageV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 235, 22);
			Grid grid4;
			VisualDiagnostics.RegisterSourceInfo(grid4 = new Grid(), new Uri("Pages\\EcuInfoPageV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 234, 18);
			BindingExtension bindingExtension28;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension28 = new BindingExtension(), new Uri("Pages\\EcuInfoPageV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 244, 21);
			DynamicResourceExtension dynamicResourceExtension13;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension13 = new DynamicResourceExtension(), new Uri("Pages\\EcuInfoPageV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 248, 25);
			BindingExtension bindingExtension29;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension29 = new BindingExtension(), new Uri("Pages\\EcuInfoPageV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 249, 25);
			Translate translate11;
			VisualDiagnostics.RegisterSourceInfo(translate11 = new Translate(), new Uri("Pages\\EcuInfoPageV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 250, 25);
			DynamicResourceExtension dynamicResourceExtension14;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension14 = new DynamicResourceExtension(), new Uri("Pages\\EcuInfoPageV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 251, 25);
			TapGestureRecognizer tapGestureRecognizer5;
			VisualDiagnostics.RegisterSourceInfo(tapGestureRecognizer5 = new TapGestureRecognizer(), new Uri("Pages\\EcuInfoPageV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 253, 30);
			Label label5;
			VisualDiagnostics.RegisterSourceInfo(label5 = new Label(), new Uri("Pages\\EcuInfoPageV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 246, 22);
			StackLayout stackLayout3;
			VisualDiagnostics.RegisterSourceInfo(stackLayout3 = new StackLayout(), new Uri("Pages\\EcuInfoPageV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 242, 18);
			Grid grid5;
			VisualDiagnostics.RegisterSourceInfo(grid5 = new Grid(), new Uri("Pages\\EcuInfoPageV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 177, 14);
			ComplexAdView complexAdView;
			VisualDiagnostics.RegisterSourceInfo(complexAdView = new ComplexAdView(), new Uri("Pages\\EcuInfoPageV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 260, 14);
			Grid grid6;
			VisualDiagnostics.RegisterSourceInfo(grid6 = new Grid(), new Uri("Pages\\EcuInfoPageV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 83, 10);
			VisualDiagnostics.RegisterSourceInfo(this, new Uri("Pages\\EcuInfoPageV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 2, 2);
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
			nameScope.RegisterName("layoutRoot", grid6);
			if (grid6.StyleId == null)
			{
				grid6.StyleId = "layoutRoot";
			}
			nameScope.RegisterName("gridSetup", grid3);
			if (grid3.StyleId == null)
			{
				grid3.StyleId = "gridSetup";
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
			nameScope.RegisterName("btnReadDTC", button);
			if (button.StyleId == null)
			{
				button.StyleId = "btnReadDTC";
			}
			nameScope.RegisterName("activityFrame2", activityFrame);
			if (activityFrame.StyleId == null)
			{
				activityFrame.StyleId = "activityFrame2";
			}
			nameScope.RegisterName("gridResults", grid5);
			if (grid5.StyleId == null)
			{
				grid5.StyleId = "gridResults";
			}
			nameScope.RegisterName("lbResults", label4);
			if (label4.StyleId == null)
			{
				label4.StyleId = "lbResults";
			}
			nameScope.RegisterName("activityFrame", activityFrame2);
			if (activityFrame2.StyleId == null)
			{
				activityFrame2.StyleId = "activityFrame";
			}
			nameScope.RegisterName("btnCancel", button2);
			if (button2.StyleId == null)
			{
				button2.StyleId = "btnCancel";
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
			this.layoutRoot = grid6;
			this.gridSetup = grid3;
			this.lvECUs = listView;
			this.gridAllOrSupportedSelector = grid2;
			this.rbDetected = radioButtonWithColor;
			this.rbAll = radioButtonWithColor2;
			this.btnReadDTC = button;
			this.activityFrame2 = activityFrame;
			this.gridResults = grid5;
			this.lbResults = label4;
			this.activityFrame = activityFrame2;
			this.btnCancel = button2;
			this.ad = complexAdView;
			this.Resources = resourceDictionary;
			resourceDictionary.Add("DTCStatusCollectionToStringConverter", dtcstatusCollectionToStringConverter);
			resourceDictionary.Add("DTCDescriptionCollectionToStringConverter", dtcdescriptionCollectionToStringConverter);
			resourceDictionary.Add("DTCCodeToStringConverter", dtccodeToStringConverter);
			resourceDictionary.Add("EmptyStringToFalseConverter", emptyStringToFalseConverter);
			resourceDictionary.Add("BoolToNegativeConverter", boolToNegativeConverter);
			this.SetValue(Page.TitleProperty, "ECU Info");
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
			xmlNamespaceResolver.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider.Add(typeFromHandle2, new XamlTypeResolver(xmlNamespaceResolver, typeof(EcuInfoPageV2).GetTypeInfo().Assembly));
			xamlServiceProvider.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(12, 5)));
			DynamicResource dynamicResource = markupExtension.ProvideValue(xamlServiceProvider);
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
			xmlNamespaceResolver2.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver2.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver2.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider2.Add(typeFromHandle4, new XamlTypeResolver(xmlNamespaceResolver2, typeof(EcuInfoPageV2).GetTypeInfo().Assembly));
			xamlServiceProvider2.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(38, 17)));
			DynamicResource dynamicResource2 = markupExtension2.ProvideValue(xamlServiceProvider2);
			linkButton.SetDynamicResource(VisualElement.StyleProperty, dynamicResource2.Key);
			translate.Text = "ios_Back";
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
			xmlNamespaceResolver3.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver3.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver3.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider3.Add(typeFromHandle6, new XamlTypeResolver(xmlNamespaceResolver3, typeof(EcuInfoPageV2).GetTypeInfo().Assembly));
			xamlServiceProvider3.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(39, 17)));
			object obj4 = markupExtension3.ProvideValue(xamlServiceProvider3);
			linkButton.Text = obj4;
			grid.Children.Add(linkButton);
			nonScalableLabel.SetValue(Grid.ColumnProperty, 1);
			nonScalableLabel.SetValue(View.HorizontalOptionsProperty, LayoutOptions.Center);
			nonScalableLabel.SetValue(Label.HorizontalTextAlignmentProperty, new TextAlignmentConverter().ConvertFromInvariantString("Center"));
			nonScalableLabel.SetValue(Label.LineBreakModeProperty, 4);
			dynamicResourceExtension3.Key = "NavigationBarNonScalableLabel";
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
			xmlNamespaceResolver4.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver4.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver4.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider4.Add(typeFromHandle8, new XamlTypeResolver(xmlNamespaceResolver4, typeof(EcuInfoPageV2).GetTypeInfo().Assembly));
			xamlServiceProvider4.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(45, 17)));
			DynamicResource dynamicResource3 = markupExtension4.ProvideValue(xamlServiceProvider4);
			nonScalableLabel.SetDynamicResource(VisualElement.StyleProperty, dynamicResource3.Key);
			translate2.Text = "ecuIdentsTitle";
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
			xmlNamespaceResolver5.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver5.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver5.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider5.Add(typeFromHandle10, new XamlTypeResolver(xmlNamespaceResolver5, typeof(EcuInfoPageV2).GetTypeInfo().Assembly));
			xamlServiceProvider5.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(46, 17)));
			object obj7 = markupExtension5.ProvideValue(xamlServiceProvider5);
			nonScalableLabel.Text = obj7;
			grid.Children.Add(nonScalableLabel);
			image.SetValue(Grid.ColumnProperty, 2);
			image.SetValue(View.MarginProperty, new Thickness(0.0));
			image.SetValue(VisualElement.BackgroundColorProperty, Color.Transparent);
			bindingExtension2.Mode = 2;
			bindingExtension2.Path = "IsSetup";
			BindingBase bindingBase2 = bindingExtension2.ProvideValue(null);
			image.SetBinding(VisualElement.IsVisibleProperty, bindingBase2);
			dynamicResourceExtension4.Key = "NB_check";
			IMarkupExtension<DynamicResource> markupExtension6 = dynamicResourceExtension4;
			XamlServiceProvider xamlServiceProvider6 = new XamlServiceProvider();
			Type typeFromHandle11 = typeof(IProvideValueTarget);
			object[] array6 = new object[0 + 3];
			array6[0] = image;
			array6[1] = grid;
			array6[2] = this;
			object obj8;
			xamlServiceProvider6.Add(typeFromHandle11, obj8 = new SimpleValueTargetProvider(array6, Image.SourceProperty, nameScope));
			xamlServiceProvider6.Add(typeof(IReferenceProvider), obj8);
			Type typeFromHandle12 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver6 = new XmlNamespaceResolver();
			xmlNamespaceResolver6.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver6.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver6.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver6.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver6.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider6.Add(typeFromHandle12, new XamlTypeResolver(xmlNamespaceResolver6, typeof(EcuInfoPageV2).GetTypeInfo().Assembly));
			xamlServiceProvider6.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(54, 17)));
			DynamicResource dynamicResource4 = markupExtension6.ProvideValue(xamlServiceProvider6);
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
			IMarkupExtension<DynamicResource> markupExtension7 = dynamicResourceExtension5;
			XamlServiceProvider xamlServiceProvider7 = new XamlServiceProvider();
			Type typeFromHandle13 = typeof(IProvideValueTarget);
			object[] array7 = new object[0 + 3];
			array7[0] = image2;
			array7[1] = grid;
			array7[2] = this;
			object obj9;
			xamlServiceProvider7.Add(typeFromHandle13, obj9 = new SimpleValueTargetProvider(array7, Image.SourceProperty, nameScope));
			xamlServiceProvider7.Add(typeof(IReferenceProvider), obj9);
			Type typeFromHandle14 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver7 = new XmlNamespaceResolver();
			xmlNamespaceResolver7.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver7.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver7.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver7.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver7.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider7.Add(typeFromHandle14, new XamlTypeResolver(xmlNamespaceResolver7, typeof(EcuInfoPageV2).GetTypeInfo().Assembly));
			xamlServiceProvider7.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(65, 17)));
			DynamicResource dynamicResource5 = markupExtension7.ProvideValue(xamlServiceProvider7);
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
			IMarkupExtension<DynamicResource> markupExtension8 = dynamicResourceExtension6;
			XamlServiceProvider xamlServiceProvider8 = new XamlServiceProvider();
			Type typeFromHandle15 = typeof(IProvideValueTarget);
			object[] array8 = new object[0 + 3];
			array8[0] = image3;
			array8[1] = grid;
			array8[2] = this;
			object obj10;
			xamlServiceProvider8.Add(typeFromHandle15, obj10 = new SimpleValueTargetProvider(array8, Image.SourceProperty, nameScope));
			xamlServiceProvider8.Add(typeof(IReferenceProvider), obj10);
			Type typeFromHandle16 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver8 = new XmlNamespaceResolver();
			xmlNamespaceResolver8.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver8.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver8.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver8.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver8.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider8.Add(typeFromHandle16, new XamlTypeResolver(xmlNamespaceResolver8, typeof(EcuInfoPageV2).GetTypeInfo().Assembly));
			xamlServiceProvider8.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(74, 17)));
			DynamicResource dynamicResource6 = markupExtension8.ProvideValue(xamlServiceProvider8);
			image3.SetDynamicResource(Image.SourceProperty, dynamicResource6.Key);
			bindingExtension7.Path = "FilterSwitchCommand";
			BindingBase bindingBase7 = bindingExtension7.ProvideValue(null);
			tapGestureRecognizer3.SetBinding(TapGestureRecognizer.CommandProperty, bindingBase7);
			tapGestureRecognizer3.SetValue(TapGestureRecognizer.NumberOfTapsRequiredProperty, 1);
			image3.GestureRecognizers.Add(tapGestureRecognizer3);
			grid.Children.Add(image3);
			this.SetValue(NavigationPage.TitleViewProperty, grid);
			grid6.SetValue(View.MarginProperty, new Thickness(5.0, 0.0));
			rowDefinition.SetValue(RowDefinition.HeightProperty, new GridLengthTypeConverter().ConvertFromInvariantString("*"));
			grid6.GetValue(Grid.RowDefinitionsProperty).Add(rowDefinition);
			rowDefinition2.SetValue(RowDefinition.HeightProperty, new GridLengthTypeConverter().ConvertFromInvariantString("auto"));
			grid6.GetValue(Grid.RowDefinitionsProperty).Add(rowDefinition2);
			grid3.SetValue(Grid.RowProperty, 0);
			bindingExtension8.Mode = 2;
			bindingExtension8.Path = "IsSetup";
			BindingBase bindingBase8 = bindingExtension8.ProvideValue(null);
			grid3.SetBinding(VisualElement.IsVisibleProperty, bindingBase8);
			rowDefinition3.SetValue(RowDefinition.HeightProperty, new GridLengthTypeConverter().ConvertFromInvariantString("auto"));
			grid3.GetValue(Grid.RowDefinitionsProperty).Add(rowDefinition3);
			rowDefinition4.SetValue(RowDefinition.HeightProperty, new GridLengthTypeConverter().ConvertFromInvariantString("*"));
			grid3.GetValue(Grid.RowDefinitionsProperty).Add(rowDefinition4);
			rowDefinition5.SetValue(RowDefinition.HeightProperty, new GridLengthTypeConverter().ConvertFromInvariantString("auto"));
			grid3.GetValue(Grid.RowDefinitionsProperty).Add(rowDefinition5);
			rowDefinition6.SetValue(RowDefinition.HeightProperty, new GridLengthTypeConverter().ConvertFromInvariantString("auto"));
			grid3.GetValue(Grid.RowDefinitionsProperty).Add(rowDefinition6);
			listView.SetValue(Grid.RowProperty, 1);
			listView.SetValue(ListView.HasUnevenRowsProperty, true);
			listView.SetValue(ListView.IsGroupingEnabledProperty, false);
			listView.ItemSelected += this.LvECUs_ItemSelected;
			bindingExtension9.Path = "ECUListToDisplay";
			BindingBase bindingBase9 = bindingExtension9.ProvideValue(null);
			listView.SetBinding(ItemsView<Cell>.ItemsSourceProperty, bindingBase9);
			IDataTemplate dataTemplate2 = dataTemplate;
			EcuInfoPageV2.<InitializeComponent>_anonXamlCDataTemplate_54 <InitializeComponent>_anonXamlCDataTemplate_ = new EcuInfoPageV2.<InitializeComponent>_anonXamlCDataTemplate_54();
			object[] array9 = new object[0 + 5];
			array9[0] = dataTemplate;
			array9[1] = listView;
			array9[2] = grid3;
			array9[3] = grid6;
			array9[4] = this;
			<InitializeComponent>_anonXamlCDataTemplate_.parentValues = array9;
			<InitializeComponent>_anonXamlCDataTemplate_.root = this;
			dataTemplate2.LoadTemplate = new Func<object>(<InitializeComponent>_anonXamlCDataTemplate_.LoadDataTemplate);
			listView.SetValue(ItemsView<Cell>.ItemTemplateProperty, dataTemplate);
			stackLayout.SetValue(StackLayout.OrientationProperty, 0);
			label.SetValue(Label.FontAttributesProperty, new FontAttributesConverter().ConvertFromInvariantString("Bold"));
			dynamicResourceExtension7.Key = "BaseFontSize";
			IMarkupExtension<DynamicResource> markupExtension9 = dynamicResourceExtension7;
			XamlServiceProvider xamlServiceProvider9 = new XamlServiceProvider();
			Type typeFromHandle17 = typeof(IProvideValueTarget);
			object[] array10 = new object[0 + 6];
			array10[0] = label;
			array10[1] = stackLayout;
			array10[2] = listView;
			array10[3] = grid3;
			array10[4] = grid6;
			array10[5] = this;
			object obj11;
			xamlServiceProvider9.Add(typeFromHandle17, obj11 = new SimpleValueTargetProvider(array10, Label.FontSizeProperty, nameScope));
			xamlServiceProvider9.Add(typeof(IReferenceProvider), obj11);
			Type typeFromHandle18 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver9 = new XmlNamespaceResolver();
			xmlNamespaceResolver9.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver9.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver9.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver9.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver9.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider9.Add(typeFromHandle18, new XamlTypeResolver(xmlNamespaceResolver9, typeof(EcuInfoPageV2).GetTypeInfo().Assembly));
			xamlServiceProvider9.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(130, 33)));
			DynamicResource dynamicResource7 = markupExtension9.ProvideValue(xamlServiceProvider9);
			label.SetDynamicResource(Label.FontSizeProperty, dynamicResource7.Key);
			label.SetValue(View.HorizontalOptionsProperty, LayoutOptions.Center);
			label.SetValue(Label.HorizontalTextAlignmentProperty, new TextAlignmentConverter().ConvertFromInvariantString("Center"));
			bindingExtension10.Mode = 2;
			staticResourceExtension.Key = "BoolToNegativeConverter";
			IMarkupExtension markupExtension10 = staticResourceExtension;
			XamlServiceProvider xamlServiceProvider10 = new XamlServiceProvider();
			Type typeFromHandle19 = typeof(IProvideValueTarget);
			object[] array11 = new object[0 + 7];
			array11[0] = bindingExtension10;
			array11[1] = label;
			array11[2] = stackLayout;
			array11[3] = listView;
			array11[4] = grid3;
			array11[5] = grid6;
			array11[6] = this;
			object obj12;
			xamlServiceProvider10.Add(typeFromHandle19, obj12 = new SimpleValueTargetProvider(array11, typeof(BindingExtension).GetRuntimeProperty("Converter"), nameScope));
			xamlServiceProvider10.Add(typeof(IReferenceProvider), obj12);
			Type typeFromHandle20 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver10 = new XmlNamespaceResolver();
			xmlNamespaceResolver10.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver10.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver10.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver10.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver10.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider10.Add(typeFromHandle20, new XamlTypeResolver(xmlNamespaceResolver10, typeof(EcuInfoPageV2).GetTypeInfo().Assembly));
			xamlServiceProvider10.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(133, 33)));
			object obj13 = markupExtension10.ProvideValue(xamlServiceProvider10);
			bindingExtension10.Converter = obj13;
			bindingExtension10.Path = "DisplayAllOrDetectedSelector";
			BindingBase bindingBase10 = bindingExtension10.ProvideValue(null);
			label.SetBinding(VisualElement.IsVisibleProperty, bindingBase10);
			translate3.Text = "dtc_ECUListHint";
			IMarkupExtension markupExtension11 = translate3;
			XamlServiceProvider xamlServiceProvider11 = new XamlServiceProvider();
			Type typeFromHandle21 = typeof(IProvideValueTarget);
			object[] array12 = new object[0 + 6];
			array12[0] = label;
			array12[1] = stackLayout;
			array12[2] = listView;
			array12[3] = grid3;
			array12[4] = grid6;
			array12[5] = this;
			object obj14;
			xamlServiceProvider11.Add(typeFromHandle21, obj14 = new SimpleValueTargetProvider(array12, Label.TextProperty, nameScope));
			xamlServiceProvider11.Add(typeof(IReferenceProvider), obj14);
			Type typeFromHandle22 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver11 = new XmlNamespaceResolver();
			xmlNamespaceResolver11.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver11.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver11.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver11.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver11.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider11.Add(typeFromHandle22, new XamlTypeResolver(xmlNamespaceResolver11, typeof(EcuInfoPageV2).GetTypeInfo().Assembly));
			xamlServiceProvider11.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(134, 33)));
			object obj15 = markupExtension11.ProvideValue(xamlServiceProvider11);
			label.Text = obj15;
			stackLayout.Children.Add(label);
			grid2.SetValue(Grid.ColumnDefinitionsProperty, new ColumnDefinitionCollectionTypeConverter().ConvertFromInvariantString("*,*"));
			bindingExtension11.Mode = 2;
			bindingExtension11.Path = "DisplayAllOrDetectedSelector";
			BindingBase bindingBase11 = bindingExtension11.ProvideValue(null);
			grid2.SetBinding(VisualElement.IsVisibleProperty, bindingBase11);
			grid2.SetValue(RadioButtonGroup.GroupNameProperty, "AllOrSupportedSelector");
			bindingExtension12.Mode = 1;
			bindingExtension12.Path = "DisplayDetected";
			BindingBase bindingBase12 = bindingExtension12.ProvideValue(null);
			grid2.SetBinding(RadioButtonGroup.SelectedValueProperty, bindingBase12);
			radioButtonWithColor.SetValue(Grid.ColumnProperty, 0);
			dynamicResourceExtension8.Key = "BackgroundColor";
			IMarkupExtension<DynamicResource> markupExtension12 = dynamicResourceExtension8;
			XamlServiceProvider xamlServiceProvider12 = new XamlServiceProvider();
			Type typeFromHandle23 = typeof(IProvideValueTarget);
			object[] array13 = new object[0 + 7];
			array13[0] = radioButtonWithColor;
			array13[1] = grid2;
			array13[2] = stackLayout;
			array13[3] = listView;
			array13[4] = grid3;
			array13[5] = grid6;
			array13[6] = this;
			object obj16;
			xamlServiceProvider12.Add(typeFromHandle23, obj16 = new SimpleValueTargetProvider(array13, VisualElement.BackgroundColorProperty, nameScope));
			xamlServiceProvider12.Add(typeof(IReferenceProvider), obj16);
			Type typeFromHandle24 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver12 = new XmlNamespaceResolver();
			xmlNamespaceResolver12.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver12.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver12.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver12.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver12.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider12.Add(typeFromHandle24, new XamlTypeResolver(xmlNamespaceResolver12, typeof(EcuInfoPageV2).GetTypeInfo().Assembly));
			xamlServiceProvider12.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(145, 37)));
			DynamicResource dynamicResource8 = markupExtension12.ProvideValue(xamlServiceProvider12);
			radioButtonWithColor.SetDynamicResource(VisualElement.BackgroundColorProperty, dynamicResource8.Key);
			dynamicResourceExtension9.Key = "ButtonAccentColor";
			IMarkupExtension<DynamicResource> markupExtension13 = dynamicResourceExtension9;
			XamlServiceProvider xamlServiceProvider13 = new XamlServiceProvider();
			Type typeFromHandle25 = typeof(IProvideValueTarget);
			object[] array14 = new object[0 + 7];
			array14[0] = radioButtonWithColor;
			array14[1] = grid2;
			array14[2] = stackLayout;
			array14[3] = listView;
			array14[4] = grid3;
			array14[5] = grid6;
			array14[6] = this;
			object obj17;
			xamlServiceProvider13.Add(typeFromHandle25, obj17 = new SimpleValueTargetProvider(array14, RadioButton.BorderColorProperty, nameScope));
			xamlServiceProvider13.Add(typeof(IReferenceProvider), obj17);
			Type typeFromHandle26 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver13 = new XmlNamespaceResolver();
			xmlNamespaceResolver13.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver13.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver13.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver13.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver13.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider13.Add(typeFromHandle26, new XamlTypeResolver(xmlNamespaceResolver13, typeof(EcuInfoPageV2).GetTypeInfo().Assembly));
			xamlServiceProvider13.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(146, 37)));
			DynamicResource dynamicResource9 = markupExtension13.ProvideValue(xamlServiceProvider13);
			radioButtonWithColor.SetDynamicResource(RadioButton.BorderColorProperty, dynamicResource9.Key);
			translate4.Text = "dtc_FoundUnits";
			IMarkupExtension markupExtension14 = translate4;
			XamlServiceProvider xamlServiceProvider14 = new XamlServiceProvider();
			Type typeFromHandle27 = typeof(IProvideValueTarget);
			object[] array15 = new object[0 + 7];
			array15[0] = radioButtonWithColor;
			array15[1] = grid2;
			array15[2] = stackLayout;
			array15[3] = listView;
			array15[4] = grid3;
			array15[5] = grid6;
			array15[6] = this;
			object obj18;
			xamlServiceProvider14.Add(typeFromHandle27, obj18 = new SimpleValueTargetProvider(array15, RadioButton.ContentProperty, nameScope));
			xamlServiceProvider14.Add(typeof(IReferenceProvider), obj18);
			Type typeFromHandle28 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver14 = new XmlNamespaceResolver();
			xmlNamespaceResolver14.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver14.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver14.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver14.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver14.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider14.Add(typeFromHandle28, new XamlTypeResolver(xmlNamespaceResolver14, typeof(EcuInfoPageV2).GetTypeInfo().Assembly));
			xamlServiceProvider14.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(147, 37)));
			object obj19 = markupExtension14.ProvideValue(xamlServiceProvider14);
			radioButtonWithColor.SetValue(RadioButton.ContentProperty, obj19);
			staticResourceExtension2.Key = "TrueValue";
			IMarkupExtension markupExtension15 = staticResourceExtension2;
			XamlServiceProvider xamlServiceProvider15 = new XamlServiceProvider();
			Type typeFromHandle29 = typeof(IProvideValueTarget);
			object[] array16 = new object[0 + 7];
			array16[0] = radioButtonWithColor;
			array16[1] = grid2;
			array16[2] = stackLayout;
			array16[3] = listView;
			array16[4] = grid3;
			array16[5] = grid6;
			array16[6] = this;
			object obj20;
			xamlServiceProvider15.Add(typeFromHandle29, obj20 = new SimpleValueTargetProvider(array16, RadioButton.ValueProperty, nameScope));
			xamlServiceProvider15.Add(typeof(IReferenceProvider), obj20);
			Type typeFromHandle30 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver15 = new XmlNamespaceResolver();
			xmlNamespaceResolver15.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver15.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver15.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver15.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver15.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider15.Add(typeFromHandle30, new XamlTypeResolver(xmlNamespaceResolver15, typeof(EcuInfoPageV2).GetTypeInfo().Assembly));
			xamlServiceProvider15.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(148, 37)));
			object obj21 = markupExtension15.ProvideValue(xamlServiceProvider15);
			radioButtonWithColor.SetValue(RadioButton.ValueProperty, obj21);
			grid2.Children.Add(radioButtonWithColor);
			radioButtonWithColor2.SetValue(Grid.ColumnProperty, 1);
			dynamicResourceExtension10.Key = "BackgroundColor";
			IMarkupExtension<DynamicResource> markupExtension16 = dynamicResourceExtension10;
			XamlServiceProvider xamlServiceProvider16 = new XamlServiceProvider();
			Type typeFromHandle31 = typeof(IProvideValueTarget);
			object[] array17 = new object[0 + 7];
			array17[0] = radioButtonWithColor2;
			array17[1] = grid2;
			array17[2] = stackLayout;
			array17[3] = listView;
			array17[4] = grid3;
			array17[5] = grid6;
			array17[6] = this;
			object obj22;
			xamlServiceProvider16.Add(typeFromHandle31, obj22 = new SimpleValueTargetProvider(array17, VisualElement.BackgroundColorProperty, nameScope));
			xamlServiceProvider16.Add(typeof(IReferenceProvider), obj22);
			Type typeFromHandle32 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver16 = new XmlNamespaceResolver();
			xmlNamespaceResolver16.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver16.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver16.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver16.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver16.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider16.Add(typeFromHandle32, new XamlTypeResolver(xmlNamespaceResolver16, typeof(EcuInfoPageV2).GetTypeInfo().Assembly));
			xamlServiceProvider16.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(152, 37)));
			DynamicResource dynamicResource10 = markupExtension16.ProvideValue(xamlServiceProvider16);
			radioButtonWithColor2.SetDynamicResource(VisualElement.BackgroundColorProperty, dynamicResource10.Key);
			dynamicResourceExtension11.Key = "ButtonAccentColor";
			IMarkupExtension<DynamicResource> markupExtension17 = dynamicResourceExtension11;
			XamlServiceProvider xamlServiceProvider17 = new XamlServiceProvider();
			Type typeFromHandle33 = typeof(IProvideValueTarget);
			object[] array18 = new object[0 + 7];
			array18[0] = radioButtonWithColor2;
			array18[1] = grid2;
			array18[2] = stackLayout;
			array18[3] = listView;
			array18[4] = grid3;
			array18[5] = grid6;
			array18[6] = this;
			object obj23;
			xamlServiceProvider17.Add(typeFromHandle33, obj23 = new SimpleValueTargetProvider(array18, RadioButton.BorderColorProperty, nameScope));
			xamlServiceProvider17.Add(typeof(IReferenceProvider), obj23);
			Type typeFromHandle34 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver17 = new XmlNamespaceResolver();
			xmlNamespaceResolver17.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver17.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver17.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver17.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver17.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider17.Add(typeFromHandle34, new XamlTypeResolver(xmlNamespaceResolver17, typeof(EcuInfoPageV2).GetTypeInfo().Assembly));
			xamlServiceProvider17.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(153, 37)));
			DynamicResource dynamicResource11 = markupExtension17.ProvideValue(xamlServiceProvider17);
			radioButtonWithColor2.SetDynamicResource(RadioButton.BorderColorProperty, dynamicResource11.Key);
			translate5.Text = "dtc_AllUnits";
			IMarkupExtension markupExtension18 = translate5;
			XamlServiceProvider xamlServiceProvider18 = new XamlServiceProvider();
			Type typeFromHandle35 = typeof(IProvideValueTarget);
			object[] array19 = new object[0 + 7];
			array19[0] = radioButtonWithColor2;
			array19[1] = grid2;
			array19[2] = stackLayout;
			array19[3] = listView;
			array19[4] = grid3;
			array19[5] = grid6;
			array19[6] = this;
			object obj24;
			xamlServiceProvider18.Add(typeFromHandle35, obj24 = new SimpleValueTargetProvider(array19, RadioButton.ContentProperty, nameScope));
			xamlServiceProvider18.Add(typeof(IReferenceProvider), obj24);
			Type typeFromHandle36 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver18 = new XmlNamespaceResolver();
			xmlNamespaceResolver18.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver18.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver18.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver18.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver18.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider18.Add(typeFromHandle36, new XamlTypeResolver(xmlNamespaceResolver18, typeof(EcuInfoPageV2).GetTypeInfo().Assembly));
			xamlServiceProvider18.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(154, 37)));
			object obj25 = markupExtension18.ProvideValue(xamlServiceProvider18);
			radioButtonWithColor2.SetValue(RadioButton.ContentProperty, obj25);
			staticResourceExtension3.Key = "FalseValue";
			IMarkupExtension markupExtension19 = staticResourceExtension3;
			XamlServiceProvider xamlServiceProvider19 = new XamlServiceProvider();
			Type typeFromHandle37 = typeof(IProvideValueTarget);
			object[] array20 = new object[0 + 7];
			array20[0] = radioButtonWithColor2;
			array20[1] = grid2;
			array20[2] = stackLayout;
			array20[3] = listView;
			array20[4] = grid3;
			array20[5] = grid6;
			array20[6] = this;
			object obj26;
			xamlServiceProvider19.Add(typeFromHandle37, obj26 = new SimpleValueTargetProvider(array20, RadioButton.ValueProperty, nameScope));
			xamlServiceProvider19.Add(typeof(IReferenceProvider), obj26);
			Type typeFromHandle38 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver19 = new XmlNamespaceResolver();
			xmlNamespaceResolver19.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver19.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver19.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver19.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver19.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider19.Add(typeFromHandle38, new XamlTypeResolver(xmlNamespaceResolver19, typeof(EcuInfoPageV2).GetTypeInfo().Assembly));
			xamlServiceProvider19.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(155, 37)));
			object obj27 = markupExtension19.ProvideValue(xamlServiceProvider19);
			radioButtonWithColor2.SetValue(RadioButton.ValueProperty, obj27);
			grid2.Children.Add(radioButtonWithColor2);
			stackLayout.Children.Add(grid2);
			listView.SetValue(ListView.HeaderProperty, stackLayout);
			grid3.Children.Add(listView);
			button.SetValue(Grid.RowProperty, 2);
			bindingExtension13.Path = "ReadECUInfoCommand";
			BindingBase bindingBase13 = bindingExtension13.ProvideValue(null);
			button.SetBinding(Button.CommandProperty, bindingBase13);
			button.SetValue(VisualElement.IsEnabledProperty, false);
			translate6.Text = "DtcPage_btnRead.Content";
			IMarkupExtension markupExtension20 = translate6;
			XamlServiceProvider xamlServiceProvider20 = new XamlServiceProvider();
			Type typeFromHandle39 = typeof(IProvideValueTarget);
			object[] array21 = new object[0 + 4];
			array21[0] = button;
			array21[1] = grid3;
			array21[2] = grid6;
			array21[3] = this;
			object obj28;
			xamlServiceProvider20.Add(typeFromHandle39, obj28 = new SimpleValueTargetProvider(array21, Button.TextProperty, nameScope));
			xamlServiceProvider20.Add(typeof(IReferenceProvider), obj28);
			Type typeFromHandle40 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver20 = new XmlNamespaceResolver();
			xmlNamespaceResolver20.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver20.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver20.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver20.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver20.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider20.Add(typeFromHandle40, new XamlTypeResolver(xmlNamespaceResolver20, typeof(EcuInfoPageV2).GetTypeInfo().Assembly));
			xamlServiceProvider20.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(167, 21)));
			object obj29 = markupExtension20.ProvideValue(xamlServiceProvider20);
			button.Text = obj29;
			grid3.Children.Add(button);
			activityFrame.SetValue(Grid.RowProperty, 1);
			activityFrame.SetValue(View.MarginProperty, new Thickness(0.0));
			bindingExtension14.Path = "IsBusy";
			BindingBase bindingBase14 = bindingExtension14.ProvideValue(null);
			activityFrame.SetBinding(VisualElement.IsVisibleProperty, bindingBase14);
			bindingExtension15.Path = "StatusText";
			BindingBase bindingBase15 = bindingExtension15.ProvideValue(null);
			activityFrame.SetBinding(ActivityFrame.TextProperty, bindingBase15);
			activityFrame.SetValue(View.VerticalOptionsProperty, LayoutOptions.Center);
			grid3.Children.Add(activityFrame);
			grid6.Children.Add(grid3);
			grid5.SetValue(Grid.RowProperty, 0);
			bindingExtension16.Mode = 2;
			bindingExtension16.Path = "IsResults";
			BindingBase bindingBase16 = bindingExtension16.ProvideValue(null);
			grid5.SetBinding(VisualElement.IsVisibleProperty, bindingBase16);
			rowDefinition7.SetValue(RowDefinition.HeightProperty, new GridLengthTypeConverter().ConvertFromInvariantString("auto"));
			grid5.GetValue(Grid.RowDefinitionsProperty).Add(rowDefinition7);
			rowDefinition8.SetValue(RowDefinition.HeightProperty, new GridLengthTypeConverter().ConvertFromInvariantString("*"));
			grid5.GetValue(Grid.RowDefinitionsProperty).Add(rowDefinition8);
			rowDefinition9.SetValue(RowDefinition.HeightProperty, new GridLengthTypeConverter().ConvertFromInvariantString("auto"));
			grid5.GetValue(Grid.RowDefinitionsProperty).Add(rowDefinition9);
			rowDefinition10.SetValue(RowDefinition.HeightProperty, new GridLengthTypeConverter().ConvertFromInvariantString("auto"));
			grid5.GetValue(Grid.RowDefinitionsProperty).Add(rowDefinition10);
			rowDefinition11.SetValue(RowDefinition.HeightProperty, new GridLengthTypeConverter().ConvertFromInvariantString("auto"));
			grid5.GetValue(Grid.RowDefinitionsProperty).Add(rowDefinition11);
			stackLayout2.SetValue(Grid.RowProperty, 1);
			bindingExtension17.Mode = 2;
			staticResourceExtension4.Key = "BoolToNegativeConverter";
			IMarkupExtension markupExtension21 = staticResourceExtension4;
			XamlServiceProvider xamlServiceProvider21 = new XamlServiceProvider();
			Type typeFromHandle41 = typeof(IProvideValueTarget);
			object[] array22 = new object[0 + 5];
			array22[0] = bindingExtension17;
			array22[1] = stackLayout2;
			array22[2] = grid5;
			array22[3] = grid6;
			array22[4] = this;
			object obj30;
			xamlServiceProvider21.Add(typeFromHandle41, obj30 = new SimpleValueTargetProvider(array22, typeof(BindingExtension).GetRuntimeProperty("Converter"), nameScope));
			xamlServiceProvider21.Add(typeof(IReferenceProvider), obj30);
			Type typeFromHandle42 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver21 = new XmlNamespaceResolver();
			xmlNamespaceResolver21.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver21.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver21.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver21.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver21.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider21.Add(typeFromHandle42, new XamlTypeResolver(xmlNamespaceResolver21, typeof(EcuInfoPageV2).GetTypeInfo().Assembly));
			xamlServiceProvider21.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(197, 21)));
			object obj31 = markupExtension21.ProvideValue(xamlServiceProvider21);
			bindingExtension17.Converter = obj31;
			bindingExtension17.Path = "HasDTCToDisplay";
			BindingBase bindingBase17 = bindingExtension17.ProvideValue(null);
			stackLayout2.SetBinding(VisualElement.IsVisibleProperty, bindingBase17);
			stackLayout2.SetValue(StackLayout.OrientationProperty, 0);
			label2.SetValue(Label.FontAttributesProperty, new FontAttributesConverter().ConvertFromInvariantString("Bold"));
			translate7.Text = "versions_NoInformationAvailable";
			IMarkupExtension markupExtension22 = translate7;
			XamlServiceProvider xamlServiceProvider22 = new XamlServiceProvider();
			Type typeFromHandle43 = typeof(IProvideValueTarget);
			object[] array23 = new object[0 + 5];
			array23[0] = label2;
			array23[1] = stackLayout2;
			array23[2] = grid5;
			array23[3] = grid6;
			array23[4] = this;
			object obj32;
			xamlServiceProvider22.Add(typeFromHandle43, obj32 = new SimpleValueTargetProvider(array23, Label.TextProperty, nameScope));
			xamlServiceProvider22.Add(typeof(IReferenceProvider), obj32);
			Type typeFromHandle44 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver22 = new XmlNamespaceResolver();
			xmlNamespaceResolver22.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver22.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver22.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver22.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver22.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider22.Add(typeFromHandle44, new XamlTypeResolver(xmlNamespaceResolver22, typeof(EcuInfoPageV2).GetTypeInfo().Assembly));
			xamlServiceProvider22.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(199, 50)));
			object obj33 = markupExtension22.ProvideValue(xamlServiceProvider22);
			label2.Text = obj33;
			stackLayout2.Children.Add(label2);
			label3.SetValue(Label.FontAttributesProperty, new FontAttributesConverter().ConvertFromInvariantString("Italic"));
			bindingExtension18.Mode = 2;
			bindingExtension18.Path = "BadELMDetected";
			BindingBase bindingBase18 = bindingExtension18.ProvideValue(null);
			label3.SetBinding(VisualElement.IsVisibleProperty, bindingBase18);
			translate8.Text = "dtc_BadELMDetected";
			IMarkupExtension markupExtension23 = translate8;
			XamlServiceProvider xamlServiceProvider23 = new XamlServiceProvider();
			Type typeFromHandle45 = typeof(IProvideValueTarget);
			object[] array24 = new object[0 + 5];
			array24[0] = label3;
			array24[1] = stackLayout2;
			array24[2] = grid5;
			array24[3] = grid6;
			array24[4] = this;
			object obj34;
			xamlServiceProvider23.Add(typeFromHandle45, obj34 = new SimpleValueTargetProvider(array24, Label.TextProperty, nameScope));
			xamlServiceProvider23.Add(typeof(IReferenceProvider), obj34);
			Type typeFromHandle46 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver23 = new XmlNamespaceResolver();
			xmlNamespaceResolver23.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver23.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver23.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver23.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver23.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider23.Add(typeFromHandle46, new XamlTypeResolver(xmlNamespaceResolver23, typeof(EcuInfoPageV2).GetTypeInfo().Assembly));
			xamlServiceProvider23.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(204, 25)));
			object obj35 = markupExtension23.ProvideValue(xamlServiceProvider23);
			label3.Text = obj35;
			dynamicResourceExtension12.Key = "RedTextColor";
			IMarkupExtension<DynamicResource> markupExtension24 = dynamicResourceExtension12;
			XamlServiceProvider xamlServiceProvider24 = new XamlServiceProvider();
			Type typeFromHandle47 = typeof(IProvideValueTarget);
			object[] array25 = new object[0 + 5];
			array25[0] = label3;
			array25[1] = stackLayout2;
			array25[2] = grid5;
			array25[3] = grid6;
			array25[4] = this;
			object obj36;
			xamlServiceProvider24.Add(typeFromHandle47, obj36 = new SimpleValueTargetProvider(array25, Label.TextColorProperty, nameScope));
			xamlServiceProvider24.Add(typeof(IReferenceProvider), obj36);
			Type typeFromHandle48 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver24 = new XmlNamespaceResolver();
			xmlNamespaceResolver24.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver24.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver24.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver24.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver24.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider24.Add(typeFromHandle48, new XamlTypeResolver(xmlNamespaceResolver24, typeof(EcuInfoPageV2).GetTypeInfo().Assembly));
			xamlServiceProvider24.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(205, 25)));
			DynamicResource dynamicResource12 = markupExtension24.ProvideValue(xamlServiceProvider24);
			label3.SetDynamicResource(Label.TextColorProperty, dynamicResource12.Key);
			tapGestureRecognizer4.SetValue(TapGestureRecognizer.NumberOfTapsRequiredProperty, 1);
			tapGestureRecognizer4.Tapped += this.LabelBadDTCTapGestureRecognizer_Tapped;
			label3.GestureRecognizers.Add(tapGestureRecognizer4);
			stackLayout2.Children.Add(label3);
			grid5.Children.Add(stackLayout2);
			scrollView.SetValue(Grid.RowProperty, 1);
			scrollView.SetValue(ScrollView.OrientationProperty, 0);
			bindingExtension19.Mode = 2;
			bindingExtension19.Path = "ECUInfoReport";
			BindingBase bindingBase19 = bindingExtension19.ProvideValue(null);
			label4.SetBinding(Label.FormattedTextProperty, bindingBase19);
			scrollView.Content = label4;
			grid5.Children.Add(scrollView);
			activityFrame2.SetValue(Grid.RowProperty, 2);
			activityFrame2.SetValue(View.MarginProperty, new Thickness(0.0));
			bindingExtension20.Path = "IsBusy";
			BindingBase bindingBase20 = bindingExtension20.ProvideValue(null);
			activityFrame2.SetBinding(VisualElement.IsVisibleProperty, bindingBase20);
			bindingExtension21.Path = "StatusText";
			BindingBase bindingBase21 = bindingExtension21.ProvideValue(null);
			activityFrame2.SetBinding(ActivityFrame.TextProperty, bindingBase21);
			activityFrame2.SetValue(View.VerticalOptionsProperty, LayoutOptions.Center);
			grid5.Children.Add(activityFrame2);
			button2.SetValue(Grid.RowProperty, 3);
			button2.Clicked += this.ButtonBackCancel_Clicked;
			bindingExtension22.Path = "CancelCommand";
			BindingBase bindingBase22 = bindingExtension22.ProvideValue(null);
			button2.SetBinding(Button.CommandProperty, bindingBase22);
			bindingExtension23.Mode = 2;
			bindingExtension23.Path = "IsCancelButtonEnabled";
			BindingBase bindingBase23 = bindingExtension23.ProvideValue(null);
			button2.SetBinding(VisualElement.IsEnabledProperty, bindingBase23);
			bindingExtension24.Path = "IsBusy";
			BindingBase bindingBase24 = bindingExtension24.ProvideValue(null);
			button2.SetBinding(VisualElement.IsVisibleProperty, bindingBase24);
			translate9.Text = "ios_Cancel";
			IMarkupExtension markupExtension25 = translate9;
			XamlServiceProvider xamlServiceProvider25 = new XamlServiceProvider();
			Type typeFromHandle49 = typeof(IProvideValueTarget);
			object[] array26 = new object[0 + 4];
			array26[0] = button2;
			array26[1] = grid5;
			array26[2] = grid6;
			array26[3] = this;
			object obj37;
			xamlServiceProvider25.Add(typeFromHandle49, obj37 = new SimpleValueTargetProvider(array26, Button.TextProperty, nameScope));
			xamlServiceProvider25.Add(typeof(IReferenceProvider), obj37);
			Type typeFromHandle50 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver25 = new XmlNamespaceResolver();
			xmlNamespaceResolver25.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver25.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver25.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver25.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver25.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider25.Add(typeFromHandle50, new XamlTypeResolver(xmlNamespaceResolver25, typeof(EcuInfoPageV2).GetTypeInfo().Assembly));
			xamlServiceProvider25.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(232, 21)));
			object obj38 = markupExtension25.ProvideValue(xamlServiceProvider25);
			button2.Text = obj38;
			grid5.Children.Add(button2);
			grid4.SetValue(Grid.RowProperty, 3);
			staticResourceExtension5.Key = "BoolToNegativeConverter";
			IMarkupExtension markupExtension26 = staticResourceExtension5;
			XamlServiceProvider xamlServiceProvider26 = new XamlServiceProvider();
			Type typeFromHandle51 = typeof(IProvideValueTarget);
			object[] array27 = new object[0 + 5];
			array27[0] = bindingExtension25;
			array27[1] = grid4;
			array27[2] = grid5;
			array27[3] = grid6;
			array27[4] = this;
			object obj39;
			xamlServiceProvider26.Add(typeFromHandle51, obj39 = new SimpleValueTargetProvider(array27, typeof(BindingExtension).GetRuntimeProperty("Converter"), nameScope));
			xamlServiceProvider26.Add(typeof(IReferenceProvider), obj39);
			Type typeFromHandle52 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver26 = new XmlNamespaceResolver();
			xmlNamespaceResolver26.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver26.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver26.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver26.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver26.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider26.Add(typeFromHandle52, new XamlTypeResolver(xmlNamespaceResolver26, typeof(EcuInfoPageV2).GetTypeInfo().Assembly));
			xamlServiceProvider26.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(234, 36)));
			object obj40 = markupExtension26.ProvideValue(xamlServiceProvider26);
			bindingExtension25.Converter = obj40;
			bindingExtension25.Path = "IsBusy";
			BindingBase bindingBase25 = bindingExtension25.ProvideValue(null);
			grid4.SetBinding(VisualElement.IsVisibleProperty, bindingBase25);
			bindingExtension26.Path = "SharedECUInfoCommand";
			BindingBase bindingBase26 = bindingExtension26.ProvideValue(null);
			button3.SetBinding(Button.CommandProperty, bindingBase26);
			bindingExtension27.Mode = 2;
			bindingExtension27.Path = "HasDTCToDisplay";
			BindingBase bindingBase27 = bindingExtension27.ProvideValue(null);
			button3.SetBinding(VisualElement.IsVisibleProperty, bindingBase27);
			translate10.Text = "ios_Share";
			IMarkupExtension markupExtension27 = translate10;
			XamlServiceProvider xamlServiceProvider27 = new XamlServiceProvider();
			Type typeFromHandle53 = typeof(IProvideValueTarget);
			object[] array28 = new object[0 + 5];
			array28[0] = button3;
			array28[1] = grid4;
			array28[2] = grid5;
			array28[3] = grid6;
			array28[4] = this;
			object obj41;
			xamlServiceProvider27.Add(typeFromHandle53, obj41 = new SimpleValueTargetProvider(array28, Button.TextProperty, nameScope));
			xamlServiceProvider27.Add(typeof(IReferenceProvider), obj41);
			Type typeFromHandle54 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver27 = new XmlNamespaceResolver();
			xmlNamespaceResolver27.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver27.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver27.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver27.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver27.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider27.Add(typeFromHandle54, new XamlTypeResolver(xmlNamespaceResolver27, typeof(EcuInfoPageV2).GetTypeInfo().Assembly));
			xamlServiceProvider27.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(238, 25)));
			object obj42 = markupExtension27.ProvideValue(xamlServiceProvider27);
			button3.Text = obj42;
			grid4.Children.Add(button3);
			grid5.Children.Add(grid4);
			stackLayout3.SetValue(Grid.RowProperty, 4);
			bindingExtension28.Mode = 2;
			bindingExtension28.Path = "HasDTCToDisplay";
			BindingBase bindingBase28 = bindingExtension28.ProvideValue(null);
			stackLayout3.SetBinding(VisualElement.IsVisibleProperty, bindingBase28);
			stackLayout3.SetValue(StackLayout.OrientationProperty, 0);
			label5.SetValue(Label.FontAttributesProperty, new FontAttributesConverter().ConvertFromInvariantString("Italic"));
			dynamicResourceExtension13.Key = "BaseFontSize-";
			IMarkupExtension<DynamicResource> markupExtension28 = dynamicResourceExtension13;
			XamlServiceProvider xamlServiceProvider28 = new XamlServiceProvider();
			Type typeFromHandle55 = typeof(IProvideValueTarget);
			object[] array29 = new object[0 + 5];
			array29[0] = label5;
			array29[1] = stackLayout3;
			array29[2] = grid5;
			array29[3] = grid6;
			array29[4] = this;
			object obj43;
			xamlServiceProvider28.Add(typeFromHandle55, obj43 = new SimpleValueTargetProvider(array29, Label.FontSizeProperty, nameScope));
			xamlServiceProvider28.Add(typeof(IReferenceProvider), obj43);
			Type typeFromHandle56 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver28 = new XmlNamespaceResolver();
			xmlNamespaceResolver28.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver28.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver28.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver28.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver28.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider28.Add(typeFromHandle56, new XamlTypeResolver(xmlNamespaceResolver28, typeof(EcuInfoPageV2).GetTypeInfo().Assembly));
			xamlServiceProvider28.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(248, 25)));
			DynamicResource dynamicResource13 = markupExtension28.ProvideValue(xamlServiceProvider28);
			label5.SetDynamicResource(Label.FontSizeProperty, dynamicResource13.Key);
			bindingExtension29.Mode = 2;
			bindingExtension29.Path = "BadELMDetected";
			BindingBase bindingBase29 = bindingExtension29.ProvideValue(null);
			label5.SetBinding(VisualElement.IsVisibleProperty, bindingBase29);
			translate11.Text = "dtc_BadELMDetected";
			IMarkupExtension markupExtension29 = translate11;
			XamlServiceProvider xamlServiceProvider29 = new XamlServiceProvider();
			Type typeFromHandle57 = typeof(IProvideValueTarget);
			object[] array30 = new object[0 + 5];
			array30[0] = label5;
			array30[1] = stackLayout3;
			array30[2] = grid5;
			array30[3] = grid6;
			array30[4] = this;
			object obj44;
			xamlServiceProvider29.Add(typeFromHandle57, obj44 = new SimpleValueTargetProvider(array30, Label.TextProperty, nameScope));
			xamlServiceProvider29.Add(typeof(IReferenceProvider), obj44);
			Type typeFromHandle58 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver29 = new XmlNamespaceResolver();
			xmlNamespaceResolver29.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver29.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver29.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver29.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver29.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider29.Add(typeFromHandle58, new XamlTypeResolver(xmlNamespaceResolver29, typeof(EcuInfoPageV2).GetTypeInfo().Assembly));
			xamlServiceProvider29.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(250, 25)));
			object obj45 = markupExtension29.ProvideValue(xamlServiceProvider29);
			label5.Text = obj45;
			dynamicResourceExtension14.Key = "RedTextColor";
			IMarkupExtension<DynamicResource> markupExtension30 = dynamicResourceExtension14;
			XamlServiceProvider xamlServiceProvider30 = new XamlServiceProvider();
			Type typeFromHandle59 = typeof(IProvideValueTarget);
			object[] array31 = new object[0 + 5];
			array31[0] = label5;
			array31[1] = stackLayout3;
			array31[2] = grid5;
			array31[3] = grid6;
			array31[4] = this;
			object obj46;
			xamlServiceProvider30.Add(typeFromHandle59, obj46 = new SimpleValueTargetProvider(array31, Label.TextColorProperty, nameScope));
			xamlServiceProvider30.Add(typeof(IReferenceProvider), obj46);
			Type typeFromHandle60 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver30 = new XmlNamespaceResolver();
			xmlNamespaceResolver30.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver30.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver30.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver30.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver30.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider30.Add(typeFromHandle60, new XamlTypeResolver(xmlNamespaceResolver30, typeof(EcuInfoPageV2).GetTypeInfo().Assembly));
			xamlServiceProvider30.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(251, 25)));
			DynamicResource dynamicResource14 = markupExtension30.ProvideValue(xamlServiceProvider30);
			label5.SetDynamicResource(Label.TextColorProperty, dynamicResource14.Key);
			tapGestureRecognizer5.SetValue(TapGestureRecognizer.NumberOfTapsRequiredProperty, 1);
			tapGestureRecognizer5.Tapped += this.LabelBadDTCTapGestureRecognizer_Tapped;
			label5.GestureRecognizers.Add(tapGestureRecognizer5);
			stackLayout3.Children.Add(label5);
			grid5.Children.Add(stackLayout3);
			grid6.Children.Add(grid5);
			complexAdView.SetValue(Grid.RowProperty, 1);
			complexAdView.SetValue(View.MarginProperty, new Thickness(0.0, 0.0, 0.0, 0.0));
			complexAdView.SetValue(VisualElement.HeightRequestProperty, 55.0);
			complexAdView.SetValue(VisualElement.IsVisibleProperty, new VisualElement.VisibilityConverter().ConvertFromInvariantString("true"));
			complexAdView.SetValue(View.VerticalOptionsProperty, LayoutOptions.End);
			grid6.Children.Add(complexAdView);
			this.SetValue(ContentPage.ContentProperty, grid6);
		}

		// Token: 0x0600371C RID: 14108 RVA: 0x002931C0 File Offset: 0x002913C0
		private void __InitComponentRuntime()
		{
			Extensions.LoadFromXaml<EcuInfoPageV2>(this, typeof(EcuInfoPageV2));
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
			this.btnReadDTC = NameScopeExtensions.FindByName<Button>(this, "btnReadDTC");
			this.activityFrame2 = NameScopeExtensions.FindByName<ActivityFrame>(this, "activityFrame2");
			this.gridResults = NameScopeExtensions.FindByName<Grid>(this, "gridResults");
			this.lbResults = NameScopeExtensions.FindByName<Label>(this, "lbResults");
			this.activityFrame = NameScopeExtensions.FindByName<ActivityFrame>(this, "activityFrame");
			this.btnCancel = NameScopeExtensions.FindByName<Button>(this, "btnCancel");
			this.ad = NameScopeExtensions.FindByName<ComplexAdView>(this, "ad");
		}

		// Token: 0x0400213A RID: 8506
		public DTCv2Model Model;

		// Token: 0x0400213B RID: 8507
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private Grid gridButtons;

		// Token: 0x0400213C RID: 8508
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private Image btnCheckAll;

		// Token: 0x0400213D RID: 8509
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private Image btnUncheckAll;

		// Token: 0x0400213E RID: 8510
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private Image btnFilter;

		// Token: 0x0400213F RID: 8511
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private Grid layoutRoot;

		// Token: 0x04002140 RID: 8512
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private Grid gridSetup;

		// Token: 0x04002141 RID: 8513
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private ListView lvECUs;

		// Token: 0x04002142 RID: 8514
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private Grid gridAllOrSupportedSelector;

		// Token: 0x04002143 RID: 8515
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private RadioButtonWithColor rbDetected;

		// Token: 0x04002144 RID: 8516
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private RadioButtonWithColor rbAll;

		// Token: 0x04002145 RID: 8517
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private Button btnReadDTC;

		// Token: 0x04002146 RID: 8518
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private ActivityFrame activityFrame2;

		// Token: 0x04002147 RID: 8519
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private Grid gridResults;

		// Token: 0x04002148 RID: 8520
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private Label lbResults;

		// Token: 0x04002149 RID: 8521
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private ActivityFrame activityFrame;

		// Token: 0x0400214A RID: 8522
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private Button btnCancel;

		// Token: 0x0400214B RID: 8523
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private ComplexAdView ad;

		// Token: 0x02000627 RID: 1575
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <ButtonBackCancel_Clicked>d__5 : IAsyncStateMachine
		{
			// Token: 0x0600371D RID: 14109 RVA: 0x00293300 File Offset: 0x00291500
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				EcuInfoPageV2 ecuInfoPageV = this;
				try
				{
					TaskAwaiter<Page> taskAwaiter;
					if (num != 0)
					{
						(sender as View).IsEnabled = false;
						if (ecuInfoPageV.Model.ECUListFull.Count != 1)
						{
							goto IL_0093;
						}
						taskAwaiter = ecuInfoPageV.Navigation.PopAsync().GetAwaiter();
						if (!taskAwaiter.IsCompleted)
						{
							num2 = 0;
							TaskAwaiter<Page> taskAwaiter2 = taskAwaiter;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<Page>, EcuInfoPageV2.<ButtonBackCancel_Clicked>d__5>(ref taskAwaiter, ref this);
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
					IL_0093:
					(sender as View).IsEnabled = true;
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

			// Token: 0x0600371E RID: 14110 RVA: 0x002933F0 File Offset: 0x002915F0
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x0400214C RID: 8524
			public int <>1__state;

			// Token: 0x0400214D RID: 8525
			public AsyncVoidMethodBuilder <>t__builder;

			// Token: 0x0400214E RID: 8526
			public object sender;

			// Token: 0x0400214F RID: 8527
			public EcuInfoPageV2 <>4__this;

			// Token: 0x04002150 RID: 8528
			private TaskAwaiter<Page> <>u__1;
		}

		// Token: 0x02000628 RID: 1576
		[CompilerGenerated]
		private sealed class <InitializeComponent>_anonXamlCDataTemplate_54
		{
			// Token: 0x0600371F RID: 14111 RVA: 0x00293400 File Offset: 0x00291600
			public <InitializeComponent>_anonXamlCDataTemplate_54()
			{
			}

			// Token: 0x06003720 RID: 14112 RVA: 0x00293414 File Offset: 0x00291614
			internal object LoadDataTemplate()
			{
				BindingExtension bindingExtension;
				VisualDiagnostics.RegisterSourceInfo(bindingExtension = new BindingExtension(), new Uri("Pages\\EcuInfoPageV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 116, 37);
				BindingExtension bindingExtension2;
				VisualDiagnostics.RegisterSourceInfo(bindingExtension2 = new BindingExtension(), new Uri("Pages\\EcuInfoPageV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 117, 37);
				LabelSwitch labelSwitch;
				VisualDiagnostics.RegisterSourceInfo(labelSwitch = new LabelSwitch(), new Uri("Pages\\EcuInfoPageV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 114, 34);
				ViewCell viewCell;
				VisualDiagnostics.RegisterSourceInfo(viewCell = new ViewCell(), new Uri("Pages\\EcuInfoPageV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 113, 30);
				NameScope nameScope = new NameScope();
				NameScope.SetNameScope(viewCell, nameScope);
				labelSwitch.SetValue(View.MarginProperty, new Thickness(0.0, 5.0));
				bindingExtension.Mode = 1;
				bindingExtension.Path = "IsSelected";
				BindingBase bindingBase = bindingExtension.ProvideValue(null);
				labelSwitch.SetBinding(LabelSwitch.IsToggledProperty, bindingBase);
				bindingExtension2.Path = "Name";
				BindingBase bindingBase2 = bindingExtension2.ProvideValue(null);
				labelSwitch.SetBinding(LabelSwitch.TextProperty, bindingBase2);
				viewCell.View = labelSwitch;
				return viewCell;
			}

			// Token: 0x04002151 RID: 8529
			internal object[] parentValues;

			// Token: 0x04002152 RID: 8530
			internal EcuInfoPageV2 root;
		}
	}
}
