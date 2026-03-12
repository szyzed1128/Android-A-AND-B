using System;
using System.CodeDom.Compiler;
using System.Diagnostics;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using CarScannerXamarinForms.Common.XAMLConverters;
using CarScannerXamarinForms.InApp;
using CarScannerXamarinForms.Settings;
using Xamarin.Forms;
using Xamarin.Forms.Internals;
using Xamarin.Forms.PlatformConfiguration.iOSSpecific;
using Xamarin.Forms.Xaml;
using Xamarin.Forms.Xaml.Diagnostics;
using Xamarin.Forms.Xaml.Internals;

namespace CarScannerXamarinForms.Coding
{
	// Token: 0x02000904 RID: 2308
	[XamlCompilation(2)]
	[XamlFilePath("Coding\\Pages\\CodingWithInputDetailsPage.xaml")]
	public class CodingWithInputDetailsPage : ContentPage
	{
		// Token: 0x06004D6E RID: 19822 RVA: 0x00399468 File Offset: 0x00397668
		public CodingWithInputDetailsPage(ICodingContainer coding)
		{
			this.InitializeComponent();
			this.coding = coding;
			base.BindingContext = this.coding;
			base.Appearing += this.CodingDetailsPage_Appearing;
		}

		// Token: 0x06004D6F RID: 19823 RVA: 0x003994A4 File Offset: 0x003976A4
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

		// Token: 0x06004D70 RID: 19824 RVA: 0x003994DC File Offset: 0x003976DC
		public async Task UpdateState()
		{
			this.activityFrame.IsVisible = true;
			this.entryValue.IsEnabled = false;
			await this.coding.UpdateCurrentState("", null);
			this.activityFrame.IsVisible = false;
			this.entryValue.IsEnabled = true;
		}

		// Token: 0x06004D71 RID: 19825 RVA: 0x00399520 File Offset: 0x00397720
		private async void BtnUpdateState_Clicked(object sender, EventArgs e)
		{
			this.UpdateState();
		}

		// Token: 0x06004D72 RID: 19826 RVA: 0x00399558 File Offset: 0x00397758
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

		// Token: 0x06004D73 RID: 19827 RVA: 0x00399590 File Offset: 0x00397790
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private void InitializeComponent()
		{
			if (ResourceLoader.IsEnabled && ResourceLoader.CanProvideContentFor(new ResourceLoader.ResourceLoadingQuery
			{
				AssemblyName = typeof(CodingWithInputDetailsPage).GetTypeInfo().Assembly.GetName(),
				ResourcePath = "Coding/Pages/CodingWithInputDetailsPage.xaml",
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
			VisualDiagnostics.RegisterSourceInfo(translate = new Translate(), new Uri("Coding\\Pages\\CodingWithInputDetailsPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 12, 5);
			DynamicResourceExtension dynamicResourceExtension;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension = new DynamicResourceExtension(), new Uri("Coding\\Pages\\CodingWithInputDetailsPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 15, 5);
			EmptyStringToFalseConverter emptyStringToFalseConverter;
			VisualDiagnostics.RegisterSourceInfo(emptyStringToFalseConverter = new EmptyStringToFalseConverter(), new Uri("Coding\\Pages\\CodingWithInputDetailsPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 19, 14);
			EmptyStringToTrueConverter emptyStringToTrueConverter;
			VisualDiagnostics.RegisterSourceInfo(emptyStringToTrueConverter = new EmptyStringToTrueConverter(), new Uri("Coding\\Pages\\CodingWithInputDetailsPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 20, 14);
			VagCodingPlatformToTrueConverter vagCodingPlatformToTrueConverter;
			VisualDiagnostics.RegisterSourceInfo(vagCodingPlatformToTrueConverter = new VagCodingPlatformToTrueConverter(), new Uri("Coding\\Pages\\CodingWithInputDetailsPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 21, 14);
			ResourceDictionary resourceDictionary;
			VisualDiagnostics.RegisterSourceInfo(resourceDictionary = new ResourceDictionary(), new Uri("Coding\\Pages\\CodingWithInputDetailsPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 18, 10);
			RowDefinition rowDefinition;
			VisualDiagnostics.RegisterSourceInfo(rowDefinition = new RowDefinition(), new Uri("Coding\\Pages\\CodingWithInputDetailsPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 28, 22);
			DynamicResourceExtension dynamicResourceExtension2;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension2 = new DynamicResourceExtension(), new Uri("Coding\\Pages\\CodingWithInputDetailsPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 33, 25);
			BindingExtension bindingExtension;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension = new BindingExtension(), new Uri("Coding\\Pages\\CodingWithInputDetailsPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 34, 25);
			Label label;
			VisualDiagnostics.RegisterSourceInfo(label = new Label(), new Uri("Coding\\Pages\\CodingWithInputDetailsPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 31, 22);
			StaticResourceExtension staticResourceExtension;
			VisualDiagnostics.RegisterSourceInfo(staticResourceExtension = new StaticResourceExtension(), new Uri("Coding\\Pages\\CodingWithInputDetailsPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 35, 28);
			BindingExtension bindingExtension2;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension2 = new BindingExtension(), new Uri("Coding\\Pages\\CodingWithInputDetailsPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 35, 28);
			BindingExtension bindingExtension3;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension3 = new BindingExtension(), new Uri("Coding\\Pages\\CodingWithInputDetailsPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 35, 118);
			Label label2;
			VisualDiagnostics.RegisterSourceInfo(label2 = new Label(), new Uri("Coding\\Pages\\CodingWithInputDetailsPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 35, 22);
			StaticResourceExtension staticResourceExtension2;
			VisualDiagnostics.RegisterSourceInfo(staticResourceExtension2 = new StaticResourceExtension(), new Uri("Coding\\Pages\\CodingWithInputDetailsPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 36, 28);
			BindingExtension bindingExtension4;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension4 = new BindingExtension(), new Uri("Coding\\Pages\\CodingWithInputDetailsPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 36, 28);
			BindingExtension bindingExtension5;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension5 = new BindingExtension(), new Uri("Coding\\Pages\\CodingWithInputDetailsPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 36, 136);
			Label label3;
			VisualDiagnostics.RegisterSourceInfo(label3 = new Label(), new Uri("Coding\\Pages\\CodingWithInputDetailsPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 36, 22);
			BindingExtension bindingExtension6;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension6 = new BindingExtension(), new Uri("Coding\\Pages\\CodingWithInputDetailsPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 38, 34);
			Translate translate2;
			VisualDiagnostics.RegisterSourceInfo(translate2 = new Translate(), new Uri("Coding\\Pages\\CodingWithInputDetailsPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 39, 32);
			Label label4;
			VisualDiagnostics.RegisterSourceInfo(label4 = new Label(), new Uri("Coding\\Pages\\CodingWithInputDetailsPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 39, 26);
			Translate translate3;
			VisualDiagnostics.RegisterSourceInfo(translate3 = new Translate(), new Uri("Coding\\Pages\\CodingWithInputDetailsPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 42, 29);
			BindingExtension bindingExtension7;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension7 = new BindingExtension(), new Uri("Coding\\Pages\\CodingWithInputDetailsPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 43, 29);
			Entry entry;
			VisualDiagnostics.RegisterSourceInfo(entry = new Entry(), new Uri("Coding\\Pages\\CodingWithInputDetailsPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 40, 26);
			StaticResourceExtension staticResourceExtension3;
			VisualDiagnostics.RegisterSourceInfo(staticResourceExtension3 = new StaticResourceExtension(), new Uri("Coding\\Pages\\CodingWithInputDetailsPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 44, 56);
			BindingExtension bindingExtension8;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension8 = new BindingExtension(), new Uri("Coding\\Pages\\CodingWithInputDetailsPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 44, 56);
			Translate translate4;
			VisualDiagnostics.RegisterSourceInfo(translate4 = new Translate(), new Uri("Coding\\Pages\\CodingWithInputDetailsPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 47, 43);
			Span span;
			VisualDiagnostics.RegisterSourceInfo(span = new Span(), new Uri("Coding\\Pages\\CodingWithInputDetailsPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 47, 38);
			BindingExtension bindingExtension9;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension9 = new BindingExtension(), new Uri("Coding\\Pages\\CodingWithInputDetailsPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 48, 43);
			Span span2;
			VisualDiagnostics.RegisterSourceInfo(span2 = new Span(), new Uri("Coding\\Pages\\CodingWithInputDetailsPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 48, 38);
			FormattedString formattedString;
			VisualDiagnostics.RegisterSourceInfo(formattedString = new FormattedString(), new Uri("Coding\\Pages\\CodingWithInputDetailsPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 46, 34);
			Label label5;
			VisualDiagnostics.RegisterSourceInfo(label5 = new Label(), new Uri("Coding\\Pages\\CodingWithInputDetailsPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 44, 26);
			StaticResourceExtension staticResourceExtension4;
			VisualDiagnostics.RegisterSourceInfo(staticResourceExtension4 = new StaticResourceExtension(), new Uri("Coding\\Pages\\CodingWithInputDetailsPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 54, 29);
			BindingExtension bindingExtension10;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension10 = new BindingExtension(), new Uri("Coding\\Pages\\CodingWithInputDetailsPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 54, 29);
			Translate translate5;
			VisualDiagnostics.RegisterSourceInfo(translate5 = new Translate(), new Uri("Coding\\Pages\\CodingWithInputDetailsPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 55, 29);
			Label label6;
			VisualDiagnostics.RegisterSourceInfo(label6 = new Label(), new Uri("Coding\\Pages\\CodingWithInputDetailsPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 52, 26);
			StackLayout stackLayout;
			VisualDiagnostics.RegisterSourceInfo(stackLayout = new StackLayout(), new Uri("Coding\\Pages\\CodingWithInputDetailsPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 38, 22);
			BindingExtension bindingExtension11;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension11 = new BindingExtension(), new Uri("Coding\\Pages\\CodingWithInputDetailsPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 57, 48);
			Translate translate6;
			VisualDiagnostics.RegisterSourceInfo(translate6 = new Translate(), new Uri("Coding\\Pages\\CodingWithInputDetailsPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 60, 39);
			Span span3;
			VisualDiagnostics.RegisterSourceInfo(span3 = new Span(), new Uri("Coding\\Pages\\CodingWithInputDetailsPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 60, 34);
			Span span4;
			VisualDiagnostics.RegisterSourceInfo(span4 = new Span(), new Uri("Coding\\Pages\\CodingWithInputDetailsPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 61, 34);
			BindingExtension bindingExtension12;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension12 = new BindingExtension(), new Uri("Coding\\Pages\\CodingWithInputDetailsPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 62, 61);
			Span span5;
			VisualDiagnostics.RegisterSourceInfo(span5 = new Span(), new Uri("Coding\\Pages\\CodingWithInputDetailsPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 62, 34);
			FormattedString formattedString2;
			VisualDiagnostics.RegisterSourceInfo(formattedString2 = new FormattedString(), new Uri("Coding\\Pages\\CodingWithInputDetailsPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 59, 30);
			Label label7;
			VisualDiagnostics.RegisterSourceInfo(label7 = new Label(), new Uri("Coding\\Pages\\CodingWithInputDetailsPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 57, 22);
			BindingExtension bindingExtension13;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension13 = new BindingExtension(), new Uri("Coding\\Pages\\CodingWithInputDetailsPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 69, 25);
			Translate translate7;
			VisualDiagnostics.RegisterSourceInfo(translate7 = new Translate(), new Uri("Coding\\Pages\\CodingWithInputDetailsPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 70, 25);
			Button button;
			VisualDiagnostics.RegisterSourceInfo(button = new Button(), new Uri("Coding\\Pages\\CodingWithInputDetailsPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 66, 22);
			Translate translate8;
			VisualDiagnostics.RegisterSourceInfo(translate8 = new Translate(), new Uri("Coding\\Pages\\CodingWithInputDetailsPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 71, 28);
			Label label8;
			VisualDiagnostics.RegisterSourceInfo(label8 = new Label(), new Uri("Coding\\Pages\\CodingWithInputDetailsPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 71, 22);
			BindingExtension bindingExtension14;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension14 = new BindingExtension(), new Uri("Coding\\Pages\\CodingWithInputDetailsPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 72, 48);
			Entry entry2;
			VisualDiagnostics.RegisterSourceInfo(entry2 = new Entry(), new Uri("Coding\\Pages\\CodingWithInputDetailsPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 72, 22);
			Translate translate9;
			VisualDiagnostics.RegisterSourceInfo(translate9 = new Translate(), new Uri("Coding\\Pages\\CodingWithInputDetailsPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 76, 25);
			Button button2;
			VisualDiagnostics.RegisterSourceInfo(button2 = new Button(), new Uri("Coding\\Pages\\CodingWithInputDetailsPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 73, 22);
			SharedSettings sharedSettings;
			VisualDiagnostics.RegisterSourceInfo(sharedSettings = SharedSettings.Current, new Uri("Coding\\Pages\\CodingWithInputDetailsPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 83, 25);
			DynamicResourceExtension dynamicResourceExtension3;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension3 = new DynamicResourceExtension(), new Uri("Coding\\Pages\\CodingWithInputDetailsPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 85, 25);
			StaticResourceExtension staticResourceExtension5;
			VisualDiagnostics.RegisterSourceInfo(staticResourceExtension5 = new StaticResourceExtension(), new Uri("Coding\\Pages\\CodingWithInputDetailsPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 86, 25);
			BindingExtension bindingExtension15;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension15 = new BindingExtension(), new Uri("Coding\\Pages\\CodingWithInputDetailsPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 86, 25);
			Translate translate10;
			VisualDiagnostics.RegisterSourceInfo(translate10 = new Translate(), new Uri("Coding\\Pages\\CodingWithInputDetailsPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 87, 25);
			Label label9;
			VisualDiagnostics.RegisterSourceInfo(label9 = new Label(), new Uri("Coding\\Pages\\CodingWithInputDetailsPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 82, 22);
			DynamicResourceExtension dynamicResourceExtension4;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension4 = new DynamicResourceExtension(), new Uri("Coding\\Pages\\CodingWithInputDetailsPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 90, 25);
			Translate translate11;
			VisualDiagnostics.RegisterSourceInfo(translate11 = new Translate(), new Uri("Coding\\Pages\\CodingWithInputDetailsPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 91, 25);
			Label label10;
			VisualDiagnostics.RegisterSourceInfo(label10 = new Label(), new Uri("Coding\\Pages\\CodingWithInputDetailsPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 88, 22);
			SharedSettings sharedSettings2;
			VisualDiagnostics.RegisterSourceInfo(sharedSettings2 = SharedSettings.Current, new Uri("Coding\\Pages\\CodingWithInputDetailsPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 95, 25);
			BindingExtension bindingExtension16;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension16 = new BindingExtension(), new Uri("Coding\\Pages\\CodingWithInputDetailsPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 96, 25);
			BindingExtension bindingExtension17;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension17 = new BindingExtension(), new Uri("Coding\\Pages\\CodingWithInputDetailsPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 97, 25);
			Translate translate12;
			VisualDiagnostics.RegisterSourceInfo(translate12 = new Translate(), new Uri("Coding\\Pages\\CodingWithInputDetailsPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 98, 25);
			LabelSwitch labelSwitch;
			VisualDiagnostics.RegisterSourceInfo(labelSwitch = new LabelSwitch(), new Uri("Coding\\Pages\\CodingWithInputDetailsPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 94, 22);
			StackLayout stackLayout2;
			VisualDiagnostics.RegisterSourceInfo(stackLayout2 = new StackLayout(), new Uri("Coding\\Pages\\CodingWithInputDetailsPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 30, 18);
			ActivityFrame activityFrame;
			VisualDiagnostics.RegisterSourceInfo(activityFrame = new ActivityFrame(), new Uri("Coding\\Pages\\CodingWithInputDetailsPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 101, 18);
			Grid grid;
			VisualDiagnostics.RegisterSourceInfo(grid = new Grid(), new Uri("Coding\\Pages\\CodingWithInputDetailsPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 26, 14);
			ScrollView scrollView;
			VisualDiagnostics.RegisterSourceInfo(scrollView = new ScrollView(), new Uri("Coding\\Pages\\CodingWithInputDetailsPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 25, 10);
			VisualDiagnostics.RegisterSourceInfo(this, new Uri("Coding\\Pages\\CodingWithInputDetailsPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 2, 2);
			NameScope nameScope = NameScope.GetNameScope(this) ?? new NameScope();
			NameScope.SetNameScope(this, nameScope);
			nameScope.RegisterName("entryPassword", entry);
			if (entry.StyleId == null)
			{
				entry.StyleId = "entryPassword";
			}
			nameScope.RegisterName("labelState", label7);
			if (label7.StyleId == null)
			{
				label7.StyleId = "labelState";
			}
			nameScope.RegisterName("btnUpdateState", button);
			if (button.StyleId == null)
			{
				button.StyleId = "btnUpdateState";
			}
			nameScope.RegisterName("entryValue", entry2);
			if (entry2.StyleId == null)
			{
				entry2.StyleId = "entryValue";
			}
			nameScope.RegisterName("btnSaveNewValue", button2);
			if (button2.StyleId == null)
			{
				button2.StyleId = "btnSaveNewValue";
			}
			nameScope.RegisterName("activityFrame", activityFrame);
			if (activityFrame.StyleId == null)
			{
				activityFrame.StyleId = "activityFrame";
			}
			this.entryPassword = entry;
			this.labelState = label7;
			this.btnUpdateState = button;
			this.entryValue = entry2;
			this.btnSaveNewValue = button2;
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
			xmlNamespaceResolver.Add("d", "http://xamarin.com/schemas/2014/forms/design");
			xmlNamespaceResolver.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver.Add("mc", "http://schemas.openxmlformats.org/markup-compatibility/2006");
			xmlNamespaceResolver.Add("settings", "clr-namespace:CarScannerXamarinForms.Settings");
			xamlServiceProvider.Add(typeFromHandle2, new XamlTypeResolver(xmlNamespaceResolver, typeof(CodingWithInputDetailsPage).GetTypeInfo().Assembly));
			xamlServiceProvider.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(12, 5)));
			object obj2 = markupExtension.ProvideValue(xamlServiceProvider);
			this.Title = obj2;
			this.SetValue(Page.PaddingProperty, new Thickness(5.0, 0.0));
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
			xmlNamespaceResolver2.Add("d", "http://xamarin.com/schemas/2014/forms/design");
			xmlNamespaceResolver2.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver2.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver2.Add("mc", "http://schemas.openxmlformats.org/markup-compatibility/2006");
			xmlNamespaceResolver2.Add("settings", "clr-namespace:CarScannerXamarinForms.Settings");
			xamlServiceProvider2.Add(typeFromHandle4, new XamlTypeResolver(xmlNamespaceResolver2, typeof(CodingWithInputDetailsPage).GetTypeInfo().Assembly));
			xamlServiceProvider2.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(15, 5)));
			DynamicResource dynamicResource = markupExtension2.ProvideValue(xamlServiceProvider2);
			this.SetDynamicResource(VisualElement.BackgroundColorProperty, dynamicResource.Key);
			this.Resources = resourceDictionary;
			scrollView.SetValue(ScrollView.OrientationProperty, 0);
			rowDefinition.SetValue(RowDefinition.HeightProperty, new GridLengthTypeConverter().ConvertFromInvariantString("*"));
			grid.GetValue(Grid.RowDefinitionsProperty).Add(rowDefinition);
			stackLayout2.SetValue(Grid.RowProperty, 0);
			stackLayout2.SetValue(StackLayout.OrientationProperty, 0);
			label.SetValue(Label.FontAttributesProperty, new FontAttributesConverter().ConvertFromInvariantString("Bold"));
			dynamicResourceExtension2.Key = "BaseFontSize+++";
			IMarkupExtension<DynamicResource> markupExtension3 = dynamicResourceExtension2;
			XamlServiceProvider xamlServiceProvider3 = new XamlServiceProvider();
			Type typeFromHandle5 = typeof(IProvideValueTarget);
			object[] array3 = new object[0 + 5];
			array3[0] = label;
			array3[1] = stackLayout2;
			array3[2] = grid;
			array3[3] = scrollView;
			array3[4] = this;
			object obj4;
			xamlServiceProvider3.Add(typeFromHandle5, obj4 = new SimpleValueTargetProvider(array3, Label.FontSizeProperty, nameScope));
			xamlServiceProvider3.Add(typeof(IReferenceProvider), obj4);
			Type typeFromHandle6 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver3 = new XmlNamespaceResolver();
			xmlNamespaceResolver3.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver3.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver3.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver3.Add("d", "http://xamarin.com/schemas/2014/forms/design");
			xmlNamespaceResolver3.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver3.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver3.Add("mc", "http://schemas.openxmlformats.org/markup-compatibility/2006");
			xmlNamespaceResolver3.Add("settings", "clr-namespace:CarScannerXamarinForms.Settings");
			xamlServiceProvider3.Add(typeFromHandle6, new XamlTypeResolver(xmlNamespaceResolver3, typeof(CodingWithInputDetailsPage).GetTypeInfo().Assembly));
			xamlServiceProvider3.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(33, 25)));
			DynamicResource dynamicResource2 = markupExtension3.ProvideValue(xamlServiceProvider3);
			label.SetDynamicResource(Label.FontSizeProperty, dynamicResource2.Key);
			bindingExtension.Path = "Name";
			BindingBase bindingBase = bindingExtension.ProvideValue(null);
			label.SetBinding(Label.TextProperty, bindingBase);
			stackLayout2.Children.Add(label);
			staticResourceExtension.Key = "EmptyStringToFalseConverter";
			IMarkupExtension markupExtension4 = staticResourceExtension;
			XamlServiceProvider xamlServiceProvider4 = new XamlServiceProvider();
			Type typeFromHandle7 = typeof(IProvideValueTarget);
			object[] array4 = new object[0 + 6];
			array4[0] = bindingExtension2;
			array4[1] = label2;
			array4[2] = stackLayout2;
			array4[3] = grid;
			array4[4] = scrollView;
			array4[5] = this;
			object obj5;
			xamlServiceProvider4.Add(typeFromHandle7, obj5 = new SimpleValueTargetProvider(array4, typeof(BindingExtension).GetRuntimeProperty("Converter"), nameScope));
			xamlServiceProvider4.Add(typeof(IReferenceProvider), obj5);
			Type typeFromHandle8 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver4 = new XmlNamespaceResolver();
			xmlNamespaceResolver4.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver4.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver4.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver4.Add("d", "http://xamarin.com/schemas/2014/forms/design");
			xmlNamespaceResolver4.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver4.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver4.Add("mc", "http://schemas.openxmlformats.org/markup-compatibility/2006");
			xmlNamespaceResolver4.Add("settings", "clr-namespace:CarScannerXamarinForms.Settings");
			xamlServiceProvider4.Add(typeFromHandle8, new XamlTypeResolver(xmlNamespaceResolver4, typeof(CodingWithInputDetailsPage).GetTypeInfo().Assembly));
			xamlServiceProvider4.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(35, 28)));
			object obj6 = markupExtension4.ProvideValue(xamlServiceProvider4);
			bindingExtension2.Converter = obj6;
			bindingExtension2.Path = "Description";
			BindingBase bindingBase2 = bindingExtension2.ProvideValue(null);
			label2.SetBinding(VisualElement.IsVisibleProperty, bindingBase2);
			bindingExtension3.Path = "Description";
			BindingBase bindingBase3 = bindingExtension3.ProvideValue(null);
			label2.SetBinding(Label.TextProperty, bindingBase3);
			stackLayout2.Children.Add(label2);
			bindingExtension4.Mode = 2;
			staticResourceExtension2.Key = "EmptyStringToFalseConverter";
			IMarkupExtension markupExtension5 = staticResourceExtension2;
			XamlServiceProvider xamlServiceProvider5 = new XamlServiceProvider();
			Type typeFromHandle9 = typeof(IProvideValueTarget);
			object[] array5 = new object[0 + 6];
			array5[0] = bindingExtension4;
			array5[1] = label3;
			array5[2] = stackLayout2;
			array5[3] = grid;
			array5[4] = scrollView;
			array5[5] = this;
			object obj7;
			xamlServiceProvider5.Add(typeFromHandle9, obj7 = new SimpleValueTargetProvider(array5, typeof(BindingExtension).GetRuntimeProperty("Converter"), nameScope));
			xamlServiceProvider5.Add(typeof(IReferenceProvider), obj7);
			Type typeFromHandle10 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver5 = new XmlNamespaceResolver();
			xmlNamespaceResolver5.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver5.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver5.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver5.Add("d", "http://xamarin.com/schemas/2014/forms/design");
			xmlNamespaceResolver5.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver5.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver5.Add("mc", "http://schemas.openxmlformats.org/markup-compatibility/2006");
			xmlNamespaceResolver5.Add("settings", "clr-namespace:CarScannerXamarinForms.Settings");
			xamlServiceProvider5.Add(typeFromHandle10, new XamlTypeResolver(xmlNamespaceResolver5, typeof(CodingWithInputDetailsPage).GetTypeInfo().Assembly));
			xamlServiceProvider5.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(36, 28)));
			object obj8 = markupExtension5.ProvideValue(xamlServiceProvider5);
			bindingExtension4.Converter = obj8;
			bindingExtension4.Path = "InnerDescription";
			BindingBase bindingBase4 = bindingExtension4.ProvideValue(null);
			label3.SetBinding(VisualElement.IsVisibleProperty, bindingBase4);
			bindingExtension5.Path = "InnerDescription";
			BindingBase bindingBase5 = bindingExtension5.ProvideValue(null);
			label3.SetBinding(Label.TextProperty, bindingBase5);
			stackLayout2.Children.Add(label3);
			bindingExtension6.Path = "PasswordVisible";
			BindingBase bindingBase6 = bindingExtension6.ProvideValue(null);
			stackLayout.SetBinding(VisualElement.IsVisibleProperty, bindingBase6);
			stackLayout.SetValue(StackLayout.OrientationProperty, 0);
			translate2.Text = "coding_Password";
			IMarkupExtension markupExtension6 = translate2;
			XamlServiceProvider xamlServiceProvider6 = new XamlServiceProvider();
			Type typeFromHandle11 = typeof(IProvideValueTarget);
			object[] array6 = new object[0 + 6];
			array6[0] = label4;
			array6[1] = stackLayout;
			array6[2] = stackLayout2;
			array6[3] = grid;
			array6[4] = scrollView;
			array6[5] = this;
			object obj9;
			xamlServiceProvider6.Add(typeFromHandle11, obj9 = new SimpleValueTargetProvider(array6, Label.TextProperty, nameScope));
			xamlServiceProvider6.Add(typeof(IReferenceProvider), obj9);
			Type typeFromHandle12 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver6 = new XmlNamespaceResolver();
			xmlNamespaceResolver6.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver6.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver6.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver6.Add("d", "http://xamarin.com/schemas/2014/forms/design");
			xmlNamespaceResolver6.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver6.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver6.Add("mc", "http://schemas.openxmlformats.org/markup-compatibility/2006");
			xmlNamespaceResolver6.Add("settings", "clr-namespace:CarScannerXamarinForms.Settings");
			xamlServiceProvider6.Add(typeFromHandle12, new XamlTypeResolver(xmlNamespaceResolver6, typeof(CodingWithInputDetailsPage).GetTypeInfo().Assembly));
			xamlServiceProvider6.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(39, 32)));
			object obj10 = markupExtension6.ProvideValue(xamlServiceProvider6);
			label4.Text = obj10;
			stackLayout.Children.Add(label4);
			translate3.Text = "coding_PasswordNotRequired";
			IMarkupExtension markupExtension7 = translate3;
			XamlServiceProvider xamlServiceProvider7 = new XamlServiceProvider();
			Type typeFromHandle13 = typeof(IProvideValueTarget);
			object[] array7 = new object[0 + 6];
			array7[0] = entry;
			array7[1] = stackLayout;
			array7[2] = stackLayout2;
			array7[3] = grid;
			array7[4] = scrollView;
			array7[5] = this;
			object obj11;
			xamlServiceProvider7.Add(typeFromHandle13, obj11 = new SimpleValueTargetProvider(array7, Entry.PlaceholderProperty, nameScope));
			xamlServiceProvider7.Add(typeof(IReferenceProvider), obj11);
			Type typeFromHandle14 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver7 = new XmlNamespaceResolver();
			xmlNamespaceResolver7.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver7.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver7.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver7.Add("d", "http://xamarin.com/schemas/2014/forms/design");
			xmlNamespaceResolver7.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver7.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver7.Add("mc", "http://schemas.openxmlformats.org/markup-compatibility/2006");
			xmlNamespaceResolver7.Add("settings", "clr-namespace:CarScannerXamarinForms.Settings");
			xamlServiceProvider7.Add(typeFromHandle14, new XamlTypeResolver(xmlNamespaceResolver7, typeof(CodingWithInputDetailsPage).GetTypeInfo().Assembly));
			xamlServiceProvider7.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(42, 29)));
			object obj12 = markupExtension7.ProvideValue(xamlServiceProvider7);
			entry.Placeholder = obj12;
			bindingExtension7.Mode = 1;
			bindingExtension7.Path = "Password";
			BindingBase bindingBase7 = bindingExtension7.ProvideValue(null);
			entry.SetBinding(Entry.TextProperty, bindingBase7);
			stackLayout.Children.Add(entry);
			label5.SetValue(Label.FontAttributesProperty, new FontAttributesConverter().ConvertFromInvariantString("Italic"));
			staticResourceExtension3.Key = "EmptyStringToFalseConverter";
			IMarkupExtension markupExtension8 = staticResourceExtension3;
			XamlServiceProvider xamlServiceProvider8 = new XamlServiceProvider();
			Type typeFromHandle15 = typeof(IProvideValueTarget);
			object[] array8 = new object[0 + 7];
			array8[0] = bindingExtension8;
			array8[1] = label5;
			array8[2] = stackLayout;
			array8[3] = stackLayout2;
			array8[4] = grid;
			array8[5] = scrollView;
			array8[6] = this;
			object obj13;
			xamlServiceProvider8.Add(typeFromHandle15, obj13 = new SimpleValueTargetProvider(array8, typeof(BindingExtension).GetRuntimeProperty("Converter"), nameScope));
			xamlServiceProvider8.Add(typeof(IReferenceProvider), obj13);
			Type typeFromHandle16 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver8 = new XmlNamespaceResolver();
			xmlNamespaceResolver8.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver8.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver8.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver8.Add("d", "http://xamarin.com/schemas/2014/forms/design");
			xmlNamespaceResolver8.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver8.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver8.Add("mc", "http://schemas.openxmlformats.org/markup-compatibility/2006");
			xmlNamespaceResolver8.Add("settings", "clr-namespace:CarScannerXamarinForms.Settings");
			xamlServiceProvider8.Add(typeFromHandle16, new XamlTypeResolver(xmlNamespaceResolver8, typeof(CodingWithInputDetailsPage).GetTypeInfo().Assembly));
			xamlServiceProvider8.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(44, 56)));
			object obj14 = markupExtension8.ProvideValue(xamlServiceProvider8);
			bindingExtension8.Converter = obj14;
			bindingExtension8.Path = "PasswordHint";
			BindingBase bindingBase8 = bindingExtension8.ProvideValue(null);
			label5.SetBinding(VisualElement.IsVisibleProperty, bindingBase8);
			translate4.Text = "coding_PasswordHint";
			IMarkupExtension markupExtension9 = translate4;
			XamlServiceProvider xamlServiceProvider9 = new XamlServiceProvider();
			Type typeFromHandle17 = typeof(IProvideValueTarget);
			object[] array9 = new object[0 + 8];
			array9[0] = span;
			array9[1] = formattedString;
			array9[2] = label5;
			array9[3] = stackLayout;
			array9[4] = stackLayout2;
			array9[5] = grid;
			array9[6] = scrollView;
			array9[7] = this;
			object obj15;
			xamlServiceProvider9.Add(typeFromHandle17, obj15 = new SimpleValueTargetProvider(array9, Span.TextProperty, nameScope));
			xamlServiceProvider9.Add(typeof(IReferenceProvider), obj15);
			Type typeFromHandle18 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver9 = new XmlNamespaceResolver();
			xmlNamespaceResolver9.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver9.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver9.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver9.Add("d", "http://xamarin.com/schemas/2014/forms/design");
			xmlNamespaceResolver9.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver9.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver9.Add("mc", "http://schemas.openxmlformats.org/markup-compatibility/2006");
			xmlNamespaceResolver9.Add("settings", "clr-namespace:CarScannerXamarinForms.Settings");
			xamlServiceProvider9.Add(typeFromHandle18, new XamlTypeResolver(xmlNamespaceResolver9, typeof(CodingWithInputDetailsPage).GetTypeInfo().Assembly));
			xamlServiceProvider9.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(47, 43)));
			object obj16 = markupExtension9.ProvideValue(xamlServiceProvider9);
			span.Text = obj16;
			formattedString.Spans.Add(span);
			bindingExtension9.Path = "PasswordHint";
			BindingBase bindingBase9 = bindingExtension9.ProvideValue(null);
			span2.SetBinding(Span.TextProperty, bindingBase9);
			formattedString.Spans.Add(span2);
			label5.SetValue(Label.FormattedTextProperty, formattedString);
			stackLayout.Children.Add(label5);
			label6.SetValue(Label.FontAttributesProperty, new FontAttributesConverter().ConvertFromInvariantString("Italic"));
			bindingExtension10.Mode = 2;
			staticResourceExtension4.Key = "EmptyStringToTrueConverter";
			IMarkupExtension markupExtension10 = staticResourceExtension4;
			XamlServiceProvider xamlServiceProvider10 = new XamlServiceProvider();
			Type typeFromHandle19 = typeof(IProvideValueTarget);
			object[] array10 = new object[0 + 7];
			array10[0] = bindingExtension10;
			array10[1] = label6;
			array10[2] = stackLayout;
			array10[3] = stackLayout2;
			array10[4] = grid;
			array10[5] = scrollView;
			array10[6] = this;
			object obj17;
			xamlServiceProvider10.Add(typeFromHandle19, obj17 = new SimpleValueTargetProvider(array10, typeof(BindingExtension).GetRuntimeProperty("Converter"), nameScope));
			xamlServiceProvider10.Add(typeof(IReferenceProvider), obj17);
			Type typeFromHandle20 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver10 = new XmlNamespaceResolver();
			xmlNamespaceResolver10.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver10.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver10.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver10.Add("d", "http://xamarin.com/schemas/2014/forms/design");
			xmlNamespaceResolver10.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver10.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver10.Add("mc", "http://schemas.openxmlformats.org/markup-compatibility/2006");
			xmlNamespaceResolver10.Add("settings", "clr-namespace:CarScannerXamarinForms.Settings");
			xamlServiceProvider10.Add(typeFromHandle20, new XamlTypeResolver(xmlNamespaceResolver10, typeof(CodingWithInputDetailsPage).GetTypeInfo().Assembly));
			xamlServiceProvider10.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(54, 29)));
			object obj18 = markupExtension10.ProvideValue(xamlServiceProvider10);
			bindingExtension10.Converter = obj18;
			bindingExtension10.Path = "Password";
			BindingBase bindingBase10 = bindingExtension10.ProvideValue(null);
			label6.SetBinding(VisualElement.IsVisibleProperty, bindingBase10);
			translate5.Text = "coding_PasswordNotRequired";
			IMarkupExtension markupExtension11 = translate5;
			XamlServiceProvider xamlServiceProvider11 = new XamlServiceProvider();
			Type typeFromHandle21 = typeof(IProvideValueTarget);
			object[] array11 = new object[0 + 6];
			array11[0] = label6;
			array11[1] = stackLayout;
			array11[2] = stackLayout2;
			array11[3] = grid;
			array11[4] = scrollView;
			array11[5] = this;
			object obj19;
			xamlServiceProvider11.Add(typeFromHandle21, obj19 = new SimpleValueTargetProvider(array11, Label.TextProperty, nameScope));
			xamlServiceProvider11.Add(typeof(IReferenceProvider), obj19);
			Type typeFromHandle22 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver11 = new XmlNamespaceResolver();
			xmlNamespaceResolver11.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver11.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver11.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver11.Add("d", "http://xamarin.com/schemas/2014/forms/design");
			xmlNamespaceResolver11.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver11.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver11.Add("mc", "http://schemas.openxmlformats.org/markup-compatibility/2006");
			xmlNamespaceResolver11.Add("settings", "clr-namespace:CarScannerXamarinForms.Settings");
			xamlServiceProvider11.Add(typeFromHandle22, new XamlTypeResolver(xmlNamespaceResolver11, typeof(CodingWithInputDetailsPage).GetTypeInfo().Assembly));
			xamlServiceProvider11.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(55, 29)));
			object obj20 = markupExtension11.ProvideValue(xamlServiceProvider11);
			label6.Text = obj20;
			stackLayout.Children.Add(label6);
			stackLayout2.Children.Add(stackLayout);
			bindingExtension11.Path = "HasCurrentState";
			BindingBase bindingBase11 = bindingExtension11.ProvideValue(null);
			label7.SetBinding(VisualElement.IsVisibleProperty, bindingBase11);
			translate6.Text = "coding_CurrentState";
			IMarkupExtension markupExtension12 = translate6;
			XamlServiceProvider xamlServiceProvider12 = new XamlServiceProvider();
			Type typeFromHandle23 = typeof(IProvideValueTarget);
			object[] array12 = new object[0 + 7];
			array12[0] = span3;
			array12[1] = formattedString2;
			array12[2] = label7;
			array12[3] = stackLayout2;
			array12[4] = grid;
			array12[5] = scrollView;
			array12[6] = this;
			object obj21;
			xamlServiceProvider12.Add(typeFromHandle23, obj21 = new SimpleValueTargetProvider(array12, Span.TextProperty, nameScope));
			xamlServiceProvider12.Add(typeof(IReferenceProvider), obj21);
			Type typeFromHandle24 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver12 = new XmlNamespaceResolver();
			xmlNamespaceResolver12.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver12.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver12.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver12.Add("d", "http://xamarin.com/schemas/2014/forms/design");
			xmlNamespaceResolver12.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver12.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver12.Add("mc", "http://schemas.openxmlformats.org/markup-compatibility/2006");
			xmlNamespaceResolver12.Add("settings", "clr-namespace:CarScannerXamarinForms.Settings");
			xamlServiceProvider12.Add(typeFromHandle24, new XamlTypeResolver(xmlNamespaceResolver12, typeof(CodingWithInputDetailsPage).GetTypeInfo().Assembly));
			xamlServiceProvider12.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(60, 39)));
			object obj22 = markupExtension12.ProvideValue(xamlServiceProvider12);
			span3.Text = obj22;
			formattedString2.Spans.Add(span3);
			span4.SetValue(Span.TextProperty, " ");
			formattedString2.Spans.Add(span4);
			span5.SetValue(Span.FontAttributesProperty, new FontAttributesConverter().ConvertFromInvariantString("Bold"));
			bindingExtension12.Mode = 2;
			bindingExtension12.Path = "CurrentState";
			BindingBase bindingBase12 = bindingExtension12.ProvideValue(null);
			span5.SetBinding(Span.TextProperty, bindingBase12);
			formattedString2.Spans.Add(span5);
			label7.SetValue(Label.FormattedTextProperty, formattedString2);
			stackLayout2.Children.Add(label7);
			button.Clicked += this.BtnUpdateState_Clicked;
			bindingExtension13.Path = "HasCurrentState";
			BindingBase bindingBase13 = bindingExtension13.ProvideValue(null);
			button.SetBinding(VisualElement.IsVisibleProperty, bindingBase13);
			translate7.Text = "Mode06Page_btnRefresh.Content";
			IMarkupExtension markupExtension13 = translate7;
			XamlServiceProvider xamlServiceProvider13 = new XamlServiceProvider();
			Type typeFromHandle25 = typeof(IProvideValueTarget);
			object[] array13 = new object[0 + 5];
			array13[0] = button;
			array13[1] = stackLayout2;
			array13[2] = grid;
			array13[3] = scrollView;
			array13[4] = this;
			object obj23;
			xamlServiceProvider13.Add(typeFromHandle25, obj23 = new SimpleValueTargetProvider(array13, Button.TextProperty, nameScope));
			xamlServiceProvider13.Add(typeof(IReferenceProvider), obj23);
			Type typeFromHandle26 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver13 = new XmlNamespaceResolver();
			xmlNamespaceResolver13.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver13.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver13.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver13.Add("d", "http://xamarin.com/schemas/2014/forms/design");
			xmlNamespaceResolver13.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver13.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver13.Add("mc", "http://schemas.openxmlformats.org/markup-compatibility/2006");
			xmlNamespaceResolver13.Add("settings", "clr-namespace:CarScannerXamarinForms.Settings");
			xamlServiceProvider13.Add(typeFromHandle26, new XamlTypeResolver(xmlNamespaceResolver13, typeof(CodingWithInputDetailsPage).GetTypeInfo().Assembly));
			xamlServiceProvider13.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(70, 25)));
			object obj24 = markupExtension13.ProvideValue(xamlServiceProvider13);
			button.Text = obj24;
			stackLayout2.Children.Add(button);
			translate8.Text = "coding_TypeNewValue";
			IMarkupExtension markupExtension14 = translate8;
			XamlServiceProvider xamlServiceProvider14 = new XamlServiceProvider();
			Type typeFromHandle27 = typeof(IProvideValueTarget);
			object[] array14 = new object[0 + 5];
			array14[0] = label8;
			array14[1] = stackLayout2;
			array14[2] = grid;
			array14[3] = scrollView;
			array14[4] = this;
			object obj25;
			xamlServiceProvider14.Add(typeFromHandle27, obj25 = new SimpleValueTargetProvider(array14, Label.TextProperty, nameScope));
			xamlServiceProvider14.Add(typeof(IReferenceProvider), obj25);
			Type typeFromHandle28 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver14 = new XmlNamespaceResolver();
			xmlNamespaceResolver14.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver14.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver14.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver14.Add("d", "http://xamarin.com/schemas/2014/forms/design");
			xmlNamespaceResolver14.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver14.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver14.Add("mc", "http://schemas.openxmlformats.org/markup-compatibility/2006");
			xmlNamespaceResolver14.Add("settings", "clr-namespace:CarScannerXamarinForms.Settings");
			xamlServiceProvider14.Add(typeFromHandle28, new XamlTypeResolver(xmlNamespaceResolver14, typeof(CodingWithInputDetailsPage).GetTypeInfo().Assembly));
			xamlServiceProvider14.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(71, 28)));
			object obj26 = markupExtension14.ProvideValue(xamlServiceProvider14);
			label8.Text = obj26;
			stackLayout2.Children.Add(label8);
			bindingExtension14.Mode = 2;
			bindingExtension14.Path = "CurrentState";
			BindingBase bindingBase14 = bindingExtension14.ProvideValue(null);
			entry2.SetBinding(Entry.TextProperty, bindingBase14);
			stackLayout2.Children.Add(entry2);
			button2.Clicked += this.btnSaveState_Clicked;
			translate9.Text = "coding_Apply";
			IMarkupExtension markupExtension15 = translate9;
			XamlServiceProvider xamlServiceProvider15 = new XamlServiceProvider();
			Type typeFromHandle29 = typeof(IProvideValueTarget);
			object[] array15 = new object[0 + 5];
			array15[0] = button2;
			array15[1] = stackLayout2;
			array15[2] = grid;
			array15[3] = scrollView;
			array15[4] = this;
			object obj27;
			xamlServiceProvider15.Add(typeFromHandle29, obj27 = new SimpleValueTargetProvider(array15, Button.TextProperty, nameScope));
			xamlServiceProvider15.Add(typeof(IReferenceProvider), obj27);
			Type typeFromHandle30 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver15 = new XmlNamespaceResolver();
			xmlNamespaceResolver15.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver15.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver15.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver15.Add("d", "http://xamarin.com/schemas/2014/forms/design");
			xmlNamespaceResolver15.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver15.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver15.Add("mc", "http://schemas.openxmlformats.org/markup-compatibility/2006");
			xmlNamespaceResolver15.Add("settings", "clr-namespace:CarScannerXamarinForms.Settings");
			xamlServiceProvider15.Add(typeFromHandle30, new XamlTypeResolver(xmlNamespaceResolver15, typeof(CodingWithInputDetailsPage).GetTypeInfo().Assembly));
			xamlServiceProvider15.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(76, 25)));
			object obj28 = markupExtension15.ProvideValue(xamlServiceProvider15);
			button2.Text = obj28;
			stackLayout2.Children.Add(button2);
			label9.SetValue(BindableObject.BindingContextProperty, sharedSettings);
			label9.SetValue(Label.FontAttributesProperty, new FontAttributesConverter().ConvertFromInvariantString("Bold"));
			dynamicResourceExtension3.Key = "BaseFontSize-";
			IMarkupExtension<DynamicResource> markupExtension16 = dynamicResourceExtension3;
			XamlServiceProvider xamlServiceProvider16 = new XamlServiceProvider();
			Type typeFromHandle31 = typeof(IProvideValueTarget);
			object[] array16 = new object[0 + 5];
			array16[0] = label9;
			array16[1] = stackLayout2;
			array16[2] = grid;
			array16[3] = scrollView;
			array16[4] = this;
			object obj29;
			xamlServiceProvider16.Add(typeFromHandle31, obj29 = new SimpleValueTargetProvider(array16, Label.FontSizeProperty, nameScope));
			xamlServiceProvider16.Add(typeof(IReferenceProvider), obj29);
			Type typeFromHandle32 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver16 = new XmlNamespaceResolver();
			xmlNamespaceResolver16.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver16.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver16.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver16.Add("d", "http://xamarin.com/schemas/2014/forms/design");
			xmlNamespaceResolver16.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver16.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver16.Add("mc", "http://schemas.openxmlformats.org/markup-compatibility/2006");
			xmlNamespaceResolver16.Add("settings", "clr-namespace:CarScannerXamarinForms.Settings");
			xamlServiceProvider16.Add(typeFromHandle32, new XamlTypeResolver(xmlNamespaceResolver16, typeof(CodingWithInputDetailsPage).GetTypeInfo().Assembly));
			xamlServiceProvider16.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(85, 25)));
			DynamicResource dynamicResource3 = markupExtension16.ProvideValue(xamlServiceProvider16);
			label9.SetDynamicResource(Label.FontSizeProperty, dynamicResource3.Key);
			staticResourceExtension5.Key = "VagCodingPlatformToTrueConverter";
			IMarkupExtension markupExtension17 = staticResourceExtension5;
			XamlServiceProvider xamlServiceProvider17 = new XamlServiceProvider();
			Type typeFromHandle33 = typeof(IProvideValueTarget);
			object[] array17 = new object[0 + 6];
			array17[0] = bindingExtension15;
			array17[1] = label9;
			array17[2] = stackLayout2;
			array17[3] = grid;
			array17[4] = scrollView;
			array17[5] = this;
			object obj30;
			xamlServiceProvider17.Add(typeFromHandle33, obj30 = new SimpleValueTargetProvider(array17, typeof(BindingExtension).GetRuntimeProperty("Converter"), nameScope));
			xamlServiceProvider17.Add(typeof(IReferenceProvider), obj30);
			Type typeFromHandle34 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver17 = new XmlNamespaceResolver();
			xmlNamespaceResolver17.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver17.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver17.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver17.Add("d", "http://xamarin.com/schemas/2014/forms/design");
			xmlNamespaceResolver17.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver17.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver17.Add("mc", "http://schemas.openxmlformats.org/markup-compatibility/2006");
			xmlNamespaceResolver17.Add("settings", "clr-namespace:CarScannerXamarinForms.Settings");
			xamlServiceProvider17.Add(typeFromHandle34, new XamlTypeResolver(xmlNamespaceResolver17, typeof(CodingWithInputDetailsPage).GetTypeInfo().Assembly));
			xamlServiceProvider17.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(86, 25)));
			object obj31 = markupExtension17.ProvideValue(xamlServiceProvider17);
			bindingExtension15.Converter = obj31;
			bindingExtension15.Path = "CodingLastPlatformSelected";
			BindingBase bindingBase15 = bindingExtension15.ProvideValue(null);
			label9.SetBinding(VisualElement.IsVisibleProperty, bindingBase15);
			translate10.Text = "coding_VagOpenHoodWarning";
			IMarkupExtension markupExtension18 = translate10;
			XamlServiceProvider xamlServiceProvider18 = new XamlServiceProvider();
			Type typeFromHandle35 = typeof(IProvideValueTarget);
			object[] array18 = new object[0 + 5];
			array18[0] = label9;
			array18[1] = stackLayout2;
			array18[2] = grid;
			array18[3] = scrollView;
			array18[4] = this;
			object obj32;
			xamlServiceProvider18.Add(typeFromHandle35, obj32 = new SimpleValueTargetProvider(array18, Label.TextProperty, nameScope));
			xamlServiceProvider18.Add(typeof(IReferenceProvider), obj32);
			Type typeFromHandle36 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver18 = new XmlNamespaceResolver();
			xmlNamespaceResolver18.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver18.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver18.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver18.Add("d", "http://xamarin.com/schemas/2014/forms/design");
			xmlNamespaceResolver18.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver18.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver18.Add("mc", "http://schemas.openxmlformats.org/markup-compatibility/2006");
			xmlNamespaceResolver18.Add("settings", "clr-namespace:CarScannerXamarinForms.Settings");
			xamlServiceProvider18.Add(typeFromHandle36, new XamlTypeResolver(xmlNamespaceResolver18, typeof(CodingWithInputDetailsPage).GetTypeInfo().Assembly));
			xamlServiceProvider18.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(87, 25)));
			object obj33 = markupExtension18.ProvideValue(xamlServiceProvider18);
			label9.Text = obj33;
			stackLayout2.Children.Add(label9);
			label10.SetValue(Label.FontAttributesProperty, new FontAttributesConverter().ConvertFromInvariantString("Italic"));
			dynamicResourceExtension4.Key = "BaseFontSize-";
			IMarkupExtension<DynamicResource> markupExtension19 = dynamicResourceExtension4;
			XamlServiceProvider xamlServiceProvider19 = new XamlServiceProvider();
			Type typeFromHandle37 = typeof(IProvideValueTarget);
			object[] array19 = new object[0 + 5];
			array19[0] = label10;
			array19[1] = stackLayout2;
			array19[2] = grid;
			array19[3] = scrollView;
			array19[4] = this;
			object obj34;
			xamlServiceProvider19.Add(typeFromHandle37, obj34 = new SimpleValueTargetProvider(array19, Label.FontSizeProperty, nameScope));
			xamlServiceProvider19.Add(typeof(IReferenceProvider), obj34);
			Type typeFromHandle38 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver19 = new XmlNamespaceResolver();
			xmlNamespaceResolver19.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver19.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver19.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver19.Add("d", "http://xamarin.com/schemas/2014/forms/design");
			xmlNamespaceResolver19.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver19.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver19.Add("mc", "http://schemas.openxmlformats.org/markup-compatibility/2006");
			xmlNamespaceResolver19.Add("settings", "clr-namespace:CarScannerXamarinForms.Settings");
			xamlServiceProvider19.Add(typeFromHandle38, new XamlTypeResolver(xmlNamespaceResolver19, typeof(CodingWithInputDetailsPage).GetTypeInfo().Assembly));
			xamlServiceProvider19.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(90, 25)));
			DynamicResource dynamicResource4 = markupExtension19.ProvideValue(xamlServiceProvider19);
			label10.SetDynamicResource(Label.FontSizeProperty, dynamicResource4.Key);
			translate11.Text = "coding_UseHistory";
			IMarkupExtension markupExtension20 = translate11;
			XamlServiceProvider xamlServiceProvider20 = new XamlServiceProvider();
			Type typeFromHandle39 = typeof(IProvideValueTarget);
			object[] array20 = new object[0 + 5];
			array20[0] = label10;
			array20[1] = stackLayout2;
			array20[2] = grid;
			array20[3] = scrollView;
			array20[4] = this;
			object obj35;
			xamlServiceProvider20.Add(typeFromHandle39, obj35 = new SimpleValueTargetProvider(array20, Label.TextProperty, nameScope));
			xamlServiceProvider20.Add(typeof(IReferenceProvider), obj35);
			Type typeFromHandle40 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver20 = new XmlNamespaceResolver();
			xmlNamespaceResolver20.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver20.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver20.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver20.Add("d", "http://xamarin.com/schemas/2014/forms/design");
			xmlNamespaceResolver20.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver20.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver20.Add("mc", "http://schemas.openxmlformats.org/markup-compatibility/2006");
			xmlNamespaceResolver20.Add("settings", "clr-namespace:CarScannerXamarinForms.Settings");
			xamlServiceProvider20.Add(typeFromHandle40, new XamlTypeResolver(xmlNamespaceResolver20, typeof(CodingWithInputDetailsPage).GetTypeInfo().Assembly));
			xamlServiceProvider20.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(91, 25)));
			object obj36 = markupExtension20.ProvideValue(xamlServiceProvider20);
			label10.Text = obj36;
			stackLayout2.Children.Add(label10);
			labelSwitch.SetValue(BindableObject.BindingContextProperty, sharedSettings2);
			bindingExtension16.Mode = 1;
			bindingExtension16.Path = "IgnoreCodingErrors";
			BindingBase bindingBase16 = bindingExtension16.ProvideValue(null);
			labelSwitch.SetBinding(LabelSwitch.IsToggledProperty, bindingBase16);
			bindingExtension17.Path = "ShowExperimental";
			BindingBase bindingBase17 = bindingExtension17.ProvideValue(null);
			labelSwitch.SetBinding(VisualElement.IsVisibleProperty, bindingBase17);
			translate12.Text = "coding_ignore_fails";
			IMarkupExtension markupExtension21 = translate12;
			XamlServiceProvider xamlServiceProvider21 = new XamlServiceProvider();
			Type typeFromHandle41 = typeof(IProvideValueTarget);
			object[] array21 = new object[0 + 5];
			array21[0] = labelSwitch;
			array21[1] = stackLayout2;
			array21[2] = grid;
			array21[3] = scrollView;
			array21[4] = this;
			object obj37;
			xamlServiceProvider21.Add(typeFromHandle41, obj37 = new SimpleValueTargetProvider(array21, LabelSwitch.TextProperty, nameScope));
			xamlServiceProvider21.Add(typeof(IReferenceProvider), obj37);
			Type typeFromHandle42 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver21 = new XmlNamespaceResolver();
			xmlNamespaceResolver21.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver21.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver21.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver21.Add("d", "http://xamarin.com/schemas/2014/forms/design");
			xmlNamespaceResolver21.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver21.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver21.Add("mc", "http://schemas.openxmlformats.org/markup-compatibility/2006");
			xmlNamespaceResolver21.Add("settings", "clr-namespace:CarScannerXamarinForms.Settings");
			xamlServiceProvider21.Add(typeFromHandle42, new XamlTypeResolver(xmlNamespaceResolver21, typeof(CodingWithInputDetailsPage).GetTypeInfo().Assembly));
			xamlServiceProvider21.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(98, 25)));
			object obj38 = markupExtension21.ProvideValue(xamlServiceProvider21);
			labelSwitch.Text = obj38;
			stackLayout2.Children.Add(labelSwitch);
			grid.Children.Add(stackLayout2);
			activityFrame.SetValue(Grid.RowProperty, 0);
			grid.Children.Add(activityFrame);
			scrollView.Content = grid;
			this.SetValue(ContentPage.ContentProperty, scrollView);
		}

		// Token: 0x06004D74 RID: 19828 RVA: 0x0039C449 File Offset: 0x0039A649
		[CompilerGenerated]
		private void <btnSaveState_Clicked>b__6_0(string s)
		{
			Device.BeginInvokeOnMainThread(delegate
			{
				this.activityFrame.Text = s;
			});
		}

		// Token: 0x06004D75 RID: 19829 RVA: 0x0039C470 File Offset: 0x0039A670
		private void __InitComponentRuntime()
		{
			Extensions.LoadFromXaml<CodingWithInputDetailsPage>(this, typeof(CodingWithInputDetailsPage));
			this.entryPassword = NameScopeExtensions.FindByName<Entry>(this, "entryPassword");
			this.labelState = NameScopeExtensions.FindByName<Label>(this, "labelState");
			this.btnUpdateState = NameScopeExtensions.FindByName<Button>(this, "btnUpdateState");
			this.entryValue = NameScopeExtensions.FindByName<Entry>(this, "entryValue");
			this.btnSaveNewValue = NameScopeExtensions.FindByName<Button>(this, "btnSaveNewValue");
			this.activityFrame = NameScopeExtensions.FindByName<ActivityFrame>(this, "activityFrame");
		}

		// Token: 0x04002DF3 RID: 11763
		private bool first_appearing = true;

		// Token: 0x04002DF4 RID: 11764
		private ICodingContainer coding;

		// Token: 0x04002DF5 RID: 11765
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private Entry entryPassword;

		// Token: 0x04002DF6 RID: 11766
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private Label labelState;

		// Token: 0x04002DF7 RID: 11767
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private Button btnUpdateState;

		// Token: 0x04002DF8 RID: 11768
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private Entry entryValue;

		// Token: 0x04002DF9 RID: 11769
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private Button btnSaveNewValue;

		// Token: 0x04002DFA RID: 11770
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private ActivityFrame activityFrame;

		// Token: 0x02000905 RID: 2309
		[CompilerGenerated]
		private sealed class <>c__DisplayClass6_0
		{
			// Token: 0x06004D76 RID: 19830 RVA: 0x00002050 File Offset: 0x00000250
			public <>c__DisplayClass6_0()
			{
			}

			// Token: 0x06004D77 RID: 19831 RVA: 0x0039C4F4 File Offset: 0x0039A6F4
			internal void <btnSaveState_Clicked>b__1()
			{
				this.<>4__this.activityFrame.Text = this.s;
			}

			// Token: 0x04002DFB RID: 11771
			public string s;

			// Token: 0x04002DFC RID: 11772
			public CodingWithInputDetailsPage <>4__this;
		}

		// Token: 0x02000906 RID: 2310
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <BtnUpdateState_Clicked>d__4 : IAsyncStateMachine
		{
			// Token: 0x06004D78 RID: 19832 RVA: 0x0039C50C File Offset: 0x0039A70C
			void IAsyncStateMachine.MoveNext()
			{
				CodingWithInputDetailsPage codingWithInputDetailsPage = this;
				try
				{
					codingWithInputDetailsPage.UpdateState();
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

			// Token: 0x06004D79 RID: 19833 RVA: 0x0039C564 File Offset: 0x0039A764
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x04002DFD RID: 11773
			public int <>1__state;

			// Token: 0x04002DFE RID: 11774
			public AsyncVoidMethodBuilder <>t__builder;

			// Token: 0x04002DFF RID: 11775
			public CodingWithInputDetailsPage <>4__this;
		}

		// Token: 0x02000907 RID: 2311
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <CodingDetailsPage_Appearing>d__2 : IAsyncStateMachine
		{
			// Token: 0x06004D7A RID: 19834 RVA: 0x0039C574 File Offset: 0x0039A774
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				CodingWithInputDetailsPage codingWithInputDetailsPage = this;
				try
				{
					TaskAwaiter<CodingRequestResult> taskAwaiter;
					if (num != 0)
					{
						if (!codingWithInputDetailsPage.first_appearing)
						{
							goto IL_00B7;
						}
						codingWithInputDetailsPage.first_appearing = false;
						codingWithInputDetailsPage.activityFrame.IsVisible = true;
						codingWithInputDetailsPage.entryValue.IsEnabled = false;
						taskAwaiter = codingWithInputDetailsPage.coding.UpdateCurrentState("", null).GetAwaiter();
						if (!taskAwaiter.IsCompleted)
						{
							num2 = 0;
							TaskAwaiter<CodingRequestResult> taskAwaiter2 = taskAwaiter;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<CodingRequestResult>, CodingWithInputDetailsPage.<CodingDetailsPage_Appearing>d__2>(ref taskAwaiter, ref this);
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
					codingWithInputDetailsPage.activityFrame.IsVisible = false;
					codingWithInputDetailsPage.entryValue.IsEnabled = true;
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

			// Token: 0x06004D7B RID: 19835 RVA: 0x0039C674 File Offset: 0x0039A874
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x04002E00 RID: 11776
			public int <>1__state;

			// Token: 0x04002E01 RID: 11777
			public AsyncVoidMethodBuilder <>t__builder;

			// Token: 0x04002E02 RID: 11778
			public CodingWithInputDetailsPage <>4__this;

			// Token: 0x04002E03 RID: 11779
			private TaskAwaiter<CodingRequestResult> <>u__1;
		}

		// Token: 0x02000908 RID: 2312
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <UpdateState>d__3 : IAsyncStateMachine
		{
			// Token: 0x06004D7C RID: 19836 RVA: 0x0039C684 File Offset: 0x0039A884
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				CodingWithInputDetailsPage codingWithInputDetailsPage = this;
				try
				{
					TaskAwaiter<CodingRequestResult> taskAwaiter;
					if (num != 0)
					{
						codingWithInputDetailsPage.activityFrame.IsVisible = true;
						codingWithInputDetailsPage.entryValue.IsEnabled = false;
						taskAwaiter = codingWithInputDetailsPage.coding.UpdateCurrentState("", null).GetAwaiter();
						if (!taskAwaiter.IsCompleted)
						{
							num2 = 0;
							TaskAwaiter<CodingRequestResult> taskAwaiter2 = taskAwaiter;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<CodingRequestResult>, CodingWithInputDetailsPage.<UpdateState>d__3>(ref taskAwaiter, ref this);
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
					codingWithInputDetailsPage.activityFrame.IsVisible = false;
					codingWithInputDetailsPage.entryValue.IsEnabled = true;
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

			// Token: 0x06004D7D RID: 19837 RVA: 0x0039C774 File Offset: 0x0039A974
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x04002E04 RID: 11780
			public int <>1__state;

			// Token: 0x04002E05 RID: 11781
			public AsyncTaskMethodBuilder <>t__builder;

			// Token: 0x04002E06 RID: 11782
			public CodingWithInputDetailsPage <>4__this;

			// Token: 0x04002E07 RID: 11783
			private TaskAwaiter<CodingRequestResult> <>u__1;
		}

		// Token: 0x02000909 RID: 2313
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <btnSaveState_Clicked>d__6 : IAsyncStateMachine
		{
			// Token: 0x06004D7E RID: 19838 RVA: 0x0039C784 File Offset: 0x0039A984
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				CodingWithInputDetailsPage codingWithInputDetailsPage = this;
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
						if (double.TryParse(codingWithInputDetailsPage.entryValue.Text, out num3) || codingWithInputDetailsPage.coding is MQBMode22ParametrizeSilentDump)
						{
							if (codingWithInputDetailsPage.coding.RequiresPro && !SharedSettings.Current.AdsProductPurchased)
							{
								taskAwaiter3 = codingWithInputDetailsPage.DisplayAlert("Car Scanner Pro", Translate.GetString("FeatureLocked"), "OK", Translate.GetString("btnCancel.Content")).GetAwaiter();
								if (!taskAwaiter3.IsCompleted)
								{
									num2 = 0;
									taskAwaiter2 = taskAwaiter3;
									this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<bool>, CodingWithInputDetailsPage.<btnSaveState_Clicked>d__6>(ref taskAwaiter3, ref this);
									return;
								}
							}
							else if (SharedSettings.Current.CodingsCounter >= 3 && !SharedSettings.Current.AdsProductPurchased)
							{
								taskAwaiter3 = codingWithInputDetailsPage.DisplayAlert("Car Scanner Pro", string.Format(Translate.GetString("coding_FreeCodingsFinished"), SharedSettings.Current.CodingsCounter, 3), "OK", Translate.GetString("btnCancel.Content")).GetAwaiter();
								if (!taskAwaiter3.IsCompleted)
								{
									num2 = 2;
									taskAwaiter2 = taskAwaiter3;
									this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<bool>, CodingWithInputDetailsPage.<btnSaveState_Clicked>d__6>(ref taskAwaiter3, ref this);
									return;
								}
								goto IL_0214;
							}
							else
							{
								codingWithInputDetailsPage.activityFrame.IsVisible = true;
								codingWithInputDetailsPage.entryValue.IsEnabled = false;
								Progress<string> progress = new Progress<string>(delegate(string s)
								{
									Device.BeginInvokeOnMainThread(new Action(new CodingWithInputDetailsPage.<>c__DisplayClass6_0
									{
										<>4__this = codingWithInputDetailsPage,
										s = s
									}.<btnSaveState_Clicked>b__1));
								});
								taskAwaiter6 = codingWithInputDetailsPage.coding.Execute(codingWithInputDetailsPage.entryPassword.Text, codingWithInputDetailsPage.entryValue.Text, codingWithInputDetailsPage.entryValue.Text, progress, null, false).GetAwaiter();
								if (!taskAwaiter6.IsCompleted)
								{
									num2 = 4;
									TaskAwaiter<CodingRequestResult> taskAwaiter7 = taskAwaiter6;
									this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<CodingRequestResult>, CodingWithInputDetailsPage.<btnSaveState_Clicked>d__6>(ref taskAwaiter6, ref this);
									return;
								}
								goto IL_0338;
							}
						}
						else
						{
							taskAwaiter4 = codingWithInputDetailsPage.DisplayAlert(Translate.GetString("coding_WrongInputFormatTitle"), Translate.GetString("coding_WrongInputFormatText") + "\n" + 123.45.ToString(), "OK").GetAwaiter();
							if (!taskAwaiter4.IsCompleted)
							{
								num2 = 7;
								TaskAwaiter taskAwaiter5 = taskAwaiter4;
								this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, CodingWithInputDetailsPage.<btnSaveState_Clicked>d__6>(ref taskAwaiter4, ref this);
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
					taskAwaiter4 = codingWithInputDetailsPage.Navigation.PushAsync(inAppPage).GetAwaiter();
					if (!taskAwaiter4.IsCompleted)
					{
						num2 = 1;
						TaskAwaiter taskAwaiter5 = taskAwaiter4;
						this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, CodingWithInputDetailsPage.<btnSaveState_Clicked>d__6>(ref taskAwaiter4, ref this);
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
					taskAwaiter4 = codingWithInputDetailsPage.Navigation.PushAsync(inAppPage2).GetAwaiter();
					if (!taskAwaiter4.IsCompleted)
					{
						num2 = 3;
						TaskAwaiter taskAwaiter5 = taskAwaiter4;
						this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, CodingWithInputDetailsPage.<btnSaveState_Clicked>d__6>(ref taskAwaiter4, ref this);
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
					taskAwaiter4 = codingWithInputDetailsPage.DisplayAlert(Translate.GetString("coding_Error"), MQBAdaptationTemplate.CodingRequestResultToString(result), "OK").GetAwaiter();
					if (!taskAwaiter4.IsCompleted)
					{
						num2 = 5;
						TaskAwaiter taskAwaiter5 = taskAwaiter4;
						this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, CodingWithInputDetailsPage.<btnSaveState_Clicked>d__6>(ref taskAwaiter4, ref this);
						return;
					}
					IL_03CA:
					taskAwaiter4.GetResult();
					IL_03D1:
					taskAwaiter6 = codingWithInputDetailsPage.coding.UpdateCurrentState("", null).GetAwaiter();
					if (!taskAwaiter6.IsCompleted)
					{
						num2 = 6;
						TaskAwaiter<CodingRequestResult> taskAwaiter7 = taskAwaiter6;
						this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<CodingRequestResult>, CodingWithInputDetailsPage.<btnSaveState_Clicked>d__6>(ref taskAwaiter6, ref this);
						return;
					}
					IL_0433:
					taskAwaiter6.GetResult();
					codingWithInputDetailsPage.activityFrame.IsVisible = false;
					codingWithInputDetailsPage.activityFrame.Text = ActivityFrame.PLEASE_WAIT_TEXT;
					codingWithInputDetailsPage.entryValue.IsEnabled = true;
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

			// Token: 0x06004D7F RID: 19839 RVA: 0x0039CCD4 File Offset: 0x0039AED4
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x04002E08 RID: 11784
			public int <>1__state;

			// Token: 0x04002E09 RID: 11785
			public AsyncVoidMethodBuilder <>t__builder;

			// Token: 0x04002E0A RID: 11786
			public CodingWithInputDetailsPage <>4__this;

			// Token: 0x04002E0B RID: 11787
			private TaskAwaiter<bool> <>u__1;

			// Token: 0x04002E0C RID: 11788
			private TaskAwaiter <>u__2;

			// Token: 0x04002E0D RID: 11789
			private TaskAwaiter<CodingRequestResult> <>u__3;
		}
	}
}
