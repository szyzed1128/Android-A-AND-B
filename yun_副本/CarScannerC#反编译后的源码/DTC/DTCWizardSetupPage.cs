using System;
using System.CodeDom.Compiler;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using CarScannerXamarinForms.OBD2;
using CarScannerXamarinForms.ProfilesV2;
using CarScannerXamarinForms.Settings;
using Syncfusion.XForms.Buttons;
using Xamarin.Forms;
using Xamarin.Forms.Internals;
using Xamarin.Forms.PlatformConfiguration.iOSSpecific;
using Xamarin.Forms.Xaml;
using Xamarin.Forms.Xaml.Diagnostics;
using Xamarin.Forms.Xaml.Internals;

namespace CarScannerXamarinForms.DTC
{
	// Token: 0x0200055A RID: 1370
	[XamlCompilation(2)]
	[XamlFilePath("DTC\\DTCWizardSetupPage.xaml")]
	public class DTCWizardSetupPage : ContentPage
	{
		// Token: 0x060032DB RID: 13019 RVA: 0x00236030 File Offset: 0x00234230
		public DTCWizardSetupPage(DTCWizardSetupPage.DTCMode mode)
		{
			this.InitializeComponent();
			if (SharedSettings.Current.ShowExperimental)
			{
				this.sliderSettings.Maximum = 6.0;
			}
			this.labelBrandAndProfile.BindingContext = SharedSettings.Current;
			string text = SharedSettings.Current.SelectedBrand;
			if (string.IsNullOrEmpty(text))
			{
				text = SharedSettings.Current.BrandAndProfile;
			}
			if (text.Contains("nissan") || text.Contains("renault") || text.Contains("dacia") || text.Contains("samsung") || text.Contains("infinit"))
			{
				this.sliderSettings.Value = 1.0;
				this.sliderSettings_ValueChanged(this.sliderSettings, new ValueChangedEventArgs(1.0, 1.0));
			}
			else if (text == "Kia" || text == "Hyundai" || text == "Genesis")
			{
				this.sliderSettings.Value = 3.0;
				this.sliderSettings_ValueChanged(this.sliderSettings, new ValueChangedEventArgs(3.0, 3.0));
			}
			else
			{
				this.sliderSettings.Value = 1.0;
				this.sliderSettings_ValueChanged(this.sliderSettings, new ValueChangedEventArgs(1.0, 1.0));
			}
			this.Mode = mode;
			if (this.Mode == DTCWizardSetupPage.DTCMode.Read)
			{
				base.Title = Translate.GetString("DtcPage_btnRead.Content");
				this.btnStart.Text = base.Title;
				OBDRequest[] profileSequence = DTCWorker.GetProfileSequence(SharedSettings.Current.DTCReadingSequence, true);
				this.swHideUncomplitedTests1.IsVisible = true;
				this.swHideArchiveDTC1.IsVisible = true;
				this.swHideUncomplitedTests2.IsVisible = true;
				this.swHideArchiveDTC2.IsVisible = true;
				if (profileSequence.Length != 0 && (SharedSettings.Current.DTCReadingModeV2 == DTCModeV2.AddToAuto || SharedSettings.Current.DTCReadingModeV2 == DTCModeV2.Auto))
				{
					this.rbProfile.IsChecked = new bool?(true);
					this.rbAdvanced.IsChecked = new bool?(false);
					this.panelMode.IsVisible = true;
					this.panelDTCAdvancedSettings.IsVisible = false;
					this.panelProfileWarning.IsVisible = true;
					this.labelSelectMode.IsVisible = true;
					return;
				}
				if (profileSequence.Length != 0 && SharedSettings.Current.DTCReadingModeV2 == DTCModeV2.ReplaceAuto)
				{
					this.rbProfile.IsChecked = new bool?(true);
					this.rbAdvanced.IsChecked = new bool?(false);
					this.panelMode.IsVisible = false;
					this.panelDTCAdvancedSettings.IsVisible = false;
					this.panelProfileWarning.IsVisible = true;
					this.labelSelectMode.IsVisible = false;
					return;
				}
				if (App.OBDReader.IsNissanConsult2Protocol && App.OBDReader.CurrentELMFormat == ELMFormat.KWP)
				{
					this.rbProfile.IsChecked = new bool?(true);
					this.rbAdvanced.IsChecked = new bool?(false);
					this.panelMode.IsVisible = false;
					this.panelDTCAdvancedSettings.IsVisible = false;
					this.panelProfileWarning.IsVisible = true;
					this.labelSelectMode.IsVisible = false;
					return;
				}
				this.rbAdvanced.IsChecked = new bool?(true);
				this.rbProfile.IsChecked = new bool?(false);
				this.panelMode.IsVisible = false;
				this.panelDTCAdvancedSettings.IsVisible = true;
				this.panelProfileWarning.IsVisible = false;
				this.labelSelectMode.IsVisible = true;
				return;
			}
			else
			{
				this.swHideUncomplitedTests1.IsVisible = false;
				this.swHideArchiveDTC1.IsVisible = false;
				this.swHideUncomplitedTests2.IsVisible = false;
				this.swHideArchiveDTC2.IsVisible = false;
				OBDRequest[] profileSequence2 = DTCWorker.GetProfileSequence(SharedSettings.Current.DTCClearingSequence, false);
				base.Title = Translate.GetString("DtcPage_btnClear.Content");
				this.btnStart.Text = base.Title;
				if (profileSequence2.Length != 0 && SharedSettings.Current.DTCClearingModeV2 == DTCModeV2.ReplaceAuto)
				{
					this.rbProfile.IsChecked = new bool?(true);
					this.rbAdvanced.IsChecked = new bool?(false);
					this.panelMode.IsVisible = false;
					this.panelDTCAdvancedSettings.IsVisible = false;
					this.panelProfileWarning.IsVisible = true;
					this.labelSelectMode.IsVisible = false;
					return;
				}
				if (profileSequence2.Length != 0 && (SharedSettings.Current.DTCClearingModeV2 == DTCModeV2.AddToAuto || SharedSettings.Current.DTCClearingModeV2 == DTCModeV2.Auto))
				{
					this.rbProfile.IsChecked = new bool?(true);
					this.rbAdvanced.IsChecked = new bool?(false);
					this.panelMode.IsVisible = true;
					this.panelDTCAdvancedSettings.IsVisible = false;
					this.panelProfileWarning.IsVisible = true;
					this.labelSelectMode.IsVisible = true;
					return;
				}
				if (App.OBDReader.IsNissanConsult2Protocol && App.OBDReader.CurrentELMFormat == ELMFormat.KWP)
				{
					this.rbProfile.IsChecked = new bool?(true);
					this.rbAdvanced.IsChecked = new bool?(false);
					this.panelMode.IsVisible = false;
					this.panelDTCAdvancedSettings.IsVisible = false;
					this.panelProfileWarning.IsVisible = true;
					this.labelSelectMode.IsVisible = false;
					return;
				}
				this.rbAdvanced.IsChecked = new bool?(true);
				this.rbProfile.IsChecked = new bool?(false);
				this.panelMode.IsVisible = false;
				this.panelDTCAdvancedSettings.IsVisible = true;
				this.panelProfileWarning.IsVisible = false;
				this.labelSelectMode.IsVisible = true;
				return;
			}
		}

		// Token: 0x060032DC RID: 13020 RVA: 0x002365A8 File Offset: 0x002347A8
		private void rb_StateChanged(object sender, StateChangedEventArgs e)
		{
			if (this.rbProfile.IsChecked.GetValueOrDefault())
			{
				bool? isChecked = this.rbAdvanced.IsChecked;
				bool flag = false;
				if ((isChecked.GetValueOrDefault() == flag) & (isChecked != null))
				{
					this.panelDTCAdvancedSettings.IsVisible = false;
					this.panelProfileWarning.IsVisible = true;
					return;
				}
			}
			this.panelDTCAdvancedSettings.IsVisible = true;
			this.panelProfileWarning.IsVisible = false;
		}

		// Token: 0x060032DD RID: 13021 RVA: 0x00236620 File Offset: 0x00234820
		private void sliderSettings_ValueChanged(object sender, ValueChangedEventArgs e)
		{
			int num = (int)Math.Round(e.NewValue);
			this.sliderSettings.Value = (double)num;
			this.labelTitle.Text = Translate.GetString("ios_DTCVar" + num.ToString() + "_Title");
			this.labelVariantDescription.Text = Translate.GetString("ios_DTCVar" + num.ToString() + "_Description");
			switch (num)
			{
			case 0:
			case 1:
			case 3:
				this.labelTitle.TextColor = (Color)Application.Current.Resources["GreenTextColor"];
				return;
			case 2:
			case 4:
			case 5:
				this.labelTitle.TextColor = (Color)Application.Current.Resources["YellowTextColor"];
				return;
			default:
				this.labelTitle.TextColor = (Color)Application.Current.Resources["RedTextColor"];
				return;
			}
		}

		// Token: 0x060032DE RID: 13022 RVA: 0x00236721 File Offset: 0x00234921
		private void BtnStart_Clicked(object sender, EventArgs e)
		{
			this.btnStart.IsEnabled = false;
			if (this.Mode == DTCWizardSetupPage.DTCMode.Clear)
			{
				this.StartClear();
			}
			else
			{
				this.StartRead();
			}
			this.btnStart.IsEnabled = true;
		}

		// Token: 0x060032DF RID: 13023 RVA: 0x00236754 File Offset: 0x00234954
		private async void StartRead()
		{
			this.btnStart.IsEnabled = false;
			if (App.OBDSimulator.IsActive)
			{
				DescriptionLoader.ResetCache();
				DescriptionLoader.PreloadDescriptionsForBrand(string.IsNullOrEmpty(SharedSettings.Current.BrandForDTC) ? SharedSettings.Current.SelectedBrand : SharedSettings.Current.BrandForDTC);
				DTCItemV2 dtcitemV = new DTCItemV2(new byte[] { 22, 147 }, "", "7E8", "03", DTCStatusHelper.DTCStatus.uds_bit3_confirmedDTC, "");
				DTCItemV2 dtcitemV2 = new DTCItemV2(new byte[] { 1, 35 }, "", "7E8", "07", DTCStatusHelper.DTCStatus.uds_bit2_pendingDTC, "");
				DTCItemV2 dtcitemV3 = new DTCItemV2(new byte[] { 3, 1 }, "", "7E8", "03", DTCStatusHelper.DTCStatus.uds_bit3_confirmedDTC, "");
				DTCItemV2 dtcitemV4 = new DTCItemV2(new byte[] { 0, 21, 147 }, "7E0", "7E8", "1902FA", DTCStatusHelper.DTCStatus.uds_bit5_testFailedSinceLastClear, "");
				dtcitemV.LoadDescription();
				dtcitemV2.LoadDescription();
				dtcitemV3.LoadDescription();
				dtcitemV4.LoadDescription();
				App.OBDReader.CurrentCarData.DTCs.Clear();
				App.OBDReader.CurrentCarData.DTCs.Enqueue(dtcitemV);
				App.OBDReader.CurrentCarData.DTCs.Enqueue(dtcitemV3);
				App.OBDReader.CurrentCarData.DTCs.Enqueue(dtcitemV2);
				App.OBDReader.CurrentCarData.DTCs.Enqueue(dtcitemV4);
				DTCWizardResultsPage dtcwizardResultsPage = new DTCWizardResultsPage(DTCWizardSetupPage.DTCMode.Read, new OBDRequest[0]);
				await base.Navigation.PushAsync(dtcwizardResultsPage);
			}
			else
			{
				OBDRequest[] commands = new OBDRequest[0];
				if (this.rbProfile.IsChecked.GetValueOrDefault())
				{
					OBDDataReader obdreader = App.OBDReader;
					await ((obdreader != null) ? obdreader.DebugWrite("\n[DTCRead:Auto]\n") : null);
					if (App.OBDReader.IsNissanConsult2Protocol && App.OBDReader.CurrentELMFormat == ELMFormat.KWP)
					{
						commands = new OBDRequest[]
						{
							new OBDRequest("A3", false, null)
						};
					}
					else
					{
						commands = DTCWorker.GetProfileSequence(SharedSettings.Current.DTCReadingSequence, true);
					}
				}
				else
				{
					if ((int)this.sliderSettings.Value == 2 || (int)this.sliderSettings.Value >= 4)
					{
						TaskAwaiter<bool> taskAwaiter = base.DisplayAlert(Translate.GetString("ios_DTCWarningTitle"), Translate.GetString("ios_DTCWarningText"), "OK", Translate.GetString("btnCancel.Content")).GetAwaiter();
						if (!taskAwaiter.IsCompleted)
						{
							await taskAwaiter;
							TaskAwaiter<bool> taskAwaiter2;
							taskAwaiter = taskAwaiter2;
							taskAwaiter2 = default(TaskAwaiter<bool>);
						}
						if (!taskAwaiter.GetResult())
						{
							return;
						}
					}
					OBDDataReader obdreader2 = App.OBDReader;
					await ((obdreader2 != null) ? obdreader2.DebugWrite(string.Format("\n[DTCRead:Manual({0})]\n", (int)this.sliderSettings.Value)) : null);
					commands = DTCWorker.GetClassicCommands((int)this.sliderSettings.Value, true);
				}
				App.OBDReader.CurrentCarData.DTCs.Clear();
				await App.OBDReader.ClearRequestQueue();
				foreach (OBDRequest obdrequest in commands)
				{
					if (obdrequest.Command != null && !obdrequest.Command.StartsWith("AT"))
					{
						obdrequest.ResponseReceived += DTCWorker.Request_ResponseReceivedCheckForNR78;
					}
				}
				DTCWizardResultsPage dtcwizardResultsPage2 = new DTCWizardResultsPage(DTCWizardSetupPage.DTCMode.Read, commands);
				await base.Navigation.PushAsync(dtcwizardResultsPage2);
				commands = null;
			}
			this.btnStart.IsEnabled = true;
		}

		// Token: 0x060032E0 RID: 13024 RVA: 0x0023678C File Offset: 0x0023498C
		private async void StartClear()
		{
			TaskAwaiter<bool> taskAwaiter = base.DisplayAlert(Translate.GetString("DtcPage_CleanCodes_Title"), Translate.GetString("DtcPage_CleanCodes_Text"), "OK", Translate.GetString("btnCancel.Content")).GetAwaiter();
			TaskAwaiter<bool> taskAwaiter2;
			if (!taskAwaiter.IsCompleted)
			{
				await taskAwaiter;
				taskAwaiter = taskAwaiter2;
				taskAwaiter2 = default(TaskAwaiter<bool>);
			}
			if (taskAwaiter.GetResult())
			{
				if (App.OBDSimulator.IsActive)
				{
					DTCWizardResultsPage dtcwizardResultsPage = new DTCWizardResultsPage(DTCWizardSetupPage.DTCMode.Clear, new OBDRequest[0]);
					await base.Navigation.PushAsync(dtcwizardResultsPage);
				}
				else
				{
					if ((int)this.sliderSettings.Value == 2 || (int)this.sliderSettings.Value >= 4)
					{
						taskAwaiter = base.DisplayAlert(Translate.GetString("ios_DTCWarningTitle"), Translate.GetString("ios_DTCWarningText"), "OK", Translate.GetString("btnCancel.Content")).GetAwaiter();
						if (!taskAwaiter.IsCompleted)
						{
							await taskAwaiter;
							taskAwaiter = taskAwaiter2;
							taskAwaiter2 = default(TaskAwaiter<bool>);
						}
						if (!taskAwaiter.GetResult())
						{
							return;
						}
					}
					OBDRequest[] commands = new OBDRequest[0];
					if (this.rbProfile.IsChecked.GetValueOrDefault())
					{
						if (App.OBDReader.IsNissanConsult2Protocol && App.OBDReader.CurrentELMFormat == ELMFormat.KWP)
						{
							commands = new string[] { "14", "14FF00", "14FFFF", "14FFFFFF", "140000", "1400FF00" }.Select((string cmd) => new OBDRequest(cmd, false, null)
							{
								DoNotDecode = true
							}).ToArray<OBDRequest>();
						}
						else
						{
							commands = DTCWorker.GetProfileSequence(SharedSettings.Current.DTCClearingSequence, false);
						}
						OBDDataReader obdreader = App.OBDReader;
						await ((obdreader != null) ? obdreader.DebugWrite("\n[DTCCLear:Auto]\n") : null);
					}
					else
					{
						commands = DTCWorker.GetClassicCommands((int)this.sliderSettings.Value, false);
						OBDDataReader obdreader2 = App.OBDReader;
						await ((obdreader2 != null) ? obdreader2.DebugWrite(string.Format("\n[DTCCLear:Manual({0})]\n", (int)this.sliderSettings.Value)) : null);
					}
					await App.OBDReader.ClearRequestQueue();
					DTCWizardResultsPage dtcwizardResultsPage2 = new DTCWizardResultsPage(DTCWizardSetupPage.DTCMode.Clear, commands);
					await base.Navigation.PushAsync(dtcwizardResultsPage2);
					commands = null;
				}
			}
		}

		// Token: 0x060032E1 RID: 13025 RVA: 0x000027D4 File Offset: 0x000009D4
		private void ContentPage_Appearing(object sender, EventArgs e)
		{
		}

		// Token: 0x060032E2 RID: 13026 RVA: 0x002367C4 File Offset: 0x002349C4
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private void InitializeComponent()
		{
			if (ResourceLoader.IsEnabled && ResourceLoader.CanProvideContentFor(new ResourceLoader.ResourceLoadingQuery
			{
				AssemblyName = typeof(DTCWizardSetupPage).GetTypeInfo().Assembly.GetName(),
				ResourcePath = "DTC/DTCWizardSetupPage.xaml",
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
			DynamicResourceExtension dynamicResourceExtension;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension = new DynamicResourceExtension(), new Uri("DTC\\DTCWizardSetupPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 12, 5);
			RowDefinition rowDefinition;
			VisualDiagnostics.RegisterSourceInfo(rowDefinition = new RowDefinition(), new Uri("DTC\\DTCWizardSetupPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 18, 18);
			RowDefinition rowDefinition2;
			VisualDiagnostics.RegisterSourceInfo(rowDefinition2 = new RowDefinition(), new Uri("DTC\\DTCWizardSetupPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 19, 18);
			RowDefinition rowDefinition3;
			VisualDiagnostics.RegisterSourceInfo(rowDefinition3 = new RowDefinition(), new Uri("DTC\\DTCWizardSetupPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 20, 18);
			RowDefinition rowDefinition4;
			VisualDiagnostics.RegisterSourceInfo(rowDefinition4 = new RowDefinition(), new Uri("DTC\\DTCWizardSetupPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 21, 18);
			Translate translate;
			VisualDiagnostics.RegisterSourceInfo(translate = new Translate(), new Uri("DTC\\DTCWizardSetupPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 30, 17);
			Label label;
			VisualDiagnostics.RegisterSourceInfo(label = new Label(), new Uri("DTC\\DTCWizardSetupPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 24, 14);
			DynamicResourceExtension dynamicResourceExtension2;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension2 = new DynamicResourceExtension(), new Uri("DTC\\DTCWizardSetupPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 39, 25);
			Translate translate2;
			VisualDiagnostics.RegisterSourceInfo(translate2 = new Translate(), new Uri("DTC\\DTCWizardSetupPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 43, 25);
			DynamicResourceExtension dynamicResourceExtension3;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension3 = new DynamicResourceExtension(), new Uri("DTC\\DTCWizardSetupPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 44, 25);
			DynamicResourceExtension dynamicResourceExtension4;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension4 = new DynamicResourceExtension(), new Uri("DTC\\DTCWizardSetupPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 45, 25);
			SfRadioButton sfRadioButton;
			VisualDiagnostics.RegisterSourceInfo(sfRadioButton = new SfRadioButton(), new Uri("DTC\\DTCWizardSetupPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 37, 22);
			Translate translate3;
			VisualDiagnostics.RegisterSourceInfo(translate3 = new Translate(), new Uri("DTC\\DTCWizardSetupPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 51, 25);
			SfRadioButton sfRadioButton2;
			VisualDiagnostics.RegisterSourceInfo(sfRadioButton2 = new SfRadioButton(), new Uri("DTC\\DTCWizardSetupPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 46, 22);
			SfRadioGroup sfRadioGroup;
			VisualDiagnostics.RegisterSourceInfo(sfRadioGroup = new SfRadioGroup(), new Uri("DTC\\DTCWizardSetupPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 36, 18);
			StackLayout stackLayout;
			VisualDiagnostics.RegisterSourceInfo(stackLayout = new StackLayout(), new Uri("DTC\\DTCWizardSetupPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 32, 14);
			RowDefinition rowDefinition5;
			VisualDiagnostics.RegisterSourceInfo(rowDefinition5 = new RowDefinition(), new Uri("DTC\\DTCWizardSetupPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 57, 22);
			RowDefinition rowDefinition6;
			VisualDiagnostics.RegisterSourceInfo(rowDefinition6 = new RowDefinition(), new Uri("DTC\\DTCWizardSetupPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 58, 22);
			RowDefinition rowDefinition7;
			VisualDiagnostics.RegisterSourceInfo(rowDefinition7 = new RowDefinition(), new Uri("DTC\\DTCWizardSetupPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 59, 22);
			RowDefinition rowDefinition8;
			VisualDiagnostics.RegisterSourceInfo(rowDefinition8 = new RowDefinition(), new Uri("DTC\\DTCWizardSetupPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 60, 22);
			Label label2;
			VisualDiagnostics.RegisterSourceInfo(label2 = new Label(), new Uri("DTC\\DTCWizardSetupPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 68, 26);
			Slider slider;
			VisualDiagnostics.RegisterSourceInfo(slider = new Slider(), new Uri("DTC\\DTCWizardSetupPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 69, 26);
			Label label3;
			VisualDiagnostics.RegisterSourceInfo(label3 = new Label(), new Uri("DTC\\DTCWizardSetupPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 78, 26);
			SharedSettings sharedSettings;
			VisualDiagnostics.RegisterSourceInfo(sharedSettings = SharedSettings.Current, new Uri("DTC\\DTCWizardSetupPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 82, 29);
			BindingExtension bindingExtension;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension = new BindingExtension(), new Uri("DTC\\DTCWizardSetupPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 83, 29);
			Translate translate4;
			VisualDiagnostics.RegisterSourceInfo(translate4 = new Translate(), new Uri("DTC\\DTCWizardSetupPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 84, 29);
			LabelSwitch labelSwitch;
			VisualDiagnostics.RegisterSourceInfo(labelSwitch = new LabelSwitch(), new Uri("DTC\\DTCWizardSetupPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 80, 26);
			SharedSettings sharedSettings2;
			VisualDiagnostics.RegisterSourceInfo(sharedSettings2 = SharedSettings.Current, new Uri("DTC\\DTCWizardSetupPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 87, 29);
			BindingExtension bindingExtension2;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension2 = new BindingExtension(), new Uri("DTC\\DTCWizardSetupPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 88, 29);
			Translate translate5;
			VisualDiagnostics.RegisterSourceInfo(translate5 = new Translate(), new Uri("DTC\\DTCWizardSetupPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 89, 29);
			LabelSwitch labelSwitch2;
			VisualDiagnostics.RegisterSourceInfo(labelSwitch2 = new LabelSwitch(), new Uri("DTC\\DTCWizardSetupPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 85, 26);
			StackLayout stackLayout2;
			VisualDiagnostics.RegisterSourceInfo(stackLayout2 = new StackLayout(), new Uri("DTC\\DTCWizardSetupPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 67, 22);
			ScrollView scrollView;
			VisualDiagnostics.RegisterSourceInfo(scrollView = new ScrollView(), new Uri("DTC\\DTCWizardSetupPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 63, 18);
			Grid grid;
			VisualDiagnostics.RegisterSourceInfo(grid = new Grid(), new Uri("DTC\\DTCWizardSetupPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 55, 14);
			Translate translate6;
			VisualDiagnostics.RegisterSourceInfo(translate6 = new Translate(), new Uri("DTC\\DTCWizardSetupPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 100, 28);
			Label label4;
			VisualDiagnostics.RegisterSourceInfo(label4 = new Label(), new Uri("DTC\\DTCWizardSetupPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 100, 22);
			BindingExtension bindingExtension3;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension3 = new BindingExtension(), new Uri("DTC\\DTCWizardSetupPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 106, 25);
			Label label5;
			VisualDiagnostics.RegisterSourceInfo(label5 = new Label(), new Uri("DTC\\DTCWizardSetupPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 101, 22);
			SharedSettings sharedSettings3;
			VisualDiagnostics.RegisterSourceInfo(sharedSettings3 = SharedSettings.Current, new Uri("DTC\\DTCWizardSetupPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 109, 25);
			BindingExtension bindingExtension4;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension4 = new BindingExtension(), new Uri("DTC\\DTCWizardSetupPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 110, 25);
			Translate translate7;
			VisualDiagnostics.RegisterSourceInfo(translate7 = new Translate(), new Uri("DTC\\DTCWizardSetupPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 111, 25);
			LabelSwitch labelSwitch3;
			VisualDiagnostics.RegisterSourceInfo(labelSwitch3 = new LabelSwitch(), new Uri("DTC\\DTCWizardSetupPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 107, 22);
			SharedSettings sharedSettings4;
			VisualDiagnostics.RegisterSourceInfo(sharedSettings4 = SharedSettings.Current, new Uri("DTC\\DTCWizardSetupPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 114, 25);
			BindingExtension bindingExtension5;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension5 = new BindingExtension(), new Uri("DTC\\DTCWizardSetupPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 115, 25);
			Translate translate8;
			VisualDiagnostics.RegisterSourceInfo(translate8 = new Translate(), new Uri("DTC\\DTCWizardSetupPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 116, 25);
			LabelSwitch labelSwitch4;
			VisualDiagnostics.RegisterSourceInfo(labelSwitch4 = new LabelSwitch(), new Uri("DTC\\DTCWizardSetupPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 112, 22);
			StackLayout stackLayout3;
			VisualDiagnostics.RegisterSourceInfo(stackLayout3 = new StackLayout(), new Uri("DTC\\DTCWizardSetupPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 99, 18);
			ScrollView scrollView2;
			VisualDiagnostics.RegisterSourceInfo(scrollView2 = new ScrollView(), new Uri("DTC\\DTCWizardSetupPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 95, 14);
			Button button;
			VisualDiagnostics.RegisterSourceInfo(button = new Button(), new Uri("DTC\\DTCWizardSetupPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 119, 14);
			Grid grid2;
			VisualDiagnostics.RegisterSourceInfo(grid2 = new Grid(), new Uri("DTC\\DTCWizardSetupPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 16, 10);
			VisualDiagnostics.RegisterSourceInfo(this, new Uri("DTC\\DTCWizardSetupPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 2, 2);
			NameScope nameScope = NameScope.GetNameScope(this) ?? new NameScope();
			NameScope.SetNameScope(this, nameScope);
			nameScope.RegisterName("labelSelectMode", label);
			if (label.StyleId == null)
			{
				label.StyleId = "labelSelectMode";
			}
			nameScope.RegisterName("panelMode", stackLayout);
			if (stackLayout.StyleId == null)
			{
				stackLayout.StyleId = "panelMode";
			}
			nameScope.RegisterName("rbProfile", sfRadioButton);
			if (sfRadioButton.StyleId == null)
			{
				sfRadioButton.StyleId = "rbProfile";
			}
			nameScope.RegisterName("rbAdvanced", sfRadioButton2);
			if (sfRadioButton2.StyleId == null)
			{
				sfRadioButton2.StyleId = "rbAdvanced";
			}
			nameScope.RegisterName("panelDTCAdvancedSettings", grid);
			if (grid.StyleId == null)
			{
				grid.StyleId = "panelDTCAdvancedSettings";
			}
			nameScope.RegisterName("labelTitle", label2);
			if (label2.StyleId == null)
			{
				label2.StyleId = "labelTitle";
			}
			nameScope.RegisterName("sliderSettings", slider);
			if (slider.StyleId == null)
			{
				slider.StyleId = "sliderSettings";
			}
			nameScope.RegisterName("labelVariantDescription", label3);
			if (label3.StyleId == null)
			{
				label3.StyleId = "labelVariantDescription";
			}
			nameScope.RegisterName("swHideUncomplitedTests1", labelSwitch);
			if (labelSwitch.StyleId == null)
			{
				labelSwitch.StyleId = "swHideUncomplitedTests1";
			}
			nameScope.RegisterName("swHideArchiveDTC1", labelSwitch2);
			if (labelSwitch2.StyleId == null)
			{
				labelSwitch2.StyleId = "swHideArchiveDTC1";
			}
			nameScope.RegisterName("panelProfileWarning", scrollView2);
			if (scrollView2.StyleId == null)
			{
				scrollView2.StyleId = "panelProfileWarning";
			}
			nameScope.RegisterName("labelBrandAndProfile", label5);
			if (label5.StyleId == null)
			{
				label5.StyleId = "labelBrandAndProfile";
			}
			nameScope.RegisterName("swHideUncomplitedTests2", labelSwitch3);
			if (labelSwitch3.StyleId == null)
			{
				labelSwitch3.StyleId = "swHideUncomplitedTests2";
			}
			nameScope.RegisterName("swHideArchiveDTC2", labelSwitch4);
			if (labelSwitch4.StyleId == null)
			{
				labelSwitch4.StyleId = "swHideArchiveDTC2";
			}
			nameScope.RegisterName("btnStart", button);
			if (button.StyleId == null)
			{
				button.StyleId = "btnStart";
			}
			this.labelSelectMode = label;
			this.panelMode = stackLayout;
			this.rbProfile = sfRadioButton;
			this.rbAdvanced = sfRadioButton2;
			this.panelDTCAdvancedSettings = grid;
			this.labelTitle = label2;
			this.sliderSettings = slider;
			this.labelVariantDescription = label3;
			this.swHideUncomplitedTests1 = labelSwitch;
			this.swHideArchiveDTC1 = labelSwitch2;
			this.panelProfileWarning = scrollView2;
			this.labelBrandAndProfile = label5;
			this.swHideUncomplitedTests2 = labelSwitch3;
			this.swHideArchiveDTC2 = labelSwitch4;
			this.btnStart = button;
			this.SetValue(Page.PrefersStatusBarHiddenProperty, 2);
			this.SetValue(Page.UseSafeAreaProperty, true);
			this.Appearing += this.ContentPage_Appearing;
			dynamicResourceExtension.Key = "BackgroundColor";
			IMarkupExtension<DynamicResource> markupExtension = dynamicResourceExtension;
			XamlServiceProvider xamlServiceProvider = new XamlServiceProvider();
			Type typeFromHandle = typeof(IProvideValueTarget);
			object[] array = new object[0 + 1];
			array[0] = this;
			object obj;
			xamlServiceProvider.Add(typeFromHandle, obj = new SimpleValueTargetProvider(array, VisualElement.BackgroundColorProperty, nameScope));
			xamlServiceProvider.Add(typeof(IReferenceProvider), obj);
			Type typeFromHandle2 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver = new XmlNamespaceResolver();
			xmlNamespaceResolver.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver.Add("syncfusion", "clr-namespace:Syncfusion.XForms.Buttons;assembly=Syncfusion.Buttons.XForms");
			xamlServiceProvider.Add(typeFromHandle2, new XamlTypeResolver(xmlNamespaceResolver, typeof(DTCWizardSetupPage).GetTypeInfo().Assembly));
			xamlServiceProvider.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(12, 5)));
			DynamicResource dynamicResource = markupExtension.ProvideValue(xamlServiceProvider);
			this.SetDynamicResource(VisualElement.BackgroundColorProperty, dynamicResource.Key);
			this.SetValue(NavigationPage.HasBackButtonProperty, true);
			this.SetValue(NavigationPage.HasNavigationBarProperty, true);
			grid2.SetValue(View.MarginProperty, new Thickness(5.0, 0.0, 5.0, 0.0));
			rowDefinition.SetValue(RowDefinition.HeightProperty, new GridLengthTypeConverter().ConvertFromInvariantString("auto"));
			grid2.GetValue(Grid.RowDefinitionsProperty).Add(rowDefinition);
			rowDefinition2.SetValue(RowDefinition.HeightProperty, new GridLengthTypeConverter().ConvertFromInvariantString("auto"));
			grid2.GetValue(Grid.RowDefinitionsProperty).Add(rowDefinition2);
			rowDefinition3.SetValue(RowDefinition.HeightProperty, new GridLengthTypeConverter().ConvertFromInvariantString("*"));
			grid2.GetValue(Grid.RowDefinitionsProperty).Add(rowDefinition3);
			rowDefinition4.SetValue(RowDefinition.HeightProperty, new GridLengthTypeConverter().ConvertFromInvariantString("auto"));
			grid2.GetValue(Grid.RowDefinitionsProperty).Add(rowDefinition4);
			label.SetValue(Grid.RowProperty, 0);
			label.SetValue(Label.FontAttributesProperty, new FontAttributesConverter().ConvertFromInvariantString("Bold"));
			label.SetValue(View.HorizontalOptionsProperty, LayoutOptions.Center);
			label.SetValue(Label.HorizontalTextAlignmentProperty, new TextAlignmentConverter().ConvertFromInvariantString("Center"));
			translate.Text = "ios_DTCSettingsTitle";
			IMarkupExtension markupExtension2 = translate;
			XamlServiceProvider xamlServiceProvider2 = new XamlServiceProvider();
			Type typeFromHandle3 = typeof(IProvideValueTarget);
			object[] array2 = new object[0 + 3];
			array2[0] = label;
			array2[1] = grid2;
			array2[2] = this;
			object obj2;
			xamlServiceProvider2.Add(typeFromHandle3, obj2 = new SimpleValueTargetProvider(array2, Label.TextProperty, nameScope));
			xamlServiceProvider2.Add(typeof(IReferenceProvider), obj2);
			Type typeFromHandle4 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver2 = new XmlNamespaceResolver();
			xmlNamespaceResolver2.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver2.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver2.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver2.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver2.Add("syncfusion", "clr-namespace:Syncfusion.XForms.Buttons;assembly=Syncfusion.Buttons.XForms");
			xamlServiceProvider2.Add(typeFromHandle4, new XamlTypeResolver(xmlNamespaceResolver2, typeof(DTCWizardSetupPage).GetTypeInfo().Assembly));
			xamlServiceProvider2.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(30, 17)));
			object obj3 = markupExtension2.ProvideValue(xamlServiceProvider2);
			label.Text = obj3;
			grid2.Children.Add(label);
			stackLayout.SetValue(Grid.RowProperty, 1);
			stackLayout.SetValue(StackLayout.OrientationProperty, 0);
			sfRadioGroup.SetValue(StackLayout.OrientationProperty, 0);
			dynamicResourceExtension2.Key = "TextColor";
			IMarkupExtension<DynamicResource> markupExtension3 = dynamicResourceExtension2;
			XamlServiceProvider xamlServiceProvider3 = new XamlServiceProvider();
			Type typeFromHandle5 = typeof(IProvideValueTarget);
			object[] array3 = new object[0 + 5];
			array3[0] = sfRadioButton;
			array3[1] = sfRadioGroup;
			array3[2] = stackLayout;
			array3[3] = grid2;
			array3[4] = this;
			object obj4;
			xamlServiceProvider3.Add(typeFromHandle5, obj4 = new SimpleValueTargetProvider(array3, ToggleButton.CheckedColorProperty, nameScope));
			xamlServiceProvider3.Add(typeof(IReferenceProvider), obj4);
			Type typeFromHandle6 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver3 = new XmlNamespaceResolver();
			xmlNamespaceResolver3.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver3.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver3.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver3.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver3.Add("syncfusion", "clr-namespace:Syncfusion.XForms.Buttons;assembly=Syncfusion.Buttons.XForms");
			xamlServiceProvider3.Add(typeFromHandle6, new XamlTypeResolver(xmlNamespaceResolver3, typeof(DTCWizardSetupPage).GetTypeInfo().Assembly));
			xamlServiceProvider3.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(39, 25)));
			DynamicResource dynamicResource2 = markupExtension3.ProvideValue(xamlServiceProvider3);
			sfRadioButton.SetDynamicResource(ToggleButton.CheckedColorProperty, dynamicResource2.Key);
			sfRadioButton.SetValue(ToggleButton.IsCheckedProperty, new bool?(true));
			sfRadioButton.SetValue(ToggleButton.IsThreeStateProperty, false);
			sfRadioButton.StateChanged += this.rb_StateChanged;
			translate2.Text = "ios_Automatic";
			IMarkupExtension markupExtension4 = translate2;
			XamlServiceProvider xamlServiceProvider4 = new XamlServiceProvider();
			Type typeFromHandle7 = typeof(IProvideValueTarget);
			object[] array4 = new object[0 + 5];
			array4[0] = sfRadioButton;
			array4[1] = sfRadioGroup;
			array4[2] = stackLayout;
			array4[3] = grid2;
			array4[4] = this;
			object obj5;
			xamlServiceProvider4.Add(typeFromHandle7, obj5 = new SimpleValueTargetProvider(array4, ToggleButton.TextProperty, nameScope));
			xamlServiceProvider4.Add(typeof(IReferenceProvider), obj5);
			Type typeFromHandle8 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver4 = new XmlNamespaceResolver();
			xmlNamespaceResolver4.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver4.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver4.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver4.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver4.Add("syncfusion", "clr-namespace:Syncfusion.XForms.Buttons;assembly=Syncfusion.Buttons.XForms");
			xamlServiceProvider4.Add(typeFromHandle8, new XamlTypeResolver(xmlNamespaceResolver4, typeof(DTCWizardSetupPage).GetTypeInfo().Assembly));
			xamlServiceProvider4.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(43, 25)));
			object obj6 = markupExtension4.ProvideValue(xamlServiceProvider4);
			sfRadioButton.Text = obj6;
			dynamicResourceExtension3.Key = "TextColor";
			IMarkupExtension<DynamicResource> markupExtension5 = dynamicResourceExtension3;
			XamlServiceProvider xamlServiceProvider5 = new XamlServiceProvider();
			Type typeFromHandle9 = typeof(IProvideValueTarget);
			object[] array5 = new object[0 + 5];
			array5[0] = sfRadioButton;
			array5[1] = sfRadioGroup;
			array5[2] = stackLayout;
			array5[3] = grid2;
			array5[4] = this;
			object obj7;
			xamlServiceProvider5.Add(typeFromHandle9, obj7 = new SimpleValueTargetProvider(array5, ToggleButton.TextColorProperty, nameScope));
			xamlServiceProvider5.Add(typeof(IReferenceProvider), obj7);
			Type typeFromHandle10 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver5 = new XmlNamespaceResolver();
			xmlNamespaceResolver5.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver5.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver5.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver5.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver5.Add("syncfusion", "clr-namespace:Syncfusion.XForms.Buttons;assembly=Syncfusion.Buttons.XForms");
			xamlServiceProvider5.Add(typeFromHandle10, new XamlTypeResolver(xmlNamespaceResolver5, typeof(DTCWizardSetupPage).GetTypeInfo().Assembly));
			xamlServiceProvider5.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(44, 25)));
			DynamicResource dynamicResource3 = markupExtension5.ProvideValue(xamlServiceProvider5);
			sfRadioButton.SetDynamicResource(ToggleButton.TextColorProperty, dynamicResource3.Key);
			dynamicResourceExtension4.Key = "TextColor";
			IMarkupExtension<DynamicResource> markupExtension6 = dynamicResourceExtension4;
			XamlServiceProvider xamlServiceProvider6 = new XamlServiceProvider();
			Type typeFromHandle11 = typeof(IProvideValueTarget);
			object[] array6 = new object[0 + 5];
			array6[0] = sfRadioButton;
			array6[1] = sfRadioGroup;
			array6[2] = stackLayout;
			array6[3] = grid2;
			array6[4] = this;
			object obj8;
			xamlServiceProvider6.Add(typeFromHandle11, obj8 = new SimpleValueTargetProvider(array6, ToggleButton.UncheckedColorProperty, nameScope));
			xamlServiceProvider6.Add(typeof(IReferenceProvider), obj8);
			Type typeFromHandle12 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver6 = new XmlNamespaceResolver();
			xmlNamespaceResolver6.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver6.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver6.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver6.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver6.Add("syncfusion", "clr-namespace:Syncfusion.XForms.Buttons;assembly=Syncfusion.Buttons.XForms");
			xamlServiceProvider6.Add(typeFromHandle12, new XamlTypeResolver(xmlNamespaceResolver6, typeof(DTCWizardSetupPage).GetTypeInfo().Assembly));
			xamlServiceProvider6.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(45, 25)));
			DynamicResource dynamicResource4 = markupExtension6.ProvideValue(xamlServiceProvider6);
			sfRadioButton.SetDynamicResource(ToggleButton.UncheckedColorProperty, dynamicResource4.Key);
			sfRadioGroup.Children.Add(sfRadioButton);
			sfRadioButton2.SetValue(ToggleButton.IsCheckedProperty, new bool?(false));
			sfRadioButton2.SetValue(ToggleButton.IsThreeStateProperty, false);
			sfRadioButton2.StateChanged += this.rb_StateChanged;
			translate3.Text = "ios_Advanced";
			IMarkupExtension markupExtension7 = translate3;
			XamlServiceProvider xamlServiceProvider7 = new XamlServiceProvider();
			Type typeFromHandle13 = typeof(IProvideValueTarget);
			object[] array7 = new object[0 + 5];
			array7[0] = sfRadioButton2;
			array7[1] = sfRadioGroup;
			array7[2] = stackLayout;
			array7[3] = grid2;
			array7[4] = this;
			object obj9;
			xamlServiceProvider7.Add(typeFromHandle13, obj9 = new SimpleValueTargetProvider(array7, ToggleButton.TextProperty, nameScope));
			xamlServiceProvider7.Add(typeof(IReferenceProvider), obj9);
			Type typeFromHandle14 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver7 = new XmlNamespaceResolver();
			xmlNamespaceResolver7.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver7.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver7.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver7.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver7.Add("syncfusion", "clr-namespace:Syncfusion.XForms.Buttons;assembly=Syncfusion.Buttons.XForms");
			xamlServiceProvider7.Add(typeFromHandle14, new XamlTypeResolver(xmlNamespaceResolver7, typeof(DTCWizardSetupPage).GetTypeInfo().Assembly));
			xamlServiceProvider7.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(51, 25)));
			object obj10 = markupExtension7.ProvideValue(xamlServiceProvider7);
			sfRadioButton2.Text = obj10;
			sfRadioGroup.Children.Add(sfRadioButton2);
			stackLayout.Children.Add(sfRadioGroup);
			grid2.Children.Add(stackLayout);
			grid.SetValue(Grid.RowProperty, 2);
			rowDefinition5.SetValue(RowDefinition.HeightProperty, new GridLengthTypeConverter().ConvertFromInvariantString("auto"));
			grid.GetValue(Grid.RowDefinitionsProperty).Add(rowDefinition5);
			rowDefinition6.SetValue(RowDefinition.HeightProperty, new GridLengthTypeConverter().ConvertFromInvariantString("*"));
			grid.GetValue(Grid.RowDefinitionsProperty).Add(rowDefinition6);
			rowDefinition7.SetValue(RowDefinition.HeightProperty, new GridLengthTypeConverter().ConvertFromInvariantString("auto"));
			grid.GetValue(Grid.RowDefinitionsProperty).Add(rowDefinition7);
			rowDefinition8.SetValue(RowDefinition.HeightProperty, new GridLengthTypeConverter().ConvertFromInvariantString("auto"));
			grid.GetValue(Grid.RowDefinitionsProperty).Add(rowDefinition8);
			scrollView.SetValue(Grid.RowProperty, 1);
			scrollView.SetValue(VisualElement.IsVisibleProperty, new VisualElement.VisibilityConverter().ConvertFromInvariantString("True"));
			scrollView.SetValue(ScrollView.OrientationProperty, 0);
			stackLayout2.SetValue(StackLayout.OrientationProperty, 0);
			label2.SetValue(Label.TextProperty, "Select read/clean variant");
			stackLayout2.Children.Add(label2);
			slider.SetValue(VisualElement.BackgroundColorProperty, Color.LightGray);
			slider.SetValue(Slider.MaximumProperty, 4.0);
			slider.SetValue(Slider.MinimumProperty, 0.0);
			slider.SetValue(Slider.ThumbColorProperty, Color.Accent);
			slider.ValueChanged += this.sliderSettings_ValueChanged;
			slider.SetValue(Slider.ValueProperty, 1.0);
			stackLayout2.Children.Add(slider);
			stackLayout2.Children.Add(label3);
			labelSwitch.SetValue(BindableObject.BindingContextProperty, sharedSettings);
			bindingExtension.Mode = 1;
			bindingExtension.Path = "HideDTCWithUncomplitedTests";
			BindingBase bindingBase = bindingExtension.ProvideValue(null);
			labelSwitch.SetBinding(LabelSwitch.IsToggledProperty, bindingBase);
			translate4.Text = "ios_HideDTCWithUncomplitedTests";
			IMarkupExtension markupExtension8 = translate4;
			XamlServiceProvider xamlServiceProvider8 = new XamlServiceProvider();
			Type typeFromHandle15 = typeof(IProvideValueTarget);
			object[] array8 = new object[0 + 6];
			array8[0] = labelSwitch;
			array8[1] = stackLayout2;
			array8[2] = scrollView;
			array8[3] = grid;
			array8[4] = grid2;
			array8[5] = this;
			object obj11;
			xamlServiceProvider8.Add(typeFromHandle15, obj11 = new SimpleValueTargetProvider(array8, LabelSwitch.TextProperty, nameScope));
			xamlServiceProvider8.Add(typeof(IReferenceProvider), obj11);
			Type typeFromHandle16 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver8 = new XmlNamespaceResolver();
			xmlNamespaceResolver8.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver8.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver8.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver8.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver8.Add("syncfusion", "clr-namespace:Syncfusion.XForms.Buttons;assembly=Syncfusion.Buttons.XForms");
			xamlServiceProvider8.Add(typeFromHandle16, new XamlTypeResolver(xmlNamespaceResolver8, typeof(DTCWizardSetupPage).GetTypeInfo().Assembly));
			xamlServiceProvider8.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(84, 29)));
			object obj12 = markupExtension8.ProvideValue(xamlServiceProvider8);
			labelSwitch.Text = obj12;
			stackLayout2.Children.Add(labelSwitch);
			labelSwitch2.SetValue(BindableObject.BindingContextProperty, sharedSettings2);
			bindingExtension2.Mode = 1;
			bindingExtension2.Path = "HideArchiveDTC";
			BindingBase bindingBase2 = bindingExtension2.ProvideValue(null);
			labelSwitch2.SetBinding(LabelSwitch.IsToggledProperty, bindingBase2);
			translate5.Text = "ios_SkipArchiveDTC";
			IMarkupExtension markupExtension9 = translate5;
			XamlServiceProvider xamlServiceProvider9 = new XamlServiceProvider();
			Type typeFromHandle17 = typeof(IProvideValueTarget);
			object[] array9 = new object[0 + 6];
			array9[0] = labelSwitch2;
			array9[1] = stackLayout2;
			array9[2] = scrollView;
			array9[3] = grid;
			array9[4] = grid2;
			array9[5] = this;
			object obj13;
			xamlServiceProvider9.Add(typeFromHandle17, obj13 = new SimpleValueTargetProvider(array9, LabelSwitch.TextProperty, nameScope));
			xamlServiceProvider9.Add(typeof(IReferenceProvider), obj13);
			Type typeFromHandle18 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver9 = new XmlNamespaceResolver();
			xmlNamespaceResolver9.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver9.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver9.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver9.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver9.Add("syncfusion", "clr-namespace:Syncfusion.XForms.Buttons;assembly=Syncfusion.Buttons.XForms");
			xamlServiceProvider9.Add(typeFromHandle18, new XamlTypeResolver(xmlNamespaceResolver9, typeof(DTCWizardSetupPage).GetTypeInfo().Assembly));
			xamlServiceProvider9.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(89, 29)));
			object obj14 = markupExtension9.ProvideValue(xamlServiceProvider9);
			labelSwitch2.Text = obj14;
			stackLayout2.Children.Add(labelSwitch2);
			scrollView.Content = stackLayout2;
			grid.Children.Add(scrollView);
			grid2.Children.Add(grid);
			scrollView2.SetValue(Grid.RowProperty, 2);
			scrollView2.SetValue(VisualElement.IsVisibleProperty, new VisualElement.VisibilityConverter().ConvertFromInvariantString("False"));
			stackLayout3.SetValue(StackLayout.OrientationProperty, 0);
			translate6.Text = "ios_ConfirmSelectedProfile";
			IMarkupExtension markupExtension10 = translate6;
			XamlServiceProvider xamlServiceProvider10 = new XamlServiceProvider();
			Type typeFromHandle19 = typeof(IProvideValueTarget);
			object[] array10 = new object[0 + 5];
			array10[0] = label4;
			array10[1] = stackLayout3;
			array10[2] = scrollView2;
			array10[3] = grid2;
			array10[4] = this;
			object obj15;
			xamlServiceProvider10.Add(typeFromHandle19, obj15 = new SimpleValueTargetProvider(array10, Label.TextProperty, nameScope));
			xamlServiceProvider10.Add(typeof(IReferenceProvider), obj15);
			Type typeFromHandle20 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver10 = new XmlNamespaceResolver();
			xmlNamespaceResolver10.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver10.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver10.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver10.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver10.Add("syncfusion", "clr-namespace:Syncfusion.XForms.Buttons;assembly=Syncfusion.Buttons.XForms");
			xamlServiceProvider10.Add(typeFromHandle20, new XamlTypeResolver(xmlNamespaceResolver10, typeof(DTCWizardSetupPage).GetTypeInfo().Assembly));
			xamlServiceProvider10.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(100, 28)));
			object obj16 = markupExtension10.ProvideValue(xamlServiceProvider10);
			label4.Text = obj16;
			stackLayout3.Children.Add(label4);
			label5.SetValue(Label.FontAttributesProperty, new FontAttributesConverter().ConvertFromInvariantString("Bold"));
			label5.SetValue(View.HorizontalOptionsProperty, LayoutOptions.Center);
			label5.SetValue(Label.HorizontalTextAlignmentProperty, new TextAlignmentConverter().ConvertFromInvariantString("Center"));
			bindingExtension3.Mode = 2;
			bindingExtension3.Path = "BrandAndProfile";
			BindingBase bindingBase3 = bindingExtension3.ProvideValue(null);
			label5.SetBinding(Label.TextProperty, bindingBase3);
			stackLayout3.Children.Add(label5);
			labelSwitch3.SetValue(BindableObject.BindingContextProperty, sharedSettings3);
			bindingExtension4.Mode = 1;
			bindingExtension4.Path = "HideDTCWithUncomplitedTests";
			BindingBase bindingBase4 = bindingExtension4.ProvideValue(null);
			labelSwitch3.SetBinding(LabelSwitch.IsToggledProperty, bindingBase4);
			translate7.Text = "ios_HideDTCWithUncomplitedTests";
			IMarkupExtension markupExtension11 = translate7;
			XamlServiceProvider xamlServiceProvider11 = new XamlServiceProvider();
			Type typeFromHandle21 = typeof(IProvideValueTarget);
			object[] array11 = new object[0 + 5];
			array11[0] = labelSwitch3;
			array11[1] = stackLayout3;
			array11[2] = scrollView2;
			array11[3] = grid2;
			array11[4] = this;
			object obj17;
			xamlServiceProvider11.Add(typeFromHandle21, obj17 = new SimpleValueTargetProvider(array11, LabelSwitch.TextProperty, nameScope));
			xamlServiceProvider11.Add(typeof(IReferenceProvider), obj17);
			Type typeFromHandle22 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver11 = new XmlNamespaceResolver();
			xmlNamespaceResolver11.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver11.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver11.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver11.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver11.Add("syncfusion", "clr-namespace:Syncfusion.XForms.Buttons;assembly=Syncfusion.Buttons.XForms");
			xamlServiceProvider11.Add(typeFromHandle22, new XamlTypeResolver(xmlNamespaceResolver11, typeof(DTCWizardSetupPage).GetTypeInfo().Assembly));
			xamlServiceProvider11.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(111, 25)));
			object obj18 = markupExtension11.ProvideValue(xamlServiceProvider11);
			labelSwitch3.Text = obj18;
			stackLayout3.Children.Add(labelSwitch3);
			labelSwitch4.SetValue(BindableObject.BindingContextProperty, sharedSettings4);
			bindingExtension5.Mode = 1;
			bindingExtension5.Path = "HideArchiveDTC";
			BindingBase bindingBase5 = bindingExtension5.ProvideValue(null);
			labelSwitch4.SetBinding(LabelSwitch.IsToggledProperty, bindingBase5);
			translate8.Text = "ios_SkipArchiveDTC";
			IMarkupExtension markupExtension12 = translate8;
			XamlServiceProvider xamlServiceProvider12 = new XamlServiceProvider();
			Type typeFromHandle23 = typeof(IProvideValueTarget);
			object[] array12 = new object[0 + 5];
			array12[0] = labelSwitch4;
			array12[1] = stackLayout3;
			array12[2] = scrollView2;
			array12[3] = grid2;
			array12[4] = this;
			object obj19;
			xamlServiceProvider12.Add(typeFromHandle23, obj19 = new SimpleValueTargetProvider(array12, LabelSwitch.TextProperty, nameScope));
			xamlServiceProvider12.Add(typeof(IReferenceProvider), obj19);
			Type typeFromHandle24 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver12 = new XmlNamespaceResolver();
			xmlNamespaceResolver12.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver12.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver12.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver12.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver12.Add("syncfusion", "clr-namespace:Syncfusion.XForms.Buttons;assembly=Syncfusion.Buttons.XForms");
			xamlServiceProvider12.Add(typeFromHandle24, new XamlTypeResolver(xmlNamespaceResolver12, typeof(DTCWizardSetupPage).GetTypeInfo().Assembly));
			xamlServiceProvider12.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(116, 25)));
			object obj20 = markupExtension12.ProvideValue(xamlServiceProvider12);
			labelSwitch4.Text = obj20;
			stackLayout3.Children.Add(labelSwitch4);
			scrollView2.Content = stackLayout3;
			grid2.Children.Add(scrollView2);
			button.SetValue(Grid.RowProperty, 3);
			button.Clicked += this.BtnStart_Clicked;
			button.SetValue(Button.TextProperty, "START");
			grid2.Children.Add(button);
			this.SetValue(ContentPage.ContentProperty, grid2);
		}

		// Token: 0x060032E3 RID: 13027 RVA: 0x00238754 File Offset: 0x00236954
		private void __InitComponentRuntime()
		{
			Extensions.LoadFromXaml<DTCWizardSetupPage>(this, typeof(DTCWizardSetupPage));
			this.labelSelectMode = NameScopeExtensions.FindByName<Label>(this, "labelSelectMode");
			this.panelMode = NameScopeExtensions.FindByName<StackLayout>(this, "panelMode");
			this.rbProfile = NameScopeExtensions.FindByName<SfRadioButton>(this, "rbProfile");
			this.rbAdvanced = NameScopeExtensions.FindByName<SfRadioButton>(this, "rbAdvanced");
			this.panelDTCAdvancedSettings = NameScopeExtensions.FindByName<Grid>(this, "panelDTCAdvancedSettings");
			this.labelTitle = NameScopeExtensions.FindByName<Label>(this, "labelTitle");
			this.sliderSettings = NameScopeExtensions.FindByName<Slider>(this, "sliderSettings");
			this.labelVariantDescription = NameScopeExtensions.FindByName<Label>(this, "labelVariantDescription");
			this.swHideUncomplitedTests1 = NameScopeExtensions.FindByName<LabelSwitch>(this, "swHideUncomplitedTests1");
			this.swHideArchiveDTC1 = NameScopeExtensions.FindByName<LabelSwitch>(this, "swHideArchiveDTC1");
			this.panelProfileWarning = NameScopeExtensions.FindByName<ScrollView>(this, "panelProfileWarning");
			this.labelBrandAndProfile = NameScopeExtensions.FindByName<Label>(this, "labelBrandAndProfile");
			this.swHideUncomplitedTests2 = NameScopeExtensions.FindByName<LabelSwitch>(this, "swHideUncomplitedTests2");
			this.swHideArchiveDTC2 = NameScopeExtensions.FindByName<LabelSwitch>(this, "swHideArchiveDTC2");
			this.btnStart = NameScopeExtensions.FindByName<Button>(this, "btnStart");
		}

		// Token: 0x04001DC0 RID: 7616
		private DTCWizardSetupPage.DTCMode Mode;

		// Token: 0x04001DC1 RID: 7617
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private Label labelSelectMode;

		// Token: 0x04001DC2 RID: 7618
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private StackLayout panelMode;

		// Token: 0x04001DC3 RID: 7619
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private SfRadioButton rbProfile;

		// Token: 0x04001DC4 RID: 7620
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private SfRadioButton rbAdvanced;

		// Token: 0x04001DC5 RID: 7621
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private Grid panelDTCAdvancedSettings;

		// Token: 0x04001DC6 RID: 7622
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private Label labelTitle;

		// Token: 0x04001DC7 RID: 7623
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private Slider sliderSettings;

		// Token: 0x04001DC8 RID: 7624
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private Label labelVariantDescription;

		// Token: 0x04001DC9 RID: 7625
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private LabelSwitch swHideUncomplitedTests1;

		// Token: 0x04001DCA RID: 7626
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private LabelSwitch swHideArchiveDTC1;

		// Token: 0x04001DCB RID: 7627
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private ScrollView panelProfileWarning;

		// Token: 0x04001DCC RID: 7628
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private Label labelBrandAndProfile;

		// Token: 0x04001DCD RID: 7629
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private LabelSwitch swHideUncomplitedTests2;

		// Token: 0x04001DCE RID: 7630
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private LabelSwitch swHideArchiveDTC2;

		// Token: 0x04001DCF RID: 7631
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private Button btnStart;

		// Token: 0x0200055B RID: 1371
		public enum DTCMode
		{
			// Token: 0x04001DD1 RID: 7633
			Read,
			// Token: 0x04001DD2 RID: 7634
			Clear
		}

		// Token: 0x0200055C RID: 1372
		[CompilerGenerated]
		[Serializable]
		private sealed class <>c
		{
			// Token: 0x060032E4 RID: 13028 RVA: 0x00238871 File Offset: 0x00236A71
			// Note: this type is marked as 'beforefieldinit'.
			static <>c()
			{
			}

			// Token: 0x060032E5 RID: 13029 RVA: 0x00002050 File Offset: 0x00000250
			public <>c()
			{
			}

			// Token: 0x060032E6 RID: 13030 RVA: 0x0023887D File Offset: 0x00236A7D
			internal OBDRequest <StartClear>b__7_0(string cmd)
			{
				return new OBDRequest(cmd, false, null)
				{
					DoNotDecode = true
				};
			}

			// Token: 0x04001DD3 RID: 7635
			public static readonly DTCWizardSetupPage.<>c <>9 = new DTCWizardSetupPage.<>c();

			// Token: 0x04001DD4 RID: 7636
			public static Func<string, OBDRequest> <>9__7_0;
		}

		// Token: 0x0200055D RID: 1373
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <StartClear>d__7 : IAsyncStateMachine
		{
			// Token: 0x060032E7 RID: 13031 RVA: 0x00238890 File Offset: 0x00236A90
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				DTCWizardSetupPage dtcwizardSetupPage = this;
				try
				{
					TaskAwaiter<bool> taskAwaiter3;
					TaskAwaiter taskAwaiter4;
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
						goto IL_012B;
					}
					case 2:
						taskAwaiter3 = taskAwaiter2;
						taskAwaiter2 = default(TaskAwaiter<bool>);
						num2 = -1;
						goto IL_01CF;
					case 3:
					{
						TaskAwaiter taskAwaiter5;
						taskAwaiter4 = taskAwaiter5;
						taskAwaiter5 = default(TaskAwaiter);
						num2 = -1;
						goto IL_02FF;
					}
					case 4:
					{
						TaskAwaiter taskAwaiter5;
						taskAwaiter4 = taskAwaiter5;
						taskAwaiter5 = default(TaskAwaiter);
						num2 = -1;
						goto IL_03A0;
					}
					case 5:
					{
						TaskAwaiter taskAwaiter5;
						taskAwaiter4 = taskAwaiter5;
						taskAwaiter5 = default(TaskAwaiter);
						num2 = -1;
						goto IL_0402;
					}
					case 6:
					{
						TaskAwaiter taskAwaiter5;
						taskAwaiter4 = taskAwaiter5;
						taskAwaiter5 = default(TaskAwaiter);
						num2 = -1;
						goto IL_0472;
					}
					default:
						taskAwaiter3 = dtcwizardSetupPage.DisplayAlert(Translate.GetString("DtcPage_CleanCodes_Title"), Translate.GetString("DtcPage_CleanCodes_Text"), "OK", Translate.GetString("btnCancel.Content")).GetAwaiter();
						if (!taskAwaiter3.IsCompleted)
						{
							num2 = 0;
							taskAwaiter2 = taskAwaiter3;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<bool>, DTCWizardSetupPage.<StartClear>d__7>(ref taskAwaiter3, ref this);
							return;
						}
						break;
					}
					if (!taskAwaiter3.GetResult())
					{
						goto IL_049B;
					}
					if (App.OBDSimulator.IsActive)
					{
						DTCWizardResultsPage dtcwizardResultsPage = new DTCWizardResultsPage(DTCWizardSetupPage.DTCMode.Clear, new OBDRequest[0]);
						taskAwaiter4 = dtcwizardSetupPage.Navigation.PushAsync(dtcwizardResultsPage).GetAwaiter();
						if (!taskAwaiter4.IsCompleted)
						{
							num2 = 1;
							TaskAwaiter taskAwaiter5 = taskAwaiter4;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, DTCWizardSetupPage.<StartClear>d__7>(ref taskAwaiter4, ref this);
							return;
						}
					}
					else
					{
						if ((int)dtcwizardSetupPage.sliderSettings.Value != 2 && (int)dtcwizardSetupPage.sliderSettings.Value < 4)
						{
							goto IL_01DD;
						}
						taskAwaiter3 = dtcwizardSetupPage.DisplayAlert(Translate.GetString("ios_DTCWarningTitle"), Translate.GetString("ios_DTCWarningText"), "OK", Translate.GetString("btnCancel.Content")).GetAwaiter();
						if (!taskAwaiter3.IsCompleted)
						{
							num2 = 2;
							taskAwaiter2 = taskAwaiter3;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<bool>, DTCWizardSetupPage.<StartClear>d__7>(ref taskAwaiter3, ref this);
							return;
						}
						goto IL_01CF;
					}
					IL_012B:
					taskAwaiter4.GetResult();
					goto IL_0480;
					IL_01CF:
					if (!taskAwaiter3.GetResult())
					{
						goto IL_049B;
					}
					IL_01DD:
					commands = new OBDRequest[0];
					if (dtcwizardSetupPage.rbProfile.IsChecked.GetValueOrDefault())
					{
						if (App.OBDReader.IsNissanConsult2Protocol && App.OBDReader.CurrentELMFormat == ELMFormat.KWP)
						{
							commands = new string[] { "14", "14FF00", "14FFFF", "14FFFFFF", "140000", "1400FF00" }.Select((string cmd) => new OBDRequest(cmd, false, null)
							{
								DoNotDecode = true
							}).ToArray<OBDRequest>();
						}
						else
						{
							commands = DTCWorker.GetProfileSequence(SharedSettings.Current.DTCClearingSequence, false);
						}
						OBDDataReader obdreader = App.OBDReader;
						taskAwaiter4 = ((obdreader != null) ? obdreader.DebugWrite("\n[DTCCLear:Auto]\n") : null).GetAwaiter();
						if (!taskAwaiter4.IsCompleted)
						{
							num2 = 3;
							TaskAwaiter taskAwaiter5 = taskAwaiter4;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, DTCWizardSetupPage.<StartClear>d__7>(ref taskAwaiter4, ref this);
							return;
						}
					}
					else
					{
						commands = DTCWorker.GetClassicCommands((int)dtcwizardSetupPage.sliderSettings.Value, false);
						OBDDataReader obdreader2 = App.OBDReader;
						taskAwaiter4 = ((obdreader2 != null) ? obdreader2.DebugWrite(string.Format("\n[DTCCLear:Manual({0})]\n", (int)dtcwizardSetupPage.sliderSettings.Value)) : null).GetAwaiter();
						if (!taskAwaiter4.IsCompleted)
						{
							num2 = 4;
							TaskAwaiter taskAwaiter5 = taskAwaiter4;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, DTCWizardSetupPage.<StartClear>d__7>(ref taskAwaiter4, ref this);
							return;
						}
						goto IL_03A0;
					}
					IL_02FF:
					taskAwaiter4.GetResult();
					goto IL_03A7;
					IL_03A0:
					taskAwaiter4.GetResult();
					IL_03A7:
					taskAwaiter4 = App.OBDReader.ClearRequestQueue().GetAwaiter();
					if (!taskAwaiter4.IsCompleted)
					{
						num2 = 5;
						TaskAwaiter taskAwaiter5 = taskAwaiter4;
						this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, DTCWizardSetupPage.<StartClear>d__7>(ref taskAwaiter4, ref this);
						return;
					}
					IL_0402:
					taskAwaiter4.GetResult();
					DTCWizardResultsPage dtcwizardResultsPage2 = new DTCWizardResultsPage(DTCWizardSetupPage.DTCMode.Clear, commands);
					taskAwaiter4 = dtcwizardSetupPage.Navigation.PushAsync(dtcwizardResultsPage2).GetAwaiter();
					if (!taskAwaiter4.IsCompleted)
					{
						num2 = 6;
						TaskAwaiter taskAwaiter5 = taskAwaiter4;
						this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, DTCWizardSetupPage.<StartClear>d__7>(ref taskAwaiter4, ref this);
						return;
					}
					IL_0472:
					taskAwaiter4.GetResult();
					commands = null;
					IL_0480:;
				}
				catch (Exception ex)
				{
					num2 = -2;
					this.<>t__builder.SetException(ex);
					return;
				}
				IL_049B:
				num2 = -2;
				this.<>t__builder.SetResult();
			}

			// Token: 0x060032E8 RID: 13032 RVA: 0x00238D68 File Offset: 0x00236F68
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x04001DD5 RID: 7637
			public int <>1__state;

			// Token: 0x04001DD6 RID: 7638
			public AsyncVoidMethodBuilder <>t__builder;

			// Token: 0x04001DD7 RID: 7639
			public DTCWizardSetupPage <>4__this;

			// Token: 0x04001DD8 RID: 7640
			private TaskAwaiter<bool> <>u__1;

			// Token: 0x04001DD9 RID: 7641
			private TaskAwaiter <>u__2;

			// Token: 0x04001DDA RID: 7642
			private OBDRequest[] <commands>5__2;
		}

		// Token: 0x0200055E RID: 1374
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <StartRead>d__6 : IAsyncStateMachine
		{
			// Token: 0x060032E9 RID: 13033 RVA: 0x00238D78 File Offset: 0x00236F78
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				DTCWizardSetupPage dtcwizardSetupPage = this;
				try
				{
					TaskAwaiter taskAwaiter3;
					TaskAwaiter<bool> taskAwaiter5;
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
						goto IL_02B1;
					}
					case 2:
						taskAwaiter5 = taskAwaiter2;
						taskAwaiter2 = default(TaskAwaiter<bool>);
						num2 = -1;
						goto IL_03A7;
					case 3:
					{
						TaskAwaiter taskAwaiter4;
						taskAwaiter3 = taskAwaiter4;
						taskAwaiter4 = default(TaskAwaiter);
						num2 = -1;
						goto IL_0432;
					}
					case 4:
					{
						TaskAwaiter taskAwaiter4;
						taskAwaiter3 = taskAwaiter4;
						taskAwaiter4 = default(TaskAwaiter);
						num2 = -1;
						goto IL_04C0;
					}
					case 5:
					{
						TaskAwaiter taskAwaiter4;
						taskAwaiter3 = taskAwaiter4;
						taskAwaiter4 = default(TaskAwaiter);
						num2 = -1;
						goto IL_0581;
					}
					default:
						dtcwizardSetupPage.btnStart.IsEnabled = false;
						if (App.OBDSimulator.IsActive)
						{
							DescriptionLoader.ResetCache();
							DescriptionLoader.PreloadDescriptionsForBrand(string.IsNullOrEmpty(SharedSettings.Current.BrandForDTC) ? SharedSettings.Current.SelectedBrand : SharedSettings.Current.BrandForDTC);
							DTCItemV2 dtcitemV = new DTCItemV2(new byte[] { 22, 147 }, "", "7E8", "03", DTCStatusHelper.DTCStatus.uds_bit3_confirmedDTC, "");
							DTCItemV2 dtcitemV2 = new DTCItemV2(new byte[] { 1, 35 }, "", "7E8", "07", DTCStatusHelper.DTCStatus.uds_bit2_pendingDTC, "");
							DTCItemV2 dtcitemV3 = new DTCItemV2(new byte[] { 3, 1 }, "", "7E8", "03", DTCStatusHelper.DTCStatus.uds_bit3_confirmedDTC, "");
							DTCItemV2 dtcitemV4 = new DTCItemV2(new byte[] { 0, 21, 147 }, "7E0", "7E8", "1902FA", DTCStatusHelper.DTCStatus.uds_bit5_testFailedSinceLastClear, "");
							dtcitemV.LoadDescription();
							dtcitemV2.LoadDescription();
							dtcitemV3.LoadDescription();
							dtcitemV4.LoadDescription();
							App.OBDReader.CurrentCarData.DTCs.Clear();
							App.OBDReader.CurrentCarData.DTCs.Enqueue(dtcitemV);
							App.OBDReader.CurrentCarData.DTCs.Enqueue(dtcitemV3);
							App.OBDReader.CurrentCarData.DTCs.Enqueue(dtcitemV2);
							App.OBDReader.CurrentCarData.DTCs.Enqueue(dtcitemV4);
							DTCWizardResultsPage dtcwizardResultsPage = new DTCWizardResultsPage(DTCWizardSetupPage.DTCMode.Read, new OBDRequest[0]);
							taskAwaiter3 = dtcwizardSetupPage.Navigation.PushAsync(dtcwizardResultsPage).GetAwaiter();
							if (!taskAwaiter3.IsCompleted)
							{
								num2 = 0;
								TaskAwaiter taskAwaiter4 = taskAwaiter3;
								this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, DTCWizardSetupPage.<StartRead>d__6>(ref taskAwaiter3, ref this);
								return;
							}
						}
						else
						{
							commands = new OBDRequest[0];
							if (dtcwizardSetupPage.rbProfile.IsChecked.GetValueOrDefault())
							{
								OBDDataReader obdreader = App.OBDReader;
								taskAwaiter3 = ((obdreader != null) ? obdreader.DebugWrite("\n[DTCRead:Auto]\n") : null).GetAwaiter();
								if (!taskAwaiter3.IsCompleted)
								{
									num2 = 1;
									TaskAwaiter taskAwaiter4 = taskAwaiter3;
									this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, DTCWizardSetupPage.<StartRead>d__6>(ref taskAwaiter3, ref this);
									return;
								}
								goto IL_02B1;
							}
							else
							{
								if ((int)dtcwizardSetupPage.sliderSettings.Value != 2 && (int)dtcwizardSetupPage.sliderSettings.Value < 4)
								{
									goto IL_03B5;
								}
								taskAwaiter5 = dtcwizardSetupPage.DisplayAlert(Translate.GetString("ios_DTCWarningTitle"), Translate.GetString("ios_DTCWarningText"), "OK", Translate.GetString("btnCancel.Content")).GetAwaiter();
								if (!taskAwaiter5.IsCompleted)
								{
									num2 = 2;
									taskAwaiter2 = taskAwaiter5;
									this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<bool>, DTCWizardSetupPage.<StartRead>d__6>(ref taskAwaiter5, ref this);
									return;
								}
								goto IL_03A7;
							}
						}
						break;
					}
					taskAwaiter3.GetResult();
					goto IL_058F;
					IL_02B1:
					taskAwaiter3.GetResult();
					if (App.OBDReader.IsNissanConsult2Protocol && App.OBDReader.CurrentELMFormat == ELMFormat.KWP)
					{
						commands = new OBDRequest[]
						{
							new OBDRequest("A3", false, null)
						};
						goto IL_0451;
					}
					commands = DTCWorker.GetProfileSequence(SharedSettings.Current.DTCReadingSequence, true);
					goto IL_0451;
					IL_03A7:
					if (!taskAwaiter5.GetResult())
					{
						goto IL_05B6;
					}
					IL_03B5:
					OBDDataReader obdreader2 = App.OBDReader;
					taskAwaiter3 = ((obdreader2 != null) ? obdreader2.DebugWrite(string.Format("\n[DTCRead:Manual({0})]\n", (int)dtcwizardSetupPage.sliderSettings.Value)) : null).GetAwaiter();
					if (!taskAwaiter3.IsCompleted)
					{
						num2 = 3;
						TaskAwaiter taskAwaiter4 = taskAwaiter3;
						this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, DTCWizardSetupPage.<StartRead>d__6>(ref taskAwaiter3, ref this);
						return;
					}
					IL_0432:
					taskAwaiter3.GetResult();
					commands = DTCWorker.GetClassicCommands((int)dtcwizardSetupPage.sliderSettings.Value, true);
					IL_0451:
					App.OBDReader.CurrentCarData.DTCs.Clear();
					taskAwaiter3 = App.OBDReader.ClearRequestQueue().GetAwaiter();
					if (!taskAwaiter3.IsCompleted)
					{
						num2 = 4;
						TaskAwaiter taskAwaiter4 = taskAwaiter3;
						this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, DTCWizardSetupPage.<StartRead>d__6>(ref taskAwaiter3, ref this);
						return;
					}
					IL_04C0:
					taskAwaiter3.GetResult();
					foreach (OBDRequest obdrequest in commands)
					{
						if (obdrequest.Command != null && !obdrequest.Command.StartsWith("AT"))
						{
							obdrequest.ResponseReceived += DTCWorker.Request_ResponseReceivedCheckForNR78;
						}
					}
					DTCWizardResultsPage dtcwizardResultsPage2 = new DTCWizardResultsPage(DTCWizardSetupPage.DTCMode.Read, commands);
					taskAwaiter3 = dtcwizardSetupPage.Navigation.PushAsync(dtcwizardResultsPage2).GetAwaiter();
					if (!taskAwaiter3.IsCompleted)
					{
						num2 = 5;
						TaskAwaiter taskAwaiter4 = taskAwaiter3;
						this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, DTCWizardSetupPage.<StartRead>d__6>(ref taskAwaiter3, ref this);
						return;
					}
					IL_0581:
					taskAwaiter3.GetResult();
					commands = null;
					IL_058F:
					dtcwizardSetupPage.btnStart.IsEnabled = true;
				}
				catch (Exception ex)
				{
					num2 = -2;
					this.<>t__builder.SetException(ex);
					return;
				}
				IL_05B6:
				num2 = -2;
				this.<>t__builder.SetResult();
			}

			// Token: 0x060032EA RID: 13034 RVA: 0x0023936C File Offset: 0x0023756C
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x04001DDB RID: 7643
			public int <>1__state;

			// Token: 0x04001DDC RID: 7644
			public AsyncVoidMethodBuilder <>t__builder;

			// Token: 0x04001DDD RID: 7645
			public DTCWizardSetupPage <>4__this;

			// Token: 0x04001DDE RID: 7646
			private TaskAwaiter <>u__1;

			// Token: 0x04001DDF RID: 7647
			private OBDRequest[] <commands>5__2;

			// Token: 0x04001DE0 RID: 7648
			private TaskAwaiter<bool> <>u__2;
		}
	}
}
