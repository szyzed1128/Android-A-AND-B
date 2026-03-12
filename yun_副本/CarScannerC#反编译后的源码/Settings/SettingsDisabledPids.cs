using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using CarScannerXamarinForms.Common;
using CarScannerXamarinForms.OBD2;
using CarScannerXamarinForms.OBD2.PIDS;
using CarScannerXamarinForms.ProfilesV2;
using CarScannerXamarinForms.ViewModels;
using Syncfusion.XForms.Buttons;
using Xamarin.Forms;
using Xamarin.Forms.Internals;
using Xamarin.Forms.PlatformConfiguration.iOSSpecific;
using Xamarin.Forms.Xaml;
using Xamarin.Forms.Xaml.Diagnostics;
using Xamarin.Forms.Xaml.Internals;

namespace CarScannerXamarinForms.Settings
{
	// Token: 0x0200022B RID: 555
	[XamlCompilation(2)]
	[XamlFilePath("Settings\\SettingsDisabledPids.xaml")]
	public class SettingsDisabledPids : ContentPage
	{
		// Token: 0x06001AFF RID: 6911 RVA: 0x0012C660 File Offset: 0x0012A860
		public SettingsDisabledPids()
		{
			this.InitializeComponent();
			if (App.OBDReader.CurrentStatus == OBDDataReaderStatus.ConnectedToECU && !App.OBDSimulator.IsActive)
			{
				this.btnRemoveUnsupported.IsVisible = true;
			}
			else
			{
				this.btnRemoveUnsupported.IsVisible = false;
			}
			Task.Run(delegate
			{
				CustomPIDViewModel.DisabledProfile.Load();
				Device.BeginInvokeOnMainThread(delegate
				{
					this.lvEnabled.ItemsSource = CustomPIDViewModel.CurrentProfile.PidCollection;
					this.lvDisabled.ItemsSource = CustomPIDViewModel.DisabledProfile.PidCollection;
				});
			});
		}

		// Token: 0x06001B00 RID: 6912 RVA: 0x0012C6CC File Offset: 0x0012A8CC
		private void rbActive_StateChanged(object sender, StateChangedEventArgs e)
		{
			if (e.IsChecked.GetValueOrDefault())
			{
				this.rbDisabledPids.IsChecked = new bool?(false);
				this.lvEnabled.IsVisible = true;
				this.lvDisabled.IsVisible = false;
			}
		}

		// Token: 0x06001B01 RID: 6913 RVA: 0x0012C714 File Offset: 0x0012A914
		private void rbDisabled_StateChanged(object sender, StateChangedEventArgs e)
		{
			if (e.IsChecked.GetValueOrDefault())
			{
				this.rbActivePids.IsChecked = new bool?(false);
				this.lvEnabled.IsVisible = false;
				this.lvDisabled.IsVisible = true;
			}
		}

		// Token: 0x06001B02 RID: 6914 RVA: 0x0012C75C File Offset: 0x0012A95C
		private void btnDisable_Clicked(object sender, EventArgs e)
		{
			CustomPID customPID = (CustomPID)((Button)sender).BindingContext;
			if (customPID != null)
			{
				object obj = this.lock_object;
				lock (obj)
				{
					CustomPIDViewModel.CurrentProfile.PidCollection.Remove(customPID);
					CustomPIDViewModel.DisabledProfile.PidCollection.Add(customPID);
					CustomPIDViewModel.CurrentProfile.Save();
					CustomPIDViewModel.DisabledProfile.Save();
					if (App.OBDReader.CurrentStatus == OBDDataReaderStatus.ConnectedToECU)
					{
						LiveDataPIDModel._PIDCollection.Remove(customPID);
					}
				}
			}
		}

		// Token: 0x06001B03 RID: 6915 RVA: 0x0012C7F8 File Offset: 0x0012A9F8
		private void btnEnable_Clicked(object sender, EventArgs e)
		{
			CustomPID customPID = (CustomPID)((Button)sender).BindingContext;
			if (customPID != null)
			{
				object obj = this.lock_object;
				lock (obj)
				{
					CustomPIDViewModel.DisabledProfile.PidCollection.Remove(customPID);
					CustomPIDViewModel.CurrentProfile.PidCollection.Add(customPID);
					CustomPIDViewModel.CurrentProfile.Save();
					CustomPIDViewModel.DisabledProfile.Save();
					if (App.OBDReader.CurrentStatus == OBDDataReaderStatus.ConnectedToECU)
					{
						LiveDataPIDModel._PIDCollection.Add(customPID);
					}
				}
			}
		}

		// Token: 0x06001B04 RID: 6916 RVA: 0x0012C894 File Offset: 0x0012AA94
		private async void btnRemoveUnsupported_Clicked(object sender, EventArgs e)
		{
			SettingsDisabledPids.<>c__DisplayClass5_0 CS$<>8__locals1 = new SettingsDisabledPids.<>c__DisplayClass5_0();
			CS$<>8__locals1.<>4__this = this;
			if (App.OBDReader.CurrentStatus == OBDDataReaderStatus.ConnectedToECU && !App.OBDSimulator.IsActive)
			{
				this.activityFrame.CancelText = Translate.GetString("btnCancel.Content");
				this.activityFrame.IsCancelVisible = true;
				this.btnRemoveUnsupported.IsEnabled = false;
				this.activityFrame.IsVisible = true;
				CS$<>8__locals1.detectingString = Translate.GetString("profile_CheckingSupportedPids");
				Progress<int> progress = new Progress<int>(delegate(int i)
				{
					MainThreadHelper.InvokeOnMainThread(delegate
					{
						CS$<>8__locals1.<>4__this.activityFrame.Text = string.Concat(new string[]
						{
							CS$<>8__locals1.detectingString,
							"\n",
							SharedSettings.Current.SelectedBrand,
							" ",
							SharedSettings.Current.SelectedProfileV2Name,
							"\n",
							i.ToString(),
							"%"
						});
					});
				});
				CS$<>8__locals1.cts = new CancellationTokenSource();
				EventHandler eventHandler = delegate(object cancelSender, EventArgs cancelArgs)
				{
					SettingsDisabledPids.<>c__DisplayClass5_0.<<btnRemoveUnsupported_Clicked>b__1>d <<btnRemoveUnsupported_Clicked>b__1>d;
					<<btnRemoveUnsupported_Clicked>b__1>d.<>t__builder = AsyncVoidMethodBuilder.Create();
					<<btnRemoveUnsupported_Clicked>b__1>d.<>4__this = CS$<>8__locals1;
					<<btnRemoveUnsupported_Clicked>b__1>d.<>1__state = -1;
					<<btnRemoveUnsupported_Clicked>b__1>d.<>t__builder.Start<SettingsDisabledPids.<>c__DisplayClass5_0.<<btnRemoveUnsupported_Clicked>b__1>d>(ref <<btnRemoveUnsupported_Clicked>b__1>d);
				};
				ActivityFrame activityFrame = this.activityFrame;
				activityFrame.CancelClicked = (EventHandler)Delegate.Remove(activityFrame.CancelClicked, eventHandler);
				ActivityFrame activityFrame2 = this.activityFrame;
				activityFrame2.CancelClicked = (EventHandler)Delegate.Combine(activityFrame2.CancelClicked, eventHandler);
				foreach (CustomPID customPID in CustomPIDViewModel.DisabledProfile.PidCollection)
				{
					CustomPIDViewModel.CurrentProfile.PidCollection.Add(customPID);
				}
				CustomPIDViewModel.DisabledProfile.PidCollection.Clear();
				CustomPIDViewModel.DisabledProfile.Save();
				CustomPIDViewModel.CurrentProfile.Save();
				await ProfileSupportedPidsTester.PerformTest(progress, CS$<>8__locals1.cts.Token);
				this.activityFrame.IsVisible = false;
				this.btnRemoveUnsupported.IsEnabled = true;
			}
		}

		// Token: 0x06001B05 RID: 6917 RVA: 0x0012C8CC File Offset: 0x0012AACC
		private async void btnResetRoles_Clicked(object sender, EventArgs e)
		{
			TaskAwaiter<bool> taskAwaiter = base.DisplayAlert(Translate.GetString("settings_ResetRolesToDefault") + "?", "", "OK", Translate.GetString("btnCancel.Content")).GetAwaiter();
			if (!taskAwaiter.IsCompleted)
			{
				await taskAwaiter;
				TaskAwaiter<bool> taskAwaiter2;
				taskAwaiter = taskAwaiter2;
				taskAwaiter2 = default(TaskAwaiter<bool>);
			}
			if (taskAwaiter.GetResult())
			{
				PIDOverrideDictionary.Instance.Clear();
				PIDOverrideDictionary.Instance.Save();
				App.OBDReader.CurrentCarData.InitializeCalculatedPIDs();
			}
		}

		// Token: 0x06001B06 RID: 6918 RVA: 0x0012C904 File Offset: 0x0012AB04
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private void InitializeComponent()
		{
			if (ResourceLoader.IsEnabled && ResourceLoader.CanProvideContentFor(new ResourceLoader.ResourceLoadingQuery
			{
				AssemblyName = typeof(SettingsDisabledPids).GetTypeInfo().Assembly.GetName(),
				ResourcePath = "Settings/SettingsDisabledPids.xaml",
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
			VisualDiagnostics.RegisterSourceInfo(translate = new Translate(), new Uri("Settings\\SettingsDisabledPids.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 12, 5);
			DynamicResourceExtension dynamicResourceExtension;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension = new DynamicResourceExtension(), new Uri("Settings\\SettingsDisabledPids.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 14, 5);
			RowDefinition rowDefinition;
			VisualDiagnostics.RegisterSourceInfo(rowDefinition = new RowDefinition(), new Uri("Settings\\SettingsDisabledPids.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 19, 18);
			RowDefinition rowDefinition2;
			VisualDiagnostics.RegisterSourceInfo(rowDefinition2 = new RowDefinition(), new Uri("Settings\\SettingsDisabledPids.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 20, 18);
			RowDefinition rowDefinition3;
			VisualDiagnostics.RegisterSourceInfo(rowDefinition3 = new RowDefinition(), new Uri("Settings\\SettingsDisabledPids.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 21, 18);
			RowDefinition rowDefinition4;
			VisualDiagnostics.RegisterSourceInfo(rowDefinition4 = new RowDefinition(), new Uri("Settings\\SettingsDisabledPids.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 22, 18);
			ColumnDefinition columnDefinition;
			VisualDiagnostics.RegisterSourceInfo(columnDefinition = new ColumnDefinition(), new Uri("Settings\\SettingsDisabledPids.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 26, 22);
			ColumnDefinition columnDefinition2;
			VisualDiagnostics.RegisterSourceInfo(columnDefinition2 = new ColumnDefinition(), new Uri("Settings\\SettingsDisabledPids.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 27, 22);
			Translate translate2;
			VisualDiagnostics.RegisterSourceInfo(translate2 = new Translate(), new Uri("Settings\\SettingsDisabledPids.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 36, 21);
			SfRadioButton sfRadioButton;
			VisualDiagnostics.RegisterSourceInfo(sfRadioButton = new SfRadioButton(), new Uri("Settings\\SettingsDisabledPids.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 30, 18);
			Translate translate3;
			VisualDiagnostics.RegisterSourceInfo(translate3 = new Translate(), new Uri("Settings\\SettingsDisabledPids.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 43, 21);
			SfRadioButton sfRadioButton2;
			VisualDiagnostics.RegisterSourceInfo(sfRadioButton2 = new SfRadioButton(), new Uri("Settings\\SettingsDisabledPids.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 37, 18);
			Grid grid;
			VisualDiagnostics.RegisterSourceInfo(grid = new Grid(), new Uri("Settings\\SettingsDisabledPids.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 24, 14);
			DataTemplate dataTemplate;
			VisualDiagnostics.RegisterSourceInfo(dataTemplate = new DataTemplate(), new Uri("Settings\\SettingsDisabledPids.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 51, 22);
			ListView listView;
			VisualDiagnostics.RegisterSourceInfo(listView = new ListView(), new Uri("Settings\\SettingsDisabledPids.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 45, 14);
			DataTemplate dataTemplate2;
			VisualDiagnostics.RegisterSourceInfo(dataTemplate2 = new DataTemplate(), new Uri("Settings\\SettingsDisabledPids.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 74, 22);
			ListView listView2;
			VisualDiagnostics.RegisterSourceInfo(listView2 = new ListView(), new Uri("Settings\\SettingsDisabledPids.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 68, 14);
			ActivityFrame activityFrame;
			VisualDiagnostics.RegisterSourceInfo(activityFrame = new ActivityFrame(), new Uri("Settings\\SettingsDisabledPids.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 91, 14);
			Translate translate4;
			VisualDiagnostics.RegisterSourceInfo(translate4 = new Translate(), new Uri("Settings\\SettingsDisabledPids.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 99, 17);
			Button button;
			VisualDiagnostics.RegisterSourceInfo(button = new Button(), new Uri("Settings\\SettingsDisabledPids.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 95, 14);
			Translate translate5;
			VisualDiagnostics.RegisterSourceInfo(translate5 = new Translate(), new Uri("Settings\\SettingsDisabledPids.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 104, 17);
			Button button2;
			VisualDiagnostics.RegisterSourceInfo(button2 = new Button(), new Uri("Settings\\SettingsDisabledPids.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 100, 14);
			Grid grid2;
			VisualDiagnostics.RegisterSourceInfo(grid2 = new Grid(), new Uri("Settings\\SettingsDisabledPids.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 17, 10);
			VisualDiagnostics.RegisterSourceInfo(this, new Uri("Settings\\SettingsDisabledPids.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 2, 2);
			NameScope nameScope = NameScope.GetNameScope(this) ?? new NameScope();
			NameScope.SetNameScope(this, nameScope);
			nameScope.RegisterName("rbActivePids", sfRadioButton);
			if (sfRadioButton.StyleId == null)
			{
				sfRadioButton.StyleId = "rbActivePids";
			}
			nameScope.RegisterName("rbDisabledPids", sfRadioButton2);
			if (sfRadioButton2.StyleId == null)
			{
				sfRadioButton2.StyleId = "rbDisabledPids";
			}
			nameScope.RegisterName("lvEnabled", listView);
			if (listView.StyleId == null)
			{
				listView.StyleId = "lvEnabled";
			}
			nameScope.RegisterName("lvDisabled", listView2);
			if (listView2.StyleId == null)
			{
				listView2.StyleId = "lvDisabled";
			}
			nameScope.RegisterName("activityFrame", activityFrame);
			if (activityFrame.StyleId == null)
			{
				activityFrame.StyleId = "activityFrame";
			}
			nameScope.RegisterName("btnRemoveUnsupported", button);
			if (button.StyleId == null)
			{
				button.StyleId = "btnRemoveUnsupported";
			}
			nameScope.RegisterName("btnResetRoles", button2);
			if (button2.StyleId == null)
			{
				button2.StyleId = "btnResetRoles";
			}
			this.rbActivePids = sfRadioButton;
			this.rbDisabledPids = sfRadioButton2;
			this.lvEnabled = listView;
			this.lvDisabled = listView2;
			this.activityFrame = activityFrame;
			this.btnRemoveUnsupported = button;
			this.btnResetRoles = button2;
			translate.Text = "ios_ManageProfilePids";
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
			xmlNamespaceResolver.Add("d", "http://xamarin.com/schemas/2014/forms/design");
			xmlNamespaceResolver.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver.Add("mc", "http://schemas.openxmlformats.org/markup-compatibility/2006");
			xmlNamespaceResolver.Add("settings", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver.Add("syncfusion", "clr-namespace:Syncfusion.XForms.Buttons;assembly=Syncfusion.Buttons.XForms");
			xamlServiceProvider.Add(typeFromHandle2, new XamlTypeResolver(xmlNamespaceResolver, typeof(SettingsDisabledPids).GetTypeInfo().Assembly));
			xamlServiceProvider.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(12, 5)));
			object obj2 = markupExtension.ProvideValue(xamlServiceProvider);
			this.Title = obj2;
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
			xmlNamespaceResolver2.Add("d", "http://xamarin.com/schemas/2014/forms/design");
			xmlNamespaceResolver2.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver2.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver2.Add("mc", "http://schemas.openxmlformats.org/markup-compatibility/2006");
			xmlNamespaceResolver2.Add("settings", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver2.Add("syncfusion", "clr-namespace:Syncfusion.XForms.Buttons;assembly=Syncfusion.Buttons.XForms");
			xamlServiceProvider2.Add(typeFromHandle4, new XamlTypeResolver(xmlNamespaceResolver2, typeof(SettingsDisabledPids).GetTypeInfo().Assembly));
			xamlServiceProvider2.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(14, 5)));
			DynamicResource dynamicResource = markupExtension2.ProvideValue(xamlServiceProvider2);
			this.SetDynamicResource(VisualElement.BackgroundColorProperty, dynamicResource.Key);
			grid2.SetValue(View.MarginProperty, new Thickness(5.0, 5.0, 5.0, 0.0));
			rowDefinition.SetValue(RowDefinition.HeightProperty, new GridLengthTypeConverter().ConvertFromInvariantString("auto"));
			grid2.GetValue(Grid.RowDefinitionsProperty).Add(rowDefinition);
			rowDefinition2.SetValue(RowDefinition.HeightProperty, new GridLengthTypeConverter().ConvertFromInvariantString("*"));
			grid2.GetValue(Grid.RowDefinitionsProperty).Add(rowDefinition2);
			rowDefinition3.SetValue(RowDefinition.HeightProperty, new GridLengthTypeConverter().ConvertFromInvariantString("auto"));
			grid2.GetValue(Grid.RowDefinitionsProperty).Add(rowDefinition3);
			rowDefinition4.SetValue(RowDefinition.HeightProperty, new GridLengthTypeConverter().ConvertFromInvariantString("auto"));
			grid2.GetValue(Grid.RowDefinitionsProperty).Add(rowDefinition4);
			grid.SetValue(Grid.RowProperty, 0);
			columnDefinition.SetValue(ColumnDefinition.WidthProperty, new GridLengthTypeConverter().ConvertFromInvariantString("*"));
			grid.GetValue(Grid.ColumnDefinitionsProperty).Add(columnDefinition);
			columnDefinition2.SetValue(ColumnDefinition.WidthProperty, new GridLengthTypeConverter().ConvertFromInvariantString("*"));
			grid.GetValue(Grid.ColumnDefinitionsProperty).Add(columnDefinition2);
			sfRadioButton.SetValue(Grid.ColumnProperty, 0);
			sfRadioButton.SetValue(ToggleButton.IsCheckedProperty, new bool?(true));
			sfRadioButton.SetValue(ToggleButton.IsThreeStateProperty, false);
			sfRadioButton.StateChanged += this.rbActive_StateChanged;
			translate2.Text = "ios_ActiveSensors";
			IMarkupExtension markupExtension3 = translate2;
			XamlServiceProvider xamlServiceProvider3 = new XamlServiceProvider();
			Type typeFromHandle5 = typeof(IProvideValueTarget);
			object[] array3 = new object[0 + 4];
			array3[0] = sfRadioButton;
			array3[1] = grid;
			array3[2] = grid2;
			array3[3] = this;
			object obj4;
			xamlServiceProvider3.Add(typeFromHandle5, obj4 = new SimpleValueTargetProvider(array3, ToggleButton.TextProperty, nameScope));
			xamlServiceProvider3.Add(typeof(IReferenceProvider), obj4);
			Type typeFromHandle6 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver3 = new XmlNamespaceResolver();
			xmlNamespaceResolver3.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver3.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver3.Add("d", "http://xamarin.com/schemas/2014/forms/design");
			xmlNamespaceResolver3.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver3.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver3.Add("mc", "http://schemas.openxmlformats.org/markup-compatibility/2006");
			xmlNamespaceResolver3.Add("settings", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver3.Add("syncfusion", "clr-namespace:Syncfusion.XForms.Buttons;assembly=Syncfusion.Buttons.XForms");
			xamlServiceProvider3.Add(typeFromHandle6, new XamlTypeResolver(xmlNamespaceResolver3, typeof(SettingsDisabledPids).GetTypeInfo().Assembly));
			xamlServiceProvider3.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(36, 21)));
			object obj5 = markupExtension3.ProvideValue(xamlServiceProvider3);
			sfRadioButton.Text = obj5;
			grid.Children.Add(sfRadioButton);
			sfRadioButton2.SetValue(Grid.ColumnProperty, 1);
			sfRadioButton2.SetValue(ToggleButton.IsCheckedProperty, new bool?(false));
			sfRadioButton2.SetValue(ToggleButton.IsThreeStateProperty, false);
			sfRadioButton2.StateChanged += this.rbDisabled_StateChanged;
			translate3.Text = "ios_InactiveSensors";
			IMarkupExtension markupExtension4 = translate3;
			XamlServiceProvider xamlServiceProvider4 = new XamlServiceProvider();
			Type typeFromHandle7 = typeof(IProvideValueTarget);
			object[] array4 = new object[0 + 4];
			array4[0] = sfRadioButton2;
			array4[1] = grid;
			array4[2] = grid2;
			array4[3] = this;
			object obj6;
			xamlServiceProvider4.Add(typeFromHandle7, obj6 = new SimpleValueTargetProvider(array4, ToggleButton.TextProperty, nameScope));
			xamlServiceProvider4.Add(typeof(IReferenceProvider), obj6);
			Type typeFromHandle8 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver4 = new XmlNamespaceResolver();
			xmlNamespaceResolver4.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver4.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver4.Add("d", "http://xamarin.com/schemas/2014/forms/design");
			xmlNamespaceResolver4.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver4.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver4.Add("mc", "http://schemas.openxmlformats.org/markup-compatibility/2006");
			xmlNamespaceResolver4.Add("settings", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver4.Add("syncfusion", "clr-namespace:Syncfusion.XForms.Buttons;assembly=Syncfusion.Buttons.XForms");
			xamlServiceProvider4.Add(typeFromHandle8, new XamlTypeResolver(xmlNamespaceResolver4, typeof(SettingsDisabledPids).GetTypeInfo().Assembly));
			xamlServiceProvider4.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(43, 21)));
			object obj7 = markupExtension4.ProvideValue(xamlServiceProvider4);
			sfRadioButton2.Text = obj7;
			grid.Children.Add(sfRadioButton2);
			grid2.Children.Add(grid);
			listView.SetValue(Grid.RowProperty, 1);
			listView.SetValue(ListView.HasUnevenRowsProperty, true);
			listView.SetValue(VisualElement.IsVisibleProperty, new VisualElement.VisibilityConverter().ConvertFromInvariantString("True"));
			IDataTemplate dataTemplate3 = dataTemplate;
			SettingsDisabledPids.<InitializeComponent>_anonXamlCDataTemplate_91 <InitializeComponent>_anonXamlCDataTemplate_ = new SettingsDisabledPids.<InitializeComponent>_anonXamlCDataTemplate_91();
			object[] array5 = new object[0 + 4];
			array5[0] = dataTemplate;
			array5[1] = listView;
			array5[2] = grid2;
			array5[3] = this;
			<InitializeComponent>_anonXamlCDataTemplate_.parentValues = array5;
			<InitializeComponent>_anonXamlCDataTemplate_.root = this;
			dataTemplate3.LoadTemplate = new Func<object>(<InitializeComponent>_anonXamlCDataTemplate_.LoadDataTemplate);
			listView.SetValue(ItemsView<Cell>.ItemTemplateProperty, dataTemplate);
			grid2.Children.Add(listView);
			listView2.SetValue(Grid.RowProperty, 1);
			listView2.SetValue(ListView.HasUnevenRowsProperty, true);
			listView2.SetValue(VisualElement.IsVisibleProperty, new VisualElement.VisibilityConverter().ConvertFromInvariantString("False"));
			IDataTemplate dataTemplate4 = dataTemplate2;
			SettingsDisabledPids.<InitializeComponent>_anonXamlCDataTemplate_92 <InitializeComponent>_anonXamlCDataTemplate_2 = new SettingsDisabledPids.<InitializeComponent>_anonXamlCDataTemplate_92();
			object[] array6 = new object[0 + 4];
			array6[0] = dataTemplate2;
			array6[1] = listView2;
			array6[2] = grid2;
			array6[3] = this;
			<InitializeComponent>_anonXamlCDataTemplate_2.parentValues = array6;
			<InitializeComponent>_anonXamlCDataTemplate_2.root = this;
			dataTemplate4.LoadTemplate = new Func<object>(<InitializeComponent>_anonXamlCDataTemplate_2.LoadDataTemplate);
			listView2.SetValue(ItemsView<Cell>.ItemTemplateProperty, dataTemplate2);
			grid2.Children.Add(listView2);
			activityFrame.SetValue(Grid.RowProperty, 1);
			activityFrame.SetValue(VisualElement.IsVisibleProperty, new VisualElement.VisibilityConverter().ConvertFromInvariantString("False"));
			grid2.Children.Add(activityFrame);
			button.SetValue(Grid.RowProperty, 2);
			button.Clicked += this.btnRemoveUnsupported_Clicked;
			translate4.Text = "ios_RemoveUnsupported";
			IMarkupExtension markupExtension5 = translate4;
			XamlServiceProvider xamlServiceProvider5 = new XamlServiceProvider();
			Type typeFromHandle9 = typeof(IProvideValueTarget);
			object[] array7 = new object[0 + 3];
			array7[0] = button;
			array7[1] = grid2;
			array7[2] = this;
			object obj8;
			xamlServiceProvider5.Add(typeFromHandle9, obj8 = new SimpleValueTargetProvider(array7, Button.TextProperty, nameScope));
			xamlServiceProvider5.Add(typeof(IReferenceProvider), obj8);
			Type typeFromHandle10 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver5 = new XmlNamespaceResolver();
			xmlNamespaceResolver5.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver5.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver5.Add("d", "http://xamarin.com/schemas/2014/forms/design");
			xmlNamespaceResolver5.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver5.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver5.Add("mc", "http://schemas.openxmlformats.org/markup-compatibility/2006");
			xmlNamespaceResolver5.Add("settings", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver5.Add("syncfusion", "clr-namespace:Syncfusion.XForms.Buttons;assembly=Syncfusion.Buttons.XForms");
			xamlServiceProvider5.Add(typeFromHandle10, new XamlTypeResolver(xmlNamespaceResolver5, typeof(SettingsDisabledPids).GetTypeInfo().Assembly));
			xamlServiceProvider5.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(99, 17)));
			object obj9 = markupExtension5.ProvideValue(xamlServiceProvider5);
			button.Text = obj9;
			grid2.Children.Add(button);
			button2.SetValue(Grid.RowProperty, 3);
			button2.Clicked += this.btnResetRoles_Clicked;
			translate5.Text = "settings_ResetRolesToDefault";
			IMarkupExtension markupExtension6 = translate5;
			XamlServiceProvider xamlServiceProvider6 = new XamlServiceProvider();
			Type typeFromHandle11 = typeof(IProvideValueTarget);
			object[] array8 = new object[0 + 3];
			array8[0] = button2;
			array8[1] = grid2;
			array8[2] = this;
			object obj10;
			xamlServiceProvider6.Add(typeFromHandle11, obj10 = new SimpleValueTargetProvider(array8, Button.TextProperty, nameScope));
			xamlServiceProvider6.Add(typeof(IReferenceProvider), obj10);
			Type typeFromHandle12 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver6 = new XmlNamespaceResolver();
			xmlNamespaceResolver6.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver6.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver6.Add("d", "http://xamarin.com/schemas/2014/forms/design");
			xmlNamespaceResolver6.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver6.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver6.Add("mc", "http://schemas.openxmlformats.org/markup-compatibility/2006");
			xmlNamespaceResolver6.Add("settings", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver6.Add("syncfusion", "clr-namespace:Syncfusion.XForms.Buttons;assembly=Syncfusion.Buttons.XForms");
			xamlServiceProvider6.Add(typeFromHandle12, new XamlTypeResolver(xmlNamespaceResolver6, typeof(SettingsDisabledPids).GetTypeInfo().Assembly));
			xamlServiceProvider6.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(104, 17)));
			object obj11 = markupExtension6.ProvideValue(xamlServiceProvider6);
			button2.Text = obj11;
			grid2.Children.Add(button2);
			this.SetValue(ContentPage.ContentProperty, grid2);
		}

		// Token: 0x06001B07 RID: 6919 RVA: 0x0012DA2D File Offset: 0x0012BC2D
		[CompilerGenerated]
		private void <.ctor>b__0_0()
		{
			CustomPIDViewModel.DisabledProfile.Load();
			Device.BeginInvokeOnMainThread(delegate
			{
				this.lvEnabled.ItemsSource = CustomPIDViewModel.CurrentProfile.PidCollection;
				this.lvDisabled.ItemsSource = CustomPIDViewModel.DisabledProfile.PidCollection;
			});
		}

		// Token: 0x06001B08 RID: 6920 RVA: 0x0012DA4A File Offset: 0x0012BC4A
		[CompilerGenerated]
		private void <.ctor>b__0_1()
		{
			this.lvEnabled.ItemsSource = CustomPIDViewModel.CurrentProfile.PidCollection;
			this.lvDisabled.ItemsSource = CustomPIDViewModel.DisabledProfile.PidCollection;
		}

		// Token: 0x06001B09 RID: 6921 RVA: 0x0012DA78 File Offset: 0x0012BC78
		private void __InitComponentRuntime()
		{
			Extensions.LoadFromXaml<SettingsDisabledPids>(this, typeof(SettingsDisabledPids));
			this.rbActivePids = NameScopeExtensions.FindByName<SfRadioButton>(this, "rbActivePids");
			this.rbDisabledPids = NameScopeExtensions.FindByName<SfRadioButton>(this, "rbDisabledPids");
			this.lvEnabled = NameScopeExtensions.FindByName<ListView>(this, "lvEnabled");
			this.lvDisabled = NameScopeExtensions.FindByName<ListView>(this, "lvDisabled");
			this.activityFrame = NameScopeExtensions.FindByName<ActivityFrame>(this, "activityFrame");
			this.btnRemoveUnsupported = NameScopeExtensions.FindByName<Button>(this, "btnRemoveUnsupported");
			this.btnResetRoles = NameScopeExtensions.FindByName<Button>(this, "btnResetRoles");
		}

		// Token: 0x04000C59 RID: 3161
		private object lock_object = new object();

		// Token: 0x04000C5A RID: 3162
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private SfRadioButton rbActivePids;

		// Token: 0x04000C5B RID: 3163
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private SfRadioButton rbDisabledPids;

		// Token: 0x04000C5C RID: 3164
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private ListView lvEnabled;

		// Token: 0x04000C5D RID: 3165
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private ListView lvDisabled;

		// Token: 0x04000C5E RID: 3166
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private ActivityFrame activityFrame;

		// Token: 0x04000C5F RID: 3167
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private Button btnRemoveUnsupported;

		// Token: 0x04000C60 RID: 3168
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private Button btnResetRoles;

		// Token: 0x0200022C RID: 556
		[CompilerGenerated]
		private sealed class <>c__DisplayClass5_0
		{
			// Token: 0x06001B0A RID: 6922 RVA: 0x00002050 File Offset: 0x00000250
			public <>c__DisplayClass5_0()
			{
			}

			// Token: 0x06001B0B RID: 6923 RVA: 0x0012DB0D File Offset: 0x0012BD0D
			internal void <btnRemoveUnsupported_Clicked>b__0(int i)
			{
				MainThreadHelper.InvokeOnMainThread(new Action(new SettingsDisabledPids.<>c__DisplayClass5_1
				{
					CS$<>8__locals1 = this,
					i = i
				}.<btnRemoveUnsupported_Clicked>b__2));
			}

			// Token: 0x06001B0C RID: 6924 RVA: 0x0012DB34 File Offset: 0x0012BD34
			internal async void <btnRemoveUnsupported_Clicked>b__1(object cancelSender, EventArgs cancelArgs)
			{
				this.cts.Cancel();
				App.OBDReader.ReplaceQueue(new OBDRequest[0]);
			}

			// Token: 0x04000C61 RID: 3169
			public SettingsDisabledPids <>4__this;

			// Token: 0x04000C62 RID: 3170
			public string detectingString;

			// Token: 0x04000C63 RID: 3171
			public CancellationTokenSource cts;

			// Token: 0x0200022D RID: 557
			[StructLayout(LayoutKind.Auto)]
			private struct <<btnRemoveUnsupported_Clicked>b__1>d : IAsyncStateMachine
			{
				// Token: 0x06001B0D RID: 6925 RVA: 0x0012DB6C File Offset: 0x0012BD6C
				void IAsyncStateMachine.MoveNext()
				{
					SettingsDisabledPids.<>c__DisplayClass5_0 CS$<>8__locals1 = this;
					try
					{
						CS$<>8__locals1.cts.Cancel();
						App.OBDReader.ReplaceQueue(new OBDRequest[0]);
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

				// Token: 0x06001B0E RID: 6926 RVA: 0x0012DBD8 File Offset: 0x0012BDD8
				[DebuggerHidden]
				void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
				{
					this.<>t__builder.SetStateMachine(stateMachine);
				}

				// Token: 0x04000C64 RID: 3172
				public int <>1__state;

				// Token: 0x04000C65 RID: 3173
				public AsyncVoidMethodBuilder <>t__builder;

				// Token: 0x04000C66 RID: 3174
				public SettingsDisabledPids.<>c__DisplayClass5_0 <>4__this;
			}
		}

		// Token: 0x0200022E RID: 558
		[CompilerGenerated]
		private sealed class <>c__DisplayClass5_1
		{
			// Token: 0x06001B0F RID: 6927 RVA: 0x00002050 File Offset: 0x00000250
			public <>c__DisplayClass5_1()
			{
			}

			// Token: 0x06001B10 RID: 6928 RVA: 0x0012DBE8 File Offset: 0x0012BDE8
			internal void <btnRemoveUnsupported_Clicked>b__2()
			{
				this.CS$<>8__locals1.<>4__this.activityFrame.Text = string.Concat(new string[]
				{
					this.CS$<>8__locals1.detectingString,
					"\n",
					SharedSettings.Current.SelectedBrand,
					" ",
					SharedSettings.Current.SelectedProfileV2Name,
					"\n",
					this.i.ToString(),
					"%"
				});
			}

			// Token: 0x04000C67 RID: 3175
			public int i;

			// Token: 0x04000C68 RID: 3176
			public SettingsDisabledPids.<>c__DisplayClass5_0 CS$<>8__locals1;
		}

		// Token: 0x0200022F RID: 559
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <btnRemoveUnsupported_Clicked>d__5 : IAsyncStateMachine
		{
			// Token: 0x06001B11 RID: 6929 RVA: 0x0012DC6C File Offset: 0x0012BE6C
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				SettingsDisabledPids settingsDisabledPids = this;
				try
				{
					TaskAwaiter<string> taskAwaiter;
					if (num != 0)
					{
						SettingsDisabledPids.<>c__DisplayClass5_0 CS$<>8__locals1 = new SettingsDisabledPids.<>c__DisplayClass5_0();
						CS$<>8__locals1.<>4__this = this;
						if (App.OBDReader.CurrentStatus != OBDDataReaderStatus.ConnectedToECU || App.OBDSimulator.IsActive)
						{
							goto IL_01DD;
						}
						settingsDisabledPids.activityFrame.CancelText = Translate.GetString("btnCancel.Content");
						settingsDisabledPids.activityFrame.IsCancelVisible = true;
						settingsDisabledPids.btnRemoveUnsupported.IsEnabled = false;
						settingsDisabledPids.activityFrame.IsVisible = true;
						CS$<>8__locals1.detectingString = Translate.GetString("profile_CheckingSupportedPids");
						Progress<int> progress = new Progress<int>(delegate(int i)
						{
							MainThreadHelper.InvokeOnMainThread(new Action(new SettingsDisabledPids.<>c__DisplayClass5_1
							{
								CS$<>8__locals1 = CS$<>8__locals1,
								i = i
							}.<btnRemoveUnsupported_Clicked>b__2));
						});
						CS$<>8__locals1.cts = new CancellationTokenSource();
						EventHandler eventHandler = delegate(object cancelSender, EventArgs cancelArgs)
						{
							SettingsDisabledPids.<>c__DisplayClass5_0.<<btnRemoveUnsupported_Clicked>b__1>d <<btnRemoveUnsupported_Clicked>b__1>d;
							<<btnRemoveUnsupported_Clicked>b__1>d.<>t__builder = AsyncVoidMethodBuilder.Create();
							<<btnRemoveUnsupported_Clicked>b__1>d.<>4__this = CS$<>8__locals1;
							<<btnRemoveUnsupported_Clicked>b__1>d.<>1__state = -1;
							<<btnRemoveUnsupported_Clicked>b__1>d.<>t__builder.Start<SettingsDisabledPids.<>c__DisplayClass5_0.<<btnRemoveUnsupported_Clicked>b__1>d>(ref <<btnRemoveUnsupported_Clicked>b__1>d);
						};
						ActivityFrame activityFrame = settingsDisabledPids.activityFrame;
						activityFrame.CancelClicked = (EventHandler)Delegate.Remove(activityFrame.CancelClicked, eventHandler);
						ActivityFrame activityFrame2 = settingsDisabledPids.activityFrame;
						activityFrame2.CancelClicked = (EventHandler)Delegate.Combine(activityFrame2.CancelClicked, eventHandler);
						IEnumerator<CustomPID> enumerator = CustomPIDViewModel.DisabledProfile.PidCollection.GetEnumerator();
						try
						{
							while (enumerator.MoveNext())
							{
								CustomPID customPID = enumerator.Current;
								CustomPIDViewModel.CurrentProfile.PidCollection.Add(customPID);
							}
						}
						finally
						{
							if (num < 0 && enumerator != null)
							{
								enumerator.Dispose();
							}
						}
						CustomPIDViewModel.DisabledProfile.PidCollection.Clear();
						CustomPIDViewModel.DisabledProfile.Save();
						CustomPIDViewModel.CurrentProfile.Save();
						taskAwaiter = ProfileSupportedPidsTester.PerformTest(progress, CS$<>8__locals1.cts.Token).GetAwaiter();
						if (!taskAwaiter.IsCompleted)
						{
							num = (num2 = 0);
							TaskAwaiter<string> taskAwaiter2 = taskAwaiter;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<string>, SettingsDisabledPids.<btnRemoveUnsupported_Clicked>d__5>(ref taskAwaiter, ref this);
							return;
						}
					}
					else
					{
						TaskAwaiter<string> taskAwaiter2;
						taskAwaiter = taskAwaiter2;
						taskAwaiter2 = default(TaskAwaiter<string>);
						num = (num2 = -1);
					}
					taskAwaiter.GetResult();
					settingsDisabledPids.activityFrame.IsVisible = false;
					settingsDisabledPids.btnRemoveUnsupported.IsEnabled = true;
					IL_01DD:;
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

			// Token: 0x06001B12 RID: 6930 RVA: 0x0012DEB8 File Offset: 0x0012C0B8
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x04000C69 RID: 3177
			public int <>1__state;

			// Token: 0x04000C6A RID: 3178
			public AsyncVoidMethodBuilder <>t__builder;

			// Token: 0x04000C6B RID: 3179
			public SettingsDisabledPids <>4__this;

			// Token: 0x04000C6C RID: 3180
			private TaskAwaiter<string> <>u__1;
		}

		// Token: 0x02000230 RID: 560
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <btnResetRoles_Clicked>d__7 : IAsyncStateMachine
		{
			// Token: 0x06001B13 RID: 6931 RVA: 0x0012DEC8 File Offset: 0x0012C0C8
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				SettingsDisabledPids settingsDisabledPids = this;
				try
				{
					TaskAwaiter<bool> taskAwaiter3;
					if (num != 0)
					{
						taskAwaiter3 = settingsDisabledPids.DisplayAlert(Translate.GetString("settings_ResetRolesToDefault") + "?", "", "OK", Translate.GetString("btnCancel.Content")).GetAwaiter();
						if (!taskAwaiter3.IsCompleted)
						{
							num2 = 0;
							taskAwaiter2 = taskAwaiter3;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<bool>, SettingsDisabledPids.<btnResetRoles_Clicked>d__7>(ref taskAwaiter3, ref this);
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
						PIDOverrideDictionary.Instance.Clear();
						PIDOverrideDictionary.Instance.Save();
						App.OBDReader.CurrentCarData.InitializeCalculatedPIDs();
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

			// Token: 0x06001B14 RID: 6932 RVA: 0x0012DFC8 File Offset: 0x0012C1C8
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x04000C6D RID: 3181
			public int <>1__state;

			// Token: 0x04000C6E RID: 3182
			public AsyncVoidMethodBuilder <>t__builder;

			// Token: 0x04000C6F RID: 3183
			public SettingsDisabledPids <>4__this;

			// Token: 0x04000C70 RID: 3184
			private TaskAwaiter<bool> <>u__1;
		}

		// Token: 0x02000231 RID: 561
		[CompilerGenerated]
		private sealed class <InitializeComponent>_anonXamlCDataTemplate_91
		{
			// Token: 0x06001B15 RID: 6933 RVA: 0x0012DFD8 File Offset: 0x0012C1D8
			public <InitializeComponent>_anonXamlCDataTemplate_91()
			{
			}

			// Token: 0x06001B16 RID: 6934 RVA: 0x0012DFEC File Offset: 0x0012C1EC
			internal object LoadDataTemplate()
			{
				ColumnDefinition columnDefinition;
				VisualDiagnostics.RegisterSourceInfo(columnDefinition = new ColumnDefinition(), new Uri("Settings\\SettingsDisabledPids.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 55, 38);
				ColumnDefinition columnDefinition2;
				VisualDiagnostics.RegisterSourceInfo(columnDefinition2 = new ColumnDefinition(), new Uri("Settings\\SettingsDisabledPids.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 56, 38);
				BindingExtension bindingExtension;
				VisualDiagnostics.RegisterSourceInfo(bindingExtension = new BindingExtension(), new Uri("Settings\\SettingsDisabledPids.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 58, 56);
				Label label;
				VisualDiagnostics.RegisterSourceInfo(label = new Label(), new Uri("Settings\\SettingsDisabledPids.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 58, 34);
				Translate translate;
				VisualDiagnostics.RegisterSourceInfo(translate = new Translate(), new Uri("Settings\\SettingsDisabledPids.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 62, 37);
				Button button;
				VisualDiagnostics.RegisterSourceInfo(button = new Button(), new Uri("Settings\\SettingsDisabledPids.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 59, 34);
				Grid grid;
				VisualDiagnostics.RegisterSourceInfo(grid = new Grid(), new Uri("Settings\\SettingsDisabledPids.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 53, 30);
				ViewCell viewCell;
				VisualDiagnostics.RegisterSourceInfo(viewCell = new ViewCell(), new Uri("Settings\\SettingsDisabledPids.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 52, 26);
				NameScope nameScope = new NameScope();
				NameScope.SetNameScope(viewCell, nameScope);
				columnDefinition.SetValue(ColumnDefinition.WidthProperty, new GridLengthTypeConverter().ConvertFromInvariantString("*"));
				grid.GetValue(Grid.ColumnDefinitionsProperty).Add(columnDefinition);
				columnDefinition2.SetValue(ColumnDefinition.WidthProperty, new GridLengthTypeConverter().ConvertFromInvariantString("auto"));
				grid.GetValue(Grid.ColumnDefinitionsProperty).Add(columnDefinition2);
				label.SetValue(Grid.ColumnProperty, 0);
				bindingExtension.Path = "Name";
				BindingBase bindingBase = bindingExtension.ProvideValue(null);
				label.SetBinding(Label.TextProperty, bindingBase);
				grid.Children.Add(label);
				button.SetValue(Grid.ColumnProperty, 1);
				button.Clicked += this.root.btnDisable_Clicked;
				translate.Text = "ios_Deactivate";
				IMarkupExtension markupExtension = translate;
				XamlServiceProvider xamlServiceProvider = new XamlServiceProvider();
				Type typeFromHandle = typeof(IProvideValueTarget);
				int num;
				object[] array = new object[(num = this.parentValues.Length) + 3];
				Array.Copy(this.parentValues, 0, array, 3, num);
				object[] array2 = array;
				array2[0] = button;
				array2[1] = grid;
				array2[2] = viewCell;
				object obj;
				xamlServiceProvider.Add(typeFromHandle, obj = new SimpleValueTargetProvider(array2, Button.TextProperty, nameScope));
				xamlServiceProvider.Add(typeof(IReferenceProvider), obj);
				Type typeFromHandle2 = typeof(IXamlTypeResolver);
				XmlNamespaceResolver xmlNamespaceResolver = new XmlNamespaceResolver();
				xmlNamespaceResolver.Add("", "http://xamarin.com/schemas/2014/forms");
				xmlNamespaceResolver.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
				xmlNamespaceResolver.Add("d", "http://xamarin.com/schemas/2014/forms/design");
				xmlNamespaceResolver.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
				xmlNamespaceResolver.Add("local", "clr-namespace:CarScannerXamarinForms");
				xmlNamespaceResolver.Add("mc", "http://schemas.openxmlformats.org/markup-compatibility/2006");
				xmlNamespaceResolver.Add("settings", "clr-namespace:CarScannerXamarinForms.Settings");
				xmlNamespaceResolver.Add("syncfusion", "clr-namespace:Syncfusion.XForms.Buttons;assembly=Syncfusion.Buttons.XForms");
				xamlServiceProvider.Add(typeFromHandle2, new XamlTypeResolver(xmlNamespaceResolver, typeof(SettingsDisabledPids.<InitializeComponent>_anonXamlCDataTemplate_91).GetTypeInfo().Assembly));
				xamlServiceProvider.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(62, 37)));
				object obj2 = markupExtension.ProvideValue(xamlServiceProvider);
				button.Text = obj2;
				grid.Children.Add(button);
				viewCell.View = grid;
				return viewCell;
			}

			// Token: 0x04000C71 RID: 3185
			internal object[] parentValues;

			// Token: 0x04000C72 RID: 3186
			internal SettingsDisabledPids root;
		}

		// Token: 0x02000232 RID: 562
		[CompilerGenerated]
		private sealed class <InitializeComponent>_anonXamlCDataTemplate_92
		{
			// Token: 0x06001B17 RID: 6935 RVA: 0x0012E390 File Offset: 0x0012C590
			public <InitializeComponent>_anonXamlCDataTemplate_92()
			{
			}

			// Token: 0x06001B18 RID: 6936 RVA: 0x0012E3A4 File Offset: 0x0012C5A4
			internal object LoadDataTemplate()
			{
				ColumnDefinition columnDefinition;
				VisualDiagnostics.RegisterSourceInfo(columnDefinition = new ColumnDefinition(), new Uri("Settings\\SettingsDisabledPids.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 78, 38);
				ColumnDefinition columnDefinition2;
				VisualDiagnostics.RegisterSourceInfo(columnDefinition2 = new ColumnDefinition(), new Uri("Settings\\SettingsDisabledPids.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 79, 38);
				BindingExtension bindingExtension;
				VisualDiagnostics.RegisterSourceInfo(bindingExtension = new BindingExtension(), new Uri("Settings\\SettingsDisabledPids.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 81, 56);
				Label label;
				VisualDiagnostics.RegisterSourceInfo(label = new Label(), new Uri("Settings\\SettingsDisabledPids.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 81, 34);
				Translate translate;
				VisualDiagnostics.RegisterSourceInfo(translate = new Translate(), new Uri("Settings\\SettingsDisabledPids.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 85, 37);
				Button button;
				VisualDiagnostics.RegisterSourceInfo(button = new Button(), new Uri("Settings\\SettingsDisabledPids.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 82, 34);
				Grid grid;
				VisualDiagnostics.RegisterSourceInfo(grid = new Grid(), new Uri("Settings\\SettingsDisabledPids.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 76, 30);
				ViewCell viewCell;
				VisualDiagnostics.RegisterSourceInfo(viewCell = new ViewCell(), new Uri("Settings\\SettingsDisabledPids.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 75, 26);
				NameScope nameScope = new NameScope();
				NameScope.SetNameScope(viewCell, nameScope);
				columnDefinition.SetValue(ColumnDefinition.WidthProperty, new GridLengthTypeConverter().ConvertFromInvariantString("*"));
				grid.GetValue(Grid.ColumnDefinitionsProperty).Add(columnDefinition);
				columnDefinition2.SetValue(ColumnDefinition.WidthProperty, new GridLengthTypeConverter().ConvertFromInvariantString("auto"));
				grid.GetValue(Grid.ColumnDefinitionsProperty).Add(columnDefinition2);
				label.SetValue(Grid.ColumnProperty, 0);
				bindingExtension.Path = "Name";
				BindingBase bindingBase = bindingExtension.ProvideValue(null);
				label.SetBinding(Label.TextProperty, bindingBase);
				grid.Children.Add(label);
				button.SetValue(Grid.ColumnProperty, 1);
				button.Clicked += this.root.btnEnable_Clicked;
				translate.Text = "ios_Activate";
				IMarkupExtension markupExtension = translate;
				XamlServiceProvider xamlServiceProvider = new XamlServiceProvider();
				Type typeFromHandle = typeof(IProvideValueTarget);
				int num;
				object[] array = new object[(num = this.parentValues.Length) + 3];
				Array.Copy(this.parentValues, 0, array, 3, num);
				object[] array2 = array;
				array2[0] = button;
				array2[1] = grid;
				array2[2] = viewCell;
				object obj;
				xamlServiceProvider.Add(typeFromHandle, obj = new SimpleValueTargetProvider(array2, Button.TextProperty, nameScope));
				xamlServiceProvider.Add(typeof(IReferenceProvider), obj);
				Type typeFromHandle2 = typeof(IXamlTypeResolver);
				XmlNamespaceResolver xmlNamespaceResolver = new XmlNamespaceResolver();
				xmlNamespaceResolver.Add("", "http://xamarin.com/schemas/2014/forms");
				xmlNamespaceResolver.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
				xmlNamespaceResolver.Add("d", "http://xamarin.com/schemas/2014/forms/design");
				xmlNamespaceResolver.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
				xmlNamespaceResolver.Add("local", "clr-namespace:CarScannerXamarinForms");
				xmlNamespaceResolver.Add("mc", "http://schemas.openxmlformats.org/markup-compatibility/2006");
				xmlNamespaceResolver.Add("settings", "clr-namespace:CarScannerXamarinForms.Settings");
				xmlNamespaceResolver.Add("syncfusion", "clr-namespace:Syncfusion.XForms.Buttons;assembly=Syncfusion.Buttons.XForms");
				xamlServiceProvider.Add(typeFromHandle2, new XamlTypeResolver(xmlNamespaceResolver, typeof(SettingsDisabledPids.<InitializeComponent>_anonXamlCDataTemplate_92).GetTypeInfo().Assembly));
				xamlServiceProvider.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(85, 37)));
				object obj2 = markupExtension.ProvideValue(xamlServiceProvider);
				button.Text = obj2;
				grid.Children.Add(button);
				viewCell.View = grid;
				return viewCell;
			}

			// Token: 0x04000C73 RID: 3187
			internal object[] parentValues;

			// Token: 0x04000C74 RID: 3188
			internal SettingsDisabledPids root;
		}
	}
}
