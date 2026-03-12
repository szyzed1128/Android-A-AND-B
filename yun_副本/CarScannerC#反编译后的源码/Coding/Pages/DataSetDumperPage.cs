using System;
using System.CodeDom.Compiler;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using CarScannerXamarinForms.Coding.DB;
using CarScannerXamarinForms.Common;
using CarScannerXamarinForms.Common.XAMLConverters;
using CarScannerXamarinForms.InApp;
using CarScannerXamarinForms.OBD2;
using CarScannerXamarinForms.Settings;
using CarScannerXamarinForms.UserControls;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Internals;
using Xamarin.Forms.PlatformConfiguration.iOSSpecific;
using Xamarin.Forms.Xaml;
using Xamarin.Forms.Xaml.Diagnostics;
using Xamarin.Forms.Xaml.Internals;

namespace CarScannerXamarinForms.Coding.Pages
{
	// Token: 0x0200093D RID: 2365
	[XamlCompilation(2)]
	[XamlFilePath("Coding\\Pages\\DataSetDumperPage.xaml")]
	public class DataSetDumperPage : ContentPage
	{
		// Token: 0x06004E53 RID: 20051 RVA: 0x003B1698 File Offset: 0x003AF898
		internal DataSetDumperPage(MQBParametrizeBase coding)
		{
			this.InitializeComponent();
			this.SetPickerItems();
			this.panelDebug.IsVisible = SharedSettings.Current.DeveloperMode;
			this.coding = coding;
			base.BindingContext = this.coding;
			base.Appearing += this.CodingDetailsPage_Appearing;
			try
			{
				if (string.IsNullOrEmpty(coding.RequestHeader) && coding.RequestHeader.Length == 3)
				{
					string unitIdFromRequestHeader = VagUnitHelper.GetUnitIdFromRequestHeader(coding.RequestHeader);
					this.unitPicker.SelectedItem = unitIdFromRequestHeader;
				}
			}
			catch (Exception)
			{
			}
		}

		// Token: 0x06004E54 RID: 20052 RVA: 0x003B1740 File Offset: 0x003AF940
		private async void CodingDetailsPage_Appearing(object sender, EventArgs e)
		{
			if (this.first_appearing)
			{
				this.first_appearing = false;
				this.activityFrame.IsVisible = true;
				this.entryValue.IsEnabled = false;
				Progress<string> progress = new Progress<string>(delegate(string s)
				{
					MainThread.BeginInvokeOnMainThread(delegate
					{
						this.activityFrame.Text = ActivityFrame.PLEASE_WAIT_TEXT + "\n" + s;
					});
				});
				await this.coding.UpdateCurrentState("", progress);
				this.activityFrame.IsVisible = false;
				this.entryValue.IsEnabled = true;
			}
		}

		// Token: 0x06004E55 RID: 20053 RVA: 0x003B1778 File Offset: 0x003AF978
		public async Task UpdateState()
		{
			this.activityFrame.IsVisible = true;
			this.entryValue.IsEnabled = false;
			Progress<string> progress = new Progress<string>(delegate(string s)
			{
				MainThread.BeginInvokeOnMainThread(delegate
				{
					this.activityFrame.Text = ActivityFrame.PLEASE_WAIT_TEXT + "\n" + s;
				});
			});
			await this.coding.UpdateCurrentState(this.coding.Password, progress);
			this.activityFrame.IsVisible = false;
			this.entryValue.IsEnabled = true;
		}

		// Token: 0x06004E56 RID: 20054 RVA: 0x003B17BB File Offset: 0x003AF9BB
		private void btnReqToRes_Clicked(object sender, EventArgs e)
		{
			this.coding.ResponseHeader = CAN11bitHelper.GetPossibleResponseHeader(this.coding.RequestHeader, "Audi", null);
		}

		// Token: 0x06004E57 RID: 20055 RVA: 0x003B17E0 File Offset: 0x003AF9E0
		private async void BtnUpdateState_Clicked(object sender, EventArgs e)
		{
			this.UpdateState();
		}

		// Token: 0x06004E58 RID: 20056 RVA: 0x003B1818 File Offset: 0x003AFA18
		private async void btnSaveState_Clicked(object sender, EventArgs e)
		{
			string text = this.entryValue.Text;
			text = text.Replace("0x", "").Replace("0X", "").Replace(",", "")
				.Replace(" ", "")
				.Trim();
			int num = 0;
			try
			{
				BitHelpers.ConvertHexToBytesX(text);
			}
			catch (Exception obj)
			{
				num = 1;
			}
			if (num == 1)
			{
				object obj;
				Exception ex = (Exception)obj;
				await base.DisplayAlert(Translate.GetString("coding_WrongInputFormatTitle"), Translate.GetString("coding_WrongInputFormatText") + "\n010203AABBCCDDEEFF", "OK");
			}
			else
			{
				byte[] array = null;
				try
				{
					array = BitHelpers.ConvertHexToBytesX(this.coding.CurrentState);
				}
				catch (Exception)
				{
				}
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
					CodingRequestResult codingRequestResult = await this.coding.Execute(this.entryPassword.Text, text, this.coding.CurrentState, progress, array, false);
					if (codingRequestResult == CodingRequestResult.Success)
					{
						SharedSettings sharedSettings = SharedSettings.Current;
						num = sharedSettings.CodingsCounter;
						sharedSettings.CodingsCounter = num + 1;
					}
					else
					{
						await base.DisplayAlert(Translate.GetString("coding_Error"), MQBAdaptationTemplate.CodingRequestResultToString(codingRequestResult), "OK");
					}
					await this.coding.UpdateCurrentState(this.coding.Password, progress);
					this.activityFrame.IsVisible = false;
					this.activityFrame.Text = ActivityFrame.PLEASE_WAIT_TEXT;
					this.entryValue.IsEnabled = true;
				}
			}
		}

		// Token: 0x06004E59 RID: 20057 RVA: 0x003B1850 File Offset: 0x003AFA50
		private async void btnExport_Clicked(object sender, EventArgs e)
		{
			if (!string.IsNullOrEmpty(this.coding.CurrentState))
			{
				try
				{
					await Share.RequestAsync(this.coding.CurrentState);
				}
				catch (Exception)
				{
				}
			}
		}

		// Token: 0x06004E5A RID: 20058 RVA: 0x003B1888 File Offset: 0x003AFA88
		private async void btnImport_Clicked(object sender, EventArgs e)
		{
			try
			{
				FileResult fileResult = await FilePicker.PickAsync(null);
				if (fileResult != null)
				{
					using (StreamReader streamReader = new StreamReader(fileResult.FullPath))
					{
						string text = streamReader.ReadToEnd();
						this.entryValue.Text = text;
					}
				}
			}
			catch (Exception)
			{
			}
		}

		// Token: 0x06004E5B RID: 20059 RVA: 0x003B18C0 File Offset: 0x003AFAC0
		private void SetPickerItems()
		{
			int[] array = new int[]
			{
				1, 2, 3, 4, 5, 6, 8, 9, 14, 16,
				17, 19, 20, 21, 22, 23, 24, 25, 27, 32,
				33, 34, 35, 37, 38, 40, 50, 52, 54, 55,
				60, 61, 66, 68, 71, 82, 85, 87, 81, 83,
				95, 101, 105, 108, 109, 111, 113, 117, 118, 119,
				127, 148, 149, 165, 187, 188, 183
			};
			this.unitPicker.ItemsSource = array.Select((int x) => x.ToString("X2")).ToArray<string>();
			this.unitPicker.SelectedItem = "5F";
		}

		// Token: 0x06004E5C RID: 20060 RVA: 0x003B1928 File Offset: 0x003AFB28
		private void UnitPicker_SelectedIndexChanged(object sender, EventArgs e)
		{
			string text = (string)this.unitPicker.SelectedItem;
			if (!string.IsNullOrEmpty(text) && this.coding != null)
			{
				string requestHeaderForMQBUnit = VagUnitHelper.GetRequestHeaderForMQBUnit(text);
				string responseHeaderForMQBUnit = VagUnitHelper.GetResponseHeaderForMQBUnit(text);
				this.coding.RequestHeader = requestHeaderForMQBUnit;
				this.coding.ResponseHeader = responseHeaderForMQBUnit;
			}
		}

		// Token: 0x06004E5D RID: 20061 RVA: 0x003B197C File Offset: 0x003AFB7C
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private void InitializeComponent()
		{
			if (ResourceLoader.IsEnabled && ResourceLoader.CanProvideContentFor(new ResourceLoader.ResourceLoadingQuery
			{
				AssemblyName = typeof(DataSetDumperPage).GetTypeInfo().Assembly.GetName(),
				ResourcePath = "Coding/Pages/DataSetDumperPage.xaml",
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
			VisualDiagnostics.RegisterSourceInfo(translate = new Translate(), new Uri("Coding\\Pages\\DataSetDumperPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 13, 5);
			DynamicResourceExtension dynamicResourceExtension;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension = new DynamicResourceExtension(), new Uri("Coding\\Pages\\DataSetDumperPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 16, 5);
			EmptyStringToFalseConverter emptyStringToFalseConverter;
			VisualDiagnostics.RegisterSourceInfo(emptyStringToFalseConverter = new EmptyStringToFalseConverter(), new Uri("Coding\\Pages\\DataSetDumperPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 20, 14);
			EmptyStringToTrueConverter emptyStringToTrueConverter;
			VisualDiagnostics.RegisterSourceInfo(emptyStringToTrueConverter = new EmptyStringToTrueConverter(), new Uri("Coding\\Pages\\DataSetDumperPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 21, 14);
			IntToHexConverter intToHexConverter;
			VisualDiagnostics.RegisterSourceInfo(intToHexConverter = new IntToHexConverter(), new Uri("Coding\\Pages\\DataSetDumperPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 22, 14);
			ResourceDictionary resourceDictionary;
			VisualDiagnostics.RegisterSourceInfo(resourceDictionary = new ResourceDictionary(), new Uri("Coding\\Pages\\DataSetDumperPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 19, 10);
			RowDefinition rowDefinition;
			VisualDiagnostics.RegisterSourceInfo(rowDefinition = new RowDefinition(), new Uri("Coding\\Pages\\DataSetDumperPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 29, 22);
			DynamicResourceExtension dynamicResourceExtension2;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension2 = new DynamicResourceExtension(), new Uri("Coding\\Pages\\DataSetDumperPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 34, 25);
			BindingExtension bindingExtension;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension = new BindingExtension(), new Uri("Coding\\Pages\\DataSetDumperPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 35, 25);
			Label label;
			VisualDiagnostics.RegisterSourceInfo(label = new Label(), new Uri("Coding\\Pages\\DataSetDumperPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 32, 22);
			StaticResourceExtension staticResourceExtension;
			VisualDiagnostics.RegisterSourceInfo(staticResourceExtension = new StaticResourceExtension(), new Uri("Coding\\Pages\\DataSetDumperPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 36, 28);
			BindingExtension bindingExtension2;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension2 = new BindingExtension(), new Uri("Coding\\Pages\\DataSetDumperPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 36, 28);
			BindingExtension bindingExtension3;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension3 = new BindingExtension(), new Uri("Coding\\Pages\\DataSetDumperPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 36, 118);
			Label label2;
			VisualDiagnostics.RegisterSourceInfo(label2 = new Label(), new Uri("Coding\\Pages\\DataSetDumperPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 36, 22);
			StaticResourceExtension staticResourceExtension2;
			VisualDiagnostics.RegisterSourceInfo(staticResourceExtension2 = new StaticResourceExtension(), new Uri("Coding\\Pages\\DataSetDumperPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 37, 28);
			BindingExtension bindingExtension4;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension4 = new BindingExtension(), new Uri("Coding\\Pages\\DataSetDumperPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 37, 28);
			BindingExtension bindingExtension5;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension5 = new BindingExtension(), new Uri("Coding\\Pages\\DataSetDumperPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 37, 136);
			Label label3;
			VisualDiagnostics.RegisterSourceInfo(label3 = new Label(), new Uri("Coding\\Pages\\DataSetDumperPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 37, 22);
			Translate translate2;
			VisualDiagnostics.RegisterSourceInfo(translate2 = new Translate(), new Uri("Coding\\Pages\\DataSetDumperPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 49, 28);
			Label label4;
			VisualDiagnostics.RegisterSourceInfo(label4 = new Label(), new Uri("Coding\\Pages\\DataSetDumperPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 49, 22);
			Translate translate3;
			VisualDiagnostics.RegisterSourceInfo(translate3 = new Translate(), new Uri("Coding\\Pages\\DataSetDumperPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 52, 25);
			BindingExtension bindingExtension6;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension6 = new BindingExtension(), new Uri("Coding\\Pages\\DataSetDumperPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 53, 25);
			Entry entry;
			VisualDiagnostics.RegisterSourceInfo(entry = new Entry(), new Uri("Coding\\Pages\\DataSetDumperPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 50, 22);
			StaticResourceExtension staticResourceExtension3;
			VisualDiagnostics.RegisterSourceInfo(staticResourceExtension3 = new StaticResourceExtension(), new Uri("Coding\\Pages\\DataSetDumperPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 54, 52);
			BindingExtension bindingExtension7;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension7 = new BindingExtension(), new Uri("Coding\\Pages\\DataSetDumperPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 54, 52);
			Translate translate4;
			VisualDiagnostics.RegisterSourceInfo(translate4 = new Translate(), new Uri("Coding\\Pages\\DataSetDumperPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 57, 39);
			Span span;
			VisualDiagnostics.RegisterSourceInfo(span = new Span(), new Uri("Coding\\Pages\\DataSetDumperPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 57, 34);
			BindingExtension bindingExtension8;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension8 = new BindingExtension(), new Uri("Coding\\Pages\\DataSetDumperPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 58, 39);
			Span span2;
			VisualDiagnostics.RegisterSourceInfo(span2 = new Span(), new Uri("Coding\\Pages\\DataSetDumperPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 58, 34);
			FormattedString formattedString;
			VisualDiagnostics.RegisterSourceInfo(formattedString = new FormattedString(), new Uri("Coding\\Pages\\DataSetDumperPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 56, 30);
			Label label5;
			VisualDiagnostics.RegisterSourceInfo(label5 = new Label(), new Uri("Coding\\Pages\\DataSetDumperPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 54, 22);
			StaticResourceExtension staticResourceExtension4;
			VisualDiagnostics.RegisterSourceInfo(staticResourceExtension4 = new StaticResourceExtension(), new Uri("Coding\\Pages\\DataSetDumperPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 64, 25);
			BindingExtension bindingExtension9;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension9 = new BindingExtension(), new Uri("Coding\\Pages\\DataSetDumperPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 64, 25);
			Translate translate5;
			VisualDiagnostics.RegisterSourceInfo(translate5 = new Translate(), new Uri("Coding\\Pages\\DataSetDumperPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 65, 25);
			Label label6;
			VisualDiagnostics.RegisterSourceInfo(label6 = new Label(), new Uri("Coding\\Pages\\DataSetDumperPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 62, 22);
			Label label7;
			VisualDiagnostics.RegisterSourceInfo(label7 = new Label(), new Uri("Coding\\Pages\\DataSetDumperPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 82, 26);
			BindingExtension bindingExtension10;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension10 = new BindingExtension(), new Uri("Coding\\Pages\\DataSetDumperPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 86, 29);
			NumericEntryV3 numericEntryV;
			VisualDiagnostics.RegisterSourceInfo(numericEntryV = new NumericEntryV3(), new Uri("Coding\\Pages\\DataSetDumperPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 83, 26);
			Label label8;
			VisualDiagnostics.RegisterSourceInfo(label8 = new Label(), new Uri("Coding\\Pages\\DataSetDumperPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 90, 26);
			BindingExtension bindingExtension11;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension11 = new BindingExtension(), new Uri("Coding\\Pages\\DataSetDumperPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 94, 29);
			NumericEntryV3 numericEntryV2;
			VisualDiagnostics.RegisterSourceInfo(numericEntryV2 = new NumericEntryV3(), new Uri("Coding\\Pages\\DataSetDumperPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 91, 26);
			StackLayout stackLayout;
			VisualDiagnostics.RegisterSourceInfo(stackLayout = new StackLayout(), new Uri("Coding\\Pages\\DataSetDumperPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 68, 22);
			Label label9;
			VisualDiagnostics.RegisterSourceInfo(label9 = new Label(), new Uri("Coding\\Pages\\DataSetDumperPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 98, 22);
			Picker picker;
			VisualDiagnostics.RegisterSourceInfo(picker = new Picker(), new Uri("Coding\\Pages\\DataSetDumperPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 99, 22);
			Label label10;
			VisualDiagnostics.RegisterSourceInfo(label10 = new Label(), new Uri("Coding\\Pages\\DataSetDumperPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 101, 22);
			StaticResourceExtension staticResourceExtension5;
			VisualDiagnostics.RegisterSourceInfo(staticResourceExtension5 = new StaticResourceExtension(), new Uri("Coding\\Pages\\DataSetDumperPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 102, 28);
			BindingExtension bindingExtension12;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension12 = new BindingExtension(), new Uri("Coding\\Pages\\DataSetDumperPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 102, 28);
			Entry entry2;
			VisualDiagnostics.RegisterSourceInfo(entry2 = new Entry(), new Uri("Coding\\Pages\\DataSetDumperPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 102, 22);
			Label label11;
			VisualDiagnostics.RegisterSourceInfo(label11 = new Label(), new Uri("Coding\\Pages\\DataSetDumperPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 103, 22);
			BindingExtension bindingExtension13;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension13 = new BindingExtension(), new Uri("Coding\\Pages\\DataSetDumperPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 107, 25);
			NumericEntryV3 numericEntryV3;
			VisualDiagnostics.RegisterSourceInfo(numericEntryV3 = new NumericEntryV3(), new Uri("Coding\\Pages\\DataSetDumperPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 104, 22);
			Button button;
			VisualDiagnostics.RegisterSourceInfo(button = new Button(), new Uri("Coding\\Pages\\DataSetDumperPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 113, 22);
			Translate translate6;
			VisualDiagnostics.RegisterSourceInfo(translate6 = new Translate(), new Uri("Coding\\Pages\\DataSetDumperPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 123, 39);
			Span span3;
			VisualDiagnostics.RegisterSourceInfo(span3 = new Span(), new Uri("Coding\\Pages\\DataSetDumperPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 123, 34);
			Span span4;
			VisualDiagnostics.RegisterSourceInfo(span4 = new Span(), new Uri("Coding\\Pages\\DataSetDumperPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 124, 34);
			BindingExtension bindingExtension14;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension14 = new BindingExtension(), new Uri("Coding\\Pages\\DataSetDumperPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 125, 61);
			Span span5;
			VisualDiagnostics.RegisterSourceInfo(span5 = new Span(), new Uri("Coding\\Pages\\DataSetDumperPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 125, 34);
			FormattedString formattedString2;
			VisualDiagnostics.RegisterSourceInfo(formattedString2 = new FormattedString(), new Uri("Coding\\Pages\\DataSetDumperPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 122, 30);
			Label label12;
			VisualDiagnostics.RegisterSourceInfo(label12 = new Label(), new Uri("Coding\\Pages\\DataSetDumperPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 117, 22);
			Button button2;
			VisualDiagnostics.RegisterSourceInfo(button2 = new Button(), new Uri("Coding\\Pages\\DataSetDumperPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 130, 22);
			Button button3;
			VisualDiagnostics.RegisterSourceInfo(button3 = new Button(), new Uri("Coding\\Pages\\DataSetDumperPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 135, 22);
			Button button4;
			VisualDiagnostics.RegisterSourceInfo(button4 = new Button(), new Uri("Coding\\Pages\\DataSetDumperPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 140, 22);
			BindingExtension bindingExtension15;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension15 = new BindingExtension(), new Uri("Coding\\Pages\\DataSetDumperPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 145, 48);
			Entry entry3;
			VisualDiagnostics.RegisterSourceInfo(entry3 = new Entry(), new Uri("Coding\\Pages\\DataSetDumperPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 145, 22);
			DynamicResourceExtension dynamicResourceExtension3;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension3 = new DynamicResourceExtension(), new Uri("Coding\\Pages\\DataSetDumperPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 150, 25);
			Translate translate7;
			VisualDiagnostics.RegisterSourceInfo(translate7 = new Translate(), new Uri("Coding\\Pages\\DataSetDumperPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 151, 25);
			Label label13;
			VisualDiagnostics.RegisterSourceInfo(label13 = new Label(), new Uri("Coding\\Pages\\DataSetDumperPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 148, 22);
			StackLayout stackLayout2;
			VisualDiagnostics.RegisterSourceInfo(stackLayout2 = new StackLayout(), new Uri("Coding\\Pages\\DataSetDumperPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 31, 18);
			ActivityFrame activityFrame;
			VisualDiagnostics.RegisterSourceInfo(activityFrame = new ActivityFrame(), new Uri("Coding\\Pages\\DataSetDumperPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 153, 18);
			Grid grid;
			VisualDiagnostics.RegisterSourceInfo(grid = new Grid(), new Uri("Coding\\Pages\\DataSetDumperPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 27, 14);
			ScrollView scrollView;
			VisualDiagnostics.RegisterSourceInfo(scrollView = new ScrollView(), new Uri("Coding\\Pages\\DataSetDumperPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 26, 10);
			VisualDiagnostics.RegisterSourceInfo(this, new Uri("Coding\\Pages\\DataSetDumperPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 2, 2);
			NameScope nameScope = NameScope.GetNameScope(this) ?? new NameScope();
			NameScope.SetNameScope(this, nameScope);
			nameScope.RegisterName("entryPassword", entry);
			if (entry.StyleId == null)
			{
				entry.StyleId = "entryPassword";
			}
			nameScope.RegisterName("panelDebug", stackLayout);
			if (stackLayout.StyleId == null)
			{
				stackLayout.StyleId = "panelDebug";
			}
			nameScope.RegisterName("unitPicker", picker);
			if (picker.StyleId == null)
			{
				picker.StyleId = "unitPicker";
			}
			nameScope.RegisterName("btnUpdateState", button);
			if (button.StyleId == null)
			{
				button.StyleId = "btnUpdateState";
			}
			nameScope.RegisterName("labelState", label12);
			if (label12.StyleId == null)
			{
				label12.StyleId = "labelState";
			}
			nameScope.RegisterName("btnExport", button2);
			if (button2.StyleId == null)
			{
				button2.StyleId = "btnExport";
			}
			nameScope.RegisterName("btnImport", button3);
			if (button3.StyleId == null)
			{
				button3.StyleId = "btnImport";
			}
			nameScope.RegisterName("btnSaveNewValue", button4);
			if (button4.StyleId == null)
			{
				button4.StyleId = "btnSaveNewValue";
			}
			nameScope.RegisterName("entryValue", entry3);
			if (entry3.StyleId == null)
			{
				entry3.StyleId = "entryValue";
			}
			nameScope.RegisterName("activityFrame", activityFrame);
			if (activityFrame.StyleId == null)
			{
				activityFrame.StyleId = "activityFrame";
			}
			this.entryPassword = entry;
			this.panelDebug = stackLayout;
			this.unitPicker = picker;
			this.btnUpdateState = button;
			this.labelState = label12;
			this.btnExport = button2;
			this.btnImport = button3;
			this.btnSaveNewValue = button4;
			this.entryValue = entry3;
			this.activityFrame = activityFrame;
			this.Resources = resourceDictionary;
			resourceDictionary.Add("EmptyStringToFalseConverter", emptyStringToFalseConverter);
			resourceDictionary.Add("EmptyStringToTrueConverter", emptyStringToTrueConverter);
			resourceDictionary.Add("IntToHexConverter", intToHexConverter);
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
			xmlNamespaceResolver.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider.Add(typeFromHandle2, new XamlTypeResolver(xmlNamespaceResolver, typeof(DataSetDumperPage).GetTypeInfo().Assembly));
			xamlServiceProvider.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(13, 5)));
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
			xmlNamespaceResolver2.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider2.Add(typeFromHandle4, new XamlTypeResolver(xmlNamespaceResolver2, typeof(DataSetDumperPage).GetTypeInfo().Assembly));
			xamlServiceProvider2.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(16, 5)));
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
			xmlNamespaceResolver3.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider3.Add(typeFromHandle6, new XamlTypeResolver(xmlNamespaceResolver3, typeof(DataSetDumperPage).GetTypeInfo().Assembly));
			xamlServiceProvider3.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(34, 25)));
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
			xmlNamespaceResolver4.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider4.Add(typeFromHandle8, new XamlTypeResolver(xmlNamespaceResolver4, typeof(DataSetDumperPage).GetTypeInfo().Assembly));
			xamlServiceProvider4.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(36, 28)));
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
			xmlNamespaceResolver5.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider5.Add(typeFromHandle10, new XamlTypeResolver(xmlNamespaceResolver5, typeof(DataSetDumperPage).GetTypeInfo().Assembly));
			xamlServiceProvider5.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(37, 28)));
			object obj8 = markupExtension5.ProvideValue(xamlServiceProvider5);
			bindingExtension4.Converter = obj8;
			bindingExtension4.Path = "InnerDescription";
			BindingBase bindingBase4 = bindingExtension4.ProvideValue(null);
			label3.SetBinding(VisualElement.IsVisibleProperty, bindingBase4);
			bindingExtension5.Path = "InnerDescription";
			BindingBase bindingBase5 = bindingExtension5.ProvideValue(null);
			label3.SetBinding(Label.TextProperty, bindingBase5);
			stackLayout2.Children.Add(label3);
			translate2.Text = "coding_Password";
			IMarkupExtension markupExtension6 = translate2;
			XamlServiceProvider xamlServiceProvider6 = new XamlServiceProvider();
			Type typeFromHandle11 = typeof(IProvideValueTarget);
			object[] array6 = new object[0 + 5];
			array6[0] = label4;
			array6[1] = stackLayout2;
			array6[2] = grid;
			array6[3] = scrollView;
			array6[4] = this;
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
			xmlNamespaceResolver6.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider6.Add(typeFromHandle12, new XamlTypeResolver(xmlNamespaceResolver6, typeof(DataSetDumperPage).GetTypeInfo().Assembly));
			xamlServiceProvider6.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(49, 28)));
			object obj10 = markupExtension6.ProvideValue(xamlServiceProvider6);
			label4.Text = obj10;
			stackLayout2.Children.Add(label4);
			translate3.Text = "coding_PasswordNotRequired";
			IMarkupExtension markupExtension7 = translate3;
			XamlServiceProvider xamlServiceProvider7 = new XamlServiceProvider();
			Type typeFromHandle13 = typeof(IProvideValueTarget);
			object[] array7 = new object[0 + 5];
			array7[0] = entry;
			array7[1] = stackLayout2;
			array7[2] = grid;
			array7[3] = scrollView;
			array7[4] = this;
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
			xmlNamespaceResolver7.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider7.Add(typeFromHandle14, new XamlTypeResolver(xmlNamespaceResolver7, typeof(DataSetDumperPage).GetTypeInfo().Assembly));
			xamlServiceProvider7.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(52, 25)));
			object obj12 = markupExtension7.ProvideValue(xamlServiceProvider7);
			entry.Placeholder = obj12;
			bindingExtension6.Mode = 1;
			bindingExtension6.Path = "Password";
			BindingBase bindingBase6 = bindingExtension6.ProvideValue(null);
			entry.SetBinding(Entry.TextProperty, bindingBase6);
			stackLayout2.Children.Add(entry);
			label5.SetValue(Label.FontAttributesProperty, new FontAttributesConverter().ConvertFromInvariantString("Italic"));
			staticResourceExtension3.Key = "EmptyStringToFalseConverter";
			IMarkupExtension markupExtension8 = staticResourceExtension3;
			XamlServiceProvider xamlServiceProvider8 = new XamlServiceProvider();
			Type typeFromHandle15 = typeof(IProvideValueTarget);
			object[] array8 = new object[0 + 6];
			array8[0] = bindingExtension7;
			array8[1] = label5;
			array8[2] = stackLayout2;
			array8[3] = grid;
			array8[4] = scrollView;
			array8[5] = this;
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
			xmlNamespaceResolver8.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider8.Add(typeFromHandle16, new XamlTypeResolver(xmlNamespaceResolver8, typeof(DataSetDumperPage).GetTypeInfo().Assembly));
			xamlServiceProvider8.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(54, 52)));
			object obj14 = markupExtension8.ProvideValue(xamlServiceProvider8);
			bindingExtension7.Converter = obj14;
			bindingExtension7.Path = "PasswordHint";
			BindingBase bindingBase7 = bindingExtension7.ProvideValue(null);
			label5.SetBinding(VisualElement.IsVisibleProperty, bindingBase7);
			translate4.Text = "coding_PasswordHint";
			IMarkupExtension markupExtension9 = translate4;
			XamlServiceProvider xamlServiceProvider9 = new XamlServiceProvider();
			Type typeFromHandle17 = typeof(IProvideValueTarget);
			object[] array9 = new object[0 + 7];
			array9[0] = span;
			array9[1] = formattedString;
			array9[2] = label5;
			array9[3] = stackLayout2;
			array9[4] = grid;
			array9[5] = scrollView;
			array9[6] = this;
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
			xmlNamespaceResolver9.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider9.Add(typeFromHandle18, new XamlTypeResolver(xmlNamespaceResolver9, typeof(DataSetDumperPage).GetTypeInfo().Assembly));
			xamlServiceProvider9.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(57, 39)));
			object obj16 = markupExtension9.ProvideValue(xamlServiceProvider9);
			span.Text = obj16;
			formattedString.Spans.Add(span);
			bindingExtension8.Path = "PasswordHint";
			BindingBase bindingBase8 = bindingExtension8.ProvideValue(null);
			span2.SetBinding(Span.TextProperty, bindingBase8);
			formattedString.Spans.Add(span2);
			label5.SetValue(Label.FormattedTextProperty, formattedString);
			stackLayout2.Children.Add(label5);
			label6.SetValue(Label.FontAttributesProperty, new FontAttributesConverter().ConvertFromInvariantString("Italic"));
			bindingExtension9.Mode = 2;
			staticResourceExtension4.Key = "EmptyStringToTrueConverter";
			IMarkupExtension markupExtension10 = staticResourceExtension4;
			XamlServiceProvider xamlServiceProvider10 = new XamlServiceProvider();
			Type typeFromHandle19 = typeof(IProvideValueTarget);
			object[] array10 = new object[0 + 6];
			array10[0] = bindingExtension9;
			array10[1] = label6;
			array10[2] = stackLayout2;
			array10[3] = grid;
			array10[4] = scrollView;
			array10[5] = this;
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
			xmlNamespaceResolver10.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider10.Add(typeFromHandle20, new XamlTypeResolver(xmlNamespaceResolver10, typeof(DataSetDumperPage).GetTypeInfo().Assembly));
			xamlServiceProvider10.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(64, 25)));
			object obj18 = markupExtension10.ProvideValue(xamlServiceProvider10);
			bindingExtension9.Converter = obj18;
			bindingExtension9.Path = "Password";
			BindingBase bindingBase9 = bindingExtension9.ProvideValue(null);
			label6.SetBinding(VisualElement.IsVisibleProperty, bindingBase9);
			translate5.Text = "coding_PasswordNotRequired";
			IMarkupExtension markupExtension11 = translate5;
			XamlServiceProvider xamlServiceProvider11 = new XamlServiceProvider();
			Type typeFromHandle21 = typeof(IProvideValueTarget);
			object[] array11 = new object[0 + 5];
			array11[0] = label6;
			array11[1] = stackLayout2;
			array11[2] = grid;
			array11[3] = scrollView;
			array11[4] = this;
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
			xmlNamespaceResolver11.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider11.Add(typeFromHandle22, new XamlTypeResolver(xmlNamespaceResolver11, typeof(DataSetDumperPage).GetTypeInfo().Assembly));
			xamlServiceProvider11.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(65, 25)));
			object obj20 = markupExtension11.ProvideValue(xamlServiceProvider11);
			label6.Text = obj20;
			stackLayout2.Children.Add(label6);
			stackLayout.SetValue(StackLayout.OrientationProperty, 0);
			label7.SetValue(Label.TextProperty, "Address format length bytes:");
			stackLayout.Children.Add(label7);
			numericEntryV.SetValue(NumericEntryV3.DoubleFormatProperty, "0");
			numericEntryV.SetValue(NumericEntryV3.MinimumProperty, 1.0);
			bindingExtension10.Mode = 1;
			bindingExtension10.Path = "AddressFormatLength";
			BindingBase bindingBase10 = bindingExtension10.ProvideValue(null);
			numericEntryV.SetBinding(NumericEntryV3.ValueProperty, bindingBase10);
			stackLayout.Children.Add(numericEntryV);
			label8.SetValue(Label.TextProperty, "Data length format bytes:");
			stackLayout.Children.Add(label8);
			numericEntryV2.SetValue(NumericEntryV3.DoubleFormatProperty, "0");
			numericEntryV2.SetValue(NumericEntryV3.MinimumProperty, 1.0);
			bindingExtension11.Mode = 1;
			bindingExtension11.Path = "DataLengthFormatLength";
			BindingBase bindingBase11 = bindingExtension11.ProvideValue(null);
			numericEntryV2.SetBinding(NumericEntryV3.ValueProperty, bindingBase11);
			stackLayout.Children.Add(numericEntryV2);
			stackLayout2.Children.Add(stackLayout);
			label9.SetValue(Label.TextProperty, "Unit:");
			stackLayout2.Children.Add(label9);
			picker.SelectedIndexChanged += this.UnitPicker_SelectedIndexChanged;
			stackLayout2.Children.Add(picker);
			label10.SetValue(Label.TextProperty, "Address (HEX):");
			stackLayout2.Children.Add(label10);
			bindingExtension12.Mode = 1;
			staticResourceExtension5.Key = "IntToHexConverter";
			IMarkupExtension markupExtension12 = staticResourceExtension5;
			XamlServiceProvider xamlServiceProvider12 = new XamlServiceProvider();
			Type typeFromHandle23 = typeof(IProvideValueTarget);
			object[] array12 = new object[0 + 6];
			array12[0] = bindingExtension12;
			array12[1] = entry2;
			array12[2] = stackLayout2;
			array12[3] = grid;
			array12[4] = scrollView;
			array12[5] = this;
			object obj21;
			xamlServiceProvider12.Add(typeFromHandle23, obj21 = new SimpleValueTargetProvider(array12, typeof(BindingExtension).GetRuntimeProperty("Converter"), nameScope));
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
			xmlNamespaceResolver12.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider12.Add(typeFromHandle24, new XamlTypeResolver(xmlNamespaceResolver12, typeof(DataSetDumperPage).GetTypeInfo().Assembly));
			xamlServiceProvider12.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(102, 28)));
			object obj22 = markupExtension12.ProvideValue(xamlServiceProvider12);
			bindingExtension12.Converter = obj22;
			bindingExtension12.Path = "Address";
			BindingBase bindingBase12 = bindingExtension12.ProvideValue(null);
			entry2.SetBinding(Entry.TextProperty, bindingBase12);
			stackLayout2.Children.Add(entry2);
			label11.SetValue(Label.TextProperty, "Data length bytes:");
			stackLayout2.Children.Add(label11);
			numericEntryV3.SetValue(NumericEntryV3.DoubleFormatProperty, "0");
			numericEntryV3.SetValue(NumericEntryV3.MinimumProperty, 1.0);
			bindingExtension13.Mode = 1;
			bindingExtension13.Path = "DataLength";
			BindingBase bindingBase13 = bindingExtension13.ProvideValue(null);
			numericEntryV3.SetBinding(NumericEntryV3.ValueProperty, bindingBase13);
			stackLayout2.Children.Add(numericEntryV3);
			button.Clicked += this.BtnUpdateState_Clicked;
			button.SetValue(Button.TextProperty, "Read");
			stackLayout2.Children.Add(button);
			label12.SetValue(Label.LineBreakModeProperty, 5);
			label12.SetValue(Label.MaxLinesProperty, 2);
			translate6.Text = "coding_CurrentState";
			IMarkupExtension markupExtension13 = translate6;
			XamlServiceProvider xamlServiceProvider13 = new XamlServiceProvider();
			Type typeFromHandle25 = typeof(IProvideValueTarget);
			object[] array13 = new object[0 + 7];
			array13[0] = span3;
			array13[1] = formattedString2;
			array13[2] = label12;
			array13[3] = stackLayout2;
			array13[4] = grid;
			array13[5] = scrollView;
			array13[6] = this;
			object obj23;
			xamlServiceProvider13.Add(typeFromHandle25, obj23 = new SimpleValueTargetProvider(array13, Span.TextProperty, nameScope));
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
			xmlNamespaceResolver13.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider13.Add(typeFromHandle26, new XamlTypeResolver(xmlNamespaceResolver13, typeof(DataSetDumperPage).GetTypeInfo().Assembly));
			xamlServiceProvider13.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(123, 39)));
			object obj24 = markupExtension13.ProvideValue(xamlServiceProvider13);
			span3.Text = obj24;
			formattedString2.Spans.Add(span3);
			span4.SetValue(Span.TextProperty, " ");
			formattedString2.Spans.Add(span4);
			span5.SetValue(Span.FontAttributesProperty, new FontAttributesConverter().ConvertFromInvariantString("Bold"));
			bindingExtension14.Mode = 2;
			bindingExtension14.Path = "CurrentState";
			BindingBase bindingBase14 = bindingExtension14.ProvideValue(null);
			span5.SetBinding(Span.TextProperty, bindingBase14);
			formattedString2.Spans.Add(span5);
			label12.SetValue(Label.FormattedTextProperty, formattedString2);
			stackLayout2.Children.Add(label12);
			button2.Clicked += this.btnExport_Clicked;
			button2.SetValue(Button.TextProperty, "Export");
			stackLayout2.Children.Add(button2);
			button3.Clicked += this.btnImport_Clicked;
			button3.SetValue(Button.TextProperty, "Import");
			stackLayout2.Children.Add(button3);
			button4.Clicked += this.btnSaveState_Clicked;
			button4.SetValue(Button.TextProperty, "Write");
			stackLayout2.Children.Add(button4);
			bindingExtension15.Mode = 2;
			bindingExtension15.Path = "CurrentState";
			BindingBase bindingBase15 = bindingExtension15.ProvideValue(null);
			entry3.SetBinding(Entry.TextProperty, bindingBase15);
			stackLayout2.Children.Add(entry3);
			label13.SetValue(Label.FontAttributesProperty, new FontAttributesConverter().ConvertFromInvariantString("Italic"));
			dynamicResourceExtension3.Key = "BaseFontSize-";
			IMarkupExtension<DynamicResource> markupExtension14 = dynamicResourceExtension3;
			XamlServiceProvider xamlServiceProvider14 = new XamlServiceProvider();
			Type typeFromHandle27 = typeof(IProvideValueTarget);
			object[] array14 = new object[0 + 5];
			array14[0] = label13;
			array14[1] = stackLayout2;
			array14[2] = grid;
			array14[3] = scrollView;
			array14[4] = this;
			object obj25;
			xamlServiceProvider14.Add(typeFromHandle27, obj25 = new SimpleValueTargetProvider(array14, Label.FontSizeProperty, nameScope));
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
			xmlNamespaceResolver14.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider14.Add(typeFromHandle28, new XamlTypeResolver(xmlNamespaceResolver14, typeof(DataSetDumperPage).GetTypeInfo().Assembly));
			xamlServiceProvider14.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(150, 25)));
			DynamicResource dynamicResource3 = markupExtension14.ProvideValue(xamlServiceProvider14);
			label13.SetDynamicResource(Label.FontSizeProperty, dynamicResource3.Key);
			translate7.Text = "coding_UseHistory";
			IMarkupExtension markupExtension15 = translate7;
			XamlServiceProvider xamlServiceProvider15 = new XamlServiceProvider();
			Type typeFromHandle29 = typeof(IProvideValueTarget);
			object[] array15 = new object[0 + 5];
			array15[0] = label13;
			array15[1] = stackLayout2;
			array15[2] = grid;
			array15[3] = scrollView;
			array15[4] = this;
			object obj26;
			xamlServiceProvider15.Add(typeFromHandle29, obj26 = new SimpleValueTargetProvider(array15, Label.TextProperty, nameScope));
			xamlServiceProvider15.Add(typeof(IReferenceProvider), obj26);
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
			xmlNamespaceResolver15.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider15.Add(typeFromHandle30, new XamlTypeResolver(xmlNamespaceResolver15, typeof(DataSetDumperPage).GetTypeInfo().Assembly));
			xamlServiceProvider15.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(151, 25)));
			object obj27 = markupExtension15.ProvideValue(xamlServiceProvider15);
			label13.Text = obj27;
			stackLayout2.Children.Add(label13);
			grid.Children.Add(stackLayout2);
			activityFrame.SetValue(Grid.RowProperty, 0);
			grid.Children.Add(activityFrame);
			scrollView.Content = grid;
			this.SetValue(ContentPage.ContentProperty, scrollView);
		}

		// Token: 0x06004E5E RID: 20062 RVA: 0x003B43AE File Offset: 0x003B25AE
		[CompilerGenerated]
		private void <CodingDetailsPage_Appearing>b__3_0(string s)
		{
			MainThread.BeginInvokeOnMainThread(delegate
			{
				this.activityFrame.Text = ActivityFrame.PLEASE_WAIT_TEXT + "\n" + s;
			});
		}

		// Token: 0x06004E5F RID: 20063 RVA: 0x003B43D3 File Offset: 0x003B25D3
		[CompilerGenerated]
		private void <UpdateState>b__4_0(string s)
		{
			MainThread.BeginInvokeOnMainThread(delegate
			{
				this.activityFrame.Text = ActivityFrame.PLEASE_WAIT_TEXT + "\n" + s;
			});
		}

		// Token: 0x06004E60 RID: 20064 RVA: 0x003B43F8 File Offset: 0x003B25F8
		[CompilerGenerated]
		private void <btnSaveState_Clicked>b__7_0(string s)
		{
			Device.BeginInvokeOnMainThread(delegate
			{
				this.activityFrame.Text = s;
			});
		}

		// Token: 0x06004E61 RID: 20065 RVA: 0x003B4420 File Offset: 0x003B2620
		private void __InitComponentRuntime()
		{
			Extensions.LoadFromXaml<DataSetDumperPage>(this, typeof(DataSetDumperPage));
			this.entryPassword = NameScopeExtensions.FindByName<Entry>(this, "entryPassword");
			this.panelDebug = NameScopeExtensions.FindByName<StackLayout>(this, "panelDebug");
			this.unitPicker = NameScopeExtensions.FindByName<Picker>(this, "unitPicker");
			this.btnUpdateState = NameScopeExtensions.FindByName<Button>(this, "btnUpdateState");
			this.labelState = NameScopeExtensions.FindByName<Label>(this, "labelState");
			this.btnExport = NameScopeExtensions.FindByName<Button>(this, "btnExport");
			this.btnImport = NameScopeExtensions.FindByName<Button>(this, "btnImport");
			this.btnSaveNewValue = NameScopeExtensions.FindByName<Button>(this, "btnSaveNewValue");
			this.entryValue = NameScopeExtensions.FindByName<Entry>(this, "entryValue");
			this.activityFrame = NameScopeExtensions.FindByName<ActivityFrame>(this, "activityFrame");
		}

		// Token: 0x04002EE6 RID: 12006
		private MQBParametrizeBase coding;

		// Token: 0x04002EE7 RID: 12007
		private bool first_appearing = true;

		// Token: 0x04002EE8 RID: 12008
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private Entry entryPassword;

		// Token: 0x04002EE9 RID: 12009
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private StackLayout panelDebug;

		// Token: 0x04002EEA RID: 12010
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private Picker unitPicker;

		// Token: 0x04002EEB RID: 12011
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private Button btnUpdateState;

		// Token: 0x04002EEC RID: 12012
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private Label labelState;

		// Token: 0x04002EED RID: 12013
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private Button btnExport;

		// Token: 0x04002EEE RID: 12014
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private Button btnImport;

		// Token: 0x04002EEF RID: 12015
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private Button btnSaveNewValue;

		// Token: 0x04002EF0 RID: 12016
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private Entry entryValue;

		// Token: 0x04002EF1 RID: 12017
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private ActivityFrame activityFrame;

		// Token: 0x0200093E RID: 2366
		[CompilerGenerated]
		[Serializable]
		private sealed class <>c
		{
			// Token: 0x06004E62 RID: 20066 RVA: 0x003B44E8 File Offset: 0x003B26E8
			// Note: this type is marked as 'beforefieldinit'.
			static <>c()
			{
			}

			// Token: 0x06004E63 RID: 20067 RVA: 0x00002050 File Offset: 0x00000250
			public <>c()
			{
			}

			// Token: 0x06004E64 RID: 20068 RVA: 0x002416BF File Offset: 0x0023F8BF
			internal string <SetPickerItems>b__10_0(int x)
			{
				return x.ToString("X2");
			}

			// Token: 0x04002EF2 RID: 12018
			public static readonly DataSetDumperPage.<>c <>9 = new DataSetDumperPage.<>c();

			// Token: 0x04002EF3 RID: 12019
			public static Func<int, string> <>9__10_0;
		}

		// Token: 0x0200093F RID: 2367
		[CompilerGenerated]
		private sealed class <>c__DisplayClass3_0
		{
			// Token: 0x06004E65 RID: 20069 RVA: 0x00002050 File Offset: 0x00000250
			public <>c__DisplayClass3_0()
			{
			}

			// Token: 0x06004E66 RID: 20070 RVA: 0x003B44F4 File Offset: 0x003B26F4
			internal void <CodingDetailsPage_Appearing>b__1()
			{
				this.<>4__this.activityFrame.Text = ActivityFrame.PLEASE_WAIT_TEXT + "\n" + this.s;
			}

			// Token: 0x04002EF4 RID: 12020
			public string s;

			// Token: 0x04002EF5 RID: 12021
			public DataSetDumperPage <>4__this;
		}

		// Token: 0x02000940 RID: 2368
		[CompilerGenerated]
		private sealed class <>c__DisplayClass4_0
		{
			// Token: 0x06004E67 RID: 20071 RVA: 0x00002050 File Offset: 0x00000250
			public <>c__DisplayClass4_0()
			{
			}

			// Token: 0x06004E68 RID: 20072 RVA: 0x003B451B File Offset: 0x003B271B
			internal void <UpdateState>b__1()
			{
				this.<>4__this.activityFrame.Text = ActivityFrame.PLEASE_WAIT_TEXT + "\n" + this.s;
			}

			// Token: 0x04002EF6 RID: 12022
			public string s;

			// Token: 0x04002EF7 RID: 12023
			public DataSetDumperPage <>4__this;
		}

		// Token: 0x02000941 RID: 2369
		[CompilerGenerated]
		private sealed class <>c__DisplayClass7_0
		{
			// Token: 0x06004E69 RID: 20073 RVA: 0x00002050 File Offset: 0x00000250
			public <>c__DisplayClass7_0()
			{
			}

			// Token: 0x06004E6A RID: 20074 RVA: 0x003B4542 File Offset: 0x003B2742
			internal void <btnSaveState_Clicked>b__1()
			{
				this.<>4__this.activityFrame.Text = this.s;
			}

			// Token: 0x04002EF8 RID: 12024
			public string s;

			// Token: 0x04002EF9 RID: 12025
			public DataSetDumperPage <>4__this;
		}

		// Token: 0x02000942 RID: 2370
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <BtnUpdateState_Clicked>d__6 : IAsyncStateMachine
		{
			// Token: 0x06004E6B RID: 20075 RVA: 0x003B455C File Offset: 0x003B275C
			void IAsyncStateMachine.MoveNext()
			{
				DataSetDumperPage dataSetDumperPage = this;
				try
				{
					dataSetDumperPage.UpdateState();
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

			// Token: 0x06004E6C RID: 20076 RVA: 0x003B45B4 File Offset: 0x003B27B4
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x04002EFA RID: 12026
			public int <>1__state;

			// Token: 0x04002EFB RID: 12027
			public AsyncVoidMethodBuilder <>t__builder;

			// Token: 0x04002EFC RID: 12028
			public DataSetDumperPage <>4__this;
		}

		// Token: 0x02000943 RID: 2371
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <CodingDetailsPage_Appearing>d__3 : IAsyncStateMachine
		{
			// Token: 0x06004E6D RID: 20077 RVA: 0x003B45C4 File Offset: 0x003B27C4
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				DataSetDumperPage dataSetDumperPage = this;
				try
				{
					TaskAwaiter<CodingRequestResult> taskAwaiter;
					if (num != 0)
					{
						if (!dataSetDumperPage.first_appearing)
						{
							goto IL_00C9;
						}
						dataSetDumperPage.first_appearing = false;
						dataSetDumperPage.activityFrame.IsVisible = true;
						dataSetDumperPage.entryValue.IsEnabled = false;
						Progress<string> progress = new Progress<string>(delegate(string s)
						{
							MainThread.BeginInvokeOnMainThread(new Action(new DataSetDumperPage.<>c__DisplayClass3_0
							{
								<>4__this = dataSetDumperPage,
								s = s
							}.<CodingDetailsPage_Appearing>b__1));
						});
						taskAwaiter = dataSetDumperPage.coding.UpdateCurrentState("", progress).GetAwaiter();
						if (!taskAwaiter.IsCompleted)
						{
							num2 = 0;
							TaskAwaiter<CodingRequestResult> taskAwaiter2 = taskAwaiter;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<CodingRequestResult>, DataSetDumperPage.<CodingDetailsPage_Appearing>d__3>(ref taskAwaiter, ref this);
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
					dataSetDumperPage.activityFrame.IsVisible = false;
					dataSetDumperPage.entryValue.IsEnabled = true;
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

			// Token: 0x06004E6E RID: 20078 RVA: 0x003B46D8 File Offset: 0x003B28D8
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x04002EFD RID: 12029
			public int <>1__state;

			// Token: 0x04002EFE RID: 12030
			public AsyncVoidMethodBuilder <>t__builder;

			// Token: 0x04002EFF RID: 12031
			public DataSetDumperPage <>4__this;

			// Token: 0x04002F00 RID: 12032
			private TaskAwaiter<CodingRequestResult> <>u__1;
		}

		// Token: 0x02000944 RID: 2372
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <UpdateState>d__4 : IAsyncStateMachine
		{
			// Token: 0x06004E6F RID: 20079 RVA: 0x003B46E8 File Offset: 0x003B28E8
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				DataSetDumperPage dataSetDumperPage = this;
				try
				{
					TaskAwaiter<CodingRequestResult> taskAwaiter;
					if (num != 0)
					{
						dataSetDumperPage.activityFrame.IsVisible = true;
						dataSetDumperPage.entryValue.IsEnabled = false;
						Progress<string> progress = new Progress<string>(delegate(string s)
						{
							MainThread.BeginInvokeOnMainThread(new Action(new DataSetDumperPage.<>c__DisplayClass4_0
							{
								<>4__this = dataSetDumperPage,
								s = s
							}.<UpdateState>b__1));
						});
						taskAwaiter = dataSetDumperPage.coding.UpdateCurrentState(dataSetDumperPage.coding.Password, progress).GetAwaiter();
						if (!taskAwaiter.IsCompleted)
						{
							num2 = 0;
							TaskAwaiter<CodingRequestResult> taskAwaiter2 = taskAwaiter;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<CodingRequestResult>, DataSetDumperPage.<UpdateState>d__4>(ref taskAwaiter, ref this);
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
					dataSetDumperPage.activityFrame.IsVisible = false;
					dataSetDumperPage.entryValue.IsEnabled = true;
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

			// Token: 0x06004E70 RID: 20080 RVA: 0x003B47F0 File Offset: 0x003B29F0
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x04002F01 RID: 12033
			public int <>1__state;

			// Token: 0x04002F02 RID: 12034
			public AsyncTaskMethodBuilder <>t__builder;

			// Token: 0x04002F03 RID: 12035
			public DataSetDumperPage <>4__this;

			// Token: 0x04002F04 RID: 12036
			private TaskAwaiter<CodingRequestResult> <>u__1;
		}

		// Token: 0x02000945 RID: 2373
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <btnExport_Clicked>d__8 : IAsyncStateMachine
		{
			// Token: 0x06004E71 RID: 20081 RVA: 0x003B4800 File Offset: 0x003B2A00
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				DataSetDumperPage dataSetDumperPage = this;
				try
				{
					if (num == 0 || !string.IsNullOrEmpty(dataSetDumperPage.coding.CurrentState))
					{
						try
						{
							TaskAwaiter taskAwaiter;
							if (num != 0)
							{
								taskAwaiter = Share.RequestAsync(dataSetDumperPage.coding.CurrentState).GetAwaiter();
								if (!taskAwaiter.IsCompleted)
								{
									num2 = 0;
									TaskAwaiter taskAwaiter2 = taskAwaiter;
									this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, DataSetDumperPage.<btnExport_Clicked>d__8>(ref taskAwaiter, ref this);
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
						catch (Exception)
						{
						}
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

			// Token: 0x06004E72 RID: 20082 RVA: 0x003B48E4 File Offset: 0x003B2AE4
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x04002F05 RID: 12037
			public int <>1__state;

			// Token: 0x04002F06 RID: 12038
			public AsyncVoidMethodBuilder <>t__builder;

			// Token: 0x04002F07 RID: 12039
			public DataSetDumperPage <>4__this;

			// Token: 0x04002F08 RID: 12040
			private TaskAwaiter <>u__1;
		}

		// Token: 0x02000946 RID: 2374
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <btnImport_Clicked>d__9 : IAsyncStateMachine
		{
			// Token: 0x06004E73 RID: 20083 RVA: 0x003B48F4 File Offset: 0x003B2AF4
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				DataSetDumperPage dataSetDumperPage = this;
				try
				{
					try
					{
						TaskAwaiter<FileResult> taskAwaiter;
						if (num != 0)
						{
							taskAwaiter = FilePicker.PickAsync(null).GetAwaiter();
							if (!taskAwaiter.IsCompleted)
							{
								num = (num2 = 0);
								TaskAwaiter<FileResult> taskAwaiter2 = taskAwaiter;
								this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<FileResult>, DataSetDumperPage.<btnImport_Clicked>d__9>(ref taskAwaiter, ref this);
								return;
							}
						}
						else
						{
							TaskAwaiter<FileResult> taskAwaiter2;
							taskAwaiter = taskAwaiter2;
							taskAwaiter2 = default(TaskAwaiter<FileResult>);
							num = (num2 = -1);
						}
						FileResult result = taskAwaiter.GetResult();
						if (result != null)
						{
							StreamReader streamReader = new StreamReader(result.FullPath);
							try
							{
								string text = streamReader.ReadToEnd();
								dataSetDumperPage.entryValue.Text = text;
							}
							finally
							{
								if (num < 0 && streamReader != null)
								{
									((IDisposable)streamReader).Dispose();
								}
							}
						}
					}
					catch (Exception)
					{
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

			// Token: 0x06004E74 RID: 20084 RVA: 0x003B4A04 File Offset: 0x003B2C04
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x04002F09 RID: 12041
			public int <>1__state;

			// Token: 0x04002F0A RID: 12042
			public AsyncVoidMethodBuilder <>t__builder;

			// Token: 0x04002F0B RID: 12043
			public DataSetDumperPage <>4__this;

			// Token: 0x04002F0C RID: 12044
			private TaskAwaiter<FileResult> <>u__1;
		}

		// Token: 0x02000947 RID: 2375
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <btnSaveState_Clicked>d__7 : IAsyncStateMachine
		{
			// Token: 0x06004E75 RID: 20085 RVA: 0x003B4A14 File Offset: 0x003B2C14
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				DataSetDumperPage dataSetDumperPage = this;
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
						taskAwaiter5 = taskAwaiter2;
						taskAwaiter2 = default(TaskAwaiter<bool>);
						num2 = -1;
						goto IL_01D8;
					case 2:
					{
						TaskAwaiter taskAwaiter4;
						taskAwaiter3 = taskAwaiter4;
						taskAwaiter4 = default(TaskAwaiter);
						num2 = -1;
						goto IL_0246;
					}
					case 3:
						taskAwaiter5 = taskAwaiter2;
						taskAwaiter2 = default(TaskAwaiter<bool>);
						num2 = -1;
						goto IL_0300;
					case 4:
					{
						TaskAwaiter taskAwaiter4;
						taskAwaiter3 = taskAwaiter4;
						taskAwaiter4 = default(TaskAwaiter);
						num2 = -1;
						goto IL_036E;
					}
					case 5:
					{
						TaskAwaiter<CodingRequestResult> taskAwaiter7;
						taskAwaiter6 = taskAwaiter7;
						taskAwaiter7 = default(TaskAwaiter<CodingRequestResult>);
						num2 = -1;
						goto IL_0424;
					}
					case 6:
					{
						TaskAwaiter taskAwaiter4;
						taskAwaiter3 = taskAwaiter4;
						taskAwaiter4 = default(TaskAwaiter);
						num2 = -1;
						goto IL_04B6;
					}
					case 7:
					{
						TaskAwaiter<CodingRequestResult> taskAwaiter7;
						taskAwaiter6 = taskAwaiter7;
						taskAwaiter7 = default(TaskAwaiter<CodingRequestResult>);
						num2 = -1;
						goto IL_052A;
					}
					default:
					{
						string text = dataSetDumperPage.entryValue.Text;
						text = text.Replace("0x", "").Replace("0X", "").Replace(",", "")
							.Replace(" ", "")
							.Trim();
						int num3 = 0;
						try
						{
							BitHelpers.ConvertHexToBytesX(text);
						}
						catch (Exception obj)
						{
							num3 = 1;
						}
						if (num3 == 1)
						{
							object obj;
							Exception ex = (Exception)obj;
							taskAwaiter3 = dataSetDumperPage.DisplayAlert(Translate.GetString("coding_WrongInputFormatTitle"), Translate.GetString("coding_WrongInputFormatText") + "\n010203AABBCCDDEEFF", "OK").GetAwaiter();
							if (!taskAwaiter3.IsCompleted)
							{
								num2 = 0;
								TaskAwaiter taskAwaiter4 = taskAwaiter3;
								this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, DataSetDumperPage.<btnSaveState_Clicked>d__7>(ref taskAwaiter3, ref this);
								return;
							}
						}
						else
						{
							byte[] array = null;
							try
							{
								array = BitHelpers.ConvertHexToBytesX(dataSetDumperPage.coding.CurrentState);
							}
							catch (Exception)
							{
							}
							if (dataSetDumperPage.coding.RequiresPro && !SharedSettings.Current.AdsProductPurchased)
							{
								taskAwaiter5 = dataSetDumperPage.DisplayAlert("Car Scanner Pro", Translate.GetString("FeatureLocked"), "OK", Translate.GetString("btnCancel.Content")).GetAwaiter();
								if (!taskAwaiter5.IsCompleted)
								{
									num2 = 1;
									taskAwaiter2 = taskAwaiter5;
									this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<bool>, DataSetDumperPage.<btnSaveState_Clicked>d__7>(ref taskAwaiter5, ref this);
									return;
								}
								goto IL_01D8;
							}
							else if (SharedSettings.Current.CodingsCounter >= 3 && !SharedSettings.Current.AdsProductPurchased)
							{
								taskAwaiter5 = dataSetDumperPage.DisplayAlert("Car Scanner Pro", string.Format(Translate.GetString("coding_FreeCodingsFinished"), SharedSettings.Current.CodingsCounter, 3), "OK", Translate.GetString("btnCancel.Content")).GetAwaiter();
								if (!taskAwaiter5.IsCompleted)
								{
									num2 = 3;
									taskAwaiter2 = taskAwaiter5;
									this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<bool>, DataSetDumperPage.<btnSaveState_Clicked>d__7>(ref taskAwaiter5, ref this);
									return;
								}
								goto IL_0300;
							}
							else
							{
								dataSetDumperPage.activityFrame.IsVisible = true;
								dataSetDumperPage.entryValue.IsEnabled = false;
								progress = new Progress<string>(delegate(string s)
								{
									Device.BeginInvokeOnMainThread(new Action(new DataSetDumperPage.<>c__DisplayClass7_0
									{
										<>4__this = dataSetDumperPage,
										s = s
									}.<btnSaveState_Clicked>b__1));
								});
								taskAwaiter6 = dataSetDumperPage.coding.Execute(dataSetDumperPage.entryPassword.Text, text, dataSetDumperPage.coding.CurrentState, progress, array, false).GetAwaiter();
								if (!taskAwaiter6.IsCompleted)
								{
									num2 = 5;
									TaskAwaiter<CodingRequestResult> taskAwaiter7 = taskAwaiter6;
									this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<CodingRequestResult>, DataSetDumperPage.<btnSaveState_Clicked>d__7>(ref taskAwaiter6, ref this);
									return;
								}
								goto IL_0424;
							}
						}
						break;
					}
					}
					taskAwaiter3.GetResult();
					goto IL_057C;
					IL_01D8:
					if (!taskAwaiter5.GetResult())
					{
						goto IL_024D;
					}
					Page inAppPage = InAppManager.GetInAppPage();
					taskAwaiter3 = dataSetDumperPage.Navigation.PushAsync(inAppPage).GetAwaiter();
					if (!taskAwaiter3.IsCompleted)
					{
						num2 = 2;
						TaskAwaiter taskAwaiter4 = taskAwaiter3;
						this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, DataSetDumperPage.<btnSaveState_Clicked>d__7>(ref taskAwaiter3, ref this);
						return;
					}
					IL_0246:
					taskAwaiter3.GetResult();
					IL_024D:
					goto IL_057C;
					IL_0300:
					if (!taskAwaiter5.GetResult())
					{
						goto IL_0375;
					}
					Page inAppPage2 = InAppManager.GetInAppPage();
					taskAwaiter3 = dataSetDumperPage.Navigation.PushAsync(inAppPage2).GetAwaiter();
					if (!taskAwaiter3.IsCompleted)
					{
						num2 = 4;
						TaskAwaiter taskAwaiter4 = taskAwaiter3;
						this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, DataSetDumperPage.<btnSaveState_Clicked>d__7>(ref taskAwaiter3, ref this);
						return;
					}
					IL_036E:
					taskAwaiter3.GetResult();
					IL_0375:
					goto IL_057C;
					IL_0424:
					CodingRequestResult result = taskAwaiter6.GetResult();
					if (result == CodingRequestResult.Success)
					{
						SharedSettings sharedSettings = SharedSettings.Current;
						int num3 = sharedSettings.CodingsCounter;
						sharedSettings.CodingsCounter = num3 + 1;
						goto IL_04BD;
					}
					taskAwaiter3 = dataSetDumperPage.DisplayAlert(Translate.GetString("coding_Error"), MQBAdaptationTemplate.CodingRequestResultToString(result), "OK").GetAwaiter();
					if (!taskAwaiter3.IsCompleted)
					{
						num2 = 6;
						TaskAwaiter taskAwaiter4 = taskAwaiter3;
						this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, DataSetDumperPage.<btnSaveState_Clicked>d__7>(ref taskAwaiter3, ref this);
						return;
					}
					IL_04B6:
					taskAwaiter3.GetResult();
					IL_04BD:
					taskAwaiter6 = dataSetDumperPage.coding.UpdateCurrentState(dataSetDumperPage.coding.Password, progress).GetAwaiter();
					if (!taskAwaiter6.IsCompleted)
					{
						num2 = 7;
						TaskAwaiter<CodingRequestResult> taskAwaiter7 = taskAwaiter6;
						this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<CodingRequestResult>, DataSetDumperPage.<btnSaveState_Clicked>d__7>(ref taskAwaiter6, ref this);
						return;
					}
					IL_052A:
					taskAwaiter6.GetResult();
					dataSetDumperPage.activityFrame.IsVisible = false;
					dataSetDumperPage.activityFrame.Text = ActivityFrame.PLEASE_WAIT_TEXT;
					dataSetDumperPage.entryValue.IsEnabled = true;
				}
				catch (Exception ex2)
				{
					num2 = -2;
					progress = null;
					this.<>t__builder.SetException(ex2);
					return;
				}
				IL_057C:
				num2 = -2;
				progress = null;
				this.<>t__builder.SetResult();
			}

			// Token: 0x06004E76 RID: 20086 RVA: 0x003B5004 File Offset: 0x003B3204
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x04002F0D RID: 12045
			public int <>1__state;

			// Token: 0x04002F0E RID: 12046
			public AsyncVoidMethodBuilder <>t__builder;

			// Token: 0x04002F0F RID: 12047
			public DataSetDumperPage <>4__this;

			// Token: 0x04002F10 RID: 12048
			private Progress<string> <progress>5__2;

			// Token: 0x04002F11 RID: 12049
			private TaskAwaiter <>u__1;

			// Token: 0x04002F12 RID: 12050
			private TaskAwaiter<bool> <>u__2;

			// Token: 0x04002F13 RID: 12051
			private TaskAwaiter<CodingRequestResult> <>u__3;
		}
	}
}
