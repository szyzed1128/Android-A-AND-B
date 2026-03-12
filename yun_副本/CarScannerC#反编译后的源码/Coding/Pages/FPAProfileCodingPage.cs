using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using CarScannerXamarinForms.Coding.DB.MQB;
using CarScannerXamarinForms.Common;
using CarScannerXamarinForms.Common.XAMLConverters;
using CarScannerXamarinForms.InApp;
using CarScannerXamarinForms.Pages;
using CarScannerXamarinForms.Settings;
using Xamarin.Forms;
using Xamarin.Forms.Internals;
using Xamarin.Forms.PlatformConfiguration.iOSSpecific;
using Xamarin.Forms.Xaml;
using Xamarin.Forms.Xaml.Diagnostics;
using Xamarin.Forms.Xaml.Internals;

namespace CarScannerXamarinForms.Coding.Pages
{
	// Token: 0x02000948 RID: 2376
	[XamlCompilation(2)]
	[XamlFilePath("Coding\\Pages\\FPAProfileCodingPage.xaml")]
	public class FPAProfileCodingPage : ContentPage
	{
		// Token: 0x06004E77 RID: 20087 RVA: 0x003B5012 File Offset: 0x003B3212
		public FPAProfileCodingPage(ICodingContainer coding)
		{
			this.InitializeComponent();
			this.Coding = (FPACoding)coding;
			base.BindingContext = this.Coding;
			base.Appearing += this.FPAProfileCodingPage_Appearing;
		}

		// Token: 0x06004E78 RID: 20088 RVA: 0x003B5054 File Offset: 0x003B3254
		private async void FPAProfileCodingPage_Appearing(object sender, EventArgs e)
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

		// Token: 0x1700178E RID: 6030
		// (get) Token: 0x06004E79 RID: 20089 RVA: 0x003B508B File Offset: 0x003B328B
		// (set) Token: 0x06004E7A RID: 20090 RVA: 0x003B5093 File Offset: 0x003B3293
		internal FPACoding Coding
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

		// Token: 0x06004E7B RID: 20091 RVA: 0x003B509C File Offset: 0x003B329C
		private async void Lv_ItemSelected(object sender, SelectedItemChangedEventArgs e)
		{
			if (this.lv.SelectedItem != null)
			{
				FPAProfile fpaprofile = (FPAProfile)this.lv.SelectedItem;
				this.lv.SelectedItem = null;
				FPAProfileEditor fpaprofileEditor = new FPAProfileEditor(fpaprofile);
				await base.Navigation.PushAsync(fpaprofileEditor);
			}
		}

		// Token: 0x06004E7C RID: 20092 RVA: 0x003B50D4 File Offset: 0x003B32D4
		private async void btnChooseOtherVersion_Clicked(object sender, EventArgs e)
		{
			IEnumerable<ValueItemWithTranslation> enumerable = from x in (from x in PackageFileReader.GetFilesInDirectory("vag.fpa3Q0907530.")
					orderby x
					select x).ToArray<string>()
				select new ValueItemWithTranslation
				{
					Title = x,
					Value = x
				};
			Action<ValueItemWithTranslation> action = delegate(ValueItemWithTranslation selected)
			{
				this.Coding.LoadModel(selected.Value);
			};
			ItemWithValueSelectorPage itemWithValueSelectorPage = new ItemWithValueSelectorPage("Dataset version", enumerable, enumerable.FirstOrDefault((ValueItemWithTranslation x) => x.Value == this.Coding.CurrentState), action);
			await base.Navigation.PushAsync(itemWithValueSelectorPage);
		}

		// Token: 0x06004E7D RID: 20093 RVA: 0x003B510C File Offset: 0x003B330C
		private async void btnSaveState_Clicked(object sender, EventArgs e)
		{
			if (!this.Coding.IsLoaded)
			{
				await base.DisplayAlert(this.Coding.CurrentState, "", "OK");
			}
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

		// Token: 0x06004E7E RID: 20094 RVA: 0x003B5144 File Offset: 0x003B3344
		private async void btnProfile_Clicked(object sender, EventArgs e)
		{
			if (this.Coding.IsLoaded)
			{
				FPAProfileEditor fpaprofileEditor = new FPAProfileEditor((FPAProfile)((View)sender).BindingContext);
				await base.Navigation.PushAsync(fpaprofileEditor);
			}
		}

		// Token: 0x06004E7F RID: 20095 RVA: 0x003B5184 File Offset: 0x003B3384
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private void InitializeComponent()
		{
			if (ResourceLoader.IsEnabled && ResourceLoader.CanProvideContentFor(new ResourceLoader.ResourceLoadingQuery
			{
				AssemblyName = typeof(FPAProfileCodingPage).GetTypeInfo().Assembly.GetName(),
				ResourcePath = "Coding/Pages/FPAProfileCodingPage.xaml",
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
			VisualDiagnostics.RegisterSourceInfo(translate = new Translate(), new Uri("Coding\\Pages\\FPAProfileCodingPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 15, 5);
			DynamicResourceExtension dynamicResourceExtension;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension = new DynamicResourceExtension(), new Uri("Coding\\Pages\\FPAProfileCodingPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 18, 5);
			EmptyStringToTrueConverter emptyStringToTrueConverter;
			VisualDiagnostics.RegisterSourceInfo(emptyStringToTrueConverter = new EmptyStringToTrueConverter(), new Uri("Coding\\Pages\\FPAProfileCodingPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 23, 14);
			EmptyStringToFalseConverter emptyStringToFalseConverter;
			VisualDiagnostics.RegisterSourceInfo(emptyStringToFalseConverter = new EmptyStringToFalseConverter(), new Uri("Coding\\Pages\\FPAProfileCodingPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 24, 14);
			VagCodingPlatformToTrueConverter vagCodingPlatformToTrueConverter;
			VisualDiagnostics.RegisterSourceInfo(vagCodingPlatformToTrueConverter = new VagCodingPlatformToTrueConverter(), new Uri("Coding\\Pages\\FPAProfileCodingPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 25, 14);
			ResourceDictionary resourceDictionary;
			VisualDiagnostics.RegisterSourceInfo(resourceDictionary = new ResourceDictionary(), new Uri("Coding\\Pages\\FPAProfileCodingPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 22, 10);
			Setter setter;
			VisualDiagnostics.RegisterSourceInfo(setter = new Setter(), new Uri("Coding\\Pages\\FPAProfileCodingPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 28, 14);
			DynamicResourceExtension dynamicResourceExtension2;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension2 = new DynamicResourceExtension(), new Uri("Coding\\Pages\\FPAProfileCodingPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 29, 48);
			Setter setter2;
			VisualDiagnostics.RegisterSourceInfo(setter2 = new Setter(), new Uri("Coding\\Pages\\FPAProfileCodingPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 29, 14);
			DynamicResourceExtension dynamicResourceExtension3;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension3 = new DynamicResourceExtension(), new Uri("Coding\\Pages\\FPAProfileCodingPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 30, 41);
			Setter setter3;
			VisualDiagnostics.RegisterSourceInfo(setter3 = new Setter(), new Uri("Coding\\Pages\\FPAProfileCodingPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 30, 14);
			Setter setter4;
			VisualDiagnostics.RegisterSourceInfo(setter4 = new Setter(), new Uri("Coding\\Pages\\FPAProfileCodingPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 31, 14);
			Style style;
			VisualDiagnostics.RegisterSourceInfo(style = new Style(typeof(LinkButton)), new Uri("Coding\\Pages\\FPAProfileCodingPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 27, 10);
			RowDefinition rowDefinition;
			VisualDiagnostics.RegisterSourceInfo(rowDefinition = new RowDefinition(), new Uri("Coding\\Pages\\FPAProfileCodingPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 37, 18);
			BindingExtension bindingExtension;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension = new BindingExtension(), new Uri("Coding\\Pages\\FPAProfileCodingPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 44, 25);
			DynamicResourceExtension dynamicResourceExtension4;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension4 = new DynamicResourceExtension(), new Uri("Coding\\Pages\\FPAProfileCodingPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 50, 37);
			BindingExtension bindingExtension2;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension2 = new BindingExtension(), new Uri("Coding\\Pages\\FPAProfileCodingPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 51, 37);
			Label label;
			VisualDiagnostics.RegisterSourceInfo(label = new Label(), new Uri("Coding\\Pages\\FPAProfileCodingPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 48, 34);
			StaticResourceExtension staticResourceExtension;
			VisualDiagnostics.RegisterSourceInfo(staticResourceExtension = new StaticResourceExtension(), new Uri("Coding\\Pages\\FPAProfileCodingPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 52, 40);
			BindingExtension bindingExtension3;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension3 = new BindingExtension(), new Uri("Coding\\Pages\\FPAProfileCodingPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 52, 40);
			BindingExtension bindingExtension4;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension4 = new BindingExtension(), new Uri("Coding\\Pages\\FPAProfileCodingPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 52, 130);
			Label label2;
			VisualDiagnostics.RegisterSourceInfo(label2 = new Label(), new Uri("Coding\\Pages\\FPAProfileCodingPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 52, 34);
			StaticResourceExtension staticResourceExtension2;
			VisualDiagnostics.RegisterSourceInfo(staticResourceExtension2 = new StaticResourceExtension(), new Uri("Coding\\Pages\\FPAProfileCodingPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 53, 40);
			BindingExtension bindingExtension5;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension5 = new BindingExtension(), new Uri("Coding\\Pages\\FPAProfileCodingPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 53, 40);
			BindingExtension bindingExtension6;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension6 = new BindingExtension(), new Uri("Coding\\Pages\\FPAProfileCodingPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 53, 148);
			Label label3;
			VisualDiagnostics.RegisterSourceInfo(label3 = new Label(), new Uri("Coding\\Pages\\FPAProfileCodingPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 53, 34);
			BindingExtension bindingExtension7;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension7 = new BindingExtension(), new Uri("Coding\\Pages\\FPAProfileCodingPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 55, 46);
			Translate translate2;
			VisualDiagnostics.RegisterSourceInfo(translate2 = new Translate(), new Uri("Coding\\Pages\\FPAProfileCodingPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 56, 44);
			Label label4;
			VisualDiagnostics.RegisterSourceInfo(label4 = new Label(), new Uri("Coding\\Pages\\FPAProfileCodingPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 56, 38);
			BindingExtension bindingExtension8;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension8 = new BindingExtension(), new Uri("Coding\\Pages\\FPAProfileCodingPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 57, 67);
			Entry entry;
			VisualDiagnostics.RegisterSourceInfo(entry = new Entry(), new Uri("Coding\\Pages\\FPAProfileCodingPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 57, 38);
			StaticResourceExtension staticResourceExtension3;
			VisualDiagnostics.RegisterSourceInfo(staticResourceExtension3 = new StaticResourceExtension(), new Uri("Coding\\Pages\\FPAProfileCodingPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 58, 68);
			BindingExtension bindingExtension9;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension9 = new BindingExtension(), new Uri("Coding\\Pages\\FPAProfileCodingPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 58, 68);
			Translate translate3;
			VisualDiagnostics.RegisterSourceInfo(translate3 = new Translate(), new Uri("Coding\\Pages\\FPAProfileCodingPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 61, 55);
			Span span;
			VisualDiagnostics.RegisterSourceInfo(span = new Span(), new Uri("Coding\\Pages\\FPAProfileCodingPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 61, 50);
			BindingExtension bindingExtension10;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension10 = new BindingExtension(), new Uri("Coding\\Pages\\FPAProfileCodingPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 62, 55);
			Span span2;
			VisualDiagnostics.RegisterSourceInfo(span2 = new Span(), new Uri("Coding\\Pages\\FPAProfileCodingPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 62, 50);
			FormattedString formattedString;
			VisualDiagnostics.RegisterSourceInfo(formattedString = new FormattedString(), new Uri("Coding\\Pages\\FPAProfileCodingPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 60, 46);
			Label label5;
			VisualDiagnostics.RegisterSourceInfo(label5 = new Label(), new Uri("Coding\\Pages\\FPAProfileCodingPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 58, 38);
			StaticResourceExtension staticResourceExtension4;
			VisualDiagnostics.RegisterSourceInfo(staticResourceExtension4 = new StaticResourceExtension(), new Uri("Coding\\Pages\\FPAProfileCodingPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 68, 41);
			BindingExtension bindingExtension11;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension11 = new BindingExtension(), new Uri("Coding\\Pages\\FPAProfileCodingPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 68, 41);
			Translate translate4;
			VisualDiagnostics.RegisterSourceInfo(translate4 = new Translate(), new Uri("Coding\\Pages\\FPAProfileCodingPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 69, 41);
			Label label6;
			VisualDiagnostics.RegisterSourceInfo(label6 = new Label(), new Uri("Coding\\Pages\\FPAProfileCodingPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 66, 38);
			StackLayout stackLayout;
			VisualDiagnostics.RegisterSourceInfo(stackLayout = new StackLayout(), new Uri("Coding\\Pages\\FPAProfileCodingPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 55, 34);
			BindingExtension bindingExtension12;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension12 = new BindingExtension(), new Uri("Coding\\Pages\\FPAProfileCodingPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 72, 60);
			Span span3;
			VisualDiagnostics.RegisterSourceInfo(span3 = new Span(), new Uri("Coding\\Pages\\FPAProfileCodingPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 75, 46);
			Span span4;
			VisualDiagnostics.RegisterSourceInfo(span4 = new Span(), new Uri("Coding\\Pages\\FPAProfileCodingPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 76, 46);
			BindingExtension bindingExtension13;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension13 = new BindingExtension(), new Uri("Coding\\Pages\\FPAProfileCodingPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 77, 73);
			Span span5;
			VisualDiagnostics.RegisterSourceInfo(span5 = new Span(), new Uri("Coding\\Pages\\FPAProfileCodingPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 77, 46);
			FormattedString formattedString2;
			VisualDiagnostics.RegisterSourceInfo(formattedString2 = new FormattedString(), new Uri("Coding\\Pages\\FPAProfileCodingPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 74, 42);
			Label label7;
			VisualDiagnostics.RegisterSourceInfo(label7 = new Label(), new Uri("Coding\\Pages\\FPAProfileCodingPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 72, 34);
			BindingExtension bindingExtension14;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension14 = new BindingExtension(), new Uri("Coding\\Pages\\FPAProfileCodingPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 84, 37);
			Button button;
			VisualDiagnostics.RegisterSourceInfo(button = new Button(), new Uri("Coding\\Pages\\FPAProfileCodingPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 81, 34);
			Translate translate5;
			VisualDiagnostics.RegisterSourceInfo(translate5 = new Translate(), new Uri("Coding\\Pages\\FPAProfileCodingPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 89, 37);
			Button button2;
			VisualDiagnostics.RegisterSourceInfo(button2 = new Button(), new Uri("Coding\\Pages\\FPAProfileCodingPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 86, 34);
			Label label8;
			VisualDiagnostics.RegisterSourceInfo(label8 = new Label(), new Uri("Coding\\Pages\\FPAProfileCodingPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 90, 34);
			BindingExtension bindingExtension15;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension15 = new BindingExtension(), new Uri("Coding\\Pages\\FPAProfileCodingPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 92, 37);
			BindingExtension bindingExtension16;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension16 = new BindingExtension(), new Uri("Coding\\Pages\\FPAProfileCodingPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 94, 37);
			LinkButton linkButton;
			VisualDiagnostics.RegisterSourceInfo(linkButton = new LinkButton(), new Uri("Coding\\Pages\\FPAProfileCodingPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 91, 34);
			BindingExtension bindingExtension17;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension17 = new BindingExtension(), new Uri("Coding\\Pages\\FPAProfileCodingPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 96, 37);
			BindingExtension bindingExtension18;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension18 = new BindingExtension(), new Uri("Coding\\Pages\\FPAProfileCodingPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 98, 37);
			LinkButton linkButton2;
			VisualDiagnostics.RegisterSourceInfo(linkButton2 = new LinkButton(), new Uri("Coding\\Pages\\FPAProfileCodingPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 95, 34);
			BindingExtension bindingExtension19;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension19 = new BindingExtension(), new Uri("Coding\\Pages\\FPAProfileCodingPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 100, 37);
			BindingExtension bindingExtension20;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension20 = new BindingExtension(), new Uri("Coding\\Pages\\FPAProfileCodingPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 102, 37);
			LinkButton linkButton3;
			VisualDiagnostics.RegisterSourceInfo(linkButton3 = new LinkButton(), new Uri("Coding\\Pages\\FPAProfileCodingPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 99, 34);
			BindingExtension bindingExtension21;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension21 = new BindingExtension(), new Uri("Coding\\Pages\\FPAProfileCodingPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 104, 37);
			BindingExtension bindingExtension22;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension22 = new BindingExtension(), new Uri("Coding\\Pages\\FPAProfileCodingPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 106, 37);
			LinkButton linkButton4;
			VisualDiagnostics.RegisterSourceInfo(linkButton4 = new LinkButton(), new Uri("Coding\\Pages\\FPAProfileCodingPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 103, 34);
			BindingExtension bindingExtension23;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension23 = new BindingExtension(), new Uri("Coding\\Pages\\FPAProfileCodingPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 108, 37);
			BindingExtension bindingExtension24;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension24 = new BindingExtension(), new Uri("Coding\\Pages\\FPAProfileCodingPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 110, 37);
			LinkButton linkButton5;
			VisualDiagnostics.RegisterSourceInfo(linkButton5 = new LinkButton(), new Uri("Coding\\Pages\\FPAProfileCodingPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 107, 34);
			BindingExtension bindingExtension25;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension25 = new BindingExtension(), new Uri("Coding\\Pages\\FPAProfileCodingPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 112, 37);
			BindingExtension bindingExtension26;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension26 = new BindingExtension(), new Uri("Coding\\Pages\\FPAProfileCodingPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 114, 37);
			LinkButton linkButton6;
			VisualDiagnostics.RegisterSourceInfo(linkButton6 = new LinkButton(), new Uri("Coding\\Pages\\FPAProfileCodingPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 111, 34);
			BindingExtension bindingExtension27;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension27 = new BindingExtension(), new Uri("Coding\\Pages\\FPAProfileCodingPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 116, 37);
			BindingExtension bindingExtension28;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension28 = new BindingExtension(), new Uri("Coding\\Pages\\FPAProfileCodingPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 118, 37);
			LinkButton linkButton7;
			VisualDiagnostics.RegisterSourceInfo(linkButton7 = new LinkButton(), new Uri("Coding\\Pages\\FPAProfileCodingPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 115, 34);
			BindingExtension bindingExtension29;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension29 = new BindingExtension(), new Uri("Coding\\Pages\\FPAProfileCodingPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 120, 37);
			BindingExtension bindingExtension30;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension30 = new BindingExtension(), new Uri("Coding\\Pages\\FPAProfileCodingPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 122, 37);
			LinkButton linkButton8;
			VisualDiagnostics.RegisterSourceInfo(linkButton8 = new LinkButton(), new Uri("Coding\\Pages\\FPAProfileCodingPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 119, 34);
			BindingExtension bindingExtension31;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension31 = new BindingExtension(), new Uri("Coding\\Pages\\FPAProfileCodingPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 124, 37);
			BindingExtension bindingExtension32;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension32 = new BindingExtension(), new Uri("Coding\\Pages\\FPAProfileCodingPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 126, 37);
			LinkButton linkButton9;
			VisualDiagnostics.RegisterSourceInfo(linkButton9 = new LinkButton(), new Uri("Coding\\Pages\\FPAProfileCodingPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 123, 34);
			BindingExtension bindingExtension33;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension33 = new BindingExtension(), new Uri("Coding\\Pages\\FPAProfileCodingPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 128, 37);
			BindingExtension bindingExtension34;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension34 = new BindingExtension(), new Uri("Coding\\Pages\\FPAProfileCodingPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 130, 37);
			LinkButton linkButton10;
			VisualDiagnostics.RegisterSourceInfo(linkButton10 = new LinkButton(), new Uri("Coding\\Pages\\FPAProfileCodingPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 127, 34);
			BindingExtension bindingExtension35;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension35 = new BindingExtension(), new Uri("Coding\\Pages\\FPAProfileCodingPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 132, 37);
			BindingExtension bindingExtension36;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension36 = new BindingExtension(), new Uri("Coding\\Pages\\FPAProfileCodingPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 134, 37);
			LinkButton linkButton11;
			VisualDiagnostics.RegisterSourceInfo(linkButton11 = new LinkButton(), new Uri("Coding\\Pages\\FPAProfileCodingPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 131, 34);
			BindingExtension bindingExtension37;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension37 = new BindingExtension(), new Uri("Coding\\Pages\\FPAProfileCodingPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 136, 37);
			BindingExtension bindingExtension38;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension38 = new BindingExtension(), new Uri("Coding\\Pages\\FPAProfileCodingPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 138, 37);
			LinkButton linkButton12;
			VisualDiagnostics.RegisterSourceInfo(linkButton12 = new LinkButton(), new Uri("Coding\\Pages\\FPAProfileCodingPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 135, 34);
			Label label9;
			VisualDiagnostics.RegisterSourceInfo(label9 = new Label(), new Uri("Coding\\Pages\\FPAProfileCodingPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 140, 34);
			StackLayout stackLayout2;
			VisualDiagnostics.RegisterSourceInfo(stackLayout2 = new StackLayout(), new Uri("Coding\\Pages\\FPAProfileCodingPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 47, 30);
			DataTemplate dataTemplate;
			VisualDiagnostics.RegisterSourceInfo(dataTemplate = new DataTemplate(), new Uri("Coding\\Pages\\FPAProfileCodingPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 144, 30);
			ListView listView;
			VisualDiagnostics.RegisterSourceInfo(listView = new ListView(), new Uri("Coding\\Pages\\FPAProfileCodingPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 41, 22);
			StackLayout stackLayout3;
			VisualDiagnostics.RegisterSourceInfo(stackLayout3 = new StackLayout(), new Uri("Coding\\Pages\\FPAProfileCodingPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 40, 18);
			ScrollView scrollView;
			VisualDiagnostics.RegisterSourceInfo(scrollView = new ScrollView(), new Uri("Coding\\Pages\\FPAProfileCodingPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 39, 14);
			ActivityFrame activityFrame;
			VisualDiagnostics.RegisterSourceInfo(activityFrame = new ActivityFrame(), new Uri("Coding\\Pages\\FPAProfileCodingPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 166, 14);
			Grid grid;
			VisualDiagnostics.RegisterSourceInfo(grid = new Grid(), new Uri("Coding\\Pages\\FPAProfileCodingPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 35, 10);
			VisualDiagnostics.RegisterSourceInfo(this, new Uri("Coding\\Pages\\FPAProfileCodingPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 2, 2);
			NameScope nameScope = NameScope.GetNameScope(this) ?? new NameScope();
			NameScope.SetNameScope(this, nameScope);
			NameScope nameScope2 = new NameScope();
			NameScope nameScope3 = new NameScope();
			NameScope nameScope4 = new NameScope();
			NameScope nameScope5 = new NameScope();
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
			nameScope.RegisterName("labelState", label7);
			if (label7.StyleId == null)
			{
				label7.StyleId = "labelState";
			}
			nameScope.RegisterName("btnChooseVersion", button);
			if (button.StyleId == null)
			{
				button.StyleId = "btnChooseVersion";
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
			this.lv = listView;
			this.entryPassword = entry;
			this.labelState = label7;
			this.btnChooseVersion = button;
			this.btnSaveNewValue = button2;
			this.activityFrame = activityFrame;
			resourceDictionary.Add("EmptyStringToTrueConverter", emptyStringToTrueConverter);
			resourceDictionary.Add("EmptyStringToFalseConverter", emptyStringToFalseConverter);
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
			xmlNamespaceResolver.Add("syncfusion", "clr-namespace:Syncfusion.XForms.Buttons;assembly=Syncfusion.Buttons.XForms");
			xmlNamespaceResolver.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider.Add(typeFromHandle2, new XamlTypeResolver(xmlNamespaceResolver, typeof(FPAProfileCodingPage).GetTypeInfo().Assembly));
			xamlServiceProvider.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(15, 5)));
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
			xmlNamespaceResolver2.Add("syncfusion", "clr-namespace:Syncfusion.XForms.Buttons;assembly=Syncfusion.Buttons.XForms");
			xmlNamespaceResolver2.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider2.Add(typeFromHandle4, new XamlTypeResolver(xmlNamespaceResolver2, typeof(FPAProfileCodingPage).GetTypeInfo().Assembly));
			xamlServiceProvider2.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(18, 5)));
			DynamicResource dynamicResource = markupExtension2.ProvideValue(xamlServiceProvider2);
			this.SetDynamicResource(VisualElement.BackgroundColorProperty, dynamicResource.Key);
			this.Resources.Add(resourceDictionary);
			setter.Property = Button.BorderColorProperty;
			setter.Value = "Transparent";
			setter.Value = Color.Transparent;
			style.Setters.Add(setter);
			setter2.Property = VisualElement.BackgroundColorProperty;
			dynamicResourceExtension2.Key = "BackgroundColor";
			IMarkupExtension<DynamicResource> markupExtension3 = dynamicResourceExtension2;
			XamlServiceProvider xamlServiceProvider3 = new XamlServiceProvider();
			Type typeFromHandle5 = typeof(IProvideValueTarget);
			object[] array3 = new object[0 + 3];
			array3[0] = setter2;
			array3[1] = style;
			array3[2] = this;
			object obj4;
			xamlServiceProvider3.Add(typeFromHandle5, obj4 = new SimpleValueTargetProvider(array3, typeof(Setter).GetRuntimeProperty("Value"), nameScope3));
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
			xmlNamespaceResolver3.Add("syncfusion", "clr-namespace:Syncfusion.XForms.Buttons;assembly=Syncfusion.Buttons.XForms");
			xmlNamespaceResolver3.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider3.Add(typeFromHandle6, new XamlTypeResolver(xmlNamespaceResolver3, typeof(FPAProfileCodingPage).GetTypeInfo().Assembly));
			xamlServiceProvider3.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(29, 48)));
			DynamicResource dynamicResource2 = markupExtension3.ProvideValue(xamlServiceProvider3);
			setter2.Value = dynamicResource2;
			style.Setters.Add(setter2);
			setter3.Property = Button.FontSizeProperty;
			dynamicResourceExtension3.Key = "BaseFontSize+";
			IMarkupExtension<DynamicResource> markupExtension4 = dynamicResourceExtension3;
			XamlServiceProvider xamlServiceProvider4 = new XamlServiceProvider();
			Type typeFromHandle7 = typeof(IProvideValueTarget);
			object[] array4 = new object[0 + 3];
			array4[0] = setter3;
			array4[1] = style;
			array4[2] = this;
			object obj5;
			xamlServiceProvider4.Add(typeFromHandle7, obj5 = new SimpleValueTargetProvider(array4, typeof(Setter).GetRuntimeProperty("Value"), nameScope4));
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
			xmlNamespaceResolver4.Add("syncfusion", "clr-namespace:Syncfusion.XForms.Buttons;assembly=Syncfusion.Buttons.XForms");
			xmlNamespaceResolver4.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider4.Add(typeFromHandle8, new XamlTypeResolver(xmlNamespaceResolver4, typeof(FPAProfileCodingPage).GetTypeInfo().Assembly));
			xamlServiceProvider4.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(30, 41)));
			DynamicResource dynamicResource3 = markupExtension4.ProvideValue(xamlServiceProvider4);
			setter3.Value = dynamicResource3;
			style.Setters.Add(setter3);
			setter4.Property = View.HorizontalOptionsProperty;
			setter4.Value = "Start";
			setter4.Value = LayoutOptions.Start;
			style.Setters.Add(setter4);
			this.Resources.Add(style);
			rowDefinition.SetValue(RowDefinition.HeightProperty, new GridLengthTypeConverter().ConvertFromInvariantString("*"));
			grid.GetValue(Grid.RowDefinitionsProperty).Add(rowDefinition);
			scrollView.SetValue(Grid.RowProperty, 0);
			scrollView.SetValue(ScrollView.OrientationProperty, 0);
			stackLayout3.SetValue(StackLayout.OrientationProperty, 0);
			listView.SetValue(Grid.RowProperty, 0);
			bindingExtension.Path = "Model.Controls";
			BindingBase bindingBase = bindingExtension.ProvideValue(null);
			listView.SetBinding(ItemsView<Cell>.ItemsSourceProperty, bindingBase);
			listView.SetValue(ListView.SelectionModeProperty, 0);
			stackLayout2.SetValue(StackLayout.OrientationProperty, 0);
			label.SetValue(Label.FontAttributesProperty, new FontAttributesConverter().ConvertFromInvariantString("Bold"));
			dynamicResourceExtension4.Key = "BaseFontSize+++";
			IMarkupExtension<DynamicResource> markupExtension5 = dynamicResourceExtension4;
			XamlServiceProvider xamlServiceProvider5 = new XamlServiceProvider();
			Type typeFromHandle9 = typeof(IProvideValueTarget);
			object[] array5 = new object[0 + 7];
			array5[0] = label;
			array5[1] = stackLayout2;
			array5[2] = listView;
			array5[3] = stackLayout3;
			array5[4] = scrollView;
			array5[5] = grid;
			array5[6] = this;
			object obj6;
			xamlServiceProvider5.Add(typeFromHandle9, obj6 = new SimpleValueTargetProvider(array5, Label.FontSizeProperty, nameScope));
			xamlServiceProvider5.Add(typeof(IReferenceProvider), obj6);
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
			xmlNamespaceResolver5.Add("syncfusion", "clr-namespace:Syncfusion.XForms.Buttons;assembly=Syncfusion.Buttons.XForms");
			xmlNamespaceResolver5.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider5.Add(typeFromHandle10, new XamlTypeResolver(xmlNamespaceResolver5, typeof(FPAProfileCodingPage).GetTypeInfo().Assembly));
			xamlServiceProvider5.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(50, 37)));
			DynamicResource dynamicResource4 = markupExtension5.ProvideValue(xamlServiceProvider5);
			label.SetDynamicResource(Label.FontSizeProperty, dynamicResource4.Key);
			bindingExtension2.Path = "Name";
			BindingBase bindingBase2 = bindingExtension2.ProvideValue(null);
			label.SetBinding(Label.TextProperty, bindingBase2);
			stackLayout2.Children.Add(label);
			staticResourceExtension.Key = "EmptyStringToFalseConverter";
			IMarkupExtension markupExtension6 = staticResourceExtension;
			XamlServiceProvider xamlServiceProvider6 = new XamlServiceProvider();
			Type typeFromHandle11 = typeof(IProvideValueTarget);
			object[] array6 = new object[0 + 8];
			array6[0] = bindingExtension3;
			array6[1] = label2;
			array6[2] = stackLayout2;
			array6[3] = listView;
			array6[4] = stackLayout3;
			array6[5] = scrollView;
			array6[6] = grid;
			array6[7] = this;
			object obj7;
			xamlServiceProvider6.Add(typeFromHandle11, obj7 = new SimpleValueTargetProvider(array6, typeof(BindingExtension).GetRuntimeProperty("Converter"), nameScope));
			xamlServiceProvider6.Add(typeof(IReferenceProvider), obj7);
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
			xmlNamespaceResolver6.Add("syncfusion", "clr-namespace:Syncfusion.XForms.Buttons;assembly=Syncfusion.Buttons.XForms");
			xmlNamespaceResolver6.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider6.Add(typeFromHandle12, new XamlTypeResolver(xmlNamespaceResolver6, typeof(FPAProfileCodingPage).GetTypeInfo().Assembly));
			xamlServiceProvider6.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(52, 40)));
			object obj8 = markupExtension6.ProvideValue(xamlServiceProvider6);
			bindingExtension3.Converter = obj8;
			bindingExtension3.Path = "Description";
			BindingBase bindingBase3 = bindingExtension3.ProvideValue(null);
			label2.SetBinding(VisualElement.IsVisibleProperty, bindingBase3);
			bindingExtension4.Path = "Description";
			BindingBase bindingBase4 = bindingExtension4.ProvideValue(null);
			label2.SetBinding(Label.TextProperty, bindingBase4);
			stackLayout2.Children.Add(label2);
			bindingExtension5.Mode = 2;
			staticResourceExtension2.Key = "EmptyStringToFalseConverter";
			IMarkupExtension markupExtension7 = staticResourceExtension2;
			XamlServiceProvider xamlServiceProvider7 = new XamlServiceProvider();
			Type typeFromHandle13 = typeof(IProvideValueTarget);
			object[] array7 = new object[0 + 8];
			array7[0] = bindingExtension5;
			array7[1] = label3;
			array7[2] = stackLayout2;
			array7[3] = listView;
			array7[4] = stackLayout3;
			array7[5] = scrollView;
			array7[6] = grid;
			array7[7] = this;
			object obj9;
			xamlServiceProvider7.Add(typeFromHandle13, obj9 = new SimpleValueTargetProvider(array7, typeof(BindingExtension).GetRuntimeProperty("Converter"), nameScope));
			xamlServiceProvider7.Add(typeof(IReferenceProvider), obj9);
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
			xmlNamespaceResolver7.Add("syncfusion", "clr-namespace:Syncfusion.XForms.Buttons;assembly=Syncfusion.Buttons.XForms");
			xmlNamespaceResolver7.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider7.Add(typeFromHandle14, new XamlTypeResolver(xmlNamespaceResolver7, typeof(FPAProfileCodingPage).GetTypeInfo().Assembly));
			xamlServiceProvider7.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(53, 40)));
			object obj10 = markupExtension7.ProvideValue(xamlServiceProvider7);
			bindingExtension5.Converter = obj10;
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
			IMarkupExtension markupExtension8 = translate2;
			XamlServiceProvider xamlServiceProvider8 = new XamlServiceProvider();
			Type typeFromHandle15 = typeof(IProvideValueTarget);
			object[] array8 = new object[0 + 8];
			array8[0] = label4;
			array8[1] = stackLayout;
			array8[2] = stackLayout2;
			array8[3] = listView;
			array8[4] = stackLayout3;
			array8[5] = scrollView;
			array8[6] = grid;
			array8[7] = this;
			object obj11;
			xamlServiceProvider8.Add(typeFromHandle15, obj11 = new SimpleValueTargetProvider(array8, Label.TextProperty, nameScope));
			xamlServiceProvider8.Add(typeof(IReferenceProvider), obj11);
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
			xmlNamespaceResolver8.Add("syncfusion", "clr-namespace:Syncfusion.XForms.Buttons;assembly=Syncfusion.Buttons.XForms");
			xmlNamespaceResolver8.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider8.Add(typeFromHandle16, new XamlTypeResolver(xmlNamespaceResolver8, typeof(FPAProfileCodingPage).GetTypeInfo().Assembly));
			xamlServiceProvider8.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(56, 44)));
			object obj12 = markupExtension8.ProvideValue(xamlServiceProvider8);
			label4.Text = obj12;
			stackLayout.Children.Add(label4);
			bindingExtension8.Mode = 1;
			bindingExtension8.Path = "Password";
			BindingBase bindingBase8 = bindingExtension8.ProvideValue(null);
			entry.SetBinding(Entry.TextProperty, bindingBase8);
			stackLayout.Children.Add(entry);
			label5.SetValue(Label.FontAttributesProperty, new FontAttributesConverter().ConvertFromInvariantString("Italic"));
			staticResourceExtension3.Key = "EmptyStringToFalseConverter";
			IMarkupExtension markupExtension9 = staticResourceExtension3;
			XamlServiceProvider xamlServiceProvider9 = new XamlServiceProvider();
			Type typeFromHandle17 = typeof(IProvideValueTarget);
			object[] array9 = new object[0 + 9];
			array9[0] = bindingExtension9;
			array9[1] = label5;
			array9[2] = stackLayout;
			array9[3] = stackLayout2;
			array9[4] = listView;
			array9[5] = stackLayout3;
			array9[6] = scrollView;
			array9[7] = grid;
			array9[8] = this;
			object obj13;
			xamlServiceProvider9.Add(typeFromHandle17, obj13 = new SimpleValueTargetProvider(array9, typeof(BindingExtension).GetRuntimeProperty("Converter"), nameScope));
			xamlServiceProvider9.Add(typeof(IReferenceProvider), obj13);
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
			xmlNamespaceResolver9.Add("syncfusion", "clr-namespace:Syncfusion.XForms.Buttons;assembly=Syncfusion.Buttons.XForms");
			xmlNamespaceResolver9.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider9.Add(typeFromHandle18, new XamlTypeResolver(xmlNamespaceResolver9, typeof(FPAProfileCodingPage).GetTypeInfo().Assembly));
			xamlServiceProvider9.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(58, 68)));
			object obj14 = markupExtension9.ProvideValue(xamlServiceProvider9);
			bindingExtension9.Converter = obj14;
			bindingExtension9.Path = "PasswordHint";
			BindingBase bindingBase9 = bindingExtension9.ProvideValue(null);
			label5.SetBinding(VisualElement.IsVisibleProperty, bindingBase9);
			translate3.Text = "coding_PasswordHint";
			IMarkupExtension markupExtension10 = translate3;
			XamlServiceProvider xamlServiceProvider10 = new XamlServiceProvider();
			Type typeFromHandle19 = typeof(IProvideValueTarget);
			object[] array10 = new object[0 + 10];
			array10[0] = span;
			array10[1] = formattedString;
			array10[2] = label5;
			array10[3] = stackLayout;
			array10[4] = stackLayout2;
			array10[5] = listView;
			array10[6] = stackLayout3;
			array10[7] = scrollView;
			array10[8] = grid;
			array10[9] = this;
			object obj15;
			xamlServiceProvider10.Add(typeFromHandle19, obj15 = new SimpleValueTargetProvider(array10, Span.TextProperty, nameScope));
			xamlServiceProvider10.Add(typeof(IReferenceProvider), obj15);
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
			xmlNamespaceResolver10.Add("syncfusion", "clr-namespace:Syncfusion.XForms.Buttons;assembly=Syncfusion.Buttons.XForms");
			xmlNamespaceResolver10.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider10.Add(typeFromHandle20, new XamlTypeResolver(xmlNamespaceResolver10, typeof(FPAProfileCodingPage).GetTypeInfo().Assembly));
			xamlServiceProvider10.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(61, 55)));
			object obj16 = markupExtension10.ProvideValue(xamlServiceProvider10);
			span.Text = obj16;
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
			IMarkupExtension markupExtension11 = staticResourceExtension4;
			XamlServiceProvider xamlServiceProvider11 = new XamlServiceProvider();
			Type typeFromHandle21 = typeof(IProvideValueTarget);
			object[] array11 = new object[0 + 9];
			array11[0] = bindingExtension11;
			array11[1] = label6;
			array11[2] = stackLayout;
			array11[3] = stackLayout2;
			array11[4] = listView;
			array11[5] = stackLayout3;
			array11[6] = scrollView;
			array11[7] = grid;
			array11[8] = this;
			object obj17;
			xamlServiceProvider11.Add(typeFromHandle21, obj17 = new SimpleValueTargetProvider(array11, typeof(BindingExtension).GetRuntimeProperty("Converter"), nameScope));
			xamlServiceProvider11.Add(typeof(IReferenceProvider), obj17);
			Type typeFromHandle22 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver11 = new XmlNamespaceResolver();
			xmlNamespaceResolver11.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver11.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver11.Add("coding", "clr-namespace:CarScannerXamarinForms.Coding");
			xmlNamespaceResolver11.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver11.Add("d", "http://xamarin.com/schemas/2014/forms/design");
			xmlNamespaceResolver11.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver11.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver11.Add("mc", "http://schemas.openxmlformats.org/markup-compatibility/2006");
			xmlNamespaceResolver11.Add("settings", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver11.Add("syncfusion", "clr-namespace:Syncfusion.XForms.Buttons;assembly=Syncfusion.Buttons.XForms");
			xmlNamespaceResolver11.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider11.Add(typeFromHandle22, new XamlTypeResolver(xmlNamespaceResolver11, typeof(FPAProfileCodingPage).GetTypeInfo().Assembly));
			xamlServiceProvider11.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(68, 41)));
			object obj18 = markupExtension11.ProvideValue(xamlServiceProvider11);
			bindingExtension11.Converter = obj18;
			bindingExtension11.Path = "Password";
			BindingBase bindingBase11 = bindingExtension11.ProvideValue(null);
			label6.SetBinding(VisualElement.IsVisibleProperty, bindingBase11);
			translate4.Text = "coding_PasswordNotRequired";
			IMarkupExtension markupExtension12 = translate4;
			XamlServiceProvider xamlServiceProvider12 = new XamlServiceProvider();
			Type typeFromHandle23 = typeof(IProvideValueTarget);
			object[] array12 = new object[0 + 8];
			array12[0] = label6;
			array12[1] = stackLayout;
			array12[2] = stackLayout2;
			array12[3] = listView;
			array12[4] = stackLayout3;
			array12[5] = scrollView;
			array12[6] = grid;
			array12[7] = this;
			object obj19;
			xamlServiceProvider12.Add(typeFromHandle23, obj19 = new SimpleValueTargetProvider(array12, Label.TextProperty, nameScope));
			xamlServiceProvider12.Add(typeof(IReferenceProvider), obj19);
			Type typeFromHandle24 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver12 = new XmlNamespaceResolver();
			xmlNamespaceResolver12.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver12.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver12.Add("coding", "clr-namespace:CarScannerXamarinForms.Coding");
			xmlNamespaceResolver12.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver12.Add("d", "http://xamarin.com/schemas/2014/forms/design");
			xmlNamespaceResolver12.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver12.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver12.Add("mc", "http://schemas.openxmlformats.org/markup-compatibility/2006");
			xmlNamespaceResolver12.Add("settings", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver12.Add("syncfusion", "clr-namespace:Syncfusion.XForms.Buttons;assembly=Syncfusion.Buttons.XForms");
			xmlNamespaceResolver12.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider12.Add(typeFromHandle24, new XamlTypeResolver(xmlNamespaceResolver12, typeof(FPAProfileCodingPage).GetTypeInfo().Assembly));
			xamlServiceProvider12.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(69, 41)));
			object obj20 = markupExtension12.ProvideValue(xamlServiceProvider12);
			label6.Text = obj20;
			stackLayout.Children.Add(label6);
			stackLayout2.Children.Add(stackLayout);
			bindingExtension12.Path = "HasCurrentState";
			BindingBase bindingBase12 = bindingExtension12.ProvideValue(null);
			label7.SetBinding(VisualElement.IsVisibleProperty, bindingBase12);
			span3.SetValue(Span.TextProperty, "Dataset version:");
			formattedString2.Spans.Add(span3);
			span4.SetValue(Span.TextProperty, " ");
			formattedString2.Spans.Add(span4);
			span5.SetValue(Span.FontAttributesProperty, new FontAttributesConverter().ConvertFromInvariantString("Bold"));
			bindingExtension13.Mode = 2;
			bindingExtension13.Path = "CurrentState";
			BindingBase bindingBase13 = bindingExtension13.ProvideValue(null);
			span5.SetBinding(Span.TextProperty, bindingBase13);
			formattedString2.Spans.Add(span5);
			label7.SetValue(Label.FormattedTextProperty, formattedString2);
			stackLayout2.Children.Add(label7);
			button.Clicked += this.btnChooseOtherVersion_Clicked;
			bindingExtension14.Path = "HasCurrentState";
			BindingBase bindingBase14 = bindingExtension14.ProvideValue(null);
			button.SetBinding(VisualElement.IsVisibleProperty, bindingBase14);
			button.SetValue(Button.TextProperty, "Choose other version");
			stackLayout2.Children.Add(button);
			button2.Clicked += this.btnSaveState_Clicked;
			translate5.Text = "coding_Apply";
			IMarkupExtension markupExtension13 = translate5;
			XamlServiceProvider xamlServiceProvider13 = new XamlServiceProvider();
			Type typeFromHandle25 = typeof(IProvideValueTarget);
			object[] array13 = new object[0 + 7];
			array13[0] = button2;
			array13[1] = stackLayout2;
			array13[2] = listView;
			array13[3] = stackLayout3;
			array13[4] = scrollView;
			array13[5] = grid;
			array13[6] = this;
			object obj21;
			xamlServiceProvider13.Add(typeFromHandle25, obj21 = new SimpleValueTargetProvider(array13, Button.TextProperty, nameScope));
			xamlServiceProvider13.Add(typeof(IReferenceProvider), obj21);
			Type typeFromHandle26 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver13 = new XmlNamespaceResolver();
			xmlNamespaceResolver13.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver13.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver13.Add("coding", "clr-namespace:CarScannerXamarinForms.Coding");
			xmlNamespaceResolver13.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver13.Add("d", "http://xamarin.com/schemas/2014/forms/design");
			xmlNamespaceResolver13.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver13.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver13.Add("mc", "http://schemas.openxmlformats.org/markup-compatibility/2006");
			xmlNamespaceResolver13.Add("settings", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver13.Add("syncfusion", "clr-namespace:Syncfusion.XForms.Buttons;assembly=Syncfusion.Buttons.XForms");
			xmlNamespaceResolver13.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider13.Add(typeFromHandle26, new XamlTypeResolver(xmlNamespaceResolver13, typeof(FPAProfileCodingPage).GetTypeInfo().Assembly));
			xamlServiceProvider13.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(89, 37)));
			object obj22 = markupExtension13.ProvideValue(xamlServiceProvider13);
			button2.Text = obj22;
			stackLayout2.Children.Add(button2);
			label8.SetValue(Label.FontAttributesProperty, new FontAttributesConverter().ConvertFromInvariantString("Bold"));
			label8.SetValue(Label.TextProperty, "Profiles:");
			stackLayout2.Children.Add(label8);
			bindingExtension15.Path = "Model.Profile1";
			BindingBase bindingBase15 = bindingExtension15.ProvideValue(null);
			linkButton.SetBinding(BindableObject.BindingContextProperty, bindingBase15);
			linkButton.Clicked += this.btnProfile_Clicked;
			bindingExtension16.Path = "IndexAndTitle";
			BindingBase bindingBase16 = bindingExtension16.ProvideValue(null);
			linkButton.SetBinding(Button.TextProperty, bindingBase16);
			stackLayout2.Children.Add(linkButton);
			bindingExtension17.Path = "Model.Profile2";
			BindingBase bindingBase17 = bindingExtension17.ProvideValue(null);
			linkButton2.SetBinding(BindableObject.BindingContextProperty, bindingBase17);
			linkButton2.Clicked += this.btnProfile_Clicked;
			bindingExtension18.Path = "IndexAndTitle";
			BindingBase bindingBase18 = bindingExtension18.ProvideValue(null);
			linkButton2.SetBinding(Button.TextProperty, bindingBase18);
			stackLayout2.Children.Add(linkButton2);
			bindingExtension19.Path = "Model.Profile3";
			BindingBase bindingBase19 = bindingExtension19.ProvideValue(null);
			linkButton3.SetBinding(BindableObject.BindingContextProperty, bindingBase19);
			linkButton3.Clicked += this.btnProfile_Clicked;
			bindingExtension20.Path = "IndexAndTitle";
			BindingBase bindingBase20 = bindingExtension20.ProvideValue(null);
			linkButton3.SetBinding(Button.TextProperty, bindingBase20);
			stackLayout2.Children.Add(linkButton3);
			bindingExtension21.Path = "Model.Profile4";
			BindingBase bindingBase21 = bindingExtension21.ProvideValue(null);
			linkButton4.SetBinding(BindableObject.BindingContextProperty, bindingBase21);
			linkButton4.Clicked += this.btnProfile_Clicked;
			bindingExtension22.Path = "IndexAndTitle";
			BindingBase bindingBase22 = bindingExtension22.ProvideValue(null);
			linkButton4.SetBinding(Button.TextProperty, bindingBase22);
			stackLayout2.Children.Add(linkButton4);
			bindingExtension23.Path = "Model.Profile5";
			BindingBase bindingBase23 = bindingExtension23.ProvideValue(null);
			linkButton5.SetBinding(BindableObject.BindingContextProperty, bindingBase23);
			linkButton5.Clicked += this.btnProfile_Clicked;
			bindingExtension24.Path = "IndexAndTitle";
			BindingBase bindingBase24 = bindingExtension24.ProvideValue(null);
			linkButton5.SetBinding(Button.TextProperty, bindingBase24);
			stackLayout2.Children.Add(linkButton5);
			bindingExtension25.Path = "Model.Profile6";
			BindingBase bindingBase25 = bindingExtension25.ProvideValue(null);
			linkButton6.SetBinding(BindableObject.BindingContextProperty, bindingBase25);
			linkButton6.Clicked += this.btnProfile_Clicked;
			bindingExtension26.Path = "IndexAndTitle";
			BindingBase bindingBase26 = bindingExtension26.ProvideValue(null);
			linkButton6.SetBinding(Button.TextProperty, bindingBase26);
			stackLayout2.Children.Add(linkButton6);
			bindingExtension27.Path = "Model.Profile7";
			BindingBase bindingBase27 = bindingExtension27.ProvideValue(null);
			linkButton7.SetBinding(BindableObject.BindingContextProperty, bindingBase27);
			linkButton7.Clicked += this.btnProfile_Clicked;
			bindingExtension28.Path = "IndexAndTitle";
			BindingBase bindingBase28 = bindingExtension28.ProvideValue(null);
			linkButton7.SetBinding(Button.TextProperty, bindingBase28);
			stackLayout2.Children.Add(linkButton7);
			bindingExtension29.Path = "Model.Profile8";
			BindingBase bindingBase29 = bindingExtension29.ProvideValue(null);
			linkButton8.SetBinding(BindableObject.BindingContextProperty, bindingBase29);
			linkButton8.Clicked += this.btnProfile_Clicked;
			bindingExtension30.Path = "IndexAndTitle";
			BindingBase bindingBase30 = bindingExtension30.ProvideValue(null);
			linkButton8.SetBinding(Button.TextProperty, bindingBase30);
			stackLayout2.Children.Add(linkButton8);
			bindingExtension31.Path = "Model.Profile9";
			BindingBase bindingBase31 = bindingExtension31.ProvideValue(null);
			linkButton9.SetBinding(BindableObject.BindingContextProperty, bindingBase31);
			linkButton9.Clicked += this.btnProfile_Clicked;
			bindingExtension32.Path = "IndexAndTitle";
			BindingBase bindingBase32 = bindingExtension32.ProvideValue(null);
			linkButton9.SetBinding(Button.TextProperty, bindingBase32);
			stackLayout2.Children.Add(linkButton9);
			bindingExtension33.Path = "Model.Profile10";
			BindingBase bindingBase33 = bindingExtension33.ProvideValue(null);
			linkButton10.SetBinding(BindableObject.BindingContextProperty, bindingBase33);
			linkButton10.Clicked += this.btnProfile_Clicked;
			bindingExtension34.Path = "IndexAndTitle";
			BindingBase bindingBase34 = bindingExtension34.ProvideValue(null);
			linkButton10.SetBinding(Button.TextProperty, bindingBase34);
			stackLayout2.Children.Add(linkButton10);
			bindingExtension35.Path = "Model.Profile11";
			BindingBase bindingBase35 = bindingExtension35.ProvideValue(null);
			linkButton11.SetBinding(BindableObject.BindingContextProperty, bindingBase35);
			linkButton11.Clicked += this.btnProfile_Clicked;
			bindingExtension36.Path = "IndexAndTitle";
			BindingBase bindingBase36 = bindingExtension36.ProvideValue(null);
			linkButton11.SetBinding(Button.TextProperty, bindingBase36);
			stackLayout2.Children.Add(linkButton11);
			bindingExtension37.Path = "Model.Profile12";
			BindingBase bindingBase37 = bindingExtension37.ProvideValue(null);
			linkButton12.SetBinding(BindableObject.BindingContextProperty, bindingBase37);
			linkButton12.Clicked += this.btnProfile_Clicked;
			bindingExtension38.Path = "IndexAndTitle";
			BindingBase bindingBase38 = bindingExtension38.ProvideValue(null);
			linkButton12.SetBinding(Button.TextProperty, bindingBase38);
			stackLayout2.Children.Add(linkButton12);
			label9.SetValue(Label.FontAttributesProperty, new FontAttributesConverter().ConvertFromInvariantString("Bold"));
			label9.SetValue(Label.TextProperty, "Controls:");
			stackLayout2.Children.Add(label9);
			listView.SetValue(ListView.HeaderProperty, stackLayout2);
			IDataTemplate dataTemplate2 = dataTemplate;
			FPAProfileCodingPage.<InitializeComponent>_anonXamlCDataTemplate_12 <InitializeComponent>_anonXamlCDataTemplate_ = new FPAProfileCodingPage.<InitializeComponent>_anonXamlCDataTemplate_12();
			object[] array14 = new object[0 + 6];
			array14[0] = dataTemplate;
			array14[1] = listView;
			array14[2] = stackLayout3;
			array14[3] = scrollView;
			array14[4] = grid;
			array14[5] = this;
			<InitializeComponent>_anonXamlCDataTemplate_.parentValues = array14;
			<InitializeComponent>_anonXamlCDataTemplate_.root = this;
			dataTemplate2.LoadTemplate = new Func<object>(<InitializeComponent>_anonXamlCDataTemplate_.LoadDataTemplate);
			listView.SetValue(ItemsView<Cell>.ItemTemplateProperty, dataTemplate);
			stackLayout3.Children.Add(listView);
			scrollView.Content = stackLayout3;
			grid.Children.Add(scrollView);
			activityFrame.SetValue(Grid.RowProperty, 0);
			activityFrame.SetValue(VisualElement.IsVisibleProperty, new VisualElement.VisibilityConverter().ConvertFromInvariantString("False"));
			grid.Children.Add(activityFrame);
			this.SetValue(ContentPage.ContentProperty, grid);
		}

		// Token: 0x06004E80 RID: 20096 RVA: 0x003B8414 File Offset: 0x003B6614
		[CompilerGenerated]
		private void <btnChooseOtherVersion_Clicked>b__8_2(ValueItemWithTranslation selected)
		{
			this.Coding.LoadModel(selected.Value);
		}

		// Token: 0x06004E81 RID: 20097 RVA: 0x003B8427 File Offset: 0x003B6627
		[CompilerGenerated]
		private bool <btnChooseOtherVersion_Clicked>b__8_3(ValueItemWithTranslation x)
		{
			return x.Value == this.Coding.CurrentState;
		}

		// Token: 0x06004E82 RID: 20098 RVA: 0x003B843F File Offset: 0x003B663F
		[CompilerGenerated]
		private void <btnSaveState_Clicked>b__9_0(string s)
		{
			Device.BeginInvokeOnMainThread(delegate
			{
				this.activityFrame.Text = s;
			});
		}

		// Token: 0x06004E83 RID: 20099 RVA: 0x003B8464 File Offset: 0x003B6664
		private void __InitComponentRuntime()
		{
			Extensions.LoadFromXaml<FPAProfileCodingPage>(this, typeof(FPAProfileCodingPage));
			this.lv = NameScopeExtensions.FindByName<ListView>(this, "lv");
			this.entryPassword = NameScopeExtensions.FindByName<Entry>(this, "entryPassword");
			this.labelState = NameScopeExtensions.FindByName<Label>(this, "labelState");
			this.btnChooseVersion = NameScopeExtensions.FindByName<Button>(this, "btnChooseVersion");
			this.btnSaveNewValue = NameScopeExtensions.FindByName<Button>(this, "btnSaveNewValue");
			this.activityFrame = NameScopeExtensions.FindByName<ActivityFrame>(this, "activityFrame");
		}

		// Token: 0x04002F14 RID: 12052
		private bool first_appearing = true;

		// Token: 0x04002F15 RID: 12053
		[CompilerGenerated]
		private FPACoding <Coding>k__BackingField;

		// Token: 0x04002F16 RID: 12054
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private ListView lv;

		// Token: 0x04002F17 RID: 12055
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private Entry entryPassword;

		// Token: 0x04002F18 RID: 12056
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private Label labelState;

		// Token: 0x04002F19 RID: 12057
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private Button btnChooseVersion;

		// Token: 0x04002F1A RID: 12058
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private Button btnSaveNewValue;

		// Token: 0x04002F1B RID: 12059
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private ActivityFrame activityFrame;

		// Token: 0x02000949 RID: 2377
		[CompilerGenerated]
		[Serializable]
		private sealed class <>c
		{
			// Token: 0x06004E84 RID: 20100 RVA: 0x003B84E8 File Offset: 0x003B66E8
			// Note: this type is marked as 'beforefieldinit'.
			static <>c()
			{
			}

			// Token: 0x06004E85 RID: 20101 RVA: 0x00002050 File Offset: 0x00000250
			public <>c()
			{
			}

			// Token: 0x06004E86 RID: 20102 RVA: 0x00016849 File Offset: 0x00014A49
			internal string <btnChooseOtherVersion_Clicked>b__8_0(string x)
			{
				return x;
			}

			// Token: 0x06004E87 RID: 20103 RVA: 0x003B84F4 File Offset: 0x003B66F4
			internal ValueItemWithTranslation <btnChooseOtherVersion_Clicked>b__8_1(string x)
			{
				return new ValueItemWithTranslation
				{
					Title = x,
					Value = x
				};
			}

			// Token: 0x04002F1C RID: 12060
			public static readonly FPAProfileCodingPage.<>c <>9 = new FPAProfileCodingPage.<>c();

			// Token: 0x04002F1D RID: 12061
			public static Func<string, string> <>9__8_0;

			// Token: 0x04002F1E RID: 12062
			public static Func<string, ValueItemWithTranslation> <>9__8_1;
		}

		// Token: 0x0200094A RID: 2378
		[CompilerGenerated]
		private sealed class <>c__DisplayClass9_0
		{
			// Token: 0x06004E88 RID: 20104 RVA: 0x00002050 File Offset: 0x00000250
			public <>c__DisplayClass9_0()
			{
			}

			// Token: 0x06004E89 RID: 20105 RVA: 0x003B8509 File Offset: 0x003B6709
			internal void <btnSaveState_Clicked>b__1()
			{
				this.<>4__this.activityFrame.Text = this.s;
			}

			// Token: 0x04002F1F RID: 12063
			public string s;

			// Token: 0x04002F20 RID: 12064
			public FPAProfileCodingPage <>4__this;
		}

		// Token: 0x0200094B RID: 2379
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <FPAProfileCodingPage_Appearing>d__2 : IAsyncStateMachine
		{
			// Token: 0x06004E8A RID: 20106 RVA: 0x003B8524 File Offset: 0x003B6724
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				FPAProfileCodingPage fpaprofileCodingPage = this;
				try
				{
					TaskAwaiter<CodingRequestResult> taskAwaiter;
					if (num != 0)
					{
						if (!fpaprofileCodingPage.first_appearing)
						{
							goto IL_00B7;
						}
						fpaprofileCodingPage.first_appearing = false;
						fpaprofileCodingPage.lv.IsEnabled = false;
						fpaprofileCodingPage.activityFrame.IsVisible = true;
						taskAwaiter = fpaprofileCodingPage.Coding.UpdateCurrentState("", null).GetAwaiter();
						if (!taskAwaiter.IsCompleted)
						{
							num2 = 0;
							TaskAwaiter<CodingRequestResult> taskAwaiter2 = taskAwaiter;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<CodingRequestResult>, FPAProfileCodingPage.<FPAProfileCodingPage_Appearing>d__2>(ref taskAwaiter, ref this);
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
					fpaprofileCodingPage.activityFrame.IsVisible = false;
					fpaprofileCodingPage.lv.IsEnabled = true;
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

			// Token: 0x06004E8B RID: 20107 RVA: 0x003B8624 File Offset: 0x003B6824
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x04002F21 RID: 12065
			public int <>1__state;

			// Token: 0x04002F22 RID: 12066
			public AsyncVoidMethodBuilder <>t__builder;

			// Token: 0x04002F23 RID: 12067
			public FPAProfileCodingPage <>4__this;

			// Token: 0x04002F24 RID: 12068
			private TaskAwaiter<CodingRequestResult> <>u__1;
		}

		// Token: 0x0200094C RID: 2380
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <Lv_ItemSelected>d__7 : IAsyncStateMachine
		{
			// Token: 0x06004E8C RID: 20108 RVA: 0x003B8634 File Offset: 0x003B6834
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				FPAProfileCodingPage fpaprofileCodingPage = this;
				try
				{
					TaskAwaiter taskAwaiter;
					if (num != 0)
					{
						if (fpaprofileCodingPage.lv.SelectedItem == null)
						{
							goto IL_00C3;
						}
						FPAProfile fpaprofile = (FPAProfile)fpaprofileCodingPage.lv.SelectedItem;
						fpaprofileCodingPage.lv.SelectedItem = null;
						FPAProfileEditor fpaprofileEditor = new FPAProfileEditor(fpaprofile);
						taskAwaiter = fpaprofileCodingPage.Navigation.PushAsync(fpaprofileEditor).GetAwaiter();
						if (!taskAwaiter.IsCompleted)
						{
							num2 = 0;
							TaskAwaiter taskAwaiter2 = taskAwaiter;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, FPAProfileCodingPage.<Lv_ItemSelected>d__7>(ref taskAwaiter, ref this);
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
				IL_00C3:
				num2 = -2;
				this.<>t__builder.SetResult();
			}

			// Token: 0x06004E8D RID: 20109 RVA: 0x003B8728 File Offset: 0x003B6928
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x04002F25 RID: 12069
			public int <>1__state;

			// Token: 0x04002F26 RID: 12070
			public AsyncVoidMethodBuilder <>t__builder;

			// Token: 0x04002F27 RID: 12071
			public FPAProfileCodingPage <>4__this;

			// Token: 0x04002F28 RID: 12072
			private TaskAwaiter <>u__1;
		}

		// Token: 0x0200094D RID: 2381
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <btnChooseOtherVersion_Clicked>d__8 : IAsyncStateMachine
		{
			// Token: 0x06004E8E RID: 20110 RVA: 0x003B8738 File Offset: 0x003B6938
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				FPAProfileCodingPage fpaprofileCodingPage = this;
				try
				{
					TaskAwaiter taskAwaiter;
					if (num != 0)
					{
						IEnumerable<ValueItemWithTranslation> enumerable = from x in (from x in PackageFileReader.GetFilesInDirectory("vag.fpa3Q0907530.")
								orderby x
								select x).ToArray<string>()
							select new ValueItemWithTranslation
							{
								Title = x,
								Value = x
							};
						Action<ValueItemWithTranslation> action = delegate(ValueItemWithTranslation selected)
						{
							base.Coding.LoadModel(selected.Value);
						};
						ItemWithValueSelectorPage itemWithValueSelectorPage = new ItemWithValueSelectorPage("Dataset version", enumerable, enumerable.FirstOrDefault((ValueItemWithTranslation x) => x.Value == base.Coding.CurrentState), action);
						taskAwaiter = fpaprofileCodingPage.Navigation.PushAsync(itemWithValueSelectorPage).GetAwaiter();
						if (!taskAwaiter.IsCompleted)
						{
							num2 = 0;
							TaskAwaiter taskAwaiter2 = taskAwaiter;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, FPAProfileCodingPage.<btnChooseOtherVersion_Clicked>d__8>(ref taskAwaiter, ref this);
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

			// Token: 0x06004E8F RID: 20111 RVA: 0x003B8880 File Offset: 0x003B6A80
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x04002F29 RID: 12073
			public int <>1__state;

			// Token: 0x04002F2A RID: 12074
			public AsyncVoidMethodBuilder <>t__builder;

			// Token: 0x04002F2B RID: 12075
			public FPAProfileCodingPage <>4__this;

			// Token: 0x04002F2C RID: 12076
			private TaskAwaiter <>u__1;
		}

		// Token: 0x0200094E RID: 2382
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <btnProfile_Clicked>d__10 : IAsyncStateMachine
		{
			// Token: 0x06004E90 RID: 20112 RVA: 0x003B8890 File Offset: 0x003B6A90
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				FPAProfileCodingPage fpaprofileCodingPage = this;
				try
				{
					TaskAwaiter taskAwaiter;
					if (num != 0)
					{
						if (!fpaprofileCodingPage.Coding.IsLoaded)
						{
							goto IL_0097;
						}
						FPAProfileEditor fpaprofileEditor = new FPAProfileEditor((FPAProfile)((View)sender).BindingContext);
						taskAwaiter = fpaprofileCodingPage.Navigation.PushAsync(fpaprofileEditor).GetAwaiter();
						if (!taskAwaiter.IsCompleted)
						{
							num2 = 0;
							TaskAwaiter taskAwaiter2 = taskAwaiter;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, FPAProfileCodingPage.<btnProfile_Clicked>d__10>(ref taskAwaiter, ref this);
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
					IL_0097:;
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

			// Token: 0x06004E91 RID: 20113 RVA: 0x003B8974 File Offset: 0x003B6B74
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x04002F2D RID: 12077
			public int <>1__state;

			// Token: 0x04002F2E RID: 12078
			public AsyncVoidMethodBuilder <>t__builder;

			// Token: 0x04002F2F RID: 12079
			public FPAProfileCodingPage <>4__this;

			// Token: 0x04002F30 RID: 12080
			public object sender;

			// Token: 0x04002F31 RID: 12081
			private TaskAwaiter <>u__1;
		}

		// Token: 0x0200094F RID: 2383
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <btnSaveState_Clicked>d__9 : IAsyncStateMachine
		{
			// Token: 0x06004E92 RID: 20114 RVA: 0x003B8984 File Offset: 0x003B6B84
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				FPAProfileCodingPage fpaprofileCodingPage = this;
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
						goto IL_0148;
					case 2:
					{
						TaskAwaiter taskAwaiter4;
						taskAwaiter3 = taskAwaiter4;
						taskAwaiter4 = default(TaskAwaiter);
						num2 = -1;
						goto IL_01B6;
					}
					case 3:
						taskAwaiter5 = taskAwaiter2;
						taskAwaiter2 = default(TaskAwaiter<bool>);
						num2 = -1;
						goto IL_0270;
					case 4:
					{
						TaskAwaiter taskAwaiter4;
						taskAwaiter3 = taskAwaiter4;
						taskAwaiter4 = default(TaskAwaiter);
						num2 = -1;
						goto IL_02DE;
					}
					case 5:
					{
						TaskAwaiter<CodingRequestResult> taskAwaiter7;
						taskAwaiter6 = taskAwaiter7;
						taskAwaiter7 = default(TaskAwaiter<CodingRequestResult>);
						num2 = -1;
						goto IL_0388;
					}
					case 6:
					{
						TaskAwaiter taskAwaiter4;
						taskAwaiter3 = taskAwaiter4;
						taskAwaiter4 = default(TaskAwaiter);
						num2 = -1;
						goto IL_041D;
					}
					case 7:
					{
						TaskAwaiter taskAwaiter4;
						taskAwaiter3 = taskAwaiter4;
						taskAwaiter4 = default(TaskAwaiter);
						num2 = -1;
						goto IL_048F;
					}
					default:
						if (fpaprofileCodingPage.Coding.IsLoaded)
						{
							goto IL_00B4;
						}
						taskAwaiter3 = fpaprofileCodingPage.DisplayAlert(fpaprofileCodingPage.Coding.CurrentState, "", "OK").GetAwaiter();
						if (!taskAwaiter3.IsCompleted)
						{
							num2 = 0;
							TaskAwaiter taskAwaiter4 = taskAwaiter3;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, FPAProfileCodingPage.<btnSaveState_Clicked>d__9>(ref taskAwaiter3, ref this);
							return;
						}
						break;
					}
					taskAwaiter3.GetResult();
					IL_00B4:
					if (fpaprofileCodingPage.Coding.RequiresPro && !SharedSettings.Current.AdsProductPurchased)
					{
						taskAwaiter5 = fpaprofileCodingPage.DisplayAlert("Car Scanner Pro", Translate.GetString("FeatureLocked"), "OK", Translate.GetString("btnCancel.Content")).GetAwaiter();
						if (!taskAwaiter5.IsCompleted)
						{
							num2 = 1;
							taskAwaiter2 = taskAwaiter5;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<bool>, FPAProfileCodingPage.<btnSaveState_Clicked>d__9>(ref taskAwaiter5, ref this);
							return;
						}
					}
					else if (SharedSettings.Current.CodingsCounter >= 3 && !SharedSettings.Current.AdsProductPurchased)
					{
						taskAwaiter5 = fpaprofileCodingPage.DisplayAlert("Car Scanner Pro", string.Format(Translate.GetString("coding_FreeCodingsFinished"), SharedSettings.Current.CodingsCounter, 3), "OK", Translate.GetString("btnCancel.Content")).GetAwaiter();
						if (!taskAwaiter5.IsCompleted)
						{
							num2 = 3;
							taskAwaiter2 = taskAwaiter5;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<bool>, FPAProfileCodingPage.<btnSaveState_Clicked>d__9>(ref taskAwaiter5, ref this);
							return;
						}
						goto IL_0270;
					}
					else
					{
						fpaprofileCodingPage.activityFrame.IsVisible = true;
						fpaprofileCodingPage.lv.IsEnabled = false;
						Progress<string> progress = new Progress<string>(delegate(string s)
						{
							Device.BeginInvokeOnMainThread(new Action(new FPAProfileCodingPage.<>c__DisplayClass9_0
							{
								<>4__this = fpaprofileCodingPage,
								s = s
							}.<btnSaveState_Clicked>b__1));
						});
						taskAwaiter6 = fpaprofileCodingPage.Coding.Execute(fpaprofileCodingPage.entryPassword.Text, "", "", progress, null, false).GetAwaiter();
						if (!taskAwaiter6.IsCompleted)
						{
							num2 = 5;
							TaskAwaiter<CodingRequestResult> taskAwaiter7 = taskAwaiter6;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<CodingRequestResult>, FPAProfileCodingPage.<btnSaveState_Clicked>d__9>(ref taskAwaiter6, ref this);
							return;
						}
						goto IL_0388;
					}
					IL_0148:
					if (!taskAwaiter5.GetResult())
					{
						goto IL_01BD;
					}
					Page inAppPage = InAppManager.GetInAppPage();
					taskAwaiter3 = fpaprofileCodingPage.Navigation.PushAsync(inAppPage).GetAwaiter();
					if (!taskAwaiter3.IsCompleted)
					{
						num2 = 2;
						TaskAwaiter taskAwaiter4 = taskAwaiter3;
						this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, FPAProfileCodingPage.<btnSaveState_Clicked>d__9>(ref taskAwaiter3, ref this);
						return;
					}
					IL_01B6:
					taskAwaiter3.GetResult();
					IL_01BD:
					goto IL_04D9;
					IL_0270:
					if (!taskAwaiter5.GetResult())
					{
						goto IL_02E5;
					}
					Page inAppPage2 = InAppManager.GetInAppPage();
					taskAwaiter3 = fpaprofileCodingPage.Navigation.PushAsync(inAppPage2).GetAwaiter();
					if (!taskAwaiter3.IsCompleted)
					{
						num2 = 4;
						TaskAwaiter taskAwaiter4 = taskAwaiter3;
						this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, FPAProfileCodingPage.<btnSaveState_Clicked>d__9>(ref taskAwaiter3, ref this);
						return;
					}
					IL_02DE:
					taskAwaiter3.GetResult();
					IL_02E5:
					goto IL_04D9;
					IL_0388:
					CodingRequestResult result = taskAwaiter6.GetResult();
					if (result == CodingRequestResult.Success)
					{
						SharedSettings sharedSettings = SharedSettings.Current;
						int codingsCounter = sharedSettings.CodingsCounter;
						sharedSettings.CodingsCounter = codingsCounter + 1;
						taskAwaiter3 = fpaprofileCodingPage.DisplayAlert(fpaprofileCodingPage.Coding.Name, Translate.GetString("coding_OperationFinished"), "OK").GetAwaiter();
						if (!taskAwaiter3.IsCompleted)
						{
							num2 = 6;
							TaskAwaiter taskAwaiter4 = taskAwaiter3;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, FPAProfileCodingPage.<btnSaveState_Clicked>d__9>(ref taskAwaiter3, ref this);
							return;
						}
					}
					else
					{
						taskAwaiter3 = fpaprofileCodingPage.DisplayAlert(Translate.GetString("coding_Error"), MQBAdaptationTemplate.CodingRequestResultToString(result), "OK").GetAwaiter();
						if (!taskAwaiter3.IsCompleted)
						{
							num2 = 7;
							TaskAwaiter taskAwaiter4 = taskAwaiter3;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, FPAProfileCodingPage.<btnSaveState_Clicked>d__9>(ref taskAwaiter3, ref this);
							return;
						}
						goto IL_048F;
					}
					IL_041D:
					taskAwaiter3.GetResult();
					goto IL_0496;
					IL_048F:
					taskAwaiter3.GetResult();
					IL_0496:
					fpaprofileCodingPage.activityFrame.IsVisible = false;
					fpaprofileCodingPage.activityFrame.Text = ActivityFrame.PLEASE_WAIT_TEXT;
					fpaprofileCodingPage.lv.IsEnabled = true;
				}
				catch (Exception ex)
				{
					num2 = -2;
					this.<>t__builder.SetException(ex);
					return;
				}
				IL_04D9:
				num2 = -2;
				this.<>t__builder.SetResult();
			}

			// Token: 0x06004E93 RID: 20115 RVA: 0x003B8E9C File Offset: 0x003B709C
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x04002F32 RID: 12082
			public int <>1__state;

			// Token: 0x04002F33 RID: 12083
			public AsyncVoidMethodBuilder <>t__builder;

			// Token: 0x04002F34 RID: 12084
			public FPAProfileCodingPage <>4__this;

			// Token: 0x04002F35 RID: 12085
			private TaskAwaiter <>u__1;

			// Token: 0x04002F36 RID: 12086
			private TaskAwaiter<bool> <>u__2;

			// Token: 0x04002F37 RID: 12087
			private TaskAwaiter<CodingRequestResult> <>u__3;
		}

		// Token: 0x02000950 RID: 2384
		[CompilerGenerated]
		private sealed class <InitializeComponent>_anonXamlCDataTemplate_12
		{
			// Token: 0x06004E94 RID: 20116 RVA: 0x003B8EAC File Offset: 0x003B70AC
			public <InitializeComponent>_anonXamlCDataTemplate_12()
			{
			}

			// Token: 0x06004E95 RID: 20117 RVA: 0x003B8EC0 File Offset: 0x003B70C0
			internal object LoadDataTemplate()
			{
				BindingExtension bindingExtension;
				VisualDiagnostics.RegisterSourceInfo(bindingExtension = new BindingExtension(), new Uri("Coding\\Pages\\FPAProfileCodingPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 149, 45);
				BindingExtension bindingExtension2;
				VisualDiagnostics.RegisterSourceInfo(bindingExtension2 = new BindingExtension(), new Uri("Coding\\Pages\\FPAProfileCodingPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 150, 45);
				LabelSwitch labelSwitch;
				VisualDiagnostics.RegisterSourceInfo(labelSwitch = new LabelSwitch(), new Uri("Coding\\Pages\\FPAProfileCodingPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 147, 42);
				StackLayout stackLayout;
				VisualDiagnostics.RegisterSourceInfo(stackLayout = new StackLayout(), new Uri("Coding\\Pages\\FPAProfileCodingPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 146, 38);
				ViewCell viewCell;
				VisualDiagnostics.RegisterSourceInfo(viewCell = new ViewCell(), new Uri("Coding\\Pages\\FPAProfileCodingPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 145, 34);
				NameScope nameScope = new NameScope();
				NameScope.SetNameScope(viewCell, nameScope);
				stackLayout.SetValue(StackLayout.OrientationProperty, 0);
				labelSwitch.SetValue(View.MarginProperty, new Thickness(0.0, 0.0, 0.0, 5.0));
				bindingExtension.Mode = 1;
				bindingExtension.Path = "SaveOnRestart";
				BindingBase bindingBase = bindingExtension.ProvideValue(null);
				labelSwitch.SetBinding(LabelSwitch.IsToggledProperty, bindingBase);
				bindingExtension2.Path = "Title";
				BindingBase bindingBase2 = bindingExtension2.ProvideValue(null);
				labelSwitch.SetBinding(LabelSwitch.TextProperty, bindingBase2);
				labelSwitch.SetValue(View.VerticalOptionsProperty, LayoutOptions.Center);
				stackLayout.Children.Add(labelSwitch);
				viewCell.View = stackLayout;
				return viewCell;
			}

			// Token: 0x04002F38 RID: 12088
			internal object[] parentValues;

			// Token: 0x04002F39 RID: 12089
			internal FPAProfileCodingPage root;
		}
	}
}
