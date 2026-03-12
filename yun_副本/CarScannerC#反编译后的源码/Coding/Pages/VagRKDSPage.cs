using System;
using System.CodeDom.Compiler;
using System.Diagnostics;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using CarScannerXamarinForms.Coding.DB.MQB;
using CarScannerXamarinForms.Common.XAMLConverters;
using CarScannerXamarinForms.InApp;
using CarScannerXamarinForms.Settings;
using Xamarin.Forms;
using Xamarin.Forms.Internals;
using Xamarin.Forms.PlatformConfiguration.iOSSpecific;
using Xamarin.Forms.Xaml;
using Xamarin.Forms.Xaml.Diagnostics;
using Xamarin.Forms.Xaml.Internals;

namespace CarScannerXamarinForms.Coding.Pages
{
	// Token: 0x02000977 RID: 2423
	[XamlCompilation(2)]
	[XamlFilePath("Coding\\Pages\\VagRKDSPage.xaml")]
	public class VagRKDSPage : ContentPage
	{
		// Token: 0x06004FCC RID: 20428 RVA: 0x003CBC4C File Offset: 0x003C9E4C
		public VagRKDSPage(ICodingContainer coding)
		{
			this.InitializeComponent();
			base.Appearing += this.VagRKDSPage_Appearing;
			this.Coding = (RKDSDatasetCoding)coding;
			this.lv.BindingContext = this.Coding;
		}

		// Token: 0x06004FCD RID: 20429 RVA: 0x003CBC9C File Offset: 0x003C9E9C
		private async void VagRKDSPage_Appearing(object sender, EventArgs e)
		{
			if (this.first_appearing)
			{
				this.first_appearing = false;
				this.lv.IsEnabled = false;
				this.activityFrame.IsVisible = true;
				await this.Coding.UpdateCurrentState("", null);
				this.activityFrame.IsVisible = false;
				this.lv.IsEnabled = true;
			}
		}

		// Token: 0x17001795 RID: 6037
		// (get) Token: 0x06004FCE RID: 20430 RVA: 0x003CBCD3 File Offset: 0x003C9ED3
		// (set) Token: 0x06004FCF RID: 20431 RVA: 0x003CBCDB File Offset: 0x003C9EDB
		internal RKDSDatasetCoding Coding
		{
			[CompilerGenerated]
			get
			{
				return this.<Coding>k__BackingField;
			}
			[CompilerGenerated]
			private set
			{
				this.<Coding>k__BackingField = value;
			}
		}

		// Token: 0x06004FD0 RID: 20432 RVA: 0x003CBCE4 File Offset: 0x003C9EE4
		private async void BtnUpdateState_Clicked(object sender, EventArgs e)
		{
			this.UpdateState();
		}

		// Token: 0x06004FD1 RID: 20433 RVA: 0x003CBD1C File Offset: 0x003C9F1C
		public async Task UpdateState()
		{
			this.activityFrame.IsVisible = true;
			await this.Coding.UpdateCurrentState("", null);
			this.activityFrame.IsVisible = false;
		}

		// Token: 0x06004FD2 RID: 20434 RVA: 0x003CBD60 File Offset: 0x003C9F60
		private async void btnSaveState_Clicked(object sender, EventArgs e)
		{
			if (this.Coding.RequiresPro && !SharedSettings.Current.AdsProductPurchased)
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
				this.lv.IsEnabled = false;
				Progress<string> progress = new Progress<string>(delegate(string s)
				{
					Device.BeginInvokeOnMainThread(delegate
					{
						this.activityFrame.Text = s;
					});
				});
				CodingRequestResult codingRequestResult = await this.Coding.Execute(this.entryPassword.Text, "", "", progress, null, false);
				if (codingRequestResult == CodingRequestResult.Success)
				{
					SharedSettings.Current.CodingsCounter++;
					await base.DisplayAlert(this.Coding.Name, Translate.GetString("coding_OperationFinished"), "OK");
				}
				else
				{
					await base.DisplayAlert(Translate.GetString("coding_Error"), MQBAdaptationTemplate.CodingRequestResultToString(codingRequestResult), "OK");
				}
				this.activityFrame.IsVisible = false;
				this.activityFrame.Text = ActivityFrame.PLEASE_WAIT_TEXT;
				this.lv.IsEnabled = true;
			}
		}

		// Token: 0x06004FD3 RID: 20435 RVA: 0x003CBD98 File Offset: 0x003C9F98
		private async void btnPickODISXml_Clicked(object sender, EventArgs e)
		{
			if (this.Coding.RequiresPro && !SharedSettings.Current.AdsProductPurchased)
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
				string text = await this.Coding.PickFileAsync();
				if (text != null)
				{
					try
					{
						string text2 = this.Coding.ODISXmlToHex(text);
						if (text2 != null)
						{
							this.activityFrame.IsVisible = true;
							this.lv.IsEnabled = false;
							Progress<string> progress = new Progress<string>(delegate(string s)
							{
								Device.BeginInvokeOnMainThread(delegate
								{
									this.activityFrame.Text = s;
								});
							});
							CodingRequestResult codingRequestResult = await this.Coding.Execute(this.entryPassword.Text, text2, "", progress, null, false);
							if (codingRequestResult == CodingRequestResult.Success)
							{
								SharedSettings.Current.CodingsCounter++;
								await base.DisplayAlert(this.Coding.Name, Translate.GetString("coding_OperationFinished"), "OK");
							}
							else
							{
								await base.DisplayAlert(Translate.GetString("coding_Error"), MQBAdaptationTemplate.CodingRequestResultToString(codingRequestResult), "OK");
							}
							this.activityFrame.IsVisible = false;
							this.activityFrame.Text = ActivityFrame.PLEASE_WAIT_TEXT;
							this.lv.IsEnabled = true;
						}
					}
					catch (Exception)
					{
					}
				}
			}
		}

		// Token: 0x06004FD4 RID: 20436 RVA: 0x003CBDD0 File Offset: 0x003C9FD0
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private void InitializeComponent()
		{
			if (ResourceLoader.IsEnabled && ResourceLoader.CanProvideContentFor(new ResourceLoader.ResourceLoadingQuery
			{
				AssemblyName = typeof(VagRKDSPage).GetTypeInfo().Assembly.GetName(),
				ResourcePath = "Coding/Pages/VagRKDSPage.xaml",
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
			VisualDiagnostics.RegisterSourceInfo(translate = new Translate(), new Uri("Coding\\Pages\\VagRKDSPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 11, 5);
			DynamicResourceExtension dynamicResourceExtension;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension = new DynamicResourceExtension(), new Uri("Coding\\Pages\\VagRKDSPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 14, 5);
			EmptyStringToFalseConverter emptyStringToFalseConverter;
			VisualDiagnostics.RegisterSourceInfo(emptyStringToFalseConverter = new EmptyStringToFalseConverter(), new Uri("Coding\\Pages\\VagRKDSPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 17, 14);
			EmptyStringToTrueConverter emptyStringToTrueConverter;
			VisualDiagnostics.RegisterSourceInfo(emptyStringToTrueConverter = new EmptyStringToTrueConverter(), new Uri("Coding\\Pages\\VagRKDSPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 18, 14);
			VagCodingPlatformToTrueConverter vagCodingPlatformToTrueConverter;
			VisualDiagnostics.RegisterSourceInfo(vagCodingPlatformToTrueConverter = new VagCodingPlatformToTrueConverter(), new Uri("Coding\\Pages\\VagRKDSPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 19, 14);
			DoubleToRKDSPressurePositionConverter doubleToRKDSPressurePositionConverter;
			VisualDiagnostics.RegisterSourceInfo(doubleToRKDSPressurePositionConverter = new DoubleToRKDSPressurePositionConverter(), new Uri("Coding\\Pages\\VagRKDSPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 20, 14);
			DoubleToStringConverter doubleToStringConverter;
			VisualDiagnostics.RegisterSourceInfo(doubleToStringConverter = new DoubleToStringConverter(), new Uri("Coding\\Pages\\VagRKDSPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 21, 14);
			IntToStringConverter intToStringConverter;
			VisualDiagnostics.RegisterSourceInfo(intToStringConverter = new IntToStringConverter(), new Uri("Coding\\Pages\\VagRKDSPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 22, 14);
			Type typeFromHandle;
			VisualDiagnostics.RegisterSourceInfo(typeFromHandle = typeof(string), new Uri("Coding\\Pages\\VagRKDSPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 23, 45);
			string text = "--";
			string text2 = "0.5";
			string text3 = "0.6";
			string text4 = "0.7";
			string text5 = "0.8";
			string text6 = "0.9";
			string text7 = "1.0";
			string text8 = "1.1";
			string text9 = "1.2";
			string text10 = "1.3";
			string text11 = "1.4";
			string text12 = "1.5";
			string text13 = "1.6";
			string text14 = "1.7";
			string text15 = "1.8";
			string text16 = "1.9";
			string text17 = "2.0";
			string text18 = "2.1";
			string text19 = "2.2";
			string text20 = "2.3";
			string text21 = "2.4";
			string text22 = "2.5";
			string text23 = "2.6";
			string text24 = "2.7";
			string text25 = "2.8";
			string text26 = "2.9";
			string text27 = "3.0";
			string text28 = "3.1";
			string text29 = "3.2";
			string text30 = "3.3";
			string text31 = "3.4";
			string text32 = "3.5";
			string text33 = "3.6";
			string text34 = "3.7";
			string text35 = "3.8";
			string text36 = "3.9";
			string text37 = "4.0";
			string text38 = "4.1";
			string text39 = "4.2";
			string text40 = "4.3";
			string text41 = "4.4";
			string text42 = "4.5";
			string text43 = "4.6";
			string text44 = "4.7";
			string text45 = "4.8";
			string text46 = "4.9";
			string text47 = "5.0";
			ArrayExtension arrayExtension;
			(arrayExtension = new ArrayExtension()).Type = typeFromHandle;
			arrayExtension.Items.Add(text);
			arrayExtension.Items.Add(text2);
			arrayExtension.Items.Add(text3);
			arrayExtension.Items.Add(text4);
			arrayExtension.Items.Add(text5);
			arrayExtension.Items.Add(text6);
			arrayExtension.Items.Add(text7);
			arrayExtension.Items.Add(text8);
			arrayExtension.Items.Add(text9);
			arrayExtension.Items.Add(text10);
			arrayExtension.Items.Add(text11);
			arrayExtension.Items.Add(text12);
			arrayExtension.Items.Add(text13);
			arrayExtension.Items.Add(text14);
			arrayExtension.Items.Add(text15);
			arrayExtension.Items.Add(text16);
			arrayExtension.Items.Add(text17);
			arrayExtension.Items.Add(text18);
			arrayExtension.Items.Add(text19);
			arrayExtension.Items.Add(text20);
			arrayExtension.Items.Add(text21);
			arrayExtension.Items.Add(text22);
			arrayExtension.Items.Add(text23);
			arrayExtension.Items.Add(text24);
			arrayExtension.Items.Add(text25);
			arrayExtension.Items.Add(text26);
			arrayExtension.Items.Add(text27);
			arrayExtension.Items.Add(text28);
			arrayExtension.Items.Add(text29);
			arrayExtension.Items.Add(text30);
			arrayExtension.Items.Add(text31);
			arrayExtension.Items.Add(text32);
			arrayExtension.Items.Add(text33);
			arrayExtension.Items.Add(text34);
			arrayExtension.Items.Add(text35);
			arrayExtension.Items.Add(text36);
			arrayExtension.Items.Add(text37);
			arrayExtension.Items.Add(text38);
			arrayExtension.Items.Add(text39);
			arrayExtension.Items.Add(text40);
			arrayExtension.Items.Add(text41);
			arrayExtension.Items.Add(text42);
			arrayExtension.Items.Add(text43);
			arrayExtension.Items.Add(text44);
			arrayExtension.Items.Add(text45);
			arrayExtension.Items.Add(text46);
			arrayExtension.Items.Add(text47);
			string[] array;
			VisualDiagnostics.RegisterSourceInfo(array = new string[]
			{
				text, text2, text3, text4, text5, text6, text7, text8, text9, text10,
				text11, text12, text13, text14, text15, text16, text17, text18, text19, text20,
				text21, text22, text23, text24, text25, text26, text27, text28, text29, text30,
				text31, text32, text33, text34, text35, text36, text37, text38, text39, text40,
				text41, text42, text43, text44, text45, text46, text47
			}, new Uri("Coding\\Pages\\VagRKDSPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 23, 14);
			string text48 = "0.0";
			ResourceDictionary resourceDictionary;
			VisualDiagnostics.RegisterSourceInfo(resourceDictionary = new ResourceDictionary(), new Uri("Coding\\Pages\\VagRKDSPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 16, 10);
			RowDefinition rowDefinition;
			VisualDiagnostics.RegisterSourceInfo(rowDefinition = new RowDefinition(), new Uri("Coding\\Pages\\VagRKDSPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 79, 18);
			BindingExtension bindingExtension;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension = new BindingExtension(), new Uri("Coding\\Pages\\VagRKDSPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 86, 17);
			DynamicResourceExtension dynamicResourceExtension2;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension2 = new DynamicResourceExtension(), new Uri("Coding\\Pages\\VagRKDSPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 91, 29);
			BindingExtension bindingExtension2;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension2 = new BindingExtension(), new Uri("Coding\\Pages\\VagRKDSPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 92, 29);
			Label label;
			VisualDiagnostics.RegisterSourceInfo(label = new Label(), new Uri("Coding\\Pages\\VagRKDSPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 89, 26);
			StaticResourceExtension staticResourceExtension;
			VisualDiagnostics.RegisterSourceInfo(staticResourceExtension = new StaticResourceExtension(), new Uri("Coding\\Pages\\VagRKDSPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 93, 32);
			BindingExtension bindingExtension3;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension3 = new BindingExtension(), new Uri("Coding\\Pages\\VagRKDSPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 93, 32);
			BindingExtension bindingExtension4;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension4 = new BindingExtension(), new Uri("Coding\\Pages\\VagRKDSPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 93, 122);
			Label label2;
			VisualDiagnostics.RegisterSourceInfo(label2 = new Label(), new Uri("Coding\\Pages\\VagRKDSPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 93, 26);
			StaticResourceExtension staticResourceExtension2;
			VisualDiagnostics.RegisterSourceInfo(staticResourceExtension2 = new StaticResourceExtension(), new Uri("Coding\\Pages\\VagRKDSPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 94, 32);
			BindingExtension bindingExtension5;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension5 = new BindingExtension(), new Uri("Coding\\Pages\\VagRKDSPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 94, 32);
			BindingExtension bindingExtension6;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension6 = new BindingExtension(), new Uri("Coding\\Pages\\VagRKDSPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 94, 127);
			Label label3;
			VisualDiagnostics.RegisterSourceInfo(label3 = new Label(), new Uri("Coding\\Pages\\VagRKDSPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 94, 26);
			BindingExtension bindingExtension7;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension7 = new BindingExtension(), new Uri("Coding\\Pages\\VagRKDSPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 95, 38);
			Translate translate2;
			VisualDiagnostics.RegisterSourceInfo(translate2 = new Translate(), new Uri("Coding\\Pages\\VagRKDSPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 96, 36);
			Label label4;
			VisualDiagnostics.RegisterSourceInfo(label4 = new Label(), new Uri("Coding\\Pages\\VagRKDSPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 96, 30);
			BindingExtension bindingExtension8;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension8 = new BindingExtension(), new Uri("Coding\\Pages\\VagRKDSPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 97, 59);
			Entry entry;
			VisualDiagnostics.RegisterSourceInfo(entry = new Entry(), new Uri("Coding\\Pages\\VagRKDSPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 97, 30);
			StaticResourceExtension staticResourceExtension3;
			VisualDiagnostics.RegisterSourceInfo(staticResourceExtension3 = new StaticResourceExtension(), new Uri("Coding\\Pages\\VagRKDSPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 98, 60);
			BindingExtension bindingExtension9;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension9 = new BindingExtension(), new Uri("Coding\\Pages\\VagRKDSPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 98, 60);
			Translate translate3;
			VisualDiagnostics.RegisterSourceInfo(translate3 = new Translate(), new Uri("Coding\\Pages\\VagRKDSPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 102, 47);
			Span span;
			VisualDiagnostics.RegisterSourceInfo(span = new Span(), new Uri("Coding\\Pages\\VagRKDSPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 102, 42);
			BindingExtension bindingExtension10;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension10 = new BindingExtension(), new Uri("Coding\\Pages\\VagRKDSPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 103, 47);
			Span span2;
			VisualDiagnostics.RegisterSourceInfo(span2 = new Span(), new Uri("Coding\\Pages\\VagRKDSPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 103, 42);
			FormattedString formattedString;
			VisualDiagnostics.RegisterSourceInfo(formattedString = new FormattedString(), new Uri("Coding\\Pages\\VagRKDSPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 101, 38);
			Label label5;
			VisualDiagnostics.RegisterSourceInfo(label5 = new Label(), new Uri("Coding\\Pages\\VagRKDSPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 98, 30);
			StaticResourceExtension staticResourceExtension4;
			VisualDiagnostics.RegisterSourceInfo(staticResourceExtension4 = new StaticResourceExtension(), new Uri("Coding\\Pages\\VagRKDSPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 109, 33);
			BindingExtension bindingExtension11;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension11 = new BindingExtension(), new Uri("Coding\\Pages\\VagRKDSPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 109, 33);
			Translate translate4;
			VisualDiagnostics.RegisterSourceInfo(translate4 = new Translate(), new Uri("Coding\\Pages\\VagRKDSPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 110, 33);
			Label label6;
			VisualDiagnostics.RegisterSourceInfo(label6 = new Label(), new Uri("Coding\\Pages\\VagRKDSPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 107, 30);
			Translate translate5;
			VisualDiagnostics.RegisterSourceInfo(translate5 = new Translate(), new Uri("Coding\\Pages\\VagRKDSPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 119, 33);
			Button button;
			VisualDiagnostics.RegisterSourceInfo(button = new Button(), new Uri("Coding\\Pages\\VagRKDSPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 116, 30);
			Translate translate6;
			VisualDiagnostics.RegisterSourceInfo(translate6 = new Translate(), new Uri("Coding\\Pages\\VagRKDSPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 123, 33);
			Button button2;
			VisualDiagnostics.RegisterSourceInfo(button2 = new Button(), new Uri("Coding\\Pages\\VagRKDSPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 120, 30);
			StackLayout stackLayout;
			VisualDiagnostics.RegisterSourceInfo(stackLayout = new StackLayout(), new Uri("Coding\\Pages\\VagRKDSPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 95, 26);
			BindingExtension bindingExtension12;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension12 = new BindingExtension(), new Uri("Coding\\Pages\\VagRKDSPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 127, 29);
			Translate translate7;
			VisualDiagnostics.RegisterSourceInfo(translate7 = new Translate(), new Uri("Coding\\Pages\\VagRKDSPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 128, 29);
			LabelSwitch labelSwitch;
			VisualDiagnostics.RegisterSourceInfo(labelSwitch = new LabelSwitch(), new Uri("Coding\\Pages\\VagRKDSPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 125, 26);
			BindingExtension bindingExtension13;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension13 = new BindingExtension(), new Uri("Coding\\Pages\\VagRKDSPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 131, 29);
			Translate translate8;
			VisualDiagnostics.RegisterSourceInfo(translate8 = new Translate(), new Uri("Coding\\Pages\\VagRKDSPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 132, 29);
			LabelSwitch labelSwitch2;
			VisualDiagnostics.RegisterSourceInfo(labelSwitch2 = new LabelSwitch(), new Uri("Coding\\Pages\\VagRKDSPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 129, 26);
			Translate translate9;
			VisualDiagnostics.RegisterSourceInfo(translate9 = new Translate(), new Uri("Coding\\Pages\\VagRKDSPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 133, 54);
			Label label7;
			VisualDiagnostics.RegisterSourceInfo(label7 = new Label(), new Uri("Coding\\Pages\\VagRKDSPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 133, 26);
			StaticResourceExtension staticResourceExtension5;
			VisualDiagnostics.RegisterSourceInfo(staticResourceExtension5 = new StaticResourceExtension(), new Uri("Coding\\Pages\\VagRKDSPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 134, 33);
			BindingExtension bindingExtension14;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension14 = new BindingExtension(), new Uri("Coding\\Pages\\VagRKDSPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 134, 33);
			string text49 = "1";
			string text50 = "2";
			string text51 = "3";
			string text52 = "4";
			string text53 = "5";
			string text54 = "6";
			string text55 = "7";
			string text56 = "8";
			string text57 = "9";
			string text58 = "10";
			Picker picker;
			VisualDiagnostics.RegisterSourceInfo(picker = new Picker(), new Uri("Coding\\Pages\\VagRKDSPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 134, 26);
			StackLayout stackLayout2;
			VisualDiagnostics.RegisterSourceInfo(stackLayout2 = new StackLayout(), new Uri("Coding\\Pages\\VagRKDSPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 88, 22);
			DataTemplate dataTemplate;
			VisualDiagnostics.RegisterSourceInfo(dataTemplate = new DataTemplate(), new Uri("Coding\\Pages\\VagRKDSPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 153, 22);
			ReferenceExtension referenceExtension;
			VisualDiagnostics.RegisterSourceInfo(referenceExtension = new ReferenceExtension(), new Uri("Coding\\Pages\\VagRKDSPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 258, 34);
			BindingExtension bindingExtension15;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension15 = new BindingExtension(), new Uri("Coding\\Pages\\VagRKDSPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 258, 34);
			Label label8;
			VisualDiagnostics.RegisterSourceInfo(label8 = new Label(), new Uri("Coding\\Pages\\VagRKDSPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 259, 26);
			BindingExtension bindingExtension16;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension16 = new BindingExtension(), new Uri("Coding\\Pages\\VagRKDSPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 260, 31);
			ColumnDefinition columnDefinition;
			VisualDiagnostics.RegisterSourceInfo(columnDefinition = new ColumnDefinition(), new Uri("Coding\\Pages\\VagRKDSPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 262, 34);
			ColumnDefinition columnDefinition2;
			VisualDiagnostics.RegisterSourceInfo(columnDefinition2 = new ColumnDefinition(), new Uri("Coding\\Pages\\VagRKDSPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 263, 34);
			ColumnDefinition columnDefinition3;
			VisualDiagnostics.RegisterSourceInfo(columnDefinition3 = new ColumnDefinition(), new Uri("Coding\\Pages\\VagRKDSPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 264, 34);
			RowDefinition rowDefinition2;
			VisualDiagnostics.RegisterSourceInfo(rowDefinition2 = new RowDefinition(), new Uri("Coding\\Pages\\VagRKDSPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 267, 34);
			RowDefinition rowDefinition3;
			VisualDiagnostics.RegisterSourceInfo(rowDefinition3 = new RowDefinition(), new Uri("Coding\\Pages\\VagRKDSPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 268, 34);
			RowDefinition rowDefinition4;
			VisualDiagnostics.RegisterSourceInfo(rowDefinition4 = new RowDefinition(), new Uri("Coding\\Pages\\VagRKDSPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 269, 34);
			RowDefinition rowDefinition5;
			VisualDiagnostics.RegisterSourceInfo(rowDefinition5 = new RowDefinition(), new Uri("Coding\\Pages\\VagRKDSPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 270, 34);
			RowDefinition rowDefinition6;
			VisualDiagnostics.RegisterSourceInfo(rowDefinition6 = new RowDefinition(), new Uri("Coding\\Pages\\VagRKDSPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 271, 34);
			Translate translate10;
			VisualDiagnostics.RegisterSourceInfo(translate10 = new Translate(), new Uri("Coding\\Pages\\VagRKDSPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 277, 33);
			Label label9;
			VisualDiagnostics.RegisterSourceInfo(label9 = new Label(), new Uri("Coding\\Pages\\VagRKDSPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 274, 30);
			BindingExtension bindingExtension17;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension17 = new BindingExtension(), new Uri("Coding\\Pages\\VagRKDSPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 283, 33);
			Entry entry2;
			VisualDiagnostics.RegisterSourceInfo(entry2 = new Entry(), new Uri("Coding\\Pages\\VagRKDSPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 279, 30);
			Translate translate11;
			VisualDiagnostics.RegisterSourceInfo(translate11 = new Translate(), new Uri("Coding\\Pages\\VagRKDSPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 289, 33);
			Label label10;
			VisualDiagnostics.RegisterSourceInfo(label10 = new Label(), new Uri("Coding\\Pages\\VagRKDSPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 286, 30);
			Translate translate12;
			VisualDiagnostics.RegisterSourceInfo(translate12 = new Translate(), new Uri("Coding\\Pages\\VagRKDSPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 294, 33);
			Label label11;
			VisualDiagnostics.RegisterSourceInfo(label11 = new Label(), new Uri("Coding\\Pages\\VagRKDSPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 291, 30);
			Translate translate13;
			VisualDiagnostics.RegisterSourceInfo(translate13 = new Translate(), new Uri("Coding\\Pages\\VagRKDSPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 299, 33);
			Label label12;
			VisualDiagnostics.RegisterSourceInfo(label12 = new Label(), new Uri("Coding\\Pages\\VagRKDSPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 296, 30);
			Translate translate14;
			VisualDiagnostics.RegisterSourceInfo(translate14 = new Translate(), new Uri("Coding\\Pages\\VagRKDSPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 304, 33);
			Label label13;
			VisualDiagnostics.RegisterSourceInfo(label13 = new Label(), new Uri("Coding\\Pages\\VagRKDSPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 301, 30);
			StaticResourceExtension staticResourceExtension6;
			VisualDiagnostics.RegisterSourceInfo(staticResourceExtension6 = new StaticResourceExtension(), new Uri("Coding\\Pages\\VagRKDSPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 309, 33);
			StaticResourceExtension staticResourceExtension7;
			VisualDiagnostics.RegisterSourceInfo(staticResourceExtension7 = new StaticResourceExtension(), new Uri("Coding\\Pages\\VagRKDSPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 310, 33);
			StaticResourceExtension staticResourceExtension8;
			VisualDiagnostics.RegisterSourceInfo(staticResourceExtension8 = new StaticResourceExtension(), new Uri("Coding\\Pages\\VagRKDSPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 310, 33);
			BindingExtension bindingExtension18;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension18 = new BindingExtension(), new Uri("Coding\\Pages\\VagRKDSPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 310, 33);
			Picker picker2;
			VisualDiagnostics.RegisterSourceInfo(picker2 = new Picker(), new Uri("Coding\\Pages\\VagRKDSPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 306, 30);
			StaticResourceExtension staticResourceExtension9;
			VisualDiagnostics.RegisterSourceInfo(staticResourceExtension9 = new StaticResourceExtension(), new Uri("Coding\\Pages\\VagRKDSPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 315, 33);
			StaticResourceExtension staticResourceExtension10;
			VisualDiagnostics.RegisterSourceInfo(staticResourceExtension10 = new StaticResourceExtension(), new Uri("Coding\\Pages\\VagRKDSPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 316, 33);
			StaticResourceExtension staticResourceExtension11;
			VisualDiagnostics.RegisterSourceInfo(staticResourceExtension11 = new StaticResourceExtension(), new Uri("Coding\\Pages\\VagRKDSPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 316, 33);
			BindingExtension bindingExtension19;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension19 = new BindingExtension(), new Uri("Coding\\Pages\\VagRKDSPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 316, 33);
			Picker picker3;
			VisualDiagnostics.RegisterSourceInfo(picker3 = new Picker(), new Uri("Coding\\Pages\\VagRKDSPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 312, 30);
			Translate translate15;
			VisualDiagnostics.RegisterSourceInfo(translate15 = new Translate(), new Uri("Coding\\Pages\\VagRKDSPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 321, 33);
			Label label14;
			VisualDiagnostics.RegisterSourceInfo(label14 = new Label(), new Uri("Coding\\Pages\\VagRKDSPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 318, 30);
			StaticResourceExtension staticResourceExtension12;
			VisualDiagnostics.RegisterSourceInfo(staticResourceExtension12 = new StaticResourceExtension(), new Uri("Coding\\Pages\\VagRKDSPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 326, 33);
			StaticResourceExtension staticResourceExtension13;
			VisualDiagnostics.RegisterSourceInfo(staticResourceExtension13 = new StaticResourceExtension(), new Uri("Coding\\Pages\\VagRKDSPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 327, 33);
			StaticResourceExtension staticResourceExtension14;
			VisualDiagnostics.RegisterSourceInfo(staticResourceExtension14 = new StaticResourceExtension(), new Uri("Coding\\Pages\\VagRKDSPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 327, 33);
			BindingExtension bindingExtension20;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension20 = new BindingExtension(), new Uri("Coding\\Pages\\VagRKDSPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 327, 33);
			Picker picker4;
			VisualDiagnostics.RegisterSourceInfo(picker4 = new Picker(), new Uri("Coding\\Pages\\VagRKDSPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 323, 30);
			StaticResourceExtension staticResourceExtension15;
			VisualDiagnostics.RegisterSourceInfo(staticResourceExtension15 = new StaticResourceExtension(), new Uri("Coding\\Pages\\VagRKDSPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 332, 33);
			StaticResourceExtension staticResourceExtension16;
			VisualDiagnostics.RegisterSourceInfo(staticResourceExtension16 = new StaticResourceExtension(), new Uri("Coding\\Pages\\VagRKDSPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 333, 33);
			StaticResourceExtension staticResourceExtension17;
			VisualDiagnostics.RegisterSourceInfo(staticResourceExtension17 = new StaticResourceExtension(), new Uri("Coding\\Pages\\VagRKDSPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 333, 33);
			BindingExtension bindingExtension21;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension21 = new BindingExtension(), new Uri("Coding\\Pages\\VagRKDSPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 333, 33);
			Picker picker5;
			VisualDiagnostics.RegisterSourceInfo(picker5 = new Picker(), new Uri("Coding\\Pages\\VagRKDSPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 329, 30);
			Translate translate16;
			VisualDiagnostics.RegisterSourceInfo(translate16 = new Translate(), new Uri("Coding\\Pages\\VagRKDSPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 339, 33);
			Label label15;
			VisualDiagnostics.RegisterSourceInfo(label15 = new Label(), new Uri("Coding\\Pages\\VagRKDSPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 336, 30);
			StaticResourceExtension staticResourceExtension18;
			VisualDiagnostics.RegisterSourceInfo(staticResourceExtension18 = new StaticResourceExtension(), new Uri("Coding\\Pages\\VagRKDSPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 344, 33);
			StaticResourceExtension staticResourceExtension19;
			VisualDiagnostics.RegisterSourceInfo(staticResourceExtension19 = new StaticResourceExtension(), new Uri("Coding\\Pages\\VagRKDSPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 345, 33);
			StaticResourceExtension staticResourceExtension20;
			VisualDiagnostics.RegisterSourceInfo(staticResourceExtension20 = new StaticResourceExtension(), new Uri("Coding\\Pages\\VagRKDSPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 345, 33);
			BindingExtension bindingExtension22;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension22 = new BindingExtension(), new Uri("Coding\\Pages\\VagRKDSPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 345, 33);
			Picker picker6;
			VisualDiagnostics.RegisterSourceInfo(picker6 = new Picker(), new Uri("Coding\\Pages\\VagRKDSPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 341, 30);
			StaticResourceExtension staticResourceExtension21;
			VisualDiagnostics.RegisterSourceInfo(staticResourceExtension21 = new StaticResourceExtension(), new Uri("Coding\\Pages\\VagRKDSPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 350, 33);
			StaticResourceExtension staticResourceExtension22;
			VisualDiagnostics.RegisterSourceInfo(staticResourceExtension22 = new StaticResourceExtension(), new Uri("Coding\\Pages\\VagRKDSPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 351, 33);
			StaticResourceExtension staticResourceExtension23;
			VisualDiagnostics.RegisterSourceInfo(staticResourceExtension23 = new StaticResourceExtension(), new Uri("Coding\\Pages\\VagRKDSPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 351, 33);
			BindingExtension bindingExtension23;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension23 = new BindingExtension(), new Uri("Coding\\Pages\\VagRKDSPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 351, 33);
			Picker picker7;
			VisualDiagnostics.RegisterSourceInfo(picker7 = new Picker(), new Uri("Coding\\Pages\\VagRKDSPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 347, 30);
			Grid grid;
			VisualDiagnostics.RegisterSourceInfo(grid = new Grid(), new Uri("Coding\\Pages\\VagRKDSPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 260, 26);
			StackLayout stackLayout3;
			VisualDiagnostics.RegisterSourceInfo(stackLayout3 = new StackLayout(), new Uri("Coding\\Pages\\VagRKDSPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 258, 22);
			ListView listView;
			VisualDiagnostics.RegisterSourceInfo(listView = new ListView(), new Uri("Coding\\Pages\\VagRKDSPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 82, 14);
			ActivityFrame activityFrame;
			VisualDiagnostics.RegisterSourceInfo(activityFrame = new ActivityFrame(), new Uri("Coding\\Pages\\VagRKDSPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 360, 14);
			Grid grid2;
			VisualDiagnostics.RegisterSourceInfo(grid2 = new Grid(), new Uri("Coding\\Pages\\VagRKDSPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 77, 10);
			VisualDiagnostics.RegisterSourceInfo(this, new Uri("Coding\\Pages\\VagRKDSPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 2, 2);
			NameScope nameScope = NameScope.GetNameScope(this) ?? new NameScope();
			NameScope.SetNameScope(this, nameScope);
			nameScope.RegisterName("lv", listView);
			if (listView.StyleId == null)
			{
				listView.StyleId = "lv";
			}
			nameScope.RegisterName("entryPassword", entry);
			if (entry.StyleId == null)
			{
				entry.StyleId = "entryPassword";
			}
			nameScope.RegisterName("btnSaveNewValue", button);
			if (button.StyleId == null)
			{
				button.StyleId = "btnSaveNewValue";
			}
			nameScope.RegisterName("btnPickODIS", button2);
			if (button2.StyleId == null)
			{
				button2.StyleId = "btnPickODIS";
			}
			nameScope.RegisterName("switchIndividual", labelSwitch);
			if (labelSwitch.StyleId == null)
			{
				labelSwitch.StyleId = "switchIndividual";
			}
			nameScope.RegisterName("switchComfort", labelSwitch2);
			if (labelSwitch2.StyleId == null)
			{
				labelSwitch2.StyleId = "switchComfort";
			}
			nameScope.RegisterName("activityFrame", activityFrame);
			if (activityFrame.StyleId == null)
			{
				activityFrame.StyleId = "activityFrame";
			}
			this.lv = listView;
			this.entryPassword = entry;
			this.btnSaveNewValue = button;
			this.btnPickODIS = button2;
			this.switchIndividual = labelSwitch;
			this.switchComfort = labelSwitch2;
			this.activityFrame = activityFrame;
			this.Resources = resourceDictionary;
			resourceDictionary.Add("EmptyStringToFalseConverter", emptyStringToFalseConverter);
			resourceDictionary.Add("EmptyStringToTrueConverter", emptyStringToTrueConverter);
			resourceDictionary.Add("VagCodingPlatformToTrueConverter", vagCodingPlatformToTrueConverter);
			resourceDictionary.Add("DoubleToRKDSPressurePositionConverter", doubleToRKDSPressurePositionConverter);
			resourceDictionary.Add("DoubleToStringConverter", doubleToStringConverter);
			resourceDictionary.Add("IntToStringConverter", intToStringConverter);
			resourceDictionary.Add("pressureValues", array);
			resourceDictionary.Add("doubleFormat", text48);
			translate.Text = "coding_Coding";
			IMarkupExtension markupExtension = translate;
			XamlServiceProvider xamlServiceProvider = new XamlServiceProvider();
			Type typeFromHandle2 = typeof(IProvideValueTarget);
			object[] array2 = new object[0 + 1];
			array2[0] = this;
			object obj;
			xamlServiceProvider.Add(typeFromHandle2, obj = new SimpleValueTargetProvider(array2, Page.TitleProperty, nameScope));
			xamlServiceProvider.Add(typeof(IReferenceProvider), obj);
			Type typeFromHandle3 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver = new XmlNamespaceResolver();
			xmlNamespaceResolver.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver.Add("settings", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider.Add(typeFromHandle3, new XamlTypeResolver(xmlNamespaceResolver, typeof(VagRKDSPage).GetTypeInfo().Assembly));
			xamlServiceProvider.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(11, 5)));
			object obj2 = markupExtension.ProvideValue(xamlServiceProvider);
			this.Title = obj2;
			this.SetValue(Page.PaddingProperty, new Thickness(5.0, 0.0));
			this.SetValue(Page.UseSafeAreaProperty, true);
			dynamicResourceExtension.Key = "BackgroundColor";
			IMarkupExtension<DynamicResource> markupExtension2 = dynamicResourceExtension;
			XamlServiceProvider xamlServiceProvider2 = new XamlServiceProvider();
			Type typeFromHandle4 = typeof(IProvideValueTarget);
			object[] array3 = new object[0 + 1];
			array3[0] = this;
			object obj3;
			xamlServiceProvider2.Add(typeFromHandle4, obj3 = new SimpleValueTargetProvider(array3, VisualElement.BackgroundColorProperty, nameScope));
			xamlServiceProvider2.Add(typeof(IReferenceProvider), obj3);
			Type typeFromHandle5 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver2 = new XmlNamespaceResolver();
			xmlNamespaceResolver2.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver2.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver2.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver2.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver2.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver2.Add("settings", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver2.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider2.Add(typeFromHandle5, new XamlTypeResolver(xmlNamespaceResolver2, typeof(VagRKDSPage).GetTypeInfo().Assembly));
			xamlServiceProvider2.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(14, 5)));
			DynamicResource dynamicResource = markupExtension2.ProvideValue(xamlServiceProvider2);
			this.SetDynamicResource(VisualElement.BackgroundColorProperty, dynamicResource.Key);
			this.Resources = resourceDictionary;
			rowDefinition.SetValue(RowDefinition.HeightProperty, new GridLengthTypeConverter().ConvertFromInvariantString("*"));
			grid2.GetValue(Grid.RowDefinitionsProperty).Add(rowDefinition);
			listView.SetValue(Grid.RowProperty, 0);
			listView.SetValue(ListView.HasUnevenRowsProperty, true);
			bindingExtension.Path = "Model.Tiresets";
			BindingBase bindingBase = bindingExtension.ProvideValue(null);
			listView.SetBinding(ItemsView<Cell>.ItemsSourceProperty, bindingBase);
			stackLayout2.SetValue(StackLayout.OrientationProperty, 0);
			label.SetValue(Label.FontAttributesProperty, new FontAttributesConverter().ConvertFromInvariantString("Bold"));
			dynamicResourceExtension2.Key = "BaseFontSize+++";
			IMarkupExtension<DynamicResource> markupExtension3 = dynamicResourceExtension2;
			XamlServiceProvider xamlServiceProvider3 = new XamlServiceProvider();
			Type typeFromHandle6 = typeof(IProvideValueTarget);
			object[] array4 = new object[0 + 5];
			array4[0] = label;
			array4[1] = stackLayout2;
			array4[2] = listView;
			array4[3] = grid2;
			array4[4] = this;
			object obj4;
			xamlServiceProvider3.Add(typeFromHandle6, obj4 = new SimpleValueTargetProvider(array4, Label.FontSizeProperty, nameScope));
			xamlServiceProvider3.Add(typeof(IReferenceProvider), obj4);
			Type typeFromHandle7 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver3 = new XmlNamespaceResolver();
			xmlNamespaceResolver3.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver3.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver3.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver3.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver3.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver3.Add("settings", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver3.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider3.Add(typeFromHandle7, new XamlTypeResolver(xmlNamespaceResolver3, typeof(VagRKDSPage).GetTypeInfo().Assembly));
			xamlServiceProvider3.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(91, 29)));
			DynamicResource dynamicResource2 = markupExtension3.ProvideValue(xamlServiceProvider3);
			label.SetDynamicResource(Label.FontSizeProperty, dynamicResource2.Key);
			bindingExtension2.Path = "Name";
			BindingBase bindingBase2 = bindingExtension2.ProvideValue(null);
			label.SetBinding(Label.TextProperty, bindingBase2);
			stackLayout2.Children.Add(label);
			staticResourceExtension.Key = "EmptyStringToFalseConverter";
			IMarkupExtension markupExtension4 = staticResourceExtension;
			XamlServiceProvider xamlServiceProvider4 = new XamlServiceProvider();
			Type typeFromHandle8 = typeof(IProvideValueTarget);
			object[] array5 = new object[0 + 6];
			array5[0] = bindingExtension3;
			array5[1] = label2;
			array5[2] = stackLayout2;
			array5[3] = listView;
			array5[4] = grid2;
			array5[5] = this;
			object obj5;
			xamlServiceProvider4.Add(typeFromHandle8, obj5 = new SimpleValueTargetProvider(array5, typeof(BindingExtension).GetRuntimeProperty("Converter"), nameScope));
			xamlServiceProvider4.Add(typeof(IReferenceProvider), obj5);
			Type typeFromHandle9 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver4 = new XmlNamespaceResolver();
			xmlNamespaceResolver4.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver4.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver4.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver4.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver4.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver4.Add("settings", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver4.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider4.Add(typeFromHandle9, new XamlTypeResolver(xmlNamespaceResolver4, typeof(VagRKDSPage).GetTypeInfo().Assembly));
			xamlServiceProvider4.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(93, 32)));
			object obj6 = markupExtension4.ProvideValue(xamlServiceProvider4);
			bindingExtension3.Converter = obj6;
			bindingExtension3.Path = "Description";
			BindingBase bindingBase3 = bindingExtension3.ProvideValue(null);
			label2.SetBinding(VisualElement.IsVisibleProperty, bindingBase3);
			bindingExtension4.Path = "Description";
			BindingBase bindingBase4 = bindingExtension4.ProvideValue(null);
			label2.SetBinding(Label.TextProperty, bindingBase4);
			stackLayout2.Children.Add(label2);
			staticResourceExtension2.Key = "EmptyStringToFalseConverter";
			IMarkupExtension markupExtension5 = staticResourceExtension2;
			XamlServiceProvider xamlServiceProvider5 = new XamlServiceProvider();
			Type typeFromHandle10 = typeof(IProvideValueTarget);
			object[] array6 = new object[0 + 6];
			array6[0] = bindingExtension5;
			array6[1] = label3;
			array6[2] = stackLayout2;
			array6[3] = listView;
			array6[4] = grid2;
			array6[5] = this;
			object obj7;
			xamlServiceProvider5.Add(typeFromHandle10, obj7 = new SimpleValueTargetProvider(array6, typeof(BindingExtension).GetRuntimeProperty("Converter"), nameScope));
			xamlServiceProvider5.Add(typeof(IReferenceProvider), obj7);
			Type typeFromHandle11 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver5 = new XmlNamespaceResolver();
			xmlNamespaceResolver5.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver5.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver5.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver5.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver5.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver5.Add("settings", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver5.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider5.Add(typeFromHandle11, new XamlTypeResolver(xmlNamespaceResolver5, typeof(VagRKDSPage).GetTypeInfo().Assembly));
			xamlServiceProvider5.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(94, 32)));
			object obj8 = markupExtension5.ProvideValue(xamlServiceProvider5);
			bindingExtension5.Converter = obj8;
			bindingExtension5.Path = "InnerDescription";
			BindingBase bindingBase5 = bindingExtension5.ProvideValue(null);
			label3.SetBinding(VisualElement.IsVisibleProperty, bindingBase5);
			bindingExtension6.Path = "InnerDescription";
			BindingBase bindingBase6 = bindingExtension6.ProvideValue(null);
			label3.SetBinding(Label.TextProperty, bindingBase6);
			stackLayout2.Children.Add(label3);
			bindingExtension7.Path = "PasswordVisible";
			BindingBase bindingBase7 = bindingExtension7.ProvideValue(null);
			stackLayout.SetBinding(VisualElement.IsVisibleProperty, bindingBase7);
			stackLayout.SetValue(StackLayout.OrientationProperty, 0);
			translate2.Text = "coding_Password";
			IMarkupExtension markupExtension6 = translate2;
			XamlServiceProvider xamlServiceProvider6 = new XamlServiceProvider();
			Type typeFromHandle12 = typeof(IProvideValueTarget);
			object[] array7 = new object[0 + 6];
			array7[0] = label4;
			array7[1] = stackLayout;
			array7[2] = stackLayout2;
			array7[3] = listView;
			array7[4] = grid2;
			array7[5] = this;
			object obj9;
			xamlServiceProvider6.Add(typeFromHandle12, obj9 = new SimpleValueTargetProvider(array7, Label.TextProperty, nameScope));
			xamlServiceProvider6.Add(typeof(IReferenceProvider), obj9);
			Type typeFromHandle13 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver6 = new XmlNamespaceResolver();
			xmlNamespaceResolver6.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver6.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver6.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver6.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver6.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver6.Add("settings", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver6.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider6.Add(typeFromHandle13, new XamlTypeResolver(xmlNamespaceResolver6, typeof(VagRKDSPage).GetTypeInfo().Assembly));
			xamlServiceProvider6.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(96, 36)));
			object obj10 = markupExtension6.ProvideValue(xamlServiceProvider6);
			label4.Text = obj10;
			stackLayout.Children.Add(label4);
			bindingExtension8.Mode = 1;
			bindingExtension8.Path = "Password";
			BindingBase bindingBase8 = bindingExtension8.ProvideValue(null);
			entry.SetBinding(Entry.TextProperty, bindingBase8);
			stackLayout.Children.Add(entry);
			label5.SetValue(Label.FontAttributesProperty, new FontAttributesConverter().ConvertFromInvariantString("Italic"));
			staticResourceExtension3.Key = "EmptyStringToFalseConverter";
			IMarkupExtension markupExtension7 = staticResourceExtension3;
			XamlServiceProvider xamlServiceProvider7 = new XamlServiceProvider();
			Type typeFromHandle14 = typeof(IProvideValueTarget);
			object[] array8 = new object[0 + 7];
			array8[0] = bindingExtension9;
			array8[1] = label5;
			array8[2] = stackLayout;
			array8[3] = stackLayout2;
			array8[4] = listView;
			array8[5] = grid2;
			array8[6] = this;
			object obj11;
			xamlServiceProvider7.Add(typeFromHandle14, obj11 = new SimpleValueTargetProvider(array8, typeof(BindingExtension).GetRuntimeProperty("Converter"), nameScope));
			xamlServiceProvider7.Add(typeof(IReferenceProvider), obj11);
			Type typeFromHandle15 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver7 = new XmlNamespaceResolver();
			xmlNamespaceResolver7.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver7.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver7.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver7.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver7.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver7.Add("settings", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver7.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider7.Add(typeFromHandle15, new XamlTypeResolver(xmlNamespaceResolver7, typeof(VagRKDSPage).GetTypeInfo().Assembly));
			xamlServiceProvider7.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(98, 60)));
			object obj12 = markupExtension7.ProvideValue(xamlServiceProvider7);
			bindingExtension9.Converter = obj12;
			bindingExtension9.Path = "PasswordHint";
			BindingBase bindingBase9 = bindingExtension9.ProvideValue(null);
			label5.SetBinding(VisualElement.IsVisibleProperty, bindingBase9);
			translate3.Text = "coding_PasswordHint";
			IMarkupExtension markupExtension8 = translate3;
			XamlServiceProvider xamlServiceProvider8 = new XamlServiceProvider();
			Type typeFromHandle16 = typeof(IProvideValueTarget);
			object[] array9 = new object[0 + 8];
			array9[0] = span;
			array9[1] = formattedString;
			array9[2] = label5;
			array9[3] = stackLayout;
			array9[4] = stackLayout2;
			array9[5] = listView;
			array9[6] = grid2;
			array9[7] = this;
			object obj13;
			xamlServiceProvider8.Add(typeFromHandle16, obj13 = new SimpleValueTargetProvider(array9, Span.TextProperty, nameScope));
			xamlServiceProvider8.Add(typeof(IReferenceProvider), obj13);
			Type typeFromHandle17 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver8 = new XmlNamespaceResolver();
			xmlNamespaceResolver8.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver8.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver8.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver8.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver8.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver8.Add("settings", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver8.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider8.Add(typeFromHandle17, new XamlTypeResolver(xmlNamespaceResolver8, typeof(VagRKDSPage).GetTypeInfo().Assembly));
			xamlServiceProvider8.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(102, 47)));
			object obj14 = markupExtension8.ProvideValue(xamlServiceProvider8);
			span.Text = obj14;
			formattedString.Spans.Add(span);
			bindingExtension10.Path = "PasswordHint";
			BindingBase bindingBase10 = bindingExtension10.ProvideValue(null);
			span2.SetBinding(Span.TextProperty, bindingBase10);
			formattedString.Spans.Add(span2);
			label5.SetValue(Label.FormattedTextProperty, formattedString);
			stackLayout.Children.Add(label5);
			label6.SetValue(Label.FontAttributesProperty, new FontAttributesConverter().ConvertFromInvariantString("Italic"));
			bindingExtension11.Mode = 2;
			staticResourceExtension4.Key = "EmptyStringToTrueConverter";
			IMarkupExtension markupExtension9 = staticResourceExtension4;
			XamlServiceProvider xamlServiceProvider9 = new XamlServiceProvider();
			Type typeFromHandle18 = typeof(IProvideValueTarget);
			object[] array10 = new object[0 + 7];
			array10[0] = bindingExtension11;
			array10[1] = label6;
			array10[2] = stackLayout;
			array10[3] = stackLayout2;
			array10[4] = listView;
			array10[5] = grid2;
			array10[6] = this;
			object obj15;
			xamlServiceProvider9.Add(typeFromHandle18, obj15 = new SimpleValueTargetProvider(array10, typeof(BindingExtension).GetRuntimeProperty("Converter"), nameScope));
			xamlServiceProvider9.Add(typeof(IReferenceProvider), obj15);
			Type typeFromHandle19 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver9 = new XmlNamespaceResolver();
			xmlNamespaceResolver9.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver9.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver9.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver9.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver9.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver9.Add("settings", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver9.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider9.Add(typeFromHandle19, new XamlTypeResolver(xmlNamespaceResolver9, typeof(VagRKDSPage).GetTypeInfo().Assembly));
			xamlServiceProvider9.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(109, 33)));
			object obj16 = markupExtension9.ProvideValue(xamlServiceProvider9);
			bindingExtension11.Converter = obj16;
			bindingExtension11.Path = "Password";
			BindingBase bindingBase11 = bindingExtension11.ProvideValue(null);
			label6.SetBinding(VisualElement.IsVisibleProperty, bindingBase11);
			translate4.Text = "coding_PasswordNotRequired";
			IMarkupExtension markupExtension10 = translate4;
			XamlServiceProvider xamlServiceProvider10 = new XamlServiceProvider();
			Type typeFromHandle20 = typeof(IProvideValueTarget);
			object[] array11 = new object[0 + 6];
			array11[0] = label6;
			array11[1] = stackLayout;
			array11[2] = stackLayout2;
			array11[3] = listView;
			array11[4] = grid2;
			array11[5] = this;
			object obj17;
			xamlServiceProvider10.Add(typeFromHandle20, obj17 = new SimpleValueTargetProvider(array11, Label.TextProperty, nameScope));
			xamlServiceProvider10.Add(typeof(IReferenceProvider), obj17);
			Type typeFromHandle21 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver10 = new XmlNamespaceResolver();
			xmlNamespaceResolver10.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver10.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver10.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver10.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver10.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver10.Add("settings", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver10.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider10.Add(typeFromHandle21, new XamlTypeResolver(xmlNamespaceResolver10, typeof(VagRKDSPage).GetTypeInfo().Assembly));
			xamlServiceProvider10.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(110, 33)));
			object obj18 = markupExtension10.ProvideValue(xamlServiceProvider10);
			label6.Text = obj18;
			stackLayout.Children.Add(label6);
			button.Clicked += this.btnSaveState_Clicked;
			translate5.Text = "coding_Apply";
			IMarkupExtension markupExtension11 = translate5;
			XamlServiceProvider xamlServiceProvider11 = new XamlServiceProvider();
			Type typeFromHandle22 = typeof(IProvideValueTarget);
			object[] array12 = new object[0 + 6];
			array12[0] = button;
			array12[1] = stackLayout;
			array12[2] = stackLayout2;
			array12[3] = listView;
			array12[4] = grid2;
			array12[5] = this;
			object obj19;
			xamlServiceProvider11.Add(typeFromHandle22, obj19 = new SimpleValueTargetProvider(array12, Button.TextProperty, nameScope));
			xamlServiceProvider11.Add(typeof(IReferenceProvider), obj19);
			Type typeFromHandle23 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver11 = new XmlNamespaceResolver();
			xmlNamespaceResolver11.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver11.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver11.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver11.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver11.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver11.Add("settings", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver11.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider11.Add(typeFromHandle23, new XamlTypeResolver(xmlNamespaceResolver11, typeof(VagRKDSPage).GetTypeInfo().Assembly));
			xamlServiceProvider11.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(119, 33)));
			object obj20 = markupExtension11.ProvideValue(xamlServiceProvider11);
			button.Text = obj20;
			stackLayout.Children.Add(button);
			button2.Clicked += this.btnPickODISXml_Clicked;
			translate6.Text = "codingDB_VagRKDS_WriteODISXml";
			IMarkupExtension markupExtension12 = translate6;
			XamlServiceProvider xamlServiceProvider12 = new XamlServiceProvider();
			Type typeFromHandle24 = typeof(IProvideValueTarget);
			object[] array13 = new object[0 + 6];
			array13[0] = button2;
			array13[1] = stackLayout;
			array13[2] = stackLayout2;
			array13[3] = listView;
			array13[4] = grid2;
			array13[5] = this;
			object obj21;
			xamlServiceProvider12.Add(typeFromHandle24, obj21 = new SimpleValueTargetProvider(array13, Button.TextProperty, nameScope));
			xamlServiceProvider12.Add(typeof(IReferenceProvider), obj21);
			Type typeFromHandle25 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver12 = new XmlNamespaceResolver();
			xmlNamespaceResolver12.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver12.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver12.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver12.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver12.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver12.Add("settings", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver12.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider12.Add(typeFromHandle25, new XamlTypeResolver(xmlNamespaceResolver12, typeof(VagRKDSPage).GetTypeInfo().Assembly));
			xamlServiceProvider12.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(123, 33)));
			object obj22 = markupExtension12.ProvideValue(xamlServiceProvider12);
			button2.Text = obj22;
			stackLayout.Children.Add(button2);
			stackLayout2.Children.Add(stackLayout);
			bindingExtension12.Path = "Model.IsIndividualVisible";
			BindingBase bindingBase12 = bindingExtension12.ProvideValue(null);
			labelSwitch.SetBinding(LabelSwitch.IsToggledProperty, bindingBase12);
			translate7.Text = "codingDB_VagRKDS_HasIndividualProfile";
			IMarkupExtension markupExtension13 = translate7;
			XamlServiceProvider xamlServiceProvider13 = new XamlServiceProvider();
			Type typeFromHandle26 = typeof(IProvideValueTarget);
			object[] array14 = new object[0 + 5];
			array14[0] = labelSwitch;
			array14[1] = stackLayout2;
			array14[2] = listView;
			array14[3] = grid2;
			array14[4] = this;
			object obj23;
			xamlServiceProvider13.Add(typeFromHandle26, obj23 = new SimpleValueTargetProvider(array14, LabelSwitch.TextProperty, nameScope));
			xamlServiceProvider13.Add(typeof(IReferenceProvider), obj23);
			Type typeFromHandle27 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver13 = new XmlNamespaceResolver();
			xmlNamespaceResolver13.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver13.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver13.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver13.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver13.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver13.Add("settings", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver13.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider13.Add(typeFromHandle27, new XamlTypeResolver(xmlNamespaceResolver13, typeof(VagRKDSPage).GetTypeInfo().Assembly));
			xamlServiceProvider13.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(128, 29)));
			object obj24 = markupExtension13.ProvideValue(xamlServiceProvider13);
			labelSwitch.Text = obj24;
			stackLayout2.Children.Add(labelSwitch);
			bindingExtension13.Path = "Model.IsComfortVisible";
			BindingBase bindingBase13 = bindingExtension13.ProvideValue(null);
			labelSwitch2.SetBinding(LabelSwitch.IsToggledProperty, bindingBase13);
			translate8.Text = "codingDB_VagRKDS_HasComfortSet";
			IMarkupExtension markupExtension14 = translate8;
			XamlServiceProvider xamlServiceProvider14 = new XamlServiceProvider();
			Type typeFromHandle28 = typeof(IProvideValueTarget);
			object[] array15 = new object[0 + 5];
			array15[0] = labelSwitch2;
			array15[1] = stackLayout2;
			array15[2] = listView;
			array15[3] = grid2;
			array15[4] = this;
			object obj25;
			xamlServiceProvider14.Add(typeFromHandle28, obj25 = new SimpleValueTargetProvider(array15, LabelSwitch.TextProperty, nameScope));
			xamlServiceProvider14.Add(typeof(IReferenceProvider), obj25);
			Type typeFromHandle29 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver14 = new XmlNamespaceResolver();
			xmlNamespaceResolver14.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver14.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver14.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver14.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver14.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver14.Add("settings", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver14.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider14.Add(typeFromHandle29, new XamlTypeResolver(xmlNamespaceResolver14, typeof(VagRKDSPage).GetTypeInfo().Assembly));
			xamlServiceProvider14.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(132, 29)));
			object obj26 = markupExtension14.ProvideValue(xamlServiceProvider14);
			labelSwitch2.Text = obj26;
			stackLayout2.Children.Add(labelSwitch2);
			label7.SetValue(Label.FontAttributesProperty, new FontAttributesConverter().ConvertFromInvariantString("Bold"));
			translate9.Text = "codingDB_VagRKDS_TiresetsCount";
			IMarkupExtension markupExtension15 = translate9;
			XamlServiceProvider xamlServiceProvider15 = new XamlServiceProvider();
			Type typeFromHandle30 = typeof(IProvideValueTarget);
			object[] array16 = new object[0 + 5];
			array16[0] = label7;
			array16[1] = stackLayout2;
			array16[2] = listView;
			array16[3] = grid2;
			array16[4] = this;
			object obj27;
			xamlServiceProvider15.Add(typeFromHandle30, obj27 = new SimpleValueTargetProvider(array16, Label.TextProperty, nameScope));
			xamlServiceProvider15.Add(typeof(IReferenceProvider), obj27);
			Type typeFromHandle31 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver15 = new XmlNamespaceResolver();
			xmlNamespaceResolver15.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver15.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver15.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver15.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver15.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver15.Add("settings", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver15.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider15.Add(typeFromHandle31, new XamlTypeResolver(xmlNamespaceResolver15, typeof(VagRKDSPage).GetTypeInfo().Assembly));
			xamlServiceProvider15.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(133, 54)));
			object obj28 = markupExtension15.ProvideValue(xamlServiceProvider15);
			label7.Text = obj28;
			stackLayout2.Children.Add(label7);
			bindingExtension14.Mode = 1;
			staticResourceExtension5.Key = "IntToStringConverter";
			IMarkupExtension markupExtension16 = staticResourceExtension5;
			XamlServiceProvider xamlServiceProvider16 = new XamlServiceProvider();
			Type typeFromHandle32 = typeof(IProvideValueTarget);
			object[] array17 = new object[0 + 6];
			array17[0] = bindingExtension14;
			array17[1] = picker;
			array17[2] = stackLayout2;
			array17[3] = listView;
			array17[4] = grid2;
			array17[5] = this;
			object obj29;
			xamlServiceProvider16.Add(typeFromHandle32, obj29 = new SimpleValueTargetProvider(array17, typeof(BindingExtension).GetRuntimeProperty("Converter"), nameScope));
			xamlServiceProvider16.Add(typeof(IReferenceProvider), obj29);
			Type typeFromHandle33 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver16 = new XmlNamespaceResolver();
			xmlNamespaceResolver16.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver16.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver16.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver16.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver16.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver16.Add("settings", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver16.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider16.Add(typeFromHandle33, new XamlTypeResolver(xmlNamespaceResolver16, typeof(VagRKDSPage).GetTypeInfo().Assembly));
			xamlServiceProvider16.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(134, 33)));
			object obj30 = markupExtension16.ProvideValue(xamlServiceProvider16);
			bindingExtension14.Converter = obj30;
			bindingExtension14.Path = "Model.TiresetsCount";
			BindingBase bindingBase14 = bindingExtension14.ProvideValue(null);
			picker.SetBinding(Picker.SelectedItemProperty, bindingBase14);
			picker.Items.Add(text49);
			picker.Items.Add(text50);
			picker.Items.Add(text51);
			picker.Items.Add(text52);
			picker.Items.Add(text53);
			picker.Items.Add(text54);
			picker.Items.Add(text55);
			picker.Items.Add(text56);
			picker.Items.Add(text57);
			picker.Items.Add(text58);
			stackLayout2.Children.Add(picker);
			listView.SetValue(ListView.HeaderProperty, stackLayout2);
			IDataTemplate dataTemplate2 = dataTemplate;
			VagRKDSPage.<InitializeComponent>_anonXamlCDataTemplate_15 <InitializeComponent>_anonXamlCDataTemplate_ = new VagRKDSPage.<InitializeComponent>_anonXamlCDataTemplate_15();
			object[] array18 = new object[0 + 4];
			array18[0] = dataTemplate;
			array18[1] = listView;
			array18[2] = grid2;
			array18[3] = this;
			<InitializeComponent>_anonXamlCDataTemplate_.parentValues = array18;
			<InitializeComponent>_anonXamlCDataTemplate_.root = this;
			dataTemplate2.LoadTemplate = new Func<object>(<InitializeComponent>_anonXamlCDataTemplate_.LoadDataTemplate);
			listView.SetValue(ItemsView<Cell>.ItemTemplateProperty, dataTemplate);
			bindingExtension15.Mode = 2;
			referenceExtension.Name = "switchIndividual";
			IMarkupExtension markupExtension17 = referenceExtension;
			XamlServiceProvider xamlServiceProvider17 = new XamlServiceProvider();
			Type typeFromHandle34 = typeof(IProvideValueTarget);
			object[] array19 = new object[0 + 5];
			array19[0] = bindingExtension15;
			array19[1] = stackLayout3;
			array19[2] = listView;
			array19[3] = grid2;
			array19[4] = this;
			object obj31;
			xamlServiceProvider17.Add(typeFromHandle34, obj31 = new SimpleValueTargetProvider(array19, typeof(BindingExtension).GetRuntimeProperty("Source"), nameScope));
			xamlServiceProvider17.Add(typeof(IReferenceProvider), obj31);
			Type typeFromHandle35 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver17 = new XmlNamespaceResolver();
			xmlNamespaceResolver17.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver17.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver17.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver17.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver17.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver17.Add("settings", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver17.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider17.Add(typeFromHandle35, new XamlTypeResolver(xmlNamespaceResolver17, typeof(VagRKDSPage).GetTypeInfo().Assembly));
			xamlServiceProvider17.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(258, 34)));
			object obj32 = markupExtension17.ProvideValue(xamlServiceProvider17);
			bindingExtension15.Source = obj32;
			bindingExtension15.Path = "IsToggled";
			BindingBase bindingBase15 = bindingExtension15.ProvideValue(null);
			stackLayout3.SetBinding(VisualElement.IsVisibleProperty, bindingBase15);
			stackLayout3.SetValue(StackLayout.OrientationProperty, 0);
			label8.SetValue(Label.FontAttributesProperty, new FontAttributesConverter().ConvertFromInvariantString("Bold"));
			label8.SetValue(Label.TextProperty, "Individual:");
			stackLayout3.Children.Add(label8);
			bindingExtension16.Path = "Model.IndividualTireset";
			BindingBase bindingBase16 = bindingExtension16.ProvideValue(null);
			grid.SetBinding(BindableObject.BindingContextProperty, bindingBase16);
			columnDefinition.SetValue(ColumnDefinition.WidthProperty, new GridLengthTypeConverter().ConvertFromInvariantString("*"));
			grid.GetValue(Grid.ColumnDefinitionsProperty).Add(columnDefinition);
			columnDefinition2.SetValue(ColumnDefinition.WidthProperty, new GridLengthTypeConverter().ConvertFromInvariantString("*"));
			grid.GetValue(Grid.ColumnDefinitionsProperty).Add(columnDefinition2);
			columnDefinition3.SetValue(ColumnDefinition.WidthProperty, new GridLengthTypeConverter().ConvertFromInvariantString("*"));
			grid.GetValue(Grid.ColumnDefinitionsProperty).Add(columnDefinition3);
			rowDefinition2.SetValue(RowDefinition.HeightProperty, new GridLengthTypeConverter().ConvertFromInvariantString("auto"));
			grid.GetValue(Grid.RowDefinitionsProperty).Add(rowDefinition2);
			rowDefinition3.SetValue(RowDefinition.HeightProperty, new GridLengthTypeConverter().ConvertFromInvariantString("auto"));
			grid.GetValue(Grid.RowDefinitionsProperty).Add(rowDefinition3);
			rowDefinition4.SetValue(RowDefinition.HeightProperty, new GridLengthTypeConverter().ConvertFromInvariantString("auto"));
			grid.GetValue(Grid.RowDefinitionsProperty).Add(rowDefinition4);
			rowDefinition5.SetValue(RowDefinition.HeightProperty, new GridLengthTypeConverter().ConvertFromInvariantString("auto"));
			grid.GetValue(Grid.RowDefinitionsProperty).Add(rowDefinition5);
			rowDefinition6.SetValue(RowDefinition.HeightProperty, new GridLengthTypeConverter().ConvertFromInvariantString("auto"));
			grid.GetValue(Grid.RowDefinitionsProperty).Add(rowDefinition6);
			label9.SetValue(Grid.RowProperty, 0);
			label9.SetValue(Grid.ColumnProperty, 0);
			translate10.Text = "codingDB_VagRKDS_TiresetName";
			IMarkupExtension markupExtension18 = translate10;
			XamlServiceProvider xamlServiceProvider18 = new XamlServiceProvider();
			Type typeFromHandle36 = typeof(IProvideValueTarget);
			object[] array20 = new object[0 + 6];
			array20[0] = label9;
			array20[1] = grid;
			array20[2] = stackLayout3;
			array20[3] = listView;
			array20[4] = grid2;
			array20[5] = this;
			object obj33;
			xamlServiceProvider18.Add(typeFromHandle36, obj33 = new SimpleValueTargetProvider(array20, Label.TextProperty, nameScope));
			xamlServiceProvider18.Add(typeof(IReferenceProvider), obj33);
			Type typeFromHandle37 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver18 = new XmlNamespaceResolver();
			xmlNamespaceResolver18.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver18.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver18.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver18.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver18.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver18.Add("settings", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver18.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider18.Add(typeFromHandle37, new XamlTypeResolver(xmlNamespaceResolver18, typeof(VagRKDSPage).GetTypeInfo().Assembly));
			xamlServiceProvider18.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(277, 33)));
			object obj34 = markupExtension18.ProvideValue(xamlServiceProvider18);
			label9.Text = obj34;
			label9.SetValue(View.VerticalOptionsProperty, LayoutOptions.Center);
			grid.Children.Add(label9);
			entry2.SetValue(Grid.RowProperty, 0);
			entry2.SetValue(Grid.ColumnProperty, 1);
			entry2.SetValue(Grid.ColumnSpanProperty, 2);
			bindingExtension17.Mode = 1;
			bindingExtension17.Path = "Name";
			BindingBase bindingBase17 = bindingExtension17.ProvideValue(null);
			entry2.SetBinding(Entry.TextProperty, bindingBase17);
			entry2.SetValue(View.VerticalOptionsProperty, LayoutOptions.Center);
			grid.Children.Add(entry2);
			label10.SetValue(Grid.RowProperty, 1);
			label10.SetValue(Grid.ColumnProperty, 0);
			translate11.Text = "codingDB_VagRKDS_LoadType";
			IMarkupExtension markupExtension19 = translate11;
			XamlServiceProvider xamlServiceProvider19 = new XamlServiceProvider();
			Type typeFromHandle38 = typeof(IProvideValueTarget);
			object[] array21 = new object[0 + 6];
			array21[0] = label10;
			array21[1] = grid;
			array21[2] = stackLayout3;
			array21[3] = listView;
			array21[4] = grid2;
			array21[5] = this;
			object obj35;
			xamlServiceProvider19.Add(typeFromHandle38, obj35 = new SimpleValueTargetProvider(array21, Label.TextProperty, nameScope));
			xamlServiceProvider19.Add(typeof(IReferenceProvider), obj35);
			Type typeFromHandle39 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver19 = new XmlNamespaceResolver();
			xmlNamespaceResolver19.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver19.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver19.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver19.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver19.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver19.Add("settings", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver19.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider19.Add(typeFromHandle39, new XamlTypeResolver(xmlNamespaceResolver19, typeof(VagRKDSPage).GetTypeInfo().Assembly));
			xamlServiceProvider19.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(289, 33)));
			object obj36 = markupExtension19.ProvideValue(xamlServiceProvider19);
			label10.Text = obj36;
			label10.SetValue(View.VerticalOptionsProperty, LayoutOptions.Center);
			grid.Children.Add(label10);
			label11.SetValue(Grid.RowProperty, 1);
			label11.SetValue(Grid.ColumnProperty, 1);
			translate12.Text = "codingDB_VagRKDS_Front";
			IMarkupExtension markupExtension20 = translate12;
			XamlServiceProvider xamlServiceProvider20 = new XamlServiceProvider();
			Type typeFromHandle40 = typeof(IProvideValueTarget);
			object[] array22 = new object[0 + 6];
			array22[0] = label11;
			array22[1] = grid;
			array22[2] = stackLayout3;
			array22[3] = listView;
			array22[4] = grid2;
			array22[5] = this;
			object obj37;
			xamlServiceProvider20.Add(typeFromHandle40, obj37 = new SimpleValueTargetProvider(array22, Label.TextProperty, nameScope));
			xamlServiceProvider20.Add(typeof(IReferenceProvider), obj37);
			Type typeFromHandle41 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver20 = new XmlNamespaceResolver();
			xmlNamespaceResolver20.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver20.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver20.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver20.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver20.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver20.Add("settings", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver20.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider20.Add(typeFromHandle41, new XamlTypeResolver(xmlNamespaceResolver20, typeof(VagRKDSPage).GetTypeInfo().Assembly));
			xamlServiceProvider20.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(294, 33)));
			object obj38 = markupExtension20.ProvideValue(xamlServiceProvider20);
			label11.Text = obj38;
			label11.SetValue(View.VerticalOptionsProperty, LayoutOptions.Center);
			grid.Children.Add(label11);
			label12.SetValue(Grid.RowProperty, 1);
			label12.SetValue(Grid.ColumnProperty, 2);
			translate13.Text = "codingDB_VagRKDS_Rear";
			IMarkupExtension markupExtension21 = translate13;
			XamlServiceProvider xamlServiceProvider21 = new XamlServiceProvider();
			Type typeFromHandle42 = typeof(IProvideValueTarget);
			object[] array23 = new object[0 + 6];
			array23[0] = label12;
			array23[1] = grid;
			array23[2] = stackLayout3;
			array23[3] = listView;
			array23[4] = grid2;
			array23[5] = this;
			object obj39;
			xamlServiceProvider21.Add(typeFromHandle42, obj39 = new SimpleValueTargetProvider(array23, Label.TextProperty, nameScope));
			xamlServiceProvider21.Add(typeof(IReferenceProvider), obj39);
			Type typeFromHandle43 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver21 = new XmlNamespaceResolver();
			xmlNamespaceResolver21.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver21.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver21.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver21.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver21.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver21.Add("settings", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver21.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider21.Add(typeFromHandle43, new XamlTypeResolver(xmlNamespaceResolver21, typeof(VagRKDSPage).GetTypeInfo().Assembly));
			xamlServiceProvider21.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(299, 33)));
			object obj40 = markupExtension21.ProvideValue(xamlServiceProvider21);
			label12.Text = obj40;
			label12.SetValue(View.VerticalOptionsProperty, LayoutOptions.Center);
			grid.Children.Add(label12);
			label13.SetValue(Grid.RowProperty, 2);
			label13.SetValue(Grid.ColumnProperty, 0);
			translate14.Text = "codingDB_VagRKDS_FullLoad";
			IMarkupExtension markupExtension22 = translate14;
			XamlServiceProvider xamlServiceProvider22 = new XamlServiceProvider();
			Type typeFromHandle44 = typeof(IProvideValueTarget);
			object[] array24 = new object[0 + 6];
			array24[0] = label13;
			array24[1] = grid;
			array24[2] = stackLayout3;
			array24[3] = listView;
			array24[4] = grid2;
			array24[5] = this;
			object obj41;
			xamlServiceProvider22.Add(typeFromHandle44, obj41 = new SimpleValueTargetProvider(array24, Label.TextProperty, nameScope));
			xamlServiceProvider22.Add(typeof(IReferenceProvider), obj41);
			Type typeFromHandle45 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver22 = new XmlNamespaceResolver();
			xmlNamespaceResolver22.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver22.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver22.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver22.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver22.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver22.Add("settings", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver22.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider22.Add(typeFromHandle45, new XamlTypeResolver(xmlNamespaceResolver22, typeof(VagRKDSPage).GetTypeInfo().Assembly));
			xamlServiceProvider22.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(304, 33)));
			object obj42 = markupExtension22.ProvideValue(xamlServiceProvider22);
			label13.Text = obj42;
			label13.SetValue(View.VerticalOptionsProperty, LayoutOptions.Center);
			grid.Children.Add(label13);
			picker2.SetValue(Grid.RowProperty, 2);
			picker2.SetValue(Grid.ColumnProperty, 1);
			staticResourceExtension6.Key = "pressureValues";
			IMarkupExtension markupExtension23 = staticResourceExtension6;
			XamlServiceProvider xamlServiceProvider23 = new XamlServiceProvider();
			Type typeFromHandle46 = typeof(IProvideValueTarget);
			object[] array25 = new object[0 + 6];
			array25[0] = picker2;
			array25[1] = grid;
			array25[2] = stackLayout3;
			array25[3] = listView;
			array25[4] = grid2;
			array25[5] = this;
			object obj43;
			xamlServiceProvider23.Add(typeFromHandle46, obj43 = new SimpleValueTargetProvider(array25, Picker.ItemsSourceProperty, nameScope));
			xamlServiceProvider23.Add(typeof(IReferenceProvider), obj43);
			Type typeFromHandle47 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver23 = new XmlNamespaceResolver();
			xmlNamespaceResolver23.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver23.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver23.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver23.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver23.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver23.Add("settings", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver23.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider23.Add(typeFromHandle47, new XamlTypeResolver(xmlNamespaceResolver23, typeof(VagRKDSPage).GetTypeInfo().Assembly));
			xamlServiceProvider23.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(309, 33)));
			object obj44 = markupExtension23.ProvideValue(xamlServiceProvider23);
			picker2.ItemsSource = obj44;
			bindingExtension18.Mode = 1;
			staticResourceExtension7.Key = "DoubleToRKDSPressurePositionConverter";
			IMarkupExtension markupExtension24 = staticResourceExtension7;
			XamlServiceProvider xamlServiceProvider24 = new XamlServiceProvider();
			Type typeFromHandle48 = typeof(IProvideValueTarget);
			object[] array26 = new object[0 + 7];
			array26[0] = bindingExtension18;
			array26[1] = picker2;
			array26[2] = grid;
			array26[3] = stackLayout3;
			array26[4] = listView;
			array26[5] = grid2;
			array26[6] = this;
			object obj45;
			xamlServiceProvider24.Add(typeFromHandle48, obj45 = new SimpleValueTargetProvider(array26, typeof(BindingExtension).GetRuntimeProperty("Converter"), nameScope));
			xamlServiceProvider24.Add(typeof(IReferenceProvider), obj45);
			Type typeFromHandle49 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver24 = new XmlNamespaceResolver();
			xmlNamespaceResolver24.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver24.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver24.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver24.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver24.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver24.Add("settings", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver24.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider24.Add(typeFromHandle49, new XamlTypeResolver(xmlNamespaceResolver24, typeof(VagRKDSPage).GetTypeInfo().Assembly));
			xamlServiceProvider24.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(310, 33)));
			object obj46 = markupExtension24.ProvideValue(xamlServiceProvider24);
			bindingExtension18.Converter = obj46;
			staticResourceExtension8.Key = "pressureValues";
			IMarkupExtension markupExtension25 = staticResourceExtension8;
			XamlServiceProvider xamlServiceProvider25 = new XamlServiceProvider();
			Type typeFromHandle50 = typeof(IProvideValueTarget);
			object[] array27 = new object[0 + 7];
			array27[0] = bindingExtension18;
			array27[1] = picker2;
			array27[2] = grid;
			array27[3] = stackLayout3;
			array27[4] = listView;
			array27[5] = grid2;
			array27[6] = this;
			object obj47;
			xamlServiceProvider25.Add(typeFromHandle50, obj47 = new SimpleValueTargetProvider(array27, typeof(BindingExtension).GetRuntimeProperty("ConverterParameter"), nameScope));
			xamlServiceProvider25.Add(typeof(IReferenceProvider), obj47);
			Type typeFromHandle51 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver25 = new XmlNamespaceResolver();
			xmlNamespaceResolver25.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver25.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver25.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver25.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver25.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver25.Add("settings", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver25.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider25.Add(typeFromHandle51, new XamlTypeResolver(xmlNamespaceResolver25, typeof(VagRKDSPage).GetTypeInfo().Assembly));
			xamlServiceProvider25.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(310, 33)));
			object obj48 = markupExtension25.ProvideValue(xamlServiceProvider25);
			bindingExtension18.ConverterParameter = obj48;
			bindingExtension18.Path = "FrontFull";
			BindingBase bindingBase18 = bindingExtension18.ProvideValue(null);
			picker2.SetBinding(Picker.SelectedIndexProperty, bindingBase18);
			picker2.SetValue(View.VerticalOptionsProperty, LayoutOptions.Center);
			grid.Children.Add(picker2);
			picker3.SetValue(Grid.RowProperty, 2);
			picker3.SetValue(Grid.ColumnProperty, 2);
			staticResourceExtension9.Key = "pressureValues";
			IMarkupExtension markupExtension26 = staticResourceExtension9;
			XamlServiceProvider xamlServiceProvider26 = new XamlServiceProvider();
			Type typeFromHandle52 = typeof(IProvideValueTarget);
			object[] array28 = new object[0 + 6];
			array28[0] = picker3;
			array28[1] = grid;
			array28[2] = stackLayout3;
			array28[3] = listView;
			array28[4] = grid2;
			array28[5] = this;
			object obj49;
			xamlServiceProvider26.Add(typeFromHandle52, obj49 = new SimpleValueTargetProvider(array28, Picker.ItemsSourceProperty, nameScope));
			xamlServiceProvider26.Add(typeof(IReferenceProvider), obj49);
			Type typeFromHandle53 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver26 = new XmlNamespaceResolver();
			xmlNamespaceResolver26.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver26.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver26.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver26.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver26.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver26.Add("settings", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver26.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider26.Add(typeFromHandle53, new XamlTypeResolver(xmlNamespaceResolver26, typeof(VagRKDSPage).GetTypeInfo().Assembly));
			xamlServiceProvider26.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(315, 33)));
			object obj50 = markupExtension26.ProvideValue(xamlServiceProvider26);
			picker3.ItemsSource = obj50;
			bindingExtension19.Mode = 1;
			staticResourceExtension10.Key = "DoubleToRKDSPressurePositionConverter";
			IMarkupExtension markupExtension27 = staticResourceExtension10;
			XamlServiceProvider xamlServiceProvider27 = new XamlServiceProvider();
			Type typeFromHandle54 = typeof(IProvideValueTarget);
			object[] array29 = new object[0 + 7];
			array29[0] = bindingExtension19;
			array29[1] = picker3;
			array29[2] = grid;
			array29[3] = stackLayout3;
			array29[4] = listView;
			array29[5] = grid2;
			array29[6] = this;
			object obj51;
			xamlServiceProvider27.Add(typeFromHandle54, obj51 = new SimpleValueTargetProvider(array29, typeof(BindingExtension).GetRuntimeProperty("Converter"), nameScope));
			xamlServiceProvider27.Add(typeof(IReferenceProvider), obj51);
			Type typeFromHandle55 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver27 = new XmlNamespaceResolver();
			xmlNamespaceResolver27.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver27.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver27.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver27.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver27.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver27.Add("settings", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver27.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider27.Add(typeFromHandle55, new XamlTypeResolver(xmlNamespaceResolver27, typeof(VagRKDSPage).GetTypeInfo().Assembly));
			xamlServiceProvider27.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(316, 33)));
			object obj52 = markupExtension27.ProvideValue(xamlServiceProvider27);
			bindingExtension19.Converter = obj52;
			staticResourceExtension11.Key = "pressureValues";
			IMarkupExtension markupExtension28 = staticResourceExtension11;
			XamlServiceProvider xamlServiceProvider28 = new XamlServiceProvider();
			Type typeFromHandle56 = typeof(IProvideValueTarget);
			object[] array30 = new object[0 + 7];
			array30[0] = bindingExtension19;
			array30[1] = picker3;
			array30[2] = grid;
			array30[3] = stackLayout3;
			array30[4] = listView;
			array30[5] = grid2;
			array30[6] = this;
			object obj53;
			xamlServiceProvider28.Add(typeFromHandle56, obj53 = new SimpleValueTargetProvider(array30, typeof(BindingExtension).GetRuntimeProperty("ConverterParameter"), nameScope));
			xamlServiceProvider28.Add(typeof(IReferenceProvider), obj53);
			Type typeFromHandle57 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver28 = new XmlNamespaceResolver();
			xmlNamespaceResolver28.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver28.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver28.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver28.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver28.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver28.Add("settings", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver28.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider28.Add(typeFromHandle57, new XamlTypeResolver(xmlNamespaceResolver28, typeof(VagRKDSPage).GetTypeInfo().Assembly));
			xamlServiceProvider28.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(316, 33)));
			object obj54 = markupExtension28.ProvideValue(xamlServiceProvider28);
			bindingExtension19.ConverterParameter = obj54;
			bindingExtension19.Path = "RearFull";
			BindingBase bindingBase19 = bindingExtension19.ProvideValue(null);
			picker3.SetBinding(Picker.SelectedIndexProperty, bindingBase19);
			picker3.SetValue(View.VerticalOptionsProperty, LayoutOptions.Center);
			grid.Children.Add(picker3);
			label14.SetValue(Grid.RowProperty, 3);
			label14.SetValue(Grid.ColumnProperty, 0);
			translate15.Text = "codingDB_VagRKDS_PartialLoad";
			IMarkupExtension markupExtension29 = translate15;
			XamlServiceProvider xamlServiceProvider29 = new XamlServiceProvider();
			Type typeFromHandle58 = typeof(IProvideValueTarget);
			object[] array31 = new object[0 + 6];
			array31[0] = label14;
			array31[1] = grid;
			array31[2] = stackLayout3;
			array31[3] = listView;
			array31[4] = grid2;
			array31[5] = this;
			object obj55;
			xamlServiceProvider29.Add(typeFromHandle58, obj55 = new SimpleValueTargetProvider(array31, Label.TextProperty, nameScope));
			xamlServiceProvider29.Add(typeof(IReferenceProvider), obj55);
			Type typeFromHandle59 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver29 = new XmlNamespaceResolver();
			xmlNamespaceResolver29.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver29.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver29.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver29.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver29.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver29.Add("settings", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver29.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider29.Add(typeFromHandle59, new XamlTypeResolver(xmlNamespaceResolver29, typeof(VagRKDSPage).GetTypeInfo().Assembly));
			xamlServiceProvider29.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(321, 33)));
			object obj56 = markupExtension29.ProvideValue(xamlServiceProvider29);
			label14.Text = obj56;
			label14.SetValue(View.VerticalOptionsProperty, LayoutOptions.Center);
			grid.Children.Add(label14);
			picker4.SetValue(Grid.RowProperty, 3);
			picker4.SetValue(Grid.ColumnProperty, 1);
			staticResourceExtension12.Key = "pressureValues";
			IMarkupExtension markupExtension30 = staticResourceExtension12;
			XamlServiceProvider xamlServiceProvider30 = new XamlServiceProvider();
			Type typeFromHandle60 = typeof(IProvideValueTarget);
			object[] array32 = new object[0 + 6];
			array32[0] = picker4;
			array32[1] = grid;
			array32[2] = stackLayout3;
			array32[3] = listView;
			array32[4] = grid2;
			array32[5] = this;
			object obj57;
			xamlServiceProvider30.Add(typeFromHandle60, obj57 = new SimpleValueTargetProvider(array32, Picker.ItemsSourceProperty, nameScope));
			xamlServiceProvider30.Add(typeof(IReferenceProvider), obj57);
			Type typeFromHandle61 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver30 = new XmlNamespaceResolver();
			xmlNamespaceResolver30.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver30.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver30.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver30.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver30.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver30.Add("settings", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver30.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider30.Add(typeFromHandle61, new XamlTypeResolver(xmlNamespaceResolver30, typeof(VagRKDSPage).GetTypeInfo().Assembly));
			xamlServiceProvider30.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(326, 33)));
			object obj58 = markupExtension30.ProvideValue(xamlServiceProvider30);
			picker4.ItemsSource = obj58;
			bindingExtension20.Mode = 1;
			staticResourceExtension13.Key = "DoubleToRKDSPressurePositionConverter";
			IMarkupExtension markupExtension31 = staticResourceExtension13;
			XamlServiceProvider xamlServiceProvider31 = new XamlServiceProvider();
			Type typeFromHandle62 = typeof(IProvideValueTarget);
			object[] array33 = new object[0 + 7];
			array33[0] = bindingExtension20;
			array33[1] = picker4;
			array33[2] = grid;
			array33[3] = stackLayout3;
			array33[4] = listView;
			array33[5] = grid2;
			array33[6] = this;
			object obj59;
			xamlServiceProvider31.Add(typeFromHandle62, obj59 = new SimpleValueTargetProvider(array33, typeof(BindingExtension).GetRuntimeProperty("Converter"), nameScope));
			xamlServiceProvider31.Add(typeof(IReferenceProvider), obj59);
			Type typeFromHandle63 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver31 = new XmlNamespaceResolver();
			xmlNamespaceResolver31.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver31.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver31.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver31.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver31.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver31.Add("settings", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver31.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider31.Add(typeFromHandle63, new XamlTypeResolver(xmlNamespaceResolver31, typeof(VagRKDSPage).GetTypeInfo().Assembly));
			xamlServiceProvider31.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(327, 33)));
			object obj60 = markupExtension31.ProvideValue(xamlServiceProvider31);
			bindingExtension20.Converter = obj60;
			staticResourceExtension14.Key = "pressureValues";
			IMarkupExtension markupExtension32 = staticResourceExtension14;
			XamlServiceProvider xamlServiceProvider32 = new XamlServiceProvider();
			Type typeFromHandle64 = typeof(IProvideValueTarget);
			object[] array34 = new object[0 + 7];
			array34[0] = bindingExtension20;
			array34[1] = picker4;
			array34[2] = grid;
			array34[3] = stackLayout3;
			array34[4] = listView;
			array34[5] = grid2;
			array34[6] = this;
			object obj61;
			xamlServiceProvider32.Add(typeFromHandle64, obj61 = new SimpleValueTargetProvider(array34, typeof(BindingExtension).GetRuntimeProperty("ConverterParameter"), nameScope));
			xamlServiceProvider32.Add(typeof(IReferenceProvider), obj61);
			Type typeFromHandle65 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver32 = new XmlNamespaceResolver();
			xmlNamespaceResolver32.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver32.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver32.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver32.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver32.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver32.Add("settings", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver32.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider32.Add(typeFromHandle65, new XamlTypeResolver(xmlNamespaceResolver32, typeof(VagRKDSPage).GetTypeInfo().Assembly));
			xamlServiceProvider32.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(327, 33)));
			object obj62 = markupExtension32.ProvideValue(xamlServiceProvider32);
			bindingExtension20.ConverterParameter = obj62;
			bindingExtension20.Path = "FrontPartial";
			BindingBase bindingBase20 = bindingExtension20.ProvideValue(null);
			picker4.SetBinding(Picker.SelectedIndexProperty, bindingBase20);
			picker4.SetValue(View.VerticalOptionsProperty, LayoutOptions.Center);
			grid.Children.Add(picker4);
			picker5.SetValue(Grid.RowProperty, 3);
			picker5.SetValue(Grid.ColumnProperty, 2);
			staticResourceExtension15.Key = "pressureValues";
			IMarkupExtension markupExtension33 = staticResourceExtension15;
			XamlServiceProvider xamlServiceProvider33 = new XamlServiceProvider();
			Type typeFromHandle66 = typeof(IProvideValueTarget);
			object[] array35 = new object[0 + 6];
			array35[0] = picker5;
			array35[1] = grid;
			array35[2] = stackLayout3;
			array35[3] = listView;
			array35[4] = grid2;
			array35[5] = this;
			object obj63;
			xamlServiceProvider33.Add(typeFromHandle66, obj63 = new SimpleValueTargetProvider(array35, Picker.ItemsSourceProperty, nameScope));
			xamlServiceProvider33.Add(typeof(IReferenceProvider), obj63);
			Type typeFromHandle67 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver33 = new XmlNamespaceResolver();
			xmlNamespaceResolver33.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver33.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver33.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver33.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver33.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver33.Add("settings", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver33.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider33.Add(typeFromHandle67, new XamlTypeResolver(xmlNamespaceResolver33, typeof(VagRKDSPage).GetTypeInfo().Assembly));
			xamlServiceProvider33.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(332, 33)));
			object obj64 = markupExtension33.ProvideValue(xamlServiceProvider33);
			picker5.ItemsSource = obj64;
			bindingExtension21.Mode = 1;
			staticResourceExtension16.Key = "DoubleToRKDSPressurePositionConverter";
			IMarkupExtension markupExtension34 = staticResourceExtension16;
			XamlServiceProvider xamlServiceProvider34 = new XamlServiceProvider();
			Type typeFromHandle68 = typeof(IProvideValueTarget);
			object[] array36 = new object[0 + 7];
			array36[0] = bindingExtension21;
			array36[1] = picker5;
			array36[2] = grid;
			array36[3] = stackLayout3;
			array36[4] = listView;
			array36[5] = grid2;
			array36[6] = this;
			object obj65;
			xamlServiceProvider34.Add(typeFromHandle68, obj65 = new SimpleValueTargetProvider(array36, typeof(BindingExtension).GetRuntimeProperty("Converter"), nameScope));
			xamlServiceProvider34.Add(typeof(IReferenceProvider), obj65);
			Type typeFromHandle69 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver34 = new XmlNamespaceResolver();
			xmlNamespaceResolver34.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver34.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver34.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver34.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver34.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver34.Add("settings", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver34.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider34.Add(typeFromHandle69, new XamlTypeResolver(xmlNamespaceResolver34, typeof(VagRKDSPage).GetTypeInfo().Assembly));
			xamlServiceProvider34.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(333, 33)));
			object obj66 = markupExtension34.ProvideValue(xamlServiceProvider34);
			bindingExtension21.Converter = obj66;
			staticResourceExtension17.Key = "pressureValues";
			IMarkupExtension markupExtension35 = staticResourceExtension17;
			XamlServiceProvider xamlServiceProvider35 = new XamlServiceProvider();
			Type typeFromHandle70 = typeof(IProvideValueTarget);
			object[] array37 = new object[0 + 7];
			array37[0] = bindingExtension21;
			array37[1] = picker5;
			array37[2] = grid;
			array37[3] = stackLayout3;
			array37[4] = listView;
			array37[5] = grid2;
			array37[6] = this;
			object obj67;
			xamlServiceProvider35.Add(typeFromHandle70, obj67 = new SimpleValueTargetProvider(array37, typeof(BindingExtension).GetRuntimeProperty("ConverterParameter"), nameScope));
			xamlServiceProvider35.Add(typeof(IReferenceProvider), obj67);
			Type typeFromHandle71 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver35 = new XmlNamespaceResolver();
			xmlNamespaceResolver35.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver35.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver35.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver35.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver35.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver35.Add("settings", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver35.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider35.Add(typeFromHandle71, new XamlTypeResolver(xmlNamespaceResolver35, typeof(VagRKDSPage).GetTypeInfo().Assembly));
			xamlServiceProvider35.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(333, 33)));
			object obj68 = markupExtension35.ProvideValue(xamlServiceProvider35);
			bindingExtension21.ConverterParameter = obj68;
			bindingExtension21.Path = "RearPartial";
			BindingBase bindingBase21 = bindingExtension21.ProvideValue(null);
			picker5.SetBinding(Picker.SelectedIndexProperty, bindingBase21);
			picker5.SetValue(View.VerticalOptionsProperty, LayoutOptions.Center);
			grid.Children.Add(picker5);
			label15.SetValue(Grid.RowProperty, 4);
			label15.SetValue(Grid.ColumnProperty, 0);
			translate16.Text = "codingDB_VagRKDS_ComfortLoad";
			IMarkupExtension markupExtension36 = translate16;
			XamlServiceProvider xamlServiceProvider36 = new XamlServiceProvider();
			Type typeFromHandle72 = typeof(IProvideValueTarget);
			object[] array38 = new object[0 + 6];
			array38[0] = label15;
			array38[1] = grid;
			array38[2] = stackLayout3;
			array38[3] = listView;
			array38[4] = grid2;
			array38[5] = this;
			object obj69;
			xamlServiceProvider36.Add(typeFromHandle72, obj69 = new SimpleValueTargetProvider(array38, Label.TextProperty, nameScope));
			xamlServiceProvider36.Add(typeof(IReferenceProvider), obj69);
			Type typeFromHandle73 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver36 = new XmlNamespaceResolver();
			xmlNamespaceResolver36.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver36.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver36.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver36.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver36.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver36.Add("settings", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver36.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider36.Add(typeFromHandle73, new XamlTypeResolver(xmlNamespaceResolver36, typeof(VagRKDSPage).GetTypeInfo().Assembly));
			xamlServiceProvider36.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(339, 33)));
			object obj70 = markupExtension36.ProvideValue(xamlServiceProvider36);
			label15.Text = obj70;
			label15.SetValue(View.VerticalOptionsProperty, LayoutOptions.Center);
			grid.Children.Add(label15);
			picker6.SetValue(Grid.RowProperty, 4);
			picker6.SetValue(Grid.ColumnProperty, 1);
			staticResourceExtension18.Key = "pressureValues";
			IMarkupExtension markupExtension37 = staticResourceExtension18;
			XamlServiceProvider xamlServiceProvider37 = new XamlServiceProvider();
			Type typeFromHandle74 = typeof(IProvideValueTarget);
			object[] array39 = new object[0 + 6];
			array39[0] = picker6;
			array39[1] = grid;
			array39[2] = stackLayout3;
			array39[3] = listView;
			array39[4] = grid2;
			array39[5] = this;
			object obj71;
			xamlServiceProvider37.Add(typeFromHandle74, obj71 = new SimpleValueTargetProvider(array39, Picker.ItemsSourceProperty, nameScope));
			xamlServiceProvider37.Add(typeof(IReferenceProvider), obj71);
			Type typeFromHandle75 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver37 = new XmlNamespaceResolver();
			xmlNamespaceResolver37.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver37.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver37.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver37.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver37.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver37.Add("settings", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver37.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider37.Add(typeFromHandle75, new XamlTypeResolver(xmlNamespaceResolver37, typeof(VagRKDSPage).GetTypeInfo().Assembly));
			xamlServiceProvider37.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(344, 33)));
			object obj72 = markupExtension37.ProvideValue(xamlServiceProvider37);
			picker6.ItemsSource = obj72;
			bindingExtension22.Mode = 1;
			staticResourceExtension19.Key = "DoubleToRKDSPressurePositionConverter";
			IMarkupExtension markupExtension38 = staticResourceExtension19;
			XamlServiceProvider xamlServiceProvider38 = new XamlServiceProvider();
			Type typeFromHandle76 = typeof(IProvideValueTarget);
			object[] array40 = new object[0 + 7];
			array40[0] = bindingExtension22;
			array40[1] = picker6;
			array40[2] = grid;
			array40[3] = stackLayout3;
			array40[4] = listView;
			array40[5] = grid2;
			array40[6] = this;
			object obj73;
			xamlServiceProvider38.Add(typeFromHandle76, obj73 = new SimpleValueTargetProvider(array40, typeof(BindingExtension).GetRuntimeProperty("Converter"), nameScope));
			xamlServiceProvider38.Add(typeof(IReferenceProvider), obj73);
			Type typeFromHandle77 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver38 = new XmlNamespaceResolver();
			xmlNamespaceResolver38.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver38.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver38.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver38.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver38.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver38.Add("settings", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver38.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider38.Add(typeFromHandle77, new XamlTypeResolver(xmlNamespaceResolver38, typeof(VagRKDSPage).GetTypeInfo().Assembly));
			xamlServiceProvider38.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(345, 33)));
			object obj74 = markupExtension38.ProvideValue(xamlServiceProvider38);
			bindingExtension22.Converter = obj74;
			staticResourceExtension20.Key = "pressureValues";
			IMarkupExtension markupExtension39 = staticResourceExtension20;
			XamlServiceProvider xamlServiceProvider39 = new XamlServiceProvider();
			Type typeFromHandle78 = typeof(IProvideValueTarget);
			object[] array41 = new object[0 + 7];
			array41[0] = bindingExtension22;
			array41[1] = picker6;
			array41[2] = grid;
			array41[3] = stackLayout3;
			array41[4] = listView;
			array41[5] = grid2;
			array41[6] = this;
			object obj75;
			xamlServiceProvider39.Add(typeFromHandle78, obj75 = new SimpleValueTargetProvider(array41, typeof(BindingExtension).GetRuntimeProperty("ConverterParameter"), nameScope));
			xamlServiceProvider39.Add(typeof(IReferenceProvider), obj75);
			Type typeFromHandle79 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver39 = new XmlNamespaceResolver();
			xmlNamespaceResolver39.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver39.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver39.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver39.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver39.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver39.Add("settings", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver39.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider39.Add(typeFromHandle79, new XamlTypeResolver(xmlNamespaceResolver39, typeof(VagRKDSPage).GetTypeInfo().Assembly));
			xamlServiceProvider39.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(345, 33)));
			object obj76 = markupExtension39.ProvideValue(xamlServiceProvider39);
			bindingExtension22.ConverterParameter = obj76;
			bindingExtension22.Path = "FrontComfort";
			BindingBase bindingBase22 = bindingExtension22.ProvideValue(null);
			picker6.SetBinding(Picker.SelectedIndexProperty, bindingBase22);
			picker6.SetValue(View.VerticalOptionsProperty, LayoutOptions.Center);
			grid.Children.Add(picker6);
			picker7.SetValue(Grid.RowProperty, 4);
			picker7.SetValue(Grid.ColumnProperty, 2);
			staticResourceExtension21.Key = "pressureValues";
			IMarkupExtension markupExtension40 = staticResourceExtension21;
			XamlServiceProvider xamlServiceProvider40 = new XamlServiceProvider();
			Type typeFromHandle80 = typeof(IProvideValueTarget);
			object[] array42 = new object[0 + 6];
			array42[0] = picker7;
			array42[1] = grid;
			array42[2] = stackLayout3;
			array42[3] = listView;
			array42[4] = grid2;
			array42[5] = this;
			object obj77;
			xamlServiceProvider40.Add(typeFromHandle80, obj77 = new SimpleValueTargetProvider(array42, Picker.ItemsSourceProperty, nameScope));
			xamlServiceProvider40.Add(typeof(IReferenceProvider), obj77);
			Type typeFromHandle81 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver40 = new XmlNamespaceResolver();
			xmlNamespaceResolver40.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver40.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver40.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver40.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver40.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver40.Add("settings", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver40.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider40.Add(typeFromHandle81, new XamlTypeResolver(xmlNamespaceResolver40, typeof(VagRKDSPage).GetTypeInfo().Assembly));
			xamlServiceProvider40.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(350, 33)));
			object obj78 = markupExtension40.ProvideValue(xamlServiceProvider40);
			picker7.ItemsSource = obj78;
			bindingExtension23.Mode = 1;
			staticResourceExtension22.Key = "DoubleToRKDSPressurePositionConverter";
			IMarkupExtension markupExtension41 = staticResourceExtension22;
			XamlServiceProvider xamlServiceProvider41 = new XamlServiceProvider();
			Type typeFromHandle82 = typeof(IProvideValueTarget);
			object[] array43 = new object[0 + 7];
			array43[0] = bindingExtension23;
			array43[1] = picker7;
			array43[2] = grid;
			array43[3] = stackLayout3;
			array43[4] = listView;
			array43[5] = grid2;
			array43[6] = this;
			object obj79;
			xamlServiceProvider41.Add(typeFromHandle82, obj79 = new SimpleValueTargetProvider(array43, typeof(BindingExtension).GetRuntimeProperty("Converter"), nameScope));
			xamlServiceProvider41.Add(typeof(IReferenceProvider), obj79);
			Type typeFromHandle83 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver41 = new XmlNamespaceResolver();
			xmlNamespaceResolver41.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver41.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver41.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver41.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver41.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver41.Add("settings", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver41.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider41.Add(typeFromHandle83, new XamlTypeResolver(xmlNamespaceResolver41, typeof(VagRKDSPage).GetTypeInfo().Assembly));
			xamlServiceProvider41.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(351, 33)));
			object obj80 = markupExtension41.ProvideValue(xamlServiceProvider41);
			bindingExtension23.Converter = obj80;
			staticResourceExtension23.Key = "pressureValues";
			IMarkupExtension markupExtension42 = staticResourceExtension23;
			XamlServiceProvider xamlServiceProvider42 = new XamlServiceProvider();
			Type typeFromHandle84 = typeof(IProvideValueTarget);
			object[] array44 = new object[0 + 7];
			array44[0] = bindingExtension23;
			array44[1] = picker7;
			array44[2] = grid;
			array44[3] = stackLayout3;
			array44[4] = listView;
			array44[5] = grid2;
			array44[6] = this;
			object obj81;
			xamlServiceProvider42.Add(typeFromHandle84, obj81 = new SimpleValueTargetProvider(array44, typeof(BindingExtension).GetRuntimeProperty("ConverterParameter"), nameScope));
			xamlServiceProvider42.Add(typeof(IReferenceProvider), obj81);
			Type typeFromHandle85 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver42 = new XmlNamespaceResolver();
			xmlNamespaceResolver42.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver42.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver42.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver42.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver42.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver42.Add("settings", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver42.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider42.Add(typeFromHandle85, new XamlTypeResolver(xmlNamespaceResolver42, typeof(VagRKDSPage).GetTypeInfo().Assembly));
			xamlServiceProvider42.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(351, 33)));
			object obj82 = markupExtension42.ProvideValue(xamlServiceProvider42);
			bindingExtension23.ConverterParameter = obj82;
			bindingExtension23.Path = "RearComfort";
			BindingBase bindingBase23 = bindingExtension23.ProvideValue(null);
			picker7.SetBinding(Picker.SelectedIndexProperty, bindingBase23);
			picker7.SetValue(View.VerticalOptionsProperty, LayoutOptions.Center);
			grid.Children.Add(picker7);
			stackLayout3.Children.Add(grid);
			listView.SetValue(ListView.FooterProperty, stackLayout3);
			grid2.Children.Add(listView);
			activityFrame.SetValue(Grid.RowProperty, 0);
			activityFrame.SetValue(VisualElement.IsVisibleProperty, new VisualElement.VisibilityConverter().ConvertFromInvariantString("False"));
			grid2.Children.Add(activityFrame);
			this.SetValue(ContentPage.ContentProperty, grid2);
		}

		// Token: 0x06004FD5 RID: 20437 RVA: 0x003D1CC9 File Offset: 0x003CFEC9
		[CompilerGenerated]
		private void <btnSaveState_Clicked>b__9_0(string s)
		{
			Device.BeginInvokeOnMainThread(delegate
			{
				this.activityFrame.Text = s;
			});
		}

		// Token: 0x06004FD6 RID: 20438 RVA: 0x003D1CEE File Offset: 0x003CFEEE
		[CompilerGenerated]
		private void <btnPickODISXml_Clicked>b__10_0(string s)
		{
			Device.BeginInvokeOnMainThread(delegate
			{
				this.activityFrame.Text = s;
			});
		}

		// Token: 0x06004FD7 RID: 20439 RVA: 0x003D1D14 File Offset: 0x003CFF14
		private void __InitComponentRuntime()
		{
			Extensions.LoadFromXaml<VagRKDSPage>(this, typeof(VagRKDSPage));
			this.lv = NameScopeExtensions.FindByName<ListView>(this, "lv");
			this.entryPassword = NameScopeExtensions.FindByName<Entry>(this, "entryPassword");
			this.btnSaveNewValue = NameScopeExtensions.FindByName<Button>(this, "btnSaveNewValue");
			this.btnPickODIS = NameScopeExtensions.FindByName<Button>(this, "btnPickODIS");
			this.switchIndividual = NameScopeExtensions.FindByName<LabelSwitch>(this, "switchIndividual");
			this.switchComfort = NameScopeExtensions.FindByName<LabelSwitch>(this, "switchComfort");
			this.activityFrame = NameScopeExtensions.FindByName<ActivityFrame>(this, "activityFrame");
		}

		// Token: 0x04002FDB RID: 12251
		[CompilerGenerated]
		private RKDSDatasetCoding <Coding>k__BackingField;

		// Token: 0x04002FDC RID: 12252
		private bool first_appearing = true;

		// Token: 0x04002FDD RID: 12253
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private ListView lv;

		// Token: 0x04002FDE RID: 12254
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private Entry entryPassword;

		// Token: 0x04002FDF RID: 12255
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private Button btnSaveNewValue;

		// Token: 0x04002FE0 RID: 12256
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private Button btnPickODIS;

		// Token: 0x04002FE1 RID: 12257
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private LabelSwitch switchIndividual;

		// Token: 0x04002FE2 RID: 12258
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private LabelSwitch switchComfort;

		// Token: 0x04002FE3 RID: 12259
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private ActivityFrame activityFrame;

		// Token: 0x02000978 RID: 2424
		[CompilerGenerated]
		private sealed class <>c__DisplayClass10_0
		{
			// Token: 0x06004FD8 RID: 20440 RVA: 0x00002050 File Offset: 0x00000250
			public <>c__DisplayClass10_0()
			{
			}

			// Token: 0x06004FD9 RID: 20441 RVA: 0x003D1DA9 File Offset: 0x003CFFA9
			internal void <btnPickODISXml_Clicked>b__1()
			{
				this.<>4__this.activityFrame.Text = this.s;
			}

			// Token: 0x04002FE4 RID: 12260
			public string s;

			// Token: 0x04002FE5 RID: 12261
			public VagRKDSPage <>4__this;
		}

		// Token: 0x02000979 RID: 2425
		[CompilerGenerated]
		private sealed class <>c__DisplayClass9_0
		{
			// Token: 0x06004FDA RID: 20442 RVA: 0x00002050 File Offset: 0x00000250
			public <>c__DisplayClass9_0()
			{
			}

			// Token: 0x06004FDB RID: 20443 RVA: 0x003D1DC1 File Offset: 0x003CFFC1
			internal void <btnSaveState_Clicked>b__1()
			{
				this.<>4__this.activityFrame.Text = this.s;
			}

			// Token: 0x04002FE6 RID: 12262
			public string s;

			// Token: 0x04002FE7 RID: 12263
			public VagRKDSPage <>4__this;
		}

		// Token: 0x0200097A RID: 2426
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <BtnUpdateState_Clicked>d__7 : IAsyncStateMachine
		{
			// Token: 0x06004FDC RID: 20444 RVA: 0x003D1DDC File Offset: 0x003CFFDC
			void IAsyncStateMachine.MoveNext()
			{
				VagRKDSPage vagRKDSPage = this;
				try
				{
					vagRKDSPage.UpdateState();
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

			// Token: 0x06004FDD RID: 20445 RVA: 0x003D1E34 File Offset: 0x003D0034
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x04002FE8 RID: 12264
			public int <>1__state;

			// Token: 0x04002FE9 RID: 12265
			public AsyncVoidMethodBuilder <>t__builder;

			// Token: 0x04002FEA RID: 12266
			public VagRKDSPage <>4__this;
		}

		// Token: 0x0200097B RID: 2427
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <UpdateState>d__8 : IAsyncStateMachine
		{
			// Token: 0x06004FDE RID: 20446 RVA: 0x003D1E44 File Offset: 0x003D0044
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				VagRKDSPage vagRKDSPage = this;
				try
				{
					TaskAwaiter<CodingRequestResult> taskAwaiter;
					if (num != 0)
					{
						vagRKDSPage.activityFrame.IsVisible = true;
						taskAwaiter = vagRKDSPage.Coding.UpdateCurrentState("", null).GetAwaiter();
						if (!taskAwaiter.IsCompleted)
						{
							num2 = 0;
							TaskAwaiter<CodingRequestResult> taskAwaiter2 = taskAwaiter;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<CodingRequestResult>, VagRKDSPage.<UpdateState>d__8>(ref taskAwaiter, ref this);
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
					vagRKDSPage.activityFrame.IsVisible = false;
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

			// Token: 0x06004FDF RID: 20447 RVA: 0x003D1F1C File Offset: 0x003D011C
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x04002FEB RID: 12267
			public int <>1__state;

			// Token: 0x04002FEC RID: 12268
			public AsyncTaskMethodBuilder <>t__builder;

			// Token: 0x04002FED RID: 12269
			public VagRKDSPage <>4__this;

			// Token: 0x04002FEE RID: 12270
			private TaskAwaiter<CodingRequestResult> <>u__1;
		}

		// Token: 0x0200097C RID: 2428
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <VagRKDSPage_Appearing>d__1 : IAsyncStateMachine
		{
			// Token: 0x06004FE0 RID: 20448 RVA: 0x003D1F2C File Offset: 0x003D012C
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				VagRKDSPage vagRKDSPage = this;
				try
				{
					TaskAwaiter<CodingRequestResult> taskAwaiter;
					if (num != 0)
					{
						if (!vagRKDSPage.first_appearing)
						{
							goto IL_00B7;
						}
						vagRKDSPage.first_appearing = false;
						vagRKDSPage.lv.IsEnabled = false;
						vagRKDSPage.activityFrame.IsVisible = true;
						taskAwaiter = vagRKDSPage.Coding.UpdateCurrentState("", null).GetAwaiter();
						if (!taskAwaiter.IsCompleted)
						{
							num2 = 0;
							TaskAwaiter<CodingRequestResult> taskAwaiter2 = taskAwaiter;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<CodingRequestResult>, VagRKDSPage.<VagRKDSPage_Appearing>d__1>(ref taskAwaiter, ref this);
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
					vagRKDSPage.activityFrame.IsVisible = false;
					vagRKDSPage.lv.IsEnabled = true;
					IL_00B7:;
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

			// Token: 0x06004FE1 RID: 20449 RVA: 0x003D202C File Offset: 0x003D022C
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x04002FEF RID: 12271
			public int <>1__state;

			// Token: 0x04002FF0 RID: 12272
			public AsyncVoidMethodBuilder <>t__builder;

			// Token: 0x04002FF1 RID: 12273
			public VagRKDSPage <>4__this;

			// Token: 0x04002FF2 RID: 12274
			private TaskAwaiter<CodingRequestResult> <>u__1;
		}

		// Token: 0x0200097D RID: 2429
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <btnPickODISXml_Clicked>d__10 : IAsyncStateMachine
		{
			// Token: 0x06004FE2 RID: 20450 RVA: 0x003D203C File Offset: 0x003D023C
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				VagRKDSPage vagRKDSPage = this;
				try
				{
					TaskAwaiter<bool> taskAwaiter3;
					TaskAwaiter taskAwaiter4;
					TaskAwaiter<string> taskAwaiter6;
					string result;
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
						goto IL_0133;
					}
					case 2:
						taskAwaiter3 = taskAwaiter2;
						taskAwaiter2 = default(TaskAwaiter<bool>);
						num2 = -1;
						goto IL_01EA;
					case 3:
					{
						TaskAwaiter taskAwaiter5;
						taskAwaiter4 = taskAwaiter5;
						taskAwaiter5 = default(TaskAwaiter);
						num2 = -1;
						goto IL_0258;
					}
					case 4:
					{
						TaskAwaiter<string> taskAwaiter7;
						taskAwaiter6 = taskAwaiter7;
						taskAwaiter7 = default(TaskAwaiter<string>);
						num = (num2 = -1);
						goto IL_02C0;
					}
					case 5:
					case 6:
					case 7:
						IL_02CE:
						try
						{
							TaskAwaiter<CodingRequestResult> taskAwaiter8;
							switch (num)
							{
							case 5:
							{
								TaskAwaiter<CodingRequestResult> taskAwaiter9;
								taskAwaiter8 = taskAwaiter9;
								taskAwaiter9 = default(TaskAwaiter<CodingRequestResult>);
								num2 = -1;
								break;
							}
							case 6:
							{
								TaskAwaiter taskAwaiter5;
								taskAwaiter4 = taskAwaiter5;
								taskAwaiter5 = default(TaskAwaiter);
								num2 = -1;
								goto IL_042C;
							}
							case 7:
							{
								TaskAwaiter taskAwaiter5;
								taskAwaiter4 = taskAwaiter5;
								taskAwaiter5 = default(TaskAwaiter);
								num2 = -1;
								goto IL_049F;
							}
							default:
							{
								string text = vagRKDSPage.Coding.ODISXmlToHex(result);
								if (text == null)
								{
									goto IL_04CE;
								}
								vagRKDSPage.activityFrame.IsVisible = true;
								vagRKDSPage.lv.IsEnabled = false;
								Progress<string> progress = new Progress<string>(delegate(string s)
								{
									Device.BeginInvokeOnMainThread(new Action(new VagRKDSPage.<>c__DisplayClass10_0
									{
										<>4__this = vagRKDSPage,
										s = s
									}.<btnPickODISXml_Clicked>b__1));
								});
								taskAwaiter8 = vagRKDSPage.Coding.Execute(vagRKDSPage.entryPassword.Text, text, "", progress, null, false).GetAwaiter();
								if (!taskAwaiter8.IsCompleted)
								{
									num2 = 5;
									TaskAwaiter<CodingRequestResult> taskAwaiter9 = taskAwaiter8;
									this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<CodingRequestResult>, VagRKDSPage.<btnPickODISXml_Clicked>d__10>(ref taskAwaiter8, ref this);
									return;
								}
								break;
							}
							}
							CodingRequestResult result2 = taskAwaiter8.GetResult();
							if (result2 == CodingRequestResult.Success)
							{
								SharedSettings sharedSettings = SharedSettings.Current;
								int codingsCounter = sharedSettings.CodingsCounter;
								sharedSettings.CodingsCounter = codingsCounter + 1;
								taskAwaiter4 = vagRKDSPage.DisplayAlert(vagRKDSPage.Coding.Name, Translate.GetString("coding_OperationFinished"), "OK").GetAwaiter();
								if (!taskAwaiter4.IsCompleted)
								{
									num2 = 6;
									TaskAwaiter taskAwaiter5 = taskAwaiter4;
									this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, VagRKDSPage.<btnPickODISXml_Clicked>d__10>(ref taskAwaiter4, ref this);
									return;
								}
							}
							else
							{
								taskAwaiter4 = vagRKDSPage.DisplayAlert(Translate.GetString("coding_Error"), MQBAdaptationTemplate.CodingRequestResultToString(result2), "OK").GetAwaiter();
								if (!taskAwaiter4.IsCompleted)
								{
									num2 = 7;
									TaskAwaiter taskAwaiter5 = taskAwaiter4;
									this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, VagRKDSPage.<btnPickODISXml_Clicked>d__10>(ref taskAwaiter4, ref this);
									return;
								}
								goto IL_049F;
							}
							IL_042C:
							taskAwaiter4.GetResult();
							goto IL_04A6;
							IL_049F:
							taskAwaiter4.GetResult();
							IL_04A6:
							vagRKDSPage.activityFrame.IsVisible = false;
							vagRKDSPage.activityFrame.Text = ActivityFrame.PLEASE_WAIT_TEXT;
							vagRKDSPage.lv.IsEnabled = true;
							IL_04CE:;
						}
						catch (Exception)
						{
						}
						goto IL_04D3;
					default:
						if (vagRKDSPage.Coding.RequiresPro && !SharedSettings.Current.AdsProductPurchased)
						{
							taskAwaiter3 = vagRKDSPage.DisplayAlert("Car Scanner Pro", Translate.GetString("FeatureLocked"), "OK", Translate.GetString("btnCancel.Content")).GetAwaiter();
							if (!taskAwaiter3.IsCompleted)
							{
								num2 = 0;
								taskAwaiter2 = taskAwaiter3;
								this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<bool>, VagRKDSPage.<btnPickODISXml_Clicked>d__10>(ref taskAwaiter3, ref this);
								return;
							}
						}
						else if (SharedSettings.Current.CodingsCounter >= 3 && !SharedSettings.Current.AdsProductPurchased)
						{
							taskAwaiter3 = vagRKDSPage.DisplayAlert("Car Scanner Pro", string.Format(Translate.GetString("coding_FreeCodingsFinished"), SharedSettings.Current.CodingsCounter, 3), "OK", Translate.GetString("btnCancel.Content")).GetAwaiter();
							if (!taskAwaiter3.IsCompleted)
							{
								num2 = 2;
								taskAwaiter2 = taskAwaiter3;
								this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<bool>, VagRKDSPage.<btnPickODISXml_Clicked>d__10>(ref taskAwaiter3, ref this);
								return;
							}
							goto IL_01EA;
						}
						else
						{
							taskAwaiter6 = vagRKDSPage.Coding.PickFileAsync().GetAwaiter();
							if (!taskAwaiter6.IsCompleted)
							{
								num2 = 4;
								TaskAwaiter<string> taskAwaiter7 = taskAwaiter6;
								this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<string>, VagRKDSPage.<btnPickODISXml_Clicked>d__10>(ref taskAwaiter6, ref this);
								return;
							}
							goto IL_02C0;
						}
						break;
					}
					if (!taskAwaiter3.GetResult())
					{
						goto IL_013A;
					}
					Page inAppPage = InAppManager.GetInAppPage();
					taskAwaiter4 = vagRKDSPage.Navigation.PushAsync(inAppPage).GetAwaiter();
					if (!taskAwaiter4.IsCompleted)
					{
						num2 = 1;
						TaskAwaiter taskAwaiter5 = taskAwaiter4;
						this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, VagRKDSPage.<btnPickODISXml_Clicked>d__10>(ref taskAwaiter4, ref this);
						return;
					}
					IL_0133:
					taskAwaiter4.GetResult();
					IL_013A:
					goto IL_04EE;
					IL_01EA:
					if (!taskAwaiter3.GetResult())
					{
						goto IL_025F;
					}
					Page inAppPage2 = InAppManager.GetInAppPage();
					taskAwaiter4 = vagRKDSPage.Navigation.PushAsync(inAppPage2).GetAwaiter();
					if (!taskAwaiter4.IsCompleted)
					{
						num2 = 3;
						TaskAwaiter taskAwaiter5 = taskAwaiter4;
						this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, VagRKDSPage.<btnPickODISXml_Clicked>d__10>(ref taskAwaiter4, ref this);
						return;
					}
					IL_0258:
					taskAwaiter4.GetResult();
					IL_025F:
					goto IL_04EE;
					IL_02C0:
					result = taskAwaiter6.GetResult();
					if (result != null)
					{
						goto IL_02CE;
					}
					IL_04D3:;
				}
				catch (Exception ex)
				{
					num2 = -2;
					this.<>t__builder.SetException(ex);
					return;
				}
				IL_04EE:
				num2 = -2;
				this.<>t__builder.SetResult();
			}

			// Token: 0x06004FE3 RID: 20451 RVA: 0x003D2580 File Offset: 0x003D0780
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x04002FF3 RID: 12275
			public int <>1__state;

			// Token: 0x04002FF4 RID: 12276
			public AsyncVoidMethodBuilder <>t__builder;

			// Token: 0x04002FF5 RID: 12277
			public VagRKDSPage <>4__this;

			// Token: 0x04002FF6 RID: 12278
			private TaskAwaiter<bool> <>u__1;

			// Token: 0x04002FF7 RID: 12279
			private TaskAwaiter <>u__2;

			// Token: 0x04002FF8 RID: 12280
			private TaskAwaiter<string> <>u__3;

			// Token: 0x04002FF9 RID: 12281
			private TaskAwaiter<CodingRequestResult> <>u__4;
		}

		// Token: 0x0200097E RID: 2430
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <btnSaveState_Clicked>d__9 : IAsyncStateMachine
		{
			// Token: 0x06004FE4 RID: 20452 RVA: 0x003D2590 File Offset: 0x003D0790
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				VagRKDSPage vagRKDSPage = this;
				try
				{
					TaskAwaiter<bool> taskAwaiter3;
					TaskAwaiter taskAwaiter4;
					TaskAwaiter<CodingRequestResult> taskAwaiter6;
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
						goto IL_0132;
					}
					case 2:
						taskAwaiter3 = taskAwaiter2;
						taskAwaiter2 = default(TaskAwaiter<bool>);
						num2 = -1;
						goto IL_01EC;
					case 3:
					{
						TaskAwaiter taskAwaiter5;
						taskAwaiter4 = taskAwaiter5;
						taskAwaiter5 = default(TaskAwaiter);
						num2 = -1;
						goto IL_025A;
					}
					case 4:
					{
						TaskAwaiter<CodingRequestResult> taskAwaiter7;
						taskAwaiter6 = taskAwaiter7;
						taskAwaiter7 = default(TaskAwaiter<CodingRequestResult>);
						num2 = -1;
						goto IL_0304;
					}
					case 5:
					{
						TaskAwaiter taskAwaiter5;
						taskAwaiter4 = taskAwaiter5;
						taskAwaiter5 = default(TaskAwaiter);
						num2 = -1;
						goto IL_0399;
					}
					case 6:
					{
						TaskAwaiter taskAwaiter5;
						taskAwaiter4 = taskAwaiter5;
						taskAwaiter5 = default(TaskAwaiter);
						num2 = -1;
						goto IL_040B;
					}
					default:
						if (vagRKDSPage.Coding.RequiresPro && !SharedSettings.Current.AdsProductPurchased)
						{
							taskAwaiter3 = vagRKDSPage.DisplayAlert("Car Scanner Pro", Translate.GetString("FeatureLocked"), "OK", Translate.GetString("btnCancel.Content")).GetAwaiter();
							if (!taskAwaiter3.IsCompleted)
							{
								num2 = 0;
								taskAwaiter2 = taskAwaiter3;
								this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<bool>, VagRKDSPage.<btnSaveState_Clicked>d__9>(ref taskAwaiter3, ref this);
								return;
							}
						}
						else if (SharedSettings.Current.CodingsCounter >= 3 && !SharedSettings.Current.AdsProductPurchased)
						{
							taskAwaiter3 = vagRKDSPage.DisplayAlert("Car Scanner Pro", string.Format(Translate.GetString("coding_FreeCodingsFinished"), SharedSettings.Current.CodingsCounter, 3), "OK", Translate.GetString("btnCancel.Content")).GetAwaiter();
							if (!taskAwaiter3.IsCompleted)
							{
								num2 = 2;
								taskAwaiter2 = taskAwaiter3;
								this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<bool>, VagRKDSPage.<btnSaveState_Clicked>d__9>(ref taskAwaiter3, ref this);
								return;
							}
							goto IL_01EC;
						}
						else
						{
							vagRKDSPage.activityFrame.IsVisible = true;
							vagRKDSPage.lv.IsEnabled = false;
							Progress<string> progress = new Progress<string>(delegate(string s)
							{
								Device.BeginInvokeOnMainThread(new Action(new VagRKDSPage.<>c__DisplayClass9_0
								{
									<>4__this = vagRKDSPage,
									s = s
								}.<btnSaveState_Clicked>b__1));
							});
							taskAwaiter6 = vagRKDSPage.Coding.Execute(vagRKDSPage.entryPassword.Text, "", "", progress, null, false).GetAwaiter();
							if (!taskAwaiter6.IsCompleted)
							{
								num2 = 4;
								TaskAwaiter<CodingRequestResult> taskAwaiter7 = taskAwaiter6;
								this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<CodingRequestResult>, VagRKDSPage.<btnSaveState_Clicked>d__9>(ref taskAwaiter6, ref this);
								return;
							}
							goto IL_0304;
						}
						break;
					}
					if (!taskAwaiter3.GetResult())
					{
						goto IL_0139;
					}
					Page inAppPage = InAppManager.GetInAppPage();
					taskAwaiter4 = vagRKDSPage.Navigation.PushAsync(inAppPage).GetAwaiter();
					if (!taskAwaiter4.IsCompleted)
					{
						num2 = 1;
						TaskAwaiter taskAwaiter5 = taskAwaiter4;
						this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, VagRKDSPage.<btnSaveState_Clicked>d__9>(ref taskAwaiter4, ref this);
						return;
					}
					IL_0132:
					taskAwaiter4.GetResult();
					IL_0139:
					goto IL_0455;
					IL_01EC:
					if (!taskAwaiter3.GetResult())
					{
						goto IL_0261;
					}
					Page inAppPage2 = InAppManager.GetInAppPage();
					taskAwaiter4 = vagRKDSPage.Navigation.PushAsync(inAppPage2).GetAwaiter();
					if (!taskAwaiter4.IsCompleted)
					{
						num2 = 3;
						TaskAwaiter taskAwaiter5 = taskAwaiter4;
						this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, VagRKDSPage.<btnSaveState_Clicked>d__9>(ref taskAwaiter4, ref this);
						return;
					}
					IL_025A:
					taskAwaiter4.GetResult();
					IL_0261:
					goto IL_0455;
					IL_0304:
					CodingRequestResult result = taskAwaiter6.GetResult();
					if (result == CodingRequestResult.Success)
					{
						SharedSettings sharedSettings = SharedSettings.Current;
						int codingsCounter = sharedSettings.CodingsCounter;
						sharedSettings.CodingsCounter = codingsCounter + 1;
						taskAwaiter4 = vagRKDSPage.DisplayAlert(vagRKDSPage.Coding.Name, Translate.GetString("coding_OperationFinished"), "OK").GetAwaiter();
						if (!taskAwaiter4.IsCompleted)
						{
							num2 = 5;
							TaskAwaiter taskAwaiter5 = taskAwaiter4;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, VagRKDSPage.<btnSaveState_Clicked>d__9>(ref taskAwaiter4, ref this);
							return;
						}
					}
					else
					{
						taskAwaiter4 = vagRKDSPage.DisplayAlert(Translate.GetString("coding_Error"), MQBAdaptationTemplate.CodingRequestResultToString(result), "OK").GetAwaiter();
						if (!taskAwaiter4.IsCompleted)
						{
							num2 = 6;
							TaskAwaiter taskAwaiter5 = taskAwaiter4;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, VagRKDSPage.<btnSaveState_Clicked>d__9>(ref taskAwaiter4, ref this);
							return;
						}
						goto IL_040B;
					}
					IL_0399:
					taskAwaiter4.GetResult();
					goto IL_0412;
					IL_040B:
					taskAwaiter4.GetResult();
					IL_0412:
					vagRKDSPage.activityFrame.IsVisible = false;
					vagRKDSPage.activityFrame.Text = ActivityFrame.PLEASE_WAIT_TEXT;
					vagRKDSPage.lv.IsEnabled = true;
				}
				catch (Exception ex)
				{
					num2 = -2;
					this.<>t__builder.SetException(ex);
					return;
				}
				IL_0455:
				num2 = -2;
				this.<>t__builder.SetResult();
			}

			// Token: 0x06004FE5 RID: 20453 RVA: 0x003D2A24 File Offset: 0x003D0C24
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x04002FFA RID: 12282
			public int <>1__state;

			// Token: 0x04002FFB RID: 12283
			public AsyncVoidMethodBuilder <>t__builder;

			// Token: 0x04002FFC RID: 12284
			public VagRKDSPage <>4__this;

			// Token: 0x04002FFD RID: 12285
			private TaskAwaiter<bool> <>u__1;

			// Token: 0x04002FFE RID: 12286
			private TaskAwaiter <>u__2;

			// Token: 0x04002FFF RID: 12287
			private TaskAwaiter<CodingRequestResult> <>u__3;
		}

		// Token: 0x0200097F RID: 2431
		[CompilerGenerated]
		private sealed class <InitializeComponent>_anonXamlCDataTemplate_15
		{
			// Token: 0x06004FE6 RID: 20454 RVA: 0x003D2A34 File Offset: 0x003D0C34
			public <InitializeComponent>_anonXamlCDataTemplate_15()
			{
			}

			// Token: 0x06004FE7 RID: 20455 RVA: 0x003D2A48 File Offset: 0x003D0C48
			internal object LoadDataTemplate()
			{
				ColumnDefinition columnDefinition;
				VisualDiagnostics.RegisterSourceInfo(columnDefinition = new ColumnDefinition(), new Uri("Coding\\Pages\\VagRKDSPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 157, 38);
				ColumnDefinition columnDefinition2;
				VisualDiagnostics.RegisterSourceInfo(columnDefinition2 = new ColumnDefinition(), new Uri("Coding\\Pages\\VagRKDSPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 158, 38);
				ColumnDefinition columnDefinition3;
				VisualDiagnostics.RegisterSourceInfo(columnDefinition3 = new ColumnDefinition(), new Uri("Coding\\Pages\\VagRKDSPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 159, 38);
				RowDefinition rowDefinition;
				VisualDiagnostics.RegisterSourceInfo(rowDefinition = new RowDefinition(), new Uri("Coding\\Pages\\VagRKDSPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 162, 38);
				RowDefinition rowDefinition2;
				VisualDiagnostics.RegisterSourceInfo(rowDefinition2 = new RowDefinition(), new Uri("Coding\\Pages\\VagRKDSPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 163, 38);
				RowDefinition rowDefinition3;
				VisualDiagnostics.RegisterSourceInfo(rowDefinition3 = new RowDefinition(), new Uri("Coding\\Pages\\VagRKDSPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 164, 38);
				RowDefinition rowDefinition4;
				VisualDiagnostics.RegisterSourceInfo(rowDefinition4 = new RowDefinition(), new Uri("Coding\\Pages\\VagRKDSPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 165, 38);
				RowDefinition rowDefinition5;
				VisualDiagnostics.RegisterSourceInfo(rowDefinition5 = new RowDefinition(), new Uri("Coding\\Pages\\VagRKDSPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 166, 38);
				Translate translate;
				VisualDiagnostics.RegisterSourceInfo(translate = new Translate(), new Uri("Coding\\Pages\\VagRKDSPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 172, 37);
				Label label;
				VisualDiagnostics.RegisterSourceInfo(label = new Label(), new Uri("Coding\\Pages\\VagRKDSPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 169, 34);
				BindingExtension bindingExtension;
				VisualDiagnostics.RegisterSourceInfo(bindingExtension = new BindingExtension(), new Uri("Coding\\Pages\\VagRKDSPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 178, 37);
				Entry entry;
				VisualDiagnostics.RegisterSourceInfo(entry = new Entry(), new Uri("Coding\\Pages\\VagRKDSPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 174, 34);
				Translate translate2;
				VisualDiagnostics.RegisterSourceInfo(translate2 = new Translate(), new Uri("Coding\\Pages\\VagRKDSPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 184, 37);
				Label label2;
				VisualDiagnostics.RegisterSourceInfo(label2 = new Label(), new Uri("Coding\\Pages\\VagRKDSPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 181, 34);
				Translate translate3;
				VisualDiagnostics.RegisterSourceInfo(translate3 = new Translate(), new Uri("Coding\\Pages\\VagRKDSPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 189, 37);
				Label label3;
				VisualDiagnostics.RegisterSourceInfo(label3 = new Label(), new Uri("Coding\\Pages\\VagRKDSPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 186, 34);
				Translate translate4;
				VisualDiagnostics.RegisterSourceInfo(translate4 = new Translate(), new Uri("Coding\\Pages\\VagRKDSPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 194, 37);
				Label label4;
				VisualDiagnostics.RegisterSourceInfo(label4 = new Label(), new Uri("Coding\\Pages\\VagRKDSPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 191, 34);
				Translate translate5;
				VisualDiagnostics.RegisterSourceInfo(translate5 = new Translate(), new Uri("Coding\\Pages\\VagRKDSPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 199, 37);
				Label label5;
				VisualDiagnostics.RegisterSourceInfo(label5 = new Label(), new Uri("Coding\\Pages\\VagRKDSPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 196, 34);
				StaticResourceExtension staticResourceExtension;
				VisualDiagnostics.RegisterSourceInfo(staticResourceExtension = new StaticResourceExtension(), new Uri("Coding\\Pages\\VagRKDSPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 204, 37);
				StaticResourceExtension staticResourceExtension2;
				VisualDiagnostics.RegisterSourceInfo(staticResourceExtension2 = new StaticResourceExtension(), new Uri("Coding\\Pages\\VagRKDSPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 205, 37);
				StaticResourceExtension staticResourceExtension3;
				VisualDiagnostics.RegisterSourceInfo(staticResourceExtension3 = new StaticResourceExtension(), new Uri("Coding\\Pages\\VagRKDSPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 205, 37);
				BindingExtension bindingExtension2;
				VisualDiagnostics.RegisterSourceInfo(bindingExtension2 = new BindingExtension(), new Uri("Coding\\Pages\\VagRKDSPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 205, 37);
				Picker picker;
				VisualDiagnostics.RegisterSourceInfo(picker = new Picker(), new Uri("Coding\\Pages\\VagRKDSPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 201, 34);
				StaticResourceExtension staticResourceExtension4;
				VisualDiagnostics.RegisterSourceInfo(staticResourceExtension4 = new StaticResourceExtension(), new Uri("Coding\\Pages\\VagRKDSPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 210, 37);
				StaticResourceExtension staticResourceExtension5;
				VisualDiagnostics.RegisterSourceInfo(staticResourceExtension5 = new StaticResourceExtension(), new Uri("Coding\\Pages\\VagRKDSPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 211, 37);
				StaticResourceExtension staticResourceExtension6;
				VisualDiagnostics.RegisterSourceInfo(staticResourceExtension6 = new StaticResourceExtension(), new Uri("Coding\\Pages\\VagRKDSPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 211, 37);
				BindingExtension bindingExtension3;
				VisualDiagnostics.RegisterSourceInfo(bindingExtension3 = new BindingExtension(), new Uri("Coding\\Pages\\VagRKDSPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 211, 37);
				Picker picker2;
				VisualDiagnostics.RegisterSourceInfo(picker2 = new Picker(), new Uri("Coding\\Pages\\VagRKDSPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 207, 34);
				Translate translate6;
				VisualDiagnostics.RegisterSourceInfo(translate6 = new Translate(), new Uri("Coding\\Pages\\VagRKDSPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 216, 37);
				Label label6;
				VisualDiagnostics.RegisterSourceInfo(label6 = new Label(), new Uri("Coding\\Pages\\VagRKDSPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 213, 34);
				StaticResourceExtension staticResourceExtension7;
				VisualDiagnostics.RegisterSourceInfo(staticResourceExtension7 = new StaticResourceExtension(), new Uri("Coding\\Pages\\VagRKDSPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 221, 37);
				StaticResourceExtension staticResourceExtension8;
				VisualDiagnostics.RegisterSourceInfo(staticResourceExtension8 = new StaticResourceExtension(), new Uri("Coding\\Pages\\VagRKDSPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 222, 37);
				StaticResourceExtension staticResourceExtension9;
				VisualDiagnostics.RegisterSourceInfo(staticResourceExtension9 = new StaticResourceExtension(), new Uri("Coding\\Pages\\VagRKDSPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 222, 37);
				BindingExtension bindingExtension4;
				VisualDiagnostics.RegisterSourceInfo(bindingExtension4 = new BindingExtension(), new Uri("Coding\\Pages\\VagRKDSPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 222, 37);
				Picker picker3;
				VisualDiagnostics.RegisterSourceInfo(picker3 = new Picker(), new Uri("Coding\\Pages\\VagRKDSPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 218, 34);
				StaticResourceExtension staticResourceExtension10;
				VisualDiagnostics.RegisterSourceInfo(staticResourceExtension10 = new StaticResourceExtension(), new Uri("Coding\\Pages\\VagRKDSPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 227, 37);
				StaticResourceExtension staticResourceExtension11;
				VisualDiagnostics.RegisterSourceInfo(staticResourceExtension11 = new StaticResourceExtension(), new Uri("Coding\\Pages\\VagRKDSPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 228, 37);
				StaticResourceExtension staticResourceExtension12;
				VisualDiagnostics.RegisterSourceInfo(staticResourceExtension12 = new StaticResourceExtension(), new Uri("Coding\\Pages\\VagRKDSPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 228, 37);
				BindingExtension bindingExtension5;
				VisualDiagnostics.RegisterSourceInfo(bindingExtension5 = new BindingExtension(), new Uri("Coding\\Pages\\VagRKDSPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 228, 37);
				Picker picker4;
				VisualDiagnostics.RegisterSourceInfo(picker4 = new Picker(), new Uri("Coding\\Pages\\VagRKDSPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 224, 34);
				ReferenceExtension referenceExtension;
				VisualDiagnostics.RegisterSourceInfo(referenceExtension = new ReferenceExtension(), new Uri("Coding\\Pages\\VagRKDSPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 234, 37);
				BindingExtension bindingExtension6;
				VisualDiagnostics.RegisterSourceInfo(bindingExtension6 = new BindingExtension(), new Uri("Coding\\Pages\\VagRKDSPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 234, 37);
				Translate translate7;
				VisualDiagnostics.RegisterSourceInfo(translate7 = new Translate(), new Uri("Coding\\Pages\\VagRKDSPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 235, 37);
				Label label7;
				VisualDiagnostics.RegisterSourceInfo(label7 = new Label(), new Uri("Coding\\Pages\\VagRKDSPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 231, 34);
				ReferenceExtension referenceExtension2;
				VisualDiagnostics.RegisterSourceInfo(referenceExtension2 = new ReferenceExtension(), new Uri("Coding\\Pages\\VagRKDSPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 240, 37);
				BindingExtension bindingExtension7;
				VisualDiagnostics.RegisterSourceInfo(bindingExtension7 = new BindingExtension(), new Uri("Coding\\Pages\\VagRKDSPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 240, 37);
				StaticResourceExtension staticResourceExtension13;
				VisualDiagnostics.RegisterSourceInfo(staticResourceExtension13 = new StaticResourceExtension(), new Uri("Coding\\Pages\\VagRKDSPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 241, 37);
				StaticResourceExtension staticResourceExtension14;
				VisualDiagnostics.RegisterSourceInfo(staticResourceExtension14 = new StaticResourceExtension(), new Uri("Coding\\Pages\\VagRKDSPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 242, 37);
				StaticResourceExtension staticResourceExtension15;
				VisualDiagnostics.RegisterSourceInfo(staticResourceExtension15 = new StaticResourceExtension(), new Uri("Coding\\Pages\\VagRKDSPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 242, 37);
				BindingExtension bindingExtension8;
				VisualDiagnostics.RegisterSourceInfo(bindingExtension8 = new BindingExtension(), new Uri("Coding\\Pages\\VagRKDSPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 242, 37);
				Picker picker5;
				VisualDiagnostics.RegisterSourceInfo(picker5 = new Picker(), new Uri("Coding\\Pages\\VagRKDSPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 237, 34);
				ReferenceExtension referenceExtension3;
				VisualDiagnostics.RegisterSourceInfo(referenceExtension3 = new ReferenceExtension(), new Uri("Coding\\Pages\\VagRKDSPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 247, 37);
				BindingExtension bindingExtension9;
				VisualDiagnostics.RegisterSourceInfo(bindingExtension9 = new BindingExtension(), new Uri("Coding\\Pages\\VagRKDSPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 247, 37);
				StaticResourceExtension staticResourceExtension16;
				VisualDiagnostics.RegisterSourceInfo(staticResourceExtension16 = new StaticResourceExtension(), new Uri("Coding\\Pages\\VagRKDSPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 248, 37);
				StaticResourceExtension staticResourceExtension17;
				VisualDiagnostics.RegisterSourceInfo(staticResourceExtension17 = new StaticResourceExtension(), new Uri("Coding\\Pages\\VagRKDSPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 249, 37);
				StaticResourceExtension staticResourceExtension18;
				VisualDiagnostics.RegisterSourceInfo(staticResourceExtension18 = new StaticResourceExtension(), new Uri("Coding\\Pages\\VagRKDSPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 249, 37);
				BindingExtension bindingExtension10;
				VisualDiagnostics.RegisterSourceInfo(bindingExtension10 = new BindingExtension(), new Uri("Coding\\Pages\\VagRKDSPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 249, 37);
				Picker picker6;
				VisualDiagnostics.RegisterSourceInfo(picker6 = new Picker(), new Uri("Coding\\Pages\\VagRKDSPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 244, 34);
				Grid grid;
				VisualDiagnostics.RegisterSourceInfo(grid = new Grid(), new Uri("Coding\\Pages\\VagRKDSPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 155, 30);
				ViewCell viewCell;
				VisualDiagnostics.RegisterSourceInfo(viewCell = new ViewCell(), new Uri("Coding\\Pages\\VagRKDSPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 154, 26);
				NameScope nameScope = new NameScope();
				NameScope.SetNameScope(viewCell, nameScope);
				columnDefinition.SetValue(ColumnDefinition.WidthProperty, new GridLengthTypeConverter().ConvertFromInvariantString("*"));
				grid.GetValue(Grid.ColumnDefinitionsProperty).Add(columnDefinition);
				columnDefinition2.SetValue(ColumnDefinition.WidthProperty, new GridLengthTypeConverter().ConvertFromInvariantString("*"));
				grid.GetValue(Grid.ColumnDefinitionsProperty).Add(columnDefinition2);
				columnDefinition3.SetValue(ColumnDefinition.WidthProperty, new GridLengthTypeConverter().ConvertFromInvariantString("*"));
				grid.GetValue(Grid.ColumnDefinitionsProperty).Add(columnDefinition3);
				rowDefinition.SetValue(RowDefinition.HeightProperty, new GridLengthTypeConverter().ConvertFromInvariantString("auto"));
				grid.GetValue(Grid.RowDefinitionsProperty).Add(rowDefinition);
				rowDefinition2.SetValue(RowDefinition.HeightProperty, new GridLengthTypeConverter().ConvertFromInvariantString("auto"));
				grid.GetValue(Grid.RowDefinitionsProperty).Add(rowDefinition2);
				rowDefinition3.SetValue(RowDefinition.HeightProperty, new GridLengthTypeConverter().ConvertFromInvariantString("auto"));
				grid.GetValue(Grid.RowDefinitionsProperty).Add(rowDefinition3);
				rowDefinition4.SetValue(RowDefinition.HeightProperty, new GridLengthTypeConverter().ConvertFromInvariantString("auto"));
				grid.GetValue(Grid.RowDefinitionsProperty).Add(rowDefinition4);
				rowDefinition5.SetValue(RowDefinition.HeightProperty, new GridLengthTypeConverter().ConvertFromInvariantString("auto"));
				grid.GetValue(Grid.RowDefinitionsProperty).Add(rowDefinition5);
				label.SetValue(Grid.RowProperty, 0);
				label.SetValue(Grid.ColumnProperty, 0);
				translate.Text = "codingDB_VagRKDS_TiresetName";
				IMarkupExtension markupExtension = translate;
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
				xamlServiceProvider.Add(typeFromHandle, obj = new SimpleValueTargetProvider(array2, Label.TextProperty, nameScope));
				xamlServiceProvider.Add(typeof(IReferenceProvider), obj);
				Type typeFromHandle2 = typeof(IXamlTypeResolver);
				XmlNamespaceResolver xmlNamespaceResolver = new XmlNamespaceResolver();
				xmlNamespaceResolver.Add("", "http://xamarin.com/schemas/2014/forms");
				xmlNamespaceResolver.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
				xmlNamespaceResolver.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
				xmlNamespaceResolver.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
				xmlNamespaceResolver.Add("local", "clr-namespace:CarScannerXamarinForms");
				xmlNamespaceResolver.Add("settings", "clr-namespace:CarScannerXamarinForms.Settings");
				xmlNamespaceResolver.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
				xamlServiceProvider.Add(typeFromHandle2, new XamlTypeResolver(xmlNamespaceResolver, typeof(VagRKDSPage.<InitializeComponent>_anonXamlCDataTemplate_15).GetTypeInfo().Assembly));
				xamlServiceProvider.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(172, 37)));
				object obj2 = markupExtension.ProvideValue(xamlServiceProvider);
				label.Text = obj2;
				label.SetValue(View.VerticalOptionsProperty, LayoutOptions.Center);
				grid.Children.Add(label);
				entry.SetValue(Grid.RowProperty, 0);
				entry.SetValue(Grid.ColumnProperty, 1);
				entry.SetValue(Grid.ColumnSpanProperty, 2);
				bindingExtension.Mode = 1;
				bindingExtension.Path = "Name";
				BindingBase bindingBase = bindingExtension.ProvideValue(null);
				entry.SetBinding(Entry.TextProperty, bindingBase);
				entry.SetValue(View.VerticalOptionsProperty, LayoutOptions.Center);
				grid.Children.Add(entry);
				label2.SetValue(Grid.RowProperty, 1);
				label2.SetValue(Grid.ColumnProperty, 0);
				translate2.Text = "codingDB_VagRKDS_LoadType";
				IMarkupExtension markupExtension2 = translate2;
				XamlServiceProvider xamlServiceProvider2 = new XamlServiceProvider();
				Type typeFromHandle3 = typeof(IProvideValueTarget);
				int num2;
				object[] array3 = new object[(num2 = this.parentValues.Length) + 3];
				Array.Copy(this.parentValues, 0, array3, 3, num2);
				object[] array4 = array3;
				array4[0] = label2;
				array4[1] = grid;
				array4[2] = viewCell;
				object obj3;
				xamlServiceProvider2.Add(typeFromHandle3, obj3 = new SimpleValueTargetProvider(array4, Label.TextProperty, nameScope));
				xamlServiceProvider2.Add(typeof(IReferenceProvider), obj3);
				Type typeFromHandle4 = typeof(IXamlTypeResolver);
				XmlNamespaceResolver xmlNamespaceResolver2 = new XmlNamespaceResolver();
				xmlNamespaceResolver2.Add("", "http://xamarin.com/schemas/2014/forms");
				xmlNamespaceResolver2.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
				xmlNamespaceResolver2.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
				xmlNamespaceResolver2.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
				xmlNamespaceResolver2.Add("local", "clr-namespace:CarScannerXamarinForms");
				xmlNamespaceResolver2.Add("settings", "clr-namespace:CarScannerXamarinForms.Settings");
				xmlNamespaceResolver2.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
				xamlServiceProvider2.Add(typeFromHandle4, new XamlTypeResolver(xmlNamespaceResolver2, typeof(VagRKDSPage.<InitializeComponent>_anonXamlCDataTemplate_15).GetTypeInfo().Assembly));
				xamlServiceProvider2.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(184, 37)));
				object obj4 = markupExtension2.ProvideValue(xamlServiceProvider2);
				label2.Text = obj4;
				label2.SetValue(View.VerticalOptionsProperty, LayoutOptions.Center);
				grid.Children.Add(label2);
				label3.SetValue(Grid.RowProperty, 1);
				label3.SetValue(Grid.ColumnProperty, 1);
				translate3.Text = "codingDB_VagRKDS_Front";
				IMarkupExtension markupExtension3 = translate3;
				XamlServiceProvider xamlServiceProvider3 = new XamlServiceProvider();
				Type typeFromHandle5 = typeof(IProvideValueTarget);
				int num3;
				object[] array5 = new object[(num3 = this.parentValues.Length) + 3];
				Array.Copy(this.parentValues, 0, array5, 3, num3);
				object[] array6 = array5;
				array6[0] = label3;
				array6[1] = grid;
				array6[2] = viewCell;
				object obj5;
				xamlServiceProvider3.Add(typeFromHandle5, obj5 = new SimpleValueTargetProvider(array6, Label.TextProperty, nameScope));
				xamlServiceProvider3.Add(typeof(IReferenceProvider), obj5);
				Type typeFromHandle6 = typeof(IXamlTypeResolver);
				XmlNamespaceResolver xmlNamespaceResolver3 = new XmlNamespaceResolver();
				xmlNamespaceResolver3.Add("", "http://xamarin.com/schemas/2014/forms");
				xmlNamespaceResolver3.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
				xmlNamespaceResolver3.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
				xmlNamespaceResolver3.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
				xmlNamespaceResolver3.Add("local", "clr-namespace:CarScannerXamarinForms");
				xmlNamespaceResolver3.Add("settings", "clr-namespace:CarScannerXamarinForms.Settings");
				xmlNamespaceResolver3.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
				xamlServiceProvider3.Add(typeFromHandle6, new XamlTypeResolver(xmlNamespaceResolver3, typeof(VagRKDSPage.<InitializeComponent>_anonXamlCDataTemplate_15).GetTypeInfo().Assembly));
				xamlServiceProvider3.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(189, 37)));
				object obj6 = markupExtension3.ProvideValue(xamlServiceProvider3);
				label3.Text = obj6;
				label3.SetValue(View.VerticalOptionsProperty, LayoutOptions.Center);
				grid.Children.Add(label3);
				label4.SetValue(Grid.RowProperty, 1);
				label4.SetValue(Grid.ColumnProperty, 2);
				translate4.Text = "codingDB_VagRKDS_Rear";
				IMarkupExtension markupExtension4 = translate4;
				XamlServiceProvider xamlServiceProvider4 = new XamlServiceProvider();
				Type typeFromHandle7 = typeof(IProvideValueTarget);
				int num4;
				object[] array7 = new object[(num4 = this.parentValues.Length) + 3];
				Array.Copy(this.parentValues, 0, array7, 3, num4);
				object[] array8 = array7;
				array8[0] = label4;
				array8[1] = grid;
				array8[2] = viewCell;
				object obj7;
				xamlServiceProvider4.Add(typeFromHandle7, obj7 = new SimpleValueTargetProvider(array8, Label.TextProperty, nameScope));
				xamlServiceProvider4.Add(typeof(IReferenceProvider), obj7);
				Type typeFromHandle8 = typeof(IXamlTypeResolver);
				XmlNamespaceResolver xmlNamespaceResolver4 = new XmlNamespaceResolver();
				xmlNamespaceResolver4.Add("", "http://xamarin.com/schemas/2014/forms");
				xmlNamespaceResolver4.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
				xmlNamespaceResolver4.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
				xmlNamespaceResolver4.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
				xmlNamespaceResolver4.Add("local", "clr-namespace:CarScannerXamarinForms");
				xmlNamespaceResolver4.Add("settings", "clr-namespace:CarScannerXamarinForms.Settings");
				xmlNamespaceResolver4.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
				xamlServiceProvider4.Add(typeFromHandle8, new XamlTypeResolver(xmlNamespaceResolver4, typeof(VagRKDSPage.<InitializeComponent>_anonXamlCDataTemplate_15).GetTypeInfo().Assembly));
				xamlServiceProvider4.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(194, 37)));
				object obj8 = markupExtension4.ProvideValue(xamlServiceProvider4);
				label4.Text = obj8;
				label4.SetValue(View.VerticalOptionsProperty, LayoutOptions.Center);
				grid.Children.Add(label4);
				label5.SetValue(Grid.RowProperty, 2);
				label5.SetValue(Grid.ColumnProperty, 0);
				translate5.Text = "codingDB_VagRKDS_FullLoad";
				IMarkupExtension markupExtension5 = translate5;
				XamlServiceProvider xamlServiceProvider5 = new XamlServiceProvider();
				Type typeFromHandle9 = typeof(IProvideValueTarget);
				int num5;
				object[] array9 = new object[(num5 = this.parentValues.Length) + 3];
				Array.Copy(this.parentValues, 0, array9, 3, num5);
				object[] array10 = array9;
				array10[0] = label5;
				array10[1] = grid;
				array10[2] = viewCell;
				object obj9;
				xamlServiceProvider5.Add(typeFromHandle9, obj9 = new SimpleValueTargetProvider(array10, Label.TextProperty, nameScope));
				xamlServiceProvider5.Add(typeof(IReferenceProvider), obj9);
				Type typeFromHandle10 = typeof(IXamlTypeResolver);
				XmlNamespaceResolver xmlNamespaceResolver5 = new XmlNamespaceResolver();
				xmlNamespaceResolver5.Add("", "http://xamarin.com/schemas/2014/forms");
				xmlNamespaceResolver5.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
				xmlNamespaceResolver5.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
				xmlNamespaceResolver5.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
				xmlNamespaceResolver5.Add("local", "clr-namespace:CarScannerXamarinForms");
				xmlNamespaceResolver5.Add("settings", "clr-namespace:CarScannerXamarinForms.Settings");
				xmlNamespaceResolver5.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
				xamlServiceProvider5.Add(typeFromHandle10, new XamlTypeResolver(xmlNamespaceResolver5, typeof(VagRKDSPage.<InitializeComponent>_anonXamlCDataTemplate_15).GetTypeInfo().Assembly));
				xamlServiceProvider5.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(199, 37)));
				object obj10 = markupExtension5.ProvideValue(xamlServiceProvider5);
				label5.Text = obj10;
				label5.SetValue(View.VerticalOptionsProperty, LayoutOptions.Center);
				grid.Children.Add(label5);
				picker.SetValue(Grid.RowProperty, 2);
				picker.SetValue(Grid.ColumnProperty, 1);
				staticResourceExtension.Key = "pressureValues";
				IMarkupExtension markupExtension6 = staticResourceExtension;
				XamlServiceProvider xamlServiceProvider6 = new XamlServiceProvider();
				Type typeFromHandle11 = typeof(IProvideValueTarget);
				int num6;
				object[] array11 = new object[(num6 = this.parentValues.Length) + 3];
				Array.Copy(this.parentValues, 0, array11, 3, num6);
				object[] array12 = array11;
				array12[0] = picker;
				array12[1] = grid;
				array12[2] = viewCell;
				object obj11;
				xamlServiceProvider6.Add(typeFromHandle11, obj11 = new SimpleValueTargetProvider(array12, Picker.ItemsSourceProperty, nameScope));
				xamlServiceProvider6.Add(typeof(IReferenceProvider), obj11);
				Type typeFromHandle12 = typeof(IXamlTypeResolver);
				XmlNamespaceResolver xmlNamespaceResolver6 = new XmlNamespaceResolver();
				xmlNamespaceResolver6.Add("", "http://xamarin.com/schemas/2014/forms");
				xmlNamespaceResolver6.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
				xmlNamespaceResolver6.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
				xmlNamespaceResolver6.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
				xmlNamespaceResolver6.Add("local", "clr-namespace:CarScannerXamarinForms");
				xmlNamespaceResolver6.Add("settings", "clr-namespace:CarScannerXamarinForms.Settings");
				xmlNamespaceResolver6.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
				xamlServiceProvider6.Add(typeFromHandle12, new XamlTypeResolver(xmlNamespaceResolver6, typeof(VagRKDSPage.<InitializeComponent>_anonXamlCDataTemplate_15).GetTypeInfo().Assembly));
				xamlServiceProvider6.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(204, 37)));
				object obj12 = markupExtension6.ProvideValue(xamlServiceProvider6);
				picker.ItemsSource = obj12;
				bindingExtension2.Mode = 1;
				staticResourceExtension2.Key = "DoubleToRKDSPressurePositionConverter";
				IMarkupExtension markupExtension7 = staticResourceExtension2;
				XamlServiceProvider xamlServiceProvider7 = new XamlServiceProvider();
				Type typeFromHandle13 = typeof(IProvideValueTarget);
				int num7;
				object[] array13 = new object[(num7 = this.parentValues.Length) + 4];
				Array.Copy(this.parentValues, 0, array13, 4, num7);
				object[] array14 = array13;
				array14[0] = bindingExtension2;
				array14[1] = picker;
				array14[2] = grid;
				array14[3] = viewCell;
				object obj13;
				xamlServiceProvider7.Add(typeFromHandle13, obj13 = new SimpleValueTargetProvider(array14, typeof(BindingExtension).GetRuntimeProperty("Converter"), nameScope));
				xamlServiceProvider7.Add(typeof(IReferenceProvider), obj13);
				Type typeFromHandle14 = typeof(IXamlTypeResolver);
				XmlNamespaceResolver xmlNamespaceResolver7 = new XmlNamespaceResolver();
				xmlNamespaceResolver7.Add("", "http://xamarin.com/schemas/2014/forms");
				xmlNamespaceResolver7.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
				xmlNamespaceResolver7.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
				xmlNamespaceResolver7.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
				xmlNamespaceResolver7.Add("local", "clr-namespace:CarScannerXamarinForms");
				xmlNamespaceResolver7.Add("settings", "clr-namespace:CarScannerXamarinForms.Settings");
				xmlNamespaceResolver7.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
				xamlServiceProvider7.Add(typeFromHandle14, new XamlTypeResolver(xmlNamespaceResolver7, typeof(VagRKDSPage.<InitializeComponent>_anonXamlCDataTemplate_15).GetTypeInfo().Assembly));
				xamlServiceProvider7.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(205, 37)));
				object obj14 = markupExtension7.ProvideValue(xamlServiceProvider7);
				bindingExtension2.Converter = obj14;
				staticResourceExtension3.Key = "pressureValues";
				IMarkupExtension markupExtension8 = staticResourceExtension3;
				XamlServiceProvider xamlServiceProvider8 = new XamlServiceProvider();
				Type typeFromHandle15 = typeof(IProvideValueTarget);
				int num8;
				object[] array15 = new object[(num8 = this.parentValues.Length) + 4];
				Array.Copy(this.parentValues, 0, array15, 4, num8);
				object[] array16 = array15;
				array16[0] = bindingExtension2;
				array16[1] = picker;
				array16[2] = grid;
				array16[3] = viewCell;
				object obj15;
				xamlServiceProvider8.Add(typeFromHandle15, obj15 = new SimpleValueTargetProvider(array16, typeof(BindingExtension).GetRuntimeProperty("ConverterParameter"), nameScope));
				xamlServiceProvider8.Add(typeof(IReferenceProvider), obj15);
				Type typeFromHandle16 = typeof(IXamlTypeResolver);
				XmlNamespaceResolver xmlNamespaceResolver8 = new XmlNamespaceResolver();
				xmlNamespaceResolver8.Add("", "http://xamarin.com/schemas/2014/forms");
				xmlNamespaceResolver8.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
				xmlNamespaceResolver8.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
				xmlNamespaceResolver8.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
				xmlNamespaceResolver8.Add("local", "clr-namespace:CarScannerXamarinForms");
				xmlNamespaceResolver8.Add("settings", "clr-namespace:CarScannerXamarinForms.Settings");
				xmlNamespaceResolver8.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
				xamlServiceProvider8.Add(typeFromHandle16, new XamlTypeResolver(xmlNamespaceResolver8, typeof(VagRKDSPage.<InitializeComponent>_anonXamlCDataTemplate_15).GetTypeInfo().Assembly));
				xamlServiceProvider8.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(205, 37)));
				object obj16 = markupExtension8.ProvideValue(xamlServiceProvider8);
				bindingExtension2.ConverterParameter = obj16;
				bindingExtension2.Path = "FrontFull";
				BindingBase bindingBase2 = bindingExtension2.ProvideValue(null);
				picker.SetBinding(Picker.SelectedIndexProperty, bindingBase2);
				picker.SetValue(View.VerticalOptionsProperty, LayoutOptions.Center);
				grid.Children.Add(picker);
				picker2.SetValue(Grid.RowProperty, 2);
				picker2.SetValue(Grid.ColumnProperty, 2);
				staticResourceExtension4.Key = "pressureValues";
				IMarkupExtension markupExtension9 = staticResourceExtension4;
				XamlServiceProvider xamlServiceProvider9 = new XamlServiceProvider();
				Type typeFromHandle17 = typeof(IProvideValueTarget);
				int num9;
				object[] array17 = new object[(num9 = this.parentValues.Length) + 3];
				Array.Copy(this.parentValues, 0, array17, 3, num9);
				object[] array18 = array17;
				array18[0] = picker2;
				array18[1] = grid;
				array18[2] = viewCell;
				object obj17;
				xamlServiceProvider9.Add(typeFromHandle17, obj17 = new SimpleValueTargetProvider(array18, Picker.ItemsSourceProperty, nameScope));
				xamlServiceProvider9.Add(typeof(IReferenceProvider), obj17);
				Type typeFromHandle18 = typeof(IXamlTypeResolver);
				XmlNamespaceResolver xmlNamespaceResolver9 = new XmlNamespaceResolver();
				xmlNamespaceResolver9.Add("", "http://xamarin.com/schemas/2014/forms");
				xmlNamespaceResolver9.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
				xmlNamespaceResolver9.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
				xmlNamespaceResolver9.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
				xmlNamespaceResolver9.Add("local", "clr-namespace:CarScannerXamarinForms");
				xmlNamespaceResolver9.Add("settings", "clr-namespace:CarScannerXamarinForms.Settings");
				xmlNamespaceResolver9.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
				xamlServiceProvider9.Add(typeFromHandle18, new XamlTypeResolver(xmlNamespaceResolver9, typeof(VagRKDSPage.<InitializeComponent>_anonXamlCDataTemplate_15).GetTypeInfo().Assembly));
				xamlServiceProvider9.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(210, 37)));
				object obj18 = markupExtension9.ProvideValue(xamlServiceProvider9);
				picker2.ItemsSource = obj18;
				bindingExtension3.Mode = 1;
				staticResourceExtension5.Key = "DoubleToRKDSPressurePositionConverter";
				IMarkupExtension markupExtension10 = staticResourceExtension5;
				XamlServiceProvider xamlServiceProvider10 = new XamlServiceProvider();
				Type typeFromHandle19 = typeof(IProvideValueTarget);
				int num10;
				object[] array19 = new object[(num10 = this.parentValues.Length) + 4];
				Array.Copy(this.parentValues, 0, array19, 4, num10);
				object[] array20 = array19;
				array20[0] = bindingExtension3;
				array20[1] = picker2;
				array20[2] = grid;
				array20[3] = viewCell;
				object obj19;
				xamlServiceProvider10.Add(typeFromHandle19, obj19 = new SimpleValueTargetProvider(array20, typeof(BindingExtension).GetRuntimeProperty("Converter"), nameScope));
				xamlServiceProvider10.Add(typeof(IReferenceProvider), obj19);
				Type typeFromHandle20 = typeof(IXamlTypeResolver);
				XmlNamespaceResolver xmlNamespaceResolver10 = new XmlNamespaceResolver();
				xmlNamespaceResolver10.Add("", "http://xamarin.com/schemas/2014/forms");
				xmlNamespaceResolver10.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
				xmlNamespaceResolver10.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
				xmlNamespaceResolver10.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
				xmlNamespaceResolver10.Add("local", "clr-namespace:CarScannerXamarinForms");
				xmlNamespaceResolver10.Add("settings", "clr-namespace:CarScannerXamarinForms.Settings");
				xmlNamespaceResolver10.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
				xamlServiceProvider10.Add(typeFromHandle20, new XamlTypeResolver(xmlNamespaceResolver10, typeof(VagRKDSPage.<InitializeComponent>_anonXamlCDataTemplate_15).GetTypeInfo().Assembly));
				xamlServiceProvider10.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(211, 37)));
				object obj20 = markupExtension10.ProvideValue(xamlServiceProvider10);
				bindingExtension3.Converter = obj20;
				staticResourceExtension6.Key = "pressureValues";
				IMarkupExtension markupExtension11 = staticResourceExtension6;
				XamlServiceProvider xamlServiceProvider11 = new XamlServiceProvider();
				Type typeFromHandle21 = typeof(IProvideValueTarget);
				int num11;
				object[] array21 = new object[(num11 = this.parentValues.Length) + 4];
				Array.Copy(this.parentValues, 0, array21, 4, num11);
				object[] array22 = array21;
				array22[0] = bindingExtension3;
				array22[1] = picker2;
				array22[2] = grid;
				array22[3] = viewCell;
				object obj21;
				xamlServiceProvider11.Add(typeFromHandle21, obj21 = new SimpleValueTargetProvider(array22, typeof(BindingExtension).GetRuntimeProperty("ConverterParameter"), nameScope));
				xamlServiceProvider11.Add(typeof(IReferenceProvider), obj21);
				Type typeFromHandle22 = typeof(IXamlTypeResolver);
				XmlNamespaceResolver xmlNamespaceResolver11 = new XmlNamespaceResolver();
				xmlNamespaceResolver11.Add("", "http://xamarin.com/schemas/2014/forms");
				xmlNamespaceResolver11.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
				xmlNamespaceResolver11.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
				xmlNamespaceResolver11.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
				xmlNamespaceResolver11.Add("local", "clr-namespace:CarScannerXamarinForms");
				xmlNamespaceResolver11.Add("settings", "clr-namespace:CarScannerXamarinForms.Settings");
				xmlNamespaceResolver11.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
				xamlServiceProvider11.Add(typeFromHandle22, new XamlTypeResolver(xmlNamespaceResolver11, typeof(VagRKDSPage.<InitializeComponent>_anonXamlCDataTemplate_15).GetTypeInfo().Assembly));
				xamlServiceProvider11.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(211, 37)));
				object obj22 = markupExtension11.ProvideValue(xamlServiceProvider11);
				bindingExtension3.ConverterParameter = obj22;
				bindingExtension3.Path = "RearFull";
				BindingBase bindingBase3 = bindingExtension3.ProvideValue(null);
				picker2.SetBinding(Picker.SelectedIndexProperty, bindingBase3);
				picker2.SetValue(View.VerticalOptionsProperty, LayoutOptions.Center);
				grid.Children.Add(picker2);
				label6.SetValue(Grid.RowProperty, 3);
				label6.SetValue(Grid.ColumnProperty, 0);
				translate6.Text = "codingDB_VagRKDS_PartialLoad";
				IMarkupExtension markupExtension12 = translate6;
				XamlServiceProvider xamlServiceProvider12 = new XamlServiceProvider();
				Type typeFromHandle23 = typeof(IProvideValueTarget);
				int num12;
				object[] array23 = new object[(num12 = this.parentValues.Length) + 3];
				Array.Copy(this.parentValues, 0, array23, 3, num12);
				object[] array24 = array23;
				array24[0] = label6;
				array24[1] = grid;
				array24[2] = viewCell;
				object obj23;
				xamlServiceProvider12.Add(typeFromHandle23, obj23 = new SimpleValueTargetProvider(array24, Label.TextProperty, nameScope));
				xamlServiceProvider12.Add(typeof(IReferenceProvider), obj23);
				Type typeFromHandle24 = typeof(IXamlTypeResolver);
				XmlNamespaceResolver xmlNamespaceResolver12 = new XmlNamespaceResolver();
				xmlNamespaceResolver12.Add("", "http://xamarin.com/schemas/2014/forms");
				xmlNamespaceResolver12.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
				xmlNamespaceResolver12.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
				xmlNamespaceResolver12.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
				xmlNamespaceResolver12.Add("local", "clr-namespace:CarScannerXamarinForms");
				xmlNamespaceResolver12.Add("settings", "clr-namespace:CarScannerXamarinForms.Settings");
				xmlNamespaceResolver12.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
				xamlServiceProvider12.Add(typeFromHandle24, new XamlTypeResolver(xmlNamespaceResolver12, typeof(VagRKDSPage.<InitializeComponent>_anonXamlCDataTemplate_15).GetTypeInfo().Assembly));
				xamlServiceProvider12.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(216, 37)));
				object obj24 = markupExtension12.ProvideValue(xamlServiceProvider12);
				label6.Text = obj24;
				label6.SetValue(View.VerticalOptionsProperty, LayoutOptions.Center);
				grid.Children.Add(label6);
				picker3.SetValue(Grid.RowProperty, 3);
				picker3.SetValue(Grid.ColumnProperty, 1);
				staticResourceExtension7.Key = "pressureValues";
				IMarkupExtension markupExtension13 = staticResourceExtension7;
				XamlServiceProvider xamlServiceProvider13 = new XamlServiceProvider();
				Type typeFromHandle25 = typeof(IProvideValueTarget);
				int num13;
				object[] array25 = new object[(num13 = this.parentValues.Length) + 3];
				Array.Copy(this.parentValues, 0, array25, 3, num13);
				object[] array26 = array25;
				array26[0] = picker3;
				array26[1] = grid;
				array26[2] = viewCell;
				object obj25;
				xamlServiceProvider13.Add(typeFromHandle25, obj25 = new SimpleValueTargetProvider(array26, Picker.ItemsSourceProperty, nameScope));
				xamlServiceProvider13.Add(typeof(IReferenceProvider), obj25);
				Type typeFromHandle26 = typeof(IXamlTypeResolver);
				XmlNamespaceResolver xmlNamespaceResolver13 = new XmlNamespaceResolver();
				xmlNamespaceResolver13.Add("", "http://xamarin.com/schemas/2014/forms");
				xmlNamespaceResolver13.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
				xmlNamespaceResolver13.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
				xmlNamespaceResolver13.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
				xmlNamespaceResolver13.Add("local", "clr-namespace:CarScannerXamarinForms");
				xmlNamespaceResolver13.Add("settings", "clr-namespace:CarScannerXamarinForms.Settings");
				xmlNamespaceResolver13.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
				xamlServiceProvider13.Add(typeFromHandle26, new XamlTypeResolver(xmlNamespaceResolver13, typeof(VagRKDSPage.<InitializeComponent>_anonXamlCDataTemplate_15).GetTypeInfo().Assembly));
				xamlServiceProvider13.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(221, 37)));
				object obj26 = markupExtension13.ProvideValue(xamlServiceProvider13);
				picker3.ItemsSource = obj26;
				bindingExtension4.Mode = 1;
				staticResourceExtension8.Key = "DoubleToRKDSPressurePositionConverter";
				IMarkupExtension markupExtension14 = staticResourceExtension8;
				XamlServiceProvider xamlServiceProvider14 = new XamlServiceProvider();
				Type typeFromHandle27 = typeof(IProvideValueTarget);
				int num14;
				object[] array27 = new object[(num14 = this.parentValues.Length) + 4];
				Array.Copy(this.parentValues, 0, array27, 4, num14);
				object[] array28 = array27;
				array28[0] = bindingExtension4;
				array28[1] = picker3;
				array28[2] = grid;
				array28[3] = viewCell;
				object obj27;
				xamlServiceProvider14.Add(typeFromHandle27, obj27 = new SimpleValueTargetProvider(array28, typeof(BindingExtension).GetRuntimeProperty("Converter"), nameScope));
				xamlServiceProvider14.Add(typeof(IReferenceProvider), obj27);
				Type typeFromHandle28 = typeof(IXamlTypeResolver);
				XmlNamespaceResolver xmlNamespaceResolver14 = new XmlNamespaceResolver();
				xmlNamespaceResolver14.Add("", "http://xamarin.com/schemas/2014/forms");
				xmlNamespaceResolver14.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
				xmlNamespaceResolver14.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
				xmlNamespaceResolver14.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
				xmlNamespaceResolver14.Add("local", "clr-namespace:CarScannerXamarinForms");
				xmlNamespaceResolver14.Add("settings", "clr-namespace:CarScannerXamarinForms.Settings");
				xmlNamespaceResolver14.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
				xamlServiceProvider14.Add(typeFromHandle28, new XamlTypeResolver(xmlNamespaceResolver14, typeof(VagRKDSPage.<InitializeComponent>_anonXamlCDataTemplate_15).GetTypeInfo().Assembly));
				xamlServiceProvider14.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(222, 37)));
				object obj28 = markupExtension14.ProvideValue(xamlServiceProvider14);
				bindingExtension4.Converter = obj28;
				staticResourceExtension9.Key = "pressureValues";
				IMarkupExtension markupExtension15 = staticResourceExtension9;
				XamlServiceProvider xamlServiceProvider15 = new XamlServiceProvider();
				Type typeFromHandle29 = typeof(IProvideValueTarget);
				int num15;
				object[] array29 = new object[(num15 = this.parentValues.Length) + 4];
				Array.Copy(this.parentValues, 0, array29, 4, num15);
				object[] array30 = array29;
				array30[0] = bindingExtension4;
				array30[1] = picker3;
				array30[2] = grid;
				array30[3] = viewCell;
				object obj29;
				xamlServiceProvider15.Add(typeFromHandle29, obj29 = new SimpleValueTargetProvider(array30, typeof(BindingExtension).GetRuntimeProperty("ConverterParameter"), nameScope));
				xamlServiceProvider15.Add(typeof(IReferenceProvider), obj29);
				Type typeFromHandle30 = typeof(IXamlTypeResolver);
				XmlNamespaceResolver xmlNamespaceResolver15 = new XmlNamespaceResolver();
				xmlNamespaceResolver15.Add("", "http://xamarin.com/schemas/2014/forms");
				xmlNamespaceResolver15.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
				xmlNamespaceResolver15.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
				xmlNamespaceResolver15.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
				xmlNamespaceResolver15.Add("local", "clr-namespace:CarScannerXamarinForms");
				xmlNamespaceResolver15.Add("settings", "clr-namespace:CarScannerXamarinForms.Settings");
				xmlNamespaceResolver15.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
				xamlServiceProvider15.Add(typeFromHandle30, new XamlTypeResolver(xmlNamespaceResolver15, typeof(VagRKDSPage.<InitializeComponent>_anonXamlCDataTemplate_15).GetTypeInfo().Assembly));
				xamlServiceProvider15.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(222, 37)));
				object obj30 = markupExtension15.ProvideValue(xamlServiceProvider15);
				bindingExtension4.ConverterParameter = obj30;
				bindingExtension4.Path = "FrontPartial";
				BindingBase bindingBase4 = bindingExtension4.ProvideValue(null);
				picker3.SetBinding(Picker.SelectedIndexProperty, bindingBase4);
				picker3.SetValue(View.VerticalOptionsProperty, LayoutOptions.Center);
				grid.Children.Add(picker3);
				picker4.SetValue(Grid.RowProperty, 3);
				picker4.SetValue(Grid.ColumnProperty, 2);
				staticResourceExtension10.Key = "pressureValues";
				IMarkupExtension markupExtension16 = staticResourceExtension10;
				XamlServiceProvider xamlServiceProvider16 = new XamlServiceProvider();
				Type typeFromHandle31 = typeof(IProvideValueTarget);
				int num16;
				object[] array31 = new object[(num16 = this.parentValues.Length) + 3];
				Array.Copy(this.parentValues, 0, array31, 3, num16);
				object[] array32 = array31;
				array32[0] = picker4;
				array32[1] = grid;
				array32[2] = viewCell;
				object obj31;
				xamlServiceProvider16.Add(typeFromHandle31, obj31 = new SimpleValueTargetProvider(array32, Picker.ItemsSourceProperty, nameScope));
				xamlServiceProvider16.Add(typeof(IReferenceProvider), obj31);
				Type typeFromHandle32 = typeof(IXamlTypeResolver);
				XmlNamespaceResolver xmlNamespaceResolver16 = new XmlNamespaceResolver();
				xmlNamespaceResolver16.Add("", "http://xamarin.com/schemas/2014/forms");
				xmlNamespaceResolver16.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
				xmlNamespaceResolver16.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
				xmlNamespaceResolver16.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
				xmlNamespaceResolver16.Add("local", "clr-namespace:CarScannerXamarinForms");
				xmlNamespaceResolver16.Add("settings", "clr-namespace:CarScannerXamarinForms.Settings");
				xmlNamespaceResolver16.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
				xamlServiceProvider16.Add(typeFromHandle32, new XamlTypeResolver(xmlNamespaceResolver16, typeof(VagRKDSPage.<InitializeComponent>_anonXamlCDataTemplate_15).GetTypeInfo().Assembly));
				xamlServiceProvider16.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(227, 37)));
				object obj32 = markupExtension16.ProvideValue(xamlServiceProvider16);
				picker4.ItemsSource = obj32;
				bindingExtension5.Mode = 1;
				staticResourceExtension11.Key = "DoubleToRKDSPressurePositionConverter";
				IMarkupExtension markupExtension17 = staticResourceExtension11;
				XamlServiceProvider xamlServiceProvider17 = new XamlServiceProvider();
				Type typeFromHandle33 = typeof(IProvideValueTarget);
				int num17;
				object[] array33 = new object[(num17 = this.parentValues.Length) + 4];
				Array.Copy(this.parentValues, 0, array33, 4, num17);
				object[] array34 = array33;
				array34[0] = bindingExtension5;
				array34[1] = picker4;
				array34[2] = grid;
				array34[3] = viewCell;
				object obj33;
				xamlServiceProvider17.Add(typeFromHandle33, obj33 = new SimpleValueTargetProvider(array34, typeof(BindingExtension).GetRuntimeProperty("Converter"), nameScope));
				xamlServiceProvider17.Add(typeof(IReferenceProvider), obj33);
				Type typeFromHandle34 = typeof(IXamlTypeResolver);
				XmlNamespaceResolver xmlNamespaceResolver17 = new XmlNamespaceResolver();
				xmlNamespaceResolver17.Add("", "http://xamarin.com/schemas/2014/forms");
				xmlNamespaceResolver17.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
				xmlNamespaceResolver17.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
				xmlNamespaceResolver17.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
				xmlNamespaceResolver17.Add("local", "clr-namespace:CarScannerXamarinForms");
				xmlNamespaceResolver17.Add("settings", "clr-namespace:CarScannerXamarinForms.Settings");
				xmlNamespaceResolver17.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
				xamlServiceProvider17.Add(typeFromHandle34, new XamlTypeResolver(xmlNamespaceResolver17, typeof(VagRKDSPage.<InitializeComponent>_anonXamlCDataTemplate_15).GetTypeInfo().Assembly));
				xamlServiceProvider17.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(228, 37)));
				object obj34 = markupExtension17.ProvideValue(xamlServiceProvider17);
				bindingExtension5.Converter = obj34;
				staticResourceExtension12.Key = "pressureValues";
				IMarkupExtension markupExtension18 = staticResourceExtension12;
				XamlServiceProvider xamlServiceProvider18 = new XamlServiceProvider();
				Type typeFromHandle35 = typeof(IProvideValueTarget);
				int num18;
				object[] array35 = new object[(num18 = this.parentValues.Length) + 4];
				Array.Copy(this.parentValues, 0, array35, 4, num18);
				object[] array36 = array35;
				array36[0] = bindingExtension5;
				array36[1] = picker4;
				array36[2] = grid;
				array36[3] = viewCell;
				object obj35;
				xamlServiceProvider18.Add(typeFromHandle35, obj35 = new SimpleValueTargetProvider(array36, typeof(BindingExtension).GetRuntimeProperty("ConverterParameter"), nameScope));
				xamlServiceProvider18.Add(typeof(IReferenceProvider), obj35);
				Type typeFromHandle36 = typeof(IXamlTypeResolver);
				XmlNamespaceResolver xmlNamespaceResolver18 = new XmlNamespaceResolver();
				xmlNamespaceResolver18.Add("", "http://xamarin.com/schemas/2014/forms");
				xmlNamespaceResolver18.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
				xmlNamespaceResolver18.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
				xmlNamespaceResolver18.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
				xmlNamespaceResolver18.Add("local", "clr-namespace:CarScannerXamarinForms");
				xmlNamespaceResolver18.Add("settings", "clr-namespace:CarScannerXamarinForms.Settings");
				xmlNamespaceResolver18.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
				xamlServiceProvider18.Add(typeFromHandle36, new XamlTypeResolver(xmlNamespaceResolver18, typeof(VagRKDSPage.<InitializeComponent>_anonXamlCDataTemplate_15).GetTypeInfo().Assembly));
				xamlServiceProvider18.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(228, 37)));
				object obj36 = markupExtension18.ProvideValue(xamlServiceProvider18);
				bindingExtension5.ConverterParameter = obj36;
				bindingExtension5.Path = "RearPartial";
				BindingBase bindingBase5 = bindingExtension5.ProvideValue(null);
				picker4.SetBinding(Picker.SelectedIndexProperty, bindingBase5);
				picker4.SetValue(View.VerticalOptionsProperty, LayoutOptions.Center);
				grid.Children.Add(picker4);
				label7.SetValue(Grid.RowProperty, 4);
				label7.SetValue(Grid.ColumnProperty, 0);
				referenceExtension.Name = "switchComfort";
				IMarkupExtension markupExtension19 = referenceExtension;
				XamlServiceProvider xamlServiceProvider19 = new XamlServiceProvider();
				Type typeFromHandle37 = typeof(IProvideValueTarget);
				int num19;
				object[] array37 = new object[(num19 = this.parentValues.Length) + 4];
				Array.Copy(this.parentValues, 0, array37, 4, num19);
				object[] array38 = array37;
				array38[0] = bindingExtension6;
				array38[1] = label7;
				array38[2] = grid;
				array38[3] = viewCell;
				object obj37;
				xamlServiceProvider19.Add(typeFromHandle37, obj37 = new SimpleValueTargetProvider(array38, typeof(BindingExtension).GetRuntimeProperty("Source"), nameScope));
				xamlServiceProvider19.Add(typeof(IReferenceProvider), obj37);
				Type typeFromHandle38 = typeof(IXamlTypeResolver);
				XmlNamespaceResolver xmlNamespaceResolver19 = new XmlNamespaceResolver();
				xmlNamespaceResolver19.Add("", "http://xamarin.com/schemas/2014/forms");
				xmlNamespaceResolver19.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
				xmlNamespaceResolver19.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
				xmlNamespaceResolver19.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
				xmlNamespaceResolver19.Add("local", "clr-namespace:CarScannerXamarinForms");
				xmlNamespaceResolver19.Add("settings", "clr-namespace:CarScannerXamarinForms.Settings");
				xmlNamespaceResolver19.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
				xamlServiceProvider19.Add(typeFromHandle38, new XamlTypeResolver(xmlNamespaceResolver19, typeof(VagRKDSPage.<InitializeComponent>_anonXamlCDataTemplate_15).GetTypeInfo().Assembly));
				xamlServiceProvider19.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(234, 37)));
				object obj38 = markupExtension19.ProvideValue(xamlServiceProvider19);
				bindingExtension6.Source = obj38;
				bindingExtension6.Path = "IsToggled";
				BindingBase bindingBase6 = bindingExtension6.ProvideValue(null);
				label7.SetBinding(VisualElement.IsVisibleProperty, bindingBase6);
				translate7.Text = "codingDB_VagRKDS_ComfortLoad";
				IMarkupExtension markupExtension20 = translate7;
				XamlServiceProvider xamlServiceProvider20 = new XamlServiceProvider();
				Type typeFromHandle39 = typeof(IProvideValueTarget);
				int num20;
				object[] array39 = new object[(num20 = this.parentValues.Length) + 3];
				Array.Copy(this.parentValues, 0, array39, 3, num20);
				object[] array40 = array39;
				array40[0] = label7;
				array40[1] = grid;
				array40[2] = viewCell;
				object obj39;
				xamlServiceProvider20.Add(typeFromHandle39, obj39 = new SimpleValueTargetProvider(array40, Label.TextProperty, nameScope));
				xamlServiceProvider20.Add(typeof(IReferenceProvider), obj39);
				Type typeFromHandle40 = typeof(IXamlTypeResolver);
				XmlNamespaceResolver xmlNamespaceResolver20 = new XmlNamespaceResolver();
				xmlNamespaceResolver20.Add("", "http://xamarin.com/schemas/2014/forms");
				xmlNamespaceResolver20.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
				xmlNamespaceResolver20.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
				xmlNamespaceResolver20.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
				xmlNamespaceResolver20.Add("local", "clr-namespace:CarScannerXamarinForms");
				xmlNamespaceResolver20.Add("settings", "clr-namespace:CarScannerXamarinForms.Settings");
				xmlNamespaceResolver20.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
				xamlServiceProvider20.Add(typeFromHandle40, new XamlTypeResolver(xmlNamespaceResolver20, typeof(VagRKDSPage.<InitializeComponent>_anonXamlCDataTemplate_15).GetTypeInfo().Assembly));
				xamlServiceProvider20.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(235, 37)));
				object obj40 = markupExtension20.ProvideValue(xamlServiceProvider20);
				label7.Text = obj40;
				label7.SetValue(View.VerticalOptionsProperty, LayoutOptions.Center);
				grid.Children.Add(label7);
				picker5.SetValue(Grid.RowProperty, 4);
				picker5.SetValue(Grid.ColumnProperty, 1);
				referenceExtension2.Name = "switchComfort";
				IMarkupExtension markupExtension21 = referenceExtension2;
				XamlServiceProvider xamlServiceProvider21 = new XamlServiceProvider();
				Type typeFromHandle41 = typeof(IProvideValueTarget);
				int num21;
				object[] array41 = new object[(num21 = this.parentValues.Length) + 4];
				Array.Copy(this.parentValues, 0, array41, 4, num21);
				object[] array42 = array41;
				array42[0] = bindingExtension7;
				array42[1] = picker5;
				array42[2] = grid;
				array42[3] = viewCell;
				object obj41;
				xamlServiceProvider21.Add(typeFromHandle41, obj41 = new SimpleValueTargetProvider(array42, typeof(BindingExtension).GetRuntimeProperty("Source"), nameScope));
				xamlServiceProvider21.Add(typeof(IReferenceProvider), obj41);
				Type typeFromHandle42 = typeof(IXamlTypeResolver);
				XmlNamespaceResolver xmlNamespaceResolver21 = new XmlNamespaceResolver();
				xmlNamespaceResolver21.Add("", "http://xamarin.com/schemas/2014/forms");
				xmlNamespaceResolver21.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
				xmlNamespaceResolver21.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
				xmlNamespaceResolver21.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
				xmlNamespaceResolver21.Add("local", "clr-namespace:CarScannerXamarinForms");
				xmlNamespaceResolver21.Add("settings", "clr-namespace:CarScannerXamarinForms.Settings");
				xmlNamespaceResolver21.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
				xamlServiceProvider21.Add(typeFromHandle42, new XamlTypeResolver(xmlNamespaceResolver21, typeof(VagRKDSPage.<InitializeComponent>_anonXamlCDataTemplate_15).GetTypeInfo().Assembly));
				xamlServiceProvider21.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(240, 37)));
				object obj42 = markupExtension21.ProvideValue(xamlServiceProvider21);
				bindingExtension7.Source = obj42;
				bindingExtension7.Path = "IsToggled";
				BindingBase bindingBase7 = bindingExtension7.ProvideValue(null);
				picker5.SetBinding(VisualElement.IsVisibleProperty, bindingBase7);
				staticResourceExtension13.Key = "pressureValues";
				IMarkupExtension markupExtension22 = staticResourceExtension13;
				XamlServiceProvider xamlServiceProvider22 = new XamlServiceProvider();
				Type typeFromHandle43 = typeof(IProvideValueTarget);
				int num22;
				object[] array43 = new object[(num22 = this.parentValues.Length) + 3];
				Array.Copy(this.parentValues, 0, array43, 3, num22);
				object[] array44 = array43;
				array44[0] = picker5;
				array44[1] = grid;
				array44[2] = viewCell;
				object obj43;
				xamlServiceProvider22.Add(typeFromHandle43, obj43 = new SimpleValueTargetProvider(array44, Picker.ItemsSourceProperty, nameScope));
				xamlServiceProvider22.Add(typeof(IReferenceProvider), obj43);
				Type typeFromHandle44 = typeof(IXamlTypeResolver);
				XmlNamespaceResolver xmlNamespaceResolver22 = new XmlNamespaceResolver();
				xmlNamespaceResolver22.Add("", "http://xamarin.com/schemas/2014/forms");
				xmlNamespaceResolver22.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
				xmlNamespaceResolver22.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
				xmlNamespaceResolver22.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
				xmlNamespaceResolver22.Add("local", "clr-namespace:CarScannerXamarinForms");
				xmlNamespaceResolver22.Add("settings", "clr-namespace:CarScannerXamarinForms.Settings");
				xmlNamespaceResolver22.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
				xamlServiceProvider22.Add(typeFromHandle44, new XamlTypeResolver(xmlNamespaceResolver22, typeof(VagRKDSPage.<InitializeComponent>_anonXamlCDataTemplate_15).GetTypeInfo().Assembly));
				xamlServiceProvider22.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(241, 37)));
				object obj44 = markupExtension22.ProvideValue(xamlServiceProvider22);
				picker5.ItemsSource = obj44;
				bindingExtension8.Mode = 1;
				staticResourceExtension14.Key = "DoubleToRKDSPressurePositionConverter";
				IMarkupExtension markupExtension23 = staticResourceExtension14;
				XamlServiceProvider xamlServiceProvider23 = new XamlServiceProvider();
				Type typeFromHandle45 = typeof(IProvideValueTarget);
				int num23;
				object[] array45 = new object[(num23 = this.parentValues.Length) + 4];
				Array.Copy(this.parentValues, 0, array45, 4, num23);
				object[] array46 = array45;
				array46[0] = bindingExtension8;
				array46[1] = picker5;
				array46[2] = grid;
				array46[3] = viewCell;
				object obj45;
				xamlServiceProvider23.Add(typeFromHandle45, obj45 = new SimpleValueTargetProvider(array46, typeof(BindingExtension).GetRuntimeProperty("Converter"), nameScope));
				xamlServiceProvider23.Add(typeof(IReferenceProvider), obj45);
				Type typeFromHandle46 = typeof(IXamlTypeResolver);
				XmlNamespaceResolver xmlNamespaceResolver23 = new XmlNamespaceResolver();
				xmlNamespaceResolver23.Add("", "http://xamarin.com/schemas/2014/forms");
				xmlNamespaceResolver23.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
				xmlNamespaceResolver23.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
				xmlNamespaceResolver23.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
				xmlNamespaceResolver23.Add("local", "clr-namespace:CarScannerXamarinForms");
				xmlNamespaceResolver23.Add("settings", "clr-namespace:CarScannerXamarinForms.Settings");
				xmlNamespaceResolver23.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
				xamlServiceProvider23.Add(typeFromHandle46, new XamlTypeResolver(xmlNamespaceResolver23, typeof(VagRKDSPage.<InitializeComponent>_anonXamlCDataTemplate_15).GetTypeInfo().Assembly));
				xamlServiceProvider23.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(242, 37)));
				object obj46 = markupExtension23.ProvideValue(xamlServiceProvider23);
				bindingExtension8.Converter = obj46;
				staticResourceExtension15.Key = "pressureValues";
				IMarkupExtension markupExtension24 = staticResourceExtension15;
				XamlServiceProvider xamlServiceProvider24 = new XamlServiceProvider();
				Type typeFromHandle47 = typeof(IProvideValueTarget);
				int num24;
				object[] array47 = new object[(num24 = this.parentValues.Length) + 4];
				Array.Copy(this.parentValues, 0, array47, 4, num24);
				object[] array48 = array47;
				array48[0] = bindingExtension8;
				array48[1] = picker5;
				array48[2] = grid;
				array48[3] = viewCell;
				object obj47;
				xamlServiceProvider24.Add(typeFromHandle47, obj47 = new SimpleValueTargetProvider(array48, typeof(BindingExtension).GetRuntimeProperty("ConverterParameter"), nameScope));
				xamlServiceProvider24.Add(typeof(IReferenceProvider), obj47);
				Type typeFromHandle48 = typeof(IXamlTypeResolver);
				XmlNamespaceResolver xmlNamespaceResolver24 = new XmlNamespaceResolver();
				xmlNamespaceResolver24.Add("", "http://xamarin.com/schemas/2014/forms");
				xmlNamespaceResolver24.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
				xmlNamespaceResolver24.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
				xmlNamespaceResolver24.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
				xmlNamespaceResolver24.Add("local", "clr-namespace:CarScannerXamarinForms");
				xmlNamespaceResolver24.Add("settings", "clr-namespace:CarScannerXamarinForms.Settings");
				xmlNamespaceResolver24.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
				xamlServiceProvider24.Add(typeFromHandle48, new XamlTypeResolver(xmlNamespaceResolver24, typeof(VagRKDSPage.<InitializeComponent>_anonXamlCDataTemplate_15).GetTypeInfo().Assembly));
				xamlServiceProvider24.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(242, 37)));
				object obj48 = markupExtension24.ProvideValue(xamlServiceProvider24);
				bindingExtension8.ConverterParameter = obj48;
				bindingExtension8.Path = "FrontComfort";
				BindingBase bindingBase8 = bindingExtension8.ProvideValue(null);
				picker5.SetBinding(Picker.SelectedIndexProperty, bindingBase8);
				picker5.SetValue(View.VerticalOptionsProperty, LayoutOptions.Center);
				grid.Children.Add(picker5);
				picker6.SetValue(Grid.RowProperty, 4);
				picker6.SetValue(Grid.ColumnProperty, 2);
				referenceExtension3.Name = "switchComfort";
				IMarkupExtension markupExtension25 = referenceExtension3;
				XamlServiceProvider xamlServiceProvider25 = new XamlServiceProvider();
				Type typeFromHandle49 = typeof(IProvideValueTarget);
				int num25;
				object[] array49 = new object[(num25 = this.parentValues.Length) + 4];
				Array.Copy(this.parentValues, 0, array49, 4, num25);
				object[] array50 = array49;
				array50[0] = bindingExtension9;
				array50[1] = picker6;
				array50[2] = grid;
				array50[3] = viewCell;
				object obj49;
				xamlServiceProvider25.Add(typeFromHandle49, obj49 = new SimpleValueTargetProvider(array50, typeof(BindingExtension).GetRuntimeProperty("Source"), nameScope));
				xamlServiceProvider25.Add(typeof(IReferenceProvider), obj49);
				Type typeFromHandle50 = typeof(IXamlTypeResolver);
				XmlNamespaceResolver xmlNamespaceResolver25 = new XmlNamespaceResolver();
				xmlNamespaceResolver25.Add("", "http://xamarin.com/schemas/2014/forms");
				xmlNamespaceResolver25.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
				xmlNamespaceResolver25.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
				xmlNamespaceResolver25.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
				xmlNamespaceResolver25.Add("local", "clr-namespace:CarScannerXamarinForms");
				xmlNamespaceResolver25.Add("settings", "clr-namespace:CarScannerXamarinForms.Settings");
				xmlNamespaceResolver25.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
				xamlServiceProvider25.Add(typeFromHandle50, new XamlTypeResolver(xmlNamespaceResolver25, typeof(VagRKDSPage.<InitializeComponent>_anonXamlCDataTemplate_15).GetTypeInfo().Assembly));
				xamlServiceProvider25.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(247, 37)));
				object obj50 = markupExtension25.ProvideValue(xamlServiceProvider25);
				bindingExtension9.Source = obj50;
				bindingExtension9.Path = "IsToggled";
				BindingBase bindingBase9 = bindingExtension9.ProvideValue(null);
				picker6.SetBinding(VisualElement.IsVisibleProperty, bindingBase9);
				staticResourceExtension16.Key = "pressureValues";
				IMarkupExtension markupExtension26 = staticResourceExtension16;
				XamlServiceProvider xamlServiceProvider26 = new XamlServiceProvider();
				Type typeFromHandle51 = typeof(IProvideValueTarget);
				int num26;
				object[] array51 = new object[(num26 = this.parentValues.Length) + 3];
				Array.Copy(this.parentValues, 0, array51, 3, num26);
				object[] array52 = array51;
				array52[0] = picker6;
				array52[1] = grid;
				array52[2] = viewCell;
				object obj51;
				xamlServiceProvider26.Add(typeFromHandle51, obj51 = new SimpleValueTargetProvider(array52, Picker.ItemsSourceProperty, nameScope));
				xamlServiceProvider26.Add(typeof(IReferenceProvider), obj51);
				Type typeFromHandle52 = typeof(IXamlTypeResolver);
				XmlNamespaceResolver xmlNamespaceResolver26 = new XmlNamespaceResolver();
				xmlNamespaceResolver26.Add("", "http://xamarin.com/schemas/2014/forms");
				xmlNamespaceResolver26.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
				xmlNamespaceResolver26.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
				xmlNamespaceResolver26.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
				xmlNamespaceResolver26.Add("local", "clr-namespace:CarScannerXamarinForms");
				xmlNamespaceResolver26.Add("settings", "clr-namespace:CarScannerXamarinForms.Settings");
				xmlNamespaceResolver26.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
				xamlServiceProvider26.Add(typeFromHandle52, new XamlTypeResolver(xmlNamespaceResolver26, typeof(VagRKDSPage.<InitializeComponent>_anonXamlCDataTemplate_15).GetTypeInfo().Assembly));
				xamlServiceProvider26.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(248, 37)));
				object obj52 = markupExtension26.ProvideValue(xamlServiceProvider26);
				picker6.ItemsSource = obj52;
				bindingExtension10.Mode = 1;
				staticResourceExtension17.Key = "DoubleToRKDSPressurePositionConverter";
				IMarkupExtension markupExtension27 = staticResourceExtension17;
				XamlServiceProvider xamlServiceProvider27 = new XamlServiceProvider();
				Type typeFromHandle53 = typeof(IProvideValueTarget);
				int num27;
				object[] array53 = new object[(num27 = this.parentValues.Length) + 4];
				Array.Copy(this.parentValues, 0, array53, 4, num27);
				object[] array54 = array53;
				array54[0] = bindingExtension10;
				array54[1] = picker6;
				array54[2] = grid;
				array54[3] = viewCell;
				object obj53;
				xamlServiceProvider27.Add(typeFromHandle53, obj53 = new SimpleValueTargetProvider(array54, typeof(BindingExtension).GetRuntimeProperty("Converter"), nameScope));
				xamlServiceProvider27.Add(typeof(IReferenceProvider), obj53);
				Type typeFromHandle54 = typeof(IXamlTypeResolver);
				XmlNamespaceResolver xmlNamespaceResolver27 = new XmlNamespaceResolver();
				xmlNamespaceResolver27.Add("", "http://xamarin.com/schemas/2014/forms");
				xmlNamespaceResolver27.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
				xmlNamespaceResolver27.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
				xmlNamespaceResolver27.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
				xmlNamespaceResolver27.Add("local", "clr-namespace:CarScannerXamarinForms");
				xmlNamespaceResolver27.Add("settings", "clr-namespace:CarScannerXamarinForms.Settings");
				xmlNamespaceResolver27.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
				xamlServiceProvider27.Add(typeFromHandle54, new XamlTypeResolver(xmlNamespaceResolver27, typeof(VagRKDSPage.<InitializeComponent>_anonXamlCDataTemplate_15).GetTypeInfo().Assembly));
				xamlServiceProvider27.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(249, 37)));
				object obj54 = markupExtension27.ProvideValue(xamlServiceProvider27);
				bindingExtension10.Converter = obj54;
				staticResourceExtension18.Key = "pressureValues";
				IMarkupExtension markupExtension28 = staticResourceExtension18;
				XamlServiceProvider xamlServiceProvider28 = new XamlServiceProvider();
				Type typeFromHandle55 = typeof(IProvideValueTarget);
				int num28;
				object[] array55 = new object[(num28 = this.parentValues.Length) + 4];
				Array.Copy(this.parentValues, 0, array55, 4, num28);
				object[] array56 = array55;
				array56[0] = bindingExtension10;
				array56[1] = picker6;
				array56[2] = grid;
				array56[3] = viewCell;
				object obj55;
				xamlServiceProvider28.Add(typeFromHandle55, obj55 = new SimpleValueTargetProvider(array56, typeof(BindingExtension).GetRuntimeProperty("ConverterParameter"), nameScope));
				xamlServiceProvider28.Add(typeof(IReferenceProvider), obj55);
				Type typeFromHandle56 = typeof(IXamlTypeResolver);
				XmlNamespaceResolver xmlNamespaceResolver28 = new XmlNamespaceResolver();
				xmlNamespaceResolver28.Add("", "http://xamarin.com/schemas/2014/forms");
				xmlNamespaceResolver28.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
				xmlNamespaceResolver28.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
				xmlNamespaceResolver28.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
				xmlNamespaceResolver28.Add("local", "clr-namespace:CarScannerXamarinForms");
				xmlNamespaceResolver28.Add("settings", "clr-namespace:CarScannerXamarinForms.Settings");
				xmlNamespaceResolver28.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
				xamlServiceProvider28.Add(typeFromHandle56, new XamlTypeResolver(xmlNamespaceResolver28, typeof(VagRKDSPage.<InitializeComponent>_anonXamlCDataTemplate_15).GetTypeInfo().Assembly));
				xamlServiceProvider28.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(249, 37)));
				object obj56 = markupExtension28.ProvideValue(xamlServiceProvider28);
				bindingExtension10.ConverterParameter = obj56;
				bindingExtension10.Path = "RearComfort";
				BindingBase bindingBase10 = bindingExtension10.ProvideValue(null);
				picker6.SetBinding(Picker.SelectedIndexProperty, bindingBase10);
				picker6.SetValue(View.VerticalOptionsProperty, LayoutOptions.Center);
				grid.Children.Add(picker6);
				viewCell.View = grid;
				return viewCell;
			}

			// Token: 0x04003000 RID: 12288
			internal object[] parentValues;

			// Token: 0x04003001 RID: 12289
			internal VagRKDSPage root;
		}
	}
}
