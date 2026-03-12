using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using CarScannerXamarinForms.OBD2;
using Syncfusion.XForms.Buttons;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Internals;
using Xamarin.Forms.PlatformConfiguration.iOSSpecific;
using Xamarin.Forms.Xaml;
using Xamarin.Forms.Xaml.Diagnostics;
using Xamarin.Forms.Xaml.Internals;

namespace CarScannerXamarinForms.Settings
{
	// Token: 0x020001F2 RID: 498
	[XamlCompilation(2)]
	[XamlFilePath("Settings\\ContactDeveloperPage.xaml")]
	public class ContactDeveloperPage : ContentPage
	{
		// Token: 0x06001A07 RID: 6663 RVA: 0x00113132 File Offset: 0x00111332
		public ContactDeveloperPage()
		{
			this.InitializeComponent();
		}

		// Token: 0x06001A08 RID: 6664 RVA: 0x00113140 File Offset: 0x00111340
		private async void BtnSend_Clicked(object sender, EventArgs e)
		{
			bool flag = false;
			int num;
			if (this.rbPurchaseIssue.IsChecked.GetValueOrDefault())
			{
				flag = true;
			}
			else if (!string.IsNullOrEmpty(this.entryBrand.Text) && !string.IsNullOrEmpty(this.entryModel.Text) && !string.IsNullOrEmpty(this.entryYear.Text) && int.TryParse(this.entryYear.Text, out num) && !string.IsNullOrEmpty(this.entryEngineVolume.Text) && this.pickerFuelType.SelectedIndex >= 0 && this.pickerTransmissionType.SelectedIndex >= 0)
			{
				flag = true;
			}
			if (!flag)
			{
				await base.DisplayAlert(Translate.GetString("cdp_FIllCarDetails"), "", "OK");
			}
			else if (string.IsNullOrEmpty(this.editorDescribeProblem.Text) || string.IsNullOrEmpty(this.editorDescribeProblem.Text.Trim()))
			{
				await base.DisplayAlert(Translate.GetString("cdp_DescribeProblem"), "", "OK");
			}
			else
			{
				string issue_type = "[Problem type: ";
				if (this.rbConnectionIssue.IsChecked.GetValueOrDefault())
				{
					issue_type += "connection]\n";
				}
				else if (this.rbPurchaseIssue.IsChecked.GetValueOrDefault())
				{
					issue_type += "purchase]\n";
				}
				else if (this.rbOther.IsChecked.GetValueOrDefault())
				{
					issue_type += "other]\n";
				}
				string user_info = "[Problem description]\n" + this.editorDescribeProblem.Text + "\n";
				bool? isChecked = this.rbPurchaseIssue.IsChecked;
				bool flag2 = false;
				if ((isChecked.GetValueOrDefault() == flag2) & (isChecked != null))
				{
					user_info = string.Concat(new string[]
					{
						user_info,
						"\n[Car details:]\nBrand: ",
						this.entryBrand.Text,
						"\nModel: ",
						this.entryModel.Text,
						"\nYear: ",
						this.entryYear.Text,
						"\nEngine volume:",
						this.entryEngineVolume.Text,
						"\nEngine model:",
						this.entryEngineModel.Text,
						"\nFuel type: ",
						StaticLists.FuelTypesList[this.pickerFuelType.SelectedIndex],
						"\n",
						string.Format("Transmission: {0}\n", this.pickerTransmissionType.SelectedItem),
						"Other: ",
						this.entryOtherInfo.Text,
						"\n"
					});
				}
				string text = await SharedSettings.Current.GetSettingsReport();
				string text2 = string.Concat(new string[] { issue_type, "\n", user_info, "\n", text });
				await this.SendEmail(text2);
				await base.Navigation.PopAsync();
			}
		}

		// Token: 0x06001A09 RID: 6665 RVA: 0x00113178 File Offset: 0x00111378
		private async Task SendEmail(string text)
		{
			int num = 0;
			try
			{
				EmailMessage message = new EmailMessage(" (" + Device.RuntimePlatform + ")", text, new string[] { "admin@carscanner.info" });
				message.BodyFormat = 0;
				message.Subject = "Car Scanner user feedback" + message.Subject;
				try
				{
					await PCLDebugStream.CurrentInstance.Flush();
					PCLDebugStream.CurrentInstance.Close();
				}
				catch (Exception)
				{
				}
				string filepath = PCLDebugStream.GetFilepath();
				if (File.Exists(filepath))
				{
					message.Attachments.Add(new EmailAttachment(filepath));
				}
				await base.DisplayAlert(base.Title, Translate.GetString("contactDeveloper_PreSendMessage"), "OK");
				await Email.ComposeAsync(message);
				message = null;
			}
			catch (Exception obj)
			{
				num = 1;
			}
			object obj;
			if (num == 1)
			{
				Exception exc = (Exception)obj;
				try
				{
					await App.OBDReader.DebugWrite("\r\n[///**** CONTACT DEVELOPER REPORT:");
					await App.OBDReader.DebugWrite(text);
					await App.OBDReader.DebugWrite("\r\n[***/// END OF CONTACT DEVELOPER REPORT]");
				}
				catch (Exception)
				{
				}
				try
				{
					await PCLDebugStream.CurrentInstance.Flush();
					PCLDebugStream.CurrentInstance.Close();
				}
				catch (Exception)
				{
				}
				try
				{
					await App.GetCurrentPage().DisplayAlert("Please send this log file to admin@carscanner.info", "Please send email to:\nadmin@carscanner.info including this log file.", "OK");
				}
				catch (Exception)
				{
				}
				try
				{
					await Share.RequestAsync(new ShareFileRequest(new ShareFile(PCLDebugStream.GetFilepath())));
				}
				catch (Exception)
				{
					base.DisplayAlert("Error", exc.Message, "OK");
				}
				exc = null;
			}
			obj = null;
		}

		// Token: 0x06001A0A RID: 6666 RVA: 0x001131C4 File Offset: 0x001113C4
		private async void BtnProblemNotSolved_Clicked(object sender, EventArgs e)
		{
			this.panelProblemSolved.IsVisible = false;
			this.panelDescribeYourProblem.IsVisible = true;
			if (this.rbPurchaseIssue.IsChecked.GetValueOrDefault())
			{
				this.btnNext.IsVisible = false;
				this.btnSend.IsVisible = true;
				await Task.Delay(200);
				this.scrollView.ScrollToAsync(this.btnNext, 0, true);
			}
			else
			{
				this.btnNext.IsVisible = true;
				await Task.Delay(200);
				this.scrollView.ScrollToAsync(this.btnNext, 0, true);
			}
		}

		// Token: 0x06001A0B RID: 6667 RVA: 0x00026430 File Offset: 0x00024630
		private void BtnProblemSolved_Clicked(object sender, EventArgs e)
		{
			base.Navigation.PopAsync();
		}

		// Token: 0x06001A0C RID: 6668 RVA: 0x001131FC File Offset: 0x001113FC
		private async void BtnNext_Clicked(object sender, EventArgs e)
		{
			if (string.IsNullOrEmpty(this.editorDescribeProblem.Text) || string.IsNullOrEmpty(this.editorDescribeProblem.Text.Trim()))
			{
				await base.DisplayAlert(Translate.GetString("cdp_DescribeProblem"), "", "OK");
			}
			else
			{
				this.btnNext.IsVisible = false;
				this.panelCarDetails.IsVisible = true;
				this.panelProblemSolved.IsVisible = false;
				this.panelConnectionProblems.IsVisible = false;
				this.panelPurchaseProblems.IsVisible = false;
				this.btnSend.IsVisible = true;
				this.panelDescribeYourProblem.IsVisible = true;
				await Task.Delay(200);
				this.scrollView.ScrollToAsync(this.btnSend, 0, true);
			}
		}

		// Token: 0x06001A0D RID: 6669 RVA: 0x00113234 File Offset: 0x00111434
		private async void RbIssue_StateChanged(object sender, StateChangedEventArgs e)
		{
			if (this.rbConnectionIssue.IsChecked.GetValueOrDefault())
			{
				this.panelConnectionProblems.IsVisible = true;
				this.panelCarDetails.IsVisible = false;
				this.panelProblemSolved.IsVisible = true;
				this.panelPurchaseProblems.IsVisible = false;
				this.btnSend.IsVisible = false;
				this.panelDescribeYourProblem.IsVisible = false;
				this.btnNext.IsVisible = true;
				this.panelNoCoding.IsVisible = false;
			}
			else if (this.rbPurchaseIssue.IsChecked.GetValueOrDefault())
			{
				this.panelConnectionProblems.IsVisible = false;
				this.panelCarDetails.IsVisible = false;
				this.panelProblemSolved.IsVisible = true;
				this.panelPurchaseProblems.IsVisible = true;
				this.btnSend.IsVisible = false;
				this.panelDescribeYourProblem.IsVisible = false;
				this.btnNext.IsVisible = false;
				this.panelNoCoding.IsVisible = false;
				if (SharedSettings.Current.AdsProductPurchased)
				{
					this.panelProblemSolved.IsVisible = false;
				}
				await Task.Delay(200);
				this.scrollView.ScrollToAsync(this.btnSend, 0, true);
			}
			else if (this.rbOther.IsChecked.GetValueOrDefault())
			{
				this.panelConnectionProblems.IsVisible = false;
				this.panelCarDetails.IsVisible = false;
				this.panelProblemSolved.IsVisible = false;
				this.panelPurchaseProblems.IsVisible = false;
				this.btnSend.IsVisible = false;
				this.panelDescribeYourProblem.IsVisible = true;
				this.btnNext.IsVisible = true;
				this.panelNoCoding.IsVisible = false;
				await Task.Delay(200);
				this.scrollView.ScrollToAsync(this.btnSend, 0, true);
			}
			else if (this.rbNoCoding.IsChecked.GetValueOrDefault())
			{
				this.panelConnectionProblems.IsVisible = false;
				this.panelCarDetails.IsVisible = false;
				this.panelProblemSolved.IsVisible = false;
				this.panelPurchaseProblems.IsVisible = false;
				this.btnSend.IsVisible = false;
				this.panelDescribeYourProblem.IsVisible = false;
				this.btnNext.IsVisible = false;
				this.panelNoCoding.IsVisible = true;
				await Task.Delay(200);
				this.scrollView.ScrollToAsync(this.btnSend, 0, true);
			}
		}

		// Token: 0x06001A0E RID: 6670 RVA: 0x0011326C File Offset: 0x0011146C
		private async void labelNoCoding_Tapped(object sender, EventArgs e)
		{
			try
			{
				Launcher.TryOpenAsync("https://www.carscanner.info/coding/");
			}
			catch (Exception)
			{
			}
		}

		// Token: 0x06001A0F RID: 6671 RVA: 0x0011329C File Offset: 0x0011149C
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private void InitializeComponent()
		{
			if (ResourceLoader.IsEnabled && ResourceLoader.CanProvideContentFor(new ResourceLoader.ResourceLoadingQuery
			{
				AssemblyName = typeof(ContactDeveloperPage).GetTypeInfo().Assembly.GetName(),
				ResourcePath = "Settings/ContactDeveloperPage.xaml",
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
			VisualDiagnostics.RegisterSourceInfo(translate = new Translate(), new Uri("Settings\\ContactDeveloperPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 12, 5);
			DynamicResourceExtension dynamicResourceExtension;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension = new DynamicResourceExtension(), new Uri("Settings\\ContactDeveloperPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 14, 5);
			BoolToNegativeConverter boolToNegativeConverter;
			VisualDiagnostics.RegisterSourceInfo(boolToNegativeConverter = new BoolToNegativeConverter(), new Uri("Settings\\ContactDeveloperPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 18, 14);
			ResourceDictionary resourceDictionary;
			VisualDiagnostics.RegisterSourceInfo(resourceDictionary = new ResourceDictionary(), new Uri("Settings\\ContactDeveloperPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 17, 10);
			RowDefinition rowDefinition;
			VisualDiagnostics.RegisterSourceInfo(rowDefinition = new RowDefinition(), new Uri("Settings\\ContactDeveloperPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 26, 18);
			RowDefinition rowDefinition2;
			VisualDiagnostics.RegisterSourceInfo(rowDefinition2 = new RowDefinition(), new Uri("Settings\\ContactDeveloperPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 27, 18);
			Translate translate2;
			VisualDiagnostics.RegisterSourceInfo(translate2 = new Translate(), new Uri("Settings\\ContactDeveloperPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 35, 28);
			Label label;
			VisualDiagnostics.RegisterSourceInfo(label = new Label(), new Uri("Settings\\ContactDeveloperPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 35, 22);
			Translate translate3;
			VisualDiagnostics.RegisterSourceInfo(translate3 = new Translate(), new Uri("Settings\\ContactDeveloperPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 41, 29);
			SfRadioButton sfRadioButton;
			VisualDiagnostics.RegisterSourceInfo(sfRadioButton = new SfRadioButton(), new Uri("Settings\\ContactDeveloperPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 37, 26);
			Translate translate4;
			VisualDiagnostics.RegisterSourceInfo(translate4 = new Translate(), new Uri("Settings\\ContactDeveloperPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 46, 29);
			SfRadioButton sfRadioButton2;
			VisualDiagnostics.RegisterSourceInfo(sfRadioButton2 = new SfRadioButton(), new Uri("Settings\\ContactDeveloperPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 42, 26);
			Translate translate5;
			VisualDiagnostics.RegisterSourceInfo(translate5 = new Translate(), new Uri("Settings\\ContactDeveloperPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 51, 29);
			SfRadioButton sfRadioButton3;
			VisualDiagnostics.RegisterSourceInfo(sfRadioButton3 = new SfRadioButton(), new Uri("Settings\\ContactDeveloperPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 47, 26);
			Translate translate6;
			VisualDiagnostics.RegisterSourceInfo(translate6 = new Translate(), new Uri("Settings\\ContactDeveloperPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 57, 29);
			SfRadioButton sfRadioButton4;
			VisualDiagnostics.RegisterSourceInfo(sfRadioButton4 = new SfRadioButton(), new Uri("Settings\\ContactDeveloperPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 53, 26);
			SfRadioGroup sfRadioGroup;
			VisualDiagnostics.RegisterSourceInfo(sfRadioGroup = new SfRadioGroup(), new Uri("Settings\\ContactDeveloperPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 36, 22);
			SharedSettings sharedSettings;
			VisualDiagnostics.RegisterSourceInfo(sharedSettings = SharedSettings.Current, new Uri("Settings\\ContactDeveloperPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 63, 25);
			StaticResourceExtension staticResourceExtension;
			VisualDiagnostics.RegisterSourceInfo(staticResourceExtension = new StaticResourceExtension(), new Uri("Settings\\ContactDeveloperPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 66, 38);
			BindingExtension bindingExtension;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension = new BindingExtension(), new Uri("Settings\\ContactDeveloperPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 66, 38);
			Translate translate7;
			VisualDiagnostics.RegisterSourceInfo(translate7 = new Translate(), new Uri("Settings\\ContactDeveloperPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 67, 36);
			On on;
			VisualDiagnostics.RegisterSourceInfo(on = new On(), new Uri("Settings\\ContactDeveloperPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 70, 42);
			On on2;
			VisualDiagnostics.RegisterSourceInfo(on2 = new On(), new Uri("Settings\\ContactDeveloperPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 71, 42);
			OnPlatform<bool> onPlatform;
			VisualDiagnostics.RegisterSourceInfo(onPlatform = new OnPlatform<bool>(), new Uri("Settings\\ContactDeveloperPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 69, 38);
			Label label2;
			VisualDiagnostics.RegisterSourceInfo(label2 = new Label(), new Uri("Settings\\ContactDeveloperPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 67, 30);
			Translate translate8;
			VisualDiagnostics.RegisterSourceInfo(translate8 = new Translate(), new Uri("Settings\\ContactDeveloperPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 76, 36);
			On on3;
			VisualDiagnostics.RegisterSourceInfo(on3 = new On(), new Uri("Settings\\ContactDeveloperPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 79, 42);
			On on4;
			VisualDiagnostics.RegisterSourceInfo(on4 = new On(), new Uri("Settings\\ContactDeveloperPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 80, 42);
			OnPlatform<bool> onPlatform2;
			VisualDiagnostics.RegisterSourceInfo(onPlatform2 = new OnPlatform<bool>(), new Uri("Settings\\ContactDeveloperPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 78, 38);
			Label label3;
			VisualDiagnostics.RegisterSourceInfo(label3 = new Label(), new Uri("Settings\\ContactDeveloperPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 76, 30);
			StackLayout stackLayout;
			VisualDiagnostics.RegisterSourceInfo(stackLayout = new StackLayout(), new Uri("Settings\\ContactDeveloperPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 66, 26);
			BindingExtension bindingExtension2;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension2 = new BindingExtension(), new Uri("Settings\\ContactDeveloperPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 89, 29);
			Translate translate9;
			VisualDiagnostics.RegisterSourceInfo(translate9 = new Translate(), new Uri("Settings\\ContactDeveloperPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 90, 29);
			Label label4;
			VisualDiagnostics.RegisterSourceInfo(label4 = new Label(), new Uri("Settings\\ContactDeveloperPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 86, 26);
			StackLayout stackLayout2;
			VisualDiagnostics.RegisterSourceInfo(stackLayout2 = new StackLayout(), new Uri("Settings\\ContactDeveloperPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 60, 22);
			Translate translate10;
			VisualDiagnostics.RegisterSourceInfo(translate10 = new Translate(), new Uri("Settings\\ContactDeveloperPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 98, 61);
			Label label5;
			VisualDiagnostics.RegisterSourceInfo(label5 = new Label(), new Uri("Settings\\ContactDeveloperPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 98, 26);
			StackLayout stackLayout3;
			VisualDiagnostics.RegisterSourceInfo(stackLayout3 = new StackLayout(), new Uri("Settings\\ContactDeveloperPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 94, 22);
			Translate translate11;
			VisualDiagnostics.RegisterSourceInfo(translate11 = new Translate(), new Uri("Settings\\ContactDeveloperPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 107, 32);
			Label label6;
			VisualDiagnostics.RegisterSourceInfo(label6 = new Label(), new Uri("Settings\\ContactDeveloperPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 107, 26);
			Translate translate12;
			VisualDiagnostics.RegisterSourceInfo(translate12 = new Translate(), new Uri("Settings\\ContactDeveloperPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 111, 29);
			Button button;
			VisualDiagnostics.RegisterSourceInfo(button = new Button(), new Uri("Settings\\ContactDeveloperPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 108, 26);
			Translate translate13;
			VisualDiagnostics.RegisterSourceInfo(translate13 = new Translate(), new Uri("Settings\\ContactDeveloperPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 115, 29);
			Button button2;
			VisualDiagnostics.RegisterSourceInfo(button2 = new Button(), new Uri("Settings\\ContactDeveloperPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 112, 26);
			StackLayout stackLayout4;
			VisualDiagnostics.RegisterSourceInfo(stackLayout4 = new StackLayout(), new Uri("Settings\\ContactDeveloperPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 103, 22);
			DynamicResourceExtension dynamicResourceExtension2;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension2 = new DynamicResourceExtension(), new Uri("Settings\\ContactDeveloperPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 125, 29);
			Translate translate14;
			VisualDiagnostics.RegisterSourceInfo(translate14 = new Translate(), new Uri("Settings\\ContactDeveloperPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 128, 29);
			DynamicResourceExtension dynamicResourceExtension3;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension3 = new DynamicResourceExtension(), new Uri("Settings\\ContactDeveloperPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 129, 29);
			DynamicResourceExtension dynamicResourceExtension4;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension4 = new DynamicResourceExtension(), new Uri("Settings\\ContactDeveloperPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 130, 29);
			Editor editor;
			VisualDiagnostics.RegisterSourceInfo(editor = new Editor(), new Uri("Settings\\ContactDeveloperPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 123, 26);
			Translate translate15;
			VisualDiagnostics.RegisterSourceInfo(translate15 = new Translate(), new Uri("Settings\\ContactDeveloperPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 134, 29);
			Button button3;
			VisualDiagnostics.RegisterSourceInfo(button3 = new Button(), new Uri("Settings\\ContactDeveloperPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 131, 26);
			StackLayout stackLayout5;
			VisualDiagnostics.RegisterSourceInfo(stackLayout5 = new StackLayout(), new Uri("Settings\\ContactDeveloperPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 119, 22);
			Translate translate16;
			VisualDiagnostics.RegisterSourceInfo(translate16 = new Translate(), new Uri("Settings\\ContactDeveloperPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 142, 32);
			Label label7;
			VisualDiagnostics.RegisterSourceInfo(label7 = new Label(), new Uri("Settings\\ContactDeveloperPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 142, 26);
			Translate translate17;
			VisualDiagnostics.RegisterSourceInfo(translate17 = new Translate(), new Uri("Settings\\ContactDeveloperPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 143, 52);
			Entry entry;
			VisualDiagnostics.RegisterSourceInfo(entry = new Entry(), new Uri("Settings\\ContactDeveloperPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 143, 26);
			Translate translate18;
			VisualDiagnostics.RegisterSourceInfo(translate18 = new Translate(), new Uri("Settings\\ContactDeveloperPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 144, 52);
			Entry entry2;
			VisualDiagnostics.RegisterSourceInfo(entry2 = new Entry(), new Uri("Settings\\ContactDeveloperPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 144, 26);
			Translate translate19;
			VisualDiagnostics.RegisterSourceInfo(translate19 = new Translate(), new Uri("Settings\\ContactDeveloperPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 145, 51);
			Entry entry3;
			VisualDiagnostics.RegisterSourceInfo(entry3 = new Entry(), new Uri("Settings\\ContactDeveloperPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 145, 26);
			Translate translate20;
			VisualDiagnostics.RegisterSourceInfo(translate20 = new Translate(), new Uri("Settings\\ContactDeveloperPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 146, 59);
			Entry entry4;
			VisualDiagnostics.RegisterSourceInfo(entry4 = new Entry(), new Uri("Settings\\ContactDeveloperPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 146, 26);
			Translate translate21;
			VisualDiagnostics.RegisterSourceInfo(translate21 = new Translate(), new Uri("Settings\\ContactDeveloperPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 147, 58);
			Entry entry5;
			VisualDiagnostics.RegisterSourceInfo(entry5 = new Entry(), new Uri("Settings\\ContactDeveloperPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 147, 26);
			Translate translate22;
			VisualDiagnostics.RegisterSourceInfo(translate22 = new Translate(), new Uri("Settings\\ContactDeveloperPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 148, 32);
			Label label8;
			VisualDiagnostics.RegisterSourceInfo(label8 = new Label(), new Uri("Settings\\ContactDeveloperPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 148, 26);
			List<string> fuelTypesList;
			VisualDiagnostics.RegisterSourceInfo(fuelTypesList = StaticLists.FuelTypesList, new Uri("Settings\\ContactDeveloperPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 151, 29);
			BindingExtension bindingExtension3;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension3 = new BindingExtension(), new Uri("Settings\\ContactDeveloperPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 151, 29);
			Picker picker;
			VisualDiagnostics.RegisterSourceInfo(picker = new Picker(), new Uri("Settings\\ContactDeveloperPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 149, 26);
			Translate translate23;
			VisualDiagnostics.RegisterSourceInfo(translate23 = new Translate(), new Uri("Settings\\ContactDeveloperPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 153, 32);
			Label label9;
			VisualDiagnostics.RegisterSourceInfo(label9 = new Label(), new Uri("Settings\\ContactDeveloperPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 153, 26);
			Type typeFromHandle;
			VisualDiagnostics.RegisterSourceInfo(typeFromHandle = typeof(string), new Uri("Settings\\ContactDeveloperPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 156, 42);
			string text = "MT";
			string text2 = "AT";
			string text3 = "CVT";
			string text4 = "Robot (DSG, PDK, DCT, PowerShift, etc.)";
			string text5 = "Other";
			ArrayExtension arrayExtension;
			(arrayExtension = new ArrayExtension()).Type = typeFromHandle;
			arrayExtension.Items.Add(text);
			arrayExtension.Items.Add(text2);
			arrayExtension.Items.Add(text3);
			arrayExtension.Items.Add(text4);
			arrayExtension.Items.Add(text5);
			string[] array;
			VisualDiagnostics.RegisterSourceInfo(array = new string[] { text, text2, text3, text4, text5 }, new Uri("Settings\\ContactDeveloperPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 156, 34);
			Picker picker2;
			VisualDiagnostics.RegisterSourceInfo(picker2 = new Picker(), new Uri("Settings\\ContactDeveloperPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 154, 26);
			Translate translate24;
			VisualDiagnostics.RegisterSourceInfo(translate24 = new Translate(), new Uri("Settings\\ContactDeveloperPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 165, 56);
			Entry entry6;
			VisualDiagnostics.RegisterSourceInfo(entry6 = new Entry(), new Uri("Settings\\ContactDeveloperPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 165, 26);
			StackLayout stackLayout6;
			VisualDiagnostics.RegisterSourceInfo(stackLayout6 = new StackLayout(), new Uri("Settings\\ContactDeveloperPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 138, 22);
			Translate translate25;
			VisualDiagnostics.RegisterSourceInfo(translate25 = new Translate(), new Uri("Settings\\ContactDeveloperPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 173, 59);
			TapGestureRecognizer tapGestureRecognizer;
			VisualDiagnostics.RegisterSourceInfo(tapGestureRecognizer = new TapGestureRecognizer(), new Uri("Settings\\ContactDeveloperPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 175, 34);
			Label label10;
			VisualDiagnostics.RegisterSourceInfo(label10 = new Label(), new Uri("Settings\\ContactDeveloperPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 173, 26);
			StackLayout stackLayout7;
			VisualDiagnostics.RegisterSourceInfo(stackLayout7 = new StackLayout(), new Uri("Settings\\ContactDeveloperPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 169, 22);
			Translate translate26;
			VisualDiagnostics.RegisterSourceInfo(translate26 = new Translate(), new Uri("Settings\\ContactDeveloperPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 186, 25);
			Button button4;
			VisualDiagnostics.RegisterSourceInfo(button4 = new Button(), new Uri("Settings\\ContactDeveloperPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 180, 22);
			StackLayout stackLayout8;
			VisualDiagnostics.RegisterSourceInfo(stackLayout8 = new StackLayout(), new Uri("Settings\\ContactDeveloperPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 33, 18);
			ScrollView scrollView;
			VisualDiagnostics.RegisterSourceInfo(scrollView = new ScrollView(), new Uri("Settings\\ContactDeveloperPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 29, 14);
			Grid grid;
			VisualDiagnostics.RegisterSourceInfo(grid = new Grid(), new Uri("Settings\\ContactDeveloperPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 24, 10);
			VisualDiagnostics.RegisterSourceInfo(this, new Uri("Settings\\ContactDeveloperPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 2, 2);
			NameScope nameScope = NameScope.GetNameScope(this) ?? new NameScope();
			NameScope.SetNameScope(this, nameScope);
			nameScope.RegisterName("scrollView", scrollView);
			if (scrollView.StyleId == null)
			{
				scrollView.StyleId = "scrollView";
			}
			nameScope.RegisterName("rbConnectionIssue", sfRadioButton);
			if (sfRadioButton.StyleId == null)
			{
				sfRadioButton.StyleId = "rbConnectionIssue";
			}
			nameScope.RegisterName("rbPurchaseIssue", sfRadioButton2);
			if (sfRadioButton2.StyleId == null)
			{
				sfRadioButton2.StyleId = "rbPurchaseIssue";
			}
			nameScope.RegisterName("rbOther", sfRadioButton3);
			if (sfRadioButton3.StyleId == null)
			{
				sfRadioButton3.StyleId = "rbOther";
			}
			nameScope.RegisterName("rbNoCoding", sfRadioButton4);
			if (sfRadioButton4.StyleId == null)
			{
				sfRadioButton4.StyleId = "rbNoCoding";
			}
			nameScope.RegisterName("panelPurchaseProblems", stackLayout2);
			if (stackLayout2.StyleId == null)
			{
				stackLayout2.StyleId = "panelPurchaseProblems";
			}
			nameScope.RegisterName("panelConnectionProblems", stackLayout3);
			if (stackLayout3.StyleId == null)
			{
				stackLayout3.StyleId = "panelConnectionProblems";
			}
			nameScope.RegisterName("labelConnectionTips", label5);
			if (label5.StyleId == null)
			{
				label5.StyleId = "labelConnectionTips";
			}
			nameScope.RegisterName("panelProblemSolved", stackLayout4);
			if (stackLayout4.StyleId == null)
			{
				stackLayout4.StyleId = "panelProblemSolved";
			}
			nameScope.RegisterName("btnProblemSolved", button);
			if (button.StyleId == null)
			{
				button.StyleId = "btnProblemSolved";
			}
			nameScope.RegisterName("btnProblemNotSolved", button2);
			if (button2.StyleId == null)
			{
				button2.StyleId = "btnProblemNotSolved";
			}
			nameScope.RegisterName("panelDescribeYourProblem", stackLayout5);
			if (stackLayout5.StyleId == null)
			{
				stackLayout5.StyleId = "panelDescribeYourProblem";
			}
			nameScope.RegisterName("editorDescribeProblem", editor);
			if (editor.StyleId == null)
			{
				editor.StyleId = "editorDescribeProblem";
			}
			nameScope.RegisterName("btnNext", button3);
			if (button3.StyleId == null)
			{
				button3.StyleId = "btnNext";
			}
			nameScope.RegisterName("panelCarDetails", stackLayout6);
			if (stackLayout6.StyleId == null)
			{
				stackLayout6.StyleId = "panelCarDetails";
			}
			nameScope.RegisterName("entryBrand", entry);
			if (entry.StyleId == null)
			{
				entry.StyleId = "entryBrand";
			}
			nameScope.RegisterName("entryModel", entry2);
			if (entry2.StyleId == null)
			{
				entry2.StyleId = "entryModel";
			}
			nameScope.RegisterName("entryYear", entry3);
			if (entry3.StyleId == null)
			{
				entry3.StyleId = "entryYear";
			}
			nameScope.RegisterName("entryEngineVolume", entry4);
			if (entry4.StyleId == null)
			{
				entry4.StyleId = "entryEngineVolume";
			}
			nameScope.RegisterName("entryEngineModel", entry5);
			if (entry5.StyleId == null)
			{
				entry5.StyleId = "entryEngineModel";
			}
			nameScope.RegisterName("pickerFuelType", picker);
			if (picker.StyleId == null)
			{
				picker.StyleId = "pickerFuelType";
			}
			nameScope.RegisterName("pickerTransmissionType", picker2);
			if (picker2.StyleId == null)
			{
				picker2.StyleId = "pickerTransmissionType";
			}
			nameScope.RegisterName("entryOtherInfo", entry6);
			if (entry6.StyleId == null)
			{
				entry6.StyleId = "entryOtherInfo";
			}
			nameScope.RegisterName("panelNoCoding", stackLayout7);
			if (stackLayout7.StyleId == null)
			{
				stackLayout7.StyleId = "panelNoCoding";
			}
			nameScope.RegisterName("labelNoCodingText", label10);
			if (label10.StyleId == null)
			{
				label10.StyleId = "labelNoCodingText";
			}
			nameScope.RegisterName("btnSend", button4);
			if (button4.StyleId == null)
			{
				button4.StyleId = "btnSend";
			}
			this.scrollView = scrollView;
			this.rbConnectionIssue = sfRadioButton;
			this.rbPurchaseIssue = sfRadioButton2;
			this.rbOther = sfRadioButton3;
			this.rbNoCoding = sfRadioButton4;
			this.panelPurchaseProblems = stackLayout2;
			this.panelConnectionProblems = stackLayout3;
			this.labelConnectionTips = label5;
			this.panelProblemSolved = stackLayout4;
			this.btnProblemSolved = button;
			this.btnProblemNotSolved = button2;
			this.panelDescribeYourProblem = stackLayout5;
			this.editorDescribeProblem = editor;
			this.btnNext = button3;
			this.panelCarDetails = stackLayout6;
			this.entryBrand = entry;
			this.entryModel = entry2;
			this.entryYear = entry3;
			this.entryEngineVolume = entry4;
			this.entryEngineModel = entry5;
			this.pickerFuelType = picker;
			this.pickerTransmissionType = picker2;
			this.entryOtherInfo = entry6;
			this.panelNoCoding = stackLayout7;
			this.labelNoCodingText = label10;
			this.btnSend = button4;
			this.Resources = resourceDictionary;
			resourceDictionary.Add("BoolToNegativeConverter", boolToNegativeConverter);
			translate.Text = "ios_ContactDeveloper";
			IMarkupExtension markupExtension = translate;
			XamlServiceProvider xamlServiceProvider = new XamlServiceProvider();
			Type typeFromHandle2 = typeof(IProvideValueTarget);
			object[] array2 = new object[0 + 1];
			array2[0] = this;
			object obj;
			xamlServiceProvider.Add(typeFromHandle2, obj = new SimpleValueTargetProvider(array2, Page.TitleProperty, nameScope));
			xamlServiceProvider.Add(typeof(IReferenceProvider), obj);
			Type typeFromHandle3 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver = new XmlNamespaceResolver();
			xmlNamespaceResolver.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver.Add("d", "http://xamarin.com/schemas/2014/forms/design");
			xmlNamespaceResolver.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver.Add("mc", "http://schemas.openxmlformats.org/markup-compatibility/2006");
			xmlNamespaceResolver.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver.Add("syncfusionRB", "clr-namespace:Syncfusion.XForms.Buttons;assembly=Syncfusion.Buttons.XForms");
			xamlServiceProvider.Add(typeFromHandle3, new XamlTypeResolver(xmlNamespaceResolver, typeof(ContactDeveloperPage).GetTypeInfo().Assembly));
			xamlServiceProvider.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(12, 5)));
			object obj2 = markupExtension.ProvideValue(xamlServiceProvider);
			this.Title = obj2;
			this.SetValue(Page.UseSafeAreaProperty, true);
			dynamicResourceExtension.Key = "BackgroundColor";
			IMarkupExtension<DynamicResource> markupExtension2 = dynamicResourceExtension;
			XamlServiceProvider xamlServiceProvider2 = new XamlServiceProvider();
			Type typeFromHandle4 = typeof(IProvideValueTarget);
			object[] array3 = new object[0 + 1];
			array3[0] = this;
			object obj3;
			xamlServiceProvider2.Add(typeFromHandle4, obj3 = new SimpleValueTargetProvider(array3, VisualElement.BackgroundColorProperty, nameScope));
			xamlServiceProvider2.Add(typeof(IReferenceProvider), obj3);
			Type typeFromHandle5 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver2 = new XmlNamespaceResolver();
			xmlNamespaceResolver2.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver2.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver2.Add("d", "http://xamarin.com/schemas/2014/forms/design");
			xmlNamespaceResolver2.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver2.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver2.Add("mc", "http://schemas.openxmlformats.org/markup-compatibility/2006");
			xmlNamespaceResolver2.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver2.Add("syncfusionRB", "clr-namespace:Syncfusion.XForms.Buttons;assembly=Syncfusion.Buttons.XForms");
			xamlServiceProvider2.Add(typeFromHandle5, new XamlTypeResolver(xmlNamespaceResolver2, typeof(ContactDeveloperPage).GetTypeInfo().Assembly));
			xamlServiceProvider2.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(14, 5)));
			DynamicResource dynamicResource = markupExtension2.ProvideValue(xamlServiceProvider2);
			this.SetDynamicResource(VisualElement.BackgroundColorProperty, dynamicResource.Key);
			this.Resources = resourceDictionary;
			grid.SetValue(Layout.PaddingProperty, new Thickness(5.0, 0.0));
			rowDefinition.SetValue(RowDefinition.HeightProperty, new GridLengthTypeConverter().ConvertFromInvariantString("*"));
			grid.GetValue(Grid.RowDefinitionsProperty).Add(rowDefinition);
			rowDefinition2.SetValue(RowDefinition.HeightProperty, new GridLengthTypeConverter().ConvertFromInvariantString("auto"));
			grid.GetValue(Grid.RowDefinitionsProperty).Add(rowDefinition2);
			scrollView.SetValue(Grid.RowProperty, 0);
			scrollView.SetValue(ScrollView.OrientationProperty, 0);
			stackLayout8.SetValue(StackLayout.OrientationProperty, 0);
			translate2.Text = "cdp_ChooseIssueType";
			IMarkupExtension markupExtension3 = translate2;
			XamlServiceProvider xamlServiceProvider3 = new XamlServiceProvider();
			Type typeFromHandle6 = typeof(IProvideValueTarget);
			object[] array4 = new object[0 + 5];
			array4[0] = label;
			array4[1] = stackLayout8;
			array4[2] = scrollView;
			array4[3] = grid;
			array4[4] = this;
			object obj4;
			xamlServiceProvider3.Add(typeFromHandle6, obj4 = new SimpleValueTargetProvider(array4, Label.TextProperty, nameScope));
			xamlServiceProvider3.Add(typeof(IReferenceProvider), obj4);
			Type typeFromHandle7 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver3 = new XmlNamespaceResolver();
			xmlNamespaceResolver3.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver3.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver3.Add("d", "http://xamarin.com/schemas/2014/forms/design");
			xmlNamespaceResolver3.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver3.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver3.Add("mc", "http://schemas.openxmlformats.org/markup-compatibility/2006");
			xmlNamespaceResolver3.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver3.Add("syncfusionRB", "clr-namespace:Syncfusion.XForms.Buttons;assembly=Syncfusion.Buttons.XForms");
			xamlServiceProvider3.Add(typeFromHandle7, new XamlTypeResolver(xmlNamespaceResolver3, typeof(ContactDeveloperPage).GetTypeInfo().Assembly));
			xamlServiceProvider3.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(35, 28)));
			object obj5 = markupExtension3.ProvideValue(xamlServiceProvider3);
			label.Text = obj5;
			stackLayout8.Children.Add(label);
			sfRadioGroup.SetValue(StackLayout.OrientationProperty, 0);
			sfRadioButton.SetValue(ToggleButton.IsCheckedProperty, new bool?(false));
			sfRadioButton.StateChanged += this.RbIssue_StateChanged;
			translate3.Text = "cdp_ConnectionIssue";
			IMarkupExtension markupExtension4 = translate3;
			XamlServiceProvider xamlServiceProvider4 = new XamlServiceProvider();
			Type typeFromHandle8 = typeof(IProvideValueTarget);
			object[] array5 = new object[0 + 6];
			array5[0] = sfRadioButton;
			array5[1] = sfRadioGroup;
			array5[2] = stackLayout8;
			array5[3] = scrollView;
			array5[4] = grid;
			array5[5] = this;
			object obj6;
			xamlServiceProvider4.Add(typeFromHandle8, obj6 = new SimpleValueTargetProvider(array5, ToggleButton.TextProperty, nameScope));
			xamlServiceProvider4.Add(typeof(IReferenceProvider), obj6);
			Type typeFromHandle9 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver4 = new XmlNamespaceResolver();
			xmlNamespaceResolver4.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver4.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver4.Add("d", "http://xamarin.com/schemas/2014/forms/design");
			xmlNamespaceResolver4.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver4.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver4.Add("mc", "http://schemas.openxmlformats.org/markup-compatibility/2006");
			xmlNamespaceResolver4.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver4.Add("syncfusionRB", "clr-namespace:Syncfusion.XForms.Buttons;assembly=Syncfusion.Buttons.XForms");
			xamlServiceProvider4.Add(typeFromHandle9, new XamlTypeResolver(xmlNamespaceResolver4, typeof(ContactDeveloperPage).GetTypeInfo().Assembly));
			xamlServiceProvider4.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(41, 29)));
			object obj7 = markupExtension4.ProvideValue(xamlServiceProvider4);
			sfRadioButton.Text = obj7;
			sfRadioGroup.Children.Add(sfRadioButton);
			sfRadioButton2.SetValue(ToggleButton.IsCheckedProperty, new bool?(false));
			sfRadioButton2.StateChanged += this.RbIssue_StateChanged;
			translate4.Text = "cdp_PurchaseIssue";
			IMarkupExtension markupExtension5 = translate4;
			XamlServiceProvider xamlServiceProvider5 = new XamlServiceProvider();
			Type typeFromHandle10 = typeof(IProvideValueTarget);
			object[] array6 = new object[0 + 6];
			array6[0] = sfRadioButton2;
			array6[1] = sfRadioGroup;
			array6[2] = stackLayout8;
			array6[3] = scrollView;
			array6[4] = grid;
			array6[5] = this;
			object obj8;
			xamlServiceProvider5.Add(typeFromHandle10, obj8 = new SimpleValueTargetProvider(array6, ToggleButton.TextProperty, nameScope));
			xamlServiceProvider5.Add(typeof(IReferenceProvider), obj8);
			Type typeFromHandle11 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver5 = new XmlNamespaceResolver();
			xmlNamespaceResolver5.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver5.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver5.Add("d", "http://xamarin.com/schemas/2014/forms/design");
			xmlNamespaceResolver5.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver5.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver5.Add("mc", "http://schemas.openxmlformats.org/markup-compatibility/2006");
			xmlNamespaceResolver5.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver5.Add("syncfusionRB", "clr-namespace:Syncfusion.XForms.Buttons;assembly=Syncfusion.Buttons.XForms");
			xamlServiceProvider5.Add(typeFromHandle11, new XamlTypeResolver(xmlNamespaceResolver5, typeof(ContactDeveloperPage).GetTypeInfo().Assembly));
			xamlServiceProvider5.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(46, 29)));
			object obj9 = markupExtension5.ProvideValue(xamlServiceProvider5);
			sfRadioButton2.Text = obj9;
			sfRadioGroup.Children.Add(sfRadioButton2);
			sfRadioButton3.SetValue(ToggleButton.IsCheckedProperty, new bool?(false));
			sfRadioButton3.StateChanged += this.RbIssue_StateChanged;
			translate5.Text = "cdp_OtherIssue";
			IMarkupExtension markupExtension6 = translate5;
			XamlServiceProvider xamlServiceProvider6 = new XamlServiceProvider();
			Type typeFromHandle12 = typeof(IProvideValueTarget);
			object[] array7 = new object[0 + 6];
			array7[0] = sfRadioButton3;
			array7[1] = sfRadioGroup;
			array7[2] = stackLayout8;
			array7[3] = scrollView;
			array7[4] = grid;
			array7[5] = this;
			object obj10;
			xamlServiceProvider6.Add(typeFromHandle12, obj10 = new SimpleValueTargetProvider(array7, ToggleButton.TextProperty, nameScope));
			xamlServiceProvider6.Add(typeof(IReferenceProvider), obj10);
			Type typeFromHandle13 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver6 = new XmlNamespaceResolver();
			xmlNamespaceResolver6.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver6.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver6.Add("d", "http://xamarin.com/schemas/2014/forms/design");
			xmlNamespaceResolver6.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver6.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver6.Add("mc", "http://schemas.openxmlformats.org/markup-compatibility/2006");
			xmlNamespaceResolver6.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver6.Add("syncfusionRB", "clr-namespace:Syncfusion.XForms.Buttons;assembly=Syncfusion.Buttons.XForms");
			xamlServiceProvider6.Add(typeFromHandle13, new XamlTypeResolver(xmlNamespaceResolver6, typeof(ContactDeveloperPage).GetTypeInfo().Assembly));
			xamlServiceProvider6.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(51, 29)));
			object obj11 = markupExtension6.ProvideValue(xamlServiceProvider6);
			sfRadioButton3.Text = obj11;
			sfRadioGroup.Children.Add(sfRadioButton3);
			sfRadioButton4.SetValue(ToggleButton.IsCheckedProperty, new bool?(false));
			sfRadioButton4.StateChanged += this.RbIssue_StateChanged;
			translate6.Text = "contactDeveloper_missingCodingService_Title";
			IMarkupExtension markupExtension7 = translate6;
			XamlServiceProvider xamlServiceProvider7 = new XamlServiceProvider();
			Type typeFromHandle14 = typeof(IProvideValueTarget);
			object[] array8 = new object[0 + 6];
			array8[0] = sfRadioButton4;
			array8[1] = sfRadioGroup;
			array8[2] = stackLayout8;
			array8[3] = scrollView;
			array8[4] = grid;
			array8[5] = this;
			object obj12;
			xamlServiceProvider7.Add(typeFromHandle14, obj12 = new SimpleValueTargetProvider(array8, ToggleButton.TextProperty, nameScope));
			xamlServiceProvider7.Add(typeof(IReferenceProvider), obj12);
			Type typeFromHandle15 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver7 = new XmlNamespaceResolver();
			xmlNamespaceResolver7.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver7.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver7.Add("d", "http://xamarin.com/schemas/2014/forms/design");
			xmlNamespaceResolver7.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver7.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver7.Add("mc", "http://schemas.openxmlformats.org/markup-compatibility/2006");
			xmlNamespaceResolver7.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver7.Add("syncfusionRB", "clr-namespace:Syncfusion.XForms.Buttons;assembly=Syncfusion.Buttons.XForms");
			xamlServiceProvider7.Add(typeFromHandle15, new XamlTypeResolver(xmlNamespaceResolver7, typeof(ContactDeveloperPage).GetTypeInfo().Assembly));
			xamlServiceProvider7.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(57, 29)));
			object obj13 = markupExtension7.ProvideValue(xamlServiceProvider7);
			sfRadioButton4.Text = obj13;
			sfRadioGroup.Children.Add(sfRadioButton4);
			stackLayout8.Children.Add(sfRadioGroup);
			stackLayout2.SetValue(BindableObject.BindingContextProperty, sharedSettings);
			stackLayout2.SetValue(VisualElement.IsVisibleProperty, new VisualElement.VisibilityConverter().ConvertFromInvariantString("False"));
			stackLayout2.SetValue(StackLayout.OrientationProperty, 0);
			bindingExtension.Mode = 2;
			staticResourceExtension.Key = "BoolToNegativeConverter";
			IMarkupExtension markupExtension8 = staticResourceExtension;
			XamlServiceProvider xamlServiceProvider8 = new XamlServiceProvider();
			Type typeFromHandle16 = typeof(IProvideValueTarget);
			object[] array9 = new object[0 + 7];
			array9[0] = bindingExtension;
			array9[1] = stackLayout;
			array9[2] = stackLayout2;
			array9[3] = stackLayout8;
			array9[4] = scrollView;
			array9[5] = grid;
			array9[6] = this;
			object obj14;
			xamlServiceProvider8.Add(typeFromHandle16, obj14 = new SimpleValueTargetProvider(array9, typeof(BindingExtension).GetRuntimeProperty("Converter"), nameScope));
			xamlServiceProvider8.Add(typeof(IReferenceProvider), obj14);
			Type typeFromHandle17 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver8 = new XmlNamespaceResolver();
			xmlNamespaceResolver8.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver8.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver8.Add("d", "http://xamarin.com/schemas/2014/forms/design");
			xmlNamespaceResolver8.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver8.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver8.Add("mc", "http://schemas.openxmlformats.org/markup-compatibility/2006");
			xmlNamespaceResolver8.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver8.Add("syncfusionRB", "clr-namespace:Syncfusion.XForms.Buttons;assembly=Syncfusion.Buttons.XForms");
			xamlServiceProvider8.Add(typeFromHandle17, new XamlTypeResolver(xmlNamespaceResolver8, typeof(ContactDeveloperPage).GetTypeInfo().Assembly));
			xamlServiceProvider8.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(66, 38)));
			object obj15 = markupExtension8.ProvideValue(xamlServiceProvider8);
			bindingExtension.Converter = obj15;
			bindingExtension.Path = "AdsProductPurchased";
			bindingExtension.TypedBinding = new TypedBinding<SharedSettings, bool>(delegate(SharedSettings A_0)
			{
				if (A_0 != null)
				{
					return new ValueTuple<bool, bool>(A_0.AdsProductPurchased, true);
				}
				return default(ValueTuple<bool, bool>);
			}, null, new Tuple<Func<SharedSettings, object>, string>[]
			{
				new Tuple<Func<SharedSettings, object>, string>((SharedSettings A_0) => A_0, "AdsProductPurchased")
			});
			BindingBase bindingBase = bindingExtension.ProvideValue(null);
			stackLayout.SetBinding(VisualElement.IsVisibleProperty, bindingBase);
			translate7.Text = "cdp_AndroidRestoreHint";
			IMarkupExtension markupExtension9 = translate7;
			XamlServiceProvider xamlServiceProvider9 = new XamlServiceProvider();
			Type typeFromHandle18 = typeof(IProvideValueTarget);
			object[] array10 = new object[0 + 7];
			array10[0] = label2;
			array10[1] = stackLayout;
			array10[2] = stackLayout2;
			array10[3] = stackLayout8;
			array10[4] = scrollView;
			array10[5] = grid;
			array10[6] = this;
			object obj16;
			xamlServiceProvider9.Add(typeFromHandle18, obj16 = new SimpleValueTargetProvider(array10, Label.TextProperty, nameScope));
			xamlServiceProvider9.Add(typeof(IReferenceProvider), obj16);
			Type typeFromHandle19 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver9 = new XmlNamespaceResolver();
			xmlNamespaceResolver9.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver9.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver9.Add("d", "http://xamarin.com/schemas/2014/forms/design");
			xmlNamespaceResolver9.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver9.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver9.Add("mc", "http://schemas.openxmlformats.org/markup-compatibility/2006");
			xmlNamespaceResolver9.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver9.Add("syncfusionRB", "clr-namespace:Syncfusion.XForms.Buttons;assembly=Syncfusion.Buttons.XForms");
			xamlServiceProvider9.Add(typeFromHandle19, new XamlTypeResolver(xmlNamespaceResolver9, typeof(ContactDeveloperPage).GetTypeInfo().Assembly));
			xamlServiceProvider9.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(67, 36)));
			object obj17 = markupExtension9.ProvideValue(xamlServiceProvider9);
			label2.Text = obj17;
			on.Platform = new List<string>(1) { "iOS" };
			on.Value = "false";
			onPlatform.Platforms.Add(on);
			on2.Platform = new List<string>(1) { "Android" };
			on2.Value = "true";
			onPlatform.Platforms.Add(on2);
			label2.SetValue(VisualElement.IsVisibleProperty, onPlatform);
			stackLayout.Children.Add(label2);
			translate8.Text = "cdp_iOSRestoreHint";
			IMarkupExtension markupExtension10 = translate8;
			XamlServiceProvider xamlServiceProvider10 = new XamlServiceProvider();
			Type typeFromHandle20 = typeof(IProvideValueTarget);
			object[] array11 = new object[0 + 7];
			array11[0] = label3;
			array11[1] = stackLayout;
			array11[2] = stackLayout2;
			array11[3] = stackLayout8;
			array11[4] = scrollView;
			array11[5] = grid;
			array11[6] = this;
			object obj18;
			xamlServiceProvider10.Add(typeFromHandle20, obj18 = new SimpleValueTargetProvider(array11, Label.TextProperty, nameScope));
			xamlServiceProvider10.Add(typeof(IReferenceProvider), obj18);
			Type typeFromHandle21 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver10 = new XmlNamespaceResolver();
			xmlNamespaceResolver10.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver10.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver10.Add("d", "http://xamarin.com/schemas/2014/forms/design");
			xmlNamespaceResolver10.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver10.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver10.Add("mc", "http://schemas.openxmlformats.org/markup-compatibility/2006");
			xmlNamespaceResolver10.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver10.Add("syncfusionRB", "clr-namespace:Syncfusion.XForms.Buttons;assembly=Syncfusion.Buttons.XForms");
			xamlServiceProvider10.Add(typeFromHandle21, new XamlTypeResolver(xmlNamespaceResolver10, typeof(ContactDeveloperPage).GetTypeInfo().Assembly));
			xamlServiceProvider10.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(76, 36)));
			object obj19 = markupExtension10.ProvideValue(xamlServiceProvider10);
			label3.Text = obj19;
			on3.Platform = new List<string>(1) { "iOS" };
			on3.Value = "true";
			onPlatform2.Platforms.Add(on3);
			on4.Platform = new List<string>(1) { "Android" };
			on4.Value = "false";
			onPlatform2.Platforms.Add(on4);
			label3.SetValue(VisualElement.IsVisibleProperty, onPlatform2);
			stackLayout.Children.Add(label3);
			stackLayout2.Children.Add(stackLayout);
			label4.SetValue(Label.FontAttributesProperty, new FontAttributesConverter().ConvertFromInvariantString("Bold"));
			label4.SetValue(View.HorizontalOptionsProperty, LayoutOptions.Center);
			bindingExtension2.Mode = 2;
			bindingExtension2.Path = "AdsProductPurchased";
			bindingExtension2.TypedBinding = new TypedBinding<SharedSettings, bool>(delegate(SharedSettings A_0)
			{
				if (A_0 != null)
				{
					return new ValueTuple<bool, bool>(A_0.AdsProductPurchased, true);
				}
				return default(ValueTuple<bool, bool>);
			}, null, new Tuple<Func<SharedSettings, object>, string>[]
			{
				new Tuple<Func<SharedSettings, object>, string>((SharedSettings A_0) => A_0, "AdsProductPurchased")
			});
			BindingBase bindingBase2 = bindingExtension2.ProvideValue(null);
			label4.SetBinding(VisualElement.IsVisibleProperty, bindingBase2);
			translate9.Text = "settings_AlreadyPro";
			IMarkupExtension markupExtension11 = translate9;
			XamlServiceProvider xamlServiceProvider11 = new XamlServiceProvider();
			Type typeFromHandle22 = typeof(IProvideValueTarget);
			object[] array12 = new object[0 + 6];
			array12[0] = label4;
			array12[1] = stackLayout2;
			array12[2] = stackLayout8;
			array12[3] = scrollView;
			array12[4] = grid;
			array12[5] = this;
			object obj20;
			xamlServiceProvider11.Add(typeFromHandle22, obj20 = new SimpleValueTargetProvider(array12, Label.TextProperty, nameScope));
			xamlServiceProvider11.Add(typeof(IReferenceProvider), obj20);
			Type typeFromHandle23 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver11 = new XmlNamespaceResolver();
			xmlNamespaceResolver11.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver11.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver11.Add("d", "http://xamarin.com/schemas/2014/forms/design");
			xmlNamespaceResolver11.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver11.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver11.Add("mc", "http://schemas.openxmlformats.org/markup-compatibility/2006");
			xmlNamespaceResolver11.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver11.Add("syncfusionRB", "clr-namespace:Syncfusion.XForms.Buttons;assembly=Syncfusion.Buttons.XForms");
			xamlServiceProvider11.Add(typeFromHandle23, new XamlTypeResolver(xmlNamespaceResolver11, typeof(ContactDeveloperPage).GetTypeInfo().Assembly));
			xamlServiceProvider11.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(90, 29)));
			object obj21 = markupExtension11.ProvideValue(xamlServiceProvider11);
			label4.Text = obj21;
			stackLayout2.Children.Add(label4);
			stackLayout8.Children.Add(stackLayout2);
			stackLayout3.SetValue(VisualElement.IsVisibleProperty, new VisualElement.VisibilityConverter().ConvertFromInvariantString("False"));
			stackLayout3.SetValue(StackLayout.OrientationProperty, 0);
			translate10.Text = "cdp_ConnectionHint";
			IMarkupExtension markupExtension12 = translate10;
			XamlServiceProvider xamlServiceProvider12 = new XamlServiceProvider();
			Type typeFromHandle24 = typeof(IProvideValueTarget);
			object[] array13 = new object[0 + 6];
			array13[0] = label5;
			array13[1] = stackLayout3;
			array13[2] = stackLayout8;
			array13[3] = scrollView;
			array13[4] = grid;
			array13[5] = this;
			object obj22;
			xamlServiceProvider12.Add(typeFromHandle24, obj22 = new SimpleValueTargetProvider(array13, Label.TextProperty, nameScope));
			xamlServiceProvider12.Add(typeof(IReferenceProvider), obj22);
			Type typeFromHandle25 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver12 = new XmlNamespaceResolver();
			xmlNamespaceResolver12.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver12.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver12.Add("d", "http://xamarin.com/schemas/2014/forms/design");
			xmlNamespaceResolver12.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver12.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver12.Add("mc", "http://schemas.openxmlformats.org/markup-compatibility/2006");
			xmlNamespaceResolver12.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver12.Add("syncfusionRB", "clr-namespace:Syncfusion.XForms.Buttons;assembly=Syncfusion.Buttons.XForms");
			xamlServiceProvider12.Add(typeFromHandle25, new XamlTypeResolver(xmlNamespaceResolver12, typeof(ContactDeveloperPage).GetTypeInfo().Assembly));
			xamlServiceProvider12.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(98, 61)));
			object obj23 = markupExtension12.ProvideValue(xamlServiceProvider12);
			label5.Text = obj23;
			stackLayout3.Children.Add(label5);
			stackLayout8.Children.Add(stackLayout3);
			stackLayout4.SetValue(VisualElement.IsVisibleProperty, new VisualElement.VisibilityConverter().ConvertFromInvariantString("False"));
			stackLayout4.SetValue(StackLayout.OrientationProperty, 0);
			translate11.Text = "cdp_DidThatHelp";
			IMarkupExtension markupExtension13 = translate11;
			XamlServiceProvider xamlServiceProvider13 = new XamlServiceProvider();
			Type typeFromHandle26 = typeof(IProvideValueTarget);
			object[] array14 = new object[0 + 6];
			array14[0] = label6;
			array14[1] = stackLayout4;
			array14[2] = stackLayout8;
			array14[3] = scrollView;
			array14[4] = grid;
			array14[5] = this;
			object obj24;
			xamlServiceProvider13.Add(typeFromHandle26, obj24 = new SimpleValueTargetProvider(array14, Label.TextProperty, nameScope));
			xamlServiceProvider13.Add(typeof(IReferenceProvider), obj24);
			Type typeFromHandle27 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver13 = new XmlNamespaceResolver();
			xmlNamespaceResolver13.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver13.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver13.Add("d", "http://xamarin.com/schemas/2014/forms/design");
			xmlNamespaceResolver13.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver13.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver13.Add("mc", "http://schemas.openxmlformats.org/markup-compatibility/2006");
			xmlNamespaceResolver13.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver13.Add("syncfusionRB", "clr-namespace:Syncfusion.XForms.Buttons;assembly=Syncfusion.Buttons.XForms");
			xamlServiceProvider13.Add(typeFromHandle27, new XamlTypeResolver(xmlNamespaceResolver13, typeof(ContactDeveloperPage).GetTypeInfo().Assembly));
			xamlServiceProvider13.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(107, 32)));
			object obj25 = markupExtension13.ProvideValue(xamlServiceProvider13);
			label6.Text = obj25;
			stackLayout4.Children.Add(label6);
			button.Clicked += this.BtnProblemSolved_Clicked;
			translate12.Text = "cdp_YesSolved";
			IMarkupExtension markupExtension14 = translate12;
			XamlServiceProvider xamlServiceProvider14 = new XamlServiceProvider();
			Type typeFromHandle28 = typeof(IProvideValueTarget);
			object[] array15 = new object[0 + 6];
			array15[0] = button;
			array15[1] = stackLayout4;
			array15[2] = stackLayout8;
			array15[3] = scrollView;
			array15[4] = grid;
			array15[5] = this;
			object obj26;
			xamlServiceProvider14.Add(typeFromHandle28, obj26 = new SimpleValueTargetProvider(array15, Button.TextProperty, nameScope));
			xamlServiceProvider14.Add(typeof(IReferenceProvider), obj26);
			Type typeFromHandle29 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver14 = new XmlNamespaceResolver();
			xmlNamespaceResolver14.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver14.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver14.Add("d", "http://xamarin.com/schemas/2014/forms/design");
			xmlNamespaceResolver14.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver14.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver14.Add("mc", "http://schemas.openxmlformats.org/markup-compatibility/2006");
			xmlNamespaceResolver14.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver14.Add("syncfusionRB", "clr-namespace:Syncfusion.XForms.Buttons;assembly=Syncfusion.Buttons.XForms");
			xamlServiceProvider14.Add(typeFromHandle29, new XamlTypeResolver(xmlNamespaceResolver14, typeof(ContactDeveloperPage).GetTypeInfo().Assembly));
			xamlServiceProvider14.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(111, 29)));
			object obj27 = markupExtension14.ProvideValue(xamlServiceProvider14);
			button.Text = obj27;
			stackLayout4.Children.Add(button);
			button2.Clicked += this.BtnProblemNotSolved_Clicked;
			translate13.Text = "cdp_NotSolved";
			IMarkupExtension markupExtension15 = translate13;
			XamlServiceProvider xamlServiceProvider15 = new XamlServiceProvider();
			Type typeFromHandle30 = typeof(IProvideValueTarget);
			object[] array16 = new object[0 + 6];
			array16[0] = button2;
			array16[1] = stackLayout4;
			array16[2] = stackLayout8;
			array16[3] = scrollView;
			array16[4] = grid;
			array16[5] = this;
			object obj28;
			xamlServiceProvider15.Add(typeFromHandle30, obj28 = new SimpleValueTargetProvider(array16, Button.TextProperty, nameScope));
			xamlServiceProvider15.Add(typeof(IReferenceProvider), obj28);
			Type typeFromHandle31 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver15 = new XmlNamespaceResolver();
			xmlNamespaceResolver15.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver15.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver15.Add("d", "http://xamarin.com/schemas/2014/forms/design");
			xmlNamespaceResolver15.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver15.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver15.Add("mc", "http://schemas.openxmlformats.org/markup-compatibility/2006");
			xmlNamespaceResolver15.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver15.Add("syncfusionRB", "clr-namespace:Syncfusion.XForms.Buttons;assembly=Syncfusion.Buttons.XForms");
			xamlServiceProvider15.Add(typeFromHandle31, new XamlTypeResolver(xmlNamespaceResolver15, typeof(ContactDeveloperPage).GetTypeInfo().Assembly));
			xamlServiceProvider15.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(115, 29)));
			object obj29 = markupExtension15.ProvideValue(xamlServiceProvider15);
			button2.Text = obj29;
			stackLayout4.Children.Add(button2);
			stackLayout8.Children.Add(stackLayout4);
			stackLayout5.SetValue(VisualElement.IsVisibleProperty, new VisualElement.VisibilityConverter().ConvertFromInvariantString("False"));
			stackLayout5.SetValue(StackLayout.OrientationProperty, 0);
			dynamicResourceExtension2.Key = "EntryBackgroundColor";
			IMarkupExtension<DynamicResource> markupExtension16 = dynamicResourceExtension2;
			XamlServiceProvider xamlServiceProvider16 = new XamlServiceProvider();
			Type typeFromHandle32 = typeof(IProvideValueTarget);
			object[] array17 = new object[0 + 6];
			array17[0] = editor;
			array17[1] = stackLayout5;
			array17[2] = stackLayout8;
			array17[3] = scrollView;
			array17[4] = grid;
			array17[5] = this;
			object obj30;
			xamlServiceProvider16.Add(typeFromHandle32, obj30 = new SimpleValueTargetProvider(array17, VisualElement.BackgroundColorProperty, nameScope));
			xamlServiceProvider16.Add(typeof(IReferenceProvider), obj30);
			Type typeFromHandle33 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver16 = new XmlNamespaceResolver();
			xmlNamespaceResolver16.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver16.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver16.Add("d", "http://xamarin.com/schemas/2014/forms/design");
			xmlNamespaceResolver16.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver16.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver16.Add("mc", "http://schemas.openxmlformats.org/markup-compatibility/2006");
			xmlNamespaceResolver16.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver16.Add("syncfusionRB", "clr-namespace:Syncfusion.XForms.Buttons;assembly=Syncfusion.Buttons.XForms");
			xamlServiceProvider16.Add(typeFromHandle33, new XamlTypeResolver(xmlNamespaceResolver16, typeof(ContactDeveloperPage).GetTypeInfo().Assembly));
			xamlServiceProvider16.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(125, 29)));
			DynamicResource dynamicResource2 = markupExtension16.ProvideValue(xamlServiceProvider16);
			editor.SetDynamicResource(VisualElement.BackgroundColorProperty, dynamicResource2.Key);
			editor.SetValue(VisualElement.HeightRequestProperty, 250.0);
			editor.SetValue(InputView.IsSpellCheckEnabledProperty, true);
			translate14.Text = "cdp_DescribeProblem";
			IMarkupExtension markupExtension17 = translate14;
			XamlServiceProvider xamlServiceProvider17 = new XamlServiceProvider();
			Type typeFromHandle34 = typeof(IProvideValueTarget);
			object[] array18 = new object[0 + 6];
			array18[0] = editor;
			array18[1] = stackLayout5;
			array18[2] = stackLayout8;
			array18[3] = scrollView;
			array18[4] = grid;
			array18[5] = this;
			object obj31;
			xamlServiceProvider17.Add(typeFromHandle34, obj31 = new SimpleValueTargetProvider(array18, Editor.PlaceholderProperty, nameScope));
			xamlServiceProvider17.Add(typeof(IReferenceProvider), obj31);
			Type typeFromHandle35 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver17 = new XmlNamespaceResolver();
			xmlNamespaceResolver17.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver17.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver17.Add("d", "http://xamarin.com/schemas/2014/forms/design");
			xmlNamespaceResolver17.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver17.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver17.Add("mc", "http://schemas.openxmlformats.org/markup-compatibility/2006");
			xmlNamespaceResolver17.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver17.Add("syncfusionRB", "clr-namespace:Syncfusion.XForms.Buttons;assembly=Syncfusion.Buttons.XForms");
			xamlServiceProvider17.Add(typeFromHandle35, new XamlTypeResolver(xmlNamespaceResolver17, typeof(ContactDeveloperPage).GetTypeInfo().Assembly));
			xamlServiceProvider17.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(128, 29)));
			object obj32 = markupExtension17.ProvideValue(xamlServiceProvider17);
			editor.Placeholder = obj32;
			dynamicResourceExtension3.Key = "GrayedTextColor";
			IMarkupExtension<DynamicResource> markupExtension18 = dynamicResourceExtension3;
			XamlServiceProvider xamlServiceProvider18 = new XamlServiceProvider();
			Type typeFromHandle36 = typeof(IProvideValueTarget);
			object[] array19 = new object[0 + 6];
			array19[0] = editor;
			array19[1] = stackLayout5;
			array19[2] = stackLayout8;
			array19[3] = scrollView;
			array19[4] = grid;
			array19[5] = this;
			object obj33;
			xamlServiceProvider18.Add(typeFromHandle36, obj33 = new SimpleValueTargetProvider(array19, Editor.PlaceholderColorProperty, nameScope));
			xamlServiceProvider18.Add(typeof(IReferenceProvider), obj33);
			Type typeFromHandle37 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver18 = new XmlNamespaceResolver();
			xmlNamespaceResolver18.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver18.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver18.Add("d", "http://xamarin.com/schemas/2014/forms/design");
			xmlNamespaceResolver18.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver18.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver18.Add("mc", "http://schemas.openxmlformats.org/markup-compatibility/2006");
			xmlNamespaceResolver18.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver18.Add("syncfusionRB", "clr-namespace:Syncfusion.XForms.Buttons;assembly=Syncfusion.Buttons.XForms");
			xamlServiceProvider18.Add(typeFromHandle37, new XamlTypeResolver(xmlNamespaceResolver18, typeof(ContactDeveloperPage).GetTypeInfo().Assembly));
			xamlServiceProvider18.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(129, 29)));
			DynamicResource dynamicResource3 = markupExtension18.ProvideValue(xamlServiceProvider18);
			editor.SetDynamicResource(Editor.PlaceholderColorProperty, dynamicResource3.Key);
			dynamicResourceExtension4.Key = "TextColor";
			IMarkupExtension<DynamicResource> markupExtension19 = dynamicResourceExtension4;
			XamlServiceProvider xamlServiceProvider19 = new XamlServiceProvider();
			Type typeFromHandle38 = typeof(IProvideValueTarget);
			object[] array20 = new object[0 + 6];
			array20[0] = editor;
			array20[1] = stackLayout5;
			array20[2] = stackLayout8;
			array20[3] = scrollView;
			array20[4] = grid;
			array20[5] = this;
			object obj34;
			xamlServiceProvider19.Add(typeFromHandle38, obj34 = new SimpleValueTargetProvider(array20, Editor.TextColorProperty, nameScope));
			xamlServiceProvider19.Add(typeof(IReferenceProvider), obj34);
			Type typeFromHandle39 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver19 = new XmlNamespaceResolver();
			xmlNamespaceResolver19.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver19.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver19.Add("d", "http://xamarin.com/schemas/2014/forms/design");
			xmlNamespaceResolver19.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver19.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver19.Add("mc", "http://schemas.openxmlformats.org/markup-compatibility/2006");
			xmlNamespaceResolver19.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver19.Add("syncfusionRB", "clr-namespace:Syncfusion.XForms.Buttons;assembly=Syncfusion.Buttons.XForms");
			xamlServiceProvider19.Add(typeFromHandle39, new XamlTypeResolver(xmlNamespaceResolver19, typeof(ContactDeveloperPage).GetTypeInfo().Assembly));
			xamlServiceProvider19.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(130, 29)));
			DynamicResource dynamicResource4 = markupExtension19.ProvideValue(xamlServiceProvider19);
			editor.SetDynamicResource(Editor.TextColorProperty, dynamicResource4.Key);
			stackLayout5.Children.Add(editor);
			button3.Clicked += this.BtnNext_Clicked;
			translate15.Text = "ios_NEXT";
			IMarkupExtension markupExtension20 = translate15;
			XamlServiceProvider xamlServiceProvider20 = new XamlServiceProvider();
			Type typeFromHandle40 = typeof(IProvideValueTarget);
			object[] array21 = new object[0 + 6];
			array21[0] = button3;
			array21[1] = stackLayout5;
			array21[2] = stackLayout8;
			array21[3] = scrollView;
			array21[4] = grid;
			array21[5] = this;
			object obj35;
			xamlServiceProvider20.Add(typeFromHandle40, obj35 = new SimpleValueTargetProvider(array21, Button.TextProperty, nameScope));
			xamlServiceProvider20.Add(typeof(IReferenceProvider), obj35);
			Type typeFromHandle41 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver20 = new XmlNamespaceResolver();
			xmlNamespaceResolver20.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver20.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver20.Add("d", "http://xamarin.com/schemas/2014/forms/design");
			xmlNamespaceResolver20.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver20.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver20.Add("mc", "http://schemas.openxmlformats.org/markup-compatibility/2006");
			xmlNamespaceResolver20.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver20.Add("syncfusionRB", "clr-namespace:Syncfusion.XForms.Buttons;assembly=Syncfusion.Buttons.XForms");
			xamlServiceProvider20.Add(typeFromHandle41, new XamlTypeResolver(xmlNamespaceResolver20, typeof(ContactDeveloperPage).GetTypeInfo().Assembly));
			xamlServiceProvider20.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(134, 29)));
			object obj36 = markupExtension20.ProvideValue(xamlServiceProvider20);
			button3.Text = obj36;
			stackLayout5.Children.Add(button3);
			stackLayout8.Children.Add(stackLayout5);
			stackLayout6.SetValue(VisualElement.IsVisibleProperty, new VisualElement.VisibilityConverter().ConvertFromInvariantString("False"));
			stackLayout6.SetValue(StackLayout.OrientationProperty, 0);
			translate16.Text = "cdp_EnterCarDetails";
			IMarkupExtension markupExtension21 = translate16;
			XamlServiceProvider xamlServiceProvider21 = new XamlServiceProvider();
			Type typeFromHandle42 = typeof(IProvideValueTarget);
			object[] array22 = new object[0 + 6];
			array22[0] = label7;
			array22[1] = stackLayout6;
			array22[2] = stackLayout8;
			array22[3] = scrollView;
			array22[4] = grid;
			array22[5] = this;
			object obj37;
			xamlServiceProvider21.Add(typeFromHandle42, obj37 = new SimpleValueTargetProvider(array22, Label.TextProperty, nameScope));
			xamlServiceProvider21.Add(typeof(IReferenceProvider), obj37);
			Type typeFromHandle43 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver21 = new XmlNamespaceResolver();
			xmlNamespaceResolver21.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver21.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver21.Add("d", "http://xamarin.com/schemas/2014/forms/design");
			xmlNamespaceResolver21.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver21.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver21.Add("mc", "http://schemas.openxmlformats.org/markup-compatibility/2006");
			xmlNamespaceResolver21.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver21.Add("syncfusionRB", "clr-namespace:Syncfusion.XForms.Buttons;assembly=Syncfusion.Buttons.XForms");
			xamlServiceProvider21.Add(typeFromHandle43, new XamlTypeResolver(xmlNamespaceResolver21, typeof(ContactDeveloperPage).GetTypeInfo().Assembly));
			xamlServiceProvider21.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(142, 32)));
			object obj38 = markupExtension21.ProvideValue(xamlServiceProvider21);
			label7.Text = obj38;
			stackLayout6.Children.Add(label7);
			translate17.Text = "cdp_CarBrand";
			IMarkupExtension markupExtension22 = translate17;
			XamlServiceProvider xamlServiceProvider22 = new XamlServiceProvider();
			Type typeFromHandle44 = typeof(IProvideValueTarget);
			object[] array23 = new object[0 + 6];
			array23[0] = entry;
			array23[1] = stackLayout6;
			array23[2] = stackLayout8;
			array23[3] = scrollView;
			array23[4] = grid;
			array23[5] = this;
			object obj39;
			xamlServiceProvider22.Add(typeFromHandle44, obj39 = new SimpleValueTargetProvider(array23, Entry.PlaceholderProperty, nameScope));
			xamlServiceProvider22.Add(typeof(IReferenceProvider), obj39);
			Type typeFromHandle45 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver22 = new XmlNamespaceResolver();
			xmlNamespaceResolver22.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver22.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver22.Add("d", "http://xamarin.com/schemas/2014/forms/design");
			xmlNamespaceResolver22.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver22.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver22.Add("mc", "http://schemas.openxmlformats.org/markup-compatibility/2006");
			xmlNamespaceResolver22.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver22.Add("syncfusionRB", "clr-namespace:Syncfusion.XForms.Buttons;assembly=Syncfusion.Buttons.XForms");
			xamlServiceProvider22.Add(typeFromHandle45, new XamlTypeResolver(xmlNamespaceResolver22, typeof(ContactDeveloperPage).GetTypeInfo().Assembly));
			xamlServiceProvider22.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(143, 52)));
			object obj40 = markupExtension22.ProvideValue(xamlServiceProvider22);
			entry.Placeholder = obj40;
			stackLayout6.Children.Add(entry);
			translate18.Text = "cdp_CarModel";
			IMarkupExtension markupExtension23 = translate18;
			XamlServiceProvider xamlServiceProvider23 = new XamlServiceProvider();
			Type typeFromHandle46 = typeof(IProvideValueTarget);
			object[] array24 = new object[0 + 6];
			array24[0] = entry2;
			array24[1] = stackLayout6;
			array24[2] = stackLayout8;
			array24[3] = scrollView;
			array24[4] = grid;
			array24[5] = this;
			object obj41;
			xamlServiceProvider23.Add(typeFromHandle46, obj41 = new SimpleValueTargetProvider(array24, Entry.PlaceholderProperty, nameScope));
			xamlServiceProvider23.Add(typeof(IReferenceProvider), obj41);
			Type typeFromHandle47 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver23 = new XmlNamespaceResolver();
			xmlNamespaceResolver23.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver23.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver23.Add("d", "http://xamarin.com/schemas/2014/forms/design");
			xmlNamespaceResolver23.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver23.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver23.Add("mc", "http://schemas.openxmlformats.org/markup-compatibility/2006");
			xmlNamespaceResolver23.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver23.Add("syncfusionRB", "clr-namespace:Syncfusion.XForms.Buttons;assembly=Syncfusion.Buttons.XForms");
			xamlServiceProvider23.Add(typeFromHandle47, new XamlTypeResolver(xmlNamespaceResolver23, typeof(ContactDeveloperPage).GetTypeInfo().Assembly));
			xamlServiceProvider23.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(144, 52)));
			object obj42 = markupExtension23.ProvideValue(xamlServiceProvider23);
			entry2.Placeholder = obj42;
			stackLayout6.Children.Add(entry2);
			translate19.Text = "cdp_CarYear";
			IMarkupExtension markupExtension24 = translate19;
			XamlServiceProvider xamlServiceProvider24 = new XamlServiceProvider();
			Type typeFromHandle48 = typeof(IProvideValueTarget);
			object[] array25 = new object[0 + 6];
			array25[0] = entry3;
			array25[1] = stackLayout6;
			array25[2] = stackLayout8;
			array25[3] = scrollView;
			array25[4] = grid;
			array25[5] = this;
			object obj43;
			xamlServiceProvider24.Add(typeFromHandle48, obj43 = new SimpleValueTargetProvider(array25, Entry.PlaceholderProperty, nameScope));
			xamlServiceProvider24.Add(typeof(IReferenceProvider), obj43);
			Type typeFromHandle49 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver24 = new XmlNamespaceResolver();
			xmlNamespaceResolver24.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver24.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver24.Add("d", "http://xamarin.com/schemas/2014/forms/design");
			xmlNamespaceResolver24.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver24.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver24.Add("mc", "http://schemas.openxmlformats.org/markup-compatibility/2006");
			xmlNamespaceResolver24.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver24.Add("syncfusionRB", "clr-namespace:Syncfusion.XForms.Buttons;assembly=Syncfusion.Buttons.XForms");
			xamlServiceProvider24.Add(typeFromHandle49, new XamlTypeResolver(xmlNamespaceResolver24, typeof(ContactDeveloperPage).GetTypeInfo().Assembly));
			xamlServiceProvider24.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(145, 51)));
			object obj44 = markupExtension24.ProvideValue(xamlServiceProvider24);
			entry3.Placeholder = obj44;
			stackLayout6.Children.Add(entry3);
			translate20.Text = "cdp_EngineDisplacement";
			IMarkupExtension markupExtension25 = translate20;
			XamlServiceProvider xamlServiceProvider25 = new XamlServiceProvider();
			Type typeFromHandle50 = typeof(IProvideValueTarget);
			object[] array26 = new object[0 + 6];
			array26[0] = entry4;
			array26[1] = stackLayout6;
			array26[2] = stackLayout8;
			array26[3] = scrollView;
			array26[4] = grid;
			array26[5] = this;
			object obj45;
			xamlServiceProvider25.Add(typeFromHandle50, obj45 = new SimpleValueTargetProvider(array26, Entry.PlaceholderProperty, nameScope));
			xamlServiceProvider25.Add(typeof(IReferenceProvider), obj45);
			Type typeFromHandle51 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver25 = new XmlNamespaceResolver();
			xmlNamespaceResolver25.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver25.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver25.Add("d", "http://xamarin.com/schemas/2014/forms/design");
			xmlNamespaceResolver25.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver25.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver25.Add("mc", "http://schemas.openxmlformats.org/markup-compatibility/2006");
			xmlNamespaceResolver25.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver25.Add("syncfusionRB", "clr-namespace:Syncfusion.XForms.Buttons;assembly=Syncfusion.Buttons.XForms");
			xamlServiceProvider25.Add(typeFromHandle51, new XamlTypeResolver(xmlNamespaceResolver25, typeof(ContactDeveloperPage).GetTypeInfo().Assembly));
			xamlServiceProvider25.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(146, 59)));
			object obj46 = markupExtension25.ProvideValue(xamlServiceProvider25);
			entry4.Placeholder = obj46;
			stackLayout6.Children.Add(entry4);
			translate21.Text = "cdp_EngineModel";
			IMarkupExtension markupExtension26 = translate21;
			XamlServiceProvider xamlServiceProvider26 = new XamlServiceProvider();
			Type typeFromHandle52 = typeof(IProvideValueTarget);
			object[] array27 = new object[0 + 6];
			array27[0] = entry5;
			array27[1] = stackLayout6;
			array27[2] = stackLayout8;
			array27[3] = scrollView;
			array27[4] = grid;
			array27[5] = this;
			object obj47;
			xamlServiceProvider26.Add(typeFromHandle52, obj47 = new SimpleValueTargetProvider(array27, Entry.PlaceholderProperty, nameScope));
			xamlServiceProvider26.Add(typeof(IReferenceProvider), obj47);
			Type typeFromHandle53 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver26 = new XmlNamespaceResolver();
			xmlNamespaceResolver26.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver26.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver26.Add("d", "http://xamarin.com/schemas/2014/forms/design");
			xmlNamespaceResolver26.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver26.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver26.Add("mc", "http://schemas.openxmlformats.org/markup-compatibility/2006");
			xmlNamespaceResolver26.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver26.Add("syncfusionRB", "clr-namespace:Syncfusion.XForms.Buttons;assembly=Syncfusion.Buttons.XForms");
			xamlServiceProvider26.Add(typeFromHandle53, new XamlTypeResolver(xmlNamespaceResolver26, typeof(ContactDeveloperPage).GetTypeInfo().Assembly));
			xamlServiceProvider26.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(147, 58)));
			object obj48 = markupExtension26.ProvideValue(xamlServiceProvider26);
			entry5.Placeholder = obj48;
			stackLayout6.Children.Add(entry5);
			translate22.Text = "Settings_Control_tbVehicleFuel.Text";
			IMarkupExtension markupExtension27 = translate22;
			XamlServiceProvider xamlServiceProvider27 = new XamlServiceProvider();
			Type typeFromHandle54 = typeof(IProvideValueTarget);
			object[] array28 = new object[0 + 6];
			array28[0] = label8;
			array28[1] = stackLayout6;
			array28[2] = stackLayout8;
			array28[3] = scrollView;
			array28[4] = grid;
			array28[5] = this;
			object obj49;
			xamlServiceProvider27.Add(typeFromHandle54, obj49 = new SimpleValueTargetProvider(array28, Label.TextProperty, nameScope));
			xamlServiceProvider27.Add(typeof(IReferenceProvider), obj49);
			Type typeFromHandle55 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver27 = new XmlNamespaceResolver();
			xmlNamespaceResolver27.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver27.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver27.Add("d", "http://xamarin.com/schemas/2014/forms/design");
			xmlNamespaceResolver27.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver27.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver27.Add("mc", "http://schemas.openxmlformats.org/markup-compatibility/2006");
			xmlNamespaceResolver27.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver27.Add("syncfusionRB", "clr-namespace:Syncfusion.XForms.Buttons;assembly=Syncfusion.Buttons.XForms");
			xamlServiceProvider27.Add(typeFromHandle55, new XamlTypeResolver(xmlNamespaceResolver27, typeof(ContactDeveloperPage).GetTypeInfo().Assembly));
			xamlServiceProvider27.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(148, 32)));
			object obj50 = markupExtension27.ProvideValue(xamlServiceProvider27);
			label8.Text = obj50;
			stackLayout6.Children.Add(label8);
			bindingExtension3.Source = fuelTypesList;
			BindingBase bindingBase3 = bindingExtension3.ProvideValue(null);
			picker.SetBinding(Picker.ItemsSourceProperty, bindingBase3);
			picker.SetValue(Picker.SelectedIndexProperty, -1);
			stackLayout6.Children.Add(picker);
			translate23.Text = "cdp_TransmissionType";
			IMarkupExtension markupExtension28 = translate23;
			XamlServiceProvider xamlServiceProvider28 = new XamlServiceProvider();
			Type typeFromHandle56 = typeof(IProvideValueTarget);
			object[] array29 = new object[0 + 6];
			array29[0] = label9;
			array29[1] = stackLayout6;
			array29[2] = stackLayout8;
			array29[3] = scrollView;
			array29[4] = grid;
			array29[5] = this;
			object obj51;
			xamlServiceProvider28.Add(typeFromHandle56, obj51 = new SimpleValueTargetProvider(array29, Label.TextProperty, nameScope));
			xamlServiceProvider28.Add(typeof(IReferenceProvider), obj51);
			Type typeFromHandle57 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver28 = new XmlNamespaceResolver();
			xmlNamespaceResolver28.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver28.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver28.Add("d", "http://xamarin.com/schemas/2014/forms/design");
			xmlNamespaceResolver28.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver28.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver28.Add("mc", "http://schemas.openxmlformats.org/markup-compatibility/2006");
			xmlNamespaceResolver28.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver28.Add("syncfusionRB", "clr-namespace:Syncfusion.XForms.Buttons;assembly=Syncfusion.Buttons.XForms");
			xamlServiceProvider28.Add(typeFromHandle57, new XamlTypeResolver(xmlNamespaceResolver28, typeof(ContactDeveloperPage).GetTypeInfo().Assembly));
			xamlServiceProvider28.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(153, 32)));
			object obj52 = markupExtension28.ProvideValue(xamlServiceProvider28);
			label9.Text = obj52;
			stackLayout6.Children.Add(label9);
			picker2.SetValue(Picker.SelectedIndexProperty, -1);
			picker2.SetValue(Picker.ItemsSourceProperty, array);
			stackLayout6.Children.Add(picker2);
			translate24.Text = "cdp_OtherInfo";
			IMarkupExtension markupExtension29 = translate24;
			XamlServiceProvider xamlServiceProvider29 = new XamlServiceProvider();
			Type typeFromHandle58 = typeof(IProvideValueTarget);
			object[] array30 = new object[0 + 6];
			array30[0] = entry6;
			array30[1] = stackLayout6;
			array30[2] = stackLayout8;
			array30[3] = scrollView;
			array30[4] = grid;
			array30[5] = this;
			object obj53;
			xamlServiceProvider29.Add(typeFromHandle58, obj53 = new SimpleValueTargetProvider(array30, Entry.PlaceholderProperty, nameScope));
			xamlServiceProvider29.Add(typeof(IReferenceProvider), obj53);
			Type typeFromHandle59 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver29 = new XmlNamespaceResolver();
			xmlNamespaceResolver29.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver29.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver29.Add("d", "http://xamarin.com/schemas/2014/forms/design");
			xmlNamespaceResolver29.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver29.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver29.Add("mc", "http://schemas.openxmlformats.org/markup-compatibility/2006");
			xmlNamespaceResolver29.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver29.Add("syncfusionRB", "clr-namespace:Syncfusion.XForms.Buttons;assembly=Syncfusion.Buttons.XForms");
			xamlServiceProvider29.Add(typeFromHandle59, new XamlTypeResolver(xmlNamespaceResolver29, typeof(ContactDeveloperPage).GetTypeInfo().Assembly));
			xamlServiceProvider29.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(165, 56)));
			object obj54 = markupExtension29.ProvideValue(xamlServiceProvider29);
			entry6.Placeholder = obj54;
			stackLayout6.Children.Add(entry6);
			stackLayout8.Children.Add(stackLayout6);
			stackLayout7.SetValue(VisualElement.IsVisibleProperty, new VisualElement.VisibilityConverter().ConvertFromInvariantString("False"));
			stackLayout7.SetValue(StackLayout.OrientationProperty, 0);
			translate25.Text = "contactDeveloper_missingCodingService_Text";
			IMarkupExtension markupExtension30 = translate25;
			XamlServiceProvider xamlServiceProvider30 = new XamlServiceProvider();
			Type typeFromHandle60 = typeof(IProvideValueTarget);
			object[] array31 = new object[0 + 6];
			array31[0] = label10;
			array31[1] = stackLayout7;
			array31[2] = stackLayout8;
			array31[3] = scrollView;
			array31[4] = grid;
			array31[5] = this;
			object obj55;
			xamlServiceProvider30.Add(typeFromHandle60, obj55 = new SimpleValueTargetProvider(array31, Label.TextProperty, nameScope));
			xamlServiceProvider30.Add(typeof(IReferenceProvider), obj55);
			Type typeFromHandle61 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver30 = new XmlNamespaceResolver();
			xmlNamespaceResolver30.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver30.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver30.Add("d", "http://xamarin.com/schemas/2014/forms/design");
			xmlNamespaceResolver30.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver30.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver30.Add("mc", "http://schemas.openxmlformats.org/markup-compatibility/2006");
			xmlNamespaceResolver30.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver30.Add("syncfusionRB", "clr-namespace:Syncfusion.XForms.Buttons;assembly=Syncfusion.Buttons.XForms");
			xamlServiceProvider30.Add(typeFromHandle61, new XamlTypeResolver(xmlNamespaceResolver30, typeof(ContactDeveloperPage).GetTypeInfo().Assembly));
			xamlServiceProvider30.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(173, 59)));
			object obj56 = markupExtension30.ProvideValue(xamlServiceProvider30);
			label10.Text = obj56;
			tapGestureRecognizer.SetValue(TapGestureRecognizer.NumberOfTapsRequiredProperty, 1);
			tapGestureRecognizer.Tapped += this.labelNoCoding_Tapped;
			label10.GestureRecognizers.Add(tapGestureRecognizer);
			stackLayout7.Children.Add(label10);
			stackLayout8.Children.Add(stackLayout7);
			button4.SetValue(Grid.RowProperty, 1);
			button4.Clicked += this.BtnSend_Clicked;
			button4.SetValue(VisualElement.IsEnabledProperty, true);
			button4.SetValue(VisualElement.IsVisibleProperty, new VisualElement.VisibilityConverter().ConvertFromInvariantString("False"));
			translate26.Text = "ios_ContactDeveloper";
			IMarkupExtension markupExtension31 = translate26;
			XamlServiceProvider xamlServiceProvider31 = new XamlServiceProvider();
			Type typeFromHandle62 = typeof(IProvideValueTarget);
			object[] array32 = new object[0 + 5];
			array32[0] = button4;
			array32[1] = stackLayout8;
			array32[2] = scrollView;
			array32[3] = grid;
			array32[4] = this;
			object obj57;
			xamlServiceProvider31.Add(typeFromHandle62, obj57 = new SimpleValueTargetProvider(array32, Button.TextProperty, nameScope));
			xamlServiceProvider31.Add(typeof(IReferenceProvider), obj57);
			Type typeFromHandle63 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver31 = new XmlNamespaceResolver();
			xmlNamespaceResolver31.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver31.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver31.Add("d", "http://xamarin.com/schemas/2014/forms/design");
			xmlNamespaceResolver31.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver31.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver31.Add("mc", "http://schemas.openxmlformats.org/markup-compatibility/2006");
			xmlNamespaceResolver31.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver31.Add("syncfusionRB", "clr-namespace:Syncfusion.XForms.Buttons;assembly=Syncfusion.Buttons.XForms");
			xamlServiceProvider31.Add(typeFromHandle63, new XamlTypeResolver(xmlNamespaceResolver31, typeof(ContactDeveloperPage).GetTypeInfo().Assembly));
			xamlServiceProvider31.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(186, 25)));
			object obj58 = markupExtension31.ProvideValue(xamlServiceProvider31);
			button4.Text = obj58;
			stackLayout8.Children.Add(button4);
			scrollView.Content = stackLayout8;
			grid.Children.Add(scrollView);
			this.SetValue(ContentPage.ContentProperty, grid);
		}

		// Token: 0x06001A10 RID: 6672 RVA: 0x00117720 File Offset: 0x00115920
		private void __InitComponentRuntime()
		{
			Extensions.LoadFromXaml<ContactDeveloperPage>(this, typeof(ContactDeveloperPage));
			this.scrollView = NameScopeExtensions.FindByName<ScrollView>(this, "scrollView");
			this.rbConnectionIssue = NameScopeExtensions.FindByName<SfRadioButton>(this, "rbConnectionIssue");
			this.rbPurchaseIssue = NameScopeExtensions.FindByName<SfRadioButton>(this, "rbPurchaseIssue");
			this.rbOther = NameScopeExtensions.FindByName<SfRadioButton>(this, "rbOther");
			this.rbNoCoding = NameScopeExtensions.FindByName<SfRadioButton>(this, "rbNoCoding");
			this.panelPurchaseProblems = NameScopeExtensions.FindByName<StackLayout>(this, "panelPurchaseProblems");
			this.panelConnectionProblems = NameScopeExtensions.FindByName<StackLayout>(this, "panelConnectionProblems");
			this.labelConnectionTips = NameScopeExtensions.FindByName<Label>(this, "labelConnectionTips");
			this.panelProblemSolved = NameScopeExtensions.FindByName<StackLayout>(this, "panelProblemSolved");
			this.btnProblemSolved = NameScopeExtensions.FindByName<Button>(this, "btnProblemSolved");
			this.btnProblemNotSolved = NameScopeExtensions.FindByName<Button>(this, "btnProblemNotSolved");
			this.panelDescribeYourProblem = NameScopeExtensions.FindByName<StackLayout>(this, "panelDescribeYourProblem");
			this.editorDescribeProblem = NameScopeExtensions.FindByName<Editor>(this, "editorDescribeProblem");
			this.btnNext = NameScopeExtensions.FindByName<Button>(this, "btnNext");
			this.panelCarDetails = NameScopeExtensions.FindByName<StackLayout>(this, "panelCarDetails");
			this.entryBrand = NameScopeExtensions.FindByName<Entry>(this, "entryBrand");
			this.entryModel = NameScopeExtensions.FindByName<Entry>(this, "entryModel");
			this.entryYear = NameScopeExtensions.FindByName<Entry>(this, "entryYear");
			this.entryEngineVolume = NameScopeExtensions.FindByName<Entry>(this, "entryEngineVolume");
			this.entryEngineModel = NameScopeExtensions.FindByName<Entry>(this, "entryEngineModel");
			this.pickerFuelType = NameScopeExtensions.FindByName<Picker>(this, "pickerFuelType");
			this.pickerTransmissionType = NameScopeExtensions.FindByName<Picker>(this, "pickerTransmissionType");
			this.entryOtherInfo = NameScopeExtensions.FindByName<Entry>(this, "entryOtherInfo");
			this.panelNoCoding = NameScopeExtensions.FindByName<StackLayout>(this, "panelNoCoding");
			this.labelNoCodingText = NameScopeExtensions.FindByName<Label>(this, "labelNoCodingText");
			this.btnSend = NameScopeExtensions.FindByName<Button>(this, "btnSend");
		}

		// Token: 0x06001A11 RID: 6673 RVA: 0x001178F8 File Offset: 0x00115AF8
		[CompilerGenerated]
		private static ValueTuple<bool, bool> <InitializeComponent>typedBindingsM__1524(SharedSettings A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<bool, bool>(A_0.AdsProductPurchased, true);
			}
			return default(ValueTuple<bool, bool>);
		}

		// Token: 0x06001A12 RID: 6674 RVA: 0x00117928 File Offset: 0x00115B28
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__1525(SharedSettings A_0)
		{
			return A_0;
		}

		// Token: 0x06001A13 RID: 6675 RVA: 0x00117938 File Offset: 0x00115B38
		[CompilerGenerated]
		private static ValueTuple<bool, bool> <InitializeComponent>typedBindingsM__1526(SharedSettings A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<bool, bool>(A_0.AdsProductPurchased, true);
			}
			return default(ValueTuple<bool, bool>);
		}

		// Token: 0x06001A14 RID: 6676 RVA: 0x00117968 File Offset: 0x00115B68
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__1527(SharedSettings A_0)
		{
			return A_0;
		}

		// Token: 0x04000B33 RID: 2867
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private ScrollView scrollView;

		// Token: 0x04000B34 RID: 2868
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private SfRadioButton rbConnectionIssue;

		// Token: 0x04000B35 RID: 2869
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private SfRadioButton rbPurchaseIssue;

		// Token: 0x04000B36 RID: 2870
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private SfRadioButton rbOther;

		// Token: 0x04000B37 RID: 2871
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private SfRadioButton rbNoCoding;

		// Token: 0x04000B38 RID: 2872
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private StackLayout panelPurchaseProblems;

		// Token: 0x04000B39 RID: 2873
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private StackLayout panelConnectionProblems;

		// Token: 0x04000B3A RID: 2874
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private Label labelConnectionTips;

		// Token: 0x04000B3B RID: 2875
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private StackLayout panelProblemSolved;

		// Token: 0x04000B3C RID: 2876
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private Button btnProblemSolved;

		// Token: 0x04000B3D RID: 2877
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private Button btnProblemNotSolved;

		// Token: 0x04000B3E RID: 2878
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private StackLayout panelDescribeYourProblem;

		// Token: 0x04000B3F RID: 2879
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private Editor editorDescribeProblem;

		// Token: 0x04000B40 RID: 2880
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private Button btnNext;

		// Token: 0x04000B41 RID: 2881
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private StackLayout panelCarDetails;

		// Token: 0x04000B42 RID: 2882
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private Entry entryBrand;

		// Token: 0x04000B43 RID: 2883
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private Entry entryModel;

		// Token: 0x04000B44 RID: 2884
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private Entry entryYear;

		// Token: 0x04000B45 RID: 2885
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private Entry entryEngineVolume;

		// Token: 0x04000B46 RID: 2886
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private Entry entryEngineModel;

		// Token: 0x04000B47 RID: 2887
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private Picker pickerFuelType;

		// Token: 0x04000B48 RID: 2888
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private Picker pickerTransmissionType;

		// Token: 0x04000B49 RID: 2889
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private Entry entryOtherInfo;

		// Token: 0x04000B4A RID: 2890
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private StackLayout panelNoCoding;

		// Token: 0x04000B4B RID: 2891
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private Label labelNoCodingText;

		// Token: 0x04000B4C RID: 2892
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private Button btnSend;

		// Token: 0x020001F3 RID: 499
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <BtnNext_Clicked>d__5 : IAsyncStateMachine
		{
			// Token: 0x06001A15 RID: 6677 RVA: 0x00117978 File Offset: 0x00115B78
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				ContactDeveloperPage contactDeveloperPage = this;
				try
				{
					TaskAwaiter taskAwaiter;
					TaskAwaiter taskAwaiter2;
					if (num != 0)
					{
						if (num != 1)
						{
							if (string.IsNullOrEmpty(contactDeveloperPage.editorDescribeProblem.Text) || string.IsNullOrEmpty(contactDeveloperPage.editorDescribeProblem.Text.Trim()))
							{
								taskAwaiter = contactDeveloperPage.DisplayAlert(Translate.GetString("cdp_DescribeProblem"), "", "OK").GetAwaiter();
								if (!taskAwaiter.IsCompleted)
								{
									num2 = 0;
									taskAwaiter2 = taskAwaiter;
									this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, ContactDeveloperPage.<BtnNext_Clicked>d__5>(ref taskAwaiter, ref this);
									return;
								}
								goto IL_00A9;
							}
							else
							{
								contactDeveloperPage.btnNext.IsVisible = false;
								contactDeveloperPage.panelCarDetails.IsVisible = true;
								contactDeveloperPage.panelProblemSolved.IsVisible = false;
								contactDeveloperPage.panelConnectionProblems.IsVisible = false;
								contactDeveloperPage.panelPurchaseProblems.IsVisible = false;
								contactDeveloperPage.btnSend.IsVisible = true;
								contactDeveloperPage.panelDescribeYourProblem.IsVisible = true;
								taskAwaiter = Task.Delay(200).GetAwaiter();
								if (!taskAwaiter.IsCompleted)
								{
									num2 = 1;
									taskAwaiter2 = taskAwaiter;
									this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, ContactDeveloperPage.<BtnNext_Clicked>d__5>(ref taskAwaiter, ref this);
									return;
								}
							}
						}
						else
						{
							taskAwaiter = taskAwaiter2;
							taskAwaiter2 = default(TaskAwaiter);
							num2 = -1;
						}
						taskAwaiter.GetResult();
						contactDeveloperPage.scrollView.ScrollToAsync(contactDeveloperPage.btnSend, 0, true);
						goto IL_0192;
					}
					taskAwaiter = taskAwaiter2;
					taskAwaiter2 = default(TaskAwaiter);
					num2 = -1;
					IL_00A9:
					taskAwaiter.GetResult();
				}
				catch (Exception ex)
				{
					num2 = -2;
					this.<>t__builder.SetException(ex);
					return;
				}
				IL_0192:
				num2 = -2;
				this.<>t__builder.SetResult();
			}

			// Token: 0x06001A16 RID: 6678 RVA: 0x00117B48 File Offset: 0x00115D48
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x04000B4D RID: 2893
			public int <>1__state;

			// Token: 0x04000B4E RID: 2894
			public AsyncVoidMethodBuilder <>t__builder;

			// Token: 0x04000B4F RID: 2895
			public ContactDeveloperPage <>4__this;

			// Token: 0x04000B50 RID: 2896
			private TaskAwaiter <>u__1;
		}

		// Token: 0x020001F4 RID: 500
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <BtnProblemNotSolved_Clicked>d__3 : IAsyncStateMachine
		{
			// Token: 0x06001A17 RID: 6679 RVA: 0x00117B58 File Offset: 0x00115D58
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				ContactDeveloperPage contactDeveloperPage = this;
				try
				{
					TaskAwaiter taskAwaiter;
					TaskAwaiter taskAwaiter2;
					if (num != 0)
					{
						if (num != 1)
						{
							contactDeveloperPage.panelProblemSolved.IsVisible = false;
							contactDeveloperPage.panelDescribeYourProblem.IsVisible = true;
							if (contactDeveloperPage.rbPurchaseIssue.IsChecked.GetValueOrDefault())
							{
								contactDeveloperPage.btnNext.IsVisible = false;
								contactDeveloperPage.btnSend.IsVisible = true;
								taskAwaiter = Task.Delay(200).GetAwaiter();
								if (!taskAwaiter.IsCompleted)
								{
									num2 = 0;
									taskAwaiter2 = taskAwaiter;
									this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, ContactDeveloperPage.<BtnProblemNotSolved_Clicked>d__3>(ref taskAwaiter, ref this);
									return;
								}
								goto IL_00BB;
							}
							else
							{
								contactDeveloperPage.btnNext.IsVisible = true;
								taskAwaiter = Task.Delay(200).GetAwaiter();
								if (!taskAwaiter.IsCompleted)
								{
									num2 = 1;
									taskAwaiter2 = taskAwaiter;
									this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, ContactDeveloperPage.<BtnProblemNotSolved_Clicked>d__3>(ref taskAwaiter, ref this);
									return;
								}
							}
						}
						else
						{
							taskAwaiter = taskAwaiter2;
							taskAwaiter2 = default(TaskAwaiter);
							num2 = -1;
						}
						taskAwaiter.GetResult();
						contactDeveloperPage.scrollView.ScrollToAsync(contactDeveloperPage.btnNext, 0, true);
						goto IL_0154;
					}
					taskAwaiter = taskAwaiter2;
					taskAwaiter2 = default(TaskAwaiter);
					num2 = -1;
					IL_00BB:
					taskAwaiter.GetResult();
					contactDeveloperPage.scrollView.ScrollToAsync(contactDeveloperPage.btnNext, 0, true);
					IL_0154:;
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

			// Token: 0x06001A18 RID: 6680 RVA: 0x00117D04 File Offset: 0x00115F04
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x04000B51 RID: 2897
			public int <>1__state;

			// Token: 0x04000B52 RID: 2898
			public AsyncVoidMethodBuilder <>t__builder;

			// Token: 0x04000B53 RID: 2899
			public ContactDeveloperPage <>4__this;

			// Token: 0x04000B54 RID: 2900
			private TaskAwaiter <>u__1;
		}

		// Token: 0x020001F5 RID: 501
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <BtnSend_Clicked>d__1 : IAsyncStateMachine
		{
			// Token: 0x06001A19 RID: 6681 RVA: 0x00117D14 File Offset: 0x00115F14
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				ContactDeveloperPage contactDeveloperPage = this;
				try
				{
					TaskAwaiter taskAwaiter;
					TaskAwaiter<string> taskAwaiter3;
					TaskAwaiter<Page> taskAwaiter5;
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
						goto IL_01CC;
					}
					case 2:
					{
						TaskAwaiter<string> taskAwaiter4;
						taskAwaiter3 = taskAwaiter4;
						taskAwaiter4 = default(TaskAwaiter<string>);
						num2 = -1;
						goto IL_03FE;
					}
					case 3:
					{
						TaskAwaiter taskAwaiter2;
						taskAwaiter = taskAwaiter2;
						taskAwaiter2 = default(TaskAwaiter);
						num2 = -1;
						goto IL_0492;
					}
					case 4:
					{
						TaskAwaiter<Page> taskAwaiter6;
						taskAwaiter5 = taskAwaiter6;
						taskAwaiter6 = default(TaskAwaiter<Page>);
						num2 = -1;
						goto IL_04F2;
					}
					default:
					{
						bool flag = false;
						int num3;
						if (contactDeveloperPage.rbPurchaseIssue.IsChecked.GetValueOrDefault())
						{
							flag = true;
						}
						else if (!string.IsNullOrEmpty(contactDeveloperPage.entryBrand.Text) && !string.IsNullOrEmpty(contactDeveloperPage.entryModel.Text) && !string.IsNullOrEmpty(contactDeveloperPage.entryYear.Text) && int.TryParse(contactDeveloperPage.entryYear.Text, out num3) && !string.IsNullOrEmpty(contactDeveloperPage.entryEngineVolume.Text) && contactDeveloperPage.pickerFuelType.SelectedIndex >= 0 && contactDeveloperPage.pickerTransmissionType.SelectedIndex >= 0)
						{
							flag = true;
						}
						if (!flag)
						{
							taskAwaiter = contactDeveloperPage.DisplayAlert(Translate.GetString("cdp_FIllCarDetails"), "", "OK").GetAwaiter();
							if (!taskAwaiter.IsCompleted)
							{
								num2 = 0;
								TaskAwaiter taskAwaiter2 = taskAwaiter;
								this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, ContactDeveloperPage.<BtnSend_Clicked>d__1>(ref taskAwaiter, ref this);
								return;
							}
						}
						else if (string.IsNullOrEmpty(contactDeveloperPage.editorDescribeProblem.Text) || string.IsNullOrEmpty(contactDeveloperPage.editorDescribeProblem.Text.Trim()))
						{
							taskAwaiter = contactDeveloperPage.DisplayAlert(Translate.GetString("cdp_DescribeProblem"), "", "OK").GetAwaiter();
							if (!taskAwaiter.IsCompleted)
							{
								num2 = 1;
								TaskAwaiter taskAwaiter2 = taskAwaiter;
								this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, ContactDeveloperPage.<BtnSend_Clicked>d__1>(ref taskAwaiter, ref this);
								return;
							}
							goto IL_01CC;
						}
						else
						{
							issue_type = "[Problem type: ";
							if (contactDeveloperPage.rbConnectionIssue.IsChecked.GetValueOrDefault())
							{
								issue_type += "connection]\n";
							}
							else if (contactDeveloperPage.rbPurchaseIssue.IsChecked.GetValueOrDefault())
							{
								issue_type += "purchase]\n";
							}
							else if (contactDeveloperPage.rbOther.IsChecked.GetValueOrDefault())
							{
								issue_type += "other]\n";
							}
							user_info = "[Problem description]\n" + contactDeveloperPage.editorDescribeProblem.Text + "\n";
							bool? isChecked = contactDeveloperPage.rbPurchaseIssue.IsChecked;
							bool flag2 = false;
							if ((isChecked.GetValueOrDefault() == flag2) & (isChecked != null))
							{
								user_info = string.Concat(new string[]
								{
									user_info,
									"\n[Car details:]\nBrand: ",
									contactDeveloperPage.entryBrand.Text,
									"\nModel: ",
									contactDeveloperPage.entryModel.Text,
									"\nYear: ",
									contactDeveloperPage.entryYear.Text,
									"\nEngine volume:",
									contactDeveloperPage.entryEngineVolume.Text,
									"\nEngine model:",
									contactDeveloperPage.entryEngineModel.Text,
									"\nFuel type: ",
									StaticLists.FuelTypesList[contactDeveloperPage.pickerFuelType.SelectedIndex],
									"\n",
									string.Format("Transmission: {0}\n", contactDeveloperPage.pickerTransmissionType.SelectedItem),
									"Other: ",
									contactDeveloperPage.entryOtherInfo.Text,
									"\n"
								});
							}
							taskAwaiter3 = SharedSettings.Current.GetSettingsReport().GetAwaiter();
							if (!taskAwaiter3.IsCompleted)
							{
								num2 = 2;
								TaskAwaiter<string> taskAwaiter4 = taskAwaiter3;
								this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<string>, ContactDeveloperPage.<BtnSend_Clicked>d__1>(ref taskAwaiter3, ref this);
								return;
							}
							goto IL_03FE;
						}
						break;
					}
					}
					taskAwaiter.GetResult();
					goto IL_0523;
					IL_01CC:
					taskAwaiter.GetResult();
					goto IL_0523;
					IL_03FE:
					string result = taskAwaiter3.GetResult();
					string text = string.Concat(new string[] { issue_type, "\n", user_info, "\n", result });
					taskAwaiter = contactDeveloperPage.SendEmail(text).GetAwaiter();
					if (!taskAwaiter.IsCompleted)
					{
						num2 = 3;
						TaskAwaiter taskAwaiter2 = taskAwaiter;
						this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, ContactDeveloperPage.<BtnSend_Clicked>d__1>(ref taskAwaiter, ref this);
						return;
					}
					IL_0492:
					taskAwaiter.GetResult();
					taskAwaiter5 = contactDeveloperPage.Navigation.PopAsync().GetAwaiter();
					if (!taskAwaiter5.IsCompleted)
					{
						num2 = 4;
						TaskAwaiter<Page> taskAwaiter6 = taskAwaiter5;
						this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<Page>, ContactDeveloperPage.<BtnSend_Clicked>d__1>(ref taskAwaiter5, ref this);
						return;
					}
					IL_04F2:
					taskAwaiter5.GetResult();
				}
				catch (Exception ex)
				{
					num2 = -2;
					issue_type = null;
					user_info = null;
					this.<>t__builder.SetException(ex);
					return;
				}
				IL_0523:
				num2 = -2;
				issue_type = null;
				user_info = null;
				this.<>t__builder.SetResult();
			}

			// Token: 0x06001A1A RID: 6682 RVA: 0x00118284 File Offset: 0x00116484
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x04000B55 RID: 2901
			public int <>1__state;

			// Token: 0x04000B56 RID: 2902
			public AsyncVoidMethodBuilder <>t__builder;

			// Token: 0x04000B57 RID: 2903
			public ContactDeveloperPage <>4__this;

			// Token: 0x04000B58 RID: 2904
			private string <issue_type>5__2;

			// Token: 0x04000B59 RID: 2905
			private string <user_info>5__3;

			// Token: 0x04000B5A RID: 2906
			private TaskAwaiter <>u__1;

			// Token: 0x04000B5B RID: 2907
			private TaskAwaiter<string> <>u__2;

			// Token: 0x04000B5C RID: 2908
			private TaskAwaiter<Page> <>u__3;
		}

		// Token: 0x020001F6 RID: 502
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <RbIssue_StateChanged>d__6 : IAsyncStateMachine
		{
			// Token: 0x06001A1B RID: 6683 RVA: 0x00118294 File Offset: 0x00116494
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				ContactDeveloperPage contactDeveloperPage = this;
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
						goto IL_0272;
					}
					case 2:
					{
						TaskAwaiter taskAwaiter2;
						taskAwaiter = taskAwaiter2;
						taskAwaiter2 = default(TaskAwaiter);
						num2 = -1;
						goto IL_035F;
					}
					default:
						if (contactDeveloperPage.rbConnectionIssue.IsChecked.GetValueOrDefault())
						{
							contactDeveloperPage.panelConnectionProblems.IsVisible = true;
							contactDeveloperPage.panelCarDetails.IsVisible = false;
							contactDeveloperPage.panelProblemSolved.IsVisible = true;
							contactDeveloperPage.panelPurchaseProblems.IsVisible = false;
							contactDeveloperPage.btnSend.IsVisible = false;
							contactDeveloperPage.panelDescribeYourProblem.IsVisible = false;
							contactDeveloperPage.btnNext.IsVisible = true;
							contactDeveloperPage.panelNoCoding.IsVisible = false;
							goto IL_037A;
						}
						if (contactDeveloperPage.rbPurchaseIssue.IsChecked.GetValueOrDefault())
						{
							contactDeveloperPage.panelConnectionProblems.IsVisible = false;
							contactDeveloperPage.panelCarDetails.IsVisible = false;
							contactDeveloperPage.panelProblemSolved.IsVisible = true;
							contactDeveloperPage.panelPurchaseProblems.IsVisible = true;
							contactDeveloperPage.btnSend.IsVisible = false;
							contactDeveloperPage.panelDescribeYourProblem.IsVisible = false;
							contactDeveloperPage.btnNext.IsVisible = false;
							contactDeveloperPage.panelNoCoding.IsVisible = false;
							if (SharedSettings.Current.AdsProductPurchased)
							{
								contactDeveloperPage.panelProblemSolved.IsVisible = false;
							}
							taskAwaiter = Task.Delay(200).GetAwaiter();
							if (!taskAwaiter.IsCompleted)
							{
								num2 = 0;
								TaskAwaiter taskAwaiter2 = taskAwaiter;
								this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, ContactDeveloperPage.<RbIssue_StateChanged>d__6>(ref taskAwaiter, ref this);
								return;
							}
						}
						else if (contactDeveloperPage.rbOther.IsChecked.GetValueOrDefault())
						{
							contactDeveloperPage.panelConnectionProblems.IsVisible = false;
							contactDeveloperPage.panelCarDetails.IsVisible = false;
							contactDeveloperPage.panelProblemSolved.IsVisible = false;
							contactDeveloperPage.panelPurchaseProblems.IsVisible = false;
							contactDeveloperPage.btnSend.IsVisible = false;
							contactDeveloperPage.panelDescribeYourProblem.IsVisible = true;
							contactDeveloperPage.btnNext.IsVisible = true;
							contactDeveloperPage.panelNoCoding.IsVisible = false;
							taskAwaiter = Task.Delay(200).GetAwaiter();
							if (!taskAwaiter.IsCompleted)
							{
								num2 = 1;
								TaskAwaiter taskAwaiter2 = taskAwaiter;
								this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, ContactDeveloperPage.<RbIssue_StateChanged>d__6>(ref taskAwaiter, ref this);
								return;
							}
							goto IL_0272;
						}
						else
						{
							if (!contactDeveloperPage.rbNoCoding.IsChecked.GetValueOrDefault())
							{
								goto IL_037A;
							}
							contactDeveloperPage.panelConnectionProblems.IsVisible = false;
							contactDeveloperPage.panelCarDetails.IsVisible = false;
							contactDeveloperPage.panelProblemSolved.IsVisible = false;
							contactDeveloperPage.panelPurchaseProblems.IsVisible = false;
							contactDeveloperPage.btnSend.IsVisible = false;
							contactDeveloperPage.panelDescribeYourProblem.IsVisible = false;
							contactDeveloperPage.btnNext.IsVisible = false;
							contactDeveloperPage.panelNoCoding.IsVisible = true;
							taskAwaiter = Task.Delay(200).GetAwaiter();
							if (!taskAwaiter.IsCompleted)
							{
								num2 = 2;
								TaskAwaiter taskAwaiter2 = taskAwaiter;
								this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, ContactDeveloperPage.<RbIssue_StateChanged>d__6>(ref taskAwaiter, ref this);
								return;
							}
							goto IL_035F;
						}
						break;
					}
					taskAwaiter.GetResult();
					contactDeveloperPage.scrollView.ScrollToAsync(contactDeveloperPage.btnSend, 0, true);
					goto IL_037A;
					IL_0272:
					taskAwaiter.GetResult();
					contactDeveloperPage.scrollView.ScrollToAsync(contactDeveloperPage.btnSend, 0, true);
					goto IL_037A;
					IL_035F:
					taskAwaiter.GetResult();
					contactDeveloperPage.scrollView.ScrollToAsync(contactDeveloperPage.btnSend, 0, true);
					IL_037A:;
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

			// Token: 0x06001A1C RID: 6684 RVA: 0x00118668 File Offset: 0x00116868
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x04000B5D RID: 2909
			public int <>1__state;

			// Token: 0x04000B5E RID: 2910
			public AsyncVoidMethodBuilder <>t__builder;

			// Token: 0x04000B5F RID: 2911
			public ContactDeveloperPage <>4__this;

			// Token: 0x04000B60 RID: 2912
			private TaskAwaiter <>u__1;
		}

		// Token: 0x020001F7 RID: 503
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <SendEmail>d__2 : IAsyncStateMachine
		{
			// Token: 0x06001A1D RID: 6685 RVA: 0x00118678 File Offset: 0x00116878
			void IAsyncStateMachine.MoveNext()
			{
				int num3;
				int num2 = num3;
				ContactDeveloperPage contactDeveloperPage = this;
				try
				{
					switch (num2)
					{
					case 0:
					case 1:
					case 2:
						break;
					case 3:
					case 4:
					case 5:
						goto IL_0254;
					case 6:
						goto IL_039B;
					case 7:
						goto IL_040E;
					case 8:
						goto IL_0486;
					default:
						num = 0;
						break;
					}
					TaskAwaiter taskAwaiter2;
					try
					{
						TaskAwaiter taskAwaiter;
						switch (num2)
						{
						case 0:
							break;
						case 1:
							taskAwaiter = taskAwaiter2;
							taskAwaiter2 = default(TaskAwaiter);
							num2 = (num3 = -1);
							goto IL_01B0;
						case 2:
							taskAwaiter = taskAwaiter2;
							taskAwaiter2 = default(TaskAwaiter);
							num2 = (num3 = -1);
							goto IL_0210;
						default:
							message = new EmailMessage(" (" + Device.RuntimePlatform + ")", text, new string[] { "admin@carscanner.info" });
							message.BodyFormat = 0;
							message.Subject = "Car Scanner user feedback" + message.Subject;
							break;
						}
						try
						{
							if (num2 != 0)
							{
								taskAwaiter = PCLDebugStream.CurrentInstance.Flush().GetAwaiter();
								if (!taskAwaiter.IsCompleted)
								{
									num2 = (num3 = 0);
									taskAwaiter2 = taskAwaiter;
									this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, ContactDeveloperPage.<SendEmail>d__2>(ref taskAwaiter, ref this);
									return;
								}
							}
							else
							{
								taskAwaiter = taskAwaiter2;
								taskAwaiter2 = default(TaskAwaiter);
								num2 = (num3 = -1);
							}
							taskAwaiter.GetResult();
							PCLDebugStream.CurrentInstance.Close();
						}
						catch (Exception)
						{
						}
						string filepath = PCLDebugStream.GetFilepath();
						if (File.Exists(filepath))
						{
							message.Attachments.Add(new EmailAttachment(filepath));
						}
						taskAwaiter = contactDeveloperPage.DisplayAlert(contactDeveloperPage.Title, Translate.GetString("contactDeveloper_PreSendMessage"), "OK").GetAwaiter();
						if (!taskAwaiter.IsCompleted)
						{
							num2 = (num3 = 1);
							taskAwaiter2 = taskAwaiter;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, ContactDeveloperPage.<SendEmail>d__2>(ref taskAwaiter, ref this);
							return;
						}
						IL_01B0:
						taskAwaiter.GetResult();
						taskAwaiter = Email.ComposeAsync(message).GetAwaiter();
						if (!taskAwaiter.IsCompleted)
						{
							num2 = (num3 = 2);
							taskAwaiter2 = taskAwaiter;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, ContactDeveloperPage.<SendEmail>d__2>(ref taskAwaiter, ref this);
							return;
						}
						IL_0210:
						taskAwaiter.GetResult();
						message = null;
					}
					catch (Exception ex)
					{
						obj = ex;
						num = 1;
					}
					int num4 = num;
					if (num4 != 1)
					{
						goto IL_051C;
					}
					exc = (Exception)obj;
					IL_0254:
					try
					{
						TaskAwaiter taskAwaiter;
						switch (num2)
						{
						case 3:
							taskAwaiter = taskAwaiter2;
							taskAwaiter2 = default(TaskAwaiter);
							num2 = (num3 = -1);
							break;
						case 4:
							taskAwaiter = taskAwaiter2;
							taskAwaiter2 = default(TaskAwaiter);
							num2 = (num3 = -1);
							goto IL_032B;
						case 5:
							taskAwaiter = taskAwaiter2;
							taskAwaiter2 = default(TaskAwaiter);
							num2 = (num3 = -1);
							goto IL_038F;
						default:
							taskAwaiter = App.OBDReader.DebugWrite("\r\n[///**** CONTACT DEVELOPER REPORT:").GetAwaiter();
							if (!taskAwaiter.IsCompleted)
							{
								num2 = (num3 = 3);
								taskAwaiter2 = taskAwaiter;
								this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, ContactDeveloperPage.<SendEmail>d__2>(ref taskAwaiter, ref this);
								return;
							}
							break;
						}
						taskAwaiter.GetResult();
						taskAwaiter = App.OBDReader.DebugWrite(text).GetAwaiter();
						if (!taskAwaiter.IsCompleted)
						{
							num2 = (num3 = 4);
							taskAwaiter2 = taskAwaiter;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, ContactDeveloperPage.<SendEmail>d__2>(ref taskAwaiter, ref this);
							return;
						}
						IL_032B:
						taskAwaiter.GetResult();
						taskAwaiter = App.OBDReader.DebugWrite("\r\n[***/// END OF CONTACT DEVELOPER REPORT]").GetAwaiter();
						if (!taskAwaiter.IsCompleted)
						{
							num2 = (num3 = 5);
							taskAwaiter2 = taskAwaiter;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, ContactDeveloperPage.<SendEmail>d__2>(ref taskAwaiter, ref this);
							return;
						}
						IL_038F:
						taskAwaiter.GetResult();
					}
					catch (Exception)
					{
					}
					IL_039B:
					try
					{
						TaskAwaiter taskAwaiter;
						if (num2 != 6)
						{
							taskAwaiter = PCLDebugStream.CurrentInstance.Flush().GetAwaiter();
							if (!taskAwaiter.IsCompleted)
							{
								num2 = (num3 = 6);
								taskAwaiter2 = taskAwaiter;
								this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, ContactDeveloperPage.<SendEmail>d__2>(ref taskAwaiter, ref this);
								return;
							}
						}
						else
						{
							taskAwaiter = taskAwaiter2;
							taskAwaiter2 = default(TaskAwaiter);
							num2 = (num3 = -1);
						}
						taskAwaiter.GetResult();
						PCLDebugStream.CurrentInstance.Close();
					}
					catch (Exception)
					{
					}
					IL_040E:
					try
					{
						TaskAwaiter taskAwaiter;
						if (num2 != 7)
						{
							taskAwaiter = App.GetCurrentPage().DisplayAlert("Please send this log file to admin@carscanner.info", "Please send email to:\nadmin@carscanner.info including this log file.", "OK").GetAwaiter();
							if (!taskAwaiter.IsCompleted)
							{
								num2 = (num3 = 7);
								taskAwaiter2 = taskAwaiter;
								this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, ContactDeveloperPage.<SendEmail>d__2>(ref taskAwaiter, ref this);
								return;
							}
						}
						else
						{
							taskAwaiter = taskAwaiter2;
							taskAwaiter2 = default(TaskAwaiter);
							num2 = (num3 = -1);
						}
						taskAwaiter.GetResult();
					}
					catch (Exception)
					{
					}
					IL_0486:
					try
					{
						TaskAwaiter taskAwaiter;
						if (num2 != 8)
						{
							taskAwaiter = Share.RequestAsync(new ShareFileRequest(new ShareFile(PCLDebugStream.GetFilepath()))).GetAwaiter();
							if (!taskAwaiter.IsCompleted)
							{
								num2 = (num3 = 8);
								taskAwaiter2 = taskAwaiter;
								this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, ContactDeveloperPage.<SendEmail>d__2>(ref taskAwaiter, ref this);
								return;
							}
						}
						else
						{
							taskAwaiter = taskAwaiter2;
							taskAwaiter2 = default(TaskAwaiter);
							num2 = (num3 = -1);
						}
						taskAwaiter.GetResult();
					}
					catch (Exception)
					{
						contactDeveloperPage.DisplayAlert("Error", exc.Message, "OK");
					}
					exc = null;
					IL_051C:
					obj = null;
				}
				catch (Exception ex2)
				{
					num3 = -2;
					this.<>t__builder.SetException(ex2);
					return;
				}
				num3 = -2;
				this.<>t__builder.SetResult();
			}

			// Token: 0x06001A1E RID: 6686 RVA: 0x00118C84 File Offset: 0x00116E84
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x04000B61 RID: 2913
			public int <>1__state;

			// Token: 0x04000B62 RID: 2914
			public AsyncTaskMethodBuilder <>t__builder;

			// Token: 0x04000B63 RID: 2915
			public string text;

			// Token: 0x04000B64 RID: 2916
			public ContactDeveloperPage <>4__this;

			// Token: 0x04000B65 RID: 2917
			private object <>7__wrap1;

			// Token: 0x04000B66 RID: 2918
			private int <>7__wrap2;

			// Token: 0x04000B67 RID: 2919
			private EmailMessage <message>5__4;

			// Token: 0x04000B68 RID: 2920
			private TaskAwaiter <>u__1;

			// Token: 0x04000B69 RID: 2921
			private Exception <exc>5__5;
		}

		// Token: 0x020001F8 RID: 504
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <labelNoCoding_Tapped>d__7 : IAsyncStateMachine
		{
			// Token: 0x06001A1F RID: 6687 RVA: 0x00118C94 File Offset: 0x00116E94
			void IAsyncStateMachine.MoveNext()
			{
				try
				{
					try
					{
						Launcher.TryOpenAsync("https://www.carscanner.info/coding/");
					}
					catch (Exception)
					{
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

			// Token: 0x06001A20 RID: 6688 RVA: 0x00118CFC File Offset: 0x00116EFC
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x04000B6A RID: 2922
			public int <>1__state;

			// Token: 0x04000B6B RID: 2923
			public AsyncVoidMethodBuilder <>t__builder;
		}
	}
}
