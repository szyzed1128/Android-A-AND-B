using System;
using System.CodeDom.Compiler;
using System.Diagnostics;
using System.Globalization;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using CarScannerXamarinForms.Common.XAMLConverters;
using CarScannerXamarinForms.InApp;
using CarScannerXamarinForms.PlatformAdapters;
using CarScannerXamarinForms.Settings;
using CarScannerXamarinForms.UserControls;
using Xamarin.Forms;
using Xamarin.Forms.Internals;
using Xamarin.Forms.PlatformConfiguration.iOSSpecific;
using Xamarin.Forms.Xaml;
using Xamarin.Forms.Xaml.Diagnostics;
using Xamarin.Forms.Xaml.Internals;

namespace CarScannerXamarinForms.Coding.Pages
{
	// Token: 0x0200092F RID: 2351
	[XamlCompilation(2)]
	[XamlFilePath("Coding\\Pages\\CodingWithHexInput.xaml")]
	public class CodingWithHexInput : ContentPage
	{
		// Token: 0x06004E17 RID: 19991 RVA: 0x003A93B8 File Offset: 0x003A75B8
		public CodingWithHexInput(ICodingContainer coding)
		{
			this.InitializeComponent();
			base.Appearing += this.CodingDetailsPage_Appearing;
			this.Model = new HexCodingModel(coding);
			base.BindingContext = this.Model;
			this.switchCustomAddress.IsVisible = SharedSettings.Current.AdsProductPurchased;
			CustomizableCodingTemplate customizableCodingTemplate = (CustomizableCodingTemplate)this.Model.Coding;
			this.defaultReadAddress = customizableCodingTemplate.ReadModeAndAddress;
			this.defaultWriteAddress = customizableCodingTemplate.WriteModeAndAddress;
		}

		// Token: 0x17001788 RID: 6024
		// (get) Token: 0x06004E18 RID: 19992 RVA: 0x003A9456 File Offset: 0x003A7656
		// (set) Token: 0x06004E19 RID: 19993 RVA: 0x003A945E File Offset: 0x003A765E
		internal HexCodingModel Model
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

		// Token: 0x06004E1A RID: 19994 RVA: 0x003A9468 File Offset: 0x003A7668
		private async void CodingDetailsPage_Appearing(object sender, EventArgs e)
		{
			if (this.first_appearing)
			{
				this.first_appearing = false;
				await this.UpdateState();
			}
		}

		// Token: 0x06004E1B RID: 19995 RVA: 0x003A94A0 File Offset: 0x003A76A0
		public async Task UpdateState()
		{
			this.activityFrame.IsVisible = true;
			this.lv.IsEnabled = false;
			if (this.Model.UseCustomAddress)
			{
				int num = 0;
				if (this.Model.CustomReadAddress.Length < 4 || !int.TryParse(this.Model.CustomReadAddress, NumberStyles.HexNumber, CultureInfo.InvariantCulture.NumberFormat, out num) || this.Model.CustomWriteAddress.Length < 4 || !int.TryParse(this.Model.CustomWriteAddress, NumberStyles.HexNumber, CultureInfo.InvariantCulture.NumberFormat, out num))
				{
					await base.DisplayAlert("Wrong custom address format!", "Address should be in HEX format.\nCorrect format example: 2201FF, or 21FF", "OK");
					return;
				}
				CustomizableCodingTemplate customizableCodingTemplate = (CustomizableCodingTemplate)this.Model.Coding;
				customizableCodingTemplate.ReadModeAndAddress = this.Model.CustomReadAddress;
				customizableCodingTemplate.WriteModeAndAddress = this.Model.CustomWriteAddress;
			}
			else
			{
				((CustomizableCodingTemplate)this.Model.Coding).ReadModeAndAddress = this.defaultReadAddress;
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
				if (this.Model.Coding is CustomizableCodingTemplate && !(this.Model.Coding as CustomizableCodingTemplate).MakeChangesToInitialData)
				{
					this.Model.Data.VariableDataLength = true;
				}
			}
			this.activityFrame.IsVisible = false;
			this.lv.IsEnabled = true;
		}

		// Token: 0x06004E1C RID: 19996 RVA: 0x00022295 File Offset: 0x00020495
		private void Entry_Completed(object sender, EventArgs e)
		{
			PlatformHelper.CommonService.HideKeyboard();
		}

		// Token: 0x06004E1D RID: 19997 RVA: 0x003A94E4 File Offset: 0x003A76E4
		private async void BtnUpdateState_Clicked(object sender, EventArgs e)
		{
			this.UpdateState();
		}

		// Token: 0x06004E1E RID: 19998 RVA: 0x003A951C File Offset: 0x003A771C
		private void FullHexEditor_Completed(object sender, EventArgs e)
		{
			PlatformHelper.CommonService.HideKeyboard();
			Editor editor = (Editor)sender;
			this.Model.Data.ApplyChanges(editor.Text);
		}

		// Token: 0x06004E1F RID: 19999 RVA: 0x003A9550 File Offset: 0x003A7750
		private void FullHexEditor_Unfocused(object sender, FocusEventArgs e)
		{
			Editor editor = (Editor)sender;
			this.Model.Data.ApplyChanges(editor.Text);
			PlatformHelper.CommonService.HideKeyboard();
		}

		// Token: 0x06004E20 RID: 20000 RVA: 0x003A9584 File Offset: 0x003A7784
		private void BinEntry_Unfocused(object sender, FocusEventArgs e)
		{
			Entry entry = (Entry)sender;
			((ByteObject)entry.BindingContext).Bin = entry.Text;
			PlatformHelper.CommonService.HideKeyboard();
		}

		// Token: 0x06004E21 RID: 20001 RVA: 0x003A95B8 File Offset: 0x003A77B8
		private void BinEntry_Completed(object sender, EventArgs e)
		{
			Entry entry = (Entry)sender;
			((ByteObject)entry.BindingContext).Bin = entry.Text;
			PlatformHelper.CommonService.HideKeyboard();
		}

		// Token: 0x06004E22 RID: 20002 RVA: 0x003A95EC File Offset: 0x003A77EC
		private void HexEntry_Unfocused(object sender, FocusEventArgs e)
		{
			Entry entry = (Entry)sender;
			((ByteObject)entry.BindingContext).Hex = entry.Text;
			PlatformHelper.CommonService.HideKeyboard();
		}

		// Token: 0x06004E23 RID: 20003 RVA: 0x003A9620 File Offset: 0x003A7820
		private void HexEntry_Completed(object sender, EventArgs e)
		{
			Entry entry = (Entry)sender;
			((ByteObject)entry.BindingContext).Hex = entry.Text;
			PlatformHelper.CommonService.HideKeyboard();
		}

		// Token: 0x06004E24 RID: 20004 RVA: 0x003A9654 File Offset: 0x003A7854
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
					if (this.Model.CustomReadAddress.Length < 4 || !int.TryParse(this.Model.CustomReadAddress, NumberStyles.HexNumber, CultureInfo.InvariantCulture.NumberFormat, out num) || this.Model.CustomWriteAddress.Length < 4 || !int.TryParse(this.Model.CustomWriteAddress, NumberStyles.HexNumber, CultureInfo.InvariantCulture.NumberFormat, out num))
					{
						await base.DisplayAlert("Wrong custom address format!", "Address should be in HEX format.\nCorrect format example: 01FF", "OK");
						return;
					}
					CustomizableCodingTemplate customizableCodingTemplate = (CustomizableCodingTemplate)this.Model.Coding;
					customizableCodingTemplate.ReadModeAndAddress = this.Model.CustomReadAddress;
					customizableCodingTemplate.WriteModeAndAddress = this.Model.CustomWriteAddress;
				}
				else
				{
					CustomizableCodingTemplate customizableCodingTemplate2 = (CustomizableCodingTemplate)this.Model.Coding;
					customizableCodingTemplate2.ReadModeAndAddress = this.Model.CustomReadAddress;
					customizableCodingTemplate2.WriteModeAndAddress = this.Model.CustomWriteAddress;
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

		// Token: 0x06004E25 RID: 20005 RVA: 0x003A968C File Offset: 0x003A788C
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private void InitializeComponent()
		{
			if (ResourceLoader.IsEnabled && ResourceLoader.CanProvideContentFor(new ResourceLoader.ResourceLoadingQuery
			{
				AssemblyName = typeof(CodingWithHexInput).GetTypeInfo().Assembly.GetName(),
				ResourcePath = "Coding/Pages/CodingWithHexInput.xaml",
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
			VisualDiagnostics.RegisterSourceInfo(translate = new Translate(), new Uri("Coding\\Pages\\CodingWithHexInput.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 11, 5);
			DynamicResourceExtension dynamicResourceExtension;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension = new DynamicResourceExtension(), new Uri("Coding\\Pages\\CodingWithHexInput.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 14, 5);
			EmptyStringToFalseConverter emptyStringToFalseConverter;
			VisualDiagnostics.RegisterSourceInfo(emptyStringToFalseConverter = new EmptyStringToFalseConverter(), new Uri("Coding\\Pages\\CodingWithHexInput.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 17, 14);
			VagCodingPlatformToTrueConverter vagCodingPlatformToTrueConverter;
			VisualDiagnostics.RegisterSourceInfo(vagCodingPlatformToTrueConverter = new VagCodingPlatformToTrueConverter(), new Uri("Coding\\Pages\\CodingWithHexInput.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 18, 14);
			ResourceDictionary resourceDictionary;
			VisualDiagnostics.RegisterSourceInfo(resourceDictionary = new ResourceDictionary(), new Uri("Coding\\Pages\\CodingWithHexInput.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 16, 10);
			RowDefinition rowDefinition;
			VisualDiagnostics.RegisterSourceInfo(rowDefinition = new RowDefinition(), new Uri("Coding\\Pages\\CodingWithHexInput.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 24, 18);
			BindingExtension bindingExtension;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension = new BindingExtension(), new Uri("Coding\\Pages\\CodingWithHexInput.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 30, 17);
			DynamicResourceExtension dynamicResourceExtension2;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension2 = new DynamicResourceExtension(), new Uri("Coding\\Pages\\CodingWithHexInput.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 36, 29);
			BindingExtension bindingExtension2;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension2 = new BindingExtension(), new Uri("Coding\\Pages\\CodingWithHexInput.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 37, 29);
			Label label;
			VisualDiagnostics.RegisterSourceInfo(label = new Label(), new Uri("Coding\\Pages\\CodingWithHexInput.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 34, 26);
			StaticResourceExtension staticResourceExtension;
			VisualDiagnostics.RegisterSourceInfo(staticResourceExtension = new StaticResourceExtension(), new Uri("Coding\\Pages\\CodingWithHexInput.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 38, 32);
			BindingExtension bindingExtension3;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension3 = new BindingExtension(), new Uri("Coding\\Pages\\CodingWithHexInput.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 38, 32);
			BindingExtension bindingExtension4;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension4 = new BindingExtension(), new Uri("Coding\\Pages\\CodingWithHexInput.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 38, 129);
			Label label2;
			VisualDiagnostics.RegisterSourceInfo(label2 = new Label(), new Uri("Coding\\Pages\\CodingWithHexInput.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 38, 26);
			StaticResourceExtension staticResourceExtension2;
			VisualDiagnostics.RegisterSourceInfo(staticResourceExtension2 = new StaticResourceExtension(), new Uri("Coding\\Pages\\CodingWithHexInput.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 39, 32);
			BindingExtension bindingExtension5;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension5 = new BindingExtension(), new Uri("Coding\\Pages\\CodingWithHexInput.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 39, 32);
			BindingExtension bindingExtension6;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension6 = new BindingExtension(), new Uri("Coding\\Pages\\CodingWithHexInput.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 39, 140);
			Label label3;
			VisualDiagnostics.RegisterSourceInfo(label3 = new Label(), new Uri("Coding\\Pages\\CodingWithHexInput.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 39, 26);
			BindingExtension bindingExtension7;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension7 = new BindingExtension(), new Uri("Coding\\Pages\\CodingWithHexInput.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 41, 38);
			Translate translate2;
			VisualDiagnostics.RegisterSourceInfo(translate2 = new Translate(), new Uri("Coding\\Pages\\CodingWithHexInput.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 42, 36);
			Label label4;
			VisualDiagnostics.RegisterSourceInfo(label4 = new Label(), new Uri("Coding\\Pages\\CodingWithHexInput.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 42, 30);
			BindingExtension bindingExtension8;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension8 = new BindingExtension(), new Uri("Coding\\Pages\\CodingWithHexInput.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 48, 33);
			Entry entry;
			VisualDiagnostics.RegisterSourceInfo(entry = new Entry(), new Uri("Coding\\Pages\\CodingWithHexInput.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 43, 30);
			StaticResourceExtension staticResourceExtension3;
			VisualDiagnostics.RegisterSourceInfo(staticResourceExtension3 = new StaticResourceExtension(), new Uri("Coding\\Pages\\CodingWithHexInput.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 49, 60);
			BindingExtension bindingExtension9;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension9 = new BindingExtension(), new Uri("Coding\\Pages\\CodingWithHexInput.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 49, 60);
			Translate translate3;
			VisualDiagnostics.RegisterSourceInfo(translate3 = new Translate(), new Uri("Coding\\Pages\\CodingWithHexInput.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 52, 47);
			Span span;
			VisualDiagnostics.RegisterSourceInfo(span = new Span(), new Uri("Coding\\Pages\\CodingWithHexInput.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 52, 42);
			BindingExtension bindingExtension10;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension10 = new BindingExtension(), new Uri("Coding\\Pages\\CodingWithHexInput.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 53, 47);
			Span span2;
			VisualDiagnostics.RegisterSourceInfo(span2 = new Span(), new Uri("Coding\\Pages\\CodingWithHexInput.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 53, 42);
			FormattedString formattedString;
			VisualDiagnostics.RegisterSourceInfo(formattedString = new FormattedString(), new Uri("Coding\\Pages\\CodingWithHexInput.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 51, 38);
			Label label5;
			VisualDiagnostics.RegisterSourceInfo(label5 = new Label(), new Uri("Coding\\Pages\\CodingWithHexInput.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 49, 30);
			StackLayout stackLayout;
			VisualDiagnostics.RegisterSourceInfo(stackLayout = new StackLayout(), new Uri("Coding\\Pages\\CodingWithHexInput.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 41, 26);
			BindingExtension bindingExtension11;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension11 = new BindingExtension(), new Uri("Coding\\Pages\\CodingWithHexInput.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 62, 29);
			Translate translate4;
			VisualDiagnostics.RegisterSourceInfo(translate4 = new Translate(), new Uri("Coding\\Pages\\CodingWithHexInput.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 63, 29);
			LabelSwitch labelSwitch;
			VisualDiagnostics.RegisterSourceInfo(labelSwitch = new LabelSwitch(), new Uri("Coding\\Pages\\CodingWithHexInput.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 60, 26);
			BindingExtension bindingExtension12;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension12 = new BindingExtension(), new Uri("Coding\\Pages\\CodingWithHexInput.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 64, 38);
			Label label6;
			VisualDiagnostics.RegisterSourceInfo(label6 = new Label(), new Uri("Coding\\Pages\\CodingWithHexInput.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 65, 30);
			BindingExtension bindingExtension13;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension13 = new BindingExtension(), new Uri("Coding\\Pages\\CodingWithHexInput.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 71, 33);
			Entry entry2;
			VisualDiagnostics.RegisterSourceInfo(entry2 = new Entry(), new Uri("Coding\\Pages\\CodingWithHexInput.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 66, 30);
			Label label7;
			VisualDiagnostics.RegisterSourceInfo(label7 = new Label(), new Uri("Coding\\Pages\\CodingWithHexInput.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 73, 30);
			BindingExtension bindingExtension14;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension14 = new BindingExtension(), new Uri("Coding\\Pages\\CodingWithHexInput.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 79, 33);
			Entry entry3;
			VisualDiagnostics.RegisterSourceInfo(entry3 = new Entry(), new Uri("Coding\\Pages\\CodingWithHexInput.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 74, 30);
			StackLayout stackLayout2;
			VisualDiagnostics.RegisterSourceInfo(stackLayout2 = new StackLayout(), new Uri("Coding\\Pages\\CodingWithHexInput.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 64, 26);
			ColumnDefinition columnDefinition;
			VisualDiagnostics.RegisterSourceInfo(columnDefinition = new ColumnDefinition(), new Uri("Coding\\Pages\\CodingWithHexInput.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 85, 34);
			ColumnDefinition columnDefinition2;
			VisualDiagnostics.RegisterSourceInfo(columnDefinition2 = new ColumnDefinition(), new Uri("Coding\\Pages\\CodingWithHexInput.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 86, 34);
			BindingExtension bindingExtension15;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension15 = new BindingExtension(), new Uri("Coding\\Pages\\CodingWithHexInput.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 92, 33);
			Translate translate5;
			VisualDiagnostics.RegisterSourceInfo(translate5 = new Translate(), new Uri("Coding\\Pages\\CodingWithHexInput.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 93, 33);
			Button button;
			VisualDiagnostics.RegisterSourceInfo(button = new Button(), new Uri("Coding\\Pages\\CodingWithHexInput.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 88, 30);
			Translate translate6;
			VisualDiagnostics.RegisterSourceInfo(translate6 = new Translate(), new Uri("Coding\\Pages\\CodingWithHexInput.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 98, 33);
			Button button2;
			VisualDiagnostics.RegisterSourceInfo(button2 = new Button(), new Uri("Coding\\Pages\\CodingWithHexInput.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 94, 30);
			Grid grid;
			VisualDiagnostics.RegisterSourceInfo(grid = new Grid(), new Uri("Coding\\Pages\\CodingWithHexInput.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 83, 26);
			Translate translate7;
			VisualDiagnostics.RegisterSourceInfo(translate7 = new Translate(), new Uri("Coding\\Pages\\CodingWithHexInput.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 102, 54);
			Label label8;
			VisualDiagnostics.RegisterSourceInfo(label8 = new Label(), new Uri("Coding\\Pages\\CodingWithHexInput.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 102, 26);
			DynamicResourceExtension dynamicResourceExtension3;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension3 = new DynamicResourceExtension(), new Uri("Coding\\Pages\\CodingWithHexInput.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 106, 29);
			BindingExtension bindingExtension16;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension16 = new BindingExtension(), new Uri("Coding\\Pages\\CodingWithHexInput.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 111, 29);
			DynamicResourceExtension dynamicResourceExtension4;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension4 = new DynamicResourceExtension(), new Uri("Coding\\Pages\\CodingWithHexInput.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 112, 29);
			Editor editor;
			VisualDiagnostics.RegisterSourceInfo(editor = new Editor(), new Uri("Coding\\Pages\\CodingWithHexInput.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 103, 26);
			StackLayout stackLayout3;
			VisualDiagnostics.RegisterSourceInfo(stackLayout3 = new StackLayout(), new Uri("Coding\\Pages\\CodingWithHexInput.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 33, 22);
			DataTemplate dataTemplate;
			VisualDiagnostics.RegisterSourceInfo(dataTemplate = new DataTemplate(), new Uri("Coding\\Pages\\CodingWithHexInput.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 118, 22);
			ListView listView;
			VisualDiagnostics.RegisterSourceInfo(listView = new ListView(), new Uri("Coding\\Pages\\CodingWithHexInput.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 26, 14);
			ActivityFrame activityFrame;
			VisualDiagnostics.RegisterSourceInfo(activityFrame = new ActivityFrame(), new Uri("Coding\\Pages\\CodingWithHexInput.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 270, 14);
			Grid grid2;
			VisualDiagnostics.RegisterSourceInfo(grid2 = new Grid(), new Uri("Coding\\Pages\\CodingWithHexInput.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 22, 10);
			VisualDiagnostics.RegisterSourceInfo(this, new Uri("Coding\\Pages\\CodingWithHexInput.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 2, 2);
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
			nameScope.RegisterName("entryCustomReadAddress", entry2);
			if (entry2.StyleId == null)
			{
				entry2.StyleId = "entryCustomReadAddress";
			}
			nameScope.RegisterName("entryCustomWriteAddress", entry3);
			if (entry3.StyleId == null)
			{
				entry3.StyleId = "entryCustomWriteAddress";
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
			nameScope.RegisterName("activityFrame", activityFrame);
			if (activityFrame.StyleId == null)
			{
				activityFrame.StyleId = "activityFrame";
			}
			this.lv = listView;
			this.entryPassword = entry;
			this.switchCustomAddress = labelSwitch;
			this.entryCustomReadAddress = entry2;
			this.entryCustomWriteAddress = entry3;
			this.btnUpdateState = button;
			this.btnSaveNewValue = button2;
			this.editorHex = editor;
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
			xmlNamespaceResolver.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider.Add(typeFromHandle2, new XamlTypeResolver(xmlNamespaceResolver, typeof(CodingWithHexInput).GetTypeInfo().Assembly));
			xamlServiceProvider.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(11, 5)));
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
			xmlNamespaceResolver2.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider2.Add(typeFromHandle4, new XamlTypeResolver(xmlNamespaceResolver2, typeof(CodingWithHexInput).GetTypeInfo().Assembly));
			xamlServiceProvider2.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(14, 5)));
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
			stackLayout3.SetValue(StackLayout.OrientationProperty, 0);
			label.SetValue(Label.FontAttributesProperty, new FontAttributesConverter().ConvertFromInvariantString("Bold"));
			dynamicResourceExtension2.Key = "BaseFontSize+++";
			IMarkupExtension<DynamicResource> markupExtension3 = dynamicResourceExtension2;
			XamlServiceProvider xamlServiceProvider3 = new XamlServiceProvider();
			Type typeFromHandle5 = typeof(IProvideValueTarget);
			object[] array3 = new object[0 + 5];
			array3[0] = label;
			array3[1] = stackLayout3;
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
			xmlNamespaceResolver3.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider3.Add(typeFromHandle6, new XamlTypeResolver(xmlNamespaceResolver3, typeof(CodingWithHexInput).GetTypeInfo().Assembly));
			xamlServiceProvider3.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(36, 29)));
			DynamicResource dynamicResource2 = markupExtension3.ProvideValue(xamlServiceProvider3);
			label.SetDynamicResource(Label.FontSizeProperty, dynamicResource2.Key);
			bindingExtension2.Path = "Coding.Name";
			BindingBase bindingBase2 = bindingExtension2.ProvideValue(null);
			label.SetBinding(Label.TextProperty, bindingBase2);
			stackLayout3.Children.Add(label);
			staticResourceExtension.Key = "EmptyStringToFalseConverter";
			IMarkupExtension markupExtension4 = staticResourceExtension;
			XamlServiceProvider xamlServiceProvider4 = new XamlServiceProvider();
			Type typeFromHandle7 = typeof(IProvideValueTarget);
			object[] array4 = new object[0 + 6];
			array4[0] = bindingExtension3;
			array4[1] = label2;
			array4[2] = stackLayout3;
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
			xmlNamespaceResolver4.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider4.Add(typeFromHandle8, new XamlTypeResolver(xmlNamespaceResolver4, typeof(CodingWithHexInput).GetTypeInfo().Assembly));
			xamlServiceProvider4.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(38, 32)));
			object obj6 = markupExtension4.ProvideValue(xamlServiceProvider4);
			bindingExtension3.Converter = obj6;
			bindingExtension3.Path = "Coding.Description";
			BindingBase bindingBase3 = bindingExtension3.ProvideValue(null);
			label2.SetBinding(VisualElement.IsVisibleProperty, bindingBase3);
			bindingExtension4.Path = "Coding.Description";
			BindingBase bindingBase4 = bindingExtension4.ProvideValue(null);
			label2.SetBinding(Label.TextProperty, bindingBase4);
			stackLayout3.Children.Add(label2);
			bindingExtension5.Mode = 2;
			staticResourceExtension2.Key = "EmptyStringToFalseConverter";
			IMarkupExtension markupExtension5 = staticResourceExtension2;
			XamlServiceProvider xamlServiceProvider5 = new XamlServiceProvider();
			Type typeFromHandle9 = typeof(IProvideValueTarget);
			object[] array5 = new object[0 + 6];
			array5[0] = bindingExtension5;
			array5[1] = label3;
			array5[2] = stackLayout3;
			array5[3] = listView;
			array5[4] = grid2;
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
			xmlNamespaceResolver5.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider5.Add(typeFromHandle10, new XamlTypeResolver(xmlNamespaceResolver5, typeof(CodingWithHexInput).GetTypeInfo().Assembly));
			xamlServiceProvider5.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(39, 32)));
			object obj8 = markupExtension5.ProvideValue(xamlServiceProvider5);
			bindingExtension5.Converter = obj8;
			bindingExtension5.Path = "InnerDescription";
			BindingBase bindingBase5 = bindingExtension5.ProvideValue(null);
			label3.SetBinding(VisualElement.IsVisibleProperty, bindingBase5);
			bindingExtension6.Path = "Coding.InnerDescription";
			BindingBase bindingBase6 = bindingExtension6.ProvideValue(null);
			label3.SetBinding(Label.TextProperty, bindingBase6);
			stackLayout3.Children.Add(label3);
			bindingExtension7.Mode = 2;
			bindingExtension7.Path = "Coding.PasswordVisible";
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
			array6[2] = stackLayout3;
			array6[3] = listView;
			array6[4] = grid2;
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
			xmlNamespaceResolver6.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider6.Add(typeFromHandle12, new XamlTypeResolver(xmlNamespaceResolver6, typeof(CodingWithHexInput).GetTypeInfo().Assembly));
			xamlServiceProvider6.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(42, 36)));
			object obj10 = markupExtension6.ProvideValue(xamlServiceProvider6);
			label4.Text = obj10;
			stackLayout.Children.Add(label4);
			entry.Completed += this.Entry_Completed;
			entry.SetValue(InputView.IsSpellCheckEnabledProperty, false);
			entry.SetValue(Entry.IsTextPredictionEnabledProperty, false);
			bindingExtension8.Mode = 1;
			bindingExtension8.Path = "Coding.Password";
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
			array7[3] = stackLayout3;
			array7[4] = listView;
			array7[5] = grid2;
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
			xmlNamespaceResolver7.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider7.Add(typeFromHandle14, new XamlTypeResolver(xmlNamespaceResolver7, typeof(CodingWithHexInput).GetTypeInfo().Assembly));
			xamlServiceProvider7.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(49, 60)));
			object obj12 = markupExtension7.ProvideValue(xamlServiceProvider7);
			bindingExtension9.Converter = obj12;
			bindingExtension9.Path = "Coding.PasswordHint";
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
			array8[4] = stackLayout3;
			array8[5] = listView;
			array8[6] = grid2;
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
			xmlNamespaceResolver8.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider8.Add(typeFromHandle16, new XamlTypeResolver(xmlNamespaceResolver8, typeof(CodingWithHexInput).GetTypeInfo().Assembly));
			xamlServiceProvider8.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(52, 47)));
			object obj14 = markupExtension8.ProvideValue(xamlServiceProvider8);
			span.Text = obj14;
			formattedString.Spans.Add(span);
			bindingExtension10.Path = "Coding.PasswordHint";
			BindingBase bindingBase10 = bindingExtension10.ProvideValue(null);
			span2.SetBinding(Span.TextProperty, bindingBase10);
			formattedString.Spans.Add(span2);
			label5.SetValue(Label.FormattedTextProperty, formattedString);
			stackLayout.Children.Add(label5);
			stackLayout3.Children.Add(stackLayout);
			bindingExtension11.Mode = 1;
			bindingExtension11.Path = "UseCustomAddress";
			BindingBase bindingBase11 = bindingExtension11.ProvideValue(null);
			labelSwitch.SetBinding(LabelSwitch.IsToggledProperty, bindingBase11);
			translate4.Text = "coding_CustomAddress";
			IMarkupExtension markupExtension9 = translate4;
			XamlServiceProvider xamlServiceProvider9 = new XamlServiceProvider();
			Type typeFromHandle17 = typeof(IProvideValueTarget);
			object[] array9 = new object[0 + 5];
			array9[0] = labelSwitch;
			array9[1] = stackLayout3;
			array9[2] = listView;
			array9[3] = grid2;
			array9[4] = this;
			object obj15;
			xamlServiceProvider9.Add(typeFromHandle17, obj15 = new SimpleValueTargetProvider(array9, LabelSwitch.TextProperty, nameScope));
			xamlServiceProvider9.Add(typeof(IReferenceProvider), obj15);
			Type typeFromHandle18 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver9 = new XmlNamespaceResolver();
			xmlNamespaceResolver9.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver9.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver9.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver9.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver9.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver9.Add("settings", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver9.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider9.Add(typeFromHandle18, new XamlTypeResolver(xmlNamespaceResolver9, typeof(CodingWithHexInput).GetTypeInfo().Assembly));
			xamlServiceProvider9.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(63, 29)));
			object obj16 = markupExtension9.ProvideValue(xamlServiceProvider9);
			labelSwitch.Text = obj16;
			stackLayout3.Children.Add(labelSwitch);
			bindingExtension12.Mode = 2;
			bindingExtension12.Path = "UseCustomAddress";
			BindingBase bindingBase12 = bindingExtension12.ProvideValue(null);
			stackLayout2.SetBinding(VisualElement.IsVisibleProperty, bindingBase12);
			stackLayout2.SetValue(StackLayout.OrientationProperty, 0);
			label6.SetValue(Label.TextProperty, "Read address:");
			stackLayout2.Children.Add(label6);
			entry2.Completed += this.Entry_Completed;
			entry2.SetValue(InputView.IsSpellCheckEnabledProperty, false);
			entry2.SetValue(Entry.IsTextPredictionEnabledProperty, false);
			bindingExtension13.Mode = 1;
			bindingExtension13.Path = "CustomReadAddress";
			BindingBase bindingBase13 = bindingExtension13.ProvideValue(null);
			entry2.SetBinding(Entry.TextProperty, bindingBase13);
			stackLayout2.Children.Add(entry2);
			label7.SetValue(Label.TextProperty, "Write address:");
			stackLayout2.Children.Add(label7);
			entry3.Completed += this.Entry_Completed;
			entry3.SetValue(InputView.IsSpellCheckEnabledProperty, false);
			entry3.SetValue(Entry.IsTextPredictionEnabledProperty, false);
			bindingExtension14.Mode = 1;
			bindingExtension14.Path = "CustomWriteAddress";
			BindingBase bindingBase14 = bindingExtension14.ProvideValue(null);
			entry3.SetBinding(Entry.TextProperty, bindingBase14);
			stackLayout2.Children.Add(entry3);
			stackLayout3.Children.Add(stackLayout2);
			columnDefinition.SetValue(ColumnDefinition.WidthProperty, new GridLengthTypeConverter().ConvertFromInvariantString("*"));
			grid.GetValue(Grid.ColumnDefinitionsProperty).Add(columnDefinition);
			columnDefinition2.SetValue(ColumnDefinition.WidthProperty, new GridLengthTypeConverter().ConvertFromInvariantString("*"));
			grid.GetValue(Grid.ColumnDefinitionsProperty).Add(columnDefinition2);
			button.SetValue(Grid.ColumnProperty, 0);
			button.Clicked += this.BtnUpdateState_Clicked;
			bindingExtension15.Path = "HasCurrentState";
			BindingBase bindingBase15 = bindingExtension15.ProvideValue(null);
			button.SetBinding(VisualElement.IsVisibleProperty, bindingBase15);
			translate5.Text = "Mode06Page_btnRefresh.Content";
			IMarkupExtension markupExtension10 = translate5;
			XamlServiceProvider xamlServiceProvider10 = new XamlServiceProvider();
			Type typeFromHandle19 = typeof(IProvideValueTarget);
			object[] array10 = new object[0 + 6];
			array10[0] = button;
			array10[1] = grid;
			array10[2] = stackLayout3;
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
			xmlNamespaceResolver10.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider10.Add(typeFromHandle20, new XamlTypeResolver(xmlNamespaceResolver10, typeof(CodingWithHexInput).GetTypeInfo().Assembly));
			xamlServiceProvider10.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(93, 33)));
			object obj18 = markupExtension10.ProvideValue(xamlServiceProvider10);
			button.Text = obj18;
			grid.Children.Add(button);
			button2.SetValue(Grid.ColumnProperty, 1);
			button2.Clicked += this.btnSaveState_Clicked;
			translate6.Text = "coding_Apply";
			IMarkupExtension markupExtension11 = translate6;
			XamlServiceProvider xamlServiceProvider11 = new XamlServiceProvider();
			Type typeFromHandle21 = typeof(IProvideValueTarget);
			object[] array11 = new object[0 + 6];
			array11[0] = button2;
			array11[1] = grid;
			array11[2] = stackLayout3;
			array11[3] = listView;
			array11[4] = grid2;
			array11[5] = this;
			object obj19;
			xamlServiceProvider11.Add(typeFromHandle21, obj19 = new SimpleValueTargetProvider(array11, Button.TextProperty, nameScope));
			xamlServiceProvider11.Add(typeof(IReferenceProvider), obj19);
			Type typeFromHandle22 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver11 = new XmlNamespaceResolver();
			xmlNamespaceResolver11.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver11.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver11.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver11.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver11.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver11.Add("settings", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver11.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider11.Add(typeFromHandle22, new XamlTypeResolver(xmlNamespaceResolver11, typeof(CodingWithHexInput).GetTypeInfo().Assembly));
			xamlServiceProvider11.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(98, 33)));
			object obj20 = markupExtension11.ProvideValue(xamlServiceProvider11);
			button2.Text = obj20;
			grid.Children.Add(button2);
			stackLayout3.Children.Add(grid);
			label8.SetValue(Label.FontAttributesProperty, new FontAttributesConverter().ConvertFromInvariantString("Bold"));
			translate7.Text = "coding_Data";
			IMarkupExtension markupExtension12 = translate7;
			XamlServiceProvider xamlServiceProvider12 = new XamlServiceProvider();
			Type typeFromHandle23 = typeof(IProvideValueTarget);
			object[] array12 = new object[0 + 5];
			array12[0] = label8;
			array12[1] = stackLayout3;
			array12[2] = listView;
			array12[3] = grid2;
			array12[4] = this;
			object obj21;
			xamlServiceProvider12.Add(typeFromHandle23, obj21 = new SimpleValueTargetProvider(array12, Label.TextProperty, nameScope));
			xamlServiceProvider12.Add(typeof(IReferenceProvider), obj21);
			Type typeFromHandle24 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver12 = new XmlNamespaceResolver();
			xmlNamespaceResolver12.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver12.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver12.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver12.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver12.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver12.Add("settings", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver12.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider12.Add(typeFromHandle24, new XamlTypeResolver(xmlNamespaceResolver12, typeof(CodingWithHexInput).GetTypeInfo().Assembly));
			xamlServiceProvider12.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(102, 54)));
			object obj22 = markupExtension12.ProvideValue(xamlServiceProvider12);
			label8.Text = obj22;
			stackLayout3.Children.Add(label8);
			editor.SetValue(Editor.AutoSizeProperty, 1);
			dynamicResourceExtension3.Key = "EntryBackgroundColor";
			IMarkupExtension<DynamicResource> markupExtension13 = dynamicResourceExtension3;
			XamlServiceProvider xamlServiceProvider13 = new XamlServiceProvider();
			Type typeFromHandle25 = typeof(IProvideValueTarget);
			object[] array13 = new object[0 + 5];
			array13[0] = editor;
			array13[1] = stackLayout3;
			array13[2] = listView;
			array13[3] = grid2;
			array13[4] = this;
			object obj23;
			xamlServiceProvider13.Add(typeFromHandle25, obj23 = new SimpleValueTargetProvider(array13, VisualElement.BackgroundColorProperty, nameScope));
			xamlServiceProvider13.Add(typeof(IReferenceProvider), obj23);
			Type typeFromHandle26 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver13 = new XmlNamespaceResolver();
			xmlNamespaceResolver13.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver13.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver13.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver13.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver13.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver13.Add("settings", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver13.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider13.Add(typeFromHandle26, new XamlTypeResolver(xmlNamespaceResolver13, typeof(CodingWithHexInput).GetTypeInfo().Assembly));
			xamlServiceProvider13.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(106, 29)));
			DynamicResource dynamicResource3 = markupExtension13.ProvideValue(xamlServiceProvider13);
			editor.SetDynamicResource(VisualElement.BackgroundColorProperty, dynamicResource3.Key);
			editor.Completed += this.FullHexEditor_Completed;
			editor.SetValue(InputView.IsSpellCheckEnabledProperty, false);
			editor.SetValue(Editor.IsTextPredictionEnabledProperty, false);
			editor.SetValue(InputView.KeyboardProperty, new KeyboardTypeConverter().ConvertFromInvariantString("Default"));
			bindingExtension16.Mode = 2;
			bindingExtension16.Path = "Data.Hex";
			BindingBase bindingBase16 = bindingExtension16.ProvideValue(null);
			editor.SetBinding(Editor.TextProperty, bindingBase16);
			dynamicResourceExtension4.Key = "TextColor";
			IMarkupExtension<DynamicResource> markupExtension14 = dynamicResourceExtension4;
			XamlServiceProvider xamlServiceProvider14 = new XamlServiceProvider();
			Type typeFromHandle27 = typeof(IProvideValueTarget);
			object[] array14 = new object[0 + 5];
			array14[0] = editor;
			array14[1] = stackLayout3;
			array14[2] = listView;
			array14[3] = grid2;
			array14[4] = this;
			object obj24;
			xamlServiceProvider14.Add(typeFromHandle27, obj24 = new SimpleValueTargetProvider(array14, Editor.TextColorProperty, nameScope));
			xamlServiceProvider14.Add(typeof(IReferenceProvider), obj24);
			Type typeFromHandle28 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver14 = new XmlNamespaceResolver();
			xmlNamespaceResolver14.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver14.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver14.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver14.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver14.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver14.Add("settings", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver14.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider14.Add(typeFromHandle28, new XamlTypeResolver(xmlNamespaceResolver14, typeof(CodingWithHexInput).GetTypeInfo().Assembly));
			xamlServiceProvider14.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(112, 29)));
			DynamicResource dynamicResource4 = markupExtension14.ProvideValue(xamlServiceProvider14);
			editor.SetDynamicResource(Editor.TextColorProperty, dynamicResource4.Key);
			editor.Unfocused += this.FullHexEditor_Unfocused;
			stackLayout3.Children.Add(editor);
			listView.SetValue(ListView.HeaderProperty, stackLayout3);
			IDataTemplate dataTemplate2 = dataTemplate;
			CodingWithHexInput.<InitializeComponent>_anonXamlCDataTemplate_10 <InitializeComponent>_anonXamlCDataTemplate_ = new CodingWithHexInput.<InitializeComponent>_anonXamlCDataTemplate_10();
			object[] array15 = new object[0 + 4];
			array15[0] = dataTemplate;
			array15[1] = listView;
			array15[2] = grid2;
			array15[3] = this;
			<InitializeComponent>_anonXamlCDataTemplate_.parentValues = array15;
			<InitializeComponent>_anonXamlCDataTemplate_.root = this;
			dataTemplate2.LoadTemplate = new Func<object>(<InitializeComponent>_anonXamlCDataTemplate_.LoadDataTemplate);
			listView.SetValue(ItemsView<Cell>.ItemTemplateProperty, dataTemplate);
			grid2.Children.Add(listView);
			activityFrame.SetValue(Grid.RowProperty, 0);
			activityFrame.SetValue(VisualElement.IsVisibleProperty, new VisualElement.VisibilityConverter().ConvertFromInvariantString("False"));
			grid2.Children.Add(activityFrame);
			this.SetValue(ContentPage.ContentProperty, grid2);
		}

		// Token: 0x06004E26 RID: 20006 RVA: 0x003ABBDD File Offset: 0x003A9DDD
		[CompilerGenerated]
		private void <btnSaveState_Clicked>b__18_0(string s)
		{
			Device.BeginInvokeOnMainThread(delegate
			{
				this.activityFrame.Text = s;
			});
		}

		// Token: 0x06004E27 RID: 20007 RVA: 0x003ABC04 File Offset: 0x003A9E04
		private void __InitComponentRuntime()
		{
			Extensions.LoadFromXaml<CodingWithHexInput>(this, typeof(CodingWithHexInput));
			this.lv = NameScopeExtensions.FindByName<ListView>(this, "lv");
			this.entryPassword = NameScopeExtensions.FindByName<Entry>(this, "entryPassword");
			this.switchCustomAddress = NameScopeExtensions.FindByName<LabelSwitch>(this, "switchCustomAddress");
			this.entryCustomReadAddress = NameScopeExtensions.FindByName<Entry>(this, "entryCustomReadAddress");
			this.entryCustomWriteAddress = NameScopeExtensions.FindByName<Entry>(this, "entryCustomWriteAddress");
			this.btnUpdateState = NameScopeExtensions.FindByName<Button>(this, "btnUpdateState");
			this.btnSaveNewValue = NameScopeExtensions.FindByName<Button>(this, "btnSaveNewValue");
			this.editorHex = NameScopeExtensions.FindByName<Editor>(this, "editorHex");
			this.activityFrame = NameScopeExtensions.FindByName<ActivityFrame>(this, "activityFrame");
		}

		// Token: 0x04002EA2 RID: 11938
		private string defaultReadAddress = "";

		// Token: 0x04002EA3 RID: 11939
		private string defaultWriteAddress = "";

		// Token: 0x04002EA4 RID: 11940
		[CompilerGenerated]
		private HexCodingModel <Model>k__BackingField;

		// Token: 0x04002EA5 RID: 11941
		private bool first_appearing = true;

		// Token: 0x04002EA6 RID: 11942
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private ListView lv;

		// Token: 0x04002EA7 RID: 11943
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private Entry entryPassword;

		// Token: 0x04002EA8 RID: 11944
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private LabelSwitch switchCustomAddress;

		// Token: 0x04002EA9 RID: 11945
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private Entry entryCustomReadAddress;

		// Token: 0x04002EAA RID: 11946
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private Entry entryCustomWriteAddress;

		// Token: 0x04002EAB RID: 11947
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private Button btnUpdateState;

		// Token: 0x04002EAC RID: 11948
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private Button btnSaveNewValue;

		// Token: 0x04002EAD RID: 11949
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private Editor editorHex;

		// Token: 0x04002EAE RID: 11950
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private ActivityFrame activityFrame;

		// Token: 0x02000930 RID: 2352
		[CompilerGenerated]
		private sealed class <>c__DisplayClass18_0
		{
			// Token: 0x06004E28 RID: 20008 RVA: 0x00002050 File Offset: 0x00000250
			public <>c__DisplayClass18_0()
			{
			}

			// Token: 0x06004E29 RID: 20009 RVA: 0x003ABCBB File Offset: 0x003A9EBB
			internal void <btnSaveState_Clicked>b__1()
			{
				this.<>4__this.activityFrame.Text = this.s;
			}

			// Token: 0x04002EAF RID: 11951
			public string s;

			// Token: 0x04002EB0 RID: 11952
			public CodingWithHexInput <>4__this;
		}

		// Token: 0x02000931 RID: 2353
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <BtnUpdateState_Clicked>d__11 : IAsyncStateMachine
		{
			// Token: 0x06004E2A RID: 20010 RVA: 0x003ABCD4 File Offset: 0x003A9ED4
			void IAsyncStateMachine.MoveNext()
			{
				CodingWithHexInput codingWithHexInput = this;
				try
				{
					codingWithHexInput.UpdateState();
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

			// Token: 0x06004E2B RID: 20011 RVA: 0x003ABD2C File Offset: 0x003A9F2C
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x04002EB1 RID: 11953
			public int <>1__state;

			// Token: 0x04002EB2 RID: 11954
			public AsyncVoidMethodBuilder <>t__builder;

			// Token: 0x04002EB3 RID: 11955
			public CodingWithHexInput <>4__this;
		}

		// Token: 0x02000932 RID: 2354
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <CodingDetailsPage_Appearing>d__8 : IAsyncStateMachine
		{
			// Token: 0x06004E2C RID: 20012 RVA: 0x003ABD3C File Offset: 0x003A9F3C
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				CodingWithHexInput codingWithHexInput = this;
				try
				{
					TaskAwaiter taskAwaiter;
					if (num != 0)
					{
						if (!codingWithHexInput.first_appearing)
						{
							goto IL_0078;
						}
						codingWithHexInput.first_appearing = false;
						taskAwaiter = codingWithHexInput.UpdateState().GetAwaiter();
						if (!taskAwaiter.IsCompleted)
						{
							num2 = 0;
							TaskAwaiter taskAwaiter2 = taskAwaiter;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, CodingWithHexInput.<CodingDetailsPage_Appearing>d__8>(ref taskAwaiter, ref this);
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

			// Token: 0x06004E2D RID: 20013 RVA: 0x003ABE00 File Offset: 0x003AA000
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x04002EB4 RID: 11956
			public int <>1__state;

			// Token: 0x04002EB5 RID: 11957
			public AsyncVoidMethodBuilder <>t__builder;

			// Token: 0x04002EB6 RID: 11958
			public CodingWithHexInput <>4__this;

			// Token: 0x04002EB7 RID: 11959
			private TaskAwaiter <>u__1;
		}

		// Token: 0x02000933 RID: 2355
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <UpdateState>d__9 : IAsyncStateMachine
		{
			// Token: 0x06004E2E RID: 20014 RVA: 0x003ABE10 File Offset: 0x003AA010
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				CodingWithHexInput codingWithHexInput = this;
				try
				{
					TaskAwaiter taskAwaiter3;
					TaskAwaiter taskAwaiter4;
					if (num != 0)
					{
						TaskAwaiter<CodingRequestResult> taskAwaiter5;
						if (num != 1)
						{
							codingWithHexInput.activityFrame.IsVisible = true;
							codingWithHexInput.lv.IsEnabled = false;
							if (codingWithHexInput.Model.UseCustomAddress)
							{
								int num3 = 0;
								if (codingWithHexInput.Model.CustomReadAddress.Length >= 4 && int.TryParse(codingWithHexInput.Model.CustomReadAddress, NumberStyles.HexNumber, CultureInfo.InvariantCulture.NumberFormat, out num3) && codingWithHexInput.Model.CustomWriteAddress.Length >= 4 && int.TryParse(codingWithHexInput.Model.CustomWriteAddress, NumberStyles.HexNumber, CultureInfo.InvariantCulture.NumberFormat, out num3))
								{
									CustomizableCodingTemplate customizableCodingTemplate = (CustomizableCodingTemplate)codingWithHexInput.Model.Coding;
									customizableCodingTemplate.ReadModeAndAddress = codingWithHexInput.Model.CustomReadAddress;
									customizableCodingTemplate.WriteModeAndAddress = codingWithHexInput.Model.CustomWriteAddress;
								}
								else
								{
									taskAwaiter3 = codingWithHexInput.DisplayAlert("Wrong custom address format!", "Address should be in HEX format.\nCorrect format example: 2201FF, or 21FF", "OK").GetAwaiter();
									if (!taskAwaiter3.IsCompleted)
									{
										num2 = 0;
										taskAwaiter4 = taskAwaiter3;
										this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, CodingWithHexInput.<UpdateState>d__9>(ref taskAwaiter3, ref this);
										return;
									}
									goto IL_0150;
								}
							}
							else
							{
								((CustomizableCodingTemplate)codingWithHexInput.Model.Coding).ReadModeAndAddress = codingWithHexInput.defaultReadAddress;
							}
							string text = "";
							if (SharedSettings.Current.DeveloperMode)
							{
								text = codingWithHexInput.entryPassword.Text;
							}
							taskAwaiter5 = codingWithHexInput.Model.Coding.UpdateCurrentState(text, null).GetAwaiter();
							if (!taskAwaiter5.IsCompleted)
							{
								num2 = 1;
								taskAwaiter2 = taskAwaiter5;
								this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<CodingRequestResult>, CodingWithHexInput.<UpdateState>d__9>(ref taskAwaiter5, ref this);
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
							codingWithHexInput.Model.Data = new HexStringObject(codingWithHexInput.Model.Coding.CurrentState);
							if (codingWithHexInput.Model.Coding is CustomizableCodingTemplate && !(codingWithHexInput.Model.Coding as CustomizableCodingTemplate).MakeChangesToInitialData)
							{
								codingWithHexInput.Model.Data.VariableDataLength = true;
							}
						}
						codingWithHexInput.activityFrame.IsVisible = false;
						codingWithHexInput.lv.IsEnabled = true;
						goto IL_028E;
					}
					taskAwaiter3 = taskAwaiter4;
					taskAwaiter4 = default(TaskAwaiter);
					num2 = -1;
					IL_0150:
					taskAwaiter3.GetResult();
				}
				catch (Exception ex)
				{
					num2 = -2;
					this.<>t__builder.SetException(ex);
					return;
				}
				IL_028E:
				num2 = -2;
				this.<>t__builder.SetResult();
			}

			// Token: 0x06004E2F RID: 20015 RVA: 0x003AC0DC File Offset: 0x003AA2DC
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x04002EB8 RID: 11960
			public int <>1__state;

			// Token: 0x04002EB9 RID: 11961
			public AsyncTaskMethodBuilder <>t__builder;

			// Token: 0x04002EBA RID: 11962
			public CodingWithHexInput <>4__this;

			// Token: 0x04002EBB RID: 11963
			private TaskAwaiter <>u__1;

			// Token: 0x04002EBC RID: 11964
			private TaskAwaiter<CodingRequestResult> <>u__2;
		}

		// Token: 0x02000934 RID: 2356
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <btnSaveState_Clicked>d__18 : IAsyncStateMachine
		{
			// Token: 0x06004E30 RID: 20016 RVA: 0x003AC0EC File Offset: 0x003AA2EC
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				CodingWithHexInput codingWithHexInput = this;
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
						goto IL_038D;
					}
					case 5:
					{
						TaskAwaiter<CodingRequestResult> taskAwaiter7;
						taskAwaiter6 = taskAwaiter7;
						taskAwaiter7 = default(TaskAwaiter<CodingRequestResult>);
						num2 = -1;
						goto IL_0492;
					}
					case 6:
					{
						TaskAwaiter taskAwaiter5;
						taskAwaiter4 = taskAwaiter5;
						taskAwaiter5 = default(TaskAwaiter);
						num2 = -1;
						goto IL_0521;
					}
					case 7:
					{
						TaskAwaiter taskAwaiter5;
						taskAwaiter4 = taskAwaiter5;
						taskAwaiter5 = default(TaskAwaiter);
						num2 = -1;
						goto IL_057C;
					}
					default:
						if (codingWithHexInput.Model.Coding.RequiresPro && !SharedSettings.Current.AdsProductPurchased)
						{
							taskAwaiter3 = codingWithHexInput.DisplayAlert("Car Scanner Pro", Translate.GetString("FeatureLocked"), "OK", Translate.GetString("btnCancel.Content")).GetAwaiter();
							if (!taskAwaiter3.IsCompleted)
							{
								num2 = 0;
								taskAwaiter2 = taskAwaiter3;
								this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<bool>, CodingWithHexInput.<btnSaveState_Clicked>d__18>(ref taskAwaiter3, ref this);
								return;
							}
						}
						else if (SharedSettings.Current.CodingsCounter >= 3 && !SharedSettings.Current.AdsProductPurchased)
						{
							taskAwaiter3 = codingWithHexInput.DisplayAlert("Car Scanner Pro", string.Format(Translate.GetString("coding_FreeCodingsFinished"), SharedSettings.Current.CodingsCounter, 3), "OK", Translate.GetString("btnCancel.Content")).GetAwaiter();
							if (!taskAwaiter3.IsCompleted)
							{
								num2 = 2;
								taskAwaiter2 = taskAwaiter3;
								this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<bool>, CodingWithHexInput.<btnSaveState_Clicked>d__18>(ref taskAwaiter3, ref this);
								return;
							}
							goto IL_01F5;
						}
						else
						{
							if (codingWithHexInput.Model.UseCustomAddress)
							{
								int num3 = 0;
								if (codingWithHexInput.Model.CustomReadAddress.Length >= 4 && int.TryParse(codingWithHexInput.Model.CustomReadAddress, NumberStyles.HexNumber, CultureInfo.InvariantCulture.NumberFormat, out num3) && codingWithHexInput.Model.CustomWriteAddress.Length >= 4 && int.TryParse(codingWithHexInput.Model.CustomWriteAddress, NumberStyles.HexNumber, CultureInfo.InvariantCulture.NumberFormat, out num3))
								{
									CustomizableCodingTemplate customizableCodingTemplate = (CustomizableCodingTemplate)codingWithHexInput.Model.Coding;
									customizableCodingTemplate.ReadModeAndAddress = codingWithHexInput.Model.CustomReadAddress;
									customizableCodingTemplate.WriteModeAndAddress = codingWithHexInput.Model.CustomWriteAddress;
								}
								else
								{
									taskAwaiter4 = codingWithHexInput.DisplayAlert("Wrong custom address format!", "Address should be in HEX format.\nCorrect format example: 01FF", "OK").GetAwaiter();
									if (!taskAwaiter4.IsCompleted)
									{
										num2 = 4;
										TaskAwaiter taskAwaiter5 = taskAwaiter4;
										this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, CodingWithHexInput.<btnSaveState_Clicked>d__18>(ref taskAwaiter4, ref this);
										return;
									}
									goto IL_038D;
								}
							}
							else
							{
								CustomizableCodingTemplate customizableCodingTemplate2 = (CustomizableCodingTemplate)codingWithHexInput.Model.Coding;
								customizableCodingTemplate2.ReadModeAndAddress = codingWithHexInput.Model.CustomReadAddress;
								customizableCodingTemplate2.WriteModeAndAddress = codingWithHexInput.Model.CustomWriteAddress;
							}
							codingWithHexInput.activityFrame.IsVisible = true;
							codingWithHexInput.lv.IsEnabled = false;
							Progress<string> progress = new Progress<string>(delegate(string s)
							{
								Device.BeginInvokeOnMainThread(new Action(new CodingWithHexInput.<>c__DisplayClass18_0
								{
									<>4__this = codingWithHexInput,
									s = s
								}.<btnSaveState_Clicked>b__1));
							});
							taskAwaiter6 = codingWithHexInput.Model.Coding.Execute(codingWithHexInput.entryPassword.Text, codingWithHexInput.Model.Data.Hex.Replace(" ", ""), codingWithHexInput.Model.Data.Hex, progress, null, false).GetAwaiter();
							if (!taskAwaiter6.IsCompleted)
							{
								num2 = 5;
								TaskAwaiter<CodingRequestResult> taskAwaiter7 = taskAwaiter6;
								this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<CodingRequestResult>, CodingWithHexInput.<btnSaveState_Clicked>d__18>(ref taskAwaiter6, ref this);
								return;
							}
							goto IL_0492;
						}
						break;
					}
					if (!taskAwaiter3.GetResult())
					{
						goto IL_0142;
					}
					Page inAppPage = InAppManager.GetInAppPage();
					taskAwaiter4 = codingWithHexInput.Navigation.PushAsync(inAppPage).GetAwaiter();
					if (!taskAwaiter4.IsCompleted)
					{
						num2 = 1;
						TaskAwaiter taskAwaiter5 = taskAwaiter4;
						this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, CodingWithHexInput.<btnSaveState_Clicked>d__18>(ref taskAwaiter4, ref this);
						return;
					}
					IL_013B:
					taskAwaiter4.GetResult();
					IL_0142:
					goto IL_05C6;
					IL_01F5:
					if (!taskAwaiter3.GetResult())
					{
						goto IL_026A;
					}
					Page inAppPage2 = InAppManager.GetInAppPage();
					taskAwaiter4 = codingWithHexInput.Navigation.PushAsync(inAppPage2).GetAwaiter();
					if (!taskAwaiter4.IsCompleted)
					{
						num2 = 3;
						TaskAwaiter taskAwaiter5 = taskAwaiter4;
						this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, CodingWithHexInput.<btnSaveState_Clicked>d__18>(ref taskAwaiter4, ref this);
						return;
					}
					IL_0263:
					taskAwaiter4.GetResult();
					IL_026A:
					goto IL_05C6;
					IL_038D:
					taskAwaiter4.GetResult();
					goto IL_05C6;
					IL_0492:
					CodingRequestResult result = taskAwaiter6.GetResult();
					if (result == CodingRequestResult.Success)
					{
						SharedSettings sharedSettings = SharedSettings.Current;
						int codingsCounter = sharedSettings.CodingsCounter;
						sharedSettings.CodingsCounter = codingsCounter + 1;
						goto IL_0528;
					}
					taskAwaiter4 = codingWithHexInput.DisplayAlert(Translate.GetString("coding_Error"), MQBAdaptationTemplate.CodingRequestResultToString(result), "OK").GetAwaiter();
					if (!taskAwaiter4.IsCompleted)
					{
						num2 = 6;
						TaskAwaiter taskAwaiter5 = taskAwaiter4;
						this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, CodingWithHexInput.<btnSaveState_Clicked>d__18>(ref taskAwaiter4, ref this);
						return;
					}
					IL_0521:
					taskAwaiter4.GetResult();
					IL_0528:
					taskAwaiter4 = codingWithHexInput.UpdateState().GetAwaiter();
					if (!taskAwaiter4.IsCompleted)
					{
						num2 = 7;
						TaskAwaiter taskAwaiter5 = taskAwaiter4;
						this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, CodingWithHexInput.<btnSaveState_Clicked>d__18>(ref taskAwaiter4, ref this);
						return;
					}
					IL_057C:
					taskAwaiter4.GetResult();
					codingWithHexInput.activityFrame.IsVisible = false;
					codingWithHexInput.activityFrame.Text = ActivityFrame.PLEASE_WAIT_TEXT;
					codingWithHexInput.lv.IsEnabled = true;
				}
				catch (Exception ex)
				{
					num2 = -2;
					this.<>t__builder.SetException(ex);
					return;
				}
				IL_05C6:
				num2 = -2;
				this.<>t__builder.SetResult();
			}

			// Token: 0x06004E31 RID: 20017 RVA: 0x003AC6F0 File Offset: 0x003AA8F0
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x04002EBD RID: 11965
			public int <>1__state;

			// Token: 0x04002EBE RID: 11966
			public AsyncVoidMethodBuilder <>t__builder;

			// Token: 0x04002EBF RID: 11967
			public CodingWithHexInput <>4__this;

			// Token: 0x04002EC0 RID: 11968
			private TaskAwaiter<bool> <>u__1;

			// Token: 0x04002EC1 RID: 11969
			private TaskAwaiter <>u__2;

			// Token: 0x04002EC2 RID: 11970
			private TaskAwaiter<CodingRequestResult> <>u__3;
		}

		// Token: 0x02000935 RID: 2357
		[CompilerGenerated]
		private sealed class <InitializeComponent>_anonXamlCDataTemplate_10
		{
			// Token: 0x06004E32 RID: 20018 RVA: 0x003AC700 File Offset: 0x003AA900
			public <InitializeComponent>_anonXamlCDataTemplate_10()
			{
			}

			// Token: 0x06004E33 RID: 20019 RVA: 0x003AC714 File Offset: 0x003AA914
			internal object LoadDataTemplate()
			{
				Translate translate;
				VisualDiagnostics.RegisterSourceInfo(translate = new Translate(), new Uri("Coding\\Pages\\CodingWithHexInput.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 124, 51);
				Span span;
				VisualDiagnostics.RegisterSourceInfo(span = new Span(), new Uri("Coding\\Pages\\CodingWithHexInput.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 124, 46);
				Span span2;
				VisualDiagnostics.RegisterSourceInfo(span2 = new Span(), new Uri("Coding\\Pages\\CodingWithHexInput.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 125, 46);
				BindingExtension bindingExtension;
				VisualDiagnostics.RegisterSourceInfo(bindingExtension = new BindingExtension(), new Uri("Coding\\Pages\\CodingWithHexInput.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 126, 51);
				Span span3;
				VisualDiagnostics.RegisterSourceInfo(span3 = new Span(), new Uri("Coding\\Pages\\CodingWithHexInput.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 126, 46);
				Span span4;
				VisualDiagnostics.RegisterSourceInfo(span4 = new Span(), new Uri("Coding\\Pages\\CodingWithHexInput.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 127, 46);
				FormattedString formattedString;
				VisualDiagnostics.RegisterSourceInfo(formattedString = new FormattedString(), new Uri("Coding\\Pages\\CodingWithHexInput.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 123, 42);
				Label label;
				VisualDiagnostics.RegisterSourceInfo(label = new Label(), new Uri("Coding\\Pages\\CodingWithHexInput.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 121, 34);
				Label label2;
				VisualDiagnostics.RegisterSourceInfo(label2 = new Label(), new Uri("Coding\\Pages\\CodingWithHexInput.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 132, 38);
				BindingExtension bindingExtension2;
				VisualDiagnostics.RegisterSourceInfo(bindingExtension2 = new BindingExtension(), new Uri("Coding\\Pages\\CodingWithHexInput.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 142, 41);
				Entry entry;
				VisualDiagnostics.RegisterSourceInfo(entry = new Entry(), new Uri("Coding\\Pages\\CodingWithHexInput.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 136, 38);
				StackLayout stackLayout;
				VisualDiagnostics.RegisterSourceInfo(stackLayout = new StackLayout(), new Uri("Coding\\Pages\\CodingWithHexInput.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 131, 34);
				Label label3;
				VisualDiagnostics.RegisterSourceInfo(label3 = new Label(), new Uri("Coding\\Pages\\CodingWithHexInput.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 148, 38);
				BindingExtension bindingExtension3;
				VisualDiagnostics.RegisterSourceInfo(bindingExtension3 = new BindingExtension(), new Uri("Coding\\Pages\\CodingWithHexInput.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 158, 41);
				Entry entry2;
				VisualDiagnostics.RegisterSourceInfo(entry2 = new Entry(), new Uri("Coding\\Pages\\CodingWithHexInput.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 152, 38);
				StackLayout stackLayout2;
				VisualDiagnostics.RegisterSourceInfo(stackLayout2 = new StackLayout(), new Uri("Coding\\Pages\\CodingWithHexInput.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 147, 34);
				ColumnDefinition columnDefinition;
				VisualDiagnostics.RegisterSourceInfo(columnDefinition = new ColumnDefinition(), new Uri("Coding\\Pages\\CodingWithHexInput.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 166, 46);
				ColumnDefinition columnDefinition2;
				VisualDiagnostics.RegisterSourceInfo(columnDefinition2 = new ColumnDefinition(), new Uri("Coding\\Pages\\CodingWithHexInput.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 167, 46);
				ColumnDefinition columnDefinition3;
				VisualDiagnostics.RegisterSourceInfo(columnDefinition3 = new ColumnDefinition(), new Uri("Coding\\Pages\\CodingWithHexInput.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 168, 46);
				ColumnDefinition columnDefinition4;
				VisualDiagnostics.RegisterSourceInfo(columnDefinition4 = new ColumnDefinition(), new Uri("Coding\\Pages\\CodingWithHexInput.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 169, 46);
				ColumnDefinition columnDefinition5;
				VisualDiagnostics.RegisterSourceInfo(columnDefinition5 = new ColumnDefinition(), new Uri("Coding\\Pages\\CodingWithHexInput.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 170, 46);
				ColumnDefinition columnDefinition6;
				VisualDiagnostics.RegisterSourceInfo(columnDefinition6 = new ColumnDefinition(), new Uri("Coding\\Pages\\CodingWithHexInput.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 171, 46);
				ColumnDefinition columnDefinition7;
				VisualDiagnostics.RegisterSourceInfo(columnDefinition7 = new ColumnDefinition(), new Uri("Coding\\Pages\\CodingWithHexInput.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 172, 46);
				ColumnDefinition columnDefinition8;
				VisualDiagnostics.RegisterSourceInfo(columnDefinition8 = new ColumnDefinition(), new Uri("Coding\\Pages\\CodingWithHexInput.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 173, 46);
				ColumnDefinition columnDefinition9;
				VisualDiagnostics.RegisterSourceInfo(columnDefinition9 = new ColumnDefinition(), new Uri("Coding\\Pages\\CodingWithHexInput.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 174, 46);
				RowDefinition rowDefinition;
				VisualDiagnostics.RegisterSourceInfo(rowDefinition = new RowDefinition(), new Uri("Coding\\Pages\\CodingWithHexInput.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 177, 46);
				RowDefinition rowDefinition2;
				VisualDiagnostics.RegisterSourceInfo(rowDefinition2 = new RowDefinition(), new Uri("Coding\\Pages\\CodingWithHexInput.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 178, 46);
				Label label4;
				VisualDiagnostics.RegisterSourceInfo(label4 = new Label(), new Uri("Coding\\Pages\\CodingWithHexInput.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 180, 42);
				Label label5;
				VisualDiagnostics.RegisterSourceInfo(label5 = new Label(), new Uri("Coding\\Pages\\CodingWithHexInput.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 185, 42);
				Label label6;
				VisualDiagnostics.RegisterSourceInfo(label6 = new Label(), new Uri("Coding\\Pages\\CodingWithHexInput.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 190, 42);
				Label label7;
				VisualDiagnostics.RegisterSourceInfo(label7 = new Label(), new Uri("Coding\\Pages\\CodingWithHexInput.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 195, 42);
				Label label8;
				VisualDiagnostics.RegisterSourceInfo(label8 = new Label(), new Uri("Coding\\Pages\\CodingWithHexInput.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 200, 42);
				Label label9;
				VisualDiagnostics.RegisterSourceInfo(label9 = new Label(), new Uri("Coding\\Pages\\CodingWithHexInput.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 205, 42);
				Label label10;
				VisualDiagnostics.RegisterSourceInfo(label10 = new Label(), new Uri("Coding\\Pages\\CodingWithHexInput.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 210, 42);
				Label label11;
				VisualDiagnostics.RegisterSourceInfo(label11 = new Label(), new Uri("Coding\\Pages\\CodingWithHexInput.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 215, 42);
				BindingExtension bindingExtension4;
				VisualDiagnostics.RegisterSourceInfo(bindingExtension4 = new BindingExtension(), new Uri("Coding\\Pages\\CodingWithHexInput.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 224, 45);
				CheckBoxWithColor checkBoxWithColor;
				VisualDiagnostics.RegisterSourceInfo(checkBoxWithColor = new CheckBoxWithColor(), new Uri("Coding\\Pages\\CodingWithHexInput.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 220, 42);
				BindingExtension bindingExtension5;
				VisualDiagnostics.RegisterSourceInfo(bindingExtension5 = new BindingExtension(), new Uri("Coding\\Pages\\CodingWithHexInput.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 229, 45);
				CheckBoxWithColor checkBoxWithColor2;
				VisualDiagnostics.RegisterSourceInfo(checkBoxWithColor2 = new CheckBoxWithColor(), new Uri("Coding\\Pages\\CodingWithHexInput.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 225, 42);
				BindingExtension bindingExtension6;
				VisualDiagnostics.RegisterSourceInfo(bindingExtension6 = new BindingExtension(), new Uri("Coding\\Pages\\CodingWithHexInput.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 234, 45);
				CheckBoxWithColor checkBoxWithColor3;
				VisualDiagnostics.RegisterSourceInfo(checkBoxWithColor3 = new CheckBoxWithColor(), new Uri("Coding\\Pages\\CodingWithHexInput.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 230, 42);
				BindingExtension bindingExtension7;
				VisualDiagnostics.RegisterSourceInfo(bindingExtension7 = new BindingExtension(), new Uri("Coding\\Pages\\CodingWithHexInput.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 239, 45);
				CheckBoxWithColor checkBoxWithColor4;
				VisualDiagnostics.RegisterSourceInfo(checkBoxWithColor4 = new CheckBoxWithColor(), new Uri("Coding\\Pages\\CodingWithHexInput.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 235, 42);
				BindingExtension bindingExtension8;
				VisualDiagnostics.RegisterSourceInfo(bindingExtension8 = new BindingExtension(), new Uri("Coding\\Pages\\CodingWithHexInput.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 244, 45);
				CheckBoxWithColor checkBoxWithColor5;
				VisualDiagnostics.RegisterSourceInfo(checkBoxWithColor5 = new CheckBoxWithColor(), new Uri("Coding\\Pages\\CodingWithHexInput.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 240, 42);
				BindingExtension bindingExtension9;
				VisualDiagnostics.RegisterSourceInfo(bindingExtension9 = new BindingExtension(), new Uri("Coding\\Pages\\CodingWithHexInput.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 249, 45);
				CheckBoxWithColor checkBoxWithColor6;
				VisualDiagnostics.RegisterSourceInfo(checkBoxWithColor6 = new CheckBoxWithColor(), new Uri("Coding\\Pages\\CodingWithHexInput.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 245, 42);
				BindingExtension bindingExtension10;
				VisualDiagnostics.RegisterSourceInfo(bindingExtension10 = new BindingExtension(), new Uri("Coding\\Pages\\CodingWithHexInput.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 254, 45);
				CheckBoxWithColor checkBoxWithColor7;
				VisualDiagnostics.RegisterSourceInfo(checkBoxWithColor7 = new CheckBoxWithColor(), new Uri("Coding\\Pages\\CodingWithHexInput.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 250, 42);
				BindingExtension bindingExtension11;
				VisualDiagnostics.RegisterSourceInfo(bindingExtension11 = new BindingExtension(), new Uri("Coding\\Pages\\CodingWithHexInput.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 259, 45);
				CheckBoxWithColor checkBoxWithColor8;
				VisualDiagnostics.RegisterSourceInfo(checkBoxWithColor8 = new CheckBoxWithColor(), new Uri("Coding\\Pages\\CodingWithHexInput.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 255, 42);
				Grid grid;
				VisualDiagnostics.RegisterSourceInfo(grid = new Grid(), new Uri("Coding\\Pages\\CodingWithHexInput.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 164, 38);
				ScrollView scrollView;
				VisualDiagnostics.RegisterSourceInfo(scrollView = new ScrollView(), new Uri("Coding\\Pages\\CodingWithHexInput.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 163, 34);
				StackLayout stackLayout3;
				VisualDiagnostics.RegisterSourceInfo(stackLayout3 = new StackLayout(), new Uri("Coding\\Pages\\CodingWithHexInput.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 120, 30);
				ViewCell viewCell;
				VisualDiagnostics.RegisterSourceInfo(viewCell = new ViewCell(), new Uri("Coding\\Pages\\CodingWithHexInput.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 119, 26);
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
				xmlNamespaceResolver.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
				xamlServiceProvider.Add(typeFromHandle2, new XamlTypeResolver(xmlNamespaceResolver, typeof(CodingWithHexInput.<InitializeComponent>_anonXamlCDataTemplate_10).GetTypeInfo().Assembly));
				xamlServiceProvider.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(124, 51)));
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
				checkBoxWithColor.SetValue(Grid.RowProperty, 1);
				checkBoxWithColor.SetValue(Grid.ColumnProperty, 0);
				checkBoxWithColor.SetValue(View.HorizontalOptionsProperty, LayoutOptions.Center);
				bindingExtension4.Mode = 1;
				bindingExtension4.Path = "Bit7";
				BindingBase bindingBase4 = bindingExtension4.ProvideValue(null);
				checkBoxWithColor.SetBinding(CheckBox.IsCheckedProperty, bindingBase4);
				grid.Children.Add(checkBoxWithColor);
				checkBoxWithColor2.SetValue(Grid.RowProperty, 1);
				checkBoxWithColor2.SetValue(Grid.ColumnProperty, 1);
				checkBoxWithColor2.SetValue(View.HorizontalOptionsProperty, LayoutOptions.Center);
				bindingExtension5.Mode = 1;
				bindingExtension5.Path = "Bit6";
				BindingBase bindingBase5 = bindingExtension5.ProvideValue(null);
				checkBoxWithColor2.SetBinding(CheckBox.IsCheckedProperty, bindingBase5);
				grid.Children.Add(checkBoxWithColor2);
				checkBoxWithColor3.SetValue(Grid.RowProperty, 1);
				checkBoxWithColor3.SetValue(Grid.ColumnProperty, 2);
				checkBoxWithColor3.SetValue(View.HorizontalOptionsProperty, LayoutOptions.Center);
				bindingExtension6.Mode = 1;
				bindingExtension6.Path = "Bit5";
				BindingBase bindingBase6 = bindingExtension6.ProvideValue(null);
				checkBoxWithColor3.SetBinding(CheckBox.IsCheckedProperty, bindingBase6);
				grid.Children.Add(checkBoxWithColor3);
				checkBoxWithColor4.SetValue(Grid.RowProperty, 1);
				checkBoxWithColor4.SetValue(Grid.ColumnProperty, 3);
				checkBoxWithColor4.SetValue(View.HorizontalOptionsProperty, LayoutOptions.Center);
				bindingExtension7.Mode = 1;
				bindingExtension7.Path = "Bit4";
				BindingBase bindingBase7 = bindingExtension7.ProvideValue(null);
				checkBoxWithColor4.SetBinding(CheckBox.IsCheckedProperty, bindingBase7);
				grid.Children.Add(checkBoxWithColor4);
				checkBoxWithColor5.SetValue(Grid.RowProperty, 1);
				checkBoxWithColor5.SetValue(Grid.ColumnProperty, 4);
				checkBoxWithColor5.SetValue(View.HorizontalOptionsProperty, LayoutOptions.Center);
				bindingExtension8.Mode = 1;
				bindingExtension8.Path = "Bit3";
				BindingBase bindingBase8 = bindingExtension8.ProvideValue(null);
				checkBoxWithColor5.SetBinding(CheckBox.IsCheckedProperty, bindingBase8);
				grid.Children.Add(checkBoxWithColor5);
				checkBoxWithColor6.SetValue(Grid.RowProperty, 1);
				checkBoxWithColor6.SetValue(Grid.ColumnProperty, 5);
				checkBoxWithColor6.SetValue(View.HorizontalOptionsProperty, LayoutOptions.Center);
				bindingExtension9.Mode = 1;
				bindingExtension9.Path = "Bit2";
				BindingBase bindingBase9 = bindingExtension9.ProvideValue(null);
				checkBoxWithColor6.SetBinding(CheckBox.IsCheckedProperty, bindingBase9);
				grid.Children.Add(checkBoxWithColor6);
				checkBoxWithColor7.SetValue(Grid.RowProperty, 1);
				checkBoxWithColor7.SetValue(Grid.ColumnProperty, 6);
				checkBoxWithColor7.SetValue(View.HorizontalOptionsProperty, LayoutOptions.Center);
				bindingExtension10.Mode = 1;
				bindingExtension10.Path = "Bit1";
				BindingBase bindingBase10 = bindingExtension10.ProvideValue(null);
				checkBoxWithColor7.SetBinding(CheckBox.IsCheckedProperty, bindingBase10);
				grid.Children.Add(checkBoxWithColor7);
				checkBoxWithColor8.SetValue(Grid.RowProperty, 1);
				checkBoxWithColor8.SetValue(Grid.ColumnProperty, 7);
				checkBoxWithColor8.SetValue(View.HorizontalOptionsProperty, LayoutOptions.Center);
				bindingExtension11.Mode = 1;
				bindingExtension11.Path = "Bit0";
				BindingBase bindingBase11 = bindingExtension11.ProvideValue(null);
				checkBoxWithColor8.SetBinding(CheckBox.IsCheckedProperty, bindingBase11);
				grid.Children.Add(checkBoxWithColor8);
				scrollView.Content = grid;
				stackLayout3.Children.Add(scrollView);
				viewCell.View = stackLayout3;
				return viewCell;
			}

			// Token: 0x04002EC3 RID: 11971
			internal object[] parentValues;

			// Token: 0x04002EC4 RID: 11972
			internal CodingWithHexInput root;
		}
	}
}
