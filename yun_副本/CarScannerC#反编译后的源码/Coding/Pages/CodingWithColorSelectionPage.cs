using System;
using System.CodeDom.Compiler;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using CarScannerXamarinForms.Coding.Models;
using CarScannerXamarinForms.Common;
using CarScannerXamarinForms.Common.XAMLConverters;
using CarScannerXamarinForms.InApp;
using CarScannerXamarinForms.Pages;
using CarScannerXamarinForms.PlatformAdapters;
using CarScannerXamarinForms.Settings;
using Xamarin.Forms;
using Xamarin.Forms.Internals;
using Xamarin.Forms.PlatformConfiguration.iOSSpecific;
using Xamarin.Forms.Xaml;
using Xamarin.Forms.Xaml.Diagnostics;
using Xamarin.Forms.Xaml.Internals;

namespace CarScannerXamarinForms.Coding.Pages
{
	// Token: 0x02000927 RID: 2343
	[XamlCompilation(2)]
	[XamlFilePath("Coding\\Pages\\CodingWithColorSelectionPage.xaml")]
	public class CodingWithColorSelectionPage : ContentPage
	{
		// Token: 0x06004DF9 RID: 19961 RVA: 0x003A5F4C File Offset: 0x003A414C
		public CodingWithColorSelectionPage(ICodingContainer coding)
		{
			this.InitializeComponent();
			this.Coding = coding;
			try
			{
				base.BindingContext = this;
			}
			catch (Exception)
			{
			}
			base.Appearing += this.CodingDetailsPage_Appearing;
		}

		// Token: 0x06004DFA RID: 19962 RVA: 0x003A5FAC File Offset: 0x003A41AC
		private void Lv_ItemSelected(object sender, SelectedItemChangedEventArgs e)
		{
			if (e.SelectedItem == null)
			{
				return;
			}
			ColorObject colorObject = (ColorObject)e.SelectedItem;
			this.lv.SelectedItem = null;
			ColorSelectorPageV2 colorSelectorPageV = new ColorSelectorPageV2(colorObject.Color, delegate(Color new_color)
			{
				colorObject.Color = new_color;
			});
			base.Navigation.PushAsync(colorSelectorPageV);
		}

		// Token: 0x06004DFB RID: 19963 RVA: 0x00022295 File Offset: 0x00020495
		private void Entry_Completed(object sender, EventArgs e)
		{
			PlatformHelper.CommonService.HideKeyboard();
		}

		// Token: 0x17001786 RID: 6022
		// (get) Token: 0x06004DFC RID: 19964 RVA: 0x003A600F File Offset: 0x003A420F
		// (set) Token: 0x06004DFD RID: 19965 RVA: 0x003A6017 File Offset: 0x003A4217
		public ObservableCollection<ColorObject> Model
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
		} = new ObservableCollection<ColorObject>();

		// Token: 0x06004DFE RID: 19966 RVA: 0x003A6020 File Offset: 0x003A4220
		private async void CodingDetailsPage_Appearing(object sender, EventArgs e)
		{
			if (this.first_appearing)
			{
				this.first_appearing = false;
				this.lv.IsEnabled = false;
				this.activityFrame.IsVisible = true;
				await this.Coding.UpdateCurrentState("", null);
				this.FillColorListFromHexData(this.Coding.CurrentState);
				this.activityFrame.IsVisible = false;
				this.lv.IsEnabled = true;
			}
		}

		// Token: 0x06004DFF RID: 19967 RVA: 0x003A6058 File Offset: 0x003A4258
		public async Task UpdateState()
		{
			this.activityFrame.IsVisible = true;
			this.lv.IsEnabled = false;
			await this.Coding.UpdateCurrentState("", null);
			this.FillColorListFromHexData(this.Coding.CurrentState);
			this.activityFrame.IsVisible = false;
			this.lv.IsEnabled = true;
		}

		// Token: 0x06004E00 RID: 19968 RVA: 0x003A609C File Offset: 0x003A429C
		private void FillColorListFromHexData(string hex)
		{
			this.Model.Clear();
			try
			{
				byte[] array = BitHelpers.ConvertHexToBytesX(hex.Trim().Replace(" ", ""));
				int num = 1;
				MQBAdaptationTemplate mqbadaptationTemplate = (MQBAdaptationTemplate)this.Coding;
				if (mqbadaptationTemplate.Address == "0586" || mqbadaptationTemplate.Address == "0587")
				{
					num = 11;
				}
				for (int i = 0; i < array.Length; i += 3)
				{
					byte b = array[i];
					byte b2 = array[i + 1];
					byte b3 = array[i + 2];
					ColorObject colorObject = new ColorObject(b, b2, b3);
					colorObject.Number = num;
					num++;
					this.Model.Add(colorObject);
				}
			}
			catch (Exception)
			{
			}
		}

		// Token: 0x06004E01 RID: 19969 RVA: 0x003A615C File Offset: 0x003A435C
		private async void BtnUpdateState_Clicked(object sender, EventArgs e)
		{
			this.UpdateState();
		}

		// Token: 0x17001787 RID: 6023
		// (get) Token: 0x06004E02 RID: 19970 RVA: 0x003A6193 File Offset: 0x003A4393
		// (set) Token: 0x06004E03 RID: 19971 RVA: 0x003A619B File Offset: 0x003A439B
		public ICodingContainer Coding
		{
			[CompilerGenerated]
			get
			{
				return this.<Coding>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<Coding>k__BackingField = value;
			}
		}

		// Token: 0x06004E04 RID: 19972 RVA: 0x003A61A4 File Offset: 0x003A43A4
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
				this.btnSaveNewValue.IsEnabled = false;
				this.btnUpdateState.IsEnabled = false;
				Progress<string> progress = new Progress<string>(delegate(string s)
				{
					Device.BeginInvokeOnMainThread(delegate
					{
						this.activityFrame.Text = s;
					});
				});
				string text = this.ColorModelToHex();
				CodingRequestResult codingRequestResult = await this.Coding.Execute(this.entryPassword.Text, text, "Color configuration", progress, null, false);
				if (codingRequestResult == CodingRequestResult.Success)
				{
					SharedSettings.Current.CodingsCounter++;
				}
				else
				{
					await base.DisplayAlert(Translate.GetString("coding_Error"), MQBAdaptationTemplate.CodingRequestResultToString(codingRequestResult), "OK");
				}
				await this.Coding.UpdateCurrentState("", null);
				this.FillColorListFromHexData(this.Coding.CurrentState);
				this.activityFrame.IsVisible = false;
				this.btnSaveNewValue.IsEnabled = true;
				this.btnUpdateState.IsEnabled = true;
				this.lv.IsEnabled = true;
				this.activityFrame.Text = ActivityFrame.PLEASE_WAIT_TEXT;
			}
		}

		// Token: 0x06004E05 RID: 19973 RVA: 0x003A61DC File Offset: 0x003A43DC
		private string ColorModelToHex()
		{
			StringBuilder stringBuilder = new StringBuilder(10);
			foreach (ColorObject colorObject in this.Model)
			{
				string text = colorObject.Color.ToHex().Substring(3);
				stringBuilder.Append(text);
			}
			return stringBuilder.ToString();
		}

		// Token: 0x06004E06 RID: 19974 RVA: 0x003A624C File Offset: 0x003A444C
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private void InitializeComponent()
		{
			if (ResourceLoader.IsEnabled && ResourceLoader.CanProvideContentFor(new ResourceLoader.ResourceLoadingQuery
			{
				AssemblyName = typeof(CodingWithColorSelectionPage).GetTypeInfo().Assembly.GetName(),
				ResourcePath = "Coding/Pages/CodingWithColorSelectionPage.xaml",
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
			VisualDiagnostics.RegisterSourceInfo(translate = new Translate(), new Uri("Coding\\Pages\\CodingWithColorSelectionPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 10, 5);
			DynamicResourceExtension dynamicResourceExtension;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension = new DynamicResourceExtension(), new Uri("Coding\\Pages\\CodingWithColorSelectionPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 13, 5);
			EmptyStringToFalseConverter emptyStringToFalseConverter;
			VisualDiagnostics.RegisterSourceInfo(emptyStringToFalseConverter = new EmptyStringToFalseConverter(), new Uri("Coding\\Pages\\CodingWithColorSelectionPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 16, 14);
			EmptyStringToTrueConverter emptyStringToTrueConverter;
			VisualDiagnostics.RegisterSourceInfo(emptyStringToTrueConverter = new EmptyStringToTrueConverter(), new Uri("Coding\\Pages\\CodingWithColorSelectionPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 17, 14);
			VagCodingPlatformToTrueConverter vagCodingPlatformToTrueConverter;
			VisualDiagnostics.RegisterSourceInfo(vagCodingPlatformToTrueConverter = new VagCodingPlatformToTrueConverter(), new Uri("Coding\\Pages\\CodingWithColorSelectionPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 18, 14);
			ResourceDictionary resourceDictionary;
			VisualDiagnostics.RegisterSourceInfo(resourceDictionary = new ResourceDictionary(), new Uri("Coding\\Pages\\CodingWithColorSelectionPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 15, 10);
			RowDefinition rowDefinition;
			VisualDiagnostics.RegisterSourceInfo(rowDefinition = new RowDefinition(), new Uri("Coding\\Pages\\CodingWithColorSelectionPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 24, 18);
			BindingExtension bindingExtension;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension = new BindingExtension(), new Uri("Coding\\Pages\\CodingWithColorSelectionPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 31, 17);
			DynamicResourceExtension dynamicResourceExtension2;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension2 = new DynamicResourceExtension(), new Uri("Coding\\Pages\\CodingWithColorSelectionPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 36, 29);
			BindingExtension bindingExtension2;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension2 = new BindingExtension(), new Uri("Coding\\Pages\\CodingWithColorSelectionPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 37, 29);
			Label label;
			VisualDiagnostics.RegisterSourceInfo(label = new Label(), new Uri("Coding\\Pages\\CodingWithColorSelectionPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 34, 26);
			StaticResourceExtension staticResourceExtension;
			VisualDiagnostics.RegisterSourceInfo(staticResourceExtension = new StaticResourceExtension(), new Uri("Coding\\Pages\\CodingWithColorSelectionPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 38, 32);
			BindingExtension bindingExtension3;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension3 = new BindingExtension(), new Uri("Coding\\Pages\\CodingWithColorSelectionPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 38, 32);
			BindingExtension bindingExtension4;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension4 = new BindingExtension(), new Uri("Coding\\Pages\\CodingWithColorSelectionPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 38, 129);
			Label label2;
			VisualDiagnostics.RegisterSourceInfo(label2 = new Label(), new Uri("Coding\\Pages\\CodingWithColorSelectionPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 38, 26);
			StaticResourceExtension staticResourceExtension2;
			VisualDiagnostics.RegisterSourceInfo(staticResourceExtension2 = new StaticResourceExtension(), new Uri("Coding\\Pages\\CodingWithColorSelectionPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 39, 32);
			BindingExtension bindingExtension5;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension5 = new BindingExtension(), new Uri("Coding\\Pages\\CodingWithColorSelectionPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 39, 32);
			BindingExtension bindingExtension6;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension6 = new BindingExtension(), new Uri("Coding\\Pages\\CodingWithColorSelectionPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 39, 127);
			Label label3;
			VisualDiagnostics.RegisterSourceInfo(label3 = new Label(), new Uri("Coding\\Pages\\CodingWithColorSelectionPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 39, 26);
			Translate translate2;
			VisualDiagnostics.RegisterSourceInfo(translate2 = new Translate(), new Uri("Coding\\Pages\\CodingWithColorSelectionPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 41, 32);
			Label label4;
			VisualDiagnostics.RegisterSourceInfo(label4 = new Label(), new Uri("Coding\\Pages\\CodingWithColorSelectionPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 41, 26);
			BindingExtension bindingExtension7;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension7 = new BindingExtension(), new Uri("Coding\\Pages\\CodingWithColorSelectionPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 47, 29);
			Entry entry;
			VisualDiagnostics.RegisterSourceInfo(entry = new Entry(), new Uri("Coding\\Pages\\CodingWithColorSelectionPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 42, 26);
			StaticResourceExtension staticResourceExtension3;
			VisualDiagnostics.RegisterSourceInfo(staticResourceExtension3 = new StaticResourceExtension(), new Uri("Coding\\Pages\\CodingWithColorSelectionPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 48, 56);
			BindingExtension bindingExtension8;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension8 = new BindingExtension(), new Uri("Coding\\Pages\\CodingWithColorSelectionPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 48, 56);
			Translate translate3;
			VisualDiagnostics.RegisterSourceInfo(translate3 = new Translate(), new Uri("Coding\\Pages\\CodingWithColorSelectionPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 51, 43);
			Span span;
			VisualDiagnostics.RegisterSourceInfo(span = new Span(), new Uri("Coding\\Pages\\CodingWithColorSelectionPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 51, 38);
			BindingExtension bindingExtension9;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension9 = new BindingExtension(), new Uri("Coding\\Pages\\CodingWithColorSelectionPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 52, 43);
			Span span2;
			VisualDiagnostics.RegisterSourceInfo(span2 = new Span(), new Uri("Coding\\Pages\\CodingWithColorSelectionPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 52, 38);
			FormattedString formattedString;
			VisualDiagnostics.RegisterSourceInfo(formattedString = new FormattedString(), new Uri("Coding\\Pages\\CodingWithColorSelectionPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 50, 34);
			Label label5;
			VisualDiagnostics.RegisterSourceInfo(label5 = new Label(), new Uri("Coding\\Pages\\CodingWithColorSelectionPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 48, 26);
			ColumnDefinition columnDefinition;
			VisualDiagnostics.RegisterSourceInfo(columnDefinition = new ColumnDefinition(), new Uri("Coding\\Pages\\CodingWithColorSelectionPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 59, 34);
			ColumnDefinition columnDefinition2;
			VisualDiagnostics.RegisterSourceInfo(columnDefinition2 = new ColumnDefinition(), new Uri("Coding\\Pages\\CodingWithColorSelectionPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 60, 34);
			BindingExtension bindingExtension10;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension10 = new BindingExtension(), new Uri("Coding\\Pages\\CodingWithColorSelectionPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 66, 33);
			Translate translate4;
			VisualDiagnostics.RegisterSourceInfo(translate4 = new Translate(), new Uri("Coding\\Pages\\CodingWithColorSelectionPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 67, 33);
			Button button;
			VisualDiagnostics.RegisterSourceInfo(button = new Button(), new Uri("Coding\\Pages\\CodingWithColorSelectionPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 62, 30);
			Translate translate5;
			VisualDiagnostics.RegisterSourceInfo(translate5 = new Translate(), new Uri("Coding\\Pages\\CodingWithColorSelectionPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 72, 33);
			Button button2;
			VisualDiagnostics.RegisterSourceInfo(button2 = new Button(), new Uri("Coding\\Pages\\CodingWithColorSelectionPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 68, 30);
			Grid grid;
			VisualDiagnostics.RegisterSourceInfo(grid = new Grid(), new Uri("Coding\\Pages\\CodingWithColorSelectionPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 57, 26);
			Translate translate6;
			VisualDiagnostics.RegisterSourceInfo(translate6 = new Translate(), new Uri("Coding\\Pages\\CodingWithColorSelectionPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 76, 54);
			Label label6;
			VisualDiagnostics.RegisterSourceInfo(label6 = new Label(), new Uri("Coding\\Pages\\CodingWithColorSelectionPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 76, 26);
			StackLayout stackLayout;
			VisualDiagnostics.RegisterSourceInfo(stackLayout = new StackLayout(), new Uri("Coding\\Pages\\CodingWithColorSelectionPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 33, 22);
			DataTemplate dataTemplate;
			VisualDiagnostics.RegisterSourceInfo(dataTemplate = new DataTemplate(), new Uri("Coding\\Pages\\CodingWithColorSelectionPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 88, 22);
			SharedSettings sharedSettings;
			VisualDiagnostics.RegisterSourceInfo(sharedSettings = SharedSettings.Current, new Uri("Coding\\Pages\\CodingWithColorSelectionPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 127, 29);
			StaticResourceExtension staticResourceExtension4;
			VisualDiagnostics.RegisterSourceInfo(staticResourceExtension4 = new StaticResourceExtension(), new Uri("Coding\\Pages\\CodingWithColorSelectionPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 129, 29);
			BindingExtension bindingExtension11;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension11 = new BindingExtension(), new Uri("Coding\\Pages\\CodingWithColorSelectionPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 129, 29);
			Translate translate7;
			VisualDiagnostics.RegisterSourceInfo(translate7 = new Translate(), new Uri("Coding\\Pages\\CodingWithColorSelectionPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 130, 29);
			Label label7;
			VisualDiagnostics.RegisterSourceInfo(label7 = new Label(), new Uri("Coding\\Pages\\CodingWithColorSelectionPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 126, 26);
			SharedSettings sharedSettings2;
			VisualDiagnostics.RegisterSourceInfo(sharedSettings2 = SharedSettings.Current, new Uri("Coding\\Pages\\CodingWithColorSelectionPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 132, 29);
			BindingExtension bindingExtension12;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension12 = new BindingExtension(), new Uri("Coding\\Pages\\CodingWithColorSelectionPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 133, 29);
			BindingExtension bindingExtension13;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension13 = new BindingExtension(), new Uri("Coding\\Pages\\CodingWithColorSelectionPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 134, 29);
			Translate translate8;
			VisualDiagnostics.RegisterSourceInfo(translate8 = new Translate(), new Uri("Coding\\Pages\\CodingWithColorSelectionPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 135, 29);
			LabelSwitch labelSwitch;
			VisualDiagnostics.RegisterSourceInfo(labelSwitch = new LabelSwitch(), new Uri("Coding\\Pages\\CodingWithColorSelectionPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 131, 26);
			StackLayout stackLayout2;
			VisualDiagnostics.RegisterSourceInfo(stackLayout2 = new StackLayout(), new Uri("Coding\\Pages\\CodingWithColorSelectionPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 125, 22);
			ListView listView;
			VisualDiagnostics.RegisterSourceInfo(listView = new ListView(), new Uri("Coding\\Pages\\CodingWithColorSelectionPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 26, 14);
			ActivityFrame activityFrame;
			VisualDiagnostics.RegisterSourceInfo(activityFrame = new ActivityFrame(), new Uri("Coding\\Pages\\CodingWithColorSelectionPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 139, 14);
			Grid grid2;
			VisualDiagnostics.RegisterSourceInfo(grid2 = new Grid(), new Uri("Coding\\Pages\\CodingWithColorSelectionPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 22, 10);
			VisualDiagnostics.RegisterSourceInfo(this, new Uri("Coding\\Pages\\CodingWithColorSelectionPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 2, 2);
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
			nameScope.RegisterName("btnUpdateState", button);
			if (button.StyleId == null)
			{
				button.StyleId = "btnUpdateState";
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
			this.btnUpdateState = button;
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
			xmlNamespaceResolver.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver.Add("settings", "clr-namespace:CarScannerXamarinForms.Settings");
			xamlServiceProvider.Add(typeFromHandle2, new XamlTypeResolver(xmlNamespaceResolver, typeof(CodingWithColorSelectionPage).GetTypeInfo().Assembly));
			xamlServiceProvider.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(10, 5)));
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
			xmlNamespaceResolver2.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver2.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver2.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver2.Add("settings", "clr-namespace:CarScannerXamarinForms.Settings");
			xamlServiceProvider2.Add(typeFromHandle4, new XamlTypeResolver(xmlNamespaceResolver2, typeof(CodingWithColorSelectionPage).GetTypeInfo().Assembly));
			xamlServiceProvider2.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(13, 5)));
			DynamicResource dynamicResource = markupExtension2.ProvideValue(xamlServiceProvider2);
			this.SetDynamicResource(VisualElement.BackgroundColorProperty, dynamicResource.Key);
			this.Resources = resourceDictionary;
			rowDefinition.SetValue(RowDefinition.HeightProperty, new GridLengthTypeConverter().ConvertFromInvariantString("*"));
			grid2.GetValue(Grid.RowDefinitionsProperty).Add(rowDefinition);
			listView.SetValue(Grid.RowProperty, 0);
			listView.SetValue(ListView.HasUnevenRowsProperty, true);
			listView.ItemSelected += this.Lv_ItemSelected;
			bindingExtension.Path = "Model";
			BindingBase bindingBase = bindingExtension.ProvideValue(null);
			listView.SetBinding(ItemsView<Cell>.ItemsSourceProperty, bindingBase);
			stackLayout.SetValue(StackLayout.OrientationProperty, 0);
			label.SetValue(Label.FontAttributesProperty, new FontAttributesConverter().ConvertFromInvariantString("Bold"));
			dynamicResourceExtension2.Key = "BaseFontSize+++";
			IMarkupExtension<DynamicResource> markupExtension3 = dynamicResourceExtension2;
			XamlServiceProvider xamlServiceProvider3 = new XamlServiceProvider();
			Type typeFromHandle5 = typeof(IProvideValueTarget);
			object[] array3 = new object[0 + 5];
			array3[0] = label;
			array3[1] = stackLayout;
			array3[2] = listView;
			array3[3] = grid2;
			array3[4] = this;
			object obj4;
			xamlServiceProvider3.Add(typeFromHandle5, obj4 = new SimpleValueTargetProvider(array3, Label.FontSizeProperty, nameScope));
			xamlServiceProvider3.Add(typeof(IReferenceProvider), obj4);
			Type typeFromHandle6 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver3 = new XmlNamespaceResolver();
			xmlNamespaceResolver3.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver3.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver3.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver3.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver3.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver3.Add("settings", "clr-namespace:CarScannerXamarinForms.Settings");
			xamlServiceProvider3.Add(typeFromHandle6, new XamlTypeResolver(xmlNamespaceResolver3, typeof(CodingWithColorSelectionPage).GetTypeInfo().Assembly));
			xamlServiceProvider3.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(36, 29)));
			DynamicResource dynamicResource2 = markupExtension3.ProvideValue(xamlServiceProvider3);
			label.SetDynamicResource(Label.FontSizeProperty, dynamicResource2.Key);
			bindingExtension2.Path = "Coding.Name";
			BindingBase bindingBase2 = bindingExtension2.ProvideValue(null);
			label.SetBinding(Label.TextProperty, bindingBase2);
			stackLayout.Children.Add(label);
			staticResourceExtension.Key = "EmptyStringToFalseConverter";
			IMarkupExtension markupExtension4 = staticResourceExtension;
			XamlServiceProvider xamlServiceProvider4 = new XamlServiceProvider();
			Type typeFromHandle7 = typeof(IProvideValueTarget);
			object[] array4 = new object[0 + 6];
			array4[0] = bindingExtension3;
			array4[1] = label2;
			array4[2] = stackLayout;
			array4[3] = listView;
			array4[4] = grid2;
			array4[5] = this;
			object obj5;
			xamlServiceProvider4.Add(typeFromHandle7, obj5 = new SimpleValueTargetProvider(array4, typeof(BindingExtension).GetRuntimeProperty("Converter"), nameScope));
			xamlServiceProvider4.Add(typeof(IReferenceProvider), obj5);
			Type typeFromHandle8 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver4 = new XmlNamespaceResolver();
			xmlNamespaceResolver4.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver4.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver4.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver4.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver4.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver4.Add("settings", "clr-namespace:CarScannerXamarinForms.Settings");
			xamlServiceProvider4.Add(typeFromHandle8, new XamlTypeResolver(xmlNamespaceResolver4, typeof(CodingWithColorSelectionPage).GetTypeInfo().Assembly));
			xamlServiceProvider4.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(38, 32)));
			object obj6 = markupExtension4.ProvideValue(xamlServiceProvider4);
			bindingExtension3.Converter = obj6;
			bindingExtension3.Path = "Coding.Description";
			BindingBase bindingBase3 = bindingExtension3.ProvideValue(null);
			label2.SetBinding(VisualElement.IsVisibleProperty, bindingBase3);
			bindingExtension4.Path = "Coding.Description";
			BindingBase bindingBase4 = bindingExtension4.ProvideValue(null);
			label2.SetBinding(Label.TextProperty, bindingBase4);
			stackLayout.Children.Add(label2);
			staticResourceExtension2.Key = "EmptyStringToFalseConverter";
			IMarkupExtension markupExtension5 = staticResourceExtension2;
			XamlServiceProvider xamlServiceProvider5 = new XamlServiceProvider();
			Type typeFromHandle9 = typeof(IProvideValueTarget);
			object[] array5 = new object[0 + 6];
			array5[0] = bindingExtension5;
			array5[1] = label3;
			array5[2] = stackLayout;
			array5[3] = listView;
			array5[4] = grid2;
			array5[5] = this;
			object obj7;
			xamlServiceProvider5.Add(typeFromHandle9, obj7 = new SimpleValueTargetProvider(array5, typeof(BindingExtension).GetRuntimeProperty("Converter"), nameScope));
			xamlServiceProvider5.Add(typeof(IReferenceProvider), obj7);
			Type typeFromHandle10 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver5 = new XmlNamespaceResolver();
			xmlNamespaceResolver5.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver5.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver5.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver5.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver5.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver5.Add("settings", "clr-namespace:CarScannerXamarinForms.Settings");
			xamlServiceProvider5.Add(typeFromHandle10, new XamlTypeResolver(xmlNamespaceResolver5, typeof(CodingWithColorSelectionPage).GetTypeInfo().Assembly));
			xamlServiceProvider5.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(39, 32)));
			object obj8 = markupExtension5.ProvideValue(xamlServiceProvider5);
			bindingExtension5.Converter = obj8;
			bindingExtension5.Path = "InnerDescription";
			BindingBase bindingBase5 = bindingExtension5.ProvideValue(null);
			label3.SetBinding(VisualElement.IsVisibleProperty, bindingBase5);
			bindingExtension6.Path = "InnerDescription";
			BindingBase bindingBase6 = bindingExtension6.ProvideValue(null);
			label3.SetBinding(Label.TextProperty, bindingBase6);
			stackLayout.Children.Add(label3);
			translate2.Text = "coding_Password";
			IMarkupExtension markupExtension6 = translate2;
			XamlServiceProvider xamlServiceProvider6 = new XamlServiceProvider();
			Type typeFromHandle11 = typeof(IProvideValueTarget);
			object[] array6 = new object[0 + 5];
			array6[0] = label4;
			array6[1] = stackLayout;
			array6[2] = listView;
			array6[3] = grid2;
			array6[4] = this;
			object obj9;
			xamlServiceProvider6.Add(typeFromHandle11, obj9 = new SimpleValueTargetProvider(array6, Label.TextProperty, nameScope));
			xamlServiceProvider6.Add(typeof(IReferenceProvider), obj9);
			Type typeFromHandle12 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver6 = new XmlNamespaceResolver();
			xmlNamespaceResolver6.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver6.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver6.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver6.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver6.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver6.Add("settings", "clr-namespace:CarScannerXamarinForms.Settings");
			xamlServiceProvider6.Add(typeFromHandle12, new XamlTypeResolver(xmlNamespaceResolver6, typeof(CodingWithColorSelectionPage).GetTypeInfo().Assembly));
			xamlServiceProvider6.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(41, 32)));
			object obj10 = markupExtension6.ProvideValue(xamlServiceProvider6);
			label4.Text = obj10;
			stackLayout.Children.Add(label4);
			entry.Completed += this.Entry_Completed;
			entry.SetValue(InputView.IsSpellCheckEnabledProperty, false);
			entry.SetValue(Entry.IsTextPredictionEnabledProperty, false);
			bindingExtension7.Mode = 1;
			bindingExtension7.Path = "Coding.Password";
			BindingBase bindingBase7 = bindingExtension7.ProvideValue(null);
			entry.SetBinding(Entry.TextProperty, bindingBase7);
			stackLayout.Children.Add(entry);
			label5.SetValue(Label.FontAttributesProperty, new FontAttributesConverter().ConvertFromInvariantString("Italic"));
			staticResourceExtension3.Key = "EmptyStringToFalseConverter";
			IMarkupExtension markupExtension7 = staticResourceExtension3;
			XamlServiceProvider xamlServiceProvider7 = new XamlServiceProvider();
			Type typeFromHandle13 = typeof(IProvideValueTarget);
			object[] array7 = new object[0 + 6];
			array7[0] = bindingExtension8;
			array7[1] = label5;
			array7[2] = stackLayout;
			array7[3] = listView;
			array7[4] = grid2;
			array7[5] = this;
			object obj11;
			xamlServiceProvider7.Add(typeFromHandle13, obj11 = new SimpleValueTargetProvider(array7, typeof(BindingExtension).GetRuntimeProperty("Converter"), nameScope));
			xamlServiceProvider7.Add(typeof(IReferenceProvider), obj11);
			Type typeFromHandle14 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver7 = new XmlNamespaceResolver();
			xmlNamespaceResolver7.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver7.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver7.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver7.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver7.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver7.Add("settings", "clr-namespace:CarScannerXamarinForms.Settings");
			xamlServiceProvider7.Add(typeFromHandle14, new XamlTypeResolver(xmlNamespaceResolver7, typeof(CodingWithColorSelectionPage).GetTypeInfo().Assembly));
			xamlServiceProvider7.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(48, 56)));
			object obj12 = markupExtension7.ProvideValue(xamlServiceProvider7);
			bindingExtension8.Converter = obj12;
			bindingExtension8.Path = "Coding.PasswordHint";
			BindingBase bindingBase8 = bindingExtension8.ProvideValue(null);
			label5.SetBinding(VisualElement.IsVisibleProperty, bindingBase8);
			translate3.Text = "coding_PasswordHint";
			IMarkupExtension markupExtension8 = translate3;
			XamlServiceProvider xamlServiceProvider8 = new XamlServiceProvider();
			Type typeFromHandle15 = typeof(IProvideValueTarget);
			object[] array8 = new object[0 + 7];
			array8[0] = span;
			array8[1] = formattedString;
			array8[2] = label5;
			array8[3] = stackLayout;
			array8[4] = listView;
			array8[5] = grid2;
			array8[6] = this;
			object obj13;
			xamlServiceProvider8.Add(typeFromHandle15, obj13 = new SimpleValueTargetProvider(array8, Span.TextProperty, nameScope));
			xamlServiceProvider8.Add(typeof(IReferenceProvider), obj13);
			Type typeFromHandle16 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver8 = new XmlNamespaceResolver();
			xmlNamespaceResolver8.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver8.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver8.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver8.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver8.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver8.Add("settings", "clr-namespace:CarScannerXamarinForms.Settings");
			xamlServiceProvider8.Add(typeFromHandle16, new XamlTypeResolver(xmlNamespaceResolver8, typeof(CodingWithColorSelectionPage).GetTypeInfo().Assembly));
			xamlServiceProvider8.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(51, 43)));
			object obj14 = markupExtension8.ProvideValue(xamlServiceProvider8);
			span.Text = obj14;
			formattedString.Spans.Add(span);
			bindingExtension9.Path = "Coding.PasswordHint";
			BindingBase bindingBase9 = bindingExtension9.ProvideValue(null);
			span2.SetBinding(Span.TextProperty, bindingBase9);
			formattedString.Spans.Add(span2);
			label5.SetValue(Label.FormattedTextProperty, formattedString);
			stackLayout.Children.Add(label5);
			columnDefinition.SetValue(ColumnDefinition.WidthProperty, new GridLengthTypeConverter().ConvertFromInvariantString("*"));
			grid.GetValue(Grid.ColumnDefinitionsProperty).Add(columnDefinition);
			columnDefinition2.SetValue(ColumnDefinition.WidthProperty, new GridLengthTypeConverter().ConvertFromInvariantString("*"));
			grid.GetValue(Grid.ColumnDefinitionsProperty).Add(columnDefinition2);
			button.SetValue(Grid.ColumnProperty, 0);
			button.Clicked += this.BtnUpdateState_Clicked;
			bindingExtension10.Path = "HasCurrentState";
			BindingBase bindingBase10 = bindingExtension10.ProvideValue(null);
			button.SetBinding(VisualElement.IsVisibleProperty, bindingBase10);
			translate4.Text = "Mode06Page_btnRefresh.Content";
			IMarkupExtension markupExtension9 = translate4;
			XamlServiceProvider xamlServiceProvider9 = new XamlServiceProvider();
			Type typeFromHandle17 = typeof(IProvideValueTarget);
			object[] array9 = new object[0 + 6];
			array9[0] = button;
			array9[1] = grid;
			array9[2] = stackLayout;
			array9[3] = listView;
			array9[4] = grid2;
			array9[5] = this;
			object obj15;
			xamlServiceProvider9.Add(typeFromHandle17, obj15 = new SimpleValueTargetProvider(array9, Button.TextProperty, nameScope));
			xamlServiceProvider9.Add(typeof(IReferenceProvider), obj15);
			Type typeFromHandle18 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver9 = new XmlNamespaceResolver();
			xmlNamespaceResolver9.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver9.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver9.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver9.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver9.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver9.Add("settings", "clr-namespace:CarScannerXamarinForms.Settings");
			xamlServiceProvider9.Add(typeFromHandle18, new XamlTypeResolver(xmlNamespaceResolver9, typeof(CodingWithColorSelectionPage).GetTypeInfo().Assembly));
			xamlServiceProvider9.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(67, 33)));
			object obj16 = markupExtension9.ProvideValue(xamlServiceProvider9);
			button.Text = obj16;
			grid.Children.Add(button);
			button2.SetValue(Grid.ColumnProperty, 1);
			button2.Clicked += this.btnSaveState_Clicked;
			translate5.Text = "coding_Apply";
			IMarkupExtension markupExtension10 = translate5;
			XamlServiceProvider xamlServiceProvider10 = new XamlServiceProvider();
			Type typeFromHandle19 = typeof(IProvideValueTarget);
			object[] array10 = new object[0 + 6];
			array10[0] = button2;
			array10[1] = grid;
			array10[2] = stackLayout;
			array10[3] = listView;
			array10[4] = grid2;
			array10[5] = this;
			object obj17;
			xamlServiceProvider10.Add(typeFromHandle19, obj17 = new SimpleValueTargetProvider(array10, Button.TextProperty, nameScope));
			xamlServiceProvider10.Add(typeof(IReferenceProvider), obj17);
			Type typeFromHandle20 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver10 = new XmlNamespaceResolver();
			xmlNamespaceResolver10.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver10.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver10.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver10.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver10.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver10.Add("settings", "clr-namespace:CarScannerXamarinForms.Settings");
			xamlServiceProvider10.Add(typeFromHandle20, new XamlTypeResolver(xmlNamespaceResolver10, typeof(CodingWithColorSelectionPage).GetTypeInfo().Assembly));
			xamlServiceProvider10.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(72, 33)));
			object obj18 = markupExtension10.ProvideValue(xamlServiceProvider10);
			button2.Text = obj18;
			grid.Children.Add(button2);
			stackLayout.Children.Add(grid);
			label6.SetValue(Label.FontAttributesProperty, new FontAttributesConverter().ConvertFromInvariantString("Bold"));
			translate6.Text = "coding_Data";
			IMarkupExtension markupExtension11 = translate6;
			XamlServiceProvider xamlServiceProvider11 = new XamlServiceProvider();
			Type typeFromHandle21 = typeof(IProvideValueTarget);
			object[] array11 = new object[0 + 5];
			array11[0] = label6;
			array11[1] = stackLayout;
			array11[2] = listView;
			array11[3] = grid2;
			array11[4] = this;
			object obj19;
			xamlServiceProvider11.Add(typeFromHandle21, obj19 = new SimpleValueTargetProvider(array11, Label.TextProperty, nameScope));
			xamlServiceProvider11.Add(typeof(IReferenceProvider), obj19);
			Type typeFromHandle22 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver11 = new XmlNamespaceResolver();
			xmlNamespaceResolver11.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver11.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver11.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver11.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver11.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver11.Add("settings", "clr-namespace:CarScannerXamarinForms.Settings");
			xamlServiceProvider11.Add(typeFromHandle22, new XamlTypeResolver(xmlNamespaceResolver11, typeof(CodingWithColorSelectionPage).GetTypeInfo().Assembly));
			xamlServiceProvider11.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(76, 54)));
			object obj20 = markupExtension11.ProvideValue(xamlServiceProvider11);
			label6.Text = obj20;
			stackLayout.Children.Add(label6);
			listView.SetValue(ListView.HeaderProperty, stackLayout);
			IDataTemplate dataTemplate2 = dataTemplate;
			CodingWithColorSelectionPage.<InitializeComponent>_anonXamlCDataTemplate_9 <InitializeComponent>_anonXamlCDataTemplate_ = new CodingWithColorSelectionPage.<InitializeComponent>_anonXamlCDataTemplate_9();
			object[] array12 = new object[0 + 4];
			array12[0] = dataTemplate;
			array12[1] = listView;
			array12[2] = grid2;
			array12[3] = this;
			<InitializeComponent>_anonXamlCDataTemplate_.parentValues = array12;
			<InitializeComponent>_anonXamlCDataTemplate_.root = this;
			dataTemplate2.LoadTemplate = new Func<object>(<InitializeComponent>_anonXamlCDataTemplate_.LoadDataTemplate);
			listView.SetValue(ItemsView<Cell>.ItemTemplateProperty, dataTemplate);
			stackLayout2.SetValue(StackLayout.OrientationProperty, 0);
			label7.SetValue(BindableObject.BindingContextProperty, sharedSettings);
			label7.SetValue(Label.FontAttributesProperty, new FontAttributesConverter().ConvertFromInvariantString("Italic"));
			staticResourceExtension4.Key = "VagCodingPlatformToTrueConverter";
			IMarkupExtension markupExtension12 = staticResourceExtension4;
			XamlServiceProvider xamlServiceProvider12 = new XamlServiceProvider();
			Type typeFromHandle23 = typeof(IProvideValueTarget);
			object[] array13 = new object[0 + 6];
			array13[0] = bindingExtension11;
			array13[1] = label7;
			array13[2] = stackLayout2;
			array13[3] = listView;
			array13[4] = grid2;
			array13[5] = this;
			object obj21;
			xamlServiceProvider12.Add(typeFromHandle23, obj21 = new SimpleValueTargetProvider(array13, typeof(BindingExtension).GetRuntimeProperty("Converter"), nameScope));
			xamlServiceProvider12.Add(typeof(IReferenceProvider), obj21);
			Type typeFromHandle24 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver12 = new XmlNamespaceResolver();
			xmlNamespaceResolver12.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver12.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver12.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver12.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver12.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver12.Add("settings", "clr-namespace:CarScannerXamarinForms.Settings");
			xamlServiceProvider12.Add(typeFromHandle24, new XamlTypeResolver(xmlNamespaceResolver12, typeof(CodingWithColorSelectionPage).GetTypeInfo().Assembly));
			xamlServiceProvider12.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(129, 29)));
			object obj22 = markupExtension12.ProvideValue(xamlServiceProvider12);
			bindingExtension11.Converter = obj22;
			bindingExtension11.Path = "CodingLastPlatformSelected";
			BindingBase bindingBase11 = bindingExtension11.ProvideValue(null);
			label7.SetBinding(VisualElement.IsVisibleProperty, bindingBase11);
			translate7.Text = "coding_VagOpenHoodWarning";
			IMarkupExtension markupExtension13 = translate7;
			XamlServiceProvider xamlServiceProvider13 = new XamlServiceProvider();
			Type typeFromHandle25 = typeof(IProvideValueTarget);
			object[] array14 = new object[0 + 5];
			array14[0] = label7;
			array14[1] = stackLayout2;
			array14[2] = listView;
			array14[3] = grid2;
			array14[4] = this;
			object obj23;
			xamlServiceProvider13.Add(typeFromHandle25, obj23 = new SimpleValueTargetProvider(array14, Label.TextProperty, nameScope));
			xamlServiceProvider13.Add(typeof(IReferenceProvider), obj23);
			Type typeFromHandle26 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver13 = new XmlNamespaceResolver();
			xmlNamespaceResolver13.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver13.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver13.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver13.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver13.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver13.Add("settings", "clr-namespace:CarScannerXamarinForms.Settings");
			xamlServiceProvider13.Add(typeFromHandle26, new XamlTypeResolver(xmlNamespaceResolver13, typeof(CodingWithColorSelectionPage).GetTypeInfo().Assembly));
			xamlServiceProvider13.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(130, 29)));
			object obj24 = markupExtension13.ProvideValue(xamlServiceProvider13);
			label7.Text = obj24;
			stackLayout2.Children.Add(label7);
			labelSwitch.SetValue(BindableObject.BindingContextProperty, sharedSettings2);
			bindingExtension12.Mode = 1;
			bindingExtension12.Path = "IgnoreCodingErrors";
			BindingBase bindingBase12 = bindingExtension12.ProvideValue(null);
			labelSwitch.SetBinding(LabelSwitch.IsToggledProperty, bindingBase12);
			bindingExtension13.Path = "ShowExperimental";
			BindingBase bindingBase13 = bindingExtension13.ProvideValue(null);
			labelSwitch.SetBinding(VisualElement.IsVisibleProperty, bindingBase13);
			translate8.Text = "coding_ignore_fails";
			IMarkupExtension markupExtension14 = translate8;
			XamlServiceProvider xamlServiceProvider14 = new XamlServiceProvider();
			Type typeFromHandle27 = typeof(IProvideValueTarget);
			object[] array15 = new object[0 + 5];
			array15[0] = labelSwitch;
			array15[1] = stackLayout2;
			array15[2] = listView;
			array15[3] = grid2;
			array15[4] = this;
			object obj25;
			xamlServiceProvider14.Add(typeFromHandle27, obj25 = new SimpleValueTargetProvider(array15, LabelSwitch.TextProperty, nameScope));
			xamlServiceProvider14.Add(typeof(IReferenceProvider), obj25);
			Type typeFromHandle28 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver14 = new XmlNamespaceResolver();
			xmlNamespaceResolver14.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver14.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver14.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver14.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver14.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver14.Add("settings", "clr-namespace:CarScannerXamarinForms.Settings");
			xamlServiceProvider14.Add(typeFromHandle28, new XamlTypeResolver(xmlNamespaceResolver14, typeof(CodingWithColorSelectionPage).GetTypeInfo().Assembly));
			xamlServiceProvider14.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(135, 29)));
			object obj26 = markupExtension14.ProvideValue(xamlServiceProvider14);
			labelSwitch.Text = obj26;
			stackLayout2.Children.Add(labelSwitch);
			listView.SetValue(ListView.FooterProperty, stackLayout2);
			grid2.Children.Add(listView);
			activityFrame.SetValue(Grid.RowProperty, 0);
			activityFrame.SetValue(VisualElement.IsVisibleProperty, new VisualElement.VisibilityConverter().ConvertFromInvariantString("False"));
			grid2.Children.Add(activityFrame);
			this.SetValue(ContentPage.ContentProperty, grid2);
		}

		// Token: 0x06004E07 RID: 19975 RVA: 0x003A838D File Offset: 0x003A658D
		[CompilerGenerated]
		private void <btnSaveState_Clicked>b__16_0(string s)
		{
			Device.BeginInvokeOnMainThread(delegate
			{
				this.activityFrame.Text = s;
			});
		}

		// Token: 0x06004E08 RID: 19976 RVA: 0x003A83B4 File Offset: 0x003A65B4
		private void __InitComponentRuntime()
		{
			Extensions.LoadFromXaml<CodingWithColorSelectionPage>(this, typeof(CodingWithColorSelectionPage));
			this.lv = NameScopeExtensions.FindByName<ListView>(this, "lv");
			this.entryPassword = NameScopeExtensions.FindByName<Entry>(this, "entryPassword");
			this.btnUpdateState = NameScopeExtensions.FindByName<Button>(this, "btnUpdateState");
			this.btnSaveNewValue = NameScopeExtensions.FindByName<Button>(this, "btnSaveNewValue");
			this.activityFrame = NameScopeExtensions.FindByName<ActivityFrame>(this, "activityFrame");
		}

		// Token: 0x04002E84 RID: 11908
		private bool first_appearing = true;

		// Token: 0x04002E85 RID: 11909
		[CompilerGenerated]
		private ObservableCollection<ColorObject> <Model>k__BackingField;

		// Token: 0x04002E86 RID: 11910
		[CompilerGenerated]
		private ICodingContainer <Coding>k__BackingField;

		// Token: 0x04002E87 RID: 11911
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private ListView lv;

		// Token: 0x04002E88 RID: 11912
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private Entry entryPassword;

		// Token: 0x04002E89 RID: 11913
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private Button btnUpdateState;

		// Token: 0x04002E8A RID: 11914
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private Button btnSaveNewValue;

		// Token: 0x04002E8B RID: 11915
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private ActivityFrame activityFrame;

		// Token: 0x02000928 RID: 2344
		[CompilerGenerated]
		private sealed class <>c__DisplayClass16_0
		{
			// Token: 0x06004E09 RID: 19977 RVA: 0x00002050 File Offset: 0x00000250
			public <>c__DisplayClass16_0()
			{
			}

			// Token: 0x06004E0A RID: 19978 RVA: 0x003A8427 File Offset: 0x003A6627
			internal void <btnSaveState_Clicked>b__1()
			{
				this.<>4__this.activityFrame.Text = this.s;
			}

			// Token: 0x04002E8C RID: 11916
			public string s;

			// Token: 0x04002E8D RID: 11917
			public CodingWithColorSelectionPage <>4__this;
		}

		// Token: 0x02000929 RID: 2345
		[CompilerGenerated]
		private sealed class <>c__DisplayClass1_0
		{
			// Token: 0x06004E0B RID: 19979 RVA: 0x00002050 File Offset: 0x00000250
			public <>c__DisplayClass1_0()
			{
			}

			// Token: 0x06004E0C RID: 19980 RVA: 0x003A843F File Offset: 0x003A663F
			internal void <Lv_ItemSelected>b__0(Color new_color)
			{
				this.colorObject.Color = new_color;
			}

			// Token: 0x04002E8E RID: 11918
			public ColorObject colorObject;
		}

		// Token: 0x0200092A RID: 2346
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <BtnUpdateState_Clicked>d__11 : IAsyncStateMachine
		{
			// Token: 0x06004E0D RID: 19981 RVA: 0x003A8450 File Offset: 0x003A6650
			void IAsyncStateMachine.MoveNext()
			{
				CodingWithColorSelectionPage codingWithColorSelectionPage = this;
				try
				{
					codingWithColorSelectionPage.UpdateState();
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

			// Token: 0x06004E0E RID: 19982 RVA: 0x003A84A8 File Offset: 0x003A66A8
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x04002E8F RID: 11919
			public int <>1__state;

			// Token: 0x04002E90 RID: 11920
			public AsyncVoidMethodBuilder <>t__builder;

			// Token: 0x04002E91 RID: 11921
			public CodingWithColorSelectionPage <>4__this;
		}

		// Token: 0x0200092B RID: 2347
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <CodingDetailsPage_Appearing>d__8 : IAsyncStateMachine
		{
			// Token: 0x06004E0F RID: 19983 RVA: 0x003A84B8 File Offset: 0x003A66B8
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				CodingWithColorSelectionPage codingWithColorSelectionPage = this;
				try
				{
					TaskAwaiter<CodingRequestResult> taskAwaiter;
					if (num != 0)
					{
						if (!codingWithColorSelectionPage.first_appearing)
						{
							goto IL_00C8;
						}
						codingWithColorSelectionPage.first_appearing = false;
						codingWithColorSelectionPage.lv.IsEnabled = false;
						codingWithColorSelectionPage.activityFrame.IsVisible = true;
						taskAwaiter = codingWithColorSelectionPage.Coding.UpdateCurrentState("", null).GetAwaiter();
						if (!taskAwaiter.IsCompleted)
						{
							num2 = 0;
							TaskAwaiter<CodingRequestResult> taskAwaiter2 = taskAwaiter;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<CodingRequestResult>, CodingWithColorSelectionPage.<CodingDetailsPage_Appearing>d__8>(ref taskAwaiter, ref this);
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
					codingWithColorSelectionPage.FillColorListFromHexData(codingWithColorSelectionPage.Coding.CurrentState);
					codingWithColorSelectionPage.activityFrame.IsVisible = false;
					codingWithColorSelectionPage.lv.IsEnabled = true;
					IL_00C8:;
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

			// Token: 0x06004E10 RID: 19984 RVA: 0x003A85CC File Offset: 0x003A67CC
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x04002E92 RID: 11922
			public int <>1__state;

			// Token: 0x04002E93 RID: 11923
			public AsyncVoidMethodBuilder <>t__builder;

			// Token: 0x04002E94 RID: 11924
			public CodingWithColorSelectionPage <>4__this;

			// Token: 0x04002E95 RID: 11925
			private TaskAwaiter<CodingRequestResult> <>u__1;
		}

		// Token: 0x0200092C RID: 2348
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <UpdateState>d__9 : IAsyncStateMachine
		{
			// Token: 0x06004E11 RID: 19985 RVA: 0x003A85DC File Offset: 0x003A67DC
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				CodingWithColorSelectionPage codingWithColorSelectionPage = this;
				try
				{
					TaskAwaiter<CodingRequestResult> taskAwaiter;
					if (num != 0)
					{
						codingWithColorSelectionPage.activityFrame.IsVisible = true;
						codingWithColorSelectionPage.lv.IsEnabled = false;
						taskAwaiter = codingWithColorSelectionPage.Coding.UpdateCurrentState("", null).GetAwaiter();
						if (!taskAwaiter.IsCompleted)
						{
							num2 = 0;
							TaskAwaiter<CodingRequestResult> taskAwaiter2 = taskAwaiter;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<CodingRequestResult>, CodingWithColorSelectionPage.<UpdateState>d__9>(ref taskAwaiter, ref this);
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
					codingWithColorSelectionPage.FillColorListFromHexData(codingWithColorSelectionPage.Coding.CurrentState);
					codingWithColorSelectionPage.activityFrame.IsVisible = false;
					codingWithColorSelectionPage.lv.IsEnabled = true;
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

			// Token: 0x06004E12 RID: 19986 RVA: 0x003A86DC File Offset: 0x003A68DC
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x04002E96 RID: 11926
			public int <>1__state;

			// Token: 0x04002E97 RID: 11927
			public AsyncTaskMethodBuilder <>t__builder;

			// Token: 0x04002E98 RID: 11928
			public CodingWithColorSelectionPage <>4__this;

			// Token: 0x04002E99 RID: 11929
			private TaskAwaiter<CodingRequestResult> <>u__1;
		}

		// Token: 0x0200092D RID: 2349
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <btnSaveState_Clicked>d__16 : IAsyncStateMachine
		{
			// Token: 0x06004E13 RID: 19987 RVA: 0x003A86EC File Offset: 0x003A68EC
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				CodingWithColorSelectionPage codingWithColorSelectionPage = this;
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
						goto IL_031F;
					}
					case 5:
					{
						TaskAwaiter taskAwaiter5;
						taskAwaiter4 = taskAwaiter5;
						taskAwaiter5 = default(TaskAwaiter);
						num2 = -1;
						goto IL_03B1;
					}
					case 6:
					{
						TaskAwaiter<CodingRequestResult> taskAwaiter7;
						taskAwaiter6 = taskAwaiter7;
						taskAwaiter7 = default(TaskAwaiter<CodingRequestResult>);
						num2 = -1;
						goto IL_041A;
					}
					default:
						if (codingWithColorSelectionPage.Coding.RequiresPro && !SharedSettings.Current.AdsProductPurchased)
						{
							taskAwaiter3 = codingWithColorSelectionPage.DisplayAlert("Car Scanner Pro", Translate.GetString("FeatureLocked"), "OK", Translate.GetString("btnCancel.Content")).GetAwaiter();
							if (!taskAwaiter3.IsCompleted)
							{
								num2 = 0;
								taskAwaiter2 = taskAwaiter3;
								this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<bool>, CodingWithColorSelectionPage.<btnSaveState_Clicked>d__16>(ref taskAwaiter3, ref this);
								return;
							}
						}
						else if (SharedSettings.Current.CodingsCounter >= 3 && !SharedSettings.Current.AdsProductPurchased)
						{
							taskAwaiter3 = codingWithColorSelectionPage.DisplayAlert("Car Scanner Pro", string.Format(Translate.GetString("coding_FreeCodingsFinished"), SharedSettings.Current.CodingsCounter, 3), "OK", Translate.GetString("btnCancel.Content")).GetAwaiter();
							if (!taskAwaiter3.IsCompleted)
							{
								num2 = 2;
								taskAwaiter2 = taskAwaiter3;
								this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<bool>, CodingWithColorSelectionPage.<btnSaveState_Clicked>d__16>(ref taskAwaiter3, ref this);
								return;
							}
							goto IL_01EC;
						}
						else
						{
							codingWithColorSelectionPage.activityFrame.IsVisible = true;
							codingWithColorSelectionPage.lv.IsEnabled = false;
							codingWithColorSelectionPage.btnSaveNewValue.IsEnabled = false;
							codingWithColorSelectionPage.btnUpdateState.IsEnabled = false;
							Progress<string> progress = new Progress<string>(delegate(string s)
							{
								Device.BeginInvokeOnMainThread(new Action(new CodingWithColorSelectionPage.<>c__DisplayClass16_0
								{
									<>4__this = codingWithColorSelectionPage,
									s = s
								}.<btnSaveState_Clicked>b__1));
							});
							string text = codingWithColorSelectionPage.ColorModelToHex();
							taskAwaiter6 = codingWithColorSelectionPage.Coding.Execute(codingWithColorSelectionPage.entryPassword.Text, text, "Color configuration", progress, null, false).GetAwaiter();
							if (!taskAwaiter6.IsCompleted)
							{
								num2 = 4;
								TaskAwaiter<CodingRequestResult> taskAwaiter7 = taskAwaiter6;
								this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<CodingRequestResult>, CodingWithColorSelectionPage.<btnSaveState_Clicked>d__16>(ref taskAwaiter6, ref this);
								return;
							}
							goto IL_031F;
						}
						break;
					}
					if (!taskAwaiter3.GetResult())
					{
						goto IL_0139;
					}
					Page inAppPage = InAppManager.GetInAppPage();
					taskAwaiter4 = codingWithColorSelectionPage.Navigation.PushAsync(inAppPage).GetAwaiter();
					if (!taskAwaiter4.IsCompleted)
					{
						num2 = 1;
						TaskAwaiter taskAwaiter5 = taskAwaiter4;
						this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, CodingWithColorSelectionPage.<btnSaveState_Clicked>d__16>(ref taskAwaiter4, ref this);
						return;
					}
					IL_0132:
					taskAwaiter4.GetResult();
					IL_0139:
					goto IL_048E;
					IL_01EC:
					if (!taskAwaiter3.GetResult())
					{
						goto IL_0261;
					}
					Page inAppPage2 = InAppManager.GetInAppPage();
					taskAwaiter4 = codingWithColorSelectionPage.Navigation.PushAsync(inAppPage2).GetAwaiter();
					if (!taskAwaiter4.IsCompleted)
					{
						num2 = 3;
						TaskAwaiter taskAwaiter5 = taskAwaiter4;
						this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, CodingWithColorSelectionPage.<btnSaveState_Clicked>d__16>(ref taskAwaiter4, ref this);
						return;
					}
					IL_025A:
					taskAwaiter4.GetResult();
					IL_0261:
					goto IL_048E;
					IL_031F:
					CodingRequestResult result = taskAwaiter6.GetResult();
					if (result == CodingRequestResult.Success)
					{
						SharedSettings sharedSettings = SharedSettings.Current;
						int codingsCounter = sharedSettings.CodingsCounter;
						sharedSettings.CodingsCounter = codingsCounter + 1;
						goto IL_03B8;
					}
					taskAwaiter4 = codingWithColorSelectionPage.DisplayAlert(Translate.GetString("coding_Error"), MQBAdaptationTemplate.CodingRequestResultToString(result), "OK").GetAwaiter();
					if (!taskAwaiter4.IsCompleted)
					{
						num2 = 5;
						TaskAwaiter taskAwaiter5 = taskAwaiter4;
						this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, CodingWithColorSelectionPage.<btnSaveState_Clicked>d__16>(ref taskAwaiter4, ref this);
						return;
					}
					IL_03B1:
					taskAwaiter4.GetResult();
					IL_03B8:
					taskAwaiter6 = codingWithColorSelectionPage.Coding.UpdateCurrentState("", null).GetAwaiter();
					if (!taskAwaiter6.IsCompleted)
					{
						num2 = 6;
						TaskAwaiter<CodingRequestResult> taskAwaiter7 = taskAwaiter6;
						this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<CodingRequestResult>, CodingWithColorSelectionPage.<btnSaveState_Clicked>d__16>(ref taskAwaiter6, ref this);
						return;
					}
					IL_041A:
					taskAwaiter6.GetResult();
					codingWithColorSelectionPage.FillColorListFromHexData(codingWithColorSelectionPage.Coding.CurrentState);
					codingWithColorSelectionPage.activityFrame.IsVisible = false;
					codingWithColorSelectionPage.btnSaveNewValue.IsEnabled = true;
					codingWithColorSelectionPage.btnUpdateState.IsEnabled = true;
					codingWithColorSelectionPage.lv.IsEnabled = true;
					codingWithColorSelectionPage.activityFrame.Text = ActivityFrame.PLEASE_WAIT_TEXT;
				}
				catch (Exception ex)
				{
					num2 = -2;
					this.<>t__builder.SetException(ex);
					return;
				}
				IL_048E:
				num2 = -2;
				this.<>t__builder.SetResult();
			}

			// Token: 0x06004E14 RID: 19988 RVA: 0x003A8BB8 File Offset: 0x003A6DB8
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x04002E9A RID: 11930
			public int <>1__state;

			// Token: 0x04002E9B RID: 11931
			public AsyncVoidMethodBuilder <>t__builder;

			// Token: 0x04002E9C RID: 11932
			public CodingWithColorSelectionPage <>4__this;

			// Token: 0x04002E9D RID: 11933
			private TaskAwaiter<bool> <>u__1;

			// Token: 0x04002E9E RID: 11934
			private TaskAwaiter <>u__2;

			// Token: 0x04002E9F RID: 11935
			private TaskAwaiter<CodingRequestResult> <>u__3;
		}

		// Token: 0x0200092E RID: 2350
		[CompilerGenerated]
		private sealed class <InitializeComponent>_anonXamlCDataTemplate_9
		{
			// Token: 0x06004E15 RID: 19989 RVA: 0x003A8BC8 File Offset: 0x003A6DC8
			public <InitializeComponent>_anonXamlCDataTemplate_9()
			{
			}

			// Token: 0x06004E16 RID: 19990 RVA: 0x003A8BDC File Offset: 0x003A6DDC
			internal object LoadDataTemplate()
			{
				DynamicResourceExtension dynamicResourceExtension;
				VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension = new DynamicResourceExtension(), new Uri("Coding\\Pages\\CodingWithColorSelectionPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 92, 33);
				DynamicResourceExtension dynamicResourceExtension2;
				VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension2 = new DynamicResourceExtension(), new Uri("Coding\\Pages\\CodingWithColorSelectionPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 95, 33);
				ColumnDefinition columnDefinition;
				VisualDiagnostics.RegisterSourceInfo(columnDefinition = new ColumnDefinition(), new Uri("Coding\\Pages\\CodingWithColorSelectionPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 98, 42);
				ColumnDefinition columnDefinition2;
				VisualDiagnostics.RegisterSourceInfo(columnDefinition2 = new ColumnDefinition(), new Uri("Coding\\Pages\\CodingWithColorSelectionPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 99, 42);
				BindingExtension bindingExtension;
				VisualDiagnostics.RegisterSourceInfo(bindingExtension = new BindingExtension(), new Uri("Coding\\Pages\\CodingWithColorSelectionPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 105, 41);
				DynamicResourceExtension dynamicResourceExtension3;
				VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension3 = new DynamicResourceExtension(), new Uri("Coding\\Pages\\CodingWithColorSelectionPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 106, 41);
				Label label;
				VisualDiagnostics.RegisterSourceInfo(label = new Label(), new Uri("Coding\\Pages\\CodingWithColorSelectionPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 101, 38);
				BindingExtension bindingExtension2;
				VisualDiagnostics.RegisterSourceInfo(bindingExtension2 = new BindingExtension(), new Uri("Coding\\Pages\\CodingWithColorSelectionPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 115, 41);
				BoxView boxView;
				VisualDiagnostics.RegisterSourceInfo(boxView = new BoxView(), new Uri("Coding\\Pages\\CodingWithColorSelectionPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 109, 38);
				Grid grid;
				VisualDiagnostics.RegisterSourceInfo(grid = new Grid(), new Uri("Coding\\Pages\\CodingWithColorSelectionPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 96, 34);
				Frame frame;
				VisualDiagnostics.RegisterSourceInfo(frame = new Frame(), new Uri("Coding\\Pages\\CodingWithColorSelectionPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 90, 30);
				ViewCell viewCell;
				VisualDiagnostics.RegisterSourceInfo(viewCell = new ViewCell(), new Uri("Coding\\Pages\\CodingWithColorSelectionPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 89, 26);
				NameScope nameScope = new NameScope();
				NameScope.SetNameScope(viewCell, nameScope);
				frame.SetValue(Layout.PaddingProperty, new Thickness(3.0));
				dynamicResourceExtension.Key = "TextColor";
				IMarkupExtension<DynamicResource> markupExtension = dynamicResourceExtension;
				XamlServiceProvider xamlServiceProvider = new XamlServiceProvider();
				Type typeFromHandle = typeof(IProvideValueTarget);
				int num;
				object[] array = new object[(num = this.parentValues.Length) + 2];
				Array.Copy(this.parentValues, 0, array, 2, num);
				object[] array2 = array;
				array2[0] = frame;
				array2[1] = viewCell;
				object obj;
				xamlServiceProvider.Add(typeFromHandle, obj = new SimpleValueTargetProvider(array2, VisualElement.BackgroundColorProperty, nameScope));
				xamlServiceProvider.Add(typeof(IReferenceProvider), obj);
				Type typeFromHandle2 = typeof(IXamlTypeResolver);
				XmlNamespaceResolver xmlNamespaceResolver = new XmlNamespaceResolver();
				xmlNamespaceResolver.Add("", "http://xamarin.com/schemas/2014/forms");
				xmlNamespaceResolver.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
				xmlNamespaceResolver.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
				xmlNamespaceResolver.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
				xmlNamespaceResolver.Add("local", "clr-namespace:CarScannerXamarinForms");
				xmlNamespaceResolver.Add("settings", "clr-namespace:CarScannerXamarinForms.Settings");
				xamlServiceProvider.Add(typeFromHandle2, new XamlTypeResolver(xmlNamespaceResolver, typeof(CodingWithColorSelectionPage.<InitializeComponent>_anonXamlCDataTemplate_9).GetTypeInfo().Assembly));
				xamlServiceProvider.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(92, 33)));
				DynamicResource dynamicResource = markupExtension.ProvideValue(xamlServiceProvider);
				frame.SetDynamicResource(VisualElement.BackgroundColorProperty, dynamicResource.Key);
				frame.SetValue(Frame.CornerRadiusProperty, 0f);
				frame.SetValue(Frame.HasShadowProperty, false);
				dynamicResourceExtension2.Key = "TextColor";
				IMarkupExtension<DynamicResource> markupExtension2 = dynamicResourceExtension2;
				XamlServiceProvider xamlServiceProvider2 = new XamlServiceProvider();
				Type typeFromHandle3 = typeof(IProvideValueTarget);
				int num2;
				object[] array3 = new object[(num2 = this.parentValues.Length) + 2];
				Array.Copy(this.parentValues, 0, array3, 2, num2);
				object[] array4 = array3;
				array4[0] = frame;
				array4[1] = viewCell;
				object obj2;
				xamlServiceProvider2.Add(typeFromHandle3, obj2 = new SimpleValueTargetProvider(array4, Frame.OutlineColorProperty, nameScope));
				xamlServiceProvider2.Add(typeof(IReferenceProvider), obj2);
				Type typeFromHandle4 = typeof(IXamlTypeResolver);
				XmlNamespaceResolver xmlNamespaceResolver2 = new XmlNamespaceResolver();
				xmlNamespaceResolver2.Add("", "http://xamarin.com/schemas/2014/forms");
				xmlNamespaceResolver2.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
				xmlNamespaceResolver2.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
				xmlNamespaceResolver2.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
				xmlNamespaceResolver2.Add("local", "clr-namespace:CarScannerXamarinForms");
				xmlNamespaceResolver2.Add("settings", "clr-namespace:CarScannerXamarinForms.Settings");
				xamlServiceProvider2.Add(typeFromHandle4, new XamlTypeResolver(xmlNamespaceResolver2, typeof(CodingWithColorSelectionPage.<InitializeComponent>_anonXamlCDataTemplate_9).GetTypeInfo().Assembly));
				xamlServiceProvider2.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(95, 33)));
				DynamicResource dynamicResource2 = markupExtension2.ProvideValue(xamlServiceProvider2);
				frame.SetDynamicResource(Frame.OutlineColorProperty, dynamicResource2.Key);
				columnDefinition.SetValue(ColumnDefinition.WidthProperty, new GridLengthTypeConverter().ConvertFromInvariantString("0.2*"));
				grid.GetValue(Grid.ColumnDefinitionsProperty).Add(columnDefinition);
				columnDefinition2.SetValue(ColumnDefinition.WidthProperty, new GridLengthTypeConverter().ConvertFromInvariantString("0.8*"));
				grid.GetValue(Grid.ColumnDefinitionsProperty).Add(columnDefinition2);
				label.SetValue(Grid.ColumnProperty, 0);
				label.SetValue(View.HorizontalOptionsProperty, LayoutOptions.Center);
				label.SetValue(Label.HorizontalTextAlignmentProperty, new TextAlignmentConverter().ConvertFromInvariantString("Center"));
				bindingExtension.Path = "Number";
				BindingBase bindingBase = bindingExtension.ProvideValue(null);
				label.SetBinding(Label.TextProperty, bindingBase);
				dynamicResourceExtension3.Key = "BackgroundColor";
				IMarkupExtension<DynamicResource> markupExtension3 = dynamicResourceExtension3;
				XamlServiceProvider xamlServiceProvider3 = new XamlServiceProvider();
				Type typeFromHandle5 = typeof(IProvideValueTarget);
				int num3;
				object[] array5 = new object[(num3 = this.parentValues.Length) + 4];
				Array.Copy(this.parentValues, 0, array5, 4, num3);
				object[] array6 = array5;
				array6[0] = label;
				array6[1] = grid;
				array6[2] = frame;
				array6[3] = viewCell;
				object obj3;
				xamlServiceProvider3.Add(typeFromHandle5, obj3 = new SimpleValueTargetProvider(array6, Label.TextColorProperty, nameScope));
				xamlServiceProvider3.Add(typeof(IReferenceProvider), obj3);
				Type typeFromHandle6 = typeof(IXamlTypeResolver);
				XmlNamespaceResolver xmlNamespaceResolver3 = new XmlNamespaceResolver();
				xmlNamespaceResolver3.Add("", "http://xamarin.com/schemas/2014/forms");
				xmlNamespaceResolver3.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
				xmlNamespaceResolver3.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
				xmlNamespaceResolver3.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
				xmlNamespaceResolver3.Add("local", "clr-namespace:CarScannerXamarinForms");
				xmlNamespaceResolver3.Add("settings", "clr-namespace:CarScannerXamarinForms.Settings");
				xamlServiceProvider3.Add(typeFromHandle6, new XamlTypeResolver(xmlNamespaceResolver3, typeof(CodingWithColorSelectionPage.<InitializeComponent>_anonXamlCDataTemplate_9).GetTypeInfo().Assembly));
				xamlServiceProvider3.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(106, 41)));
				DynamicResource dynamicResource3 = markupExtension3.ProvideValue(xamlServiceProvider3);
				label.SetDynamicResource(Label.TextColorProperty, dynamicResource3.Key);
				label.SetValue(View.VerticalOptionsProperty, LayoutOptions.Center);
				label.SetValue(Label.VerticalTextAlignmentProperty, new TextAlignmentConverter().ConvertFromInvariantString("Center"));
				grid.Children.Add(label);
				boxView.SetValue(Grid.ColumnProperty, 1);
				boxView.SetValue(View.MarginProperty, new Thickness(0.0));
				boxView.SetValue(VisualElement.HeightRequestProperty, 50.0);
				boxView.SetValue(View.HorizontalOptionsProperty, LayoutOptions.Fill);
				boxView.SetValue(View.VerticalOptionsProperty, LayoutOptions.Fill);
				bindingExtension2.Path = "Color";
				BindingBase bindingBase2 = bindingExtension2.ProvideValue(null);
				boxView.SetBinding(BoxView.ColorProperty, bindingBase2);
				grid.Children.Add(boxView);
				frame.SetValue(ContentView.ContentProperty, grid);
				viewCell.View = frame;
				return viewCell;
			}

			// Token: 0x04002EA0 RID: 11936
			internal object[] parentValues;

			// Token: 0x04002EA1 RID: 11937
			internal CodingWithColorSelectionPage root;
		}
	}
}
