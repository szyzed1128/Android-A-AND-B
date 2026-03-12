using System;
using System.CodeDom.Compiler;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using AiForms.Renderers;
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

namespace CarScannerXamarinForms.Coding.PagesV2
{
	// Token: 0x0200098D RID: 2445
	[XamlCompilation(2)]
	[XamlFilePath("Coding\\PagesV2\\CodingModeSelectionV2.xaml")]
	public class CodingModeSelectionV2 : ContentPage
	{
		// Token: 0x06005063 RID: 20579 RVA: 0x003DD600 File Offset: 0x003DB800
		public CodingModeSelectionV2()
		{
			try
			{
				this.InitializeComponent();
				try
				{
					if ((SharedSettings.Current.SelectedBrand == "Lada" || SharedSettings.Current.SelectedBrand == "Лада" || SharedSettings.Current.SelectedBrand == "VAZ" || SharedSettings.Current.SelectedBrand == "ВАЗ") && (Thread.CurrentThread.CurrentCulture.TwoLetterISOLanguageName.ToUpperInvariant() == "RU" || Thread.CurrentThread.CurrentUICulture.TwoLetterISOLanguageName.ToUpperInvariant() == "RU"))
					{
						this.lbNotSupportedYet.Text = "Кодирование не поддерживается Вашим автомобилем, либо Ваш адаптер не поддерживает кодирование.\nПоддерживаемые автомобили ВАЗ/Лада: Vesta и X-Ray (в части блоков Renault). Все остальные модели ВАЗ/Лада не поддерживаются для кодирования.\nДля кодирования на Vesta/X-Ray нужен адаптер с реальной поддержкой всех команд ELM327 версии 1.3a или выше\nPS. Пожалуйста, не надо присылать мне скриншоты из ELM327 Identifier. Это совершенно бесполезное приложение, которое производители плохих адаптеров давно научились обходить. А если вы мне не верите - занимайтесь кодированием через ELM Identifier, флаг вам в руки :)";
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

		// Token: 0x06005064 RID: 20580 RVA: 0x003DD6E8 File Offset: 0x003DB8E8
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

		// Token: 0x06005065 RID: 20581 RVA: 0x003DD6FC File Offset: 0x003DB8FC
		private async void CodingModeSelection_Appearing(object sender, EventArgs e)
		{
			CarPlayManager instance = CarPlayManager.Instance;
			if (instance != null)
			{
				instance.DisplayNonDismissableAlert(Translate.GetString("carPlay_NotAvailableInCoding"));
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

		// Token: 0x06005066 RID: 20582 RVA: 0x003DD734 File Offset: 0x003DB934
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
			this.lv.BindingContext = this.codingModel;
			this.activityFrame.IsVisible = false;
			base.IsEnabled = true;
		}

		// Token: 0x06005067 RID: 20583 RVA: 0x003DD778 File Offset: 0x003DB978
		private async void cellItem_Tapped(object sender, EventArgs e)
		{
			BindableObject bindableObject = (Element)sender;
			base.IsEnabled = false;
			CodingGroupCollection codingGroupCollection = (CodingGroupCollection)bindableObject.BindingContext;
			this.codingModel.SetGroup(codingGroupCollection.Group);
			CodingListPageV2 codingListPageV = new CodingListPageV2(this.codingModel, codingGroupCollection.Name);
			await base.Navigation.PushAsync(codingListPageV);
			base.IsEnabled = true;
		}

		// Token: 0x06005068 RID: 20584 RVA: 0x003DD7B8 File Offset: 0x003DB9B8
		private async void btnBackup_Clicked(object sender, EventArgs e)
		{
			this.btnBackup.IsEnabled = false;
			CodingBackupPage codingBackupPage = new CodingBackupPage();
			await base.Navigation.PushAsync(codingBackupPage);
			this.btnBackup.IsEnabled = true;
		}

		// Token: 0x06005069 RID: 20585 RVA: 0x003DD7F0 File Offset: 0x003DB9F0
		private async void platformSelector_Tapped(object sender, EventArgs e)
		{
			base.IsEnabled = false;
			await this.SelectPlatform();
			base.IsEnabled = true;
		}

		// Token: 0x0600506A RID: 20586 RVA: 0x003DD828 File Offset: 0x003DBA28
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private void InitializeComponent()
		{
			if (ResourceLoader.IsEnabled && ResourceLoader.CanProvideContentFor(new ResourceLoader.ResourceLoadingQuery
			{
				AssemblyName = typeof(CodingModeSelectionV2).GetTypeInfo().Assembly.GetName(),
				ResourcePath = "Coding/PagesV2/CodingModeSelectionV2.xaml",
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
			VisualDiagnostics.RegisterSourceInfo(translate = new Translate(), new Uri("Coding\\PagesV2\\CodingModeSelectionV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 12, 5);
			DynamicResourceExtension dynamicResourceExtension;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension = new DynamicResourceExtension(), new Uri("Coding\\PagesV2\\CodingModeSelectionV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 15, 5);
			RowDefinition rowDefinition;
			VisualDiagnostics.RegisterSourceInfo(rowDefinition = new RowDefinition(), new Uri("Coding\\PagesV2\\CodingModeSelectionV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 19, 18);
			RowDefinition rowDefinition2;
			VisualDiagnostics.RegisterSourceInfo(rowDefinition2 = new RowDefinition(), new Uri("Coding\\PagesV2\\CodingModeSelectionV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 20, 18);
			RowDefinition rowDefinition3;
			VisualDiagnostics.RegisterSourceInfo(rowDefinition3 = new RowDefinition(), new Uri("Coding\\PagesV2\\CodingModeSelectionV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 21, 18);
			BindingExtension bindingExtension;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension = new BindingExtension(), new Uri("Coding\\PagesV2\\CodingModeSelectionV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 27, 17);
			BindingExtension bindingExtension2;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension2 = new BindingExtension(), new Uri("Coding\\PagesV2\\CodingModeSelectionV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 29, 17);
			LabelWithBorder labelWithBorder;
			VisualDiagnostics.RegisterSourceInfo(labelWithBorder = new LabelWithBorder(), new Uri("Coding\\PagesV2\\CodingModeSelectionV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 24, 14);
			Translate translate2;
			VisualDiagnostics.RegisterSourceInfo(translate2 = new Translate(), new Uri("Coding\\PagesV2\\CodingModeSelectionV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 35, 21);
			BindingExtension bindingExtension3;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension3 = new BindingExtension(), new Uri("Coding\\PagesV2\\CodingModeSelectionV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 37, 21);
			StaticResourceExtension staticResourceExtension;
			VisualDiagnostics.RegisterSourceInfo(staticResourceExtension = new StaticResourceExtension(), new Uri("Coding\\PagesV2\\CodingModeSelectionV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 42, 33);
			Translate translate3;
			VisualDiagnostics.RegisterSourceInfo(translate3 = new Translate(), new Uri("Coding\\PagesV2\\CodingModeSelectionV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 43, 33);
			DynamicResourceExtension dynamicResourceExtension2;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension2 = new DynamicResourceExtension(), new Uri("Coding\\PagesV2\\CodingModeSelectionV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 44, 33);
			Label label;
			VisualDiagnostics.RegisterSourceInfo(label = new Label(), new Uri("Coding\\PagesV2\\CodingModeSelectionV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 40, 30);
			StaticResourceExtension staticResourceExtension2;
			VisualDiagnostics.RegisterSourceInfo(staticResourceExtension2 = new StaticResourceExtension(), new Uri("Coding\\PagesV2\\CodingModeSelectionV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 47, 33);
			Translate translate4;
			VisualDiagnostics.RegisterSourceInfo(translate4 = new Translate(), new Uri("Coding\\PagesV2\\CodingModeSelectionV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 48, 33);
			DynamicResourceExtension dynamicResourceExtension3;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension3 = new DynamicResourceExtension(), new Uri("Coding\\PagesV2\\CodingModeSelectionV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 49, 33);
			Label label2;
			VisualDiagnostics.RegisterSourceInfo(label2 = new Label(), new Uri("Coding\\PagesV2\\CodingModeSelectionV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 45, 30);
			StaticResourceExtension staticResourceExtension3;
			VisualDiagnostics.RegisterSourceInfo(staticResourceExtension3 = new StaticResourceExtension(), new Uri("Coding\\PagesV2\\CodingModeSelectionV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 52, 33);
			BindingExtension bindingExtension4;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension4 = new BindingExtension(), new Uri("Coding\\PagesV2\\CodingModeSelectionV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 53, 33);
			DynamicResourceExtension dynamicResourceExtension4;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension4 = new DynamicResourceExtension(), new Uri("Coding\\PagesV2\\CodingModeSelectionV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 54, 33);
			Label label3;
			VisualDiagnostics.RegisterSourceInfo(label3 = new Label(), new Uri("Coding\\PagesV2\\CodingModeSelectionV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 50, 30);
			StackLayout stackLayout;
			VisualDiagnostics.RegisterSourceInfo(stackLayout = new StackLayout(), new Uri("Coding\\PagesV2\\CodingModeSelectionV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 39, 26);
			DataTemplate dataTemplate;
			VisualDiagnostics.RegisterSourceInfo(dataTemplate = new DataTemplate(), new Uri("Coding\\PagesV2\\CodingModeSelectionV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 59, 26);
			Section section;
			VisualDiagnostics.RegisterSourceInfo(section = new Section(), new Uri("Coding\\PagesV2\\CodingModeSelectionV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 34, 18);
			SettingsView settingsView;
			VisualDiagnostics.RegisterSourceInfo(settingsView = new SettingsView(), new Uri("Coding\\PagesV2\\CodingModeSelectionV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 30, 14);
			Translate translate5;
			VisualDiagnostics.RegisterSourceInfo(translate5 = new Translate(), new Uri("Coding\\PagesV2\\CodingModeSelectionV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 113, 17);
			Label label4;
			VisualDiagnostics.RegisterSourceInfo(label4 = new Label(), new Uri("Coding\\PagesV2\\CodingModeSelectionV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 110, 14);
			ActivityFrame activityFrame;
			VisualDiagnostics.RegisterSourceInfo(activityFrame = new ActivityFrame(), new Uri("Coding\\PagesV2\\CodingModeSelectionV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 114, 14);
			Translate translate6;
			VisualDiagnostics.RegisterSourceInfo(translate6 = new Translate(), new Uri("Coding\\PagesV2\\CodingModeSelectionV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 124, 17);
			Button button;
			VisualDiagnostics.RegisterSourceInfo(button = new Button(), new Uri("Coding\\PagesV2\\CodingModeSelectionV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 118, 14);
			Grid grid;
			VisualDiagnostics.RegisterSourceInfo(grid = new Grid(), new Uri("Coding\\PagesV2\\CodingModeSelectionV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 17, 10);
			VisualDiagnostics.RegisterSourceInfo(this, new Uri("Coding\\PagesV2\\CodingModeSelectionV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 2, 2);
			NameScope nameScope = NameScope.GetNameScope(this) ?? new NameScope();
			NameScope.SetNameScope(this, nameScope);
			nameScope.RegisterName("pickerPlatform", labelWithBorder);
			if (labelWithBorder.StyleId == null)
			{
				labelWithBorder.StyleId = "pickerPlatform";
			}
			nameScope.RegisterName("lv", settingsView);
			if (settingsView.StyleId == null)
			{
				settingsView.StyleId = "lv";
			}
			nameScope.RegisterName("lbNotSupportedYet", label4);
			if (label4.StyleId == null)
			{
				label4.StyleId = "lbNotSupportedYet";
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
			this.lv = settingsView;
			this.lbNotSupportedYet = label4;
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
			xmlNamespaceResolver.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xmlNamespaceResolver.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider.Add(typeFromHandle2, new XamlTypeResolver(xmlNamespaceResolver, typeof(CodingModeSelectionV2).GetTypeInfo().Assembly));
			xamlServiceProvider.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(12, 5)));
			object obj2 = markupExtension.ProvideValue(xamlServiceProvider);
			this.Title = obj2;
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
			xmlNamespaceResolver2.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver2.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver2.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver2.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver2.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xmlNamespaceResolver2.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider2.Add(typeFromHandle4, new XamlTypeResolver(xmlNamespaceResolver2, typeof(CodingModeSelectionV2).GetTypeInfo().Assembly));
			xamlServiceProvider2.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(15, 5)));
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
			settingsView.SetValue(Grid.RowProperty, 1);
			settingsView.SetValue(TableView.HasUnevenRowsProperty, true);
			translate2.Text = "coding_Category";
			IMarkupExtension markupExtension3 = translate2;
			XamlServiceProvider xamlServiceProvider3 = new XamlServiceProvider();
			Type typeFromHandle5 = typeof(IProvideValueTarget);
			object[] array3 = new object[0 + 4];
			array3[0] = section;
			array3[1] = settingsView;
			array3[2] = grid;
			array3[3] = this;
			object obj4;
			xamlServiceProvider3.Add(typeFromHandle5, obj4 = new SimpleValueTargetProvider(array3, SectionBase.TitleProperty, nameScope));
			xamlServiceProvider3.Add(typeof(IReferenceProvider), obj4);
			Type typeFromHandle6 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver3 = new XmlNamespaceResolver();
			xmlNamespaceResolver3.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver3.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver3.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver3.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver3.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver3.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver3.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xmlNamespaceResolver3.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider3.Add(typeFromHandle6, new XamlTypeResolver(xmlNamespaceResolver3, typeof(CodingModeSelectionV2).GetTypeInfo().Assembly));
			xamlServiceProvider3.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(35, 21)));
			object obj5 = markupExtension3.ProvideValue(xamlServiceProvider3);
			section.Title = obj5;
			section.SetValue(Section.FooterVisibleProperty, true);
			bindingExtension3.Mode = 2;
			bindingExtension3.Path = "Groups";
			BindingBase bindingBase3 = bindingExtension3.ProvideValue(null);
			section.SetBinding(Section.ItemsSourceProperty, bindingBase3);
			stackLayout.SetValue(View.MarginProperty, new Thickness(15.0, 0.0, 0.0, 0.0));
			stackLayout.SetValue(StackLayout.OrientationProperty, 0);
			label.SetValue(Label.FontAttributesProperty, new FontAttributesConverter().ConvertFromInvariantString("Italic"));
			staticResourceExtension.Key = "BaseFontSize+";
			IMarkupExtension markupExtension4 = staticResourceExtension;
			XamlServiceProvider xamlServiceProvider4 = new XamlServiceProvider();
			Type typeFromHandle7 = typeof(IProvideValueTarget);
			object[] array4 = new object[0 + 6];
			array4[0] = label;
			array4[1] = stackLayout;
			array4[2] = section;
			array4[3] = settingsView;
			array4[4] = grid;
			array4[5] = this;
			object obj6;
			xamlServiceProvider4.Add(typeFromHandle7, obj6 = new SimpleValueTargetProvider(array4, Label.FontSizeProperty, nameScope));
			xamlServiceProvider4.Add(typeof(IReferenceProvider), obj6);
			Type typeFromHandle8 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver4 = new XmlNamespaceResolver();
			xmlNamespaceResolver4.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver4.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver4.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver4.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver4.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver4.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver4.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xmlNamespaceResolver4.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider4.Add(typeFromHandle8, new XamlTypeResolver(xmlNamespaceResolver4, typeof(CodingModeSelectionV2).GetTypeInfo().Assembly));
			xamlServiceProvider4.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(42, 33)));
			object obj7 = markupExtension4.ProvideValue(xamlServiceProvider4);
			label.FontSize = (double)obj7;
			translate3.Text = "DtcPage_CleanCodes_Title";
			IMarkupExtension markupExtension5 = translate3;
			XamlServiceProvider xamlServiceProvider5 = new XamlServiceProvider();
			Type typeFromHandle9 = typeof(IProvideValueTarget);
			object[] array5 = new object[0 + 6];
			array5[0] = label;
			array5[1] = stackLayout;
			array5[2] = section;
			array5[3] = settingsView;
			array5[4] = grid;
			array5[5] = this;
			object obj8;
			xamlServiceProvider5.Add(typeFromHandle9, obj8 = new SimpleValueTargetProvider(array5, Label.TextProperty, nameScope));
			xamlServiceProvider5.Add(typeof(IReferenceProvider), obj8);
			Type typeFromHandle10 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver5 = new XmlNamespaceResolver();
			xmlNamespaceResolver5.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver5.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver5.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver5.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver5.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver5.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver5.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xmlNamespaceResolver5.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider5.Add(typeFromHandle10, new XamlTypeResolver(xmlNamespaceResolver5, typeof(CodingModeSelectionV2).GetTypeInfo().Assembly));
			xamlServiceProvider5.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(43, 33)));
			object obj9 = markupExtension5.ProvideValue(xamlServiceProvider5);
			label.Text = obj9;
			dynamicResourceExtension2.Key = "SettingsFooterTextColor";
			IMarkupExtension<DynamicResource> markupExtension6 = dynamicResourceExtension2;
			XamlServiceProvider xamlServiceProvider6 = new XamlServiceProvider();
			Type typeFromHandle11 = typeof(IProvideValueTarget);
			object[] array6 = new object[0 + 6];
			array6[0] = label;
			array6[1] = stackLayout;
			array6[2] = section;
			array6[3] = settingsView;
			array6[4] = grid;
			array6[5] = this;
			object obj10;
			xamlServiceProvider6.Add(typeFromHandle11, obj10 = new SimpleValueTargetProvider(array6, Label.TextColorProperty, nameScope));
			xamlServiceProvider6.Add(typeof(IReferenceProvider), obj10);
			Type typeFromHandle12 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver6 = new XmlNamespaceResolver();
			xmlNamespaceResolver6.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver6.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver6.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver6.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver6.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver6.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver6.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xmlNamespaceResolver6.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider6.Add(typeFromHandle12, new XamlTypeResolver(xmlNamespaceResolver6, typeof(CodingModeSelectionV2).GetTypeInfo().Assembly));
			xamlServiceProvider6.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(44, 33)));
			DynamicResource dynamicResource2 = markupExtension6.ProvideValue(xamlServiceProvider6);
			label.SetDynamicResource(Label.TextColorProperty, dynamicResource2.Key);
			stackLayout.Children.Add(label);
			label2.SetValue(Label.FontAttributesProperty, new FontAttributesConverter().ConvertFromInvariantString("Italic"));
			staticResourceExtension2.Key = "BaseFontSize+";
			IMarkupExtension markupExtension7 = staticResourceExtension2;
			XamlServiceProvider xamlServiceProvider7 = new XamlServiceProvider();
			Type typeFromHandle13 = typeof(IProvideValueTarget);
			object[] array7 = new object[0 + 6];
			array7[0] = label2;
			array7[1] = stackLayout;
			array7[2] = section;
			array7[3] = settingsView;
			array7[4] = grid;
			array7[5] = this;
			object obj11;
			xamlServiceProvider7.Add(typeFromHandle13, obj11 = new SimpleValueTargetProvider(array7, Label.FontSizeProperty, nameScope));
			xamlServiceProvider7.Add(typeof(IReferenceProvider), obj11);
			Type typeFromHandle14 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver7 = new XmlNamespaceResolver();
			xmlNamespaceResolver7.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver7.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver7.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver7.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver7.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver7.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver7.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xmlNamespaceResolver7.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider7.Add(typeFromHandle14, new XamlTypeResolver(xmlNamespaceResolver7, typeof(CodingModeSelectionV2).GetTypeInfo().Assembly));
			xamlServiceProvider7.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(47, 33)));
			object obj12 = markupExtension7.ProvideValue(xamlServiceProvider7);
			label2.FontSize = (double)obj12;
			translate4.Text = "coding_WarningText";
			IMarkupExtension markupExtension8 = translate4;
			XamlServiceProvider xamlServiceProvider8 = new XamlServiceProvider();
			Type typeFromHandle15 = typeof(IProvideValueTarget);
			object[] array8 = new object[0 + 6];
			array8[0] = label2;
			array8[1] = stackLayout;
			array8[2] = section;
			array8[3] = settingsView;
			array8[4] = grid;
			array8[5] = this;
			object obj13;
			xamlServiceProvider8.Add(typeFromHandle15, obj13 = new SimpleValueTargetProvider(array8, Label.TextProperty, nameScope));
			xamlServiceProvider8.Add(typeof(IReferenceProvider), obj13);
			Type typeFromHandle16 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver8 = new XmlNamespaceResolver();
			xmlNamespaceResolver8.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver8.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver8.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver8.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver8.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver8.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver8.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xmlNamespaceResolver8.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider8.Add(typeFromHandle16, new XamlTypeResolver(xmlNamespaceResolver8, typeof(CodingModeSelectionV2).GetTypeInfo().Assembly));
			xamlServiceProvider8.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(48, 33)));
			object obj14 = markupExtension8.ProvideValue(xamlServiceProvider8);
			label2.Text = obj14;
			dynamicResourceExtension3.Key = "SettingsFooterTextColor";
			IMarkupExtension<DynamicResource> markupExtension9 = dynamicResourceExtension3;
			XamlServiceProvider xamlServiceProvider9 = new XamlServiceProvider();
			Type typeFromHandle17 = typeof(IProvideValueTarget);
			object[] array9 = new object[0 + 6];
			array9[0] = label2;
			array9[1] = stackLayout;
			array9[2] = section;
			array9[3] = settingsView;
			array9[4] = grid;
			array9[5] = this;
			object obj15;
			xamlServiceProvider9.Add(typeFromHandle17, obj15 = new SimpleValueTargetProvider(array9, Label.TextColorProperty, nameScope));
			xamlServiceProvider9.Add(typeof(IReferenceProvider), obj15);
			Type typeFromHandle18 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver9 = new XmlNamespaceResolver();
			xmlNamespaceResolver9.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver9.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver9.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver9.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver9.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver9.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver9.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xmlNamespaceResolver9.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider9.Add(typeFromHandle18, new XamlTypeResolver(xmlNamespaceResolver9, typeof(CodingModeSelectionV2).GetTypeInfo().Assembly));
			xamlServiceProvider9.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(49, 33)));
			DynamicResource dynamicResource3 = markupExtension9.ProvideValue(xamlServiceProvider9);
			label2.SetDynamicResource(Label.TextColorProperty, dynamicResource3.Key);
			stackLayout.Children.Add(label2);
			label3.SetValue(Label.FontAttributesProperty, new FontAttributesConverter().ConvertFromInvariantString("Italic"));
			staticResourceExtension3.Key = "BaseFontSize+";
			IMarkupExtension markupExtension10 = staticResourceExtension3;
			XamlServiceProvider xamlServiceProvider10 = new XamlServiceProvider();
			Type typeFromHandle19 = typeof(IProvideValueTarget);
			object[] array10 = new object[0 + 6];
			array10[0] = label3;
			array10[1] = stackLayout;
			array10[2] = section;
			array10[3] = settingsView;
			array10[4] = grid;
			array10[5] = this;
			object obj16;
			xamlServiceProvider10.Add(typeFromHandle19, obj16 = new SimpleValueTargetProvider(array10, Label.FontSizeProperty, nameScope));
			xamlServiceProvider10.Add(typeof(IReferenceProvider), obj16);
			Type typeFromHandle20 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver10 = new XmlNamespaceResolver();
			xmlNamespaceResolver10.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver10.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver10.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver10.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver10.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver10.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver10.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xmlNamespaceResolver10.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider10.Add(typeFromHandle20, new XamlTypeResolver(xmlNamespaceResolver10, typeof(CodingModeSelectionV2).GetTypeInfo().Assembly));
			xamlServiceProvider10.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(52, 33)));
			object obj17 = markupExtension10.ProvideValue(xamlServiceProvider10);
			label3.FontSize = (double)obj17;
			bindingExtension4.Path = "WarningSpecific";
			BindingBase bindingBase4 = bindingExtension4.ProvideValue(null);
			label3.SetBinding(Label.TextProperty, bindingBase4);
			dynamicResourceExtension4.Key = "SettingsFooterTextColor";
			IMarkupExtension<DynamicResource> markupExtension11 = dynamicResourceExtension4;
			XamlServiceProvider xamlServiceProvider11 = new XamlServiceProvider();
			Type typeFromHandle21 = typeof(IProvideValueTarget);
			object[] array11 = new object[0 + 6];
			array11[0] = label3;
			array11[1] = stackLayout;
			array11[2] = section;
			array11[3] = settingsView;
			array11[4] = grid;
			array11[5] = this;
			object obj18;
			xamlServiceProvider11.Add(typeFromHandle21, obj18 = new SimpleValueTargetProvider(array11, Label.TextColorProperty, nameScope));
			xamlServiceProvider11.Add(typeof(IReferenceProvider), obj18);
			Type typeFromHandle22 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver11 = new XmlNamespaceResolver();
			xmlNamespaceResolver11.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver11.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver11.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver11.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver11.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver11.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver11.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xmlNamespaceResolver11.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider11.Add(typeFromHandle22, new XamlTypeResolver(xmlNamespaceResolver11, typeof(CodingModeSelectionV2).GetTypeInfo().Assembly));
			xamlServiceProvider11.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(54, 33)));
			DynamicResource dynamicResource4 = markupExtension11.ProvideValue(xamlServiceProvider11);
			label3.SetDynamicResource(Label.TextColorProperty, dynamicResource4.Key);
			stackLayout.Children.Add(label3);
			section.SetValue(Section.FooterViewProperty, stackLayout);
			IDataTemplate dataTemplate2 = dataTemplate;
			CodingModeSelectionV2.<InitializeComponent>_anonXamlCDataTemplate_3 <InitializeComponent>_anonXamlCDataTemplate_ = new CodingModeSelectionV2.<InitializeComponent>_anonXamlCDataTemplate_3();
			object[] array12 = new object[0 + 5];
			array12[0] = dataTemplate;
			array12[1] = section;
			array12[2] = settingsView;
			array12[3] = grid;
			array12[4] = this;
			<InitializeComponent>_anonXamlCDataTemplate_.parentValues = array12;
			<InitializeComponent>_anonXamlCDataTemplate_.root = this;
			dataTemplate2.LoadTemplate = new Func<object>(<InitializeComponent>_anonXamlCDataTemplate_.LoadDataTemplate);
			section.SetValue(Section.ItemTemplateProperty, dataTemplate);
			settingsView.Root.Add(section);
			grid.Children.Add(settingsView);
			label4.SetValue(VisualElement.IsVisibleProperty, new VisualElement.VisibilityConverter().ConvertFromInvariantString("false"));
			translate5.Text = "coding_CarNotSupported";
			IMarkupExtension markupExtension12 = translate5;
			XamlServiceProvider xamlServiceProvider12 = new XamlServiceProvider();
			Type typeFromHandle23 = typeof(IProvideValueTarget);
			object[] array13 = new object[0 + 3];
			array13[0] = label4;
			array13[1] = grid;
			array13[2] = this;
			object obj19;
			xamlServiceProvider12.Add(typeFromHandle23, obj19 = new SimpleValueTargetProvider(array13, Label.TextProperty, nameScope));
			xamlServiceProvider12.Add(typeof(IReferenceProvider), obj19);
			Type typeFromHandle24 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver12 = new XmlNamespaceResolver();
			xmlNamespaceResolver12.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver12.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver12.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver12.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver12.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver12.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver12.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xmlNamespaceResolver12.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider12.Add(typeFromHandle24, new XamlTypeResolver(xmlNamespaceResolver12, typeof(CodingModeSelectionV2).GetTypeInfo().Assembly));
			xamlServiceProvider12.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(113, 17)));
			object obj20 = markupExtension12.ProvideValue(xamlServiceProvider12);
			label4.Text = obj20;
			grid.Children.Add(label4);
			activityFrame.SetValue(Grid.RowProperty, 1);
			activityFrame.SetValue(VisualElement.IsVisibleProperty, new VisualElement.VisibilityConverter().ConvertFromInvariantString("False"));
			grid.Children.Add(activityFrame);
			button.SetValue(Grid.RowProperty, 2);
			button.SetValue(VisualElement.BackgroundColorProperty, Color.DarkBlue);
			button.Clicked += this.btnBackup_Clicked;
			button.SetValue(Button.CornerRadiusProperty, 9);
			translate6.Text = "coding_History";
			IMarkupExtension markupExtension13 = translate6;
			XamlServiceProvider xamlServiceProvider13 = new XamlServiceProvider();
			Type typeFromHandle25 = typeof(IProvideValueTarget);
			object[] array14 = new object[0 + 3];
			array14[0] = button;
			array14[1] = grid;
			array14[2] = this;
			object obj21;
			xamlServiceProvider13.Add(typeFromHandle25, obj21 = new SimpleValueTargetProvider(array14, Button.TextProperty, nameScope));
			xamlServiceProvider13.Add(typeof(IReferenceProvider), obj21);
			Type typeFromHandle26 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver13 = new XmlNamespaceResolver();
			xmlNamespaceResolver13.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver13.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver13.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver13.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver13.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver13.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver13.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xmlNamespaceResolver13.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider13.Add(typeFromHandle26, new XamlTypeResolver(xmlNamespaceResolver13, typeof(CodingModeSelectionV2).GetTypeInfo().Assembly));
			xamlServiceProvider13.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(124, 17)));
			object obj22 = markupExtension13.ProvideValue(xamlServiceProvider13);
			button.Text = obj22;
			button.SetValue(Button.TextColorProperty, Color.White);
			grid.Children.Add(button);
			this.SetValue(ContentPage.ContentProperty, grid);
		}

		// Token: 0x0600506B RID: 20587 RVA: 0x003DF3A0 File Offset: 0x003DD5A0
		[CompilerGenerated]
		private void <BuildProgress>b__2_0(string str)
		{
			MainThread.InvokeOnMainThreadAsync(delegate
			{
				this.activityFrame.Text = ActivityFrame.PLEASE_WAIT_TEXT + "\n" + str;
			});
		}

		// Token: 0x0600506C RID: 20588 RVA: 0x003DF3C8 File Offset: 0x003DD5C8
		private void __InitComponentRuntime()
		{
			Extensions.LoadFromXaml<CodingModeSelectionV2>(this, typeof(CodingModeSelectionV2));
			this.pickerPlatform = NameScopeExtensions.FindByName<LabelWithBorder>(this, "pickerPlatform");
			this.lv = NameScopeExtensions.FindByName<SettingsView>(this, "lv");
			this.lbNotSupportedYet = NameScopeExtensions.FindByName<Label>(this, "lbNotSupportedYet");
			this.activityFrame = NameScopeExtensions.FindByName<ActivityFrame>(this, "activityFrame");
			this.btnBackup = NameScopeExtensions.FindByName<Button>(this, "btnBackup");
		}

		// Token: 0x04003035 RID: 12341
		private bool warningShowed;

		// Token: 0x04003036 RID: 12342
		private CodingListModel codingModel;

		// Token: 0x04003037 RID: 12343
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private LabelWithBorder pickerPlatform;

		// Token: 0x04003038 RID: 12344
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private SettingsView lv;

		// Token: 0x04003039 RID: 12345
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private Label lbNotSupportedYet;

		// Token: 0x0400303A RID: 12346
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private ActivityFrame activityFrame;

		// Token: 0x0400303B RID: 12347
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private Button btnBackup;

		// Token: 0x0200098E RID: 2446
		[CompilerGenerated]
		[Serializable]
		private sealed class <>c
		{
			// Token: 0x0600506D RID: 20589 RVA: 0x003DF43B File Offset: 0x003DD63B
			// Note: this type is marked as 'beforefieldinit'.
			static <>c()
			{
			}

			// Token: 0x0600506E RID: 20590 RVA: 0x00002050 File Offset: 0x00000250
			public <>c()
			{
			}

			// Token: 0x0600506F RID: 20591 RVA: 0x003A1D8F File Offset: 0x0039FF8F
			internal bool <CodingModeSelection_Appearing>b__3_0(ValueItemWithTranslation x)
			{
				return x.Value == SharedSettings.Current.CodingLastPlatformSelected;
			}

			// Token: 0x0400303C RID: 12348
			public static readonly CodingModeSelectionV2.<>c <>9 = new CodingModeSelectionV2.<>c();

			// Token: 0x0400303D RID: 12349
			public static Func<ValueItemWithTranslation, bool> <>9__3_0;
		}

		// Token: 0x0200098F RID: 2447
		[CompilerGenerated]
		private sealed class <>c__DisplayClass2_0
		{
			// Token: 0x06005070 RID: 20592 RVA: 0x00002050 File Offset: 0x00000250
			public <>c__DisplayClass2_0()
			{
			}

			// Token: 0x06005071 RID: 20593 RVA: 0x003DF447 File Offset: 0x003DD647
			internal void <BuildProgress>b__1()
			{
				this.<>4__this.activityFrame.Text = ActivityFrame.PLEASE_WAIT_TEXT + "\n" + this.str;
			}

			// Token: 0x0400303E RID: 12350
			public string str;

			// Token: 0x0400303F RID: 12351
			public CodingModeSelectionV2 <>4__this;
		}

		// Token: 0x02000990 RID: 2448
		[CompilerGenerated]
		private sealed class <>c__DisplayClass5_0
		{
			// Token: 0x06005072 RID: 20594 RVA: 0x00002050 File Offset: 0x00000250
			public <>c__DisplayClass5_0()
			{
			}

			// Token: 0x06005073 RID: 20595 RVA: 0x003DF46E File Offset: 0x003DD66E
			internal void <SelectPlatform>b__0(ValueItemWithTranslation selected_item)
			{
				SharedSettings.Current.CodingLastPlatformSelected = selected_item.Value;
				this.<>4__this.codingModel.CurrentPlatform = selected_item;
				this.semaphore.Release();
			}

			// Token: 0x04003040 RID: 12352
			public CodingModeSelectionV2 <>4__this;

			// Token: 0x04003041 RID: 12353
			public SemaphoreSlim semaphore;
		}

		// Token: 0x02000991 RID: 2449
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <CodingModeSelection_Appearing>d__3 : IAsyncStateMachine
		{
			// Token: 0x06005074 RID: 20596 RVA: 0x003DF4A0 File Offset: 0x003DD6A0
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				CodingModeSelectionV2 codingModeSelectionV = this;
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
						goto IL_0178;
					}
					case 2:
					{
						TaskAwaiter taskAwaiter7;
						taskAwaiter6 = taskAwaiter7;
						taskAwaiter7 = default(TaskAwaiter);
						num2 = -1;
						goto IL_025E;
					}
					case 3:
					{
						TaskAwaiter taskAwaiter7;
						taskAwaiter6 = taskAwaiter7;
						taskAwaiter7 = default(TaskAwaiter);
						num2 = -1;
						goto IL_02C8;
					}
					case 4:
					{
						TaskAwaiter taskAwaiter7;
						taskAwaiter6 = taskAwaiter7;
						taskAwaiter7 = default(TaskAwaiter);
						num2 = -1;
						goto IL_0394;
					}
					case 5:
					{
						TaskAwaiter taskAwaiter7;
						taskAwaiter6 = taskAwaiter7;
						taskAwaiter7 = default(TaskAwaiter);
						num2 = -1;
						goto IL_041A;
					}
					default:
					{
						CarPlayManager instance = CarPlayManager.Instance;
						if (instance != null)
						{
							instance.DisplayNonDismissableAlert(Translate.GetString("carPlay_NotAvailableInCoding"));
						}
						if (codingModeSelectionV.warningShowed)
						{
							goto IL_0457;
						}
						codingModeSelectionV.warningShowed = true;
						codingModeSelectionV.codingModel = new CodingListModel();
						codingModeSelectionV.lv.BindingContext = codingModeSelectionV.codingModel;
						codingModeSelectionV.pickerPlatform.BindingContext = codingModeSelectionV.codingModel;
						taskAwaiter3 = codingModeSelectionV.DisplayAlert(Translate.GetString("DtcPage_CleanCodes_Title"), Translate.GetString("coding_WarningText") + "\n" + codingModeSelectionV.codingModel.WarningSpecific, Translate.GetString("ios_AGREE"), Translate.GetString("btnCancel.Content")).GetAwaiter();
						if (!taskAwaiter3.IsCompleted)
						{
							num2 = 0;
							taskAwaiter2 = taskAwaiter3;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<bool>, CodingModeSelectionV2.<CodingModeSelection_Appearing>d__3>(ref taskAwaiter3, ref this);
							return;
						}
						break;
					}
					}
					if (!taskAwaiter3.GetResult())
					{
						taskAwaiter4 = codingModeSelectionV.Navigation.PopAsync().GetAwaiter();
						if (!taskAwaiter4.IsCompleted)
						{
							num2 = 1;
							TaskAwaiter<Page> taskAwaiter5 = taskAwaiter4;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<Page>, CodingModeSelectionV2.<CodingModeSelection_Appearing>d__3>(ref taskAwaiter4, ref this);
							return;
						}
					}
					else if (codingModeSelectionV.codingModel.IsPlatformSelectorVisible)
					{
						if (string.IsNullOrEmpty(SharedSettings.Current.CodingLastPlatformSelected))
						{
							ValueItemWithTranslation valueItemWithTranslation = codingModeSelectionV.codingModel.Platforms.FirstOrDefault<ValueItemWithTranslation>();
							if (valueItemWithTranslation == null)
							{
								goto IL_0271;
							}
							SharedSettings.Current.CodingLastPlatformSelected = valueItemWithTranslation.Value;
							codingModeSelectionV.codingModel.CurrentPlatform = valueItemWithTranslation;
							codingModeSelectionV.activityFrame.Text = ActivityFrame.PLEASE_WAIT_TEXT;
							codingModeSelectionV.activityFrame.IsVisible = true;
							taskAwaiter6 = codingModeSelectionV.codingModel.LoadEasyCodingCollection(codingModeSelectionV.BuildProgress()).GetAwaiter();
							if (!taskAwaiter6.IsCompleted)
							{
								num2 = 2;
								TaskAwaiter taskAwaiter7 = taskAwaiter6;
								this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, CodingModeSelectionV2.<CodingModeSelection_Appearing>d__3>(ref taskAwaiter6, ref this);
								return;
							}
							goto IL_025E;
						}
						else
						{
							ValueItemWithTranslation valueItemWithTranslation2 = codingModeSelectionV.codingModel.Platforms.FirstOrDefault((ValueItemWithTranslation x) => x.Value == SharedSettings.Current.CodingLastPlatformSelected);
							if (valueItemWithTranslation2 != null)
							{
								codingModeSelectionV.codingModel.CurrentPlatform = valueItemWithTranslation2;
							}
							codingModeSelectionV.activityFrame.Text = ActivityFrame.PLEASE_WAIT_TEXT;
							codingModeSelectionV.activityFrame.IsVisible = true;
							taskAwaiter6 = codingModeSelectionV.codingModel.LoadEasyCodingCollection(codingModeSelectionV.BuildProgress()).GetAwaiter();
							if (!taskAwaiter6.IsCompleted)
							{
								num2 = 4;
								TaskAwaiter taskAwaiter7 = taskAwaiter6;
								this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, CodingModeSelectionV2.<CodingModeSelection_Appearing>d__3>(ref taskAwaiter6, ref this);
								return;
							}
							goto IL_0394;
						}
					}
					else
					{
						codingModeSelectionV.activityFrame.IsVisible = true;
						taskAwaiter6 = codingModeSelectionV.codingModel.LoadEasyCodingCollection(codingModeSelectionV.BuildProgress()).GetAwaiter();
						if (!taskAwaiter6.IsCompleted)
						{
							num2 = 5;
							TaskAwaiter taskAwaiter7 = taskAwaiter6;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, CodingModeSelectionV2.<CodingModeSelection_Appearing>d__3>(ref taskAwaiter6, ref this);
							return;
						}
						goto IL_041A;
					}
					IL_0178:
					taskAwaiter4.GetResult();
					goto IL_0457;
					IL_025E:
					taskAwaiter6.GetResult();
					codingModeSelectionV.activityFrame.IsVisible = false;
					IL_0271:
					taskAwaiter6 = codingModeSelectionV.SelectPlatform().GetAwaiter();
					if (!taskAwaiter6.IsCompleted)
					{
						num2 = 3;
						TaskAwaiter taskAwaiter7 = taskAwaiter6;
						this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, CodingModeSelectionV2.<CodingModeSelection_Appearing>d__3>(ref taskAwaiter6, ref this);
						return;
					}
					IL_02C8:
					taskAwaiter6.GetResult();
					goto IL_0457;
					IL_0394:
					taskAwaiter6.GetResult();
					codingModeSelectionV.activityFrame.IsVisible = false;
					goto IL_0457;
					IL_041A:
					taskAwaiter6.GetResult();
					if (codingModeSelectionV.codingModel.Groups.Count == 0)
					{
						codingModeSelectionV.lbNotSupportedYet.IsVisible = true;
						codingModeSelectionV.lv.IsVisible = false;
					}
					codingModeSelectionV.activityFrame.IsVisible = false;
					IL_0457:;
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

			// Token: 0x06005075 RID: 20597 RVA: 0x003DF950 File Offset: 0x003DDB50
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x04003042 RID: 12354
			public int <>1__state;

			// Token: 0x04003043 RID: 12355
			public AsyncVoidMethodBuilder <>t__builder;

			// Token: 0x04003044 RID: 12356
			public CodingModeSelectionV2 <>4__this;

			// Token: 0x04003045 RID: 12357
			private TaskAwaiter<bool> <>u__1;

			// Token: 0x04003046 RID: 12358
			private TaskAwaiter<Page> <>u__2;

			// Token: 0x04003047 RID: 12359
			private TaskAwaiter <>u__3;
		}

		// Token: 0x02000992 RID: 2450
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <SelectPlatform>d__5 : IAsyncStateMachine
		{
			// Token: 0x06005076 RID: 20598 RVA: 0x003DF960 File Offset: 0x003DDB60
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				CodingModeSelectionV2 codingModeSelectionV = this;
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
						goto IL_01CA;
					}
					default:
					{
						CS$<>8__locals1 = new CodingModeSelectionV2.<>c__DisplayClass5_0();
						CS$<>8__locals1.<>4__this = this;
						CS$<>8__locals1.semaphore = new SemaphoreSlim(0, 1);
						ItemWithValueSelectorPage itemWithValueSelectorPage = new ItemWithValueSelectorPage(Translate.GetString("coding_vag_choose_platform"), codingModeSelectionV.codingModel.Platforms, null, delegate(ValueItemWithTranslation selected_item)
						{
							SharedSettings.Current.CodingLastPlatformSelected = selected_item.Value;
							CS$<>8__locals1.<>4__this.codingModel.CurrentPlatform = selected_item;
							CS$<>8__locals1.semaphore.Release();
						});
						taskAwaiter = codingModeSelectionV.Navigation.PushAsync(itemWithValueSelectorPage).GetAwaiter();
						if (!taskAwaiter.IsCompleted)
						{
							num2 = 0;
							TaskAwaiter taskAwaiter2 = taskAwaiter;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, CodingModeSelectionV2.<SelectPlatform>d__5>(ref taskAwaiter, ref this);
							return;
						}
						break;
					}
					}
					taskAwaiter.GetResult();
					codingModeSelectionV.IsEnabled = true;
					taskAwaiter = CS$<>8__locals1.semaphore.WaitAsync().GetAwaiter();
					if (!taskAwaiter.IsCompleted)
					{
						num2 = 1;
						TaskAwaiter taskAwaiter2 = taskAwaiter;
						this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, CodingModeSelectionV2.<SelectPlatform>d__5>(ref taskAwaiter, ref this);
						return;
					}
					IL_0141:
					taskAwaiter.GetResult();
					codingModeSelectionV.IsEnabled = false;
					codingModeSelectionV.activityFrame.Text = ActivityFrame.PLEASE_WAIT_TEXT;
					codingModeSelectionV.activityFrame.IsVisible = true;
					taskAwaiter = codingModeSelectionV.codingModel.LoadEasyCodingCollection(codingModeSelectionV.BuildProgress()).GetAwaiter();
					if (!taskAwaiter.IsCompleted)
					{
						num2 = 2;
						TaskAwaiter taskAwaiter2 = taskAwaiter;
						this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, CodingModeSelectionV2.<SelectPlatform>d__5>(ref taskAwaiter, ref this);
						return;
					}
					IL_01CA:
					taskAwaiter.GetResult();
					codingModeSelectionV.lv.BindingContext = codingModeSelectionV.codingModel;
					codingModeSelectionV.activityFrame.IsVisible = false;
					codingModeSelectionV.IsEnabled = true;
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

			// Token: 0x06005077 RID: 20599 RVA: 0x003DFBBC File Offset: 0x003DDDBC
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x04003048 RID: 12360
			public int <>1__state;

			// Token: 0x04003049 RID: 12361
			public AsyncTaskMethodBuilder <>t__builder;

			// Token: 0x0400304A RID: 12362
			public CodingModeSelectionV2 <>4__this;

			// Token: 0x0400304B RID: 12363
			private CodingModeSelectionV2.<>c__DisplayClass5_0 <>8__1;

			// Token: 0x0400304C RID: 12364
			private TaskAwaiter <>u__1;
		}

		// Token: 0x02000993 RID: 2451
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <btnBackup_Clicked>d__7 : IAsyncStateMachine
		{
			// Token: 0x06005078 RID: 20600 RVA: 0x003DFBCC File Offset: 0x003DDDCC
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				CodingModeSelectionV2 codingModeSelectionV = this;
				try
				{
					TaskAwaiter taskAwaiter;
					if (num != 0)
					{
						codingModeSelectionV.btnBackup.IsEnabled = false;
						CodingBackupPage codingBackupPage = new CodingBackupPage();
						taskAwaiter = codingModeSelectionV.Navigation.PushAsync(codingBackupPage).GetAwaiter();
						if (!taskAwaiter.IsCompleted)
						{
							num2 = 0;
							TaskAwaiter taskAwaiter2 = taskAwaiter;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, CodingModeSelectionV2.<btnBackup_Clicked>d__7>(ref taskAwaiter, ref this);
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
					codingModeSelectionV.btnBackup.IsEnabled = true;
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

			// Token: 0x06005079 RID: 20601 RVA: 0x003DFCA4 File Offset: 0x003DDEA4
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x0400304D RID: 12365
			public int <>1__state;

			// Token: 0x0400304E RID: 12366
			public AsyncVoidMethodBuilder <>t__builder;

			// Token: 0x0400304F RID: 12367
			public CodingModeSelectionV2 <>4__this;

			// Token: 0x04003050 RID: 12368
			private TaskAwaiter <>u__1;
		}

		// Token: 0x02000994 RID: 2452
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <cellItem_Tapped>d__6 : IAsyncStateMachine
		{
			// Token: 0x0600507A RID: 20602 RVA: 0x003DFCB4 File Offset: 0x003DDEB4
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				CodingModeSelectionV2 codingModeSelectionV = this;
				try
				{
					TaskAwaiter taskAwaiter;
					if (num != 0)
					{
						BindableObject bindableObject = (Element)sender;
						codingModeSelectionV.IsEnabled = false;
						CodingGroupCollection codingGroupCollection = (CodingGroupCollection)bindableObject.BindingContext;
						codingModeSelectionV.codingModel.SetGroup(codingGroupCollection.Group);
						CodingListPageV2 codingListPageV = new CodingListPageV2(codingModeSelectionV.codingModel, codingGroupCollection.Name);
						taskAwaiter = codingModeSelectionV.Navigation.PushAsync(codingListPageV).GetAwaiter();
						if (!taskAwaiter.IsCompleted)
						{
							num2 = 0;
							TaskAwaiter taskAwaiter2 = taskAwaiter;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, CodingModeSelectionV2.<cellItem_Tapped>d__6>(ref taskAwaiter, ref this);
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
					codingModeSelectionV.IsEnabled = true;
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

			// Token: 0x0600507B RID: 20603 RVA: 0x003DFDB8 File Offset: 0x003DDFB8
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x04003051 RID: 12369
			public int <>1__state;

			// Token: 0x04003052 RID: 12370
			public AsyncVoidMethodBuilder <>t__builder;

			// Token: 0x04003053 RID: 12371
			public object sender;

			// Token: 0x04003054 RID: 12372
			public CodingModeSelectionV2 <>4__this;

			// Token: 0x04003055 RID: 12373
			private TaskAwaiter <>u__1;
		}

		// Token: 0x02000995 RID: 2453
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <platformSelector_Tapped>d__8 : IAsyncStateMachine
		{
			// Token: 0x0600507C RID: 20604 RVA: 0x003DFDC8 File Offset: 0x003DDFC8
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				CodingModeSelectionV2 codingModeSelectionV = this;
				try
				{
					TaskAwaiter taskAwaiter;
					if (num != 0)
					{
						codingModeSelectionV.IsEnabled = false;
						taskAwaiter = codingModeSelectionV.SelectPlatform().GetAwaiter();
						if (!taskAwaiter.IsCompleted)
						{
							num2 = 0;
							TaskAwaiter taskAwaiter2 = taskAwaiter;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, CodingModeSelectionV2.<platformSelector_Tapped>d__8>(ref taskAwaiter, ref this);
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
					codingModeSelectionV.IsEnabled = true;
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

			// Token: 0x0600507D RID: 20605 RVA: 0x003DFE88 File Offset: 0x003DE088
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x04003056 RID: 12374
			public int <>1__state;

			// Token: 0x04003057 RID: 12375
			public AsyncVoidMethodBuilder <>t__builder;

			// Token: 0x04003058 RID: 12376
			public CodingModeSelectionV2 <>4__this;

			// Token: 0x04003059 RID: 12377
			private TaskAwaiter <>u__1;
		}

		// Token: 0x02000996 RID: 2454
		[CompilerGenerated]
		private sealed class <InitializeComponent>_anonXamlCDataTemplate_3
		{
			// Token: 0x0600507E RID: 20606 RVA: 0x003DFE98 File Offset: 0x003DE098
			public <InitializeComponent>_anonXamlCDataTemplate_3()
			{
			}

			// Token: 0x0600507F RID: 20607 RVA: 0x003DFEAC File Offset: 0x003DE0AC
			internal object LoadDataTemplate()
			{
				BindingExtension bindingExtension;
				VisualDiagnostics.RegisterSourceInfo(bindingExtension = new BindingExtension(), new Uri("Coding\\PagesV2\\CodingModeSelectionV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 61, 33);
				DynamicResourceExtension dynamicResourceExtension;
				VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension = new DynamicResourceExtension(), new Uri("Coding\\PagesV2\\CodingModeSelectionV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 63, 33);
				StaticResourceExtension staticResourceExtension;
				VisualDiagnostics.RegisterSourceInfo(staticResourceExtension = new StaticResourceExtension(), new Uri("Coding\\PagesV2\\CodingModeSelectionV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 64, 33);
				LabelCell labelCell;
				VisualDiagnostics.RegisterSourceInfo(labelCell = new LabelCell(), new Uri("Coding\\PagesV2\\CodingModeSelectionV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 60, 30);
				NameScope nameScope = new NameScope();
				NameScope.SetNameScope(labelCell, nameScope);
				bindingExtension.Path = "Name";
				BindingBase bindingBase = bindingExtension.ProvideValue(null);
				labelCell.SetBinding(CellBase.TitleProperty, bindingBase);
				labelCell.Tapped += this.root.cellItem_Tapped;
				dynamicResourceExtension.Key = "ButtonAccentColor";
				IMarkupExtension<DynamicResource> markupExtension = dynamicResourceExtension;
				XamlServiceProvider xamlServiceProvider = new XamlServiceProvider();
				Type typeFromHandle = typeof(IProvideValueTarget);
				int num;
				object[] array = new object[(num = this.parentValues.Length) + 1];
				Array.Copy(this.parentValues, 0, array, 1, num);
				object[] array2 = array;
				array2[0] = labelCell;
				object obj;
				xamlServiceProvider.Add(typeFromHandle, obj = new SimpleValueTargetProvider(array2, CellBase.TitleColorProperty, nameScope));
				xamlServiceProvider.Add(typeof(IReferenceProvider), obj);
				Type typeFromHandle2 = typeof(IXamlTypeResolver);
				XmlNamespaceResolver xmlNamespaceResolver = new XmlNamespaceResolver();
				xmlNamespaceResolver.Add("", "http://xamarin.com/schemas/2014/forms");
				xmlNamespaceResolver.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
				xmlNamespaceResolver.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
				xmlNamespaceResolver.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
				xmlNamespaceResolver.Add("local", "clr-namespace:CarScannerXamarinForms");
				xmlNamespaceResolver.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
				xmlNamespaceResolver.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
				xmlNamespaceResolver.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
				xamlServiceProvider.Add(typeFromHandle2, new XamlTypeResolver(xmlNamespaceResolver, typeof(CodingModeSelectionV2.<InitializeComponent>_anonXamlCDataTemplate_3).GetTypeInfo().Assembly));
				xamlServiceProvider.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(63, 33)));
				DynamicResource dynamicResource = markupExtension.ProvideValue(xamlServiceProvider);
				labelCell.SetDynamicResource(CellBase.TitleColorProperty, dynamicResource.Key);
				staticResourceExtension.Key = "BaseFontSize++";
				IMarkupExtension markupExtension2 = staticResourceExtension;
				XamlServiceProvider xamlServiceProvider2 = new XamlServiceProvider();
				Type typeFromHandle3 = typeof(IProvideValueTarget);
				int num2;
				object[] array3 = new object[(num2 = this.parentValues.Length) + 1];
				Array.Copy(this.parentValues, 0, array3, 1, num2);
				object[] array4 = array3;
				array4[0] = labelCell;
				object obj2;
				xamlServiceProvider2.Add(typeFromHandle3, obj2 = new SimpleValueTargetProvider(array4, CellBase.TitleFontSizeProperty, nameScope));
				xamlServiceProvider2.Add(typeof(IReferenceProvider), obj2);
				Type typeFromHandle4 = typeof(IXamlTypeResolver);
				XmlNamespaceResolver xmlNamespaceResolver2 = new XmlNamespaceResolver();
				xmlNamespaceResolver2.Add("", "http://xamarin.com/schemas/2014/forms");
				xmlNamespaceResolver2.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
				xmlNamespaceResolver2.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
				xmlNamespaceResolver2.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
				xmlNamespaceResolver2.Add("local", "clr-namespace:CarScannerXamarinForms");
				xmlNamespaceResolver2.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
				xmlNamespaceResolver2.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
				xmlNamespaceResolver2.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
				xamlServiceProvider2.Add(typeFromHandle4, new XamlTypeResolver(xmlNamespaceResolver2, typeof(CodingModeSelectionV2.<InitializeComponent>_anonXamlCDataTemplate_3).GetTypeInfo().Assembly));
				xamlServiceProvider2.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(64, 33)));
				object obj3 = markupExtension2.ProvideValue(xamlServiceProvider2);
				labelCell.TitleFontSize = (double)obj3;
				labelCell.SetValue(LabelCell.ValueTextProperty, ">");
				BindableObject bindableObject = labelCell;
				BindableProperty valueTextFontSizeProperty = LabelCell.ValueTextFontSizeProperty;
				IExtendedTypeConverter extendedTypeConverter = new FontSizeConverter();
				string text = "28";
				XamlServiceProvider xamlServiceProvider3 = new XamlServiceProvider();
				Type typeFromHandle5 = typeof(IProvideValueTarget);
				int num3;
				object[] array5 = new object[(num3 = this.parentValues.Length) + 1];
				Array.Copy(this.parentValues, 0, array5, 1, num3);
				object[] array6 = array5;
				array6[0] = labelCell;
				object obj4;
				xamlServiceProvider3.Add(typeFromHandle5, obj4 = new SimpleValueTargetProvider(array6, LabelCell.ValueTextFontSizeProperty, nameScope));
				xamlServiceProvider3.Add(typeof(IReferenceProvider), obj4);
				Type typeFromHandle6 = typeof(IXamlTypeResolver);
				XmlNamespaceResolver xmlNamespaceResolver3 = new XmlNamespaceResolver();
				xmlNamespaceResolver3.Add("", "http://xamarin.com/schemas/2014/forms");
				xmlNamespaceResolver3.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
				xmlNamespaceResolver3.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
				xmlNamespaceResolver3.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
				xmlNamespaceResolver3.Add("local", "clr-namespace:CarScannerXamarinForms");
				xmlNamespaceResolver3.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
				xmlNamespaceResolver3.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
				xmlNamespaceResolver3.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
				xamlServiceProvider3.Add(typeFromHandle6, new XamlTypeResolver(xmlNamespaceResolver3, typeof(CodingModeSelectionV2.<InitializeComponent>_anonXamlCDataTemplate_3).GetTypeInfo().Assembly));
				xamlServiceProvider3.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(66, 33)));
				bindableObject.SetValue(valueTextFontSizeProperty, extendedTypeConverter.ConvertFromInvariantString(text, xamlServiceProvider3));
				return labelCell;
			}

			// Token: 0x0400305A RID: 12378
			internal object[] parentValues;

			// Token: 0x0400305B RID: 12379
			internal CodingModeSelectionV2 root;
		}
	}
}
