using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using CarScannerXamarinForms.Coding;
using CarScannerXamarinForms.Common.XAMLConverters;
using CarScannerXamarinForms.OBD2.PIDS;
using Syncfusion.XForms.Expander;
using Xamarin.Forms;
using Xamarin.Forms.Internals;
using Xamarin.Forms.PlatformConfiguration.iOSSpecific;
using Xamarin.Forms.Xaml;
using Xamarin.Forms.Xaml.Diagnostics;
using Xamarin.Forms.Xaml.Internals;

namespace CarScannerXamarinForms.Settings
{
	// Token: 0x020001F9 RID: 505
	[XamlCompilation(2)]
	[XamlFilePath("Settings\\CustomCodingEditor.xaml")]
	public class CustomCodingEditor : ContentPage
	{
		// Token: 0x06001A21 RID: 6689 RVA: 0x00118D0C File Offset: 0x00116F0C
		public CustomCodingEditor(CustomCodingsListViewModel codingsList, CustomizableCodingTemplate coding)
		{
			this.InitializeComponent();
			this.Coding = coding;
			this.CodingsList = codingsList;
			List<string> list = (from Enum x in Enum.GetValues(typeof(CodingGroup))
				select Translate.GetString("coding_Group_" + ((CodingGroup)x).ToString())).ToList<string>();
			this.pickerGroup.ItemsSource = list;
			base.BindingContext = this.Coding;
			base.Disappearing += this.CustomCodingEditor_Disappearing;
		}

		// Token: 0x06001A22 RID: 6690 RVA: 0x00118D9B File Offset: 0x00116F9B
		private void CustomCodingEditor_Disappearing(object sender, EventArgs e)
		{
			this.CodingsList.Save();
		}

		// Token: 0x17000F94 RID: 3988
		// (get) Token: 0x06001A23 RID: 6691 RVA: 0x00118DA8 File Offset: 0x00116FA8
		// (set) Token: 0x06001A24 RID: 6692 RVA: 0x00118DB0 File Offset: 0x00116FB0
		public CustomizableCodingTemplate Coding
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

		// Token: 0x06001A25 RID: 6693 RVA: 0x00118DBC File Offset: 0x00116FBC
		private void btnAddTranslation_Click(object sender, EventArgs e)
		{
			CustomizableCodingTemplate coding = this.Coding;
			TranslationItem translationItem = new TranslationItem();
			if (coding.Translations.Count == 0)
			{
				translationItem.Language = "ru";
			}
			else
			{
				translationItem.Language = "lang";
			}
			translationItem.Name = coding.Name;
			translationItem.ShortName = coding.Description;
			coding.Translations.Add(translationItem);
		}

		// Token: 0x06001A26 RID: 6694 RVA: 0x00118E20 File Offset: 0x00117020
		private void btnDelTranslation_Click(object sender, EventArgs e)
		{
			if (this.lvTranslations.SelectedItem != null)
			{
				TranslationItem translationItem = (TranslationItem)this.lvTranslations.SelectedItem;
				this.Coding.Translations.Remove(translationItem);
			}
		}

		// Token: 0x06001A27 RID: 6695 RVA: 0x00118E60 File Offset: 0x00117060
		private void btnAddOption_Click(object sender, EventArgs e)
		{
			MQBAdaptationOption mqbadaptationOption = new MQBAdaptationOption("New", "");
			this.Coding.Options.Add(mqbadaptationOption);
			this.lvOptions.SelectedItem = mqbadaptationOption;
			this.lvOptions.ScrollTo(mqbadaptationOption, 0, true);
		}

		// Token: 0x06001A28 RID: 6696 RVA: 0x00118EA8 File Offset: 0x001170A8
		private void btnDelOption_Click(object sender, EventArgs e)
		{
			if (this.lvOptions.SelectedItem != null)
			{
				MQBAdaptationOption mqbadaptationOption = (MQBAdaptationOption)this.lvOptions.SelectedItem;
				this.Coding.Options.Remove(mqbadaptationOption);
			}
		}

		// Token: 0x06001A29 RID: 6697 RVA: 0x00118EE8 File Offset: 0x001170E8
		private void btnUpOption_Click(object sender, EventArgs e)
		{
			if (this.lvOptions.SelectedItem != null)
			{
				MQBAdaptationOption mqbadaptationOption = (MQBAdaptationOption)this.lvOptions.SelectedItem;
				int num = this.Coding.Options.IndexOf(mqbadaptationOption);
				if (num > 0)
				{
					this.Coding.Options.Move(num, num - 1);
				}
			}
		}

		// Token: 0x06001A2A RID: 6698 RVA: 0x00118F40 File Offset: 0x00117140
		private void btnDownOption_Click(object sender, EventArgs e)
		{
			if (this.lvOptions.SelectedItem != null)
			{
				MQBAdaptationOption mqbadaptationOption = (MQBAdaptationOption)this.lvOptions.SelectedItem;
				int num = this.Coding.Options.IndexOf(mqbadaptationOption);
				if (num < this.Coding.Options.Count - 1)
				{
					this.Coding.Options.Move(num, num + 1);
				}
			}
		}

		// Token: 0x06001A2B RID: 6699 RVA: 0x00118FA8 File Offset: 0x001171A8
		private void btnGenRequestHeader_Click(object sender, EventArgs e)
		{
			CustomizableCodingTemplate coding = this.Coding;
			int num = int.Parse(coding.RequestHeader, NumberStyles.HexNumber);
			int num2 = 8;
			if (sender == this.btnGenRequestHeaderVag)
			{
				num2 = 106;
			}
			else if (sender == this.btnGenRequestHeaderRenault)
			{
				num2 = 32;
			}
			else if (sender == this.btnGenRequestHeaderGeneric)
			{
				num2 = 8;
			}
			if (coding.RequestHeader.StartsWith("7E"))
			{
				num2 = 8;
			}
			coding.ResponseHeader = (num + num2).ToString("X3");
		}

		// Token: 0x06001A2C RID: 6700 RVA: 0x00119020 File Offset: 0x00117220
		private void btnReadAddressToWriteAddress_Click(object sender, EventArgs e)
		{
			string text = this.Coding.ReadModeAndAddress.Substring(0, 2);
			string text2 = this.Coding.ReadModeAndAddress.Substring(2);
			string text3 = "2E";
			if (text == "22")
			{
				text3 = "2E";
			}
			else if (text == "21")
			{
				text3 = "3B";
			}
			this.Coding.WriteModeAndAddress = text3 + text2;
		}

		// Token: 0x06001A2D RID: 6701 RVA: 0x00119094 File Offset: 0x00117294
		private void btnAddOptionTranslation_Click(object sender, EventArgs e)
		{
			CustomizableCodingTemplate coding = this.Coding;
			MQBAdaptationOption mqbadaptationOption = (MQBAdaptationOption)this.lvOptions.SelectedItem;
			if (mqbadaptationOption == null)
			{
				return;
			}
			TranslationItem translationItem = new TranslationItem();
			if (mqbadaptationOption.Translations.Count == 0)
			{
				translationItem.Language = "ru";
			}
			else if (!mqbadaptationOption.Translations.Any((TranslationItem x) => x.Language == "de"))
			{
				translationItem.Language = "de";
			}
			else
			{
				translationItem.Language = "lang";
			}
			translationItem.Name = mqbadaptationOption.Title;
			translationItem.ShortName = "";
			mqbadaptationOption.Translations.Add(translationItem);
			this.lvOptionTranslations.SelectedItem = translationItem;
			this.lvOptionTranslations.ScrollTo(translationItem, 0, true);
		}

		// Token: 0x06001A2E RID: 6702 RVA: 0x00119160 File Offset: 0x00117360
		private void btnDelOptionTranslation_Click(object sender, EventArgs e)
		{
			if (this.lvOptions.SelectedItem != null && this.lvOptionTranslations.SelectedItem != null)
			{
				CustomizableCodingTemplate coding = this.Coding;
				MQBAdaptationOption mqbadaptationOption = (MQBAdaptationOption)this.lvOptions.SelectedItem;
				TranslationItem translationItem = (TranslationItem)this.lvOptionTranslations.SelectedItem;
				mqbadaptationOption.Translations.Remove(translationItem);
			}
		}

		// Token: 0x06001A2F RID: 6703 RVA: 0x001191BC File Offset: 0x001173BC
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private void InitializeComponent()
		{
			if (ResourceLoader.IsEnabled && ResourceLoader.CanProvideContentFor(new ResourceLoader.ResourceLoadingQuery
			{
				AssemblyName = typeof(CustomCodingEditor).GetTypeInfo().Assembly.GetName(),
				ResourcePath = "Settings/CustomCodingEditor.xaml",
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
			DynamicResourceExtension dynamicResourceExtension;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension = new DynamicResourceExtension(), new Uri("Settings\\CustomCodingEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 13, 5);
			UnitsToIntConverter unitsToIntConverter;
			VisualDiagnostics.RegisterSourceInfo(unitsToIntConverter = new UnitsToIntConverter(), new Uri("Settings\\CustomCodingEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 16, 14);
			BoolToNegativeConverter boolToNegativeConverter;
			VisualDiagnostics.RegisterSourceInfo(boolToNegativeConverter = new BoolToNegativeConverter(), new Uri("Settings\\CustomCodingEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 17, 14);
			DoubleToStringConverter doubleToStringConverter;
			VisualDiagnostics.RegisterSourceInfo(doubleToStringConverter = new DoubleToStringConverter(), new Uri("Settings\\CustomCodingEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 18, 14);
			CustomPIDTypeFormulaToTrue customPIDTypeFormulaToTrue;
			VisualDiagnostics.RegisterSourceInfo(customPIDTypeFormulaToTrue = new CustomPIDTypeFormulaToTrue(), new Uri("Settings\\CustomCodingEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 19, 14);
			EnumToIntConverter enumToIntConverter;
			VisualDiagnostics.RegisterSourceInfo(enumToIntConverter = new EnumToIntConverter(), new Uri("Settings\\CustomCodingEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 20, 14);
			EnumValueToTrueConverter enumValueToTrueConverter;
			VisualDiagnostics.RegisterSourceInfo(enumValueToTrueConverter = new EnumValueToTrueConverter(), new Uri("Settings\\CustomCodingEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 21, 14);
			EnumValueToFalseConverter enumValueToFalseConverter;
			VisualDiagnostics.RegisterSourceInfo(enumValueToFalseConverter = new EnumValueToFalseConverter(), new Uri("Settings\\CustomCodingEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 22, 14);
			IntToStringConverter intToStringConverter;
			VisualDiagnostics.RegisterSourceInfo(intToStringConverter = new IntToStringConverter(), new Uri("Settings\\CustomCodingEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 23, 14);
			CodingInputValueTypeToIntConverterForCustomCodingEditor codingInputValueTypeToIntConverterForCustomCodingEditor;
			VisualDiagnostics.RegisterSourceInfo(codingInputValueTypeToIntConverterForCustomCodingEditor = new CodingInputValueTypeToIntConverterForCustomCodingEditor(), new Uri("Settings\\CustomCodingEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 24, 14);
			ResourceDictionary resourceDictionary;
			VisualDiagnostics.RegisterSourceInfo(resourceDictionary = new ResourceDictionary(), new Uri("Settings\\CustomCodingEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 15, 10);
			DynamicResourceExtension dynamicResourceExtension2;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension2 = new DynamicResourceExtension(), new Uri("Settings\\CustomCodingEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 33, 21);
			DynamicResourceExtension dynamicResourceExtension3;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension3 = new DynamicResourceExtension(), new Uri("Settings\\CustomCodingEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 34, 21);
			DynamicResourceExtension dynamicResourceExtension4;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension4 = new DynamicResourceExtension(), new Uri("Settings\\CustomCodingEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 42, 33);
			Label label;
			VisualDiagnostics.RegisterSourceInfo(label = new Label(), new Uri("Settings\\CustomCodingEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 38, 30);
			Grid grid;
			VisualDiagnostics.RegisterSourceInfo(grid = new Grid(), new Uri("Settings\\CustomCodingEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 37, 26);
			Label label2;
			VisualDiagnostics.RegisterSourceInfo(label2 = new Label(), new Uri("Settings\\CustomCodingEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 50, 30);
			StaticResourceExtension staticResourceExtension;
			VisualDiagnostics.RegisterSourceInfo(staticResourceExtension = new StaticResourceExtension(), new Uri("Settings\\CustomCodingEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 51, 58);
			BindingExtension bindingExtension;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension = new BindingExtension(), new Uri("Settings\\CustomCodingEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 51, 58);
			Picker picker;
			VisualDiagnostics.RegisterSourceInfo(picker = new Picker(), new Uri("Settings\\CustomCodingEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 51, 30);
			Label label3;
			VisualDiagnostics.RegisterSourceInfo(label3 = new Label(), new Uri("Settings\\CustomCodingEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 52, 30);
			BindingExtension bindingExtension2;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension2 = new BindingExtension(), new Uri("Settings\\CustomCodingEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 53, 36);
			Entry entry;
			VisualDiagnostics.RegisterSourceInfo(entry = new Entry(), new Uri("Settings\\CustomCodingEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 53, 30);
			Label label4;
			VisualDiagnostics.RegisterSourceInfo(label4 = new Label(), new Uri("Settings\\CustomCodingEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 54, 30);
			BindingExtension bindingExtension3;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension3 = new BindingExtension(), new Uri("Settings\\CustomCodingEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 55, 36);
			Entry entry2;
			VisualDiagnostics.RegisterSourceInfo(entry2 = new Entry(), new Uri("Settings\\CustomCodingEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 55, 30);
			Label label5;
			VisualDiagnostics.RegisterSourceInfo(label5 = new Label(), new Uri("Settings\\CustomCodingEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 56, 30);
			BindingExtension bindingExtension4;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension4 = new BindingExtension(), new Uri("Settings\\CustomCodingEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 57, 36);
			Entry entry3;
			VisualDiagnostics.RegisterSourceInfo(entry3 = new Entry(), new Uri("Settings\\CustomCodingEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 57, 30);
			Label label6;
			VisualDiagnostics.RegisterSourceInfo(label6 = new Label(), new Uri("Settings\\CustomCodingEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 58, 30);
			BindingExtension bindingExtension5;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension5 = new BindingExtension(), new Uri("Settings\\CustomCodingEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 59, 36);
			Entry entry4;
			VisualDiagnostics.RegisterSourceInfo(entry4 = new Entry(), new Uri("Settings\\CustomCodingEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 59, 30);
			StackLayout stackLayout;
			VisualDiagnostics.RegisterSourceInfo(stackLayout = new StackLayout(), new Uri("Settings\\CustomCodingEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 48, 26);
			SfExpander sfExpander;
			VisualDiagnostics.RegisterSourceInfo(sfExpander = new SfExpander(), new Uri("Settings\\CustomCodingEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 30, 18);
			DynamicResourceExtension dynamicResourceExtension5;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension5 = new DynamicResourceExtension(), new Uri("Settings\\CustomCodingEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 66, 21);
			DynamicResourceExtension dynamicResourceExtension6;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension6 = new DynamicResourceExtension(), new Uri("Settings\\CustomCodingEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 67, 21);
			DynamicResourceExtension dynamicResourceExtension7;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension7 = new DynamicResourceExtension(), new Uri("Settings\\CustomCodingEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 75, 33);
			Label label7;
			VisualDiagnostics.RegisterSourceInfo(label7 = new Label(), new Uri("Settings\\CustomCodingEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 71, 30);
			Grid grid2;
			VisualDiagnostics.RegisterSourceInfo(grid2 = new Grid(), new Uri("Settings\\CustomCodingEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 70, 26);
			ColumnDefinition columnDefinition;
			VisualDiagnostics.RegisterSourceInfo(columnDefinition = new ColumnDefinition(), new Uri("Settings\\CustomCodingEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 83, 34);
			ColumnDefinition columnDefinition2;
			VisualDiagnostics.RegisterSourceInfo(columnDefinition2 = new ColumnDefinition(), new Uri("Settings\\CustomCodingEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 84, 34);
			BindingExtension bindingExtension6;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension6 = new BindingExtension(), new Uri("Settings\\CustomCodingEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 92, 37);
			DataTemplate dataTemplate;
			VisualDiagnostics.RegisterSourceInfo(dataTemplate = new DataTemplate(), new Uri("Settings\\CustomCodingEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 94, 42);
			ListView listView;
			VisualDiagnostics.RegisterSourceInfo(listView = new ListView(), new Uri("Settings\\CustomCodingEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 88, 34);
			ColumnDefinition columnDefinition3;
			VisualDiagnostics.RegisterSourceInfo(columnDefinition3 = new ColumnDefinition(), new Uri("Settings\\CustomCodingEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 101, 42);
			ColumnDefinition columnDefinition4;
			VisualDiagnostics.RegisterSourceInfo(columnDefinition4 = new ColumnDefinition(), new Uri("Settings\\CustomCodingEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 102, 42);
			Button button;
			VisualDiagnostics.RegisterSourceInfo(button = new Button(), new Uri("Settings\\CustomCodingEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 104, 38);
			Button button2;
			VisualDiagnostics.RegisterSourceInfo(button2 = new Button(), new Uri("Settings\\CustomCodingEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 109, 38);
			Grid grid3;
			VisualDiagnostics.RegisterSourceInfo(grid3 = new Grid(), new Uri("Settings\\CustomCodingEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 99, 34);
			StackLayout stackLayout2;
			VisualDiagnostics.RegisterSourceInfo(stackLayout2 = new StackLayout(), new Uri("Settings\\CustomCodingEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 86, 30);
			ReferenceExtension referenceExtension;
			VisualDiagnostics.RegisterSourceInfo(referenceExtension = new ReferenceExtension(), new Uri("Settings\\CustomCodingEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 118, 33);
			BindingExtension bindingExtension7;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension7 = new BindingExtension(), new Uri("Settings\\CustomCodingEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 118, 33);
			Label label8;
			VisualDiagnostics.RegisterSourceInfo(label8 = new Label(), new Uri("Settings\\CustomCodingEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 121, 38);
			BindingExtension bindingExtension8;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension8 = new BindingExtension(), new Uri("Settings\\CustomCodingEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 123, 41);
			Entry entry5;
			VisualDiagnostics.RegisterSourceInfo(entry5 = new Entry(), new Uri("Settings\\CustomCodingEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 122, 38);
			StackLayout stackLayout3;
			VisualDiagnostics.RegisterSourceInfo(stackLayout3 = new StackLayout(), new Uri("Settings\\CustomCodingEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 120, 34);
			Label label9;
			VisualDiagnostics.RegisterSourceInfo(label9 = new Label(), new Uri("Settings\\CustomCodingEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 128, 34);
			BindingExtension bindingExtension9;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension9 = new BindingExtension(), new Uri("Settings\\CustomCodingEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 129, 40);
			Entry entry6;
			VisualDiagnostics.RegisterSourceInfo(entry6 = new Entry(), new Uri("Settings\\CustomCodingEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 129, 34);
			Label label10;
			VisualDiagnostics.RegisterSourceInfo(label10 = new Label(), new Uri("Settings\\CustomCodingEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 130, 34);
			BindingExtension bindingExtension10;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension10 = new BindingExtension(), new Uri("Settings\\CustomCodingEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 131, 40);
			Entry entry7;
			VisualDiagnostics.RegisterSourceInfo(entry7 = new Entry(), new Uri("Settings\\CustomCodingEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 131, 34);
			Label label11;
			VisualDiagnostics.RegisterSourceInfo(label11 = new Label(), new Uri("Settings\\CustomCodingEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 132, 34);
			BindingExtension bindingExtension11;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension11 = new BindingExtension(), new Uri("Settings\\CustomCodingEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 133, 40);
			Entry entry8;
			VisualDiagnostics.RegisterSourceInfo(entry8 = new Entry(), new Uri("Settings\\CustomCodingEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 133, 34);
			StackLayout stackLayout4;
			VisualDiagnostics.RegisterSourceInfo(stackLayout4 = new StackLayout(), new Uri("Settings\\CustomCodingEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 116, 30);
			Grid grid4;
			VisualDiagnostics.RegisterSourceInfo(grid4 = new Grid(), new Uri("Settings\\CustomCodingEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 81, 26);
			SfExpander sfExpander2;
			VisualDiagnostics.RegisterSourceInfo(sfExpander2 = new SfExpander(), new Uri("Settings\\CustomCodingEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 63, 18);
			DynamicResourceExtension dynamicResourceExtension8;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension8 = new DynamicResourceExtension(), new Uri("Settings\\CustomCodingEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 143, 21);
			DynamicResourceExtension dynamicResourceExtension9;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension9 = new DynamicResourceExtension(), new Uri("Settings\\CustomCodingEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 144, 21);
			DynamicResourceExtension dynamicResourceExtension10;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension10 = new DynamicResourceExtension(), new Uri("Settings\\CustomCodingEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 152, 33);
			Label label12;
			VisualDiagnostics.RegisterSourceInfo(label12 = new Label(), new Uri("Settings\\CustomCodingEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 148, 30);
			Grid grid5;
			VisualDiagnostics.RegisterSourceInfo(grid5 = new Grid(), new Uri("Settings\\CustomCodingEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 147, 26);
			Label label13;
			VisualDiagnostics.RegisterSourceInfo(label13 = new Label(), new Uri("Settings\\CustomCodingEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 160, 34);
			BindingExtension bindingExtension12;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension12 = new BindingExtension(), new Uri("Settings\\CustomCodingEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 161, 40);
			Entry entry9;
			VisualDiagnostics.RegisterSourceInfo(entry9 = new Entry(), new Uri("Settings\\CustomCodingEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 161, 34);
			Label label14;
			VisualDiagnostics.RegisterSourceInfo(label14 = new Label(), new Uri("Settings\\CustomCodingEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 162, 34);
			BindingExtension bindingExtension13;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension13 = new BindingExtension(), new Uri("Settings\\CustomCodingEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 163, 53);
			Entry entry10;
			VisualDiagnostics.RegisterSourceInfo(entry10 = new Entry(), new Uri("Settings\\CustomCodingEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 163, 34);
			Button button3;
			VisualDiagnostics.RegisterSourceInfo(button3 = new Button(), new Uri("Settings\\CustomCodingEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 165, 38);
			Button button4;
			VisualDiagnostics.RegisterSourceInfo(button4 = new Button(), new Uri("Settings\\CustomCodingEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 170, 38);
			Button button5;
			VisualDiagnostics.RegisterSourceInfo(button5 = new Button(), new Uri("Settings\\CustomCodingEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 175, 38);
			StackLayout stackLayout5;
			VisualDiagnostics.RegisterSourceInfo(stackLayout5 = new StackLayout(), new Uri("Settings\\CustomCodingEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 164, 34);
			StackLayout stackLayout6;
			VisualDiagnostics.RegisterSourceInfo(stackLayout6 = new StackLayout(), new Uri("Settings\\CustomCodingEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 159, 30);
			Label label15;
			VisualDiagnostics.RegisterSourceInfo(label15 = new Label(), new Uri("Settings\\CustomCodingEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 185, 34);
			BindingExtension bindingExtension14;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension14 = new BindingExtension(), new Uri("Settings\\CustomCodingEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 186, 40);
			Entry entry11;
			VisualDiagnostics.RegisterSourceInfo(entry11 = new Entry(), new Uri("Settings\\CustomCodingEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 186, 34);
			Label label16;
			VisualDiagnostics.RegisterSourceInfo(label16 = new Label(), new Uri("Settings\\CustomCodingEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 187, 34);
			BindingExtension bindingExtension15;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension15 = new BindingExtension(), new Uri("Settings\\CustomCodingEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 188, 40);
			Entry entry12;
			VisualDiagnostics.RegisterSourceInfo(entry12 = new Entry(), new Uri("Settings\\CustomCodingEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 188, 34);
			StackLayout stackLayout7;
			VisualDiagnostics.RegisterSourceInfo(stackLayout7 = new StackLayout(), new Uri("Settings\\CustomCodingEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 184, 30);
			BindingExtension bindingExtension16;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension16 = new BindingExtension(), new Uri("Settings\\CustomCodingEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 190, 48);
			LabelSwitch labelSwitch;
			VisualDiagnostics.RegisterSourceInfo(labelSwitch = new LabelSwitch(), new Uri("Settings\\CustomCodingEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 190, 30);
			BindingExtension bindingExtension17;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension17 = new BindingExtension(), new Uri("Settings\\CustomCodingEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 191, 42);
			Label label17;
			VisualDiagnostics.RegisterSourceInfo(label17 = new Label(), new Uri("Settings\\CustomCodingEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 192, 34);
			BindingExtension bindingExtension18;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension18 = new BindingExtension(), new Uri("Settings\\CustomCodingEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 193, 40);
			Entry entry13;
			VisualDiagnostics.RegisterSourceInfo(entry13 = new Entry(), new Uri("Settings\\CustomCodingEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 193, 34);
			Label label18;
			VisualDiagnostics.RegisterSourceInfo(label18 = new Label(), new Uri("Settings\\CustomCodingEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 194, 34);
			BindingExtension bindingExtension19;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension19 = new BindingExtension(), new Uri("Settings\\CustomCodingEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 195, 40);
			Entry entry14;
			VisualDiagnostics.RegisterSourceInfo(entry14 = new Entry(), new Uri("Settings\\CustomCodingEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 195, 34);
			StackLayout stackLayout8;
			VisualDiagnostics.RegisterSourceInfo(stackLayout8 = new StackLayout(), new Uri("Settings\\CustomCodingEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 191, 30);
			ColumnDefinition columnDefinition5;
			VisualDiagnostics.RegisterSourceInfo(columnDefinition5 = new ColumnDefinition(), new Uri("Settings\\CustomCodingEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 200, 38);
			ColumnDefinition columnDefinition6;
			VisualDiagnostics.RegisterSourceInfo(columnDefinition6 = new ColumnDefinition(), new Uri("Settings\\CustomCodingEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 201, 38);
			ColumnDefinition columnDefinition7;
			VisualDiagnostics.RegisterSourceInfo(columnDefinition7 = new ColumnDefinition(), new Uri("Settings\\CustomCodingEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 202, 38);
			Label label19;
			VisualDiagnostics.RegisterSourceInfo(label19 = new Label(), new Uri("Settings\\CustomCodingEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 205, 38);
			BindingExtension bindingExtension20;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension20 = new BindingExtension(), new Uri("Settings\\CustomCodingEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 206, 44);
			Entry entry15;
			VisualDiagnostics.RegisterSourceInfo(entry15 = new Entry(), new Uri("Settings\\CustomCodingEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 206, 38);
			StackLayout stackLayout9;
			VisualDiagnostics.RegisterSourceInfo(stackLayout9 = new StackLayout(), new Uri("Settings\\CustomCodingEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 204, 34);
			Button button6;
			VisualDiagnostics.RegisterSourceInfo(button6 = new Button(), new Uri("Settings\\CustomCodingEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 208, 34);
			Label label20;
			VisualDiagnostics.RegisterSourceInfo(label20 = new Label(), new Uri("Settings\\CustomCodingEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 216, 38);
			BindingExtension bindingExtension21;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension21 = new BindingExtension(), new Uri("Settings\\CustomCodingEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 217, 44);
			Entry entry16;
			VisualDiagnostics.RegisterSourceInfo(entry16 = new Entry(), new Uri("Settings\\CustomCodingEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 217, 38);
			StackLayout stackLayout10;
			VisualDiagnostics.RegisterSourceInfo(stackLayout10 = new StackLayout(), new Uri("Settings\\CustomCodingEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 215, 34);
			Grid grid6;
			VisualDiagnostics.RegisterSourceInfo(grid6 = new Grid(), new Uri("Settings\\CustomCodingEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 198, 30);
			Label label21;
			VisualDiagnostics.RegisterSourceInfo(label21 = new Label(), new Uri("Settings\\CustomCodingEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 221, 34);
			BindingExtension bindingExtension22;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension22 = new BindingExtension(), new Uri("Settings\\CustomCodingEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 225, 37);
			Entry entry17;
			VisualDiagnostics.RegisterSourceInfo(entry17 = new Entry(), new Uri("Settings\\CustomCodingEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 222, 34);
			StackLayout stackLayout11;
			VisualDiagnostics.RegisterSourceInfo(stackLayout11 = new StackLayout(), new Uri("Settings\\CustomCodingEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 220, 30);
			Label label22;
			VisualDiagnostics.RegisterSourceInfo(label22 = new Label(), new Uri("Settings\\CustomCodingEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 231, 34);
			BindingExtension bindingExtension23;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension23 = new BindingExtension(), new Uri("Settings\\CustomCodingEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 235, 37);
			Entry entry18;
			VisualDiagnostics.RegisterSourceInfo(entry18 = new Entry(), new Uri("Settings\\CustomCodingEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 232, 34);
			StackLayout stackLayout12;
			VisualDiagnostics.RegisterSourceInfo(stackLayout12 = new StackLayout(), new Uri("Settings\\CustomCodingEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 230, 30);
			Label label23;
			VisualDiagnostics.RegisterSourceInfo(label23 = new Label(), new Uri("Settings\\CustomCodingEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 241, 30);
			BindingExtension bindingExtension24;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension24 = new BindingExtension(), new Uri("Settings\\CustomCodingEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 242, 36);
			Entry entry19;
			VisualDiagnostics.RegisterSourceInfo(entry19 = new Entry(), new Uri("Settings\\CustomCodingEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 242, 30);
			Label label24;
			VisualDiagnostics.RegisterSourceInfo(label24 = new Label(), new Uri("Settings\\CustomCodingEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 244, 30);
			BindingExtension bindingExtension25;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension25 = new BindingExtension(), new Uri("Settings\\CustomCodingEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 245, 36);
			Entry entry20;
			VisualDiagnostics.RegisterSourceInfo(entry20 = new Entry(), new Uri("Settings\\CustomCodingEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 245, 30);
			Label label25;
			VisualDiagnostics.RegisterSourceInfo(label25 = new Label(), new Uri("Settings\\CustomCodingEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 246, 30);
			BindingExtension bindingExtension26;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension26 = new BindingExtension(), new Uri("Settings\\CustomCodingEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 247, 36);
			Entry entry21;
			VisualDiagnostics.RegisterSourceInfo(entry21 = new Entry(), new Uri("Settings\\CustomCodingEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 247, 30);
			Label label26;
			VisualDiagnostics.RegisterSourceInfo(label26 = new Label(), new Uri("Settings\\CustomCodingEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 248, 30);
			BindingExtension bindingExtension27;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension27 = new BindingExtension(), new Uri("Settings\\CustomCodingEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 249, 36);
			Entry entry22;
			VisualDiagnostics.RegisterSourceInfo(entry22 = new Entry(), new Uri("Settings\\CustomCodingEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 249, 30);
			BindingExtension bindingExtension28;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension28 = new BindingExtension(), new Uri("Settings\\CustomCodingEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 250, 48);
			LabelSwitch labelSwitch2;
			VisualDiagnostics.RegisterSourceInfo(labelSwitch2 = new LabelSwitch(), new Uri("Settings\\CustomCodingEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 250, 30);
			StackLayout stackLayout13;
			VisualDiagnostics.RegisterSourceInfo(stackLayout13 = new StackLayout(), new Uri("Settings\\CustomCodingEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 158, 26);
			SfExpander sfExpander3;
			VisualDiagnostics.RegisterSourceInfo(sfExpander3 = new SfExpander(), new Uri("Settings\\CustomCodingEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 140, 18);
			DynamicResourceExtension dynamicResourceExtension11;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension11 = new DynamicResourceExtension(), new Uri("Settings\\CustomCodingEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 259, 21);
			DynamicResourceExtension dynamicResourceExtension12;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension12 = new DynamicResourceExtension(), new Uri("Settings\\CustomCodingEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 260, 21);
			DynamicResourceExtension dynamicResourceExtension13;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension13 = new DynamicResourceExtension(), new Uri("Settings\\CustomCodingEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 268, 33);
			Label label27;
			VisualDiagnostics.RegisterSourceInfo(label27 = new Label(), new Uri("Settings\\CustomCodingEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 264, 30);
			Grid grid7;
			VisualDiagnostics.RegisterSourceInfo(grid7 = new Grid(), new Uri("Settings\\CustomCodingEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 263, 26);
			StaticResourceExtension staticResourceExtension2;
			VisualDiagnostics.RegisterSourceInfo(staticResourceExtension2 = new StaticResourceExtension(), new Uri("Settings\\CustomCodingEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 276, 57);
			BindingExtension bindingExtension29;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension29 = new BindingExtension(), new Uri("Settings\\CustomCodingEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 276, 57);
			string text = "Options";
			string text2 = "Input";
			string text3 = "IEEE-754 Float";
			string text4 = "HEX";
			string text5 = "ASCII text";
			Picker picker2;
			VisualDiagnostics.RegisterSourceInfo(picker2 = new Picker(), new Uri("Settings\\CustomCodingEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 276, 30);
			StaticResourceExtension staticResourceExtension3;
			VisualDiagnostics.RegisterSourceInfo(staticResourceExtension3 = new StaticResourceExtension(), new Uri("Settings\\CustomCodingEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 304, 33);
			BindingExtension bindingExtension30;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension30 = new BindingExtension(), new Uri("Settings\\CustomCodingEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 304, 33);
			ColumnDefinition columnDefinition8;
			VisualDiagnostics.RegisterSourceInfo(columnDefinition8 = new ColumnDefinition(), new Uri("Settings\\CustomCodingEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 306, 38);
			ColumnDefinition columnDefinition9;
			VisualDiagnostics.RegisterSourceInfo(columnDefinition9 = new ColumnDefinition(), new Uri("Settings\\CustomCodingEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 307, 38);
			RowDefinition rowDefinition;
			VisualDiagnostics.RegisterSourceInfo(rowDefinition = new RowDefinition(), new Uri("Settings\\CustomCodingEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 310, 38);
			RowDefinition rowDefinition2;
			VisualDiagnostics.RegisterSourceInfo(rowDefinition2 = new RowDefinition(), new Uri("Settings\\CustomCodingEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 311, 38);
			Label label28;
			VisualDiagnostics.RegisterSourceInfo(label28 = new Label(), new Uri("Settings\\CustomCodingEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 318, 38);
			StaticResourceExtension staticResourceExtension4;
			VisualDiagnostics.RegisterSourceInfo(staticResourceExtension4 = new StaticResourceExtension(), new Uri("Settings\\CustomCodingEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 321, 41);
			BindingExtension bindingExtension31;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension31 = new BindingExtension(), new Uri("Settings\\CustomCodingEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 321, 41);
			Entry entry23;
			VisualDiagnostics.RegisterSourceInfo(entry23 = new Entry(), new Uri("Settings\\CustomCodingEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 319, 38);
			StackLayout stackLayout14;
			VisualDiagnostics.RegisterSourceInfo(stackLayout14 = new StackLayout(), new Uri("Settings\\CustomCodingEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 313, 34);
			BindingExtension bindingExtension32;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension32 = new BindingExtension(), new Uri("Settings\\CustomCodingEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 334, 41);
			DataTemplate dataTemplate2;
			VisualDiagnostics.RegisterSourceInfo(dataTemplate2 = new DataTemplate(), new Uri("Settings\\CustomCodingEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 336, 46);
			ListView listView2;
			VisualDiagnostics.RegisterSourceInfo(listView2 = new ListView(), new Uri("Settings\\CustomCodingEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 330, 38);
			ColumnDefinition columnDefinition10;
			VisualDiagnostics.RegisterSourceInfo(columnDefinition10 = new ColumnDefinition(), new Uri("Settings\\CustomCodingEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 343, 46);
			ColumnDefinition columnDefinition11;
			VisualDiagnostics.RegisterSourceInfo(columnDefinition11 = new ColumnDefinition(), new Uri("Settings\\CustomCodingEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 344, 46);
			Button button7;
			VisualDiagnostics.RegisterSourceInfo(button7 = new Button(), new Uri("Settings\\CustomCodingEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 346, 42);
			Button button8;
			VisualDiagnostics.RegisterSourceInfo(button8 = new Button(), new Uri("Settings\\CustomCodingEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 351, 42);
			Grid grid8;
			VisualDiagnostics.RegisterSourceInfo(grid8 = new Grid(), new Uri("Settings\\CustomCodingEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 341, 38);
			ColumnDefinition columnDefinition12;
			VisualDiagnostics.RegisterSourceInfo(columnDefinition12 = new ColumnDefinition(), new Uri("Settings\\CustomCodingEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 359, 46);
			ColumnDefinition columnDefinition13;
			VisualDiagnostics.RegisterSourceInfo(columnDefinition13 = new ColumnDefinition(), new Uri("Settings\\CustomCodingEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 360, 46);
			Button button9;
			VisualDiagnostics.RegisterSourceInfo(button9 = new Button(), new Uri("Settings\\CustomCodingEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 362, 42);
			Button button10;
			VisualDiagnostics.RegisterSourceInfo(button10 = new Button(), new Uri("Settings\\CustomCodingEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 367, 42);
			Grid grid9;
			VisualDiagnostics.RegisterSourceInfo(grid9 = new Grid(), new Uri("Settings\\CustomCodingEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 357, 38);
			StackLayout stackLayout15;
			VisualDiagnostics.RegisterSourceInfo(stackLayout15 = new StackLayout(), new Uri("Settings\\CustomCodingEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 326, 34);
			ReferenceExtension referenceExtension2;
			VisualDiagnostics.RegisterSourceInfo(referenceExtension2 = new ReferenceExtension(), new Uri("Settings\\CustomCodingEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 378, 37);
			BindingExtension bindingExtension33;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension33 = new BindingExtension(), new Uri("Settings\\CustomCodingEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 378, 37);
			Label label29;
			VisualDiagnostics.RegisterSourceInfo(label29 = new Label(), new Uri("Settings\\CustomCodingEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 380, 38);
			BindingExtension bindingExtension34;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension34 = new BindingExtension(), new Uri("Settings\\CustomCodingEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 381, 44);
			Entry entry24;
			VisualDiagnostics.RegisterSourceInfo(entry24 = new Entry(), new Uri("Settings\\CustomCodingEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 381, 38);
			Label label30;
			VisualDiagnostics.RegisterSourceInfo(label30 = new Label(), new Uri("Settings\\CustomCodingEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 382, 38);
			BindingExtension bindingExtension35;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension35 = new BindingExtension(), new Uri("Settings\\CustomCodingEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 383, 44);
			Entry entry25;
			VisualDiagnostics.RegisterSourceInfo(entry25 = new Entry(), new Uri("Settings\\CustomCodingEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 383, 38);
			Label label31;
			VisualDiagnostics.RegisterSourceInfo(label31 = new Label(), new Uri("Settings\\CustomCodingEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 384, 38);
			BindingExtension bindingExtension36;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension36 = new BindingExtension(), new Uri("Settings\\CustomCodingEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 385, 44);
			Entry entry26;
			VisualDiagnostics.RegisterSourceInfo(entry26 = new Entry(), new Uri("Settings\\CustomCodingEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 385, 38);
			Label label32;
			VisualDiagnostics.RegisterSourceInfo(label32 = new Label(), new Uri("Settings\\CustomCodingEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 387, 38);
			ColumnDefinition columnDefinition14;
			VisualDiagnostics.RegisterSourceInfo(columnDefinition14 = new ColumnDefinition(), new Uri("Settings\\CustomCodingEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 398, 46);
			ColumnDefinition columnDefinition15;
			VisualDiagnostics.RegisterSourceInfo(columnDefinition15 = new ColumnDefinition(), new Uri("Settings\\CustomCodingEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 399, 46);
			BindingExtension bindingExtension37;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension37 = new BindingExtension(), new Uri("Settings\\CustomCodingEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 407, 49);
			DataTemplate dataTemplate3;
			VisualDiagnostics.RegisterSourceInfo(dataTemplate3 = new DataTemplate(), new Uri("Settings\\CustomCodingEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 409, 54);
			ListView listView3;
			VisualDiagnostics.RegisterSourceInfo(listView3 = new ListView(), new Uri("Settings\\CustomCodingEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 403, 46);
			ColumnDefinition columnDefinition16;
			VisualDiagnostics.RegisterSourceInfo(columnDefinition16 = new ColumnDefinition(), new Uri("Settings\\CustomCodingEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 416, 54);
			ColumnDefinition columnDefinition17;
			VisualDiagnostics.RegisterSourceInfo(columnDefinition17 = new ColumnDefinition(), new Uri("Settings\\CustomCodingEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 417, 54);
			Button button11;
			VisualDiagnostics.RegisterSourceInfo(button11 = new Button(), new Uri("Settings\\CustomCodingEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 419, 50);
			Button button12;
			VisualDiagnostics.RegisterSourceInfo(button12 = new Button(), new Uri("Settings\\CustomCodingEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 424, 50);
			Grid grid10;
			VisualDiagnostics.RegisterSourceInfo(grid10 = new Grid(), new Uri("Settings\\CustomCodingEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 414, 46);
			StackLayout stackLayout16;
			VisualDiagnostics.RegisterSourceInfo(stackLayout16 = new StackLayout(), new Uri("Settings\\CustomCodingEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 401, 42);
			ReferenceExtension referenceExtension3;
			VisualDiagnostics.RegisterSourceInfo(referenceExtension3 = new ReferenceExtension(), new Uri("Settings\\CustomCodingEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 434, 45);
			BindingExtension bindingExtension38;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension38 = new BindingExtension(), new Uri("Settings\\CustomCodingEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 434, 45);
			Label label33;
			VisualDiagnostics.RegisterSourceInfo(label33 = new Label(), new Uri("Settings\\CustomCodingEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 436, 46);
			BindingExtension bindingExtension39;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension39 = new BindingExtension(), new Uri("Settings\\CustomCodingEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 437, 52);
			Entry entry27;
			VisualDiagnostics.RegisterSourceInfo(entry27 = new Entry(), new Uri("Settings\\CustomCodingEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 437, 46);
			Label label34;
			VisualDiagnostics.RegisterSourceInfo(label34 = new Label(), new Uri("Settings\\CustomCodingEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 438, 46);
			BindingExtension bindingExtension40;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension40 = new BindingExtension(), new Uri("Settings\\CustomCodingEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 439, 52);
			Entry entry28;
			VisualDiagnostics.RegisterSourceInfo(entry28 = new Entry(), new Uri("Settings\\CustomCodingEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 439, 46);
			StackLayout stackLayout17;
			VisualDiagnostics.RegisterSourceInfo(stackLayout17 = new StackLayout(), new Uri("Settings\\CustomCodingEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 432, 42);
			Grid grid11;
			VisualDiagnostics.RegisterSourceInfo(grid11 = new Grid(), new Uri("Settings\\CustomCodingEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 396, 38);
			StackLayout stackLayout18;
			VisualDiagnostics.RegisterSourceInfo(stackLayout18 = new StackLayout(), new Uri("Settings\\CustomCodingEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 375, 34);
			Grid grid12;
			VisualDiagnostics.RegisterSourceInfo(grid12 = new Grid(), new Uri("Settings\\CustomCodingEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 301, 30);
			StaticResourceExtension staticResourceExtension5;
			VisualDiagnostics.RegisterSourceInfo(staticResourceExtension5 = new StaticResourceExtension(), new Uri("Settings\\CustomCodingEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 454, 33);
			BindingExtension bindingExtension41;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension41 = new BindingExtension(), new Uri("Settings\\CustomCodingEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 454, 33);
			Label label35;
			VisualDiagnostics.RegisterSourceInfo(label35 = new Label(), new Uri("Settings\\CustomCodingEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 457, 34);
			StaticResourceExtension staticResourceExtension6;
			VisualDiagnostics.RegisterSourceInfo(staticResourceExtension6 = new StaticResourceExtension(), new Uri("Settings\\CustomCodingEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 458, 58);
			BindingExtension bindingExtension42;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension42 = new BindingExtension(), new Uri("Settings\\CustomCodingEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 458, 58);
			Entry entry29;
			VisualDiagnostics.RegisterSourceInfo(entry29 = new Entry(), new Uri("Settings\\CustomCodingEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 458, 34);
			Label label36;
			VisualDiagnostics.RegisterSourceInfo(label36 = new Label(), new Uri("Settings\\CustomCodingEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 459, 34);
			StaticResourceExtension staticResourceExtension7;
			VisualDiagnostics.RegisterSourceInfo(staticResourceExtension7 = new StaticResourceExtension(), new Uri("Settings\\CustomCodingEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 460, 59);
			BindingExtension bindingExtension43;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension43 = new BindingExtension(), new Uri("Settings\\CustomCodingEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 460, 59);
			Entry entry30;
			VisualDiagnostics.RegisterSourceInfo(entry30 = new Entry(), new Uri("Settings\\CustomCodingEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 460, 34);
			Label label37;
			VisualDiagnostics.RegisterSourceInfo(label37 = new Label(), new Uri("Settings\\CustomCodingEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 464, 34);
			StaticResourceExtension staticResourceExtension8;
			VisualDiagnostics.RegisterSourceInfo(staticResourceExtension8 = new StaticResourceExtension(), new Uri("Settings\\CustomCodingEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 465, 57);
			BindingExtension bindingExtension44;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension44 = new BindingExtension(), new Uri("Settings\\CustomCodingEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 465, 57);
			Entry entry31;
			VisualDiagnostics.RegisterSourceInfo(entry31 = new Entry(), new Uri("Settings\\CustomCodingEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 465, 34);
			Label label38;
			VisualDiagnostics.RegisterSourceInfo(label38 = new Label(), new Uri("Settings\\CustomCodingEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 466, 34);
			StaticResourceExtension staticResourceExtension9;
			VisualDiagnostics.RegisterSourceInfo(staticResourceExtension9 = new StaticResourceExtension(), new Uri("Settings\\CustomCodingEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 467, 58);
			BindingExtension bindingExtension45;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension45 = new BindingExtension(), new Uri("Settings\\CustomCodingEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 467, 58);
			Entry entry32;
			VisualDiagnostics.RegisterSourceInfo(entry32 = new Entry(), new Uri("Settings\\CustomCodingEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 467, 34);
			BindingExtension bindingExtension46;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension46 = new BindingExtension(), new Uri("Settings\\CustomCodingEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 471, 37);
			LabelSwitch labelSwitch3;
			VisualDiagnostics.RegisterSourceInfo(labelSwitch3 = new LabelSwitch(), new Uri("Settings\\CustomCodingEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 469, 34);
			BindingExtension bindingExtension47;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension47 = new BindingExtension(), new Uri("Settings\\CustomCodingEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 475, 37);
			LabelSwitch labelSwitch4;
			VisualDiagnostics.RegisterSourceInfo(labelSwitch4 = new LabelSwitch(), new Uri("Settings\\CustomCodingEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 473, 34);
			StackLayout stackLayout19;
			VisualDiagnostics.RegisterSourceInfo(stackLayout19 = new StackLayout(), new Uri("Settings\\CustomCodingEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 450, 30);
			StaticResourceExtension staticResourceExtension10;
			VisualDiagnostics.RegisterSourceInfo(staticResourceExtension10 = new StaticResourceExtension(), new Uri("Settings\\CustomCodingEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 485, 33);
			BindingExtension bindingExtension48;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension48 = new BindingExtension(), new Uri("Settings\\CustomCodingEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 485, 33);
			Label label39;
			VisualDiagnostics.RegisterSourceInfo(label39 = new Label(), new Uri("Settings\\CustomCodingEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 488, 34);
			StaticResourceExtension staticResourceExtension11;
			VisualDiagnostics.RegisterSourceInfo(staticResourceExtension11 = new StaticResourceExtension(), new Uri("Settings\\CustomCodingEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 489, 63);
			BindingExtension bindingExtension49;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension49 = new BindingExtension(), new Uri("Settings\\CustomCodingEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 489, 63);
			Entry entry33;
			VisualDiagnostics.RegisterSourceInfo(entry33 = new Entry(), new Uri("Settings\\CustomCodingEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 489, 34);
			StackLayout stackLayout20;
			VisualDiagnostics.RegisterSourceInfo(stackLayout20 = new StackLayout(), new Uri("Settings\\CustomCodingEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 481, 30);
			StackLayout stackLayout21;
			VisualDiagnostics.RegisterSourceInfo(stackLayout21 = new StackLayout(), new Uri("Settings\\CustomCodingEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 274, 26);
			SfExpander sfExpander4;
			VisualDiagnostics.RegisterSourceInfo(sfExpander4 = new SfExpander(), new Uri("Settings\\CustomCodingEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 256, 18);
			StackLayout stackLayout22;
			VisualDiagnostics.RegisterSourceInfo(stackLayout22 = new StackLayout(), new Uri("Settings\\CustomCodingEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 29, 14);
			ScrollView scrollView;
			VisualDiagnostics.RegisterSourceInfo(scrollView = new ScrollView(), new Uri("Settings\\CustomCodingEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 28, 10);
			VisualDiagnostics.RegisterSourceInfo(this, new Uri("Settings\\CustomCodingEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 2, 2);
			NameScope nameScope = NameScope.GetNameScope(this) ?? new NameScope();
			NameScope.SetNameScope(this, nameScope);
			nameScope.RegisterName("pickerGroup", picker);
			if (picker.StyleId == null)
			{
				picker.StyleId = "pickerGroup";
			}
			nameScope.RegisterName("gridTranslations", grid4);
			if (grid4.StyleId == null)
			{
				grid4.StyleId = "gridTranslations";
			}
			nameScope.RegisterName("lvTranslations", listView);
			if (listView.StyleId == null)
			{
				listView.StyleId = "lvTranslations";
			}
			nameScope.RegisterName("btnAddTranslation", button);
			if (button.StyleId == null)
			{
				button.StyleId = "btnAddTranslation";
			}
			nameScope.RegisterName("btnDelTranslation", button2);
			if (button2.StyleId == null)
			{
				button2.StyleId = "btnDelTranslation";
			}
			nameScope.RegisterName("btnGenRequestHeaderGeneric", button3);
			if (button3.StyleId == null)
			{
				button3.StyleId = "btnGenRequestHeaderGeneric";
			}
			nameScope.RegisterName("btnGenRequestHeaderVag", button4);
			if (button4.StyleId == null)
			{
				button4.StyleId = "btnGenRequestHeaderVag";
			}
			nameScope.RegisterName("btnGenRequestHeaderRenault", button5);
			if (button5.StyleId == null)
			{
				button5.StyleId = "btnGenRequestHeaderRenault";
			}
			nameScope.RegisterName("btnReadAddressToWriteAddress", button6);
			if (button6.StyleId == null)
			{
				button6.StyleId = "btnReadAddressToWriteAddress";
			}
			nameScope.RegisterName("tbATST", entry17);
			if (entry17.StyleId == null)
			{
				entry17.StyleId = "tbATST";
			}
			nameScope.RegisterName("tbProtocol", entry18);
			if (entry18.StyleId == null)
			{
				entry18.StyleId = "tbProtocol";
			}
			nameScope.RegisterName("pickerType", picker2);
			if (picker2.StyleId == null)
			{
				picker2.StyleId = "pickerType";
			}
			nameScope.RegisterName("gridOptions", grid12);
			if (grid12.StyleId == null)
			{
				grid12.StyleId = "gridOptions";
			}
			nameScope.RegisterName("tbByteId2", entry23);
			if (entry23.StyleId == null)
			{
				entry23.StyleId = "tbByteId2";
			}
			nameScope.RegisterName("lvOptions", listView2);
			if (listView2.StyleId == null)
			{
				listView2.StyleId = "lvOptions";
			}
			nameScope.RegisterName("btnAddOption", button7);
			if (button7.StyleId == null)
			{
				button7.StyleId = "btnAddOption";
			}
			nameScope.RegisterName("btnDelOption", button8);
			if (button8.StyleId == null)
			{
				button8.StyleId = "btnDelOption";
			}
			nameScope.RegisterName("btnUpOption", button9);
			if (button9.StyleId == null)
			{
				button9.StyleId = "btnUpOption";
			}
			nameScope.RegisterName("btnDownOption", button10);
			if (button10.StyleId == null)
			{
				button10.StyleId = "btnDownOption";
			}
			nameScope.RegisterName("gridOptionTranslations", grid11);
			if (grid11.StyleId == null)
			{
				grid11.StyleId = "gridOptionTranslations";
			}
			nameScope.RegisterName("lvOptionTranslations", listView3);
			if (listView3.StyleId == null)
			{
				listView3.StyleId = "lvOptionTranslations";
			}
			nameScope.RegisterName("btnAddOptionTranslation", button11);
			if (button11.StyleId == null)
			{
				button11.StyleId = "btnAddOptionTranslation";
			}
			nameScope.RegisterName("btnDelOptionTranslation", button12);
			if (button12.StyleId == null)
			{
				button12.StyleId = "btnDelOptionTranslation";
			}
			nameScope.RegisterName("InputValueTypePanel", stackLayout19);
			if (stackLayout19.StyleId == null)
			{
				stackLayout19.StyleId = "InputValueTypePanel";
			}
			nameScope.RegisterName("tbByteId", entry29);
			if (entry29.StyleId == null)
			{
				entry29.StyleId = "tbByteId";
			}
			nameScope.RegisterName("btDataLen", entry30);
			if (entry30.StyleId == null)
			{
				entry30.StyleId = "btDataLen";
			}
			nameScope.RegisterName("tbMulti", entry31);
			if (entry31.StyleId == null)
			{
				entry31.StyleId = "tbMulti";
			}
			nameScope.RegisterName("tbOffset", entry32);
			if (entry32.StyleId == null)
			{
				entry32.StyleId = "tbOffset";
			}
			nameScope.RegisterName("cbSigned", labelSwitch3);
			if (labelSwitch3.StyleId == null)
			{
				labelSwitch3.StyleId = "cbSigned";
			}
			nameScope.RegisterName("cbReversed", labelSwitch4);
			if (labelSwitch4.StyleId == null)
			{
				labelSwitch4.StyleId = "cbReversed";
			}
			nameScope.RegisterName("InputFloatPanel", stackLayout20);
			if (stackLayout20.StyleId == null)
			{
				stackLayout20.StyleId = "InputFloatPanel";
			}
			nameScope.RegisterName("tbByteIdFloat", entry33);
			if (entry33.StyleId == null)
			{
				entry33.StyleId = "tbByteIdFloat";
			}
			this.pickerGroup = picker;
			this.gridTranslations = grid4;
			this.lvTranslations = listView;
			this.btnAddTranslation = button;
			this.btnDelTranslation = button2;
			this.btnGenRequestHeaderGeneric = button3;
			this.btnGenRequestHeaderVag = button4;
			this.btnGenRequestHeaderRenault = button5;
			this.btnReadAddressToWriteAddress = button6;
			this.tbATST = entry17;
			this.tbProtocol = entry18;
			this.pickerType = picker2;
			this.gridOptions = grid12;
			this.tbByteId2 = entry23;
			this.lvOptions = listView2;
			this.btnAddOption = button7;
			this.btnDelOption = button8;
			this.btnUpOption = button9;
			this.btnDownOption = button10;
			this.gridOptionTranslations = grid11;
			this.lvOptionTranslations = listView3;
			this.btnAddOptionTranslation = button11;
			this.btnDelOptionTranslation = button12;
			this.InputValueTypePanel = stackLayout19;
			this.tbByteId = entry29;
			this.btDataLen = entry30;
			this.tbMulti = entry31;
			this.tbOffset = entry32;
			this.cbSigned = labelSwitch3;
			this.cbReversed = labelSwitch4;
			this.InputFloatPanel = stackLayout20;
			this.tbByteIdFloat = entry33;
			this.Resources = resourceDictionary;
			resourceDictionary.Add("UnitsToIntConverter", unitsToIntConverter);
			resourceDictionary.Add("BoolToNegativeConverter", boolToNegativeConverter);
			resourceDictionary.Add("DoubleToStringConverter", doubleToStringConverter);
			resourceDictionary.Add("CustomPIDTypeFormulaToTrue", customPIDTypeFormulaToTrue);
			resourceDictionary.Add("EnumToIntConverter", enumToIntConverter);
			resourceDictionary.Add("EnumValueToTrueConverter", enumValueToTrueConverter);
			resourceDictionary.Add("EnumValueToFalseConverter", enumValueToFalseConverter);
			resourceDictionary.Add("IntToStringConverter", intToStringConverter);
			resourceDictionary.Add("CodingInputValueTypeToIntConverterForCustomCodingEditor", codingInputValueTypeToIntConverterForCustomCodingEditor);
			this.SetValue(Page.TitleProperty, "Custom coding editor");
			this.SetValue(Page.UseSafeAreaProperty, true);
			dynamicResourceExtension.Key = "BackgroundColor";
			IMarkupExtension<DynamicResource> markupExtension = dynamicResourceExtension;
			XamlServiceProvider xamlServiceProvider = new XamlServiceProvider();
			Type typeFromHandle = typeof(IProvideValueTarget);
			object[] array = new object[0 + 1];
			array[0] = this;
			object obj;
			xamlServiceProvider.Add(typeFromHandle, obj = new SimpleValueTargetProvider(array, VisualElement.BackgroundColorProperty, nameScope));
			xamlServiceProvider.Add(typeof(IReferenceProvider), obj);
			Type typeFromHandle2 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver = new XmlNamespaceResolver();
			xmlNamespaceResolver.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver.Add("expander", "clr-namespace:Syncfusion.XForms.Expander;assembly=Syncfusion.Expander.XForms");
			xmlNamespaceResolver.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver.Add("settings", "clr-namespace:CarScannerXamarinForms.Settings");
			xamlServiceProvider.Add(typeFromHandle2, new XamlTypeResolver(xmlNamespaceResolver, typeof(CustomCodingEditor).GetTypeInfo().Assembly));
			xamlServiceProvider.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(13, 5)));
			DynamicResource dynamicResource = markupExtension.ProvideValue(xamlServiceProvider);
			this.SetDynamicResource(VisualElement.BackgroundColorProperty, dynamicResource.Key);
			this.Resources = resourceDictionary;
			scrollView.SetValue(ScrollView.OrientationProperty, 0);
			stackLayout22.SetValue(StackLayout.OrientationProperty, 0);
			sfExpander.SetValue(SfExpander.AnimationDurationProperty, 250.0);
			sfExpander.SetValue(SfExpander.AnimationEasingProperty, 3);
			dynamicResourceExtension2.Key = "NavigationBarBackgroundColor";
			IMarkupExtension<DynamicResource> markupExtension2 = dynamicResourceExtension2;
			XamlServiceProvider xamlServiceProvider2 = new XamlServiceProvider();
			Type typeFromHandle3 = typeof(IProvideValueTarget);
			object[] array2 = new object[0 + 4];
			array2[0] = sfExpander;
			array2[1] = stackLayout22;
			array2[2] = scrollView;
			array2[3] = this;
			object obj2;
			xamlServiceProvider2.Add(typeFromHandle3, obj2 = new SimpleValueTargetProvider(array2, SfExpander.HeaderBackgroundColorProperty, nameScope));
			xamlServiceProvider2.Add(typeof(IReferenceProvider), obj2);
			Type typeFromHandle4 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver2 = new XmlNamespaceResolver();
			xmlNamespaceResolver2.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver2.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver2.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver2.Add("expander", "clr-namespace:Syncfusion.XForms.Expander;assembly=Syncfusion.Expander.XForms");
			xmlNamespaceResolver2.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver2.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver2.Add("settings", "clr-namespace:CarScannerXamarinForms.Settings");
			xamlServiceProvider2.Add(typeFromHandle4, new XamlTypeResolver(xmlNamespaceResolver2, typeof(CustomCodingEditor).GetTypeInfo().Assembly));
			xamlServiceProvider2.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(33, 21)));
			DynamicResource dynamicResource2 = markupExtension2.ProvideValue(xamlServiceProvider2);
			sfExpander.SetDynamicResource(SfExpander.HeaderBackgroundColorProperty, dynamicResource2.Key);
			dynamicResourceExtension3.Key = "NavigationBarButtonColor";
			IMarkupExtension<DynamicResource> markupExtension3 = dynamicResourceExtension3;
			XamlServiceProvider xamlServiceProvider3 = new XamlServiceProvider();
			Type typeFromHandle5 = typeof(IProvideValueTarget);
			object[] array3 = new object[0 + 4];
			array3[0] = sfExpander;
			array3[1] = stackLayout22;
			array3[2] = scrollView;
			array3[3] = this;
			object obj3;
			xamlServiceProvider3.Add(typeFromHandle5, obj3 = new SimpleValueTargetProvider(array3, SfExpander.IconColorProperty, nameScope));
			xamlServiceProvider3.Add(typeof(IReferenceProvider), obj3);
			Type typeFromHandle6 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver3 = new XmlNamespaceResolver();
			xmlNamespaceResolver3.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver3.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver3.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver3.Add("expander", "clr-namespace:Syncfusion.XForms.Expander;assembly=Syncfusion.Expander.XForms");
			xmlNamespaceResolver3.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver3.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver3.Add("settings", "clr-namespace:CarScannerXamarinForms.Settings");
			xamlServiceProvider3.Add(typeFromHandle6, new XamlTypeResolver(xmlNamespaceResolver3, typeof(CustomCodingEditor).GetTypeInfo().Assembly));
			xamlServiceProvider3.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(34, 21)));
			DynamicResource dynamicResource3 = markupExtension3.ProvideValue(xamlServiceProvider3);
			sfExpander.SetDynamicResource(SfExpander.IconColorProperty, dynamicResource3.Key);
			sfExpander.SetValue(SfExpander.IsExpandedProperty, true);
			label.SetValue(View.MarginProperty, new Thickness(5.0));
			label.SetValue(Label.FontAttributesProperty, new FontAttributesConverter().ConvertFromInvariantString("Bold"));
			label.SetValue(Label.TextProperty, "1. Description:");
			dynamicResourceExtension4.Key = "NavigationBarTextColor";
			IMarkupExtension<DynamicResource> markupExtension4 = dynamicResourceExtension4;
			XamlServiceProvider xamlServiceProvider4 = new XamlServiceProvider();
			Type typeFromHandle7 = typeof(IProvideValueTarget);
			object[] array4 = new object[0 + 6];
			array4[0] = label;
			array4[1] = grid;
			array4[2] = sfExpander;
			array4[3] = stackLayout22;
			array4[4] = scrollView;
			array4[5] = this;
			object obj4;
			xamlServiceProvider4.Add(typeFromHandle7, obj4 = new SimpleValueTargetProvider(array4, Label.TextColorProperty, nameScope));
			xamlServiceProvider4.Add(typeof(IReferenceProvider), obj4);
			Type typeFromHandle8 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver4 = new XmlNamespaceResolver();
			xmlNamespaceResolver4.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver4.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver4.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver4.Add("expander", "clr-namespace:Syncfusion.XForms.Expander;assembly=Syncfusion.Expander.XForms");
			xmlNamespaceResolver4.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver4.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver4.Add("settings", "clr-namespace:CarScannerXamarinForms.Settings");
			xamlServiceProvider4.Add(typeFromHandle8, new XamlTypeResolver(xmlNamespaceResolver4, typeof(CustomCodingEditor).GetTypeInfo().Assembly));
			xamlServiceProvider4.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(42, 33)));
			DynamicResource dynamicResource4 = markupExtension4.ProvideValue(xamlServiceProvider4);
			label.SetDynamicResource(Label.TextColorProperty, dynamicResource4.Key);
			label.SetValue(View.VerticalOptionsProperty, LayoutOptions.Center);
			label.SetValue(Label.VerticalTextAlignmentProperty, new TextAlignmentConverter().ConvertFromInvariantString("Center"));
			grid.Children.Add(label);
			sfExpander.SetValue(SfExpander.HeaderProperty, grid);
			stackLayout.SetValue(StackLayout.OrientationProperty, 0);
			label2.SetValue(Label.TextProperty, "Group:");
			stackLayout.Children.Add(label2);
			bindingExtension.Mode = 1;
			staticResourceExtension.Key = "EnumToIntConverter";
			IMarkupExtension markupExtension5 = staticResourceExtension;
			XamlServiceProvider xamlServiceProvider5 = new XamlServiceProvider();
			Type typeFromHandle9 = typeof(IProvideValueTarget);
			object[] array5 = new object[0 + 7];
			array5[0] = bindingExtension;
			array5[1] = picker;
			array5[2] = stackLayout;
			array5[3] = sfExpander;
			array5[4] = stackLayout22;
			array5[5] = scrollView;
			array5[6] = this;
			object obj5;
			xamlServiceProvider5.Add(typeFromHandle9, obj5 = new SimpleValueTargetProvider(array5, typeof(BindingExtension).GetRuntimeProperty("Converter"), nameScope));
			xamlServiceProvider5.Add(typeof(IReferenceProvider), obj5);
			Type typeFromHandle10 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver5 = new XmlNamespaceResolver();
			xmlNamespaceResolver5.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver5.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver5.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver5.Add("expander", "clr-namespace:Syncfusion.XForms.Expander;assembly=Syncfusion.Expander.XForms");
			xmlNamespaceResolver5.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver5.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver5.Add("settings", "clr-namespace:CarScannerXamarinForms.Settings");
			xamlServiceProvider5.Add(typeFromHandle10, new XamlTypeResolver(xmlNamespaceResolver5, typeof(CustomCodingEditor).GetTypeInfo().Assembly));
			xamlServiceProvider5.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(51, 58)));
			object obj6 = markupExtension5.ProvideValue(xamlServiceProvider5);
			bindingExtension.Converter = obj6;
			bindingExtension.Path = "Group";
			BindingBase bindingBase = bindingExtension.ProvideValue(null);
			picker.SetBinding(Picker.SelectedIndexProperty, bindingBase);
			stackLayout.Children.Add(picker);
			label3.SetValue(Label.TextProperty, "Name:");
			stackLayout.Children.Add(label3);
			bindingExtension2.Mode = 1;
			bindingExtension2.Path = "NameRaw";
			BindingBase bindingBase2 = bindingExtension2.ProvideValue(null);
			entry.SetBinding(Entry.TextProperty, bindingBase2);
			stackLayout.Children.Add(entry);
			label4.SetValue(Label.TextProperty, "Description:");
			stackLayout.Children.Add(label4);
			bindingExtension3.Mode = 1;
			bindingExtension3.Path = "DescriptionRaw";
			BindingBase bindingBase3 = bindingExtension3.ProvideValue(null);
			entry2.SetBinding(Entry.TextProperty, bindingBase3);
			stackLayout.Children.Add(entry2);
			label5.SetValue(Label.TextProperty, "Inner description:");
			stackLayout.Children.Add(label5);
			bindingExtension4.Mode = 1;
			bindingExtension4.Path = "InnerDescriptionRaw";
			BindingBase bindingBase4 = bindingExtension4.ProvideValue(null);
			entry3.SetBinding(Entry.TextProperty, bindingBase4);
			stackLayout.Children.Add(entry3);
			label6.SetValue(Label.TextProperty, "Comment:");
			stackLayout.Children.Add(label6);
			bindingExtension5.Mode = 1;
			bindingExtension5.Path = "Comment";
			BindingBase bindingBase5 = bindingExtension5.ProvideValue(null);
			entry4.SetBinding(Entry.TextProperty, bindingBase5);
			stackLayout.Children.Add(entry4);
			sfExpander.SetValue(SfExpander.ContentProperty, stackLayout);
			stackLayout22.Children.Add(sfExpander);
			sfExpander2.SetValue(SfExpander.AnimationDurationProperty, 250.0);
			sfExpander2.SetValue(SfExpander.AnimationEasingProperty, 3);
			dynamicResourceExtension5.Key = "NavigationBarBackgroundColor";
			IMarkupExtension<DynamicResource> markupExtension6 = dynamicResourceExtension5;
			XamlServiceProvider xamlServiceProvider6 = new XamlServiceProvider();
			Type typeFromHandle11 = typeof(IProvideValueTarget);
			object[] array6 = new object[0 + 4];
			array6[0] = sfExpander2;
			array6[1] = stackLayout22;
			array6[2] = scrollView;
			array6[3] = this;
			object obj7;
			xamlServiceProvider6.Add(typeFromHandle11, obj7 = new SimpleValueTargetProvider(array6, SfExpander.HeaderBackgroundColorProperty, nameScope));
			xamlServiceProvider6.Add(typeof(IReferenceProvider), obj7);
			Type typeFromHandle12 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver6 = new XmlNamespaceResolver();
			xmlNamespaceResolver6.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver6.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver6.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver6.Add("expander", "clr-namespace:Syncfusion.XForms.Expander;assembly=Syncfusion.Expander.XForms");
			xmlNamespaceResolver6.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver6.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver6.Add("settings", "clr-namespace:CarScannerXamarinForms.Settings");
			xamlServiceProvider6.Add(typeFromHandle12, new XamlTypeResolver(xmlNamespaceResolver6, typeof(CustomCodingEditor).GetTypeInfo().Assembly));
			xamlServiceProvider6.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(66, 21)));
			DynamicResource dynamicResource5 = markupExtension6.ProvideValue(xamlServiceProvider6);
			sfExpander2.SetDynamicResource(SfExpander.HeaderBackgroundColorProperty, dynamicResource5.Key);
			dynamicResourceExtension6.Key = "NavigationBarButtonColor";
			IMarkupExtension<DynamicResource> markupExtension7 = dynamicResourceExtension6;
			XamlServiceProvider xamlServiceProvider7 = new XamlServiceProvider();
			Type typeFromHandle13 = typeof(IProvideValueTarget);
			object[] array7 = new object[0 + 4];
			array7[0] = sfExpander2;
			array7[1] = stackLayout22;
			array7[2] = scrollView;
			array7[3] = this;
			object obj8;
			xamlServiceProvider7.Add(typeFromHandle13, obj8 = new SimpleValueTargetProvider(array7, SfExpander.IconColorProperty, nameScope));
			xamlServiceProvider7.Add(typeof(IReferenceProvider), obj8);
			Type typeFromHandle14 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver7 = new XmlNamespaceResolver();
			xmlNamespaceResolver7.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver7.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver7.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver7.Add("expander", "clr-namespace:Syncfusion.XForms.Expander;assembly=Syncfusion.Expander.XForms");
			xmlNamespaceResolver7.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver7.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver7.Add("settings", "clr-namespace:CarScannerXamarinForms.Settings");
			xamlServiceProvider7.Add(typeFromHandle14, new XamlTypeResolver(xmlNamespaceResolver7, typeof(CustomCodingEditor).GetTypeInfo().Assembly));
			xamlServiceProvider7.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(67, 21)));
			DynamicResource dynamicResource6 = markupExtension7.ProvideValue(xamlServiceProvider7);
			sfExpander2.SetDynamicResource(SfExpander.IconColorProperty, dynamicResource6.Key);
			sfExpander2.SetValue(SfExpander.IsExpandedProperty, false);
			label7.SetValue(View.MarginProperty, new Thickness(5.0));
			label7.SetValue(Label.FontAttributesProperty, new FontAttributesConverter().ConvertFromInvariantString("Bold"));
			label7.SetValue(Label.TextProperty, "1.1. Translations:");
			dynamicResourceExtension7.Key = "NavigationBarTextColor";
			IMarkupExtension<DynamicResource> markupExtension8 = dynamicResourceExtension7;
			XamlServiceProvider xamlServiceProvider8 = new XamlServiceProvider();
			Type typeFromHandle15 = typeof(IProvideValueTarget);
			object[] array8 = new object[0 + 6];
			array8[0] = label7;
			array8[1] = grid2;
			array8[2] = sfExpander2;
			array8[3] = stackLayout22;
			array8[4] = scrollView;
			array8[5] = this;
			object obj9;
			xamlServiceProvider8.Add(typeFromHandle15, obj9 = new SimpleValueTargetProvider(array8, Label.TextColorProperty, nameScope));
			xamlServiceProvider8.Add(typeof(IReferenceProvider), obj9);
			Type typeFromHandle16 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver8 = new XmlNamespaceResolver();
			xmlNamespaceResolver8.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver8.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver8.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver8.Add("expander", "clr-namespace:Syncfusion.XForms.Expander;assembly=Syncfusion.Expander.XForms");
			xmlNamespaceResolver8.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver8.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver8.Add("settings", "clr-namespace:CarScannerXamarinForms.Settings");
			xamlServiceProvider8.Add(typeFromHandle16, new XamlTypeResolver(xmlNamespaceResolver8, typeof(CustomCodingEditor).GetTypeInfo().Assembly));
			xamlServiceProvider8.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(75, 33)));
			DynamicResource dynamicResource7 = markupExtension8.ProvideValue(xamlServiceProvider8);
			label7.SetDynamicResource(Label.TextColorProperty, dynamicResource7.Key);
			label7.SetValue(View.VerticalOptionsProperty, LayoutOptions.Center);
			label7.SetValue(Label.VerticalTextAlignmentProperty, new TextAlignmentConverter().ConvertFromInvariantString("Center"));
			grid2.Children.Add(label7);
			sfExpander2.SetValue(SfExpander.HeaderProperty, grid2);
			grid4.SetValue(View.MarginProperty, new Thickness(2.0));
			columnDefinition.SetValue(ColumnDefinition.WidthProperty, new GridLengthTypeConverter().ConvertFromInvariantString("0.4*"));
			grid4.GetValue(Grid.ColumnDefinitionsProperty).Add(columnDefinition);
			columnDefinition2.SetValue(ColumnDefinition.WidthProperty, new GridLengthTypeConverter().ConvertFromInvariantString("0.6*"));
			grid4.GetValue(Grid.ColumnDefinitionsProperty).Add(columnDefinition2);
			stackLayout2.SetValue(StackLayout.OrientationProperty, 0);
			listView.SetValue(Grid.ColumnProperty, 0);
			listView.SetValue(VisualElement.HeightRequestProperty, 100.0);
			bindingExtension6.Path = "Translations";
			BindingBase bindingBase6 = bindingExtension6.ProvideValue(null);
			listView.SetBinding(ItemsView<Cell>.ItemsSourceProperty, bindingBase6);
			IDataTemplate dataTemplate4 = dataTemplate;
			CustomCodingEditor.<InitializeComponent>_anonXamlCDataTemplate_82 <InitializeComponent>_anonXamlCDataTemplate_ = new CustomCodingEditor.<InitializeComponent>_anonXamlCDataTemplate_82();
			object[] array9 = new object[0 + 8];
			array9[0] = dataTemplate;
			array9[1] = listView;
			array9[2] = stackLayout2;
			array9[3] = grid4;
			array9[4] = sfExpander2;
			array9[5] = stackLayout22;
			array9[6] = scrollView;
			array9[7] = this;
			<InitializeComponent>_anonXamlCDataTemplate_.parentValues = array9;
			<InitializeComponent>_anonXamlCDataTemplate_.root = this;
			dataTemplate4.LoadTemplate = new Func<object>(<InitializeComponent>_anonXamlCDataTemplate_.LoadDataTemplate);
			listView.SetValue(ItemsView<Cell>.ItemTemplateProperty, dataTemplate);
			stackLayout2.Children.Add(listView);
			columnDefinition3.SetValue(ColumnDefinition.WidthProperty, new GridLengthTypeConverter().ConvertFromInvariantString("*"));
			grid3.GetValue(Grid.ColumnDefinitionsProperty).Add(columnDefinition3);
			columnDefinition4.SetValue(ColumnDefinition.WidthProperty, new GridLengthTypeConverter().ConvertFromInvariantString("*"));
			grid3.GetValue(Grid.ColumnDefinitionsProperty).Add(columnDefinition4);
			button.SetValue(Grid.ColumnProperty, 0);
			button.Clicked += this.btnAddTranslation_Click;
			button.SetValue(Button.TextProperty, "Add");
			grid3.Children.Add(button);
			button2.SetValue(Grid.ColumnProperty, 1);
			button2.Clicked += this.btnDelTranslation_Click;
			button2.SetValue(Button.TextProperty, "Del");
			grid3.Children.Add(button2);
			stackLayout2.Children.Add(grid3);
			grid4.Children.Add(stackLayout2);
			stackLayout4.SetValue(Grid.ColumnProperty, 1);
			referenceExtension.Name = "lvTranslations";
			IMarkupExtension markupExtension9 = referenceExtension;
			XamlServiceProvider xamlServiceProvider9 = new XamlServiceProvider();
			Type typeFromHandle17 = typeof(IProvideValueTarget);
			object[] array10 = new object[0 + 7];
			array10[0] = bindingExtension7;
			array10[1] = stackLayout4;
			array10[2] = grid4;
			array10[3] = sfExpander2;
			array10[4] = stackLayout22;
			array10[5] = scrollView;
			array10[6] = this;
			object obj10;
			xamlServiceProvider9.Add(typeFromHandle17, obj10 = new SimpleValueTargetProvider(array10, typeof(BindingExtension).GetRuntimeProperty("Source"), nameScope));
			xamlServiceProvider9.Add(typeof(IReferenceProvider), obj10);
			Type typeFromHandle18 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver9 = new XmlNamespaceResolver();
			xmlNamespaceResolver9.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver9.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver9.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver9.Add("expander", "clr-namespace:Syncfusion.XForms.Expander;assembly=Syncfusion.Expander.XForms");
			xmlNamespaceResolver9.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver9.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver9.Add("settings", "clr-namespace:CarScannerXamarinForms.Settings");
			xamlServiceProvider9.Add(typeFromHandle18, new XamlTypeResolver(xmlNamespaceResolver9, typeof(CustomCodingEditor).GetTypeInfo().Assembly));
			xamlServiceProvider9.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(118, 33)));
			object obj11 = markupExtension9.ProvideValue(xamlServiceProvider9);
			bindingExtension7.Source = obj11;
			bindingExtension7.Path = "SelectedItem";
			bindingExtension7.Mode = 2;
			BindingBase bindingBase7 = bindingExtension7.ProvideValue(null);
			stackLayout4.SetBinding(BindableObject.BindingContextProperty, bindingBase7);
			stackLayout4.SetValue(StackLayout.OrientationProperty, 0);
			stackLayout3.SetValue(StackLayout.OrientationProperty, 1);
			label8.SetValue(Label.TextProperty, "Language:");
			label8.SetValue(View.VerticalOptionsProperty, LayoutOptions.Center);
			stackLayout3.Children.Add(label8);
			bindingExtension8.Mode = 1;
			bindingExtension8.Path = "Language";
			BindingBase bindingBase8 = bindingExtension8.ProvideValue(null);
			entry5.SetBinding(Entry.TextProperty, bindingBase8);
			entry5.SetValue(View.VerticalOptionsProperty, LayoutOptions.Center);
			entry5.SetValue(VisualElement.WidthRequestProperty, 50.0);
			stackLayout3.Children.Add(entry5);
			stackLayout4.Children.Add(stackLayout3);
			label9.SetValue(Label.TextProperty, "Name:");
			stackLayout4.Children.Add(label9);
			bindingExtension9.Mode = 1;
			bindingExtension9.Path = "Name";
			BindingBase bindingBase9 = bindingExtension9.ProvideValue(null);
			entry6.SetBinding(Entry.TextProperty, bindingBase9);
			stackLayout4.Children.Add(entry6);
			label10.SetValue(Label.TextProperty, "Description:");
			stackLayout4.Children.Add(label10);
			bindingExtension10.Mode = 1;
			bindingExtension10.Path = "ShortName";
			BindingBase bindingBase10 = bindingExtension10.ProvideValue(null);
			entry7.SetBinding(Entry.TextProperty, bindingBase10);
			stackLayout4.Children.Add(entry7);
			label11.SetValue(Label.TextProperty, "Inner description:");
			stackLayout4.Children.Add(label11);
			bindingExtension11.Mode = 1;
			bindingExtension11.Path = "AdditionalText";
			BindingBase bindingBase11 = bindingExtension11.ProvideValue(null);
			entry8.SetBinding(Entry.TextProperty, bindingBase11);
			stackLayout4.Children.Add(entry8);
			grid4.Children.Add(stackLayout4);
			sfExpander2.SetValue(SfExpander.ContentProperty, grid4);
			stackLayout22.Children.Add(sfExpander2);
			sfExpander3.SetValue(SfExpander.AnimationDurationProperty, 250.0);
			sfExpander3.SetValue(SfExpander.AnimationEasingProperty, 3);
			dynamicResourceExtension8.Key = "NavigationBarBackgroundColor";
			IMarkupExtension<DynamicResource> markupExtension10 = dynamicResourceExtension8;
			XamlServiceProvider xamlServiceProvider10 = new XamlServiceProvider();
			Type typeFromHandle19 = typeof(IProvideValueTarget);
			object[] array11 = new object[0 + 4];
			array11[0] = sfExpander3;
			array11[1] = stackLayout22;
			array11[2] = scrollView;
			array11[3] = this;
			object obj12;
			xamlServiceProvider10.Add(typeFromHandle19, obj12 = new SimpleValueTargetProvider(array11, SfExpander.HeaderBackgroundColorProperty, nameScope));
			xamlServiceProvider10.Add(typeof(IReferenceProvider), obj12);
			Type typeFromHandle20 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver10 = new XmlNamespaceResolver();
			xmlNamespaceResolver10.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver10.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver10.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver10.Add("expander", "clr-namespace:Syncfusion.XForms.Expander;assembly=Syncfusion.Expander.XForms");
			xmlNamespaceResolver10.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver10.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver10.Add("settings", "clr-namespace:CarScannerXamarinForms.Settings");
			xamlServiceProvider10.Add(typeFromHandle20, new XamlTypeResolver(xmlNamespaceResolver10, typeof(CustomCodingEditor).GetTypeInfo().Assembly));
			xamlServiceProvider10.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(143, 21)));
			DynamicResource dynamicResource8 = markupExtension10.ProvideValue(xamlServiceProvider10);
			sfExpander3.SetDynamicResource(SfExpander.HeaderBackgroundColorProperty, dynamicResource8.Key);
			dynamicResourceExtension9.Key = "NavigationBarButtonColor";
			IMarkupExtension<DynamicResource> markupExtension11 = dynamicResourceExtension9;
			XamlServiceProvider xamlServiceProvider11 = new XamlServiceProvider();
			Type typeFromHandle21 = typeof(IProvideValueTarget);
			object[] array12 = new object[0 + 4];
			array12[0] = sfExpander3;
			array12[1] = stackLayout22;
			array12[2] = scrollView;
			array12[3] = this;
			object obj13;
			xamlServiceProvider11.Add(typeFromHandle21, obj13 = new SimpleValueTargetProvider(array12, SfExpander.IconColorProperty, nameScope));
			xamlServiceProvider11.Add(typeof(IReferenceProvider), obj13);
			Type typeFromHandle22 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver11 = new XmlNamespaceResolver();
			xmlNamespaceResolver11.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver11.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver11.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver11.Add("expander", "clr-namespace:Syncfusion.XForms.Expander;assembly=Syncfusion.Expander.XForms");
			xmlNamespaceResolver11.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver11.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver11.Add("settings", "clr-namespace:CarScannerXamarinForms.Settings");
			xamlServiceProvider11.Add(typeFromHandle22, new XamlTypeResolver(xmlNamespaceResolver11, typeof(CustomCodingEditor).GetTypeInfo().Assembly));
			xamlServiceProvider11.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(144, 21)));
			DynamicResource dynamicResource9 = markupExtension11.ProvideValue(xamlServiceProvider11);
			sfExpander3.SetDynamicResource(SfExpander.IconColorProperty, dynamicResource9.Key);
			sfExpander3.SetValue(SfExpander.IsExpandedProperty, true);
			label12.SetValue(View.MarginProperty, new Thickness(5.0));
			label12.SetValue(Label.FontAttributesProperty, new FontAttributesConverter().ConvertFromInvariantString("Bold"));
			label12.SetValue(Label.TextProperty, "2. Address and unit:");
			dynamicResourceExtension10.Key = "NavigationBarTextColor";
			IMarkupExtension<DynamicResource> markupExtension12 = dynamicResourceExtension10;
			XamlServiceProvider xamlServiceProvider12 = new XamlServiceProvider();
			Type typeFromHandle23 = typeof(IProvideValueTarget);
			object[] array13 = new object[0 + 6];
			array13[0] = label12;
			array13[1] = grid5;
			array13[2] = sfExpander3;
			array13[3] = stackLayout22;
			array13[4] = scrollView;
			array13[5] = this;
			object obj14;
			xamlServiceProvider12.Add(typeFromHandle23, obj14 = new SimpleValueTargetProvider(array13, Label.TextColorProperty, nameScope));
			xamlServiceProvider12.Add(typeof(IReferenceProvider), obj14);
			Type typeFromHandle24 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver12 = new XmlNamespaceResolver();
			xmlNamespaceResolver12.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver12.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver12.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver12.Add("expander", "clr-namespace:Syncfusion.XForms.Expander;assembly=Syncfusion.Expander.XForms");
			xmlNamespaceResolver12.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver12.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver12.Add("settings", "clr-namespace:CarScannerXamarinForms.Settings");
			xamlServiceProvider12.Add(typeFromHandle24, new XamlTypeResolver(xmlNamespaceResolver12, typeof(CustomCodingEditor).GetTypeInfo().Assembly));
			xamlServiceProvider12.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(152, 33)));
			DynamicResource dynamicResource10 = markupExtension12.ProvideValue(xamlServiceProvider12);
			label12.SetDynamicResource(Label.TextColorProperty, dynamicResource10.Key);
			label12.SetValue(View.VerticalOptionsProperty, LayoutOptions.Center);
			label12.SetValue(Label.VerticalTextAlignmentProperty, new TextAlignmentConverter().ConvertFromInvariantString("Center"));
			grid5.Children.Add(label12);
			sfExpander3.SetValue(SfExpander.HeaderProperty, grid5);
			stackLayout13.SetValue(StackLayout.OrientationProperty, 0);
			stackLayout6.SetValue(StackLayout.OrientationProperty, 0);
			label13.SetValue(Label.TextProperty, "Request header:");
			stackLayout6.Children.Add(label13);
			bindingExtension12.Mode = 1;
			bindingExtension12.Path = "RequestHeader";
			BindingBase bindingBase12 = bindingExtension12.ProvideValue(null);
			entry9.SetBinding(Entry.TextProperty, bindingBase12);
			stackLayout6.Children.Add(entry9);
			label14.SetValue(Label.TextProperty, "Response header:");
			stackLayout6.Children.Add(label14);
			entry10.SetValue(View.MarginProperty, new Thickness(5.0, 0.0));
			bindingExtension13.Mode = 1;
			bindingExtension13.Path = "ResponseHeader";
			BindingBase bindingBase13 = bindingExtension13.ProvideValue(null);
			entry10.SetBinding(Entry.TextProperty, bindingBase13);
			stackLayout6.Children.Add(entry10);
			stackLayout5.SetValue(VisualElement.IsVisibleProperty, new VisualElement.VisibilityConverter().ConvertFromInvariantString("False"));
			stackLayout5.SetValue(StackLayout.OrientationProperty, 1);
			button3.SetValue(View.MarginProperty, new Thickness(2.0, 0.0));
			button3.Clicked += this.btnGenRequestHeader_Click;
			button3.SetValue(Button.TextProperty, "GEN");
			stackLayout5.Children.Add(button3);
			button4.SetValue(View.MarginProperty, new Thickness(2.0, 0.0));
			button4.Clicked += this.btnGenRequestHeader_Click;
			button4.SetValue(Button.TextProperty, "VAG");
			stackLayout5.Children.Add(button4);
			button5.SetValue(View.MarginProperty, new Thickness(2.0, 0.0));
			button5.Clicked += this.btnGenRequestHeader_Click;
			button5.SetValue(Button.TextProperty, "REN");
			stackLayout5.Children.Add(button5);
			stackLayout6.Children.Add(stackLayout5);
			stackLayout13.Children.Add(stackLayout6);
			stackLayout7.SetValue(StackLayout.OrientationProperty, 0);
			label15.SetValue(Label.TextProperty, "Extended address:");
			stackLayout7.Children.Add(label15);
			bindingExtension14.Mode = 1;
			bindingExtension14.Path = "ExtendedAddress";
			BindingBase bindingBase14 = bindingExtension14.ProvideValue(null);
			entry11.SetBinding(Entry.TextProperty, bindingBase14);
			stackLayout7.Children.Add(entry11);
			label16.SetValue(Label.TextProperty, "Tester address:");
			stackLayout7.Children.Add(label16);
			bindingExtension15.Mode = 1;
			bindingExtension15.Path = "TesterAddress";
			BindingBase bindingBase15 = bindingExtension15.ProvideValue(null);
			entry12.SetBinding(Entry.TextProperty, bindingBase15);
			stackLayout7.Children.Add(entry12);
			stackLayout13.Children.Add(stackLayout7);
			bindingExtension16.Mode = 1;
			bindingExtension16.Path = "PasswordVisible";
			BindingBase bindingBase16 = bindingExtension16.ProvideValue(null);
			labelSwitch.SetBinding(LabelSwitch.IsToggledProperty, bindingBase16);
			labelSwitch.SetValue(LabelSwitch.TextProperty, "Password visible:");
			stackLayout13.Children.Add(labelSwitch);
			bindingExtension17.Mode = 2;
			bindingExtension17.Path = "PasswordVisible";
			BindingBase bindingBase17 = bindingExtension17.ProvideValue(null);
			stackLayout8.SetBinding(VisualElement.IsVisibleProperty, bindingBase17);
			stackLayout8.SetValue(StackLayout.OrientationProperty, 0);
			label17.SetValue(Label.TextProperty, "Password:");
			stackLayout8.Children.Add(label17);
			bindingExtension18.Mode = 1;
			bindingExtension18.Path = "Password";
			BindingBase bindingBase18 = bindingExtension18.ProvideValue(null);
			entry13.SetBinding(Entry.TextProperty, bindingBase18);
			stackLayout8.Children.Add(entry13);
			label18.SetValue(Label.TextProperty, "Password hint:");
			stackLayout8.Children.Add(label18);
			bindingExtension19.Mode = 1;
			bindingExtension19.Path = "PasswordHint";
			BindingBase bindingBase19 = bindingExtension19.ProvideValue(null);
			entry14.SetBinding(Entry.TextProperty, bindingBase19);
			stackLayout8.Children.Add(entry14);
			stackLayout13.Children.Add(stackLayout8);
			columnDefinition5.SetValue(ColumnDefinition.WidthProperty, new GridLengthTypeConverter().ConvertFromInvariantString("0.45*"));
			grid6.GetValue(Grid.ColumnDefinitionsProperty).Add(columnDefinition5);
			columnDefinition6.SetValue(ColumnDefinition.WidthProperty, new GridLengthTypeConverter().ConvertFromInvariantString("0.1*"));
			grid6.GetValue(Grid.ColumnDefinitionsProperty).Add(columnDefinition6);
			columnDefinition7.SetValue(ColumnDefinition.WidthProperty, new GridLengthTypeConverter().ConvertFromInvariantString("0.45*"));
			grid6.GetValue(Grid.ColumnDefinitionsProperty).Add(columnDefinition7);
			stackLayout9.SetValue(Grid.ColumnProperty, 0);
			stackLayout9.SetValue(StackLayout.OrientationProperty, 0);
			label19.SetValue(Label.TextProperty, "Read mode and address:");
			stackLayout9.Children.Add(label19);
			bindingExtension20.Mode = 1;
			bindingExtension20.Path = "ReadModeAndAddress";
			BindingBase bindingBase20 = bindingExtension20.ProvideValue(null);
			entry15.SetBinding(Entry.TextProperty, bindingBase20);
			stackLayout9.Children.Add(entry15);
			grid6.Children.Add(stackLayout9);
			button6.SetValue(Grid.ColumnProperty, 1);
			button6.Clicked += this.btnReadAddressToWriteAddress_Click;
			button6.SetValue(View.HorizontalOptionsProperty, LayoutOptions.Center);
			button6.SetValue(Button.TextProperty, ">");
			button6.SetValue(View.VerticalOptionsProperty, LayoutOptions.Center);
			grid6.Children.Add(button6);
			stackLayout10.SetValue(Grid.ColumnProperty, 2);
			stackLayout10.SetValue(StackLayout.OrientationProperty, 0);
			label20.SetValue(Label.TextProperty, "Write mode and address:");
			stackLayout10.Children.Add(label20);
			bindingExtension21.Mode = 1;
			bindingExtension21.Path = "WriteModeAndAddress";
			BindingBase bindingBase21 = bindingExtension21.ProvideValue(null);
			entry16.SetBinding(Entry.TextProperty, bindingBase21);
			stackLayout10.Children.Add(entry16);
			grid6.Children.Add(stackLayout10);
			stackLayout13.Children.Add(grid6);
			stackLayout11.SetValue(StackLayout.OrientationProperty, 1);
			label21.SetValue(Label.TextProperty, "ATST (hex): ");
			label21.SetValue(View.VerticalOptionsProperty, LayoutOptions.Center);
			stackLayout11.Children.Add(label21);
			entry17.SetValue(View.MarginProperty, new Thickness(5.0, 0.0));
			bindingExtension22.Mode = 1;
			bindingExtension22.Path = "ATST";
			BindingBase bindingBase22 = bindingExtension22.ProvideValue(null);
			entry17.SetBinding(Entry.TextProperty, bindingBase22);
			entry17.SetValue(View.VerticalOptionsProperty, LayoutOptions.Center);
			entry17.SetValue(VisualElement.WidthRequestProperty, 50.0);
			stackLayout11.Children.Add(entry17);
			stackLayout13.Children.Add(stackLayout11);
			stackLayout12.SetValue(StackLayout.OrientationProperty, 1);
			label22.SetValue(Label.TextProperty, "Protocol: ");
			label22.SetValue(View.VerticalOptionsProperty, LayoutOptions.Center);
			stackLayout12.Children.Add(label22);
			entry18.SetValue(View.MarginProperty, new Thickness(5.0, 0.0));
			bindingExtension23.Mode = 1;
			bindingExtension23.Path = "Protocol";
			BindingBase bindingBase23 = bindingExtension23.ProvideValue(null);
			entry18.SetBinding(Entry.TextProperty, bindingBase23);
			entry18.SetValue(View.VerticalOptionsProperty, LayoutOptions.Center);
			entry18.SetValue(VisualElement.WidthRequestProperty, 50.0);
			stackLayout12.Children.Add(entry18);
			stackLayout13.Children.Add(stackLayout12);
			label23.SetValue(Label.TextProperty, "Open session command: ");
			stackLayout13.Children.Add(label23);
			bindingExtension24.Mode = 1;
			bindingExtension24.Path = "OpenSessionCommand";
			BindingBase bindingBase24 = bindingExtension24.ProvideValue(null);
			entry19.SetBinding(Entry.TextProperty, bindingBase24);
			stackLayout13.Children.Add(entry19);
			label24.SetValue(Label.TextProperty, "PreRead Commands:");
			stackLayout13.Children.Add(label24);
			bindingExtension25.Mode = 1;
			bindingExtension25.Path = "PreReadCommands";
			BindingBase bindingBase25 = bindingExtension25.ProvideValue(null);
			entry20.SetBinding(Entry.TextProperty, bindingBase25);
			stackLayout13.Children.Add(entry20);
			label25.SetValue(Label.TextProperty, "PreWrite Commands:");
			stackLayout13.Children.Add(label25);
			bindingExtension26.Mode = 1;
			bindingExtension26.Path = "PreWriteCommands";
			BindingBase bindingBase26 = bindingExtension26.ProvideValue(null);
			entry21.SetBinding(Entry.TextProperty, bindingBase26);
			stackLayout13.Children.Add(entry21);
			label26.SetValue(Label.TextProperty, "PostWrite Commands:");
			stackLayout13.Children.Add(label26);
			bindingExtension27.Mode = 1;
			bindingExtension27.Path = "PostWriteCommands";
			BindingBase bindingBase27 = bindingExtension27.ProvideValue(null);
			entry22.SetBinding(Entry.TextProperty, bindingBase27);
			stackLayout13.Children.Add(entry22);
			bindingExtension28.Mode = 1;
			bindingExtension28.Path = "MakeChangesToInitialData";
			BindingBase bindingBase28 = bindingExtension28.ProvideValue(null);
			labelSwitch2.SetBinding(LabelSwitch.IsToggledProperty, bindingBase28);
			labelSwitch2.SetValue(LabelSwitch.TextProperty, "Make changes to initial data");
			stackLayout13.Children.Add(labelSwitch2);
			sfExpander3.SetValue(SfExpander.ContentProperty, stackLayout13);
			stackLayout22.Children.Add(sfExpander3);
			sfExpander4.SetValue(SfExpander.AnimationDurationProperty, 250.0);
			sfExpander4.SetValue(SfExpander.AnimationEasingProperty, 3);
			dynamicResourceExtension11.Key = "NavigationBarBackgroundColor";
			IMarkupExtension<DynamicResource> markupExtension13 = dynamicResourceExtension11;
			XamlServiceProvider xamlServiceProvider13 = new XamlServiceProvider();
			Type typeFromHandle25 = typeof(IProvideValueTarget);
			object[] array14 = new object[0 + 4];
			array14[0] = sfExpander4;
			array14[1] = stackLayout22;
			array14[2] = scrollView;
			array14[3] = this;
			object obj15;
			xamlServiceProvider13.Add(typeFromHandle25, obj15 = new SimpleValueTargetProvider(array14, SfExpander.HeaderBackgroundColorProperty, nameScope));
			xamlServiceProvider13.Add(typeof(IReferenceProvider), obj15);
			Type typeFromHandle26 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver13 = new XmlNamespaceResolver();
			xmlNamespaceResolver13.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver13.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver13.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver13.Add("expander", "clr-namespace:Syncfusion.XForms.Expander;assembly=Syncfusion.Expander.XForms");
			xmlNamespaceResolver13.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver13.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver13.Add("settings", "clr-namespace:CarScannerXamarinForms.Settings");
			xamlServiceProvider13.Add(typeFromHandle26, new XamlTypeResolver(xmlNamespaceResolver13, typeof(CustomCodingEditor).GetTypeInfo().Assembly));
			xamlServiceProvider13.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(259, 21)));
			DynamicResource dynamicResource11 = markupExtension13.ProvideValue(xamlServiceProvider13);
			sfExpander4.SetDynamicResource(SfExpander.HeaderBackgroundColorProperty, dynamicResource11.Key);
			dynamicResourceExtension12.Key = "NavigationBarButtonColor";
			IMarkupExtension<DynamicResource> markupExtension14 = dynamicResourceExtension12;
			XamlServiceProvider xamlServiceProvider14 = new XamlServiceProvider();
			Type typeFromHandle27 = typeof(IProvideValueTarget);
			object[] array15 = new object[0 + 4];
			array15[0] = sfExpander4;
			array15[1] = stackLayout22;
			array15[2] = scrollView;
			array15[3] = this;
			object obj16;
			xamlServiceProvider14.Add(typeFromHandle27, obj16 = new SimpleValueTargetProvider(array15, SfExpander.IconColorProperty, nameScope));
			xamlServiceProvider14.Add(typeof(IReferenceProvider), obj16);
			Type typeFromHandle28 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver14 = new XmlNamespaceResolver();
			xmlNamespaceResolver14.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver14.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver14.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver14.Add("expander", "clr-namespace:Syncfusion.XForms.Expander;assembly=Syncfusion.Expander.XForms");
			xmlNamespaceResolver14.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver14.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver14.Add("settings", "clr-namespace:CarScannerXamarinForms.Settings");
			xamlServiceProvider14.Add(typeFromHandle28, new XamlTypeResolver(xmlNamespaceResolver14, typeof(CustomCodingEditor).GetTypeInfo().Assembly));
			xamlServiceProvider14.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(260, 21)));
			DynamicResource dynamicResource12 = markupExtension14.ProvideValue(xamlServiceProvider14);
			sfExpander4.SetDynamicResource(SfExpander.IconColorProperty, dynamicResource12.Key);
			sfExpander4.SetValue(SfExpander.IsExpandedProperty, true);
			label27.SetValue(View.MarginProperty, new Thickness(5.0));
			label27.SetValue(Label.FontAttributesProperty, new FontAttributesConverter().ConvertFromInvariantString("Bold"));
			label27.SetValue(Label.TextProperty, "3. Value type:");
			dynamicResourceExtension13.Key = "NavigationBarTextColor";
			IMarkupExtension<DynamicResource> markupExtension15 = dynamicResourceExtension13;
			XamlServiceProvider xamlServiceProvider15 = new XamlServiceProvider();
			Type typeFromHandle29 = typeof(IProvideValueTarget);
			object[] array16 = new object[0 + 6];
			array16[0] = label27;
			array16[1] = grid7;
			array16[2] = sfExpander4;
			array16[3] = stackLayout22;
			array16[4] = scrollView;
			array16[5] = this;
			object obj17;
			xamlServiceProvider15.Add(typeFromHandle29, obj17 = new SimpleValueTargetProvider(array16, Label.TextColorProperty, nameScope));
			xamlServiceProvider15.Add(typeof(IReferenceProvider), obj17);
			Type typeFromHandle30 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver15 = new XmlNamespaceResolver();
			xmlNamespaceResolver15.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver15.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver15.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver15.Add("expander", "clr-namespace:Syncfusion.XForms.Expander;assembly=Syncfusion.Expander.XForms");
			xmlNamespaceResolver15.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver15.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver15.Add("settings", "clr-namespace:CarScannerXamarinForms.Settings");
			xamlServiceProvider15.Add(typeFromHandle30, new XamlTypeResolver(xmlNamespaceResolver15, typeof(CustomCodingEditor).GetTypeInfo().Assembly));
			xamlServiceProvider15.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(268, 33)));
			DynamicResource dynamicResource13 = markupExtension15.ProvideValue(xamlServiceProvider15);
			label27.SetDynamicResource(Label.TextColorProperty, dynamicResource13.Key);
			label27.SetValue(View.VerticalOptionsProperty, LayoutOptions.Center);
			label27.SetValue(Label.VerticalTextAlignmentProperty, new TextAlignmentConverter().ConvertFromInvariantString("Center"));
			grid7.Children.Add(label27);
			sfExpander4.SetValue(SfExpander.HeaderProperty, grid7);
			stackLayout21.SetValue(StackLayout.OrientationProperty, 0);
			bindingExtension29.Mode = 1;
			staticResourceExtension2.Key = "CodingInputValueTypeToIntConverterForCustomCodingEditor";
			IMarkupExtension markupExtension16 = staticResourceExtension2;
			XamlServiceProvider xamlServiceProvider16 = new XamlServiceProvider();
			Type typeFromHandle31 = typeof(IProvideValueTarget);
			object[] array17 = new object[0 + 7];
			array17[0] = bindingExtension29;
			array17[1] = picker2;
			array17[2] = stackLayout21;
			array17[3] = sfExpander4;
			array17[4] = stackLayout22;
			array17[5] = scrollView;
			array17[6] = this;
			object obj18;
			xamlServiceProvider16.Add(typeFromHandle31, obj18 = new SimpleValueTargetProvider(array17, typeof(BindingExtension).GetRuntimeProperty("Converter"), nameScope));
			xamlServiceProvider16.Add(typeof(IReferenceProvider), obj18);
			Type typeFromHandle32 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver16 = new XmlNamespaceResolver();
			xmlNamespaceResolver16.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver16.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver16.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver16.Add("expander", "clr-namespace:Syncfusion.XForms.Expander;assembly=Syncfusion.Expander.XForms");
			xmlNamespaceResolver16.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver16.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver16.Add("settings", "clr-namespace:CarScannerXamarinForms.Settings");
			xamlServiceProvider16.Add(typeFromHandle32, new XamlTypeResolver(xmlNamespaceResolver16, typeof(CustomCodingEditor).GetTypeInfo().Assembly));
			xamlServiceProvider16.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(276, 57)));
			object obj19 = markupExtension16.ProvideValue(xamlServiceProvider16);
			bindingExtension29.Converter = obj19;
			bindingExtension29.Path = "ValueType";
			BindingBase bindingBase29 = bindingExtension29.ProvideValue(null);
			picker2.SetBinding(Picker.SelectedIndexProperty, bindingBase29);
			picker2.Items.Add(text);
			picker2.Items.Add(text2);
			picker2.Items.Add(text3);
			picker2.Items.Add(text4);
			picker2.Items.Add(text5);
			stackLayout21.Children.Add(picker2);
			grid12.SetValue(View.MarginProperty, new Thickness(2.0));
			bindingExtension30.Mode = 2;
			staticResourceExtension3.Key = "EnumValueToTrueConverter";
			IMarkupExtension markupExtension17 = staticResourceExtension3;
			XamlServiceProvider xamlServiceProvider17 = new XamlServiceProvider();
			Type typeFromHandle33 = typeof(IProvideValueTarget);
			object[] array18 = new object[0 + 7];
			array18[0] = bindingExtension30;
			array18[1] = grid12;
			array18[2] = stackLayout21;
			array18[3] = sfExpander4;
			array18[4] = stackLayout22;
			array18[5] = scrollView;
			array18[6] = this;
			object obj20;
			xamlServiceProvider17.Add(typeFromHandle33, obj20 = new SimpleValueTargetProvider(array18, typeof(BindingExtension).GetRuntimeProperty("Converter"), nameScope));
			xamlServiceProvider17.Add(typeof(IReferenceProvider), obj20);
			Type typeFromHandle34 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver17 = new XmlNamespaceResolver();
			xmlNamespaceResolver17.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver17.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver17.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver17.Add("expander", "clr-namespace:Syncfusion.XForms.Expander;assembly=Syncfusion.Expander.XForms");
			xmlNamespaceResolver17.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver17.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver17.Add("settings", "clr-namespace:CarScannerXamarinForms.Settings");
			xamlServiceProvider17.Add(typeFromHandle34, new XamlTypeResolver(xmlNamespaceResolver17, typeof(CustomCodingEditor).GetTypeInfo().Assembly));
			xamlServiceProvider17.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(304, 33)));
			object obj21 = markupExtension17.ProvideValue(xamlServiceProvider17);
			bindingExtension30.Converter = obj21;
			bindingExtension30.ConverterParameter = "0";
			bindingExtension30.Path = "ValueType";
			BindingBase bindingBase30 = bindingExtension30.ProvideValue(null);
			grid12.SetBinding(VisualElement.IsVisibleProperty, bindingBase30);
			columnDefinition8.SetValue(ColumnDefinition.WidthProperty, new GridLengthTypeConverter().ConvertFromInvariantString("0.4*"));
			grid12.GetValue(Grid.ColumnDefinitionsProperty).Add(columnDefinition8);
			columnDefinition9.SetValue(ColumnDefinition.WidthProperty, new GridLengthTypeConverter().ConvertFromInvariantString("0.6*"));
			grid12.GetValue(Grid.ColumnDefinitionsProperty).Add(columnDefinition9);
			rowDefinition.SetValue(RowDefinition.HeightProperty, new GridLengthTypeConverter().ConvertFromInvariantString("auto"));
			grid12.GetValue(Grid.RowDefinitionsProperty).Add(rowDefinition);
			rowDefinition2.SetValue(RowDefinition.HeightProperty, new GridLengthTypeConverter().ConvertFromInvariantString("auto"));
			grid12.GetValue(Grid.RowDefinitionsProperty).Add(rowDefinition2);
			stackLayout14.SetValue(Grid.RowProperty, 0);
			stackLayout14.SetValue(Grid.ColumnProperty, 0);
			stackLayout14.SetValue(Grid.ColumnSpanProperty, 2);
			stackLayout14.SetValue(StackLayout.OrientationProperty, 1);
			label28.SetValue(Label.TextProperty, "Byte number (starting from 0): ");
			label28.SetValue(View.VerticalOptionsProperty, LayoutOptions.Center);
			stackLayout14.Children.Add(label28);
			bindingExtension31.Mode = 1;
			staticResourceExtension4.Key = "IntToStringConverter";
			IMarkupExtension markupExtension18 = staticResourceExtension4;
			XamlServiceProvider xamlServiceProvider18 = new XamlServiceProvider();
			Type typeFromHandle35 = typeof(IProvideValueTarget);
			object[] array19 = new object[0 + 9];
			array19[0] = bindingExtension31;
			array19[1] = entry23;
			array19[2] = stackLayout14;
			array19[3] = grid12;
			array19[4] = stackLayout21;
			array19[5] = sfExpander4;
			array19[6] = stackLayout22;
			array19[7] = scrollView;
			array19[8] = this;
			object obj22;
			xamlServiceProvider18.Add(typeFromHandle35, obj22 = new SimpleValueTargetProvider(array19, typeof(BindingExtension).GetRuntimeProperty("Converter"), nameScope));
			xamlServiceProvider18.Add(typeof(IReferenceProvider), obj22);
			Type typeFromHandle36 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver18 = new XmlNamespaceResolver();
			xmlNamespaceResolver18.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver18.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver18.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver18.Add("expander", "clr-namespace:Syncfusion.XForms.Expander;assembly=Syncfusion.Expander.XForms");
			xmlNamespaceResolver18.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver18.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver18.Add("settings", "clr-namespace:CarScannerXamarinForms.Settings");
			xamlServiceProvider18.Add(typeFromHandle36, new XamlTypeResolver(xmlNamespaceResolver18, typeof(CustomCodingEditor).GetTypeInfo().Assembly));
			xamlServiceProvider18.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(321, 41)));
			object obj23 = markupExtension18.ProvideValue(xamlServiceProvider18);
			bindingExtension31.Converter = obj23;
			bindingExtension31.Path = "StartByteId";
			BindingBase bindingBase31 = bindingExtension31.ProvideValue(null);
			entry23.SetBinding(Entry.TextProperty, bindingBase31);
			entry23.SetValue(View.VerticalOptionsProperty, LayoutOptions.Center);
			entry23.SetValue(VisualElement.WidthRequestProperty, 80.0);
			stackLayout14.Children.Add(entry23);
			grid12.Children.Add(stackLayout14);
			stackLayout15.SetValue(Grid.RowProperty, 1);
			stackLayout15.SetValue(Grid.ColumnProperty, 0);
			stackLayout15.SetValue(StackLayout.OrientationProperty, 0);
			listView2.SetValue(Grid.ColumnProperty, 0);
			listView2.SetValue(VisualElement.HeightRequestProperty, 180.0);
			bindingExtension32.Path = "Options";
			BindingBase bindingBase32 = bindingExtension32.ProvideValue(null);
			listView2.SetBinding(ItemsView<Cell>.ItemsSourceProperty, bindingBase32);
			IDataTemplate dataTemplate5 = dataTemplate2;
			CustomCodingEditor.<InitializeComponent>_anonXamlCDataTemplate_83 <InitializeComponent>_anonXamlCDataTemplate_2 = new CustomCodingEditor.<InitializeComponent>_anonXamlCDataTemplate_83();
			object[] array20 = new object[0 + 9];
			array20[0] = dataTemplate2;
			array20[1] = listView2;
			array20[2] = stackLayout15;
			array20[3] = grid12;
			array20[4] = stackLayout21;
			array20[5] = sfExpander4;
			array20[6] = stackLayout22;
			array20[7] = scrollView;
			array20[8] = this;
			<InitializeComponent>_anonXamlCDataTemplate_2.parentValues = array20;
			<InitializeComponent>_anonXamlCDataTemplate_2.root = this;
			dataTemplate5.LoadTemplate = new Func<object>(<InitializeComponent>_anonXamlCDataTemplate_2.LoadDataTemplate);
			listView2.SetValue(ItemsView<Cell>.ItemTemplateProperty, dataTemplate2);
			stackLayout15.Children.Add(listView2);
			columnDefinition10.SetValue(ColumnDefinition.WidthProperty, new GridLengthTypeConverter().ConvertFromInvariantString("*"));
			grid8.GetValue(Grid.ColumnDefinitionsProperty).Add(columnDefinition10);
			columnDefinition11.SetValue(ColumnDefinition.WidthProperty, new GridLengthTypeConverter().ConvertFromInvariantString("*"));
			grid8.GetValue(Grid.ColumnDefinitionsProperty).Add(columnDefinition11);
			button7.SetValue(Grid.ColumnProperty, 0);
			button7.Clicked += this.btnAddOption_Click;
			button7.SetValue(Button.TextProperty, "Add");
			grid8.Children.Add(button7);
			button8.SetValue(Grid.ColumnProperty, 1);
			button8.Clicked += this.btnDelOption_Click;
			button8.SetValue(Button.TextProperty, "Del");
			grid8.Children.Add(button8);
			stackLayout15.Children.Add(grid8);
			columnDefinition12.SetValue(ColumnDefinition.WidthProperty, new GridLengthTypeConverter().ConvertFromInvariantString("*"));
			grid9.GetValue(Grid.ColumnDefinitionsProperty).Add(columnDefinition12);
			columnDefinition13.SetValue(ColumnDefinition.WidthProperty, new GridLengthTypeConverter().ConvertFromInvariantString("*"));
			grid9.GetValue(Grid.ColumnDefinitionsProperty).Add(columnDefinition13);
			button9.SetValue(Grid.ColumnProperty, 0);
			button9.Clicked += this.btnUpOption_Click;
			button9.SetValue(Button.TextProperty, "Up");
			grid9.Children.Add(button9);
			button10.SetValue(Grid.ColumnProperty, 1);
			button10.Clicked += this.btnDownOption_Click;
			button10.SetValue(Button.TextProperty, "Down");
			grid9.Children.Add(button10);
			stackLayout15.Children.Add(grid9);
			grid12.Children.Add(stackLayout15);
			stackLayout18.SetValue(Grid.RowProperty, 1);
			stackLayout18.SetValue(Grid.ColumnProperty, 1);
			referenceExtension2.Name = "lvOptions";
			IMarkupExtension markupExtension19 = referenceExtension2;
			XamlServiceProvider xamlServiceProvider19 = new XamlServiceProvider();
			Type typeFromHandle37 = typeof(IProvideValueTarget);
			object[] array21 = new object[0 + 8];
			array21[0] = bindingExtension33;
			array21[1] = stackLayout18;
			array21[2] = grid12;
			array21[3] = stackLayout21;
			array21[4] = sfExpander4;
			array21[5] = stackLayout22;
			array21[6] = scrollView;
			array21[7] = this;
			object obj24;
			xamlServiceProvider19.Add(typeFromHandle37, obj24 = new SimpleValueTargetProvider(array21, typeof(BindingExtension).GetRuntimeProperty("Source"), nameScope));
			xamlServiceProvider19.Add(typeof(IReferenceProvider), obj24);
			Type typeFromHandle38 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver19 = new XmlNamespaceResolver();
			xmlNamespaceResolver19.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver19.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver19.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver19.Add("expander", "clr-namespace:Syncfusion.XForms.Expander;assembly=Syncfusion.Expander.XForms");
			xmlNamespaceResolver19.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver19.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver19.Add("settings", "clr-namespace:CarScannerXamarinForms.Settings");
			xamlServiceProvider19.Add(typeFromHandle38, new XamlTypeResolver(xmlNamespaceResolver19, typeof(CustomCodingEditor).GetTypeInfo().Assembly));
			xamlServiceProvider19.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(378, 37)));
			object obj25 = markupExtension19.ProvideValue(xamlServiceProvider19);
			bindingExtension33.Source = obj25;
			bindingExtension33.Path = "SelectedItem";
			BindingBase bindingBase33 = bindingExtension33.ProvideValue(null);
			stackLayout18.SetBinding(BindableObject.BindingContextProperty, bindingBase33);
			stackLayout18.SetValue(StackLayout.OrientationProperty, 0);
			label29.SetValue(Label.TextProperty, "Title:");
			stackLayout18.Children.Add(label29);
			bindingExtension34.Mode = 1;
			bindingExtension34.Path = "TitleRaw";
			BindingBase bindingBase34 = bindingExtension34.ProvideValue(null);
			entry24.SetBinding(Entry.TextProperty, bindingBase34);
			stackLayout18.Children.Add(entry24);
			label30.SetValue(Label.TextProperty, "Value:");
			stackLayout18.Children.Add(label30);
			bindingExtension35.Mode = 1;
			bindingExtension35.Path = "Value";
			BindingBase bindingBase35 = bindingExtension35.ProvideValue(null);
			entry25.SetBinding(Entry.TextProperty, bindingBase35);
			stackLayout18.Children.Add(entry25);
			label31.SetValue(Label.TextProperty, "Read only value:");
			stackLayout18.Children.Add(label31);
			bindingExtension36.Mode = 1;
			bindingExtension36.Path = "ReadOnlyValue";
			BindingBase bindingBase36 = bindingExtension36.ProvideValue(null);
			entry26.SetBinding(Entry.TextProperty, bindingBase36);
			stackLayout18.Children.Add(entry26);
			label32.SetValue(View.MarginProperty, new Thickness(5.0));
			label32.SetValue(Label.FontAttributesProperty, new FontAttributesConverter().ConvertFromInvariantString("Bold"));
			label32.SetValue(Label.TextProperty, "Translations:");
			label32.SetValue(View.VerticalOptionsProperty, LayoutOptions.Center);
			label32.SetValue(Label.VerticalTextAlignmentProperty, new TextAlignmentConverter().ConvertFromInvariantString("Center"));
			stackLayout18.Children.Add(label32);
			columnDefinition14.SetValue(ColumnDefinition.WidthProperty, new GridLengthTypeConverter().ConvertFromInvariantString("0.4*"));
			grid11.GetValue(Grid.ColumnDefinitionsProperty).Add(columnDefinition14);
			columnDefinition15.SetValue(ColumnDefinition.WidthProperty, new GridLengthTypeConverter().ConvertFromInvariantString("0.6*"));
			grid11.GetValue(Grid.ColumnDefinitionsProperty).Add(columnDefinition15);
			stackLayout16.SetValue(StackLayout.OrientationProperty, 0);
			listView3.SetValue(Grid.ColumnProperty, 0);
			listView3.SetValue(VisualElement.HeightRequestProperty, 80.0);
			bindingExtension37.Path = "Translations";
			BindingBase bindingBase37 = bindingExtension37.ProvideValue(null);
			listView3.SetBinding(ItemsView<Cell>.ItemsSourceProperty, bindingBase37);
			IDataTemplate dataTemplate6 = dataTemplate3;
			CustomCodingEditor.<InitializeComponent>_anonXamlCDataTemplate_84 <InitializeComponent>_anonXamlCDataTemplate_3 = new CustomCodingEditor.<InitializeComponent>_anonXamlCDataTemplate_84();
			object[] array22 = new object[0 + 11];
			array22[0] = dataTemplate3;
			array22[1] = listView3;
			array22[2] = stackLayout16;
			array22[3] = grid11;
			array22[4] = stackLayout18;
			array22[5] = grid12;
			array22[6] = stackLayout21;
			array22[7] = sfExpander4;
			array22[8] = stackLayout22;
			array22[9] = scrollView;
			array22[10] = this;
			<InitializeComponent>_anonXamlCDataTemplate_3.parentValues = array22;
			<InitializeComponent>_anonXamlCDataTemplate_3.root = this;
			dataTemplate6.LoadTemplate = new Func<object>(<InitializeComponent>_anonXamlCDataTemplate_3.LoadDataTemplate);
			listView3.SetValue(ItemsView<Cell>.ItemTemplateProperty, dataTemplate3);
			stackLayout16.Children.Add(listView3);
			columnDefinition16.SetValue(ColumnDefinition.WidthProperty, new GridLengthTypeConverter().ConvertFromInvariantString("*"));
			grid10.GetValue(Grid.ColumnDefinitionsProperty).Add(columnDefinition16);
			columnDefinition17.SetValue(ColumnDefinition.WidthProperty, new GridLengthTypeConverter().ConvertFromInvariantString("*"));
			grid10.GetValue(Grid.ColumnDefinitionsProperty).Add(columnDefinition17);
			button11.SetValue(Grid.ColumnProperty, 0);
			button11.Clicked += this.btnAddOptionTranslation_Click;
			button11.SetValue(Button.TextProperty, "+");
			grid10.Children.Add(button11);
			button12.SetValue(Grid.ColumnProperty, 1);
			button12.Clicked += this.btnDelOptionTranslation_Click;
			button12.SetValue(Button.TextProperty, "-");
			grid10.Children.Add(button12);
			stackLayout16.Children.Add(grid10);
			grid11.Children.Add(stackLayout16);
			stackLayout17.SetValue(Grid.ColumnProperty, 1);
			referenceExtension3.Name = "lvOptionTranslations";
			IMarkupExtension markupExtension20 = referenceExtension3;
			XamlServiceProvider xamlServiceProvider20 = new XamlServiceProvider();
			Type typeFromHandle39 = typeof(IProvideValueTarget);
			object[] array23 = new object[0 + 10];
			array23[0] = bindingExtension38;
			array23[1] = stackLayout17;
			array23[2] = grid11;
			array23[3] = stackLayout18;
			array23[4] = grid12;
			array23[5] = stackLayout21;
			array23[6] = sfExpander4;
			array23[7] = stackLayout22;
			array23[8] = scrollView;
			array23[9] = this;
			object obj26;
			xamlServiceProvider20.Add(typeFromHandle39, obj26 = new SimpleValueTargetProvider(array23, typeof(BindingExtension).GetRuntimeProperty("Source"), nameScope));
			xamlServiceProvider20.Add(typeof(IReferenceProvider), obj26);
			Type typeFromHandle40 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver20 = new XmlNamespaceResolver();
			xmlNamespaceResolver20.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver20.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver20.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver20.Add("expander", "clr-namespace:Syncfusion.XForms.Expander;assembly=Syncfusion.Expander.XForms");
			xmlNamespaceResolver20.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver20.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver20.Add("settings", "clr-namespace:CarScannerXamarinForms.Settings");
			xamlServiceProvider20.Add(typeFromHandle40, new XamlTypeResolver(xmlNamespaceResolver20, typeof(CustomCodingEditor).GetTypeInfo().Assembly));
			xamlServiceProvider20.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(434, 45)));
			object obj27 = markupExtension20.ProvideValue(xamlServiceProvider20);
			bindingExtension38.Source = obj27;
			bindingExtension38.Path = "SelectedItem";
			BindingBase bindingBase38 = bindingExtension38.ProvideValue(null);
			stackLayout17.SetBinding(BindableObject.BindingContextProperty, bindingBase38);
			stackLayout17.SetValue(StackLayout.OrientationProperty, 0);
			label33.SetValue(Label.TextProperty, "Language:");
			stackLayout17.Children.Add(label33);
			bindingExtension39.Mode = 1;
			bindingExtension39.Path = "Language";
			BindingBase bindingBase39 = bindingExtension39.ProvideValue(null);
			entry27.SetBinding(Entry.TextProperty, bindingBase39);
			stackLayout17.Children.Add(entry27);
			label34.SetValue(Label.TextProperty, "Title:");
			stackLayout17.Children.Add(label34);
			bindingExtension40.Mode = 1;
			bindingExtension40.Path = "Name";
			BindingBase bindingBase40 = bindingExtension40.ProvideValue(null);
			entry28.SetBinding(Entry.TextProperty, bindingBase40);
			stackLayout17.Children.Add(entry28);
			grid11.Children.Add(stackLayout17);
			stackLayout18.Children.Add(grid11);
			grid12.Children.Add(stackLayout18);
			stackLayout21.Children.Add(grid12);
			stackLayout19.SetValue(Grid.ColumnProperty, 0);
			stackLayout19.SetValue(View.MarginProperty, new Thickness(2.0));
			bindingExtension41.Mode = 2;
			staticResourceExtension5.Key = "EnumValueToTrueConverter";
			IMarkupExtension markupExtension21 = staticResourceExtension5;
			XamlServiceProvider xamlServiceProvider21 = new XamlServiceProvider();
			Type typeFromHandle41 = typeof(IProvideValueTarget);
			object[] array24 = new object[0 + 7];
			array24[0] = bindingExtension41;
			array24[1] = stackLayout19;
			array24[2] = stackLayout21;
			array24[3] = sfExpander4;
			array24[4] = stackLayout22;
			array24[5] = scrollView;
			array24[6] = this;
			object obj28;
			xamlServiceProvider21.Add(typeFromHandle41, obj28 = new SimpleValueTargetProvider(array24, typeof(BindingExtension).GetRuntimeProperty("Converter"), nameScope));
			xamlServiceProvider21.Add(typeof(IReferenceProvider), obj28);
			Type typeFromHandle42 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver21 = new XmlNamespaceResolver();
			xmlNamespaceResolver21.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver21.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver21.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver21.Add("expander", "clr-namespace:Syncfusion.XForms.Expander;assembly=Syncfusion.Expander.XForms");
			xmlNamespaceResolver21.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver21.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver21.Add("settings", "clr-namespace:CarScannerXamarinForms.Settings");
			xamlServiceProvider21.Add(typeFromHandle42, new XamlTypeResolver(xmlNamespaceResolver21, typeof(CustomCodingEditor).GetTypeInfo().Assembly));
			xamlServiceProvider21.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(454, 33)));
			object obj29 = markupExtension21.ProvideValue(xamlServiceProvider21);
			bindingExtension41.Converter = obj29;
			bindingExtension41.ConverterParameter = "1";
			bindingExtension41.Path = "ValueType";
			BindingBase bindingBase41 = bindingExtension41.ProvideValue(null);
			stackLayout19.SetBinding(VisualElement.IsVisibleProperty, bindingBase41);
			stackLayout19.SetValue(StackLayout.OrientationProperty, 0);
			label35.SetValue(Label.TextProperty, "Byte number (starting from 0): ");
			label35.SetValue(View.VerticalOptionsProperty, LayoutOptions.Center);
			stackLayout19.Children.Add(label35);
			bindingExtension42.Mode = 1;
			staticResourceExtension6.Key = "IntToStringConverter";
			IMarkupExtension markupExtension22 = staticResourceExtension6;
			XamlServiceProvider xamlServiceProvider22 = new XamlServiceProvider();
			Type typeFromHandle43 = typeof(IProvideValueTarget);
			object[] array25 = new object[0 + 8];
			array25[0] = bindingExtension42;
			array25[1] = entry29;
			array25[2] = stackLayout19;
			array25[3] = stackLayout21;
			array25[4] = sfExpander4;
			array25[5] = stackLayout22;
			array25[6] = scrollView;
			array25[7] = this;
			object obj30;
			xamlServiceProvider22.Add(typeFromHandle43, obj30 = new SimpleValueTargetProvider(array25, typeof(BindingExtension).GetRuntimeProperty("Converter"), nameScope));
			xamlServiceProvider22.Add(typeof(IReferenceProvider), obj30);
			Type typeFromHandle44 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver22 = new XmlNamespaceResolver();
			xmlNamespaceResolver22.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver22.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver22.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver22.Add("expander", "clr-namespace:Syncfusion.XForms.Expander;assembly=Syncfusion.Expander.XForms");
			xmlNamespaceResolver22.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver22.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver22.Add("settings", "clr-namespace:CarScannerXamarinForms.Settings");
			xamlServiceProvider22.Add(typeFromHandle44, new XamlTypeResolver(xmlNamespaceResolver22, typeof(CustomCodingEditor).GetTypeInfo().Assembly));
			xamlServiceProvider22.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(458, 58)));
			object obj31 = markupExtension22.ProvideValue(xamlServiceProvider22);
			bindingExtension42.Converter = obj31;
			bindingExtension42.Path = "StartByteId";
			BindingBase bindingBase42 = bindingExtension42.ProvideValue(null);
			entry29.SetBinding(Entry.TextProperty, bindingBase42);
			stackLayout19.Children.Add(entry29);
			label36.SetValue(Label.TextProperty, " DataLen: ");
			stackLayout19.Children.Add(label36);
			bindingExtension43.Mode = 1;
			staticResourceExtension7.Key = "IntToStringConverter";
			IMarkupExtension markupExtension23 = staticResourceExtension7;
			XamlServiceProvider xamlServiceProvider23 = new XamlServiceProvider();
			Type typeFromHandle45 = typeof(IProvideValueTarget);
			object[] array26 = new object[0 + 8];
			array26[0] = bindingExtension43;
			array26[1] = entry30;
			array26[2] = stackLayout19;
			array26[3] = stackLayout21;
			array26[4] = sfExpander4;
			array26[5] = stackLayout22;
			array26[6] = scrollView;
			array26[7] = this;
			object obj32;
			xamlServiceProvider23.Add(typeFromHandle45, obj32 = new SimpleValueTargetProvider(array26, typeof(BindingExtension).GetRuntimeProperty("Converter"), nameScope));
			xamlServiceProvider23.Add(typeof(IReferenceProvider), obj32);
			Type typeFromHandle46 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver23 = new XmlNamespaceResolver();
			xmlNamespaceResolver23.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver23.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver23.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver23.Add("expander", "clr-namespace:Syncfusion.XForms.Expander;assembly=Syncfusion.Expander.XForms");
			xmlNamespaceResolver23.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver23.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver23.Add("settings", "clr-namespace:CarScannerXamarinForms.Settings");
			xamlServiceProvider23.Add(typeFromHandle46, new XamlTypeResolver(xmlNamespaceResolver23, typeof(CustomCodingEditor).GetTypeInfo().Assembly));
			xamlServiceProvider23.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(460, 59)));
			object obj33 = markupExtension23.ProvideValue(xamlServiceProvider23);
			bindingExtension43.Converter = obj33;
			bindingExtension43.Path = "DataLength";
			BindingBase bindingBase43 = bindingExtension43.ProvideValue(null);
			entry30.SetBinding(Entry.TextProperty, bindingBase43);
			stackLayout19.Children.Add(entry30);
			label37.SetValue(Label.TextProperty, " Multi: ");
			label37.SetValue(View.VerticalOptionsProperty, LayoutOptions.Center);
			stackLayout19.Children.Add(label37);
			bindingExtension44.Mode = 1;
			staticResourceExtension8.Key = "DoubleToStringConverter";
			IMarkupExtension markupExtension24 = staticResourceExtension8;
			XamlServiceProvider xamlServiceProvider24 = new XamlServiceProvider();
			Type typeFromHandle47 = typeof(IProvideValueTarget);
			object[] array27 = new object[0 + 8];
			array27[0] = bindingExtension44;
			array27[1] = entry31;
			array27[2] = stackLayout19;
			array27[3] = stackLayout21;
			array27[4] = sfExpander4;
			array27[5] = stackLayout22;
			array27[6] = scrollView;
			array27[7] = this;
			object obj34;
			xamlServiceProvider24.Add(typeFromHandle47, obj34 = new SimpleValueTargetProvider(array27, typeof(BindingExtension).GetRuntimeProperty("Converter"), nameScope));
			xamlServiceProvider24.Add(typeof(IReferenceProvider), obj34);
			Type typeFromHandle48 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver24 = new XmlNamespaceResolver();
			xmlNamespaceResolver24.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver24.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver24.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver24.Add("expander", "clr-namespace:Syncfusion.XForms.Expander;assembly=Syncfusion.Expander.XForms");
			xmlNamespaceResolver24.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver24.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver24.Add("settings", "clr-namespace:CarScannerXamarinForms.Settings");
			xamlServiceProvider24.Add(typeFromHandle48, new XamlTypeResolver(xmlNamespaceResolver24, typeof(CustomCodingEditor).GetTypeInfo().Assembly));
			xamlServiceProvider24.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(465, 57)));
			object obj35 = markupExtension24.ProvideValue(xamlServiceProvider24);
			bindingExtension44.Converter = obj35;
			bindingExtension44.Path = "Multiplier";
			BindingBase bindingBase44 = bindingExtension44.ProvideValue(null);
			entry31.SetBinding(Entry.TextProperty, bindingBase44);
			stackLayout19.Children.Add(entry31);
			label38.SetValue(Label.TextProperty, " Offset: ");
			label38.SetValue(View.VerticalOptionsProperty, LayoutOptions.Center);
			stackLayout19.Children.Add(label38);
			bindingExtension45.Mode = 1;
			staticResourceExtension9.Key = "DoubleToStringConverter";
			IMarkupExtension markupExtension25 = staticResourceExtension9;
			XamlServiceProvider xamlServiceProvider25 = new XamlServiceProvider();
			Type typeFromHandle49 = typeof(IProvideValueTarget);
			object[] array28 = new object[0 + 8];
			array28[0] = bindingExtension45;
			array28[1] = entry32;
			array28[2] = stackLayout19;
			array28[3] = stackLayout21;
			array28[4] = sfExpander4;
			array28[5] = stackLayout22;
			array28[6] = scrollView;
			array28[7] = this;
			object obj36;
			xamlServiceProvider25.Add(typeFromHandle49, obj36 = new SimpleValueTargetProvider(array28, typeof(BindingExtension).GetRuntimeProperty("Converter"), nameScope));
			xamlServiceProvider25.Add(typeof(IReferenceProvider), obj36);
			Type typeFromHandle50 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver25 = new XmlNamespaceResolver();
			xmlNamespaceResolver25.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver25.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver25.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver25.Add("expander", "clr-namespace:Syncfusion.XForms.Expander;assembly=Syncfusion.Expander.XForms");
			xmlNamespaceResolver25.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver25.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver25.Add("settings", "clr-namespace:CarScannerXamarinForms.Settings");
			xamlServiceProvider25.Add(typeFromHandle50, new XamlTypeResolver(xmlNamespaceResolver25, typeof(CustomCodingEditor).GetTypeInfo().Assembly));
			xamlServiceProvider25.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(467, 58)));
			object obj37 = markupExtension25.ProvideValue(xamlServiceProvider25);
			bindingExtension45.Converter = obj37;
			bindingExtension45.Path = "Offset";
			BindingBase bindingBase45 = bindingExtension45.ProvideValue(null);
			entry32.SetBinding(Entry.TextProperty, bindingBase45);
			stackLayout19.Children.Add(entry32);
			bindingExtension46.Mode = 1;
			bindingExtension46.Path = "IsSigned";
			BindingBase bindingBase46 = bindingExtension46.ProvideValue(null);
			labelSwitch3.SetBinding(LabelSwitch.IsToggledProperty, bindingBase46);
			labelSwitch3.SetValue(LabelSwitch.TextProperty, "Signed");
			stackLayout19.Children.Add(labelSwitch3);
			bindingExtension47.Mode = 1;
			bindingExtension47.Path = "ReversedByteSet";
			BindingBase bindingBase47 = bindingExtension47.ProvideValue(null);
			labelSwitch4.SetBinding(LabelSwitch.IsToggledProperty, bindingBase47);
			labelSwitch4.SetValue(LabelSwitch.TextProperty, "Reversed byte order");
			stackLayout19.Children.Add(labelSwitch4);
			stackLayout21.Children.Add(stackLayout19);
			stackLayout20.SetValue(Grid.ColumnProperty, 0);
			stackLayout20.SetValue(View.MarginProperty, new Thickness(2.0));
			bindingExtension48.Mode = 2;
			staticResourceExtension10.Key = "EnumValueToTrueConverter";
			IMarkupExtension markupExtension26 = staticResourceExtension10;
			XamlServiceProvider xamlServiceProvider26 = new XamlServiceProvider();
			Type typeFromHandle51 = typeof(IProvideValueTarget);
			object[] array29 = new object[0 + 7];
			array29[0] = bindingExtension48;
			array29[1] = stackLayout20;
			array29[2] = stackLayout21;
			array29[3] = sfExpander4;
			array29[4] = stackLayout22;
			array29[5] = scrollView;
			array29[6] = this;
			object obj38;
			xamlServiceProvider26.Add(typeFromHandle51, obj38 = new SimpleValueTargetProvider(array29, typeof(BindingExtension).GetRuntimeProperty("Converter"), nameScope));
			xamlServiceProvider26.Add(typeof(IReferenceProvider), obj38);
			Type typeFromHandle52 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver26 = new XmlNamespaceResolver();
			xmlNamespaceResolver26.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver26.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver26.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver26.Add("expander", "clr-namespace:Syncfusion.XForms.Expander;assembly=Syncfusion.Expander.XForms");
			xmlNamespaceResolver26.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver26.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver26.Add("settings", "clr-namespace:CarScannerXamarinForms.Settings");
			xamlServiceProvider26.Add(typeFromHandle52, new XamlTypeResolver(xmlNamespaceResolver26, typeof(CustomCodingEditor).GetTypeInfo().Assembly));
			xamlServiceProvider26.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(485, 33)));
			object obj39 = markupExtension26.ProvideValue(xamlServiceProvider26);
			bindingExtension48.Converter = obj39;
			bindingExtension48.ConverterParameter = "11";
			bindingExtension48.Path = "ValueType";
			BindingBase bindingBase48 = bindingExtension48.ProvideValue(null);
			stackLayout20.SetBinding(VisualElement.IsVisibleProperty, bindingBase48);
			stackLayout20.SetValue(StackLayout.OrientationProperty, 0);
			label39.SetValue(Label.TextProperty, "Byte number (starting from 0): ");
			label39.SetValue(View.VerticalOptionsProperty, LayoutOptions.Center);
			stackLayout20.Children.Add(label39);
			bindingExtension49.Mode = 1;
			staticResourceExtension11.Key = "IntToStringConverter";
			IMarkupExtension markupExtension27 = staticResourceExtension11;
			XamlServiceProvider xamlServiceProvider27 = new XamlServiceProvider();
			Type typeFromHandle53 = typeof(IProvideValueTarget);
			object[] array30 = new object[0 + 8];
			array30[0] = bindingExtension49;
			array30[1] = entry33;
			array30[2] = stackLayout20;
			array30[3] = stackLayout21;
			array30[4] = sfExpander4;
			array30[5] = stackLayout22;
			array30[6] = scrollView;
			array30[7] = this;
			object obj40;
			xamlServiceProvider27.Add(typeFromHandle53, obj40 = new SimpleValueTargetProvider(array30, typeof(BindingExtension).GetRuntimeProperty("Converter"), nameScope));
			xamlServiceProvider27.Add(typeof(IReferenceProvider), obj40);
			Type typeFromHandle54 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver27 = new XmlNamespaceResolver();
			xmlNamespaceResolver27.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver27.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver27.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver27.Add("expander", "clr-namespace:Syncfusion.XForms.Expander;assembly=Syncfusion.Expander.XForms");
			xmlNamespaceResolver27.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver27.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver27.Add("settings", "clr-namespace:CarScannerXamarinForms.Settings");
			xamlServiceProvider27.Add(typeFromHandle54, new XamlTypeResolver(xmlNamespaceResolver27, typeof(CustomCodingEditor).GetTypeInfo().Assembly));
			xamlServiceProvider27.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(489, 63)));
			object obj41 = markupExtension27.ProvideValue(xamlServiceProvider27);
			bindingExtension49.Converter = obj41;
			bindingExtension49.Path = "StartByteId";
			BindingBase bindingBase49 = bindingExtension49.ProvideValue(null);
			entry33.SetBinding(Entry.TextProperty, bindingBase49);
			stackLayout20.Children.Add(entry33);
			stackLayout21.Children.Add(stackLayout20);
			sfExpander4.SetValue(SfExpander.ContentProperty, stackLayout21);
			stackLayout22.Children.Add(sfExpander4);
			scrollView.Content = stackLayout22;
			this.SetValue(ContentPage.ContentProperty, scrollView);
		}

		// Token: 0x06001A30 RID: 6704 RVA: 0x001209FC File Offset: 0x0011EBFC
		private void __InitComponentRuntime()
		{
			Extensions.LoadFromXaml<CustomCodingEditor>(this, typeof(CustomCodingEditor));
			this.pickerGroup = NameScopeExtensions.FindByName<Picker>(this, "pickerGroup");
			this.gridTranslations = NameScopeExtensions.FindByName<Grid>(this, "gridTranslations");
			this.lvTranslations = NameScopeExtensions.FindByName<ListView>(this, "lvTranslations");
			this.btnAddTranslation = NameScopeExtensions.FindByName<Button>(this, "btnAddTranslation");
			this.btnDelTranslation = NameScopeExtensions.FindByName<Button>(this, "btnDelTranslation");
			this.btnGenRequestHeaderGeneric = NameScopeExtensions.FindByName<Button>(this, "btnGenRequestHeaderGeneric");
			this.btnGenRequestHeaderVag = NameScopeExtensions.FindByName<Button>(this, "btnGenRequestHeaderVag");
			this.btnGenRequestHeaderRenault = NameScopeExtensions.FindByName<Button>(this, "btnGenRequestHeaderRenault");
			this.btnReadAddressToWriteAddress = NameScopeExtensions.FindByName<Button>(this, "btnReadAddressToWriteAddress");
			this.tbATST = NameScopeExtensions.FindByName<Entry>(this, "tbATST");
			this.tbProtocol = NameScopeExtensions.FindByName<Entry>(this, "tbProtocol");
			this.pickerType = NameScopeExtensions.FindByName<Picker>(this, "pickerType");
			this.gridOptions = NameScopeExtensions.FindByName<Grid>(this, "gridOptions");
			this.tbByteId2 = NameScopeExtensions.FindByName<Entry>(this, "tbByteId2");
			this.lvOptions = NameScopeExtensions.FindByName<ListView>(this, "lvOptions");
			this.btnAddOption = NameScopeExtensions.FindByName<Button>(this, "btnAddOption");
			this.btnDelOption = NameScopeExtensions.FindByName<Button>(this, "btnDelOption");
			this.btnUpOption = NameScopeExtensions.FindByName<Button>(this, "btnUpOption");
			this.btnDownOption = NameScopeExtensions.FindByName<Button>(this, "btnDownOption");
			this.gridOptionTranslations = NameScopeExtensions.FindByName<Grid>(this, "gridOptionTranslations");
			this.lvOptionTranslations = NameScopeExtensions.FindByName<ListView>(this, "lvOptionTranslations");
			this.btnAddOptionTranslation = NameScopeExtensions.FindByName<Button>(this, "btnAddOptionTranslation");
			this.btnDelOptionTranslation = NameScopeExtensions.FindByName<Button>(this, "btnDelOptionTranslation");
			this.InputValueTypePanel = NameScopeExtensions.FindByName<StackLayout>(this, "InputValueTypePanel");
			this.tbByteId = NameScopeExtensions.FindByName<Entry>(this, "tbByteId");
			this.btDataLen = NameScopeExtensions.FindByName<Entry>(this, "btDataLen");
			this.tbMulti = NameScopeExtensions.FindByName<Entry>(this, "tbMulti");
			this.tbOffset = NameScopeExtensions.FindByName<Entry>(this, "tbOffset");
			this.cbSigned = NameScopeExtensions.FindByName<LabelSwitch>(this, "cbSigned");
			this.cbReversed = NameScopeExtensions.FindByName<LabelSwitch>(this, "cbReversed");
			this.InputFloatPanel = NameScopeExtensions.FindByName<StackLayout>(this, "InputFloatPanel");
			this.tbByteIdFloat = NameScopeExtensions.FindByName<Entry>(this, "tbByteIdFloat");
		}

		// Token: 0x04000B6C RID: 2924
		private CustomCodingsListViewModel CodingsList;

		// Token: 0x04000B6D RID: 2925
		[CompilerGenerated]
		private CustomizableCodingTemplate <Coding>k__BackingField;

		// Token: 0x04000B6E RID: 2926
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private Picker pickerGroup;

		// Token: 0x04000B6F RID: 2927
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private Grid gridTranslations;

		// Token: 0x04000B70 RID: 2928
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private ListView lvTranslations;

		// Token: 0x04000B71 RID: 2929
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private Button btnAddTranslation;

		// Token: 0x04000B72 RID: 2930
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private Button btnDelTranslation;

		// Token: 0x04000B73 RID: 2931
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private Button btnGenRequestHeaderGeneric;

		// Token: 0x04000B74 RID: 2932
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private Button btnGenRequestHeaderVag;

		// Token: 0x04000B75 RID: 2933
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private Button btnGenRequestHeaderRenault;

		// Token: 0x04000B76 RID: 2934
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private Button btnReadAddressToWriteAddress;

		// Token: 0x04000B77 RID: 2935
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private Entry tbATST;

		// Token: 0x04000B78 RID: 2936
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private Entry tbProtocol;

		// Token: 0x04000B79 RID: 2937
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private Picker pickerType;

		// Token: 0x04000B7A RID: 2938
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private Grid gridOptions;

		// Token: 0x04000B7B RID: 2939
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private Entry tbByteId2;

		// Token: 0x04000B7C RID: 2940
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private ListView lvOptions;

		// Token: 0x04000B7D RID: 2941
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private Button btnAddOption;

		// Token: 0x04000B7E RID: 2942
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private Button btnDelOption;

		// Token: 0x04000B7F RID: 2943
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private Button btnUpOption;

		// Token: 0x04000B80 RID: 2944
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private Button btnDownOption;

		// Token: 0x04000B81 RID: 2945
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private Grid gridOptionTranslations;

		// Token: 0x04000B82 RID: 2946
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private ListView lvOptionTranslations;

		// Token: 0x04000B83 RID: 2947
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private Button btnAddOptionTranslation;

		// Token: 0x04000B84 RID: 2948
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private Button btnDelOptionTranslation;

		// Token: 0x04000B85 RID: 2949
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private StackLayout InputValueTypePanel;

		// Token: 0x04000B86 RID: 2950
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private Entry tbByteId;

		// Token: 0x04000B87 RID: 2951
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private Entry btDataLen;

		// Token: 0x04000B88 RID: 2952
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private Entry tbMulti;

		// Token: 0x04000B89 RID: 2953
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private Entry tbOffset;

		// Token: 0x04000B8A RID: 2954
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private LabelSwitch cbSigned;

		// Token: 0x04000B8B RID: 2955
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private LabelSwitch cbReversed;

		// Token: 0x04000B8C RID: 2956
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private StackLayout InputFloatPanel;

		// Token: 0x04000B8D RID: 2957
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private Entry tbByteIdFloat;

		// Token: 0x020001FA RID: 506
		[CompilerGenerated]
		[Serializable]
		private sealed class <>c
		{
			// Token: 0x06001A31 RID: 6705 RVA: 0x00120C3A File Offset: 0x0011EE3A
			// Note: this type is marked as 'beforefieldinit'.
			static <>c()
			{
			}

			// Token: 0x06001A32 RID: 6706 RVA: 0x00002050 File Offset: 0x00000250
			public <>c()
			{
			}

			// Token: 0x06001A33 RID: 6707 RVA: 0x00120C48 File Offset: 0x0011EE48
			internal string <.ctor>b__0_0(Enum x)
			{
				return Translate.GetString("coding_Group_" + ((CodingGroup)x).ToString());
			}

			// Token: 0x06001A34 RID: 6708 RVA: 0x00120C78 File Offset: 0x0011EE78
			internal bool <btnAddOptionTranslation_Click>b__15_0(TranslationItem x)
			{
				return x.Language == "de";
			}

			// Token: 0x04000B8E RID: 2958
			public static readonly CustomCodingEditor.<>c <>9 = new CustomCodingEditor.<>c();

			// Token: 0x04000B8F RID: 2959
			public static Func<Enum, string> <>9__0_0;

			// Token: 0x04000B90 RID: 2960
			public static Func<TranslationItem, bool> <>9__15_0;
		}

		// Token: 0x020001FB RID: 507
		[CompilerGenerated]
		private sealed class <InitializeComponent>_anonXamlCDataTemplate_82
		{
			// Token: 0x06001A35 RID: 6709 RVA: 0x00120C8C File Offset: 0x0011EE8C
			public <InitializeComponent>_anonXamlCDataTemplate_82()
			{
			}

			// Token: 0x06001A36 RID: 6710 RVA: 0x00120CA0 File Offset: 0x0011EEA0
			internal object LoadDataTemplate()
			{
				BindingExtension bindingExtension;
				VisualDiagnostics.RegisterSourceInfo(bindingExtension = new BindingExtension(), new Uri("Settings\\CustomCodingEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 95, 55);
				TextCell textCell;
				VisualDiagnostics.RegisterSourceInfo(textCell = new TextCell(), new Uri("Settings\\CustomCodingEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 95, 46);
				NameScope nameScope = new NameScope();
				NameScope.SetNameScope(textCell, nameScope);
				bindingExtension.Mode = 2;
				bindingExtension.Path = "Language";
				BindingBase bindingBase = bindingExtension.ProvideValue(null);
				textCell.SetBinding(TextCell.TextProperty, bindingBase);
				return textCell;
			}

			// Token: 0x04000B91 RID: 2961
			internal object[] parentValues;

			// Token: 0x04000B92 RID: 2962
			internal CustomCodingEditor root;
		}

		// Token: 0x020001FC RID: 508
		[CompilerGenerated]
		private sealed class <InitializeComponent>_anonXamlCDataTemplate_83
		{
			// Token: 0x06001A37 RID: 6711 RVA: 0x00120D38 File Offset: 0x0011EF38
			public <InitializeComponent>_anonXamlCDataTemplate_83()
			{
			}

			// Token: 0x06001A38 RID: 6712 RVA: 0x00120D4C File Offset: 0x0011EF4C
			internal object LoadDataTemplate()
			{
				BindingExtension bindingExtension;
				VisualDiagnostics.RegisterSourceInfo(bindingExtension = new BindingExtension(), new Uri("Settings\\CustomCodingEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 337, 59);
				BindingExtension bindingExtension2;
				VisualDiagnostics.RegisterSourceInfo(bindingExtension2 = new BindingExtension(), new Uri("Settings\\CustomCodingEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 337, 97);
				TextCell textCell;
				VisualDiagnostics.RegisterSourceInfo(textCell = new TextCell(), new Uri("Settings\\CustomCodingEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 337, 50);
				NameScope nameScope = new NameScope();
				NameScope.SetNameScope(textCell, nameScope);
				bindingExtension.Mode = 2;
				bindingExtension.Path = "Value";
				BindingBase bindingBase = bindingExtension.ProvideValue(null);
				textCell.SetBinding(TextCell.DetailProperty, bindingBase);
				bindingExtension2.Mode = 2;
				bindingExtension2.Path = "TitleRaw";
				BindingBase bindingBase2 = bindingExtension2.ProvideValue(null);
				textCell.SetBinding(TextCell.TextProperty, bindingBase2);
				return textCell;
			}

			// Token: 0x04000B93 RID: 2963
			internal object[] parentValues;

			// Token: 0x04000B94 RID: 2964
			internal CustomCodingEditor root;
		}

		// Token: 0x020001FD RID: 509
		[CompilerGenerated]
		private sealed class <InitializeComponent>_anonXamlCDataTemplate_84
		{
			// Token: 0x06001A39 RID: 6713 RVA: 0x00120E40 File Offset: 0x0011F040
			public <InitializeComponent>_anonXamlCDataTemplate_84()
			{
			}

			// Token: 0x06001A3A RID: 6714 RVA: 0x00120E54 File Offset: 0x0011F054
			internal object LoadDataTemplate()
			{
				BindingExtension bindingExtension;
				VisualDiagnostics.RegisterSourceInfo(bindingExtension = new BindingExtension(), new Uri("Settings\\CustomCodingEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 410, 67);
				TextCell textCell;
				VisualDiagnostics.RegisterSourceInfo(textCell = new TextCell(), new Uri("Settings\\CustomCodingEditor.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 410, 58);
				NameScope nameScope = new NameScope();
				NameScope.SetNameScope(textCell, nameScope);
				bindingExtension.Path = "Language";
				BindingBase bindingBase = bindingExtension.ProvideValue(null);
				textCell.SetBinding(TextCell.TextProperty, bindingBase);
				return textCell;
			}

			// Token: 0x04000B95 RID: 2965
			internal object[] parentValues;

			// Token: 0x04000B96 RID: 2966
			internal CustomCodingEditor root;
		}
	}
}
