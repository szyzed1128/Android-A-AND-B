using System;
using System.CodeDom.Compiler;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using CarScannerXamarinForms.DataRecorder;
using CarScannerXamarinForms.OBD2;
using CarScannerXamarinForms.Settings;
using CarScannerXamarinForms.UserControls;
using Syncfusion.ListView.XForms;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Internals;
using Xamarin.Forms.PlatformConfiguration.iOSSpecific;
using Xamarin.Forms.Xaml;
using Xamarin.Forms.Xaml.Diagnostics;
using Xamarin.Forms.Xaml.Internals;

namespace CarScannerXamarinForms.DTC
{
	// Token: 0x0200054A RID: 1354
	[XamlCompilation(2)]
	[XamlFilePath("DTC\\DTCWizardResultsPage.xaml")]
	public class DTCWizardResultsPage : ContentPage
	{
		// Token: 0x060032A5 RID: 12965 RVA: 0x00230A28 File Offset: 0x0022EC28
		public DTCWizardResultsPage(DTCWizardSetupPage.DTCMode mode, OBDRequest[] commands)
		{
			this.InitializeComponent();
			if (SharedSettings.Current.AdsProductPurchased)
			{
				this.ad.IsVisible = false;
			}
			else
			{
				this.ad.IsVisible = true;
			}
			this.Init(mode, commands);
		}

		// Token: 0x060032A6 RID: 12966 RVA: 0x00230A88 File Offset: 0x0022EC88
		private void Init(DTCWizardSetupPage.DTCMode mode, OBDRequest[] commands)
		{
			this.dtcs = new ConcurrentBag<DTCItemV2>();
			this.commands = commands;
			this.jobStarted = false;
			this.Mode = mode;
			this.QueueLength = commands.Length;
			this.activityFrame.Text = this.QueueLength.ToString();
			if (mode == DTCWizardSetupPage.DTCMode.Read)
			{
				this.lvDTC.ItemsSource = App.OBDReader.CurrentCarData.DTCs;
				if (App.OBDSimulator.IsActive)
				{
					this.lvDTC.ItemsSource = null;
				}
				this.lvDTC.IsVisible = true;
				base.Title = Translate.GetString("DtcPage_btnRead.Content");
			}
			else
			{
				this.lvDTC.IsVisible = false;
				base.Title = Translate.GetString("DtcPage_btnClear.Content");
			}
			if (this.QueueLength == 0 && App.OBDSimulator.IsActive)
			{
				this.activityFrame.Text = ActivityFrame.PLEASE_WAIT_TEXT;
				if (this.Mode != DTCWizardSetupPage.DTCMode.Read)
				{
					App.OBDReader.CurrentCarData.DTCs.Clear();
					return;
				}
			}
			else if (this.QueueLength == 0)
			{
				this.activityFrame.Text = "QueueLength=0";
			}
		}

		// Token: 0x060032A7 RID: 12967 RVA: 0x00230B9C File Offset: 0x0022ED9C
		private void UpdateDTCList()
		{
			bool flag = false;
			List<DTCItemV2> list = new List<DTCItemV2>();
			DTCItemV2 dtcitemV;
			while (App.OBDReader.CurrentCarData.DTCs.TryDequeue(out dtcitemV))
			{
				list.Add(dtcitemV);
			}
			using (List<DTCItemV2>.Enumerator enumerator = list.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					DTCItemV2 dtc = enumerator.Current;
					if ((!SharedSettings.Current.HideDTCWithUncomplitedTests || !dtc.OnlyTestNotComplitedDTC) && (!SharedSettings.Current.HideArchiveDTC || !dtc.IsArchive) && !this.dtcs.ToArray().Any((DTCItemV2 x) => x.Equals(dtc)))
					{
						dtc.LoadDescription();
						this.dtcs.Add(dtc);
						flag = true;
					}
				}
			}
			if (flag)
			{
				if (MainThread.IsMainThread)
				{
					this.lvDTC.ItemsSource = null;
					this.lvDTC.ItemsSource = this.dtcs.ToArray();
					return;
				}
				Device.BeginInvokeOnMainThread(delegate
				{
					this.lvDTC.ItemsSource = null;
					this.lvDTC.ItemsSource = this.dtcs.ToArray();
				});
			}
		}

		// Token: 0x060032A8 RID: 12968 RVA: 0x00230CD0 File Offset: 0x0022EED0
		protected override bool OnBackButtonPressed()
		{
			if (App.OBDSimulator.IsActive)
			{
				this.btnBack_Clicked(this, EventArgs.Empty);
			}
			else if (App.OBDReader.GetQueueCount() > 0)
			{
				this.btnCancel_Clicked(this.btnCancel, EventArgs.Empty);
			}
			else
			{
				this.btnBack_Clicked(this, EventArgs.Empty);
			}
			return true;
		}

		// Token: 0x060032A9 RID: 12969 RVA: 0x00230D24 File Offset: 0x0022EF24
		private async void ContentPage_Appearing(object sender, EventArgs e)
		{
			if (App.OBDSimulator.IsActive)
			{
				this.jobStarted = true;
				await Task.Delay(1500);
				if (this.Mode == DTCWizardSetupPage.DTCMode.Read)
				{
					this.lvDTC.ItemsSource = App.OBDReader.CurrentCarData.DTCs;
					this.FinishRead();
				}
				else
				{
					this.FinishClear();
				}
				this.activityFrame.IsVisible = false;
			}
			else if (!this.jobStarted)
			{
				this.activityFrame.IsVisible = true;
				Progress<string> progress = new Progress<string>();
				this.dtcs = new ConcurrentBag<DTCItemV2>();
				if (this.Mode == DTCWizardSetupPage.DTCMode.Read)
				{
					Device.StartTimer(TimeSpan.FromSeconds(3.0), delegate
					{
						if (this.Mode == DTCWizardSetupPage.DTCMode.Read && this.btnCancel.IsVisible)
						{
							this.UpdateDTCList();
							return true;
						}
						return false;
					});
				}
				progress.ProgressChanged += this.ProgressChanged;
				App.OBDReader.DTCReadingQueueProgress -= this.OBDReader_DTCReadingQueueProgress;
				App.OBDReader.DTCReadingQueueProgress += this.OBDReader_DTCReadingQueueProgress;
				await DTCWorker.Start(this.commands, this.Mode == DTCWizardSetupPage.DTCMode.Read, progress);
				this.jobStarted = true;
			}
		}

		// Token: 0x060032AA RID: 12970 RVA: 0x00230D5C File Offset: 0x0022EF5C
		private void ProgressChanged(object sender, string progress)
		{
			if (SharedSettings.Current.ShowExperimental)
			{
				Device.BeginInvokeOnMainThread(delegate
				{
					this.activityFrame.Text = progress;
				});
			}
		}

		// Token: 0x060032AB RID: 12971 RVA: 0x00230D9C File Offset: 0x0022EF9C
		private void OBDReader_DTCReadingQueueProgress(object sender, int requestsInQueueLeft)
		{
			DTCWizardResultsPage.<>c__DisplayClass11_0 CS$<>8__locals1 = new DTCWizardResultsPage.<>c__DisplayClass11_0();
			CS$<>8__locals1.<>4__this = this;
			CS$<>8__locals1.requestsInQueueLeft = requestsInQueueLeft;
			if (this.jobStarted)
			{
				Device.BeginInvokeOnMainThread(delegate
				{
					DTCWizardResultsPage.<>c__DisplayClass11_0.<<OBDReader_DTCReadingQueueProgress>b__0>d <<OBDReader_DTCReadingQueueProgress>b__0>d;
					<<OBDReader_DTCReadingQueueProgress>b__0>d.<>t__builder = AsyncVoidMethodBuilder.Create();
					<<OBDReader_DTCReadingQueueProgress>b__0>d.<>4__this = CS$<>8__locals1;
					<<OBDReader_DTCReadingQueueProgress>b__0>d.<>1__state = -1;
					<<OBDReader_DTCReadingQueueProgress>b__0>d.<>t__builder.Start<DTCWizardResultsPage.<>c__DisplayClass11_0.<<OBDReader_DTCReadingQueueProgress>b__0>d>(ref <<OBDReader_DTCReadingQueueProgress>b__0>d);
				});
			}
		}

		// Token: 0x060032AC RID: 12972 RVA: 0x00230DD8 File Offset: 0x0022EFD8
		private void FinishRead()
		{
			this.btnCancel.IsVisible = false;
			this.gridFinishButtonsRead.IsVisible = true;
			this.gridFinishButtonsClear.IsVisible = false;
			if (this.dtcs.Count == 0 && App.OBDReader.CurrentCarData.DTCs.IsEmpty)
			{
				this.panelNoDTC.IsVisible = true;
				this.lvDTC.IsVisible = false;
				this.btnExportReport.IsVisible = false;
				return;
			}
			this.UpdateDTCList();
			DataRecorderV2 recorder = App.OBDReader.CurrentCarData.Recorder;
			if (recorder != null)
			{
				recorder.Record(this.dtcs.ToList<DTCItemV2>());
			}
			this.panelNoDTC.IsVisible = false;
			this.lvDTC.IsVisible = true;
			this.btnExportReport.IsVisible = true;
		}

		// Token: 0x060032AD RID: 12973 RVA: 0x00230EA0 File Offset: 0x0022F0A0
		private void FinishClear()
		{
			this.btnCancel.IsVisible = false;
			this.gridFinishButtonsRead.IsVisible = false;
			this.gridFinishButtonsClear.IsVisible = true;
			this.activityFrame.IsVisible = false;
			this.lbFinished.IsVisible = true;
		}

		// Token: 0x060032AE RID: 12974 RVA: 0x00230EE0 File Offset: 0x0022F0E0
		private async void ContentPage_Disappearing(object sender, EventArgs e)
		{
		}

		// Token: 0x060032AF RID: 12975 RVA: 0x00230F10 File Offset: 0x0022F110
		private async void btnCancel_Clicked(object sender, EventArgs e)
		{
			TaskAwaiter<bool> taskAwaiter = base.DisplayAlert(Translate.GetString("ios_StopOperation"), "", Translate.GetString("ios_Yes"), Translate.GetString("ios_No")).GetAwaiter();
			TaskAwaiter<bool> taskAwaiter2;
			if (!taskAwaiter.IsCompleted)
			{
				await taskAwaiter;
				taskAwaiter = taskAwaiter2;
				taskAwaiter2 = default(TaskAwaiter<bool>);
			}
			if (taskAwaiter.GetResult())
			{
				this.btnCancel.IsEnabled = false;
				App.OBDReader.DTCReadingQueueProgress -= this.OBDReader_DTCReadingQueueProgress;
				App.OBDReader.CurrentMode = OBDDataReader.OBDModes.Universal;
				App.OBDReader.ReplaceQueue(new List<OBDRequest>());
				this.FinishRead();
				taskAwaiter = App.OBDReader.CheckECUConnectionWhileRunning(false).GetAwaiter();
				if (!taskAwaiter.IsCompleted)
				{
					await taskAwaiter;
					taskAwaiter = taskAwaiter2;
					taskAwaiter2 = default(TaskAwaiter<bool>);
				}
				if (!taskAwaiter.GetResult())
				{
					this.activityFrame.Text = "ECU disconnected, trying to reconnect.";
					await App.OBDReader.Stop("DTCWizardResult:UserCancelled");
					await App.OBDReader.ReinitializeConnectionToECU("DTCResults: btnCancel");
				}
				this.activityFrame.IsVisible = false;
			}
		}

		// Token: 0x060032B0 RID: 12976 RVA: 0x00230F48 File Offset: 0x0022F148
		private async void btnBack_Clicked(object sender, EventArgs e)
		{
			App.OBDReader.DTCReadingQueueProgress -= this.OBDReader_DTCReadingQueueProgress;
			await base.Navigation.PopAsync(true);
		}

		// Token: 0x060032B1 RID: 12977 RVA: 0x00230F80 File Offset: 0x0022F180
		private void lv_ItemTapped(object sender, ItemTappedEventArgs e)
		{
			if (e.ItemData == null)
			{
				return;
			}
			DTCItemV2 dtcitemV = e.ItemData as DTCItemV2;
			if (dtcitemV != null)
			{
				DTCWorker.GoogleForDTC(dtcitemV.Code);
			}
			this.lvDTC.SelectedItem = null;
		}

		// Token: 0x060032B2 RID: 12978 RVA: 0x00230FBC File Offset: 0x0022F1BC
		private async void btnClear_Clicked(object sender, EventArgs e)
		{
			if (this.lvDTC.ItemsSource == null)
			{
				await this.GoToFullDTClear();
			}
			else
			{
				List<DTCItemV2> list = (this.lvDTC.ItemsSource as IEnumerable<DTCItemV2>).ToList<DTCItemV2>();
				list.Reverse();
				if (list.Count == 0)
				{
					await this.GoToFullDTClear();
				}
				else
				{
					List<OBDRequest[]> clearRequestsCollection = new List<OBDRequest[]>();
					foreach (DTCItemV2 dtcitemV in list)
					{
						OBDRequest[] requestForClear = dtcitemV.GetRequestForClear();
						bool flag = true;
						foreach (OBDRequest[] array in clearRequestsCollection)
						{
							if (array.Length == requestForClear.Length)
							{
								bool flag2 = true;
								for (int i = 0; i < array.Length; i++)
								{
									if (!array[i].Equals(requestForClear[i]))
									{
										flag2 = false;
										break;
									}
								}
								if (flag2)
								{
									flag = false;
									break;
								}
							}
						}
						if (flag)
						{
							clearRequestsCollection.Add(requestForClear);
						}
					}
					if (clearRequestsCollection.Count > 0)
					{
						string action_clear_found = Translate.GetString("dtc_ClearFoundDTC");
						string action_clear_full = Translate.GetString("dtc_ClearFullMode");
						string @string = Translate.GetString("dtc_ClearFoundsQuestion");
						string cancel = Translate.GetString("btnCancel.Content");
						string text = await this.DisplayActionSheetCustom(@string, cancel, null, new string[] { action_clear_found, action_clear_full });
						if (text == cancel)
						{
							return;
						}
						if (text == action_clear_full)
						{
							await this.GoToFullDTClear();
						}
						else if (text == action_clear_found)
						{
							List<OBDRequest> list2 = new List<OBDRequest>();
							foreach (OBDRequest[] array2 in clearRequestsCollection)
							{
								list2.AddRange(array2);
							}
							list2.AddRange(DTCWorker.GetRestoreConnectionCommands());
							this.Init(DTCWizardSetupPage.DTCMode.Clear, list2.ToArray());
							this.ContentPage_Appearing(this, EventArgs.Empty);
						}
						action_clear_found = null;
						action_clear_full = null;
						cancel = null;
					}
					else
					{
						await this.GoToFullDTClear();
					}
					clearRequestsCollection = null;
				}
			}
		}

		// Token: 0x060032B3 RID: 12979 RVA: 0x00230FF4 File Offset: 0x0022F1F4
		private async Task GoToFullDTClear()
		{
			try
			{
				Page page = base.Navigation.NavigationStack.FirstOrDefault((Page x) => x is DTCWizardSetupPage);
				base.Navigation.RemovePage(page);
				DTCWizardSetupPage dtcwizardSetupPage = new DTCWizardSetupPage(DTCWizardSetupPage.DTCMode.Clear);
				base.Navigation.InsertPageBefore(dtcwizardSetupPage, this);
				await base.Navigation.PopAsync();
			}
			catch (Exception)
			{
			}
		}

		// Token: 0x060032B4 RID: 12980 RVA: 0x00231038 File Offset: 0x0022F238
		private async void btnRead_Clicked(object sender, EventArgs e)
		{
			this.btnRead.IsEnabled = false;
			try
			{
				Page page = base.Navigation.NavigationStack.FirstOrDefault((Page x) => x is DTCWizardSetupPage);
				base.Navigation.RemovePage(page);
				DTCWizardSetupPage dtcwizardSetupPage = new DTCWizardSetupPage(DTCWizardSetupPage.DTCMode.Read);
				base.Navigation.InsertPageBefore(dtcwizardSetupPage, this);
				await base.Navigation.PopAsync();
			}
			catch (Exception)
			{
			}
			this.btnRead.IsEnabled = true;
		}

		// Token: 0x060032B5 RID: 12981 RVA: 0x00231070 File Offset: 0x0022F270
		private async void ClearOneLinkButton_Clicked(object sender, EventArgs e)
		{
			DTCItemV2 dtc = (sender as LinkButton).BindingContext as DTCItemV2;
			TaskAwaiter<bool> taskAwaiter = base.DisplayAlert(Translate.GetString("DtcPage_CleanCodes_Title"), string.Format(Translate.GetString("ios_ClearOneECUDTC"), dtc.ECU), "OK", Translate.GetString("btnCancel.Content")).GetAwaiter();
			if (!taskAwaiter.IsCompleted)
			{
				await taskAwaiter;
				TaskAwaiter<bool> taskAwaiter2;
				taskAwaiter = taskAwaiter2;
				taskAwaiter2 = default(TaskAwaiter<bool>);
			}
			if (taskAwaiter.GetResult())
			{
				this.Init(DTCWizardSetupPage.DTCMode.Clear, dtc.GetRequestForClear());
				this.ContentPage_Appearing(this, EventArgs.Empty);
			}
		}

		// Token: 0x060032B6 RID: 12982 RVA: 0x002310B0 File Offset: 0x0022F2B0
		private async void btnExportReport_Clicked(object sender, EventArgs e)
		{
			this.btnExportReport.IsEnabled = false;
			string text = ReportGenerator.CreateReport((IEnumerable<DTCItemV2>)this.lvDTC.ItemsSource);
			try
			{
				await Share.RequestAsync(text);
			}
			catch (Exception)
			{
			}
			this.btnExportReport.IsEnabled = true;
		}

		// Token: 0x060032B7 RID: 12983 RVA: 0x002310E8 File Offset: 0x0022F2E8
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private void InitializeComponent()
		{
			if (ResourceLoader.IsEnabled && ResourceLoader.CanProvideContentFor(new ResourceLoader.ResourceLoadingQuery
			{
				AssemblyName = typeof(DTCWizardResultsPage).GetTypeInfo().Assembly.GetName(),
				ResourcePath = "DTC/DTCWizardResultsPage.xaml",
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
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension = new DynamicResourceExtension(), new Uri("DTC\\DTCWizardResultsPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 12, 5);
			DTCStatusCollectionToStringConverter dtcstatusCollectionToStringConverter;
			VisualDiagnostics.RegisterSourceInfo(dtcstatusCollectionToStringConverter = new DTCStatusCollectionToStringConverter(), new Uri("DTC\\DTCWizardResultsPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 18, 14);
			DTCDescriptionCollectionToStringConverter dtcdescriptionCollectionToStringConverter;
			VisualDiagnostics.RegisterSourceInfo(dtcdescriptionCollectionToStringConverter = new DTCDescriptionCollectionToStringConverter(), new Uri("DTC\\DTCWizardResultsPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 19, 14);
			DTCCodeToStringConverter dtccodeToStringConverter;
			VisualDiagnostics.RegisterSourceInfo(dtccodeToStringConverter = new DTCCodeToStringConverter(), new Uri("DTC\\DTCWizardResultsPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 20, 14);
			EmptyStringToFalseConverter emptyStringToFalseConverter;
			VisualDiagnostics.RegisterSourceInfo(emptyStringToFalseConverter = new EmptyStringToFalseConverter(), new Uri("DTC\\DTCWizardResultsPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 21, 14);
			ResourceDictionary resourceDictionary;
			VisualDiagnostics.RegisterSourceInfo(resourceDictionary = new ResourceDictionary(), new Uri("DTC\\DTCWizardResultsPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 17, 10);
			RowDefinition rowDefinition;
			VisualDiagnostics.RegisterSourceInfo(rowDefinition = new RowDefinition(), new Uri("DTC\\DTCWizardResultsPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 27, 18);
			RowDefinition rowDefinition2;
			VisualDiagnostics.RegisterSourceInfo(rowDefinition2 = new RowDefinition(), new Uri("DTC\\DTCWizardResultsPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 28, 18);
			RowDefinition rowDefinition3;
			VisualDiagnostics.RegisterSourceInfo(rowDefinition3 = new RowDefinition(), new Uri("DTC\\DTCWizardResultsPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 29, 18);
			RowDefinition rowDefinition4;
			VisualDiagnostics.RegisterSourceInfo(rowDefinition4 = new RowDefinition(), new Uri("DTC\\DTCWizardResultsPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 30, 18);
			RowDefinition rowDefinition5;
			VisualDiagnostics.RegisterSourceInfo(rowDefinition5 = new RowDefinition(), new Uri("DTC\\DTCWizardResultsPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 31, 18);
			Translate translate;
			VisualDiagnostics.RegisterSourceInfo(translate = new Translate(), new Uri("DTC\\DTCWizardResultsPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 38, 46);
			Label label;
			VisualDiagnostics.RegisterSourceInfo(label = new Label(), new Uri("DTC\\DTCWizardResultsPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 38, 18);
			Translate translate2;
			VisualDiagnostics.RegisterSourceInfo(translate2 = new Translate(), new Uri("DTC\\DTCWizardResultsPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 39, 50);
			Label label2;
			VisualDiagnostics.RegisterSourceInfo(label2 = new Label(), new Uri("DTC\\DTCWizardResultsPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 39, 18);
			StackLayout stackLayout;
			VisualDiagnostics.RegisterSourceInfo(stackLayout = new StackLayout(), new Uri("DTC\\DTCWizardResultsPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 33, 14);
			Translate translate3;
			VisualDiagnostics.RegisterSourceInfo(translate3 = new Translate(), new Uri("DTC\\DTCWizardResultsPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 47, 17);
			Label label3;
			VisualDiagnostics.RegisterSourceInfo(label3 = new Label(), new Uri("DTC\\DTCWizardResultsPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 41, 14);
			DataTemplate dataTemplate;
			VisualDiagnostics.RegisterSourceInfo(dataTemplate = new DataTemplate(), new Uri("DTC\\DTCWizardResultsPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 56, 22);
			SfListView sfListView;
			VisualDiagnostics.RegisterSourceInfo(sfListView = new SfListView(), new Uri("DTC\\DTCWizardResultsPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 48, 14);
			ActivityFrame activityFrame;
			VisualDiagnostics.RegisterSourceInfo(activityFrame = new ActivityFrame(), new Uri("DTC\\DTCWizardResultsPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 159, 14);
			Translate translate4;
			VisualDiagnostics.RegisterSourceInfo(translate4 = new Translate(), new Uri("DTC\\DTCWizardResultsPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 169, 17);
			Button button;
			VisualDiagnostics.RegisterSourceInfo(button = new Button(), new Uri("DTC\\DTCWizardResultsPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 164, 14);
			ColumnDefinition columnDefinition;
			VisualDiagnostics.RegisterSourceInfo(columnDefinition = new ColumnDefinition(), new Uri("DTC\\DTCWizardResultsPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 175, 22);
			ColumnDefinition columnDefinition2;
			VisualDiagnostics.RegisterSourceInfo(columnDefinition2 = new ColumnDefinition(), new Uri("DTC\\DTCWizardResultsPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 176, 22);
			ColumnDefinition columnDefinition3;
			VisualDiagnostics.RegisterSourceInfo(columnDefinition3 = new ColumnDefinition(), new Uri("DTC\\DTCWizardResultsPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 177, 22);
			Translate translate5;
			VisualDiagnostics.RegisterSourceInfo(translate5 = new Translate(), new Uri("DTC\\DTCWizardResultsPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 183, 21);
			Button button2;
			VisualDiagnostics.RegisterSourceInfo(button2 = new Button(), new Uri("DTC\\DTCWizardResultsPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 179, 18);
			Translate translate6;
			VisualDiagnostics.RegisterSourceInfo(translate6 = new Translate(), new Uri("DTC\\DTCWizardResultsPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 189, 21);
			Button button3;
			VisualDiagnostics.RegisterSourceInfo(button3 = new Button(), new Uri("DTC\\DTCWizardResultsPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 184, 18);
			Translate translate7;
			VisualDiagnostics.RegisterSourceInfo(translate7 = new Translate(), new Uri("DTC\\DTCWizardResultsPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 196, 21);
			Button button4;
			VisualDiagnostics.RegisterSourceInfo(button4 = new Button(), new Uri("DTC\\DTCWizardResultsPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 190, 18);
			Grid grid;
			VisualDiagnostics.RegisterSourceInfo(grid = new Grid(), new Uri("DTC\\DTCWizardResultsPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 170, 14);
			Translate translate8;
			VisualDiagnostics.RegisterSourceInfo(translate8 = new Translate(), new Uri("DTC\\DTCWizardResultsPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 206, 21);
			Button button5;
			VisualDiagnostics.RegisterSourceInfo(button5 = new Button(), new Uri("DTC\\DTCWizardResultsPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 202, 18);
			Translate translate9;
			VisualDiagnostics.RegisterSourceInfo(translate9 = new Translate(), new Uri("DTC\\DTCWizardResultsPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 213, 21);
			Button button6;
			VisualDiagnostics.RegisterSourceInfo(button6 = new Button(), new Uri("DTC\\DTCWizardResultsPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 207, 18);
			Grid grid2;
			VisualDiagnostics.RegisterSourceInfo(grid2 = new Grid(), new Uri("DTC\\DTCWizardResultsPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 198, 14);
			ComplexAdView complexAdView;
			VisualDiagnostics.RegisterSourceInfo(complexAdView = new ComplexAdView(), new Uri("DTC\\DTCWizardResultsPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 215, 14);
			Grid grid3;
			VisualDiagnostics.RegisterSourceInfo(grid3 = new Grid(), new Uri("DTC\\DTCWizardResultsPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 25, 10);
			VisualDiagnostics.RegisterSourceInfo(this, new Uri("DTC\\DTCWizardResultsPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 2, 2);
			NameScope nameScope = NameScope.GetNameScope(this) ?? new NameScope();
			NameScope.SetNameScope(this, nameScope);
			nameScope.RegisterName("panelNoDTC", stackLayout);
			if (stackLayout.StyleId == null)
			{
				stackLayout.StyleId = "panelNoDTC";
			}
			nameScope.RegisterName("labelDeepDTCHint", label2);
			if (label2.StyleId == null)
			{
				label2.StyleId = "labelDeepDTCHint";
			}
			nameScope.RegisterName("lbFinished", label3);
			if (label3.StyleId == null)
			{
				label3.StyleId = "lbFinished";
			}
			nameScope.RegisterName("lvDTC", sfListView);
			if (sfListView.StyleId == null)
			{
				sfListView.StyleId = "lvDTC";
			}
			nameScope.RegisterName("activityFrame", activityFrame);
			if (activityFrame.StyleId == null)
			{
				activityFrame.StyleId = "activityFrame";
			}
			nameScope.RegisterName("btnCancel", button);
			if (button.StyleId == null)
			{
				button.StyleId = "btnCancel";
			}
			nameScope.RegisterName("gridFinishButtonsRead", grid);
			if (grid.StyleId == null)
			{
				grid.StyleId = "gridFinishButtonsRead";
			}
			nameScope.RegisterName("btnExportReport", button3);
			if (button3.StyleId == null)
			{
				button3.StyleId = "btnExportReport";
			}
			nameScope.RegisterName("btnClear", button4);
			if (button4.StyleId == null)
			{
				button4.StyleId = "btnClear";
			}
			nameScope.RegisterName("gridFinishButtonsClear", grid2);
			if (grid2.StyleId == null)
			{
				grid2.StyleId = "gridFinishButtonsClear";
			}
			nameScope.RegisterName("btnRead", button6);
			if (button6.StyleId == null)
			{
				button6.StyleId = "btnRead";
			}
			nameScope.RegisterName("ad", complexAdView);
			if (complexAdView.StyleId == null)
			{
				complexAdView.StyleId = "ad";
			}
			this.panelNoDTC = stackLayout;
			this.labelDeepDTCHint = label2;
			this.lbFinished = label3;
			this.lvDTC = sfListView;
			this.activityFrame = activityFrame;
			this.btnCancel = button;
			this.gridFinishButtonsRead = grid;
			this.btnExportReport = button3;
			this.btnClear = button4;
			this.gridFinishButtonsClear = grid2;
			this.btnRead = button6;
			this.ad = complexAdView;
			this.Resources = resourceDictionary;
			resourceDictionary.Add("DTCStatusCollectionToStringConverter", dtcstatusCollectionToStringConverter);
			resourceDictionary.Add("DTCDescriptionCollectionToStringConverter", dtcdescriptionCollectionToStringConverter);
			resourceDictionary.Add("DTCCodeToStringConverter", dtccodeToStringConverter);
			resourceDictionary.Add("EmptyStringToFalseConverter", emptyStringToFalseConverter);
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
			xmlNamespaceResolver.Add("syncfusion", "clr-namespace:Syncfusion.ListView.XForms;assembly=Syncfusion.SfListView.XForms");
			xamlServiceProvider.Add(typeFromHandle2, new XamlTypeResolver(xmlNamespaceResolver, typeof(DTCWizardResultsPage).GetTypeInfo().Assembly));
			xamlServiceProvider.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(12, 5)));
			DynamicResource dynamicResource = markupExtension.ProvideValue(xamlServiceProvider);
			this.SetDynamicResource(VisualElement.BackgroundColorProperty, dynamicResource.Key);
			this.Disappearing += this.ContentPage_Disappearing;
			this.SetValue(NavigationPage.HasBackButtonProperty, false);
			this.SetValue(NavigationPage.HasNavigationBarProperty, true);
			this.Resources = resourceDictionary;
			grid3.SetValue(View.MarginProperty, new Thickness(5.0, 0.0, 5.0, 0.0));
			rowDefinition.SetValue(RowDefinition.HeightProperty, new GridLengthTypeConverter().ConvertFromInvariantString("*"));
			grid3.GetValue(Grid.RowDefinitionsProperty).Add(rowDefinition);
			rowDefinition2.SetValue(RowDefinition.HeightProperty, new GridLengthTypeConverter().ConvertFromInvariantString("auto"));
			grid3.GetValue(Grid.RowDefinitionsProperty).Add(rowDefinition2);
			rowDefinition3.SetValue(RowDefinition.HeightProperty, new GridLengthTypeConverter().ConvertFromInvariantString("auto"));
			grid3.GetValue(Grid.RowDefinitionsProperty).Add(rowDefinition3);
			rowDefinition4.SetValue(RowDefinition.HeightProperty, new GridLengthTypeConverter().ConvertFromInvariantString("auto"));
			grid3.GetValue(Grid.RowDefinitionsProperty).Add(rowDefinition4);
			rowDefinition5.SetValue(RowDefinition.HeightProperty, new GridLengthTypeConverter().ConvertFromInvariantString("auto"));
			grid3.GetValue(Grid.RowDefinitionsProperty).Add(rowDefinition5);
			stackLayout.SetValue(Grid.RowProperty, 0);
			stackLayout.SetValue(VisualElement.IsVisibleProperty, new VisualElement.VisibilityConverter().ConvertFromInvariantString("False"));
			stackLayout.SetValue(StackLayout.OrientationProperty, 0);
			label.SetValue(Label.FontAttributesProperty, new FontAttributesConverter().ConvertFromInvariantString("Bold"));
			translate.Text = "ios_NoDTC";
			IMarkupExtension markupExtension2 = translate;
			XamlServiceProvider xamlServiceProvider2 = new XamlServiceProvider();
			Type typeFromHandle3 = typeof(IProvideValueTarget);
			object[] array2 = new object[0 + 4];
			array2[0] = label;
			array2[1] = stackLayout;
			array2[2] = grid3;
			array2[3] = this;
			object obj2;
			xamlServiceProvider2.Add(typeFromHandle3, obj2 = new SimpleValueTargetProvider(array2, Label.TextProperty, nameScope));
			xamlServiceProvider2.Add(typeof(IReferenceProvider), obj2);
			Type typeFromHandle4 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver2 = new XmlNamespaceResolver();
			xmlNamespaceResolver2.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver2.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver2.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver2.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver2.Add("syncfusion", "clr-namespace:Syncfusion.ListView.XForms;assembly=Syncfusion.SfListView.XForms");
			xamlServiceProvider2.Add(typeFromHandle4, new XamlTypeResolver(xmlNamespaceResolver2, typeof(DTCWizardResultsPage).GetTypeInfo().Assembly));
			xamlServiceProvider2.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(38, 46)));
			object obj3 = markupExtension2.ProvideValue(xamlServiceProvider2);
			label.Text = obj3;
			stackLayout.Children.Add(label);
			translate2.Text = "ios_NoDTCDeepRecommendation";
			IMarkupExtension markupExtension3 = translate2;
			XamlServiceProvider xamlServiceProvider3 = new XamlServiceProvider();
			Type typeFromHandle5 = typeof(IProvideValueTarget);
			object[] array3 = new object[0 + 4];
			array3[0] = label2;
			array3[1] = stackLayout;
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
			xmlNamespaceResolver3.Add("syncfusion", "clr-namespace:Syncfusion.ListView.XForms;assembly=Syncfusion.SfListView.XForms");
			xamlServiceProvider3.Add(typeFromHandle6, new XamlTypeResolver(xmlNamespaceResolver3, typeof(DTCWizardResultsPage).GetTypeInfo().Assembly));
			xamlServiceProvider3.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(39, 50)));
			object obj5 = markupExtension3.ProvideValue(xamlServiceProvider3);
			label2.Text = obj5;
			stackLayout.Children.Add(label2);
			grid3.Children.Add(stackLayout);
			label3.SetValue(Grid.RowProperty, 0);
			label3.SetValue(View.HorizontalOptionsProperty, LayoutOptions.Center);
			label3.SetValue(Label.HorizontalTextAlignmentProperty, new TextAlignmentConverter().ConvertFromInvariantString("Center"));
			label3.SetValue(VisualElement.IsVisibleProperty, new VisualElement.VisibilityConverter().ConvertFromInvariantString("False"));
			translate3.Text = "ios_DTCClearFinished";
			IMarkupExtension markupExtension4 = translate3;
			XamlServiceProvider xamlServiceProvider4 = new XamlServiceProvider();
			Type typeFromHandle7 = typeof(IProvideValueTarget);
			object[] array4 = new object[0 + 3];
			array4[0] = label3;
			array4[1] = grid3;
			array4[2] = this;
			object obj6;
			xamlServiceProvider4.Add(typeFromHandle7, obj6 = new SimpleValueTargetProvider(array4, Label.TextProperty, nameScope));
			xamlServiceProvider4.Add(typeof(IReferenceProvider), obj6);
			Type typeFromHandle8 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver4 = new XmlNamespaceResolver();
			xmlNamespaceResolver4.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver4.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver4.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver4.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver4.Add("syncfusion", "clr-namespace:Syncfusion.ListView.XForms;assembly=Syncfusion.SfListView.XForms");
			xamlServiceProvider4.Add(typeFromHandle8, new XamlTypeResolver(xmlNamespaceResolver4, typeof(DTCWizardResultsPage).GetTypeInfo().Assembly));
			xamlServiceProvider4.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(47, 17)));
			object obj7 = markupExtension4.ProvideValue(xamlServiceProvider4);
			label3.Text = obj7;
			grid3.Children.Add(label3);
			sfListView.SetValue(Grid.RowProperty, 0);
			sfListView.SetValue(SfListView.AutoFitModeProperty, 2);
			sfListView.SetValue(VisualElement.BackgroundColorProperty, Color.Transparent);
			sfListView.ItemTapped += new ItemTappedEventHandler(this.lv_ItemTapped);
			sfListView.SetValue(SfListView.SelectionBackgroundColorProperty, Color.Transparent);
			IDataTemplate dataTemplate2 = dataTemplate;
			DTCWizardResultsPage.<InitializeComponent>_anonXamlCDataTemplate_32 <InitializeComponent>_anonXamlCDataTemplate_ = new DTCWizardResultsPage.<InitializeComponent>_anonXamlCDataTemplate_32();
			object[] array5 = new object[0 + 4];
			array5[0] = dataTemplate;
			array5[1] = sfListView;
			array5[2] = grid3;
			array5[3] = this;
			<InitializeComponent>_anonXamlCDataTemplate_.parentValues = array5;
			<InitializeComponent>_anonXamlCDataTemplate_.root = this;
			dataTemplate2.LoadTemplate = new Func<object>(<InitializeComponent>_anonXamlCDataTemplate_.LoadDataTemplate);
			sfListView.SetValue(SfListView.ItemTemplateProperty, dataTemplate);
			grid3.Children.Add(sfListView);
			activityFrame.SetValue(Grid.RowProperty, 1);
			activityFrame.SetValue(VisualElement.IsVisibleProperty, new VisualElement.VisibilityConverter().ConvertFromInvariantString("true"));
			activityFrame.SetValue(View.VerticalOptionsProperty, LayoutOptions.Start);
			grid3.Children.Add(activityFrame);
			button.SetValue(Grid.RowProperty, 2);
			button.Clicked += this.btnCancel_Clicked;
			button.SetValue(View.HorizontalOptionsProperty, LayoutOptions.CenterAndExpand);
			translate4.Text = "ios_Cancel";
			IMarkupExtension markupExtension5 = translate4;
			XamlServiceProvider xamlServiceProvider5 = new XamlServiceProvider();
			Type typeFromHandle9 = typeof(IProvideValueTarget);
			object[] array6 = new object[0 + 3];
			array6[0] = button;
			array6[1] = grid3;
			array6[2] = this;
			object obj8;
			xamlServiceProvider5.Add(typeFromHandle9, obj8 = new SimpleValueTargetProvider(array6, Button.TextProperty, nameScope));
			xamlServiceProvider5.Add(typeof(IReferenceProvider), obj8);
			Type typeFromHandle10 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver5 = new XmlNamespaceResolver();
			xmlNamespaceResolver5.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver5.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver5.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver5.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver5.Add("syncfusion", "clr-namespace:Syncfusion.ListView.XForms;assembly=Syncfusion.SfListView.XForms");
			xamlServiceProvider5.Add(typeFromHandle10, new XamlTypeResolver(xmlNamespaceResolver5, typeof(DTCWizardResultsPage).GetTypeInfo().Assembly));
			xamlServiceProvider5.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(169, 17)));
			object obj9 = markupExtension5.ProvideValue(xamlServiceProvider5);
			button.Text = obj9;
			grid3.Children.Add(button);
			grid.SetValue(Grid.RowProperty, 2);
			grid.SetValue(VisualElement.IsVisibleProperty, new VisualElement.VisibilityConverter().ConvertFromInvariantString("False"));
			columnDefinition.SetValue(ColumnDefinition.WidthProperty, new GridLengthTypeConverter().ConvertFromInvariantString("*"));
			grid.GetValue(Grid.ColumnDefinitionsProperty).Add(columnDefinition);
			columnDefinition2.SetValue(ColumnDefinition.WidthProperty, new GridLengthTypeConverter().ConvertFromInvariantString("*"));
			grid.GetValue(Grid.ColumnDefinitionsProperty).Add(columnDefinition2);
			columnDefinition3.SetValue(ColumnDefinition.WidthProperty, new GridLengthTypeConverter().ConvertFromInvariantString("*"));
			grid.GetValue(Grid.ColumnDefinitionsProperty).Add(columnDefinition3);
			button2.SetValue(Grid.ColumnProperty, 0);
			button2.Clicked += this.btnBack_Clicked;
			button2.SetValue(View.HorizontalOptionsProperty, LayoutOptions.Start);
			translate5.Text = "ios_Back2";
			IMarkupExtension markupExtension6 = translate5;
			XamlServiceProvider xamlServiceProvider6 = new XamlServiceProvider();
			Type typeFromHandle11 = typeof(IProvideValueTarget);
			object[] array7 = new object[0 + 4];
			array7[0] = button2;
			array7[1] = grid;
			array7[2] = grid3;
			array7[3] = this;
			object obj10;
			xamlServiceProvider6.Add(typeFromHandle11, obj10 = new SimpleValueTargetProvider(array7, Button.TextProperty, nameScope));
			xamlServiceProvider6.Add(typeof(IReferenceProvider), obj10);
			Type typeFromHandle12 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver6 = new XmlNamespaceResolver();
			xmlNamespaceResolver6.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver6.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver6.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver6.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver6.Add("syncfusion", "clr-namespace:Syncfusion.ListView.XForms;assembly=Syncfusion.SfListView.XForms");
			xamlServiceProvider6.Add(typeFromHandle12, new XamlTypeResolver(xmlNamespaceResolver6, typeof(DTCWizardResultsPage).GetTypeInfo().Assembly));
			xamlServiceProvider6.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(183, 21)));
			object obj11 = markupExtension6.ProvideValue(xamlServiceProvider6);
			button2.Text = obj11;
			grid.Children.Add(button2);
			button3.SetValue(Grid.ColumnProperty, 1);
			button3.Clicked += this.btnExportReport_Clicked;
			button3.SetValue(VisualElement.IsVisibleProperty, new VisualElement.VisibilityConverter().ConvertFromInvariantString("False"));
			translate6.Text = "dtc_ExportReport";
			IMarkupExtension markupExtension7 = translate6;
			XamlServiceProvider xamlServiceProvider7 = new XamlServiceProvider();
			Type typeFromHandle13 = typeof(IProvideValueTarget);
			object[] array8 = new object[0 + 4];
			array8[0] = button3;
			array8[1] = grid;
			array8[2] = grid3;
			array8[3] = this;
			object obj12;
			xamlServiceProvider7.Add(typeFromHandle13, obj12 = new SimpleValueTargetProvider(array8, Button.TextProperty, nameScope));
			xamlServiceProvider7.Add(typeof(IReferenceProvider), obj12);
			Type typeFromHandle14 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver7 = new XmlNamespaceResolver();
			xmlNamespaceResolver7.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver7.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver7.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver7.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver7.Add("syncfusion", "clr-namespace:Syncfusion.ListView.XForms;assembly=Syncfusion.SfListView.XForms");
			xamlServiceProvider7.Add(typeFromHandle14, new XamlTypeResolver(xmlNamespaceResolver7, typeof(DTCWizardResultsPage).GetTypeInfo().Assembly));
			xamlServiceProvider7.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(189, 21)));
			object obj13 = markupExtension7.ProvideValue(xamlServiceProvider7);
			button3.Text = obj13;
			grid.Children.Add(button3);
			button4.SetValue(Grid.ColumnProperty, 2);
			button4.Clicked += this.btnClear_Clicked;
			button4.SetValue(View.HorizontalOptionsProperty, LayoutOptions.End);
			button4.SetValue(VisualElement.IsVisibleProperty, new VisualElement.VisibilityConverter().ConvertFromInvariantString("true"));
			translate7.Text = "DtcPage_btnClear.Content";
			IMarkupExtension markupExtension8 = translate7;
			XamlServiceProvider xamlServiceProvider8 = new XamlServiceProvider();
			Type typeFromHandle15 = typeof(IProvideValueTarget);
			object[] array9 = new object[0 + 4];
			array9[0] = button4;
			array9[1] = grid;
			array9[2] = grid3;
			array9[3] = this;
			object obj14;
			xamlServiceProvider8.Add(typeFromHandle15, obj14 = new SimpleValueTargetProvider(array9, Button.TextProperty, nameScope));
			xamlServiceProvider8.Add(typeof(IReferenceProvider), obj14);
			Type typeFromHandle16 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver8 = new XmlNamespaceResolver();
			xmlNamespaceResolver8.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver8.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver8.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver8.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver8.Add("syncfusion", "clr-namespace:Syncfusion.ListView.XForms;assembly=Syncfusion.SfListView.XForms");
			xamlServiceProvider8.Add(typeFromHandle16, new XamlTypeResolver(xmlNamespaceResolver8, typeof(DTCWizardResultsPage).GetTypeInfo().Assembly));
			xamlServiceProvider8.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(196, 21)));
			object obj15 = markupExtension8.ProvideValue(xamlServiceProvider8);
			button4.Text = obj15;
			grid.Children.Add(button4);
			grid3.Children.Add(grid);
			grid2.SetValue(Grid.RowProperty, 2);
			grid2.SetValue(VisualElement.IsVisibleProperty, new VisualElement.VisibilityConverter().ConvertFromInvariantString("False"));
			button5.SetValue(Grid.ColumnProperty, 0);
			button5.Clicked += this.btnBack_Clicked;
			button5.SetValue(View.HorizontalOptionsProperty, LayoutOptions.Start);
			translate8.Text = "ios_Back2";
			IMarkupExtension markupExtension9 = translate8;
			XamlServiceProvider xamlServiceProvider9 = new XamlServiceProvider();
			Type typeFromHandle17 = typeof(IProvideValueTarget);
			object[] array10 = new object[0 + 4];
			array10[0] = button5;
			array10[1] = grid2;
			array10[2] = grid3;
			array10[3] = this;
			object obj16;
			xamlServiceProvider9.Add(typeFromHandle17, obj16 = new SimpleValueTargetProvider(array10, Button.TextProperty, nameScope));
			xamlServiceProvider9.Add(typeof(IReferenceProvider), obj16);
			Type typeFromHandle18 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver9 = new XmlNamespaceResolver();
			xmlNamespaceResolver9.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver9.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver9.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver9.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver9.Add("syncfusion", "clr-namespace:Syncfusion.ListView.XForms;assembly=Syncfusion.SfListView.XForms");
			xamlServiceProvider9.Add(typeFromHandle18, new XamlTypeResolver(xmlNamespaceResolver9, typeof(DTCWizardResultsPage).GetTypeInfo().Assembly));
			xamlServiceProvider9.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(206, 21)));
			object obj17 = markupExtension9.ProvideValue(xamlServiceProvider9);
			button5.Text = obj17;
			grid2.Children.Add(button5);
			button6.SetValue(Grid.ColumnProperty, 1);
			button6.Clicked += this.btnRead_Clicked;
			button6.SetValue(View.HorizontalOptionsProperty, LayoutOptions.End);
			button6.SetValue(VisualElement.IsVisibleProperty, new VisualElement.VisibilityConverter().ConvertFromInvariantString("true"));
			translate9.Text = "DtcPage_btnRead.Content";
			IMarkupExtension markupExtension10 = translate9;
			XamlServiceProvider xamlServiceProvider10 = new XamlServiceProvider();
			Type typeFromHandle19 = typeof(IProvideValueTarget);
			object[] array11 = new object[0 + 4];
			array11[0] = button6;
			array11[1] = grid2;
			array11[2] = grid3;
			array11[3] = this;
			object obj18;
			xamlServiceProvider10.Add(typeFromHandle19, obj18 = new SimpleValueTargetProvider(array11, Button.TextProperty, nameScope));
			xamlServiceProvider10.Add(typeof(IReferenceProvider), obj18);
			Type typeFromHandle20 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver10 = new XmlNamespaceResolver();
			xmlNamespaceResolver10.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver10.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver10.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver10.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver10.Add("syncfusion", "clr-namespace:Syncfusion.ListView.XForms;assembly=Syncfusion.SfListView.XForms");
			xamlServiceProvider10.Add(typeFromHandle20, new XamlTypeResolver(xmlNamespaceResolver10, typeof(DTCWizardResultsPage).GetTypeInfo().Assembly));
			xamlServiceProvider10.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(213, 21)));
			object obj19 = markupExtension10.ProvideValue(xamlServiceProvider10);
			button6.Text = obj19;
			grid2.Children.Add(button6);
			grid3.Children.Add(grid2);
			complexAdView.SetValue(Grid.RowProperty, 4);
			complexAdView.SetValue(View.MarginProperty, new Thickness(0.0, 0.0, 0.0, 0.0));
			complexAdView.SetValue(VisualElement.HeightRequestProperty, 55.0);
			complexAdView.SetValue(VisualElement.IsVisibleProperty, new VisualElement.VisibilityConverter().ConvertFromInvariantString("true"));
			complexAdView.SetValue(View.VerticalOptionsProperty, LayoutOptions.End);
			grid3.Children.Add(complexAdView);
			this.SetValue(ContentPage.ContentProperty, grid3);
		}

		// Token: 0x060032B8 RID: 12984 RVA: 0x00232C9A File Offset: 0x00230E9A
		[CompilerGenerated]
		private void <UpdateDTCList>b__3_0()
		{
			this.lvDTC.ItemsSource = null;
			this.lvDTC.ItemsSource = this.dtcs.ToArray();
		}

		// Token: 0x060032B9 RID: 12985 RVA: 0x00232CBE File Offset: 0x00230EBE
		[CompilerGenerated]
		private bool <ContentPage_Appearing>b__9_0()
		{
			if (this.Mode == DTCWizardSetupPage.DTCMode.Read && this.btnCancel.IsVisible)
			{
				this.UpdateDTCList();
				return true;
			}
			return false;
		}

		// Token: 0x060032BA RID: 12986 RVA: 0x00232CE0 File Offset: 0x00230EE0
		private void __InitComponentRuntime()
		{
			Extensions.LoadFromXaml<DTCWizardResultsPage>(this, typeof(DTCWizardResultsPage));
			this.panelNoDTC = NameScopeExtensions.FindByName<StackLayout>(this, "panelNoDTC");
			this.labelDeepDTCHint = NameScopeExtensions.FindByName<Label>(this, "labelDeepDTCHint");
			this.lbFinished = NameScopeExtensions.FindByName<Label>(this, "lbFinished");
			this.lvDTC = NameScopeExtensions.FindByName<SfListView>(this, "lvDTC");
			this.activityFrame = NameScopeExtensions.FindByName<ActivityFrame>(this, "activityFrame");
			this.btnCancel = NameScopeExtensions.FindByName<Button>(this, "btnCancel");
			this.gridFinishButtonsRead = NameScopeExtensions.FindByName<Grid>(this, "gridFinishButtonsRead");
			this.btnExportReport = NameScopeExtensions.FindByName<Button>(this, "btnExportReport");
			this.btnClear = NameScopeExtensions.FindByName<Button>(this, "btnClear");
			this.gridFinishButtonsClear = NameScopeExtensions.FindByName<Grid>(this, "gridFinishButtonsClear");
			this.btnRead = NameScopeExtensions.FindByName<Button>(this, "btnRead");
			this.ad = NameScopeExtensions.FindByName<ComplexAdView>(this, "ad");
		}

		// Token: 0x04001D76 RID: 7542
		private ConcurrentBag<DTCItemV2> dtcs = new ConcurrentBag<DTCItemV2>();

		// Token: 0x04001D77 RID: 7543
		private OBDRequest[] commands = new OBDRequest[0];

		// Token: 0x04001D78 RID: 7544
		private bool jobStarted;

		// Token: 0x04001D79 RID: 7545
		private int QueueLength;

		// Token: 0x04001D7A RID: 7546
		private DTCWizardSetupPage.DTCMode Mode;

		// Token: 0x04001D7B RID: 7547
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private StackLayout panelNoDTC;

		// Token: 0x04001D7C RID: 7548
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private Label labelDeepDTCHint;

		// Token: 0x04001D7D RID: 7549
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private Label lbFinished;

		// Token: 0x04001D7E RID: 7550
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private SfListView lvDTC;

		// Token: 0x04001D7F RID: 7551
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private ActivityFrame activityFrame;

		// Token: 0x04001D80 RID: 7552
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private Button btnCancel;

		// Token: 0x04001D81 RID: 7553
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private Grid gridFinishButtonsRead;

		// Token: 0x04001D82 RID: 7554
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private Button btnExportReport;

		// Token: 0x04001D83 RID: 7555
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private Button btnClear;

		// Token: 0x04001D84 RID: 7556
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private Grid gridFinishButtonsClear;

		// Token: 0x04001D85 RID: 7557
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private Button btnRead;

		// Token: 0x04001D86 RID: 7558
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private ComplexAdView ad;

		// Token: 0x0200054B RID: 1355
		[CompilerGenerated]
		[Serializable]
		private sealed class <>c
		{
			// Token: 0x060032BB RID: 12987 RVA: 0x00232DCA File Offset: 0x00230FCA
			// Note: this type is marked as 'beforefieldinit'.
			static <>c()
			{
			}

			// Token: 0x060032BC RID: 12988 RVA: 0x00002050 File Offset: 0x00000250
			public <>c()
			{
			}

			// Token: 0x060032BD RID: 12989 RVA: 0x00232DD6 File Offset: 0x00230FD6
			internal bool <GoToFullDTClear>b__19_0(Page x)
			{
				return x is DTCWizardSetupPage;
			}

			// Token: 0x060032BE RID: 12990 RVA: 0x00232DD6 File Offset: 0x00230FD6
			internal bool <btnRead_Clicked>b__20_0(Page x)
			{
				return x is DTCWizardSetupPage;
			}

			// Token: 0x04001D87 RID: 7559
			public static readonly DTCWizardResultsPage.<>c <>9 = new DTCWizardResultsPage.<>c();

			// Token: 0x04001D88 RID: 7560
			public static Func<Page, bool> <>9__19_0;

			// Token: 0x04001D89 RID: 7561
			public static Func<Page, bool> <>9__20_0;
		}

		// Token: 0x0200054C RID: 1356
		[CompilerGenerated]
		private sealed class <>c__DisplayClass10_0
		{
			// Token: 0x060032BF RID: 12991 RVA: 0x00002050 File Offset: 0x00000250
			public <>c__DisplayClass10_0()
			{
			}

			// Token: 0x060032C0 RID: 12992 RVA: 0x00232DE1 File Offset: 0x00230FE1
			internal void <ProgressChanged>b__0()
			{
				this.<>4__this.activityFrame.Text = this.progress;
			}

			// Token: 0x04001D8A RID: 7562
			public DTCWizardResultsPage <>4__this;

			// Token: 0x04001D8B RID: 7563
			public string progress;
		}

		// Token: 0x0200054D RID: 1357
		[CompilerGenerated]
		private sealed class <>c__DisplayClass11_0
		{
			// Token: 0x060032C1 RID: 12993 RVA: 0x00002050 File Offset: 0x00000250
			public <>c__DisplayClass11_0()
			{
			}

			// Token: 0x060032C2 RID: 12994 RVA: 0x00232DFC File Offset: 0x00230FFC
			internal async void <OBDReader_DTCReadingQueueProgress>b__0()
			{
				try
				{
					double num = (double)(this.<>4__this.QueueLength - this.requestsInQueueLeft) / (double)this.<>4__this.QueueLength * 100.0;
					this.<>4__this.activityFrame.Text = num.ToString("0") + "% [" + this.requestsInQueueLeft.ToString() + "]";
					if (this.requestsInQueueLeft < 1)
					{
						App.OBDReader.CurrentMode = OBDDataReader.OBDModes.Universal;
						this.<>4__this.ProgressChanged(this.<>4__this, "Finished! Checking ECU connection");
						TaskAwaiter<bool> taskAwaiter = App.OBDReader.CheckECUConnectionWhileRunning(false).GetAwaiter();
						if (!taskAwaiter.IsCompleted)
						{
							await taskAwaiter;
							TaskAwaiter<bool> taskAwaiter2;
							taskAwaiter = taskAwaiter2;
							taskAwaiter2 = default(TaskAwaiter<bool>);
						}
						if (!taskAwaiter.GetResult())
						{
							this.<>4__this.ProgressChanged(this.<>4__this, "ECU disconnected, trying to reconnect.");
							await App.OBDReader.Stop("DTCWizardResults: ECU disconnected, trying to reconnect");
							await App.OBDReader.ReinitializeConnectionToECU("DTCResults finishing queue #163");
							this.<>4__this.ProgressChanged(this.<>4__this, "Reconnected");
						}
						this.<>4__this.activityFrame.IsVisible = false;
						if (this.<>4__this.Mode == DTCWizardSetupPage.DTCMode.Read)
						{
							this.<>4__this.FinishRead();
						}
						else
						{
							this.<>4__this.FinishClear();
						}
						App.OBDReader.DTCReadingQueueProgress -= this.<>4__this.OBDReader_DTCReadingQueueProgress;
						NavigationPage.SetHasBackButton(this.<>4__this, true);
						NavigationPage.SetBackButtonTitle(this.<>4__this, Translate.GetString("ios_Back2"));
					}
				}
				catch (Exception)
				{
				}
			}

			// Token: 0x04001D8C RID: 7564
			public DTCWizardResultsPage <>4__this;

			// Token: 0x04001D8D RID: 7565
			public int requestsInQueueLeft;

			// Token: 0x0200054E RID: 1358
			[StructLayout(LayoutKind.Auto)]
			private struct <<OBDReader_DTCReadingQueueProgress>b__0>d : IAsyncStateMachine
			{
				// Token: 0x060032C3 RID: 12995 RVA: 0x00232E34 File Offset: 0x00231034
				void IAsyncStateMachine.MoveNext()
				{
					int num2;
					int num = num2;
					DTCWizardResultsPage.<>c__DisplayClass11_0 CS$<>8__locals1 = this;
					try
					{
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
								goto IL_018E;
							}
							case 2:
								taskAwaiter3 = taskAwaiter2;
								taskAwaiter2 = default(TaskAwaiter<bool>);
								num2 = -1;
								goto IL_01F2;
							default:
							{
								double num3 = (double)(CS$<>8__locals1.<>4__this.QueueLength - CS$<>8__locals1.requestsInQueueLeft) / (double)CS$<>8__locals1.<>4__this.QueueLength * 100.0;
								CS$<>8__locals1.<>4__this.activityFrame.Text = num3.ToString("0") + "% [" + CS$<>8__locals1.requestsInQueueLeft.ToString() + "]";
								if (CS$<>8__locals1.requestsInQueueLeft >= 1)
								{
									goto IL_0282;
								}
								App.OBDReader.CurrentMode = OBDDataReader.OBDModes.Universal;
								CS$<>8__locals1.<>4__this.ProgressChanged(CS$<>8__locals1.<>4__this, "Finished! Checking ECU connection");
								taskAwaiter3 = App.OBDReader.CheckECUConnectionWhileRunning(false).GetAwaiter();
								if (!taskAwaiter3.IsCompleted)
								{
									num2 = 0;
									taskAwaiter2 = taskAwaiter3;
									this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<bool>, DTCWizardResultsPage.<>c__DisplayClass11_0.<<OBDReader_DTCReadingQueueProgress>b__0>d>(ref taskAwaiter3, ref this);
									return;
								}
								break;
							}
							}
							if (taskAwaiter3.GetResult())
							{
								goto IL_0210;
							}
							CS$<>8__locals1.<>4__this.ProgressChanged(CS$<>8__locals1.<>4__this, "ECU disconnected, trying to reconnect.");
							taskAwaiter4 = App.OBDReader.Stop("DTCWizardResults: ECU disconnected, trying to reconnect").GetAwaiter();
							if (!taskAwaiter4.IsCompleted)
							{
								num2 = 1;
								TaskAwaiter taskAwaiter5 = taskAwaiter4;
								this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, DTCWizardResultsPage.<>c__DisplayClass11_0.<<OBDReader_DTCReadingQueueProgress>b__0>d>(ref taskAwaiter4, ref this);
								return;
							}
							IL_018E:
							taskAwaiter4.GetResult();
							taskAwaiter3 = App.OBDReader.ReinitializeConnectionToECU("DTCResults finishing queue #163").GetAwaiter();
							if (!taskAwaiter3.IsCompleted)
							{
								num2 = 2;
								taskAwaiter2 = taskAwaiter3;
								this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<bool>, DTCWizardResultsPage.<>c__DisplayClass11_0.<<OBDReader_DTCReadingQueueProgress>b__0>d>(ref taskAwaiter3, ref this);
								return;
							}
							IL_01F2:
							taskAwaiter3.GetResult();
							CS$<>8__locals1.<>4__this.ProgressChanged(CS$<>8__locals1.<>4__this, "Reconnected");
							IL_0210:
							CS$<>8__locals1.<>4__this.activityFrame.IsVisible = false;
							if (CS$<>8__locals1.<>4__this.Mode == DTCWizardSetupPage.DTCMode.Read)
							{
								CS$<>8__locals1.<>4__this.FinishRead();
							}
							else
							{
								CS$<>8__locals1.<>4__this.FinishClear();
							}
							App.OBDReader.DTCReadingQueueProgress -= CS$<>8__locals1.<>4__this.OBDReader_DTCReadingQueueProgress;
							NavigationPage.SetHasBackButton(CS$<>8__locals1.<>4__this, true);
							NavigationPage.SetBackButtonTitle(CS$<>8__locals1.<>4__this, Translate.GetString("ios_Back2"));
							IL_0282:;
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

				// Token: 0x060032C4 RID: 12996 RVA: 0x0023312C File Offset: 0x0023132C
				[DebuggerHidden]
				void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
				{
					this.<>t__builder.SetStateMachine(stateMachine);
				}

				// Token: 0x04001D8E RID: 7566
				public int <>1__state;

				// Token: 0x04001D8F RID: 7567
				public AsyncVoidMethodBuilder <>t__builder;

				// Token: 0x04001D90 RID: 7568
				public DTCWizardResultsPage.<>c__DisplayClass11_0 <>4__this;

				// Token: 0x04001D91 RID: 7569
				private TaskAwaiter<bool> <>u__1;

				// Token: 0x04001D92 RID: 7570
				private TaskAwaiter <>u__2;
			}
		}

		// Token: 0x0200054F RID: 1359
		[CompilerGenerated]
		private sealed class <>c__DisplayClass3_0
		{
			// Token: 0x060032C5 RID: 12997 RVA: 0x00002050 File Offset: 0x00000250
			public <>c__DisplayClass3_0()
			{
			}

			// Token: 0x060032C6 RID: 12998 RVA: 0x0023313A File Offset: 0x0023133A
			internal bool <UpdateDTCList>b__1(DTCItemV2 x)
			{
				return x.Equals(this.dtc);
			}

			// Token: 0x04001D93 RID: 7571
			public DTCItemV2 dtc;
		}

		// Token: 0x02000550 RID: 1360
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <ClearOneLinkButton_Clicked>d__21 : IAsyncStateMachine
		{
			// Token: 0x060032C7 RID: 12999 RVA: 0x00233148 File Offset: 0x00231348
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				DTCWizardResultsPage dtcwizardResultsPage = this;
				try
				{
					TaskAwaiter<bool> taskAwaiter3;
					if (num != 0)
					{
						dtc = (sender as LinkButton).BindingContext as DTCItemV2;
						taskAwaiter3 = dtcwizardResultsPage.DisplayAlert(Translate.GetString("DtcPage_CleanCodes_Title"), string.Format(Translate.GetString("ios_ClearOneECUDTC"), dtc.ECU), "OK", Translate.GetString("btnCancel.Content")).GetAwaiter();
						if (!taskAwaiter3.IsCompleted)
						{
							num2 = 0;
							taskAwaiter2 = taskAwaiter3;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<bool>, DTCWizardResultsPage.<ClearOneLinkButton_Clicked>d__21>(ref taskAwaiter3, ref this);
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
						OBDRequest[] requestForClear = dtc.GetRequestForClear();
						dtcwizardResultsPage.Init(DTCWizardSetupPage.DTCMode.Clear, requestForClear);
						dtcwizardResultsPage.ContentPage_Appearing(dtcwizardResultsPage, EventArgs.Empty);
					}
				}
				catch (Exception ex)
				{
					num2 = -2;
					dtc = null;
					this.<>t__builder.SetException(ex);
					return;
				}
				num2 = -2;
				dtc = null;
				this.<>t__builder.SetResult();
			}

			// Token: 0x060032C8 RID: 13000 RVA: 0x00233280 File Offset: 0x00231480
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x04001D94 RID: 7572
			public int <>1__state;

			// Token: 0x04001D95 RID: 7573
			public AsyncVoidMethodBuilder <>t__builder;

			// Token: 0x04001D96 RID: 7574
			public object sender;

			// Token: 0x04001D97 RID: 7575
			public DTCWizardResultsPage <>4__this;

			// Token: 0x04001D98 RID: 7576
			private DTCItemV2 <dtc>5__2;

			// Token: 0x04001D99 RID: 7577
			private TaskAwaiter<bool> <>u__1;
		}

		// Token: 0x02000551 RID: 1361
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <ContentPage_Appearing>d__9 : IAsyncStateMachine
		{
			// Token: 0x060032C9 RID: 13001 RVA: 0x00233290 File Offset: 0x00231490
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				DTCWizardResultsPage dtcwizardResultsPage = this;
				try
				{
					TaskAwaiter taskAwaiter;
					TaskAwaiter taskAwaiter2;
					if (num != 0)
					{
						if (num != 1)
						{
							if (App.OBDSimulator.IsActive)
							{
								dtcwizardResultsPage.jobStarted = true;
								taskAwaiter = Task.Delay(1500).GetAwaiter();
								if (!taskAwaiter.IsCompleted)
								{
									num2 = 0;
									taskAwaiter2 = taskAwaiter;
									this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, DTCWizardResultsPage.<ContentPage_Appearing>d__9>(ref taskAwaiter, ref this);
									return;
								}
								goto IL_0086;
							}
							else
							{
								if (dtcwizardResultsPage.jobStarted)
								{
									goto IL_01C9;
								}
								dtcwizardResultsPage.activityFrame.IsVisible = true;
								Progress<string> progress = new Progress<string>();
								dtcwizardResultsPage.dtcs = new ConcurrentBag<DTCItemV2>();
								if (dtcwizardResultsPage.Mode == DTCWizardSetupPage.DTCMode.Read)
								{
									Device.StartTimer(TimeSpan.FromSeconds(3.0), delegate
									{
										if (dtcwizardResultsPage.Mode == DTCWizardSetupPage.DTCMode.Read && dtcwizardResultsPage.btnCancel.IsVisible)
										{
											base.UpdateDTCList();
											return true;
										}
										return false;
									});
								}
								progress.ProgressChanged += dtcwizardResultsPage.ProgressChanged;
								App.OBDReader.DTCReadingQueueProgress -= dtcwizardResultsPage.OBDReader_DTCReadingQueueProgress;
								App.OBDReader.DTCReadingQueueProgress += dtcwizardResultsPage.OBDReader_DTCReadingQueueProgress;
								taskAwaiter = DTCWorker.Start(dtcwizardResultsPage.commands, dtcwizardResultsPage.Mode == DTCWizardSetupPage.DTCMode.Read, progress).GetAwaiter();
								if (!taskAwaiter.IsCompleted)
								{
									num2 = 1;
									taskAwaiter2 = taskAwaiter;
									this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, DTCWizardResultsPage.<ContentPage_Appearing>d__9>(ref taskAwaiter, ref this);
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
						dtcwizardResultsPage.jobStarted = true;
						goto IL_01C9;
					}
					taskAwaiter = taskAwaiter2;
					taskAwaiter2 = default(TaskAwaiter);
					num2 = -1;
					IL_0086:
					taskAwaiter.GetResult();
					if (dtcwizardResultsPage.Mode == DTCWizardSetupPage.DTCMode.Read)
					{
						dtcwizardResultsPage.lvDTC.ItemsSource = App.OBDReader.CurrentCarData.DTCs;
						dtcwizardResultsPage.FinishRead();
					}
					else
					{
						dtcwizardResultsPage.FinishClear();
					}
					dtcwizardResultsPage.activityFrame.IsVisible = false;
					IL_01C9:;
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

			// Token: 0x060032CA RID: 13002 RVA: 0x002334B0 File Offset: 0x002316B0
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x04001D9A RID: 7578
			public int <>1__state;

			// Token: 0x04001D9B RID: 7579
			public AsyncVoidMethodBuilder <>t__builder;

			// Token: 0x04001D9C RID: 7580
			public DTCWizardResultsPage <>4__this;

			// Token: 0x04001D9D RID: 7581
			private TaskAwaiter <>u__1;
		}

		// Token: 0x02000552 RID: 1362
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <ContentPage_Disappearing>d__14 : IAsyncStateMachine
		{
			// Token: 0x060032CB RID: 13003 RVA: 0x002334C0 File Offset: 0x002316C0
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

			// Token: 0x060032CC RID: 13004 RVA: 0x0023350C File Offset: 0x0023170C
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x04001D9E RID: 7582
			public int <>1__state;

			// Token: 0x04001D9F RID: 7583
			public AsyncVoidMethodBuilder <>t__builder;
		}

		// Token: 0x02000553 RID: 1363
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <GoToFullDTClear>d__19 : IAsyncStateMachine
		{
			// Token: 0x060032CD RID: 13005 RVA: 0x0023351C File Offset: 0x0023171C
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				DTCWizardResultsPage dtcwizardResultsPage = this;
				try
				{
					try
					{
						TaskAwaiter<Page> taskAwaiter;
						if (num != 0)
						{
							Page page = dtcwizardResultsPage.Navigation.NavigationStack.FirstOrDefault((Page x) => x is DTCWizardSetupPage);
							dtcwizardResultsPage.Navigation.RemovePage(page);
							DTCWizardSetupPage dtcwizardSetupPage = new DTCWizardSetupPage(DTCWizardSetupPage.DTCMode.Clear);
							dtcwizardResultsPage.Navigation.InsertPageBefore(dtcwizardSetupPage, dtcwizardResultsPage);
							taskAwaiter = dtcwizardResultsPage.Navigation.PopAsync().GetAwaiter();
							if (!taskAwaiter.IsCompleted)
							{
								num2 = 0;
								TaskAwaiter<Page> taskAwaiter2 = taskAwaiter;
								this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<Page>, DTCWizardResultsPage.<GoToFullDTClear>d__19>(ref taskAwaiter, ref this);
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

			// Token: 0x060032CE RID: 13006 RVA: 0x00233640 File Offset: 0x00231840
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x04001DA0 RID: 7584
			public int <>1__state;

			// Token: 0x04001DA1 RID: 7585
			public AsyncTaskMethodBuilder <>t__builder;

			// Token: 0x04001DA2 RID: 7586
			public DTCWizardResultsPage <>4__this;

			// Token: 0x04001DA3 RID: 7587
			private TaskAwaiter<Page> <>u__1;
		}

		// Token: 0x02000554 RID: 1364
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <btnBack_Clicked>d__16 : IAsyncStateMachine
		{
			// Token: 0x060032CF RID: 13007 RVA: 0x00233650 File Offset: 0x00231850
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				DTCWizardResultsPage dtcwizardResultsPage = this;
				try
				{
					TaskAwaiter<Page> taskAwaiter;
					if (num != 0)
					{
						App.OBDReader.DTCReadingQueueProgress -= dtcwizardResultsPage.OBDReader_DTCReadingQueueProgress;
						taskAwaiter = dtcwizardResultsPage.Navigation.PopAsync(true).GetAwaiter();
						if (!taskAwaiter.IsCompleted)
						{
							num2 = 0;
							TaskAwaiter<Page> taskAwaiter2 = taskAwaiter;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<Page>, DTCWizardResultsPage.<btnBack_Clicked>d__16>(ref taskAwaiter, ref this);
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

			// Token: 0x060032D0 RID: 13008 RVA: 0x00233720 File Offset: 0x00231920
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x04001DA4 RID: 7588
			public int <>1__state;

			// Token: 0x04001DA5 RID: 7589
			public AsyncVoidMethodBuilder <>t__builder;

			// Token: 0x04001DA6 RID: 7590
			public DTCWizardResultsPage <>4__this;

			// Token: 0x04001DA7 RID: 7591
			private TaskAwaiter<Page> <>u__1;
		}

		// Token: 0x02000555 RID: 1365
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <btnCancel_Clicked>d__15 : IAsyncStateMachine
		{
			// Token: 0x060032D1 RID: 13009 RVA: 0x00233730 File Offset: 0x00231930
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				DTCWizardResultsPage dtcwizardResultsPage = this;
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
						taskAwaiter3 = taskAwaiter2;
						taskAwaiter2 = default(TaskAwaiter<bool>);
						num2 = -1;
						goto IL_0142;
					case 2:
					{
						TaskAwaiter taskAwaiter5;
						taskAwaiter4 = taskAwaiter5;
						taskAwaiter5 = default(TaskAwaiter);
						num2 = -1;
						goto IL_01BB;
					}
					case 3:
						taskAwaiter3 = taskAwaiter2;
						taskAwaiter2 = default(TaskAwaiter<bool>);
						num2 = -1;
						goto IL_021C;
					default:
						taskAwaiter3 = dtcwizardResultsPage.DisplayAlert(Translate.GetString("ios_StopOperation"), "", Translate.GetString("ios_Yes"), Translate.GetString("ios_No")).GetAwaiter();
						if (!taskAwaiter3.IsCompleted)
						{
							num2 = 0;
							taskAwaiter2 = taskAwaiter3;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<bool>, DTCWizardResultsPage.<btnCancel_Clicked>d__15>(ref taskAwaiter3, ref this);
							return;
						}
						break;
					}
					if (!taskAwaiter3.GetResult())
					{
						goto IL_0230;
					}
					dtcwizardResultsPage.btnCancel.IsEnabled = false;
					App.OBDReader.DTCReadingQueueProgress -= dtcwizardResultsPage.OBDReader_DTCReadingQueueProgress;
					App.OBDReader.CurrentMode = OBDDataReader.OBDModes.Universal;
					App.OBDReader.ReplaceQueue(new List<OBDRequest>());
					dtcwizardResultsPage.FinishRead();
					taskAwaiter3 = App.OBDReader.CheckECUConnectionWhileRunning(false).GetAwaiter();
					if (!taskAwaiter3.IsCompleted)
					{
						num2 = 1;
						taskAwaiter2 = taskAwaiter3;
						this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<bool>, DTCWizardResultsPage.<btnCancel_Clicked>d__15>(ref taskAwaiter3, ref this);
						return;
					}
					IL_0142:
					if (taskAwaiter3.GetResult())
					{
						goto IL_0224;
					}
					dtcwizardResultsPage.activityFrame.Text = "ECU disconnected, trying to reconnect.";
					taskAwaiter4 = App.OBDReader.Stop("DTCWizardResult:UserCancelled").GetAwaiter();
					if (!taskAwaiter4.IsCompleted)
					{
						num2 = 2;
						TaskAwaiter taskAwaiter5 = taskAwaiter4;
						this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, DTCWizardResultsPage.<btnCancel_Clicked>d__15>(ref taskAwaiter4, ref this);
						return;
					}
					IL_01BB:
					taskAwaiter4.GetResult();
					taskAwaiter3 = App.OBDReader.ReinitializeConnectionToECU("DTCResults: btnCancel").GetAwaiter();
					if (!taskAwaiter3.IsCompleted)
					{
						num2 = 3;
						taskAwaiter2 = taskAwaiter3;
						this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<bool>, DTCWizardResultsPage.<btnCancel_Clicked>d__15>(ref taskAwaiter3, ref this);
						return;
					}
					IL_021C:
					taskAwaiter3.GetResult();
					IL_0224:
					dtcwizardResultsPage.activityFrame.IsVisible = false;
					IL_0230:;
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

			// Token: 0x060032D2 RID: 13010 RVA: 0x002339B8 File Offset: 0x00231BB8
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x04001DA8 RID: 7592
			public int <>1__state;

			// Token: 0x04001DA9 RID: 7593
			public AsyncVoidMethodBuilder <>t__builder;

			// Token: 0x04001DAA RID: 7594
			public DTCWizardResultsPage <>4__this;

			// Token: 0x04001DAB RID: 7595
			private TaskAwaiter<bool> <>u__1;

			// Token: 0x04001DAC RID: 7596
			private TaskAwaiter <>u__2;
		}

		// Token: 0x02000556 RID: 1366
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <btnClear_Clicked>d__18 : IAsyncStateMachine
		{
			// Token: 0x060032D3 RID: 13011 RVA: 0x002339C8 File Offset: 0x00231BC8
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				DTCWizardResultsPage dtcwizardResultsPage = this;
				try
				{
					TaskAwaiter taskAwaiter;
					TaskAwaiter<string> taskAwaiter3;
					switch (num)
					{
					case 0:
					{
						TaskAwaiter taskAwaiter2;
						taskAwaiter = taskAwaiter2;
						taskAwaiter2 = default(TaskAwaiter);
						num = (num2 = -1);
						break;
					}
					case 1:
					{
						TaskAwaiter taskAwaiter2;
						taskAwaiter = taskAwaiter2;
						taskAwaiter2 = default(TaskAwaiter);
						num = (num2 = -1);
						goto IL_010D;
					}
					case 2:
					{
						TaskAwaiter<string> taskAwaiter4;
						taskAwaiter3 = taskAwaiter4;
						taskAwaiter4 = default(TaskAwaiter<string>);
						num = (num2 = -1);
						goto IL_02AC;
					}
					case 3:
					{
						TaskAwaiter taskAwaiter2;
						taskAwaiter = taskAwaiter2;
						taskAwaiter2 = default(TaskAwaiter);
						num = (num2 = -1);
						goto IL_032C;
					}
					case 4:
					{
						TaskAwaiter taskAwaiter2;
						taskAwaiter = taskAwaiter2;
						taskAwaiter2 = default(TaskAwaiter);
						num = (num2 = -1);
						goto IL_0417;
					}
					default:
						if (dtcwizardResultsPage.lvDTC.ItemsSource == null)
						{
							taskAwaiter = dtcwizardResultsPage.GoToFullDTClear().GetAwaiter();
							if (!taskAwaiter.IsCompleted)
							{
								num = (num2 = 0);
								TaskAwaiter taskAwaiter2 = taskAwaiter;
								this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, DTCWizardResultsPage.<btnClear_Clicked>d__18>(ref taskAwaiter, ref this);
								return;
							}
						}
						else
						{
							List<DTCItemV2> list = (dtcwizardResultsPage.lvDTC.ItemsSource as IEnumerable<DTCItemV2>).ToList<DTCItemV2>();
							list.Reverse();
							if (list.Count == 0)
							{
								taskAwaiter = dtcwizardResultsPage.GoToFullDTClear().GetAwaiter();
								if (!taskAwaiter.IsCompleted)
								{
									num = (num2 = 1);
									TaskAwaiter taskAwaiter2 = taskAwaiter;
									this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, DTCWizardResultsPage.<btnClear_Clicked>d__18>(ref taskAwaiter, ref this);
									return;
								}
								goto IL_010D;
							}
							else
							{
								clearRequestsCollection = new List<OBDRequest[]>();
								List<DTCItemV2>.Enumerator enumerator = list.GetEnumerator();
								try
								{
									while (enumerator.MoveNext())
									{
										DTCItemV2 dtcitemV = enumerator.Current;
										OBDRequest[] requestForClear = dtcitemV.GetRequestForClear();
										bool flag = true;
										List<OBDRequest[]>.Enumerator enumerator2 = clearRequestsCollection.GetEnumerator();
										try
										{
											while (enumerator2.MoveNext())
											{
												OBDRequest[] array = enumerator2.Current;
												if (array.Length == requestForClear.Length)
												{
													bool flag2 = true;
													for (int i = 0; i < array.Length; i++)
													{
														if (!array[i].Equals(requestForClear[i]))
														{
															flag2 = false;
															break;
														}
													}
													if (flag2)
													{
														flag = false;
														break;
													}
												}
											}
										}
										finally
										{
											if (num < 0)
											{
												((IDisposable)enumerator2).Dispose();
											}
										}
										if (flag)
										{
											clearRequestsCollection.Add(requestForClear);
										}
									}
								}
								finally
								{
									if (num < 0)
									{
										((IDisposable)enumerator).Dispose();
									}
								}
								if (clearRequestsCollection.Count > 0)
								{
									action_clear_found = Translate.GetString("dtc_ClearFoundDTC");
									action_clear_full = Translate.GetString("dtc_ClearFullMode");
									string @string = Translate.GetString("dtc_ClearFoundsQuestion");
									cancel = Translate.GetString("btnCancel.Content");
									taskAwaiter3 = dtcwizardResultsPage.DisplayActionSheetCustom(@string, cancel, null, new string[] { action_clear_found, action_clear_full }).GetAwaiter();
									if (!taskAwaiter3.IsCompleted)
									{
										num = (num2 = 2);
										TaskAwaiter<string> taskAwaiter4 = taskAwaiter3;
										this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<string>, DTCWizardResultsPage.<btnClear_Clicked>d__18>(ref taskAwaiter3, ref this);
										return;
									}
									goto IL_02AC;
								}
								else
								{
									taskAwaiter = dtcwizardResultsPage.GoToFullDTClear().GetAwaiter();
									if (!taskAwaiter.IsCompleted)
									{
										num = (num2 = 4);
										TaskAwaiter taskAwaiter2 = taskAwaiter;
										this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, DTCWizardResultsPage.<btnClear_Clicked>d__18>(ref taskAwaiter, ref this);
										return;
									}
									goto IL_0417;
								}
							}
						}
						break;
					}
					taskAwaiter.GetResult();
					goto IL_0440;
					IL_010D:
					taskAwaiter.GetResult();
					goto IL_0425;
					IL_02AC:
					string result = taskAwaiter3.GetResult();
					if (result == cancel)
					{
						goto IL_0440;
					}
					if (result == action_clear_full)
					{
						taskAwaiter = dtcwizardResultsPage.GoToFullDTClear().GetAwaiter();
						if (!taskAwaiter.IsCompleted)
						{
							num = (num2 = 3);
							TaskAwaiter taskAwaiter2 = taskAwaiter;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, DTCWizardResultsPage.<btnClear_Clicked>d__18>(ref taskAwaiter, ref this);
							return;
						}
					}
					else
					{
						if (result == action_clear_found)
						{
							List<OBDRequest> list2 = new List<OBDRequest>();
							List<OBDRequest[]>.Enumerator enumerator2 = clearRequestsCollection.GetEnumerator();
							try
							{
								while (enumerator2.MoveNext())
								{
									OBDRequest[] array2 = enumerator2.Current;
									list2.AddRange(array2);
								}
							}
							finally
							{
								if (num < 0)
								{
									((IDisposable)enumerator2).Dispose();
								}
							}
							list2.AddRange(DTCWorker.GetRestoreConnectionCommands());
							dtcwizardResultsPage.Init(DTCWizardSetupPage.DTCMode.Clear, list2.ToArray());
							dtcwizardResultsPage.ContentPage_Appearing(dtcwizardResultsPage, EventArgs.Empty);
							goto IL_03AF;
						}
						goto IL_03AF;
					}
					IL_032C:
					taskAwaiter.GetResult();
					IL_03AF:
					action_clear_found = null;
					action_clear_full = null;
					cancel = null;
					goto IL_041E;
					IL_0417:
					taskAwaiter.GetResult();
					IL_041E:
					clearRequestsCollection = null;
					IL_0425:;
				}
				catch (Exception ex)
				{
					num2 = -2;
					this.<>t__builder.SetException(ex);
					return;
				}
				IL_0440:
				num2 = -2;
				this.<>t__builder.SetResult();
			}

			// Token: 0x060032D4 RID: 13012 RVA: 0x00233E8C File Offset: 0x0023208C
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x04001DAD RID: 7597
			public int <>1__state;

			// Token: 0x04001DAE RID: 7598
			public AsyncVoidMethodBuilder <>t__builder;

			// Token: 0x04001DAF RID: 7599
			public DTCWizardResultsPage <>4__this;

			// Token: 0x04001DB0 RID: 7600
			private TaskAwaiter <>u__1;

			// Token: 0x04001DB1 RID: 7601
			private List<OBDRequest[]> <clearRequestsCollection>5__2;

			// Token: 0x04001DB2 RID: 7602
			private string <action_clear_found>5__3;

			// Token: 0x04001DB3 RID: 7603
			private string <action_clear_full>5__4;

			// Token: 0x04001DB4 RID: 7604
			private string <cancel>5__5;

			// Token: 0x04001DB5 RID: 7605
			private TaskAwaiter<string> <>u__2;
		}

		// Token: 0x02000557 RID: 1367
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <btnExportReport_Clicked>d__22 : IAsyncStateMachine
		{
			// Token: 0x060032D5 RID: 13013 RVA: 0x00233E9C File Offset: 0x0023209C
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				DTCWizardResultsPage dtcwizardResultsPage = this;
				try
				{
					string text;
					if (num != 0)
					{
						dtcwizardResultsPage.btnExportReport.IsEnabled = false;
						text = ReportGenerator.CreateReport((IEnumerable<DTCItemV2>)dtcwizardResultsPage.lvDTC.ItemsSource);
					}
					try
					{
						TaskAwaiter taskAwaiter;
						if (num != 0)
						{
							taskAwaiter = Share.RequestAsync(text).GetAwaiter();
							if (!taskAwaiter.IsCompleted)
							{
								num2 = 0;
								TaskAwaiter taskAwaiter2 = taskAwaiter;
								this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, DTCWizardResultsPage.<btnExportReport_Clicked>d__22>(ref taskAwaiter, ref this);
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
					dtcwizardResultsPage.btnExportReport.IsEnabled = true;
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

			// Token: 0x060032D6 RID: 13014 RVA: 0x00233F94 File Offset: 0x00232194
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x04001DB6 RID: 7606
			public int <>1__state;

			// Token: 0x04001DB7 RID: 7607
			public AsyncVoidMethodBuilder <>t__builder;

			// Token: 0x04001DB8 RID: 7608
			public DTCWizardResultsPage <>4__this;

			// Token: 0x04001DB9 RID: 7609
			private TaskAwaiter <>u__1;
		}

		// Token: 0x02000558 RID: 1368
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <btnRead_Clicked>d__20 : IAsyncStateMachine
		{
			// Token: 0x060032D7 RID: 13015 RVA: 0x00233FA4 File Offset: 0x002321A4
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				DTCWizardResultsPage dtcwizardResultsPage = this;
				try
				{
					if (num != 0)
					{
						dtcwizardResultsPage.btnRead.IsEnabled = false;
					}
					try
					{
						TaskAwaiter<Page> taskAwaiter;
						if (num != 0)
						{
							Page page = dtcwizardResultsPage.Navigation.NavigationStack.FirstOrDefault((Page x) => x is DTCWizardSetupPage);
							dtcwizardResultsPage.Navigation.RemovePage(page);
							DTCWizardSetupPage dtcwizardSetupPage = new DTCWizardSetupPage(DTCWizardSetupPage.DTCMode.Read);
							dtcwizardResultsPage.Navigation.InsertPageBefore(dtcwizardSetupPage, dtcwizardResultsPage);
							taskAwaiter = dtcwizardResultsPage.Navigation.PopAsync().GetAwaiter();
							if (!taskAwaiter.IsCompleted)
							{
								num2 = 0;
								TaskAwaiter<Page> taskAwaiter2 = taskAwaiter;
								this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<Page>, DTCWizardResultsPage.<btnRead_Clicked>d__20>(ref taskAwaiter, ref this);
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
					}
					catch (Exception)
					{
					}
					dtcwizardResultsPage.btnRead.IsEnabled = true;
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

			// Token: 0x060032D8 RID: 13016 RVA: 0x002340E4 File Offset: 0x002322E4
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x04001DBA RID: 7610
			public int <>1__state;

			// Token: 0x04001DBB RID: 7611
			public AsyncVoidMethodBuilder <>t__builder;

			// Token: 0x04001DBC RID: 7612
			public DTCWizardResultsPage <>4__this;

			// Token: 0x04001DBD RID: 7613
			private TaskAwaiter<Page> <>u__1;
		}

		// Token: 0x02000559 RID: 1369
		[CompilerGenerated]
		private sealed class <InitializeComponent>_anonXamlCDataTemplate_32
		{
			// Token: 0x060032D9 RID: 13017 RVA: 0x002340F4 File Offset: 0x002322F4
			public <InitializeComponent>_anonXamlCDataTemplate_32()
			{
			}

			// Token: 0x060032DA RID: 13018 RVA: 0x00234108 File Offset: 0x00232308
			internal object LoadDataTemplate()
			{
				RowDefinition rowDefinition;
				VisualDiagnostics.RegisterSourceInfo(rowDefinition = new RowDefinition(), new Uri("DTC\\DTCWizardResultsPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 65, 34);
				RowDefinition rowDefinition2;
				VisualDiagnostics.RegisterSourceInfo(rowDefinition2 = new RowDefinition(), new Uri("DTC\\DTCWizardResultsPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 66, 34);
				RowDefinition rowDefinition3;
				VisualDiagnostics.RegisterSourceInfo(rowDefinition3 = new RowDefinition(), new Uri("DTC\\DTCWizardResultsPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 67, 34);
				ColumnDefinition columnDefinition;
				VisualDiagnostics.RegisterSourceInfo(columnDefinition = new ColumnDefinition(), new Uri("DTC\\DTCWizardResultsPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 72, 38);
				DynamicResourceExtension dynamicResourceExtension;
				VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension = new DynamicResourceExtension(), new Uri("DTC\\DTCWizardResultsPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 79, 37);
				StaticResourceExtension staticResourceExtension;
				VisualDiagnostics.RegisterSourceInfo(staticResourceExtension = new StaticResourceExtension(), new Uri("DTC\\DTCWizardResultsPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 80, 37);
				BindingExtension bindingExtension;
				VisualDiagnostics.RegisterSourceInfo(bindingExtension = new BindingExtension(), new Uri("DTC\\DTCWizardResultsPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 80, 37);
				Label label;
				VisualDiagnostics.RegisterSourceInfo(label = new Label(), new Uri("DTC\\DTCWizardResultsPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 75, 34);
				Grid grid;
				VisualDiagnostics.RegisterSourceInfo(grid = new Grid(), new Uri("DTC\\DTCWizardResultsPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 70, 30);
				ColumnDefinition columnDefinition2;
				VisualDiagnostics.RegisterSourceInfo(columnDefinition2 = new ColumnDefinition(), new Uri("DTC\\DTCWizardResultsPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 86, 38);
				ColumnDefinition columnDefinition3;
				VisualDiagnostics.RegisterSourceInfo(columnDefinition3 = new ColumnDefinition(), new Uri("DTC\\DTCWizardResultsPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 87, 38);
				DynamicResourceExtension dynamicResourceExtension2;
				VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension2 = new DynamicResourceExtension(), new Uri("DTC\\DTCWizardResultsPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 95, 41);
				BindingExtension bindingExtension2;
				VisualDiagnostics.RegisterSourceInfo(bindingExtension2 = new BindingExtension(), new Uri("DTC\\DTCWizardResultsPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 96, 41);
				Translate translate;
				VisualDiagnostics.RegisterSourceInfo(translate = new Translate(), new Uri("DTC\\DTCWizardResultsPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 97, 41);
				Label label2;
				VisualDiagnostics.RegisterSourceInfo(label2 = new Label(), new Uri("DTC\\DTCWizardResultsPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 93, 38);
				DynamicResourceExtension dynamicResourceExtension3;
				VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension3 = new DynamicResourceExtension(), new Uri("DTC\\DTCWizardResultsPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 105, 53);
				Translate translate2;
				VisualDiagnostics.RegisterSourceInfo(translate2 = new Translate(), new Uri("DTC\\DTCWizardResultsPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 106, 53);
				Span span;
				VisualDiagnostics.RegisterSourceInfo(span = new Span(), new Uri("DTC\\DTCWizardResultsPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 103, 50);
				DynamicResourceExtension dynamicResourceExtension4;
				VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension4 = new DynamicResourceExtension(), new Uri("DTC\\DTCWizardResultsPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 109, 53);
				BindingExtension bindingExtension3;
				VisualDiagnostics.RegisterSourceInfo(bindingExtension3 = new BindingExtension(), new Uri("DTC\\DTCWizardResultsPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 110, 53);
				Span span2;
				VisualDiagnostics.RegisterSourceInfo(span2 = new Span(), new Uri("DTC\\DTCWizardResultsPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 107, 50);
				FormattedString formattedString;
				VisualDiagnostics.RegisterSourceInfo(formattedString = new FormattedString(), new Uri("DTC\\DTCWizardResultsPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 102, 46);
				Label label3;
				VisualDiagnostics.RegisterSourceInfo(label3 = new Label(), new Uri("DTC\\DTCWizardResultsPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 100, 38);
				DynamicResourceExtension dynamicResourceExtension5;
				VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension5 = new DynamicResourceExtension(), new Uri("DTC\\DTCWizardResultsPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 117, 41);
				StaticResourceExtension staticResourceExtension2;
				VisualDiagnostics.RegisterSourceInfo(staticResourceExtension2 = new StaticResourceExtension(), new Uri("DTC\\DTCWizardResultsPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 118, 41);
				BindingExtension bindingExtension4;
				VisualDiagnostics.RegisterSourceInfo(bindingExtension4 = new BindingExtension(), new Uri("DTC\\DTCWizardResultsPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 118, 41);
				BindingExtension bindingExtension5;
				VisualDiagnostics.RegisterSourceInfo(bindingExtension5 = new BindingExtension(), new Uri("DTC\\DTCWizardResultsPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 119, 41);
				Label label4;
				VisualDiagnostics.RegisterSourceInfo(label4 = new Label(), new Uri("DTC\\DTCWizardResultsPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 116, 38);
				DynamicResourceExtension dynamicResourceExtension6;
				VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension6 = new DynamicResourceExtension(), new Uri("DTC\\DTCWizardResultsPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 127, 41);
				StaticResourceExtension staticResourceExtension3;
				VisualDiagnostics.RegisterSourceInfo(staticResourceExtension3 = new StaticResourceExtension(), new Uri("DTC\\DTCWizardResultsPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 128, 41);
				BindingExtension bindingExtension6;
				VisualDiagnostics.RegisterSourceInfo(bindingExtension6 = new BindingExtension(), new Uri("DTC\\DTCWizardResultsPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 128, 41);
				BindingExtension bindingExtension7;
				VisualDiagnostics.RegisterSourceInfo(bindingExtension7 = new BindingExtension(), new Uri("DTC\\DTCWizardResultsPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 129, 41);
				Label label5;
				VisualDiagnostics.RegisterSourceInfo(label5 = new Label(), new Uri("DTC\\DTCWizardResultsPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 126, 38);
				DynamicResourceExtension dynamicResourceExtension7;
				VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension7 = new DynamicResourceExtension(), new Uri("DTC\\DTCWizardResultsPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 133, 41);
				Translate translate3;
				VisualDiagnostics.RegisterSourceInfo(translate3 = new Translate(), new Uri("DTC\\DTCWizardResultsPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 135, 41);
				Label label6;
				VisualDiagnostics.RegisterSourceInfo(label6 = new Label(), new Uri("DTC\\DTCWizardResultsPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 131, 38);
				StackLayout stackLayout;
				VisualDiagnostics.RegisterSourceInfo(stackLayout = new StackLayout(), new Uri("DTC\\DTCWizardResultsPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 89, 34);
				DynamicResourceExtension dynamicResourceExtension8;
				VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension8 = new DynamicResourceExtension(), new Uri("DTC\\DTCWizardResultsPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 144, 37);
				LinkButton linkButton;
				VisualDiagnostics.RegisterSourceInfo(linkButton = new LinkButton(), new Uri("DTC\\DTCWizardResultsPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 137, 34);
				Grid grid2;
				VisualDiagnostics.RegisterSourceInfo(grid2 = new Grid(), new Uri("DTC\\DTCWizardResultsPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 84, 30);
				DynamicResourceExtension dynamicResourceExtension9;
				VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension9 = new DynamicResourceExtension(), new Uri("DTC\\DTCWizardResultsPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 152, 33);
				Frame frame;
				VisualDiagnostics.RegisterSourceInfo(frame = new Frame(), new Uri("DTC\\DTCWizardResultsPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 147, 30);
				Grid grid3;
				VisualDiagnostics.RegisterSourceInfo(grid3 = new Grid(), new Uri("DTC\\DTCWizardResultsPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 63, 26);
				NameScope nameScope = new NameScope();
				NameScope.SetNameScope(grid3, nameScope);
				nameScope.RegisterName("labelCode", label);
				if (label.StyleId == null)
				{
					label.StyleId = "labelCode";
				}
				grid3.SetValue(View.MarginProperty, new Thickness(0.0, 0.0, 0.0, 5.0));
				grid3.SetValue(Grid.RowSpacingProperty, 0.0);
				rowDefinition.SetValue(RowDefinition.HeightProperty, new GridLengthTypeConverter().ConvertFromInvariantString("auto"));
				grid3.GetValue(Grid.RowDefinitionsProperty).Add(rowDefinition);
				rowDefinition2.SetValue(RowDefinition.HeightProperty, new GridLengthTypeConverter().ConvertFromInvariantString("auto"));
				grid3.GetValue(Grid.RowDefinitionsProperty).Add(rowDefinition2);
				rowDefinition3.SetValue(RowDefinition.HeightProperty, new GridLengthTypeConverter().ConvertFromInvariantString("2"));
				grid3.GetValue(Grid.RowDefinitionsProperty).Add(rowDefinition3);
				grid.SetValue(Grid.RowProperty, 0);
				columnDefinition.SetValue(ColumnDefinition.WidthProperty, new GridLengthTypeConverter().ConvertFromInvariantString("*"));
				grid.GetValue(Grid.ColumnDefinitionsProperty).Add(columnDefinition);
				label.SetValue(Grid.ColumnProperty, 0);
				label.SetValue(Label.FontAttributesProperty, new FontAttributesConverter().ConvertFromInvariantString("Bold"));
				dynamicResourceExtension.Key = "BaseFontSize++";
				IMarkupExtension<DynamicResource> markupExtension = dynamicResourceExtension;
				XamlServiceProvider xamlServiceProvider = new XamlServiceProvider();
				Type typeFromHandle = typeof(IProvideValueTarget);
				int num;
				object[] array = new object[(num = this.parentValues.Length) + 3];
				Array.Copy(this.parentValues, 0, array, 3, num);
				object[] array2 = array;
				array2[0] = label;
				array2[1] = grid;
				array2[2] = grid3;
				object obj;
				xamlServiceProvider.Add(typeFromHandle, obj = new SimpleValueTargetProvider(array2, Label.FontSizeProperty, nameScope));
				xamlServiceProvider.Add(typeof(IReferenceProvider), obj);
				Type typeFromHandle2 = typeof(IXamlTypeResolver);
				XmlNamespaceResolver xmlNamespaceResolver = new XmlNamespaceResolver();
				xmlNamespaceResolver.Add("", "http://xamarin.com/schemas/2014/forms");
				xmlNamespaceResolver.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
				xmlNamespaceResolver.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
				xmlNamespaceResolver.Add("local", "clr-namespace:CarScannerXamarinForms");
				xmlNamespaceResolver.Add("syncfusion", "clr-namespace:Syncfusion.ListView.XForms;assembly=Syncfusion.SfListView.XForms");
				xamlServiceProvider.Add(typeFromHandle2, new XamlTypeResolver(xmlNamespaceResolver, typeof(DTCWizardResultsPage.<InitializeComponent>_anonXamlCDataTemplate_32).GetTypeInfo().Assembly));
				xamlServiceProvider.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(79, 37)));
				DynamicResource dynamicResource = markupExtension.ProvideValue(xamlServiceProvider);
				label.SetDynamicResource(Label.FontSizeProperty, dynamicResource.Key);
				staticResourceExtension.Key = "DTCCodeToStringConverter";
				IMarkupExtension markupExtension2 = staticResourceExtension;
				XamlServiceProvider xamlServiceProvider2 = new XamlServiceProvider();
				Type typeFromHandle3 = typeof(IProvideValueTarget);
				int num2;
				object[] array3 = new object[(num2 = this.parentValues.Length) + 4];
				Array.Copy(this.parentValues, 0, array3, 4, num2);
				object[] array4 = array3;
				array4[0] = bindingExtension;
				array4[1] = label;
				array4[2] = grid;
				array4[3] = grid3;
				object obj2;
				xamlServiceProvider2.Add(typeFromHandle3, obj2 = new SimpleValueTargetProvider(array4, typeof(BindingExtension).GetRuntimeProperty("Converter"), nameScope));
				xamlServiceProvider2.Add(typeof(IReferenceProvider), obj2);
				Type typeFromHandle4 = typeof(IXamlTypeResolver);
				XmlNamespaceResolver xmlNamespaceResolver2 = new XmlNamespaceResolver();
				xmlNamespaceResolver2.Add("", "http://xamarin.com/schemas/2014/forms");
				xmlNamespaceResolver2.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
				xmlNamespaceResolver2.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
				xmlNamespaceResolver2.Add("local", "clr-namespace:CarScannerXamarinForms");
				xmlNamespaceResolver2.Add("syncfusion", "clr-namespace:Syncfusion.ListView.XForms;assembly=Syncfusion.SfListView.XForms");
				xamlServiceProvider2.Add(typeFromHandle4, new XamlTypeResolver(xmlNamespaceResolver2, typeof(DTCWizardResultsPage.<InitializeComponent>_anonXamlCDataTemplate_32).GetTypeInfo().Assembly));
				xamlServiceProvider2.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(80, 37)));
				object obj3 = markupExtension2.ProvideValue(xamlServiceProvider2);
				bindingExtension.Converter = obj3;
				bindingExtension.Path = "Code";
				BindingBase bindingBase = bindingExtension.ProvideValue(null);
				label.SetBinding(Label.TextProperty, bindingBase);
				grid.Children.Add(label);
				grid3.Children.Add(grid);
				grid2.SetValue(Grid.RowProperty, 1);
				grid2.SetValue(Grid.RowSpacingProperty, 0.0);
				columnDefinition2.SetValue(ColumnDefinition.WidthProperty, new GridLengthTypeConverter().ConvertFromInvariantString("*"));
				grid2.GetValue(Grid.ColumnDefinitionsProperty).Add(columnDefinition2);
				columnDefinition3.SetValue(ColumnDefinition.WidthProperty, new GridLengthTypeConverter().ConvertFromInvariantString("auto"));
				grid2.GetValue(Grid.ColumnDefinitionsProperty).Add(columnDefinition3);
				stackLayout.SetValue(Grid.ColumnProperty, 0);
				stackLayout.SetValue(StackLayout.OrientationProperty, 0);
				stackLayout.SetValue(StackLayout.SpacingProperty, 0.0);
				label2.SetValue(Label.FontAttributesProperty, new FontAttributesConverter().ConvertFromInvariantString("Bold"));
				dynamicResourceExtension2.Key = "BaseFontSize";
				IMarkupExtension<DynamicResource> markupExtension3 = dynamicResourceExtension2;
				XamlServiceProvider xamlServiceProvider3 = new XamlServiceProvider();
				Type typeFromHandle5 = typeof(IProvideValueTarget);
				int num3;
				object[] array5 = new object[(num3 = this.parentValues.Length) + 4];
				Array.Copy(this.parentValues, 0, array5, 4, num3);
				object[] array6 = array5;
				array6[0] = label2;
				array6[1] = stackLayout;
				array6[2] = grid2;
				array6[3] = grid3;
				object obj4;
				xamlServiceProvider3.Add(typeFromHandle5, obj4 = new SimpleValueTargetProvider(array6, Label.FontSizeProperty, nameScope));
				xamlServiceProvider3.Add(typeof(IReferenceProvider), obj4);
				Type typeFromHandle6 = typeof(IXamlTypeResolver);
				XmlNamespaceResolver xmlNamespaceResolver3 = new XmlNamespaceResolver();
				xmlNamespaceResolver3.Add("", "http://xamarin.com/schemas/2014/forms");
				xmlNamespaceResolver3.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
				xmlNamespaceResolver3.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
				xmlNamespaceResolver3.Add("local", "clr-namespace:CarScannerXamarinForms");
				xmlNamespaceResolver3.Add("syncfusion", "clr-namespace:Syncfusion.ListView.XForms;assembly=Syncfusion.SfListView.XForms");
				xamlServiceProvider3.Add(typeFromHandle6, new XamlTypeResolver(xmlNamespaceResolver3, typeof(DTCWizardResultsPage.<InitializeComponent>_anonXamlCDataTemplate_32).GetTypeInfo().Assembly));
				xamlServiceProvider3.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(95, 41)));
				DynamicResource dynamicResource2 = markupExtension3.ProvideValue(xamlServiceProvider3);
				label2.SetDynamicResource(Label.FontSizeProperty, dynamicResource2.Key);
				bindingExtension2.Path = "IsArchive";
				BindingBase bindingBase2 = bindingExtension2.ProvideValue(null);
				label2.SetBinding(VisualElement.IsVisibleProperty, bindingBase2);
				translate.Text = "ios_Archive";
				IMarkupExtension markupExtension4 = translate;
				XamlServiceProvider xamlServiceProvider4 = new XamlServiceProvider();
				Type typeFromHandle7 = typeof(IProvideValueTarget);
				int num4;
				object[] array7 = new object[(num4 = this.parentValues.Length) + 4];
				Array.Copy(this.parentValues, 0, array7, 4, num4);
				object[] array8 = array7;
				array8[0] = label2;
				array8[1] = stackLayout;
				array8[2] = grid2;
				array8[3] = grid3;
				object obj5;
				xamlServiceProvider4.Add(typeFromHandle7, obj5 = new SimpleValueTargetProvider(array8, Label.TextProperty, nameScope));
				xamlServiceProvider4.Add(typeof(IReferenceProvider), obj5);
				Type typeFromHandle8 = typeof(IXamlTypeResolver);
				XmlNamespaceResolver xmlNamespaceResolver4 = new XmlNamespaceResolver();
				xmlNamespaceResolver4.Add("", "http://xamarin.com/schemas/2014/forms");
				xmlNamespaceResolver4.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
				xmlNamespaceResolver4.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
				xmlNamespaceResolver4.Add("local", "clr-namespace:CarScannerXamarinForms");
				xmlNamespaceResolver4.Add("syncfusion", "clr-namespace:Syncfusion.ListView.XForms;assembly=Syncfusion.SfListView.XForms");
				xamlServiceProvider4.Add(typeFromHandle8, new XamlTypeResolver(xmlNamespaceResolver4, typeof(DTCWizardResultsPage.<InitializeComponent>_anonXamlCDataTemplate_32).GetTypeInfo().Assembly));
				xamlServiceProvider4.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(97, 41)));
				object obj6 = markupExtension4.ProvideValue(xamlServiceProvider4);
				label2.Text = obj6;
				stackLayout.Children.Add(label2);
				span.SetValue(Span.FontAttributesProperty, new FontAttributesConverter().ConvertFromInvariantString("Bold"));
				dynamicResourceExtension3.Key = "BaseFontSize";
				IMarkupExtension<DynamicResource> markupExtension5 = dynamicResourceExtension3;
				XamlServiceProvider xamlServiceProvider5 = new XamlServiceProvider();
				Type typeFromHandle9 = typeof(IProvideValueTarget);
				int num5;
				object[] array9 = new object[(num5 = this.parentValues.Length) + 6];
				Array.Copy(this.parentValues, 0, array9, 6, num5);
				object[] array10 = array9;
				array10[0] = span;
				array10[1] = formattedString;
				array10[2] = label3;
				array10[3] = stackLayout;
				array10[4] = grid2;
				array10[5] = grid3;
				object obj7;
				xamlServiceProvider5.Add(typeFromHandle9, obj7 = new SimpleValueTargetProvider(array10, Span.FontSizeProperty, nameScope));
				xamlServiceProvider5.Add(typeof(IReferenceProvider), obj7);
				Type typeFromHandle10 = typeof(IXamlTypeResolver);
				XmlNamespaceResolver xmlNamespaceResolver5 = new XmlNamespaceResolver();
				xmlNamespaceResolver5.Add("", "http://xamarin.com/schemas/2014/forms");
				xmlNamespaceResolver5.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
				xmlNamespaceResolver5.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
				xmlNamespaceResolver5.Add("local", "clr-namespace:CarScannerXamarinForms");
				xmlNamespaceResolver5.Add("syncfusion", "clr-namespace:Syncfusion.ListView.XForms;assembly=Syncfusion.SfListView.XForms");
				xamlServiceProvider5.Add(typeFromHandle10, new XamlTypeResolver(xmlNamespaceResolver5, typeof(DTCWizardResultsPage.<InitializeComponent>_anonXamlCDataTemplate_32).GetTypeInfo().Assembly));
				xamlServiceProvider5.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(105, 53)));
				DynamicResource dynamicResource3 = markupExtension5.ProvideValue(xamlServiceProvider5);
				span.SetDynamicResource(Span.FontSizeProperty, dynamicResource3.Key);
				translate2.Text = "dtc_ECU";
				IMarkupExtension markupExtension6 = translate2;
				XamlServiceProvider xamlServiceProvider6 = new XamlServiceProvider();
				Type typeFromHandle11 = typeof(IProvideValueTarget);
				int num6;
				object[] array11 = new object[(num6 = this.parentValues.Length) + 6];
				Array.Copy(this.parentValues, 0, array11, 6, num6);
				object[] array12 = array11;
				array12[0] = span;
				array12[1] = formattedString;
				array12[2] = label3;
				array12[3] = stackLayout;
				array12[4] = grid2;
				array12[5] = grid3;
				object obj8;
				xamlServiceProvider6.Add(typeFromHandle11, obj8 = new SimpleValueTargetProvider(array12, Span.TextProperty, nameScope));
				xamlServiceProvider6.Add(typeof(IReferenceProvider), obj8);
				Type typeFromHandle12 = typeof(IXamlTypeResolver);
				XmlNamespaceResolver xmlNamespaceResolver6 = new XmlNamespaceResolver();
				xmlNamespaceResolver6.Add("", "http://xamarin.com/schemas/2014/forms");
				xmlNamespaceResolver6.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
				xmlNamespaceResolver6.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
				xmlNamespaceResolver6.Add("local", "clr-namespace:CarScannerXamarinForms");
				xmlNamespaceResolver6.Add("syncfusion", "clr-namespace:Syncfusion.ListView.XForms;assembly=Syncfusion.SfListView.XForms");
				xamlServiceProvider6.Add(typeFromHandle12, new XamlTypeResolver(xmlNamespaceResolver6, typeof(DTCWizardResultsPage.<InitializeComponent>_anonXamlCDataTemplate_32).GetTypeInfo().Assembly));
				xamlServiceProvider6.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(106, 53)));
				object obj9 = markupExtension6.ProvideValue(xamlServiceProvider6);
				span.Text = obj9;
				formattedString.Spans.Add(span);
				span2.SetValue(Span.FontAttributesProperty, new FontAttributesConverter().ConvertFromInvariantString("Bold"));
				dynamicResourceExtension4.Key = "BaseFontSize";
				IMarkupExtension<DynamicResource> markupExtension7 = dynamicResourceExtension4;
				XamlServiceProvider xamlServiceProvider7 = new XamlServiceProvider();
				Type typeFromHandle13 = typeof(IProvideValueTarget);
				int num7;
				object[] array13 = new object[(num7 = this.parentValues.Length) + 6];
				Array.Copy(this.parentValues, 0, array13, 6, num7);
				object[] array14 = array13;
				array14[0] = span2;
				array14[1] = formattedString;
				array14[2] = label3;
				array14[3] = stackLayout;
				array14[4] = grid2;
				array14[5] = grid3;
				object obj10;
				xamlServiceProvider7.Add(typeFromHandle13, obj10 = new SimpleValueTargetProvider(array14, Span.FontSizeProperty, nameScope));
				xamlServiceProvider7.Add(typeof(IReferenceProvider), obj10);
				Type typeFromHandle14 = typeof(IXamlTypeResolver);
				XmlNamespaceResolver xmlNamespaceResolver7 = new XmlNamespaceResolver();
				xmlNamespaceResolver7.Add("", "http://xamarin.com/schemas/2014/forms");
				xmlNamespaceResolver7.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
				xmlNamespaceResolver7.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
				xmlNamespaceResolver7.Add("local", "clr-namespace:CarScannerXamarinForms");
				xmlNamespaceResolver7.Add("syncfusion", "clr-namespace:Syncfusion.ListView.XForms;assembly=Syncfusion.SfListView.XForms");
				xamlServiceProvider7.Add(typeFromHandle14, new XamlTypeResolver(xmlNamespaceResolver7, typeof(DTCWizardResultsPage.<InitializeComponent>_anonXamlCDataTemplate_32).GetTypeInfo().Assembly));
				xamlServiceProvider7.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(109, 53)));
				DynamicResource dynamicResource4 = markupExtension7.ProvideValue(xamlServiceProvider7);
				span2.SetDynamicResource(Span.FontSizeProperty, dynamicResource4.Key);
				bindingExtension3.Path = "ECU";
				BindingBase bindingBase3 = bindingExtension3.ProvideValue(null);
				span2.SetBinding(Span.TextProperty, bindingBase3);
				formattedString.Spans.Add(span2);
				label3.SetValue(Label.FormattedTextProperty, formattedString);
				stackLayout.Children.Add(label3);
				dynamicResourceExtension5.Key = "BaseFontSize";
				IMarkupExtension<DynamicResource> markupExtension8 = dynamicResourceExtension5;
				XamlServiceProvider xamlServiceProvider8 = new XamlServiceProvider();
				Type typeFromHandle15 = typeof(IProvideValueTarget);
				int num8;
				object[] array15 = new object[(num8 = this.parentValues.Length) + 4];
				Array.Copy(this.parentValues, 0, array15, 4, num8);
				object[] array16 = array15;
				array16[0] = label4;
				array16[1] = stackLayout;
				array16[2] = grid2;
				array16[3] = grid3;
				object obj11;
				xamlServiceProvider8.Add(typeFromHandle15, obj11 = new SimpleValueTargetProvider(array16, Label.FontSizeProperty, nameScope));
				xamlServiceProvider8.Add(typeof(IReferenceProvider), obj11);
				Type typeFromHandle16 = typeof(IXamlTypeResolver);
				XmlNamespaceResolver xmlNamespaceResolver8 = new XmlNamespaceResolver();
				xmlNamespaceResolver8.Add("", "http://xamarin.com/schemas/2014/forms");
				xmlNamespaceResolver8.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
				xmlNamespaceResolver8.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
				xmlNamespaceResolver8.Add("local", "clr-namespace:CarScannerXamarinForms");
				xmlNamespaceResolver8.Add("syncfusion", "clr-namespace:Syncfusion.ListView.XForms;assembly=Syncfusion.SfListView.XForms");
				xamlServiceProvider8.Add(typeFromHandle16, new XamlTypeResolver(xmlNamespaceResolver8, typeof(DTCWizardResultsPage.<InitializeComponent>_anonXamlCDataTemplate_32).GetTypeInfo().Assembly));
				xamlServiceProvider8.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(117, 41)));
				DynamicResource dynamicResource5 = markupExtension8.ProvideValue(xamlServiceProvider8);
				label4.SetDynamicResource(Label.FontSizeProperty, dynamicResource5.Key);
				staticResourceExtension2.Key = "DTCStatusCollectionToStringConverter";
				IMarkupExtension markupExtension9 = staticResourceExtension2;
				XamlServiceProvider xamlServiceProvider9 = new XamlServiceProvider();
				Type typeFromHandle17 = typeof(IProvideValueTarget);
				int num9;
				object[] array17 = new object[(num9 = this.parentValues.Length) + 5];
				Array.Copy(this.parentValues, 0, array17, 5, num9);
				object[] array18 = array17;
				array18[0] = bindingExtension4;
				array18[1] = label4;
				array18[2] = stackLayout;
				array18[3] = grid2;
				array18[4] = grid3;
				object obj12;
				xamlServiceProvider9.Add(typeFromHandle17, obj12 = new SimpleValueTargetProvider(array18, typeof(BindingExtension).GetRuntimeProperty("Converter"), nameScope));
				xamlServiceProvider9.Add(typeof(IReferenceProvider), obj12);
				Type typeFromHandle18 = typeof(IXamlTypeResolver);
				XmlNamespaceResolver xmlNamespaceResolver9 = new XmlNamespaceResolver();
				xmlNamespaceResolver9.Add("", "http://xamarin.com/schemas/2014/forms");
				xmlNamespaceResolver9.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
				xmlNamespaceResolver9.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
				xmlNamespaceResolver9.Add("local", "clr-namespace:CarScannerXamarinForms");
				xmlNamespaceResolver9.Add("syncfusion", "clr-namespace:Syncfusion.ListView.XForms;assembly=Syncfusion.SfListView.XForms");
				xamlServiceProvider9.Add(typeFromHandle18, new XamlTypeResolver(xmlNamespaceResolver9, typeof(DTCWizardResultsPage.<InitializeComponent>_anonXamlCDataTemplate_32).GetTypeInfo().Assembly));
				xamlServiceProvider9.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(118, 41)));
				object obj13 = markupExtension9.ProvideValue(xamlServiceProvider9);
				bindingExtension4.Converter = obj13;
				bindingExtension4.Path = "Statuses";
				BindingBase bindingBase4 = bindingExtension4.ProvideValue(null);
				label4.SetBinding(Label.FormattedTextProperty, bindingBase4);
				bindingExtension5.Path = "StatusVisible";
				BindingBase bindingBase5 = bindingExtension5.ProvideValue(null);
				label4.SetBinding(VisualElement.IsVisibleProperty, bindingBase5);
				label4.SetValue(Label.LineBreakModeProperty, 1);
				stackLayout.Children.Add(label4);
				dynamicResourceExtension6.Key = "BaseFontSize-";
				IMarkupExtension<DynamicResource> markupExtension10 = dynamicResourceExtension6;
				XamlServiceProvider xamlServiceProvider10 = new XamlServiceProvider();
				Type typeFromHandle19 = typeof(IProvideValueTarget);
				int num10;
				object[] array19 = new object[(num10 = this.parentValues.Length) + 4];
				Array.Copy(this.parentValues, 0, array19, 4, num10);
				object[] array20 = array19;
				array20[0] = label5;
				array20[1] = stackLayout;
				array20[2] = grid2;
				array20[3] = grid3;
				object obj14;
				xamlServiceProvider10.Add(typeFromHandle19, obj14 = new SimpleValueTargetProvider(array20, Label.FontSizeProperty, nameScope));
				xamlServiceProvider10.Add(typeof(IReferenceProvider), obj14);
				Type typeFromHandle20 = typeof(IXamlTypeResolver);
				XmlNamespaceResolver xmlNamespaceResolver10 = new XmlNamespaceResolver();
				xmlNamespaceResolver10.Add("", "http://xamarin.com/schemas/2014/forms");
				xmlNamespaceResolver10.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
				xmlNamespaceResolver10.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
				xmlNamespaceResolver10.Add("local", "clr-namespace:CarScannerXamarinForms");
				xmlNamespaceResolver10.Add("syncfusion", "clr-namespace:Syncfusion.ListView.XForms;assembly=Syncfusion.SfListView.XForms");
				xamlServiceProvider10.Add(typeFromHandle20, new XamlTypeResolver(xmlNamespaceResolver10, typeof(DTCWizardResultsPage.<InitializeComponent>_anonXamlCDataTemplate_32).GetTypeInfo().Assembly));
				xamlServiceProvider10.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(127, 41)));
				DynamicResource dynamicResource6 = markupExtension10.ProvideValue(xamlServiceProvider10);
				label5.SetDynamicResource(Label.FontSizeProperty, dynamicResource6.Key);
				staticResourceExtension3.Key = "DTCDescriptionCollectionToStringConverter";
				IMarkupExtension markupExtension11 = staticResourceExtension3;
				XamlServiceProvider xamlServiceProvider11 = new XamlServiceProvider();
				Type typeFromHandle21 = typeof(IProvideValueTarget);
				int num11;
				object[] array21 = new object[(num11 = this.parentValues.Length) + 5];
				Array.Copy(this.parentValues, 0, array21, 5, num11);
				object[] array22 = array21;
				array22[0] = bindingExtension6;
				array22[1] = label5;
				array22[2] = stackLayout;
				array22[3] = grid2;
				array22[4] = grid3;
				object obj15;
				xamlServiceProvider11.Add(typeFromHandle21, obj15 = new SimpleValueTargetProvider(array22, typeof(BindingExtension).GetRuntimeProperty("Converter"), nameScope));
				xamlServiceProvider11.Add(typeof(IReferenceProvider), obj15);
				Type typeFromHandle22 = typeof(IXamlTypeResolver);
				XmlNamespaceResolver xmlNamespaceResolver11 = new XmlNamespaceResolver();
				xmlNamespaceResolver11.Add("", "http://xamarin.com/schemas/2014/forms");
				xmlNamespaceResolver11.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
				xmlNamespaceResolver11.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
				xmlNamespaceResolver11.Add("local", "clr-namespace:CarScannerXamarinForms");
				xmlNamespaceResolver11.Add("syncfusion", "clr-namespace:Syncfusion.ListView.XForms;assembly=Syncfusion.SfListView.XForms");
				xamlServiceProvider11.Add(typeFromHandle22, new XamlTypeResolver(xmlNamespaceResolver11, typeof(DTCWizardResultsPage.<InitializeComponent>_anonXamlCDataTemplate_32).GetTypeInfo().Assembly));
				xamlServiceProvider11.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(128, 41)));
				object obj16 = markupExtension11.ProvideValue(xamlServiceProvider11);
				bindingExtension6.Converter = obj16;
				bindingExtension6.Path = "Descriptions";
				BindingBase bindingBase6 = bindingExtension6.ProvideValue(null);
				label5.SetBinding(Label.FormattedTextProperty, bindingBase6);
				bindingExtension7.Path = "DescriptionsVisible";
				BindingBase bindingBase7 = bindingExtension7.ProvideValue(null);
				label5.SetBinding(VisualElement.IsVisibleProperty, bindingBase7);
				label5.SetValue(Label.LineBreakModeProperty, 1);
				stackLayout.Children.Add(label5);
				label6.SetValue(Label.FontAttributesProperty, new FontAttributesConverter().ConvertFromInvariantString("Italic"));
				dynamicResourceExtension7.Key = "BaseFontSize--";
				IMarkupExtension<DynamicResource> markupExtension12 = dynamicResourceExtension7;
				XamlServiceProvider xamlServiceProvider12 = new XamlServiceProvider();
				Type typeFromHandle23 = typeof(IProvideValueTarget);
				int num12;
				object[] array23 = new object[(num12 = this.parentValues.Length) + 4];
				Array.Copy(this.parentValues, 0, array23, 4, num12);
				object[] array24 = array23;
				array24[0] = label6;
				array24[1] = stackLayout;
				array24[2] = grid2;
				array24[3] = grid3;
				object obj17;
				xamlServiceProvider12.Add(typeFromHandle23, obj17 = new SimpleValueTargetProvider(array24, Label.FontSizeProperty, nameScope));
				xamlServiceProvider12.Add(typeof(IReferenceProvider), obj17);
				Type typeFromHandle24 = typeof(IXamlTypeResolver);
				XmlNamespaceResolver xmlNamespaceResolver12 = new XmlNamespaceResolver();
				xmlNamespaceResolver12.Add("", "http://xamarin.com/schemas/2014/forms");
				xmlNamespaceResolver12.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
				xmlNamespaceResolver12.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
				xmlNamespaceResolver12.Add("local", "clr-namespace:CarScannerXamarinForms");
				xmlNamespaceResolver12.Add("syncfusion", "clr-namespace:Syncfusion.ListView.XForms;assembly=Syncfusion.SfListView.XForms");
				xamlServiceProvider12.Add(typeFromHandle24, new XamlTypeResolver(xmlNamespaceResolver12, typeof(DTCWizardResultsPage.<InitializeComponent>_anonXamlCDataTemplate_32).GetTypeInfo().Assembly));
				xamlServiceProvider12.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(133, 41)));
				DynamicResource dynamicResource7 = markupExtension12.ProvideValue(xamlServiceProvider12);
				label6.SetDynamicResource(Label.FontSizeProperty, dynamicResource7.Key);
				label6.SetValue(Label.LineBreakModeProperty, 1);
				translate3.Text = "ios_TapToGetDescription";
				IMarkupExtension markupExtension13 = translate3;
				XamlServiceProvider xamlServiceProvider13 = new XamlServiceProvider();
				Type typeFromHandle25 = typeof(IProvideValueTarget);
				int num13;
				object[] array25 = new object[(num13 = this.parentValues.Length) + 4];
				Array.Copy(this.parentValues, 0, array25, 4, num13);
				object[] array26 = array25;
				array26[0] = label6;
				array26[1] = stackLayout;
				array26[2] = grid2;
				array26[3] = grid3;
				object obj18;
				xamlServiceProvider13.Add(typeFromHandle25, obj18 = new SimpleValueTargetProvider(array26, Label.TextProperty, nameScope));
				xamlServiceProvider13.Add(typeof(IReferenceProvider), obj18);
				Type typeFromHandle26 = typeof(IXamlTypeResolver);
				XmlNamespaceResolver xmlNamespaceResolver13 = new XmlNamespaceResolver();
				xmlNamespaceResolver13.Add("", "http://xamarin.com/schemas/2014/forms");
				xmlNamespaceResolver13.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
				xmlNamespaceResolver13.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
				xmlNamespaceResolver13.Add("local", "clr-namespace:CarScannerXamarinForms");
				xmlNamespaceResolver13.Add("syncfusion", "clr-namespace:Syncfusion.ListView.XForms;assembly=Syncfusion.SfListView.XForms");
				xamlServiceProvider13.Add(typeFromHandle26, new XamlTypeResolver(xmlNamespaceResolver13, typeof(DTCWizardResultsPage.<InitializeComponent>_anonXamlCDataTemplate_32).GetTypeInfo().Assembly));
				xamlServiceProvider13.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(135, 41)));
				object obj19 = markupExtension13.ProvideValue(xamlServiceProvider13);
				label6.Text = obj19;
				stackLayout.Children.Add(label6);
				grid2.Children.Add(stackLayout);
				linkButton.SetValue(Grid.ColumnProperty, 1);
				linkButton.SetValue(View.MarginProperty, new Thickness(0.0));
				linkButton.SetValue(VisualElement.BackgroundColorProperty, Color.Transparent);
				linkButton.SetValue(Button.BorderColorProperty, Color.Transparent);
				linkButton.Clicked += this.root.ClearOneLinkButton_Clicked;
				linkButton.SetValue(View.HorizontalOptionsProperty, LayoutOptions.End);
				dynamicResourceExtension8.Key = "TC_del";
				IMarkupExtension<DynamicResource> markupExtension14 = dynamicResourceExtension8;
				XamlServiceProvider xamlServiceProvider14 = new XamlServiceProvider();
				Type typeFromHandle27 = typeof(IProvideValueTarget);
				int num14;
				object[] array27 = new object[(num14 = this.parentValues.Length) + 3];
				Array.Copy(this.parentValues, 0, array27, 3, num14);
				object[] array28 = array27;
				array28[0] = linkButton;
				array28[1] = grid2;
				array28[2] = grid3;
				object obj20;
				xamlServiceProvider14.Add(typeFromHandle27, obj20 = new SimpleValueTargetProvider(array28, Button.ImageProperty, nameScope));
				xamlServiceProvider14.Add(typeof(IReferenceProvider), obj20);
				Type typeFromHandle28 = typeof(IXamlTypeResolver);
				XmlNamespaceResolver xmlNamespaceResolver14 = new XmlNamespaceResolver();
				xmlNamespaceResolver14.Add("", "http://xamarin.com/schemas/2014/forms");
				xmlNamespaceResolver14.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
				xmlNamespaceResolver14.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
				xmlNamespaceResolver14.Add("local", "clr-namespace:CarScannerXamarinForms");
				xmlNamespaceResolver14.Add("syncfusion", "clr-namespace:Syncfusion.ListView.XForms;assembly=Syncfusion.SfListView.XForms");
				xamlServiceProvider14.Add(typeFromHandle28, new XamlTypeResolver(xmlNamespaceResolver14, typeof(DTCWizardResultsPage.<InitializeComponent>_anonXamlCDataTemplate_32).GetTypeInfo().Assembly));
				xamlServiceProvider14.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(144, 37)));
				DynamicResource dynamicResource8 = markupExtension14.ProvideValue(xamlServiceProvider14);
				linkButton.SetDynamicResource(Button.ImageProperty, dynamicResource8.Key);
				linkButton.SetValue(View.VerticalOptionsProperty, LayoutOptions.Start);
				grid2.Children.Add(linkButton);
				grid3.Children.Add(grid2);
				frame.SetValue(Grid.RowProperty, 2);
				frame.SetValue(Frame.HasShadowProperty, true);
				frame.SetValue(VisualElement.HeightRequestProperty, 2.0);
				frame.SetValue(View.HorizontalOptionsProperty, LayoutOptions.FillAndExpand);
				dynamicResourceExtension9.Key = "ListViewSeparatorColor";
				IMarkupExtension<DynamicResource> markupExtension15 = dynamicResourceExtension9;
				XamlServiceProvider xamlServiceProvider15 = new XamlServiceProvider();
				Type typeFromHandle29 = typeof(IProvideValueTarget);
				int num15;
				object[] array29 = new object[(num15 = this.parentValues.Length) + 2];
				Array.Copy(this.parentValues, 0, array29, 2, num15);
				object[] array30 = array29;
				array30[0] = frame;
				array30[1] = grid3;
				object obj21;
				xamlServiceProvider15.Add(typeFromHandle29, obj21 = new SimpleValueTargetProvider(array30, Frame.OutlineColorProperty, nameScope));
				xamlServiceProvider15.Add(typeof(IReferenceProvider), obj21);
				Type typeFromHandle30 = typeof(IXamlTypeResolver);
				XmlNamespaceResolver xmlNamespaceResolver15 = new XmlNamespaceResolver();
				xmlNamespaceResolver15.Add("", "http://xamarin.com/schemas/2014/forms");
				xmlNamespaceResolver15.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
				xmlNamespaceResolver15.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
				xmlNamespaceResolver15.Add("local", "clr-namespace:CarScannerXamarinForms");
				xmlNamespaceResolver15.Add("syncfusion", "clr-namespace:Syncfusion.ListView.XForms;assembly=Syncfusion.SfListView.XForms");
				xamlServiceProvider15.Add(typeFromHandle30, new XamlTypeResolver(xmlNamespaceResolver15, typeof(DTCWizardResultsPage.<InitializeComponent>_anonXamlCDataTemplate_32).GetTypeInfo().Assembly));
				xamlServiceProvider15.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(152, 33)));
				DynamicResource dynamicResource9 = markupExtension15.ProvideValue(xamlServiceProvider15);
				frame.SetDynamicResource(Frame.OutlineColorProperty, dynamicResource9.Key);
				grid3.Children.Add(frame);
				return grid3;
			}

			// Token: 0x04001DBE RID: 7614
			internal object[] parentValues;

			// Token: 0x04001DBF RID: 7615
			internal DTCWizardResultsPage root;
		}
	}
}
