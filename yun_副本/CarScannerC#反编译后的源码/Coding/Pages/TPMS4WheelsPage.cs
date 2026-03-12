using System;
using System.CodeDom.Compiler;
using System.Diagnostics;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
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
	// Token: 0x02000971 RID: 2417
	[XamlCompilation(2)]
	[XamlFilePath("Coding\\Pages\\TPMS4WheelsPage.xaml")]
	public class TPMS4WheelsPage : ContentPage
	{
		// Token: 0x06004F70 RID: 20336 RVA: 0x003C846E File Offset: 0x003C666E
		public TPMS4WheelsPage(ICodingContainer coding)
		{
			this.InitializeComponent();
			this.coding = coding;
			base.BindingContext = this.coding;
			base.Appearing += this.CodingDetailsPage_Appearing;
		}

		// Token: 0x06004F71 RID: 20337 RVA: 0x003C84A8 File Offset: 0x003C66A8
		private async void CodingDetailsPage_Appearing(object sender, EventArgs e)
		{
			if (this.first_appearing)
			{
				this.first_appearing = false;
				this.activityFrame.IsVisible = true;
				await this.coding.UpdateCurrentState("", null);
				this.activityFrame.IsVisible = false;
			}
		}

		// Token: 0x06004F72 RID: 20338 RVA: 0x003C84E0 File Offset: 0x003C66E0
		public async Task UpdateState()
		{
			this.activityFrame.IsVisible = true;
			await this.coding.UpdateCurrentState("", null);
			this.activityFrame.IsVisible = false;
		}

		// Token: 0x06004F73 RID: 20339 RVA: 0x003C8524 File Offset: 0x003C6724
		private async void BtnUpdateState_Clicked(object sender, EventArgs e)
		{
			this.UpdateState();
		}

		// Token: 0x06004F74 RID: 20340 RVA: 0x003C855C File Offset: 0x003C675C
		private async void btnSaveState_Clicked(object sender, EventArgs e)
		{
			if (this.coding.RequiresPro && !SharedSettings.Current.AdsProductPurchased)
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
				Progress<string> progress = new Progress<string>(delegate(string s)
				{
					Device.BeginInvokeOnMainThread(delegate
					{
						this.activityFrame.Text = s;
					});
				});
				CodingRequestResult codingRequestResult = await this.coding.Execute("", "", "", progress, null, false);
				if (codingRequestResult == CodingRequestResult.Success)
				{
					SharedSettings.Current.CodingsCounter++;
				}
				else
				{
					await base.DisplayAlert(Translate.GetString("coding_Error"), MQBAdaptationTemplate.CodingRequestResultToString(codingRequestResult), "OK");
				}
				await this.coding.UpdateCurrentState("", null);
				this.activityFrame.IsVisible = false;
				this.activityFrame.Text = ActivityFrame.PLEASE_WAIT_TEXT;
			}
		}

		// Token: 0x06004F75 RID: 20341 RVA: 0x003C8594 File Offset: 0x003C6794
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private void InitializeComponent()
		{
			if (ResourceLoader.IsEnabled && ResourceLoader.CanProvideContentFor(new ResourceLoader.ResourceLoadingQuery
			{
				AssemblyName = typeof(TPMS4WheelsPage).GetTypeInfo().Assembly.GetName(),
				ResourcePath = "Coding/Pages/TPMS4WheelsPage.xaml",
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
			VisualDiagnostics.RegisterSourceInfo(translate = new Translate(), new Uri("Coding\\Pages\\TPMS4WheelsPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 13, 5);
			DynamicResourceExtension dynamicResourceExtension;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension = new DynamicResourceExtension(), new Uri("Coding\\Pages\\TPMS4WheelsPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 16, 5);
			EmptyStringToFalseConverter emptyStringToFalseConverter;
			VisualDiagnostics.RegisterSourceInfo(emptyStringToFalseConverter = new EmptyStringToFalseConverter(), new Uri("Coding\\Pages\\TPMS4WheelsPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 20, 14);
			EmptyStringToTrueConverter emptyStringToTrueConverter;
			VisualDiagnostics.RegisterSourceInfo(emptyStringToTrueConverter = new EmptyStringToTrueConverter(), new Uri("Coding\\Pages\\TPMS4WheelsPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 21, 14);
			VagCodingPlatformToTrueConverter vagCodingPlatformToTrueConverter;
			VisualDiagnostics.RegisterSourceInfo(vagCodingPlatformToTrueConverter = new VagCodingPlatformToTrueConverter(), new Uri("Coding\\Pages\\TPMS4WheelsPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 22, 14);
			ResourceDictionary resourceDictionary;
			VisualDiagnostics.RegisterSourceInfo(resourceDictionary = new ResourceDictionary(), new Uri("Coding\\Pages\\TPMS4WheelsPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 19, 10);
			RowDefinition rowDefinition;
			VisualDiagnostics.RegisterSourceInfo(rowDefinition = new RowDefinition(), new Uri("Coding\\Pages\\TPMS4WheelsPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 29, 22);
			DynamicResourceExtension dynamicResourceExtension2;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension2 = new DynamicResourceExtension(), new Uri("Coding\\Pages\\TPMS4WheelsPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 34, 25);
			BindingExtension bindingExtension;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension = new BindingExtension(), new Uri("Coding\\Pages\\TPMS4WheelsPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 35, 25);
			Label label;
			VisualDiagnostics.RegisterSourceInfo(label = new Label(), new Uri("Coding\\Pages\\TPMS4WheelsPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 32, 22);
			StaticResourceExtension staticResourceExtension;
			VisualDiagnostics.RegisterSourceInfo(staticResourceExtension = new StaticResourceExtension(), new Uri("Coding\\Pages\\TPMS4WheelsPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 36, 28);
			BindingExtension bindingExtension2;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension2 = new BindingExtension(), new Uri("Coding\\Pages\\TPMS4WheelsPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 36, 28);
			BindingExtension bindingExtension3;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension3 = new BindingExtension(), new Uri("Coding\\Pages\\TPMS4WheelsPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 36, 118);
			Label label2;
			VisualDiagnostics.RegisterSourceInfo(label2 = new Label(), new Uri("Coding\\Pages\\TPMS4WheelsPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 36, 22);
			StaticResourceExtension staticResourceExtension2;
			VisualDiagnostics.RegisterSourceInfo(staticResourceExtension2 = new StaticResourceExtension(), new Uri("Coding\\Pages\\TPMS4WheelsPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 37, 28);
			BindingExtension bindingExtension4;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension4 = new BindingExtension(), new Uri("Coding\\Pages\\TPMS4WheelsPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 37, 28);
			BindingExtension bindingExtension5;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension5 = new BindingExtension(), new Uri("Coding\\Pages\\TPMS4WheelsPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 37, 136);
			Label label3;
			VisualDiagnostics.RegisterSourceInfo(label3 = new Label(), new Uri("Coding\\Pages\\TPMS4WheelsPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 37, 22);
			Translate translate2;
			VisualDiagnostics.RegisterSourceInfo(translate2 = new Translate(), new Uri("Coding\\Pages\\TPMS4WheelsPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 43, 25);
			Button button;
			VisualDiagnostics.RegisterSourceInfo(button = new Button(), new Uri("Coding\\Pages\\TPMS4WheelsPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 40, 22);
			Translate translate3;
			VisualDiagnostics.RegisterSourceInfo(translate3 = new Translate(), new Uri("Coding\\Pages\\TPMS4WheelsPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 44, 28);
			Label label4;
			VisualDiagnostics.RegisterSourceInfo(label4 = new Label(), new Uri("Coding\\Pages\\TPMS4WheelsPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 44, 22);
			BindingExtension bindingExtension6;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension6 = new BindingExtension(), new Uri("Coding\\Pages\\TPMS4WheelsPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 45, 28);
			BindingExtension bindingExtension7;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension7 = new BindingExtension(), new Uri("Coding\\Pages\\TPMS4WheelsPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 45, 61);
			Label label5;
			VisualDiagnostics.RegisterSourceInfo(label5 = new Label(), new Uri("Coding\\Pages\\TPMS4WheelsPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 45, 22);
			BindingExtension bindingExtension8;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension8 = new BindingExtension(), new Uri("Coding\\Pages\\TPMS4WheelsPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 48, 25);
			BindingExtension bindingExtension9;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension9 = new BindingExtension(), new Uri("Coding\\Pages\\TPMS4WheelsPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 49, 25);
			Entry entry;
			VisualDiagnostics.RegisterSourceInfo(entry = new Entry(), new Uri("Coding\\Pages\\TPMS4WheelsPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 46, 22);
			BindingExtension bindingExtension10;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension10 = new BindingExtension(), new Uri("Coding\\Pages\\TPMS4WheelsPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 50, 28);
			BindingExtension bindingExtension11;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension11 = new BindingExtension(), new Uri("Coding\\Pages\\TPMS4WheelsPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 50, 61);
			Label label6;
			VisualDiagnostics.RegisterSourceInfo(label6 = new Label(), new Uri("Coding\\Pages\\TPMS4WheelsPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 50, 22);
			BindingExtension bindingExtension12;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension12 = new BindingExtension(), new Uri("Coding\\Pages\\TPMS4WheelsPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 53, 25);
			BindingExtension bindingExtension13;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension13 = new BindingExtension(), new Uri("Coding\\Pages\\TPMS4WheelsPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 54, 25);
			Entry entry2;
			VisualDiagnostics.RegisterSourceInfo(entry2 = new Entry(), new Uri("Coding\\Pages\\TPMS4WheelsPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 51, 22);
			BindingExtension bindingExtension14;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension14 = new BindingExtension(), new Uri("Coding\\Pages\\TPMS4WheelsPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 55, 28);
			BindingExtension bindingExtension15;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension15 = new BindingExtension(), new Uri("Coding\\Pages\\TPMS4WheelsPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 55, 61);
			Label label7;
			VisualDiagnostics.RegisterSourceInfo(label7 = new Label(), new Uri("Coding\\Pages\\TPMS4WheelsPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 55, 22);
			BindingExtension bindingExtension16;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension16 = new BindingExtension(), new Uri("Coding\\Pages\\TPMS4WheelsPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 58, 25);
			BindingExtension bindingExtension17;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension17 = new BindingExtension(), new Uri("Coding\\Pages\\TPMS4WheelsPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 59, 25);
			Entry entry3;
			VisualDiagnostics.RegisterSourceInfo(entry3 = new Entry(), new Uri("Coding\\Pages\\TPMS4WheelsPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 56, 22);
			BindingExtension bindingExtension18;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension18 = new BindingExtension(), new Uri("Coding\\Pages\\TPMS4WheelsPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 60, 28);
			BindingExtension bindingExtension19;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension19 = new BindingExtension(), new Uri("Coding\\Pages\\TPMS4WheelsPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 60, 61);
			Label label8;
			VisualDiagnostics.RegisterSourceInfo(label8 = new Label(), new Uri("Coding\\Pages\\TPMS4WheelsPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 60, 22);
			BindingExtension bindingExtension20;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension20 = new BindingExtension(), new Uri("Coding\\Pages\\TPMS4WheelsPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 63, 25);
			BindingExtension bindingExtension21;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension21 = new BindingExtension(), new Uri("Coding\\Pages\\TPMS4WheelsPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 64, 25);
			Entry entry4;
			VisualDiagnostics.RegisterSourceInfo(entry4 = new Entry(), new Uri("Coding\\Pages\\TPMS4WheelsPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 61, 22);
			BindingExtension bindingExtension22;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension22 = new BindingExtension(), new Uri("Coding\\Pages\\TPMS4WheelsPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 65, 28);
			BindingExtension bindingExtension23;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension23 = new BindingExtension(), new Uri("Coding\\Pages\\TPMS4WheelsPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 65, 61);
			Label label9;
			VisualDiagnostics.RegisterSourceInfo(label9 = new Label(), new Uri("Coding\\Pages\\TPMS4WheelsPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 65, 22);
			BindingExtension bindingExtension24;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension24 = new BindingExtension(), new Uri("Coding\\Pages\\TPMS4WheelsPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 68, 25);
			BindingExtension bindingExtension25;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension25 = new BindingExtension(), new Uri("Coding\\Pages\\TPMS4WheelsPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 69, 25);
			Entry entry5;
			VisualDiagnostics.RegisterSourceInfo(entry5 = new Entry(), new Uri("Coding\\Pages\\TPMS4WheelsPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 66, 22);
			Translate translate4;
			VisualDiagnostics.RegisterSourceInfo(translate4 = new Translate(), new Uri("Coding\\Pages\\TPMS4WheelsPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 73, 25);
			Button button2;
			VisualDiagnostics.RegisterSourceInfo(button2 = new Button(), new Uri("Coding\\Pages\\TPMS4WheelsPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 70, 22);
			DynamicResourceExtension dynamicResourceExtension3;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension3 = new DynamicResourceExtension(), new Uri("Coding\\Pages\\TPMS4WheelsPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 81, 25);
			Translate translate5;
			VisualDiagnostics.RegisterSourceInfo(translate5 = new Translate(), new Uri("Coding\\Pages\\TPMS4WheelsPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 82, 25);
			Label label10;
			VisualDiagnostics.RegisterSourceInfo(label10 = new Label(), new Uri("Coding\\Pages\\TPMS4WheelsPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 79, 22);
			StackLayout stackLayout;
			VisualDiagnostics.RegisterSourceInfo(stackLayout = new StackLayout(), new Uri("Coding\\Pages\\TPMS4WheelsPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 31, 18);
			ActivityFrame activityFrame;
			VisualDiagnostics.RegisterSourceInfo(activityFrame = new ActivityFrame(), new Uri("Coding\\Pages\\TPMS4WheelsPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 96, 18);
			Grid grid;
			VisualDiagnostics.RegisterSourceInfo(grid = new Grid(), new Uri("Coding\\Pages\\TPMS4WheelsPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 27, 14);
			ScrollView scrollView;
			VisualDiagnostics.RegisterSourceInfo(scrollView = new ScrollView(), new Uri("Coding\\Pages\\TPMS4WheelsPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 26, 10);
			VisualDiagnostics.RegisterSourceInfo(this, new Uri("Coding\\Pages\\TPMS4WheelsPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 2, 2);
			NameScope nameScope = NameScope.GetNameScope(this) ?? new NameScope();
			NameScope.SetNameScope(this, nameScope);
			nameScope.RegisterName("btnUpdateState", button);
			if (button.StyleId == null)
			{
				button.StyleId = "btnUpdateState";
			}
			nameScope.RegisterName("entryId1", entry);
			if (entry.StyleId == null)
			{
				entry.StyleId = "entryId1";
			}
			nameScope.RegisterName("entryId2", entry2);
			if (entry2.StyleId == null)
			{
				entry2.StyleId = "entryId2";
			}
			nameScope.RegisterName("entryId3", entry3);
			if (entry3.StyleId == null)
			{
				entry3.StyleId = "entryId3";
			}
			nameScope.RegisterName("entryId4", entry4);
			if (entry4.StyleId == null)
			{
				entry4.StyleId = "entryId4";
			}
			nameScope.RegisterName("entryId5", entry5);
			if (entry5.StyleId == null)
			{
				entry5.StyleId = "entryId5";
			}
			nameScope.RegisterName("btnSaveNewValue", button2);
			if (button2.StyleId == null)
			{
				button2.StyleId = "btnSaveNewValue";
			}
			nameScope.RegisterName("activityFrame", activityFrame);
			if (activityFrame.StyleId == null)
			{
				activityFrame.StyleId = "activityFrame";
			}
			this.btnUpdateState = button;
			this.entryId1 = entry;
			this.entryId2 = entry2;
			this.entryId3 = entry3;
			this.entryId4 = entry4;
			this.entryId5 = entry5;
			this.btnSaveNewValue = button2;
			this.activityFrame = activityFrame;
			this.Resources = resourceDictionary;
			resourceDictionary.Add("EmptyStringToFalseConverter", emptyStringToFalseConverter);
			resourceDictionary.Add("EmptyStringToTrueConverter", emptyStringToTrueConverter);
			resourceDictionary.Add("VagCodingPlatformToTrueConverter", vagCodingPlatformToTrueConverter);
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
			xmlNamespaceResolver.Add("d", "http://xamarin.com/schemas/2014/forms/design");
			xmlNamespaceResolver.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver.Add("mc", "http://schemas.openxmlformats.org/markup-compatibility/2006");
			xmlNamespaceResolver.Add("settings", "clr-namespace:CarScannerXamarinForms.Settings");
			xamlServiceProvider.Add(typeFromHandle2, new XamlTypeResolver(xmlNamespaceResolver, typeof(TPMS4WheelsPage).GetTypeInfo().Assembly));
			xamlServiceProvider.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(13, 5)));
			object obj2 = markupExtension.ProvideValue(xamlServiceProvider);
			this.Title = obj2;
			this.SetValue(Page.PaddingProperty, new Thickness(5.0, 0.0));
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
			xmlNamespaceResolver2.Add("d", "http://xamarin.com/schemas/2014/forms/design");
			xmlNamespaceResolver2.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver2.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver2.Add("mc", "http://schemas.openxmlformats.org/markup-compatibility/2006");
			xmlNamespaceResolver2.Add("settings", "clr-namespace:CarScannerXamarinForms.Settings");
			xamlServiceProvider2.Add(typeFromHandle4, new XamlTypeResolver(xmlNamespaceResolver2, typeof(TPMS4WheelsPage).GetTypeInfo().Assembly));
			xamlServiceProvider2.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(16, 5)));
			DynamicResource dynamicResource = markupExtension2.ProvideValue(xamlServiceProvider2);
			this.SetDynamicResource(VisualElement.BackgroundColorProperty, dynamicResource.Key);
			this.Resources = resourceDictionary;
			scrollView.SetValue(ScrollView.OrientationProperty, 0);
			rowDefinition.SetValue(RowDefinition.HeightProperty, new GridLengthTypeConverter().ConvertFromInvariantString("*"));
			grid.GetValue(Grid.RowDefinitionsProperty).Add(rowDefinition);
			stackLayout.SetValue(Grid.RowProperty, 0);
			stackLayout.SetValue(StackLayout.OrientationProperty, 0);
			label.SetValue(Label.FontAttributesProperty, new FontAttributesConverter().ConvertFromInvariantString("Bold"));
			dynamicResourceExtension2.Key = "BaseFontSize+++";
			IMarkupExtension<DynamicResource> markupExtension3 = dynamicResourceExtension2;
			XamlServiceProvider xamlServiceProvider3 = new XamlServiceProvider();
			Type typeFromHandle5 = typeof(IProvideValueTarget);
			object[] array3 = new object[0 + 5];
			array3[0] = label;
			array3[1] = stackLayout;
			array3[2] = grid;
			array3[3] = scrollView;
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
			xmlNamespaceResolver3.Add("d", "http://xamarin.com/schemas/2014/forms/design");
			xmlNamespaceResolver3.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver3.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver3.Add("mc", "http://schemas.openxmlformats.org/markup-compatibility/2006");
			xmlNamespaceResolver3.Add("settings", "clr-namespace:CarScannerXamarinForms.Settings");
			xamlServiceProvider3.Add(typeFromHandle6, new XamlTypeResolver(xmlNamespaceResolver3, typeof(TPMS4WheelsPage).GetTypeInfo().Assembly));
			xamlServiceProvider3.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(34, 25)));
			DynamicResource dynamicResource2 = markupExtension3.ProvideValue(xamlServiceProvider3);
			label.SetDynamicResource(Label.FontSizeProperty, dynamicResource2.Key);
			bindingExtension.Path = "Name";
			bindingExtension.TypedBinding = new TypedBinding<TPMSCodingBase, string>(delegate(TPMSCodingBase A_0)
			{
				if (A_0 != null)
				{
					return new ValueTuple<string, bool>(A_0.Name, true);
				}
				return default(ValueTuple<string, bool>);
			}, delegate(TPMSCodingBase A_0, string A_1)
			{
				if (A_0 != null)
				{
					A_0.Name = A_1;
					return;
				}
			}, new Tuple<Func<TPMSCodingBase, object>, string>[]
			{
				new Tuple<Func<TPMSCodingBase, object>, string>((TPMSCodingBase A_0) => A_0, "Name")
			});
			BindingBase bindingBase = bindingExtension.ProvideValue(null);
			label.SetBinding(Label.TextProperty, bindingBase);
			stackLayout.Children.Add(label);
			staticResourceExtension.Key = "EmptyStringToFalseConverter";
			IMarkupExtension markupExtension4 = staticResourceExtension;
			XamlServiceProvider xamlServiceProvider4 = new XamlServiceProvider();
			Type typeFromHandle7 = typeof(IProvideValueTarget);
			object[] array4 = new object[0 + 6];
			array4[0] = bindingExtension2;
			array4[1] = label2;
			array4[2] = stackLayout;
			array4[3] = grid;
			array4[4] = scrollView;
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
			xmlNamespaceResolver4.Add("d", "http://xamarin.com/schemas/2014/forms/design");
			xmlNamespaceResolver4.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver4.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver4.Add("mc", "http://schemas.openxmlformats.org/markup-compatibility/2006");
			xmlNamespaceResolver4.Add("settings", "clr-namespace:CarScannerXamarinForms.Settings");
			xamlServiceProvider4.Add(typeFromHandle8, new XamlTypeResolver(xmlNamespaceResolver4, typeof(TPMS4WheelsPage).GetTypeInfo().Assembly));
			xamlServiceProvider4.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(36, 28)));
			object obj6 = markupExtension4.ProvideValue(xamlServiceProvider4);
			bindingExtension2.Converter = obj6;
			bindingExtension2.Path = "Description";
			bindingExtension2.TypedBinding = new TypedBinding<TPMSCodingBase, string>(delegate(TPMSCodingBase A_0)
			{
				if (A_0 != null)
				{
					return new ValueTuple<string, bool>(A_0.Description, true);
				}
				return default(ValueTuple<string, bool>);
			}, delegate(TPMSCodingBase A_0, string A_1)
			{
				if (A_0 != null)
				{
					A_0.Description = A_1;
					return;
				}
			}, new Tuple<Func<TPMSCodingBase, object>, string>[]
			{
				new Tuple<Func<TPMSCodingBase, object>, string>((TPMSCodingBase A_0) => A_0, "Description")
			});
			BindingBase bindingBase2 = bindingExtension2.ProvideValue(null);
			label2.SetBinding(VisualElement.IsVisibleProperty, bindingBase2);
			bindingExtension3.Path = "Description";
			bindingExtension3.TypedBinding = new TypedBinding<TPMSCodingBase, string>(delegate(TPMSCodingBase A_0)
			{
				if (A_0 != null)
				{
					return new ValueTuple<string, bool>(A_0.Description, true);
				}
				return default(ValueTuple<string, bool>);
			}, delegate(TPMSCodingBase A_0, string A_1)
			{
				if (A_0 != null)
				{
					A_0.Description = A_1;
					return;
				}
			}, new Tuple<Func<TPMSCodingBase, object>, string>[]
			{
				new Tuple<Func<TPMSCodingBase, object>, string>((TPMSCodingBase A_0) => A_0, "Description")
			});
			BindingBase bindingBase3 = bindingExtension3.ProvideValue(null);
			label2.SetBinding(Label.TextProperty, bindingBase3);
			stackLayout.Children.Add(label2);
			bindingExtension4.Mode = 2;
			staticResourceExtension2.Key = "EmptyStringToFalseConverter";
			IMarkupExtension markupExtension5 = staticResourceExtension2;
			XamlServiceProvider xamlServiceProvider5 = new XamlServiceProvider();
			Type typeFromHandle9 = typeof(IProvideValueTarget);
			object[] array5 = new object[0 + 6];
			array5[0] = bindingExtension4;
			array5[1] = label3;
			array5[2] = stackLayout;
			array5[3] = grid;
			array5[4] = scrollView;
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
			xmlNamespaceResolver5.Add("d", "http://xamarin.com/schemas/2014/forms/design");
			xmlNamespaceResolver5.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver5.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver5.Add("mc", "http://schemas.openxmlformats.org/markup-compatibility/2006");
			xmlNamespaceResolver5.Add("settings", "clr-namespace:CarScannerXamarinForms.Settings");
			xamlServiceProvider5.Add(typeFromHandle10, new XamlTypeResolver(xmlNamespaceResolver5, typeof(TPMS4WheelsPage).GetTypeInfo().Assembly));
			xamlServiceProvider5.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(37, 28)));
			object obj8 = markupExtension5.ProvideValue(xamlServiceProvider5);
			bindingExtension4.Converter = obj8;
			bindingExtension4.Path = "InnerDescription";
			bindingExtension4.TypedBinding = new TypedBinding<TPMSCodingBase, string>(delegate(TPMSCodingBase A_0)
			{
				if (A_0 != null)
				{
					return new ValueTuple<string, bool>(A_0.InnerDescription, true);
				}
				return default(ValueTuple<string, bool>);
			}, null, new Tuple<Func<TPMSCodingBase, object>, string>[]
			{
				new Tuple<Func<TPMSCodingBase, object>, string>((TPMSCodingBase A_0) => A_0, "InnerDescription")
			});
			BindingBase bindingBase4 = bindingExtension4.ProvideValue(null);
			label3.SetBinding(VisualElement.IsVisibleProperty, bindingBase4);
			bindingExtension5.Path = "InnerDescription";
			bindingExtension5.TypedBinding = new TypedBinding<TPMSCodingBase, string>(delegate(TPMSCodingBase A_0)
			{
				if (A_0 != null)
				{
					return new ValueTuple<string, bool>(A_0.InnerDescription, true);
				}
				return default(ValueTuple<string, bool>);
			}, delegate(TPMSCodingBase A_0, string A_1)
			{
				if (A_0 != null)
				{
					A_0.InnerDescription = A_1;
					return;
				}
			}, new Tuple<Func<TPMSCodingBase, object>, string>[]
			{
				new Tuple<Func<TPMSCodingBase, object>, string>((TPMSCodingBase A_0) => A_0, "InnerDescription")
			});
			BindingBase bindingBase5 = bindingExtension5.ProvideValue(null);
			label3.SetBinding(Label.TextProperty, bindingBase5);
			stackLayout.Children.Add(label3);
			button.Clicked += this.BtnUpdateState_Clicked;
			translate2.Text = "Mode06Page_btnRefresh.Content";
			IMarkupExtension markupExtension6 = translate2;
			XamlServiceProvider xamlServiceProvider6 = new XamlServiceProvider();
			Type typeFromHandle11 = typeof(IProvideValueTarget);
			object[] array6 = new object[0 + 5];
			array6[0] = button;
			array6[1] = stackLayout;
			array6[2] = grid;
			array6[3] = scrollView;
			array6[4] = this;
			object obj9;
			xamlServiceProvider6.Add(typeFromHandle11, obj9 = new SimpleValueTargetProvider(array6, Button.TextProperty, nameScope));
			xamlServiceProvider6.Add(typeof(IReferenceProvider), obj9);
			Type typeFromHandle12 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver6 = new XmlNamespaceResolver();
			xmlNamespaceResolver6.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver6.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver6.Add("coding", "clr-namespace:CarScannerXamarinForms.Coding");
			xmlNamespaceResolver6.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver6.Add("d", "http://xamarin.com/schemas/2014/forms/design");
			xmlNamespaceResolver6.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver6.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver6.Add("mc", "http://schemas.openxmlformats.org/markup-compatibility/2006");
			xmlNamespaceResolver6.Add("settings", "clr-namespace:CarScannerXamarinForms.Settings");
			xamlServiceProvider6.Add(typeFromHandle12, new XamlTypeResolver(xmlNamespaceResolver6, typeof(TPMS4WheelsPage).GetTypeInfo().Assembly));
			xamlServiceProvider6.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(43, 25)));
			object obj10 = markupExtension6.ProvideValue(xamlServiceProvider6);
			button.Text = obj10;
			stackLayout.Children.Add(button);
			translate3.Text = "coding_TypeNewValue";
			IMarkupExtension markupExtension7 = translate3;
			XamlServiceProvider xamlServiceProvider7 = new XamlServiceProvider();
			Type typeFromHandle13 = typeof(IProvideValueTarget);
			object[] array7 = new object[0 + 5];
			array7[0] = label4;
			array7[1] = stackLayout;
			array7[2] = grid;
			array7[3] = scrollView;
			array7[4] = this;
			object obj11;
			xamlServiceProvider7.Add(typeFromHandle13, obj11 = new SimpleValueTargetProvider(array7, Label.TextProperty, nameScope));
			xamlServiceProvider7.Add(typeof(IReferenceProvider), obj11);
			Type typeFromHandle14 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver7 = new XmlNamespaceResolver();
			xmlNamespaceResolver7.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver7.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver7.Add("coding", "clr-namespace:CarScannerXamarinForms.Coding");
			xmlNamespaceResolver7.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver7.Add("d", "http://xamarin.com/schemas/2014/forms/design");
			xmlNamespaceResolver7.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver7.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver7.Add("mc", "http://schemas.openxmlformats.org/markup-compatibility/2006");
			xmlNamespaceResolver7.Add("settings", "clr-namespace:CarScannerXamarinForms.Settings");
			xamlServiceProvider7.Add(typeFromHandle14, new XamlTypeResolver(xmlNamespaceResolver7, typeof(TPMS4WheelsPage).GetTypeInfo().Assembly));
			xamlServiceProvider7.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(44, 28)));
			object obj12 = markupExtension7.ProvideValue(xamlServiceProvider7);
			label4.Text = obj12;
			stackLayout.Children.Add(label4);
			bindingExtension6.Path = "ID1Visible";
			bindingExtension6.TypedBinding = new TypedBinding<TPMSCodingBase, bool>(delegate(TPMSCodingBase A_0)
			{
				if (A_0 != null)
				{
					return new ValueTuple<bool, bool>(A_0.ID1Visible, true);
				}
				return default(ValueTuple<bool, bool>);
			}, delegate(TPMSCodingBase A_0, bool A_1)
			{
				if (A_0 != null)
				{
					A_0.ID1Visible = A_1;
					return;
				}
			}, new Tuple<Func<TPMSCodingBase, object>, string>[]
			{
				new Tuple<Func<TPMSCodingBase, object>, string>((TPMSCodingBase A_0) => A_0, "ID1Visible")
			});
			BindingBase bindingBase6 = bindingExtension6.ProvideValue(null);
			label5.SetBinding(VisualElement.IsVisibleProperty, bindingBase6);
			bindingExtension7.Path = "ID1Title";
			bindingExtension7.TypedBinding = new TypedBinding<TPMSCodingBase, string>(delegate(TPMSCodingBase A_0)
			{
				if (A_0 != null)
				{
					return new ValueTuple<string, bool>(A_0.ID1Title, true);
				}
				return default(ValueTuple<string, bool>);
			}, delegate(TPMSCodingBase A_0, string A_1)
			{
				if (A_0 != null)
				{
					A_0.ID1Title = A_1;
					return;
				}
			}, new Tuple<Func<TPMSCodingBase, object>, string>[]
			{
				new Tuple<Func<TPMSCodingBase, object>, string>((TPMSCodingBase A_0) => A_0, "ID1Title")
			});
			BindingBase bindingBase7 = bindingExtension7.ProvideValue(null);
			label5.SetBinding(Label.TextProperty, bindingBase7);
			stackLayout.Children.Add(label5);
			bindingExtension8.Path = "ID1Visible";
			bindingExtension8.TypedBinding = new TypedBinding<TPMSCodingBase, bool>(delegate(TPMSCodingBase A_0)
			{
				if (A_0 != null)
				{
					return new ValueTuple<bool, bool>(A_0.ID1Visible, true);
				}
				return default(ValueTuple<bool, bool>);
			}, delegate(TPMSCodingBase A_0, bool A_1)
			{
				if (A_0 != null)
				{
					A_0.ID1Visible = A_1;
					return;
				}
			}, new Tuple<Func<TPMSCodingBase, object>, string>[]
			{
				new Tuple<Func<TPMSCodingBase, object>, string>((TPMSCodingBase A_0) => A_0, "ID1Visible")
			});
			BindingBase bindingBase8 = bindingExtension8.ProvideValue(null);
			entry.SetBinding(VisualElement.IsVisibleProperty, bindingBase8);
			bindingExtension9.Mode = 1;
			bindingExtension9.Path = "ID1";
			bindingExtension9.TypedBinding = new TypedBinding<TPMSCodingBase, string>(delegate(TPMSCodingBase A_0)
			{
				if (A_0 != null)
				{
					return new ValueTuple<string, bool>(A_0.ID1, true);
				}
				return default(ValueTuple<string, bool>);
			}, delegate(TPMSCodingBase A_0, string A_1)
			{
				if (A_0 != null)
				{
					A_0.ID1 = A_1;
					return;
				}
			}, new Tuple<Func<TPMSCodingBase, object>, string>[]
			{
				new Tuple<Func<TPMSCodingBase, object>, string>((TPMSCodingBase A_0) => A_0, "ID1")
			});
			BindingBase bindingBase9 = bindingExtension9.ProvideValue(null);
			entry.SetBinding(Entry.TextProperty, bindingBase9);
			stackLayout.Children.Add(entry);
			bindingExtension10.Path = "ID2Visible";
			bindingExtension10.TypedBinding = new TypedBinding<TPMSCodingBase, bool>(delegate(TPMSCodingBase A_0)
			{
				if (A_0 != null)
				{
					return new ValueTuple<bool, bool>(A_0.ID2Visible, true);
				}
				return default(ValueTuple<bool, bool>);
			}, delegate(TPMSCodingBase A_0, bool A_1)
			{
				if (A_0 != null)
				{
					A_0.ID2Visible = A_1;
					return;
				}
			}, new Tuple<Func<TPMSCodingBase, object>, string>[]
			{
				new Tuple<Func<TPMSCodingBase, object>, string>((TPMSCodingBase A_0) => A_0, "ID2Visible")
			});
			BindingBase bindingBase10 = bindingExtension10.ProvideValue(null);
			label6.SetBinding(VisualElement.IsVisibleProperty, bindingBase10);
			bindingExtension11.Path = "ID2Title";
			bindingExtension11.TypedBinding = new TypedBinding<TPMSCodingBase, string>(delegate(TPMSCodingBase A_0)
			{
				if (A_0 != null)
				{
					return new ValueTuple<string, bool>(A_0.ID2Title, true);
				}
				return default(ValueTuple<string, bool>);
			}, delegate(TPMSCodingBase A_0, string A_1)
			{
				if (A_0 != null)
				{
					A_0.ID2Title = A_1;
					return;
				}
			}, new Tuple<Func<TPMSCodingBase, object>, string>[]
			{
				new Tuple<Func<TPMSCodingBase, object>, string>((TPMSCodingBase A_0) => A_0, "ID2Title")
			});
			BindingBase bindingBase11 = bindingExtension11.ProvideValue(null);
			label6.SetBinding(Label.TextProperty, bindingBase11);
			stackLayout.Children.Add(label6);
			bindingExtension12.Path = "ID2Visible";
			bindingExtension12.TypedBinding = new TypedBinding<TPMSCodingBase, bool>(delegate(TPMSCodingBase A_0)
			{
				if (A_0 != null)
				{
					return new ValueTuple<bool, bool>(A_0.ID2Visible, true);
				}
				return default(ValueTuple<bool, bool>);
			}, delegate(TPMSCodingBase A_0, bool A_1)
			{
				if (A_0 != null)
				{
					A_0.ID2Visible = A_1;
					return;
				}
			}, new Tuple<Func<TPMSCodingBase, object>, string>[]
			{
				new Tuple<Func<TPMSCodingBase, object>, string>((TPMSCodingBase A_0) => A_0, "ID2Visible")
			});
			BindingBase bindingBase12 = bindingExtension12.ProvideValue(null);
			entry2.SetBinding(VisualElement.IsVisibleProperty, bindingBase12);
			bindingExtension13.Mode = 1;
			bindingExtension13.Path = "ID2";
			bindingExtension13.TypedBinding = new TypedBinding<TPMSCodingBase, string>(delegate(TPMSCodingBase A_0)
			{
				if (A_0 != null)
				{
					return new ValueTuple<string, bool>(A_0.ID2, true);
				}
				return default(ValueTuple<string, bool>);
			}, delegate(TPMSCodingBase A_0, string A_1)
			{
				if (A_0 != null)
				{
					A_0.ID2 = A_1;
					return;
				}
			}, new Tuple<Func<TPMSCodingBase, object>, string>[]
			{
				new Tuple<Func<TPMSCodingBase, object>, string>((TPMSCodingBase A_0) => A_0, "ID2")
			});
			BindingBase bindingBase13 = bindingExtension13.ProvideValue(null);
			entry2.SetBinding(Entry.TextProperty, bindingBase13);
			stackLayout.Children.Add(entry2);
			bindingExtension14.Path = "ID3Visible";
			bindingExtension14.TypedBinding = new TypedBinding<TPMSCodingBase, bool>(delegate(TPMSCodingBase A_0)
			{
				if (A_0 != null)
				{
					return new ValueTuple<bool, bool>(A_0.ID3Visible, true);
				}
				return default(ValueTuple<bool, bool>);
			}, delegate(TPMSCodingBase A_0, bool A_1)
			{
				if (A_0 != null)
				{
					A_0.ID3Visible = A_1;
					return;
				}
			}, new Tuple<Func<TPMSCodingBase, object>, string>[]
			{
				new Tuple<Func<TPMSCodingBase, object>, string>((TPMSCodingBase A_0) => A_0, "ID3Visible")
			});
			BindingBase bindingBase14 = bindingExtension14.ProvideValue(null);
			label7.SetBinding(VisualElement.IsVisibleProperty, bindingBase14);
			bindingExtension15.Path = "ID3Title";
			bindingExtension15.TypedBinding = new TypedBinding<TPMSCodingBase, string>(delegate(TPMSCodingBase A_0)
			{
				if (A_0 != null)
				{
					return new ValueTuple<string, bool>(A_0.ID3Title, true);
				}
				return default(ValueTuple<string, bool>);
			}, delegate(TPMSCodingBase A_0, string A_1)
			{
				if (A_0 != null)
				{
					A_0.ID3Title = A_1;
					return;
				}
			}, new Tuple<Func<TPMSCodingBase, object>, string>[]
			{
				new Tuple<Func<TPMSCodingBase, object>, string>((TPMSCodingBase A_0) => A_0, "ID3Title")
			});
			BindingBase bindingBase15 = bindingExtension15.ProvideValue(null);
			label7.SetBinding(Label.TextProperty, bindingBase15);
			stackLayout.Children.Add(label7);
			bindingExtension16.Path = "ID3Visible";
			bindingExtension16.TypedBinding = new TypedBinding<TPMSCodingBase, bool>(delegate(TPMSCodingBase A_0)
			{
				if (A_0 != null)
				{
					return new ValueTuple<bool, bool>(A_0.ID3Visible, true);
				}
				return default(ValueTuple<bool, bool>);
			}, delegate(TPMSCodingBase A_0, bool A_1)
			{
				if (A_0 != null)
				{
					A_0.ID3Visible = A_1;
					return;
				}
			}, new Tuple<Func<TPMSCodingBase, object>, string>[]
			{
				new Tuple<Func<TPMSCodingBase, object>, string>((TPMSCodingBase A_0) => A_0, "ID3Visible")
			});
			BindingBase bindingBase16 = bindingExtension16.ProvideValue(null);
			entry3.SetBinding(VisualElement.IsVisibleProperty, bindingBase16);
			bindingExtension17.Mode = 1;
			bindingExtension17.Path = "ID3";
			bindingExtension17.TypedBinding = new TypedBinding<TPMSCodingBase, string>(delegate(TPMSCodingBase A_0)
			{
				if (A_0 != null)
				{
					return new ValueTuple<string, bool>(A_0.ID3, true);
				}
				return default(ValueTuple<string, bool>);
			}, delegate(TPMSCodingBase A_0, string A_1)
			{
				if (A_0 != null)
				{
					A_0.ID3 = A_1;
					return;
				}
			}, new Tuple<Func<TPMSCodingBase, object>, string>[]
			{
				new Tuple<Func<TPMSCodingBase, object>, string>((TPMSCodingBase A_0) => A_0, "ID3")
			});
			BindingBase bindingBase17 = bindingExtension17.ProvideValue(null);
			entry3.SetBinding(Entry.TextProperty, bindingBase17);
			stackLayout.Children.Add(entry3);
			bindingExtension18.Path = "ID4Visible";
			bindingExtension18.TypedBinding = new TypedBinding<TPMSCodingBase, bool>(delegate(TPMSCodingBase A_0)
			{
				if (A_0 != null)
				{
					return new ValueTuple<bool, bool>(A_0.ID4Visible, true);
				}
				return default(ValueTuple<bool, bool>);
			}, delegate(TPMSCodingBase A_0, bool A_1)
			{
				if (A_0 != null)
				{
					A_0.ID4Visible = A_1;
					return;
				}
			}, new Tuple<Func<TPMSCodingBase, object>, string>[]
			{
				new Tuple<Func<TPMSCodingBase, object>, string>((TPMSCodingBase A_0) => A_0, "ID4Visible")
			});
			BindingBase bindingBase18 = bindingExtension18.ProvideValue(null);
			label8.SetBinding(VisualElement.IsVisibleProperty, bindingBase18);
			bindingExtension19.Path = "ID4Title";
			bindingExtension19.TypedBinding = new TypedBinding<TPMSCodingBase, string>(delegate(TPMSCodingBase A_0)
			{
				if (A_0 != null)
				{
					return new ValueTuple<string, bool>(A_0.ID4Title, true);
				}
				return default(ValueTuple<string, bool>);
			}, delegate(TPMSCodingBase A_0, string A_1)
			{
				if (A_0 != null)
				{
					A_0.ID4Title = A_1;
					return;
				}
			}, new Tuple<Func<TPMSCodingBase, object>, string>[]
			{
				new Tuple<Func<TPMSCodingBase, object>, string>((TPMSCodingBase A_0) => A_0, "ID4Title")
			});
			BindingBase bindingBase19 = bindingExtension19.ProvideValue(null);
			label8.SetBinding(Label.TextProperty, bindingBase19);
			stackLayout.Children.Add(label8);
			bindingExtension20.Path = "ID4Visible";
			bindingExtension20.TypedBinding = new TypedBinding<TPMSCodingBase, bool>(delegate(TPMSCodingBase A_0)
			{
				if (A_0 != null)
				{
					return new ValueTuple<bool, bool>(A_0.ID4Visible, true);
				}
				return default(ValueTuple<bool, bool>);
			}, delegate(TPMSCodingBase A_0, bool A_1)
			{
				if (A_0 != null)
				{
					A_0.ID4Visible = A_1;
					return;
				}
			}, new Tuple<Func<TPMSCodingBase, object>, string>[]
			{
				new Tuple<Func<TPMSCodingBase, object>, string>((TPMSCodingBase A_0) => A_0, "ID4Visible")
			});
			BindingBase bindingBase20 = bindingExtension20.ProvideValue(null);
			entry4.SetBinding(VisualElement.IsVisibleProperty, bindingBase20);
			bindingExtension21.Mode = 1;
			bindingExtension21.Path = "ID4";
			bindingExtension21.TypedBinding = new TypedBinding<TPMSCodingBase, string>(delegate(TPMSCodingBase A_0)
			{
				if (A_0 != null)
				{
					return new ValueTuple<string, bool>(A_0.ID4, true);
				}
				return default(ValueTuple<string, bool>);
			}, delegate(TPMSCodingBase A_0, string A_1)
			{
				if (A_0 != null)
				{
					A_0.ID4 = A_1;
					return;
				}
			}, new Tuple<Func<TPMSCodingBase, object>, string>[]
			{
				new Tuple<Func<TPMSCodingBase, object>, string>((TPMSCodingBase A_0) => A_0, "ID4")
			});
			BindingBase bindingBase21 = bindingExtension21.ProvideValue(null);
			entry4.SetBinding(Entry.TextProperty, bindingBase21);
			stackLayout.Children.Add(entry4);
			bindingExtension22.Path = "ID5Visible";
			bindingExtension22.TypedBinding = new TypedBinding<TPMSCodingBase, bool>(delegate(TPMSCodingBase A_0)
			{
				if (A_0 != null)
				{
					return new ValueTuple<bool, bool>(A_0.ID5Visible, true);
				}
				return default(ValueTuple<bool, bool>);
			}, delegate(TPMSCodingBase A_0, bool A_1)
			{
				if (A_0 != null)
				{
					A_0.ID5Visible = A_1;
					return;
				}
			}, new Tuple<Func<TPMSCodingBase, object>, string>[]
			{
				new Tuple<Func<TPMSCodingBase, object>, string>((TPMSCodingBase A_0) => A_0, "ID5Visible")
			});
			BindingBase bindingBase22 = bindingExtension22.ProvideValue(null);
			label9.SetBinding(VisualElement.IsVisibleProperty, bindingBase22);
			bindingExtension23.Path = "ID5Title";
			bindingExtension23.TypedBinding = new TypedBinding<TPMSCodingBase, string>(delegate(TPMSCodingBase A_0)
			{
				if (A_0 != null)
				{
					return new ValueTuple<string, bool>(A_0.ID5Title, true);
				}
				return default(ValueTuple<string, bool>);
			}, delegate(TPMSCodingBase A_0, string A_1)
			{
				if (A_0 != null)
				{
					A_0.ID5Title = A_1;
					return;
				}
			}, new Tuple<Func<TPMSCodingBase, object>, string>[]
			{
				new Tuple<Func<TPMSCodingBase, object>, string>((TPMSCodingBase A_0) => A_0, "ID5Title")
			});
			BindingBase bindingBase23 = bindingExtension23.ProvideValue(null);
			label9.SetBinding(Label.TextProperty, bindingBase23);
			stackLayout.Children.Add(label9);
			bindingExtension24.Path = "ID5Visible";
			bindingExtension24.TypedBinding = new TypedBinding<TPMSCodingBase, bool>(delegate(TPMSCodingBase A_0)
			{
				if (A_0 != null)
				{
					return new ValueTuple<bool, bool>(A_0.ID5Visible, true);
				}
				return default(ValueTuple<bool, bool>);
			}, delegate(TPMSCodingBase A_0, bool A_1)
			{
				if (A_0 != null)
				{
					A_0.ID5Visible = A_1;
					return;
				}
			}, new Tuple<Func<TPMSCodingBase, object>, string>[]
			{
				new Tuple<Func<TPMSCodingBase, object>, string>((TPMSCodingBase A_0) => A_0, "ID5Visible")
			});
			BindingBase bindingBase24 = bindingExtension24.ProvideValue(null);
			entry5.SetBinding(VisualElement.IsVisibleProperty, bindingBase24);
			bindingExtension25.Mode = 1;
			bindingExtension25.Path = "ID5";
			bindingExtension25.TypedBinding = new TypedBinding<TPMSCodingBase, string>(delegate(TPMSCodingBase A_0)
			{
				if (A_0 != null)
				{
					return new ValueTuple<string, bool>(A_0.ID5, true);
				}
				return default(ValueTuple<string, bool>);
			}, delegate(TPMSCodingBase A_0, string A_1)
			{
				if (A_0 != null)
				{
					A_0.ID5 = A_1;
					return;
				}
			}, new Tuple<Func<TPMSCodingBase, object>, string>[]
			{
				new Tuple<Func<TPMSCodingBase, object>, string>((TPMSCodingBase A_0) => A_0, "ID5")
			});
			BindingBase bindingBase25 = bindingExtension25.ProvideValue(null);
			entry5.SetBinding(Entry.TextProperty, bindingBase25);
			stackLayout.Children.Add(entry5);
			button2.Clicked += this.btnSaveState_Clicked;
			translate4.Text = "coding_Apply";
			IMarkupExtension markupExtension8 = translate4;
			XamlServiceProvider xamlServiceProvider8 = new XamlServiceProvider();
			Type typeFromHandle15 = typeof(IProvideValueTarget);
			object[] array8 = new object[0 + 5];
			array8[0] = button2;
			array8[1] = stackLayout;
			array8[2] = grid;
			array8[3] = scrollView;
			array8[4] = this;
			object obj13;
			xamlServiceProvider8.Add(typeFromHandle15, obj13 = new SimpleValueTargetProvider(array8, Button.TextProperty, nameScope));
			xamlServiceProvider8.Add(typeof(IReferenceProvider), obj13);
			Type typeFromHandle16 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver8 = new XmlNamespaceResolver();
			xmlNamespaceResolver8.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver8.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver8.Add("coding", "clr-namespace:CarScannerXamarinForms.Coding");
			xmlNamespaceResolver8.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver8.Add("d", "http://xamarin.com/schemas/2014/forms/design");
			xmlNamespaceResolver8.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver8.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver8.Add("mc", "http://schemas.openxmlformats.org/markup-compatibility/2006");
			xmlNamespaceResolver8.Add("settings", "clr-namespace:CarScannerXamarinForms.Settings");
			xamlServiceProvider8.Add(typeFromHandle16, new XamlTypeResolver(xmlNamespaceResolver8, typeof(TPMS4WheelsPage).GetTypeInfo().Assembly));
			xamlServiceProvider8.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(73, 25)));
			object obj14 = markupExtension8.ProvideValue(xamlServiceProvider8);
			button2.Text = obj14;
			stackLayout.Children.Add(button2);
			label10.SetValue(Label.FontAttributesProperty, new FontAttributesConverter().ConvertFromInvariantString("Italic"));
			dynamicResourceExtension3.Key = "BaseFontSize-";
			IMarkupExtension<DynamicResource> markupExtension9 = dynamicResourceExtension3;
			XamlServiceProvider xamlServiceProvider9 = new XamlServiceProvider();
			Type typeFromHandle17 = typeof(IProvideValueTarget);
			object[] array9 = new object[0 + 5];
			array9[0] = label10;
			array9[1] = stackLayout;
			array9[2] = grid;
			array9[3] = scrollView;
			array9[4] = this;
			object obj15;
			xamlServiceProvider9.Add(typeFromHandle17, obj15 = new SimpleValueTargetProvider(array9, Label.FontSizeProperty, nameScope));
			xamlServiceProvider9.Add(typeof(IReferenceProvider), obj15);
			Type typeFromHandle18 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver9 = new XmlNamespaceResolver();
			xmlNamespaceResolver9.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver9.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver9.Add("coding", "clr-namespace:CarScannerXamarinForms.Coding");
			xmlNamespaceResolver9.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver9.Add("d", "http://xamarin.com/schemas/2014/forms/design");
			xmlNamespaceResolver9.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver9.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver9.Add("mc", "http://schemas.openxmlformats.org/markup-compatibility/2006");
			xmlNamespaceResolver9.Add("settings", "clr-namespace:CarScannerXamarinForms.Settings");
			xamlServiceProvider9.Add(typeFromHandle18, new XamlTypeResolver(xmlNamespaceResolver9, typeof(TPMS4WheelsPage).GetTypeInfo().Assembly));
			xamlServiceProvider9.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(81, 25)));
			DynamicResource dynamicResource3 = markupExtension9.ProvideValue(xamlServiceProvider9);
			label10.SetDynamicResource(Label.FontSizeProperty, dynamicResource3.Key);
			translate5.Text = "coding_UseHistory";
			IMarkupExtension markupExtension10 = translate5;
			XamlServiceProvider xamlServiceProvider10 = new XamlServiceProvider();
			Type typeFromHandle19 = typeof(IProvideValueTarget);
			object[] array10 = new object[0 + 5];
			array10[0] = label10;
			array10[1] = stackLayout;
			array10[2] = grid;
			array10[3] = scrollView;
			array10[4] = this;
			object obj16;
			xamlServiceProvider10.Add(typeFromHandle19, obj16 = new SimpleValueTargetProvider(array10, Label.TextProperty, nameScope));
			xamlServiceProvider10.Add(typeof(IReferenceProvider), obj16);
			Type typeFromHandle20 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver10 = new XmlNamespaceResolver();
			xmlNamespaceResolver10.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver10.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver10.Add("coding", "clr-namespace:CarScannerXamarinForms.Coding");
			xmlNamespaceResolver10.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver10.Add("d", "http://xamarin.com/schemas/2014/forms/design");
			xmlNamespaceResolver10.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver10.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver10.Add("mc", "http://schemas.openxmlformats.org/markup-compatibility/2006");
			xmlNamespaceResolver10.Add("settings", "clr-namespace:CarScannerXamarinForms.Settings");
			xamlServiceProvider10.Add(typeFromHandle20, new XamlTypeResolver(xmlNamespaceResolver10, typeof(TPMS4WheelsPage).GetTypeInfo().Assembly));
			xamlServiceProvider10.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(82, 25)));
			object obj17 = markupExtension10.ProvideValue(xamlServiceProvider10);
			label10.Text = obj17;
			stackLayout.Children.Add(label10);
			grid.Children.Add(stackLayout);
			activityFrame.SetValue(Grid.RowProperty, 0);
			grid.Children.Add(activityFrame);
			scrollView.Content = grid;
			this.SetValue(ContentPage.ContentProperty, scrollView);
		}

		// Token: 0x06004F76 RID: 20342 RVA: 0x003CABCA File Offset: 0x003C8DCA
		[CompilerGenerated]
		private void <btnSaveState_Clicked>b__6_0(string s)
		{
			Device.BeginInvokeOnMainThread(delegate
			{
				this.activityFrame.Text = s;
			});
		}

		// Token: 0x06004F77 RID: 20343 RVA: 0x003CABF0 File Offset: 0x003C8DF0
		private void __InitComponentRuntime()
		{
			Extensions.LoadFromXaml<TPMS4WheelsPage>(this, typeof(TPMS4WheelsPage));
			this.btnUpdateState = NameScopeExtensions.FindByName<Button>(this, "btnUpdateState");
			this.entryId1 = NameScopeExtensions.FindByName<Entry>(this, "entryId1");
			this.entryId2 = NameScopeExtensions.FindByName<Entry>(this, "entryId2");
			this.entryId3 = NameScopeExtensions.FindByName<Entry>(this, "entryId3");
			this.entryId4 = NameScopeExtensions.FindByName<Entry>(this, "entryId4");
			this.entryId5 = NameScopeExtensions.FindByName<Entry>(this, "entryId5");
			this.btnSaveNewValue = NameScopeExtensions.FindByName<Button>(this, "btnSaveNewValue");
			this.activityFrame = NameScopeExtensions.FindByName<ActivityFrame>(this, "activityFrame");
		}

		// Token: 0x06004F78 RID: 20344 RVA: 0x003CAC98 File Offset: 0x003C8E98
		[CompilerGenerated]
		private static ValueTuple<string, bool> <InitializeComponent>typedBindingsM__189(TPMSCodingBase A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<string, bool>(A_0.Name, true);
			}
			return default(ValueTuple<string, bool>);
		}

		// Token: 0x06004F79 RID: 20345 RVA: 0x003CACC8 File Offset: 0x003C8EC8
		[CompilerGenerated]
		private static void <InitializeComponent>typedBindingsM__190(TPMSCodingBase A_0, string A_1)
		{
			if (A_0 != null)
			{
				A_0.Name = A_1;
				return;
			}
		}

		// Token: 0x06004F7A RID: 20346 RVA: 0x003CACE4 File Offset: 0x003C8EE4
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__191(TPMSCodingBase A_0)
		{
			return A_0;
		}

		// Token: 0x06004F7B RID: 20347 RVA: 0x003CACF4 File Offset: 0x003C8EF4
		[CompilerGenerated]
		private static ValueTuple<string, bool> <InitializeComponent>typedBindingsM__192(TPMSCodingBase A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<string, bool>(A_0.Description, true);
			}
			return default(ValueTuple<string, bool>);
		}

		// Token: 0x06004F7C RID: 20348 RVA: 0x003CAD24 File Offset: 0x003C8F24
		[CompilerGenerated]
		private static void <InitializeComponent>typedBindingsM__193(TPMSCodingBase A_0, string A_1)
		{
			if (A_0 != null)
			{
				A_0.Description = A_1;
				return;
			}
		}

		// Token: 0x06004F7D RID: 20349 RVA: 0x003CAD40 File Offset: 0x003C8F40
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__194(TPMSCodingBase A_0)
		{
			return A_0;
		}

		// Token: 0x06004F7E RID: 20350 RVA: 0x003CAD50 File Offset: 0x003C8F50
		[CompilerGenerated]
		private static ValueTuple<string, bool> <InitializeComponent>typedBindingsM__195(TPMSCodingBase A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<string, bool>(A_0.Description, true);
			}
			return default(ValueTuple<string, bool>);
		}

		// Token: 0x06004F7F RID: 20351 RVA: 0x003CAD80 File Offset: 0x003C8F80
		[CompilerGenerated]
		private static void <InitializeComponent>typedBindingsM__196(TPMSCodingBase A_0, string A_1)
		{
			if (A_0 != null)
			{
				A_0.Description = A_1;
				return;
			}
		}

		// Token: 0x06004F80 RID: 20352 RVA: 0x003CAD9C File Offset: 0x003C8F9C
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__197(TPMSCodingBase A_0)
		{
			return A_0;
		}

		// Token: 0x06004F81 RID: 20353 RVA: 0x003CADAC File Offset: 0x003C8FAC
		[CompilerGenerated]
		private static ValueTuple<string, bool> <InitializeComponent>typedBindingsM__198(TPMSCodingBase A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<string, bool>(A_0.InnerDescription, true);
			}
			return default(ValueTuple<string, bool>);
		}

		// Token: 0x06004F82 RID: 20354 RVA: 0x003CADDC File Offset: 0x003C8FDC
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__199(TPMSCodingBase A_0)
		{
			return A_0;
		}

		// Token: 0x06004F83 RID: 20355 RVA: 0x003CADEC File Offset: 0x003C8FEC
		[CompilerGenerated]
		private static ValueTuple<string, bool> <InitializeComponent>typedBindingsM__200(TPMSCodingBase A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<string, bool>(A_0.InnerDescription, true);
			}
			return default(ValueTuple<string, bool>);
		}

		// Token: 0x06004F84 RID: 20356 RVA: 0x003CAE1C File Offset: 0x003C901C
		[CompilerGenerated]
		private static void <InitializeComponent>typedBindingsM__201(TPMSCodingBase A_0, string A_1)
		{
			if (A_0 != null)
			{
				A_0.InnerDescription = A_1;
				return;
			}
		}

		// Token: 0x06004F85 RID: 20357 RVA: 0x003CAE38 File Offset: 0x003C9038
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__202(TPMSCodingBase A_0)
		{
			return A_0;
		}

		// Token: 0x06004F86 RID: 20358 RVA: 0x003CAE48 File Offset: 0x003C9048
		[CompilerGenerated]
		private static ValueTuple<bool, bool> <InitializeComponent>typedBindingsM__203(TPMSCodingBase A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<bool, bool>(A_0.ID1Visible, true);
			}
			return default(ValueTuple<bool, bool>);
		}

		// Token: 0x06004F87 RID: 20359 RVA: 0x003CAE78 File Offset: 0x003C9078
		[CompilerGenerated]
		private static void <InitializeComponent>typedBindingsM__204(TPMSCodingBase A_0, bool A_1)
		{
			if (A_0 != null)
			{
				A_0.ID1Visible = A_1;
				return;
			}
		}

		// Token: 0x06004F88 RID: 20360 RVA: 0x003CAE94 File Offset: 0x003C9094
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__205(TPMSCodingBase A_0)
		{
			return A_0;
		}

		// Token: 0x06004F89 RID: 20361 RVA: 0x003CAEA4 File Offset: 0x003C90A4
		[CompilerGenerated]
		private static ValueTuple<string, bool> <InitializeComponent>typedBindingsM__206(TPMSCodingBase A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<string, bool>(A_0.ID1Title, true);
			}
			return default(ValueTuple<string, bool>);
		}

		// Token: 0x06004F8A RID: 20362 RVA: 0x003CAED4 File Offset: 0x003C90D4
		[CompilerGenerated]
		private static void <InitializeComponent>typedBindingsM__207(TPMSCodingBase A_0, string A_1)
		{
			if (A_0 != null)
			{
				A_0.ID1Title = A_1;
				return;
			}
		}

		// Token: 0x06004F8B RID: 20363 RVA: 0x003CAEF0 File Offset: 0x003C90F0
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__208(TPMSCodingBase A_0)
		{
			return A_0;
		}

		// Token: 0x06004F8C RID: 20364 RVA: 0x003CAF00 File Offset: 0x003C9100
		[CompilerGenerated]
		private static ValueTuple<bool, bool> <InitializeComponent>typedBindingsM__209(TPMSCodingBase A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<bool, bool>(A_0.ID1Visible, true);
			}
			return default(ValueTuple<bool, bool>);
		}

		// Token: 0x06004F8D RID: 20365 RVA: 0x003CAF30 File Offset: 0x003C9130
		[CompilerGenerated]
		private static void <InitializeComponent>typedBindingsM__210(TPMSCodingBase A_0, bool A_1)
		{
			if (A_0 != null)
			{
				A_0.ID1Visible = A_1;
				return;
			}
		}

		// Token: 0x06004F8E RID: 20366 RVA: 0x003CAF4C File Offset: 0x003C914C
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__211(TPMSCodingBase A_0)
		{
			return A_0;
		}

		// Token: 0x06004F8F RID: 20367 RVA: 0x003CAF5C File Offset: 0x003C915C
		[CompilerGenerated]
		private static ValueTuple<string, bool> <InitializeComponent>typedBindingsM__212(TPMSCodingBase A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<string, bool>(A_0.ID1, true);
			}
			return default(ValueTuple<string, bool>);
		}

		// Token: 0x06004F90 RID: 20368 RVA: 0x003CAF8C File Offset: 0x003C918C
		[CompilerGenerated]
		private static void <InitializeComponent>typedBindingsM__213(TPMSCodingBase A_0, string A_1)
		{
			if (A_0 != null)
			{
				A_0.ID1 = A_1;
				return;
			}
		}

		// Token: 0x06004F91 RID: 20369 RVA: 0x003CAFA8 File Offset: 0x003C91A8
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__214(TPMSCodingBase A_0)
		{
			return A_0;
		}

		// Token: 0x06004F92 RID: 20370 RVA: 0x003CAFB8 File Offset: 0x003C91B8
		[CompilerGenerated]
		private static ValueTuple<bool, bool> <InitializeComponent>typedBindingsM__215(TPMSCodingBase A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<bool, bool>(A_0.ID2Visible, true);
			}
			return default(ValueTuple<bool, bool>);
		}

		// Token: 0x06004F93 RID: 20371 RVA: 0x003CAFE8 File Offset: 0x003C91E8
		[CompilerGenerated]
		private static void <InitializeComponent>typedBindingsM__216(TPMSCodingBase A_0, bool A_1)
		{
			if (A_0 != null)
			{
				A_0.ID2Visible = A_1;
				return;
			}
		}

		// Token: 0x06004F94 RID: 20372 RVA: 0x003CB004 File Offset: 0x003C9204
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__217(TPMSCodingBase A_0)
		{
			return A_0;
		}

		// Token: 0x06004F95 RID: 20373 RVA: 0x003CB014 File Offset: 0x003C9214
		[CompilerGenerated]
		private static ValueTuple<string, bool> <InitializeComponent>typedBindingsM__218(TPMSCodingBase A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<string, bool>(A_0.ID2Title, true);
			}
			return default(ValueTuple<string, bool>);
		}

		// Token: 0x06004F96 RID: 20374 RVA: 0x003CB044 File Offset: 0x003C9244
		[CompilerGenerated]
		private static void <InitializeComponent>typedBindingsM__219(TPMSCodingBase A_0, string A_1)
		{
			if (A_0 != null)
			{
				A_0.ID2Title = A_1;
				return;
			}
		}

		// Token: 0x06004F97 RID: 20375 RVA: 0x003CB060 File Offset: 0x003C9260
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__220(TPMSCodingBase A_0)
		{
			return A_0;
		}

		// Token: 0x06004F98 RID: 20376 RVA: 0x003CB070 File Offset: 0x003C9270
		[CompilerGenerated]
		private static ValueTuple<bool, bool> <InitializeComponent>typedBindingsM__221(TPMSCodingBase A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<bool, bool>(A_0.ID2Visible, true);
			}
			return default(ValueTuple<bool, bool>);
		}

		// Token: 0x06004F99 RID: 20377 RVA: 0x003CB0A0 File Offset: 0x003C92A0
		[CompilerGenerated]
		private static void <InitializeComponent>typedBindingsM__222(TPMSCodingBase A_0, bool A_1)
		{
			if (A_0 != null)
			{
				A_0.ID2Visible = A_1;
				return;
			}
		}

		// Token: 0x06004F9A RID: 20378 RVA: 0x003CB0BC File Offset: 0x003C92BC
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__223(TPMSCodingBase A_0)
		{
			return A_0;
		}

		// Token: 0x06004F9B RID: 20379 RVA: 0x003CB0CC File Offset: 0x003C92CC
		[CompilerGenerated]
		private static ValueTuple<string, bool> <InitializeComponent>typedBindingsM__224(TPMSCodingBase A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<string, bool>(A_0.ID2, true);
			}
			return default(ValueTuple<string, bool>);
		}

		// Token: 0x06004F9C RID: 20380 RVA: 0x003CB0FC File Offset: 0x003C92FC
		[CompilerGenerated]
		private static void <InitializeComponent>typedBindingsM__225(TPMSCodingBase A_0, string A_1)
		{
			if (A_0 != null)
			{
				A_0.ID2 = A_1;
				return;
			}
		}

		// Token: 0x06004F9D RID: 20381 RVA: 0x003CB118 File Offset: 0x003C9318
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__226(TPMSCodingBase A_0)
		{
			return A_0;
		}

		// Token: 0x06004F9E RID: 20382 RVA: 0x003CB128 File Offset: 0x003C9328
		[CompilerGenerated]
		private static ValueTuple<bool, bool> <InitializeComponent>typedBindingsM__227(TPMSCodingBase A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<bool, bool>(A_0.ID3Visible, true);
			}
			return default(ValueTuple<bool, bool>);
		}

		// Token: 0x06004F9F RID: 20383 RVA: 0x003CB158 File Offset: 0x003C9358
		[CompilerGenerated]
		private static void <InitializeComponent>typedBindingsM__228(TPMSCodingBase A_0, bool A_1)
		{
			if (A_0 != null)
			{
				A_0.ID3Visible = A_1;
				return;
			}
		}

		// Token: 0x06004FA0 RID: 20384 RVA: 0x003CB174 File Offset: 0x003C9374
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__229(TPMSCodingBase A_0)
		{
			return A_0;
		}

		// Token: 0x06004FA1 RID: 20385 RVA: 0x003CB184 File Offset: 0x003C9384
		[CompilerGenerated]
		private static ValueTuple<string, bool> <InitializeComponent>typedBindingsM__230(TPMSCodingBase A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<string, bool>(A_0.ID3Title, true);
			}
			return default(ValueTuple<string, bool>);
		}

		// Token: 0x06004FA2 RID: 20386 RVA: 0x003CB1B4 File Offset: 0x003C93B4
		[CompilerGenerated]
		private static void <InitializeComponent>typedBindingsM__231(TPMSCodingBase A_0, string A_1)
		{
			if (A_0 != null)
			{
				A_0.ID3Title = A_1;
				return;
			}
		}

		// Token: 0x06004FA3 RID: 20387 RVA: 0x003CB1D0 File Offset: 0x003C93D0
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__232(TPMSCodingBase A_0)
		{
			return A_0;
		}

		// Token: 0x06004FA4 RID: 20388 RVA: 0x003CB1E0 File Offset: 0x003C93E0
		[CompilerGenerated]
		private static ValueTuple<bool, bool> <InitializeComponent>typedBindingsM__233(TPMSCodingBase A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<bool, bool>(A_0.ID3Visible, true);
			}
			return default(ValueTuple<bool, bool>);
		}

		// Token: 0x06004FA5 RID: 20389 RVA: 0x003CB210 File Offset: 0x003C9410
		[CompilerGenerated]
		private static void <InitializeComponent>typedBindingsM__234(TPMSCodingBase A_0, bool A_1)
		{
			if (A_0 != null)
			{
				A_0.ID3Visible = A_1;
				return;
			}
		}

		// Token: 0x06004FA6 RID: 20390 RVA: 0x003CB22C File Offset: 0x003C942C
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__235(TPMSCodingBase A_0)
		{
			return A_0;
		}

		// Token: 0x06004FA7 RID: 20391 RVA: 0x003CB23C File Offset: 0x003C943C
		[CompilerGenerated]
		private static ValueTuple<string, bool> <InitializeComponent>typedBindingsM__236(TPMSCodingBase A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<string, bool>(A_0.ID3, true);
			}
			return default(ValueTuple<string, bool>);
		}

		// Token: 0x06004FA8 RID: 20392 RVA: 0x003CB26C File Offset: 0x003C946C
		[CompilerGenerated]
		private static void <InitializeComponent>typedBindingsM__237(TPMSCodingBase A_0, string A_1)
		{
			if (A_0 != null)
			{
				A_0.ID3 = A_1;
				return;
			}
		}

		// Token: 0x06004FA9 RID: 20393 RVA: 0x003CB288 File Offset: 0x003C9488
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__238(TPMSCodingBase A_0)
		{
			return A_0;
		}

		// Token: 0x06004FAA RID: 20394 RVA: 0x003CB298 File Offset: 0x003C9498
		[CompilerGenerated]
		private static ValueTuple<bool, bool> <InitializeComponent>typedBindingsM__239(TPMSCodingBase A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<bool, bool>(A_0.ID4Visible, true);
			}
			return default(ValueTuple<bool, bool>);
		}

		// Token: 0x06004FAB RID: 20395 RVA: 0x003CB2C8 File Offset: 0x003C94C8
		[CompilerGenerated]
		private static void <InitializeComponent>typedBindingsM__240(TPMSCodingBase A_0, bool A_1)
		{
			if (A_0 != null)
			{
				A_0.ID4Visible = A_1;
				return;
			}
		}

		// Token: 0x06004FAC RID: 20396 RVA: 0x003CB2E4 File Offset: 0x003C94E4
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__241(TPMSCodingBase A_0)
		{
			return A_0;
		}

		// Token: 0x06004FAD RID: 20397 RVA: 0x003CB2F4 File Offset: 0x003C94F4
		[CompilerGenerated]
		private static ValueTuple<string, bool> <InitializeComponent>typedBindingsM__242(TPMSCodingBase A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<string, bool>(A_0.ID4Title, true);
			}
			return default(ValueTuple<string, bool>);
		}

		// Token: 0x06004FAE RID: 20398 RVA: 0x003CB324 File Offset: 0x003C9524
		[CompilerGenerated]
		private static void <InitializeComponent>typedBindingsM__243(TPMSCodingBase A_0, string A_1)
		{
			if (A_0 != null)
			{
				A_0.ID4Title = A_1;
				return;
			}
		}

		// Token: 0x06004FAF RID: 20399 RVA: 0x003CB340 File Offset: 0x003C9540
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__244(TPMSCodingBase A_0)
		{
			return A_0;
		}

		// Token: 0x06004FB0 RID: 20400 RVA: 0x003CB350 File Offset: 0x003C9550
		[CompilerGenerated]
		private static ValueTuple<bool, bool> <InitializeComponent>typedBindingsM__245(TPMSCodingBase A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<bool, bool>(A_0.ID4Visible, true);
			}
			return default(ValueTuple<bool, bool>);
		}

		// Token: 0x06004FB1 RID: 20401 RVA: 0x003CB380 File Offset: 0x003C9580
		[CompilerGenerated]
		private static void <InitializeComponent>typedBindingsM__246(TPMSCodingBase A_0, bool A_1)
		{
			if (A_0 != null)
			{
				A_0.ID4Visible = A_1;
				return;
			}
		}

		// Token: 0x06004FB2 RID: 20402 RVA: 0x003CB39C File Offset: 0x003C959C
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__247(TPMSCodingBase A_0)
		{
			return A_0;
		}

		// Token: 0x06004FB3 RID: 20403 RVA: 0x003CB3AC File Offset: 0x003C95AC
		[CompilerGenerated]
		private static ValueTuple<string, bool> <InitializeComponent>typedBindingsM__248(TPMSCodingBase A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<string, bool>(A_0.ID4, true);
			}
			return default(ValueTuple<string, bool>);
		}

		// Token: 0x06004FB4 RID: 20404 RVA: 0x003CB3DC File Offset: 0x003C95DC
		[CompilerGenerated]
		private static void <InitializeComponent>typedBindingsM__249(TPMSCodingBase A_0, string A_1)
		{
			if (A_0 != null)
			{
				A_0.ID4 = A_1;
				return;
			}
		}

		// Token: 0x06004FB5 RID: 20405 RVA: 0x003CB3F8 File Offset: 0x003C95F8
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__250(TPMSCodingBase A_0)
		{
			return A_0;
		}

		// Token: 0x06004FB6 RID: 20406 RVA: 0x003CB408 File Offset: 0x003C9608
		[CompilerGenerated]
		private static ValueTuple<bool, bool> <InitializeComponent>typedBindingsM__251(TPMSCodingBase A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<bool, bool>(A_0.ID5Visible, true);
			}
			return default(ValueTuple<bool, bool>);
		}

		// Token: 0x06004FB7 RID: 20407 RVA: 0x003CB438 File Offset: 0x003C9638
		[CompilerGenerated]
		private static void <InitializeComponent>typedBindingsM__252(TPMSCodingBase A_0, bool A_1)
		{
			if (A_0 != null)
			{
				A_0.ID5Visible = A_1;
				return;
			}
		}

		// Token: 0x06004FB8 RID: 20408 RVA: 0x003CB454 File Offset: 0x003C9654
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__253(TPMSCodingBase A_0)
		{
			return A_0;
		}

		// Token: 0x06004FB9 RID: 20409 RVA: 0x003CB464 File Offset: 0x003C9664
		[CompilerGenerated]
		private static ValueTuple<string, bool> <InitializeComponent>typedBindingsM__254(TPMSCodingBase A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<string, bool>(A_0.ID5Title, true);
			}
			return default(ValueTuple<string, bool>);
		}

		// Token: 0x06004FBA RID: 20410 RVA: 0x003CB494 File Offset: 0x003C9694
		[CompilerGenerated]
		private static void <InitializeComponent>typedBindingsM__255(TPMSCodingBase A_0, string A_1)
		{
			if (A_0 != null)
			{
				A_0.ID5Title = A_1;
				return;
			}
		}

		// Token: 0x06004FBB RID: 20411 RVA: 0x003CB4B0 File Offset: 0x003C96B0
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__256(TPMSCodingBase A_0)
		{
			return A_0;
		}

		// Token: 0x06004FBC RID: 20412 RVA: 0x003CB4C0 File Offset: 0x003C96C0
		[CompilerGenerated]
		private static ValueTuple<bool, bool> <InitializeComponent>typedBindingsM__257(TPMSCodingBase A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<bool, bool>(A_0.ID5Visible, true);
			}
			return default(ValueTuple<bool, bool>);
		}

		// Token: 0x06004FBD RID: 20413 RVA: 0x003CB4F0 File Offset: 0x003C96F0
		[CompilerGenerated]
		private static void <InitializeComponent>typedBindingsM__258(TPMSCodingBase A_0, bool A_1)
		{
			if (A_0 != null)
			{
				A_0.ID5Visible = A_1;
				return;
			}
		}

		// Token: 0x06004FBE RID: 20414 RVA: 0x003CB50C File Offset: 0x003C970C
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__259(TPMSCodingBase A_0)
		{
			return A_0;
		}

		// Token: 0x06004FBF RID: 20415 RVA: 0x003CB51C File Offset: 0x003C971C
		[CompilerGenerated]
		private static ValueTuple<string, bool> <InitializeComponent>typedBindingsM__260(TPMSCodingBase A_0)
		{
			if (A_0 != null)
			{
				return new ValueTuple<string, bool>(A_0.ID5, true);
			}
			return default(ValueTuple<string, bool>);
		}

		// Token: 0x06004FC0 RID: 20416 RVA: 0x003CB54C File Offset: 0x003C974C
		[CompilerGenerated]
		private static void <InitializeComponent>typedBindingsM__261(TPMSCodingBase A_0, string A_1)
		{
			if (A_0 != null)
			{
				A_0.ID5 = A_1;
				return;
			}
		}

		// Token: 0x06004FC1 RID: 20417 RVA: 0x003CB568 File Offset: 0x003C9768
		[CompilerGenerated]
		private static object <InitializeComponent>typedBindingsM__262(TPMSCodingBase A_0)
		{
			return A_0;
		}

		// Token: 0x04002FBE RID: 12222
		private bool first_appearing = true;

		// Token: 0x04002FBF RID: 12223
		private ICodingContainer coding;

		// Token: 0x04002FC0 RID: 12224
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private Button btnUpdateState;

		// Token: 0x04002FC1 RID: 12225
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private Entry entryId1;

		// Token: 0x04002FC2 RID: 12226
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private Entry entryId2;

		// Token: 0x04002FC3 RID: 12227
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private Entry entryId3;

		// Token: 0x04002FC4 RID: 12228
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private Entry entryId4;

		// Token: 0x04002FC5 RID: 12229
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private Entry entryId5;

		// Token: 0x04002FC6 RID: 12230
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private Button btnSaveNewValue;

		// Token: 0x04002FC7 RID: 12231
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private ActivityFrame activityFrame;

		// Token: 0x02000972 RID: 2418
		[CompilerGenerated]
		private sealed class <>c__DisplayClass6_0
		{
			// Token: 0x06004FC2 RID: 20418 RVA: 0x00002050 File Offset: 0x00000250
			public <>c__DisplayClass6_0()
			{
			}

			// Token: 0x06004FC3 RID: 20419 RVA: 0x003CB576 File Offset: 0x003C9776
			internal void <btnSaveState_Clicked>b__1()
			{
				this.<>4__this.activityFrame.Text = this.s;
			}

			// Token: 0x04002FC8 RID: 12232
			public string s;

			// Token: 0x04002FC9 RID: 12233
			public TPMS4WheelsPage <>4__this;
		}

		// Token: 0x02000973 RID: 2419
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <BtnUpdateState_Clicked>d__4 : IAsyncStateMachine
		{
			// Token: 0x06004FC4 RID: 20420 RVA: 0x003CB590 File Offset: 0x003C9790
			void IAsyncStateMachine.MoveNext()
			{
				TPMS4WheelsPage tpms4WheelsPage = this;
				try
				{
					tpms4WheelsPage.UpdateState();
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

			// Token: 0x06004FC5 RID: 20421 RVA: 0x003CB5E8 File Offset: 0x003C97E8
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x04002FCA RID: 12234
			public int <>1__state;

			// Token: 0x04002FCB RID: 12235
			public AsyncVoidMethodBuilder <>t__builder;

			// Token: 0x04002FCC RID: 12236
			public TPMS4WheelsPage <>4__this;
		}

		// Token: 0x02000974 RID: 2420
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <CodingDetailsPage_Appearing>d__2 : IAsyncStateMachine
		{
			// Token: 0x06004FC6 RID: 20422 RVA: 0x003CB5F8 File Offset: 0x003C97F8
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				TPMS4WheelsPage tpms4WheelsPage = this;
				try
				{
					TaskAwaiter<CodingRequestResult> taskAwaiter;
					if (num != 0)
					{
						if (!tpms4WheelsPage.first_appearing)
						{
							goto IL_009F;
						}
						tpms4WheelsPage.first_appearing = false;
						tpms4WheelsPage.activityFrame.IsVisible = true;
						taskAwaiter = tpms4WheelsPage.coding.UpdateCurrentState("", null).GetAwaiter();
						if (!taskAwaiter.IsCompleted)
						{
							num2 = 0;
							TaskAwaiter<CodingRequestResult> taskAwaiter2 = taskAwaiter;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<CodingRequestResult>, TPMS4WheelsPage.<CodingDetailsPage_Appearing>d__2>(ref taskAwaiter, ref this);
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
					tpms4WheelsPage.activityFrame.IsVisible = false;
					IL_009F:;
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

			// Token: 0x06004FC7 RID: 20423 RVA: 0x003CB6E0 File Offset: 0x003C98E0
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x04002FCD RID: 12237
			public int <>1__state;

			// Token: 0x04002FCE RID: 12238
			public AsyncVoidMethodBuilder <>t__builder;

			// Token: 0x04002FCF RID: 12239
			public TPMS4WheelsPage <>4__this;

			// Token: 0x04002FD0 RID: 12240
			private TaskAwaiter<CodingRequestResult> <>u__1;
		}

		// Token: 0x02000975 RID: 2421
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <UpdateState>d__3 : IAsyncStateMachine
		{
			// Token: 0x06004FC8 RID: 20424 RVA: 0x003CB6F0 File Offset: 0x003C98F0
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				TPMS4WheelsPage tpms4WheelsPage = this;
				try
				{
					TaskAwaiter<CodingRequestResult> taskAwaiter;
					if (num != 0)
					{
						tpms4WheelsPage.activityFrame.IsVisible = true;
						taskAwaiter = tpms4WheelsPage.coding.UpdateCurrentState("", null).GetAwaiter();
						if (!taskAwaiter.IsCompleted)
						{
							num2 = 0;
							TaskAwaiter<CodingRequestResult> taskAwaiter2 = taskAwaiter;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<CodingRequestResult>, TPMS4WheelsPage.<UpdateState>d__3>(ref taskAwaiter, ref this);
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
					tpms4WheelsPage.activityFrame.IsVisible = false;
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

			// Token: 0x06004FC9 RID: 20425 RVA: 0x003CB7C8 File Offset: 0x003C99C8
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x04002FD1 RID: 12241
			public int <>1__state;

			// Token: 0x04002FD2 RID: 12242
			public AsyncTaskMethodBuilder <>t__builder;

			// Token: 0x04002FD3 RID: 12243
			public TPMS4WheelsPage <>4__this;

			// Token: 0x04002FD4 RID: 12244
			private TaskAwaiter<CodingRequestResult> <>u__1;
		}

		// Token: 0x02000976 RID: 2422
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <btnSaveState_Clicked>d__6 : IAsyncStateMachine
		{
			// Token: 0x06004FCA RID: 20426 RVA: 0x003CB7D8 File Offset: 0x003C99D8
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				TPMS4WheelsPage tpms4WheelsPage = this;
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
						goto IL_02F2;
					}
					case 5:
					{
						TaskAwaiter taskAwaiter5;
						taskAwaiter4 = taskAwaiter5;
						taskAwaiter5 = default(TaskAwaiter);
						num2 = -1;
						goto IL_0381;
					}
					case 6:
					{
						TaskAwaiter<CodingRequestResult> taskAwaiter7;
						taskAwaiter6 = taskAwaiter7;
						taskAwaiter7 = default(TaskAwaiter<CodingRequestResult>);
						num2 = -1;
						goto IL_03E7;
					}
					default:
						if (tpms4WheelsPage.coding.RequiresPro && !SharedSettings.Current.AdsProductPurchased)
						{
							taskAwaiter3 = tpms4WheelsPage.DisplayAlert("Car Scanner Pro", Translate.GetString("FeatureLocked"), "OK", Translate.GetString("btnCancel.Content")).GetAwaiter();
							if (!taskAwaiter3.IsCompleted)
							{
								num2 = 0;
								taskAwaiter2 = taskAwaiter3;
								this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<bool>, TPMS4WheelsPage.<btnSaveState_Clicked>d__6>(ref taskAwaiter3, ref this);
								return;
							}
						}
						else if (SharedSettings.Current.CodingsCounter >= 3 && !SharedSettings.Current.AdsProductPurchased)
						{
							taskAwaiter3 = tpms4WheelsPage.DisplayAlert("Car Scanner Pro", string.Format(Translate.GetString("coding_FreeCodingsFinished"), SharedSettings.Current.CodingsCounter, 3), "OK", Translate.GetString("btnCancel.Content")).GetAwaiter();
							if (!taskAwaiter3.IsCompleted)
							{
								num2 = 2;
								taskAwaiter2 = taskAwaiter3;
								this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<bool>, TPMS4WheelsPage.<btnSaveState_Clicked>d__6>(ref taskAwaiter3, ref this);
								return;
							}
							goto IL_01EC;
						}
						else
						{
							tpms4WheelsPage.activityFrame.IsVisible = true;
							Progress<string> progress = new Progress<string>(delegate(string s)
							{
								Device.BeginInvokeOnMainThread(new Action(new TPMS4WheelsPage.<>c__DisplayClass6_0
								{
									<>4__this = tpms4WheelsPage,
									s = s
								}.<btnSaveState_Clicked>b__1));
							});
							taskAwaiter6 = tpms4WheelsPage.coding.Execute("", "", "", progress, null, false).GetAwaiter();
							if (!taskAwaiter6.IsCompleted)
							{
								num2 = 4;
								TaskAwaiter<CodingRequestResult> taskAwaiter7 = taskAwaiter6;
								this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<CodingRequestResult>, TPMS4WheelsPage.<btnSaveState_Clicked>d__6>(ref taskAwaiter6, ref this);
								return;
							}
							goto IL_02F2;
						}
						break;
					}
					if (!taskAwaiter3.GetResult())
					{
						goto IL_0139;
					}
					Page inAppPage = InAppManager.GetInAppPage();
					taskAwaiter4 = tpms4WheelsPage.Navigation.PushAsync(inAppPage).GetAwaiter();
					if (!taskAwaiter4.IsCompleted)
					{
						num2 = 1;
						TaskAwaiter taskAwaiter5 = taskAwaiter4;
						this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, TPMS4WheelsPage.<btnSaveState_Clicked>d__6>(ref taskAwaiter4, ref this);
						return;
					}
					IL_0132:
					taskAwaiter4.GetResult();
					IL_0139:
					goto IL_0426;
					IL_01EC:
					if (!taskAwaiter3.GetResult())
					{
						goto IL_0261;
					}
					Page inAppPage2 = InAppManager.GetInAppPage();
					taskAwaiter4 = tpms4WheelsPage.Navigation.PushAsync(inAppPage2).GetAwaiter();
					if (!taskAwaiter4.IsCompleted)
					{
						num2 = 3;
						TaskAwaiter taskAwaiter5 = taskAwaiter4;
						this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, TPMS4WheelsPage.<btnSaveState_Clicked>d__6>(ref taskAwaiter4, ref this);
						return;
					}
					IL_025A:
					taskAwaiter4.GetResult();
					IL_0261:
					goto IL_0426;
					IL_02F2:
					CodingRequestResult result = taskAwaiter6.GetResult();
					if (result == CodingRequestResult.Success)
					{
						SharedSettings sharedSettings = SharedSettings.Current;
						int codingsCounter = sharedSettings.CodingsCounter;
						sharedSettings.CodingsCounter = codingsCounter + 1;
						goto IL_0388;
					}
					taskAwaiter4 = tpms4WheelsPage.DisplayAlert(Translate.GetString("coding_Error"), MQBAdaptationTemplate.CodingRequestResultToString(result), "OK").GetAwaiter();
					if (!taskAwaiter4.IsCompleted)
					{
						num2 = 5;
						TaskAwaiter taskAwaiter5 = taskAwaiter4;
						this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, TPMS4WheelsPage.<btnSaveState_Clicked>d__6>(ref taskAwaiter4, ref this);
						return;
					}
					IL_0381:
					taskAwaiter4.GetResult();
					IL_0388:
					taskAwaiter6 = tpms4WheelsPage.coding.UpdateCurrentState("", null).GetAwaiter();
					if (!taskAwaiter6.IsCompleted)
					{
						num2 = 6;
						TaskAwaiter<CodingRequestResult> taskAwaiter7 = taskAwaiter6;
						this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<CodingRequestResult>, TPMS4WheelsPage.<btnSaveState_Clicked>d__6>(ref taskAwaiter6, ref this);
						return;
					}
					IL_03E7:
					taskAwaiter6.GetResult();
					tpms4WheelsPage.activityFrame.IsVisible = false;
					tpms4WheelsPage.activityFrame.Text = ActivityFrame.PLEASE_WAIT_TEXT;
				}
				catch (Exception ex)
				{
					num2 = -2;
					this.<>t__builder.SetException(ex);
					return;
				}
				IL_0426:
				num2 = -2;
				this.<>t__builder.SetResult();
			}

			// Token: 0x06004FCB RID: 20427 RVA: 0x003CBC3C File Offset: 0x003C9E3C
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x04002FD5 RID: 12245
			public int <>1__state;

			// Token: 0x04002FD6 RID: 12246
			public AsyncVoidMethodBuilder <>t__builder;

			// Token: 0x04002FD7 RID: 12247
			public TPMS4WheelsPage <>4__this;

			// Token: 0x04002FD8 RID: 12248
			private TaskAwaiter<bool> <>u__1;

			// Token: 0x04002FD9 RID: 12249
			private TaskAwaiter <>u__2;

			// Token: 0x04002FDA RID: 12250
			private TaskAwaiter<CodingRequestResult> <>u__3;
		}
	}
}
