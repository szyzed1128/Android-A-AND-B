using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using AiForms.Renderers;
using CarScannerXamarinForms.Backup;
using CarScannerXamarinForms.Common;
using CarScannerXamarinForms.PlatformAdapters;
using CarScannerXamarinForms.UserControls;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Internals;
using Xamarin.Forms.PlatformConfiguration.iOSSpecific;
using Xamarin.Forms.Xaml;
using Xamarin.Forms.Xaml.Diagnostics;
using Xamarin.Forms.Xaml.Internals;

namespace CarScannerXamarinForms.Settings
{
	// Token: 0x02000217 RID: 535
	[XamlCompilation(2)]
	[XamlFilePath("Settings\\SettingsBackup.xaml")]
	public class SettingsBackup : ContentPage
	{
		// Token: 0x06001AB8 RID: 6840 RVA: 0x00127FC4 File Offset: 0x001261C4
		public SettingsBackup()
		{
			this.InitializeComponent();
			this.InitModel();
		}

		// Token: 0x06001AB9 RID: 6841 RVA: 0x00127FD8 File Offset: 0x001261D8
		private void InitModel()
		{
			this.activityFrame.IsVisible = true;
			this.btnRestoreFromBackup.IsEnabled = false;
			this.btnShareBackup.IsEnabled = false;
			List<FileSystemElement> files = BackupManager.GetFiles();
			this.lv.ItemsSource = files;
			this.lv.SelectedItem = null;
			this.activityFrame.IsVisible = false;
		}

		// Token: 0x06001ABA RID: 6842 RVA: 0x00128033 File Offset: 0x00126233
		private void lv_ItemSelected(object sender, SelectedItemChangedEventArgs e)
		{
			if (e.SelectedItem == null)
			{
				this.btnRestoreFromBackup.IsEnabled = false;
				this.btnShareBackup.IsEnabled = false;
				return;
			}
			this.btnRestoreFromBackup.IsEnabled = true;
			this.btnShareBackup.IsEnabled = true;
		}

		// Token: 0x06001ABB RID: 6843 RVA: 0x00128070 File Offset: 0x00126270
		private void btnShare_Clicked(object sender, EventArgs e)
		{
			FileSystemElement fileSystemElement = (sender as MenuItem).BindingContext as FileSystemElement;
			this.ShareFile(fileSystemElement.Path);
		}

		// Token: 0x06001ABC RID: 6844 RVA: 0x0012809C File Offset: 0x0012629C
		private async void ShareFile(string path)
		{
			try
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
						EmailMessage emailMessage = new EmailMessage("Car Scanner Backup", "", Array.Empty<string>());
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
						Share.RequestAsync(new ShareFileRequest(new ShareFile(path)));
					}
					else
					{
						PlatformHelper.IOSService.SendDebugEmail(path, "", "", "");
					}
				}
			}
			catch (Exception)
			{
			}
		}

		// Token: 0x06001ABD RID: 6845 RVA: 0x001280DC File Offset: 0x001262DC
		private async void btnDelete_Clicked(object sender, EventArgs e)
		{
			FileSystemElement fileSystemElement = (sender as MenuItem).BindingContext as FileSystemElement;
			await this.DeleteFile(fileSystemElement);
		}

		// Token: 0x06001ABE RID: 6846 RVA: 0x0012811C File Offset: 0x0012631C
		private void mnuRename_Clicked(object sender, EventArgs e)
		{
			FileSystemElement fileSystemElement = (sender as MenuItem).BindingContext as FileSystemElement;
			this.RenameFile(fileSystemElement);
		}

		// Token: 0x06001ABF RID: 6847 RVA: 0x00128144 File Offset: 0x00126344
		private async Task DeleteFile(FileSystemElement file)
		{
			TaskAwaiter<bool> taskAwaiter = base.DisplayAlert(Translate.GetString("ios_DeleteBackup"), file.Name, "OK", Translate.GetString("btnCancel.Content")).GetAwaiter();
			if (!taskAwaiter.IsCompleted)
			{
				await taskAwaiter;
				TaskAwaiter<bool> taskAwaiter2;
				taskAwaiter = taskAwaiter2;
				taskAwaiter2 = default(TaskAwaiter<bool>);
			}
			if (taskAwaiter.GetResult())
			{
				file.Delete();
				this.InitModel();
			}
		}

		// Token: 0x06001AC0 RID: 6848 RVA: 0x00128190 File Offset: 0x00126390
		private async void btnCreateBackup_Clicked(object sender, EventArgs e)
		{
			SettingsBackup.<>c__DisplayClass8_0 CS$<>8__locals1 = new SettingsBackup.<>c__DisplayClass8_0();
			CS$<>8__locals1.<>4__this = this;
			this.activityFrame.ResetTextToDefault();
			this.activityFrame.IsVisible = true;
			bool flag = await base.DisplayAlert(Translate.GetString("ios_IncludeDataRecordsTitle"), Translate.GetString("ios_IncludeDataRecordsText"), Translate.GetString("ios_Yes"), Translate.GetString("ios_No"));
			CS$<>8__locals1.include_records = flag;
			CS$<>8__locals1.progress_handler = new Progress<int>(delegate(int p)
			{
				Device.BeginInvokeOnMainThread(delegate
				{
					CS$<>8__locals1.<>4__this.activityFrame.Text = p.ToString() + "%";
				});
			});
			await Task.Run(delegate
			{
				SettingsBackup.<>c__DisplayClass8_0.<<btnCreateBackup_Clicked>b__1>d <<btnCreateBackup_Clicked>b__1>d;
				<<btnCreateBackup_Clicked>b__1>d.<>t__builder = AsyncTaskMethodBuilder.Create();
				<<btnCreateBackup_Clicked>b__1>d.<>4__this = CS$<>8__locals1;
				<<btnCreateBackup_Clicked>b__1>d.<>1__state = -1;
				<<btnCreateBackup_Clicked>b__1>d.<>t__builder.Start<SettingsBackup.<>c__DisplayClass8_0.<<btnCreateBackup_Clicked>b__1>d>(ref <<btnCreateBackup_Clicked>b__1>d);
				return <<btnCreateBackup_Clicked>b__1>d.<>t__builder.Task;
			});
			this.InitModel();
			this.activityFrame.IsVisible = false;
		}

		// Token: 0x06001AC1 RID: 6849 RVA: 0x001281C8 File Offset: 0x001263C8
		private async void btnRestoreFromBackup_Clicked(object sender, EventArgs e)
		{
			SettingsBackup.<>c__DisplayClass9_0 CS$<>8__locals1 = new SettingsBackup.<>c__DisplayClass9_0();
			if (this.lv.SelectedItem != null)
			{
				CS$<>8__locals1.file = this.lv.SelectedItem as FileSystemElement;
				string text = Translate.GetString("ios_RestoreBackupConfirmationTitle");
				text = string.Format(text, CS$<>8__locals1.file.Name);
				TaskAwaiter<bool> taskAwaiter = base.DisplayAlert(text, Translate.GetString("ios_RestoreBackupConfirmationText"), "OK", Translate.GetString("btnCancel.Content")).GetAwaiter();
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
					CS$<>8__locals1.new_car = "";
					await Task.Run(delegate
					{
						SettingsBackup.<>c__DisplayClass9_0.<<btnRestoreFromBackup_Clicked>b__0>d <<btnRestoreFromBackup_Clicked>b__0>d;
						<<btnRestoreFromBackup_Clicked>b__0>d.<>t__builder = AsyncTaskMethodBuilder.Create();
						<<btnRestoreFromBackup_Clicked>b__0>d.<>4__this = CS$<>8__locals1;
						<<btnRestoreFromBackup_Clicked>b__0>d.<>1__state = -1;
						<<btnRestoreFromBackup_Clicked>b__0>d.<>t__builder.Start<SettingsBackup.<>c__DisplayClass9_0.<<btnRestoreFromBackup_Clicked>b__0>d>(ref <<btnRestoreFromBackup_Clicked>b__0>d);
						return <<btnRestoreFromBackup_Clicked>b__0>d.<>t__builder.Task;
					});
					this.activityFrame.IsVisible = false;
					await base.DisplayAlert(Translate.GetString("ios_RestoreFinishedTitle"), string.Format(Translate.GetString("ios_RestoreFinishedText"), CS$<>8__locals1.new_car), "OK");
				}
			}
		}

		// Token: 0x06001AC2 RID: 6850 RVA: 0x000027D4 File Offset: 0x000009D4
		private void ContentPage_Disappearing(object sender, EventArgs e)
		{
		}

		// Token: 0x06001AC3 RID: 6851 RVA: 0x000027D4 File Offset: 0x000009D4
		private void ContentPage_Appearing(object sender, EventArgs e)
		{
		}

		// Token: 0x06001AC4 RID: 6852 RVA: 0x000027D4 File Offset: 0x000009D4
		private void ContentPage_SizeChanged(object sender, EventArgs e)
		{
		}

		// Token: 0x06001AC5 RID: 6853 RVA: 0x00128200 File Offset: 0x00126400
		private void btnShareBackup_Clicked(object sender, EventArgs e)
		{
			if (this.lv.SelectedItem != null)
			{
				FileSystemElement fileSystemElement = (FileSystemElement)this.lv.SelectedItem;
				this.ShareFile(fileSystemElement.Path);
			}
		}

		// Token: 0x06001AC6 RID: 6854 RVA: 0x00128238 File Offset: 0x00126438
		private void btnRenameBackup_Clicked(object sender, EventArgs e)
		{
			if (this.lv.SelectedItem == null)
			{
				return;
			}
			FileSystemElement fileSystemElement = this.lv.SelectedItem as FileSystemElement;
			this.RenameFile(fileSystemElement);
		}

		// Token: 0x06001AC7 RID: 6855 RVA: 0x0012826C File Offset: 0x0012646C
		private void RenameFile(FileSystemElement file)
		{
			SettingsRecordRename settingsRecordRename = new SettingsRecordRename(file, delegate
			{
				this.InitModel();
			});
			base.Navigation.PushAsync(settingsRecordRename);
		}

		// Token: 0x06001AC8 RID: 6856 RVA: 0x0012829C File Offset: 0x0012649C
		private async void btnDeleteBackup_Clicked(object sender, EventArgs e)
		{
			if (this.lv.SelectedItem != null)
			{
				FileSystemElement fileSystemElement = this.lv.SelectedItem as FileSystemElement;
				await this.DeleteFile(fileSystemElement);
			}
		}

		// Token: 0x06001AC9 RID: 6857 RVA: 0x001282D4 File Offset: 0x001264D4
		private async void btnImportDroid_Clicked(object sender, EventArgs e)
		{
			try
			{
				FileResult fileResult = await FilePicker.PickAsync(null);
				if (fileResult != null && fileResult.FileName.EndsWith("cbz", StringComparison.OrdinalIgnoreCase))
				{
					string localFilePath = FileSystemHelper.GetLocalFilePath(fileResult.FileName);
					File.Copy(fileResult.FullPath, localFilePath);
					List<FileSystemElement> files = BackupManager.GetFiles();
					this.lv.ItemsSource = files;
				}
			}
			catch (Exception)
			{
			}
		}

		// Token: 0x06001ACA RID: 6858 RVA: 0x0012830C File Offset: 0x0012650C
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private void InitializeComponent()
		{
			if (ResourceLoader.IsEnabled && ResourceLoader.CanProvideContentFor(new ResourceLoader.ResourceLoadingQuery
			{
				AssemblyName = typeof(SettingsBackup).GetTypeInfo().Assembly.GetName(),
				ResourcePath = "Settings/SettingsBackup.xaml",
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
			VisualDiagnostics.RegisterSourceInfo(translate = new Translate(), new Uri("Settings\\SettingsBackup.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 9, 5);
			DynamicResourceExtension dynamicResourceExtension;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension = new DynamicResourceExtension(), new Uri("Settings\\SettingsBackup.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 13, 5);
			DynamicResourceExtension dynamicResourceExtension2;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension2 = new DynamicResourceExtension(), new Uri("Settings\\SettingsBackup.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 17, 32);
			RowDefinition rowDefinition;
			VisualDiagnostics.RegisterSourceInfo(rowDefinition = new RowDefinition(), new Uri("Settings\\SettingsBackup.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 19, 18);
			RowDefinition rowDefinition2;
			VisualDiagnostics.RegisterSourceInfo(rowDefinition2 = new RowDefinition(), new Uri("Settings\\SettingsBackup.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 20, 18);
			DataTemplate dataTemplate;
			VisualDiagnostics.RegisterSourceInfo(dataTemplate = new DataTemplate(), new Uri("Settings\\SettingsBackup.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 28, 22);
			ReferenceExtension referenceExtension;
			VisualDiagnostics.RegisterSourceInfo(referenceExtension = new ReferenceExtension(), new Uri("Settings\\SettingsBackup.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 65, 25);
			BindingExtension bindingExtension;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension = new BindingExtension(), new Uri("Settings\\SettingsBackup.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 65, 25);
			Translate translate2;
			VisualDiagnostics.RegisterSourceInfo(translate2 = new Translate(), new Uri("Settings\\SettingsBackup.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 66, 37);
			Translate translate3;
			VisualDiagnostics.RegisterSourceInfo(translate3 = new Translate(), new Uri("Settings\\SettingsBackup.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 69, 33);
			DynamicResourceExtension dynamicResourceExtension3;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension3 = new DynamicResourceExtension(), new Uri("Settings\\SettingsBackup.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 71, 33);
			ButtonCell buttonCell;
			VisualDiagnostics.RegisterSourceInfo(buttonCell = new ButtonCell(), new Uri("Settings\\SettingsBackup.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 67, 30);
			Translate translate4;
			VisualDiagnostics.RegisterSourceInfo(translate4 = new Translate(), new Uri("Settings\\SettingsBackup.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 74, 33);
			DynamicResourceExtension dynamicResourceExtension4;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension4 = new DynamicResourceExtension(), new Uri("Settings\\SettingsBackup.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 76, 33);
			ButtonCell buttonCell2;
			VisualDiagnostics.RegisterSourceInfo(buttonCell2 = new ButtonCell(), new Uri("Settings\\SettingsBackup.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 72, 30);
			Translate translate5;
			VisualDiagnostics.RegisterSourceInfo(translate5 = new Translate(), new Uri("Settings\\SettingsBackup.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 80, 33);
			DynamicResourceExtension dynamicResourceExtension5;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension5 = new DynamicResourceExtension(), new Uri("Settings\\SettingsBackup.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 86, 33);
			ButtonCell buttonCell3;
			VisualDiagnostics.RegisterSourceInfo(buttonCell3 = new ButtonCell(), new Uri("Settings\\SettingsBackup.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 78, 30);
			Translate translate6;
			VisualDiagnostics.RegisterSourceInfo(translate6 = new Translate(), new Uri("Settings\\SettingsBackup.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 89, 33);
			DynamicResourceExtension dynamicResourceExtension6;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension6 = new DynamicResourceExtension(), new Uri("Settings\\SettingsBackup.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 96, 33);
			ButtonCell buttonCell4;
			VisualDiagnostics.RegisterSourceInfo(buttonCell4 = new ButtonCell(), new Uri("Settings\\SettingsBackup.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 87, 30);
			Section section;
			VisualDiagnostics.RegisterSourceInfo(section = new Section(), new Uri("Settings\\SettingsBackup.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 66, 26);
			SettingsView settingsView;
			VisualDiagnostics.RegisterSourceInfo(settingsView = new SettingsView(), new Uri("Settings\\SettingsBackup.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 62, 22);
			ListView listView;
			VisualDiagnostics.RegisterSourceInfo(listView = new ListView(), new Uri("Settings\\SettingsBackup.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 22, 14);
			ActivityFrame activityFrame;
			VisualDiagnostics.RegisterSourceInfo(activityFrame = new ActivityFrame(), new Uri("Settings\\SettingsBackup.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 101, 14);
			Grid grid;
			VisualDiagnostics.RegisterSourceInfo(grid = new Grid(), new Uri("Settings\\SettingsBackup.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 17, 10);
			VisualDiagnostics.RegisterSourceInfo(this, new Uri("Settings\\SettingsBackup.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 2, 2);
			NameScope nameScope = NameScope.GetNameScope(this) ?? new NameScope();
			NameScope.SetNameScope(this, nameScope);
			nameScope.RegisterName("lv", listView);
			if (listView.StyleId == null)
			{
				listView.StyleId = "lv";
			}
			nameScope.RegisterName("settingsFooter", settingsView);
			if (settingsView.StyleId == null)
			{
				settingsView.StyleId = "settingsFooter";
			}
			nameScope.RegisterName("btnCreateBackup", buttonCell);
			if (buttonCell.StyleId == null)
			{
				buttonCell.StyleId = "btnCreateBackup";
			}
			nameScope.RegisterName("btnRestoreFromBackup", buttonCell2);
			if (buttonCell2.StyleId == null)
			{
				buttonCell2.StyleId = "btnRestoreFromBackup";
			}
			nameScope.RegisterName("btnShareBackup", buttonCell3);
			if (buttonCell3.StyleId == null)
			{
				buttonCell3.StyleId = "btnShareBackup";
			}
			nameScope.RegisterName("btnImportDroid", buttonCell4);
			if (buttonCell4.StyleId == null)
			{
				buttonCell4.StyleId = "btnImportDroid";
			}
			nameScope.RegisterName("activityFrame", activityFrame);
			if (activityFrame.StyleId == null)
			{
				activityFrame.StyleId = "activityFrame";
			}
			this.lv = listView;
			this.settingsFooter = settingsView;
			this.btnCreateBackup = buttonCell;
			this.btnRestoreFromBackup = buttonCell2;
			this.btnShareBackup = buttonCell3;
			this.btnImportDroid = buttonCell4;
			this.activityFrame = activityFrame;
			translate.Text = "ios_BackupTitle";
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
			xmlNamespaceResolver.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xamlServiceProvider.Add(typeFromHandle2, new XamlTypeResolver(xmlNamespaceResolver, typeof(SettingsBackup).GetTypeInfo().Assembly));
			xamlServiceProvider.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(9, 5)));
			object obj2 = markupExtension.ProvideValue(xamlServiceProvider);
			this.Title = obj2;
			this.SetValue(Page.PaddingProperty, new Thickness(0.0, 0.0, 0.0, 0.0));
			this.SetValue(Page.UseSafeAreaProperty, true);
			this.Appearing += this.ContentPage_Appearing;
			dynamicResourceExtension.Key = "SettingsBackground";
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
			xmlNamespaceResolver2.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xamlServiceProvider2.Add(typeFromHandle4, new XamlTypeResolver(xmlNamespaceResolver2, typeof(SettingsBackup).GetTypeInfo().Assembly));
			xamlServiceProvider2.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(13, 5)));
			DynamicResource dynamicResource = markupExtension2.ProvideValue(xamlServiceProvider2);
			this.SetDynamicResource(VisualElement.BackgroundColorProperty, dynamicResource.Key);
			this.Disappearing += this.ContentPage_Disappearing;
			this.SizeChanged += this.ContentPage_SizeChanged;
			grid.SetValue(View.MarginProperty, new Thickness(5.0, 0.0, 5.0, 0.0));
			dynamicResourceExtension2.Key = "SettingsCellBackground";
			IMarkupExtension<DynamicResource> markupExtension3 = dynamicResourceExtension2;
			XamlServiceProvider xamlServiceProvider3 = new XamlServiceProvider();
			Type typeFromHandle5 = typeof(IProvideValueTarget);
			object[] array3 = new object[0 + 2];
			array3[0] = grid;
			array3[1] = this;
			object obj4;
			xamlServiceProvider3.Add(typeFromHandle5, obj4 = new SimpleValueTargetProvider(array3, VisualElement.BackgroundColorProperty, nameScope));
			xamlServiceProvider3.Add(typeof(IReferenceProvider), obj4);
			Type typeFromHandle6 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver3 = new XmlNamespaceResolver();
			xmlNamespaceResolver3.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver3.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver3.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver3.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver3.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xamlServiceProvider3.Add(typeFromHandle6, new XamlTypeResolver(xmlNamespaceResolver3, typeof(SettingsBackup).GetTypeInfo().Assembly));
			xamlServiceProvider3.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(17, 32)));
			DynamicResource dynamicResource2 = markupExtension3.ProvideValue(xamlServiceProvider3);
			grid.SetDynamicResource(VisualElement.BackgroundColorProperty, dynamicResource2.Key);
			rowDefinition.SetValue(RowDefinition.HeightProperty, new GridLengthTypeConverter().ConvertFromInvariantString("*"));
			grid.GetValue(Grid.RowDefinitionsProperty).Add(rowDefinition);
			rowDefinition2.SetValue(RowDefinition.HeightProperty, new GridLengthTypeConverter().ConvertFromInvariantString("auto"));
			grid.GetValue(Grid.RowDefinitionsProperty).Add(rowDefinition2);
			listView.SetValue(Grid.RowProperty, 0);
			listView.SetValue(VisualElement.BackgroundColorProperty, Color.Transparent);
			listView.ItemSelected += this.lv_ItemSelected;
			IDataTemplate dataTemplate2 = dataTemplate;
			SettingsBackup.<InitializeComponent>_anonXamlCDataTemplate_89 <InitializeComponent>_anonXamlCDataTemplate_ = new SettingsBackup.<InitializeComponent>_anonXamlCDataTemplate_89();
			object[] array4 = new object[0 + 4];
			array4[0] = dataTemplate;
			array4[1] = listView;
			array4[2] = grid;
			array4[3] = this;
			<InitializeComponent>_anonXamlCDataTemplate_.parentValues = array4;
			<InitializeComponent>_anonXamlCDataTemplate_.root = this;
			dataTemplate2.LoadTemplate = new Func<object>(<InitializeComponent>_anonXamlCDataTemplate_.LoadDataTemplate);
			listView.SetValue(ItemsView<Cell>.ItemTemplateProperty, dataTemplate);
			settingsView.SetValue(TableView.HasUnevenRowsProperty, false);
			referenceExtension.Name = "settingsFooter";
			IMarkupExtension markupExtension4 = referenceExtension;
			XamlServiceProvider xamlServiceProvider4 = new XamlServiceProvider();
			Type typeFromHandle7 = typeof(IProvideValueTarget);
			object[] array5 = new object[0 + 5];
			array5[0] = bindingExtension;
			array5[1] = settingsView;
			array5[2] = listView;
			array5[3] = grid;
			array5[4] = this;
			object obj5;
			xamlServiceProvider4.Add(typeFromHandle7, obj5 = new SimpleValueTargetProvider(array5, typeof(BindingExtension).GetRuntimeProperty("Source"), nameScope));
			xamlServiceProvider4.Add(typeof(IReferenceProvider), obj5);
			Type typeFromHandle8 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver4 = new XmlNamespaceResolver();
			xmlNamespaceResolver4.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver4.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver4.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver4.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver4.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xamlServiceProvider4.Add(typeFromHandle8, new XamlTypeResolver(xmlNamespaceResolver4, typeof(SettingsBackup).GetTypeInfo().Assembly));
			xamlServiceProvider4.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(65, 25)));
			object obj6 = markupExtension4.ProvideValue(xamlServiceProvider4);
			bindingExtension.Source = obj6;
			bindingExtension.Path = "VisibleContentHeight";
			BindingBase bindingBase = bindingExtension.ProvideValue(null);
			settingsView.SetBinding(VisualElement.HeightRequestProperty, bindingBase);
			translate2.Text = "settings_ChooseAction";
			IMarkupExtension markupExtension5 = translate2;
			XamlServiceProvider xamlServiceProvider5 = new XamlServiceProvider();
			Type typeFromHandle9 = typeof(IProvideValueTarget);
			object[] array6 = new object[0 + 5];
			array6[0] = section;
			array6[1] = settingsView;
			array6[2] = listView;
			array6[3] = grid;
			array6[4] = this;
			object obj7;
			xamlServiceProvider5.Add(typeFromHandle9, obj7 = new SimpleValueTargetProvider(array6, SectionBase.TitleProperty, nameScope));
			xamlServiceProvider5.Add(typeof(IReferenceProvider), obj7);
			Type typeFromHandle10 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver5 = new XmlNamespaceResolver();
			xmlNamespaceResolver5.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver5.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver5.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver5.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver5.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xamlServiceProvider5.Add(typeFromHandle10, new XamlTypeResolver(xmlNamespaceResolver5, typeof(SettingsBackup).GetTypeInfo().Assembly));
			xamlServiceProvider5.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(66, 37)));
			object obj8 = markupExtension5.ProvideValue(xamlServiceProvider5);
			section.Title = obj8;
			translate3.Text = "ios_CreateBackup";
			IMarkupExtension markupExtension6 = translate3;
			XamlServiceProvider xamlServiceProvider6 = new XamlServiceProvider();
			Type typeFromHandle11 = typeof(IProvideValueTarget);
			object[] array7 = new object[0 + 6];
			array7[0] = buttonCell;
			array7[1] = section;
			array7[2] = settingsView;
			array7[3] = listView;
			array7[4] = grid;
			array7[5] = this;
			object obj9;
			xamlServiceProvider6.Add(typeFromHandle11, obj9 = new SimpleValueTargetProvider(array7, CellBase.TitleProperty, nameScope));
			xamlServiceProvider6.Add(typeof(IReferenceProvider), obj9);
			Type typeFromHandle12 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver6 = new XmlNamespaceResolver();
			xmlNamespaceResolver6.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver6.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver6.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver6.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver6.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xamlServiceProvider6.Add(typeFromHandle12, new XamlTypeResolver(xmlNamespaceResolver6, typeof(SettingsBackup).GetTypeInfo().Assembly));
			xamlServiceProvider6.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(69, 33)));
			object obj10 = markupExtension6.ProvideValue(xamlServiceProvider6);
			buttonCell.Title = obj10;
			buttonCell.Tapped += this.btnCreateBackup_Clicked;
			dynamicResourceExtension3.Key = "ButtonAccentColor";
			IMarkupExtension<DynamicResource> markupExtension7 = dynamicResourceExtension3;
			XamlServiceProvider xamlServiceProvider7 = new XamlServiceProvider();
			Type typeFromHandle13 = typeof(IProvideValueTarget);
			object[] array8 = new object[0 + 6];
			array8[0] = buttonCell;
			array8[1] = section;
			array8[2] = settingsView;
			array8[3] = listView;
			array8[4] = grid;
			array8[5] = this;
			object obj11;
			xamlServiceProvider7.Add(typeFromHandle13, obj11 = new SimpleValueTargetProvider(array8, CellBase.TitleColorProperty, nameScope));
			xamlServiceProvider7.Add(typeof(IReferenceProvider), obj11);
			Type typeFromHandle14 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver7 = new XmlNamespaceResolver();
			xmlNamespaceResolver7.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver7.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver7.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver7.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver7.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xamlServiceProvider7.Add(typeFromHandle14, new XamlTypeResolver(xmlNamespaceResolver7, typeof(SettingsBackup).GetTypeInfo().Assembly));
			xamlServiceProvider7.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(71, 33)));
			DynamicResource dynamicResource3 = markupExtension7.ProvideValue(xamlServiceProvider7);
			buttonCell.SetDynamicResource(CellBase.TitleColorProperty, dynamicResource3.Key);
			section.Add(buttonCell);
			translate4.Text = "ios_RestoreBackup";
			IMarkupExtension markupExtension8 = translate4;
			XamlServiceProvider xamlServiceProvider8 = new XamlServiceProvider();
			Type typeFromHandle15 = typeof(IProvideValueTarget);
			object[] array9 = new object[0 + 6];
			array9[0] = buttonCell2;
			array9[1] = section;
			array9[2] = settingsView;
			array9[3] = listView;
			array9[4] = grid;
			array9[5] = this;
			object obj12;
			xamlServiceProvider8.Add(typeFromHandle15, obj12 = new SimpleValueTargetProvider(array9, CellBase.TitleProperty, nameScope));
			xamlServiceProvider8.Add(typeof(IReferenceProvider), obj12);
			Type typeFromHandle16 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver8 = new XmlNamespaceResolver();
			xmlNamespaceResolver8.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver8.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver8.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver8.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver8.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xamlServiceProvider8.Add(typeFromHandle16, new XamlTypeResolver(xmlNamespaceResolver8, typeof(SettingsBackup).GetTypeInfo().Assembly));
			xamlServiceProvider8.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(74, 33)));
			object obj13 = markupExtension8.ProvideValue(xamlServiceProvider8);
			buttonCell2.Title = obj13;
			buttonCell2.Tapped += this.btnRestoreFromBackup_Clicked;
			dynamicResourceExtension4.Key = "ButtonAccentColor";
			IMarkupExtension<DynamicResource> markupExtension9 = dynamicResourceExtension4;
			XamlServiceProvider xamlServiceProvider9 = new XamlServiceProvider();
			Type typeFromHandle17 = typeof(IProvideValueTarget);
			object[] array10 = new object[0 + 6];
			array10[0] = buttonCell2;
			array10[1] = section;
			array10[2] = settingsView;
			array10[3] = listView;
			array10[4] = grid;
			array10[5] = this;
			object obj14;
			xamlServiceProvider9.Add(typeFromHandle17, obj14 = new SimpleValueTargetProvider(array10, CellBase.TitleColorProperty, nameScope));
			xamlServiceProvider9.Add(typeof(IReferenceProvider), obj14);
			Type typeFromHandle18 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver9 = new XmlNamespaceResolver();
			xmlNamespaceResolver9.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver9.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver9.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver9.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver9.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xamlServiceProvider9.Add(typeFromHandle18, new XamlTypeResolver(xmlNamespaceResolver9, typeof(SettingsBackup).GetTypeInfo().Assembly));
			xamlServiceProvider9.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(76, 33)));
			DynamicResource dynamicResource4 = markupExtension9.ProvideValue(xamlServiceProvider9);
			buttonCell2.SetDynamicResource(CellBase.TitleColorProperty, dynamicResource4.Key);
			section.Add(buttonCell2);
			translate5.Text = "ios_Share";
			IMarkupExtension markupExtension10 = translate5;
			XamlServiceProvider xamlServiceProvider10 = new XamlServiceProvider();
			Type typeFromHandle19 = typeof(IProvideValueTarget);
			object[] array11 = new object[0 + 6];
			array11[0] = buttonCell3;
			array11[1] = section;
			array11[2] = settingsView;
			array11[3] = listView;
			array11[4] = grid;
			array11[5] = this;
			object obj15;
			xamlServiceProvider10.Add(typeFromHandle19, obj15 = new SimpleValueTargetProvider(array11, CellBase.TitleProperty, nameScope));
			xamlServiceProvider10.Add(typeof(IReferenceProvider), obj15);
			Type typeFromHandle20 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver10 = new XmlNamespaceResolver();
			xmlNamespaceResolver10.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver10.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver10.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver10.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver10.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xamlServiceProvider10.Add(typeFromHandle20, new XamlTypeResolver(xmlNamespaceResolver10, typeof(SettingsBackup).GetTypeInfo().Assembly));
			xamlServiceProvider10.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(80, 33)));
			object obj16 = markupExtension10.ProvideValue(xamlServiceProvider10);
			buttonCell3.Title = obj16;
			buttonCell3.SetValue(Grid.RowProperty, 3);
			buttonCell3.SetValue(Grid.ColumnProperty, 0);
			buttonCell3.SetValue(Grid.ColumnSpanProperty, 1);
			buttonCell3.SetValue(Cell.IsEnabledProperty, false);
			buttonCell3.Tapped += this.btnShareBackup_Clicked;
			dynamicResourceExtension5.Key = "ButtonAccentColor";
			IMarkupExtension<DynamicResource> markupExtension11 = dynamicResourceExtension5;
			XamlServiceProvider xamlServiceProvider11 = new XamlServiceProvider();
			Type typeFromHandle21 = typeof(IProvideValueTarget);
			object[] array12 = new object[0 + 6];
			array12[0] = buttonCell3;
			array12[1] = section;
			array12[2] = settingsView;
			array12[3] = listView;
			array12[4] = grid;
			array12[5] = this;
			object obj17;
			xamlServiceProvider11.Add(typeFromHandle21, obj17 = new SimpleValueTargetProvider(array12, CellBase.TitleColorProperty, nameScope));
			xamlServiceProvider11.Add(typeof(IReferenceProvider), obj17);
			Type typeFromHandle22 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver11 = new XmlNamespaceResolver();
			xmlNamespaceResolver11.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver11.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver11.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver11.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver11.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xamlServiceProvider11.Add(typeFromHandle22, new XamlTypeResolver(xmlNamespaceResolver11, typeof(SettingsBackup).GetTypeInfo().Assembly));
			xamlServiceProvider11.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(86, 33)));
			DynamicResource dynamicResource5 = markupExtension11.ProvideValue(xamlServiceProvider11);
			buttonCell3.SetDynamicResource(CellBase.TitleColorProperty, dynamicResource5.Key);
			section.Add(buttonCell3);
			translate6.Text = "droid_ImportFromFile";
			IMarkupExtension markupExtension12 = translate6;
			XamlServiceProvider xamlServiceProvider12 = new XamlServiceProvider();
			Type typeFromHandle23 = typeof(IProvideValueTarget);
			object[] array13 = new object[0 + 6];
			array13[0] = buttonCell4;
			array13[1] = section;
			array13[2] = settingsView;
			array13[3] = listView;
			array13[4] = grid;
			array13[5] = this;
			object obj18;
			xamlServiceProvider12.Add(typeFromHandle23, obj18 = new SimpleValueTargetProvider(array13, CellBase.TitleProperty, nameScope));
			xamlServiceProvider12.Add(typeof(IReferenceProvider), obj18);
			Type typeFromHandle24 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver12 = new XmlNamespaceResolver();
			xmlNamespaceResolver12.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver12.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver12.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver12.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver12.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xamlServiceProvider12.Add(typeFromHandle24, new XamlTypeResolver(xmlNamespaceResolver12, typeof(SettingsBackup).GetTypeInfo().Assembly));
			xamlServiceProvider12.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(89, 33)));
			object obj19 = markupExtension12.ProvideValue(xamlServiceProvider12);
			buttonCell4.Title = obj19;
			buttonCell4.SetValue(Grid.RowProperty, 3);
			buttonCell4.SetValue(Grid.ColumnProperty, 1);
			buttonCell4.SetValue(Grid.ColumnSpanProperty, 1);
			buttonCell4.SetValue(Cell.IsEnabledProperty, true);
			buttonCell4.SetValue(CellBase.IsVisibleProperty, true);
			buttonCell4.Tapped += this.btnImportDroid_Clicked;
			dynamicResourceExtension6.Key = "ButtonAccentColor";
			IMarkupExtension<DynamicResource> markupExtension13 = dynamicResourceExtension6;
			XamlServiceProvider xamlServiceProvider13 = new XamlServiceProvider();
			Type typeFromHandle25 = typeof(IProvideValueTarget);
			object[] array14 = new object[0 + 6];
			array14[0] = buttonCell4;
			array14[1] = section;
			array14[2] = settingsView;
			array14[3] = listView;
			array14[4] = grid;
			array14[5] = this;
			object obj20;
			xamlServiceProvider13.Add(typeFromHandle25, obj20 = new SimpleValueTargetProvider(array14, CellBase.TitleColorProperty, nameScope));
			xamlServiceProvider13.Add(typeof(IReferenceProvider), obj20);
			Type typeFromHandle26 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver13 = new XmlNamespaceResolver();
			xmlNamespaceResolver13.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver13.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver13.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver13.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver13.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xamlServiceProvider13.Add(typeFromHandle26, new XamlTypeResolver(xmlNamespaceResolver13, typeof(SettingsBackup).GetTypeInfo().Assembly));
			xamlServiceProvider13.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(96, 33)));
			DynamicResource dynamicResource6 = markupExtension13.ProvideValue(xamlServiceProvider13);
			buttonCell4.SetDynamicResource(CellBase.TitleColorProperty, dynamicResource6.Key);
			section.Add(buttonCell4);
			settingsView.Root.Add(section);
			listView.SetValue(ListView.FooterProperty, settingsView);
			grid.Children.Add(listView);
			activityFrame.SetValue(Grid.RowProperty, 0);
			activityFrame.SetValue(VisualElement.IsVisibleProperty, new VisualElement.VisibilityConverter().ConvertFromInvariantString("false"));
			activityFrame.SetValue(View.VerticalOptionsProperty, LayoutOptions.Start);
			grid.Children.Add(activityFrame);
			this.SetValue(ContentPage.ContentProperty, grid);
		}

		// Token: 0x06001ACB RID: 6859 RVA: 0x00129AE0 File Offset: 0x00127CE0
		[CompilerGenerated]
		private void <RenameFile>b__15_0()
		{
			this.InitModel();
		}

		// Token: 0x06001ACC RID: 6860 RVA: 0x00129AE8 File Offset: 0x00127CE8
		private void __InitComponentRuntime()
		{
			Extensions.LoadFromXaml<SettingsBackup>(this, typeof(SettingsBackup));
			this.lv = NameScopeExtensions.FindByName<ListView>(this, "lv");
			this.settingsFooter = NameScopeExtensions.FindByName<SettingsView>(this, "settingsFooter");
			this.btnCreateBackup = NameScopeExtensions.FindByName<ButtonCell>(this, "btnCreateBackup");
			this.btnRestoreFromBackup = NameScopeExtensions.FindByName<ButtonCell>(this, "btnRestoreFromBackup");
			this.btnShareBackup = NameScopeExtensions.FindByName<ButtonCell>(this, "btnShareBackup");
			this.btnImportDroid = NameScopeExtensions.FindByName<ButtonCell>(this, "btnImportDroid");
			this.activityFrame = NameScopeExtensions.FindByName<ActivityFrame>(this, "activityFrame");
		}

		// Token: 0x04000C05 RID: 3077
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private ListView lv;

		// Token: 0x04000C06 RID: 3078
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private SettingsView settingsFooter;

		// Token: 0x04000C07 RID: 3079
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private ButtonCell btnCreateBackup;

		// Token: 0x04000C08 RID: 3080
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private ButtonCell btnRestoreFromBackup;

		// Token: 0x04000C09 RID: 3081
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private ButtonCell btnShareBackup;

		// Token: 0x04000C0A RID: 3082
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private ButtonCell btnImportDroid;

		// Token: 0x04000C0B RID: 3083
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private ActivityFrame activityFrame;

		// Token: 0x02000218 RID: 536
		[CompilerGenerated]
		private sealed class <>c__DisplayClass8_0
		{
			// Token: 0x06001ACD RID: 6861 RVA: 0x00002050 File Offset: 0x00000250
			public <>c__DisplayClass8_0()
			{
			}

			// Token: 0x06001ACE RID: 6862 RVA: 0x00129B7D File Offset: 0x00127D7D
			internal void <btnCreateBackup_Clicked>b__0(int p)
			{
				Device.BeginInvokeOnMainThread(new Action(new SettingsBackup.<>c__DisplayClass8_1
				{
					CS$<>8__locals1 = this,
					p = p
				}.<btnCreateBackup_Clicked>b__2));
			}

			// Token: 0x06001ACF RID: 6863 RVA: 0x00129BA4 File Offset: 0x00127DA4
			internal async Task <btnCreateBackup_Clicked>b__1()
			{
				await BackupManager.CreateBackup(this.include_records, this.progress_handler);
			}

			// Token: 0x04000C0C RID: 3084
			public SettingsBackup <>4__this;

			// Token: 0x04000C0D RID: 3085
			public bool include_records;

			// Token: 0x04000C0E RID: 3086
			public Progress<int> progress_handler;

			// Token: 0x02000219 RID: 537
			[StructLayout(LayoutKind.Auto)]
			private struct <<btnCreateBackup_Clicked>b__1>d : IAsyncStateMachine
			{
				// Token: 0x06001AD0 RID: 6864 RVA: 0x00129BE8 File Offset: 0x00127DE8
				void IAsyncStateMachine.MoveNext()
				{
					int num2;
					int num = num2;
					SettingsBackup.<>c__DisplayClass8_0 CS$<>8__locals1 = this;
					try
					{
						TaskAwaiter<bool> taskAwaiter;
						if (num != 0)
						{
							taskAwaiter = BackupManager.CreateBackup(CS$<>8__locals1.include_records, CS$<>8__locals1.progress_handler).GetAwaiter();
							if (!taskAwaiter.IsCompleted)
							{
								num2 = 0;
								TaskAwaiter<bool> taskAwaiter2 = taskAwaiter;
								this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<bool>, SettingsBackup.<>c__DisplayClass8_0.<<btnCreateBackup_Clicked>b__1>d>(ref taskAwaiter, ref this);
								return;
							}
						}
						else
						{
							TaskAwaiter<bool> taskAwaiter2;
							taskAwaiter = taskAwaiter2;
							taskAwaiter2 = default(TaskAwaiter<bool>);
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

				// Token: 0x06001AD1 RID: 6865 RVA: 0x00129CA8 File Offset: 0x00127EA8
				[DebuggerHidden]
				void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
				{
					this.<>t__builder.SetStateMachine(stateMachine);
				}

				// Token: 0x04000C0F RID: 3087
				public int <>1__state;

				// Token: 0x04000C10 RID: 3088
				public AsyncTaskMethodBuilder <>t__builder;

				// Token: 0x04000C11 RID: 3089
				public SettingsBackup.<>c__DisplayClass8_0 <>4__this;

				// Token: 0x04000C12 RID: 3090
				private TaskAwaiter<bool> <>u__1;
			}
		}

		// Token: 0x0200021A RID: 538
		[CompilerGenerated]
		private sealed class <>c__DisplayClass8_1
		{
			// Token: 0x06001AD2 RID: 6866 RVA: 0x00002050 File Offset: 0x00000250
			public <>c__DisplayClass8_1()
			{
			}

			// Token: 0x06001AD3 RID: 6867 RVA: 0x00129CB6 File Offset: 0x00127EB6
			internal void <btnCreateBackup_Clicked>b__2()
			{
				this.CS$<>8__locals1.<>4__this.activityFrame.Text = this.p.ToString() + "%";
			}

			// Token: 0x04000C13 RID: 3091
			public int p;

			// Token: 0x04000C14 RID: 3092
			public SettingsBackup.<>c__DisplayClass8_0 CS$<>8__locals1;
		}

		// Token: 0x0200021B RID: 539
		[CompilerGenerated]
		private sealed class <>c__DisplayClass9_0
		{
			// Token: 0x06001AD4 RID: 6868 RVA: 0x00002050 File Offset: 0x00000250
			public <>c__DisplayClass9_0()
			{
			}

			// Token: 0x06001AD5 RID: 6869 RVA: 0x00129CE4 File Offset: 0x00127EE4
			internal async Task <btnRestoreFromBackup_Clicked>b__0()
			{
				string text = await BackupManager.LoadFromBackup(this.file.Path);
				this.new_car = text;
			}

			// Token: 0x04000C15 RID: 3093
			public FileSystemElement file;

			// Token: 0x04000C16 RID: 3094
			public string new_car;

			// Token: 0x0200021C RID: 540
			[StructLayout(LayoutKind.Auto)]
			private struct <<btnRestoreFromBackup_Clicked>b__0>d : IAsyncStateMachine
			{
				// Token: 0x06001AD6 RID: 6870 RVA: 0x00129D28 File Offset: 0x00127F28
				void IAsyncStateMachine.MoveNext()
				{
					int num2;
					int num = num2;
					SettingsBackup.<>c__DisplayClass9_0 CS$<>8__locals1 = this;
					try
					{
						TaskAwaiter<string> taskAwaiter;
						if (num != 0)
						{
							taskAwaiter = BackupManager.LoadFromBackup(CS$<>8__locals1.file.Path).GetAwaiter();
							if (!taskAwaiter.IsCompleted)
							{
								num2 = 0;
								TaskAwaiter<string> taskAwaiter2 = taskAwaiter;
								this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<string>, SettingsBackup.<>c__DisplayClass9_0.<<btnRestoreFromBackup_Clicked>b__0>d>(ref taskAwaiter, ref this);
								return;
							}
						}
						else
						{
							TaskAwaiter<string> taskAwaiter2;
							taskAwaiter = taskAwaiter2;
							taskAwaiter2 = default(TaskAwaiter<string>);
							num2 = -1;
						}
						string result = taskAwaiter.GetResult();
						CS$<>8__locals1.new_car = result;
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

				// Token: 0x06001AD7 RID: 6871 RVA: 0x00129DF0 File Offset: 0x00127FF0
				[DebuggerHidden]
				void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
				{
					this.<>t__builder.SetStateMachine(stateMachine);
				}

				// Token: 0x04000C17 RID: 3095
				public int <>1__state;

				// Token: 0x04000C18 RID: 3096
				public AsyncTaskMethodBuilder <>t__builder;

				// Token: 0x04000C19 RID: 3097
				public SettingsBackup.<>c__DisplayClass9_0 <>4__this;

				// Token: 0x04000C1A RID: 3098
				private TaskAwaiter<string> <>u__1;
			}
		}

		// Token: 0x0200021D RID: 541
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <DeleteFile>d__7 : IAsyncStateMachine
		{
			// Token: 0x06001AD8 RID: 6872 RVA: 0x00129E00 File Offset: 0x00128000
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				SettingsBackup settingsBackup = this;
				try
				{
					TaskAwaiter<bool> taskAwaiter3;
					if (num != 0)
					{
						taskAwaiter3 = settingsBackup.DisplayAlert(Translate.GetString("ios_DeleteBackup"), file.Name, "OK", Translate.GetString("btnCancel.Content")).GetAwaiter();
						if (!taskAwaiter3.IsCompleted)
						{
							num2 = 0;
							taskAwaiter2 = taskAwaiter3;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<bool>, SettingsBackup.<DeleteFile>d__7>(ref taskAwaiter3, ref this);
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
						file.Delete();
						settingsBackup.InitModel();
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

			// Token: 0x06001AD9 RID: 6873 RVA: 0x00129EEC File Offset: 0x001280EC
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x04000C1B RID: 3099
			public int <>1__state;

			// Token: 0x04000C1C RID: 3100
			public AsyncTaskMethodBuilder <>t__builder;

			// Token: 0x04000C1D RID: 3101
			public SettingsBackup <>4__this;

			// Token: 0x04000C1E RID: 3102
			public FileSystemElement file;

			// Token: 0x04000C1F RID: 3103
			private TaskAwaiter<bool> <>u__1;
		}

		// Token: 0x0200021E RID: 542
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <ShareFile>d__4 : IAsyncStateMachine
		{
			// Token: 0x06001ADA RID: 6874 RVA: 0x00129EFC File Offset: 0x001280FC
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				SettingsBackup settingsBackup = this;
				try
				{
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
							goto IL_0143;
						}
						case 2:
						{
							TaskAwaiter taskAwaiter4;
							taskAwaiter3 = taskAwaiter4;
							taskAwaiter4 = default(TaskAwaiter);
							num2 = -1;
							goto IL_01F2;
						}
						default:
							if (PlatformHelper.IsAndroid)
							{
								exportVariantSend = Translate.GetString("droid_ExportSend");
								exportVariantFile = Translate.GetString("droid_ExportFile");
								taskAwaiter = settingsBackup.DisplayActionSheetCustom(Translate.GetString("droid_ExportMode"), null, null, new string[] { exportVariantFile, exportVariantSend }).GetAwaiter();
								if (!taskAwaiter.IsCompleted)
								{
									num2 = 0;
									TaskAwaiter<string> taskAwaiter2 = taskAwaiter;
									this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<string>, SettingsBackup.<ShareFile>d__4>(ref taskAwaiter, ref this);
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
									Share.RequestAsync(new ShareFileRequest(new ShareFile(path)));
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
								this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, SettingsBackup.<ShareFile>d__4>(ref taskAwaiter3, ref this);
								return;
							}
						}
						else
						{
							if (!(result == exportVariantSend))
							{
								goto IL_01F9;
							}
							EmailMessage emailMessage = new EmailMessage("Car Scanner Backup", "", Array.Empty<string>());
							if (File.Exists(path))
							{
								emailMessage.Attachments.Add(new EmailAttachment(path));
							}
							taskAwaiter3 = Email.ComposeAsync(emailMessage).GetAwaiter();
							if (!taskAwaiter3.IsCompleted)
							{
								num2 = 2;
								TaskAwaiter taskAwaiter4 = taskAwaiter3;
								this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, SettingsBackup.<ShareFile>d__4>(ref taskAwaiter3, ref this);
								return;
							}
							goto IL_01F2;
						}
						IL_0143:
						taskAwaiter3.GetResult();
						goto IL_01F9;
						IL_01F2:
						taskAwaiter3.GetResult();
						IL_01F9:
						exportVariantSend = null;
						exportVariantFile = null;
						IL_0250:;
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

			// Token: 0x06001ADB RID: 6875 RVA: 0x0012A1C0 File Offset: 0x001283C0
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x04000C20 RID: 3104
			public int <>1__state;

			// Token: 0x04000C21 RID: 3105
			public AsyncVoidMethodBuilder <>t__builder;

			// Token: 0x04000C22 RID: 3106
			public SettingsBackup <>4__this;

			// Token: 0x04000C23 RID: 3107
			public string path;

			// Token: 0x04000C24 RID: 3108
			private string <exportVariantSend>5__2;

			// Token: 0x04000C25 RID: 3109
			private string <exportVariantFile>5__3;

			// Token: 0x04000C26 RID: 3110
			private TaskAwaiter<string> <>u__1;

			// Token: 0x04000C27 RID: 3111
			private TaskAwaiter <>u__2;
		}

		// Token: 0x0200021F RID: 543
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <btnCreateBackup_Clicked>d__8 : IAsyncStateMachine
		{
			// Token: 0x06001ADC RID: 6876 RVA: 0x0012A1D0 File Offset: 0x001283D0
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				SettingsBackup settingsBackup = this;
				try
				{
					TaskAwaiter taskAwaiter;
					TaskAwaiter<bool> taskAwaiter3;
					if (num != 0)
					{
						if (num == 1)
						{
							TaskAwaiter taskAwaiter2;
							taskAwaiter = taskAwaiter2;
							taskAwaiter2 = default(TaskAwaiter);
							num2 = -1;
							goto IL_0163;
						}
						CS$<>8__locals1 = new SettingsBackup.<>c__DisplayClass8_0();
						CS$<>8__locals1.<>4__this = this;
						settingsBackup.activityFrame.ResetTextToDefault();
						settingsBackup.activityFrame.IsVisible = true;
						taskAwaiter3 = settingsBackup.DisplayAlert(Translate.GetString("ios_IncludeDataRecordsTitle"), Translate.GetString("ios_IncludeDataRecordsText"), Translate.GetString("ios_Yes"), Translate.GetString("ios_No")).GetAwaiter();
						if (!taskAwaiter3.IsCompleted)
						{
							num2 = 0;
							TaskAwaiter<bool> taskAwaiter4 = taskAwaiter3;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<bool>, SettingsBackup.<btnCreateBackup_Clicked>d__8>(ref taskAwaiter3, ref this);
							return;
						}
					}
					else
					{
						TaskAwaiter<bool> taskAwaiter4;
						taskAwaiter3 = taskAwaiter4;
						taskAwaiter4 = default(TaskAwaiter<bool>);
						num2 = -1;
					}
					bool result = taskAwaiter3.GetResult();
					CS$<>8__locals1.include_records = result;
					CS$<>8__locals1.progress_handler = new Progress<int>(delegate(int p)
					{
						Device.BeginInvokeOnMainThread(new Action(new SettingsBackup.<>c__DisplayClass8_1
						{
							CS$<>8__locals1 = CS$<>8__locals1,
							p = p
						}.<btnCreateBackup_Clicked>b__2));
					});
					taskAwaiter = Task.Run(delegate
					{
						SettingsBackup.<>c__DisplayClass8_0.<<btnCreateBackup_Clicked>b__1>d <<btnCreateBackup_Clicked>b__1>d;
						<<btnCreateBackup_Clicked>b__1>d.<>t__builder = AsyncTaskMethodBuilder.Create();
						<<btnCreateBackup_Clicked>b__1>d.<>4__this = CS$<>8__locals1;
						<<btnCreateBackup_Clicked>b__1>d.<>1__state = -1;
						<<btnCreateBackup_Clicked>b__1>d.<>t__builder.Start<SettingsBackup.<>c__DisplayClass8_0.<<btnCreateBackup_Clicked>b__1>d>(ref <<btnCreateBackup_Clicked>b__1>d);
						return <<btnCreateBackup_Clicked>b__1>d.<>t__builder.Task;
					}).GetAwaiter();
					if (!taskAwaiter.IsCompleted)
					{
						num2 = 1;
						TaskAwaiter taskAwaiter2 = taskAwaiter;
						this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, SettingsBackup.<btnCreateBackup_Clicked>d__8>(ref taskAwaiter, ref this);
						return;
					}
					IL_0163:
					taskAwaiter.GetResult();
					settingsBackup.InitModel();
					settingsBackup.activityFrame.IsVisible = false;
				}
				catch (Exception ex)
				{
					num2 = -2;
					CS$<>8__locals1 = null;
					this.<>t__builder.SetException(ex);
					return;
				}
				num2 = -2;
				CS$<>8__locals1 = null;
				this.<>t__builder.SetResult();
			}

			// Token: 0x06001ADD RID: 6877 RVA: 0x0012A3B4 File Offset: 0x001285B4
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x04000C28 RID: 3112
			public int <>1__state;

			// Token: 0x04000C29 RID: 3113
			public AsyncVoidMethodBuilder <>t__builder;

			// Token: 0x04000C2A RID: 3114
			public SettingsBackup <>4__this;

			// Token: 0x04000C2B RID: 3115
			private SettingsBackup.<>c__DisplayClass8_0 <>8__1;

			// Token: 0x04000C2C RID: 3116
			private TaskAwaiter<bool> <>u__1;

			// Token: 0x04000C2D RID: 3117
			private TaskAwaiter <>u__2;
		}

		// Token: 0x02000220 RID: 544
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <btnDeleteBackup_Clicked>d__16 : IAsyncStateMachine
		{
			// Token: 0x06001ADE RID: 6878 RVA: 0x0012A3C4 File Offset: 0x001285C4
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				SettingsBackup settingsBackup = this;
				try
				{
					TaskAwaiter taskAwaiter;
					if (num != 0)
					{
						if (settingsBackup.lv.SelectedItem == null)
						{
							goto IL_00A8;
						}
						FileSystemElement fileSystemElement = settingsBackup.lv.SelectedItem as FileSystemElement;
						taskAwaiter = settingsBackup.DeleteFile(fileSystemElement).GetAwaiter();
						if (!taskAwaiter.IsCompleted)
						{
							num2 = 0;
							TaskAwaiter taskAwaiter2 = taskAwaiter;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, SettingsBackup.<btnDeleteBackup_Clicked>d__16>(ref taskAwaiter, ref this);
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
				IL_00A8:
				num2 = -2;
				this.<>t__builder.SetResult();
			}

			// Token: 0x06001ADF RID: 6879 RVA: 0x0012A49C File Offset: 0x0012869C
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x04000C2E RID: 3118
			public int <>1__state;

			// Token: 0x04000C2F RID: 3119
			public AsyncVoidMethodBuilder <>t__builder;

			// Token: 0x04000C30 RID: 3120
			public SettingsBackup <>4__this;

			// Token: 0x04000C31 RID: 3121
			private TaskAwaiter <>u__1;
		}

		// Token: 0x02000221 RID: 545
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <btnDelete_Clicked>d__5 : IAsyncStateMachine
		{
			// Token: 0x06001AE0 RID: 6880 RVA: 0x0012A4AC File Offset: 0x001286AC
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				SettingsBackup settingsBackup = this;
				try
				{
					TaskAwaiter taskAwaiter;
					if (num != 0)
					{
						FileSystemElement fileSystemElement = (sender as MenuItem).BindingContext as FileSystemElement;
						taskAwaiter = settingsBackup.DeleteFile(fileSystemElement).GetAwaiter();
						if (!taskAwaiter.IsCompleted)
						{
							num2 = 0;
							TaskAwaiter taskAwaiter2 = taskAwaiter;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, SettingsBackup.<btnDelete_Clicked>d__5>(ref taskAwaiter, ref this);
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

			// Token: 0x06001AE1 RID: 6881 RVA: 0x0012A578 File Offset: 0x00128778
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x04000C32 RID: 3122
			public int <>1__state;

			// Token: 0x04000C33 RID: 3123
			public AsyncVoidMethodBuilder <>t__builder;

			// Token: 0x04000C34 RID: 3124
			public object sender;

			// Token: 0x04000C35 RID: 3125
			public SettingsBackup <>4__this;

			// Token: 0x04000C36 RID: 3126
			private TaskAwaiter <>u__1;
		}

		// Token: 0x02000222 RID: 546
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <btnImportDroid_Clicked>d__17 : IAsyncStateMachine
		{
			// Token: 0x06001AE2 RID: 6882 RVA: 0x0012A588 File Offset: 0x00128788
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				SettingsBackup settingsBackup = this;
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
								this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<FileResult>, SettingsBackup.<btnImportDroid_Clicked>d__17>(ref taskAwaiter, ref this);
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
						if (result != null && result.FileName.EndsWith("cbz", StringComparison.OrdinalIgnoreCase))
						{
							string localFilePath = FileSystemHelper.GetLocalFilePath(result.FileName);
							File.Copy(result.FullPath, localFilePath);
							List<FileSystemElement> files = BackupManager.GetFiles();
							settingsBackup.lv.ItemsSource = files;
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

			// Token: 0x06001AE3 RID: 6883 RVA: 0x0012A698 File Offset: 0x00128898
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x04000C37 RID: 3127
			public int <>1__state;

			// Token: 0x04000C38 RID: 3128
			public AsyncVoidMethodBuilder <>t__builder;

			// Token: 0x04000C39 RID: 3129
			public SettingsBackup <>4__this;

			// Token: 0x04000C3A RID: 3130
			private TaskAwaiter<FileResult> <>u__1;
		}

		// Token: 0x02000223 RID: 547
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <btnRestoreFromBackup_Clicked>d__9 : IAsyncStateMachine
		{
			// Token: 0x06001AE4 RID: 6884 RVA: 0x0012A6A8 File Offset: 0x001288A8
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				SettingsBackup settingsBackup = this;
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
						goto IL_0177;
					}
					case 2:
					{
						TaskAwaiter taskAwaiter5;
						taskAwaiter4 = taskAwaiter5;
						taskAwaiter5 = default(TaskAwaiter);
						num2 = -1;
						goto IL_0207;
					}
					default:
					{
						CS$<>8__locals1 = new SettingsBackup.<>c__DisplayClass9_0();
						if (settingsBackup.lv.SelectedItem == null)
						{
							goto IL_0230;
						}
						CS$<>8__locals1.file = settingsBackup.lv.SelectedItem as FileSystemElement;
						string text = Translate.GetString("ios_RestoreBackupConfirmationTitle");
						text = string.Format(text, CS$<>8__locals1.file.Name);
						taskAwaiter3 = settingsBackup.DisplayAlert(text, Translate.GetString("ios_RestoreBackupConfirmationText"), "OK", Translate.GetString("btnCancel.Content")).GetAwaiter();
						if (!taskAwaiter3.IsCompleted)
						{
							num2 = 0;
							taskAwaiter2 = taskAwaiter3;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<bool>, SettingsBackup.<btnRestoreFromBackup_Clicked>d__9>(ref taskAwaiter3, ref this);
							return;
						}
						break;
					}
					}
					if (!taskAwaiter3.GetResult())
					{
						goto IL_020E;
					}
					settingsBackup.activityFrame.IsVisible = true;
					CS$<>8__locals1.new_car = "";
					taskAwaiter4 = Task.Run(delegate
					{
						SettingsBackup.<>c__DisplayClass9_0.<<btnRestoreFromBackup_Clicked>b__0>d <<btnRestoreFromBackup_Clicked>b__0>d;
						<<btnRestoreFromBackup_Clicked>b__0>d.<>t__builder = AsyncTaskMethodBuilder.Create();
						<<btnRestoreFromBackup_Clicked>b__0>d.<>4__this = CS$<>8__locals1;
						<<btnRestoreFromBackup_Clicked>b__0>d.<>1__state = -1;
						<<btnRestoreFromBackup_Clicked>b__0>d.<>t__builder.Start<SettingsBackup.<>c__DisplayClass9_0.<<btnRestoreFromBackup_Clicked>b__0>d>(ref <<btnRestoreFromBackup_Clicked>b__0>d);
						return <<btnRestoreFromBackup_Clicked>b__0>d.<>t__builder.Task;
					}).GetAwaiter();
					if (!taskAwaiter4.IsCompleted)
					{
						num2 = 1;
						TaskAwaiter taskAwaiter5 = taskAwaiter4;
						this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, SettingsBackup.<btnRestoreFromBackup_Clicked>d__9>(ref taskAwaiter4, ref this);
						return;
					}
					IL_0177:
					taskAwaiter4.GetResult();
					settingsBackup.activityFrame.IsVisible = false;
					taskAwaiter4 = settingsBackup.DisplayAlert(Translate.GetString("ios_RestoreFinishedTitle"), string.Format(Translate.GetString("ios_RestoreFinishedText"), CS$<>8__locals1.new_car), "OK").GetAwaiter();
					if (!taskAwaiter4.IsCompleted)
					{
						num2 = 2;
						TaskAwaiter taskAwaiter5 = taskAwaiter4;
						this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, SettingsBackup.<btnRestoreFromBackup_Clicked>d__9>(ref taskAwaiter4, ref this);
						return;
					}
					IL_0207:
					taskAwaiter4.GetResult();
					IL_020E:;
				}
				catch (Exception ex)
				{
					num2 = -2;
					CS$<>8__locals1 = null;
					this.<>t__builder.SetException(ex);
					return;
				}
				IL_0230:
				num2 = -2;
				CS$<>8__locals1 = null;
				this.<>t__builder.SetResult();
			}

			// Token: 0x06001AE5 RID: 6885 RVA: 0x0012A91C File Offset: 0x00128B1C
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x04000C3B RID: 3131
			public int <>1__state;

			// Token: 0x04000C3C RID: 3132
			public AsyncVoidMethodBuilder <>t__builder;

			// Token: 0x04000C3D RID: 3133
			public SettingsBackup <>4__this;

			// Token: 0x04000C3E RID: 3134
			private SettingsBackup.<>c__DisplayClass9_0 <>8__1;

			// Token: 0x04000C3F RID: 3135
			private TaskAwaiter<bool> <>u__1;

			// Token: 0x04000C40 RID: 3136
			private TaskAwaiter <>u__2;
		}

		// Token: 0x02000224 RID: 548
		[CompilerGenerated]
		private sealed class <InitializeComponent>_anonXamlCDataTemplate_89
		{
			// Token: 0x06001AE6 RID: 6886 RVA: 0x0012A92C File Offset: 0x00128B2C
			public <InitializeComponent>_anonXamlCDataTemplate_89()
			{
			}

			// Token: 0x06001AE7 RID: 6887 RVA: 0x0012A940 File Offset: 0x00128B40
			internal object LoadDataTemplate()
			{
				BindingExtension bindingExtension;
				VisualDiagnostics.RegisterSourceInfo(bindingExtension = new BindingExtension(), new Uri("Settings\\SettingsBackup.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 39, 37);
				Translate translate;
				VisualDiagnostics.RegisterSourceInfo(translate = new Translate(), new Uri("Settings\\SettingsBackup.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 41, 37);
				MenuItem menuItem;
				VisualDiagnostics.RegisterSourceInfo(menuItem = new MenuItem(), new Uri("Settings\\SettingsBackup.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 37, 34);
				BindingExtension bindingExtension2;
				VisualDiagnostics.RegisterSourceInfo(bindingExtension2 = new BindingExtension(), new Uri("Settings\\SettingsBackup.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 44, 37);
				Translate translate2;
				VisualDiagnostics.RegisterSourceInfo(translate2 = new Translate(), new Uri("Settings\\SettingsBackup.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 46, 37);
				MenuItem menuItem2;
				VisualDiagnostics.RegisterSourceInfo(menuItem2 = new MenuItem(), new Uri("Settings\\SettingsBackup.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 42, 34);
				BindingExtension bindingExtension3;
				VisualDiagnostics.RegisterSourceInfo(bindingExtension3 = new BindingExtension(), new Uri("Settings\\SettingsBackup.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 49, 37);
				Translate translate3;
				VisualDiagnostics.RegisterSourceInfo(translate3 = new Translate(), new Uri("Settings\\SettingsBackup.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 51, 37);
				MenuItem menuItem3;
				VisualDiagnostics.RegisterSourceInfo(menuItem3 = new MenuItem(), new Uri("Settings\\SettingsBackup.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 47, 34);
				StaticResourceExtension staticResourceExtension;
				VisualDiagnostics.RegisterSourceInfo(staticResourceExtension = new StaticResourceExtension(), new Uri("Settings\\SettingsBackup.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 31, 33);
				BindingExtension bindingExtension4;
				VisualDiagnostics.RegisterSourceInfo(bindingExtension4 = new BindingExtension(), new Uri("Settings\\SettingsBackup.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 32, 33);
				DynamicResourceExtension dynamicResourceExtension;
				VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension = new DynamicResourceExtension(), new Uri("Settings\\SettingsBackup.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 33, 33);
				Label label;
				VisualDiagnostics.RegisterSourceInfo(label = new Label(), new Uri("Settings\\SettingsBackup.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 30, 30);
				ViewCell viewCell;
				VisualDiagnostics.RegisterSourceInfo(viewCell = new ViewCell(), new Uri("Settings\\SettingsBackup.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 29, 26);
				NameScope nameScope = new NameScope();
				NameScope.SetNameScope(viewCell, nameScope);
				menuItem.Clicked += this.root.mnuRename_Clicked;
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
				xmlNamespaceResolver.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
				xmlNamespaceResolver.Add("local", "clr-namespace:CarScannerXamarinForms");
				xmlNamespaceResolver.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
				xamlServiceProvider.Add(typeFromHandle2, new XamlTypeResolver(xmlNamespaceResolver, typeof(SettingsBackup.<InitializeComponent>_anonXamlCDataTemplate_89).GetTypeInfo().Assembly));
				xamlServiceProvider.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(41, 37)));
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
				xmlNamespaceResolver2.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
				xmlNamespaceResolver2.Add("local", "clr-namespace:CarScannerXamarinForms");
				xmlNamespaceResolver2.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
				xamlServiceProvider2.Add(typeFromHandle4, new XamlTypeResolver(xmlNamespaceResolver2, typeof(SettingsBackup.<InitializeComponent>_anonXamlCDataTemplate_89).GetTypeInfo().Assembly));
				xamlServiceProvider2.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(46, 37)));
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
				xmlNamespaceResolver3.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
				xmlNamespaceResolver3.Add("local", "clr-namespace:CarScannerXamarinForms");
				xmlNamespaceResolver3.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
				xamlServiceProvider3.Add(typeFromHandle6, new XamlTypeResolver(xmlNamespaceResolver3, typeof(SettingsBackup.<InitializeComponent>_anonXamlCDataTemplate_89).GetTypeInfo().Assembly));
				xamlServiceProvider3.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(51, 37)));
				object obj6 = markupExtension3.ProvideValue(xamlServiceProvider3);
				menuItem3.Text = obj6;
				viewCell.ContextActions.Add(menuItem3);
				staticResourceExtension.Key = "BaseFontSize++";
				IMarkupExtension markupExtension4 = staticResourceExtension;
				XamlServiceProvider xamlServiceProvider4 = new XamlServiceProvider();
				Type typeFromHandle7 = typeof(IProvideValueTarget);
				int num4;
				object[] array7 = new object[(num4 = this.parentValues.Length) + 2];
				Array.Copy(this.parentValues, 0, array7, 2, num4);
				object[] array8 = array7;
				array8[0] = label;
				array8[1] = viewCell;
				object obj7;
				xamlServiceProvider4.Add(typeFromHandle7, obj7 = new SimpleValueTargetProvider(array8, Label.FontSizeProperty, nameScope));
				xamlServiceProvider4.Add(typeof(IReferenceProvider), obj7);
				Type typeFromHandle8 = typeof(IXamlTypeResolver);
				XmlNamespaceResolver xmlNamespaceResolver4 = new XmlNamespaceResolver();
				xmlNamespaceResolver4.Add("", "http://xamarin.com/schemas/2014/forms");
				xmlNamespaceResolver4.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
				xmlNamespaceResolver4.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
				xmlNamespaceResolver4.Add("local", "clr-namespace:CarScannerXamarinForms");
				xmlNamespaceResolver4.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
				xamlServiceProvider4.Add(typeFromHandle8, new XamlTypeResolver(xmlNamespaceResolver4, typeof(SettingsBackup.<InitializeComponent>_anonXamlCDataTemplate_89).GetTypeInfo().Assembly));
				xamlServiceProvider4.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(31, 33)));
				object obj8 = markupExtension4.ProvideValue(xamlServiceProvider4);
				label.FontSize = (double)obj8;
				bindingExtension4.Path = "Name";
				BindingBase bindingBase4 = bindingExtension4.ProvideValue(null);
				label.SetBinding(Label.TextProperty, bindingBase4);
				dynamicResourceExtension.Key = "SettingsCellTitleColor";
				IMarkupExtension<DynamicResource> markupExtension5 = dynamicResourceExtension;
				XamlServiceProvider xamlServiceProvider5 = new XamlServiceProvider();
				Type typeFromHandle9 = typeof(IProvideValueTarget);
				int num5;
				object[] array9 = new object[(num5 = this.parentValues.Length) + 2];
				Array.Copy(this.parentValues, 0, array9, 2, num5);
				object[] array10 = array9;
				array10[0] = label;
				array10[1] = viewCell;
				object obj9;
				xamlServiceProvider5.Add(typeFromHandle9, obj9 = new SimpleValueTargetProvider(array10, Label.TextColorProperty, nameScope));
				xamlServiceProvider5.Add(typeof(IReferenceProvider), obj9);
				Type typeFromHandle10 = typeof(IXamlTypeResolver);
				XmlNamespaceResolver xmlNamespaceResolver5 = new XmlNamespaceResolver();
				xmlNamespaceResolver5.Add("", "http://xamarin.com/schemas/2014/forms");
				xmlNamespaceResolver5.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
				xmlNamespaceResolver5.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
				xmlNamespaceResolver5.Add("local", "clr-namespace:CarScannerXamarinForms");
				xmlNamespaceResolver5.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
				xamlServiceProvider5.Add(typeFromHandle10, new XamlTypeResolver(xmlNamespaceResolver5, typeof(SettingsBackup.<InitializeComponent>_anonXamlCDataTemplate_89).GetTypeInfo().Assembly));
				xamlServiceProvider5.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(33, 33)));
				DynamicResource dynamicResource = markupExtension5.ProvideValue(xamlServiceProvider5);
				label.SetDynamicResource(Label.TextColorProperty, dynamicResource.Key);
				label.SetValue(View.VerticalOptionsProperty, LayoutOptions.Center);
				label.SetValue(Label.VerticalTextAlignmentProperty, new TextAlignmentConverter().ConvertFromInvariantString("Center"));
				viewCell.View = label;
				return viewCell;
			}

			// Token: 0x04000C41 RID: 3137
			internal object[] parentValues;

			// Token: 0x04000C42 RID: 3138
			internal SettingsBackup root;
		}
	}
}
