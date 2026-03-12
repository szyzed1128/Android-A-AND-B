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
using CarScannerXamarinForms.Coding.DB.MQB;
using CarScannerXamarinForms.Common;
using CarScannerXamarinForms.Common.XAMLConverters;
using CarScannerXamarinForms.InApp;
using CarScannerXamarinForms.Pages;
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
	// Token: 0x02000961 RID: 2401
	[XamlCompilation(2)]
	[XamlFilePath("Coding\\Pages\\MQBTailGatePageCustomization.xaml")]
	public class MQBTailGatePageCustomization : ContentPage
	{
		// Token: 0x06004ED4 RID: 20180 RVA: 0x003BF0FD File Offset: 0x003BD2FD
		public MQBTailGatePageCustomization(ICodingContainer coding)
		{
			this.InitializeComponent();
			this.Coding = (TailGateParametrizeCustomizationCoding)coding;
			base.BindingContext = this.Coding;
			base.Appearing += this.MQBTailGatePageCustomization_Appearing;
		}

		// Token: 0x06004ED5 RID: 20181 RVA: 0x003BF13C File Offset: 0x003BD33C
		private async void MQBTailGatePageCustomization_Appearing(object sender, EventArgs e)
		{
			if (this.first_appearing)
			{
				this.first_appearing = false;
				this.stackRoot.IsEnabled = false;
				this.activityFrame.IsVisible = true;
				await this.Coding.UpdateCurrentState("", null);
				this.activityFrame.IsVisible = false;
				this.stackRoot.IsEnabled = true;
			}
		}

		// Token: 0x17001794 RID: 6036
		// (get) Token: 0x06004ED6 RID: 20182 RVA: 0x003BF173 File Offset: 0x003BD373
		// (set) Token: 0x06004ED7 RID: 20183 RVA: 0x003BF17B File Offset: 0x003BD37B
		internal TailGateParametrizeCustomizationCoding Coding
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

		// Token: 0x06004ED8 RID: 20184 RVA: 0x003BF184 File Offset: 0x003BD384
		private async void btnChooseOtherVersion_Clicked(object sender, EventArgs e)
		{
			TailGateParametrizeCustomizationCoding coding = base.BindingContext as TailGateParametrizeCustomizationCoding;
			if (string.IsNullOrEmpty(coding.PartNumber))
			{
				await base.DisplayAlert("Error", "Partnumber not detected, coding not supported!", "OK");
			}
			else if (coding.LongCoding == null || coding.LongCoding.Length == 0)
			{
				await base.DisplayAlert("Error", "Long coding not detected, coding not supported!", "OK");
			}
			else
			{
				IEnumerable<ValueItemWithTranslation> enumerable = from x in (from x in PackageFileReader.GetFilesInDirectory("vag.tailgate6d.")
						orderby x
						select x).ToArray<string>()
					where x.Contains(this.Coding.PartNumber, StringComparison.OrdinalIgnoreCase) || x.Contains(this.Coding.DSVersion, StringComparison.OrdinalIgnoreCase)
					select new ValueItemWithTranslation
					{
						Title = Path.GetFileNameWithoutExtension(x).Replace('_', ' ').Trim(' ')
							.ToUpper(),
						Value = x
					};
				Action<ValueItemWithTranslation> action = delegate(ValueItemWithTranslation selected)
				{
					try
					{
						MQB_6D_DatasetBuilderModel mqb_6D_DatasetBuilderModel = new MQB_6D_DatasetBuilderModel();
						using (Stream stream = PackageFileReader.OpenFileStream("vag.tailgate6d." + selected.Value))
						{
							byte[] array = new byte[stream.Length];
							stream.Read(array, 0, array.Length);
							mqb_6D_DatasetBuilderModel.LoadFromBytes(array, coding.PartNumber, coding.LongCoding);
						}
						coding.Model = mqb_6D_DatasetBuilderModel;
						coding.DisplayControls = true;
					}
					catch (Exception)
					{
						App.GetCurrentPage().DisplayAlert("Error!", "Error loading dataset " + selected.Title, "OK");
					}
				};
				ItemWithValueSelectorPage itemWithValueSelectorPage = new ItemWithValueSelectorPage("Dataset version", enumerable, null, action);
				await base.Navigation.PushAsync(itemWithValueSelectorPage);
			}
		}

		// Token: 0x06004ED9 RID: 20185 RVA: 0x003BF1BC File Offset: 0x003BD3BC
		private async void btnApply_Clicked(object sender, EventArgs e)
		{
			TailGateParametrizeCustomizationCoding Coding = base.BindingContext as TailGateParametrizeCustomizationCoding;
			if (!Coding.Model.IsLoaded)
			{
				await base.DisplayAlert(Coding.CurrentState, "", "OK");
			}
			if (Coding.RequiresPro && !SharedSettings.Current.AdsProductPurchased)
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
				this.stackRoot.IsEnabled = false;
				Progress<string> progress = new Progress<string>(delegate(string s)
				{
					Device.BeginInvokeOnMainThread(delegate
					{
						this.activityFrame.Text = s;
					});
				});
				CodingRequestResult codingRequestResult = await Coding.Execute(this.entryPassword.Text, "", "", progress, null, false);
				if (codingRequestResult == CodingRequestResult.Success)
				{
					SharedSettings.Current.CodingsCounter++;
					await base.DisplayAlert(Coding.Name, Translate.GetString("coding_OperationFinished"), "OK");
				}
				else
				{
					await base.DisplayAlert(Translate.GetString("coding_Error"), MQBAdaptationTemplate.CodingRequestResultToString(codingRequestResult), "OK");
				}
				this.activityFrame.IsVisible = false;
				this.activityFrame.Text = ActivityFrame.PLEASE_WAIT_TEXT;
				this.stackRoot.IsEnabled = true;
			}
		}

		// Token: 0x06004EDA RID: 20186 RVA: 0x003BF1F4 File Offset: 0x003BD3F4
		private async void BtnUpdateState_Clicked(object sender, EventArgs e)
		{
			await this.UpdateState();
		}

		// Token: 0x06004EDB RID: 20187 RVA: 0x003BF22C File Offset: 0x003BD42C
		public async Task UpdateState()
		{
			this.activityFrame.Text = ActivityFrame.PLEASE_WAIT_TEXT;
			this.activityFrame.IsVisible = true;
			this.stackRoot.IsEnabled = false;
			Progress<string> progress = new Progress<string>(delegate(string s)
			{
				MainThread.BeginInvokeOnMainThread(delegate
				{
					this.activityFrame.Text = ActivityFrame.PLEASE_WAIT_TEXT + "\n" + s;
				});
			});
			if (!App.OBDSimulator.IsActive)
			{
				await this.Coding.UpdateCurrentState("", progress);
			}
			this.activityFrame.IsVisible = false;
			this.stackRoot.IsEnabled = true;
			this.activityFrame.Text = ActivityFrame.PLEASE_WAIT_TEXT;
		}

		// Token: 0x06004EDC RID: 20188 RVA: 0x003BF26F File Offset: 0x003BD46F
		private void BtnLoadFromUserBytes_Clicked(object sender, EventArgs e)
		{
			this.Coding.Model.LoadFromUserBytes();
		}

		// Token: 0x06004EDD RID: 20189 RVA: 0x003BF284 File Offset: 0x003BD484
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private void InitializeComponent()
		{
			if (ResourceLoader.IsEnabled && ResourceLoader.CanProvideContentFor(new ResourceLoader.ResourceLoadingQuery
			{
				AssemblyName = typeof(MQBTailGatePageCustomization).GetTypeInfo().Assembly.GetName(),
				ResourcePath = "Coding/Pages/MQBTailGatePageCustomization.xaml",
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
			VisualDiagnostics.RegisterSourceInfo(translate = new Translate(), new Uri("Coding\\Pages\\MQBTailGatePageCustomization.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 16, 5);
			DynamicResourceExtension dynamicResourceExtension;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension = new DynamicResourceExtension(), new Uri("Coding\\Pages\\MQBTailGatePageCustomization.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 19, 5);
			EmptyStringToFalseConverter emptyStringToFalseConverter;
			VisualDiagnostics.RegisterSourceInfo(emptyStringToFalseConverter = new EmptyStringToFalseConverter(), new Uri("Coding\\Pages\\MQBTailGatePageCustomization.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 22, 10);
			EmptyStringToTrueConverter emptyStringToTrueConverter;
			VisualDiagnostics.RegisterSourceInfo(emptyStringToTrueConverter = new EmptyStringToTrueConverter(), new Uri("Coding\\Pages\\MQBTailGatePageCustomization.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 23, 10);
			VagCodingPlatformToTrueConverter vagCodingPlatformToTrueConverter;
			VisualDiagnostics.RegisterSourceInfo(vagCodingPlatformToTrueConverter = new VagCodingPlatformToTrueConverter(), new Uri("Coding\\Pages\\MQBTailGatePageCustomization.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 24, 10);
			BoolToNegativeConverter boolToNegativeConverter;
			VisualDiagnostics.RegisterSourceInfo(boolToNegativeConverter = new BoolToNegativeConverter(), new Uri("Coding\\Pages\\MQBTailGatePageCustomization.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 25, 10);
			DynamicResourceExtension dynamicResourceExtension2;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension2 = new DynamicResourceExtension(), new Uri("Coding\\Pages\\MQBTailGatePageCustomization.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 34, 25);
			BindingExtension bindingExtension;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension = new BindingExtension(), new Uri("Coding\\Pages\\MQBTailGatePageCustomization.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 35, 25);
			Label label;
			VisualDiagnostics.RegisterSourceInfo(label = new Label(), new Uri("Coding\\Pages\\MQBTailGatePageCustomization.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 32, 22);
			StaticResourceExtension staticResourceExtension;
			VisualDiagnostics.RegisterSourceInfo(staticResourceExtension = new StaticResourceExtension(), new Uri("Coding\\Pages\\MQBTailGatePageCustomization.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 36, 28);
			BindingExtension bindingExtension2;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension2 = new BindingExtension(), new Uri("Coding\\Pages\\MQBTailGatePageCustomization.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 36, 28);
			BindingExtension bindingExtension3;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension3 = new BindingExtension(), new Uri("Coding\\Pages\\MQBTailGatePageCustomization.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 36, 118);
			Label label2;
			VisualDiagnostics.RegisterSourceInfo(label2 = new Label(), new Uri("Coding\\Pages\\MQBTailGatePageCustomization.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 36, 22);
			StaticResourceExtension staticResourceExtension2;
			VisualDiagnostics.RegisterSourceInfo(staticResourceExtension2 = new StaticResourceExtension(), new Uri("Coding\\Pages\\MQBTailGatePageCustomization.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 37, 28);
			BindingExtension bindingExtension4;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension4 = new BindingExtension(), new Uri("Coding\\Pages\\MQBTailGatePageCustomization.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 37, 28);
			BindingExtension bindingExtension5;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension5 = new BindingExtension(), new Uri("Coding\\Pages\\MQBTailGatePageCustomization.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 37, 123);
			Label label3;
			VisualDiagnostics.RegisterSourceInfo(label3 = new Label(), new Uri("Coding\\Pages\\MQBTailGatePageCustomization.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 37, 22);
			BindingExtension bindingExtension6;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension6 = new BindingExtension(), new Uri("Coding\\Pages\\MQBTailGatePageCustomization.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 38, 34);
			Translate translate2;
			VisualDiagnostics.RegisterSourceInfo(translate2 = new Translate(), new Uri("Coding\\Pages\\MQBTailGatePageCustomization.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 39, 32);
			Label label4;
			VisualDiagnostics.RegisterSourceInfo(label4 = new Label(), new Uri("Coding\\Pages\\MQBTailGatePageCustomization.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 39, 26);
			BindingExtension bindingExtension7;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension7 = new BindingExtension(), new Uri("Coding\\Pages\\MQBTailGatePageCustomization.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 40, 55);
			Entry entry;
			VisualDiagnostics.RegisterSourceInfo(entry = new Entry(), new Uri("Coding\\Pages\\MQBTailGatePageCustomization.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 40, 26);
			StaticResourceExtension staticResourceExtension3;
			VisualDiagnostics.RegisterSourceInfo(staticResourceExtension3 = new StaticResourceExtension(), new Uri("Coding\\Pages\\MQBTailGatePageCustomization.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 41, 56);
			BindingExtension bindingExtension8;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension8 = new BindingExtension(), new Uri("Coding\\Pages\\MQBTailGatePageCustomization.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 41, 56);
			Translate translate3;
			VisualDiagnostics.RegisterSourceInfo(translate3 = new Translate(), new Uri("Coding\\Pages\\MQBTailGatePageCustomization.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 45, 43);
			Span span;
			VisualDiagnostics.RegisterSourceInfo(span = new Span(), new Uri("Coding\\Pages\\MQBTailGatePageCustomization.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 45, 38);
			BindingExtension bindingExtension9;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension9 = new BindingExtension(), new Uri("Coding\\Pages\\MQBTailGatePageCustomization.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 46, 43);
			Span span2;
			VisualDiagnostics.RegisterSourceInfo(span2 = new Span(), new Uri("Coding\\Pages\\MQBTailGatePageCustomization.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 46, 38);
			FormattedString formattedString;
			VisualDiagnostics.RegisterSourceInfo(formattedString = new FormattedString(), new Uri("Coding\\Pages\\MQBTailGatePageCustomization.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 44, 34);
			Label label5;
			VisualDiagnostics.RegisterSourceInfo(label5 = new Label(), new Uri("Coding\\Pages\\MQBTailGatePageCustomization.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 41, 26);
			StaticResourceExtension staticResourceExtension4;
			VisualDiagnostics.RegisterSourceInfo(staticResourceExtension4 = new StaticResourceExtension(), new Uri("Coding\\Pages\\MQBTailGatePageCustomization.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 52, 29);
			BindingExtension bindingExtension10;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension10 = new BindingExtension(), new Uri("Coding\\Pages\\MQBTailGatePageCustomization.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 52, 29);
			Translate translate4;
			VisualDiagnostics.RegisterSourceInfo(translate4 = new Translate(), new Uri("Coding\\Pages\\MQBTailGatePageCustomization.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 53, 29);
			Label label6;
			VisualDiagnostics.RegisterSourceInfo(label6 = new Label(), new Uri("Coding\\Pages\\MQBTailGatePageCustomization.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 50, 26);
			StackLayout stackLayout;
			VisualDiagnostics.RegisterSourceInfo(stackLayout = new StackLayout(), new Uri("Coding\\Pages\\MQBTailGatePageCustomization.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 38, 22);
			BindingExtension bindingExtension11;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension11 = new BindingExtension(), new Uri("Coding\\Pages\\MQBTailGatePageCustomization.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 55, 48);
			Translate translate5;
			VisualDiagnostics.RegisterSourceInfo(translate5 = new Translate(), new Uri("Coding\\Pages\\MQBTailGatePageCustomization.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 58, 39);
			Span span3;
			VisualDiagnostics.RegisterSourceInfo(span3 = new Span(), new Uri("Coding\\Pages\\MQBTailGatePageCustomization.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 58, 34);
			Span span4;
			VisualDiagnostics.RegisterSourceInfo(span4 = new Span(), new Uri("Coding\\Pages\\MQBTailGatePageCustomization.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 59, 34);
			BindingExtension bindingExtension12;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension12 = new BindingExtension(), new Uri("Coding\\Pages\\MQBTailGatePageCustomization.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 60, 39);
			Span span5;
			VisualDiagnostics.RegisterSourceInfo(span5 = new Span(), new Uri("Coding\\Pages\\MQBTailGatePageCustomization.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 60, 34);
			FormattedString formattedString2;
			VisualDiagnostics.RegisterSourceInfo(formattedString2 = new FormattedString(), new Uri("Coding\\Pages\\MQBTailGatePageCustomization.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 57, 30);
			Label label7;
			VisualDiagnostics.RegisterSourceInfo(label7 = new Label(), new Uri("Coding\\Pages\\MQBTailGatePageCustomization.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 55, 22);
			BindingExtension bindingExtension13;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension13 = new BindingExtension(), new Uri("Coding\\Pages\\MQBTailGatePageCustomization.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 67, 25);
			Translate translate6;
			VisualDiagnostics.RegisterSourceInfo(translate6 = new Translate(), new Uri("Coding\\Pages\\MQBTailGatePageCustomization.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 68, 25);
			Button button;
			VisualDiagnostics.RegisterSourceInfo(button = new Button(), new Uri("Coding\\Pages\\MQBTailGatePageCustomization.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 64, 22);
			Button button2;
			VisualDiagnostics.RegisterSourceInfo(button2 = new Button(), new Uri("Coding\\Pages\\MQBTailGatePageCustomization.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 69, 22);
			BindingExtension bindingExtension14;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension14 = new BindingExtension(), new Uri("Coding\\Pages\\MQBTailGatePageCustomization.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 74, 34);
			BindingExtension bindingExtension15;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension15 = new BindingExtension(), new Uri("Coding\\Pages\\MQBTailGatePageCustomization.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 77, 29);
			StaticResourceExtension staticResourceExtension5;
			VisualDiagnostics.RegisterSourceInfo(staticResourceExtension5 = new StaticResourceExtension(), new Uri("Coding\\Pages\\MQBTailGatePageCustomization.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 78, 29);
			BindingExtension bindingExtension16;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension16 = new BindingExtension(), new Uri("Coding\\Pages\\MQBTailGatePageCustomization.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 78, 29);
			CheckBoxWithLabel checkBoxWithLabel;
			VisualDiagnostics.RegisterSourceInfo(checkBoxWithLabel = new CheckBoxWithLabel(), new Uri("Coding\\Pages\\MQBTailGatePageCustomization.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 76, 26);
			StaticResourceExtension staticResourceExtension6;
			VisualDiagnostics.RegisterSourceInfo(staticResourceExtension6 = new StaticResourceExtension(), new Uri("Coding\\Pages\\MQBTailGatePageCustomization.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 81, 38);
			BindingExtension bindingExtension17;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension17 = new BindingExtension(), new Uri("Coding\\Pages\\MQBTailGatePageCustomization.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 81, 38);
			DynamicResourceExtension dynamicResourceExtension3;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension3 = new DynamicResourceExtension(), new Uri("Coding\\Pages\\MQBTailGatePageCustomization.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 84, 33);
			DynamicResourceExtension dynamicResourceExtension4;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension4 = new DynamicResourceExtension(), new Uri("Coding\\Pages\\MQBTailGatePageCustomization.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 86, 33);
			Label label8;
			VisualDiagnostics.RegisterSourceInfo(label8 = new Label(), new Uri("Coding\\Pages\\MQBTailGatePageCustomization.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 83, 30);
			BindingExtension bindingExtension18;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension18 = new BindingExtension(), new Uri("Coding\\Pages\\MQBTailGatePageCustomization.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 87, 78);
			Byte64Values byte64Values = Byte64Values.Unknown;
			RadioButtonWithColor radioButtonWithColor;
			VisualDiagnostics.RegisterSourceInfo(radioButtonWithColor = new RadioButtonWithColor(), new Uri("Coding\\Pages\\MQBTailGatePageCustomization.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 89, 34);
			Byte64Values byte64Values2 = Byte64Values.NoEasyClose;
			RadioButtonWithColor radioButtonWithColor2;
			VisualDiagnostics.RegisterSourceInfo(radioButtonWithColor2 = new RadioButtonWithColor(), new Uri("Coding\\Pages\\MQBTailGatePageCustomization.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 93, 34);
			Byte64Values byte64Values3 = Byte64Values.OpenCloseFromRemote;
			RadioButtonWithColor radioButtonWithColor3;
			VisualDiagnostics.RegisterSourceInfo(radioButtonWithColor3 = new RadioButtonWithColor(), new Uri("Coding\\Pages\\MQBTailGatePageCustomization.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 97, 34);
			Byte64Values byte64Values4 = Byte64Values.OnlyOpenFromRemote;
			RadioButtonWithColor radioButtonWithColor4;
			VisualDiagnostics.RegisterSourceInfo(radioButtonWithColor4 = new RadioButtonWithColor(), new Uri("Coding\\Pages\\MQBTailGatePageCustomization.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 101, 34);
			StackLayout stackLayout2;
			VisualDiagnostics.RegisterSourceInfo(stackLayout2 = new StackLayout(), new Uri("Coding\\Pages\\MQBTailGatePageCustomization.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 87, 30);
			BindingExtension bindingExtension19;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension19 = new BindingExtension(), new Uri("Coding\\Pages\\MQBTailGatePageCustomization.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 107, 51);
			CheckBoxWithLabel checkBoxWithLabel2;
			VisualDiagnostics.RegisterSourceInfo(checkBoxWithLabel2 = new CheckBoxWithLabel(), new Uri("Coding\\Pages\\MQBTailGatePageCustomization.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 107, 30);
			BindingExtension bindingExtension20;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension20 = new BindingExtension(), new Uri("Coding\\Pages\\MQBTailGatePageCustomization.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 109, 51);
			CheckBoxWithLabel checkBoxWithLabel3;
			VisualDiagnostics.RegisterSourceInfo(checkBoxWithLabel3 = new CheckBoxWithLabel(), new Uri("Coding\\Pages\\MQBTailGatePageCustomization.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 109, 30);
			BindingExtension bindingExtension21;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension21 = new BindingExtension(), new Uri("Coding\\Pages\\MQBTailGatePageCustomization.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 111, 51);
			CheckBoxWithLabel checkBoxWithLabel4;
			VisualDiagnostics.RegisterSourceInfo(checkBoxWithLabel4 = new CheckBoxWithLabel(), new Uri("Coding\\Pages\\MQBTailGatePageCustomization.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 111, 30);
			BindingExtension bindingExtension22;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension22 = new BindingExtension(), new Uri("Coding\\Pages\\MQBTailGatePageCustomization.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 113, 51);
			CheckBoxWithLabel checkBoxWithLabel5;
			VisualDiagnostics.RegisterSourceInfo(checkBoxWithLabel5 = new CheckBoxWithLabel(), new Uri("Coding\\Pages\\MQBTailGatePageCustomization.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 113, 30);
			BindingExtension bindingExtension23;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension23 = new BindingExtension(), new Uri("Coding\\Pages\\MQBTailGatePageCustomization.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 115, 78);
			DynamicResourceExtension dynamicResourceExtension5;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension5 = new DynamicResourceExtension(), new Uri("Coding\\Pages\\MQBTailGatePageCustomization.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 117, 37);
			DynamicResourceExtension dynamicResourceExtension6;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension6 = new DynamicResourceExtension(), new Uri("Coding\\Pages\\MQBTailGatePageCustomization.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 119, 37);
			Label label9;
			VisualDiagnostics.RegisterSourceInfo(label9 = new Label(), new Uri("Coding\\Pages\\MQBTailGatePageCustomization.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 116, 34);
			Byte66LowNibblePartValues byte66LowNibblePartValues = Byte66LowNibblePartValues.Unknown;
			RadioButtonWithColor radioButtonWithColor5;
			VisualDiagnostics.RegisterSourceInfo(radioButtonWithColor5 = new RadioButtonWithColor(), new Uri("Coding\\Pages\\MQBTailGatePageCustomization.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 120, 34);
			Byte66LowNibblePartValues byte66LowNibblePartValues2 = Byte66LowNibblePartValues.AllowOpenAndCloseWhileIgnitionOnAndOff;
			RadioButtonWithColor radioButtonWithColor6;
			VisualDiagnostics.RegisterSourceInfo(radioButtonWithColor6 = new RadioButtonWithColor(), new Uri("Coding\\Pages\\MQBTailGatePageCustomization.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 124, 34);
			Byte66LowNibblePartValues byte66LowNibblePartValues3 = Byte66LowNibblePartValues.AllowClosingWhileEngineRunning;
			RadioButtonWithColor radioButtonWithColor7;
			VisualDiagnostics.RegisterSourceInfo(radioButtonWithColor7 = new RadioButtonWithColor(), new Uri("Coding\\Pages\\MQBTailGatePageCustomization.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 128, 34);
			Byte66LowNibblePartValues byte66LowNibblePartValues4 = Byte66LowNibblePartValues.AllowOnlyOpeningWhileIgnitionTurnedOn;
			RadioButtonWithColor radioButtonWithColor8;
			VisualDiagnostics.RegisterSourceInfo(radioButtonWithColor8 = new RadioButtonWithColor(), new Uri("Coding\\Pages\\MQBTailGatePageCustomization.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 132, 34);
			Byte66LowNibblePartValues byte66LowNibblePartValues5 = Byte66LowNibblePartValues.AllowOnlyOpeningWhileIgnitionTurnedOnOrOff;
			RadioButtonWithColor radioButtonWithColor9;
			VisualDiagnostics.RegisterSourceInfo(radioButtonWithColor9 = new RadioButtonWithColor(), new Uri("Coding\\Pages\\MQBTailGatePageCustomization.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 136, 34);
			Byte66LowNibblePartValues byte66LowNibblePartValues6 = Byte66LowNibblePartValues.AllowOnlyOpeningWhileIgnitionTurnedOff;
			RadioButtonWithColor radioButtonWithColor10;
			VisualDiagnostics.RegisterSourceInfo(radioButtonWithColor10 = new RadioButtonWithColor(), new Uri("Coding\\Pages\\MQBTailGatePageCustomization.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 140, 34);
			StackLayout stackLayout3;
			VisualDiagnostics.RegisterSourceInfo(stackLayout3 = new StackLayout(), new Uri("Coding\\Pages\\MQBTailGatePageCustomization.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 115, 30);
			BindingExtension bindingExtension24;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension24 = new BindingExtension(), new Uri("Coding\\Pages\\MQBTailGatePageCustomization.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 147, 51);
			CheckBoxWithLabel checkBoxWithLabel6;
			VisualDiagnostics.RegisterSourceInfo(checkBoxWithLabel6 = new CheckBoxWithLabel(), new Uri("Coding\\Pages\\MQBTailGatePageCustomization.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 147, 30);
			BindingExtension bindingExtension25;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension25 = new BindingExtension(), new Uri("Coding\\Pages\\MQBTailGatePageCustomization.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 148, 51);
			CheckBoxWithLabel checkBoxWithLabel7;
			VisualDiagnostics.RegisterSourceInfo(checkBoxWithLabel7 = new CheckBoxWithLabel(), new Uri("Coding\\Pages\\MQBTailGatePageCustomization.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 148, 30);
			BindingExtension bindingExtension26;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension26 = new BindingExtension(), new Uri("Coding\\Pages\\MQBTailGatePageCustomization.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 149, 51);
			CheckBoxWithLabel checkBoxWithLabel8;
			VisualDiagnostics.RegisterSourceInfo(checkBoxWithLabel8 = new CheckBoxWithLabel(), new Uri("Coding\\Pages\\MQBTailGatePageCustomization.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 149, 30);
			BindingExtension bindingExtension27;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension27 = new BindingExtension(), new Uri("Coding\\Pages\\MQBTailGatePageCustomization.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 152, 86);
			DynamicResourceExtension dynamicResourceExtension7;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension7 = new DynamicResourceExtension(), new Uri("Coding\\Pages\\MQBTailGatePageCustomization.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 154, 37);
			DynamicResourceExtension dynamicResourceExtension8;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension8 = new DynamicResourceExtension(), new Uri("Coding\\Pages\\MQBTailGatePageCustomization.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 156, 37);
			Label label10;
			VisualDiagnostics.RegisterSourceInfo(label10 = new Label(), new Uri("Coding\\Pages\\MQBTailGatePageCustomization.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 153, 34);
			EasyCloseModes easyCloseModes = EasyCloseModes.Unknown;
			RadioButtonWithColor radioButtonWithColor11;
			VisualDiagnostics.RegisterSourceInfo(radioButtonWithColor11 = new RadioButtonWithColor(), new Uri("Coding\\Pages\\MQBTailGatePageCustomization.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 157, 34);
			EasyCloseModes easyCloseModes2 = EasyCloseModes.Enabled;
			RadioButtonWithColor radioButtonWithColor12;
			VisualDiagnostics.RegisterSourceInfo(radioButtonWithColor12 = new RadioButtonWithColor(), new Uri("Coding\\Pages\\MQBTailGatePageCustomization.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 161, 34);
			EasyCloseModes easyCloseModes3 = EasyCloseModes.Disabled;
			RadioButtonWithColor radioButtonWithColor13;
			VisualDiagnostics.RegisterSourceInfo(radioButtonWithColor13 = new RadioButtonWithColor(), new Uri("Coding\\Pages\\MQBTailGatePageCustomization.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 165, 34);
			StackLayout stackLayout4;
			VisualDiagnostics.RegisterSourceInfo(stackLayout4 = new StackLayout(), new Uri("Coding\\Pages\\MQBTailGatePageCustomization.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 152, 30);
			StackLayout stackLayout5;
			VisualDiagnostics.RegisterSourceInfo(stackLayout5 = new StackLayout(), new Uri("Coding\\Pages\\MQBTailGatePageCustomization.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 81, 26);
			BindingExtension bindingExtension28;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension28 = new BindingExtension(), new Uri("Coding\\Pages\\MQBTailGatePageCustomization.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 173, 47);
			CheckBoxWithLabel checkBoxWithLabel9;
			VisualDiagnostics.RegisterSourceInfo(checkBoxWithLabel9 = new CheckBoxWithLabel(), new Uri("Coding\\Pages\\MQBTailGatePageCustomization.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 173, 26);
			BindingExtension bindingExtension29;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension29 = new BindingExtension(), new Uri("Coding\\Pages\\MQBTailGatePageCustomization.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 174, 38);
			Label label11;
			VisualDiagnostics.RegisterSourceInfo(label11 = new Label(), new Uri("Coding\\Pages\\MQBTailGatePageCustomization.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 175, 30);
			BindingExtension bindingExtension30;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension30 = new BindingExtension(), new Uri("Coding\\Pages\\MQBTailGatePageCustomization.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 176, 36);
			Entry entry2;
			VisualDiagnostics.RegisterSourceInfo(entry2 = new Entry(), new Uri("Coding\\Pages\\MQBTailGatePageCustomization.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 176, 30);
			Label label12;
			VisualDiagnostics.RegisterSourceInfo(label12 = new Label(), new Uri("Coding\\Pages\\MQBTailGatePageCustomization.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 177, 30);
			BindingExtension bindingExtension31;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension31 = new BindingExtension(), new Uri("Coding\\Pages\\MQBTailGatePageCustomization.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 178, 36);
			Entry entry3;
			VisualDiagnostics.RegisterSourceInfo(entry3 = new Entry(), new Uri("Coding\\Pages\\MQBTailGatePageCustomization.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 178, 30);
			Button button3;
			VisualDiagnostics.RegisterSourceInfo(button3 = new Button(), new Uri("Coding\\Pages\\MQBTailGatePageCustomization.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 179, 30);
			StackLayout stackLayout6;
			VisualDiagnostics.RegisterSourceInfo(stackLayout6 = new StackLayout(), new Uri("Coding\\Pages\\MQBTailGatePageCustomization.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 174, 26);
			Button button4;
			VisualDiagnostics.RegisterSourceInfo(button4 = new Button(), new Uri("Coding\\Pages\\MQBTailGatePageCustomization.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 188, 26);
			StackLayout stackLayout7;
			VisualDiagnostics.RegisterSourceInfo(stackLayout7 = new StackLayout(), new Uri("Coding\\Pages\\MQBTailGatePageCustomization.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 74, 22);
			StackLayout stackLayout8;
			VisualDiagnostics.RegisterSourceInfo(stackLayout8 = new StackLayout(), new Uri("Coding\\Pages\\MQBTailGatePageCustomization.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 31, 18);
			ScrollView scrollView;
			VisualDiagnostics.RegisterSourceInfo(scrollView = new ScrollView(), new Uri("Coding\\Pages\\MQBTailGatePageCustomization.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 30, 14);
			ActivityFrame activityFrame;
			VisualDiagnostics.RegisterSourceInfo(activityFrame = new ActivityFrame(), new Uri("Coding\\Pages\\MQBTailGatePageCustomization.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 198, 14);
			Grid grid;
			VisualDiagnostics.RegisterSourceInfo(grid = new Grid(), new Uri("Coding\\Pages\\MQBTailGatePageCustomization.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 29, 10);
			VisualDiagnostics.RegisterSourceInfo(this, new Uri("Coding\\Pages\\MQBTailGatePageCustomization.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 2, 2);
			NameScope nameScope = NameScope.GetNameScope(this) ?? new NameScope();
			NameScope.SetNameScope(this, nameScope);
			nameScope.RegisterName("page", this);
			if (this.StyleId == null)
			{
				this.StyleId = "page";
			}
			nameScope.RegisterName("stackRoot", stackLayout8);
			if (stackLayout8.StyleId == null)
			{
				stackLayout8.StyleId = "stackRoot";
			}
			nameScope.RegisterName("entryPassword", entry);
			if (entry.StyleId == null)
			{
				entry.StyleId = "entryPassword";
			}
			nameScope.RegisterName("labelState", label7);
			if (label7.StyleId == null)
			{
				label7.StyleId = "labelState";
			}
			nameScope.RegisterName("btnUpdateState", button);
			if (button.StyleId == null)
			{
				button.StyleId = "btnUpdateState";
			}
			nameScope.RegisterName("btnChooseOtherVersion", button2);
			if (button2.StyleId == null)
			{
				button2.StyleId = "btnChooseOtherVersion";
			}
			nameScope.RegisterName("btnLoadFromUserBytes", button3);
			if (button3.StyleId == null)
			{
				button3.StyleId = "btnLoadFromUserBytes";
			}
			nameScope.RegisterName("btnApply", button4);
			if (button4.StyleId == null)
			{
				button4.StyleId = "btnApply";
			}
			nameScope.RegisterName("activityFrame", activityFrame);
			if (activityFrame.StyleId == null)
			{
				activityFrame.StyleId = "activityFrame";
			}
			this.page = this;
			this.stackRoot = stackLayout8;
			this.entryPassword = entry;
			this.labelState = label7;
			this.btnUpdateState = button;
			this.btnChooseOtherVersion = button2;
			this.btnLoadFromUserBytes = button3;
			this.btnApply = button4;
			this.activityFrame = activityFrame;
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
			xmlNamespaceResolver.Add("coding", "clr-namespace:CarScannerXamarinForms.Coding");
			xmlNamespaceResolver.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver.Add("mqb", "clr-namespace:CarScannerXamarinForms.Coding.DB.MQB");
			xmlNamespaceResolver.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xmlNamespaceResolver.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider.Add(typeFromHandle2, new XamlTypeResolver(xmlNamespaceResolver, typeof(MQBTailGatePageCustomization).GetTypeInfo().Assembly));
			xamlServiceProvider.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(16, 5)));
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
			xmlNamespaceResolver2.Add("coding", "clr-namespace:CarScannerXamarinForms.Coding");
			xmlNamespaceResolver2.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver2.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver2.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver2.Add("mqb", "clr-namespace:CarScannerXamarinForms.Coding.DB.MQB");
			xmlNamespaceResolver2.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver2.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xmlNamespaceResolver2.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver2.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider2.Add(typeFromHandle4, new XamlTypeResolver(xmlNamespaceResolver2, typeof(MQBTailGatePageCustomization).GetTypeInfo().Assembly));
			xamlServiceProvider2.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(19, 5)));
			DynamicResource dynamicResource = markupExtension2.ProvideValue(xamlServiceProvider2);
			this.SetDynamicResource(VisualElement.BackgroundColorProperty, dynamicResource.Key);
			this.Resources.Add("EmptyStringToFalseConverter", emptyStringToFalseConverter);
			this.Resources.Add("EmptyStringToTrueConverter", emptyStringToTrueConverter);
			this.Resources.Add("VagCodingPlatformToTrueConverter", vagCodingPlatformToTrueConverter);
			this.Resources.Add("BoolToNegativeConverter", boolToNegativeConverter);
			grid.SetValue(Grid.RowDefinitionsProperty, new RowDefinitionCollectionTypeConverter().ConvertFromInvariantString("*"));
			scrollView.SetValue(Grid.RowProperty, 0);
			scrollView.SetValue(ScrollView.OrientationProperty, 0);
			stackLayout8.SetValue(StackLayout.OrientationProperty, 0);
			label.SetValue(Label.FontAttributesProperty, new FontAttributesConverter().ConvertFromInvariantString("Bold"));
			dynamicResourceExtension2.Key = "BaseFontSize+++";
			IMarkupExtension<DynamicResource> markupExtension3 = dynamicResourceExtension2;
			XamlServiceProvider xamlServiceProvider3 = new XamlServiceProvider();
			Type typeFromHandle5 = typeof(IProvideValueTarget);
			object[] array3 = new object[0 + 5];
			array3[0] = label;
			array3[1] = stackLayout8;
			array3[2] = scrollView;
			array3[3] = grid;
			array3[4] = this;
			object obj4;
			xamlServiceProvider3.Add(typeFromHandle5, obj4 = new SimpleValueTargetProvider(array3, Label.FontSizeProperty, nameScope));
			xamlServiceProvider3.Add(typeof(IReferenceProvider), obj4);
			Type typeFromHandle6 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver3 = new XmlNamespaceResolver();
			xmlNamespaceResolver3.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver3.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver3.Add("coding", "clr-namespace:CarScannerXamarinForms.Coding");
			xmlNamespaceResolver3.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver3.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver3.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver3.Add("mqb", "clr-namespace:CarScannerXamarinForms.Coding.DB.MQB");
			xmlNamespaceResolver3.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver3.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xmlNamespaceResolver3.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver3.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider3.Add(typeFromHandle6, new XamlTypeResolver(xmlNamespaceResolver3, typeof(MQBTailGatePageCustomization).GetTypeInfo().Assembly));
			xamlServiceProvider3.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(34, 25)));
			DynamicResource dynamicResource2 = markupExtension3.ProvideValue(xamlServiceProvider3);
			label.SetDynamicResource(Label.FontSizeProperty, dynamicResource2.Key);
			bindingExtension.Path = "Name";
			bindingExtension.TypedBinding = new TypedBinding<TailGateParametrizeCustomizationCoding, string>(delegate(TailGateParametrizeCustomizationCoding A_0)
			{
				if (A_0 != null)
				{
					return new ValueTuple<string, bool>(A_0.Name, true);
				}
				return default(ValueTuple<string, bool>);
			}, delegate(TailGateParametrizeCustomizationCoding A_0, string A_1)
			{
				if (A_0 != null)
				{
					A_0.Name = A_1;
					return;
				}
			}, new Tuple<Func<TailGateParametrizeCustomizationCoding, object>, string>[]
			{
				new Tuple<Func<TailGateParametrizeCustomizationCoding, object>, string>((TailGateParametrizeCustomizationCoding A_0) => A_0, "Name")
			});
			BindingBase bindingBase = bindingExtension.ProvideValue(null);
			label.SetBinding(Label.TextProperty, bindingBase);
			stackLayout8.Children.Add(label);
			staticResourceExtension.Key = "EmptyStringToFalseConverter";
			IMarkupExtension markupExtension4 = staticResourceExtension;
			XamlServiceProvider xamlServiceProvider4 = new XamlServiceProvider();
			Type typeFromHandle7 = typeof(IProvideValueTarget);
			object[] array4 = new object[0 + 6];
			array4[0] = bindingExtension2;
			array4[1] = label2;
			array4[2] = stackLayout8;
			array4[3] = scrollView;
			array4[4] = grid;
			array4[5] = this;
			object obj5;
			xamlServiceProvider4.Add(typeFromHandle7, obj5 = new SimpleValueTargetProvider(array4, typeof(BindingExtension).GetRuntimeProperty("Converter"), nameScope));
			xamlServiceProvider4.Add(typeof(IReferenceProvider), obj5);
			Type typeFromHandle8 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver4 = new XmlNamespaceResolver();
			xmlNamespaceResolver4.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver4.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver4.Add("coding", "clr-namespace:CarScannerXamarinForms.Coding");
			xmlNamespaceResolver4.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver4.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver4.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver4.Add("mqb", "clr-namespace:CarScannerXamarinForms.Coding.DB.MQB");
			xmlNamespaceResolver4.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver4.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xmlNamespaceResolver4.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver4.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider4.Add(typeFromHandle8, new XamlTypeResolver(xmlNamespaceResolver4, typeof(MQBTailGatePageCustomization).GetTypeInfo().Assembly));
			xamlServiceProvider4.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(36, 28)));
			object obj6 = markupExtension4.ProvideValue(xamlServiceProvider4);
			bindingExtension2.Converter = obj6;
			bindingExtension2.Path = "Description";
			bindingExtension2.TypedBinding = new TypedBinding<TailGateParametrizeCustomizationCoding, string>(delegate(TailGateParametrizeCustomizationCoding A_0)
			{
				if (A_0 != null)
				{
					return new ValueTuple<string, bool>(A_0.Description, true);
				}
				return default(ValueTuple<string, bool>);
			}, delegate(TailGateParametrizeCustomizationCoding A_0, string A_1)
			{
				if (A_0 != null)
				{
					A_0.Description = A_1;
					return;
				}
			}, new Tuple<Func<TailGateParametrizeCustomizationCoding, object>, string>[]
			{
				new Tuple<Func<TailGateParametrizeCustomizationCoding, object>, string>((TailGateParametrizeCustomizationCoding A_0) => A_0, "Description")
			});
			BindingBase bindingBase2 = bindingExtension2.ProvideValue(null);
			label2.SetBinding(VisualElement.IsVisibleProperty, bindingBase2);
			bindingExtension3.Path = "Description";
			bindingExtension3.TypedBinding = new TypedBinding<TailGateParametrizeCustomizationCoding, string>(delegate(TailGateParametrizeCustomizationCoding A_0)
			{
				if (A_0 != null)
				{
					return new ValueTuple<string, bool>(A_0.Description, true);
				}
				return default(ValueTuple<string, bool>);
			}, delegate(TailGateParametrizeCustomizationCoding A_0, string A_1)
			{
				if (A_0 != null)
				{
					A_0.Description = A_1;
					return;
				}
			}, new Tuple<Func<TailGateParametrizeCustomizationCoding, object>, string>[]
			{
				new Tuple<Func<TailGateParametrizeCustomizationCoding, object>, string>((TailGateParametrizeCustomizationCoding A_0) => A_0, "Description")
			});
			BindingBase bindingBase3 = bindingExtension3.ProvideValue(null);
			label2.SetBinding(Label.TextProperty, bindingBase3);
			stackLayout8.Children.Add(label2);
			staticResourceExtension2.Key = "EmptyStringToFalseConverter";
			IMarkupExtension markupExtension5 = staticResourceExtension2;
			XamlServiceProvider xamlServiceProvider5 = new XamlServiceProvider();
			Type typeFromHandle9 = typeof(IProvideValueTarget);
			object[] array5 = new object[0 + 6];
			array5[0] = bindingExtension4;
			array5[1] = label3;
			array5[2] = stackLayout8;
			array5[3] = scrollView;
			array5[4] = grid;
			array5[5] = this;
			object obj7;
			xamlServiceProvider5.Add(typeFromHandle9, obj7 = new SimpleValueTargetProvider(array5, typeof(BindingExtension).GetRuntimeProperty("Converter"), nameScope));
			xamlServiceProvider5.Add(typeof(IReferenceProvider), obj7);
			Type typeFromHandle10 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver5 = new XmlNamespaceResolver();
			xmlNamespaceResolver5.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver5.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver5.Add("coding", "clr-namespace:CarScannerXamarinForms.Coding");
			xmlNamespaceResolver5.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver5.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver5.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver5.Add("mqb", "clr-namespace:CarScannerXamarinForms.Coding.DB.MQB");
			xmlNamespaceResolver5.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver5.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xmlNamespaceResolver5.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver5.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider5.Add(typeFromHandle10, new XamlTypeResolver(xmlNamespaceResolver5, typeof(MQBTailGatePageCustomization).GetTypeInfo().Assembly));
			xamlServiceProvider5.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(37, 28)));
			object obj8 = markupExtension5.ProvideValue(xamlServiceProvider5);
			bindingExtension4.Converter = obj8;
			bindingExtension4.Path = "InnerDescription";
			bindingExtension4.TypedBinding = new TypedBinding<TailGateParametrizeCustomizationCoding, string>(delegate(TailGateParametrizeCustomizationCoding A_0)
			{
				if (A_0 != null)
				{
					return new ValueTuple<string, bool>(A_0.InnerDescription, true);
				}
				return default(ValueTuple<string, bool>);
			}, delegate(TailGateParametrizeCustomizationCoding A_0, string A_1)
			{
				if (A_0 != null)
				{
					A_0.InnerDescription = A_1;
					return;
				}
			}, new Tuple<Func<TailGateParametrizeCustomizationCoding, object>, string>[]
			{
				new Tuple<Func<TailGateParametrizeCustomizationCoding, object>, string>((TailGateParametrizeCustomizationCoding A_0) => A_0, "InnerDescription")
			});
			BindingBase bindingBase4 = bindingExtension4.ProvideValue(null);
			label3.SetBinding(VisualElement.IsVisibleProperty, bindingBase4);
			bindingExtension5.Path = "InnerDescription";
			bindingExtension5.TypedBinding = new TypedBinding<TailGateParametrizeCustomizationCoding, string>(delegate(TailGateParametrizeCustomizationCoding A_0)
			{
				if (A_0 != null)
				{
					return new ValueTuple<string, bool>(A_0.InnerDescription, true);
				}
				return default(ValueTuple<string, bool>);
			}, delegate(TailGateParametrizeCustomizationCoding A_0, string A_1)
			{
				if (A_0 != null)
				{
					A_0.InnerDescription = A_1;
					return;
				}
			}, new Tuple<Func<TailGateParametrizeCustomizationCoding, object>, string>[]
			{
				new Tuple<Func<TailGateParametrizeCustomizationCoding, object>, string>((TailGateParametrizeCustomizationCoding A_0) => A_0, "InnerDescription")
			});
			BindingBase bindingBase5 = bindingExtension5.ProvideValue(null);
			label3.SetBinding(Label.TextProperty, bindingBase5);
			stackLayout8.Children.Add(label3);
			bindingExtension6.Path = "PasswordVisible";
			bindingExtension6.TypedBinding = new TypedBinding<TailGateParametrizeCustomizationCoding, bool>(delegate(TailGateParametrizeCustomizationCoding A_0)
			{
				if (A_0 != null)
				{
					return new ValueTuple<bool, bool>(A_0.PasswordVisible, true);
				}
				return default(ValueTuple<bool, bool>);
			}, delegate(TailGateParametrizeCustomizationCoding A_0, bool A_1)
			{
				if (A_0 != null)
				{
					A_0.PasswordVisible = A_1;
					return;
				}
			}, new Tuple<Func<TailGateParametrizeCustomizationCoding, object>, string>[]
			{
				new Tuple<Func<TailGateParametrizeCustomizationCoding, object>, string>((TailGateParametrizeCustomizationCoding A_0) => A_0, "PasswordVisible")
			});
			BindingBase bindingBase6 = bindingExtension6.ProvideValue(null);
			stackLayout.SetBinding(VisualElement.IsVisibleProperty, bindingBase6);
			stackLayout.SetValue(StackLayout.OrientationProperty, 0);
			translate2.Text = "coding_Password";
			IMarkupExtension markupExtension6 = translate2;
			XamlServiceProvider xamlServiceProvider6 = new XamlServiceProvider();
			Type typeFromHandle11 = typeof(IProvideValueTarget);
			object[] array6 = new object[0 + 6];
			array6[0] = label4;
			array6[1] = stackLayout;
			array6[2] = stackLayout8;
			array6[3] = scrollView;
			array6[4] = grid;
			array6[5] = this;
			object obj9;
			xamlServiceProvider6.Add(typeFromHandle11, obj9 = new SimpleValueTargetProvider(array6, Label.TextProperty, nameScope));
			xamlServiceProvider6.Add(typeof(IReferenceProvider), obj9);
			Type typeFromHandle12 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver6 = new XmlNamespaceResolver();
			xmlNamespaceResolver6.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver6.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver6.Add("coding", "clr-namespace:CarScannerXamarinForms.Coding");
			xmlNamespaceResolver6.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver6.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver6.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver6.Add("mqb", "clr-namespace:CarScannerXamarinForms.Coding.DB.MQB");
			xmlNamespaceResolver6.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver6.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xmlNamespaceResolver6.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver6.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider6.Add(typeFromHandle12, new XamlTypeResolver(xmlNamespaceResolver6, typeof(MQBTailGatePageCustomization).GetTypeInfo().Assembly));
			xamlServiceProvider6.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(39, 32)));
			object obj10 = markupExtension6.ProvideValue(xamlServiceProvider6);
			label4.Text = obj10;
			stackLayout.Children.Add(label4);
			bindingExtension7.Mode = 1;
			bindingExtension7.Path = "Password";
			bindingExtension7.TypedBinding = new TypedBinding<TailGateParametrizeCustomizationCoding, string>(delegate(TailGateParametrizeCustomizationCoding A_0)
			{
				if (A_0 != null)
				{
					return new ValueTuple<string, bool>(A_0.Password, true);
				}
				return default(ValueTuple<string, bool>);
			}, delegate(TailGateParametrizeCustomizationCoding A_0, string A_1)
			{
				if (A_0 != null)
				{
					A_0.Password = A_1;
					return;
				}
			}, new Tuple<Func<TailGateParametrizeCustomizationCoding, object>, string>[]
			{
				new Tuple<Func<TailGateParametrizeCustomizationCoding, object>, string>((TailGateParametrizeCustomizationCoding A_0) => A_0, "Password")
			});
			BindingBase bindingBase7 = bindingExtension7.ProvideValue(null);
			entry.SetBinding(Entry.TextProperty, bindingBase7);
			stackLayout.Children.Add(entry);
			label5.SetValue(Label.FontAttributesProperty, new FontAttributesConverter().ConvertFromInvariantString("Italic"));
			staticResourceExtension3.Key = "EmptyStringToFalseConverter";
			IMarkupExtension markupExtension7 = staticResourceExtension3;
			XamlServiceProvider xamlServiceProvider7 = new XamlServiceProvider();
			Type typeFromHandle13 = typeof(IProvideValueTarget);
			object[] array7 = new object[0 + 7];
			array7[0] = bindingExtension8;
			array7[1] = label5;
			array7[2] = stackLayout;
			array7[3] = stackLayout8;
			array7[4] = scrollView;
			array7[5] = grid;
			array7[6] = this;
			object obj11;
			xamlServiceProvider7.Add(typeFromHandle13, obj11 = new SimpleValueTargetProvider(array7, typeof(BindingExtension).GetRuntimeProperty("Converter"), nameScope));
			xamlServiceProvider7.Add(typeof(IReferenceProvider), obj11);
			Type typeFromHandle14 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver7 = new XmlNamespaceResolver();
			xmlNamespaceResolver7.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver7.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver7.Add("coding", "clr-namespace:CarScannerXamarinForms.Coding");
			xmlNamespaceResolver7.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver7.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver7.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver7.Add("mqb", "clr-namespace:CarScannerXamarinForms.Coding.DB.MQB");
			xmlNamespaceResolver7.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver7.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xmlNamespaceResolver7.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver7.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider7.Add(typeFromHandle14, new XamlTypeResolver(xmlNamespaceResolver7, typeof(MQBTailGatePageCustomization).GetTypeInfo().Assembly));
			xamlServiceProvider7.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(41, 56)));
			object obj12 = markupExtension7.ProvideValue(xamlServiceProvider7);
			bindingExtension8.Converter = obj12;
			bindingExtension8.Path = "PasswordHint";
			bindingExtension8.TypedBinding = new TypedBinding<TailGateParametrizeCustomizationCoding, string>(delegate(TailGateParametrizeCustomizationCoding A_0)
			{
				if (A_0 != null)
				{
					return new ValueTuple<string, bool>(A_0.PasswordHint, true);
				}
				return default(ValueTuple<string, bool>);
			}, delegate(TailGateParametrizeCustomizationCoding A_0, string A_1)
			{
				if (A_0 != null)
				{
					A_0.PasswordHint = A_1;
					return;
				}
			}, new Tuple<Func<TailGateParametrizeCustomizationCoding, object>, string>[]
			{
				new Tuple<Func<TailGateParametrizeCustomizationCoding, object>, string>((TailGateParametrizeCustomizationCoding A_0) => A_0, "PasswordHint")
			});
			BindingBase bindingBase8 = bindingExtension8.ProvideValue(null);
			label5.SetBinding(VisualElement.IsVisibleProperty, bindingBase8);
			translate3.Text = "coding_PasswordHint";
			IMarkupExtension markupExtension8 = translate3;
			XamlServiceProvider xamlServiceProvider8 = new XamlServiceProvider();
			Type typeFromHandle15 = typeof(IProvideValueTarget);
			object[] array8 = new object[0 + 8];
			array8[0] = span;
			array8[1] = formattedString;
			array8[2] = label5;
			array8[3] = stackLayout;
			array8[4] = stackLayout8;
			array8[5] = scrollView;
			array8[6] = grid;
			array8[7] = this;
			object obj13;
			xamlServiceProvider8.Add(typeFromHandle15, obj13 = new SimpleValueTargetProvider(array8, Span.TextProperty, nameScope));
			xamlServiceProvider8.Add(typeof(IReferenceProvider), obj13);
			Type typeFromHandle16 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver8 = new XmlNamespaceResolver();
			xmlNamespaceResolver8.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver8.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver8.Add("coding", "clr-namespace:CarScannerXamarinForms.Coding");
			xmlNamespaceResolver8.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver8.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver8.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver8.Add("mqb", "clr-namespace:CarScannerXamarinForms.Coding.DB.MQB");
			xmlNamespaceResolver8.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver8.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xmlNamespaceResolver8.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver8.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider8.Add(typeFromHandle16, new XamlTypeResolver(xmlNamespaceResolver8, typeof(MQBTailGatePageCustomization).GetTypeInfo().Assembly));
			xamlServiceProvider8.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(45, 43)));
			object obj14 = markupExtension8.ProvideValue(xamlServiceProvider8);
			span.Text = obj14;
			formattedString.Spans.Add(span);
			bindingExtension9.Path = "PasswordHint";
			bindingExtension9.TypedBinding = new TypedBinding<TailGateParametrizeCustomizationCoding, string>(delegate(TailGateParametrizeCustomizationCoding A_0)
			{
				if (A_0 != null)
				{
					return new ValueTuple<string, bool>(A_0.PasswordHint, true);
				}
				return default(ValueTuple<string, bool>);
			}, delegate(TailGateParametrizeCustomizationCoding A_0, string A_1)
			{
				if (A_0 != null)
				{
					A_0.PasswordHint = A_1;
					return;
				}
			}, new Tuple<Func<TailGateParametrizeCustomizationCoding, object>, string>[]
			{
				new Tuple<Func<TailGateParametrizeCustomizationCoding, object>, string>((TailGateParametrizeCustomizationCoding A_0) => A_0, "PasswordHint")
			});
			BindingBase bindingBase9 = bindingExtension9.ProvideValue(null);
			span2.SetBinding(Span.TextProperty, bindingBase9);
			formattedString.Spans.Add(span2);
			label5.SetValue(Label.FormattedTextProperty, formattedString);
			stackLayout.Children.Add(label5);
			label6.SetValue(Label.FontAttributesProperty, new FontAttributesConverter().ConvertFromInvariantString("Italic"));
			bindingExtension10.Mode = 2;
			staticResourceExtension4.Key = "EmptyStringToTrueConverter";
			IMarkupExtension markupExtension9 = staticResourceExtension4;
			XamlServiceProvider xamlServiceProvider9 = new XamlServiceProvider();
			Type typeFromHandle17 = typeof(IProvideValueTarget);
			object[] array9 = new object[0 + 7];
			array9[0] = bindingExtension10;
			array9[1] = label6;
			array9[2] = stackLayout;
			array9[3] = stackLayout8;
			array9[4] = scrollView;
			array9[5] = grid;
			array9[6] = this;
			object obj15;
			xamlServiceProvider9.Add(typeFromHandle17, obj15 = new SimpleValueTargetProvider(array9, typeof(BindingExtension).GetRuntimeProperty("Converter"), nameScope));
			xamlServiceProvider9.Add(typeof(IReferenceProvider), obj15);
			Type typeFromHandle18 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver9 = new XmlNamespaceResolver();
			xmlNamespaceResolver9.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver9.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver9.Add("coding", "clr-namespace:CarScannerXamarinForms.Coding");
			xmlNamespaceResolver9.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver9.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver9.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver9.Add("mqb", "clr-namespace:CarScannerXamarinForms.Coding.DB.MQB");
			xmlNamespaceResolver9.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver9.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xmlNamespaceResolver9.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver9.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider9.Add(typeFromHandle18, new XamlTypeResolver(xmlNamespaceResolver9, typeof(MQBTailGatePageCustomization).GetTypeInfo().Assembly));
			xamlServiceProvider9.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(52, 29)));
			object obj16 = markupExtension9.ProvideValue(xamlServiceProvider9);
			bindingExtension10.Converter = obj16;
			bindingExtension10.Path = "Password";
			bindingExtension10.TypedBinding = new TypedBinding<TailGateParametrizeCustomizationCoding, string>(delegate(TailGateParametrizeCustomizationCoding A_0)
			{
				if (A_0 != null)
				{
					return new ValueTuple<string, bool>(A_0.Password, true);
				}
				return default(ValueTuple<string, bool>);
			}, null, new Tuple<Func<TailGateParametrizeCustomizationCoding, object>, string>[]
			{
				new Tuple<Func<TailGateParametrizeCustomizationCoding, object>, string>((TailGateParametrizeCustomizationCoding A_0) => A_0, "Password")
			});
			BindingBase bindingBase10 = bindingExtension10.ProvideValue(null);
			label6.SetBinding(VisualElement.IsVisibleProperty, bindingBase10);
			translate4.Text = "coding_PasswordNotRequired";
			IMarkupExtension markupExtension10 = translate4;
			XamlServiceProvider xamlServiceProvider10 = new XamlServiceProvider();
			Type typeFromHandle19 = typeof(IProvideValueTarget);
			object[] array10 = new object[0 + 6];
			array10[0] = label6;
			array10[1] = stackLayout;
			array10[2] = stackLayout8;
			array10[3] = scrollView;
			array10[4] = grid;
			array10[5] = this;
			object obj17;
			xamlServiceProvider10.Add(typeFromHandle19, obj17 = new SimpleValueTargetProvider(array10, Label.TextProperty, nameScope));
			xamlServiceProvider10.Add(typeof(IReferenceProvider), obj17);
			Type typeFromHandle20 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver10 = new XmlNamespaceResolver();
			xmlNamespaceResolver10.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver10.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver10.Add("coding", "clr-namespace:CarScannerXamarinForms.Coding");
			xmlNamespaceResolver10.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver10.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver10.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver10.Add("mqb", "clr-namespace:CarScannerXamarinForms.Coding.DB.MQB");
			xmlNamespaceResolver10.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver10.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xmlNamespaceResolver10.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver10.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider10.Add(typeFromHandle20, new XamlTypeResolver(xmlNamespaceResolver10, typeof(MQBTailGatePageCustomization).GetTypeInfo().Assembly));
			xamlServiceProvider10.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(53, 29)));
			object obj18 = markupExtension10.ProvideValue(xamlServiceProvider10);
			label6.Text = obj18;
			stackLayout.Children.Add(label6);
			stackLayout8.Children.Add(stackLayout);
			bindingExtension11.Mode = 2;
			bindingExtension11.Path = "HasCurrentState";
			bindingExtension11.TypedBinding = new TypedBinding<TailGateParametrizeCustomizationCoding, bool>(delegate(TailGateParametrizeCustomizationCoding A_0)
			{
				if (A_0 != null)
				{
					return new ValueTuple<bool, bool>(A_0.HasCurrentState, true);
				}
				return default(ValueTuple<bool, bool>);
			}, null, new Tuple<Func<TailGateParametrizeCustomizationCoding, object>, string>[]
			{
				new Tuple<Func<TailGateParametrizeCustomizationCoding, object>, string>((TailGateParametrizeCustomizationCoding A_0) => A_0, "HasCurrentState")
			});
			BindingBase bindingBase11 = bindingExtension11.ProvideValue(null);
			label7.SetBinding(VisualElement.IsVisibleProperty, bindingBase11);
			translate5.Text = "coding_CurrentState";
			IMarkupExtension markupExtension11 = translate5;
			XamlServiceProvider xamlServiceProvider11 = new XamlServiceProvider();
			Type typeFromHandle21 = typeof(IProvideValueTarget);
			object[] array11 = new object[0 + 7];
			array11[0] = span3;
			array11[1] = formattedString2;
			array11[2] = label7;
			array11[3] = stackLayout8;
			array11[4] = scrollView;
			array11[5] = grid;
			array11[6] = this;
			object obj19;
			xamlServiceProvider11.Add(typeFromHandle21, obj19 = new SimpleValueTargetProvider(array11, Span.TextProperty, nameScope));
			xamlServiceProvider11.Add(typeof(IReferenceProvider), obj19);
			Type typeFromHandle22 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver11 = new XmlNamespaceResolver();
			xmlNamespaceResolver11.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver11.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver11.Add("coding", "clr-namespace:CarScannerXamarinForms.Coding");
			xmlNamespaceResolver11.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver11.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver11.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver11.Add("mqb", "clr-namespace:CarScannerXamarinForms.Coding.DB.MQB");
			xmlNamespaceResolver11.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver11.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xmlNamespaceResolver11.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver11.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider11.Add(typeFromHandle22, new XamlTypeResolver(xmlNamespaceResolver11, typeof(MQBTailGatePageCustomization).GetTypeInfo().Assembly));
			xamlServiceProvider11.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(58, 39)));
			object obj20 = markupExtension11.ProvideValue(xamlServiceProvider11);
			span3.Text = obj20;
			formattedString2.Spans.Add(span3);
			span4.SetValue(Span.TextProperty, " ");
			formattedString2.Spans.Add(span4);
			bindingExtension12.Mode = 2;
			bindingExtension12.Path = "CurrentState";
			bindingExtension12.TypedBinding = new TypedBinding<TailGateParametrizeCustomizationCoding, string>(delegate(TailGateParametrizeCustomizationCoding A_0)
			{
				if (A_0 != null)
				{
					return new ValueTuple<string, bool>(A_0.CurrentState, true);
				}
				return default(ValueTuple<string, bool>);
			}, null, new Tuple<Func<TailGateParametrizeCustomizationCoding, object>, string>[]
			{
				new Tuple<Func<TailGateParametrizeCustomizationCoding, object>, string>((TailGateParametrizeCustomizationCoding A_0) => A_0, "CurrentState")
			});
			BindingBase bindingBase12 = bindingExtension12.ProvideValue(null);
			span5.SetBinding(Span.TextProperty, bindingBase12);
			formattedString2.Spans.Add(span5);
			label7.SetValue(Label.FormattedTextProperty, formattedString2);
			stackLayout8.Children.Add(label7);
			button.Clicked += this.BtnUpdateState_Clicked;
			bindingExtension13.Path = "HasCurrentState";
			bindingExtension13.TypedBinding = new TypedBinding<TailGateParametrizeCustomizationCoding, bool>(delegate(TailGateParametrizeCustomizationCoding A_0)
			{
				if (A_0 != null)
				{
					return new ValueTuple<bool, bool>(A_0.HasCurrentState, true);
				}
				return default(ValueTuple<bool, bool>);
			}, delegate(TailGateParametrizeCustomizationCoding A_0, bool A_1)
			{
				if (A_0 != null)
				{
					A_0.HasCurrentState = A_1;
					return;
				}
			}, new Tuple<Func<TailGateParametrizeCustomizationCoding, object>, string>[]
			{
				new Tuple<Func<TailGateParametrizeCustomizationCoding, object>, string>((TailGateParametrizeCustomizationCoding A_0) => A_0, "HasCurrentState")
			});
			BindingBase bindingBase13 = bindingExtension13.ProvideValue(null);
			button.SetBinding(VisualElement.IsVisibleProperty, bindingBase13);
			translate6.Text = "Mode06Page_btnRefresh.Content";
			IMarkupExtension markupExtension12 = translate6;
			XamlServiceProvider xamlServiceProvider12 = new XamlServiceProvider();
			Type typeFromHandle23 = typeof(IProvideValueTarget);
			object[] array12 = new object[0 + 5];
			array12[0] = button;
			array12[1] = stackLayout8;
			array12[2] = scrollView;
			array12[3] = grid;
			array12[4] = this;
			object obj21;
			xamlServiceProvider12.Add(typeFromHandle23, obj21 = new SimpleValueTargetProvider(array12, Button.TextProperty, nameScope));
			xamlServiceProvider12.Add(typeof(IReferenceProvider), obj21);
			Type typeFromHandle24 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver12 = new XmlNamespaceResolver();
			xmlNamespaceResolver12.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver12.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver12.Add("coding", "clr-namespace:CarScannerXamarinForms.Coding");
			xmlNamespaceResolver12.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver12.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver12.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver12.Add("mqb", "clr-namespace:CarScannerXamarinForms.Coding.DB.MQB");
			xmlNamespaceResolver12.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver12.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xmlNamespaceResolver12.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver12.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider12.Add(typeFromHandle24, new XamlTypeResolver(xmlNamespaceResolver12, typeof(MQBTailGatePageCustomization).GetTypeInfo().Assembly));
			xamlServiceProvider12.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(68, 25)));
			object obj22 = markupExtension12.ProvideValue(xamlServiceProvider12);
			button.Text = obj22;
			stackLayout8.Children.Add(button);
			button2.Clicked += this.btnChooseOtherVersion_Clicked;
			button2.SetValue(Button.TextProperty, "Choose other version");
			stackLayout8.Children.Add(button2);
			bindingExtension14.Mode = 2;
			bindingExtension14.Path = "DisplayControls";
			bindingExtension14.TypedBinding = new TypedBinding<TailGateParametrizeCustomizationCoding, bool>(delegate(TailGateParametrizeCustomizationCoding A_0)
			{
				if (A_0 != null)
				{
					return new ValueTuple<bool, bool>(A_0.DisplayControls, true);
				}
				return default(ValueTuple<bool, bool>);
			}, null, new Tuple<Func<TailGateParametrizeCustomizationCoding, object>, string>[]
			{
				new Tuple<Func<TailGateParametrizeCustomizationCoding, object>, string>((TailGateParametrizeCustomizationCoding A_0) => A_0, "DisplayControls")
			});
			BindingBase bindingBase14 = bindingExtension14.ProvideValue(null);
			stackLayout7.SetBinding(VisualElement.IsVisibleProperty, bindingBase14);
			stackLayout7.SetValue(StackLayout.OrientationProperty, 0);
			bindingExtension15.Path = "Model.ShouldSetByte62FromLongCoding";
			bindingExtension15.TypedBinding = new TypedBinding<TailGateParametrizeCustomizationCoding, bool>(delegate(TailGateParametrizeCustomizationCoding A_0)
			{
				if (A_0 != null)
				{
					MQB_6D_DatasetBuilderModel model = A_0.Model;
					if (model != null)
					{
						return new ValueTuple<bool, bool>(model.ShouldSetByte62FromLongCoding, true);
					}
				}
				return default(ValueTuple<bool, bool>);
			}, delegate(TailGateParametrizeCustomizationCoding A_0, bool A_1)
			{
				if (A_0 != null)
				{
					MQB_6D_DatasetBuilderModel model2 = A_0.Model;
					if (model2 != null)
					{
						model2.ShouldSetByte62FromLongCoding = A_1;
						return;
					}
				}
			}, new Tuple<Func<TailGateParametrizeCustomizationCoding, object>, string>[]
			{
				new Tuple<Func<TailGateParametrizeCustomizationCoding, object>, string>((TailGateParametrizeCustomizationCoding A_0) => A_0, "Model"),
				new Tuple<Func<TailGateParametrizeCustomizationCoding, object>, string>((TailGateParametrizeCustomizationCoding A_0) => A_0.Model, "ShouldSetByte62FromLongCoding")
			});
			BindingBase bindingBase15 = bindingExtension15.ProvideValue(null);
			checkBoxWithLabel.SetBinding(CheckBoxWithLabel.IsToggledProperty, bindingBase15);
			staticResourceExtension5.Key = "BoolToNegativeConverter";
			IMarkupExtension markupExtension13 = staticResourceExtension5;
			XamlServiceProvider xamlServiceProvider13 = new XamlServiceProvider();
			Type typeFromHandle25 = typeof(IProvideValueTarget);
			object[] array13 = new object[0 + 7];
			array13[0] = bindingExtension16;
			array13[1] = checkBoxWithLabel;
			array13[2] = stackLayout7;
			array13[3] = stackLayout8;
			array13[4] = scrollView;
			array13[5] = grid;
			array13[6] = this;
			object obj23;
			xamlServiceProvider13.Add(typeFromHandle25, obj23 = new SimpleValueTargetProvider(array13, typeof(BindingExtension).GetRuntimeProperty("Converter"), nameScope));
			xamlServiceProvider13.Add(typeof(IReferenceProvider), obj23);
			Type typeFromHandle26 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver13 = new XmlNamespaceResolver();
			xmlNamespaceResolver13.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver13.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver13.Add("coding", "clr-namespace:CarScannerXamarinForms.Coding");
			xmlNamespaceResolver13.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver13.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver13.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver13.Add("mqb", "clr-namespace:CarScannerXamarinForms.Coding.DB.MQB");
			xmlNamespaceResolver13.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver13.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xmlNamespaceResolver13.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver13.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider13.Add(typeFromHandle26, new XamlTypeResolver(xmlNamespaceResolver13, typeof(MQBTailGatePageCustomization).GetTypeInfo().Assembly));
			xamlServiceProvider13.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(78, 29)));
			object obj24 = markupExtension13.ProvideValue(xamlServiceProvider13);
			bindingExtension16.Converter = obj24;
			bindingExtension16.Path = "Model.IsByte62EqualsToLongCodingFirstByte";
			bindingExtension16.TypedBinding = new TypedBinding<TailGateParametrizeCustomizationCoding, bool>(delegate(TailGateParametrizeCustomizationCoding A_0)
			{
				if (A_0 != null)
				{
					MQB_6D_DatasetBuilderModel model3 = A_0.Model;
					if (model3 != null)
					{
						return new ValueTuple<bool, bool>(model3.IsByte62EqualsToLongCodingFirstByte, true);
					}
				}
				return default(ValueTuple<bool, bool>);
			}, null, new Tuple<Func<TailGateParametrizeCustomizationCoding, object>, string>[]
			{
				new Tuple<Func<TailGateParametrizeCustomizationCoding, object>, string>((TailGateParametrizeCustomizationCoding A_0) => A_0, "Model"),
				new Tuple<Func<TailGateParametrizeCustomizationCoding, object>, string>((TailGateParametrizeCustomizationCoding A_0) => A_0.Model, "IsByte62EqualsToLongCodingFirstByte")
			});
			BindingBase bindingBase16 = bindingExtension16.ProvideValue(null);
			checkBoxWithLabel.SetBinding(VisualElement.IsVisibleProperty, bindingBase16);
			checkBoxWithLabel.SetValue(CheckBoxWithLabel.TextProperty, "Make byte 0x62 equal to long coding value");
			stackLayout7.Children.Add(checkBoxWithLabel);
			staticResourceExtension6.Key = "BoolToNegativeConverter";
			IMarkupExtension markupExtension14 = staticResourceExtension6;
			XamlServiceProvider xamlServiceProvider14 = new XamlServiceProvider();
			Type typeFromHandle27 = typeof(IProvideValueTarget);
			object[] array14 = new object[0 + 7];
			array14[0] = bindingExtension17;
			array14[1] = stackLayout5;
			array14[2] = stackLayout7;
			array14[3] = stackLayout8;
			array14[4] = scrollView;
			array14[5] = grid;
			array14[6] = this;
			object obj25;
			xamlServiceProvider14.Add(typeFromHandle27, obj25 = new SimpleValueTargetProvider(array14, typeof(BindingExtension).GetRuntimeProperty("Converter"), nameScope));
			xamlServiceProvider14.Add(typeof(IReferenceProvider), obj25);
			Type typeFromHandle28 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver14 = new XmlNamespaceResolver();
			xmlNamespaceResolver14.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver14.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver14.Add("coding", "clr-namespace:CarScannerXamarinForms.Coding");
			xmlNamespaceResolver14.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver14.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver14.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver14.Add("mqb", "clr-namespace:CarScannerXamarinForms.Coding.DB.MQB");
			xmlNamespaceResolver14.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver14.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xmlNamespaceResolver14.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver14.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider14.Add(typeFromHandle28, new XamlTypeResolver(xmlNamespaceResolver14, typeof(MQBTailGatePageCustomization).GetTypeInfo().Assembly));
			xamlServiceProvider14.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(81, 38)));
			object obj26 = markupExtension14.ProvideValue(xamlServiceProvider14);
			bindingExtension17.Converter = obj26;
			bindingExtension17.Path = "Model.UseUserDefinedBytes";
			bindingExtension17.TypedBinding = new TypedBinding<TailGateParametrizeCustomizationCoding, bool>(delegate(TailGateParametrizeCustomizationCoding A_0)
			{
				if (A_0 != null)
				{
					MQB_6D_DatasetBuilderModel model4 = A_0.Model;
					if (model4 != null)
					{
						return new ValueTuple<bool, bool>(model4.UseUserDefinedBytes, true);
					}
				}
				return default(ValueTuple<bool, bool>);
			}, delegate(TailGateParametrizeCustomizationCoding A_0, bool A_1)
			{
				if (A_0 != null)
				{
					MQB_6D_DatasetBuilderModel model5 = A_0.Model;
					if (model5 != null)
					{
						model5.UseUserDefinedBytes = A_1;
						return;
					}
				}
			}, new Tuple<Func<TailGateParametrizeCustomizationCoding, object>, string>[]
			{
				new Tuple<Func<TailGateParametrizeCustomizationCoding, object>, string>((TailGateParametrizeCustomizationCoding A_0) => A_0, "Model"),
				new Tuple<Func<TailGateParametrizeCustomizationCoding, object>, string>((TailGateParametrizeCustomizationCoding A_0) => A_0.Model, "UseUserDefinedBytes")
			});
			BindingBase bindingBase17 = bindingExtension17.ProvideValue(null);
			stackLayout5.SetBinding(VisualElement.IsVisibleProperty, bindingBase17);
			stackLayout5.SetValue(StackLayout.OrientationProperty, 0);
			dynamicResourceExtension3.Key = "BaseFontSize+";
			IMarkupExtension<DynamicResource> markupExtension15 = dynamicResourceExtension3;
			XamlServiceProvider xamlServiceProvider15 = new XamlServiceProvider();
			Type typeFromHandle29 = typeof(IProvideValueTarget);
			object[] array15 = new object[0 + 7];
			array15[0] = label8;
			array15[1] = stackLayout5;
			array15[2] = stackLayout7;
			array15[3] = stackLayout8;
			array15[4] = scrollView;
			array15[5] = grid;
			array15[6] = this;
			object obj27;
			xamlServiceProvider15.Add(typeFromHandle29, obj27 = new SimpleValueTargetProvider(array15, Label.FontSizeProperty, nameScope));
			xamlServiceProvider15.Add(typeof(IReferenceProvider), obj27);
			Type typeFromHandle30 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver15 = new XmlNamespaceResolver();
			xmlNamespaceResolver15.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver15.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver15.Add("coding", "clr-namespace:CarScannerXamarinForms.Coding");
			xmlNamespaceResolver15.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver15.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver15.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver15.Add("mqb", "clr-namespace:CarScannerXamarinForms.Coding.DB.MQB");
			xmlNamespaceResolver15.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver15.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xmlNamespaceResolver15.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver15.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider15.Add(typeFromHandle30, new XamlTypeResolver(xmlNamespaceResolver15, typeof(MQBTailGatePageCustomization).GetTypeInfo().Assembly));
			xamlServiceProvider15.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(84, 33)));
			DynamicResource dynamicResource3 = markupExtension15.ProvideValue(xamlServiceProvider15);
			label8.SetDynamicResource(Label.FontSizeProperty, dynamicResource3.Key);
			label8.SetValue(Label.TextProperty, "Easy close options:");
			dynamicResourceExtension4.Key = "TextColor";
			IMarkupExtension<DynamicResource> markupExtension16 = dynamicResourceExtension4;
			XamlServiceProvider xamlServiceProvider16 = new XamlServiceProvider();
			Type typeFromHandle31 = typeof(IProvideValueTarget);
			object[] array16 = new object[0 + 7];
			array16[0] = label8;
			array16[1] = stackLayout5;
			array16[2] = stackLayout7;
			array16[3] = stackLayout8;
			array16[4] = scrollView;
			array16[5] = grid;
			array16[6] = this;
			object obj28;
			xamlServiceProvider16.Add(typeFromHandle31, obj28 = new SimpleValueTargetProvider(array16, Label.TextColorProperty, nameScope));
			xamlServiceProvider16.Add(typeof(IReferenceProvider), obj28);
			Type typeFromHandle32 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver16 = new XmlNamespaceResolver();
			xmlNamespaceResolver16.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver16.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver16.Add("coding", "clr-namespace:CarScannerXamarinForms.Coding");
			xmlNamespaceResolver16.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver16.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver16.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver16.Add("mqb", "clr-namespace:CarScannerXamarinForms.Coding.DB.MQB");
			xmlNamespaceResolver16.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver16.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xmlNamespaceResolver16.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver16.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider16.Add(typeFromHandle32, new XamlTypeResolver(xmlNamespaceResolver16, typeof(MQBTailGatePageCustomization).GetTypeInfo().Assembly));
			xamlServiceProvider16.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(86, 33)));
			DynamicResource dynamicResource4 = markupExtension16.ProvideValue(xamlServiceProvider16);
			label8.SetDynamicResource(Label.TextColorProperty, dynamicResource4.Key);
			stackLayout5.Children.Add(label8);
			stackLayout2.SetValue(RadioButtonGroup.GroupNameProperty, "byte64");
			bindingExtension18.Mode = 1;
			bindingExtension18.Path = "Model.Byte64";
			bindingExtension18.TypedBinding = new TypedBinding<TailGateParametrizeCustomizationCoding, Byte64Values>(delegate(TailGateParametrizeCustomizationCoding A_0)
			{
				if (A_0 != null)
				{
					MQB_6D_DatasetBuilderModel model6 = A_0.Model;
					if (model6 != null)
					{
						return new ValueTuple<Byte64Values, bool>(model6.Byte64, true);
					}
				}
				return default(ValueTuple<Byte64Values, bool>);
			}, delegate(TailGateParametrizeCustomizationCoding A_0, Byte64Values A_1)
			{
				if (A_0 != null)
				{
					MQB_6D_DatasetBuilderModel model7 = A_0.Model;
					if (model7 != null)
					{
						model7.Byte64 = A_1;
						return;
					}
				}
			}, new Tuple<Func<TailGateParametrizeCustomizationCoding, object>, string>[]
			{
				new Tuple<Func<TailGateParametrizeCustomizationCoding, object>, string>((TailGateParametrizeCustomizationCoding A_0) => A_0, "Model"),
				new Tuple<Func<TailGateParametrizeCustomizationCoding, object>, string>((TailGateParametrizeCustomizationCoding A_0) => A_0.Model, "Byte64")
			});
			BindingBase bindingBase18 = bindingExtension18.ProvideValue(null);
			stackLayout2.SetBinding(RadioButtonGroup.SelectedValueProperty, bindingBase18);
			radioButtonWithColor.SetValue(RadioButton.ContentProperty, "Unknown/Leave unchanged");
			radioButtonWithColor.SetValue(RadioButton.GroupNameProperty, "byte64");
			radioButtonWithColor.SetValue(RadioButton.ValueProperty, byte64Values);
			stackLayout2.Children.Add(radioButtonWithColor);
			radioButtonWithColor2.SetValue(RadioButton.ContentProperty, "No Easy close");
			radioButtonWithColor2.SetValue(RadioButton.GroupNameProperty, "byte64");
			radioButtonWithColor2.SetValue(RadioButton.ValueProperty, byte64Values2);
			stackLayout2.Children.Add(radioButtonWithColor2);
			radioButtonWithColor3.SetValue(RadioButton.ContentProperty, "Open/Close from remote");
			radioButtonWithColor3.SetValue(RadioButton.GroupNameProperty, "byte64");
			radioButtonWithColor3.SetValue(RadioButton.ValueProperty, byte64Values3);
			stackLayout2.Children.Add(radioButtonWithColor3);
			radioButtonWithColor4.SetValue(RadioButton.ContentProperty, "Only open from remote");
			radioButtonWithColor4.SetValue(RadioButton.GroupNameProperty, "byte64");
			radioButtonWithColor4.SetValue(RadioButton.ValueProperty, byte64Values4);
			stackLayout2.Children.Add(radioButtonWithColor4);
			stackLayout5.Children.Add(stackLayout2);
			bindingExtension19.Mode = 1;
			bindingExtension19.Path = "Model.KickCloseEnabled";
			bindingExtension19.TypedBinding = new TypedBinding<TailGateParametrizeCustomizationCoding, bool>(delegate(TailGateParametrizeCustomizationCoding A_0)
			{
				if (A_0 != null)
				{
					MQB_6D_DatasetBuilderModel model8 = A_0.Model;
					if (model8 != null)
					{
						return new ValueTuple<bool, bool>(model8.KickCloseEnabled, true);
					}
				}
				return default(ValueTuple<bool, bool>);
			}, delegate(TailGateParametrizeCustomizationCoding A_0, bool A_1)
			{
				if (A_0 != null)
				{
					MQB_6D_DatasetBuilderModel model9 = A_0.Model;
					if (model9 != null)
					{
						model9.KickCloseEnabled = A_1;
						return;
					}
				}
			}, new Tuple<Func<TailGateParametrizeCustomizationCoding, object>, string>[]
			{
				new Tuple<Func<TailGateParametrizeCustomizationCoding, object>, string>((TailGateParametrizeCustomizationCoding A_0) => A_0, "Model"),
				new Tuple<Func<TailGateParametrizeCustomizationCoding, object>, string>((TailGateParametrizeCustomizationCoding A_0) => A_0.Model, "KickCloseEnabled")
			});
			BindingBase bindingBase19 = bindingExtension19.ProvideValue(null);
			checkBoxWithLabel2.SetBinding(CheckBoxWithLabel.IsToggledProperty, bindingBase19);
			checkBoxWithLabel2.SetValue(CheckBoxWithLabel.TextProperty, "Kick Close enabled");
			stackLayout5.Children.Add(checkBoxWithLabel2);
			bindingExtension20.Mode = 1;
			bindingExtension20.Path = "Model.IsClosingForbiddenWhenKeyIsNear";
			bindingExtension20.TypedBinding = new TypedBinding<TailGateParametrizeCustomizationCoding, bool>(delegate(TailGateParametrizeCustomizationCoding A_0)
			{
				if (A_0 != null)
				{
					MQB_6D_DatasetBuilderModel model10 = A_0.Model;
					if (model10 != null)
					{
						return new ValueTuple<bool, bool>(model10.IsClosingForbiddenWhenKeyIsNear, true);
					}
				}
				return default(ValueTuple<bool, bool>);
			}, delegate(TailGateParametrizeCustomizationCoding A_0, bool A_1)
			{
				if (A_0 != null)
				{
					MQB_6D_DatasetBuilderModel model11 = A_0.Model;
					if (model11 != null)
					{
						model11.IsClosingForbiddenWhenKeyIsNear = A_1;
						return;
					}
				}
			}, new Tuple<Func<TailGateParametrizeCustomizationCoding, object>, string>[]
			{
				new Tuple<Func<TailGateParametrizeCustomizationCoding, object>, string>((TailGateParametrizeCustomizationCoding A_0) => A_0, "Model"),
				new Tuple<Func<TailGateParametrizeCustomizationCoding, object>, string>((TailGateParametrizeCustomizationCoding A_0) => A_0.Model, "IsClosingForbiddenWhenKeyIsNear")
			});
			BindingBase bindingBase20 = bindingExtension20.ProvideValue(null);
			checkBoxWithLabel3.SetBinding(CheckBoxWithLabel.IsToggledProperty, bindingBase20);
			checkBoxWithLabel3.SetValue(CheckBoxWithLabel.TextProperty, "Closing forbidden when key is near");
			stackLayout5.Children.Add(checkBoxWithLabel3);
			bindingExtension21.Mode = 1;
			bindingExtension21.Path = "Model.IsRemoteClosingForbidden";
			bindingExtension21.TypedBinding = new TypedBinding<TailGateParametrizeCustomizationCoding, bool>(delegate(TailGateParametrizeCustomizationCoding A_0)
			{
				if (A_0 != null)
				{
					MQB_6D_DatasetBuilderModel model12 = A_0.Model;
					if (model12 != null)
					{
						return new ValueTuple<bool, bool>(model12.IsRemoteClosingForbidden, true);
					}
				}
				return default(ValueTuple<bool, bool>);
			}, delegate(TailGateParametrizeCustomizationCoding A_0, bool A_1)
			{
				if (A_0 != null)
				{
					MQB_6D_DatasetBuilderModel model13 = A_0.Model;
					if (model13 != null)
					{
						model13.IsRemoteClosingForbidden = A_1;
						return;
					}
				}
			}, new Tuple<Func<TailGateParametrizeCustomizationCoding, object>, string>[]
			{
				new Tuple<Func<TailGateParametrizeCustomizationCoding, object>, string>((TailGateParametrizeCustomizationCoding A_0) => A_0, "Model"),
				new Tuple<Func<TailGateParametrizeCustomizationCoding, object>, string>((TailGateParametrizeCustomizationCoding A_0) => A_0.Model, "IsRemoteClosingForbidden")
			});
			BindingBase bindingBase21 = bindingExtension21.ProvideValue(null);
			checkBoxWithLabel4.SetBinding(CheckBoxWithLabel.IsToggledProperty, bindingBase21);
			checkBoxWithLabel4.SetValue(CheckBoxWithLabel.TextProperty, "Remote closing forbidden");
			stackLayout5.Children.Add(checkBoxWithLabel4);
			bindingExtension22.Mode = 1;
			bindingExtension22.Path = "Model.IsOpeningForbiddenWhileIgnitionTurnedOn";
			bindingExtension22.TypedBinding = new TypedBinding<TailGateParametrizeCustomizationCoding, bool>(delegate(TailGateParametrizeCustomizationCoding A_0)
			{
				if (A_0 != null)
				{
					MQB_6D_DatasetBuilderModel model14 = A_0.Model;
					if (model14 != null)
					{
						return new ValueTuple<bool, bool>(model14.IsOpeningForbiddenWhileIgnitionTurnedOn, true);
					}
				}
				return default(ValueTuple<bool, bool>);
			}, delegate(TailGateParametrizeCustomizationCoding A_0, bool A_1)
			{
				if (A_0 != null)
				{
					MQB_6D_DatasetBuilderModel model15 = A_0.Model;
					if (model15 != null)
					{
						model15.IsOpeningForbiddenWhileIgnitionTurnedOn = A_1;
						return;
					}
				}
			}, new Tuple<Func<TailGateParametrizeCustomizationCoding, object>, string>[]
			{
				new Tuple<Func<TailGateParametrizeCustomizationCoding, object>, string>((TailGateParametrizeCustomizationCoding A_0) => A_0, "Model"),
				new Tuple<Func<TailGateParametrizeCustomizationCoding, object>, string>((TailGateParametrizeCustomizationCoding A_0) => A_0.Model, "IsOpeningForbiddenWhileIgnitionTurnedOn")
			});
			BindingBase bindingBase22 = bindingExtension22.ProvideValue(null);
			checkBoxWithLabel5.SetBinding(CheckBoxWithLabel.IsToggledProperty, bindingBase22);
			checkBoxWithLabel5.SetValue(CheckBoxWithLabel.TextProperty, "Opening forbidden while ignition turned on");
			stackLayout5.Children.Add(checkBoxWithLabel5);
			stackLayout3.SetValue(RadioButtonGroup.GroupNameProperty, "byte66");
			bindingExtension23.Mode = 1;
			bindingExtension23.Path = "Model.Byte66InteriorButton";
			bindingExtension23.TypedBinding = new TypedBinding<TailGateParametrizeCustomizationCoding, Byte66LowNibblePartValues>(delegate(TailGateParametrizeCustomizationCoding A_0)
			{
				if (A_0 != null)
				{
					MQB_6D_DatasetBuilderModel model16 = A_0.Model;
					if (model16 != null)
					{
						return new ValueTuple<Byte66LowNibblePartValues, bool>(model16.Byte66InteriorButton, true);
					}
				}
				return default(ValueTuple<Byte66LowNibblePartValues, bool>);
			}, delegate(TailGateParametrizeCustomizationCoding A_0, Byte66LowNibblePartValues A_1)
			{
				if (A_0 != null)
				{
					MQB_6D_DatasetBuilderModel model17 = A_0.Model;
					if (model17 != null)
					{
						model17.Byte66InteriorButton = A_1;
						return;
					}
				}
			}, new Tuple<Func<TailGateParametrizeCustomizationCoding, object>, string>[]
			{
				new Tuple<Func<TailGateParametrizeCustomizationCoding, object>, string>((TailGateParametrizeCustomizationCoding A_0) => A_0, "Model"),
				new Tuple<Func<TailGateParametrizeCustomizationCoding, object>, string>((TailGateParametrizeCustomizationCoding A_0) => A_0.Model, "Byte66InteriorButton")
			});
			BindingBase bindingBase23 = bindingExtension23.ProvideValue(null);
			stackLayout3.SetBinding(RadioButtonGroup.SelectedValueProperty, bindingBase23);
			dynamicResourceExtension5.Key = "BaseFontSize+";
			IMarkupExtension<DynamicResource> markupExtension17 = dynamicResourceExtension5;
			XamlServiceProvider xamlServiceProvider17 = new XamlServiceProvider();
			Type typeFromHandle33 = typeof(IProvideValueTarget);
			object[] array17 = new object[0 + 8];
			array17[0] = label9;
			array17[1] = stackLayout3;
			array17[2] = stackLayout5;
			array17[3] = stackLayout7;
			array17[4] = stackLayout8;
			array17[5] = scrollView;
			array17[6] = grid;
			array17[7] = this;
			object obj29;
			xamlServiceProvider17.Add(typeFromHandle33, obj29 = new SimpleValueTargetProvider(array17, Label.FontSizeProperty, nameScope));
			xamlServiceProvider17.Add(typeof(IReferenceProvider), obj29);
			Type typeFromHandle34 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver17 = new XmlNamespaceResolver();
			xmlNamespaceResolver17.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver17.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver17.Add("coding", "clr-namespace:CarScannerXamarinForms.Coding");
			xmlNamespaceResolver17.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver17.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver17.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver17.Add("mqb", "clr-namespace:CarScannerXamarinForms.Coding.DB.MQB");
			xmlNamespaceResolver17.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver17.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xmlNamespaceResolver17.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver17.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider17.Add(typeFromHandle34, new XamlTypeResolver(xmlNamespaceResolver17, typeof(MQBTailGatePageCustomization).GetTypeInfo().Assembly));
			xamlServiceProvider17.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(117, 37)));
			DynamicResource dynamicResource5 = markupExtension17.ProvideValue(xamlServiceProvider17);
			label9.SetDynamicResource(Label.FontSizeProperty, dynamicResource5.Key);
			label9.SetValue(Label.TextProperty, "Interior button:");
			dynamicResourceExtension6.Key = "TextColor";
			IMarkupExtension<DynamicResource> markupExtension18 = dynamicResourceExtension6;
			XamlServiceProvider xamlServiceProvider18 = new XamlServiceProvider();
			Type typeFromHandle35 = typeof(IProvideValueTarget);
			object[] array18 = new object[0 + 8];
			array18[0] = label9;
			array18[1] = stackLayout3;
			array18[2] = stackLayout5;
			array18[3] = stackLayout7;
			array18[4] = stackLayout8;
			array18[5] = scrollView;
			array18[6] = grid;
			array18[7] = this;
			object obj30;
			xamlServiceProvider18.Add(typeFromHandle35, obj30 = new SimpleValueTargetProvider(array18, Label.TextColorProperty, nameScope));
			xamlServiceProvider18.Add(typeof(IReferenceProvider), obj30);
			Type typeFromHandle36 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver18 = new XmlNamespaceResolver();
			xmlNamespaceResolver18.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver18.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver18.Add("coding", "clr-namespace:CarScannerXamarinForms.Coding");
			xmlNamespaceResolver18.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver18.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver18.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver18.Add("mqb", "clr-namespace:CarScannerXamarinForms.Coding.DB.MQB");
			xmlNamespaceResolver18.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver18.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xmlNamespaceResolver18.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver18.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider18.Add(typeFromHandle36, new XamlTypeResolver(xmlNamespaceResolver18, typeof(MQBTailGatePageCustomization).GetTypeInfo().Assembly));
			xamlServiceProvider18.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(119, 37)));
			DynamicResource dynamicResource6 = markupExtension18.ProvideValue(xamlServiceProvider18);
			label9.SetDynamicResource(Label.TextColorProperty, dynamicResource6.Key);
			stackLayout3.Children.Add(label9);
			radioButtonWithColor5.SetValue(RadioButton.ContentProperty, "Unknown/Leave unchaged");
			radioButtonWithColor5.SetValue(RadioButton.GroupNameProperty, "byte66");
			radioButtonWithColor5.SetValue(RadioButton.ValueProperty, byte66LowNibblePartValues);
			stackLayout3.Children.Add(radioButtonWithColor5);
			radioButtonWithColor6.SetValue(RadioButton.ContentProperty, "Allow open and close while ignition on and off");
			radioButtonWithColor6.SetValue(RadioButton.GroupNameProperty, "byte66");
			radioButtonWithColor6.SetValue(RadioButton.ValueProperty, byte66LowNibblePartValues2);
			stackLayout3.Children.Add(radioButtonWithColor6);
			radioButtonWithColor7.SetValue(RadioButton.ContentProperty, "Allow closing while engine is running");
			radioButtonWithColor7.SetValue(RadioButton.GroupNameProperty, "byte66");
			radioButtonWithColor7.SetValue(RadioButton.ValueProperty, byte66LowNibblePartValues3);
			stackLayout3.Children.Add(radioButtonWithColor7);
			radioButtonWithColor8.SetValue(RadioButton.ContentProperty, "Allow only opening while ignition is turned On");
			radioButtonWithColor8.SetValue(RadioButton.GroupNameProperty, "byte66");
			radioButtonWithColor8.SetValue(RadioButton.ValueProperty, byte66LowNibblePartValues4);
			stackLayout3.Children.Add(radioButtonWithColor8);
			radioButtonWithColor9.SetValue(RadioButton.ContentProperty, "Allow only opening while ignition is turned On or Off");
			radioButtonWithColor9.SetValue(RadioButton.GroupNameProperty, "byte66");
			radioButtonWithColor9.SetValue(RadioButton.ValueProperty, byte66LowNibblePartValues5);
			stackLayout3.Children.Add(radioButtonWithColor9);
			radioButtonWithColor10.SetValue(RadioButton.ContentProperty, "Allow only opening while ignition is turned Off");
			radioButtonWithColor10.SetValue(RadioButton.GroupNameProperty, "byte66");
			radioButtonWithColor10.SetValue(RadioButton.ValueProperty, byte66LowNibblePartValues6);
			stackLayout3.Children.Add(radioButtonWithColor10);
			stackLayout5.Children.Add(stackLayout3);
			bindingExtension24.Mode = 1;
			bindingExtension24.Path = "Model.InteriorButtonOnlyUnlocksTrunk";
			bindingExtension24.TypedBinding = new TypedBinding<TailGateParametrizeCustomizationCoding, bool>(delegate(TailGateParametrizeCustomizationCoding A_0)
			{
				if (A_0 != null)
				{
					MQB_6D_DatasetBuilderModel model18 = A_0.Model;
					if (model18 != null)
					{
						return new ValueTuple<bool, bool>(model18.InteriorButtonOnlyUnlocksTrunk, true);
					}
				}
				return default(ValueTuple<bool, bool>);
			}, delegate(TailGateParametrizeCustomizationCoding A_0, bool A_1)
			{
				if (A_0 != null)
				{
					MQB_6D_DatasetBuilderModel model19 = A_0.Model;
					if (model19 != null)
					{
						model19.InteriorButtonOnlyUnlocksTrunk = A_1;
						return;
					}
				}
			}, new Tuple<Func<TailGateParametrizeCustomizationCoding, object>, string>[]
			{
				new Tuple<Func<TailGateParametrizeCustomizationCoding, object>, string>((TailGateParametrizeCustomizationCoding A_0) => A_0, "Model"),
				new Tuple<Func<TailGateParametrizeCustomizationCoding, object>, string>((TailGateParametrizeCustomizationCoding A_0) => A_0.Model, "InteriorButtonOnlyUnlocksTrunk")
			});
			BindingBase bindingBase24 = bindingExtension24.ProvideValue(null);
			checkBoxWithLabel6.SetBinding(CheckBoxWithLabel.IsToggledProperty, bindingBase24);
			checkBoxWithLabel6.SetValue(CheckBoxWithLabel.TextProperty, "Interior button: only unlocks trunk");
			stackLayout5.Children.Add(checkBoxWithLabel6);
			bindingExtension25.Mode = 1;
			bindingExtension25.Path = "Model.InteriorButtonCloseByHolding";
			bindingExtension25.TypedBinding = new TypedBinding<TailGateParametrizeCustomizationCoding, bool>(delegate(TailGateParametrizeCustomizationCoding A_0)
			{
				if (A_0 != null)
				{
					MQB_6D_DatasetBuilderModel model20 = A_0.Model;
					if (model20 != null)
					{
						return new ValueTuple<bool, bool>(model20.InteriorButtonCloseByHolding, true);
					}
				}
				return default(ValueTuple<bool, bool>);
			}, delegate(TailGateParametrizeCustomizationCoding A_0, bool A_1)
			{
				if (A_0 != null)
				{
					MQB_6D_DatasetBuilderModel model21 = A_0.Model;
					if (model21 != null)
					{
						model21.InteriorButtonCloseByHolding = A_1;
						return;
					}
				}
			}, new Tuple<Func<TailGateParametrizeCustomizationCoding, object>, string>[]
			{
				new Tuple<Func<TailGateParametrizeCustomizationCoding, object>, string>((TailGateParametrizeCustomizationCoding A_0) => A_0, "Model"),
				new Tuple<Func<TailGateParametrizeCustomizationCoding, object>, string>((TailGateParametrizeCustomizationCoding A_0) => A_0.Model, "InteriorButtonCloseByHolding")
			});
			BindingBase bindingBase25 = bindingExtension25.ProvideValue(null);
			checkBoxWithLabel7.SetBinding(CheckBoxWithLabel.IsToggledProperty, bindingBase25);
			checkBoxWithLabel7.SetValue(CheckBoxWithLabel.TextProperty, "Interior button: close by holding");
			stackLayout5.Children.Add(checkBoxWithLabel7);
			bindingExtension26.Mode = 1;
			bindingExtension26.Path = "Model.InteriorButtonRestricted";
			bindingExtension26.TypedBinding = new TypedBinding<TailGateParametrizeCustomizationCoding, bool>(delegate(TailGateParametrizeCustomizationCoding A_0)
			{
				if (A_0 != null)
				{
					MQB_6D_DatasetBuilderModel model22 = A_0.Model;
					if (model22 != null)
					{
						return new ValueTuple<bool, bool>(model22.InteriorButtonRestricted, true);
					}
				}
				return default(ValueTuple<bool, bool>);
			}, delegate(TailGateParametrizeCustomizationCoding A_0, bool A_1)
			{
				if (A_0 != null)
				{
					MQB_6D_DatasetBuilderModel model23 = A_0.Model;
					if (model23 != null)
					{
						model23.InteriorButtonRestricted = A_1;
						return;
					}
				}
			}, new Tuple<Func<TailGateParametrizeCustomizationCoding, object>, string>[]
			{
				new Tuple<Func<TailGateParametrizeCustomizationCoding, object>, string>((TailGateParametrizeCustomizationCoding A_0) => A_0, "Model"),
				new Tuple<Func<TailGateParametrizeCustomizationCoding, object>, string>((TailGateParametrizeCustomizationCoding A_0) => A_0.Model, "InteriorButtonRestricted")
			});
			BindingBase bindingBase26 = bindingExtension26.ProvideValue(null);
			checkBoxWithLabel8.SetBinding(CheckBoxWithLabel.IsToggledProperty, bindingBase26);
			checkBoxWithLabel8.SetValue(CheckBoxWithLabel.TextProperty, "Interior button: Restricted");
			stackLayout5.Children.Add(checkBoxWithLabel8);
			stackLayout4.SetValue(RadioButtonGroup.GroupNameProperty, "easyCloseModes");
			bindingExtension27.Mode = 1;
			bindingExtension27.Path = "Model.EasyCloseEnabled";
			bindingExtension27.TypedBinding = new TypedBinding<TailGateParametrizeCustomizationCoding, EasyCloseModes>(delegate(TailGateParametrizeCustomizationCoding A_0)
			{
				if (A_0 != null)
				{
					MQB_6D_DatasetBuilderModel model24 = A_0.Model;
					if (model24 != null)
					{
						return new ValueTuple<EasyCloseModes, bool>(model24.EasyCloseEnabled, true);
					}
				}
				return default(ValueTuple<EasyCloseModes, bool>);
			}, delegate(TailGateParametrizeCustomizationCoding A_0, EasyCloseModes A_1)
			{
				if (A_0 != null)
				{
					MQB_6D_DatasetBuilderModel model25 = A_0.Model;
					if (model25 != null)
					{
						model25.EasyCloseEnabled = A_1;
						return;
					}
				}
			}, new Tuple<Func<TailGateParametrizeCustomizationCoding, object>, string>[]
			{
				new Tuple<Func<TailGateParametrizeCustomizationCoding, object>, string>((TailGateParametrizeCustomizationCoding A_0) => A_0, "Model"),
				new Tuple<Func<TailGateParametrizeCustomizationCoding, object>, string>((TailGateParametrizeCustomizationCoding A_0) => A_0.Model, "EasyCloseEnabled")
			});
			BindingBase bindingBase27 = bindingExtension27.ProvideValue(null);
			stackLayout4.SetBinding(RadioButtonGroup.SelectedValueProperty, bindingBase27);
			dynamicResourceExtension7.Key = "BaseFontSize+";
			IMarkupExtension<DynamicResource> markupExtension19 = dynamicResourceExtension7;
			XamlServiceProvider xamlServiceProvider19 = new XamlServiceProvider();
			Type typeFromHandle37 = typeof(IProvideValueTarget);
			object[] array19 = new object[0 + 8];
			array19[0] = label10;
			array19[1] = stackLayout4;
			array19[2] = stackLayout5;
			array19[3] = stackLayout7;
			array19[4] = stackLayout8;
			array19[5] = scrollView;
			array19[6] = grid;
			array19[7] = this;
			object obj31;
			xamlServiceProvider19.Add(typeFromHandle37, obj31 = new SimpleValueTargetProvider(array19, Label.FontSizeProperty, nameScope));
			xamlServiceProvider19.Add(typeof(IReferenceProvider), obj31);
			Type typeFromHandle38 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver19 = new XmlNamespaceResolver();
			xmlNamespaceResolver19.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver19.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver19.Add("coding", "clr-namespace:CarScannerXamarinForms.Coding");
			xmlNamespaceResolver19.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver19.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver19.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver19.Add("mqb", "clr-namespace:CarScannerXamarinForms.Coding.DB.MQB");
			xmlNamespaceResolver19.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver19.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xmlNamespaceResolver19.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver19.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider19.Add(typeFromHandle38, new XamlTypeResolver(xmlNamespaceResolver19, typeof(MQBTailGatePageCustomization).GetTypeInfo().Assembly));
			xamlServiceProvider19.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(154, 37)));
			DynamicResource dynamicResource7 = markupExtension19.ProvideValue(xamlServiceProvider19);
			label10.SetDynamicResource(Label.FontSizeProperty, dynamicResource7.Key);
			label10.SetValue(Label.TextProperty, "Easy close configuration:");
			dynamicResourceExtension8.Key = "TextColor";
			IMarkupExtension<DynamicResource> markupExtension20 = dynamicResourceExtension8;
			XamlServiceProvider xamlServiceProvider20 = new XamlServiceProvider();
			Type typeFromHandle39 = typeof(IProvideValueTarget);
			object[] array20 = new object[0 + 8];
			array20[0] = label10;
			array20[1] = stackLayout4;
			array20[2] = stackLayout5;
			array20[3] = stackLayout7;
			array20[4] = stackLayout8;
			array20[5] = scrollView;
			array20[6] = grid;
			array20[7] = this;
			object obj32;
			xamlServiceProvider20.Add(typeFromHandle39, obj32 = new SimpleValueTargetProvider(array20, Label.TextColorProperty, nameScope));
			xamlServiceProvider20.Add(typeof(IReferenceProvider), obj32);
			Type typeFromHandle40 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver20 = new XmlNamespaceResolver();
			xmlNamespaceResolver20.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver20.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver20.Add("coding", "clr-namespace:CarScannerXamarinForms.Coding");
			xmlNamespaceResolver20.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver20.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver20.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver20.Add("mqb", "clr-namespace:CarScannerXamarinForms.Coding.DB.MQB");
			xmlNamespaceResolver20.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver20.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xmlNamespaceResolver20.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver20.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider20.Add(typeFromHandle40, new XamlTypeResolver(xmlNamespaceResolver20, typeof(MQBTailGatePageCustomization).GetTypeInfo().Assembly));
			xamlServiceProvider20.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(156, 37)));
			DynamicResource dynamicResource8 = markupExtension20.ProvideValue(xamlServiceProvider20);
			label10.SetDynamicResource(Label.TextColorProperty, dynamicResource8.Key);
			stackLayout4.Children.Add(label10);
			radioButtonWithColor11.SetValue(RadioButton.ContentProperty, "Unknown/Leave unchaged");
			radioButtonWithColor11.SetValue(RadioButton.GroupNameProperty, "easyCloseModes");
			radioButtonWithColor11.SetValue(RadioButton.ValueProperty, easyCloseModes);
			stackLayout4.Children.Add(radioButtonWithColor11);
			radioButtonWithColor12.SetValue(RadioButton.ContentProperty, "Enabled");
			radioButtonWithColor12.SetValue(RadioButton.GroupNameProperty, "easyCloseModes");
			radioButtonWithColor12.SetValue(RadioButton.ValueProperty, easyCloseModes2);
			stackLayout4.Children.Add(radioButtonWithColor12);
			radioButtonWithColor13.SetValue(RadioButton.ContentProperty, "Disabled");
			radioButtonWithColor13.SetValue(RadioButton.GroupNameProperty, "easyCloseModes");
			radioButtonWithColor13.SetValue(RadioButton.ValueProperty, easyCloseModes3);
			stackLayout4.Children.Add(radioButtonWithColor13);
			stackLayout5.Children.Add(stackLayout4);
			stackLayout7.Children.Add(stackLayout5);
			bindingExtension28.Mode = 1;
			bindingExtension28.Path = "Model.UseUserDefinedBytes";
			bindingExtension28.TypedBinding = new TypedBinding<TailGateParametrizeCustomizationCoding, bool>(delegate(TailGateParametrizeCustomizationCoding A_0)
			{
				if (A_0 != null)
				{
					MQB_6D_DatasetBuilderModel model26 = A_0.Model;
					if (model26 != null)
					{
						return new ValueTuple<bool, bool>(model26.UseUserDefinedBytes, true);
					}
				}
				return default(ValueTuple<bool, bool>);
			}, delegate(TailGateParametrizeCustomizationCoding A_0, bool A_1)
			{
				if (A_0 != null)
				{
					MQB_6D_DatasetBuilderModel model27 = A_0.Model;
					if (model27 != null)
					{
						model27.UseUserDefinedBytes = A_1;
						return;
					}
				}
			}, new Tuple<Func<TailGateParametrizeCustomizationCoding, object>, string>[]
			{
				new Tuple<Func<TailGateParametrizeCustomizationCoding, object>, string>((TailGateParametrizeCustomizationCoding A_0) => A_0, "Model"),
				new Tuple<Func<TailGateParametrizeCustomizationCoding, object>, string>((TailGateParametrizeCustomizationCoding A_0) => A_0.Model, "UseUserDefinedBytes")
			});
			BindingBase bindingBase28 = bindingExtension28.ProvideValue(null);
			checkBoxWithLabel9.SetBinding(CheckBoxWithLabel.IsToggledProperty, bindingBase28);
			checkBoxWithLabel9.SetValue(CheckBoxWithLabel.TextProperty, "Use user defined bytes");
			stackLayout7.Children.Add(checkBoxWithLabel9);
			bindingExtension29.Mode = 2;
			bindingExtension29.Path = "Model.UseUserDefinedBytes";
			bindingExtension29.TypedBinding = new TypedBinding<TailGateParametrizeCustomizationCoding, bool>(delegate(TailGateParametrizeCustomizationCoding A_0)
			{
				if (A_0 != null)
				{
					MQB_6D_DatasetBuilderModel model28 = A_0.Model;
					if (model28 != null)
					{
						return new ValueTuple<bool, bool>(model28.UseUserDefinedBytes, true);
					}
				}
				return default(ValueTuple<bool, bool>);
			}, null, new Tuple<Func<TailGateParametrizeCustomizationCoding, object>, string>[]
			{
				new Tuple<Func<TailGateParametrizeCustomizationCoding, object>, string>((TailGateParametrizeCustomizationCoding A_0) => A_0, "Model"),
				new Tuple<Func<TailGateParametrizeCustomizationCoding, object>, string>((TailGateParametrizeCustomizationCoding A_0) => A_0.Model, "UseUserDefinedBytes")
			});
			BindingBase bindingBase29 = bindingExtension29.ProvideValue(null);
			stackLayout6.SetBinding(VisualElement.IsVisibleProperty, bindingBase29);
			stackLayout6.SetValue(StackLayout.OrientationProperty, 0);
			label11.SetValue(Label.TextProperty, "Bytes 0x64 - 0x67:");
			stackLayout6.Children.Add(label11);
			bindingExtension30.Mode = 1;
			bindingExtension30.Path = "Model.Bytes64_67";
			bindingExtension30.TypedBinding = new TypedBinding<TailGateParametrizeCustomizationCoding, string>(delegate(TailGateParametrizeCustomizationCoding A_0)
			{
				if (A_0 != null)
				{
					MQB_6D_DatasetBuilderModel model29 = A_0.Model;
					if (model29 != null)
					{
						return new ValueTuple<string, bool>(model29.Bytes64_67, true);
					}
				}
				return default(ValueTuple<string, bool>);
			}, delegate(TailGateParametrizeCustomizationCoding A_0, string A_1)
			{
				if (A_0 != null)
				{
					MQB_6D_DatasetBuilderModel model30 = A_0.Model;
					if (model30 != null)
					{
						model30.Bytes64_67 = A_1;
						return;
					}
				}
			}, new Tuple<Func<TailGateParametrizeCustomizationCoding, object>, string>[]
			{
				new Tuple<Func<TailGateParametrizeCustomizationCoding, object>, string>((TailGateParametrizeCustomizationCoding A_0) => A_0, "Model"),
				new Tuple<Func<TailGateParametrizeCustomizationCoding, object>, string>((TailGateParametrizeCustomizationCoding A_0) => A_0.Model, "Bytes64_67")
			});
			BindingBase bindingBase30 = bindingExtension30.ProvideValue(null);
			entry2.SetBinding(Entry.TextProperty, bindingBase30);
			stackLayout6.Children.Add(entry2);
			label12.SetValue(Label.TextProperty, "Bytes 0x6B - 0x6C:");
			stackLayout6.Children.Add(label12);
			bindingExtension31.Mode = 1;
			bindingExtension31.Path = "Model.Bytes6B_6C";
			bindingExtension31.TypedBinding = new TypedBinding<TailGateParametrizeCustomizationCoding, string>(delegate(TailGateParametrizeCustomizationCoding A_0)
			{
				if (A_0 != null)
				{
					MQB_6D_DatasetBuilderModel model31 = A_0.Model;
					if (model31 != null)
					{
						return new ValueTuple<string, bool>(model31.Bytes6B_6C, true);
					}
				}
				return default(ValueTuple<string, bool>);
			}, delegate(TailGateParametrizeCustomizationCoding A_0, string A_1)
			{
				if (A_0 != null)
				{
					MQB_6D_DatasetBuilderModel model32 = A_0.Model;
					if (model32 != null)
					{
						model32.Bytes6B_6C = A_1;
						return;
					}
				}
			}, new Tuple<Func<TailGateParametrizeCustomizationCoding, object>, string>[]
			{
				new Tuple<Func<TailGateParametrizeCustomizationCoding, object>, string>((TailGateParametrizeCustomizationCoding A_0) => A_0, "Model"),
				new Tuple<Func<TailGateParametrizeCustomizationCoding, object>, string>((TailGateParametrizeCustomizationCoding A_0) => A_0.Model, "Bytes6B_6C")
			});
			BindingBase bindingBase31 = bindingExtension31.ProvideValue(null);
			entry3.SetBinding(Entry.TextProperty, bindingBase31);
			stackLayout6.Children.Add(entry3);
			button3.Clicked += this.BtnLoadFromUserBytes_Clicked;
			button3.SetValue(Button.TextProperty, "Load from user bytes");
			stackLayout6.Children.Add(button3);
			stackLayout7.Children.Add(stackLayout6);
			button4.Clicked += this.btnApply_Clicked;
			button4.SetValue(Button.TextProperty, "Apply");
			stackLayout7.Children.Add(button4);
			stackLayout8.Children.Add(stackLayout7);
			scrollView.Content = stackLayout8;
			grid.Children.Add(scrollView);
			activityFrame.SetValue(Grid.RowProperty, 0);
			activityFrame.SetValue(VisualElement.IsVisibleProperty, new VisualElement.VisibilityConverter().ConvertFromInvariantString("False"));
			grid.Children.Add(activityFrame);
			this.SetValue(ContentPage.ContentProperty, grid);
		}

		// Token: 0x06004EDE RID: 20190 RVA: 0x003C3DFD File Offset: 0x003C1FFD
		[CompilerGenerated]
		private void <btnApply_Clicked>b__8_0(string s)
		{
			Device.BeginInvokeOnMainThread(delegate
			{
				this.activityFrame.Text = s;
			});
		}

		// Token: 0x06004EDF RID: 20191 RVA: 0x003C3E22 File Offset: 0x003C2022
		[CompilerGenerated]
		private void <UpdateState>b__10_0(string s)
		{
			MainThread.BeginInvokeOnMainThread(delegate
			{
				this.activityFrame.Text = ActivityFrame.PLEASE_WAIT_TEXT + "\n" + s;
			});
		}

		// Token: 0x06004EE0 RID: 20192 RVA: 0x003C3E48 File Offset: 0x003C2048
		private void __InitComponentRuntime()
		{
			Extensions.LoadFromXaml<MQBTailGatePageCustomization>(this, typeof(MQBTailGatePageCustomization));
			this.page = NameScopeExtensions.FindByName<ContentPage>(this, "page");
			this.stackRoot = NameScopeExtensions.FindByName<StackLayout>(this, "stackRoot");
			this.entryPassword = NameScopeExtensions.FindByName<Entry>(this, "entryPassword");
			this.labelState = NameScopeExtensions.FindByName<Label>(this, "labelState");
			this.btnUpdateState = NameScopeExtensions.FindByName<Button>(this, "btnUpdateState");
			this.btnChooseOtherVersion = NameScopeExtensions.FindByName<Button>(this, "btnChooseOtherVersion");
			this.btnLoadFromUserBytes = NameScopeExtensions.FindByName<Button>(this, "btnLoadFromUserBytes");
			this.btnApply = NameScopeExtensions.FindByName<Button>(this, "btnApply");
			this.activityFrame = NameScopeExtensions.FindByName<ActivityFrame>(this, "activityFrame");
		}

		// Token: 0x06004EE1 RID: 20193 RVA: 0x003C3F00 File Offset: 0x003C2100
		[CompilerGenerated]
		private static ValueTuple<string, bool> <InitializeComponent>typedBindingsM__84(TailGateParametrizeCustomizationCoding A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<string, bool>(A_0.Name, true);
			}
			return default(ValueTuple<string, bool>);
		}

		// Token: 0x06004EE2 RID: 20194 RVA: 0x003C3F30 File Offset: 0x003C2130
		[CompilerGenerated]
		private static void <InitializeComponent>typedBindingsM__85(TailGateParametrizeCustomizationCoding A_0, string A_1)
		{
			if (A_0 != null)
			{
				A_0.Name = A_1;
				return;
			}
		}

		// Token: 0x06004EE3 RID: 20195 RVA: 0x003C3F4C File Offset: 0x003C214C
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__86(TailGateParametrizeCustomizationCoding A_0)
		{
			return A_0;
		}

		// Token: 0x06004EE4 RID: 20196 RVA: 0x003C3F5C File Offset: 0x003C215C
		[CompilerGenerated]
		private static ValueTuple<string, bool> <InitializeComponent>typedBindingsM__87(TailGateParametrizeCustomizationCoding A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<string, bool>(A_0.Description, true);
			}
			return default(ValueTuple<string, bool>);
		}

		// Token: 0x06004EE5 RID: 20197 RVA: 0x003C3F8C File Offset: 0x003C218C
		[CompilerGenerated]
		private static void <InitializeComponent>typedBindingsM__88(TailGateParametrizeCustomizationCoding A_0, string A_1)
		{
			if (A_0 != null)
			{
				A_0.Description = A_1;
				return;
			}
		}

		// Token: 0x06004EE6 RID: 20198 RVA: 0x003C3FA8 File Offset: 0x003C21A8
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__89(TailGateParametrizeCustomizationCoding A_0)
		{
			return A_0;
		}

		// Token: 0x06004EE7 RID: 20199 RVA: 0x003C3FB8 File Offset: 0x003C21B8
		[CompilerGenerated]
		private static ValueTuple<string, bool> <InitializeComponent>typedBindingsM__90(TailGateParametrizeCustomizationCoding A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<string, bool>(A_0.Description, true);
			}
			return default(ValueTuple<string, bool>);
		}

		// Token: 0x06004EE8 RID: 20200 RVA: 0x003C3FE8 File Offset: 0x003C21E8
		[CompilerGenerated]
		private static void <InitializeComponent>typedBindingsM__91(TailGateParametrizeCustomizationCoding A_0, string A_1)
		{
			if (A_0 != null)
			{
				A_0.Description = A_1;
				return;
			}
		}

		// Token: 0x06004EE9 RID: 20201 RVA: 0x003C4004 File Offset: 0x003C2204
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__92(TailGateParametrizeCustomizationCoding A_0)
		{
			return A_0;
		}

		// Token: 0x06004EEA RID: 20202 RVA: 0x003C4014 File Offset: 0x003C2214
		[CompilerGenerated]
		private static ValueTuple<string, bool> <InitializeComponent>typedBindingsM__93(TailGateParametrizeCustomizationCoding A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<string, bool>(A_0.InnerDescription, true);
			}
			return default(ValueTuple<string, bool>);
		}

		// Token: 0x06004EEB RID: 20203 RVA: 0x003C4044 File Offset: 0x003C2244
		[CompilerGenerated]
		private static void <InitializeComponent>typedBindingsM__94(TailGateParametrizeCustomizationCoding A_0, string A_1)
		{
			if (A_0 != null)
			{
				A_0.InnerDescription = A_1;
				return;
			}
		}

		// Token: 0x06004EEC RID: 20204 RVA: 0x003C4060 File Offset: 0x003C2260
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__95(TailGateParametrizeCustomizationCoding A_0)
		{
			return A_0;
		}

		// Token: 0x06004EED RID: 20205 RVA: 0x003C4070 File Offset: 0x003C2270
		[CompilerGenerated]
		private static ValueTuple<string, bool> <InitializeComponent>typedBindingsM__96(TailGateParametrizeCustomizationCoding A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<string, bool>(A_0.InnerDescription, true);
			}
			return default(ValueTuple<string, bool>);
		}

		// Token: 0x06004EEE RID: 20206 RVA: 0x003C40A0 File Offset: 0x003C22A0
		[CompilerGenerated]
		private static void <InitializeComponent>typedBindingsM__97(TailGateParametrizeCustomizationCoding A_0, string A_1)
		{
			if (A_0 != null)
			{
				A_0.InnerDescription = A_1;
				return;
			}
		}

		// Token: 0x06004EEF RID: 20207 RVA: 0x003C40BC File Offset: 0x003C22BC
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__98(TailGateParametrizeCustomizationCoding A_0)
		{
			return A_0;
		}

		// Token: 0x06004EF0 RID: 20208 RVA: 0x003C40CC File Offset: 0x003C22CC
		[CompilerGenerated]
		private static ValueTuple<bool, bool> <InitializeComponent>typedBindingsM__99(TailGateParametrizeCustomizationCoding A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<bool, bool>(A_0.PasswordVisible, true);
			}
			return default(ValueTuple<bool, bool>);
		}

		// Token: 0x06004EF1 RID: 20209 RVA: 0x003C40FC File Offset: 0x003C22FC
		[CompilerGenerated]
		private static void <InitializeComponent>typedBindingsM__100(TailGateParametrizeCustomizationCoding A_0, bool A_1)
		{
			if (A_0 != null)
			{
				A_0.PasswordVisible = A_1;
				return;
			}
		}

		// Token: 0x06004EF2 RID: 20210 RVA: 0x003C4118 File Offset: 0x003C2318
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__101(TailGateParametrizeCustomizationCoding A_0)
		{
			return A_0;
		}

		// Token: 0x06004EF3 RID: 20211 RVA: 0x003C4128 File Offset: 0x003C2328
		[CompilerGenerated]
		private static ValueTuple<string, bool> <InitializeComponent>typedBindingsM__102(TailGateParametrizeCustomizationCoding A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<string, bool>(A_0.Password, true);
			}
			return default(ValueTuple<string, bool>);
		}

		// Token: 0x06004EF4 RID: 20212 RVA: 0x003C4158 File Offset: 0x003C2358
		[CompilerGenerated]
		private static void <InitializeComponent>typedBindingsM__103(TailGateParametrizeCustomizationCoding A_0, string A_1)
		{
			if (A_0 != null)
			{
				A_0.Password = A_1;
				return;
			}
		}

		// Token: 0x06004EF5 RID: 20213 RVA: 0x003C4174 File Offset: 0x003C2374
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__104(TailGateParametrizeCustomizationCoding A_0)
		{
			return A_0;
		}

		// Token: 0x06004EF6 RID: 20214 RVA: 0x003C4184 File Offset: 0x003C2384
		[CompilerGenerated]
		private static ValueTuple<string, bool> <InitializeComponent>typedBindingsM__105(TailGateParametrizeCustomizationCoding A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<string, bool>(A_0.PasswordHint, true);
			}
			return default(ValueTuple<string, bool>);
		}

		// Token: 0x06004EF7 RID: 20215 RVA: 0x003C41B4 File Offset: 0x003C23B4
		[CompilerGenerated]
		private static void <InitializeComponent>typedBindingsM__106(TailGateParametrizeCustomizationCoding A_0, string A_1)
		{
			if (A_0 != null)
			{
				A_0.PasswordHint = A_1;
				return;
			}
		}

		// Token: 0x06004EF8 RID: 20216 RVA: 0x003C41D0 File Offset: 0x003C23D0
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__107(TailGateParametrizeCustomizationCoding A_0)
		{
			return A_0;
		}

		// Token: 0x06004EF9 RID: 20217 RVA: 0x003C41E0 File Offset: 0x003C23E0
		[CompilerGenerated]
		private static ValueTuple<string, bool> <InitializeComponent>typedBindingsM__108(TailGateParametrizeCustomizationCoding A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<string, bool>(A_0.PasswordHint, true);
			}
			return default(ValueTuple<string, bool>);
		}

		// Token: 0x06004EFA RID: 20218 RVA: 0x003C4210 File Offset: 0x003C2410
		[CompilerGenerated]
		private static void <InitializeComponent>typedBindingsM__109(TailGateParametrizeCustomizationCoding A_0, string A_1)
		{
			if (A_0 != null)
			{
				A_0.PasswordHint = A_1;
				return;
			}
		}

		// Token: 0x06004EFB RID: 20219 RVA: 0x003C422C File Offset: 0x003C242C
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__110(TailGateParametrizeCustomizationCoding A_0)
		{
			return A_0;
		}

		// Token: 0x06004EFC RID: 20220 RVA: 0x003C423C File Offset: 0x003C243C
		[CompilerGenerated]
		private static ValueTuple<string, bool> <InitializeComponent>typedBindingsM__111(TailGateParametrizeCustomizationCoding A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<string, bool>(A_0.Password, true);
			}
			return default(ValueTuple<string, bool>);
		}

		// Token: 0x06004EFD RID: 20221 RVA: 0x003C426C File Offset: 0x003C246C
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__112(TailGateParametrizeCustomizationCoding A_0)
		{
			return A_0;
		}

		// Token: 0x06004EFE RID: 20222 RVA: 0x003C427C File Offset: 0x003C247C
		[CompilerGenerated]
		private static ValueTuple<bool, bool> <InitializeComponent>typedBindingsM__113(TailGateParametrizeCustomizationCoding A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<bool, bool>(A_0.HasCurrentState, true);
			}
			return default(ValueTuple<bool, bool>);
		}

		// Token: 0x06004EFF RID: 20223 RVA: 0x003C42AC File Offset: 0x003C24AC
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__114(TailGateParametrizeCustomizationCoding A_0)
		{
			return A_0;
		}

		// Token: 0x06004F00 RID: 20224 RVA: 0x003C42BC File Offset: 0x003C24BC
		[CompilerGenerated]
		private static ValueTuple<string, bool> <InitializeComponent>typedBindingsM__115(TailGateParametrizeCustomizationCoding A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<string, bool>(A_0.CurrentState, true);
			}
			return default(ValueTuple<string, bool>);
		}

		// Token: 0x06004F01 RID: 20225 RVA: 0x003C42EC File Offset: 0x003C24EC
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__116(TailGateParametrizeCustomizationCoding A_0)
		{
			return A_0;
		}

		// Token: 0x06004F02 RID: 20226 RVA: 0x003C42FC File Offset: 0x003C24FC
		[CompilerGenerated]
		private static ValueTuple<bool, bool> <InitializeComponent>typedBindingsM__117(TailGateParametrizeCustomizationCoding A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<bool, bool>(A_0.HasCurrentState, true);
			}
			return default(ValueTuple<bool, bool>);
		}

		// Token: 0x06004F03 RID: 20227 RVA: 0x003C432C File Offset: 0x003C252C
		[CompilerGenerated]
		private static void <InitializeComponent>typedBindingsM__118(TailGateParametrizeCustomizationCoding A_0, bool A_1)
		{
			if (A_0 != null)
			{
				A_0.HasCurrentState = A_1;
				return;
			}
		}

		// Token: 0x06004F04 RID: 20228 RVA: 0x003C4348 File Offset: 0x003C2548
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__119(TailGateParametrizeCustomizationCoding A_0)
		{
			return A_0;
		}

		// Token: 0x06004F05 RID: 20229 RVA: 0x003C4358 File Offset: 0x003C2558
		[CompilerGenerated]
		private static ValueTuple<bool, bool> <InitializeComponent>typedBindingsM__120(TailGateParametrizeCustomizationCoding A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<bool, bool>(A_0.DisplayControls, true);
			}
			return default(ValueTuple<bool, bool>);
		}

		// Token: 0x06004F06 RID: 20230 RVA: 0x003C4388 File Offset: 0x003C2588
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__121(TailGateParametrizeCustomizationCoding A_0)
		{
			return A_0;
		}

		// Token: 0x06004F07 RID: 20231 RVA: 0x003C4398 File Offset: 0x003C2598
		[CompilerGenerated]
		private static ValueTuple<bool, bool> <InitializeComponent>typedBindingsM__122(TailGateParametrizeCustomizationCoding A_0)
		{
			if (A_0 != null)
			{
				MQB_6D_DatasetBuilderModel model = A_0.Model;
				if (model != null)
				{
					return new ValueTuple<bool, bool>(model.ShouldSetByte62FromLongCoding, true);
				}
			}
			return default(ValueTuple<bool, bool>);
		}

		// Token: 0x06004F08 RID: 20232 RVA: 0x003C43D0 File Offset: 0x003C25D0
		[CompilerGenerated]
		private static void <InitializeComponent>typedBindingsM__123(TailGateParametrizeCustomizationCoding A_0, bool A_1)
		{
			if (A_0 != null)
			{
				MQB_6D_DatasetBuilderModel model = A_0.Model;
				if (model != null)
				{
					model.ShouldSetByte62FromLongCoding = A_1;
					return;
				}
			}
		}

		// Token: 0x06004F09 RID: 20233 RVA: 0x003C43F8 File Offset: 0x003C25F8
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__124(TailGateParametrizeCustomizationCoding A_0)
		{
			return A_0;
		}

		// Token: 0x06004F0A RID: 20234 RVA: 0x003C4408 File Offset: 0x003C2608
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__125(TailGateParametrizeCustomizationCoding A_0)
		{
			return A_0.Model;
		}

		// Token: 0x06004F0B RID: 20235 RVA: 0x003C441C File Offset: 0x003C261C
		[CompilerGenerated]
		private static ValueTuple<bool, bool> <InitializeComponent>typedBindingsM__126(TailGateParametrizeCustomizationCoding A_0)
		{
			if (A_0 != null)
			{
				MQB_6D_DatasetBuilderModel model = A_0.Model;
				if (model != null)
				{
					return new ValueTuple<bool, bool>(model.IsByte62EqualsToLongCodingFirstByte, true);
				}
			}
			return default(ValueTuple<bool, bool>);
		}

		// Token: 0x06004F0C RID: 20236 RVA: 0x003C4454 File Offset: 0x003C2654
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__128(TailGateParametrizeCustomizationCoding A_0)
		{
			return A_0;
		}

		// Token: 0x06004F0D RID: 20237 RVA: 0x003C4464 File Offset: 0x003C2664
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__129(TailGateParametrizeCustomizationCoding A_0)
		{
			return A_0.Model;
		}

		// Token: 0x06004F0E RID: 20238 RVA: 0x003C4478 File Offset: 0x003C2678
		[CompilerGenerated]
		private static ValueTuple<bool, bool> <InitializeComponent>typedBindingsM__130(TailGateParametrizeCustomizationCoding A_0)
		{
			if (A_0 != null)
			{
				MQB_6D_DatasetBuilderModel model = A_0.Model;
				if (model != null)
				{
					return new ValueTuple<bool, bool>(model.UseUserDefinedBytes, true);
				}
			}
			return default(ValueTuple<bool, bool>);
		}

		// Token: 0x06004F0F RID: 20239 RVA: 0x003C44B0 File Offset: 0x003C26B0
		[CompilerGenerated]
		private static void <InitializeComponent>typedBindingsM__131(TailGateParametrizeCustomizationCoding A_0, bool A_1)
		{
			if (A_0 != null)
			{
				MQB_6D_DatasetBuilderModel model = A_0.Model;
				if (model != null)
				{
					model.UseUserDefinedBytes = A_1;
					return;
				}
			}
		}

		// Token: 0x06004F10 RID: 20240 RVA: 0x003C44D8 File Offset: 0x003C26D8
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__132(TailGateParametrizeCustomizationCoding A_0)
		{
			return A_0;
		}

		// Token: 0x06004F11 RID: 20241 RVA: 0x003C44E8 File Offset: 0x003C26E8
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__133(TailGateParametrizeCustomizationCoding A_0)
		{
			return A_0.Model;
		}

		// Token: 0x06004F12 RID: 20242 RVA: 0x003C44FC File Offset: 0x003C26FC
		[CompilerGenerated]
		private static ValueTuple<Byte64Values, bool> <InitializeComponent>typedBindingsM__134(TailGateParametrizeCustomizationCoding A_0)
		{
			if (A_0 != null)
			{
				MQB_6D_DatasetBuilderModel model = A_0.Model;
				if (model != null)
				{
					return new ValueTuple<Byte64Values, bool>(model.Byte64, true);
				}
			}
			return default(ValueTuple<Byte64Values, bool>);
		}

		// Token: 0x06004F13 RID: 20243 RVA: 0x003C4534 File Offset: 0x003C2734
		[CompilerGenerated]
		private static void <InitializeComponent>typedBindingsM__135(TailGateParametrizeCustomizationCoding A_0, Byte64Values A_1)
		{
			if (A_0 != null)
			{
				MQB_6D_DatasetBuilderModel model = A_0.Model;
				if (model != null)
				{
					model.Byte64 = A_1;
					return;
				}
			}
		}

		// Token: 0x06004F14 RID: 20244 RVA: 0x003C455C File Offset: 0x003C275C
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__136(TailGateParametrizeCustomizationCoding A_0)
		{
			return A_0;
		}

		// Token: 0x06004F15 RID: 20245 RVA: 0x003C456C File Offset: 0x003C276C
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__137(TailGateParametrizeCustomizationCoding A_0)
		{
			return A_0.Model;
		}

		// Token: 0x06004F16 RID: 20246 RVA: 0x003C4580 File Offset: 0x003C2780
		[CompilerGenerated]
		private static ValueTuple<bool, bool> <InitializeComponent>typedBindingsM__138(TailGateParametrizeCustomizationCoding A_0)
		{
			if (A_0 != null)
			{
				MQB_6D_DatasetBuilderModel model = A_0.Model;
				if (model != null)
				{
					return new ValueTuple<bool, bool>(model.KickCloseEnabled, true);
				}
			}
			return default(ValueTuple<bool, bool>);
		}

		// Token: 0x06004F17 RID: 20247 RVA: 0x003C45B8 File Offset: 0x003C27B8
		[CompilerGenerated]
		private static void <InitializeComponent>typedBindingsM__139(TailGateParametrizeCustomizationCoding A_0, bool A_1)
		{
			if (A_0 != null)
			{
				MQB_6D_DatasetBuilderModel model = A_0.Model;
				if (model != null)
				{
					model.KickCloseEnabled = A_1;
					return;
				}
			}
		}

		// Token: 0x06004F18 RID: 20248 RVA: 0x003C45E0 File Offset: 0x003C27E0
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__140(TailGateParametrizeCustomizationCoding A_0)
		{
			return A_0;
		}

		// Token: 0x06004F19 RID: 20249 RVA: 0x003C45F0 File Offset: 0x003C27F0
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__141(TailGateParametrizeCustomizationCoding A_0)
		{
			return A_0.Model;
		}

		// Token: 0x06004F1A RID: 20250 RVA: 0x003C4604 File Offset: 0x003C2804
		[CompilerGenerated]
		private static ValueTuple<bool, bool> <InitializeComponent>typedBindingsM__142(TailGateParametrizeCustomizationCoding A_0)
		{
			if (A_0 != null)
			{
				MQB_6D_DatasetBuilderModel model = A_0.Model;
				if (model != null)
				{
					return new ValueTuple<bool, bool>(model.IsClosingForbiddenWhenKeyIsNear, true);
				}
			}
			return default(ValueTuple<bool, bool>);
		}

		// Token: 0x06004F1B RID: 20251 RVA: 0x003C463C File Offset: 0x003C283C
		[CompilerGenerated]
		private static void <InitializeComponent>typedBindingsM__143(TailGateParametrizeCustomizationCoding A_0, bool A_1)
		{
			if (A_0 != null)
			{
				MQB_6D_DatasetBuilderModel model = A_0.Model;
				if (model != null)
				{
					model.IsClosingForbiddenWhenKeyIsNear = A_1;
					return;
				}
			}
		}

		// Token: 0x06004F1C RID: 20252 RVA: 0x003C4664 File Offset: 0x003C2864
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__144(TailGateParametrizeCustomizationCoding A_0)
		{
			return A_0;
		}

		// Token: 0x06004F1D RID: 20253 RVA: 0x003C4674 File Offset: 0x003C2874
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__145(TailGateParametrizeCustomizationCoding A_0)
		{
			return A_0.Model;
		}

		// Token: 0x06004F1E RID: 20254 RVA: 0x003C4688 File Offset: 0x003C2888
		[CompilerGenerated]
		private static ValueTuple<bool, bool> <InitializeComponent>typedBindingsM__146(TailGateParametrizeCustomizationCoding A_0)
		{
			if (A_0 != null)
			{
				MQB_6D_DatasetBuilderModel model = A_0.Model;
				if (model != null)
				{
					return new ValueTuple<bool, bool>(model.IsRemoteClosingForbidden, true);
				}
			}
			return default(ValueTuple<bool, bool>);
		}

		// Token: 0x06004F1F RID: 20255 RVA: 0x003C46C0 File Offset: 0x003C28C0
		[CompilerGenerated]
		private static void <InitializeComponent>typedBindingsM__147(TailGateParametrizeCustomizationCoding A_0, bool A_1)
		{
			if (A_0 != null)
			{
				MQB_6D_DatasetBuilderModel model = A_0.Model;
				if (model != null)
				{
					model.IsRemoteClosingForbidden = A_1;
					return;
				}
			}
		}

		// Token: 0x06004F20 RID: 20256 RVA: 0x003C46E8 File Offset: 0x003C28E8
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__148(TailGateParametrizeCustomizationCoding A_0)
		{
			return A_0;
		}

		// Token: 0x06004F21 RID: 20257 RVA: 0x003C46F8 File Offset: 0x003C28F8
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__149(TailGateParametrizeCustomizationCoding A_0)
		{
			return A_0.Model;
		}

		// Token: 0x06004F22 RID: 20258 RVA: 0x003C470C File Offset: 0x003C290C
		[CompilerGenerated]
		private static ValueTuple<bool, bool> <InitializeComponent>typedBindingsM__150(TailGateParametrizeCustomizationCoding A_0)
		{
			if (A_0 != null)
			{
				MQB_6D_DatasetBuilderModel model = A_0.Model;
				if (model != null)
				{
					return new ValueTuple<bool, bool>(model.IsOpeningForbiddenWhileIgnitionTurnedOn, true);
				}
			}
			return default(ValueTuple<bool, bool>);
		}

		// Token: 0x06004F23 RID: 20259 RVA: 0x003C4744 File Offset: 0x003C2944
		[CompilerGenerated]
		private static void <InitializeComponent>typedBindingsM__151(TailGateParametrizeCustomizationCoding A_0, bool A_1)
		{
			if (A_0 != null)
			{
				MQB_6D_DatasetBuilderModel model = A_0.Model;
				if (model != null)
				{
					model.IsOpeningForbiddenWhileIgnitionTurnedOn = A_1;
					return;
				}
			}
		}

		// Token: 0x06004F24 RID: 20260 RVA: 0x003C476C File Offset: 0x003C296C
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__152(TailGateParametrizeCustomizationCoding A_0)
		{
			return A_0;
		}

		// Token: 0x06004F25 RID: 20261 RVA: 0x003C477C File Offset: 0x003C297C
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__153(TailGateParametrizeCustomizationCoding A_0)
		{
			return A_0.Model;
		}

		// Token: 0x06004F26 RID: 20262 RVA: 0x003C4790 File Offset: 0x003C2990
		[CompilerGenerated]
		private static ValueTuple<Byte66LowNibblePartValues, bool> <InitializeComponent>typedBindingsM__154(TailGateParametrizeCustomizationCoding A_0)
		{
			if (A_0 != null)
			{
				MQB_6D_DatasetBuilderModel model = A_0.Model;
				if (model != null)
				{
					return new ValueTuple<Byte66LowNibblePartValues, bool>(model.Byte66InteriorButton, true);
				}
			}
			return default(ValueTuple<Byte66LowNibblePartValues, bool>);
		}

		// Token: 0x06004F27 RID: 20263 RVA: 0x003C47C8 File Offset: 0x003C29C8
		[CompilerGenerated]
		private static void <InitializeComponent>typedBindingsM__155(TailGateParametrizeCustomizationCoding A_0, Byte66LowNibblePartValues A_1)
		{
			if (A_0 != null)
			{
				MQB_6D_DatasetBuilderModel model = A_0.Model;
				if (model != null)
				{
					model.Byte66InteriorButton = A_1;
					return;
				}
			}
		}

		// Token: 0x06004F28 RID: 20264 RVA: 0x003C47F0 File Offset: 0x003C29F0
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__156(TailGateParametrizeCustomizationCoding A_0)
		{
			return A_0;
		}

		// Token: 0x06004F29 RID: 20265 RVA: 0x003C4800 File Offset: 0x003C2A00
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__157(TailGateParametrizeCustomizationCoding A_0)
		{
			return A_0.Model;
		}

		// Token: 0x06004F2A RID: 20266 RVA: 0x003C4814 File Offset: 0x003C2A14
		[CompilerGenerated]
		private static ValueTuple<bool, bool> <InitializeComponent>typedBindingsM__158(TailGateParametrizeCustomizationCoding A_0)
		{
			if (A_0 != null)
			{
				MQB_6D_DatasetBuilderModel model = A_0.Model;
				if (model != null)
				{
					return new ValueTuple<bool, bool>(model.InteriorButtonOnlyUnlocksTrunk, true);
				}
			}
			return default(ValueTuple<bool, bool>);
		}

		// Token: 0x06004F2B RID: 20267 RVA: 0x003C484C File Offset: 0x003C2A4C
		[CompilerGenerated]
		private static void <InitializeComponent>typedBindingsM__159(TailGateParametrizeCustomizationCoding A_0, bool A_1)
		{
			if (A_0 != null)
			{
				MQB_6D_DatasetBuilderModel model = A_0.Model;
				if (model != null)
				{
					model.InteriorButtonOnlyUnlocksTrunk = A_1;
					return;
				}
			}
		}

		// Token: 0x06004F2C RID: 20268 RVA: 0x003C4874 File Offset: 0x003C2A74
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__160(TailGateParametrizeCustomizationCoding A_0)
		{
			return A_0;
		}

		// Token: 0x06004F2D RID: 20269 RVA: 0x003C4884 File Offset: 0x003C2A84
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__161(TailGateParametrizeCustomizationCoding A_0)
		{
			return A_0.Model;
		}

		// Token: 0x06004F2E RID: 20270 RVA: 0x003C4898 File Offset: 0x003C2A98
		[CompilerGenerated]
		private static ValueTuple<bool, bool> <InitializeComponent>typedBindingsM__162(TailGateParametrizeCustomizationCoding A_0)
		{
			if (A_0 != null)
			{
				MQB_6D_DatasetBuilderModel model = A_0.Model;
				if (model != null)
				{
					return new ValueTuple<bool, bool>(model.InteriorButtonCloseByHolding, true);
				}
			}
			return default(ValueTuple<bool, bool>);
		}

		// Token: 0x06004F2F RID: 20271 RVA: 0x003C48D0 File Offset: 0x003C2AD0
		[CompilerGenerated]
		private static void <InitializeComponent>typedBindingsM__163(TailGateParametrizeCustomizationCoding A_0, bool A_1)
		{
			if (A_0 != null)
			{
				MQB_6D_DatasetBuilderModel model = A_0.Model;
				if (model != null)
				{
					model.InteriorButtonCloseByHolding = A_1;
					return;
				}
			}
		}

		// Token: 0x06004F30 RID: 20272 RVA: 0x003C48F8 File Offset: 0x003C2AF8
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__164(TailGateParametrizeCustomizationCoding A_0)
		{
			return A_0;
		}

		// Token: 0x06004F31 RID: 20273 RVA: 0x003C4908 File Offset: 0x003C2B08
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__165(TailGateParametrizeCustomizationCoding A_0)
		{
			return A_0.Model;
		}

		// Token: 0x06004F32 RID: 20274 RVA: 0x003C491C File Offset: 0x003C2B1C
		[CompilerGenerated]
		private static ValueTuple<bool, bool> <InitializeComponent>typedBindingsM__166(TailGateParametrizeCustomizationCoding A_0)
		{
			if (A_0 != null)
			{
				MQB_6D_DatasetBuilderModel model = A_0.Model;
				if (model != null)
				{
					return new ValueTuple<bool, bool>(model.InteriorButtonRestricted, true);
				}
			}
			return default(ValueTuple<bool, bool>);
		}

		// Token: 0x06004F33 RID: 20275 RVA: 0x003C4954 File Offset: 0x003C2B54
		[CompilerGenerated]
		private static void <InitializeComponent>typedBindingsM__167(TailGateParametrizeCustomizationCoding A_0, bool A_1)
		{
			if (A_0 != null)
			{
				MQB_6D_DatasetBuilderModel model = A_0.Model;
				if (model != null)
				{
					model.InteriorButtonRestricted = A_1;
					return;
				}
			}
		}

		// Token: 0x06004F34 RID: 20276 RVA: 0x003C497C File Offset: 0x003C2B7C
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__168(TailGateParametrizeCustomizationCoding A_0)
		{
			return A_0;
		}

		// Token: 0x06004F35 RID: 20277 RVA: 0x003C498C File Offset: 0x003C2B8C
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__169(TailGateParametrizeCustomizationCoding A_0)
		{
			return A_0.Model;
		}

		// Token: 0x06004F36 RID: 20278 RVA: 0x003C49A0 File Offset: 0x003C2BA0
		[CompilerGenerated]
		private static ValueTuple<EasyCloseModes, bool> <InitializeComponent>typedBindingsM__170(TailGateParametrizeCustomizationCoding A_0)
		{
			if (A_0 != null)
			{
				MQB_6D_DatasetBuilderModel model = A_0.Model;
				if (model != null)
				{
					return new ValueTuple<EasyCloseModes, bool>(model.EasyCloseEnabled, true);
				}
			}
			return default(ValueTuple<EasyCloseModes, bool>);
		}

		// Token: 0x06004F37 RID: 20279 RVA: 0x003C49D8 File Offset: 0x003C2BD8
		[CompilerGenerated]
		private static void <InitializeComponent>typedBindingsM__171(TailGateParametrizeCustomizationCoding A_0, EasyCloseModes A_1)
		{
			if (A_0 != null)
			{
				MQB_6D_DatasetBuilderModel model = A_0.Model;
				if (model != null)
				{
					model.EasyCloseEnabled = A_1;
					return;
				}
			}
		}

		// Token: 0x06004F38 RID: 20280 RVA: 0x003C4A00 File Offset: 0x003C2C00
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__172(TailGateParametrizeCustomizationCoding A_0)
		{
			return A_0;
		}

		// Token: 0x06004F39 RID: 20281 RVA: 0x003C4A10 File Offset: 0x003C2C10
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__173(TailGateParametrizeCustomizationCoding A_0)
		{
			return A_0.Model;
		}

		// Token: 0x06004F3A RID: 20282 RVA: 0x003C4A24 File Offset: 0x003C2C24
		[CompilerGenerated]
		private static ValueTuple<bool, bool> <InitializeComponent>typedBindingsM__174(TailGateParametrizeCustomizationCoding A_0)
		{
			if (A_0 != null)
			{
				MQB_6D_DatasetBuilderModel model = A_0.Model;
				if (model != null)
				{
					return new ValueTuple<bool, bool>(model.UseUserDefinedBytes, true);
				}
			}
			return default(ValueTuple<bool, bool>);
		}

		// Token: 0x06004F3B RID: 20283 RVA: 0x003C4A5C File Offset: 0x003C2C5C
		[CompilerGenerated]
		private static void <InitializeComponent>typedBindingsM__175(TailGateParametrizeCustomizationCoding A_0, bool A_1)
		{
			if (A_0 != null)
			{
				MQB_6D_DatasetBuilderModel model = A_0.Model;
				if (model != null)
				{
					model.UseUserDefinedBytes = A_1;
					return;
				}
			}
		}

		// Token: 0x06004F3C RID: 20284 RVA: 0x003C4A84 File Offset: 0x003C2C84
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__176(TailGateParametrizeCustomizationCoding A_0)
		{
			return A_0;
		}

		// Token: 0x06004F3D RID: 20285 RVA: 0x003C4A94 File Offset: 0x003C2C94
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__177(TailGateParametrizeCustomizationCoding A_0)
		{
			return A_0.Model;
		}

		// Token: 0x06004F3E RID: 20286 RVA: 0x003C4AA8 File Offset: 0x003C2CA8
		[CompilerGenerated]
		private static ValueTuple<bool, bool> <InitializeComponent>typedBindingsM__178(TailGateParametrizeCustomizationCoding A_0)
		{
			if (A_0 != null)
			{
				MQB_6D_DatasetBuilderModel model = A_0.Model;
				if (model != null)
				{
					return new ValueTuple<bool, bool>(model.UseUserDefinedBytes, true);
				}
			}
			return default(ValueTuple<bool, bool>);
		}

		// Token: 0x06004F3F RID: 20287 RVA: 0x003C4AE0 File Offset: 0x003C2CE0
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__179(TailGateParametrizeCustomizationCoding A_0)
		{
			return A_0;
		}

		// Token: 0x06004F40 RID: 20288 RVA: 0x003C4AF0 File Offset: 0x003C2CF0
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__180(TailGateParametrizeCustomizationCoding A_0)
		{
			return A_0.Model;
		}

		// Token: 0x06004F41 RID: 20289 RVA: 0x003C4B04 File Offset: 0x003C2D04
		[CompilerGenerated]
		private static ValueTuple<string, bool> <InitializeComponent>typedBindingsM__181(TailGateParametrizeCustomizationCoding A_0)
		{
			if (A_0 != null)
			{
				MQB_6D_DatasetBuilderModel model = A_0.Model;
				if (model != null)
				{
					return new ValueTuple<string, bool>(model.Bytes64_67, true);
				}
			}
			return default(ValueTuple<string, bool>);
		}

		// Token: 0x06004F42 RID: 20290 RVA: 0x003C4B3C File Offset: 0x003C2D3C
		[CompilerGenerated]
		private static void <InitializeComponent>typedBindingsM__182(TailGateParametrizeCustomizationCoding A_0, string A_1)
		{
			if (A_0 != null)
			{
				MQB_6D_DatasetBuilderModel model = A_0.Model;
				if (model != null)
				{
					model.Bytes64_67 = A_1;
					return;
				}
			}
		}

		// Token: 0x06004F43 RID: 20291 RVA: 0x003C4B64 File Offset: 0x003C2D64
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__183(TailGateParametrizeCustomizationCoding A_0)
		{
			return A_0;
		}

		// Token: 0x06004F44 RID: 20292 RVA: 0x003C4B74 File Offset: 0x003C2D74
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__184(TailGateParametrizeCustomizationCoding A_0)
		{
			return A_0.Model;
		}

		// Token: 0x06004F45 RID: 20293 RVA: 0x003C4B88 File Offset: 0x003C2D88
		[CompilerGenerated]
		private static ValueTuple<string, bool> <InitializeComponent>typedBindingsM__185(TailGateParametrizeCustomizationCoding A_0)
		{
			if (A_0 != null)
			{
				MQB_6D_DatasetBuilderModel model = A_0.Model;
				if (model != null)
				{
					return new ValueTuple<string, bool>(model.Bytes6B_6C, true);
				}
			}
			return default(ValueTuple<string, bool>);
		}

		// Token: 0x06004F46 RID: 20294 RVA: 0x003C4BC0 File Offset: 0x003C2DC0
		[CompilerGenerated]
		private static void <InitializeComponent>typedBindingsM__186(TailGateParametrizeCustomizationCoding A_0, string A_1)
		{
			if (A_0 != null)
			{
				MQB_6D_DatasetBuilderModel model = A_0.Model;
				if (model != null)
				{
					model.Bytes6B_6C = A_1;
					return;
				}
			}
		}

		// Token: 0x06004F47 RID: 20295 RVA: 0x003C4BE8 File Offset: 0x003C2DE8
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__187(TailGateParametrizeCustomizationCoding A_0)
		{
			return A_0;
		}

		// Token: 0x06004F48 RID: 20296 RVA: 0x003C4BF8 File Offset: 0x003C2DF8
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__188(TailGateParametrizeCustomizationCoding A_0)
		{
			return A_0.Model;
		}

		// Token: 0x04002F77 RID: 12151
		private bool first_appearing = true;

		// Token: 0x04002F78 RID: 12152
		[CompilerGenerated]
		private TailGateParametrizeCustomizationCoding <Coding>k__BackingField;

		// Token: 0x04002F79 RID: 12153
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private ContentPage page;

		// Token: 0x04002F7A RID: 12154
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private StackLayout stackRoot;

		// Token: 0x04002F7B RID: 12155
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private Entry entryPassword;

		// Token: 0x04002F7C RID: 12156
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private Label labelState;

		// Token: 0x04002F7D RID: 12157
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private Button btnUpdateState;

		// Token: 0x04002F7E RID: 12158
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private Button btnChooseOtherVersion;

		// Token: 0x04002F7F RID: 12159
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private Button btnLoadFromUserBytes;

		// Token: 0x04002F80 RID: 12160
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private Button btnApply;

		// Token: 0x04002F81 RID: 12161
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private ActivityFrame activityFrame;

		// Token: 0x02000962 RID: 2402
		[CompilerGenerated]
		[Serializable]
		private sealed class <>c
		{
			// Token: 0x06004F49 RID: 20297 RVA: 0x003C4C0B File Offset: 0x003C2E0B
			// Note: this type is marked as 'beforefieldinit'.
			static <>c()
			{
			}

			// Token: 0x06004F4A RID: 20298 RVA: 0x00002050 File Offset: 0x00000250
			public <>c()
			{
			}

			// Token: 0x06004F4B RID: 20299 RVA: 0x00016849 File Offset: 0x00014A49
			internal string <btnChooseOtherVersion_Clicked>b__7_0(string x)
			{
				return x;
			}

			// Token: 0x06004F4C RID: 20300 RVA: 0x003C4C17 File Offset: 0x003C2E17
			internal ValueItemWithTranslation <btnChooseOtherVersion_Clicked>b__7_2(string x)
			{
				return new ValueItemWithTranslation
				{
					Title = Path.GetFileNameWithoutExtension(x).Replace('_', ' ').Trim(' ')
						.ToUpper(),
					Value = x
				};
			}

			// Token: 0x04002F82 RID: 12162
			public static readonly MQBTailGatePageCustomization.<>c <>9 = new MQBTailGatePageCustomization.<>c();

			// Token: 0x04002F83 RID: 12163
			public static Func<string, string> <>9__7_0;

			// Token: 0x04002F84 RID: 12164
			public static Func<string, ValueItemWithTranslation> <>9__7_2;
		}

		// Token: 0x02000963 RID: 2403
		[CompilerGenerated]
		private sealed class <>c__DisplayClass10_0
		{
			// Token: 0x06004F4D RID: 20301 RVA: 0x00002050 File Offset: 0x00000250
			public <>c__DisplayClass10_0()
			{
			}

			// Token: 0x06004F4E RID: 20302 RVA: 0x003C4C46 File Offset: 0x003C2E46
			internal void <UpdateState>b__1()
			{
				this.<>4__this.activityFrame.Text = ActivityFrame.PLEASE_WAIT_TEXT + "\n" + this.s;
			}

			// Token: 0x04002F85 RID: 12165
			public string s;

			// Token: 0x04002F86 RID: 12166
			public MQBTailGatePageCustomization <>4__this;
		}

		// Token: 0x02000964 RID: 2404
		[CompilerGenerated]
		private sealed class <>c__DisplayClass7_0
		{
			// Token: 0x06004F4F RID: 20303 RVA: 0x00002050 File Offset: 0x00000250
			public <>c__DisplayClass7_0()
			{
			}

			// Token: 0x06004F50 RID: 20304 RVA: 0x003C4C6D File Offset: 0x003C2E6D
			internal bool <btnChooseOtherVersion_Clicked>b__1(string x)
			{
				return x.Contains(this.<>4__this.Coding.PartNumber, StringComparison.OrdinalIgnoreCase) || x.Contains(this.<>4__this.Coding.DSVersion, StringComparison.OrdinalIgnoreCase);
			}

			// Token: 0x06004F51 RID: 20305 RVA: 0x003C4CA4 File Offset: 0x003C2EA4
			internal void <btnChooseOtherVersion_Clicked>b__3(ValueItemWithTranslation selected)
			{
				try
				{
					MQB_6D_DatasetBuilderModel mqb_6D_DatasetBuilderModel = new MQB_6D_DatasetBuilderModel();
					using (Stream stream = PackageFileReader.OpenFileStream("vag.tailgate6d." + selected.Value))
					{
						byte[] array = new byte[stream.Length];
						stream.Read(array, 0, array.Length);
						mqb_6D_DatasetBuilderModel.LoadFromBytes(array, this.coding.PartNumber, this.coding.LongCoding);
					}
					this.coding.Model = mqb_6D_DatasetBuilderModel;
					this.coding.DisplayControls = true;
				}
				catch (Exception)
				{
					App.GetCurrentPage().DisplayAlert("Error!", "Error loading dataset " + selected.Title, "OK");
				}
			}

			// Token: 0x04002F87 RID: 12167
			public MQBTailGatePageCustomization <>4__this;

			// Token: 0x04002F88 RID: 12168
			public TailGateParametrizeCustomizationCoding coding;
		}

		// Token: 0x02000965 RID: 2405
		[CompilerGenerated]
		private sealed class <>c__DisplayClass8_0
		{
			// Token: 0x06004F52 RID: 20306 RVA: 0x00002050 File Offset: 0x00000250
			public <>c__DisplayClass8_0()
			{
			}

			// Token: 0x06004F53 RID: 20307 RVA: 0x003C4D70 File Offset: 0x003C2F70
			internal void <btnApply_Clicked>b__1()
			{
				this.<>4__this.activityFrame.Text = this.s;
			}

			// Token: 0x04002F89 RID: 12169
			public string s;

			// Token: 0x04002F8A RID: 12170
			public MQBTailGatePageCustomization <>4__this;
		}

		// Token: 0x02000966 RID: 2406
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <BtnUpdateState_Clicked>d__9 : IAsyncStateMachine
		{
			// Token: 0x06004F54 RID: 20308 RVA: 0x003C4D88 File Offset: 0x003C2F88
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				MQBTailGatePageCustomization mqbtailGatePageCustomization = this;
				try
				{
					TaskAwaiter taskAwaiter;
					if (num != 0)
					{
						taskAwaiter = mqbtailGatePageCustomization.UpdateState().GetAwaiter();
						if (!taskAwaiter.IsCompleted)
						{
							num2 = 0;
							TaskAwaiter taskAwaiter2 = taskAwaiter;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, MQBTailGatePageCustomization.<BtnUpdateState_Clicked>d__9>(ref taskAwaiter, ref this);
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

			// Token: 0x06004F55 RID: 20309 RVA: 0x003C4E3C File Offset: 0x003C303C
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x04002F8B RID: 12171
			public int <>1__state;

			// Token: 0x04002F8C RID: 12172
			public AsyncVoidMethodBuilder <>t__builder;

			// Token: 0x04002F8D RID: 12173
			public MQBTailGatePageCustomization <>4__this;

			// Token: 0x04002F8E RID: 12174
			private TaskAwaiter <>u__1;
		}

		// Token: 0x02000967 RID: 2407
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <MQBTailGatePageCustomization_Appearing>d__2 : IAsyncStateMachine
		{
			// Token: 0x06004F56 RID: 20310 RVA: 0x003C4E4C File Offset: 0x003C304C
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				MQBTailGatePageCustomization mqbtailGatePageCustomization = this;
				try
				{
					TaskAwaiter<CodingRequestResult> taskAwaiter;
					if (num != 0)
					{
						if (!mqbtailGatePageCustomization.first_appearing)
						{
							goto IL_00B7;
						}
						mqbtailGatePageCustomization.first_appearing = false;
						mqbtailGatePageCustomization.stackRoot.IsEnabled = false;
						mqbtailGatePageCustomization.activityFrame.IsVisible = true;
						taskAwaiter = mqbtailGatePageCustomization.Coding.UpdateCurrentState("", null).GetAwaiter();
						if (!taskAwaiter.IsCompleted)
						{
							num2 = 0;
							TaskAwaiter<CodingRequestResult> taskAwaiter2 = taskAwaiter;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<CodingRequestResult>, MQBTailGatePageCustomization.<MQBTailGatePageCustomization_Appearing>d__2>(ref taskAwaiter, ref this);
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
					mqbtailGatePageCustomization.activityFrame.IsVisible = false;
					mqbtailGatePageCustomization.stackRoot.IsEnabled = true;
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

			// Token: 0x06004F57 RID: 20311 RVA: 0x003C4F4C File Offset: 0x003C314C
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x04002F8F RID: 12175
			public int <>1__state;

			// Token: 0x04002F90 RID: 12176
			public AsyncVoidMethodBuilder <>t__builder;

			// Token: 0x04002F91 RID: 12177
			public MQBTailGatePageCustomization <>4__this;

			// Token: 0x04002F92 RID: 12178
			private TaskAwaiter<CodingRequestResult> <>u__1;
		}

		// Token: 0x02000968 RID: 2408
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <UpdateState>d__10 : IAsyncStateMachine
		{
			// Token: 0x06004F58 RID: 20312 RVA: 0x003C4F5C File Offset: 0x003C315C
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				MQBTailGatePageCustomization mqbtailGatePageCustomization = this;
				try
				{
					TaskAwaiter<CodingRequestResult> taskAwaiter;
					if (num != 0)
					{
						mqbtailGatePageCustomization.activityFrame.Text = ActivityFrame.PLEASE_WAIT_TEXT;
						mqbtailGatePageCustomization.activityFrame.IsVisible = true;
						mqbtailGatePageCustomization.stackRoot.IsEnabled = false;
						Progress<string> progress = new Progress<string>(delegate(string s)
						{
							MainThread.BeginInvokeOnMainThread(new Action(new MQBTailGatePageCustomization.<>c__DisplayClass10_0
							{
								<>4__this = mqbtailGatePageCustomization,
								s = s
							}.<UpdateState>b__1));
						});
						if (App.OBDSimulator.IsActive)
						{
							goto IL_00BE;
						}
						taskAwaiter = mqbtailGatePageCustomization.Coding.UpdateCurrentState("", progress).GetAwaiter();
						if (!taskAwaiter.IsCompleted)
						{
							num2 = 0;
							TaskAwaiter<CodingRequestResult> taskAwaiter2 = taskAwaiter;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<CodingRequestResult>, MQBTailGatePageCustomization.<UpdateState>d__10>(ref taskAwaiter, ref this);
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
					IL_00BE:
					mqbtailGatePageCustomization.activityFrame.IsVisible = false;
					mqbtailGatePageCustomization.stackRoot.IsEnabled = true;
					mqbtailGatePageCustomization.activityFrame.Text = ActivityFrame.PLEASE_WAIT_TEXT;
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

			// Token: 0x06004F59 RID: 20313 RVA: 0x003C5090 File Offset: 0x003C3290
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x04002F93 RID: 12179
			public int <>1__state;

			// Token: 0x04002F94 RID: 12180
			public AsyncTaskMethodBuilder <>t__builder;

			// Token: 0x04002F95 RID: 12181
			public MQBTailGatePageCustomization <>4__this;

			// Token: 0x04002F96 RID: 12182
			private TaskAwaiter<CodingRequestResult> <>u__1;
		}

		// Token: 0x02000969 RID: 2409
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <btnApply_Clicked>d__8 : IAsyncStateMachine
		{
			// Token: 0x06004F5A RID: 20314 RVA: 0x003C50A0 File Offset: 0x003C32A0
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				MQBTailGatePageCustomization mqbtailGatePageCustomization = this;
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
						goto IL_015E;
					case 2:
					{
						TaskAwaiter taskAwaiter4;
						taskAwaiter3 = taskAwaiter4;
						taskAwaiter4 = default(TaskAwaiter);
						num2 = -1;
						goto IL_01CC;
					}
					case 3:
						taskAwaiter5 = taskAwaiter2;
						taskAwaiter2 = default(TaskAwaiter<bool>);
						num2 = -1;
						goto IL_0286;
					case 4:
					{
						TaskAwaiter taskAwaiter4;
						taskAwaiter3 = taskAwaiter4;
						taskAwaiter4 = default(TaskAwaiter);
						num2 = -1;
						goto IL_02F4;
					}
					case 5:
					{
						TaskAwaiter<CodingRequestResult> taskAwaiter7;
						taskAwaiter6 = taskAwaiter7;
						taskAwaiter7 = default(TaskAwaiter<CodingRequestResult>);
						num2 = -1;
						goto IL_039E;
					}
					case 6:
					{
						TaskAwaiter taskAwaiter4;
						taskAwaiter3 = taskAwaiter4;
						taskAwaiter4 = default(TaskAwaiter);
						num2 = -1;
						goto IL_0433;
					}
					case 7:
					{
						TaskAwaiter taskAwaiter4;
						taskAwaiter3 = taskAwaiter4;
						taskAwaiter4 = default(TaskAwaiter);
						num2 = -1;
						goto IL_04A8;
					}
					default:
						Coding = mqbtailGatePageCustomization.BindingContext as TailGateParametrizeCustomizationCoding;
						if (Coding.Model.IsLoaded)
						{
							goto IL_00CA;
						}
						taskAwaiter3 = mqbtailGatePageCustomization.DisplayAlert(Coding.CurrentState, "", "OK").GetAwaiter();
						if (!taskAwaiter3.IsCompleted)
						{
							num2 = 0;
							TaskAwaiter taskAwaiter4 = taskAwaiter3;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, MQBTailGatePageCustomization.<btnApply_Clicked>d__8>(ref taskAwaiter3, ref this);
							return;
						}
						break;
					}
					taskAwaiter3.GetResult();
					IL_00CA:
					if (Coding.RequiresPro && !SharedSettings.Current.AdsProductPurchased)
					{
						taskAwaiter5 = mqbtailGatePageCustomization.DisplayAlert("Car Scanner Pro", Translate.GetString("FeatureLocked"), "OK", Translate.GetString("btnCancel.Content")).GetAwaiter();
						if (!taskAwaiter5.IsCompleted)
						{
							num2 = 1;
							taskAwaiter2 = taskAwaiter5;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<bool>, MQBTailGatePageCustomization.<btnApply_Clicked>d__8>(ref taskAwaiter5, ref this);
							return;
						}
					}
					else if (SharedSettings.Current.CodingsCounter >= 3 && !SharedSettings.Current.AdsProductPurchased)
					{
						taskAwaiter5 = mqbtailGatePageCustomization.DisplayAlert("Car Scanner Pro", string.Format(Translate.GetString("coding_FreeCodingsFinished"), SharedSettings.Current.CodingsCounter, 3), "OK", Translate.GetString("btnCancel.Content")).GetAwaiter();
						if (!taskAwaiter5.IsCompleted)
						{
							num2 = 3;
							taskAwaiter2 = taskAwaiter5;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<bool>, MQBTailGatePageCustomization.<btnApply_Clicked>d__8>(ref taskAwaiter5, ref this);
							return;
						}
						goto IL_0286;
					}
					else
					{
						mqbtailGatePageCustomization.activityFrame.IsVisible = true;
						mqbtailGatePageCustomization.stackRoot.IsEnabled = false;
						Progress<string> progress = new Progress<string>(delegate(string s)
						{
							Device.BeginInvokeOnMainThread(new Action(new MQBTailGatePageCustomization.<>c__DisplayClass8_0
							{
								<>4__this = mqbtailGatePageCustomization,
								s = s
							}.<btnApply_Clicked>b__1));
						});
						taskAwaiter6 = Coding.Execute(mqbtailGatePageCustomization.entryPassword.Text, "", "", progress, null, false).GetAwaiter();
						if (!taskAwaiter6.IsCompleted)
						{
							num2 = 5;
							TaskAwaiter<CodingRequestResult> taskAwaiter7 = taskAwaiter6;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<CodingRequestResult>, MQBTailGatePageCustomization.<btnApply_Clicked>d__8>(ref taskAwaiter6, ref this);
							return;
						}
						goto IL_039E;
					}
					IL_015E:
					if (!taskAwaiter5.GetResult())
					{
						goto IL_01D3;
					}
					Page inAppPage = InAppManager.GetInAppPage();
					taskAwaiter3 = mqbtailGatePageCustomization.Navigation.PushAsync(inAppPage).GetAwaiter();
					if (!taskAwaiter3.IsCompleted)
					{
						num2 = 2;
						TaskAwaiter taskAwaiter4 = taskAwaiter3;
						this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, MQBTailGatePageCustomization.<btnApply_Clicked>d__8>(ref taskAwaiter3, ref this);
						return;
					}
					IL_01CC:
					taskAwaiter3.GetResult();
					IL_01D3:
					goto IL_04F9;
					IL_0286:
					if (!taskAwaiter5.GetResult())
					{
						goto IL_02FB;
					}
					Page inAppPage2 = InAppManager.GetInAppPage();
					taskAwaiter3 = mqbtailGatePageCustomization.Navigation.PushAsync(inAppPage2).GetAwaiter();
					if (!taskAwaiter3.IsCompleted)
					{
						num2 = 4;
						TaskAwaiter taskAwaiter4 = taskAwaiter3;
						this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, MQBTailGatePageCustomization.<btnApply_Clicked>d__8>(ref taskAwaiter3, ref this);
						return;
					}
					IL_02F4:
					taskAwaiter3.GetResult();
					IL_02FB:
					goto IL_04F9;
					IL_039E:
					CodingRequestResult result = taskAwaiter6.GetResult();
					if (result == CodingRequestResult.Success)
					{
						SharedSettings sharedSettings = SharedSettings.Current;
						int codingsCounter = sharedSettings.CodingsCounter;
						sharedSettings.CodingsCounter = codingsCounter + 1;
						taskAwaiter3 = mqbtailGatePageCustomization.DisplayAlert(Coding.Name, Translate.GetString("coding_OperationFinished"), "OK").GetAwaiter();
						if (!taskAwaiter3.IsCompleted)
						{
							num2 = 6;
							TaskAwaiter taskAwaiter4 = taskAwaiter3;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, MQBTailGatePageCustomization.<btnApply_Clicked>d__8>(ref taskAwaiter3, ref this);
							return;
						}
					}
					else
					{
						taskAwaiter3 = mqbtailGatePageCustomization.DisplayAlert(Translate.GetString("coding_Error"), MQBAdaptationTemplate.CodingRequestResultToString(result), "OK").GetAwaiter();
						if (!taskAwaiter3.IsCompleted)
						{
							num2 = 7;
							TaskAwaiter taskAwaiter4 = taskAwaiter3;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, MQBTailGatePageCustomization.<btnApply_Clicked>d__8>(ref taskAwaiter3, ref this);
							return;
						}
						goto IL_04A8;
					}
					IL_0433:
					taskAwaiter3.GetResult();
					goto IL_04AF;
					IL_04A8:
					taskAwaiter3.GetResult();
					IL_04AF:
					mqbtailGatePageCustomization.activityFrame.IsVisible = false;
					mqbtailGatePageCustomization.activityFrame.Text = ActivityFrame.PLEASE_WAIT_TEXT;
					mqbtailGatePageCustomization.stackRoot.IsEnabled = true;
				}
				catch (Exception ex)
				{
					num2 = -2;
					Coding = null;
					this.<>t__builder.SetException(ex);
					return;
				}
				IL_04F9:
				num2 = -2;
				Coding = null;
				this.<>t__builder.SetResult();
			}

			// Token: 0x06004F5B RID: 20315 RVA: 0x003C55DC File Offset: 0x003C37DC
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x04002F97 RID: 12183
			public int <>1__state;

			// Token: 0x04002F98 RID: 12184
			public AsyncVoidMethodBuilder <>t__builder;

			// Token: 0x04002F99 RID: 12185
			public MQBTailGatePageCustomization <>4__this;

			// Token: 0x04002F9A RID: 12186
			private TailGateParametrizeCustomizationCoding <Coding>5__2;

			// Token: 0x04002F9B RID: 12187
			private TaskAwaiter <>u__1;

			// Token: 0x04002F9C RID: 12188
			private TaskAwaiter<bool> <>u__2;

			// Token: 0x04002F9D RID: 12189
			private TaskAwaiter<CodingRequestResult> <>u__3;
		}

		// Token: 0x0200096A RID: 2410
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <btnChooseOtherVersion_Clicked>d__7 : IAsyncStateMachine
		{
			// Token: 0x06004F5C RID: 20316 RVA: 0x003C55EC File Offset: 0x003C37EC
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				MQBTailGatePageCustomization mqbtailGatePageCustomization = this;
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
						goto IL_0148;
					}
					case 2:
					{
						TaskAwaiter taskAwaiter2;
						taskAwaiter = taskAwaiter2;
						taskAwaiter2 = default(TaskAwaiter);
						num2 = -1;
						goto IL_0236;
					}
					default:
					{
						MQBTailGatePageCustomization.<>c__DisplayClass7_0 CS$<>8__locals1 = new MQBTailGatePageCustomization.<>c__DisplayClass7_0();
						CS$<>8__locals1.<>4__this = this;
						CS$<>8__locals1.coding = mqbtailGatePageCustomization.BindingContext as TailGateParametrizeCustomizationCoding;
						if (string.IsNullOrEmpty(CS$<>8__locals1.coding.PartNumber))
						{
							taskAwaiter = mqbtailGatePageCustomization.DisplayAlert("Error", "Partnumber not detected, coding not supported!", "OK").GetAwaiter();
							if (!taskAwaiter.IsCompleted)
							{
								num2 = 0;
								TaskAwaiter taskAwaiter2 = taskAwaiter;
								this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, MQBTailGatePageCustomization.<btnChooseOtherVersion_Clicked>d__7>(ref taskAwaiter, ref this);
								return;
							}
						}
						else if (CS$<>8__locals1.coding.LongCoding == null || CS$<>8__locals1.coding.LongCoding.Length == 0)
						{
							taskAwaiter = mqbtailGatePageCustomization.DisplayAlert("Error", "Long coding not detected, coding not supported!", "OK").GetAwaiter();
							if (!taskAwaiter.IsCompleted)
							{
								num2 = 1;
								TaskAwaiter taskAwaiter2 = taskAwaiter;
								this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, MQBTailGatePageCustomization.<btnChooseOtherVersion_Clicked>d__7>(ref taskAwaiter, ref this);
								return;
							}
							goto IL_0148;
						}
						else
						{
							IEnumerable<ValueItemWithTranslation> enumerable = from x in (from x in PackageFileReader.GetFilesInDirectory("vag.tailgate6d.")
									orderby x
									select x).ToArray<string>()
								where x.Contains(CS$<>8__locals1.<>4__this.Coding.PartNumber, StringComparison.OrdinalIgnoreCase) || x.Contains(CS$<>8__locals1.<>4__this.Coding.DSVersion, StringComparison.OrdinalIgnoreCase)
								select new ValueItemWithTranslation
								{
									Title = Path.GetFileNameWithoutExtension(x).Replace('_', ' ').Trim(' ')
										.ToUpper(),
									Value = x
								};
							Action<ValueItemWithTranslation> action = delegate(ValueItemWithTranslation selected)
							{
								try
								{
									MQB_6D_DatasetBuilderModel mqb_6D_DatasetBuilderModel = new MQB_6D_DatasetBuilderModel();
									using (Stream stream = PackageFileReader.OpenFileStream("vag.tailgate6d." + selected.Value))
									{
										byte[] array = new byte[stream.Length];
										stream.Read(array, 0, array.Length);
										mqb_6D_DatasetBuilderModel.LoadFromBytes(array, CS$<>8__locals1.coding.PartNumber, CS$<>8__locals1.coding.LongCoding);
									}
									CS$<>8__locals1.coding.Model = mqb_6D_DatasetBuilderModel;
									CS$<>8__locals1.coding.DisplayControls = true;
								}
								catch (Exception)
								{
									App.GetCurrentPage().DisplayAlert("Error!", "Error loading dataset " + selected.Title, "OK");
								}
							};
							ItemWithValueSelectorPage itemWithValueSelectorPage = new ItemWithValueSelectorPage("Dataset version", enumerable, null, action);
							taskAwaiter = mqbtailGatePageCustomization.Navigation.PushAsync(itemWithValueSelectorPage).GetAwaiter();
							if (!taskAwaiter.IsCompleted)
							{
								num2 = 2;
								TaskAwaiter taskAwaiter2 = taskAwaiter;
								this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, MQBTailGatePageCustomization.<btnChooseOtherVersion_Clicked>d__7>(ref taskAwaiter, ref this);
								return;
							}
							goto IL_0236;
						}
						break;
					}
					}
					taskAwaiter.GetResult();
					goto IL_0258;
					IL_0148:
					taskAwaiter.GetResult();
					goto IL_0258;
					IL_0236:
					taskAwaiter.GetResult();
				}
				catch (Exception ex)
				{
					num2 = -2;
					this.<>t__builder.SetException(ex);
					return;
				}
				IL_0258:
				num2 = -2;
				this.<>t__builder.SetResult();
			}

			// Token: 0x06004F5D RID: 20317 RVA: 0x003C5880 File Offset: 0x003C3A80
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x04002F9E RID: 12190
			public int <>1__state;

			// Token: 0x04002F9F RID: 12191
			public AsyncVoidMethodBuilder <>t__builder;

			// Token: 0x04002FA0 RID: 12192
			public MQBTailGatePageCustomization <>4__this;

			// Token: 0x04002FA1 RID: 12193
			private TaskAwaiter <>u__1;
		}
	}
}
