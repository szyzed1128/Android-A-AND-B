using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using CarScannerXamarinForms.OBD2;
using CarScannerXamarinForms.OBD2.PIDS;
using CarScannerXamarinForms.OBD2.RequestProducers;
using CarScannerXamarinForms.Settings;
using CarScannerXamarinForms.UserControls;
using CarScannerXamarinForms.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Internals;
using Xamarin.Forms.PlatformConfiguration.iOSSpecific;
using Xamarin.Forms.Xaml;
using Xamarin.Forms.Xaml.Diagnostics;
using Xamarin.Forms.Xaml.Internals;

namespace CarScannerXamarinForms
{
	// Token: 0x020000F1 RID: 241
	[XamlFilePath("Pages\\Mode06Page.xaml")]
	public class Mode06Page : ContentPage, INotifyPropertyChanged
	{
		// Token: 0x060004AA RID: 1194 RVA: 0x00044D2C File Offset: 0x00042F2C
		public Mode06Page()
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
			bool mode06AttentionShow = SharedSettings.Current.Mode06AttentionShow;
			if (!App.OBDSimulator.IsActive)
			{
				this.ReadMode06Data();
			}
			else
			{
				Mode6Test mode6Test = new Mode6Test
				{
					MaxValue = 100.0,
					MinValue = 0.0,
					MonitorName = "Test monitor Id 01",
					TestId = "Range test",
					TestValue = 150.0,
					Units = UnitsHelper.Units.None
				};
				App.OBDReader.CurrentCarData.Mode06TestCollection.Add(mode6Test);
				mode6Test = new Mode6Test
				{
					MaxValue = 20.0,
					MinValue = 0.0,
					MonitorName = "Test monitor Id 02",
					TestId = "Voltage test",
					TestValue = 15.0,
					Units = UnitsHelper.Units.volts
				};
				mode6Test.CheckTestPassed();
				App.OBDReader.CurrentCarData.Mode06TestCollection.Add(mode6Test);
			}
			base.BindingContext = App.OBDReader.CurrentCarData.Mode06TestCollection;
		}

		// Token: 0x060004AB RID: 1195 RVA: 0x000027D4 File Offset: 0x000009D4
		private void Handle_SizeChanged(object sender, EventArgs e)
		{
		}

		// Token: 0x060004AC RID: 1196 RVA: 0x00044E74 File Offset: 0x00043074
		private void Handle_Appearing(object sender, EventArgs e)
		{
			if (!SharedSettings.Current.InfoShowed_Mode06)
			{
				SharedSettings.Current.InfoShowed_Mode06 = true;
				this.btnInfo_Clicked(null, null);
			}
		}

		// Token: 0x17000094 RID: 148
		// (get) Token: 0x060004AD RID: 1197 RVA: 0x00044E95 File Offset: 0x00043095
		public ObservableCollection<Mode6Test> DefaultViewModel
		{
			get
			{
				return App.OBDReader.CurrentCarData.Mode06TestCollection;
			}
		}

		// Token: 0x060004AE RID: 1198 RVA: 0x00044EA6 File Offset: 0x000430A6
		private void btnHideAttention_Clicked(object sender, EventArgs e)
		{
			SharedSettings.Current.Mode06AttentionShow = true;
		}

		// Token: 0x060004AF RID: 1199 RVA: 0x00044EB4 File Offset: 0x000430B4
		private async void btnRefresh_Clicked(object sender, EventArgs e)
		{
			if (App.OBDReader.CurrentStatus == OBDDataReaderStatus.ConnectedToECU && !App.OBDSimulator.IsActive)
			{
				await this.ReadMode06Data();
			}
			else
			{
				App.OBDReader.CurrentCarData.Mode06TestCollection.Clear();
				Mode6Test mode6Test = new Mode6Test();
				mode6Test.MaxValue = 100.0;
				mode6Test.MinValue = 0.0;
				mode6Test.MonitorName = "Test monitor Id 01";
				mode6Test.TestId = "Range test";
				mode6Test.TestValue = 150.0;
				mode6Test.Units = UnitsHelper.Units.None;
				App.OBDReader.CurrentCarData.Mode06TestCollection.Add(mode6Test);
				Mode6Test mode6Test2 = new Mode6Test();
				mode6Test2.MaxValue = 20.0;
				mode6Test2.MinValue = 0.0;
				mode6Test2.MonitorName = "Test monitor Id 02";
				mode6Test2.TestId = "Voltage test";
				mode6Test2.TestValue = 15.0;
				mode6Test2.Units = UnitsHelper.Units.volts;
				mode6Test2.CheckTestPassed();
				App.OBDReader.CurrentCarData.Mode06TestCollection.Add(mode6Test2);
			}
		}

		// Token: 0x060004B0 RID: 1200 RVA: 0x00044EEC File Offset: 0x000430EC
		private async Task ReadMode06Data()
		{
			this.btnRefresh.IsEnabled = false;
			this.lbNotSupported.Text = "";
			this.lbNotSupported.IsVisible = false;
			string old_caption = this.btnRefresh.Text;
			this.btnRefresh.Text = Translate.GetString("Mode06Page_btnRefresh_Reading");
			this.progressBar.IsVisible = true;
			Progress<double> progress = new Progress<double>(delegate(double x)
			{
				this.progressBar.Progress = x;
			});
			if (!this.full_search_completed)
			{
				await App.OBDReader.ReadMode06(progress);
				this.full_search_completed = true;
			}
			else
			{
				string[] array = App.OBDReader.CurrentCarData.Mode06TestCollection.Select((Mode6Test x) => x.Cmd).Distinct<string>().ToArray<string>();
				await App.OBDReader.ReadMode06(progress, array);
			}
			if (App.OBDReader.CurrentCarData.Mode06TestCollection != null && App.OBDReader.CurrentCarData.Mode06TestCollection.Count == 0)
			{
				string text = Translate.GetString("Mode06_NotSupported_OrBadELM");
				if (App.OBDReader.CurrentELMFormat == ELMFormat.KWP)
				{
					text = Translate.GetString("Mode06_NotSupported");
				}
				else if (SharedSettings.Current.FuelType == FuelTypes.Diesel && (SharedSettings.Current.SelectedBrand == "Hyundai" || SharedSettings.Current.SelectedBrand == "Kia" || SharedSettings.Current.SelectedBrand == "Genesis"))
				{
					text = Translate.GetString("Mode06_NotSupported");
				}
				this.lbNotSupported.Text = text;
				this.lbNotSupported.IsVisible = true;
			}
			RequestProducerStatic.UpdateOBDReaderRequests();
			this.progressBar.IsVisible = false;
			this.btnRefresh.IsEnabled = true;
			this.btnRefresh.Text = old_caption;
		}

		// Token: 0x060004B1 RID: 1201 RVA: 0x00044F30 File Offset: 0x00043130
		private async void btnBack_Clicked(object sender, EventArgs e)
		{
			await base.Navigation.PopAsync(true);
		}

		// Token: 0x060004B2 RID: 1202 RVA: 0x00044F67 File Offset: 0x00043167
		protected override bool OnBackButtonPressed()
		{
			if (Device.RuntimePlatform == "UWP" || Device.RuntimePlatform == "Android")
			{
				this.btnBack_Clicked(this, null);
			}
			return true;
		}

		// Token: 0x060004B3 RID: 1203 RVA: 0x00044F94 File Offset: 0x00043194
		private async void btnCreateCustomPid_Clicked(object sender, EventArgs e)
		{
			if (!App.OBDSimulator.IsActive)
			{
				CustomPID customPID = Mode6Test.CreateCustomPidFromMode6Test((sender as MenuItem).BindingContext as Mode6Test);
				customPID.IsFormulaHidden = true;
				CustomPIDViewModel.CurrentCustom.PidCollection.Add(customPID);
				CustomPIDViewModel.CurrentCustom.Save();
				LiveDataPIDModel.UpdatePIDCollection(App.OBDReader);
			}
			await base.DisplayAlert(Translate.GetString("Mode06Page_SensorCreated_Title"), Translate.GetString("Mode06Page_SensorCreated_Text"), "OK");
		}

		// Token: 0x060004B4 RID: 1204 RVA: 0x00044FD3 File Offset: 0x000431D3
		private void btnInfo_Clicked(object sender, EventArgs e)
		{
			base.DisplayAlert(Translate.GetString("ios_Mode06Page_Title"), Translate.GetString("ios_Mode06Alert_Text"), "OK");
		}

		// Token: 0x060004B5 RID: 1205 RVA: 0x00034544 File Offset: 0x00032744
		private void Handle_ItemTapped(object sender, ItemTappedEventArgs e)
		{
			(sender as ListView).SelectedItem = null;
		}

		// Token: 0x060004B6 RID: 1206 RVA: 0x00044FF8 File Offset: 0x000431F8
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private void InitializeComponent()
		{
			if (ResourceLoader.IsEnabled && ResourceLoader.CanProvideContentFor(new ResourceLoader.ResourceLoadingQuery
			{
				AssemblyName = typeof(Mode06Page).GetTypeInfo().Assembly.GetName(),
				ResourcePath = "Pages/Mode06Page.xaml",
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
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension = new DynamicResourceExtension(), new Uri("Pages\\Mode06Page.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 12, 5);
			On on;
			VisualDiagnostics.RegisterSourceInfo(on = new On(), new Uri("Pages\\Mode06Page.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 18, 14);
			On on2;
			VisualDiagnostics.RegisterSourceInfo(on2 = new On(), new Uri("Pages\\Mode06Page.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 19, 14);
			OnPlatform<Thickness> onPlatform;
			VisualDiagnostics.RegisterSourceInfo(onPlatform = new OnPlatform<Thickness>(), new Uri("Pages\\Mode06Page.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 17, 10);
			Setter setter;
			VisualDiagnostics.RegisterSourceInfo(setter = new Setter(), new Uri("Pages\\Mode06Page.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 25, 18);
			Setter setter2;
			VisualDiagnostics.RegisterSourceInfo(setter2 = new Setter(), new Uri("Pages\\Mode06Page.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 26, 18);
			Setter setter3;
			VisualDiagnostics.RegisterSourceInfo(setter3 = new Setter(), new Uri("Pages\\Mode06Page.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 27, 18);
			Setter setter4;
			VisualDiagnostics.RegisterSourceInfo(setter4 = new Setter(), new Uri("Pages\\Mode06Page.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 28, 18);
			DynamicResourceExtension dynamicResourceExtension2;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension2 = new DynamicResourceExtension(), new Uri("Pages\\Mode06Page.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 29, 45);
			Setter setter5;
			VisualDiagnostics.RegisterSourceInfo(setter5 = new Setter(), new Uri("Pages\\Mode06Page.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 29, 18);
			Style style;
			VisualDiagnostics.RegisterSourceInfo(style = new Style(typeof(Label)), new Uri("Pages\\Mode06Page.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 24, 14);
			ResourceDictionary resourceDictionary;
			VisualDiagnostics.RegisterSourceInfo(resourceDictionary = new ResourceDictionary(), new Uri("Pages\\Mode06Page.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 23, 10);
			ColumnDefinition columnDefinition;
			VisualDiagnostics.RegisterSourceInfo(columnDefinition = new ColumnDefinition(), new Uri("Pages\\Mode06Page.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 36, 18);
			ColumnDefinition columnDefinition2;
			VisualDiagnostics.RegisterSourceInfo(columnDefinition2 = new ColumnDefinition(), new Uri("Pages\\Mode06Page.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 37, 18);
			ColumnDefinition columnDefinition3;
			VisualDiagnostics.RegisterSourceInfo(columnDefinition3 = new ColumnDefinition(), new Uri("Pages\\Mode06Page.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 38, 18);
			DynamicResourceExtension dynamicResourceExtension3;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension3 = new DynamicResourceExtension(), new Uri("Pages\\Mode06Page.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 45, 17);
			Translate translate;
			VisualDiagnostics.RegisterSourceInfo(translate = new Translate(), new Uri("Pages\\Mode06Page.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 46, 17);
			LinkButton linkButton;
			VisualDiagnostics.RegisterSourceInfo(linkButton = new LinkButton(), new Uri("Pages\\Mode06Page.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 40, 14);
			DynamicResourceExtension dynamicResourceExtension4;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension4 = new DynamicResourceExtension(), new Uri("Pages\\Mode06Page.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 57, 17);
			Translate translate2;
			VisualDiagnostics.RegisterSourceInfo(translate2 = new Translate(), new Uri("Pages\\Mode06Page.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 58, 17);
			NonScalableLabel nonScalableLabel;
			VisualDiagnostics.RegisterSourceInfo(nonScalableLabel = new NonScalableLabel(), new Uri("Pages\\Mode06Page.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 55, 14);
			DynamicResourceExtension dynamicResourceExtension5;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension5 = new DynamicResourceExtension(), new Uri("Pages\\Mode06Page.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 63, 17);
			DynamicResourceExtension dynamicResourceExtension6;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension6 = new DynamicResourceExtension(), new Uri("Pages\\Mode06Page.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 64, 17);
			On on3;
			VisualDiagnostics.RegisterSourceInfo(on3 = new On(), new Uri("Pages\\Mode06Page.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 68, 26);
			On on4;
			VisualDiagnostics.RegisterSourceInfo(on4 = new On(), new Uri("Pages\\Mode06Page.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 69, 26);
			OnPlatform<Thickness> onPlatform2;
			VisualDiagnostics.RegisterSourceInfo(onPlatform2 = new OnPlatform<Thickness>(), new Uri("Pages\\Mode06Page.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 67, 22);
			LinkButton linkButton2;
			VisualDiagnostics.RegisterSourceInfo(linkButton2 = new LinkButton(), new Uri("Pages\\Mode06Page.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 59, 14);
			Grid grid;
			VisualDiagnostics.RegisterSourceInfo(grid = new Grid(), new Uri("Pages\\Mode06Page.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 34, 10);
			OnPlatform<Thickness> onPlatform3;
			VisualDiagnostics.RegisterSourceInfo(onPlatform3 = new OnPlatform<Thickness>(), new Uri("Pages\\Mode06Page.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 78, 18);
			RowDefinition rowDefinition;
			VisualDiagnostics.RegisterSourceInfo(rowDefinition = new RowDefinition(), new Uri("Pages\\Mode06Page.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 85, 18);
			RowDefinition rowDefinition2;
			VisualDiagnostics.RegisterSourceInfo(rowDefinition2 = new RowDefinition(), new Uri("Pages\\Mode06Page.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 86, 18);
			RowDefinition rowDefinition3;
			VisualDiagnostics.RegisterSourceInfo(rowDefinition3 = new RowDefinition(), new Uri("Pages\\Mode06Page.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 87, 18);
			RowDefinition rowDefinition4;
			VisualDiagnostics.RegisterSourceInfo(rowDefinition4 = new RowDefinition(), new Uri("Pages\\Mode06Page.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 88, 18);
			RowDefinition rowDefinition5;
			VisualDiagnostics.RegisterSourceInfo(rowDefinition5 = new RowDefinition(), new Uri("Pages\\Mode06Page.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 89, 18);
			ColumnDefinition columnDefinition4;
			VisualDiagnostics.RegisterSourceInfo(columnDefinition4 = new ColumnDefinition(), new Uri("Pages\\Mode06Page.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 93, 18);
			BindingExtension bindingExtension;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension = new BindingExtension(), new Uri("Pages\\Mode06Page.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 110, 17);
			DataTemplate dataTemplate;
			VisualDiagnostics.RegisterSourceInfo(dataTemplate = new DataTemplate(), new Uri("Pages\\Mode06Page.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 112, 22);
			ListView listView;
			VisualDiagnostics.RegisterSourceInfo(listView = new ListView(), new Uri("Pages\\Mode06Page.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 104, 14);
			DynamicResourceExtension dynamicResourceExtension7;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension7 = new DynamicResourceExtension(), new Uri("Pages\\Mode06Page.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 306, 17);
			Label label;
			VisualDiagnostics.RegisterSourceInfo(label = new Label(), new Uri("Pages\\Mode06Page.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 299, 14);
			Translate translate3;
			VisualDiagnostics.RegisterSourceInfo(translate3 = new Translate(), new Uri("Pages\\Mode06Page.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 314, 17);
			Button button;
			VisualDiagnostics.RegisterSourceInfo(button = new Button(), new Uri("Pages\\Mode06Page.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 309, 14);
			ProgressBar progressBar;
			VisualDiagnostics.RegisterSourceInfo(progressBar = new ProgressBar(), new Uri("Pages\\Mode06Page.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 315, 14);
			ComplexAdView complexAdView;
			VisualDiagnostics.RegisterSourceInfo(complexAdView = new ComplexAdView(), new Uri("Pages\\Mode06Page.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 320, 14);
			Grid grid2;
			VisualDiagnostics.RegisterSourceInfo(grid2 = new Grid(), new Uri("Pages\\Mode06Page.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 76, 10);
			VisualDiagnostics.RegisterSourceInfo(this, new Uri("Pages\\Mode06Page.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 2, 2);
			NameScope nameScope = NameScope.GetNameScope(this) ?? new NameScope();
			NameScope.SetNameScope(this, nameScope);
			NameScope nameScope2 = new NameScope();
			NameScope nameScope3 = new NameScope();
			NameScope nameScope4 = new NameScope();
			NameScope nameScope5 = new NameScope();
			NameScope nameScope6 = new NameScope();
			nameScope.RegisterName("lbNotSupported", label);
			if (label.StyleId == null)
			{
				label.StyleId = "lbNotSupported";
			}
			nameScope.RegisterName("btnRefresh", button);
			if (button.StyleId == null)
			{
				button.StyleId = "btnRefresh";
			}
			nameScope.RegisterName("progressBar", progressBar);
			if (progressBar.StyleId == null)
			{
				progressBar.StyleId = "progressBar";
			}
			nameScope.RegisterName("ad", complexAdView);
			if (complexAdView.StyleId == null)
			{
				complexAdView.StyleId = "ad";
			}
			this.lbNotSupported = label;
			this.btnRefresh = button;
			this.progressBar = progressBar;
			this.ad = complexAdView;
			this.Resources = resourceDictionary;
			setter.Property = View.VerticalOptionsProperty;
			setter.Value = "Center";
			setter.Value = LayoutOptions.Center;
			style.Setters.Add(setter);
			setter2.Property = View.HorizontalOptionsProperty;
			setter2.Value = "Center";
			setter2.Value = LayoutOptions.Center;
			style.Setters.Add(setter2);
			setter3.Property = Label.VerticalTextAlignmentProperty;
			setter3.Value = "Center";
			setter3.Value = new TextAlignmentConverter().ConvertFromInvariantString("Center");
			style.Setters.Add(setter3);
			setter4.Property = Label.HorizontalTextAlignmentProperty;
			setter4.Value = "Center";
			setter4.Value = new TextAlignmentConverter().ConvertFromInvariantString("Center");
			style.Setters.Add(setter4);
			setter5.Property = Label.FontSizeProperty;
			dynamicResourceExtension2.Key = "BaseFontSize";
			IMarkupExtension<DynamicResource> markupExtension = dynamicResourceExtension2;
			XamlServiceProvider xamlServiceProvider = new XamlServiceProvider();
			Type typeFromHandle = typeof(IProvideValueTarget);
			object[] array = new object[0 + 4];
			array[0] = setter5;
			array[1] = style;
			array[2] = resourceDictionary;
			array[3] = this;
			object obj;
			xamlServiceProvider.Add(typeFromHandle, obj = new SimpleValueTargetProvider(array, typeof(Setter).GetRuntimeProperty("Value"), nameScope6));
			xamlServiceProvider.Add(typeof(IReferenceProvider), obj);
			Type typeFromHandle2 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver = new XmlNamespaceResolver();
			xmlNamespaceResolver.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider.Add(typeFromHandle2, new XamlTypeResolver(xmlNamespaceResolver, typeof(Mode06Page).GetTypeInfo().Assembly));
			xamlServiceProvider.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(29, 45)));
			DynamicResource dynamicResource = markupExtension.ProvideValue(xamlServiceProvider);
			setter5.Value = dynamicResource;
			style.Setters.Add(setter5);
			resourceDictionary.Add("SmallLabel", style);
			this.SetValue(Page.PrefersStatusBarHiddenProperty, 2);
			this.SetValue(Page.UseSafeAreaProperty, true);
			this.Appearing += this.Handle_Appearing;
			dynamicResourceExtension.Key = "BackgroundColor";
			IMarkupExtension<DynamicResource> markupExtension2 = dynamicResourceExtension;
			XamlServiceProvider xamlServiceProvider2 = new XamlServiceProvider();
			Type typeFromHandle3 = typeof(IProvideValueTarget);
			object[] array2 = new object[0 + 1];
			array2[0] = this;
			object obj2;
			xamlServiceProvider2.Add(typeFromHandle3, obj2 = new SimpleValueTargetProvider(array2, VisualElement.BackgroundColorProperty, nameScope));
			xamlServiceProvider2.Add(typeof(IReferenceProvider), obj2);
			Type typeFromHandle4 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver2 = new XmlNamespaceResolver();
			xmlNamespaceResolver2.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver2.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver2.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver2.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver2.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider2.Add(typeFromHandle4, new XamlTypeResolver(xmlNamespaceResolver2, typeof(Mode06Page).GetTypeInfo().Assembly));
			xamlServiceProvider2.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(12, 5)));
			DynamicResource dynamicResource2 = markupExtension2.ProvideValue(xamlServiceProvider2);
			this.SetDynamicResource(VisualElement.BackgroundColorProperty, dynamicResource2.Key);
			this.SetValue(NavigationPage.HasBackButtonProperty, false);
			this.SetValue(NavigationPage.HasNavigationBarProperty, true);
			this.SizeChanged += this.Handle_SizeChanged;
			on.Platform = new List<string>(2) { "Android", "WinPhone" };
			on.Value = "0";
			onPlatform.Platforms.Add(on);
			on2.Platform = new List<string>(1) { "iOS" };
			on2.Value = "5,20,5,0";
			onPlatform.Platforms.Add(on2);
			this.SetValue(Page.PaddingProperty, onPlatform);
			this.Resources = resourceDictionary;
			grid.SetValue(Grid.RowProperty, 0);
			columnDefinition.SetValue(ColumnDefinition.WidthProperty, new GridLengthTypeConverter().ConvertFromInvariantString("*"));
			grid.GetValue(Grid.ColumnDefinitionsProperty).Add(columnDefinition);
			columnDefinition2.SetValue(ColumnDefinition.WidthProperty, new GridLengthTypeConverter().ConvertFromInvariantString("auto"));
			grid.GetValue(Grid.ColumnDefinitionsProperty).Add(columnDefinition2);
			columnDefinition3.SetValue(ColumnDefinition.WidthProperty, new GridLengthTypeConverter().ConvertFromInvariantString("*"));
			grid.GetValue(Grid.ColumnDefinitionsProperty).Add(columnDefinition3);
			linkButton.SetValue(Grid.ColumnProperty, 0);
			linkButton.SetValue(View.MarginProperty, new Thickness(0.0));
			linkButton.Clicked += this.btnBack_Clicked;
			linkButton.SetValue(View.HorizontalOptionsProperty, LayoutOptions.Start);
			dynamicResourceExtension3.Key = "NavigationBarButton";
			IMarkupExtension<DynamicResource> markupExtension3 = dynamicResourceExtension3;
			XamlServiceProvider xamlServiceProvider3 = new XamlServiceProvider();
			Type typeFromHandle5 = typeof(IProvideValueTarget);
			object[] array3 = new object[0 + 3];
			array3[0] = linkButton;
			array3[1] = grid;
			array3[2] = this;
			object obj3;
			xamlServiceProvider3.Add(typeFromHandle5, obj3 = new SimpleValueTargetProvider(array3, VisualElement.StyleProperty, nameScope));
			xamlServiceProvider3.Add(typeof(IReferenceProvider), obj3);
			Type typeFromHandle6 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver3 = new XmlNamespaceResolver();
			xmlNamespaceResolver3.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver3.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver3.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver3.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver3.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider3.Add(typeFromHandle6, new XamlTypeResolver(xmlNamespaceResolver3, typeof(Mode06Page).GetTypeInfo().Assembly));
			xamlServiceProvider3.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(45, 17)));
			DynamicResource dynamicResource3 = markupExtension3.ProvideValue(xamlServiceProvider3);
			linkButton.SetDynamicResource(VisualElement.StyleProperty, dynamicResource3.Key);
			translate.Text = "ios_Back";
			IMarkupExtension markupExtension4 = translate;
			XamlServiceProvider xamlServiceProvider4 = new XamlServiceProvider();
			Type typeFromHandle7 = typeof(IProvideValueTarget);
			object[] array4 = new object[0 + 3];
			array4[0] = linkButton;
			array4[1] = grid;
			array4[2] = this;
			object obj4;
			xamlServiceProvider4.Add(typeFromHandle7, obj4 = new SimpleValueTargetProvider(array4, Button.TextProperty, nameScope));
			xamlServiceProvider4.Add(typeof(IReferenceProvider), obj4);
			Type typeFromHandle8 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver4 = new XmlNamespaceResolver();
			xmlNamespaceResolver4.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver4.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver4.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver4.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver4.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider4.Add(typeFromHandle8, new XamlTypeResolver(xmlNamespaceResolver4, typeof(Mode06Page).GetTypeInfo().Assembly));
			xamlServiceProvider4.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(46, 17)));
			object obj5 = markupExtension4.ProvideValue(xamlServiceProvider4);
			linkButton.Text = obj5;
			grid.Children.Add(linkButton);
			nonScalableLabel.SetValue(Grid.ColumnProperty, 1);
			dynamicResourceExtension4.Key = "NavigationBarNonScalableLabel";
			IMarkupExtension<DynamicResource> markupExtension5 = dynamicResourceExtension4;
			XamlServiceProvider xamlServiceProvider5 = new XamlServiceProvider();
			Type typeFromHandle9 = typeof(IProvideValueTarget);
			object[] array5 = new object[0 + 3];
			array5[0] = nonScalableLabel;
			array5[1] = grid;
			array5[2] = this;
			object obj6;
			xamlServiceProvider5.Add(typeFromHandle9, obj6 = new SimpleValueTargetProvider(array5, VisualElement.StyleProperty, nameScope));
			xamlServiceProvider5.Add(typeof(IReferenceProvider), obj6);
			Type typeFromHandle10 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver5 = new XmlNamespaceResolver();
			xmlNamespaceResolver5.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver5.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver5.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver5.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver5.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider5.Add(typeFromHandle10, new XamlTypeResolver(xmlNamespaceResolver5, typeof(Mode06Page).GetTypeInfo().Assembly));
			xamlServiceProvider5.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(57, 17)));
			DynamicResource dynamicResource4 = markupExtension5.ProvideValue(xamlServiceProvider5);
			nonScalableLabel.SetDynamicResource(VisualElement.StyleProperty, dynamicResource4.Key);
			translate2.Text = "ios_Mode06Page_Title";
			IMarkupExtension markupExtension6 = translate2;
			XamlServiceProvider xamlServiceProvider6 = new XamlServiceProvider();
			Type typeFromHandle11 = typeof(IProvideValueTarget);
			object[] array6 = new object[0 + 3];
			array6[0] = nonScalableLabel;
			array6[1] = grid;
			array6[2] = this;
			object obj7;
			xamlServiceProvider6.Add(typeFromHandle11, obj7 = new SimpleValueTargetProvider(array6, Label.TextProperty, nameScope));
			xamlServiceProvider6.Add(typeof(IReferenceProvider), obj7);
			Type typeFromHandle12 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver6 = new XmlNamespaceResolver();
			xmlNamespaceResolver6.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver6.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver6.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver6.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver6.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider6.Add(typeFromHandle12, new XamlTypeResolver(xmlNamespaceResolver6, typeof(Mode06Page).GetTypeInfo().Assembly));
			xamlServiceProvider6.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(58, 17)));
			object obj8 = markupExtension6.ProvideValue(xamlServiceProvider6);
			nonScalableLabel.Text = obj8;
			grid.Children.Add(nonScalableLabel);
			linkButton2.SetValue(Grid.ColumnProperty, 2);
			linkButton2.Clicked += this.btnInfo_Clicked;
			linkButton2.SetValue(View.HorizontalOptionsProperty, LayoutOptions.End);
			dynamicResourceExtension5.Key = "InfoImageNavigationBarTextColor";
			IMarkupExtension<DynamicResource> markupExtension7 = dynamicResourceExtension5;
			XamlServiceProvider xamlServiceProvider7 = new XamlServiceProvider();
			Type typeFromHandle13 = typeof(IProvideValueTarget);
			object[] array7 = new object[0 + 3];
			array7[0] = linkButton2;
			array7[1] = grid;
			array7[2] = this;
			object obj9;
			xamlServiceProvider7.Add(typeFromHandle13, obj9 = new SimpleValueTargetProvider(array7, Button.ImageProperty, nameScope));
			xamlServiceProvider7.Add(typeof(IReferenceProvider), obj9);
			Type typeFromHandle14 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver7 = new XmlNamespaceResolver();
			xmlNamespaceResolver7.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver7.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver7.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver7.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver7.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider7.Add(typeFromHandle14, new XamlTypeResolver(xmlNamespaceResolver7, typeof(Mode06Page).GetTypeInfo().Assembly));
			xamlServiceProvider7.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(63, 17)));
			DynamicResource dynamicResource5 = markupExtension7.ProvideValue(xamlServiceProvider7);
			linkButton2.SetDynamicResource(Button.ImageProperty, dynamicResource5.Key);
			dynamicResourceExtension6.Key = "NavigationBarButton";
			IMarkupExtension<DynamicResource> markupExtension8 = dynamicResourceExtension6;
			XamlServiceProvider xamlServiceProvider8 = new XamlServiceProvider();
			Type typeFromHandle15 = typeof(IProvideValueTarget);
			object[] array8 = new object[0 + 3];
			array8[0] = linkButton2;
			array8[1] = grid;
			array8[2] = this;
			object obj10;
			xamlServiceProvider8.Add(typeFromHandle15, obj10 = new SimpleValueTargetProvider(array8, VisualElement.StyleProperty, nameScope));
			xamlServiceProvider8.Add(typeof(IReferenceProvider), obj10);
			Type typeFromHandle16 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver8 = new XmlNamespaceResolver();
			xmlNamespaceResolver8.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver8.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver8.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver8.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver8.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider8.Add(typeFromHandle16, new XamlTypeResolver(xmlNamespaceResolver8, typeof(Mode06Page).GetTypeInfo().Assembly));
			xamlServiceProvider8.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(64, 17)));
			DynamicResource dynamicResource6 = markupExtension8.ProvideValue(xamlServiceProvider8);
			linkButton2.SetDynamicResource(VisualElement.StyleProperty, dynamicResource6.Key);
			linkButton2.SetValue(View.VerticalOptionsProperty, LayoutOptions.Center);
			on3.Platform = new List<string>(1) { "iOS" };
			on3.Value = "0";
			onPlatform2.Platforms.Add(on3);
			on4.Platform = new List<string>(1) { "Android" };
			on4.Value = "0,0,5,0";
			onPlatform2.Platforms.Add(on4);
			linkButton2.SetValue(View.MarginProperty, onPlatform2);
			grid.Children.Add(linkButton2);
			this.SetValue(NavigationPage.TitleViewProperty, grid);
			onPlatform3.Android = new Thickness(5.0, 0.0, 5.0, 0.0);
			onPlatform3.WinPhone = new Thickness(0.0);
			onPlatform3.iOS = new Thickness(5.0, 0.0, 5.0, 0.0);
			grid2.SetValue(View.MarginProperty, onPlatform3);
			rowDefinition.SetValue(RowDefinition.HeightProperty, new GridLengthTypeConverter().ConvertFromInvariantString("auto"));
			grid2.GetValue(Grid.RowDefinitionsProperty).Add(rowDefinition);
			rowDefinition2.SetValue(RowDefinition.HeightProperty, new GridLengthTypeConverter().ConvertFromInvariantString("*"));
			grid2.GetValue(Grid.RowDefinitionsProperty).Add(rowDefinition2);
			rowDefinition3.SetValue(RowDefinition.HeightProperty, new GridLengthTypeConverter().ConvertFromInvariantString("auto"));
			grid2.GetValue(Grid.RowDefinitionsProperty).Add(rowDefinition3);
			rowDefinition4.SetValue(RowDefinition.HeightProperty, new GridLengthTypeConverter().ConvertFromInvariantString("auto"));
			grid2.GetValue(Grid.RowDefinitionsProperty).Add(rowDefinition4);
			rowDefinition5.SetValue(RowDefinition.HeightProperty, new GridLengthTypeConverter().ConvertFromInvariantString("auto"));
			grid2.GetValue(Grid.RowDefinitionsProperty).Add(rowDefinition5);
			columnDefinition4.SetValue(ColumnDefinition.WidthProperty, new GridLengthTypeConverter().ConvertFromInvariantString("*"));
			grid2.GetValue(Grid.ColumnDefinitionsProperty).Add(columnDefinition4);
			listView.SetValue(Grid.RowProperty, 1);
			listView.SetValue(VisualElement.BackgroundColorProperty, Color.Transparent);
			listView.SetValue(ListView.HasUnevenRowsProperty, true);
			listView.SetValue(View.HorizontalOptionsProperty, LayoutOptions.FillAndExpand);
			listView.ItemTapped += this.Handle_ItemTapped;
			BindingBase bindingBase = bindingExtension.ProvideValue(null);
			listView.SetBinding(ItemsView<Cell>.ItemsSourceProperty, bindingBase);
			IDataTemplate dataTemplate2 = dataTemplate;
			Mode06Page.<InitializeComponent>_anonXamlCDataTemplate_69 <InitializeComponent>_anonXamlCDataTemplate_ = new Mode06Page.<InitializeComponent>_anonXamlCDataTemplate_69();
			object[] array9 = new object[0 + 4];
			array9[0] = dataTemplate;
			array9[1] = listView;
			array9[2] = grid2;
			array9[3] = this;
			<InitializeComponent>_anonXamlCDataTemplate_.parentValues = array9;
			<InitializeComponent>_anonXamlCDataTemplate_.root = this;
			dataTemplate2.LoadTemplate = new Func<object>(<InitializeComponent>_anonXamlCDataTemplate_.LoadDataTemplate);
			listView.SetValue(ItemsView<Cell>.ItemTemplateProperty, dataTemplate);
			grid2.Children.Add(listView);
			label.SetValue(Grid.RowProperty, 1);
			label.SetValue(Label.FontAttributesProperty, new FontAttributesConverter().ConvertFromInvariantString("Bold"));
			label.SetValue(View.HorizontalOptionsProperty, LayoutOptions.Center);
			label.SetValue(Label.HorizontalTextAlignmentProperty, new TextAlignmentConverter().ConvertFromInvariantString("Center"));
			label.SetValue(VisualElement.IsVisibleProperty, new VisualElement.VisibilityConverter().ConvertFromInvariantString("False"));
			dynamicResourceExtension7.Key = "RedTextColor";
			IMarkupExtension<DynamicResource> markupExtension9 = dynamicResourceExtension7;
			XamlServiceProvider xamlServiceProvider9 = new XamlServiceProvider();
			Type typeFromHandle17 = typeof(IProvideValueTarget);
			object[] array10 = new object[0 + 3];
			array10[0] = label;
			array10[1] = grid2;
			array10[2] = this;
			object obj11;
			xamlServiceProvider9.Add(typeFromHandle17, obj11 = new SimpleValueTargetProvider(array10, Label.TextColorProperty, nameScope));
			xamlServiceProvider9.Add(typeof(IReferenceProvider), obj11);
			Type typeFromHandle18 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver9 = new XmlNamespaceResolver();
			xmlNamespaceResolver9.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver9.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver9.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver9.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver9.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider9.Add(typeFromHandle18, new XamlTypeResolver(xmlNamespaceResolver9, typeof(Mode06Page).GetTypeInfo().Assembly));
			xamlServiceProvider9.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(306, 17)));
			DynamicResource dynamicResource7 = markupExtension9.ProvideValue(xamlServiceProvider9);
			label.SetDynamicResource(Label.TextColorProperty, dynamicResource7.Key);
			label.SetValue(View.VerticalOptionsProperty, LayoutOptions.Center);
			label.SetValue(Label.VerticalTextAlignmentProperty, new TextAlignmentConverter().ConvertFromInvariantString("Center"));
			grid2.Children.Add(label);
			button.SetValue(Grid.RowProperty, 2);
			button.Clicked += this.btnRefresh_Clicked;
			button.SetValue(View.HorizontalOptionsProperty, LayoutOptions.Center);
			translate3.Text = "Mode06Page_btnRefresh.Content";
			IMarkupExtension markupExtension10 = translate3;
			XamlServiceProvider xamlServiceProvider10 = new XamlServiceProvider();
			Type typeFromHandle19 = typeof(IProvideValueTarget);
			object[] array11 = new object[0 + 3];
			array11[0] = button;
			array11[1] = grid2;
			array11[2] = this;
			object obj12;
			xamlServiceProvider10.Add(typeFromHandle19, obj12 = new SimpleValueTargetProvider(array11, Button.TextProperty, nameScope));
			xamlServiceProvider10.Add(typeof(IReferenceProvider), obj12);
			Type typeFromHandle20 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver10 = new XmlNamespaceResolver();
			xmlNamespaceResolver10.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver10.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver10.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver10.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver10.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider10.Add(typeFromHandle20, new XamlTypeResolver(xmlNamespaceResolver10, typeof(Mode06Page).GetTypeInfo().Assembly));
			xamlServiceProvider10.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(314, 17)));
			object obj13 = markupExtension10.ProvideValue(xamlServiceProvider10);
			button.Text = obj13;
			grid2.Children.Add(button);
			progressBar.SetValue(Grid.RowProperty, 3);
			progressBar.SetValue(VisualElement.IsVisibleProperty, new VisualElement.VisibilityConverter().ConvertFromInvariantString("false"));
			progressBar.SetValue(ProgressBar.ProgressProperty, 0.0);
			grid2.Children.Add(progressBar);
			complexAdView.SetValue(Grid.RowProperty, 4);
			complexAdView.SetValue(View.MarginProperty, new Thickness(-5.0, 0.0, -5.0, -5.0));
			complexAdView.SetValue(VisualElement.HeightRequestProperty, 55.0);
			complexAdView.SetValue(VisualElement.IsVisibleProperty, new VisualElement.VisibilityConverter().ConvertFromInvariantString("true"));
			complexAdView.SetValue(View.VerticalOptionsProperty, LayoutOptions.End);
			grid2.Children.Add(complexAdView);
			this.SetValue(ContentPage.ContentProperty, grid2);
		}

		// Token: 0x060004B7 RID: 1207 RVA: 0x00046C9E File Offset: 0x00044E9E
		[CompilerGenerated]
		private void <ReadMode06Data>b__8_0(double x)
		{
			this.progressBar.Progress = x;
		}

		// Token: 0x060004B8 RID: 1208 RVA: 0x00046CAC File Offset: 0x00044EAC
		private void __InitComponentRuntime()
		{
			Extensions.LoadFromXaml<Mode06Page>(this, typeof(Mode06Page));
			this.lbNotSupported = NameScopeExtensions.FindByName<Label>(this, "lbNotSupported");
			this.btnRefresh = NameScopeExtensions.FindByName<Button>(this, "btnRefresh");
			this.progressBar = NameScopeExtensions.FindByName<ProgressBar>(this, "progressBar");
			this.ad = NameScopeExtensions.FindByName<ComplexAdView>(this, "ad");
		}

		// Token: 0x0400037E RID: 894
		private bool full_search_completed;

		// Token: 0x0400037F RID: 895
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private Label lbNotSupported;

		// Token: 0x04000380 RID: 896
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private Button btnRefresh;

		// Token: 0x04000381 RID: 897
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private ProgressBar progressBar;

		// Token: 0x04000382 RID: 898
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private ComplexAdView ad;

		// Token: 0x020000F2 RID: 242
		[CompilerGenerated]
		[Serializable]
		private sealed class <>c
		{
			// Token: 0x060004B9 RID: 1209 RVA: 0x00046D0E File Offset: 0x00044F0E
			// Note: this type is marked as 'beforefieldinit'.
			static <>c()
			{
			}

			// Token: 0x060004BA RID: 1210 RVA: 0x00002050 File Offset: 0x00000250
			public <>c()
			{
			}

			// Token: 0x060004BB RID: 1211 RVA: 0x00046D1A File Offset: 0x00044F1A
			internal string <ReadMode06Data>b__8_1(Mode6Test x)
			{
				return x.Cmd;
			}

			// Token: 0x04000383 RID: 899
			public static readonly Mode06Page.<>c <>9 = new Mode06Page.<>c();

			// Token: 0x04000384 RID: 900
			public static Func<Mode6Test, string> <>9__8_1;
		}

		// Token: 0x020000F3 RID: 243
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <ReadMode06Data>d__8 : IAsyncStateMachine
		{
			// Token: 0x060004BC RID: 1212 RVA: 0x00046D24 File Offset: 0x00044F24
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				Mode06Page mode06Page = this;
				try
				{
					TaskAwaiter taskAwaiter;
					TaskAwaiter taskAwaiter2;
					if (num != 0)
					{
						if (num != 1)
						{
							mode06Page.btnRefresh.IsEnabled = false;
							mode06Page.lbNotSupported.Text = "";
							mode06Page.lbNotSupported.IsVisible = false;
							old_caption = mode06Page.btnRefresh.Text;
							mode06Page.btnRefresh.Text = Translate.GetString("Mode06Page_btnRefresh_Reading");
							mode06Page.progressBar.IsVisible = true;
							Progress<double> progress = new Progress<double>(delegate(double x)
							{
								mode06Page.progressBar.Progress = x;
							});
							if (!mode06Page.full_search_completed)
							{
								taskAwaiter = App.OBDReader.ReadMode06(progress).GetAwaiter();
								if (!taskAwaiter.IsCompleted)
								{
									num2 = 0;
									taskAwaiter2 = taskAwaiter;
									this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, Mode06Page.<ReadMode06Data>d__8>(ref taskAwaiter, ref this);
									return;
								}
								goto IL_00E8;
							}
							else
							{
								string[] array = App.OBDReader.CurrentCarData.Mode06TestCollection.Select((Mode6Test x) => x.Cmd).Distinct<string>().ToArray<string>();
								taskAwaiter = App.OBDReader.ReadMode06(progress, array).GetAwaiter();
								if (!taskAwaiter.IsCompleted)
								{
									num2 = 1;
									taskAwaiter2 = taskAwaiter;
									this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, Mode06Page.<ReadMode06Data>d__8>(ref taskAwaiter, ref this);
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
						goto IL_019C;
					}
					taskAwaiter = taskAwaiter2;
					taskAwaiter2 = default(TaskAwaiter);
					num2 = -1;
					IL_00E8:
					taskAwaiter.GetResult();
					mode06Page.full_search_completed = true;
					IL_019C:
					if (App.OBDReader.CurrentCarData.Mode06TestCollection != null && App.OBDReader.CurrentCarData.Mode06TestCollection.Count == 0)
					{
						string text = Translate.GetString("Mode06_NotSupported_OrBadELM");
						if (App.OBDReader.CurrentELMFormat == ELMFormat.KWP)
						{
							text = Translate.GetString("Mode06_NotSupported");
						}
						else if (SharedSettings.Current.FuelType == FuelTypes.Diesel && (SharedSettings.Current.SelectedBrand == "Hyundai" || SharedSettings.Current.SelectedBrand == "Kia" || SharedSettings.Current.SelectedBrand == "Genesis"))
						{
							text = Translate.GetString("Mode06_NotSupported");
						}
						mode06Page.lbNotSupported.Text = text;
						mode06Page.lbNotSupported.IsVisible = true;
					}
					RequestProducerStatic.UpdateOBDReaderRequests();
					mode06Page.progressBar.IsVisible = false;
					mode06Page.btnRefresh.IsEnabled = true;
					mode06Page.btnRefresh.Text = old_caption;
				}
				catch (Exception ex)
				{
					num2 = -2;
					old_caption = null;
					this.<>t__builder.SetException(ex);
					return;
				}
				num2 = -2;
				old_caption = null;
				this.<>t__builder.SetResult();
			}

			// Token: 0x060004BD RID: 1213 RVA: 0x0004701C File Offset: 0x0004521C
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x04000385 RID: 901
			public int <>1__state;

			// Token: 0x04000386 RID: 902
			public AsyncTaskMethodBuilder <>t__builder;

			// Token: 0x04000387 RID: 903
			public Mode06Page <>4__this;

			// Token: 0x04000388 RID: 904
			private string <old_caption>5__2;

			// Token: 0x04000389 RID: 905
			private TaskAwaiter <>u__1;
		}

		// Token: 0x020000F4 RID: 244
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <btnBack_Clicked>d__9 : IAsyncStateMachine
		{
			// Token: 0x060004BE RID: 1214 RVA: 0x0004702C File Offset: 0x0004522C
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				Mode06Page mode06Page = this;
				try
				{
					TaskAwaiter<Page> taskAwaiter;
					if (num != 0)
					{
						taskAwaiter = mode06Page.Navigation.PopAsync(true).GetAwaiter();
						if (!taskAwaiter.IsCompleted)
						{
							num2 = 0;
							TaskAwaiter<Page> taskAwaiter2 = taskAwaiter;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<Page>, Mode06Page.<btnBack_Clicked>d__9>(ref taskAwaiter, ref this);
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

			// Token: 0x060004BF RID: 1215 RVA: 0x000470E8 File Offset: 0x000452E8
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x0400038A RID: 906
			public int <>1__state;

			// Token: 0x0400038B RID: 907
			public AsyncVoidMethodBuilder <>t__builder;

			// Token: 0x0400038C RID: 908
			public Mode06Page <>4__this;

			// Token: 0x0400038D RID: 909
			private TaskAwaiter<Page> <>u__1;
		}

		// Token: 0x020000F5 RID: 245
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <btnCreateCustomPid_Clicked>d__11 : IAsyncStateMachine
		{
			// Token: 0x060004C0 RID: 1216 RVA: 0x000470F8 File Offset: 0x000452F8
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				Mode06Page mode06Page = this;
				try
				{
					TaskAwaiter taskAwaiter;
					if (num != 0)
					{
						if (!App.OBDSimulator.IsActive)
						{
							CustomPID customPID = Mode6Test.CreateCustomPidFromMode6Test((sender as MenuItem).BindingContext as Mode6Test);
							customPID.IsFormulaHidden = true;
							CustomPIDViewModel.CurrentCustom.PidCollection.Add(customPID);
							CustomPIDViewModel.CurrentCustom.Save();
							LiveDataPIDModel.UpdatePIDCollection(App.OBDReader);
						}
						taskAwaiter = mode06Page.DisplayAlert(Translate.GetString("Mode06Page_SensorCreated_Title"), Translate.GetString("Mode06Page_SensorCreated_Text"), "OK").GetAwaiter();
						if (!taskAwaiter.IsCompleted)
						{
							num2 = 0;
							TaskAwaiter taskAwaiter2 = taskAwaiter;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, Mode06Page.<btnCreateCustomPid_Clicked>d__11>(ref taskAwaiter, ref this);
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

			// Token: 0x060004C1 RID: 1217 RVA: 0x0004721C File Offset: 0x0004541C
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x0400038E RID: 910
			public int <>1__state;

			// Token: 0x0400038F RID: 911
			public AsyncVoidMethodBuilder <>t__builder;

			// Token: 0x04000390 RID: 912
			public object sender;

			// Token: 0x04000391 RID: 913
			public Mode06Page <>4__this;

			// Token: 0x04000392 RID: 914
			private TaskAwaiter <>u__1;
		}

		// Token: 0x020000F6 RID: 246
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <btnRefresh_Clicked>d__7 : IAsyncStateMachine
		{
			// Token: 0x060004C2 RID: 1218 RVA: 0x0004722C File Offset: 0x0004542C
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				Mode06Page mode06Page = this;
				try
				{
					TaskAwaiter taskAwaiter;
					if (num != 0)
					{
						if (App.OBDReader.CurrentStatus != OBDDataReaderStatus.ConnectedToECU || App.OBDSimulator.IsActive)
						{
							App.OBDReader.CurrentCarData.Mode06TestCollection.Clear();
							Mode6Test mode6Test = new Mode6Test
							{
								MaxValue = 100.0,
								MinValue = 0.0,
								MonitorName = "Test monitor Id 01",
								TestId = "Range test",
								TestValue = 150.0,
								Units = UnitsHelper.Units.None
							};
							App.OBDReader.CurrentCarData.Mode06TestCollection.Add(mode6Test);
							mode6Test = new Mode6Test
							{
								MaxValue = 20.0,
								MinValue = 0.0,
								MonitorName = "Test monitor Id 02",
								TestId = "Voltage test",
								TestValue = 15.0,
								Units = UnitsHelper.Units.volts
							};
							mode6Test.CheckTestPassed();
							App.OBDReader.CurrentCarData.Mode06TestCollection.Add(mode6Test);
							goto IL_016F;
						}
						taskAwaiter = mode06Page.ReadMode06Data().GetAwaiter();
						if (!taskAwaiter.IsCompleted)
						{
							num2 = 0;
							TaskAwaiter taskAwaiter2 = taskAwaiter;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, Mode06Page.<btnRefresh_Clicked>d__7>(ref taskAwaiter, ref this);
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
					IL_016F:;
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

			// Token: 0x060004C3 RID: 1219 RVA: 0x000473F4 File Offset: 0x000455F4
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x04000393 RID: 915
			public int <>1__state;

			// Token: 0x04000394 RID: 916
			public AsyncVoidMethodBuilder <>t__builder;

			// Token: 0x04000395 RID: 917
			public Mode06Page <>4__this;

			// Token: 0x04000396 RID: 918
			private TaskAwaiter <>u__1;
		}

		// Token: 0x020000F7 RID: 247
		[CompilerGenerated]
		private sealed class <InitializeComponent>_anonXamlCDataTemplate_69
		{
			// Token: 0x060004C4 RID: 1220 RVA: 0x00047404 File Offset: 0x00045604
			public <InitializeComponent>_anonXamlCDataTemplate_69()
			{
			}

			// Token: 0x060004C5 RID: 1221 RVA: 0x00047418 File Offset: 0x00045618
			internal object LoadDataTemplate()
			{
				BindingExtension bindingExtension;
				VisualDiagnostics.RegisterSourceInfo(bindingExtension = new BindingExtension(), new Uri("Pages\\Mode06Page.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 118, 37);
				Translate translate;
				VisualDiagnostics.RegisterSourceInfo(translate = new Translate(), new Uri("Pages\\Mode06Page.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 120, 37);
				MenuItem menuItem;
				VisualDiagnostics.RegisterSourceInfo(menuItem = new MenuItem(), new Uri("Pages\\Mode06Page.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 116, 34);
				RowDefinition rowDefinition;
				VisualDiagnostics.RegisterSourceInfo(rowDefinition = new RowDefinition(), new Uri("Pages\\Mode06Page.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 124, 38);
				RowDefinition rowDefinition2;
				VisualDiagnostics.RegisterSourceInfo(rowDefinition2 = new RowDefinition(), new Uri("Pages\\Mode06Page.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 125, 38);
				RowDefinition rowDefinition3;
				VisualDiagnostics.RegisterSourceInfo(rowDefinition3 = new RowDefinition(), new Uri("Pages\\Mode06Page.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 126, 38);
				RowDefinition rowDefinition4;
				VisualDiagnostics.RegisterSourceInfo(rowDefinition4 = new RowDefinition(), new Uri("Pages\\Mode06Page.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 127, 38);
				ColumnDefinition columnDefinition;
				VisualDiagnostics.RegisterSourceInfo(columnDefinition = new ColumnDefinition(), new Uri("Pages\\Mode06Page.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 131, 38);
				ColumnDefinition columnDefinition2;
				VisualDiagnostics.RegisterSourceInfo(columnDefinition2 = new ColumnDefinition(), new Uri("Pages\\Mode06Page.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 132, 38);
				ColumnDefinition columnDefinition3;
				VisualDiagnostics.RegisterSourceInfo(columnDefinition3 = new ColumnDefinition(), new Uri("Pages\\Mode06Page.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 133, 38);
				ColumnDefinition columnDefinition4;
				VisualDiagnostics.RegisterSourceInfo(columnDefinition4 = new ColumnDefinition(), new Uri("Pages\\Mode06Page.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 134, 38);
				DynamicResourceExtension dynamicResourceExtension;
				VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension = new DynamicResourceExtension(), new Uri("Pages\\Mode06Page.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 144, 41);
				BindingExtension bindingExtension2;
				VisualDiagnostics.RegisterSourceInfo(bindingExtension2 = new BindingExtension(), new Uri("Pages\\Mode06Page.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 147, 41);
				Label label;
				VisualDiagnostics.RegisterSourceInfo(label = new Label(), new Uri("Pages\\Mode06Page.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 142, 38);
				DynamicResourceExtension dynamicResourceExtension2;
				VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension2 = new DynamicResourceExtension(), new Uri("Pages\\Mode06Page.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 149, 41);
				BindingExtension bindingExtension3;
				VisualDiagnostics.RegisterSourceInfo(bindingExtension3 = new BindingExtension(), new Uri("Pages\\Mode06Page.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 152, 41);
				Label label2;
				VisualDiagnostics.RegisterSourceInfo(label2 = new Label(), new Uri("Pages\\Mode06Page.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 148, 38);
				StackLayout stackLayout;
				VisualDiagnostics.RegisterSourceInfo(stackLayout = new StackLayout(), new Uri("Pages\\Mode06Page.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 137, 34);
				StaticResourceExtension staticResourceExtension;
				VisualDiagnostics.RegisterSourceInfo(staticResourceExtension = new StaticResourceExtension(), new Uri("Pages\\Mode06Page.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 159, 37);
				Translate translate2;
				VisualDiagnostics.RegisterSourceInfo(translate2 = new Translate(), new Uri("Pages\\Mode06Page.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 160, 37);
				Label label3;
				VisualDiagnostics.RegisterSourceInfo(label3 = new Label(), new Uri("Pages\\Mode06Page.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 155, 34);
				StaticResourceExtension staticResourceExtension2;
				VisualDiagnostics.RegisterSourceInfo(staticResourceExtension2 = new StaticResourceExtension(), new Uri("Pages\\Mode06Page.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 165, 37);
				Translate translate3;
				VisualDiagnostics.RegisterSourceInfo(translate3 = new Translate(), new Uri("Pages\\Mode06Page.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 166, 37);
				Label label4;
				VisualDiagnostics.RegisterSourceInfo(label4 = new Label(), new Uri("Pages\\Mode06Page.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 161, 34);
				StaticResourceExtension staticResourceExtension3;
				VisualDiagnostics.RegisterSourceInfo(staticResourceExtension3 = new StaticResourceExtension(), new Uri("Pages\\Mode06Page.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 171, 37);
				Translate translate4;
				VisualDiagnostics.RegisterSourceInfo(translate4 = new Translate(), new Uri("Pages\\Mode06Page.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 172, 37);
				Label label5;
				VisualDiagnostics.RegisterSourceInfo(label5 = new Label(), new Uri("Pages\\Mode06Page.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 167, 34);
				StaticResourceExtension staticResourceExtension4;
				VisualDiagnostics.RegisterSourceInfo(staticResourceExtension4 = new StaticResourceExtension(), new Uri("Pages\\Mode06Page.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 177, 37);
				Translate translate5;
				VisualDiagnostics.RegisterSourceInfo(translate5 = new Translate(), new Uri("Pages\\Mode06Page.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 178, 37);
				Label label6;
				VisualDiagnostics.RegisterSourceInfo(label6 = new Label(), new Uri("Pages\\Mode06Page.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 173, 34);
				BindingExtension bindingExtension4;
				VisualDiagnostics.RegisterSourceInfo(bindingExtension4 = new BindingExtension(), new Uri("Pages\\Mode06Page.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 185, 37);
				BindingExtension bindingExtension5;
				VisualDiagnostics.RegisterSourceInfo(bindingExtension5 = new BindingExtension(), new Uri("Pages\\Mode06Page.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 190, 41);
				StaticResourceExtension staticResourceExtension5;
				VisualDiagnostics.RegisterSourceInfo(staticResourceExtension5 = new StaticResourceExtension(), new Uri("Pages\\Mode06Page.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 195, 45);
				BindingExtension bindingExtension6;
				VisualDiagnostics.RegisterSourceInfo(bindingExtension6 = new BindingExtension(), new Uri("Pages\\Mode06Page.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 196, 45);
				Label label7;
				VisualDiagnostics.RegisterSourceInfo(label7 = new Label(), new Uri("Pages\\Mode06Page.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 193, 42);
				StaticResourceExtension staticResourceExtension6;
				VisualDiagnostics.RegisterSourceInfo(staticResourceExtension6 = new StaticResourceExtension(), new Uri("Pages\\Mode06Page.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 200, 45);
				BindingExtension bindingExtension7;
				VisualDiagnostics.RegisterSourceInfo(bindingExtension7 = new BindingExtension(), new Uri("Pages\\Mode06Page.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 201, 45);
				Label label8;
				VisualDiagnostics.RegisterSourceInfo(label8 = new Label(), new Uri("Pages\\Mode06Page.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 198, 42);
				StackLayout stackLayout2;
				VisualDiagnostics.RegisterSourceInfo(stackLayout2 = new StackLayout(), new Uri("Pages\\Mode06Page.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 188, 38);
				BindingExtension bindingExtension8;
				VisualDiagnostics.RegisterSourceInfo(bindingExtension8 = new BindingExtension(), new Uri("Pages\\Mode06Page.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 206, 41);
				StaticResourceExtension staticResourceExtension7;
				VisualDiagnostics.RegisterSourceInfo(staticResourceExtension7 = new StaticResourceExtension(), new Uri("Pages\\Mode06Page.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 210, 45);
				BindingExtension bindingExtension9;
				VisualDiagnostics.RegisterSourceInfo(bindingExtension9 = new BindingExtension(), new Uri("Pages\\Mode06Page.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 211, 45);
				Label label9;
				VisualDiagnostics.RegisterSourceInfo(label9 = new Label(), new Uri("Pages\\Mode06Page.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 209, 42);
				StaticResourceExtension staticResourceExtension8;
				VisualDiagnostics.RegisterSourceInfo(staticResourceExtension8 = new StaticResourceExtension(), new Uri("Pages\\Mode06Page.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 214, 45);
				BindingExtension bindingExtension10;
				VisualDiagnostics.RegisterSourceInfo(bindingExtension10 = new BindingExtension(), new Uri("Pages\\Mode06Page.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 215, 45);
				Label label10;
				VisualDiagnostics.RegisterSourceInfo(label10 = new Label(), new Uri("Pages\\Mode06Page.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 213, 42);
				StackLayout stackLayout3;
				VisualDiagnostics.RegisterSourceInfo(stackLayout3 = new StackLayout(), new Uri("Pages\\Mode06Page.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 204, 38);
				Grid grid;
				VisualDiagnostics.RegisterSourceInfo(grid = new Grid(), new Uri("Pages\\Mode06Page.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 180, 34);
				BindingExtension bindingExtension11;
				VisualDiagnostics.RegisterSourceInfo(bindingExtension11 = new BindingExtension(), new Uri("Pages\\Mode06Page.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 227, 37);
				StaticResourceExtension staticResourceExtension9;
				VisualDiagnostics.RegisterSourceInfo(staticResourceExtension9 = new StaticResourceExtension(), new Uri("Pages\\Mode06Page.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 233, 48);
				BindingExtension bindingExtension12;
				VisualDiagnostics.RegisterSourceInfo(bindingExtension12 = new BindingExtension(), new Uri("Pages\\Mode06Page.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 233, 84);
				Label label11;
				VisualDiagnostics.RegisterSourceInfo(label11 = new Label(), new Uri("Pages\\Mode06Page.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 233, 42);
				StaticResourceExtension staticResourceExtension10;
				VisualDiagnostics.RegisterSourceInfo(staticResourceExtension10 = new StaticResourceExtension(), new Uri("Pages\\Mode06Page.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 234, 48);
				BindingExtension bindingExtension13;
				VisualDiagnostics.RegisterSourceInfo(bindingExtension13 = new BindingExtension(), new Uri("Pages\\Mode06Page.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 234, 84);
				Label label12;
				VisualDiagnostics.RegisterSourceInfo(label12 = new Label(), new Uri("Pages\\Mode06Page.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 234, 42);
				StackLayout stackLayout4;
				VisualDiagnostics.RegisterSourceInfo(stackLayout4 = new StackLayout(), new Uri("Pages\\Mode06Page.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 229, 38);
				Grid grid2;
				VisualDiagnostics.RegisterSourceInfo(grid2 = new Grid(), new Uri("Pages\\Mode06Page.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 222, 34);
				StaticResourceExtension staticResourceExtension11;
				VisualDiagnostics.RegisterSourceInfo(staticResourceExtension11 = new StaticResourceExtension(), new Uri("Pages\\Mode06Page.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 245, 44);
				BindingExtension bindingExtension14;
				VisualDiagnostics.RegisterSourceInfo(bindingExtension14 = new BindingExtension(), new Uri("Pages\\Mode06Page.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 245, 80);
				Label label13;
				VisualDiagnostics.RegisterSourceInfo(label13 = new Label(), new Uri("Pages\\Mode06Page.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 245, 38);
				StaticResourceExtension staticResourceExtension12;
				VisualDiagnostics.RegisterSourceInfo(staticResourceExtension12 = new StaticResourceExtension(), new Uri("Pages\\Mode06Page.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 246, 44);
				BindingExtension bindingExtension15;
				VisualDiagnostics.RegisterSourceInfo(bindingExtension15 = new BindingExtension(), new Uri("Pages\\Mode06Page.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 246, 80);
				Label label14;
				VisualDiagnostics.RegisterSourceInfo(label14 = new Label(), new Uri("Pages\\Mode06Page.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 246, 38);
				StackLayout stackLayout5;
				VisualDiagnostics.RegisterSourceInfo(stackLayout5 = new StackLayout(), new Uri("Pages\\Mode06Page.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 239, 34);
				StaticResourceExtension staticResourceExtension13;
				VisualDiagnostics.RegisterSourceInfo(staticResourceExtension13 = new StaticResourceExtension(), new Uri("Pages\\Mode06Page.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 256, 44);
				BindingExtension bindingExtension16;
				VisualDiagnostics.RegisterSourceInfo(bindingExtension16 = new BindingExtension(), new Uri("Pages\\Mode06Page.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 256, 80);
				Label label15;
				VisualDiagnostics.RegisterSourceInfo(label15 = new Label(), new Uri("Pages\\Mode06Page.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 256, 38);
				StaticResourceExtension staticResourceExtension14;
				VisualDiagnostics.RegisterSourceInfo(staticResourceExtension14 = new StaticResourceExtension(), new Uri("Pages\\Mode06Page.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 257, 44);
				BindingExtension bindingExtension17;
				VisualDiagnostics.RegisterSourceInfo(bindingExtension17 = new BindingExtension(), new Uri("Pages\\Mode06Page.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 257, 80);
				Label label16;
				VisualDiagnostics.RegisterSourceInfo(label16 = new Label(), new Uri("Pages\\Mode06Page.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 257, 38);
				StackLayout stackLayout6;
				VisualDiagnostics.RegisterSourceInfo(stackLayout6 = new StackLayout(), new Uri("Pages\\Mode06Page.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 250, 34);
				BindingExtension bindingExtension18;
				VisualDiagnostics.RegisterSourceInfo(bindingExtension18 = new BindingExtension(), new Uri("Pages\\Mode06Page.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 264, 37);
				BindingExtension bindingExtension19;
				VisualDiagnostics.RegisterSourceInfo(bindingExtension19 = new BindingExtension(), new Uri("Pages\\Mode06Page.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 268, 41);
				StaticResourceExtension staticResourceExtension15;
				VisualDiagnostics.RegisterSourceInfo(staticResourceExtension15 = new StaticResourceExtension(), new Uri("Pages\\Mode06Page.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 269, 41);
				Translate translate6;
				VisualDiagnostics.RegisterSourceInfo(translate6 = new Translate(), new Uri("Pages\\Mode06Page.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 270, 41);
				Label label17;
				VisualDiagnostics.RegisterSourceInfo(label17 = new Label(), new Uri("Pages\\Mode06Page.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 266, 38);
				BindingExtension bindingExtension20;
				VisualDiagnostics.RegisterSourceInfo(bindingExtension20 = new BindingExtension(), new Uri("Pages\\Mode06Page.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 274, 41);
				StaticResourceExtension staticResourceExtension16;
				VisualDiagnostics.RegisterSourceInfo(staticResourceExtension16 = new StaticResourceExtension(), new Uri("Pages\\Mode06Page.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 275, 41);
				Translate translate7;
				VisualDiagnostics.RegisterSourceInfo(translate7 = new Translate(), new Uri("Pages\\Mode06Page.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 276, 41);
				Label label18;
				VisualDiagnostics.RegisterSourceInfo(label18 = new Label(), new Uri("Pages\\Mode06Page.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 272, 38);
				Grid grid3;
				VisualDiagnostics.RegisterSourceInfo(grid3 = new Grid(), new Uri("Pages\\Mode06Page.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 259, 34);
				BindingExtension bindingExtension21;
				VisualDiagnostics.RegisterSourceInfo(bindingExtension21 = new BindingExtension(), new Uri("Pages\\Mode06Page.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 285, 37);
				StaticResourceExtension staticResourceExtension17;
				VisualDiagnostics.RegisterSourceInfo(staticResourceExtension17 = new StaticResourceExtension(), new Uri("Pages\\Mode06Page.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 289, 41);
				Translate translate8;
				VisualDiagnostics.RegisterSourceInfo(translate8 = new Translate(), new Uri("Pages\\Mode06Page.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 290, 41);
				Label label19;
				VisualDiagnostics.RegisterSourceInfo(label19 = new Label(), new Uri("Pages\\Mode06Page.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 287, 38);
				Grid grid4;
				VisualDiagnostics.RegisterSourceInfo(grid4 = new Grid(), new Uri("Pages\\Mode06Page.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 280, 34);
				Grid grid5;
				VisualDiagnostics.RegisterSourceInfo(grid5 = new Grid(), new Uri("Pages\\Mode06Page.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 122, 30);
				ViewCell viewCell;
				VisualDiagnostics.RegisterSourceInfo(viewCell = new ViewCell(), new Uri("Pages\\Mode06Page.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 113, 26);
				NameScope nameScope = new NameScope();
				NameScope.SetNameScope(viewCell, nameScope);
				nameScope.RegisterName("ProValuePanel", grid);
				if (grid.StyleId == null)
				{
					grid.StyleId = "ProValuePanel";
				}
				nameScope.RegisterName("FreeValuePanel", grid2);
				if (grid2.StyleId == null)
				{
					grid2.StyleId = "FreeValuePanel";
				}
				nameScope.RegisterName("ProResultPanel", grid3);
				if (grid3.StyleId == null)
				{
					grid3.StyleId = "ProResultPanel";
				}
				nameScope.RegisterName("FreeDummyPanel", grid4);
				if (grid4.StyleId == null)
				{
					grid4.StyleId = "FreeDummyPanel";
				}
				menuItem.Clicked += this.root.btnCreateCustomPid_Clicked;
				bindingExtension.Path = ".";
				BindingBase bindingBase = bindingExtension.ProvideValue(null);
				menuItem.SetBinding(MenuItem.CommandParameterProperty, bindingBase);
				menuItem.SetValue(MenuItem.IsDestructiveProperty, false);
				translate.Text = "menuCreateCustomPid_Create.Text";
				IMarkupExtension markupExtension = translate;
				XamlServiceProvider xamlServiceProvider = new XamlServiceProvider();
				Type typeFromHandle = typeof(IProvideValueTarget);
				int num;
				object[] array = new object[(num = this.parentValues.Length) + 2];
				Array.Copy(this.parentValues, 0, array, 2, num);
				object[] array2 = array;
				array2[0] = menuItem;
				array2[1] = viewCell;
				object obj;
				xamlServiceProvider.Add(typeFromHandle, obj = new SimpleValueTargetProvider(array2, MenuItem.TextProperty, nameScope));
				xamlServiceProvider.Add(typeof(IReferenceProvider), obj);
				Type typeFromHandle2 = typeof(IXamlTypeResolver);
				XmlNamespaceResolver xmlNamespaceResolver = new XmlNamespaceResolver();
				xmlNamespaceResolver.Add("", "http://xamarin.com/schemas/2014/forms");
				xmlNamespaceResolver.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
				xmlNamespaceResolver.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
				xmlNamespaceResolver.Add("local", "clr-namespace:CarScannerXamarinForms");
				xmlNamespaceResolver.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
				xamlServiceProvider.Add(typeFromHandle2, new XamlTypeResolver(xmlNamespaceResolver, typeof(Mode06Page.<InitializeComponent>_anonXamlCDataTemplate_69).GetTypeInfo().Assembly));
				xamlServiceProvider.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(120, 37)));
				object obj2 = markupExtension.ProvideValue(xamlServiceProvider);
				menuItem.Text = obj2;
				viewCell.ContextActions.Add(menuItem);
				grid5.SetValue(View.MarginProperty, new Thickness(0.0, 5.0));
				grid5.SetValue(View.HorizontalOptionsProperty, LayoutOptions.FillAndExpand);
				rowDefinition.SetValue(RowDefinition.HeightProperty, new GridLengthTypeConverter().ConvertFromInvariantString("auto"));
				grid5.GetValue(Grid.RowDefinitionsProperty).Add(rowDefinition);
				rowDefinition2.SetValue(RowDefinition.HeightProperty, new GridLengthTypeConverter().ConvertFromInvariantString("auto"));
				grid5.GetValue(Grid.RowDefinitionsProperty).Add(rowDefinition2);
				rowDefinition3.SetValue(RowDefinition.HeightProperty, new GridLengthTypeConverter().ConvertFromInvariantString("auto"));
				grid5.GetValue(Grid.RowDefinitionsProperty).Add(rowDefinition3);
				rowDefinition4.SetValue(RowDefinition.HeightProperty, new GridLengthTypeConverter().ConvertFromInvariantString("auto"));
				grid5.GetValue(Grid.RowDefinitionsProperty).Add(rowDefinition4);
				columnDefinition.SetValue(ColumnDefinition.WidthProperty, new GridLengthTypeConverter().ConvertFromInvariantString("*"));
				grid5.GetValue(Grid.ColumnDefinitionsProperty).Add(columnDefinition);
				columnDefinition2.SetValue(ColumnDefinition.WidthProperty, new GridLengthTypeConverter().ConvertFromInvariantString("*"));
				grid5.GetValue(Grid.ColumnDefinitionsProperty).Add(columnDefinition2);
				columnDefinition3.SetValue(ColumnDefinition.WidthProperty, new GridLengthTypeConverter().ConvertFromInvariantString("*"));
				grid5.GetValue(Grid.ColumnDefinitionsProperty).Add(columnDefinition3);
				columnDefinition4.SetValue(ColumnDefinition.WidthProperty, new GridLengthTypeConverter().ConvertFromInvariantString("*"));
				grid5.GetValue(Grid.ColumnDefinitionsProperty).Add(columnDefinition4);
				stackLayout.SetValue(Grid.RowProperty, 0);
				stackLayout.SetValue(Grid.ColumnProperty, 0);
				stackLayout.SetValue(Grid.ColumnSpanProperty, 4);
				stackLayout.SetValue(StackLayout.OrientationProperty, 0);
				label.SetValue(Label.FontAttributesProperty, new FontAttributesConverter().ConvertFromInvariantString("Bold"));
				dynamicResourceExtension.Key = "BaseFontSize";
				IMarkupExtension<DynamicResource> markupExtension2 = dynamicResourceExtension;
				XamlServiceProvider xamlServiceProvider2 = new XamlServiceProvider();
				Type typeFromHandle3 = typeof(IProvideValueTarget);
				int num2;
				object[] array3 = new object[(num2 = this.parentValues.Length) + 4];
				Array.Copy(this.parentValues, 0, array3, 4, num2);
				object[] array4 = array3;
				array4[0] = label;
				array4[1] = stackLayout;
				array4[2] = grid5;
				array4[3] = viewCell;
				object obj3;
				xamlServiceProvider2.Add(typeFromHandle3, obj3 = new SimpleValueTargetProvider(array4, Label.FontSizeProperty, nameScope));
				xamlServiceProvider2.Add(typeof(IReferenceProvider), obj3);
				Type typeFromHandle4 = typeof(IXamlTypeResolver);
				XmlNamespaceResolver xmlNamespaceResolver2 = new XmlNamespaceResolver();
				xmlNamespaceResolver2.Add("", "http://xamarin.com/schemas/2014/forms");
				xmlNamespaceResolver2.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
				xmlNamespaceResolver2.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
				xmlNamespaceResolver2.Add("local", "clr-namespace:CarScannerXamarinForms");
				xmlNamespaceResolver2.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
				xamlServiceProvider2.Add(typeFromHandle4, new XamlTypeResolver(xmlNamespaceResolver2, typeof(Mode06Page.<InitializeComponent>_anonXamlCDataTemplate_69).GetTypeInfo().Assembly));
				xamlServiceProvider2.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(144, 41)));
				DynamicResource dynamicResource = markupExtension2.ProvideValue(xamlServiceProvider2);
				label.SetDynamicResource(Label.FontSizeProperty, dynamicResource.Key);
				label.SetValue(Label.HorizontalTextAlignmentProperty, new TextAlignmentConverter().ConvertFromInvariantString("Start"));
				label.SetValue(Label.LineBreakModeProperty, 1);
				bindingExtension2.Path = "MonitorName";
				BindingBase bindingBase2 = bindingExtension2.ProvideValue(null);
				label.SetBinding(Label.TextProperty, bindingBase2);
				stackLayout.Children.Add(label);
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
				array6[2] = grid5;
				array6[3] = viewCell;
				object obj4;
				xamlServiceProvider3.Add(typeFromHandle5, obj4 = new SimpleValueTargetProvider(array6, Label.FontSizeProperty, nameScope));
				xamlServiceProvider3.Add(typeof(IReferenceProvider), obj4);
				Type typeFromHandle6 = typeof(IXamlTypeResolver);
				XmlNamespaceResolver xmlNamespaceResolver3 = new XmlNamespaceResolver();
				xmlNamespaceResolver3.Add("", "http://xamarin.com/schemas/2014/forms");
				xmlNamespaceResolver3.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
				xmlNamespaceResolver3.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
				xmlNamespaceResolver3.Add("local", "clr-namespace:CarScannerXamarinForms");
				xmlNamespaceResolver3.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
				xamlServiceProvider3.Add(typeFromHandle6, new XamlTypeResolver(xmlNamespaceResolver3, typeof(Mode06Page.<InitializeComponent>_anonXamlCDataTemplate_69).GetTypeInfo().Assembly));
				xamlServiceProvider3.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(149, 41)));
				DynamicResource dynamicResource2 = markupExtension3.ProvideValue(xamlServiceProvider3);
				label2.SetDynamicResource(Label.FontSizeProperty, dynamicResource2.Key);
				label2.SetValue(Label.HorizontalTextAlignmentProperty, new TextAlignmentConverter().ConvertFromInvariantString("Start"));
				label2.SetValue(Label.LineBreakModeProperty, 1);
				bindingExtension3.Path = "TestId";
				BindingBase bindingBase3 = bindingExtension3.ProvideValue(null);
				label2.SetBinding(Label.TextProperty, bindingBase3);
				stackLayout.Children.Add(label2);
				grid5.Children.Add(stackLayout);
				label3.SetValue(Grid.RowProperty, 1);
				label3.SetValue(Grid.ColumnProperty, 0);
				label3.SetValue(Label.FontAttributesProperty, new FontAttributesConverter().ConvertFromInvariantString("Bold"));
				staticResourceExtension.Key = "SmallLabel";
				IMarkupExtension markupExtension4 = staticResourceExtension;
				XamlServiceProvider xamlServiceProvider4 = new XamlServiceProvider();
				Type typeFromHandle7 = typeof(IProvideValueTarget);
				int num4;
				object[] array7 = new object[(num4 = this.parentValues.Length) + 3];
				Array.Copy(this.parentValues, 0, array7, 3, num4);
				object[] array8 = array7;
				array8[0] = label3;
				array8[1] = grid5;
				array8[2] = viewCell;
				object obj5;
				xamlServiceProvider4.Add(typeFromHandle7, obj5 = new SimpleValueTargetProvider(array8, VisualElement.StyleProperty, nameScope));
				xamlServiceProvider4.Add(typeof(IReferenceProvider), obj5);
				Type typeFromHandle8 = typeof(IXamlTypeResolver);
				XmlNamespaceResolver xmlNamespaceResolver4 = new XmlNamespaceResolver();
				xmlNamespaceResolver4.Add("", "http://xamarin.com/schemas/2014/forms");
				xmlNamespaceResolver4.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
				xmlNamespaceResolver4.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
				xmlNamespaceResolver4.Add("local", "clr-namespace:CarScannerXamarinForms");
				xmlNamespaceResolver4.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
				xamlServiceProvider4.Add(typeFromHandle8, new XamlTypeResolver(xmlNamespaceResolver4, typeof(Mode06Page.<InitializeComponent>_anonXamlCDataTemplate_69).GetTypeInfo().Assembly));
				xamlServiceProvider4.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(159, 37)));
				object obj6 = markupExtension4.ProvideValue(xamlServiceProvider4);
				label3.Style = obj6;
				translate2.Text = "Mode06Page_tbValue.Text";
				IMarkupExtension markupExtension5 = translate2;
				XamlServiceProvider xamlServiceProvider5 = new XamlServiceProvider();
				Type typeFromHandle9 = typeof(IProvideValueTarget);
				int num5;
				object[] array9 = new object[(num5 = this.parentValues.Length) + 3];
				Array.Copy(this.parentValues, 0, array9, 3, num5);
				object[] array10 = array9;
				array10[0] = label3;
				array10[1] = grid5;
				array10[2] = viewCell;
				object obj7;
				xamlServiceProvider5.Add(typeFromHandle9, obj7 = new SimpleValueTargetProvider(array10, Label.TextProperty, nameScope));
				xamlServiceProvider5.Add(typeof(IReferenceProvider), obj7);
				Type typeFromHandle10 = typeof(IXamlTypeResolver);
				XmlNamespaceResolver xmlNamespaceResolver5 = new XmlNamespaceResolver();
				xmlNamespaceResolver5.Add("", "http://xamarin.com/schemas/2014/forms");
				xmlNamespaceResolver5.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
				xmlNamespaceResolver5.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
				xmlNamespaceResolver5.Add("local", "clr-namespace:CarScannerXamarinForms");
				xmlNamespaceResolver5.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
				xamlServiceProvider5.Add(typeFromHandle10, new XamlTypeResolver(xmlNamespaceResolver5, typeof(Mode06Page.<InitializeComponent>_anonXamlCDataTemplate_69).GetTypeInfo().Assembly));
				xamlServiceProvider5.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(160, 37)));
				object obj8 = markupExtension5.ProvideValue(xamlServiceProvider5);
				label3.Text = obj8;
				grid5.Children.Add(label3);
				label4.SetValue(Grid.RowProperty, 1);
				label4.SetValue(Grid.ColumnProperty, 1);
				label4.SetValue(Label.FontAttributesProperty, new FontAttributesConverter().ConvertFromInvariantString("Bold"));
				staticResourceExtension2.Key = "SmallLabel";
				IMarkupExtension markupExtension6 = staticResourceExtension2;
				XamlServiceProvider xamlServiceProvider6 = new XamlServiceProvider();
				Type typeFromHandle11 = typeof(IProvideValueTarget);
				int num6;
				object[] array11 = new object[(num6 = this.parentValues.Length) + 3];
				Array.Copy(this.parentValues, 0, array11, 3, num6);
				object[] array12 = array11;
				array12[0] = label4;
				array12[1] = grid5;
				array12[2] = viewCell;
				object obj9;
				xamlServiceProvider6.Add(typeFromHandle11, obj9 = new SimpleValueTargetProvider(array12, VisualElement.StyleProperty, nameScope));
				xamlServiceProvider6.Add(typeof(IReferenceProvider), obj9);
				Type typeFromHandle12 = typeof(IXamlTypeResolver);
				XmlNamespaceResolver xmlNamespaceResolver6 = new XmlNamespaceResolver();
				xmlNamespaceResolver6.Add("", "http://xamarin.com/schemas/2014/forms");
				xmlNamespaceResolver6.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
				xmlNamespaceResolver6.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
				xmlNamespaceResolver6.Add("local", "clr-namespace:CarScannerXamarinForms");
				xmlNamespaceResolver6.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
				xamlServiceProvider6.Add(typeFromHandle12, new XamlTypeResolver(xmlNamespaceResolver6, typeof(Mode06Page.<InitializeComponent>_anonXamlCDataTemplate_69).GetTypeInfo().Assembly));
				xamlServiceProvider6.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(165, 37)));
				object obj10 = markupExtension6.ProvideValue(xamlServiceProvider6);
				label4.Style = obj10;
				translate3.Text = "Mode06Page_tbMinimum.Text";
				IMarkupExtension markupExtension7 = translate3;
				XamlServiceProvider xamlServiceProvider7 = new XamlServiceProvider();
				Type typeFromHandle13 = typeof(IProvideValueTarget);
				int num7;
				object[] array13 = new object[(num7 = this.parentValues.Length) + 3];
				Array.Copy(this.parentValues, 0, array13, 3, num7);
				object[] array14 = array13;
				array14[0] = label4;
				array14[1] = grid5;
				array14[2] = viewCell;
				object obj11;
				xamlServiceProvider7.Add(typeFromHandle13, obj11 = new SimpleValueTargetProvider(array14, Label.TextProperty, nameScope));
				xamlServiceProvider7.Add(typeof(IReferenceProvider), obj11);
				Type typeFromHandle14 = typeof(IXamlTypeResolver);
				XmlNamespaceResolver xmlNamespaceResolver7 = new XmlNamespaceResolver();
				xmlNamespaceResolver7.Add("", "http://xamarin.com/schemas/2014/forms");
				xmlNamespaceResolver7.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
				xmlNamespaceResolver7.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
				xmlNamespaceResolver7.Add("local", "clr-namespace:CarScannerXamarinForms");
				xmlNamespaceResolver7.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
				xamlServiceProvider7.Add(typeFromHandle14, new XamlTypeResolver(xmlNamespaceResolver7, typeof(Mode06Page.<InitializeComponent>_anonXamlCDataTemplate_69).GetTypeInfo().Assembly));
				xamlServiceProvider7.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(166, 37)));
				object obj12 = markupExtension7.ProvideValue(xamlServiceProvider7);
				label4.Text = obj12;
				grid5.Children.Add(label4);
				label5.SetValue(Grid.RowProperty, 1);
				label5.SetValue(Grid.ColumnProperty, 2);
				label5.SetValue(Label.FontAttributesProperty, new FontAttributesConverter().ConvertFromInvariantString("Bold"));
				staticResourceExtension3.Key = "SmallLabel";
				IMarkupExtension markupExtension8 = staticResourceExtension3;
				XamlServiceProvider xamlServiceProvider8 = new XamlServiceProvider();
				Type typeFromHandle15 = typeof(IProvideValueTarget);
				int num8;
				object[] array15 = new object[(num8 = this.parentValues.Length) + 3];
				Array.Copy(this.parentValues, 0, array15, 3, num8);
				object[] array16 = array15;
				array16[0] = label5;
				array16[1] = grid5;
				array16[2] = viewCell;
				object obj13;
				xamlServiceProvider8.Add(typeFromHandle15, obj13 = new SimpleValueTargetProvider(array16, VisualElement.StyleProperty, nameScope));
				xamlServiceProvider8.Add(typeof(IReferenceProvider), obj13);
				Type typeFromHandle16 = typeof(IXamlTypeResolver);
				XmlNamespaceResolver xmlNamespaceResolver8 = new XmlNamespaceResolver();
				xmlNamespaceResolver8.Add("", "http://xamarin.com/schemas/2014/forms");
				xmlNamespaceResolver8.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
				xmlNamespaceResolver8.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
				xmlNamespaceResolver8.Add("local", "clr-namespace:CarScannerXamarinForms");
				xmlNamespaceResolver8.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
				xamlServiceProvider8.Add(typeFromHandle16, new XamlTypeResolver(xmlNamespaceResolver8, typeof(Mode06Page.<InitializeComponent>_anonXamlCDataTemplate_69).GetTypeInfo().Assembly));
				xamlServiceProvider8.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(171, 37)));
				object obj14 = markupExtension8.ProvideValue(xamlServiceProvider8);
				label5.Style = obj14;
				translate4.Text = "Mode06Page_tbMaximum.Text";
				IMarkupExtension markupExtension9 = translate4;
				XamlServiceProvider xamlServiceProvider9 = new XamlServiceProvider();
				Type typeFromHandle17 = typeof(IProvideValueTarget);
				int num9;
				object[] array17 = new object[(num9 = this.parentValues.Length) + 3];
				Array.Copy(this.parentValues, 0, array17, 3, num9);
				object[] array18 = array17;
				array18[0] = label5;
				array18[1] = grid5;
				array18[2] = viewCell;
				object obj15;
				xamlServiceProvider9.Add(typeFromHandle17, obj15 = new SimpleValueTargetProvider(array18, Label.TextProperty, nameScope));
				xamlServiceProvider9.Add(typeof(IReferenceProvider), obj15);
				Type typeFromHandle18 = typeof(IXamlTypeResolver);
				XmlNamespaceResolver xmlNamespaceResolver9 = new XmlNamespaceResolver();
				xmlNamespaceResolver9.Add("", "http://xamarin.com/schemas/2014/forms");
				xmlNamespaceResolver9.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
				xmlNamespaceResolver9.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
				xmlNamespaceResolver9.Add("local", "clr-namespace:CarScannerXamarinForms");
				xmlNamespaceResolver9.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
				xamlServiceProvider9.Add(typeFromHandle18, new XamlTypeResolver(xmlNamespaceResolver9, typeof(Mode06Page.<InitializeComponent>_anonXamlCDataTemplate_69).GetTypeInfo().Assembly));
				xamlServiceProvider9.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(172, 37)));
				object obj16 = markupExtension9.ProvideValue(xamlServiceProvider9);
				label5.Text = obj16;
				grid5.Children.Add(label5);
				label6.SetValue(Grid.RowProperty, 1);
				label6.SetValue(Grid.ColumnProperty, 3);
				label6.SetValue(Label.FontAttributesProperty, new FontAttributesConverter().ConvertFromInvariantString("Bold"));
				staticResourceExtension4.Key = "SmallLabel";
				IMarkupExtension markupExtension10 = staticResourceExtension4;
				XamlServiceProvider xamlServiceProvider10 = new XamlServiceProvider();
				Type typeFromHandle19 = typeof(IProvideValueTarget);
				int num10;
				object[] array19 = new object[(num10 = this.parentValues.Length) + 3];
				Array.Copy(this.parentValues, 0, array19, 3, num10);
				object[] array20 = array19;
				array20[0] = label6;
				array20[1] = grid5;
				array20[2] = viewCell;
				object obj17;
				xamlServiceProvider10.Add(typeFromHandle19, obj17 = new SimpleValueTargetProvider(array20, VisualElement.StyleProperty, nameScope));
				xamlServiceProvider10.Add(typeof(IReferenceProvider), obj17);
				Type typeFromHandle20 = typeof(IXamlTypeResolver);
				XmlNamespaceResolver xmlNamespaceResolver10 = new XmlNamespaceResolver();
				xmlNamespaceResolver10.Add("", "http://xamarin.com/schemas/2014/forms");
				xmlNamespaceResolver10.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
				xmlNamespaceResolver10.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
				xmlNamespaceResolver10.Add("local", "clr-namespace:CarScannerXamarinForms");
				xmlNamespaceResolver10.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
				xamlServiceProvider10.Add(typeFromHandle20, new XamlTypeResolver(xmlNamespaceResolver10, typeof(Mode06Page.<InitializeComponent>_anonXamlCDataTemplate_69).GetTypeInfo().Assembly));
				xamlServiceProvider10.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(177, 37)));
				object obj18 = markupExtension10.ProvideValue(xamlServiceProvider10);
				label6.Style = obj18;
				translate5.Text = "Mode06Page_tbResult.Text";
				IMarkupExtension markupExtension11 = translate5;
				XamlServiceProvider xamlServiceProvider11 = new XamlServiceProvider();
				Type typeFromHandle21 = typeof(IProvideValueTarget);
				int num11;
				object[] array21 = new object[(num11 = this.parentValues.Length) + 3];
				Array.Copy(this.parentValues, 0, array21, 3, num11);
				object[] array22 = array21;
				array22[0] = label6;
				array22[1] = grid5;
				array22[2] = viewCell;
				object obj19;
				xamlServiceProvider11.Add(typeFromHandle21, obj19 = new SimpleValueTargetProvider(array22, Label.TextProperty, nameScope));
				xamlServiceProvider11.Add(typeof(IReferenceProvider), obj19);
				Type typeFromHandle22 = typeof(IXamlTypeResolver);
				XmlNamespaceResolver xmlNamespaceResolver11 = new XmlNamespaceResolver();
				xmlNamespaceResolver11.Add("", "http://xamarin.com/schemas/2014/forms");
				xmlNamespaceResolver11.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
				xmlNamespaceResolver11.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
				xmlNamespaceResolver11.Add("local", "clr-namespace:CarScannerXamarinForms");
				xmlNamespaceResolver11.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
				xamlServiceProvider11.Add(typeFromHandle22, new XamlTypeResolver(xmlNamespaceResolver11, typeof(Mode06Page.<InitializeComponent>_anonXamlCDataTemplate_69).GetTypeInfo().Assembly));
				xamlServiceProvider11.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(178, 37)));
				object obj20 = markupExtension11.ProvideValue(xamlServiceProvider11);
				label6.Text = obj20;
				grid5.Children.Add(label6);
				grid.SetValue(Grid.RowProperty, 2);
				grid.SetValue(Grid.ColumnProperty, 0);
				grid.SetValue(View.HorizontalOptionsProperty, LayoutOptions.Center);
				bindingExtension4.Mode = 2;
				bindingExtension4.Path = "IsPro";
				BindingBase bindingBase4 = bindingExtension4.ProvideValue(null);
				grid.SetBinding(VisualElement.IsVisibleProperty, bindingBase4);
				grid.SetValue(View.VerticalOptionsProperty, LayoutOptions.Center);
				stackLayout2.SetValue(View.HorizontalOptionsProperty, LayoutOptions.Center);
				bindingExtension5.Mode = 2;
				bindingExtension5.Path = "TestPassed";
				BindingBase bindingBase5 = bindingExtension5.ProvideValue(null);
				stackLayout2.SetBinding(VisualElement.IsVisibleProperty, bindingBase5);
				stackLayout2.SetValue(StackLayout.OrientationProperty, 1);
				stackLayout2.SetValue(View.VerticalOptionsProperty, LayoutOptions.Center);
				label7.SetValue(Label.LineBreakModeProperty, 1);
				staticResourceExtension5.Key = "SmallLabel";
				IMarkupExtension markupExtension12 = staticResourceExtension5;
				XamlServiceProvider xamlServiceProvider12 = new XamlServiceProvider();
				Type typeFromHandle23 = typeof(IProvideValueTarget);
				int num12;
				object[] array23 = new object[(num12 = this.parentValues.Length) + 5];
				Array.Copy(this.parentValues, 0, array23, 5, num12);
				object[] array24 = array23;
				array24[0] = label7;
				array24[1] = stackLayout2;
				array24[2] = grid;
				array24[3] = grid5;
				array24[4] = viewCell;
				object obj21;
				xamlServiceProvider12.Add(typeFromHandle23, obj21 = new SimpleValueTargetProvider(array24, VisualElement.StyleProperty, nameScope));
				xamlServiceProvider12.Add(typeof(IReferenceProvider), obj21);
				Type typeFromHandle24 = typeof(IXamlTypeResolver);
				XmlNamespaceResolver xmlNamespaceResolver12 = new XmlNamespaceResolver();
				xmlNamespaceResolver12.Add("", "http://xamarin.com/schemas/2014/forms");
				xmlNamespaceResolver12.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
				xmlNamespaceResolver12.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
				xmlNamespaceResolver12.Add("local", "clr-namespace:CarScannerXamarinForms");
				xmlNamespaceResolver12.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
				xamlServiceProvider12.Add(typeFromHandle24, new XamlTypeResolver(xmlNamespaceResolver12, typeof(Mode06Page.<InitializeComponent>_anonXamlCDataTemplate_69).GetTypeInfo().Assembly));
				xamlServiceProvider12.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(195, 45)));
				object obj22 = markupExtension12.ProvideValue(xamlServiceProvider12);
				label7.Style = obj22;
				bindingExtension6.Path = "TestValue";
				BindingBase bindingBase6 = bindingExtension6.ProvideValue(null);
				label7.SetBinding(Label.TextProperty, bindingBase6);
				stackLayout2.Children.Add(label7);
				label8.SetValue(Label.LineBreakModeProperty, 1);
				staticResourceExtension6.Key = "SmallLabel";
				IMarkupExtension markupExtension13 = staticResourceExtension6;
				XamlServiceProvider xamlServiceProvider13 = new XamlServiceProvider();
				Type typeFromHandle25 = typeof(IProvideValueTarget);
				int num13;
				object[] array25 = new object[(num13 = this.parentValues.Length) + 5];
				Array.Copy(this.parentValues, 0, array25, 5, num13);
				object[] array26 = array25;
				array26[0] = label8;
				array26[1] = stackLayout2;
				array26[2] = grid;
				array26[3] = grid5;
				array26[4] = viewCell;
				object obj23;
				xamlServiceProvider13.Add(typeFromHandle25, obj23 = new SimpleValueTargetProvider(array26, VisualElement.StyleProperty, nameScope));
				xamlServiceProvider13.Add(typeof(IReferenceProvider), obj23);
				Type typeFromHandle26 = typeof(IXamlTypeResolver);
				XmlNamespaceResolver xmlNamespaceResolver13 = new XmlNamespaceResolver();
				xmlNamespaceResolver13.Add("", "http://xamarin.com/schemas/2014/forms");
				xmlNamespaceResolver13.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
				xmlNamespaceResolver13.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
				xmlNamespaceResolver13.Add("local", "clr-namespace:CarScannerXamarinForms");
				xmlNamespaceResolver13.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
				xamlServiceProvider13.Add(typeFromHandle26, new XamlTypeResolver(xmlNamespaceResolver13, typeof(Mode06Page.<InitializeComponent>_anonXamlCDataTemplate_69).GetTypeInfo().Assembly));
				xamlServiceProvider13.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(200, 45)));
				object obj24 = markupExtension13.ProvideValue(xamlServiceProvider13);
				label8.Style = obj24;
				bindingExtension7.Path = "UnitsValue";
				BindingBase bindingBase7 = bindingExtension7.ProvideValue(null);
				label8.SetBinding(Label.TextProperty, bindingBase7);
				stackLayout2.Children.Add(label8);
				grid.Children.Add(stackLayout2);
				stackLayout3.SetValue(View.HorizontalOptionsProperty, LayoutOptions.Center);
				bindingExtension8.Mode = 2;
				bindingExtension8.Path = "TestNotPassed";
				BindingBase bindingBase8 = bindingExtension8.ProvideValue(null);
				stackLayout3.SetBinding(VisualElement.IsVisibleProperty, bindingBase8);
				stackLayout3.SetValue(StackLayout.OrientationProperty, 1);
				stackLayout3.SetValue(View.VerticalOptionsProperty, LayoutOptions.Center);
				staticResourceExtension7.Key = "SmallLabel";
				IMarkupExtension markupExtension14 = staticResourceExtension7;
				XamlServiceProvider xamlServiceProvider14 = new XamlServiceProvider();
				Type typeFromHandle27 = typeof(IProvideValueTarget);
				int num14;
				object[] array27 = new object[(num14 = this.parentValues.Length) + 5];
				Array.Copy(this.parentValues, 0, array27, 5, num14);
				object[] array28 = array27;
				array28[0] = label9;
				array28[1] = stackLayout3;
				array28[2] = grid;
				array28[3] = grid5;
				array28[4] = viewCell;
				object obj25;
				xamlServiceProvider14.Add(typeFromHandle27, obj25 = new SimpleValueTargetProvider(array28, VisualElement.StyleProperty, nameScope));
				xamlServiceProvider14.Add(typeof(IReferenceProvider), obj25);
				Type typeFromHandle28 = typeof(IXamlTypeResolver);
				XmlNamespaceResolver xmlNamespaceResolver14 = new XmlNamespaceResolver();
				xmlNamespaceResolver14.Add("", "http://xamarin.com/schemas/2014/forms");
				xmlNamespaceResolver14.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
				xmlNamespaceResolver14.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
				xmlNamespaceResolver14.Add("local", "clr-namespace:CarScannerXamarinForms");
				xmlNamespaceResolver14.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
				xamlServiceProvider14.Add(typeFromHandle28, new XamlTypeResolver(xmlNamespaceResolver14, typeof(Mode06Page.<InitializeComponent>_anonXamlCDataTemplate_69).GetTypeInfo().Assembly));
				xamlServiceProvider14.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(210, 45)));
				object obj26 = markupExtension14.ProvideValue(xamlServiceProvider14);
				label9.Style = obj26;
				bindingExtension9.Path = "TestValue";
				BindingBase bindingBase9 = bindingExtension9.ProvideValue(null);
				label9.SetBinding(Label.TextProperty, bindingBase9);
				label9.SetValue(Label.TextColorProperty, Color.Red);
				stackLayout3.Children.Add(label9);
				staticResourceExtension8.Key = "SmallLabel";
				IMarkupExtension markupExtension15 = staticResourceExtension8;
				XamlServiceProvider xamlServiceProvider15 = new XamlServiceProvider();
				Type typeFromHandle29 = typeof(IProvideValueTarget);
				int num15;
				object[] array29 = new object[(num15 = this.parentValues.Length) + 5];
				Array.Copy(this.parentValues, 0, array29, 5, num15);
				object[] array30 = array29;
				array30[0] = label10;
				array30[1] = stackLayout3;
				array30[2] = grid;
				array30[3] = grid5;
				array30[4] = viewCell;
				object obj27;
				xamlServiceProvider15.Add(typeFromHandle29, obj27 = new SimpleValueTargetProvider(array30, VisualElement.StyleProperty, nameScope));
				xamlServiceProvider15.Add(typeof(IReferenceProvider), obj27);
				Type typeFromHandle30 = typeof(IXamlTypeResolver);
				XmlNamespaceResolver xmlNamespaceResolver15 = new XmlNamespaceResolver();
				xmlNamespaceResolver15.Add("", "http://xamarin.com/schemas/2014/forms");
				xmlNamespaceResolver15.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
				xmlNamespaceResolver15.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
				xmlNamespaceResolver15.Add("local", "clr-namespace:CarScannerXamarinForms");
				xmlNamespaceResolver15.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
				xamlServiceProvider15.Add(typeFromHandle30, new XamlTypeResolver(xmlNamespaceResolver15, typeof(Mode06Page.<InitializeComponent>_anonXamlCDataTemplate_69).GetTypeInfo().Assembly));
				xamlServiceProvider15.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(214, 45)));
				object obj28 = markupExtension15.ProvideValue(xamlServiceProvider15);
				label10.Style = obj28;
				bindingExtension10.Path = "UnitsValue";
				BindingBase bindingBase10 = bindingExtension10.ProvideValue(null);
				label10.SetBinding(Label.TextProperty, bindingBase10);
				label10.SetValue(Label.TextColorProperty, Color.Red);
				stackLayout3.Children.Add(label10);
				grid.Children.Add(stackLayout3);
				grid5.Children.Add(grid);
				grid2.SetValue(Grid.RowProperty, 2);
				grid2.SetValue(Grid.ColumnProperty, 0);
				grid2.SetValue(View.HorizontalOptionsProperty, LayoutOptions.Center);
				bindingExtension11.Mode = 2;
				bindingExtension11.Path = "IsFree";
				BindingBase bindingBase11 = bindingExtension11.ProvideValue(null);
				grid2.SetBinding(VisualElement.IsVisibleProperty, bindingBase11);
				grid2.SetValue(View.VerticalOptionsProperty, LayoutOptions.Center);
				stackLayout4.SetValue(View.HorizontalOptionsProperty, LayoutOptions.Center);
				stackLayout4.SetValue(StackLayout.OrientationProperty, 1);
				stackLayout4.SetValue(View.VerticalOptionsProperty, LayoutOptions.Center);
				staticResourceExtension9.Key = "SmallLabel";
				IMarkupExtension markupExtension16 = staticResourceExtension9;
				XamlServiceProvider xamlServiceProvider16 = new XamlServiceProvider();
				Type typeFromHandle31 = typeof(IProvideValueTarget);
				int num16;
				object[] array31 = new object[(num16 = this.parentValues.Length) + 5];
				Array.Copy(this.parentValues, 0, array31, 5, num16);
				object[] array32 = array31;
				array32[0] = label11;
				array32[1] = stackLayout4;
				array32[2] = grid2;
				array32[3] = grid5;
				array32[4] = viewCell;
				object obj29;
				xamlServiceProvider16.Add(typeFromHandle31, obj29 = new SimpleValueTargetProvider(array32, VisualElement.StyleProperty, nameScope));
				xamlServiceProvider16.Add(typeof(IReferenceProvider), obj29);
				Type typeFromHandle32 = typeof(IXamlTypeResolver);
				XmlNamespaceResolver xmlNamespaceResolver16 = new XmlNamespaceResolver();
				xmlNamespaceResolver16.Add("", "http://xamarin.com/schemas/2014/forms");
				xmlNamespaceResolver16.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
				xmlNamespaceResolver16.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
				xmlNamespaceResolver16.Add("local", "clr-namespace:CarScannerXamarinForms");
				xmlNamespaceResolver16.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
				xamlServiceProvider16.Add(typeFromHandle32, new XamlTypeResolver(xmlNamespaceResolver16, typeof(Mode06Page.<InitializeComponent>_anonXamlCDataTemplate_69).GetTypeInfo().Assembly));
				xamlServiceProvider16.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(233, 48)));
				object obj30 = markupExtension16.ProvideValue(xamlServiceProvider16);
				label11.Style = obj30;
				bindingExtension12.Path = "TestValue";
				BindingBase bindingBase12 = bindingExtension12.ProvideValue(null);
				label11.SetBinding(Label.TextProperty, bindingBase12);
				stackLayout4.Children.Add(label11);
				staticResourceExtension10.Key = "SmallLabel";
				IMarkupExtension markupExtension17 = staticResourceExtension10;
				XamlServiceProvider xamlServiceProvider17 = new XamlServiceProvider();
				Type typeFromHandle33 = typeof(IProvideValueTarget);
				int num17;
				object[] array33 = new object[(num17 = this.parentValues.Length) + 5];
				Array.Copy(this.parentValues, 0, array33, 5, num17);
				object[] array34 = array33;
				array34[0] = label12;
				array34[1] = stackLayout4;
				array34[2] = grid2;
				array34[3] = grid5;
				array34[4] = viewCell;
				object obj31;
				xamlServiceProvider17.Add(typeFromHandle33, obj31 = new SimpleValueTargetProvider(array34, VisualElement.StyleProperty, nameScope));
				xamlServiceProvider17.Add(typeof(IReferenceProvider), obj31);
				Type typeFromHandle34 = typeof(IXamlTypeResolver);
				XmlNamespaceResolver xmlNamespaceResolver17 = new XmlNamespaceResolver();
				xmlNamespaceResolver17.Add("", "http://xamarin.com/schemas/2014/forms");
				xmlNamespaceResolver17.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
				xmlNamespaceResolver17.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
				xmlNamespaceResolver17.Add("local", "clr-namespace:CarScannerXamarinForms");
				xmlNamespaceResolver17.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
				xamlServiceProvider17.Add(typeFromHandle34, new XamlTypeResolver(xmlNamespaceResolver17, typeof(Mode06Page.<InitializeComponent>_anonXamlCDataTemplate_69).GetTypeInfo().Assembly));
				xamlServiceProvider17.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(234, 48)));
				object obj32 = markupExtension17.ProvideValue(xamlServiceProvider17);
				label12.Style = obj32;
				bindingExtension13.Path = "UnitsValue";
				BindingBase bindingBase13 = bindingExtension13.ProvideValue(null);
				label12.SetBinding(Label.TextProperty, bindingBase13);
				stackLayout4.Children.Add(label12);
				grid2.Children.Add(stackLayout4);
				grid5.Children.Add(grid2);
				stackLayout5.SetValue(Grid.RowProperty, 2);
				stackLayout5.SetValue(Grid.ColumnProperty, 1);
				stackLayout5.SetValue(View.HorizontalOptionsProperty, LayoutOptions.Center);
				stackLayout5.SetValue(StackLayout.OrientationProperty, 1);
				stackLayout5.SetValue(View.VerticalOptionsProperty, LayoutOptions.Center);
				staticResourceExtension11.Key = "SmallLabel";
				IMarkupExtension markupExtension18 = staticResourceExtension11;
				XamlServiceProvider xamlServiceProvider18 = new XamlServiceProvider();
				Type typeFromHandle35 = typeof(IProvideValueTarget);
				int num18;
				object[] array35 = new object[(num18 = this.parentValues.Length) + 4];
				Array.Copy(this.parentValues, 0, array35, 4, num18);
				object[] array36 = array35;
				array36[0] = label13;
				array36[1] = stackLayout5;
				array36[2] = grid5;
				array36[3] = viewCell;
				object obj33;
				xamlServiceProvider18.Add(typeFromHandle35, obj33 = new SimpleValueTargetProvider(array36, VisualElement.StyleProperty, nameScope));
				xamlServiceProvider18.Add(typeof(IReferenceProvider), obj33);
				Type typeFromHandle36 = typeof(IXamlTypeResolver);
				XmlNamespaceResolver xmlNamespaceResolver18 = new XmlNamespaceResolver();
				xmlNamespaceResolver18.Add("", "http://xamarin.com/schemas/2014/forms");
				xmlNamespaceResolver18.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
				xmlNamespaceResolver18.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
				xmlNamespaceResolver18.Add("local", "clr-namespace:CarScannerXamarinForms");
				xmlNamespaceResolver18.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
				xamlServiceProvider18.Add(typeFromHandle36, new XamlTypeResolver(xmlNamespaceResolver18, typeof(Mode06Page.<InitializeComponent>_anonXamlCDataTemplate_69).GetTypeInfo().Assembly));
				xamlServiceProvider18.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(245, 44)));
				object obj34 = markupExtension18.ProvideValue(xamlServiceProvider18);
				label13.Style = obj34;
				bindingExtension14.Path = "MinValue";
				BindingBase bindingBase14 = bindingExtension14.ProvideValue(null);
				label13.SetBinding(Label.TextProperty, bindingBase14);
				stackLayout5.Children.Add(label13);
				staticResourceExtension12.Key = "SmallLabel";
				IMarkupExtension markupExtension19 = staticResourceExtension12;
				XamlServiceProvider xamlServiceProvider19 = new XamlServiceProvider();
				Type typeFromHandle37 = typeof(IProvideValueTarget);
				int num19;
				object[] array37 = new object[(num19 = this.parentValues.Length) + 4];
				Array.Copy(this.parentValues, 0, array37, 4, num19);
				object[] array38 = array37;
				array38[0] = label14;
				array38[1] = stackLayout5;
				array38[2] = grid5;
				array38[3] = viewCell;
				object obj35;
				xamlServiceProvider19.Add(typeFromHandle37, obj35 = new SimpleValueTargetProvider(array38, VisualElement.StyleProperty, nameScope));
				xamlServiceProvider19.Add(typeof(IReferenceProvider), obj35);
				Type typeFromHandle38 = typeof(IXamlTypeResolver);
				XmlNamespaceResolver xmlNamespaceResolver19 = new XmlNamespaceResolver();
				xmlNamespaceResolver19.Add("", "http://xamarin.com/schemas/2014/forms");
				xmlNamespaceResolver19.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
				xmlNamespaceResolver19.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
				xmlNamespaceResolver19.Add("local", "clr-namespace:CarScannerXamarinForms");
				xmlNamespaceResolver19.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
				xamlServiceProvider19.Add(typeFromHandle38, new XamlTypeResolver(xmlNamespaceResolver19, typeof(Mode06Page.<InitializeComponent>_anonXamlCDataTemplate_69).GetTypeInfo().Assembly));
				xamlServiceProvider19.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(246, 44)));
				object obj36 = markupExtension19.ProvideValue(xamlServiceProvider19);
				label14.Style = obj36;
				bindingExtension15.Path = "UnitsValue";
				BindingBase bindingBase15 = bindingExtension15.ProvideValue(null);
				label14.SetBinding(Label.TextProperty, bindingBase15);
				stackLayout5.Children.Add(label14);
				grid5.Children.Add(stackLayout5);
				stackLayout6.SetValue(Grid.RowProperty, 2);
				stackLayout6.SetValue(Grid.ColumnProperty, 2);
				stackLayout6.SetValue(View.HorizontalOptionsProperty, LayoutOptions.Center);
				stackLayout6.SetValue(StackLayout.OrientationProperty, 1);
				stackLayout6.SetValue(View.VerticalOptionsProperty, LayoutOptions.Center);
				staticResourceExtension13.Key = "SmallLabel";
				IMarkupExtension markupExtension20 = staticResourceExtension13;
				XamlServiceProvider xamlServiceProvider20 = new XamlServiceProvider();
				Type typeFromHandle39 = typeof(IProvideValueTarget);
				int num20;
				object[] array39 = new object[(num20 = this.parentValues.Length) + 4];
				Array.Copy(this.parentValues, 0, array39, 4, num20);
				object[] array40 = array39;
				array40[0] = label15;
				array40[1] = stackLayout6;
				array40[2] = grid5;
				array40[3] = viewCell;
				object obj37;
				xamlServiceProvider20.Add(typeFromHandle39, obj37 = new SimpleValueTargetProvider(array40, VisualElement.StyleProperty, nameScope));
				xamlServiceProvider20.Add(typeof(IReferenceProvider), obj37);
				Type typeFromHandle40 = typeof(IXamlTypeResolver);
				XmlNamespaceResolver xmlNamespaceResolver20 = new XmlNamespaceResolver();
				xmlNamespaceResolver20.Add("", "http://xamarin.com/schemas/2014/forms");
				xmlNamespaceResolver20.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
				xmlNamespaceResolver20.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
				xmlNamespaceResolver20.Add("local", "clr-namespace:CarScannerXamarinForms");
				xmlNamespaceResolver20.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
				xamlServiceProvider20.Add(typeFromHandle40, new XamlTypeResolver(xmlNamespaceResolver20, typeof(Mode06Page.<InitializeComponent>_anonXamlCDataTemplate_69).GetTypeInfo().Assembly));
				xamlServiceProvider20.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(256, 44)));
				object obj38 = markupExtension20.ProvideValue(xamlServiceProvider20);
				label15.Style = obj38;
				bindingExtension16.Path = "MaxValue";
				BindingBase bindingBase16 = bindingExtension16.ProvideValue(null);
				label15.SetBinding(Label.TextProperty, bindingBase16);
				stackLayout6.Children.Add(label15);
				staticResourceExtension14.Key = "SmallLabel";
				IMarkupExtension markupExtension21 = staticResourceExtension14;
				XamlServiceProvider xamlServiceProvider21 = new XamlServiceProvider();
				Type typeFromHandle41 = typeof(IProvideValueTarget);
				int num21;
				object[] array41 = new object[(num21 = this.parentValues.Length) + 4];
				Array.Copy(this.parentValues, 0, array41, 4, num21);
				object[] array42 = array41;
				array42[0] = label16;
				array42[1] = stackLayout6;
				array42[2] = grid5;
				array42[3] = viewCell;
				object obj39;
				xamlServiceProvider21.Add(typeFromHandle41, obj39 = new SimpleValueTargetProvider(array42, VisualElement.StyleProperty, nameScope));
				xamlServiceProvider21.Add(typeof(IReferenceProvider), obj39);
				Type typeFromHandle42 = typeof(IXamlTypeResolver);
				XmlNamespaceResolver xmlNamespaceResolver21 = new XmlNamespaceResolver();
				xmlNamespaceResolver21.Add("", "http://xamarin.com/schemas/2014/forms");
				xmlNamespaceResolver21.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
				xmlNamespaceResolver21.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
				xmlNamespaceResolver21.Add("local", "clr-namespace:CarScannerXamarinForms");
				xmlNamespaceResolver21.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
				xamlServiceProvider21.Add(typeFromHandle42, new XamlTypeResolver(xmlNamespaceResolver21, typeof(Mode06Page.<InitializeComponent>_anonXamlCDataTemplate_69).GetTypeInfo().Assembly));
				xamlServiceProvider21.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(257, 44)));
				object obj40 = markupExtension21.ProvideValue(xamlServiceProvider21);
				label16.Style = obj40;
				bindingExtension17.Path = "UnitsValue";
				BindingBase bindingBase17 = bindingExtension17.ProvideValue(null);
				label16.SetBinding(Label.TextProperty, bindingBase17);
				stackLayout6.Children.Add(label16);
				grid5.Children.Add(stackLayout6);
				grid3.SetValue(Grid.RowProperty, 2);
				grid3.SetValue(Grid.ColumnProperty, 3);
				grid3.SetValue(View.HorizontalOptionsProperty, LayoutOptions.Center);
				bindingExtension18.Path = "IsPro";
				BindingBase bindingBase18 = bindingExtension18.ProvideValue(null);
				grid3.SetBinding(VisualElement.IsVisibleProperty, bindingBase18);
				grid3.SetValue(View.VerticalOptionsProperty, LayoutOptions.Center);
				label17.SetValue(Label.FontAttributesProperty, new FontAttributesConverter().ConvertFromInvariantString("Bold"));
				bindingExtension19.Path = "TestNotPassed";
				BindingBase bindingBase19 = bindingExtension19.ProvideValue(null);
				label17.SetBinding(VisualElement.IsVisibleProperty, bindingBase19);
				staticResourceExtension15.Key = "SmallLabel";
				IMarkupExtension markupExtension22 = staticResourceExtension15;
				XamlServiceProvider xamlServiceProvider22 = new XamlServiceProvider();
				Type typeFromHandle43 = typeof(IProvideValueTarget);
				int num22;
				object[] array43 = new object[(num22 = this.parentValues.Length) + 4];
				Array.Copy(this.parentValues, 0, array43, 4, num22);
				object[] array44 = array43;
				array44[0] = label17;
				array44[1] = grid3;
				array44[2] = grid5;
				array44[3] = viewCell;
				object obj41;
				xamlServiceProvider22.Add(typeFromHandle43, obj41 = new SimpleValueTargetProvider(array44, VisualElement.StyleProperty, nameScope));
				xamlServiceProvider22.Add(typeof(IReferenceProvider), obj41);
				Type typeFromHandle44 = typeof(IXamlTypeResolver);
				XmlNamespaceResolver xmlNamespaceResolver22 = new XmlNamespaceResolver();
				xmlNamespaceResolver22.Add("", "http://xamarin.com/schemas/2014/forms");
				xmlNamespaceResolver22.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
				xmlNamespaceResolver22.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
				xmlNamespaceResolver22.Add("local", "clr-namespace:CarScannerXamarinForms");
				xmlNamespaceResolver22.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
				xamlServiceProvider22.Add(typeFromHandle44, new XamlTypeResolver(xmlNamespaceResolver22, typeof(Mode06Page.<InitializeComponent>_anonXamlCDataTemplate_69).GetTypeInfo().Assembly));
				xamlServiceProvider22.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(269, 41)));
				object obj42 = markupExtension22.ProvideValue(xamlServiceProvider22);
				label17.Style = obj42;
				translate6.Text = "Mode06Page_tbFailed.Text";
				IMarkupExtension markupExtension23 = translate6;
				XamlServiceProvider xamlServiceProvider23 = new XamlServiceProvider();
				Type typeFromHandle45 = typeof(IProvideValueTarget);
				int num23;
				object[] array45 = new object[(num23 = this.parentValues.Length) + 4];
				Array.Copy(this.parentValues, 0, array45, 4, num23);
				object[] array46 = array45;
				array46[0] = label17;
				array46[1] = grid3;
				array46[2] = grid5;
				array46[3] = viewCell;
				object obj43;
				xamlServiceProvider23.Add(typeFromHandle45, obj43 = new SimpleValueTargetProvider(array46, Label.TextProperty, nameScope));
				xamlServiceProvider23.Add(typeof(IReferenceProvider), obj43);
				Type typeFromHandle46 = typeof(IXamlTypeResolver);
				XmlNamespaceResolver xmlNamespaceResolver23 = new XmlNamespaceResolver();
				xmlNamespaceResolver23.Add("", "http://xamarin.com/schemas/2014/forms");
				xmlNamespaceResolver23.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
				xmlNamespaceResolver23.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
				xmlNamespaceResolver23.Add("local", "clr-namespace:CarScannerXamarinForms");
				xmlNamespaceResolver23.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
				xamlServiceProvider23.Add(typeFromHandle46, new XamlTypeResolver(xmlNamespaceResolver23, typeof(Mode06Page.<InitializeComponent>_anonXamlCDataTemplate_69).GetTypeInfo().Assembly));
				xamlServiceProvider23.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(270, 41)));
				object obj44 = markupExtension23.ProvideValue(xamlServiceProvider23);
				label17.Text = obj44;
				label17.SetValue(Label.TextColorProperty, Color.Red);
				grid3.Children.Add(label17);
				label18.SetValue(Label.FontAttributesProperty, new FontAttributesConverter().ConvertFromInvariantString("Bold"));
				bindingExtension20.Path = "TestPassed";
				BindingBase bindingBase20 = bindingExtension20.ProvideValue(null);
				label18.SetBinding(VisualElement.IsVisibleProperty, bindingBase20);
				staticResourceExtension16.Key = "SmallLabel";
				IMarkupExtension markupExtension24 = staticResourceExtension16;
				XamlServiceProvider xamlServiceProvider24 = new XamlServiceProvider();
				Type typeFromHandle47 = typeof(IProvideValueTarget);
				int num24;
				object[] array47 = new object[(num24 = this.parentValues.Length) + 4];
				Array.Copy(this.parentValues, 0, array47, 4, num24);
				object[] array48 = array47;
				array48[0] = label18;
				array48[1] = grid3;
				array48[2] = grid5;
				array48[3] = viewCell;
				object obj45;
				xamlServiceProvider24.Add(typeFromHandle47, obj45 = new SimpleValueTargetProvider(array48, VisualElement.StyleProperty, nameScope));
				xamlServiceProvider24.Add(typeof(IReferenceProvider), obj45);
				Type typeFromHandle48 = typeof(IXamlTypeResolver);
				XmlNamespaceResolver xmlNamespaceResolver24 = new XmlNamespaceResolver();
				xmlNamespaceResolver24.Add("", "http://xamarin.com/schemas/2014/forms");
				xmlNamespaceResolver24.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
				xmlNamespaceResolver24.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
				xmlNamespaceResolver24.Add("local", "clr-namespace:CarScannerXamarinForms");
				xmlNamespaceResolver24.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
				xamlServiceProvider24.Add(typeFromHandle48, new XamlTypeResolver(xmlNamespaceResolver24, typeof(Mode06Page.<InitializeComponent>_anonXamlCDataTemplate_69).GetTypeInfo().Assembly));
				xamlServiceProvider24.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(275, 41)));
				object obj46 = markupExtension24.ProvideValue(xamlServiceProvider24);
				label18.Style = obj46;
				translate7.Text = "Mode06Page_tbPassed.Text";
				IMarkupExtension markupExtension25 = translate7;
				XamlServiceProvider xamlServiceProvider25 = new XamlServiceProvider();
				Type typeFromHandle49 = typeof(IProvideValueTarget);
				int num25;
				object[] array49 = new object[(num25 = this.parentValues.Length) + 4];
				Array.Copy(this.parentValues, 0, array49, 4, num25);
				object[] array50 = array49;
				array50[0] = label18;
				array50[1] = grid3;
				array50[2] = grid5;
				array50[3] = viewCell;
				object obj47;
				xamlServiceProvider25.Add(typeFromHandle49, obj47 = new SimpleValueTargetProvider(array50, Label.TextProperty, nameScope));
				xamlServiceProvider25.Add(typeof(IReferenceProvider), obj47);
				Type typeFromHandle50 = typeof(IXamlTypeResolver);
				XmlNamespaceResolver xmlNamespaceResolver25 = new XmlNamespaceResolver();
				xmlNamespaceResolver25.Add("", "http://xamarin.com/schemas/2014/forms");
				xmlNamespaceResolver25.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
				xmlNamespaceResolver25.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
				xmlNamespaceResolver25.Add("local", "clr-namespace:CarScannerXamarinForms");
				xmlNamespaceResolver25.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
				xamlServiceProvider25.Add(typeFromHandle50, new XamlTypeResolver(xmlNamespaceResolver25, typeof(Mode06Page.<InitializeComponent>_anonXamlCDataTemplate_69).GetTypeInfo().Assembly));
				xamlServiceProvider25.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(276, 41)));
				object obj48 = markupExtension25.ProvideValue(xamlServiceProvider25);
				label18.Text = obj48;
				label18.SetValue(Label.TextColorProperty, Color.Green);
				grid3.Children.Add(label18);
				grid5.Children.Add(grid3);
				grid4.SetValue(Grid.RowProperty, 2);
				grid4.SetValue(Grid.ColumnProperty, 3);
				grid4.SetValue(View.HorizontalOptionsProperty, LayoutOptions.Center);
				bindingExtension21.Path = "IsFree";
				BindingBase bindingBase21 = bindingExtension21.ProvideValue(null);
				grid4.SetBinding(VisualElement.IsVisibleProperty, bindingBase21);
				grid4.SetValue(View.VerticalOptionsProperty, LayoutOptions.Center);
				label19.SetValue(Label.FontAttributesProperty, new FontAttributesConverter().ConvertFromInvariantString("Bold"));
				staticResourceExtension17.Key = "SmallLabel";
				IMarkupExtension markupExtension26 = staticResourceExtension17;
				XamlServiceProvider xamlServiceProvider26 = new XamlServiceProvider();
				Type typeFromHandle51 = typeof(IProvideValueTarget);
				int num26;
				object[] array51 = new object[(num26 = this.parentValues.Length) + 4];
				Array.Copy(this.parentValues, 0, array51, 4, num26);
				object[] array52 = array51;
				array52[0] = label19;
				array52[1] = grid4;
				array52[2] = grid5;
				array52[3] = viewCell;
				object obj49;
				xamlServiceProvider26.Add(typeFromHandle51, obj49 = new SimpleValueTargetProvider(array52, VisualElement.StyleProperty, nameScope));
				xamlServiceProvider26.Add(typeof(IReferenceProvider), obj49);
				Type typeFromHandle52 = typeof(IXamlTypeResolver);
				XmlNamespaceResolver xmlNamespaceResolver26 = new XmlNamespaceResolver();
				xmlNamespaceResolver26.Add("", "http://xamarin.com/schemas/2014/forms");
				xmlNamespaceResolver26.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
				xmlNamespaceResolver26.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
				xmlNamespaceResolver26.Add("local", "clr-namespace:CarScannerXamarinForms");
				xmlNamespaceResolver26.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
				xamlServiceProvider26.Add(typeFromHandle52, new XamlTypeResolver(xmlNamespaceResolver26, typeof(Mode06Page.<InitializeComponent>_anonXamlCDataTemplate_69).GetTypeInfo().Assembly));
				xamlServiceProvider26.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(289, 41)));
				object obj50 = markupExtension26.ProvideValue(xamlServiceProvider26);
				label19.Style = obj50;
				translate8.Text = "Mode06Page_tbInPro.Text";
				IMarkupExtension markupExtension27 = translate8;
				XamlServiceProvider xamlServiceProvider27 = new XamlServiceProvider();
				Type typeFromHandle53 = typeof(IProvideValueTarget);
				int num27;
				object[] array53 = new object[(num27 = this.parentValues.Length) + 4];
				Array.Copy(this.parentValues, 0, array53, 4, num27);
				object[] array54 = array53;
				array54[0] = label19;
				array54[1] = grid4;
				array54[2] = grid5;
				array54[3] = viewCell;
				object obj51;
				xamlServiceProvider27.Add(typeFromHandle53, obj51 = new SimpleValueTargetProvider(array54, Label.TextProperty, nameScope));
				xamlServiceProvider27.Add(typeof(IReferenceProvider), obj51);
				Type typeFromHandle54 = typeof(IXamlTypeResolver);
				XmlNamespaceResolver xmlNamespaceResolver27 = new XmlNamespaceResolver();
				xmlNamespaceResolver27.Add("", "http://xamarin.com/schemas/2014/forms");
				xmlNamespaceResolver27.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
				xmlNamespaceResolver27.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
				xmlNamespaceResolver27.Add("local", "clr-namespace:CarScannerXamarinForms");
				xmlNamespaceResolver27.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
				xamlServiceProvider27.Add(typeFromHandle54, new XamlTypeResolver(xmlNamespaceResolver27, typeof(Mode06Page.<InitializeComponent>_anonXamlCDataTemplate_69).GetTypeInfo().Assembly));
				xamlServiceProvider27.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(290, 41)));
				object obj52 = markupExtension27.ProvideValue(xamlServiceProvider27);
				label19.Text = obj52;
				grid4.Children.Add(label19);
				grid5.Children.Add(grid4);
				viewCell.View = grid5;
				return viewCell;
			}

			// Token: 0x04000397 RID: 919
			internal object[] parentValues;

			// Token: 0x04000398 RID: 920
			internal Mode06Page root;
		}
	}
}
