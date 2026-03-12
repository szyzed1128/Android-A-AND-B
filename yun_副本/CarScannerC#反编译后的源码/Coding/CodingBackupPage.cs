using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using CarScannerXamarinForms.Common;
using CarScannerXamarinForms.Common.XAMLConverters;
using CarScannerXamarinForms.UserControls;
using Xamarin.Forms;
using Xamarin.Forms.Internals;
using Xamarin.Forms.PlatformConfiguration.iOSSpecific;
using Xamarin.Forms.Xaml;
using Xamarin.Forms.Xaml.Diagnostics;
using Xamarin.Forms.Xaml.Internals;

namespace CarScannerXamarinForms.Coding
{
	// Token: 0x020008EB RID: 2283
	[XamlCompilation(2)]
	[XamlFilePath("Coding\\Pages\\CodingBackupPage.xaml")]
	public class CodingBackupPage : ContentPage
	{
		// Token: 0x06004D16 RID: 19734 RVA: 0x0038C4CC File Offset: 0x0038A6CC
		public CodingBackupPage()
		{
			this.InitializeComponent();
			ToolbarItem toolbarItem = new ToolbarItem("", (string)Application.Current.Resources["NB_settings"], delegate
			{
				this.panelOptions.IsVisible = !this.panelOptions.IsVisible;
			}, 0, 0);
			base.ToolbarItems.Add(new ToolbarItem("", "icons8_checked_checkbox", delegate
			{
				foreach (CodingLogItem codingLogItem in this.Model.VisibleItems)
				{
					codingLogItem.Selected = true;
				}
				this.UpdateRestoreButton();
			}, 0, 0));
			base.ToolbarItems.Add(new ToolbarItem("", "icons8_unchecked_checkbox", delegate
			{
				foreach (CodingLogItem codingLogItem2 in this.Model.VisibleItems)
				{
					codingLogItem2.Selected = false;
				}
				this.UpdateRestoreButton();
			}, 0, 0));
			base.ToolbarItems.Add(toolbarItem);
		}

		// Token: 0x06004D17 RID: 19735 RVA: 0x0038C574 File Offset: 0x0038A774
		private async void CodingBackupPage_Appearing(object sender, EventArgs e)
		{
			if (!this.loaded)
			{
				this.activityFrame.Text = ActivityFrame.PLEASE_WAIT_TEXT;
				this.activityFrame.IsVisible = true;
				this.Model = new CodingBackupModel();
				await this.Model.Load();
				base.BindingContext = this.Model;
				this.activityFrame.IsVisible = false;
				this.loaded = true;
			}
		}

		// Token: 0x06004D18 RID: 19736 RVA: 0x0038C5AB File Offset: 0x0038A7AB
		private void Sw_Toggled(object sender, ToggledEventArgs e)
		{
			this.UpdateRestoreButton();
		}

		// Token: 0x06004D19 RID: 19737 RVA: 0x0038C5B3 File Offset: 0x0038A7B3
		private void Lv_ItemSelected(object sender, SelectedItemChangedEventArgs e)
		{
			if (this.lv.SelectedItem == null)
			{
				return;
			}
			CodingLogItem codingLogItem = (CodingLogItem)this.lv.SelectedItem;
			this.lv.SelectedItem = null;
			codingLogItem.Selected = !codingLogItem.Selected;
			this.UpdateRestoreButton();
		}

		// Token: 0x06004D1A RID: 19738 RVA: 0x0038C5F4 File Offset: 0x0038A7F4
		private void UpdateRestoreButton()
		{
			if (this.Model.VisibleItems.Any((CodingLogItem x) => x.Selected))
			{
				this.btnRestore.IsEnabled = true;
				this.btnShowHide.IsEnabled = true;
				return;
			}
			this.btnRestore.IsEnabled = false;
			this.btnShowHide.IsEnabled = false;
		}

		// Token: 0x06004D1B RID: 19739 RVA: 0x0038C664 File Offset: 0x0038A864
		private async void btnRestore_Clicked(object sender, EventArgs e)
		{
			bool skipWithTheSameData = this.swSkipItemsWithTheSameData.IsToggled;
			this.activityFrame.IsVisible = true;
			this.btnRestore.IsEnabled = false;
			this.lv.IsEnabled = false;
			this.activityFrame.Text = ActivityFrame.PLEASE_WAIT_TEXT;
			CodingLogItem[] selected = (from x in this.Model.VisibleItems
				where x.Selected
				orderby x.Timestamp descending
				select x).ToArray<CodingLogItem>();
			int i = 0;
			while (i < selected.Length)
			{
				CodingLogItem codingBackupItem = selected[i];
				string step_string = string.Format(Translate.GetString("coding_progress_step"), i + 1, selected.Length);
				step_string = string.Concat(new string[]
				{
					ActivityFrame.PLEASE_WAIT_TEXT,
					"\n",
					step_string,
					"\n",
					codingBackupItem.Title
				});
				ICodingContainer container;
				switch (codingBackupItem.Type)
				{
				case CodingLogItem.CodingTypes.MQB:
					goto IL_0243;
				case CodingLogItem.CodingTypes.KWP2000:
					container = codingBackupItem.ToKWP2000Coding();
					break;
				case CodingLogItem.CodingTypes.TOYOTA:
					container = codingBackupItem.ToToyotaCoding();
					break;
				case CodingLogItem.CodingTypes.UDS:
					container = codingBackupItem.ToUDSCoding();
					break;
				case CodingLogItem.CodingTypes.HyundaiKiaUDS:
					container = codingBackupItem.ToHyundaiKiaUDSCoding();
					break;
				case CodingLogItem.CodingTypes.CustomizableCodingTemplate:
					container = codingBackupItem.ToCustomizableCodingTemplate();
					break;
				case CodingLogItem.CodingTypes.MQBParametrizeBase:
					container = codingBackupItem.ToMQBParametrizeBase();
					break;
				case CodingLogItem.CodingTypes.MQBCustomizableMode22Coding:
					container = codingBackupItem.ToMQBCustomizableMode22Coding();
					break;
				default:
					goto IL_0243;
				}
				IL_0254:
				Progress<string> progress = new Progress<string>(delegate(string s)
				{
					Device.BeginInvokeOnMainThread(delegate
					{
						this.activityFrame.Text = step_string + "\n" + s;
					});
				});
				CodingRequestResult codingRequestResult = await container.Execute(container.Password, codingBackupItem.OldData, Translate.GetString("coding_Restored") + "/" + codingBackupItem.Date, progress, null, skipWithTheSameData);
				CodingRequestResult restore_result = codingRequestResult;
				string text = "OK";
				if (restore_result != CodingRequestResult.Success)
				{
					text = "FAIL [" + restore_result.ToString() + "]";
				}
				this.logEditor.Text = string.Concat(new string[]
				{
					this.logEditor.Text,
					"[",
					DateTimeNowHelper.NowSafe.TimeOfDay.ToString(),
					"] ",
					codingBackupItem.Title,
					": ",
					text,
					"\n"
				});
				await App.OBDReader.DebugWrite(string.Format("\r\n[Restore: {0} result={1}]\n", codingBackupItem.Title, restore_result));
				if (restore_result == CodingRequestResult.Success)
				{
					await Task.Delay(3500);
					if (codingBackupItem.Type == CodingLogItem.CodingTypes.MQB || codingBackupItem.Type == CodingLogItem.CodingTypes.MQBParametrizeBase)
					{
						TaskAwaiter<CodingRequestResult> taskAwaiter = container.UpdateCurrentState("", null).GetAwaiter();
						if (!taskAwaiter.IsCompleted)
						{
							await taskAwaiter;
							TaskAwaiter<CodingRequestResult> taskAwaiter2;
							taskAwaiter = taskAwaiter2;
							taskAwaiter2 = default(TaskAwaiter<CodingRequestResult>);
						}
						if (taskAwaiter.GetResult() == CodingRequestResult.Success)
						{
							if (container.CurrentState == codingBackupItem.OldData)
							{
								codingBackupItem.Selected = false;
								this.logEditor.Text = this.logEditor.Text + "[" + DateTimeNowHelper.NowSafe.TimeOfDay.ToString() + "] Data verification: OK\n";
							}
							else
							{
								this.logEditor.Text = this.logEditor.Text + "[" + DateTimeNowHelper.NowSafe.TimeOfDay.ToString() + "] Data verification: ERROR [DATA MISMATCH]\n";
								await base.DisplayAlert(Translate.GetString("coding_Error"), container.Name + ":\nRestored, but data verification failed!\nTry again!", "OK");
							}
						}
						else
						{
							this.logEditor.Text = this.logEditor.Text + "[" + DateTimeNowHelper.NowSafe.TimeOfDay.ToString() + "] Data verification: ERROR READING\n";
							await base.DisplayAlert(Translate.GetString("coding_Error"), container.Name + ":\nRestored, but can't verify data!\nTry again!", "OK");
						}
					}
					else if (codingBackupItem.Type == CodingLogItem.CodingTypes.MQBParametrizeBase)
					{
						await Task.Delay(15000);
						TaskAwaiter<CodingRequestResult> taskAwaiter = container.UpdateCurrentState(codingBackupItem.Password, null).GetAwaiter();
						if (!taskAwaiter.IsCompleted)
						{
							await taskAwaiter;
							TaskAwaiter<CodingRequestResult> taskAwaiter2;
							taskAwaiter = taskAwaiter2;
							taskAwaiter2 = default(TaskAwaiter<CodingRequestResult>);
						}
						if (taskAwaiter.GetResult() == CodingRequestResult.Success)
						{
							if (container.CurrentState == codingBackupItem.OldData)
							{
								codingBackupItem.Selected = false;
								this.logEditor.Text = this.logEditor.Text + "[" + DateTimeNowHelper.NowSafe.TimeOfDay.ToString() + "] Data verification: OK\n";
							}
							else
							{
								this.logEditor.Text = this.logEditor.Text + "[" + DateTimeNowHelper.NowSafe.TimeOfDay.ToString() + "] Data verification: ERROR [DATA MISMATCH]\n";
								await base.DisplayAlert(Translate.GetString("coding_Error"), container.Name + ":\nRestored, but data verification failed!\nTry again!", "OK");
							}
						}
						else
						{
							this.logEditor.Text = this.logEditor.Text + "[" + DateTimeNowHelper.NowSafe.TimeOfDay.ToString() + "] Data verification: ERROR READING\n";
							await base.DisplayAlert(Translate.GetString("coding_Error"), container.Name + ":\nRestored, but can't verify data!\nTry again!", "OK");
						}
					}
					else
					{
						codingBackupItem.Selected = false;
					}
				}
				else
				{
					await base.DisplayAlert(Translate.GetString("coding_Error"), container.Name + ":\n" + MQBAdaptationTemplate.CodingRequestResultToString(restore_result), "OK");
				}
				codingBackupItem = null;
				container = null;
				i++;
				continue;
				IL_0243:
				container = codingBackupItem.ToMQBAdaptationTemplate();
				goto IL_0254;
			}
			List<long> reselectAfterReloading = (from x in this.Model.VisibleItems
				where x.Selected
				select x.Timestamp).ToList<long>();
			await this.Model.Load();
			using (List<long>.Enumerator enumerator = reselectAfterReloading.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					long timestamp = enumerator.Current;
					CodingLogItem codingLogItem = this.Model.RawItems.FirstOrDefault((CodingLogItem x) => x.Timestamp == timestamp);
					if (codingLogItem != null)
					{
						codingLogItem.Selected = true;
					}
				}
			}
			this.btnRestore.IsEnabled = true;
			this.lv.IsEnabled = true;
			this.activityFrame.IsVisible = false;
		}

		// Token: 0x06004D1C RID: 19740 RVA: 0x0038C69C File Offset: 0x0038A89C
		private async void btnRemove_Clicked(object sender, EventArgs e)
		{
			this.activityFrame.IsVisible = true;
			this.btnRestore.IsEnabled = false;
			this.lv.IsEnabled = false;
			this.activityFrame.Text = ActivityFrame.PLEASE_WAIT_TEXT;
			foreach (CodingLogItem codingLogItem in this.Model.VisibleItems.Where((CodingLogItem x) => x.Selected).ToArray<CodingLogItem>())
			{
				this.Model.VisibleItems.Remove(codingLogItem);
			}
			await Task.Run(delegate
			{
				CodingLogItem.SaveLogItems(this.Model.RawItems);
			});
			this.btnRestore.IsEnabled = true;
			this.lv.IsEnabled = true;
			this.activityFrame.IsVisible = false;
		}

		// Token: 0x06004D1D RID: 19741 RVA: 0x0038C5AB File Offset: 0x0038A7AB
		private void CheckBox_CheckedChanged(object sender, CheckedChangedEventArgs e)
		{
			this.UpdateRestoreButton();
		}

		// Token: 0x06004D1E RID: 19742 RVA: 0x0038C6D4 File Offset: 0x0038A8D4
		private async void btnShowHide_Clicked(object sender, EventArgs e)
		{
			this.activityFrame.Text = ActivityFrame.PLEASE_WAIT_TEXT;
			this.activityFrame.IsVisible = true;
			this.lv.IsEnabled = false;
			this.btnRestore.IsEnabled = false;
			this.btnShowHide.IsEnabled = false;
			foreach (CodingLogItem codingLogItem in this.Model.VisibleItems.Where((CodingLogItem x) => x.Selected))
			{
				codingLogItem.IsVisible = !codingLogItem.IsVisible;
				codingLogItem.Selected = false;
			}
			await Task.Run(delegate
			{
				CodingLogItem.SaveLogItems(this.Model.RawItems);
			});
			await this.Model.Load();
			this.activityFrame.Text = ActivityFrame.PLEASE_WAIT_TEXT;
			this.activityFrame.IsVisible = false;
			this.lv.IsEnabled = true;
			this.btnRestore.IsEnabled = true;
			this.btnShowHide.IsEnabled = true;
		}

		// Token: 0x06004D1F RID: 19743 RVA: 0x0038C70C File Offset: 0x0038A90C
		private async void btnShowLog_Clicked(object sender, EventArgs e)
		{
			this.logEditor.IsVisible = !this.logEditor.IsVisible;
		}

		// Token: 0x06004D20 RID: 19744 RVA: 0x0038C744 File Offset: 0x0038A944
		private async void btnBackToStock_Clicked(object sender, EventArgs e)
		{
			string[] vins = this.Model.GetVINCodes().ToArray();
			string text = await this.DisplayActionSheetCustom(Translate.GetString("coding_RestoreToInitialChooseVIN"), Translate.GetString("btnCancel.Content"), null, vins);
			if (vins.Contains(text))
			{
				await this.Model.CreateRestoreToStockItemsForVinCode(text);
				await base.DisplayAlert(Translate.GetString("coding_ReadyToRestoreTitle"), string.Format(Translate.GetString("coding_ReadyToRestoreText"), this.btnRestore.Text), "OK");
			}
		}

		// Token: 0x06004D21 RID: 19745 RVA: 0x0038C77C File Offset: 0x0038A97C
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private void InitializeComponent()
		{
			if (ResourceLoader.IsEnabled && ResourceLoader.CanProvideContentFor(new ResourceLoader.ResourceLoadingQuery
			{
				AssemblyName = typeof(CodingBackupPage).GetTypeInfo().Assembly.GetName(),
				ResourcePath = "Coding/Pages/CodingBackupPage.xaml",
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
			VisualDiagnostics.RegisterSourceInfo(translate = new Translate(), new Uri("Coding\\Pages\\CodingBackupPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 13, 5);
			DynamicResourceExtension dynamicResourceExtension;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension = new DynamicResourceExtension(), new Uri("Coding\\Pages\\CodingBackupPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 17, 5);
			BoolConverterAlwaysTrueOnIOS boolConverterAlwaysTrueOnIOS;
			VisualDiagnostics.RegisterSourceInfo(boolConverterAlwaysTrueOnIOS = new BoolConverterAlwaysTrueOnIOS(), new Uri("Coding\\Pages\\CodingBackupPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 21, 14);
			VagCodingPlatformToTrueConverter vagCodingPlatformToTrueConverter;
			VisualDiagnostics.RegisterSourceInfo(vagCodingPlatformToTrueConverter = new VagCodingPlatformToTrueConverter(), new Uri("Coding\\Pages\\CodingBackupPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 22, 14);
			ResourceDictionary resourceDictionary;
			VisualDiagnostics.RegisterSourceInfo(resourceDictionary = new ResourceDictionary(), new Uri("Coding\\Pages\\CodingBackupPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 20, 10);
			RowDefinition rowDefinition;
			VisualDiagnostics.RegisterSourceInfo(rowDefinition = new RowDefinition(), new Uri("Coding\\Pages\\CodingBackupPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 28, 18);
			RowDefinition rowDefinition2;
			VisualDiagnostics.RegisterSourceInfo(rowDefinition2 = new RowDefinition(), new Uri("Coding\\Pages\\CodingBackupPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 29, 18);
			RowDefinition rowDefinition3;
			VisualDiagnostics.RegisterSourceInfo(rowDefinition3 = new RowDefinition(), new Uri("Coding\\Pages\\CodingBackupPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 30, 18);
			RowDefinition rowDefinition4;
			VisualDiagnostics.RegisterSourceInfo(rowDefinition4 = new RowDefinition(), new Uri("Coding\\Pages\\CodingBackupPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 31, 18);
			BindingExtension bindingExtension;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension = new BindingExtension(), new Uri("Coding\\Pages\\CodingBackupPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 37, 17);
			Entry entry;
			VisualDiagnostics.RegisterSourceInfo(entry = new Entry(), new Uri("Coding\\Pages\\CodingBackupPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 33, 14);
			Translate translate2;
			VisualDiagnostics.RegisterSourceInfo(translate2 = new Translate(), new Uri("Coding\\Pages\\CodingBackupPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 46, 21);
			LabelSwitch labelSwitch;
			VisualDiagnostics.RegisterSourceInfo(labelSwitch = new LabelSwitch(), new Uri("Coding\\Pages\\CodingBackupPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 43, 18);
			BindingExtension bindingExtension2;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension2 = new BindingExtension(), new Uri("Coding\\Pages\\CodingBackupPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 49, 21);
			Translate translate3;
			VisualDiagnostics.RegisterSourceInfo(translate3 = new Translate(), new Uri("Coding\\Pages\\CodingBackupPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 50, 21);
			LabelSwitch labelSwitch2;
			VisualDiagnostics.RegisterSourceInfo(labelSwitch2 = new LabelSwitch(), new Uri("Coding\\Pages\\CodingBackupPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 47, 18);
			Translate translate4;
			VisualDiagnostics.RegisterSourceInfo(translate4 = new Translate(), new Uri("Coding\\Pages\\CodingBackupPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 54, 21);
			Button button;
			VisualDiagnostics.RegisterSourceInfo(button = new Button(), new Uri("Coding\\Pages\\CodingBackupPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 51, 18);
			DynamicResourceExtension dynamicResourceExtension2;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension2 = new DynamicResourceExtension(), new Uri("Coding\\Pages\\CodingBackupPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 57, 21);
			DynamicResourceExtension dynamicResourceExtension3;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension3 = new DynamicResourceExtension(), new Uri("Coding\\Pages\\CodingBackupPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 59, 21);
			Editor editor;
			VisualDiagnostics.RegisterSourceInfo(editor = new Editor(), new Uri("Coding\\Pages\\CodingBackupPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 55, 18);
			StackLayout stackLayout;
			VisualDiagnostics.RegisterSourceInfo(stackLayout = new StackLayout(), new Uri("Coding\\Pages\\CodingBackupPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 38, 14);
			BindingExtension bindingExtension3;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension3 = new BindingExtension(), new Uri("Coding\\Pages\\CodingBackupPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 66, 17);
			DataTemplate dataTemplate;
			VisualDiagnostics.RegisterSourceInfo(dataTemplate = new DataTemplate(), new Uri("Coding\\Pages\\CodingBackupPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 68, 22);
			ListView listView;
			VisualDiagnostics.RegisterSourceInfo(listView = new ListView(), new Uri("Coding\\Pages\\CodingBackupPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 61, 14);
			ActivityFrame activityFrame;
			VisualDiagnostics.RegisterSourceInfo(activityFrame = new ActivityFrame(), new Uri("Coding\\Pages\\CodingBackupPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 130, 14);
			ColumnDefinition columnDefinition;
			VisualDiagnostics.RegisterSourceInfo(columnDefinition = new ColumnDefinition(), new Uri("Coding\\Pages\\CodingBackupPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 138, 22);
			ColumnDefinition columnDefinition2;
			VisualDiagnostics.RegisterSourceInfo(columnDefinition2 = new ColumnDefinition(), new Uri("Coding\\Pages\\CodingBackupPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 139, 22);
			RowDefinition rowDefinition5;
			VisualDiagnostics.RegisterSourceInfo(rowDefinition5 = new RowDefinition(), new Uri("Coding\\Pages\\CodingBackupPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 142, 22);
			RowDefinition rowDefinition6;
			VisualDiagnostics.RegisterSourceInfo(rowDefinition6 = new RowDefinition(), new Uri("Coding\\Pages\\CodingBackupPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 143, 22);
			Translate translate5;
			VisualDiagnostics.RegisterSourceInfo(translate5 = new Translate(), new Uri("Coding\\Pages\\CodingBackupPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 151, 21);
			Button button2;
			VisualDiagnostics.RegisterSourceInfo(button2 = new Button(), new Uri("Coding\\Pages\\CodingBackupPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 146, 18);
			Translate translate6;
			VisualDiagnostics.RegisterSourceInfo(translate6 = new Translate(), new Uri("Coding\\Pages\\CodingBackupPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 157, 21);
			Button button3;
			VisualDiagnostics.RegisterSourceInfo(button3 = new Button(), new Uri("Coding\\Pages\\CodingBackupPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 152, 18);
			Grid grid;
			VisualDiagnostics.RegisterSourceInfo(grid = new Grid(), new Uri("Coding\\Pages\\CodingBackupPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 135, 14);
			Grid grid2;
			VisualDiagnostics.RegisterSourceInfo(grid2 = new Grid(), new Uri("Coding\\Pages\\CodingBackupPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 26, 10);
			VisualDiagnostics.RegisterSourceInfo(this, new Uri("Coding\\Pages\\CodingBackupPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 2, 2);
			NameScope nameScope = NameScope.GetNameScope(this) ?? new NameScope();
			NameScope.SetNameScope(this, nameScope);
			nameScope.RegisterName("searchBar", entry);
			if (entry.StyleId == null)
			{
				entry.StyleId = "searchBar";
			}
			nameScope.RegisterName("panelOptions", stackLayout);
			if (stackLayout.StyleId == null)
			{
				stackLayout.StyleId = "panelOptions";
			}
			nameScope.RegisterName("swSkipItemsWithTheSameData", labelSwitch);
			if (labelSwitch.StyleId == null)
			{
				labelSwitch.StyleId = "swSkipItemsWithTheSameData";
			}
			nameScope.RegisterName("swShowHidden", labelSwitch2);
			if (labelSwitch2.StyleId == null)
			{
				labelSwitch2.StyleId = "swShowHidden";
			}
			nameScope.RegisterName("btnShowLog", button);
			if (button.StyleId == null)
			{
				button.StyleId = "btnShowLog";
			}
			nameScope.RegisterName("logEditor", editor);
			if (editor.StyleId == null)
			{
				editor.StyleId = "logEditor";
			}
			nameScope.RegisterName("lv", listView);
			if (listView.StyleId == null)
			{
				listView.StyleId = "lv";
			}
			nameScope.RegisterName("activityFrame", activityFrame);
			if (activityFrame.StyleId == null)
			{
				activityFrame.StyleId = "activityFrame";
			}
			nameScope.RegisterName("btnRestore", button2);
			if (button2.StyleId == null)
			{
				button2.StyleId = "btnRestore";
			}
			nameScope.RegisterName("btnShowHide", button3);
			if (button3.StyleId == null)
			{
				button3.StyleId = "btnShowHide";
			}
			this.searchBar = entry;
			this.panelOptions = stackLayout;
			this.swSkipItemsWithTheSameData = labelSwitch;
			this.swShowHidden = labelSwitch2;
			this.btnShowLog = button;
			this.logEditor = editor;
			this.lv = listView;
			this.activityFrame = activityFrame;
			this.btnRestore = button2;
			this.btnShowHide = button3;
			this.Resources = resourceDictionary;
			resourceDictionary.Add("BoolConverterAlwaysTrueOnIOS", boolConverterAlwaysTrueOnIOS);
			resourceDictionary.Add("VagCodingPlatformToTrueConverter", vagCodingPlatformToTrueConverter);
			translate.Text = "coding_History";
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
			xamlServiceProvider.Add(typeFromHandle2, new XamlTypeResolver(xmlNamespaceResolver, typeof(CodingBackupPage).GetTypeInfo().Assembly));
			xamlServiceProvider.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(13, 5)));
			object obj2 = markupExtension.ProvideValue(xamlServiceProvider);
			this.Title = obj2;
			this.SetValue(Page.PaddingProperty, new Thickness(5.0, 0.0));
			this.SetValue(Page.UseSafeAreaProperty, true);
			this.Appearing += this.CodingBackupPage_Appearing;
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
			xamlServiceProvider2.Add(typeFromHandle4, new XamlTypeResolver(xmlNamespaceResolver2, typeof(CodingBackupPage).GetTypeInfo().Assembly));
			xamlServiceProvider2.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(17, 5)));
			DynamicResource dynamicResource = markupExtension2.ProvideValue(xamlServiceProvider2);
			this.SetDynamicResource(VisualElement.BackgroundColorProperty, dynamicResource.Key);
			this.Resources = resourceDictionary;
			rowDefinition.SetValue(RowDefinition.HeightProperty, new GridLengthTypeConverter().ConvertFromInvariantString("auto"));
			grid2.GetValue(Grid.RowDefinitionsProperty).Add(rowDefinition);
			rowDefinition2.SetValue(RowDefinition.HeightProperty, new GridLengthTypeConverter().ConvertFromInvariantString("auto"));
			grid2.GetValue(Grid.RowDefinitionsProperty).Add(rowDefinition2);
			rowDefinition3.SetValue(RowDefinition.HeightProperty, new GridLengthTypeConverter().ConvertFromInvariantString("*"));
			grid2.GetValue(Grid.RowDefinitionsProperty).Add(rowDefinition3);
			rowDefinition4.SetValue(RowDefinition.HeightProperty, new GridLengthTypeConverter().ConvertFromInvariantString("auto"));
			grid2.GetValue(Grid.RowDefinitionsProperty).Add(rowDefinition4);
			entry.SetValue(Grid.RowProperty, 0);
			entry.SetValue(Entry.PlaceholderProperty, "\ud83d\udd0e");
			bindingExtension.Mode = 1;
			bindingExtension.Path = "Filter";
			BindingBase bindingBase = bindingExtension.ProvideValue(null);
			entry.SetBinding(Entry.TextProperty, bindingBase);
			grid2.Children.Add(entry);
			stackLayout.SetValue(Grid.RowProperty, 1);
			stackLayout.SetValue(VisualElement.IsVisibleProperty, new VisualElement.VisibilityConverter().ConvertFromInvariantString("False"));
			stackLayout.SetValue(StackLayout.OrientationProperty, 0);
			labelSwitch.SetValue(LabelSwitch.IsToggledProperty, true);
			translate2.Text = "coding_SkipItemsWithTheSameData";
			IMarkupExtension markupExtension3 = translate2;
			XamlServiceProvider xamlServiceProvider3 = new XamlServiceProvider();
			Type typeFromHandle5 = typeof(IProvideValueTarget);
			object[] array3 = new object[0 + 4];
			array3[0] = labelSwitch;
			array3[1] = stackLayout;
			array3[2] = grid2;
			array3[3] = this;
			object obj4;
			xamlServiceProvider3.Add(typeFromHandle5, obj4 = new SimpleValueTargetProvider(array3, LabelSwitch.TextProperty, nameScope));
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
			xamlServiceProvider3.Add(typeFromHandle6, new XamlTypeResolver(xmlNamespaceResolver3, typeof(CodingBackupPage).GetTypeInfo().Assembly));
			xamlServiceProvider3.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(46, 21)));
			object obj5 = markupExtension3.ProvideValue(xamlServiceProvider3);
			labelSwitch.Text = obj5;
			stackLayout.Children.Add(labelSwitch);
			bindingExtension2.Mode = 1;
			bindingExtension2.Path = "ShowHidden";
			BindingBase bindingBase2 = bindingExtension2.ProvideValue(null);
			labelSwitch2.SetBinding(LabelSwitch.IsToggledProperty, bindingBase2);
			translate3.Text = "coding_BackupShowHidden";
			IMarkupExtension markupExtension4 = translate3;
			XamlServiceProvider xamlServiceProvider4 = new XamlServiceProvider();
			Type typeFromHandle7 = typeof(IProvideValueTarget);
			object[] array4 = new object[0 + 4];
			array4[0] = labelSwitch2;
			array4[1] = stackLayout;
			array4[2] = grid2;
			array4[3] = this;
			object obj6;
			xamlServiceProvider4.Add(typeFromHandle7, obj6 = new SimpleValueTargetProvider(array4, LabelSwitch.TextProperty, nameScope));
			xamlServiceProvider4.Add(typeof(IReferenceProvider), obj6);
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
			xamlServiceProvider4.Add(typeFromHandle8, new XamlTypeResolver(xmlNamespaceResolver4, typeof(CodingBackupPage).GetTypeInfo().Assembly));
			xamlServiceProvider4.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(50, 21)));
			object obj7 = markupExtension4.ProvideValue(xamlServiceProvider4);
			labelSwitch2.Text = obj7;
			stackLayout.Children.Add(labelSwitch2);
			button.Clicked += this.btnShowLog_Clicked;
			translate4.Text = "coding_RestoreLog";
			IMarkupExtension markupExtension5 = translate4;
			XamlServiceProvider xamlServiceProvider5 = new XamlServiceProvider();
			Type typeFromHandle9 = typeof(IProvideValueTarget);
			object[] array5 = new object[0 + 4];
			array5[0] = button;
			array5[1] = stackLayout;
			array5[2] = grid2;
			array5[3] = this;
			object obj8;
			xamlServiceProvider5.Add(typeFromHandle9, obj8 = new SimpleValueTargetProvider(array5, Button.TextProperty, nameScope));
			xamlServiceProvider5.Add(typeof(IReferenceProvider), obj8);
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
			xamlServiceProvider5.Add(typeFromHandle10, new XamlTypeResolver(xmlNamespaceResolver5, typeof(CodingBackupPage).GetTypeInfo().Assembly));
			xamlServiceProvider5.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(54, 21)));
			object obj9 = markupExtension5.ProvideValue(xamlServiceProvider5);
			button.Text = obj9;
			stackLayout.Children.Add(button);
			dynamicResourceExtension2.Key = "BackgroundColor";
			IMarkupExtension<DynamicResource> markupExtension6 = dynamicResourceExtension2;
			XamlServiceProvider xamlServiceProvider6 = new XamlServiceProvider();
			Type typeFromHandle11 = typeof(IProvideValueTarget);
			object[] array6 = new object[0 + 4];
			array6[0] = editor;
			array6[1] = stackLayout;
			array6[2] = grid2;
			array6[3] = this;
			object obj10;
			xamlServiceProvider6.Add(typeFromHandle11, obj10 = new SimpleValueTargetProvider(array6, VisualElement.BackgroundColorProperty, nameScope));
			xamlServiceProvider6.Add(typeof(IReferenceProvider), obj10);
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
			xamlServiceProvider6.Add(typeFromHandle12, new XamlTypeResolver(xmlNamespaceResolver6, typeof(CodingBackupPage).GetTypeInfo().Assembly));
			xamlServiceProvider6.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(57, 21)));
			DynamicResource dynamicResource2 = markupExtension6.ProvideValue(xamlServiceProvider6);
			editor.SetDynamicResource(VisualElement.BackgroundColorProperty, dynamicResource2.Key);
			editor.SetValue(VisualElement.IsVisibleProperty, new VisualElement.VisibilityConverter().ConvertFromInvariantString("False"));
			dynamicResourceExtension3.Key = "TextColor";
			IMarkupExtension<DynamicResource> markupExtension7 = dynamicResourceExtension3;
			XamlServiceProvider xamlServiceProvider7 = new XamlServiceProvider();
			Type typeFromHandle13 = typeof(IProvideValueTarget);
			object[] array7 = new object[0 + 4];
			array7[0] = editor;
			array7[1] = stackLayout;
			array7[2] = grid2;
			array7[3] = this;
			object obj11;
			xamlServiceProvider7.Add(typeFromHandle13, obj11 = new SimpleValueTargetProvider(array7, Editor.TextColorProperty, nameScope));
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
			xamlServiceProvider7.Add(typeFromHandle14, new XamlTypeResolver(xmlNamespaceResolver7, typeof(CodingBackupPage).GetTypeInfo().Assembly));
			xamlServiceProvider7.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(59, 21)));
			DynamicResource dynamicResource3 = markupExtension7.ProvideValue(xamlServiceProvider7);
			editor.SetDynamicResource(Editor.TextColorProperty, dynamicResource3.Key);
			stackLayout.Children.Add(editor);
			grid2.Children.Add(stackLayout);
			listView.SetValue(Grid.RowProperty, 2);
			listView.SetValue(ListView.HasUnevenRowsProperty, true);
			listView.ItemSelected += this.Lv_ItemSelected;
			bindingExtension3.Path = "VisibleItems";
			BindingBase bindingBase3 = bindingExtension3.ProvideValue(null);
			listView.SetBinding(ItemsView<Cell>.ItemsSourceProperty, bindingBase3);
			IDataTemplate dataTemplate2 = dataTemplate;
			CodingBackupPage.<InitializeComponent>_anonXamlCDataTemplate_6 <InitializeComponent>_anonXamlCDataTemplate_ = new CodingBackupPage.<InitializeComponent>_anonXamlCDataTemplate_6();
			object[] array8 = new object[0 + 4];
			array8[0] = dataTemplate;
			array8[1] = listView;
			array8[2] = grid2;
			array8[3] = this;
			<InitializeComponent>_anonXamlCDataTemplate_.parentValues = array8;
			<InitializeComponent>_anonXamlCDataTemplate_.root = this;
			dataTemplate2.LoadTemplate = new Func<object>(<InitializeComponent>_anonXamlCDataTemplate_.LoadDataTemplate);
			listView.SetValue(ItemsView<Cell>.ItemTemplateProperty, dataTemplate);
			grid2.Children.Add(listView);
			activityFrame.SetValue(Grid.RowProperty, 1);
			activityFrame.SetValue(VisualElement.IsVisibleProperty, new VisualElement.VisibilityConverter().ConvertFromInvariantString("False"));
			grid2.Children.Add(activityFrame);
			grid.SetValue(Grid.RowProperty, 3);
			columnDefinition.SetValue(ColumnDefinition.WidthProperty, new GridLengthTypeConverter().ConvertFromInvariantString("*"));
			grid.GetValue(Grid.ColumnDefinitionsProperty).Add(columnDefinition);
			columnDefinition2.SetValue(ColumnDefinition.WidthProperty, new GridLengthTypeConverter().ConvertFromInvariantString("*"));
			grid.GetValue(Grid.ColumnDefinitionsProperty).Add(columnDefinition2);
			rowDefinition5.SetValue(RowDefinition.HeightProperty, new GridLengthTypeConverter().ConvertFromInvariantString("auto"));
			grid.GetValue(Grid.RowDefinitionsProperty).Add(rowDefinition5);
			rowDefinition6.SetValue(RowDefinition.HeightProperty, new GridLengthTypeConverter().ConvertFromInvariantString("auto"));
			grid.GetValue(Grid.RowDefinitionsProperty).Add(rowDefinition6);
			button2.SetValue(Grid.ColumnProperty, 0);
			button2.Clicked += this.btnRestore_Clicked;
			button2.SetValue(VisualElement.IsEnabledProperty, false);
			translate5.Text = "coding_Restore";
			IMarkupExtension markupExtension8 = translate5;
			XamlServiceProvider xamlServiceProvider8 = new XamlServiceProvider();
			Type typeFromHandle15 = typeof(IProvideValueTarget);
			object[] array9 = new object[0 + 4];
			array9[0] = button2;
			array9[1] = grid;
			array9[2] = grid2;
			array9[3] = this;
			object obj12;
			xamlServiceProvider8.Add(typeFromHandle15, obj12 = new SimpleValueTargetProvider(array9, Button.TextProperty, nameScope));
			xamlServiceProvider8.Add(typeof(IReferenceProvider), obj12);
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
			xamlServiceProvider8.Add(typeFromHandle16, new XamlTypeResolver(xmlNamespaceResolver8, typeof(CodingBackupPage).GetTypeInfo().Assembly));
			xamlServiceProvider8.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(151, 21)));
			object obj13 = markupExtension8.ProvideValue(xamlServiceProvider8);
			button2.Text = obj13;
			grid.Children.Add(button2);
			button3.SetValue(Grid.ColumnProperty, 1);
			button3.Clicked += this.btnShowHide_Clicked;
			button3.SetValue(VisualElement.IsEnabledProperty, false);
			translate6.Text = "coding_ShowHide";
			IMarkupExtension markupExtension9 = translate6;
			XamlServiceProvider xamlServiceProvider9 = new XamlServiceProvider();
			Type typeFromHandle17 = typeof(IProvideValueTarget);
			object[] array10 = new object[0 + 4];
			array10[0] = button3;
			array10[1] = grid;
			array10[2] = grid2;
			array10[3] = this;
			object obj14;
			xamlServiceProvider9.Add(typeFromHandle17, obj14 = new SimpleValueTargetProvider(array10, Button.TextProperty, nameScope));
			xamlServiceProvider9.Add(typeof(IReferenceProvider), obj14);
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
			xamlServiceProvider9.Add(typeFromHandle18, new XamlTypeResolver(xmlNamespaceResolver9, typeof(CodingBackupPage).GetTypeInfo().Assembly));
			xamlServiceProvider9.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(157, 21)));
			object obj15 = markupExtension9.ProvideValue(xamlServiceProvider9);
			button3.Text = obj15;
			grid.Children.Add(button3);
			grid2.Children.Add(grid);
			this.SetValue(ContentPage.ContentProperty, grid2);
		}

		// Token: 0x06004D22 RID: 19746 RVA: 0x0038E0A6 File Offset: 0x0038C2A6
		[CompilerGenerated]
		private void <.ctor>b__0_0()
		{
			this.panelOptions.IsVisible = !this.panelOptions.IsVisible;
		}

		// Token: 0x06004D23 RID: 19747 RVA: 0x0038E0C4 File Offset: 0x0038C2C4
		[CompilerGenerated]
		private void <.ctor>b__0_1()
		{
			foreach (CodingLogItem codingLogItem in this.Model.VisibleItems)
			{
				codingLogItem.Selected = true;
			}
			this.UpdateRestoreButton();
		}

		// Token: 0x06004D24 RID: 19748 RVA: 0x0038E11C File Offset: 0x0038C31C
		[CompilerGenerated]
		private void <.ctor>b__0_2()
		{
			foreach (CodingLogItem codingLogItem in this.Model.VisibleItems)
			{
				codingLogItem.Selected = false;
			}
			this.UpdateRestoreButton();
		}

		// Token: 0x06004D25 RID: 19749 RVA: 0x0038E174 File Offset: 0x0038C374
		[CompilerGenerated]
		private void <btnRemove_Clicked>b__8_1()
		{
			CodingLogItem.SaveLogItems(this.Model.RawItems);
		}

		// Token: 0x06004D26 RID: 19750 RVA: 0x0038E174 File Offset: 0x0038C374
		[CompilerGenerated]
		private void <btnShowHide_Clicked>b__10_1()
		{
			CodingLogItem.SaveLogItems(this.Model.RawItems);
		}

		// Token: 0x06004D27 RID: 19751 RVA: 0x0038E188 File Offset: 0x0038C388
		private void __InitComponentRuntime()
		{
			Extensions.LoadFromXaml<CodingBackupPage>(this, typeof(CodingBackupPage));
			this.searchBar = NameScopeExtensions.FindByName<Entry>(this, "searchBar");
			this.panelOptions = NameScopeExtensions.FindByName<StackLayout>(this, "panelOptions");
			this.swSkipItemsWithTheSameData = NameScopeExtensions.FindByName<LabelSwitch>(this, "swSkipItemsWithTheSameData");
			this.swShowHidden = NameScopeExtensions.FindByName<LabelSwitch>(this, "swShowHidden");
			this.btnShowLog = NameScopeExtensions.FindByName<Button>(this, "btnShowLog");
			this.logEditor = NameScopeExtensions.FindByName<Editor>(this, "logEditor");
			this.lv = NameScopeExtensions.FindByName<ListView>(this, "lv");
			this.activityFrame = NameScopeExtensions.FindByName<ActivityFrame>(this, "activityFrame");
			this.btnRestore = NameScopeExtensions.FindByName<Button>(this, "btnRestore");
			this.btnShowHide = NameScopeExtensions.FindByName<Button>(this, "btnShowHide");
		}

		// Token: 0x04002D86 RID: 11654
		public CodingBackupModel Model;

		// Token: 0x04002D87 RID: 11655
		private bool loaded;

		// Token: 0x04002D88 RID: 11656
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private Entry searchBar;

		// Token: 0x04002D89 RID: 11657
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private StackLayout panelOptions;

		// Token: 0x04002D8A RID: 11658
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private LabelSwitch swSkipItemsWithTheSameData;

		// Token: 0x04002D8B RID: 11659
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private LabelSwitch swShowHidden;

		// Token: 0x04002D8C RID: 11660
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private Button btnShowLog;

		// Token: 0x04002D8D RID: 11661
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private Editor logEditor;

		// Token: 0x04002D8E RID: 11662
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private ListView lv;

		// Token: 0x04002D8F RID: 11663
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private ActivityFrame activityFrame;

		// Token: 0x04002D90 RID: 11664
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private Button btnRestore;

		// Token: 0x04002D91 RID: 11665
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private Button btnShowHide;

		// Token: 0x020008EC RID: 2284
		[CompilerGenerated]
		[Serializable]
		private sealed class <>c
		{
			// Token: 0x06004D28 RID: 19752 RVA: 0x0038E250 File Offset: 0x0038C450
			// Note: this type is marked as 'beforefieldinit'.
			static <>c()
			{
			}

			// Token: 0x06004D29 RID: 19753 RVA: 0x00002050 File Offset: 0x00000250
			public <>c()
			{
			}

			// Token: 0x06004D2A RID: 19754 RVA: 0x0038E25C File Offset: 0x0038C45C
			internal bool <UpdateRestoreButton>b__6_0(CodingLogItem x)
			{
				return x.Selected;
			}

			// Token: 0x06004D2B RID: 19755 RVA: 0x0038E25C File Offset: 0x0038C45C
			internal bool <btnRestore_Clicked>b__7_0(CodingLogItem x)
			{
				return x.Selected;
			}

			// Token: 0x06004D2C RID: 19756 RVA: 0x00377DB5 File Offset: 0x00375FB5
			internal long <btnRestore_Clicked>b__7_1(CodingLogItem x)
			{
				return x.Timestamp;
			}

			// Token: 0x06004D2D RID: 19757 RVA: 0x0038E25C File Offset: 0x0038C45C
			internal bool <btnRestore_Clicked>b__7_2(CodingLogItem x)
			{
				return x.Selected;
			}

			// Token: 0x06004D2E RID: 19758 RVA: 0x00377DB5 File Offset: 0x00375FB5
			internal long <btnRestore_Clicked>b__7_3(CodingLogItem x)
			{
				return x.Timestamp;
			}

			// Token: 0x06004D2F RID: 19759 RVA: 0x0038E25C File Offset: 0x0038C45C
			internal bool <btnRemove_Clicked>b__8_0(CodingLogItem x)
			{
				return x.Selected;
			}

			// Token: 0x06004D30 RID: 19760 RVA: 0x0038E25C File Offset: 0x0038C45C
			internal bool <btnShowHide_Clicked>b__10_0(CodingLogItem x)
			{
				return x.Selected;
			}

			// Token: 0x04002D92 RID: 11666
			public static readonly CodingBackupPage.<>c <>9 = new CodingBackupPage.<>c();

			// Token: 0x04002D93 RID: 11667
			public static Func<CodingLogItem, bool> <>9__6_0;

			// Token: 0x04002D94 RID: 11668
			public static Func<CodingLogItem, bool> <>9__7_0;

			// Token: 0x04002D95 RID: 11669
			public static Func<CodingLogItem, long> <>9__7_1;

			// Token: 0x04002D96 RID: 11670
			public static Func<CodingLogItem, bool> <>9__7_2;

			// Token: 0x04002D97 RID: 11671
			public static Func<CodingLogItem, long> <>9__7_3;

			// Token: 0x04002D98 RID: 11672
			public static Func<CodingLogItem, bool> <>9__8_0;

			// Token: 0x04002D99 RID: 11673
			public static Func<CodingLogItem, bool> <>9__10_0;
		}

		// Token: 0x020008ED RID: 2285
		[CompilerGenerated]
		private sealed class <>c__DisplayClass7_0
		{
			// Token: 0x06004D31 RID: 19761 RVA: 0x00002050 File Offset: 0x00000250
			public <>c__DisplayClass7_0()
			{
			}

			// Token: 0x06004D32 RID: 19762 RVA: 0x0038E264 File Offset: 0x0038C464
			internal void <btnRestore_Clicked>b__4(string s)
			{
				Device.BeginInvokeOnMainThread(new Action(new CodingBackupPage.<>c__DisplayClass7_1
				{
					CS$<>8__locals1 = this,
					s = s
				}.<btnRestore_Clicked>b__5));
			}

			// Token: 0x04002D9A RID: 11674
			public string step_string;

			// Token: 0x04002D9B RID: 11675
			public CodingBackupPage <>4__this;
		}

		// Token: 0x020008EE RID: 2286
		[CompilerGenerated]
		private sealed class <>c__DisplayClass7_1
		{
			// Token: 0x06004D33 RID: 19763 RVA: 0x00002050 File Offset: 0x00000250
			public <>c__DisplayClass7_1()
			{
			}

			// Token: 0x06004D34 RID: 19764 RVA: 0x0038E289 File Offset: 0x0038C489
			internal void <btnRestore_Clicked>b__5()
			{
				this.CS$<>8__locals1.<>4__this.activityFrame.Text = this.CS$<>8__locals1.step_string + "\n" + this.s;
			}

			// Token: 0x04002D9C RID: 11676
			public string s;

			// Token: 0x04002D9D RID: 11677
			public CodingBackupPage.<>c__DisplayClass7_0 CS$<>8__locals1;
		}

		// Token: 0x020008EF RID: 2287
		[CompilerGenerated]
		private sealed class <>c__DisplayClass7_2
		{
			// Token: 0x06004D35 RID: 19765 RVA: 0x00002050 File Offset: 0x00000250
			public <>c__DisplayClass7_2()
			{
			}

			// Token: 0x06004D36 RID: 19766 RVA: 0x0038E2BB File Offset: 0x0038C4BB
			internal bool <btnRestore_Clicked>b__6(CodingLogItem x)
			{
				return x.Timestamp == this.timestamp;
			}

			// Token: 0x04002D9E RID: 11678
			public long timestamp;
		}

		// Token: 0x020008F0 RID: 2288
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <CodingBackupPage_Appearing>d__3 : IAsyncStateMachine
		{
			// Token: 0x06004D37 RID: 19767 RVA: 0x0038E2CC File Offset: 0x0038C4CC
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				CodingBackupPage codingBackupPage = this;
				try
				{
					TaskAwaiter taskAwaiter;
					if (num != 0)
					{
						if (codingBackupPage.loaded)
						{
							goto IL_00BF;
						}
						codingBackupPage.activityFrame.Text = ActivityFrame.PLEASE_WAIT_TEXT;
						codingBackupPage.activityFrame.IsVisible = true;
						codingBackupPage.Model = new CodingBackupModel();
						taskAwaiter = codingBackupPage.Model.Load().GetAwaiter();
						if (!taskAwaiter.IsCompleted)
						{
							num2 = 0;
							TaskAwaiter taskAwaiter2 = taskAwaiter;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, CodingBackupPage.<CodingBackupPage_Appearing>d__3>(ref taskAwaiter, ref this);
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
					codingBackupPage.BindingContext = codingBackupPage.Model;
					codingBackupPage.activityFrame.IsVisible = false;
					codingBackupPage.loaded = true;
					IL_00BF:;
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

			// Token: 0x06004D38 RID: 19768 RVA: 0x0038E3D4 File Offset: 0x0038C5D4
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x04002D9F RID: 11679
			public int <>1__state;

			// Token: 0x04002DA0 RID: 11680
			public AsyncVoidMethodBuilder <>t__builder;

			// Token: 0x04002DA1 RID: 11681
			public CodingBackupPage <>4__this;

			// Token: 0x04002DA2 RID: 11682
			private TaskAwaiter <>u__1;
		}

		// Token: 0x020008F1 RID: 2289
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <btnBackToStock_Clicked>d__12 : IAsyncStateMachine
		{
			// Token: 0x06004D39 RID: 19769 RVA: 0x0038E3E4 File Offset: 0x0038C5E4
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				CodingBackupPage codingBackupPage = this;
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
						goto IL_011B;
					}
					case 2:
					{
						TaskAwaiter taskAwaiter4;
						taskAwaiter3 = taskAwaiter4;
						taskAwaiter4 = default(TaskAwaiter);
						num2 = -1;
						goto IL_019F;
					}
					default:
						vins = codingBackupPage.Model.GetVINCodes().ToArray();
						taskAwaiter = codingBackupPage.DisplayActionSheetCustom(Translate.GetString("coding_RestoreToInitialChooseVIN"), Translate.GetString("btnCancel.Content"), null, vins).GetAwaiter();
						if (!taskAwaiter.IsCompleted)
						{
							num2 = 0;
							TaskAwaiter<string> taskAwaiter2 = taskAwaiter;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<string>, CodingBackupPage.<btnBackToStock_Clicked>d__12>(ref taskAwaiter, ref this);
							return;
						}
						break;
					}
					string result = taskAwaiter.GetResult();
					if (!vins.Contains(result))
					{
						goto IL_01A6;
					}
					taskAwaiter3 = codingBackupPage.Model.CreateRestoreToStockItemsForVinCode(result).GetAwaiter();
					if (!taskAwaiter3.IsCompleted)
					{
						num2 = 1;
						TaskAwaiter taskAwaiter4 = taskAwaiter3;
						this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, CodingBackupPage.<btnBackToStock_Clicked>d__12>(ref taskAwaiter3, ref this);
						return;
					}
					IL_011B:
					taskAwaiter3.GetResult();
					taskAwaiter3 = codingBackupPage.DisplayAlert(Translate.GetString("coding_ReadyToRestoreTitle"), string.Format(Translate.GetString("coding_ReadyToRestoreText"), codingBackupPage.btnRestore.Text), "OK").GetAwaiter();
					if (!taskAwaiter3.IsCompleted)
					{
						num2 = 2;
						TaskAwaiter taskAwaiter4 = taskAwaiter3;
						this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, CodingBackupPage.<btnBackToStock_Clicked>d__12>(ref taskAwaiter3, ref this);
						return;
					}
					IL_019F:
					taskAwaiter3.GetResult();
					IL_01A6:;
				}
				catch (Exception ex)
				{
					num2 = -2;
					vins = null;
					this.<>t__builder.SetException(ex);
					return;
				}
				num2 = -2;
				vins = null;
				this.<>t__builder.SetResult();
			}

			// Token: 0x06004D3A RID: 19770 RVA: 0x0038E5F0 File Offset: 0x0038C7F0
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x04002DA3 RID: 11683
			public int <>1__state;

			// Token: 0x04002DA4 RID: 11684
			public AsyncVoidMethodBuilder <>t__builder;

			// Token: 0x04002DA5 RID: 11685
			public CodingBackupPage <>4__this;

			// Token: 0x04002DA6 RID: 11686
			private string[] <vins>5__2;

			// Token: 0x04002DA7 RID: 11687
			private TaskAwaiter<string> <>u__1;

			// Token: 0x04002DA8 RID: 11688
			private TaskAwaiter <>u__2;
		}

		// Token: 0x020008F2 RID: 2290
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <btnRemove_Clicked>d__8 : IAsyncStateMachine
		{
			// Token: 0x06004D3B RID: 19771 RVA: 0x0038E600 File Offset: 0x0038C800
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				CodingBackupPage codingBackupPage = this;
				try
				{
					TaskAwaiter taskAwaiter;
					if (num != 0)
					{
						codingBackupPage.activityFrame.IsVisible = true;
						codingBackupPage.btnRestore.IsEnabled = false;
						codingBackupPage.lv.IsEnabled = false;
						codingBackupPage.activityFrame.Text = ActivityFrame.PLEASE_WAIT_TEXT;
						foreach (CodingLogItem codingLogItem in codingBackupPage.Model.VisibleItems.Where((CodingLogItem x) => x.Selected).ToArray<CodingLogItem>())
						{
							codingBackupPage.Model.VisibleItems.Remove(codingLogItem);
						}
						taskAwaiter = Task.Run(delegate
						{
							CodingLogItem.SaveLogItems(codingBackupPage.Model.RawItems);
						}).GetAwaiter();
						if (!taskAwaiter.IsCompleted)
						{
							num2 = 0;
							TaskAwaiter taskAwaiter2 = taskAwaiter;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, CodingBackupPage.<btnRemove_Clicked>d__8>(ref taskAwaiter, ref this);
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
					codingBackupPage.btnRestore.IsEnabled = true;
					codingBackupPage.lv.IsEnabled = true;
					codingBackupPage.activityFrame.IsVisible = false;
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

			// Token: 0x06004D3C RID: 19772 RVA: 0x0038E784 File Offset: 0x0038C984
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x04002DA9 RID: 11689
			public int <>1__state;

			// Token: 0x04002DAA RID: 11690
			public AsyncVoidMethodBuilder <>t__builder;

			// Token: 0x04002DAB RID: 11691
			public CodingBackupPage <>4__this;

			// Token: 0x04002DAC RID: 11692
			private TaskAwaiter <>u__1;
		}

		// Token: 0x020008F3 RID: 2291
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <btnRestore_Clicked>d__7 : IAsyncStateMachine
		{
			// Token: 0x06004D3D RID: 19773 RVA: 0x0038E794 File Offset: 0x0038C994
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				CodingBackupPage codingBackupPage = this;
				try
				{
					TaskAwaiter<CodingRequestResult> taskAwaiter3;
					TaskAwaiter taskAwaiter4;
					switch (num)
					{
					case 0:
						taskAwaiter3 = taskAwaiter2;
						taskAwaiter2 = default(TaskAwaiter<CodingRequestResult>);
						num = (num2 = -1);
						break;
					case 1:
					{
						TaskAwaiter taskAwaiter5;
						taskAwaiter4 = taskAwaiter5;
						taskAwaiter5 = default(TaskAwaiter);
						num = (num2 = -1);
						goto IL_0433;
					}
					case 2:
					{
						TaskAwaiter taskAwaiter5;
						taskAwaiter4 = taskAwaiter5;
						taskAwaiter5 = default(TaskAwaiter);
						num = (num2 = -1);
						goto IL_04A0;
					}
					case 3:
						taskAwaiter3 = taskAwaiter2;
						taskAwaiter2 = default(TaskAwaiter<CodingRequestResult>);
						num = (num2 = -1);
						goto IL_0527;
					case 4:
					{
						TaskAwaiter taskAwaiter5;
						taskAwaiter4 = taskAwaiter5;
						taskAwaiter5 = default(TaskAwaiter);
						num = (num2 = -1);
						goto IL_0660;
					}
					case 5:
					{
						TaskAwaiter taskAwaiter5;
						taskAwaiter4 = taskAwaiter5;
						taskAwaiter5 = default(TaskAwaiter);
						num = (num2 = -1);
						goto IL_0729;
					}
					case 6:
					{
						TaskAwaiter taskAwaiter5;
						taskAwaiter4 = taskAwaiter5;
						taskAwaiter5 = default(TaskAwaiter);
						num = (num2 = -1);
						goto IL_07A1;
					}
					case 7:
						taskAwaiter3 = taskAwaiter2;
						taskAwaiter2 = default(TaskAwaiter<CodingRequestResult>);
						num = (num2 = -1);
						goto IL_0810;
					case 8:
					{
						TaskAwaiter taskAwaiter5;
						taskAwaiter4 = taskAwaiter5;
						taskAwaiter5 = default(TaskAwaiter);
						num = (num2 = -1);
						goto IL_0949;
					}
					case 9:
					{
						TaskAwaiter taskAwaiter5;
						taskAwaiter4 = taskAwaiter5;
						taskAwaiter5 = default(TaskAwaiter);
						num = (num2 = -1);
						goto IL_0A13;
					}
					case 10:
					{
						TaskAwaiter taskAwaiter5;
						taskAwaiter4 = taskAwaiter5;
						taskAwaiter5 = default(TaskAwaiter);
						num = (num2 = -1);
						goto IL_0AB7;
					}
					case 11:
					{
						TaskAwaiter taskAwaiter5;
						taskAwaiter4 = taskAwaiter5;
						taskAwaiter5 = default(TaskAwaiter);
						num = (num2 = -1);
						goto IL_0BAC;
					}
					default:
						skipWithTheSameData = codingBackupPage.swSkipItemsWithTheSameData.IsToggled;
						codingBackupPage.activityFrame.IsVisible = true;
						codingBackupPage.btnRestore.IsEnabled = false;
						codingBackupPage.lv.IsEnabled = false;
						codingBackupPage.activityFrame.Text = ActivityFrame.PLEASE_WAIT_TEXT;
						selected = (from x in codingBackupPage.Model.VisibleItems
							where x.Selected
							orderby x.Timestamp descending
							select x).ToArray<CodingLogItem>();
						i = 0;
						goto IL_0ADE;
					}
					IL_02FF:
					CodingRequestResult result = taskAwaiter3.GetResult();
					restore_result = result;
					string text = "OK";
					if (restore_result != CodingRequestResult.Success)
					{
						text = "FAIL [" + restore_result.ToString() + "]";
					}
					codingBackupPage.logEditor.Text = string.Concat(new string[]
					{
						codingBackupPage.logEditor.Text,
						"[",
						DateTimeNowHelper.NowSafe.TimeOfDay.ToString(),
						"] ",
						codingBackupItem.Title,
						": ",
						text,
						"\n"
					});
					taskAwaiter4 = App.OBDReader.DebugWrite(string.Format("\r\n[Restore: {0} result={1}]\n", codingBackupItem.Title, restore_result)).GetAwaiter();
					if (!taskAwaiter4.IsCompleted)
					{
						num = (num2 = 1);
						TaskAwaiter taskAwaiter5 = taskAwaiter4;
						this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, CodingBackupPage.<btnRestore_Clicked>d__7>(ref taskAwaiter4, ref this);
						return;
					}
					IL_0433:
					taskAwaiter4.GetResult();
					if (restore_result == CodingRequestResult.Success)
					{
						taskAwaiter4 = Task.Delay(3500).GetAwaiter();
						if (!taskAwaiter4.IsCompleted)
						{
							num = (num2 = 2);
							TaskAwaiter taskAwaiter5 = taskAwaiter4;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, CodingBackupPage.<btnRestore_Clicked>d__7>(ref taskAwaiter4, ref this);
							return;
						}
					}
					else
					{
						taskAwaiter4 = codingBackupPage.DisplayAlert(Translate.GetString("coding_Error"), container.Name + ":\n" + MQBAdaptationTemplate.CodingRequestResultToString(restore_result), "OK").GetAwaiter();
						if (!taskAwaiter4.IsCompleted)
						{
							num = (num2 = 10);
							TaskAwaiter taskAwaiter5 = taskAwaiter4;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, CodingBackupPage.<btnRestore_Clicked>d__7>(ref taskAwaiter4, ref this);
							return;
						}
						goto IL_0AB7;
					}
					IL_04A0:
					taskAwaiter4.GetResult();
					if (codingBackupItem.Type == CodingLogItem.CodingTypes.MQB || codingBackupItem.Type == CodingLogItem.CodingTypes.MQBParametrizeBase)
					{
						taskAwaiter3 = container.UpdateCurrentState("", null).GetAwaiter();
						if (!taskAwaiter3.IsCompleted)
						{
							num = (num2 = 3);
							taskAwaiter2 = taskAwaiter3;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<CodingRequestResult>, CodingBackupPage.<btnRestore_Clicked>d__7>(ref taskAwaiter3, ref this);
							return;
						}
					}
					else
					{
						if (codingBackupItem.Type != CodingLogItem.CodingTypes.MQBParametrizeBase)
						{
							codingBackupItem.Selected = false;
							goto IL_0ABE;
						}
						taskAwaiter4 = Task.Delay(15000).GetAwaiter();
						if (!taskAwaiter4.IsCompleted)
						{
							num = (num2 = 6);
							TaskAwaiter taskAwaiter5 = taskAwaiter4;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, CodingBackupPage.<btnRestore_Clicked>d__7>(ref taskAwaiter4, ref this);
							return;
						}
						goto IL_07A1;
					}
					IL_0527:
					if (taskAwaiter3.GetResult() == CodingRequestResult.Success)
					{
						if (container.CurrentState == codingBackupItem.OldData)
						{
							codingBackupItem.Selected = false;
							codingBackupPage.logEditor.Text = codingBackupPage.logEditor.Text + "[" + DateTimeNowHelper.NowSafe.TimeOfDay.ToString() + "] Data verification: OK\n";
							goto IL_0ABE;
						}
						codingBackupPage.logEditor.Text = codingBackupPage.logEditor.Text + "[" + DateTimeNowHelper.NowSafe.TimeOfDay.ToString() + "] Data verification: ERROR [DATA MISMATCH]\n";
						taskAwaiter4 = codingBackupPage.DisplayAlert(Translate.GetString("coding_Error"), container.Name + ":\nRestored, but data verification failed!\nTry again!", "OK").GetAwaiter();
						if (!taskAwaiter4.IsCompleted)
						{
							num = (num2 = 4);
							TaskAwaiter taskAwaiter5 = taskAwaiter4;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, CodingBackupPage.<btnRestore_Clicked>d__7>(ref taskAwaiter4, ref this);
							return;
						}
					}
					else
					{
						codingBackupPage.logEditor.Text = codingBackupPage.logEditor.Text + "[" + DateTimeNowHelper.NowSafe.TimeOfDay.ToString() + "] Data verification: ERROR READING\n";
						taskAwaiter4 = codingBackupPage.DisplayAlert(Translate.GetString("coding_Error"), container.Name + ":\nRestored, but can't verify data!\nTry again!", "OK").GetAwaiter();
						if (!taskAwaiter4.IsCompleted)
						{
							num = (num2 = 5);
							TaskAwaiter taskAwaiter5 = taskAwaiter4;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, CodingBackupPage.<btnRestore_Clicked>d__7>(ref taskAwaiter4, ref this);
							return;
						}
						goto IL_0729;
					}
					IL_0660:
					taskAwaiter4.GetResult();
					goto IL_0ABE;
					IL_0729:
					taskAwaiter4.GetResult();
					goto IL_0ABE;
					IL_07A1:
					taskAwaiter4.GetResult();
					taskAwaiter3 = container.UpdateCurrentState(codingBackupItem.Password, null).GetAwaiter();
					if (!taskAwaiter3.IsCompleted)
					{
						num = (num2 = 7);
						taskAwaiter2 = taskAwaiter3;
						this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<CodingRequestResult>, CodingBackupPage.<btnRestore_Clicked>d__7>(ref taskAwaiter3, ref this);
						return;
					}
					IL_0810:
					if (taskAwaiter3.GetResult() == CodingRequestResult.Success)
					{
						if (container.CurrentState == codingBackupItem.OldData)
						{
							codingBackupItem.Selected = false;
							codingBackupPage.logEditor.Text = codingBackupPage.logEditor.Text + "[" + DateTimeNowHelper.NowSafe.TimeOfDay.ToString() + "] Data verification: OK\n";
							goto IL_0ABE;
						}
						codingBackupPage.logEditor.Text = codingBackupPage.logEditor.Text + "[" + DateTimeNowHelper.NowSafe.TimeOfDay.ToString() + "] Data verification: ERROR [DATA MISMATCH]\n";
						taskAwaiter4 = codingBackupPage.DisplayAlert(Translate.GetString("coding_Error"), container.Name + ":\nRestored, but data verification failed!\nTry again!", "OK").GetAwaiter();
						if (!taskAwaiter4.IsCompleted)
						{
							num = (num2 = 8);
							TaskAwaiter taskAwaiter5 = taskAwaiter4;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, CodingBackupPage.<btnRestore_Clicked>d__7>(ref taskAwaiter4, ref this);
							return;
						}
					}
					else
					{
						codingBackupPage.logEditor.Text = codingBackupPage.logEditor.Text + "[" + DateTimeNowHelper.NowSafe.TimeOfDay.ToString() + "] Data verification: ERROR READING\n";
						taskAwaiter4 = codingBackupPage.DisplayAlert(Translate.GetString("coding_Error"), container.Name + ":\nRestored, but can't verify data!\nTry again!", "OK").GetAwaiter();
						if (!taskAwaiter4.IsCompleted)
						{
							num = (num2 = 9);
							TaskAwaiter taskAwaiter5 = taskAwaiter4;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, CodingBackupPage.<btnRestore_Clicked>d__7>(ref taskAwaiter4, ref this);
							return;
						}
						goto IL_0A13;
					}
					IL_0949:
					taskAwaiter4.GetResult();
					goto IL_0ABE;
					IL_0A13:
					taskAwaiter4.GetResult();
					goto IL_0ABE;
					IL_0AB7:
					taskAwaiter4.GetResult();
					IL_0ABE:
					codingBackupItem = null;
					container = null;
					int num3 = i;
					i = num3 + 1;
					IL_0ADE:
					if (i >= selected.Length)
					{
						reselectAfterReloading = (from x in codingBackupPage.Model.VisibleItems
							where x.Selected
							select x.Timestamp).ToList<long>();
						taskAwaiter4 = codingBackupPage.Model.Load().GetAwaiter();
						if (!taskAwaiter4.IsCompleted)
						{
							num = (num2 = 11);
							TaskAwaiter taskAwaiter5 = taskAwaiter4;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, CodingBackupPage.<btnRestore_Clicked>d__7>(ref taskAwaiter4, ref this);
							return;
						}
					}
					else
					{
						CodingBackupPage.<>c__DisplayClass7_0 CS$<>8__locals1 = new CodingBackupPage.<>c__DisplayClass7_0();
						CS$<>8__locals1.<>4__this = codingBackupPage;
						codingBackupItem = selected[i];
						CS$<>8__locals1.step_string = string.Format(Translate.GetString("coding_progress_step"), i + 1, selected.Length);
						CS$<>8__locals1.step_string = string.Concat(new string[]
						{
							ActivityFrame.PLEASE_WAIT_TEXT,
							"\n",
							CS$<>8__locals1.step_string,
							"\n",
							codingBackupItem.Title
						});
						switch (codingBackupItem.Type)
						{
						case CodingLogItem.CodingTypes.KWP2000:
							container = codingBackupItem.ToKWP2000Coding();
							goto IL_0254;
						case CodingLogItem.CodingTypes.TOYOTA:
							container = codingBackupItem.ToToyotaCoding();
							goto IL_0254;
						case CodingLogItem.CodingTypes.UDS:
							container = codingBackupItem.ToUDSCoding();
							goto IL_0254;
						case CodingLogItem.CodingTypes.HyundaiKiaUDS:
							container = codingBackupItem.ToHyundaiKiaUDSCoding();
							goto IL_0254;
						case CodingLogItem.CodingTypes.CustomizableCodingTemplate:
							container = codingBackupItem.ToCustomizableCodingTemplate();
							goto IL_0254;
						case CodingLogItem.CodingTypes.MQBParametrizeBase:
							container = codingBackupItem.ToMQBParametrizeBase();
							goto IL_0254;
						case CodingLogItem.CodingTypes.MQBCustomizableMode22Coding:
							container = codingBackupItem.ToMQBCustomizableMode22Coding();
							goto IL_0254;
						}
						container = codingBackupItem.ToMQBAdaptationTemplate();
						IL_0254:
						Progress<string> progress = new Progress<string>(delegate(string s)
						{
							Device.BeginInvokeOnMainThread(new Action(new CodingBackupPage.<>c__DisplayClass7_1
							{
								CS$<>8__locals1 = CS$<>8__locals1,
								s = s
							}.<btnRestore_Clicked>b__5));
						});
						taskAwaiter3 = container.Execute(container.Password, codingBackupItem.OldData, Translate.GetString("coding_Restored") + "/" + codingBackupItem.Date, progress, null, skipWithTheSameData).GetAwaiter();
						if (!taskAwaiter3.IsCompleted)
						{
							num = (num2 = 0);
							taskAwaiter2 = taskAwaiter3;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<CodingRequestResult>, CodingBackupPage.<btnRestore_Clicked>d__7>(ref taskAwaiter3, ref this);
							return;
						}
						goto IL_02FF;
					}
					IL_0BAC:
					taskAwaiter4.GetResult();
					List<long>.Enumerator enumerator = reselectAfterReloading.GetEnumerator();
					try
					{
						while (enumerator.MoveNext())
						{
							CodingBackupPage.<>c__DisplayClass7_2 CS$<>8__locals2 = new CodingBackupPage.<>c__DisplayClass7_2();
							CS$<>8__locals2.timestamp = enumerator.Current;
							CodingLogItem codingLogItem = codingBackupPage.Model.RawItems.FirstOrDefault((CodingLogItem x) => x.Timestamp == CS$<>8__locals2.timestamp);
							if (codingLogItem != null)
							{
								codingLogItem.Selected = true;
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
					codingBackupPage.btnRestore.IsEnabled = true;
					codingBackupPage.lv.IsEnabled = true;
					codingBackupPage.activityFrame.IsVisible = false;
				}
				catch (Exception ex)
				{
					num2 = -2;
					selected = null;
					reselectAfterReloading = null;
					this.<>t__builder.SetException(ex);
					return;
				}
				num2 = -2;
				selected = null;
				reselectAfterReloading = null;
				this.<>t__builder.SetResult();
			}

			// Token: 0x06004D3E RID: 19774 RVA: 0x0038F464 File Offset: 0x0038D664
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x04002DAD RID: 11693
			public int <>1__state;

			// Token: 0x04002DAE RID: 11694
			public AsyncVoidMethodBuilder <>t__builder;

			// Token: 0x04002DAF RID: 11695
			public CodingBackupPage <>4__this;

			// Token: 0x04002DB0 RID: 11696
			private bool <skipWithTheSameData>5__2;

			// Token: 0x04002DB1 RID: 11697
			private CodingLogItem[] <selected>5__3;

			// Token: 0x04002DB2 RID: 11698
			private List<long> <reselectAfterReloading>5__4;

			// Token: 0x04002DB3 RID: 11699
			private int <i>5__5;

			// Token: 0x04002DB4 RID: 11700
			private CodingLogItem <codingBackupItem>5__6;

			// Token: 0x04002DB5 RID: 11701
			private ICodingContainer <container>5__7;

			// Token: 0x04002DB6 RID: 11702
			private CodingRequestResult <restore_result>5__8;

			// Token: 0x04002DB7 RID: 11703
			private TaskAwaiter<CodingRequestResult> <>u__1;

			// Token: 0x04002DB8 RID: 11704
			private TaskAwaiter <>u__2;
		}

		// Token: 0x020008F4 RID: 2292
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <btnShowHide_Clicked>d__10 : IAsyncStateMachine
		{
			// Token: 0x06004D3F RID: 19775 RVA: 0x0038F474 File Offset: 0x0038D674
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				CodingBackupPage codingBackupPage = this;
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
							num = (num2 = -1);
							goto IL_0184;
						}
						codingBackupPage.activityFrame.Text = ActivityFrame.PLEASE_WAIT_TEXT;
						codingBackupPage.activityFrame.IsVisible = true;
						codingBackupPage.lv.IsEnabled = false;
						codingBackupPage.btnRestore.IsEnabled = false;
						codingBackupPage.btnShowHide.IsEnabled = false;
						IEnumerator<CodingLogItem> enumerator = codingBackupPage.Model.VisibleItems.Where((CodingLogItem x) => x.Selected).GetEnumerator();
						try
						{
							while (enumerator.MoveNext())
							{
								CodingLogItem codingLogItem = enumerator.Current;
								codingLogItem.IsVisible = !codingLogItem.IsVisible;
								codingLogItem.Selected = false;
							}
						}
						finally
						{
							if (num < 0 && enumerator != null)
							{
								enumerator.Dispose();
							}
						}
						taskAwaiter = Task.Run(delegate
						{
							CodingLogItem.SaveLogItems(codingBackupPage.Model.RawItems);
						}).GetAwaiter();
						if (!taskAwaiter.IsCompleted)
						{
							num = (num2 = 0);
							TaskAwaiter taskAwaiter2 = taskAwaiter;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, CodingBackupPage.<btnShowHide_Clicked>d__10>(ref taskAwaiter, ref this);
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
					taskAwaiter = codingBackupPage.Model.Load().GetAwaiter();
					if (!taskAwaiter.IsCompleted)
					{
						num = (num2 = 1);
						TaskAwaiter taskAwaiter2 = taskAwaiter;
						this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, CodingBackupPage.<btnShowHide_Clicked>d__10>(ref taskAwaiter, ref this);
						return;
					}
					IL_0184:
					taskAwaiter.GetResult();
					codingBackupPage.activityFrame.Text = ActivityFrame.PLEASE_WAIT_TEXT;
					codingBackupPage.activityFrame.IsVisible = false;
					codingBackupPage.lv.IsEnabled = true;
					codingBackupPage.btnRestore.IsEnabled = true;
					codingBackupPage.btnShowHide.IsEnabled = true;
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

			// Token: 0x06004D40 RID: 19776 RVA: 0x0038F6B0 File Offset: 0x0038D8B0
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x04002DB9 RID: 11705
			public int <>1__state;

			// Token: 0x04002DBA RID: 11706
			public AsyncVoidMethodBuilder <>t__builder;

			// Token: 0x04002DBB RID: 11707
			public CodingBackupPage <>4__this;

			// Token: 0x04002DBC RID: 11708
			private TaskAwaiter <>u__1;
		}

		// Token: 0x020008F5 RID: 2293
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <btnShowLog_Clicked>d__11 : IAsyncStateMachine
		{
			// Token: 0x06004D41 RID: 19777 RVA: 0x0038F6C0 File Offset: 0x0038D8C0
			void IAsyncStateMachine.MoveNext()
			{
				CodingBackupPage codingBackupPage = this;
				try
				{
					codingBackupPage.logEditor.IsVisible = !codingBackupPage.logEditor.IsVisible;
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

			// Token: 0x06004D42 RID: 19778 RVA: 0x0038F72C File Offset: 0x0038D92C
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x04002DBD RID: 11709
			public int <>1__state;

			// Token: 0x04002DBE RID: 11710
			public AsyncVoidMethodBuilder <>t__builder;

			// Token: 0x04002DBF RID: 11711
			public CodingBackupPage <>4__this;
		}

		// Token: 0x020008F6 RID: 2294
		[CompilerGenerated]
		private sealed class <InitializeComponent>_anonXamlCDataTemplate_6
		{
			// Token: 0x06004D43 RID: 19779 RVA: 0x0038F73C File Offset: 0x0038D93C
			public <InitializeComponent>_anonXamlCDataTemplate_6()
			{
			}

			// Token: 0x06004D44 RID: 19780 RVA: 0x0038F750 File Offset: 0x0038D950
			internal object LoadDataTemplate()
			{
				ColumnDefinition columnDefinition;
				VisualDiagnostics.RegisterSourceInfo(columnDefinition = new ColumnDefinition(), new Uri("Coding\\Pages\\CodingBackupPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 72, 38);
				ColumnDefinition columnDefinition2;
				VisualDiagnostics.RegisterSourceInfo(columnDefinition2 = new ColumnDefinition(), new Uri("Coding\\Pages\\CodingBackupPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 73, 38);
				BindingExtension bindingExtension;
				VisualDiagnostics.RegisterSourceInfo(bindingExtension = new BindingExtension(), new Uri("Coding\\Pages\\CodingBackupPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 79, 77);
				Span span;
				VisualDiagnostics.RegisterSourceInfo(span = new Span(), new Uri("Coding\\Pages\\CodingBackupPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 79, 50);
				Span span2;
				VisualDiagnostics.RegisterSourceInfo(span2 = new Span(), new Uri("Coding\\Pages\\CodingBackupPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 80, 50);
				BindingExtension bindingExtension2;
				VisualDiagnostics.RegisterSourceInfo(bindingExtension2 = new BindingExtension(), new Uri("Coding\\Pages\\CodingBackupPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 81, 55);
				Span span3;
				VisualDiagnostics.RegisterSourceInfo(span3 = new Span(), new Uri("Coding\\Pages\\CodingBackupPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 81, 50);
				FormattedString formattedString;
				VisualDiagnostics.RegisterSourceInfo(formattedString = new FormattedString(), new Uri("Coding\\Pages\\CodingBackupPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 78, 46);
				Label label;
				VisualDiagnostics.RegisterSourceInfo(label = new Label(), new Uri("Coding\\Pages\\CodingBackupPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 76, 38);
				DynamicResourceExtension dynamicResourceExtension;
				VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension = new DynamicResourceExtension(), new Uri("Coding\\Pages\\CodingBackupPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 90, 55);
				BindingExtension bindingExtension3;
				VisualDiagnostics.RegisterSourceInfo(bindingExtension3 = new BindingExtension(), new Uri("Coding\\Pages\\CodingBackupPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 90, 98);
				Span span4;
				VisualDiagnostics.RegisterSourceInfo(span4 = new Span(), new Uri("Coding\\Pages\\CodingBackupPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 90, 50);
				DynamicResourceExtension dynamicResourceExtension2;
				VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension2 = new DynamicResourceExtension(), new Uri("Coding\\Pages\\CodingBackupPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 91, 55);
				Span span5;
				VisualDiagnostics.RegisterSourceInfo(span5 = new Span(), new Uri("Coding\\Pages\\CodingBackupPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 91, 50);
				DynamicResourceExtension dynamicResourceExtension3;
				VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension3 = new DynamicResourceExtension(), new Uri("Coding\\Pages\\CodingBackupPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 92, 55);
				BindingExtension bindingExtension4;
				VisualDiagnostics.RegisterSourceInfo(bindingExtension4 = new BindingExtension(), new Uri("Coding\\Pages\\CodingBackupPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 92, 98);
				Span span6;
				VisualDiagnostics.RegisterSourceInfo(span6 = new Span(), new Uri("Coding\\Pages\\CodingBackupPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 92, 50);
				FormattedString formattedString2;
				VisualDiagnostics.RegisterSourceInfo(formattedString2 = new FormattedString(), new Uri("Coding\\Pages\\CodingBackupPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 89, 46);
				Label label2;
				VisualDiagnostics.RegisterSourceInfo(label2 = new Label(), new Uri("Coding\\Pages\\CodingBackupPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 87, 38);
				DynamicResourceExtension dynamicResourceExtension4;
				VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension4 = new DynamicResourceExtension(), new Uri("Coding\\Pages\\CodingBackupPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 97, 41);
				BindingExtension bindingExtension5;
				VisualDiagnostics.RegisterSourceInfo(bindingExtension5 = new BindingExtension(), new Uri("Coding\\Pages\\CodingBackupPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 98, 41);
				BindingExtension bindingExtension6;
				VisualDiagnostics.RegisterSourceInfo(bindingExtension6 = new BindingExtension(), new Uri("Coding\\Pages\\CodingBackupPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 99, 41);
				Label label3;
				VisualDiagnostics.RegisterSourceInfo(label3 = new Label(), new Uri("Coding\\Pages\\CodingBackupPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 96, 38);
				StaticResourceExtension staticResourceExtension;
				VisualDiagnostics.RegisterSourceInfo(staticResourceExtension = new StaticResourceExtension(), new Uri("Coding\\Pages\\CodingBackupPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 100, 50);
				BindingExtension bindingExtension7;
				VisualDiagnostics.RegisterSourceInfo(bindingExtension7 = new BindingExtension(), new Uri("Coding\\Pages\\CodingBackupPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 100, 50);
				StaticResourceExtension staticResourceExtension2;
				VisualDiagnostics.RegisterSourceInfo(staticResourceExtension2 = new StaticResourceExtension(), new Uri("Coding\\Pages\\CodingBackupPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 101, 54);
				BindingExtension bindingExtension8;
				VisualDiagnostics.RegisterSourceInfo(bindingExtension8 = new BindingExtension(), new Uri("Coding\\Pages\\CodingBackupPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 101, 54);
				Translate translate;
				VisualDiagnostics.RegisterSourceInfo(translate = new Translate(), new Uri("Coding\\Pages\\CodingBackupPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 102, 52);
				Label label4;
				VisualDiagnostics.RegisterSourceInfo(label4 = new Label(), new Uri("Coding\\Pages\\CodingBackupPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 102, 46);
				BindingExtension bindingExtension9;
				VisualDiagnostics.RegisterSourceInfo(bindingExtension9 = new BindingExtension(), new Uri("Coding\\Pages\\CodingBackupPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 105, 49);
				Entry entry;
				VisualDiagnostics.RegisterSourceInfo(entry = new Entry(), new Uri("Coding\\Pages\\CodingBackupPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 103, 46);
				StackLayout stackLayout;
				VisualDiagnostics.RegisterSourceInfo(stackLayout = new StackLayout(), new Uri("Coding\\Pages\\CodingBackupPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 101, 42);
				StackLayout stackLayout2;
				VisualDiagnostics.RegisterSourceInfo(stackLayout2 = new StackLayout(), new Uri("Coding\\Pages\\CodingBackupPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 100, 38);
				StackLayout stackLayout3;
				VisualDiagnostics.RegisterSourceInfo(stackLayout3 = new StackLayout(), new Uri("Coding\\Pages\\CodingBackupPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 75, 34);
				BindingExtension bindingExtension10;
				VisualDiagnostics.RegisterSourceInfo(bindingExtension10 = new BindingExtension(), new Uri("Coding\\Pages\\CodingBackupPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 119, 37);
				CheckBoxWithColor checkBoxWithColor;
				VisualDiagnostics.RegisterSourceInfo(checkBoxWithColor = new CheckBoxWithColor(), new Uri("Coding\\Pages\\CodingBackupPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 115, 34);
				Grid grid;
				VisualDiagnostics.RegisterSourceInfo(grid = new Grid(), new Uri("Coding\\Pages\\CodingBackupPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 70, 30);
				ViewCell viewCell;
				VisualDiagnostics.RegisterSourceInfo(viewCell = new ViewCell(), new Uri("Coding\\Pages\\CodingBackupPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 69, 26);
				NameScope nameScope = new NameScope();
				NameScope.SetNameScope(viewCell, nameScope);
				nameScope.RegisterName("entryPassword", entry);
				if (entry.StyleId == null)
				{
					entry.StyleId = "entryPassword";
				}
				columnDefinition.SetValue(ColumnDefinition.WidthProperty, new GridLengthTypeConverter().ConvertFromInvariantString("*"));
				grid.GetValue(Grid.ColumnDefinitionsProperty).Add(columnDefinition);
				columnDefinition2.SetValue(ColumnDefinition.WidthProperty, new GridLengthTypeConverter().ConvertFromInvariantString("auto"));
				grid.GetValue(Grid.ColumnDefinitionsProperty).Add(columnDefinition2);
				stackLayout3.SetValue(Grid.ColumnProperty, 0);
				stackLayout3.SetValue(StackLayout.OrientationProperty, 0);
				span.SetValue(Span.FontAttributesProperty, new FontAttributesConverter().ConvertFromInvariantString("Bold"));
				bindingExtension.Path = "Title";
				BindingBase bindingBase = bindingExtension.ProvideValue(null);
				span.SetBinding(Span.TextProperty, bindingBase);
				formattedString.Spans.Add(span);
				span2.SetValue(Span.TextProperty, ": ");
				formattedString.Spans.Add(span2);
				bindingExtension2.Path = "UserFriendlyValue";
				BindingBase bindingBase2 = bindingExtension2.ProvideValue(null);
				span3.SetBinding(Span.TextProperty, bindingBase2);
				formattedString.Spans.Add(span3);
				label.SetValue(Label.FormattedTextProperty, formattedString);
				stackLayout3.Children.Add(label);
				dynamicResourceExtension.Key = "BaseFontSize-";
				IMarkupExtension<DynamicResource> markupExtension = dynamicResourceExtension;
				XamlServiceProvider xamlServiceProvider = new XamlServiceProvider();
				Type typeFromHandle = typeof(IProvideValueTarget);
				int num;
				object[] array = new object[(num = this.parentValues.Length) + 6];
				Array.Copy(this.parentValues, 0, array, 6, num);
				object[] array2 = array;
				array2[0] = span4;
				array2[1] = formattedString2;
				array2[2] = label2;
				array2[3] = stackLayout3;
				array2[4] = grid;
				array2[5] = viewCell;
				object obj;
				xamlServiceProvider.Add(typeFromHandle, obj = new SimpleValueTargetProvider(array2, Span.FontSizeProperty, nameScope));
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
				xamlServiceProvider.Add(typeFromHandle2, new XamlTypeResolver(xmlNamespaceResolver, typeof(CodingBackupPage.<InitializeComponent>_anonXamlCDataTemplate_6).GetTypeInfo().Assembly));
				xamlServiceProvider.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(90, 55)));
				DynamicResource dynamicResource = markupExtension.ProvideValue(xamlServiceProvider);
				span4.SetDynamicResource(Span.FontSizeProperty, dynamicResource.Key);
				bindingExtension3.Path = "Date";
				BindingBase bindingBase3 = bindingExtension3.ProvideValue(null);
				span4.SetBinding(Span.TextProperty, bindingBase3);
				formattedString2.Spans.Add(span4);
				dynamicResourceExtension2.Key = "BaseFontSize-";
				IMarkupExtension<DynamicResource> markupExtension2 = dynamicResourceExtension2;
				XamlServiceProvider xamlServiceProvider2 = new XamlServiceProvider();
				Type typeFromHandle3 = typeof(IProvideValueTarget);
				int num2;
				object[] array3 = new object[(num2 = this.parentValues.Length) + 6];
				Array.Copy(this.parentValues, 0, array3, 6, num2);
				object[] array4 = array3;
				array4[0] = span5;
				array4[1] = formattedString2;
				array4[2] = label2;
				array4[3] = stackLayout3;
				array4[4] = grid;
				array4[5] = viewCell;
				object obj2;
				xamlServiceProvider2.Add(typeFromHandle3, obj2 = new SimpleValueTargetProvider(array4, Span.FontSizeProperty, nameScope));
				xamlServiceProvider2.Add(typeof(IReferenceProvider), obj2);
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
				xamlServiceProvider2.Add(typeFromHandle4, new XamlTypeResolver(xmlNamespaceResolver2, typeof(CodingBackupPage.<InitializeComponent>_anonXamlCDataTemplate_6).GetTypeInfo().Assembly));
				xamlServiceProvider2.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(91, 55)));
				DynamicResource dynamicResource2 = markupExtension2.ProvideValue(xamlServiceProvider2);
				span5.SetDynamicResource(Span.FontSizeProperty, dynamicResource2.Key);
				span5.SetValue(Span.TextProperty, " / VIN:");
				formattedString2.Spans.Add(span5);
				dynamicResourceExtension3.Key = "BaseFontSize-";
				IMarkupExtension<DynamicResource> markupExtension3 = dynamicResourceExtension3;
				XamlServiceProvider xamlServiceProvider3 = new XamlServiceProvider();
				Type typeFromHandle5 = typeof(IProvideValueTarget);
				int num3;
				object[] array5 = new object[(num3 = this.parentValues.Length) + 6];
				Array.Copy(this.parentValues, 0, array5, 6, num3);
				object[] array6 = array5;
				array6[0] = span6;
				array6[1] = formattedString2;
				array6[2] = label2;
				array6[3] = stackLayout3;
				array6[4] = grid;
				array6[5] = viewCell;
				object obj3;
				xamlServiceProvider3.Add(typeFromHandle5, obj3 = new SimpleValueTargetProvider(array6, Span.FontSizeProperty, nameScope));
				xamlServiceProvider3.Add(typeof(IReferenceProvider), obj3);
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
				xamlServiceProvider3.Add(typeFromHandle6, new XamlTypeResolver(xmlNamespaceResolver3, typeof(CodingBackupPage.<InitializeComponent>_anonXamlCDataTemplate_6).GetTypeInfo().Assembly));
				xamlServiceProvider3.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(92, 55)));
				DynamicResource dynamicResource3 = markupExtension3.ProvideValue(xamlServiceProvider3);
				span6.SetDynamicResource(Span.FontSizeProperty, dynamicResource3.Key);
				bindingExtension4.Path = "VIN";
				BindingBase bindingBase4 = bindingExtension4.ProvideValue(null);
				span6.SetBinding(Span.TextProperty, bindingBase4);
				formattedString2.Spans.Add(span6);
				label2.SetValue(Label.FormattedTextProperty, formattedString2);
				stackLayout3.Children.Add(label2);
				dynamicResourceExtension4.Key = "BaseFontSize-";
				IMarkupExtension<DynamicResource> markupExtension4 = dynamicResourceExtension4;
				XamlServiceProvider xamlServiceProvider4 = new XamlServiceProvider();
				Type typeFromHandle7 = typeof(IProvideValueTarget);
				int num4;
				object[] array7 = new object[(num4 = this.parentValues.Length) + 4];
				Array.Copy(this.parentValues, 0, array7, 4, num4);
				object[] array8 = array7;
				array8[0] = label3;
				array8[1] = stackLayout3;
				array8[2] = grid;
				array8[3] = viewCell;
				object obj4;
				xamlServiceProvider4.Add(typeFromHandle7, obj4 = new SimpleValueTargetProvider(array8, Label.FontSizeProperty, nameScope));
				xamlServiceProvider4.Add(typeof(IReferenceProvider), obj4);
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
				xamlServiceProvider4.Add(typeFromHandle8, new XamlTypeResolver(xmlNamespaceResolver4, typeof(CodingBackupPage.<InitializeComponent>_anonXamlCDataTemplate_6).GetTypeInfo().Assembly));
				xamlServiceProvider4.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(97, 41)));
				DynamicResource dynamicResource4 = markupExtension4.ProvideValue(xamlServiceProvider4);
				label3.SetDynamicResource(Label.FontSizeProperty, dynamicResource4.Key);
				bindingExtension5.Mode = 4;
				bindingExtension5.Path = "HasAdditionalGeneratedInfo";
				BindingBase bindingBase5 = bindingExtension5.ProvideValue(null);
				label3.SetBinding(VisualElement.IsVisibleProperty, bindingBase5);
				bindingExtension6.Mode = 4;
				bindingExtension6.Path = "AdditionalGeneratedInfo";
				BindingBase bindingBase6 = bindingExtension6.ProvideValue(null);
				label3.SetBinding(Label.TextProperty, bindingBase6);
				stackLayout3.Children.Add(label3);
				staticResourceExtension.Key = "VagCodingPlatformToTrueConverter";
				IMarkupExtension markupExtension5 = staticResourceExtension;
				XamlServiceProvider xamlServiceProvider5 = new XamlServiceProvider();
				Type typeFromHandle9 = typeof(IProvideValueTarget);
				int num5;
				object[] array9 = new object[(num5 = this.parentValues.Length) + 5];
				Array.Copy(this.parentValues, 0, array9, 5, num5);
				object[] array10 = array9;
				array10[0] = bindingExtension7;
				array10[1] = stackLayout2;
				array10[2] = stackLayout3;
				array10[3] = grid;
				array10[4] = viewCell;
				object obj5;
				xamlServiceProvider5.Add(typeFromHandle9, obj5 = new SimpleValueTargetProvider(array10, typeof(BindingExtension).GetRuntimeProperty("Converter"), nameScope));
				xamlServiceProvider5.Add(typeof(IReferenceProvider), obj5);
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
				xamlServiceProvider5.Add(typeFromHandle10, new XamlTypeResolver(xmlNamespaceResolver5, typeof(CodingBackupPage.<InitializeComponent>_anonXamlCDataTemplate_6).GetTypeInfo().Assembly));
				xamlServiceProvider5.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(100, 50)));
				object obj6 = markupExtension5.ProvideValue(xamlServiceProvider5);
				bindingExtension7.Converter = obj6;
				bindingExtension7.Path = ".";
				BindingBase bindingBase7 = bindingExtension7.ProvideValue(null);
				stackLayout2.SetBinding(VisualElement.IsVisibleProperty, bindingBase7);
				stackLayout2.SetValue(StackLayout.OrientationProperty, 0);
				bindingExtension8.Mode = 2;
				staticResourceExtension2.Key = "BoolConverterAlwaysTrueOnIOS";
				IMarkupExtension markupExtension6 = staticResourceExtension2;
				XamlServiceProvider xamlServiceProvider6 = new XamlServiceProvider();
				Type typeFromHandle11 = typeof(IProvideValueTarget);
				int num6;
				object[] array11 = new object[(num6 = this.parentValues.Length) + 6];
				Array.Copy(this.parentValues, 0, array11, 6, num6);
				object[] array12 = array11;
				array12[0] = bindingExtension8;
				array12[1] = stackLayout;
				array12[2] = stackLayout2;
				array12[3] = stackLayout3;
				array12[4] = grid;
				array12[5] = viewCell;
				object obj7;
				xamlServiceProvider6.Add(typeFromHandle11, obj7 = new SimpleValueTargetProvider(array12, typeof(BindingExtension).GetRuntimeProperty("Converter"), nameScope));
				xamlServiceProvider6.Add(typeof(IReferenceProvider), obj7);
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
				xamlServiceProvider6.Add(typeFromHandle12, new XamlTypeResolver(xmlNamespaceResolver6, typeof(CodingBackupPage.<InitializeComponent>_anonXamlCDataTemplate_6).GetTypeInfo().Assembly));
				xamlServiceProvider6.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(101, 54)));
				object obj8 = markupExtension6.ProvideValue(xamlServiceProvider6);
				bindingExtension8.Converter = obj8;
				bindingExtension8.Path = "Selected";
				BindingBase bindingBase8 = bindingExtension8.ProvideValue(null);
				stackLayout.SetBinding(VisualElement.IsVisibleProperty, bindingBase8);
				stackLayout.SetValue(StackLayout.OrientationProperty, 1);
				translate.Text = "coding_Password";
				IMarkupExtension markupExtension7 = translate;
				XamlServiceProvider xamlServiceProvider7 = new XamlServiceProvider();
				Type typeFromHandle13 = typeof(IProvideValueTarget);
				int num7;
				object[] array13 = new object[(num7 = this.parentValues.Length) + 6];
				Array.Copy(this.parentValues, 0, array13, 6, num7);
				object[] array14 = array13;
				array14[0] = label4;
				array14[1] = stackLayout;
				array14[2] = stackLayout2;
				array14[3] = stackLayout3;
				array14[4] = grid;
				array14[5] = viewCell;
				object obj9;
				xamlServiceProvider7.Add(typeFromHandle13, obj9 = new SimpleValueTargetProvider(array14, Label.TextProperty, nameScope));
				xamlServiceProvider7.Add(typeof(IReferenceProvider), obj9);
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
				xamlServiceProvider7.Add(typeFromHandle14, new XamlTypeResolver(xmlNamespaceResolver7, typeof(CodingBackupPage.<InitializeComponent>_anonXamlCDataTemplate_6).GetTypeInfo().Assembly));
				xamlServiceProvider7.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(102, 52)));
				object obj10 = markupExtension7.ProvideValue(xamlServiceProvider7);
				label4.Text = obj10;
				label4.SetValue(View.VerticalOptionsProperty, LayoutOptions.Center);
				stackLayout.Children.Add(label4);
				bindingExtension9.Mode = 1;
				bindingExtension9.Path = "Password";
				BindingBase bindingBase9 = bindingExtension9.ProvideValue(null);
				entry.SetBinding(Entry.TextProperty, bindingBase9);
				entry.SetValue(View.VerticalOptionsProperty, LayoutOptions.Center);
				entry.SetValue(VisualElement.WidthRequestProperty, 100.0);
				stackLayout.Children.Add(entry);
				stackLayout2.Children.Add(stackLayout);
				stackLayout3.Children.Add(stackLayout2);
				grid.Children.Add(stackLayout3);
				checkBoxWithColor.SetValue(Grid.ColumnProperty, 1);
				checkBoxWithColor.CheckedChanged += this.root.CheckBox_CheckedChanged;
				checkBoxWithColor.SetValue(View.HorizontalOptionsProperty, LayoutOptions.End);
				bindingExtension10.Mode = 1;
				bindingExtension10.Path = "Selected";
				BindingBase bindingBase10 = bindingExtension10.ProvideValue(null);
				checkBoxWithColor.SetBinding(CheckBox.IsCheckedProperty, bindingBase10);
				checkBoxWithColor.SetValue(View.VerticalOptionsProperty, LayoutOptions.Start);
				grid.Children.Add(checkBoxWithColor);
				viewCell.View = grid;
				return viewCell;
			}

			// Token: 0x04002DC0 RID: 11712
			internal object[] parentValues;

			// Token: 0x04002DC1 RID: 11713
			internal CodingBackupPage root;
		}
	}
}
