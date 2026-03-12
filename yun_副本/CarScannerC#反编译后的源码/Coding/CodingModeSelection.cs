using System;
using System.CodeDom.Compiler;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using CarScannerXamarinForms.CarPlay;
using CarScannerXamarinForms.Common;
using CarScannerXamarinForms.Pages;
using CarScannerXamarinForms.Settings;
using CarScannerXamarinForms.UserControls;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Internals;
using Xamarin.Forms.PlatformConfiguration.iOSSpecific;
using Xamarin.Forms.Xaml;
using Xamarin.Forms.Xaml.Diagnostics;
using Xamarin.Forms.Xaml.Internals;

namespace CarScannerXamarinForms.Coding
{
	// Token: 0x02000916 RID: 2326
	[XamlCompilation(2)]
	[XamlFilePath("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Coding\\Pages\\CodingModeSelection.xaml")]
	public class CodingModeSelection : ContentPage
	{
		// Token: 0x06004DC8 RID: 19912 RVA: 0x003A07F4 File Offset: 0x0039E9F4
		public CodingModeSelection()
		{
			try
			{
				this.InitializeComponent();
				try
				{
					if ((SharedSettings.Current.SelectedBrand == "Lada" || SharedSettings.Current.SelectedBrand == "Лада" || SharedSettings.Current.SelectedBrand == "VAZ" || SharedSettings.Current.SelectedBrand == "ВАЗ") && (Thread.CurrentThread.CurrentCulture.TwoLetterISOLanguageName.ToUpperInvariant() == "RU" || Thread.CurrentThread.CurrentUICulture.TwoLetterISOLanguageName.ToUpperInvariant() == "RU"))
					{
						this.lbNotSupportedYet.Text = "Либо Ваш автомобиль не поддерживается для кодирования, либо Ваш адаптер не поддерживает кодирование.\nПоддерживаемые модели Лада: Vesta и X-Ray.\nНужен адаптер с реальной поддержкой всех команд ELM327 версии 1.3a или выше.";
					}
				}
				catch (Exception)
				{
				}
			}
			catch (Exception)
			{
			}
		}

		// Token: 0x06004DC9 RID: 19913 RVA: 0x003A08DC File Offset: 0x0039EADC
		private IProgress<string> BuildProgress()
		{
			return new Progress<string>(delegate(string str)
			{
				MainThread.InvokeOnMainThreadAsync(delegate
				{
					this.activityFrame.Text = ActivityFrame.PLEASE_WAIT_TEXT + "\n" + str;
				});
			});
		}

		// Token: 0x06004DCA RID: 19914 RVA: 0x003A08F0 File Offset: 0x0039EAF0
		private async void CodingModeSelection_Appearing(object sender, EventArgs e)
		{
			if (CarPlayManager.Instance != null)
			{
				CarPlayManager instance = CarPlayManager.Instance;
				if (instance != null)
				{
					instance.DisplayNonDismissableAlert(Translate.GetString("carPlay_NotAvailableInCoding"));
				}
			}
			if (!this.warningShowed)
			{
				this.warningShowed = true;
				this.codingModel = new CodingListModel();
				this.lv.BindingContext = this.codingModel;
				this.pickerPlatform.BindingContext = this.codingModel;
				TaskAwaiter<bool> taskAwaiter = base.DisplayAlert(Translate.GetString("DtcPage_CleanCodes_Title"), Translate.GetString("coding_WarningText") + "\n" + this.codingModel.WarningSpecific, Translate.GetString("ios_AGREE"), Translate.GetString("btnCancel.Content")).GetAwaiter();
				if (!taskAwaiter.IsCompleted)
				{
					await taskAwaiter;
					TaskAwaiter<bool> taskAwaiter2;
					taskAwaiter = taskAwaiter2;
					taskAwaiter2 = default(TaskAwaiter<bool>);
				}
				if (!taskAwaiter.GetResult())
				{
					await base.Navigation.PopAsync();
				}
				else if (this.codingModel.IsPlatformSelectorVisible)
				{
					if (string.IsNullOrEmpty(SharedSettings.Current.CodingLastPlatformSelected))
					{
						ValueItemWithTranslation valueItemWithTranslation = this.codingModel.Platforms.FirstOrDefault<ValueItemWithTranslation>();
						if (valueItemWithTranslation != null)
						{
							SharedSettings.Current.CodingLastPlatformSelected = valueItemWithTranslation.Value;
							this.codingModel.CurrentPlatform = valueItemWithTranslation;
							this.activityFrame.Text = ActivityFrame.PLEASE_WAIT_TEXT;
							this.activityFrame.IsVisible = true;
							await this.codingModel.LoadEasyCodingCollection(this.BuildProgress());
							this.activityFrame.IsVisible = false;
						}
						await this.SelectPlatform();
					}
					else
					{
						ValueItemWithTranslation valueItemWithTranslation2 = this.codingModel.Platforms.FirstOrDefault((ValueItemWithTranslation x) => x.Value == SharedSettings.Current.CodingLastPlatformSelected);
						if (valueItemWithTranslation2 != null)
						{
							this.codingModel.CurrentPlatform = valueItemWithTranslation2;
						}
						this.activityFrame.Text = ActivityFrame.PLEASE_WAIT_TEXT;
						this.activityFrame.IsVisible = true;
						await this.codingModel.LoadEasyCodingCollection(this.BuildProgress());
						this.activityFrame.IsVisible = false;
					}
				}
				else
				{
					this.activityFrame.IsVisible = true;
					await this.codingModel.LoadEasyCodingCollection(this.BuildProgress());
					if (this.codingModel.Groups.Count == 0)
					{
						this.lbNotSupportedYet.IsVisible = true;
						this.lv.IsVisible = false;
					}
					this.activityFrame.IsVisible = false;
				}
			}
		}

		// Token: 0x06004DCB RID: 19915 RVA: 0x003A0928 File Offset: 0x0039EB28
		private async Task SelectPlatform()
		{
			SemaphoreSlim semaphore = new SemaphoreSlim(0, 1);
			ItemWithValueSelectorPage itemWithValueSelectorPage = new ItemWithValueSelectorPage(Translate.GetString("coding_vag_choose_platform"), this.codingModel.Platforms, null, delegate(ValueItemWithTranslation selected_item)
			{
				SharedSettings.Current.CodingLastPlatformSelected = selected_item.Value;
				this.codingModel.CurrentPlatform = selected_item;
				semaphore.Release();
			});
			await base.Navigation.PushAsync(itemWithValueSelectorPage);
			base.IsEnabled = true;
			await semaphore.WaitAsync();
			base.IsEnabled = false;
			this.activityFrame.Text = ActivityFrame.PLEASE_WAIT_TEXT;
			this.activityFrame.IsVisible = true;
			await this.codingModel.LoadEasyCodingCollection(this.BuildProgress());
			this.activityFrame.IsVisible = false;
			base.IsEnabled = true;
		}

		// Token: 0x06004DCC RID: 19916 RVA: 0x003A096C File Offset: 0x0039EB6C
		private async void Lv_ItemSelected(object sender, SelectedItemChangedEventArgs e)
		{
			if (this.lv.SelectedItem != null)
			{
				base.IsEnabled = false;
				CodingGroupCollection codingGroupCollection = (CodingGroupCollection)this.lv.SelectedItem;
				this.lv.SelectedItem = null;
				this.lv.IsEnabled = false;
				this.codingModel.SetGroup(codingGroupCollection.Group);
				CodingListPage codingListPage = new CodingListPage(this.codingModel, codingGroupCollection.Name);
				await base.Navigation.PushAsync(codingListPage);
				this.lv.IsEnabled = true;
				base.IsEnabled = true;
			}
		}

		// Token: 0x06004DCD RID: 19917 RVA: 0x003A09A4 File Offset: 0x0039EBA4
		private async void btnBackup_Clicked(object sender, EventArgs e)
		{
			this.btnBackup.IsEnabled = false;
			CodingBackupPage codingBackupPage = new CodingBackupPage();
			await base.Navigation.PushAsync(codingBackupPage);
			this.btnBackup.IsEnabled = true;
		}

		// Token: 0x06004DCE RID: 19918 RVA: 0x003A09DC File Offset: 0x0039EBDC
		private async void platformSelector_Tapped(object sender, EventArgs e)
		{
			base.IsEnabled = false;
			await this.SelectPlatform();
			base.IsEnabled = true;
		}

		// Token: 0x06004DCF RID: 19919 RVA: 0x003A0A14 File Offset: 0x0039EC14
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private void InitializeComponent()
		{
			if (ResourceLoader.IsEnabled && ResourceLoader.CanProvideContentFor(new ResourceLoader.ResourceLoadingQuery
			{
				AssemblyName = typeof(CodingModeSelection).GetTypeInfo().Assembly.GetName(),
				ResourcePath = "Coding/Pages/CodingModeSelection.xaml",
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
			VisualDiagnostics.RegisterSourceInfo(translate = new Translate(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Coding\\Pages\\CodingModeSelection.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 12, 5);
			DynamicResourceExtension dynamicResourceExtension;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension = new DynamicResourceExtension(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Coding\\Pages\\CodingModeSelection.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 16, 5);
			RowDefinition rowDefinition;
			VisualDiagnostics.RegisterSourceInfo(rowDefinition = new RowDefinition(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Coding\\Pages\\CodingModeSelection.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 21, 18);
			RowDefinition rowDefinition2;
			VisualDiagnostics.RegisterSourceInfo(rowDefinition2 = new RowDefinition(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Coding\\Pages\\CodingModeSelection.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 22, 18);
			RowDefinition rowDefinition3;
			VisualDiagnostics.RegisterSourceInfo(rowDefinition3 = new RowDefinition(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Coding\\Pages\\CodingModeSelection.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 23, 18);
			BindingExtension bindingExtension;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension = new BindingExtension(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Coding\\Pages\\CodingModeSelection.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 29, 17);
			BindingExtension bindingExtension2;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension2 = new BindingExtension(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Coding\\Pages\\CodingModeSelection.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 31, 17);
			LabelWithBorder labelWithBorder;
			VisualDiagnostics.RegisterSourceInfo(labelWithBorder = new LabelWithBorder(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Coding\\Pages\\CodingModeSelection.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 26, 14);
			BindingExtension bindingExtension3;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension3 = new BindingExtension(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Coding\\Pages\\CodingModeSelection.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 37, 17);
			DataTemplate dataTemplate;
			VisualDiagnostics.RegisterSourceInfo(dataTemplate = new DataTemplate(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Coding\\Pages\\CodingModeSelection.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 39, 22);
			Translate translate2;
			VisualDiagnostics.RegisterSourceInfo(translate2 = new Translate(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Coding\\Pages\\CodingModeSelection.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 50, 50);
			Label label;
			VisualDiagnostics.RegisterSourceInfo(label = new Label(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Coding\\Pages\\CodingModeSelection.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 50, 22);
			Translate translate3;
			VisualDiagnostics.RegisterSourceInfo(translate3 = new Translate(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Coding\\Pages\\CodingModeSelection.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 54, 56);
			Label label2;
			VisualDiagnostics.RegisterSourceInfo(label2 = new Label(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Coding\\Pages\\CodingModeSelection.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 54, 26);
			Translate translate4;
			VisualDiagnostics.RegisterSourceInfo(translate4 = new Translate(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Coding\\Pages\\CodingModeSelection.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 55, 56);
			Label label3;
			VisualDiagnostics.RegisterSourceInfo(label3 = new Label(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Coding\\Pages\\CodingModeSelection.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 55, 26);
			BindingExtension bindingExtension4;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension4 = new BindingExtension(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Coding\\Pages\\CodingModeSelection.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 56, 56);
			Label label4;
			VisualDiagnostics.RegisterSourceInfo(label4 = new Label(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Coding\\Pages\\CodingModeSelection.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 56, 26);
			StackLayout stackLayout;
			VisualDiagnostics.RegisterSourceInfo(stackLayout = new StackLayout(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Coding\\Pages\\CodingModeSelection.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 53, 22);
			ListView listView;
			VisualDiagnostics.RegisterSourceInfo(listView = new ListView(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Coding\\Pages\\CodingModeSelection.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 32, 14);
			Translate translate5;
			VisualDiagnostics.RegisterSourceInfo(translate5 = new Translate(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Coding\\Pages\\CodingModeSelection.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 63, 17);
			Label label5;
			VisualDiagnostics.RegisterSourceInfo(label5 = new Label(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Coding\\Pages\\CodingModeSelection.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 60, 14);
			ActivityFrame activityFrame;
			VisualDiagnostics.RegisterSourceInfo(activityFrame = new ActivityFrame(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Coding\\Pages\\CodingModeSelection.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 64, 14);
			Translate translate6;
			VisualDiagnostics.RegisterSourceInfo(translate6 = new Translate(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Coding\\Pages\\CodingModeSelection.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 74, 17);
			Button button;
			VisualDiagnostics.RegisterSourceInfo(button = new Button(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Coding\\Pages\\CodingModeSelection.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 68, 14);
			Grid grid;
			VisualDiagnostics.RegisterSourceInfo(grid = new Grid(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Coding\\Pages\\CodingModeSelection.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 19, 10);
			VisualDiagnostics.RegisterSourceInfo(this, new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Coding\\Pages\\CodingModeSelection.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 2, 2);
			NameScope nameScope = NameScope.GetNameScope(this) ?? new NameScope();
			NameScope.SetNameScope(this, nameScope);
			nameScope.RegisterName("pickerPlatform", labelWithBorder);
			if (labelWithBorder.StyleId == null)
			{
				labelWithBorder.StyleId = "pickerPlatform";
			}
			nameScope.RegisterName("lv", listView);
			if (listView.StyleId == null)
			{
				listView.StyleId = "lv";
			}
			nameScope.RegisterName("lbNotSupportedYet", label5);
			if (label5.StyleId == null)
			{
				label5.StyleId = "lbNotSupportedYet";
			}
			nameScope.RegisterName("activityFrame", activityFrame);
			if (activityFrame.StyleId == null)
			{
				activityFrame.StyleId = "activityFrame";
			}
			nameScope.RegisterName("btnBackup", button);
			if (button.StyleId == null)
			{
				button.StyleId = "btnBackup";
			}
			this.pickerPlatform = labelWithBorder;
			this.lv = listView;
			this.lbNotSupportedYet = label5;
			this.activityFrame = activityFrame;
			this.btnBackup = button;
			translate.Text = "coding_Coding";
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
			xmlNamespaceResolver.Add("d", "http://xamarin.com/schemas/2014/forms/design");
			xmlNamespaceResolver.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver.Add("mc", "http://schemas.openxmlformats.org/markup-compatibility/2006");
			xmlNamespaceResolver.Add("settings", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider.Add(typeFromHandle2, new XamlTypeResolver(xmlNamespaceResolver, typeof(CodingModeSelection).GetTypeInfo().Assembly));
			xamlServiceProvider.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(12, 5)));
			object obj2 = markupExtension.ProvideValue(xamlServiceProvider);
			this.Title = obj2;
			this.SetValue(Page.PaddingProperty, new Thickness(5.0, 0.0));
			this.SetValue(Page.UseSafeAreaProperty, true);
			this.Appearing += this.CodingModeSelection_Appearing;
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
			xmlNamespaceResolver2.Add("d", "http://xamarin.com/schemas/2014/forms/design");
			xmlNamespaceResolver2.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver2.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver2.Add("mc", "http://schemas.openxmlformats.org/markup-compatibility/2006");
			xmlNamespaceResolver2.Add("settings", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver2.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider2.Add(typeFromHandle4, new XamlTypeResolver(xmlNamespaceResolver2, typeof(CodingModeSelection).GetTypeInfo().Assembly));
			xamlServiceProvider2.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(16, 5)));
			DynamicResource dynamicResource = markupExtension2.ProvideValue(xamlServiceProvider2);
			this.SetDynamicResource(VisualElement.BackgroundColorProperty, dynamicResource.Key);
			grid.SetValue(View.MarginProperty, new Thickness(5.0, 5.0, 5.0, 0.0));
			rowDefinition.SetValue(RowDefinition.HeightProperty, new GridLengthTypeConverter().ConvertFromInvariantString("auto"));
			grid.GetValue(Grid.RowDefinitionsProperty).Add(rowDefinition);
			rowDefinition2.SetValue(RowDefinition.HeightProperty, new GridLengthTypeConverter().ConvertFromInvariantString("*"));
			grid.GetValue(Grid.RowDefinitionsProperty).Add(rowDefinition2);
			rowDefinition3.SetValue(RowDefinition.HeightProperty, new GridLengthTypeConverter().ConvertFromInvariantString("auto"));
			grid.GetValue(Grid.RowDefinitionsProperty).Add(rowDefinition3);
			labelWithBorder.SetValue(Grid.RowProperty, 0);
			bindingExtension.Mode = 2;
			bindingExtension.Path = "IsPlatformSelectorVisible";
			BindingBase bindingBase = bindingExtension.ProvideValue(null);
			labelWithBorder.SetBinding(VisualElement.IsVisibleProperty, bindingBase);
			labelWithBorder.Tapped += this.platformSelector_Tapped;
			bindingExtension2.Mode = 2;
			bindingExtension2.Path = "CurrentPlatform.Title";
			BindingBase bindingBase2 = bindingExtension2.ProvideValue(null);
			labelWithBorder.SetBinding(LabelWithBorder.TextProperty, bindingBase2);
			grid.Children.Add(labelWithBorder);
			listView.SetValue(Grid.RowProperty, 1);
			listView.SetValue(ListView.HasUnevenRowsProperty, true);
			listView.ItemSelected += this.Lv_ItemSelected;
			bindingExtension3.Path = "Groups";
			BindingBase bindingBase3 = bindingExtension3.ProvideValue(null);
			listView.SetBinding(ItemsView<Cell>.ItemsSourceProperty, bindingBase3);
			IDataTemplate dataTemplate2 = dataTemplate;
			CodingModeSelection.<InitializeComponent>_anonXamlCDataTemplate_99 <InitializeComponent>_anonXamlCDataTemplate_ = new CodingModeSelection.<InitializeComponent>_anonXamlCDataTemplate_99();
			object[] array3 = new object[0 + 4];
			array3[0] = dataTemplate;
			array3[1] = listView;
			array3[2] = grid;
			array3[3] = this;
			<InitializeComponent>_anonXamlCDataTemplate_.parentValues = array3;
			<InitializeComponent>_anonXamlCDataTemplate_.root = this;
			dataTemplate2.LoadTemplate = new Func<object>(<InitializeComponent>_anonXamlCDataTemplate_.LoadDataTemplate);
			listView.SetValue(ItemsView<Cell>.ItemTemplateProperty, dataTemplate);
			label.SetValue(Label.FontAttributesProperty, new FontAttributesConverter().ConvertFromInvariantString("Bold"));
			translate2.Text = "coding_Category";
			IMarkupExtension markupExtension3 = translate2;
			XamlServiceProvider xamlServiceProvider3 = new XamlServiceProvider();
			Type typeFromHandle5 = typeof(IProvideValueTarget);
			object[] array4 = new object[0 + 4];
			array4[0] = label;
			array4[1] = listView;
			array4[2] = grid;
			array4[3] = this;
			object obj4;
			xamlServiceProvider3.Add(typeFromHandle5, obj4 = new SimpleValueTargetProvider(array4, Label.TextProperty, nameScope));
			xamlServiceProvider3.Add(typeof(IReferenceProvider), obj4);
			Type typeFromHandle6 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver3 = new XmlNamespaceResolver();
			xmlNamespaceResolver3.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver3.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver3.Add("d", "http://xamarin.com/schemas/2014/forms/design");
			xmlNamespaceResolver3.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver3.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver3.Add("mc", "http://schemas.openxmlformats.org/markup-compatibility/2006");
			xmlNamespaceResolver3.Add("settings", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver3.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider3.Add(typeFromHandle6, new XamlTypeResolver(xmlNamespaceResolver3, typeof(CodingModeSelection).GetTypeInfo().Assembly));
			xamlServiceProvider3.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(50, 50)));
			object obj5 = markupExtension3.ProvideValue(xamlServiceProvider3);
			label.Text = obj5;
			listView.SetValue(ListView.HeaderProperty, label);
			stackLayout.SetValue(View.MarginProperty, new Thickness(0.0, 5.0, 0.0, 0.0));
			stackLayout.SetValue(StackLayout.OrientationProperty, 0);
			label2.SetValue(Label.FontAttributesProperty, new FontAttributesConverter().ConvertFromInvariantString("Italic"));
			translate3.Text = "DtcPage_CleanCodes_Title";
			IMarkupExtension markupExtension4 = translate3;
			XamlServiceProvider xamlServiceProvider4 = new XamlServiceProvider();
			Type typeFromHandle7 = typeof(IProvideValueTarget);
			object[] array5 = new object[0 + 5];
			array5[0] = label2;
			array5[1] = stackLayout;
			array5[2] = listView;
			array5[3] = grid;
			array5[4] = this;
			object obj6;
			xamlServiceProvider4.Add(typeFromHandle7, obj6 = new SimpleValueTargetProvider(array5, Label.TextProperty, nameScope));
			xamlServiceProvider4.Add(typeof(IReferenceProvider), obj6);
			Type typeFromHandle8 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver4 = new XmlNamespaceResolver();
			xmlNamespaceResolver4.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver4.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver4.Add("d", "http://xamarin.com/schemas/2014/forms/design");
			xmlNamespaceResolver4.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver4.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver4.Add("mc", "http://schemas.openxmlformats.org/markup-compatibility/2006");
			xmlNamespaceResolver4.Add("settings", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver4.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider4.Add(typeFromHandle8, new XamlTypeResolver(xmlNamespaceResolver4, typeof(CodingModeSelection).GetTypeInfo().Assembly));
			xamlServiceProvider4.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(54, 56)));
			object obj7 = markupExtension4.ProvideValue(xamlServiceProvider4);
			label2.Text = obj7;
			stackLayout.Children.Add(label2);
			label3.SetValue(Label.FontAttributesProperty, new FontAttributesConverter().ConvertFromInvariantString("Italic"));
			translate4.Text = "coding_WarningText";
			IMarkupExtension markupExtension5 = translate4;
			XamlServiceProvider xamlServiceProvider5 = new XamlServiceProvider();
			Type typeFromHandle9 = typeof(IProvideValueTarget);
			object[] array6 = new object[0 + 5];
			array6[0] = label3;
			array6[1] = stackLayout;
			array6[2] = listView;
			array6[3] = grid;
			array6[4] = this;
			object obj8;
			xamlServiceProvider5.Add(typeFromHandle9, obj8 = new SimpleValueTargetProvider(array6, Label.TextProperty, nameScope));
			xamlServiceProvider5.Add(typeof(IReferenceProvider), obj8);
			Type typeFromHandle10 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver5 = new XmlNamespaceResolver();
			xmlNamespaceResolver5.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver5.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver5.Add("d", "http://xamarin.com/schemas/2014/forms/design");
			xmlNamespaceResolver5.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver5.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver5.Add("mc", "http://schemas.openxmlformats.org/markup-compatibility/2006");
			xmlNamespaceResolver5.Add("settings", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver5.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider5.Add(typeFromHandle10, new XamlTypeResolver(xmlNamespaceResolver5, typeof(CodingModeSelection).GetTypeInfo().Assembly));
			xamlServiceProvider5.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(55, 56)));
			object obj9 = markupExtension5.ProvideValue(xamlServiceProvider5);
			label3.Text = obj9;
			stackLayout.Children.Add(label3);
			label4.SetValue(Label.FontAttributesProperty, new FontAttributesConverter().ConvertFromInvariantString("Italic"));
			bindingExtension4.Path = "WarningSpecific";
			BindingBase bindingBase4 = bindingExtension4.ProvideValue(null);
			label4.SetBinding(Label.TextProperty, bindingBase4);
			stackLayout.Children.Add(label4);
			listView.SetValue(ListView.FooterProperty, stackLayout);
			grid.Children.Add(listView);
			label5.SetValue(VisualElement.IsVisibleProperty, new VisualElement.VisibilityConverter().ConvertFromInvariantString("false"));
			translate5.Text = "coding_CarNotSupported";
			IMarkupExtension markupExtension6 = translate5;
			XamlServiceProvider xamlServiceProvider6 = new XamlServiceProvider();
			Type typeFromHandle11 = typeof(IProvideValueTarget);
			object[] array7 = new object[0 + 3];
			array7[0] = label5;
			array7[1] = grid;
			array7[2] = this;
			object obj10;
			xamlServiceProvider6.Add(typeFromHandle11, obj10 = new SimpleValueTargetProvider(array7, Label.TextProperty, nameScope));
			xamlServiceProvider6.Add(typeof(IReferenceProvider), obj10);
			Type typeFromHandle12 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver6 = new XmlNamespaceResolver();
			xmlNamespaceResolver6.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver6.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver6.Add("d", "http://xamarin.com/schemas/2014/forms/design");
			xmlNamespaceResolver6.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver6.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver6.Add("mc", "http://schemas.openxmlformats.org/markup-compatibility/2006");
			xmlNamespaceResolver6.Add("settings", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver6.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider6.Add(typeFromHandle12, new XamlTypeResolver(xmlNamespaceResolver6, typeof(CodingModeSelection).GetTypeInfo().Assembly));
			xamlServiceProvider6.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(63, 17)));
			object obj11 = markupExtension6.ProvideValue(xamlServiceProvider6);
			label5.Text = obj11;
			grid.Children.Add(label5);
			activityFrame.SetValue(Grid.RowProperty, 1);
			activityFrame.SetValue(VisualElement.IsVisibleProperty, new VisualElement.VisibilityConverter().ConvertFromInvariantString("False"));
			grid.Children.Add(activityFrame);
			button.SetValue(Grid.RowProperty, 2);
			button.SetValue(VisualElement.BackgroundColorProperty, Color.DarkBlue);
			button.Clicked += this.btnBackup_Clicked;
			button.SetValue(Button.CornerRadiusProperty, 9);
			translate6.Text = "coding_History";
			IMarkupExtension markupExtension7 = translate6;
			XamlServiceProvider xamlServiceProvider7 = new XamlServiceProvider();
			Type typeFromHandle13 = typeof(IProvideValueTarget);
			object[] array8 = new object[0 + 3];
			array8[0] = button;
			array8[1] = grid;
			array8[2] = this;
			object obj12;
			xamlServiceProvider7.Add(typeFromHandle13, obj12 = new SimpleValueTargetProvider(array8, Button.TextProperty, nameScope));
			xamlServiceProvider7.Add(typeof(IReferenceProvider), obj12);
			Type typeFromHandle14 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver7 = new XmlNamespaceResolver();
			xmlNamespaceResolver7.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver7.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver7.Add("d", "http://xamarin.com/schemas/2014/forms/design");
			xmlNamespaceResolver7.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver7.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver7.Add("mc", "http://schemas.openxmlformats.org/markup-compatibility/2006");
			xmlNamespaceResolver7.Add("settings", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver7.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider7.Add(typeFromHandle14, new XamlTypeResolver(xmlNamespaceResolver7, typeof(CodingModeSelection).GetTypeInfo().Assembly));
			xamlServiceProvider7.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(74, 17)));
			object obj13 = markupExtension7.ProvideValue(xamlServiceProvider7);
			button.Text = obj13;
			button.SetValue(Button.TextColorProperty, Color.White);
			grid.Children.Add(button);
			this.SetValue(ContentPage.ContentProperty, grid);
		}

		// Token: 0x06004DD0 RID: 19920 RVA: 0x003A1CE7 File Offset: 0x0039FEE7
		[CompilerGenerated]
		private void <BuildProgress>b__2_0(string str)
		{
			MainThread.InvokeOnMainThreadAsync(delegate
			{
				this.activityFrame.Text = ActivityFrame.PLEASE_WAIT_TEXT + "\n" + str;
			});
		}

		// Token: 0x06004DD1 RID: 19921 RVA: 0x003A1D10 File Offset: 0x0039FF10
		private void __InitComponentRuntime()
		{
			Extensions.LoadFromXaml<CodingModeSelection>(this, typeof(CodingModeSelection));
			this.pickerPlatform = NameScopeExtensions.FindByName<LabelWithBorder>(this, "pickerPlatform");
			this.lv = NameScopeExtensions.FindByName<ListView>(this, "lv");
			this.lbNotSupportedYet = NameScopeExtensions.FindByName<Label>(this, "lbNotSupportedYet");
			this.activityFrame = NameScopeExtensions.FindByName<ActivityFrame>(this, "activityFrame");
			this.btnBackup = NameScopeExtensions.FindByName<Button>(this, "btnBackup");
		}

		// Token: 0x04002E41 RID: 11841
		private bool warningShowed;

		// Token: 0x04002E42 RID: 11842
		private CodingListModel codingModel;

		// Token: 0x04002E43 RID: 11843
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private LabelWithBorder pickerPlatform;

		// Token: 0x04002E44 RID: 11844
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private ListView lv;

		// Token: 0x04002E45 RID: 11845
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private Label lbNotSupportedYet;

		// Token: 0x04002E46 RID: 11846
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private ActivityFrame activityFrame;

		// Token: 0x04002E47 RID: 11847
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private Button btnBackup;

		// Token: 0x02000917 RID: 2327
		[CompilerGenerated]
		[Serializable]
		private sealed class <>c
		{
			// Token: 0x06004DD2 RID: 19922 RVA: 0x003A1D83 File Offset: 0x0039FF83
			// Note: this type is marked as 'beforefieldinit'.
			static <>c()
			{
			}

			// Token: 0x06004DD3 RID: 19923 RVA: 0x00002050 File Offset: 0x00000250
			public <>c()
			{
			}

			// Token: 0x06004DD4 RID: 19924 RVA: 0x003A1D8F File Offset: 0x0039FF8F
			internal bool <CodingModeSelection_Appearing>b__3_0(ValueItemWithTranslation x)
			{
				return x.Value == SharedSettings.Current.CodingLastPlatformSelected;
			}

			// Token: 0x04002E48 RID: 11848
			public static readonly CodingModeSelection.<>c <>9 = new CodingModeSelection.<>c();

			// Token: 0x04002E49 RID: 11849
			public static Func<ValueItemWithTranslation, bool> <>9__3_0;
		}

		// Token: 0x02000918 RID: 2328
		[CompilerGenerated]
		private sealed class <>c__DisplayClass2_0
		{
			// Token: 0x06004DD5 RID: 19925 RVA: 0x00002050 File Offset: 0x00000250
			public <>c__DisplayClass2_0()
			{
			}

			// Token: 0x06004DD6 RID: 19926 RVA: 0x003A1DA6 File Offset: 0x0039FFA6
			internal void <BuildProgress>b__1()
			{
				this.<>4__this.activityFrame.Text = ActivityFrame.PLEASE_WAIT_TEXT + "\n" + this.str;
			}

			// Token: 0x04002E4A RID: 11850
			public string str;

			// Token: 0x04002E4B RID: 11851
			public CodingModeSelection <>4__this;
		}

		// Token: 0x02000919 RID: 2329
		[CompilerGenerated]
		private sealed class <>c__DisplayClass4_0
		{
			// Token: 0x06004DD7 RID: 19927 RVA: 0x00002050 File Offset: 0x00000250
			public <>c__DisplayClass4_0()
			{
			}

			// Token: 0x06004DD8 RID: 19928 RVA: 0x003A1DCD File Offset: 0x0039FFCD
			internal void <SelectPlatform>b__0(ValueItemWithTranslation selected_item)
			{
				SharedSettings.Current.CodingLastPlatformSelected = selected_item.Value;
				this.<>4__this.codingModel.CurrentPlatform = selected_item;
				this.semaphore.Release();
			}

			// Token: 0x04002E4C RID: 11852
			public CodingModeSelection <>4__this;

			// Token: 0x04002E4D RID: 11853
			public SemaphoreSlim semaphore;
		}

		// Token: 0x0200091A RID: 2330
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <CodingModeSelection_Appearing>d__3 : IAsyncStateMachine
		{
			// Token: 0x06004DD9 RID: 19929 RVA: 0x003A1DFC File Offset: 0x0039FFFC
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				CodingModeSelection codingModeSelection = this;
				try
				{
					TaskAwaiter<bool> taskAwaiter3;
					TaskAwaiter<Page> taskAwaiter4;
					TaskAwaiter taskAwaiter6;
					switch (num)
					{
					case 0:
						taskAwaiter3 = taskAwaiter2;
						taskAwaiter2 = default(TaskAwaiter<bool>);
						num2 = -1;
						break;
					case 1:
					{
						TaskAwaiter<Page> taskAwaiter5;
						taskAwaiter4 = taskAwaiter5;
						taskAwaiter5 = default(TaskAwaiter<Page>);
						num2 = -1;
						goto IL_017F;
					}
					case 2:
					{
						TaskAwaiter taskAwaiter7;
						taskAwaiter6 = taskAwaiter7;
						taskAwaiter7 = default(TaskAwaiter);
						num2 = -1;
						goto IL_0265;
					}
					case 3:
					{
						TaskAwaiter taskAwaiter7;
						taskAwaiter6 = taskAwaiter7;
						taskAwaiter7 = default(TaskAwaiter);
						num2 = -1;
						goto IL_02CF;
					}
					case 4:
					{
						TaskAwaiter taskAwaiter7;
						taskAwaiter6 = taskAwaiter7;
						taskAwaiter7 = default(TaskAwaiter);
						num2 = -1;
						goto IL_039B;
					}
					case 5:
					{
						TaskAwaiter taskAwaiter7;
						taskAwaiter6 = taskAwaiter7;
						taskAwaiter7 = default(TaskAwaiter);
						num2 = -1;
						goto IL_0421;
					}
					default:
						if (CarPlayManager.Instance != null)
						{
							CarPlayManager instance = CarPlayManager.Instance;
							if (instance != null)
							{
								instance.DisplayNonDismissableAlert(Translate.GetString("carPlay_NotAvailableInCoding"));
							}
						}
						if (codingModeSelection.warningShowed)
						{
							goto IL_045E;
						}
						codingModeSelection.warningShowed = true;
						codingModeSelection.codingModel = new CodingListModel();
						codingModeSelection.lv.BindingContext = codingModeSelection.codingModel;
						codingModeSelection.pickerPlatform.BindingContext = codingModeSelection.codingModel;
						taskAwaiter3 = codingModeSelection.DisplayAlert(Translate.GetString("DtcPage_CleanCodes_Title"), Translate.GetString("coding_WarningText") + "\n" + codingModeSelection.codingModel.WarningSpecific, Translate.GetString("ios_AGREE"), Translate.GetString("btnCancel.Content")).GetAwaiter();
						if (!taskAwaiter3.IsCompleted)
						{
							num2 = 0;
							taskAwaiter2 = taskAwaiter3;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<bool>, CodingModeSelection.<CodingModeSelection_Appearing>d__3>(ref taskAwaiter3, ref this);
							return;
						}
						break;
					}
					if (!taskAwaiter3.GetResult())
					{
						taskAwaiter4 = codingModeSelection.Navigation.PopAsync().GetAwaiter();
						if (!taskAwaiter4.IsCompleted)
						{
							num2 = 1;
							TaskAwaiter<Page> taskAwaiter5 = taskAwaiter4;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<Page>, CodingModeSelection.<CodingModeSelection_Appearing>d__3>(ref taskAwaiter4, ref this);
							return;
						}
					}
					else if (codingModeSelection.codingModel.IsPlatformSelectorVisible)
					{
						if (string.IsNullOrEmpty(SharedSettings.Current.CodingLastPlatformSelected))
						{
							ValueItemWithTranslation valueItemWithTranslation = codingModeSelection.codingModel.Platforms.FirstOrDefault<ValueItemWithTranslation>();
							if (valueItemWithTranslation == null)
							{
								goto IL_0278;
							}
							SharedSettings.Current.CodingLastPlatformSelected = valueItemWithTranslation.Value;
							codingModeSelection.codingModel.CurrentPlatform = valueItemWithTranslation;
							codingModeSelection.activityFrame.Text = ActivityFrame.PLEASE_WAIT_TEXT;
							codingModeSelection.activityFrame.IsVisible = true;
							taskAwaiter6 = codingModeSelection.codingModel.LoadEasyCodingCollection(codingModeSelection.BuildProgress()).GetAwaiter();
							if (!taskAwaiter6.IsCompleted)
							{
								num2 = 2;
								TaskAwaiter taskAwaiter7 = taskAwaiter6;
								this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, CodingModeSelection.<CodingModeSelection_Appearing>d__3>(ref taskAwaiter6, ref this);
								return;
							}
							goto IL_0265;
						}
						else
						{
							ValueItemWithTranslation valueItemWithTranslation2 = codingModeSelection.codingModel.Platforms.FirstOrDefault((ValueItemWithTranslation x) => x.Value == SharedSettings.Current.CodingLastPlatformSelected);
							if (valueItemWithTranslation2 != null)
							{
								codingModeSelection.codingModel.CurrentPlatform = valueItemWithTranslation2;
							}
							codingModeSelection.activityFrame.Text = ActivityFrame.PLEASE_WAIT_TEXT;
							codingModeSelection.activityFrame.IsVisible = true;
							taskAwaiter6 = codingModeSelection.codingModel.LoadEasyCodingCollection(codingModeSelection.BuildProgress()).GetAwaiter();
							if (!taskAwaiter6.IsCompleted)
							{
								num2 = 4;
								TaskAwaiter taskAwaiter7 = taskAwaiter6;
								this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, CodingModeSelection.<CodingModeSelection_Appearing>d__3>(ref taskAwaiter6, ref this);
								return;
							}
							goto IL_039B;
						}
					}
					else
					{
						codingModeSelection.activityFrame.IsVisible = true;
						taskAwaiter6 = codingModeSelection.codingModel.LoadEasyCodingCollection(codingModeSelection.BuildProgress()).GetAwaiter();
						if (!taskAwaiter6.IsCompleted)
						{
							num2 = 5;
							TaskAwaiter taskAwaiter7 = taskAwaiter6;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, CodingModeSelection.<CodingModeSelection_Appearing>d__3>(ref taskAwaiter6, ref this);
							return;
						}
						goto IL_0421;
					}
					IL_017F:
					taskAwaiter4.GetResult();
					goto IL_045E;
					IL_0265:
					taskAwaiter6.GetResult();
					codingModeSelection.activityFrame.IsVisible = false;
					IL_0278:
					taskAwaiter6 = codingModeSelection.SelectPlatform().GetAwaiter();
					if (!taskAwaiter6.IsCompleted)
					{
						num2 = 3;
						TaskAwaiter taskAwaiter7 = taskAwaiter6;
						this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, CodingModeSelection.<CodingModeSelection_Appearing>d__3>(ref taskAwaiter6, ref this);
						return;
					}
					IL_02CF:
					taskAwaiter6.GetResult();
					goto IL_045E;
					IL_039B:
					taskAwaiter6.GetResult();
					codingModeSelection.activityFrame.IsVisible = false;
					goto IL_045E;
					IL_0421:
					taskAwaiter6.GetResult();
					if (codingModeSelection.codingModel.Groups.Count == 0)
					{
						codingModeSelection.lbNotSupportedYet.IsVisible = true;
						codingModeSelection.lv.IsVisible = false;
					}
					codingModeSelection.activityFrame.IsVisible = false;
					IL_045E:;
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

			// Token: 0x06004DDA RID: 19930 RVA: 0x003A22B4 File Offset: 0x003A04B4
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x04002E4E RID: 11854
			public int <>1__state;

			// Token: 0x04002E4F RID: 11855
			public AsyncVoidMethodBuilder <>t__builder;

			// Token: 0x04002E50 RID: 11856
			public CodingModeSelection <>4__this;

			// Token: 0x04002E51 RID: 11857
			private TaskAwaiter<bool> <>u__1;

			// Token: 0x04002E52 RID: 11858
			private TaskAwaiter<Page> <>u__2;

			// Token: 0x04002E53 RID: 11859
			private TaskAwaiter <>u__3;
		}

		// Token: 0x0200091B RID: 2331
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <Lv_ItemSelected>d__5 : IAsyncStateMachine
		{
			// Token: 0x06004DDB RID: 19931 RVA: 0x003A22C4 File Offset: 0x003A04C4
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				CodingModeSelection codingModeSelection = this;
				try
				{
					TaskAwaiter taskAwaiter;
					if (num != 0)
					{
						if (codingModeSelection.lv.SelectedItem == null)
						{
							goto IL_0108;
						}
						codingModeSelection.IsEnabled = false;
						CodingGroupCollection codingGroupCollection = (CodingGroupCollection)codingModeSelection.lv.SelectedItem;
						codingModeSelection.lv.SelectedItem = null;
						codingModeSelection.lv.IsEnabled = false;
						codingModeSelection.codingModel.SetGroup(codingGroupCollection.Group);
						CodingListPage codingListPage = new CodingListPage(codingModeSelection.codingModel, codingGroupCollection.Name);
						taskAwaiter = codingModeSelection.Navigation.PushAsync(codingListPage).GetAwaiter();
						if (!taskAwaiter.IsCompleted)
						{
							num2 = 0;
							TaskAwaiter taskAwaiter2 = taskAwaiter;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, CodingModeSelection.<Lv_ItemSelected>d__5>(ref taskAwaiter, ref this);
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
					codingModeSelection.lv.IsEnabled = true;
					codingModeSelection.IsEnabled = true;
				}
				catch (Exception ex)
				{
					num2 = -2;
					this.<>t__builder.SetException(ex);
					return;
				}
				IL_0108:
				num2 = -2;
				this.<>t__builder.SetResult();
			}

			// Token: 0x06004DDC RID: 19932 RVA: 0x003A23FC File Offset: 0x003A05FC
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x04002E54 RID: 11860
			public int <>1__state;

			// Token: 0x04002E55 RID: 11861
			public AsyncVoidMethodBuilder <>t__builder;

			// Token: 0x04002E56 RID: 11862
			public CodingModeSelection <>4__this;

			// Token: 0x04002E57 RID: 11863
			private TaskAwaiter <>u__1;
		}

		// Token: 0x0200091C RID: 2332
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <SelectPlatform>d__4 : IAsyncStateMachine
		{
			// Token: 0x06004DDD RID: 19933 RVA: 0x003A240C File Offset: 0x003A060C
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				CodingModeSelection codingModeSelection = this;
				try
				{
					TaskAwaiter taskAwaiter;
					switch (num)
					{
					case 0:
					{
						TaskAwaiter taskAwaiter2;
						taskAwaiter = taskAwaiter2;
						taskAwaiter2 = default(TaskAwaiter);
						num2 = -1;
						break;
					}
					case 1:
					{
						TaskAwaiter taskAwaiter2;
						taskAwaiter = taskAwaiter2;
						taskAwaiter2 = default(TaskAwaiter);
						num2 = -1;
						goto IL_0141;
					}
					case 2:
					{
						TaskAwaiter taskAwaiter2;
						taskAwaiter = taskAwaiter2;
						taskAwaiter2 = default(TaskAwaiter);
						num2 = -1;
						goto IL_01C7;
					}
					default:
					{
						CS$<>8__locals1 = new CodingModeSelection.<>c__DisplayClass4_0();
						CS$<>8__locals1.<>4__this = this;
						CS$<>8__locals1.semaphore = new SemaphoreSlim(0, 1);
						ItemWithValueSelectorPage itemWithValueSelectorPage = new ItemWithValueSelectorPage(Translate.GetString("coding_vag_choose_platform"), codingModeSelection.codingModel.Platforms, null, delegate(ValueItemWithTranslation selected_item)
						{
							SharedSettings.Current.CodingLastPlatformSelected = selected_item.Value;
							CS$<>8__locals1.<>4__this.codingModel.CurrentPlatform = selected_item;
							CS$<>8__locals1.semaphore.Release();
						});
						taskAwaiter = codingModeSelection.Navigation.PushAsync(itemWithValueSelectorPage).GetAwaiter();
						if (!taskAwaiter.IsCompleted)
						{
							num2 = 0;
							TaskAwaiter taskAwaiter2 = taskAwaiter;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, CodingModeSelection.<SelectPlatform>d__4>(ref taskAwaiter, ref this);
							return;
						}
						break;
					}
					}
					taskAwaiter.GetResult();
					codingModeSelection.IsEnabled = true;
					taskAwaiter = CS$<>8__locals1.semaphore.WaitAsync().GetAwaiter();
					if (!taskAwaiter.IsCompleted)
					{
						num2 = 1;
						TaskAwaiter taskAwaiter2 = taskAwaiter;
						this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, CodingModeSelection.<SelectPlatform>d__4>(ref taskAwaiter, ref this);
						return;
					}
					IL_0141:
					taskAwaiter.GetResult();
					codingModeSelection.IsEnabled = false;
					codingModeSelection.activityFrame.Text = ActivityFrame.PLEASE_WAIT_TEXT;
					codingModeSelection.activityFrame.IsVisible = true;
					taskAwaiter = codingModeSelection.codingModel.LoadEasyCodingCollection(codingModeSelection.BuildProgress()).GetAwaiter();
					if (!taskAwaiter.IsCompleted)
					{
						num2 = 2;
						TaskAwaiter taskAwaiter2 = taskAwaiter;
						this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, CodingModeSelection.<SelectPlatform>d__4>(ref taskAwaiter, ref this);
						return;
					}
					IL_01C7:
					taskAwaiter.GetResult();
					codingModeSelection.activityFrame.IsVisible = false;
					codingModeSelection.IsEnabled = true;
				}
				catch (Exception ex)
				{
					num2 = -2;
					CS$<>8__locals1 = null;
					this.<>t__builder.SetException(ex);
					return;
				}
				num2 = -2;
				CS$<>8__locals1 = null;
				this.<>t__builder.SetResult();
			}

			// Token: 0x06004DDE RID: 19934 RVA: 0x003A2654 File Offset: 0x003A0854
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x04002E58 RID: 11864
			public int <>1__state;

			// Token: 0x04002E59 RID: 11865
			public AsyncTaskMethodBuilder <>t__builder;

			// Token: 0x04002E5A RID: 11866
			public CodingModeSelection <>4__this;

			// Token: 0x04002E5B RID: 11867
			private CodingModeSelection.<>c__DisplayClass4_0 <>8__1;

			// Token: 0x04002E5C RID: 11868
			private TaskAwaiter <>u__1;
		}

		// Token: 0x0200091D RID: 2333
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <btnBackup_Clicked>d__7 : IAsyncStateMachine
		{
			// Token: 0x06004DDF RID: 19935 RVA: 0x003A2664 File Offset: 0x003A0864
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				CodingModeSelection codingModeSelection = this;
				try
				{
					TaskAwaiter taskAwaiter;
					if (num != 0)
					{
						codingModeSelection.btnBackup.IsEnabled = false;
						CodingBackupPage codingBackupPage = new CodingBackupPage();
						taskAwaiter = codingModeSelection.Navigation.PushAsync(codingBackupPage).GetAwaiter();
						if (!taskAwaiter.IsCompleted)
						{
							num2 = 0;
							TaskAwaiter taskAwaiter2 = taskAwaiter;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, CodingModeSelection.<btnBackup_Clicked>d__7>(ref taskAwaiter, ref this);
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
					codingModeSelection.btnBackup.IsEnabled = true;
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

			// Token: 0x06004DE0 RID: 19936 RVA: 0x003A273C File Offset: 0x003A093C
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x04002E5D RID: 11869
			public int <>1__state;

			// Token: 0x04002E5E RID: 11870
			public AsyncVoidMethodBuilder <>t__builder;

			// Token: 0x04002E5F RID: 11871
			public CodingModeSelection <>4__this;

			// Token: 0x04002E60 RID: 11872
			private TaskAwaiter <>u__1;
		}

		// Token: 0x0200091E RID: 2334
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <platformSelector_Tapped>d__8 : IAsyncStateMachine
		{
			// Token: 0x06004DE1 RID: 19937 RVA: 0x003A274C File Offset: 0x003A094C
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				CodingModeSelection codingModeSelection = this;
				try
				{
					TaskAwaiter taskAwaiter;
					if (num != 0)
					{
						codingModeSelection.IsEnabled = false;
						taskAwaiter = codingModeSelection.SelectPlatform().GetAwaiter();
						if (!taskAwaiter.IsCompleted)
						{
							num2 = 0;
							TaskAwaiter taskAwaiter2 = taskAwaiter;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, CodingModeSelection.<platformSelector_Tapped>d__8>(ref taskAwaiter, ref this);
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
					codingModeSelection.IsEnabled = true;
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

			// Token: 0x06004DE2 RID: 19938 RVA: 0x003A280C File Offset: 0x003A0A0C
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x04002E61 RID: 11873
			public int <>1__state;

			// Token: 0x04002E62 RID: 11874
			public AsyncVoidMethodBuilder <>t__builder;

			// Token: 0x04002E63 RID: 11875
			public CodingModeSelection <>4__this;

			// Token: 0x04002E64 RID: 11876
			private TaskAwaiter <>u__1;
		}

		// Token: 0x0200091F RID: 2335
		[CompilerGenerated]
		private sealed class <InitializeComponent>_anonXamlCDataTemplate_99
		{
			// Token: 0x06004DE3 RID: 19939 RVA: 0x003A281C File Offset: 0x003A0A1C
			public <InitializeComponent>_anonXamlCDataTemplate_99()
			{
			}

			// Token: 0x06004DE4 RID: 19940 RVA: 0x003A2830 File Offset: 0x003A0A30
			internal object LoadDataTemplate()
			{
				DynamicResourceExtension dynamicResourceExtension;
				VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension = new DynamicResourceExtension(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Coding\\Pages\\CodingModeSelection.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 43, 33);
				BindingExtension bindingExtension;
				VisualDiagnostics.RegisterSourceInfo(bindingExtension = new BindingExtension(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Coding\\Pages\\CodingModeSelection.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 44, 33);
				DynamicResourceExtension dynamicResourceExtension2;
				VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension2 = new DynamicResourceExtension(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Coding\\Pages\\CodingModeSelection.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 45, 33);
				Label label;
				VisualDiagnostics.RegisterSourceInfo(label = new Label(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Coding\\Pages\\CodingModeSelection.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 41, 30);
				ViewCell viewCell;
				VisualDiagnostics.RegisterSourceInfo(viewCell = new ViewCell(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Coding\\Pages\\CodingModeSelection.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 40, 26);
				NameScope nameScope = new NameScope();
				NameScope.SetNameScope(viewCell, nameScope);
				label.SetValue(View.MarginProperty, new Thickness(0.0, 2.0, 0.0, 5.0));
				dynamicResourceExtension.Key = "BaseFontSize++";
				IMarkupExtension<DynamicResource> markupExtension = dynamicResourceExtension;
				XamlServiceProvider xamlServiceProvider = new XamlServiceProvider();
				Type typeFromHandle = typeof(IProvideValueTarget);
				int num;
				object[] array = new object[(num = this.parentValues.Length) + 2];
				Array.Copy(this.parentValues, 0, array, 2, num);
				object[] array2 = array;
				array2[0] = label;
				array2[1] = viewCell;
				object obj;
				xamlServiceProvider.Add(typeFromHandle, obj = new SimpleValueTargetProvider(array2, Label.FontSizeProperty, nameScope));
				xamlServiceProvider.Add(typeof(IReferenceProvider), obj);
				Type typeFromHandle2 = typeof(IXamlTypeResolver);
				XmlNamespaceResolver xmlNamespaceResolver = new XmlNamespaceResolver();
				xmlNamespaceResolver.Add("", "http://xamarin.com/schemas/2014/forms");
				xmlNamespaceResolver.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
				xmlNamespaceResolver.Add("d", "http://xamarin.com/schemas/2014/forms/design");
				xmlNamespaceResolver.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
				xmlNamespaceResolver.Add("local", "clr-namespace:CarScannerXamarinForms");
				xmlNamespaceResolver.Add("mc", "http://schemas.openxmlformats.org/markup-compatibility/2006");
				xmlNamespaceResolver.Add("settings", "clr-namespace:CarScannerXamarinForms.Settings");
				xmlNamespaceResolver.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
				xamlServiceProvider.Add(typeFromHandle2, new XamlTypeResolver(xmlNamespaceResolver, typeof(CodingModeSelection.<InitializeComponent>_anonXamlCDataTemplate_99).GetTypeInfo().Assembly));
				xamlServiceProvider.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(43, 33)));
				DynamicResource dynamicResource = markupExtension.ProvideValue(xamlServiceProvider);
				label.SetDynamicResource(Label.FontSizeProperty, dynamicResource.Key);
				bindingExtension.Path = "Name";
				BindingBase bindingBase = bindingExtension.ProvideValue(null);
				label.SetBinding(Label.TextProperty, bindingBase);
				dynamicResourceExtension2.Key = "ButtonAccentColor";
				IMarkupExtension<DynamicResource> markupExtension2 = dynamicResourceExtension2;
				XamlServiceProvider xamlServiceProvider2 = new XamlServiceProvider();
				Type typeFromHandle3 = typeof(IProvideValueTarget);
				int num2;
				object[] array3 = new object[(num2 = this.parentValues.Length) + 2];
				Array.Copy(this.parentValues, 0, array3, 2, num2);
				object[] array4 = array3;
				array4[0] = label;
				array4[1] = viewCell;
				object obj2;
				xamlServiceProvider2.Add(typeFromHandle3, obj2 = new SimpleValueTargetProvider(array4, Label.TextColorProperty, nameScope));
				xamlServiceProvider2.Add(typeof(IReferenceProvider), obj2);
				Type typeFromHandle4 = typeof(IXamlTypeResolver);
				XmlNamespaceResolver xmlNamespaceResolver2 = new XmlNamespaceResolver();
				xmlNamespaceResolver2.Add("", "http://xamarin.com/schemas/2014/forms");
				xmlNamespaceResolver2.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
				xmlNamespaceResolver2.Add("d", "http://xamarin.com/schemas/2014/forms/design");
				xmlNamespaceResolver2.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
				xmlNamespaceResolver2.Add("local", "clr-namespace:CarScannerXamarinForms");
				xmlNamespaceResolver2.Add("mc", "http://schemas.openxmlformats.org/markup-compatibility/2006");
				xmlNamespaceResolver2.Add("settings", "clr-namespace:CarScannerXamarinForms.Settings");
				xmlNamespaceResolver2.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
				xamlServiceProvider2.Add(typeFromHandle4, new XamlTypeResolver(xmlNamespaceResolver2, typeof(CodingModeSelection.<InitializeComponent>_anonXamlCDataTemplate_99).GetTypeInfo().Assembly));
				xamlServiceProvider2.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(45, 33)));
				DynamicResource dynamicResource2 = markupExtension2.ProvideValue(xamlServiceProvider2);
				label.SetDynamicResource(Label.TextColorProperty, dynamicResource2.Key);
				viewCell.View = label;
				return viewCell;
			}

			// Token: 0x04002E65 RID: 11877
			internal object[] parentValues;

			// Token: 0x04002E66 RID: 11878
			internal CodingModeSelection root;
		}
	}
}
