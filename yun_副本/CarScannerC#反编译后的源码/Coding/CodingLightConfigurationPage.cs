using System;
using System.CodeDom.Compiler;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using CarScannerXamarinForms.Common;
using CarScannerXamarinForms.Common.XAMLConverters;
using CarScannerXamarinForms.InApp;
using CarScannerXamarinForms.Pages;
using CarScannerXamarinForms.Settings;
using CarScannerXamarinForms.UserControls;
using Xamarin.Forms;
using Xamarin.Forms.Internals;
using Xamarin.Forms.PlatformConfiguration.iOSSpecific;
using Xamarin.Forms.Xaml;
using Xamarin.Forms.Xaml.Diagnostics;
using Xamarin.Forms.Xaml.Internals;

namespace CarScannerXamarinForms.Coding
{
	// Token: 0x020008F7 RID: 2295
	[XamlCompilation(2)]
	[XamlFilePath("Coding\\Pages\\CodingLightConfigurationPage.xaml")]
	public class CodingLightConfigurationPage : ContentPage
	{
		// Token: 0x06004D45 RID: 19781 RVA: 0x00390C57 File Offset: 0x0038EE57
		public CodingLightConfigurationPage(ICodingContainer coding)
		{
			this.InitializeComponent();
			this.coding = (MQB_LightConfigurationCoding)coding;
			base.BindingContext = this.coding;
			base.Appearing += this.CodingDetailsPage_Appearing;
		}

		// Token: 0x06004D46 RID: 19782 RVA: 0x00390C98 File Offset: 0x0038EE98
		private async void CodingDetailsPage_Appearing(object sender, EventArgs e)
		{
			if (this.first_appearing)
			{
				this.first_appearing = false;
				await this.UpdateState();
			}
		}

		// Token: 0x06004D47 RID: 19783 RVA: 0x00390CD0 File Offset: 0x0038EED0
		public async Task UpdateState()
		{
			this.activityFrame.IsVisible = true;
			TaskAwaiter<CodingRequestResult> taskAwaiter = this.coding.UpdateCurrentState("", null).GetAwaiter();
			if (!taskAwaiter.IsCompleted)
			{
				await taskAwaiter;
				TaskAwaiter<CodingRequestResult> taskAwaiter2;
				taskAwaiter = taskAwaiter2;
				taskAwaiter2 = default(TaskAwaiter<CodingRequestResult>);
			}
			if (taskAwaiter.GetResult() != CodingRequestResult.Success)
			{
				this.labelNotSupported.IsVisible = true;
				this.stackConfiguration.IsEnabled = false;
			}
			else
			{
				this.labelNotSupported.IsVisible = false;
				this.stackConfiguration.IsEnabled = true;
			}
			this.activityFrame.IsVisible = false;
		}

		// Token: 0x06004D48 RID: 19784 RVA: 0x00390D14 File Offset: 0x0038EF14
		private async void BtnUpdateState_Clicked(object sender, EventArgs e)
		{
			this.UpdateState();
		}

		// Token: 0x06004D49 RID: 19785 RVA: 0x00390D4C File Offset: 0x0038EF4C
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
				this.stackConfiguration.IsEnabled = false;
				this.btnSaveNewValue.IsEnabled = false;
				this.btnUpdateState.IsEnabled = false;
				Progress<string> progress = new Progress<string>(delegate(string s)
				{
					Device.BeginInvokeOnMainThread(delegate
					{
						this.activityFrame.Text = s;
					});
				});
				string text = BitHelpers.ByteArrayToHexString(this.coding.LightConfiguration.ApplyToData());
				CodingRequestResult codingRequestResult = await this.coding.Execute(this.entryPassword.Text, text, Translate.GetString("coding_ManualLightCustomization"), progress, null, false);
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
				this.btnSaveNewValue.IsEnabled = true;
				this.btnUpdateState.IsEnabled = true;
				this.activityFrame.Text = ActivityFrame.PLEASE_WAIT_TEXT;
			}
		}

		// Token: 0x06004D4A RID: 19786 RVA: 0x00390D84 File Offset: 0x0038EF84
		private void lbLampType_Tapped(object sender, EventArgs e)
		{
			ItemWithValueSelectorPage itemWithValueSelectorPage = new ItemWithValueSelectorPage(Translate.GetString("coding_LampType"), MQB_LampType.LampTypesList.Select((MQB_LampType x) => x), this.coding.LightConfiguration.LampType, delegate(ValueItemWithTranslation val)
			{
				this.coding.LightConfiguration.LampType = (MQB_LampType)val;
			});
			base.Navigation.PushAsync(itemWithValueSelectorPage);
		}

		// Token: 0x06004D4B RID: 19787 RVA: 0x00390DF4 File Offset: 0x0038EFF4
		private async void lbFunctionSelector_Tapped(object sender, EventArgs e)
		{
			LabelWithBorder labelWithBorder = (LabelWithBorder)sender;
			string tag = labelWithBorder.Tag.ToUpperInvariant();
			Action<ValueItemWithTranslation> action = delegate(ValueItemWithTranslation selected)
			{
				MQB_LightFunction mqb_LightFunction2 = (MQB_LightFunction)selected;
				string tag2 = tag;
				if (tag2 != null)
				{
					int length2 = tag2.Length;
					if (length2 == 1)
					{
						switch (tag2[0])
						{
						case 'A':
							this.coding.LightConfiguration.FunctionA = mqb_LightFunction2;
							return;
						case 'B':
							this.coding.LightConfiguration.FunctionB = mqb_LightFunction2;
							return;
						case 'C':
							this.coding.LightConfiguration.FunctionC = mqb_LightFunction2;
							return;
						case 'D':
							this.coding.LightConfiguration.FunctionD = mqb_LightFunction2;
							return;
						case 'E':
							this.coding.LightConfiguration.FunctionE = mqb_LightFunction2;
							return;
						case 'F':
							this.coding.LightConfiguration.FunctionF = mqb_LightFunction2;
							return;
						case 'G':
							this.coding.LightConfiguration.FunctionG = mqb_LightFunction2;
							return;
						case 'H':
							this.coding.LightConfiguration.FunctionH = mqb_LightFunction2;
							break;
						default:
							return;
						}
					}
				}
			};
			MQB_LightFunction mqb_LightFunction = null;
			string tag3 = tag;
			if (tag3 != null)
			{
				int length = tag3.Length;
				if (length == 1)
				{
					switch (tag3[0])
					{
					case 'A':
						mqb_LightFunction = this.coding.LightConfiguration.FunctionA;
						break;
					case 'B':
						mqb_LightFunction = this.coding.LightConfiguration.FunctionB;
						break;
					case 'C':
						mqb_LightFunction = this.coding.LightConfiguration.FunctionC;
						break;
					case 'D':
						mqb_LightFunction = this.coding.LightConfiguration.FunctionD;
						break;
					case 'E':
						mqb_LightFunction = this.coding.LightConfiguration.FunctionE;
						break;
					case 'F':
						mqb_LightFunction = this.coding.LightConfiguration.FunctionF;
						break;
					case 'G':
						mqb_LightFunction = this.coding.LightConfiguration.FunctionG;
						break;
					case 'H':
						mqb_LightFunction = this.coding.LightConfiguration.FunctionH;
						break;
					}
				}
			}
			ItemWithValueSelectorPage itemWithValueSelectorPage = new ItemWithValueSelectorPage(Translate.GetString("coding_Function") + " " + tag, MQB_LightFunction.LightFunctionsList.Select((MQB_LightFunction x) => x), mqb_LightFunction, action);
			await base.Navigation.PushAsync(itemWithValueSelectorPage);
		}

		// Token: 0x06004D4C RID: 19788 RVA: 0x00390E34 File Offset: 0x0038F034
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private void InitializeComponent()
		{
			if (ResourceLoader.IsEnabled && ResourceLoader.CanProvideContentFor(new ResourceLoader.ResourceLoadingQuery
			{
				AssemblyName = typeof(CodingLightConfigurationPage).GetTypeInfo().Assembly.GetName(),
				ResourcePath = "Coding/Pages/CodingLightConfigurationPage.xaml",
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
			VisualDiagnostics.RegisterSourceInfo(translate = new Translate(), new Uri("Coding\\Pages\\CodingLightConfigurationPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 15, 5);
			DynamicResourceExtension dynamicResourceExtension;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension = new DynamicResourceExtension(), new Uri("Coding\\Pages\\CodingLightConfigurationPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 18, 5);
			EmptyStringToFalseConverter emptyStringToFalseConverter;
			VisualDiagnostics.RegisterSourceInfo(emptyStringToFalseConverter = new EmptyStringToFalseConverter(), new Uri("Coding\\Pages\\CodingLightConfigurationPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 22, 14);
			DimmingDirectionToIntConverter dimmingDirectionToIntConverter;
			VisualDiagnostics.RegisterSourceInfo(dimmingDirectionToIntConverter = new DimmingDirectionToIntConverter(), new Uri("Coding\\Pages\\CodingLightConfigurationPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 23, 14);
			LightControlToIntConverter lightControlToIntConverter;
			VisualDiagnostics.RegisterSourceInfo(lightControlToIntConverter = new LightControlToIntConverter(), new Uri("Coding\\Pages\\CodingLightConfigurationPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 24, 14);
			VagCodingPlatformToTrueConverter vagCodingPlatformToTrueConverter;
			VisualDiagnostics.RegisterSourceInfo(vagCodingPlatformToTrueConverter = new VagCodingPlatformToTrueConverter(), new Uri("Coding\\Pages\\CodingLightConfigurationPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 25, 14);
			ResourceDictionary resourceDictionary;
			VisualDiagnostics.RegisterSourceInfo(resourceDictionary = new ResourceDictionary(), new Uri("Coding\\Pages\\CodingLightConfigurationPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 21, 10);
			RowDefinition rowDefinition;
			VisualDiagnostics.RegisterSourceInfo(rowDefinition = new RowDefinition(), new Uri("Coding\\Pages\\CodingLightConfigurationPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 32, 22);
			DynamicResourceExtension dynamicResourceExtension2;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension2 = new DynamicResourceExtension(), new Uri("Coding\\Pages\\CodingLightConfigurationPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 37, 25);
			BindingExtension bindingExtension;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension = new BindingExtension(), new Uri("Coding\\Pages\\CodingLightConfigurationPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 38, 25);
			Label label;
			VisualDiagnostics.RegisterSourceInfo(label = new Label(), new Uri("Coding\\Pages\\CodingLightConfigurationPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 35, 22);
			BindingExtension bindingExtension2;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension2 = new BindingExtension(), new Uri("Coding\\Pages\\CodingLightConfigurationPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 39, 28);
			Label label2;
			VisualDiagnostics.RegisterSourceInfo(label2 = new Label(), new Uri("Coding\\Pages\\CodingLightConfigurationPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 39, 22);
			Translate translate2;
			VisualDiagnostics.RegisterSourceInfo(translate2 = new Translate(), new Uri("Coding\\Pages\\CodingLightConfigurationPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 44, 25);
			DynamicResourceExtension dynamicResourceExtension3;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension3 = new DynamicResourceExtension(), new Uri("Coding\\Pages\\CodingLightConfigurationPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 45, 25);
			Label label3;
			VisualDiagnostics.RegisterSourceInfo(label3 = new Label(), new Uri("Coding\\Pages\\CodingLightConfigurationPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 41, 22);
			BindingExtension bindingExtension3;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension3 = new BindingExtension(), new Uri("Coding\\Pages\\CodingLightConfigurationPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 49, 25);
			Translate translate3;
			VisualDiagnostics.RegisterSourceInfo(translate3 = new Translate(), new Uri("Coding\\Pages\\CodingLightConfigurationPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 51, 32);
			Label label4;
			VisualDiagnostics.RegisterSourceInfo(label4 = new Label(), new Uri("Coding\\Pages\\CodingLightConfigurationPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 51, 26);
			BindingExtension bindingExtension4;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension4 = new BindingExtension(), new Uri("Coding\\Pages\\CodingLightConfigurationPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 52, 72);
			LabelWithBorder labelWithBorder;
			VisualDiagnostics.RegisterSourceInfo(labelWithBorder = new LabelWithBorder(), new Uri("Coding\\Pages\\CodingLightConfigurationPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 52, 26);
			Translate translate4;
			VisualDiagnostics.RegisterSourceInfo(translate4 = new Translate(), new Uri("Coding\\Pages\\CodingLightConfigurationPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 58, 43);
			Span span;
			VisualDiagnostics.RegisterSourceInfo(span = new Span(), new Uri("Coding\\Pages\\CodingLightConfigurationPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 58, 38);
			Span span2;
			VisualDiagnostics.RegisterSourceInfo(span2 = new Span(), new Uri("Coding\\Pages\\CodingLightConfigurationPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 59, 38);
			FormattedString formattedString;
			VisualDiagnostics.RegisterSourceInfo(formattedString = new FormattedString(), new Uri("Coding\\Pages\\CodingLightConfigurationPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 57, 34);
			Label label5;
			VisualDiagnostics.RegisterSourceInfo(label5 = new Label(), new Uri("Coding\\Pages\\CodingLightConfigurationPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 55, 26);
			BindingExtension bindingExtension5;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension5 = new BindingExtension(), new Uri("Coding\\Pages\\CodingLightConfigurationPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 66, 29);
			LabelWithBorder labelWithBorder2;
			VisualDiagnostics.RegisterSourceInfo(labelWithBorder2 = new LabelWithBorder(), new Uri("Coding\\Pages\\CodingLightConfigurationPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 63, 26);
			Translate translate5;
			VisualDiagnostics.RegisterSourceInfo(translate5 = new Translate(), new Uri("Coding\\Pages\\CodingLightConfigurationPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 72, 43);
			Span span3;
			VisualDiagnostics.RegisterSourceInfo(span3 = new Span(), new Uri("Coding\\Pages\\CodingLightConfigurationPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 72, 38);
			Span span4;
			VisualDiagnostics.RegisterSourceInfo(span4 = new Span(), new Uri("Coding\\Pages\\CodingLightConfigurationPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 73, 38);
			FormattedString formattedString2;
			VisualDiagnostics.RegisterSourceInfo(formattedString2 = new FormattedString(), new Uri("Coding\\Pages\\CodingLightConfigurationPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 71, 34);
			Label label6;
			VisualDiagnostics.RegisterSourceInfo(label6 = new Label(), new Uri("Coding\\Pages\\CodingLightConfigurationPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 69, 26);
			BindingExtension bindingExtension6;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension6 = new BindingExtension(), new Uri("Coding\\Pages\\CodingLightConfigurationPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 80, 29);
			LabelWithBorder labelWithBorder3;
			VisualDiagnostics.RegisterSourceInfo(labelWithBorder3 = new LabelWithBorder(), new Uri("Coding\\Pages\\CodingLightConfigurationPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 77, 26);
			Translate translate6;
			VisualDiagnostics.RegisterSourceInfo(translate6 = new Translate(), new Uri("Coding\\Pages\\CodingLightConfigurationPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 84, 43);
			Span span5;
			VisualDiagnostics.RegisterSourceInfo(span5 = new Span(), new Uri("Coding\\Pages\\CodingLightConfigurationPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 84, 38);
			Span span6;
			VisualDiagnostics.RegisterSourceInfo(span6 = new Span(), new Uri("Coding\\Pages\\CodingLightConfigurationPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 85, 38);
			FormattedString formattedString3;
			VisualDiagnostics.RegisterSourceInfo(formattedString3 = new FormattedString(), new Uri("Coding\\Pages\\CodingLightConfigurationPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 83, 34);
			Label label7;
			VisualDiagnostics.RegisterSourceInfo(label7 = new Label(), new Uri("Coding\\Pages\\CodingLightConfigurationPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 81, 26);
			BindingExtension bindingExtension7;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension7 = new BindingExtension(), new Uri("Coding\\Pages\\CodingLightConfigurationPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 93, 29);
			NumericEntryV3 numericEntryV;
			VisualDiagnostics.RegisterSourceInfo(numericEntryV = new NumericEntryV3(), new Uri("Coding\\Pages\\CodingLightConfigurationPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 89, 26);
			Translate translate7;
			VisualDiagnostics.RegisterSourceInfo(translate7 = new Translate(), new Uri("Coding\\Pages\\CodingLightConfigurationPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 94, 32);
			Label label8;
			VisualDiagnostics.RegisterSourceInfo(label8 = new Label(), new Uri("Coding\\Pages\\CodingLightConfigurationPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 94, 26);
			DynamicResourceExtension dynamicResourceExtension4;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension4 = new DynamicResourceExtension(), new Uri("Coding\\Pages\\CodingLightConfigurationPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 96, 29);
			string[] lightControlList;
			VisualDiagnostics.RegisterSourceInfo(lightControlList = MQB_LightConfiguration.LightControlList, new Uri("Coding\\Pages\\CodingLightConfigurationPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 97, 29);
			BindingExtension bindingExtension8;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension8 = new BindingExtension(), new Uri("Coding\\Pages\\CodingLightConfigurationPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 97, 29);
			StaticResourceExtension staticResourceExtension;
			VisualDiagnostics.RegisterSourceInfo(staticResourceExtension = new StaticResourceExtension(), new Uri("Coding\\Pages\\CodingLightConfigurationPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 98, 29);
			BindingExtension bindingExtension9;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension9 = new BindingExtension(), new Uri("Coding\\Pages\\CodingLightConfigurationPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 98, 29);
			Picker picker;
			VisualDiagnostics.RegisterSourceInfo(picker = new Picker(), new Uri("Coding\\Pages\\CodingLightConfigurationPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 95, 26);
			Translate translate8;
			VisualDiagnostics.RegisterSourceInfo(translate8 = new Translate(), new Uri("Coding\\Pages\\CodingLightConfigurationPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 104, 43);
			Span span7;
			VisualDiagnostics.RegisterSourceInfo(span7 = new Span(), new Uri("Coding\\Pages\\CodingLightConfigurationPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 104, 38);
			Span span8;
			VisualDiagnostics.RegisterSourceInfo(span8 = new Span(), new Uri("Coding\\Pages\\CodingLightConfigurationPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 105, 38);
			FormattedString formattedString4;
			VisualDiagnostics.RegisterSourceInfo(formattedString4 = new FormattedString(), new Uri("Coding\\Pages\\CodingLightConfigurationPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 103, 34);
			Label label9;
			VisualDiagnostics.RegisterSourceInfo(label9 = new Label(), new Uri("Coding\\Pages\\CodingLightConfigurationPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 101, 26);
			BindingExtension bindingExtension10;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension10 = new BindingExtension(), new Uri("Coding\\Pages\\CodingLightConfigurationPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 112, 29);
			LabelWithBorder labelWithBorder4;
			VisualDiagnostics.RegisterSourceInfo(labelWithBorder4 = new LabelWithBorder(), new Uri("Coding\\Pages\\CodingLightConfigurationPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 109, 26);
			Translate translate9;
			VisualDiagnostics.RegisterSourceInfo(translate9 = new Translate(), new Uri("Coding\\Pages\\CodingLightConfigurationPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 117, 43);
			Span span9;
			VisualDiagnostics.RegisterSourceInfo(span9 = new Span(), new Uri("Coding\\Pages\\CodingLightConfigurationPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 117, 38);
			Span span10;
			VisualDiagnostics.RegisterSourceInfo(span10 = new Span(), new Uri("Coding\\Pages\\CodingLightConfigurationPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 118, 38);
			FormattedString formattedString5;
			VisualDiagnostics.RegisterSourceInfo(formattedString5 = new FormattedString(), new Uri("Coding\\Pages\\CodingLightConfigurationPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 116, 34);
			Label label10;
			VisualDiagnostics.RegisterSourceInfo(label10 = new Label(), new Uri("Coding\\Pages\\CodingLightConfigurationPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 114, 26);
			BindingExtension bindingExtension11;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension11 = new BindingExtension(), new Uri("Coding\\Pages\\CodingLightConfigurationPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 125, 29);
			LabelWithBorder labelWithBorder5;
			VisualDiagnostics.RegisterSourceInfo(labelWithBorder5 = new LabelWithBorder(), new Uri("Coding\\Pages\\CodingLightConfigurationPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 122, 26);
			Translate translate10;
			VisualDiagnostics.RegisterSourceInfo(translate10 = new Translate(), new Uri("Coding\\Pages\\CodingLightConfigurationPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 130, 43);
			Span span11;
			VisualDiagnostics.RegisterSourceInfo(span11 = new Span(), new Uri("Coding\\Pages\\CodingLightConfigurationPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 130, 38);
			Span span12;
			VisualDiagnostics.RegisterSourceInfo(span12 = new Span(), new Uri("Coding\\Pages\\CodingLightConfigurationPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 131, 38);
			FormattedString formattedString6;
			VisualDiagnostics.RegisterSourceInfo(formattedString6 = new FormattedString(), new Uri("Coding\\Pages\\CodingLightConfigurationPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 129, 34);
			Label label11;
			VisualDiagnostics.RegisterSourceInfo(label11 = new Label(), new Uri("Coding\\Pages\\CodingLightConfigurationPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 127, 26);
			BindingExtension bindingExtension12;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension12 = new BindingExtension(), new Uri("Coding\\Pages\\CodingLightConfigurationPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 139, 29);
			NumericEntryV3 numericEntryV2;
			VisualDiagnostics.RegisterSourceInfo(numericEntryV2 = new NumericEntryV3(), new Uri("Coding\\Pages\\CodingLightConfigurationPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 135, 26);
			Translate translate11;
			VisualDiagnostics.RegisterSourceInfo(translate11 = new Translate(), new Uri("Coding\\Pages\\CodingLightConfigurationPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 143, 43);
			Span span13;
			VisualDiagnostics.RegisterSourceInfo(span13 = new Span(), new Uri("Coding\\Pages\\CodingLightConfigurationPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 143, 38);
			Span span14;
			VisualDiagnostics.RegisterSourceInfo(span14 = new Span(), new Uri("Coding\\Pages\\CodingLightConfigurationPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 144, 38);
			FormattedString formattedString7;
			VisualDiagnostics.RegisterSourceInfo(formattedString7 = new FormattedString(), new Uri("Coding\\Pages\\CodingLightConfigurationPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 142, 34);
			Label label12;
			VisualDiagnostics.RegisterSourceInfo(label12 = new Label(), new Uri("Coding\\Pages\\CodingLightConfigurationPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 140, 26);
			DynamicResourceExtension dynamicResourceExtension5;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension5 = new DynamicResourceExtension(), new Uri("Coding\\Pages\\CodingLightConfigurationPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 149, 29);
			string[] dimmingDirectionList;
			VisualDiagnostics.RegisterSourceInfo(dimmingDirectionList = MQB_LightConfiguration.DimmingDirectionList, new Uri("Coding\\Pages\\CodingLightConfigurationPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 150, 29);
			BindingExtension bindingExtension13;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension13 = new BindingExtension(), new Uri("Coding\\Pages\\CodingLightConfigurationPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 150, 29);
			StaticResourceExtension staticResourceExtension2;
			VisualDiagnostics.RegisterSourceInfo(staticResourceExtension2 = new StaticResourceExtension(), new Uri("Coding\\Pages\\CodingLightConfigurationPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 151, 29);
			BindingExtension bindingExtension14;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension14 = new BindingExtension(), new Uri("Coding\\Pages\\CodingLightConfigurationPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 151, 29);
			Picker picker2;
			VisualDiagnostics.RegisterSourceInfo(picker2 = new Picker(), new Uri("Coding\\Pages\\CodingLightConfigurationPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 148, 26);
			Translate translate12;
			VisualDiagnostics.RegisterSourceInfo(translate12 = new Translate(), new Uri("Coding\\Pages\\CodingLightConfigurationPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 158, 43);
			Span span15;
			VisualDiagnostics.RegisterSourceInfo(span15 = new Span(), new Uri("Coding\\Pages\\CodingLightConfigurationPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 158, 38);
			Span span16;
			VisualDiagnostics.RegisterSourceInfo(span16 = new Span(), new Uri("Coding\\Pages\\CodingLightConfigurationPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 159, 38);
			FormattedString formattedString8;
			VisualDiagnostics.RegisterSourceInfo(formattedString8 = new FormattedString(), new Uri("Coding\\Pages\\CodingLightConfigurationPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 157, 34);
			Label label13;
			VisualDiagnostics.RegisterSourceInfo(label13 = new Label(), new Uri("Coding\\Pages\\CodingLightConfigurationPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 155, 26);
			BindingExtension bindingExtension15;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension15 = new BindingExtension(), new Uri("Coding\\Pages\\CodingLightConfigurationPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 166, 29);
			LabelWithBorder labelWithBorder6;
			VisualDiagnostics.RegisterSourceInfo(labelWithBorder6 = new LabelWithBorder(), new Uri("Coding\\Pages\\CodingLightConfigurationPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 163, 26);
			Translate translate13;
			VisualDiagnostics.RegisterSourceInfo(translate13 = new Translate(), new Uri("Coding\\Pages\\CodingLightConfigurationPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 170, 43);
			Span span17;
			VisualDiagnostics.RegisterSourceInfo(span17 = new Span(), new Uri("Coding\\Pages\\CodingLightConfigurationPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 170, 38);
			Span span18;
			VisualDiagnostics.RegisterSourceInfo(span18 = new Span(), new Uri("Coding\\Pages\\CodingLightConfigurationPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 171, 38);
			FormattedString formattedString9;
			VisualDiagnostics.RegisterSourceInfo(formattedString9 = new FormattedString(), new Uri("Coding\\Pages\\CodingLightConfigurationPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 169, 34);
			Label label14;
			VisualDiagnostics.RegisterSourceInfo(label14 = new Label(), new Uri("Coding\\Pages\\CodingLightConfigurationPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 167, 26);
			BindingExtension bindingExtension16;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension16 = new BindingExtension(), new Uri("Coding\\Pages\\CodingLightConfigurationPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 178, 29);
			LabelWithBorder labelWithBorder7;
			VisualDiagnostics.RegisterSourceInfo(labelWithBorder7 = new LabelWithBorder(), new Uri("Coding\\Pages\\CodingLightConfigurationPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 175, 26);
			Translate translate14;
			VisualDiagnostics.RegisterSourceInfo(translate14 = new Translate(), new Uri("Coding\\Pages\\CodingLightConfigurationPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 184, 43);
			Span span19;
			VisualDiagnostics.RegisterSourceInfo(span19 = new Span(), new Uri("Coding\\Pages\\CodingLightConfigurationPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 184, 38);
			Span span20;
			VisualDiagnostics.RegisterSourceInfo(span20 = new Span(), new Uri("Coding\\Pages\\CodingLightConfigurationPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 185, 38);
			FormattedString formattedString10;
			VisualDiagnostics.RegisterSourceInfo(formattedString10 = new FormattedString(), new Uri("Coding\\Pages\\CodingLightConfigurationPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 183, 34);
			Label label15;
			VisualDiagnostics.RegisterSourceInfo(label15 = new Label(), new Uri("Coding\\Pages\\CodingLightConfigurationPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 181, 26);
			BindingExtension bindingExtension17;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension17 = new BindingExtension(), new Uri("Coding\\Pages\\CodingLightConfigurationPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 193, 29);
			NumericEntryV3 numericEntryV3;
			VisualDiagnostics.RegisterSourceInfo(numericEntryV3 = new NumericEntryV3(), new Uri("Coding\\Pages\\CodingLightConfigurationPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 189, 26);
			Translate translate15;
			VisualDiagnostics.RegisterSourceInfo(translate15 = new Translate(), new Uri("Coding\\Pages\\CodingLightConfigurationPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 197, 43);
			Span span21;
			VisualDiagnostics.RegisterSourceInfo(span21 = new Span(), new Uri("Coding\\Pages\\CodingLightConfigurationPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 197, 38);
			Span span22;
			VisualDiagnostics.RegisterSourceInfo(span22 = new Span(), new Uri("Coding\\Pages\\CodingLightConfigurationPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 198, 38);
			FormattedString formattedString11;
			VisualDiagnostics.RegisterSourceInfo(formattedString11 = new FormattedString(), new Uri("Coding\\Pages\\CodingLightConfigurationPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 196, 34);
			Label label16;
			VisualDiagnostics.RegisterSourceInfo(label16 = new Label(), new Uri("Coding\\Pages\\CodingLightConfigurationPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 194, 26);
			DynamicResourceExtension dynamicResourceExtension6;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension6 = new DynamicResourceExtension(), new Uri("Coding\\Pages\\CodingLightConfigurationPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 203, 29);
			string[] dimmingDirectionList2;
			VisualDiagnostics.RegisterSourceInfo(dimmingDirectionList2 = MQB_LightConfiguration.DimmingDirectionList, new Uri("Coding\\Pages\\CodingLightConfigurationPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 204, 29);
			BindingExtension bindingExtension18;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension18 = new BindingExtension(), new Uri("Coding\\Pages\\CodingLightConfigurationPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 204, 29);
			StaticResourceExtension staticResourceExtension3;
			VisualDiagnostics.RegisterSourceInfo(staticResourceExtension3 = new StaticResourceExtension(), new Uri("Coding\\Pages\\CodingLightConfigurationPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 205, 29);
			BindingExtension bindingExtension19;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension19 = new BindingExtension(), new Uri("Coding\\Pages\\CodingLightConfigurationPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 205, 29);
			Picker picker3;
			VisualDiagnostics.RegisterSourceInfo(picker3 = new Picker(), new Uri("Coding\\Pages\\CodingLightConfigurationPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 202, 26);
			Translate translate16;
			VisualDiagnostics.RegisterSourceInfo(translate16 = new Translate(), new Uri("Coding\\Pages\\CodingLightConfigurationPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 214, 43);
			Span span23;
			VisualDiagnostics.RegisterSourceInfo(span23 = new Span(), new Uri("Coding\\Pages\\CodingLightConfigurationPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 214, 38);
			Span span24;
			VisualDiagnostics.RegisterSourceInfo(span24 = new Span(), new Uri("Coding\\Pages\\CodingLightConfigurationPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 215, 38);
			FormattedString formattedString12;
			VisualDiagnostics.RegisterSourceInfo(formattedString12 = new FormattedString(), new Uri("Coding\\Pages\\CodingLightConfigurationPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 213, 34);
			Label label17;
			VisualDiagnostics.RegisterSourceInfo(label17 = new Label(), new Uri("Coding\\Pages\\CodingLightConfigurationPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 211, 26);
			BindingExtension bindingExtension20;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension20 = new BindingExtension(), new Uri("Coding\\Pages\\CodingLightConfigurationPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 222, 29);
			LabelWithBorder labelWithBorder8;
			VisualDiagnostics.RegisterSourceInfo(labelWithBorder8 = new LabelWithBorder(), new Uri("Coding\\Pages\\CodingLightConfigurationPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 219, 26);
			Translate translate17;
			VisualDiagnostics.RegisterSourceInfo(translate17 = new Translate(), new Uri("Coding\\Pages\\CodingLightConfigurationPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 227, 43);
			Span span25;
			VisualDiagnostics.RegisterSourceInfo(span25 = new Span(), new Uri("Coding\\Pages\\CodingLightConfigurationPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 227, 38);
			Span span26;
			VisualDiagnostics.RegisterSourceInfo(span26 = new Span(), new Uri("Coding\\Pages\\CodingLightConfigurationPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 228, 38);
			FormattedString formattedString13;
			VisualDiagnostics.RegisterSourceInfo(formattedString13 = new FormattedString(), new Uri("Coding\\Pages\\CodingLightConfigurationPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 226, 34);
			Label label18;
			VisualDiagnostics.RegisterSourceInfo(label18 = new Label(), new Uri("Coding\\Pages\\CodingLightConfigurationPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 224, 26);
			BindingExtension bindingExtension21;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension21 = new BindingExtension(), new Uri("Coding\\Pages\\CodingLightConfigurationPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 235, 29);
			LabelWithBorder labelWithBorder9;
			VisualDiagnostics.RegisterSourceInfo(labelWithBorder9 = new LabelWithBorder(), new Uri("Coding\\Pages\\CodingLightConfigurationPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 232, 26);
			Translate translate18;
			VisualDiagnostics.RegisterSourceInfo(translate18 = new Translate(), new Uri("Coding\\Pages\\CodingLightConfigurationPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 240, 43);
			Span span27;
			VisualDiagnostics.RegisterSourceInfo(span27 = new Span(), new Uri("Coding\\Pages\\CodingLightConfigurationPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 240, 38);
			Span span28;
			VisualDiagnostics.RegisterSourceInfo(span28 = new Span(), new Uri("Coding\\Pages\\CodingLightConfigurationPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 241, 38);
			FormattedString formattedString14;
			VisualDiagnostics.RegisterSourceInfo(formattedString14 = new FormattedString(), new Uri("Coding\\Pages\\CodingLightConfigurationPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 239, 34);
			Label label19;
			VisualDiagnostics.RegisterSourceInfo(label19 = new Label(), new Uri("Coding\\Pages\\CodingLightConfigurationPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 237, 26);
			BindingExtension bindingExtension22;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension22 = new BindingExtension(), new Uri("Coding\\Pages\\CodingLightConfigurationPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 249, 29);
			NumericEntryV3 numericEntryV4;
			VisualDiagnostics.RegisterSourceInfo(numericEntryV4 = new NumericEntryV3(), new Uri("Coding\\Pages\\CodingLightConfigurationPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 245, 26);
			Translate translate19;
			VisualDiagnostics.RegisterSourceInfo(translate19 = new Translate(), new Uri("Coding\\Pages\\CodingLightConfigurationPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 253, 43);
			Span span29;
			VisualDiagnostics.RegisterSourceInfo(span29 = new Span(), new Uri("Coding\\Pages\\CodingLightConfigurationPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 253, 38);
			Span span30;
			VisualDiagnostics.RegisterSourceInfo(span30 = new Span(), new Uri("Coding\\Pages\\CodingLightConfigurationPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 254, 38);
			FormattedString formattedString15;
			VisualDiagnostics.RegisterSourceInfo(formattedString15 = new FormattedString(), new Uri("Coding\\Pages\\CodingLightConfigurationPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 252, 34);
			Label label20;
			VisualDiagnostics.RegisterSourceInfo(label20 = new Label(), new Uri("Coding\\Pages\\CodingLightConfigurationPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 250, 26);
			DynamicResourceExtension dynamicResourceExtension7;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension7 = new DynamicResourceExtension(), new Uri("Coding\\Pages\\CodingLightConfigurationPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 259, 29);
			string[] dimmingDirectionList3;
			VisualDiagnostics.RegisterSourceInfo(dimmingDirectionList3 = MQB_LightConfiguration.DimmingDirectionList, new Uri("Coding\\Pages\\CodingLightConfigurationPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 260, 29);
			BindingExtension bindingExtension23;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension23 = new BindingExtension(), new Uri("Coding\\Pages\\CodingLightConfigurationPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 260, 29);
			StaticResourceExtension staticResourceExtension4;
			VisualDiagnostics.RegisterSourceInfo(staticResourceExtension4 = new StaticResourceExtension(), new Uri("Coding\\Pages\\CodingLightConfigurationPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 261, 29);
			BindingExtension bindingExtension24;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension24 = new BindingExtension(), new Uri("Coding\\Pages\\CodingLightConfigurationPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 261, 29);
			Picker picker4;
			VisualDiagnostics.RegisterSourceInfo(picker4 = new Picker(), new Uri("Coding\\Pages\\CodingLightConfigurationPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 258, 26);
			StackLayout stackLayout;
			VisualDiagnostics.RegisterSourceInfo(stackLayout = new StackLayout(), new Uri("Coding\\Pages\\CodingLightConfigurationPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 47, 22);
			BindingExtension bindingExtension25;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension25 = new BindingExtension(), new Uri("Coding\\Pages\\CodingLightConfigurationPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 266, 34);
			Translate translate20;
			VisualDiagnostics.RegisterSourceInfo(translate20 = new Translate(), new Uri("Coding\\Pages\\CodingLightConfigurationPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 267, 32);
			Label label21;
			VisualDiagnostics.RegisterSourceInfo(label21 = new Label(), new Uri("Coding\\Pages\\CodingLightConfigurationPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 267, 26);
			Translate translate21;
			VisualDiagnostics.RegisterSourceInfo(translate21 = new Translate(), new Uri("Coding\\Pages\\CodingLightConfigurationPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 270, 29);
			BindingExtension bindingExtension26;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension26 = new BindingExtension(), new Uri("Coding\\Pages\\CodingLightConfigurationPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 271, 29);
			Entry entry;
			VisualDiagnostics.RegisterSourceInfo(entry = new Entry(), new Uri("Coding\\Pages\\CodingLightConfigurationPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 268, 26);
			StaticResourceExtension staticResourceExtension5;
			VisualDiagnostics.RegisterSourceInfo(staticResourceExtension5 = new StaticResourceExtension(), new Uri("Coding\\Pages\\CodingLightConfigurationPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 272, 56);
			BindingExtension bindingExtension27;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension27 = new BindingExtension(), new Uri("Coding\\Pages\\CodingLightConfigurationPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 272, 56);
			Translate translate22;
			VisualDiagnostics.RegisterSourceInfo(translate22 = new Translate(), new Uri("Coding\\Pages\\CodingLightConfigurationPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 275, 43);
			Span span31;
			VisualDiagnostics.RegisterSourceInfo(span31 = new Span(), new Uri("Coding\\Pages\\CodingLightConfigurationPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 275, 38);
			BindingExtension bindingExtension28;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension28 = new BindingExtension(), new Uri("Coding\\Pages\\CodingLightConfigurationPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 276, 43);
			Span span32;
			VisualDiagnostics.RegisterSourceInfo(span32 = new Span(), new Uri("Coding\\Pages\\CodingLightConfigurationPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 276, 38);
			FormattedString formattedString16;
			VisualDiagnostics.RegisterSourceInfo(formattedString16 = new FormattedString(), new Uri("Coding\\Pages\\CodingLightConfigurationPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 274, 34);
			Label label22;
			VisualDiagnostics.RegisterSourceInfo(label22 = new Label(), new Uri("Coding\\Pages\\CodingLightConfigurationPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 272, 26);
			StackLayout stackLayout2;
			VisualDiagnostics.RegisterSourceInfo(stackLayout2 = new StackLayout(), new Uri("Coding\\Pages\\CodingLightConfigurationPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 266, 22);
			Translate translate23;
			VisualDiagnostics.RegisterSourceInfo(translate23 = new Translate(), new Uri("Coding\\Pages\\CodingLightConfigurationPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 286, 25);
			Button button;
			VisualDiagnostics.RegisterSourceInfo(button = new Button(), new Uri("Coding\\Pages\\CodingLightConfigurationPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 283, 22);
			Translate translate24;
			VisualDiagnostics.RegisterSourceInfo(translate24 = new Translate(), new Uri("Coding\\Pages\\CodingLightConfigurationPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 291, 25);
			Button button2;
			VisualDiagnostics.RegisterSourceInfo(button2 = new Button(), new Uri("Coding\\Pages\\CodingLightConfigurationPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 288, 22);
			SharedSettings sharedSettings;
			VisualDiagnostics.RegisterSourceInfo(sharedSettings = SharedSettings.Current, new Uri("Coding\\Pages\\CodingLightConfigurationPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 293, 25);
			StaticResourceExtension staticResourceExtension6;
			VisualDiagnostics.RegisterSourceInfo(staticResourceExtension6 = new StaticResourceExtension(), new Uri("Coding\\Pages\\CodingLightConfigurationPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 295, 25);
			BindingExtension bindingExtension29;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension29 = new BindingExtension(), new Uri("Coding\\Pages\\CodingLightConfigurationPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 295, 25);
			Translate translate25;
			VisualDiagnostics.RegisterSourceInfo(translate25 = new Translate(), new Uri("Coding\\Pages\\CodingLightConfigurationPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 296, 25);
			Label label23;
			VisualDiagnostics.RegisterSourceInfo(label23 = new Label(), new Uri("Coding\\Pages\\CodingLightConfigurationPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 292, 22);
			SharedSettings sharedSettings2;
			VisualDiagnostics.RegisterSourceInfo(sharedSettings2 = SharedSettings.Current, new Uri("Coding\\Pages\\CodingLightConfigurationPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 298, 25);
			BindingExtension bindingExtension30;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension30 = new BindingExtension(), new Uri("Coding\\Pages\\CodingLightConfigurationPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 299, 25);
			BindingExtension bindingExtension31;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension31 = new BindingExtension(), new Uri("Coding\\Pages\\CodingLightConfigurationPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 300, 25);
			Translate translate26;
			VisualDiagnostics.RegisterSourceInfo(translate26 = new Translate(), new Uri("Coding\\Pages\\CodingLightConfigurationPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 301, 25);
			LabelSwitch labelSwitch;
			VisualDiagnostics.RegisterSourceInfo(labelSwitch = new LabelSwitch(), new Uri("Coding\\Pages\\CodingLightConfigurationPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 297, 22);
			StackLayout stackLayout3;
			VisualDiagnostics.RegisterSourceInfo(stackLayout3 = new StackLayout(), new Uri("Coding\\Pages\\CodingLightConfigurationPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 34, 18);
			ActivityFrame activityFrame;
			VisualDiagnostics.RegisterSourceInfo(activityFrame = new ActivityFrame(), new Uri("Coding\\Pages\\CodingLightConfigurationPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 303, 18);
			Grid grid;
			VisualDiagnostics.RegisterSourceInfo(grid = new Grid(), new Uri("Coding\\Pages\\CodingLightConfigurationPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 30, 14);
			ScrollView scrollView;
			VisualDiagnostics.RegisterSourceInfo(scrollView = new ScrollView(), new Uri("Coding\\Pages\\CodingLightConfigurationPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 29, 10);
			VisualDiagnostics.RegisterSourceInfo(this, new Uri("Coding\\Pages\\CodingLightConfigurationPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 2, 2);
			NameScope nameScope = NameScope.GetNameScope(this) ?? new NameScope();
			NameScope.SetNameScope(this, nameScope);
			nameScope.RegisterName("labelNotSupported", label3);
			if (label3.StyleId == null)
			{
				label3.StyleId = "labelNotSupported";
			}
			nameScope.RegisterName("stackConfiguration", stackLayout);
			if (stackLayout.StyleId == null)
			{
				stackLayout.StyleId = "stackConfiguration";
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
			nameScope.RegisterName("btnUpdateState", button2);
			if (button2.StyleId == null)
			{
				button2.StyleId = "btnUpdateState";
			}
			nameScope.RegisterName("activityFrame", activityFrame);
			if (activityFrame.StyleId == null)
			{
				activityFrame.StyleId = "activityFrame";
			}
			this.labelNotSupported = label3;
			this.stackConfiguration = stackLayout;
			this.entryPassword = entry;
			this.btnSaveNewValue = button;
			this.btnUpdateState = button2;
			this.activityFrame = activityFrame;
			this.Resources = resourceDictionary;
			resourceDictionary.Add("EmptyStringToFalseConverter", emptyStringToFalseConverter);
			resourceDictionary.Add("DimmingDirectionToIntConverter", dimmingDirectionToIntConverter);
			resourceDictionary.Add("LightControlToIntConverter", lightControlToIntConverter);
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
			xamlServiceProvider.Add(typeFromHandle2, new XamlTypeResolver(xmlNamespaceResolver, typeof(CodingLightConfigurationPage).GetTypeInfo().Assembly));
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
			xamlServiceProvider2.Add(typeFromHandle4, new XamlTypeResolver(xmlNamespaceResolver2, typeof(CodingLightConfigurationPage).GetTypeInfo().Assembly));
			xamlServiceProvider2.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(18, 5)));
			DynamicResource dynamicResource = markupExtension2.ProvideValue(xamlServiceProvider2);
			this.SetDynamicResource(VisualElement.BackgroundColorProperty, dynamicResource.Key);
			this.Resources = resourceDictionary;
			scrollView.SetValue(ScrollView.OrientationProperty, 0);
			rowDefinition.SetValue(RowDefinition.HeightProperty, new GridLengthTypeConverter().ConvertFromInvariantString("*"));
			grid.GetValue(Grid.RowDefinitionsProperty).Add(rowDefinition);
			stackLayout3.SetValue(Grid.RowProperty, 0);
			stackLayout3.SetValue(StackLayout.OrientationProperty, 0);
			label.SetValue(Label.FontAttributesProperty, new FontAttributesConverter().ConvertFromInvariantString("Bold"));
			dynamicResourceExtension2.Key = "BaseFontSize+++";
			IMarkupExtension<DynamicResource> markupExtension3 = dynamicResourceExtension2;
			XamlServiceProvider xamlServiceProvider3 = new XamlServiceProvider();
			Type typeFromHandle5 = typeof(IProvideValueTarget);
			object[] array3 = new object[0 + 5];
			array3[0] = label;
			array3[1] = stackLayout3;
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
			xmlNamespaceResolver3.Add("syncfusion", "clr-namespace:Syncfusion.XForms.Buttons;assembly=Syncfusion.Buttons.XForms");
			xmlNamespaceResolver3.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider3.Add(typeFromHandle6, new XamlTypeResolver(xmlNamespaceResolver3, typeof(CodingLightConfigurationPage).GetTypeInfo().Assembly));
			xamlServiceProvider3.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(37, 25)));
			DynamicResource dynamicResource2 = markupExtension3.ProvideValue(xamlServiceProvider3);
			label.SetDynamicResource(Label.FontSizeProperty, dynamicResource2.Key);
			bindingExtension.Path = "Name";
			BindingBase bindingBase = bindingExtension.ProvideValue(null);
			label.SetBinding(Label.TextProperty, bindingBase);
			stackLayout3.Children.Add(label);
			bindingExtension2.Path = "Description";
			BindingBase bindingBase2 = bindingExtension2.ProvideValue(null);
			label2.SetBinding(Label.TextProperty, bindingBase2);
			stackLayout3.Children.Add(label2);
			label3.SetValue(VisualElement.IsVisibleProperty, new VisualElement.VisibilityConverter().ConvertFromInvariantString("False"));
			translate2.Text = "coding_NotSupported";
			IMarkupExtension markupExtension4 = translate2;
			XamlServiceProvider xamlServiceProvider4 = new XamlServiceProvider();
			Type typeFromHandle7 = typeof(IProvideValueTarget);
			object[] array4 = new object[0 + 5];
			array4[0] = label3;
			array4[1] = stackLayout3;
			array4[2] = grid;
			array4[3] = scrollView;
			array4[4] = this;
			object obj5;
			xamlServiceProvider4.Add(typeFromHandle7, obj5 = new SimpleValueTargetProvider(array4, Label.TextProperty, nameScope));
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
			xamlServiceProvider4.Add(typeFromHandle8, new XamlTypeResolver(xmlNamespaceResolver4, typeof(CodingLightConfigurationPage).GetTypeInfo().Assembly));
			xamlServiceProvider4.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(44, 25)));
			object obj6 = markupExtension4.ProvideValue(xamlServiceProvider4);
			label3.Text = obj6;
			dynamicResourceExtension3.Key = "RedTextColor";
			IMarkupExtension<DynamicResource> markupExtension5 = dynamicResourceExtension3;
			XamlServiceProvider xamlServiceProvider5 = new XamlServiceProvider();
			Type typeFromHandle9 = typeof(IProvideValueTarget);
			object[] array5 = new object[0 + 5];
			array5[0] = label3;
			array5[1] = stackLayout3;
			array5[2] = grid;
			array5[3] = scrollView;
			array5[4] = this;
			object obj7;
			xamlServiceProvider5.Add(typeFromHandle9, obj7 = new SimpleValueTargetProvider(array5, Label.TextColorProperty, nameScope));
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
			xmlNamespaceResolver5.Add("syncfusion", "clr-namespace:Syncfusion.XForms.Buttons;assembly=Syncfusion.Buttons.XForms");
			xmlNamespaceResolver5.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider5.Add(typeFromHandle10, new XamlTypeResolver(xmlNamespaceResolver5, typeof(CodingLightConfigurationPage).GetTypeInfo().Assembly));
			xamlServiceProvider5.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(45, 25)));
			DynamicResource dynamicResource3 = markupExtension5.ProvideValue(xamlServiceProvider5);
			label3.SetDynamicResource(Label.TextColorProperty, dynamicResource3.Key);
			stackLayout3.Children.Add(label3);
			bindingExtension3.Path = "LightConfiguration";
			BindingBase bindingBase3 = bindingExtension3.ProvideValue(null);
			stackLayout.SetBinding(BindableObject.BindingContextProperty, bindingBase3);
			stackLayout.SetValue(StackLayout.OrientationProperty, 0);
			translate3.Text = "coding_LampType";
			IMarkupExtension markupExtension6 = translate3;
			XamlServiceProvider xamlServiceProvider6 = new XamlServiceProvider();
			Type typeFromHandle11 = typeof(IProvideValueTarget);
			object[] array6 = new object[0 + 6];
			array6[0] = label4;
			array6[1] = stackLayout;
			array6[2] = stackLayout3;
			array6[3] = grid;
			array6[4] = scrollView;
			array6[5] = this;
			object obj8;
			xamlServiceProvider6.Add(typeFromHandle11, obj8 = new SimpleValueTargetProvider(array6, Label.TextProperty, nameScope));
			xamlServiceProvider6.Add(typeof(IReferenceProvider), obj8);
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
			xamlServiceProvider6.Add(typeFromHandle12, new XamlTypeResolver(xmlNamespaceResolver6, typeof(CodingLightConfigurationPage).GetTypeInfo().Assembly));
			xamlServiceProvider6.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(51, 32)));
			object obj9 = markupExtension6.ProvideValue(xamlServiceProvider6);
			label4.Text = obj9;
			stackLayout.Children.Add(label4);
			labelWithBorder.Tapped += this.lbLampType_Tapped;
			bindingExtension4.Mode = 2;
			bindingExtension4.Path = "LampType.Title";
			BindingBase bindingBase4 = bindingExtension4.ProvideValue(null);
			labelWithBorder.SetBinding(LabelWithBorder.TextProperty, bindingBase4);
			stackLayout.Children.Add(labelWithBorder);
			translate4.Text = "coding_Function";
			IMarkupExtension markupExtension7 = translate4;
			XamlServiceProvider xamlServiceProvider7 = new XamlServiceProvider();
			Type typeFromHandle13 = typeof(IProvideValueTarget);
			object[] array7 = new object[0 + 8];
			array7[0] = span;
			array7[1] = formattedString;
			array7[2] = label5;
			array7[3] = stackLayout;
			array7[4] = stackLayout3;
			array7[5] = grid;
			array7[6] = scrollView;
			array7[7] = this;
			object obj10;
			xamlServiceProvider7.Add(typeFromHandle13, obj10 = new SimpleValueTargetProvider(array7, Span.TextProperty, nameScope));
			xamlServiceProvider7.Add(typeof(IReferenceProvider), obj10);
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
			xamlServiceProvider7.Add(typeFromHandle14, new XamlTypeResolver(xmlNamespaceResolver7, typeof(CodingLightConfigurationPage).GetTypeInfo().Assembly));
			xamlServiceProvider7.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(58, 43)));
			object obj11 = markupExtension7.ProvideValue(xamlServiceProvider7);
			span.Text = obj11;
			formattedString.Spans.Add(span);
			span2.SetValue(Span.TextProperty, " A:");
			formattedString.Spans.Add(span2);
			label5.SetValue(Label.FormattedTextProperty, formattedString);
			stackLayout.Children.Add(label5);
			labelWithBorder2.Tag = "A";
			labelWithBorder2.Tapped += this.lbFunctionSelector_Tapped;
			bindingExtension5.Mode = 2;
			bindingExtension5.Path = "FunctionA.Title";
			BindingBase bindingBase5 = bindingExtension5.ProvideValue(null);
			labelWithBorder2.SetBinding(LabelWithBorder.TextProperty, bindingBase5);
			stackLayout.Children.Add(labelWithBorder2);
			translate5.Text = "coding_Function";
			IMarkupExtension markupExtension8 = translate5;
			XamlServiceProvider xamlServiceProvider8 = new XamlServiceProvider();
			Type typeFromHandle15 = typeof(IProvideValueTarget);
			object[] array8 = new object[0 + 8];
			array8[0] = span3;
			array8[1] = formattedString2;
			array8[2] = label6;
			array8[3] = stackLayout;
			array8[4] = stackLayout3;
			array8[5] = grid;
			array8[6] = scrollView;
			array8[7] = this;
			object obj12;
			xamlServiceProvider8.Add(typeFromHandle15, obj12 = new SimpleValueTargetProvider(array8, Span.TextProperty, nameScope));
			xamlServiceProvider8.Add(typeof(IReferenceProvider), obj12);
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
			xamlServiceProvider8.Add(typeFromHandle16, new XamlTypeResolver(xmlNamespaceResolver8, typeof(CodingLightConfigurationPage).GetTypeInfo().Assembly));
			xamlServiceProvider8.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(72, 43)));
			object obj13 = markupExtension8.ProvideValue(xamlServiceProvider8);
			span3.Text = obj13;
			formattedString2.Spans.Add(span3);
			span4.SetValue(Span.TextProperty, " B:");
			formattedString2.Spans.Add(span4);
			label6.SetValue(Label.FormattedTextProperty, formattedString2);
			stackLayout.Children.Add(label6);
			labelWithBorder3.Tag = "B";
			labelWithBorder3.Tapped += this.lbFunctionSelector_Tapped;
			bindingExtension6.Mode = 2;
			bindingExtension6.Path = "FunctionB.Title";
			BindingBase bindingBase6 = bindingExtension6.ProvideValue(null);
			labelWithBorder3.SetBinding(LabelWithBorder.TextProperty, bindingBase6);
			stackLayout.Children.Add(labelWithBorder3);
			translate6.Text = "coding_Dimmwert";
			IMarkupExtension markupExtension9 = translate6;
			XamlServiceProvider xamlServiceProvider9 = new XamlServiceProvider();
			Type typeFromHandle17 = typeof(IProvideValueTarget);
			object[] array9 = new object[0 + 8];
			array9[0] = span5;
			array9[1] = formattedString3;
			array9[2] = label7;
			array9[3] = stackLayout;
			array9[4] = stackLayout3;
			array9[5] = grid;
			array9[6] = scrollView;
			array9[7] = this;
			object obj14;
			xamlServiceProvider9.Add(typeFromHandle17, obj14 = new SimpleValueTargetProvider(array9, Span.TextProperty, nameScope));
			xamlServiceProvider9.Add(typeof(IReferenceProvider), obj14);
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
			xamlServiceProvider9.Add(typeFromHandle18, new XamlTypeResolver(xmlNamespaceResolver9, typeof(CodingLightConfigurationPage).GetTypeInfo().Assembly));
			xamlServiceProvider9.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(84, 43)));
			object obj15 = markupExtension9.ProvideValue(xamlServiceProvider9);
			span5.Text = obj15;
			formattedString3.Spans.Add(span5);
			span6.SetValue(Span.TextProperty, " A, B:");
			formattedString3.Spans.Add(span6);
			label7.SetValue(Label.FormattedTextProperty, formattedString3);
			stackLayout.Children.Add(label7);
			numericEntryV.SetValue(NumericEntryV3.DoubleFormatProperty, "0");
			numericEntryV.SetValue(NumericEntryV3.MaximumProperty, 127.0);
			numericEntryV.SetValue(NumericEntryV3.MinimumProperty, 0.0);
			bindingExtension7.Mode = 1;
			bindingExtension7.Path = "DimmwertAB";
			BindingBase bindingBase7 = bindingExtension7.ProvideValue(null);
			numericEntryV.SetBinding(NumericEntryV3.ValueProperty, bindingBase7);
			stackLayout.Children.Add(numericEntryV);
			translate7.Text = "coding_LightControlAB";
			IMarkupExtension markupExtension10 = translate7;
			XamlServiceProvider xamlServiceProvider10 = new XamlServiceProvider();
			Type typeFromHandle19 = typeof(IProvideValueTarget);
			object[] array10 = new object[0 + 6];
			array10[0] = label8;
			array10[1] = stackLayout;
			array10[2] = stackLayout3;
			array10[3] = grid;
			array10[4] = scrollView;
			array10[5] = this;
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
			xmlNamespaceResolver10.Add("syncfusion", "clr-namespace:Syncfusion.XForms.Buttons;assembly=Syncfusion.Buttons.XForms");
			xmlNamespaceResolver10.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider10.Add(typeFromHandle20, new XamlTypeResolver(xmlNamespaceResolver10, typeof(CodingLightConfigurationPage).GetTypeInfo().Assembly));
			xamlServiceProvider10.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(94, 32)));
			object obj17 = markupExtension10.ProvideValue(xamlServiceProvider10);
			label8.Text = obj17;
			stackLayout.Children.Add(label8);
			dynamicResourceExtension4.Key = "BaseFontSize-";
			IMarkupExtension<DynamicResource> markupExtension11 = dynamicResourceExtension4;
			XamlServiceProvider xamlServiceProvider11 = new XamlServiceProvider();
			Type typeFromHandle21 = typeof(IProvideValueTarget);
			object[] array11 = new object[0 + 6];
			array11[0] = picker;
			array11[1] = stackLayout;
			array11[2] = stackLayout3;
			array11[3] = grid;
			array11[4] = scrollView;
			array11[5] = this;
			object obj18;
			xamlServiceProvider11.Add(typeFromHandle21, obj18 = new SimpleValueTargetProvider(array11, Picker.FontSizeProperty, nameScope));
			xamlServiceProvider11.Add(typeof(IReferenceProvider), obj18);
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
			xamlServiceProvider11.Add(typeFromHandle22, new XamlTypeResolver(xmlNamespaceResolver11, typeof(CodingLightConfigurationPage).GetTypeInfo().Assembly));
			xamlServiceProvider11.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(96, 29)));
			DynamicResource dynamicResource4 = markupExtension11.ProvideValue(xamlServiceProvider11);
			picker.SetDynamicResource(Picker.FontSizeProperty, dynamicResource4.Key);
			bindingExtension8.Source = lightControlList;
			BindingBase bindingBase8 = bindingExtension8.ProvideValue(null);
			picker.SetBinding(Picker.ItemsSourceProperty, bindingBase8);
			bindingExtension9.Mode = 1;
			staticResourceExtension.Key = "LightControlToIntConverter";
			IMarkupExtension markupExtension12 = staticResourceExtension;
			XamlServiceProvider xamlServiceProvider12 = new XamlServiceProvider();
			Type typeFromHandle23 = typeof(IProvideValueTarget);
			object[] array12 = new object[0 + 7];
			array12[0] = bindingExtension9;
			array12[1] = picker;
			array12[2] = stackLayout;
			array12[3] = stackLayout3;
			array12[4] = grid;
			array12[5] = scrollView;
			array12[6] = this;
			object obj19;
			xamlServiceProvider12.Add(typeFromHandle23, obj19 = new SimpleValueTargetProvider(array12, typeof(BindingExtension).GetRuntimeProperty("Converter"), nameScope));
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
			xamlServiceProvider12.Add(typeFromHandle24, new XamlTypeResolver(xmlNamespaceResolver12, typeof(CodingLightConfigurationPage).GetTypeInfo().Assembly));
			xamlServiceProvider12.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(98, 29)));
			object obj20 = markupExtension12.ProvideValue(xamlServiceProvider12);
			bindingExtension9.Converter = obj20;
			bindingExtension9.Path = "LightControlAB";
			BindingBase bindingBase9 = bindingExtension9.ProvideValue(null);
			picker.SetBinding(Picker.SelectedIndexProperty, bindingBase9);
			stackLayout.Children.Add(picker);
			translate8.Text = "coding_Function";
			IMarkupExtension markupExtension13 = translate8;
			XamlServiceProvider xamlServiceProvider13 = new XamlServiceProvider();
			Type typeFromHandle25 = typeof(IProvideValueTarget);
			object[] array13 = new object[0 + 8];
			array13[0] = span7;
			array13[1] = formattedString4;
			array13[2] = label9;
			array13[3] = stackLayout;
			array13[4] = stackLayout3;
			array13[5] = grid;
			array13[6] = scrollView;
			array13[7] = this;
			object obj21;
			xamlServiceProvider13.Add(typeFromHandle25, obj21 = new SimpleValueTargetProvider(array13, Span.TextProperty, nameScope));
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
			xamlServiceProvider13.Add(typeFromHandle26, new XamlTypeResolver(xmlNamespaceResolver13, typeof(CodingLightConfigurationPage).GetTypeInfo().Assembly));
			xamlServiceProvider13.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(104, 43)));
			object obj22 = markupExtension13.ProvideValue(xamlServiceProvider13);
			span7.Text = obj22;
			formattedString4.Spans.Add(span7);
			span8.SetValue(Span.TextProperty, " C:");
			formattedString4.Spans.Add(span8);
			label9.SetValue(Label.FormattedTextProperty, formattedString4);
			stackLayout.Children.Add(label9);
			labelWithBorder4.Tag = "C";
			labelWithBorder4.Tapped += this.lbFunctionSelector_Tapped;
			bindingExtension10.Mode = 2;
			bindingExtension10.Path = "FunctionC.Title";
			BindingBase bindingBase10 = bindingExtension10.ProvideValue(null);
			labelWithBorder4.SetBinding(LabelWithBorder.TextProperty, bindingBase10);
			stackLayout.Children.Add(labelWithBorder4);
			translate9.Text = "coding_Function";
			IMarkupExtension markupExtension14 = translate9;
			XamlServiceProvider xamlServiceProvider14 = new XamlServiceProvider();
			Type typeFromHandle27 = typeof(IProvideValueTarget);
			object[] array14 = new object[0 + 8];
			array14[0] = span9;
			array14[1] = formattedString5;
			array14[2] = label10;
			array14[3] = stackLayout;
			array14[4] = stackLayout3;
			array14[5] = grid;
			array14[6] = scrollView;
			array14[7] = this;
			object obj23;
			xamlServiceProvider14.Add(typeFromHandle27, obj23 = new SimpleValueTargetProvider(array14, Span.TextProperty, nameScope));
			xamlServiceProvider14.Add(typeof(IReferenceProvider), obj23);
			Type typeFromHandle28 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver14 = new XmlNamespaceResolver();
			xmlNamespaceResolver14.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver14.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver14.Add("coding", "clr-namespace:CarScannerXamarinForms.Coding");
			xmlNamespaceResolver14.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver14.Add("d", "http://xamarin.com/schemas/2014/forms/design");
			xmlNamespaceResolver14.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver14.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver14.Add("mc", "http://schemas.openxmlformats.org/markup-compatibility/2006");
			xmlNamespaceResolver14.Add("settings", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver14.Add("syncfusion", "clr-namespace:Syncfusion.XForms.Buttons;assembly=Syncfusion.Buttons.XForms");
			xmlNamespaceResolver14.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider14.Add(typeFromHandle28, new XamlTypeResolver(xmlNamespaceResolver14, typeof(CodingLightConfigurationPage).GetTypeInfo().Assembly));
			xamlServiceProvider14.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(117, 43)));
			object obj24 = markupExtension14.ProvideValue(xamlServiceProvider14);
			span9.Text = obj24;
			formattedString5.Spans.Add(span9);
			span10.SetValue(Span.TextProperty, " D:");
			formattedString5.Spans.Add(span10);
			label10.SetValue(Label.FormattedTextProperty, formattedString5);
			stackLayout.Children.Add(label10);
			labelWithBorder5.Tag = "D";
			labelWithBorder5.Tapped += this.lbFunctionSelector_Tapped;
			bindingExtension11.Mode = 2;
			bindingExtension11.Path = "FunctionD.Title";
			BindingBase bindingBase11 = bindingExtension11.ProvideValue(null);
			labelWithBorder5.SetBinding(LabelWithBorder.TextProperty, bindingBase11);
			stackLayout.Children.Add(labelWithBorder5);
			translate10.Text = "coding_Dimmwert";
			IMarkupExtension markupExtension15 = translate10;
			XamlServiceProvider xamlServiceProvider15 = new XamlServiceProvider();
			Type typeFromHandle29 = typeof(IProvideValueTarget);
			object[] array15 = new object[0 + 8];
			array15[0] = span11;
			array15[1] = formattedString6;
			array15[2] = label11;
			array15[3] = stackLayout;
			array15[4] = stackLayout3;
			array15[5] = grid;
			array15[6] = scrollView;
			array15[7] = this;
			object obj25;
			xamlServiceProvider15.Add(typeFromHandle29, obj25 = new SimpleValueTargetProvider(array15, Span.TextProperty, nameScope));
			xamlServiceProvider15.Add(typeof(IReferenceProvider), obj25);
			Type typeFromHandle30 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver15 = new XmlNamespaceResolver();
			xmlNamespaceResolver15.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver15.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver15.Add("coding", "clr-namespace:CarScannerXamarinForms.Coding");
			xmlNamespaceResolver15.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver15.Add("d", "http://xamarin.com/schemas/2014/forms/design");
			xmlNamespaceResolver15.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver15.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver15.Add("mc", "http://schemas.openxmlformats.org/markup-compatibility/2006");
			xmlNamespaceResolver15.Add("settings", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver15.Add("syncfusion", "clr-namespace:Syncfusion.XForms.Buttons;assembly=Syncfusion.Buttons.XForms");
			xmlNamespaceResolver15.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider15.Add(typeFromHandle30, new XamlTypeResolver(xmlNamespaceResolver15, typeof(CodingLightConfigurationPage).GetTypeInfo().Assembly));
			xamlServiceProvider15.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(130, 43)));
			object obj26 = markupExtension15.ProvideValue(xamlServiceProvider15);
			span11.Text = obj26;
			formattedString6.Spans.Add(span11);
			span12.SetValue(Span.TextProperty, " C, D:");
			formattedString6.Spans.Add(span12);
			label11.SetValue(Label.FormattedTextProperty, formattedString6);
			stackLayout.Children.Add(label11);
			numericEntryV2.SetValue(NumericEntryV3.DoubleFormatProperty, "0");
			numericEntryV2.SetValue(NumericEntryV3.MaximumProperty, 127.0);
			numericEntryV2.SetValue(NumericEntryV3.MinimumProperty, 0.0);
			bindingExtension12.Mode = 1;
			bindingExtension12.Path = "DimmwertCD";
			BindingBase bindingBase12 = bindingExtension12.ProvideValue(null);
			numericEntryV2.SetBinding(NumericEntryV3.ValueProperty, bindingBase12);
			stackLayout.Children.Add(numericEntryV2);
			translate11.Text = "coding_DimmingDirection";
			IMarkupExtension markupExtension16 = translate11;
			XamlServiceProvider xamlServiceProvider16 = new XamlServiceProvider();
			Type typeFromHandle31 = typeof(IProvideValueTarget);
			object[] array16 = new object[0 + 8];
			array16[0] = span13;
			array16[1] = formattedString7;
			array16[2] = label12;
			array16[3] = stackLayout;
			array16[4] = stackLayout3;
			array16[5] = grid;
			array16[6] = scrollView;
			array16[7] = this;
			object obj27;
			xamlServiceProvider16.Add(typeFromHandle31, obj27 = new SimpleValueTargetProvider(array16, Span.TextProperty, nameScope));
			xamlServiceProvider16.Add(typeof(IReferenceProvider), obj27);
			Type typeFromHandle32 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver16 = new XmlNamespaceResolver();
			xmlNamespaceResolver16.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver16.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver16.Add("coding", "clr-namespace:CarScannerXamarinForms.Coding");
			xmlNamespaceResolver16.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver16.Add("d", "http://xamarin.com/schemas/2014/forms/design");
			xmlNamespaceResolver16.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver16.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver16.Add("mc", "http://schemas.openxmlformats.org/markup-compatibility/2006");
			xmlNamespaceResolver16.Add("settings", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver16.Add("syncfusion", "clr-namespace:Syncfusion.XForms.Buttons;assembly=Syncfusion.Buttons.XForms");
			xmlNamespaceResolver16.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider16.Add(typeFromHandle32, new XamlTypeResolver(xmlNamespaceResolver16, typeof(CodingLightConfigurationPage).GetTypeInfo().Assembly));
			xamlServiceProvider16.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(143, 43)));
			object obj28 = markupExtension16.ProvideValue(xamlServiceProvider16);
			span13.Text = obj28;
			formattedString7.Spans.Add(span13);
			span14.SetValue(Span.TextProperty, " C, D:");
			formattedString7.Spans.Add(span14);
			label12.SetValue(Label.FormattedTextProperty, formattedString7);
			stackLayout.Children.Add(label12);
			dynamicResourceExtension5.Key = "BaseFontSize-";
			IMarkupExtension<DynamicResource> markupExtension17 = dynamicResourceExtension5;
			XamlServiceProvider xamlServiceProvider17 = new XamlServiceProvider();
			Type typeFromHandle33 = typeof(IProvideValueTarget);
			object[] array17 = new object[0 + 6];
			array17[0] = picker2;
			array17[1] = stackLayout;
			array17[2] = stackLayout3;
			array17[3] = grid;
			array17[4] = scrollView;
			array17[5] = this;
			object obj29;
			xamlServiceProvider17.Add(typeFromHandle33, obj29 = new SimpleValueTargetProvider(array17, Picker.FontSizeProperty, nameScope));
			xamlServiceProvider17.Add(typeof(IReferenceProvider), obj29);
			Type typeFromHandle34 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver17 = new XmlNamespaceResolver();
			xmlNamespaceResolver17.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver17.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver17.Add("coding", "clr-namespace:CarScannerXamarinForms.Coding");
			xmlNamespaceResolver17.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver17.Add("d", "http://xamarin.com/schemas/2014/forms/design");
			xmlNamespaceResolver17.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver17.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver17.Add("mc", "http://schemas.openxmlformats.org/markup-compatibility/2006");
			xmlNamespaceResolver17.Add("settings", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver17.Add("syncfusion", "clr-namespace:Syncfusion.XForms.Buttons;assembly=Syncfusion.Buttons.XForms");
			xmlNamespaceResolver17.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider17.Add(typeFromHandle34, new XamlTypeResolver(xmlNamespaceResolver17, typeof(CodingLightConfigurationPage).GetTypeInfo().Assembly));
			xamlServiceProvider17.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(149, 29)));
			DynamicResource dynamicResource5 = markupExtension17.ProvideValue(xamlServiceProvider17);
			picker2.SetDynamicResource(Picker.FontSizeProperty, dynamicResource5.Key);
			bindingExtension13.Source = dimmingDirectionList;
			BindingBase bindingBase13 = bindingExtension13.ProvideValue(null);
			picker2.SetBinding(Picker.ItemsSourceProperty, bindingBase13);
			bindingExtension14.Mode = 1;
			staticResourceExtension2.Key = "DimmingDirectionToIntConverter";
			IMarkupExtension markupExtension18 = staticResourceExtension2;
			XamlServiceProvider xamlServiceProvider18 = new XamlServiceProvider();
			Type typeFromHandle35 = typeof(IProvideValueTarget);
			object[] array18 = new object[0 + 7];
			array18[0] = bindingExtension14;
			array18[1] = picker2;
			array18[2] = stackLayout;
			array18[3] = stackLayout3;
			array18[4] = grid;
			array18[5] = scrollView;
			array18[6] = this;
			object obj30;
			xamlServiceProvider18.Add(typeFromHandle35, obj30 = new SimpleValueTargetProvider(array18, typeof(BindingExtension).GetRuntimeProperty("Converter"), nameScope));
			xamlServiceProvider18.Add(typeof(IReferenceProvider), obj30);
			Type typeFromHandle36 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver18 = new XmlNamespaceResolver();
			xmlNamespaceResolver18.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver18.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver18.Add("coding", "clr-namespace:CarScannerXamarinForms.Coding");
			xmlNamespaceResolver18.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver18.Add("d", "http://xamarin.com/schemas/2014/forms/design");
			xmlNamespaceResolver18.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver18.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver18.Add("mc", "http://schemas.openxmlformats.org/markup-compatibility/2006");
			xmlNamespaceResolver18.Add("settings", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver18.Add("syncfusion", "clr-namespace:Syncfusion.XForms.Buttons;assembly=Syncfusion.Buttons.XForms");
			xmlNamespaceResolver18.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider18.Add(typeFromHandle36, new XamlTypeResolver(xmlNamespaceResolver18, typeof(CodingLightConfigurationPage).GetTypeInfo().Assembly));
			xamlServiceProvider18.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(151, 29)));
			object obj31 = markupExtension18.ProvideValue(xamlServiceProvider18);
			bindingExtension14.Converter = obj31;
			bindingExtension14.Path = "DimmingDirectionCD";
			BindingBase bindingBase14 = bindingExtension14.ProvideValue(null);
			picker2.SetBinding(Picker.SelectedIndexProperty, bindingBase14);
			stackLayout.Children.Add(picker2);
			translate12.Text = "coding_Function";
			IMarkupExtension markupExtension19 = translate12;
			XamlServiceProvider xamlServiceProvider19 = new XamlServiceProvider();
			Type typeFromHandle37 = typeof(IProvideValueTarget);
			object[] array19 = new object[0 + 8];
			array19[0] = span15;
			array19[1] = formattedString8;
			array19[2] = label13;
			array19[3] = stackLayout;
			array19[4] = stackLayout3;
			array19[5] = grid;
			array19[6] = scrollView;
			array19[7] = this;
			object obj32;
			xamlServiceProvider19.Add(typeFromHandle37, obj32 = new SimpleValueTargetProvider(array19, Span.TextProperty, nameScope));
			xamlServiceProvider19.Add(typeof(IReferenceProvider), obj32);
			Type typeFromHandle38 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver19 = new XmlNamespaceResolver();
			xmlNamespaceResolver19.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver19.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver19.Add("coding", "clr-namespace:CarScannerXamarinForms.Coding");
			xmlNamespaceResolver19.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver19.Add("d", "http://xamarin.com/schemas/2014/forms/design");
			xmlNamespaceResolver19.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver19.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver19.Add("mc", "http://schemas.openxmlformats.org/markup-compatibility/2006");
			xmlNamespaceResolver19.Add("settings", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver19.Add("syncfusion", "clr-namespace:Syncfusion.XForms.Buttons;assembly=Syncfusion.Buttons.XForms");
			xmlNamespaceResolver19.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider19.Add(typeFromHandle38, new XamlTypeResolver(xmlNamespaceResolver19, typeof(CodingLightConfigurationPage).GetTypeInfo().Assembly));
			xamlServiceProvider19.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(158, 43)));
			object obj33 = markupExtension19.ProvideValue(xamlServiceProvider19);
			span15.Text = obj33;
			formattedString8.Spans.Add(span15);
			span16.SetValue(Span.TextProperty, " E:");
			formattedString8.Spans.Add(span16);
			label13.SetValue(Label.FormattedTextProperty, formattedString8);
			stackLayout.Children.Add(label13);
			labelWithBorder6.Tag = "E";
			labelWithBorder6.Tapped += this.lbFunctionSelector_Tapped;
			bindingExtension15.Mode = 2;
			bindingExtension15.Path = "FunctionE.Title";
			BindingBase bindingBase15 = bindingExtension15.ProvideValue(null);
			labelWithBorder6.SetBinding(LabelWithBorder.TextProperty, bindingBase15);
			stackLayout.Children.Add(labelWithBorder6);
			translate13.Text = "coding_Function";
			IMarkupExtension markupExtension20 = translate13;
			XamlServiceProvider xamlServiceProvider20 = new XamlServiceProvider();
			Type typeFromHandle39 = typeof(IProvideValueTarget);
			object[] array20 = new object[0 + 8];
			array20[0] = span17;
			array20[1] = formattedString9;
			array20[2] = label14;
			array20[3] = stackLayout;
			array20[4] = stackLayout3;
			array20[5] = grid;
			array20[6] = scrollView;
			array20[7] = this;
			object obj34;
			xamlServiceProvider20.Add(typeFromHandle39, obj34 = new SimpleValueTargetProvider(array20, Span.TextProperty, nameScope));
			xamlServiceProvider20.Add(typeof(IReferenceProvider), obj34);
			Type typeFromHandle40 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver20 = new XmlNamespaceResolver();
			xmlNamespaceResolver20.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver20.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver20.Add("coding", "clr-namespace:CarScannerXamarinForms.Coding");
			xmlNamespaceResolver20.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver20.Add("d", "http://xamarin.com/schemas/2014/forms/design");
			xmlNamespaceResolver20.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver20.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver20.Add("mc", "http://schemas.openxmlformats.org/markup-compatibility/2006");
			xmlNamespaceResolver20.Add("settings", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver20.Add("syncfusion", "clr-namespace:Syncfusion.XForms.Buttons;assembly=Syncfusion.Buttons.XForms");
			xmlNamespaceResolver20.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider20.Add(typeFromHandle40, new XamlTypeResolver(xmlNamespaceResolver20, typeof(CodingLightConfigurationPage).GetTypeInfo().Assembly));
			xamlServiceProvider20.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(170, 43)));
			object obj35 = markupExtension20.ProvideValue(xamlServiceProvider20);
			span17.Text = obj35;
			formattedString9.Spans.Add(span17);
			span18.SetValue(Span.TextProperty, " F:");
			formattedString9.Spans.Add(span18);
			label14.SetValue(Label.FormattedTextProperty, formattedString9);
			stackLayout.Children.Add(label14);
			labelWithBorder7.Tag = "F";
			labelWithBorder7.Tapped += this.lbFunctionSelector_Tapped;
			bindingExtension16.Mode = 2;
			bindingExtension16.Path = "FunctionF.Title";
			BindingBase bindingBase16 = bindingExtension16.ProvideValue(null);
			labelWithBorder7.SetBinding(LabelWithBorder.TextProperty, bindingBase16);
			stackLayout.Children.Add(labelWithBorder7);
			translate14.Text = "coding_Dimmwert";
			IMarkupExtension markupExtension21 = translate14;
			XamlServiceProvider xamlServiceProvider21 = new XamlServiceProvider();
			Type typeFromHandle41 = typeof(IProvideValueTarget);
			object[] array21 = new object[0 + 8];
			array21[0] = span19;
			array21[1] = formattedString10;
			array21[2] = label15;
			array21[3] = stackLayout;
			array21[4] = stackLayout3;
			array21[5] = grid;
			array21[6] = scrollView;
			array21[7] = this;
			object obj36;
			xamlServiceProvider21.Add(typeFromHandle41, obj36 = new SimpleValueTargetProvider(array21, Span.TextProperty, nameScope));
			xamlServiceProvider21.Add(typeof(IReferenceProvider), obj36);
			Type typeFromHandle42 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver21 = new XmlNamespaceResolver();
			xmlNamespaceResolver21.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver21.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver21.Add("coding", "clr-namespace:CarScannerXamarinForms.Coding");
			xmlNamespaceResolver21.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver21.Add("d", "http://xamarin.com/schemas/2014/forms/design");
			xmlNamespaceResolver21.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver21.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver21.Add("mc", "http://schemas.openxmlformats.org/markup-compatibility/2006");
			xmlNamespaceResolver21.Add("settings", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver21.Add("syncfusion", "clr-namespace:Syncfusion.XForms.Buttons;assembly=Syncfusion.Buttons.XForms");
			xmlNamespaceResolver21.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider21.Add(typeFromHandle42, new XamlTypeResolver(xmlNamespaceResolver21, typeof(CodingLightConfigurationPage).GetTypeInfo().Assembly));
			xamlServiceProvider21.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(184, 43)));
			object obj37 = markupExtension21.ProvideValue(xamlServiceProvider21);
			span19.Text = obj37;
			formattedString10.Spans.Add(span19);
			span20.SetValue(Span.TextProperty, " E, F:");
			formattedString10.Spans.Add(span20);
			label15.SetValue(Label.FormattedTextProperty, formattedString10);
			stackLayout.Children.Add(label15);
			numericEntryV3.SetValue(NumericEntryV3.DoubleFormatProperty, "0");
			numericEntryV3.SetValue(NumericEntryV3.MaximumProperty, 127.0);
			numericEntryV3.SetValue(NumericEntryV3.MinimumProperty, 0.0);
			bindingExtension17.Mode = 1;
			bindingExtension17.Path = "DimmwertEF";
			BindingBase bindingBase17 = bindingExtension17.ProvideValue(null);
			numericEntryV3.SetBinding(NumericEntryV3.ValueProperty, bindingBase17);
			stackLayout.Children.Add(numericEntryV3);
			translate15.Text = "coding_DimmingDirection";
			IMarkupExtension markupExtension22 = translate15;
			XamlServiceProvider xamlServiceProvider22 = new XamlServiceProvider();
			Type typeFromHandle43 = typeof(IProvideValueTarget);
			object[] array22 = new object[0 + 8];
			array22[0] = span21;
			array22[1] = formattedString11;
			array22[2] = label16;
			array22[3] = stackLayout;
			array22[4] = stackLayout3;
			array22[5] = grid;
			array22[6] = scrollView;
			array22[7] = this;
			object obj38;
			xamlServiceProvider22.Add(typeFromHandle43, obj38 = new SimpleValueTargetProvider(array22, Span.TextProperty, nameScope));
			xamlServiceProvider22.Add(typeof(IReferenceProvider), obj38);
			Type typeFromHandle44 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver22 = new XmlNamespaceResolver();
			xmlNamespaceResolver22.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver22.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver22.Add("coding", "clr-namespace:CarScannerXamarinForms.Coding");
			xmlNamespaceResolver22.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver22.Add("d", "http://xamarin.com/schemas/2014/forms/design");
			xmlNamespaceResolver22.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver22.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver22.Add("mc", "http://schemas.openxmlformats.org/markup-compatibility/2006");
			xmlNamespaceResolver22.Add("settings", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver22.Add("syncfusion", "clr-namespace:Syncfusion.XForms.Buttons;assembly=Syncfusion.Buttons.XForms");
			xmlNamespaceResolver22.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider22.Add(typeFromHandle44, new XamlTypeResolver(xmlNamespaceResolver22, typeof(CodingLightConfigurationPage).GetTypeInfo().Assembly));
			xamlServiceProvider22.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(197, 43)));
			object obj39 = markupExtension22.ProvideValue(xamlServiceProvider22);
			span21.Text = obj39;
			formattedString11.Spans.Add(span21);
			span22.SetValue(Span.TextProperty, " E, F:");
			formattedString11.Spans.Add(span22);
			label16.SetValue(Label.FormattedTextProperty, formattedString11);
			stackLayout.Children.Add(label16);
			dynamicResourceExtension6.Key = "BaseFontSize-";
			IMarkupExtension<DynamicResource> markupExtension23 = dynamicResourceExtension6;
			XamlServiceProvider xamlServiceProvider23 = new XamlServiceProvider();
			Type typeFromHandle45 = typeof(IProvideValueTarget);
			object[] array23 = new object[0 + 6];
			array23[0] = picker3;
			array23[1] = stackLayout;
			array23[2] = stackLayout3;
			array23[3] = grid;
			array23[4] = scrollView;
			array23[5] = this;
			object obj40;
			xamlServiceProvider23.Add(typeFromHandle45, obj40 = new SimpleValueTargetProvider(array23, Picker.FontSizeProperty, nameScope));
			xamlServiceProvider23.Add(typeof(IReferenceProvider), obj40);
			Type typeFromHandle46 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver23 = new XmlNamespaceResolver();
			xmlNamespaceResolver23.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver23.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver23.Add("coding", "clr-namespace:CarScannerXamarinForms.Coding");
			xmlNamespaceResolver23.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver23.Add("d", "http://xamarin.com/schemas/2014/forms/design");
			xmlNamespaceResolver23.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver23.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver23.Add("mc", "http://schemas.openxmlformats.org/markup-compatibility/2006");
			xmlNamespaceResolver23.Add("settings", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver23.Add("syncfusion", "clr-namespace:Syncfusion.XForms.Buttons;assembly=Syncfusion.Buttons.XForms");
			xmlNamespaceResolver23.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider23.Add(typeFromHandle46, new XamlTypeResolver(xmlNamespaceResolver23, typeof(CodingLightConfigurationPage).GetTypeInfo().Assembly));
			xamlServiceProvider23.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(203, 29)));
			DynamicResource dynamicResource6 = markupExtension23.ProvideValue(xamlServiceProvider23);
			picker3.SetDynamicResource(Picker.FontSizeProperty, dynamicResource6.Key);
			bindingExtension18.Source = dimmingDirectionList2;
			BindingBase bindingBase18 = bindingExtension18.ProvideValue(null);
			picker3.SetBinding(Picker.ItemsSourceProperty, bindingBase18);
			bindingExtension19.Mode = 1;
			staticResourceExtension3.Key = "DimmingDirectionToIntConverter";
			IMarkupExtension markupExtension24 = staticResourceExtension3;
			XamlServiceProvider xamlServiceProvider24 = new XamlServiceProvider();
			Type typeFromHandle47 = typeof(IProvideValueTarget);
			object[] array24 = new object[0 + 7];
			array24[0] = bindingExtension19;
			array24[1] = picker3;
			array24[2] = stackLayout;
			array24[3] = stackLayout3;
			array24[4] = grid;
			array24[5] = scrollView;
			array24[6] = this;
			object obj41;
			xamlServiceProvider24.Add(typeFromHandle47, obj41 = new SimpleValueTargetProvider(array24, typeof(BindingExtension).GetRuntimeProperty("Converter"), nameScope));
			xamlServiceProvider24.Add(typeof(IReferenceProvider), obj41);
			Type typeFromHandle48 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver24 = new XmlNamespaceResolver();
			xmlNamespaceResolver24.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver24.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver24.Add("coding", "clr-namespace:CarScannerXamarinForms.Coding");
			xmlNamespaceResolver24.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver24.Add("d", "http://xamarin.com/schemas/2014/forms/design");
			xmlNamespaceResolver24.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver24.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver24.Add("mc", "http://schemas.openxmlformats.org/markup-compatibility/2006");
			xmlNamespaceResolver24.Add("settings", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver24.Add("syncfusion", "clr-namespace:Syncfusion.XForms.Buttons;assembly=Syncfusion.Buttons.XForms");
			xmlNamespaceResolver24.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider24.Add(typeFromHandle48, new XamlTypeResolver(xmlNamespaceResolver24, typeof(CodingLightConfigurationPage).GetTypeInfo().Assembly));
			xamlServiceProvider24.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(205, 29)));
			object obj42 = markupExtension24.ProvideValue(xamlServiceProvider24);
			bindingExtension19.Converter = obj42;
			bindingExtension19.Path = "DimmingDirectionEF";
			BindingBase bindingBase19 = bindingExtension19.ProvideValue(null);
			picker3.SetBinding(Picker.SelectedIndexProperty, bindingBase19);
			stackLayout.Children.Add(picker3);
			translate16.Text = "coding_Function";
			IMarkupExtension markupExtension25 = translate16;
			XamlServiceProvider xamlServiceProvider25 = new XamlServiceProvider();
			Type typeFromHandle49 = typeof(IProvideValueTarget);
			object[] array25 = new object[0 + 8];
			array25[0] = span23;
			array25[1] = formattedString12;
			array25[2] = label17;
			array25[3] = stackLayout;
			array25[4] = stackLayout3;
			array25[5] = grid;
			array25[6] = scrollView;
			array25[7] = this;
			object obj43;
			xamlServiceProvider25.Add(typeFromHandle49, obj43 = new SimpleValueTargetProvider(array25, Span.TextProperty, nameScope));
			xamlServiceProvider25.Add(typeof(IReferenceProvider), obj43);
			Type typeFromHandle50 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver25 = new XmlNamespaceResolver();
			xmlNamespaceResolver25.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver25.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver25.Add("coding", "clr-namespace:CarScannerXamarinForms.Coding");
			xmlNamespaceResolver25.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver25.Add("d", "http://xamarin.com/schemas/2014/forms/design");
			xmlNamespaceResolver25.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver25.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver25.Add("mc", "http://schemas.openxmlformats.org/markup-compatibility/2006");
			xmlNamespaceResolver25.Add("settings", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver25.Add("syncfusion", "clr-namespace:Syncfusion.XForms.Buttons;assembly=Syncfusion.Buttons.XForms");
			xmlNamespaceResolver25.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider25.Add(typeFromHandle50, new XamlTypeResolver(xmlNamespaceResolver25, typeof(CodingLightConfigurationPage).GetTypeInfo().Assembly));
			xamlServiceProvider25.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(214, 43)));
			object obj44 = markupExtension25.ProvideValue(xamlServiceProvider25);
			span23.Text = obj44;
			formattedString12.Spans.Add(span23);
			span24.SetValue(Span.TextProperty, " G:");
			formattedString12.Spans.Add(span24);
			label17.SetValue(Label.FormattedTextProperty, formattedString12);
			stackLayout.Children.Add(label17);
			labelWithBorder8.Tag = "G";
			labelWithBorder8.Tapped += this.lbFunctionSelector_Tapped;
			bindingExtension20.Mode = 2;
			bindingExtension20.Path = "FunctionG.Title";
			BindingBase bindingBase20 = bindingExtension20.ProvideValue(null);
			labelWithBorder8.SetBinding(LabelWithBorder.TextProperty, bindingBase20);
			stackLayout.Children.Add(labelWithBorder8);
			translate17.Text = "coding_Function";
			IMarkupExtension markupExtension26 = translate17;
			XamlServiceProvider xamlServiceProvider26 = new XamlServiceProvider();
			Type typeFromHandle51 = typeof(IProvideValueTarget);
			object[] array26 = new object[0 + 8];
			array26[0] = span25;
			array26[1] = formattedString13;
			array26[2] = label18;
			array26[3] = stackLayout;
			array26[4] = stackLayout3;
			array26[5] = grid;
			array26[6] = scrollView;
			array26[7] = this;
			object obj45;
			xamlServiceProvider26.Add(typeFromHandle51, obj45 = new SimpleValueTargetProvider(array26, Span.TextProperty, nameScope));
			xamlServiceProvider26.Add(typeof(IReferenceProvider), obj45);
			Type typeFromHandle52 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver26 = new XmlNamespaceResolver();
			xmlNamespaceResolver26.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver26.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver26.Add("coding", "clr-namespace:CarScannerXamarinForms.Coding");
			xmlNamespaceResolver26.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver26.Add("d", "http://xamarin.com/schemas/2014/forms/design");
			xmlNamespaceResolver26.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver26.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver26.Add("mc", "http://schemas.openxmlformats.org/markup-compatibility/2006");
			xmlNamespaceResolver26.Add("settings", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver26.Add("syncfusion", "clr-namespace:Syncfusion.XForms.Buttons;assembly=Syncfusion.Buttons.XForms");
			xmlNamespaceResolver26.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider26.Add(typeFromHandle52, new XamlTypeResolver(xmlNamespaceResolver26, typeof(CodingLightConfigurationPage).GetTypeInfo().Assembly));
			xamlServiceProvider26.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(227, 43)));
			object obj46 = markupExtension26.ProvideValue(xamlServiceProvider26);
			span25.Text = obj46;
			formattedString13.Spans.Add(span25);
			span26.SetValue(Span.TextProperty, " H:");
			formattedString13.Spans.Add(span26);
			label18.SetValue(Label.FormattedTextProperty, formattedString13);
			stackLayout.Children.Add(label18);
			labelWithBorder9.Tag = "H";
			labelWithBorder9.Tapped += this.lbFunctionSelector_Tapped;
			bindingExtension21.Mode = 2;
			bindingExtension21.Path = "FunctionH.Title";
			BindingBase bindingBase21 = bindingExtension21.ProvideValue(null);
			labelWithBorder9.SetBinding(LabelWithBorder.TextProperty, bindingBase21);
			stackLayout.Children.Add(labelWithBorder9);
			translate18.Text = "coding_Dimmwert";
			IMarkupExtension markupExtension27 = translate18;
			XamlServiceProvider xamlServiceProvider27 = new XamlServiceProvider();
			Type typeFromHandle53 = typeof(IProvideValueTarget);
			object[] array27 = new object[0 + 8];
			array27[0] = span27;
			array27[1] = formattedString14;
			array27[2] = label19;
			array27[3] = stackLayout;
			array27[4] = stackLayout3;
			array27[5] = grid;
			array27[6] = scrollView;
			array27[7] = this;
			object obj47;
			xamlServiceProvider27.Add(typeFromHandle53, obj47 = new SimpleValueTargetProvider(array27, Span.TextProperty, nameScope));
			xamlServiceProvider27.Add(typeof(IReferenceProvider), obj47);
			Type typeFromHandle54 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver27 = new XmlNamespaceResolver();
			xmlNamespaceResolver27.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver27.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver27.Add("coding", "clr-namespace:CarScannerXamarinForms.Coding");
			xmlNamespaceResolver27.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver27.Add("d", "http://xamarin.com/schemas/2014/forms/design");
			xmlNamespaceResolver27.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver27.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver27.Add("mc", "http://schemas.openxmlformats.org/markup-compatibility/2006");
			xmlNamespaceResolver27.Add("settings", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver27.Add("syncfusion", "clr-namespace:Syncfusion.XForms.Buttons;assembly=Syncfusion.Buttons.XForms");
			xmlNamespaceResolver27.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider27.Add(typeFromHandle54, new XamlTypeResolver(xmlNamespaceResolver27, typeof(CodingLightConfigurationPage).GetTypeInfo().Assembly));
			xamlServiceProvider27.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(240, 43)));
			object obj48 = markupExtension27.ProvideValue(xamlServiceProvider27);
			span27.Text = obj48;
			formattedString14.Spans.Add(span27);
			span28.SetValue(Span.TextProperty, " G, H:");
			formattedString14.Spans.Add(span28);
			label19.SetValue(Label.FormattedTextProperty, formattedString14);
			stackLayout.Children.Add(label19);
			numericEntryV4.SetValue(NumericEntryV3.DoubleFormatProperty, "0");
			numericEntryV4.SetValue(NumericEntryV3.MaximumProperty, 127.0);
			numericEntryV4.SetValue(NumericEntryV3.MinimumProperty, 0.0);
			bindingExtension22.Mode = 1;
			bindingExtension22.Path = "DimmwertGH";
			BindingBase bindingBase22 = bindingExtension22.ProvideValue(null);
			numericEntryV4.SetBinding(NumericEntryV3.ValueProperty, bindingBase22);
			stackLayout.Children.Add(numericEntryV4);
			translate19.Text = "coding_DimmingDirection";
			IMarkupExtension markupExtension28 = translate19;
			XamlServiceProvider xamlServiceProvider28 = new XamlServiceProvider();
			Type typeFromHandle55 = typeof(IProvideValueTarget);
			object[] array28 = new object[0 + 8];
			array28[0] = span29;
			array28[1] = formattedString15;
			array28[2] = label20;
			array28[3] = stackLayout;
			array28[4] = stackLayout3;
			array28[5] = grid;
			array28[6] = scrollView;
			array28[7] = this;
			object obj49;
			xamlServiceProvider28.Add(typeFromHandle55, obj49 = new SimpleValueTargetProvider(array28, Span.TextProperty, nameScope));
			xamlServiceProvider28.Add(typeof(IReferenceProvider), obj49);
			Type typeFromHandle56 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver28 = new XmlNamespaceResolver();
			xmlNamespaceResolver28.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver28.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver28.Add("coding", "clr-namespace:CarScannerXamarinForms.Coding");
			xmlNamespaceResolver28.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver28.Add("d", "http://xamarin.com/schemas/2014/forms/design");
			xmlNamespaceResolver28.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver28.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver28.Add("mc", "http://schemas.openxmlformats.org/markup-compatibility/2006");
			xmlNamespaceResolver28.Add("settings", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver28.Add("syncfusion", "clr-namespace:Syncfusion.XForms.Buttons;assembly=Syncfusion.Buttons.XForms");
			xmlNamespaceResolver28.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider28.Add(typeFromHandle56, new XamlTypeResolver(xmlNamespaceResolver28, typeof(CodingLightConfigurationPage).GetTypeInfo().Assembly));
			xamlServiceProvider28.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(253, 43)));
			object obj50 = markupExtension28.ProvideValue(xamlServiceProvider28);
			span29.Text = obj50;
			formattedString15.Spans.Add(span29);
			span30.SetValue(Span.TextProperty, " G, H:");
			formattedString15.Spans.Add(span30);
			label20.SetValue(Label.FormattedTextProperty, formattedString15);
			stackLayout.Children.Add(label20);
			dynamicResourceExtension7.Key = "BaseFontSize-";
			IMarkupExtension<DynamicResource> markupExtension29 = dynamicResourceExtension7;
			XamlServiceProvider xamlServiceProvider29 = new XamlServiceProvider();
			Type typeFromHandle57 = typeof(IProvideValueTarget);
			object[] array29 = new object[0 + 6];
			array29[0] = picker4;
			array29[1] = stackLayout;
			array29[2] = stackLayout3;
			array29[3] = grid;
			array29[4] = scrollView;
			array29[5] = this;
			object obj51;
			xamlServiceProvider29.Add(typeFromHandle57, obj51 = new SimpleValueTargetProvider(array29, Picker.FontSizeProperty, nameScope));
			xamlServiceProvider29.Add(typeof(IReferenceProvider), obj51);
			Type typeFromHandle58 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver29 = new XmlNamespaceResolver();
			xmlNamespaceResolver29.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver29.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver29.Add("coding", "clr-namespace:CarScannerXamarinForms.Coding");
			xmlNamespaceResolver29.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver29.Add("d", "http://xamarin.com/schemas/2014/forms/design");
			xmlNamespaceResolver29.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver29.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver29.Add("mc", "http://schemas.openxmlformats.org/markup-compatibility/2006");
			xmlNamespaceResolver29.Add("settings", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver29.Add("syncfusion", "clr-namespace:Syncfusion.XForms.Buttons;assembly=Syncfusion.Buttons.XForms");
			xmlNamespaceResolver29.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider29.Add(typeFromHandle58, new XamlTypeResolver(xmlNamespaceResolver29, typeof(CodingLightConfigurationPage).GetTypeInfo().Assembly));
			xamlServiceProvider29.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(259, 29)));
			DynamicResource dynamicResource7 = markupExtension29.ProvideValue(xamlServiceProvider29);
			picker4.SetDynamicResource(Picker.FontSizeProperty, dynamicResource7.Key);
			bindingExtension23.Source = dimmingDirectionList3;
			BindingBase bindingBase23 = bindingExtension23.ProvideValue(null);
			picker4.SetBinding(Picker.ItemsSourceProperty, bindingBase23);
			bindingExtension24.Mode = 1;
			staticResourceExtension4.Key = "DimmingDirectionToIntConverter";
			IMarkupExtension markupExtension30 = staticResourceExtension4;
			XamlServiceProvider xamlServiceProvider30 = new XamlServiceProvider();
			Type typeFromHandle59 = typeof(IProvideValueTarget);
			object[] array30 = new object[0 + 7];
			array30[0] = bindingExtension24;
			array30[1] = picker4;
			array30[2] = stackLayout;
			array30[3] = stackLayout3;
			array30[4] = grid;
			array30[5] = scrollView;
			array30[6] = this;
			object obj52;
			xamlServiceProvider30.Add(typeFromHandle59, obj52 = new SimpleValueTargetProvider(array30, typeof(BindingExtension).GetRuntimeProperty("Converter"), nameScope));
			xamlServiceProvider30.Add(typeof(IReferenceProvider), obj52);
			Type typeFromHandle60 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver30 = new XmlNamespaceResolver();
			xmlNamespaceResolver30.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver30.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver30.Add("coding", "clr-namespace:CarScannerXamarinForms.Coding");
			xmlNamespaceResolver30.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver30.Add("d", "http://xamarin.com/schemas/2014/forms/design");
			xmlNamespaceResolver30.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver30.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver30.Add("mc", "http://schemas.openxmlformats.org/markup-compatibility/2006");
			xmlNamespaceResolver30.Add("settings", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver30.Add("syncfusion", "clr-namespace:Syncfusion.XForms.Buttons;assembly=Syncfusion.Buttons.XForms");
			xmlNamespaceResolver30.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider30.Add(typeFromHandle60, new XamlTypeResolver(xmlNamespaceResolver30, typeof(CodingLightConfigurationPage).GetTypeInfo().Assembly));
			xamlServiceProvider30.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(261, 29)));
			object obj53 = markupExtension30.ProvideValue(xamlServiceProvider30);
			bindingExtension24.Converter = obj53;
			bindingExtension24.Path = "DimmingDirectionGH";
			BindingBase bindingBase24 = bindingExtension24.ProvideValue(null);
			picker4.SetBinding(Picker.SelectedIndexProperty, bindingBase24);
			stackLayout.Children.Add(picker4);
			stackLayout3.Children.Add(stackLayout);
			bindingExtension25.Path = "PasswordVisible";
			BindingBase bindingBase25 = bindingExtension25.ProvideValue(null);
			stackLayout2.SetBinding(VisualElement.IsVisibleProperty, bindingBase25);
			stackLayout2.SetValue(StackLayout.OrientationProperty, 0);
			translate20.Text = "coding_Password";
			IMarkupExtension markupExtension31 = translate20;
			XamlServiceProvider xamlServiceProvider31 = new XamlServiceProvider();
			Type typeFromHandle61 = typeof(IProvideValueTarget);
			object[] array31 = new object[0 + 6];
			array31[0] = label21;
			array31[1] = stackLayout2;
			array31[2] = stackLayout3;
			array31[3] = grid;
			array31[4] = scrollView;
			array31[5] = this;
			object obj54;
			xamlServiceProvider31.Add(typeFromHandle61, obj54 = new SimpleValueTargetProvider(array31, Label.TextProperty, nameScope));
			xamlServiceProvider31.Add(typeof(IReferenceProvider), obj54);
			Type typeFromHandle62 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver31 = new XmlNamespaceResolver();
			xmlNamespaceResolver31.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver31.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver31.Add("coding", "clr-namespace:CarScannerXamarinForms.Coding");
			xmlNamespaceResolver31.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver31.Add("d", "http://xamarin.com/schemas/2014/forms/design");
			xmlNamespaceResolver31.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver31.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver31.Add("mc", "http://schemas.openxmlformats.org/markup-compatibility/2006");
			xmlNamespaceResolver31.Add("settings", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver31.Add("syncfusion", "clr-namespace:Syncfusion.XForms.Buttons;assembly=Syncfusion.Buttons.XForms");
			xmlNamespaceResolver31.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider31.Add(typeFromHandle62, new XamlTypeResolver(xmlNamespaceResolver31, typeof(CodingLightConfigurationPage).GetTypeInfo().Assembly));
			xamlServiceProvider31.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(267, 32)));
			object obj55 = markupExtension31.ProvideValue(xamlServiceProvider31);
			label21.Text = obj55;
			stackLayout2.Children.Add(label21);
			translate21.Text = "coding_PasswordNotRequired";
			IMarkupExtension markupExtension32 = translate21;
			XamlServiceProvider xamlServiceProvider32 = new XamlServiceProvider();
			Type typeFromHandle63 = typeof(IProvideValueTarget);
			object[] array32 = new object[0 + 6];
			array32[0] = entry;
			array32[1] = stackLayout2;
			array32[2] = stackLayout3;
			array32[3] = grid;
			array32[4] = scrollView;
			array32[5] = this;
			object obj56;
			xamlServiceProvider32.Add(typeFromHandle63, obj56 = new SimpleValueTargetProvider(array32, Entry.PlaceholderProperty, nameScope));
			xamlServiceProvider32.Add(typeof(IReferenceProvider), obj56);
			Type typeFromHandle64 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver32 = new XmlNamespaceResolver();
			xmlNamespaceResolver32.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver32.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver32.Add("coding", "clr-namespace:CarScannerXamarinForms.Coding");
			xmlNamespaceResolver32.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver32.Add("d", "http://xamarin.com/schemas/2014/forms/design");
			xmlNamespaceResolver32.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver32.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver32.Add("mc", "http://schemas.openxmlformats.org/markup-compatibility/2006");
			xmlNamespaceResolver32.Add("settings", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver32.Add("syncfusion", "clr-namespace:Syncfusion.XForms.Buttons;assembly=Syncfusion.Buttons.XForms");
			xmlNamespaceResolver32.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider32.Add(typeFromHandle64, new XamlTypeResolver(xmlNamespaceResolver32, typeof(CodingLightConfigurationPage).GetTypeInfo().Assembly));
			xamlServiceProvider32.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(270, 29)));
			object obj57 = markupExtension32.ProvideValue(xamlServiceProvider32);
			entry.Placeholder = obj57;
			bindingExtension26.Mode = 1;
			bindingExtension26.Path = "Password";
			BindingBase bindingBase26 = bindingExtension26.ProvideValue(null);
			entry.SetBinding(Entry.TextProperty, bindingBase26);
			stackLayout2.Children.Add(entry);
			label22.SetValue(Label.FontAttributesProperty, new FontAttributesConverter().ConvertFromInvariantString("Italic"));
			staticResourceExtension5.Key = "EmptyStringToFalseConverter";
			IMarkupExtension markupExtension33 = staticResourceExtension5;
			XamlServiceProvider xamlServiceProvider33 = new XamlServiceProvider();
			Type typeFromHandle65 = typeof(IProvideValueTarget);
			object[] array33 = new object[0 + 7];
			array33[0] = bindingExtension27;
			array33[1] = label22;
			array33[2] = stackLayout2;
			array33[3] = stackLayout3;
			array33[4] = grid;
			array33[5] = scrollView;
			array33[6] = this;
			object obj58;
			xamlServiceProvider33.Add(typeFromHandle65, obj58 = new SimpleValueTargetProvider(array33, typeof(BindingExtension).GetRuntimeProperty("Converter"), nameScope));
			xamlServiceProvider33.Add(typeof(IReferenceProvider), obj58);
			Type typeFromHandle66 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver33 = new XmlNamespaceResolver();
			xmlNamespaceResolver33.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver33.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver33.Add("coding", "clr-namespace:CarScannerXamarinForms.Coding");
			xmlNamespaceResolver33.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver33.Add("d", "http://xamarin.com/schemas/2014/forms/design");
			xmlNamespaceResolver33.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver33.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver33.Add("mc", "http://schemas.openxmlformats.org/markup-compatibility/2006");
			xmlNamespaceResolver33.Add("settings", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver33.Add("syncfusion", "clr-namespace:Syncfusion.XForms.Buttons;assembly=Syncfusion.Buttons.XForms");
			xmlNamespaceResolver33.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider33.Add(typeFromHandle66, new XamlTypeResolver(xmlNamespaceResolver33, typeof(CodingLightConfigurationPage).GetTypeInfo().Assembly));
			xamlServiceProvider33.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(272, 56)));
			object obj59 = markupExtension33.ProvideValue(xamlServiceProvider33);
			bindingExtension27.Converter = obj59;
			bindingExtension27.Path = "PasswordHint";
			BindingBase bindingBase27 = bindingExtension27.ProvideValue(null);
			label22.SetBinding(VisualElement.IsVisibleProperty, bindingBase27);
			translate22.Text = "coding_PasswordHint";
			IMarkupExtension markupExtension34 = translate22;
			XamlServiceProvider xamlServiceProvider34 = new XamlServiceProvider();
			Type typeFromHandle67 = typeof(IProvideValueTarget);
			object[] array34 = new object[0 + 8];
			array34[0] = span31;
			array34[1] = formattedString16;
			array34[2] = label22;
			array34[3] = stackLayout2;
			array34[4] = stackLayout3;
			array34[5] = grid;
			array34[6] = scrollView;
			array34[7] = this;
			object obj60;
			xamlServiceProvider34.Add(typeFromHandle67, obj60 = new SimpleValueTargetProvider(array34, Span.TextProperty, nameScope));
			xamlServiceProvider34.Add(typeof(IReferenceProvider), obj60);
			Type typeFromHandle68 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver34 = new XmlNamespaceResolver();
			xmlNamespaceResolver34.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver34.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver34.Add("coding", "clr-namespace:CarScannerXamarinForms.Coding");
			xmlNamespaceResolver34.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver34.Add("d", "http://xamarin.com/schemas/2014/forms/design");
			xmlNamespaceResolver34.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver34.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver34.Add("mc", "http://schemas.openxmlformats.org/markup-compatibility/2006");
			xmlNamespaceResolver34.Add("settings", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver34.Add("syncfusion", "clr-namespace:Syncfusion.XForms.Buttons;assembly=Syncfusion.Buttons.XForms");
			xmlNamespaceResolver34.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider34.Add(typeFromHandle68, new XamlTypeResolver(xmlNamespaceResolver34, typeof(CodingLightConfigurationPage).GetTypeInfo().Assembly));
			xamlServiceProvider34.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(275, 43)));
			object obj61 = markupExtension34.ProvideValue(xamlServiceProvider34);
			span31.Text = obj61;
			formattedString16.Spans.Add(span31);
			bindingExtension28.Path = "PasswordHint";
			BindingBase bindingBase28 = bindingExtension28.ProvideValue(null);
			span32.SetBinding(Span.TextProperty, bindingBase28);
			formattedString16.Spans.Add(span32);
			label22.SetValue(Label.FormattedTextProperty, formattedString16);
			stackLayout2.Children.Add(label22);
			stackLayout3.Children.Add(stackLayout2);
			button.Clicked += this.btnSaveState_Clicked;
			translate23.Text = "coding_Apply";
			IMarkupExtension markupExtension35 = translate23;
			XamlServiceProvider xamlServiceProvider35 = new XamlServiceProvider();
			Type typeFromHandle69 = typeof(IProvideValueTarget);
			object[] array35 = new object[0 + 5];
			array35[0] = button;
			array35[1] = stackLayout3;
			array35[2] = grid;
			array35[3] = scrollView;
			array35[4] = this;
			object obj62;
			xamlServiceProvider35.Add(typeFromHandle69, obj62 = new SimpleValueTargetProvider(array35, Button.TextProperty, nameScope));
			xamlServiceProvider35.Add(typeof(IReferenceProvider), obj62);
			Type typeFromHandle70 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver35 = new XmlNamespaceResolver();
			xmlNamespaceResolver35.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver35.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver35.Add("coding", "clr-namespace:CarScannerXamarinForms.Coding");
			xmlNamespaceResolver35.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver35.Add("d", "http://xamarin.com/schemas/2014/forms/design");
			xmlNamespaceResolver35.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver35.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver35.Add("mc", "http://schemas.openxmlformats.org/markup-compatibility/2006");
			xmlNamespaceResolver35.Add("settings", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver35.Add("syncfusion", "clr-namespace:Syncfusion.XForms.Buttons;assembly=Syncfusion.Buttons.XForms");
			xmlNamespaceResolver35.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider35.Add(typeFromHandle70, new XamlTypeResolver(xmlNamespaceResolver35, typeof(CodingLightConfigurationPage).GetTypeInfo().Assembly));
			xamlServiceProvider35.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(286, 25)));
			object obj63 = markupExtension35.ProvideValue(xamlServiceProvider35);
			button.Text = obj63;
			stackLayout3.Children.Add(button);
			button2.Clicked += this.BtnUpdateState_Clicked;
			translate24.Text = "Mode06Page_btnRefresh.Content";
			IMarkupExtension markupExtension36 = translate24;
			XamlServiceProvider xamlServiceProvider36 = new XamlServiceProvider();
			Type typeFromHandle71 = typeof(IProvideValueTarget);
			object[] array36 = new object[0 + 5];
			array36[0] = button2;
			array36[1] = stackLayout3;
			array36[2] = grid;
			array36[3] = scrollView;
			array36[4] = this;
			object obj64;
			xamlServiceProvider36.Add(typeFromHandle71, obj64 = new SimpleValueTargetProvider(array36, Button.TextProperty, nameScope));
			xamlServiceProvider36.Add(typeof(IReferenceProvider), obj64);
			Type typeFromHandle72 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver36 = new XmlNamespaceResolver();
			xmlNamespaceResolver36.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver36.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver36.Add("coding", "clr-namespace:CarScannerXamarinForms.Coding");
			xmlNamespaceResolver36.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver36.Add("d", "http://xamarin.com/schemas/2014/forms/design");
			xmlNamespaceResolver36.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver36.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver36.Add("mc", "http://schemas.openxmlformats.org/markup-compatibility/2006");
			xmlNamespaceResolver36.Add("settings", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver36.Add("syncfusion", "clr-namespace:Syncfusion.XForms.Buttons;assembly=Syncfusion.Buttons.XForms");
			xmlNamespaceResolver36.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider36.Add(typeFromHandle72, new XamlTypeResolver(xmlNamespaceResolver36, typeof(CodingLightConfigurationPage).GetTypeInfo().Assembly));
			xamlServiceProvider36.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(291, 25)));
			object obj65 = markupExtension36.ProvideValue(xamlServiceProvider36);
			button2.Text = obj65;
			stackLayout3.Children.Add(button2);
			label23.SetValue(BindableObject.BindingContextProperty, sharedSettings);
			label23.SetValue(Label.FontAttributesProperty, new FontAttributesConverter().ConvertFromInvariantString("Italic"));
			staticResourceExtension6.Key = "VagCodingPlatformToTrueConverter";
			IMarkupExtension markupExtension37 = staticResourceExtension6;
			XamlServiceProvider xamlServiceProvider37 = new XamlServiceProvider();
			Type typeFromHandle73 = typeof(IProvideValueTarget);
			object[] array37 = new object[0 + 6];
			array37[0] = bindingExtension29;
			array37[1] = label23;
			array37[2] = stackLayout3;
			array37[3] = grid;
			array37[4] = scrollView;
			array37[5] = this;
			object obj66;
			xamlServiceProvider37.Add(typeFromHandle73, obj66 = new SimpleValueTargetProvider(array37, typeof(BindingExtension).GetRuntimeProperty("Converter"), nameScope));
			xamlServiceProvider37.Add(typeof(IReferenceProvider), obj66);
			Type typeFromHandle74 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver37 = new XmlNamespaceResolver();
			xmlNamespaceResolver37.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver37.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver37.Add("coding", "clr-namespace:CarScannerXamarinForms.Coding");
			xmlNamespaceResolver37.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver37.Add("d", "http://xamarin.com/schemas/2014/forms/design");
			xmlNamespaceResolver37.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver37.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver37.Add("mc", "http://schemas.openxmlformats.org/markup-compatibility/2006");
			xmlNamespaceResolver37.Add("settings", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver37.Add("syncfusion", "clr-namespace:Syncfusion.XForms.Buttons;assembly=Syncfusion.Buttons.XForms");
			xmlNamespaceResolver37.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider37.Add(typeFromHandle74, new XamlTypeResolver(xmlNamespaceResolver37, typeof(CodingLightConfigurationPage).GetTypeInfo().Assembly));
			xamlServiceProvider37.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(295, 25)));
			object obj67 = markupExtension37.ProvideValue(xamlServiceProvider37);
			bindingExtension29.Converter = obj67;
			bindingExtension29.Path = "CodingLastPlatformSelected";
			BindingBase bindingBase29 = bindingExtension29.ProvideValue(null);
			label23.SetBinding(VisualElement.IsVisibleProperty, bindingBase29);
			translate25.Text = "coding_VagOpenHoodWarning";
			IMarkupExtension markupExtension38 = translate25;
			XamlServiceProvider xamlServiceProvider38 = new XamlServiceProvider();
			Type typeFromHandle75 = typeof(IProvideValueTarget);
			object[] array38 = new object[0 + 5];
			array38[0] = label23;
			array38[1] = stackLayout3;
			array38[2] = grid;
			array38[3] = scrollView;
			array38[4] = this;
			object obj68;
			xamlServiceProvider38.Add(typeFromHandle75, obj68 = new SimpleValueTargetProvider(array38, Label.TextProperty, nameScope));
			xamlServiceProvider38.Add(typeof(IReferenceProvider), obj68);
			Type typeFromHandle76 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver38 = new XmlNamespaceResolver();
			xmlNamespaceResolver38.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver38.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver38.Add("coding", "clr-namespace:CarScannerXamarinForms.Coding");
			xmlNamespaceResolver38.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver38.Add("d", "http://xamarin.com/schemas/2014/forms/design");
			xmlNamespaceResolver38.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver38.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver38.Add("mc", "http://schemas.openxmlformats.org/markup-compatibility/2006");
			xmlNamespaceResolver38.Add("settings", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver38.Add("syncfusion", "clr-namespace:Syncfusion.XForms.Buttons;assembly=Syncfusion.Buttons.XForms");
			xmlNamespaceResolver38.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider38.Add(typeFromHandle76, new XamlTypeResolver(xmlNamespaceResolver38, typeof(CodingLightConfigurationPage).GetTypeInfo().Assembly));
			xamlServiceProvider38.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(296, 25)));
			object obj69 = markupExtension38.ProvideValue(xamlServiceProvider38);
			label23.Text = obj69;
			stackLayout3.Children.Add(label23);
			labelSwitch.SetValue(BindableObject.BindingContextProperty, sharedSettings2);
			bindingExtension30.Mode = 1;
			bindingExtension30.Path = "IgnoreCodingErrors";
			BindingBase bindingBase30 = bindingExtension30.ProvideValue(null);
			labelSwitch.SetBinding(LabelSwitch.IsToggledProperty, bindingBase30);
			bindingExtension31.Path = "ShowExperimental";
			BindingBase bindingBase31 = bindingExtension31.ProvideValue(null);
			labelSwitch.SetBinding(VisualElement.IsVisibleProperty, bindingBase31);
			translate26.Text = "coding_ignore_fails";
			IMarkupExtension markupExtension39 = translate26;
			XamlServiceProvider xamlServiceProvider39 = new XamlServiceProvider();
			Type typeFromHandle77 = typeof(IProvideValueTarget);
			object[] array39 = new object[0 + 5];
			array39[0] = labelSwitch;
			array39[1] = stackLayout3;
			array39[2] = grid;
			array39[3] = scrollView;
			array39[4] = this;
			object obj70;
			xamlServiceProvider39.Add(typeFromHandle77, obj70 = new SimpleValueTargetProvider(array39, LabelSwitch.TextProperty, nameScope));
			xamlServiceProvider39.Add(typeof(IReferenceProvider), obj70);
			Type typeFromHandle78 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver39 = new XmlNamespaceResolver();
			xmlNamespaceResolver39.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver39.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver39.Add("coding", "clr-namespace:CarScannerXamarinForms.Coding");
			xmlNamespaceResolver39.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver39.Add("d", "http://xamarin.com/schemas/2014/forms/design");
			xmlNamespaceResolver39.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver39.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver39.Add("mc", "http://schemas.openxmlformats.org/markup-compatibility/2006");
			xmlNamespaceResolver39.Add("settings", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver39.Add("syncfusion", "clr-namespace:Syncfusion.XForms.Buttons;assembly=Syncfusion.Buttons.XForms");
			xmlNamespaceResolver39.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider39.Add(typeFromHandle78, new XamlTypeResolver(xmlNamespaceResolver39, typeof(CodingLightConfigurationPage).GetTypeInfo().Assembly));
			xamlServiceProvider39.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(301, 25)));
			object obj71 = markupExtension39.ProvideValue(xamlServiceProvider39);
			labelSwitch.Text = obj71;
			stackLayout3.Children.Add(labelSwitch);
			grid.Children.Add(stackLayout3);
			activityFrame.SetValue(Grid.RowProperty, 0);
			grid.Children.Add(activityFrame);
			scrollView.Content = grid;
			this.SetValue(ContentPage.ContentProperty, scrollView);
		}

		// Token: 0x06004D4D RID: 19789 RVA: 0x0039797B File Offset: 0x00395B7B
		[CompilerGenerated]
		private void <btnSaveState_Clicked>b__6_0(string s)
		{
			Device.BeginInvokeOnMainThread(delegate
			{
				this.activityFrame.Text = s;
			});
		}

		// Token: 0x06004D4E RID: 19790 RVA: 0x003979A0 File Offset: 0x00395BA0
		[CompilerGenerated]
		private void <lbLampType_Tapped>b__7_1(ValueItemWithTranslation val)
		{
			this.coding.LightConfiguration.LampType = (MQB_LampType)val;
		}

		// Token: 0x06004D4F RID: 19791 RVA: 0x003979B8 File Offset: 0x00395BB8
		private void __InitComponentRuntime()
		{
			Extensions.LoadFromXaml<CodingLightConfigurationPage>(this, typeof(CodingLightConfigurationPage));
			this.labelNotSupported = NameScopeExtensions.FindByName<Label>(this, "labelNotSupported");
			this.stackConfiguration = NameScopeExtensions.FindByName<StackLayout>(this, "stackConfiguration");
			this.entryPassword = NameScopeExtensions.FindByName<Entry>(this, "entryPassword");
			this.btnSaveNewValue = NameScopeExtensions.FindByName<Button>(this, "btnSaveNewValue");
			this.btnUpdateState = NameScopeExtensions.FindByName<Button>(this, "btnUpdateState");
			this.activityFrame = NameScopeExtensions.FindByName<ActivityFrame>(this, "activityFrame");
		}

		// Token: 0x04002DC2 RID: 11714
		private MQB_LightConfigurationCoding coding;

		// Token: 0x04002DC3 RID: 11715
		private bool first_appearing = true;

		// Token: 0x04002DC4 RID: 11716
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private Label labelNotSupported;

		// Token: 0x04002DC5 RID: 11717
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private StackLayout stackConfiguration;

		// Token: 0x04002DC6 RID: 11718
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private Entry entryPassword;

		// Token: 0x04002DC7 RID: 11719
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private Button btnSaveNewValue;

		// Token: 0x04002DC8 RID: 11720
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private Button btnUpdateState;

		// Token: 0x04002DC9 RID: 11721
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private ActivityFrame activityFrame;

		// Token: 0x020008F8 RID: 2296
		[CompilerGenerated]
		[Serializable]
		private sealed class <>c
		{
			// Token: 0x06004D50 RID: 19792 RVA: 0x00397A3C File Offset: 0x00395C3C
			// Note: this type is marked as 'beforefieldinit'.
			static <>c()
			{
			}

			// Token: 0x06004D51 RID: 19793 RVA: 0x00002050 File Offset: 0x00000250
			public <>c()
			{
			}

			// Token: 0x06004D52 RID: 19794 RVA: 0x00016849 File Offset: 0x00014A49
			internal ValueItemWithTranslation <lbLampType_Tapped>b__7_0(MQB_LampType x)
			{
				return x;
			}

			// Token: 0x06004D53 RID: 19795 RVA: 0x00016849 File Offset: 0x00014A49
			internal ValueItemWithTranslation <lbFunctionSelector_Tapped>b__8_1(MQB_LightFunction x)
			{
				return x;
			}

			// Token: 0x04002DCA RID: 11722
			public static readonly CodingLightConfigurationPage.<>c <>9 = new CodingLightConfigurationPage.<>c();

			// Token: 0x04002DCB RID: 11723
			public static Func<MQB_LampType, ValueItemWithTranslation> <>9__7_0;

			// Token: 0x04002DCC RID: 11724
			public static Func<MQB_LightFunction, ValueItemWithTranslation> <>9__8_1;
		}

		// Token: 0x020008F9 RID: 2297
		[CompilerGenerated]
		private sealed class <>c__DisplayClass6_0
		{
			// Token: 0x06004D54 RID: 19796 RVA: 0x00002050 File Offset: 0x00000250
			public <>c__DisplayClass6_0()
			{
			}

			// Token: 0x06004D55 RID: 19797 RVA: 0x00397A48 File Offset: 0x00395C48
			internal void <btnSaveState_Clicked>b__1()
			{
				this.<>4__this.activityFrame.Text = this.s;
			}

			// Token: 0x04002DCD RID: 11725
			public string s;

			// Token: 0x04002DCE RID: 11726
			public CodingLightConfigurationPage <>4__this;
		}

		// Token: 0x020008FA RID: 2298
		[CompilerGenerated]
		private sealed class <>c__DisplayClass8_0
		{
			// Token: 0x06004D56 RID: 19798 RVA: 0x00002050 File Offset: 0x00000250
			public <>c__DisplayClass8_0()
			{
			}

			// Token: 0x06004D57 RID: 19799 RVA: 0x00397A60 File Offset: 0x00395C60
			internal void <lbFunctionSelector_Tapped>b__0(ValueItemWithTranslation selected)
			{
				MQB_LightFunction mqb_LightFunction = (MQB_LightFunction)selected;
				string text = this.tag;
				if (text != null)
				{
					int length = text.Length;
					if (length == 1)
					{
						switch (text[0])
						{
						case 'A':
							this.<>4__this.coding.LightConfiguration.FunctionA = mqb_LightFunction;
							return;
						case 'B':
							this.<>4__this.coding.LightConfiguration.FunctionB = mqb_LightFunction;
							return;
						case 'C':
							this.<>4__this.coding.LightConfiguration.FunctionC = mqb_LightFunction;
							return;
						case 'D':
							this.<>4__this.coding.LightConfiguration.FunctionD = mqb_LightFunction;
							return;
						case 'E':
							this.<>4__this.coding.LightConfiguration.FunctionE = mqb_LightFunction;
							return;
						case 'F':
							this.<>4__this.coding.LightConfiguration.FunctionF = mqb_LightFunction;
							return;
						case 'G':
							this.<>4__this.coding.LightConfiguration.FunctionG = mqb_LightFunction;
							return;
						case 'H':
							this.<>4__this.coding.LightConfiguration.FunctionH = mqb_LightFunction;
							break;
						default:
							return;
						}
					}
				}
			}

			// Token: 0x04002DCF RID: 11727
			public string tag;

			// Token: 0x04002DD0 RID: 11728
			public CodingLightConfigurationPage <>4__this;
		}

		// Token: 0x020008FB RID: 2299
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <BtnUpdateState_Clicked>d__5 : IAsyncStateMachine
		{
			// Token: 0x06004D58 RID: 19800 RVA: 0x00397B78 File Offset: 0x00395D78
			void IAsyncStateMachine.MoveNext()
			{
				CodingLightConfigurationPage codingLightConfigurationPage = this;
				try
				{
					codingLightConfigurationPage.UpdateState();
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

			// Token: 0x06004D59 RID: 19801 RVA: 0x00397BD0 File Offset: 0x00395DD0
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x04002DD1 RID: 11729
			public int <>1__state;

			// Token: 0x04002DD2 RID: 11730
			public AsyncVoidMethodBuilder <>t__builder;

			// Token: 0x04002DD3 RID: 11731
			public CodingLightConfigurationPage <>4__this;
		}

		// Token: 0x020008FC RID: 2300
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <CodingDetailsPage_Appearing>d__3 : IAsyncStateMachine
		{
			// Token: 0x06004D5A RID: 19802 RVA: 0x00397BE0 File Offset: 0x00395DE0
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				CodingLightConfigurationPage codingLightConfigurationPage = this;
				try
				{
					TaskAwaiter taskAwaiter;
					if (num != 0)
					{
						if (!codingLightConfigurationPage.first_appearing)
						{
							goto IL_0078;
						}
						codingLightConfigurationPage.first_appearing = false;
						taskAwaiter = codingLightConfigurationPage.UpdateState().GetAwaiter();
						if (!taskAwaiter.IsCompleted)
						{
							num2 = 0;
							TaskAwaiter taskAwaiter2 = taskAwaiter;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, CodingLightConfigurationPage.<CodingDetailsPage_Appearing>d__3>(ref taskAwaiter, ref this);
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
					IL_0078:;
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

			// Token: 0x06004D5B RID: 19803 RVA: 0x00397CA4 File Offset: 0x00395EA4
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x04002DD4 RID: 11732
			public int <>1__state;

			// Token: 0x04002DD5 RID: 11733
			public AsyncVoidMethodBuilder <>t__builder;

			// Token: 0x04002DD6 RID: 11734
			public CodingLightConfigurationPage <>4__this;

			// Token: 0x04002DD7 RID: 11735
			private TaskAwaiter <>u__1;
		}

		// Token: 0x020008FD RID: 2301
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <UpdateState>d__4 : IAsyncStateMachine
		{
			// Token: 0x06004D5C RID: 19804 RVA: 0x00397CB4 File Offset: 0x00395EB4
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				CodingLightConfigurationPage codingLightConfigurationPage = this;
				try
				{
					TaskAwaiter<CodingRequestResult> taskAwaiter3;
					if (num != 0)
					{
						codingLightConfigurationPage.activityFrame.IsVisible = true;
						taskAwaiter3 = codingLightConfigurationPage.coding.UpdateCurrentState("", null).GetAwaiter();
						if (!taskAwaiter3.IsCompleted)
						{
							num2 = 0;
							taskAwaiter2 = taskAwaiter3;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<CodingRequestResult>, CodingLightConfigurationPage.<UpdateState>d__4>(ref taskAwaiter3, ref this);
							return;
						}
					}
					else
					{
						taskAwaiter3 = taskAwaiter2;
						taskAwaiter2 = default(TaskAwaiter<CodingRequestResult>);
						num2 = -1;
					}
					if (taskAwaiter3.GetResult() != CodingRequestResult.Success)
					{
						codingLightConfigurationPage.labelNotSupported.IsVisible = true;
						codingLightConfigurationPage.stackConfiguration.IsEnabled = false;
					}
					else
					{
						codingLightConfigurationPage.labelNotSupported.IsVisible = false;
						codingLightConfigurationPage.stackConfiguration.IsEnabled = true;
					}
					codingLightConfigurationPage.activityFrame.IsVisible = false;
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

			// Token: 0x06004D5D RID: 19805 RVA: 0x00397DC0 File Offset: 0x00395FC0
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x04002DD8 RID: 11736
			public int <>1__state;

			// Token: 0x04002DD9 RID: 11737
			public AsyncTaskMethodBuilder <>t__builder;

			// Token: 0x04002DDA RID: 11738
			public CodingLightConfigurationPage <>4__this;

			// Token: 0x04002DDB RID: 11739
			private TaskAwaiter<CodingRequestResult> <>u__1;
		}

		// Token: 0x020008FE RID: 2302
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <btnSaveState_Clicked>d__6 : IAsyncStateMachine
		{
			// Token: 0x06004D5E RID: 19806 RVA: 0x00397DD0 File Offset: 0x00395FD0
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				CodingLightConfigurationPage codingLightConfigurationPage = this;
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
						goto IL_0333;
					}
					case 5:
					{
						TaskAwaiter taskAwaiter5;
						taskAwaiter4 = taskAwaiter5;
						taskAwaiter5 = default(TaskAwaiter);
						num2 = -1;
						goto IL_03C5;
					}
					case 6:
					{
						TaskAwaiter<CodingRequestResult> taskAwaiter7;
						taskAwaiter6 = taskAwaiter7;
						taskAwaiter7 = default(TaskAwaiter<CodingRequestResult>);
						num2 = -1;
						goto IL_042E;
					}
					default:
						if (codingLightConfigurationPage.coding.RequiresPro && !SharedSettings.Current.AdsProductPurchased)
						{
							taskAwaiter3 = codingLightConfigurationPage.DisplayAlert("Car Scanner Pro", Translate.GetString("FeatureLocked"), "OK", Translate.GetString("btnCancel.Content")).GetAwaiter();
							if (!taskAwaiter3.IsCompleted)
							{
								num2 = 0;
								taskAwaiter2 = taskAwaiter3;
								this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<bool>, CodingLightConfigurationPage.<btnSaveState_Clicked>d__6>(ref taskAwaiter3, ref this);
								return;
							}
						}
						else if (SharedSettings.Current.CodingsCounter >= 3 && !SharedSettings.Current.AdsProductPurchased)
						{
							taskAwaiter3 = codingLightConfigurationPage.DisplayAlert("Car Scanner Pro", string.Format(Translate.GetString("coding_FreeCodingsFinished"), SharedSettings.Current.CodingsCounter, 3), "OK", Translate.GetString("btnCancel.Content")).GetAwaiter();
							if (!taskAwaiter3.IsCompleted)
							{
								num2 = 2;
								taskAwaiter2 = taskAwaiter3;
								this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<bool>, CodingLightConfigurationPage.<btnSaveState_Clicked>d__6>(ref taskAwaiter3, ref this);
								return;
							}
							goto IL_01EC;
						}
						else
						{
							codingLightConfigurationPage.activityFrame.IsVisible = true;
							codingLightConfigurationPage.stackConfiguration.IsEnabled = false;
							codingLightConfigurationPage.btnSaveNewValue.IsEnabled = false;
							codingLightConfigurationPage.btnUpdateState.IsEnabled = false;
							Progress<string> progress = new Progress<string>(delegate(string s)
							{
								Device.BeginInvokeOnMainThread(new Action(new CodingLightConfigurationPage.<>c__DisplayClass6_0
								{
									<>4__this = codingLightConfigurationPage,
									s = s
								}.<btnSaveState_Clicked>b__1));
							});
							string text = BitHelpers.ByteArrayToHexString(codingLightConfigurationPage.coding.LightConfiguration.ApplyToData());
							taskAwaiter6 = codingLightConfigurationPage.coding.Execute(codingLightConfigurationPage.entryPassword.Text, text, Translate.GetString("coding_ManualLightCustomization"), progress, null, false).GetAwaiter();
							if (!taskAwaiter6.IsCompleted)
							{
								num2 = 4;
								TaskAwaiter<CodingRequestResult> taskAwaiter7 = taskAwaiter6;
								this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<CodingRequestResult>, CodingLightConfigurationPage.<btnSaveState_Clicked>d__6>(ref taskAwaiter6, ref this);
								return;
							}
							goto IL_0333;
						}
						break;
					}
					if (!taskAwaiter3.GetResult())
					{
						goto IL_0139;
					}
					Page inAppPage = InAppManager.GetInAppPage();
					taskAwaiter4 = codingLightConfigurationPage.Navigation.PushAsync(inAppPage).GetAwaiter();
					if (!taskAwaiter4.IsCompleted)
					{
						num2 = 1;
						TaskAwaiter taskAwaiter5 = taskAwaiter4;
						this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, CodingLightConfigurationPage.<btnSaveState_Clicked>d__6>(ref taskAwaiter4, ref this);
						return;
					}
					IL_0132:
					taskAwaiter4.GetResult();
					IL_0139:
					goto IL_0485;
					IL_01EC:
					if (!taskAwaiter3.GetResult())
					{
						goto IL_0261;
					}
					Page inAppPage2 = InAppManager.GetInAppPage();
					taskAwaiter4 = codingLightConfigurationPage.Navigation.PushAsync(inAppPage2).GetAwaiter();
					if (!taskAwaiter4.IsCompleted)
					{
						num2 = 3;
						TaskAwaiter taskAwaiter5 = taskAwaiter4;
						this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, CodingLightConfigurationPage.<btnSaveState_Clicked>d__6>(ref taskAwaiter4, ref this);
						return;
					}
					IL_025A:
					taskAwaiter4.GetResult();
					IL_0261:
					goto IL_0485;
					IL_0333:
					CodingRequestResult result = taskAwaiter6.GetResult();
					if (result == CodingRequestResult.Success)
					{
						SharedSettings sharedSettings = SharedSettings.Current;
						int codingsCounter = sharedSettings.CodingsCounter;
						sharedSettings.CodingsCounter = codingsCounter + 1;
						goto IL_03CC;
					}
					taskAwaiter4 = codingLightConfigurationPage.DisplayAlert(Translate.GetString("coding_Error"), MQBAdaptationTemplate.CodingRequestResultToString(result), "OK").GetAwaiter();
					if (!taskAwaiter4.IsCompleted)
					{
						num2 = 5;
						TaskAwaiter taskAwaiter5 = taskAwaiter4;
						this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, CodingLightConfigurationPage.<btnSaveState_Clicked>d__6>(ref taskAwaiter4, ref this);
						return;
					}
					IL_03C5:
					taskAwaiter4.GetResult();
					IL_03CC:
					taskAwaiter6 = codingLightConfigurationPage.coding.UpdateCurrentState("", null).GetAwaiter();
					if (!taskAwaiter6.IsCompleted)
					{
						num2 = 6;
						TaskAwaiter<CodingRequestResult> taskAwaiter7 = taskAwaiter6;
						this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<CodingRequestResult>, CodingLightConfigurationPage.<btnSaveState_Clicked>d__6>(ref taskAwaiter6, ref this);
						return;
					}
					IL_042E:
					taskAwaiter6.GetResult();
					codingLightConfigurationPage.activityFrame.IsVisible = false;
					codingLightConfigurationPage.btnSaveNewValue.IsEnabled = true;
					codingLightConfigurationPage.btnUpdateState.IsEnabled = true;
					codingLightConfigurationPage.activityFrame.Text = ActivityFrame.PLEASE_WAIT_TEXT;
				}
				catch (Exception ex)
				{
					num2 = -2;
					this.<>t__builder.SetException(ex);
					return;
				}
				IL_0485:
				num2 = -2;
				this.<>t__builder.SetResult();
			}

			// Token: 0x06004D5F RID: 19807 RVA: 0x00398294 File Offset: 0x00396494
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x04002DDC RID: 11740
			public int <>1__state;

			// Token: 0x04002DDD RID: 11741
			public AsyncVoidMethodBuilder <>t__builder;

			// Token: 0x04002DDE RID: 11742
			public CodingLightConfigurationPage <>4__this;

			// Token: 0x04002DDF RID: 11743
			private TaskAwaiter<bool> <>u__1;

			// Token: 0x04002DE0 RID: 11744
			private TaskAwaiter <>u__2;

			// Token: 0x04002DE1 RID: 11745
			private TaskAwaiter<CodingRequestResult> <>u__3;
		}

		// Token: 0x020008FF RID: 2303
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <lbFunctionSelector_Tapped>d__8 : IAsyncStateMachine
		{
			// Token: 0x06004D60 RID: 19808 RVA: 0x003982A4 File Offset: 0x003964A4
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				CodingLightConfigurationPage codingLightConfigurationPage = this;
				try
				{
					TaskAwaiter taskAwaiter;
					if (num != 0)
					{
						CodingLightConfigurationPage.<>c__DisplayClass8_0 CS$<>8__locals1 = new CodingLightConfigurationPage.<>c__DisplayClass8_0();
						CS$<>8__locals1.<>4__this = this;
						LabelWithBorder labelWithBorder = (LabelWithBorder)sender;
						CS$<>8__locals1.tag = labelWithBorder.Tag.ToUpperInvariant();
						Action<ValueItemWithTranslation> action = delegate(ValueItemWithTranslation selected)
						{
							MQB_LightFunction mqb_LightFunction2 = (MQB_LightFunction)selected;
							string tag2 = CS$<>8__locals1.tag;
							if (tag2 != null)
							{
								int length2 = tag2.Length;
								if (length2 == 1)
								{
									switch (tag2[0])
									{
									case 'A':
										CS$<>8__locals1.<>4__this.coding.LightConfiguration.FunctionA = mqb_LightFunction2;
										return;
									case 'B':
										CS$<>8__locals1.<>4__this.coding.LightConfiguration.FunctionB = mqb_LightFunction2;
										return;
									case 'C':
										CS$<>8__locals1.<>4__this.coding.LightConfiguration.FunctionC = mqb_LightFunction2;
										return;
									case 'D':
										CS$<>8__locals1.<>4__this.coding.LightConfiguration.FunctionD = mqb_LightFunction2;
										return;
									case 'E':
										CS$<>8__locals1.<>4__this.coding.LightConfiguration.FunctionE = mqb_LightFunction2;
										return;
									case 'F':
										CS$<>8__locals1.<>4__this.coding.LightConfiguration.FunctionF = mqb_LightFunction2;
										return;
									case 'G':
										CS$<>8__locals1.<>4__this.coding.LightConfiguration.FunctionG = mqb_LightFunction2;
										return;
									case 'H':
										CS$<>8__locals1.<>4__this.coding.LightConfiguration.FunctionH = mqb_LightFunction2;
										break;
									default:
										return;
									}
								}
							}
						};
						MQB_LightFunction mqb_LightFunction = null;
						string tag = CS$<>8__locals1.tag;
						if (tag != null)
						{
							int length = tag.Length;
							if (length == 1)
							{
								switch (tag[0])
								{
								case 'A':
									mqb_LightFunction = codingLightConfigurationPage.coding.LightConfiguration.FunctionA;
									break;
								case 'B':
									mqb_LightFunction = codingLightConfigurationPage.coding.LightConfiguration.FunctionB;
									break;
								case 'C':
									mqb_LightFunction = codingLightConfigurationPage.coding.LightConfiguration.FunctionC;
									break;
								case 'D':
									mqb_LightFunction = codingLightConfigurationPage.coding.LightConfiguration.FunctionD;
									break;
								case 'E':
									mqb_LightFunction = codingLightConfigurationPage.coding.LightConfiguration.FunctionE;
									break;
								case 'F':
									mqb_LightFunction = codingLightConfigurationPage.coding.LightConfiguration.FunctionF;
									break;
								case 'G':
									mqb_LightFunction = codingLightConfigurationPage.coding.LightConfiguration.FunctionG;
									break;
								case 'H':
									mqb_LightFunction = codingLightConfigurationPage.coding.LightConfiguration.FunctionH;
									break;
								}
							}
						}
						ItemWithValueSelectorPage itemWithValueSelectorPage = new ItemWithValueSelectorPage(Translate.GetString("coding_Function") + " " + CS$<>8__locals1.tag, MQB_LightFunction.LightFunctionsList.Select((MQB_LightFunction x) => x), mqb_LightFunction, action);
						taskAwaiter = codingLightConfigurationPage.Navigation.PushAsync(itemWithValueSelectorPage).GetAwaiter();
						if (!taskAwaiter.IsCompleted)
						{
							num2 = 0;
							TaskAwaiter taskAwaiter2 = taskAwaiter;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, CodingLightConfigurationPage.<lbFunctionSelector_Tapped>d__8>(ref taskAwaiter, ref this);
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

			// Token: 0x06004D61 RID: 19809 RVA: 0x003984FC File Offset: 0x003966FC
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x04002DE2 RID: 11746
			public int <>1__state;

			// Token: 0x04002DE3 RID: 11747
			public AsyncVoidMethodBuilder <>t__builder;

			// Token: 0x04002DE4 RID: 11748
			public CodingLightConfigurationPage <>4__this;

			// Token: 0x04002DE5 RID: 11749
			public object sender;

			// Token: 0x04002DE6 RID: 11750
			private TaskAwaiter <>u__1;
		}
	}
}
