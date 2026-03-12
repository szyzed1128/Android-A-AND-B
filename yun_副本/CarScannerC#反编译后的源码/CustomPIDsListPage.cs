using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using CarScannerXamarinForms.Common;
using CarScannerXamarinForms.OBD2;
using CarScannerXamarinForms.OBD2.PIDS;
using CarScannerXamarinForms.PlatformAdapters;
using CarScannerXamarinForms.Settings;
using CarScannerXamarinForms.ViewModels;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Internals;
using Xamarin.Forms.PlatformConfiguration.iOSSpecific;
using Xamarin.Forms.Xaml;
using Xamarin.Forms.Xaml.Diagnostics;
using Xamarin.Forms.Xaml.Internals;

namespace CarScannerXamarinForms
{
	// Token: 0x0200016B RID: 363
	[XamlFilePath("Settings\\CustomPIDsListPage.xaml")]
	public class CustomPIDsListPage : ContentPage
	{
		// Token: 0x06001543 RID: 5443 RVA: 0x0009007A File Offset: 0x0008E27A
		public CustomPIDsListPage()
		{
			this.InitializeComponent();
			if (PlatformHelper.IsAndroid)
			{
				this.btnImportFromFile.IsVisible = true;
			}
			base.BindingContext = CustomPIDViewModel.CurrentCustom;
		}

		// Token: 0x06001544 RID: 5444 RVA: 0x000900A8 File Offset: 0x0008E2A8
		private void btnProfileSelector_Clicked(object sender, EventArgs e)
		{
			ProfileSelectorV2Page profileSelectorV2Page = new ProfileSelectorV2Page();
			base.Navigation.PushAsync(profileSelectorV2Page);
		}

		// Token: 0x06001545 RID: 5445 RVA: 0x000027D4 File Offset: 0x000009D4
		private void Handle_SizeChanged(object sender, EventArgs e)
		{
		}

		// Token: 0x06001546 RID: 5446 RVA: 0x000900C8 File Offset: 0x0008E2C8
		private void Handle_Appearing(object sender, EventArgs e)
		{
			base.ToolbarItems.Add(new ToolbarItem("", (string)Application.Current.Resources["NB_add"], delegate
			{
				this.btnNew_Clicked(null, null);
			}, 0, 0));
			base.ToolbarItems.Add(new ToolbarItem("", (string)Application.Current.Resources["InfoImageNavigationBarTextColor"], delegate
			{
				this.btnInfo_Clicked(null, null);
			}, 0, 0));
		}

		// Token: 0x06001547 RID: 5447 RVA: 0x0009014D File Offset: 0x0008E34D
		private void btnInfo_Clicked(object sender, EventArgs e)
		{
			Device.OpenUri(new Uri("http://carscanner.info/custompids"));
		}

		// Token: 0x06001548 RID: 5448 RVA: 0x00090160 File Offset: 0x0008E360
		private async void Page_Disappearing(object sender, EventArgs e)
		{
			base.ToolbarItems.Clear();
			if (App.OBDReader.CurrentStatus == OBDDataReaderStatus.ConnectedToECU)
			{
				LiveDataPIDModel.UpdatePIDCollection(App.OBDReader);
			}
			CustomPIDViewModel.CurrentCustom.Save();
		}

		// Token: 0x06001549 RID: 5449 RVA: 0x00090198 File Offset: 0x0008E398
		private void listView_ItemTapped(object sender, ItemTappedEventArgs e)
		{
			if (e == null)
			{
				return;
			}
			Page page = new CustomPIDsEditorPage(e.Item as CustomPID);
			((ListView)sender).SelectedItem = null;
			base.Navigation.PushAsync(page);
		}

		// Token: 0x0600154A RID: 5450 RVA: 0x000027D4 File Offset: 0x000009D4
		private void switch_Toggled(object sender, ToggledEventArgs e)
		{
		}

		// Token: 0x0600154B RID: 5451 RVA: 0x000901D8 File Offset: 0x0008E3D8
		private async void btnEdit_Clicked(object sender, EventArgs e)
		{
			CustomPIDsEditorPage customPIDsEditorPage = new CustomPIDsEditorPage(((Button)sender).BindingContext as CustomPID);
			await base.Navigation.PushAsync(customPIDsEditorPage);
		}

		// Token: 0x0600154C RID: 5452 RVA: 0x00090218 File Offset: 0x0008E418
		private async void btnNew_Clicked(object sender, EventArgs e)
		{
			base.IsBusy = true;
			CustomPID customPID = new CustomPID("New PID", "New PID", "", "", "", UnitsHelper.Units.None, 0.0, 100.0, "", "", false, Roles.None, CustomPIDType.Formula, 0, 1, 1.0, 1.0, 0.0, false, false, true, null);
			CustomPIDViewModel.CurrentCustom.PidCollection.Add(customPID);
			CustomPIDViewModel.CurrentCustom.Save();
			try
			{
				this.lvPids.ScrollTo(customPID, 0, true);
			}
			catch
			{
			}
			base.IsBusy = false;
		}

		// Token: 0x0600154D RID: 5453 RVA: 0x00090250 File Offset: 0x0008E450
		private async void btnRemoveAll_Clicked(object sender, EventArgs e)
		{
			TaskAwaiter<bool> taskAwaiter = base.DisplayAlert("Delete all PIDs?", "Are you sure, you want to delete all custom PIDs?", "Yes", "No").GetAwaiter();
			if (!taskAwaiter.IsCompleted)
			{
				await taskAwaiter;
				TaskAwaiter<bool> taskAwaiter2;
				taskAwaiter = taskAwaiter2;
				taskAwaiter2 = default(TaskAwaiter<bool>);
			}
			if (taskAwaiter.GetResult())
			{
				base.IsBusy = true;
				CustomPIDViewModel.CurrentCustom.PidCollection.Clear();
				CustomPIDViewModel.CurrentCustom.Save();
				base.IsBusy = false;
			}
		}

		// Token: 0x0600154E RID: 5454 RVA: 0x00090288 File Offset: 0x0008E488
		private async void btnDel_Clicked(object sender, EventArgs e)
		{
			if (CustomPIDViewModel.CurrentCustom.PidCollection.Count == 1)
			{
				CustomPIDViewModel.CurrentCustom.PidCollection.Clear();
				CustomPIDViewModel.CurrentCustom.Save();
			}
			else
			{
				CustomPID customPID = (sender as MenuItem).BindingContext as CustomPID;
				CustomPIDViewModel.CurrentCustom.PidCollection.Remove(customPID);
				CustomPIDViewModel.CurrentCustom.Save();
			}
		}

		// Token: 0x0600154F RID: 5455 RVA: 0x000902C0 File Offset: 0x0008E4C0
		private async void btnImportFromFile_Clicked(object sender, EventArgs e)
		{
			FileResult fileResult = await FilePicker.PickAsync(null);
			if (fileResult != null)
			{
				this.ImportCallback(fileResult.FullPath, delegate
				{
				});
			}
		}

		// Token: 0x06001550 RID: 5456 RVA: 0x000902F8 File Offset: 0x0008E4F8
		private async void ImportCallback(string in_file, Action complete_delegate)
		{
			using (FileStream fileStream = File.OpenRead(in_file))
			{
				foreach (CustomPID customPID in CSVLoader.LoadFromCSV(fileStream))
				{
					CustomPIDViewModel.CurrentCustom.PidCollection.Add(customPID);
				}
			}
			CustomPIDViewModel.CurrentCustom.Save();
			await App.GetCurrentPage().DisplayAlert(Translate.GetString("droid_ImportedTitle_CSV"), Translate.GetString("droid_ImportedText_CSV"), "OK");
			complete_delegate();
		}

		// Token: 0x06001551 RID: 5457 RVA: 0x00090338 File Offset: 0x0008E538
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private void InitializeComponent()
		{
			if (ResourceLoader.IsEnabled && ResourceLoader.CanProvideContentFor(new ResourceLoader.ResourceLoadingQuery
			{
				AssemblyName = typeof(CustomPIDsListPage).GetTypeInfo().Assembly.GetName(),
				ResourcePath = "Settings/CustomPIDsListPage.xaml",
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
			VisualDiagnostics.RegisterSourceInfo(translate = new Translate(), new Uri("Settings\\CustomPIDsListPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 9, 5);
			DynamicResourceExtension dynamicResourceExtension;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension = new DynamicResourceExtension(), new Uri("Settings\\CustomPIDsListPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 13, 5);
			UnitsToStringConverter unitsToStringConverter;
			VisualDiagnostics.RegisterSourceInfo(unitsToStringConverter = new UnitsToStringConverter(), new Uri("Settings\\CustomPIDsListPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 18, 14);
			BoolToNegativeConverter boolToNegativeConverter;
			VisualDiagnostics.RegisterSourceInfo(boolToNegativeConverter = new BoolToNegativeConverter(), new Uri("Settings\\CustomPIDsListPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 19, 14);
			ResourceDictionary resourceDictionary;
			VisualDiagnostics.RegisterSourceInfo(resourceDictionary = new ResourceDictionary(), new Uri("Settings\\CustomPIDsListPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 17, 10);
			OnPlatform<Thickness> onPlatform;
			VisualDiagnostics.RegisterSourceInfo(onPlatform = new OnPlatform<Thickness>(), new Uri("Settings\\CustomPIDsListPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 26, 18);
			RowDefinition rowDefinition;
			VisualDiagnostics.RegisterSourceInfo(rowDefinition = new RowDefinition(), new Uri("Settings\\CustomPIDsListPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 32, 18);
			RowDefinition rowDefinition2;
			VisualDiagnostics.RegisterSourceInfo(rowDefinition2 = new RowDefinition(), new Uri("Settings\\CustomPIDsListPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 33, 18);
			BindingExtension bindingExtension;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension = new BindingExtension(), new Uri("Settings\\CustomPIDsListPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 42, 17);
			DataTemplate dataTemplate;
			VisualDiagnostics.RegisterSourceInfo(dataTemplate = new DataTemplate(), new Uri("Settings\\CustomPIDsListPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 44, 22);
			ListView listView;
			VisualDiagnostics.RegisterSourceInfo(listView = new ListView(), new Uri("Settings\\CustomPIDsListPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 35, 14);
			ColumnDefinition columnDefinition;
			VisualDiagnostics.RegisterSourceInfo(columnDefinition = new ColumnDefinition(), new Uri("Settings\\CustomPIDsListPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 106, 26);
			ColumnDefinition columnDefinition2;
			VisualDiagnostics.RegisterSourceInfo(columnDefinition2 = new ColumnDefinition(), new Uri("Settings\\CustomPIDsListPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 107, 26);
			Translate translate2;
			VisualDiagnostics.RegisterSourceInfo(translate2 = new Translate(), new Uri("Settings\\CustomPIDsListPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 115, 25);
			Button button;
			VisualDiagnostics.RegisterSourceInfo(button = new Button(), new Uri("Settings\\CustomPIDsListPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 109, 22);
			Translate translate3;
			VisualDiagnostics.RegisterSourceInfo(translate3 = new Translate(), new Uri("Settings\\CustomPIDsListPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 121, 25);
			Button button2;
			VisualDiagnostics.RegisterSourceInfo(button2 = new Button(), new Uri("Settings\\CustomPIDsListPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 116, 22);
			Grid grid;
			VisualDiagnostics.RegisterSourceInfo(grid = new Grid(), new Uri("Settings\\CustomPIDsListPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 104, 18);
			DynamicResourceExtension dynamicResourceExtension2;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension2 = new DynamicResourceExtension(), new Uri("Settings\\CustomPIDsListPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 127, 21);
			Translate translate4;
			VisualDiagnostics.RegisterSourceInfo(translate4 = new Translate(), new Uri("Settings\\CustomPIDsListPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 129, 21);
			Label label;
			VisualDiagnostics.RegisterSourceInfo(label = new Label(), new Uri("Settings\\CustomPIDsListPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 125, 18);
			DynamicResourceExtension dynamicResourceExtension3;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension3 = new DynamicResourceExtension(), new Uri("Settings\\CustomPIDsListPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 131, 21);
			Translate translate5;
			VisualDiagnostics.RegisterSourceInfo(translate5 = new Translate(), new Uri("Settings\\CustomPIDsListPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 133, 21);
			Label label2;
			VisualDiagnostics.RegisterSourceInfo(label2 = new Label(), new Uri("Settings\\CustomPIDsListPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 130, 18);
			SharedSettings sharedSettings;
			VisualDiagnostics.RegisterSourceInfo(sharedSettings = SharedSettings.Current, new Uri("Settings\\CustomPIDsListPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 136, 21);
			DynamicResourceExtension dynamicResourceExtension4;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension4 = new DynamicResourceExtension(), new Uri("Settings\\CustomPIDsListPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 138, 21);
			BindingExtension bindingExtension2;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension2 = new BindingExtension(), new Uri("Settings\\CustomPIDsListPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 140, 21);
			Label label3;
			VisualDiagnostics.RegisterSourceInfo(label3 = new Label(), new Uri("Settings\\CustomPIDsListPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 134, 18);
			Translate translate6;
			VisualDiagnostics.RegisterSourceInfo(translate6 = new Translate(), new Uri("Settings\\CustomPIDsListPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 144, 21);
			Button button3;
			VisualDiagnostics.RegisterSourceInfo(button3 = new Button(), new Uri("Settings\\CustomPIDsListPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 141, 18);
			StackLayout stackLayout;
			VisualDiagnostics.RegisterSourceInfo(stackLayout = new StackLayout(), new Uri("Settings\\CustomPIDsListPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 96, 14);
			Grid grid2;
			VisualDiagnostics.RegisterSourceInfo(grid2 = new Grid(), new Uri("Settings\\CustomPIDsListPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 24, 10);
			VisualDiagnostics.RegisterSourceInfo(this, new Uri("Settings\\CustomPIDsListPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 2, 2);
			NameScope nameScope = NameScope.GetNameScope(this) ?? new NameScope();
			NameScope.SetNameScope(this, nameScope);
			nameScope.RegisterName("lvPids", listView);
			if (listView.StyleId == null)
			{
				listView.StyleId = "lvPids";
			}
			nameScope.RegisterName("btnImportFromFile", button);
			if (button.StyleId == null)
			{
				button.StyleId = "btnImportFromFile";
			}
			nameScope.RegisterName("btnRemoveAll", button2);
			if (button2.StyleId == null)
			{
				button2.StyleId = "btnRemoveAll";
			}
			nameScope.RegisterName("labelSelectedProfile", label3);
			if (label3.StyleId == null)
			{
				label3.StyleId = "labelSelectedProfile";
			}
			this.lvPids = listView;
			this.btnImportFromFile = button;
			this.btnRemoveAll = button2;
			this.labelSelectedProfile = label3;
			this.Resources = resourceDictionary;
			resourceDictionary.Add("UnitsToStringConverter", unitsToStringConverter);
			resourceDictionary.Add("BoolToNegativeConverter", boolToNegativeConverter);
			translate.Text = "Settings_Control_btnEditCustomPIDs.Content";
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
			xmlNamespaceResolver.Add("settings", "clr-namespace:CarScannerXamarinForms.Settings");
			xamlServiceProvider.Add(typeFromHandle2, new XamlTypeResolver(xmlNamespaceResolver, typeof(CustomPIDsListPage).GetTypeInfo().Assembly));
			xamlServiceProvider.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(9, 5)));
			object obj2 = markupExtension.ProvideValue(xamlServiceProvider);
			this.Title = obj2;
			this.SetValue(Page.PaddingProperty, new Thickness(5.0, 0.0));
			this.SetValue(Page.UseSafeAreaProperty, true);
			this.Appearing += this.Handle_Appearing;
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
			xmlNamespaceResolver2.Add("settings", "clr-namespace:CarScannerXamarinForms.Settings");
			xamlServiceProvider2.Add(typeFromHandle4, new XamlTypeResolver(xmlNamespaceResolver2, typeof(CustomPIDsListPage).GetTypeInfo().Assembly));
			xamlServiceProvider2.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(13, 5)));
			DynamicResource dynamicResource = markupExtension2.ProvideValue(xamlServiceProvider2);
			this.SetDynamicResource(VisualElement.BackgroundColorProperty, dynamicResource.Key);
			this.Disappearing += this.Page_Disappearing;
			this.SizeChanged += this.Handle_SizeChanged;
			this.Resources = resourceDictionary;
			grid2.SetValue(Grid.RowSpacingProperty, 0.0);
			onPlatform.Android = new Thickness(5.0, 0.0, 5.0, 0.0);
			onPlatform.iOS = new Thickness(5.0, 0.0, 5.0, 0.0);
			grid2.SetValue(View.MarginProperty, onPlatform);
			rowDefinition.SetValue(RowDefinition.HeightProperty, new GridLengthTypeConverter().ConvertFromInvariantString("*"));
			grid2.GetValue(Grid.RowDefinitionsProperty).Add(rowDefinition);
			rowDefinition2.SetValue(RowDefinition.HeightProperty, new GridLengthTypeConverter().ConvertFromInvariantString("auto"));
			grid2.GetValue(Grid.RowDefinitionsProperty).Add(rowDefinition2);
			listView.SetValue(Grid.RowProperty, 0);
			listView.SetValue(View.MarginProperty, new Thickness(0.0));
			listView.SetValue(VisualElement.BackgroundColorProperty, Color.Transparent);
			listView.SetValue(ListView.HasUnevenRowsProperty, true);
			listView.ItemTapped += this.listView_ItemTapped;
			bindingExtension.Path = "PidCollection";
			BindingBase bindingBase = bindingExtension.ProvideValue(null);
			listView.SetBinding(ItemsView<Cell>.ItemsSourceProperty, bindingBase);
			IDataTemplate dataTemplate2 = dataTemplate;
			CustomPIDsListPage.<InitializeComponent>_anonXamlCDataTemplate_85 <InitializeComponent>_anonXamlCDataTemplate_ = new CustomPIDsListPage.<InitializeComponent>_anonXamlCDataTemplate_85();
			object[] array3 = new object[0 + 4];
			array3[0] = dataTemplate;
			array3[1] = listView;
			array3[2] = grid2;
			array3[3] = this;
			<InitializeComponent>_anonXamlCDataTemplate_.parentValues = array3;
			<InitializeComponent>_anonXamlCDataTemplate_.root = this;
			dataTemplate2.LoadTemplate = new Func<object>(<InitializeComponent>_anonXamlCDataTemplate_.LoadDataTemplate);
			listView.SetValue(ItemsView<Cell>.ItemTemplateProperty, dataTemplate);
			grid2.Children.Add(listView);
			stackLayout.SetValue(Grid.RowProperty, 1);
			stackLayout.SetValue(View.MarginProperty, new Thickness(0.0, 5.0));
			stackLayout.SetValue(StackLayout.OrientationProperty, 0);
			columnDefinition.SetValue(ColumnDefinition.WidthProperty, new GridLengthTypeConverter().ConvertFromInvariantString("*"));
			grid.GetValue(Grid.ColumnDefinitionsProperty).Add(columnDefinition);
			columnDefinition2.SetValue(ColumnDefinition.WidthProperty, new GridLengthTypeConverter().ConvertFromInvariantString("*"));
			grid.GetValue(Grid.ColumnDefinitionsProperty).Add(columnDefinition2);
			button.SetValue(Grid.ColumnProperty, 0);
			button.Clicked += this.btnImportFromFile_Clicked;
			button.SetValue(View.HorizontalOptionsProperty, LayoutOptions.Start);
			button.SetValue(VisualElement.IsVisibleProperty, new VisualElement.VisibilityConverter().ConvertFromInvariantString("False"));
			translate2.Text = "droid_ImportFromFile";
			IMarkupExtension markupExtension3 = translate2;
			XamlServiceProvider xamlServiceProvider3 = new XamlServiceProvider();
			Type typeFromHandle5 = typeof(IProvideValueTarget);
			object[] array4 = new object[0 + 5];
			array4[0] = button;
			array4[1] = grid;
			array4[2] = stackLayout;
			array4[3] = grid2;
			array4[4] = this;
			object obj4;
			xamlServiceProvider3.Add(typeFromHandle5, obj4 = new SimpleValueTargetProvider(array4, Button.TextProperty, nameScope));
			xamlServiceProvider3.Add(typeof(IReferenceProvider), obj4);
			Type typeFromHandle6 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver3 = new XmlNamespaceResolver();
			xmlNamespaceResolver3.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver3.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver3.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver3.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver3.Add("settings", "clr-namespace:CarScannerXamarinForms.Settings");
			xamlServiceProvider3.Add(typeFromHandle6, new XamlTypeResolver(xmlNamespaceResolver3, typeof(CustomPIDsListPage).GetTypeInfo().Assembly));
			xamlServiceProvider3.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(115, 25)));
			object obj5 = markupExtension3.ProvideValue(xamlServiceProvider3);
			button.Text = obj5;
			grid.Children.Add(button);
			button2.SetValue(Grid.ColumnProperty, 1);
			button2.Clicked += this.btnRemoveAll_Clicked;
			button2.SetValue(View.HorizontalOptionsProperty, LayoutOptions.End);
			translate3.Text = "CustomPIDList_btnRemoveAll.Label";
			IMarkupExtension markupExtension4 = translate3;
			XamlServiceProvider xamlServiceProvider4 = new XamlServiceProvider();
			Type typeFromHandle7 = typeof(IProvideValueTarget);
			object[] array5 = new object[0 + 5];
			array5[0] = button2;
			array5[1] = grid;
			array5[2] = stackLayout;
			array5[3] = grid2;
			array5[4] = this;
			object obj6;
			xamlServiceProvider4.Add(typeFromHandle7, obj6 = new SimpleValueTargetProvider(array5, Button.TextProperty, nameScope));
			xamlServiceProvider4.Add(typeof(IReferenceProvider), obj6);
			Type typeFromHandle8 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver4 = new XmlNamespaceResolver();
			xmlNamespaceResolver4.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver4.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver4.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver4.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver4.Add("settings", "clr-namespace:CarScannerXamarinForms.Settings");
			xamlServiceProvider4.Add(typeFromHandle8, new XamlTypeResolver(xmlNamespaceResolver4, typeof(CustomPIDsListPage).GetTypeInfo().Assembly));
			xamlServiceProvider4.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(121, 25)));
			object obj7 = markupExtension4.ProvideValue(xamlServiceProvider4);
			button2.Text = obj7;
			grid.Children.Add(button2);
			stackLayout.Children.Add(grid);
			label.SetValue(Label.FontAttributesProperty, new FontAttributesConverter().ConvertFromInvariantString("Bold"));
			dynamicResourceExtension2.Key = "BaseFontSize";
			IMarkupExtension<DynamicResource> markupExtension5 = dynamicResourceExtension2;
			XamlServiceProvider xamlServiceProvider5 = new XamlServiceProvider();
			Type typeFromHandle9 = typeof(IProvideValueTarget);
			object[] array6 = new object[0 + 4];
			array6[0] = label;
			array6[1] = stackLayout;
			array6[2] = grid2;
			array6[3] = this;
			object obj8;
			xamlServiceProvider5.Add(typeFromHandle9, obj8 = new SimpleValueTargetProvider(array6, Label.FontSizeProperty, nameScope));
			xamlServiceProvider5.Add(typeof(IReferenceProvider), obj8);
			Type typeFromHandle10 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver5 = new XmlNamespaceResolver();
			xmlNamespaceResolver5.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver5.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver5.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver5.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver5.Add("settings", "clr-namespace:CarScannerXamarinForms.Settings");
			xamlServiceProvider5.Add(typeFromHandle10, new XamlTypeResolver(xmlNamespaceResolver5, typeof(CustomPIDsListPage).GetTypeInfo().Assembly));
			xamlServiceProvider5.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(127, 21)));
			DynamicResource dynamicResource2 = markupExtension5.ProvideValue(xamlServiceProvider5);
			label.SetDynamicResource(Label.FontSizeProperty, dynamicResource2.Key);
			label.SetValue(Label.HorizontalTextAlignmentProperty, new TextAlignmentConverter().ConvertFromInvariantString("Center"));
			translate4.Text = "ios_CustomPIDsIncludedInProfiles";
			IMarkupExtension markupExtension6 = translate4;
			XamlServiceProvider xamlServiceProvider6 = new XamlServiceProvider();
			Type typeFromHandle11 = typeof(IProvideValueTarget);
			object[] array7 = new object[0 + 4];
			array7[0] = label;
			array7[1] = stackLayout;
			array7[2] = grid2;
			array7[3] = this;
			object obj9;
			xamlServiceProvider6.Add(typeFromHandle11, obj9 = new SimpleValueTargetProvider(array7, Label.TextProperty, nameScope));
			xamlServiceProvider6.Add(typeof(IReferenceProvider), obj9);
			Type typeFromHandle12 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver6 = new XmlNamespaceResolver();
			xmlNamespaceResolver6.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver6.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver6.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver6.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver6.Add("settings", "clr-namespace:CarScannerXamarinForms.Settings");
			xamlServiceProvider6.Add(typeFromHandle12, new XamlTypeResolver(xmlNamespaceResolver6, typeof(CustomPIDsListPage).GetTypeInfo().Assembly));
			xamlServiceProvider6.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(129, 21)));
			object obj10 = markupExtension6.ProvideValue(xamlServiceProvider6);
			label.Text = obj10;
			stackLayout.Children.Add(label);
			dynamicResourceExtension3.Key = "BaseFontSize";
			IMarkupExtension<DynamicResource> markupExtension7 = dynamicResourceExtension3;
			XamlServiceProvider xamlServiceProvider7 = new XamlServiceProvider();
			Type typeFromHandle13 = typeof(IProvideValueTarget);
			object[] array8 = new object[0 + 4];
			array8[0] = label2;
			array8[1] = stackLayout;
			array8[2] = grid2;
			array8[3] = this;
			object obj11;
			xamlServiceProvider7.Add(typeFromHandle13, obj11 = new SimpleValueTargetProvider(array8, Label.FontSizeProperty, nameScope));
			xamlServiceProvider7.Add(typeof(IReferenceProvider), obj11);
			Type typeFromHandle14 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver7 = new XmlNamespaceResolver();
			xmlNamespaceResolver7.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver7.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver7.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver7.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver7.Add("settings", "clr-namespace:CarScannerXamarinForms.Settings");
			xamlServiceProvider7.Add(typeFromHandle14, new XamlTypeResolver(xmlNamespaceResolver7, typeof(CustomPIDsListPage).GetTypeInfo().Assembly));
			xamlServiceProvider7.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(131, 21)));
			DynamicResource dynamicResource3 = markupExtension7.ProvideValue(xamlServiceProvider7);
			label2.SetDynamicResource(Label.FontSizeProperty, dynamicResource3.Key);
			label2.SetValue(Label.HorizontalTextAlignmentProperty, new TextAlignmentConverter().ConvertFromInvariantString("Center"));
			translate5.Text = "Settings_Control_tbChooseProfile.Text";
			IMarkupExtension markupExtension8 = translate5;
			XamlServiceProvider xamlServiceProvider8 = new XamlServiceProvider();
			Type typeFromHandle15 = typeof(IProvideValueTarget);
			object[] array9 = new object[0 + 4];
			array9[0] = label2;
			array9[1] = stackLayout;
			array9[2] = grid2;
			array9[3] = this;
			object obj12;
			xamlServiceProvider8.Add(typeFromHandle15, obj12 = new SimpleValueTargetProvider(array9, Label.TextProperty, nameScope));
			xamlServiceProvider8.Add(typeof(IReferenceProvider), obj12);
			Type typeFromHandle16 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver8 = new XmlNamespaceResolver();
			xmlNamespaceResolver8.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver8.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver8.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver8.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver8.Add("settings", "clr-namespace:CarScannerXamarinForms.Settings");
			xamlServiceProvider8.Add(typeFromHandle16, new XamlTypeResolver(xmlNamespaceResolver8, typeof(CustomPIDsListPage).GetTypeInfo().Assembly));
			xamlServiceProvider8.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(133, 21)));
			object obj13 = markupExtension8.ProvideValue(xamlServiceProvider8);
			label2.Text = obj13;
			stackLayout.Children.Add(label2);
			label3.SetValue(BindableObject.BindingContextProperty, sharedSettings);
			label3.SetValue(Label.FontAttributesProperty, new FontAttributesConverter().ConvertFromInvariantString("Bold"));
			dynamicResourceExtension4.Key = "BaseFontSize";
			IMarkupExtension<DynamicResource> markupExtension9 = dynamicResourceExtension4;
			XamlServiceProvider xamlServiceProvider9 = new XamlServiceProvider();
			Type typeFromHandle17 = typeof(IProvideValueTarget);
			object[] array10 = new object[0 + 4];
			array10[0] = label3;
			array10[1] = stackLayout;
			array10[2] = grid2;
			array10[3] = this;
			object obj14;
			xamlServiceProvider9.Add(typeFromHandle17, obj14 = new SimpleValueTargetProvider(array10, Label.FontSizeProperty, nameScope));
			xamlServiceProvider9.Add(typeof(IReferenceProvider), obj14);
			Type typeFromHandle18 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver9 = new XmlNamespaceResolver();
			xmlNamespaceResolver9.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver9.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver9.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver9.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver9.Add("settings", "clr-namespace:CarScannerXamarinForms.Settings");
			xamlServiceProvider9.Add(typeFromHandle18, new XamlTypeResolver(xmlNamespaceResolver9, typeof(CustomPIDsListPage).GetTypeInfo().Assembly));
			xamlServiceProvider9.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(138, 21)));
			DynamicResource dynamicResource4 = markupExtension9.ProvideValue(xamlServiceProvider9);
			label3.SetDynamicResource(Label.FontSizeProperty, dynamicResource4.Key);
			label3.SetValue(Label.HorizontalTextAlignmentProperty, new TextAlignmentConverter().ConvertFromInvariantString("Center"));
			bindingExtension2.Mode = 2;
			bindingExtension2.Path = "BrandAndProfile";
			BindingBase bindingBase2 = bindingExtension2.ProvideValue(null);
			label3.SetBinding(Label.TextProperty, bindingBase2);
			stackLayout.Children.Add(label3);
			button3.Clicked += this.btnProfileSelector_Clicked;
			button3.SetValue(View.HorizontalOptionsProperty, LayoutOptions.CenterAndExpand);
			translate6.Text = "Settings_Control_btnSelectProfile.Content";
			IMarkupExtension markupExtension10 = translate6;
			XamlServiceProvider xamlServiceProvider10 = new XamlServiceProvider();
			Type typeFromHandle19 = typeof(IProvideValueTarget);
			object[] array11 = new object[0 + 4];
			array11[0] = button3;
			array11[1] = stackLayout;
			array11[2] = grid2;
			array11[3] = this;
			object obj15;
			xamlServiceProvider10.Add(typeFromHandle19, obj15 = new SimpleValueTargetProvider(array11, Button.TextProperty, nameScope));
			xamlServiceProvider10.Add(typeof(IReferenceProvider), obj15);
			Type typeFromHandle20 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver10 = new XmlNamespaceResolver();
			xmlNamespaceResolver10.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver10.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver10.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver10.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver10.Add("settings", "clr-namespace:CarScannerXamarinForms.Settings");
			xamlServiceProvider10.Add(typeFromHandle20, new XamlTypeResolver(xmlNamespaceResolver10, typeof(CustomPIDsListPage).GetTypeInfo().Assembly));
			xamlServiceProvider10.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(144, 21)));
			object obj16 = markupExtension10.ProvideValue(xamlServiceProvider10);
			button3.Text = obj16;
			stackLayout.Children.Add(button3);
			grid2.Children.Add(stackLayout);
			this.SetValue(ContentPage.ContentProperty, grid2);
		}

		// Token: 0x06001552 RID: 5458 RVA: 0x000919D2 File Offset: 0x0008FBD2
		[CompilerGenerated]
		private void <Handle_Appearing>b__3_0()
		{
			this.btnNew_Clicked(null, null);
		}

		// Token: 0x06001553 RID: 5459 RVA: 0x000919DC File Offset: 0x0008FBDC
		[CompilerGenerated]
		private void <Handle_Appearing>b__3_1()
		{
			this.btnInfo_Clicked(null, null);
		}

		// Token: 0x06001554 RID: 5460 RVA: 0x000919E8 File Offset: 0x0008FBE8
		private void __InitComponentRuntime()
		{
			Extensions.LoadFromXaml<CustomPIDsListPage>(this, typeof(CustomPIDsListPage));
			this.lvPids = NameScopeExtensions.FindByName<ListView>(this, "lvPids");
			this.btnImportFromFile = NameScopeExtensions.FindByName<Button>(this, "btnImportFromFile");
			this.btnRemoveAll = NameScopeExtensions.FindByName<Button>(this, "btnRemoveAll");
			this.labelSelectedProfile = NameScopeExtensions.FindByName<Label>(this, "labelSelectedProfile");
		}

		// Token: 0x040005C2 RID: 1474
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private ListView lvPids;

		// Token: 0x040005C3 RID: 1475
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private Button btnImportFromFile;

		// Token: 0x040005C4 RID: 1476
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private Button btnRemoveAll;

		// Token: 0x040005C5 RID: 1477
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private Label labelSelectedProfile;

		// Token: 0x0200016C RID: 364
		[CompilerGenerated]
		[Serializable]
		private sealed class <>c
		{
			// Token: 0x06001555 RID: 5461 RVA: 0x00091A4A File Offset: 0x0008FC4A
			// Note: this type is marked as 'beforefieldinit'.
			static <>c()
			{
			}

			// Token: 0x06001556 RID: 5462 RVA: 0x00002050 File Offset: 0x00000250
			public <>c()
			{
			}

			// Token: 0x06001557 RID: 5463 RVA: 0x000027D4 File Offset: 0x000009D4
			internal void <btnImportFromFile_Clicked>b__12_0()
			{
			}

			// Token: 0x040005C6 RID: 1478
			public static readonly CustomPIDsListPage.<>c <>9 = new CustomPIDsListPage.<>c();

			// Token: 0x040005C7 RID: 1479
			public static Action <>9__12_0;
		}

		// Token: 0x0200016D RID: 365
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <ImportCallback>d__13 : IAsyncStateMachine
		{
			// Token: 0x06001558 RID: 5464 RVA: 0x00091A58 File Offset: 0x0008FC58
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				try
				{
					TaskAwaiter taskAwaiter;
					if (num != 0)
					{
						FileStream fileStream = File.OpenRead(in_file);
						try
						{
							IEnumerator<CustomPID> enumerator = CSVLoader.LoadFromCSV(fileStream).GetEnumerator();
							try
							{
								while (enumerator.MoveNext())
								{
									CustomPID customPID = enumerator.Current;
									CustomPIDViewModel.CurrentCustom.PidCollection.Add(customPID);
								}
							}
							finally
							{
								if (num < 0 && enumerator != null)
								{
									enumerator.Dispose();
								}
							}
						}
						finally
						{
							if (num < 0 && fileStream != null)
							{
								((IDisposable)fileStream).Dispose();
							}
						}
						CustomPIDViewModel.CurrentCustom.Save();
						taskAwaiter = App.GetCurrentPage().DisplayAlert(Translate.GetString("droid_ImportedTitle_CSV"), Translate.GetString("droid_ImportedText_CSV"), "OK").GetAwaiter();
						if (!taskAwaiter.IsCompleted)
						{
							num = (num2 = 0);
							TaskAwaiter taskAwaiter2 = taskAwaiter;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, CustomPIDsListPage.<ImportCallback>d__13>(ref taskAwaiter, ref this);
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
					complete_delegate();
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

			// Token: 0x06001559 RID: 5465 RVA: 0x00091BB0 File Offset: 0x0008FDB0
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x040005C8 RID: 1480
			public int <>1__state;

			// Token: 0x040005C9 RID: 1481
			public AsyncVoidMethodBuilder <>t__builder;

			// Token: 0x040005CA RID: 1482
			public string in_file;

			// Token: 0x040005CB RID: 1483
			public Action complete_delegate;

			// Token: 0x040005CC RID: 1484
			private TaskAwaiter <>u__1;
		}

		// Token: 0x0200016E RID: 366
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <Page_Disappearing>d__5 : IAsyncStateMachine
		{
			// Token: 0x0600155A RID: 5466 RVA: 0x00091BC0 File Offset: 0x0008FDC0
			void IAsyncStateMachine.MoveNext()
			{
				CustomPIDsListPage customPIDsListPage = this;
				try
				{
					customPIDsListPage.ToolbarItems.Clear();
					if (App.OBDReader.CurrentStatus == OBDDataReaderStatus.ConnectedToECU)
					{
						LiveDataPIDModel.UpdatePIDCollection(App.OBDReader);
					}
					CustomPIDViewModel.CurrentCustom.Save();
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

			// Token: 0x0600155B RID: 5467 RVA: 0x00091C3C File Offset: 0x0008FE3C
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x040005CD RID: 1485
			public int <>1__state;

			// Token: 0x040005CE RID: 1486
			public AsyncVoidMethodBuilder <>t__builder;

			// Token: 0x040005CF RID: 1487
			public CustomPIDsListPage <>4__this;
		}

		// Token: 0x0200016F RID: 367
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <btnDel_Clicked>d__11 : IAsyncStateMachine
		{
			// Token: 0x0600155C RID: 5468 RVA: 0x00091C4C File Offset: 0x0008FE4C
			void IAsyncStateMachine.MoveNext()
			{
				try
				{
					if (CustomPIDViewModel.CurrentCustom.PidCollection.Count == 1)
					{
						CustomPIDViewModel.CurrentCustom.PidCollection.Clear();
						CustomPIDViewModel.CurrentCustom.Save();
					}
					else
					{
						CustomPID customPID = (sender as MenuItem).BindingContext as CustomPID;
						CustomPIDViewModel.CurrentCustom.PidCollection.Remove(customPID);
						CustomPIDViewModel.CurrentCustom.Save();
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

			// Token: 0x0600155D RID: 5469 RVA: 0x00091CF4 File Offset: 0x0008FEF4
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x040005D0 RID: 1488
			public int <>1__state;

			// Token: 0x040005D1 RID: 1489
			public AsyncVoidMethodBuilder <>t__builder;

			// Token: 0x040005D2 RID: 1490
			public object sender;
		}

		// Token: 0x02000170 RID: 368
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <btnEdit_Clicked>d__8 : IAsyncStateMachine
		{
			// Token: 0x0600155E RID: 5470 RVA: 0x00091D04 File Offset: 0x0008FF04
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				CustomPIDsListPage customPIDsListPage = this;
				try
				{
					TaskAwaiter taskAwaiter;
					if (num != 0)
					{
						CustomPIDsEditorPage customPIDsEditorPage = new CustomPIDsEditorPage(((Button)sender).BindingContext as CustomPID);
						taskAwaiter = customPIDsListPage.Navigation.PushAsync(customPIDsEditorPage).GetAwaiter();
						if (!taskAwaiter.IsCompleted)
						{
							num2 = 0;
							TaskAwaiter taskAwaiter2 = taskAwaiter;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, CustomPIDsListPage.<btnEdit_Clicked>d__8>(ref taskAwaiter, ref this);
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

			// Token: 0x0600155F RID: 5471 RVA: 0x00091DDC File Offset: 0x0008FFDC
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x040005D3 RID: 1491
			public int <>1__state;

			// Token: 0x040005D4 RID: 1492
			public AsyncVoidMethodBuilder <>t__builder;

			// Token: 0x040005D5 RID: 1493
			public object sender;

			// Token: 0x040005D6 RID: 1494
			public CustomPIDsListPage <>4__this;

			// Token: 0x040005D7 RID: 1495
			private TaskAwaiter <>u__1;
		}

		// Token: 0x02000171 RID: 369
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <btnImportFromFile_Clicked>d__12 : IAsyncStateMachine
		{
			// Token: 0x06001560 RID: 5472 RVA: 0x00091DEC File Offset: 0x0008FFEC
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				CustomPIDsListPage customPIDsListPage = this;
				try
				{
					TaskAwaiter<FileResult> taskAwaiter;
					if (num != 0)
					{
						taskAwaiter = FilePicker.PickAsync(null).GetAwaiter();
						if (!taskAwaiter.IsCompleted)
						{
							num2 = 0;
							TaskAwaiter<FileResult> taskAwaiter2 = taskAwaiter;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<FileResult>, CustomPIDsListPage.<btnImportFromFile_Clicked>d__12>(ref taskAwaiter, ref this);
							return;
						}
					}
					else
					{
						TaskAwaiter<FileResult> taskAwaiter2;
						taskAwaiter = taskAwaiter2;
						taskAwaiter2 = default(TaskAwaiter<FileResult>);
						num2 = -1;
					}
					FileResult result = taskAwaiter.GetResult();
					if (result != null)
					{
						customPIDsListPage.ImportCallback(result.FullPath, delegate
						{
						});
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

			// Token: 0x06001561 RID: 5473 RVA: 0x00091ED4 File Offset: 0x000900D4
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x040005D8 RID: 1496
			public int <>1__state;

			// Token: 0x040005D9 RID: 1497
			public AsyncVoidMethodBuilder <>t__builder;

			// Token: 0x040005DA RID: 1498
			public CustomPIDsListPage <>4__this;

			// Token: 0x040005DB RID: 1499
			private TaskAwaiter<FileResult> <>u__1;
		}

		// Token: 0x02000172 RID: 370
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <btnNew_Clicked>d__9 : IAsyncStateMachine
		{
			// Token: 0x06001562 RID: 5474 RVA: 0x00091EE4 File Offset: 0x000900E4
			void IAsyncStateMachine.MoveNext()
			{
				CustomPIDsListPage customPIDsListPage = this;
				try
				{
					customPIDsListPage.IsBusy = true;
					CustomPID customPID = new CustomPID("New PID", "New PID", "", "", "", UnitsHelper.Units.None, 0.0, 100.0, "", "", false, Roles.None, CustomPIDType.Formula, 0, 1, 1.0, 1.0, 0.0, false, false, true, null);
					CustomPIDViewModel.CurrentCustom.PidCollection.Add(customPID);
					CustomPIDViewModel.CurrentCustom.Save();
					try
					{
						customPIDsListPage.lvPids.ScrollTo(customPID, 0, true);
					}
					catch
					{
					}
					customPIDsListPage.IsBusy = false;
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

			// Token: 0x06001563 RID: 5475 RVA: 0x00091FDC File Offset: 0x000901DC
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x040005DC RID: 1500
			public int <>1__state;

			// Token: 0x040005DD RID: 1501
			public AsyncVoidMethodBuilder <>t__builder;

			// Token: 0x040005DE RID: 1502
			public CustomPIDsListPage <>4__this;
		}

		// Token: 0x02000173 RID: 371
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <btnRemoveAll_Clicked>d__10 : IAsyncStateMachine
		{
			// Token: 0x06001564 RID: 5476 RVA: 0x00091FEC File Offset: 0x000901EC
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				CustomPIDsListPage customPIDsListPage = this;
				try
				{
					TaskAwaiter<bool> taskAwaiter3;
					if (num != 0)
					{
						taskAwaiter3 = customPIDsListPage.DisplayAlert("Delete all PIDs?", "Are you sure, you want to delete all custom PIDs?", "Yes", "No").GetAwaiter();
						if (!taskAwaiter3.IsCompleted)
						{
							num2 = 0;
							taskAwaiter2 = taskAwaiter3;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<bool>, CustomPIDsListPage.<btnRemoveAll_Clicked>d__10>(ref taskAwaiter3, ref this);
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
						customPIDsListPage.IsBusy = true;
						CustomPIDViewModel.CurrentCustom.PidCollection.Clear();
						CustomPIDViewModel.CurrentCustom.Save();
						customPIDsListPage.IsBusy = false;
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

			// Token: 0x06001565 RID: 5477 RVA: 0x000920DC File Offset: 0x000902DC
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x040005DF RID: 1503
			public int <>1__state;

			// Token: 0x040005E0 RID: 1504
			public AsyncVoidMethodBuilder <>t__builder;

			// Token: 0x040005E1 RID: 1505
			public CustomPIDsListPage <>4__this;

			// Token: 0x040005E2 RID: 1506
			private TaskAwaiter<bool> <>u__1;
		}

		// Token: 0x02000174 RID: 372
		[CompilerGenerated]
		private sealed class <InitializeComponent>_anonXamlCDataTemplate_85
		{
			// Token: 0x06001566 RID: 5478 RVA: 0x000920EC File Offset: 0x000902EC
			public <InitializeComponent>_anonXamlCDataTemplate_85()
			{
			}

			// Token: 0x06001567 RID: 5479 RVA: 0x00092100 File Offset: 0x00090300
			internal object LoadDataTemplate()
			{
				BindingExtension bindingExtension;
				VisualDiagnostics.RegisterSourceInfo(bindingExtension = new BindingExtension(), new Uri("Settings\\CustomPIDsListPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 49, 37);
				Translate translate;
				VisualDiagnostics.RegisterSourceInfo(translate = new Translate(), new Uri("Settings\\CustomPIDsListPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 51, 37);
				MenuItem menuItem;
				VisualDiagnostics.RegisterSourceInfo(menuItem = new MenuItem(), new Uri("Settings\\CustomPIDsListPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 47, 34);
				ColumnDefinition columnDefinition;
				VisualDiagnostics.RegisterSourceInfo(columnDefinition = new ColumnDefinition(), new Uri("Settings\\CustomPIDsListPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 55, 38);
				ColumnDefinition columnDefinition2;
				VisualDiagnostics.RegisterSourceInfo(columnDefinition2 = new ColumnDefinition(), new Uri("Settings\\CustomPIDsListPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 56, 38);
				ColumnDefinition columnDefinition3;
				VisualDiagnostics.RegisterSourceInfo(columnDefinition3 = new ColumnDefinition(), new Uri("Settings\\CustomPIDsListPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 57, 38);
				DynamicResourceExtension dynamicResourceExtension;
				VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension = new DynamicResourceExtension(), new Uri("Settings\\CustomPIDsListPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 67, 41);
				BindingExtension bindingExtension2;
				VisualDiagnostics.RegisterSourceInfo(bindingExtension2 = new BindingExtension(), new Uri("Settings\\CustomPIDsListPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 68, 41);
				Label label;
				VisualDiagnostics.RegisterSourceInfo(label = new Label(), new Uri("Settings\\CustomPIDsListPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 65, 38);
				StaticResourceExtension staticResourceExtension;
				VisualDiagnostics.RegisterSourceInfo(staticResourceExtension = new StaticResourceExtension(), new Uri("Settings\\CustomPIDsListPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 69, 50);
				BindingExtension bindingExtension3;
				VisualDiagnostics.RegisterSourceInfo(bindingExtension3 = new BindingExtension(), new Uri("Settings\\CustomPIDsListPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 69, 50);
				DynamicResourceExtension dynamicResourceExtension2;
				VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension2 = new DynamicResourceExtension(), new Uri("Settings\\CustomPIDsListPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 70, 48);
				Label label2;
				VisualDiagnostics.RegisterSourceInfo(label2 = new Label(), new Uri("Settings\\CustomPIDsListPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 70, 42);
				DynamicResourceExtension dynamicResourceExtension3;
				VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension3 = new DynamicResourceExtension(), new Uri("Settings\\CustomPIDsListPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 71, 48);
				BindingExtension bindingExtension4;
				VisualDiagnostics.RegisterSourceInfo(bindingExtension4 = new BindingExtension(), new Uri("Settings\\CustomPIDsListPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 71, 91);
				Label label3;
				VisualDiagnostics.RegisterSourceInfo(label3 = new Label(), new Uri("Settings\\CustomPIDsListPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 71, 42);
				StackLayout stackLayout;
				VisualDiagnostics.RegisterSourceInfo(stackLayout = new StackLayout(), new Uri("Settings\\CustomPIDsListPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 69, 38);
				StaticResourceExtension staticResourceExtension2;
				VisualDiagnostics.RegisterSourceInfo(staticResourceExtension2 = new StaticResourceExtension(), new Uri("Settings\\CustomPIDsListPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 73, 50);
				BindingExtension bindingExtension5;
				VisualDiagnostics.RegisterSourceInfo(bindingExtension5 = new BindingExtension(), new Uri("Settings\\CustomPIDsListPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 73, 50);
				DynamicResourceExtension dynamicResourceExtension4;
				VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension4 = new DynamicResourceExtension(), new Uri("Settings\\CustomPIDsListPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 75, 45);
				BindingExtension bindingExtension6;
				VisualDiagnostics.RegisterSourceInfo(bindingExtension6 = new BindingExtension(), new Uri("Settings\\CustomPIDsListPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 76, 45);
				BindingExtension bindingExtension7;
				VisualDiagnostics.RegisterSourceInfo(bindingExtension7 = new BindingExtension(), new Uri("Settings\\CustomPIDsListPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 77, 45);
				Label label4;
				VisualDiagnostics.RegisterSourceInfo(label4 = new Label(), new Uri("Settings\\CustomPIDsListPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 74, 42);
				DynamicResourceExtension dynamicResourceExtension5;
				VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension5 = new DynamicResourceExtension(), new Uri("Settings\\CustomPIDsListPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 80, 45);
				StaticResourceExtension staticResourceExtension3;
				VisualDiagnostics.RegisterSourceInfo(staticResourceExtension3 = new StaticResourceExtension(), new Uri("Settings\\CustomPIDsListPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 81, 45);
				BindingExtension bindingExtension8;
				VisualDiagnostics.RegisterSourceInfo(bindingExtension8 = new BindingExtension(), new Uri("Settings\\CustomPIDsListPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 81, 45);
				BindingExtension bindingExtension9;
				VisualDiagnostics.RegisterSourceInfo(bindingExtension9 = new BindingExtension(), new Uri("Settings\\CustomPIDsListPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 82, 45);
				Label label5;
				VisualDiagnostics.RegisterSourceInfo(label5 = new Label(), new Uri("Settings\\CustomPIDsListPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 79, 42);
				StackLayout stackLayout2;
				VisualDiagnostics.RegisterSourceInfo(stackLayout2 = new StackLayout(), new Uri("Settings\\CustomPIDsListPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 73, 38);
				StackLayout stackLayout3;
				VisualDiagnostics.RegisterSourceInfo(stackLayout3 = new StackLayout(), new Uri("Settings\\CustomPIDsListPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 64, 34);
				Grid grid;
				VisualDiagnostics.RegisterSourceInfo(grid = new Grid(), new Uri("Settings\\CustomPIDsListPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 53, 30);
				ViewCell viewCell;
				VisualDiagnostics.RegisterSourceInfo(viewCell = new ViewCell(), new Uri("Settings\\CustomPIDsListPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 45, 26);
				NameScope nameScope = new NameScope();
				NameScope.SetNameScope(viewCell, nameScope);
				menuItem.Clicked += this.root.btnDel_Clicked;
				bindingExtension.Path = ".";
				BindingBase bindingBase = bindingExtension.ProvideValue(null);
				menuItem.SetBinding(MenuItem.CommandParameterProperty, bindingBase);
				menuItem.SetValue(MenuItem.IsDestructiveProperty, true);
				translate.Text = "SpeedTest_btnRemove.Label";
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
				xmlNamespaceResolver.Add("settings", "clr-namespace:CarScannerXamarinForms.Settings");
				xamlServiceProvider.Add(typeFromHandle2, new XamlTypeResolver(xmlNamespaceResolver, typeof(CustomPIDsListPage.<InitializeComponent>_anonXamlCDataTemplate_85).GetTypeInfo().Assembly));
				xamlServiceProvider.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(51, 37)));
				object obj2 = markupExtension.ProvideValue(xamlServiceProvider);
				menuItem.Text = obj2;
				viewCell.ContextActions.Add(menuItem);
				columnDefinition.SetValue(ColumnDefinition.WidthProperty, new GridLengthTypeConverter().ConvertFromInvariantString("*"));
				grid.GetValue(Grid.ColumnDefinitionsProperty).Add(columnDefinition);
				columnDefinition2.SetValue(ColumnDefinition.WidthProperty, new GridLengthTypeConverter().ConvertFromInvariantString("auto"));
				grid.GetValue(Grid.ColumnDefinitionsProperty).Add(columnDefinition2);
				columnDefinition3.SetValue(ColumnDefinition.WidthProperty, new GridLengthTypeConverter().ConvertFromInvariantString("auto"));
				grid.GetValue(Grid.ColumnDefinitionsProperty).Add(columnDefinition3);
				stackLayout3.SetValue(Grid.ColumnProperty, 0);
				stackLayout3.SetValue(StackLayout.OrientationProperty, 0);
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
				array4[1] = stackLayout3;
				array4[2] = grid;
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
				xmlNamespaceResolver2.Add("settings", "clr-namespace:CarScannerXamarinForms.Settings");
				xamlServiceProvider2.Add(typeFromHandle4, new XamlTypeResolver(xmlNamespaceResolver2, typeof(CustomPIDsListPage.<InitializeComponent>_anonXamlCDataTemplate_85).GetTypeInfo().Assembly));
				xamlServiceProvider2.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(67, 41)));
				DynamicResource dynamicResource = markupExtension2.ProvideValue(xamlServiceProvider2);
				label.SetDynamicResource(Label.FontSizeProperty, dynamicResource.Key);
				bindingExtension2.Mode = 2;
				bindingExtension2.Path = "Name";
				BindingBase bindingBase2 = bindingExtension2.ProvideValue(null);
				label.SetBinding(Label.TextProperty, bindingBase2);
				stackLayout3.Children.Add(label);
				bindingExtension3.Mode = 2;
				staticResourceExtension.Key = "BoolToNegativeConverter";
				IMarkupExtension markupExtension3 = staticResourceExtension;
				XamlServiceProvider xamlServiceProvider3 = new XamlServiceProvider();
				Type typeFromHandle5 = typeof(IProvideValueTarget);
				int num3;
				object[] array5 = new object[(num3 = this.parentValues.Length) + 5];
				Array.Copy(this.parentValues, 0, array5, 5, num3);
				object[] array6 = array5;
				array6[0] = bindingExtension3;
				array6[1] = stackLayout;
				array6[2] = stackLayout3;
				array6[3] = grid;
				array6[4] = viewCell;
				object obj4;
				xamlServiceProvider3.Add(typeFromHandle5, obj4 = new SimpleValueTargetProvider(array6, typeof(BindingExtension).GetRuntimeProperty("Converter"), nameScope));
				xamlServiceProvider3.Add(typeof(IReferenceProvider), obj4);
				Type typeFromHandle6 = typeof(IXamlTypeResolver);
				XmlNamespaceResolver xmlNamespaceResolver3 = new XmlNamespaceResolver();
				xmlNamespaceResolver3.Add("", "http://xamarin.com/schemas/2014/forms");
				xmlNamespaceResolver3.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
				xmlNamespaceResolver3.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
				xmlNamespaceResolver3.Add("local", "clr-namespace:CarScannerXamarinForms");
				xmlNamespaceResolver3.Add("settings", "clr-namespace:CarScannerXamarinForms.Settings");
				xamlServiceProvider3.Add(typeFromHandle6, new XamlTypeResolver(xmlNamespaceResolver3, typeof(CustomPIDsListPage.<InitializeComponent>_anonXamlCDataTemplate_85).GetTypeInfo().Assembly));
				xamlServiceProvider3.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(69, 50)));
				object obj5 = markupExtension3.ProvideValue(xamlServiceProvider3);
				bindingExtension3.Converter = obj5;
				bindingExtension3.Path = "IsFormulaHidden";
				BindingBase bindingBase3 = bindingExtension3.ProvideValue(null);
				stackLayout.SetBinding(VisualElement.IsVisibleProperty, bindingBase3);
				stackLayout.SetValue(StackLayout.OrientationProperty, 1);
				dynamicResourceExtension2.Key = "BaseFontSize-";
				IMarkupExtension<DynamicResource> markupExtension4 = dynamicResourceExtension2;
				XamlServiceProvider xamlServiceProvider4 = new XamlServiceProvider();
				Type typeFromHandle7 = typeof(IProvideValueTarget);
				int num4;
				object[] array7 = new object[(num4 = this.parentValues.Length) + 5];
				Array.Copy(this.parentValues, 0, array7, 5, num4);
				object[] array8 = array7;
				array8[0] = label2;
				array8[1] = stackLayout;
				array8[2] = stackLayout3;
				array8[3] = grid;
				array8[4] = viewCell;
				object obj6;
				xamlServiceProvider4.Add(typeFromHandle7, obj6 = new SimpleValueTargetProvider(array8, Label.FontSizeProperty, nameScope));
				xamlServiceProvider4.Add(typeof(IReferenceProvider), obj6);
				Type typeFromHandle8 = typeof(IXamlTypeResolver);
				XmlNamespaceResolver xmlNamespaceResolver4 = new XmlNamespaceResolver();
				xmlNamespaceResolver4.Add("", "http://xamarin.com/schemas/2014/forms");
				xmlNamespaceResolver4.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
				xmlNamespaceResolver4.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
				xmlNamespaceResolver4.Add("local", "clr-namespace:CarScannerXamarinForms");
				xmlNamespaceResolver4.Add("settings", "clr-namespace:CarScannerXamarinForms.Settings");
				xamlServiceProvider4.Add(typeFromHandle8, new XamlTypeResolver(xmlNamespaceResolver4, typeof(CustomPIDsListPage.<InitializeComponent>_anonXamlCDataTemplate_85).GetTypeInfo().Assembly));
				xamlServiceProvider4.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(70, 48)));
				DynamicResource dynamicResource2 = markupExtension4.ProvideValue(xamlServiceProvider4);
				label2.SetDynamicResource(Label.FontSizeProperty, dynamicResource2.Key);
				label2.SetValue(Label.TextProperty, "PID: ");
				stackLayout.Children.Add(label2);
				dynamicResourceExtension3.Key = "BaseFontSize-";
				IMarkupExtension<DynamicResource> markupExtension5 = dynamicResourceExtension3;
				XamlServiceProvider xamlServiceProvider5 = new XamlServiceProvider();
				Type typeFromHandle9 = typeof(IProvideValueTarget);
				int num5;
				object[] array9 = new object[(num5 = this.parentValues.Length) + 5];
				Array.Copy(this.parentValues, 0, array9, 5, num5);
				object[] array10 = array9;
				array10[0] = label3;
				array10[1] = stackLayout;
				array10[2] = stackLayout3;
				array10[3] = grid;
				array10[4] = viewCell;
				object obj7;
				xamlServiceProvider5.Add(typeFromHandle9, obj7 = new SimpleValueTargetProvider(array10, Label.FontSizeProperty, nameScope));
				xamlServiceProvider5.Add(typeof(IReferenceProvider), obj7);
				Type typeFromHandle10 = typeof(IXamlTypeResolver);
				XmlNamespaceResolver xmlNamespaceResolver5 = new XmlNamespaceResolver();
				xmlNamespaceResolver5.Add("", "http://xamarin.com/schemas/2014/forms");
				xmlNamespaceResolver5.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
				xmlNamespaceResolver5.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
				xmlNamespaceResolver5.Add("local", "clr-namespace:CarScannerXamarinForms");
				xmlNamespaceResolver5.Add("settings", "clr-namespace:CarScannerXamarinForms.Settings");
				xamlServiceProvider5.Add(typeFromHandle10, new XamlTypeResolver(xmlNamespaceResolver5, typeof(CustomPIDsListPage.<InitializeComponent>_anonXamlCDataTemplate_85).GetTypeInfo().Assembly));
				xamlServiceProvider5.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(71, 48)));
				DynamicResource dynamicResource3 = markupExtension5.ProvideValue(xamlServiceProvider5);
				label3.SetDynamicResource(Label.FontSizeProperty, dynamicResource3.Key);
				bindingExtension4.Mode = 2;
				bindingExtension4.Path = "Command";
				BindingBase bindingBase4 = bindingExtension4.ProvideValue(null);
				label3.SetBinding(Label.TextProperty, bindingBase4);
				stackLayout.Children.Add(label3);
				stackLayout3.Children.Add(stackLayout);
				bindingExtension5.Mode = 2;
				staticResourceExtension2.Key = "BoolToNegativeConverter";
				IMarkupExtension markupExtension6 = staticResourceExtension2;
				XamlServiceProvider xamlServiceProvider6 = new XamlServiceProvider();
				Type typeFromHandle11 = typeof(IProvideValueTarget);
				int num6;
				object[] array11 = new object[(num6 = this.parentValues.Length) + 5];
				Array.Copy(this.parentValues, 0, array11, 5, num6);
				object[] array12 = array11;
				array12[0] = bindingExtension5;
				array12[1] = stackLayout2;
				array12[2] = stackLayout3;
				array12[3] = grid;
				array12[4] = viewCell;
				object obj8;
				xamlServiceProvider6.Add(typeFromHandle11, obj8 = new SimpleValueTargetProvider(array12, typeof(BindingExtension).GetRuntimeProperty("Converter"), nameScope));
				xamlServiceProvider6.Add(typeof(IReferenceProvider), obj8);
				Type typeFromHandle12 = typeof(IXamlTypeResolver);
				XmlNamespaceResolver xmlNamespaceResolver6 = new XmlNamespaceResolver();
				xmlNamespaceResolver6.Add("", "http://xamarin.com/schemas/2014/forms");
				xmlNamespaceResolver6.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
				xmlNamespaceResolver6.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
				xmlNamespaceResolver6.Add("local", "clr-namespace:CarScannerXamarinForms");
				xmlNamespaceResolver6.Add("settings", "clr-namespace:CarScannerXamarinForms.Settings");
				xamlServiceProvider6.Add(typeFromHandle12, new XamlTypeResolver(xmlNamespaceResolver6, typeof(CustomPIDsListPage.<InitializeComponent>_anonXamlCDataTemplate_85).GetTypeInfo().Assembly));
				xamlServiceProvider6.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(73, 50)));
				object obj9 = markupExtension6.ProvideValue(xamlServiceProvider6);
				bindingExtension5.Converter = obj9;
				bindingExtension5.Path = "IsFormulaHidden";
				BindingBase bindingBase5 = bindingExtension5.ProvideValue(null);
				stackLayout2.SetBinding(VisualElement.IsVisibleProperty, bindingBase5);
				stackLayout2.SetValue(StackLayout.OrientationProperty, 0);
				dynamicResourceExtension4.Key = "BaseFontSize-";
				IMarkupExtension<DynamicResource> markupExtension7 = dynamicResourceExtension4;
				XamlServiceProvider xamlServiceProvider7 = new XamlServiceProvider();
				Type typeFromHandle13 = typeof(IProvideValueTarget);
				int num7;
				object[] array13 = new object[(num7 = this.parentValues.Length) + 5];
				Array.Copy(this.parentValues, 0, array13, 5, num7);
				object[] array14 = array13;
				array14[0] = label4;
				array14[1] = stackLayout2;
				array14[2] = stackLayout3;
				array14[3] = grid;
				array14[4] = viewCell;
				object obj10;
				xamlServiceProvider7.Add(typeFromHandle13, obj10 = new SimpleValueTargetProvider(array14, Label.FontSizeProperty, nameScope));
				xamlServiceProvider7.Add(typeof(IReferenceProvider), obj10);
				Type typeFromHandle14 = typeof(IXamlTypeResolver);
				XmlNamespaceResolver xmlNamespaceResolver7 = new XmlNamespaceResolver();
				xmlNamespaceResolver7.Add("", "http://xamarin.com/schemas/2014/forms");
				xmlNamespaceResolver7.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
				xmlNamespaceResolver7.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
				xmlNamespaceResolver7.Add("local", "clr-namespace:CarScannerXamarinForms");
				xmlNamespaceResolver7.Add("settings", "clr-namespace:CarScannerXamarinForms.Settings");
				xamlServiceProvider7.Add(typeFromHandle14, new XamlTypeResolver(xmlNamespaceResolver7, typeof(CustomPIDsListPage.<InitializeComponent>_anonXamlCDataTemplate_85).GetTypeInfo().Assembly));
				xamlServiceProvider7.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(75, 45)));
				DynamicResource dynamicResource4 = markupExtension7.ProvideValue(xamlServiceProvider7);
				label4.SetDynamicResource(Label.FontSizeProperty, dynamicResource4.Key);
				bindingExtension6.Mode = 2;
				bindingExtension6.Path = "IsFormulaCorrect";
				BindingBase bindingBase6 = bindingExtension6.ProvideValue(null);
				label4.SetBinding(VisualElement.IsVisibleProperty, bindingBase6);
				bindingExtension7.Path = "Formula";
				BindingBase bindingBase7 = bindingExtension7.ProvideValue(null);
				label4.SetBinding(Label.TextProperty, bindingBase7);
				label4.SetValue(Label.TextColorProperty, Color.Green);
				stackLayout2.Children.Add(label4);
				dynamicResourceExtension5.Key = "BaseFontSize-";
				IMarkupExtension<DynamicResource> markupExtension8 = dynamicResourceExtension5;
				XamlServiceProvider xamlServiceProvider8 = new XamlServiceProvider();
				Type typeFromHandle15 = typeof(IProvideValueTarget);
				int num8;
				object[] array15 = new object[(num8 = this.parentValues.Length) + 5];
				Array.Copy(this.parentValues, 0, array15, 5, num8);
				object[] array16 = array15;
				array16[0] = label5;
				array16[1] = stackLayout2;
				array16[2] = stackLayout3;
				array16[3] = grid;
				array16[4] = viewCell;
				object obj11;
				xamlServiceProvider8.Add(typeFromHandle15, obj11 = new SimpleValueTargetProvider(array16, Label.FontSizeProperty, nameScope));
				xamlServiceProvider8.Add(typeof(IReferenceProvider), obj11);
				Type typeFromHandle16 = typeof(IXamlTypeResolver);
				XmlNamespaceResolver xmlNamespaceResolver8 = new XmlNamespaceResolver();
				xmlNamespaceResolver8.Add("", "http://xamarin.com/schemas/2014/forms");
				xmlNamespaceResolver8.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
				xmlNamespaceResolver8.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
				xmlNamespaceResolver8.Add("local", "clr-namespace:CarScannerXamarinForms");
				xmlNamespaceResolver8.Add("settings", "clr-namespace:CarScannerXamarinForms.Settings");
				xamlServiceProvider8.Add(typeFromHandle16, new XamlTypeResolver(xmlNamespaceResolver8, typeof(CustomPIDsListPage.<InitializeComponent>_anonXamlCDataTemplate_85).GetTypeInfo().Assembly));
				xamlServiceProvider8.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(80, 45)));
				DynamicResource dynamicResource5 = markupExtension8.ProvideValue(xamlServiceProvider8);
				label5.SetDynamicResource(Label.FontSizeProperty, dynamicResource5.Key);
				bindingExtension8.Mode = 2;
				staticResourceExtension3.Key = "BoolToNegativeConverter";
				IMarkupExtension markupExtension9 = staticResourceExtension3;
				XamlServiceProvider xamlServiceProvider9 = new XamlServiceProvider();
				Type typeFromHandle17 = typeof(IProvideValueTarget);
				int num9;
				object[] array17 = new object[(num9 = this.parentValues.Length) + 6];
				Array.Copy(this.parentValues, 0, array17, 6, num9);
				object[] array18 = array17;
				array18[0] = bindingExtension8;
				array18[1] = label5;
				array18[2] = stackLayout2;
				array18[3] = stackLayout3;
				array18[4] = grid;
				array18[5] = viewCell;
				object obj12;
				xamlServiceProvider9.Add(typeFromHandle17, obj12 = new SimpleValueTargetProvider(array18, typeof(BindingExtension).GetRuntimeProperty("Converter"), nameScope));
				xamlServiceProvider9.Add(typeof(IReferenceProvider), obj12);
				Type typeFromHandle18 = typeof(IXamlTypeResolver);
				XmlNamespaceResolver xmlNamespaceResolver9 = new XmlNamespaceResolver();
				xmlNamespaceResolver9.Add("", "http://xamarin.com/schemas/2014/forms");
				xmlNamespaceResolver9.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
				xmlNamespaceResolver9.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
				xmlNamespaceResolver9.Add("local", "clr-namespace:CarScannerXamarinForms");
				xmlNamespaceResolver9.Add("settings", "clr-namespace:CarScannerXamarinForms.Settings");
				xamlServiceProvider9.Add(typeFromHandle18, new XamlTypeResolver(xmlNamespaceResolver9, typeof(CustomPIDsListPage.<InitializeComponent>_anonXamlCDataTemplate_85).GetTypeInfo().Assembly));
				xamlServiceProvider9.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(81, 45)));
				object obj13 = markupExtension9.ProvideValue(xamlServiceProvider9);
				bindingExtension8.Converter = obj13;
				bindingExtension8.Path = "IsFormulaCorrect";
				BindingBase bindingBase8 = bindingExtension8.ProvideValue(null);
				label5.SetBinding(VisualElement.IsVisibleProperty, bindingBase8);
				bindingExtension9.Path = "Formula";
				BindingBase bindingBase9 = bindingExtension9.ProvideValue(null);
				label5.SetBinding(Label.TextProperty, bindingBase9);
				label5.SetValue(Label.TextColorProperty, Color.Red);
				stackLayout2.Children.Add(label5);
				stackLayout3.Children.Add(stackLayout2);
				grid.Children.Add(stackLayout3);
				viewCell.View = grid;
				return viewCell;
			}

			// Token: 0x040005E3 RID: 1507
			internal object[] parentValues;

			// Token: 0x040005E4 RID: 1508
			internal CustomPIDsListPage root;
		}
	}
}
