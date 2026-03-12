using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using CarScannerXamarinForms.OBD2;
using CarScannerXamarinForms.PlatformAdapters;
using CarScannerXamarinForms.Settings;
using Xamarin.Forms;
using Xamarin.Forms.Internals;
using Xamarin.Forms.PlatformConfiguration.iOSSpecific;
using Xamarin.Forms.Xaml;
using Xamarin.Forms.Xaml.Diagnostics;
using Xamarin.Forms.Xaml.Internals;

namespace CarScannerXamarinForms
{
	// Token: 0x020001E4 RID: 484
	[XamlFilePath("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Settings\\SettingsPage.xaml")]
	public class SettingsPage : ContentPage
	{
		// Token: 0x060019C9 RID: 6601 RVA: 0x000027D4 File Offset: 0x000009D4
		private void Handle_SizeChanged(object sender, EventArgs e)
		{
		}

		// Token: 0x060019CA RID: 6602 RVA: 0x0010BC76 File Offset: 0x00109E76
		public SettingsPage()
		{
			this.InitializeComponent();
		}

		// Token: 0x060019CB RID: 6603 RVA: 0x0010BC84 File Offset: 0x00109E84
		private void btnConnection_Clicked(object sender, EventArgs e)
		{
			base.Navigation.PushAsync(new SettingsConnectionPage());
		}

		// Token: 0x060019CC RID: 6604 RVA: 0x0010BC97 File Offset: 0x00109E97
		private void btnCustomPidsSettings_Clicked(object sender, EventArgs e)
		{
			base.Navigation.PushAsync(new CustomPIDsListPage());
		}

		// Token: 0x060019CD RID: 6605 RVA: 0x0010BCAA File Offset: 0x00109EAA
		private void btnInterfaceSettings_Clicked(object sender, EventArgs e)
		{
			base.Navigation.PushAsync(new SettingsInterfacePage());
		}

		// Token: 0x060019CE RID: 6606 RVA: 0x0010BCBD File Offset: 0x00109EBD
		private void btnFuelRateSettings_Clicked(object sender, EventArgs e)
		{
			base.Navigation.PushAsync(new SettingsFuelRatePage());
		}

		// Token: 0x060019CF RID: 6607 RVA: 0x0010BCD0 File Offset: 0x00109ED0
		private void btnVehicleOptionsSettings_Clicked(object sender, EventArgs e)
		{
			base.Navigation.PushAsync(new SettingsVehicleOptions());
		}

		// Token: 0x060019D0 RID: 6608 RVA: 0x0010BCE4 File Offset: 0x00109EE4
		private async void Handle_ItemTapped(object sender, ItemTappedEventArgs e)
		{
			ListView lv = sender as ListView;
			SettingsPageElement settingsPageElement = e.Item as SettingsPageElement;
			lv.IsEnabled = false;
			lv.SelectedItem = null;
			await this.Launch(settingsPageElement);
			lv.IsEnabled = true;
		}

		// Token: 0x060019D1 RID: 6609 RVA: 0x0010BD2C File Offset: 0x00109F2C
		private async Task Launch(SettingsPageElement spe)
		{
			if (spe.PageType != null)
			{
				Page page = (Page)Activator.CreateInstance(spe.PageType);
				if (page is InAppPurchasePage)
				{
					InAppPurchasePage inAppPurchasePage = page as InAppPurchasePage;
					if (spe.Image == "settings_restore.png")
					{
						inAppPurchasePage.RestoreOnAppear = true;
					}
					await base.Navigation.PushAsync(page);
				}
				else if (page is SettingsGarage)
				{
					if (App.OBDReader.CurrentStatus == OBDDataReaderStatus.Disconnected)
					{
						await base.Navigation.PushAsync(page);
					}
					else
					{
						await base.DisplayAlert(Translate.GetString("ios_PleaseDisconnectFirst_Title"), Translate.GetString("ios_PleaseDisconnectFirst_Text"), "OK");
					}
				}
				else
				{
					await base.Navigation.PushAsync(page);
				}
			}
			else if (spe.Image == "settings_rate.png")
			{
				string text = string.Format(Translate.GetString("ios_AboutRatingsTitle"), PlatformHelper.AppMarketTitle);
				bool flag = Device.RuntimePlatform == "iOS";
				if (!flag)
				{
					flag = await base.DisplayAlert(text, Translate.GetString("ios_AboutRatings"), "OK", Translate.GetString("btnCancel.Content"));
				}
				if (flag)
				{
					DependencyService.Get<IRequestReview>(0).Request(true);
				}
			}
			else if (spe.Image == "settings_mail.png")
			{
				if (App.OBDReader.CurrentStatus == OBDDataReaderStatus.Disconnected)
				{
					if (PlatformHelper.IsAndroid)
					{
						await base.DisplayAlert(Translate.GetString("DtcPage_CleanCodes_Title"), Translate.GetString("droid_SelectEmail"), "OK");
					}
					await SharedSettings.Current.SendDebugEmail();
				}
				else
				{
					await base.DisplayAlert(Translate.GetString("ios_PleaseDisconnectFirst_Title"), "", "OK");
				}
			}
			else if (spe.Image == "settings_terminal.png")
			{
				if (App.OBDReader.CurrentStatus == OBDDataReaderStatus.ConnectedToECU || App.OBDReader.CurrentStatus == OBDDataReaderStatus.ConnectedToELM)
				{
					await base.Navigation.PushAsync(new TerminalPage());
				}
				else
				{
					TaskAwaiter<bool> taskAwaiter = base.DisplayAlert(Translate.GetString("ios_ConnectToELMOnlyTitle"), Translate.GetString("ios_ConnectToELMOnlyText"), "OK", Translate.GetString("btnCancel.Content")).GetAwaiter();
					if (!taskAwaiter.IsCompleted)
					{
						await taskAwaiter;
						TaskAwaiter<bool> taskAwaiter2;
						taskAwaiter = taskAwaiter2;
						taskAwaiter2 = default(TaskAwaiter<bool>);
					}
					if (taskAwaiter.GetResult())
					{
						SimpleMainPage.Instance.StartConnection(false);
						await App.GetCurrentPage().Navigation.PopAsync();
					}
				}
			}
		}

		// Token: 0x060019D2 RID: 6610 RVA: 0x0010BD78 File Offset: 0x00109F78
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private void InitializeComponent()
		{
			if (ResourceLoader.IsEnabled && ResourceLoader.CanProvideContentFor(new ResourceLoader.ResourceLoadingQuery
			{
				AssemblyName = typeof(SettingsPage).GetTypeInfo().Assembly.GetName(),
				ResourcePath = "Settings/SettingsPage.xaml",
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
			VisualDiagnostics.RegisterSourceInfo(translate = new Translate(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Settings\\SettingsPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 8, 5);
			DynamicResourceExtension dynamicResourceExtension;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension = new DynamicResourceExtension(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Settings\\SettingsPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 10, 5);
			List<SettingsPageElement> settingsList;
			VisualDiagnostics.RegisterSourceInfo(settingsList = StaticLists.SettingsList, new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Settings\\SettingsPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 17, 13);
			BindingExtension bindingExtension;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension = new BindingExtension(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Settings\\SettingsPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 17, 13);
			OnPlatform<Thickness> onPlatform;
			VisualDiagnostics.RegisterSourceInfo(onPlatform = new OnPlatform<Thickness>(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Settings\\SettingsPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 19, 18);
			DataTemplate dataTemplate;
			VisualDiagnostics.RegisterSourceInfo(dataTemplate = new DataTemplate(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Settings\\SettingsPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 26, 18);
			ListView listView;
			VisualDiagnostics.RegisterSourceInfo(listView = new ListView(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Settings\\SettingsPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 13, 10);
			VisualDiagnostics.RegisterSourceInfo(this, new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Settings\\SettingsPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 2, 2);
			NameScope nameScope = NameScope.GetNameScope(this) ?? new NameScope();
			NameScope.SetNameScope(this, nameScope);
			translate.Text = "ios_MainPage_Settings";
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
			xamlServiceProvider.Add(typeFromHandle2, new XamlTypeResolver(xmlNamespaceResolver, typeof(SettingsPage).GetTypeInfo().Assembly));
			xamlServiceProvider.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(8, 5)));
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
			xmlNamespaceResolver2.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver2.Add("local", "clr-namespace:CarScannerXamarinForms");
			xamlServiceProvider2.Add(typeFromHandle4, new XamlTypeResolver(xmlNamespaceResolver2, typeof(SettingsPage).GetTypeInfo().Assembly));
			xamlServiceProvider2.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(10, 5)));
			DynamicResource dynamicResource = markupExtension2.ProvideValue(xamlServiceProvider2);
			this.SetDynamicResource(VisualElement.BackgroundColorProperty, dynamicResource.Key);
			this.SizeChanged += this.Handle_SizeChanged;
			listView.SetValue(View.MarginProperty, new Thickness(5.0, 0.0));
			listView.SetValue(VisualElement.BackgroundColorProperty, Color.Transparent);
			listView.ItemTapped += this.Handle_ItemTapped;
			bindingExtension.Source = settingsList;
			BindingBase bindingBase = bindingExtension.ProvideValue(null);
			listView.SetBinding(ItemsView<Cell>.ItemsSourceProperty, bindingBase);
			onPlatform.Android = new Thickness(5.0, 0.0, 5.0, 0.0);
			onPlatform.WinPhone = new Thickness(0.0);
			onPlatform.iOS = new Thickness(5.0, 0.0, 5.0, 0.0);
			listView.SetValue(View.MarginProperty, onPlatform);
			IDataTemplate dataTemplate2 = dataTemplate;
			SettingsPage.<InitializeComponent>_anonXamlCDataTemplate_102 <InitializeComponent>_anonXamlCDataTemplate_ = new SettingsPage.<InitializeComponent>_anonXamlCDataTemplate_102();
			object[] array3 = new object[0 + 3];
			array3[0] = dataTemplate;
			array3[1] = listView;
			array3[2] = this;
			<InitializeComponent>_anonXamlCDataTemplate_.parentValues = array3;
			<InitializeComponent>_anonXamlCDataTemplate_.root = this;
			dataTemplate2.LoadTemplate = new Func<object>(<InitializeComponent>_anonXamlCDataTemplate_.LoadDataTemplate);
			listView.SetValue(ItemsView<Cell>.ItemTemplateProperty, dataTemplate);
			this.SetValue(ContentPage.ContentProperty, listView);
		}

		// Token: 0x060019D3 RID: 6611 RVA: 0x0010C2B1 File Offset: 0x0010A4B1
		private void __InitComponentRuntime()
		{
			Extensions.LoadFromXaml<SettingsPage>(this, typeof(SettingsPage));
		}

		// Token: 0x020001E5 RID: 485
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <Handle_ItemTapped>d__7 : IAsyncStateMachine
		{
			// Token: 0x060019D4 RID: 6612 RVA: 0x0010C2C4 File Offset: 0x0010A4C4
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				SettingsPage settingsPage = this;
				try
				{
					TaskAwaiter taskAwaiter;
					if (num != 0)
					{
						lv = sender as ListView;
						SettingsPageElement settingsPageElement = e.Item as SettingsPageElement;
						lv.IsEnabled = false;
						lv.SelectedItem = null;
						taskAwaiter = settingsPage.Launch(settingsPageElement).GetAwaiter();
						if (!taskAwaiter.IsCompleted)
						{
							num2 = 0;
							TaskAwaiter taskAwaiter2 = taskAwaiter;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, SettingsPage.<Handle_ItemTapped>d__7>(ref taskAwaiter, ref this);
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
					lv.IsEnabled = true;
				}
				catch (Exception ex)
				{
					num2 = -2;
					lv = null;
					this.<>t__builder.SetException(ex);
					return;
				}
				num2 = -2;
				lv = null;
				this.<>t__builder.SetResult();
			}

			// Token: 0x060019D5 RID: 6613 RVA: 0x0010C3D0 File Offset: 0x0010A5D0
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x04000B14 RID: 2836
			public int <>1__state;

			// Token: 0x04000B15 RID: 2837
			public AsyncVoidMethodBuilder <>t__builder;

			// Token: 0x04000B16 RID: 2838
			public object sender;

			// Token: 0x04000B17 RID: 2839
			public ItemTappedEventArgs e;

			// Token: 0x04000B18 RID: 2840
			public SettingsPage <>4__this;

			// Token: 0x04000B19 RID: 2841
			private ListView <lv>5__2;

			// Token: 0x04000B1A RID: 2842
			private TaskAwaiter <>u__1;
		}

		// Token: 0x020001E6 RID: 486
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <Launch>d__8 : IAsyncStateMachine
		{
			// Token: 0x060019D6 RID: 6614 RVA: 0x0010C3E0 File Offset: 0x0010A5E0
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				SettingsPage settingsPage = this;
				try
				{
					TaskAwaiter taskAwaiter3;
					TaskAwaiter<bool> taskAwaiter5;
					TaskAwaiter<Page> taskAwaiter6;
					bool flag;
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
						goto IL_0179;
					}
					case 2:
					{
						TaskAwaiter taskAwaiter4;
						taskAwaiter3 = taskAwaiter4;
						taskAwaiter4 = default(TaskAwaiter);
						num2 = -1;
						goto IL_01F5;
					}
					case 3:
					{
						TaskAwaiter taskAwaiter4;
						taskAwaiter3 = taskAwaiter4;
						taskAwaiter4 = default(TaskAwaiter);
						num2 = -1;
						goto IL_025E;
					}
					case 4:
						taskAwaiter5 = taskAwaiter2;
						taskAwaiter2 = default(TaskAwaiter<bool>);
						num2 = -1;
						goto IL_0321;
					case 5:
					{
						TaskAwaiter taskAwaiter4;
						taskAwaiter3 = taskAwaiter4;
						taskAwaiter4 = default(TaskAwaiter);
						num2 = -1;
						goto IL_03E2;
					}
					case 6:
					{
						TaskAwaiter taskAwaiter4;
						taskAwaiter3 = taskAwaiter4;
						taskAwaiter4 = default(TaskAwaiter);
						num2 = -1;
						goto IL_0444;
					}
					case 7:
					{
						TaskAwaiter taskAwaiter4;
						taskAwaiter3 = taskAwaiter4;
						taskAwaiter4 = default(TaskAwaiter);
						num2 = -1;
						goto IL_04BB;
					}
					case 8:
					{
						TaskAwaiter taskAwaiter4;
						taskAwaiter3 = taskAwaiter4;
						taskAwaiter4 = default(TaskAwaiter);
						num2 = -1;
						goto IL_055C;
					}
					case 9:
						taskAwaiter5 = taskAwaiter2;
						taskAwaiter2 = default(TaskAwaiter<bool>);
						num2 = -1;
						goto IL_05E3;
					case 10:
					{
						TaskAwaiter<Page> taskAwaiter7;
						taskAwaiter6 = taskAwaiter7;
						taskAwaiter7 = default(TaskAwaiter<Page>);
						num2 = -1;
						goto IL_0656;
					}
					default:
						if (spe.PageType != null)
						{
							Page page = (Page)Activator.CreateInstance(spe.PageType);
							if (page is InAppPurchasePage)
							{
								InAppPurchasePage inAppPurchasePage = page as InAppPurchasePage;
								if (spe.Image == "settings_restore.png")
								{
									inAppPurchasePage.RestoreOnAppear = true;
								}
								taskAwaiter3 = settingsPage.Navigation.PushAsync(page).GetAwaiter();
								if (!taskAwaiter3.IsCompleted)
								{
									num2 = 0;
									TaskAwaiter taskAwaiter4 = taskAwaiter3;
									this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, SettingsPage.<Launch>d__8>(ref taskAwaiter3, ref this);
									return;
								}
							}
							else if (page is SettingsGarage)
							{
								if (App.OBDReader.CurrentStatus == OBDDataReaderStatus.Disconnected)
								{
									taskAwaiter3 = settingsPage.Navigation.PushAsync(page).GetAwaiter();
									if (!taskAwaiter3.IsCompleted)
									{
										num2 = 1;
										TaskAwaiter taskAwaiter4 = taskAwaiter3;
										this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, SettingsPage.<Launch>d__8>(ref taskAwaiter3, ref this);
										return;
									}
									goto IL_0179;
								}
								else
								{
									taskAwaiter3 = settingsPage.DisplayAlert(Translate.GetString("ios_PleaseDisconnectFirst_Title"), Translate.GetString("ios_PleaseDisconnectFirst_Text"), "OK").GetAwaiter();
									if (!taskAwaiter3.IsCompleted)
									{
										num2 = 2;
										TaskAwaiter taskAwaiter4 = taskAwaiter3;
										this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, SettingsPage.<Launch>d__8>(ref taskAwaiter3, ref this);
										return;
									}
									goto IL_01F5;
								}
							}
							else
							{
								taskAwaiter3 = settingsPage.Navigation.PushAsync(page).GetAwaiter();
								if (!taskAwaiter3.IsCompleted)
								{
									num2 = 3;
									TaskAwaiter taskAwaiter4 = taskAwaiter3;
									this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, SettingsPage.<Launch>d__8>(ref taskAwaiter3, ref this);
									return;
								}
								goto IL_025E;
							}
						}
						else if (spe.Image == "settings_rate.png")
						{
							string text = string.Format(Translate.GetString("ios_AboutRatingsTitle"), PlatformHelper.AppMarketTitle);
							flag = Device.RuntimePlatform == "iOS";
							if (flag)
							{
								goto IL_032A;
							}
							taskAwaiter5 = settingsPage.DisplayAlert(text, Translate.GetString("ios_AboutRatings"), "OK", Translate.GetString("btnCancel.Content")).GetAwaiter();
							if (!taskAwaiter5.IsCompleted)
							{
								num2 = 4;
								taskAwaiter2 = taskAwaiter5;
								this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<bool>, SettingsPage.<Launch>d__8>(ref taskAwaiter5, ref this);
								return;
							}
							goto IL_0321;
						}
						else if (spe.Image == "settings_mail.png")
						{
							if (App.OBDReader.CurrentStatus == OBDDataReaderStatus.Disconnected)
							{
								if (!PlatformHelper.IsAndroid)
								{
									goto IL_03E9;
								}
								taskAwaiter3 = settingsPage.DisplayAlert(Translate.GetString("DtcPage_CleanCodes_Title"), Translate.GetString("droid_SelectEmail"), "OK").GetAwaiter();
								if (!taskAwaiter3.IsCompleted)
								{
									num2 = 5;
									TaskAwaiter taskAwaiter4 = taskAwaiter3;
									this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, SettingsPage.<Launch>d__8>(ref taskAwaiter3, ref this);
									return;
								}
								goto IL_03E2;
							}
							else
							{
								taskAwaiter3 = settingsPage.DisplayAlert(Translate.GetString("ios_PleaseDisconnectFirst_Title"), "", "OK").GetAwaiter();
								if (!taskAwaiter3.IsCompleted)
								{
									num2 = 7;
									TaskAwaiter taskAwaiter4 = taskAwaiter3;
									this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, SettingsPage.<Launch>d__8>(ref taskAwaiter3, ref this);
									return;
								}
								goto IL_04BB;
							}
						}
						else
						{
							if (!(spe.Image == "settings_terminal.png"))
							{
								goto IL_065E;
							}
							if (App.OBDReader.CurrentStatus == OBDDataReaderStatus.ConnectedToECU || App.OBDReader.CurrentStatus == OBDDataReaderStatus.ConnectedToELM)
							{
								taskAwaiter3 = settingsPage.Navigation.PushAsync(new TerminalPage()).GetAwaiter();
								if (!taskAwaiter3.IsCompleted)
								{
									num2 = 8;
									TaskAwaiter taskAwaiter4 = taskAwaiter3;
									this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, SettingsPage.<Launch>d__8>(ref taskAwaiter3, ref this);
									return;
								}
								goto IL_055C;
							}
							else
							{
								taskAwaiter5 = settingsPage.DisplayAlert(Translate.GetString("ios_ConnectToELMOnlyTitle"), Translate.GetString("ios_ConnectToELMOnlyText"), "OK", Translate.GetString("btnCancel.Content")).GetAwaiter();
								if (!taskAwaiter5.IsCompleted)
								{
									num2 = 9;
									taskAwaiter2 = taskAwaiter5;
									this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<bool>, SettingsPage.<Launch>d__8>(ref taskAwaiter5, ref this);
									return;
								}
								goto IL_05E3;
							}
						}
						break;
					}
					taskAwaiter3.GetResult();
					goto IL_065E;
					IL_0179:
					taskAwaiter3.GetResult();
					goto IL_065E;
					IL_01F5:
					taskAwaiter3.GetResult();
					goto IL_065E;
					IL_025E:
					taskAwaiter3.GetResult();
					goto IL_065E;
					IL_0321:
					flag = taskAwaiter5.GetResult();
					IL_032A:
					if (flag)
					{
						DependencyService.Get<IRequestReview>(0).Request(true);
						goto IL_065E;
					}
					goto IL_065E;
					IL_03E2:
					taskAwaiter3.GetResult();
					IL_03E9:
					taskAwaiter3 = SharedSettings.Current.SendDebugEmail().GetAwaiter();
					if (!taskAwaiter3.IsCompleted)
					{
						num2 = 6;
						TaskAwaiter taskAwaiter4 = taskAwaiter3;
						this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, SettingsPage.<Launch>d__8>(ref taskAwaiter3, ref this);
						return;
					}
					IL_0444:
					taskAwaiter3.GetResult();
					goto IL_065E;
					IL_04BB:
					taskAwaiter3.GetResult();
					goto IL_065E;
					IL_055C:
					taskAwaiter3.GetResult();
					goto IL_065E;
					IL_05E3:
					if (!taskAwaiter5.GetResult())
					{
						goto IL_065E;
					}
					SimpleMainPage.Instance.StartConnection(false);
					taskAwaiter6 = App.GetCurrentPage().Navigation.PopAsync().GetAwaiter();
					if (!taskAwaiter6.IsCompleted)
					{
						num2 = 10;
						TaskAwaiter<Page> taskAwaiter7 = taskAwaiter6;
						this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<Page>, SettingsPage.<Launch>d__8>(ref taskAwaiter6, ref this);
						return;
					}
					IL_0656:
					taskAwaiter6.GetResult();
					IL_065E:;
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

			// Token: 0x060019D7 RID: 6615 RVA: 0x0010CA98 File Offset: 0x0010AC98
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x04000B1B RID: 2843
			public int <>1__state;

			// Token: 0x04000B1C RID: 2844
			public AsyncTaskMethodBuilder <>t__builder;

			// Token: 0x04000B1D RID: 2845
			public SettingsPageElement spe;

			// Token: 0x04000B1E RID: 2846
			public SettingsPage <>4__this;

			// Token: 0x04000B1F RID: 2847
			private TaskAwaiter <>u__1;

			// Token: 0x04000B20 RID: 2848
			private TaskAwaiter<bool> <>u__2;

			// Token: 0x04000B21 RID: 2849
			private TaskAwaiter<Page> <>u__3;
		}

		// Token: 0x020001E7 RID: 487
		[CompilerGenerated]
		private sealed class <InitializeComponent>_anonXamlCDataTemplate_102
		{
			// Token: 0x060019D8 RID: 6616 RVA: 0x0010CAA8 File Offset: 0x0010ACA8
			public <InitializeComponent>_anonXamlCDataTemplate_102()
			{
			}

			// Token: 0x060019D9 RID: 6617 RVA: 0x0010CABC File Offset: 0x0010ACBC
			internal object LoadDataTemplate()
			{
				ColumnDefinition columnDefinition;
				VisualDiagnostics.RegisterSourceInfo(columnDefinition = new ColumnDefinition(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Settings\\SettingsPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 31, 34);
				ColumnDefinition columnDefinition2;
				VisualDiagnostics.RegisterSourceInfo(columnDefinition2 = new ColumnDefinition(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Settings\\SettingsPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 32, 34);
				ColumnDefinition columnDefinition3;
				VisualDiagnostics.RegisterSourceInfo(columnDefinition3 = new ColumnDefinition(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Settings\\SettingsPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 33, 34);
				ColumnDefinition columnDefinition4;
				VisualDiagnostics.RegisterSourceInfo(columnDefinition4 = new ColumnDefinition(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Settings\\SettingsPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 34, 34);
				BindingExtension bindingExtension;
				VisualDiagnostics.RegisterSourceInfo(bindingExtension = new BindingExtension(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Settings\\SettingsPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 39, 33);
				Image image;
				VisualDiagnostics.RegisterSourceInfo(image = new Image(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Settings\\SettingsPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 37, 30);
				BindingExtension bindingExtension2;
				VisualDiagnostics.RegisterSourceInfo(bindingExtension2 = new BindingExtension(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Settings\\SettingsPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 44, 33);
				DynamicResourceExtension dynamicResourceExtension;
				VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension = new DynamicResourceExtension(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Settings\\SettingsPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 45, 33);
				Label label;
				VisualDiagnostics.RegisterSourceInfo(label = new Label(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Settings\\SettingsPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 41, 30);
				Label label2;
				VisualDiagnostics.RegisterSourceInfo(label2 = new Label(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Settings\\SettingsPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 48, 30);
				Grid grid;
				VisualDiagnostics.RegisterSourceInfo(grid = new Grid(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Settings\\SettingsPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 29, 26);
				ViewCell viewCell;
				VisualDiagnostics.RegisterSourceInfo(viewCell = new ViewCell(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Settings\\SettingsPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 27, 22);
				NameScope nameScope = new NameScope();
				NameScope.SetNameScope(viewCell, nameScope);
				columnDefinition.SetValue(ColumnDefinition.WidthProperty, new GridLengthTypeConverter().ConvertFromInvariantString("auto"));
				grid.GetValue(Grid.ColumnDefinitionsProperty).Add(columnDefinition);
				columnDefinition2.SetValue(ColumnDefinition.WidthProperty, new GridLengthTypeConverter().ConvertFromInvariantString("auto"));
				grid.GetValue(Grid.ColumnDefinitionsProperty).Add(columnDefinition2);
				columnDefinition3.SetValue(ColumnDefinition.WidthProperty, new GridLengthTypeConverter().ConvertFromInvariantString("*"));
				grid.GetValue(Grid.ColumnDefinitionsProperty).Add(columnDefinition3);
				columnDefinition4.SetValue(ColumnDefinition.WidthProperty, new GridLengthTypeConverter().ConvertFromInvariantString("auto"));
				grid.GetValue(Grid.ColumnDefinitionsProperty).Add(columnDefinition4);
				image.SetValue(Grid.ColumnProperty, 0);
				bindingExtension.Path = "Image";
				BindingBase bindingBase = bindingExtension.ProvideValue(null);
				image.SetBinding(Image.SourceProperty, bindingBase);
				image.SetValue(View.VerticalOptionsProperty, LayoutOptions.Center);
				grid.Children.Add(image);
				label.SetValue(Grid.ColumnProperty, 1);
				label.SetValue(View.HorizontalOptionsProperty, LayoutOptions.Start);
				bindingExtension2.Path = "Text";
				BindingBase bindingBase2 = bindingExtension2.ProvideValue(null);
				label.SetBinding(Label.TextProperty, bindingBase2);
				dynamicResourceExtension.Key = "TextColor";
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
				xamlServiceProvider.Add(typeFromHandle, obj = new SimpleValueTargetProvider(array2, Label.TextColorProperty, nameScope));
				xamlServiceProvider.Add(typeof(IReferenceProvider), obj);
				Type typeFromHandle2 = typeof(IXamlTypeResolver);
				XmlNamespaceResolver xmlNamespaceResolver = new XmlNamespaceResolver();
				xmlNamespaceResolver.Add("", "http://xamarin.com/schemas/2014/forms");
				xmlNamespaceResolver.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
				xmlNamespaceResolver.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
				xmlNamespaceResolver.Add("local", "clr-namespace:CarScannerXamarinForms");
				xamlServiceProvider.Add(typeFromHandle2, new XamlTypeResolver(xmlNamespaceResolver, typeof(SettingsPage.<InitializeComponent>_anonXamlCDataTemplate_102).GetTypeInfo().Assembly));
				xamlServiceProvider.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(45, 33)));
				DynamicResource dynamicResource = markupExtension.ProvideValue(xamlServiceProvider);
				label.SetDynamicResource(Label.TextColorProperty, dynamicResource.Key);
				label.SetValue(View.VerticalOptionsProperty, LayoutOptions.Center);
				label.SetValue(Label.VerticalTextAlignmentProperty, new TextAlignmentConverter().ConvertFromInvariantString("Center"));
				grid.Children.Add(label);
				label2.SetValue(Grid.ColumnProperty, 3);
				label2.SetValue(Label.TextProperty, ">");
				label2.SetValue(View.VerticalOptionsProperty, LayoutOptions.Center);
				label2.SetValue(Label.VerticalTextAlignmentProperty, new TextAlignmentConverter().ConvertFromInvariantString("Center"));
				grid.Children.Add(label2);
				viewCell.View = grid;
				return viewCell;
			}

			// Token: 0x04000B22 RID: 2850
			internal object[] parentValues;

			// Token: 0x04000B23 RID: 2851
			internal SettingsPage root;
		}
	}
}
