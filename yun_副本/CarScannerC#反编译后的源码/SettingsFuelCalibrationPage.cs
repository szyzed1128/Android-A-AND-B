using System;
using System.CodeDom.Compiler;
using System.Diagnostics;
using System.Globalization;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using CarScannerXamarinForms.Common;
using CarScannerXamarinForms.OBD2;
using CarScannerXamarinForms.OBD2.RequestProducers;
using CarScannerXamarinForms.Settings;
using CarScannerXamarinForms.UserControls;
using CarScannerXamarinForms.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Internals;
using Xamarin.Forms.PlatformConfiguration.iOSSpecific;
using Xamarin.Forms.Xaml;
using Xamarin.Forms.Xaml.Diagnostics;
using Xamarin.Forms.Xaml.Internals;

namespace CarScannerXamarinForms
{
	// Token: 0x0200017B RID: 379
	[XamlFilePath("Settings\\SettingsFuelCalibrationPage.xaml")]
	public class SettingsFuelCalibrationPage : ContentPage
	{
		// Token: 0x0600158D RID: 5517 RVA: 0x00095930 File Offset: 0x00093B30
		public SettingsFuelCalibrationPage()
		{
			try
			{
				base.BindingContext = SharedSettings.Current;
				this.InitializeComponent();
				if (SharedSettings.Current.Use_km)
				{
					this.entryOdometerFinish.Text = SharedSettings.Current.CalibrationStartOdometer.ToString(CultureInfo.InvariantCulture);
				}
				else
				{
					this.entryOdometerFinish.Text = (SharedSettings.Current.CalibrationStartOdometer / 1.60934).ToString(CultureInfo.InvariantCulture);
				}
			}
			catch (Exception)
			{
			}
		}

		// Token: 0x0600158E RID: 5518 RVA: 0x000027D4 File Offset: 0x000009D4
		private void Handle_SizeChanged(object sender, EventArgs e)
		{
		}

		// Token: 0x0600158F RID: 5519 RVA: 0x000959C8 File Offset: 0x00093BC8
		private async void Page_Disappearing(object sender, EventArgs e)
		{
		}

		// Token: 0x06001590 RID: 5520 RVA: 0x000959F8 File Offset: 0x00093BF8
		private void numericEntry_TextChanged(object sender, TextChangedEventArgs e)
		{
			string text = e.NewTextValue.Replace(".", CultureInfo.CurrentUICulture.NumberFormat.NumberDecimalSeparator).Replace(",", CultureInfo.CurrentUICulture.NumberFormat.NumberDecimalSeparator);
			Entry entry = sender as Entry;
			double num = 0.0;
			if (!(e.NewTextValue == "") && !(e.NewTextValue == "-") && !double.TryParse(text, out num))
			{
				if (!double.TryParse(e.OldTextValue, out num))
				{
					num = 0.0;
				}
				entry.Text = e.OldTextValue;
			}
		}

		// Token: 0x06001591 RID: 5521 RVA: 0x00095AA4 File Offset: 0x00093CA4
		private async void btnStartCalibration_Clicked(object sender, EventArgs e)
		{
			int num = 0;
			if (!int.TryParse(this.entryOdometerStart.Text, out num) || num <= 0)
			{
				await base.DisplayAlert(Translate.GetString("Calibration_IncorrectOdoTitle"), Translate.GetString("Calibration_IncorrectOdoText"), "OK");
			}
			else
			{
				double num2 = (double)num;
				if (!SharedSettings.Current.Use_km)
				{
					num2 *= 1.609344;
				}
				SharedSettings.Current.CalibrationStartOdometer = num2;
				this.entryOdometerFinish.Text = SharedSettings.Current.CalibrationStartOdometer.ToString(CultureInfo.InvariantCulture);
				string text = Translate.GetString("CalibrationJustFilledFuelTankFull");
				if (SharedSettings.Current.FuelFlowCorrectionFactor > 1.0 || SharedSettings.Current.FuelFlowCorrectionFactor < 1.0)
				{
					text = text + "\r\n" + Translate.GetString("CalibrationStartResetCurrent");
				}
				TaskAwaiter<bool> taskAwaiter = base.DisplayAlert(Translate.GetString("CalibrationStartQuestion"), text, Translate.GetString("ios_Yes"), Translate.GetString("ios_No")).GetAwaiter();
				TaskAwaiter<bool> taskAwaiter2;
				if (!taskAwaiter.IsCompleted)
				{
					await taskAwaiter;
					taskAwaiter = taskAwaiter2;
					taskAwaiter2 = default(TaskAwaiter<bool>);
				}
				if (taskAwaiter.GetResult())
				{
					SharedSettings.Current.CalibrationStartTime = DateTimeNowHelper.NowSafe;
					DriveCycle.SaveAndReset();
					if (!SharedSettings.Current.AlwaysRecordFuelConsumption)
					{
						taskAwaiter = base.DisplayAlert(Translate.GetString("Calibration_RecomendationAlwayRecordFuelConsumptionTitle"), Translate.GetString("Calibration_RecomendationAlwayRecordFuelConsumptionText"), Translate.GetString("ios_Yes"), Translate.GetString("ios_No")).GetAwaiter();
						if (!taskAwaiter.IsCompleted)
						{
							await taskAwaiter;
							taskAwaiter = taskAwaiter2;
							taskAwaiter2 = default(TaskAwaiter<bool>);
						}
						if (taskAwaiter.GetResult())
						{
							SharedSettings.Current.AlwaysRecordFuelConsumption = true;
						}
					}
					if (App.OBDReader.CurrentStatus == OBDDataReaderStatus.ConnectedToECU)
					{
						RequestProducerStatic.UpdateOBDReaderRequests();
					}
					await base.DisplayAlert(Translate.GetString("Calibration_StartedMessageTitle"), Translate.GetString("Settings_Control_tbReadyToFinishCalibration.Text"), "OK");
					await base.Navigation.PopAsync();
				}
			}
		}

		// Token: 0x06001592 RID: 5522 RVA: 0x00095ADC File Offset: 0x00093CDC
		private async void btnResetCalibration_Clicked(object sender, EventArgs e)
		{
			TaskAwaiter<bool> taskAwaiter = base.DisplayAlert(Translate.GetString("ios_ResetCalibrationTitle"), Translate.GetString("ios_ResetCalibrationText"), "OK", Translate.GetString("btnCancel.Content")).GetAwaiter();
			if (!taskAwaiter.IsCompleted)
			{
				await taskAwaiter;
				TaskAwaiter<bool> taskAwaiter2;
				taskAwaiter = taskAwaiter2;
				taskAwaiter2 = default(TaskAwaiter<bool>);
			}
			if (taskAwaiter.GetResult())
			{
				SharedSettings.Current.FuelFlowCorrectionFactor = 1.0;
			}
		}

		// Token: 0x06001593 RID: 5523 RVA: 0x00095B14 File Offset: 0x00093D14
		private async void btnFinishCalibration_Clicked(object sender, EventArgs e)
		{
			string text = this.entryFuelFilled.Text.Replace(".", CultureInfo.CurrentUICulture.NumberFormat.NumberDecimalSeparator).Replace(",", CultureInfo.CurrentUICulture.NumberFormat.NumberDecimalSeparator);
			double fuel_vol = 0.0;
			if (!double.TryParse(text, out fuel_vol))
			{
				await base.DisplayAlert(Translate.GetString("Calibration_IncorrectFuelTitle"), Translate.GetString("Calibration_IncorrectFuelText"), "OK");
			}
			else
			{
				double odo = 0.0;
				if (!double.TryParse(this.entryOdometerFinish.Text.Replace(".", CultureInfo.CurrentUICulture.NumberFormat.NumberDecimalSeparator).Replace(",", CultureInfo.CurrentUICulture.NumberFormat.NumberDecimalSeparator), out odo))
				{
					await base.DisplayAlert(Translate.GetString("Calibration_IncorrectOdoTitle"), Translate.GetString("Calibration_IncorrectOdoText"), "OK");
				}
				else
				{
					if (!SharedSettings.Current.Use_km)
					{
						odo *= 1.609344;
					}
					if (odo <= SharedSettings.Current.CalibrationStartOdometer)
					{
						await base.DisplayAlert(Translate.GetString("Calibration_IncorrectOdoTitle"), Translate.GetString("Calibration_IncorrectOdoText"), "OK");
					}
					else
					{
						if (!SharedSettings.Current.UseLitersForVolume)
						{
							if (SharedSettings.Current.UseUSGallon)
							{
								fuel_vol *= 3.78541;
							}
							else
							{
								fuel_vol *= 4.54609;
							}
						}
						TaskAwaiter<bool> taskAwaiter = base.DisplayAlert(Translate.GetString("Calibration_FinishTitle"), Translate.GetString("Calibration_FinishText"), Translate.GetString("ios_Yes"), Translate.GetString("ios_No")).GetAwaiter();
						if (!taskAwaiter.IsCompleted)
						{
							await taskAwaiter;
							TaskAwaiter<bool> taskAwaiter2;
							taskAwaiter = taskAwaiter2;
							taskAwaiter2 = default(TaskAwaiter<bool>);
						}
						if (taskAwaiter.GetResult())
						{
							this.btnFinishCalibration.IsEnabled = false;
							DriveCycleViewModel driveCycleViewModel = DriveCycleViewModel.Current;
							driveCycleViewModel.CalculatePeriodForCalibration(SharedSettings.Current.CalibrationStartTime);
							double periodAvgFuelConsumption = driveCycleViewModel.PeriodAvgFuelConsumption;
							double num = fuel_vol / (odo - SharedSettings.Current.CalibrationStartOdometer) * 100.0;
							double num2 = Math.Abs(num / periodAvgFuelConsumption);
							if (double.IsInfinity(num2) || double.IsNaN(num2) || num2 <= 0.0)
							{
								this.btnFinishCalibration.IsEnabled = true;
								string text2 = string.Format(Translate.GetString("ios_CalibrationErrorText"), num, periodAvgFuelConsumption);
								await base.DisplayAlert(Translate.GetString("ios_CalibrationErrorTitle"), text2, "OK");
							}
							else
							{
								SharedSettings.Current.FuelFlowCorrectionFactor = num2;
								this.btnFinishCalibration.IsEnabled = true;
								SharedSettings.Current.CalibrationStartTime = DateTime.MinValue;
								SharedSettings.Current.CalibrationStartOdometer = 0.0;
							}
						}
					}
				}
			}
		}

		// Token: 0x06001594 RID: 5524 RVA: 0x00095B4B File Offset: 0x00093D4B
		private void btnCancelCalibration_Clicked(object sender, EventArgs e)
		{
			SharedSettings.Current.CalibrationStartTime = DateTime.MinValue;
			SharedSettings.Current.CalibrationStartOdometer = 0.0;
		}

		// Token: 0x06001595 RID: 5525 RVA: 0x00095B6F File Offset: 0x00093D6F
		private void btnFuelCalibrationManual_Clicked(object sender, EventArgs e)
		{
			this.panel_ManualCalibrationSet.IsVisible = !this.panel_ManualCalibrationSet.IsVisible;
			this.labelCalibrationFactor.IsVisible = !this.panel_ManualCalibrationSet.IsVisible;
			bool isVisible = this.panel_ManualCalibrationSet.IsVisible;
		}

		// Token: 0x06001596 RID: 5526 RVA: 0x00095BAF File Offset: 0x00093DAF
		private void btnOK_ManualCalibration_Clicked(object sender, EventArgs e)
		{
			this.panel_ManualCalibrationSet.IsVisible = false;
			this.labelCalibrationFactor.IsVisible = true;
		}

		// Token: 0x06001597 RID: 5527 RVA: 0x00095BCC File Offset: 0x00093DCC
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private void InitializeComponent()
		{
			if (ResourceLoader.IsEnabled && ResourceLoader.CanProvideContentFor(new ResourceLoader.ResourceLoadingQuery
			{
				AssemblyName = typeof(SettingsFuelCalibrationPage).GetTypeInfo().Assembly.GetName(),
				ResourcePath = "Settings/SettingsFuelCalibrationPage.xaml",
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
			VisualDiagnostics.RegisterSourceInfo(translate = new Translate(), new Uri("Settings\\SettingsFuelCalibrationPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 9, 5);
			DynamicResourceExtension dynamicResourceExtension;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension = new DynamicResourceExtension(), new Uri("Settings\\SettingsFuelCalibrationPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 12, 5);
			BoolToNegativeConverter boolToNegativeConverter;
			VisualDiagnostics.RegisterSourceInfo(boolToNegativeConverter = new BoolToNegativeConverter(), new Uri("Settings\\SettingsFuelCalibrationPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 17, 14);
			ResourceDictionary resourceDictionary;
			VisualDiagnostics.RegisterSourceInfo(resourceDictionary = new ResourceDictionary(), new Uri("Settings\\SettingsFuelCalibrationPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 16, 10);
			OnPlatform<Thickness> onPlatform;
			VisualDiagnostics.RegisterSourceInfo(onPlatform = new OnPlatform<Thickness>(), new Uri("Settings\\SettingsFuelCalibrationPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 24, 22);
			StaticResourceExtension staticResourceExtension;
			VisualDiagnostics.RegisterSourceInfo(staticResourceExtension = new StaticResourceExtension(), new Uri("Settings\\SettingsFuelCalibrationPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 32, 21);
			BindingExtension bindingExtension;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension = new BindingExtension(), new Uri("Settings\\SettingsFuelCalibrationPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 32, 21);
			Translate translate2;
			VisualDiagnostics.RegisterSourceInfo(translate2 = new Translate(), new Uri("Settings\\SettingsFuelCalibrationPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 34, 28);
			Label label;
			VisualDiagnostics.RegisterSourceInfo(label = new Label(), new Uri("Settings\\SettingsFuelCalibrationPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 34, 22);
			Translate translate3;
			VisualDiagnostics.RegisterSourceInfo(translate3 = new Translate(), new Uri("Settings\\SettingsFuelCalibrationPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 35, 28);
			Label label2;
			VisualDiagnostics.RegisterSourceInfo(label2 = new Label(), new Uri("Settings\\SettingsFuelCalibrationPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 35, 22);
			ColumnDefinition columnDefinition;
			VisualDiagnostics.RegisterSourceInfo(columnDefinition = new ColumnDefinition(), new Uri("Settings\\SettingsFuelCalibrationPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 38, 30);
			ColumnDefinition columnDefinition2;
			VisualDiagnostics.RegisterSourceInfo(columnDefinition2 = new ColumnDefinition(), new Uri("Settings\\SettingsFuelCalibrationPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 39, 30);
			Entry entry;
			VisualDiagnostics.RegisterSourceInfo(entry = new Entry(), new Uri("Settings\\SettingsFuelCalibrationPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 41, 26);
			BindingExtension bindingExtension2;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension2 = new BindingExtension(), new Uri("Settings\\SettingsFuelCalibrationPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 48, 29);
			Translate translate4;
			VisualDiagnostics.RegisterSourceInfo(translate4 = new Translate(), new Uri("Settings\\SettingsFuelCalibrationPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 49, 29);
			Label label3;
			VisualDiagnostics.RegisterSourceInfo(label3 = new Label(), new Uri("Settings\\SettingsFuelCalibrationPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 46, 26);
			StaticResourceExtension staticResourceExtension2;
			VisualDiagnostics.RegisterSourceInfo(staticResourceExtension2 = new StaticResourceExtension(), new Uri("Settings\\SettingsFuelCalibrationPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 52, 29);
			BindingExtension bindingExtension3;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension3 = new BindingExtension(), new Uri("Settings\\SettingsFuelCalibrationPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 52, 29);
			Translate translate5;
			VisualDiagnostics.RegisterSourceInfo(translate5 = new Translate(), new Uri("Settings\\SettingsFuelCalibrationPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 53, 29);
			Label label4;
			VisualDiagnostics.RegisterSourceInfo(label4 = new Label(), new Uri("Settings\\SettingsFuelCalibrationPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 50, 26);
			Grid grid;
			VisualDiagnostics.RegisterSourceInfo(grid = new Grid(), new Uri("Settings\\SettingsFuelCalibrationPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 36, 22);
			Translate translate6;
			VisualDiagnostics.RegisterSourceInfo(translate6 = new Translate(), new Uri("Settings\\SettingsFuelCalibrationPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 58, 25);
			Button button;
			VisualDiagnostics.RegisterSourceInfo(button = new Button(), new Uri("Settings\\SettingsFuelCalibrationPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 55, 22);
			Translate translate7;
			VisualDiagnostics.RegisterSourceInfo(translate7 = new Translate(), new Uri("Settings\\SettingsFuelCalibrationPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 59, 28);
			Label label5;
			VisualDiagnostics.RegisterSourceInfo(label5 = new Label(), new Uri("Settings\\SettingsFuelCalibrationPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 59, 22);
			BindingExtension bindingExtension4;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension4 = new BindingExtension(), new Uri("Settings\\SettingsFuelCalibrationPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 65, 25);
			Label label6;
			VisualDiagnostics.RegisterSourceInfo(label6 = new Label(), new Uri("Settings\\SettingsFuelCalibrationPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 60, 22);
			ColumnDefinition columnDefinition3;
			VisualDiagnostics.RegisterSourceInfo(columnDefinition3 = new ColumnDefinition(), new Uri("Settings\\SettingsFuelCalibrationPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 68, 30);
			ColumnDefinition columnDefinition4;
			VisualDiagnostics.RegisterSourceInfo(columnDefinition4 = new ColumnDefinition(), new Uri("Settings\\SettingsFuelCalibrationPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 69, 30);
			BindingExtension bindingExtension5;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension5 = new BindingExtension(), new Uri("Settings\\SettingsFuelCalibrationPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 76, 29);
			NumericEntryV3 numericEntryV;
			VisualDiagnostics.RegisterSourceInfo(numericEntryV = new NumericEntryV3(), new Uri("Settings\\SettingsFuelCalibrationPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 71, 26);
			Button button2;
			VisualDiagnostics.RegisterSourceInfo(button2 = new Button(), new Uri("Settings\\SettingsFuelCalibrationPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 78, 26);
			Grid grid2;
			VisualDiagnostics.RegisterSourceInfo(grid2 = new Grid(), new Uri("Settings\\SettingsFuelCalibrationPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 66, 22);
			Translate translate8;
			VisualDiagnostics.RegisterSourceInfo(translate8 = new Translate(), new Uri("Settings\\SettingsFuelCalibrationPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 87, 25);
			Button button3;
			VisualDiagnostics.RegisterSourceInfo(button3 = new Button(), new Uri("Settings\\SettingsFuelCalibrationPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 84, 22);
			Translate translate9;
			VisualDiagnostics.RegisterSourceInfo(translate9 = new Translate(), new Uri("Settings\\SettingsFuelCalibrationPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 91, 25);
			Button button4;
			VisualDiagnostics.RegisterSourceInfo(button4 = new Button(), new Uri("Settings\\SettingsFuelCalibrationPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 88, 22);
			StackLayout stackLayout;
			VisualDiagnostics.RegisterSourceInfo(stackLayout = new StackLayout(), new Uri("Settings\\SettingsFuelCalibrationPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 30, 18);
			BindingExtension bindingExtension6;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension6 = new BindingExtension(), new Uri("Settings\\SettingsFuelCalibrationPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 97, 21);
			Translate translate10;
			VisualDiagnostics.RegisterSourceInfo(translate10 = new Translate(), new Uri("Settings\\SettingsFuelCalibrationPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 99, 28);
			Label label7;
			VisualDiagnostics.RegisterSourceInfo(label7 = new Label(), new Uri("Settings\\SettingsFuelCalibrationPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 99, 22);
			Translate translate11;
			VisualDiagnostics.RegisterSourceInfo(translate11 = new Translate(), new Uri("Settings\\SettingsFuelCalibrationPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 100, 28);
			Label label8;
			VisualDiagnostics.RegisterSourceInfo(label8 = new Label(), new Uri("Settings\\SettingsFuelCalibrationPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 100, 22);
			ColumnDefinition columnDefinition5;
			VisualDiagnostics.RegisterSourceInfo(columnDefinition5 = new ColumnDefinition(), new Uri("Settings\\SettingsFuelCalibrationPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 103, 30);
			ColumnDefinition columnDefinition6;
			VisualDiagnostics.RegisterSourceInfo(columnDefinition6 = new ColumnDefinition(), new Uri("Settings\\SettingsFuelCalibrationPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 104, 30);
			Entry entry2;
			VisualDiagnostics.RegisterSourceInfo(entry2 = new Entry(), new Uri("Settings\\SettingsFuelCalibrationPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 106, 26);
			BindingExtension bindingExtension7;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension7 = new BindingExtension(), new Uri("Settings\\SettingsFuelCalibrationPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 113, 29);
			Translate translate12;
			VisualDiagnostics.RegisterSourceInfo(translate12 = new Translate(), new Uri("Settings\\SettingsFuelCalibrationPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 114, 29);
			Label label9;
			VisualDiagnostics.RegisterSourceInfo(label9 = new Label(), new Uri("Settings\\SettingsFuelCalibrationPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 111, 26);
			StaticResourceExtension staticResourceExtension3;
			VisualDiagnostics.RegisterSourceInfo(staticResourceExtension3 = new StaticResourceExtension(), new Uri("Settings\\SettingsFuelCalibrationPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 117, 29);
			BindingExtension bindingExtension8;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension8 = new BindingExtension(), new Uri("Settings\\SettingsFuelCalibrationPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 117, 29);
			Translate translate13;
			VisualDiagnostics.RegisterSourceInfo(translate13 = new Translate(), new Uri("Settings\\SettingsFuelCalibrationPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 118, 29);
			Label label10;
			VisualDiagnostics.RegisterSourceInfo(label10 = new Label(), new Uri("Settings\\SettingsFuelCalibrationPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 115, 26);
			Grid grid3;
			VisualDiagnostics.RegisterSourceInfo(grid3 = new Grid(), new Uri("Settings\\SettingsFuelCalibrationPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 101, 22);
			Translate translate14;
			VisualDiagnostics.RegisterSourceInfo(translate14 = new Translate(), new Uri("Settings\\SettingsFuelCalibrationPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 121, 28);
			Label label11;
			VisualDiagnostics.RegisterSourceInfo(label11 = new Label(), new Uri("Settings\\SettingsFuelCalibrationPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 121, 22);
			ColumnDefinition columnDefinition7;
			VisualDiagnostics.RegisterSourceInfo(columnDefinition7 = new ColumnDefinition(), new Uri("Settings\\SettingsFuelCalibrationPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 124, 30);
			ColumnDefinition columnDefinition8;
			VisualDiagnostics.RegisterSourceInfo(columnDefinition8 = new ColumnDefinition(), new Uri("Settings\\SettingsFuelCalibrationPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 125, 30);
			Entry entry3;
			VisualDiagnostics.RegisterSourceInfo(entry3 = new Entry(), new Uri("Settings\\SettingsFuelCalibrationPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 127, 26);
			BindingExtension bindingExtension9;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension9 = new BindingExtension(), new Uri("Settings\\SettingsFuelCalibrationPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 134, 29);
			Translate translate15;
			VisualDiagnostics.RegisterSourceInfo(translate15 = new Translate(), new Uri("Settings\\SettingsFuelCalibrationPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 135, 29);
			Label label12;
			VisualDiagnostics.RegisterSourceInfo(label12 = new Label(), new Uri("Settings\\SettingsFuelCalibrationPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 132, 26);
			StaticResourceExtension staticResourceExtension4;
			VisualDiagnostics.RegisterSourceInfo(staticResourceExtension4 = new StaticResourceExtension(), new Uri("Settings\\SettingsFuelCalibrationPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 138, 29);
			BindingExtension bindingExtension10;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension10 = new BindingExtension(), new Uri("Settings\\SettingsFuelCalibrationPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 138, 29);
			Translate translate16;
			VisualDiagnostics.RegisterSourceInfo(translate16 = new Translate(), new Uri("Settings\\SettingsFuelCalibrationPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 139, 29);
			Label label13;
			VisualDiagnostics.RegisterSourceInfo(label13 = new Label(), new Uri("Settings\\SettingsFuelCalibrationPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 136, 26);
			Grid grid4;
			VisualDiagnostics.RegisterSourceInfo(grid4 = new Grid(), new Uri("Settings\\SettingsFuelCalibrationPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 122, 22);
			Translate translate17;
			VisualDiagnostics.RegisterSourceInfo(translate17 = new Translate(), new Uri("Settings\\SettingsFuelCalibrationPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 144, 25);
			Button button5;
			VisualDiagnostics.RegisterSourceInfo(button5 = new Button(), new Uri("Settings\\SettingsFuelCalibrationPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 141, 22);
			Translate translate18;
			VisualDiagnostics.RegisterSourceInfo(translate18 = new Translate(), new Uri("Settings\\SettingsFuelCalibrationPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 148, 25);
			Button button6;
			VisualDiagnostics.RegisterSourceInfo(button6 = new Button(), new Uri("Settings\\SettingsFuelCalibrationPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 145, 22);
			StackLayout stackLayout2;
			VisualDiagnostics.RegisterSourceInfo(stackLayout2 = new StackLayout(), new Uri("Settings\\SettingsFuelCalibrationPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 95, 18);
			StackLayout stackLayout3;
			VisualDiagnostics.RegisterSourceInfo(stackLayout3 = new StackLayout(), new Uri("Settings\\SettingsFuelCalibrationPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 22, 14);
			ScrollView scrollView;
			VisualDiagnostics.RegisterSourceInfo(scrollView = new ScrollView(), new Uri("Settings\\SettingsFuelCalibrationPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 21, 10);
			VisualDiagnostics.RegisterSourceInfo(this, new Uri("Settings\\SettingsFuelCalibrationPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 2, 2);
			NameScope nameScope = NameScope.GetNameScope(this) ?? new NameScope();
			NameScope.SetNameScope(this, nameScope);
			nameScope.RegisterName("panelBeforeCalibrationStarted", stackLayout);
			if (stackLayout.StyleId == null)
			{
				stackLayout.StyleId = "panelBeforeCalibrationStarted";
			}
			nameScope.RegisterName("entryOdometerStart", entry);
			if (entry.StyleId == null)
			{
				entry.StyleId = "entryOdometerStart";
			}
			nameScope.RegisterName("btnStartCalibration", button);
			if (button.StyleId == null)
			{
				button.StyleId = "btnStartCalibration";
			}
			nameScope.RegisterName("labelCalibrationFactor", label6);
			if (label6.StyleId == null)
			{
				label6.StyleId = "labelCalibrationFactor";
			}
			nameScope.RegisterName("panel_ManualCalibrationSet", grid2);
			if (grid2.StyleId == null)
			{
				grid2.StyleId = "panel_ManualCalibrationSet";
			}
			nameScope.RegisterName("entryFuelCorrectionManual", numericEntryV);
			if (numericEntryV.StyleId == null)
			{
				numericEntryV.StyleId = "entryFuelCorrectionManual";
			}
			nameScope.RegisterName("btnOK_ManualCalibration", button2);
			if (button2.StyleId == null)
			{
				button2.StyleId = "btnOK_ManualCalibration";
			}
			nameScope.RegisterName("btnFuelCalibrationManual", button3);
			if (button3.StyleId == null)
			{
				button3.StyleId = "btnFuelCalibrationManual";
			}
			nameScope.RegisterName("btnResetCalibration", button4);
			if (button4.StyleId == null)
			{
				button4.StyleId = "btnResetCalibration";
			}
			nameScope.RegisterName("panelAfterCalibrationStarted", stackLayout2);
			if (stackLayout2.StyleId == null)
			{
				stackLayout2.StyleId = "panelAfterCalibrationStarted";
			}
			nameScope.RegisterName("entryFuelFilled", entry2);
			if (entry2.StyleId == null)
			{
				entry2.StyleId = "entryFuelFilled";
			}
			nameScope.RegisterName("entryOdometerFinish", entry3);
			if (entry3.StyleId == null)
			{
				entry3.StyleId = "entryOdometerFinish";
			}
			nameScope.RegisterName("btnFinishCalibration", button5);
			if (button5.StyleId == null)
			{
				button5.StyleId = "btnFinishCalibration";
			}
			nameScope.RegisterName("btnCancelCalibration", button6);
			if (button6.StyleId == null)
			{
				button6.StyleId = "btnCancelCalibration";
			}
			this.panelBeforeCalibrationStarted = stackLayout;
			this.entryOdometerStart = entry;
			this.btnStartCalibration = button;
			this.labelCalibrationFactor = label6;
			this.panel_ManualCalibrationSet = grid2;
			this.entryFuelCorrectionManual = numericEntryV;
			this.btnOK_ManualCalibration = button2;
			this.btnFuelCalibrationManual = button3;
			this.btnResetCalibration = button4;
			this.panelAfterCalibrationStarted = stackLayout2;
			this.entryFuelFilled = entry2;
			this.entryOdometerFinish = entry3;
			this.btnFinishCalibration = button5;
			this.btnCancelCalibration = button6;
			this.Resources = resourceDictionary;
			resourceDictionary.Add("BoolToNegativeConverter", boolToNegativeConverter);
			translate.Text = "Settings_Control_tbFuelCalibration.Text";
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
			xmlNamespaceResolver.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider.Add(typeFromHandle2, new XamlTypeResolver(xmlNamespaceResolver, typeof(SettingsFuelCalibrationPage).GetTypeInfo().Assembly));
			xamlServiceProvider.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(9, 5)));
			object obj2 = markupExtension.ProvideValue(xamlServiceProvider);
			this.Title = obj2;
			this.SetValue(Page.PaddingProperty, new Thickness(0.0));
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
			xmlNamespaceResolver2.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver2.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver2.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider2.Add(typeFromHandle4, new XamlTypeResolver(xmlNamespaceResolver2, typeof(SettingsFuelCalibrationPage).GetTypeInfo().Assembly));
			xamlServiceProvider2.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(12, 5)));
			DynamicResource dynamicResource = markupExtension2.ProvideValue(xamlServiceProvider2);
			this.SetDynamicResource(VisualElement.BackgroundColorProperty, dynamicResource.Key);
			this.Disappearing += this.Page_Disappearing;
			this.SizeChanged += this.Handle_SizeChanged;
			this.Resources = resourceDictionary;
			scrollView.SetValue(ScrollView.OrientationProperty, 0);
			stackLayout3.SetValue(StackLayout.OrientationProperty, 0);
			onPlatform.Android = new Thickness(5.0, 0.0, 5.0, 0.0);
			onPlatform.iOS = new Thickness(5.0, 0.0, 5.0, 0.0);
			stackLayout3.SetValue(View.MarginProperty, onPlatform);
			staticResourceExtension.Key = "BoolToNegativeConverter";
			IMarkupExtension markupExtension3 = staticResourceExtension;
			XamlServiceProvider xamlServiceProvider3 = new XamlServiceProvider();
			Type typeFromHandle5 = typeof(IProvideValueTarget);
			object[] array3 = new object[0 + 5];
			array3[0] = bindingExtension;
			array3[1] = stackLayout;
			array3[2] = stackLayout3;
			array3[3] = scrollView;
			array3[4] = this;
			object obj4;
			xamlServiceProvider3.Add(typeFromHandle5, obj4 = new SimpleValueTargetProvider(array3, typeof(BindingExtension).GetRuntimeProperty("Converter"), nameScope));
			xamlServiceProvider3.Add(typeof(IReferenceProvider), obj4);
			Type typeFromHandle6 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver3 = new XmlNamespaceResolver();
			xmlNamespaceResolver3.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver3.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver3.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver3.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver3.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider3.Add(typeFromHandle6, new XamlTypeResolver(xmlNamespaceResolver3, typeof(SettingsFuelCalibrationPage).GetTypeInfo().Assembly));
			xamlServiceProvider3.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(32, 21)));
			object obj5 = markupExtension3.ProvideValue(xamlServiceProvider3);
			bindingExtension.Converter = obj5;
			bindingExtension.Path = "IsCalibrationStartTimeSet";
			BindingBase bindingBase = bindingExtension.ProvideValue(null);
			stackLayout.SetBinding(VisualElement.IsVisibleProperty, bindingBase);
			stackLayout.SetValue(StackLayout.OrientationProperty, 0);
			translate2.Text = "Settings_Control_tbCalibrationBeforeStart.Text";
			IMarkupExtension markupExtension4 = translate2;
			XamlServiceProvider xamlServiceProvider4 = new XamlServiceProvider();
			Type typeFromHandle7 = typeof(IProvideValueTarget);
			object[] array4 = new object[0 + 5];
			array4[0] = label;
			array4[1] = stackLayout;
			array4[2] = stackLayout3;
			array4[3] = scrollView;
			array4[4] = this;
			object obj6;
			xamlServiceProvider4.Add(typeFromHandle7, obj6 = new SimpleValueTargetProvider(array4, Label.TextProperty, nameScope));
			xamlServiceProvider4.Add(typeof(IReferenceProvider), obj6);
			Type typeFromHandle8 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver4 = new XmlNamespaceResolver();
			xmlNamespaceResolver4.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver4.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver4.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver4.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver4.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider4.Add(typeFromHandle8, new XamlTypeResolver(xmlNamespaceResolver4, typeof(SettingsFuelCalibrationPage).GetTypeInfo().Assembly));
			xamlServiceProvider4.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(34, 28)));
			object obj7 = markupExtension4.ProvideValue(xamlServiceProvider4);
			label.Text = obj7;
			stackLayout.Children.Add(label);
			translate3.Text = "Settings_Control_tbOdoInput.Text";
			IMarkupExtension markupExtension5 = translate3;
			XamlServiceProvider xamlServiceProvider5 = new XamlServiceProvider();
			Type typeFromHandle9 = typeof(IProvideValueTarget);
			object[] array5 = new object[0 + 5];
			array5[0] = label2;
			array5[1] = stackLayout;
			array5[2] = stackLayout3;
			array5[3] = scrollView;
			array5[4] = this;
			object obj8;
			xamlServiceProvider5.Add(typeFromHandle9, obj8 = new SimpleValueTargetProvider(array5, Label.TextProperty, nameScope));
			xamlServiceProvider5.Add(typeof(IReferenceProvider), obj8);
			Type typeFromHandle10 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver5 = new XmlNamespaceResolver();
			xmlNamespaceResolver5.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver5.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver5.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver5.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver5.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider5.Add(typeFromHandle10, new XamlTypeResolver(xmlNamespaceResolver5, typeof(SettingsFuelCalibrationPage).GetTypeInfo().Assembly));
			xamlServiceProvider5.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(35, 28)));
			object obj9 = markupExtension5.ProvideValue(xamlServiceProvider5);
			label2.Text = obj9;
			stackLayout.Children.Add(label2);
			columnDefinition.SetValue(ColumnDefinition.WidthProperty, new GridLengthTypeConverter().ConvertFromInvariantString("*"));
			grid.GetValue(Grid.ColumnDefinitionsProperty).Add(columnDefinition);
			columnDefinition2.SetValue(ColumnDefinition.WidthProperty, new GridLengthTypeConverter().ConvertFromInvariantString("auto"));
			grid.GetValue(Grid.ColumnDefinitionsProperty).Add(columnDefinition2);
			entry.SetValue(Grid.ColumnProperty, 0);
			entry.SetValue(Entry.TextProperty, "0");
			entry.TextChanged += this.numericEntry_TextChanged;
			grid.Children.Add(entry);
			label3.SetValue(Grid.ColumnProperty, 1);
			bindingExtension2.Mode = 2;
			bindingExtension2.Path = "Use_km";
			BindingBase bindingBase2 = bindingExtension2.ProvideValue(null);
			label3.SetBinding(VisualElement.IsVisibleProperty, bindingBase2);
			translate4.Text = "Settings_Control_tbKm.Text";
			IMarkupExtension markupExtension6 = translate4;
			XamlServiceProvider xamlServiceProvider6 = new XamlServiceProvider();
			Type typeFromHandle11 = typeof(IProvideValueTarget);
			object[] array6 = new object[0 + 6];
			array6[0] = label3;
			array6[1] = grid;
			array6[2] = stackLayout;
			array6[3] = stackLayout3;
			array6[4] = scrollView;
			array6[5] = this;
			object obj10;
			xamlServiceProvider6.Add(typeFromHandle11, obj10 = new SimpleValueTargetProvider(array6, Label.TextProperty, nameScope));
			xamlServiceProvider6.Add(typeof(IReferenceProvider), obj10);
			Type typeFromHandle12 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver6 = new XmlNamespaceResolver();
			xmlNamespaceResolver6.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver6.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver6.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver6.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver6.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider6.Add(typeFromHandle12, new XamlTypeResolver(xmlNamespaceResolver6, typeof(SettingsFuelCalibrationPage).GetTypeInfo().Assembly));
			xamlServiceProvider6.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(49, 29)));
			object obj11 = markupExtension6.ProvideValue(xamlServiceProvider6);
			label3.Text = obj11;
			grid.Children.Add(label3);
			label4.SetValue(Grid.ColumnProperty, 1);
			bindingExtension3.Mode = 2;
			staticResourceExtension2.Key = "BoolToNegativeConverter";
			IMarkupExtension markupExtension7 = staticResourceExtension2;
			XamlServiceProvider xamlServiceProvider7 = new XamlServiceProvider();
			Type typeFromHandle13 = typeof(IProvideValueTarget);
			object[] array7 = new object[0 + 7];
			array7[0] = bindingExtension3;
			array7[1] = label4;
			array7[2] = grid;
			array7[3] = stackLayout;
			array7[4] = stackLayout3;
			array7[5] = scrollView;
			array7[6] = this;
			object obj12;
			xamlServiceProvider7.Add(typeFromHandle13, obj12 = new SimpleValueTargetProvider(array7, typeof(BindingExtension).GetRuntimeProperty("Converter"), nameScope));
			xamlServiceProvider7.Add(typeof(IReferenceProvider), obj12);
			Type typeFromHandle14 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver7 = new XmlNamespaceResolver();
			xmlNamespaceResolver7.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver7.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver7.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver7.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver7.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider7.Add(typeFromHandle14, new XamlTypeResolver(xmlNamespaceResolver7, typeof(SettingsFuelCalibrationPage).GetTypeInfo().Assembly));
			xamlServiceProvider7.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(52, 29)));
			object obj13 = markupExtension7.ProvideValue(xamlServiceProvider7);
			bindingExtension3.Converter = obj13;
			bindingExtension3.Path = "Use_km";
			BindingBase bindingBase3 = bindingExtension3.ProvideValue(null);
			label4.SetBinding(VisualElement.IsVisibleProperty, bindingBase3);
			translate5.Text = "Settings_Control_tbMiles.Text";
			IMarkupExtension markupExtension8 = translate5;
			XamlServiceProvider xamlServiceProvider8 = new XamlServiceProvider();
			Type typeFromHandle15 = typeof(IProvideValueTarget);
			object[] array8 = new object[0 + 6];
			array8[0] = label4;
			array8[1] = grid;
			array8[2] = stackLayout;
			array8[3] = stackLayout3;
			array8[4] = scrollView;
			array8[5] = this;
			object obj14;
			xamlServiceProvider8.Add(typeFromHandle15, obj14 = new SimpleValueTargetProvider(array8, Label.TextProperty, nameScope));
			xamlServiceProvider8.Add(typeof(IReferenceProvider), obj14);
			Type typeFromHandle16 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver8 = new XmlNamespaceResolver();
			xmlNamespaceResolver8.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver8.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver8.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver8.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver8.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider8.Add(typeFromHandle16, new XamlTypeResolver(xmlNamespaceResolver8, typeof(SettingsFuelCalibrationPage).GetTypeInfo().Assembly));
			xamlServiceProvider8.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(53, 29)));
			object obj15 = markupExtension8.ProvideValue(xamlServiceProvider8);
			label4.Text = obj15;
			grid.Children.Add(label4);
			stackLayout.Children.Add(grid);
			button.Clicked += this.btnStartCalibration_Clicked;
			translate6.Text = "Settings_Control_btnStartCalibration.Content";
			IMarkupExtension markupExtension9 = translate6;
			XamlServiceProvider xamlServiceProvider9 = new XamlServiceProvider();
			Type typeFromHandle17 = typeof(IProvideValueTarget);
			object[] array9 = new object[0 + 5];
			array9[0] = button;
			array9[1] = stackLayout;
			array9[2] = stackLayout3;
			array9[3] = scrollView;
			array9[4] = this;
			object obj16;
			xamlServiceProvider9.Add(typeFromHandle17, obj16 = new SimpleValueTargetProvider(array9, Button.TextProperty, nameScope));
			xamlServiceProvider9.Add(typeof(IReferenceProvider), obj16);
			Type typeFromHandle18 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver9 = new XmlNamespaceResolver();
			xmlNamespaceResolver9.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver9.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver9.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver9.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver9.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider9.Add(typeFromHandle18, new XamlTypeResolver(xmlNamespaceResolver9, typeof(SettingsFuelCalibrationPage).GetTypeInfo().Assembly));
			xamlServiceProvider9.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(58, 25)));
			object obj17 = markupExtension9.ProvideValue(xamlServiceProvider9);
			button.Text = obj17;
			stackLayout.Children.Add(button);
			translate7.Text = "Settings_Control_tbCurrentCalibration.Text";
			IMarkupExtension markupExtension10 = translate7;
			XamlServiceProvider xamlServiceProvider10 = new XamlServiceProvider();
			Type typeFromHandle19 = typeof(IProvideValueTarget);
			object[] array10 = new object[0 + 5];
			array10[0] = label5;
			array10[1] = stackLayout;
			array10[2] = stackLayout3;
			array10[3] = scrollView;
			array10[4] = this;
			object obj18;
			xamlServiceProvider10.Add(typeFromHandle19, obj18 = new SimpleValueTargetProvider(array10, Label.TextProperty, nameScope));
			xamlServiceProvider10.Add(typeof(IReferenceProvider), obj18);
			Type typeFromHandle20 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver10 = new XmlNamespaceResolver();
			xmlNamespaceResolver10.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver10.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver10.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver10.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver10.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider10.Add(typeFromHandle20, new XamlTypeResolver(xmlNamespaceResolver10, typeof(SettingsFuelCalibrationPage).GetTypeInfo().Assembly));
			xamlServiceProvider10.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(59, 28)));
			object obj19 = markupExtension10.ProvideValue(xamlServiceProvider10);
			label5.Text = obj19;
			stackLayout.Children.Add(label5);
			label6.SetValue(Label.FontAttributesProperty, new FontAttributesConverter().ConvertFromInvariantString("Bold"));
			label6.SetValue(View.HorizontalOptionsProperty, LayoutOptions.Center);
			label6.SetValue(Label.HorizontalTextAlignmentProperty, new TextAlignmentConverter().ConvertFromInvariantString("Center"));
			bindingExtension4.Path = "FuelFlowCorrectionFactor";
			BindingBase bindingBase4 = bindingExtension4.ProvideValue(null);
			label6.SetBinding(Label.TextProperty, bindingBase4);
			stackLayout.Children.Add(label6);
			grid2.SetValue(VisualElement.IsVisibleProperty, new VisualElement.VisibilityConverter().ConvertFromInvariantString("False"));
			columnDefinition3.SetValue(ColumnDefinition.WidthProperty, new GridLengthTypeConverter().ConvertFromInvariantString("*"));
			grid2.GetValue(Grid.ColumnDefinitionsProperty).Add(columnDefinition3);
			columnDefinition4.SetValue(ColumnDefinition.WidthProperty, new GridLengthTypeConverter().ConvertFromInvariantString("auto"));
			grid2.GetValue(Grid.ColumnDefinitionsProperty).Add(columnDefinition4);
			numericEntryV.SetValue(Grid.ColumnProperty, 0);
			numericEntryV.SetValue(NumericEntryV3.DoubleFormatProperty, "0.########");
			numericEntryV.SetValue(NumericEntryV3.MinimumProperty, 1E-08);
			bindingExtension5.Mode = 1;
			bindingExtension5.Path = "FuelFlowCorrectionFactor";
			BindingBase bindingBase5 = bindingExtension5.ProvideValue(null);
			numericEntryV.SetBinding(NumericEntryV3.ValueProperty, bindingBase5);
			grid2.Children.Add(numericEntryV);
			button2.SetValue(Grid.ColumnProperty, 1);
			button2.Clicked += this.btnOK_ManualCalibration_Clicked;
			button2.SetValue(Button.TextProperty, "OK");
			grid2.Children.Add(button2);
			stackLayout.Children.Add(grid2);
			button3.Clicked += this.btnFuelCalibrationManual_Clicked;
			translate8.Text = "ios_FuelCalibrationManual";
			IMarkupExtension markupExtension11 = translate8;
			XamlServiceProvider xamlServiceProvider11 = new XamlServiceProvider();
			Type typeFromHandle21 = typeof(IProvideValueTarget);
			object[] array11 = new object[0 + 5];
			array11[0] = button3;
			array11[1] = stackLayout;
			array11[2] = stackLayout3;
			array11[3] = scrollView;
			array11[4] = this;
			object obj20;
			xamlServiceProvider11.Add(typeFromHandle21, obj20 = new SimpleValueTargetProvider(array11, Button.TextProperty, nameScope));
			xamlServiceProvider11.Add(typeof(IReferenceProvider), obj20);
			Type typeFromHandle22 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver11 = new XmlNamespaceResolver();
			xmlNamespaceResolver11.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver11.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver11.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver11.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver11.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider11.Add(typeFromHandle22, new XamlTypeResolver(xmlNamespaceResolver11, typeof(SettingsFuelCalibrationPage).GetTypeInfo().Assembly));
			xamlServiceProvider11.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(87, 25)));
			object obj21 = markupExtension11.ProvideValue(xamlServiceProvider11);
			button3.Text = obj21;
			stackLayout.Children.Add(button3);
			button4.Clicked += this.btnResetCalibration_Clicked;
			translate9.Text = "Settings_Control_btnResetCalibration.Content";
			IMarkupExtension markupExtension12 = translate9;
			XamlServiceProvider xamlServiceProvider12 = new XamlServiceProvider();
			Type typeFromHandle23 = typeof(IProvideValueTarget);
			object[] array12 = new object[0 + 5];
			array12[0] = button4;
			array12[1] = stackLayout;
			array12[2] = stackLayout3;
			array12[3] = scrollView;
			array12[4] = this;
			object obj22;
			xamlServiceProvider12.Add(typeFromHandle23, obj22 = new SimpleValueTargetProvider(array12, Button.TextProperty, nameScope));
			xamlServiceProvider12.Add(typeof(IReferenceProvider), obj22);
			Type typeFromHandle24 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver12 = new XmlNamespaceResolver();
			xmlNamespaceResolver12.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver12.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver12.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver12.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver12.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider12.Add(typeFromHandle24, new XamlTypeResolver(xmlNamespaceResolver12, typeof(SettingsFuelCalibrationPage).GetTypeInfo().Assembly));
			xamlServiceProvider12.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(91, 25)));
			object obj23 = markupExtension12.ProvideValue(xamlServiceProvider12);
			button4.Text = obj23;
			stackLayout.Children.Add(button4);
			stackLayout3.Children.Add(stackLayout);
			bindingExtension6.Path = "IsCalibrationStartTimeSet";
			BindingBase bindingBase6 = bindingExtension6.ProvideValue(null);
			stackLayout2.SetBinding(VisualElement.IsVisibleProperty, bindingBase6);
			stackLayout2.SetValue(StackLayout.OrientationProperty, 0);
			translate10.Text = "Settings_Control_tbReadyToFinishCalibration.Text";
			IMarkupExtension markupExtension13 = translate10;
			XamlServiceProvider xamlServiceProvider13 = new XamlServiceProvider();
			Type typeFromHandle25 = typeof(IProvideValueTarget);
			object[] array13 = new object[0 + 5];
			array13[0] = label7;
			array13[1] = stackLayout2;
			array13[2] = stackLayout3;
			array13[3] = scrollView;
			array13[4] = this;
			object obj24;
			xamlServiceProvider13.Add(typeFromHandle25, obj24 = new SimpleValueTargetProvider(array13, Label.TextProperty, nameScope));
			xamlServiceProvider13.Add(typeof(IReferenceProvider), obj24);
			Type typeFromHandle26 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver13 = new XmlNamespaceResolver();
			xmlNamespaceResolver13.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver13.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver13.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver13.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver13.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider13.Add(typeFromHandle26, new XamlTypeResolver(xmlNamespaceResolver13, typeof(SettingsFuelCalibrationPage).GetTypeInfo().Assembly));
			xamlServiceProvider13.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(99, 28)));
			object obj25 = markupExtension13.ProvideValue(xamlServiceProvider13);
			label7.Text = obj25;
			stackLayout2.Children.Add(label7);
			translate11.Text = "Settings_Control_tbCalibrationFinishFuel.Text";
			IMarkupExtension markupExtension14 = translate11;
			XamlServiceProvider xamlServiceProvider14 = new XamlServiceProvider();
			Type typeFromHandle27 = typeof(IProvideValueTarget);
			object[] array14 = new object[0 + 5];
			array14[0] = label8;
			array14[1] = stackLayout2;
			array14[2] = stackLayout3;
			array14[3] = scrollView;
			array14[4] = this;
			object obj26;
			xamlServiceProvider14.Add(typeFromHandle27, obj26 = new SimpleValueTargetProvider(array14, Label.TextProperty, nameScope));
			xamlServiceProvider14.Add(typeof(IReferenceProvider), obj26);
			Type typeFromHandle28 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver14 = new XmlNamespaceResolver();
			xmlNamespaceResolver14.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver14.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver14.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver14.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver14.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider14.Add(typeFromHandle28, new XamlTypeResolver(xmlNamespaceResolver14, typeof(SettingsFuelCalibrationPage).GetTypeInfo().Assembly));
			xamlServiceProvider14.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(100, 28)));
			object obj27 = markupExtension14.ProvideValue(xamlServiceProvider14);
			label8.Text = obj27;
			stackLayout2.Children.Add(label8);
			columnDefinition5.SetValue(ColumnDefinition.WidthProperty, new GridLengthTypeConverter().ConvertFromInvariantString("*"));
			grid3.GetValue(Grid.ColumnDefinitionsProperty).Add(columnDefinition5);
			columnDefinition6.SetValue(ColumnDefinition.WidthProperty, new GridLengthTypeConverter().ConvertFromInvariantString("auto"));
			grid3.GetValue(Grid.ColumnDefinitionsProperty).Add(columnDefinition6);
			entry2.SetValue(Grid.ColumnProperty, 0);
			entry2.SetValue(Entry.TextProperty, "0");
			entry2.TextChanged += this.numericEntry_TextChanged;
			grid3.Children.Add(entry2);
			label9.SetValue(Grid.ColumnProperty, 1);
			bindingExtension7.Mode = 2;
			bindingExtension7.Path = "Use_km";
			BindingBase bindingBase7 = bindingExtension7.ProvideValue(null);
			label9.SetBinding(VisualElement.IsVisibleProperty, bindingBase7);
			translate12.Text = "Settings_Control_tbLiters.Text";
			IMarkupExtension markupExtension15 = translate12;
			XamlServiceProvider xamlServiceProvider15 = new XamlServiceProvider();
			Type typeFromHandle29 = typeof(IProvideValueTarget);
			object[] array15 = new object[0 + 6];
			array15[0] = label9;
			array15[1] = grid3;
			array15[2] = stackLayout2;
			array15[3] = stackLayout3;
			array15[4] = scrollView;
			array15[5] = this;
			object obj28;
			xamlServiceProvider15.Add(typeFromHandle29, obj28 = new SimpleValueTargetProvider(array15, Label.TextProperty, nameScope));
			xamlServiceProvider15.Add(typeof(IReferenceProvider), obj28);
			Type typeFromHandle30 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver15 = new XmlNamespaceResolver();
			xmlNamespaceResolver15.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver15.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver15.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver15.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver15.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider15.Add(typeFromHandle30, new XamlTypeResolver(xmlNamespaceResolver15, typeof(SettingsFuelCalibrationPage).GetTypeInfo().Assembly));
			xamlServiceProvider15.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(114, 29)));
			object obj29 = markupExtension15.ProvideValue(xamlServiceProvider15);
			label9.Text = obj29;
			grid3.Children.Add(label9);
			label10.SetValue(Grid.ColumnProperty, 1);
			bindingExtension8.Mode = 2;
			staticResourceExtension3.Key = "BoolToNegativeConverter";
			IMarkupExtension markupExtension16 = staticResourceExtension3;
			XamlServiceProvider xamlServiceProvider16 = new XamlServiceProvider();
			Type typeFromHandle31 = typeof(IProvideValueTarget);
			object[] array16 = new object[0 + 7];
			array16[0] = bindingExtension8;
			array16[1] = label10;
			array16[2] = grid3;
			array16[3] = stackLayout2;
			array16[4] = stackLayout3;
			array16[5] = scrollView;
			array16[6] = this;
			object obj30;
			xamlServiceProvider16.Add(typeFromHandle31, obj30 = new SimpleValueTargetProvider(array16, typeof(BindingExtension).GetRuntimeProperty("Converter"), nameScope));
			xamlServiceProvider16.Add(typeof(IReferenceProvider), obj30);
			Type typeFromHandle32 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver16 = new XmlNamespaceResolver();
			xmlNamespaceResolver16.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver16.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver16.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver16.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver16.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider16.Add(typeFromHandle32, new XamlTypeResolver(xmlNamespaceResolver16, typeof(SettingsFuelCalibrationPage).GetTypeInfo().Assembly));
			xamlServiceProvider16.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(117, 29)));
			object obj31 = markupExtension16.ProvideValue(xamlServiceProvider16);
			bindingExtension8.Converter = obj31;
			bindingExtension8.Path = "Use_km";
			BindingBase bindingBase8 = bindingExtension8.ProvideValue(null);
			label10.SetBinding(VisualElement.IsVisibleProperty, bindingBase8);
			translate13.Text = "Settings_Control_tbGalons.Text";
			IMarkupExtension markupExtension17 = translate13;
			XamlServiceProvider xamlServiceProvider17 = new XamlServiceProvider();
			Type typeFromHandle33 = typeof(IProvideValueTarget);
			object[] array17 = new object[0 + 6];
			array17[0] = label10;
			array17[1] = grid3;
			array17[2] = stackLayout2;
			array17[3] = stackLayout3;
			array17[4] = scrollView;
			array17[5] = this;
			object obj32;
			xamlServiceProvider17.Add(typeFromHandle33, obj32 = new SimpleValueTargetProvider(array17, Label.TextProperty, nameScope));
			xamlServiceProvider17.Add(typeof(IReferenceProvider), obj32);
			Type typeFromHandle34 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver17 = new XmlNamespaceResolver();
			xmlNamespaceResolver17.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver17.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver17.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver17.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver17.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider17.Add(typeFromHandle34, new XamlTypeResolver(xmlNamespaceResolver17, typeof(SettingsFuelCalibrationPage).GetTypeInfo().Assembly));
			xamlServiceProvider17.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(118, 29)));
			object obj33 = markupExtension17.ProvideValue(xamlServiceProvider17);
			label10.Text = obj33;
			grid3.Children.Add(label10);
			stackLayout2.Children.Add(grid3);
			translate14.Text = "Settings_Control_tbOdoInput.Text";
			IMarkupExtension markupExtension18 = translate14;
			XamlServiceProvider xamlServiceProvider18 = new XamlServiceProvider();
			Type typeFromHandle35 = typeof(IProvideValueTarget);
			object[] array18 = new object[0 + 5];
			array18[0] = label11;
			array18[1] = stackLayout2;
			array18[2] = stackLayout3;
			array18[3] = scrollView;
			array18[4] = this;
			object obj34;
			xamlServiceProvider18.Add(typeFromHandle35, obj34 = new SimpleValueTargetProvider(array18, Label.TextProperty, nameScope));
			xamlServiceProvider18.Add(typeof(IReferenceProvider), obj34);
			Type typeFromHandle36 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver18 = new XmlNamespaceResolver();
			xmlNamespaceResolver18.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver18.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver18.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver18.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver18.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider18.Add(typeFromHandle36, new XamlTypeResolver(xmlNamespaceResolver18, typeof(SettingsFuelCalibrationPage).GetTypeInfo().Assembly));
			xamlServiceProvider18.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(121, 28)));
			object obj35 = markupExtension18.ProvideValue(xamlServiceProvider18);
			label11.Text = obj35;
			stackLayout2.Children.Add(label11);
			columnDefinition7.SetValue(ColumnDefinition.WidthProperty, new GridLengthTypeConverter().ConvertFromInvariantString("*"));
			grid4.GetValue(Grid.ColumnDefinitionsProperty).Add(columnDefinition7);
			columnDefinition8.SetValue(ColumnDefinition.WidthProperty, new GridLengthTypeConverter().ConvertFromInvariantString("auto"));
			grid4.GetValue(Grid.ColumnDefinitionsProperty).Add(columnDefinition8);
			entry3.SetValue(Grid.ColumnProperty, 0);
			entry3.SetValue(Entry.TextProperty, "0");
			entry3.TextChanged += this.numericEntry_TextChanged;
			grid4.Children.Add(entry3);
			label12.SetValue(Grid.ColumnProperty, 1);
			bindingExtension9.Mode = 2;
			bindingExtension9.Path = "Use_km";
			BindingBase bindingBase9 = bindingExtension9.ProvideValue(null);
			label12.SetBinding(VisualElement.IsVisibleProperty, bindingBase9);
			translate15.Text = "Settings_Control_tbKm.Text";
			IMarkupExtension markupExtension19 = translate15;
			XamlServiceProvider xamlServiceProvider19 = new XamlServiceProvider();
			Type typeFromHandle37 = typeof(IProvideValueTarget);
			object[] array19 = new object[0 + 6];
			array19[0] = label12;
			array19[1] = grid4;
			array19[2] = stackLayout2;
			array19[3] = stackLayout3;
			array19[4] = scrollView;
			array19[5] = this;
			object obj36;
			xamlServiceProvider19.Add(typeFromHandle37, obj36 = new SimpleValueTargetProvider(array19, Label.TextProperty, nameScope));
			xamlServiceProvider19.Add(typeof(IReferenceProvider), obj36);
			Type typeFromHandle38 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver19 = new XmlNamespaceResolver();
			xmlNamespaceResolver19.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver19.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver19.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver19.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver19.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider19.Add(typeFromHandle38, new XamlTypeResolver(xmlNamespaceResolver19, typeof(SettingsFuelCalibrationPage).GetTypeInfo().Assembly));
			xamlServiceProvider19.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(135, 29)));
			object obj37 = markupExtension19.ProvideValue(xamlServiceProvider19);
			label12.Text = obj37;
			grid4.Children.Add(label12);
			label13.SetValue(Grid.ColumnProperty, 1);
			bindingExtension10.Mode = 2;
			staticResourceExtension4.Key = "BoolToNegativeConverter";
			IMarkupExtension markupExtension20 = staticResourceExtension4;
			XamlServiceProvider xamlServiceProvider20 = new XamlServiceProvider();
			Type typeFromHandle39 = typeof(IProvideValueTarget);
			object[] array20 = new object[0 + 7];
			array20[0] = bindingExtension10;
			array20[1] = label13;
			array20[2] = grid4;
			array20[3] = stackLayout2;
			array20[4] = stackLayout3;
			array20[5] = scrollView;
			array20[6] = this;
			object obj38;
			xamlServiceProvider20.Add(typeFromHandle39, obj38 = new SimpleValueTargetProvider(array20, typeof(BindingExtension).GetRuntimeProperty("Converter"), nameScope));
			xamlServiceProvider20.Add(typeof(IReferenceProvider), obj38);
			Type typeFromHandle40 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver20 = new XmlNamespaceResolver();
			xmlNamespaceResolver20.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver20.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver20.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver20.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver20.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider20.Add(typeFromHandle40, new XamlTypeResolver(xmlNamespaceResolver20, typeof(SettingsFuelCalibrationPage).GetTypeInfo().Assembly));
			xamlServiceProvider20.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(138, 29)));
			object obj39 = markupExtension20.ProvideValue(xamlServiceProvider20);
			bindingExtension10.Converter = obj39;
			bindingExtension10.Path = "Use_km";
			BindingBase bindingBase10 = bindingExtension10.ProvideValue(null);
			label13.SetBinding(VisualElement.IsVisibleProperty, bindingBase10);
			translate16.Text = "Settings_Control_tbMiles.Text";
			IMarkupExtension markupExtension21 = translate16;
			XamlServiceProvider xamlServiceProvider21 = new XamlServiceProvider();
			Type typeFromHandle41 = typeof(IProvideValueTarget);
			object[] array21 = new object[0 + 6];
			array21[0] = label13;
			array21[1] = grid4;
			array21[2] = stackLayout2;
			array21[3] = stackLayout3;
			array21[4] = scrollView;
			array21[5] = this;
			object obj40;
			xamlServiceProvider21.Add(typeFromHandle41, obj40 = new SimpleValueTargetProvider(array21, Label.TextProperty, nameScope));
			xamlServiceProvider21.Add(typeof(IReferenceProvider), obj40);
			Type typeFromHandle42 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver21 = new XmlNamespaceResolver();
			xmlNamespaceResolver21.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver21.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver21.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver21.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver21.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider21.Add(typeFromHandle42, new XamlTypeResolver(xmlNamespaceResolver21, typeof(SettingsFuelCalibrationPage).GetTypeInfo().Assembly));
			xamlServiceProvider21.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(139, 29)));
			object obj41 = markupExtension21.ProvideValue(xamlServiceProvider21);
			label13.Text = obj41;
			grid4.Children.Add(label13);
			stackLayout2.Children.Add(grid4);
			button5.Clicked += this.btnFinishCalibration_Clicked;
			translate17.Text = "Settings_Control_btnFinishCalibration.Content";
			IMarkupExtension markupExtension22 = translate17;
			XamlServiceProvider xamlServiceProvider22 = new XamlServiceProvider();
			Type typeFromHandle43 = typeof(IProvideValueTarget);
			object[] array22 = new object[0 + 5];
			array22[0] = button5;
			array22[1] = stackLayout2;
			array22[2] = stackLayout3;
			array22[3] = scrollView;
			array22[4] = this;
			object obj42;
			xamlServiceProvider22.Add(typeFromHandle43, obj42 = new SimpleValueTargetProvider(array22, Button.TextProperty, nameScope));
			xamlServiceProvider22.Add(typeof(IReferenceProvider), obj42);
			Type typeFromHandle44 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver22 = new XmlNamespaceResolver();
			xmlNamespaceResolver22.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver22.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver22.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver22.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver22.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider22.Add(typeFromHandle44, new XamlTypeResolver(xmlNamespaceResolver22, typeof(SettingsFuelCalibrationPage).GetTypeInfo().Assembly));
			xamlServiceProvider22.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(144, 25)));
			object obj43 = markupExtension22.ProvideValue(xamlServiceProvider22);
			button5.Text = obj43;
			stackLayout2.Children.Add(button5);
			button6.Clicked += this.btnCancelCalibration_Clicked;
			translate18.Text = "Settings_Control_btnCancelCalibration.Content";
			IMarkupExtension markupExtension23 = translate18;
			XamlServiceProvider xamlServiceProvider23 = new XamlServiceProvider();
			Type typeFromHandle45 = typeof(IProvideValueTarget);
			object[] array23 = new object[0 + 5];
			array23[0] = button6;
			array23[1] = stackLayout2;
			array23[2] = stackLayout3;
			array23[3] = scrollView;
			array23[4] = this;
			object obj44;
			xamlServiceProvider23.Add(typeFromHandle45, obj44 = new SimpleValueTargetProvider(array23, Button.TextProperty, nameScope));
			xamlServiceProvider23.Add(typeof(IReferenceProvider), obj44);
			Type typeFromHandle46 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver23 = new XmlNamespaceResolver();
			xmlNamespaceResolver23.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver23.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver23.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver23.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver23.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider23.Add(typeFromHandle46, new XamlTypeResolver(xmlNamespaceResolver23, typeof(SettingsFuelCalibrationPage).GetTypeInfo().Assembly));
			xamlServiceProvider23.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(148, 25)));
			object obj45 = markupExtension23.ProvideValue(xamlServiceProvider23);
			button6.Text = obj45;
			stackLayout2.Children.Add(button6);
			stackLayout3.Children.Add(stackLayout2);
			scrollView.Content = stackLayout3;
			this.SetValue(ContentPage.ContentProperty, scrollView);
		}

		// Token: 0x06001598 RID: 5528 RVA: 0x00098CD0 File Offset: 0x00096ED0
		private void __InitComponentRuntime()
		{
			Extensions.LoadFromXaml<SettingsFuelCalibrationPage>(this, typeof(SettingsFuelCalibrationPage));
			this.panelBeforeCalibrationStarted = NameScopeExtensions.FindByName<StackLayout>(this, "panelBeforeCalibrationStarted");
			this.entryOdometerStart = NameScopeExtensions.FindByName<Entry>(this, "entryOdometerStart");
			this.btnStartCalibration = NameScopeExtensions.FindByName<Button>(this, "btnStartCalibration");
			this.labelCalibrationFactor = NameScopeExtensions.FindByName<Label>(this, "labelCalibrationFactor");
			this.panel_ManualCalibrationSet = NameScopeExtensions.FindByName<Grid>(this, "panel_ManualCalibrationSet");
			this.entryFuelCorrectionManual = NameScopeExtensions.FindByName<NumericEntryV3>(this, "entryFuelCorrectionManual");
			this.btnOK_ManualCalibration = NameScopeExtensions.FindByName<Button>(this, "btnOK_ManualCalibration");
			this.btnFuelCalibrationManual = NameScopeExtensions.FindByName<Button>(this, "btnFuelCalibrationManual");
			this.btnResetCalibration = NameScopeExtensions.FindByName<Button>(this, "btnResetCalibration");
			this.panelAfterCalibrationStarted = NameScopeExtensions.FindByName<StackLayout>(this, "panelAfterCalibrationStarted");
			this.entryFuelFilled = NameScopeExtensions.FindByName<Entry>(this, "entryFuelFilled");
			this.entryOdometerFinish = NameScopeExtensions.FindByName<Entry>(this, "entryOdometerFinish");
			this.btnFinishCalibration = NameScopeExtensions.FindByName<Button>(this, "btnFinishCalibration");
			this.btnCancelCalibration = NameScopeExtensions.FindByName<Button>(this, "btnCancelCalibration");
		}

		// Token: 0x04000606 RID: 1542
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private StackLayout panelBeforeCalibrationStarted;

		// Token: 0x04000607 RID: 1543
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private Entry entryOdometerStart;

		// Token: 0x04000608 RID: 1544
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private Button btnStartCalibration;

		// Token: 0x04000609 RID: 1545
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private Label labelCalibrationFactor;

		// Token: 0x0400060A RID: 1546
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private Grid panel_ManualCalibrationSet;

		// Token: 0x0400060B RID: 1547
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private NumericEntryV3 entryFuelCorrectionManual;

		// Token: 0x0400060C RID: 1548
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private Button btnOK_ManualCalibration;

		// Token: 0x0400060D RID: 1549
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private Button btnFuelCalibrationManual;

		// Token: 0x0400060E RID: 1550
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private Button btnResetCalibration;

		// Token: 0x0400060F RID: 1551
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private StackLayout panelAfterCalibrationStarted;

		// Token: 0x04000610 RID: 1552
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private Entry entryFuelFilled;

		// Token: 0x04000611 RID: 1553
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private Entry entryOdometerFinish;

		// Token: 0x04000612 RID: 1554
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private Button btnFinishCalibration;

		// Token: 0x04000613 RID: 1555
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private Button btnCancelCalibration;

		// Token: 0x0200017C RID: 380
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <Page_Disappearing>d__2 : IAsyncStateMachine
		{
			// Token: 0x06001599 RID: 5529 RVA: 0x00098DDC File Offset: 0x00096FDC
			void IAsyncStateMachine.MoveNext()
			{
				try
				{
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

			// Token: 0x0600159A RID: 5530 RVA: 0x00098E28 File Offset: 0x00097028
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x04000614 RID: 1556
			public int <>1__state;

			// Token: 0x04000615 RID: 1557
			public AsyncVoidMethodBuilder <>t__builder;
		}

		// Token: 0x0200017D RID: 381
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <btnFinishCalibration_Clicked>d__6 : IAsyncStateMachine
		{
			// Token: 0x0600159B RID: 5531 RVA: 0x00098E38 File Offset: 0x00097038
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				SettingsFuelCalibrationPage settingsFuelCalibrationPage = this;
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
						goto IL_01C6;
					}
					case 2:
					{
						TaskAwaiter taskAwaiter4;
						taskAwaiter3 = taskAwaiter4;
						taskAwaiter4 = default(TaskAwaiter);
						num2 = -1;
						goto IL_0276;
					}
					case 3:
						taskAwaiter5 = taskAwaiter2;
						taskAwaiter2 = default(TaskAwaiter<bool>);
						num2 = -1;
						goto IL_0347;
					case 4:
					{
						TaskAwaiter taskAwaiter4;
						taskAwaiter3 = taskAwaiter4;
						taskAwaiter4 = default(TaskAwaiter);
						num2 = -1;
						goto IL_0460;
					}
					default:
					{
						string text = settingsFuelCalibrationPage.entryFuelFilled.Text.Replace(".", CultureInfo.CurrentUICulture.NumberFormat.NumberDecimalSeparator).Replace(",", CultureInfo.CurrentUICulture.NumberFormat.NumberDecimalSeparator);
						fuel_vol = 0.0;
						if (!double.TryParse(text, out fuel_vol))
						{
							taskAwaiter3 = settingsFuelCalibrationPage.DisplayAlert(Translate.GetString("Calibration_IncorrectFuelTitle"), Translate.GetString("Calibration_IncorrectFuelText"), "OK").GetAwaiter();
							if (!taskAwaiter3.IsCompleted)
							{
								num2 = 0;
								TaskAwaiter taskAwaiter4 = taskAwaiter3;
								this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, SettingsFuelCalibrationPage.<btnFinishCalibration_Clicked>d__6>(ref taskAwaiter3, ref this);
								return;
							}
						}
						else
						{
							odo = 0.0;
							if (!double.TryParse(settingsFuelCalibrationPage.entryOdometerFinish.Text.Replace(".", CultureInfo.CurrentUICulture.NumberFormat.NumberDecimalSeparator).Replace(",", CultureInfo.CurrentUICulture.NumberFormat.NumberDecimalSeparator), out odo))
							{
								taskAwaiter3 = settingsFuelCalibrationPage.DisplayAlert(Translate.GetString("Calibration_IncorrectOdoTitle"), Translate.GetString("Calibration_IncorrectOdoText"), "OK").GetAwaiter();
								if (!taskAwaiter3.IsCompleted)
								{
									num2 = 1;
									TaskAwaiter taskAwaiter4 = taskAwaiter3;
									this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, SettingsFuelCalibrationPage.<btnFinishCalibration_Clicked>d__6>(ref taskAwaiter3, ref this);
									return;
								}
								goto IL_01C6;
							}
							else
							{
								if (!SharedSettings.Current.Use_km)
								{
									odo *= 1.609344;
								}
								if (odo <= SharedSettings.Current.CalibrationStartOdometer)
								{
									taskAwaiter3 = settingsFuelCalibrationPage.DisplayAlert(Translate.GetString("Calibration_IncorrectOdoTitle"), Translate.GetString("Calibration_IncorrectOdoText"), "OK").GetAwaiter();
									if (!taskAwaiter3.IsCompleted)
									{
										num2 = 2;
										TaskAwaiter taskAwaiter4 = taskAwaiter3;
										this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, SettingsFuelCalibrationPage.<btnFinishCalibration_Clicked>d__6>(ref taskAwaiter3, ref this);
										return;
									}
									goto IL_0276;
								}
								else
								{
									if (!SharedSettings.Current.UseLitersForVolume)
									{
										if (SharedSettings.Current.UseUSGallon)
										{
											fuel_vol *= 3.78541;
										}
										else
										{
											fuel_vol *= 4.54609;
										}
									}
									taskAwaiter5 = settingsFuelCalibrationPage.DisplayAlert(Translate.GetString("Calibration_FinishTitle"), Translate.GetString("Calibration_FinishText"), Translate.GetString("ios_Yes"), Translate.GetString("ios_No")).GetAwaiter();
									if (!taskAwaiter5.IsCompleted)
									{
										num2 = 3;
										taskAwaiter2 = taskAwaiter5;
										this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<bool>, SettingsFuelCalibrationPage.<btnFinishCalibration_Clicked>d__6>(ref taskAwaiter5, ref this);
										return;
									}
									goto IL_0347;
								}
							}
						}
						break;
					}
					}
					taskAwaiter3.GetResult();
					goto IL_04BE;
					IL_01C6:
					taskAwaiter3.GetResult();
					goto IL_04BE;
					IL_0276:
					taskAwaiter3.GetResult();
					goto IL_04BE;
					IL_0347:
					if (!taskAwaiter5.GetResult())
					{
						goto IL_04BE;
					}
					settingsFuelCalibrationPage.btnFinishCalibration.IsEnabled = false;
					DriveCycleViewModel driveCycleViewModel = DriveCycleViewModel.Current;
					driveCycleViewModel.CalculatePeriodForCalibration(SharedSettings.Current.CalibrationStartTime);
					double periodAvgFuelConsumption = driveCycleViewModel.PeriodAvgFuelConsumption;
					double num3 = fuel_vol / (odo - SharedSettings.Current.CalibrationStartOdometer) * 100.0;
					double num4 = Math.Abs(num3 / periodAvgFuelConsumption);
					if (!double.IsInfinity(num4) && !double.IsNaN(num4) && num4 > 0.0)
					{
						SharedSettings.Current.FuelFlowCorrectionFactor = num4;
						settingsFuelCalibrationPage.btnFinishCalibration.IsEnabled = true;
						SharedSettings.Current.CalibrationStartTime = DateTime.MinValue;
						SharedSettings.Current.CalibrationStartOdometer = 0.0;
						goto IL_04A3;
					}
					settingsFuelCalibrationPage.btnFinishCalibration.IsEnabled = true;
					string text2 = Translate.GetString("ios_CalibrationErrorText");
					text2 = string.Format(text2, num3, periodAvgFuelConsumption);
					taskAwaiter3 = settingsFuelCalibrationPage.DisplayAlert(Translate.GetString("ios_CalibrationErrorTitle"), text2, "OK").GetAwaiter();
					if (!taskAwaiter3.IsCompleted)
					{
						num2 = 4;
						TaskAwaiter taskAwaiter4 = taskAwaiter3;
						this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, SettingsFuelCalibrationPage.<btnFinishCalibration_Clicked>d__6>(ref taskAwaiter3, ref this);
						return;
					}
					IL_0460:
					taskAwaiter3.GetResult();
					IL_04A3:;
				}
				catch (Exception ex)
				{
					num2 = -2;
					this.<>t__builder.SetException(ex);
					return;
				}
				IL_04BE:
				num2 = -2;
				this.<>t__builder.SetResult();
			}

			// Token: 0x0600159C RID: 5532 RVA: 0x00099334 File Offset: 0x00097534
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x04000616 RID: 1558
			public int <>1__state;

			// Token: 0x04000617 RID: 1559
			public AsyncVoidMethodBuilder <>t__builder;

			// Token: 0x04000618 RID: 1560
			public SettingsFuelCalibrationPage <>4__this;

			// Token: 0x04000619 RID: 1561
			private double <fuel_vol>5__2;

			// Token: 0x0400061A RID: 1562
			private double <odo>5__3;

			// Token: 0x0400061B RID: 1563
			private TaskAwaiter <>u__1;

			// Token: 0x0400061C RID: 1564
			private TaskAwaiter<bool> <>u__2;
		}

		// Token: 0x0200017E RID: 382
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <btnResetCalibration_Clicked>d__5 : IAsyncStateMachine
		{
			// Token: 0x0600159D RID: 5533 RVA: 0x00099344 File Offset: 0x00097544
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				SettingsFuelCalibrationPage settingsFuelCalibrationPage = this;
				try
				{
					TaskAwaiter<bool> taskAwaiter3;
					if (num != 0)
					{
						taskAwaiter3 = settingsFuelCalibrationPage.DisplayAlert(Translate.GetString("ios_ResetCalibrationTitle"), Translate.GetString("ios_ResetCalibrationText"), "OK", Translate.GetString("btnCancel.Content")).GetAwaiter();
						if (!taskAwaiter3.IsCompleted)
						{
							num2 = 0;
							taskAwaiter2 = taskAwaiter3;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<bool>, SettingsFuelCalibrationPage.<btnResetCalibration_Clicked>d__5>(ref taskAwaiter3, ref this);
							return;
						}
					}
					else
					{
						taskAwaiter3 = taskAwaiter2;
						taskAwaiter2 = default(TaskAwaiter<bool>);
						num2 = -1;
					}
					if (taskAwaiter3.GetResult())
					{
						SharedSettings.Current.FuelFlowCorrectionFactor = 1.0;
					}
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

			// Token: 0x0600159E RID: 5534 RVA: 0x00099430 File Offset: 0x00097630
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x0400061D RID: 1565
			public int <>1__state;

			// Token: 0x0400061E RID: 1566
			public AsyncVoidMethodBuilder <>t__builder;

			// Token: 0x0400061F RID: 1567
			public SettingsFuelCalibrationPage <>4__this;

			// Token: 0x04000620 RID: 1568
			private TaskAwaiter<bool> <>u__1;
		}

		// Token: 0x0200017F RID: 383
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <btnStartCalibration_Clicked>d__4 : IAsyncStateMachine
		{
			// Token: 0x0600159F RID: 5535 RVA: 0x00099440 File Offset: 0x00097640
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				SettingsFuelCalibrationPage settingsFuelCalibrationPage = this;
				try
				{
					TaskAwaiter taskAwaiter3;
					TaskAwaiter<bool> taskAwaiter5;
					TaskAwaiter<Page> taskAwaiter6;
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
						taskAwaiter5 = taskAwaiter2;
						taskAwaiter2 = default(TaskAwaiter<bool>);
						num2 = -1;
						goto IL_01CC;
					case 2:
						taskAwaiter5 = taskAwaiter2;
						taskAwaiter2 = default(TaskAwaiter<bool>);
						num2 = -1;
						goto IL_027A;
					case 3:
					{
						TaskAwaiter taskAwaiter4;
						taskAwaiter3 = taskAwaiter4;
						taskAwaiter4 = default(TaskAwaiter);
						num2 = -1;
						goto IL_0310;
					}
					case 4:
					{
						TaskAwaiter<Page> taskAwaiter7;
						taskAwaiter6 = taskAwaiter7;
						taskAwaiter7 = default(TaskAwaiter<Page>);
						num2 = -1;
						goto IL_0370;
					}
					default:
					{
						int num3 = 0;
						if (!int.TryParse(settingsFuelCalibrationPage.entryOdometerStart.Text, out num3) || num3 <= 0)
						{
							taskAwaiter3 = settingsFuelCalibrationPage.DisplayAlert(Translate.GetString("Calibration_IncorrectOdoTitle"), Translate.GetString("Calibration_IncorrectOdoText"), "OK").GetAwaiter();
							if (!taskAwaiter3.IsCompleted)
							{
								num2 = 0;
								TaskAwaiter taskAwaiter4 = taskAwaiter3;
								this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, SettingsFuelCalibrationPage.<btnStartCalibration_Clicked>d__4>(ref taskAwaiter3, ref this);
								return;
							}
						}
						else
						{
							double num4 = (double)num3;
							if (!SharedSettings.Current.Use_km)
							{
								num4 *= 1.609344;
							}
							SharedSettings.Current.CalibrationStartOdometer = num4;
							settingsFuelCalibrationPage.entryOdometerFinish.Text = SharedSettings.Current.CalibrationStartOdometer.ToString(CultureInfo.InvariantCulture);
							string text = Translate.GetString("CalibrationJustFilledFuelTankFull");
							if (SharedSettings.Current.FuelFlowCorrectionFactor > 1.0 || SharedSettings.Current.FuelFlowCorrectionFactor < 1.0)
							{
								text = text + "\r\n" + Translate.GetString("CalibrationStartResetCurrent");
							}
							taskAwaiter5 = settingsFuelCalibrationPage.DisplayAlert(Translate.GetString("CalibrationStartQuestion"), text, Translate.GetString("ios_Yes"), Translate.GetString("ios_No")).GetAwaiter();
							if (!taskAwaiter5.IsCompleted)
							{
								num2 = 1;
								taskAwaiter2 = taskAwaiter5;
								this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<bool>, SettingsFuelCalibrationPage.<btnStartCalibration_Clicked>d__4>(ref taskAwaiter5, ref this);
								return;
							}
							goto IL_01CC;
						}
						break;
					}
					}
					taskAwaiter3.GetResult();
					goto IL_0393;
					IL_01CC:
					if (!taskAwaiter5.GetResult())
					{
						goto IL_0378;
					}
					SharedSettings.Current.CalibrationStartTime = DateTimeNowHelper.NowSafe;
					DriveCycle.SaveAndReset();
					if (SharedSettings.Current.AlwaysRecordFuelConsumption)
					{
						goto IL_028E;
					}
					taskAwaiter5 = settingsFuelCalibrationPage.DisplayAlert(Translate.GetString("Calibration_RecomendationAlwayRecordFuelConsumptionTitle"), Translate.GetString("Calibration_RecomendationAlwayRecordFuelConsumptionText"), Translate.GetString("ios_Yes"), Translate.GetString("ios_No")).GetAwaiter();
					if (!taskAwaiter5.IsCompleted)
					{
						num2 = 2;
						taskAwaiter2 = taskAwaiter5;
						this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<bool>, SettingsFuelCalibrationPage.<btnStartCalibration_Clicked>d__4>(ref taskAwaiter5, ref this);
						return;
					}
					IL_027A:
					if (taskAwaiter5.GetResult())
					{
						SharedSettings.Current.AlwaysRecordFuelConsumption = true;
					}
					IL_028E:
					if (App.OBDReader.CurrentStatus == OBDDataReaderStatus.ConnectedToECU)
					{
						RequestProducerStatic.UpdateOBDReaderRequests();
					}
					taskAwaiter3 = settingsFuelCalibrationPage.DisplayAlert(Translate.GetString("Calibration_StartedMessageTitle"), Translate.GetString("Settings_Control_tbReadyToFinishCalibration.Text"), "OK").GetAwaiter();
					if (!taskAwaiter3.IsCompleted)
					{
						num2 = 3;
						TaskAwaiter taskAwaiter4 = taskAwaiter3;
						this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, SettingsFuelCalibrationPage.<btnStartCalibration_Clicked>d__4>(ref taskAwaiter3, ref this);
						return;
					}
					IL_0310:
					taskAwaiter3.GetResult();
					taskAwaiter6 = settingsFuelCalibrationPage.Navigation.PopAsync().GetAwaiter();
					if (!taskAwaiter6.IsCompleted)
					{
						num2 = 4;
						TaskAwaiter<Page> taskAwaiter7 = taskAwaiter6;
						this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<Page>, SettingsFuelCalibrationPage.<btnStartCalibration_Clicked>d__4>(ref taskAwaiter6, ref this);
						return;
					}
					IL_0370:
					taskAwaiter6.GetResult();
					IL_0378:;
				}
				catch (Exception ex)
				{
					num2 = -2;
					this.<>t__builder.SetException(ex);
					return;
				}
				IL_0393:
				num2 = -2;
				this.<>t__builder.SetResult();
			}

			// Token: 0x060015A0 RID: 5536 RVA: 0x00099810 File Offset: 0x00097A10
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x04000621 RID: 1569
			public int <>1__state;

			// Token: 0x04000622 RID: 1570
			public AsyncVoidMethodBuilder <>t__builder;

			// Token: 0x04000623 RID: 1571
			public SettingsFuelCalibrationPage <>4__this;

			// Token: 0x04000624 RID: 1572
			private TaskAwaiter <>u__1;

			// Token: 0x04000625 RID: 1573
			private TaskAwaiter<bool> <>u__2;

			// Token: 0x04000626 RID: 1574
			private TaskAwaiter<Page> <>u__3;
		}
	}
}
