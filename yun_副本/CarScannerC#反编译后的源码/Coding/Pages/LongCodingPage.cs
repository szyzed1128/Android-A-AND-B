using System;
using System.CodeDom.Compiler;
using System.Diagnostics;
using System.Globalization;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using CarScannerXamarinForms.Common.XAMLConverters;
using CarScannerXamarinForms.InApp;
using CarScannerXamarinForms.OBD2;
using CarScannerXamarinForms.PlatformAdapters;
using CarScannerXamarinForms.Settings;
using Xamarin.Forms;
using Xamarin.Forms.Internals;
using Xamarin.Forms.PlatformConfiguration.iOSSpecific;
using Xamarin.Forms.Xaml;
using Xamarin.Forms.Xaml.Diagnostics;
using Xamarin.Forms.Xaml.Internals;

namespace CarScannerXamarinForms.Coding.Pages
{
	// Token: 0x02000953 RID: 2387
	[XamlCompilation(2)]
	[XamlFilePath("Coding\\Pages\\LongCodingPage.xaml")]
	public class LongCodingPage : ContentPage
	{
		// Token: 0x06004E9B RID: 20123 RVA: 0x003B9940 File Offset: 0x003B7B40
		public LongCodingPage(ICodingContainer coding)
		{
			this.InitializeComponent();
			base.Appearing += this.CodingDetailsPage_Appearing;
			this.Model = new LongCodingModel(coding);
			base.BindingContext = this.Model;
			this.switchCustomAddress.IsVisible = SharedSettings.Current.AdsProductPurchased;
			MQBAdaptationTemplate mqbadaptationTemplate = (MQBAdaptationTemplate)this.Model.Coding;
			this.defaultAddress = mqbadaptationTemplate.Address;
		}

		// Token: 0x06004E9C RID: 20124 RVA: 0x003B99C8 File Offset: 0x003B7BC8
		private async void btnResetOnOff_Clicked(object sender, EventArgs e)
		{
			await this.ResetECU("02");
		}

		// Token: 0x06004E9D RID: 20125 RVA: 0x003B9A00 File Offset: 0x003B7C00
		private async void btnResetHard_Clicked(object sender, EventArgs e)
		{
			await this.ResetECU("01");
		}

		// Token: 0x06004E9E RID: 20126 RVA: 0x003B9A38 File Offset: 0x003B7C38
		private async void btnResetSoft_Clicked(object sender, EventArgs e)
		{
			await this.ResetECU("03");
		}

		// Token: 0x06004E9F RID: 20127 RVA: 0x003B9A70 File Offset: 0x003B7C70
		private async Task ResetECU(string reset_value)
		{
			this.lv.IsEnabled = false;
			this.activityFrame.IsVisible = true;
			MQBLongCoding mqblongCoding = (MQBLongCoding)this.Model.Coding;
			SemaphoreSlim semaphoreSlim = new SemaphoreSlim(0, 1);
			OBDRequest obdrequest = new OBDRequest("11" + reset_value, mqblongCoding.RequestHeader, "ATSP6;ATSH" + mqblongCoding.RequestHeader + ";1003", "", false);
			obdrequest.ResponseReceived += delegate(OBDRequest request, string data)
			{
				semaphoreSlim.Release();
			};
			App.OBDReader.ReplaceQueue(new OBDRequest[] { obdrequest });
			await semaphoreSlim.WaitAsync();
			this.lv.IsEnabled = true;
			this.activityFrame.IsVisible = false;
		}

		// Token: 0x1700178F RID: 6031
		// (get) Token: 0x06004EA0 RID: 20128 RVA: 0x003B9ABB File Offset: 0x003B7CBB
		// (set) Token: 0x06004EA1 RID: 20129 RVA: 0x003B9AC3 File Offset: 0x003B7CC3
		internal LongCodingModel Model
		{
			[CompilerGenerated]
			get
			{
				return this.<Model>k__BackingField;
			}
			[CompilerGenerated]
			private set
			{
				this.<Model>k__BackingField = value;
			}
		}

		// Token: 0x06004EA2 RID: 20130 RVA: 0x003B9ACC File Offset: 0x003B7CCC
		private async void CodingDetailsPage_Appearing(object sender, EventArgs e)
		{
			if (this.first_appearing)
			{
				this.first_appearing = false;
				await this.UpdateState();
			}
		}

		// Token: 0x06004EA3 RID: 20131 RVA: 0x003B9B04 File Offset: 0x003B7D04
		public async Task UpdateState()
		{
			this.activityFrame.IsVisible = true;
			this.lv.IsEnabled = false;
			if (this.Model.UseCustomAddress)
			{
				int num = 0;
				if (this.Model.CustomAddress.Length != 4 || !int.TryParse(this.Model.CustomAddress, NumberStyles.HexNumber, CultureInfo.InvariantCulture.NumberFormat, out num))
				{
					await base.DisplayAlert("Wrong custom address format!", "Address should be in HEX format.\nCorrect format example: 01FF", "OK");
					return;
				}
				((MQBAdaptationTemplate)this.Model.Coding).Address = this.Model.CustomAddress;
			}
			else
			{
				((MQBAdaptationTemplate)this.Model.Coding).Address = this.defaultAddress;
			}
			string text = "";
			if (SharedSettings.Current.DeveloperMode)
			{
				text = this.entryPassword.Text;
			}
			TaskAwaiter<CodingRequestResult> taskAwaiter = this.Model.Coding.UpdateCurrentState(text, null).GetAwaiter();
			if (!taskAwaiter.IsCompleted)
			{
				await taskAwaiter;
				TaskAwaiter<CodingRequestResult> taskAwaiter2;
				taskAwaiter = taskAwaiter2;
				taskAwaiter2 = default(TaskAwaiter<CodingRequestResult>);
			}
			if (taskAwaiter.GetResult() == CodingRequestResult.Success)
			{
				this.Model.Data = new HexStringObject(this.Model.Coding.CurrentState);
			}
			this.activityFrame.IsVisible = false;
			this.lv.IsEnabled = true;
		}

		// Token: 0x06004EA4 RID: 20132 RVA: 0x003B9B48 File Offset: 0x003B7D48
		private async void BtnUpdateState_Clicked(object sender, EventArgs e)
		{
			this.UpdateState();
		}

		// Token: 0x06004EA5 RID: 20133 RVA: 0x00022295 File Offset: 0x00020495
		private void Entry_Completed(object sender, EventArgs e)
		{
			PlatformHelper.CommonService.HideKeyboard();
		}

		// Token: 0x06004EA6 RID: 20134 RVA: 0x003B9B80 File Offset: 0x003B7D80
		private void FullHexEditor_Completed(object sender, EventArgs e)
		{
			PlatformHelper.CommonService.HideKeyboard();
			Editor editor = (Editor)sender;
			this.Model.Data.ApplyChanges(editor.Text);
		}

		// Token: 0x06004EA7 RID: 20135 RVA: 0x003B9BB4 File Offset: 0x003B7DB4
		private void FullHexEditor_Unfocused(object sender, FocusEventArgs e)
		{
			Editor editor = (Editor)sender;
			this.Model.Data.ApplyChanges(editor.Text);
			PlatformHelper.CommonService.HideKeyboard();
		}

		// Token: 0x06004EA8 RID: 20136 RVA: 0x003B9BE8 File Offset: 0x003B7DE8
		private void BinEntry_Unfocused(object sender, FocusEventArgs e)
		{
			Entry entry = (Entry)sender;
			((ByteObject)entry.BindingContext).Bin = entry.Text;
			PlatformHelper.CommonService.HideKeyboard();
		}

		// Token: 0x06004EA9 RID: 20137 RVA: 0x003B9C1C File Offset: 0x003B7E1C
		private void BinEntry_Completed(object sender, EventArgs e)
		{
			Entry entry = (Entry)sender;
			((ByteObject)entry.BindingContext).Bin = entry.Text;
			PlatformHelper.CommonService.HideKeyboard();
		}

		// Token: 0x06004EAA RID: 20138 RVA: 0x003B9C50 File Offset: 0x003B7E50
		private void HexEntry_Unfocused(object sender, FocusEventArgs e)
		{
			Entry entry = (Entry)sender;
			((ByteObject)entry.BindingContext).Hex = entry.Text;
			PlatformHelper.CommonService.HideKeyboard();
		}

		// Token: 0x06004EAB RID: 20139 RVA: 0x003B9C84 File Offset: 0x003B7E84
		private void HexEntry_Completed(object sender, EventArgs e)
		{
			Entry entry = (Entry)sender;
			((ByteObject)entry.BindingContext).Hex = entry.Text;
			PlatformHelper.CommonService.HideKeyboard();
		}

		// Token: 0x06004EAC RID: 20140 RVA: 0x003B9CB8 File Offset: 0x003B7EB8
		private async void btnSaveState_Clicked(object sender, EventArgs e)
		{
			if (this.Model.Coding.RequiresPro && !SharedSettings.Current.AdsProductPurchased)
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
				if (this.Model.UseCustomAddress)
				{
					int num = 0;
					if (this.Model.CustomAddress.Length != 4 || !int.TryParse(this.Model.CustomAddress, NumberStyles.HexNumber, CultureInfo.InvariantCulture.NumberFormat, out num))
					{
						await base.DisplayAlert("Wrong custom address format!", "Address should be in HEX format.\nCorrect format example: 01FF", "OK");
						return;
					}
					((MQBAdaptationTemplate)this.Model.Coding).Address = this.Model.CustomAddress;
				}
				else
				{
					((MQBAdaptationTemplate)this.Model.Coding).Address = this.defaultAddress;
				}
				this.activityFrame.IsVisible = true;
				this.lv.IsEnabled = false;
				Progress<string> progress = new Progress<string>(delegate(string s)
				{
					Device.BeginInvokeOnMainThread(delegate
					{
						this.activityFrame.Text = s;
					});
				});
				CodingRequestResult codingRequestResult = await this.Model.Coding.Execute(this.entryPassword.Text, this.Model.Data.Hex.Replace(" ", ""), this.Model.Data.Hex, progress, null, false);
				if (codingRequestResult == CodingRequestResult.Success)
				{
					SharedSettings.Current.CodingsCounter++;
				}
				else
				{
					await base.DisplayAlert(Translate.GetString("coding_Error"), MQBAdaptationTemplate.CodingRequestResultToString(codingRequestResult), "OK");
				}
				await this.UpdateState();
				this.activityFrame.IsVisible = false;
				this.activityFrame.Text = ActivityFrame.PLEASE_WAIT_TEXT;
				this.lv.IsEnabled = true;
			}
		}

		// Token: 0x06004EAD RID: 20141 RVA: 0x003B9CF0 File Offset: 0x003B7EF0
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private void InitializeComponent()
		{
			if (ResourceLoader.IsEnabled && ResourceLoader.CanProvideContentFor(new ResourceLoader.ResourceLoadingQuery
			{
				AssemblyName = typeof(LongCodingPage).GetTypeInfo().Assembly.GetName(),
				ResourcePath = "Coding/Pages/LongCodingPage.xaml",
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
			VisualDiagnostics.RegisterSourceInfo(translate = new Translate(), new Uri("Coding\\Pages\\LongCodingPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 10, 5);
			DynamicResourceExtension dynamicResourceExtension;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension = new DynamicResourceExtension(), new Uri("Coding\\Pages\\LongCodingPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 13, 5);
			EmptyStringToFalseConverter emptyStringToFalseConverter;
			VisualDiagnostics.RegisterSourceInfo(emptyStringToFalseConverter = new EmptyStringToFalseConverter(), new Uri("Coding\\Pages\\LongCodingPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 16, 14);
			VagCodingPlatformToTrueConverter vagCodingPlatformToTrueConverter;
			VisualDiagnostics.RegisterSourceInfo(vagCodingPlatformToTrueConverter = new VagCodingPlatformToTrueConverter(), new Uri("Coding\\Pages\\LongCodingPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 17, 14);
			ResourceDictionary resourceDictionary;
			VisualDiagnostics.RegisterSourceInfo(resourceDictionary = new ResourceDictionary(), new Uri("Coding\\Pages\\LongCodingPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 15, 10);
			RowDefinition rowDefinition;
			VisualDiagnostics.RegisterSourceInfo(rowDefinition = new RowDefinition(), new Uri("Coding\\Pages\\LongCodingPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 23, 18);
			BindingExtension bindingExtension;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension = new BindingExtension(), new Uri("Coding\\Pages\\LongCodingPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 29, 17);
			DynamicResourceExtension dynamicResourceExtension2;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension2 = new DynamicResourceExtension(), new Uri("Coding\\Pages\\LongCodingPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 35, 29);
			BindingExtension bindingExtension2;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension2 = new BindingExtension(), new Uri("Coding\\Pages\\LongCodingPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 36, 29);
			Label label;
			VisualDiagnostics.RegisterSourceInfo(label = new Label(), new Uri("Coding\\Pages\\LongCodingPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 33, 26);
			StaticResourceExtension staticResourceExtension;
			VisualDiagnostics.RegisterSourceInfo(staticResourceExtension = new StaticResourceExtension(), new Uri("Coding\\Pages\\LongCodingPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 37, 32);
			BindingExtension bindingExtension3;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension3 = new BindingExtension(), new Uri("Coding\\Pages\\LongCodingPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 37, 32);
			BindingExtension bindingExtension4;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension4 = new BindingExtension(), new Uri("Coding\\Pages\\LongCodingPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 37, 129);
			Label label2;
			VisualDiagnostics.RegisterSourceInfo(label2 = new Label(), new Uri("Coding\\Pages\\LongCodingPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 37, 26);
			Translate translate2;
			VisualDiagnostics.RegisterSourceInfo(translate2 = new Translate(), new Uri("Coding\\Pages\\LongCodingPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 39, 32);
			Label label3;
			VisualDiagnostics.RegisterSourceInfo(label3 = new Label(), new Uri("Coding\\Pages\\LongCodingPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 39, 26);
			BindingExtension bindingExtension5;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension5 = new BindingExtension(), new Uri("Coding\\Pages\\LongCodingPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 45, 29);
			Entry entry;
			VisualDiagnostics.RegisterSourceInfo(entry = new Entry(), new Uri("Coding\\Pages\\LongCodingPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 40, 26);
			StaticResourceExtension staticResourceExtension2;
			VisualDiagnostics.RegisterSourceInfo(staticResourceExtension2 = new StaticResourceExtension(), new Uri("Coding\\Pages\\LongCodingPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 46, 56);
			BindingExtension bindingExtension6;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension6 = new BindingExtension(), new Uri("Coding\\Pages\\LongCodingPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 46, 56);
			Translate translate3;
			VisualDiagnostics.RegisterSourceInfo(translate3 = new Translate(), new Uri("Coding\\Pages\\LongCodingPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 49, 43);
			Span span;
			VisualDiagnostics.RegisterSourceInfo(span = new Span(), new Uri("Coding\\Pages\\LongCodingPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 49, 38);
			BindingExtension bindingExtension7;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension7 = new BindingExtension(), new Uri("Coding\\Pages\\LongCodingPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 50, 43);
			Span span2;
			VisualDiagnostics.RegisterSourceInfo(span2 = new Span(), new Uri("Coding\\Pages\\LongCodingPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 50, 38);
			FormattedString formattedString;
			VisualDiagnostics.RegisterSourceInfo(formattedString = new FormattedString(), new Uri("Coding\\Pages\\LongCodingPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 48, 34);
			Label label4;
			VisualDiagnostics.RegisterSourceInfo(label4 = new Label(), new Uri("Coding\\Pages\\LongCodingPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 46, 26);
			BindingExtension bindingExtension8;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension8 = new BindingExtension(), new Uri("Coding\\Pages\\LongCodingPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 57, 29);
			Translate translate4;
			VisualDiagnostics.RegisterSourceInfo(translate4 = new Translate(), new Uri("Coding\\Pages\\LongCodingPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 58, 29);
			LabelSwitch labelSwitch;
			VisualDiagnostics.RegisterSourceInfo(labelSwitch = new LabelSwitch(), new Uri("Coding\\Pages\\LongCodingPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 55, 26);
			BindingExtension bindingExtension9;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension9 = new BindingExtension(), new Uri("Coding\\Pages\\LongCodingPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 64, 29);
			BindingExtension bindingExtension10;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension10 = new BindingExtension(), new Uri("Coding\\Pages\\LongCodingPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 65, 29);
			Entry entry2;
			VisualDiagnostics.RegisterSourceInfo(entry2 = new Entry(), new Uri("Coding\\Pages\\LongCodingPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 59, 26);
			ColumnDefinition columnDefinition;
			VisualDiagnostics.RegisterSourceInfo(columnDefinition = new ColumnDefinition(), new Uri("Coding\\Pages\\LongCodingPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 69, 34);
			ColumnDefinition columnDefinition2;
			VisualDiagnostics.RegisterSourceInfo(columnDefinition2 = new ColumnDefinition(), new Uri("Coding\\Pages\\LongCodingPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 70, 34);
			BindingExtension bindingExtension11;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension11 = new BindingExtension(), new Uri("Coding\\Pages\\LongCodingPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 76, 33);
			Translate translate5;
			VisualDiagnostics.RegisterSourceInfo(translate5 = new Translate(), new Uri("Coding\\Pages\\LongCodingPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 77, 33);
			Button button;
			VisualDiagnostics.RegisterSourceInfo(button = new Button(), new Uri("Coding\\Pages\\LongCodingPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 72, 30);
			Translate translate6;
			VisualDiagnostics.RegisterSourceInfo(translate6 = new Translate(), new Uri("Coding\\Pages\\LongCodingPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 82, 33);
			Button button2;
			VisualDiagnostics.RegisterSourceInfo(button2 = new Button(), new Uri("Coding\\Pages\\LongCodingPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 78, 30);
			Grid grid;
			VisualDiagnostics.RegisterSourceInfo(grid = new Grid(), new Uri("Coding\\Pages\\LongCodingPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 67, 26);
			Translate translate7;
			VisualDiagnostics.RegisterSourceInfo(translate7 = new Translate(), new Uri("Coding\\Pages\\LongCodingPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 86, 54);
			Label label5;
			VisualDiagnostics.RegisterSourceInfo(label5 = new Label(), new Uri("Coding\\Pages\\LongCodingPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 86, 26);
			BindingExtension bindingExtension12;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension12 = new BindingExtension(), new Uri("Coding\\Pages\\LongCodingPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 94, 29);
			DynamicResourceExtension dynamicResourceExtension3;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension3 = new DynamicResourceExtension(), new Uri("Coding\\Pages\\LongCodingPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 95, 29);
			Editor editor;
			VisualDiagnostics.RegisterSourceInfo(editor = new Editor(), new Uri("Coding\\Pages\\LongCodingPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 87, 26);
			StackLayout stackLayout;
			VisualDiagnostics.RegisterSourceInfo(stackLayout = new StackLayout(), new Uri("Coding\\Pages\\LongCodingPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 32, 22);
			DataTemplate dataTemplate;
			VisualDiagnostics.RegisterSourceInfo(dataTemplate = new DataTemplate(), new Uri("Coding\\Pages\\LongCodingPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 103, 22);
			Translate translate8;
			VisualDiagnostics.RegisterSourceInfo(translate8 = new Translate(), new Uri("Coding\\Pages\\LongCodingPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 255, 54);
			Label label6;
			VisualDiagnostics.RegisterSourceInfo(label6 = new Label(), new Uri("Coding\\Pages\\LongCodingPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 255, 26);
			Translate translate9;
			VisualDiagnostics.RegisterSourceInfo(translate9 = new Translate(), new Uri("Coding\\Pages\\LongCodingPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 259, 29);
			Button button3;
			VisualDiagnostics.RegisterSourceInfo(button3 = new Button(), new Uri("Coding\\Pages\\LongCodingPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 256, 26);
			Translate translate10;
			VisualDiagnostics.RegisterSourceInfo(translate10 = new Translate(), new Uri("Coding\\Pages\\LongCodingPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 263, 29);
			Button button4;
			VisualDiagnostics.RegisterSourceInfo(button4 = new Button(), new Uri("Coding\\Pages\\LongCodingPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 260, 26);
			Translate translate11;
			VisualDiagnostics.RegisterSourceInfo(translate11 = new Translate(), new Uri("Coding\\Pages\\LongCodingPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 267, 29);
			Button button5;
			VisualDiagnostics.RegisterSourceInfo(button5 = new Button(), new Uri("Coding\\Pages\\LongCodingPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 264, 26);
			SharedSettings sharedSettings;
			VisualDiagnostics.RegisterSourceInfo(sharedSettings = SharedSettings.Current, new Uri("Coding\\Pages\\LongCodingPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 269, 29);
			StaticResourceExtension staticResourceExtension3;
			VisualDiagnostics.RegisterSourceInfo(staticResourceExtension3 = new StaticResourceExtension(), new Uri("Coding\\Pages\\LongCodingPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 270, 29);
			BindingExtension bindingExtension13;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension13 = new BindingExtension(), new Uri("Coding\\Pages\\LongCodingPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 270, 29);
			Translate translate12;
			VisualDiagnostics.RegisterSourceInfo(translate12 = new Translate(), new Uri("Coding\\Pages\\LongCodingPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 271, 29);
			Label label7;
			VisualDiagnostics.RegisterSourceInfo(label7 = new Label(), new Uri("Coding\\Pages\\LongCodingPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 268, 26);
			SharedSettings sharedSettings2;
			VisualDiagnostics.RegisterSourceInfo(sharedSettings2 = SharedSettings.Current, new Uri("Coding\\Pages\\LongCodingPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 273, 29);
			BindingExtension bindingExtension14;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension14 = new BindingExtension(), new Uri("Coding\\Pages\\LongCodingPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 274, 29);
			BindingExtension bindingExtension15;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension15 = new BindingExtension(), new Uri("Coding\\Pages\\LongCodingPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 275, 29);
			Translate translate13;
			VisualDiagnostics.RegisterSourceInfo(translate13 = new Translate(), new Uri("Coding\\Pages\\LongCodingPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 276, 29);
			LabelSwitch labelSwitch2;
			VisualDiagnostics.RegisterSourceInfo(labelSwitch2 = new LabelSwitch(), new Uri("Coding\\Pages\\LongCodingPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 272, 26);
			StackLayout stackLayout2;
			VisualDiagnostics.RegisterSourceInfo(stackLayout2 = new StackLayout(), new Uri("Coding\\Pages\\LongCodingPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 254, 22);
			ListView listView;
			VisualDiagnostics.RegisterSourceInfo(listView = new ListView(), new Uri("Coding\\Pages\\LongCodingPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 25, 14);
			ActivityFrame activityFrame;
			VisualDiagnostics.RegisterSourceInfo(activityFrame = new ActivityFrame(), new Uri("Coding\\Pages\\LongCodingPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 280, 14);
			Grid grid2;
			VisualDiagnostics.RegisterSourceInfo(grid2 = new Grid(), new Uri("Coding\\Pages\\LongCodingPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 21, 10);
			VisualDiagnostics.RegisterSourceInfo(this, new Uri("Coding\\Pages\\LongCodingPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 2, 2);
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
			nameScope.RegisterName("switchCustomAddress", labelSwitch);
			if (labelSwitch.StyleId == null)
			{
				labelSwitch.StyleId = "switchCustomAddress";
			}
			nameScope.RegisterName("entryCustomAddress", entry2);
			if (entry2.StyleId == null)
			{
				entry2.StyleId = "entryCustomAddress";
			}
			nameScope.RegisterName("btnUpdateState", button);
			if (button.StyleId == null)
			{
				button.StyleId = "btnUpdateState";
			}
			nameScope.RegisterName("btnSaveNewValue", button2);
			if (button2.StyleId == null)
			{
				button2.StyleId = "btnSaveNewValue";
			}
			nameScope.RegisterName("editorHex", editor);
			if (editor.StyleId == null)
			{
				editor.StyleId = "editorHex";
			}
			nameScope.RegisterName("btnResetOnOff", button3);
			if (button3.StyleId == null)
			{
				button3.StyleId = "btnResetOnOff";
			}
			nameScope.RegisterName("btnResetSoft", button4);
			if (button4.StyleId == null)
			{
				button4.StyleId = "btnResetSoft";
			}
			nameScope.RegisterName("btnResetHard", button5);
			if (button5.StyleId == null)
			{
				button5.StyleId = "btnResetHard";
			}
			nameScope.RegisterName("activityFrame", activityFrame);
			if (activityFrame.StyleId == null)
			{
				activityFrame.StyleId = "activityFrame";
			}
			this.lv = listView;
			this.entryPassword = entry;
			this.switchCustomAddress = labelSwitch;
			this.entryCustomAddress = entry2;
			this.btnUpdateState = button;
			this.btnSaveNewValue = button2;
			this.editorHex = editor;
			this.btnResetOnOff = button3;
			this.btnResetSoft = button4;
			this.btnResetHard = button5;
			this.activityFrame = activityFrame;
			this.Resources = resourceDictionary;
			resourceDictionary.Add("EmptyStringToFalseConverter", emptyStringToFalseConverter);
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
			xamlServiceProvider.Add(typeFromHandle2, new XamlTypeResolver(xmlNamespaceResolver, typeof(LongCodingPage).GetTypeInfo().Assembly));
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
			xamlServiceProvider2.Add(typeFromHandle4, new XamlTypeResolver(xmlNamespaceResolver2, typeof(LongCodingPage).GetTypeInfo().Assembly));
			xamlServiceProvider2.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(13, 5)));
			DynamicResource dynamicResource = markupExtension2.ProvideValue(xamlServiceProvider2);
			this.SetDynamicResource(VisualElement.BackgroundColorProperty, dynamicResource.Key);
			this.Resources = resourceDictionary;
			rowDefinition.SetValue(RowDefinition.HeightProperty, new GridLengthTypeConverter().ConvertFromInvariantString("*"));
			grid2.GetValue(Grid.RowDefinitionsProperty).Add(rowDefinition);
			listView.SetValue(Grid.RowProperty, 0);
			listView.SetValue(ListView.HasUnevenRowsProperty, true);
			bindingExtension.Path = "Data";
			BindingBase bindingBase = bindingExtension.ProvideValue(null);
			listView.SetBinding(ItemsView<Cell>.ItemsSourceProperty, bindingBase);
			listView.SetValue(ListView.SelectionModeProperty, 0);
			stackLayout.SetValue(StackLayout.OrientationProperty, 0);
			label.SetValue(Label.FontAttributesProperty, new FontAttributesConverter().ConvertFromInvariantString("Bold"));
			dynamicResourceExtension2.Key = "BaseFontSize+++";
			IMarkupExtension<DynamicResource> markupExtension3 = dynamicResourceExtension2;
			XamlServiceProvider xamlServiceProvider3 = new XamlServiceProvider();
			Type typeFromHandle5 = typeof(IProvideValueTarget);
			object[] array3 = new object[0 + 5];
			array3[0] = label;
			array3[1] = stackLayout;
			array3[2] = listView;
			array3[3] = grid2;
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
			xamlServiceProvider3.Add(typeFromHandle6, new XamlTypeResolver(xmlNamespaceResolver3, typeof(LongCodingPage).GetTypeInfo().Assembly));
			xamlServiceProvider3.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(35, 29)));
			DynamicResource dynamicResource2 = markupExtension3.ProvideValue(xamlServiceProvider3);
			label.SetDynamicResource(Label.FontSizeProperty, dynamicResource2.Key);
			bindingExtension2.Path = "Coding.Name";
			BindingBase bindingBase2 = bindingExtension2.ProvideValue(null);
			label.SetBinding(Label.TextProperty, bindingBase2);
			stackLayout.Children.Add(label);
			staticResourceExtension.Key = "EmptyStringToFalseConverter";
			IMarkupExtension markupExtension4 = staticResourceExtension;
			XamlServiceProvider xamlServiceProvider4 = new XamlServiceProvider();
			Type typeFromHandle7 = typeof(IProvideValueTarget);
			object[] array4 = new object[0 + 6];
			array4[0] = bindingExtension3;
			array4[1] = label2;
			array4[2] = stackLayout;
			array4[3] = listView;
			array4[4] = grid2;
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
			xamlServiceProvider4.Add(typeFromHandle8, new XamlTypeResolver(xmlNamespaceResolver4, typeof(LongCodingPage).GetTypeInfo().Assembly));
			xamlServiceProvider4.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(37, 32)));
			object obj6 = markupExtension4.ProvideValue(xamlServiceProvider4);
			bindingExtension3.Converter = obj6;
			bindingExtension3.Path = "Coding.Description";
			BindingBase bindingBase3 = bindingExtension3.ProvideValue(null);
			label2.SetBinding(VisualElement.IsVisibleProperty, bindingBase3);
			bindingExtension4.Path = "Coding.Description";
			BindingBase bindingBase4 = bindingExtension4.ProvideValue(null);
			label2.SetBinding(Label.TextProperty, bindingBase4);
			stackLayout.Children.Add(label2);
			translate2.Text = "coding_Password";
			IMarkupExtension markupExtension5 = translate2;
			XamlServiceProvider xamlServiceProvider5 = new XamlServiceProvider();
			Type typeFromHandle9 = typeof(IProvideValueTarget);
			object[] array5 = new object[0 + 5];
			array5[0] = label3;
			array5[1] = stackLayout;
			array5[2] = listView;
			array5[3] = grid2;
			array5[4] = this;
			object obj7;
			xamlServiceProvider5.Add(typeFromHandle9, obj7 = new SimpleValueTargetProvider(array5, Label.TextProperty, nameScope));
			xamlServiceProvider5.Add(typeof(IReferenceProvider), obj7);
			Type typeFromHandle10 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver5 = new XmlNamespaceResolver();
			xmlNamespaceResolver5.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver5.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver5.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver5.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver5.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver5.Add("settings", "clr-namespace:CarScannerXamarinForms.Settings");
			xamlServiceProvider5.Add(typeFromHandle10, new XamlTypeResolver(xmlNamespaceResolver5, typeof(LongCodingPage).GetTypeInfo().Assembly));
			xamlServiceProvider5.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(39, 32)));
			object obj8 = markupExtension5.ProvideValue(xamlServiceProvider5);
			label3.Text = obj8;
			stackLayout.Children.Add(label3);
			entry.Completed += this.Entry_Completed;
			entry.SetValue(InputView.IsSpellCheckEnabledProperty, false);
			entry.SetValue(Entry.IsTextPredictionEnabledProperty, false);
			bindingExtension5.Mode = 1;
			bindingExtension5.Path = "Coding.Password";
			BindingBase bindingBase5 = bindingExtension5.ProvideValue(null);
			entry.SetBinding(Entry.TextProperty, bindingBase5);
			stackLayout.Children.Add(entry);
			label4.SetValue(Label.FontAttributesProperty, new FontAttributesConverter().ConvertFromInvariantString("Italic"));
			staticResourceExtension2.Key = "EmptyStringToFalseConverter";
			IMarkupExtension markupExtension6 = staticResourceExtension2;
			XamlServiceProvider xamlServiceProvider6 = new XamlServiceProvider();
			Type typeFromHandle11 = typeof(IProvideValueTarget);
			object[] array6 = new object[0 + 6];
			array6[0] = bindingExtension6;
			array6[1] = label4;
			array6[2] = stackLayout;
			array6[3] = listView;
			array6[4] = grid2;
			array6[5] = this;
			object obj9;
			xamlServiceProvider6.Add(typeFromHandle11, obj9 = new SimpleValueTargetProvider(array6, typeof(BindingExtension).GetRuntimeProperty("Converter"), nameScope));
			xamlServiceProvider6.Add(typeof(IReferenceProvider), obj9);
			Type typeFromHandle12 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver6 = new XmlNamespaceResolver();
			xmlNamespaceResolver6.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver6.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver6.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver6.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver6.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver6.Add("settings", "clr-namespace:CarScannerXamarinForms.Settings");
			xamlServiceProvider6.Add(typeFromHandle12, new XamlTypeResolver(xmlNamespaceResolver6, typeof(LongCodingPage).GetTypeInfo().Assembly));
			xamlServiceProvider6.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(46, 56)));
			object obj10 = markupExtension6.ProvideValue(xamlServiceProvider6);
			bindingExtension6.Converter = obj10;
			bindingExtension6.Path = "Coding.PasswordHint";
			BindingBase bindingBase6 = bindingExtension6.ProvideValue(null);
			label4.SetBinding(VisualElement.IsVisibleProperty, bindingBase6);
			translate3.Text = "coding_PasswordHint";
			IMarkupExtension markupExtension7 = translate3;
			XamlServiceProvider xamlServiceProvider7 = new XamlServiceProvider();
			Type typeFromHandle13 = typeof(IProvideValueTarget);
			object[] array7 = new object[0 + 7];
			array7[0] = span;
			array7[1] = formattedString;
			array7[2] = label4;
			array7[3] = stackLayout;
			array7[4] = listView;
			array7[5] = grid2;
			array7[6] = this;
			object obj11;
			xamlServiceProvider7.Add(typeFromHandle13, obj11 = new SimpleValueTargetProvider(array7, Span.TextProperty, nameScope));
			xamlServiceProvider7.Add(typeof(IReferenceProvider), obj11);
			Type typeFromHandle14 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver7 = new XmlNamespaceResolver();
			xmlNamespaceResolver7.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver7.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver7.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver7.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver7.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver7.Add("settings", "clr-namespace:CarScannerXamarinForms.Settings");
			xamlServiceProvider7.Add(typeFromHandle14, new XamlTypeResolver(xmlNamespaceResolver7, typeof(LongCodingPage).GetTypeInfo().Assembly));
			xamlServiceProvider7.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(49, 43)));
			object obj12 = markupExtension7.ProvideValue(xamlServiceProvider7);
			span.Text = obj12;
			formattedString.Spans.Add(span);
			bindingExtension7.Path = "Coding.PasswordHint";
			BindingBase bindingBase7 = bindingExtension7.ProvideValue(null);
			span2.SetBinding(Span.TextProperty, bindingBase7);
			formattedString.Spans.Add(span2);
			label4.SetValue(Label.FormattedTextProperty, formattedString);
			stackLayout.Children.Add(label4);
			bindingExtension8.Mode = 1;
			bindingExtension8.Path = "UseCustomAddress";
			BindingBase bindingBase8 = bindingExtension8.ProvideValue(null);
			labelSwitch.SetBinding(LabelSwitch.IsToggledProperty, bindingBase8);
			translate4.Text = "coding_CustomAddress";
			IMarkupExtension markupExtension8 = translate4;
			XamlServiceProvider xamlServiceProvider8 = new XamlServiceProvider();
			Type typeFromHandle15 = typeof(IProvideValueTarget);
			object[] array8 = new object[0 + 5];
			array8[0] = labelSwitch;
			array8[1] = stackLayout;
			array8[2] = listView;
			array8[3] = grid2;
			array8[4] = this;
			object obj13;
			xamlServiceProvider8.Add(typeFromHandle15, obj13 = new SimpleValueTargetProvider(array8, LabelSwitch.TextProperty, nameScope));
			xamlServiceProvider8.Add(typeof(IReferenceProvider), obj13);
			Type typeFromHandle16 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver8 = new XmlNamespaceResolver();
			xmlNamespaceResolver8.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver8.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver8.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver8.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver8.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver8.Add("settings", "clr-namespace:CarScannerXamarinForms.Settings");
			xamlServiceProvider8.Add(typeFromHandle16, new XamlTypeResolver(xmlNamespaceResolver8, typeof(LongCodingPage).GetTypeInfo().Assembly));
			xamlServiceProvider8.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(58, 29)));
			object obj14 = markupExtension8.ProvideValue(xamlServiceProvider8);
			labelSwitch.Text = obj14;
			stackLayout.Children.Add(labelSwitch);
			entry2.Completed += this.Entry_Completed;
			entry2.SetValue(InputView.IsSpellCheckEnabledProperty, false);
			entry2.SetValue(Entry.IsTextPredictionEnabledProperty, false);
			bindingExtension9.Mode = 2;
			bindingExtension9.Path = "UseCustomAddress";
			BindingBase bindingBase9 = bindingExtension9.ProvideValue(null);
			entry2.SetBinding(VisualElement.IsVisibleProperty, bindingBase9);
			bindingExtension10.Mode = 1;
			bindingExtension10.Path = "CustomAddress";
			BindingBase bindingBase10 = bindingExtension10.ProvideValue(null);
			entry2.SetBinding(Entry.TextProperty, bindingBase10);
			stackLayout.Children.Add(entry2);
			columnDefinition.SetValue(ColumnDefinition.WidthProperty, new GridLengthTypeConverter().ConvertFromInvariantString("*"));
			grid.GetValue(Grid.ColumnDefinitionsProperty).Add(columnDefinition);
			columnDefinition2.SetValue(ColumnDefinition.WidthProperty, new GridLengthTypeConverter().ConvertFromInvariantString("*"));
			grid.GetValue(Grid.ColumnDefinitionsProperty).Add(columnDefinition2);
			button.SetValue(Grid.ColumnProperty, 0);
			button.Clicked += this.BtnUpdateState_Clicked;
			bindingExtension11.Path = "HasCurrentState";
			BindingBase bindingBase11 = bindingExtension11.ProvideValue(null);
			button.SetBinding(VisualElement.IsVisibleProperty, bindingBase11);
			translate5.Text = "Mode06Page_btnRefresh.Content";
			IMarkupExtension markupExtension9 = translate5;
			XamlServiceProvider xamlServiceProvider9 = new XamlServiceProvider();
			Type typeFromHandle17 = typeof(IProvideValueTarget);
			object[] array9 = new object[0 + 6];
			array9[0] = button;
			array9[1] = grid;
			array9[2] = stackLayout;
			array9[3] = listView;
			array9[4] = grid2;
			array9[5] = this;
			object obj15;
			xamlServiceProvider9.Add(typeFromHandle17, obj15 = new SimpleValueTargetProvider(array9, Button.TextProperty, nameScope));
			xamlServiceProvider9.Add(typeof(IReferenceProvider), obj15);
			Type typeFromHandle18 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver9 = new XmlNamespaceResolver();
			xmlNamespaceResolver9.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver9.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver9.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver9.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver9.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver9.Add("settings", "clr-namespace:CarScannerXamarinForms.Settings");
			xamlServiceProvider9.Add(typeFromHandle18, new XamlTypeResolver(xmlNamespaceResolver9, typeof(LongCodingPage).GetTypeInfo().Assembly));
			xamlServiceProvider9.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(77, 33)));
			object obj16 = markupExtension9.ProvideValue(xamlServiceProvider9);
			button.Text = obj16;
			grid.Children.Add(button);
			button2.SetValue(Grid.ColumnProperty, 1);
			button2.Clicked += this.btnSaveState_Clicked;
			translate6.Text = "coding_Apply";
			IMarkupExtension markupExtension10 = translate6;
			XamlServiceProvider xamlServiceProvider10 = new XamlServiceProvider();
			Type typeFromHandle19 = typeof(IProvideValueTarget);
			object[] array10 = new object[0 + 6];
			array10[0] = button2;
			array10[1] = grid;
			array10[2] = stackLayout;
			array10[3] = listView;
			array10[4] = grid2;
			array10[5] = this;
			object obj17;
			xamlServiceProvider10.Add(typeFromHandle19, obj17 = new SimpleValueTargetProvider(array10, Button.TextProperty, nameScope));
			xamlServiceProvider10.Add(typeof(IReferenceProvider), obj17);
			Type typeFromHandle20 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver10 = new XmlNamespaceResolver();
			xmlNamespaceResolver10.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver10.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver10.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver10.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver10.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver10.Add("settings", "clr-namespace:CarScannerXamarinForms.Settings");
			xamlServiceProvider10.Add(typeFromHandle20, new XamlTypeResolver(xmlNamespaceResolver10, typeof(LongCodingPage).GetTypeInfo().Assembly));
			xamlServiceProvider10.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(82, 33)));
			object obj18 = markupExtension10.ProvideValue(xamlServiceProvider10);
			button2.Text = obj18;
			grid.Children.Add(button2);
			stackLayout.Children.Add(grid);
			label5.SetValue(Label.FontAttributesProperty, new FontAttributesConverter().ConvertFromInvariantString("Bold"));
			translate7.Text = "coding_Data";
			IMarkupExtension markupExtension11 = translate7;
			XamlServiceProvider xamlServiceProvider11 = new XamlServiceProvider();
			Type typeFromHandle21 = typeof(IProvideValueTarget);
			object[] array11 = new object[0 + 5];
			array11[0] = label5;
			array11[1] = stackLayout;
			array11[2] = listView;
			array11[3] = grid2;
			array11[4] = this;
			object obj19;
			xamlServiceProvider11.Add(typeFromHandle21, obj19 = new SimpleValueTargetProvider(array11, Label.TextProperty, nameScope));
			xamlServiceProvider11.Add(typeof(IReferenceProvider), obj19);
			Type typeFromHandle22 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver11 = new XmlNamespaceResolver();
			xmlNamespaceResolver11.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver11.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver11.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver11.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver11.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver11.Add("settings", "clr-namespace:CarScannerXamarinForms.Settings");
			xamlServiceProvider11.Add(typeFromHandle22, new XamlTypeResolver(xmlNamespaceResolver11, typeof(LongCodingPage).GetTypeInfo().Assembly));
			xamlServiceProvider11.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(86, 54)));
			object obj20 = markupExtension11.ProvideValue(xamlServiceProvider11);
			label5.Text = obj20;
			stackLayout.Children.Add(label5);
			editor.SetValue(Editor.AutoSizeProperty, 1);
			editor.Completed += this.FullHexEditor_Completed;
			editor.SetValue(InputView.IsSpellCheckEnabledProperty, false);
			editor.SetValue(Editor.IsTextPredictionEnabledProperty, false);
			editor.SetValue(InputView.KeyboardProperty, new KeyboardTypeConverter().ConvertFromInvariantString("Default"));
			bindingExtension12.Mode = 2;
			bindingExtension12.Path = "Data.Hex";
			BindingBase bindingBase12 = bindingExtension12.ProvideValue(null);
			editor.SetBinding(Editor.TextProperty, bindingBase12);
			dynamicResourceExtension3.Key = "TextColor";
			IMarkupExtension<DynamicResource> markupExtension12 = dynamicResourceExtension3;
			XamlServiceProvider xamlServiceProvider12 = new XamlServiceProvider();
			Type typeFromHandle23 = typeof(IProvideValueTarget);
			object[] array12 = new object[0 + 5];
			array12[0] = editor;
			array12[1] = stackLayout;
			array12[2] = listView;
			array12[3] = grid2;
			array12[4] = this;
			object obj21;
			xamlServiceProvider12.Add(typeFromHandle23, obj21 = new SimpleValueTargetProvider(array12, Editor.TextColorProperty, nameScope));
			xamlServiceProvider12.Add(typeof(IReferenceProvider), obj21);
			Type typeFromHandle24 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver12 = new XmlNamespaceResolver();
			xmlNamespaceResolver12.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver12.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver12.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver12.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver12.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver12.Add("settings", "clr-namespace:CarScannerXamarinForms.Settings");
			xamlServiceProvider12.Add(typeFromHandle24, new XamlTypeResolver(xmlNamespaceResolver12, typeof(LongCodingPage).GetTypeInfo().Assembly));
			xamlServiceProvider12.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(95, 29)));
			DynamicResource dynamicResource3 = markupExtension12.ProvideValue(xamlServiceProvider12);
			editor.SetDynamicResource(Editor.TextColorProperty, dynamicResource3.Key);
			editor.Unfocused += this.FullHexEditor_Unfocused;
			stackLayout.Children.Add(editor);
			listView.SetValue(ListView.HeaderProperty, stackLayout);
			IDataTemplate dataTemplate2 = dataTemplate;
			LongCodingPage.<InitializeComponent>_anonXamlCDataTemplate_14 <InitializeComponent>_anonXamlCDataTemplate_ = new LongCodingPage.<InitializeComponent>_anonXamlCDataTemplate_14();
			object[] array13 = new object[0 + 4];
			array13[0] = dataTemplate;
			array13[1] = listView;
			array13[2] = grid2;
			array13[3] = this;
			<InitializeComponent>_anonXamlCDataTemplate_.parentValues = array13;
			<InitializeComponent>_anonXamlCDataTemplate_.root = this;
			dataTemplate2.LoadTemplate = new Func<object>(<InitializeComponent>_anonXamlCDataTemplate_.LoadDataTemplate);
			listView.SetValue(ItemsView<Cell>.ItemTemplateProperty, dataTemplate);
			stackLayout2.SetValue(StackLayout.OrientationProperty, 0);
			label6.SetValue(Label.FontAttributesProperty, new FontAttributesConverter().ConvertFromInvariantString("Bold"));
			translate8.Text = "coding_ECUReset";
			IMarkupExtension markupExtension13 = translate8;
			XamlServiceProvider xamlServiceProvider13 = new XamlServiceProvider();
			Type typeFromHandle25 = typeof(IProvideValueTarget);
			object[] array14 = new object[0 + 5];
			array14[0] = label6;
			array14[1] = stackLayout2;
			array14[2] = listView;
			array14[3] = grid2;
			array14[4] = this;
			object obj22;
			xamlServiceProvider13.Add(typeFromHandle25, obj22 = new SimpleValueTargetProvider(array14, Label.TextProperty, nameScope));
			xamlServiceProvider13.Add(typeof(IReferenceProvider), obj22);
			Type typeFromHandle26 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver13 = new XmlNamespaceResolver();
			xmlNamespaceResolver13.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver13.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver13.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver13.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver13.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver13.Add("settings", "clr-namespace:CarScannerXamarinForms.Settings");
			xamlServiceProvider13.Add(typeFromHandle26, new XamlTypeResolver(xmlNamespaceResolver13, typeof(LongCodingPage).GetTypeInfo().Assembly));
			xamlServiceProvider13.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(255, 54)));
			object obj23 = markupExtension13.ProvideValue(xamlServiceProvider13);
			label6.Text = obj23;
			stackLayout2.Children.Add(label6);
			button3.Clicked += this.btnResetOnOff_Clicked;
			translate9.Text = "coding_KeyOnOffReset";
			IMarkupExtension markupExtension14 = translate9;
			XamlServiceProvider xamlServiceProvider14 = new XamlServiceProvider();
			Type typeFromHandle27 = typeof(IProvideValueTarget);
			object[] array15 = new object[0 + 5];
			array15[0] = button3;
			array15[1] = stackLayout2;
			array15[2] = listView;
			array15[3] = grid2;
			array15[4] = this;
			object obj24;
			xamlServiceProvider14.Add(typeFromHandle27, obj24 = new SimpleValueTargetProvider(array15, Button.TextProperty, nameScope));
			xamlServiceProvider14.Add(typeof(IReferenceProvider), obj24);
			Type typeFromHandle28 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver14 = new XmlNamespaceResolver();
			xmlNamespaceResolver14.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver14.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver14.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver14.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver14.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver14.Add("settings", "clr-namespace:CarScannerXamarinForms.Settings");
			xamlServiceProvider14.Add(typeFromHandle28, new XamlTypeResolver(xmlNamespaceResolver14, typeof(LongCodingPage).GetTypeInfo().Assembly));
			xamlServiceProvider14.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(259, 29)));
			object obj25 = markupExtension14.ProvideValue(xamlServiceProvider14);
			button3.Text = obj25;
			stackLayout2.Children.Add(button3);
			button4.Clicked += this.btnResetSoft_Clicked;
			translate10.Text = "coding_SoftReset";
			IMarkupExtension markupExtension15 = translate10;
			XamlServiceProvider xamlServiceProvider15 = new XamlServiceProvider();
			Type typeFromHandle29 = typeof(IProvideValueTarget);
			object[] array16 = new object[0 + 5];
			array16[0] = button4;
			array16[1] = stackLayout2;
			array16[2] = listView;
			array16[3] = grid2;
			array16[4] = this;
			object obj26;
			xamlServiceProvider15.Add(typeFromHandle29, obj26 = new SimpleValueTargetProvider(array16, Button.TextProperty, nameScope));
			xamlServiceProvider15.Add(typeof(IReferenceProvider), obj26);
			Type typeFromHandle30 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver15 = new XmlNamespaceResolver();
			xmlNamespaceResolver15.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver15.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver15.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver15.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver15.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver15.Add("settings", "clr-namespace:CarScannerXamarinForms.Settings");
			xamlServiceProvider15.Add(typeFromHandle30, new XamlTypeResolver(xmlNamespaceResolver15, typeof(LongCodingPage).GetTypeInfo().Assembly));
			xamlServiceProvider15.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(263, 29)));
			object obj27 = markupExtension15.ProvideValue(xamlServiceProvider15);
			button4.Text = obj27;
			stackLayout2.Children.Add(button4);
			button5.Clicked += this.btnResetHard_Clicked;
			translate11.Text = "coding_HardReset";
			IMarkupExtension markupExtension16 = translate11;
			XamlServiceProvider xamlServiceProvider16 = new XamlServiceProvider();
			Type typeFromHandle31 = typeof(IProvideValueTarget);
			object[] array17 = new object[0 + 5];
			array17[0] = button5;
			array17[1] = stackLayout2;
			array17[2] = listView;
			array17[3] = grid2;
			array17[4] = this;
			object obj28;
			xamlServiceProvider16.Add(typeFromHandle31, obj28 = new SimpleValueTargetProvider(array17, Button.TextProperty, nameScope));
			xamlServiceProvider16.Add(typeof(IReferenceProvider), obj28);
			Type typeFromHandle32 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver16 = new XmlNamespaceResolver();
			xmlNamespaceResolver16.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver16.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver16.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver16.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver16.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver16.Add("settings", "clr-namespace:CarScannerXamarinForms.Settings");
			xamlServiceProvider16.Add(typeFromHandle32, new XamlTypeResolver(xmlNamespaceResolver16, typeof(LongCodingPage).GetTypeInfo().Assembly));
			xamlServiceProvider16.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(267, 29)));
			object obj29 = markupExtension16.ProvideValue(xamlServiceProvider16);
			button5.Text = obj29;
			stackLayout2.Children.Add(button5);
			label7.SetValue(BindableObject.BindingContextProperty, sharedSettings);
			staticResourceExtension3.Key = "VagCodingPlatformToTrueConverter";
			IMarkupExtension markupExtension17 = staticResourceExtension3;
			XamlServiceProvider xamlServiceProvider17 = new XamlServiceProvider();
			Type typeFromHandle33 = typeof(IProvideValueTarget);
			object[] array18 = new object[0 + 6];
			array18[0] = bindingExtension13;
			array18[1] = label7;
			array18[2] = stackLayout2;
			array18[3] = listView;
			array18[4] = grid2;
			array18[5] = this;
			object obj30;
			xamlServiceProvider17.Add(typeFromHandle33, obj30 = new SimpleValueTargetProvider(array18, typeof(BindingExtension).GetRuntimeProperty("Converter"), nameScope));
			xamlServiceProvider17.Add(typeof(IReferenceProvider), obj30);
			Type typeFromHandle34 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver17 = new XmlNamespaceResolver();
			xmlNamespaceResolver17.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver17.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver17.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver17.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver17.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver17.Add("settings", "clr-namespace:CarScannerXamarinForms.Settings");
			xamlServiceProvider17.Add(typeFromHandle34, new XamlTypeResolver(xmlNamespaceResolver17, typeof(LongCodingPage).GetTypeInfo().Assembly));
			xamlServiceProvider17.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(270, 29)));
			object obj31 = markupExtension17.ProvideValue(xamlServiceProvider17);
			bindingExtension13.Converter = obj31;
			bindingExtension13.Path = "CodingLastPlatformSelected";
			BindingBase bindingBase13 = bindingExtension13.ProvideValue(null);
			label7.SetBinding(VisualElement.IsVisibleProperty, bindingBase13);
			IMarkupExtension markupExtension18 = translate12;
			XamlServiceProvider xamlServiceProvider18 = new XamlServiceProvider();
			Type typeFromHandle35 = typeof(IProvideValueTarget);
			object[] array19 = new object[0 + 5];
			array19[0] = label7;
			array19[1] = stackLayout2;
			array19[2] = listView;
			array19[3] = grid2;
			array19[4] = this;
			object obj32;
			xamlServiceProvider18.Add(typeFromHandle35, obj32 = new SimpleValueTargetProvider(array19, Label.TextProperty, nameScope));
			xamlServiceProvider18.Add(typeof(IReferenceProvider), obj32);
			Type typeFromHandle36 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver18 = new XmlNamespaceResolver();
			xmlNamespaceResolver18.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver18.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver18.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver18.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver18.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver18.Add("settings", "clr-namespace:CarScannerXamarinForms.Settings");
			xamlServiceProvider18.Add(typeFromHandle36, new XamlTypeResolver(xmlNamespaceResolver18, typeof(LongCodingPage).GetTypeInfo().Assembly));
			xamlServiceProvider18.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(271, 29)));
			object obj33 = markupExtension18.ProvideValue(xamlServiceProvider18);
			label7.Text = obj33;
			stackLayout2.Children.Add(label7);
			labelSwitch2.SetValue(BindableObject.BindingContextProperty, sharedSettings2);
			bindingExtension14.Mode = 1;
			bindingExtension14.Path = "IgnoreCodingErrors";
			BindingBase bindingBase14 = bindingExtension14.ProvideValue(null);
			labelSwitch2.SetBinding(LabelSwitch.IsToggledProperty, bindingBase14);
			bindingExtension15.Path = "ShowExperimental";
			BindingBase bindingBase15 = bindingExtension15.ProvideValue(null);
			labelSwitch2.SetBinding(VisualElement.IsVisibleProperty, bindingBase15);
			translate13.Text = "coding_ignore_fails";
			IMarkupExtension markupExtension19 = translate13;
			XamlServiceProvider xamlServiceProvider19 = new XamlServiceProvider();
			Type typeFromHandle37 = typeof(IProvideValueTarget);
			object[] array20 = new object[0 + 5];
			array20[0] = labelSwitch2;
			array20[1] = stackLayout2;
			array20[2] = listView;
			array20[3] = grid2;
			array20[4] = this;
			object obj34;
			xamlServiceProvider19.Add(typeFromHandle37, obj34 = new SimpleValueTargetProvider(array20, LabelSwitch.TextProperty, nameScope));
			xamlServiceProvider19.Add(typeof(IReferenceProvider), obj34);
			Type typeFromHandle38 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver19 = new XmlNamespaceResolver();
			xmlNamespaceResolver19.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver19.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver19.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver19.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver19.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver19.Add("settings", "clr-namespace:CarScannerXamarinForms.Settings");
			xamlServiceProvider19.Add(typeFromHandle38, new XamlTypeResolver(xmlNamespaceResolver19, typeof(LongCodingPage).GetTypeInfo().Assembly));
			xamlServiceProvider19.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(276, 29)));
			object obj35 = markupExtension19.ProvideValue(xamlServiceProvider19);
			labelSwitch2.Text = obj35;
			stackLayout2.Children.Add(labelSwitch2);
			listView.SetValue(ListView.FooterProperty, stackLayout2);
			grid2.Children.Add(listView);
			activityFrame.SetValue(Grid.RowProperty, 0);
			activityFrame.SetValue(VisualElement.IsVisibleProperty, new VisualElement.VisibilityConverter().ConvertFromInvariantString("False"));
			grid2.Children.Add(activityFrame);
			this.SetValue(ContentPage.ContentProperty, grid2);
		}

		// Token: 0x06004EAE RID: 20142 RVA: 0x003BC892 File Offset: 0x003BAA92
		[CompilerGenerated]
		private void <btnSaveState_Clicked>b__21_0(string s)
		{
			Device.BeginInvokeOnMainThread(delegate
			{
				this.activityFrame.Text = s;
			});
		}

		// Token: 0x06004EAF RID: 20143 RVA: 0x003BC8B8 File Offset: 0x003BAAB8
		private void __InitComponentRuntime()
		{
			Extensions.LoadFromXaml<LongCodingPage>(this, typeof(LongCodingPage));
			this.lv = NameScopeExtensions.FindByName<ListView>(this, "lv");
			this.entryPassword = NameScopeExtensions.FindByName<Entry>(this, "entryPassword");
			this.switchCustomAddress = NameScopeExtensions.FindByName<LabelSwitch>(this, "switchCustomAddress");
			this.entryCustomAddress = NameScopeExtensions.FindByName<Entry>(this, "entryCustomAddress");
			this.btnUpdateState = NameScopeExtensions.FindByName<Button>(this, "btnUpdateState");
			this.btnSaveNewValue = NameScopeExtensions.FindByName<Button>(this, "btnSaveNewValue");
			this.editorHex = NameScopeExtensions.FindByName<Editor>(this, "editorHex");
			this.btnResetOnOff = NameScopeExtensions.FindByName<Button>(this, "btnResetOnOff");
			this.btnResetSoft = NameScopeExtensions.FindByName<Button>(this, "btnResetSoft");
			this.btnResetHard = NameScopeExtensions.FindByName<Button>(this, "btnResetHard");
			this.activityFrame = NameScopeExtensions.FindByName<ActivityFrame>(this, "activityFrame");
		}

		// Token: 0x04002F3C RID: 12092
		private string defaultAddress = "0600";

		// Token: 0x04002F3D RID: 12093
		[CompilerGenerated]
		private LongCodingModel <Model>k__BackingField;

		// Token: 0x04002F3E RID: 12094
		private bool first_appearing = true;

		// Token: 0x04002F3F RID: 12095
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private ListView lv;

		// Token: 0x04002F40 RID: 12096
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private Entry entryPassword;

		// Token: 0x04002F41 RID: 12097
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private LabelSwitch switchCustomAddress;

		// Token: 0x04002F42 RID: 12098
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private Entry entryCustomAddress;

		// Token: 0x04002F43 RID: 12099
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private Button btnUpdateState;

		// Token: 0x04002F44 RID: 12100
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private Button btnSaveNewValue;

		// Token: 0x04002F45 RID: 12101
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private Editor editorHex;

		// Token: 0x04002F46 RID: 12102
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private Button btnResetOnOff;

		// Token: 0x04002F47 RID: 12103
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private Button btnResetSoft;

		// Token: 0x04002F48 RID: 12104
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private Button btnResetHard;

		// Token: 0x04002F49 RID: 12105
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private ActivityFrame activityFrame;

		// Token: 0x02000954 RID: 2388
		[CompilerGenerated]
		private sealed class <>c__DisplayClass21_0
		{
			// Token: 0x06004EB0 RID: 20144 RVA: 0x00002050 File Offset: 0x00000250
			public <>c__DisplayClass21_0()
			{
			}

			// Token: 0x06004EB1 RID: 20145 RVA: 0x003BC991 File Offset: 0x003BAB91
			internal void <btnSaveState_Clicked>b__1()
			{
				this.<>4__this.activityFrame.Text = this.s;
			}

			// Token: 0x04002F4A RID: 12106
			public string s;

			// Token: 0x04002F4B RID: 12107
			public LongCodingPage <>4__this;
		}

		// Token: 0x02000955 RID: 2389
		[CompilerGenerated]
		private sealed class <>c__DisplayClass5_0
		{
			// Token: 0x06004EB2 RID: 20146 RVA: 0x00002050 File Offset: 0x00000250
			public <>c__DisplayClass5_0()
			{
			}

			// Token: 0x06004EB3 RID: 20147 RVA: 0x003BC9A9 File Offset: 0x003BABA9
			internal void <ResetECU>b__0(OBDRequest request, string data)
			{
				this.semaphoreSlim.Release();
			}

			// Token: 0x04002F4C RID: 12108
			public SemaphoreSlim semaphoreSlim;
		}

		// Token: 0x02000956 RID: 2390
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <BtnUpdateState_Clicked>d__13 : IAsyncStateMachine
		{
			// Token: 0x06004EB4 RID: 20148 RVA: 0x003BC9B8 File Offset: 0x003BABB8
			void IAsyncStateMachine.MoveNext()
			{
				LongCodingPage longCodingPage = this;
				try
				{
					longCodingPage.UpdateState();
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

			// Token: 0x06004EB5 RID: 20149 RVA: 0x003BCA10 File Offset: 0x003BAC10
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x04002F4D RID: 12109
			public int <>1__state;

			// Token: 0x04002F4E RID: 12110
			public AsyncVoidMethodBuilder <>t__builder;

			// Token: 0x04002F4F RID: 12111
			public LongCodingPage <>4__this;
		}

		// Token: 0x02000957 RID: 2391
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <CodingDetailsPage_Appearing>d__11 : IAsyncStateMachine
		{
			// Token: 0x06004EB6 RID: 20150 RVA: 0x003BCA20 File Offset: 0x003BAC20
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				LongCodingPage longCodingPage = this;
				try
				{
					TaskAwaiter taskAwaiter;
					if (num != 0)
					{
						if (!longCodingPage.first_appearing)
						{
							goto IL_0078;
						}
						longCodingPage.first_appearing = false;
						taskAwaiter = longCodingPage.UpdateState().GetAwaiter();
						if (!taskAwaiter.IsCompleted)
						{
							num2 = 0;
							TaskAwaiter taskAwaiter2 = taskAwaiter;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, LongCodingPage.<CodingDetailsPage_Appearing>d__11>(ref taskAwaiter, ref this);
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
					IL_0078:;
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

			// Token: 0x06004EB7 RID: 20151 RVA: 0x003BCAE4 File Offset: 0x003BACE4
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x04002F50 RID: 12112
			public int <>1__state;

			// Token: 0x04002F51 RID: 12113
			public AsyncVoidMethodBuilder <>t__builder;

			// Token: 0x04002F52 RID: 12114
			public LongCodingPage <>4__this;

			// Token: 0x04002F53 RID: 12115
			private TaskAwaiter <>u__1;
		}

		// Token: 0x02000958 RID: 2392
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <ResetECU>d__5 : IAsyncStateMachine
		{
			// Token: 0x06004EB8 RID: 20152 RVA: 0x003BCAF4 File Offset: 0x003BACF4
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				LongCodingPage longCodingPage = this;
				try
				{
					TaskAwaiter taskAwaiter;
					if (num != 0)
					{
						LongCodingPage.<>c__DisplayClass5_0 CS$<>8__locals1 = new LongCodingPage.<>c__DisplayClass5_0();
						longCodingPage.lv.IsEnabled = false;
						longCodingPage.activityFrame.IsVisible = true;
						MQBLongCoding mqblongCoding = (MQBLongCoding)longCodingPage.Model.Coding;
						CS$<>8__locals1.semaphoreSlim = new SemaphoreSlim(0, 1);
						OBDRequest obdrequest = new OBDRequest("11" + reset_value, mqblongCoding.RequestHeader, "ATSP6;ATSH" + mqblongCoding.RequestHeader + ";1003", "", false);
						obdrequest.ResponseReceived += delegate(OBDRequest request, string data)
						{
							CS$<>8__locals1.semaphoreSlim.Release();
						};
						App.OBDReader.ReplaceQueue(new OBDRequest[] { obdrequest });
						taskAwaiter = CS$<>8__locals1.semaphoreSlim.WaitAsync().GetAwaiter();
						if (!taskAwaiter.IsCompleted)
						{
							num2 = 0;
							TaskAwaiter taskAwaiter2 = taskAwaiter;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, LongCodingPage.<ResetECU>d__5>(ref taskAwaiter, ref this);
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
					longCodingPage.lv.IsEnabled = true;
					longCodingPage.activityFrame.IsVisible = false;
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

			// Token: 0x06004EB9 RID: 20153 RVA: 0x003BCC74 File Offset: 0x003BAE74
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x04002F54 RID: 12116
			public int <>1__state;

			// Token: 0x04002F55 RID: 12117
			public AsyncTaskMethodBuilder <>t__builder;

			// Token: 0x04002F56 RID: 12118
			public LongCodingPage <>4__this;

			// Token: 0x04002F57 RID: 12119
			public string reset_value;

			// Token: 0x04002F58 RID: 12120
			private TaskAwaiter <>u__1;
		}

		// Token: 0x02000959 RID: 2393
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <UpdateState>d__12 : IAsyncStateMachine
		{
			// Token: 0x06004EBA RID: 20154 RVA: 0x003BCC84 File Offset: 0x003BAE84
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				LongCodingPage longCodingPage = this;
				try
				{
					TaskAwaiter taskAwaiter3;
					TaskAwaiter taskAwaiter4;
					if (num != 0)
					{
						TaskAwaiter<CodingRequestResult> taskAwaiter5;
						if (num != 1)
						{
							longCodingPage.activityFrame.IsVisible = true;
							longCodingPage.lv.IsEnabled = false;
							if (longCodingPage.Model.UseCustomAddress)
							{
								int num3 = 0;
								if (longCodingPage.Model.CustomAddress.Length == 4 && int.TryParse(longCodingPage.Model.CustomAddress, NumberStyles.HexNumber, CultureInfo.InvariantCulture.NumberFormat, out num3))
								{
									((MQBAdaptationTemplate)longCodingPage.Model.Coding).Address = longCodingPage.Model.CustomAddress;
								}
								else
								{
									taskAwaiter3 = longCodingPage.DisplayAlert("Wrong custom address format!", "Address should be in HEX format.\nCorrect format example: 01FF", "OK").GetAwaiter();
									if (!taskAwaiter3.IsCompleted)
									{
										num2 = 0;
										taskAwaiter4 = taskAwaiter3;
										this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, LongCodingPage.<UpdateState>d__12>(ref taskAwaiter3, ref this);
										return;
									}
									goto IL_0106;
								}
							}
							else
							{
								((MQBAdaptationTemplate)longCodingPage.Model.Coding).Address = longCodingPage.defaultAddress;
							}
							string text = "";
							if (SharedSettings.Current.DeveloperMode)
							{
								text = longCodingPage.entryPassword.Text;
							}
							taskAwaiter5 = longCodingPage.Model.Coding.UpdateCurrentState(text, null).GetAwaiter();
							if (!taskAwaiter5.IsCompleted)
							{
								num2 = 1;
								taskAwaiter2 = taskAwaiter5;
								this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<CodingRequestResult>, LongCodingPage.<UpdateState>d__12>(ref taskAwaiter5, ref this);
								return;
							}
						}
						else
						{
							taskAwaiter5 = taskAwaiter2;
							taskAwaiter2 = default(TaskAwaiter<CodingRequestResult>);
							num2 = -1;
						}
						if (taskAwaiter5.GetResult() == CodingRequestResult.Success)
						{
							longCodingPage.Model.Data = new HexStringObject(longCodingPage.Model.Coding.CurrentState);
						}
						longCodingPage.activityFrame.IsVisible = false;
						longCodingPage.lv.IsEnabled = true;
						goto IL_020A;
					}
					taskAwaiter3 = taskAwaiter4;
					taskAwaiter4 = default(TaskAwaiter);
					num2 = -1;
					IL_0106:
					taskAwaiter3.GetResult();
				}
				catch (Exception ex)
				{
					num2 = -2;
					this.<>t__builder.SetException(ex);
					return;
				}
				IL_020A:
				num2 = -2;
				this.<>t__builder.SetResult();
			}

			// Token: 0x06004EBB RID: 20155 RVA: 0x003BCECC File Offset: 0x003BB0CC
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x04002F59 RID: 12121
			public int <>1__state;

			// Token: 0x04002F5A RID: 12122
			public AsyncTaskMethodBuilder <>t__builder;

			// Token: 0x04002F5B RID: 12123
			public LongCodingPage <>4__this;

			// Token: 0x04002F5C RID: 12124
			private TaskAwaiter <>u__1;

			// Token: 0x04002F5D RID: 12125
			private TaskAwaiter<CodingRequestResult> <>u__2;
		}

		// Token: 0x0200095A RID: 2394
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <btnResetHard_Clicked>d__3 : IAsyncStateMachine
		{
			// Token: 0x06004EBC RID: 20156 RVA: 0x003BCEDC File Offset: 0x003BB0DC
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				LongCodingPage longCodingPage = this;
				try
				{
					TaskAwaiter taskAwaiter;
					if (num != 0)
					{
						taskAwaiter = longCodingPage.ResetECU("01").GetAwaiter();
						if (!taskAwaiter.IsCompleted)
						{
							num2 = 0;
							TaskAwaiter taskAwaiter2 = taskAwaiter;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, LongCodingPage.<btnResetHard_Clicked>d__3>(ref taskAwaiter, ref this);
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

			// Token: 0x06004EBD RID: 20157 RVA: 0x003BCF94 File Offset: 0x003BB194
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x04002F5E RID: 12126
			public int <>1__state;

			// Token: 0x04002F5F RID: 12127
			public AsyncVoidMethodBuilder <>t__builder;

			// Token: 0x04002F60 RID: 12128
			public LongCodingPage <>4__this;

			// Token: 0x04002F61 RID: 12129
			private TaskAwaiter <>u__1;
		}

		// Token: 0x0200095B RID: 2395
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <btnResetOnOff_Clicked>d__2 : IAsyncStateMachine
		{
			// Token: 0x06004EBE RID: 20158 RVA: 0x003BCFA4 File Offset: 0x003BB1A4
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				LongCodingPage longCodingPage = this;
				try
				{
					TaskAwaiter taskAwaiter;
					if (num != 0)
					{
						taskAwaiter = longCodingPage.ResetECU("02").GetAwaiter();
						if (!taskAwaiter.IsCompleted)
						{
							num2 = 0;
							TaskAwaiter taskAwaiter2 = taskAwaiter;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, LongCodingPage.<btnResetOnOff_Clicked>d__2>(ref taskAwaiter, ref this);
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

			// Token: 0x06004EBF RID: 20159 RVA: 0x003BD05C File Offset: 0x003BB25C
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x04002F62 RID: 12130
			public int <>1__state;

			// Token: 0x04002F63 RID: 12131
			public AsyncVoidMethodBuilder <>t__builder;

			// Token: 0x04002F64 RID: 12132
			public LongCodingPage <>4__this;

			// Token: 0x04002F65 RID: 12133
			private TaskAwaiter <>u__1;
		}

		// Token: 0x0200095C RID: 2396
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <btnResetSoft_Clicked>d__4 : IAsyncStateMachine
		{
			// Token: 0x06004EC0 RID: 20160 RVA: 0x003BD06C File Offset: 0x003BB26C
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				LongCodingPage longCodingPage = this;
				try
				{
					TaskAwaiter taskAwaiter;
					if (num != 0)
					{
						taskAwaiter = longCodingPage.ResetECU("03").GetAwaiter();
						if (!taskAwaiter.IsCompleted)
						{
							num2 = 0;
							TaskAwaiter taskAwaiter2 = taskAwaiter;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, LongCodingPage.<btnResetSoft_Clicked>d__4>(ref taskAwaiter, ref this);
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

			// Token: 0x06004EC1 RID: 20161 RVA: 0x003BD124 File Offset: 0x003BB324
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x04002F66 RID: 12134
			public int <>1__state;

			// Token: 0x04002F67 RID: 12135
			public AsyncVoidMethodBuilder <>t__builder;

			// Token: 0x04002F68 RID: 12136
			public LongCodingPage <>4__this;

			// Token: 0x04002F69 RID: 12137
			private TaskAwaiter <>u__1;
		}

		// Token: 0x0200095D RID: 2397
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <btnSaveState_Clicked>d__21 : IAsyncStateMachine
		{
			// Token: 0x06004EC2 RID: 20162 RVA: 0x003BD134 File Offset: 0x003BB334
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				LongCodingPage longCodingPage = this;
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
						goto IL_013B;
					}
					case 2:
						taskAwaiter3 = taskAwaiter2;
						taskAwaiter2 = default(TaskAwaiter<bool>);
						num2 = -1;
						goto IL_01F5;
					case 3:
					{
						TaskAwaiter taskAwaiter5;
						taskAwaiter4 = taskAwaiter5;
						taskAwaiter5 = default(TaskAwaiter);
						num2 = -1;
						goto IL_0263;
					}
					case 4:
					{
						TaskAwaiter taskAwaiter5;
						taskAwaiter4 = taskAwaiter5;
						taskAwaiter5 = default(TaskAwaiter);
						num2 = -1;
						goto IL_0343;
					}
					case 5:
					{
						TaskAwaiter<CodingRequestResult> taskAwaiter7;
						taskAwaiter6 = taskAwaiter7;
						taskAwaiter7 = default(TaskAwaiter<CodingRequestResult>);
						num2 = -1;
						goto IL_0432;
					}
					case 6:
					{
						TaskAwaiter taskAwaiter5;
						taskAwaiter4 = taskAwaiter5;
						taskAwaiter5 = default(TaskAwaiter);
						num2 = -1;
						goto IL_04C1;
					}
					case 7:
					{
						TaskAwaiter taskAwaiter5;
						taskAwaiter4 = taskAwaiter5;
						taskAwaiter5 = default(TaskAwaiter);
						num2 = -1;
						goto IL_051C;
					}
					default:
						if (longCodingPage.Model.Coding.RequiresPro && !SharedSettings.Current.AdsProductPurchased)
						{
							taskAwaiter3 = longCodingPage.DisplayAlert("Car Scanner Pro", Translate.GetString("FeatureLocked"), "OK", Translate.GetString("btnCancel.Content")).GetAwaiter();
							if (!taskAwaiter3.IsCompleted)
							{
								num2 = 0;
								taskAwaiter2 = taskAwaiter3;
								this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<bool>, LongCodingPage.<btnSaveState_Clicked>d__21>(ref taskAwaiter3, ref this);
								return;
							}
						}
						else if (SharedSettings.Current.CodingsCounter >= 3 && !SharedSettings.Current.AdsProductPurchased)
						{
							taskAwaiter3 = longCodingPage.DisplayAlert("Car Scanner Pro", string.Format(Translate.GetString("coding_FreeCodingsFinished"), SharedSettings.Current.CodingsCounter, 3), "OK", Translate.GetString("btnCancel.Content")).GetAwaiter();
							if (!taskAwaiter3.IsCompleted)
							{
								num2 = 2;
								taskAwaiter2 = taskAwaiter3;
								this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<bool>, LongCodingPage.<btnSaveState_Clicked>d__21>(ref taskAwaiter3, ref this);
								return;
							}
							goto IL_01F5;
						}
						else
						{
							if (longCodingPage.Model.UseCustomAddress)
							{
								int num3 = 0;
								if (longCodingPage.Model.CustomAddress.Length == 4 && int.TryParse(longCodingPage.Model.CustomAddress, NumberStyles.HexNumber, CultureInfo.InvariantCulture.NumberFormat, out num3))
								{
									((MQBAdaptationTemplate)longCodingPage.Model.Coding).Address = longCodingPage.Model.CustomAddress;
								}
								else
								{
									taskAwaiter4 = longCodingPage.DisplayAlert("Wrong custom address format!", "Address should be in HEX format.\nCorrect format example: 01FF", "OK").GetAwaiter();
									if (!taskAwaiter4.IsCompleted)
									{
										num2 = 4;
										TaskAwaiter taskAwaiter5 = taskAwaiter4;
										this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, LongCodingPage.<btnSaveState_Clicked>d__21>(ref taskAwaiter4, ref this);
										return;
									}
									goto IL_0343;
								}
							}
							else
							{
								((MQBAdaptationTemplate)longCodingPage.Model.Coding).Address = longCodingPage.defaultAddress;
							}
							longCodingPage.activityFrame.IsVisible = true;
							longCodingPage.lv.IsEnabled = false;
							Progress<string> progress = new Progress<string>(delegate(string s)
							{
								Device.BeginInvokeOnMainThread(new Action(new LongCodingPage.<>c__DisplayClass21_0
								{
									<>4__this = longCodingPage,
									s = s
								}.<btnSaveState_Clicked>b__1));
							});
							taskAwaiter6 = longCodingPage.Model.Coding.Execute(longCodingPage.entryPassword.Text, longCodingPage.Model.Data.Hex.Replace(" ", ""), longCodingPage.Model.Data.Hex, progress, null, false).GetAwaiter();
							if (!taskAwaiter6.IsCompleted)
							{
								num2 = 5;
								TaskAwaiter<CodingRequestResult> taskAwaiter7 = taskAwaiter6;
								this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<CodingRequestResult>, LongCodingPage.<btnSaveState_Clicked>d__21>(ref taskAwaiter6, ref this);
								return;
							}
							goto IL_0432;
						}
						break;
					}
					if (!taskAwaiter3.GetResult())
					{
						goto IL_0142;
					}
					Page inAppPage = InAppManager.GetInAppPage();
					taskAwaiter4 = longCodingPage.Navigation.PushAsync(inAppPage).GetAwaiter();
					if (!taskAwaiter4.IsCompleted)
					{
						num2 = 1;
						TaskAwaiter taskAwaiter5 = taskAwaiter4;
						this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, LongCodingPage.<btnSaveState_Clicked>d__21>(ref taskAwaiter4, ref this);
						return;
					}
					IL_013B:
					taskAwaiter4.GetResult();
					IL_0142:
					goto IL_0566;
					IL_01F5:
					if (!taskAwaiter3.GetResult())
					{
						goto IL_026A;
					}
					Page inAppPage2 = InAppManager.GetInAppPage();
					taskAwaiter4 = longCodingPage.Navigation.PushAsync(inAppPage2).GetAwaiter();
					if (!taskAwaiter4.IsCompleted)
					{
						num2 = 3;
						TaskAwaiter taskAwaiter5 = taskAwaiter4;
						this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, LongCodingPage.<btnSaveState_Clicked>d__21>(ref taskAwaiter4, ref this);
						return;
					}
					IL_0263:
					taskAwaiter4.GetResult();
					IL_026A:
					goto IL_0566;
					IL_0343:
					taskAwaiter4.GetResult();
					goto IL_0566;
					IL_0432:
					CodingRequestResult result = taskAwaiter6.GetResult();
					if (result == CodingRequestResult.Success)
					{
						SharedSettings sharedSettings = SharedSettings.Current;
						int codingsCounter = sharedSettings.CodingsCounter;
						sharedSettings.CodingsCounter = codingsCounter + 1;
						goto IL_04C8;
					}
					taskAwaiter4 = longCodingPage.DisplayAlert(Translate.GetString("coding_Error"), MQBAdaptationTemplate.CodingRequestResultToString(result), "OK").GetAwaiter();
					if (!taskAwaiter4.IsCompleted)
					{
						num2 = 6;
						TaskAwaiter taskAwaiter5 = taskAwaiter4;
						this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, LongCodingPage.<btnSaveState_Clicked>d__21>(ref taskAwaiter4, ref this);
						return;
					}
					IL_04C1:
					taskAwaiter4.GetResult();
					IL_04C8:
					taskAwaiter4 = longCodingPage.UpdateState().GetAwaiter();
					if (!taskAwaiter4.IsCompleted)
					{
						num2 = 7;
						TaskAwaiter taskAwaiter5 = taskAwaiter4;
						this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, LongCodingPage.<btnSaveState_Clicked>d__21>(ref taskAwaiter4, ref this);
						return;
					}
					IL_051C:
					taskAwaiter4.GetResult();
					longCodingPage.activityFrame.IsVisible = false;
					longCodingPage.activityFrame.Text = ActivityFrame.PLEASE_WAIT_TEXT;
					longCodingPage.lv.IsEnabled = true;
				}
				catch (Exception ex)
				{
					num2 = -2;
					this.<>t__builder.SetException(ex);
					return;
				}
				IL_0566:
				num2 = -2;
				this.<>t__builder.SetResult();
			}

			// Token: 0x06004EC3 RID: 20163 RVA: 0x003BD6D8 File Offset: 0x003BB8D8
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x04002F6A RID: 12138
			public int <>1__state;

			// Token: 0x04002F6B RID: 12139
			public AsyncVoidMethodBuilder <>t__builder;

			// Token: 0x04002F6C RID: 12140
			public LongCodingPage <>4__this;

			// Token: 0x04002F6D RID: 12141
			private TaskAwaiter<bool> <>u__1;

			// Token: 0x04002F6E RID: 12142
			private TaskAwaiter <>u__2;

			// Token: 0x04002F6F RID: 12143
			private TaskAwaiter<CodingRequestResult> <>u__3;
		}

		// Token: 0x0200095E RID: 2398
		[CompilerGenerated]
		private sealed class <InitializeComponent>_anonXamlCDataTemplate_14
		{
			// Token: 0x06004EC4 RID: 20164 RVA: 0x003BD6E8 File Offset: 0x003BB8E8
			public <InitializeComponent>_anonXamlCDataTemplate_14()
			{
			}

			// Token: 0x06004EC5 RID: 20165 RVA: 0x003BD6FC File Offset: 0x003BB8FC
			internal object LoadDataTemplate()
			{
				Translate translate;
				VisualDiagnostics.RegisterSourceInfo(translate = new Translate(), new Uri("Coding\\Pages\\LongCodingPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 109, 51);
				Span span;
				VisualDiagnostics.RegisterSourceInfo(span = new Span(), new Uri("Coding\\Pages\\LongCodingPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 109, 46);
				Span span2;
				VisualDiagnostics.RegisterSourceInfo(span2 = new Span(), new Uri("Coding\\Pages\\LongCodingPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 110, 46);
				BindingExtension bindingExtension;
				VisualDiagnostics.RegisterSourceInfo(bindingExtension = new BindingExtension(), new Uri("Coding\\Pages\\LongCodingPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 111, 51);
				Span span3;
				VisualDiagnostics.RegisterSourceInfo(span3 = new Span(), new Uri("Coding\\Pages\\LongCodingPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 111, 46);
				Span span4;
				VisualDiagnostics.RegisterSourceInfo(span4 = new Span(), new Uri("Coding\\Pages\\LongCodingPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 112, 46);
				FormattedString formattedString;
				VisualDiagnostics.RegisterSourceInfo(formattedString = new FormattedString(), new Uri("Coding\\Pages\\LongCodingPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 108, 42);
				Label label;
				VisualDiagnostics.RegisterSourceInfo(label = new Label(), new Uri("Coding\\Pages\\LongCodingPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 106, 34);
				Label label2;
				VisualDiagnostics.RegisterSourceInfo(label2 = new Label(), new Uri("Coding\\Pages\\LongCodingPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 117, 38);
				BindingExtension bindingExtension2;
				VisualDiagnostics.RegisterSourceInfo(bindingExtension2 = new BindingExtension(), new Uri("Coding\\Pages\\LongCodingPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 127, 41);
				Entry entry;
				VisualDiagnostics.RegisterSourceInfo(entry = new Entry(), new Uri("Coding\\Pages\\LongCodingPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 121, 38);
				StackLayout stackLayout;
				VisualDiagnostics.RegisterSourceInfo(stackLayout = new StackLayout(), new Uri("Coding\\Pages\\LongCodingPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 116, 34);
				Label label3;
				VisualDiagnostics.RegisterSourceInfo(label3 = new Label(), new Uri("Coding\\Pages\\LongCodingPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 133, 38);
				BindingExtension bindingExtension3;
				VisualDiagnostics.RegisterSourceInfo(bindingExtension3 = new BindingExtension(), new Uri("Coding\\Pages\\LongCodingPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 143, 41);
				Entry entry2;
				VisualDiagnostics.RegisterSourceInfo(entry2 = new Entry(), new Uri("Coding\\Pages\\LongCodingPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 137, 38);
				StackLayout stackLayout2;
				VisualDiagnostics.RegisterSourceInfo(stackLayout2 = new StackLayout(), new Uri("Coding\\Pages\\LongCodingPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 132, 34);
				ColumnDefinition columnDefinition;
				VisualDiagnostics.RegisterSourceInfo(columnDefinition = new ColumnDefinition(), new Uri("Coding\\Pages\\LongCodingPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 151, 46);
				ColumnDefinition columnDefinition2;
				VisualDiagnostics.RegisterSourceInfo(columnDefinition2 = new ColumnDefinition(), new Uri("Coding\\Pages\\LongCodingPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 152, 46);
				ColumnDefinition columnDefinition3;
				VisualDiagnostics.RegisterSourceInfo(columnDefinition3 = new ColumnDefinition(), new Uri("Coding\\Pages\\LongCodingPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 153, 46);
				ColumnDefinition columnDefinition4;
				VisualDiagnostics.RegisterSourceInfo(columnDefinition4 = new ColumnDefinition(), new Uri("Coding\\Pages\\LongCodingPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 154, 46);
				ColumnDefinition columnDefinition5;
				VisualDiagnostics.RegisterSourceInfo(columnDefinition5 = new ColumnDefinition(), new Uri("Coding\\Pages\\LongCodingPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 155, 46);
				ColumnDefinition columnDefinition6;
				VisualDiagnostics.RegisterSourceInfo(columnDefinition6 = new ColumnDefinition(), new Uri("Coding\\Pages\\LongCodingPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 156, 46);
				ColumnDefinition columnDefinition7;
				VisualDiagnostics.RegisterSourceInfo(columnDefinition7 = new ColumnDefinition(), new Uri("Coding\\Pages\\LongCodingPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 157, 46);
				ColumnDefinition columnDefinition8;
				VisualDiagnostics.RegisterSourceInfo(columnDefinition8 = new ColumnDefinition(), new Uri("Coding\\Pages\\LongCodingPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 158, 46);
				ColumnDefinition columnDefinition9;
				VisualDiagnostics.RegisterSourceInfo(columnDefinition9 = new ColumnDefinition(), new Uri("Coding\\Pages\\LongCodingPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 159, 46);
				RowDefinition rowDefinition;
				VisualDiagnostics.RegisterSourceInfo(rowDefinition = new RowDefinition(), new Uri("Coding\\Pages\\LongCodingPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 162, 46);
				RowDefinition rowDefinition2;
				VisualDiagnostics.RegisterSourceInfo(rowDefinition2 = new RowDefinition(), new Uri("Coding\\Pages\\LongCodingPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 163, 46);
				Label label4;
				VisualDiagnostics.RegisterSourceInfo(label4 = new Label(), new Uri("Coding\\Pages\\LongCodingPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 165, 42);
				Label label5;
				VisualDiagnostics.RegisterSourceInfo(label5 = new Label(), new Uri("Coding\\Pages\\LongCodingPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 170, 42);
				Label label6;
				VisualDiagnostics.RegisterSourceInfo(label6 = new Label(), new Uri("Coding\\Pages\\LongCodingPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 175, 42);
				Label label7;
				VisualDiagnostics.RegisterSourceInfo(label7 = new Label(), new Uri("Coding\\Pages\\LongCodingPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 180, 42);
				Label label8;
				VisualDiagnostics.RegisterSourceInfo(label8 = new Label(), new Uri("Coding\\Pages\\LongCodingPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 185, 42);
				Label label9;
				VisualDiagnostics.RegisterSourceInfo(label9 = new Label(), new Uri("Coding\\Pages\\LongCodingPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 190, 42);
				Label label10;
				VisualDiagnostics.RegisterSourceInfo(label10 = new Label(), new Uri("Coding\\Pages\\LongCodingPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 195, 42);
				Label label11;
				VisualDiagnostics.RegisterSourceInfo(label11 = new Label(), new Uri("Coding\\Pages\\LongCodingPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 200, 42);
				BindingExtension bindingExtension4;
				VisualDiagnostics.RegisterSourceInfo(bindingExtension4 = new BindingExtension(), new Uri("Coding\\Pages\\LongCodingPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 209, 45);
				CheckBox checkBox;
				VisualDiagnostics.RegisterSourceInfo(checkBox = new CheckBox(), new Uri("Coding\\Pages\\LongCodingPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 205, 42);
				BindingExtension bindingExtension5;
				VisualDiagnostics.RegisterSourceInfo(bindingExtension5 = new BindingExtension(), new Uri("Coding\\Pages\\LongCodingPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 214, 45);
				CheckBox checkBox2;
				VisualDiagnostics.RegisterSourceInfo(checkBox2 = new CheckBox(), new Uri("Coding\\Pages\\LongCodingPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 210, 42);
				BindingExtension bindingExtension6;
				VisualDiagnostics.RegisterSourceInfo(bindingExtension6 = new BindingExtension(), new Uri("Coding\\Pages\\LongCodingPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 219, 45);
				CheckBox checkBox3;
				VisualDiagnostics.RegisterSourceInfo(checkBox3 = new CheckBox(), new Uri("Coding\\Pages\\LongCodingPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 215, 42);
				BindingExtension bindingExtension7;
				VisualDiagnostics.RegisterSourceInfo(bindingExtension7 = new BindingExtension(), new Uri("Coding\\Pages\\LongCodingPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 224, 45);
				CheckBox checkBox4;
				VisualDiagnostics.RegisterSourceInfo(checkBox4 = new CheckBox(), new Uri("Coding\\Pages\\LongCodingPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 220, 42);
				BindingExtension bindingExtension8;
				VisualDiagnostics.RegisterSourceInfo(bindingExtension8 = new BindingExtension(), new Uri("Coding\\Pages\\LongCodingPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 229, 45);
				CheckBox checkBox5;
				VisualDiagnostics.RegisterSourceInfo(checkBox5 = new CheckBox(), new Uri("Coding\\Pages\\LongCodingPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 225, 42);
				BindingExtension bindingExtension9;
				VisualDiagnostics.RegisterSourceInfo(bindingExtension9 = new BindingExtension(), new Uri("Coding\\Pages\\LongCodingPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 234, 45);
				CheckBox checkBox6;
				VisualDiagnostics.RegisterSourceInfo(checkBox6 = new CheckBox(), new Uri("Coding\\Pages\\LongCodingPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 230, 42);
				BindingExtension bindingExtension10;
				VisualDiagnostics.RegisterSourceInfo(bindingExtension10 = new BindingExtension(), new Uri("Coding\\Pages\\LongCodingPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 239, 45);
				CheckBox checkBox7;
				VisualDiagnostics.RegisterSourceInfo(checkBox7 = new CheckBox(), new Uri("Coding\\Pages\\LongCodingPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 235, 42);
				BindingExtension bindingExtension11;
				VisualDiagnostics.RegisterSourceInfo(bindingExtension11 = new BindingExtension(), new Uri("Coding\\Pages\\LongCodingPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 244, 45);
				CheckBox checkBox8;
				VisualDiagnostics.RegisterSourceInfo(checkBox8 = new CheckBox(), new Uri("Coding\\Pages\\LongCodingPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 240, 42);
				Grid grid;
				VisualDiagnostics.RegisterSourceInfo(grid = new Grid(), new Uri("Coding\\Pages\\LongCodingPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 149, 38);
				ScrollView scrollView;
				VisualDiagnostics.RegisterSourceInfo(scrollView = new ScrollView(), new Uri("Coding\\Pages\\LongCodingPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 148, 34);
				StackLayout stackLayout3;
				VisualDiagnostics.RegisterSourceInfo(stackLayout3 = new StackLayout(), new Uri("Coding\\Pages\\LongCodingPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 105, 30);
				ViewCell viewCell;
				VisualDiagnostics.RegisterSourceInfo(viewCell = new ViewCell(), new Uri("Coding\\Pages\\LongCodingPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 104, 26);
				NameScope nameScope = new NameScope();
				NameScope.SetNameScope(viewCell, nameScope);
				stackLayout3.SetValue(StackLayout.OrientationProperty, 0);
				stackLayout3.SetValue(StackLayout.SpacingProperty, 1.0);
				label.SetValue(Grid.ColumnProperty, 0);
				label.SetValue(Label.FontAttributesProperty, new FontAttributesConverter().ConvertFromInvariantString("Bold"));
				translate.Text = "coding_Byte";
				IMarkupExtension markupExtension = translate;
				XamlServiceProvider xamlServiceProvider = new XamlServiceProvider();
				Type typeFromHandle = typeof(IProvideValueTarget);
				int num;
				object[] array = new object[(num = this.parentValues.Length) + 5];
				Array.Copy(this.parentValues, 0, array, 5, num);
				object[] array2 = array;
				array2[0] = span;
				array2[1] = formattedString;
				array2[2] = label;
				array2[3] = stackLayout3;
				array2[4] = viewCell;
				object obj;
				xamlServiceProvider.Add(typeFromHandle, obj = new SimpleValueTargetProvider(array2, Span.TextProperty, nameScope));
				xamlServiceProvider.Add(typeof(IReferenceProvider), obj);
				Type typeFromHandle2 = typeof(IXamlTypeResolver);
				XmlNamespaceResolver xmlNamespaceResolver = new XmlNamespaceResolver();
				xmlNamespaceResolver.Add("", "http://xamarin.com/schemas/2014/forms");
				xmlNamespaceResolver.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
				xmlNamespaceResolver.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
				xmlNamespaceResolver.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
				xmlNamespaceResolver.Add("local", "clr-namespace:CarScannerXamarinForms");
				xmlNamespaceResolver.Add("settings", "clr-namespace:CarScannerXamarinForms.Settings");
				xamlServiceProvider.Add(typeFromHandle2, new XamlTypeResolver(xmlNamespaceResolver, typeof(LongCodingPage.<InitializeComponent>_anonXamlCDataTemplate_14).GetTypeInfo().Assembly));
				xamlServiceProvider.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(109, 51)));
				object obj2 = markupExtension.ProvideValue(xamlServiceProvider);
				span.Text = obj2;
				formattedString.Spans.Add(span);
				span2.SetValue(Span.TextProperty, " ");
				formattedString.Spans.Add(span2);
				bindingExtension.Path = "ByteIdx";
				BindingBase bindingBase = bindingExtension.ProvideValue(null);
				span3.SetBinding(Span.TextProperty, bindingBase);
				formattedString.Spans.Add(span3);
				span4.SetValue(Span.TextProperty, ":");
				formattedString.Spans.Add(span4);
				label.SetValue(Label.FormattedTextProperty, formattedString);
				stackLayout3.Children.Add(label);
				stackLayout.SetValue(StackLayout.OrientationProperty, 1);
				label2.SetValue(Label.TextProperty, "HEX:");
				label2.SetValue(View.VerticalOptionsProperty, LayoutOptions.Center);
				label2.SetValue(Label.VerticalTextAlignmentProperty, new TextAlignmentConverter().ConvertFromInvariantString("Center"));
				stackLayout.Children.Add(label2);
				entry.SetValue(Grid.ColumnProperty, 1);
				entry.Completed += this.root.HexEntry_Completed;
				entry.SetValue(View.HorizontalOptionsProperty, LayoutOptions.Center);
				entry.SetValue(InputView.IsSpellCheckEnabledProperty, false);
				entry.SetValue(Entry.IsTextPredictionEnabledProperty, false);
				bindingExtension2.Mode = 2;
				bindingExtension2.Path = "Hex";
				BindingBase bindingBase2 = bindingExtension2.ProvideValue(null);
				entry.SetBinding(Entry.TextProperty, bindingBase2);
				entry.Unfocused += this.root.HexEntry_Unfocused;
				entry.SetValue(View.VerticalOptionsProperty, LayoutOptions.Center);
				entry.SetValue(Entry.VerticalTextAlignmentProperty, new TextAlignmentConverter().ConvertFromInvariantString("Center"));
				stackLayout.Children.Add(entry);
				stackLayout3.Children.Add(stackLayout);
				stackLayout2.SetValue(StackLayout.OrientationProperty, 1);
				label3.SetValue(Label.TextProperty, "BIN:");
				label3.SetValue(View.VerticalOptionsProperty, LayoutOptions.Center);
				label3.SetValue(Label.VerticalTextAlignmentProperty, new TextAlignmentConverter().ConvertFromInvariantString("Center"));
				stackLayout2.Children.Add(label3);
				entry2.SetValue(Grid.ColumnProperty, 1);
				entry2.Completed += this.root.BinEntry_Completed;
				entry2.SetValue(View.HorizontalOptionsProperty, LayoutOptions.Center);
				entry2.SetValue(InputView.IsSpellCheckEnabledProperty, false);
				entry2.SetValue(Entry.IsTextPredictionEnabledProperty, false);
				bindingExtension3.Mode = 2;
				bindingExtension3.Path = "Bin";
				BindingBase bindingBase3 = bindingExtension3.ProvideValue(null);
				entry2.SetBinding(Entry.TextProperty, bindingBase3);
				entry2.Unfocused += this.root.BinEntry_Unfocused;
				entry2.SetValue(View.VerticalOptionsProperty, LayoutOptions.Center);
				entry2.SetValue(Entry.VerticalTextAlignmentProperty, new TextAlignmentConverter().ConvertFromInvariantString("Center"));
				stackLayout2.Children.Add(entry2);
				stackLayout3.Children.Add(stackLayout2);
				scrollView.SetValue(ScrollView.OrientationProperty, 1);
				columnDefinition.SetValue(ColumnDefinition.WidthProperty, new GridLengthTypeConverter().ConvertFromInvariantString("auto"));
				grid.GetValue(Grid.ColumnDefinitionsProperty).Add(columnDefinition);
				columnDefinition2.SetValue(ColumnDefinition.WidthProperty, new GridLengthTypeConverter().ConvertFromInvariantString("auto"));
				grid.GetValue(Grid.ColumnDefinitionsProperty).Add(columnDefinition2);
				columnDefinition3.SetValue(ColumnDefinition.WidthProperty, new GridLengthTypeConverter().ConvertFromInvariantString("auto"));
				grid.GetValue(Grid.ColumnDefinitionsProperty).Add(columnDefinition3);
				columnDefinition4.SetValue(ColumnDefinition.WidthProperty, new GridLengthTypeConverter().ConvertFromInvariantString("auto"));
				grid.GetValue(Grid.ColumnDefinitionsProperty).Add(columnDefinition4);
				columnDefinition5.SetValue(ColumnDefinition.WidthProperty, new GridLengthTypeConverter().ConvertFromInvariantString("auto"));
				grid.GetValue(Grid.ColumnDefinitionsProperty).Add(columnDefinition5);
				columnDefinition6.SetValue(ColumnDefinition.WidthProperty, new GridLengthTypeConverter().ConvertFromInvariantString("auto"));
				grid.GetValue(Grid.ColumnDefinitionsProperty).Add(columnDefinition6);
				columnDefinition7.SetValue(ColumnDefinition.WidthProperty, new GridLengthTypeConverter().ConvertFromInvariantString("auto"));
				grid.GetValue(Grid.ColumnDefinitionsProperty).Add(columnDefinition7);
				columnDefinition8.SetValue(ColumnDefinition.WidthProperty, new GridLengthTypeConverter().ConvertFromInvariantString("auto"));
				grid.GetValue(Grid.ColumnDefinitionsProperty).Add(columnDefinition8);
				columnDefinition9.SetValue(ColumnDefinition.WidthProperty, new GridLengthTypeConverter().ConvertFromInvariantString("*"));
				grid.GetValue(Grid.ColumnDefinitionsProperty).Add(columnDefinition9);
				rowDefinition.SetValue(RowDefinition.HeightProperty, new GridLengthTypeConverter().ConvertFromInvariantString("auto"));
				grid.GetValue(Grid.RowDefinitionsProperty).Add(rowDefinition);
				rowDefinition2.SetValue(RowDefinition.HeightProperty, new GridLengthTypeConverter().ConvertFromInvariantString("auto"));
				grid.GetValue(Grid.RowDefinitionsProperty).Add(rowDefinition2);
				label4.SetValue(Grid.RowProperty, 0);
				label4.SetValue(Grid.ColumnProperty, 0);
				label4.SetValue(View.HorizontalOptionsProperty, LayoutOptions.Center);
				label4.SetValue(Label.TextProperty, "7");
				grid.Children.Add(label4);
				label5.SetValue(Grid.RowProperty, 0);
				label5.SetValue(Grid.ColumnProperty, 1);
				label5.SetValue(View.HorizontalOptionsProperty, LayoutOptions.Center);
				label5.SetValue(Label.TextProperty, "6");
				grid.Children.Add(label5);
				label6.SetValue(Grid.RowProperty, 0);
				label6.SetValue(Grid.ColumnProperty, 2);
				label6.SetValue(View.HorizontalOptionsProperty, LayoutOptions.Center);
				label6.SetValue(Label.TextProperty, "5");
				grid.Children.Add(label6);
				label7.SetValue(Grid.RowProperty, 0);
				label7.SetValue(Grid.ColumnProperty, 3);
				label7.SetValue(View.HorizontalOptionsProperty, LayoutOptions.Center);
				label7.SetValue(Label.TextProperty, "4");
				grid.Children.Add(label7);
				label8.SetValue(Grid.RowProperty, 0);
				label8.SetValue(Grid.ColumnProperty, 4);
				label8.SetValue(View.HorizontalOptionsProperty, LayoutOptions.Center);
				label8.SetValue(Label.TextProperty, "3");
				grid.Children.Add(label8);
				label9.SetValue(Grid.RowProperty, 0);
				label9.SetValue(Grid.ColumnProperty, 5);
				label9.SetValue(View.HorizontalOptionsProperty, LayoutOptions.Center);
				label9.SetValue(Label.TextProperty, "2");
				grid.Children.Add(label9);
				label10.SetValue(Grid.RowProperty, 0);
				label10.SetValue(Grid.ColumnProperty, 6);
				label10.SetValue(View.HorizontalOptionsProperty, LayoutOptions.Center);
				label10.SetValue(Label.TextProperty, "1");
				grid.Children.Add(label10);
				label11.SetValue(Grid.RowProperty, 0);
				label11.SetValue(Grid.ColumnProperty, 7);
				label11.SetValue(View.HorizontalOptionsProperty, LayoutOptions.Center);
				label11.SetValue(Label.TextProperty, "0");
				grid.Children.Add(label11);
				checkBox.SetValue(Grid.RowProperty, 1);
				checkBox.SetValue(Grid.ColumnProperty, 0);
				checkBox.SetValue(View.HorizontalOptionsProperty, LayoutOptions.Center);
				bindingExtension4.Mode = 1;
				bindingExtension4.Path = "Bit7";
				BindingBase bindingBase4 = bindingExtension4.ProvideValue(null);
				checkBox.SetBinding(CheckBox.IsCheckedProperty, bindingBase4);
				grid.Children.Add(checkBox);
				checkBox2.SetValue(Grid.RowProperty, 1);
				checkBox2.SetValue(Grid.ColumnProperty, 1);
				checkBox2.SetValue(View.HorizontalOptionsProperty, LayoutOptions.Center);
				bindingExtension5.Mode = 1;
				bindingExtension5.Path = "Bit6";
				BindingBase bindingBase5 = bindingExtension5.ProvideValue(null);
				checkBox2.SetBinding(CheckBox.IsCheckedProperty, bindingBase5);
				grid.Children.Add(checkBox2);
				checkBox3.SetValue(Grid.RowProperty, 1);
				checkBox3.SetValue(Grid.ColumnProperty, 2);
				checkBox3.SetValue(View.HorizontalOptionsProperty, LayoutOptions.Center);
				bindingExtension6.Mode = 1;
				bindingExtension6.Path = "Bit5";
				BindingBase bindingBase6 = bindingExtension6.ProvideValue(null);
				checkBox3.SetBinding(CheckBox.IsCheckedProperty, bindingBase6);
				grid.Children.Add(checkBox3);
				checkBox4.SetValue(Grid.RowProperty, 1);
				checkBox4.SetValue(Grid.ColumnProperty, 3);
				checkBox4.SetValue(View.HorizontalOptionsProperty, LayoutOptions.Center);
				bindingExtension7.Mode = 1;
				bindingExtension7.Path = "Bit4";
				BindingBase bindingBase7 = bindingExtension7.ProvideValue(null);
				checkBox4.SetBinding(CheckBox.IsCheckedProperty, bindingBase7);
				grid.Children.Add(checkBox4);
				checkBox5.SetValue(Grid.RowProperty, 1);
				checkBox5.SetValue(Grid.ColumnProperty, 4);
				checkBox5.SetValue(View.HorizontalOptionsProperty, LayoutOptions.Center);
				bindingExtension8.Mode = 1;
				bindingExtension8.Path = "Bit3";
				BindingBase bindingBase8 = bindingExtension8.ProvideValue(null);
				checkBox5.SetBinding(CheckBox.IsCheckedProperty, bindingBase8);
				grid.Children.Add(checkBox5);
				checkBox6.SetValue(Grid.RowProperty, 1);
				checkBox6.SetValue(Grid.ColumnProperty, 5);
				checkBox6.SetValue(View.HorizontalOptionsProperty, LayoutOptions.Center);
				bindingExtension9.Mode = 1;
				bindingExtension9.Path = "Bit2";
				BindingBase bindingBase9 = bindingExtension9.ProvideValue(null);
				checkBox6.SetBinding(CheckBox.IsCheckedProperty, bindingBase9);
				grid.Children.Add(checkBox6);
				checkBox7.SetValue(Grid.RowProperty, 1);
				checkBox7.SetValue(Grid.ColumnProperty, 6);
				checkBox7.SetValue(View.HorizontalOptionsProperty, LayoutOptions.Center);
				bindingExtension10.Mode = 1;
				bindingExtension10.Path = "Bit1";
				BindingBase bindingBase10 = bindingExtension10.ProvideValue(null);
				checkBox7.SetBinding(CheckBox.IsCheckedProperty, bindingBase10);
				grid.Children.Add(checkBox7);
				checkBox8.SetValue(Grid.RowProperty, 1);
				checkBox8.SetValue(Grid.ColumnProperty, 7);
				checkBox8.SetValue(View.HorizontalOptionsProperty, LayoutOptions.Center);
				bindingExtension11.Mode = 1;
				bindingExtension11.Path = "Bit0";
				BindingBase bindingBase11 = bindingExtension11.ProvideValue(null);
				checkBox8.SetBinding(CheckBox.IsCheckedProperty, bindingBase11);
				grid.Children.Add(checkBox8);
				scrollView.Content = grid;
				stackLayout3.Children.Add(scrollView);
				viewCell.View = stackLayout3;
				return viewCell;
			}

			// Token: 0x04002F70 RID: 12144
			internal object[] parentValues;

			// Token: 0x04002F71 RID: 12145
			internal LongCodingPage root;
		}
	}
}
