using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Globalization;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using AiForms.Renderers;
using CarScannerXamarinForms.Coding.DB.MQB;
using CarScannerXamarinForms.Common.XAMLConverters;
using CarScannerXamarinForms.InApp;
using CarScannerXamarinForms.Settings;
using CarScannerXamarinForms.UserControls;
using Xamarin.CommunityToolkit.Behaviors;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Internals;
using Xamarin.Forms.PlatformConfiguration.iOSSpecific;
using Xamarin.Forms.Xaml;
using Xamarin.Forms.Xaml.Diagnostics;
using Xamarin.Forms.Xaml.Internals;

namespace CarScannerXamarinForms.Coding.PagesV2
{
	// Token: 0x02000980 RID: 2432
	[XamlCompilation(2)]
	[XamlFilePath("Coding\\PagesV2\\CodingA5CustomizationPage.xaml")]
	public class CodingA5CustomizationPage : ContentPage
	{
		// Token: 0x06004FE8 RID: 20456 RVA: 0x003D61C4 File Offset: 0x003D43C4
		public CodingA5CustomizationPage(ICodingContainer coding)
		{
			this.InitializeComponent();
			this.coding = (A5ParametrizeV2)coding;
			try
			{
				base.BindingContext = this.coding;
			}
			catch (Exception)
			{
			}
			base.Appearing += this.CodingDetailsPage_Appearing;
		}

		// Token: 0x06004FE9 RID: 20457 RVA: 0x003D6224 File Offset: 0x003D4424
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
					await this.coding.UpdateCurrentState("", progress);
				}
				this.activityFrame.IsVisible = false;
				this.lv.IsEnabled = true;
			}
		}

		// Token: 0x06004FEA RID: 20458 RVA: 0x003D625C File Offset: 0x003D445C
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
				await this.coding.UpdateCurrentState("", progress);
			}
			this.activityFrame.IsVisible = false;
			this.lv.IsEnabled = true;
			this.activityFrame.Text = ActivityFrame.PLEASE_WAIT_TEXT;
		}

		// Token: 0x06004FEB RID: 20459 RVA: 0x003D62A0 File Offset: 0x003D44A0
		private async void BtnUpdateState_Clicked(object sender, EventArgs e)
		{
			this.UpdateState();
		}

		// Token: 0x06004FEC RID: 20460 RVA: 0x003D62D8 File Offset: 0x003D44D8
		private async void optionCell_Tapped(object sender, EventArgs e)
		{
			if (this.coding.SelectedDataset == null)
			{
				await base.DisplayAlert("No dataset selected!", "", "OK");
			}
			else
			{
				int num;
				if (!int.TryParse(this.entryLaneAssistTimer.Text, NumberStyles.Integer, CultureInfo.InvariantCulture, out num) || num < 0 || num > 65535)
				{
					await base.DisplayAlert("Wrong Lane assist timer format", "Min. = 0, Max = 65535, format example: 17", "OK");
				}
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
		}

		// Token: 0x06004FED RID: 20461 RVA: 0x003D6318 File Offset: 0x003D4518
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private void InitializeComponent()
		{
			if (ResourceLoader.IsEnabled && ResourceLoader.CanProvideContentFor(new ResourceLoader.ResourceLoadingQuery
			{
				AssemblyName = typeof(CodingA5CustomizationPage).GetTypeInfo().Assembly.GetName(),
				ResourcePath = "Coding/PagesV2/CodingA5CustomizationPage.xaml",
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
			VisualDiagnostics.RegisterSourceInfo(translate = new Translate(), new Uri("Coding\\PagesV2\\CodingA5CustomizationPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 16, 5);
			DynamicResourceExtension dynamicResourceExtension;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension = new DynamicResourceExtension(), new Uri("Coding\\PagesV2\\CodingA5CustomizationPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 19, 5);
			EmptyStringToFalseConverter emptyStringToFalseConverter;
			VisualDiagnostics.RegisterSourceInfo(emptyStringToFalseConverter = new EmptyStringToFalseConverter(), new Uri("Coding\\PagesV2\\CodingA5CustomizationPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 23, 14);
			EmptyStringToTrueConverter emptyStringToTrueConverter;
			VisualDiagnostics.RegisterSourceInfo(emptyStringToTrueConverter = new EmptyStringToTrueConverter(), new Uri("Coding\\PagesV2\\CodingA5CustomizationPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 24, 14);
			IntMsToStringSecondsConverterWithNegativeWrongValue intMsToStringSecondsConverterWithNegativeWrongValue;
			VisualDiagnostics.RegisterSourceInfo(intMsToStringSecondsConverterWithNegativeWrongValue = new IntMsToStringSecondsConverterWithNegativeWrongValue(), new Uri("Coding\\PagesV2\\CodingA5CustomizationPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 25, 14);
			IntToStringConverter intToStringConverter;
			VisualDiagnostics.RegisterSourceInfo(intToStringConverter = new IntToStringConverter(), new Uri("Coding\\PagesV2\\CodingA5CustomizationPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 26, 14);
			ResourceDictionary resourceDictionary;
			VisualDiagnostics.RegisterSourceInfo(resourceDictionary = new ResourceDictionary(), new Uri("Coding\\PagesV2\\CodingA5CustomizationPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 22, 10);
			DynamicResourceExtension dynamicResourceExtension2;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension2 = new DynamicResourceExtension(), new Uri("Coding\\PagesV2\\CodingA5CustomizationPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 40, 29);
			BindingExtension bindingExtension;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension = new BindingExtension(), new Uri("Coding\\PagesV2\\CodingA5CustomizationPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 41, 29);
			DynamicResourceExtension dynamicResourceExtension3;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension3 = new DynamicResourceExtension(), new Uri("Coding\\PagesV2\\CodingA5CustomizationPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 42, 29);
			Label label;
			VisualDiagnostics.RegisterSourceInfo(label = new Label(), new Uri("Coding\\PagesV2\\CodingA5CustomizationPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 37, 26);
			StaticResourceExtension staticResourceExtension;
			VisualDiagnostics.RegisterSourceInfo(staticResourceExtension = new StaticResourceExtension(), new Uri("Coding\\PagesV2\\CodingA5CustomizationPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 44, 57);
			BindingExtension bindingExtension2;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension2 = new BindingExtension(), new Uri("Coding\\PagesV2\\CodingA5CustomizationPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 44, 57);
			BindingExtension bindingExtension3;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension3 = new BindingExtension(), new Uri("Coding\\PagesV2\\CodingA5CustomizationPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 45, 32);
			Label label2;
			VisualDiagnostics.RegisterSourceInfo(label2 = new Label(), new Uri("Coding\\PagesV2\\CodingA5CustomizationPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 45, 26);
			CustomCell customCell;
			VisualDiagnostics.RegisterSourceInfo(customCell = new CustomCell(), new Uri("Coding\\PagesV2\\CodingA5CustomizationPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 44, 22);
			StaticResourceExtension staticResourceExtension2;
			VisualDiagnostics.RegisterSourceInfo(staticResourceExtension2 = new StaticResourceExtension(), new Uri("Coding\\PagesV2\\CodingA5CustomizationPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 48, 57);
			BindingExtension bindingExtension4;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension4 = new BindingExtension(), new Uri("Coding\\PagesV2\\CodingA5CustomizationPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 48, 57);
			BindingExtension bindingExtension5;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension5 = new BindingExtension(), new Uri("Coding\\PagesV2\\CodingA5CustomizationPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 49, 32);
			Label label3;
			VisualDiagnostics.RegisterSourceInfo(label3 = new Label(), new Uri("Coding\\PagesV2\\CodingA5CustomizationPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 49, 26);
			CustomCell customCell2;
			VisualDiagnostics.RegisterSourceInfo(customCell2 = new CustomCell(), new Uri("Coding\\PagesV2\\CodingA5CustomizationPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 48, 22);
			Section section;
			VisualDiagnostics.RegisterSourceInfo(section = new Section(), new Uri("Coding\\PagesV2\\CodingA5CustomizationPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 35, 18);
			Translate translate2;
			VisualDiagnostics.RegisterSourceInfo(translate2 = new Translate(), new Uri("Coding\\PagesV2\\CodingA5CustomizationPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 52, 29);
			BindingExtension bindingExtension6;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension6 = new BindingExtension(), new Uri("Coding\\PagesV2\\CodingA5CustomizationPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 52, 76);
			BindingExtension bindingExtension7;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension7 = new BindingExtension(), new Uri("Coding\\PagesV2\\CodingA5CustomizationPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 54, 55);
			Entry entry;
			VisualDiagnostics.RegisterSourceInfo(entry = new Entry(), new Uri("Coding\\PagesV2\\CodingA5CustomizationPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 54, 26);
			CustomCell customCell3;
			VisualDiagnostics.RegisterSourceInfo(customCell3 = new CustomCell(), new Uri("Coding\\PagesV2\\CodingA5CustomizationPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 53, 22);
			StaticResourceExtension staticResourceExtension3;
			VisualDiagnostics.RegisterSourceInfo(staticResourceExtension3 = new StaticResourceExtension(), new Uri("Coding\\PagesV2\\CodingA5CustomizationPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 57, 57);
			BindingExtension bindingExtension8;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension8 = new BindingExtension(), new Uri("Coding\\PagesV2\\CodingA5CustomizationPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 57, 57);
			Translate translate3;
			VisualDiagnostics.RegisterSourceInfo(translate3 = new Translate(), new Uri("Coding\\PagesV2\\CodingA5CustomizationPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 61, 43);
			Span span;
			VisualDiagnostics.RegisterSourceInfo(span = new Span(), new Uri("Coding\\PagesV2\\CodingA5CustomizationPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 61, 38);
			BindingExtension bindingExtension9;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension9 = new BindingExtension(), new Uri("Coding\\PagesV2\\CodingA5CustomizationPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 62, 43);
			Span span2;
			VisualDiagnostics.RegisterSourceInfo(span2 = new Span(), new Uri("Coding\\PagesV2\\CodingA5CustomizationPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 62, 38);
			FormattedString formattedString;
			VisualDiagnostics.RegisterSourceInfo(formattedString = new FormattedString(), new Uri("Coding\\PagesV2\\CodingA5CustomizationPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 60, 34);
			Label label4;
			VisualDiagnostics.RegisterSourceInfo(label4 = new Label(), new Uri("Coding\\PagesV2\\CodingA5CustomizationPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 58, 26);
			CustomCell customCell4;
			VisualDiagnostics.RegisterSourceInfo(customCell4 = new CustomCell(), new Uri("Coding\\PagesV2\\CodingA5CustomizationPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 57, 22);
			StaticResourceExtension staticResourceExtension4;
			VisualDiagnostics.RegisterSourceInfo(staticResourceExtension4 = new StaticResourceExtension(), new Uri("Coding\\PagesV2\\CodingA5CustomizationPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 67, 57);
			BindingExtension bindingExtension10;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension10 = new BindingExtension(), new Uri("Coding\\PagesV2\\CodingA5CustomizationPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 67, 57);
			Translate translate4;
			VisualDiagnostics.RegisterSourceInfo(translate4 = new Translate(), new Uri("Coding\\PagesV2\\CodingA5CustomizationPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 68, 32);
			Label label5;
			VisualDiagnostics.RegisterSourceInfo(label5 = new Label(), new Uri("Coding\\PagesV2\\CodingA5CustomizationPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 68, 26);
			CustomCell customCell5;
			VisualDiagnostics.RegisterSourceInfo(customCell5 = new CustomCell(), new Uri("Coding\\PagesV2\\CodingA5CustomizationPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 67, 22);
			Section section2;
			VisualDiagnostics.RegisterSourceInfo(section2 = new Section(), new Uri("Coding\\PagesV2\\CodingA5CustomizationPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 52, 18);
			Translate translate5;
			VisualDiagnostics.RegisterSourceInfo(translate5 = new Translate(), new Uri("Coding\\PagesV2\\CodingA5CustomizationPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 72, 29);
			BindingExtension bindingExtension11;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension11 = new BindingExtension(), new Uri("Coding\\PagesV2\\CodingA5CustomizationPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 72, 80);
			BindingExtension bindingExtension12;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension12 = new BindingExtension(), new Uri("Coding\\PagesV2\\CodingA5CustomizationPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 74, 35);
			LabelCell labelCell;
			VisualDiagnostics.RegisterSourceInfo(labelCell = new LabelCell(), new Uri("Coding\\PagesV2\\CodingA5CustomizationPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 74, 22);
			Translate translate6;
			VisualDiagnostics.RegisterSourceInfo(translate6 = new Translate(), new Uri("Coding\\PagesV2\\CodingA5CustomizationPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 77, 25);
			DynamicResourceExtension dynamicResourceExtension4;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension4 = new DynamicResourceExtension(), new Uri("Coding\\PagesV2\\CodingA5CustomizationPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 79, 25);
			ButtonCell buttonCell;
			VisualDiagnostics.RegisterSourceInfo(buttonCell = new ButtonCell(), new Uri("Coding\\PagesV2\\CodingA5CustomizationPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 75, 22);
			Section section3;
			VisualDiagnostics.RegisterSourceInfo(section3 = new Section(), new Uri("Coding\\PagesV2\\CodingA5CustomizationPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 72, 18);
			Translate translate7;
			VisualDiagnostics.RegisterSourceInfo(translate7 = new Translate(), new Uri("Coding\\PagesV2\\CodingA5CustomizationPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 82, 29);
			StaticResourceExtension staticResourceExtension5;
			VisualDiagnostics.RegisterSourceInfo(staticResourceExtension5 = new StaticResourceExtension(), new Uri("Coding\\PagesV2\\CodingA5CustomizationPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 85, 36);
			BindingExtension bindingExtension13;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension13 = new BindingExtension(), new Uri("Coding\\PagesV2\\CodingA5CustomizationPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 85, 86);
			Label label6;
			VisualDiagnostics.RegisterSourceInfo(label6 = new Label(), new Uri("Coding\\PagesV2\\CodingA5CustomizationPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 85, 30);
			BindingExtension bindingExtension14;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension14 = new BindingExtension(), new Uri("Coding\\PagesV2\\CodingA5CustomizationPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 89, 33);
			BindingExtension bindingExtension15;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension15 = new BindingExtension(), new Uri("Coding\\PagesV2\\CodingA5CustomizationPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 90, 33);
			BindingExtension bindingExtension16;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension16 = new BindingExtension(), new Uri("Coding\\PagesV2\\CodingA5CustomizationPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 91, 33);
			Picker picker;
			VisualDiagnostics.RegisterSourceInfo(picker = new Picker(), new Uri("Coding\\PagesV2\\CodingA5CustomizationPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 87, 30);
			Grid grid;
			VisualDiagnostics.RegisterSourceInfo(grid = new Grid(), new Uri("Coding\\PagesV2\\CodingA5CustomizationPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 84, 26);
			SettingsCustomCellForPicker settingsCustomCellForPicker;
			VisualDiagnostics.RegisterSourceInfo(settingsCustomCellForPicker = new SettingsCustomCellForPicker(), new Uri("Coding\\PagesV2\\CodingA5CustomizationPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 83, 22);
			Section section4;
			VisualDiagnostics.RegisterSourceInfo(section4 = new Section(), new Uri("Coding\\PagesV2\\CodingA5CustomizationPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 82, 18);
			Translate translate8;
			VisualDiagnostics.RegisterSourceInfo(translate8 = new Translate(), new Uri("Coding\\PagesV2\\CodingA5CustomizationPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 98, 21);
			BindingExtension bindingExtension17;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension17 = new BindingExtension(), new Uri("Coding\\PagesV2\\CodingA5CustomizationPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 99, 21);
			StaticResourceExtension staticResourceExtension6;
			VisualDiagnostics.RegisterSourceInfo(staticResourceExtension6 = new StaticResourceExtension(), new Uri("Coding\\PagesV2\\CodingA5CustomizationPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 104, 29);
			BindingExtension bindingExtension18;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension18 = new BindingExtension(), new Uri("Coding\\PagesV2\\CodingA5CustomizationPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 104, 29);
			NumericValidationBehavior numericValidationBehavior;
			VisualDiagnostics.RegisterSourceInfo(numericValidationBehavior = new NumericValidationBehavior(), new Uri("Coding\\PagesV2\\CodingA5CustomizationPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 106, 34);
			Entry entry2;
			VisualDiagnostics.RegisterSourceInfo(entry2 = new Entry(), new Uri("Coding\\PagesV2\\CodingA5CustomizationPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 101, 26);
			CustomCell customCell6;
			VisualDiagnostics.RegisterSourceInfo(customCell6 = new CustomCell(), new Uri("Coding\\PagesV2\\CodingA5CustomizationPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 100, 22);
			Section section5;
			VisualDiagnostics.RegisterSourceInfo(section5 = new Section(), new Uri("Coding\\PagesV2\\CodingA5CustomizationPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 96, 18);
			Translate translate9;
			VisualDiagnostics.RegisterSourceInfo(translate9 = new Translate(), new Uri("Coding\\PagesV2\\CodingA5CustomizationPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 115, 29);
			BindingExtension bindingExtension19;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension19 = new BindingExtension(), new Uri("Coding\\PagesV2\\CodingA5CustomizationPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 115, 91);
			StaticResourceExtension staticResourceExtension7;
			VisualDiagnostics.RegisterSourceInfo(staticResourceExtension7 = new StaticResourceExtension(), new Uri("Coding\\PagesV2\\CodingA5CustomizationPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 117, 51);
			BindingExtension bindingExtension20;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension20 = new BindingExtension(), new Uri("Coding\\PagesV2\\CodingA5CustomizationPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 117, 51);
			NumericValidationBehavior numericValidationBehavior2;
			VisualDiagnostics.RegisterSourceInfo(numericValidationBehavior2 = new NumericValidationBehavior(), new Uri("Coding\\PagesV2\\CodingA5CustomizationPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 119, 34);
			Entry entry3;
			VisualDiagnostics.RegisterSourceInfo(entry3 = new Entry(), new Uri("Coding\\PagesV2\\CodingA5CustomizationPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 117, 26);
			CustomCell customCell7;
			VisualDiagnostics.RegisterSourceInfo(customCell7 = new CustomCell(), new Uri("Coding\\PagesV2\\CodingA5CustomizationPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 116, 22);
			Section section6;
			VisualDiagnostics.RegisterSourceInfo(section6 = new Section(), new Uri("Coding\\PagesV2\\CodingA5CustomizationPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 115, 18);
			BindingExtension bindingExtension21;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension21 = new BindingExtension(), new Uri("Coding\\PagesV2\\CodingA5CustomizationPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 128, 56);
			Translate translate10;
			VisualDiagnostics.RegisterSourceInfo(translate10 = new Translate(), new Uri("Coding\\PagesV2\\CodingA5CustomizationPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 129, 53);
			BindingExtension bindingExtension22;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension22 = new BindingExtension(), new Uri("Coding\\PagesV2\\CodingA5CustomizationPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 129, 100);
			SettingsCheckBoxCellPatched settingsCheckBoxCellPatched;
			VisualDiagnostics.RegisterSourceInfo(settingsCheckBoxCellPatched = new SettingsCheckBoxCellPatched(), new Uri("Coding\\PagesV2\\CodingA5CustomizationPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 129, 22);
			Section section7;
			VisualDiagnostics.RegisterSourceInfo(section7 = new Section(), new Uri("Coding\\PagesV2\\CodingA5CustomizationPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 128, 18);
			Translate translate11;
			VisualDiagnostics.RegisterSourceInfo(translate11 = new Translate(), new Uri("Coding\\PagesV2\\CodingA5CustomizationPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 132, 29);
			BindingExtension bindingExtension23;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension23 = new BindingExtension(), new Uri("Coding\\PagesV2\\CodingA5CustomizationPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 132, 85);
			Translate translate12;
			VisualDiagnostics.RegisterSourceInfo(translate12 = new Translate(), new Uri("Coding\\PagesV2\\CodingA5CustomizationPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 133, 35);
			LabelCell labelCell2;
			VisualDiagnostics.RegisterSourceInfo(labelCell2 = new LabelCell(), new Uri("Coding\\PagesV2\\CodingA5CustomizationPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 133, 22);
			StaticResourceExtension staticResourceExtension8;
			VisualDiagnostics.RegisterSourceInfo(staticResourceExtension8 = new StaticResourceExtension(), new Uri("Coding\\PagesV2\\CodingA5CustomizationPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 135, 51);
			BindingExtension bindingExtension24;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension24 = new BindingExtension(), new Uri("Coding\\PagesV2\\CodingA5CustomizationPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 135, 51);
			NumericValidationBehavior numericValidationBehavior3;
			VisualDiagnostics.RegisterSourceInfo(numericValidationBehavior3 = new NumericValidationBehavior(), new Uri("Coding\\PagesV2\\CodingA5CustomizationPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 137, 34);
			Entry entry4;
			VisualDiagnostics.RegisterSourceInfo(entry4 = new Entry(), new Uri("Coding\\PagesV2\\CodingA5CustomizationPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 135, 26);
			CustomCell customCell8;
			VisualDiagnostics.RegisterSourceInfo(customCell8 = new CustomCell(), new Uri("Coding\\PagesV2\\CodingA5CustomizationPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 134, 22);
			Translate translate13;
			VisualDiagnostics.RegisterSourceInfo(translate13 = new Translate(), new Uri("Coding\\PagesV2\\CodingA5CustomizationPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 145, 35);
			LabelCell labelCell3;
			VisualDiagnostics.RegisterSourceInfo(labelCell3 = new LabelCell(), new Uri("Coding\\PagesV2\\CodingA5CustomizationPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 145, 22);
			StaticResourceExtension staticResourceExtension9;
			VisualDiagnostics.RegisterSourceInfo(staticResourceExtension9 = new StaticResourceExtension(), new Uri("Coding\\PagesV2\\CodingA5CustomizationPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 147, 51);
			BindingExtension bindingExtension25;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension25 = new BindingExtension(), new Uri("Coding\\PagesV2\\CodingA5CustomizationPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 147, 51);
			NumericValidationBehavior numericValidationBehavior4;
			VisualDiagnostics.RegisterSourceInfo(numericValidationBehavior4 = new NumericValidationBehavior(), new Uri("Coding\\PagesV2\\CodingA5CustomizationPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 149, 34);
			Entry entry5;
			VisualDiagnostics.RegisterSourceInfo(entry5 = new Entry(), new Uri("Coding\\PagesV2\\CodingA5CustomizationPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 147, 26);
			CustomCell customCell9;
			VisualDiagnostics.RegisterSourceInfo(customCell9 = new CustomCell(), new Uri("Coding\\PagesV2\\CodingA5CustomizationPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 146, 22);
			Section section8;
			VisualDiagnostics.RegisterSourceInfo(section8 = new Section(), new Uri("Coding\\PagesV2\\CodingA5CustomizationPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 132, 18);
			BindingExtension bindingExtension26;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension26 = new BindingExtension(), new Uri("Coding\\PagesV2\\CodingA5CustomizationPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 162, 21);
			DataTemplate dataTemplate;
			VisualDiagnostics.RegisterSourceInfo(dataTemplate = new DataTemplate(), new Uri("Coding\\PagesV2\\CodingA5CustomizationPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 164, 26);
			Section section9;
			VisualDiagnostics.RegisterSourceInfo(section9 = new Section(), new Uri("Coding\\PagesV2\\CodingA5CustomizationPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 159, 18);
			Translate translate14;
			VisualDiagnostics.RegisterSourceInfo(translate14 = new Translate(), new Uri("Coding\\PagesV2\\CodingA5CustomizationPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 176, 35);
			LabelCell labelCell4;
			VisualDiagnostics.RegisterSourceInfo(labelCell4 = new LabelCell(), new Uri("Coding\\PagesV2\\CodingA5CustomizationPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 176, 22);
			Section section10;
			VisualDiagnostics.RegisterSourceInfo(section10 = new Section(), new Uri("Coding\\PagesV2\\CodingA5CustomizationPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 175, 18);
			SettingsView settingsView;
			VisualDiagnostics.RegisterSourceInfo(settingsView = new SettingsView(), new Uri("Coding\\PagesV2\\CodingA5CustomizationPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 31, 14);
			ActivityFrame activityFrame;
			VisualDiagnostics.RegisterSourceInfo(activityFrame = new ActivityFrame(), new Uri("Coding\\PagesV2\\CodingA5CustomizationPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 186, 14);
			Grid grid2;
			VisualDiagnostics.RegisterSourceInfo(grid2 = new Grid(), new Uri("Coding\\PagesV2\\CodingA5CustomizationPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 30, 10);
			VisualDiagnostics.RegisterSourceInfo(this, new Uri("Coding\\PagesV2\\CodingA5CustomizationPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 2, 2);
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
			nameScope.RegisterName("laneAssistPanel", section5);
			if (section5.StyleId == null)
			{
				section5.StyleId = "laneAssistPanel";
			}
			nameScope.RegisterName("entryLaneAssistTimer", entry2);
			if (entry2.StyleId == null)
			{
				entry2.StyleId = "entryLaneAssistTimer";
			}
			nameScope.RegisterName("optionsPanel", section9);
			if (section9.StyleId == null)
			{
				section9.StyleId = "optionsPanel";
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
			this.laneAssistPanel = section5;
			this.entryLaneAssistTimer = entry2;
			this.optionsPanel = section9;
			this.activityFrame = activityFrame;
			this.Resources = resourceDictionary;
			resourceDictionary.Add("EmptyStringToFalseConverter", emptyStringToFalseConverter);
			resourceDictionary.Add("EmptyStringToTrueConverter", emptyStringToTrueConverter);
			resourceDictionary.Add("IntMsToStringSecondsConverterWithNegativeWrongValue", intMsToStringSecondsConverterWithNegativeWrongValue);
			resourceDictionary.Add("IntToStringConverter", intToStringConverter);
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
			xmlNamespaceResolver.Add("coding", "clr-namespace:CarScannerXamarinForms.Coding");
			xmlNamespaceResolver.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver.Add("mqb", "clr-namespace:CarScannerXamarinForms.Coding.DB.MQB");
			xmlNamespaceResolver.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xmlNamespaceResolver.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider.Add(typeFromHandle2, new XamlTypeResolver(xmlNamespaceResolver, typeof(CodingA5CustomizationPage).GetTypeInfo().Assembly));
			xamlServiceProvider.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(16, 5)));
			object obj2 = markupExtension.ProvideValue(xamlServiceProvider);
			this.Title = obj2;
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
			xmlNamespaceResolver2.Add("coding", "clr-namespace:CarScannerXamarinForms.Coding");
			xmlNamespaceResolver2.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver2.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver2.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver2.Add("mqb", "clr-namespace:CarScannerXamarinForms.Coding.DB.MQB");
			xmlNamespaceResolver2.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver2.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xmlNamespaceResolver2.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver2.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider2.Add(typeFromHandle4, new XamlTypeResolver(xmlNamespaceResolver2, typeof(CodingA5CustomizationPage).GetTypeInfo().Assembly));
			xamlServiceProvider2.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(19, 5)));
			DynamicResource dynamicResource = markupExtension2.ProvideValue(xamlServiceProvider2);
			this.SetDynamicResource(VisualElement.BackgroundColorProperty, dynamicResource.Key);
			this.Resources = resourceDictionary;
			grid2.SetValue(Grid.RowDefinitionsProperty, new RowDefinitionCollectionTypeConverter().ConvertFromInvariantString("*"));
			settingsView.SetValue(Grid.RowProperty, 0);
			settingsView.SetValue(TableView.HasUnevenRowsProperty, true);
			label.SetValue(View.MarginProperty, new Thickness(12.0, 0.0, 0.0, 0.0));
			label.SetValue(Label.FontAttributesProperty, new FontAttributesConverter().ConvertFromInvariantString("Bold"));
			dynamicResourceExtension2.Key = "BaseFontSize+++";
			IMarkupExtension<DynamicResource> markupExtension3 = dynamicResourceExtension2;
			XamlServiceProvider xamlServiceProvider3 = new XamlServiceProvider();
			Type typeFromHandle5 = typeof(IProvideValueTarget);
			object[] array3 = new object[0 + 5];
			array3[0] = label;
			array3[1] = section;
			array3[2] = settingsView;
			array3[3] = grid2;
			array3[4] = this;
			object obj4;
			xamlServiceProvider3.Add(typeFromHandle5, obj4 = new SimpleValueTargetProvider(array3, Label.FontSizeProperty, nameScope));
			xamlServiceProvider3.Add(typeof(IReferenceProvider), obj4);
			Type typeFromHandle6 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver3 = new XmlNamespaceResolver();
			xmlNamespaceResolver3.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver3.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver3.Add("coding", "clr-namespace:CarScannerXamarinForms.Coding");
			xmlNamespaceResolver3.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver3.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver3.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver3.Add("mqb", "clr-namespace:CarScannerXamarinForms.Coding.DB.MQB");
			xmlNamespaceResolver3.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver3.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xmlNamespaceResolver3.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver3.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider3.Add(typeFromHandle6, new XamlTypeResolver(xmlNamespaceResolver3, typeof(CodingA5CustomizationPage).GetTypeInfo().Assembly));
			xamlServiceProvider3.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(40, 29)));
			DynamicResource dynamicResource2 = markupExtension3.ProvideValue(xamlServiceProvider3);
			label.SetDynamicResource(Label.FontSizeProperty, dynamicResource2.Key);
			bindingExtension.Path = "Name";
			bindingExtension.TypedBinding = new TypedBinding<A5ParametrizeV2, string>(delegate(A5ParametrizeV2 A_0)
			{
				if (A_0 != null)
				{
					return new ValueTuple<string, bool>(A_0.Name, true);
				}
				return default(ValueTuple<string, bool>);
			}, delegate(A5ParametrizeV2 A_0, string A_1)
			{
				if (A_0 != null)
				{
					A_0.Name = A_1;
					return;
				}
			}, new Tuple<Func<A5ParametrizeV2, object>, string>[]
			{
				new Tuple<Func<A5ParametrizeV2, object>, string>((A5ParametrizeV2 A_0) => A_0, "Name")
			});
			BindingBase bindingBase = bindingExtension.ProvideValue(null);
			label.SetBinding(Label.TextProperty, bindingBase);
			dynamicResourceExtension3.Key = "SettingsHeaderTextColor";
			IMarkupExtension<DynamicResource> markupExtension4 = dynamicResourceExtension3;
			XamlServiceProvider xamlServiceProvider4 = new XamlServiceProvider();
			Type typeFromHandle7 = typeof(IProvideValueTarget);
			object[] array4 = new object[0 + 5];
			array4[0] = label;
			array4[1] = section;
			array4[2] = settingsView;
			array4[3] = grid2;
			array4[4] = this;
			object obj5;
			xamlServiceProvider4.Add(typeFromHandle7, obj5 = new SimpleValueTargetProvider(array4, Label.TextColorProperty, nameScope));
			xamlServiceProvider4.Add(typeof(IReferenceProvider), obj5);
			Type typeFromHandle8 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver4 = new XmlNamespaceResolver();
			xmlNamespaceResolver4.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver4.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver4.Add("coding", "clr-namespace:CarScannerXamarinForms.Coding");
			xmlNamespaceResolver4.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver4.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver4.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver4.Add("mqb", "clr-namespace:CarScannerXamarinForms.Coding.DB.MQB");
			xmlNamespaceResolver4.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver4.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xmlNamespaceResolver4.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver4.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider4.Add(typeFromHandle8, new XamlTypeResolver(xmlNamespaceResolver4, typeof(CodingA5CustomizationPage).GetTypeInfo().Assembly));
			xamlServiceProvider4.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(42, 29)));
			DynamicResource dynamicResource3 = markupExtension4.ProvideValue(xamlServiceProvider4);
			label.SetDynamicResource(Label.TextColorProperty, dynamicResource3.Key);
			section.SetValue(Section.HeaderViewProperty, label);
			customCell.SetValue(CustomCell.IsSelectableProperty, false);
			staticResourceExtension.Key = "EmptyStringToFalseConverter";
			IMarkupExtension markupExtension5 = staticResourceExtension;
			XamlServiceProvider xamlServiceProvider5 = new XamlServiceProvider();
			Type typeFromHandle9 = typeof(IProvideValueTarget);
			object[] array5 = new object[0 + 6];
			array5[0] = bindingExtension2;
			array5[1] = customCell;
			array5[2] = section;
			array5[3] = settingsView;
			array5[4] = grid2;
			array5[5] = this;
			object obj6;
			xamlServiceProvider5.Add(typeFromHandle9, obj6 = new SimpleValueTargetProvider(array5, typeof(BindingExtension).GetRuntimeProperty("Converter"), nameScope));
			xamlServiceProvider5.Add(typeof(IReferenceProvider), obj6);
			Type typeFromHandle10 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver5 = new XmlNamespaceResolver();
			xmlNamespaceResolver5.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver5.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver5.Add("coding", "clr-namespace:CarScannerXamarinForms.Coding");
			xmlNamespaceResolver5.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver5.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver5.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver5.Add("mqb", "clr-namespace:CarScannerXamarinForms.Coding.DB.MQB");
			xmlNamespaceResolver5.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver5.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xmlNamespaceResolver5.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver5.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider5.Add(typeFromHandle10, new XamlTypeResolver(xmlNamespaceResolver5, typeof(CodingA5CustomizationPage).GetTypeInfo().Assembly));
			xamlServiceProvider5.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(44, 57)));
			object obj7 = markupExtension5.ProvideValue(xamlServiceProvider5);
			bindingExtension2.Converter = obj7;
			bindingExtension2.Path = "Description";
			bindingExtension2.TypedBinding = new TypedBinding<A5ParametrizeV2, string>(delegate(A5ParametrizeV2 A_0)
			{
				if (A_0 != null)
				{
					return new ValueTuple<string, bool>(A_0.Description, true);
				}
				return default(ValueTuple<string, bool>);
			}, delegate(A5ParametrizeV2 A_0, string A_1)
			{
				if (A_0 != null)
				{
					A_0.Description = A_1;
					return;
				}
			}, new Tuple<Func<A5ParametrizeV2, object>, string>[]
			{
				new Tuple<Func<A5ParametrizeV2, object>, string>((A5ParametrizeV2 A_0) => A_0, "Description")
			});
			BindingBase bindingBase2 = bindingExtension2.ProvideValue(null);
			customCell.SetBinding(CellBase.IsVisibleProperty, bindingBase2);
			bindingExtension3.Path = "Description";
			bindingExtension3.TypedBinding = new TypedBinding<A5ParametrizeV2, string>(delegate(A5ParametrizeV2 A_0)
			{
				if (A_0 != null)
				{
					return new ValueTuple<string, bool>(A_0.Description, true);
				}
				return default(ValueTuple<string, bool>);
			}, delegate(A5ParametrizeV2 A_0, string A_1)
			{
				if (A_0 != null)
				{
					A_0.Description = A_1;
					return;
				}
			}, new Tuple<Func<A5ParametrizeV2, object>, string>[]
			{
				new Tuple<Func<A5ParametrizeV2, object>, string>((A5ParametrizeV2 A_0) => A_0, "Description")
			});
			BindingBase bindingBase3 = bindingExtension3.ProvideValue(null);
			label2.SetBinding(Label.TextProperty, bindingBase3);
			customCell.SetValue(CustomCell.ContentProperty, label2);
			section.Add(customCell);
			customCell2.SetValue(CustomCell.IsSelectableProperty, false);
			staticResourceExtension2.Key = "EmptyStringToFalseConverter";
			IMarkupExtension markupExtension6 = staticResourceExtension2;
			XamlServiceProvider xamlServiceProvider6 = new XamlServiceProvider();
			Type typeFromHandle11 = typeof(IProvideValueTarget);
			object[] array6 = new object[0 + 6];
			array6[0] = bindingExtension4;
			array6[1] = customCell2;
			array6[2] = section;
			array6[3] = settingsView;
			array6[4] = grid2;
			array6[5] = this;
			object obj8;
			xamlServiceProvider6.Add(typeFromHandle11, obj8 = new SimpleValueTargetProvider(array6, typeof(BindingExtension).GetRuntimeProperty("Converter"), nameScope));
			xamlServiceProvider6.Add(typeof(IReferenceProvider), obj8);
			Type typeFromHandle12 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver6 = new XmlNamespaceResolver();
			xmlNamespaceResolver6.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver6.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver6.Add("coding", "clr-namespace:CarScannerXamarinForms.Coding");
			xmlNamespaceResolver6.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver6.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver6.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver6.Add("mqb", "clr-namespace:CarScannerXamarinForms.Coding.DB.MQB");
			xmlNamespaceResolver6.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver6.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xmlNamespaceResolver6.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver6.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider6.Add(typeFromHandle12, new XamlTypeResolver(xmlNamespaceResolver6, typeof(CodingA5CustomizationPage).GetTypeInfo().Assembly));
			xamlServiceProvider6.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(48, 57)));
			object obj9 = markupExtension6.ProvideValue(xamlServiceProvider6);
			bindingExtension4.Converter = obj9;
			bindingExtension4.Path = "InnerDescription";
			bindingExtension4.TypedBinding = new TypedBinding<A5ParametrizeV2, string>(delegate(A5ParametrizeV2 A_0)
			{
				if (A_0 != null)
				{
					return new ValueTuple<string, bool>(A_0.InnerDescription, true);
				}
				return default(ValueTuple<string, bool>);
			}, delegate(A5ParametrizeV2 A_0, string A_1)
			{
				if (A_0 != null)
				{
					A_0.InnerDescription = A_1;
					return;
				}
			}, new Tuple<Func<A5ParametrizeV2, object>, string>[]
			{
				new Tuple<Func<A5ParametrizeV2, object>, string>((A5ParametrizeV2 A_0) => A_0, "InnerDescription")
			});
			BindingBase bindingBase4 = bindingExtension4.ProvideValue(null);
			customCell2.SetBinding(CellBase.IsVisibleProperty, bindingBase4);
			bindingExtension5.Path = "InnerDescription";
			bindingExtension5.TypedBinding = new TypedBinding<A5ParametrizeV2, string>(delegate(A5ParametrizeV2 A_0)
			{
				if (A_0 != null)
				{
					return new ValueTuple<string, bool>(A_0.InnerDescription, true);
				}
				return default(ValueTuple<string, bool>);
			}, delegate(A5ParametrizeV2 A_0, string A_1)
			{
				if (A_0 != null)
				{
					A_0.InnerDescription = A_1;
					return;
				}
			}, new Tuple<Func<A5ParametrizeV2, object>, string>[]
			{
				new Tuple<Func<A5ParametrizeV2, object>, string>((A5ParametrizeV2 A_0) => A_0, "InnerDescription")
			});
			BindingBase bindingBase5 = bindingExtension5.ProvideValue(null);
			label3.SetBinding(Label.TextProperty, bindingBase5);
			customCell2.SetValue(CustomCell.ContentProperty, label3);
			section.Add(customCell2);
			settingsView.Root.Add(section);
			translate2.Text = "coding_Password";
			IMarkupExtension markupExtension7 = translate2;
			XamlServiceProvider xamlServiceProvider7 = new XamlServiceProvider();
			Type typeFromHandle13 = typeof(IProvideValueTarget);
			object[] array7 = new object[0 + 4];
			array7[0] = section2;
			array7[1] = settingsView;
			array7[2] = grid2;
			array7[3] = this;
			object obj10;
			xamlServiceProvider7.Add(typeFromHandle13, obj10 = new SimpleValueTargetProvider(array7, SectionBase.TitleProperty, nameScope));
			xamlServiceProvider7.Add(typeof(IReferenceProvider), obj10);
			Type typeFromHandle14 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver7 = new XmlNamespaceResolver();
			xmlNamespaceResolver7.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver7.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver7.Add("coding", "clr-namespace:CarScannerXamarinForms.Coding");
			xmlNamespaceResolver7.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver7.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver7.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver7.Add("mqb", "clr-namespace:CarScannerXamarinForms.Coding.DB.MQB");
			xmlNamespaceResolver7.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver7.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xmlNamespaceResolver7.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver7.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider7.Add(typeFromHandle14, new XamlTypeResolver(xmlNamespaceResolver7, typeof(CodingA5CustomizationPage).GetTypeInfo().Assembly));
			xamlServiceProvider7.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(52, 29)));
			object obj11 = markupExtension7.ProvideValue(xamlServiceProvider7);
			section2.Title = obj11;
			bindingExtension6.Path = "PasswordVisible";
			bindingExtension6.TypedBinding = new TypedBinding<A5ParametrizeV2, bool>(delegate(A5ParametrizeV2 A_0)
			{
				if (A_0 != null)
				{
					return new ValueTuple<bool, bool>(A_0.PasswordVisible, true);
				}
				return default(ValueTuple<bool, bool>);
			}, delegate(A5ParametrizeV2 A_0, bool A_1)
			{
				if (A_0 != null)
				{
					A_0.PasswordVisible = A_1;
					return;
				}
			}, new Tuple<Func<A5ParametrizeV2, object>, string>[]
			{
				new Tuple<Func<A5ParametrizeV2, object>, string>((A5ParametrizeV2 A_0) => A_0, "PasswordVisible")
			});
			BindingBase bindingBase6 = bindingExtension6.ProvideValue(null);
			section2.SetBinding(Section.IsVisibleProperty, bindingBase6);
			customCell3.SetValue(CustomCell.IsSelectableProperty, false);
			bindingExtension7.Mode = 1;
			bindingExtension7.Path = "Password";
			bindingExtension7.TypedBinding = new TypedBinding<A5ParametrizeV2, string>(delegate(A5ParametrizeV2 A_0)
			{
				if (A_0 != null)
				{
					return new ValueTuple<string, bool>(A_0.Password, true);
				}
				return default(ValueTuple<string, bool>);
			}, delegate(A5ParametrizeV2 A_0, string A_1)
			{
				if (A_0 != null)
				{
					A_0.Password = A_1;
					return;
				}
			}, new Tuple<Func<A5ParametrizeV2, object>, string>[]
			{
				new Tuple<Func<A5ParametrizeV2, object>, string>((A5ParametrizeV2 A_0) => A_0, "Password")
			});
			BindingBase bindingBase7 = bindingExtension7.ProvideValue(null);
			entry.SetBinding(Entry.TextProperty, bindingBase7);
			customCell3.SetValue(CustomCell.ContentProperty, entry);
			section2.Add(customCell3);
			customCell4.SetValue(CustomCell.IsSelectableProperty, false);
			staticResourceExtension3.Key = "EmptyStringToFalseConverter";
			IMarkupExtension markupExtension8 = staticResourceExtension3;
			XamlServiceProvider xamlServiceProvider8 = new XamlServiceProvider();
			Type typeFromHandle15 = typeof(IProvideValueTarget);
			object[] array8 = new object[0 + 6];
			array8[0] = bindingExtension8;
			array8[1] = customCell4;
			array8[2] = section2;
			array8[3] = settingsView;
			array8[4] = grid2;
			array8[5] = this;
			object obj12;
			xamlServiceProvider8.Add(typeFromHandle15, obj12 = new SimpleValueTargetProvider(array8, typeof(BindingExtension).GetRuntimeProperty("Converter"), nameScope));
			xamlServiceProvider8.Add(typeof(IReferenceProvider), obj12);
			Type typeFromHandle16 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver8 = new XmlNamespaceResolver();
			xmlNamespaceResolver8.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver8.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver8.Add("coding", "clr-namespace:CarScannerXamarinForms.Coding");
			xmlNamespaceResolver8.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver8.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver8.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver8.Add("mqb", "clr-namespace:CarScannerXamarinForms.Coding.DB.MQB");
			xmlNamespaceResolver8.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver8.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xmlNamespaceResolver8.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver8.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider8.Add(typeFromHandle16, new XamlTypeResolver(xmlNamespaceResolver8, typeof(CodingA5CustomizationPage).GetTypeInfo().Assembly));
			xamlServiceProvider8.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(57, 57)));
			object obj13 = markupExtension8.ProvideValue(xamlServiceProvider8);
			bindingExtension8.Converter = obj13;
			bindingExtension8.Path = "PasswordHint";
			bindingExtension8.TypedBinding = new TypedBinding<A5ParametrizeV2, string>(delegate(A5ParametrizeV2 A_0)
			{
				if (A_0 != null)
				{
					return new ValueTuple<string, bool>(A_0.PasswordHint, true);
				}
				return default(ValueTuple<string, bool>);
			}, delegate(A5ParametrizeV2 A_0, string A_1)
			{
				if (A_0 != null)
				{
					A_0.PasswordHint = A_1;
					return;
				}
			}, new Tuple<Func<A5ParametrizeV2, object>, string>[]
			{
				new Tuple<Func<A5ParametrizeV2, object>, string>((A5ParametrizeV2 A_0) => A_0, "PasswordHint")
			});
			BindingBase bindingBase8 = bindingExtension8.ProvideValue(null);
			customCell4.SetBinding(CellBase.IsVisibleProperty, bindingBase8);
			label4.SetValue(Label.FontAttributesProperty, new FontAttributesConverter().ConvertFromInvariantString("Italic"));
			translate3.Text = "coding_PasswordHint";
			IMarkupExtension markupExtension9 = translate3;
			XamlServiceProvider xamlServiceProvider9 = new XamlServiceProvider();
			Type typeFromHandle17 = typeof(IProvideValueTarget);
			object[] array9 = new object[0 + 8];
			array9[0] = span;
			array9[1] = formattedString;
			array9[2] = label4;
			array9[3] = customCell4;
			array9[4] = section2;
			array9[5] = settingsView;
			array9[6] = grid2;
			array9[7] = this;
			object obj14;
			xamlServiceProvider9.Add(typeFromHandle17, obj14 = new SimpleValueTargetProvider(array9, Span.TextProperty, nameScope));
			xamlServiceProvider9.Add(typeof(IReferenceProvider), obj14);
			Type typeFromHandle18 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver9 = new XmlNamespaceResolver();
			xmlNamespaceResolver9.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver9.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver9.Add("coding", "clr-namespace:CarScannerXamarinForms.Coding");
			xmlNamespaceResolver9.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver9.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver9.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver9.Add("mqb", "clr-namespace:CarScannerXamarinForms.Coding.DB.MQB");
			xmlNamespaceResolver9.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver9.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xmlNamespaceResolver9.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver9.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider9.Add(typeFromHandle18, new XamlTypeResolver(xmlNamespaceResolver9, typeof(CodingA5CustomizationPage).GetTypeInfo().Assembly));
			xamlServiceProvider9.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(61, 43)));
			object obj15 = markupExtension9.ProvideValue(xamlServiceProvider9);
			span.Text = obj15;
			formattedString.Spans.Add(span);
			bindingExtension9.Path = "PasswordHint";
			bindingExtension9.TypedBinding = new TypedBinding<A5ParametrizeV2, string>(delegate(A5ParametrizeV2 A_0)
			{
				if (A_0 != null)
				{
					return new ValueTuple<string, bool>(A_0.PasswordHint, true);
				}
				return default(ValueTuple<string, bool>);
			}, delegate(A5ParametrizeV2 A_0, string A_1)
			{
				if (A_0 != null)
				{
					A_0.PasswordHint = A_1;
					return;
				}
			}, new Tuple<Func<A5ParametrizeV2, object>, string>[]
			{
				new Tuple<Func<A5ParametrizeV2, object>, string>((A5ParametrizeV2 A_0) => A_0, "PasswordHint")
			});
			BindingBase bindingBase9 = bindingExtension9.ProvideValue(null);
			span2.SetBinding(Span.TextProperty, bindingBase9);
			formattedString.Spans.Add(span2);
			label4.SetValue(Label.FormattedTextProperty, formattedString);
			customCell4.SetValue(CustomCell.ContentProperty, label4);
			section2.Add(customCell4);
			customCell5.SetValue(CustomCell.IsSelectableProperty, false);
			bindingExtension10.Mode = 2;
			staticResourceExtension4.Key = "EmptyStringToTrueConverter";
			IMarkupExtension markupExtension10 = staticResourceExtension4;
			XamlServiceProvider xamlServiceProvider10 = new XamlServiceProvider();
			Type typeFromHandle19 = typeof(IProvideValueTarget);
			object[] array10 = new object[0 + 6];
			array10[0] = bindingExtension10;
			array10[1] = customCell5;
			array10[2] = section2;
			array10[3] = settingsView;
			array10[4] = grid2;
			array10[5] = this;
			object obj16;
			xamlServiceProvider10.Add(typeFromHandle19, obj16 = new SimpleValueTargetProvider(array10, typeof(BindingExtension).GetRuntimeProperty("Converter"), nameScope));
			xamlServiceProvider10.Add(typeof(IReferenceProvider), obj16);
			Type typeFromHandle20 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver10 = new XmlNamespaceResolver();
			xmlNamespaceResolver10.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver10.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver10.Add("coding", "clr-namespace:CarScannerXamarinForms.Coding");
			xmlNamespaceResolver10.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver10.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver10.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver10.Add("mqb", "clr-namespace:CarScannerXamarinForms.Coding.DB.MQB");
			xmlNamespaceResolver10.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver10.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xmlNamespaceResolver10.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver10.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider10.Add(typeFromHandle20, new XamlTypeResolver(xmlNamespaceResolver10, typeof(CodingA5CustomizationPage).GetTypeInfo().Assembly));
			xamlServiceProvider10.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(67, 57)));
			object obj17 = markupExtension10.ProvideValue(xamlServiceProvider10);
			bindingExtension10.Converter = obj17;
			bindingExtension10.Path = "Password";
			bindingExtension10.TypedBinding = new TypedBinding<A5ParametrizeV2, string>(delegate(A5ParametrizeV2 A_0)
			{
				if (A_0 != null)
				{
					return new ValueTuple<string, bool>(A_0.Password, true);
				}
				return default(ValueTuple<string, bool>);
			}, null, new Tuple<Func<A5ParametrizeV2, object>, string>[]
			{
				new Tuple<Func<A5ParametrizeV2, object>, string>((A5ParametrizeV2 A_0) => A_0, "Password")
			});
			BindingBase bindingBase10 = bindingExtension10.ProvideValue(null);
			customCell5.SetBinding(CellBase.IsVisibleProperty, bindingBase10);
			translate4.Text = "coding_PasswordNotRequired";
			IMarkupExtension markupExtension11 = translate4;
			XamlServiceProvider xamlServiceProvider11 = new XamlServiceProvider();
			Type typeFromHandle21 = typeof(IProvideValueTarget);
			object[] array11 = new object[0 + 6];
			array11[0] = label5;
			array11[1] = customCell5;
			array11[2] = section2;
			array11[3] = settingsView;
			array11[4] = grid2;
			array11[5] = this;
			object obj18;
			xamlServiceProvider11.Add(typeFromHandle21, obj18 = new SimpleValueTargetProvider(array11, Label.TextProperty, nameScope));
			xamlServiceProvider11.Add(typeof(IReferenceProvider), obj18);
			Type typeFromHandle22 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver11 = new XmlNamespaceResolver();
			xmlNamespaceResolver11.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver11.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver11.Add("coding", "clr-namespace:CarScannerXamarinForms.Coding");
			xmlNamespaceResolver11.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver11.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver11.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver11.Add("mqb", "clr-namespace:CarScannerXamarinForms.Coding.DB.MQB");
			xmlNamespaceResolver11.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver11.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xmlNamespaceResolver11.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver11.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider11.Add(typeFromHandle22, new XamlTypeResolver(xmlNamespaceResolver11, typeof(CodingA5CustomizationPage).GetTypeInfo().Assembly));
			xamlServiceProvider11.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(68, 32)));
			object obj19 = markupExtension11.ProvideValue(xamlServiceProvider11);
			label5.Text = obj19;
			customCell5.SetValue(CustomCell.ContentProperty, label5);
			section2.Add(customCell5);
			settingsView.Root.Add(section2);
			translate5.Text = "coding_CurrentState";
			IMarkupExtension markupExtension12 = translate5;
			XamlServiceProvider xamlServiceProvider12 = new XamlServiceProvider();
			Type typeFromHandle23 = typeof(IProvideValueTarget);
			object[] array12 = new object[0 + 4];
			array12[0] = section3;
			array12[1] = settingsView;
			array12[2] = grid2;
			array12[3] = this;
			object obj20;
			xamlServiceProvider12.Add(typeFromHandle23, obj20 = new SimpleValueTargetProvider(array12, SectionBase.TitleProperty, nameScope));
			xamlServiceProvider12.Add(typeof(IReferenceProvider), obj20);
			Type typeFromHandle24 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver12 = new XmlNamespaceResolver();
			xmlNamespaceResolver12.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver12.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver12.Add("coding", "clr-namespace:CarScannerXamarinForms.Coding");
			xmlNamespaceResolver12.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver12.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver12.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver12.Add("mqb", "clr-namespace:CarScannerXamarinForms.Coding.DB.MQB");
			xmlNamespaceResolver12.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver12.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xmlNamespaceResolver12.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver12.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider12.Add(typeFromHandle24, new XamlTypeResolver(xmlNamespaceResolver12, typeof(CodingA5CustomizationPage).GetTypeInfo().Assembly));
			xamlServiceProvider12.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(72, 29)));
			object obj21 = markupExtension12.ProvideValue(xamlServiceProvider12);
			section3.Title = obj21;
			bindingExtension11.Mode = 2;
			bindingExtension11.Path = "HasCurrentState";
			bindingExtension11.TypedBinding = new TypedBinding<A5ParametrizeV2, bool>(delegate(A5ParametrizeV2 A_0)
			{
				if (A_0 != null)
				{
					return new ValueTuple<bool, bool>(A_0.HasCurrentState, true);
				}
				return default(ValueTuple<bool, bool>);
			}, null, new Tuple<Func<A5ParametrizeV2, object>, string>[]
			{
				new Tuple<Func<A5ParametrizeV2, object>, string>((A5ParametrizeV2 A_0) => A_0, "HasCurrentState")
			});
			BindingBase bindingBase11 = bindingExtension11.ProvideValue(null);
			section3.SetBinding(Section.IsVisibleProperty, bindingBase11);
			bindingExtension12.Mode = 2;
			bindingExtension12.Path = "CurrentState";
			bindingExtension12.TypedBinding = new TypedBinding<A5ParametrizeV2, string>(delegate(A5ParametrizeV2 A_0)
			{
				if (A_0 != null)
				{
					return new ValueTuple<string, bool>(A_0.CurrentState, true);
				}
				return default(ValueTuple<string, bool>);
			}, null, new Tuple<Func<A5ParametrizeV2, object>, string>[]
			{
				new Tuple<Func<A5ParametrizeV2, object>, string>((A5ParametrizeV2 A_0) => A_0, "CurrentState")
			});
			BindingBase bindingBase12 = bindingExtension12.ProvideValue(null);
			labelCell.SetBinding(CellBase.TitleProperty, bindingBase12);
			labelCell.SetValue(CellBase.TitleFontAttributesProperty, new FontAttributes?(1));
			section3.Add(labelCell);
			translate6.Text = "Mode06Page_btnRefresh.Content";
			IMarkupExtension markupExtension13 = translate6;
			XamlServiceProvider xamlServiceProvider13 = new XamlServiceProvider();
			Type typeFromHandle25 = typeof(IProvideValueTarget);
			object[] array13 = new object[0 + 5];
			array13[0] = buttonCell;
			array13[1] = section3;
			array13[2] = settingsView;
			array13[3] = grid2;
			array13[4] = this;
			object obj22;
			xamlServiceProvider13.Add(typeFromHandle25, obj22 = new SimpleValueTargetProvider(array13, CellBase.TitleProperty, nameScope));
			xamlServiceProvider13.Add(typeof(IReferenceProvider), obj22);
			Type typeFromHandle26 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver13 = new XmlNamespaceResolver();
			xmlNamespaceResolver13.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver13.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver13.Add("coding", "clr-namespace:CarScannerXamarinForms.Coding");
			xmlNamespaceResolver13.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver13.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver13.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver13.Add("mqb", "clr-namespace:CarScannerXamarinForms.Coding.DB.MQB");
			xmlNamespaceResolver13.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver13.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xmlNamespaceResolver13.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver13.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider13.Add(typeFromHandle26, new XamlTypeResolver(xmlNamespaceResolver13, typeof(CodingA5CustomizationPage).GetTypeInfo().Assembly));
			xamlServiceProvider13.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(77, 25)));
			object obj23 = markupExtension13.ProvideValue(xamlServiceProvider13);
			buttonCell.Title = obj23;
			buttonCell.Tapped += this.BtnUpdateState_Clicked;
			dynamicResourceExtension4.Key = "ButtonAccentColor";
			IMarkupExtension<DynamicResource> markupExtension14 = dynamicResourceExtension4;
			XamlServiceProvider xamlServiceProvider14 = new XamlServiceProvider();
			Type typeFromHandle27 = typeof(IProvideValueTarget);
			object[] array14 = new object[0 + 5];
			array14[0] = buttonCell;
			array14[1] = section3;
			array14[2] = settingsView;
			array14[3] = grid2;
			array14[4] = this;
			object obj24;
			xamlServiceProvider14.Add(typeFromHandle27, obj24 = new SimpleValueTargetProvider(array14, CellBase.TitleColorProperty, nameScope));
			xamlServiceProvider14.Add(typeof(IReferenceProvider), obj24);
			Type typeFromHandle28 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver14 = new XmlNamespaceResolver();
			xmlNamespaceResolver14.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver14.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver14.Add("coding", "clr-namespace:CarScannerXamarinForms.Coding");
			xmlNamespaceResolver14.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver14.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver14.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver14.Add("mqb", "clr-namespace:CarScannerXamarinForms.Coding.DB.MQB");
			xmlNamespaceResolver14.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver14.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xmlNamespaceResolver14.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver14.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider14.Add(typeFromHandle28, new XamlTypeResolver(xmlNamespaceResolver14, typeof(CodingA5CustomizationPage).GetTypeInfo().Assembly));
			xamlServiceProvider14.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(79, 25)));
			DynamicResource dynamicResource4 = markupExtension14.ProvideValue(xamlServiceProvider14);
			buttonCell.SetDynamicResource(CellBase.TitleColorProperty, dynamicResource4.Key);
			section3.Add(buttonCell);
			settingsView.Root.Add(section3);
			translate7.Text = "codingDB_a5_sourceDataset";
			IMarkupExtension markupExtension15 = translate7;
			XamlServiceProvider xamlServiceProvider15 = new XamlServiceProvider();
			Type typeFromHandle29 = typeof(IProvideValueTarget);
			object[] array15 = new object[0 + 4];
			array15[0] = section4;
			array15[1] = settingsView;
			array15[2] = grid2;
			array15[3] = this;
			object obj25;
			xamlServiceProvider15.Add(typeFromHandle29, obj25 = new SimpleValueTargetProvider(array15, SectionBase.TitleProperty, nameScope));
			xamlServiceProvider15.Add(typeof(IReferenceProvider), obj25);
			Type typeFromHandle30 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver15 = new XmlNamespaceResolver();
			xmlNamespaceResolver15.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver15.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver15.Add("coding", "clr-namespace:CarScannerXamarinForms.Coding");
			xmlNamespaceResolver15.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver15.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver15.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver15.Add("mqb", "clr-namespace:CarScannerXamarinForms.Coding.DB.MQB");
			xmlNamespaceResolver15.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver15.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xmlNamespaceResolver15.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver15.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider15.Add(typeFromHandle30, new XamlTypeResolver(xmlNamespaceResolver15, typeof(CodingA5CustomizationPage).GetTypeInfo().Assembly));
			xamlServiceProvider15.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(82, 29)));
			object obj26 = markupExtension15.ProvideValue(xamlServiceProvider15);
			section4.Title = obj26;
			staticResourceExtension5.Key = "SettingsValueAccentLabel";
			IMarkupExtension markupExtension16 = staticResourceExtension5;
			XamlServiceProvider xamlServiceProvider16 = new XamlServiceProvider();
			Type typeFromHandle31 = typeof(IProvideValueTarget);
			object[] array16 = new object[0 + 7];
			array16[0] = label6;
			array16[1] = grid;
			array16[2] = settingsCustomCellForPicker;
			array16[3] = section4;
			array16[4] = settingsView;
			array16[5] = grid2;
			array16[6] = this;
			object obj27;
			xamlServiceProvider16.Add(typeFromHandle31, obj27 = new SimpleValueTargetProvider(array16, VisualElement.StyleProperty, nameScope));
			xamlServiceProvider16.Add(typeof(IReferenceProvider), obj27);
			Type typeFromHandle32 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver16 = new XmlNamespaceResolver();
			xmlNamespaceResolver16.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver16.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver16.Add("coding", "clr-namespace:CarScannerXamarinForms.Coding");
			xmlNamespaceResolver16.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver16.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver16.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver16.Add("mqb", "clr-namespace:CarScannerXamarinForms.Coding.DB.MQB");
			xmlNamespaceResolver16.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver16.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xmlNamespaceResolver16.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver16.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider16.Add(typeFromHandle32, new XamlTypeResolver(xmlNamespaceResolver16, typeof(CodingA5CustomizationPage).GetTypeInfo().Assembly));
			xamlServiceProvider16.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(85, 36)));
			object obj28 = markupExtension16.ProvideValue(xamlServiceProvider16);
			label6.Style = obj28;
			bindingExtension13.Mode = 2;
			bindingExtension13.Path = "SelectedDataset.Title";
			bindingExtension13.TypedBinding = new TypedBinding<A5ParametrizeV2, string>(delegate(A5ParametrizeV2 A_0)
			{
				if (A_0 != null)
				{
					ProxyFile selectedDataset = A_0.SelectedDataset;
					if (selectedDataset != null)
					{
						return new ValueTuple<string, bool>(selectedDataset.Title, true);
					}
				}
				return default(ValueTuple<string, bool>);
			}, null, new Tuple<Func<A5ParametrizeV2, object>, string>[]
			{
				new Tuple<Func<A5ParametrizeV2, object>, string>((A5ParametrizeV2 A_0) => A_0, "SelectedDataset"),
				new Tuple<Func<A5ParametrizeV2, object>, string>((A5ParametrizeV2 A_0) => A_0.SelectedDataset, "Title")
			});
			BindingBase bindingBase13 = bindingExtension13.ProvideValue(null);
			label6.SetBinding(Label.TextProperty, bindingBase13);
			grid.Children.Add(label6);
			picker.SetValue(VisualElement.IsVisibleProperty, new VisualElement.VisibilityConverter().ConvertFromInvariantString("False"));
			bindingExtension14.Path = "Title";
			BindingBase bindingBase14 = bindingExtension14.ProvideValue(null);
			picker.ItemDisplayBinding = bindingBase14;
			bindingExtension15.Path = "AvailableDatasets";
			bindingExtension15.TypedBinding = new TypedBinding<A5ParametrizeV2, ObservableCollection<ProxyFile>>(delegate(A5ParametrizeV2 A_0)
			{
				if (A_0 != null)
				{
					return new ValueTuple<ObservableCollection<ProxyFile>, bool>(A_0.AvailableDatasets, true);
				}
				return default(ValueTuple<ObservableCollection<ProxyFile>, bool>);
			}, delegate(A5ParametrizeV2 A_0, ObservableCollection<ProxyFile> A_1)
			{
				if (A_0 != null)
				{
					A_0.AvailableDatasets = A_1;
					return;
				}
			}, new Tuple<Func<A5ParametrizeV2, object>, string>[]
			{
				new Tuple<Func<A5ParametrizeV2, object>, string>((A5ParametrizeV2 A_0) => A_0, "AvailableDatasets")
			});
			BindingBase bindingBase15 = bindingExtension15.ProvideValue(null);
			picker.SetBinding(Picker.ItemsSourceProperty, bindingBase15);
			bindingExtension16.Mode = 1;
			bindingExtension16.Path = "SelectedDataset";
			bindingExtension16.TypedBinding = new TypedBinding<A5ParametrizeV2, ProxyFile>(delegate(A5ParametrizeV2 A_0)
			{
				if (A_0 != null)
				{
					return new ValueTuple<ProxyFile, bool>(A_0.SelectedDataset, true);
				}
				return default(ValueTuple<ProxyFile, bool>);
			}, delegate(A5ParametrizeV2 A_0, ProxyFile A_1)
			{
				if (A_0 != null)
				{
					A_0.SelectedDataset = A_1;
					return;
				}
			}, new Tuple<Func<A5ParametrizeV2, object>, string>[]
			{
				new Tuple<Func<A5ParametrizeV2, object>, string>((A5ParametrizeV2 A_0) => A_0, "SelectedDataset")
			});
			BindingBase bindingBase16 = bindingExtension16.ProvideValue(null);
			picker.SetBinding(Picker.SelectedItemProperty, bindingBase16);
			grid.Children.Add(picker);
			settingsCustomCellForPicker.SetValue(CustomCell.ContentProperty, grid);
			section4.Add(settingsCustomCellForPicker);
			settingsView.Root.Add(section4);
			translate8.Text = "codingDB_a5_laneAssistTimer";
			IMarkupExtension markupExtension17 = translate8;
			XamlServiceProvider xamlServiceProvider17 = new XamlServiceProvider();
			Type typeFromHandle33 = typeof(IProvideValueTarget);
			object[] array17 = new object[0 + 4];
			array17[0] = section5;
			array17[1] = settingsView;
			array17[2] = grid2;
			array17[3] = this;
			object obj29;
			xamlServiceProvider17.Add(typeFromHandle33, obj29 = new SimpleValueTargetProvider(array17, SectionBase.TitleProperty, nameScope));
			xamlServiceProvider17.Add(typeof(IReferenceProvider), obj29);
			Type typeFromHandle34 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver17 = new XmlNamespaceResolver();
			xmlNamespaceResolver17.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver17.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver17.Add("coding", "clr-namespace:CarScannerXamarinForms.Coding");
			xmlNamespaceResolver17.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver17.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver17.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver17.Add("mqb", "clr-namespace:CarScannerXamarinForms.Coding.DB.MQB");
			xmlNamespaceResolver17.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver17.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xmlNamespaceResolver17.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver17.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider17.Add(typeFromHandle34, new XamlTypeResolver(xmlNamespaceResolver17, typeof(CodingA5CustomizationPage).GetTypeInfo().Assembly));
			xamlServiceProvider17.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(98, 21)));
			object obj30 = markupExtension17.ProvideValue(xamlServiceProvider17);
			section5.Title = obj30;
			bindingExtension17.Path = "Customizer.LaneAssistTimerConfigurationAvailable";
			bindingExtension17.TypedBinding = new TypedBinding<A5ParametrizeV2, bool>(delegate(A5ParametrizeV2 A_0)
			{
				if (A_0 != null)
				{
					A5DatasetCustomizer customizer = A_0.Customizer;
					if (customizer != null)
					{
						return new ValueTuple<bool, bool>(customizer.LaneAssistTimerConfigurationAvailable, true);
					}
				}
				return default(ValueTuple<bool, bool>);
			}, delegate(A5ParametrizeV2 A_0, bool A_1)
			{
				if (A_0 != null)
				{
					A5DatasetCustomizer customizer2 = A_0.Customizer;
					if (customizer2 != null)
					{
						customizer2.LaneAssistTimerConfigurationAvailable = A_1;
						return;
					}
				}
			}, new Tuple<Func<A5ParametrizeV2, object>, string>[]
			{
				new Tuple<Func<A5ParametrizeV2, object>, string>((A5ParametrizeV2 A_0) => A_0, "Customizer"),
				new Tuple<Func<A5ParametrizeV2, object>, string>((A5ParametrizeV2 A_0) => A_0.Customizer, "LaneAssistTimerConfigurationAvailable")
			});
			BindingBase bindingBase17 = bindingExtension17.ProvideValue(null);
			section5.SetBinding(Section.IsVisibleProperty, bindingBase17);
			customCell6.SetValue(CustomCell.IsSelectableProperty, false);
			entry2.SetValue(InputView.KeyboardProperty, new KeyboardTypeConverter().ConvertFromInvariantString("Numeric"));
			bindingExtension18.Mode = 1;
			staticResourceExtension6.Key = "IntMsToStringSecondsConverterWithNegativeWrongValue";
			IMarkupExtension markupExtension18 = staticResourceExtension6;
			XamlServiceProvider xamlServiceProvider18 = new XamlServiceProvider();
			Type typeFromHandle35 = typeof(IProvideValueTarget);
			object[] array18 = new object[0 + 7];
			array18[0] = bindingExtension18;
			array18[1] = entry2;
			array18[2] = customCell6;
			array18[3] = section5;
			array18[4] = settingsView;
			array18[5] = grid2;
			array18[6] = this;
			object obj31;
			xamlServiceProvider18.Add(typeFromHandle35, obj31 = new SimpleValueTargetProvider(array18, typeof(BindingExtension).GetRuntimeProperty("Converter"), nameScope));
			xamlServiceProvider18.Add(typeof(IReferenceProvider), obj31);
			Type typeFromHandle36 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver18 = new XmlNamespaceResolver();
			xmlNamespaceResolver18.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver18.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver18.Add("coding", "clr-namespace:CarScannerXamarinForms.Coding");
			xmlNamespaceResolver18.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver18.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver18.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver18.Add("mqb", "clr-namespace:CarScannerXamarinForms.Coding.DB.MQB");
			xmlNamespaceResolver18.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver18.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xmlNamespaceResolver18.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver18.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider18.Add(typeFromHandle36, new XamlTypeResolver(xmlNamespaceResolver18, typeof(CodingA5CustomizationPage).GetTypeInfo().Assembly));
			xamlServiceProvider18.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(104, 29)));
			object obj32 = markupExtension18.ProvideValue(xamlServiceProvider18);
			bindingExtension18.Converter = obj32;
			bindingExtension18.Path = "Customizer.LaneAssistTimer";
			bindingExtension18.TypedBinding = new TypedBinding<A5ParametrizeV2, int>(delegate(A5ParametrizeV2 A_0)
			{
				if (A_0 != null)
				{
					A5DatasetCustomizer customizer3 = A_0.Customizer;
					if (customizer3 != null)
					{
						return new ValueTuple<int, bool>(customizer3.LaneAssistTimer, true);
					}
				}
				return default(ValueTuple<int, bool>);
			}, delegate(A5ParametrizeV2 A_0, int A_1)
			{
				if (A_0 != null)
				{
					A5DatasetCustomizer customizer4 = A_0.Customizer;
					if (customizer4 != null)
					{
						customizer4.LaneAssistTimer = A_1;
						return;
					}
				}
			}, new Tuple<Func<A5ParametrizeV2, object>, string>[]
			{
				new Tuple<Func<A5ParametrizeV2, object>, string>((A5ParametrizeV2 A_0) => A_0, "Customizer"),
				new Tuple<Func<A5ParametrizeV2, object>, string>((A5ParametrizeV2 A_0) => A_0.Customizer, "LaneAssistTimer")
			});
			BindingBase bindingBase18 = bindingExtension18.ProvideValue(null);
			entry2.SetBinding(Entry.TextProperty, bindingBase18);
			numericValidationBehavior.SetValue(NumericValidationBehavior.MaximumDecimalPlacesProperty, 0);
			numericValidationBehavior.SetValue(NumericValidationBehavior.MinimumDecimalPlacesProperty, 0);
			numericValidationBehavior.SetValue(NumericValidationBehavior.MinimumValueProperty, 1.0);
			((ICollection<Behavior>)entry2.GetValue(VisualElement.BehaviorsProperty)).Add(numericValidationBehavior);
			customCell6.SetValue(CustomCell.ContentProperty, entry2);
			section5.Add(customCell6);
			settingsView.Root.Add(section5);
			translate9.Text = "coding_A5_LaneAssistStartSpeed";
			IMarkupExtension markupExtension19 = translate9;
			XamlServiceProvider xamlServiceProvider19 = new XamlServiceProvider();
			Type typeFromHandle37 = typeof(IProvideValueTarget);
			object[] array19 = new object[0 + 4];
			array19[0] = section6;
			array19[1] = settingsView;
			array19[2] = grid2;
			array19[3] = this;
			object obj33;
			xamlServiceProvider19.Add(typeFromHandle37, obj33 = new SimpleValueTargetProvider(array19, SectionBase.TitleProperty, nameScope));
			xamlServiceProvider19.Add(typeof(IReferenceProvider), obj33);
			Type typeFromHandle38 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver19 = new XmlNamespaceResolver();
			xmlNamespaceResolver19.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver19.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver19.Add("coding", "clr-namespace:CarScannerXamarinForms.Coding");
			xmlNamespaceResolver19.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver19.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver19.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver19.Add("mqb", "clr-namespace:CarScannerXamarinForms.Coding.DB.MQB");
			xmlNamespaceResolver19.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver19.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xmlNamespaceResolver19.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver19.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider19.Add(typeFromHandle38, new XamlTypeResolver(xmlNamespaceResolver19, typeof(CodingA5CustomizationPage).GetTypeInfo().Assembly));
			xamlServiceProvider19.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(115, 29)));
			object obj34 = markupExtension19.ProvideValue(xamlServiceProvider19);
			section6.Title = obj34;
			bindingExtension19.Path = "Customizer.LaneAssistStartSpeedConfigurationAvailable";
			bindingExtension19.TypedBinding = new TypedBinding<A5ParametrizeV2, bool>(delegate(A5ParametrizeV2 A_0)
			{
				if (A_0 != null)
				{
					A5DatasetCustomizer customizer5 = A_0.Customizer;
					if (customizer5 != null)
					{
						return new ValueTuple<bool, bool>(customizer5.LaneAssistStartSpeedConfigurationAvailable, true);
					}
				}
				return default(ValueTuple<bool, bool>);
			}, delegate(A5ParametrizeV2 A_0, bool A_1)
			{
				if (A_0 != null)
				{
					A5DatasetCustomizer customizer6 = A_0.Customizer;
					if (customizer6 != null)
					{
						customizer6.LaneAssistStartSpeedConfigurationAvailable = A_1;
						return;
					}
				}
			}, new Tuple<Func<A5ParametrizeV2, object>, string>[]
			{
				new Tuple<Func<A5ParametrizeV2, object>, string>((A5ParametrizeV2 A_0) => A_0, "Customizer"),
				new Tuple<Func<A5ParametrizeV2, object>, string>((A5ParametrizeV2 A_0) => A_0.Customizer, "LaneAssistStartSpeedConfigurationAvailable")
			});
			BindingBase bindingBase19 = bindingExtension19.ProvideValue(null);
			section6.SetBinding(Section.IsVisibleProperty, bindingBase19);
			customCell7.SetValue(CustomCell.IsSelectableProperty, false);
			entry3.SetValue(InputView.KeyboardProperty, new KeyboardTypeConverter().ConvertFromInvariantString("Numeric"));
			bindingExtension20.Mode = 1;
			staticResourceExtension7.Key = "IntToStringConverter";
			IMarkupExtension markupExtension20 = staticResourceExtension7;
			XamlServiceProvider xamlServiceProvider20 = new XamlServiceProvider();
			Type typeFromHandle39 = typeof(IProvideValueTarget);
			object[] array20 = new object[0 + 7];
			array20[0] = bindingExtension20;
			array20[1] = entry3;
			array20[2] = customCell7;
			array20[3] = section6;
			array20[4] = settingsView;
			array20[5] = grid2;
			array20[6] = this;
			object obj35;
			xamlServiceProvider20.Add(typeFromHandle39, obj35 = new SimpleValueTargetProvider(array20, typeof(BindingExtension).GetRuntimeProperty("Converter"), nameScope));
			xamlServiceProvider20.Add(typeof(IReferenceProvider), obj35);
			Type typeFromHandle40 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver20 = new XmlNamespaceResolver();
			xmlNamespaceResolver20.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver20.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver20.Add("coding", "clr-namespace:CarScannerXamarinForms.Coding");
			xmlNamespaceResolver20.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver20.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver20.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver20.Add("mqb", "clr-namespace:CarScannerXamarinForms.Coding.DB.MQB");
			xmlNamespaceResolver20.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver20.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xmlNamespaceResolver20.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver20.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider20.Add(typeFromHandle40, new XamlTypeResolver(xmlNamespaceResolver20, typeof(CodingA5CustomizationPage).GetTypeInfo().Assembly));
			xamlServiceProvider20.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(117, 51)));
			object obj36 = markupExtension20.ProvideValue(xamlServiceProvider20);
			bindingExtension20.Converter = obj36;
			bindingExtension20.Path = "Customizer.LaneAssistStartSpeed";
			bindingExtension20.TypedBinding = new TypedBinding<A5ParametrizeV2, int>(delegate(A5ParametrizeV2 A_0)
			{
				if (A_0 != null)
				{
					A5DatasetCustomizer customizer7 = A_0.Customizer;
					if (customizer7 != null)
					{
						return new ValueTuple<int, bool>(customizer7.LaneAssistStartSpeed, true);
					}
				}
				return default(ValueTuple<int, bool>);
			}, delegate(A5ParametrizeV2 A_0, int A_1)
			{
				if (A_0 != null)
				{
					A5DatasetCustomizer customizer8 = A_0.Customizer;
					if (customizer8 != null)
					{
						customizer8.LaneAssistStartSpeed = A_1;
						return;
					}
				}
			}, new Tuple<Func<A5ParametrizeV2, object>, string>[]
			{
				new Tuple<Func<A5ParametrizeV2, object>, string>((A5ParametrizeV2 A_0) => A_0, "Customizer"),
				new Tuple<Func<A5ParametrizeV2, object>, string>((A5ParametrizeV2 A_0) => A_0.Customizer, "LaneAssistStartSpeed")
			});
			BindingBase bindingBase20 = bindingExtension20.ProvideValue(null);
			entry3.SetBinding(Entry.TextProperty, bindingBase20);
			numericValidationBehavior2.SetValue(NumericValidationBehavior.MaximumDecimalPlacesProperty, 0);
			numericValidationBehavior2.SetValue(NumericValidationBehavior.MinimumDecimalPlacesProperty, 0);
			numericValidationBehavior2.SetValue(NumericValidationBehavior.MinimumValueProperty, 0.0);
			((ICollection<Behavior>)entry3.GetValue(VisualElement.BehaviorsProperty)).Add(numericValidationBehavior2);
			customCell7.SetValue(CustomCell.ContentProperty, entry3);
			section6.Add(customCell7);
			settingsView.Root.Add(section6);
			section7.SetValue(SectionBase.TitleProperty, "Traffic Jam Assist");
			bindingExtension21.Path = "Customizer.TJAConfigurationAvailable";
			bindingExtension21.TypedBinding = new TypedBinding<A5ParametrizeV2, bool>(delegate(A5ParametrizeV2 A_0)
			{
				if (A_0 != null)
				{
					A5DatasetCustomizer customizer9 = A_0.Customizer;
					if (customizer9 != null)
					{
						return new ValueTuple<bool, bool>(customizer9.TJAConfigurationAvailable, true);
					}
				}
				return default(ValueTuple<bool, bool>);
			}, delegate(A5ParametrizeV2 A_0, bool A_1)
			{
				if (A_0 != null)
				{
					A5DatasetCustomizer customizer10 = A_0.Customizer;
					if (customizer10 != null)
					{
						customizer10.TJAConfigurationAvailable = A_1;
						return;
					}
				}
			}, new Tuple<Func<A5ParametrizeV2, object>, string>[]
			{
				new Tuple<Func<A5ParametrizeV2, object>, string>((A5ParametrizeV2 A_0) => A_0, "Customizer"),
				new Tuple<Func<A5ParametrizeV2, object>, string>((A5ParametrizeV2 A_0) => A_0.Customizer, "TJAConfigurationAvailable")
			});
			BindingBase bindingBase21 = bindingExtension21.ProvideValue(null);
			section7.SetBinding(Section.IsVisibleProperty, bindingBase21);
			translate10.Text = "codingDB_a5_TJA";
			IMarkupExtension markupExtension21 = translate10;
			XamlServiceProvider xamlServiceProvider21 = new XamlServiceProvider();
			Type typeFromHandle41 = typeof(IProvideValueTarget);
			object[] array21 = new object[0 + 5];
			array21[0] = settingsCheckBoxCellPatched;
			array21[1] = section7;
			array21[2] = settingsView;
			array21[3] = grid2;
			array21[4] = this;
			object obj37;
			xamlServiceProvider21.Add(typeFromHandle41, obj37 = new SimpleValueTargetProvider(array21, CellBase.TitleProperty, nameScope));
			xamlServiceProvider21.Add(typeof(IReferenceProvider), obj37);
			Type typeFromHandle42 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver21 = new XmlNamespaceResolver();
			xmlNamespaceResolver21.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver21.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver21.Add("coding", "clr-namespace:CarScannerXamarinForms.Coding");
			xmlNamespaceResolver21.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver21.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver21.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver21.Add("mqb", "clr-namespace:CarScannerXamarinForms.Coding.DB.MQB");
			xmlNamespaceResolver21.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver21.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xmlNamespaceResolver21.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver21.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider21.Add(typeFromHandle42, new XamlTypeResolver(xmlNamespaceResolver21, typeof(CodingA5CustomizationPage).GetTypeInfo().Assembly));
			xamlServiceProvider21.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(129, 53)));
			object obj38 = markupExtension21.ProvideValue(xamlServiceProvider21);
			settingsCheckBoxCellPatched.Title = obj38;
			bindingExtension22.Mode = 1;
			bindingExtension22.Path = "Customizer.TJA";
			bindingExtension22.TypedBinding = new TypedBinding<A5ParametrizeV2, bool>(delegate(A5ParametrizeV2 A_0)
			{
				if (A_0 != null)
				{
					A5DatasetCustomizer customizer11 = A_0.Customizer;
					if (customizer11 != null)
					{
						return new ValueTuple<bool, bool>(customizer11.TJA, true);
					}
				}
				return default(ValueTuple<bool, bool>);
			}, delegate(A5ParametrizeV2 A_0, bool A_1)
			{
				if (A_0 != null)
				{
					A5DatasetCustomizer customizer12 = A_0.Customizer;
					if (customizer12 != null)
					{
						customizer12.TJA = A_1;
						return;
					}
				}
			}, new Tuple<Func<A5ParametrizeV2, object>, string>[]
			{
				new Tuple<Func<A5ParametrizeV2, object>, string>((A5ParametrizeV2 A_0) => A_0, "Customizer"),
				new Tuple<Func<A5ParametrizeV2, object>, string>((A5ParametrizeV2 A_0) => A_0.Customizer, "TJA")
			});
			BindingBase bindingBase22 = bindingExtension22.ProvideValue(null);
			settingsCheckBoxCellPatched.SetBinding(CheckboxCell.CheckedProperty, bindingBase22);
			section7.Add(settingsCheckBoxCellPatched);
			settingsView.Root.Add(section7);
			translate11.Text = "coding_A5_HighBeamAssist";
			IMarkupExtension markupExtension22 = translate11;
			XamlServiceProvider xamlServiceProvider22 = new XamlServiceProvider();
			Type typeFromHandle43 = typeof(IProvideValueTarget);
			object[] array22 = new object[0 + 4];
			array22[0] = section8;
			array22[1] = settingsView;
			array22[2] = grid2;
			array22[3] = this;
			object obj39;
			xamlServiceProvider22.Add(typeFromHandle43, obj39 = new SimpleValueTargetProvider(array22, SectionBase.TitleProperty, nameScope));
			xamlServiceProvider22.Add(typeof(IReferenceProvider), obj39);
			Type typeFromHandle44 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver22 = new XmlNamespaceResolver();
			xmlNamespaceResolver22.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver22.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver22.Add("coding", "clr-namespace:CarScannerXamarinForms.Coding");
			xmlNamespaceResolver22.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver22.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver22.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver22.Add("mqb", "clr-namespace:CarScannerXamarinForms.Coding.DB.MQB");
			xmlNamespaceResolver22.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver22.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xmlNamespaceResolver22.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver22.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider22.Add(typeFromHandle44, new XamlTypeResolver(xmlNamespaceResolver22, typeof(CodingA5CustomizationPage).GetTypeInfo().Assembly));
			xamlServiceProvider22.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(132, 29)));
			object obj40 = markupExtension22.ProvideValue(xamlServiceProvider22);
			section8.Title = obj40;
			bindingExtension23.Path = "Customizer.LightAssistSpeedConfigurationAvailable";
			bindingExtension23.TypedBinding = new TypedBinding<A5ParametrizeV2, bool>(delegate(A5ParametrizeV2 A_0)
			{
				if (A_0 != null)
				{
					A5DatasetCustomizer customizer13 = A_0.Customizer;
					if (customizer13 != null)
					{
						return new ValueTuple<bool, bool>(customizer13.LightAssistSpeedConfigurationAvailable, true);
					}
				}
				return default(ValueTuple<bool, bool>);
			}, delegate(A5ParametrizeV2 A_0, bool A_1)
			{
				if (A_0 != null)
				{
					A5DatasetCustomizer customizer14 = A_0.Customizer;
					if (customizer14 != null)
					{
						customizer14.LightAssistSpeedConfigurationAvailable = A_1;
						return;
					}
				}
			}, new Tuple<Func<A5ParametrizeV2, object>, string>[]
			{
				new Tuple<Func<A5ParametrizeV2, object>, string>((A5ParametrizeV2 A_0) => A_0, "Customizer"),
				new Tuple<Func<A5ParametrizeV2, object>, string>((A5ParametrizeV2 A_0) => A_0.Customizer, "LightAssistSpeedConfigurationAvailable")
			});
			BindingBase bindingBase23 = bindingExtension23.ProvideValue(null);
			section8.SetBinding(Section.IsVisibleProperty, bindingBase23);
			translate12.Text = "coding_A5_HighBeamAssistOnSpeed";
			IMarkupExtension markupExtension23 = translate12;
			XamlServiceProvider xamlServiceProvider23 = new XamlServiceProvider();
			Type typeFromHandle45 = typeof(IProvideValueTarget);
			object[] array23 = new object[0 + 5];
			array23[0] = labelCell2;
			array23[1] = section8;
			array23[2] = settingsView;
			array23[3] = grid2;
			array23[4] = this;
			object obj41;
			xamlServiceProvider23.Add(typeFromHandle45, obj41 = new SimpleValueTargetProvider(array23, CellBase.TitleProperty, nameScope));
			xamlServiceProvider23.Add(typeof(IReferenceProvider), obj41);
			Type typeFromHandle46 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver23 = new XmlNamespaceResolver();
			xmlNamespaceResolver23.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver23.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver23.Add("coding", "clr-namespace:CarScannerXamarinForms.Coding");
			xmlNamespaceResolver23.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver23.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver23.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver23.Add("mqb", "clr-namespace:CarScannerXamarinForms.Coding.DB.MQB");
			xmlNamespaceResolver23.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver23.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xmlNamespaceResolver23.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver23.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider23.Add(typeFromHandle46, new XamlTypeResolver(xmlNamespaceResolver23, typeof(CodingA5CustomizationPage).GetTypeInfo().Assembly));
			xamlServiceProvider23.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(133, 35)));
			object obj42 = markupExtension23.ProvideValue(xamlServiceProvider23);
			labelCell2.Title = obj42;
			section8.Add(labelCell2);
			customCell8.SetValue(CustomCell.IsSelectableProperty, false);
			entry4.SetValue(InputView.KeyboardProperty, new KeyboardTypeConverter().ConvertFromInvariantString("Numeric"));
			bindingExtension24.Mode = 1;
			staticResourceExtension8.Key = "IntToStringConverter";
			IMarkupExtension markupExtension24 = staticResourceExtension8;
			XamlServiceProvider xamlServiceProvider24 = new XamlServiceProvider();
			Type typeFromHandle47 = typeof(IProvideValueTarget);
			object[] array24 = new object[0 + 7];
			array24[0] = bindingExtension24;
			array24[1] = entry4;
			array24[2] = customCell8;
			array24[3] = section8;
			array24[4] = settingsView;
			array24[5] = grid2;
			array24[6] = this;
			object obj43;
			xamlServiceProvider24.Add(typeFromHandle47, obj43 = new SimpleValueTargetProvider(array24, typeof(BindingExtension).GetRuntimeProperty("Converter"), nameScope));
			xamlServiceProvider24.Add(typeof(IReferenceProvider), obj43);
			Type typeFromHandle48 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver24 = new XmlNamespaceResolver();
			xmlNamespaceResolver24.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver24.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver24.Add("coding", "clr-namespace:CarScannerXamarinForms.Coding");
			xmlNamespaceResolver24.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver24.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver24.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver24.Add("mqb", "clr-namespace:CarScannerXamarinForms.Coding.DB.MQB");
			xmlNamespaceResolver24.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver24.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xmlNamespaceResolver24.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver24.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider24.Add(typeFromHandle48, new XamlTypeResolver(xmlNamespaceResolver24, typeof(CodingA5CustomizationPage).GetTypeInfo().Assembly));
			xamlServiceProvider24.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(135, 51)));
			object obj44 = markupExtension24.ProvideValue(xamlServiceProvider24);
			bindingExtension24.Converter = obj44;
			bindingExtension24.Path = "Customizer.LightAssistOnSpeed";
			bindingExtension24.TypedBinding = new TypedBinding<A5ParametrizeV2, int>(delegate(A5ParametrizeV2 A_0)
			{
				if (A_0 != null)
				{
					A5DatasetCustomizer customizer15 = A_0.Customizer;
					if (customizer15 != null)
					{
						return new ValueTuple<int, bool>(customizer15.LightAssistOnSpeed, true);
					}
				}
				return default(ValueTuple<int, bool>);
			}, delegate(A5ParametrizeV2 A_0, int A_1)
			{
				if (A_0 != null)
				{
					A5DatasetCustomizer customizer16 = A_0.Customizer;
					if (customizer16 != null)
					{
						customizer16.LightAssistOnSpeed = A_1;
						return;
					}
				}
			}, new Tuple<Func<A5ParametrizeV2, object>, string>[]
			{
				new Tuple<Func<A5ParametrizeV2, object>, string>((A5ParametrizeV2 A_0) => A_0, "Customizer"),
				new Tuple<Func<A5ParametrizeV2, object>, string>((A5ParametrizeV2 A_0) => A_0.Customizer, "LightAssistOnSpeed")
			});
			BindingBase bindingBase24 = bindingExtension24.ProvideValue(null);
			entry4.SetBinding(Entry.TextProperty, bindingBase24);
			numericValidationBehavior3.SetValue(NumericValidationBehavior.MaximumDecimalPlacesProperty, 0);
			numericValidationBehavior3.SetValue(NumericValidationBehavior.MinimumDecimalPlacesProperty, 0);
			numericValidationBehavior3.SetValue(NumericValidationBehavior.MinimumValueProperty, 0.0);
			((ICollection<Behavior>)entry4.GetValue(VisualElement.BehaviorsProperty)).Add(numericValidationBehavior3);
			customCell8.SetValue(CustomCell.ContentProperty, entry4);
			section8.Add(customCell8);
			translate13.Text = "coding_A5_HighBeamAssistOffSpeed";
			IMarkupExtension markupExtension25 = translate13;
			XamlServiceProvider xamlServiceProvider25 = new XamlServiceProvider();
			Type typeFromHandle49 = typeof(IProvideValueTarget);
			object[] array25 = new object[0 + 5];
			array25[0] = labelCell3;
			array25[1] = section8;
			array25[2] = settingsView;
			array25[3] = grid2;
			array25[4] = this;
			object obj45;
			xamlServiceProvider25.Add(typeFromHandle49, obj45 = new SimpleValueTargetProvider(array25, CellBase.TitleProperty, nameScope));
			xamlServiceProvider25.Add(typeof(IReferenceProvider), obj45);
			Type typeFromHandle50 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver25 = new XmlNamespaceResolver();
			xmlNamespaceResolver25.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver25.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver25.Add("coding", "clr-namespace:CarScannerXamarinForms.Coding");
			xmlNamespaceResolver25.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver25.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver25.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver25.Add("mqb", "clr-namespace:CarScannerXamarinForms.Coding.DB.MQB");
			xmlNamespaceResolver25.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver25.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xmlNamespaceResolver25.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver25.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider25.Add(typeFromHandle50, new XamlTypeResolver(xmlNamespaceResolver25, typeof(CodingA5CustomizationPage).GetTypeInfo().Assembly));
			xamlServiceProvider25.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(145, 35)));
			object obj46 = markupExtension25.ProvideValue(xamlServiceProvider25);
			labelCell3.Title = obj46;
			section8.Add(labelCell3);
			customCell9.SetValue(CustomCell.IsSelectableProperty, false);
			entry5.SetValue(InputView.KeyboardProperty, new KeyboardTypeConverter().ConvertFromInvariantString("Numeric"));
			bindingExtension25.Mode = 1;
			staticResourceExtension9.Key = "IntToStringConverter";
			IMarkupExtension markupExtension26 = staticResourceExtension9;
			XamlServiceProvider xamlServiceProvider26 = new XamlServiceProvider();
			Type typeFromHandle51 = typeof(IProvideValueTarget);
			object[] array26 = new object[0 + 7];
			array26[0] = bindingExtension25;
			array26[1] = entry5;
			array26[2] = customCell9;
			array26[3] = section8;
			array26[4] = settingsView;
			array26[5] = grid2;
			array26[6] = this;
			object obj47;
			xamlServiceProvider26.Add(typeFromHandle51, obj47 = new SimpleValueTargetProvider(array26, typeof(BindingExtension).GetRuntimeProperty("Converter"), nameScope));
			xamlServiceProvider26.Add(typeof(IReferenceProvider), obj47);
			Type typeFromHandle52 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver26 = new XmlNamespaceResolver();
			xmlNamespaceResolver26.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver26.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver26.Add("coding", "clr-namespace:CarScannerXamarinForms.Coding");
			xmlNamespaceResolver26.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver26.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver26.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver26.Add("mqb", "clr-namespace:CarScannerXamarinForms.Coding.DB.MQB");
			xmlNamespaceResolver26.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver26.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xmlNamespaceResolver26.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver26.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider26.Add(typeFromHandle52, new XamlTypeResolver(xmlNamespaceResolver26, typeof(CodingA5CustomizationPage).GetTypeInfo().Assembly));
			xamlServiceProvider26.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(147, 51)));
			object obj48 = markupExtension26.ProvideValue(xamlServiceProvider26);
			bindingExtension25.Converter = obj48;
			bindingExtension25.Path = "Customizer.LightAssistOffSpeed";
			bindingExtension25.TypedBinding = new TypedBinding<A5ParametrizeV2, int>(delegate(A5ParametrizeV2 A_0)
			{
				if (A_0 != null)
				{
					A5DatasetCustomizer customizer17 = A_0.Customizer;
					if (customizer17 != null)
					{
						return new ValueTuple<int, bool>(customizer17.LightAssistOffSpeed, true);
					}
				}
				return default(ValueTuple<int, bool>);
			}, delegate(A5ParametrizeV2 A_0, int A_1)
			{
				if (A_0 != null)
				{
					A5DatasetCustomizer customizer18 = A_0.Customizer;
					if (customizer18 != null)
					{
						customizer18.LightAssistOffSpeed = A_1;
						return;
					}
				}
			}, new Tuple<Func<A5ParametrizeV2, object>, string>[]
			{
				new Tuple<Func<A5ParametrizeV2, object>, string>((A5ParametrizeV2 A_0) => A_0, "Customizer"),
				new Tuple<Func<A5ParametrizeV2, object>, string>((A5ParametrizeV2 A_0) => A_0.Customizer, "LightAssistOffSpeed")
			});
			BindingBase bindingBase25 = bindingExtension25.ProvideValue(null);
			entry5.SetBinding(Entry.TextProperty, bindingBase25);
			numericValidationBehavior4.SetValue(NumericValidationBehavior.MaximumDecimalPlacesProperty, 0);
			numericValidationBehavior4.SetValue(NumericValidationBehavior.MinimumDecimalPlacesProperty, 0);
			numericValidationBehavior4.SetValue(NumericValidationBehavior.MinimumValueProperty, 0.0);
			((ICollection<Behavior>)entry5.GetValue(VisualElement.BehaviorsProperty)).Add(numericValidationBehavior4);
			customCell9.SetValue(CustomCell.ContentProperty, entry5);
			section8.Add(customCell9);
			settingsView.Root.Add(section8);
			section9.SetValue(SectionBase.TitleProperty, "");
			bindingExtension26.Path = "Options";
			bindingExtension26.TypedBinding = new TypedBinding<A5ParametrizeV2, ObservableCollection<MQBAdaptationOption>>(delegate(A5ParametrizeV2 A_0)
			{
				if (A_0 != null)
				{
					return new ValueTuple<ObservableCollection<MQBAdaptationOption>, bool>(A_0.Options, true);
				}
				return default(ValueTuple<ObservableCollection<MQBAdaptationOption>, bool>);
			}, delegate(A5ParametrizeV2 A_0, ObservableCollection<MQBAdaptationOption> A_1)
			{
				if (A_0 != null)
				{
					A_0.Options = A_1;
					return;
				}
			}, new Tuple<Func<A5ParametrizeV2, object>, string>[]
			{
				new Tuple<Func<A5ParametrizeV2, object>, string>((A5ParametrizeV2 A_0) => A_0, "Options")
			});
			BindingBase bindingBase26 = bindingExtension26.ProvideValue(null);
			section9.SetBinding(Section.ItemsSourceProperty, bindingBase26);
			IDataTemplate dataTemplate2 = dataTemplate;
			CodingA5CustomizationPage.<InitializeComponent>_anonXamlCDataTemplate_1 <InitializeComponent>_anonXamlCDataTemplate_ = new CodingA5CustomizationPage.<InitializeComponent>_anonXamlCDataTemplate_1();
			object[] array27 = new object[0 + 5];
			array27[0] = dataTemplate;
			array27[1] = section9;
			array27[2] = settingsView;
			array27[3] = grid2;
			array27[4] = this;
			<InitializeComponent>_anonXamlCDataTemplate_.parentValues = array27;
			<InitializeComponent>_anonXamlCDataTemplate_.root = this;
			dataTemplate2.LoadTemplate = new Func<object>(<InitializeComponent>_anonXamlCDataTemplate_.LoadDataTemplate);
			section9.SetValue(Section.ItemTemplateProperty, dataTemplate);
			settingsView.Root.Add(section9);
			translate14.Text = "coding_VagOpenHoodWarning";
			IMarkupExtension markupExtension27 = translate14;
			XamlServiceProvider xamlServiceProvider27 = new XamlServiceProvider();
			Type typeFromHandle53 = typeof(IProvideValueTarget);
			object[] array28 = new object[0 + 5];
			array28[0] = labelCell4;
			array28[1] = section10;
			array28[2] = settingsView;
			array28[3] = grid2;
			array28[4] = this;
			object obj49;
			xamlServiceProvider27.Add(typeFromHandle53, obj49 = new SimpleValueTargetProvider(array28, CellBase.TitleProperty, nameScope));
			xamlServiceProvider27.Add(typeof(IReferenceProvider), obj49);
			Type typeFromHandle54 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver27 = new XmlNamespaceResolver();
			xmlNamespaceResolver27.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver27.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver27.Add("coding", "clr-namespace:CarScannerXamarinForms.Coding");
			xmlNamespaceResolver27.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver27.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver27.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver27.Add("mqb", "clr-namespace:CarScannerXamarinForms.Coding.DB.MQB");
			xmlNamespaceResolver27.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver27.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xmlNamespaceResolver27.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver27.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider27.Add(typeFromHandle54, new XamlTypeResolver(xmlNamespaceResolver27, typeof(CodingA5CustomizationPage).GetTypeInfo().Assembly));
			xamlServiceProvider27.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(176, 35)));
			object obj50 = markupExtension27.ProvideValue(xamlServiceProvider27);
			labelCell4.Title = obj50;
			labelCell4.SetValue(CellBase.TitleFontAttributesProperty, new FontAttributes?(1));
			section10.Add(labelCell4);
			settingsView.Root.Add(section10);
			grid2.Children.Add(settingsView);
			activityFrame.SetValue(Grid.RowProperty, 0);
			activityFrame.SetValue(VisualElement.IsVisibleProperty, new VisualElement.VisibilityConverter().ConvertFromInvariantString("False"));
			grid2.Children.Add(activityFrame);
			this.SetValue(ContentPage.ContentProperty, grid2);
		}

		// Token: 0x06004FEE RID: 20462 RVA: 0x003DB12B File Offset: 0x003D932B
		[CompilerGenerated]
		private void <CodingDetailsPage_Appearing>b__3_0(string s)
		{
			MainThread.BeginInvokeOnMainThread(delegate
			{
				this.activityFrame.Text = ActivityFrame.PLEASE_WAIT_TEXT + "\n" + s;
			});
		}

		// Token: 0x06004FEF RID: 20463 RVA: 0x003DB150 File Offset: 0x003D9350
		[CompilerGenerated]
		private void <UpdateState>b__4_0(string s)
		{
			MainThread.BeginInvokeOnMainThread(delegate
			{
				this.activityFrame.Text = ActivityFrame.PLEASE_WAIT_TEXT + "\n" + s;
			});
		}

		// Token: 0x06004FF0 RID: 20464 RVA: 0x003DB175 File Offset: 0x003D9375
		[CompilerGenerated]
		private void <optionCell_Tapped>b__6_0(string s)
		{
			Device.BeginInvokeOnMainThread(delegate
			{
				this.activityFrame.Text = s;
			});
		}

		// Token: 0x06004FF1 RID: 20465 RVA: 0x003DB19C File Offset: 0x003D939C
		private void __InitComponentRuntime()
		{
			Extensions.LoadFromXaml<CodingA5CustomizationPage>(this, typeof(CodingA5CustomizationPage));
			this.page = NameScopeExtensions.FindByName<ContentPage>(this, "page");
			this.lv = NameScopeExtensions.FindByName<SettingsView>(this, "lv");
			this.entryPassword = NameScopeExtensions.FindByName<Entry>(this, "entryPassword");
			this.btnUpdateState = NameScopeExtensions.FindByName<ButtonCell>(this, "btnUpdateState");
			this.laneAssistPanel = NameScopeExtensions.FindByName<Section>(this, "laneAssistPanel");
			this.entryLaneAssistTimer = NameScopeExtensions.FindByName<Entry>(this, "entryLaneAssistTimer");
			this.optionsPanel = NameScopeExtensions.FindByName<Section>(this, "optionsPanel");
			this.activityFrame = NameScopeExtensions.FindByName<ActivityFrame>(this, "activityFrame");
		}

		// Token: 0x06004FF2 RID: 20466 RVA: 0x003DB244 File Offset: 0x003D9444
		[CompilerGenerated]
		private static ValueTuple<string, bool> <InitializeComponent>typedBindingsM__0(A5ParametrizeV2 A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<string, bool>(A_0.Name, true);
			}
			return default(ValueTuple<string, bool>);
		}

		// Token: 0x06004FF3 RID: 20467 RVA: 0x003DB274 File Offset: 0x003D9474
		[CompilerGenerated]
		private static void <InitializeComponent>typedBindingsM__1(A5ParametrizeV2 A_0, string A_1)
		{
			if (A_0 != null)
			{
				A_0.Name = A_1;
				return;
			}
		}

		// Token: 0x06004FF4 RID: 20468 RVA: 0x003DB290 File Offset: 0x003D9490
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__2(A5ParametrizeV2 A_0)
		{
			return A_0;
		}

		// Token: 0x06004FF5 RID: 20469 RVA: 0x003DB2A0 File Offset: 0x003D94A0
		[CompilerGenerated]
		private static ValueTuple<string, bool> <InitializeComponent>typedBindingsM__3(A5ParametrizeV2 A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<string, bool>(A_0.Description, true);
			}
			return default(ValueTuple<string, bool>);
		}

		// Token: 0x06004FF6 RID: 20470 RVA: 0x003DB2D0 File Offset: 0x003D94D0
		[CompilerGenerated]
		private static void <InitializeComponent>typedBindingsM__4(A5ParametrizeV2 A_0, string A_1)
		{
			if (A_0 != null)
			{
				A_0.Description = A_1;
				return;
			}
		}

		// Token: 0x06004FF7 RID: 20471 RVA: 0x003DB2EC File Offset: 0x003D94EC
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__5(A5ParametrizeV2 A_0)
		{
			return A_0;
		}

		// Token: 0x06004FF8 RID: 20472 RVA: 0x003DB2FC File Offset: 0x003D94FC
		[CompilerGenerated]
		private static ValueTuple<string, bool> <InitializeComponent>typedBindingsM__6(A5ParametrizeV2 A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<string, bool>(A_0.Description, true);
			}
			return default(ValueTuple<string, bool>);
		}

		// Token: 0x06004FF9 RID: 20473 RVA: 0x003DB32C File Offset: 0x003D952C
		[CompilerGenerated]
		private static void <InitializeComponent>typedBindingsM__7(A5ParametrizeV2 A_0, string A_1)
		{
			if (A_0 != null)
			{
				A_0.Description = A_1;
				return;
			}
		}

		// Token: 0x06004FFA RID: 20474 RVA: 0x003DB348 File Offset: 0x003D9548
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__8(A5ParametrizeV2 A_0)
		{
			return A_0;
		}

		// Token: 0x06004FFB RID: 20475 RVA: 0x003DB358 File Offset: 0x003D9558
		[CompilerGenerated]
		private static ValueTuple<string, bool> <InitializeComponent>typedBindingsM__9(A5ParametrizeV2 A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<string, bool>(A_0.InnerDescription, true);
			}
			return default(ValueTuple<string, bool>);
		}

		// Token: 0x06004FFC RID: 20476 RVA: 0x003DB388 File Offset: 0x003D9588
		[CompilerGenerated]
		private static void <InitializeComponent>typedBindingsM__10(A5ParametrizeV2 A_0, string A_1)
		{
			if (A_0 != null)
			{
				A_0.InnerDescription = A_1;
				return;
			}
		}

		// Token: 0x06004FFD RID: 20477 RVA: 0x003DB3A4 File Offset: 0x003D95A4
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__11(A5ParametrizeV2 A_0)
		{
			return A_0;
		}

		// Token: 0x06004FFE RID: 20478 RVA: 0x003DB3B4 File Offset: 0x003D95B4
		[CompilerGenerated]
		private static ValueTuple<string, bool> <InitializeComponent>typedBindingsM__12(A5ParametrizeV2 A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<string, bool>(A_0.InnerDescription, true);
			}
			return default(ValueTuple<string, bool>);
		}

		// Token: 0x06004FFF RID: 20479 RVA: 0x003DB3E4 File Offset: 0x003D95E4
		[CompilerGenerated]
		private static void <InitializeComponent>typedBindingsM__13(A5ParametrizeV2 A_0, string A_1)
		{
			if (A_0 != null)
			{
				A_0.InnerDescription = A_1;
				return;
			}
		}

		// Token: 0x06005000 RID: 20480 RVA: 0x003DB400 File Offset: 0x003D9600
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__14(A5ParametrizeV2 A_0)
		{
			return A_0;
		}

		// Token: 0x06005001 RID: 20481 RVA: 0x003DB410 File Offset: 0x003D9610
		[CompilerGenerated]
		private static ValueTuple<bool, bool> <InitializeComponent>typedBindingsM__15(A5ParametrizeV2 A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<bool, bool>(A_0.PasswordVisible, true);
			}
			return default(ValueTuple<bool, bool>);
		}

		// Token: 0x06005002 RID: 20482 RVA: 0x003DB440 File Offset: 0x003D9640
		[CompilerGenerated]
		private static void <InitializeComponent>typedBindingsM__16(A5ParametrizeV2 A_0, bool A_1)
		{
			if (A_0 != null)
			{
				A_0.PasswordVisible = A_1;
				return;
			}
		}

		// Token: 0x06005003 RID: 20483 RVA: 0x003DB45C File Offset: 0x003D965C
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__17(A5ParametrizeV2 A_0)
		{
			return A_0;
		}

		// Token: 0x06005004 RID: 20484 RVA: 0x003DB46C File Offset: 0x003D966C
		[CompilerGenerated]
		private static ValueTuple<string, bool> <InitializeComponent>typedBindingsM__18(A5ParametrizeV2 A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<string, bool>(A_0.Password, true);
			}
			return default(ValueTuple<string, bool>);
		}

		// Token: 0x06005005 RID: 20485 RVA: 0x003DB49C File Offset: 0x003D969C
		[CompilerGenerated]
		private static void <InitializeComponent>typedBindingsM__19(A5ParametrizeV2 A_0, string A_1)
		{
			if (A_0 != null)
			{
				A_0.Password = A_1;
				return;
			}
		}

		// Token: 0x06005006 RID: 20486 RVA: 0x003DB4B8 File Offset: 0x003D96B8
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__20(A5ParametrizeV2 A_0)
		{
			return A_0;
		}

		// Token: 0x06005007 RID: 20487 RVA: 0x003DB4C8 File Offset: 0x003D96C8
		[CompilerGenerated]
		private static ValueTuple<string, bool> <InitializeComponent>typedBindingsM__21(A5ParametrizeV2 A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<string, bool>(A_0.PasswordHint, true);
			}
			return default(ValueTuple<string, bool>);
		}

		// Token: 0x06005008 RID: 20488 RVA: 0x003DB4F8 File Offset: 0x003D96F8
		[CompilerGenerated]
		private static void <InitializeComponent>typedBindingsM__22(A5ParametrizeV2 A_0, string A_1)
		{
			if (A_0 != null)
			{
				A_0.PasswordHint = A_1;
				return;
			}
		}

		// Token: 0x06005009 RID: 20489 RVA: 0x003DB514 File Offset: 0x003D9714
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__23(A5ParametrizeV2 A_0)
		{
			return A_0;
		}

		// Token: 0x0600500A RID: 20490 RVA: 0x003DB524 File Offset: 0x003D9724
		[CompilerGenerated]
		private static ValueTuple<string, bool> <InitializeComponent>typedBindingsM__24(A5ParametrizeV2 A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<string, bool>(A_0.PasswordHint, true);
			}
			return default(ValueTuple<string, bool>);
		}

		// Token: 0x0600500B RID: 20491 RVA: 0x003DB554 File Offset: 0x003D9754
		[CompilerGenerated]
		private static void <InitializeComponent>typedBindingsM__25(A5ParametrizeV2 A_0, string A_1)
		{
			if (A_0 != null)
			{
				A_0.PasswordHint = A_1;
				return;
			}
		}

		// Token: 0x0600500C RID: 20492 RVA: 0x003DB570 File Offset: 0x003D9770
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__26(A5ParametrizeV2 A_0)
		{
			return A_0;
		}

		// Token: 0x0600500D RID: 20493 RVA: 0x003DB580 File Offset: 0x003D9780
		[CompilerGenerated]
		private static ValueTuple<string, bool> <InitializeComponent>typedBindingsM__27(A5ParametrizeV2 A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<string, bool>(A_0.Password, true);
			}
			return default(ValueTuple<string, bool>);
		}

		// Token: 0x0600500E RID: 20494 RVA: 0x003DB5B0 File Offset: 0x003D97B0
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__28(A5ParametrizeV2 A_0)
		{
			return A_0;
		}

		// Token: 0x0600500F RID: 20495 RVA: 0x003DB5C0 File Offset: 0x003D97C0
		[CompilerGenerated]
		private static ValueTuple<bool, bool> <InitializeComponent>typedBindingsM__29(A5ParametrizeV2 A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<bool, bool>(A_0.HasCurrentState, true);
			}
			return default(ValueTuple<bool, bool>);
		}

		// Token: 0x06005010 RID: 20496 RVA: 0x003DB5F0 File Offset: 0x003D97F0
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__30(A5ParametrizeV2 A_0)
		{
			return A_0;
		}

		// Token: 0x06005011 RID: 20497 RVA: 0x003DB600 File Offset: 0x003D9800
		[CompilerGenerated]
		private static ValueTuple<string, bool> <InitializeComponent>typedBindingsM__31(A5ParametrizeV2 A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<string, bool>(A_0.CurrentState, true);
			}
			return default(ValueTuple<string, bool>);
		}

		// Token: 0x06005012 RID: 20498 RVA: 0x003DB630 File Offset: 0x003D9830
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__32(A5ParametrizeV2 A_0)
		{
			return A_0;
		}

		// Token: 0x06005013 RID: 20499 RVA: 0x003DB640 File Offset: 0x003D9840
		[CompilerGenerated]
		private static ValueTuple<string, bool> <InitializeComponent>typedBindingsM__33(A5ParametrizeV2 A_0)
		{
			if (A_0 != null)
			{
				ProxyFile selectedDataset = A_0.SelectedDataset;
				if (selectedDataset != null)
				{
					return new ValueTuple<string, bool>(selectedDataset.Title, true);
				}
			}
			return default(ValueTuple<string, bool>);
		}

		// Token: 0x06005014 RID: 20500 RVA: 0x003DB678 File Offset: 0x003D9878
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__34(A5ParametrizeV2 A_0)
		{
			return A_0;
		}

		// Token: 0x06005015 RID: 20501 RVA: 0x003DB688 File Offset: 0x003D9888
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__35(A5ParametrizeV2 A_0)
		{
			return A_0.SelectedDataset;
		}

		// Token: 0x06005016 RID: 20502 RVA: 0x003DB69C File Offset: 0x003D989C
		[CompilerGenerated]
		private static ValueTuple<ObservableCollection<ProxyFile>, bool> <InitializeComponent>typedBindingsM__36(A5ParametrizeV2 A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<ObservableCollection<ProxyFile>, bool>(A_0.AvailableDatasets, true);
			}
			return default(ValueTuple<ObservableCollection<ProxyFile>, bool>);
		}

		// Token: 0x06005017 RID: 20503 RVA: 0x003DB6CC File Offset: 0x003D98CC
		[CompilerGenerated]
		private static void <InitializeComponent>typedBindingsM__37(A5ParametrizeV2 A_0, ObservableCollection<ProxyFile> A_1)
		{
			if (A_0 != null)
			{
				A_0.AvailableDatasets = A_1;
				return;
			}
		}

		// Token: 0x06005018 RID: 20504 RVA: 0x003DB6E8 File Offset: 0x003D98E8
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__38(A5ParametrizeV2 A_0)
		{
			return A_0;
		}

		// Token: 0x06005019 RID: 20505 RVA: 0x003DB6F8 File Offset: 0x003D98F8
		[CompilerGenerated]
		private static ValueTuple<ProxyFile, bool> <InitializeComponent>typedBindingsM__39(A5ParametrizeV2 A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<ProxyFile, bool>(A_0.SelectedDataset, true);
			}
			return default(ValueTuple<ProxyFile, bool>);
		}

		// Token: 0x0600501A RID: 20506 RVA: 0x003DB728 File Offset: 0x003D9928
		[CompilerGenerated]
		private static void <InitializeComponent>typedBindingsM__40(A5ParametrizeV2 A_0, ProxyFile A_1)
		{
			if (A_0 != null)
			{
				A_0.SelectedDataset = A_1;
				return;
			}
		}

		// Token: 0x0600501B RID: 20507 RVA: 0x003DB744 File Offset: 0x003D9944
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__41(A5ParametrizeV2 A_0)
		{
			return A_0;
		}

		// Token: 0x0600501C RID: 20508 RVA: 0x003DB754 File Offset: 0x003D9954
		[CompilerGenerated]
		private static ValueTuple<bool, bool> <InitializeComponent>typedBindingsM__42(A5ParametrizeV2 A_0)
		{
			if (A_0 != null)
			{
				A5DatasetCustomizer customizer = A_0.Customizer;
				if (customizer != null)
				{
					return new ValueTuple<bool, bool>(customizer.LaneAssistTimerConfigurationAvailable, true);
				}
			}
			return default(ValueTuple<bool, bool>);
		}

		// Token: 0x0600501D RID: 20509 RVA: 0x003DB78C File Offset: 0x003D998C
		[CompilerGenerated]
		private static void <InitializeComponent>typedBindingsM__43(A5ParametrizeV2 A_0, bool A_1)
		{
			if (A_0 != null)
			{
				A5DatasetCustomizer customizer = A_0.Customizer;
				if (customizer != null)
				{
					customizer.LaneAssistTimerConfigurationAvailable = A_1;
					return;
				}
			}
		}

		// Token: 0x0600501E RID: 20510 RVA: 0x003DB7B4 File Offset: 0x003D99B4
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__44(A5ParametrizeV2 A_0)
		{
			return A_0;
		}

		// Token: 0x0600501F RID: 20511 RVA: 0x003DB7C4 File Offset: 0x003D99C4
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__45(A5ParametrizeV2 A_0)
		{
			return A_0.Customizer;
		}

		// Token: 0x06005020 RID: 20512 RVA: 0x003DB7D8 File Offset: 0x003D99D8
		[CompilerGenerated]
		private static ValueTuple<int, bool> <InitializeComponent>typedBindingsM__46(A5ParametrizeV2 A_0)
		{
			if (A_0 != null)
			{
				A5DatasetCustomizer customizer = A_0.Customizer;
				if (customizer != null)
				{
					return new ValueTuple<int, bool>(customizer.LaneAssistTimer, true);
				}
			}
			return default(ValueTuple<int, bool>);
		}

		// Token: 0x06005021 RID: 20513 RVA: 0x003DB810 File Offset: 0x003D9A10
		[CompilerGenerated]
		private static void <InitializeComponent>typedBindingsM__47(A5ParametrizeV2 A_0, int A_1)
		{
			if (A_0 != null)
			{
				A5DatasetCustomizer customizer = A_0.Customizer;
				if (customizer != null)
				{
					customizer.LaneAssistTimer = A_1;
					return;
				}
			}
		}

		// Token: 0x06005022 RID: 20514 RVA: 0x003DB838 File Offset: 0x003D9A38
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__48(A5ParametrizeV2 A_0)
		{
			return A_0;
		}

		// Token: 0x06005023 RID: 20515 RVA: 0x003DB848 File Offset: 0x003D9A48
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__49(A5ParametrizeV2 A_0)
		{
			return A_0.Customizer;
		}

		// Token: 0x06005024 RID: 20516 RVA: 0x003DB85C File Offset: 0x003D9A5C
		[CompilerGenerated]
		private static ValueTuple<bool, bool> <InitializeComponent>typedBindingsM__50(A5ParametrizeV2 A_0)
		{
			if (A_0 != null)
			{
				A5DatasetCustomizer customizer = A_0.Customizer;
				if (customizer != null)
				{
					return new ValueTuple<bool, bool>(customizer.LaneAssistStartSpeedConfigurationAvailable, true);
				}
			}
			return default(ValueTuple<bool, bool>);
		}

		// Token: 0x06005025 RID: 20517 RVA: 0x003DB894 File Offset: 0x003D9A94
		[CompilerGenerated]
		private static void <InitializeComponent>typedBindingsM__51(A5ParametrizeV2 A_0, bool A_1)
		{
			if (A_0 != null)
			{
				A5DatasetCustomizer customizer = A_0.Customizer;
				if (customizer != null)
				{
					customizer.LaneAssistStartSpeedConfigurationAvailable = A_1;
					return;
				}
			}
		}

		// Token: 0x06005026 RID: 20518 RVA: 0x003DB8BC File Offset: 0x003D9ABC
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__52(A5ParametrizeV2 A_0)
		{
			return A_0;
		}

		// Token: 0x06005027 RID: 20519 RVA: 0x003DB8CC File Offset: 0x003D9ACC
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__53(A5ParametrizeV2 A_0)
		{
			return A_0.Customizer;
		}

		// Token: 0x06005028 RID: 20520 RVA: 0x003DB8E0 File Offset: 0x003D9AE0
		[CompilerGenerated]
		private static ValueTuple<int, bool> <InitializeComponent>typedBindingsM__54(A5ParametrizeV2 A_0)
		{
			if (A_0 != null)
			{
				A5DatasetCustomizer customizer = A_0.Customizer;
				if (customizer != null)
				{
					return new ValueTuple<int, bool>(customizer.LaneAssistStartSpeed, true);
				}
			}
			return default(ValueTuple<int, bool>);
		}

		// Token: 0x06005029 RID: 20521 RVA: 0x003DB918 File Offset: 0x003D9B18
		[CompilerGenerated]
		private static void <InitializeComponent>typedBindingsM__55(A5ParametrizeV2 A_0, int A_1)
		{
			if (A_0 != null)
			{
				A5DatasetCustomizer customizer = A_0.Customizer;
				if (customizer != null)
				{
					customizer.LaneAssistStartSpeed = A_1;
					return;
				}
			}
		}

		// Token: 0x0600502A RID: 20522 RVA: 0x003DB940 File Offset: 0x003D9B40
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__56(A5ParametrizeV2 A_0)
		{
			return A_0;
		}

		// Token: 0x0600502B RID: 20523 RVA: 0x003DB950 File Offset: 0x003D9B50
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__57(A5ParametrizeV2 A_0)
		{
			return A_0.Customizer;
		}

		// Token: 0x0600502C RID: 20524 RVA: 0x003DB964 File Offset: 0x003D9B64
		[CompilerGenerated]
		private static ValueTuple<bool, bool> <InitializeComponent>typedBindingsM__58(A5ParametrizeV2 A_0)
		{
			if (A_0 != null)
			{
				A5DatasetCustomizer customizer = A_0.Customizer;
				if (customizer != null)
				{
					return new ValueTuple<bool, bool>(customizer.TJAConfigurationAvailable, true);
				}
			}
			return default(ValueTuple<bool, bool>);
		}

		// Token: 0x0600502D RID: 20525 RVA: 0x003DB99C File Offset: 0x003D9B9C
		[CompilerGenerated]
		private static void <InitializeComponent>typedBindingsM__59(A5ParametrizeV2 A_0, bool A_1)
		{
			if (A_0 != null)
			{
				A5DatasetCustomizer customizer = A_0.Customizer;
				if (customizer != null)
				{
					customizer.TJAConfigurationAvailable = A_1;
					return;
				}
			}
		}

		// Token: 0x0600502E RID: 20526 RVA: 0x003DB9C4 File Offset: 0x003D9BC4
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__60(A5ParametrizeV2 A_0)
		{
			return A_0;
		}

		// Token: 0x0600502F RID: 20527 RVA: 0x003DB9D4 File Offset: 0x003D9BD4
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__61(A5ParametrizeV2 A_0)
		{
			return A_0.Customizer;
		}

		// Token: 0x06005030 RID: 20528 RVA: 0x003DB9E8 File Offset: 0x003D9BE8
		[CompilerGenerated]
		private static ValueTuple<bool, bool> <InitializeComponent>typedBindingsM__62(A5ParametrizeV2 A_0)
		{
			if (A_0 != null)
			{
				A5DatasetCustomizer customizer = A_0.Customizer;
				if (customizer != null)
				{
					return new ValueTuple<bool, bool>(customizer.TJA, true);
				}
			}
			return default(ValueTuple<bool, bool>);
		}

		// Token: 0x06005031 RID: 20529 RVA: 0x003DBA20 File Offset: 0x003D9C20
		[CompilerGenerated]
		private static void <InitializeComponent>typedBindingsM__63(A5ParametrizeV2 A_0, bool A_1)
		{
			if (A_0 != null)
			{
				A5DatasetCustomizer customizer = A_0.Customizer;
				if (customizer != null)
				{
					customizer.TJA = A_1;
					return;
				}
			}
		}

		// Token: 0x06005032 RID: 20530 RVA: 0x003DBA48 File Offset: 0x003D9C48
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__64(A5ParametrizeV2 A_0)
		{
			return A_0;
		}

		// Token: 0x06005033 RID: 20531 RVA: 0x003DBA58 File Offset: 0x003D9C58
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__65(A5ParametrizeV2 A_0)
		{
			return A_0.Customizer;
		}

		// Token: 0x06005034 RID: 20532 RVA: 0x003DBA6C File Offset: 0x003D9C6C
		[CompilerGenerated]
		private static ValueTuple<bool, bool> <InitializeComponent>typedBindingsM__66(A5ParametrizeV2 A_0)
		{
			if (A_0 != null)
			{
				A5DatasetCustomizer customizer = A_0.Customizer;
				if (customizer != null)
				{
					return new ValueTuple<bool, bool>(customizer.LightAssistSpeedConfigurationAvailable, true);
				}
			}
			return default(ValueTuple<bool, bool>);
		}

		// Token: 0x06005035 RID: 20533 RVA: 0x003DBAA4 File Offset: 0x003D9CA4
		[CompilerGenerated]
		private static void <InitializeComponent>typedBindingsM__67(A5ParametrizeV2 A_0, bool A_1)
		{
			if (A_0 != null)
			{
				A5DatasetCustomizer customizer = A_0.Customizer;
				if (customizer != null)
				{
					customizer.LightAssistSpeedConfigurationAvailable = A_1;
					return;
				}
			}
		}

		// Token: 0x06005036 RID: 20534 RVA: 0x003DBACC File Offset: 0x003D9CCC
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__68(A5ParametrizeV2 A_0)
		{
			return A_0;
		}

		// Token: 0x06005037 RID: 20535 RVA: 0x003DBADC File Offset: 0x003D9CDC
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__69(A5ParametrizeV2 A_0)
		{
			return A_0.Customizer;
		}

		// Token: 0x06005038 RID: 20536 RVA: 0x003DBAF0 File Offset: 0x003D9CF0
		[CompilerGenerated]
		private static ValueTuple<int, bool> <InitializeComponent>typedBindingsM__70(A5ParametrizeV2 A_0)
		{
			if (A_0 != null)
			{
				A5DatasetCustomizer customizer = A_0.Customizer;
				if (customizer != null)
				{
					return new ValueTuple<int, bool>(customizer.LightAssistOnSpeed, true);
				}
			}
			return default(ValueTuple<int, bool>);
		}

		// Token: 0x06005039 RID: 20537 RVA: 0x003DBB28 File Offset: 0x003D9D28
		[CompilerGenerated]
		private static void <InitializeComponent>typedBindingsM__71(A5ParametrizeV2 A_0, int A_1)
		{
			if (A_0 != null)
			{
				A5DatasetCustomizer customizer = A_0.Customizer;
				if (customizer != null)
				{
					customizer.LightAssistOnSpeed = A_1;
					return;
				}
			}
		}

		// Token: 0x0600503A RID: 20538 RVA: 0x003DBB50 File Offset: 0x003D9D50
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__72(A5ParametrizeV2 A_0)
		{
			return A_0;
		}

		// Token: 0x0600503B RID: 20539 RVA: 0x003DBB60 File Offset: 0x003D9D60
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__73(A5ParametrizeV2 A_0)
		{
			return A_0.Customizer;
		}

		// Token: 0x0600503C RID: 20540 RVA: 0x003DBB74 File Offset: 0x003D9D74
		[CompilerGenerated]
		private static ValueTuple<int, bool> <InitializeComponent>typedBindingsM__74(A5ParametrizeV2 A_0)
		{
			if (A_0 != null)
			{
				A5DatasetCustomizer customizer = A_0.Customizer;
				if (customizer != null)
				{
					return new ValueTuple<int, bool>(customizer.LightAssistOffSpeed, true);
				}
			}
			return default(ValueTuple<int, bool>);
		}

		// Token: 0x0600503D RID: 20541 RVA: 0x003DBBAC File Offset: 0x003D9DAC
		[CompilerGenerated]
		private static void <InitializeComponent>typedBindingsM__75(A5ParametrizeV2 A_0, int A_1)
		{
			if (A_0 != null)
			{
				A5DatasetCustomizer customizer = A_0.Customizer;
				if (customizer != null)
				{
					customizer.LightAssistOffSpeed = A_1;
					return;
				}
			}
		}

		// Token: 0x0600503E RID: 20542 RVA: 0x003DBBD4 File Offset: 0x003D9DD4
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__76(A5ParametrizeV2 A_0)
		{
			return A_0;
		}

		// Token: 0x0600503F RID: 20543 RVA: 0x003DBBE4 File Offset: 0x003D9DE4
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__77(A5ParametrizeV2 A_0)
		{
			return A_0.Customizer;
		}

		// Token: 0x06005040 RID: 20544 RVA: 0x003DBBF8 File Offset: 0x003D9DF8
		[CompilerGenerated]
		private static ValueTuple<ObservableCollection<MQBAdaptationOption>, bool> <InitializeComponent>typedBindingsM__78(A5ParametrizeV2 A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<ObservableCollection<MQBAdaptationOption>, bool>(A_0.Options, true);
			}
			return default(ValueTuple<ObservableCollection<MQBAdaptationOption>, bool>);
		}

		// Token: 0x06005041 RID: 20545 RVA: 0x003DBC28 File Offset: 0x003D9E28
		[CompilerGenerated]
		private static void <InitializeComponent>typedBindingsM__79(A5ParametrizeV2 A_0, ObservableCollection<MQBAdaptationOption> A_1)
		{
			if (A_0 != null)
			{
				A_0.Options = A_1;
				return;
			}
		}

		// Token: 0x06005042 RID: 20546 RVA: 0x003DBC44 File Offset: 0x003D9E44
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__80(A5ParametrizeV2 A_0)
		{
			return A_0;
		}

		// Token: 0x04003002 RID: 12290
		private A5ParametrizeV2 coding;

		// Token: 0x04003003 RID: 12291
		private bool first_appearing = true;

		// Token: 0x04003004 RID: 12292
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private ContentPage page;

		// Token: 0x04003005 RID: 12293
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private SettingsView lv;

		// Token: 0x04003006 RID: 12294
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private Entry entryPassword;

		// Token: 0x04003007 RID: 12295
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private ButtonCell btnUpdateState;

		// Token: 0x04003008 RID: 12296
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private Section laneAssistPanel;

		// Token: 0x04003009 RID: 12297
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private Entry entryLaneAssistTimer;

		// Token: 0x0400300A RID: 12298
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private Section optionsPanel;

		// Token: 0x0400300B RID: 12299
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private ActivityFrame activityFrame;

		// Token: 0x02000981 RID: 2433
		[CompilerGenerated]
		private sealed class <>c__DisplayClass3_0
		{
			// Token: 0x06005043 RID: 20547 RVA: 0x00002050 File Offset: 0x00000250
			public <>c__DisplayClass3_0()
			{
			}

			// Token: 0x06005044 RID: 20548 RVA: 0x003DBC52 File Offset: 0x003D9E52
			internal void <CodingDetailsPage_Appearing>b__1()
			{
				this.<>4__this.activityFrame.Text = ActivityFrame.PLEASE_WAIT_TEXT + "\n" + this.s;
			}

			// Token: 0x0400300C RID: 12300
			public string s;

			// Token: 0x0400300D RID: 12301
			public CodingA5CustomizationPage <>4__this;
		}

		// Token: 0x02000982 RID: 2434
		[CompilerGenerated]
		private sealed class <>c__DisplayClass4_0
		{
			// Token: 0x06005045 RID: 20549 RVA: 0x00002050 File Offset: 0x00000250
			public <>c__DisplayClass4_0()
			{
			}

			// Token: 0x06005046 RID: 20550 RVA: 0x003DBC79 File Offset: 0x003D9E79
			internal void <UpdateState>b__1()
			{
				this.<>4__this.activityFrame.Text = ActivityFrame.PLEASE_WAIT_TEXT + "\n" + this.s;
			}

			// Token: 0x0400300E RID: 12302
			public string s;

			// Token: 0x0400300F RID: 12303
			public CodingA5CustomizationPage <>4__this;
		}

		// Token: 0x02000983 RID: 2435
		[CompilerGenerated]
		private sealed class <>c__DisplayClass6_0
		{
			// Token: 0x06005047 RID: 20551 RVA: 0x00002050 File Offset: 0x00000250
			public <>c__DisplayClass6_0()
			{
			}

			// Token: 0x06005048 RID: 20552 RVA: 0x003DBCA0 File Offset: 0x003D9EA0
			internal void <optionCell_Tapped>b__1()
			{
				this.<>4__this.activityFrame.Text = this.s;
			}

			// Token: 0x04003010 RID: 12304
			public string s;

			// Token: 0x04003011 RID: 12305
			public CodingA5CustomizationPage <>4__this;
		}

		// Token: 0x02000984 RID: 2436
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <BtnUpdateState_Clicked>d__5 : IAsyncStateMachine
		{
			// Token: 0x06005049 RID: 20553 RVA: 0x003DBCB8 File Offset: 0x003D9EB8
			void IAsyncStateMachine.MoveNext()
			{
				CodingA5CustomizationPage codingA5CustomizationPage = this;
				try
				{
					codingA5CustomizationPage.UpdateState();
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

			// Token: 0x0600504A RID: 20554 RVA: 0x003DBD10 File Offset: 0x003D9F10
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x04003012 RID: 12306
			public int <>1__state;

			// Token: 0x04003013 RID: 12307
			public AsyncVoidMethodBuilder <>t__builder;

			// Token: 0x04003014 RID: 12308
			public CodingA5CustomizationPage <>4__this;
		}

		// Token: 0x02000985 RID: 2437
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <CodingDetailsPage_Appearing>d__3 : IAsyncStateMachine
		{
			// Token: 0x0600504B RID: 20555 RVA: 0x003DBD20 File Offset: 0x003D9F20
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				CodingA5CustomizationPage codingA5CustomizationPage = this;
				try
				{
					TaskAwaiter<CodingRequestResult> taskAwaiter;
					if (num != 0)
					{
						if (!codingA5CustomizationPage.first_appearing)
						{
							goto IL_00D8;
						}
						codingA5CustomizationPage.first_appearing = false;
						codingA5CustomizationPage.lv.IsEnabled = false;
						codingA5CustomizationPage.activityFrame.IsVisible = true;
						Progress<string> progress = new Progress<string>(delegate(string s)
						{
							MainThread.BeginInvokeOnMainThread(new Action(new CodingA5CustomizationPage.<>c__DisplayClass3_0
							{
								<>4__this = codingA5CustomizationPage,
								s = s
							}.<CodingDetailsPage_Appearing>b__1));
						});
						if (App.OBDSimulator.IsActive)
						{
							goto IL_00C0;
						}
						taskAwaiter = codingA5CustomizationPage.coding.UpdateCurrentState("", progress).GetAwaiter();
						if (!taskAwaiter.IsCompleted)
						{
							num2 = 0;
							TaskAwaiter<CodingRequestResult> taskAwaiter2 = taskAwaiter;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<CodingRequestResult>, CodingA5CustomizationPage.<CodingDetailsPage_Appearing>d__3>(ref taskAwaiter, ref this);
							return;
						}
					}
					else
					{
						TaskAwaiter<CodingRequestResult> taskAwaiter2;
						taskAwaiter = taskAwaiter2;
						taskAwaiter2 = default(TaskAwaiter<CodingRequestResult>);
						num2 = -1;
					}
					taskAwaiter.GetResult();
					IL_00C0:
					codingA5CustomizationPage.activityFrame.IsVisible = false;
					codingA5CustomizationPage.lv.IsEnabled = true;
					IL_00D8:;
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

			// Token: 0x0600504C RID: 20556 RVA: 0x003DBE44 File Offset: 0x003DA044
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x04003015 RID: 12309
			public int <>1__state;

			// Token: 0x04003016 RID: 12310
			public AsyncVoidMethodBuilder <>t__builder;

			// Token: 0x04003017 RID: 12311
			public CodingA5CustomizationPage <>4__this;

			// Token: 0x04003018 RID: 12312
			private TaskAwaiter<CodingRequestResult> <>u__1;
		}

		// Token: 0x02000986 RID: 2438
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <UpdateState>d__4 : IAsyncStateMachine
		{
			// Token: 0x0600504D RID: 20557 RVA: 0x003DBE54 File Offset: 0x003DA054
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				CodingA5CustomizationPage codingA5CustomizationPage = this;
				try
				{
					TaskAwaiter<CodingRequestResult> taskAwaiter;
					if (num != 0)
					{
						codingA5CustomizationPage.activityFrame.Text = ActivityFrame.PLEASE_WAIT_TEXT;
						codingA5CustomizationPage.activityFrame.IsVisible = true;
						codingA5CustomizationPage.lv.IsEnabled = false;
						Progress<string> progress = new Progress<string>(delegate(string s)
						{
							MainThread.BeginInvokeOnMainThread(new Action(new CodingA5CustomizationPage.<>c__DisplayClass4_0
							{
								<>4__this = codingA5CustomizationPage,
								s = s
							}.<UpdateState>b__1));
						});
						if (App.OBDSimulator.IsActive)
						{
							goto IL_00BE;
						}
						taskAwaiter = codingA5CustomizationPage.coding.UpdateCurrentState("", progress).GetAwaiter();
						if (!taskAwaiter.IsCompleted)
						{
							num2 = 0;
							TaskAwaiter<CodingRequestResult> taskAwaiter2 = taskAwaiter;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<CodingRequestResult>, CodingA5CustomizationPage.<UpdateState>d__4>(ref taskAwaiter, ref this);
							return;
						}
					}
					else
					{
						TaskAwaiter<CodingRequestResult> taskAwaiter2;
						taskAwaiter = taskAwaiter2;
						taskAwaiter2 = default(TaskAwaiter<CodingRequestResult>);
						num2 = -1;
					}
					taskAwaiter.GetResult();
					IL_00BE:
					codingA5CustomizationPage.activityFrame.IsVisible = false;
					codingA5CustomizationPage.lv.IsEnabled = true;
					codingA5CustomizationPage.activityFrame.Text = ActivityFrame.PLEASE_WAIT_TEXT;
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

			// Token: 0x0600504E RID: 20558 RVA: 0x003DBF88 File Offset: 0x003DA188
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x04003019 RID: 12313
			public int <>1__state;

			// Token: 0x0400301A RID: 12314
			public AsyncTaskMethodBuilder <>t__builder;

			// Token: 0x0400301B RID: 12315
			public CodingA5CustomizationPage <>4__this;

			// Token: 0x0400301C RID: 12316
			private TaskAwaiter<CodingRequestResult> <>u__1;
		}

		// Token: 0x02000987 RID: 2439
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <optionCell_Tapped>d__6 : IAsyncStateMachine
		{
			// Token: 0x0600504F RID: 20559 RVA: 0x003DBF98 File Offset: 0x003DA198
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				CodingA5CustomizationPage codingA5CustomizationPage = this;
				try
				{
					TaskAwaiter taskAwaiter3;
					TaskAwaiter<bool> taskAwaiter5;
					TaskAwaiter<CodingRequestResult> taskAwaiter6;
					switch (num)
					{
					case 0:
					{
						TaskAwaiter taskAwaiter4;
						taskAwaiter3 = taskAwaiter4;
						taskAwaiter4 = default(TaskAwaiter);
						num2 = -1;
						break;
					}
					case 1:
					{
						TaskAwaiter taskAwaiter4;
						taskAwaiter3 = taskAwaiter4;
						taskAwaiter4 = default(TaskAwaiter);
						num2 = -1;
						goto IL_0147;
					}
					case 2:
						taskAwaiter5 = taskAwaiter2;
						taskAwaiter2 = default(TaskAwaiter<bool>);
						num2 = -1;
						goto IL_01F8;
					case 3:
					{
						TaskAwaiter taskAwaiter4;
						taskAwaiter3 = taskAwaiter4;
						taskAwaiter4 = default(TaskAwaiter);
						num2 = -1;
						goto IL_0266;
					}
					case 4:
						taskAwaiter5 = taskAwaiter2;
						taskAwaiter2 = default(TaskAwaiter<bool>);
						num2 = -1;
						goto IL_0320;
					case 5:
					{
						TaskAwaiter taskAwaiter4;
						taskAwaiter3 = taskAwaiter4;
						taskAwaiter4 = default(TaskAwaiter);
						num2 = -1;
						goto IL_038E;
					}
					case 6:
					{
						TaskAwaiter<CodingRequestResult> taskAwaiter7;
						taskAwaiter6 = taskAwaiter7;
						taskAwaiter7 = default(TaskAwaiter<CodingRequestResult>);
						num2 = -1;
						goto IL_0444;
					}
					case 7:
					{
						TaskAwaiter taskAwaiter4;
						taskAwaiter3 = taskAwaiter4;
						taskAwaiter4 = default(TaskAwaiter);
						num2 = -1;
						goto IL_04EB;
					}
					case 8:
					{
						TaskAwaiter taskAwaiter4;
						taskAwaiter3 = taskAwaiter4;
						taskAwaiter4 = default(TaskAwaiter);
						num2 = -1;
						goto IL_0561;
					}
					case 9:
					{
						TaskAwaiter<CodingRequestResult> taskAwaiter7;
						taskAwaiter6 = taskAwaiter7;
						taskAwaiter7 = default(TaskAwaiter<CodingRequestResult>);
						num2 = -1;
						goto IL_05D0;
					}
					default:
						if (codingA5CustomizationPage.coding.SelectedDataset == null)
						{
							taskAwaiter3 = codingA5CustomizationPage.DisplayAlert("No dataset selected!", "", "OK").GetAwaiter();
							if (!taskAwaiter3.IsCompleted)
							{
								num2 = 0;
								TaskAwaiter taskAwaiter4 = taskAwaiter3;
								this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, CodingA5CustomizationPage.<optionCell_Tapped>d__6>(ref taskAwaiter3, ref this);
								return;
							}
						}
						else
						{
							int num3;
							if (int.TryParse(codingA5CustomizationPage.entryLaneAssistTimer.Text, NumberStyles.Integer, CultureInfo.InvariantCulture, out num3) && num3 >= 0 && num3 <= 65535)
							{
								goto IL_014E;
							}
							taskAwaiter3 = codingA5CustomizationPage.DisplayAlert("Wrong Lane assist timer format", "Min. = 0, Max = 65535, format example: 17", "OK").GetAwaiter();
							if (!taskAwaiter3.IsCompleted)
							{
								num2 = 1;
								TaskAwaiter taskAwaiter4 = taskAwaiter3;
								this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, CodingA5CustomizationPage.<optionCell_Tapped>d__6>(ref taskAwaiter3, ref this);
								return;
							}
							goto IL_0147;
						}
						break;
					}
					taskAwaiter3.GetResult();
					goto IL_0622;
					IL_0147:
					taskAwaiter3.GetResult();
					IL_014E:
					MQBAdaptationOption mqbadaptationOption = (MQBAdaptationOption)((Element)sender).BindingContext;
					if (codingA5CustomizationPage.coding.RequiresPro && !SharedSettings.Current.AdsProductPurchased)
					{
						taskAwaiter5 = codingA5CustomizationPage.DisplayAlert("Car Scanner Pro", Translate.GetString("FeatureLocked"), "OK", Translate.GetString("btnCancel.Content")).GetAwaiter();
						if (!taskAwaiter5.IsCompleted)
						{
							num2 = 2;
							taskAwaiter2 = taskAwaiter5;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<bool>, CodingA5CustomizationPage.<optionCell_Tapped>d__6>(ref taskAwaiter5, ref this);
							return;
						}
					}
					else if (SharedSettings.Current.CodingsCounter >= 3 && !SharedSettings.Current.AdsProductPurchased)
					{
						taskAwaiter5 = codingA5CustomizationPage.DisplayAlert("Car Scanner Pro", string.Format(Translate.GetString("coding_FreeCodingsFinished"), SharedSettings.Current.CodingsCounter, 3), "OK", Translate.GetString("btnCancel.Content")).GetAwaiter();
						if (!taskAwaiter5.IsCompleted)
						{
							num2 = 4;
							taskAwaiter2 = taskAwaiter5;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<bool>, CodingA5CustomizationPage.<optionCell_Tapped>d__6>(ref taskAwaiter5, ref this);
							return;
						}
						goto IL_0320;
					}
					else
					{
						codingA5CustomizationPage.activityFrame.IsVisible = true;
						codingA5CustomizationPage.lv.IsEnabled = false;
						progress = new Progress<string>(delegate(string s)
						{
							Device.BeginInvokeOnMainThread(new Action(new CodingA5CustomizationPage.<>c__DisplayClass6_0
							{
								<>4__this = codingA5CustomizationPage,
								s = s
							}.<optionCell_Tapped>b__1));
						});
						taskAwaiter6 = codingA5CustomizationPage.coding.Execute(codingA5CustomizationPage.entryPassword.Text, mqbadaptationOption.Value, mqbadaptationOption.Title, progress, null, false).GetAwaiter();
						if (!taskAwaiter6.IsCompleted)
						{
							num2 = 6;
							TaskAwaiter<CodingRequestResult> taskAwaiter7 = taskAwaiter6;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<CodingRequestResult>, CodingA5CustomizationPage.<optionCell_Tapped>d__6>(ref taskAwaiter6, ref this);
							return;
						}
						goto IL_0444;
					}
					IL_01F8:
					if (!taskAwaiter5.GetResult())
					{
						goto IL_026D;
					}
					Page inAppPage = InAppManager.GetInAppPage();
					taskAwaiter3 = codingA5CustomizationPage.Navigation.PushAsync(inAppPage).GetAwaiter();
					if (!taskAwaiter3.IsCompleted)
					{
						num2 = 3;
						TaskAwaiter taskAwaiter4 = taskAwaiter3;
						this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, CodingA5CustomizationPage.<optionCell_Tapped>d__6>(ref taskAwaiter3, ref this);
						return;
					}
					IL_0266:
					taskAwaiter3.GetResult();
					IL_026D:
					goto IL_0622;
					IL_0320:
					if (!taskAwaiter5.GetResult())
					{
						goto IL_0395;
					}
					Page inAppPage2 = InAppManager.GetInAppPage();
					taskAwaiter3 = codingA5CustomizationPage.Navigation.PushAsync(inAppPage2).GetAwaiter();
					if (!taskAwaiter3.IsCompleted)
					{
						num2 = 5;
						TaskAwaiter taskAwaiter4 = taskAwaiter3;
						this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, CodingA5CustomizationPage.<optionCell_Tapped>d__6>(ref taskAwaiter3, ref this);
						return;
					}
					IL_038E:
					taskAwaiter3.GetResult();
					IL_0395:
					goto IL_0622;
					IL_0444:
					CodingRequestResult result = taskAwaiter6.GetResult();
					if (result == CodingRequestResult.Success)
					{
						SharedSettings sharedSettings = SharedSettings.Current;
						int codingsCounter = sharedSettings.CodingsCounter;
						sharedSettings.CodingsCounter = codingsCounter + 1;
						if (!(codingA5CustomizationPage.coding is IServiceProcedure))
						{
							goto IL_0568;
						}
						taskAwaiter3 = codingA5CustomizationPage.DisplayAlert(codingA5CustomizationPage.coding.Name, Translate.GetString("coding_OperationFinished"), "OK").GetAwaiter();
						if (!taskAwaiter3.IsCompleted)
						{
							num2 = 7;
							TaskAwaiter taskAwaiter4 = taskAwaiter3;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, CodingA5CustomizationPage.<optionCell_Tapped>d__6>(ref taskAwaiter3, ref this);
							return;
						}
					}
					else
					{
						taskAwaiter3 = codingA5CustomizationPage.DisplayAlert(Translate.GetString("coding_Error"), MQBAdaptationTemplate.CodingRequestResultToString(result), "OK").GetAwaiter();
						if (!taskAwaiter3.IsCompleted)
						{
							num2 = 8;
							TaskAwaiter taskAwaiter4 = taskAwaiter3;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, CodingA5CustomizationPage.<optionCell_Tapped>d__6>(ref taskAwaiter3, ref this);
							return;
						}
						goto IL_0561;
					}
					IL_04EB:
					taskAwaiter3.GetResult();
					goto IL_0568;
					IL_0561:
					taskAwaiter3.GetResult();
					IL_0568:
					taskAwaiter6 = codingA5CustomizationPage.coding.UpdateCurrentState("", progress).GetAwaiter();
					if (!taskAwaiter6.IsCompleted)
					{
						num2 = 9;
						TaskAwaiter<CodingRequestResult> taskAwaiter7 = taskAwaiter6;
						this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<CodingRequestResult>, CodingA5CustomizationPage.<optionCell_Tapped>d__6>(ref taskAwaiter6, ref this);
						return;
					}
					IL_05D0:
					taskAwaiter6.GetResult();
					codingA5CustomizationPage.activityFrame.IsVisible = false;
					codingA5CustomizationPage.activityFrame.Text = ActivityFrame.PLEASE_WAIT_TEXT;
					codingA5CustomizationPage.lv.IsEnabled = true;
				}
				catch (Exception ex)
				{
					num2 = -2;
					progress = null;
					this.<>t__builder.SetException(ex);
					return;
				}
				IL_0622:
				num2 = -2;
				progress = null;
				this.<>t__builder.SetResult();
			}

			// Token: 0x06005050 RID: 20560 RVA: 0x003DC600 File Offset: 0x003DA800
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x0400301D RID: 12317
			public int <>1__state;

			// Token: 0x0400301E RID: 12318
			public AsyncVoidMethodBuilder <>t__builder;

			// Token: 0x0400301F RID: 12319
			public CodingA5CustomizationPage <>4__this;

			// Token: 0x04003020 RID: 12320
			public object sender;

			// Token: 0x04003021 RID: 12321
			private Progress<string> <progress>5__2;

			// Token: 0x04003022 RID: 12322
			private TaskAwaiter <>u__1;

			// Token: 0x04003023 RID: 12323
			private TaskAwaiter<bool> <>u__2;

			// Token: 0x04003024 RID: 12324
			private TaskAwaiter<CodingRequestResult> <>u__3;
		}

		// Token: 0x02000988 RID: 2440
		[CompilerGenerated]
		private sealed class <InitializeComponent>_anonXamlCDataTemplate_1
		{
			// Token: 0x06005051 RID: 20561 RVA: 0x003DC610 File Offset: 0x003DA810
			public <InitializeComponent>_anonXamlCDataTemplate_1()
			{
			}

			// Token: 0x06005052 RID: 20562 RVA: 0x003DC624 File Offset: 0x003DA824
			internal object LoadDataTemplate()
			{
				BindingExtension bindingExtension;
				VisualDiagnostics.RegisterSourceInfo(bindingExtension = new BindingExtension(), new Uri("Coding\\PagesV2\\CodingA5CustomizationPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 166, 33);
				DynamicResourceExtension dynamicResourceExtension;
				VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension = new DynamicResourceExtension(), new Uri("Coding\\PagesV2\\CodingA5CustomizationPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 168, 33);
				ButtonCell buttonCell;
				VisualDiagnostics.RegisterSourceInfo(buttonCell = new ButtonCell(), new Uri("Coding\\PagesV2\\CodingA5CustomizationPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 165, 30);
				NameScope nameScope = new NameScope();
				NameScope.SetNameScope(buttonCell, nameScope);
				bindingExtension.Path = "Title";
				bindingExtension.TypedBinding = new TypedBinding<MQBAdaptationOption, string>(delegate(MQBAdaptationOption A_0)
				{
					if (A_0 != null)
					{
						return new ValueTuple<string, bool>(A_0.Title, true);
					}
					return default(ValueTuple<string, bool>);
				}, delegate(MQBAdaptationOption A_0, string A_1)
				{
					if (A_0 != null)
					{
						A_0.Title = A_1;
						return;
					}
				}, new Tuple<Func<MQBAdaptationOption, object>, string>[]
				{
					new Tuple<Func<MQBAdaptationOption, object>, string>((MQBAdaptationOption A_0) => A_0, "Title")
				});
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
				xmlNamespaceResolver.Add("mqb", "clr-namespace:CarScannerXamarinForms.Coding.DB.MQB");
				xmlNamespaceResolver.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
				xmlNamespaceResolver.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
				xmlNamespaceResolver.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
				xmlNamespaceResolver.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
				xamlServiceProvider.Add(typeFromHandle2, new XamlTypeResolver(xmlNamespaceResolver, typeof(CodingA5CustomizationPage.<InitializeComponent>_anonXamlCDataTemplate_1).GetTypeInfo().Assembly));
				xamlServiceProvider.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(168, 33)));
				DynamicResource dynamicResource = markupExtension.ProvideValue(xamlServiceProvider);
				buttonCell.SetDynamicResource(CellBase.TitleColorProperty, dynamicResource.Key);
				return buttonCell;
			}

			// Token: 0x06005053 RID: 20563 RVA: 0x003DC8C8 File Offset: 0x003DAAC8
			[CompilerGenerated]
			private static ValueTuple<string, bool> <LoadDataTemplate>typedBindingsM__81(MQBAdaptationOption A_0)
			{
				if (A_0 != null)
				{
					return new ValueTuple<string, bool>(A_0.Title, true);
				}
				return default(ValueTuple<string, bool>);
			}

			// Token: 0x06005054 RID: 20564 RVA: 0x003DC8F8 File Offset: 0x003DAAF8
			[CompilerGenerated]
			private static void <LoadDataTemplate>typedBindingsM__82(MQBAdaptationOption A_0, string A_1)
			{
				if (A_0 != null)
				{
					A_0.Title = A_1;
					return;
				}
			}

			// Token: 0x06005055 RID: 20565 RVA: 0x003DC914 File Offset: 0x003DAB14
			[CompilerGenerated]
			private static object <LoadDataTemplate>typedBindingsM__83(MQBAdaptationOption A_0)
			{
				return A_0;
			}

			// Token: 0x04003025 RID: 12325
			internal object[] parentValues;

			// Token: 0x04003026 RID: 12326
			internal CodingA5CustomizationPage root;
		}
	}
}
