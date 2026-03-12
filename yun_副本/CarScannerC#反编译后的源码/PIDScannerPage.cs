using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using CarScannerXamarinForms.Common;
using CarScannerXamarinForms.OBD2;
using CarScannerXamarinForms.PIDScanner;
using CarScannerXamarinForms.PlatformAdapters;
using CarScannerXamarinForms.UserControls;
using CarScannerXamarinForms.ViewModels;
using Newtonsoft.Json;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Internals;
using Xamarin.Forms.PlatformConfiguration.iOSSpecific;
using Xamarin.Forms.Xaml;
using Xamarin.Forms.Xaml.Diagnostics;
using Xamarin.Forms.Xaml.Internals;

namespace CarScannerXamarinForms
{
	// Token: 0x02000147 RID: 327
	[XamlFilePath("PIDScanner\\PIDScannerPage.xaml")]
	public class PIDScannerPage : ContentPage
	{
		// Token: 0x0600061B RID: 1563 RVA: 0x0006634B File Offset: 0x0006454B
		public PIDScannerPage()
		{
			this.InitializeComponent();
		}

		// Token: 0x0600061C RID: 1564 RVA: 0x00066364 File Offset: 0x00064564
		private async void btnBack_Clicked(object sender, EventArgs e)
		{
			if (this.gridResults.IsVisible)
			{
				this.scrollViewScanSettings.IsVisible = true;
				this.gridResults.IsVisible = false;
			}
			else
			{
				await base.Navigation.PopAsync(true);
			}
		}

		// Token: 0x0600061D RID: 1565 RVA: 0x0006639B File Offset: 0x0006459B
		protected override bool OnBackButtonPressed()
		{
			if (Device.RuntimePlatform == "UWP" || Device.RuntimePlatform == "Android")
			{
				this.btnBack_Clicked(this, null);
			}
			return true;
		}

		// Token: 0x0600061E RID: 1566 RVA: 0x000663C8 File Offset: 0x000645C8
		private async void btnStart_Clicked(object sender, EventArgs e)
		{
			this.btnStart.IsEnabled = false;
			this.btnStart.IsVisible = false;
			this.btnCancel.IsVisible = true;
			this.btnCancel.IsEnabled = true;
			await App.OBDReader.DebugWrite("\n[PID SCANNER STARTED]\n");
			List<OBDRequest> requests = new OBDScanTask(this.entryPIDTemplate.Text, this.entryPIDStart.Text, this.entryPIDEnd.Text, this.entryHeaderStart.Text, this.entryHeaderEnd.Text, this.entryStartDiagnosticsCommand.Text, this.entryStopDiagnosticsCommand.Text, this.sendStartDiagCmdOnEachSwitch.IsToggled).GetRequests();
			this.ScanResults = new List<OBDScanResult>(requests.Count);
			this.requestsLeft = requests.Count;
			foreach (OBDRequest obdrequest in requests)
			{
				obdrequest.ResponseReceived += this.Req_ResponseReceived;
				obdrequest.DoNotDecode = true;
			}
			App.OBDReader.ReplaceQueue(requests);
		}

		// Token: 0x0600061F RID: 1567 RVA: 0x00066400 File Offset: 0x00064600
		private void Req_ResponseReceived(OBDRequest request, string reply)
		{
			this.requestsLeft--;
			if (!reply.Contains("NO DATA") && !reply.Contains("UNABLE") && !reply.Contains("ERROR") && !reply.Contains("STOPPED") && !reply.Contains("OK") && (request.ELMFormat != ELMFormat.CAN11bit || reply.Length < 7 || reply[5] != '7' || (reply[6] != 'F' && reply[6] != 'f')))
			{
				OBDScanResult obdscanResult = new OBDScanResult
				{
					RequestHeader = request.Header,
					RequestPID = request.Command,
					Response = reply
				};
				this.ScanResults.Add(obdscanResult);
			}
			Device.BeginInvokeOnMainThread(delegate
			{
				this.labelProgress.Text = this.requestsLeft.ToString();
			});
			if (this.requestsLeft <= 0)
			{
				Device.BeginInvokeOnMainThread(delegate
				{
					this.lvResults.ItemsSource = this.ScanResults;
					this.btnStart.IsEnabled = true;
					this.btnStart.IsVisible = true;
					this.btnCancel.IsEnabled = false;
					this.btnCancel.IsVisible = false;
					this.scrollViewScanSettings.IsVisible = false;
					this.gridResults.IsVisible = true;
					this.labelProgress.Text = "";
				});
			}
		}

		// Token: 0x06000620 RID: 1568 RVA: 0x000664F4 File Offset: 0x000646F4
		private async void btnCancel_Clicked(object sender, EventArgs e)
		{
			this.btnCancel.IsEnabled = false;
			this.btnCancel.IsVisible = false;
			App.OBDReader.ClearRequestQueue();
			this.btnStart.IsEnabled = true;
			this.btnStart.IsVisible = true;
			this.labelProgress.Text = "";
			if (this.ScanResults.Count > 0)
			{
				this.lvResults.ItemsSource = this.ScanResults;
				this.scrollViewScanSettings.IsVisible = false;
				this.gridResults.IsVisible = true;
			}
		}

		// Token: 0x06000621 RID: 1569 RVA: 0x0006652C File Offset: 0x0006472C
		private void btnExport_Clicked(object sender, EventArgs e)
		{
			string text = JsonConvert.SerializeObject(this.ScanResults);
			DateTime nowSafe = DateTimeNowHelper.NowSafe;
			string localFilePath = FileSystemHelper.GetLocalFilePath(string.Concat(new string[]
			{
				"scan-",
				nowSafe.Year.ToString("00"),
				nowSafe.Month.ToString("00"),
				nowSafe.Day.ToString("00"),
				"-",
				nowSafe.Hour.ToString("00"),
				nowSafe.Minute.ToString("00"),
				nowSafe.Second.ToString("00"),
				".txt"
			}));
			using (StreamWriter streamWriter = new StreamWriter(localFilePath, false))
			{
				streamWriter.Write(text);
				streamWriter.Flush();
			}
			try
			{
				this.ShareFile(localFilePath);
			}
			catch (Exception)
			{
			}
		}

		// Token: 0x06000622 RID: 1570 RVA: 0x00066650 File Offset: 0x00064850
		private async void ShareFile(string path)
		{
			if (PlatformHelper.IsAndroid)
			{
				string exportVariantSend = Translate.GetString("droid_ExportSend");
				string exportVariantFile = Translate.GetString("droid_ExportFile");
				string text = await this.DisplayActionSheetCustom(Translate.GetString("droid_ExportMode"), null, null, new string[] { exportVariantFile, exportVariantSend });
				if (text == exportVariantFile)
				{
					await Share.RequestAsync(new ShareFileRequest(new ShareFile(path)));
				}
				else if (text == exportVariantSend)
				{
					EmailMessage emailMessage = new EmailMessage("Car Scanner Scan results", "", Array.Empty<string>());
					if (File.Exists(path))
					{
						emailMessage.Attachments.Add(new EmailAttachment(path));
					}
					await Email.ComposeAsync(emailMessage);
				}
				exportVariantSend = null;
				exportVariantFile = null;
			}
			else if (PlatformHelper.IsiOS)
			{
				if (Device.Idiom == 1)
				{
					Share.RequestAsync(new ShareFileRequest("Car Scanner Scan results", new ShareFile(path)));
				}
				else
				{
					PlatformHelper.IOSService.SendDebugEmail(path, "", "", "");
				}
			}
		}

		// Token: 0x06000623 RID: 1571 RVA: 0x00066690 File Offset: 0x00064890
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private void InitializeComponent()
		{
			if (ResourceLoader.IsEnabled && ResourceLoader.CanProvideContentFor(new ResourceLoader.ResourceLoadingQuery
			{
				AssemblyName = typeof(PIDScannerPage).GetTypeInfo().Assembly.GetName(),
				ResourcePath = "PIDScanner/PIDScannerPage.xaml",
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
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension = new DynamicResourceExtension(), new Uri("PIDScanner\\PIDScannerPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 10, 5);
			CarretReturnToNewLineConverter carretReturnToNewLineConverter;
			VisualDiagnostics.RegisterSourceInfo(carretReturnToNewLineConverter = new CarretReturnToNewLineConverter(), new Uri("PIDScanner\\PIDScannerPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 15, 14);
			ResourceDictionary resourceDictionary;
			VisualDiagnostics.RegisterSourceInfo(resourceDictionary = new ResourceDictionary(), new Uri("PIDScanner\\PIDScannerPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 14, 10);
			On on;
			VisualDiagnostics.RegisterSourceInfo(on = new On(), new Uri("PIDScanner\\PIDScannerPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 20, 14);
			On on2;
			VisualDiagnostics.RegisterSourceInfo(on2 = new On(), new Uri("PIDScanner\\PIDScannerPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 21, 14);
			OnPlatform<Thickness> onPlatform;
			VisualDiagnostics.RegisterSourceInfo(onPlatform = new OnPlatform<Thickness>(), new Uri("PIDScanner\\PIDScannerPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 19, 10);
			RowDefinition rowDefinition;
			VisualDiagnostics.RegisterSourceInfo(rowDefinition = new RowDefinition(), new Uri("PIDScanner\\PIDScannerPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 28, 18);
			RowDefinition rowDefinition2;
			VisualDiagnostics.RegisterSourceInfo(rowDefinition2 = new RowDefinition(), new Uri("PIDScanner\\PIDScannerPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 29, 18);
			ColumnDefinition columnDefinition;
			VisualDiagnostics.RegisterSourceInfo(columnDefinition = new ColumnDefinition(), new Uri("PIDScanner\\PIDScannerPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 34, 22);
			ColumnDefinition columnDefinition2;
			VisualDiagnostics.RegisterSourceInfo(columnDefinition2 = new ColumnDefinition(), new Uri("PIDScanner\\PIDScannerPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 35, 22);
			ColumnDefinition columnDefinition3;
			VisualDiagnostics.RegisterSourceInfo(columnDefinition3 = new ColumnDefinition(), new Uri("PIDScanner\\PIDScannerPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 36, 22);
			Translate translate;
			VisualDiagnostics.RegisterSourceInfo(translate = new Translate(), new Uri("PIDScanner\\PIDScannerPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 45, 21);
			OnPlatform<bool> onPlatform2;
			VisualDiagnostics.RegisterSourceInfo(onPlatform2 = new OnPlatform<bool>(), new Uri("PIDScanner\\PIDScannerPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 48, 26);
			LinkButton linkButton;
			VisualDiagnostics.RegisterSourceInfo(linkButton = new LinkButton(), new Uri("PIDScanner\\PIDScannerPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 38, 18);
			Translate translate2;
			VisualDiagnostics.RegisterSourceInfo(translate2 = new Translate(), new Uri("PIDScanner\\PIDScannerPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 59, 21);
			Label label;
			VisualDiagnostics.RegisterSourceInfo(label = new Label(), new Uri("PIDScanner\\PIDScannerPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 54, 18);
			Grid grid;
			VisualDiagnostics.RegisterSourceInfo(grid = new Grid(), new Uri("PIDScanner\\PIDScannerPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 32, 14);
			Label label2;
			VisualDiagnostics.RegisterSourceInfo(label2 = new Label(), new Uri("PIDScanner\\PIDScannerPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 68, 22);
			Entry entry;
			VisualDiagnostics.RegisterSourceInfo(entry = new Entry(), new Uri("PIDScanner\\PIDScannerPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 69, 22);
			Label label3;
			VisualDiagnostics.RegisterSourceInfo(label3 = new Label(), new Uri("PIDScanner\\PIDScannerPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 71, 22);
			Entry entry2;
			VisualDiagnostics.RegisterSourceInfo(entry2 = new Entry(), new Uri("PIDScanner\\PIDScannerPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 72, 22);
			Label label4;
			VisualDiagnostics.RegisterSourceInfo(label4 = new Label(), new Uri("PIDScanner\\PIDScannerPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 74, 22);
			Entry entry3;
			VisualDiagnostics.RegisterSourceInfo(entry3 = new Entry(), new Uri("PIDScanner\\PIDScannerPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 75, 22);
			Label label5;
			VisualDiagnostics.RegisterSourceInfo(label5 = new Label(), new Uri("PIDScanner\\PIDScannerPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 77, 22);
			Entry entry4;
			VisualDiagnostics.RegisterSourceInfo(entry4 = new Entry(), new Uri("PIDScanner\\PIDScannerPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 78, 22);
			Label label6;
			VisualDiagnostics.RegisterSourceInfo(label6 = new Label(), new Uri("PIDScanner\\PIDScannerPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 80, 22);
			Entry entry5;
			VisualDiagnostics.RegisterSourceInfo(entry5 = new Entry(), new Uri("PIDScanner\\PIDScannerPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 81, 22);
			Label label7;
			VisualDiagnostics.RegisterSourceInfo(label7 = new Label(), new Uri("PIDScanner\\PIDScannerPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 83, 22);
			Entry entry6;
			VisualDiagnostics.RegisterSourceInfo(entry6 = new Entry(), new Uri("PIDScanner\\PIDScannerPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 84, 22);
			Label label8;
			VisualDiagnostics.RegisterSourceInfo(label8 = new Label(), new Uri("PIDScanner\\PIDScannerPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 85, 22);
			Entry entry7;
			VisualDiagnostics.RegisterSourceInfo(entry7 = new Entry(), new Uri("PIDScanner\\PIDScannerPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 86, 22);
			Label label9;
			VisualDiagnostics.RegisterSourceInfo(label9 = new Label(), new Uri("PIDScanner\\PIDScannerPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 89, 22);
			Switch @switch;
			VisualDiagnostics.RegisterSourceInfo(@switch = new Switch(), new Uri("PIDScanner\\PIDScannerPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 90, 22);
			Label label10;
			VisualDiagnostics.RegisterSourceInfo(label10 = new Label(), new Uri("PIDScanner\\PIDScannerPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 92, 22);
			Button button;
			VisualDiagnostics.RegisterSourceInfo(button = new Button(), new Uri("PIDScanner\\PIDScannerPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 94, 22);
			Button button2;
			VisualDiagnostics.RegisterSourceInfo(button2 = new Button(), new Uri("PIDScanner\\PIDScannerPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 98, 22);
			StackLayout stackLayout;
			VisualDiagnostics.RegisterSourceInfo(stackLayout = new StackLayout(), new Uri("PIDScanner\\PIDScannerPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 67, 18);
			ScrollView scrollView;
			VisualDiagnostics.RegisterSourceInfo(scrollView = new ScrollView(), new Uri("PIDScanner\\PIDScannerPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 63, 14);
			RowDefinition rowDefinition3;
			VisualDiagnostics.RegisterSourceInfo(rowDefinition3 = new RowDefinition(), new Uri("PIDScanner\\PIDScannerPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 110, 22);
			RowDefinition rowDefinition4;
			VisualDiagnostics.RegisterSourceInfo(rowDefinition4 = new RowDefinition(), new Uri("PIDScanner\\PIDScannerPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 111, 22);
			DataTemplate dataTemplate;
			VisualDiagnostics.RegisterSourceInfo(dataTemplate = new DataTemplate(), new Uri("PIDScanner\\PIDScannerPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 118, 26);
			ListView listView;
			VisualDiagnostics.RegisterSourceInfo(listView = new ListView(), new Uri("PIDScanner\\PIDScannerPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 113, 18);
			Button button3;
			VisualDiagnostics.RegisterSourceInfo(button3 = new Button(), new Uri("PIDScanner\\PIDScannerPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 145, 18);
			Grid grid2;
			VisualDiagnostics.RegisterSourceInfo(grid2 = new Grid(), new Uri("PIDScanner\\PIDScannerPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 105, 14);
			Grid grid3;
			VisualDiagnostics.RegisterSourceInfo(grid3 = new Grid(), new Uri("PIDScanner\\PIDScannerPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 26, 10);
			VisualDiagnostics.RegisterSourceInfo(this, new Uri("PIDScanner\\PIDScannerPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 2, 2);
			NameScope nameScope = NameScope.GetNameScope(this) ?? new NameScope();
			NameScope.SetNameScope(this, nameScope);
			nameScope.RegisterName("LayoutRoot", grid3);
			if (grid3.StyleId == null)
			{
				grid3.StyleId = "LayoutRoot";
			}
			nameScope.RegisterName("gridButtons", grid);
			if (grid.StyleId == null)
			{
				grid.StyleId = "gridButtons";
			}
			nameScope.RegisterName("scrollViewScanSettings", scrollView);
			if (scrollView.StyleId == null)
			{
				scrollView.StyleId = "scrollViewScanSettings";
			}
			nameScope.RegisterName("entryHeaderStart", entry);
			if (entry.StyleId == null)
			{
				entry.StyleId = "entryHeaderStart";
			}
			nameScope.RegisterName("entryHeaderEnd", entry2);
			if (entry2.StyleId == null)
			{
				entry2.StyleId = "entryHeaderEnd";
			}
			nameScope.RegisterName("entryPIDTemplate", entry3);
			if (entry3.StyleId == null)
			{
				entry3.StyleId = "entryPIDTemplate";
			}
			nameScope.RegisterName("entryPIDStart", entry4);
			if (entry4.StyleId == null)
			{
				entry4.StyleId = "entryPIDStart";
			}
			nameScope.RegisterName("entryPIDEnd", entry5);
			if (entry5.StyleId == null)
			{
				entry5.StyleId = "entryPIDEnd";
			}
			nameScope.RegisterName("entryStartDiagnosticsCommand", entry6);
			if (entry6.StyleId == null)
			{
				entry6.StyleId = "entryStartDiagnosticsCommand";
			}
			nameScope.RegisterName("entryStopDiagnosticsCommand", entry7);
			if (entry7.StyleId == null)
			{
				entry7.StyleId = "entryStopDiagnosticsCommand";
			}
			nameScope.RegisterName("sendStartDiagCmdOnEachSwitch", @switch);
			if (@switch.StyleId == null)
			{
				@switch.StyleId = "sendStartDiagCmdOnEachSwitch";
			}
			nameScope.RegisterName("labelProgress", label10);
			if (label10.StyleId == null)
			{
				label10.StyleId = "labelProgress";
			}
			nameScope.RegisterName("btnStart", button);
			if (button.StyleId == null)
			{
				button.StyleId = "btnStart";
			}
			nameScope.RegisterName("btnCancel", button2);
			if (button2.StyleId == null)
			{
				button2.StyleId = "btnCancel";
			}
			nameScope.RegisterName("gridResults", grid2);
			if (grid2.StyleId == null)
			{
				grid2.StyleId = "gridResults";
			}
			nameScope.RegisterName("lvResults", listView);
			if (listView.StyleId == null)
			{
				listView.StyleId = "lvResults";
			}
			nameScope.RegisterName("btnExport", button3);
			if (button3.StyleId == null)
			{
				button3.StyleId = "btnExport";
			}
			this.LayoutRoot = grid3;
			this.gridButtons = grid;
			this.scrollViewScanSettings = scrollView;
			this.entryHeaderStart = entry;
			this.entryHeaderEnd = entry2;
			this.entryPIDTemplate = entry3;
			this.entryPIDStart = entry4;
			this.entryPIDEnd = entry5;
			this.entryStartDiagnosticsCommand = entry6;
			this.entryStopDiagnosticsCommand = entry7;
			this.sendStartDiagCmdOnEachSwitch = @switch;
			this.labelProgress = label10;
			this.btnStart = button;
			this.btnCancel = button2;
			this.gridResults = grid2;
			this.lvResults = listView;
			this.btnExport = button3;
			this.Resources = resourceDictionary;
			resourceDictionary.Add("CarretReturnToNewLineConverter", carretReturnToNewLineConverter);
			this.SetValue(Page.PrefersStatusBarHiddenProperty, 2);
			this.SetValue(Page.UseSafeAreaProperty, true);
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
			xamlServiceProvider.Add(typeFromHandle2, new XamlTypeResolver(xmlNamespaceResolver, typeof(PIDScannerPage).GetTypeInfo().Assembly));
			xamlServiceProvider.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(10, 5)));
			DynamicResource dynamicResource = markupExtension.ProvideValue(xamlServiceProvider);
			this.SetDynamicResource(VisualElement.BackgroundColorProperty, dynamicResource.Key);
			this.SetValue(NavigationPage.HasBackButtonProperty, false);
			this.SetValue(NavigationPage.HasNavigationBarProperty, false);
			this.Resources = resourceDictionary;
			on.Platform = new List<string>(2) { "Android", "WinPhone" };
			on.Value = "0";
			onPlatform.Platforms.Add(on);
			on2.Platform = new List<string>(1) { "iOS" };
			on2.Value = "5,0,5,5";
			onPlatform.Platforms.Add(on2);
			this.SetValue(Page.PaddingProperty, onPlatform);
			rowDefinition.SetValue(RowDefinition.HeightProperty, new GridLengthTypeConverter().ConvertFromInvariantString("auto"));
			grid3.GetValue(Grid.RowDefinitionsProperty).Add(rowDefinition);
			rowDefinition2.SetValue(RowDefinition.HeightProperty, new GridLengthTypeConverter().ConvertFromInvariantString("*"));
			grid3.GetValue(Grid.RowDefinitionsProperty).Add(rowDefinition2);
			grid.SetValue(Grid.RowProperty, 0);
			columnDefinition.SetValue(ColumnDefinition.WidthProperty, new GridLengthTypeConverter().ConvertFromInvariantString("*"));
			grid.GetValue(Grid.ColumnDefinitionsProperty).Add(columnDefinition);
			columnDefinition2.SetValue(ColumnDefinition.WidthProperty, new GridLengthTypeConverter().ConvertFromInvariantString("auto"));
			grid.GetValue(Grid.ColumnDefinitionsProperty).Add(columnDefinition2);
			columnDefinition3.SetValue(ColumnDefinition.WidthProperty, new GridLengthTypeConverter().ConvertFromInvariantString("*"));
			grid.GetValue(Grid.ColumnDefinitionsProperty).Add(columnDefinition3);
			linkButton.SetValue(Grid.ColumnProperty, 0);
			linkButton.SetValue(View.MarginProperty, new Thickness(0.0));
			linkButton.SetValue(VisualElement.BackgroundColorProperty, Color.Transparent);
			linkButton.SetValue(Button.BorderColorProperty, Color.Transparent);
			linkButton.Clicked += this.btnBack_Clicked;
			linkButton.SetValue(View.HorizontalOptionsProperty, LayoutOptions.Start);
			translate.Text = "ios_Back";
			IMarkupExtension markupExtension2 = translate;
			XamlServiceProvider xamlServiceProvider2 = new XamlServiceProvider();
			Type typeFromHandle3 = typeof(IProvideValueTarget);
			object[] array2 = new object[0 + 4];
			array2[0] = linkButton;
			array2[1] = grid;
			array2[2] = grid3;
			array2[3] = this;
			object obj2;
			xamlServiceProvider2.Add(typeFromHandle3, obj2 = new SimpleValueTargetProvider(array2, Button.TextProperty, nameScope));
			xamlServiceProvider2.Add(typeof(IReferenceProvider), obj2);
			Type typeFromHandle4 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver2 = new XmlNamespaceResolver();
			xmlNamespaceResolver2.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver2.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver2.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver2.Add("local", "clr-namespace:CarScannerXamarinForms");
			xamlServiceProvider2.Add(typeFromHandle4, new XamlTypeResolver(xmlNamespaceResolver2, typeof(PIDScannerPage).GetTypeInfo().Assembly));
			xamlServiceProvider2.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(45, 21)));
			object obj3 = markupExtension2.ProvideValue(xamlServiceProvider2);
			linkButton.Text = obj3;
			linkButton.SetValue(View.VerticalOptionsProperty, LayoutOptions.Center);
			onPlatform2.iOS = true;
			onPlatform2.Android = true;
			linkButton.SetValue(VisualElement.IsVisibleProperty, onPlatform2);
			grid.Children.Add(linkButton);
			label.SetValue(Grid.ColumnProperty, 1);
			label.SetValue(Label.FontAttributesProperty, new FontAttributesConverter().ConvertFromInvariantString("Bold"));
			label.SetValue(View.HorizontalOptionsProperty, LayoutOptions.Center);
			label.SetValue(Label.HorizontalTextAlignmentProperty, new TextAlignmentConverter().ConvertFromInvariantString("Center"));
			translate2.Text = "ios_MainPage_TileTerminal";
			IMarkupExtension markupExtension3 = translate2;
			XamlServiceProvider xamlServiceProvider3 = new XamlServiceProvider();
			Type typeFromHandle5 = typeof(IProvideValueTarget);
			object[] array3 = new object[0 + 4];
			array3[0] = label;
			array3[1] = grid;
			array3[2] = grid3;
			array3[3] = this;
			object obj4;
			xamlServiceProvider3.Add(typeFromHandle5, obj4 = new SimpleValueTargetProvider(array3, Label.TextProperty, nameScope));
			xamlServiceProvider3.Add(typeof(IReferenceProvider), obj4);
			Type typeFromHandle6 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver3 = new XmlNamespaceResolver();
			xmlNamespaceResolver3.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver3.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver3.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver3.Add("local", "clr-namespace:CarScannerXamarinForms");
			xamlServiceProvider3.Add(typeFromHandle6, new XamlTypeResolver(xmlNamespaceResolver3, typeof(PIDScannerPage).GetTypeInfo().Assembly));
			xamlServiceProvider3.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(59, 21)));
			object obj5 = markupExtension3.ProvideValue(xamlServiceProvider3);
			label.Text = obj5;
			label.SetValue(View.VerticalOptionsProperty, LayoutOptions.Center);
			label.SetValue(Label.VerticalTextAlignmentProperty, new TextAlignmentConverter().ConvertFromInvariantString("Center"));
			grid.Children.Add(label);
			grid3.Children.Add(grid);
			scrollView.SetValue(Grid.RowProperty, 1);
			scrollView.SetValue(ScrollView.OrientationProperty, 0);
			stackLayout.SetValue(StackLayout.OrientationProperty, 0);
			label2.SetValue(Label.TextProperty, "Start header:");
			stackLayout.Children.Add(label2);
			entry.SetValue(Entry.TextProperty, "7E0");
			stackLayout.Children.Add(entry);
			label3.SetValue(Label.TextProperty, "End header:");
			stackLayout.Children.Add(label3);
			entry2.SetValue(Entry.TextProperty, "7E1");
			stackLayout.Children.Add(entry2);
			label4.SetValue(Label.TextProperty, "PID template:");
			stackLayout.Children.Add(label4);
			entry3.SetValue(Entry.TextProperty, "21{0}");
			stackLayout.Children.Add(entry3);
			label5.SetValue(Label.TextProperty, "From value:");
			stackLayout.Children.Add(label5);
			entry4.SetValue(Entry.TextProperty, "01");
			stackLayout.Children.Add(entry4);
			label6.SetValue(Label.TextProperty, "To value:");
			stackLayout.Children.Add(label6);
			entry5.SetValue(Entry.TextProperty, "20");
			stackLayout.Children.Add(entry5);
			label7.SetValue(Label.TextProperty, "Start diagnostic commands:");
			stackLayout.Children.Add(label7);
			entry6.SetValue(Entry.TextProperty, "");
			stackLayout.Children.Add(entry6);
			label8.SetValue(Label.TextProperty, "Stop diagnostic commands:");
			stackLayout.Children.Add(label8);
			entry7.SetValue(Entry.TextProperty, "");
			stackLayout.Children.Add(entry7);
			label9.SetValue(Label.TextProperty, "Send start diagnostic commands on every command:");
			stackLayout.Children.Add(label9);
			@switch.SetValue(Switch.IsToggledProperty, false);
			stackLayout.Children.Add(@switch);
			stackLayout.Children.Add(label10);
			button.Clicked += this.btnStart_Clicked;
			button.SetValue(Button.TextProperty, "Start");
			stackLayout.Children.Add(button);
			button2.Clicked += this.btnCancel_Clicked;
			button2.SetValue(VisualElement.IsVisibleProperty, new VisualElement.VisibilityConverter().ConvertFromInvariantString("false"));
			button2.SetValue(Button.TextProperty, "Cancel");
			stackLayout.Children.Add(button2);
			scrollView.Content = stackLayout;
			grid3.Children.Add(scrollView);
			grid2.SetValue(Grid.RowProperty, 1);
			grid2.SetValue(VisualElement.IsVisibleProperty, new VisualElement.VisibilityConverter().ConvertFromInvariantString("False"));
			rowDefinition3.SetValue(RowDefinition.HeightProperty, new GridLengthTypeConverter().ConvertFromInvariantString("*"));
			grid2.GetValue(Grid.RowDefinitionsProperty).Add(rowDefinition3);
			rowDefinition4.SetValue(RowDefinition.HeightProperty, new GridLengthTypeConverter().ConvertFromInvariantString("auto"));
			grid2.GetValue(Grid.RowDefinitionsProperty).Add(rowDefinition4);
			listView.SetValue(Grid.RowProperty, 0);
			listView.SetValue(ListView.HasUnevenRowsProperty, true);
			IDataTemplate dataTemplate2 = dataTemplate;
			PIDScannerPage.<InitializeComponent>_anonXamlCDataTemplate_78 <InitializeComponent>_anonXamlCDataTemplate_ = new PIDScannerPage.<InitializeComponent>_anonXamlCDataTemplate_78();
			object[] array4 = new object[0 + 5];
			array4[0] = dataTemplate;
			array4[1] = listView;
			array4[2] = grid2;
			array4[3] = grid3;
			array4[4] = this;
			<InitializeComponent>_anonXamlCDataTemplate_.parentValues = array4;
			<InitializeComponent>_anonXamlCDataTemplate_.root = this;
			dataTemplate2.LoadTemplate = new Func<object>(<InitializeComponent>_anonXamlCDataTemplate_.LoadDataTemplate);
			listView.SetValue(ItemsView<Cell>.ItemTemplateProperty, dataTemplate);
			grid2.Children.Add(listView);
			button3.SetValue(Grid.RowProperty, 1);
			button3.Clicked += this.btnExport_Clicked;
			button3.SetValue(Button.TextProperty, "Export");
			grid2.Children.Add(button3);
			grid3.Children.Add(grid2);
			this.SetValue(ContentPage.ContentProperty, grid3);
		}

		// Token: 0x06000624 RID: 1572 RVA: 0x00067C4B File Offset: 0x00065E4B
		[CompilerGenerated]
		private void <Req_ResponseReceived>b__6_0()
		{
			this.labelProgress.Text = this.requestsLeft.ToString();
		}

		// Token: 0x06000625 RID: 1573 RVA: 0x00067C64 File Offset: 0x00065E64
		[CompilerGenerated]
		private void <Req_ResponseReceived>b__6_1()
		{
			this.lvResults.ItemsSource = this.ScanResults;
			this.btnStart.IsEnabled = true;
			this.btnStart.IsVisible = true;
			this.btnCancel.IsEnabled = false;
			this.btnCancel.IsVisible = false;
			this.scrollViewScanSettings.IsVisible = false;
			this.gridResults.IsVisible = true;
			this.labelProgress.Text = "";
		}

		// Token: 0x06000626 RID: 1574 RVA: 0x00067CDC File Offset: 0x00065EDC
		private void __InitComponentRuntime()
		{
			Extensions.LoadFromXaml<PIDScannerPage>(this, typeof(PIDScannerPage));
			this.LayoutRoot = NameScopeExtensions.FindByName<Grid>(this, "LayoutRoot");
			this.gridButtons = NameScopeExtensions.FindByName<Grid>(this, "gridButtons");
			this.scrollViewScanSettings = NameScopeExtensions.FindByName<ScrollView>(this, "scrollViewScanSettings");
			this.entryHeaderStart = NameScopeExtensions.FindByName<Entry>(this, "entryHeaderStart");
			this.entryHeaderEnd = NameScopeExtensions.FindByName<Entry>(this, "entryHeaderEnd");
			this.entryPIDTemplate = NameScopeExtensions.FindByName<Entry>(this, "entryPIDTemplate");
			this.entryPIDStart = NameScopeExtensions.FindByName<Entry>(this, "entryPIDStart");
			this.entryPIDEnd = NameScopeExtensions.FindByName<Entry>(this, "entryPIDEnd");
			this.entryStartDiagnosticsCommand = NameScopeExtensions.FindByName<Entry>(this, "entryStartDiagnosticsCommand");
			this.entryStopDiagnosticsCommand = NameScopeExtensions.FindByName<Entry>(this, "entryStopDiagnosticsCommand");
			this.sendStartDiagCmdOnEachSwitch = NameScopeExtensions.FindByName<Switch>(this, "sendStartDiagCmdOnEachSwitch");
			this.labelProgress = NameScopeExtensions.FindByName<Label>(this, "labelProgress");
			this.btnStart = NameScopeExtensions.FindByName<Button>(this, "btnStart");
			this.btnCancel = NameScopeExtensions.FindByName<Button>(this, "btnCancel");
			this.gridResults = NameScopeExtensions.FindByName<Grid>(this, "gridResults");
			this.lvResults = NameScopeExtensions.FindByName<ListView>(this, "lvResults");
			this.btnExport = NameScopeExtensions.FindByName<Button>(this, "btnExport");
		}

		// Token: 0x040004FB RID: 1275
		private int requestsLeft;

		// Token: 0x040004FC RID: 1276
		private List<OBDScanResult> ScanResults = new List<OBDScanResult>();

		// Token: 0x040004FD RID: 1277
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private Grid LayoutRoot;

		// Token: 0x040004FE RID: 1278
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private Grid gridButtons;

		// Token: 0x040004FF RID: 1279
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private ScrollView scrollViewScanSettings;

		// Token: 0x04000500 RID: 1280
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private Entry entryHeaderStart;

		// Token: 0x04000501 RID: 1281
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private Entry entryHeaderEnd;

		// Token: 0x04000502 RID: 1282
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private Entry entryPIDTemplate;

		// Token: 0x04000503 RID: 1283
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private Entry entryPIDStart;

		// Token: 0x04000504 RID: 1284
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private Entry entryPIDEnd;

		// Token: 0x04000505 RID: 1285
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private Entry entryStartDiagnosticsCommand;

		// Token: 0x04000506 RID: 1286
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private Entry entryStopDiagnosticsCommand;

		// Token: 0x04000507 RID: 1287
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private Switch sendStartDiagCmdOnEachSwitch;

		// Token: 0x04000508 RID: 1288
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private Label labelProgress;

		// Token: 0x04000509 RID: 1289
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private Button btnStart;

		// Token: 0x0400050A RID: 1290
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private Button btnCancel;

		// Token: 0x0400050B RID: 1291
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private Grid gridResults;

		// Token: 0x0400050C RID: 1292
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private ListView lvResults;

		// Token: 0x0400050D RID: 1293
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private Button btnExport;

		// Token: 0x02000148 RID: 328
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <ShareFile>d__9 : IAsyncStateMachine
		{
			// Token: 0x06000627 RID: 1575 RVA: 0x00067E1C File Offset: 0x0006601C
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				PIDScannerPage pidscannerPage = this;
				try
				{
					TaskAwaiter<string> taskAwaiter;
					TaskAwaiter taskAwaiter3;
					switch (num)
					{
					case 0:
					{
						TaskAwaiter<string> taskAwaiter2;
						taskAwaiter = taskAwaiter2;
						taskAwaiter2 = default(TaskAwaiter<string>);
						num2 = -1;
						break;
					}
					case 1:
					{
						TaskAwaiter taskAwaiter4;
						taskAwaiter3 = taskAwaiter4;
						taskAwaiter4 = default(TaskAwaiter);
						num2 = -1;
						goto IL_013E;
					}
					case 2:
					{
						TaskAwaiter taskAwaiter4;
						taskAwaiter3 = taskAwaiter4;
						taskAwaiter4 = default(TaskAwaiter);
						num2 = -1;
						goto IL_01ED;
					}
					default:
						if (PlatformHelper.IsAndroid)
						{
							exportVariantSend = Translate.GetString("droid_ExportSend");
							exportVariantFile = Translate.GetString("droid_ExportFile");
							taskAwaiter = pidscannerPage.DisplayActionSheetCustom(Translate.GetString("droid_ExportMode"), null, null, new string[] { exportVariantFile, exportVariantSend }).GetAwaiter();
							if (!taskAwaiter.IsCompleted)
							{
								num2 = 0;
								TaskAwaiter<string> taskAwaiter2 = taskAwaiter;
								this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<string>, PIDScannerPage.<ShareFile>d__9>(ref taskAwaiter, ref this);
								return;
							}
						}
						else
						{
							if (!PlatformHelper.IsiOS)
							{
								goto IL_0250;
							}
							if (Device.Idiom == 1)
							{
								Share.RequestAsync(new ShareFileRequest("Car Scanner Scan results", new ShareFile(path)));
								goto IL_0250;
							}
							PlatformHelper.IOSService.SendDebugEmail(path, "", "", "");
							goto IL_0250;
						}
						break;
					}
					string result = taskAwaiter.GetResult();
					if (result == exportVariantFile)
					{
						taskAwaiter3 = Share.RequestAsync(new ShareFileRequest(new ShareFile(path))).GetAwaiter();
						if (!taskAwaiter3.IsCompleted)
						{
							num2 = 1;
							TaskAwaiter taskAwaiter4 = taskAwaiter3;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, PIDScannerPage.<ShareFile>d__9>(ref taskAwaiter3, ref this);
							return;
						}
					}
					else
					{
						if (!(result == exportVariantSend))
						{
							goto IL_01F4;
						}
						EmailMessage emailMessage = new EmailMessage("Car Scanner Scan results", "", Array.Empty<string>());
						if (File.Exists(path))
						{
							emailMessage.Attachments.Add(new EmailAttachment(path));
						}
						taskAwaiter3 = Email.ComposeAsync(emailMessage).GetAwaiter();
						if (!taskAwaiter3.IsCompleted)
						{
							num2 = 2;
							TaskAwaiter taskAwaiter4 = taskAwaiter3;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, PIDScannerPage.<ShareFile>d__9>(ref taskAwaiter3, ref this);
							return;
						}
						goto IL_01ED;
					}
					IL_013E:
					taskAwaiter3.GetResult();
					goto IL_01F4;
					IL_01ED:
					taskAwaiter3.GetResult();
					IL_01F4:
					exportVariantSend = null;
					exportVariantFile = null;
					IL_0250:;
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

			// Token: 0x06000628 RID: 1576 RVA: 0x000680C4 File Offset: 0x000662C4
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x0400050E RID: 1294
			public int <>1__state;

			// Token: 0x0400050F RID: 1295
			public AsyncVoidMethodBuilder <>t__builder;

			// Token: 0x04000510 RID: 1296
			public PIDScannerPage <>4__this;

			// Token: 0x04000511 RID: 1297
			public string path;

			// Token: 0x04000512 RID: 1298
			private string <exportVariantSend>5__2;

			// Token: 0x04000513 RID: 1299
			private string <exportVariantFile>5__3;

			// Token: 0x04000514 RID: 1300
			private TaskAwaiter<string> <>u__1;

			// Token: 0x04000515 RID: 1301
			private TaskAwaiter <>u__2;
		}

		// Token: 0x02000149 RID: 329
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <btnBack_Clicked>d__1 : IAsyncStateMachine
		{
			// Token: 0x06000629 RID: 1577 RVA: 0x000680D4 File Offset: 0x000662D4
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				PIDScannerPage pidscannerPage = this;
				try
				{
					TaskAwaiter<Page> taskAwaiter;
					if (num != 0)
					{
						if (pidscannerPage.gridResults.IsVisible)
						{
							pidscannerPage.scrollViewScanSettings.IsVisible = true;
							pidscannerPage.gridResults.IsVisible = false;
							goto IL_0097;
						}
						taskAwaiter = pidscannerPage.Navigation.PopAsync(true).GetAwaiter();
						if (!taskAwaiter.IsCompleted)
						{
							num2 = 0;
							TaskAwaiter<Page> taskAwaiter2 = taskAwaiter;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<Page>, PIDScannerPage.<btnBack_Clicked>d__1>(ref taskAwaiter, ref this);
							return;
						}
					}
					else
					{
						TaskAwaiter<Page> taskAwaiter2;
						taskAwaiter = taskAwaiter2;
						taskAwaiter2 = default(TaskAwaiter<Page>);
						num2 = -1;
					}
					taskAwaiter.GetResult();
					IL_0097:;
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

			// Token: 0x0600062A RID: 1578 RVA: 0x000681B4 File Offset: 0x000663B4
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x04000516 RID: 1302
			public int <>1__state;

			// Token: 0x04000517 RID: 1303
			public AsyncVoidMethodBuilder <>t__builder;

			// Token: 0x04000518 RID: 1304
			public PIDScannerPage <>4__this;

			// Token: 0x04000519 RID: 1305
			private TaskAwaiter<Page> <>u__1;
		}

		// Token: 0x0200014A RID: 330
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <btnCancel_Clicked>d__7 : IAsyncStateMachine
		{
			// Token: 0x0600062B RID: 1579 RVA: 0x000681C4 File Offset: 0x000663C4
			void IAsyncStateMachine.MoveNext()
			{
				PIDScannerPage pidscannerPage = this;
				try
				{
					pidscannerPage.btnCancel.IsEnabled = false;
					pidscannerPage.btnCancel.IsVisible = false;
					App.OBDReader.ClearRequestQueue();
					pidscannerPage.btnStart.IsEnabled = true;
					pidscannerPage.btnStart.IsVisible = true;
					pidscannerPage.labelProgress.Text = "";
					if (pidscannerPage.ScanResults.Count > 0)
					{
						pidscannerPage.lvResults.ItemsSource = pidscannerPage.ScanResults;
						pidscannerPage.scrollViewScanSettings.IsVisible = false;
						pidscannerPage.gridResults.IsVisible = true;
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

			// Token: 0x0600062C RID: 1580 RVA: 0x00068298 File Offset: 0x00066498
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x0400051A RID: 1306
			public int <>1__state;

			// Token: 0x0400051B RID: 1307
			public AsyncVoidMethodBuilder <>t__builder;

			// Token: 0x0400051C RID: 1308
			public PIDScannerPage <>4__this;
		}

		// Token: 0x0200014B RID: 331
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <btnStart_Clicked>d__4 : IAsyncStateMachine
		{
			// Token: 0x0600062D RID: 1581 RVA: 0x000682A8 File Offset: 0x000664A8
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				PIDScannerPage pidscannerPage = this;
				try
				{
					TaskAwaiter taskAwaiter;
					if (num != 0)
					{
						pidscannerPage.btnStart.IsEnabled = false;
						pidscannerPage.btnStart.IsVisible = false;
						pidscannerPage.btnCancel.IsVisible = true;
						pidscannerPage.btnCancel.IsEnabled = true;
						taskAwaiter = App.OBDReader.DebugWrite("\n[PID SCANNER STARTED]\n").GetAwaiter();
						if (!taskAwaiter.IsCompleted)
						{
							num = (num2 = 0);
							TaskAwaiter taskAwaiter2 = taskAwaiter;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, PIDScannerPage.<btnStart_Clicked>d__4>(ref taskAwaiter, ref this);
							return;
						}
					}
					else
					{
						TaskAwaiter taskAwaiter2;
						taskAwaiter = taskAwaiter2;
						taskAwaiter2 = default(TaskAwaiter);
						num = (num2 = -1);
					}
					taskAwaiter.GetResult();
					List<OBDRequest> requests = new OBDScanTask(pidscannerPage.entryPIDTemplate.Text, pidscannerPage.entryPIDStart.Text, pidscannerPage.entryPIDEnd.Text, pidscannerPage.entryHeaderStart.Text, pidscannerPage.entryHeaderEnd.Text, pidscannerPage.entryStartDiagnosticsCommand.Text, pidscannerPage.entryStopDiagnosticsCommand.Text, pidscannerPage.sendStartDiagCmdOnEachSwitch.IsToggled).GetRequests();
					pidscannerPage.ScanResults = new List<OBDScanResult>(requests.Count);
					pidscannerPage.requestsLeft = requests.Count;
					List<OBDRequest>.Enumerator enumerator = requests.GetEnumerator();
					try
					{
						while (enumerator.MoveNext())
						{
							OBDRequest obdrequest = enumerator.Current;
							obdrequest.ResponseReceived += pidscannerPage.Req_ResponseReceived;
							obdrequest.DoNotDecode = true;
						}
					}
					finally
					{
						if (num < 0)
						{
							((IDisposable)enumerator).Dispose();
						}
					}
					App.OBDReader.ReplaceQueue(requests);
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

			// Token: 0x0600062E RID: 1582 RVA: 0x00068490 File Offset: 0x00066690
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x0400051D RID: 1309
			public int <>1__state;

			// Token: 0x0400051E RID: 1310
			public AsyncVoidMethodBuilder <>t__builder;

			// Token: 0x0400051F RID: 1311
			public PIDScannerPage <>4__this;

			// Token: 0x04000520 RID: 1312
			private TaskAwaiter <>u__1;
		}

		// Token: 0x0200014C RID: 332
		[CompilerGenerated]
		private sealed class <InitializeComponent>_anonXamlCDataTemplate_78
		{
			// Token: 0x0600062F RID: 1583 RVA: 0x000684A0 File Offset: 0x000666A0
			public <InitializeComponent>_anonXamlCDataTemplate_78()
			{
			}

			// Token: 0x06000630 RID: 1584 RVA: 0x000684B4 File Offset: 0x000666B4
			internal object LoadDataTemplate()
			{
				Span span;
				VisualDiagnostics.RegisterSourceInfo(span = new Span(), new Uri("PIDScanner\\PIDScannerPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 124, 50);
				BindingExtension bindingExtension;
				VisualDiagnostics.RegisterSourceInfo(bindingExtension = new BindingExtension(), new Uri("PIDScanner\\PIDScannerPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 125, 55);
				Span span2;
				VisualDiagnostics.RegisterSourceInfo(span2 = new Span(), new Uri("PIDScanner\\PIDScannerPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 125, 50);
				FormattedString formattedString;
				VisualDiagnostics.RegisterSourceInfo(formattedString = new FormattedString(), new Uri("PIDScanner\\PIDScannerPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 123, 46);
				Label label;
				VisualDiagnostics.RegisterSourceInfo(label = new Label(), new Uri("PIDScanner\\PIDScannerPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 121, 38);
				Span span3;
				VisualDiagnostics.RegisterSourceInfo(span3 = new Span(), new Uri("PIDScanner\\PIDScannerPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 132, 50);
				BindingExtension bindingExtension2;
				VisualDiagnostics.RegisterSourceInfo(bindingExtension2 = new BindingExtension(), new Uri("PIDScanner\\PIDScannerPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 133, 55);
				Span span4;
				VisualDiagnostics.RegisterSourceInfo(span4 = new Span(), new Uri("PIDScanner\\PIDScannerPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 133, 50);
				FormattedString formattedString2;
				VisualDiagnostics.RegisterSourceInfo(formattedString2 = new FormattedString(), new Uri("PIDScanner\\PIDScannerPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 131, 46);
				Label label2;
				VisualDiagnostics.RegisterSourceInfo(label2 = new Label(), new Uri("PIDScanner\\PIDScannerPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 129, 38);
				Label label3;
				VisualDiagnostics.RegisterSourceInfo(label3 = new Label(), new Uri("PIDScanner\\PIDScannerPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 137, 38);
				StaticResourceExtension staticResourceExtension;
				VisualDiagnostics.RegisterSourceInfo(staticResourceExtension = new StaticResourceExtension(), new Uri("PIDScanner\\PIDScannerPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 138, 74);
				BindingExtension bindingExtension3;
				VisualDiagnostics.RegisterSourceInfo(bindingExtension3 = new BindingExtension(), new Uri("PIDScanner\\PIDScannerPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 138, 74);
				Label label4;
				VisualDiagnostics.RegisterSourceInfo(label4 = new Label(), new Uri("PIDScanner\\PIDScannerPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 138, 38);
				StackLayout stackLayout;
				VisualDiagnostics.RegisterSourceInfo(stackLayout = new StackLayout(), new Uri("PIDScanner\\PIDScannerPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 120, 34);
				ViewCell viewCell;
				VisualDiagnostics.RegisterSourceInfo(viewCell = new ViewCell(), new Uri("PIDScanner\\PIDScannerPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 119, 30);
				NameScope nameScope = new NameScope();
				NameScope.SetNameScope(viewCell, nameScope);
				stackLayout.SetValue(StackLayout.OrientationProperty, 0);
				span.SetValue(Span.FontAttributesProperty, new FontAttributesConverter().ConvertFromInvariantString("Bold"));
				span.SetValue(Span.TextProperty, "Header: ");
				formattedString.Spans.Add(span);
				bindingExtension.Path = "RequestHeader";
				BindingBase bindingBase = bindingExtension.ProvideValue(null);
				span2.SetBinding(Span.TextProperty, bindingBase);
				formattedString.Spans.Add(span2);
				label.SetValue(Label.FormattedTextProperty, formattedString);
				stackLayout.Children.Add(label);
				span3.SetValue(Span.FontAttributesProperty, new FontAttributesConverter().ConvertFromInvariantString("Bold"));
				span3.SetValue(Span.TextProperty, "PID: ");
				formattedString2.Spans.Add(span3);
				bindingExtension2.Path = "RequestPID";
				BindingBase bindingBase2 = bindingExtension2.ProvideValue(null);
				span4.SetBinding(Span.TextProperty, bindingBase2);
				formattedString2.Spans.Add(span4);
				label2.SetValue(Label.FormattedTextProperty, formattedString2);
				stackLayout.Children.Add(label2);
				label3.SetValue(Label.FontAttributesProperty, new FontAttributesConverter().ConvertFromInvariantString("Bold"));
				label3.SetValue(Label.TextProperty, "Data:");
				stackLayout.Children.Add(label3);
				label4.SetValue(Label.LineBreakModeProperty, 2);
				staticResourceExtension.Key = "CarretReturnToNewLineConverter";
				IMarkupExtension markupExtension = staticResourceExtension;
				XamlServiceProvider xamlServiceProvider = new XamlServiceProvider();
				Type typeFromHandle = typeof(IProvideValueTarget);
				int num;
				object[] array = new object[(num = this.parentValues.Length) + 4];
				Array.Copy(this.parentValues, 0, array, 4, num);
				object[] array2 = array;
				array2[0] = bindingExtension3;
				array2[1] = label4;
				array2[2] = stackLayout;
				array2[3] = viewCell;
				object obj;
				xamlServiceProvider.Add(typeFromHandle, obj = new SimpleValueTargetProvider(array2, typeof(BindingExtension).GetRuntimeProperty("Converter"), nameScope));
				xamlServiceProvider.Add(typeof(IReferenceProvider), obj);
				Type typeFromHandle2 = typeof(IXamlTypeResolver);
				XmlNamespaceResolver xmlNamespaceResolver = new XmlNamespaceResolver();
				xmlNamespaceResolver.Add("", "http://xamarin.com/schemas/2014/forms");
				xmlNamespaceResolver.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
				xmlNamespaceResolver.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
				xmlNamespaceResolver.Add("local", "clr-namespace:CarScannerXamarinForms");
				xamlServiceProvider.Add(typeFromHandle2, new XamlTypeResolver(xmlNamespaceResolver, typeof(PIDScannerPage.<InitializeComponent>_anonXamlCDataTemplate_78).GetTypeInfo().Assembly));
				xamlServiceProvider.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(138, 74)));
				object obj2 = markupExtension.ProvideValue(xamlServiceProvider);
				bindingExtension3.Converter = obj2;
				bindingExtension3.Path = "Response";
				BindingBase bindingBase3 = bindingExtension3.ProvideValue(null);
				label4.SetBinding(Label.TextProperty, bindingBase3);
				stackLayout.Children.Add(label4);
				viewCell.View = stackLayout;
				return viewCell;
			}

			// Token: 0x04000521 RID: 1313
			internal object[] parentValues;

			// Token: 0x04000522 RID: 1314
			internal PIDScannerPage root;
		}
	}
}
