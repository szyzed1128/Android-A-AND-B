using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using AiForms.Renderers;
using CarScannerXamarinForms.Common;
using CarScannerXamarinForms.Common.XAMLConverters;
using CarScannerXamarinForms.DataRecorder;
using CarScannerXamarinForms.OBD2;
using CarScannerXamarinForms.Pages;
using CarScannerXamarinForms.PlatformAdapters;
using CarScannerXamarinForms.UserControls;
using CarScannerXamarinForms.ViewModels;
using ICSharpCode.SharpZipLib.Zip;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Internals;
using Xamarin.Forms.PlatformConfiguration.iOSSpecific;
using Xamarin.Forms.Xaml;
using Xamarin.Forms.Xaml.Diagnostics;
using Xamarin.Forms.Xaml.Internals;

namespace CarScannerXamarinForms.Settings
{
	// Token: 0x02000250 RID: 592
	[XamlFilePath("Settings\\SettingsRecording.xaml")]
	public class SettingsRecording : ContentPage
	{
		// Token: 0x06001BBD RID: 7101 RVA: 0x0013BC00 File Offset: 0x00139E00
		public SettingsRecording()
		{
			this.tracksModel = new RecorderedTracksViewModel();
			try
			{
				this.InitializeComponent();
			}
			catch (Exception)
			{
			}
			base.Appearing += this.SettingsRecording_Appearing;
			base.BindingContext = SharedSettings.Current;
			this.lvRecords.BindingContext = this.Model;
			this.tbiSelectionModeChange = new ToolbarItem("", (string)Application.Current.Resources["NB_check"], new Action(this.ChangeSelection), 0, 0);
			base.ToolbarItems.Add(this.tbiSelectionModeChange);
			base.ToolbarItems.Add(new ToolbarItem("", (string)Application.Current.Resources["NB_delete"], delegate
			{
				this.DeleteAll();
			}, 0, 0));
			base.ToolbarItems.Add(new ToolbarItem("", (string)Application.Current.Resources["NB_info"], delegate
			{
				this.btnInfo_Clicked(null, null);
			}, 0, 0));
			this.LoadList();
		}

		// Token: 0x06001BBE RID: 7102 RVA: 0x0013BD30 File Offset: 0x00139F30
		private void ChangeSelection()
		{
			this.IsSelectionMode = !this.IsSelectionMode;
			if (this.IsSelectionMode)
			{
				int num = base.ToolbarItems.IndexOf(this.tbiSelectionModeChange);
				this.tbiSelectionModeChange = new ToolbarItem("", (string)Application.Current.Resources["NB_uncheck"], new Action(this.ChangeSelection), 0, 0);
				base.ToolbarItems[num] = this.tbiSelectionModeChange;
				return;
			}
			foreach (FileToRecordProxy fileToRecordProxy in this.Model.RecorderedTracks)
			{
				fileToRecordProxy.IsSelected = false;
			}
			this.lvRecords.SelectedItem = null;
			int num2 = base.ToolbarItems.IndexOf(this.tbiSelectionModeChange);
			this.tbiSelectionModeChange = new ToolbarItem("", (string)Application.Current.Resources["NB_check"], new Action(this.ChangeSelection), 0, 0);
			base.ToolbarItems[num2] = this.tbiSelectionModeChange;
		}

		// Token: 0x17000FA8 RID: 4008
		// (get) Token: 0x06001BBF RID: 7103 RVA: 0x0013BE5C File Offset: 0x0013A05C
		// (set) Token: 0x06001BC0 RID: 7104 RVA: 0x0013BE73 File Offset: 0x0013A073
		public bool IsSelectionMode
		{
			get
			{
				return this.Model != null && this.Model.IsSelectionMode;
			}
			set
			{
				if (value != this.IsSelectionMode)
				{
					this.Model.IsSelectionMode = value;
					this.OnPropertyChanged("IsSelectionMode");
				}
			}
		}

		// Token: 0x06001BC1 RID: 7105 RVA: 0x0013BE98 File Offset: 0x0013A098
		private async void LoadList()
		{
			this.activityFrame.IsVisible = true;
			await Task.Run(delegate
			{
				this.tracksModel.LoadList();
			});
			Device.BeginInvokeOnMainThread(delegate
			{
				this.lvRecords.ItemsSource = null;
				BindableObjectExtensions.SetBinding(this.lvRecords, ItemsView<Cell>.ItemsSourceProperty, "RecorderedTracks", 2, null, null);
				this.activityFrame.IsVisible = false;
			});
		}

		// Token: 0x17000FA9 RID: 4009
		// (get) Token: 0x06001BC2 RID: 7106 RVA: 0x0013BECF File Offset: 0x0013A0CF
		private RecorderedTracksViewModel Model
		{
			get
			{
				return this.tracksModel;
			}
		}

		// Token: 0x06001BC3 RID: 7107 RVA: 0x0013BED8 File Offset: 0x0013A0D8
		private void LvRecords_ItemAppearing(object sender, ItemVisibilityEventArgs e)
		{
			FileToRecordProxy fileToRecordProxy = e.Item as FileToRecordProxy;
			if (fileToRecordProxy != null)
			{
				if (!fileToRecordProxy.SizeLoaded)
				{
					fileToRecordProxy.LoadSize();
				}
				if (!fileToRecordProxy.MetaLoaded)
				{
					fileToRecordProxy.LoadMeta();
				}
			}
		}

		// Token: 0x06001BC4 RID: 7108 RVA: 0x0013BF10 File Offset: 0x0013A110
		private async void SettingsRecording_Appearing(object sender, EventArgs e)
		{
			await this.UpdateGPSState();
			await Task.Delay(1500);
		}

		// Token: 0x06001BC5 RID: 7109 RVA: 0x0013BF48 File Offset: 0x0013A148
		private async void gridHeader_SizeChanged(object sender, EventArgs e)
		{
			await Task.Delay(1000);
			this.gridHeader.HeightRequest = this.settings.VisibleContentHeight;
		}

		// Token: 0x06001BC6 RID: 7110 RVA: 0x0013BF80 File Offset: 0x0013A180
		private async void RecordingCB_Tapped(object sender, EventArgs e)
		{
			if (!SharedSettings.Current.RecordData && App.OBDReader.CurrentStatus == OBDDataReaderStatus.ConnectedToECU && App.OBDReader.CurrentCarData.Recorder == null)
			{
				DataRecorderV2.StartRecording();
			}
			else
			{
				DataRecorderV2 recorder = App.OBDReader.CurrentCarData.Recorder;
				App.OBDReader.CurrentCarData.Recorder = null;
				if (recorder != null)
				{
					recorder.Stop();
				}
				this.activityFrame.IsVisible = true;
				await Task.Delay(500);
				this.LoadList();
			}
		}

		// Token: 0x06001BC7 RID: 7111 RVA: 0x0013BFB7 File Offset: 0x0013A1B7
		private void btnInfo_Clicked(object sender, EventArgs e)
		{
			base.DisplayAlert(Translate.GetString("Settings_Control_DataRecording.Content"), Translate.GetString("ios_RecordData_InfoText"), "OK");
		}

		// Token: 0x06001BC8 RID: 7112 RVA: 0x0013BFDC File Offset: 0x0013A1DC
		private async void DeleteAll()
		{
			TaskAwaiter<bool> taskAwaiter = base.DisplayAlert(Translate.GetString("ios_DeleteData_Title"), Translate.GetString("ios_DeleteData_Text"), Translate.GetString("ios_Yes"), Translate.GetString("ios_No")).GetAwaiter();
			if (!taskAwaiter.IsCompleted)
			{
				await taskAwaiter;
				TaskAwaiter<bool> taskAwaiter2;
				taskAwaiter = taskAwaiter2;
				taskAwaiter2 = default(TaskAwaiter<bool>);
			}
			if (taskAwaiter.GetResult())
			{
				this.activityFrame.IsVisible = true;
				await Task.Delay(100);
				foreach (FileToRecordProxy fileToRecordProxy in this.tracksModel.RecorderedTracks)
				{
					File.Delete(fileToRecordProxy.Path);
				}
				this.tracksModel.LoadList();
				this.activityFrame.IsVisible = false;
				await Task.Delay(100);
			}
		}

		// Token: 0x06001BC9 RID: 7113 RVA: 0x0013C014 File Offset: 0x0013A214
		private async void btnDelete_Clicked(object sender, EventArgs e)
		{
			try
			{
				FileToRecordProxy fileToRecordProxy = (sender as MenuItem).BindingContext as FileToRecordProxy;
				File.Delete(fileToRecordProxy.Path);
				this.tracksModel.RecorderedTracks.Remove(fileToRecordProxy);
			}
			catch (Exception)
			{
			}
		}

		// Token: 0x06001BCA RID: 7114 RVA: 0x0013C054 File Offset: 0x0013A254
		private async void btnRename_Clicked(object sender, EventArgs e)
		{
			SettingsRecordRename settingsRecordRename = new SettingsRecordRename(new FileSystemElement(((sender as MenuItem).BindingContext as FileToRecordProxy).Path, FileSystemElementType.File, false), delegate
			{
				this.LoadList();
			});
			await base.Navigation.PushAsync(settingsRecordRename);
		}

		// Token: 0x06001BCB RID: 7115 RVA: 0x0013C094 File Offset: 0x0013A294
		private async void btnShare_Clicked(object sender, EventArgs e)
		{
			SettingsRecording.<>c__DisplayClass18_0 CS$<>8__locals1 = new SettingsRecording.<>c__DisplayClass18_0();
			CS$<>8__locals1.<>4__this = this;
			CS$<>8__locals1.fileproxy = (sender as MenuItem).BindingContext as FileToRecordProxy;
			if (App.OBDReader != null && App.OBDReader.CurrentCarData != null && App.OBDReader.CurrentCarData.Recorder != null && CS$<>8__locals1.fileproxy.Name == Path.GetFileNameWithoutExtension(App.OBDReader.CurrentCarData.Recorder.FileName))
			{
				await base.DisplayAlert("File is busy!", "Please disconnect first or restart Car Scanner!", "OK");
				this.lvRecords.SelectedItem = null;
			}
			else
			{
				string[] array = new string[] { "CSV #1", "CSV #2", "BRC" };
				string format = await this.DisplayActionSheetCustom(Translate.GetString("ios_SelectExportFormat"), Translate.GetString("btnCancel.Content"), null, array);
				if (format == "BRC")
				{
					string path = CS$<>8__locals1.fileproxy.Path;
					if (PlatformHelper.IsAndroid)
					{
						string exportVariantSend = Translate.GetString("droid_ExportSend");
						string exportVariantFile = Translate.GetString("droid_ExportFile");
						string exportVariantResult = await this.DisplayActionSheetCustom(Translate.GetString("droid_ExportMode"), null, null, new string[] { exportVariantFile, exportVariantSend });
						if (exportVariantResult == exportVariantFile)
						{
							await Share.RequestAsync(new ShareFileRequest(new ShareFile(path)));
						}
						if (exportVariantResult == exportVariantSend)
						{
							EmailMessage emailMessage = new EmailMessage(CS$<>8__locals1.fileproxy.Name, "", Array.Empty<string>());
							if (File.Exists(path))
							{
								emailMessage.Attachments.Add(new EmailAttachment(path));
							}
							try
							{
								await Email.ComposeAsync(emailMessage);
							}
							catch (Exception)
							{
							}
						}
						exportVariantSend = null;
						exportVariantFile = null;
						exportVariantResult = null;
					}
					else if (Device.Idiom == 1)
					{
						Share.RequestAsync(new ShareFileRequest(new ShareFile(path)));
					}
					else
					{
						PlatformHelper.IOSService.SendDebugEmail(path, "", "", "");
					}
					path = null;
				}
				else if (format == "CSV #1" || format == "CSV #2")
				{
					SettingsRecording.<>c__DisplayClass18_1 CS$<>8__locals2 = new SettingsRecording.<>c__DisplayClass18_1();
					CS$<>8__locals2.CS$<>8__locals1 = CS$<>8__locals1;
					this.activityFrame.IsVisible = true;
					await Task.Delay(150);
					CS$<>8__locals2.recorder = null;
					string text = Path.GetExtension(CS$<>8__locals2.CS$<>8__locals1.fileproxy.Path).ToLowerInvariant();
					if (text == ".rec")
					{
						await Task.Run(delegate
						{
							CS$<>8__locals2.recorder = RecorderedTracksViewModel.LoadRecorderFromFile(CS$<>8__locals2.CS$<>8__locals1.fileproxy.Path);
						});
					}
					else
					{
						if (!(text == ".brc"))
						{
							await base.DisplayAlert("Error loading data record.", "Wrong extention", "OK");
							this.lvRecords.SelectedItem = null;
							this.activityFrame.IsVisible = false;
							this.lvRecords.IsEnabled = true;
							return;
						}
						await Task.Run(delegate
						{
							if (BRCHelper.GetVersion(CS$<>8__locals2.CS$<>8__locals1.fileproxy.Path) == BRCHelper.BRCType.V1)
							{
								CS$<>8__locals2.recorder = DataRecorder.LoadFromFile(CS$<>8__locals2.CS$<>8__locals1.fileproxy.Path);
								return;
							}
							CS$<>8__locals2.recorder = DataRecordContainer.LoadFromFile(CS$<>8__locals2.CS$<>8__locals1.fileproxy.Path, false);
						});
					}
					if (CS$<>8__locals2.recorder == null)
					{
						await base.DisplayAlert("Error loading data record.", "Wrong extention", "OK");
						this.lvRecords.SelectedItem = null;
						this.activityFrame.IsVisible = false;
						this.lvRecords.IsEnabled = true;
						return;
					}
					CS$<>8__locals2.path = "";
					CS$<>8__locals2.progress = new Progress<int>(delegate(int p)
					{
						Device.BeginInvokeOnMainThread(delegate
						{
							CS$<>8__locals2.CS$<>8__locals1.<>4__this.activityFrame.Text = Translate.GetString("ios_PleaseWait") + "\n" + p.ToString() + "%";
						});
					});
					CS$<>8__locals2.format_version = 2;
					if (format == "CSV #1")
					{
						CS$<>8__locals2.format_version = 1;
					}
					if (format == "CSV #2")
					{
						CS$<>8__locals2.format_version = 2;
					}
					await Task.Run(delegate
					{
						CS$<>8__locals2.path = Brc2CsvConverter.Convert(CS$<>8__locals2.recorder, CS$<>8__locals2.CS$<>8__locals1.fileproxy.Name, CS$<>8__locals2.progress, CS$<>8__locals2.format_version);
					});
					if (PlatformHelper.IsAndroid)
					{
						string path = Translate.GetString("droid_ExportSend");
						string exportVariantResult = Translate.GetString("droid_ExportFile");
						string exportVariantFile = await this.DisplayActionSheetCustom(Translate.GetString("droid_ExportMode"), null, null, new string[] { exportVariantResult, path });
						if (exportVariantFile == exportVariantResult)
						{
							try
							{
								await Share.RequestAsync(new ShareFileRequest(new ShareFile(CS$<>8__locals2.path)));
							}
							catch (Exception)
							{
							}
						}
						if (exportVariantFile == path)
						{
							EmailMessage emailMessage2 = new EmailMessage(CS$<>8__locals2.CS$<>8__locals1.fileproxy.Name, "", Array.Empty<string>());
							if (File.Exists(CS$<>8__locals2.path))
							{
								emailMessage2.Attachments.Add(new EmailAttachment(CS$<>8__locals2.path));
							}
							try
							{
								await Email.ComposeAsync(emailMessage2);
							}
							catch (Exception)
							{
							}
						}
						path = null;
						exportVariantResult = null;
						exportVariantFile = null;
					}
					else if (Device.Idiom == 1)
					{
						await Share.RequestAsync(new ShareFileRequest(new ShareFile(CS$<>8__locals2.path)));
					}
					else
					{
						PlatformHelper.IOSService.SendDebugEmail(CS$<>8__locals2.path, "", "", "");
					}
					CS$<>8__locals2 = null;
				}
				else if (format == "REMOVE NAN")
				{
					SettingsRecording.<>c__DisplayClass18_3 CS$<>8__locals3 = new SettingsRecording.<>c__DisplayClass18_3();
					CS$<>8__locals3.CS$<>8__locals3 = CS$<>8__locals1;
					CS$<>8__locals3.recorder = null;
					string text2 = Path.GetExtension(CS$<>8__locals3.CS$<>8__locals3.fileproxy.Path).ToLowerInvariant();
					if (text2 == ".rec")
					{
						await Task.Run(delegate
						{
							CS$<>8__locals3.recorder = RecorderedTracksViewModel.LoadRecorderFromFile(CS$<>8__locals3.CS$<>8__locals3.fileproxy.Path);
						});
					}
					else if (text2 == ".brc")
					{
						await Task.Run(delegate
						{
							if (BRCHelper.GetVersion(CS$<>8__locals3.CS$<>8__locals3.fileproxy.Path) == BRCHelper.BRCType.V1)
							{
								CS$<>8__locals3.recorder = DataRecorder.LoadFromFile(CS$<>8__locals3.CS$<>8__locals3.fileproxy.Path);
								return;
							}
							CS$<>8__locals3.recorder = DataRecordContainer.LoadFromFile(CS$<>8__locals3.CS$<>8__locals3.fileproxy.Path, false);
						});
					}
					NanExcluderFromBRC.Exclude(CS$<>8__locals3.CS$<>8__locals3.fileproxy.Path, CS$<>8__locals3.recorder);
					this.LoadList();
					CS$<>8__locals3 = null;
				}
				this.activityFrame.IsVisible = false;
			}
		}

		// Token: 0x06001BCC RID: 7116 RVA: 0x0013C0D4 File Offset: 0x0013A2D4
		private async void lv_ItemTapped(object sender, ItemTappedEventArgs e)
		{
			if (e.Item != null)
			{
				FileToRecordProxy fileproxy = e.Item as FileToRecordProxy;
				if (this.Model.IsSelectionMode)
				{
					fileproxy.IsSelected = !fileproxy.IsSelected;
					this.lvRecords.SelectedItem = null;
				}
				else
				{
					IDataRecordContainer recorder = null;
					if (App.OBDReader != null && App.OBDReader.CurrentCarData != null && App.OBDReader.CurrentCarData.Recorder != null && fileproxy.Name == Path.GetFileNameWithoutExtension(App.OBDReader.CurrentCarData.Recorder.FileName))
					{
						await base.DisplayAlert("File is busy!", "Please disconnect first or restart Car Scanner!", "OK");
						this.lvRecords.SelectedItem = null;
					}
					else
					{
						this.activityFrame.IsVisible = true;
						this.lvRecords.IsEnabled = false;
						string text = Path.GetExtension(fileproxy.Path).ToLowerInvariant();
						if (text == ".rec")
						{
							await Task.Run(delegate
							{
								recorder = RecorderedTracksViewModel.LoadRecorderFromFile(fileproxy.Path);
							});
						}
						else
						{
							if (!(text == ".brc"))
							{
								await base.DisplayAlert("Error loading data record.", "Wrong extention", "OK");
								this.lvRecords.SelectedItem = null;
								this.activityFrame.IsVisible = false;
								this.lvRecords.IsEnabled = true;
								return;
							}
							await Task.Run(delegate
							{
								try
								{
									BRCHelper.BRCType version = BRCHelper.GetVersion(fileproxy.Path);
									if (version != BRCHelper.BRCType.V1)
									{
										if (version == BRCHelper.BRCType.V2)
										{
											recorder = DataRecordContainer.LoadFromFile(fileproxy.Path, false);
										}
									}
									else
									{
										recorder = DataRecorder.LoadFromFile(fileproxy.Path);
									}
								}
								catch (Exception)
								{
								}
							});
						}
						if (recorder != null)
						{
							DataRecorder.AdaptToNewUnits(recorder);
							DataRecordContainer.RemoveNaNs(recorder);
							DataRecorderSettingsPage dataRecorderSettingsPage = new DataRecorderSettingsPage();
							dataRecorderSettingsPage.BindingContext = recorder;
							this.lvRecords.SelectedItem = null;
							this.activityFrame.IsVisible = false;
							this.lvRecords.IsEnabled = true;
							await base.Navigation.PushAsync(dataRecorderSettingsPage);
						}
						else
						{
							await base.DisplayAlert("Error loading data record.", "Corruped data or file is locked.\nTry to restart Car Scanner if this file is in use right now", "OK");
							this.lvRecords.SelectedItem = null;
							this.activityFrame.IsVisible = false;
							this.lvRecords.IsEnabled = true;
						}
					}
				}
			}
		}

		// Token: 0x06001BCD RID: 7117 RVA: 0x0013C114 File Offset: 0x0013A314
		private async void Handle_Toggled(object sender, ToggledEventArgs e)
		{
			if (e.Value && App.OBDReader.CurrentStatus == OBDDataReaderStatus.ConnectedToECU && App.OBDReader.CurrentCarData.Recorder == null)
			{
				DataRecorderV2.StartRecording();
			}
			if (!e.Value)
			{
				DataRecorderV2 recorder = App.OBDReader.CurrentCarData.Recorder;
				App.OBDReader.CurrentCarData.Recorder = null;
				if (recorder != null)
				{
					recorder.Stop();
				}
				this.activityFrame.IsVisible = true;
				this.LoadList();
			}
		}

		// Token: 0x06001BCE RID: 7118 RVA: 0x0013C154 File Offset: 0x0013A354
		private async void btnImportFromFile_Clicked(object sender, EventArgs e)
		{
			try
			{
				FileResult fileResult = await FilePicker.PickAsync(null);
				if (fileResult != null && ".brc".Equals(Path.GetExtension(fileResult.FullPath), StringComparison.OrdinalIgnoreCase))
				{
					File.Copy(fileResult.FullPath, FileSystemHelper.GetLocalFilePath(fileResult.FileName));
					this.LoadList();
				}
			}
			catch (Exception)
			{
			}
		}

		// Token: 0x06001BCF RID: 7119 RVA: 0x0013C18C File Offset: 0x0013A38C
		private async Task UpdateGPSState()
		{
			TaskAwaiter<PermissionStatus> taskAwaiter = Permissions.CheckStatusAsync<Permissions.LocationWhenInUse>().GetAwaiter();
			if (!taskAwaiter.IsCompleted)
			{
				await taskAwaiter;
				TaskAwaiter<PermissionStatus> taskAwaiter2;
				taskAwaiter = taskAwaiter2;
				taskAwaiter2 = default(TaskAwaiter<PermissionStatus>);
			}
			if (taskAwaiter.GetResult() == 3)
			{
				this.HasGPSAccess = true;
			}
			else
			{
				this.HasGPSAccess = false;
			}
		}

		// Token: 0x17000FAA RID: 4010
		// (get) Token: 0x06001BD0 RID: 7120 RVA: 0x0013C1CF File Offset: 0x0013A3CF
		// (set) Token: 0x06001BD1 RID: 7121 RVA: 0x0013C1D8 File Offset: 0x0013A3D8
		public bool HasGPSAccess
		{
			get
			{
				return this._HasGPSAccess;
			}
			set
			{
				this._HasGPSAccess = value;
				if (this._HasGPSAccess)
				{
					this.cellGPSPermissionStatus.Description = Translate.GetString("settings_GPSPermissionStatus") + " " + Translate.GetString("settings_Granted");
					this.cellGPSPermissionStatus.DescriptionColor = (Color)Application.Current.Resources["GreenTextColor"];
				}
				else
				{
					this.cellGPSPermissionStatus.Description = Translate.GetString("settings_GPSPermissionStatus") + " " + Translate.GetString("settings_NotGranted");
					this.cellGPSPermissionStatus.DescriptionColor = (Color)Application.Current.Resources["RedTextColor"];
				}
				this.OnPropertyChanged("HasGPSAccess");
			}
		}

		// Token: 0x17000FAB RID: 4011
		// (get) Token: 0x06001BD2 RID: 7122 RVA: 0x0013C29B File Offset: 0x0013A49B
		// (set) Token: 0x06001BD3 RID: 7123 RVA: 0x0013C2A8 File Offset: 0x0013A4A8
		public bool RecordLocationDataProxy
		{
			get
			{
				return SharedSettings.Current.RecordLocationData;
			}
			set
			{
				if (value == SharedSettings.Current.RecordLocationData)
				{
					return;
				}
				if (value)
				{
					if (this.HasGPSAccess)
					{
						SharedSettings.Current.RecordLocationData = true;
						SharedSettings.Current.UseGPS = true;
					}
					else
					{
						this.AskForGPSPermission();
					}
				}
				else
				{
					SharedSettings.Current.RecordLocationData = false;
				}
				this.OnPropertyChanged("RecordLocationDataProxy");
			}
		}

		// Token: 0x06001BD4 RID: 7124 RVA: 0x0013C308 File Offset: 0x0013A508
		private async Task AskForGPSPermission()
		{
			TaskAwaiter<PermissionStatus> taskAwaiter = Permissions.RequestAsync<Permissions.LocationWhenInUse>().GetAwaiter();
			if (!taskAwaiter.IsCompleted)
			{
				await taskAwaiter;
				TaskAwaiter<PermissionStatus> taskAwaiter2;
				taskAwaiter = taskAwaiter2;
				taskAwaiter2 = default(TaskAwaiter<PermissionStatus>);
			}
			if (taskAwaiter.GetResult() == 3)
			{
				this.HasGPSAccess = true;
				this.RecordLocationDataProxy = true;
			}
			else
			{
				this.HasGPSAccess = false;
				this.RecordLocationDataProxy = false;
			}
		}

		// Token: 0x06001BD5 RID: 7125 RVA: 0x0013C34C File Offset: 0x0013A54C
		private async void CellGPSPermissionStatus_Tapped(object sender, EventArgs e)
		{
			this.cellGPSPermissionStatus.IsEnabled = false;
			await this.UpdateGPSState();
			if (!this.HasGPSAccess)
			{
				PermissionHelper.OpenPermissionsSettings();
			}
			this.cellGPSPermissionStatus.IsEnabled = true;
		}

		// Token: 0x06001BD6 RID: 7126 RVA: 0x0013C384 File Offset: 0x0013A584
		private async void btnShareSelected_Clicked(object sender, EventArgs e)
		{
			if (this.Model.IsSelectionMode)
			{
				List<FileToRecordProxy> selected = this.Model.RecorderedTracks.Where((FileToRecordProxy x) => x.IsSelected).ToList<FileToRecordProxy>();
				if (selected != null && selected.Count != 0)
				{
					string[] array = new string[] { "CSV #1", "CSV #2", "BRC" };
					string text = await this.DisplayActionSheetCustom(Translate.GetString("ios_SelectExportFormat"), Translate.GetString("btnCancel.Content"), null, array);
					string format = text;
					if (!(format == Translate.GetString("btnCancel.Content")))
					{
						string output_path = FileSystemHelper.GetCacheFilePath("exported_records.zip");
						if (File.Exists(output_path))
						{
							try
							{
								File.Delete(output_path);
							}
							catch (Exception)
							{
								return;
							}
						}
						IProgress<int> progress = new Progress<int>(delegate(int percent)
						{
							Device.BeginInvokeOnMainThread(delegate
							{
								this.activityFrame.Text = Translate.GetString("ios_PleaseWait") + "\n" + percent.ToString() + "%";
							});
						});
						this.activityFrame.IsVisible = true;
						this.lvRecords.IsEnabled = false;
						try
						{
							int num = 0;
							try
							{
								using (FileStream fstream = File.Open(output_path, FileMode.Create))
								{
									using (ZipOutputStream zstream = new ZipOutputStream(fstream))
									{
										zstream.UseZip64 = 0;
										zstream.SetLevel(5);
										int i = 0;
										while (i < selected.Count)
										{
											SettingsRecording.<>c__DisplayClass32_1 CS$<>8__locals1 = new SettingsRecording.<>c__DisplayClass32_1();
											CS$<>8__locals1.input_path = null;
											CS$<>8__locals1.input_title = null;
											if (format == "BRC")
											{
												CS$<>8__locals1.input_path = selected[i].Path;
												CS$<>8__locals1.input_title = Path.GetFileName(CS$<>8__locals1.input_path);
												goto IL_060C;
											}
											if (!(format == "CSV #1") && !(format == "CSV #2"))
											{
												goto IL_060C;
											}
											CS$<>8__locals1.fileproxy = selected[i];
											CS$<>8__locals1.recorder = null;
											string text2 = Path.GetExtension(CS$<>8__locals1.fileproxy.Path).ToLowerInvariant();
											if (text2 == ".rec")
											{
												await Task.Run(delegate
												{
													CS$<>8__locals1.recorder = RecorderedTracksViewModel.LoadRecorderFromFile(CS$<>8__locals1.fileproxy.Path);
												});
											}
											else
											{
												if (!(text2 == ".brc"))
												{
													await base.DisplayAlert("Error loading data record " + CS$<>8__locals1.fileproxy.Name, "Skipped", "OK");
													goto IL_0758;
												}
												await Task.Run(delegate
												{
													if (BRCHelper.GetVersion(CS$<>8__locals1.fileproxy.Path) == BRCHelper.BRCType.V1)
													{
														CS$<>8__locals1.recorder = DataRecorder.LoadFromFile(CS$<>8__locals1.fileproxy.Path);
														return;
													}
													CS$<>8__locals1.recorder = DataRecordContainer.LoadFromFile(CS$<>8__locals1.fileproxy.Path, false);
												});
											}
											if (CS$<>8__locals1.recorder != null)
											{
												CS$<>8__locals1.format_version = 2;
												if (format == "CSV #1")
												{
													CS$<>8__locals1.format_version = 1;
												}
												if (format == "CSV #2")
												{
													CS$<>8__locals1.format_version = 2;
												}
												await Task.Run(delegate
												{
													CS$<>8__locals1.input_path = Brc2CsvConverter.Convert(CS$<>8__locals1.recorder, CS$<>8__locals1.fileproxy.Name, null, CS$<>8__locals1.format_version);
													CS$<>8__locals1.input_title = Path.GetFileName(CS$<>8__locals1.input_path);
												});
												goto IL_060C;
											}
											await base.DisplayAlert("Error loading data record " + CS$<>8__locals1.fileproxy.Name, "Skipped", "OK");
											IL_0758:
											i++;
											continue;
											IL_060C:
											if (CS$<>8__locals1.input_path != null && CS$<>8__locals1.input_title != null)
											{
												using (FileStream input_stream = File.OpenRead(CS$<>8__locals1.input_path))
												{
													zstream.PutNextEntry(new ZipEntry(CS$<>8__locals1.input_title));
													await input_stream.CopyToAsync(zstream);
													zstream.CloseEntry();
												}
												FileStream input_stream = null;
												if (Path.GetExtension(CS$<>8__locals1.input_path) == ".csv")
												{
													File.Delete(CS$<>8__locals1.input_path);
												}
												int num2 = i * 100 / selected.Count;
												IProgress<int> progress2 = progress;
												if (progress2 != null)
												{
													progress2.Report(num2);
												}
												CS$<>8__locals1 = null;
												goto IL_0758;
											}
											goto IL_0758;
										}
										await zstream.FlushAsync();
										zstream.Finish();
									}
									ZipOutputStream zstream = null;
								}
								FileStream fstream = null;
								IProgress<int> progress3 = progress;
								if (progress3 != null)
								{
									progress3.Report(100);
								}
								this.activityFrame.IsVisible = false;
								this.lvRecords.IsEnabled = true;
								if (PlatformHelper.IsAndroid)
								{
									string exportVariantSend = Translate.GetString("droid_ExportSend");
									string exportVariantFile = Translate.GetString("droid_ExportFile");
									text = await this.DisplayActionSheetCustom(Translate.GetString("droid_ExportMode"), null, null, new string[] { exportVariantFile, exportVariantSend });
									string exportVariantResult = text;
									if (exportVariantResult == exportVariantFile)
									{
										await Share.RequestAsync(new ShareFileRequest(new ShareFile(output_path)));
									}
									if (exportVariantResult == exportVariantSend)
									{
										EmailMessage emailMessage = new EmailMessage("Car Scanner records", "", Array.Empty<string>());
										if (File.Exists(output_path))
										{
											emailMessage.Attachments.Add(new EmailAttachment(output_path));
										}
										try
										{
											await Email.ComposeAsync(emailMessage);
										}
										catch (Exception)
										{
										}
									}
									exportVariantSend = null;
									exportVariantFile = null;
									exportVariantResult = null;
								}
								else if (Device.Idiom == 1)
								{
									Share.RequestAsync(new ShareFileRequest(new ShareFile(output_path)));
								}
								else
								{
									PlatformHelper.IOSService.SendDebugEmail(output_path, "", "", "");
								}
							}
							catch (Exception obj)
							{
								num = 1;
							}
							object obj;
							if (num == 1)
							{
								await base.DisplayAlert("Error", ((Exception)obj).Message, "OK");
							}
							obj = null;
						}
						finally
						{
							this.activityFrame.IsVisible = false;
							this.lvRecords.IsEnabled = true;
						}
					}
				}
			}
		}

		// Token: 0x06001BD7 RID: 7127 RVA: 0x0013C3BC File Offset: 0x0013A5BC
		private async void btnDeleteSelected_Clicked(object sender, EventArgs e)
		{
			if (this.Model.IsSelectionMode)
			{
				List<FileToRecordProxy> list = this.Model.RecorderedTracks.Where((FileToRecordProxy x) => x.IsSelected).ToList<FileToRecordProxy>();
				if (list != null && list.Count != 0)
				{
					TaskAwaiter<bool> taskAwaiter = base.DisplayAlert(Translate.GetString("records_DeleteSelected"), "", Translate.GetString("ios_Yes"), Translate.GetString("ios_No")).GetAwaiter();
					if (!taskAwaiter.IsCompleted)
					{
						await taskAwaiter;
						TaskAwaiter<bool> taskAwaiter2;
						taskAwaiter = taskAwaiter2;
						taskAwaiter2 = default(TaskAwaiter<bool>);
					}
					if (taskAwaiter.GetResult())
					{
						foreach (FileToRecordProxy fileToRecordProxy in this.Model.RecorderedTracks.Where((FileToRecordProxy x) => x.IsSelected).ToList<FileToRecordProxy>())
						{
							try
							{
								if (File.Exists(fileToRecordProxy.Path))
								{
									File.Delete(fileToRecordProxy.Path);
								}
								this.Model.RecorderedTracks.Remove(fileToRecordProxy);
							}
							catch (Exception)
							{
							}
						}
					}
				}
			}
		}

		// Token: 0x06001BD8 RID: 7128 RVA: 0x0013C3F4 File Offset: 0x0013A5F4
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private void InitializeComponent()
		{
			if (ResourceLoader.IsEnabled && ResourceLoader.CanProvideContentFor(new ResourceLoader.ResourceLoadingQuery
			{
				AssemblyName = typeof(SettingsRecording).GetTypeInfo().Assembly.GetName(),
				ResourcePath = "Settings/SettingsRecording.xaml",
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
			VisualDiagnostics.RegisterSourceInfo(translate = new Translate(), new Uri("Settings\\SettingsRecording.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 14, 5);
			DynamicResourceExtension dynamicResourceExtension;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension = new DynamicResourceExtension(), new Uri("Settings\\SettingsRecording.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 17, 5);
			BoolToNegativeConverter boolToNegativeConverter;
			VisualDiagnostics.RegisterSourceInfo(boolToNegativeConverter = new BoolToNegativeConverter(), new Uri("Settings\\SettingsRecording.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 20, 14);
			EmptyStringToFalseConverter emptyStringToFalseConverter;
			VisualDiagnostics.RegisterSourceInfo(emptyStringToFalseConverter = new EmptyStringToFalseConverter(), new Uri("Settings\\SettingsRecording.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 21, 14);
			PermissionStatusToColorConverter permissionStatusToColorConverter;
			VisualDiagnostics.RegisterSourceInfo(permissionStatusToColorConverter = new PermissionStatusToColorConverter(), new Uri("Settings\\SettingsRecording.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 23, 14);
			PermissionStatusToStringConverter permissionStatusToStringConverter;
			VisualDiagnostics.RegisterSourceInfo(permissionStatusToStringConverter = new PermissionStatusToStringConverter(), new Uri("Settings\\SettingsRecording.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 24, 14);
			ResourceDictionary resourceDictionary;
			VisualDiagnostics.RegisterSourceInfo(resourceDictionary = new ResourceDictionary(), new Uri("Settings\\SettingsRecording.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 19, 10);
			OnPlatform<Thickness> onPlatform;
			VisualDiagnostics.RegisterSourceInfo(onPlatform = new OnPlatform<Thickness>(), new Uri("Settings\\SettingsRecording.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 30, 18);
			RowDefinition rowDefinition;
			VisualDiagnostics.RegisterSourceInfo(rowDefinition = new RowDefinition(), new Uri("Settings\\SettingsRecording.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 36, 18);
			RowDefinition rowDefinition2;
			VisualDiagnostics.RegisterSourceInfo(rowDefinition2 = new RowDefinition(), new Uri("Settings\\SettingsRecording.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 37, 18);
			RowDefinition rowDefinition3;
			VisualDiagnostics.RegisterSourceInfo(rowDefinition3 = new RowDefinition(), new Uri("Settings\\SettingsRecording.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 38, 18);
			ReferenceExtension referenceExtension;
			VisualDiagnostics.RegisterSourceInfo(referenceExtension = new ReferenceExtension(), new Uri("Settings\\SettingsRecording.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 54, 25);
			BindingExtension bindingExtension;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension = new BindingExtension(), new Uri("Settings\\SettingsRecording.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 54, 25);
			Frame frame;
			VisualDiagnostics.RegisterSourceInfo(frame = new Frame(), new Uri("Settings\\SettingsRecording.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 63, 38);
			Translate translate2;
			VisualDiagnostics.RegisterSourceInfo(translate2 = new Translate(), new Uri("Settings\\SettingsRecording.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 67, 37);
			SharedSettings sharedSettings;
			VisualDiagnostics.RegisterSourceInfo(sharedSettings = SharedSettings.Current, new Uri("Settings\\SettingsRecording.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 68, 37);
			BindingExtension bindingExtension2;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension2 = new BindingExtension(), new Uri("Settings\\SettingsRecording.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 69, 37);
			SettingsCheckBoxCellPatched settingsCheckBoxCellPatched;
			VisualDiagnostics.RegisterSourceInfo(settingsCheckBoxCellPatched = new SettingsCheckBoxCellPatched(), new Uri("Settings\\SettingsRecording.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 66, 34);
			Translate translate3;
			VisualDiagnostics.RegisterSourceInfo(translate3 = new Translate(), new Uri("Settings\\SettingsRecording.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 74, 37);
			ReferenceExtension referenceExtension2;
			VisualDiagnostics.RegisterSourceInfo(referenceExtension2 = new ReferenceExtension(), new Uri("Settings\\SettingsRecording.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 75, 37);
			BindingExtension bindingExtension3;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension3 = new BindingExtension(), new Uri("Settings\\SettingsRecording.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 76, 37);
			SettingsCheckBoxCellPatched settingsCheckBoxCellPatched2;
			VisualDiagnostics.RegisterSourceInfo(settingsCheckBoxCellPatched2 = new SettingsCheckBoxCellPatched(), new Uri("Settings\\SettingsRecording.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 72, 34);
			Translate translate4;
			VisualDiagnostics.RegisterSourceInfo(translate4 = new Translate(), new Uri("Settings\\SettingsRecording.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 84, 47);
			On on;
			VisualDiagnostics.RegisterSourceInfo(on = new On(), new Uri("Settings\\SettingsRecording.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 87, 46);
			Translate translate5;
			VisualDiagnostics.RegisterSourceInfo(translate5 = new Translate(), new Uri("Settings\\SettingsRecording.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 88, 68);
			On on2;
			VisualDiagnostics.RegisterSourceInfo(on2 = new On(), new Uri("Settings\\SettingsRecording.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 88, 46);
			OnPlatform<string> onPlatform2;
			VisualDiagnostics.RegisterSourceInfo(onPlatform2 = new OnPlatform<string>(), new Uri("Settings\\SettingsRecording.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 86, 42);
			LabelCell labelCell;
			VisualDiagnostics.RegisterSourceInfo(labelCell = new LabelCell(), new Uri("Settings\\SettingsRecording.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 84, 34);
			Section section;
			VisualDiagnostics.RegisterSourceInfo(section = new Section(), new Uri("Settings\\SettingsRecording.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 61, 30);
			SettingsView settingsView;
			VisualDiagnostics.RegisterSourceInfo(settingsView = new SettingsView(), new Uri("Settings\\SettingsRecording.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 56, 26);
			Grid grid;
			VisualDiagnostics.RegisterSourceInfo(grid = new Grid(), new Uri("Settings\\SettingsRecording.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 52, 22);
			DataTemplate dataTemplate;
			VisualDiagnostics.RegisterSourceInfo(dataTemplate = new DataTemplate(), new Uri("Settings\\SettingsRecording.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 99, 22);
			ListView listView;
			VisualDiagnostics.RegisterSourceInfo(listView = new ListView(), new Uri("Settings\\SettingsRecording.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 44, 14);
			ActivityFrame activityFrame;
			VisualDiagnostics.RegisterSourceInfo(activityFrame = new ActivityFrame(), new Uri("Settings\\SettingsRecording.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 161, 14);
			ReferenceExtension referenceExtension3;
			VisualDiagnostics.RegisterSourceInfo(referenceExtension3 = new ReferenceExtension(), new Uri("Settings\\SettingsRecording.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 183, 17);
			BindingExtension bindingExtension4;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension4 = new BindingExtension(), new Uri("Settings\\SettingsRecording.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 190, 21);
			Translate translate6;
			VisualDiagnostics.RegisterSourceInfo(translate6 = new Translate(), new Uri("Settings\\SettingsRecording.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 191, 21);
			Button button;
			VisualDiagnostics.RegisterSourceInfo(button = new Button(), new Uri("Settings\\SettingsRecording.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 185, 18);
			Translate translate7;
			VisualDiagnostics.RegisterSourceInfo(translate7 = new Translate(), new Uri("Settings\\SettingsRecording.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 198, 21);
			On on3;
			VisualDiagnostics.RegisterSourceInfo(on3 = new On(), new Uri("Settings\\SettingsRecording.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 201, 30);
			On on4;
			VisualDiagnostics.RegisterSourceInfo(on4 = new On(), new Uri("Settings\\SettingsRecording.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 202, 30);
			OnPlatform<bool> onPlatform3;
			VisualDiagnostics.RegisterSourceInfo(onPlatform3 = new OnPlatform<bool>(), new Uri("Settings\\SettingsRecording.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 200, 26);
			Button button2;
			VisualDiagnostics.RegisterSourceInfo(button2 = new Button(), new Uri("Settings\\SettingsRecording.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 193, 18);
			BindingExtension bindingExtension5;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension5 = new BindingExtension(), new Uri("Settings\\SettingsRecording.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 212, 21);
			Translate translate8;
			VisualDiagnostics.RegisterSourceInfo(translate8 = new Translate(), new Uri("Settings\\SettingsRecording.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 213, 21);
			Button button3;
			VisualDiagnostics.RegisterSourceInfo(button3 = new Button(), new Uri("Settings\\SettingsRecording.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 207, 18);
			Grid grid2;
			VisualDiagnostics.RegisterSourceInfo(grid2 = new Grid(), new Uri("Settings\\SettingsRecording.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 181, 14);
			Grid grid3;
			VisualDiagnostics.RegisterSourceInfo(grid3 = new Grid(), new Uri("Settings\\SettingsRecording.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 28, 10);
			VisualDiagnostics.RegisterSourceInfo(this, new Uri("Settings\\SettingsRecording.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 2, 2);
			NameScope nameScope = NameScope.GetNameScope(this) ?? new NameScope();
			NameScope.SetNameScope(this, nameScope);
			nameScope.RegisterName("me", this);
			if (this.StyleId == null)
			{
				this.StyleId = "me";
			}
			nameScope.RegisterName("lvRecords", listView);
			if (listView.StyleId == null)
			{
				listView.StyleId = "lvRecords";
			}
			nameScope.RegisterName("gridHeader", grid);
			if (grid.StyleId == null)
			{
				grid.StyleId = "gridHeader";
			}
			nameScope.RegisterName("settings", settingsView);
			if (settingsView.StyleId == null)
			{
				settingsView.StyleId = "settings";
			}
			nameScope.RegisterName("cellGPSPermissionStatus", settingsCheckBoxCellPatched2);
			if (settingsCheckBoxCellPatched2.StyleId == null)
			{
				settingsCheckBoxCellPatched2.StyleId = "cellGPSPermissionStatus";
			}
			nameScope.RegisterName("activityFrame", activityFrame);
			if (activityFrame.StyleId == null)
			{
				activityFrame.StyleId = "activityFrame";
			}
			nameScope.RegisterName("btnShareSelected", button);
			if (button.StyleId == null)
			{
				button.StyleId = "btnShareSelected";
			}
			nameScope.RegisterName("btnDeleteSelected", button3);
			if (button3.StyleId == null)
			{
				button3.StyleId = "btnDeleteSelected";
			}
			this.me = this;
			this.lvRecords = listView;
			this.gridHeader = grid;
			this.settings = settingsView;
			this.cellGPSPermissionStatus = settingsCheckBoxCellPatched2;
			this.activityFrame = activityFrame;
			this.btnShareSelected = button;
			this.btnDeleteSelected = button3;
			this.Resources = resourceDictionary;
			resourceDictionary.Add("BoolToNegativeConverter", boolToNegativeConverter);
			resourceDictionary.Add("EmptyStringToFalseConverter", emptyStringToFalseConverter);
			resourceDictionary.Add("PermissionStatusToColorConverter", permissionStatusToColorConverter);
			resourceDictionary.Add("PermissionStatusToStringConverter", permissionStatusToStringConverter);
			translate.Text = "Settings_Control_DataRecording.Content";
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
			xmlNamespaceResolver.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xmlNamespaceResolver.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver.Add("vm", "clr-namespace:CarScannerXamarinForms.ViewModels");
			xamlServiceProvider.Add(typeFromHandle2, new XamlTypeResolver(xmlNamespaceResolver, typeof(SettingsRecording).GetTypeInfo().Assembly));
			xamlServiceProvider.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(14, 5)));
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
			xmlNamespaceResolver2.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver2.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver2.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver2.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver2.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xmlNamespaceResolver2.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver2.Add("vm", "clr-namespace:CarScannerXamarinForms.ViewModels");
			xamlServiceProvider2.Add(typeFromHandle4, new XamlTypeResolver(xmlNamespaceResolver2, typeof(SettingsRecording).GetTypeInfo().Assembly));
			xamlServiceProvider2.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(17, 5)));
			DynamicResource dynamicResource = markupExtension2.ProvideValue(xamlServiceProvider2);
			this.SetDynamicResource(VisualElement.BackgroundColorProperty, dynamicResource.Key);
			this.Resources = resourceDictionary;
			onPlatform.Android = new Thickness(5.0, 0.0, 5.0, 0.0);
			onPlatform.iOS = new Thickness(5.0, 0.0, 5.0, 0.0);
			grid3.SetValue(View.MarginProperty, onPlatform);
			rowDefinition.SetValue(RowDefinition.HeightProperty, new GridLengthTypeConverter().ConvertFromInvariantString("auto"));
			grid3.GetValue(Grid.RowDefinitionsProperty).Add(rowDefinition);
			rowDefinition2.SetValue(RowDefinition.HeightProperty, new GridLengthTypeConverter().ConvertFromInvariantString("*"));
			grid3.GetValue(Grid.RowDefinitionsProperty).Add(rowDefinition2);
			rowDefinition3.SetValue(RowDefinition.HeightProperty, new GridLengthTypeConverter().ConvertFromInvariantString("auto"));
			grid3.GetValue(Grid.RowDefinitionsProperty).Add(rowDefinition3);
			listView.SetValue(Grid.RowProperty, 1);
			listView.SetValue(VisualElement.BackgroundColorProperty, Color.Transparent);
			listView.SetValue(ListView.HasUnevenRowsProperty, true);
			listView.ItemAppearing += this.LvRecords_ItemAppearing;
			listView.ItemTapped += this.lv_ItemTapped;
			referenceExtension.Name = "settings";
			IMarkupExtension markupExtension3 = referenceExtension;
			XamlServiceProvider xamlServiceProvider3 = new XamlServiceProvider();
			Type typeFromHandle5 = typeof(IProvideValueTarget);
			object[] array3 = new object[0 + 5];
			array3[0] = bindingExtension;
			array3[1] = grid;
			array3[2] = listView;
			array3[3] = grid3;
			array3[4] = this;
			object obj4;
			xamlServiceProvider3.Add(typeFromHandle5, obj4 = new SimpleValueTargetProvider(array3, typeof(BindingExtension).GetRuntimeProperty("Source"), nameScope));
			xamlServiceProvider3.Add(typeof(IReferenceProvider), obj4);
			Type typeFromHandle6 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver3 = new XmlNamespaceResolver();
			xmlNamespaceResolver3.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver3.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver3.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver3.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver3.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver3.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver3.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xmlNamespaceResolver3.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver3.Add("vm", "clr-namespace:CarScannerXamarinForms.ViewModels");
			xamlServiceProvider3.Add(typeFromHandle6, new XamlTypeResolver(xmlNamespaceResolver3, typeof(SettingsRecording).GetTypeInfo().Assembly));
			xamlServiceProvider3.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(54, 25)));
			object obj5 = markupExtension3.ProvideValue(xamlServiceProvider3);
			bindingExtension.Source = obj5;
			bindingExtension.Path = "VisibleContentHeight";
			BindingBase bindingBase = bindingExtension.ProvideValue(null);
			grid.SetBinding(VisualElement.HeightRequestProperty, bindingBase);
			grid.SizeChanged += this.gridHeader_SizeChanged;
			settingsView.SetValue(Grid.RowProperty, 0);
			settingsView.SetValue(TableView.HasUnevenRowsProperty, true);
			settingsView.SetValue(SettingsView.HeaderHeightProperty, 0.0);
			section.SetValue(SectionBase.TitleProperty, "");
			section.SetValue(Section.FooterVisibleProperty, false);
			frame.SetValue(VisualElement.HeightRequestProperty, 1.0);
			section.SetValue(Section.FooterViewProperty, frame);
			translate2.Text = "Settings_Control_tbRecordData.Text";
			IMarkupExtension markupExtension4 = translate2;
			XamlServiceProvider xamlServiceProvider4 = new XamlServiceProvider();
			Type typeFromHandle7 = typeof(IProvideValueTarget);
			object[] array4 = new object[0 + 7];
			array4[0] = settingsCheckBoxCellPatched;
			array4[1] = section;
			array4[2] = settingsView;
			array4[3] = grid;
			array4[4] = listView;
			array4[5] = grid3;
			array4[6] = this;
			object obj6;
			xamlServiceProvider4.Add(typeFromHandle7, obj6 = new SimpleValueTargetProvider(array4, CellBase.TitleProperty, nameScope));
			xamlServiceProvider4.Add(typeof(IReferenceProvider), obj6);
			Type typeFromHandle8 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver4 = new XmlNamespaceResolver();
			xmlNamespaceResolver4.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver4.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver4.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver4.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver4.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver4.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver4.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xmlNamespaceResolver4.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver4.Add("vm", "clr-namespace:CarScannerXamarinForms.ViewModels");
			xamlServiceProvider4.Add(typeFromHandle8, new XamlTypeResolver(xmlNamespaceResolver4, typeof(SettingsRecording).GetTypeInfo().Assembly));
			xamlServiceProvider4.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(67, 37)));
			object obj7 = markupExtension4.ProvideValue(xamlServiceProvider4);
			settingsCheckBoxCellPatched.Title = obj7;
			settingsCheckBoxCellPatched.SetValue(BindableObject.BindingContextProperty, sharedSettings);
			bindingExtension2.Mode = 1;
			bindingExtension2.Path = "RecordData";
			BindingBase bindingBase2 = bindingExtension2.ProvideValue(null);
			settingsCheckBoxCellPatched.SetBinding(CheckboxCell.CheckedProperty, bindingBase2);
			settingsCheckBoxCellPatched.Tapped += this.RecordingCB_Tapped;
			section.Add(settingsCheckBoxCellPatched);
			translate3.Text = "records_RecordGPS";
			IMarkupExtension markupExtension5 = translate3;
			XamlServiceProvider xamlServiceProvider5 = new XamlServiceProvider();
			Type typeFromHandle9 = typeof(IProvideValueTarget);
			object[] array5 = new object[0 + 7];
			array5[0] = settingsCheckBoxCellPatched2;
			array5[1] = section;
			array5[2] = settingsView;
			array5[3] = grid;
			array5[4] = listView;
			array5[5] = grid3;
			array5[6] = this;
			object obj8;
			xamlServiceProvider5.Add(typeFromHandle9, obj8 = new SimpleValueTargetProvider(array5, CellBase.TitleProperty, nameScope));
			xamlServiceProvider5.Add(typeof(IReferenceProvider), obj8);
			Type typeFromHandle10 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver5 = new XmlNamespaceResolver();
			xmlNamespaceResolver5.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver5.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver5.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver5.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver5.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver5.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver5.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xmlNamespaceResolver5.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver5.Add("vm", "clr-namespace:CarScannerXamarinForms.ViewModels");
			xamlServiceProvider5.Add(typeFromHandle10, new XamlTypeResolver(xmlNamespaceResolver5, typeof(SettingsRecording).GetTypeInfo().Assembly));
			xamlServiceProvider5.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(74, 37)));
			object obj9 = markupExtension5.ProvideValue(xamlServiceProvider5);
			settingsCheckBoxCellPatched2.Title = obj9;
			referenceExtension2.Name = "me";
			IMarkupExtension markupExtension6 = referenceExtension2;
			XamlServiceProvider xamlServiceProvider6 = new XamlServiceProvider();
			Type typeFromHandle11 = typeof(IProvideValueTarget);
			object[] array6 = new object[0 + 7];
			array6[0] = settingsCheckBoxCellPatched2;
			array6[1] = section;
			array6[2] = settingsView;
			array6[3] = grid;
			array6[4] = listView;
			array6[5] = grid3;
			array6[6] = this;
			object obj10;
			xamlServiceProvider6.Add(typeFromHandle11, obj10 = new SimpleValueTargetProvider(array6, BindableObject.BindingContextProperty, nameScope));
			xamlServiceProvider6.Add(typeof(IReferenceProvider), obj10);
			Type typeFromHandle12 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver6 = new XmlNamespaceResolver();
			xmlNamespaceResolver6.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver6.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver6.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver6.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver6.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver6.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver6.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xmlNamespaceResolver6.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver6.Add("vm", "clr-namespace:CarScannerXamarinForms.ViewModels");
			xamlServiceProvider6.Add(typeFromHandle12, new XamlTypeResolver(xmlNamespaceResolver6, typeof(SettingsRecording).GetTypeInfo().Assembly));
			xamlServiceProvider6.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(75, 37)));
			object obj11 = markupExtension6.ProvideValue(xamlServiceProvider6);
			settingsCheckBoxCellPatched2.SetValue(BindableObject.BindingContextProperty, obj11);
			bindingExtension3.Mode = 1;
			bindingExtension3.Path = "RecordLocationDataProxy";
			BindingBase bindingBase3 = bindingExtension3.ProvideValue(null);
			settingsCheckBoxCellPatched2.SetBinding(CheckboxCell.CheckedProperty, bindingBase3);
			section.Add(settingsCheckBoxCellPatched2);
			translate4.Text = "set_RecordedFiles";
			IMarkupExtension markupExtension7 = translate4;
			XamlServiceProvider xamlServiceProvider7 = new XamlServiceProvider();
			Type typeFromHandle13 = typeof(IProvideValueTarget);
			object[] array7 = new object[0 + 7];
			array7[0] = labelCell;
			array7[1] = section;
			array7[2] = settingsView;
			array7[3] = grid;
			array7[4] = listView;
			array7[5] = grid3;
			array7[6] = this;
			object obj12;
			xamlServiceProvider7.Add(typeFromHandle13, obj12 = new SimpleValueTargetProvider(array7, CellBase.TitleProperty, nameScope));
			xamlServiceProvider7.Add(typeof(IReferenceProvider), obj12);
			Type typeFromHandle14 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver7 = new XmlNamespaceResolver();
			xmlNamespaceResolver7.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver7.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver7.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver7.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver7.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver7.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver7.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xmlNamespaceResolver7.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver7.Add("vm", "clr-namespace:CarScannerXamarinForms.ViewModels");
			xamlServiceProvider7.Add(typeFromHandle14, new XamlTypeResolver(xmlNamespaceResolver7, typeof(SettingsRecording).GetTypeInfo().Assembly));
			xamlServiceProvider7.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(84, 47)));
			object obj13 = markupExtension7.ProvideValue(xamlServiceProvider7);
			labelCell.Title = obj13;
			labelCell.SetValue(CellBase.TitleFontAttributesProperty, new FontAttributes?(1));
			on.Platform = new List<string>(1) { "iOS" };
			on.Value = "";
			onPlatform2.Platforms.Add(on);
			on2.Platform = new List<string>(1) { "Android" };
			translate5.Text = "records_scrollDown";
			IMarkupExtension markupExtension8 = translate5;
			XamlServiceProvider xamlServiceProvider8 = new XamlServiceProvider();
			Type typeFromHandle15 = typeof(IProvideValueTarget);
			object[] array8 = new object[0 + 9];
			array8[0] = on2;
			array8[1] = onPlatform2;
			array8[2] = labelCell;
			array8[3] = section;
			array8[4] = settingsView;
			array8[5] = grid;
			array8[6] = listView;
			array8[7] = grid3;
			array8[8] = this;
			object obj14;
			xamlServiceProvider8.Add(typeFromHandle15, obj14 = new SimpleValueTargetProvider(array8, typeof(On).GetRuntimeProperty("Value"), nameScope));
			xamlServiceProvider8.Add(typeof(IReferenceProvider), obj14);
			Type typeFromHandle16 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver8 = new XmlNamespaceResolver();
			xmlNamespaceResolver8.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver8.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver8.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver8.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver8.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver8.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver8.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xmlNamespaceResolver8.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver8.Add("vm", "clr-namespace:CarScannerXamarinForms.ViewModels");
			xamlServiceProvider8.Add(typeFromHandle16, new XamlTypeResolver(xmlNamespaceResolver8, typeof(SettingsRecording).GetTypeInfo().Assembly));
			xamlServiceProvider8.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(88, 68)));
			object obj15 = markupExtension8.ProvideValue(xamlServiceProvider8);
			on2.Value = obj15;
			onPlatform2.Platforms.Add(on2);
			labelCell.SetValue(CellBase.DescriptionProperty, onPlatform2);
			section.Add(labelCell);
			settingsView.Root.Add(section);
			grid.Children.Add(settingsView);
			listView.SetValue(ListView.HeaderProperty, grid);
			IDataTemplate dataTemplate2 = dataTemplate;
			SettingsRecording.<InitializeComponent>_anonXamlCDataTemplate_97 <InitializeComponent>_anonXamlCDataTemplate_ = new SettingsRecording.<InitializeComponent>_anonXamlCDataTemplate_97();
			object[] array9 = new object[0 + 4];
			array9[0] = dataTemplate;
			array9[1] = listView;
			array9[2] = grid3;
			array9[3] = this;
			<InitializeComponent>_anonXamlCDataTemplate_.parentValues = array9;
			<InitializeComponent>_anonXamlCDataTemplate_.root = this;
			dataTemplate2.LoadTemplate = new Func<object>(<InitializeComponent>_anonXamlCDataTemplate_.LoadDataTemplate);
			listView.SetValue(ItemsView<Cell>.ItemTemplateProperty, dataTemplate);
			grid3.Children.Add(listView);
			activityFrame.SetValue(Grid.RowProperty, 1);
			activityFrame.SetValue(VisualElement.IsVisibleProperty, new VisualElement.VisibilityConverter().ConvertFromInvariantString("false"));
			activityFrame.SetValue(View.VerticalOptionsProperty, LayoutOptions.Start);
			grid3.Children.Add(activityFrame);
			grid2.SetValue(Grid.RowProperty, 2);
			referenceExtension3.Name = "me";
			IMarkupExtension markupExtension9 = referenceExtension3;
			XamlServiceProvider xamlServiceProvider9 = new XamlServiceProvider();
			Type typeFromHandle17 = typeof(IProvideValueTarget);
			object[] array10 = new object[0 + 3];
			array10[0] = grid2;
			array10[1] = grid3;
			array10[2] = this;
			object obj16;
			xamlServiceProvider9.Add(typeFromHandle17, obj16 = new SimpleValueTargetProvider(array10, BindableObject.BindingContextProperty, nameScope));
			xamlServiceProvider9.Add(typeof(IReferenceProvider), obj16);
			Type typeFromHandle18 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver9 = new XmlNamespaceResolver();
			xmlNamespaceResolver9.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver9.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver9.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver9.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver9.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver9.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver9.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xmlNamespaceResolver9.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver9.Add("vm", "clr-namespace:CarScannerXamarinForms.ViewModels");
			xamlServiceProvider9.Add(typeFromHandle18, new XamlTypeResolver(xmlNamespaceResolver9, typeof(SettingsRecording).GetTypeInfo().Assembly));
			xamlServiceProvider9.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(183, 17)));
			object obj17 = markupExtension9.ProvideValue(xamlServiceProvider9);
			grid2.SetValue(BindableObject.BindingContextProperty, obj17);
			grid2.SetValue(Grid.ColumnDefinitionsProperty, new ColumnDefinitionCollectionTypeConverter().ConvertFromInvariantString("*,*,*"));
			button.SetValue(Grid.RowProperty, 0);
			button.SetValue(Grid.ColumnProperty, 0);
			button.Clicked += this.btnShareSelected_Clicked;
			bindingExtension4.Mode = 2;
			bindingExtension4.Path = "IsSelectionMode";
			BindingBase bindingBase4 = bindingExtension4.ProvideValue(null);
			button.SetBinding(VisualElement.IsVisibleProperty, bindingBase4);
			translate6.Text = "ios_Share";
			IMarkupExtension markupExtension10 = translate6;
			XamlServiceProvider xamlServiceProvider10 = new XamlServiceProvider();
			Type typeFromHandle19 = typeof(IProvideValueTarget);
			object[] array11 = new object[0 + 4];
			array11[0] = button;
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
			xmlNamespaceResolver10.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver10.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver10.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver10.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver10.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xmlNamespaceResolver10.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver10.Add("vm", "clr-namespace:CarScannerXamarinForms.ViewModels");
			xamlServiceProvider10.Add(typeFromHandle20, new XamlTypeResolver(xmlNamespaceResolver10, typeof(SettingsRecording).GetTypeInfo().Assembly));
			xamlServiceProvider10.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(191, 21)));
			object obj19 = markupExtension10.ProvideValue(xamlServiceProvider10);
			button.Text = obj19;
			grid2.Children.Add(button);
			button2.SetValue(Grid.RowProperty, 0);
			button2.SetValue(Grid.ColumnProperty, 1);
			button2.Clicked += this.btnImportFromFile_Clicked;
			button2.SetValue(View.HorizontalOptionsProperty, LayoutOptions.Center);
			translate7.Text = "droid_ImportFromFile";
			IMarkupExtension markupExtension11 = translate7;
			XamlServiceProvider xamlServiceProvider11 = new XamlServiceProvider();
			Type typeFromHandle21 = typeof(IProvideValueTarget);
			object[] array12 = new object[0 + 4];
			array12[0] = button2;
			array12[1] = grid2;
			array12[2] = grid3;
			array12[3] = this;
			object obj20;
			xamlServiceProvider11.Add(typeFromHandle21, obj20 = new SimpleValueTargetProvider(array12, Button.TextProperty, nameScope));
			xamlServiceProvider11.Add(typeof(IReferenceProvider), obj20);
			Type typeFromHandle22 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver11 = new XmlNamespaceResolver();
			xmlNamespaceResolver11.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver11.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver11.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver11.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver11.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver11.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver11.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xmlNamespaceResolver11.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver11.Add("vm", "clr-namespace:CarScannerXamarinForms.ViewModels");
			xamlServiceProvider11.Add(typeFromHandle22, new XamlTypeResolver(xmlNamespaceResolver11, typeof(SettingsRecording).GetTypeInfo().Assembly));
			xamlServiceProvider11.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(198, 21)));
			object obj21 = markupExtension11.ProvideValue(xamlServiceProvider11);
			button2.Text = obj21;
			on3.Platform = new List<string>(1) { "iOS" };
			on3.Value = "True";
			onPlatform3.Platforms.Add(on3);
			on4.Platform = new List<string>(1) { "Android" };
			on4.Value = "True";
			onPlatform3.Platforms.Add(on4);
			button2.SetValue(VisualElement.IsVisibleProperty, onPlatform3);
			grid2.Children.Add(button2);
			button3.SetValue(Grid.RowProperty, 0);
			button3.SetValue(Grid.ColumnProperty, 2);
			button3.Clicked += this.btnDeleteSelected_Clicked;
			bindingExtension5.Mode = 2;
			bindingExtension5.Path = "IsSelectionMode";
			BindingBase bindingBase5 = bindingExtension5.ProvideValue(null);
			button3.SetBinding(VisualElement.IsVisibleProperty, bindingBase5);
			translate8.Text = "SpeedTest_btnRemove.Label";
			IMarkupExtension markupExtension12 = translate8;
			XamlServiceProvider xamlServiceProvider12 = new XamlServiceProvider();
			Type typeFromHandle23 = typeof(IProvideValueTarget);
			object[] array13 = new object[0 + 4];
			array13[0] = button3;
			array13[1] = grid2;
			array13[2] = grid3;
			array13[3] = this;
			object obj22;
			xamlServiceProvider12.Add(typeFromHandle23, obj22 = new SimpleValueTargetProvider(array13, Button.TextProperty, nameScope));
			xamlServiceProvider12.Add(typeof(IReferenceProvider), obj22);
			Type typeFromHandle24 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver12 = new XmlNamespaceResolver();
			xmlNamespaceResolver12.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver12.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver12.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver12.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver12.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver12.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver12.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xmlNamespaceResolver12.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver12.Add("vm", "clr-namespace:CarScannerXamarinForms.ViewModels");
			xamlServiceProvider12.Add(typeFromHandle24, new XamlTypeResolver(xmlNamespaceResolver12, typeof(SettingsRecording).GetTypeInfo().Assembly));
			xamlServiceProvider12.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(213, 21)));
			object obj23 = markupExtension12.ProvideValue(xamlServiceProvider12);
			button3.Text = obj23;
			grid2.Children.Add(button3);
			grid3.Children.Add(grid2);
			this.SetValue(ContentPage.ContentProperty, grid3);
		}

		// Token: 0x06001BD9 RID: 7129 RVA: 0x0013E4D5 File Offset: 0x0013C6D5
		[CompilerGenerated]
		private void <.ctor>b__0_0()
		{
			this.DeleteAll();
		}

		// Token: 0x06001BDA RID: 7130 RVA: 0x0013E4DD File Offset: 0x0013C6DD
		[CompilerGenerated]
		private void <.ctor>b__0_1()
		{
			this.btnInfo_Clicked(null, null);
		}

		// Token: 0x06001BDB RID: 7131 RVA: 0x0013E4E7 File Offset: 0x0013C6E7
		[CompilerGenerated]
		private void <LoadList>b__6_0()
		{
			this.tracksModel.LoadList();
		}

		// Token: 0x06001BDC RID: 7132 RVA: 0x0013E4F4 File Offset: 0x0013C6F4
		[CompilerGenerated]
		private void <LoadList>b__6_1()
		{
			this.lvRecords.ItemsSource = null;
			BindableObjectExtensions.SetBinding(this.lvRecords, ItemsView<Cell>.ItemsSourceProperty, "RecorderedTracks", 2, null, null);
			this.activityFrame.IsVisible = false;
		}

		// Token: 0x06001BDD RID: 7133 RVA: 0x0013E526 File Offset: 0x0013C726
		[CompilerGenerated]
		private void <btnRename_Clicked>b__17_0()
		{
			this.LoadList();
		}

		// Token: 0x06001BDE RID: 7134 RVA: 0x0013E52E File Offset: 0x0013C72E
		[CompilerGenerated]
		private void <btnShareSelected_Clicked>b__32_1(int percent)
		{
			Device.BeginInvokeOnMainThread(delegate
			{
				this.activityFrame.Text = Translate.GetString("ios_PleaseWait") + "\n" + percent.ToString() + "%";
			});
		}

		// Token: 0x06001BDF RID: 7135 RVA: 0x0013E554 File Offset: 0x0013C754
		private void __InitComponentRuntime()
		{
			Extensions.LoadFromXaml<SettingsRecording>(this, typeof(SettingsRecording));
			this.me = NameScopeExtensions.FindByName<ContentPage>(this, "me");
			this.lvRecords = NameScopeExtensions.FindByName<ListView>(this, "lvRecords");
			this.gridHeader = NameScopeExtensions.FindByName<Grid>(this, "gridHeader");
			this.settings = NameScopeExtensions.FindByName<SettingsView>(this, "settings");
			this.cellGPSPermissionStatus = NameScopeExtensions.FindByName<SettingsCheckBoxCellPatched>(this, "cellGPSPermissionStatus");
			this.activityFrame = NameScopeExtensions.FindByName<ActivityFrame>(this, "activityFrame");
			this.btnShareSelected = NameScopeExtensions.FindByName<Button>(this, "btnShareSelected");
			this.btnDeleteSelected = NameScopeExtensions.FindByName<Button>(this, "btnDeleteSelected");
		}

		// Token: 0x04000CE5 RID: 3301
		private ToolbarItem tbiSelectionModeChange;

		// Token: 0x04000CE6 RID: 3302
		private RecorderedTracksViewModel tracksModel;

		// Token: 0x04000CE7 RID: 3303
		private bool _HasGPSAccess;

		// Token: 0x04000CE8 RID: 3304
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private ContentPage me;

		// Token: 0x04000CE9 RID: 3305
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private ListView lvRecords;

		// Token: 0x04000CEA RID: 3306
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private Grid gridHeader;

		// Token: 0x04000CEB RID: 3307
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private SettingsView settings;

		// Token: 0x04000CEC RID: 3308
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private SettingsCheckBoxCellPatched cellGPSPermissionStatus;

		// Token: 0x04000CED RID: 3309
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private ActivityFrame activityFrame;

		// Token: 0x04000CEE RID: 3310
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private Button btnShareSelected;

		// Token: 0x04000CEF RID: 3311
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private Button btnDeleteSelected;

		// Token: 0x02000251 RID: 593
		[CompilerGenerated]
		[Serializable]
		private sealed class <>c
		{
			// Token: 0x06001BE0 RID: 7136 RVA: 0x0013E5FA File Offset: 0x0013C7FA
			// Note: this type is marked as 'beforefieldinit'.
			static <>c()
			{
			}

			// Token: 0x06001BE1 RID: 7137 RVA: 0x00002050 File Offset: 0x00000250
			public <>c()
			{
			}

			// Token: 0x06001BE2 RID: 7138 RVA: 0x0013E606 File Offset: 0x0013C806
			internal bool <btnShareSelected_Clicked>b__32_0(FileToRecordProxy x)
			{
				return x.IsSelected;
			}

			// Token: 0x06001BE3 RID: 7139 RVA: 0x0013E606 File Offset: 0x0013C806
			internal bool <btnDeleteSelected_Clicked>b__33_0(FileToRecordProxy x)
			{
				return x.IsSelected;
			}

			// Token: 0x06001BE4 RID: 7140 RVA: 0x0013E606 File Offset: 0x0013C806
			internal bool <btnDeleteSelected_Clicked>b__33_1(FileToRecordProxy x)
			{
				return x.IsSelected;
			}

			// Token: 0x04000CF0 RID: 3312
			public static readonly SettingsRecording.<>c <>9 = new SettingsRecording.<>c();

			// Token: 0x04000CF1 RID: 3313
			public static Func<FileToRecordProxy, bool> <>9__32_0;

			// Token: 0x04000CF2 RID: 3314
			public static Func<FileToRecordProxy, bool> <>9__33_0;

			// Token: 0x04000CF3 RID: 3315
			public static Func<FileToRecordProxy, bool> <>9__33_1;
		}

		// Token: 0x02000252 RID: 594
		[CompilerGenerated]
		private sealed class <>c__DisplayClass18_0
		{
			// Token: 0x06001BE5 RID: 7141 RVA: 0x00002050 File Offset: 0x00000250
			public <>c__DisplayClass18_0()
			{
			}

			// Token: 0x06001BE6 RID: 7142 RVA: 0x0013E60E File Offset: 0x0013C80E
			internal void <btnShare_Clicked>b__2(int p)
			{
				Device.BeginInvokeOnMainThread(new Action(new SettingsRecording.<>c__DisplayClass18_2
				{
					CS$<>8__locals2 = this,
					p = p
				}.<btnShare_Clicked>b__4));
			}

			// Token: 0x04000CF4 RID: 3316
			public FileToRecordProxy fileproxy;

			// Token: 0x04000CF5 RID: 3317
			public SettingsRecording <>4__this;
		}

		// Token: 0x02000253 RID: 595
		[CompilerGenerated]
		private sealed class <>c__DisplayClass18_1
		{
			// Token: 0x06001BE7 RID: 7143 RVA: 0x00002050 File Offset: 0x00000250
			public <>c__DisplayClass18_1()
			{
			}

			// Token: 0x06001BE8 RID: 7144 RVA: 0x0013E633 File Offset: 0x0013C833
			internal void <btnShare_Clicked>b__0()
			{
				this.recorder = RecorderedTracksViewModel.LoadRecorderFromFile(this.CS$<>8__locals1.fileproxy.Path);
			}

			// Token: 0x06001BE9 RID: 7145 RVA: 0x0013E650 File Offset: 0x0013C850
			internal void <btnShare_Clicked>b__1()
			{
				if (BRCHelper.GetVersion(this.CS$<>8__locals1.fileproxy.Path) == BRCHelper.BRCType.V1)
				{
					this.recorder = DataRecorder.LoadFromFile(this.CS$<>8__locals1.fileproxy.Path);
					return;
				}
				this.recorder = DataRecordContainer.LoadFromFile(this.CS$<>8__locals1.fileproxy.Path, false);
			}

			// Token: 0x06001BEA RID: 7146 RVA: 0x0013E6AC File Offset: 0x0013C8AC
			internal void <btnShare_Clicked>b__3()
			{
				this.path = Brc2CsvConverter.Convert(this.recorder, this.CS$<>8__locals1.fileproxy.Name, this.progress, this.format_version);
			}

			// Token: 0x04000CF6 RID: 3318
			public IDataRecordContainer recorder;

			// Token: 0x04000CF7 RID: 3319
			public string path;

			// Token: 0x04000CF8 RID: 3320
			public Progress<int> progress;

			// Token: 0x04000CF9 RID: 3321
			public int format_version;

			// Token: 0x04000CFA RID: 3322
			public SettingsRecording.<>c__DisplayClass18_0 CS$<>8__locals1;
		}

		// Token: 0x02000254 RID: 596
		[CompilerGenerated]
		private sealed class <>c__DisplayClass18_2
		{
			// Token: 0x06001BEB RID: 7147 RVA: 0x00002050 File Offset: 0x00000250
			public <>c__DisplayClass18_2()
			{
			}

			// Token: 0x06001BEC RID: 7148 RVA: 0x0013E6DB File Offset: 0x0013C8DB
			internal void <btnShare_Clicked>b__4()
			{
				this.CS$<>8__locals2.<>4__this.activityFrame.Text = Translate.GetString("ios_PleaseWait") + "\n" + this.p.ToString() + "%";
			}

			// Token: 0x04000CFB RID: 3323
			public int p;

			// Token: 0x04000CFC RID: 3324
			public SettingsRecording.<>c__DisplayClass18_0 CS$<>8__locals2;
		}

		// Token: 0x02000255 RID: 597
		[CompilerGenerated]
		private sealed class <>c__DisplayClass18_3
		{
			// Token: 0x06001BED RID: 7149 RVA: 0x00002050 File Offset: 0x00000250
			public <>c__DisplayClass18_3()
			{
			}

			// Token: 0x06001BEE RID: 7150 RVA: 0x0013E716 File Offset: 0x0013C916
			internal void <btnShare_Clicked>b__5()
			{
				this.recorder = RecorderedTracksViewModel.LoadRecorderFromFile(this.CS$<>8__locals3.fileproxy.Path);
			}

			// Token: 0x06001BEF RID: 7151 RVA: 0x0013E734 File Offset: 0x0013C934
			internal void <btnShare_Clicked>b__6()
			{
				if (BRCHelper.GetVersion(this.CS$<>8__locals3.fileproxy.Path) == BRCHelper.BRCType.V1)
				{
					this.recorder = DataRecorder.LoadFromFile(this.CS$<>8__locals3.fileproxy.Path);
					return;
				}
				this.recorder = DataRecordContainer.LoadFromFile(this.CS$<>8__locals3.fileproxy.Path, false);
			}

			// Token: 0x04000CFD RID: 3325
			public IDataRecordContainer recorder;

			// Token: 0x04000CFE RID: 3326
			public SettingsRecording.<>c__DisplayClass18_0 CS$<>8__locals3;
		}

		// Token: 0x02000256 RID: 598
		[CompilerGenerated]
		private sealed class <>c__DisplayClass19_0
		{
			// Token: 0x06001BF0 RID: 7152 RVA: 0x00002050 File Offset: 0x00000250
			public <>c__DisplayClass19_0()
			{
			}

			// Token: 0x06001BF1 RID: 7153 RVA: 0x0013E790 File Offset: 0x0013C990
			internal void <lv_ItemTapped>b__0()
			{
				this.recorder = RecorderedTracksViewModel.LoadRecorderFromFile(this.fileproxy.Path);
			}

			// Token: 0x06001BF2 RID: 7154 RVA: 0x0013E7A8 File Offset: 0x0013C9A8
			internal void <lv_ItemTapped>b__1()
			{
				try
				{
					BRCHelper.BRCType version = BRCHelper.GetVersion(this.fileproxy.Path);
					if (version != BRCHelper.BRCType.V1)
					{
						if (version == BRCHelper.BRCType.V2)
						{
							this.recorder = DataRecordContainer.LoadFromFile(this.fileproxy.Path, false);
						}
					}
					else
					{
						this.recorder = DataRecorder.LoadFromFile(this.fileproxy.Path);
					}
				}
				catch (Exception)
				{
				}
			}

			// Token: 0x04000CFF RID: 3327
			public IDataRecordContainer recorder;

			// Token: 0x04000D00 RID: 3328
			public FileToRecordProxy fileproxy;
		}

		// Token: 0x02000257 RID: 599
		[CompilerGenerated]
		private sealed class <>c__DisplayClass32_0
		{
			// Token: 0x06001BF3 RID: 7155 RVA: 0x00002050 File Offset: 0x00000250
			public <>c__DisplayClass32_0()
			{
			}

			// Token: 0x06001BF4 RID: 7156 RVA: 0x0013E814 File Offset: 0x0013CA14
			internal void <btnShareSelected_Clicked>b__2()
			{
				this.<>4__this.activityFrame.Text = Translate.GetString("ios_PleaseWait") + "\n" + this.percent.ToString() + "%";
			}

			// Token: 0x04000D01 RID: 3329
			public int percent;

			// Token: 0x04000D02 RID: 3330
			public SettingsRecording <>4__this;
		}

		// Token: 0x02000258 RID: 600
		[CompilerGenerated]
		private sealed class <>c__DisplayClass32_1
		{
			// Token: 0x06001BF5 RID: 7157 RVA: 0x00002050 File Offset: 0x00000250
			public <>c__DisplayClass32_1()
			{
			}

			// Token: 0x06001BF6 RID: 7158 RVA: 0x0013E84A File Offset: 0x0013CA4A
			internal void <btnShareSelected_Clicked>b__3()
			{
				this.recorder = RecorderedTracksViewModel.LoadRecorderFromFile(this.fileproxy.Path);
			}

			// Token: 0x06001BF7 RID: 7159 RVA: 0x0013E864 File Offset: 0x0013CA64
			internal void <btnShareSelected_Clicked>b__4()
			{
				if (BRCHelper.GetVersion(this.fileproxy.Path) == BRCHelper.BRCType.V1)
				{
					this.recorder = DataRecorder.LoadFromFile(this.fileproxy.Path);
					return;
				}
				this.recorder = DataRecordContainer.LoadFromFile(this.fileproxy.Path, false);
			}

			// Token: 0x06001BF8 RID: 7160 RVA: 0x0013E8B1 File Offset: 0x0013CAB1
			internal void <btnShareSelected_Clicked>b__5()
			{
				this.input_path = Brc2CsvConverter.Convert(this.recorder, this.fileproxy.Name, null, this.format_version);
				this.input_title = Path.GetFileName(this.input_path);
			}

			// Token: 0x04000D03 RID: 3331
			public string input_path;

			// Token: 0x04000D04 RID: 3332
			public string input_title;

			// Token: 0x04000D05 RID: 3333
			public IDataRecordContainer recorder;

			// Token: 0x04000D06 RID: 3334
			public FileToRecordProxy fileproxy;

			// Token: 0x04000D07 RID: 3335
			public int format_version;
		}

		// Token: 0x02000259 RID: 601
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <AskForGPSPermission>d__30 : IAsyncStateMachine
		{
			// Token: 0x06001BF9 RID: 7161 RVA: 0x0013E8E8 File Offset: 0x0013CAE8
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				SettingsRecording settingsRecording = this;
				try
				{
					TaskAwaiter<PermissionStatus> taskAwaiter3;
					if (num != 0)
					{
						taskAwaiter3 = Permissions.RequestAsync<Permissions.LocationWhenInUse>().GetAwaiter();
						if (!taskAwaiter3.IsCompleted)
						{
							num2 = 0;
							taskAwaiter2 = taskAwaiter3;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<PermissionStatus>, SettingsRecording.<AskForGPSPermission>d__30>(ref taskAwaiter3, ref this);
							return;
						}
					}
					else
					{
						taskAwaiter3 = taskAwaiter2;
						taskAwaiter2 = default(TaskAwaiter<PermissionStatus>);
						num2 = -1;
					}
					if (taskAwaiter3.GetResult() == 3)
					{
						settingsRecording.HasGPSAccess = true;
						settingsRecording.RecordLocationDataProxy = true;
					}
					else
					{
						settingsRecording.HasGPSAccess = false;
						settingsRecording.RecordLocationDataProxy = false;
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

			// Token: 0x06001BFA RID: 7162 RVA: 0x0013E9BC File Offset: 0x0013CBBC
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x04000D08 RID: 3336
			public int <>1__state;

			// Token: 0x04000D09 RID: 3337
			public AsyncTaskMethodBuilder <>t__builder;

			// Token: 0x04000D0A RID: 3338
			public SettingsRecording <>4__this;

			// Token: 0x04000D0B RID: 3339
			private TaskAwaiter<PermissionStatus> <>u__1;
		}

		// Token: 0x0200025A RID: 602
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <CellGPSPermissionStatus_Tapped>d__31 : IAsyncStateMachine
		{
			// Token: 0x06001BFB RID: 7163 RVA: 0x0013E9CC File Offset: 0x0013CBCC
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				SettingsRecording settingsRecording = this;
				try
				{
					TaskAwaiter taskAwaiter;
					if (num != 0)
					{
						settingsRecording.cellGPSPermissionStatus.IsEnabled = false;
						taskAwaiter = settingsRecording.UpdateGPSState().GetAwaiter();
						if (!taskAwaiter.IsCompleted)
						{
							num2 = 0;
							TaskAwaiter taskAwaiter2 = taskAwaiter;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, SettingsRecording.<CellGPSPermissionStatus_Tapped>d__31>(ref taskAwaiter, ref this);
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
					if (!settingsRecording.HasGPSAccess)
					{
						PermissionHelper.OpenPermissionsSettings();
					}
					settingsRecording.cellGPSPermissionStatus.IsEnabled = true;
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

			// Token: 0x06001BFC RID: 7164 RVA: 0x0013EAA4 File Offset: 0x0013CCA4
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x04000D0C RID: 3340
			public int <>1__state;

			// Token: 0x04000D0D RID: 3341
			public AsyncVoidMethodBuilder <>t__builder;

			// Token: 0x04000D0E RID: 3342
			public SettingsRecording <>4__this;

			// Token: 0x04000D0F RID: 3343
			private TaskAwaiter <>u__1;
		}

		// Token: 0x0200025B RID: 603
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <DeleteAll>d__15 : IAsyncStateMachine
		{
			// Token: 0x06001BFD RID: 7165 RVA: 0x0013EAB4 File Offset: 0x0013CCB4
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				SettingsRecording settingsRecording = this;
				try
				{
					TaskAwaiter<bool> taskAwaiter3;
					TaskAwaiter taskAwaiter4;
					switch (num)
					{
					case 0:
						taskAwaiter3 = taskAwaiter2;
						taskAwaiter2 = default(TaskAwaiter<bool>);
						num = (num2 = -1);
						break;
					case 1:
					{
						TaskAwaiter taskAwaiter5;
						taskAwaiter4 = taskAwaiter5;
						taskAwaiter5 = default(TaskAwaiter);
						num = (num2 = -1);
						goto IL_0109;
					}
					case 2:
					{
						TaskAwaiter taskAwaiter5;
						taskAwaiter4 = taskAwaiter5;
						taskAwaiter5 = default(TaskAwaiter);
						num = (num2 = -1);
						goto IL_01B9;
					}
					default:
						taskAwaiter3 = settingsRecording.DisplayAlert(Translate.GetString("ios_DeleteData_Title"), Translate.GetString("ios_DeleteData_Text"), Translate.GetString("ios_Yes"), Translate.GetString("ios_No")).GetAwaiter();
						if (!taskAwaiter3.IsCompleted)
						{
							num = (num2 = 0);
							taskAwaiter2 = taskAwaiter3;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<bool>, SettingsRecording.<DeleteAll>d__15>(ref taskAwaiter3, ref this);
							return;
						}
						break;
					}
					if (!taskAwaiter3.GetResult())
					{
						goto IL_01C0;
					}
					settingsRecording.activityFrame.IsVisible = true;
					taskAwaiter4 = Task.Delay(100).GetAwaiter();
					if (!taskAwaiter4.IsCompleted)
					{
						num = (num2 = 1);
						TaskAwaiter taskAwaiter5 = taskAwaiter4;
						this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, SettingsRecording.<DeleteAll>d__15>(ref taskAwaiter4, ref this);
						return;
					}
					IL_0109:
					taskAwaiter4.GetResult();
					IEnumerator<FileToRecordProxy> enumerator = settingsRecording.tracksModel.RecorderedTracks.GetEnumerator();
					try
					{
						while (enumerator.MoveNext())
						{
							FileToRecordProxy fileToRecordProxy = enumerator.Current;
							File.Delete(fileToRecordProxy.Path);
						}
					}
					finally
					{
						if (num < 0 && enumerator != null)
						{
							enumerator.Dispose();
						}
					}
					settingsRecording.tracksModel.LoadList();
					settingsRecording.activityFrame.IsVisible = false;
					taskAwaiter4 = Task.Delay(100).GetAwaiter();
					if (!taskAwaiter4.IsCompleted)
					{
						num = (num2 = 2);
						TaskAwaiter taskAwaiter5 = taskAwaiter4;
						this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, SettingsRecording.<DeleteAll>d__15>(ref taskAwaiter4, ref this);
						return;
					}
					IL_01B9:
					taskAwaiter4.GetResult();
					IL_01C0:;
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

			// Token: 0x06001BFE RID: 7166 RVA: 0x0013ECE4 File Offset: 0x0013CEE4
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x04000D10 RID: 3344
			public int <>1__state;

			// Token: 0x04000D11 RID: 3345
			public AsyncVoidMethodBuilder <>t__builder;

			// Token: 0x04000D12 RID: 3346
			public SettingsRecording <>4__this;

			// Token: 0x04000D13 RID: 3347
			private TaskAwaiter<bool> <>u__1;

			// Token: 0x04000D14 RID: 3348
			private TaskAwaiter <>u__2;
		}

		// Token: 0x0200025C RID: 604
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <Handle_Toggled>d__20 : IAsyncStateMachine
		{
			// Token: 0x06001BFF RID: 7167 RVA: 0x0013ECF4 File Offset: 0x0013CEF4
			void IAsyncStateMachine.MoveNext()
			{
				SettingsRecording settingsRecording = this;
				try
				{
					if (e.Value && App.OBDReader.CurrentStatus == OBDDataReaderStatus.ConnectedToECU && App.OBDReader.CurrentCarData.Recorder == null)
					{
						DataRecorderV2.StartRecording();
					}
					if (!e.Value)
					{
						DataRecorderV2 recorder = App.OBDReader.CurrentCarData.Recorder;
						App.OBDReader.CurrentCarData.Recorder = null;
						if (recorder != null)
						{
							recorder.Stop();
						}
						settingsRecording.activityFrame.IsVisible = true;
						settingsRecording.LoadList();
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

			// Token: 0x06001C00 RID: 7168 RVA: 0x0013EDBC File Offset: 0x0013CFBC
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x04000D15 RID: 3349
			public int <>1__state;

			// Token: 0x04000D16 RID: 3350
			public AsyncVoidMethodBuilder <>t__builder;

			// Token: 0x04000D17 RID: 3351
			public ToggledEventArgs e;

			// Token: 0x04000D18 RID: 3352
			public SettingsRecording <>4__this;
		}

		// Token: 0x0200025D RID: 605
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <LoadList>d__6 : IAsyncStateMachine
		{
			// Token: 0x06001C01 RID: 7169 RVA: 0x0013EDCC File Offset: 0x0013CFCC
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				SettingsRecording settingsRecording = this;
				try
				{
					TaskAwaiter taskAwaiter;
					if (num != 0)
					{
						settingsRecording.activityFrame.IsVisible = true;
						taskAwaiter = Task.Run(delegate
						{
							settingsRecording.tracksModel.LoadList();
						}).GetAwaiter();
						if (!taskAwaiter.IsCompleted)
						{
							num2 = 0;
							TaskAwaiter taskAwaiter2 = taskAwaiter;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, SettingsRecording.<LoadList>d__6>(ref taskAwaiter, ref this);
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
					Device.BeginInvokeOnMainThread(delegate
					{
						settingsRecording.lvRecords.ItemsSource = null;
						BindableObjectExtensions.SetBinding(settingsRecording.lvRecords, ItemsView<Cell>.ItemsSourceProperty, "RecorderedTracks", 2, null, null);
						settingsRecording.activityFrame.IsVisible = false;
					});
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

			// Token: 0x06001C02 RID: 7170 RVA: 0x0013EEA8 File Offset: 0x0013D0A8
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x04000D19 RID: 3353
			public int <>1__state;

			// Token: 0x04000D1A RID: 3354
			public AsyncVoidMethodBuilder <>t__builder;

			// Token: 0x04000D1B RID: 3355
			public SettingsRecording <>4__this;

			// Token: 0x04000D1C RID: 3356
			private TaskAwaiter <>u__1;
		}

		// Token: 0x0200025E RID: 606
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <RecordingCB_Tapped>d__13 : IAsyncStateMachine
		{
			// Token: 0x06001C03 RID: 7171 RVA: 0x0013EEB8 File Offset: 0x0013D0B8
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				SettingsRecording settingsRecording = this;
				try
				{
					TaskAwaiter taskAwaiter;
					if (num != 0)
					{
						if (!SharedSettings.Current.RecordData && App.OBDReader.CurrentStatus == OBDDataReaderStatus.ConnectedToECU && App.OBDReader.CurrentCarData.Recorder == null)
						{
							DataRecorderV2.StartRecording();
							goto IL_00DF;
						}
						DataRecorderV2 recorder = App.OBDReader.CurrentCarData.Recorder;
						App.OBDReader.CurrentCarData.Recorder = null;
						if (recorder != null)
						{
							recorder.Stop();
						}
						settingsRecording.activityFrame.IsVisible = true;
						taskAwaiter = Task.Delay(500).GetAwaiter();
						if (!taskAwaiter.IsCompleted)
						{
							num2 = 0;
							TaskAwaiter taskAwaiter2 = taskAwaiter;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, SettingsRecording.<RecordingCB_Tapped>d__13>(ref taskAwaiter, ref this);
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
					settingsRecording.LoadList();
					IL_00DF:;
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

			// Token: 0x06001C04 RID: 7172 RVA: 0x0013EFE4 File Offset: 0x0013D1E4
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x04000D1D RID: 3357
			public int <>1__state;

			// Token: 0x04000D1E RID: 3358
			public AsyncVoidMethodBuilder <>t__builder;

			// Token: 0x04000D1F RID: 3359
			public SettingsRecording <>4__this;

			// Token: 0x04000D20 RID: 3360
			private TaskAwaiter <>u__1;
		}

		// Token: 0x0200025F RID: 607
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <SettingsRecording_Appearing>d__11 : IAsyncStateMachine
		{
			// Token: 0x06001C05 RID: 7173 RVA: 0x0013EFF4 File Offset: 0x0013D1F4
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				SettingsRecording settingsRecording = this;
				try
				{
					TaskAwaiter taskAwaiter;
					if (num != 0)
					{
						if (num == 1)
						{
							TaskAwaiter taskAwaiter2;
							taskAwaiter = taskAwaiter2;
							taskAwaiter2 = default(TaskAwaiter);
							num2 = -1;
							goto IL_00C8;
						}
						taskAwaiter = settingsRecording.UpdateGPSState().GetAwaiter();
						if (!taskAwaiter.IsCompleted)
						{
							num2 = 0;
							TaskAwaiter taskAwaiter2 = taskAwaiter;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, SettingsRecording.<SettingsRecording_Appearing>d__11>(ref taskAwaiter, ref this);
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
					taskAwaiter = Task.Delay(1500).GetAwaiter();
					if (!taskAwaiter.IsCompleted)
					{
						num2 = 1;
						TaskAwaiter taskAwaiter2 = taskAwaiter;
						this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, SettingsRecording.<SettingsRecording_Appearing>d__11>(ref taskAwaiter, ref this);
						return;
					}
					IL_00C8:
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

			// Token: 0x06001C06 RID: 7174 RVA: 0x0013F10C File Offset: 0x0013D30C
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x04000D21 RID: 3361
			public int <>1__state;

			// Token: 0x04000D22 RID: 3362
			public AsyncVoidMethodBuilder <>t__builder;

			// Token: 0x04000D23 RID: 3363
			public SettingsRecording <>4__this;

			// Token: 0x04000D24 RID: 3364
			private TaskAwaiter <>u__1;
		}

		// Token: 0x02000260 RID: 608
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <UpdateGPSState>d__22 : IAsyncStateMachine
		{
			// Token: 0x06001C07 RID: 7175 RVA: 0x0013F11C File Offset: 0x0013D31C
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				SettingsRecording settingsRecording = this;
				try
				{
					TaskAwaiter<PermissionStatus> taskAwaiter3;
					if (num != 0)
					{
						taskAwaiter3 = Permissions.CheckStatusAsync<Permissions.LocationWhenInUse>().GetAwaiter();
						if (!taskAwaiter3.IsCompleted)
						{
							num2 = 0;
							taskAwaiter2 = taskAwaiter3;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<PermissionStatus>, SettingsRecording.<UpdateGPSState>d__22>(ref taskAwaiter3, ref this);
							return;
						}
					}
					else
					{
						taskAwaiter3 = taskAwaiter2;
						taskAwaiter2 = default(TaskAwaiter<PermissionStatus>);
						num2 = -1;
					}
					if (taskAwaiter3.GetResult() == 3)
					{
						settingsRecording.HasGPSAccess = true;
					}
					else
					{
						settingsRecording.HasGPSAccess = false;
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

			// Token: 0x06001C08 RID: 7176 RVA: 0x0013F1E0 File Offset: 0x0013D3E0
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x04000D25 RID: 3365
			public int <>1__state;

			// Token: 0x04000D26 RID: 3366
			public AsyncTaskMethodBuilder <>t__builder;

			// Token: 0x04000D27 RID: 3367
			public SettingsRecording <>4__this;

			// Token: 0x04000D28 RID: 3368
			private TaskAwaiter<PermissionStatus> <>u__1;
		}

		// Token: 0x02000261 RID: 609
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <btnDeleteSelected_Clicked>d__33 : IAsyncStateMachine
		{
			// Token: 0x06001C09 RID: 7177 RVA: 0x0013F1F0 File Offset: 0x0013D3F0
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				SettingsRecording settingsRecording = this;
				try
				{
					TaskAwaiter<bool> taskAwaiter3;
					if (num != 0)
					{
						if (!settingsRecording.Model.IsSelectionMode)
						{
							goto IL_019E;
						}
						List<FileToRecordProxy> list = settingsRecording.Model.RecorderedTracks.Where((FileToRecordProxy x) => x.IsSelected).ToList<FileToRecordProxy>();
						if (list == null || list.Count == 0)
						{
							goto IL_019E;
						}
						taskAwaiter3 = settingsRecording.DisplayAlert(Translate.GetString("records_DeleteSelected"), "", Translate.GetString("ios_Yes"), Translate.GetString("ios_No")).GetAwaiter();
						if (!taskAwaiter3.IsCompleted)
						{
							num = (num2 = 0);
							taskAwaiter2 = taskAwaiter3;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<bool>, SettingsRecording.<btnDeleteSelected_Clicked>d__33>(ref taskAwaiter3, ref this);
							return;
						}
					}
					else
					{
						taskAwaiter3 = taskAwaiter2;
						taskAwaiter2 = default(TaskAwaiter<bool>);
						num = (num2 = -1);
					}
					if (taskAwaiter3.GetResult())
					{
						List<FileToRecordProxy>.Enumerator enumerator = settingsRecording.Model.RecorderedTracks.Where((FileToRecordProxy x) => x.IsSelected).ToList<FileToRecordProxy>().GetEnumerator();
						try
						{
							while (enumerator.MoveNext())
							{
								FileToRecordProxy fileToRecordProxy = enumerator.Current;
								try
								{
									if (File.Exists(fileToRecordProxy.Path))
									{
										File.Delete(fileToRecordProxy.Path);
									}
									settingsRecording.Model.RecorderedTracks.Remove(fileToRecordProxy);
								}
								catch (Exception)
								{
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
					}
				}
				catch (Exception ex)
				{
					num2 = -2;
					this.<>t__builder.SetException(ex);
					return;
				}
				IL_019E:
				num2 = -2;
				this.<>t__builder.SetResult();
			}

			// Token: 0x06001C0A RID: 7178 RVA: 0x0013F3FC File Offset: 0x0013D5FC
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x04000D29 RID: 3369
			public int <>1__state;

			// Token: 0x04000D2A RID: 3370
			public AsyncVoidMethodBuilder <>t__builder;

			// Token: 0x04000D2B RID: 3371
			public SettingsRecording <>4__this;

			// Token: 0x04000D2C RID: 3372
			private TaskAwaiter<bool> <>u__1;
		}

		// Token: 0x02000262 RID: 610
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <btnDelete_Clicked>d__16 : IAsyncStateMachine
		{
			// Token: 0x06001C0B RID: 7179 RVA: 0x0013F40C File Offset: 0x0013D60C
			void IAsyncStateMachine.MoveNext()
			{
				SettingsRecording settingsRecording = this;
				try
				{
					try
					{
						FileToRecordProxy fileToRecordProxy = (sender as MenuItem).BindingContext as FileToRecordProxy;
						File.Delete(fileToRecordProxy.Path);
						settingsRecording.tracksModel.RecorderedTracks.Remove(fileToRecordProxy);
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

			// Token: 0x06001C0C RID: 7180 RVA: 0x0013F4A0 File Offset: 0x0013D6A0
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x04000D2D RID: 3373
			public int <>1__state;

			// Token: 0x04000D2E RID: 3374
			public AsyncVoidMethodBuilder <>t__builder;

			// Token: 0x04000D2F RID: 3375
			public object sender;

			// Token: 0x04000D30 RID: 3376
			public SettingsRecording <>4__this;
		}

		// Token: 0x02000263 RID: 611
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <btnImportFromFile_Clicked>d__21 : IAsyncStateMachine
		{
			// Token: 0x06001C0D RID: 7181 RVA: 0x0013F4B0 File Offset: 0x0013D6B0
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				SettingsRecording settingsRecording = this;
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
								num2 = 0;
								TaskAwaiter<FileResult> taskAwaiter2 = taskAwaiter;
								this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<FileResult>, SettingsRecording.<btnImportFromFile_Clicked>d__21>(ref taskAwaiter, ref this);
								return;
							}
						}
						else
						{
							TaskAwaiter<FileResult> taskAwaiter2;
							taskAwaiter = taskAwaiter2;
							taskAwaiter2 = default(TaskAwaiter<FileResult>);
							num2 = -1;
						}
						FileResult result = taskAwaiter.GetResult();
						if (result != null && ".brc".Equals(Path.GetExtension(result.FullPath), StringComparison.OrdinalIgnoreCase))
						{
							File.Copy(result.FullPath, FileSystemHelper.GetLocalFilePath(result.FileName));
							settingsRecording.LoadList();
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

			// Token: 0x06001C0E RID: 7182 RVA: 0x0013F5B4 File Offset: 0x0013D7B4
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x04000D31 RID: 3377
			public int <>1__state;

			// Token: 0x04000D32 RID: 3378
			public AsyncVoidMethodBuilder <>t__builder;

			// Token: 0x04000D33 RID: 3379
			public SettingsRecording <>4__this;

			// Token: 0x04000D34 RID: 3380
			private TaskAwaiter<FileResult> <>u__1;
		}

		// Token: 0x02000264 RID: 612
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <btnRename_Clicked>d__17 : IAsyncStateMachine
		{
			// Token: 0x06001C0F RID: 7183 RVA: 0x0013F5C4 File Offset: 0x0013D7C4
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				SettingsRecording settingsRecording = this;
				try
				{
					TaskAwaiter taskAwaiter;
					if (num != 0)
					{
						SettingsRecordRename settingsRecordRename = new SettingsRecordRename(new FileSystemElement(((sender as MenuItem).BindingContext as FileToRecordProxy).Path, FileSystemElementType.File, false), delegate
						{
							base.LoadList();
						});
						taskAwaiter = settingsRecording.Navigation.PushAsync(settingsRecordRename).GetAwaiter();
						if (!taskAwaiter.IsCompleted)
						{
							num2 = 0;
							TaskAwaiter taskAwaiter2 = taskAwaiter;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, SettingsRecording.<btnRename_Clicked>d__17>(ref taskAwaiter, ref this);
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

			// Token: 0x06001C10 RID: 7184 RVA: 0x0013F6B4 File Offset: 0x0013D8B4
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x04000D35 RID: 3381
			public int <>1__state;

			// Token: 0x04000D36 RID: 3382
			public AsyncVoidMethodBuilder <>t__builder;

			// Token: 0x04000D37 RID: 3383
			public object sender;

			// Token: 0x04000D38 RID: 3384
			public SettingsRecording <>4__this;

			// Token: 0x04000D39 RID: 3385
			private TaskAwaiter <>u__1;
		}

		// Token: 0x02000265 RID: 613
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <btnShareSelected_Clicked>d__32 : IAsyncStateMachine
		{
			// Token: 0x06001C11 RID: 7185 RVA: 0x0013F6C4 File Offset: 0x0013D8C4
			void IAsyncStateMachine.MoveNext()
			{
				int num3;
				int num2 = num3;
				SettingsRecording settingsRecording = this;
				try
				{
					TaskAwaiter<string> taskAwaiter;
					TaskAwaiter<string> taskAwaiter2;
					if (num2 != 0)
					{
						if (num2 - 1 <= 10)
						{
							goto IL_019A;
						}
						if (!settingsRecording.Model.IsSelectionMode)
						{
							goto IL_0B9E;
						}
						selected = settingsRecording.Model.RecorderedTracks.Where((FileToRecordProxy x) => x.IsSelected).ToList<FileToRecordProxy>();
						if (selected == null || selected.Count == 0)
						{
							goto IL_0B9E;
						}
						string[] array = new string[] { "CSV #1", "CSV #2", "BRC" };
						taskAwaiter = settingsRecording.DisplayActionSheetCustom(Translate.GetString("ios_SelectExportFormat"), Translate.GetString("btnCancel.Content"), null, array).GetAwaiter();
						if (!taskAwaiter.IsCompleted)
						{
							num2 = (num3 = 0);
							taskAwaiter2 = taskAwaiter;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<string>, SettingsRecording.<btnShareSelected_Clicked>d__32>(ref taskAwaiter, ref this);
							return;
						}
					}
					else
					{
						taskAwaiter = taskAwaiter2;
						taskAwaiter2 = default(TaskAwaiter<string>);
						num2 = (num3 = -1);
					}
					string text = taskAwaiter.GetResult();
					format = text;
					if (format == Translate.GetString("btnCancel.Content"))
					{
						goto IL_0B9E;
					}
					output_path = FileSystemHelper.GetCacheFilePath("exported_records.zip");
					if (File.Exists(output_path))
					{
						try
						{
							File.Delete(output_path);
						}
						catch (Exception)
						{
							goto IL_0B9E;
						}
					}
					progress = new Progress<int>(delegate(int percent)
					{
						Device.BeginInvokeOnMainThread(new Action(new SettingsRecording.<>c__DisplayClass32_0
						{
							<>4__this = settingsRecording,
							percent = percent
						}.<btnShareSelected_Clicked>b__2));
					});
					settingsRecording.activityFrame.IsVisible = true;
					settingsRecording.lvRecords.IsEnabled = false;
					IL_019A:
					try
					{
						TaskAwaiter taskAwaiter4;
						TaskAwaiter taskAwaiter3;
						if (num2 - 1 > 9)
						{
							if (num2 == 11)
							{
								taskAwaiter3 = taskAwaiter4;
								taskAwaiter4 = default(TaskAwaiter);
								num2 = (num3 = -1);
								goto IL_0B3A;
							}
							num = 0;
						}
						int num5;
						try
						{
							switch (num2)
							{
							case 1:
							case 2:
							case 3:
							case 4:
							case 5:
							case 6:
							case 7:
								break;
							case 8:
								taskAwaiter = taskAwaiter2;
								taskAwaiter2 = default(TaskAwaiter<string>);
								num2 = (num3 = -1);
								goto IL_0900;
							case 9:
								taskAwaiter3 = taskAwaiter4;
								taskAwaiter4 = default(TaskAwaiter);
								num2 = (num3 = -1);
								goto IL_0989;
							case 10:
								IL_09E0:
								try
								{
									if (num2 != 10)
									{
										EmailMessage emailMessage;
										taskAwaiter3 = Email.ComposeAsync(emailMessage).GetAwaiter();
										if (!taskAwaiter3.IsCompleted)
										{
											num2 = (num3 = 10);
											taskAwaiter4 = taskAwaiter3;
											this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, SettingsRecording.<btnShareSelected_Clicked>d__32>(ref taskAwaiter3, ref this);
											return;
										}
									}
									else
									{
										taskAwaiter3 = taskAwaiter4;
										taskAwaiter4 = default(TaskAwaiter);
										num2 = (num3 = -1);
									}
									taskAwaiter3.GetResult();
								}
								catch (Exception)
								{
								}
								goto IL_0A4B;
							default:
								fstream = File.Open(output_path, FileMode.Create);
								break;
							}
							try
							{
								if (num2 - 1 > 6)
								{
									zstream = new ZipOutputStream(fstream);
								}
								try
								{
									switch (num2)
									{
									case 1:
										taskAwaiter3 = taskAwaiter4;
										taskAwaiter4 = default(TaskAwaiter);
										num2 = (num3 = -1);
										break;
									case 2:
										taskAwaiter3 = taskAwaiter4;
										taskAwaiter4 = default(TaskAwaiter);
										num2 = (num3 = -1);
										goto IL_042C;
									case 3:
										taskAwaiter3 = taskAwaiter4;
										taskAwaiter4 = default(TaskAwaiter);
										num2 = (num3 = -1);
										goto IL_04B3;
									case 4:
										taskAwaiter3 = taskAwaiter4;
										taskAwaiter4 = default(TaskAwaiter);
										num2 = (num3 = -1);
										goto IL_054A;
									case 5:
										taskAwaiter3 = taskAwaiter4;
										taskAwaiter4 = default(TaskAwaiter);
										num2 = (num3 = -1);
										goto IL_0605;
									case 6:
									{
										IL_0642:
										try
										{
											if (num2 != 6)
											{
												ZipEntry zipEntry = new ZipEntry(CS$<>8__locals1.input_title);
												zstream.PutNextEntry(zipEntry);
												taskAwaiter3 = input_stream.CopyToAsync(zstream).GetAwaiter();
												if (!taskAwaiter3.IsCompleted)
												{
													num2 = (num3 = 6);
													taskAwaiter4 = taskAwaiter3;
													this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, SettingsRecording.<btnShareSelected_Clicked>d__32>(ref taskAwaiter3, ref this);
													return;
												}
											}
											else
											{
												taskAwaiter3 = taskAwaiter4;
												taskAwaiter4 = default(TaskAwaiter);
												num2 = (num3 = -1);
											}
											taskAwaiter3.GetResult();
											zstream.CloseEntry();
										}
										finally
										{
											if (num2 < 0 && input_stream != null)
											{
												((IDisposable)input_stream).Dispose();
											}
										}
										input_stream = null;
										if (Path.GetExtension(CS$<>8__locals1.input_path) == ".csv")
										{
											File.Delete(CS$<>8__locals1.input_path);
										}
										int num4 = i * 100 / selected.Count;
										IProgress<int> progress2 = progress;
										if (progress2 != null)
										{
											progress2.Report(num4);
										}
										CS$<>8__locals1 = null;
										goto IL_0758;
									}
									case 7:
										taskAwaiter3 = taskAwaiter4;
										taskAwaiter4 = default(TaskAwaiter);
										num2 = (num3 = -1);
										goto IL_07DC;
									default:
										zstream.UseZip64 = 0;
										zstream.SetLevel(5);
										i = 0;
										goto IL_076A;
									}
									IL_03AB:
									taskAwaiter3.GetResult();
									goto IL_04BF;
									IL_042C:
									taskAwaiter3.GetResult();
									goto IL_04BF;
									IL_04B3:
									taskAwaiter3.GetResult();
									goto IL_0758;
									IL_04BF:
									if (CS$<>8__locals1.recorder == null)
									{
										taskAwaiter3 = settingsRecording.DisplayAlert("Error loading data record " + CS$<>8__locals1.fileproxy.Name, "Skipped", "OK").GetAwaiter();
										if (!taskAwaiter3.IsCompleted)
										{
											num2 = (num3 = 4);
											taskAwaiter4 = taskAwaiter3;
											this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, SettingsRecording.<btnShareSelected_Clicked>d__32>(ref taskAwaiter3, ref this);
											return;
										}
									}
									else
									{
										CS$<>8__locals1.format_version = 2;
										if (format == "CSV #1")
										{
											CS$<>8__locals1.format_version = 1;
										}
										if (format == "CSV #2")
										{
											CS$<>8__locals1.format_version = 2;
										}
										taskAwaiter3 = Task.Run(delegate
										{
											CS$<>8__locals1.input_path = Brc2CsvConverter.Convert(CS$<>8__locals1.recorder, CS$<>8__locals1.fileproxy.Name, null, CS$<>8__locals1.format_version);
											CS$<>8__locals1.input_title = Path.GetFileName(CS$<>8__locals1.input_path);
										}).GetAwaiter();
										if (!taskAwaiter3.IsCompleted)
										{
											num2 = (num3 = 5);
											taskAwaiter4 = taskAwaiter3;
											this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, SettingsRecording.<btnShareSelected_Clicked>d__32>(ref taskAwaiter3, ref this);
											return;
										}
										goto IL_0605;
									}
									IL_054A:
									taskAwaiter3.GetResult();
									goto IL_0758;
									IL_0605:
									taskAwaiter3.GetResult();
									IL_060C:
									if (CS$<>8__locals1.input_path != null && CS$<>8__locals1.input_title != null)
									{
										input_stream = File.OpenRead(CS$<>8__locals1.input_path);
										goto IL_0642;
									}
									IL_0758:
									num5 = i;
									i = num5 + 1;
									IL_076A:
									if (i >= selected.Count)
									{
										taskAwaiter3 = zstream.FlushAsync().GetAwaiter();
										if (!taskAwaiter3.IsCompleted)
										{
											num2 = (num3 = 7);
											taskAwaiter4 = taskAwaiter3;
											this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, SettingsRecording.<btnShareSelected_Clicked>d__32>(ref taskAwaiter3, ref this);
											return;
										}
									}
									else
									{
										CS$<>8__locals1 = new SettingsRecording.<>c__DisplayClass32_1();
										CS$<>8__locals1.input_path = null;
										CS$<>8__locals1.input_title = null;
										if (format == "BRC")
										{
											CS$<>8__locals1.input_path = selected[i].Path;
											CS$<>8__locals1.input_title = Path.GetFileName(CS$<>8__locals1.input_path);
											goto IL_060C;
										}
										if (!(format == "CSV #1") && !(format == "CSV #2"))
										{
											goto IL_060C;
										}
										CS$<>8__locals1.fileproxy = selected[i];
										CS$<>8__locals1.recorder = null;
										string text2 = Path.GetExtension(CS$<>8__locals1.fileproxy.Path).ToLowerInvariant();
										if (text2 == ".rec")
										{
											taskAwaiter3 = Task.Run(delegate
											{
												CS$<>8__locals1.recorder = RecorderedTracksViewModel.LoadRecorderFromFile(CS$<>8__locals1.fileproxy.Path);
											}).GetAwaiter();
											if (!taskAwaiter3.IsCompleted)
											{
												num2 = (num3 = 1);
												taskAwaiter4 = taskAwaiter3;
												this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, SettingsRecording.<btnShareSelected_Clicked>d__32>(ref taskAwaiter3, ref this);
												return;
											}
											goto IL_03AB;
										}
										else if (text2 == ".brc")
										{
											taskAwaiter3 = Task.Run(delegate
											{
												if (BRCHelper.GetVersion(CS$<>8__locals1.fileproxy.Path) == BRCHelper.BRCType.V1)
												{
													CS$<>8__locals1.recorder = DataRecorder.LoadFromFile(CS$<>8__locals1.fileproxy.Path);
													return;
												}
												CS$<>8__locals1.recorder = DataRecordContainer.LoadFromFile(CS$<>8__locals1.fileproxy.Path, false);
											}).GetAwaiter();
											if (!taskAwaiter3.IsCompleted)
											{
												num2 = (num3 = 2);
												taskAwaiter4 = taskAwaiter3;
												this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, SettingsRecording.<btnShareSelected_Clicked>d__32>(ref taskAwaiter3, ref this);
												return;
											}
											goto IL_042C;
										}
										else
										{
											taskAwaiter3 = settingsRecording.DisplayAlert("Error loading data record " + CS$<>8__locals1.fileproxy.Name, "Skipped", "OK").GetAwaiter();
											if (!taskAwaiter3.IsCompleted)
											{
												num2 = (num3 = 3);
												taskAwaiter4 = taskAwaiter3;
												this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, SettingsRecording.<btnShareSelected_Clicked>d__32>(ref taskAwaiter3, ref this);
												return;
											}
											goto IL_04B3;
										}
									}
									IL_07DC:
									taskAwaiter3.GetResult();
									zstream.Finish();
								}
								finally
								{
									if (num2 < 0 && zstream != null)
									{
										zstream.Dispose();
									}
								}
								zstream = null;
							}
							finally
							{
								if (num2 < 0 && fstream != null)
								{
									((IDisposable)fstream).Dispose();
								}
							}
							fstream = null;
							IProgress<int> progress3 = progress;
							if (progress3 != null)
							{
								progress3.Report(100);
							}
							settingsRecording.activityFrame.IsVisible = false;
							settingsRecording.lvRecords.IsEnabled = true;
							if (PlatformHelper.IsAndroid)
							{
								exportVariantSend = Translate.GetString("droid_ExportSend");
								exportVariantFile = Translate.GetString("droid_ExportFile");
								taskAwaiter = settingsRecording.DisplayActionSheetCustom(Translate.GetString("droid_ExportMode"), null, null, new string[] { exportVariantFile, exportVariantSend }).GetAwaiter();
								if (!taskAwaiter.IsCompleted)
								{
									num2 = (num3 = 8);
									taskAwaiter2 = taskAwaiter;
									this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<string>, SettingsRecording.<btnShareSelected_Clicked>d__32>(ref taskAwaiter, ref this);
									return;
								}
							}
							else
							{
								if (Device.Idiom == 1)
								{
									Share.RequestAsync(new ShareFileRequest(new ShareFile(output_path)));
									goto IL_0AA2;
								}
								PlatformHelper.IOSService.SendDebugEmail(output_path, "", "", "");
								goto IL_0AA2;
							}
							IL_0900:
							text = taskAwaiter.GetResult();
							exportVariantResult = text;
							if (!(exportVariantResult == exportVariantFile))
							{
								goto IL_0990;
							}
							taskAwaiter3 = Share.RequestAsync(new ShareFileRequest(new ShareFile(output_path))).GetAwaiter();
							if (!taskAwaiter3.IsCompleted)
							{
								num2 = (num3 = 9);
								taskAwaiter4 = taskAwaiter3;
								this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, SettingsRecording.<btnShareSelected_Clicked>d__32>(ref taskAwaiter3, ref this);
								return;
							}
							IL_0989:
							taskAwaiter3.GetResult();
							IL_0990:
							if (exportVariantResult == exportVariantSend)
							{
								EmailMessage emailMessage = new EmailMessage("Car Scanner records", "", Array.Empty<string>());
								if (File.Exists(output_path))
								{
									emailMessage.Attachments.Add(new EmailAttachment(output_path));
									goto IL_09E0;
								}
								goto IL_09E0;
							}
							IL_0A4B:
							exportVariantSend = null;
							exportVariantFile = null;
							exportVariantResult = null;
							IL_0AA2:;
						}
						catch (Exception ex)
						{
							obj = ex;
							num = 1;
						}
						num5 = num;
						if (num5 != 1)
						{
							goto IL_0B41;
						}
						Exception ex2 = (Exception)obj;
						taskAwaiter3 = settingsRecording.DisplayAlert("Error", ex2.Message, "OK").GetAwaiter();
						if (!taskAwaiter3.IsCompleted)
						{
							num2 = (num3 = 11);
							taskAwaiter4 = taskAwaiter3;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, SettingsRecording.<btnShareSelected_Clicked>d__32>(ref taskAwaiter3, ref this);
							return;
						}
						IL_0B3A:
						taskAwaiter3.GetResult();
						IL_0B41:
						obj = null;
					}
					finally
					{
						if (num2 < 0)
						{
							settingsRecording.activityFrame.IsVisible = false;
							settingsRecording.lvRecords.IsEnabled = true;
						}
					}
				}
				catch (Exception ex3)
				{
					num3 = -2;
					selected = null;
					format = null;
					output_path = null;
					progress = null;
					this.<>t__builder.SetException(ex3);
					return;
				}
				IL_0B9E:
				num3 = -2;
				selected = null;
				format = null;
				output_path = null;
				progress = null;
				this.<>t__builder.SetResult();
			}

			// Token: 0x06001C12 RID: 7186 RVA: 0x00140364 File Offset: 0x0013E564
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x04000D3A RID: 3386
			public int <>1__state;

			// Token: 0x04000D3B RID: 3387
			public AsyncVoidMethodBuilder <>t__builder;

			// Token: 0x04000D3C RID: 3388
			public SettingsRecording <>4__this;

			// Token: 0x04000D3D RID: 3389
			private SettingsRecording.<>c__DisplayClass32_1 <>8__1;

			// Token: 0x04000D3E RID: 3390
			private List<FileToRecordProxy> <selected>5__2;

			// Token: 0x04000D3F RID: 3391
			private string <format>5__3;

			// Token: 0x04000D40 RID: 3392
			private string <output_path>5__4;

			// Token: 0x04000D41 RID: 3393
			private IProgress<int> <progress>5__5;

			// Token: 0x04000D42 RID: 3394
			private TaskAwaiter<string> <>u__1;

			// Token: 0x04000D43 RID: 3395
			private object <>7__wrap5;

			// Token: 0x04000D44 RID: 3396
			private int <>7__wrap6;

			// Token: 0x04000D45 RID: 3397
			private FileStream <fstream>5__8;

			// Token: 0x04000D46 RID: 3398
			private ZipOutputStream <zstream>5__9;

			// Token: 0x04000D47 RID: 3399
			private int <i>5__10;

			// Token: 0x04000D48 RID: 3400
			private TaskAwaiter <>u__2;

			// Token: 0x04000D49 RID: 3401
			private FileStream <input_stream>5__11;

			// Token: 0x04000D4A RID: 3402
			private string <exportVariantSend>5__12;

			// Token: 0x04000D4B RID: 3403
			private string <exportVariantFile>5__13;

			// Token: 0x04000D4C RID: 3404
			private string <exportVariantResult>5__14;
		}

		// Token: 0x02000266 RID: 614
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <btnShare_Clicked>d__18 : IAsyncStateMachine
		{
			// Token: 0x06001C13 RID: 7187 RVA: 0x00140374 File Offset: 0x0013E574
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				SettingsRecording settingsRecording = this;
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
						TaskAwaiter<string> taskAwaiter4;
						taskAwaiter3 = taskAwaiter4;
						taskAwaiter4 = default(TaskAwaiter<string>);
						num = (num2 = -1);
						goto IL_01F8;
					}
					case 2:
					{
						TaskAwaiter<string> taskAwaiter4;
						taskAwaiter3 = taskAwaiter4;
						taskAwaiter4 = default(TaskAwaiter<string>);
						num = (num2 = -1);
						goto IL_02D9;
					}
					case 3:
					{
						TaskAwaiter taskAwaiter2;
						taskAwaiter = taskAwaiter2;
						taskAwaiter2 = default(TaskAwaiter);
						num = (num2 = -1);
						goto IL_0360;
					}
					case 4:
						IL_03C2:
						try
						{
							if (num != 4)
							{
								EmailMessage emailMessage;
								taskAwaiter = Email.ComposeAsync(emailMessage).GetAwaiter();
								if (!taskAwaiter.IsCompleted)
								{
									num = (num2 = 4);
									TaskAwaiter taskAwaiter2 = taskAwaiter;
									this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, SettingsRecording.<btnShare_Clicked>d__18>(ref taskAwaiter, ref this);
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
						}
						catch (Exception)
						{
						}
						goto IL_0428;
					case 5:
					{
						TaskAwaiter taskAwaiter2;
						taskAwaiter = taskAwaiter2;
						taskAwaiter2 = default(TaskAwaiter);
						num = (num2 = -1);
						goto IL_0532;
					}
					case 6:
					{
						TaskAwaiter taskAwaiter2;
						taskAwaiter = taskAwaiter2;
						taskAwaiter2 = default(TaskAwaiter);
						num = (num2 = -1);
						goto IL_05D8;
					}
					case 7:
					{
						TaskAwaiter taskAwaiter2;
						taskAwaiter = taskAwaiter2;
						taskAwaiter2 = default(TaskAwaiter);
						num = (num2 = -1);
						goto IL_0656;
					}
					case 8:
					{
						TaskAwaiter taskAwaiter2;
						taskAwaiter = taskAwaiter2;
						taskAwaiter2 = default(TaskAwaiter);
						num = (num2 = -1);
						goto IL_06C5;
					}
					case 9:
					{
						TaskAwaiter taskAwaiter2;
						taskAwaiter = taskAwaiter2;
						taskAwaiter2 = default(TaskAwaiter);
						num = (num2 = -1);
						goto IL_0769;
					}
					case 10:
					{
						TaskAwaiter taskAwaiter2;
						taskAwaiter = taskAwaiter2;
						taskAwaiter2 = default(TaskAwaiter);
						num = (num2 = -1);
						goto IL_087C;
					}
					case 11:
					{
						TaskAwaiter<string> taskAwaiter4;
						taskAwaiter3 = taskAwaiter4;
						taskAwaiter4 = default(TaskAwaiter<string>);
						num = (num2 = -1);
						goto IL_0929;
					}
					case 12:
						IL_094D:
						try
						{
							if (num != 12)
							{
								taskAwaiter = Share.RequestAsync(new ShareFileRequest(new ShareFile(CS$<>8__locals2.path))).GetAwaiter();
								if (!taskAwaiter.IsCompleted)
								{
									num = (num2 = 12);
									TaskAwaiter taskAwaiter2 = taskAwaiter;
									this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, SettingsRecording.<btnShare_Clicked>d__18>(ref taskAwaiter, ref this);
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
						}
						catch (Exception)
						{
						}
						goto IL_09C8;
					case 13:
						IL_0A32:
						try
						{
							if (num != 13)
							{
								EmailMessage emailMessage2;
								taskAwaiter = Email.ComposeAsync(emailMessage2).GetAwaiter();
								if (!taskAwaiter.IsCompleted)
								{
									num = (num2 = 13);
									TaskAwaiter taskAwaiter2 = taskAwaiter;
									this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, SettingsRecording.<btnShare_Clicked>d__18>(ref taskAwaiter, ref this);
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
						}
						catch (Exception)
						{
						}
						goto IL_0A9A;
					case 14:
					{
						TaskAwaiter taskAwaiter2;
						taskAwaiter = taskAwaiter2;
						taskAwaiter2 = default(TaskAwaiter);
						num = (num2 = -1);
						goto IL_0B25;
					}
					case 15:
					{
						TaskAwaiter taskAwaiter2;
						taskAwaiter = taskAwaiter2;
						taskAwaiter2 = default(TaskAwaiter);
						num = (num2 = -1);
						goto IL_0C30;
					}
					case 16:
					{
						TaskAwaiter taskAwaiter2;
						taskAwaiter = taskAwaiter2;
						taskAwaiter2 = default(TaskAwaiter);
						num = (num2 = -1);
						goto IL_0CAC;
					}
					default:
						CS$<>8__locals1 = new SettingsRecording.<>c__DisplayClass18_0();
						CS$<>8__locals1.<>4__this = this;
						CS$<>8__locals1.fileproxy = (sender as MenuItem).BindingContext as FileToRecordProxy;
						if (App.OBDReader != null && App.OBDReader.CurrentCarData != null && App.OBDReader.CurrentCarData.Recorder != null && CS$<>8__locals1.fileproxy.Name == Path.GetFileNameWithoutExtension(App.OBDReader.CurrentCarData.Recorder.FileName))
						{
							taskAwaiter = settingsRecording.DisplayAlert("File is busy!", "Please disconnect first or restart Car Scanner!", "OK").GetAwaiter();
							if (!taskAwaiter.IsCompleted)
							{
								num = (num2 = 0);
								TaskAwaiter taskAwaiter2 = taskAwaiter;
								this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, SettingsRecording.<btnShare_Clicked>d__18>(ref taskAwaiter, ref this);
								return;
							}
						}
						else
						{
							string[] array = new string[] { "CSV #1", "CSV #2", "BRC" };
							taskAwaiter3 = settingsRecording.DisplayActionSheetCustom(Translate.GetString("ios_SelectExportFormat"), Translate.GetString("btnCancel.Content"), null, array).GetAwaiter();
							if (!taskAwaiter3.IsCompleted)
							{
								num = (num2 = 1);
								TaskAwaiter<string> taskAwaiter4 = taskAwaiter3;
								this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<string>, SettingsRecording.<btnShare_Clicked>d__18>(ref taskAwaiter3, ref this);
								return;
							}
							goto IL_01F8;
						}
						break;
					}
					taskAwaiter.GetResult();
					settingsRecording.lvRecords.SelectedItem = null;
					goto IL_0D1A;
					IL_01F8:
					string text = taskAwaiter3.GetResult();
					format = text;
					if (format == "BRC")
					{
						path = CS$<>8__locals1.fileproxy.Path;
						if (PlatformHelper.IsAndroid)
						{
							exportVariantSend = Translate.GetString("droid_ExportSend");
							exportVariantFile = Translate.GetString("droid_ExportFile");
							taskAwaiter3 = settingsRecording.DisplayActionSheetCustom(Translate.GetString("droid_ExportMode"), null, null, new string[] { exportVariantFile, exportVariantSend }).GetAwaiter();
							if (!taskAwaiter3.IsCompleted)
							{
								num = (num2 = 2);
								TaskAwaiter<string> taskAwaiter4 = taskAwaiter3;
								this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<string>, SettingsRecording.<btnShare_Clicked>d__18>(ref taskAwaiter3, ref this);
								return;
							}
						}
						else
						{
							if (Device.Idiom == 1)
							{
								Share.RequestAsync(new ShareFileRequest(new ShareFile(path)));
								goto IL_047F;
							}
							PlatformHelper.IOSService.SendDebugEmail(path, "", "", "");
							goto IL_047F;
						}
					}
					else if (format == "CSV #1" || format == "CSV #2")
					{
						CS$<>8__locals2 = new SettingsRecording.<>c__DisplayClass18_1();
						CS$<>8__locals2.CS$<>8__locals1 = CS$<>8__locals1;
						settingsRecording.activityFrame.IsVisible = true;
						taskAwaiter = Task.Delay(150).GetAwaiter();
						if (!taskAwaiter.IsCompleted)
						{
							num = (num2 = 5);
							TaskAwaiter taskAwaiter2 = taskAwaiter;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, SettingsRecording.<btnShare_Clicked>d__18>(ref taskAwaiter, ref this);
							return;
						}
						goto IL_0532;
					}
					else
					{
						if (!(format == "REMOVE NAN"))
						{
							goto IL_0CE5;
						}
						CS$<>8__locals3 = new SettingsRecording.<>c__DisplayClass18_3();
						CS$<>8__locals3.CS$<>8__locals3 = CS$<>8__locals1;
						CS$<>8__locals3.recorder = null;
						string text2 = Path.GetExtension(CS$<>8__locals3.CS$<>8__locals3.fileproxy.Path).ToLowerInvariant();
						if (text2 == ".rec")
						{
							taskAwaiter = Task.Run(delegate
							{
								CS$<>8__locals3.recorder = RecorderedTracksViewModel.LoadRecorderFromFile(CS$<>8__locals3.CS$<>8__locals3.fileproxy.Path);
							}).GetAwaiter();
							if (!taskAwaiter.IsCompleted)
							{
								num = (num2 = 15);
								TaskAwaiter taskAwaiter2 = taskAwaiter;
								this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, SettingsRecording.<btnShare_Clicked>d__18>(ref taskAwaiter, ref this);
								return;
							}
							goto IL_0C30;
						}
						else
						{
							if (!(text2 == ".brc"))
							{
								goto IL_0CB3;
							}
							taskAwaiter = Task.Run(delegate
							{
								if (BRCHelper.GetVersion(CS$<>8__locals3.CS$<>8__locals3.fileproxy.Path) == BRCHelper.BRCType.V1)
								{
									CS$<>8__locals3.recorder = DataRecorder.LoadFromFile(CS$<>8__locals3.CS$<>8__locals3.fileproxy.Path);
									return;
								}
								CS$<>8__locals3.recorder = DataRecordContainer.LoadFromFile(CS$<>8__locals3.CS$<>8__locals3.fileproxy.Path, false);
							}).GetAwaiter();
							if (!taskAwaiter.IsCompleted)
							{
								num = (num2 = 16);
								TaskAwaiter taskAwaiter2 = taskAwaiter;
								this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, SettingsRecording.<btnShare_Clicked>d__18>(ref taskAwaiter, ref this);
								return;
							}
							goto IL_0CAC;
						}
					}
					IL_02D9:
					text = taskAwaiter3.GetResult();
					exportVariantResult = text;
					if (!(exportVariantResult == exportVariantFile))
					{
						goto IL_0367;
					}
					taskAwaiter = Share.RequestAsync(new ShareFileRequest(new ShareFile(path))).GetAwaiter();
					if (!taskAwaiter.IsCompleted)
					{
						num = (num2 = 3);
						TaskAwaiter taskAwaiter2 = taskAwaiter;
						this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, SettingsRecording.<btnShare_Clicked>d__18>(ref taskAwaiter, ref this);
						return;
					}
					IL_0360:
					taskAwaiter.GetResult();
					IL_0367:
					if (exportVariantResult == exportVariantSend)
					{
						EmailMessage emailMessage = new EmailMessage(CS$<>8__locals1.fileproxy.Name, "", Array.Empty<string>());
						if (File.Exists(path))
						{
							emailMessage.Attachments.Add(new EmailAttachment(path));
							goto IL_03C2;
						}
						goto IL_03C2;
					}
					IL_0428:
					exportVariantSend = null;
					exportVariantFile = null;
					exportVariantResult = null;
					IL_047F:
					path = null;
					goto IL_0CE5;
					IL_0532:
					taskAwaiter.GetResult();
					CS$<>8__locals2.recorder = null;
					string text3 = Path.GetExtension(CS$<>8__locals2.CS$<>8__locals1.fileproxy.Path).ToLowerInvariant();
					if (text3 == ".rec")
					{
						taskAwaiter = Task.Run(delegate
						{
							CS$<>8__locals2.recorder = RecorderedTracksViewModel.LoadRecorderFromFile(CS$<>8__locals2.CS$<>8__locals1.fileproxy.Path);
						}).GetAwaiter();
						if (!taskAwaiter.IsCompleted)
						{
							num = (num2 = 6);
							TaskAwaiter taskAwaiter2 = taskAwaiter;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, SettingsRecording.<btnShare_Clicked>d__18>(ref taskAwaiter, ref this);
							return;
						}
					}
					else if (text3 == ".brc")
					{
						taskAwaiter = Task.Run(delegate
						{
							if (BRCHelper.GetVersion(CS$<>8__locals2.CS$<>8__locals1.fileproxy.Path) == BRCHelper.BRCType.V1)
							{
								CS$<>8__locals2.recorder = DataRecorder.LoadFromFile(CS$<>8__locals2.CS$<>8__locals1.fileproxy.Path);
								return;
							}
							CS$<>8__locals2.recorder = DataRecordContainer.LoadFromFile(CS$<>8__locals2.CS$<>8__locals1.fileproxy.Path, false);
						}).GetAwaiter();
						if (!taskAwaiter.IsCompleted)
						{
							num = (num2 = 7);
							TaskAwaiter taskAwaiter2 = taskAwaiter;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, SettingsRecording.<btnShare_Clicked>d__18>(ref taskAwaiter, ref this);
							return;
						}
						goto IL_0656;
					}
					else
					{
						taskAwaiter = settingsRecording.DisplayAlert("Error loading data record.", "Wrong extention", "OK").GetAwaiter();
						if (!taskAwaiter.IsCompleted)
						{
							num = (num2 = 8);
							TaskAwaiter taskAwaiter2 = taskAwaiter;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, SettingsRecording.<btnShare_Clicked>d__18>(ref taskAwaiter, ref this);
							return;
						}
						goto IL_06C5;
					}
					IL_05D8:
					taskAwaiter.GetResult();
					goto IL_06F5;
					IL_0656:
					taskAwaiter.GetResult();
					goto IL_06F5;
					IL_06C5:
					taskAwaiter.GetResult();
					settingsRecording.lvRecords.SelectedItem = null;
					settingsRecording.activityFrame.IsVisible = false;
					settingsRecording.lvRecords.IsEnabled = true;
					goto IL_0D1A;
					IL_06F5:
					if (CS$<>8__locals2.recorder == null)
					{
						taskAwaiter = settingsRecording.DisplayAlert("Error loading data record.", "Wrong extention", "OK").GetAwaiter();
						if (!taskAwaiter.IsCompleted)
						{
							num = (num2 = 9);
							TaskAwaiter taskAwaiter2 = taskAwaiter;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, SettingsRecording.<btnShare_Clicked>d__18>(ref taskAwaiter, ref this);
							return;
						}
					}
					else
					{
						CS$<>8__locals2.path = "";
						CS$<>8__locals2.progress = new Progress<int>(delegate(int p)
						{
							Device.BeginInvokeOnMainThread(new Action(new SettingsRecording.<>c__DisplayClass18_2
							{
								CS$<>8__locals2 = CS$<>8__locals2.CS$<>8__locals1,
								p = p
							}.<btnShare_Clicked>b__4));
						});
						CS$<>8__locals2.format_version = 2;
						if (format == "CSV #1")
						{
							CS$<>8__locals2.format_version = 1;
						}
						if (format == "CSV #2")
						{
							CS$<>8__locals2.format_version = 2;
						}
						taskAwaiter = Task.Run(delegate
						{
							CS$<>8__locals2.path = Brc2CsvConverter.Convert(CS$<>8__locals2.recorder, CS$<>8__locals2.CS$<>8__locals1.fileproxy.Name, CS$<>8__locals2.progress, CS$<>8__locals2.format_version);
						}).GetAwaiter();
						if (!taskAwaiter.IsCompleted)
						{
							num = (num2 = 10);
							TaskAwaiter taskAwaiter2 = taskAwaiter;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, SettingsRecording.<btnShare_Clicked>d__18>(ref taskAwaiter, ref this);
							return;
						}
						goto IL_087C;
					}
					IL_0769:
					taskAwaiter.GetResult();
					settingsRecording.lvRecords.SelectedItem = null;
					settingsRecording.activityFrame.IsVisible = false;
					settingsRecording.lvRecords.IsEnabled = true;
					goto IL_0D1A;
					IL_087C:
					taskAwaiter.GetResult();
					if (PlatformHelper.IsAndroid)
					{
						path = Translate.GetString("droid_ExportSend");
						exportVariantResult = Translate.GetString("droid_ExportFile");
						taskAwaiter3 = settingsRecording.DisplayActionSheetCustom(Translate.GetString("droid_ExportMode"), null, null, new string[] { exportVariantResult, path }).GetAwaiter();
						if (!taskAwaiter3.IsCompleted)
						{
							num = (num2 = 11);
							TaskAwaiter<string> taskAwaiter4 = taskAwaiter3;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<string>, SettingsRecording.<btnShare_Clicked>d__18>(ref taskAwaiter3, ref this);
							return;
						}
					}
					else
					{
						if (Device.Idiom != 1)
						{
							PlatformHelper.IOSService.SendDebugEmail(CS$<>8__locals2.path, "", "", "");
							goto IL_0B53;
						}
						taskAwaiter = Share.RequestAsync(new ShareFileRequest(new ShareFile(CS$<>8__locals2.path))).GetAwaiter();
						if (!taskAwaiter.IsCompleted)
						{
							num = (num2 = 14);
							TaskAwaiter taskAwaiter2 = taskAwaiter;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, SettingsRecording.<btnShare_Clicked>d__18>(ref taskAwaiter, ref this);
							return;
						}
						goto IL_0B25;
					}
					IL_0929:
					text = taskAwaiter3.GetResult();
					exportVariantFile = text;
					if (exportVariantFile == exportVariantResult)
					{
						goto IL_094D;
					}
					IL_09C8:
					if (exportVariantFile == path)
					{
						EmailMessage emailMessage2 = new EmailMessage(CS$<>8__locals2.CS$<>8__locals1.fileproxy.Name, "", Array.Empty<string>());
						if (File.Exists(CS$<>8__locals2.path))
						{
							emailMessage2.Attachments.Add(new EmailAttachment(CS$<>8__locals2.path));
							goto IL_0A32;
						}
						goto IL_0A32;
					}
					IL_0A9A:
					path = null;
					exportVariantResult = null;
					exportVariantFile = null;
					goto IL_0B53;
					IL_0B25:
					taskAwaiter.GetResult();
					IL_0B53:
					CS$<>8__locals2 = null;
					goto IL_0CE5;
					IL_0C30:
					taskAwaiter.GetResult();
					goto IL_0CB3;
					IL_0CAC:
					taskAwaiter.GetResult();
					IL_0CB3:
					NanExcluderFromBRC.Exclude(CS$<>8__locals3.CS$<>8__locals3.fileproxy.Path, CS$<>8__locals3.recorder);
					settingsRecording.LoadList();
					CS$<>8__locals3 = null;
					IL_0CE5:
					settingsRecording.activityFrame.IsVisible = false;
				}
				catch (Exception ex)
				{
					num2 = -2;
					CS$<>8__locals1 = null;
					format = null;
					this.<>t__builder.SetException(ex);
					return;
				}
				IL_0D1A:
				num2 = -2;
				CS$<>8__locals1 = null;
				format = null;
				this.<>t__builder.SetResult();
			}

			// Token: 0x06001C14 RID: 7188 RVA: 0x00141120 File Offset: 0x0013F320
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x04000D4D RID: 3405
			public int <>1__state;

			// Token: 0x04000D4E RID: 3406
			public AsyncVoidMethodBuilder <>t__builder;

			// Token: 0x04000D4F RID: 3407
			public SettingsRecording <>4__this;

			// Token: 0x04000D50 RID: 3408
			public object sender;

			// Token: 0x04000D51 RID: 3409
			private SettingsRecording.<>c__DisplayClass18_0 <>8__1;

			// Token: 0x04000D52 RID: 3410
			private SettingsRecording.<>c__DisplayClass18_1 <>8__2;

			// Token: 0x04000D53 RID: 3411
			private SettingsRecording.<>c__DisplayClass18_3 <>8__3;

			// Token: 0x04000D54 RID: 3412
			private string <format>5__2;

			// Token: 0x04000D55 RID: 3413
			private TaskAwaiter <>u__1;

			// Token: 0x04000D56 RID: 3414
			private TaskAwaiter<string> <>u__2;

			// Token: 0x04000D57 RID: 3415
			private string <path>5__3;

			// Token: 0x04000D58 RID: 3416
			private string <exportVariantSend>5__4;

			// Token: 0x04000D59 RID: 3417
			private string <exportVariantFile>5__5;

			// Token: 0x04000D5A RID: 3418
			private string <exportVariantResult>5__6;
		}

		// Token: 0x02000267 RID: 615
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <gridHeader_SizeChanged>d__12 : IAsyncStateMachine
		{
			// Token: 0x06001C15 RID: 7189 RVA: 0x00141130 File Offset: 0x0013F330
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				SettingsRecording settingsRecording = this;
				try
				{
					TaskAwaiter taskAwaiter;
					if (num != 0)
					{
						taskAwaiter = Task.Delay(1000).GetAwaiter();
						if (!taskAwaiter.IsCompleted)
						{
							num2 = 0;
							TaskAwaiter taskAwaiter2 = taskAwaiter;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, SettingsRecording.<gridHeader_SizeChanged>d__12>(ref taskAwaiter, ref this);
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
					settingsRecording.gridHeader.HeightRequest = settingsRecording.settings.VisibleContentHeight;
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

			// Token: 0x06001C16 RID: 7190 RVA: 0x001411FC File Offset: 0x0013F3FC
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x04000D5B RID: 3419
			public int <>1__state;

			// Token: 0x04000D5C RID: 3420
			public AsyncVoidMethodBuilder <>t__builder;

			// Token: 0x04000D5D RID: 3421
			public SettingsRecording <>4__this;

			// Token: 0x04000D5E RID: 3422
			private TaskAwaiter <>u__1;
		}

		// Token: 0x02000268 RID: 616
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <lv_ItemTapped>d__19 : IAsyncStateMachine
		{
			// Token: 0x06001C17 RID: 7191 RVA: 0x0014120C File Offset: 0x0013F40C
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				SettingsRecording settingsRecording = this;
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
						goto IL_022D;
					}
					case 2:
					{
						TaskAwaiter taskAwaiter2;
						taskAwaiter = taskAwaiter2;
						taskAwaiter2 = default(TaskAwaiter);
						num2 = -1;
						goto IL_02AA;
					}
					case 3:
					{
						TaskAwaiter taskAwaiter2;
						taskAwaiter = taskAwaiter2;
						taskAwaiter2 = default(TaskAwaiter);
						num2 = -1;
						goto IL_0319;
					}
					case 4:
					{
						TaskAwaiter taskAwaiter2;
						taskAwaiter = taskAwaiter2;
						taskAwaiter2 = default(TaskAwaiter);
						num2 = -1;
						goto IL_0411;
					}
					case 5:
					{
						TaskAwaiter taskAwaiter2;
						taskAwaiter = taskAwaiter2;
						taskAwaiter2 = default(TaskAwaiter);
						num2 = -1;
						goto IL_0480;
					}
					default:
						CS$<>8__locals1 = new SettingsRecording.<>c__DisplayClass19_0();
						if (e.Item == null)
						{
							goto IL_04CD;
						}
						CS$<>8__locals1.fileproxy = e.Item as FileToRecordProxy;
						if (settingsRecording.Model.IsSelectionMode)
						{
							CS$<>8__locals1.fileproxy.IsSelected = !CS$<>8__locals1.fileproxy.IsSelected;
							settingsRecording.lvRecords.SelectedItem = null;
							goto IL_04CD;
						}
						CS$<>8__locals1.recorder = null;
						if (App.OBDReader != null && App.OBDReader.CurrentCarData != null && App.OBDReader.CurrentCarData.Recorder != null && CS$<>8__locals1.fileproxy.Name == Path.GetFileNameWithoutExtension(App.OBDReader.CurrentCarData.Recorder.FileName))
						{
							taskAwaiter = settingsRecording.DisplayAlert("File is busy!", "Please disconnect first or restart Car Scanner!", "OK").GetAwaiter();
							if (!taskAwaiter.IsCompleted)
							{
								num2 = 0;
								TaskAwaiter taskAwaiter2 = taskAwaiter;
								this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, SettingsRecording.<lv_ItemTapped>d__19>(ref taskAwaiter, ref this);
								return;
							}
						}
						else
						{
							settingsRecording.activityFrame.IsVisible = true;
							settingsRecording.lvRecords.IsEnabled = false;
							string text = Path.GetExtension(CS$<>8__locals1.fileproxy.Path).ToLowerInvariant();
							if (text == ".rec")
							{
								taskAwaiter = Task.Run(delegate
								{
									CS$<>8__locals1.recorder = RecorderedTracksViewModel.LoadRecorderFromFile(CS$<>8__locals1.fileproxy.Path);
								}).GetAwaiter();
								if (!taskAwaiter.IsCompleted)
								{
									num2 = 1;
									TaskAwaiter taskAwaiter2 = taskAwaiter;
									this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, SettingsRecording.<lv_ItemTapped>d__19>(ref taskAwaiter, ref this);
									return;
								}
								goto IL_022D;
							}
							else if (text == ".brc")
							{
								taskAwaiter = Task.Run(delegate
								{
									try
									{
										BRCHelper.BRCType version = BRCHelper.GetVersion(CS$<>8__locals1.fileproxy.Path);
										if (version != BRCHelper.BRCType.V1)
										{
											if (version == BRCHelper.BRCType.V2)
											{
												CS$<>8__locals1.recorder = DataRecordContainer.LoadFromFile(CS$<>8__locals1.fileproxy.Path, false);
											}
										}
										else
										{
											CS$<>8__locals1.recorder = DataRecorder.LoadFromFile(CS$<>8__locals1.fileproxy.Path);
										}
									}
									catch (Exception)
									{
									}
								}).GetAwaiter();
								if (!taskAwaiter.IsCompleted)
								{
									num2 = 2;
									TaskAwaiter taskAwaiter2 = taskAwaiter;
									this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, SettingsRecording.<lv_ItemTapped>d__19>(ref taskAwaiter, ref this);
									return;
								}
								goto IL_02AA;
							}
							else
							{
								taskAwaiter = settingsRecording.DisplayAlert("Error loading data record.", "Wrong extention", "OK").GetAwaiter();
								if (!taskAwaiter.IsCompleted)
								{
									num2 = 3;
									TaskAwaiter taskAwaiter2 = taskAwaiter;
									this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, SettingsRecording.<lv_ItemTapped>d__19>(ref taskAwaiter, ref this);
									return;
								}
								goto IL_0319;
							}
						}
						break;
					}
					taskAwaiter.GetResult();
					settingsRecording.lvRecords.SelectedItem = null;
					goto IL_04CD;
					IL_022D:
					taskAwaiter.GetResult();
					goto IL_0349;
					IL_02AA:
					taskAwaiter.GetResult();
					goto IL_0349;
					IL_0319:
					taskAwaiter.GetResult();
					settingsRecording.lvRecords.SelectedItem = null;
					settingsRecording.activityFrame.IsVisible = false;
					settingsRecording.lvRecords.IsEnabled = true;
					goto IL_04CD;
					IL_0349:
					if (CS$<>8__locals1.recorder != null)
					{
						DataRecorder.AdaptToNewUnits(CS$<>8__locals1.recorder);
						DataRecordContainer.RemoveNaNs(CS$<>8__locals1.recorder);
						DataRecorderSettingsPage dataRecorderSettingsPage = new DataRecorderSettingsPage();
						dataRecorderSettingsPage.BindingContext = CS$<>8__locals1.recorder;
						settingsRecording.lvRecords.SelectedItem = null;
						settingsRecording.activityFrame.IsVisible = false;
						settingsRecording.lvRecords.IsEnabled = true;
						taskAwaiter = settingsRecording.Navigation.PushAsync(dataRecorderSettingsPage).GetAwaiter();
						if (!taskAwaiter.IsCompleted)
						{
							num2 = 4;
							TaskAwaiter taskAwaiter2 = taskAwaiter;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, SettingsRecording.<lv_ItemTapped>d__19>(ref taskAwaiter, ref this);
							return;
						}
					}
					else
					{
						taskAwaiter = settingsRecording.DisplayAlert("Error loading data record.", "Corruped data or file is locked.\nTry to restart Car Scanner if this file is in use right now", "OK").GetAwaiter();
						if (!taskAwaiter.IsCompleted)
						{
							num2 = 5;
							TaskAwaiter taskAwaiter2 = taskAwaiter;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, SettingsRecording.<lv_ItemTapped>d__19>(ref taskAwaiter, ref this);
							return;
						}
						goto IL_0480;
					}
					IL_0411:
					taskAwaiter.GetResult();
					goto IL_04AB;
					IL_0480:
					taskAwaiter.GetResult();
					settingsRecording.lvRecords.SelectedItem = null;
					settingsRecording.activityFrame.IsVisible = false;
					settingsRecording.lvRecords.IsEnabled = true;
					IL_04AB:;
				}
				catch (Exception ex)
				{
					num2 = -2;
					CS$<>8__locals1 = null;
					this.<>t__builder.SetException(ex);
					return;
				}
				IL_04CD:
				num2 = -2;
				CS$<>8__locals1 = null;
				this.<>t__builder.SetResult();
			}

			// Token: 0x06001C18 RID: 7192 RVA: 0x0014171C File Offset: 0x0013F91C
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x04000D5F RID: 3423
			public int <>1__state;

			// Token: 0x04000D60 RID: 3424
			public AsyncVoidMethodBuilder <>t__builder;

			// Token: 0x04000D61 RID: 3425
			public ItemTappedEventArgs e;

			// Token: 0x04000D62 RID: 3426
			public SettingsRecording <>4__this;

			// Token: 0x04000D63 RID: 3427
			private SettingsRecording.<>c__DisplayClass19_0 <>8__1;

			// Token: 0x04000D64 RID: 3428
			private TaskAwaiter <>u__1;
		}

		// Token: 0x02000269 RID: 617
		[CompilerGenerated]
		private sealed class <InitializeComponent>_anonXamlCDataTemplate_97
		{
			// Token: 0x06001C19 RID: 7193 RVA: 0x0014172C File Offset: 0x0013F92C
			public <InitializeComponent>_anonXamlCDataTemplate_97()
			{
			}

			// Token: 0x06001C1A RID: 7194 RVA: 0x00141740 File Offset: 0x0013F940
			internal object LoadDataTemplate()
			{
				BindingExtension bindingExtension;
				VisualDiagnostics.RegisterSourceInfo(bindingExtension = new BindingExtension(), new Uri("Settings\\SettingsRecording.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 142, 37);
				Translate translate;
				VisualDiagnostics.RegisterSourceInfo(translate = new Translate(), new Uri("Settings\\SettingsRecording.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 144, 37);
				MenuItem menuItem;
				VisualDiagnostics.RegisterSourceInfo(menuItem = new MenuItem(), new Uri("Settings\\SettingsRecording.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 140, 34);
				BindingExtension bindingExtension2;
				VisualDiagnostics.RegisterSourceInfo(bindingExtension2 = new BindingExtension(), new Uri("Settings\\SettingsRecording.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 147, 37);
				Translate translate2;
				VisualDiagnostics.RegisterSourceInfo(translate2 = new Translate(), new Uri("Settings\\SettingsRecording.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 149, 37);
				MenuItem menuItem2;
				VisualDiagnostics.RegisterSourceInfo(menuItem2 = new MenuItem(), new Uri("Settings\\SettingsRecording.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 145, 34);
				BindingExtension bindingExtension3;
				VisualDiagnostics.RegisterSourceInfo(bindingExtension3 = new BindingExtension(), new Uri("Settings\\SettingsRecording.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 152, 37);
				Translate translate3;
				VisualDiagnostics.RegisterSourceInfo(translate3 = new Translate(), new Uri("Settings\\SettingsRecording.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 154, 37);
				MenuItem menuItem3;
				VisualDiagnostics.RegisterSourceInfo(menuItem3 = new MenuItem(), new Uri("Settings\\SettingsRecording.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 150, 34);
				BindingExtension bindingExtension4;
				VisualDiagnostics.RegisterSourceInfo(bindingExtension4 = new BindingExtension(), new Uri("Settings\\SettingsRecording.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 105, 37);
				ReferenceExtension referenceExtension;
				VisualDiagnostics.RegisterSourceInfo(referenceExtension = new ReferenceExtension(), new Uri("Settings\\SettingsRecording.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 106, 37);
				BindingExtension bindingExtension5;
				VisualDiagnostics.RegisterSourceInfo(bindingExtension5 = new BindingExtension(), new Uri("Settings\\SettingsRecording.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 106, 37);
				CheckBox checkBox;
				VisualDiagnostics.RegisterSourceInfo(checkBox = new CheckBox(), new Uri("Settings\\SettingsRecording.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 102, 34);
				DynamicResourceExtension dynamicResourceExtension;
				VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension = new DynamicResourceExtension(), new Uri("Settings\\SettingsRecording.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 118, 53);
				BindingExtension bindingExtension6;
				VisualDiagnostics.RegisterSourceInfo(bindingExtension6 = new BindingExtension(), new Uri("Settings\\SettingsRecording.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 119, 53);
				Span span;
				VisualDiagnostics.RegisterSourceInfo(span = new Span(), new Uri("Settings\\SettingsRecording.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 116, 50);
				DynamicResourceExtension dynamicResourceExtension2;
				VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension2 = new DynamicResourceExtension(), new Uri("Settings\\SettingsRecording.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 120, 55);
				Span span2;
				VisualDiagnostics.RegisterSourceInfo(span2 = new Span(), new Uri("Settings\\SettingsRecording.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 120, 50);
				DynamicResourceExtension dynamicResourceExtension3;
				VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension3 = new DynamicResourceExtension(), new Uri("Settings\\SettingsRecording.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 121, 55);
				BindingExtension bindingExtension7;
				VisualDiagnostics.RegisterSourceInfo(bindingExtension7 = new BindingExtension(), new Uri("Settings\\SettingsRecording.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 121, 98);
				Span span3;
				VisualDiagnostics.RegisterSourceInfo(span3 = new Span(), new Uri("Settings\\SettingsRecording.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 121, 50);
				DynamicResourceExtension dynamicResourceExtension4;
				VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension4 = new DynamicResourceExtension(), new Uri("Settings\\SettingsRecording.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 122, 55);
				Span span4;
				VisualDiagnostics.RegisterSourceInfo(span4 = new Span(), new Uri("Settings\\SettingsRecording.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 122, 50);
				FormattedString formattedString;
				VisualDiagnostics.RegisterSourceInfo(formattedString = new FormattedString(), new Uri("Settings\\SettingsRecording.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 115, 46);
				Label label;
				VisualDiagnostics.RegisterSourceInfo(label = new Label(), new Uri("Settings\\SettingsRecording.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 113, 38);
				DynamicResourceExtension dynamicResourceExtension5;
				VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension5 = new DynamicResourceExtension(), new Uri("Settings\\SettingsRecording.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 129, 41);
				BindingExtension bindingExtension8;
				VisualDiagnostics.RegisterSourceInfo(bindingExtension8 = new BindingExtension(), new Uri("Settings\\SettingsRecording.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 130, 41);
				Label label2;
				VisualDiagnostics.RegisterSourceInfo(label2 = new Label(), new Uri("Settings\\SettingsRecording.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 127, 38);
				DynamicResourceExtension dynamicResourceExtension6;
				VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension6 = new DynamicResourceExtension(), new Uri("Settings\\SettingsRecording.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 132, 41);
				StaticResourceExtension staticResourceExtension;
				VisualDiagnostics.RegisterSourceInfo(staticResourceExtension = new StaticResourceExtension(), new Uri("Settings\\SettingsRecording.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 133, 41);
				BindingExtension bindingExtension9;
				VisualDiagnostics.RegisterSourceInfo(bindingExtension9 = new BindingExtension(), new Uri("Settings\\SettingsRecording.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 133, 41);
				BindingExtension bindingExtension10;
				VisualDiagnostics.RegisterSourceInfo(bindingExtension10 = new BindingExtension(), new Uri("Settings\\SettingsRecording.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 134, 41);
				Label label3;
				VisualDiagnostics.RegisterSourceInfo(label3 = new Label(), new Uri("Settings\\SettingsRecording.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 131, 38);
				StackLayout stackLayout;
				VisualDiagnostics.RegisterSourceInfo(stackLayout = new StackLayout(), new Uri("Settings\\SettingsRecording.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 109, 34);
				Grid grid;
				VisualDiagnostics.RegisterSourceInfo(grid = new Grid(), new Uri("Settings\\SettingsRecording.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 101, 30);
				ViewCell viewCell;
				VisualDiagnostics.RegisterSourceInfo(viewCell = new ViewCell(), new Uri("Settings\\SettingsRecording.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 100, 26);
				NameScope nameScope = new NameScope();
				NameScope.SetNameScope(viewCell, nameScope);
				menuItem.Clicked += this.root.btnRename_Clicked;
				bindingExtension.Path = ".";
				BindingBase bindingBase = bindingExtension.ProvideValue(null);
				menuItem.SetBinding(MenuItem.CommandParameterProperty, bindingBase);
				menuItem.SetValue(MenuItem.IsDestructiveProperty, false);
				translate.Text = "ios_Rename";
				IMarkupExtension markupExtension = translate;
				XamlServiceProvider xamlServiceProvider = new XamlServiceProvider();
				Type typeFromHandle = typeof(IProvideValueTarget);
				int num;
				object[] array = new object[(num = this.parentValues.Length) + 2];
				Array.Copy(this.parentValues, 0, array, 2, num);
				object[] array2 = array;
				array2[0] = menuItem;
				array2[1] = viewCell;
				object obj;
				xamlServiceProvider.Add(typeFromHandle, obj = new SimpleValueTargetProvider(array2, MenuItem.TextProperty, nameScope));
				xamlServiceProvider.Add(typeof(IReferenceProvider), obj);
				Type typeFromHandle2 = typeof(IXamlTypeResolver);
				XmlNamespaceResolver xmlNamespaceResolver = new XmlNamespaceResolver();
				xmlNamespaceResolver.Add("", "http://xamarin.com/schemas/2014/forms");
				xmlNamespaceResolver.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
				xmlNamespaceResolver.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
				xmlNamespaceResolver.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
				xmlNamespaceResolver.Add("local", "clr-namespace:CarScannerXamarinForms");
				xmlNamespaceResolver.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
				xmlNamespaceResolver.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
				xmlNamespaceResolver.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
				xmlNamespaceResolver.Add("vm", "clr-namespace:CarScannerXamarinForms.ViewModels");
				xamlServiceProvider.Add(typeFromHandle2, new XamlTypeResolver(xmlNamespaceResolver, typeof(SettingsRecording.<InitializeComponent>_anonXamlCDataTemplate_97).GetTypeInfo().Assembly));
				xamlServiceProvider.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(144, 37)));
				object obj2 = markupExtension.ProvideValue(xamlServiceProvider);
				menuItem.Text = obj2;
				viewCell.ContextActions.Add(menuItem);
				menuItem2.Clicked += this.root.btnShare_Clicked;
				bindingExtension2.Path = ".";
				BindingBase bindingBase2 = bindingExtension2.ProvideValue(null);
				menuItem2.SetBinding(MenuItem.CommandParameterProperty, bindingBase2);
				menuItem2.SetValue(MenuItem.IsDestructiveProperty, false);
				translate2.Text = "ios_Share";
				IMarkupExtension markupExtension2 = translate2;
				XamlServiceProvider xamlServiceProvider2 = new XamlServiceProvider();
				Type typeFromHandle3 = typeof(IProvideValueTarget);
				int num2;
				object[] array3 = new object[(num2 = this.parentValues.Length) + 2];
				Array.Copy(this.parentValues, 0, array3, 2, num2);
				object[] array4 = array3;
				array4[0] = menuItem2;
				array4[1] = viewCell;
				object obj3;
				xamlServiceProvider2.Add(typeFromHandle3, obj3 = new SimpleValueTargetProvider(array4, MenuItem.TextProperty, nameScope));
				xamlServiceProvider2.Add(typeof(IReferenceProvider), obj3);
				Type typeFromHandle4 = typeof(IXamlTypeResolver);
				XmlNamespaceResolver xmlNamespaceResolver2 = new XmlNamespaceResolver();
				xmlNamespaceResolver2.Add("", "http://xamarin.com/schemas/2014/forms");
				xmlNamespaceResolver2.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
				xmlNamespaceResolver2.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
				xmlNamespaceResolver2.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
				xmlNamespaceResolver2.Add("local", "clr-namespace:CarScannerXamarinForms");
				xmlNamespaceResolver2.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
				xmlNamespaceResolver2.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
				xmlNamespaceResolver2.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
				xmlNamespaceResolver2.Add("vm", "clr-namespace:CarScannerXamarinForms.ViewModels");
				xamlServiceProvider2.Add(typeFromHandle4, new XamlTypeResolver(xmlNamespaceResolver2, typeof(SettingsRecording.<InitializeComponent>_anonXamlCDataTemplate_97).GetTypeInfo().Assembly));
				xamlServiceProvider2.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(149, 37)));
				object obj4 = markupExtension2.ProvideValue(xamlServiceProvider2);
				menuItem2.Text = obj4;
				viewCell.ContextActions.Add(menuItem2);
				menuItem3.Clicked += this.root.btnDelete_Clicked;
				bindingExtension3.Path = ".";
				BindingBase bindingBase3 = bindingExtension3.ProvideValue(null);
				menuItem3.SetBinding(MenuItem.CommandParameterProperty, bindingBase3);
				menuItem3.SetValue(MenuItem.IsDestructiveProperty, true);
				translate3.Text = "SpeedTest_btnRemove.Label";
				IMarkupExtension markupExtension3 = translate3;
				XamlServiceProvider xamlServiceProvider3 = new XamlServiceProvider();
				Type typeFromHandle5 = typeof(IProvideValueTarget);
				int num3;
				object[] array5 = new object[(num3 = this.parentValues.Length) + 2];
				Array.Copy(this.parentValues, 0, array5, 2, num3);
				object[] array6 = array5;
				array6[0] = menuItem3;
				array6[1] = viewCell;
				object obj5;
				xamlServiceProvider3.Add(typeFromHandle5, obj5 = new SimpleValueTargetProvider(array6, MenuItem.TextProperty, nameScope));
				xamlServiceProvider3.Add(typeof(IReferenceProvider), obj5);
				Type typeFromHandle6 = typeof(IXamlTypeResolver);
				XmlNamespaceResolver xmlNamespaceResolver3 = new XmlNamespaceResolver();
				xmlNamespaceResolver3.Add("", "http://xamarin.com/schemas/2014/forms");
				xmlNamespaceResolver3.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
				xmlNamespaceResolver3.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
				xmlNamespaceResolver3.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
				xmlNamespaceResolver3.Add("local", "clr-namespace:CarScannerXamarinForms");
				xmlNamespaceResolver3.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
				xmlNamespaceResolver3.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
				xmlNamespaceResolver3.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
				xmlNamespaceResolver3.Add("vm", "clr-namespace:CarScannerXamarinForms.ViewModels");
				xamlServiceProvider3.Add(typeFromHandle6, new XamlTypeResolver(xmlNamespaceResolver3, typeof(SettingsRecording.<InitializeComponent>_anonXamlCDataTemplate_97).GetTypeInfo().Assembly));
				xamlServiceProvider3.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(154, 37)));
				object obj6 = markupExtension3.ProvideValue(xamlServiceProvider3);
				menuItem3.Text = obj6;
				viewCell.ContextActions.Add(menuItem3);
				grid.SetValue(Grid.ColumnDefinitionsProperty, new ColumnDefinitionCollectionTypeConverter().ConvertFromInvariantString("auto, *"));
				grid.SetValue(Grid.RowDefinitionsProperty, new RowDefinitionCollectionTypeConverter().ConvertFromInvariantString("auto"));
				checkBox.SetValue(Grid.RowProperty, 0);
				checkBox.SetValue(Grid.ColumnProperty, 0);
				bindingExtension4.Mode = 1;
				bindingExtension4.Path = "IsSelected";
				BindingBase bindingBase4 = bindingExtension4.ProvideValue(null);
				checkBox.SetBinding(CheckBox.IsCheckedProperty, bindingBase4);
				referenceExtension.Name = "me";
				IMarkupExtension markupExtension4 = referenceExtension;
				XamlServiceProvider xamlServiceProvider4 = new XamlServiceProvider();
				Type typeFromHandle7 = typeof(IProvideValueTarget);
				int num4;
				object[] array7 = new object[(num4 = this.parentValues.Length) + 4];
				Array.Copy(this.parentValues, 0, array7, 4, num4);
				object[] array8 = array7;
				array8[0] = bindingExtension5;
				array8[1] = checkBox;
				array8[2] = grid;
				array8[3] = viewCell;
				object obj7;
				xamlServiceProvider4.Add(typeFromHandle7, obj7 = new SimpleValueTargetProvider(array8, typeof(BindingExtension).GetRuntimeProperty("Source"), nameScope));
				xamlServiceProvider4.Add(typeof(IReferenceProvider), obj7);
				Type typeFromHandle8 = typeof(IXamlTypeResolver);
				XmlNamespaceResolver xmlNamespaceResolver4 = new XmlNamespaceResolver();
				xmlNamespaceResolver4.Add("", "http://xamarin.com/schemas/2014/forms");
				xmlNamespaceResolver4.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
				xmlNamespaceResolver4.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
				xmlNamespaceResolver4.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
				xmlNamespaceResolver4.Add("local", "clr-namespace:CarScannerXamarinForms");
				xmlNamespaceResolver4.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
				xmlNamespaceResolver4.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
				xmlNamespaceResolver4.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
				xmlNamespaceResolver4.Add("vm", "clr-namespace:CarScannerXamarinForms.ViewModels");
				xamlServiceProvider4.Add(typeFromHandle8, new XamlTypeResolver(xmlNamespaceResolver4, typeof(SettingsRecording.<InitializeComponent>_anonXamlCDataTemplate_97).GetTypeInfo().Assembly));
				xamlServiceProvider4.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(106, 37)));
				object obj8 = markupExtension4.ProvideValue(xamlServiceProvider4);
				bindingExtension5.Source = obj8;
				bindingExtension5.Path = "IsSelectionMode";
				bindingExtension5.Mode = 2;
				BindingBase bindingBase5 = bindingExtension5.ProvideValue(null);
				checkBox.SetBinding(VisualElement.IsVisibleProperty, bindingBase5);
				grid.Children.Add(checkBox);
				stackLayout.SetValue(Grid.RowProperty, 0);
				stackLayout.SetValue(Grid.ColumnProperty, 1);
				stackLayout.SetValue(StackLayout.OrientationProperty, 0);
				span.SetValue(Span.FontAttributesProperty, new FontAttributesConverter().ConvertFromInvariantString("Bold"));
				dynamicResourceExtension.Key = "BaseFontSize++";
				IMarkupExtension<DynamicResource> markupExtension5 = dynamicResourceExtension;
				XamlServiceProvider xamlServiceProvider5 = new XamlServiceProvider();
				Type typeFromHandle9 = typeof(IProvideValueTarget);
				int num5;
				object[] array9 = new object[(num5 = this.parentValues.Length) + 6];
				Array.Copy(this.parentValues, 0, array9, 6, num5);
				object[] array10 = array9;
				array10[0] = span;
				array10[1] = formattedString;
				array10[2] = label;
				array10[3] = stackLayout;
				array10[4] = grid;
				array10[5] = viewCell;
				object obj9;
				xamlServiceProvider5.Add(typeFromHandle9, obj9 = new SimpleValueTargetProvider(array10, Span.FontSizeProperty, nameScope));
				xamlServiceProvider5.Add(typeof(IReferenceProvider), obj9);
				Type typeFromHandle10 = typeof(IXamlTypeResolver);
				XmlNamespaceResolver xmlNamespaceResolver5 = new XmlNamespaceResolver();
				xmlNamespaceResolver5.Add("", "http://xamarin.com/schemas/2014/forms");
				xmlNamespaceResolver5.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
				xmlNamespaceResolver5.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
				xmlNamespaceResolver5.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
				xmlNamespaceResolver5.Add("local", "clr-namespace:CarScannerXamarinForms");
				xmlNamespaceResolver5.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
				xmlNamespaceResolver5.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
				xmlNamespaceResolver5.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
				xmlNamespaceResolver5.Add("vm", "clr-namespace:CarScannerXamarinForms.ViewModels");
				xamlServiceProvider5.Add(typeFromHandle10, new XamlTypeResolver(xmlNamespaceResolver5, typeof(SettingsRecording.<InitializeComponent>_anonXamlCDataTemplate_97).GetTypeInfo().Assembly));
				xamlServiceProvider5.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(118, 53)));
				DynamicResource dynamicResource = markupExtension5.ProvideValue(xamlServiceProvider5);
				span.SetDynamicResource(Span.FontSizeProperty, dynamicResource.Key);
				bindingExtension6.Path = "Name";
				BindingBase bindingBase6 = bindingExtension6.ProvideValue(null);
				span.SetBinding(Span.TextProperty, bindingBase6);
				formattedString.Spans.Add(span);
				dynamicResourceExtension2.Key = "BaseFontSize+";
				IMarkupExtension<DynamicResource> markupExtension6 = dynamicResourceExtension2;
				XamlServiceProvider xamlServiceProvider6 = new XamlServiceProvider();
				Type typeFromHandle11 = typeof(IProvideValueTarget);
				int num6;
				object[] array11 = new object[(num6 = this.parentValues.Length) + 6];
				Array.Copy(this.parentValues, 0, array11, 6, num6);
				object[] array12 = array11;
				array12[0] = span2;
				array12[1] = formattedString;
				array12[2] = label;
				array12[3] = stackLayout;
				array12[4] = grid;
				array12[5] = viewCell;
				object obj10;
				xamlServiceProvider6.Add(typeFromHandle11, obj10 = new SimpleValueTargetProvider(array12, Span.FontSizeProperty, nameScope));
				xamlServiceProvider6.Add(typeof(IReferenceProvider), obj10);
				Type typeFromHandle12 = typeof(IXamlTypeResolver);
				XmlNamespaceResolver xmlNamespaceResolver6 = new XmlNamespaceResolver();
				xmlNamespaceResolver6.Add("", "http://xamarin.com/schemas/2014/forms");
				xmlNamespaceResolver6.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
				xmlNamespaceResolver6.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
				xmlNamespaceResolver6.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
				xmlNamespaceResolver6.Add("local", "clr-namespace:CarScannerXamarinForms");
				xmlNamespaceResolver6.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
				xmlNamespaceResolver6.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
				xmlNamespaceResolver6.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
				xmlNamespaceResolver6.Add("vm", "clr-namespace:CarScannerXamarinForms.ViewModels");
				xamlServiceProvider6.Add(typeFromHandle12, new XamlTypeResolver(xmlNamespaceResolver6, typeof(SettingsRecording.<InitializeComponent>_anonXamlCDataTemplate_97).GetTypeInfo().Assembly));
				xamlServiceProvider6.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(120, 55)));
				DynamicResource dynamicResource2 = markupExtension6.ProvideValue(xamlServiceProvider6);
				span2.SetDynamicResource(Span.FontSizeProperty, dynamicResource2.Key);
				span2.SetValue(Span.TextProperty, " [");
				formattedString.Spans.Add(span2);
				dynamicResourceExtension3.Key = "BaseFontSize+";
				IMarkupExtension<DynamicResource> markupExtension7 = dynamicResourceExtension3;
				XamlServiceProvider xamlServiceProvider7 = new XamlServiceProvider();
				Type typeFromHandle13 = typeof(IProvideValueTarget);
				int num7;
				object[] array13 = new object[(num7 = this.parentValues.Length) + 6];
				Array.Copy(this.parentValues, 0, array13, 6, num7);
				object[] array14 = array13;
				array14[0] = span3;
				array14[1] = formattedString;
				array14[2] = label;
				array14[3] = stackLayout;
				array14[4] = grid;
				array14[5] = viewCell;
				object obj11;
				xamlServiceProvider7.Add(typeFromHandle13, obj11 = new SimpleValueTargetProvider(array14, Span.FontSizeProperty, nameScope));
				xamlServiceProvider7.Add(typeof(IReferenceProvider), obj11);
				Type typeFromHandle14 = typeof(IXamlTypeResolver);
				XmlNamespaceResolver xmlNamespaceResolver7 = new XmlNamespaceResolver();
				xmlNamespaceResolver7.Add("", "http://xamarin.com/schemas/2014/forms");
				xmlNamespaceResolver7.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
				xmlNamespaceResolver7.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
				xmlNamespaceResolver7.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
				xmlNamespaceResolver7.Add("local", "clr-namespace:CarScannerXamarinForms");
				xmlNamespaceResolver7.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
				xmlNamespaceResolver7.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
				xmlNamespaceResolver7.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
				xmlNamespaceResolver7.Add("vm", "clr-namespace:CarScannerXamarinForms.ViewModels");
				xamlServiceProvider7.Add(typeFromHandle14, new XamlTypeResolver(xmlNamespaceResolver7, typeof(SettingsRecording.<InitializeComponent>_anonXamlCDataTemplate_97).GetTypeInfo().Assembly));
				xamlServiceProvider7.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(121, 55)));
				DynamicResource dynamicResource3 = markupExtension7.ProvideValue(xamlServiceProvider7);
				span3.SetDynamicResource(Span.FontSizeProperty, dynamicResource3.Key);
				bindingExtension7.Path = "Size";
				BindingBase bindingBase7 = bindingExtension7.ProvideValue(null);
				span3.SetBinding(Span.TextProperty, bindingBase7);
				formattedString.Spans.Add(span3);
				dynamicResourceExtension4.Key = "BaseFontSize+";
				IMarkupExtension<DynamicResource> markupExtension8 = dynamicResourceExtension4;
				XamlServiceProvider xamlServiceProvider8 = new XamlServiceProvider();
				Type typeFromHandle15 = typeof(IProvideValueTarget);
				int num8;
				object[] array15 = new object[(num8 = this.parentValues.Length) + 6];
				Array.Copy(this.parentValues, 0, array15, 6, num8);
				object[] array16 = array15;
				array16[0] = span4;
				array16[1] = formattedString;
				array16[2] = label;
				array16[3] = stackLayout;
				array16[4] = grid;
				array16[5] = viewCell;
				object obj12;
				xamlServiceProvider8.Add(typeFromHandle15, obj12 = new SimpleValueTargetProvider(array16, Span.FontSizeProperty, nameScope));
				xamlServiceProvider8.Add(typeof(IReferenceProvider), obj12);
				Type typeFromHandle16 = typeof(IXamlTypeResolver);
				XmlNamespaceResolver xmlNamespaceResolver8 = new XmlNamespaceResolver();
				xmlNamespaceResolver8.Add("", "http://xamarin.com/schemas/2014/forms");
				xmlNamespaceResolver8.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
				xmlNamespaceResolver8.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
				xmlNamespaceResolver8.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
				xmlNamespaceResolver8.Add("local", "clr-namespace:CarScannerXamarinForms");
				xmlNamespaceResolver8.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
				xmlNamespaceResolver8.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
				xmlNamespaceResolver8.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
				xmlNamespaceResolver8.Add("vm", "clr-namespace:CarScannerXamarinForms.ViewModels");
				xamlServiceProvider8.Add(typeFromHandle16, new XamlTypeResolver(xmlNamespaceResolver8, typeof(SettingsRecording.<InitializeComponent>_anonXamlCDataTemplate_97).GetTypeInfo().Assembly));
				xamlServiceProvider8.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(122, 55)));
				DynamicResource dynamicResource4 = markupExtension8.ProvideValue(xamlServiceProvider8);
				span4.SetDynamicResource(Span.FontSizeProperty, dynamicResource4.Key);
				span4.SetValue(Span.TextProperty, "]");
				formattedString.Spans.Add(span4);
				label.SetValue(Label.FormattedTextProperty, formattedString);
				stackLayout.Children.Add(label);
				label2.SetValue(Label.FontAttributesProperty, new FontAttributesConverter().ConvertFromInvariantString("Italic"));
				dynamicResourceExtension5.Key = "BaseFontSize+";
				IMarkupExtension<DynamicResource> markupExtension9 = dynamicResourceExtension5;
				XamlServiceProvider xamlServiceProvider9 = new XamlServiceProvider();
				Type typeFromHandle17 = typeof(IProvideValueTarget);
				int num9;
				object[] array17 = new object[(num9 = this.parentValues.Length) + 4];
				Array.Copy(this.parentValues, 0, array17, 4, num9);
				object[] array18 = array17;
				array18[0] = label2;
				array18[1] = stackLayout;
				array18[2] = grid;
				array18[3] = viewCell;
				object obj13;
				xamlServiceProvider9.Add(typeFromHandle17, obj13 = new SimpleValueTargetProvider(array18, Label.FontSizeProperty, nameScope));
				xamlServiceProvider9.Add(typeof(IReferenceProvider), obj13);
				Type typeFromHandle18 = typeof(IXamlTypeResolver);
				XmlNamespaceResolver xmlNamespaceResolver9 = new XmlNamespaceResolver();
				xmlNamespaceResolver9.Add("", "http://xamarin.com/schemas/2014/forms");
				xmlNamespaceResolver9.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
				xmlNamespaceResolver9.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
				xmlNamespaceResolver9.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
				xmlNamespaceResolver9.Add("local", "clr-namespace:CarScannerXamarinForms");
				xmlNamespaceResolver9.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
				xmlNamespaceResolver9.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
				xmlNamespaceResolver9.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
				xmlNamespaceResolver9.Add("vm", "clr-namespace:CarScannerXamarinForms.ViewModels");
				xamlServiceProvider9.Add(typeFromHandle18, new XamlTypeResolver(xmlNamespaceResolver9, typeof(SettingsRecording.<InitializeComponent>_anonXamlCDataTemplate_97).GetTypeInfo().Assembly));
				xamlServiceProvider9.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(129, 41)));
				DynamicResource dynamicResource5 = markupExtension9.ProvideValue(xamlServiceProvider9);
				label2.SetDynamicResource(Label.FontSizeProperty, dynamicResource5.Key);
				bindingExtension8.Path = "CarName";
				BindingBase bindingBase8 = bindingExtension8.ProvideValue(null);
				label2.SetBinding(Label.TextProperty, bindingBase8);
				stackLayout.Children.Add(label2);
				dynamicResourceExtension6.Key = "BaseFontSize+";
				IMarkupExtension<DynamicResource> markupExtension10 = dynamicResourceExtension6;
				XamlServiceProvider xamlServiceProvider10 = new XamlServiceProvider();
				Type typeFromHandle19 = typeof(IProvideValueTarget);
				int num10;
				object[] array19 = new object[(num10 = this.parentValues.Length) + 4];
				Array.Copy(this.parentValues, 0, array19, 4, num10);
				object[] array20 = array19;
				array20[0] = label3;
				array20[1] = stackLayout;
				array20[2] = grid;
				array20[3] = viewCell;
				object obj14;
				xamlServiceProvider10.Add(typeFromHandle19, obj14 = new SimpleValueTargetProvider(array20, Label.FontSizeProperty, nameScope));
				xamlServiceProvider10.Add(typeof(IReferenceProvider), obj14);
				Type typeFromHandle20 = typeof(IXamlTypeResolver);
				XmlNamespaceResolver xmlNamespaceResolver10 = new XmlNamespaceResolver();
				xmlNamespaceResolver10.Add("", "http://xamarin.com/schemas/2014/forms");
				xmlNamespaceResolver10.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
				xmlNamespaceResolver10.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
				xmlNamespaceResolver10.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
				xmlNamespaceResolver10.Add("local", "clr-namespace:CarScannerXamarinForms");
				xmlNamespaceResolver10.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
				xmlNamespaceResolver10.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
				xmlNamespaceResolver10.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
				xmlNamespaceResolver10.Add("vm", "clr-namespace:CarScannerXamarinForms.ViewModels");
				xamlServiceProvider10.Add(typeFromHandle20, new XamlTypeResolver(xmlNamespaceResolver10, typeof(SettingsRecording.<InitializeComponent>_anonXamlCDataTemplate_97).GetTypeInfo().Assembly));
				xamlServiceProvider10.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(132, 41)));
				DynamicResource dynamicResource6 = markupExtension10.ProvideValue(xamlServiceProvider10);
				label3.SetDynamicResource(Label.FontSizeProperty, dynamicResource6.Key);
				staticResourceExtension.Key = "EmptyStringToFalseConverter";
				IMarkupExtension markupExtension11 = staticResourceExtension;
				XamlServiceProvider xamlServiceProvider11 = new XamlServiceProvider();
				Type typeFromHandle21 = typeof(IProvideValueTarget);
				int num11;
				object[] array21 = new object[(num11 = this.parentValues.Length) + 5];
				Array.Copy(this.parentValues, 0, array21, 5, num11);
				object[] array22 = array21;
				array22[0] = bindingExtension9;
				array22[1] = label3;
				array22[2] = stackLayout;
				array22[3] = grid;
				array22[4] = viewCell;
				object obj15;
				xamlServiceProvider11.Add(typeFromHandle21, obj15 = new SimpleValueTargetProvider(array22, typeof(BindingExtension).GetRuntimeProperty("Converter"), nameScope));
				xamlServiceProvider11.Add(typeof(IReferenceProvider), obj15);
				Type typeFromHandle22 = typeof(IXamlTypeResolver);
				XmlNamespaceResolver xmlNamespaceResolver11 = new XmlNamespaceResolver();
				xmlNamespaceResolver11.Add("", "http://xamarin.com/schemas/2014/forms");
				xmlNamespaceResolver11.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
				xmlNamespaceResolver11.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
				xmlNamespaceResolver11.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
				xmlNamespaceResolver11.Add("local", "clr-namespace:CarScannerXamarinForms");
				xmlNamespaceResolver11.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
				xmlNamespaceResolver11.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
				xmlNamespaceResolver11.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
				xmlNamespaceResolver11.Add("vm", "clr-namespace:CarScannerXamarinForms.ViewModels");
				xamlServiceProvider11.Add(typeFromHandle22, new XamlTypeResolver(xmlNamespaceResolver11, typeof(SettingsRecording.<InitializeComponent>_anonXamlCDataTemplate_97).GetTypeInfo().Assembly));
				xamlServiceProvider11.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(133, 41)));
				object obj16 = markupExtension11.ProvideValue(xamlServiceProvider11);
				bindingExtension9.Converter = obj16;
				bindingExtension9.Path = "VIN";
				BindingBase bindingBase9 = bindingExtension9.ProvideValue(null);
				label3.SetBinding(VisualElement.IsVisibleProperty, bindingBase9);
				bindingExtension10.Path = "VIN";
				BindingBase bindingBase10 = bindingExtension10.ProvideValue(null);
				label3.SetBinding(Label.TextProperty, bindingBase10);
				stackLayout.Children.Add(label3);
				grid.Children.Add(stackLayout);
				viewCell.View = grid;
				return viewCell;
			}

			// Token: 0x04000D65 RID: 3429
			internal object[] parentValues;

			// Token: 0x04000D66 RID: 3430
			internal SettingsRecording root;
		}
	}
}
