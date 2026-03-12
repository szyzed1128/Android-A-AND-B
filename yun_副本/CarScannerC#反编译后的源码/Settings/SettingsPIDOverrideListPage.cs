using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using CarScannerXamarinForms.Common;
using CarScannerXamarinForms.Common.XAMLConverters;
using CarScannerXamarinForms.OBD2;
using CarScannerXamarinForms.OBD2.PIDS;
using CarScannerXamarinForms.PlatformAdapters;
using CarScannerXamarinForms.Settings.SettingsV3;
using CarScannerXamarinForms.ViewModels;
using Newtonsoft.Json;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Internals;
using Xamarin.Forms.PlatformConfiguration.iOSSpecific;
using Xamarin.Forms.Xaml;
using Xamarin.Forms.Xaml.Diagnostics;
using Xamarin.Forms.Xaml.Internals;

namespace CarScannerXamarinForms.Settings
{
	// Token: 0x02000244 RID: 580
	[XamlCompilation(2)]
	[XamlFilePath("Settings\\SettingsPIDOverrideListPage.xaml")]
	public class SettingsPIDOverrideListPage : ContentPage
	{
		// Token: 0x06001B56 RID: 6998 RVA: 0x0013280C File Offset: 0x00130A0C
		public SettingsPIDOverrideListPage()
		{
			this.InitializeComponent();
			this.pickerBaseList.ItemsSource = new string[]
			{
				Translate.GetString("ios_Sensors_All"),
				Translate.GetString("ios_Sensors_OBDII"),
				Translate.GetString("ios_ProfileAll"),
				Translate.GetString("ios_ProfileActive"),
				Translate.GetString("ios_ProfileDisabled"),
				Translate.GetString("ios_Custom"),
				Translate.GetString("ios_Supported"),
				Translate.GetString("settings_WithRoles")
			};
			base.ToolbarItems.Add(new ToolbarItem("", (string)Application.Current.Resources["NB_add"], delegate
			{
				this.AddNewCustomPid();
			}, 0, 0));
			base.ToolbarItems.Add(new ToolbarItem("", (string)Application.Current.Resources["NB_delete"], delegate
			{
				this.DeleteAllCustomPidsQuestion();
			}, 0, 0));
			base.ToolbarItems.Add(new ToolbarItem("", (string)Application.Current.Resources["NB_settings"], delegate
			{
				this.btnManageProfilePids_Clicked(this, EventArgs.Empty);
			}, 0, 0));
			this.Model = new PIDEditorListModel();
			base.Appearing += this.SettingsPIDOverrideListPage_Appearing;
			base.BindingContext = this.Model;
		}

		// Token: 0x06001B57 RID: 6999 RVA: 0x0013297C File Offset: 0x00130B7C
		private async void SettingsPIDOverrideListPage_Appearing(object sender, EventArgs e)
		{
			this.activityFrame.IsVisible = true;
			if (!CustomPIDViewModel.DisabledProfile.Loaded)
			{
				await Task.Run(delegate
				{
					CustomPIDViewModel.DisabledProfile.Load();
				});
			}
			await this.Model.UpdateBaseList(true);
			this.activityFrame.IsVisible = false;
		}

		// Token: 0x06001B58 RID: 7000 RVA: 0x001329B4 File Offset: 0x00130BB4
		private async void AddNewCustomPid()
		{
			CustomPID cpid = new CustomPID("New PID", "New PID", "", "", "", UnitsHelper.Units.None, 0.0, 100.0, "", "", false, Roles.None, CustomPIDType.Formula, 0, 1, 1.0, 1.0, 0.0, false, false, true, null);
			CustomPIDViewModel.CurrentCustom.PidCollection.Add(cpid);
			CustomPIDViewModel.CurrentCustom.Save();
			if (App.OBDReader.CurrentStatus == OBDDataReaderStatus.ConnectedToECU)
			{
				LiveDataPIDModel._PIDCollection.Add(cpid);
			}
			if (this.Model.View == PIDEditorListModel.ViewByType.All || this.Model.View == PIDEditorListModel.ViewByType.LastCarSupported || this.Model.View == PIDEditorListModel.ViewByType.UserPids)
			{
				await this.Model.UpdateBaseList(false);
				try
				{
					this.lv.ScrollTo(cpid, 0, false);
				}
				catch
				{
				}
			}
		}

		// Token: 0x06001B59 RID: 7001 RVA: 0x001329EC File Offset: 0x00130BEC
		private async void DeleteAllCustomPidsQuestion()
		{
			TaskAwaiter<bool> taskAwaiter = base.DisplayAlert(Translate.GetString("ios_PidListDeleteAllCustomTitle"), Translate.GetString("ios_PidListDeleteAllCustomText"), Translate.GetString("ios_Yes"), Translate.GetString("ios_No")).GetAwaiter();
			if (!taskAwaiter.IsCompleted)
			{
				await taskAwaiter;
				TaskAwaiter<bool> taskAwaiter2;
				taskAwaiter = taskAwaiter2;
				taskAwaiter2 = default(TaskAwaiter<bool>);
			}
			if (taskAwaiter.GetResult())
			{
				if (App.OBDReader.CurrentStatus != OBDDataReaderStatus.Disconnected)
				{
					foreach (CustomPID customPID in CustomPIDViewModel.CurrentCustom.PidCollection.ToArray<CustomPID>())
					{
						try
						{
							LiveDataPIDModel._PIDCollection.Remove(customPID);
						}
						catch
						{
						}
					}
				}
				CustomPIDViewModel.CurrentCustom.PidCollection.Clear();
				CustomPIDViewModel.CurrentCustom.Save();
				this.Model.UpdateBaseList(false);
			}
		}

		// Token: 0x06001B5A RID: 7002 RVA: 0x00132A23 File Offset: 0x00130C23
		private void SearchBar_TextChanged(object sender, TextChangedEventArgs e)
		{
			this.Model.Filter = e.NewTextValue;
		}

		// Token: 0x17000FA5 RID: 4005
		// (get) Token: 0x06001B5B RID: 7003 RVA: 0x00132A36 File Offset: 0x00130C36
		// (set) Token: 0x06001B5C RID: 7004 RVA: 0x00132A3E File Offset: 0x00130C3E
		internal PIDEditorListModel Model
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

		// Token: 0x06001B5D RID: 7005 RVA: 0x00132A48 File Offset: 0x00130C48
		public ObservableCollection<IPID> LoadList()
		{
			List<IPID> list = new List<IPID>();
			list.AddRange(App.OBDReader.CurrentCarData.LiveDataPIDs.Where((PID x) => x is IPIDFloatValue || x is IPIDWithStringValue || x is PID0103_FuelSystemStatus || x is PID_Status));
			list.AddRange(CustomPIDViewModel.CurrentProfile.PidCollection);
			list.AddRange(CustomPIDViewModel.DisabledProfile.PidCollection);
			list.AddRange(CustomPIDViewModel.CurrentCustom.PidCollection);
			return new ObservableCollection<IPID>(list);
		}

		// Token: 0x06001B5E RID: 7006 RVA: 0x00132AC8 File Offset: 0x00130CC8
		private void Lv_ItemSelected(object sender, SelectedItemChangedEventArgs e)
		{
			if (e.SelectedItem == null)
			{
				return;
			}
			IPID ipid = e.SelectedItem as IPID;
			this.lv.SelectedItem = null;
			if (ipid is CustomPID && CustomPIDViewModel.CurrentCustom.PidCollection.Contains(ipid))
			{
				Page page = new CustomPIDsEditorPage(ipid as CustomPID);
				base.Navigation.PushAsync(page);
				return;
			}
			Page page2 = null;
			if (App.UseLegacyUI)
			{
				if (PlatformHelper.IsiOS)
				{
					page2 = new SettingsPIDOverridePage(ipid);
				}
			}
			else
			{
				page2 = new SettingsPIDOverrideEditorPageV3(ipid);
			}
			base.Navigation.PushAsync(page2);
		}

		// Token: 0x06001B5F RID: 7007 RVA: 0x000D7C27 File Offset: 0x000D5E27
		private void btnManageProfilePids_Clicked(object sender, EventArgs e)
		{
			base.Navigation.PushAsync(new SettingsDisabledPids());
		}

		// Token: 0x06001B60 RID: 7008 RVA: 0x00132B5C File Offset: 0x00130D5C
		private async void btnExportAsFile_Clicked(object sender, EventArgs e)
		{
			string cacheDirectory = FileSystem.CacheDirectory;
			string text = "custompids.csp";
			string text2 = Path.Combine(cacheDirectory, text);
			string text3 = JsonConvert.SerializeObject(CustomPIDViewModel.CurrentCustom.PidCollection);
			try
			{
				using (StreamWriter streamWriter = new StreamWriter(text2, false, Encoding.UTF8))
				{
					streamWriter.WriteLine(text3);
					streamWriter.Flush();
				}
			}
			catch (Exception)
			{
			}
			try
			{
				await Share.RequestAsync(new ShareFileRequest
				{
					Title = "Custom PIDs",
					File = new ShareFile(text2)
				});
			}
			catch (Exception)
			{
			}
		}

		// Token: 0x06001B61 RID: 7009 RVA: 0x00132B8C File Offset: 0x00130D8C
		private async void btnImportFromFile_Clicked(object sender, EventArgs e)
		{
			try
			{
				FileResult fileResult = await FilePicker.PickAsync(null);
				if (fileResult != null)
				{
					if (fileResult.FileName.EndsWith("csp", StringComparison.OrdinalIgnoreCase))
					{
						using (StreamReader sr = new StreamReader(fileResult.FullPath, Encoding.UTF8))
						{
							List<CustomPID> tempList = JsonConvert.DeserializeObject<List<CustomPID>>(sr.ReadToEnd());
							using (List<CustomPID>.Enumerator enumerator = tempList.GetEnumerator())
							{
								while (enumerator.MoveNext())
								{
									CustomPID pid = enumerator.Current;
									if (pid.Id <= 1000000 || pid.Id >= 10000000)
									{
										pid.Id = -1000;
									}
									CustomPID customPID = CustomPIDViewModel.CurrentCustom.PidCollection.FirstOrDefault((CustomPID x) => x.Id == pid.Id);
									if (customPID != null)
									{
										CustomPIDViewModel.CurrentCustom.PidCollection.Remove(customPID);
									}
									CustomPIDViewModel.CurrentCustom.PidCollection.Add(pid);
									if (App.OBDReader.CurrentStatus == OBDDataReaderStatus.ConnectedToECU)
									{
										LiveDataPIDModel._PIDCollection.Add(pid);
									}
								}
							}
							CustomPIDViewModel.CurrentCustom.Save();
							if (this.Model.View == PIDEditorListModel.ViewByType.All || this.Model.View == PIDEditorListModel.ViewByType.LastCarSupported || this.Model.View == PIDEditorListModel.ViewByType.UserPids)
							{
								await this.Model.UpdateBaseList(false);
								try
								{
									this.lv.ScrollTo(tempList.LastOrDefault<CustomPID>(), 0, false);
								}
								catch
								{
								}
							}
							tempList = null;
						}
						StreamReader sr = null;
					}
					else if (fileResult.FileName.EndsWith("csv", StringComparison.OrdinalIgnoreCase))
					{
						using (FileStream fileStream = File.OpenRead(fileResult.FullPath))
						{
							foreach (CustomPID customPID2 in CSVLoader.LoadFromCSV(fileStream))
							{
								CustomPIDViewModel.CurrentCustom.PidCollection.Add(customPID2);
							}
						}
						CustomPIDViewModel.CurrentCustom.Save();
						if (this.Model.View == PIDEditorListModel.ViewByType.All || this.Model.View == PIDEditorListModel.ViewByType.LastCarSupported || this.Model.View == PIDEditorListModel.ViewByType.UserPids)
						{
							await this.Model.UpdateBaseList(false);
						}
					}
				}
			}
			catch (Exception)
			{
			}
		}

		// Token: 0x06001B62 RID: 7010 RVA: 0x00132BC4 File Offset: 0x00130DC4
		private async void btnDel_Clicked(object sender, EventArgs e)
		{
			this.lv.IsEnabled = false;
			CustomPID customPID = (sender as MenuItem).BindingContext as CustomPID;
			try
			{
				LiveDataPIDModel._PIDCollection.Remove(customPID);
			}
			catch
			{
			}
			CustomPIDViewModel.CurrentCustom.PidCollection.Remove(customPID);
			CustomPIDViewModel.CurrentCustom.Save();
			await this.Model.UpdateBaseList(false);
			this.lv.IsEnabled = true;
		}

		// Token: 0x06001B63 RID: 7011 RVA: 0x00132C04 File Offset: 0x00130E04
		private void btnSelectProfile_Clicked(object sender, EventArgs e)
		{
			if (App.OBDReader.CurrentStatus == OBDDataReaderStatus.Disconnected)
			{
				ProfileSelectorV2Page profileSelectorV2Page = new ProfileSelectorV2Page();
				base.Navigation.PushAsync(profileSelectorV2Page);
				return;
			}
			base.DisplayAlert(Translate.GetString("ios_PleaseDisconnectFirst_Title"), Translate.GetString("ios_PleaseDisconnectFirst_Text"), "OK");
		}

		// Token: 0x06001B64 RID: 7012 RVA: 0x00132C54 File Offset: 0x00130E54
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private void InitializeComponent()
		{
			if (ResourceLoader.IsEnabled && ResourceLoader.CanProvideContentFor(new ResourceLoader.ResourceLoadingQuery
			{
				AssemblyName = typeof(SettingsPIDOverrideListPage).GetTypeInfo().Assembly.GetName(),
				ResourcePath = "Settings/SettingsPIDOverrideListPage.xaml",
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
			VisualDiagnostics.RegisterSourceInfo(translate = new Translate(), new Uri("Settings\\SettingsPIDOverrideListPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 11, 5);
			DynamicResourceExtension dynamicResourceExtension;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension = new DynamicResourceExtension(), new Uri("Settings\\SettingsPIDOverrideListPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 13, 5);
			UnitsToStringConverter unitsToStringConverter;
			VisualDiagnostics.RegisterSourceInfo(unitsToStringConverter = new UnitsToStringConverter(), new Uri("Settings\\SettingsPIDOverrideListPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 16, 14);
			BoolToNegativeConverter boolToNegativeConverter;
			VisualDiagnostics.RegisterSourceInfo(boolToNegativeConverter = new BoolToNegativeConverter(), new Uri("Settings\\SettingsPIDOverrideListPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 17, 14);
			SkipCyclesIntToPriorityStringConverter skipCyclesIntToPriorityStringConverter;
			VisualDiagnostics.RegisterSourceInfo(skipCyclesIntToPriorityStringConverter = new SkipCyclesIntToPriorityStringConverter(), new Uri("Settings\\SettingsPIDOverrideListPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 18, 14);
			EnumToIntConverter enumToIntConverter;
			VisualDiagnostics.RegisterSourceInfo(enumToIntConverter = new EnumToIntConverter(), new Uri("Settings\\SettingsPIDOverrideListPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 19, 14);
			EnumValueToTrueConverter enumValueToTrueConverter;
			VisualDiagnostics.RegisterSourceInfo(enumValueToTrueConverter = new EnumValueToTrueConverter(), new Uri("Settings\\SettingsPIDOverrideListPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 20, 14);
			EnumValueToFalseConverter enumValueToFalseConverter;
			VisualDiagnostics.RegisterSourceInfo(enumValueToFalseConverter = new EnumValueToFalseConverter(), new Uri("Settings\\SettingsPIDOverrideListPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 21, 14);
			EmptyStringToFalseConverter emptyStringToFalseConverter;
			VisualDiagnostics.RegisterSourceInfo(emptyStringToFalseConverter = new EmptyStringToFalseConverter(), new Uri("Settings\\SettingsPIDOverrideListPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 22, 14);
			PidToUnitsTitleConverter pidToUnitsTitleConverter;
			VisualDiagnostics.RegisterSourceInfo(pidToUnitsTitleConverter = new PidToUnitsTitleConverter(), new Uri("Settings\\SettingsPIDOverrideListPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 23, 14);
			DataTemplate dataTemplate;
			VisualDiagnostics.RegisterSourceInfo(dataTemplate = new DataTemplate(), new Uri("Settings\\SettingsPIDOverrideListPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 25, 14);
			DataTemplate dataTemplate2;
			VisualDiagnostics.RegisterSourceInfo(dataTemplate2 = new DataTemplate(), new Uri("Settings\\SettingsPIDOverrideListPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 129, 14);
			StaticResourceExtension staticResourceExtension;
			VisualDiagnostics.RegisterSourceInfo(staticResourceExtension = new StaticResourceExtension(), new Uri("Settings\\SettingsPIDOverrideListPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 166, 17);
			StaticResourceExtension staticResourceExtension2;
			VisualDiagnostics.RegisterSourceInfo(staticResourceExtension2 = new StaticResourceExtension(), new Uri("Settings\\SettingsPIDOverrideListPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 167, 17);
			PIDItemDataTemplateSelector piditemDataTemplateSelector;
			VisualDiagnostics.RegisterSourceInfo(piditemDataTemplateSelector = new PIDItemDataTemplateSelector(), new Uri("Settings\\SettingsPIDOverrideListPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 164, 14);
			ResourceDictionary resourceDictionary;
			VisualDiagnostics.RegisterSourceInfo(resourceDictionary = new ResourceDictionary(), new Uri("Settings\\SettingsPIDOverrideListPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 15, 10);
			RowDefinition rowDefinition;
			VisualDiagnostics.RegisterSourceInfo(rowDefinition = new RowDefinition(), new Uri("Settings\\SettingsPIDOverrideListPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 180, 18);
			RowDefinition rowDefinition2;
			VisualDiagnostics.RegisterSourceInfo(rowDefinition2 = new RowDefinition(), new Uri("Settings\\SettingsPIDOverrideListPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 181, 18);
			RowDefinition rowDefinition3;
			VisualDiagnostics.RegisterSourceInfo(rowDefinition3 = new RowDefinition(), new Uri("Settings\\SettingsPIDOverrideListPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 182, 18);
			ColumnDefinition columnDefinition;
			VisualDiagnostics.RegisterSourceInfo(columnDefinition = new ColumnDefinition(), new Uri("Settings\\SettingsPIDOverrideListPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 185, 18);
			ColumnDefinition columnDefinition2;
			VisualDiagnostics.RegisterSourceInfo(columnDefinition2 = new ColumnDefinition(), new Uri("Settings\\SettingsPIDOverrideListPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 186, 18);
			DynamicResourceExtension dynamicResourceExtension2;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension2 = new DynamicResourceExtension(), new Uri("Settings\\SettingsPIDOverrideListPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 192, 17);
			DynamicResourceExtension dynamicResourceExtension3;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension3 = new DynamicResourceExtension(), new Uri("Settings\\SettingsPIDOverrideListPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 193, 17);
			DynamicResourceExtension dynamicResourceExtension4;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension4 = new DynamicResourceExtension(), new Uri("Settings\\SettingsPIDOverrideListPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 196, 17);
			Entry entry;
			VisualDiagnostics.RegisterSourceInfo(entry = new Entry(), new Uri("Settings\\SettingsPIDOverrideListPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 188, 14);
			DynamicResourceExtension dynamicResourceExtension5;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension5 = new DynamicResourceExtension(), new Uri("Settings\\SettingsPIDOverrideListPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 201, 17);
			DynamicResourceExtension dynamicResourceExtension6;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension6 = new DynamicResourceExtension(), new Uri("Settings\\SettingsPIDOverrideListPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 202, 17);
			StaticResourceExtension staticResourceExtension3;
			VisualDiagnostics.RegisterSourceInfo(staticResourceExtension3 = new StaticResourceExtension(), new Uri("Settings\\SettingsPIDOverrideListPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 203, 17);
			BindingExtension bindingExtension;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension = new BindingExtension(), new Uri("Settings\\SettingsPIDOverrideListPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 203, 17);
			Picker picker;
			VisualDiagnostics.RegisterSourceInfo(picker = new Picker(), new Uri("Settings\\SettingsPIDOverrideListPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 197, 14);
			StaticResourceExtension staticResourceExtension4;
			VisualDiagnostics.RegisterSourceInfo(staticResourceExtension4 = new StaticResourceExtension(), new Uri("Settings\\SettingsPIDOverrideListPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 212, 17);
			BindingExtension bindingExtension2;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension2 = new BindingExtension(), new Uri("Settings\\SettingsPIDOverrideListPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 213, 17);
			StaticResourceExtension staticResourceExtension5;
			VisualDiagnostics.RegisterSourceInfo(staticResourceExtension5 = new StaticResourceExtension(), new Uri("Settings\\SettingsPIDOverrideListPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 222, 29);
			BindingExtension bindingExtension3;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension3 = new BindingExtension(), new Uri("Settings\\SettingsPIDOverrideListPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 222, 29);
			BindingExtension bindingExtension4;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension4 = new BindingExtension(), new Uri("Settings\\SettingsPIDOverrideListPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 223, 29);
			Label label;
			VisualDiagnostics.RegisterSourceInfo(label = new Label(), new Uri("Settings\\SettingsPIDOverrideListPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 218, 26);
			DynamicResourceExtension dynamicResourceExtension7;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension7 = new DynamicResourceExtension(), new Uri("Settings\\SettingsPIDOverrideListPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 227, 29);
			BindingExtension bindingExtension5;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension5 = new BindingExtension(), new Uri("Settings\\SettingsPIDOverrideListPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 229, 29);
			Translate translate2;
			VisualDiagnostics.RegisterSourceInfo(translate2 = new Translate(), new Uri("Settings\\SettingsPIDOverrideListPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 230, 29);
			Label label2;
			VisualDiagnostics.RegisterSourceInfo(label2 = new Label(), new Uri("Settings\\SettingsPIDOverrideListPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 226, 26);
			BindingExtension bindingExtension6;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension6 = new BindingExtension(), new Uri("Settings\\SettingsPIDOverrideListPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 231, 38);
			SharedSettings sharedSettings;
			VisualDiagnostics.RegisterSourceInfo(sharedSettings = SharedSettings.Current, new Uri("Settings\\SettingsPIDOverrideListPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 234, 33);
			DynamicResourceExtension dynamicResourceExtension8;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension8 = new DynamicResourceExtension(), new Uri("Settings\\SettingsPIDOverrideListPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 236, 33);
			BindingExtension bindingExtension7;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension7 = new BindingExtension(), new Uri("Settings\\SettingsPIDOverrideListPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 238, 33);
			Label label3;
			VisualDiagnostics.RegisterSourceInfo(label3 = new Label(), new Uri("Settings\\SettingsPIDOverrideListPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 232, 30);
			StackLayout stackLayout;
			VisualDiagnostics.RegisterSourceInfo(stackLayout = new StackLayout(), new Uri("Settings\\SettingsPIDOverrideListPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 231, 26);
			BindingExtension bindingExtension8;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension8 = new BindingExtension(), new Uri("Settings\\SettingsPIDOverrideListPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 243, 29);
			Translate translate3;
			VisualDiagnostics.RegisterSourceInfo(translate3 = new Translate(), new Uri("Settings\\SettingsPIDOverrideListPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 244, 29);
			Button button;
			VisualDiagnostics.RegisterSourceInfo(button = new Button(), new Uri("Settings\\SettingsPIDOverrideListPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 240, 26);
			StackLayout stackLayout2;
			VisualDiagnostics.RegisterSourceInfo(stackLayout2 = new StackLayout(), new Uri("Settings\\SettingsPIDOverrideListPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 215, 22);
			ListView listView;
			VisualDiagnostics.RegisterSourceInfo(listView = new ListView(), new Uri("Settings\\SettingsPIDOverrideListPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 204, 14);
			ActivityFrame activityFrame;
			VisualDiagnostics.RegisterSourceInfo(activityFrame = new ActivityFrame(), new Uri("Settings\\SettingsPIDOverrideListPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 257, 14);
			Translate translate4;
			VisualDiagnostics.RegisterSourceInfo(translate4 = new Translate(), new Uri("Settings\\SettingsPIDOverrideListPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 272, 17);
			Button button2;
			VisualDiagnostics.RegisterSourceInfo(button2 = new Button(), new Uri("Settings\\SettingsPIDOverrideListPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 266, 14);
			Translate translate5;
			VisualDiagnostics.RegisterSourceInfo(translate5 = new Translate(), new Uri("Settings\\SettingsPIDOverrideListPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 288, 17);
			Button button3;
			VisualDiagnostics.RegisterSourceInfo(button3 = new Button(), new Uri("Settings\\SettingsPIDOverrideListPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 282, 14);
			Grid grid;
			VisualDiagnostics.RegisterSourceInfo(grid = new Grid(), new Uri("Settings\\SettingsPIDOverrideListPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 178, 10);
			VisualDiagnostics.RegisterSourceInfo(this, new Uri("Settings\\SettingsPIDOverrideListPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 2, 2);
			NameScope nameScope = NameScope.GetNameScope(this) ?? new NameScope();
			NameScope.SetNameScope(this, nameScope);
			nameScope.RegisterName("searchBar", entry);
			if (entry.StyleId == null)
			{
				entry.StyleId = "searchBar";
			}
			nameScope.RegisterName("pickerBaseList", picker);
			if (picker.StyleId == null)
			{
				picker.StyleId = "pickerBaseList";
			}
			nameScope.RegisterName("lv", listView);
			if (listView.StyleId == null)
			{
				listView.StyleId = "lv";
			}
			nameScope.RegisterName("labelSelectedProfile", label3);
			if (label3.StyleId == null)
			{
				label3.StyleId = "labelSelectedProfile";
			}
			nameScope.RegisterName("activityFrame", activityFrame);
			if (activityFrame.StyleId == null)
			{
				activityFrame.StyleId = "activityFrame";
			}
			nameScope.RegisterName("btnImportFromFile", button2);
			if (button2.StyleId == null)
			{
				button2.StyleId = "btnImportFromFile";
			}
			nameScope.RegisterName("btnExportAsFile", button3);
			if (button3.StyleId == null)
			{
				button3.StyleId = "btnExportAsFile";
			}
			this.searchBar = entry;
			this.pickerBaseList = picker;
			this.lv = listView;
			this.labelSelectedProfile = label3;
			this.activityFrame = activityFrame;
			this.btnImportFromFile = button2;
			this.btnExportAsFile = button3;
			this.Resources = resourceDictionary;
			resourceDictionary.Add("UnitsToStringConverter", unitsToStringConverter);
			resourceDictionary.Add("BoolToNegativeConverter", boolToNegativeConverter);
			resourceDictionary.Add("SkipCyclesIntToPriorityStringConverter", skipCyclesIntToPriorityStringConverter);
			resourceDictionary.Add("EnumToIntConverter", enumToIntConverter);
			resourceDictionary.Add("EnumValueToTrueConverter", enumValueToTrueConverter);
			resourceDictionary.Add("EnumValueToFalseConverter", enumValueToFalseConverter);
			resourceDictionary.Add("EmptyStringToFalseConverter", emptyStringToFalseConverter);
			resourceDictionary.Add("PidToUnitsTitleConverter", pidToUnitsTitleConverter);
			IDataTemplate dataTemplate3 = dataTemplate;
			SettingsPIDOverrideListPage.<InitializeComponent>_anonXamlCDataTemplate_95 <InitializeComponent>_anonXamlCDataTemplate_ = new SettingsPIDOverrideListPage.<InitializeComponent>_anonXamlCDataTemplate_95();
			object[] array = new object[0 + 3];
			array[0] = dataTemplate;
			array[1] = resourceDictionary;
			array[2] = this;
			<InitializeComponent>_anonXamlCDataTemplate_.parentValues = array;
			<InitializeComponent>_anonXamlCDataTemplate_.root = this;
			dataTemplate3.LoadTemplate = new Func<object>(<InitializeComponent>_anonXamlCDataTemplate_.LoadDataTemplate);
			resourceDictionary.Add("editablePID", dataTemplate);
			IDataTemplate dataTemplate4 = dataTemplate2;
			SettingsPIDOverrideListPage.<InitializeComponent>_anonXamlCDataTemplate_96 <InitializeComponent>_anonXamlCDataTemplate_2 = new SettingsPIDOverrideListPage.<InitializeComponent>_anonXamlCDataTemplate_96();
			object[] array2 = new object[0 + 3];
			array2[0] = dataTemplate2;
			array2[1] = resourceDictionary;
			array2[2] = this;
			<InitializeComponent>_anonXamlCDataTemplate_2.parentValues = array2;
			<InitializeComponent>_anonXamlCDataTemplate_2.root = this;
			dataTemplate4.LoadTemplate = new Func<object>(<InitializeComponent>_anonXamlCDataTemplate_2.LoadDataTemplate);
			resourceDictionary.Add("nonEditablePID", dataTemplate2);
			staticResourceExtension.Key = "editablePID";
			IMarkupExtension markupExtension = staticResourceExtension;
			XamlServiceProvider xamlServiceProvider = new XamlServiceProvider();
			Type typeFromHandle = typeof(IProvideValueTarget);
			object[] array3 = new object[0 + 3];
			array3[0] = piditemDataTemplateSelector;
			array3[1] = resourceDictionary;
			array3[2] = this;
			object obj;
			xamlServiceProvider.Add(typeFromHandle, obj = new SimpleValueTargetProvider(array3, typeof(PIDItemDataTemplateSelector).GetRuntimeProperty("EditableCustomPID"), nameScope));
			xamlServiceProvider.Add(typeof(IReferenceProvider), obj);
			Type typeFromHandle2 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver = new XmlNamespaceResolver();
			xmlNamespaceResolver.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver.Add("pids", "clr-namespace:CarScannerXamarinForms.OBD2.PIDS");
			xmlNamespaceResolver.Add("settings", "clr-namespace:CarScannerXamarinForms.Settings");
			xamlServiceProvider.Add(typeFromHandle2, new XamlTypeResolver(xmlNamespaceResolver, typeof(SettingsPIDOverrideListPage).GetTypeInfo().Assembly));
			xamlServiceProvider.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(166, 17)));
			object obj2 = markupExtension.ProvideValue(xamlServiceProvider);
			piditemDataTemplateSelector.EditableCustomPID = obj2;
			staticResourceExtension2.Key = "nonEditablePID";
			IMarkupExtension markupExtension2 = staticResourceExtension2;
			XamlServiceProvider xamlServiceProvider2 = new XamlServiceProvider();
			Type typeFromHandle3 = typeof(IProvideValueTarget);
			object[] array4 = new object[0 + 3];
			array4[0] = piditemDataTemplateSelector;
			array4[1] = resourceDictionary;
			array4[2] = this;
			object obj3;
			xamlServiceProvider2.Add(typeFromHandle3, obj3 = new SimpleValueTargetProvider(array4, typeof(PIDItemDataTemplateSelector).GetRuntimeProperty("NonEditablePID"), nameScope));
			xamlServiceProvider2.Add(typeof(IReferenceProvider), obj3);
			Type typeFromHandle4 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver2 = new XmlNamespaceResolver();
			xmlNamespaceResolver2.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver2.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver2.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver2.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver2.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver2.Add("pids", "clr-namespace:CarScannerXamarinForms.OBD2.PIDS");
			xmlNamespaceResolver2.Add("settings", "clr-namespace:CarScannerXamarinForms.Settings");
			xamlServiceProvider2.Add(typeFromHandle4, new XamlTypeResolver(xmlNamespaceResolver2, typeof(SettingsPIDOverrideListPage).GetTypeInfo().Assembly));
			xamlServiceProvider2.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(167, 17)));
			object obj4 = markupExtension2.ProvideValue(xamlServiceProvider2);
			piditemDataTemplateSelector.NonEditablePID = obj4;
			resourceDictionary.Add("PIDItemDataTemplateSelector", piditemDataTemplateSelector);
			translate.Text = "ios_Sensors";
			IMarkupExtension markupExtension3 = translate;
			XamlServiceProvider xamlServiceProvider3 = new XamlServiceProvider();
			Type typeFromHandle5 = typeof(IProvideValueTarget);
			object[] array5 = new object[0 + 1];
			array5[0] = this;
			object obj5;
			xamlServiceProvider3.Add(typeFromHandle5, obj5 = new SimpleValueTargetProvider(array5, Page.TitleProperty, nameScope));
			xamlServiceProvider3.Add(typeof(IReferenceProvider), obj5);
			Type typeFromHandle6 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver3 = new XmlNamespaceResolver();
			xmlNamespaceResolver3.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver3.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver3.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver3.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver3.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver3.Add("pids", "clr-namespace:CarScannerXamarinForms.OBD2.PIDS");
			xmlNamespaceResolver3.Add("settings", "clr-namespace:CarScannerXamarinForms.Settings");
			xamlServiceProvider3.Add(typeFromHandle6, new XamlTypeResolver(xmlNamespaceResolver3, typeof(SettingsPIDOverrideListPage).GetTypeInfo().Assembly));
			xamlServiceProvider3.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(11, 5)));
			object obj6 = markupExtension3.ProvideValue(xamlServiceProvider3);
			this.Title = obj6;
			this.SetValue(Page.UseSafeAreaProperty, true);
			dynamicResourceExtension.Key = "SettingsBackground";
			IMarkupExtension<DynamicResource> markupExtension4 = dynamicResourceExtension;
			XamlServiceProvider xamlServiceProvider4 = new XamlServiceProvider();
			Type typeFromHandle7 = typeof(IProvideValueTarget);
			object[] array6 = new object[0 + 1];
			array6[0] = this;
			object obj7;
			xamlServiceProvider4.Add(typeFromHandle7, obj7 = new SimpleValueTargetProvider(array6, VisualElement.BackgroundColorProperty, nameScope));
			xamlServiceProvider4.Add(typeof(IReferenceProvider), obj7);
			Type typeFromHandle8 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver4 = new XmlNamespaceResolver();
			xmlNamespaceResolver4.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver4.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver4.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver4.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver4.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver4.Add("pids", "clr-namespace:CarScannerXamarinForms.OBD2.PIDS");
			xmlNamespaceResolver4.Add("settings", "clr-namespace:CarScannerXamarinForms.Settings");
			xamlServiceProvider4.Add(typeFromHandle8, new XamlTypeResolver(xmlNamespaceResolver4, typeof(SettingsPIDOverrideListPage).GetTypeInfo().Assembly));
			xamlServiceProvider4.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(13, 5)));
			DynamicResource dynamicResource = markupExtension4.ProvideValue(xamlServiceProvider4);
			this.SetDynamicResource(VisualElement.BackgroundColorProperty, dynamicResource.Key);
			this.Resources = resourceDictionary;
			rowDefinition.SetValue(RowDefinition.HeightProperty, new GridLengthTypeConverter().ConvertFromInvariantString("auto"));
			grid.GetValue(Grid.RowDefinitionsProperty).Add(rowDefinition);
			rowDefinition2.SetValue(RowDefinition.HeightProperty, new GridLengthTypeConverter().ConvertFromInvariantString("*"));
			grid.GetValue(Grid.RowDefinitionsProperty).Add(rowDefinition2);
			rowDefinition3.SetValue(RowDefinition.HeightProperty, new GridLengthTypeConverter().ConvertFromInvariantString("auto"));
			grid.GetValue(Grid.RowDefinitionsProperty).Add(rowDefinition3);
			columnDefinition.SetValue(ColumnDefinition.WidthProperty, new GridLengthTypeConverter().ConvertFromInvariantString("*"));
			grid.GetValue(Grid.ColumnDefinitionsProperty).Add(columnDefinition);
			columnDefinition2.SetValue(ColumnDefinition.WidthProperty, new GridLengthTypeConverter().ConvertFromInvariantString("*"));
			grid.GetValue(Grid.ColumnDefinitionsProperty).Add(columnDefinition2);
			entry.SetValue(Grid.RowProperty, 0);
			entry.SetValue(Grid.ColumnProperty, 0);
			dynamicResourceExtension2.Key = "SettingsBackground";
			IMarkupExtension<DynamicResource> markupExtension5 = dynamicResourceExtension2;
			XamlServiceProvider xamlServiceProvider5 = new XamlServiceProvider();
			Type typeFromHandle9 = typeof(IProvideValueTarget);
			object[] array7 = new object[0 + 3];
			array7[0] = entry;
			array7[1] = grid;
			array7[2] = this;
			object obj8;
			xamlServiceProvider5.Add(typeFromHandle9, obj8 = new SimpleValueTargetProvider(array7, VisualElement.BackgroundColorProperty, nameScope));
			xamlServiceProvider5.Add(typeof(IReferenceProvider), obj8);
			Type typeFromHandle10 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver5 = new XmlNamespaceResolver();
			xmlNamespaceResolver5.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver5.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver5.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver5.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver5.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver5.Add("pids", "clr-namespace:CarScannerXamarinForms.OBD2.PIDS");
			xmlNamespaceResolver5.Add("settings", "clr-namespace:CarScannerXamarinForms.Settings");
			xamlServiceProvider5.Add(typeFromHandle10, new XamlTypeResolver(xmlNamespaceResolver5, typeof(SettingsPIDOverrideListPage).GetTypeInfo().Assembly));
			xamlServiceProvider5.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(192, 17)));
			DynamicResource dynamicResource2 = markupExtension5.ProvideValue(xamlServiceProvider5);
			entry.SetDynamicResource(VisualElement.BackgroundColorProperty, dynamicResource2.Key);
			dynamicResourceExtension3.Key = "BaseFontSize+";
			IMarkupExtension<DynamicResource> markupExtension6 = dynamicResourceExtension3;
			XamlServiceProvider xamlServiceProvider6 = new XamlServiceProvider();
			Type typeFromHandle11 = typeof(IProvideValueTarget);
			object[] array8 = new object[0 + 3];
			array8[0] = entry;
			array8[1] = grid;
			array8[2] = this;
			object obj9;
			xamlServiceProvider6.Add(typeFromHandle11, obj9 = new SimpleValueTargetProvider(array8, Entry.FontSizeProperty, nameScope));
			xamlServiceProvider6.Add(typeof(IReferenceProvider), obj9);
			Type typeFromHandle12 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver6 = new XmlNamespaceResolver();
			xmlNamespaceResolver6.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver6.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver6.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver6.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver6.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver6.Add("pids", "clr-namespace:CarScannerXamarinForms.OBD2.PIDS");
			xmlNamespaceResolver6.Add("settings", "clr-namespace:CarScannerXamarinForms.Settings");
			xamlServiceProvider6.Add(typeFromHandle12, new XamlTypeResolver(xmlNamespaceResolver6, typeof(SettingsPIDOverrideListPage).GetTypeInfo().Assembly));
			xamlServiceProvider6.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(193, 17)));
			DynamicResource dynamicResource3 = markupExtension6.ProvideValue(xamlServiceProvider6);
			entry.SetDynamicResource(Entry.FontSizeProperty, dynamicResource3.Key);
			entry.SetValue(Entry.PlaceholderProperty, "\ud83d\udd0e");
			entry.TextChanged += this.SearchBar_TextChanged;
			dynamicResourceExtension4.Key = "SettingsCellValueTextColor";
			IMarkupExtension<DynamicResource> markupExtension7 = dynamicResourceExtension4;
			XamlServiceProvider xamlServiceProvider7 = new XamlServiceProvider();
			Type typeFromHandle13 = typeof(IProvideValueTarget);
			object[] array9 = new object[0 + 3];
			array9[0] = entry;
			array9[1] = grid;
			array9[2] = this;
			object obj10;
			xamlServiceProvider7.Add(typeFromHandle13, obj10 = new SimpleValueTargetProvider(array9, Entry.TextColorProperty, nameScope));
			xamlServiceProvider7.Add(typeof(IReferenceProvider), obj10);
			Type typeFromHandle14 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver7 = new XmlNamespaceResolver();
			xmlNamespaceResolver7.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver7.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver7.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver7.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver7.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver7.Add("pids", "clr-namespace:CarScannerXamarinForms.OBD2.PIDS");
			xmlNamespaceResolver7.Add("settings", "clr-namespace:CarScannerXamarinForms.Settings");
			xamlServiceProvider7.Add(typeFromHandle14, new XamlTypeResolver(xmlNamespaceResolver7, typeof(SettingsPIDOverrideListPage).GetTypeInfo().Assembly));
			xamlServiceProvider7.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(196, 17)));
			DynamicResource dynamicResource4 = markupExtension7.ProvideValue(xamlServiceProvider7);
			entry.SetDynamicResource(Entry.TextColorProperty, dynamicResource4.Key);
			grid.Children.Add(entry);
			picker.SetValue(Grid.RowProperty, 0);
			picker.SetValue(Grid.ColumnProperty, 1);
			dynamicResourceExtension5.Key = "SettingsBackground";
			IMarkupExtension<DynamicResource> markupExtension8 = dynamicResourceExtension5;
			XamlServiceProvider xamlServiceProvider8 = new XamlServiceProvider();
			Type typeFromHandle15 = typeof(IProvideValueTarget);
			object[] array10 = new object[0 + 3];
			array10[0] = picker;
			array10[1] = grid;
			array10[2] = this;
			object obj11;
			xamlServiceProvider8.Add(typeFromHandle15, obj11 = new SimpleValueTargetProvider(array10, VisualElement.BackgroundColorProperty, nameScope));
			xamlServiceProvider8.Add(typeof(IReferenceProvider), obj11);
			Type typeFromHandle16 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver8 = new XmlNamespaceResolver();
			xmlNamespaceResolver8.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver8.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver8.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver8.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver8.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver8.Add("pids", "clr-namespace:CarScannerXamarinForms.OBD2.PIDS");
			xmlNamespaceResolver8.Add("settings", "clr-namespace:CarScannerXamarinForms.Settings");
			xamlServiceProvider8.Add(typeFromHandle16, new XamlTypeResolver(xmlNamespaceResolver8, typeof(SettingsPIDOverrideListPage).GetTypeInfo().Assembly));
			xamlServiceProvider8.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(201, 17)));
			DynamicResource dynamicResource5 = markupExtension8.ProvideValue(xamlServiceProvider8);
			picker.SetDynamicResource(VisualElement.BackgroundColorProperty, dynamicResource5.Key);
			dynamicResourceExtension6.Key = "BaseFontSize+";
			IMarkupExtension<DynamicResource> markupExtension9 = dynamicResourceExtension6;
			XamlServiceProvider xamlServiceProvider9 = new XamlServiceProvider();
			Type typeFromHandle17 = typeof(IProvideValueTarget);
			object[] array11 = new object[0 + 3];
			array11[0] = picker;
			array11[1] = grid;
			array11[2] = this;
			object obj12;
			xamlServiceProvider9.Add(typeFromHandle17, obj12 = new SimpleValueTargetProvider(array11, Picker.FontSizeProperty, nameScope));
			xamlServiceProvider9.Add(typeof(IReferenceProvider), obj12);
			Type typeFromHandle18 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver9 = new XmlNamespaceResolver();
			xmlNamespaceResolver9.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver9.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver9.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver9.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver9.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver9.Add("pids", "clr-namespace:CarScannerXamarinForms.OBD2.PIDS");
			xmlNamespaceResolver9.Add("settings", "clr-namespace:CarScannerXamarinForms.Settings");
			xamlServiceProvider9.Add(typeFromHandle18, new XamlTypeResolver(xmlNamespaceResolver9, typeof(SettingsPIDOverrideListPage).GetTypeInfo().Assembly));
			xamlServiceProvider9.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(202, 17)));
			DynamicResource dynamicResource6 = markupExtension9.ProvideValue(xamlServiceProvider9);
			picker.SetDynamicResource(Picker.FontSizeProperty, dynamicResource6.Key);
			bindingExtension.Mode = 1;
			staticResourceExtension3.Key = "EnumToIntConverter";
			IMarkupExtension markupExtension10 = staticResourceExtension3;
			XamlServiceProvider xamlServiceProvider10 = new XamlServiceProvider();
			Type typeFromHandle19 = typeof(IProvideValueTarget);
			object[] array12 = new object[0 + 4];
			array12[0] = bindingExtension;
			array12[1] = picker;
			array12[2] = grid;
			array12[3] = this;
			object obj13;
			xamlServiceProvider10.Add(typeFromHandle19, obj13 = new SimpleValueTargetProvider(array12, typeof(BindingExtension).GetRuntimeProperty("Converter"), nameScope));
			xamlServiceProvider10.Add(typeof(IReferenceProvider), obj13);
			Type typeFromHandle20 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver10 = new XmlNamespaceResolver();
			xmlNamespaceResolver10.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver10.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver10.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver10.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver10.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver10.Add("pids", "clr-namespace:CarScannerXamarinForms.OBD2.PIDS");
			xmlNamespaceResolver10.Add("settings", "clr-namespace:CarScannerXamarinForms.Settings");
			xamlServiceProvider10.Add(typeFromHandle20, new XamlTypeResolver(xmlNamespaceResolver10, typeof(SettingsPIDOverrideListPage).GetTypeInfo().Assembly));
			xamlServiceProvider10.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(203, 17)));
			object obj14 = markupExtension10.ProvideValue(xamlServiceProvider10);
			bindingExtension.Converter = obj14;
			bindingExtension.Path = "View";
			BindingBase bindingBase = bindingExtension.ProvideValue(null);
			picker.SetBinding(Picker.SelectedIndexProperty, bindingBase);
			grid.Children.Add(picker);
			listView.SetValue(Grid.RowProperty, 1);
			listView.SetValue(Grid.ColumnProperty, 0);
			listView.SetValue(Grid.ColumnSpanProperty, 2);
			listView.SetValue(View.MarginProperty, new Thickness(5.0, 0.0, 5.0, 0.0));
			listView.SetValue(ListView.HasUnevenRowsProperty, true);
			listView.ItemSelected += this.Lv_ItemSelected;
			staticResourceExtension4.Key = "PIDItemDataTemplateSelector";
			IMarkupExtension markupExtension11 = staticResourceExtension4;
			XamlServiceProvider xamlServiceProvider11 = new XamlServiceProvider();
			Type typeFromHandle21 = typeof(IProvideValueTarget);
			object[] array13 = new object[0 + 3];
			array13[0] = listView;
			array13[1] = grid;
			array13[2] = this;
			object obj15;
			xamlServiceProvider11.Add(typeFromHandle21, obj15 = new SimpleValueTargetProvider(array13, ItemsView<Cell>.ItemTemplateProperty, nameScope));
			xamlServiceProvider11.Add(typeof(IReferenceProvider), obj15);
			Type typeFromHandle22 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver11 = new XmlNamespaceResolver();
			xmlNamespaceResolver11.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver11.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver11.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver11.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver11.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver11.Add("pids", "clr-namespace:CarScannerXamarinForms.OBD2.PIDS");
			xmlNamespaceResolver11.Add("settings", "clr-namespace:CarScannerXamarinForms.Settings");
			xamlServiceProvider11.Add(typeFromHandle22, new XamlTypeResolver(xmlNamespaceResolver11, typeof(SettingsPIDOverrideListPage).GetTypeInfo().Assembly));
			xamlServiceProvider11.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(212, 17)));
			object obj16 = markupExtension11.ProvideValue(xamlServiceProvider11);
			listView.ItemTemplate = obj16;
			bindingExtension2.Path = ".";
			BindingBase bindingBase2 = bindingExtension2.ProvideValue(null);
			listView.SetBinding(ItemsView<Cell>.ItemsSourceProperty, bindingBase2);
			stackLayout2.SetValue(StackLayout.OrientationProperty, 0);
			label.SetValue(Label.FontAttributesProperty, new FontAttributesConverter().ConvertFromInvariantString("Bold"));
			label.SetValue(View.HorizontalOptionsProperty, LayoutOptions.Center);
			label.SetValue(Label.HorizontalTextAlignmentProperty, new TextAlignmentConverter().ConvertFromInvariantString("Center"));
			bindingExtension3.Mode = 2;
			staticResourceExtension5.Key = "EmptyStringToFalseConverter";
			IMarkupExtension markupExtension12 = staticResourceExtension5;
			XamlServiceProvider xamlServiceProvider12 = new XamlServiceProvider();
			Type typeFromHandle23 = typeof(IProvideValueTarget);
			object[] array14 = new object[0 + 6];
			array14[0] = bindingExtension3;
			array14[1] = label;
			array14[2] = stackLayout2;
			array14[3] = listView;
			array14[4] = grid;
			array14[5] = this;
			object obj17;
			xamlServiceProvider12.Add(typeFromHandle23, obj17 = new SimpleValueTargetProvider(array14, typeof(BindingExtension).GetRuntimeProperty("Converter"), nameScope));
			xamlServiceProvider12.Add(typeof(IReferenceProvider), obj17);
			Type typeFromHandle24 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver12 = new XmlNamespaceResolver();
			xmlNamespaceResolver12.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver12.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver12.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver12.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver12.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver12.Add("pids", "clr-namespace:CarScannerXamarinForms.OBD2.PIDS");
			xmlNamespaceResolver12.Add("settings", "clr-namespace:CarScannerXamarinForms.Settings");
			xamlServiceProvider12.Add(typeFromHandle24, new XamlTypeResolver(xmlNamespaceResolver12, typeof(SettingsPIDOverrideListPage).GetTypeInfo().Assembly));
			xamlServiceProvider12.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(222, 29)));
			object obj18 = markupExtension12.ProvideValue(xamlServiceProvider12);
			bindingExtension3.Converter = obj18;
			bindingExtension3.Path = "EmptyListText";
			BindingBase bindingBase3 = bindingExtension3.ProvideValue(null);
			label.SetBinding(VisualElement.IsVisibleProperty, bindingBase3);
			bindingExtension4.Mode = 2;
			bindingExtension4.Path = "EmptyListText";
			BindingBase bindingBase4 = bindingExtension4.ProvideValue(null);
			label.SetBinding(Label.TextProperty, bindingBase4);
			stackLayout2.Children.Add(label);
			dynamicResourceExtension7.Key = "BaseFontSize";
			IMarkupExtension<DynamicResource> markupExtension13 = dynamicResourceExtension7;
			XamlServiceProvider xamlServiceProvider13 = new XamlServiceProvider();
			Type typeFromHandle25 = typeof(IProvideValueTarget);
			object[] array15 = new object[0 + 5];
			array15[0] = label2;
			array15[1] = stackLayout2;
			array15[2] = listView;
			array15[3] = grid;
			array15[4] = this;
			object obj19;
			xamlServiceProvider13.Add(typeFromHandle25, obj19 = new SimpleValueTargetProvider(array15, Label.FontSizeProperty, nameScope));
			xamlServiceProvider13.Add(typeof(IReferenceProvider), obj19);
			Type typeFromHandle26 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver13 = new XmlNamespaceResolver();
			xmlNamespaceResolver13.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver13.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver13.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver13.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver13.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver13.Add("pids", "clr-namespace:CarScannerXamarinForms.OBD2.PIDS");
			xmlNamespaceResolver13.Add("settings", "clr-namespace:CarScannerXamarinForms.Settings");
			xamlServiceProvider13.Add(typeFromHandle26, new XamlTypeResolver(xmlNamespaceResolver13, typeof(SettingsPIDOverrideListPage).GetTypeInfo().Assembly));
			xamlServiceProvider13.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(227, 29)));
			DynamicResource dynamicResource7 = markupExtension13.ProvideValue(xamlServiceProvider13);
			label2.SetDynamicResource(Label.FontSizeProperty, dynamicResource7.Key);
			label2.SetValue(Label.HorizontalTextAlignmentProperty, new TextAlignmentConverter().ConvertFromInvariantString("Center"));
			bindingExtension5.Mode = 2;
			bindingExtension5.Path = "IsSelectProfileButtonVisible";
			BindingBase bindingBase5 = bindingExtension5.ProvideValue(null);
			label2.SetBinding(VisualElement.IsVisibleProperty, bindingBase5);
			translate2.Text = "Settings_Control_tbChooseProfile.Text";
			IMarkupExtension markupExtension14 = translate2;
			XamlServiceProvider xamlServiceProvider14 = new XamlServiceProvider();
			Type typeFromHandle27 = typeof(IProvideValueTarget);
			object[] array16 = new object[0 + 5];
			array16[0] = label2;
			array16[1] = stackLayout2;
			array16[2] = listView;
			array16[3] = grid;
			array16[4] = this;
			object obj20;
			xamlServiceProvider14.Add(typeFromHandle27, obj20 = new SimpleValueTargetProvider(array16, Label.TextProperty, nameScope));
			xamlServiceProvider14.Add(typeof(IReferenceProvider), obj20);
			Type typeFromHandle28 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver14 = new XmlNamespaceResolver();
			xmlNamespaceResolver14.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver14.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver14.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver14.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver14.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver14.Add("pids", "clr-namespace:CarScannerXamarinForms.OBD2.PIDS");
			xmlNamespaceResolver14.Add("settings", "clr-namespace:CarScannerXamarinForms.Settings");
			xamlServiceProvider14.Add(typeFromHandle28, new XamlTypeResolver(xmlNamespaceResolver14, typeof(SettingsPIDOverrideListPage).GetTypeInfo().Assembly));
			xamlServiceProvider14.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(230, 29)));
			object obj21 = markupExtension14.ProvideValue(xamlServiceProvider14);
			label2.Text = obj21;
			stackLayout2.Children.Add(label2);
			bindingExtension6.Mode = 2;
			bindingExtension6.Path = "IsSelectProfileButtonVisible";
			BindingBase bindingBase6 = bindingExtension6.ProvideValue(null);
			stackLayout.SetBinding(VisualElement.IsVisibleProperty, bindingBase6);
			stackLayout.SetValue(StackLayout.OrientationProperty, 0);
			label3.SetValue(BindableObject.BindingContextProperty, sharedSettings);
			label3.SetValue(Label.FontAttributesProperty, new FontAttributesConverter().ConvertFromInvariantString("Bold"));
			dynamicResourceExtension8.Key = "BaseFontSize";
			IMarkupExtension<DynamicResource> markupExtension15 = dynamicResourceExtension8;
			XamlServiceProvider xamlServiceProvider15 = new XamlServiceProvider();
			Type typeFromHandle29 = typeof(IProvideValueTarget);
			object[] array17 = new object[0 + 6];
			array17[0] = label3;
			array17[1] = stackLayout;
			array17[2] = stackLayout2;
			array17[3] = listView;
			array17[4] = grid;
			array17[5] = this;
			object obj22;
			xamlServiceProvider15.Add(typeFromHandle29, obj22 = new SimpleValueTargetProvider(array17, Label.FontSizeProperty, nameScope));
			xamlServiceProvider15.Add(typeof(IReferenceProvider), obj22);
			Type typeFromHandle30 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver15 = new XmlNamespaceResolver();
			xmlNamespaceResolver15.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver15.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver15.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver15.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver15.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver15.Add("pids", "clr-namespace:CarScannerXamarinForms.OBD2.PIDS");
			xmlNamespaceResolver15.Add("settings", "clr-namespace:CarScannerXamarinForms.Settings");
			xamlServiceProvider15.Add(typeFromHandle30, new XamlTypeResolver(xmlNamespaceResolver15, typeof(SettingsPIDOverrideListPage).GetTypeInfo().Assembly));
			xamlServiceProvider15.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(236, 33)));
			DynamicResource dynamicResource8 = markupExtension15.ProvideValue(xamlServiceProvider15);
			label3.SetDynamicResource(Label.FontSizeProperty, dynamicResource8.Key);
			label3.SetValue(Label.HorizontalTextAlignmentProperty, new TextAlignmentConverter().ConvertFromInvariantString("Center"));
			bindingExtension7.Mode = 2;
			bindingExtension7.Path = "BrandAndProfile";
			BindingBase bindingBase7 = bindingExtension7.ProvideValue(null);
			label3.SetBinding(Label.TextProperty, bindingBase7);
			stackLayout.Children.Add(label3);
			stackLayout2.Children.Add(stackLayout);
			button.Clicked += this.btnSelectProfile_Clicked;
			button.SetValue(View.HorizontalOptionsProperty, LayoutOptions.CenterAndExpand);
			bindingExtension8.Mode = 2;
			bindingExtension8.Path = "IsSelectProfileButtonVisible";
			BindingBase bindingBase8 = bindingExtension8.ProvideValue(null);
			button.SetBinding(VisualElement.IsVisibleProperty, bindingBase8);
			translate3.Text = "Settings_Control_btnSelectProfile.Content";
			IMarkupExtension markupExtension16 = translate3;
			XamlServiceProvider xamlServiceProvider16 = new XamlServiceProvider();
			Type typeFromHandle31 = typeof(IProvideValueTarget);
			object[] array18 = new object[0 + 5];
			array18[0] = button;
			array18[1] = stackLayout2;
			array18[2] = listView;
			array18[3] = grid;
			array18[4] = this;
			object obj23;
			xamlServiceProvider16.Add(typeFromHandle31, obj23 = new SimpleValueTargetProvider(array18, Button.TextProperty, nameScope));
			xamlServiceProvider16.Add(typeof(IReferenceProvider), obj23);
			Type typeFromHandle32 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver16 = new XmlNamespaceResolver();
			xmlNamespaceResolver16.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver16.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver16.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver16.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver16.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver16.Add("pids", "clr-namespace:CarScannerXamarinForms.OBD2.PIDS");
			xmlNamespaceResolver16.Add("settings", "clr-namespace:CarScannerXamarinForms.Settings");
			xamlServiceProvider16.Add(typeFromHandle32, new XamlTypeResolver(xmlNamespaceResolver16, typeof(SettingsPIDOverrideListPage).GetTypeInfo().Assembly));
			xamlServiceProvider16.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(244, 29)));
			object obj24 = markupExtension16.ProvideValue(xamlServiceProvider16);
			button.Text = obj24;
			stackLayout2.Children.Add(button);
			listView.SetValue(ListView.HeaderProperty, stackLayout2);
			grid.Children.Add(listView);
			activityFrame.SetValue(Grid.RowProperty, 1);
			activityFrame.SetValue(Grid.ColumnProperty, 0);
			activityFrame.SetValue(Grid.ColumnSpanProperty, 2);
			activityFrame.SetValue(VisualElement.IsVisibleProperty, new VisualElement.VisibilityConverter().ConvertFromInvariantString("False"));
			grid.Children.Add(activityFrame);
			button2.SetValue(Grid.RowProperty, 2);
			button2.SetValue(Grid.ColumnProperty, 0);
			button2.SetValue(Grid.ColumnSpanProperty, 1);
			button2.Clicked += this.btnImportFromFile_Clicked;
			translate4.Text = "droid_ImportFromFile";
			IMarkupExtension markupExtension17 = translate4;
			XamlServiceProvider xamlServiceProvider17 = new XamlServiceProvider();
			Type typeFromHandle33 = typeof(IProvideValueTarget);
			object[] array19 = new object[0 + 3];
			array19[0] = button2;
			array19[1] = grid;
			array19[2] = this;
			object obj25;
			xamlServiceProvider17.Add(typeFromHandle33, obj25 = new SimpleValueTargetProvider(array19, Button.TextProperty, nameScope));
			xamlServiceProvider17.Add(typeof(IReferenceProvider), obj25);
			Type typeFromHandle34 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver17 = new XmlNamespaceResolver();
			xmlNamespaceResolver17.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver17.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver17.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver17.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver17.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver17.Add("pids", "clr-namespace:CarScannerXamarinForms.OBD2.PIDS");
			xmlNamespaceResolver17.Add("settings", "clr-namespace:CarScannerXamarinForms.Settings");
			xamlServiceProvider17.Add(typeFromHandle34, new XamlTypeResolver(xmlNamespaceResolver17, typeof(SettingsPIDOverrideListPage).GetTypeInfo().Assembly));
			xamlServiceProvider17.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(272, 17)));
			object obj26 = markupExtension17.ProvideValue(xamlServiceProvider17);
			button2.Text = obj26;
			grid.Children.Add(button2);
			button3.SetValue(Grid.RowProperty, 2);
			button3.SetValue(Grid.ColumnProperty, 1);
			button3.SetValue(Grid.ColumnSpanProperty, 1);
			button3.Clicked += this.btnExportAsFile_Clicked;
			translate5.Text = "ios_Share";
			IMarkupExtension markupExtension18 = translate5;
			XamlServiceProvider xamlServiceProvider18 = new XamlServiceProvider();
			Type typeFromHandle35 = typeof(IProvideValueTarget);
			object[] array20 = new object[0 + 3];
			array20[0] = button3;
			array20[1] = grid;
			array20[2] = this;
			object obj27;
			xamlServiceProvider18.Add(typeFromHandle35, obj27 = new SimpleValueTargetProvider(array20, Button.TextProperty, nameScope));
			xamlServiceProvider18.Add(typeof(IReferenceProvider), obj27);
			Type typeFromHandle36 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver18 = new XmlNamespaceResolver();
			xmlNamespaceResolver18.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver18.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver18.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver18.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver18.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver18.Add("pids", "clr-namespace:CarScannerXamarinForms.OBD2.PIDS");
			xmlNamespaceResolver18.Add("settings", "clr-namespace:CarScannerXamarinForms.Settings");
			xamlServiceProvider18.Add(typeFromHandle36, new XamlTypeResolver(xmlNamespaceResolver18, typeof(SettingsPIDOverrideListPage).GetTypeInfo().Assembly));
			xamlServiceProvider18.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(288, 17)));
			object obj28 = markupExtension18.ProvideValue(xamlServiceProvider18);
			button3.Text = obj28;
			grid.Children.Add(button3);
			this.SetValue(ContentPage.ContentProperty, grid);
		}

		// Token: 0x06001B65 RID: 7013 RVA: 0x00135501 File Offset: 0x00133701
		[CompilerGenerated]
		private void <.ctor>b__0_0()
		{
			this.AddNewCustomPid();
		}

		// Token: 0x06001B66 RID: 7014 RVA: 0x00135509 File Offset: 0x00133709
		[CompilerGenerated]
		private void <.ctor>b__0_1()
		{
			this.DeleteAllCustomPidsQuestion();
		}

		// Token: 0x06001B67 RID: 7015 RVA: 0x00135511 File Offset: 0x00133711
		[CompilerGenerated]
		private void <.ctor>b__0_2()
		{
			this.btnManageProfilePids_Clicked(this, EventArgs.Empty);
		}

		// Token: 0x06001B68 RID: 7016 RVA: 0x00135520 File Offset: 0x00133720
		private void __InitComponentRuntime()
		{
			Extensions.LoadFromXaml<SettingsPIDOverrideListPage>(this, typeof(SettingsPIDOverrideListPage));
			this.searchBar = NameScopeExtensions.FindByName<Entry>(this, "searchBar");
			this.pickerBaseList = NameScopeExtensions.FindByName<Picker>(this, "pickerBaseList");
			this.lv = NameScopeExtensions.FindByName<ListView>(this, "lv");
			this.labelSelectedProfile = NameScopeExtensions.FindByName<Label>(this, "labelSelectedProfile");
			this.activityFrame = NameScopeExtensions.FindByName<ActivityFrame>(this, "activityFrame");
			this.btnImportFromFile = NameScopeExtensions.FindByName<Button>(this, "btnImportFromFile");
			this.btnExportAsFile = NameScopeExtensions.FindByName<Button>(this, "btnExportAsFile");
		}

		// Token: 0x04000CB7 RID: 3255
		[CompilerGenerated]
		private PIDEditorListModel <Model>k__BackingField;

		// Token: 0x04000CB8 RID: 3256
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private Entry searchBar;

		// Token: 0x04000CB9 RID: 3257
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private Picker pickerBaseList;

		// Token: 0x04000CBA RID: 3258
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private ListView lv;

		// Token: 0x04000CBB RID: 3259
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private Label labelSelectedProfile;

		// Token: 0x04000CBC RID: 3260
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private ActivityFrame activityFrame;

		// Token: 0x04000CBD RID: 3261
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private Button btnImportFromFile;

		// Token: 0x04000CBE RID: 3262
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private Button btnExportAsFile;

		// Token: 0x02000245 RID: 581
		[CompilerGenerated]
		[Serializable]
		private sealed class <>c
		{
			// Token: 0x06001B69 RID: 7017 RVA: 0x001355B5 File Offset: 0x001337B5
			// Note: this type is marked as 'beforefieldinit'.
			static <>c()
			{
			}

			// Token: 0x06001B6A RID: 7018 RVA: 0x00002050 File Offset: 0x00000250
			public <>c()
			{
			}

			// Token: 0x06001B6B RID: 7019 RVA: 0x00041D11 File Offset: 0x0003FF11
			internal void <SettingsPIDOverrideListPage_Appearing>b__1_0()
			{
				CustomPIDViewModel.DisabledProfile.Load();
			}

			// Token: 0x06001B6C RID: 7020 RVA: 0x001355C1 File Offset: 0x001337C1
			internal bool <LoadList>b__9_0(PID x)
			{
				return x is IPIDFloatValue || x is IPIDWithStringValue || x is PID0103_FuelSystemStatus || x is PID_Status;
			}

			// Token: 0x04000CBF RID: 3263
			public static readonly SettingsPIDOverrideListPage.<>c <>9 = new SettingsPIDOverrideListPage.<>c();

			// Token: 0x04000CC0 RID: 3264
			public static Action <>9__1_0;

			// Token: 0x04000CC1 RID: 3265
			public static Func<PID, bool> <>9__9_0;
		}

		// Token: 0x02000246 RID: 582
		[CompilerGenerated]
		private sealed class <>c__DisplayClass13_0
		{
			// Token: 0x06001B6D RID: 7021 RVA: 0x00002050 File Offset: 0x00000250
			public <>c__DisplayClass13_0()
			{
			}

			// Token: 0x06001B6E RID: 7022 RVA: 0x001355E6 File Offset: 0x001337E6
			internal bool <btnImportFromFile_Clicked>b__0(CustomPID x)
			{
				return x.Id == this.pid.Id;
			}

			// Token: 0x04000CC2 RID: 3266
			public CustomPID pid;
		}

		// Token: 0x02000247 RID: 583
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <AddNewCustomPid>d__2 : IAsyncStateMachine
		{
			// Token: 0x06001B6F RID: 7023 RVA: 0x001355FC File Offset: 0x001337FC
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				SettingsPIDOverrideListPage settingsPIDOverrideListPage = this;
				try
				{
					TaskAwaiter taskAwaiter;
					if (num != 0)
					{
						cpid = new CustomPID("New PID", "New PID", "", "", "", UnitsHelper.Units.None, 0.0, 100.0, "", "", false, Roles.None, CustomPIDType.Formula, 0, 1, 1.0, 1.0, 0.0, false, false, true, null);
						CustomPIDViewModel.CurrentCustom.PidCollection.Add(cpid);
						CustomPIDViewModel.CurrentCustom.Save();
						if (App.OBDReader.CurrentStatus == OBDDataReaderStatus.ConnectedToECU)
						{
							LiveDataPIDModel._PIDCollection.Add(cpid);
						}
						if (settingsPIDOverrideListPage.Model.View != PIDEditorListModel.ViewByType.All && settingsPIDOverrideListPage.Model.View != PIDEditorListModel.ViewByType.LastCarSupported && settingsPIDOverrideListPage.Model.View != PIDEditorListModel.ViewByType.UserPids)
						{
							goto IL_0154;
						}
						taskAwaiter = settingsPIDOverrideListPage.Model.UpdateBaseList(false).GetAwaiter();
						if (!taskAwaiter.IsCompleted)
						{
							num2 = 0;
							TaskAwaiter taskAwaiter2 = taskAwaiter;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, SettingsPIDOverrideListPage.<AddNewCustomPid>d__2>(ref taskAwaiter, ref this);
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
					try
					{
						settingsPIDOverrideListPage.lv.ScrollTo(cpid, 0, false);
					}
					catch
					{
					}
					IL_0154:;
				}
				catch (Exception ex)
				{
					num2 = -2;
					cpid = null;
					this.<>t__builder.SetException(ex);
					return;
				}
				num2 = -2;
				cpid = null;
				this.<>t__builder.SetResult();
			}

			// Token: 0x06001B70 RID: 7024 RVA: 0x001357CC File Offset: 0x001339CC
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x04000CC3 RID: 3267
			public int <>1__state;

			// Token: 0x04000CC4 RID: 3268
			public AsyncVoidMethodBuilder <>t__builder;

			// Token: 0x04000CC5 RID: 3269
			public SettingsPIDOverrideListPage <>4__this;

			// Token: 0x04000CC6 RID: 3270
			private CustomPID <cpid>5__2;

			// Token: 0x04000CC7 RID: 3271
			private TaskAwaiter <>u__1;
		}

		// Token: 0x02000248 RID: 584
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <DeleteAllCustomPidsQuestion>d__3 : IAsyncStateMachine
		{
			// Token: 0x06001B71 RID: 7025 RVA: 0x001357DC File Offset: 0x001339DC
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				SettingsPIDOverrideListPage settingsPIDOverrideListPage = this;
				try
				{
					TaskAwaiter<bool> taskAwaiter3;
					if (num != 0)
					{
						taskAwaiter3 = settingsPIDOverrideListPage.DisplayAlert(Translate.GetString("ios_PidListDeleteAllCustomTitle"), Translate.GetString("ios_PidListDeleteAllCustomText"), Translate.GetString("ios_Yes"), Translate.GetString("ios_No")).GetAwaiter();
						if (!taskAwaiter3.IsCompleted)
						{
							num2 = 0;
							taskAwaiter2 = taskAwaiter3;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<bool>, SettingsPIDOverrideListPage.<DeleteAllCustomPidsQuestion>d__3>(ref taskAwaiter3, ref this);
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
						if (App.OBDReader.CurrentStatus != OBDDataReaderStatus.Disconnected)
						{
							foreach (CustomPID customPID in CustomPIDViewModel.CurrentCustom.PidCollection.ToArray<CustomPID>())
							{
								try
								{
									LiveDataPIDModel._PIDCollection.Remove(customPID);
								}
								catch
								{
								}
							}
						}
						CustomPIDViewModel.CurrentCustom.PidCollection.Clear();
						CustomPIDViewModel.CurrentCustom.Save();
						settingsPIDOverrideListPage.Model.UpdateBaseList(false);
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

			// Token: 0x06001B72 RID: 7026 RVA: 0x00135938 File Offset: 0x00133B38
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x04000CC8 RID: 3272
			public int <>1__state;

			// Token: 0x04000CC9 RID: 3273
			public AsyncVoidMethodBuilder <>t__builder;

			// Token: 0x04000CCA RID: 3274
			public SettingsPIDOverrideListPage <>4__this;

			// Token: 0x04000CCB RID: 3275
			private TaskAwaiter<bool> <>u__1;
		}

		// Token: 0x02000249 RID: 585
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <SettingsPIDOverrideListPage_Appearing>d__1 : IAsyncStateMachine
		{
			// Token: 0x06001B73 RID: 7027 RVA: 0x00135948 File Offset: 0x00133B48
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				SettingsPIDOverrideListPage settingsPIDOverrideListPage = this;
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
							goto IL_0100;
						}
						settingsPIDOverrideListPage.activityFrame.IsVisible = true;
						if (CustomPIDViewModel.DisabledProfile.Loaded)
						{
							goto IL_00A9;
						}
						taskAwaiter = Task.Run(delegate
						{
							CustomPIDViewModel.DisabledProfile.Load();
						}).GetAwaiter();
						if (!taskAwaiter.IsCompleted)
						{
							num2 = 0;
							TaskAwaiter taskAwaiter2 = taskAwaiter;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, SettingsPIDOverrideListPage.<SettingsPIDOverrideListPage_Appearing>d__1>(ref taskAwaiter, ref this);
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
					IL_00A9:
					taskAwaiter = settingsPIDOverrideListPage.Model.UpdateBaseList(true).GetAwaiter();
					if (!taskAwaiter.IsCompleted)
					{
						num2 = 1;
						TaskAwaiter taskAwaiter2 = taskAwaiter;
						this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, SettingsPIDOverrideListPage.<SettingsPIDOverrideListPage_Appearing>d__1>(ref taskAwaiter, ref this);
						return;
					}
					IL_0100:
					taskAwaiter.GetResult();
					settingsPIDOverrideListPage.activityFrame.IsVisible = false;
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

			// Token: 0x06001B74 RID: 7028 RVA: 0x00135AB0 File Offset: 0x00133CB0
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x04000CCC RID: 3276
			public int <>1__state;

			// Token: 0x04000CCD RID: 3277
			public AsyncVoidMethodBuilder <>t__builder;

			// Token: 0x04000CCE RID: 3278
			public SettingsPIDOverrideListPage <>4__this;

			// Token: 0x04000CCF RID: 3279
			private TaskAwaiter <>u__1;
		}

		// Token: 0x0200024A RID: 586
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <btnDel_Clicked>d__14 : IAsyncStateMachine
		{
			// Token: 0x06001B75 RID: 7029 RVA: 0x00135AC0 File Offset: 0x00133CC0
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				SettingsPIDOverrideListPage settingsPIDOverrideListPage = this;
				try
				{
					TaskAwaiter taskAwaiter;
					if (num != 0)
					{
						settingsPIDOverrideListPage.lv.IsEnabled = false;
						CustomPID customPID = (sender as MenuItem).BindingContext as CustomPID;
						try
						{
							LiveDataPIDModel._PIDCollection.Remove(customPID);
						}
						catch
						{
						}
						CustomPIDViewModel.CurrentCustom.PidCollection.Remove(customPID);
						CustomPIDViewModel.CurrentCustom.Save();
						taskAwaiter = settingsPIDOverrideListPage.Model.UpdateBaseList(false).GetAwaiter();
						if (!taskAwaiter.IsCompleted)
						{
							num2 = 0;
							TaskAwaiter taskAwaiter2 = taskAwaiter;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, SettingsPIDOverrideListPage.<btnDel_Clicked>d__14>(ref taskAwaiter, ref this);
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
					settingsPIDOverrideListPage.lv.IsEnabled = true;
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

			// Token: 0x06001B76 RID: 7030 RVA: 0x00135BE4 File Offset: 0x00133DE4
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x04000CD0 RID: 3280
			public int <>1__state;

			// Token: 0x04000CD1 RID: 3281
			public AsyncVoidMethodBuilder <>t__builder;

			// Token: 0x04000CD2 RID: 3282
			public SettingsPIDOverrideListPage <>4__this;

			// Token: 0x04000CD3 RID: 3283
			public object sender;

			// Token: 0x04000CD4 RID: 3284
			private TaskAwaiter <>u__1;
		}

		// Token: 0x0200024B RID: 587
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <btnExportAsFile_Clicked>d__12 : IAsyncStateMachine
		{
			// Token: 0x06001B77 RID: 7031 RVA: 0x00135BF4 File Offset: 0x00133DF4
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				try
				{
					string text2;
					if (num != 0)
					{
						string cacheDirectory = FileSystem.CacheDirectory;
						string text = "custompids.csp";
						text2 = Path.Combine(cacheDirectory, text);
						string text3 = JsonConvert.SerializeObject(CustomPIDViewModel.CurrentCustom.PidCollection);
						try
						{
							StreamWriter streamWriter = new StreamWriter(text2, false, Encoding.UTF8);
							try
							{
								streamWriter.WriteLine(text3);
								streamWriter.Flush();
							}
							finally
							{
								if (num < 0 && streamWriter != null)
								{
									((IDisposable)streamWriter).Dispose();
								}
							}
						}
						catch (Exception)
						{
						}
					}
					try
					{
						TaskAwaiter taskAwaiter;
						if (num != 0)
						{
							taskAwaiter = Share.RequestAsync(new ShareFileRequest
							{
								Title = "Custom PIDs",
								File = new ShareFile(text2)
							}).GetAwaiter();
							if (!taskAwaiter.IsCompleted)
							{
								num = (num2 = 0);
								TaskAwaiter taskAwaiter2 = taskAwaiter;
								this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, SettingsPIDOverrideListPage.<btnExportAsFile_Clicked>d__12>(ref taskAwaiter, ref this);
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

			// Token: 0x06001B78 RID: 7032 RVA: 0x00135D44 File Offset: 0x00133F44
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x04000CD5 RID: 3285
			public int <>1__state;

			// Token: 0x04000CD6 RID: 3286
			public AsyncVoidMethodBuilder <>t__builder;

			// Token: 0x04000CD7 RID: 3287
			private TaskAwaiter <>u__1;
		}

		// Token: 0x0200024C RID: 588
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <btnImportFromFile_Clicked>d__13 : IAsyncStateMachine
		{
			// Token: 0x06001B79 RID: 7033 RVA: 0x00135D54 File Offset: 0x00133F54
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				SettingsPIDOverrideListPage settingsPIDOverrideListPage = this;
				try
				{
					try
					{
						TaskAwaiter<FileResult> taskAwaiter;
						TaskAwaiter taskAwaiter3;
						switch (num)
						{
						case 0:
						{
							TaskAwaiter<FileResult> taskAwaiter2;
							taskAwaiter = taskAwaiter2;
							taskAwaiter2 = default(TaskAwaiter<FileResult>);
							num = (num2 = -1);
							break;
						}
						case 1:
							IL_00B3:
							try
							{
								if (num != 1)
								{
									string text = sr.ReadToEnd();
									tempList = JsonConvert.DeserializeObject<List<CustomPID>>(text);
									List<CustomPID>.Enumerator enumerator = tempList.GetEnumerator();
									try
									{
										while (enumerator.MoveNext())
										{
											SettingsPIDOverrideListPage.<>c__DisplayClass13_0 CS$<>8__locals1 = new SettingsPIDOverrideListPage.<>c__DisplayClass13_0();
											CS$<>8__locals1.pid = enumerator.Current;
											if (CS$<>8__locals1.pid.Id <= 1000000 || CS$<>8__locals1.pid.Id >= 10000000)
											{
												CS$<>8__locals1.pid.Id = -1000;
											}
											CustomPID customPID = CustomPIDViewModel.CurrentCustom.PidCollection.FirstOrDefault((CustomPID x) => x.Id == CS$<>8__locals1.pid.Id);
											if (customPID != null)
											{
												CustomPIDViewModel.CurrentCustom.PidCollection.Remove(customPID);
											}
											CustomPIDViewModel.CurrentCustom.PidCollection.Add(CS$<>8__locals1.pid);
											if (App.OBDReader.CurrentStatus == OBDDataReaderStatus.ConnectedToECU)
											{
												LiveDataPIDModel._PIDCollection.Add(CS$<>8__locals1.pid);
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
									CustomPIDViewModel.CurrentCustom.Save();
									if (settingsPIDOverrideListPage.Model.View != PIDEditorListModel.ViewByType.All && settingsPIDOverrideListPage.Model.View != PIDEditorListModel.ViewByType.LastCarSupported && settingsPIDOverrideListPage.Model.View != PIDEditorListModel.ViewByType.UserPids)
									{
										goto IL_0272;
									}
									taskAwaiter3 = settingsPIDOverrideListPage.Model.UpdateBaseList(false).GetAwaiter();
									if (!taskAwaiter3.IsCompleted)
									{
										num = (num2 = 1);
										TaskAwaiter taskAwaiter4 = taskAwaiter3;
										this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, SettingsPIDOverrideListPage.<btnImportFromFile_Clicked>d__13>(ref taskAwaiter3, ref this);
										return;
									}
								}
								else
								{
									TaskAwaiter taskAwaiter4;
									taskAwaiter3 = taskAwaiter4;
									taskAwaiter4 = default(TaskAwaiter);
									num = (num2 = -1);
								}
								taskAwaiter3.GetResult();
								try
								{
									settingsPIDOverrideListPage.lv.ScrollTo(tempList.LastOrDefault<CustomPID>(), 0, false);
								}
								catch
								{
								}
								IL_0272:
								tempList = null;
							}
							finally
							{
								if (num < 0 && sr != null)
								{
									((IDisposable)sr).Dispose();
								}
							}
							sr = null;
							goto IL_03AD;
						case 2:
						{
							TaskAwaiter taskAwaiter4;
							taskAwaiter3 = taskAwaiter4;
							taskAwaiter4 = default(TaskAwaiter);
							num = (num2 = -1);
							goto IL_03A6;
						}
						default:
							taskAwaiter = FilePicker.PickAsync(null).GetAwaiter();
							if (!taskAwaiter.IsCompleted)
							{
								num = (num2 = 0);
								TaskAwaiter<FileResult> taskAwaiter2 = taskAwaiter;
								this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<FileResult>, SettingsPIDOverrideListPage.<btnImportFromFile_Clicked>d__13>(ref taskAwaiter, ref this);
								return;
							}
							break;
						}
						FileResult result = taskAwaiter.GetResult();
						if (result == null)
						{
							goto IL_03AD;
						}
						if (result.FileName.EndsWith("csp", StringComparison.OrdinalIgnoreCase))
						{
							sr = new StreamReader(result.FullPath, Encoding.UTF8);
							goto IL_00B3;
						}
						if (!result.FileName.EndsWith("csv", StringComparison.OrdinalIgnoreCase))
						{
							goto IL_03AD;
						}
						FileStream fileStream = File.OpenRead(result.FullPath);
						try
						{
							IEnumerator<CustomPID> enumerator2 = CSVLoader.LoadFromCSV(fileStream).GetEnumerator();
							try
							{
								while (enumerator2.MoveNext())
								{
									CustomPID customPID2 = enumerator2.Current;
									CustomPIDViewModel.CurrentCustom.PidCollection.Add(customPID2);
								}
							}
							finally
							{
								if (num < 0 && enumerator2 != null)
								{
									enumerator2.Dispose();
								}
							}
						}
						finally
						{
							if (num < 0 && fileStream != null)
							{
								((IDisposable)fileStream).Dispose();
							}
						}
						CustomPIDViewModel.CurrentCustom.Save();
						if (settingsPIDOverrideListPage.Model.View != PIDEditorListModel.ViewByType.All && settingsPIDOverrideListPage.Model.View != PIDEditorListModel.ViewByType.LastCarSupported && settingsPIDOverrideListPage.Model.View != PIDEditorListModel.ViewByType.UserPids)
						{
							goto IL_03AD;
						}
						taskAwaiter3 = settingsPIDOverrideListPage.Model.UpdateBaseList(false).GetAwaiter();
						if (!taskAwaiter3.IsCompleted)
						{
							num = (num2 = 2);
							TaskAwaiter taskAwaiter4 = taskAwaiter3;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, SettingsPIDOverrideListPage.<btnImportFromFile_Clicked>d__13>(ref taskAwaiter3, ref this);
							return;
						}
						IL_03A6:
						taskAwaiter3.GetResult();
						IL_03AD:;
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

			// Token: 0x06001B7A RID: 7034 RVA: 0x001361F0 File Offset: 0x001343F0
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x04000CD8 RID: 3288
			public int <>1__state;

			// Token: 0x04000CD9 RID: 3289
			public AsyncVoidMethodBuilder <>t__builder;

			// Token: 0x04000CDA RID: 3290
			public SettingsPIDOverrideListPage <>4__this;

			// Token: 0x04000CDB RID: 3291
			private TaskAwaiter<FileResult> <>u__1;

			// Token: 0x04000CDC RID: 3292
			private StreamReader <sr>5__2;

			// Token: 0x04000CDD RID: 3293
			private List<CustomPID> <tempList>5__3;

			// Token: 0x04000CDE RID: 3294
			private TaskAwaiter <>u__2;
		}

		// Token: 0x0200024D RID: 589
		[CompilerGenerated]
		private sealed class <InitializeComponent>_anonXamlCDataTemplate_95
		{
			// Token: 0x06001B7B RID: 7035 RVA: 0x00136200 File Offset: 0x00134400
			public <InitializeComponent>_anonXamlCDataTemplate_95()
			{
			}

			// Token: 0x06001B7C RID: 7036 RVA: 0x00136214 File Offset: 0x00134414
			internal object LoadDataTemplate()
			{
				BindingExtension bindingExtension;
				VisualDiagnostics.RegisterSourceInfo(bindingExtension = new BindingExtension(), new Uri("Settings\\SettingsPIDOverrideListPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 30, 29);
				Translate translate;
				VisualDiagnostics.RegisterSourceInfo(translate = new Translate(), new Uri("Settings\\SettingsPIDOverrideListPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 32, 29);
				MenuItem menuItem;
				VisualDiagnostics.RegisterSourceInfo(menuItem = new MenuItem(), new Uri("Settings\\SettingsPIDOverrideListPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 28, 26);
				RowDefinition rowDefinition;
				VisualDiagnostics.RegisterSourceInfo(rowDefinition = new RowDefinition(), new Uri("Settings\\SettingsPIDOverrideListPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 36, 30);
				RowDefinition rowDefinition2;
				VisualDiagnostics.RegisterSourceInfo(rowDefinition2 = new RowDefinition(), new Uri("Settings\\SettingsPIDOverrideListPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 37, 30);
				RowDefinition rowDefinition3;
				VisualDiagnostics.RegisterSourceInfo(rowDefinition3 = new RowDefinition(), new Uri("Settings\\SettingsPIDOverrideListPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 38, 30);
				DynamicResourceExtension dynamicResourceExtension;
				VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension = new DynamicResourceExtension(), new Uri("Settings\\SettingsPIDOverrideListPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 43, 29);
				BindingExtension bindingExtension2;
				VisualDiagnostics.RegisterSourceInfo(bindingExtension2 = new BindingExtension(), new Uri("Settings\\SettingsPIDOverrideListPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 44, 29);
				Label label;
				VisualDiagnostics.RegisterSourceInfo(label = new Label(), new Uri("Settings\\SettingsPIDOverrideListPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 40, 26);
				StaticResourceExtension staticResourceExtension;
				VisualDiagnostics.RegisterSourceInfo(staticResourceExtension = new StaticResourceExtension(), new Uri("Settings\\SettingsPIDOverrideListPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 47, 29);
				BindingExtension bindingExtension3;
				VisualDiagnostics.RegisterSourceInfo(bindingExtension3 = new BindingExtension(), new Uri("Settings\\SettingsPIDOverrideListPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 47, 29);
				DynamicResourceExtension dynamicResourceExtension2;
				VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension2 = new DynamicResourceExtension(), new Uri("Settings\\SettingsPIDOverrideListPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 49, 36);
				Span span;
				VisualDiagnostics.RegisterSourceInfo(span = new Span(), new Uri("Settings\\SettingsPIDOverrideListPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 52, 42);
				BindingExtension bindingExtension4;
				VisualDiagnostics.RegisterSourceInfo(bindingExtension4 = new BindingExtension(), new Uri("Settings\\SettingsPIDOverrideListPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 53, 47);
				Span span2;
				VisualDiagnostics.RegisterSourceInfo(span2 = new Span(), new Uri("Settings\\SettingsPIDOverrideListPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 53, 42);
				Span span3;
				VisualDiagnostics.RegisterSourceInfo(span3 = new Span(), new Uri("Settings\\SettingsPIDOverrideListPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 54, 42);
				Translate translate2;
				VisualDiagnostics.RegisterSourceInfo(translate2 = new Translate(), new Uri("Settings\\SettingsPIDOverrideListPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 55, 47);
				Span span4;
				VisualDiagnostics.RegisterSourceInfo(span4 = new Span(), new Uri("Settings\\SettingsPIDOverrideListPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 55, 42);
				BindingExtension bindingExtension5;
				VisualDiagnostics.RegisterSourceInfo(bindingExtension5 = new BindingExtension(), new Uri("Settings\\SettingsPIDOverrideListPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 56, 47);
				Span span5;
				VisualDiagnostics.RegisterSourceInfo(span5 = new Span(), new Uri("Settings\\SettingsPIDOverrideListPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 56, 42);
				Span span6;
				VisualDiagnostics.RegisterSourceInfo(span6 = new Span(), new Uri("Settings\\SettingsPIDOverrideListPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 57, 42);
				BindingExtension bindingExtension6;
				VisualDiagnostics.RegisterSourceInfo(bindingExtension6 = new BindingExtension(), new Uri("Settings\\SettingsPIDOverrideListPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 58, 47);
				Span span7;
				VisualDiagnostics.RegisterSourceInfo(span7 = new Span(), new Uri("Settings\\SettingsPIDOverrideListPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 58, 42);
				Span span8;
				VisualDiagnostics.RegisterSourceInfo(span8 = new Span(), new Uri("Settings\\SettingsPIDOverrideListPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 59, 42);
				Translate translate3;
				VisualDiagnostics.RegisterSourceInfo(translate3 = new Translate(), new Uri("Settings\\SettingsPIDOverrideListPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 60, 47);
				Span span9;
				VisualDiagnostics.RegisterSourceInfo(span9 = new Span(), new Uri("Settings\\SettingsPIDOverrideListPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 60, 42);
				StaticResourceExtension staticResourceExtension2;
				VisualDiagnostics.RegisterSourceInfo(staticResourceExtension2 = new StaticResourceExtension(), new Uri("Settings\\SettingsPIDOverrideListPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 61, 47);
				BindingExtension bindingExtension7;
				VisualDiagnostics.RegisterSourceInfo(bindingExtension7 = new BindingExtension(), new Uri("Settings\\SettingsPIDOverrideListPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 61, 47);
				Span span10;
				VisualDiagnostics.RegisterSourceInfo(span10 = new Span(), new Uri("Settings\\SettingsPIDOverrideListPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 61, 42);
				FormattedString formattedString;
				VisualDiagnostics.RegisterSourceInfo(formattedString = new FormattedString(), new Uri("Settings\\SettingsPIDOverrideListPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 51, 38);
				Label label2;
				VisualDiagnostics.RegisterSourceInfo(label2 = new Label(), new Uri("Settings\\SettingsPIDOverrideListPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 49, 30);
				StaticResourceExtension staticResourceExtension3;
				VisualDiagnostics.RegisterSourceInfo(staticResourceExtension3 = new StaticResourceExtension(), new Uri("Settings\\SettingsPIDOverrideListPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 66, 42);
				BindingExtension bindingExtension8;
				VisualDiagnostics.RegisterSourceInfo(bindingExtension8 = new BindingExtension(), new Uri("Settings\\SettingsPIDOverrideListPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 66, 42);
				DynamicResourceExtension dynamicResourceExtension3;
				VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension3 = new DynamicResourceExtension(), new Uri("Settings\\SettingsPIDOverrideListPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 68, 37);
				BindingExtension bindingExtension9;
				VisualDiagnostics.RegisterSourceInfo(bindingExtension9 = new BindingExtension(), new Uri("Settings\\SettingsPIDOverrideListPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 69, 37);
				BindingExtension bindingExtension10;
				VisualDiagnostics.RegisterSourceInfo(bindingExtension10 = new BindingExtension(), new Uri("Settings\\SettingsPIDOverrideListPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 70, 37);
				Label label3;
				VisualDiagnostics.RegisterSourceInfo(label3 = new Label(), new Uri("Settings\\SettingsPIDOverrideListPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 67, 34);
				DynamicResourceExtension dynamicResourceExtension4;
				VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension4 = new DynamicResourceExtension(), new Uri("Settings\\SettingsPIDOverrideListPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 73, 37);
				StaticResourceExtension staticResourceExtension4;
				VisualDiagnostics.RegisterSourceInfo(staticResourceExtension4 = new StaticResourceExtension(), new Uri("Settings\\SettingsPIDOverrideListPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 74, 37);
				BindingExtension bindingExtension11;
				VisualDiagnostics.RegisterSourceInfo(bindingExtension11 = new BindingExtension(), new Uri("Settings\\SettingsPIDOverrideListPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 74, 37);
				BindingExtension bindingExtension12;
				VisualDiagnostics.RegisterSourceInfo(bindingExtension12 = new BindingExtension(), new Uri("Settings\\SettingsPIDOverrideListPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 75, 37);
				Label label4;
				VisualDiagnostics.RegisterSourceInfo(label4 = new Label(), new Uri("Settings\\SettingsPIDOverrideListPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 72, 34);
				StackLayout stackLayout;
				VisualDiagnostics.RegisterSourceInfo(stackLayout = new StackLayout(), new Uri("Settings\\SettingsPIDOverrideListPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 66, 30);
				StaticResourceExtension staticResourceExtension5;
				VisualDiagnostics.RegisterSourceInfo(staticResourceExtension5 = new StaticResourceExtension(), new Uri("Settings\\SettingsPIDOverrideListPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 79, 42);
				BindingExtension bindingExtension13;
				VisualDiagnostics.RegisterSourceInfo(bindingExtension13 = new BindingExtension(), new Uri("Settings\\SettingsPIDOverrideListPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 79, 42);
				DynamicResourceExtension dynamicResourceExtension5;
				VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension5 = new DynamicResourceExtension(), new Uri("Settings\\SettingsPIDOverrideListPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 80, 40);
				Translate translate4;
				VisualDiagnostics.RegisterSourceInfo(translate4 = new Translate(), new Uri("Settings\\SettingsPIDOverrideListPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 83, 51);
				Span span11;
				VisualDiagnostics.RegisterSourceInfo(span11 = new Span(), new Uri("Settings\\SettingsPIDOverrideListPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 83, 46);
				BindingExtension bindingExtension14;
				VisualDiagnostics.RegisterSourceInfo(bindingExtension14 = new BindingExtension(), new Uri("Settings\\SettingsPIDOverrideListPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 84, 73);
				Span span12;
				VisualDiagnostics.RegisterSourceInfo(span12 = new Span(), new Uri("Settings\\SettingsPIDOverrideListPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 84, 46);
				Span span13;
				VisualDiagnostics.RegisterSourceInfo(span13 = new Span(), new Uri("Settings\\SettingsPIDOverrideListPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 85, 46);
				Translate translate5;
				VisualDiagnostics.RegisterSourceInfo(translate5 = new Translate(), new Uri("Settings\\SettingsPIDOverrideListPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 86, 51);
				Span span14;
				VisualDiagnostics.RegisterSourceInfo(span14 = new Span(), new Uri("Settings\\SettingsPIDOverrideListPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 86, 46);
				BindingExtension bindingExtension15;
				VisualDiagnostics.RegisterSourceInfo(bindingExtension15 = new BindingExtension(), new Uri("Settings\\SettingsPIDOverrideListPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 87, 51);
				Span span15;
				VisualDiagnostics.RegisterSourceInfo(span15 = new Span(), new Uri("Settings\\SettingsPIDOverrideListPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 87, 46);
				Span span16;
				VisualDiagnostics.RegisterSourceInfo(span16 = new Span(), new Uri("Settings\\SettingsPIDOverrideListPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 88, 46);
				BindingExtension bindingExtension16;
				VisualDiagnostics.RegisterSourceInfo(bindingExtension16 = new BindingExtension(), new Uri("Settings\\SettingsPIDOverrideListPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 89, 51);
				Span span17;
				VisualDiagnostics.RegisterSourceInfo(span17 = new Span(), new Uri("Settings\\SettingsPIDOverrideListPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 89, 46);
				Span span18;
				VisualDiagnostics.RegisterSourceInfo(span18 = new Span(), new Uri("Settings\\SettingsPIDOverrideListPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 90, 46);
				BindingExtension bindingExtension17;
				VisualDiagnostics.RegisterSourceInfo(bindingExtension17 = new BindingExtension(), new Uri("Settings\\SettingsPIDOverrideListPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 91, 51);
				Span span19;
				VisualDiagnostics.RegisterSourceInfo(span19 = new Span(), new Uri("Settings\\SettingsPIDOverrideListPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 91, 46);
				Span span20;
				VisualDiagnostics.RegisterSourceInfo(span20 = new Span(), new Uri("Settings\\SettingsPIDOverrideListPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 92, 46);
				BindingExtension bindingExtension18;
				VisualDiagnostics.RegisterSourceInfo(bindingExtension18 = new BindingExtension(), new Uri("Settings\\SettingsPIDOverrideListPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 93, 51);
				Span span21;
				VisualDiagnostics.RegisterSourceInfo(span21 = new Span(), new Uri("Settings\\SettingsPIDOverrideListPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 93, 46);
				FormattedString formattedString2;
				VisualDiagnostics.RegisterSourceInfo(formattedString2 = new FormattedString(), new Uri("Settings\\SettingsPIDOverrideListPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 82, 42);
				Label label5;
				VisualDiagnostics.RegisterSourceInfo(label5 = new Label(), new Uri("Settings\\SettingsPIDOverrideListPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 80, 34);
				StackLayout stackLayout2;
				VisualDiagnostics.RegisterSourceInfo(stackLayout2 = new StackLayout(), new Uri("Settings\\SettingsPIDOverrideListPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 79, 30);
				StaticResourceExtension staticResourceExtension6;
				VisualDiagnostics.RegisterSourceInfo(staticResourceExtension6 = new StaticResourceExtension(), new Uri("Settings\\SettingsPIDOverrideListPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 99, 42);
				BindingExtension bindingExtension19;
				VisualDiagnostics.RegisterSourceInfo(bindingExtension19 = new BindingExtension(), new Uri("Settings\\SettingsPIDOverrideListPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 99, 42);
				DynamicResourceExtension dynamicResourceExtension6;
				VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension6 = new DynamicResourceExtension(), new Uri("Settings\\SettingsPIDOverrideListPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 100, 40);
				Translate translate6;
				VisualDiagnostics.RegisterSourceInfo(translate6 = new Translate(), new Uri("Settings\\SettingsPIDOverrideListPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 103, 51);
				Span span22;
				VisualDiagnostics.RegisterSourceInfo(span22 = new Span(), new Uri("Settings\\SettingsPIDOverrideListPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 103, 46);
				BindingExtension bindingExtension20;
				VisualDiagnostics.RegisterSourceInfo(bindingExtension20 = new BindingExtension(), new Uri("Settings\\SettingsPIDOverrideListPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 104, 51);
				Span span23;
				VisualDiagnostics.RegisterSourceInfo(span23 = new Span(), new Uri("Settings\\SettingsPIDOverrideListPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 104, 46);
				Span span24;
				VisualDiagnostics.RegisterSourceInfo(span24 = new Span(), new Uri("Settings\\SettingsPIDOverrideListPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 105, 46);
				Translate translate7;
				VisualDiagnostics.RegisterSourceInfo(translate7 = new Translate(), new Uri("Settings\\SettingsPIDOverrideListPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 106, 51);
				Span span25;
				VisualDiagnostics.RegisterSourceInfo(span25 = new Span(), new Uri("Settings\\SettingsPIDOverrideListPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 106, 46);
				BindingExtension bindingExtension21;
				VisualDiagnostics.RegisterSourceInfo(bindingExtension21 = new BindingExtension(), new Uri("Settings\\SettingsPIDOverrideListPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 107, 51);
				Span span26;
				VisualDiagnostics.RegisterSourceInfo(span26 = new Span(), new Uri("Settings\\SettingsPIDOverrideListPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 107, 46);
				FormattedString formattedString3;
				VisualDiagnostics.RegisterSourceInfo(formattedString3 = new FormattedString(), new Uri("Settings\\SettingsPIDOverrideListPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 102, 42);
				Label label6;
				VisualDiagnostics.RegisterSourceInfo(label6 = new Label(), new Uri("Settings\\SettingsPIDOverrideListPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 100, 34);
				StackLayout stackLayout3;
				VisualDiagnostics.RegisterSourceInfo(stackLayout3 = new StackLayout(), new Uri("Settings\\SettingsPIDOverrideListPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 99, 30);
				StaticResourceExtension staticResourceExtension7;
				VisualDiagnostics.RegisterSourceInfo(staticResourceExtension7 = new StaticResourceExtension(), new Uri("Settings\\SettingsPIDOverrideListPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 113, 42);
				BindingExtension bindingExtension22;
				VisualDiagnostics.RegisterSourceInfo(bindingExtension22 = new BindingExtension(), new Uri("Settings\\SettingsPIDOverrideListPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 113, 42);
				DynamicResourceExtension dynamicResourceExtension7;
				VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension7 = new DynamicResourceExtension(), new Uri("Settings\\SettingsPIDOverrideListPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 114, 40);
				Translate translate8;
				VisualDiagnostics.RegisterSourceInfo(translate8 = new Translate(), new Uri("Settings\\SettingsPIDOverrideListPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 114, 83);
				Label label7;
				VisualDiagnostics.RegisterSourceInfo(label7 = new Label(), new Uri("Settings\\SettingsPIDOverrideListPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 114, 34);
				StackLayout stackLayout4;
				VisualDiagnostics.RegisterSourceInfo(stackLayout4 = new StackLayout(), new Uri("Settings\\SettingsPIDOverrideListPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 113, 30);
				StackLayout stackLayout5;
				VisualDiagnostics.RegisterSourceInfo(stackLayout5 = new StackLayout(), new Uri("Settings\\SettingsPIDOverrideListPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 45, 26);
				DynamicResourceExtension dynamicResourceExtension8;
				VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension8 = new DynamicResourceExtension(), new Uri("Settings\\SettingsPIDOverrideListPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 118, 45);
				Translate translate9;
				VisualDiagnostics.RegisterSourceInfo(translate9 = new Translate(), new Uri("Settings\\SettingsPIDOverrideListPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 121, 43);
				Span span27;
				VisualDiagnostics.RegisterSourceInfo(span27 = new Span(), new Uri("Settings\\SettingsPIDOverrideListPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 121, 38);
				StaticResourceExtension staticResourceExtension8;
				VisualDiagnostics.RegisterSourceInfo(staticResourceExtension8 = new StaticResourceExtension(), new Uri("Settings\\SettingsPIDOverrideListPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 122, 43);
				BindingExtension bindingExtension23;
				VisualDiagnostics.RegisterSourceInfo(bindingExtension23 = new BindingExtension(), new Uri("Settings\\SettingsPIDOverrideListPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 122, 43);
				Span span28;
				VisualDiagnostics.RegisterSourceInfo(span28 = new Span(), new Uri("Settings\\SettingsPIDOverrideListPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 122, 38);
				FormattedString formattedString4;
				VisualDiagnostics.RegisterSourceInfo(formattedString4 = new FormattedString(), new Uri("Settings\\SettingsPIDOverrideListPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 120, 34);
				Label label8;
				VisualDiagnostics.RegisterSourceInfo(label8 = new Label(), new Uri("Settings\\SettingsPIDOverrideListPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 118, 26);
				Grid grid;
				VisualDiagnostics.RegisterSourceInfo(grid = new Grid(), new Uri("Settings\\SettingsPIDOverrideListPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 34, 22);
				ViewCell viewCell;
				VisualDiagnostics.RegisterSourceInfo(viewCell = new ViewCell(), new Uri("Settings\\SettingsPIDOverrideListPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 26, 18);
				NameScope nameScope = new NameScope();
				NameScope.SetNameScope(viewCell, nameScope);
				menuItem.Clicked += this.root.btnDel_Clicked;
				bindingExtension.Path = ".";
				bindingExtension.TypedBinding = new TypedBinding<CustomPID, CustomPID>((CustomPID A_0) => new ValueTuple<CustomPID, bool>(A_0, true), null, null);
				BindingBase bindingBase = bindingExtension.ProvideValue(null);
				menuItem.SetBinding(MenuItem.CommandParameterProperty, bindingBase);
				menuItem.SetValue(MenuItem.IsDestructiveProperty, true);
				translate.Text = "SpeedTest_btnRemove.Label";
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
				xmlNamespaceResolver.Add("pids", "clr-namespace:CarScannerXamarinForms.OBD2.PIDS");
				xmlNamespaceResolver.Add("settings", "clr-namespace:CarScannerXamarinForms.Settings");
				xamlServiceProvider.Add(typeFromHandle2, new XamlTypeResolver(xmlNamespaceResolver, typeof(SettingsPIDOverrideListPage.<InitializeComponent>_anonXamlCDataTemplate_95).GetTypeInfo().Assembly));
				xamlServiceProvider.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(32, 29)));
				object obj2 = markupExtension.ProvideValue(xamlServiceProvider);
				menuItem.Text = obj2;
				viewCell.ContextActions.Add(menuItem);
				rowDefinition.SetValue(RowDefinition.HeightProperty, new GridLengthTypeConverter().ConvertFromInvariantString("auto"));
				grid.GetValue(Grid.RowDefinitionsProperty).Add(rowDefinition);
				rowDefinition2.SetValue(RowDefinition.HeightProperty, new GridLengthTypeConverter().ConvertFromInvariantString("auto"));
				grid.GetValue(Grid.RowDefinitionsProperty).Add(rowDefinition2);
				rowDefinition3.SetValue(RowDefinition.HeightProperty, new GridLengthTypeConverter().ConvertFromInvariantString("auto"));
				grid.GetValue(Grid.RowDefinitionsProperty).Add(rowDefinition3);
				label.SetValue(Grid.RowProperty, 0);
				label.SetValue(Label.FontAttributesProperty, new FontAttributesConverter().ConvertFromInvariantString("Bold"));
				dynamicResourceExtension.Key = "BaseFontSize+";
				IMarkupExtension<DynamicResource> markupExtension2 = dynamicResourceExtension;
				XamlServiceProvider xamlServiceProvider2 = new XamlServiceProvider();
				Type typeFromHandle3 = typeof(IProvideValueTarget);
				int num2;
				object[] array3 = new object[(num2 = this.parentValues.Length) + 3];
				Array.Copy(this.parentValues, 0, array3, 3, num2);
				object[] array4 = array3;
				array4[0] = label;
				array4[1] = grid;
				array4[2] = viewCell;
				object obj3;
				xamlServiceProvider2.Add(typeFromHandle3, obj3 = new SimpleValueTargetProvider(array4, Label.FontSizeProperty, nameScope));
				xamlServiceProvider2.Add(typeof(IReferenceProvider), obj3);
				Type typeFromHandle4 = typeof(IXamlTypeResolver);
				XmlNamespaceResolver xmlNamespaceResolver2 = new XmlNamespaceResolver();
				xmlNamespaceResolver2.Add("", "http://xamarin.com/schemas/2014/forms");
				xmlNamespaceResolver2.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
				xmlNamespaceResolver2.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
				xmlNamespaceResolver2.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
				xmlNamespaceResolver2.Add("local", "clr-namespace:CarScannerXamarinForms");
				xmlNamespaceResolver2.Add("pids", "clr-namespace:CarScannerXamarinForms.OBD2.PIDS");
				xmlNamespaceResolver2.Add("settings", "clr-namespace:CarScannerXamarinForms.Settings");
				xamlServiceProvider2.Add(typeFromHandle4, new XamlTypeResolver(xmlNamespaceResolver2, typeof(SettingsPIDOverrideListPage.<InitializeComponent>_anonXamlCDataTemplate_95).GetTypeInfo().Assembly));
				xamlServiceProvider2.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(43, 29)));
				DynamicResource dynamicResource = markupExtension2.ProvideValue(xamlServiceProvider2);
				label.SetDynamicResource(Label.FontSizeProperty, dynamicResource.Key);
				bindingExtension2.Mode = 2;
				bindingExtension2.Path = "Name";
				bindingExtension2.TypedBinding = new TypedBinding<CustomPID, string>(delegate(CustomPID A_0)
				{
					if (A_0 != null)
					{
						return new ValueTuple<string, bool>(A_0.Name, true);
					}
					return default(ValueTuple<string, bool>);
				}, null, new Tuple<Func<CustomPID, object>, string>[]
				{
					new Tuple<Func<CustomPID, object>, string>((CustomPID A_0) => A_0, "Name")
				});
				BindingBase bindingBase2 = bindingExtension2.ProvideValue(null);
				label.SetBinding(Label.TextProperty, bindingBase2);
				grid.Children.Add(label);
				stackLayout5.SetValue(Grid.RowProperty, 1);
				bindingExtension3.Mode = 2;
				staticResourceExtension.Key = "BoolToNegativeConverter";
				IMarkupExtension markupExtension3 = staticResourceExtension;
				XamlServiceProvider xamlServiceProvider3 = new XamlServiceProvider();
				Type typeFromHandle5 = typeof(IProvideValueTarget);
				int num3;
				object[] array5 = new object[(num3 = this.parentValues.Length) + 4];
				Array.Copy(this.parentValues, 0, array5, 4, num3);
				object[] array6 = array5;
				array6[0] = bindingExtension3;
				array6[1] = stackLayout5;
				array6[2] = grid;
				array6[3] = viewCell;
				object obj4;
				xamlServiceProvider3.Add(typeFromHandle5, obj4 = new SimpleValueTargetProvider(array6, typeof(BindingExtension).GetRuntimeProperty("Converter"), nameScope));
				xamlServiceProvider3.Add(typeof(IReferenceProvider), obj4);
				Type typeFromHandle6 = typeof(IXamlTypeResolver);
				XmlNamespaceResolver xmlNamespaceResolver3 = new XmlNamespaceResolver();
				xmlNamespaceResolver3.Add("", "http://xamarin.com/schemas/2014/forms");
				xmlNamespaceResolver3.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
				xmlNamespaceResolver3.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
				xmlNamespaceResolver3.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
				xmlNamespaceResolver3.Add("local", "clr-namespace:CarScannerXamarinForms");
				xmlNamespaceResolver3.Add("pids", "clr-namespace:CarScannerXamarinForms.OBD2.PIDS");
				xmlNamespaceResolver3.Add("settings", "clr-namespace:CarScannerXamarinForms.Settings");
				xamlServiceProvider3.Add(typeFromHandle6, new XamlTypeResolver(xmlNamespaceResolver3, typeof(SettingsPIDOverrideListPage.<InitializeComponent>_anonXamlCDataTemplate_95).GetTypeInfo().Assembly));
				xamlServiceProvider3.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(47, 29)));
				object obj5 = markupExtension3.ProvideValue(xamlServiceProvider3);
				bindingExtension3.Converter = obj5;
				bindingExtension3.Path = "IsFormulaHidden";
				bindingExtension3.TypedBinding = new TypedBinding<CustomPID, bool>(delegate(CustomPID A_0)
				{
					if (A_0 != null)
					{
						return new ValueTuple<bool, bool>(A_0.IsFormulaHidden, true);
					}
					return default(ValueTuple<bool, bool>);
				}, null, new Tuple<Func<CustomPID, object>, string>[]
				{
					new Tuple<Func<CustomPID, object>, string>((CustomPID A_0) => A_0, "IsFormulaHidden")
				});
				BindingBase bindingBase3 = bindingExtension3.ProvideValue(null);
				stackLayout5.SetBinding(VisualElement.IsVisibleProperty, bindingBase3);
				stackLayout5.SetValue(StackLayout.OrientationProperty, 0);
				dynamicResourceExtension2.Key = "BaseFontSize-";
				IMarkupExtension<DynamicResource> markupExtension4 = dynamicResourceExtension2;
				XamlServiceProvider xamlServiceProvider4 = new XamlServiceProvider();
				Type typeFromHandle7 = typeof(IProvideValueTarget);
				int num4;
				object[] array7 = new object[(num4 = this.parentValues.Length) + 4];
				Array.Copy(this.parentValues, 0, array7, 4, num4);
				object[] array8 = array7;
				array8[0] = label2;
				array8[1] = stackLayout5;
				array8[2] = grid;
				array8[3] = viewCell;
				object obj6;
				xamlServiceProvider4.Add(typeFromHandle7, obj6 = new SimpleValueTargetProvider(array8, Label.FontSizeProperty, nameScope));
				xamlServiceProvider4.Add(typeof(IReferenceProvider), obj6);
				Type typeFromHandle8 = typeof(IXamlTypeResolver);
				XmlNamespaceResolver xmlNamespaceResolver4 = new XmlNamespaceResolver();
				xmlNamespaceResolver4.Add("", "http://xamarin.com/schemas/2014/forms");
				xmlNamespaceResolver4.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
				xmlNamespaceResolver4.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
				xmlNamespaceResolver4.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
				xmlNamespaceResolver4.Add("local", "clr-namespace:CarScannerXamarinForms");
				xmlNamespaceResolver4.Add("pids", "clr-namespace:CarScannerXamarinForms.OBD2.PIDS");
				xmlNamespaceResolver4.Add("settings", "clr-namespace:CarScannerXamarinForms.Settings");
				xamlServiceProvider4.Add(typeFromHandle8, new XamlTypeResolver(xmlNamespaceResolver4, typeof(SettingsPIDOverrideListPage.<InitializeComponent>_anonXamlCDataTemplate_95).GetTypeInfo().Assembly));
				xamlServiceProvider4.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(49, 36)));
				DynamicResource dynamicResource2 = markupExtension4.ProvideValue(xamlServiceProvider4);
				label2.SetDynamicResource(Label.FontSizeProperty, dynamicResource2.Key);
				span.SetValue(Span.TextProperty, "ID=");
				formattedString.Spans.Add(span);
				bindingExtension4.Mode = 2;
				bindingExtension4.Path = "Id";
				bindingExtension4.TypedBinding = new TypedBinding<CustomPID, int>(delegate(CustomPID A_0)
				{
					if (A_0 != null)
					{
						return new ValueTuple<int, bool>(A_0.Id, true);
					}
					return default(ValueTuple<int, bool>);
				}, null, new Tuple<Func<CustomPID, object>, string>[]
				{
					new Tuple<Func<CustomPID, object>, string>((CustomPID A_0) => A_0, "Id")
				});
				BindingBase bindingBase4 = bindingExtension4.ProvideValue(null);
				span2.SetBinding(Span.TextProperty, bindingBase4);
				formattedString.Spans.Add(span2);
				span3.SetValue(Span.TextProperty, ", ");
				formattedString.Spans.Add(span3);
				translate2.Text = "ios_Header";
				IMarkupExtension markupExtension5 = translate2;
				XamlServiceProvider xamlServiceProvider5 = new XamlServiceProvider();
				Type typeFromHandle9 = typeof(IProvideValueTarget);
				int num5;
				object[] array9 = new object[(num5 = this.parentValues.Length) + 6];
				Array.Copy(this.parentValues, 0, array9, 6, num5);
				object[] array10 = array9;
				array10[0] = span4;
				array10[1] = formattedString;
				array10[2] = label2;
				array10[3] = stackLayout5;
				array10[4] = grid;
				array10[5] = viewCell;
				object obj7;
				xamlServiceProvider5.Add(typeFromHandle9, obj7 = new SimpleValueTargetProvider(array10, Span.TextProperty, nameScope));
				xamlServiceProvider5.Add(typeof(IReferenceProvider), obj7);
				Type typeFromHandle10 = typeof(IXamlTypeResolver);
				XmlNamespaceResolver xmlNamespaceResolver5 = new XmlNamespaceResolver();
				xmlNamespaceResolver5.Add("", "http://xamarin.com/schemas/2014/forms");
				xmlNamespaceResolver5.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
				xmlNamespaceResolver5.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
				xmlNamespaceResolver5.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
				xmlNamespaceResolver5.Add("local", "clr-namespace:CarScannerXamarinForms");
				xmlNamespaceResolver5.Add("pids", "clr-namespace:CarScannerXamarinForms.OBD2.PIDS");
				xmlNamespaceResolver5.Add("settings", "clr-namespace:CarScannerXamarinForms.Settings");
				xamlServiceProvider5.Add(typeFromHandle10, new XamlTypeResolver(xmlNamespaceResolver5, typeof(SettingsPIDOverrideListPage.<InitializeComponent>_anonXamlCDataTemplate_95).GetTypeInfo().Assembly));
				xamlServiceProvider5.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(55, 47)));
				object obj8 = markupExtension5.ProvideValue(xamlServiceProvider5);
				span4.Text = obj8;
				formattedString.Spans.Add(span4);
				bindingExtension5.Mode = 2;
				bindingExtension5.Path = "Header";
				bindingExtension5.TypedBinding = new TypedBinding<CustomPID, string>(delegate(CustomPID A_0)
				{
					if (A_0 != null)
					{
						return new ValueTuple<string, bool>(A_0.Header, true);
					}
					return default(ValueTuple<string, bool>);
				}, null, new Tuple<Func<CustomPID, object>, string>[]
				{
					new Tuple<Func<CustomPID, object>, string>((CustomPID A_0) => A_0, "Header")
				});
				BindingBase bindingBase5 = bindingExtension5.ProvideValue(null);
				span5.SetBinding(Span.TextProperty, bindingBase5);
				formattedString.Spans.Add(span5);
				span6.SetValue(Span.TextProperty, ", PID: ");
				formattedString.Spans.Add(span6);
				bindingExtension6.Mode = 2;
				bindingExtension6.Path = "Command";
				bindingExtension6.TypedBinding = new TypedBinding<CustomPID, string>(delegate(CustomPID A_0)
				{
					if (A_0 != null)
					{
						return new ValueTuple<string, bool>(A_0.Command, true);
					}
					return default(ValueTuple<string, bool>);
				}, null, new Tuple<Func<CustomPID, object>, string>[]
				{
					new Tuple<Func<CustomPID, object>, string>((CustomPID A_0) => A_0, "Command")
				});
				BindingBase bindingBase6 = bindingExtension6.ProvideValue(null);
				span7.SetBinding(Span.TextProperty, bindingBase6);
				formattedString.Spans.Add(span7);
				span8.SetValue(Span.TextProperty, ", ");
				formattedString.Spans.Add(span8);
				translate3.Text = "ios_Units";
				IMarkupExtension markupExtension6 = translate3;
				XamlServiceProvider xamlServiceProvider6 = new XamlServiceProvider();
				Type typeFromHandle11 = typeof(IProvideValueTarget);
				int num6;
				object[] array11 = new object[(num6 = this.parentValues.Length) + 6];
				Array.Copy(this.parentValues, 0, array11, 6, num6);
				object[] array12 = array11;
				array12[0] = span9;
				array12[1] = formattedString;
				array12[2] = label2;
				array12[3] = stackLayout5;
				array12[4] = grid;
				array12[5] = viewCell;
				object obj9;
				xamlServiceProvider6.Add(typeFromHandle11, obj9 = new SimpleValueTargetProvider(array12, Span.TextProperty, nameScope));
				xamlServiceProvider6.Add(typeof(IReferenceProvider), obj9);
				Type typeFromHandle12 = typeof(IXamlTypeResolver);
				XmlNamespaceResolver xmlNamespaceResolver6 = new XmlNamespaceResolver();
				xmlNamespaceResolver6.Add("", "http://xamarin.com/schemas/2014/forms");
				xmlNamespaceResolver6.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
				xmlNamespaceResolver6.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
				xmlNamespaceResolver6.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
				xmlNamespaceResolver6.Add("local", "clr-namespace:CarScannerXamarinForms");
				xmlNamespaceResolver6.Add("pids", "clr-namespace:CarScannerXamarinForms.OBD2.PIDS");
				xmlNamespaceResolver6.Add("settings", "clr-namespace:CarScannerXamarinForms.Settings");
				xamlServiceProvider6.Add(typeFromHandle12, new XamlTypeResolver(xmlNamespaceResolver6, typeof(SettingsPIDOverrideListPage.<InitializeComponent>_anonXamlCDataTemplate_95).GetTypeInfo().Assembly));
				xamlServiceProvider6.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(60, 47)));
				object obj10 = markupExtension6.ProvideValue(xamlServiceProvider6);
				span9.Text = obj10;
				formattedString.Spans.Add(span9);
				bindingExtension7.Mode = 2;
				staticResourceExtension2.Key = "UnitsToStringConverter";
				IMarkupExtension markupExtension7 = staticResourceExtension2;
				XamlServiceProvider xamlServiceProvider7 = new XamlServiceProvider();
				Type typeFromHandle13 = typeof(IProvideValueTarget);
				int num7;
				object[] array13 = new object[(num7 = this.parentValues.Length) + 7];
				Array.Copy(this.parentValues, 0, array13, 7, num7);
				object[] array14 = array13;
				array14[0] = bindingExtension7;
				array14[1] = span10;
				array14[2] = formattedString;
				array14[3] = label2;
				array14[4] = stackLayout5;
				array14[5] = grid;
				array14[6] = viewCell;
				object obj11;
				xamlServiceProvider7.Add(typeFromHandle13, obj11 = new SimpleValueTargetProvider(array14, typeof(BindingExtension).GetRuntimeProperty("Converter"), nameScope));
				xamlServiceProvider7.Add(typeof(IReferenceProvider), obj11);
				Type typeFromHandle14 = typeof(IXamlTypeResolver);
				XmlNamespaceResolver xmlNamespaceResolver7 = new XmlNamespaceResolver();
				xmlNamespaceResolver7.Add("", "http://xamarin.com/schemas/2014/forms");
				xmlNamespaceResolver7.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
				xmlNamespaceResolver7.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
				xmlNamespaceResolver7.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
				xmlNamespaceResolver7.Add("local", "clr-namespace:CarScannerXamarinForms");
				xmlNamespaceResolver7.Add("pids", "clr-namespace:CarScannerXamarinForms.OBD2.PIDS");
				xmlNamespaceResolver7.Add("settings", "clr-namespace:CarScannerXamarinForms.Settings");
				xamlServiceProvider7.Add(typeFromHandle14, new XamlTypeResolver(xmlNamespaceResolver7, typeof(SettingsPIDOverrideListPage.<InitializeComponent>_anonXamlCDataTemplate_95).GetTypeInfo().Assembly));
				xamlServiceProvider7.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(61, 47)));
				object obj12 = markupExtension7.ProvideValue(xamlServiceProvider7);
				bindingExtension7.Converter = obj12;
				bindingExtension7.Path = "Units";
				bindingExtension7.TypedBinding = new TypedBinding<CustomPID, UnitsHelper.Units>(delegate(CustomPID A_0)
				{
					if (A_0 != null)
					{
						return new ValueTuple<UnitsHelper.Units, bool>(A_0.Units, true);
					}
					return default(ValueTuple<UnitsHelper.Units, bool>);
				}, null, new Tuple<Func<CustomPID, object>, string>[]
				{
					new Tuple<Func<CustomPID, object>, string>((CustomPID A_0) => A_0, "Units")
				});
				BindingBase bindingBase7 = bindingExtension7.ProvideValue(null);
				span10.SetBinding(Span.TextProperty, bindingBase7);
				formattedString.Spans.Add(span10);
				label2.SetValue(Label.FormattedTextProperty, formattedString);
				stackLayout5.Children.Add(label2);
				bindingExtension8.Mode = 2;
				staticResourceExtension3.Key = "EnumValueToTrueConverter";
				IMarkupExtension markupExtension8 = staticResourceExtension3;
				XamlServiceProvider xamlServiceProvider8 = new XamlServiceProvider();
				Type typeFromHandle15 = typeof(IProvideValueTarget);
				int num8;
				object[] array15 = new object[(num8 = this.parentValues.Length) + 5];
				Array.Copy(this.parentValues, 0, array15, 5, num8);
				object[] array16 = array15;
				array16[0] = bindingExtension8;
				array16[1] = stackLayout;
				array16[2] = stackLayout5;
				array16[3] = grid;
				array16[4] = viewCell;
				object obj13;
				xamlServiceProvider8.Add(typeFromHandle15, obj13 = new SimpleValueTargetProvider(array16, typeof(BindingExtension).GetRuntimeProperty("Converter"), nameScope));
				xamlServiceProvider8.Add(typeof(IReferenceProvider), obj13);
				Type typeFromHandle16 = typeof(IXamlTypeResolver);
				XmlNamespaceResolver xmlNamespaceResolver8 = new XmlNamespaceResolver();
				xmlNamespaceResolver8.Add("", "http://xamarin.com/schemas/2014/forms");
				xmlNamespaceResolver8.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
				xmlNamespaceResolver8.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
				xmlNamespaceResolver8.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
				xmlNamespaceResolver8.Add("local", "clr-namespace:CarScannerXamarinForms");
				xmlNamespaceResolver8.Add("pids", "clr-namespace:CarScannerXamarinForms.OBD2.PIDS");
				xmlNamespaceResolver8.Add("settings", "clr-namespace:CarScannerXamarinForms.Settings");
				xamlServiceProvider8.Add(typeFromHandle16, new XamlTypeResolver(xmlNamespaceResolver8, typeof(SettingsPIDOverrideListPage.<InitializeComponent>_anonXamlCDataTemplate_95).GetTypeInfo().Assembly));
				xamlServiceProvider8.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(66, 42)));
				object obj14 = markupExtension8.ProvideValue(xamlServiceProvider8);
				bindingExtension8.Converter = obj14;
				bindingExtension8.ConverterParameter = "0";
				bindingExtension8.Path = "Type";
				bindingExtension8.TypedBinding = new TypedBinding<CustomPID, CustomPIDType>(delegate(CustomPID A_0)
				{
					if (A_0 != null)
					{
						return new ValueTuple<CustomPIDType, bool>(A_0.Type, true);
					}
					return default(ValueTuple<CustomPIDType, bool>);
				}, null, new Tuple<Func<CustomPID, object>, string>[]
				{
					new Tuple<Func<CustomPID, object>, string>((CustomPID A_0) => A_0, "Type")
				});
				BindingBase bindingBase8 = bindingExtension8.ProvideValue(null);
				stackLayout.SetBinding(VisualElement.IsVisibleProperty, bindingBase8);
				stackLayout.SetValue(StackLayout.OrientationProperty, 0);
				dynamicResourceExtension3.Key = "BaseFontSize-";
				IMarkupExtension<DynamicResource> markupExtension9 = dynamicResourceExtension3;
				XamlServiceProvider xamlServiceProvider9 = new XamlServiceProvider();
				Type typeFromHandle17 = typeof(IProvideValueTarget);
				int num9;
				object[] array17 = new object[(num9 = this.parentValues.Length) + 5];
				Array.Copy(this.parentValues, 0, array17, 5, num9);
				object[] array18 = array17;
				array18[0] = label3;
				array18[1] = stackLayout;
				array18[2] = stackLayout5;
				array18[3] = grid;
				array18[4] = viewCell;
				object obj15;
				xamlServiceProvider9.Add(typeFromHandle17, obj15 = new SimpleValueTargetProvider(array18, Label.FontSizeProperty, nameScope));
				xamlServiceProvider9.Add(typeof(IReferenceProvider), obj15);
				Type typeFromHandle18 = typeof(IXamlTypeResolver);
				XmlNamespaceResolver xmlNamespaceResolver9 = new XmlNamespaceResolver();
				xmlNamespaceResolver9.Add("", "http://xamarin.com/schemas/2014/forms");
				xmlNamespaceResolver9.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
				xmlNamespaceResolver9.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
				xmlNamespaceResolver9.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
				xmlNamespaceResolver9.Add("local", "clr-namespace:CarScannerXamarinForms");
				xmlNamespaceResolver9.Add("pids", "clr-namespace:CarScannerXamarinForms.OBD2.PIDS");
				xmlNamespaceResolver9.Add("settings", "clr-namespace:CarScannerXamarinForms.Settings");
				xamlServiceProvider9.Add(typeFromHandle18, new XamlTypeResolver(xmlNamespaceResolver9, typeof(SettingsPIDOverrideListPage.<InitializeComponent>_anonXamlCDataTemplate_95).GetTypeInfo().Assembly));
				xamlServiceProvider9.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(68, 37)));
				DynamicResource dynamicResource3 = markupExtension9.ProvideValue(xamlServiceProvider9);
				label3.SetDynamicResource(Label.FontSizeProperty, dynamicResource3.Key);
				bindingExtension9.Mode = 2;
				bindingExtension9.Path = "IsFormulaCorrect";
				bindingExtension9.TypedBinding = new TypedBinding<CustomPID, bool>(delegate(CustomPID A_0)
				{
					if (A_0 != null)
					{
						return new ValueTuple<bool, bool>(A_0.IsFormulaCorrect, true);
					}
					return default(ValueTuple<bool, bool>);
				}, null, new Tuple<Func<CustomPID, object>, string>[]
				{
					new Tuple<Func<CustomPID, object>, string>((CustomPID A_0) => A_0, "IsFormulaCorrect")
				});
				BindingBase bindingBase9 = bindingExtension9.ProvideValue(null);
				label3.SetBinding(VisualElement.IsVisibleProperty, bindingBase9);
				bindingExtension10.Path = "Formula";
				bindingExtension10.TypedBinding = new TypedBinding<CustomPID, string>(delegate(CustomPID A_0)
				{
					if (A_0 != null)
					{
						return new ValueTuple<string, bool>(A_0.Formula, true);
					}
					return default(ValueTuple<string, bool>);
				}, delegate(CustomPID A_0, string A_1)
				{
					if (A_0 != null)
					{
						A_0.Formula = A_1;
						return;
					}
				}, new Tuple<Func<CustomPID, object>, string>[]
				{
					new Tuple<Func<CustomPID, object>, string>((CustomPID A_0) => A_0, "Formula")
				});
				BindingBase bindingBase10 = bindingExtension10.ProvideValue(null);
				label3.SetBinding(Label.TextProperty, bindingBase10);
				label3.SetValue(Label.TextColorProperty, Color.Green);
				stackLayout.Children.Add(label3);
				dynamicResourceExtension4.Key = "BaseFontSize-";
				IMarkupExtension<DynamicResource> markupExtension10 = dynamicResourceExtension4;
				XamlServiceProvider xamlServiceProvider10 = new XamlServiceProvider();
				Type typeFromHandle19 = typeof(IProvideValueTarget);
				int num10;
				object[] array19 = new object[(num10 = this.parentValues.Length) + 5];
				Array.Copy(this.parentValues, 0, array19, 5, num10);
				object[] array20 = array19;
				array20[0] = label4;
				array20[1] = stackLayout;
				array20[2] = stackLayout5;
				array20[3] = grid;
				array20[4] = viewCell;
				object obj16;
				xamlServiceProvider10.Add(typeFromHandle19, obj16 = new SimpleValueTargetProvider(array20, Label.FontSizeProperty, nameScope));
				xamlServiceProvider10.Add(typeof(IReferenceProvider), obj16);
				Type typeFromHandle20 = typeof(IXamlTypeResolver);
				XmlNamespaceResolver xmlNamespaceResolver10 = new XmlNamespaceResolver();
				xmlNamespaceResolver10.Add("", "http://xamarin.com/schemas/2014/forms");
				xmlNamespaceResolver10.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
				xmlNamespaceResolver10.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
				xmlNamespaceResolver10.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
				xmlNamespaceResolver10.Add("local", "clr-namespace:CarScannerXamarinForms");
				xmlNamespaceResolver10.Add("pids", "clr-namespace:CarScannerXamarinForms.OBD2.PIDS");
				xmlNamespaceResolver10.Add("settings", "clr-namespace:CarScannerXamarinForms.Settings");
				xamlServiceProvider10.Add(typeFromHandle20, new XamlTypeResolver(xmlNamespaceResolver10, typeof(SettingsPIDOverrideListPage.<InitializeComponent>_anonXamlCDataTemplate_95).GetTypeInfo().Assembly));
				xamlServiceProvider10.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(73, 37)));
				DynamicResource dynamicResource4 = markupExtension10.ProvideValue(xamlServiceProvider10);
				label4.SetDynamicResource(Label.FontSizeProperty, dynamicResource4.Key);
				bindingExtension11.Mode = 2;
				staticResourceExtension4.Key = "BoolToNegativeConverter";
				IMarkupExtension markupExtension11 = staticResourceExtension4;
				XamlServiceProvider xamlServiceProvider11 = new XamlServiceProvider();
				Type typeFromHandle21 = typeof(IProvideValueTarget);
				int num11;
				object[] array21 = new object[(num11 = this.parentValues.Length) + 6];
				Array.Copy(this.parentValues, 0, array21, 6, num11);
				object[] array22 = array21;
				array22[0] = bindingExtension11;
				array22[1] = label4;
				array22[2] = stackLayout;
				array22[3] = stackLayout5;
				array22[4] = grid;
				array22[5] = viewCell;
				object obj17;
				xamlServiceProvider11.Add(typeFromHandle21, obj17 = new SimpleValueTargetProvider(array22, typeof(BindingExtension).GetRuntimeProperty("Converter"), nameScope));
				xamlServiceProvider11.Add(typeof(IReferenceProvider), obj17);
				Type typeFromHandle22 = typeof(IXamlTypeResolver);
				XmlNamespaceResolver xmlNamespaceResolver11 = new XmlNamespaceResolver();
				xmlNamespaceResolver11.Add("", "http://xamarin.com/schemas/2014/forms");
				xmlNamespaceResolver11.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
				xmlNamespaceResolver11.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
				xmlNamespaceResolver11.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
				xmlNamespaceResolver11.Add("local", "clr-namespace:CarScannerXamarinForms");
				xmlNamespaceResolver11.Add("pids", "clr-namespace:CarScannerXamarinForms.OBD2.PIDS");
				xmlNamespaceResolver11.Add("settings", "clr-namespace:CarScannerXamarinForms.Settings");
				xamlServiceProvider11.Add(typeFromHandle22, new XamlTypeResolver(xmlNamespaceResolver11, typeof(SettingsPIDOverrideListPage.<InitializeComponent>_anonXamlCDataTemplate_95).GetTypeInfo().Assembly));
				xamlServiceProvider11.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(74, 37)));
				object obj18 = markupExtension11.ProvideValue(xamlServiceProvider11);
				bindingExtension11.Converter = obj18;
				bindingExtension11.Path = "IsFormulaCorrect";
				bindingExtension11.TypedBinding = new TypedBinding<CustomPID, bool>(delegate(CustomPID A_0)
				{
					if (A_0 != null)
					{
						return new ValueTuple<bool, bool>(A_0.IsFormulaCorrect, true);
					}
					return default(ValueTuple<bool, bool>);
				}, null, new Tuple<Func<CustomPID, object>, string>[]
				{
					new Tuple<Func<CustomPID, object>, string>((CustomPID A_0) => A_0, "IsFormulaCorrect")
				});
				BindingBase bindingBase11 = bindingExtension11.ProvideValue(null);
				label4.SetBinding(VisualElement.IsVisibleProperty, bindingBase11);
				bindingExtension12.Path = "Formula";
				bindingExtension12.TypedBinding = new TypedBinding<CustomPID, string>(delegate(CustomPID A_0)
				{
					if (A_0 != null)
					{
						return new ValueTuple<string, bool>(A_0.Formula, true);
					}
					return default(ValueTuple<string, bool>);
				}, delegate(CustomPID A_0, string A_1)
				{
					if (A_0 != null)
					{
						A_0.Formula = A_1;
						return;
					}
				}, new Tuple<Func<CustomPID, object>, string>[]
				{
					new Tuple<Func<CustomPID, object>, string>((CustomPID A_0) => A_0, "Formula")
				});
				BindingBase bindingBase12 = bindingExtension12.ProvideValue(null);
				label4.SetBinding(Label.TextProperty, bindingBase12);
				label4.SetValue(Label.TextColorProperty, Color.Red);
				stackLayout.Children.Add(label4);
				stackLayout5.Children.Add(stackLayout);
				bindingExtension13.Mode = 2;
				staticResourceExtension5.Key = "EnumValueToTrueConverter";
				IMarkupExtension markupExtension12 = staticResourceExtension5;
				XamlServiceProvider xamlServiceProvider12 = new XamlServiceProvider();
				Type typeFromHandle23 = typeof(IProvideValueTarget);
				int num12;
				object[] array23 = new object[(num12 = this.parentValues.Length) + 5];
				Array.Copy(this.parentValues, 0, array23, 5, num12);
				object[] array24 = array23;
				array24[0] = bindingExtension13;
				array24[1] = stackLayout2;
				array24[2] = stackLayout5;
				array24[3] = grid;
				array24[4] = viewCell;
				object obj19;
				xamlServiceProvider12.Add(typeFromHandle23, obj19 = new SimpleValueTargetProvider(array24, typeof(BindingExtension).GetRuntimeProperty("Converter"), nameScope));
				xamlServiceProvider12.Add(typeof(IReferenceProvider), obj19);
				Type typeFromHandle24 = typeof(IXamlTypeResolver);
				XmlNamespaceResolver xmlNamespaceResolver12 = new XmlNamespaceResolver();
				xmlNamespaceResolver12.Add("", "http://xamarin.com/schemas/2014/forms");
				xmlNamespaceResolver12.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
				xmlNamespaceResolver12.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
				xmlNamespaceResolver12.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
				xmlNamespaceResolver12.Add("local", "clr-namespace:CarScannerXamarinForms");
				xmlNamespaceResolver12.Add("pids", "clr-namespace:CarScannerXamarinForms.OBD2.PIDS");
				xmlNamespaceResolver12.Add("settings", "clr-namespace:CarScannerXamarinForms.Settings");
				xamlServiceProvider12.Add(typeFromHandle24, new XamlTypeResolver(xmlNamespaceResolver12, typeof(SettingsPIDOverrideListPage.<InitializeComponent>_anonXamlCDataTemplate_95).GetTypeInfo().Assembly));
				xamlServiceProvider12.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(79, 42)));
				object obj20 = markupExtension12.ProvideValue(xamlServiceProvider12);
				bindingExtension13.Converter = obj20;
				bindingExtension13.ConverterParameter = "1";
				bindingExtension13.Path = "Type";
				bindingExtension13.TypedBinding = new TypedBinding<CustomPID, CustomPIDType>(delegate(CustomPID A_0)
				{
					if (A_0 != null)
					{
						return new ValueTuple<CustomPIDType, bool>(A_0.Type, true);
					}
					return default(ValueTuple<CustomPIDType, bool>);
				}, null, new Tuple<Func<CustomPID, object>, string>[]
				{
					new Tuple<Func<CustomPID, object>, string>((CustomPID A_0) => A_0, "Type")
				});
				BindingBase bindingBase13 = bindingExtension13.ProvideValue(null);
				stackLayout2.SetBinding(VisualElement.IsVisibleProperty, bindingBase13);
				stackLayout2.SetValue(StackLayout.OrientationProperty, 0);
				dynamicResourceExtension5.Key = "BaseFontSize-";
				IMarkupExtension<DynamicResource> markupExtension13 = dynamicResourceExtension5;
				XamlServiceProvider xamlServiceProvider13 = new XamlServiceProvider();
				Type typeFromHandle25 = typeof(IProvideValueTarget);
				int num13;
				object[] array25 = new object[(num13 = this.parentValues.Length) + 5];
				Array.Copy(this.parentValues, 0, array25, 5, num13);
				object[] array26 = array25;
				array26[0] = label5;
				array26[1] = stackLayout2;
				array26[2] = stackLayout5;
				array26[3] = grid;
				array26[4] = viewCell;
				object obj21;
				xamlServiceProvider13.Add(typeFromHandle25, obj21 = new SimpleValueTargetProvider(array26, Label.FontSizeProperty, nameScope));
				xamlServiceProvider13.Add(typeof(IReferenceProvider), obj21);
				Type typeFromHandle26 = typeof(IXamlTypeResolver);
				XmlNamespaceResolver xmlNamespaceResolver13 = new XmlNamespaceResolver();
				xmlNamespaceResolver13.Add("", "http://xamarin.com/schemas/2014/forms");
				xmlNamespaceResolver13.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
				xmlNamespaceResolver13.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
				xmlNamespaceResolver13.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
				xmlNamespaceResolver13.Add("local", "clr-namespace:CarScannerXamarinForms");
				xmlNamespaceResolver13.Add("pids", "clr-namespace:CarScannerXamarinForms.OBD2.PIDS");
				xmlNamespaceResolver13.Add("settings", "clr-namespace:CarScannerXamarinForms.Settings");
				xamlServiceProvider13.Add(typeFromHandle26, new XamlTypeResolver(xmlNamespaceResolver13, typeof(SettingsPIDOverrideListPage.<InitializeComponent>_anonXamlCDataTemplate_95).GetTypeInfo().Assembly));
				xamlServiceProvider13.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(80, 40)));
				DynamicResource dynamicResource5 = markupExtension13.ProvideValue(xamlServiceProvider13);
				label5.SetDynamicResource(Label.FontSizeProperty, dynamicResource5.Key);
				translate4.Text = "ios_Byte";
				IMarkupExtension markupExtension14 = translate4;
				XamlServiceProvider xamlServiceProvider14 = new XamlServiceProvider();
				Type typeFromHandle27 = typeof(IProvideValueTarget);
				int num14;
				object[] array27 = new object[(num14 = this.parentValues.Length) + 7];
				Array.Copy(this.parentValues, 0, array27, 7, num14);
				object[] array28 = array27;
				array28[0] = span11;
				array28[1] = formattedString2;
				array28[2] = label5;
				array28[3] = stackLayout2;
				array28[4] = stackLayout5;
				array28[5] = grid;
				array28[6] = viewCell;
				object obj22;
				xamlServiceProvider14.Add(typeFromHandle27, obj22 = new SimpleValueTargetProvider(array28, Span.TextProperty, nameScope));
				xamlServiceProvider14.Add(typeof(IReferenceProvider), obj22);
				Type typeFromHandle28 = typeof(IXamlTypeResolver);
				XmlNamespaceResolver xmlNamespaceResolver14 = new XmlNamespaceResolver();
				xmlNamespaceResolver14.Add("", "http://xamarin.com/schemas/2014/forms");
				xmlNamespaceResolver14.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
				xmlNamespaceResolver14.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
				xmlNamespaceResolver14.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
				xmlNamespaceResolver14.Add("local", "clr-namespace:CarScannerXamarinForms");
				xmlNamespaceResolver14.Add("pids", "clr-namespace:CarScannerXamarinForms.OBD2.PIDS");
				xmlNamespaceResolver14.Add("settings", "clr-namespace:CarScannerXamarinForms.Settings");
				xamlServiceProvider14.Add(typeFromHandle28, new XamlTypeResolver(xmlNamespaceResolver14, typeof(SettingsPIDOverrideListPage.<InitializeComponent>_anonXamlCDataTemplate_95).GetTypeInfo().Assembly));
				xamlServiceProvider14.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(83, 51)));
				object obj23 = markupExtension14.ProvideValue(xamlServiceProvider14);
				span11.Text = obj23;
				formattedString2.Spans.Add(span11);
				span12.SetValue(Span.FontAttributesProperty, new FontAttributesConverter().ConvertFromInvariantString("Bold"));
				bindingExtension14.Mode = 2;
				bindingExtension14.Path = "StartByteId";
				bindingExtension14.TypedBinding = new TypedBinding<CustomPID, int>(delegate(CustomPID A_0)
				{
					if (A_0 != null)
					{
						return new ValueTuple<int, bool>(A_0.StartByteId, true);
					}
					return default(ValueTuple<int, bool>);
				}, null, new Tuple<Func<CustomPID, object>, string>[]
				{
					new Tuple<Func<CustomPID, object>, string>((CustomPID A_0) => A_0, "StartByteId")
				});
				BindingBase bindingBase14 = bindingExtension14.ProvideValue(null);
				span12.SetBinding(Span.TextProperty, bindingBase14);
				formattedString2.Spans.Add(span12);
				span13.SetValue(Span.TextProperty, ", ");
				formattedString2.Spans.Add(span13);
				translate5.Text = "ios_DataLength";
				IMarkupExtension markupExtension15 = translate5;
				XamlServiceProvider xamlServiceProvider15 = new XamlServiceProvider();
				Type typeFromHandle29 = typeof(IProvideValueTarget);
				int num15;
				object[] array29 = new object[(num15 = this.parentValues.Length) + 7];
				Array.Copy(this.parentValues, 0, array29, 7, num15);
				object[] array30 = array29;
				array30[0] = span14;
				array30[1] = formattedString2;
				array30[2] = label5;
				array30[3] = stackLayout2;
				array30[4] = stackLayout5;
				array30[5] = grid;
				array30[6] = viewCell;
				object obj24;
				xamlServiceProvider15.Add(typeFromHandle29, obj24 = new SimpleValueTargetProvider(array30, Span.TextProperty, nameScope));
				xamlServiceProvider15.Add(typeof(IReferenceProvider), obj24);
				Type typeFromHandle30 = typeof(IXamlTypeResolver);
				XmlNamespaceResolver xmlNamespaceResolver15 = new XmlNamespaceResolver();
				xmlNamespaceResolver15.Add("", "http://xamarin.com/schemas/2014/forms");
				xmlNamespaceResolver15.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
				xmlNamespaceResolver15.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
				xmlNamespaceResolver15.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
				xmlNamespaceResolver15.Add("local", "clr-namespace:CarScannerXamarinForms");
				xmlNamespaceResolver15.Add("pids", "clr-namespace:CarScannerXamarinForms.OBD2.PIDS");
				xmlNamespaceResolver15.Add("settings", "clr-namespace:CarScannerXamarinForms.Settings");
				xamlServiceProvider15.Add(typeFromHandle30, new XamlTypeResolver(xmlNamespaceResolver15, typeof(SettingsPIDOverrideListPage.<InitializeComponent>_anonXamlCDataTemplate_95).GetTypeInfo().Assembly));
				xamlServiceProvider15.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(86, 51)));
				object obj25 = markupExtension15.ProvideValue(xamlServiceProvider15);
				span14.Text = obj25;
				formattedString2.Spans.Add(span14);
				bindingExtension15.Mode = 2;
				bindingExtension15.Path = "DataLength";
				bindingExtension15.TypedBinding = new TypedBinding<CustomPID, int>(delegate(CustomPID A_0)
				{
					if (A_0 != null)
					{
						return new ValueTuple<int, bool>(A_0.DataLength, true);
					}
					return default(ValueTuple<int, bool>);
				}, null, new Tuple<Func<CustomPID, object>, string>[]
				{
					new Tuple<Func<CustomPID, object>, string>((CustomPID A_0) => A_0, "DataLength")
				});
				BindingBase bindingBase15 = bindingExtension15.ProvideValue(null);
				span15.SetBinding(Span.TextProperty, bindingBase15);
				formattedString2.Spans.Add(span15);
				span16.SetValue(Span.TextProperty, ", X*");
				formattedString2.Spans.Add(span16);
				bindingExtension16.Mode = 2;
				bindingExtension16.Path = "Multiplier";
				bindingExtension16.TypedBinding = new TypedBinding<CustomPID, double>(delegate(CustomPID A_0)
				{
					if (A_0 != null)
					{
						return new ValueTuple<double, bool>(A_0.Multiplier, true);
					}
					return default(ValueTuple<double, bool>);
				}, null, new Tuple<Func<CustomPID, object>, string>[]
				{
					new Tuple<Func<CustomPID, object>, string>((CustomPID A_0) => A_0, "Multiplier")
				});
				BindingBase bindingBase16 = bindingExtension16.ProvideValue(null);
				span17.SetBinding(Span.TextProperty, bindingBase16);
				formattedString2.Spans.Add(span17);
				span18.SetValue(Span.TextProperty, "/");
				formattedString2.Spans.Add(span18);
				bindingExtension17.Mode = 2;
				bindingExtension17.Path = "Divider";
				bindingExtension17.TypedBinding = new TypedBinding<CustomPID, double>(delegate(CustomPID A_0)
				{
					if (A_0 != null)
					{
						return new ValueTuple<double, bool>(A_0.Divider, true);
					}
					return default(ValueTuple<double, bool>);
				}, null, new Tuple<Func<CustomPID, object>, string>[]
				{
					new Tuple<Func<CustomPID, object>, string>((CustomPID A_0) => A_0, "Divider")
				});
				BindingBase bindingBase17 = bindingExtension17.ProvideValue(null);
				span19.SetBinding(Span.TextProperty, bindingBase17);
				formattedString2.Spans.Add(span19);
				span20.SetValue(Span.TextProperty, "+");
				formattedString2.Spans.Add(span20);
				bindingExtension18.Mode = 2;
				bindingExtension18.Path = "Offset";
				bindingExtension18.TypedBinding = new TypedBinding<CustomPID, double>(delegate(CustomPID A_0)
				{
					if (A_0 != null)
					{
						return new ValueTuple<double, bool>(A_0.Offset, true);
					}
					return default(ValueTuple<double, bool>);
				}, null, new Tuple<Func<CustomPID, object>, string>[]
				{
					new Tuple<Func<CustomPID, object>, string>((CustomPID A_0) => A_0, "Offset")
				});
				BindingBase bindingBase18 = bindingExtension18.ProvideValue(null);
				span21.SetBinding(Span.TextProperty, bindingBase18);
				formattedString2.Spans.Add(span21);
				label5.SetValue(Label.FormattedTextProperty, formattedString2);
				stackLayout2.Children.Add(label5);
				stackLayout5.Children.Add(stackLayout2);
				bindingExtension19.Mode = 2;
				staticResourceExtension6.Key = "EnumValueToTrueConverter";
				IMarkupExtension markupExtension16 = staticResourceExtension6;
				XamlServiceProvider xamlServiceProvider16 = new XamlServiceProvider();
				Type typeFromHandle31 = typeof(IProvideValueTarget);
				int num16;
				object[] array31 = new object[(num16 = this.parentValues.Length) + 5];
				Array.Copy(this.parentValues, 0, array31, 5, num16);
				object[] array32 = array31;
				array32[0] = bindingExtension19;
				array32[1] = stackLayout3;
				array32[2] = stackLayout5;
				array32[3] = grid;
				array32[4] = viewCell;
				object obj26;
				xamlServiceProvider16.Add(typeFromHandle31, obj26 = new SimpleValueTargetProvider(array32, typeof(BindingExtension).GetRuntimeProperty("Converter"), nameScope));
				xamlServiceProvider16.Add(typeof(IReferenceProvider), obj26);
				Type typeFromHandle32 = typeof(IXamlTypeResolver);
				XmlNamespaceResolver xmlNamespaceResolver16 = new XmlNamespaceResolver();
				xmlNamespaceResolver16.Add("", "http://xamarin.com/schemas/2014/forms");
				xmlNamespaceResolver16.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
				xmlNamespaceResolver16.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
				xmlNamespaceResolver16.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
				xmlNamespaceResolver16.Add("local", "clr-namespace:CarScannerXamarinForms");
				xmlNamespaceResolver16.Add("pids", "clr-namespace:CarScannerXamarinForms.OBD2.PIDS");
				xmlNamespaceResolver16.Add("settings", "clr-namespace:CarScannerXamarinForms.Settings");
				xamlServiceProvider16.Add(typeFromHandle32, new XamlTypeResolver(xmlNamespaceResolver16, typeof(SettingsPIDOverrideListPage.<InitializeComponent>_anonXamlCDataTemplate_95).GetTypeInfo().Assembly));
				xamlServiceProvider16.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(99, 42)));
				object obj27 = markupExtension16.ProvideValue(xamlServiceProvider16);
				bindingExtension19.Converter = obj27;
				bindingExtension19.ConverterParameter = "2";
				bindingExtension19.Path = "Type";
				bindingExtension19.TypedBinding = new TypedBinding<CustomPID, CustomPIDType>(delegate(CustomPID A_0)
				{
					if (A_0 != null)
					{
						return new ValueTuple<CustomPIDType, bool>(A_0.Type, true);
					}
					return default(ValueTuple<CustomPIDType, bool>);
				}, null, new Tuple<Func<CustomPID, object>, string>[]
				{
					new Tuple<Func<CustomPID, object>, string>((CustomPID A_0) => A_0, "Type")
				});
				BindingBase bindingBase19 = bindingExtension19.ProvideValue(null);
				stackLayout3.SetBinding(VisualElement.IsVisibleProperty, bindingBase19);
				stackLayout3.SetValue(StackLayout.OrientationProperty, 0);
				dynamicResourceExtension6.Key = "BaseFontSize-";
				IMarkupExtension<DynamicResource> markupExtension17 = dynamicResourceExtension6;
				XamlServiceProvider xamlServiceProvider17 = new XamlServiceProvider();
				Type typeFromHandle33 = typeof(IProvideValueTarget);
				int num17;
				object[] array33 = new object[(num17 = this.parentValues.Length) + 5];
				Array.Copy(this.parentValues, 0, array33, 5, num17);
				object[] array34 = array33;
				array34[0] = label6;
				array34[1] = stackLayout3;
				array34[2] = stackLayout5;
				array34[3] = grid;
				array34[4] = viewCell;
				object obj28;
				xamlServiceProvider17.Add(typeFromHandle33, obj28 = new SimpleValueTargetProvider(array34, Label.FontSizeProperty, nameScope));
				xamlServiceProvider17.Add(typeof(IReferenceProvider), obj28);
				Type typeFromHandle34 = typeof(IXamlTypeResolver);
				XmlNamespaceResolver xmlNamespaceResolver17 = new XmlNamespaceResolver();
				xmlNamespaceResolver17.Add("", "http://xamarin.com/schemas/2014/forms");
				xmlNamespaceResolver17.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
				xmlNamespaceResolver17.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
				xmlNamespaceResolver17.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
				xmlNamespaceResolver17.Add("local", "clr-namespace:CarScannerXamarinForms");
				xmlNamespaceResolver17.Add("pids", "clr-namespace:CarScannerXamarinForms.OBD2.PIDS");
				xmlNamespaceResolver17.Add("settings", "clr-namespace:CarScannerXamarinForms.Settings");
				xamlServiceProvider17.Add(typeFromHandle34, new XamlTypeResolver(xmlNamespaceResolver17, typeof(SettingsPIDOverrideListPage.<InitializeComponent>_anonXamlCDataTemplate_95).GetTypeInfo().Assembly));
				xamlServiceProvider17.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(100, 40)));
				DynamicResource dynamicResource6 = markupExtension17.ProvideValue(xamlServiceProvider17);
				label6.SetDynamicResource(Label.FontSizeProperty, dynamicResource6.Key);
				translate6.Text = "ios_Byte";
				IMarkupExtension markupExtension18 = translate6;
				XamlServiceProvider xamlServiceProvider18 = new XamlServiceProvider();
				Type typeFromHandle35 = typeof(IProvideValueTarget);
				int num18;
				object[] array35 = new object[(num18 = this.parentValues.Length) + 7];
				Array.Copy(this.parentValues, 0, array35, 7, num18);
				object[] array36 = array35;
				array36[0] = span22;
				array36[1] = formattedString3;
				array36[2] = label6;
				array36[3] = stackLayout3;
				array36[4] = stackLayout5;
				array36[5] = grid;
				array36[6] = viewCell;
				object obj29;
				xamlServiceProvider18.Add(typeFromHandle35, obj29 = new SimpleValueTargetProvider(array36, Span.TextProperty, nameScope));
				xamlServiceProvider18.Add(typeof(IReferenceProvider), obj29);
				Type typeFromHandle36 = typeof(IXamlTypeResolver);
				XmlNamespaceResolver xmlNamespaceResolver18 = new XmlNamespaceResolver();
				xmlNamespaceResolver18.Add("", "http://xamarin.com/schemas/2014/forms");
				xmlNamespaceResolver18.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
				xmlNamespaceResolver18.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
				xmlNamespaceResolver18.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
				xmlNamespaceResolver18.Add("local", "clr-namespace:CarScannerXamarinForms");
				xmlNamespaceResolver18.Add("pids", "clr-namespace:CarScannerXamarinForms.OBD2.PIDS");
				xmlNamespaceResolver18.Add("settings", "clr-namespace:CarScannerXamarinForms.Settings");
				xamlServiceProvider18.Add(typeFromHandle36, new XamlTypeResolver(xmlNamespaceResolver18, typeof(SettingsPIDOverrideListPage.<InitializeComponent>_anonXamlCDataTemplate_95).GetTypeInfo().Assembly));
				xamlServiceProvider18.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(103, 51)));
				object obj30 = markupExtension18.ProvideValue(xamlServiceProvider18);
				span22.Text = obj30;
				formattedString3.Spans.Add(span22);
				bindingExtension20.Mode = 2;
				bindingExtension20.Path = "StartByteId";
				bindingExtension20.TypedBinding = new TypedBinding<CustomPID, int>(delegate(CustomPID A_0)
				{
					if (A_0 != null)
					{
						return new ValueTuple<int, bool>(A_0.StartByteId, true);
					}
					return default(ValueTuple<int, bool>);
				}, null, new Tuple<Func<CustomPID, object>, string>[]
				{
					new Tuple<Func<CustomPID, object>, string>((CustomPID A_0) => A_0, "StartByteId")
				});
				BindingBase bindingBase20 = bindingExtension20.ProvideValue(null);
				span23.SetBinding(Span.TextProperty, bindingBase20);
				formattedString3.Spans.Add(span23);
				span24.SetValue(Span.TextProperty, ", ");
				formattedString3.Spans.Add(span24);
				translate7.Text = "ios_Bit";
				IMarkupExtension markupExtension19 = translate7;
				XamlServiceProvider xamlServiceProvider19 = new XamlServiceProvider();
				Type typeFromHandle37 = typeof(IProvideValueTarget);
				int num19;
				object[] array37 = new object[(num19 = this.parentValues.Length) + 7];
				Array.Copy(this.parentValues, 0, array37, 7, num19);
				object[] array38 = array37;
				array38[0] = span25;
				array38[1] = formattedString3;
				array38[2] = label6;
				array38[3] = stackLayout3;
				array38[4] = stackLayout5;
				array38[5] = grid;
				array38[6] = viewCell;
				object obj31;
				xamlServiceProvider19.Add(typeFromHandle37, obj31 = new SimpleValueTargetProvider(array38, Span.TextProperty, nameScope));
				xamlServiceProvider19.Add(typeof(IReferenceProvider), obj31);
				Type typeFromHandle38 = typeof(IXamlTypeResolver);
				XmlNamespaceResolver xmlNamespaceResolver19 = new XmlNamespaceResolver();
				xmlNamespaceResolver19.Add("", "http://xamarin.com/schemas/2014/forms");
				xmlNamespaceResolver19.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
				xmlNamespaceResolver19.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
				xmlNamespaceResolver19.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
				xmlNamespaceResolver19.Add("local", "clr-namespace:CarScannerXamarinForms");
				xmlNamespaceResolver19.Add("pids", "clr-namespace:CarScannerXamarinForms.OBD2.PIDS");
				xmlNamespaceResolver19.Add("settings", "clr-namespace:CarScannerXamarinForms.Settings");
				xamlServiceProvider19.Add(typeFromHandle38, new XamlTypeResolver(xmlNamespaceResolver19, typeof(SettingsPIDOverrideListPage.<InitializeComponent>_anonXamlCDataTemplate_95).GetTypeInfo().Assembly));
				xamlServiceProvider19.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(106, 51)));
				object obj32 = markupExtension19.ProvideValue(xamlServiceProvider19);
				span25.Text = obj32;
				formattedString3.Spans.Add(span25);
				bindingExtension21.Mode = 2;
				bindingExtension21.Path = "Bit";
				bindingExtension21.TypedBinding = new TypedBinding<CustomPID, int>(delegate(CustomPID A_0)
				{
					if (A_0 != null)
					{
						return new ValueTuple<int, bool>(A_0.Bit, true);
					}
					return default(ValueTuple<int, bool>);
				}, null, new Tuple<Func<CustomPID, object>, string>[]
				{
					new Tuple<Func<CustomPID, object>, string>((CustomPID A_0) => A_0, "Bit")
				});
				BindingBase bindingBase21 = bindingExtension21.ProvideValue(null);
				span26.SetBinding(Span.TextProperty, bindingBase21);
				formattedString3.Spans.Add(span26);
				label6.SetValue(Label.FormattedTextProperty, formattedString3);
				stackLayout3.Children.Add(label6);
				stackLayout5.Children.Add(stackLayout3);
				bindingExtension22.Mode = 2;
				staticResourceExtension7.Key = "EnumValueToTrueConverter";
				IMarkupExtension markupExtension20 = staticResourceExtension7;
				XamlServiceProvider xamlServiceProvider20 = new XamlServiceProvider();
				Type typeFromHandle39 = typeof(IProvideValueTarget);
				int num20;
				object[] array39 = new object[(num20 = this.parentValues.Length) + 5];
				Array.Copy(this.parentValues, 0, array39, 5, num20);
				object[] array40 = array39;
				array40[0] = bindingExtension22;
				array40[1] = stackLayout4;
				array40[2] = stackLayout5;
				array40[3] = grid;
				array40[4] = viewCell;
				object obj33;
				xamlServiceProvider20.Add(typeFromHandle39, obj33 = new SimpleValueTargetProvider(array40, typeof(BindingExtension).GetRuntimeProperty("Converter"), nameScope));
				xamlServiceProvider20.Add(typeof(IReferenceProvider), obj33);
				Type typeFromHandle40 = typeof(IXamlTypeResolver);
				XmlNamespaceResolver xmlNamespaceResolver20 = new XmlNamespaceResolver();
				xmlNamespaceResolver20.Add("", "http://xamarin.com/schemas/2014/forms");
				xmlNamespaceResolver20.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
				xmlNamespaceResolver20.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
				xmlNamespaceResolver20.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
				xmlNamespaceResolver20.Add("local", "clr-namespace:CarScannerXamarinForms");
				xmlNamespaceResolver20.Add("pids", "clr-namespace:CarScannerXamarinForms.OBD2.PIDS");
				xmlNamespaceResolver20.Add("settings", "clr-namespace:CarScannerXamarinForms.Settings");
				xamlServiceProvider20.Add(typeFromHandle40, new XamlTypeResolver(xmlNamespaceResolver20, typeof(SettingsPIDOverrideListPage.<InitializeComponent>_anonXamlCDataTemplate_95).GetTypeInfo().Assembly));
				xamlServiceProvider20.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(113, 42)));
				object obj34 = markupExtension20.ProvideValue(xamlServiceProvider20);
				bindingExtension22.Converter = obj34;
				bindingExtension22.ConverterParameter = "3";
				bindingExtension22.Path = "Type";
				bindingExtension22.TypedBinding = new TypedBinding<CustomPID, CustomPIDType>(delegate(CustomPID A_0)
				{
					if (A_0 != null)
					{
						return new ValueTuple<CustomPIDType, bool>(A_0.Type, true);
					}
					return default(ValueTuple<CustomPIDType, bool>);
				}, null, new Tuple<Func<CustomPID, object>, string>[]
				{
					new Tuple<Func<CustomPID, object>, string>((CustomPID A_0) => A_0, "Type")
				});
				BindingBase bindingBase22 = bindingExtension22.ProvideValue(null);
				stackLayout4.SetBinding(VisualElement.IsVisibleProperty, bindingBase22);
				stackLayout4.SetValue(StackLayout.OrientationProperty, 0);
				dynamicResourceExtension7.Key = "BaseFontSize-";
				IMarkupExtension<DynamicResource> markupExtension21 = dynamicResourceExtension7;
				XamlServiceProvider xamlServiceProvider21 = new XamlServiceProvider();
				Type typeFromHandle41 = typeof(IProvideValueTarget);
				int num21;
				object[] array41 = new object[(num21 = this.parentValues.Length) + 5];
				Array.Copy(this.parentValues, 0, array41, 5, num21);
				object[] array42 = array41;
				array42[0] = label7;
				array42[1] = stackLayout4;
				array42[2] = stackLayout5;
				array42[3] = grid;
				array42[4] = viewCell;
				object obj35;
				xamlServiceProvider21.Add(typeFromHandle41, obj35 = new SimpleValueTargetProvider(array42, Label.FontSizeProperty, nameScope));
				xamlServiceProvider21.Add(typeof(IReferenceProvider), obj35);
				Type typeFromHandle42 = typeof(IXamlTypeResolver);
				XmlNamespaceResolver xmlNamespaceResolver21 = new XmlNamespaceResolver();
				xmlNamespaceResolver21.Add("", "http://xamarin.com/schemas/2014/forms");
				xmlNamespaceResolver21.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
				xmlNamespaceResolver21.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
				xmlNamespaceResolver21.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
				xmlNamespaceResolver21.Add("local", "clr-namespace:CarScannerXamarinForms");
				xmlNamespaceResolver21.Add("pids", "clr-namespace:CarScannerXamarinForms.OBD2.PIDS");
				xmlNamespaceResolver21.Add("settings", "clr-namespace:CarScannerXamarinForms.Settings");
				xamlServiceProvider21.Add(typeFromHandle42, new XamlTypeResolver(xmlNamespaceResolver21, typeof(SettingsPIDOverrideListPage.<InitializeComponent>_anonXamlCDataTemplate_95).GetTypeInfo().Assembly));
				xamlServiceProvider21.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(114, 40)));
				DynamicResource dynamicResource7 = markupExtension21.ProvideValue(xamlServiceProvider21);
				label7.SetDynamicResource(Label.FontSizeProperty, dynamicResource7.Key);
				translate8.Text = "ios_CustomPIDEditor_IsAction";
				IMarkupExtension markupExtension22 = translate8;
				XamlServiceProvider xamlServiceProvider22 = new XamlServiceProvider();
				Type typeFromHandle43 = typeof(IProvideValueTarget);
				int num22;
				object[] array43 = new object[(num22 = this.parentValues.Length) + 5];
				Array.Copy(this.parentValues, 0, array43, 5, num22);
				object[] array44 = array43;
				array44[0] = label7;
				array44[1] = stackLayout4;
				array44[2] = stackLayout5;
				array44[3] = grid;
				array44[4] = viewCell;
				object obj36;
				xamlServiceProvider22.Add(typeFromHandle43, obj36 = new SimpleValueTargetProvider(array44, Label.TextProperty, nameScope));
				xamlServiceProvider22.Add(typeof(IReferenceProvider), obj36);
				Type typeFromHandle44 = typeof(IXamlTypeResolver);
				XmlNamespaceResolver xmlNamespaceResolver22 = new XmlNamespaceResolver();
				xmlNamespaceResolver22.Add("", "http://xamarin.com/schemas/2014/forms");
				xmlNamespaceResolver22.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
				xmlNamespaceResolver22.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
				xmlNamespaceResolver22.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
				xmlNamespaceResolver22.Add("local", "clr-namespace:CarScannerXamarinForms");
				xmlNamespaceResolver22.Add("pids", "clr-namespace:CarScannerXamarinForms.OBD2.PIDS");
				xmlNamespaceResolver22.Add("settings", "clr-namespace:CarScannerXamarinForms.Settings");
				xamlServiceProvider22.Add(typeFromHandle44, new XamlTypeResolver(xmlNamespaceResolver22, typeof(SettingsPIDOverrideListPage.<InitializeComponent>_anonXamlCDataTemplate_95).GetTypeInfo().Assembly));
				xamlServiceProvider22.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(114, 83)));
				object obj37 = markupExtension22.ProvideValue(xamlServiceProvider22);
				label7.Text = obj37;
				stackLayout4.Children.Add(label7);
				stackLayout5.Children.Add(stackLayout4);
				grid.Children.Add(stackLayout5);
				label8.SetValue(Grid.RowProperty, 2);
				dynamicResourceExtension8.Key = "BaseFontSize-";
				IMarkupExtension<DynamicResource> markupExtension23 = dynamicResourceExtension8;
				XamlServiceProvider xamlServiceProvider23 = new XamlServiceProvider();
				Type typeFromHandle45 = typeof(IProvideValueTarget);
				int num23;
				object[] array45 = new object[(num23 = this.parentValues.Length) + 3];
				Array.Copy(this.parentValues, 0, array45, 3, num23);
				object[] array46 = array45;
				array46[0] = label8;
				array46[1] = grid;
				array46[2] = viewCell;
				object obj38;
				xamlServiceProvider23.Add(typeFromHandle45, obj38 = new SimpleValueTargetProvider(array46, Label.FontSizeProperty, nameScope));
				xamlServiceProvider23.Add(typeof(IReferenceProvider), obj38);
				Type typeFromHandle46 = typeof(IXamlTypeResolver);
				XmlNamespaceResolver xmlNamespaceResolver23 = new XmlNamespaceResolver();
				xmlNamespaceResolver23.Add("", "http://xamarin.com/schemas/2014/forms");
				xmlNamespaceResolver23.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
				xmlNamespaceResolver23.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
				xmlNamespaceResolver23.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
				xmlNamespaceResolver23.Add("local", "clr-namespace:CarScannerXamarinForms");
				xmlNamespaceResolver23.Add("pids", "clr-namespace:CarScannerXamarinForms.OBD2.PIDS");
				xmlNamespaceResolver23.Add("settings", "clr-namespace:CarScannerXamarinForms.Settings");
				xamlServiceProvider23.Add(typeFromHandle46, new XamlTypeResolver(xmlNamespaceResolver23, typeof(SettingsPIDOverrideListPage.<InitializeComponent>_anonXamlCDataTemplate_95).GetTypeInfo().Assembly));
				xamlServiceProvider23.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(118, 45)));
				DynamicResource dynamicResource8 = markupExtension23.ProvideValue(xamlServiceProvider23);
				label8.SetDynamicResource(Label.FontSizeProperty, dynamicResource8.Key);
				translate9.Text = "ios_Priority";
				IMarkupExtension markupExtension24 = translate9;
				XamlServiceProvider xamlServiceProvider24 = new XamlServiceProvider();
				Type typeFromHandle47 = typeof(IProvideValueTarget);
				int num24;
				object[] array47 = new object[(num24 = this.parentValues.Length) + 5];
				Array.Copy(this.parentValues, 0, array47, 5, num24);
				object[] array48 = array47;
				array48[0] = span27;
				array48[1] = formattedString4;
				array48[2] = label8;
				array48[3] = grid;
				array48[4] = viewCell;
				object obj39;
				xamlServiceProvider24.Add(typeFromHandle47, obj39 = new SimpleValueTargetProvider(array48, Span.TextProperty, nameScope));
				xamlServiceProvider24.Add(typeof(IReferenceProvider), obj39);
				Type typeFromHandle48 = typeof(IXamlTypeResolver);
				XmlNamespaceResolver xmlNamespaceResolver24 = new XmlNamespaceResolver();
				xmlNamespaceResolver24.Add("", "http://xamarin.com/schemas/2014/forms");
				xmlNamespaceResolver24.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
				xmlNamespaceResolver24.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
				xmlNamespaceResolver24.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
				xmlNamespaceResolver24.Add("local", "clr-namespace:CarScannerXamarinForms");
				xmlNamespaceResolver24.Add("pids", "clr-namespace:CarScannerXamarinForms.OBD2.PIDS");
				xmlNamespaceResolver24.Add("settings", "clr-namespace:CarScannerXamarinForms.Settings");
				xamlServiceProvider24.Add(typeFromHandle48, new XamlTypeResolver(xmlNamespaceResolver24, typeof(SettingsPIDOverrideListPage.<InitializeComponent>_anonXamlCDataTemplate_95).GetTypeInfo().Assembly));
				xamlServiceProvider24.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(121, 43)));
				object obj40 = markupExtension24.ProvideValue(xamlServiceProvider24);
				span27.Text = obj40;
				formattedString4.Spans.Add(span27);
				bindingExtension23.Mode = 2;
				staticResourceExtension8.Key = "SkipCyclesIntToPriorityStringConverter";
				IMarkupExtension markupExtension25 = staticResourceExtension8;
				XamlServiceProvider xamlServiceProvider25 = new XamlServiceProvider();
				Type typeFromHandle49 = typeof(IProvideValueTarget);
				int num25;
				object[] array49 = new object[(num25 = this.parentValues.Length) + 6];
				Array.Copy(this.parentValues, 0, array49, 6, num25);
				object[] array50 = array49;
				array50[0] = bindingExtension23;
				array50[1] = span28;
				array50[2] = formattedString4;
				array50[3] = label8;
				array50[4] = grid;
				array50[5] = viewCell;
				object obj41;
				xamlServiceProvider25.Add(typeFromHandle49, obj41 = new SimpleValueTargetProvider(array50, typeof(BindingExtension).GetRuntimeProperty("Converter"), nameScope));
				xamlServiceProvider25.Add(typeof(IReferenceProvider), obj41);
				Type typeFromHandle50 = typeof(IXamlTypeResolver);
				XmlNamespaceResolver xmlNamespaceResolver25 = new XmlNamespaceResolver();
				xmlNamespaceResolver25.Add("", "http://xamarin.com/schemas/2014/forms");
				xmlNamespaceResolver25.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
				xmlNamespaceResolver25.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
				xmlNamespaceResolver25.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
				xmlNamespaceResolver25.Add("local", "clr-namespace:CarScannerXamarinForms");
				xmlNamespaceResolver25.Add("pids", "clr-namespace:CarScannerXamarinForms.OBD2.PIDS");
				xmlNamespaceResolver25.Add("settings", "clr-namespace:CarScannerXamarinForms.Settings");
				xamlServiceProvider25.Add(typeFromHandle50, new XamlTypeResolver(xmlNamespaceResolver25, typeof(SettingsPIDOverrideListPage.<InitializeComponent>_anonXamlCDataTemplate_95).GetTypeInfo().Assembly));
				xamlServiceProvider25.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(122, 43)));
				object obj42 = markupExtension25.ProvideValue(xamlServiceProvider25);
				bindingExtension23.Converter = obj42;
				bindingExtension23.Path = "SkipCycles";
				bindingExtension23.TypedBinding = new TypedBinding<CustomPID, int>(delegate(CustomPID A_0)
				{
					if (A_0 != null)
					{
						return new ValueTuple<int, bool>(A_0.SkipCycles, true);
					}
					return default(ValueTuple<int, bool>);
				}, null, new Tuple<Func<CustomPID, object>, string>[]
				{
					new Tuple<Func<CustomPID, object>, string>((CustomPID A_0) => A_0, "SkipCycles")
				});
				BindingBase bindingBase23 = bindingExtension23.ProvideValue(null);
				span28.SetBinding(Span.TextProperty, bindingBase23);
				formattedString4.Spans.Add(span28);
				label8.SetValue(Label.FormattedTextProperty, formattedString4);
				grid.Children.Add(label8);
				viewCell.View = grid;
				return viewCell;
			}

			// Token: 0x06001B7D RID: 7037 RVA: 0x0013A250 File Offset: 0x00138450
			[CompilerGenerated]
			private static ValueTuple<CustomPID, bool> <LoadDataTemplate>typedBindingsM__1537(CustomPID A_0)
			{
				return new ValueTuple<CustomPID, bool>(A_0, true);
			}

			// Token: 0x06001B7E RID: 7038 RVA: 0x0013A264 File Offset: 0x00138464
			[CompilerGenerated]
			private static ValueTuple<string, bool> <LoadDataTemplate>typedBindingsM__1538(CustomPID A_0)
			{
				if (A_0 != null)
				{
					return new ValueTuple<string, bool>(A_0.Name, true);
				}
				return default(ValueTuple<string, bool>);
			}

			// Token: 0x06001B7F RID: 7039 RVA: 0x0013A294 File Offset: 0x00138494
			[CompilerGenerated]
			private static object <LoadDataTemplate>typedBindingsM__1539(CustomPID A_0)
			{
				return A_0;
			}

			// Token: 0x06001B80 RID: 7040 RVA: 0x0013A2A4 File Offset: 0x001384A4
			[CompilerGenerated]
			private static ValueTuple<bool, bool> <LoadDataTemplate>typedBindingsM__1540(CustomPID A_0)
			{
				if (A_0 != null)
				{
					return new ValueTuple<bool, bool>(A_0.IsFormulaHidden, true);
				}
				return default(ValueTuple<bool, bool>);
			}

			// Token: 0x06001B81 RID: 7041 RVA: 0x0013A2D4 File Offset: 0x001384D4
			[CompilerGenerated]
			private static object <LoadDataTemplate>typedBindingsM__1541(CustomPID A_0)
			{
				return A_0;
			}

			// Token: 0x06001B82 RID: 7042 RVA: 0x0013A2E4 File Offset: 0x001384E4
			[CompilerGenerated]
			private static ValueTuple<int, bool> <LoadDataTemplate>typedBindingsM__1542(CustomPID A_0)
			{
				if (A_0 != null)
				{
					return new ValueTuple<int, bool>(A_0.Id, true);
				}
				return default(ValueTuple<int, bool>);
			}

			// Token: 0x06001B83 RID: 7043 RVA: 0x0013A314 File Offset: 0x00138514
			[CompilerGenerated]
			private static object <LoadDataTemplate>typedBindingsM__1543(CustomPID A_0)
			{
				return A_0;
			}

			// Token: 0x06001B84 RID: 7044 RVA: 0x0013A324 File Offset: 0x00138524
			[CompilerGenerated]
			private static ValueTuple<string, bool> <LoadDataTemplate>typedBindingsM__1544(CustomPID A_0)
			{
				if (A_0 != null)
				{
					return new ValueTuple<string, bool>(A_0.Header, true);
				}
				return default(ValueTuple<string, bool>);
			}

			// Token: 0x06001B85 RID: 7045 RVA: 0x0013A354 File Offset: 0x00138554
			[CompilerGenerated]
			private static object <LoadDataTemplate>typedBindingsM__1545(CustomPID A_0)
			{
				return A_0;
			}

			// Token: 0x06001B86 RID: 7046 RVA: 0x0013A364 File Offset: 0x00138564
			[CompilerGenerated]
			private static ValueTuple<string, bool> <LoadDataTemplate>typedBindingsM__1546(CustomPID A_0)
			{
				if (A_0 != null)
				{
					return new ValueTuple<string, bool>(A_0.Command, true);
				}
				return default(ValueTuple<string, bool>);
			}

			// Token: 0x06001B87 RID: 7047 RVA: 0x0013A394 File Offset: 0x00138594
			[CompilerGenerated]
			private static object <LoadDataTemplate>typedBindingsM__1547(CustomPID A_0)
			{
				return A_0;
			}

			// Token: 0x06001B88 RID: 7048 RVA: 0x0013A3A4 File Offset: 0x001385A4
			[CompilerGenerated]
			private static ValueTuple<UnitsHelper.Units, bool> <LoadDataTemplate>typedBindingsM__1548(CustomPID A_0)
			{
				if (A_0 != null)
				{
					return new ValueTuple<UnitsHelper.Units, bool>(A_0.Units, true);
				}
				return default(ValueTuple<UnitsHelper.Units, bool>);
			}

			// Token: 0x06001B89 RID: 7049 RVA: 0x0013A3D4 File Offset: 0x001385D4
			[CompilerGenerated]
			private static object <LoadDataTemplate>typedBindingsM__1549(CustomPID A_0)
			{
				return A_0;
			}

			// Token: 0x06001B8A RID: 7050 RVA: 0x0013A3E4 File Offset: 0x001385E4
			[CompilerGenerated]
			private static ValueTuple<CustomPIDType, bool> <LoadDataTemplate>typedBindingsM__1550(CustomPID A_0)
			{
				if (A_0 != null)
				{
					return new ValueTuple<CustomPIDType, bool>(A_0.Type, true);
				}
				return default(ValueTuple<CustomPIDType, bool>);
			}

			// Token: 0x06001B8B RID: 7051 RVA: 0x0013A414 File Offset: 0x00138614
			[CompilerGenerated]
			private static object <LoadDataTemplate>typedBindingsM__1551(CustomPID A_0)
			{
				return A_0;
			}

			// Token: 0x06001B8C RID: 7052 RVA: 0x0013A424 File Offset: 0x00138624
			[CompilerGenerated]
			private static ValueTuple<bool, bool> <LoadDataTemplate>typedBindingsM__1552(CustomPID A_0)
			{
				if (A_0 != null)
				{
					return new ValueTuple<bool, bool>(A_0.IsFormulaCorrect, true);
				}
				return default(ValueTuple<bool, bool>);
			}

			// Token: 0x06001B8D RID: 7053 RVA: 0x0013A454 File Offset: 0x00138654
			[CompilerGenerated]
			private static object <LoadDataTemplate>typedBindingsM__1553(CustomPID A_0)
			{
				return A_0;
			}

			// Token: 0x06001B8E RID: 7054 RVA: 0x0013A464 File Offset: 0x00138664
			[CompilerGenerated]
			private static ValueTuple<string, bool> <LoadDataTemplate>typedBindingsM__1554(CustomPID A_0)
			{
				if (A_0 != null)
				{
					return new ValueTuple<string, bool>(A_0.Formula, true);
				}
				return default(ValueTuple<string, bool>);
			}

			// Token: 0x06001B8F RID: 7055 RVA: 0x0013A494 File Offset: 0x00138694
			[CompilerGenerated]
			private static void <LoadDataTemplate>typedBindingsM__1555(CustomPID A_0, string A_1)
			{
				if (A_0 != null)
				{
					A_0.Formula = A_1;
					return;
				}
			}

			// Token: 0x06001B90 RID: 7056 RVA: 0x0013A4B0 File Offset: 0x001386B0
			[CompilerGenerated]
			private static object <LoadDataTemplate>typedBindingsM__1556(CustomPID A_0)
			{
				return A_0;
			}

			// Token: 0x06001B91 RID: 7057 RVA: 0x0013A4C0 File Offset: 0x001386C0
			[CompilerGenerated]
			private static ValueTuple<bool, bool> <LoadDataTemplate>typedBindingsM__1557(CustomPID A_0)
			{
				if (A_0 != null)
				{
					return new ValueTuple<bool, bool>(A_0.IsFormulaCorrect, true);
				}
				return default(ValueTuple<bool, bool>);
			}

			// Token: 0x06001B92 RID: 7058 RVA: 0x0013A4F0 File Offset: 0x001386F0
			[CompilerGenerated]
			private static object <LoadDataTemplate>typedBindingsM__1558(CustomPID A_0)
			{
				return A_0;
			}

			// Token: 0x06001B93 RID: 7059 RVA: 0x0013A500 File Offset: 0x00138700
			[CompilerGenerated]
			private static ValueTuple<string, bool> <LoadDataTemplate>typedBindingsM__1559(CustomPID A_0)
			{
				if (A_0 != null)
				{
					return new ValueTuple<string, bool>(A_0.Formula, true);
				}
				return default(ValueTuple<string, bool>);
			}

			// Token: 0x06001B94 RID: 7060 RVA: 0x0013A530 File Offset: 0x00138730
			[CompilerGenerated]
			private static void <LoadDataTemplate>typedBindingsM__1560(CustomPID A_0, string A_1)
			{
				if (A_0 != null)
				{
					A_0.Formula = A_1;
					return;
				}
			}

			// Token: 0x06001B95 RID: 7061 RVA: 0x0013A54C File Offset: 0x0013874C
			[CompilerGenerated]
			private static object <LoadDataTemplate>typedBindingsM__1561(CustomPID A_0)
			{
				return A_0;
			}

			// Token: 0x06001B96 RID: 7062 RVA: 0x0013A55C File Offset: 0x0013875C
			[CompilerGenerated]
			private static ValueTuple<CustomPIDType, bool> <LoadDataTemplate>typedBindingsM__1562(CustomPID A_0)
			{
				if (A_0 != null)
				{
					return new ValueTuple<CustomPIDType, bool>(A_0.Type, true);
				}
				return default(ValueTuple<CustomPIDType, bool>);
			}

			// Token: 0x06001B97 RID: 7063 RVA: 0x0013A58C File Offset: 0x0013878C
			[CompilerGenerated]
			private static object <LoadDataTemplate>typedBindingsM__1563(CustomPID A_0)
			{
				return A_0;
			}

			// Token: 0x06001B98 RID: 7064 RVA: 0x0013A59C File Offset: 0x0013879C
			[CompilerGenerated]
			private static ValueTuple<int, bool> <LoadDataTemplate>typedBindingsM__1564(CustomPID A_0)
			{
				if (A_0 != null)
				{
					return new ValueTuple<int, bool>(A_0.StartByteId, true);
				}
				return default(ValueTuple<int, bool>);
			}

			// Token: 0x06001B99 RID: 7065 RVA: 0x0013A5CC File Offset: 0x001387CC
			[CompilerGenerated]
			private static object <LoadDataTemplate>typedBindingsM__1565(CustomPID A_0)
			{
				return A_0;
			}

			// Token: 0x06001B9A RID: 7066 RVA: 0x0013A5DC File Offset: 0x001387DC
			[CompilerGenerated]
			private static ValueTuple<int, bool> <LoadDataTemplate>typedBindingsM__1566(CustomPID A_0)
			{
				if (A_0 != null)
				{
					return new ValueTuple<int, bool>(A_0.DataLength, true);
				}
				return default(ValueTuple<int, bool>);
			}

			// Token: 0x06001B9B RID: 7067 RVA: 0x0013A60C File Offset: 0x0013880C
			[CompilerGenerated]
			private static object <LoadDataTemplate>typedBindingsM__1567(CustomPID A_0)
			{
				return A_0;
			}

			// Token: 0x06001B9C RID: 7068 RVA: 0x0013A61C File Offset: 0x0013881C
			[CompilerGenerated]
			private static ValueTuple<double, bool> <LoadDataTemplate>typedBindingsM__1568(CustomPID A_0)
			{
				if (A_0 != null)
				{
					return new ValueTuple<double, bool>(A_0.Multiplier, true);
				}
				return default(ValueTuple<double, bool>);
			}

			// Token: 0x06001B9D RID: 7069 RVA: 0x0013A64C File Offset: 0x0013884C
			[CompilerGenerated]
			private static object <LoadDataTemplate>typedBindingsM__1569(CustomPID A_0)
			{
				return A_0;
			}

			// Token: 0x06001B9E RID: 7070 RVA: 0x0013A65C File Offset: 0x0013885C
			[CompilerGenerated]
			private static ValueTuple<double, bool> <LoadDataTemplate>typedBindingsM__1570(CustomPID A_0)
			{
				if (A_0 != null)
				{
					return new ValueTuple<double, bool>(A_0.Divider, true);
				}
				return default(ValueTuple<double, bool>);
			}

			// Token: 0x06001B9F RID: 7071 RVA: 0x0013A68C File Offset: 0x0013888C
			[CompilerGenerated]
			private static object <LoadDataTemplate>typedBindingsM__1571(CustomPID A_0)
			{
				return A_0;
			}

			// Token: 0x06001BA0 RID: 7072 RVA: 0x0013A69C File Offset: 0x0013889C
			[CompilerGenerated]
			private static ValueTuple<double, bool> <LoadDataTemplate>typedBindingsM__1572(CustomPID A_0)
			{
				if (A_0 != null)
				{
					return new ValueTuple<double, bool>(A_0.Offset, true);
				}
				return default(ValueTuple<double, bool>);
			}

			// Token: 0x06001BA1 RID: 7073 RVA: 0x0013A6CC File Offset: 0x001388CC
			[CompilerGenerated]
			private static object <LoadDataTemplate>typedBindingsM__1573(CustomPID A_0)
			{
				return A_0;
			}

			// Token: 0x06001BA2 RID: 7074 RVA: 0x0013A6DC File Offset: 0x001388DC
			[CompilerGenerated]
			private static ValueTuple<CustomPIDType, bool> <LoadDataTemplate>typedBindingsM__1574(CustomPID A_0)
			{
				if (A_0 != null)
				{
					return new ValueTuple<CustomPIDType, bool>(A_0.Type, true);
				}
				return default(ValueTuple<CustomPIDType, bool>);
			}

			// Token: 0x06001BA3 RID: 7075 RVA: 0x0013A70C File Offset: 0x0013890C
			[CompilerGenerated]
			private static object <LoadDataTemplate>typedBindingsM__1575(CustomPID A_0)
			{
				return A_0;
			}

			// Token: 0x06001BA4 RID: 7076 RVA: 0x0013A71C File Offset: 0x0013891C
			[CompilerGenerated]
			private static ValueTuple<int, bool> <LoadDataTemplate>typedBindingsM__1576(CustomPID A_0)
			{
				if (A_0 != null)
				{
					return new ValueTuple<int, bool>(A_0.StartByteId, true);
				}
				return default(ValueTuple<int, bool>);
			}

			// Token: 0x06001BA5 RID: 7077 RVA: 0x0013A74C File Offset: 0x0013894C
			[CompilerGenerated]
			private static object <LoadDataTemplate>typedBindingsM__1577(CustomPID A_0)
			{
				return A_0;
			}

			// Token: 0x06001BA6 RID: 7078 RVA: 0x0013A75C File Offset: 0x0013895C
			[CompilerGenerated]
			private static ValueTuple<int, bool> <LoadDataTemplate>typedBindingsM__1578(CustomPID A_0)
			{
				if (A_0 != null)
				{
					return new ValueTuple<int, bool>(A_0.Bit, true);
				}
				return default(ValueTuple<int, bool>);
			}

			// Token: 0x06001BA7 RID: 7079 RVA: 0x0013A78C File Offset: 0x0013898C
			[CompilerGenerated]
			private static object <LoadDataTemplate>typedBindingsM__1579(CustomPID A_0)
			{
				return A_0;
			}

			// Token: 0x06001BA8 RID: 7080 RVA: 0x0013A79C File Offset: 0x0013899C
			[CompilerGenerated]
			private static ValueTuple<CustomPIDType, bool> <LoadDataTemplate>typedBindingsM__1580(CustomPID A_0)
			{
				if (A_0 != null)
				{
					return new ValueTuple<CustomPIDType, bool>(A_0.Type, true);
				}
				return default(ValueTuple<CustomPIDType, bool>);
			}

			// Token: 0x06001BA9 RID: 7081 RVA: 0x0013A7CC File Offset: 0x001389CC
			[CompilerGenerated]
			private static object <LoadDataTemplate>typedBindingsM__1581(CustomPID A_0)
			{
				return A_0;
			}

			// Token: 0x06001BAA RID: 7082 RVA: 0x0013A7DC File Offset: 0x001389DC
			[CompilerGenerated]
			private static ValueTuple<int, bool> <LoadDataTemplate>typedBindingsM__1582(CustomPID A_0)
			{
				if (A_0 != null)
				{
					return new ValueTuple<int, bool>(A_0.SkipCycles, true);
				}
				return default(ValueTuple<int, bool>);
			}

			// Token: 0x06001BAB RID: 7083 RVA: 0x0013A80C File Offset: 0x00138A0C
			[CompilerGenerated]
			private static object <LoadDataTemplate>typedBindingsM__1583(CustomPID A_0)
			{
				return A_0;
			}

			// Token: 0x04000CDF RID: 3295
			internal object[] parentValues;

			// Token: 0x04000CE0 RID: 3296
			internal SettingsPIDOverrideListPage root;
		}

		// Token: 0x0200024E RID: 590
		[CompilerGenerated]
		private sealed class <InitializeComponent>_anonXamlCDataTemplate_96
		{
			// Token: 0x06001BAC RID: 7084 RVA: 0x0013A81C File Offset: 0x00138A1C
			public <InitializeComponent>_anonXamlCDataTemplate_96()
			{
			}

			// Token: 0x06001BAD RID: 7085 RVA: 0x0013A830 File Offset: 0x00138A30
			internal object LoadDataTemplate()
			{
				RowDefinition rowDefinition;
				VisualDiagnostics.RegisterSourceInfo(rowDefinition = new RowDefinition(), new Uri("Settings\\SettingsPIDOverrideListPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 133, 30);
				RowDefinition rowDefinition2;
				VisualDiagnostics.RegisterSourceInfo(rowDefinition2 = new RowDefinition(), new Uri("Settings\\SettingsPIDOverrideListPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 134, 30);
				DynamicResourceExtension dynamicResourceExtension;
				VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension = new DynamicResourceExtension(), new Uri("Settings\\SettingsPIDOverrideListPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 139, 29);
				BindingExtension bindingExtension;
				VisualDiagnostics.RegisterSourceInfo(bindingExtension = new BindingExtension(), new Uri("Settings\\SettingsPIDOverrideListPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 140, 29);
				Label label;
				VisualDiagnostics.RegisterSourceInfo(label = new Label(), new Uri("Settings\\SettingsPIDOverrideListPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 136, 26);
				DynamicResourceExtension dynamicResourceExtension2;
				VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension2 = new DynamicResourceExtension(), new Uri("Settings\\SettingsPIDOverrideListPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 141, 45);
				Span span;
				VisualDiagnostics.RegisterSourceInfo(span = new Span(), new Uri("Settings\\SettingsPIDOverrideListPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 144, 38);
				BindingExtension bindingExtension2;
				VisualDiagnostics.RegisterSourceInfo(bindingExtension2 = new BindingExtension(), new Uri("Settings\\SettingsPIDOverrideListPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 145, 43);
				Span span2;
				VisualDiagnostics.RegisterSourceInfo(span2 = new Span(), new Uri("Settings\\SettingsPIDOverrideListPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 145, 38);
				Span span3;
				VisualDiagnostics.RegisterSourceInfo(span3 = new Span(), new Uri("Settings\\SettingsPIDOverrideListPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 146, 38);
				Translate translate;
				VisualDiagnostics.RegisterSourceInfo(translate = new Translate(), new Uri("Settings\\SettingsPIDOverrideListPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 147, 43);
				Span span4;
				VisualDiagnostics.RegisterSourceInfo(span4 = new Span(), new Uri("Settings\\SettingsPIDOverrideListPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 147, 38);
				StaticResourceExtension staticResourceExtension;
				VisualDiagnostics.RegisterSourceInfo(staticResourceExtension = new StaticResourceExtension(), new Uri("Settings\\SettingsPIDOverrideListPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 148, 43);
				BindingExtension bindingExtension3;
				VisualDiagnostics.RegisterSourceInfo(bindingExtension3 = new BindingExtension(), new Uri("Settings\\SettingsPIDOverrideListPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 148, 43);
				Span span5;
				VisualDiagnostics.RegisterSourceInfo(span5 = new Span(), new Uri("Settings\\SettingsPIDOverrideListPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 148, 38);
				Span span6;
				VisualDiagnostics.RegisterSourceInfo(span6 = new Span(), new Uri("Settings\\SettingsPIDOverrideListPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 149, 38);
				Translate translate2;
				VisualDiagnostics.RegisterSourceInfo(translate2 = new Translate(), new Uri("Settings\\SettingsPIDOverrideListPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 150, 43);
				Span span7;
				VisualDiagnostics.RegisterSourceInfo(span7 = new Span(), new Uri("Settings\\SettingsPIDOverrideListPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 150, 38);
				StaticResourceExtension staticResourceExtension2;
				VisualDiagnostics.RegisterSourceInfo(staticResourceExtension2 = new StaticResourceExtension(), new Uri("Settings\\SettingsPIDOverrideListPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 151, 43);
				BindingExtension bindingExtension4;
				VisualDiagnostics.RegisterSourceInfo(bindingExtension4 = new BindingExtension(), new Uri("Settings\\SettingsPIDOverrideListPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 151, 43);
				Span span8;
				VisualDiagnostics.RegisterSourceInfo(span8 = new Span(), new Uri("Settings\\SettingsPIDOverrideListPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 151, 38);
				Span span9;
				VisualDiagnostics.RegisterSourceInfo(span9 = new Span(), new Uri("Settings\\SettingsPIDOverrideListPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 153, 38);
				Translate translate3;
				VisualDiagnostics.RegisterSourceInfo(translate3 = new Translate(), new Uri("Settings\\SettingsPIDOverrideListPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 154, 43);
				Span span10;
				VisualDiagnostics.RegisterSourceInfo(span10 = new Span(), new Uri("Settings\\SettingsPIDOverrideListPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 154, 38);
				BindingExtension bindingExtension5;
				VisualDiagnostics.RegisterSourceInfo(bindingExtension5 = new BindingExtension(), new Uri("Settings\\SettingsPIDOverrideListPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 155, 43);
				Span span11;
				VisualDiagnostics.RegisterSourceInfo(span11 = new Span(), new Uri("Settings\\SettingsPIDOverrideListPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 155, 38);
				FormattedString formattedString;
				VisualDiagnostics.RegisterSourceInfo(formattedString = new FormattedString(), new Uri("Settings\\SettingsPIDOverrideListPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 143, 34);
				Label label2;
				VisualDiagnostics.RegisterSourceInfo(label2 = new Label(), new Uri("Settings\\SettingsPIDOverrideListPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 141, 26);
				Grid grid;
				VisualDiagnostics.RegisterSourceInfo(grid = new Grid(), new Uri("Settings\\SettingsPIDOverrideListPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 131, 22);
				ViewCell viewCell;
				VisualDiagnostics.RegisterSourceInfo(viewCell = new ViewCell(), new Uri("Settings\\SettingsPIDOverrideListPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 130, 18);
				NameScope nameScope = new NameScope();
				NameScope.SetNameScope(viewCell, nameScope);
				rowDefinition.SetValue(RowDefinition.HeightProperty, new GridLengthTypeConverter().ConvertFromInvariantString("auto"));
				grid.GetValue(Grid.RowDefinitionsProperty).Add(rowDefinition);
				rowDefinition2.SetValue(RowDefinition.HeightProperty, new GridLengthTypeConverter().ConvertFromInvariantString("auto"));
				grid.GetValue(Grid.RowDefinitionsProperty).Add(rowDefinition2);
				label.SetValue(Grid.RowProperty, 0);
				label.SetValue(Label.FontAttributesProperty, new FontAttributesConverter().ConvertFromInvariantString("Bold"));
				dynamicResourceExtension.Key = "BaseFontSize+";
				IMarkupExtension<DynamicResource> markupExtension = dynamicResourceExtension;
				XamlServiceProvider xamlServiceProvider = new XamlServiceProvider();
				Type typeFromHandle = typeof(IProvideValueTarget);
				int num;
				object[] array = new object[(num = this.parentValues.Length) + 3];
				Array.Copy(this.parentValues, 0, array, 3, num);
				object[] array2 = array;
				array2[0] = label;
				array2[1] = grid;
				array2[2] = viewCell;
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
				xmlNamespaceResolver.Add("pids", "clr-namespace:CarScannerXamarinForms.OBD2.PIDS");
				xmlNamespaceResolver.Add("settings", "clr-namespace:CarScannerXamarinForms.Settings");
				xamlServiceProvider.Add(typeFromHandle2, new XamlTypeResolver(xmlNamespaceResolver, typeof(SettingsPIDOverrideListPage.<InitializeComponent>_anonXamlCDataTemplate_96).GetTypeInfo().Assembly));
				xamlServiceProvider.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(139, 29)));
				DynamicResource dynamicResource = markupExtension.ProvideValue(xamlServiceProvider);
				label.SetDynamicResource(Label.FontSizeProperty, dynamicResource.Key);
				bindingExtension.Mode = 2;
				bindingExtension.Path = "Name";
				bindingExtension.TypedBinding = new TypedBinding<PID, string>(delegate(PID A_0)
				{
					if (A_0 != null)
					{
						return new ValueTuple<string, bool>(A_0.Name, true);
					}
					return default(ValueTuple<string, bool>);
				}, null, new Tuple<Func<PID, object>, string>[]
				{
					new Tuple<Func<PID, object>, string>((PID A_0) => A_0, "Name")
				});
				BindingBase bindingBase = bindingExtension.ProvideValue(null);
				label.SetBinding(Label.TextProperty, bindingBase);
				grid.Children.Add(label);
				label2.SetValue(Grid.RowProperty, 1);
				dynamicResourceExtension2.Key = "BaseFontSize-";
				IMarkupExtension<DynamicResource> markupExtension2 = dynamicResourceExtension2;
				XamlServiceProvider xamlServiceProvider2 = new XamlServiceProvider();
				Type typeFromHandle3 = typeof(IProvideValueTarget);
				int num2;
				object[] array3 = new object[(num2 = this.parentValues.Length) + 3];
				Array.Copy(this.parentValues, 0, array3, 3, num2);
				object[] array4 = array3;
				array4[0] = label2;
				array4[1] = grid;
				array4[2] = viewCell;
				object obj2;
				xamlServiceProvider2.Add(typeFromHandle3, obj2 = new SimpleValueTargetProvider(array4, Label.FontSizeProperty, nameScope));
				xamlServiceProvider2.Add(typeof(IReferenceProvider), obj2);
				Type typeFromHandle4 = typeof(IXamlTypeResolver);
				XmlNamespaceResolver xmlNamespaceResolver2 = new XmlNamespaceResolver();
				xmlNamespaceResolver2.Add("", "http://xamarin.com/schemas/2014/forms");
				xmlNamespaceResolver2.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
				xmlNamespaceResolver2.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
				xmlNamespaceResolver2.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
				xmlNamespaceResolver2.Add("local", "clr-namespace:CarScannerXamarinForms");
				xmlNamespaceResolver2.Add("pids", "clr-namespace:CarScannerXamarinForms.OBD2.PIDS");
				xmlNamespaceResolver2.Add("settings", "clr-namespace:CarScannerXamarinForms.Settings");
				xamlServiceProvider2.Add(typeFromHandle4, new XamlTypeResolver(xmlNamespaceResolver2, typeof(SettingsPIDOverrideListPage.<InitializeComponent>_anonXamlCDataTemplate_96).GetTypeInfo().Assembly));
				xamlServiceProvider2.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(141, 45)));
				DynamicResource dynamicResource2 = markupExtension2.ProvideValue(xamlServiceProvider2);
				label2.SetDynamicResource(Label.FontSizeProperty, dynamicResource2.Key);
				span.SetValue(Span.TextProperty, "ID=");
				formattedString.Spans.Add(span);
				bindingExtension2.Mode = 2;
				bindingExtension2.Path = "Id";
				bindingExtension2.TypedBinding = new TypedBinding<PID, int>(delegate(PID A_0)
				{
					if (A_0 != null)
					{
						return new ValueTuple<int, bool>(A_0.Id, true);
					}
					return default(ValueTuple<int, bool>);
				}, null, new Tuple<Func<PID, object>, string>[]
				{
					new Tuple<Func<PID, object>, string>((PID A_0) => A_0, "Id")
				});
				BindingBase bindingBase2 = bindingExtension2.ProvideValue(null);
				span2.SetBinding(Span.TextProperty, bindingBase2);
				formattedString.Spans.Add(span2);
				span3.SetValue(Span.TextProperty, ", ");
				formattedString.Spans.Add(span3);
				translate.Text = "ios_Priority";
				IMarkupExtension markupExtension3 = translate;
				XamlServiceProvider xamlServiceProvider3 = new XamlServiceProvider();
				Type typeFromHandle5 = typeof(IProvideValueTarget);
				int num3;
				object[] array5 = new object[(num3 = this.parentValues.Length) + 5];
				Array.Copy(this.parentValues, 0, array5, 5, num3);
				object[] array6 = array5;
				array6[0] = span4;
				array6[1] = formattedString;
				array6[2] = label2;
				array6[3] = grid;
				array6[4] = viewCell;
				object obj3;
				xamlServiceProvider3.Add(typeFromHandle5, obj3 = new SimpleValueTargetProvider(array6, Span.TextProperty, nameScope));
				xamlServiceProvider3.Add(typeof(IReferenceProvider), obj3);
				Type typeFromHandle6 = typeof(IXamlTypeResolver);
				XmlNamespaceResolver xmlNamespaceResolver3 = new XmlNamespaceResolver();
				xmlNamespaceResolver3.Add("", "http://xamarin.com/schemas/2014/forms");
				xmlNamespaceResolver3.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
				xmlNamespaceResolver3.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
				xmlNamespaceResolver3.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
				xmlNamespaceResolver3.Add("local", "clr-namespace:CarScannerXamarinForms");
				xmlNamespaceResolver3.Add("pids", "clr-namespace:CarScannerXamarinForms.OBD2.PIDS");
				xmlNamespaceResolver3.Add("settings", "clr-namespace:CarScannerXamarinForms.Settings");
				xamlServiceProvider3.Add(typeFromHandle6, new XamlTypeResolver(xmlNamespaceResolver3, typeof(SettingsPIDOverrideListPage.<InitializeComponent>_anonXamlCDataTemplate_96).GetTypeInfo().Assembly));
				xamlServiceProvider3.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(147, 43)));
				object obj4 = markupExtension3.ProvideValue(xamlServiceProvider3);
				span4.Text = obj4;
				formattedString.Spans.Add(span4);
				bindingExtension3.Mode = 2;
				staticResourceExtension.Key = "SkipCyclesIntToPriorityStringConverter";
				IMarkupExtension markupExtension4 = staticResourceExtension;
				XamlServiceProvider xamlServiceProvider4 = new XamlServiceProvider();
				Type typeFromHandle7 = typeof(IProvideValueTarget);
				int num4;
				object[] array7 = new object[(num4 = this.parentValues.Length) + 6];
				Array.Copy(this.parentValues, 0, array7, 6, num4);
				object[] array8 = array7;
				array8[0] = bindingExtension3;
				array8[1] = span5;
				array8[2] = formattedString;
				array8[3] = label2;
				array8[4] = grid;
				array8[5] = viewCell;
				object obj5;
				xamlServiceProvider4.Add(typeFromHandle7, obj5 = new SimpleValueTargetProvider(array8, typeof(BindingExtension).GetRuntimeProperty("Converter"), nameScope));
				xamlServiceProvider4.Add(typeof(IReferenceProvider), obj5);
				Type typeFromHandle8 = typeof(IXamlTypeResolver);
				XmlNamespaceResolver xmlNamespaceResolver4 = new XmlNamespaceResolver();
				xmlNamespaceResolver4.Add("", "http://xamarin.com/schemas/2014/forms");
				xmlNamespaceResolver4.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
				xmlNamespaceResolver4.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
				xmlNamespaceResolver4.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
				xmlNamespaceResolver4.Add("local", "clr-namespace:CarScannerXamarinForms");
				xmlNamespaceResolver4.Add("pids", "clr-namespace:CarScannerXamarinForms.OBD2.PIDS");
				xmlNamespaceResolver4.Add("settings", "clr-namespace:CarScannerXamarinForms.Settings");
				xamlServiceProvider4.Add(typeFromHandle8, new XamlTypeResolver(xmlNamespaceResolver4, typeof(SettingsPIDOverrideListPage.<InitializeComponent>_anonXamlCDataTemplate_96).GetTypeInfo().Assembly));
				xamlServiceProvider4.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(148, 43)));
				object obj6 = markupExtension4.ProvideValue(xamlServiceProvider4);
				bindingExtension3.Converter = obj6;
				bindingExtension3.Path = "SkipCycles";
				bindingExtension3.TypedBinding = new TypedBinding<PID, int>(delegate(PID A_0)
				{
					if (A_0 != null)
					{
						return new ValueTuple<int, bool>(A_0.SkipCycles, true);
					}
					return default(ValueTuple<int, bool>);
				}, null, new Tuple<Func<PID, object>, string>[]
				{
					new Tuple<Func<PID, object>, string>((PID A_0) => A_0, "SkipCycles")
				});
				BindingBase bindingBase3 = bindingExtension3.ProvideValue(null);
				span5.SetBinding(Span.TextProperty, bindingBase3);
				formattedString.Spans.Add(span5);
				span6.SetValue(Span.TextProperty, ", ");
				formattedString.Spans.Add(span6);
				translate2.Text = "ios_Units";
				IMarkupExtension markupExtension5 = translate2;
				XamlServiceProvider xamlServiceProvider5 = new XamlServiceProvider();
				Type typeFromHandle9 = typeof(IProvideValueTarget);
				int num5;
				object[] array9 = new object[(num5 = this.parentValues.Length) + 5];
				Array.Copy(this.parentValues, 0, array9, 5, num5);
				object[] array10 = array9;
				array10[0] = span7;
				array10[1] = formattedString;
				array10[2] = label2;
				array10[3] = grid;
				array10[4] = viewCell;
				object obj7;
				xamlServiceProvider5.Add(typeFromHandle9, obj7 = new SimpleValueTargetProvider(array10, Span.TextProperty, nameScope));
				xamlServiceProvider5.Add(typeof(IReferenceProvider), obj7);
				Type typeFromHandle10 = typeof(IXamlTypeResolver);
				XmlNamespaceResolver xmlNamespaceResolver5 = new XmlNamespaceResolver();
				xmlNamespaceResolver5.Add("", "http://xamarin.com/schemas/2014/forms");
				xmlNamespaceResolver5.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
				xmlNamespaceResolver5.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
				xmlNamespaceResolver5.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
				xmlNamespaceResolver5.Add("local", "clr-namespace:CarScannerXamarinForms");
				xmlNamespaceResolver5.Add("pids", "clr-namespace:CarScannerXamarinForms.OBD2.PIDS");
				xmlNamespaceResolver5.Add("settings", "clr-namespace:CarScannerXamarinForms.Settings");
				xamlServiceProvider5.Add(typeFromHandle10, new XamlTypeResolver(xmlNamespaceResolver5, typeof(SettingsPIDOverrideListPage.<InitializeComponent>_anonXamlCDataTemplate_96).GetTypeInfo().Assembly));
				xamlServiceProvider5.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(150, 43)));
				object obj8 = markupExtension5.ProvideValue(xamlServiceProvider5);
				span7.Text = obj8;
				formattedString.Spans.Add(span7);
				staticResourceExtension2.Key = "PidToUnitsTitleConverter";
				IMarkupExtension markupExtension6 = staticResourceExtension2;
				XamlServiceProvider xamlServiceProvider6 = new XamlServiceProvider();
				Type typeFromHandle11 = typeof(IProvideValueTarget);
				int num6;
				object[] array11 = new object[(num6 = this.parentValues.Length) + 6];
				Array.Copy(this.parentValues, 0, array11, 6, num6);
				object[] array12 = array11;
				array12[0] = bindingExtension4;
				array12[1] = span8;
				array12[2] = formattedString;
				array12[3] = label2;
				array12[4] = grid;
				array12[5] = viewCell;
				object obj9;
				xamlServiceProvider6.Add(typeFromHandle11, obj9 = new SimpleValueTargetProvider(array12, typeof(BindingExtension).GetRuntimeProperty("Converter"), nameScope));
				xamlServiceProvider6.Add(typeof(IReferenceProvider), obj9);
				Type typeFromHandle12 = typeof(IXamlTypeResolver);
				XmlNamespaceResolver xmlNamespaceResolver6 = new XmlNamespaceResolver();
				xmlNamespaceResolver6.Add("", "http://xamarin.com/schemas/2014/forms");
				xmlNamespaceResolver6.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
				xmlNamespaceResolver6.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
				xmlNamespaceResolver6.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
				xmlNamespaceResolver6.Add("local", "clr-namespace:CarScannerXamarinForms");
				xmlNamespaceResolver6.Add("pids", "clr-namespace:CarScannerXamarinForms.OBD2.PIDS");
				xmlNamespaceResolver6.Add("settings", "clr-namespace:CarScannerXamarinForms.Settings");
				xamlServiceProvider6.Add(typeFromHandle12, new XamlTypeResolver(xmlNamespaceResolver6, typeof(SettingsPIDOverrideListPage.<InitializeComponent>_anonXamlCDataTemplate_96).GetTypeInfo().Assembly));
				xamlServiceProvider6.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(151, 43)));
				object obj10 = markupExtension6.ProvideValue(xamlServiceProvider6);
				bindingExtension4.Converter = obj10;
				bindingExtension4.Path = ".";
				bindingExtension4.TypedBinding = new TypedBinding<PID, PID>((PID A_0) => new ValueTuple<PID, bool>(A_0, true), null, null);
				BindingBase bindingBase4 = bindingExtension4.ProvideValue(null);
				span8.SetBinding(Span.TextProperty, bindingBase4);
				formattedString.Spans.Add(span8);
				span9.SetValue(Span.TextProperty, ", ");
				formattedString.Spans.Add(span9);
				translate3.Text = "ios_Role";
				IMarkupExtension markupExtension7 = translate3;
				XamlServiceProvider xamlServiceProvider7 = new XamlServiceProvider();
				Type typeFromHandle13 = typeof(IProvideValueTarget);
				int num7;
				object[] array13 = new object[(num7 = this.parentValues.Length) + 5];
				Array.Copy(this.parentValues, 0, array13, 5, num7);
				object[] array14 = array13;
				array14[0] = span10;
				array14[1] = formattedString;
				array14[2] = label2;
				array14[3] = grid;
				array14[4] = viewCell;
				object obj11;
				xamlServiceProvider7.Add(typeFromHandle13, obj11 = new SimpleValueTargetProvider(array14, Span.TextProperty, nameScope));
				xamlServiceProvider7.Add(typeof(IReferenceProvider), obj11);
				Type typeFromHandle14 = typeof(IXamlTypeResolver);
				XmlNamespaceResolver xmlNamespaceResolver7 = new XmlNamespaceResolver();
				xmlNamespaceResolver7.Add("", "http://xamarin.com/schemas/2014/forms");
				xmlNamespaceResolver7.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
				xmlNamespaceResolver7.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
				xmlNamespaceResolver7.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
				xmlNamespaceResolver7.Add("local", "clr-namespace:CarScannerXamarinForms");
				xmlNamespaceResolver7.Add("pids", "clr-namespace:CarScannerXamarinForms.OBD2.PIDS");
				xmlNamespaceResolver7.Add("settings", "clr-namespace:CarScannerXamarinForms.Settings");
				xamlServiceProvider7.Add(typeFromHandle14, new XamlTypeResolver(xmlNamespaceResolver7, typeof(SettingsPIDOverrideListPage.<InitializeComponent>_anonXamlCDataTemplate_96).GetTypeInfo().Assembly));
				xamlServiceProvider7.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(154, 43)));
				object obj12 = markupExtension7.ProvideValue(xamlServiceProvider7);
				span10.Text = obj12;
				formattedString.Spans.Add(span10);
				bindingExtension5.Mode = 2;
				bindingExtension5.Path = "Role";
				bindingExtension5.TypedBinding = new TypedBinding<PID, Roles>(delegate(PID A_0)
				{
					if (A_0 != null)
					{
						return new ValueTuple<Roles, bool>(A_0.Role, true);
					}
					return default(ValueTuple<Roles, bool>);
				}, null, new Tuple<Func<PID, object>, string>[]
				{
					new Tuple<Func<PID, object>, string>((PID A_0) => A_0, "Role")
				});
				BindingBase bindingBase5 = bindingExtension5.ProvideValue(null);
				span11.SetBinding(Span.TextProperty, bindingBase5);
				formattedString.Spans.Add(span11);
				label2.SetValue(Label.FormattedTextProperty, formattedString);
				grid.Children.Add(label2);
				viewCell.View = grid;
				return viewCell;
			}

			// Token: 0x06001BAE RID: 7086 RVA: 0x0013BA90 File Offset: 0x00139C90
			[CompilerGenerated]
			private static ValueTuple<string, bool> <LoadDataTemplate>typedBindingsM__1584(PID A_0)
			{
				if (A_0 != null)
				{
					return new ValueTuple<string, bool>(A_0.Name, true);
				}
				return default(ValueTuple<string, bool>);
			}

			// Token: 0x06001BAF RID: 7087 RVA: 0x0013BAC0 File Offset: 0x00139CC0
			[CompilerGenerated]
			private static object <LoadDataTemplate>typedBindingsM__1585(PID A_0)
			{
				return A_0;
			}

			// Token: 0x06001BB0 RID: 7088 RVA: 0x0013BAD0 File Offset: 0x00139CD0
			[CompilerGenerated]
			private static ValueTuple<int, bool> <LoadDataTemplate>typedBindingsM__1586(PID A_0)
			{
				if (A_0 != null)
				{
					return new ValueTuple<int, bool>(A_0.Id, true);
				}
				return default(ValueTuple<int, bool>);
			}

			// Token: 0x06001BB1 RID: 7089 RVA: 0x0013BB00 File Offset: 0x00139D00
			[CompilerGenerated]
			private static object <LoadDataTemplate>typedBindingsM__1587(PID A_0)
			{
				return A_0;
			}

			// Token: 0x06001BB2 RID: 7090 RVA: 0x0013BB10 File Offset: 0x00139D10
			[CompilerGenerated]
			private static ValueTuple<int, bool> <LoadDataTemplate>typedBindingsM__1588(PID A_0)
			{
				if (A_0 != null)
				{
					return new ValueTuple<int, bool>(A_0.SkipCycles, true);
				}
				return default(ValueTuple<int, bool>);
			}

			// Token: 0x06001BB3 RID: 7091 RVA: 0x0013BB40 File Offset: 0x00139D40
			[CompilerGenerated]
			private static object <LoadDataTemplate>typedBindingsM__1589(PID A_0)
			{
				return A_0;
			}

			// Token: 0x06001BB4 RID: 7092 RVA: 0x0013BB50 File Offset: 0x00139D50
			[CompilerGenerated]
			private static ValueTuple<PID, bool> <LoadDataTemplate>typedBindingsM__1590(PID A_0)
			{
				return new ValueTuple<PID, bool>(A_0, true);
			}

			// Token: 0x06001BB5 RID: 7093 RVA: 0x0013BB64 File Offset: 0x00139D64
			[CompilerGenerated]
			private static ValueTuple<Roles, bool> <LoadDataTemplate>typedBindingsM__1591(PID A_0)
			{
				if (A_0 != null)
				{
					return new ValueTuple<Roles, bool>(A_0.Role, true);
				}
				return default(ValueTuple<Roles, bool>);
			}

			// Token: 0x06001BB6 RID: 7094 RVA: 0x0013BB94 File Offset: 0x00139D94
			[CompilerGenerated]
			private static object <LoadDataTemplate>typedBindingsM__1592(PID A_0)
			{
				return A_0;
			}

			// Token: 0x04000CE1 RID: 3297
			internal object[] parentValues;

			// Token: 0x04000CE2 RID: 3298
			internal SettingsPIDOverrideListPage root;
		}
	}
}
