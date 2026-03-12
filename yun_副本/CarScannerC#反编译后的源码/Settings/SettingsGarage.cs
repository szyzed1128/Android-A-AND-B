using System;
using System.CodeDom.Compiler;
using System.Diagnostics;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using CarScannerXamarinForms.Garage;
using Syncfusion.ListView.XForms;
using Xamarin.Forms;
using Xamarin.Forms.Internals;
using Xamarin.Forms.PlatformConfiguration.iOSSpecific;
using Xamarin.Forms.Xaml;
using Xamarin.Forms.Xaml.Diagnostics;
using Xamarin.Forms.Xaml.Internals;

namespace CarScannerXamarinForms.Settings
{
	// Token: 0x02000233 RID: 563
	[XamlFilePath("Settings\\SettingsGarage.xaml")]
	public class SettingsGarage : ContentPage
	{
		// Token: 0x06001B19 RID: 6937 RVA: 0x0012E748 File Offset: 0x0012C948
		public SettingsGarage()
		{
			this.InitializeComponent();
			this.ti_add = new ToolbarItem("", (string)Application.Current.Resources["NB_add"], delegate
			{
				this.btnNew_Clicked(null, null);
			}, 0, 0);
			this.ti_del = new ToolbarItem("", (string)Application.Current.Resources["NB_delete"], delegate
			{
				this.DeleteSelected();
			}, 0, 0);
			this.ti_info = new ToolbarItem("", (string)Application.Current.Resources["NB_info"], delegate
			{
				this.btnInfo_Clicked(null, null);
			}, 0, 0);
			base.ToolbarItems.Add(this.ti_add);
			base.ToolbarItems.Add(this.ti_info);
			this.LoadList();
		}

		// Token: 0x06001B1A RID: 6938 RVA: 0x0012E830 File Offset: 0x0012CA30
		private async void LoadList()
		{
			this.activityFrame.IsVisible = true;
			this.garageModel = new GarageModel();
			this.garageModel.Initialize(true);
			if (this.garageModel.CurrentCar != null && this.garageModel.CurrentCar.Name != null)
			{
				this.labelCurrentCar.Text = Translate.GetString("ios_CurrentCar") + this.garageModel.CurrentCar.Name;
			}
			else
			{
				this.labelCurrentCar.Text = Translate.GetString("ios_CurrentCar");
			}
			this.lv.ItemsSource = this.garageModel.Cars;
			this.lv.SelectedItem = this.garageModel.CurrentCar;
			this.activityFrame.IsVisible = false;
		}

		// Token: 0x06001B1B RID: 6939 RVA: 0x0012E868 File Offset: 0x0012CA68
		private async void DeleteSelected()
		{
			if (this.lv.SelectedItem != null)
			{
				MyCar myCar = this.lv.SelectedItem as MyCar;
				this.garageModel.Cars.Remove(myCar);
				this.garageModel.SaveToFile();
			}
		}

		// Token: 0x06001B1C RID: 6940 RVA: 0x0012E8A0 File Offset: 0x0012CAA0
		private async void btnDelCar_Tapped(object sender, object args)
		{
			this.DeleteSelected();
		}

		// Token: 0x06001B1D RID: 6941 RVA: 0x0012E8D8 File Offset: 0x0012CAD8
		private async void btnNew_Clicked(object p1, object p2)
		{
			SettingsGarageCarEditorPage settingsGarageCarEditorPage = new SettingsGarageCarEditorPage(null, this.garageModel);
			await base.Navigation.PushAsync(settingsGarageCarEditorPage);
		}

		// Token: 0x06001B1E RID: 6942 RVA: 0x0012E90F File Offset: 0x0012CB0F
		private void btnInfo_Clicked(object sender, EventArgs e)
		{
			base.DisplayAlert(Translate.GetString("ios_GarageTitle"), Translate.GetString("ios_GarageTitle_InfoText"), "OK");
		}

		// Token: 0x06001B1F RID: 6943 RVA: 0x000027D4 File Offset: 0x000009D4
		private void Handle_SizeChanged(object sender, EventArgs e)
		{
		}

		// Token: 0x06001B20 RID: 6944 RVA: 0x0012E934 File Offset: 0x0012CB34
		private void ContentPage_Appearing(object sender, EventArgs e)
		{
			if (this.garageModel != null && this.garageModel.CurrentCar != null)
			{
				this.labelCurrentCar.Text = Translate.GetString("ios_CurrentCar") + this.garageModel.CurrentCar.Name;
			}
		}

		// Token: 0x06001B21 RID: 6945 RVA: 0x0012E980 File Offset: 0x0012CB80
		private async void btnEditCar_Clicked(object sender, EventArgs e)
		{
			MyCar myCar = this.lv.SelectedItem as MyCar;
			if (myCar != null)
			{
				SettingsGarageCarEditorPage settingsGarageCarEditorPage = new SettingsGarageCarEditorPage(myCar, this.garageModel);
				await base.Navigation.PushAsync(settingsGarageCarEditorPage);
			}
		}

		// Token: 0x06001B22 RID: 6946 RVA: 0x0012E9B8 File Offset: 0x0012CBB8
		private async void btnChangeCar_Clicked(object sender, EventArgs e)
		{
			base.IsEnabled = false;
			try
			{
				SettingsGarage.<>c__DisplayClass13_0 CS$<>8__locals1 = new SettingsGarage.<>c__DisplayClass13_0();
				CS$<>8__locals1.car = this.lv.SelectedItem as MyCar;
				if (CS$<>8__locals1.car != this.garageModel.CurrentCar)
				{
					this.activityFrame.IsVisible = true;
					CS$<>8__locals1.apply_result = new Tuple<bool, string>(false, "");
					await Task.Run<Tuple<bool, string>>(() => CS$<>8__locals1.apply_result = CS$<>8__locals1.car.ApplyToSettings());
					if (!CS$<>8__locals1.apply_result.Item1)
					{
						await base.DisplayAlert("Error changing car", CS$<>8__locals1.apply_result.Item2, "OK");
						await base.Navigation.PopAsync();
					}
					else if (CS$<>8__locals1.apply_result.Item1)
					{
						await base.Navigation.PopAsync();
					}
				}
				CS$<>8__locals1 = null;
			}
			catch (Exception)
			{
			}
			this.activityFrame.IsVisible = false;
			base.IsEnabled = true;
		}

		// Token: 0x06001B23 RID: 6947 RVA: 0x000027D4 File Offset: 0x000009D4
		private void Page_Disappearing(object sender, EventArgs e)
		{
		}

		// Token: 0x06001B24 RID: 6948 RVA: 0x0012E9F0 File Offset: 0x0012CBF0
		private void lv_ItemSelected(object sender, SelectedItemChangedEventArgs e)
		{
			if (this.lv.SelectedItem == null || this.lv.SelectedItem == this.garageModel.CurrentCar)
			{
				this.btnChangeCar.IsEnabled = false;
				this.btnDelCar.IsEnabled = false;
				return;
			}
			this.btnChangeCar.IsEnabled = true;
			this.btnDelCar.IsEnabled = true;
		}

		// Token: 0x06001B25 RID: 6949 RVA: 0x0012EA54 File Offset: 0x0012CC54
		private async void grid_Tapped(object sender, EventArgs e)
		{
			MyCar myCar = (MyCar)((Element)sender).BindingContext;
			if (this.lv.SelectedItem != myCar)
			{
				this.lv.SelectedItem = myCar;
			}
		}

		// Token: 0x06001B26 RID: 6950 RVA: 0x0012EA93 File Offset: 0x0012CC93
		private void grid_DoubleTapped(object sender, EventArgs e)
		{
			this.grid_Tapped(sender, e);
			this.btnChangeCar_Clicked(sender, e);
		}

		// Token: 0x06001B27 RID: 6951 RVA: 0x0012EAA8 File Offset: 0x0012CCA8
		private async void Lv_ItemTapped(object sender, ItemTappedEventArgs e)
		{
		}

		// Token: 0x06001B28 RID: 6952 RVA: 0x0012EAD8 File Offset: 0x0012CCD8
		private async void Lv_ItemDoubleTapped(object sender, ItemDoubleTappedEventArgs e)
		{
			object itemData = e.ItemData;
			this.btnChangeCar_Clicked(sender, e);
		}

		// Token: 0x06001B29 RID: 6953 RVA: 0x0012EB20 File Offset: 0x0012CD20
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private void InitializeComponent()
		{
			if (ResourceLoader.IsEnabled && ResourceLoader.CanProvideContentFor(new ResourceLoader.ResourceLoadingQuery
			{
				AssemblyName = typeof(SettingsGarage).GetTypeInfo().Assembly.GetName(),
				ResourcePath = "Settings/SettingsGarage.xaml",
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
			VisualDiagnostics.RegisterSourceInfo(translate = new Translate(), new Uri("Settings\\SettingsGarage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 10, 5);
			DynamicResourceExtension dynamicResourceExtension;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension = new DynamicResourceExtension(), new Uri("Settings\\SettingsGarage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 14, 5);
			RowDefinition rowDefinition;
			VisualDiagnostics.RegisterSourceInfo(rowDefinition = new RowDefinition(), new Uri("Settings\\SettingsGarage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 20, 18);
			RowDefinition rowDefinition2;
			VisualDiagnostics.RegisterSourceInfo(rowDefinition2 = new RowDefinition(), new Uri("Settings\\SettingsGarage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 21, 18);
			RowDefinition rowDefinition3;
			VisualDiagnostics.RegisterSourceInfo(rowDefinition3 = new RowDefinition(), new Uri("Settings\\SettingsGarage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 22, 18);
			Label label;
			VisualDiagnostics.RegisterSourceInfo(label = new Label(), new Uri("Settings\\SettingsGarage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 24, 14);
			DynamicResourceExtension dynamicResourceExtension2;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension2 = new DynamicResourceExtension(), new Uri("Settings\\SettingsGarage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 86, 17);
			DataTemplate dataTemplate;
			VisualDiagnostics.RegisterSourceInfo(dataTemplate = new DataTemplate(), new Uri("Settings\\SettingsGarage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 89, 22);
			DataTemplate dataTemplate2;
			VisualDiagnostics.RegisterSourceInfo(dataTemplate2 = new DataTemplate(), new Uri("Settings\\SettingsGarage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 124, 22);
			SfListView sfListView;
			VisualDiagnostics.RegisterSourceInfo(sfListView = new SfListView(), new Uri("Settings\\SettingsGarage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 78, 14);
			ActivityFrame activityFrame;
			VisualDiagnostics.RegisterSourceInfo(activityFrame = new ActivityFrame(), new Uri("Settings\\SettingsGarage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 161, 14);
			ColumnDefinition columnDefinition;
			VisualDiagnostics.RegisterSourceInfo(columnDefinition = new ColumnDefinition(), new Uri("Settings\\SettingsGarage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 168, 22);
			ColumnDefinition columnDefinition2;
			VisualDiagnostics.RegisterSourceInfo(columnDefinition2 = new ColumnDefinition(), new Uri("Settings\\SettingsGarage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 169, 22);
			ColumnDefinition columnDefinition3;
			VisualDiagnostics.RegisterSourceInfo(columnDefinition3 = new ColumnDefinition(), new Uri("Settings\\SettingsGarage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 170, 22);
			Translate translate2;
			VisualDiagnostics.RegisterSourceInfo(translate2 = new Translate(), new Uri("Settings\\SettingsGarage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 177, 21);
			Button button;
			VisualDiagnostics.RegisterSourceInfo(button = new Button(), new Uri("Settings\\SettingsGarage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 172, 18);
			Translate translate3;
			VisualDiagnostics.RegisterSourceInfo(translate3 = new Translate(), new Uri("Settings\\SettingsGarage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 184, 21);
			Button button2;
			VisualDiagnostics.RegisterSourceInfo(button2 = new Button(), new Uri("Settings\\SettingsGarage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 179, 18);
			DynamicResourceExtension dynamicResourceExtension3;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension3 = new DynamicResourceExtension(), new Uri("Settings\\SettingsGarage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 190, 21);
			Translate translate4;
			VisualDiagnostics.RegisterSourceInfo(translate4 = new Translate(), new Uri("Settings\\SettingsGarage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 192, 21);
			Button button3;
			VisualDiagnostics.RegisterSourceInfo(button3 = new Button(), new Uri("Settings\\SettingsGarage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 187, 18);
			Grid grid;
			VisualDiagnostics.RegisterSourceInfo(grid = new Grid(), new Uri("Settings\\SettingsGarage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 166, 14);
			Grid grid2;
			VisualDiagnostics.RegisterSourceInfo(grid2 = new Grid(), new Uri("Settings\\SettingsGarage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 18, 10);
			VisualDiagnostics.RegisterSourceInfo(this, new Uri("Settings\\SettingsGarage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 2, 2);
			NameScope nameScope = NameScope.GetNameScope(this) ?? new NameScope();
			NameScope.SetNameScope(this, nameScope);
			nameScope.RegisterName("labelCurrentCar", label);
			if (label.StyleId == null)
			{
				label.StyleId = "labelCurrentCar";
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
			nameScope.RegisterName("btnChangeCar", button);
			if (button.StyleId == null)
			{
				button.StyleId = "btnChangeCar";
			}
			nameScope.RegisterName("btnEditCar", button2);
			if (button2.StyleId == null)
			{
				button2.StyleId = "btnEditCar";
			}
			nameScope.RegisterName("btnDelCar", button3);
			if (button3.StyleId == null)
			{
				button3.StyleId = "btnDelCar";
			}
			this.labelCurrentCar = label;
			this.lv = sfListView;
			this.activityFrame = activityFrame;
			this.btnChangeCar = button;
			this.btnEditCar = button2;
			this.btnDelCar = button3;
			translate.Text = "ios_GarageTitle";
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
			xmlNamespaceResolver.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xmlNamespaceResolver.Add("syncfusion", "clr-namespace:Syncfusion.ListView.XForms;assembly=Syncfusion.SfListView.XForms");
			xamlServiceProvider.Add(typeFromHandle2, new XamlTypeResolver(xmlNamespaceResolver, typeof(SettingsGarage).GetTypeInfo().Assembly));
			xamlServiceProvider.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(10, 5)));
			object obj2 = markupExtension.ProvideValue(xamlServiceProvider);
			this.Title = obj2;
			this.SetValue(Page.PaddingProperty, new Thickness(0.0));
			this.SetValue(Page.UseSafeAreaProperty, true);
			this.Appearing += this.ContentPage_Appearing;
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
			xmlNamespaceResolver2.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver2.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver2.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xmlNamespaceResolver2.Add("syncfusion", "clr-namespace:Syncfusion.ListView.XForms;assembly=Syncfusion.SfListView.XForms");
			xamlServiceProvider2.Add(typeFromHandle4, new XamlTypeResolver(xmlNamespaceResolver2, typeof(SettingsGarage).GetTypeInfo().Assembly));
			xamlServiceProvider2.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(14, 5)));
			DynamicResource dynamicResource = markupExtension2.ProvideValue(xamlServiceProvider2);
			this.SetDynamicResource(VisualElement.BackgroundColorProperty, dynamicResource.Key);
			this.Disappearing += this.Page_Disappearing;
			this.SizeChanged += this.Handle_SizeChanged;
			grid2.SetValue(View.MarginProperty, new Thickness(5.0, 0.0, 5.0, 0.0));
			rowDefinition.SetValue(RowDefinition.HeightProperty, new GridLengthTypeConverter().ConvertFromInvariantString("auto"));
			grid2.GetValue(Grid.RowDefinitionsProperty).Add(rowDefinition);
			rowDefinition2.SetValue(RowDefinition.HeightProperty, new GridLengthTypeConverter().ConvertFromInvariantString("*"));
			grid2.GetValue(Grid.RowDefinitionsProperty).Add(rowDefinition2);
			rowDefinition3.SetValue(RowDefinition.HeightProperty, new GridLengthTypeConverter().ConvertFromInvariantString("auto"));
			grid2.GetValue(Grid.RowDefinitionsProperty).Add(rowDefinition3);
			label.SetValue(Label.FontAttributesProperty, new FontAttributesConverter().ConvertFromInvariantString("Bold"));
			label.SetValue(View.HorizontalOptionsProperty, LayoutOptions.Center);
			label.SetValue(Label.HorizontalTextAlignmentProperty, new TextAlignmentConverter().ConvertFromInvariantString("Center"));
			label.SetValue(Label.TextProperty, "");
			grid2.Children.Add(label);
			sfListView.SetValue(Grid.RowProperty, 1);
			sfListView.SetValue(SfListView.AutoFitModeProperty, 0);
			sfListView.SetValue(VisualElement.BackgroundColorProperty, Color.Transparent);
			sfListView.ItemDoubleTapped += new ItemDoubleTappedEventHandler(this.Lv_ItemDoubleTapped);
			sfListView.SetValue(SfListView.ItemSpacingProperty, new Thickness(2.0));
			sfListView.ItemTapped += new ItemTappedEventHandler(this.Lv_ItemTapped);
			dynamicResourceExtension2.Key = "ButtonAccentColor";
			IMarkupExtension<DynamicResource> markupExtension3 = dynamicResourceExtension2;
			XamlServiceProvider xamlServiceProvider3 = new XamlServiceProvider();
			Type typeFromHandle5 = typeof(IProvideValueTarget);
			object[] array3 = new object[0 + 3];
			array3[0] = sfListView;
			array3[1] = grid2;
			array3[2] = this;
			object obj4;
			xamlServiceProvider3.Add(typeFromHandle5, obj4 = new SimpleValueTargetProvider(array3, SfListView.SelectionBackgroundColorProperty, nameScope));
			xamlServiceProvider3.Add(typeof(IReferenceProvider), obj4);
			Type typeFromHandle6 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver3 = new XmlNamespaceResolver();
			xmlNamespaceResolver3.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver3.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver3.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver3.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver3.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xmlNamespaceResolver3.Add("syncfusion", "clr-namespace:Syncfusion.ListView.XForms;assembly=Syncfusion.SfListView.XForms");
			xamlServiceProvider3.Add(typeFromHandle6, new XamlTypeResolver(xmlNamespaceResolver3, typeof(SettingsGarage).GetTypeInfo().Assembly));
			xamlServiceProvider3.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(86, 17)));
			DynamicResource dynamicResource2 = markupExtension3.ProvideValue(xamlServiceProvider3);
			sfListView.SetDynamicResource(SfListView.SelectionBackgroundColorProperty, dynamicResource2.Key);
			sfListView.SetValue(SfListView.SelectionModeProperty, 0);
			IDataTemplate dataTemplate3 = dataTemplate;
			SettingsGarage.<InitializeComponent>_anonXamlCDataTemplate_93 <InitializeComponent>_anonXamlCDataTemplate_ = new SettingsGarage.<InitializeComponent>_anonXamlCDataTemplate_93();
			object[] array4 = new object[0 + 4];
			array4[0] = dataTemplate;
			array4[1] = sfListView;
			array4[2] = grid2;
			array4[3] = this;
			<InitializeComponent>_anonXamlCDataTemplate_.parentValues = array4;
			<InitializeComponent>_anonXamlCDataTemplate_.root = this;
			dataTemplate3.LoadTemplate = new Func<object>(<InitializeComponent>_anonXamlCDataTemplate_.LoadDataTemplate);
			sfListView.SetValue(SfListView.SelectedItemTemplateProperty, dataTemplate);
			IDataTemplate dataTemplate4 = dataTemplate2;
			SettingsGarage.<InitializeComponent>_anonXamlCDataTemplate_94 <InitializeComponent>_anonXamlCDataTemplate_2 = new SettingsGarage.<InitializeComponent>_anonXamlCDataTemplate_94();
			object[] array5 = new object[0 + 4];
			array5[0] = dataTemplate2;
			array5[1] = sfListView;
			array5[2] = grid2;
			array5[3] = this;
			<InitializeComponent>_anonXamlCDataTemplate_2.parentValues = array5;
			<InitializeComponent>_anonXamlCDataTemplate_2.root = this;
			dataTemplate4.LoadTemplate = new Func<object>(<InitializeComponent>_anonXamlCDataTemplate_2.LoadDataTemplate);
			sfListView.SetValue(SfListView.ItemTemplateProperty, dataTemplate2);
			grid2.Children.Add(sfListView);
			activityFrame.SetValue(Grid.RowProperty, 1);
			activityFrame.SetValue(VisualElement.IsVisibleProperty, new VisualElement.VisibilityConverter().ConvertFromInvariantString("false"));
			activityFrame.SetValue(View.VerticalOptionsProperty, LayoutOptions.Start);
			grid2.Children.Add(activityFrame);
			grid.SetValue(Grid.RowProperty, 2);
			columnDefinition.SetValue(ColumnDefinition.WidthProperty, new GridLengthTypeConverter().ConvertFromInvariantString("2*"));
			grid.GetValue(Grid.ColumnDefinitionsProperty).Add(columnDefinition);
			columnDefinition2.SetValue(ColumnDefinition.WidthProperty, new GridLengthTypeConverter().ConvertFromInvariantString("1*"));
			grid.GetValue(Grid.ColumnDefinitionsProperty).Add(columnDefinition2);
			columnDefinition3.SetValue(ColumnDefinition.WidthProperty, new GridLengthTypeConverter().ConvertFromInvariantString("1*"));
			grid.GetValue(Grid.ColumnDefinitionsProperty).Add(columnDefinition3);
			button.SetValue(Grid.ColumnProperty, 0);
			button.SetValue(VisualElement.BackgroundColorProperty, Color.DarkGreen);
			button.Clicked += this.btnChangeCar_Clicked;
			translate2.Text = "ios_SelectCar";
			IMarkupExtension markupExtension4 = translate2;
			XamlServiceProvider xamlServiceProvider4 = new XamlServiceProvider();
			Type typeFromHandle7 = typeof(IProvideValueTarget);
			object[] array6 = new object[0 + 4];
			array6[0] = button;
			array6[1] = grid;
			array6[2] = grid2;
			array6[3] = this;
			object obj5;
			xamlServiceProvider4.Add(typeFromHandle7, obj5 = new SimpleValueTargetProvider(array6, Button.TextProperty, nameScope));
			xamlServiceProvider4.Add(typeof(IReferenceProvider), obj5);
			Type typeFromHandle8 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver4 = new XmlNamespaceResolver();
			xmlNamespaceResolver4.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver4.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver4.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver4.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver4.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xmlNamespaceResolver4.Add("syncfusion", "clr-namespace:Syncfusion.ListView.XForms;assembly=Syncfusion.SfListView.XForms");
			xamlServiceProvider4.Add(typeFromHandle8, new XamlTypeResolver(xmlNamespaceResolver4, typeof(SettingsGarage).GetTypeInfo().Assembly));
			xamlServiceProvider4.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(177, 21)));
			object obj6 = markupExtension4.ProvideValue(xamlServiceProvider4);
			button.Text = obj6;
			button.SetValue(Button.TextColorProperty, Color.White);
			grid.Children.Add(button);
			button2.SetValue(Grid.ColumnProperty, 1);
			button2.SetValue(VisualElement.BackgroundColorProperty, Color.DarkBlue);
			button2.Clicked += this.btnEditCar_Clicked;
			translate3.Text = "ios_EditCar";
			IMarkupExtension markupExtension5 = translate3;
			XamlServiceProvider xamlServiceProvider5 = new XamlServiceProvider();
			Type typeFromHandle9 = typeof(IProvideValueTarget);
			object[] array7 = new object[0 + 4];
			array7[0] = button2;
			array7[1] = grid;
			array7[2] = grid2;
			array7[3] = this;
			object obj7;
			xamlServiceProvider5.Add(typeFromHandle9, obj7 = new SimpleValueTargetProvider(array7, Button.TextProperty, nameScope));
			xamlServiceProvider5.Add(typeof(IReferenceProvider), obj7);
			Type typeFromHandle10 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver5 = new XmlNamespaceResolver();
			xmlNamespaceResolver5.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver5.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver5.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver5.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver5.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xmlNamespaceResolver5.Add("syncfusion", "clr-namespace:Syncfusion.ListView.XForms;assembly=Syncfusion.SfListView.XForms");
			xamlServiceProvider5.Add(typeFromHandle10, new XamlTypeResolver(xmlNamespaceResolver5, typeof(SettingsGarage).GetTypeInfo().Assembly));
			xamlServiceProvider5.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(184, 21)));
			object obj8 = markupExtension5.ProvideValue(xamlServiceProvider5);
			button2.Text = obj8;
			button2.SetValue(Button.TextColorProperty, Color.White);
			grid.Children.Add(button2);
			button3.SetValue(Grid.ColumnProperty, 2);
			dynamicResourceExtension3.Key = "ButtonRedColor";
			IMarkupExtension<DynamicResource> markupExtension6 = dynamicResourceExtension3;
			XamlServiceProvider xamlServiceProvider6 = new XamlServiceProvider();
			Type typeFromHandle11 = typeof(IProvideValueTarget);
			object[] array8 = new object[0 + 4];
			array8[0] = button3;
			array8[1] = grid;
			array8[2] = grid2;
			array8[3] = this;
			object obj9;
			xamlServiceProvider6.Add(typeFromHandle11, obj9 = new SimpleValueTargetProvider(array8, VisualElement.BackgroundColorProperty, nameScope));
			xamlServiceProvider6.Add(typeof(IReferenceProvider), obj9);
			Type typeFromHandle12 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver6 = new XmlNamespaceResolver();
			xmlNamespaceResolver6.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver6.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver6.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver6.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver6.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xmlNamespaceResolver6.Add("syncfusion", "clr-namespace:Syncfusion.ListView.XForms;assembly=Syncfusion.SfListView.XForms");
			xamlServiceProvider6.Add(typeFromHandle12, new XamlTypeResolver(xmlNamespaceResolver6, typeof(SettingsGarage).GetTypeInfo().Assembly));
			xamlServiceProvider6.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(190, 21)));
			DynamicResource dynamicResource3 = markupExtension6.ProvideValue(xamlServiceProvider6);
			button3.SetDynamicResource(VisualElement.BackgroundColorProperty, dynamicResource3.Key);
			button3.Clicked += new EventHandler(this.btnDelCar_Tapped);
			translate4.Text = "SpeedTest_btnRemove.Label";
			IMarkupExtension markupExtension7 = translate4;
			XamlServiceProvider xamlServiceProvider7 = new XamlServiceProvider();
			Type typeFromHandle13 = typeof(IProvideValueTarget);
			object[] array9 = new object[0 + 4];
			array9[0] = button3;
			array9[1] = grid;
			array9[2] = grid2;
			array9[3] = this;
			object obj10;
			xamlServiceProvider7.Add(typeFromHandle13, obj10 = new SimpleValueTargetProvider(array9, Button.TextProperty, nameScope));
			xamlServiceProvider7.Add(typeof(IReferenceProvider), obj10);
			Type typeFromHandle14 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver7 = new XmlNamespaceResolver();
			xmlNamespaceResolver7.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver7.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver7.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver7.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver7.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xmlNamespaceResolver7.Add("syncfusion", "clr-namespace:Syncfusion.ListView.XForms;assembly=Syncfusion.SfListView.XForms");
			xamlServiceProvider7.Add(typeFromHandle14, new XamlTypeResolver(xmlNamespaceResolver7, typeof(SettingsGarage).GetTypeInfo().Assembly));
			xamlServiceProvider7.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(192, 21)));
			object obj11 = markupExtension7.ProvideValue(xamlServiceProvider7);
			button3.Text = obj11;
			button3.SetValue(Button.TextColorProperty, Color.White);
			grid.Children.Add(button3);
			grid2.Children.Add(grid);
			this.SetValue(ContentPage.ContentProperty, grid2);
		}

		// Token: 0x06001B2A RID: 6954 RVA: 0x0012FD8D File Offset: 0x0012DF8D
		[CompilerGenerated]
		private void <.ctor>b__0_0()
		{
			this.btnNew_Clicked(null, null);
		}

		// Token: 0x06001B2B RID: 6955 RVA: 0x0012FD97 File Offset: 0x0012DF97
		[CompilerGenerated]
		private void <.ctor>b__0_1()
		{
			this.DeleteSelected();
		}

		// Token: 0x06001B2C RID: 6956 RVA: 0x0012FD9F File Offset: 0x0012DF9F
		[CompilerGenerated]
		private void <.ctor>b__0_2()
		{
			this.btnInfo_Clicked(null, null);
		}

		// Token: 0x06001B2D RID: 6957 RVA: 0x0012FDAC File Offset: 0x0012DFAC
		private void __InitComponentRuntime()
		{
			Extensions.LoadFromXaml<SettingsGarage>(this, typeof(SettingsGarage));
			this.labelCurrentCar = NameScopeExtensions.FindByName<Label>(this, "labelCurrentCar");
			this.lv = NameScopeExtensions.FindByName<SfListView>(this, "lv");
			this.activityFrame = NameScopeExtensions.FindByName<ActivityFrame>(this, "activityFrame");
			this.btnChangeCar = NameScopeExtensions.FindByName<Button>(this, "btnChangeCar");
			this.btnEditCar = NameScopeExtensions.FindByName<Button>(this, "btnEditCar");
			this.btnDelCar = NameScopeExtensions.FindByName<Button>(this, "btnDelCar");
		}

		// Token: 0x04000C75 RID: 3189
		private ToolbarItem ti_add;

		// Token: 0x04000C76 RID: 3190
		private ToolbarItem ti_del;

		// Token: 0x04000C77 RID: 3191
		private ToolbarItem ti_info;

		// Token: 0x04000C78 RID: 3192
		private GarageModel garageModel;

		// Token: 0x04000C79 RID: 3193
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private Label labelCurrentCar;

		// Token: 0x04000C7A RID: 3194
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private SfListView lv;

		// Token: 0x04000C7B RID: 3195
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private ActivityFrame activityFrame;

		// Token: 0x04000C7C RID: 3196
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private Button btnChangeCar;

		// Token: 0x04000C7D RID: 3197
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private Button btnEditCar;

		// Token: 0x04000C7E RID: 3198
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private Button btnDelCar;

		// Token: 0x02000234 RID: 564
		[CompilerGenerated]
		private sealed class <>c__DisplayClass13_0
		{
			// Token: 0x06001B2E RID: 6958 RVA: 0x00002050 File Offset: 0x00000250
			public <>c__DisplayClass13_0()
			{
			}

			// Token: 0x06001B2F RID: 6959 RVA: 0x0012FE30 File Offset: 0x0012E030
			internal Tuple<bool, string> <btnChangeCar_Clicked>b__0()
			{
				return this.apply_result = this.car.ApplyToSettings();
			}

			// Token: 0x04000C7F RID: 3199
			public MyCar car;

			// Token: 0x04000C80 RID: 3200
			public Tuple<bool, string> apply_result;
		}

		// Token: 0x02000235 RID: 565
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <DeleteSelected>d__6 : IAsyncStateMachine
		{
			// Token: 0x06001B30 RID: 6960 RVA: 0x0012FE54 File Offset: 0x0012E054
			void IAsyncStateMachine.MoveNext()
			{
				SettingsGarage settingsGarage = this;
				try
				{
					if (settingsGarage.lv.SelectedItem != null)
					{
						MyCar myCar = settingsGarage.lv.SelectedItem as MyCar;
						settingsGarage.garageModel.Cars.Remove(myCar);
						settingsGarage.garageModel.SaveToFile();
					}
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

			// Token: 0x06001B31 RID: 6961 RVA: 0x0012FEE0 File Offset: 0x0012E0E0
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x04000C81 RID: 3201
			public int <>1__state;

			// Token: 0x04000C82 RID: 3202
			public AsyncVoidMethodBuilder <>t__builder;

			// Token: 0x04000C83 RID: 3203
			public SettingsGarage <>4__this;
		}

		// Token: 0x02000236 RID: 566
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <LoadList>d__5 : IAsyncStateMachine
		{
			// Token: 0x06001B32 RID: 6962 RVA: 0x0012FEF0 File Offset: 0x0012E0F0
			void IAsyncStateMachine.MoveNext()
			{
				SettingsGarage settingsGarage = this;
				try
				{
					settingsGarage.activityFrame.IsVisible = true;
					settingsGarage.garageModel = new GarageModel();
					settingsGarage.garageModel.Initialize(true);
					if (settingsGarage.garageModel.CurrentCar != null && settingsGarage.garageModel.CurrentCar.Name != null)
					{
						settingsGarage.labelCurrentCar.Text = Translate.GetString("ios_CurrentCar") + settingsGarage.garageModel.CurrentCar.Name;
					}
					else
					{
						settingsGarage.labelCurrentCar.Text = Translate.GetString("ios_CurrentCar");
					}
					settingsGarage.lv.ItemsSource = settingsGarage.garageModel.Cars;
					settingsGarage.lv.SelectedItem = settingsGarage.garageModel.CurrentCar;
					settingsGarage.activityFrame.IsVisible = false;
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

			// Token: 0x06001B33 RID: 6963 RVA: 0x0012FFFC File Offset: 0x0012E1FC
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x04000C84 RID: 3204
			public int <>1__state;

			// Token: 0x04000C85 RID: 3205
			public AsyncVoidMethodBuilder <>t__builder;

			// Token: 0x04000C86 RID: 3206
			public SettingsGarage <>4__this;
		}

		// Token: 0x02000237 RID: 567
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <Lv_ItemDoubleTapped>d__19 : IAsyncStateMachine
		{
			// Token: 0x06001B34 RID: 6964 RVA: 0x0013000C File Offset: 0x0012E20C
			void IAsyncStateMachine.MoveNext()
			{
				SettingsGarage settingsGarage = this;
				try
				{
					object itemData = e.ItemData;
					settingsGarage.btnChangeCar_Clicked(sender, e);
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

			// Token: 0x06001B35 RID: 6965 RVA: 0x0013007C File Offset: 0x0012E27C
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x04000C87 RID: 3207
			public int <>1__state;

			// Token: 0x04000C88 RID: 3208
			public AsyncVoidMethodBuilder <>t__builder;

			// Token: 0x04000C89 RID: 3209
			public ItemDoubleTappedEventArgs e;

			// Token: 0x04000C8A RID: 3210
			public SettingsGarage <>4__this;

			// Token: 0x04000C8B RID: 3211
			public object sender;
		}

		// Token: 0x02000238 RID: 568
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <Lv_ItemTapped>d__18 : IAsyncStateMachine
		{
			// Token: 0x06001B36 RID: 6966 RVA: 0x0013008C File Offset: 0x0012E28C
			void IAsyncStateMachine.MoveNext()
			{
				try
				{
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

			// Token: 0x06001B37 RID: 6967 RVA: 0x001300D8 File Offset: 0x0012E2D8
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x04000C8C RID: 3212
			public int <>1__state;

			// Token: 0x04000C8D RID: 3213
			public AsyncVoidMethodBuilder <>t__builder;
		}

		// Token: 0x02000239 RID: 569
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <btnChangeCar_Clicked>d__13 : IAsyncStateMachine
		{
			// Token: 0x06001B38 RID: 6968 RVA: 0x001300E8 File Offset: 0x0012E2E8
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				SettingsGarage settingsGarage = this;
				try
				{
					if (num > 3)
					{
						settingsGarage.IsEnabled = false;
					}
					try
					{
						TaskAwaiter<Tuple<bool, string>> taskAwaiter;
						TaskAwaiter taskAwaiter3;
						TaskAwaiter<Page> taskAwaiter5;
						switch (num)
						{
						case 0:
						{
							TaskAwaiter<Tuple<bool, string>> taskAwaiter2;
							taskAwaiter = taskAwaiter2;
							taskAwaiter2 = default(TaskAwaiter<Tuple<bool, string>>);
							num2 = -1;
							break;
						}
						case 1:
						{
							TaskAwaiter taskAwaiter4;
							taskAwaiter3 = taskAwaiter4;
							taskAwaiter4 = default(TaskAwaiter);
							num2 = -1;
							goto IL_0182;
						}
						case 2:
						{
							TaskAwaiter<Page> taskAwaiter6;
							taskAwaiter5 = taskAwaiter6;
							taskAwaiter6 = default(TaskAwaiter<Page>);
							num2 = -1;
							goto IL_01E5;
						}
						case 3:
						{
							TaskAwaiter<Page> taskAwaiter6;
							taskAwaiter5 = taskAwaiter6;
							taskAwaiter6 = default(TaskAwaiter<Page>);
							num2 = -1;
							goto IL_025A;
						}
						default:
							CS$<>8__locals1 = new SettingsGarage.<>c__DisplayClass13_0();
							CS$<>8__locals1.car = settingsGarage.lv.SelectedItem as MyCar;
							if (CS$<>8__locals1.car == settingsGarage.garageModel.CurrentCar)
							{
								goto IL_0262;
							}
							settingsGarage.activityFrame.IsVisible = true;
							CS$<>8__locals1.apply_result = new Tuple<bool, string>(false, "");
							taskAwaiter = Task.Run<Tuple<bool, string>>(() => CS$<>8__locals1.apply_result = CS$<>8__locals1.car.ApplyToSettings()).GetAwaiter();
							if (!taskAwaiter.IsCompleted)
							{
								num2 = 0;
								TaskAwaiter<Tuple<bool, string>> taskAwaiter2 = taskAwaiter;
								this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<Tuple<bool, string>>, SettingsGarage.<btnChangeCar_Clicked>d__13>(ref taskAwaiter, ref this);
								return;
							}
							break;
						}
						taskAwaiter.GetResult();
						if (!CS$<>8__locals1.apply_result.Item1)
						{
							taskAwaiter3 = settingsGarage.DisplayAlert("Error changing car", CS$<>8__locals1.apply_result.Item2, "OK").GetAwaiter();
							if (!taskAwaiter3.IsCompleted)
							{
								num2 = 1;
								TaskAwaiter taskAwaiter4 = taskAwaiter3;
								this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, SettingsGarage.<btnChangeCar_Clicked>d__13>(ref taskAwaiter3, ref this);
								return;
							}
						}
						else
						{
							if (!CS$<>8__locals1.apply_result.Item1)
							{
								goto IL_0262;
							}
							taskAwaiter5 = settingsGarage.Navigation.PopAsync().GetAwaiter();
							if (!taskAwaiter5.IsCompleted)
							{
								num2 = 3;
								TaskAwaiter<Page> taskAwaiter6 = taskAwaiter5;
								this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<Page>, SettingsGarage.<btnChangeCar_Clicked>d__13>(ref taskAwaiter5, ref this);
								return;
							}
							goto IL_025A;
						}
						IL_0182:
						taskAwaiter3.GetResult();
						taskAwaiter5 = settingsGarage.Navigation.PopAsync().GetAwaiter();
						if (!taskAwaiter5.IsCompleted)
						{
							num2 = 2;
							TaskAwaiter<Page> taskAwaiter6 = taskAwaiter5;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<Page>, SettingsGarage.<btnChangeCar_Clicked>d__13>(ref taskAwaiter5, ref this);
							return;
						}
						IL_01E5:
						taskAwaiter5.GetResult();
						goto IL_0262;
						IL_025A:
						taskAwaiter5.GetResult();
						IL_0262:
						CS$<>8__locals1 = null;
					}
					catch (Exception)
					{
					}
					settingsGarage.activityFrame.IsVisible = false;
					settingsGarage.IsEnabled = true;
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

			// Token: 0x06001B39 RID: 6969 RVA: 0x001303D8 File Offset: 0x0012E5D8
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x04000C8E RID: 3214
			public int <>1__state;

			// Token: 0x04000C8F RID: 3215
			public AsyncVoidMethodBuilder <>t__builder;

			// Token: 0x04000C90 RID: 3216
			public SettingsGarage <>4__this;

			// Token: 0x04000C91 RID: 3217
			private SettingsGarage.<>c__DisplayClass13_0 <>8__1;

			// Token: 0x04000C92 RID: 3218
			private TaskAwaiter<Tuple<bool, string>> <>u__1;

			// Token: 0x04000C93 RID: 3219
			private TaskAwaiter <>u__2;

			// Token: 0x04000C94 RID: 3220
			private TaskAwaiter<Page> <>u__3;
		}

		// Token: 0x0200023A RID: 570
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <btnDelCar_Tapped>d__7 : IAsyncStateMachine
		{
			// Token: 0x06001B3A RID: 6970 RVA: 0x001303E8 File Offset: 0x0012E5E8
			void IAsyncStateMachine.MoveNext()
			{
				SettingsGarage settingsGarage = this;
				try
				{
					settingsGarage.DeleteSelected();
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

			// Token: 0x06001B3B RID: 6971 RVA: 0x00130440 File Offset: 0x0012E640
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x04000C95 RID: 3221
			public int <>1__state;

			// Token: 0x04000C96 RID: 3222
			public AsyncVoidMethodBuilder <>t__builder;

			// Token: 0x04000C97 RID: 3223
			public SettingsGarage <>4__this;
		}

		// Token: 0x0200023B RID: 571
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <btnEditCar_Clicked>d__12 : IAsyncStateMachine
		{
			// Token: 0x06001B3C RID: 6972 RVA: 0x00130450 File Offset: 0x0012E650
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				SettingsGarage settingsGarage = this;
				try
				{
					TaskAwaiter taskAwaiter;
					if (num != 0)
					{
						MyCar myCar = settingsGarage.lv.SelectedItem as MyCar;
						if (myCar == null)
						{
							goto IL_0093;
						}
						SettingsGarageCarEditorPage settingsGarageCarEditorPage = new SettingsGarageCarEditorPage(myCar, settingsGarage.garageModel);
						taskAwaiter = settingsGarage.Navigation.PushAsync(settingsGarageCarEditorPage).GetAwaiter();
						if (!taskAwaiter.IsCompleted)
						{
							num2 = 0;
							TaskAwaiter taskAwaiter2 = taskAwaiter;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, SettingsGarage.<btnEditCar_Clicked>d__12>(ref taskAwaiter, ref this);
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
					IL_0093:;
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

			// Token: 0x06001B3D RID: 6973 RVA: 0x00130530 File Offset: 0x0012E730
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x04000C98 RID: 3224
			public int <>1__state;

			// Token: 0x04000C99 RID: 3225
			public AsyncVoidMethodBuilder <>t__builder;

			// Token: 0x04000C9A RID: 3226
			public SettingsGarage <>4__this;

			// Token: 0x04000C9B RID: 3227
			private TaskAwaiter <>u__1;
		}

		// Token: 0x0200023C RID: 572
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <btnNew_Clicked>d__8 : IAsyncStateMachine
		{
			// Token: 0x06001B3E RID: 6974 RVA: 0x00130540 File Offset: 0x0012E740
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				SettingsGarage settingsGarage = this;
				try
				{
					TaskAwaiter taskAwaiter;
					if (num != 0)
					{
						SettingsGarageCarEditorPage settingsGarageCarEditorPage = new SettingsGarageCarEditorPage(null, settingsGarage.garageModel);
						taskAwaiter = settingsGarage.Navigation.PushAsync(settingsGarageCarEditorPage).GetAwaiter();
						if (!taskAwaiter.IsCompleted)
						{
							num2 = 0;
							TaskAwaiter taskAwaiter2 = taskAwaiter;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, SettingsGarage.<btnNew_Clicked>d__8>(ref taskAwaiter, ref this);
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

			// Token: 0x06001B3F RID: 6975 RVA: 0x00130608 File Offset: 0x0012E808
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x04000C9C RID: 3228
			public int <>1__state;

			// Token: 0x04000C9D RID: 3229
			public AsyncVoidMethodBuilder <>t__builder;

			// Token: 0x04000C9E RID: 3230
			public SettingsGarage <>4__this;

			// Token: 0x04000C9F RID: 3231
			private TaskAwaiter <>u__1;
		}

		// Token: 0x0200023D RID: 573
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <grid_Tapped>d__16 : IAsyncStateMachine
		{
			// Token: 0x06001B40 RID: 6976 RVA: 0x00130618 File Offset: 0x0012E818
			void IAsyncStateMachine.MoveNext()
			{
				SettingsGarage settingsGarage = this;
				try
				{
					MyCar myCar = (MyCar)((Element)sender).BindingContext;
					if (settingsGarage.lv.SelectedItem != myCar)
					{
						settingsGarage.lv.SelectedItem = myCar;
					}
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

			// Token: 0x06001B41 RID: 6977 RVA: 0x00130698 File Offset: 0x0012E898
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x04000CA0 RID: 3232
			public int <>1__state;

			// Token: 0x04000CA1 RID: 3233
			public AsyncVoidMethodBuilder <>t__builder;

			// Token: 0x04000CA2 RID: 3234
			public object sender;

			// Token: 0x04000CA3 RID: 3235
			public SettingsGarage <>4__this;
		}

		// Token: 0x0200023E RID: 574
		[CompilerGenerated]
		private sealed class <InitializeComponent>_anonXamlCDataTemplate_93
		{
			// Token: 0x06001B42 RID: 6978 RVA: 0x001306A8 File Offset: 0x0012E8A8
			public <InitializeComponent>_anonXamlCDataTemplate_93()
			{
			}

			// Token: 0x06001B43 RID: 6979 RVA: 0x001306BC File Offset: 0x0012E8BC
			internal object LoadDataTemplate()
			{
				DynamicResourceExtension dynamicResourceExtension;
				VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension = new DynamicResourceExtension(), new Uri("Settings\\SettingsGarage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 90, 31);
				RowDefinition rowDefinition;
				VisualDiagnostics.RegisterSourceInfo(rowDefinition = new RowDefinition(), new Uri("Settings\\SettingsGarage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 92, 34);
				RowDefinition rowDefinition2;
				VisualDiagnostics.RegisterSourceInfo(rowDefinition2 = new RowDefinition(), new Uri("Settings\\SettingsGarage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 93, 34);
				StaticResourceExtension staticResourceExtension;
				VisualDiagnostics.RegisterSourceInfo(staticResourceExtension = new StaticResourceExtension(), new Uri("Settings\\SettingsGarage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 97, 33);
				BindingExtension bindingExtension;
				VisualDiagnostics.RegisterSourceInfo(bindingExtension = new BindingExtension(), new Uri("Settings\\SettingsGarage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 103, 47);
				Span span;
				VisualDiagnostics.RegisterSourceInfo(span = new Span(), new Uri("Settings\\SettingsGarage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 103, 42);
				Span span2;
				VisualDiagnostics.RegisterSourceInfo(span2 = new Span(), new Uri("Settings\\SettingsGarage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 104, 42);
				BindingExtension bindingExtension2;
				VisualDiagnostics.RegisterSourceInfo(bindingExtension2 = new BindingExtension(), new Uri("Settings\\SettingsGarage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 105, 47);
				Span span3;
				VisualDiagnostics.RegisterSourceInfo(span3 = new Span(), new Uri("Settings\\SettingsGarage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 105, 42);
				Span span4;
				VisualDiagnostics.RegisterSourceInfo(span4 = new Span(), new Uri("Settings\\SettingsGarage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 106, 42);
				FormattedString formattedString;
				VisualDiagnostics.RegisterSourceInfo(formattedString = new FormattedString(), new Uri("Settings\\SettingsGarage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 102, 38);
				Label label;
				VisualDiagnostics.RegisterSourceInfo(label = new Label(), new Uri("Settings\\SettingsGarage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 95, 30);
				DynamicResourceExtension dynamicResourceExtension2;
				VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension2 = new DynamicResourceExtension(), new Uri("Settings\\SettingsGarage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 112, 33);
				Frame frame;
				VisualDiagnostics.RegisterSourceInfo(frame = new Frame(), new Uri("Settings\\SettingsGarage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 110, 30);
				Grid grid;
				VisualDiagnostics.RegisterSourceInfo(grid = new Grid(), new Uri("Settings\\SettingsGarage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 90, 26);
				NameScope nameScope = new NameScope();
				NameScope.SetNameScope(grid, nameScope);
				dynamicResourceExtension.Key = "ButtonAccentColor";
				IMarkupExtension<DynamicResource> markupExtension = dynamicResourceExtension;
				XamlServiceProvider xamlServiceProvider = new XamlServiceProvider();
				Type typeFromHandle = typeof(IProvideValueTarget);
				int num;
				object[] array = new object[(num = this.parentValues.Length) + 1];
				Array.Copy(this.parentValues, 0, array, 1, num);
				object[] array2 = array;
				array2[0] = grid;
				object obj;
				xamlServiceProvider.Add(typeFromHandle, obj = new SimpleValueTargetProvider(array2, VisualElement.BackgroundColorProperty, nameScope));
				xamlServiceProvider.Add(typeof(IReferenceProvider), obj);
				Type typeFromHandle2 = typeof(IXamlTypeResolver);
				XmlNamespaceResolver xmlNamespaceResolver = new XmlNamespaceResolver();
				xmlNamespaceResolver.Add("", "http://xamarin.com/schemas/2014/forms");
				xmlNamespaceResolver.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
				xmlNamespaceResolver.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
				xmlNamespaceResolver.Add("local", "clr-namespace:CarScannerXamarinForms");
				xmlNamespaceResolver.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
				xmlNamespaceResolver.Add("syncfusion", "clr-namespace:Syncfusion.ListView.XForms;assembly=Syncfusion.SfListView.XForms");
				xamlServiceProvider.Add(typeFromHandle2, new XamlTypeResolver(xmlNamespaceResolver, typeof(SettingsGarage.<InitializeComponent>_anonXamlCDataTemplate_93).GetTypeInfo().Assembly));
				xamlServiceProvider.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(90, 31)));
				DynamicResource dynamicResource = markupExtension.ProvideValue(xamlServiceProvider);
				grid.SetDynamicResource(VisualElement.BackgroundColorProperty, dynamicResource.Key);
				rowDefinition.SetValue(RowDefinition.HeightProperty, new GridLengthTypeConverter().ConvertFromInvariantString("auto"));
				grid.GetValue(Grid.RowDefinitionsProperty).Add(rowDefinition);
				rowDefinition2.SetValue(RowDefinition.HeightProperty, new GridLengthTypeConverter().ConvertFromInvariantString("3"));
				grid.GetValue(Grid.RowDefinitionsProperty).Add(rowDefinition2);
				label.SetValue(Grid.RowProperty, 0);
				staticResourceExtension.Key = "BaseFontSize++";
				IMarkupExtension markupExtension2 = staticResourceExtension;
				XamlServiceProvider xamlServiceProvider2 = new XamlServiceProvider();
				Type typeFromHandle3 = typeof(IProvideValueTarget);
				int num2;
				object[] array3 = new object[(num2 = this.parentValues.Length) + 2];
				Array.Copy(this.parentValues, 0, array3, 2, num2);
				object[] array4 = array3;
				array4[0] = label;
				array4[1] = grid;
				object obj2;
				xamlServiceProvider2.Add(typeFromHandle3, obj2 = new SimpleValueTargetProvider(array4, Label.FontSizeProperty, nameScope));
				xamlServiceProvider2.Add(typeof(IReferenceProvider), obj2);
				Type typeFromHandle4 = typeof(IXamlTypeResolver);
				XmlNamespaceResolver xmlNamespaceResolver2 = new XmlNamespaceResolver();
				xmlNamespaceResolver2.Add("", "http://xamarin.com/schemas/2014/forms");
				xmlNamespaceResolver2.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
				xmlNamespaceResolver2.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
				xmlNamespaceResolver2.Add("local", "clr-namespace:CarScannerXamarinForms");
				xmlNamespaceResolver2.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
				xmlNamespaceResolver2.Add("syncfusion", "clr-namespace:Syncfusion.ListView.XForms;assembly=Syncfusion.SfListView.XForms");
				xamlServiceProvider2.Add(typeFromHandle4, new XamlTypeResolver(xmlNamespaceResolver2, typeof(SettingsGarage.<InitializeComponent>_anonXamlCDataTemplate_93).GetTypeInfo().Assembly));
				xamlServiceProvider2.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(97, 33)));
				object obj3 = markupExtension2.ProvideValue(xamlServiceProvider2);
				label.FontSize = (double)obj3;
				label.SetValue(Label.TextColorProperty, Color.White);
				label.SetValue(View.VerticalOptionsProperty, LayoutOptions.Center);
				label.SetValue(Label.VerticalTextAlignmentProperty, new TextAlignmentConverter().ConvertFromInvariantString("Center"));
				bindingExtension.Path = "Name";
				BindingBase bindingBase = bindingExtension.ProvideValue(null);
				span.SetBinding(Span.TextProperty, bindingBase);
				formattedString.Spans.Add(span);
				span2.SetValue(Span.TextProperty, " (");
				formattedString.Spans.Add(span2);
				bindingExtension2.Path = "Year";
				BindingBase bindingBase2 = bindingExtension2.ProvideValue(null);
				span3.SetBinding(Span.TextProperty, bindingBase2);
				formattedString.Spans.Add(span3);
				span4.SetValue(Span.TextProperty, ")");
				formattedString.Spans.Add(span4);
				label.SetValue(Label.FormattedTextProperty, formattedString);
				grid.Children.Add(label);
				frame.SetValue(Grid.RowProperty, 1);
				dynamicResourceExtension2.Key = "ListViewSeparatorColor";
				IMarkupExtension<DynamicResource> markupExtension3 = dynamicResourceExtension2;
				XamlServiceProvider xamlServiceProvider3 = new XamlServiceProvider();
				Type typeFromHandle5 = typeof(IProvideValueTarget);
				int num3;
				object[] array5 = new object[(num3 = this.parentValues.Length) + 2];
				Array.Copy(this.parentValues, 0, array5, 2, num3);
				object[] array6 = array5;
				array6[0] = frame;
				array6[1] = grid;
				object obj4;
				xamlServiceProvider3.Add(typeFromHandle5, obj4 = new SimpleValueTargetProvider(array6, Frame.BorderColorProperty, nameScope));
				xamlServiceProvider3.Add(typeof(IReferenceProvider), obj4);
				Type typeFromHandle6 = typeof(IXamlTypeResolver);
				XmlNamespaceResolver xmlNamespaceResolver3 = new XmlNamespaceResolver();
				xmlNamespaceResolver3.Add("", "http://xamarin.com/schemas/2014/forms");
				xmlNamespaceResolver3.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
				xmlNamespaceResolver3.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
				xmlNamespaceResolver3.Add("local", "clr-namespace:CarScannerXamarinForms");
				xmlNamespaceResolver3.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
				xmlNamespaceResolver3.Add("syncfusion", "clr-namespace:Syncfusion.ListView.XForms;assembly=Syncfusion.SfListView.XForms");
				xamlServiceProvider3.Add(typeFromHandle6, new XamlTypeResolver(xmlNamespaceResolver3, typeof(SettingsGarage.<InitializeComponent>_anonXamlCDataTemplate_93).GetTypeInfo().Assembly));
				xamlServiceProvider3.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(112, 33)));
				DynamicResource dynamicResource2 = markupExtension3.ProvideValue(xamlServiceProvider3);
				frame.SetDynamicResource(Frame.BorderColorProperty, dynamicResource2.Key);
				frame.SetValue(Frame.HasShadowProperty, false);
				frame.SetValue(VisualElement.HeightRequestProperty, 2.0);
				grid.Children.Add(frame);
				return grid;
			}

			// Token: 0x04000CA4 RID: 3236
			internal object[] parentValues;

			// Token: 0x04000CA5 RID: 3237
			internal SettingsGarage root;
		}

		// Token: 0x0200023F RID: 575
		[CompilerGenerated]
		private sealed class <InitializeComponent>_anonXamlCDataTemplate_94
		{
			// Token: 0x06001B44 RID: 6980 RVA: 0x00130EBC File Offset: 0x0012F0BC
			public <InitializeComponent>_anonXamlCDataTemplate_94()
			{
			}

			// Token: 0x06001B45 RID: 6981 RVA: 0x00130ED0 File Offset: 0x0012F0D0
			internal object LoadDataTemplate()
			{
				RowDefinition rowDefinition;
				VisualDiagnostics.RegisterSourceInfo(rowDefinition = new RowDefinition(), new Uri("Settings\\SettingsGarage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 127, 34);
				RowDefinition rowDefinition2;
				VisualDiagnostics.RegisterSourceInfo(rowDefinition2 = new RowDefinition(), new Uri("Settings\\SettingsGarage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 128, 34);
				StaticResourceExtension staticResourceExtension;
				VisualDiagnostics.RegisterSourceInfo(staticResourceExtension = new StaticResourceExtension(), new Uri("Settings\\SettingsGarage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 132, 33);
				DynamicResourceExtension dynamicResourceExtension;
				VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension = new DynamicResourceExtension(), new Uri("Settings\\SettingsGarage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 133, 33);
				BindingExtension bindingExtension;
				VisualDiagnostics.RegisterSourceInfo(bindingExtension = new BindingExtension(), new Uri("Settings\\SettingsGarage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 138, 47);
				Span span;
				VisualDiagnostics.RegisterSourceInfo(span = new Span(), new Uri("Settings\\SettingsGarage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 138, 42);
				Span span2;
				VisualDiagnostics.RegisterSourceInfo(span2 = new Span(), new Uri("Settings\\SettingsGarage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 139, 42);
				BindingExtension bindingExtension2;
				VisualDiagnostics.RegisterSourceInfo(bindingExtension2 = new BindingExtension(), new Uri("Settings\\SettingsGarage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 140, 47);
				Span span3;
				VisualDiagnostics.RegisterSourceInfo(span3 = new Span(), new Uri("Settings\\SettingsGarage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 140, 42);
				Span span4;
				VisualDiagnostics.RegisterSourceInfo(span4 = new Span(), new Uri("Settings\\SettingsGarage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 141, 42);
				FormattedString formattedString;
				VisualDiagnostics.RegisterSourceInfo(formattedString = new FormattedString(), new Uri("Settings\\SettingsGarage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 137, 38);
				Label label;
				VisualDiagnostics.RegisterSourceInfo(label = new Label(), new Uri("Settings\\SettingsGarage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 130, 30);
				DynamicResourceExtension dynamicResourceExtension2;
				VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension2 = new DynamicResourceExtension(), new Uri("Settings\\SettingsGarage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 148, 33);
				Frame frame;
				VisualDiagnostics.RegisterSourceInfo(frame = new Frame(), new Uri("Settings\\SettingsGarage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 146, 30);
				Grid grid;
				VisualDiagnostics.RegisterSourceInfo(grid = new Grid(), new Uri("Settings\\SettingsGarage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 125, 26);
				NameScope nameScope = new NameScope();
				NameScope.SetNameScope(grid, nameScope);
				rowDefinition.SetValue(RowDefinition.HeightProperty, new GridLengthTypeConverter().ConvertFromInvariantString("auto"));
				grid.GetValue(Grid.RowDefinitionsProperty).Add(rowDefinition);
				rowDefinition2.SetValue(RowDefinition.HeightProperty, new GridLengthTypeConverter().ConvertFromInvariantString("3"));
				grid.GetValue(Grid.RowDefinitionsProperty).Add(rowDefinition2);
				label.SetValue(Grid.RowProperty, 0);
				staticResourceExtension.Key = "BaseFontSize++";
				IMarkupExtension markupExtension = staticResourceExtension;
				XamlServiceProvider xamlServiceProvider = new XamlServiceProvider();
				Type typeFromHandle = typeof(IProvideValueTarget);
				int num;
				object[] array = new object[(num = this.parentValues.Length) + 2];
				Array.Copy(this.parentValues, 0, array, 2, num);
				object[] array2 = array;
				array2[0] = label;
				array2[1] = grid;
				object obj;
				xamlServiceProvider.Add(typeFromHandle, obj = new SimpleValueTargetProvider(array2, Label.FontSizeProperty, nameScope));
				xamlServiceProvider.Add(typeof(IReferenceProvider), obj);
				Type typeFromHandle2 = typeof(IXamlTypeResolver);
				XmlNamespaceResolver xmlNamespaceResolver = new XmlNamespaceResolver();
				xmlNamespaceResolver.Add("", "http://xamarin.com/schemas/2014/forms");
				xmlNamespaceResolver.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
				xmlNamespaceResolver.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
				xmlNamespaceResolver.Add("local", "clr-namespace:CarScannerXamarinForms");
				xmlNamespaceResolver.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
				xmlNamespaceResolver.Add("syncfusion", "clr-namespace:Syncfusion.ListView.XForms;assembly=Syncfusion.SfListView.XForms");
				xamlServiceProvider.Add(typeFromHandle2, new XamlTypeResolver(xmlNamespaceResolver, typeof(SettingsGarage.<InitializeComponent>_anonXamlCDataTemplate_94).GetTypeInfo().Assembly));
				xamlServiceProvider.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(132, 33)));
				object obj2 = markupExtension.ProvideValue(xamlServiceProvider);
				label.FontSize = (double)obj2;
				dynamicResourceExtension.Key = "TextColor";
				IMarkupExtension<DynamicResource> markupExtension2 = dynamicResourceExtension;
				XamlServiceProvider xamlServiceProvider2 = new XamlServiceProvider();
				Type typeFromHandle3 = typeof(IProvideValueTarget);
				int num2;
				object[] array3 = new object[(num2 = this.parentValues.Length) + 2];
				Array.Copy(this.parentValues, 0, array3, 2, num2);
				object[] array4 = array3;
				array4[0] = label;
				array4[1] = grid;
				object obj3;
				xamlServiceProvider2.Add(typeFromHandle3, obj3 = new SimpleValueTargetProvider(array4, Label.TextColorProperty, nameScope));
				xamlServiceProvider2.Add(typeof(IReferenceProvider), obj3);
				Type typeFromHandle4 = typeof(IXamlTypeResolver);
				XmlNamespaceResolver xmlNamespaceResolver2 = new XmlNamespaceResolver();
				xmlNamespaceResolver2.Add("", "http://xamarin.com/schemas/2014/forms");
				xmlNamespaceResolver2.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
				xmlNamespaceResolver2.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
				xmlNamespaceResolver2.Add("local", "clr-namespace:CarScannerXamarinForms");
				xmlNamespaceResolver2.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
				xmlNamespaceResolver2.Add("syncfusion", "clr-namespace:Syncfusion.ListView.XForms;assembly=Syncfusion.SfListView.XForms");
				xamlServiceProvider2.Add(typeFromHandle4, new XamlTypeResolver(xmlNamespaceResolver2, typeof(SettingsGarage.<InitializeComponent>_anonXamlCDataTemplate_94).GetTypeInfo().Assembly));
				xamlServiceProvider2.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(133, 33)));
				DynamicResource dynamicResource = markupExtension2.ProvideValue(xamlServiceProvider2);
				label.SetDynamicResource(Label.TextColorProperty, dynamicResource.Key);
				label.SetValue(View.VerticalOptionsProperty, LayoutOptions.Center);
				label.SetValue(Label.VerticalTextAlignmentProperty, new TextAlignmentConverter().ConvertFromInvariantString("Center"));
				bindingExtension.Path = "Name";
				BindingBase bindingBase = bindingExtension.ProvideValue(null);
				span.SetBinding(Span.TextProperty, bindingBase);
				formattedString.Spans.Add(span);
				span2.SetValue(Span.TextProperty, " (");
				formattedString.Spans.Add(span2);
				bindingExtension2.Path = "Year";
				BindingBase bindingBase2 = bindingExtension2.ProvideValue(null);
				span3.SetBinding(Span.TextProperty, bindingBase2);
				formattedString.Spans.Add(span3);
				span4.SetValue(Span.TextProperty, ")");
				formattedString.Spans.Add(span4);
				label.SetValue(Label.FormattedTextProperty, formattedString);
				grid.Children.Add(label);
				frame.SetValue(Grid.RowProperty, 1);
				dynamicResourceExtension2.Key = "ListViewSeparatorColor";
				IMarkupExtension<DynamicResource> markupExtension3 = dynamicResourceExtension2;
				XamlServiceProvider xamlServiceProvider3 = new XamlServiceProvider();
				Type typeFromHandle5 = typeof(IProvideValueTarget);
				int num3;
				object[] array5 = new object[(num3 = this.parentValues.Length) + 2];
				Array.Copy(this.parentValues, 0, array5, 2, num3);
				object[] array6 = array5;
				array6[0] = frame;
				array6[1] = grid;
				object obj4;
				xamlServiceProvider3.Add(typeFromHandle5, obj4 = new SimpleValueTargetProvider(array6, Frame.BorderColorProperty, nameScope));
				xamlServiceProvider3.Add(typeof(IReferenceProvider), obj4);
				Type typeFromHandle6 = typeof(IXamlTypeResolver);
				XmlNamespaceResolver xmlNamespaceResolver3 = new XmlNamespaceResolver();
				xmlNamespaceResolver3.Add("", "http://xamarin.com/schemas/2014/forms");
				xmlNamespaceResolver3.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
				xmlNamespaceResolver3.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
				xmlNamespaceResolver3.Add("local", "clr-namespace:CarScannerXamarinForms");
				xmlNamespaceResolver3.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
				xmlNamespaceResolver3.Add("syncfusion", "clr-namespace:Syncfusion.ListView.XForms;assembly=Syncfusion.SfListView.XForms");
				xamlServiceProvider3.Add(typeFromHandle6, new XamlTypeResolver(xmlNamespaceResolver3, typeof(SettingsGarage.<InitializeComponent>_anonXamlCDataTemplate_94).GetTypeInfo().Assembly));
				xamlServiceProvider3.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(148, 33)));
				DynamicResource dynamicResource2 = markupExtension3.ProvideValue(xamlServiceProvider3);
				frame.SetDynamicResource(Frame.BorderColorProperty, dynamicResource2.Key);
				frame.SetValue(Frame.HasShadowProperty, false);
				frame.SetValue(VisualElement.HeightRequestProperty, 2.0);
				grid.Children.Add(frame);
				return grid;
			}

			// Token: 0x04000CA6 RID: 3238
			internal object[] parentValues;

			// Token: 0x04000CA7 RID: 3239
			internal SettingsGarage root;
		}
	}
}
