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
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Internals;
using Xamarin.Forms.PlatformConfiguration.iOSSpecific;
using Xamarin.Forms.Xaml;
using Xamarin.Forms.Xaml.Diagnostics;
using Xamarin.Forms.Xaml.Internals;

namespace CarScannerXamarinForms.Coding
{
	// Token: 0x0200090A RID: 2314
	[XamlCompilation(2)]
	[XamlFilePath("Coding\\Pages\\CodingWithOptionsDetailsPage.xaml")]
	public class CodingWithOptionsDetailsPage : ContentPage
	{
		// Token: 0x06004D80 RID: 19840 RVA: 0x0039CCE4 File Offset: 0x0039AEE4
		public CodingWithOptionsDetailsPage(ICodingContainer coding)
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

		// Token: 0x06004D81 RID: 19841 RVA: 0x0039CD40 File Offset: 0x0039AF40
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
				await this.coding.UpdateCurrentState("", progress);
				this.activityFrame.IsVisible = false;
				this.lv.IsEnabled = true;
			}
		}

		// Token: 0x06004D82 RID: 19842 RVA: 0x0039CD78 File Offset: 0x0039AF78
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
			await this.coding.UpdateCurrentState("", progress);
			this.activityFrame.IsVisible = false;
			this.lv.IsEnabled = true;
			this.activityFrame.Text = ActivityFrame.PLEASE_WAIT_TEXT;
		}

		// Token: 0x06004D83 RID: 19843 RVA: 0x0039CDBC File Offset: 0x0039AFBC
		private async void BtnUpdateState_Clicked(object sender, EventArgs e)
		{
			this.UpdateState();
		}

		// Token: 0x06004D84 RID: 19844 RVA: 0x0039CDF4 File Offset: 0x0039AFF4
		private async void Lv_ItemSelected(object sender, SelectedItemChangedEventArgs e)
		{
			if (e.SelectedItem != null)
			{
				MQBAdaptationOption mqbadaptationOption = (MQBAdaptationOption)e.SelectedItem;
				this.lv.SelectedItem = null;
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
					progress = null;
				}
			}
		}

		// Token: 0x06004D85 RID: 19845 RVA: 0x0039CE34 File Offset: 0x0039B034
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private void InitializeComponent()
		{
			if (ResourceLoader.IsEnabled && ResourceLoader.CanProvideContentFor(new ResourceLoader.ResourceLoadingQuery
			{
				AssemblyName = typeof(CodingWithOptionsDetailsPage).GetTypeInfo().Assembly.GetName(),
				ResourcePath = "Coding/Pages/CodingWithOptionsDetailsPage.xaml",
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
			VisualDiagnostics.RegisterSourceInfo(translate = new Translate(), new Uri("Coding\\Pages\\CodingWithOptionsDetailsPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 10, 5);
			DynamicResourceExtension dynamicResourceExtension;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension = new DynamicResourceExtension(), new Uri("Coding\\Pages\\CodingWithOptionsDetailsPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 13, 5);
			EmptyStringToFalseConverter emptyStringToFalseConverter;
			VisualDiagnostics.RegisterSourceInfo(emptyStringToFalseConverter = new EmptyStringToFalseConverter(), new Uri("Coding\\Pages\\CodingWithOptionsDetailsPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 16, 14);
			EmptyStringToTrueConverter emptyStringToTrueConverter;
			VisualDiagnostics.RegisterSourceInfo(emptyStringToTrueConverter = new EmptyStringToTrueConverter(), new Uri("Coding\\Pages\\CodingWithOptionsDetailsPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 17, 14);
			VagCodingPlatformToTrueConverter vagCodingPlatformToTrueConverter;
			VisualDiagnostics.RegisterSourceInfo(vagCodingPlatformToTrueConverter = new VagCodingPlatformToTrueConverter(), new Uri("Coding\\Pages\\CodingWithOptionsDetailsPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 18, 14);
			ResourceDictionary resourceDictionary;
			VisualDiagnostics.RegisterSourceInfo(resourceDictionary = new ResourceDictionary(), new Uri("Coding\\Pages\\CodingWithOptionsDetailsPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 15, 10);
			RowDefinition rowDefinition;
			VisualDiagnostics.RegisterSourceInfo(rowDefinition = new RowDefinition(), new Uri("Coding\\Pages\\CodingWithOptionsDetailsPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 24, 18);
			BindingExtension bindingExtension;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension = new BindingExtension(), new Uri("Coding\\Pages\\CodingWithOptionsDetailsPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 31, 17);
			DynamicResourceExtension dynamicResourceExtension2;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension2 = new DynamicResourceExtension(), new Uri("Coding\\Pages\\CodingWithOptionsDetailsPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 36, 29);
			BindingExtension bindingExtension2;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension2 = new BindingExtension(), new Uri("Coding\\Pages\\CodingWithOptionsDetailsPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 37, 29);
			Label label;
			VisualDiagnostics.RegisterSourceInfo(label = new Label(), new Uri("Coding\\Pages\\CodingWithOptionsDetailsPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 34, 26);
			StaticResourceExtension staticResourceExtension;
			VisualDiagnostics.RegisterSourceInfo(staticResourceExtension = new StaticResourceExtension(), new Uri("Coding\\Pages\\CodingWithOptionsDetailsPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 38, 32);
			BindingExtension bindingExtension3;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension3 = new BindingExtension(), new Uri("Coding\\Pages\\CodingWithOptionsDetailsPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 38, 32);
			BindingExtension bindingExtension4;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension4 = new BindingExtension(), new Uri("Coding\\Pages\\CodingWithOptionsDetailsPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 38, 122);
			Label label2;
			VisualDiagnostics.RegisterSourceInfo(label2 = new Label(), new Uri("Coding\\Pages\\CodingWithOptionsDetailsPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 38, 26);
			StaticResourceExtension staticResourceExtension2;
			VisualDiagnostics.RegisterSourceInfo(staticResourceExtension2 = new StaticResourceExtension(), new Uri("Coding\\Pages\\CodingWithOptionsDetailsPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 39, 32);
			BindingExtension bindingExtension5;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension5 = new BindingExtension(), new Uri("Coding\\Pages\\CodingWithOptionsDetailsPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 39, 32);
			BindingExtension bindingExtension6;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension6 = new BindingExtension(), new Uri("Coding\\Pages\\CodingWithOptionsDetailsPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 39, 127);
			Label label3;
			VisualDiagnostics.RegisterSourceInfo(label3 = new Label(), new Uri("Coding\\Pages\\CodingWithOptionsDetailsPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 39, 26);
			BindingExtension bindingExtension7;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension7 = new BindingExtension(), new Uri("Coding\\Pages\\CodingWithOptionsDetailsPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 40, 38);
			Translate translate2;
			VisualDiagnostics.RegisterSourceInfo(translate2 = new Translate(), new Uri("Coding\\Pages\\CodingWithOptionsDetailsPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 41, 36);
			Label label4;
			VisualDiagnostics.RegisterSourceInfo(label4 = new Label(), new Uri("Coding\\Pages\\CodingWithOptionsDetailsPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 41, 30);
			BindingExtension bindingExtension8;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension8 = new BindingExtension(), new Uri("Coding\\Pages\\CodingWithOptionsDetailsPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 42, 59);
			Entry entry;
			VisualDiagnostics.RegisterSourceInfo(entry = new Entry(), new Uri("Coding\\Pages\\CodingWithOptionsDetailsPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 42, 30);
			StaticResourceExtension staticResourceExtension3;
			VisualDiagnostics.RegisterSourceInfo(staticResourceExtension3 = new StaticResourceExtension(), new Uri("Coding\\Pages\\CodingWithOptionsDetailsPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 43, 60);
			BindingExtension bindingExtension9;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension9 = new BindingExtension(), new Uri("Coding\\Pages\\CodingWithOptionsDetailsPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 43, 60);
			Translate translate3;
			VisualDiagnostics.RegisterSourceInfo(translate3 = new Translate(), new Uri("Coding\\Pages\\CodingWithOptionsDetailsPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 47, 47);
			Span span;
			VisualDiagnostics.RegisterSourceInfo(span = new Span(), new Uri("Coding\\Pages\\CodingWithOptionsDetailsPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 47, 42);
			BindingExtension bindingExtension10;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension10 = new BindingExtension(), new Uri("Coding\\Pages\\CodingWithOptionsDetailsPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 48, 47);
			Span span2;
			VisualDiagnostics.RegisterSourceInfo(span2 = new Span(), new Uri("Coding\\Pages\\CodingWithOptionsDetailsPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 48, 42);
			FormattedString formattedString;
			VisualDiagnostics.RegisterSourceInfo(formattedString = new FormattedString(), new Uri("Coding\\Pages\\CodingWithOptionsDetailsPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 46, 38);
			Label label5;
			VisualDiagnostics.RegisterSourceInfo(label5 = new Label(), new Uri("Coding\\Pages\\CodingWithOptionsDetailsPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 43, 30);
			StaticResourceExtension staticResourceExtension4;
			VisualDiagnostics.RegisterSourceInfo(staticResourceExtension4 = new StaticResourceExtension(), new Uri("Coding\\Pages\\CodingWithOptionsDetailsPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 54, 33);
			BindingExtension bindingExtension11;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension11 = new BindingExtension(), new Uri("Coding\\Pages\\CodingWithOptionsDetailsPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 54, 33);
			Translate translate4;
			VisualDiagnostics.RegisterSourceInfo(translate4 = new Translate(), new Uri("Coding\\Pages\\CodingWithOptionsDetailsPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 55, 33);
			Label label6;
			VisualDiagnostics.RegisterSourceInfo(label6 = new Label(), new Uri("Coding\\Pages\\CodingWithOptionsDetailsPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 52, 30);
			StackLayout stackLayout;
			VisualDiagnostics.RegisterSourceInfo(stackLayout = new StackLayout(), new Uri("Coding\\Pages\\CodingWithOptionsDetailsPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 40, 26);
			BindingExtension bindingExtension12;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension12 = new BindingExtension(), new Uri("Coding\\Pages\\CodingWithOptionsDetailsPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 58, 52);
			Translate translate5;
			VisualDiagnostics.RegisterSourceInfo(translate5 = new Translate(), new Uri("Coding\\Pages\\CodingWithOptionsDetailsPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 61, 43);
			Span span3;
			VisualDiagnostics.RegisterSourceInfo(span3 = new Span(), new Uri("Coding\\Pages\\CodingWithOptionsDetailsPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 61, 38);
			Span span4;
			VisualDiagnostics.RegisterSourceInfo(span4 = new Span(), new Uri("Coding\\Pages\\CodingWithOptionsDetailsPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 62, 38);
			BindingExtension bindingExtension13;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension13 = new BindingExtension(), new Uri("Coding\\Pages\\CodingWithOptionsDetailsPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 63, 65);
			Span span5;
			VisualDiagnostics.RegisterSourceInfo(span5 = new Span(), new Uri("Coding\\Pages\\CodingWithOptionsDetailsPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 63, 38);
			FormattedString formattedString2;
			VisualDiagnostics.RegisterSourceInfo(formattedString2 = new FormattedString(), new Uri("Coding\\Pages\\CodingWithOptionsDetailsPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 60, 34);
			Label label7;
			VisualDiagnostics.RegisterSourceInfo(label7 = new Label(), new Uri("Coding\\Pages\\CodingWithOptionsDetailsPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 58, 26);
			BindingExtension bindingExtension14;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension14 = new BindingExtension(), new Uri("Coding\\Pages\\CodingWithOptionsDetailsPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 70, 29);
			Translate translate6;
			VisualDiagnostics.RegisterSourceInfo(translate6 = new Translate(), new Uri("Coding\\Pages\\CodingWithOptionsDetailsPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 71, 29);
			Button button;
			VisualDiagnostics.RegisterSourceInfo(button = new Button(), new Uri("Coding\\Pages\\CodingWithOptionsDetailsPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 67, 26);
			Translate translate7;
			VisualDiagnostics.RegisterSourceInfo(translate7 = new Translate(), new Uri("Coding\\Pages\\CodingWithOptionsDetailsPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 73, 32);
			Label label8;
			VisualDiagnostics.RegisterSourceInfo(label8 = new Label(), new Uri("Coding\\Pages\\CodingWithOptionsDetailsPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 73, 26);
			StackLayout stackLayout2;
			VisualDiagnostics.RegisterSourceInfo(stackLayout2 = new StackLayout(), new Uri("Coding\\Pages\\CodingWithOptionsDetailsPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 33, 22);
			DataTemplate dataTemplate;
			VisualDiagnostics.RegisterSourceInfo(dataTemplate = new DataTemplate(), new Uri("Coding\\Pages\\CodingWithOptionsDetailsPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 77, 22);
			SharedSettings sharedSettings;
			VisualDiagnostics.RegisterSourceInfo(sharedSettings = SharedSettings.Current, new Uri("Coding\\Pages\\CodingWithOptionsDetailsPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 90, 29);
			DynamicResourceExtension dynamicResourceExtension3;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension3 = new DynamicResourceExtension(), new Uri("Coding\\Pages\\CodingWithOptionsDetailsPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 92, 29);
			StaticResourceExtension staticResourceExtension5;
			VisualDiagnostics.RegisterSourceInfo(staticResourceExtension5 = new StaticResourceExtension(), new Uri("Coding\\Pages\\CodingWithOptionsDetailsPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 93, 29);
			BindingExtension bindingExtension15;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension15 = new BindingExtension(), new Uri("Coding\\Pages\\CodingWithOptionsDetailsPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 93, 29);
			Translate translate8;
			VisualDiagnostics.RegisterSourceInfo(translate8 = new Translate(), new Uri("Coding\\Pages\\CodingWithOptionsDetailsPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 94, 29);
			Label label9;
			VisualDiagnostics.RegisterSourceInfo(label9 = new Label(), new Uri("Coding\\Pages\\CodingWithOptionsDetailsPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 89, 26);
			DynamicResourceExtension dynamicResourceExtension4;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension4 = new DynamicResourceExtension(), new Uri("Coding\\Pages\\CodingWithOptionsDetailsPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 97, 29);
			Translate translate9;
			VisualDiagnostics.RegisterSourceInfo(translate9 = new Translate(), new Uri("Coding\\Pages\\CodingWithOptionsDetailsPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 98, 29);
			Label label10;
			VisualDiagnostics.RegisterSourceInfo(label10 = new Label(), new Uri("Coding\\Pages\\CodingWithOptionsDetailsPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 95, 26);
			SharedSettings sharedSettings2;
			VisualDiagnostics.RegisterSourceInfo(sharedSettings2 = SharedSettings.Current, new Uri("Coding\\Pages\\CodingWithOptionsDetailsPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 100, 29);
			BindingExtension bindingExtension16;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension16 = new BindingExtension(), new Uri("Coding\\Pages\\CodingWithOptionsDetailsPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 101, 29);
			BindingExtension bindingExtension17;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension17 = new BindingExtension(), new Uri("Coding\\Pages\\CodingWithOptionsDetailsPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 102, 29);
			Translate translate10;
			VisualDiagnostics.RegisterSourceInfo(translate10 = new Translate(), new Uri("Coding\\Pages\\CodingWithOptionsDetailsPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 103, 29);
			LabelSwitch labelSwitch;
			VisualDiagnostics.RegisterSourceInfo(labelSwitch = new LabelSwitch(), new Uri("Coding\\Pages\\CodingWithOptionsDetailsPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 99, 26);
			StackLayout stackLayout3;
			VisualDiagnostics.RegisterSourceInfo(stackLayout3 = new StackLayout(), new Uri("Coding\\Pages\\CodingWithOptionsDetailsPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 88, 22);
			ListView listView;
			VisualDiagnostics.RegisterSourceInfo(listView = new ListView(), new Uri("Coding\\Pages\\CodingWithOptionsDetailsPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 26, 14);
			ActivityFrame activityFrame;
			VisualDiagnostics.RegisterSourceInfo(activityFrame = new ActivityFrame(), new Uri("Coding\\Pages\\CodingWithOptionsDetailsPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 109, 14);
			Grid grid;
			VisualDiagnostics.RegisterSourceInfo(grid = new Grid(), new Uri("Coding\\Pages\\CodingWithOptionsDetailsPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 22, 10);
			VisualDiagnostics.RegisterSourceInfo(this, new Uri("Coding\\Pages\\CodingWithOptionsDetailsPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 2, 2);
			NameScope nameScope = NameScope.GetNameScope(this) ?? new NameScope();
			NameScope.SetNameScope(this, nameScope);
			nameScope.RegisterName("lv", listView);
			if (listView.StyleId == null)
			{
				listView.StyleId = "lv";
			}
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
			nameScope.RegisterName("activityFrame", activityFrame);
			if (activityFrame.StyleId == null)
			{
				activityFrame.StyleId = "activityFrame";
			}
			this.lv = listView;
			this.entryPassword = entry;
			this.labelState = label7;
			this.btnUpdateState = button;
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
			xmlNamespaceResolver.Add("settings", "clr-namespace:CarScannerXamarinForms.Settings");
			xamlServiceProvider.Add(typeFromHandle2, new XamlTypeResolver(xmlNamespaceResolver, typeof(CodingWithOptionsDetailsPage).GetTypeInfo().Assembly));
			xamlServiceProvider.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(10, 5)));
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
			xmlNamespaceResolver2.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver2.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver2.Add("settings", "clr-namespace:CarScannerXamarinForms.Settings");
			xamlServiceProvider2.Add(typeFromHandle4, new XamlTypeResolver(xmlNamespaceResolver2, typeof(CodingWithOptionsDetailsPage).GetTypeInfo().Assembly));
			xamlServiceProvider2.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(13, 5)));
			DynamicResource dynamicResource = markupExtension2.ProvideValue(xamlServiceProvider2);
			this.SetDynamicResource(VisualElement.BackgroundColorProperty, dynamicResource.Key);
			this.Resources = resourceDictionary;
			rowDefinition.SetValue(RowDefinition.HeightProperty, new GridLengthTypeConverter().ConvertFromInvariantString("*"));
			grid.GetValue(Grid.RowDefinitionsProperty).Add(rowDefinition);
			listView.SetValue(Grid.RowProperty, 0);
			listView.SetValue(ListView.HasUnevenRowsProperty, true);
			listView.ItemSelected += this.Lv_ItemSelected;
			bindingExtension.Path = "Options";
			BindingBase bindingBase = bindingExtension.ProvideValue(null);
			listView.SetBinding(ItemsView<Cell>.ItemsSourceProperty, bindingBase);
			stackLayout2.SetValue(StackLayout.OrientationProperty, 0);
			label.SetValue(Label.FontAttributesProperty, new FontAttributesConverter().ConvertFromInvariantString("Bold"));
			dynamicResourceExtension2.Key = "BaseFontSize+++";
			IMarkupExtension<DynamicResource> markupExtension3 = dynamicResourceExtension2;
			XamlServiceProvider xamlServiceProvider3 = new XamlServiceProvider();
			Type typeFromHandle5 = typeof(IProvideValueTarget);
			object[] array3 = new object[0 + 5];
			array3[0] = label;
			array3[1] = stackLayout2;
			array3[2] = listView;
			array3[3] = grid;
			array3[4] = this;
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
			xmlNamespaceResolver3.Add("settings", "clr-namespace:CarScannerXamarinForms.Settings");
			xamlServiceProvider3.Add(typeFromHandle6, new XamlTypeResolver(xmlNamespaceResolver3, typeof(CodingWithOptionsDetailsPage).GetTypeInfo().Assembly));
			xamlServiceProvider3.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(36, 29)));
			DynamicResource dynamicResource2 = markupExtension3.ProvideValue(xamlServiceProvider3);
			label.SetDynamicResource(Label.FontSizeProperty, dynamicResource2.Key);
			bindingExtension2.Path = "Name";
			BindingBase bindingBase2 = bindingExtension2.ProvideValue(null);
			label.SetBinding(Label.TextProperty, bindingBase2);
			stackLayout2.Children.Add(label);
			staticResourceExtension.Key = "EmptyStringToFalseConverter";
			IMarkupExtension markupExtension4 = staticResourceExtension;
			XamlServiceProvider xamlServiceProvider4 = new XamlServiceProvider();
			Type typeFromHandle7 = typeof(IProvideValueTarget);
			object[] array4 = new object[0 + 6];
			array4[0] = bindingExtension3;
			array4[1] = label2;
			array4[2] = stackLayout2;
			array4[3] = listView;
			array4[4] = grid;
			array4[5] = this;
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
			xmlNamespaceResolver4.Add("settings", "clr-namespace:CarScannerXamarinForms.Settings");
			xamlServiceProvider4.Add(typeFromHandle8, new XamlTypeResolver(xmlNamespaceResolver4, typeof(CodingWithOptionsDetailsPage).GetTypeInfo().Assembly));
			xamlServiceProvider4.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(38, 32)));
			object obj6 = markupExtension4.ProvideValue(xamlServiceProvider4);
			bindingExtension3.Converter = obj6;
			bindingExtension3.Path = "Description";
			BindingBase bindingBase3 = bindingExtension3.ProvideValue(null);
			label2.SetBinding(VisualElement.IsVisibleProperty, bindingBase3);
			bindingExtension4.Path = "Description";
			BindingBase bindingBase4 = bindingExtension4.ProvideValue(null);
			label2.SetBinding(Label.TextProperty, bindingBase4);
			stackLayout2.Children.Add(label2);
			staticResourceExtension2.Key = "EmptyStringToFalseConverter";
			IMarkupExtension markupExtension5 = staticResourceExtension2;
			XamlServiceProvider xamlServiceProvider5 = new XamlServiceProvider();
			Type typeFromHandle9 = typeof(IProvideValueTarget);
			object[] array5 = new object[0 + 6];
			array5[0] = bindingExtension5;
			array5[1] = label3;
			array5[2] = stackLayout2;
			array5[3] = listView;
			array5[4] = grid;
			array5[5] = this;
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
			xmlNamespaceResolver5.Add("settings", "clr-namespace:CarScannerXamarinForms.Settings");
			xamlServiceProvider5.Add(typeFromHandle10, new XamlTypeResolver(xmlNamespaceResolver5, typeof(CodingWithOptionsDetailsPage).GetTypeInfo().Assembly));
			xamlServiceProvider5.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(39, 32)));
			object obj8 = markupExtension5.ProvideValue(xamlServiceProvider5);
			bindingExtension5.Converter = obj8;
			bindingExtension5.Path = "InnerDescription";
			BindingBase bindingBase5 = bindingExtension5.ProvideValue(null);
			label3.SetBinding(VisualElement.IsVisibleProperty, bindingBase5);
			bindingExtension6.Path = "InnerDescription";
			BindingBase bindingBase6 = bindingExtension6.ProvideValue(null);
			label3.SetBinding(Label.TextProperty, bindingBase6);
			stackLayout2.Children.Add(label3);
			bindingExtension7.Path = "PasswordVisible";
			BindingBase bindingBase7 = bindingExtension7.ProvideValue(null);
			stackLayout.SetBinding(VisualElement.IsVisibleProperty, bindingBase7);
			stackLayout.SetValue(StackLayout.OrientationProperty, 0);
			translate2.Text = "coding_Password";
			IMarkupExtension markupExtension6 = translate2;
			XamlServiceProvider xamlServiceProvider6 = new XamlServiceProvider();
			Type typeFromHandle11 = typeof(IProvideValueTarget);
			object[] array6 = new object[0 + 6];
			array6[0] = label4;
			array6[1] = stackLayout;
			array6[2] = stackLayout2;
			array6[3] = listView;
			array6[4] = grid;
			array6[5] = this;
			object obj9;
			xamlServiceProvider6.Add(typeFromHandle11, obj9 = new SimpleValueTargetProvider(array6, Label.TextProperty, nameScope));
			xamlServiceProvider6.Add(typeof(IReferenceProvider), obj9);
			Type typeFromHandle12 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver6 = new XmlNamespaceResolver();
			xmlNamespaceResolver6.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver6.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver6.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver6.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver6.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver6.Add("settings", "clr-namespace:CarScannerXamarinForms.Settings");
			xamlServiceProvider6.Add(typeFromHandle12, new XamlTypeResolver(xmlNamespaceResolver6, typeof(CodingWithOptionsDetailsPage).GetTypeInfo().Assembly));
			xamlServiceProvider6.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(41, 36)));
			object obj10 = markupExtension6.ProvideValue(xamlServiceProvider6);
			label4.Text = obj10;
			stackLayout.Children.Add(label4);
			bindingExtension8.Mode = 1;
			bindingExtension8.Path = "Password";
			BindingBase bindingBase8 = bindingExtension8.ProvideValue(null);
			entry.SetBinding(Entry.TextProperty, bindingBase8);
			stackLayout.Children.Add(entry);
			label5.SetValue(Label.FontAttributesProperty, new FontAttributesConverter().ConvertFromInvariantString("Italic"));
			staticResourceExtension3.Key = "EmptyStringToFalseConverter";
			IMarkupExtension markupExtension7 = staticResourceExtension3;
			XamlServiceProvider xamlServiceProvider7 = new XamlServiceProvider();
			Type typeFromHandle13 = typeof(IProvideValueTarget);
			object[] array7 = new object[0 + 7];
			array7[0] = bindingExtension9;
			array7[1] = label5;
			array7[2] = stackLayout;
			array7[3] = stackLayout2;
			array7[4] = listView;
			array7[5] = grid;
			array7[6] = this;
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
			xmlNamespaceResolver7.Add("settings", "clr-namespace:CarScannerXamarinForms.Settings");
			xamlServiceProvider7.Add(typeFromHandle14, new XamlTypeResolver(xmlNamespaceResolver7, typeof(CodingWithOptionsDetailsPage).GetTypeInfo().Assembly));
			xamlServiceProvider7.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(43, 60)));
			object obj12 = markupExtension7.ProvideValue(xamlServiceProvider7);
			bindingExtension9.Converter = obj12;
			bindingExtension9.Path = "PasswordHint";
			BindingBase bindingBase9 = bindingExtension9.ProvideValue(null);
			label5.SetBinding(VisualElement.IsVisibleProperty, bindingBase9);
			translate3.Text = "coding_PasswordHint";
			IMarkupExtension markupExtension8 = translate3;
			XamlServiceProvider xamlServiceProvider8 = new XamlServiceProvider();
			Type typeFromHandle15 = typeof(IProvideValueTarget);
			object[] array8 = new object[0 + 8];
			array8[0] = span;
			array8[1] = formattedString;
			array8[2] = label5;
			array8[3] = stackLayout;
			array8[4] = stackLayout2;
			array8[5] = listView;
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
			xmlNamespaceResolver8.Add("settings", "clr-namespace:CarScannerXamarinForms.Settings");
			xamlServiceProvider8.Add(typeFromHandle16, new XamlTypeResolver(xmlNamespaceResolver8, typeof(CodingWithOptionsDetailsPage).GetTypeInfo().Assembly));
			xamlServiceProvider8.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(47, 47)));
			object obj14 = markupExtension8.ProvideValue(xamlServiceProvider8);
			span.Text = obj14;
			formattedString.Spans.Add(span);
			bindingExtension10.Path = "PasswordHint";
			BindingBase bindingBase10 = bindingExtension10.ProvideValue(null);
			span2.SetBinding(Span.TextProperty, bindingBase10);
			formattedString.Spans.Add(span2);
			label5.SetValue(Label.FormattedTextProperty, formattedString);
			stackLayout.Children.Add(label5);
			label6.SetValue(Label.FontAttributesProperty, new FontAttributesConverter().ConvertFromInvariantString("Italic"));
			bindingExtension11.Mode = 2;
			staticResourceExtension4.Key = "EmptyStringToTrueConverter";
			IMarkupExtension markupExtension9 = staticResourceExtension4;
			XamlServiceProvider xamlServiceProvider9 = new XamlServiceProvider();
			Type typeFromHandle17 = typeof(IProvideValueTarget);
			object[] array9 = new object[0 + 7];
			array9[0] = bindingExtension11;
			array9[1] = label6;
			array9[2] = stackLayout;
			array9[3] = stackLayout2;
			array9[4] = listView;
			array9[5] = grid;
			array9[6] = this;
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
			xmlNamespaceResolver9.Add("settings", "clr-namespace:CarScannerXamarinForms.Settings");
			xamlServiceProvider9.Add(typeFromHandle18, new XamlTypeResolver(xmlNamespaceResolver9, typeof(CodingWithOptionsDetailsPage).GetTypeInfo().Assembly));
			xamlServiceProvider9.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(54, 33)));
			object obj16 = markupExtension9.ProvideValue(xamlServiceProvider9);
			bindingExtension11.Converter = obj16;
			bindingExtension11.Path = "Password";
			BindingBase bindingBase11 = bindingExtension11.ProvideValue(null);
			label6.SetBinding(VisualElement.IsVisibleProperty, bindingBase11);
			translate4.Text = "coding_PasswordNotRequired";
			IMarkupExtension markupExtension10 = translate4;
			XamlServiceProvider xamlServiceProvider10 = new XamlServiceProvider();
			Type typeFromHandle19 = typeof(IProvideValueTarget);
			object[] array10 = new object[0 + 6];
			array10[0] = label6;
			array10[1] = stackLayout;
			array10[2] = stackLayout2;
			array10[3] = listView;
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
			xmlNamespaceResolver10.Add("settings", "clr-namespace:CarScannerXamarinForms.Settings");
			xamlServiceProvider10.Add(typeFromHandle20, new XamlTypeResolver(xmlNamespaceResolver10, typeof(CodingWithOptionsDetailsPage).GetTypeInfo().Assembly));
			xamlServiceProvider10.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(55, 33)));
			object obj18 = markupExtension10.ProvideValue(xamlServiceProvider10);
			label6.Text = obj18;
			stackLayout.Children.Add(label6);
			stackLayout2.Children.Add(stackLayout);
			bindingExtension12.Mode = 2;
			bindingExtension12.Path = "HasCurrentState";
			BindingBase bindingBase12 = bindingExtension12.ProvideValue(null);
			label7.SetBinding(VisualElement.IsVisibleProperty, bindingBase12);
			translate5.Text = "coding_CurrentState";
			IMarkupExtension markupExtension11 = translate5;
			XamlServiceProvider xamlServiceProvider11 = new XamlServiceProvider();
			Type typeFromHandle21 = typeof(IProvideValueTarget);
			object[] array11 = new object[0 + 7];
			array11[0] = span3;
			array11[1] = formattedString2;
			array11[2] = label7;
			array11[3] = stackLayout2;
			array11[4] = listView;
			array11[5] = grid;
			array11[6] = this;
			object obj19;
			xamlServiceProvider11.Add(typeFromHandle21, obj19 = new SimpleValueTargetProvider(array11, Span.TextProperty, nameScope));
			xamlServiceProvider11.Add(typeof(IReferenceProvider), obj19);
			Type typeFromHandle22 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver11 = new XmlNamespaceResolver();
			xmlNamespaceResolver11.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver11.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver11.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver11.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver11.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver11.Add("settings", "clr-namespace:CarScannerXamarinForms.Settings");
			xamlServiceProvider11.Add(typeFromHandle22, new XamlTypeResolver(xmlNamespaceResolver11, typeof(CodingWithOptionsDetailsPage).GetTypeInfo().Assembly));
			xamlServiceProvider11.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(61, 43)));
			object obj20 = markupExtension11.ProvideValue(xamlServiceProvider11);
			span3.Text = obj20;
			formattedString2.Spans.Add(span3);
			span4.SetValue(Span.TextProperty, " ");
			formattedString2.Spans.Add(span4);
			span5.SetValue(Span.FontAttributesProperty, new FontAttributesConverter().ConvertFromInvariantString("Bold"));
			bindingExtension13.Mode = 2;
			bindingExtension13.Path = "CurrentState";
			BindingBase bindingBase13 = bindingExtension13.ProvideValue(null);
			span5.SetBinding(Span.TextProperty, bindingBase13);
			formattedString2.Spans.Add(span5);
			label7.SetValue(Label.FormattedTextProperty, formattedString2);
			stackLayout2.Children.Add(label7);
			button.Clicked += this.BtnUpdateState_Clicked;
			bindingExtension14.Path = "HasCurrentState";
			BindingBase bindingBase14 = bindingExtension14.ProvideValue(null);
			button.SetBinding(VisualElement.IsVisibleProperty, bindingBase14);
			translate6.Text = "Mode06Page_btnRefresh.Content";
			IMarkupExtension markupExtension12 = translate6;
			XamlServiceProvider xamlServiceProvider12 = new XamlServiceProvider();
			Type typeFromHandle23 = typeof(IProvideValueTarget);
			object[] array12 = new object[0 + 5];
			array12[0] = button;
			array12[1] = stackLayout2;
			array12[2] = listView;
			array12[3] = grid;
			array12[4] = this;
			object obj21;
			xamlServiceProvider12.Add(typeFromHandle23, obj21 = new SimpleValueTargetProvider(array12, Button.TextProperty, nameScope));
			xamlServiceProvider12.Add(typeof(IReferenceProvider), obj21);
			Type typeFromHandle24 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver12 = new XmlNamespaceResolver();
			xmlNamespaceResolver12.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver12.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver12.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver12.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver12.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver12.Add("settings", "clr-namespace:CarScannerXamarinForms.Settings");
			xamlServiceProvider12.Add(typeFromHandle24, new XamlTypeResolver(xmlNamespaceResolver12, typeof(CodingWithOptionsDetailsPage).GetTypeInfo().Assembly));
			xamlServiceProvider12.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(71, 29)));
			object obj22 = markupExtension12.ProvideValue(xamlServiceProvider12);
			button.Text = obj22;
			stackLayout2.Children.Add(button);
			translate7.Text = "coding_ChooseOption";
			IMarkupExtension markupExtension13 = translate7;
			XamlServiceProvider xamlServiceProvider13 = new XamlServiceProvider();
			Type typeFromHandle25 = typeof(IProvideValueTarget);
			object[] array13 = new object[0 + 5];
			array13[0] = label8;
			array13[1] = stackLayout2;
			array13[2] = listView;
			array13[3] = grid;
			array13[4] = this;
			object obj23;
			xamlServiceProvider13.Add(typeFromHandle25, obj23 = new SimpleValueTargetProvider(array13, Label.TextProperty, nameScope));
			xamlServiceProvider13.Add(typeof(IReferenceProvider), obj23);
			Type typeFromHandle26 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver13 = new XmlNamespaceResolver();
			xmlNamespaceResolver13.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver13.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver13.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver13.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver13.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver13.Add("settings", "clr-namespace:CarScannerXamarinForms.Settings");
			xamlServiceProvider13.Add(typeFromHandle26, new XamlTypeResolver(xmlNamespaceResolver13, typeof(CodingWithOptionsDetailsPage).GetTypeInfo().Assembly));
			xamlServiceProvider13.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(73, 32)));
			object obj24 = markupExtension13.ProvideValue(xamlServiceProvider13);
			label8.Text = obj24;
			stackLayout2.Children.Add(label8);
			listView.SetValue(ListView.HeaderProperty, stackLayout2);
			IDataTemplate dataTemplate2 = dataTemplate;
			CodingWithOptionsDetailsPage.<InitializeComponent>_anonXamlCDataTemplate_11 <InitializeComponent>_anonXamlCDataTemplate_ = new CodingWithOptionsDetailsPage.<InitializeComponent>_anonXamlCDataTemplate_11();
			object[] array14 = new object[0 + 4];
			array14[0] = dataTemplate;
			array14[1] = listView;
			array14[2] = grid;
			array14[3] = this;
			<InitializeComponent>_anonXamlCDataTemplate_.parentValues = array14;
			<InitializeComponent>_anonXamlCDataTemplate_.root = this;
			dataTemplate2.LoadTemplate = new Func<object>(<InitializeComponent>_anonXamlCDataTemplate_.LoadDataTemplate);
			listView.SetValue(ItemsView<Cell>.ItemTemplateProperty, dataTemplate);
			stackLayout3.SetValue(StackLayout.OrientationProperty, 0);
			label9.SetValue(BindableObject.BindingContextProperty, sharedSettings);
			label9.SetValue(Label.FontAttributesProperty, new FontAttributesConverter().ConvertFromInvariantString("Bold"));
			dynamicResourceExtension3.Key = "BaseFontSize-";
			IMarkupExtension<DynamicResource> markupExtension14 = dynamicResourceExtension3;
			XamlServiceProvider xamlServiceProvider14 = new XamlServiceProvider();
			Type typeFromHandle27 = typeof(IProvideValueTarget);
			object[] array15 = new object[0 + 5];
			array15[0] = label9;
			array15[1] = stackLayout3;
			array15[2] = listView;
			array15[3] = grid;
			array15[4] = this;
			object obj25;
			xamlServiceProvider14.Add(typeFromHandle27, obj25 = new SimpleValueTargetProvider(array15, Label.FontSizeProperty, nameScope));
			xamlServiceProvider14.Add(typeof(IReferenceProvider), obj25);
			Type typeFromHandle28 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver14 = new XmlNamespaceResolver();
			xmlNamespaceResolver14.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver14.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver14.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver14.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver14.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver14.Add("settings", "clr-namespace:CarScannerXamarinForms.Settings");
			xamlServiceProvider14.Add(typeFromHandle28, new XamlTypeResolver(xmlNamespaceResolver14, typeof(CodingWithOptionsDetailsPage).GetTypeInfo().Assembly));
			xamlServiceProvider14.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(92, 29)));
			DynamicResource dynamicResource3 = markupExtension14.ProvideValue(xamlServiceProvider14);
			label9.SetDynamicResource(Label.FontSizeProperty, dynamicResource3.Key);
			staticResourceExtension5.Key = "VagCodingPlatformToTrueConverter";
			IMarkupExtension markupExtension15 = staticResourceExtension5;
			XamlServiceProvider xamlServiceProvider15 = new XamlServiceProvider();
			Type typeFromHandle29 = typeof(IProvideValueTarget);
			object[] array16 = new object[0 + 6];
			array16[0] = bindingExtension15;
			array16[1] = label9;
			array16[2] = stackLayout3;
			array16[3] = listView;
			array16[4] = grid;
			array16[5] = this;
			object obj26;
			xamlServiceProvider15.Add(typeFromHandle29, obj26 = new SimpleValueTargetProvider(array16, typeof(BindingExtension).GetRuntimeProperty("Converter"), nameScope));
			xamlServiceProvider15.Add(typeof(IReferenceProvider), obj26);
			Type typeFromHandle30 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver15 = new XmlNamespaceResolver();
			xmlNamespaceResolver15.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver15.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver15.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver15.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver15.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver15.Add("settings", "clr-namespace:CarScannerXamarinForms.Settings");
			xamlServiceProvider15.Add(typeFromHandle30, new XamlTypeResolver(xmlNamespaceResolver15, typeof(CodingWithOptionsDetailsPage).GetTypeInfo().Assembly));
			xamlServiceProvider15.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(93, 29)));
			object obj27 = markupExtension15.ProvideValue(xamlServiceProvider15);
			bindingExtension15.Converter = obj27;
			bindingExtension15.Path = "CodingLastPlatformSelected";
			BindingBase bindingBase15 = bindingExtension15.ProvideValue(null);
			label9.SetBinding(VisualElement.IsVisibleProperty, bindingBase15);
			translate8.Text = "coding_VagOpenHoodWarning";
			IMarkupExtension markupExtension16 = translate8;
			XamlServiceProvider xamlServiceProvider16 = new XamlServiceProvider();
			Type typeFromHandle31 = typeof(IProvideValueTarget);
			object[] array17 = new object[0 + 5];
			array17[0] = label9;
			array17[1] = stackLayout3;
			array17[2] = listView;
			array17[3] = grid;
			array17[4] = this;
			object obj28;
			xamlServiceProvider16.Add(typeFromHandle31, obj28 = new SimpleValueTargetProvider(array17, Label.TextProperty, nameScope));
			xamlServiceProvider16.Add(typeof(IReferenceProvider), obj28);
			Type typeFromHandle32 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver16 = new XmlNamespaceResolver();
			xmlNamespaceResolver16.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver16.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver16.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver16.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver16.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver16.Add("settings", "clr-namespace:CarScannerXamarinForms.Settings");
			xamlServiceProvider16.Add(typeFromHandle32, new XamlTypeResolver(xmlNamespaceResolver16, typeof(CodingWithOptionsDetailsPage).GetTypeInfo().Assembly));
			xamlServiceProvider16.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(94, 29)));
			object obj29 = markupExtension16.ProvideValue(xamlServiceProvider16);
			label9.Text = obj29;
			stackLayout3.Children.Add(label9);
			label10.SetValue(Label.FontAttributesProperty, new FontAttributesConverter().ConvertFromInvariantString("Italic"));
			dynamicResourceExtension4.Key = "BaseFontSize-";
			IMarkupExtension<DynamicResource> markupExtension17 = dynamicResourceExtension4;
			XamlServiceProvider xamlServiceProvider17 = new XamlServiceProvider();
			Type typeFromHandle33 = typeof(IProvideValueTarget);
			object[] array18 = new object[0 + 5];
			array18[0] = label10;
			array18[1] = stackLayout3;
			array18[2] = listView;
			array18[3] = grid;
			array18[4] = this;
			object obj30;
			xamlServiceProvider17.Add(typeFromHandle33, obj30 = new SimpleValueTargetProvider(array18, Label.FontSizeProperty, nameScope));
			xamlServiceProvider17.Add(typeof(IReferenceProvider), obj30);
			Type typeFromHandle34 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver17 = new XmlNamespaceResolver();
			xmlNamespaceResolver17.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver17.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver17.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver17.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver17.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver17.Add("settings", "clr-namespace:CarScannerXamarinForms.Settings");
			xamlServiceProvider17.Add(typeFromHandle34, new XamlTypeResolver(xmlNamespaceResolver17, typeof(CodingWithOptionsDetailsPage).GetTypeInfo().Assembly));
			xamlServiceProvider17.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(97, 29)));
			DynamicResource dynamicResource4 = markupExtension17.ProvideValue(xamlServiceProvider17);
			label10.SetDynamicResource(Label.FontSizeProperty, dynamicResource4.Key);
			translate9.Text = "coding_UseHistory";
			IMarkupExtension markupExtension18 = translate9;
			XamlServiceProvider xamlServiceProvider18 = new XamlServiceProvider();
			Type typeFromHandle35 = typeof(IProvideValueTarget);
			object[] array19 = new object[0 + 5];
			array19[0] = label10;
			array19[1] = stackLayout3;
			array19[2] = listView;
			array19[3] = grid;
			array19[4] = this;
			object obj31;
			xamlServiceProvider18.Add(typeFromHandle35, obj31 = new SimpleValueTargetProvider(array19, Label.TextProperty, nameScope));
			xamlServiceProvider18.Add(typeof(IReferenceProvider), obj31);
			Type typeFromHandle36 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver18 = new XmlNamespaceResolver();
			xmlNamespaceResolver18.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver18.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver18.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver18.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver18.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver18.Add("settings", "clr-namespace:CarScannerXamarinForms.Settings");
			xamlServiceProvider18.Add(typeFromHandle36, new XamlTypeResolver(xmlNamespaceResolver18, typeof(CodingWithOptionsDetailsPage).GetTypeInfo().Assembly));
			xamlServiceProvider18.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(98, 29)));
			object obj32 = markupExtension18.ProvideValue(xamlServiceProvider18);
			label10.Text = obj32;
			stackLayout3.Children.Add(label10);
			labelSwitch.SetValue(BindableObject.BindingContextProperty, sharedSettings2);
			bindingExtension16.Mode = 1;
			bindingExtension16.Path = "IgnoreCodingErrors";
			BindingBase bindingBase16 = bindingExtension16.ProvideValue(null);
			labelSwitch.SetBinding(LabelSwitch.IsToggledProperty, bindingBase16);
			bindingExtension17.Path = "ShowExperimental";
			BindingBase bindingBase17 = bindingExtension17.ProvideValue(null);
			labelSwitch.SetBinding(VisualElement.IsVisibleProperty, bindingBase17);
			translate10.Text = "coding_ignore_fails";
			IMarkupExtension markupExtension19 = translate10;
			XamlServiceProvider xamlServiceProvider19 = new XamlServiceProvider();
			Type typeFromHandle37 = typeof(IProvideValueTarget);
			object[] array20 = new object[0 + 5];
			array20[0] = labelSwitch;
			array20[1] = stackLayout3;
			array20[2] = listView;
			array20[3] = grid;
			array20[4] = this;
			object obj33;
			xamlServiceProvider19.Add(typeFromHandle37, obj33 = new SimpleValueTargetProvider(array20, LabelSwitch.TextProperty, nameScope));
			xamlServiceProvider19.Add(typeof(IReferenceProvider), obj33);
			Type typeFromHandle38 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver19 = new XmlNamespaceResolver();
			xmlNamespaceResolver19.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver19.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver19.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver19.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver19.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver19.Add("settings", "clr-namespace:CarScannerXamarinForms.Settings");
			xamlServiceProvider19.Add(typeFromHandle38, new XamlTypeResolver(xmlNamespaceResolver19, typeof(CodingWithOptionsDetailsPage).GetTypeInfo().Assembly));
			xamlServiceProvider19.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(103, 29)));
			object obj34 = markupExtension19.ProvideValue(xamlServiceProvider19);
			labelSwitch.Text = obj34;
			stackLayout3.Children.Add(labelSwitch);
			listView.SetValue(ListView.FooterProperty, stackLayout3);
			grid.Children.Add(listView);
			activityFrame.SetValue(Grid.RowProperty, 0);
			activityFrame.SetValue(VisualElement.IsVisibleProperty, new VisualElement.VisibilityConverter().ConvertFromInvariantString("False"));
			grid.Children.Add(activityFrame);
			this.SetValue(ContentPage.ContentProperty, grid);
		}

		// Token: 0x06004D86 RID: 19846 RVA: 0x0039F7EC File Offset: 0x0039D9EC
		[CompilerGenerated]
		private void <CodingDetailsPage_Appearing>b__2_0(string s)
		{
			MainThread.BeginInvokeOnMainThread(delegate
			{
				this.activityFrame.Text = ActivityFrame.PLEASE_WAIT_TEXT + "\n" + s;
			});
		}

		// Token: 0x06004D87 RID: 19847 RVA: 0x0039F811 File Offset: 0x0039DA11
		[CompilerGenerated]
		private void <UpdateState>b__3_0(string s)
		{
			MainThread.BeginInvokeOnMainThread(delegate
			{
				this.activityFrame.Text = ActivityFrame.PLEASE_WAIT_TEXT + "\n" + s;
			});
		}

		// Token: 0x06004D88 RID: 19848 RVA: 0x0039F836 File Offset: 0x0039DA36
		[CompilerGenerated]
		private void <Lv_ItemSelected>b__6_0(string s)
		{
			Device.BeginInvokeOnMainThread(delegate
			{
				this.activityFrame.Text = s;
			});
		}

		// Token: 0x06004D89 RID: 19849 RVA: 0x0039F85C File Offset: 0x0039DA5C
		private void __InitComponentRuntime()
		{
			Extensions.LoadFromXaml<CodingWithOptionsDetailsPage>(this, typeof(CodingWithOptionsDetailsPage));
			this.lv = NameScopeExtensions.FindByName<ListView>(this, "lv");
			this.entryPassword = NameScopeExtensions.FindByName<Entry>(this, "entryPassword");
			this.labelState = NameScopeExtensions.FindByName<Label>(this, "labelState");
			this.btnUpdateState = NameScopeExtensions.FindByName<Button>(this, "btnUpdateState");
			this.activityFrame = NameScopeExtensions.FindByName<ActivityFrame>(this, "activityFrame");
		}

		// Token: 0x04002E0E RID: 11790
		private bool first_appearing = true;

		// Token: 0x04002E0F RID: 11791
		private ICodingContainer coding;

		// Token: 0x04002E10 RID: 11792
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private ListView lv;

		// Token: 0x04002E11 RID: 11793
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private Entry entryPassword;

		// Token: 0x04002E12 RID: 11794
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private Label labelState;

		// Token: 0x04002E13 RID: 11795
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private Button btnUpdateState;

		// Token: 0x04002E14 RID: 11796
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private ActivityFrame activityFrame;

		// Token: 0x0200090B RID: 2315
		[CompilerGenerated]
		private sealed class <>c__DisplayClass2_0
		{
			// Token: 0x06004D8A RID: 19850 RVA: 0x00002050 File Offset: 0x00000250
			public <>c__DisplayClass2_0()
			{
			}

			// Token: 0x06004D8B RID: 19851 RVA: 0x0039F8CF File Offset: 0x0039DACF
			internal void <CodingDetailsPage_Appearing>b__1()
			{
				this.<>4__this.activityFrame.Text = ActivityFrame.PLEASE_WAIT_TEXT + "\n" + this.s;
			}

			// Token: 0x04002E15 RID: 11797
			public string s;

			// Token: 0x04002E16 RID: 11798
			public CodingWithOptionsDetailsPage <>4__this;
		}

		// Token: 0x0200090C RID: 2316
		[CompilerGenerated]
		private sealed class <>c__DisplayClass3_0
		{
			// Token: 0x06004D8C RID: 19852 RVA: 0x00002050 File Offset: 0x00000250
			public <>c__DisplayClass3_0()
			{
			}

			// Token: 0x06004D8D RID: 19853 RVA: 0x0039F8F6 File Offset: 0x0039DAF6
			internal void <UpdateState>b__1()
			{
				this.<>4__this.activityFrame.Text = ActivityFrame.PLEASE_WAIT_TEXT + "\n" + this.s;
			}

			// Token: 0x04002E17 RID: 11799
			public string s;

			// Token: 0x04002E18 RID: 11800
			public CodingWithOptionsDetailsPage <>4__this;
		}

		// Token: 0x0200090D RID: 2317
		[CompilerGenerated]
		private sealed class <>c__DisplayClass6_0
		{
			// Token: 0x06004D8E RID: 19854 RVA: 0x00002050 File Offset: 0x00000250
			public <>c__DisplayClass6_0()
			{
			}

			// Token: 0x06004D8F RID: 19855 RVA: 0x0039F91D File Offset: 0x0039DB1D
			internal void <Lv_ItemSelected>b__1()
			{
				this.<>4__this.activityFrame.Text = this.s;
			}

			// Token: 0x04002E19 RID: 11801
			public string s;

			// Token: 0x04002E1A RID: 11802
			public CodingWithOptionsDetailsPage <>4__this;
		}

		// Token: 0x0200090E RID: 2318
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <BtnUpdateState_Clicked>d__4 : IAsyncStateMachine
		{
			// Token: 0x06004D90 RID: 19856 RVA: 0x0039F938 File Offset: 0x0039DB38
			void IAsyncStateMachine.MoveNext()
			{
				CodingWithOptionsDetailsPage codingWithOptionsDetailsPage = this;
				try
				{
					codingWithOptionsDetailsPage.UpdateState();
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

			// Token: 0x06004D91 RID: 19857 RVA: 0x0039F990 File Offset: 0x0039DB90
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x04002E1B RID: 11803
			public int <>1__state;

			// Token: 0x04002E1C RID: 11804
			public AsyncVoidMethodBuilder <>t__builder;

			// Token: 0x04002E1D RID: 11805
			public CodingWithOptionsDetailsPage <>4__this;
		}

		// Token: 0x0200090F RID: 2319
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <CodingDetailsPage_Appearing>d__2 : IAsyncStateMachine
		{
			// Token: 0x06004D92 RID: 19858 RVA: 0x0039F9A0 File Offset: 0x0039DBA0
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				CodingWithOptionsDetailsPage codingWithOptionsDetailsPage = this;
				try
				{
					TaskAwaiter<CodingRequestResult> taskAwaiter;
					if (num != 0)
					{
						if (!codingWithOptionsDetailsPage.first_appearing)
						{
							goto IL_00C9;
						}
						codingWithOptionsDetailsPage.first_appearing = false;
						codingWithOptionsDetailsPage.lv.IsEnabled = false;
						codingWithOptionsDetailsPage.activityFrame.IsVisible = true;
						Progress<string> progress = new Progress<string>(delegate(string s)
						{
							MainThread.BeginInvokeOnMainThread(new Action(new CodingWithOptionsDetailsPage.<>c__DisplayClass2_0
							{
								<>4__this = codingWithOptionsDetailsPage,
								s = s
							}.<CodingDetailsPage_Appearing>b__1));
						});
						taskAwaiter = codingWithOptionsDetailsPage.coding.UpdateCurrentState("", progress).GetAwaiter();
						if (!taskAwaiter.IsCompleted)
						{
							num2 = 0;
							TaskAwaiter<CodingRequestResult> taskAwaiter2 = taskAwaiter;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<CodingRequestResult>, CodingWithOptionsDetailsPage.<CodingDetailsPage_Appearing>d__2>(ref taskAwaiter, ref this);
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
					codingWithOptionsDetailsPage.activityFrame.IsVisible = false;
					codingWithOptionsDetailsPage.lv.IsEnabled = true;
					IL_00C9:;
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

			// Token: 0x06004D93 RID: 19859 RVA: 0x0039FAB4 File Offset: 0x0039DCB4
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x04002E1E RID: 11806
			public int <>1__state;

			// Token: 0x04002E1F RID: 11807
			public AsyncVoidMethodBuilder <>t__builder;

			// Token: 0x04002E20 RID: 11808
			public CodingWithOptionsDetailsPage <>4__this;

			// Token: 0x04002E21 RID: 11809
			private TaskAwaiter<CodingRequestResult> <>u__1;
		}

		// Token: 0x02000910 RID: 2320
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <Lv_ItemSelected>d__6 : IAsyncStateMachine
		{
			// Token: 0x06004D94 RID: 19860 RVA: 0x0039FAC4 File Offset: 0x0039DCC4
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				CodingWithOptionsDetailsPage codingWithOptionsDetailsPage = this;
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
						goto IL_0163;
					}
					case 2:
						taskAwaiter3 = taskAwaiter2;
						taskAwaiter2 = default(TaskAwaiter<bool>);
						num2 = -1;
						goto IL_021D;
					case 3:
					{
						TaskAwaiter taskAwaiter5;
						taskAwaiter4 = taskAwaiter5;
						taskAwaiter5 = default(TaskAwaiter);
						num2 = -1;
						goto IL_028B;
					}
					case 4:
					{
						TaskAwaiter<CodingRequestResult> taskAwaiter7;
						taskAwaiter6 = taskAwaiter7;
						taskAwaiter7 = default(TaskAwaiter<CodingRequestResult>);
						num2 = -1;
						goto IL_0341;
					}
					case 5:
					{
						TaskAwaiter taskAwaiter5;
						taskAwaiter4 = taskAwaiter5;
						taskAwaiter5 = default(TaskAwaiter);
						num2 = -1;
						goto IL_03E6;
					}
					case 6:
					{
						TaskAwaiter taskAwaiter5;
						taskAwaiter4 = taskAwaiter5;
						taskAwaiter5 = default(TaskAwaiter);
						num2 = -1;
						goto IL_045B;
					}
					case 7:
					{
						TaskAwaiter<CodingRequestResult> taskAwaiter7;
						taskAwaiter6 = taskAwaiter7;
						taskAwaiter7 = default(TaskAwaiter<CodingRequestResult>);
						num2 = -1;
						goto IL_04C9;
					}
					default:
					{
						if (e.SelectedItem == null)
						{
							goto IL_0500;
						}
						MQBAdaptationOption mqbadaptationOption = (MQBAdaptationOption)e.SelectedItem;
						codingWithOptionsDetailsPage.lv.SelectedItem = null;
						if (codingWithOptionsDetailsPage.coding.RequiresPro && !SharedSettings.Current.AdsProductPurchased)
						{
							taskAwaiter3 = codingWithOptionsDetailsPage.DisplayAlert("Car Scanner Pro", Translate.GetString("FeatureLocked"), "OK", Translate.GetString("btnCancel.Content")).GetAwaiter();
							if (!taskAwaiter3.IsCompleted)
							{
								num2 = 0;
								taskAwaiter2 = taskAwaiter3;
								this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<bool>, CodingWithOptionsDetailsPage.<Lv_ItemSelected>d__6>(ref taskAwaiter3, ref this);
								return;
							}
						}
						else if (SharedSettings.Current.CodingsCounter >= 3 && !SharedSettings.Current.AdsProductPurchased)
						{
							taskAwaiter3 = codingWithOptionsDetailsPage.DisplayAlert("Car Scanner Pro", string.Format(Translate.GetString("coding_FreeCodingsFinished"), SharedSettings.Current.CodingsCounter, 3), "OK", Translate.GetString("btnCancel.Content")).GetAwaiter();
							if (!taskAwaiter3.IsCompleted)
							{
								num2 = 2;
								taskAwaiter2 = taskAwaiter3;
								this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<bool>, CodingWithOptionsDetailsPage.<Lv_ItemSelected>d__6>(ref taskAwaiter3, ref this);
								return;
							}
							goto IL_021D;
						}
						else
						{
							codingWithOptionsDetailsPage.activityFrame.IsVisible = true;
							codingWithOptionsDetailsPage.lv.IsEnabled = false;
							progress = new Progress<string>(delegate(string s)
							{
								Device.BeginInvokeOnMainThread(new Action(new CodingWithOptionsDetailsPage.<>c__DisplayClass6_0
								{
									<>4__this = codingWithOptionsDetailsPage,
									s = s
								}.<Lv_ItemSelected>b__1));
							});
							taskAwaiter6 = codingWithOptionsDetailsPage.coding.Execute(codingWithOptionsDetailsPage.entryPassword.Text, mqbadaptationOption.Value, mqbadaptationOption.Title, progress, null, false).GetAwaiter();
							if (!taskAwaiter6.IsCompleted)
							{
								num2 = 4;
								TaskAwaiter<CodingRequestResult> taskAwaiter7 = taskAwaiter6;
								this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<CodingRequestResult>, CodingWithOptionsDetailsPage.<Lv_ItemSelected>d__6>(ref taskAwaiter6, ref this);
								return;
							}
							goto IL_0341;
						}
						break;
					}
					}
					if (!taskAwaiter3.GetResult())
					{
						goto IL_016A;
					}
					Page inAppPage = InAppManager.GetInAppPage();
					taskAwaiter4 = codingWithOptionsDetailsPage.Navigation.PushAsync(inAppPage).GetAwaiter();
					if (!taskAwaiter4.IsCompleted)
					{
						num2 = 1;
						TaskAwaiter taskAwaiter5 = taskAwaiter4;
						this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, CodingWithOptionsDetailsPage.<Lv_ItemSelected>d__6>(ref taskAwaiter4, ref this);
						return;
					}
					IL_0163:
					taskAwaiter4.GetResult();
					IL_016A:
					goto IL_051B;
					IL_021D:
					if (!taskAwaiter3.GetResult())
					{
						goto IL_0292;
					}
					Page inAppPage2 = InAppManager.GetInAppPage();
					taskAwaiter4 = codingWithOptionsDetailsPage.Navigation.PushAsync(inAppPage2).GetAwaiter();
					if (!taskAwaiter4.IsCompleted)
					{
						num2 = 3;
						TaskAwaiter taskAwaiter5 = taskAwaiter4;
						this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, CodingWithOptionsDetailsPage.<Lv_ItemSelected>d__6>(ref taskAwaiter4, ref this);
						return;
					}
					IL_028B:
					taskAwaiter4.GetResult();
					IL_0292:
					goto IL_051B;
					IL_0341:
					CodingRequestResult result = taskAwaiter6.GetResult();
					if (result == CodingRequestResult.Success)
					{
						SharedSettings sharedSettings = SharedSettings.Current;
						int codingsCounter = sharedSettings.CodingsCounter;
						sharedSettings.CodingsCounter = codingsCounter + 1;
						if (!(codingWithOptionsDetailsPage.coding is IServiceProcedure))
						{
							goto IL_0462;
						}
						taskAwaiter4 = codingWithOptionsDetailsPage.DisplayAlert(codingWithOptionsDetailsPage.coding.Name, Translate.GetString("coding_OperationFinished"), "OK").GetAwaiter();
						if (!taskAwaiter4.IsCompleted)
						{
							num2 = 5;
							TaskAwaiter taskAwaiter5 = taskAwaiter4;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, CodingWithOptionsDetailsPage.<Lv_ItemSelected>d__6>(ref taskAwaiter4, ref this);
							return;
						}
					}
					else
					{
						taskAwaiter4 = codingWithOptionsDetailsPage.DisplayAlert(Translate.GetString("coding_Error"), MQBAdaptationTemplate.CodingRequestResultToString(result), "OK").GetAwaiter();
						if (!taskAwaiter4.IsCompleted)
						{
							num2 = 6;
							TaskAwaiter taskAwaiter5 = taskAwaiter4;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, CodingWithOptionsDetailsPage.<Lv_ItemSelected>d__6>(ref taskAwaiter4, ref this);
							return;
						}
						goto IL_045B;
					}
					IL_03E6:
					taskAwaiter4.GetResult();
					goto IL_0462;
					IL_045B:
					taskAwaiter4.GetResult();
					IL_0462:
					taskAwaiter6 = codingWithOptionsDetailsPage.coding.UpdateCurrentState("", progress).GetAwaiter();
					if (!taskAwaiter6.IsCompleted)
					{
						num2 = 7;
						TaskAwaiter<CodingRequestResult> taskAwaiter7 = taskAwaiter6;
						this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<CodingRequestResult>, CodingWithOptionsDetailsPage.<Lv_ItemSelected>d__6>(ref taskAwaiter6, ref this);
						return;
					}
					IL_04C9:
					taskAwaiter6.GetResult();
					codingWithOptionsDetailsPage.activityFrame.IsVisible = false;
					codingWithOptionsDetailsPage.activityFrame.Text = ActivityFrame.PLEASE_WAIT_TEXT;
					codingWithOptionsDetailsPage.lv.IsEnabled = true;
					progress = null;
					IL_0500:;
				}
				catch (Exception ex)
				{
					num2 = -2;
					this.<>t__builder.SetException(ex);
					return;
				}
				IL_051B:
				num2 = -2;
				this.<>t__builder.SetResult();
			}

			// Token: 0x06004D95 RID: 19861 RVA: 0x003A001C File Offset: 0x0039E21C
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x04002E22 RID: 11810
			public int <>1__state;

			// Token: 0x04002E23 RID: 11811
			public AsyncVoidMethodBuilder <>t__builder;

			// Token: 0x04002E24 RID: 11812
			public SelectedItemChangedEventArgs e;

			// Token: 0x04002E25 RID: 11813
			public CodingWithOptionsDetailsPage <>4__this;

			// Token: 0x04002E26 RID: 11814
			private Progress<string> <progress>5__2;

			// Token: 0x04002E27 RID: 11815
			private TaskAwaiter<bool> <>u__1;

			// Token: 0x04002E28 RID: 11816
			private TaskAwaiter <>u__2;

			// Token: 0x04002E29 RID: 11817
			private TaskAwaiter<CodingRequestResult> <>u__3;
		}

		// Token: 0x02000911 RID: 2321
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <UpdateState>d__3 : IAsyncStateMachine
		{
			// Token: 0x06004D96 RID: 19862 RVA: 0x003A002C File Offset: 0x0039E22C
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				CodingWithOptionsDetailsPage codingWithOptionsDetailsPage = this;
				try
				{
					TaskAwaiter<CodingRequestResult> taskAwaiter;
					if (num != 0)
					{
						codingWithOptionsDetailsPage.activityFrame.Text = ActivityFrame.PLEASE_WAIT_TEXT;
						codingWithOptionsDetailsPage.activityFrame.IsVisible = true;
						codingWithOptionsDetailsPage.lv.IsEnabled = false;
						Progress<string> progress = new Progress<string>(delegate(string s)
						{
							MainThread.BeginInvokeOnMainThread(new Action(new CodingWithOptionsDetailsPage.<>c__DisplayClass3_0
							{
								<>4__this = codingWithOptionsDetailsPage,
								s = s
							}.<UpdateState>b__1));
						});
						taskAwaiter = codingWithOptionsDetailsPage.coding.UpdateCurrentState("", progress).GetAwaiter();
						if (!taskAwaiter.IsCompleted)
						{
							num2 = 0;
							TaskAwaiter<CodingRequestResult> taskAwaiter2 = taskAwaiter;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<CodingRequestResult>, CodingWithOptionsDetailsPage.<UpdateState>d__3>(ref taskAwaiter, ref this);
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
					codingWithOptionsDetailsPage.activityFrame.IsVisible = false;
					codingWithOptionsDetailsPage.lv.IsEnabled = true;
					codingWithOptionsDetailsPage.activityFrame.Text = ActivityFrame.PLEASE_WAIT_TEXT;
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

			// Token: 0x06004D97 RID: 19863 RVA: 0x003A0150 File Offset: 0x0039E350
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x04002E2A RID: 11818
			public int <>1__state;

			// Token: 0x04002E2B RID: 11819
			public AsyncTaskMethodBuilder <>t__builder;

			// Token: 0x04002E2C RID: 11820
			public CodingWithOptionsDetailsPage <>4__this;

			// Token: 0x04002E2D RID: 11821
			private TaskAwaiter<CodingRequestResult> <>u__1;
		}

		// Token: 0x02000912 RID: 2322
		[CompilerGenerated]
		private sealed class <InitializeComponent>_anonXamlCDataTemplate_11
		{
			// Token: 0x06004D98 RID: 19864 RVA: 0x003A0160 File Offset: 0x0039E360
			public <InitializeComponent>_anonXamlCDataTemplate_11()
			{
			}

			// Token: 0x06004D99 RID: 19865 RVA: 0x003A0174 File Offset: 0x0039E374
			internal object LoadDataTemplate()
			{
				DynamicResourceExtension dynamicResourceExtension;
				VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension = new DynamicResourceExtension(), new Uri("Coding\\Pages\\CodingWithOptionsDetailsPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 81, 33);
				BindingExtension bindingExtension;
				VisualDiagnostics.RegisterSourceInfo(bindingExtension = new BindingExtension(), new Uri("Coding\\Pages\\CodingWithOptionsDetailsPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 82, 33);
				DynamicResourceExtension dynamicResourceExtension2;
				VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension2 = new DynamicResourceExtension(), new Uri("Coding\\Pages\\CodingWithOptionsDetailsPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 83, 33);
				Label label;
				VisualDiagnostics.RegisterSourceInfo(label = new Label(), new Uri("Coding\\Pages\\CodingWithOptionsDetailsPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 79, 30);
				ViewCell viewCell;
				VisualDiagnostics.RegisterSourceInfo(viewCell = new ViewCell(), new Uri("Coding\\Pages\\CodingWithOptionsDetailsPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 78, 26);
				NameScope nameScope = new NameScope();
				NameScope.SetNameScope(viewCell, nameScope);
				label.SetValue(View.MarginProperty, new Thickness(0.0, 5.0, 0.0, 5.0));
				dynamicResourceExtension.Key = "BaseFontSize+";
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
				xmlNamespaceResolver.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
				xmlNamespaceResolver.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
				xmlNamespaceResolver.Add("local", "clr-namespace:CarScannerXamarinForms");
				xmlNamespaceResolver.Add("settings", "clr-namespace:CarScannerXamarinForms.Settings");
				xamlServiceProvider.Add(typeFromHandle2, new XamlTypeResolver(xmlNamespaceResolver, typeof(CodingWithOptionsDetailsPage.<InitializeComponent>_anonXamlCDataTemplate_11).GetTypeInfo().Assembly));
				xamlServiceProvider.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(81, 33)));
				DynamicResource dynamicResource = markupExtension.ProvideValue(xamlServiceProvider);
				label.SetDynamicResource(Label.FontSizeProperty, dynamicResource.Key);
				bindingExtension.Path = "Title";
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
				xmlNamespaceResolver2.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
				xmlNamespaceResolver2.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
				xmlNamespaceResolver2.Add("local", "clr-namespace:CarScannerXamarinForms");
				xmlNamespaceResolver2.Add("settings", "clr-namespace:CarScannerXamarinForms.Settings");
				xamlServiceProvider2.Add(typeFromHandle4, new XamlTypeResolver(xmlNamespaceResolver2, typeof(CodingWithOptionsDetailsPage.<InitializeComponent>_anonXamlCDataTemplate_11).GetTypeInfo().Assembly));
				xamlServiceProvider2.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(83, 33)));
				DynamicResource dynamicResource2 = markupExtension2.ProvideValue(xamlServiceProvider2);
				label.SetDynamicResource(Label.TextColorProperty, dynamicResource2.Key);
				viewCell.View = label;
				return viewCell;
			}

			// Token: 0x04002E2E RID: 11822
			internal object[] parentValues;

			// Token: 0x04002E2F RID: 11823
			internal CodingWithOptionsDetailsPage root;
		}
	}
}
