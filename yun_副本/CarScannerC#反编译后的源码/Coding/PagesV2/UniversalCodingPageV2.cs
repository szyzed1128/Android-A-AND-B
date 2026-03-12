using System;
using System.CodeDom.Compiler;
using System.Diagnostics;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using AiForms.Renderers;
using CarScannerXamarinForms.Common.XAMLConverters;
using CarScannerXamarinForms.InApp;
using CarScannerXamarinForms.Settings;
using Xamarin.CommunityToolkit.Converters;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Internals;
using Xamarin.Forms.PlatformConfiguration.iOSSpecific;
using Xamarin.Forms.Xaml;
using Xamarin.Forms.Xaml.Diagnostics;
using Xamarin.Forms.Xaml.Internals;

namespace CarScannerXamarinForms.Coding.PagesV2
{
	// Token: 0x020009A6 RID: 2470
	[XamlCompilation(2)]
	[XamlFilePath("Coding\\PagesV2\\UniversalCodingPageV2.xaml")]
	public class UniversalCodingPageV2 : ContentPage
	{
		// Token: 0x060050AC RID: 20652 RVA: 0x003E7308 File Offset: 0x003E5508
		public UniversalCodingPageV2(ICodingContainer coding)
		{
			this.InitializeComponent();
			this.coding = coding;
			try
			{
				base.BindingContext = this.coding;
			}
			catch (Exception)
			{
			}
			base.Appearing += this.CodingDetailsPage_Appearing;
		}

		// Token: 0x060050AD RID: 20653 RVA: 0x003E7364 File Offset: 0x003E5564
		private async void CodingDetailsPage_Appearing(object sender, EventArgs e)
		{
			if (this.first_appearing)
			{
				this.first_appearing = false;
				this.lv.IsEnabled = false;
				this.activityFrame.IsVisible = true;
				Progress<string> progress = new Progress<string>(delegate(string s)
				{
					MainThread.BeginInvokeOnMainThread(delegate
					{
						this.activityFrame.Text = ActivityFrame.PLEASE_WAIT_TEXT + "\n" + s;
					});
				});
				if (!App.OBDSimulator.IsActive)
				{
					if (this.coding is MQBAdaptationTemplate)
					{
						await this.coding.UpdateCurrentState("", progress);
					}
					else
					{
						await this.coding.UpdateCurrentState(this.entryPassword.Text, progress);
					}
				}
				this.activityFrame.IsVisible = false;
				this.lv.IsEnabled = true;
			}
		}

		// Token: 0x060050AE RID: 20654 RVA: 0x003E739C File Offset: 0x003E559C
		public async Task UpdateState()
		{
			this.activityFrame.Text = ActivityFrame.PLEASE_WAIT_TEXT;
			this.activityFrame.IsVisible = true;
			this.lv.IsEnabled = false;
			Progress<string> progress = new Progress<string>(delegate(string s)
			{
				MainThread.BeginInvokeOnMainThread(delegate
				{
					this.activityFrame.Text = ActivityFrame.PLEASE_WAIT_TEXT + "\n" + s;
				});
			});
			if (!App.OBDSimulator.IsActive)
			{
				if (this.coding is MQBAdaptationTemplate)
				{
					await this.coding.UpdateCurrentState("", progress);
				}
				else
				{
					await this.coding.UpdateCurrentState(this.entryPassword.Text, progress);
				}
			}
			this.activityFrame.IsVisible = false;
			this.lv.IsEnabled = true;
			this.activityFrame.Text = ActivityFrame.PLEASE_WAIT_TEXT;
		}

		// Token: 0x060050AF RID: 20655 RVA: 0x003E73E0 File Offset: 0x003E55E0
		private async void BtnUpdateState_Clicked(object sender, EventArgs e)
		{
			this.UpdateState();
		}

		// Token: 0x060050B0 RID: 20656 RVA: 0x003E7418 File Offset: 0x003E5618
		private async void optionCell_Tapped(object sender, EventArgs e)
		{
			MQBAdaptationOption mqbadaptationOption = (MQBAdaptationOption)((Element)sender).BindingContext;
			if (this.coding.RequiresPro && !SharedSettings.Current.AdsProductPurchased)
			{
				TaskAwaiter<bool> taskAwaiter = base.DisplayAlert("Car Scanner Pro", Translate.GetString("FeatureLocked"), "OK", Translate.GetString("btnCancel.Content")).GetAwaiter();
				if (!taskAwaiter.IsCompleted)
				{
					await taskAwaiter;
					TaskAwaiter<bool> taskAwaiter2;
					taskAwaiter = taskAwaiter2;
					taskAwaiter2 = default(TaskAwaiter<bool>);
				}
				if (taskAwaiter.GetResult())
				{
					Page inAppPage = InAppManager.GetInAppPage();
					await base.Navigation.PushAsync(inAppPage);
				}
			}
			else if (SharedSettings.Current.CodingsCounter >= 3 && !SharedSettings.Current.AdsProductPurchased)
			{
				TaskAwaiter<bool> taskAwaiter = base.DisplayAlert("Car Scanner Pro", string.Format(Translate.GetString("coding_FreeCodingsFinished"), SharedSettings.Current.CodingsCounter, 3), "OK", Translate.GetString("btnCancel.Content")).GetAwaiter();
				if (!taskAwaiter.IsCompleted)
				{
					await taskAwaiter;
					TaskAwaiter<bool> taskAwaiter2;
					taskAwaiter = taskAwaiter2;
					taskAwaiter2 = default(TaskAwaiter<bool>);
				}
				if (taskAwaiter.GetResult())
				{
					Page inAppPage2 = InAppManager.GetInAppPage();
					await base.Navigation.PushAsync(inAppPage2);
				}
			}
			else
			{
				this.activityFrame.IsVisible = true;
				this.lv.IsEnabled = false;
				Progress<string> progress = new Progress<string>(delegate(string s)
				{
					Device.BeginInvokeOnMainThread(delegate
					{
						this.activityFrame.Text = s;
					});
				});
				CodingRequestResult codingRequestResult = await this.coding.Execute(this.entryPassword.Text, mqbadaptationOption.Value, mqbadaptationOption.Title, progress, null, false);
				if (codingRequestResult == CodingRequestResult.Success)
				{
					SharedSettings.Current.CodingsCounter++;
					if (this.coding is IServiceProcedure)
					{
						await base.DisplayAlert(this.coding.Name, Translate.GetString("coding_OperationFinished"), "OK");
					}
				}
				else
				{
					await base.DisplayAlert(Translate.GetString("coding_Error"), MQBAdaptationTemplate.CodingRequestResultToString(codingRequestResult), "OK");
				}
				await this.coding.UpdateCurrentState("", progress);
				this.activityFrame.IsVisible = false;
				this.activityFrame.Text = ActivityFrame.PLEASE_WAIT_TEXT;
				this.lv.IsEnabled = true;
			}
		}

		// Token: 0x060050B1 RID: 20657 RVA: 0x003E7458 File Offset: 0x003E5658
		private async void btnSaveState_Clicked(object sender, EventArgs e)
		{
			double num;
			if (double.TryParse(this.entryValue.Text, out num) || this.coding is MQBMode22ParametrizeSilentDump || this.coding.ValueType == AdaptationValueTypes.InputTextType)
			{
				if (this.coding.RequiresPro && !SharedSettings.Current.AdsProductPurchased)
				{
					TaskAwaiter<bool> taskAwaiter = base.DisplayAlert("Car Scanner Pro", Translate.GetString("FeatureLocked"), "OK", Translate.GetString("btnCancel.Content")).GetAwaiter();
					if (!taskAwaiter.IsCompleted)
					{
						await taskAwaiter;
						TaskAwaiter<bool> taskAwaiter2;
						taskAwaiter = taskAwaiter2;
						taskAwaiter2 = default(TaskAwaiter<bool>);
					}
					if (taskAwaiter.GetResult())
					{
						Page inAppPage = InAppManager.GetInAppPage();
						await base.Navigation.PushAsync(inAppPage);
					}
				}
				else if (SharedSettings.Current.CodingsCounter >= 3 && !SharedSettings.Current.AdsProductPurchased)
				{
					TaskAwaiter<bool> taskAwaiter = base.DisplayAlert("Car Scanner Pro", string.Format(Translate.GetString("coding_FreeCodingsFinished"), SharedSettings.Current.CodingsCounter, 3), "OK", Translate.GetString("btnCancel.Content")).GetAwaiter();
					if (!taskAwaiter.IsCompleted)
					{
						await taskAwaiter;
						TaskAwaiter<bool> taskAwaiter2;
						taskAwaiter = taskAwaiter2;
						taskAwaiter2 = default(TaskAwaiter<bool>);
					}
					if (taskAwaiter.GetResult())
					{
						Page inAppPage2 = InAppManager.GetInAppPage();
						await base.Navigation.PushAsync(inAppPage2);
					}
				}
				else
				{
					this.activityFrame.IsVisible = true;
					this.entryValue.IsEnabled = false;
					Progress<string> progress = new Progress<string>(delegate(string s)
					{
						Device.BeginInvokeOnMainThread(delegate
						{
							this.activityFrame.Text = s;
						});
					});
					CodingRequestResult codingRequestResult = await this.coding.Execute(this.entryPassword.Text, this.entryValue.Text, this.entryValue.Text, progress, null, false);
					if (codingRequestResult == CodingRequestResult.Success)
					{
						SharedSettings.Current.CodingsCounter++;
					}
					else
					{
						await base.DisplayAlert(Translate.GetString("coding_Error"), MQBAdaptationTemplate.CodingRequestResultToString(codingRequestResult), "OK");
					}
					await this.coding.UpdateCurrentState("", null);
					this.activityFrame.IsVisible = false;
					this.activityFrame.Text = ActivityFrame.PLEASE_WAIT_TEXT;
					this.entryValue.IsEnabled = true;
				}
			}
			else
			{
				await base.DisplayAlert(Translate.GetString("coding_WrongInputFormatTitle"), Translate.GetString("coding_WrongInputFormatText") + "\n" + 123.45.ToString(), "OK");
			}
		}

		// Token: 0x060050B2 RID: 20658 RVA: 0x003E7490 File Offset: 0x003E5690
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private void InitializeComponent()
		{
			if (ResourceLoader.IsEnabled && ResourceLoader.CanProvideContentFor(new ResourceLoader.ResourceLoadingQuery
			{
				AssemblyName = typeof(UniversalCodingPageV2).GetTypeInfo().Assembly.GetName(),
				ResourcePath = "Coding/PagesV2/UniversalCodingPageV2.xaml",
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
			VisualDiagnostics.RegisterSourceInfo(translate = new Translate(), new Uri("Coding\\PagesV2\\UniversalCodingPageV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 15, 5);
			DynamicResourceExtension dynamicResourceExtension;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension = new DynamicResourceExtension(), new Uri("Coding\\PagesV2\\UniversalCodingPageV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 17, 5);
			EmptyStringToFalseConverter emptyStringToFalseConverter;
			VisualDiagnostics.RegisterSourceInfo(emptyStringToFalseConverter = new EmptyStringToFalseConverter(), new Uri("Coding\\PagesV2\\UniversalCodingPageV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 20, 14);
			EmptyStringToTrueConverter emptyStringToTrueConverter;
			VisualDiagnostics.RegisterSourceInfo(emptyStringToTrueConverter = new EmptyStringToTrueConverter(), new Uri("Coding\\PagesV2\\UniversalCodingPageV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 21, 14);
			VagCodingPlatformToTrueConverter vagCodingPlatformToTrueConverter;
			VisualDiagnostics.RegisterSourceInfo(vagCodingPlatformToTrueConverter = new VagCodingPlatformToTrueConverter(), new Uri("Coding\\PagesV2\\UniversalCodingPageV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 22, 14);
			AdaptationValueTypes adaptationValueTypes = AdaptationValueTypes.InputValueType;
			AdaptationValueTypes adaptationValueTypes2 = AdaptationValueTypes.InputTextType;
			AdaptationValueTypes adaptationValueTypes3 = AdaptationValueTypes.InputFloatIEEE754;
			EnumToBoolConverter enumToBoolConverter;
			VisualDiagnostics.RegisterSourceInfo(enumToBoolConverter = new EnumToBoolConverter(), new Uri("Coding\\PagesV2\\UniversalCodingPageV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 23, 14);
			AdaptationValueTypes adaptationValueTypes4 = AdaptationValueTypes.OptionType;
			AdaptationValueTypes adaptationValueTypes5 = AdaptationValueTypes.OptionType;
			EnumToBoolConverter enumToBoolConverter2;
			VisualDiagnostics.RegisterSourceInfo(enumToBoolConverter2 = new EnumToBoolConverter(), new Uri("Coding\\PagesV2\\UniversalCodingPageV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 32, 14);
			ResourceDictionary resourceDictionary;
			VisualDiagnostics.RegisterSourceInfo(resourceDictionary = new ResourceDictionary(), new Uri("Coding\\PagesV2\\UniversalCodingPageV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 19, 10);
			DynamicResourceExtension dynamicResourceExtension2;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension2 = new DynamicResourceExtension(), new Uri("Coding\\PagesV2\\UniversalCodingPageV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 52, 29);
			BindingExtension bindingExtension;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension = new BindingExtension(), new Uri("Coding\\PagesV2\\UniversalCodingPageV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 53, 29);
			DynamicResourceExtension dynamicResourceExtension3;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension3 = new DynamicResourceExtension(), new Uri("Coding\\PagesV2\\UniversalCodingPageV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 54, 29);
			Label label;
			VisualDiagnostics.RegisterSourceInfo(label = new Label(), new Uri("Coding\\PagesV2\\UniversalCodingPageV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 49, 26);
			StaticResourceExtension staticResourceExtension;
			VisualDiagnostics.RegisterSourceInfo(staticResourceExtension = new StaticResourceExtension(), new Uri("Coding\\PagesV2\\UniversalCodingPageV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 56, 57);
			BindingExtension bindingExtension2;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension2 = new BindingExtension(), new Uri("Coding\\PagesV2\\UniversalCodingPageV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 56, 57);
			BindingExtension bindingExtension3;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension3 = new BindingExtension(), new Uri("Coding\\PagesV2\\UniversalCodingPageV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 58, 32);
			Label label2;
			VisualDiagnostics.RegisterSourceInfo(label2 = new Label(), new Uri("Coding\\PagesV2\\UniversalCodingPageV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 58, 26);
			CustomCell customCell;
			VisualDiagnostics.RegisterSourceInfo(customCell = new CustomCell(), new Uri("Coding\\PagesV2\\UniversalCodingPageV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 56, 22);
			StaticResourceExtension staticResourceExtension2;
			VisualDiagnostics.RegisterSourceInfo(staticResourceExtension2 = new StaticResourceExtension(), new Uri("Coding\\PagesV2\\UniversalCodingPageV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 63, 57);
			BindingExtension bindingExtension4;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension4 = new BindingExtension(), new Uri("Coding\\PagesV2\\UniversalCodingPageV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 63, 57);
			Translate translate2;
			VisualDiagnostics.RegisterSourceInfo(translate2 = new Translate(), new Uri("Coding\\PagesV2\\UniversalCodingPageV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 67, 43);
			Span span;
			VisualDiagnostics.RegisterSourceInfo(span = new Span(), new Uri("Coding\\PagesV2\\UniversalCodingPageV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 67, 38);
			Span span2;
			VisualDiagnostics.RegisterSourceInfo(span2 = new Span(), new Uri("Coding\\PagesV2\\UniversalCodingPageV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 68, 38);
			BindingExtension bindingExtension5;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension5 = new BindingExtension(), new Uri("Coding\\PagesV2\\UniversalCodingPageV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 69, 43);
			Span span3;
			VisualDiagnostics.RegisterSourceInfo(span3 = new Span(), new Uri("Coding\\PagesV2\\UniversalCodingPageV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 69, 38);
			FormattedString formattedString;
			VisualDiagnostics.RegisterSourceInfo(formattedString = new FormattedString(), new Uri("Coding\\PagesV2\\UniversalCodingPageV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 66, 34);
			Label label3;
			VisualDiagnostics.RegisterSourceInfo(label3 = new Label(), new Uri("Coding\\PagesV2\\UniversalCodingPageV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 64, 26);
			CustomCell customCell2;
			VisualDiagnostics.RegisterSourceInfo(customCell2 = new CustomCell(), new Uri("Coding\\PagesV2\\UniversalCodingPageV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 63, 22);
			Section section;
			VisualDiagnostics.RegisterSourceInfo(section = new Section(), new Uri("Coding\\PagesV2\\UniversalCodingPageV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 47, 18);
			Translate translate3;
			VisualDiagnostics.RegisterSourceInfo(translate3 = new Translate(), new Uri("Coding\\PagesV2\\UniversalCodingPageV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 78, 29);
			BindingExtension bindingExtension6;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension6 = new BindingExtension(), new Uri("Coding\\PagesV2\\UniversalCodingPageV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 78, 76);
			BindingExtension bindingExtension7;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension7 = new BindingExtension(), new Uri("Coding\\PagesV2\\UniversalCodingPageV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 80, 55);
			Entry entry;
			VisualDiagnostics.RegisterSourceInfo(entry = new Entry(), new Uri("Coding\\PagesV2\\UniversalCodingPageV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 80, 26);
			CustomCell customCell3;
			VisualDiagnostics.RegisterSourceInfo(customCell3 = new CustomCell(), new Uri("Coding\\PagesV2\\UniversalCodingPageV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 79, 22);
			StaticResourceExtension staticResourceExtension3;
			VisualDiagnostics.RegisterSourceInfo(staticResourceExtension3 = new StaticResourceExtension(), new Uri("Coding\\PagesV2\\UniversalCodingPageV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 83, 57);
			BindingExtension bindingExtension8;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension8 = new BindingExtension(), new Uri("Coding\\PagesV2\\UniversalCodingPageV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 83, 57);
			Translate translate4;
			VisualDiagnostics.RegisterSourceInfo(translate4 = new Translate(), new Uri("Coding\\PagesV2\\UniversalCodingPageV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 87, 43);
			Span span4;
			VisualDiagnostics.RegisterSourceInfo(span4 = new Span(), new Uri("Coding\\PagesV2\\UniversalCodingPageV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 87, 38);
			BindingExtension bindingExtension9;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension9 = new BindingExtension(), new Uri("Coding\\PagesV2\\UniversalCodingPageV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 88, 43);
			Span span5;
			VisualDiagnostics.RegisterSourceInfo(span5 = new Span(), new Uri("Coding\\PagesV2\\UniversalCodingPageV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 88, 38);
			FormattedString formattedString2;
			VisualDiagnostics.RegisterSourceInfo(formattedString2 = new FormattedString(), new Uri("Coding\\PagesV2\\UniversalCodingPageV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 86, 34);
			Label label4;
			VisualDiagnostics.RegisterSourceInfo(label4 = new Label(), new Uri("Coding\\PagesV2\\UniversalCodingPageV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 84, 26);
			CustomCell customCell4;
			VisualDiagnostics.RegisterSourceInfo(customCell4 = new CustomCell(), new Uri("Coding\\PagesV2\\UniversalCodingPageV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 83, 22);
			StaticResourceExtension staticResourceExtension4;
			VisualDiagnostics.RegisterSourceInfo(staticResourceExtension4 = new StaticResourceExtension(), new Uri("Coding\\PagesV2\\UniversalCodingPageV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 93, 57);
			BindingExtension bindingExtension10;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension10 = new BindingExtension(), new Uri("Coding\\PagesV2\\UniversalCodingPageV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 93, 57);
			Translate translate5;
			VisualDiagnostics.RegisterSourceInfo(translate5 = new Translate(), new Uri("Coding\\PagesV2\\UniversalCodingPageV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 94, 32);
			Label label5;
			VisualDiagnostics.RegisterSourceInfo(label5 = new Label(), new Uri("Coding\\PagesV2\\UniversalCodingPageV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 94, 26);
			CustomCell customCell5;
			VisualDiagnostics.RegisterSourceInfo(customCell5 = new CustomCell(), new Uri("Coding\\PagesV2\\UniversalCodingPageV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 93, 22);
			Section section2;
			VisualDiagnostics.RegisterSourceInfo(section2 = new Section(), new Uri("Coding\\PagesV2\\UniversalCodingPageV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 78, 18);
			Translate translate6;
			VisualDiagnostics.RegisterSourceInfo(translate6 = new Translate(), new Uri("Coding\\PagesV2\\UniversalCodingPageV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 98, 29);
			BindingExtension bindingExtension11;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension11 = new BindingExtension(), new Uri("Coding\\PagesV2\\UniversalCodingPageV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 98, 80);
			BindingExtension bindingExtension12;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension12 = new BindingExtension(), new Uri("Coding\\PagesV2\\UniversalCodingPageV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 100, 35);
			LabelCell labelCell;
			VisualDiagnostics.RegisterSourceInfo(labelCell = new LabelCell(), new Uri("Coding\\PagesV2\\UniversalCodingPageV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 100, 22);
			Translate translate7;
			VisualDiagnostics.RegisterSourceInfo(translate7 = new Translate(), new Uri("Coding\\PagesV2\\UniversalCodingPageV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 104, 25);
			DynamicResourceExtension dynamicResourceExtension4;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension4 = new DynamicResourceExtension(), new Uri("Coding\\PagesV2\\UniversalCodingPageV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 106, 25);
			ButtonCell buttonCell;
			VisualDiagnostics.RegisterSourceInfo(buttonCell = new ButtonCell(), new Uri("Coding\\PagesV2\\UniversalCodingPageV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 102, 22);
			Section section3;
			VisualDiagnostics.RegisterSourceInfo(section3 = new Section(), new Uri("Coding\\PagesV2\\UniversalCodingPageV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 98, 18);
			Translate translate8;
			VisualDiagnostics.RegisterSourceInfo(translate8 = new Translate(), new Uri("Coding\\PagesV2\\UniversalCodingPageV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 111, 21);
			StaticResourceExtension staticResourceExtension5;
			VisualDiagnostics.RegisterSourceInfo(staticResourceExtension5 = new StaticResourceExtension(), new Uri("Coding\\PagesV2\\UniversalCodingPageV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 112, 21);
			BindingExtension bindingExtension13;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension13 = new BindingExtension(), new Uri("Coding\\PagesV2\\UniversalCodingPageV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 112, 21);
			BindingExtension bindingExtension14;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension14 = new BindingExtension(), new Uri("Coding\\PagesV2\\UniversalCodingPageV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 113, 21);
			DataTemplate dataTemplate;
			VisualDiagnostics.RegisterSourceInfo(dataTemplate = new DataTemplate(), new Uri("Coding\\PagesV2\\UniversalCodingPageV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 115, 26);
			Section section4;
			VisualDiagnostics.RegisterSourceInfo(section4 = new Section(), new Uri("Coding\\PagesV2\\UniversalCodingPageV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 109, 18);
			Translate translate9;
			VisualDiagnostics.RegisterSourceInfo(translate9 = new Translate(), new Uri("Coding\\PagesV2\\UniversalCodingPageV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 126, 21);
			StaticResourceExtension staticResourceExtension6;
			VisualDiagnostics.RegisterSourceInfo(staticResourceExtension6 = new StaticResourceExtension(), new Uri("Coding\\PagesV2\\UniversalCodingPageV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 127, 21);
			BindingExtension bindingExtension15;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension15 = new BindingExtension(), new Uri("Coding\\PagesV2\\UniversalCodingPageV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 127, 21);
			BindingExtension bindingExtension16;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension16 = new BindingExtension(), new Uri("Coding\\PagesV2\\UniversalCodingPageV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 129, 52);
			Entry entry2;
			VisualDiagnostics.RegisterSourceInfo(entry2 = new Entry(), new Uri("Coding\\PagesV2\\UniversalCodingPageV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 129, 26);
			CustomCell customCell6;
			VisualDiagnostics.RegisterSourceInfo(customCell6 = new CustomCell(), new Uri("Coding\\PagesV2\\UniversalCodingPageV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 128, 22);
			Translate translate10;
			VisualDiagnostics.RegisterSourceInfo(translate10 = new Translate(), new Uri("Coding\\PagesV2\\UniversalCodingPageV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 133, 25);
			DynamicResourceExtension dynamicResourceExtension5;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension5 = new DynamicResourceExtension(), new Uri("Coding\\PagesV2\\UniversalCodingPageV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 135, 25);
			ButtonCell buttonCell2;
			VisualDiagnostics.RegisterSourceInfo(buttonCell2 = new ButtonCell(), new Uri("Coding\\PagesV2\\UniversalCodingPageV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 131, 22);
			Section section5;
			VisualDiagnostics.RegisterSourceInfo(section5 = new Section(), new Uri("Coding\\PagesV2\\UniversalCodingPageV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 124, 18);
			Translate translate11;
			VisualDiagnostics.RegisterSourceInfo(translate11 = new Translate(), new Uri("Coding\\PagesV2\\UniversalCodingPageV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 140, 25);
			SharedSettings sharedSettings;
			VisualDiagnostics.RegisterSourceInfo(sharedSettings = SharedSettings.Current, new Uri("Coding\\PagesV2\\UniversalCodingPageV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 141, 25);
			StaticResourceExtension staticResourceExtension7;
			VisualDiagnostics.RegisterSourceInfo(staticResourceExtension7 = new StaticResourceExtension(), new Uri("Coding\\PagesV2\\UniversalCodingPageV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 142, 25);
			BindingExtension bindingExtension17;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension17 = new BindingExtension(), new Uri("Coding\\PagesV2\\UniversalCodingPageV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 142, 25);
			LabelCell labelCell2;
			VisualDiagnostics.RegisterSourceInfo(labelCell2 = new LabelCell(), new Uri("Coding\\PagesV2\\UniversalCodingPageV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 139, 22);
			Translate translate12;
			VisualDiagnostics.RegisterSourceInfo(translate12 = new Translate(), new Uri("Coding\\PagesV2\\UniversalCodingPageV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 144, 35);
			LabelCell labelCell3;
			VisualDiagnostics.RegisterSourceInfo(labelCell3 = new LabelCell(), new Uri("Coding\\PagesV2\\UniversalCodingPageV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 144, 22);
			Section section6;
			VisualDiagnostics.RegisterSourceInfo(section6 = new Section(), new Uri("Coding\\PagesV2\\UniversalCodingPageV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 138, 18);
			SettingsView settingsView;
			VisualDiagnostics.RegisterSourceInfo(settingsView = new SettingsView(), new Uri("Coding\\PagesV2\\UniversalCodingPageV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 43, 14);
			ActivityFrame activityFrame;
			VisualDiagnostics.RegisterSourceInfo(activityFrame = new ActivityFrame(), new Uri("Coding\\PagesV2\\UniversalCodingPageV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 152, 14);
			Grid grid;
			VisualDiagnostics.RegisterSourceInfo(grid = new Grid(), new Uri("Coding\\PagesV2\\UniversalCodingPageV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 42, 10);
			VisualDiagnostics.RegisterSourceInfo(this, new Uri("Coding\\PagesV2\\UniversalCodingPageV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 2, 2);
			NameScope nameScope = NameScope.GetNameScope(this) ?? new NameScope();
			NameScope.SetNameScope(this, nameScope);
			nameScope.RegisterName("page", this);
			if (this.StyleId == null)
			{
				this.StyleId = "page";
			}
			nameScope.RegisterName("lv", settingsView);
			if (settingsView.StyleId == null)
			{
				settingsView.StyleId = "lv";
			}
			nameScope.RegisterName("entryPassword", entry);
			if (entry.StyleId == null)
			{
				entry.StyleId = "entryPassword";
			}
			nameScope.RegisterName("btnUpdateState", buttonCell);
			if (buttonCell.StyleId == null)
			{
				buttonCell.StyleId = "btnUpdateState";
			}
			nameScope.RegisterName("optionsPanel", section4);
			if (section4.StyleId == null)
			{
				section4.StyleId = "optionsPanel";
			}
			nameScope.RegisterName("inputPanel", section5);
			if (section5.StyleId == null)
			{
				section5.StyleId = "inputPanel";
			}
			nameScope.RegisterName("entryValue", entry2);
			if (entry2.StyleId == null)
			{
				entry2.StyleId = "entryValue";
			}
			nameScope.RegisterName("btnSaveNewValue", buttonCell2);
			if (buttonCell2.StyleId == null)
			{
				buttonCell2.StyleId = "btnSaveNewValue";
			}
			nameScope.RegisterName("activityFrame", activityFrame);
			if (activityFrame.StyleId == null)
			{
				activityFrame.StyleId = "activityFrame";
			}
			this.page = this;
			this.lv = settingsView;
			this.entryPassword = entry;
			this.btnUpdateState = buttonCell;
			this.optionsPanel = section4;
			this.inputPanel = section5;
			this.entryValue = entry2;
			this.btnSaveNewValue = buttonCell2;
			this.activityFrame = activityFrame;
			this.Resources = resourceDictionary;
			resourceDictionary.Add("EmptyStringToFalseConverter", emptyStringToFalseConverter);
			resourceDictionary.Add("EmptyStringToTrueConverter", emptyStringToTrueConverter);
			resourceDictionary.Add("VagCodingPlatformToTrueConverter", vagCodingPlatformToTrueConverter);
			enumToBoolConverter.TrueValues.Add(adaptationValueTypes);
			enumToBoolConverter.TrueValues.Add(adaptationValueTypes2);
			enumToBoolConverter.TrueValues.Add(adaptationValueTypes3);
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
			xmlNamespaceResolver.Add("coding", "clr-namespace:CarScannerXamarinForms.Coding");
			xmlNamespaceResolver.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xmlNamespaceResolver.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider.Add(typeFromHandle2, new XamlTypeResolver(xmlNamespaceResolver, typeof(UniversalCodingPageV2).GetTypeInfo().Assembly));
			xamlServiceProvider.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(23, 14)));
			IValueConverter valueConverter = markupExtension.ProvideValue(xamlServiceProvider);
			resourceDictionary.Add("InputValueTypeToTrue", valueConverter);
			enumToBoolConverter2.TrueValues.Add(adaptationValueTypes4);
			enumToBoolConverter2.TrueValues.Add(adaptationValueTypes5);
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
			xmlNamespaceResolver2.Add("coding", "clr-namespace:CarScannerXamarinForms.Coding");
			xmlNamespaceResolver2.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver2.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver2.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver2.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver2.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xmlNamespaceResolver2.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver2.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider2.Add(typeFromHandle4, new XamlTypeResolver(xmlNamespaceResolver2, typeof(UniversalCodingPageV2).GetTypeInfo().Assembly));
			xamlServiceProvider2.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(32, 14)));
			IValueConverter valueConverter2 = markupExtension2.ProvideValue(xamlServiceProvider2);
			resourceDictionary.Add("InputOptionTypeToTrue", valueConverter2);
			translate.Text = "coding_Coding";
			IMarkupExtension markupExtension3 = translate;
			XamlServiceProvider xamlServiceProvider3 = new XamlServiceProvider();
			Type typeFromHandle5 = typeof(IProvideValueTarget);
			object[] array3 = new object[0 + 1];
			array3[0] = this;
			object obj3;
			xamlServiceProvider3.Add(typeFromHandle5, obj3 = new SimpleValueTargetProvider(array3, Page.TitleProperty, nameScope));
			xamlServiceProvider3.Add(typeof(IReferenceProvider), obj3);
			Type typeFromHandle6 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver3 = new XmlNamespaceResolver();
			xmlNamespaceResolver3.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver3.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver3.Add("coding", "clr-namespace:CarScannerXamarinForms.Coding");
			xmlNamespaceResolver3.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver3.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver3.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver3.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver3.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xmlNamespaceResolver3.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver3.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider3.Add(typeFromHandle6, new XamlTypeResolver(xmlNamespaceResolver3, typeof(UniversalCodingPageV2).GetTypeInfo().Assembly));
			xamlServiceProvider3.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(15, 5)));
			object obj4 = markupExtension3.ProvideValue(xamlServiceProvider3);
			this.Title = obj4;
			this.SetValue(Page.UseSafeAreaProperty, true);
			dynamicResourceExtension.Key = "BackgroundColor";
			IMarkupExtension<DynamicResource> markupExtension4 = dynamicResourceExtension;
			XamlServiceProvider xamlServiceProvider4 = new XamlServiceProvider();
			Type typeFromHandle7 = typeof(IProvideValueTarget);
			object[] array4 = new object[0 + 1];
			array4[0] = this;
			object obj5;
			xamlServiceProvider4.Add(typeFromHandle7, obj5 = new SimpleValueTargetProvider(array4, VisualElement.BackgroundColorProperty, nameScope));
			xamlServiceProvider4.Add(typeof(IReferenceProvider), obj5);
			Type typeFromHandle8 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver4 = new XmlNamespaceResolver();
			xmlNamespaceResolver4.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver4.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver4.Add("coding", "clr-namespace:CarScannerXamarinForms.Coding");
			xmlNamespaceResolver4.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver4.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver4.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver4.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver4.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xmlNamespaceResolver4.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver4.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider4.Add(typeFromHandle8, new XamlTypeResolver(xmlNamespaceResolver4, typeof(UniversalCodingPageV2).GetTypeInfo().Assembly));
			xamlServiceProvider4.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(17, 5)));
			DynamicResource dynamicResource = markupExtension4.ProvideValue(xamlServiceProvider4);
			this.SetDynamicResource(VisualElement.BackgroundColorProperty, dynamicResource.Key);
			this.Resources = resourceDictionary;
			grid.SetValue(Grid.RowDefinitionsProperty, new RowDefinitionCollectionTypeConverter().ConvertFromInvariantString("*"));
			settingsView.SetValue(Grid.RowProperty, 0);
			settingsView.SetValue(TableView.HasUnevenRowsProperty, true);
			label.SetValue(View.MarginProperty, new Thickness(12.0, 0.0, 0.0, 0.0));
			label.SetValue(Label.FontAttributesProperty, new FontAttributesConverter().ConvertFromInvariantString("Bold"));
			dynamicResourceExtension2.Key = "BaseFontSize+++";
			IMarkupExtension<DynamicResource> markupExtension5 = dynamicResourceExtension2;
			XamlServiceProvider xamlServiceProvider5 = new XamlServiceProvider();
			Type typeFromHandle9 = typeof(IProvideValueTarget);
			object[] array5 = new object[0 + 5];
			array5[0] = label;
			array5[1] = section;
			array5[2] = settingsView;
			array5[3] = grid;
			array5[4] = this;
			object obj6;
			xamlServiceProvider5.Add(typeFromHandle9, obj6 = new SimpleValueTargetProvider(array5, Label.FontSizeProperty, nameScope));
			xamlServiceProvider5.Add(typeof(IReferenceProvider), obj6);
			Type typeFromHandle10 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver5 = new XmlNamespaceResolver();
			xmlNamespaceResolver5.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver5.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver5.Add("coding", "clr-namespace:CarScannerXamarinForms.Coding");
			xmlNamespaceResolver5.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver5.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver5.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver5.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver5.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xmlNamespaceResolver5.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver5.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider5.Add(typeFromHandle10, new XamlTypeResolver(xmlNamespaceResolver5, typeof(UniversalCodingPageV2).GetTypeInfo().Assembly));
			xamlServiceProvider5.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(52, 29)));
			DynamicResource dynamicResource2 = markupExtension5.ProvideValue(xamlServiceProvider5);
			label.SetDynamicResource(Label.FontSizeProperty, dynamicResource2.Key);
			bindingExtension.Path = "Name";
			BindingBase bindingBase = bindingExtension.ProvideValue(null);
			label.SetBinding(Label.TextProperty, bindingBase);
			dynamicResourceExtension3.Key = "SettingsHeaderTextColor";
			IMarkupExtension<DynamicResource> markupExtension6 = dynamicResourceExtension3;
			XamlServiceProvider xamlServiceProvider6 = new XamlServiceProvider();
			Type typeFromHandle11 = typeof(IProvideValueTarget);
			object[] array6 = new object[0 + 5];
			array6[0] = label;
			array6[1] = section;
			array6[2] = settingsView;
			array6[3] = grid;
			array6[4] = this;
			object obj7;
			xamlServiceProvider6.Add(typeFromHandle11, obj7 = new SimpleValueTargetProvider(array6, Label.TextColorProperty, nameScope));
			xamlServiceProvider6.Add(typeof(IReferenceProvider), obj7);
			Type typeFromHandle12 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver6 = new XmlNamespaceResolver();
			xmlNamespaceResolver6.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver6.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver6.Add("coding", "clr-namespace:CarScannerXamarinForms.Coding");
			xmlNamespaceResolver6.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver6.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver6.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver6.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver6.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xmlNamespaceResolver6.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver6.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider6.Add(typeFromHandle12, new XamlTypeResolver(xmlNamespaceResolver6, typeof(UniversalCodingPageV2).GetTypeInfo().Assembly));
			xamlServiceProvider6.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(54, 29)));
			DynamicResource dynamicResource3 = markupExtension6.ProvideValue(xamlServiceProvider6);
			label.SetDynamicResource(Label.TextColorProperty, dynamicResource3.Key);
			section.SetValue(Section.HeaderViewProperty, label);
			customCell.SetValue(CustomCell.IsSelectableProperty, false);
			staticResourceExtension.Key = "EmptyStringToFalseConverter";
			IMarkupExtension markupExtension7 = staticResourceExtension;
			XamlServiceProvider xamlServiceProvider7 = new XamlServiceProvider();
			Type typeFromHandle13 = typeof(IProvideValueTarget);
			object[] array7 = new object[0 + 6];
			array7[0] = bindingExtension2;
			array7[1] = customCell;
			array7[2] = section;
			array7[3] = settingsView;
			array7[4] = grid;
			array7[5] = this;
			object obj8;
			xamlServiceProvider7.Add(typeFromHandle13, obj8 = new SimpleValueTargetProvider(array7, typeof(BindingExtension).GetRuntimeProperty("Converter"), nameScope));
			xamlServiceProvider7.Add(typeof(IReferenceProvider), obj8);
			Type typeFromHandle14 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver7 = new XmlNamespaceResolver();
			xmlNamespaceResolver7.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver7.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver7.Add("coding", "clr-namespace:CarScannerXamarinForms.Coding");
			xmlNamespaceResolver7.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver7.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver7.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver7.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver7.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xmlNamespaceResolver7.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver7.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider7.Add(typeFromHandle14, new XamlTypeResolver(xmlNamespaceResolver7, typeof(UniversalCodingPageV2).GetTypeInfo().Assembly));
			xamlServiceProvider7.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(56, 57)));
			object obj9 = markupExtension7.ProvideValue(xamlServiceProvider7);
			bindingExtension2.Converter = obj9;
			bindingExtension2.Path = "Description";
			BindingBase bindingBase2 = bindingExtension2.ProvideValue(null);
			customCell.SetBinding(CellBase.IsVisibleProperty, bindingBase2);
			bindingExtension3.Path = "Description";
			BindingBase bindingBase3 = bindingExtension3.ProvideValue(null);
			label2.SetBinding(Label.TextProperty, bindingBase3);
			customCell.SetValue(CustomCell.ContentProperty, label2);
			section.Add(customCell);
			customCell2.SetValue(CustomCell.IsSelectableProperty, false);
			staticResourceExtension2.Key = "EmptyStringToFalseConverter";
			IMarkupExtension markupExtension8 = staticResourceExtension2;
			XamlServiceProvider xamlServiceProvider8 = new XamlServiceProvider();
			Type typeFromHandle15 = typeof(IProvideValueTarget);
			object[] array8 = new object[0 + 6];
			array8[0] = bindingExtension4;
			array8[1] = customCell2;
			array8[2] = section;
			array8[3] = settingsView;
			array8[4] = grid;
			array8[5] = this;
			object obj10;
			xamlServiceProvider8.Add(typeFromHandle15, obj10 = new SimpleValueTargetProvider(array8, typeof(BindingExtension).GetRuntimeProperty("Converter"), nameScope));
			xamlServiceProvider8.Add(typeof(IReferenceProvider), obj10);
			Type typeFromHandle16 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver8 = new XmlNamespaceResolver();
			xmlNamespaceResolver8.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver8.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver8.Add("coding", "clr-namespace:CarScannerXamarinForms.Coding");
			xmlNamespaceResolver8.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver8.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver8.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver8.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver8.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xmlNamespaceResolver8.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver8.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider8.Add(typeFromHandle16, new XamlTypeResolver(xmlNamespaceResolver8, typeof(UniversalCodingPageV2).GetTypeInfo().Assembly));
			xamlServiceProvider8.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(63, 57)));
			object obj11 = markupExtension8.ProvideValue(xamlServiceProvider8);
			bindingExtension4.Converter = obj11;
			bindingExtension4.Path = "InnerDescription";
			BindingBase bindingBase4 = bindingExtension4.ProvideValue(null);
			customCell2.SetBinding(CellBase.IsVisibleProperty, bindingBase4);
			translate2.Text = "info_IMPORTANT";
			IMarkupExtension markupExtension9 = translate2;
			XamlServiceProvider xamlServiceProvider9 = new XamlServiceProvider();
			Type typeFromHandle17 = typeof(IProvideValueTarget);
			object[] array9 = new object[0 + 8];
			array9[0] = span;
			array9[1] = formattedString;
			array9[2] = label3;
			array9[3] = customCell2;
			array9[4] = section;
			array9[5] = settingsView;
			array9[6] = grid;
			array9[7] = this;
			object obj12;
			xamlServiceProvider9.Add(typeFromHandle17, obj12 = new SimpleValueTargetProvider(array9, Span.TextProperty, nameScope));
			xamlServiceProvider9.Add(typeof(IReferenceProvider), obj12);
			Type typeFromHandle18 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver9 = new XmlNamespaceResolver();
			xmlNamespaceResolver9.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver9.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver9.Add("coding", "clr-namespace:CarScannerXamarinForms.Coding");
			xmlNamespaceResolver9.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver9.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver9.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver9.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver9.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xmlNamespaceResolver9.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver9.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider9.Add(typeFromHandle18, new XamlTypeResolver(xmlNamespaceResolver9, typeof(UniversalCodingPageV2).GetTypeInfo().Assembly));
			xamlServiceProvider9.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(67, 43)));
			object obj13 = markupExtension9.ProvideValue(xamlServiceProvider9);
			span.Text = obj13;
			span.SetValue(Span.TextColorProperty, Color.Red);
			formattedString.Spans.Add(span);
			span2.SetValue(Span.TextProperty, " ");
			formattedString.Spans.Add(span2);
			bindingExtension5.Path = "InnerDescription";
			BindingBase bindingBase5 = bindingExtension5.ProvideValue(null);
			span3.SetBinding(Span.TextProperty, bindingBase5);
			formattedString.Spans.Add(span3);
			label3.SetValue(Label.FormattedTextProperty, formattedString);
			customCell2.SetValue(CustomCell.ContentProperty, label3);
			section.Add(customCell2);
			settingsView.Root.Add(section);
			translate3.Text = "coding_Password";
			IMarkupExtension markupExtension10 = translate3;
			XamlServiceProvider xamlServiceProvider10 = new XamlServiceProvider();
			Type typeFromHandle19 = typeof(IProvideValueTarget);
			object[] array10 = new object[0 + 4];
			array10[0] = section2;
			array10[1] = settingsView;
			array10[2] = grid;
			array10[3] = this;
			object obj14;
			xamlServiceProvider10.Add(typeFromHandle19, obj14 = new SimpleValueTargetProvider(array10, SectionBase.TitleProperty, nameScope));
			xamlServiceProvider10.Add(typeof(IReferenceProvider), obj14);
			Type typeFromHandle20 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver10 = new XmlNamespaceResolver();
			xmlNamespaceResolver10.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver10.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver10.Add("coding", "clr-namespace:CarScannerXamarinForms.Coding");
			xmlNamespaceResolver10.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver10.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver10.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver10.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver10.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xmlNamespaceResolver10.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver10.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider10.Add(typeFromHandle20, new XamlTypeResolver(xmlNamespaceResolver10, typeof(UniversalCodingPageV2).GetTypeInfo().Assembly));
			xamlServiceProvider10.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(78, 29)));
			object obj15 = markupExtension10.ProvideValue(xamlServiceProvider10);
			section2.Title = obj15;
			bindingExtension6.Path = "PasswordVisible";
			BindingBase bindingBase6 = bindingExtension6.ProvideValue(null);
			section2.SetBinding(Section.IsVisibleProperty, bindingBase6);
			customCell3.SetValue(CustomCell.IsSelectableProperty, false);
			bindingExtension7.Mode = 1;
			bindingExtension7.Path = "Password";
			BindingBase bindingBase7 = bindingExtension7.ProvideValue(null);
			entry.SetBinding(Entry.TextProperty, bindingBase7);
			customCell3.SetValue(CustomCell.ContentProperty, entry);
			section2.Add(customCell3);
			customCell4.SetValue(CustomCell.IsSelectableProperty, false);
			staticResourceExtension3.Key = "EmptyStringToFalseConverter";
			IMarkupExtension markupExtension11 = staticResourceExtension3;
			XamlServiceProvider xamlServiceProvider11 = new XamlServiceProvider();
			Type typeFromHandle21 = typeof(IProvideValueTarget);
			object[] array11 = new object[0 + 6];
			array11[0] = bindingExtension8;
			array11[1] = customCell4;
			array11[2] = section2;
			array11[3] = settingsView;
			array11[4] = grid;
			array11[5] = this;
			object obj16;
			xamlServiceProvider11.Add(typeFromHandle21, obj16 = new SimpleValueTargetProvider(array11, typeof(BindingExtension).GetRuntimeProperty("Converter"), nameScope));
			xamlServiceProvider11.Add(typeof(IReferenceProvider), obj16);
			Type typeFromHandle22 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver11 = new XmlNamespaceResolver();
			xmlNamespaceResolver11.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver11.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver11.Add("coding", "clr-namespace:CarScannerXamarinForms.Coding");
			xmlNamespaceResolver11.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver11.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver11.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver11.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver11.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xmlNamespaceResolver11.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver11.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider11.Add(typeFromHandle22, new XamlTypeResolver(xmlNamespaceResolver11, typeof(UniversalCodingPageV2).GetTypeInfo().Assembly));
			xamlServiceProvider11.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(83, 57)));
			object obj17 = markupExtension11.ProvideValue(xamlServiceProvider11);
			bindingExtension8.Converter = obj17;
			bindingExtension8.Path = "PasswordHint";
			BindingBase bindingBase8 = bindingExtension8.ProvideValue(null);
			customCell4.SetBinding(CellBase.IsVisibleProperty, bindingBase8);
			label4.SetValue(Label.FontAttributesProperty, new FontAttributesConverter().ConvertFromInvariantString("Italic"));
			translate4.Text = "coding_PasswordHint";
			IMarkupExtension markupExtension12 = translate4;
			XamlServiceProvider xamlServiceProvider12 = new XamlServiceProvider();
			Type typeFromHandle23 = typeof(IProvideValueTarget);
			object[] array12 = new object[0 + 8];
			array12[0] = span4;
			array12[1] = formattedString2;
			array12[2] = label4;
			array12[3] = customCell4;
			array12[4] = section2;
			array12[5] = settingsView;
			array12[6] = grid;
			array12[7] = this;
			object obj18;
			xamlServiceProvider12.Add(typeFromHandle23, obj18 = new SimpleValueTargetProvider(array12, Span.TextProperty, nameScope));
			xamlServiceProvider12.Add(typeof(IReferenceProvider), obj18);
			Type typeFromHandle24 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver12 = new XmlNamespaceResolver();
			xmlNamespaceResolver12.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver12.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver12.Add("coding", "clr-namespace:CarScannerXamarinForms.Coding");
			xmlNamespaceResolver12.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver12.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver12.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver12.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver12.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xmlNamespaceResolver12.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver12.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider12.Add(typeFromHandle24, new XamlTypeResolver(xmlNamespaceResolver12, typeof(UniversalCodingPageV2).GetTypeInfo().Assembly));
			xamlServiceProvider12.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(87, 43)));
			object obj19 = markupExtension12.ProvideValue(xamlServiceProvider12);
			span4.Text = obj19;
			formattedString2.Spans.Add(span4);
			bindingExtension9.Path = "PasswordHint";
			BindingBase bindingBase9 = bindingExtension9.ProvideValue(null);
			span5.SetBinding(Span.TextProperty, bindingBase9);
			formattedString2.Spans.Add(span5);
			label4.SetValue(Label.FormattedTextProperty, formattedString2);
			customCell4.SetValue(CustomCell.ContentProperty, label4);
			section2.Add(customCell4);
			customCell5.SetValue(CustomCell.IsSelectableProperty, false);
			bindingExtension10.Mode = 2;
			staticResourceExtension4.Key = "EmptyStringToTrueConverter";
			IMarkupExtension markupExtension13 = staticResourceExtension4;
			XamlServiceProvider xamlServiceProvider13 = new XamlServiceProvider();
			Type typeFromHandle25 = typeof(IProvideValueTarget);
			object[] array13 = new object[0 + 6];
			array13[0] = bindingExtension10;
			array13[1] = customCell5;
			array13[2] = section2;
			array13[3] = settingsView;
			array13[4] = grid;
			array13[5] = this;
			object obj20;
			xamlServiceProvider13.Add(typeFromHandle25, obj20 = new SimpleValueTargetProvider(array13, typeof(BindingExtension).GetRuntimeProperty("Converter"), nameScope));
			xamlServiceProvider13.Add(typeof(IReferenceProvider), obj20);
			Type typeFromHandle26 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver13 = new XmlNamespaceResolver();
			xmlNamespaceResolver13.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver13.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver13.Add("coding", "clr-namespace:CarScannerXamarinForms.Coding");
			xmlNamespaceResolver13.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver13.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver13.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver13.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver13.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xmlNamespaceResolver13.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver13.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider13.Add(typeFromHandle26, new XamlTypeResolver(xmlNamespaceResolver13, typeof(UniversalCodingPageV2).GetTypeInfo().Assembly));
			xamlServiceProvider13.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(93, 57)));
			object obj21 = markupExtension13.ProvideValue(xamlServiceProvider13);
			bindingExtension10.Converter = obj21;
			bindingExtension10.Path = "Password";
			BindingBase bindingBase10 = bindingExtension10.ProvideValue(null);
			customCell5.SetBinding(CellBase.IsVisibleProperty, bindingBase10);
			translate5.Text = "coding_PasswordNotRequired";
			IMarkupExtension markupExtension14 = translate5;
			XamlServiceProvider xamlServiceProvider14 = new XamlServiceProvider();
			Type typeFromHandle27 = typeof(IProvideValueTarget);
			object[] array14 = new object[0 + 6];
			array14[0] = label5;
			array14[1] = customCell5;
			array14[2] = section2;
			array14[3] = settingsView;
			array14[4] = grid;
			array14[5] = this;
			object obj22;
			xamlServiceProvider14.Add(typeFromHandle27, obj22 = new SimpleValueTargetProvider(array14, Label.TextProperty, nameScope));
			xamlServiceProvider14.Add(typeof(IReferenceProvider), obj22);
			Type typeFromHandle28 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver14 = new XmlNamespaceResolver();
			xmlNamespaceResolver14.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver14.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver14.Add("coding", "clr-namespace:CarScannerXamarinForms.Coding");
			xmlNamespaceResolver14.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver14.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver14.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver14.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver14.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xmlNamespaceResolver14.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver14.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider14.Add(typeFromHandle28, new XamlTypeResolver(xmlNamespaceResolver14, typeof(UniversalCodingPageV2).GetTypeInfo().Assembly));
			xamlServiceProvider14.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(94, 32)));
			object obj23 = markupExtension14.ProvideValue(xamlServiceProvider14);
			label5.Text = obj23;
			customCell5.SetValue(CustomCell.ContentProperty, label5);
			section2.Add(customCell5);
			settingsView.Root.Add(section2);
			translate6.Text = "coding_CurrentState";
			IMarkupExtension markupExtension15 = translate6;
			XamlServiceProvider xamlServiceProvider15 = new XamlServiceProvider();
			Type typeFromHandle29 = typeof(IProvideValueTarget);
			object[] array15 = new object[0 + 4];
			array15[0] = section3;
			array15[1] = settingsView;
			array15[2] = grid;
			array15[3] = this;
			object obj24;
			xamlServiceProvider15.Add(typeFromHandle29, obj24 = new SimpleValueTargetProvider(array15, SectionBase.TitleProperty, nameScope));
			xamlServiceProvider15.Add(typeof(IReferenceProvider), obj24);
			Type typeFromHandle30 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver15 = new XmlNamespaceResolver();
			xmlNamespaceResolver15.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver15.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver15.Add("coding", "clr-namespace:CarScannerXamarinForms.Coding");
			xmlNamespaceResolver15.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver15.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver15.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver15.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver15.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xmlNamespaceResolver15.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver15.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider15.Add(typeFromHandle30, new XamlTypeResolver(xmlNamespaceResolver15, typeof(UniversalCodingPageV2).GetTypeInfo().Assembly));
			xamlServiceProvider15.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(98, 29)));
			object obj25 = markupExtension15.ProvideValue(xamlServiceProvider15);
			section3.Title = obj25;
			bindingExtension11.Mode = 2;
			bindingExtension11.Path = "HasCurrentState";
			BindingBase bindingBase11 = bindingExtension11.ProvideValue(null);
			section3.SetBinding(Section.IsVisibleProperty, bindingBase11);
			bindingExtension12.Mode = 2;
			bindingExtension12.Path = "CurrentState";
			BindingBase bindingBase12 = bindingExtension12.ProvideValue(null);
			labelCell.SetBinding(CellBase.TitleProperty, bindingBase12);
			labelCell.SetValue(CellBase.TitleFontAttributesProperty, new FontAttributes?(1));
			section3.Add(labelCell);
			translate7.Text = "Mode06Page_btnRefresh.Content";
			IMarkupExtension markupExtension16 = translate7;
			XamlServiceProvider xamlServiceProvider16 = new XamlServiceProvider();
			Type typeFromHandle31 = typeof(IProvideValueTarget);
			object[] array16 = new object[0 + 5];
			array16[0] = buttonCell;
			array16[1] = section3;
			array16[2] = settingsView;
			array16[3] = grid;
			array16[4] = this;
			object obj26;
			xamlServiceProvider16.Add(typeFromHandle31, obj26 = new SimpleValueTargetProvider(array16, CellBase.TitleProperty, nameScope));
			xamlServiceProvider16.Add(typeof(IReferenceProvider), obj26);
			Type typeFromHandle32 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver16 = new XmlNamespaceResolver();
			xmlNamespaceResolver16.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver16.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver16.Add("coding", "clr-namespace:CarScannerXamarinForms.Coding");
			xmlNamespaceResolver16.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver16.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver16.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver16.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver16.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xmlNamespaceResolver16.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver16.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider16.Add(typeFromHandle32, new XamlTypeResolver(xmlNamespaceResolver16, typeof(UniversalCodingPageV2).GetTypeInfo().Assembly));
			xamlServiceProvider16.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(104, 25)));
			object obj27 = markupExtension16.ProvideValue(xamlServiceProvider16);
			buttonCell.Title = obj27;
			buttonCell.Tapped += this.BtnUpdateState_Clicked;
			dynamicResourceExtension4.Key = "ButtonAccentColor";
			IMarkupExtension<DynamicResource> markupExtension17 = dynamicResourceExtension4;
			XamlServiceProvider xamlServiceProvider17 = new XamlServiceProvider();
			Type typeFromHandle33 = typeof(IProvideValueTarget);
			object[] array17 = new object[0 + 5];
			array17[0] = buttonCell;
			array17[1] = section3;
			array17[2] = settingsView;
			array17[3] = grid;
			array17[4] = this;
			object obj28;
			xamlServiceProvider17.Add(typeFromHandle33, obj28 = new SimpleValueTargetProvider(array17, CellBase.TitleColorProperty, nameScope));
			xamlServiceProvider17.Add(typeof(IReferenceProvider), obj28);
			Type typeFromHandle34 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver17 = new XmlNamespaceResolver();
			xmlNamespaceResolver17.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver17.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver17.Add("coding", "clr-namespace:CarScannerXamarinForms.Coding");
			xmlNamespaceResolver17.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver17.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver17.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver17.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver17.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xmlNamespaceResolver17.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver17.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider17.Add(typeFromHandle34, new XamlTypeResolver(xmlNamespaceResolver17, typeof(UniversalCodingPageV2).GetTypeInfo().Assembly));
			xamlServiceProvider17.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(106, 25)));
			DynamicResource dynamicResource4 = markupExtension17.ProvideValue(xamlServiceProvider17);
			buttonCell.SetDynamicResource(CellBase.TitleColorProperty, dynamicResource4.Key);
			section3.Add(buttonCell);
			settingsView.Root.Add(section3);
			translate8.Text = "coding_ChooseOption";
			IMarkupExtension markupExtension18 = translate8;
			XamlServiceProvider xamlServiceProvider18 = new XamlServiceProvider();
			Type typeFromHandle35 = typeof(IProvideValueTarget);
			object[] array18 = new object[0 + 4];
			array18[0] = section4;
			array18[1] = settingsView;
			array18[2] = grid;
			array18[3] = this;
			object obj29;
			xamlServiceProvider18.Add(typeFromHandle35, obj29 = new SimpleValueTargetProvider(array18, SectionBase.TitleProperty, nameScope));
			xamlServiceProvider18.Add(typeof(IReferenceProvider), obj29);
			Type typeFromHandle36 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver18 = new XmlNamespaceResolver();
			xmlNamespaceResolver18.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver18.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver18.Add("coding", "clr-namespace:CarScannerXamarinForms.Coding");
			xmlNamespaceResolver18.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver18.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver18.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver18.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver18.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xmlNamespaceResolver18.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver18.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider18.Add(typeFromHandle36, new XamlTypeResolver(xmlNamespaceResolver18, typeof(UniversalCodingPageV2).GetTypeInfo().Assembly));
			xamlServiceProvider18.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(111, 21)));
			object obj30 = markupExtension18.ProvideValue(xamlServiceProvider18);
			section4.Title = obj30;
			staticResourceExtension5.Key = "InputOptionTypeToTrue";
			IMarkupExtension markupExtension19 = staticResourceExtension5;
			XamlServiceProvider xamlServiceProvider19 = new XamlServiceProvider();
			Type typeFromHandle37 = typeof(IProvideValueTarget);
			object[] array19 = new object[0 + 5];
			array19[0] = bindingExtension13;
			array19[1] = section4;
			array19[2] = settingsView;
			array19[3] = grid;
			array19[4] = this;
			object obj31;
			xamlServiceProvider19.Add(typeFromHandle37, obj31 = new SimpleValueTargetProvider(array19, typeof(BindingExtension).GetRuntimeProperty("Converter"), nameScope));
			xamlServiceProvider19.Add(typeof(IReferenceProvider), obj31);
			Type typeFromHandle38 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver19 = new XmlNamespaceResolver();
			xmlNamespaceResolver19.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver19.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver19.Add("coding", "clr-namespace:CarScannerXamarinForms.Coding");
			xmlNamespaceResolver19.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver19.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver19.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver19.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver19.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xmlNamespaceResolver19.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver19.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider19.Add(typeFromHandle38, new XamlTypeResolver(xmlNamespaceResolver19, typeof(UniversalCodingPageV2).GetTypeInfo().Assembly));
			xamlServiceProvider19.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(112, 21)));
			object obj32 = markupExtension19.ProvideValue(xamlServiceProvider19);
			bindingExtension13.Converter = obj32;
			bindingExtension13.Path = "ValueType";
			BindingBase bindingBase13 = bindingExtension13.ProvideValue(null);
			section4.SetBinding(Section.IsVisibleProperty, bindingBase13);
			bindingExtension14.Path = "Options";
			BindingBase bindingBase14 = bindingExtension14.ProvideValue(null);
			section4.SetBinding(Section.ItemsSourceProperty, bindingBase14);
			IDataTemplate dataTemplate2 = dataTemplate;
			UniversalCodingPageV2.<InitializeComponent>_anonXamlCDataTemplate_5 <InitializeComponent>_anonXamlCDataTemplate_ = new UniversalCodingPageV2.<InitializeComponent>_anonXamlCDataTemplate_5();
			object[] array20 = new object[0 + 5];
			array20[0] = dataTemplate;
			array20[1] = section4;
			array20[2] = settingsView;
			array20[3] = grid;
			array20[4] = this;
			<InitializeComponent>_anonXamlCDataTemplate_.parentValues = array20;
			<InitializeComponent>_anonXamlCDataTemplate_.root = this;
			dataTemplate2.LoadTemplate = new Func<object>(<InitializeComponent>_anonXamlCDataTemplate_.LoadDataTemplate);
			section4.SetValue(Section.ItemTemplateProperty, dataTemplate);
			settingsView.Root.Add(section4);
			translate9.Text = "coding_TypeNewValue";
			IMarkupExtension markupExtension20 = translate9;
			XamlServiceProvider xamlServiceProvider20 = new XamlServiceProvider();
			Type typeFromHandle39 = typeof(IProvideValueTarget);
			object[] array21 = new object[0 + 4];
			array21[0] = section5;
			array21[1] = settingsView;
			array21[2] = grid;
			array21[3] = this;
			object obj33;
			xamlServiceProvider20.Add(typeFromHandle39, obj33 = new SimpleValueTargetProvider(array21, SectionBase.TitleProperty, nameScope));
			xamlServiceProvider20.Add(typeof(IReferenceProvider), obj33);
			Type typeFromHandle40 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver20 = new XmlNamespaceResolver();
			xmlNamespaceResolver20.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver20.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver20.Add("coding", "clr-namespace:CarScannerXamarinForms.Coding");
			xmlNamespaceResolver20.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver20.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver20.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver20.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver20.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xmlNamespaceResolver20.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver20.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider20.Add(typeFromHandle40, new XamlTypeResolver(xmlNamespaceResolver20, typeof(UniversalCodingPageV2).GetTypeInfo().Assembly));
			xamlServiceProvider20.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(126, 21)));
			object obj34 = markupExtension20.ProvideValue(xamlServiceProvider20);
			section5.Title = obj34;
			staticResourceExtension6.Key = "InputValueTypeToTrue";
			IMarkupExtension markupExtension21 = staticResourceExtension6;
			XamlServiceProvider xamlServiceProvider21 = new XamlServiceProvider();
			Type typeFromHandle41 = typeof(IProvideValueTarget);
			object[] array22 = new object[0 + 5];
			array22[0] = bindingExtension15;
			array22[1] = section5;
			array22[2] = settingsView;
			array22[3] = grid;
			array22[4] = this;
			object obj35;
			xamlServiceProvider21.Add(typeFromHandle41, obj35 = new SimpleValueTargetProvider(array22, typeof(BindingExtension).GetRuntimeProperty("Converter"), nameScope));
			xamlServiceProvider21.Add(typeof(IReferenceProvider), obj35);
			Type typeFromHandle42 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver21 = new XmlNamespaceResolver();
			xmlNamespaceResolver21.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver21.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver21.Add("coding", "clr-namespace:CarScannerXamarinForms.Coding");
			xmlNamespaceResolver21.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver21.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver21.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver21.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver21.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xmlNamespaceResolver21.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver21.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider21.Add(typeFromHandle42, new XamlTypeResolver(xmlNamespaceResolver21, typeof(UniversalCodingPageV2).GetTypeInfo().Assembly));
			xamlServiceProvider21.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(127, 21)));
			object obj36 = markupExtension21.ProvideValue(xamlServiceProvider21);
			bindingExtension15.Converter = obj36;
			bindingExtension15.Path = "ValueType";
			BindingBase bindingBase15 = bindingExtension15.ProvideValue(null);
			section5.SetBinding(Section.IsVisibleProperty, bindingBase15);
			customCell6.SetValue(CustomCell.IsSelectableProperty, false);
			bindingExtension16.Mode = 2;
			bindingExtension16.Path = "CurrentState";
			BindingBase bindingBase16 = bindingExtension16.ProvideValue(null);
			entry2.SetBinding(Entry.TextProperty, bindingBase16);
			customCell6.SetValue(CustomCell.ContentProperty, entry2);
			section5.Add(customCell6);
			translate10.Text = "coding_Apply";
			IMarkupExtension markupExtension22 = translate10;
			XamlServiceProvider xamlServiceProvider22 = new XamlServiceProvider();
			Type typeFromHandle43 = typeof(IProvideValueTarget);
			object[] array23 = new object[0 + 5];
			array23[0] = buttonCell2;
			array23[1] = section5;
			array23[2] = settingsView;
			array23[3] = grid;
			array23[4] = this;
			object obj37;
			xamlServiceProvider22.Add(typeFromHandle43, obj37 = new SimpleValueTargetProvider(array23, CellBase.TitleProperty, nameScope));
			xamlServiceProvider22.Add(typeof(IReferenceProvider), obj37);
			Type typeFromHandle44 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver22 = new XmlNamespaceResolver();
			xmlNamespaceResolver22.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver22.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver22.Add("coding", "clr-namespace:CarScannerXamarinForms.Coding");
			xmlNamespaceResolver22.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver22.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver22.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver22.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver22.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xmlNamespaceResolver22.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver22.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider22.Add(typeFromHandle44, new XamlTypeResolver(xmlNamespaceResolver22, typeof(UniversalCodingPageV2).GetTypeInfo().Assembly));
			xamlServiceProvider22.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(133, 25)));
			object obj38 = markupExtension22.ProvideValue(xamlServiceProvider22);
			buttonCell2.Title = obj38;
			buttonCell2.Tapped += this.btnSaveState_Clicked;
			dynamicResourceExtension5.Key = "ButtonAccentColor";
			IMarkupExtension<DynamicResource> markupExtension23 = dynamicResourceExtension5;
			XamlServiceProvider xamlServiceProvider23 = new XamlServiceProvider();
			Type typeFromHandle45 = typeof(IProvideValueTarget);
			object[] array24 = new object[0 + 5];
			array24[0] = buttonCell2;
			array24[1] = section5;
			array24[2] = settingsView;
			array24[3] = grid;
			array24[4] = this;
			object obj39;
			xamlServiceProvider23.Add(typeFromHandle45, obj39 = new SimpleValueTargetProvider(array24, CellBase.TitleColorProperty, nameScope));
			xamlServiceProvider23.Add(typeof(IReferenceProvider), obj39);
			Type typeFromHandle46 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver23 = new XmlNamespaceResolver();
			xmlNamespaceResolver23.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver23.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver23.Add("coding", "clr-namespace:CarScannerXamarinForms.Coding");
			xmlNamespaceResolver23.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver23.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver23.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver23.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver23.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xmlNamespaceResolver23.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver23.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider23.Add(typeFromHandle46, new XamlTypeResolver(xmlNamespaceResolver23, typeof(UniversalCodingPageV2).GetTypeInfo().Assembly));
			xamlServiceProvider23.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(135, 25)));
			DynamicResource dynamicResource5 = markupExtension23.ProvideValue(xamlServiceProvider23);
			buttonCell2.SetDynamicResource(CellBase.TitleColorProperty, dynamicResource5.Key);
			section5.Add(buttonCell2);
			settingsView.Root.Add(section5);
			translate11.Text = "coding_VagOpenHoodWarning";
			IMarkupExtension markupExtension24 = translate11;
			XamlServiceProvider xamlServiceProvider24 = new XamlServiceProvider();
			Type typeFromHandle47 = typeof(IProvideValueTarget);
			object[] array25 = new object[0 + 5];
			array25[0] = labelCell2;
			array25[1] = section6;
			array25[2] = settingsView;
			array25[3] = grid;
			array25[4] = this;
			object obj40;
			xamlServiceProvider24.Add(typeFromHandle47, obj40 = new SimpleValueTargetProvider(array25, CellBase.TitleProperty, nameScope));
			xamlServiceProvider24.Add(typeof(IReferenceProvider), obj40);
			Type typeFromHandle48 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver24 = new XmlNamespaceResolver();
			xmlNamespaceResolver24.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver24.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver24.Add("coding", "clr-namespace:CarScannerXamarinForms.Coding");
			xmlNamespaceResolver24.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver24.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver24.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver24.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver24.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xmlNamespaceResolver24.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver24.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider24.Add(typeFromHandle48, new XamlTypeResolver(xmlNamespaceResolver24, typeof(UniversalCodingPageV2).GetTypeInfo().Assembly));
			xamlServiceProvider24.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(140, 25)));
			object obj41 = markupExtension24.ProvideValue(xamlServiceProvider24);
			labelCell2.Title = obj41;
			labelCell2.SetValue(BindableObject.BindingContextProperty, sharedSettings);
			staticResourceExtension7.Key = "VagCodingPlatformToTrueConverter";
			IMarkupExtension markupExtension25 = staticResourceExtension7;
			XamlServiceProvider xamlServiceProvider25 = new XamlServiceProvider();
			Type typeFromHandle49 = typeof(IProvideValueTarget);
			object[] array26 = new object[0 + 6];
			array26[0] = bindingExtension17;
			array26[1] = labelCell2;
			array26[2] = section6;
			array26[3] = settingsView;
			array26[4] = grid;
			array26[5] = this;
			object obj42;
			xamlServiceProvider25.Add(typeFromHandle49, obj42 = new SimpleValueTargetProvider(array26, typeof(BindingExtension).GetRuntimeProperty("Converter"), nameScope));
			xamlServiceProvider25.Add(typeof(IReferenceProvider), obj42);
			Type typeFromHandle50 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver25 = new XmlNamespaceResolver();
			xmlNamespaceResolver25.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver25.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver25.Add("coding", "clr-namespace:CarScannerXamarinForms.Coding");
			xmlNamespaceResolver25.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver25.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver25.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver25.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver25.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xmlNamespaceResolver25.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver25.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider25.Add(typeFromHandle50, new XamlTypeResolver(xmlNamespaceResolver25, typeof(UniversalCodingPageV2).GetTypeInfo().Assembly));
			xamlServiceProvider25.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(142, 25)));
			object obj43 = markupExtension25.ProvideValue(xamlServiceProvider25);
			bindingExtension17.Converter = obj43;
			bindingExtension17.Path = "CodingLastPlatformSelected";
			BindingBase bindingBase17 = bindingExtension17.ProvideValue(null);
			labelCell2.SetBinding(CellBase.IsVisibleProperty, bindingBase17);
			labelCell2.SetValue(CellBase.TitleFontAttributesProperty, new FontAttributes?(1));
			section6.Add(labelCell2);
			translate12.Text = "coding_UseHistory";
			IMarkupExtension markupExtension26 = translate12;
			XamlServiceProvider xamlServiceProvider26 = new XamlServiceProvider();
			Type typeFromHandle51 = typeof(IProvideValueTarget);
			object[] array27 = new object[0 + 5];
			array27[0] = labelCell3;
			array27[1] = section6;
			array27[2] = settingsView;
			array27[3] = grid;
			array27[4] = this;
			object obj44;
			xamlServiceProvider26.Add(typeFromHandle51, obj44 = new SimpleValueTargetProvider(array27, CellBase.TitleProperty, nameScope));
			xamlServiceProvider26.Add(typeof(IReferenceProvider), obj44);
			Type typeFromHandle52 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver26 = new XmlNamespaceResolver();
			xmlNamespaceResolver26.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver26.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver26.Add("coding", "clr-namespace:CarScannerXamarinForms.Coding");
			xmlNamespaceResolver26.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver26.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver26.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver26.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver26.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xmlNamespaceResolver26.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver26.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider26.Add(typeFromHandle52, new XamlTypeResolver(xmlNamespaceResolver26, typeof(UniversalCodingPageV2).GetTypeInfo().Assembly));
			xamlServiceProvider26.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(144, 35)));
			object obj45 = markupExtension26.ProvideValue(xamlServiceProvider26);
			labelCell3.Title = obj45;
			section6.Add(labelCell3);
			settingsView.Root.Add(section6);
			grid.Children.Add(settingsView);
			activityFrame.SetValue(Grid.RowProperty, 0);
			activityFrame.SetValue(VisualElement.IsVisibleProperty, new VisualElement.VisibilityConverter().ConvertFromInvariantString("False"));
			grid.Children.Add(activityFrame);
			this.SetValue(ContentPage.ContentProperty, grid);
		}

		// Token: 0x060050B3 RID: 20659 RVA: 0x003EB04C File Offset: 0x003E924C
		[CompilerGenerated]
		private void <CodingDetailsPage_Appearing>b__2_0(string s)
		{
			MainThread.BeginInvokeOnMainThread(delegate
			{
				this.activityFrame.Text = ActivityFrame.PLEASE_WAIT_TEXT + "\n" + s;
			});
		}

		// Token: 0x060050B4 RID: 20660 RVA: 0x003EB071 File Offset: 0x003E9271
		[CompilerGenerated]
		private void <UpdateState>b__3_0(string s)
		{
			MainThread.BeginInvokeOnMainThread(delegate
			{
				this.activityFrame.Text = ActivityFrame.PLEASE_WAIT_TEXT + "\n" + s;
			});
		}

		// Token: 0x060050B5 RID: 20661 RVA: 0x003EB096 File Offset: 0x003E9296
		[CompilerGenerated]
		private void <optionCell_Tapped>b__6_0(string s)
		{
			Device.BeginInvokeOnMainThread(delegate
			{
				this.activityFrame.Text = s;
			});
		}

		// Token: 0x060050B6 RID: 20662 RVA: 0x003EB0BB File Offset: 0x003E92BB
		[CompilerGenerated]
		private void <btnSaveState_Clicked>b__7_0(string s)
		{
			Device.BeginInvokeOnMainThread(delegate
			{
				this.activityFrame.Text = s;
			});
		}

		// Token: 0x060050B7 RID: 20663 RVA: 0x003EB0E0 File Offset: 0x003E92E0
		private void __InitComponentRuntime()
		{
			Extensions.LoadFromXaml<UniversalCodingPageV2>(this, typeof(UniversalCodingPageV2));
			this.page = NameScopeExtensions.FindByName<ContentPage>(this, "page");
			this.lv = NameScopeExtensions.FindByName<SettingsView>(this, "lv");
			this.entryPassword = NameScopeExtensions.FindByName<Entry>(this, "entryPassword");
			this.btnUpdateState = NameScopeExtensions.FindByName<ButtonCell>(this, "btnUpdateState");
			this.optionsPanel = NameScopeExtensions.FindByName<Section>(this, "optionsPanel");
			this.inputPanel = NameScopeExtensions.FindByName<Section>(this, "inputPanel");
			this.entryValue = NameScopeExtensions.FindByName<Entry>(this, "entryValue");
			this.btnSaveNewValue = NameScopeExtensions.FindByName<ButtonCell>(this, "btnSaveNewValue");
			this.activityFrame = NameScopeExtensions.FindByName<ActivityFrame>(this, "activityFrame");
		}

		// Token: 0x0400309A RID: 12442
		private bool first_appearing = true;

		// Token: 0x0400309B RID: 12443
		private ICodingContainer coding;

		// Token: 0x0400309C RID: 12444
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private ContentPage page;

		// Token: 0x0400309D RID: 12445
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private SettingsView lv;

		// Token: 0x0400309E RID: 12446
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private Entry entryPassword;

		// Token: 0x0400309F RID: 12447
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private ButtonCell btnUpdateState;

		// Token: 0x040030A0 RID: 12448
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private Section optionsPanel;

		// Token: 0x040030A1 RID: 12449
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private Section inputPanel;

		// Token: 0x040030A2 RID: 12450
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private Entry entryValue;

		// Token: 0x040030A3 RID: 12451
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private ButtonCell btnSaveNewValue;

		// Token: 0x040030A4 RID: 12452
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private ActivityFrame activityFrame;

		// Token: 0x020009A7 RID: 2471
		[CompilerGenerated]
		private sealed class <>c__DisplayClass2_0
		{
			// Token: 0x060050B8 RID: 20664 RVA: 0x00002050 File Offset: 0x00000250
			public <>c__DisplayClass2_0()
			{
			}

			// Token: 0x060050B9 RID: 20665 RVA: 0x003EB197 File Offset: 0x003E9397
			internal void <CodingDetailsPage_Appearing>b__1()
			{
				this.<>4__this.activityFrame.Text = ActivityFrame.PLEASE_WAIT_TEXT + "\n" + this.s;
			}

			// Token: 0x040030A5 RID: 12453
			public string s;

			// Token: 0x040030A6 RID: 12454
			public UniversalCodingPageV2 <>4__this;
		}

		// Token: 0x020009A8 RID: 2472
		[CompilerGenerated]
		private sealed class <>c__DisplayClass3_0
		{
			// Token: 0x060050BA RID: 20666 RVA: 0x00002050 File Offset: 0x00000250
			public <>c__DisplayClass3_0()
			{
			}

			// Token: 0x060050BB RID: 20667 RVA: 0x003EB1BE File Offset: 0x003E93BE
			internal void <UpdateState>b__1()
			{
				this.<>4__this.activityFrame.Text = ActivityFrame.PLEASE_WAIT_TEXT + "\n" + this.s;
			}

			// Token: 0x040030A7 RID: 12455
			public string s;

			// Token: 0x040030A8 RID: 12456
			public UniversalCodingPageV2 <>4__this;
		}

		// Token: 0x020009A9 RID: 2473
		[CompilerGenerated]
		private sealed class <>c__DisplayClass6_0
		{
			// Token: 0x060050BC RID: 20668 RVA: 0x00002050 File Offset: 0x00000250
			public <>c__DisplayClass6_0()
			{
			}

			// Token: 0x060050BD RID: 20669 RVA: 0x003EB1E5 File Offset: 0x003E93E5
			internal void <optionCell_Tapped>b__1()
			{
				this.<>4__this.activityFrame.Text = this.s;
			}

			// Token: 0x040030A9 RID: 12457
			public string s;

			// Token: 0x040030AA RID: 12458
			public UniversalCodingPageV2 <>4__this;
		}

		// Token: 0x020009AA RID: 2474
		[CompilerGenerated]
		private sealed class <>c__DisplayClass7_0
		{
			// Token: 0x060050BE RID: 20670 RVA: 0x00002050 File Offset: 0x00000250
			public <>c__DisplayClass7_0()
			{
			}

			// Token: 0x060050BF RID: 20671 RVA: 0x003EB1FD File Offset: 0x003E93FD
			internal void <btnSaveState_Clicked>b__1()
			{
				this.<>4__this.activityFrame.Text = this.s;
			}

			// Token: 0x040030AB RID: 12459
			public string s;

			// Token: 0x040030AC RID: 12460
			public UniversalCodingPageV2 <>4__this;
		}

		// Token: 0x020009AB RID: 2475
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <BtnUpdateState_Clicked>d__4 : IAsyncStateMachine
		{
			// Token: 0x060050C0 RID: 20672 RVA: 0x003EB218 File Offset: 0x003E9418
			void IAsyncStateMachine.MoveNext()
			{
				UniversalCodingPageV2 universalCodingPageV = this;
				try
				{
					universalCodingPageV.UpdateState();
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

			// Token: 0x060050C1 RID: 20673 RVA: 0x003EB270 File Offset: 0x003E9470
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x040030AD RID: 12461
			public int <>1__state;

			// Token: 0x040030AE RID: 12462
			public AsyncVoidMethodBuilder <>t__builder;

			// Token: 0x040030AF RID: 12463
			public UniversalCodingPageV2 <>4__this;
		}

		// Token: 0x020009AC RID: 2476
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <CodingDetailsPage_Appearing>d__2 : IAsyncStateMachine
		{
			// Token: 0x060050C2 RID: 20674 RVA: 0x003EB280 File Offset: 0x003E9480
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				UniversalCodingPageV2 universalCodingPageV = this;
				try
				{
					TaskAwaiter<CodingRequestResult> taskAwaiter;
					TaskAwaiter<CodingRequestResult> taskAwaiter2;
					if (num != 0)
					{
						if (num != 1)
						{
							if (!universalCodingPageV.first_appearing)
							{
								goto IL_015E;
							}
							universalCodingPageV.first_appearing = false;
							universalCodingPageV.lv.IsEnabled = false;
							universalCodingPageV.activityFrame.IsVisible = true;
							Progress<string> progress = new Progress<string>(delegate(string s)
							{
								MainThread.BeginInvokeOnMainThread(new Action(new UniversalCodingPageV2.<>c__DisplayClass2_0
								{
									<>4__this = universalCodingPageV,
									s = s
								}.<CodingDetailsPage_Appearing>b__1));
							});
							if (App.OBDSimulator.IsActive)
							{
								goto IL_0146;
							}
							if (universalCodingPageV.coding is MQBAdaptationTemplate)
							{
								taskAwaiter = universalCodingPageV.coding.UpdateCurrentState("", progress).GetAwaiter();
								if (!taskAwaiter.IsCompleted)
								{
									num2 = 0;
									taskAwaiter2 = taskAwaiter;
									this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<CodingRequestResult>, UniversalCodingPageV2.<CodingDetailsPage_Appearing>d__2>(ref taskAwaiter, ref this);
									return;
								}
								goto IL_00D2;
							}
							else
							{
								taskAwaiter = universalCodingPageV.coding.UpdateCurrentState(universalCodingPageV.entryPassword.Text, progress).GetAwaiter();
								if (!taskAwaiter.IsCompleted)
								{
									num2 = 1;
									taskAwaiter2 = taskAwaiter;
									this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<CodingRequestResult>, UniversalCodingPageV2.<CodingDetailsPage_Appearing>d__2>(ref taskAwaiter, ref this);
									return;
								}
							}
						}
						else
						{
							taskAwaiter = taskAwaiter2;
							taskAwaiter2 = default(TaskAwaiter<CodingRequestResult>);
							num2 = -1;
						}
						taskAwaiter.GetResult();
						goto IL_0146;
					}
					taskAwaiter = taskAwaiter2;
					taskAwaiter2 = default(TaskAwaiter<CodingRequestResult>);
					num2 = -1;
					IL_00D2:
					taskAwaiter.GetResult();
					IL_0146:
					universalCodingPageV.activityFrame.IsVisible = false;
					universalCodingPageV.lv.IsEnabled = true;
					IL_015E:;
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

			// Token: 0x060050C3 RID: 20675 RVA: 0x003EB438 File Offset: 0x003E9638
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x040030B0 RID: 12464
			public int <>1__state;

			// Token: 0x040030B1 RID: 12465
			public AsyncVoidMethodBuilder <>t__builder;

			// Token: 0x040030B2 RID: 12466
			public UniversalCodingPageV2 <>4__this;

			// Token: 0x040030B3 RID: 12467
			private TaskAwaiter<CodingRequestResult> <>u__1;
		}

		// Token: 0x020009AD RID: 2477
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <UpdateState>d__3 : IAsyncStateMachine
		{
			// Token: 0x060050C4 RID: 20676 RVA: 0x003EB448 File Offset: 0x003E9648
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				UniversalCodingPageV2 universalCodingPageV = this;
				try
				{
					TaskAwaiter<CodingRequestResult> taskAwaiter;
					TaskAwaiter<CodingRequestResult> taskAwaiter2;
					if (num != 0)
					{
						if (num != 1)
						{
							universalCodingPageV.activityFrame.Text = ActivityFrame.PLEASE_WAIT_TEXT;
							universalCodingPageV.activityFrame.IsVisible = true;
							universalCodingPageV.lv.IsEnabled = false;
							Progress<string> progress = new Progress<string>(delegate(string s)
							{
								MainThread.BeginInvokeOnMainThread(new Action(new UniversalCodingPageV2.<>c__DisplayClass3_0
								{
									<>4__this = universalCodingPageV,
									s = s
								}.<UpdateState>b__1));
							});
							if (App.OBDSimulator.IsActive)
							{
								goto IL_0144;
							}
							if (universalCodingPageV.coding is MQBAdaptationTemplate)
							{
								taskAwaiter = universalCodingPageV.coding.UpdateCurrentState("", progress).GetAwaiter();
								if (!taskAwaiter.IsCompleted)
								{
									num2 = 0;
									taskAwaiter2 = taskAwaiter;
									this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<CodingRequestResult>, UniversalCodingPageV2.<UpdateState>d__3>(ref taskAwaiter, ref this);
									return;
								}
								goto IL_00D0;
							}
							else
							{
								taskAwaiter = universalCodingPageV.coding.UpdateCurrentState(universalCodingPageV.entryPassword.Text, progress).GetAwaiter();
								if (!taskAwaiter.IsCompleted)
								{
									num2 = 1;
									taskAwaiter2 = taskAwaiter;
									this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<CodingRequestResult>, UniversalCodingPageV2.<UpdateState>d__3>(ref taskAwaiter, ref this);
									return;
								}
							}
						}
						else
						{
							taskAwaiter = taskAwaiter2;
							taskAwaiter2 = default(TaskAwaiter<CodingRequestResult>);
							num2 = -1;
						}
						taskAwaiter.GetResult();
						goto IL_0144;
					}
					taskAwaiter = taskAwaiter2;
					taskAwaiter2 = default(TaskAwaiter<CodingRequestResult>);
					num2 = -1;
					IL_00D0:
					taskAwaiter.GetResult();
					IL_0144:
					universalCodingPageV.activityFrame.IsVisible = false;
					universalCodingPageV.lv.IsEnabled = true;
					universalCodingPageV.activityFrame.Text = ActivityFrame.PLEASE_WAIT_TEXT;
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

			// Token: 0x060050C5 RID: 20677 RVA: 0x003EB60C File Offset: 0x003E980C
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x040030B4 RID: 12468
			public int <>1__state;

			// Token: 0x040030B5 RID: 12469
			public AsyncTaskMethodBuilder <>t__builder;

			// Token: 0x040030B6 RID: 12470
			public UniversalCodingPageV2 <>4__this;

			// Token: 0x040030B7 RID: 12471
			private TaskAwaiter<CodingRequestResult> <>u__1;
		}

		// Token: 0x020009AE RID: 2478
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <btnSaveState_Clicked>d__7 : IAsyncStateMachine
		{
			// Token: 0x060050C6 RID: 20678 RVA: 0x003EB61C File Offset: 0x003E981C
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				UniversalCodingPageV2 universalCodingPageV = this;
				try
				{
					TaskAwaiter<bool> taskAwaiter3;
					TaskAwaiter taskAwaiter4;
					TaskAwaiter<CodingRequestResult> taskAwaiter6;
					switch (num)
					{
					case 0:
						taskAwaiter3 = taskAwaiter2;
						taskAwaiter2 = default(TaskAwaiter<bool>);
						num2 = -1;
						break;
					case 1:
					{
						TaskAwaiter taskAwaiter5;
						taskAwaiter4 = taskAwaiter5;
						taskAwaiter5 = default(TaskAwaiter);
						num2 = -1;
						goto IL_0168;
					}
					case 2:
						taskAwaiter3 = taskAwaiter2;
						taskAwaiter2 = default(TaskAwaiter<bool>);
						num2 = -1;
						goto IL_0222;
					case 3:
					{
						TaskAwaiter taskAwaiter5;
						taskAwaiter4 = taskAwaiter5;
						taskAwaiter5 = default(TaskAwaiter);
						num2 = -1;
						goto IL_0290;
					}
					case 4:
					{
						TaskAwaiter<CodingRequestResult> taskAwaiter7;
						taskAwaiter6 = taskAwaiter7;
						taskAwaiter7 = default(TaskAwaiter<CodingRequestResult>);
						num2 = -1;
						goto IL_0346;
					}
					case 5:
					{
						TaskAwaiter taskAwaiter5;
						taskAwaiter4 = taskAwaiter5;
						taskAwaiter5 = default(TaskAwaiter);
						num2 = -1;
						goto IL_03D8;
					}
					case 6:
					{
						TaskAwaiter<CodingRequestResult> taskAwaiter7;
						taskAwaiter6 = taskAwaiter7;
						taskAwaiter7 = default(TaskAwaiter<CodingRequestResult>);
						num2 = -1;
						goto IL_0441;
					}
					case 7:
					{
						TaskAwaiter taskAwaiter5;
						taskAwaiter4 = taskAwaiter5;
						taskAwaiter5 = default(TaskAwaiter);
						num2 = -1;
						goto IL_04FF;
					}
					default:
					{
						double num3;
						if (double.TryParse(universalCodingPageV.entryValue.Text, out num3) || universalCodingPageV.coding is MQBMode22ParametrizeSilentDump || universalCodingPageV.coding.ValueType == AdaptationValueTypes.InputTextType)
						{
							if (universalCodingPageV.coding.RequiresPro && !SharedSettings.Current.AdsProductPurchased)
							{
								taskAwaiter3 = universalCodingPageV.DisplayAlert("Car Scanner Pro", Translate.GetString("FeatureLocked"), "OK", Translate.GetString("btnCancel.Content")).GetAwaiter();
								if (!taskAwaiter3.IsCompleted)
								{
									num2 = 0;
									taskAwaiter2 = taskAwaiter3;
									this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<bool>, UniversalCodingPageV2.<btnSaveState_Clicked>d__7>(ref taskAwaiter3, ref this);
									return;
								}
							}
							else if (SharedSettings.Current.CodingsCounter >= 3 && !SharedSettings.Current.AdsProductPurchased)
							{
								taskAwaiter3 = universalCodingPageV.DisplayAlert("Car Scanner Pro", string.Format(Translate.GetString("coding_FreeCodingsFinished"), SharedSettings.Current.CodingsCounter, 3), "OK", Translate.GetString("btnCancel.Content")).GetAwaiter();
								if (!taskAwaiter3.IsCompleted)
								{
									num2 = 2;
									taskAwaiter2 = taskAwaiter3;
									this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<bool>, UniversalCodingPageV2.<btnSaveState_Clicked>d__7>(ref taskAwaiter3, ref this);
									return;
								}
								goto IL_0222;
							}
							else
							{
								universalCodingPageV.activityFrame.IsVisible = true;
								universalCodingPageV.entryValue.IsEnabled = false;
								Progress<string> progress = new Progress<string>(delegate(string s)
								{
									Device.BeginInvokeOnMainThread(new Action(new UniversalCodingPageV2.<>c__DisplayClass7_0
									{
										<>4__this = universalCodingPageV,
										s = s
									}.<btnSaveState_Clicked>b__1));
								});
								taskAwaiter6 = universalCodingPageV.coding.Execute(universalCodingPageV.entryPassword.Text, universalCodingPageV.entryValue.Text, universalCodingPageV.entryValue.Text, progress, null, false).GetAwaiter();
								if (!taskAwaiter6.IsCompleted)
								{
									num2 = 4;
									TaskAwaiter<CodingRequestResult> taskAwaiter7 = taskAwaiter6;
									this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<CodingRequestResult>, UniversalCodingPageV2.<btnSaveState_Clicked>d__7>(ref taskAwaiter6, ref this);
									return;
								}
								goto IL_0346;
							}
						}
						else
						{
							taskAwaiter4 = universalCodingPageV.DisplayAlert(Translate.GetString("coding_WrongInputFormatTitle"), Translate.GetString("coding_WrongInputFormatText") + "\n" + 123.45.ToString(), "OK").GetAwaiter();
							if (!taskAwaiter4.IsCompleted)
							{
								num2 = 7;
								TaskAwaiter taskAwaiter5 = taskAwaiter4;
								this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, UniversalCodingPageV2.<btnSaveState_Clicked>d__7>(ref taskAwaiter4, ref this);
								return;
							}
							goto IL_04FF;
						}
						break;
					}
					}
					if (!taskAwaiter3.GetResult())
					{
						goto IL_016F;
					}
					Page inAppPage = InAppManager.GetInAppPage();
					taskAwaiter4 = universalCodingPageV.Navigation.PushAsync(inAppPage).GetAwaiter();
					if (!taskAwaiter4.IsCompleted)
					{
						num2 = 1;
						TaskAwaiter taskAwaiter5 = taskAwaiter4;
						this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, UniversalCodingPageV2.<btnSaveState_Clicked>d__7>(ref taskAwaiter4, ref this);
						return;
					}
					IL_0168:
					taskAwaiter4.GetResult();
					IL_016F:
					goto IL_0521;
					IL_0222:
					if (!taskAwaiter3.GetResult())
					{
						goto IL_0297;
					}
					Page inAppPage2 = InAppManager.GetInAppPage();
					taskAwaiter4 = universalCodingPageV.Navigation.PushAsync(inAppPage2).GetAwaiter();
					if (!taskAwaiter4.IsCompleted)
					{
						num2 = 3;
						TaskAwaiter taskAwaiter5 = taskAwaiter4;
						this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, UniversalCodingPageV2.<btnSaveState_Clicked>d__7>(ref taskAwaiter4, ref this);
						return;
					}
					IL_0290:
					taskAwaiter4.GetResult();
					IL_0297:
					goto IL_0521;
					IL_0346:
					CodingRequestResult result = taskAwaiter6.GetResult();
					if (result == CodingRequestResult.Success)
					{
						SharedSettings sharedSettings = SharedSettings.Current;
						int codingsCounter = sharedSettings.CodingsCounter;
						sharedSettings.CodingsCounter = codingsCounter + 1;
						goto IL_03DF;
					}
					taskAwaiter4 = universalCodingPageV.DisplayAlert(Translate.GetString("coding_Error"), MQBAdaptationTemplate.CodingRequestResultToString(result), "OK").GetAwaiter();
					if (!taskAwaiter4.IsCompleted)
					{
						num2 = 5;
						TaskAwaiter taskAwaiter5 = taskAwaiter4;
						this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, UniversalCodingPageV2.<btnSaveState_Clicked>d__7>(ref taskAwaiter4, ref this);
						return;
					}
					IL_03D8:
					taskAwaiter4.GetResult();
					IL_03DF:
					taskAwaiter6 = universalCodingPageV.coding.UpdateCurrentState("", null).GetAwaiter();
					if (!taskAwaiter6.IsCompleted)
					{
						num2 = 6;
						TaskAwaiter<CodingRequestResult> taskAwaiter7 = taskAwaiter6;
						this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<CodingRequestResult>, UniversalCodingPageV2.<btnSaveState_Clicked>d__7>(ref taskAwaiter6, ref this);
						return;
					}
					IL_0441:
					taskAwaiter6.GetResult();
					universalCodingPageV.activityFrame.IsVisible = false;
					universalCodingPageV.activityFrame.Text = ActivityFrame.PLEASE_WAIT_TEXT;
					universalCodingPageV.entryValue.IsEnabled = true;
					goto IL_0506;
					IL_04FF:
					taskAwaiter4.GetResult();
					IL_0506:;
				}
				catch (Exception ex)
				{
					num2 = -2;
					this.<>t__builder.SetException(ex);
					return;
				}
				IL_0521:
				num2 = -2;
				this.<>t__builder.SetResult();
			}

			// Token: 0x060050C7 RID: 20679 RVA: 0x003EBB7C File Offset: 0x003E9D7C
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x040030B8 RID: 12472
			public int <>1__state;

			// Token: 0x040030B9 RID: 12473
			public AsyncVoidMethodBuilder <>t__builder;

			// Token: 0x040030BA RID: 12474
			public UniversalCodingPageV2 <>4__this;

			// Token: 0x040030BB RID: 12475
			private TaskAwaiter<bool> <>u__1;

			// Token: 0x040030BC RID: 12476
			private TaskAwaiter <>u__2;

			// Token: 0x040030BD RID: 12477
			private TaskAwaiter<CodingRequestResult> <>u__3;
		}

		// Token: 0x020009AF RID: 2479
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <optionCell_Tapped>d__6 : IAsyncStateMachine
		{
			// Token: 0x060050C8 RID: 20680 RVA: 0x003EBB8C File Offset: 0x003E9D8C
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				UniversalCodingPageV2 universalCodingPageV = this;
				try
				{
					TaskAwaiter<bool> taskAwaiter3;
					TaskAwaiter taskAwaiter4;
					TaskAwaiter<CodingRequestResult> taskAwaiter6;
					switch (num)
					{
					case 0:
						taskAwaiter3 = taskAwaiter2;
						taskAwaiter2 = default(TaskAwaiter<bool>);
						num2 = -1;
						break;
					case 1:
					{
						TaskAwaiter taskAwaiter5;
						taskAwaiter4 = taskAwaiter5;
						taskAwaiter5 = default(TaskAwaiter);
						num2 = -1;
						goto IL_014C;
					}
					case 2:
						taskAwaiter3 = taskAwaiter2;
						taskAwaiter2 = default(TaskAwaiter<bool>);
						num2 = -1;
						goto IL_0206;
					case 3:
					{
						TaskAwaiter taskAwaiter5;
						taskAwaiter4 = taskAwaiter5;
						taskAwaiter5 = default(TaskAwaiter);
						num2 = -1;
						goto IL_0274;
					}
					case 4:
					{
						TaskAwaiter<CodingRequestResult> taskAwaiter7;
						taskAwaiter6 = taskAwaiter7;
						taskAwaiter7 = default(TaskAwaiter<CodingRequestResult>);
						num2 = -1;
						goto IL_032A;
					}
					case 5:
					{
						TaskAwaiter taskAwaiter5;
						taskAwaiter4 = taskAwaiter5;
						taskAwaiter5 = default(TaskAwaiter);
						num2 = -1;
						goto IL_03CF;
					}
					case 6:
					{
						TaskAwaiter taskAwaiter5;
						taskAwaiter4 = taskAwaiter5;
						taskAwaiter5 = default(TaskAwaiter);
						num2 = -1;
						goto IL_0444;
					}
					case 7:
					{
						TaskAwaiter<CodingRequestResult> taskAwaiter7;
						taskAwaiter6 = taskAwaiter7;
						taskAwaiter7 = default(TaskAwaiter<CodingRequestResult>);
						num2 = -1;
						goto IL_04B2;
					}
					default:
					{
						MQBAdaptationOption mqbadaptationOption = (MQBAdaptationOption)((Element)sender).BindingContext;
						if (universalCodingPageV.coding.RequiresPro && !SharedSettings.Current.AdsProductPurchased)
						{
							taskAwaiter3 = universalCodingPageV.DisplayAlert("Car Scanner Pro", Translate.GetString("FeatureLocked"), "OK", Translate.GetString("btnCancel.Content")).GetAwaiter();
							if (!taskAwaiter3.IsCompleted)
							{
								num2 = 0;
								taskAwaiter2 = taskAwaiter3;
								this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<bool>, UniversalCodingPageV2.<optionCell_Tapped>d__6>(ref taskAwaiter3, ref this);
								return;
							}
						}
						else if (SharedSettings.Current.CodingsCounter >= 3 && !SharedSettings.Current.AdsProductPurchased)
						{
							taskAwaiter3 = universalCodingPageV.DisplayAlert("Car Scanner Pro", string.Format(Translate.GetString("coding_FreeCodingsFinished"), SharedSettings.Current.CodingsCounter, 3), "OK", Translate.GetString("btnCancel.Content")).GetAwaiter();
							if (!taskAwaiter3.IsCompleted)
							{
								num2 = 2;
								taskAwaiter2 = taskAwaiter3;
								this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<bool>, UniversalCodingPageV2.<optionCell_Tapped>d__6>(ref taskAwaiter3, ref this);
								return;
							}
							goto IL_0206;
						}
						else
						{
							universalCodingPageV.activityFrame.IsVisible = true;
							universalCodingPageV.lv.IsEnabled = false;
							progress = new Progress<string>(delegate(string s)
							{
								Device.BeginInvokeOnMainThread(new Action(new UniversalCodingPageV2.<>c__DisplayClass6_0
								{
									<>4__this = universalCodingPageV,
									s = s
								}.<optionCell_Tapped>b__1));
							});
							taskAwaiter6 = universalCodingPageV.coding.Execute(universalCodingPageV.entryPassword.Text, mqbadaptationOption.Value, mqbadaptationOption.Title, progress, null, false).GetAwaiter();
							if (!taskAwaiter6.IsCompleted)
							{
								num2 = 4;
								TaskAwaiter<CodingRequestResult> taskAwaiter7 = taskAwaiter6;
								this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<CodingRequestResult>, UniversalCodingPageV2.<optionCell_Tapped>d__6>(ref taskAwaiter6, ref this);
								return;
							}
							goto IL_032A;
						}
						break;
					}
					}
					if (!taskAwaiter3.GetResult())
					{
						goto IL_0153;
					}
					Page inAppPage = InAppManager.GetInAppPage();
					taskAwaiter4 = universalCodingPageV.Navigation.PushAsync(inAppPage).GetAwaiter();
					if (!taskAwaiter4.IsCompleted)
					{
						num2 = 1;
						TaskAwaiter taskAwaiter5 = taskAwaiter4;
						this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, UniversalCodingPageV2.<optionCell_Tapped>d__6>(ref taskAwaiter4, ref this);
						return;
					}
					IL_014C:
					taskAwaiter4.GetResult();
					IL_0153:
					goto IL_0504;
					IL_0206:
					if (!taskAwaiter3.GetResult())
					{
						goto IL_027B;
					}
					Page inAppPage2 = InAppManager.GetInAppPage();
					taskAwaiter4 = universalCodingPageV.Navigation.PushAsync(inAppPage2).GetAwaiter();
					if (!taskAwaiter4.IsCompleted)
					{
						num2 = 3;
						TaskAwaiter taskAwaiter5 = taskAwaiter4;
						this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, UniversalCodingPageV2.<optionCell_Tapped>d__6>(ref taskAwaiter4, ref this);
						return;
					}
					IL_0274:
					taskAwaiter4.GetResult();
					IL_027B:
					goto IL_0504;
					IL_032A:
					CodingRequestResult result = taskAwaiter6.GetResult();
					if (result == CodingRequestResult.Success)
					{
						SharedSettings sharedSettings = SharedSettings.Current;
						int codingsCounter = sharedSettings.CodingsCounter;
						sharedSettings.CodingsCounter = codingsCounter + 1;
						if (!(universalCodingPageV.coding is IServiceProcedure))
						{
							goto IL_044B;
						}
						taskAwaiter4 = universalCodingPageV.DisplayAlert(universalCodingPageV.coding.Name, Translate.GetString("coding_OperationFinished"), "OK").GetAwaiter();
						if (!taskAwaiter4.IsCompleted)
						{
							num2 = 5;
							TaskAwaiter taskAwaiter5 = taskAwaiter4;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, UniversalCodingPageV2.<optionCell_Tapped>d__6>(ref taskAwaiter4, ref this);
							return;
						}
					}
					else
					{
						taskAwaiter4 = universalCodingPageV.DisplayAlert(Translate.GetString("coding_Error"), MQBAdaptationTemplate.CodingRequestResultToString(result), "OK").GetAwaiter();
						if (!taskAwaiter4.IsCompleted)
						{
							num2 = 6;
							TaskAwaiter taskAwaiter5 = taskAwaiter4;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, UniversalCodingPageV2.<optionCell_Tapped>d__6>(ref taskAwaiter4, ref this);
							return;
						}
						goto IL_0444;
					}
					IL_03CF:
					taskAwaiter4.GetResult();
					goto IL_044B;
					IL_0444:
					taskAwaiter4.GetResult();
					IL_044B:
					taskAwaiter6 = universalCodingPageV.coding.UpdateCurrentState("", progress).GetAwaiter();
					if (!taskAwaiter6.IsCompleted)
					{
						num2 = 7;
						TaskAwaiter<CodingRequestResult> taskAwaiter7 = taskAwaiter6;
						this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<CodingRequestResult>, UniversalCodingPageV2.<optionCell_Tapped>d__6>(ref taskAwaiter6, ref this);
						return;
					}
					IL_04B2:
					taskAwaiter6.GetResult();
					universalCodingPageV.activityFrame.IsVisible = false;
					universalCodingPageV.activityFrame.Text = ActivityFrame.PLEASE_WAIT_TEXT;
					universalCodingPageV.lv.IsEnabled = true;
				}
				catch (Exception ex)
				{
					num2 = -2;
					progress = null;
					this.<>t__builder.SetException(ex);
					return;
				}
				IL_0504:
				num2 = -2;
				progress = null;
				this.<>t__builder.SetResult();
			}

			// Token: 0x060050C9 RID: 20681 RVA: 0x003EC0D4 File Offset: 0x003EA2D4
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x040030BE RID: 12478
			public int <>1__state;

			// Token: 0x040030BF RID: 12479
			public AsyncVoidMethodBuilder <>t__builder;

			// Token: 0x040030C0 RID: 12480
			public object sender;

			// Token: 0x040030C1 RID: 12481
			public UniversalCodingPageV2 <>4__this;

			// Token: 0x040030C2 RID: 12482
			private Progress<string> <progress>5__2;

			// Token: 0x040030C3 RID: 12483
			private TaskAwaiter<bool> <>u__1;

			// Token: 0x040030C4 RID: 12484
			private TaskAwaiter <>u__2;

			// Token: 0x040030C5 RID: 12485
			private TaskAwaiter<CodingRequestResult> <>u__3;
		}

		// Token: 0x020009B0 RID: 2480
		[CompilerGenerated]
		private sealed class <InitializeComponent>_anonXamlCDataTemplate_5
		{
			// Token: 0x060050CA RID: 20682 RVA: 0x003EC0E4 File Offset: 0x003EA2E4
			public <InitializeComponent>_anonXamlCDataTemplate_5()
			{
			}

			// Token: 0x060050CB RID: 20683 RVA: 0x003EC0F8 File Offset: 0x003EA2F8
			internal object LoadDataTemplate()
			{
				BindingExtension bindingExtension;
				VisualDiagnostics.RegisterSourceInfo(bindingExtension = new BindingExtension(), new Uri("Coding\\PagesV2\\UniversalCodingPageV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 117, 33);
				DynamicResourceExtension dynamicResourceExtension;
				VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension = new DynamicResourceExtension(), new Uri("Coding\\PagesV2\\UniversalCodingPageV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 119, 33);
				ButtonCell buttonCell;
				VisualDiagnostics.RegisterSourceInfo(buttonCell = new ButtonCell(), new Uri("Coding\\PagesV2\\UniversalCodingPageV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 116, 30);
				NameScope nameScope = new NameScope();
				NameScope.SetNameScope(buttonCell, nameScope);
				bindingExtension.Path = "Title";
				BindingBase bindingBase = bindingExtension.ProvideValue(null);
				buttonCell.SetBinding(CellBase.TitleProperty, bindingBase);
				buttonCell.Tapped += this.root.optionCell_Tapped;
				dynamicResourceExtension.Key = "ButtonAccentColor";
				IMarkupExtension<DynamicResource> markupExtension = dynamicResourceExtension;
				XamlServiceProvider xamlServiceProvider = new XamlServiceProvider();
				Type typeFromHandle = typeof(IProvideValueTarget);
				int num;
				object[] array = new object[(num = this.parentValues.Length) + 1];
				Array.Copy(this.parentValues, 0, array, 1, num);
				object[] array2 = array;
				array2[0] = buttonCell;
				object obj;
				xamlServiceProvider.Add(typeFromHandle, obj = new SimpleValueTargetProvider(array2, CellBase.TitleColorProperty, nameScope));
				xamlServiceProvider.Add(typeof(IReferenceProvider), obj);
				Type typeFromHandle2 = typeof(IXamlTypeResolver);
				XmlNamespaceResolver xmlNamespaceResolver = new XmlNamespaceResolver();
				xmlNamespaceResolver.Add("", "http://xamarin.com/schemas/2014/forms");
				xmlNamespaceResolver.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
				xmlNamespaceResolver.Add("coding", "clr-namespace:CarScannerXamarinForms.Coding");
				xmlNamespaceResolver.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
				xmlNamespaceResolver.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
				xmlNamespaceResolver.Add("local", "clr-namespace:CarScannerXamarinForms");
				xmlNamespaceResolver.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
				xmlNamespaceResolver.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
				xmlNamespaceResolver.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
				xmlNamespaceResolver.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
				xamlServiceProvider.Add(typeFromHandle2, new XamlTypeResolver(xmlNamespaceResolver, typeof(UniversalCodingPageV2.<InitializeComponent>_anonXamlCDataTemplate_5).GetTypeInfo().Assembly));
				xamlServiceProvider.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(119, 33)));
				DynamicResource dynamicResource = markupExtension.ProvideValue(xamlServiceProvider);
				buttonCell.SetDynamicResource(CellBase.TitleColorProperty, dynamicResource.Key);
				return buttonCell;
			}

			// Token: 0x040030C6 RID: 12486
			internal object[] parentValues;

			// Token: 0x040030C7 RID: 12487
			internal UniversalCodingPageV2 root;
		}
	}
}
