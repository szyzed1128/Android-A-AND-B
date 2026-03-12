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
using CarScannerXamarinForms.UserControls;
using Xamarin.Forms;
using Xamarin.Forms.Internals;
using Xamarin.Forms.PlatformConfiguration.iOSSpecific;
using Xamarin.Forms.Xaml;
using Xamarin.Forms.Xaml.Diagnostics;
using Xamarin.Forms.Xaml.Internals;

namespace CarScannerXamarinForms.Coding.PagesV2
{
	// Token: 0x02000997 RID: 2455
	[XamlCompilation(2)]
	[XamlFilePath("Coding\\PagesV2\\CodingWithInputDetailsPageV2.xaml")]
	public class CodingWithInputDetailsPageV2 : ContentPage
	{
		// Token: 0x06005080 RID: 20608 RVA: 0x003E03A0 File Offset: 0x003DE5A0
		public CodingWithInputDetailsPageV2(ICodingContainer coding)
		{
			this.InitializeComponent();
			this.coding = coding;
			base.BindingContext = this.coding;
			base.Appearing += this.CodingDetailsPage_Appearing;
		}

		// Token: 0x06005081 RID: 20609 RVA: 0x003E03DC File Offset: 0x003DE5DC
		private async void CodingDetailsPage_Appearing(object sender, EventArgs e)
		{
			if (this.first_appearing)
			{
				this.first_appearing = false;
				this.activityFrame.IsVisible = true;
				this.entryValue.IsEnabled = false;
				await this.coding.UpdateCurrentState("", null);
				this.activityFrame.IsVisible = false;
				this.entryValue.IsEnabled = true;
			}
		}

		// Token: 0x06005082 RID: 20610 RVA: 0x003E0414 File Offset: 0x003DE614
		public async Task UpdateState()
		{
			this.activityFrame.IsVisible = true;
			this.entryValue.IsEnabled = false;
			await this.coding.UpdateCurrentState("", null);
			this.activityFrame.IsVisible = false;
			this.entryValue.IsEnabled = true;
		}

		// Token: 0x06005083 RID: 20611 RVA: 0x003E0458 File Offset: 0x003DE658
		private async void BtnUpdateState_Clicked(object sender, EventArgs e)
		{
			this.UpdateState();
		}

		// Token: 0x06005084 RID: 20612 RVA: 0x003E0490 File Offset: 0x003DE690
		private async void btnSaveState_Clicked(object sender, EventArgs e)
		{
			double num;
			if (double.TryParse(this.entryValue.Text, out num) || this.coding is MQBMode22ParametrizeSilentDump)
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

		// Token: 0x06005085 RID: 20613 RVA: 0x003E04C8 File Offset: 0x003DE6C8
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private void InitializeComponent()
		{
			if (ResourceLoader.IsEnabled && ResourceLoader.CanProvideContentFor(new ResourceLoader.ResourceLoadingQuery
			{
				AssemblyName = typeof(CodingWithInputDetailsPageV2).GetTypeInfo().Assembly.GetName(),
				ResourcePath = "Coding/PagesV2/CodingWithInputDetailsPageV2.xaml",
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
			VisualDiagnostics.RegisterSourceInfo(translate = new Translate(), new Uri("Coding\\PagesV2\\CodingWithInputDetailsPageV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 13, 5);
			DynamicResourceExtension dynamicResourceExtension;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension = new DynamicResourceExtension(), new Uri("Coding\\PagesV2\\CodingWithInputDetailsPageV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 15, 5);
			EmptyStringToFalseConverter emptyStringToFalseConverter;
			VisualDiagnostics.RegisterSourceInfo(emptyStringToFalseConverter = new EmptyStringToFalseConverter(), new Uri("Coding\\PagesV2\\CodingWithInputDetailsPageV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 18, 14);
			EmptyStringToTrueConverter emptyStringToTrueConverter;
			VisualDiagnostics.RegisterSourceInfo(emptyStringToTrueConverter = new EmptyStringToTrueConverter(), new Uri("Coding\\PagesV2\\CodingWithInputDetailsPageV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 19, 14);
			VagCodingPlatformToTrueConverter vagCodingPlatformToTrueConverter;
			VisualDiagnostics.RegisterSourceInfo(vagCodingPlatformToTrueConverter = new VagCodingPlatformToTrueConverter(), new Uri("Coding\\PagesV2\\CodingWithInputDetailsPageV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 20, 14);
			ResourceDictionary resourceDictionary;
			VisualDiagnostics.RegisterSourceInfo(resourceDictionary = new ResourceDictionary(), new Uri("Coding\\PagesV2\\CodingWithInputDetailsPageV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 17, 10);
			DynamicResourceExtension dynamicResourceExtension2;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension2 = new DynamicResourceExtension(), new Uri("Coding\\PagesV2\\CodingWithInputDetailsPageV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 34, 33);
			BindingExtension bindingExtension;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension = new BindingExtension(), new Uri("Coding\\PagesV2\\CodingWithInputDetailsPageV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 35, 33);
			Label label;
			VisualDiagnostics.RegisterSourceInfo(label = new Label(), new Uri("Coding\\PagesV2\\CodingWithInputDetailsPageV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 32, 30);
			StaticResourceExtension staticResourceExtension;
			VisualDiagnostics.RegisterSourceInfo(staticResourceExtension = new StaticResourceExtension(), new Uri("Coding\\PagesV2\\CodingWithInputDetailsPageV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 36, 36);
			BindingExtension bindingExtension2;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension2 = new BindingExtension(), new Uri("Coding\\PagesV2\\CodingWithInputDetailsPageV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 36, 36);
			BindingExtension bindingExtension3;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension3 = new BindingExtension(), new Uri("Coding\\PagesV2\\CodingWithInputDetailsPageV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 36, 126);
			Label label2;
			VisualDiagnostics.RegisterSourceInfo(label2 = new Label(), new Uri("Coding\\PagesV2\\CodingWithInputDetailsPageV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 36, 30);
			StaticResourceExtension staticResourceExtension2;
			VisualDiagnostics.RegisterSourceInfo(staticResourceExtension2 = new StaticResourceExtension(), new Uri("Coding\\PagesV2\\CodingWithInputDetailsPageV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 37, 36);
			BindingExtension bindingExtension4;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension4 = new BindingExtension(), new Uri("Coding\\PagesV2\\CodingWithInputDetailsPageV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 37, 36);
			BindingExtension bindingExtension5;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension5 = new BindingExtension(), new Uri("Coding\\PagesV2\\CodingWithInputDetailsPageV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 37, 131);
			Label label3;
			VisualDiagnostics.RegisterSourceInfo(label3 = new Label(), new Uri("Coding\\PagesV2\\CodingWithInputDetailsPageV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 37, 30);
			StackLayout stackLayout;
			VisualDiagnostics.RegisterSourceInfo(stackLayout = new StackLayout(), new Uri("Coding\\PagesV2\\CodingWithInputDetailsPageV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 31, 26);
			CustomCell customCell;
			VisualDiagnostics.RegisterSourceInfo(customCell = new CustomCell(), new Uri("Coding\\PagesV2\\CodingWithInputDetailsPageV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 30, 22);
			Section section;
			VisualDiagnostics.RegisterSourceInfo(section = new Section(), new Uri("Coding\\PagesV2\\CodingWithInputDetailsPageV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 29, 18);
			Translate translate2;
			VisualDiagnostics.RegisterSourceInfo(translate2 = new Translate(), new Uri("Coding\\PagesV2\\CodingWithInputDetailsPageV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 41, 29);
			BindingExtension bindingExtension6;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension6 = new BindingExtension(), new Uri("Coding\\PagesV2\\CodingWithInputDetailsPageV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 41, 76);
			BindingExtension bindingExtension7;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension7 = new BindingExtension(), new Uri("Coding\\PagesV2\\CodingWithInputDetailsPageV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 43, 55);
			Entry entry;
			VisualDiagnostics.RegisterSourceInfo(entry = new Entry(), new Uri("Coding\\PagesV2\\CodingWithInputDetailsPageV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 43, 26);
			CustomCell customCell2;
			VisualDiagnostics.RegisterSourceInfo(customCell2 = new CustomCell(), new Uri("Coding\\PagesV2\\CodingWithInputDetailsPageV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 42, 22);
			StaticResourceExtension staticResourceExtension3;
			VisualDiagnostics.RegisterSourceInfo(staticResourceExtension3 = new StaticResourceExtension(), new Uri("Coding\\PagesV2\\CodingWithInputDetailsPageV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 46, 57);
			BindingExtension bindingExtension8;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension8 = new BindingExtension(), new Uri("Coding\\PagesV2\\CodingWithInputDetailsPageV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 46, 57);
			Translate translate3;
			VisualDiagnostics.RegisterSourceInfo(translate3 = new Translate(), new Uri("Coding\\PagesV2\\CodingWithInputDetailsPageV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 50, 43);
			Span span;
			VisualDiagnostics.RegisterSourceInfo(span = new Span(), new Uri("Coding\\PagesV2\\CodingWithInputDetailsPageV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 50, 38);
			BindingExtension bindingExtension9;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension9 = new BindingExtension(), new Uri("Coding\\PagesV2\\CodingWithInputDetailsPageV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 51, 43);
			Span span2;
			VisualDiagnostics.RegisterSourceInfo(span2 = new Span(), new Uri("Coding\\PagesV2\\CodingWithInputDetailsPageV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 51, 38);
			FormattedString formattedString;
			VisualDiagnostics.RegisterSourceInfo(formattedString = new FormattedString(), new Uri("Coding\\PagesV2\\CodingWithInputDetailsPageV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 49, 34);
			Label label4;
			VisualDiagnostics.RegisterSourceInfo(label4 = new Label(), new Uri("Coding\\PagesV2\\CodingWithInputDetailsPageV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 47, 26);
			CustomCell customCell3;
			VisualDiagnostics.RegisterSourceInfo(customCell3 = new CustomCell(), new Uri("Coding\\PagesV2\\CodingWithInputDetailsPageV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 46, 22);
			StaticResourceExtension staticResourceExtension4;
			VisualDiagnostics.RegisterSourceInfo(staticResourceExtension4 = new StaticResourceExtension(), new Uri("Coding\\PagesV2\\CodingWithInputDetailsPageV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 56, 57);
			BindingExtension bindingExtension10;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension10 = new BindingExtension(), new Uri("Coding\\PagesV2\\CodingWithInputDetailsPageV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 56, 57);
			Translate translate4;
			VisualDiagnostics.RegisterSourceInfo(translate4 = new Translate(), new Uri("Coding\\PagesV2\\CodingWithInputDetailsPageV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 57, 32);
			Label label5;
			VisualDiagnostics.RegisterSourceInfo(label5 = new Label(), new Uri("Coding\\PagesV2\\CodingWithInputDetailsPageV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 57, 26);
			CustomCell customCell4;
			VisualDiagnostics.RegisterSourceInfo(customCell4 = new CustomCell(), new Uri("Coding\\PagesV2\\CodingWithInputDetailsPageV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 56, 22);
			Section section2;
			VisualDiagnostics.RegisterSourceInfo(section2 = new Section(), new Uri("Coding\\PagesV2\\CodingWithInputDetailsPageV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 41, 18);
			Translate translate5;
			VisualDiagnostics.RegisterSourceInfo(translate5 = new Translate(), new Uri("Coding\\PagesV2\\CodingWithInputDetailsPageV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 61, 29);
			BindingExtension bindingExtension11;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension11 = new BindingExtension(), new Uri("Coding\\PagesV2\\CodingWithInputDetailsPageV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 61, 80);
			BindingExtension bindingExtension12;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension12 = new BindingExtension(), new Uri("Coding\\PagesV2\\CodingWithInputDetailsPageV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 63, 35);
			LabelCell labelCell;
			VisualDiagnostics.RegisterSourceInfo(labelCell = new LabelCell(), new Uri("Coding\\PagesV2\\CodingWithInputDetailsPageV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 63, 22);
			Translate translate6;
			VisualDiagnostics.RegisterSourceInfo(translate6 = new Translate(), new Uri("Coding\\PagesV2\\CodingWithInputDetailsPageV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 66, 25);
			DynamicResourceExtension dynamicResourceExtension3;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension3 = new DynamicResourceExtension(), new Uri("Coding\\PagesV2\\CodingWithInputDetailsPageV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 68, 25);
			ButtonCell buttonCell;
			VisualDiagnostics.RegisterSourceInfo(buttonCell = new ButtonCell(), new Uri("Coding\\PagesV2\\CodingWithInputDetailsPageV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 64, 22);
			Section section3;
			VisualDiagnostics.RegisterSourceInfo(section3 = new Section(), new Uri("Coding\\PagesV2\\CodingWithInputDetailsPageV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 61, 18);
			Translate translate7;
			VisualDiagnostics.RegisterSourceInfo(translate7 = new Translate(), new Uri("Coding\\PagesV2\\CodingWithInputDetailsPageV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 71, 29);
			BindingExtension bindingExtension13;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension13 = new BindingExtension(), new Uri("Coding\\PagesV2\\CodingWithInputDetailsPageV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 73, 52);
			Entry entry2;
			VisualDiagnostics.RegisterSourceInfo(entry2 = new Entry(), new Uri("Coding\\PagesV2\\CodingWithInputDetailsPageV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 73, 26);
			CustomCell customCell5;
			VisualDiagnostics.RegisterSourceInfo(customCell5 = new CustomCell(), new Uri("Coding\\PagesV2\\CodingWithInputDetailsPageV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 72, 22);
			Translate translate8;
			VisualDiagnostics.RegisterSourceInfo(translate8 = new Translate(), new Uri("Coding\\PagesV2\\CodingWithInputDetailsPageV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 77, 25);
			DynamicResourceExtension dynamicResourceExtension4;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension4 = new DynamicResourceExtension(), new Uri("Coding\\PagesV2\\CodingWithInputDetailsPageV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 79, 25);
			ButtonCell buttonCell2;
			VisualDiagnostics.RegisterSourceInfo(buttonCell2 = new ButtonCell(), new Uri("Coding\\PagesV2\\CodingWithInputDetailsPageV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 75, 22);
			Section section4;
			VisualDiagnostics.RegisterSourceInfo(section4 = new Section(), new Uri("Coding\\PagesV2\\CodingWithInputDetailsPageV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 71, 18);
			Translate translate9;
			VisualDiagnostics.RegisterSourceInfo(translate9 = new Translate(), new Uri("Coding\\PagesV2\\CodingWithInputDetailsPageV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 84, 25);
			SharedSettings sharedSettings;
			VisualDiagnostics.RegisterSourceInfo(sharedSettings = SharedSettings.Current, new Uri("Coding\\PagesV2\\CodingWithInputDetailsPageV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 85, 25);
			StaticResourceExtension staticResourceExtension5;
			VisualDiagnostics.RegisterSourceInfo(staticResourceExtension5 = new StaticResourceExtension(), new Uri("Coding\\PagesV2\\CodingWithInputDetailsPageV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 86, 25);
			BindingExtension bindingExtension14;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension14 = new BindingExtension(), new Uri("Coding\\PagesV2\\CodingWithInputDetailsPageV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 86, 25);
			LabelCell labelCell2;
			VisualDiagnostics.RegisterSourceInfo(labelCell2 = new LabelCell(), new Uri("Coding\\PagesV2\\CodingWithInputDetailsPageV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 83, 22);
			Translate translate10;
			VisualDiagnostics.RegisterSourceInfo(translate10 = new Translate(), new Uri("Coding\\PagesV2\\CodingWithInputDetailsPageV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 88, 35);
			LabelCell labelCell3;
			VisualDiagnostics.RegisterSourceInfo(labelCell3 = new LabelCell(), new Uri("Coding\\PagesV2\\CodingWithInputDetailsPageV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 88, 22);
			Translate translate11;
			VisualDiagnostics.RegisterSourceInfo(translate11 = new Translate(), new Uri("Coding\\PagesV2\\CodingWithInputDetailsPageV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 90, 25);
			SharedSettings sharedSettings2;
			VisualDiagnostics.RegisterSourceInfo(sharedSettings2 = SharedSettings.Current, new Uri("Coding\\PagesV2\\CodingWithInputDetailsPageV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 91, 25);
			BindingExtension bindingExtension15;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension15 = new BindingExtension(), new Uri("Coding\\PagesV2\\CodingWithInputDetailsPageV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 92, 25);
			BindingExtension bindingExtension16;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension16 = new BindingExtension(), new Uri("Coding\\PagesV2\\CodingWithInputDetailsPageV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 93, 25);
			SettingsCheckBoxCellPatched settingsCheckBoxCellPatched;
			VisualDiagnostics.RegisterSourceInfo(settingsCheckBoxCellPatched = new SettingsCheckBoxCellPatched(), new Uri("Coding\\PagesV2\\CodingWithInputDetailsPageV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 89, 22);
			Section section5;
			VisualDiagnostics.RegisterSourceInfo(section5 = new Section(), new Uri("Coding\\PagesV2\\CodingWithInputDetailsPageV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 82, 18);
			SettingsView settingsView;
			VisualDiagnostics.RegisterSourceInfo(settingsView = new SettingsView(), new Uri("Coding\\PagesV2\\CodingWithInputDetailsPageV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 25, 14);
			ActivityFrame activityFrame;
			VisualDiagnostics.RegisterSourceInfo(activityFrame = new ActivityFrame(), new Uri("Coding\\PagesV2\\CodingWithInputDetailsPageV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 96, 14);
			Grid grid;
			VisualDiagnostics.RegisterSourceInfo(grid = new Grid(), new Uri("Coding\\PagesV2\\CodingWithInputDetailsPageV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 24, 10);
			VisualDiagnostics.RegisterSourceInfo(this, new Uri("Coding\\PagesV2\\CodingWithInputDetailsPageV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 2, 2);
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
			this.entryValue = entry2;
			this.btnSaveNewValue = buttonCell2;
			this.activityFrame = activityFrame;
			this.Resources = resourceDictionary;
			resourceDictionary.Add("EmptyStringToFalseConverter", emptyStringToFalseConverter);
			resourceDictionary.Add("EmptyStringToTrueConverter", emptyStringToTrueConverter);
			resourceDictionary.Add("VagCodingPlatformToTrueConverter", vagCodingPlatformToTrueConverter);
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
			xamlServiceProvider.Add(typeFromHandle2, new XamlTypeResolver(xmlNamespaceResolver, typeof(CodingWithInputDetailsPageV2).GetTypeInfo().Assembly));
			xamlServiceProvider.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(13, 5)));
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
			xmlNamespaceResolver2.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver2.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver2.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver2.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver2.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xmlNamespaceResolver2.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider2.Add(typeFromHandle4, new XamlTypeResolver(xmlNamespaceResolver2, typeof(CodingWithInputDetailsPageV2).GetTypeInfo().Assembly));
			xamlServiceProvider2.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(15, 5)));
			DynamicResource dynamicResource = markupExtension2.ProvideValue(xamlServiceProvider2);
			this.SetDynamicResource(VisualElement.BackgroundColorProperty, dynamicResource.Key);
			this.Resources = resourceDictionary;
			grid.SetValue(Grid.RowDefinitionsProperty, new RowDefinitionCollectionTypeConverter().ConvertFromInvariantString("*"));
			settingsView.SetValue(Grid.RowProperty, 0);
			settingsView.SetValue(TableView.HasUnevenRowsProperty, true);
			customCell.SetValue(CustomCell.IsSelectableProperty, false);
			stackLayout.SetValue(StackLayout.OrientationProperty, 0);
			label.SetValue(Label.FontAttributesProperty, new FontAttributesConverter().ConvertFromInvariantString("Bold"));
			dynamicResourceExtension2.Key = "BaseFontSize+++";
			IMarkupExtension<DynamicResource> markupExtension3 = dynamicResourceExtension2;
			XamlServiceProvider xamlServiceProvider3 = new XamlServiceProvider();
			Type typeFromHandle5 = typeof(IProvideValueTarget);
			object[] array3 = new object[0 + 7];
			array3[0] = label;
			array3[1] = stackLayout;
			array3[2] = customCell;
			array3[3] = section;
			array3[4] = settingsView;
			array3[5] = grid;
			array3[6] = this;
			object obj4;
			xamlServiceProvider3.Add(typeFromHandle5, obj4 = new SimpleValueTargetProvider(array3, Label.FontSizeProperty, nameScope));
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
			xamlServiceProvider3.Add(typeFromHandle6, new XamlTypeResolver(xmlNamespaceResolver3, typeof(CodingWithInputDetailsPageV2).GetTypeInfo().Assembly));
			xamlServiceProvider3.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(34, 33)));
			DynamicResource dynamicResource2 = markupExtension3.ProvideValue(xamlServiceProvider3);
			label.SetDynamicResource(Label.FontSizeProperty, dynamicResource2.Key);
			bindingExtension.Path = "Name";
			BindingBase bindingBase = bindingExtension.ProvideValue(null);
			label.SetBinding(Label.TextProperty, bindingBase);
			stackLayout.Children.Add(label);
			staticResourceExtension.Key = "EmptyStringToFalseConverter";
			IMarkupExtension markupExtension4 = staticResourceExtension;
			XamlServiceProvider xamlServiceProvider4 = new XamlServiceProvider();
			Type typeFromHandle7 = typeof(IProvideValueTarget);
			object[] array4 = new object[0 + 8];
			array4[0] = bindingExtension2;
			array4[1] = label2;
			array4[2] = stackLayout;
			array4[3] = customCell;
			array4[4] = section;
			array4[5] = settingsView;
			array4[6] = grid;
			array4[7] = this;
			object obj5;
			xamlServiceProvider4.Add(typeFromHandle7, obj5 = new SimpleValueTargetProvider(array4, typeof(BindingExtension).GetRuntimeProperty("Converter"), nameScope));
			xamlServiceProvider4.Add(typeof(IReferenceProvider), obj5);
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
			xamlServiceProvider4.Add(typeFromHandle8, new XamlTypeResolver(xmlNamespaceResolver4, typeof(CodingWithInputDetailsPageV2).GetTypeInfo().Assembly));
			xamlServiceProvider4.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(36, 36)));
			object obj6 = markupExtension4.ProvideValue(xamlServiceProvider4);
			bindingExtension2.Converter = obj6;
			bindingExtension2.Path = "Description";
			BindingBase bindingBase2 = bindingExtension2.ProvideValue(null);
			label2.SetBinding(VisualElement.IsVisibleProperty, bindingBase2);
			bindingExtension3.Path = "Description";
			BindingBase bindingBase3 = bindingExtension3.ProvideValue(null);
			label2.SetBinding(Label.TextProperty, bindingBase3);
			stackLayout.Children.Add(label2);
			staticResourceExtension2.Key = "EmptyStringToFalseConverter";
			IMarkupExtension markupExtension5 = staticResourceExtension2;
			XamlServiceProvider xamlServiceProvider5 = new XamlServiceProvider();
			Type typeFromHandle9 = typeof(IProvideValueTarget);
			object[] array5 = new object[0 + 8];
			array5[0] = bindingExtension4;
			array5[1] = label3;
			array5[2] = stackLayout;
			array5[3] = customCell;
			array5[4] = section;
			array5[5] = settingsView;
			array5[6] = grid;
			array5[7] = this;
			object obj7;
			xamlServiceProvider5.Add(typeFromHandle9, obj7 = new SimpleValueTargetProvider(array5, typeof(BindingExtension).GetRuntimeProperty("Converter"), nameScope));
			xamlServiceProvider5.Add(typeof(IReferenceProvider), obj7);
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
			xamlServiceProvider5.Add(typeFromHandle10, new XamlTypeResolver(xmlNamespaceResolver5, typeof(CodingWithInputDetailsPageV2).GetTypeInfo().Assembly));
			xamlServiceProvider5.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(37, 36)));
			object obj8 = markupExtension5.ProvideValue(xamlServiceProvider5);
			bindingExtension4.Converter = obj8;
			bindingExtension4.Path = "InnerDescription";
			BindingBase bindingBase4 = bindingExtension4.ProvideValue(null);
			label3.SetBinding(VisualElement.IsVisibleProperty, bindingBase4);
			bindingExtension5.Path = "InnerDescription";
			BindingBase bindingBase5 = bindingExtension5.ProvideValue(null);
			label3.SetBinding(Label.TextProperty, bindingBase5);
			stackLayout.Children.Add(label3);
			customCell.SetValue(CustomCell.ContentProperty, stackLayout);
			section.Add(customCell);
			settingsView.Root.Add(section);
			translate2.Text = "coding_Password";
			IMarkupExtension markupExtension6 = translate2;
			XamlServiceProvider xamlServiceProvider6 = new XamlServiceProvider();
			Type typeFromHandle11 = typeof(IProvideValueTarget);
			object[] array6 = new object[0 + 4];
			array6[0] = section2;
			array6[1] = settingsView;
			array6[2] = grid;
			array6[3] = this;
			object obj9;
			xamlServiceProvider6.Add(typeFromHandle11, obj9 = new SimpleValueTargetProvider(array6, SectionBase.TitleProperty, nameScope));
			xamlServiceProvider6.Add(typeof(IReferenceProvider), obj9);
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
			xamlServiceProvider6.Add(typeFromHandle12, new XamlTypeResolver(xmlNamespaceResolver6, typeof(CodingWithInputDetailsPageV2).GetTypeInfo().Assembly));
			xamlServiceProvider6.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(41, 29)));
			object obj10 = markupExtension6.ProvideValue(xamlServiceProvider6);
			section2.Title = obj10;
			bindingExtension6.Path = "PasswordVisible";
			BindingBase bindingBase6 = bindingExtension6.ProvideValue(null);
			section2.SetBinding(Section.IsVisibleProperty, bindingBase6);
			customCell2.SetValue(CustomCell.IsSelectableProperty, false);
			bindingExtension7.Mode = 1;
			bindingExtension7.Path = "Password";
			BindingBase bindingBase7 = bindingExtension7.ProvideValue(null);
			entry.SetBinding(Entry.TextProperty, bindingBase7);
			customCell2.SetValue(CustomCell.ContentProperty, entry);
			section2.Add(customCell2);
			customCell3.SetValue(CustomCell.IsSelectableProperty, false);
			staticResourceExtension3.Key = "EmptyStringToFalseConverter";
			IMarkupExtension markupExtension7 = staticResourceExtension3;
			XamlServiceProvider xamlServiceProvider7 = new XamlServiceProvider();
			Type typeFromHandle13 = typeof(IProvideValueTarget);
			object[] array7 = new object[0 + 6];
			array7[0] = bindingExtension8;
			array7[1] = customCell3;
			array7[2] = section2;
			array7[3] = settingsView;
			array7[4] = grid;
			array7[5] = this;
			object obj11;
			xamlServiceProvider7.Add(typeFromHandle13, obj11 = new SimpleValueTargetProvider(array7, typeof(BindingExtension).GetRuntimeProperty("Converter"), nameScope));
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
			xamlServiceProvider7.Add(typeFromHandle14, new XamlTypeResolver(xmlNamespaceResolver7, typeof(CodingWithInputDetailsPageV2).GetTypeInfo().Assembly));
			xamlServiceProvider7.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(46, 57)));
			object obj12 = markupExtension7.ProvideValue(xamlServiceProvider7);
			bindingExtension8.Converter = obj12;
			bindingExtension8.Path = "PasswordHint";
			BindingBase bindingBase8 = bindingExtension8.ProvideValue(null);
			customCell3.SetBinding(CellBase.IsVisibleProperty, bindingBase8);
			label4.SetValue(Label.FontAttributesProperty, new FontAttributesConverter().ConvertFromInvariantString("Italic"));
			translate3.Text = "coding_PasswordHint";
			IMarkupExtension markupExtension8 = translate3;
			XamlServiceProvider xamlServiceProvider8 = new XamlServiceProvider();
			Type typeFromHandle15 = typeof(IProvideValueTarget);
			object[] array8 = new object[0 + 8];
			array8[0] = span;
			array8[1] = formattedString;
			array8[2] = label4;
			array8[3] = customCell3;
			array8[4] = section2;
			array8[5] = settingsView;
			array8[6] = grid;
			array8[7] = this;
			object obj13;
			xamlServiceProvider8.Add(typeFromHandle15, obj13 = new SimpleValueTargetProvider(array8, Span.TextProperty, nameScope));
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
			xamlServiceProvider8.Add(typeFromHandle16, new XamlTypeResolver(xmlNamespaceResolver8, typeof(CodingWithInputDetailsPageV2).GetTypeInfo().Assembly));
			xamlServiceProvider8.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(50, 43)));
			object obj14 = markupExtension8.ProvideValue(xamlServiceProvider8);
			span.Text = obj14;
			formattedString.Spans.Add(span);
			bindingExtension9.Path = "PasswordHint";
			BindingBase bindingBase9 = bindingExtension9.ProvideValue(null);
			span2.SetBinding(Span.TextProperty, bindingBase9);
			formattedString.Spans.Add(span2);
			label4.SetValue(Label.FormattedTextProperty, formattedString);
			customCell3.SetValue(CustomCell.ContentProperty, label4);
			section2.Add(customCell3);
			customCell4.SetValue(CustomCell.IsSelectableProperty, false);
			bindingExtension10.Mode = 2;
			staticResourceExtension4.Key = "EmptyStringToTrueConverter";
			IMarkupExtension markupExtension9 = staticResourceExtension4;
			XamlServiceProvider xamlServiceProvider9 = new XamlServiceProvider();
			Type typeFromHandle17 = typeof(IProvideValueTarget);
			object[] array9 = new object[0 + 6];
			array9[0] = bindingExtension10;
			array9[1] = customCell4;
			array9[2] = section2;
			array9[3] = settingsView;
			array9[4] = grid;
			array9[5] = this;
			object obj15;
			xamlServiceProvider9.Add(typeFromHandle17, obj15 = new SimpleValueTargetProvider(array9, typeof(BindingExtension).GetRuntimeProperty("Converter"), nameScope));
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
			xamlServiceProvider9.Add(typeFromHandle18, new XamlTypeResolver(xmlNamespaceResolver9, typeof(CodingWithInputDetailsPageV2).GetTypeInfo().Assembly));
			xamlServiceProvider9.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(56, 57)));
			object obj16 = markupExtension9.ProvideValue(xamlServiceProvider9);
			bindingExtension10.Converter = obj16;
			bindingExtension10.Path = "Password";
			BindingBase bindingBase10 = bindingExtension10.ProvideValue(null);
			customCell4.SetBinding(CellBase.IsVisibleProperty, bindingBase10);
			translate4.Text = "coding_PasswordNotRequired";
			IMarkupExtension markupExtension10 = translate4;
			XamlServiceProvider xamlServiceProvider10 = new XamlServiceProvider();
			Type typeFromHandle19 = typeof(IProvideValueTarget);
			object[] array10 = new object[0 + 6];
			array10[0] = label5;
			array10[1] = customCell4;
			array10[2] = section2;
			array10[3] = settingsView;
			array10[4] = grid;
			array10[5] = this;
			object obj17;
			xamlServiceProvider10.Add(typeFromHandle19, obj17 = new SimpleValueTargetProvider(array10, Label.TextProperty, nameScope));
			xamlServiceProvider10.Add(typeof(IReferenceProvider), obj17);
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
			xamlServiceProvider10.Add(typeFromHandle20, new XamlTypeResolver(xmlNamespaceResolver10, typeof(CodingWithInputDetailsPageV2).GetTypeInfo().Assembly));
			xamlServiceProvider10.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(57, 32)));
			object obj18 = markupExtension10.ProvideValue(xamlServiceProvider10);
			label5.Text = obj18;
			customCell4.SetValue(CustomCell.ContentProperty, label5);
			section2.Add(customCell4);
			settingsView.Root.Add(section2);
			translate5.Text = "coding_CurrentState";
			IMarkupExtension markupExtension11 = translate5;
			XamlServiceProvider xamlServiceProvider11 = new XamlServiceProvider();
			Type typeFromHandle21 = typeof(IProvideValueTarget);
			object[] array11 = new object[0 + 4];
			array11[0] = section3;
			array11[1] = settingsView;
			array11[2] = grid;
			array11[3] = this;
			object obj19;
			xamlServiceProvider11.Add(typeFromHandle21, obj19 = new SimpleValueTargetProvider(array11, SectionBase.TitleProperty, nameScope));
			xamlServiceProvider11.Add(typeof(IReferenceProvider), obj19);
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
			xamlServiceProvider11.Add(typeFromHandle22, new XamlTypeResolver(xmlNamespaceResolver11, typeof(CodingWithInputDetailsPageV2).GetTypeInfo().Assembly));
			xamlServiceProvider11.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(61, 29)));
			object obj20 = markupExtension11.ProvideValue(xamlServiceProvider11);
			section3.Title = obj20;
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
			translate6.Text = "Mode06Page_btnRefresh.Content";
			IMarkupExtension markupExtension12 = translate6;
			XamlServiceProvider xamlServiceProvider12 = new XamlServiceProvider();
			Type typeFromHandle23 = typeof(IProvideValueTarget);
			object[] array12 = new object[0 + 5];
			array12[0] = buttonCell;
			array12[1] = section3;
			array12[2] = settingsView;
			array12[3] = grid;
			array12[4] = this;
			object obj21;
			xamlServiceProvider12.Add(typeFromHandle23, obj21 = new SimpleValueTargetProvider(array12, CellBase.TitleProperty, nameScope));
			xamlServiceProvider12.Add(typeof(IReferenceProvider), obj21);
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
			xamlServiceProvider12.Add(typeFromHandle24, new XamlTypeResolver(xmlNamespaceResolver12, typeof(CodingWithInputDetailsPageV2).GetTypeInfo().Assembly));
			xamlServiceProvider12.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(66, 25)));
			object obj22 = markupExtension12.ProvideValue(xamlServiceProvider12);
			buttonCell.Title = obj22;
			buttonCell.Tapped += this.BtnUpdateState_Clicked;
			dynamicResourceExtension3.Key = "ButtonAccentColor";
			IMarkupExtension<DynamicResource> markupExtension13 = dynamicResourceExtension3;
			XamlServiceProvider xamlServiceProvider13 = new XamlServiceProvider();
			Type typeFromHandle25 = typeof(IProvideValueTarget);
			object[] array13 = new object[0 + 5];
			array13[0] = buttonCell;
			array13[1] = section3;
			array13[2] = settingsView;
			array13[3] = grid;
			array13[4] = this;
			object obj23;
			xamlServiceProvider13.Add(typeFromHandle25, obj23 = new SimpleValueTargetProvider(array13, CellBase.TitleColorProperty, nameScope));
			xamlServiceProvider13.Add(typeof(IReferenceProvider), obj23);
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
			xamlServiceProvider13.Add(typeFromHandle26, new XamlTypeResolver(xmlNamespaceResolver13, typeof(CodingWithInputDetailsPageV2).GetTypeInfo().Assembly));
			xamlServiceProvider13.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(68, 25)));
			DynamicResource dynamicResource3 = markupExtension13.ProvideValue(xamlServiceProvider13);
			buttonCell.SetDynamicResource(CellBase.TitleColorProperty, dynamicResource3.Key);
			section3.Add(buttonCell);
			settingsView.Root.Add(section3);
			translate7.Text = "coding_TypeNewValue";
			IMarkupExtension markupExtension14 = translate7;
			XamlServiceProvider xamlServiceProvider14 = new XamlServiceProvider();
			Type typeFromHandle27 = typeof(IProvideValueTarget);
			object[] array14 = new object[0 + 4];
			array14[0] = section4;
			array14[1] = settingsView;
			array14[2] = grid;
			array14[3] = this;
			object obj24;
			xamlServiceProvider14.Add(typeFromHandle27, obj24 = new SimpleValueTargetProvider(array14, SectionBase.TitleProperty, nameScope));
			xamlServiceProvider14.Add(typeof(IReferenceProvider), obj24);
			Type typeFromHandle28 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver14 = new XmlNamespaceResolver();
			xmlNamespaceResolver14.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver14.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver14.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver14.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver14.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver14.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver14.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xmlNamespaceResolver14.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider14.Add(typeFromHandle28, new XamlTypeResolver(xmlNamespaceResolver14, typeof(CodingWithInputDetailsPageV2).GetTypeInfo().Assembly));
			xamlServiceProvider14.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(71, 29)));
			object obj25 = markupExtension14.ProvideValue(xamlServiceProvider14);
			section4.Title = obj25;
			customCell5.SetValue(CustomCell.IsSelectableProperty, false);
			bindingExtension13.Mode = 2;
			bindingExtension13.Path = "CurrentState";
			BindingBase bindingBase13 = bindingExtension13.ProvideValue(null);
			entry2.SetBinding(Entry.TextProperty, bindingBase13);
			customCell5.SetValue(CustomCell.ContentProperty, entry2);
			section4.Add(customCell5);
			translate8.Text = "coding_Apply";
			IMarkupExtension markupExtension15 = translate8;
			XamlServiceProvider xamlServiceProvider15 = new XamlServiceProvider();
			Type typeFromHandle29 = typeof(IProvideValueTarget);
			object[] array15 = new object[0 + 5];
			array15[0] = buttonCell2;
			array15[1] = section4;
			array15[2] = settingsView;
			array15[3] = grid;
			array15[4] = this;
			object obj26;
			xamlServiceProvider15.Add(typeFromHandle29, obj26 = new SimpleValueTargetProvider(array15, CellBase.TitleProperty, nameScope));
			xamlServiceProvider15.Add(typeof(IReferenceProvider), obj26);
			Type typeFromHandle30 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver15 = new XmlNamespaceResolver();
			xmlNamespaceResolver15.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver15.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver15.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver15.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver15.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver15.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver15.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xmlNamespaceResolver15.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider15.Add(typeFromHandle30, new XamlTypeResolver(xmlNamespaceResolver15, typeof(CodingWithInputDetailsPageV2).GetTypeInfo().Assembly));
			xamlServiceProvider15.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(77, 25)));
			object obj27 = markupExtension15.ProvideValue(xamlServiceProvider15);
			buttonCell2.Title = obj27;
			buttonCell2.Tapped += this.btnSaveState_Clicked;
			dynamicResourceExtension4.Key = "ButtonAccentColor";
			IMarkupExtension<DynamicResource> markupExtension16 = dynamicResourceExtension4;
			XamlServiceProvider xamlServiceProvider16 = new XamlServiceProvider();
			Type typeFromHandle31 = typeof(IProvideValueTarget);
			object[] array16 = new object[0 + 5];
			array16[0] = buttonCell2;
			array16[1] = section4;
			array16[2] = settingsView;
			array16[3] = grid;
			array16[4] = this;
			object obj28;
			xamlServiceProvider16.Add(typeFromHandle31, obj28 = new SimpleValueTargetProvider(array16, CellBase.TitleColorProperty, nameScope));
			xamlServiceProvider16.Add(typeof(IReferenceProvider), obj28);
			Type typeFromHandle32 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver16 = new XmlNamespaceResolver();
			xmlNamespaceResolver16.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver16.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver16.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver16.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver16.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver16.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver16.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xmlNamespaceResolver16.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider16.Add(typeFromHandle32, new XamlTypeResolver(xmlNamespaceResolver16, typeof(CodingWithInputDetailsPageV2).GetTypeInfo().Assembly));
			xamlServiceProvider16.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(79, 25)));
			DynamicResource dynamicResource4 = markupExtension16.ProvideValue(xamlServiceProvider16);
			buttonCell2.SetDynamicResource(CellBase.TitleColorProperty, dynamicResource4.Key);
			section4.Add(buttonCell2);
			settingsView.Root.Add(section4);
			translate9.Text = "coding_VagOpenHoodWarning";
			IMarkupExtension markupExtension17 = translate9;
			XamlServiceProvider xamlServiceProvider17 = new XamlServiceProvider();
			Type typeFromHandle33 = typeof(IProvideValueTarget);
			object[] array17 = new object[0 + 5];
			array17[0] = labelCell2;
			array17[1] = section5;
			array17[2] = settingsView;
			array17[3] = grid;
			array17[4] = this;
			object obj29;
			xamlServiceProvider17.Add(typeFromHandle33, obj29 = new SimpleValueTargetProvider(array17, CellBase.TitleProperty, nameScope));
			xamlServiceProvider17.Add(typeof(IReferenceProvider), obj29);
			Type typeFromHandle34 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver17 = new XmlNamespaceResolver();
			xmlNamespaceResolver17.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver17.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver17.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver17.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver17.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver17.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver17.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xmlNamespaceResolver17.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider17.Add(typeFromHandle34, new XamlTypeResolver(xmlNamespaceResolver17, typeof(CodingWithInputDetailsPageV2).GetTypeInfo().Assembly));
			xamlServiceProvider17.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(84, 25)));
			object obj30 = markupExtension17.ProvideValue(xamlServiceProvider17);
			labelCell2.Title = obj30;
			labelCell2.SetValue(BindableObject.BindingContextProperty, sharedSettings);
			staticResourceExtension5.Key = "VagCodingPlatformToTrueConverter";
			IMarkupExtension markupExtension18 = staticResourceExtension5;
			XamlServiceProvider xamlServiceProvider18 = new XamlServiceProvider();
			Type typeFromHandle35 = typeof(IProvideValueTarget);
			object[] array18 = new object[0 + 6];
			array18[0] = bindingExtension14;
			array18[1] = labelCell2;
			array18[2] = section5;
			array18[3] = settingsView;
			array18[4] = grid;
			array18[5] = this;
			object obj31;
			xamlServiceProvider18.Add(typeFromHandle35, obj31 = new SimpleValueTargetProvider(array18, typeof(BindingExtension).GetRuntimeProperty("Converter"), nameScope));
			xamlServiceProvider18.Add(typeof(IReferenceProvider), obj31);
			Type typeFromHandle36 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver18 = new XmlNamespaceResolver();
			xmlNamespaceResolver18.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver18.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver18.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver18.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver18.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver18.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver18.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xmlNamespaceResolver18.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider18.Add(typeFromHandle36, new XamlTypeResolver(xmlNamespaceResolver18, typeof(CodingWithInputDetailsPageV2).GetTypeInfo().Assembly));
			xamlServiceProvider18.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(86, 25)));
			object obj32 = markupExtension18.ProvideValue(xamlServiceProvider18);
			bindingExtension14.Converter = obj32;
			bindingExtension14.Path = "CodingLastPlatformSelected";
			BindingBase bindingBase14 = bindingExtension14.ProvideValue(null);
			labelCell2.SetBinding(CellBase.IsVisibleProperty, bindingBase14);
			labelCell2.SetValue(CellBase.TitleFontAttributesProperty, new FontAttributes?(1));
			section5.Add(labelCell2);
			translate10.Text = "coding_UseHistory";
			IMarkupExtension markupExtension19 = translate10;
			XamlServiceProvider xamlServiceProvider19 = new XamlServiceProvider();
			Type typeFromHandle37 = typeof(IProvideValueTarget);
			object[] array19 = new object[0 + 5];
			array19[0] = labelCell3;
			array19[1] = section5;
			array19[2] = settingsView;
			array19[3] = grid;
			array19[4] = this;
			object obj33;
			xamlServiceProvider19.Add(typeFromHandle37, obj33 = new SimpleValueTargetProvider(array19, CellBase.TitleProperty, nameScope));
			xamlServiceProvider19.Add(typeof(IReferenceProvider), obj33);
			Type typeFromHandle38 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver19 = new XmlNamespaceResolver();
			xmlNamespaceResolver19.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver19.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver19.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver19.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver19.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver19.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver19.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xmlNamespaceResolver19.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider19.Add(typeFromHandle38, new XamlTypeResolver(xmlNamespaceResolver19, typeof(CodingWithInputDetailsPageV2).GetTypeInfo().Assembly));
			xamlServiceProvider19.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(88, 35)));
			object obj34 = markupExtension19.ProvideValue(xamlServiceProvider19);
			labelCell3.Title = obj34;
			section5.Add(labelCell3);
			translate11.Text = "coding_ignore_fails";
			IMarkupExtension markupExtension20 = translate11;
			XamlServiceProvider xamlServiceProvider20 = new XamlServiceProvider();
			Type typeFromHandle39 = typeof(IProvideValueTarget);
			object[] array20 = new object[0 + 5];
			array20[0] = settingsCheckBoxCellPatched;
			array20[1] = section5;
			array20[2] = settingsView;
			array20[3] = grid;
			array20[4] = this;
			object obj35;
			xamlServiceProvider20.Add(typeFromHandle39, obj35 = new SimpleValueTargetProvider(array20, CellBase.TitleProperty, nameScope));
			xamlServiceProvider20.Add(typeof(IReferenceProvider), obj35);
			Type typeFromHandle40 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver20 = new XmlNamespaceResolver();
			xmlNamespaceResolver20.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver20.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver20.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver20.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver20.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver20.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver20.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xmlNamespaceResolver20.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider20.Add(typeFromHandle40, new XamlTypeResolver(xmlNamespaceResolver20, typeof(CodingWithInputDetailsPageV2).GetTypeInfo().Assembly));
			xamlServiceProvider20.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(90, 25)));
			object obj36 = markupExtension20.ProvideValue(xamlServiceProvider20);
			settingsCheckBoxCellPatched.Title = obj36;
			settingsCheckBoxCellPatched.SetValue(BindableObject.BindingContextProperty, sharedSettings2);
			bindingExtension15.Mode = 1;
			bindingExtension15.Path = "IgnoreCodingErrors";
			BindingBase bindingBase15 = bindingExtension15.ProvideValue(null);
			settingsCheckBoxCellPatched.SetBinding(CheckboxCell.CheckedProperty, bindingBase15);
			bindingExtension16.Path = "ShowExperimental";
			BindingBase bindingBase16 = bindingExtension16.ProvideValue(null);
			settingsCheckBoxCellPatched.SetBinding(CellBase.IsVisibleProperty, bindingBase16);
			section5.Add(settingsCheckBoxCellPatched);
			settingsView.Root.Add(section5);
			grid.Children.Add(settingsView);
			activityFrame.SetValue(Grid.RowProperty, 0);
			activityFrame.SetValue(VisualElement.IsVisibleProperty, new VisualElement.VisibilityConverter().ConvertFromInvariantString("False"));
			grid.Children.Add(activityFrame);
			this.SetValue(ContentPage.ContentProperty, grid);
		}

		// Token: 0x06005086 RID: 20614 RVA: 0x003E320A File Offset: 0x003E140A
		[CompilerGenerated]
		private void <btnSaveState_Clicked>b__6_0(string s)
		{
			Device.BeginInvokeOnMainThread(delegate
			{
				this.activityFrame.Text = s;
			});
		}

		// Token: 0x06005087 RID: 20615 RVA: 0x003E3230 File Offset: 0x003E1430
		private void __InitComponentRuntime()
		{
			Extensions.LoadFromXaml<CodingWithInputDetailsPageV2>(this, typeof(CodingWithInputDetailsPageV2));
			this.page = NameScopeExtensions.FindByName<ContentPage>(this, "page");
			this.lv = NameScopeExtensions.FindByName<SettingsView>(this, "lv");
			this.entryPassword = NameScopeExtensions.FindByName<Entry>(this, "entryPassword");
			this.btnUpdateState = NameScopeExtensions.FindByName<ButtonCell>(this, "btnUpdateState");
			this.entryValue = NameScopeExtensions.FindByName<Entry>(this, "entryValue");
			this.btnSaveNewValue = NameScopeExtensions.FindByName<ButtonCell>(this, "btnSaveNewValue");
			this.activityFrame = NameScopeExtensions.FindByName<ActivityFrame>(this, "activityFrame");
		}

		// Token: 0x0400305C RID: 12380
		private bool first_appearing = true;

		// Token: 0x0400305D RID: 12381
		private ICodingContainer coding;

		// Token: 0x0400305E RID: 12382
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private ContentPage page;

		// Token: 0x0400305F RID: 12383
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private SettingsView lv;

		// Token: 0x04003060 RID: 12384
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private Entry entryPassword;

		// Token: 0x04003061 RID: 12385
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private ButtonCell btnUpdateState;

		// Token: 0x04003062 RID: 12386
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private Entry entryValue;

		// Token: 0x04003063 RID: 12387
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private ButtonCell btnSaveNewValue;

		// Token: 0x04003064 RID: 12388
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private ActivityFrame activityFrame;

		// Token: 0x02000998 RID: 2456
		[CompilerGenerated]
		private sealed class <>c__DisplayClass6_0
		{
			// Token: 0x06005088 RID: 20616 RVA: 0x00002050 File Offset: 0x00000250
			public <>c__DisplayClass6_0()
			{
			}

			// Token: 0x06005089 RID: 20617 RVA: 0x003E32C5 File Offset: 0x003E14C5
			internal void <btnSaveState_Clicked>b__1()
			{
				this.<>4__this.activityFrame.Text = this.s;
			}

			// Token: 0x04003065 RID: 12389
			public string s;

			// Token: 0x04003066 RID: 12390
			public CodingWithInputDetailsPageV2 <>4__this;
		}

		// Token: 0x02000999 RID: 2457
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <BtnUpdateState_Clicked>d__4 : IAsyncStateMachine
		{
			// Token: 0x0600508A RID: 20618 RVA: 0x003E32E0 File Offset: 0x003E14E0
			void IAsyncStateMachine.MoveNext()
			{
				CodingWithInputDetailsPageV2 codingWithInputDetailsPageV = this;
				try
				{
					codingWithInputDetailsPageV.UpdateState();
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

			// Token: 0x0600508B RID: 20619 RVA: 0x003E3338 File Offset: 0x003E1538
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x04003067 RID: 12391
			public int <>1__state;

			// Token: 0x04003068 RID: 12392
			public AsyncVoidMethodBuilder <>t__builder;

			// Token: 0x04003069 RID: 12393
			public CodingWithInputDetailsPageV2 <>4__this;
		}

		// Token: 0x0200099A RID: 2458
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <CodingDetailsPage_Appearing>d__2 : IAsyncStateMachine
		{
			// Token: 0x0600508C RID: 20620 RVA: 0x003E3348 File Offset: 0x003E1548
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				CodingWithInputDetailsPageV2 codingWithInputDetailsPageV = this;
				try
				{
					TaskAwaiter<CodingRequestResult> taskAwaiter;
					if (num != 0)
					{
						if (!codingWithInputDetailsPageV.first_appearing)
						{
							goto IL_00B7;
						}
						codingWithInputDetailsPageV.first_appearing = false;
						codingWithInputDetailsPageV.activityFrame.IsVisible = true;
						codingWithInputDetailsPageV.entryValue.IsEnabled = false;
						taskAwaiter = codingWithInputDetailsPageV.coding.UpdateCurrentState("", null).GetAwaiter();
						if (!taskAwaiter.IsCompleted)
						{
							num2 = 0;
							TaskAwaiter<CodingRequestResult> taskAwaiter2 = taskAwaiter;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<CodingRequestResult>, CodingWithInputDetailsPageV2.<CodingDetailsPage_Appearing>d__2>(ref taskAwaiter, ref this);
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
					codingWithInputDetailsPageV.activityFrame.IsVisible = false;
					codingWithInputDetailsPageV.entryValue.IsEnabled = true;
					IL_00B7:;
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

			// Token: 0x0600508D RID: 20621 RVA: 0x003E3448 File Offset: 0x003E1648
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x0400306A RID: 12394
			public int <>1__state;

			// Token: 0x0400306B RID: 12395
			public AsyncVoidMethodBuilder <>t__builder;

			// Token: 0x0400306C RID: 12396
			public CodingWithInputDetailsPageV2 <>4__this;

			// Token: 0x0400306D RID: 12397
			private TaskAwaiter<CodingRequestResult> <>u__1;
		}

		// Token: 0x0200099B RID: 2459
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <UpdateState>d__3 : IAsyncStateMachine
		{
			// Token: 0x0600508E RID: 20622 RVA: 0x003E3458 File Offset: 0x003E1658
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				CodingWithInputDetailsPageV2 codingWithInputDetailsPageV = this;
				try
				{
					TaskAwaiter<CodingRequestResult> taskAwaiter;
					if (num != 0)
					{
						codingWithInputDetailsPageV.activityFrame.IsVisible = true;
						codingWithInputDetailsPageV.entryValue.IsEnabled = false;
						taskAwaiter = codingWithInputDetailsPageV.coding.UpdateCurrentState("", null).GetAwaiter();
						if (!taskAwaiter.IsCompleted)
						{
							num2 = 0;
							TaskAwaiter<CodingRequestResult> taskAwaiter2 = taskAwaiter;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<CodingRequestResult>, CodingWithInputDetailsPageV2.<UpdateState>d__3>(ref taskAwaiter, ref this);
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
					codingWithInputDetailsPageV.activityFrame.IsVisible = false;
					codingWithInputDetailsPageV.entryValue.IsEnabled = true;
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

			// Token: 0x0600508F RID: 20623 RVA: 0x003E3548 File Offset: 0x003E1748
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x0400306E RID: 12398
			public int <>1__state;

			// Token: 0x0400306F RID: 12399
			public AsyncTaskMethodBuilder <>t__builder;

			// Token: 0x04003070 RID: 12400
			public CodingWithInputDetailsPageV2 <>4__this;

			// Token: 0x04003071 RID: 12401
			private TaskAwaiter<CodingRequestResult> <>u__1;
		}

		// Token: 0x0200099C RID: 2460
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <btnSaveState_Clicked>d__6 : IAsyncStateMachine
		{
			// Token: 0x06005090 RID: 20624 RVA: 0x003E3558 File Offset: 0x003E1758
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				CodingWithInputDetailsPageV2 codingWithInputDetailsPageV = this;
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
						goto IL_015A;
					}
					case 2:
						taskAwaiter3 = taskAwaiter2;
						taskAwaiter2 = default(TaskAwaiter<bool>);
						num2 = -1;
						goto IL_0214;
					case 3:
					{
						TaskAwaiter taskAwaiter5;
						taskAwaiter4 = taskAwaiter5;
						taskAwaiter5 = default(TaskAwaiter);
						num2 = -1;
						goto IL_0282;
					}
					case 4:
					{
						TaskAwaiter<CodingRequestResult> taskAwaiter7;
						taskAwaiter6 = taskAwaiter7;
						taskAwaiter7 = default(TaskAwaiter<CodingRequestResult>);
						num2 = -1;
						goto IL_0338;
					}
					case 5:
					{
						TaskAwaiter taskAwaiter5;
						taskAwaiter4 = taskAwaiter5;
						taskAwaiter5 = default(TaskAwaiter);
						num2 = -1;
						goto IL_03CA;
					}
					case 6:
					{
						TaskAwaiter<CodingRequestResult> taskAwaiter7;
						taskAwaiter6 = taskAwaiter7;
						taskAwaiter7 = default(TaskAwaiter<CodingRequestResult>);
						num2 = -1;
						goto IL_0433;
					}
					case 7:
					{
						TaskAwaiter taskAwaiter5;
						taskAwaiter4 = taskAwaiter5;
						taskAwaiter5 = default(TaskAwaiter);
						num2 = -1;
						goto IL_04F1;
					}
					default:
					{
						double num3;
						if (double.TryParse(codingWithInputDetailsPageV.entryValue.Text, out num3) || codingWithInputDetailsPageV.coding is MQBMode22ParametrizeSilentDump)
						{
							if (codingWithInputDetailsPageV.coding.RequiresPro && !SharedSettings.Current.AdsProductPurchased)
							{
								taskAwaiter3 = codingWithInputDetailsPageV.DisplayAlert("Car Scanner Pro", Translate.GetString("FeatureLocked"), "OK", Translate.GetString("btnCancel.Content")).GetAwaiter();
								if (!taskAwaiter3.IsCompleted)
								{
									num2 = 0;
									taskAwaiter2 = taskAwaiter3;
									this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<bool>, CodingWithInputDetailsPageV2.<btnSaveState_Clicked>d__6>(ref taskAwaiter3, ref this);
									return;
								}
							}
							else if (SharedSettings.Current.CodingsCounter >= 3 && !SharedSettings.Current.AdsProductPurchased)
							{
								taskAwaiter3 = codingWithInputDetailsPageV.DisplayAlert("Car Scanner Pro", string.Format(Translate.GetString("coding_FreeCodingsFinished"), SharedSettings.Current.CodingsCounter, 3), "OK", Translate.GetString("btnCancel.Content")).GetAwaiter();
								if (!taskAwaiter3.IsCompleted)
								{
									num2 = 2;
									taskAwaiter2 = taskAwaiter3;
									this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<bool>, CodingWithInputDetailsPageV2.<btnSaveState_Clicked>d__6>(ref taskAwaiter3, ref this);
									return;
								}
								goto IL_0214;
							}
							else
							{
								codingWithInputDetailsPageV.activityFrame.IsVisible = true;
								codingWithInputDetailsPageV.entryValue.IsEnabled = false;
								Progress<string> progress = new Progress<string>(delegate(string s)
								{
									Device.BeginInvokeOnMainThread(new Action(new CodingWithInputDetailsPageV2.<>c__DisplayClass6_0
									{
										<>4__this = codingWithInputDetailsPageV,
										s = s
									}.<btnSaveState_Clicked>b__1));
								});
								taskAwaiter6 = codingWithInputDetailsPageV.coding.Execute(codingWithInputDetailsPageV.entryPassword.Text, codingWithInputDetailsPageV.entryValue.Text, codingWithInputDetailsPageV.entryValue.Text, progress, null, false).GetAwaiter();
								if (!taskAwaiter6.IsCompleted)
								{
									num2 = 4;
									TaskAwaiter<CodingRequestResult> taskAwaiter7 = taskAwaiter6;
									this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<CodingRequestResult>, CodingWithInputDetailsPageV2.<btnSaveState_Clicked>d__6>(ref taskAwaiter6, ref this);
									return;
								}
								goto IL_0338;
							}
						}
						else
						{
							taskAwaiter4 = codingWithInputDetailsPageV.DisplayAlert(Translate.GetString("coding_WrongInputFormatTitle"), Translate.GetString("coding_WrongInputFormatText") + "\n" + 123.45.ToString(), "OK").GetAwaiter();
							if (!taskAwaiter4.IsCompleted)
							{
								num2 = 7;
								TaskAwaiter taskAwaiter5 = taskAwaiter4;
								this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, CodingWithInputDetailsPageV2.<btnSaveState_Clicked>d__6>(ref taskAwaiter4, ref this);
								return;
							}
							goto IL_04F1;
						}
						break;
					}
					}
					if (!taskAwaiter3.GetResult())
					{
						goto IL_0161;
					}
					Page inAppPage = InAppManager.GetInAppPage();
					taskAwaiter4 = codingWithInputDetailsPageV.Navigation.PushAsync(inAppPage).GetAwaiter();
					if (!taskAwaiter4.IsCompleted)
					{
						num2 = 1;
						TaskAwaiter taskAwaiter5 = taskAwaiter4;
						this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, CodingWithInputDetailsPageV2.<btnSaveState_Clicked>d__6>(ref taskAwaiter4, ref this);
						return;
					}
					IL_015A:
					taskAwaiter4.GetResult();
					IL_0161:
					goto IL_0513;
					IL_0214:
					if (!taskAwaiter3.GetResult())
					{
						goto IL_0289;
					}
					Page inAppPage2 = InAppManager.GetInAppPage();
					taskAwaiter4 = codingWithInputDetailsPageV.Navigation.PushAsync(inAppPage2).GetAwaiter();
					if (!taskAwaiter4.IsCompleted)
					{
						num2 = 3;
						TaskAwaiter taskAwaiter5 = taskAwaiter4;
						this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, CodingWithInputDetailsPageV2.<btnSaveState_Clicked>d__6>(ref taskAwaiter4, ref this);
						return;
					}
					IL_0282:
					taskAwaiter4.GetResult();
					IL_0289:
					goto IL_0513;
					IL_0338:
					CodingRequestResult result = taskAwaiter6.GetResult();
					if (result == CodingRequestResult.Success)
					{
						SharedSettings sharedSettings = SharedSettings.Current;
						int codingsCounter = sharedSettings.CodingsCounter;
						sharedSettings.CodingsCounter = codingsCounter + 1;
						goto IL_03D1;
					}
					taskAwaiter4 = codingWithInputDetailsPageV.DisplayAlert(Translate.GetString("coding_Error"), MQBAdaptationTemplate.CodingRequestResultToString(result), "OK").GetAwaiter();
					if (!taskAwaiter4.IsCompleted)
					{
						num2 = 5;
						TaskAwaiter taskAwaiter5 = taskAwaiter4;
						this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, CodingWithInputDetailsPageV2.<btnSaveState_Clicked>d__6>(ref taskAwaiter4, ref this);
						return;
					}
					IL_03CA:
					taskAwaiter4.GetResult();
					IL_03D1:
					taskAwaiter6 = codingWithInputDetailsPageV.coding.UpdateCurrentState("", null).GetAwaiter();
					if (!taskAwaiter6.IsCompleted)
					{
						num2 = 6;
						TaskAwaiter<CodingRequestResult> taskAwaiter7 = taskAwaiter6;
						this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<CodingRequestResult>, CodingWithInputDetailsPageV2.<btnSaveState_Clicked>d__6>(ref taskAwaiter6, ref this);
						return;
					}
					IL_0433:
					taskAwaiter6.GetResult();
					codingWithInputDetailsPageV.activityFrame.IsVisible = false;
					codingWithInputDetailsPageV.activityFrame.Text = ActivityFrame.PLEASE_WAIT_TEXT;
					codingWithInputDetailsPageV.entryValue.IsEnabled = true;
					goto IL_04F8;
					IL_04F1:
					taskAwaiter4.GetResult();
					IL_04F8:;
				}
				catch (Exception ex)
				{
					num2 = -2;
					this.<>t__builder.SetException(ex);
					return;
				}
				IL_0513:
				num2 = -2;
				this.<>t__builder.SetResult();
			}

			// Token: 0x06005091 RID: 20625 RVA: 0x003E3AA8 File Offset: 0x003E1CA8
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x04003072 RID: 12402
			public int <>1__state;

			// Token: 0x04003073 RID: 12403
			public AsyncVoidMethodBuilder <>t__builder;

			// Token: 0x04003074 RID: 12404
			public CodingWithInputDetailsPageV2 <>4__this;

			// Token: 0x04003075 RID: 12405
			private TaskAwaiter<bool> <>u__1;

			// Token: 0x04003076 RID: 12406
			private TaskAwaiter <>u__2;

			// Token: 0x04003077 RID: 12407
			private TaskAwaiter<CodingRequestResult> <>u__3;
		}
	}
}
