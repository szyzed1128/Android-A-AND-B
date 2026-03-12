using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Reflection;
using CarScannerXamarinForms.Common.XAMLConverters;
using CarScannerXamarinForms.OBD2.PIDS;
using CarScannerXamarinForms.OBD2.PIDS.CalculatedPIDs;
using Xamarin.Forms;
using Xamarin.Forms.Internals;
using Xamarin.Forms.Xaml;
using Xamarin.Forms.Xaml.Diagnostics;
using Xamarin.Forms.Xaml.Internals;

namespace CarScannerXamarinForms.Settings
{
	// Token: 0x02000289 RID: 649
	[XamlCompilation(2)]
	[XamlFilePath("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Settings\\SettingsPIDOverridePage.xaml")]
	public class SettingsPIDOverridePage : ContentPage
	{
		// Token: 0x06001EF2 RID: 7922 RVA: 0x00151D74 File Offset: 0x0014FF74
		public SettingsPIDOverridePage(IPID pid)
		{
			this.InitializeComponent();
			Array values = Enum.GetValues(typeof(Roles));
			this.pickerRole.ItemsSource = values;
			if (pid is CalculatedPIDV2)
			{
				this.panelNotForCalculated.IsVisible = false;
			}
			if (pid is IPIDFloatValue)
			{
				UnitsHelper.Units[] possibleConversionsForUnit = UnitsHelper.GetPossibleConversionsForUnit(((IPIDFloatValue)pid).Units);
				if (possibleConversionsForUnit.Length > 1)
				{
					this.panelUnits.IsVisible = true;
					List<UnitsHelper.Units> list = new List<UnitsHelper.Units>();
					list.Add(UnitsHelper.Units.None);
					list.AddRange(possibleConversionsForUnit);
					this.pickerUnits.ItemsSource = list;
				}
				else
				{
					this.panelUnits.IsVisible = false;
				}
			}
			else
			{
				this.panelUnits.IsVisible = false;
			}
			base.BindingContext = pid;
		}

		// Token: 0x06001EF3 RID: 7923 RVA: 0x00151E2B File Offset: 0x0015002B
		private void SettingsPIDOverridePage_Disappearing(object sender, EventArgs e)
		{
			PIDOverrideDictionary.Instance.Save();
		}

		// Token: 0x06001EF4 RID: 7924 RVA: 0x00151E38 File Offset: 0x00150038
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private void InitializeComponent()
		{
			if (ResourceLoader.IsEnabled && ResourceLoader.CanProvideContentFor(new ResourceLoader.ResourceLoadingQuery
			{
				AssemblyName = typeof(SettingsPIDOverridePage).GetTypeInfo().Assembly.GetName(),
				ResourcePath = "Settings/SettingsPIDOverridePage.xaml",
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
			BindingExtension bindingExtension;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension = new BindingExtension(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Settings\\SettingsPIDOverridePage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 9, 5);
			SkipCyclesIntToPriorityStringConverter skipCyclesIntToPriorityStringConverter;
			VisualDiagnostics.RegisterSourceInfo(skipCyclesIntToPriorityStringConverter = new SkipCyclesIntToPriorityStringConverter(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Settings\\SettingsPIDOverridePage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 12, 10);
			UnitsToStringInvariantConverter unitsToStringInvariantConverter;
			VisualDiagnostics.RegisterSourceInfo(unitsToStringInvariantConverter = new UnitsToStringInvariantConverter(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Settings\\SettingsPIDOverridePage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 13, 10);
			UnitsToStringConverter unitsToStringConverter;
			VisualDiagnostics.RegisterSourceInfo(unitsToStringConverter = new UnitsToStringConverter(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Settings\\SettingsPIDOverridePage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 14, 10);
			BindingExtension bindingExtension2;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension2 = new BindingExtension(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Settings\\SettingsPIDOverridePage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 19, 46);
			Label label;
			VisualDiagnostics.RegisterSourceInfo(label = new Label(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Settings\\SettingsPIDOverridePage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 19, 18);
			Translate translate;
			VisualDiagnostics.RegisterSourceInfo(translate = new Translate(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Settings\\SettingsPIDOverridePage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 20, 24);
			Label label2;
			VisualDiagnostics.RegisterSourceInfo(label2 = new Label(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Settings\\SettingsPIDOverridePage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 20, 18);
			BindingExtension bindingExtension3;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension3 = new BindingExtension(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Settings\\SettingsPIDOverridePage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 21, 24);
			Entry entry;
			VisualDiagnostics.RegisterSourceInfo(entry = new Entry(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Settings\\SettingsPIDOverridePage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 21, 18);
			Translate translate2;
			VisualDiagnostics.RegisterSourceInfo(translate2 = new Translate(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Settings\\SettingsPIDOverridePage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 22, 24);
			Label label3;
			VisualDiagnostics.RegisterSourceInfo(label3 = new Label(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Settings\\SettingsPIDOverridePage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 22, 18);
			BindingExtension bindingExtension4;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension4 = new BindingExtension(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Settings\\SettingsPIDOverridePage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 23, 24);
			Entry entry2;
			VisualDiagnostics.RegisterSourceInfo(entry2 = new Entry(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Settings\\SettingsPIDOverridePage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 23, 18);
			Translate translate3;
			VisualDiagnostics.RegisterSourceInfo(translate3 = new Translate(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Settings\\SettingsPIDOverridePage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 29, 39);
			Span span;
			VisualDiagnostics.RegisterSourceInfo(span = new Span(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Settings\\SettingsPIDOverridePage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 29, 34);
			Span span2;
			VisualDiagnostics.RegisterSourceInfo(span2 = new Span(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Settings\\SettingsPIDOverridePage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 30, 34);
			StaticResourceExtension staticResourceExtension;
			VisualDiagnostics.RegisterSourceInfo(staticResourceExtension = new StaticResourceExtension(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Settings\\SettingsPIDOverridePage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 31, 61);
			BindingExtension bindingExtension5;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension5 = new BindingExtension(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Settings\\SettingsPIDOverridePage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 31, 61);
			Span span3;
			VisualDiagnostics.RegisterSourceInfo(span3 = new Span(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Settings\\SettingsPIDOverridePage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 31, 34);
			FormattedString formattedString;
			VisualDiagnostics.RegisterSourceInfo(formattedString = new FormattedString(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Settings\\SettingsPIDOverridePage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 28, 30);
			Label label4;
			VisualDiagnostics.RegisterSourceInfo(label4 = new Label(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Settings\\SettingsPIDOverridePage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 26, 22);
			Translate translate4;
			VisualDiagnostics.RegisterSourceInfo(translate4 = new Translate(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Settings\\SettingsPIDOverridePage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 35, 28);
			Label label5;
			VisualDiagnostics.RegisterSourceInfo(label5 = new Label(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Settings\\SettingsPIDOverridePage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 35, 22);
			StaticResourceExtension staticResourceExtension2;
			VisualDiagnostics.RegisterSourceInfo(staticResourceExtension2 = new StaticResourceExtension(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Settings\\SettingsPIDOverridePage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 36, 29);
			BindingExtension bindingExtension6;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension6 = new BindingExtension(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Settings\\SettingsPIDOverridePage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 36, 29);
			Type typeFromHandle;
			VisualDiagnostics.RegisterSourceInfo(typeFromHandle = typeof(string), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Settings\\SettingsPIDOverridePage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 38, 38);
			string text = "1/1";
			string text2 = "1/2";
			string text3 = "1/3";
			string text4 = "1/4";
			string text5 = "1/5";
			string text6 = "1/6";
			string text7 = "1/7";
			string text8 = "1/8";
			string text9 = "1/9";
			string text10 = "1/10";
			string text11 = "1/15";
			string text12 = "1/20";
			string text13 = "1/25";
			string text14 = "1/50";
			string text15 = "1/75";
			string text16 = "1/100";
			string text17 = "1/150";
			string text18 = "1/200";
			string text19 = "1/250";
			string text20 = "1/300";
			string text21 = "1/400";
			string text22 = "1/500";
			string text23 = "1/750";
			string text24 = "1/1000";
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
			string[] array;
			VisualDiagnostics.RegisterSourceInfo(array = new string[]
			{
				text, text2, text3, text4, text5, text6, text7, text8, text9, text10,
				text11, text12, text13, text14, text15, text16, text17, text18, text19, text20,
				text21, text22, text23, text24
			}, new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Settings\\SettingsPIDOverridePage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 38, 30);
			Picker picker;
			VisualDiagnostics.RegisterSourceInfo(picker = new Picker(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Settings\\SettingsPIDOverridePage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 36, 22);
			Translate translate5;
			VisualDiagnostics.RegisterSourceInfo(translate5 = new Translate(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Settings\\SettingsPIDOverridePage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 70, 39);
			Span span4;
			VisualDiagnostics.RegisterSourceInfo(span4 = new Span(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Settings\\SettingsPIDOverridePage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 70, 34);
			Span span5;
			VisualDiagnostics.RegisterSourceInfo(span5 = new Span(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Settings\\SettingsPIDOverridePage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 71, 34);
			BindingExtension bindingExtension7;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension7 = new BindingExtension(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Settings\\SettingsPIDOverridePage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 72, 61);
			Span span6;
			VisualDiagnostics.RegisterSourceInfo(span6 = new Span(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Settings\\SettingsPIDOverridePage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 72, 34);
			FormattedString formattedString2;
			VisualDiagnostics.RegisterSourceInfo(formattedString2 = new FormattedString(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Settings\\SettingsPIDOverridePage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 69, 30);
			Label label6;
			VisualDiagnostics.RegisterSourceInfo(label6 = new Label(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Settings\\SettingsPIDOverridePage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 67, 22);
			Translate translate6;
			VisualDiagnostics.RegisterSourceInfo(translate6 = new Translate(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Settings\\SettingsPIDOverridePage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 76, 28);
			Label label7;
			VisualDiagnostics.RegisterSourceInfo(label7 = new Label(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Settings\\SettingsPIDOverridePage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 76, 22);
			BindingExtension bindingExtension8;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension8 = new BindingExtension(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Settings\\SettingsPIDOverridePage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 77, 49);
			Picker picker2;
			VisualDiagnostics.RegisterSourceInfo(picker2 = new Picker(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Settings\\SettingsPIDOverridePage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 77, 22);
			StackLayout stackLayout;
			VisualDiagnostics.RegisterSourceInfo(stackLayout = new StackLayout(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Settings\\SettingsPIDOverridePage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 25, 18);
			Translate translate7;
			VisualDiagnostics.RegisterSourceInfo(translate7 = new Translate(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Settings\\SettingsPIDOverridePage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 89, 39);
			Span span7;
			VisualDiagnostics.RegisterSourceInfo(span7 = new Span(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Settings\\SettingsPIDOverridePage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 89, 34);
			Span span8;
			VisualDiagnostics.RegisterSourceInfo(span8 = new Span(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Settings\\SettingsPIDOverridePage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 90, 34);
			StaticResourceExtension staticResourceExtension3;
			VisualDiagnostics.RegisterSourceInfo(staticResourceExtension3 = new StaticResourceExtension(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Settings\\SettingsPIDOverridePage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 91, 61);
			BindingExtension bindingExtension9;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension9 = new BindingExtension(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Settings\\SettingsPIDOverridePage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 91, 61);
			Span span9;
			VisualDiagnostics.RegisterSourceInfo(span9 = new Span(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Settings\\SettingsPIDOverridePage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 91, 34);
			FormattedString formattedString3;
			VisualDiagnostics.RegisterSourceInfo(formattedString3 = new FormattedString(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Settings\\SettingsPIDOverridePage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 88, 30);
			Label label8;
			VisualDiagnostics.RegisterSourceInfo(label8 = new Label(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Settings\\SettingsPIDOverridePage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 86, 22);
			Translate translate8;
			VisualDiagnostics.RegisterSourceInfo(translate8 = new Translate(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Settings\\SettingsPIDOverridePage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 95, 48);
			Label label9;
			VisualDiagnostics.RegisterSourceInfo(label9 = new Label(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Settings\\SettingsPIDOverridePage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 95, 22);
			StaticResourceExtension staticResourceExtension4;
			VisualDiagnostics.RegisterSourceInfo(staticResourceExtension4 = new StaticResourceExtension(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Settings\\SettingsPIDOverridePage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 98, 25);
			BindingExtension bindingExtension10;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension10 = new BindingExtension(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Settings\\SettingsPIDOverridePage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 98, 25);
			BindingExtension bindingExtension11;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension11 = new BindingExtension(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Settings\\SettingsPIDOverridePage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 99, 25);
			Picker picker3;
			VisualDiagnostics.RegisterSourceInfo(picker3 = new Picker(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Settings\\SettingsPIDOverridePage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 96, 22);
			StackLayout stackLayout2;
			VisualDiagnostics.RegisterSourceInfo(stackLayout2 = new StackLayout(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Settings\\SettingsPIDOverridePage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 82, 18);
			StackLayout stackLayout3;
			VisualDiagnostics.RegisterSourceInfo(stackLayout3 = new StackLayout(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Settings\\SettingsPIDOverridePage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 18, 14);
			ScrollView scrollView;
			VisualDiagnostics.RegisterSourceInfo(scrollView = new ScrollView(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Settings\\SettingsPIDOverridePage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 17, 10);
			VisualDiagnostics.RegisterSourceInfo(this, new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Settings\\SettingsPIDOverridePage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 2, 2);
			NameScope nameScope = NameScope.GetNameScope(this) ?? new NameScope();
			NameScope.SetNameScope(this, nameScope);
			nameScope.RegisterName("panelNotForCalculated", stackLayout);
			if (stackLayout.StyleId == null)
			{
				stackLayout.StyleId = "panelNotForCalculated";
			}
			nameScope.RegisterName("pickerRole", picker2);
			if (picker2.StyleId == null)
			{
				picker2.StyleId = "pickerRole";
			}
			nameScope.RegisterName("panelUnits", stackLayout2);
			if (stackLayout2.StyleId == null)
			{
				stackLayout2.StyleId = "panelUnits";
			}
			nameScope.RegisterName("labelUnits", label9);
			if (label9.StyleId == null)
			{
				label9.StyleId = "labelUnits";
			}
			nameScope.RegisterName("pickerUnits", picker3);
			if (picker3.StyleId == null)
			{
				picker3.StyleId = "pickerUnits";
			}
			this.panelNotForCalculated = stackLayout;
			this.pickerRole = picker2;
			this.panelUnits = stackLayout2;
			this.labelUnits = label9;
			this.pickerUnits = picker3;
			bindingExtension.Path = "OriginalShortName";
			BindingBase bindingBase = bindingExtension.ProvideValue(null);
			this.SetBinding(Page.TitleProperty, bindingBase);
			this.Disappearing += this.SettingsPIDOverridePage_Disappearing;
			this.Resources.Add("SkipCyclesIntToPriorityStringConverter", skipCyclesIntToPriorityStringConverter);
			this.Resources.Add("UnitsToStringInvariantConverter", unitsToStringInvariantConverter);
			this.Resources.Add("UnitsToStringConverter", unitsToStringConverter);
			scrollView.SetValue(Layout.PaddingProperty, new Thickness(5.0, 0.0, 5.0, 0.0));
			scrollView.SetValue(ScrollView.OrientationProperty, 0);
			label.SetValue(Label.FontAttributesProperty, new FontAttributesConverter().ConvertFromInvariantString("Bold"));
			bindingExtension2.Path = "OriginalName";
			BindingBase bindingBase2 = bindingExtension2.ProvideValue(null);
			label.SetBinding(Label.TextProperty, bindingBase2);
			stackLayout3.Children.Add(label);
			translate.Text = "pid_override_CustomName";
			IMarkupExtension markupExtension = translate;
			XamlServiceProvider xamlServiceProvider = new XamlServiceProvider();
			Type typeFromHandle2 = typeof(IProvideValueTarget);
			object[] array2 = new object[0 + 4];
			array2[0] = label2;
			array2[1] = stackLayout3;
			array2[2] = scrollView;
			array2[3] = this;
			object obj;
			xamlServiceProvider.Add(typeFromHandle2, obj = new SimpleValueTargetProvider(array2, Label.TextProperty, nameScope));
			xamlServiceProvider.Add(typeof(IReferenceProvider), obj);
			Type typeFromHandle3 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver = new XmlNamespaceResolver();
			xmlNamespaceResolver.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver.Add("syncfusionTB", "clr-namespace:Syncfusion.SfNumericTextBox.XForms;assembly=Syncfusion.SfNumericTextBox.XForms");
			xamlServiceProvider.Add(typeFromHandle3, new XamlTypeResolver(xmlNamespaceResolver, typeof(SettingsPIDOverridePage).GetTypeInfo().Assembly));
			xamlServiceProvider.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(20, 24)));
			object obj2 = markupExtension.ProvideValue(xamlServiceProvider);
			label2.Text = obj2;
			stackLayout3.Children.Add(label2);
			bindingExtension3.Mode = 1;
			bindingExtension3.Path = "CustomName";
			BindingBase bindingBase3 = bindingExtension3.ProvideValue(null);
			entry.SetBinding(Entry.TextProperty, bindingBase3);
			stackLayout3.Children.Add(entry);
			translate2.Text = "pid_override_CustomShortName";
			IMarkupExtension markupExtension2 = translate2;
			XamlServiceProvider xamlServiceProvider2 = new XamlServiceProvider();
			Type typeFromHandle4 = typeof(IProvideValueTarget);
			object[] array3 = new object[0 + 4];
			array3[0] = label3;
			array3[1] = stackLayout3;
			array3[2] = scrollView;
			array3[3] = this;
			object obj3;
			xamlServiceProvider2.Add(typeFromHandle4, obj3 = new SimpleValueTargetProvider(array3, Label.TextProperty, nameScope));
			xamlServiceProvider2.Add(typeof(IReferenceProvider), obj3);
			Type typeFromHandle5 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver2 = new XmlNamespaceResolver();
			xmlNamespaceResolver2.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver2.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver2.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver2.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver2.Add("syncfusionTB", "clr-namespace:Syncfusion.SfNumericTextBox.XForms;assembly=Syncfusion.SfNumericTextBox.XForms");
			xamlServiceProvider2.Add(typeFromHandle5, new XamlTypeResolver(xmlNamespaceResolver2, typeof(SettingsPIDOverridePage).GetTypeInfo().Assembly));
			xamlServiceProvider2.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(22, 24)));
			object obj4 = markupExtension2.ProvideValue(xamlServiceProvider2);
			label3.Text = obj4;
			stackLayout3.Children.Add(label3);
			bindingExtension4.Mode = 1;
			bindingExtension4.Path = "CustomShortName";
			BindingBase bindingBase4 = bindingExtension4.ProvideValue(null);
			entry2.SetBinding(Entry.TextProperty, bindingBase4);
			stackLayout3.Children.Add(entry2);
			stackLayout.SetValue(StackLayout.OrientationProperty, 0);
			translate3.Text = "pid_override_DefaultPriority";
			IMarkupExtension markupExtension3 = translate3;
			XamlServiceProvider xamlServiceProvider3 = new XamlServiceProvider();
			Type typeFromHandle6 = typeof(IProvideValueTarget);
			object[] array4 = new object[0 + 7];
			array4[0] = span;
			array4[1] = formattedString;
			array4[2] = label4;
			array4[3] = stackLayout;
			array4[4] = stackLayout3;
			array4[5] = scrollView;
			array4[6] = this;
			object obj5;
			xamlServiceProvider3.Add(typeFromHandle6, obj5 = new SimpleValueTargetProvider(array4, Span.TextProperty, nameScope));
			xamlServiceProvider3.Add(typeof(IReferenceProvider), obj5);
			Type typeFromHandle7 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver3 = new XmlNamespaceResolver();
			xmlNamespaceResolver3.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver3.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver3.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver3.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver3.Add("syncfusionTB", "clr-namespace:Syncfusion.SfNumericTextBox.XForms;assembly=Syncfusion.SfNumericTextBox.XForms");
			xamlServiceProvider3.Add(typeFromHandle7, new XamlTypeResolver(xmlNamespaceResolver3, typeof(SettingsPIDOverridePage).GetTypeInfo().Assembly));
			xamlServiceProvider3.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(29, 39)));
			object obj6 = markupExtension3.ProvideValue(xamlServiceProvider3);
			span.Text = obj6;
			formattedString.Spans.Add(span);
			span2.SetValue(Span.TextProperty, " ");
			formattedString.Spans.Add(span2);
			span3.SetValue(Span.FontAttributesProperty, new FontAttributesConverter().ConvertFromInvariantString("Bold"));
			bindingExtension5.Mode = 1;
			staticResourceExtension.Key = "SkipCyclesIntToPriorityStringConverter";
			IMarkupExtension markupExtension4 = staticResourceExtension;
			XamlServiceProvider xamlServiceProvider4 = new XamlServiceProvider();
			Type typeFromHandle8 = typeof(IProvideValueTarget);
			object[] array5 = new object[0 + 8];
			array5[0] = bindingExtension5;
			array5[1] = span3;
			array5[2] = formattedString;
			array5[3] = label4;
			array5[4] = stackLayout;
			array5[5] = stackLayout3;
			array5[6] = scrollView;
			array5[7] = this;
			object obj7;
			xamlServiceProvider4.Add(typeFromHandle8, obj7 = new SimpleValueTargetProvider(array5, typeof(BindingExtension).GetRuntimeProperty("Converter"), nameScope));
			xamlServiceProvider4.Add(typeof(IReferenceProvider), obj7);
			Type typeFromHandle9 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver4 = new XmlNamespaceResolver();
			xmlNamespaceResolver4.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver4.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver4.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver4.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver4.Add("syncfusionTB", "clr-namespace:Syncfusion.SfNumericTextBox.XForms;assembly=Syncfusion.SfNumericTextBox.XForms");
			xamlServiceProvider4.Add(typeFromHandle9, new XamlTypeResolver(xmlNamespaceResolver4, typeof(SettingsPIDOverridePage).GetTypeInfo().Assembly));
			xamlServiceProvider4.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(31, 61)));
			object obj8 = markupExtension4.ProvideValue(xamlServiceProvider4);
			bindingExtension5.Converter = obj8;
			bindingExtension5.Path = "OriginalSkipCycles";
			BindingBase bindingBase5 = bindingExtension5.ProvideValue(null);
			span3.SetBinding(Span.TextProperty, bindingBase5);
			formattedString.Spans.Add(span3);
			label4.SetValue(Label.FormattedTextProperty, formattedString);
			stackLayout.Children.Add(label4);
			translate4.Text = "pid_override_Priority";
			IMarkupExtension markupExtension5 = translate4;
			XamlServiceProvider xamlServiceProvider5 = new XamlServiceProvider();
			Type typeFromHandle10 = typeof(IProvideValueTarget);
			object[] array6 = new object[0 + 5];
			array6[0] = label5;
			array6[1] = stackLayout;
			array6[2] = stackLayout3;
			array6[3] = scrollView;
			array6[4] = this;
			object obj9;
			xamlServiceProvider5.Add(typeFromHandle10, obj9 = new SimpleValueTargetProvider(array6, Label.TextProperty, nameScope));
			xamlServiceProvider5.Add(typeof(IReferenceProvider), obj9);
			Type typeFromHandle11 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver5 = new XmlNamespaceResolver();
			xmlNamespaceResolver5.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver5.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver5.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver5.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver5.Add("syncfusionTB", "clr-namespace:Syncfusion.SfNumericTextBox.XForms;assembly=Syncfusion.SfNumericTextBox.XForms");
			xamlServiceProvider5.Add(typeFromHandle11, new XamlTypeResolver(xmlNamespaceResolver5, typeof(SettingsPIDOverridePage).GetTypeInfo().Assembly));
			xamlServiceProvider5.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(35, 28)));
			object obj10 = markupExtension5.ProvideValue(xamlServiceProvider5);
			label5.Text = obj10;
			stackLayout.Children.Add(label5);
			bindingExtension6.Mode = 1;
			staticResourceExtension2.Key = "SkipCyclesIntToPriorityStringConverter";
			IMarkupExtension markupExtension6 = staticResourceExtension2;
			XamlServiceProvider xamlServiceProvider6 = new XamlServiceProvider();
			Type typeFromHandle12 = typeof(IProvideValueTarget);
			object[] array7 = new object[0 + 6];
			array7[0] = bindingExtension6;
			array7[1] = picker;
			array7[2] = stackLayout;
			array7[3] = stackLayout3;
			array7[4] = scrollView;
			array7[5] = this;
			object obj11;
			xamlServiceProvider6.Add(typeFromHandle12, obj11 = new SimpleValueTargetProvider(array7, typeof(BindingExtension).GetRuntimeProperty("Converter"), nameScope));
			xamlServiceProvider6.Add(typeof(IReferenceProvider), obj11);
			Type typeFromHandle13 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver6 = new XmlNamespaceResolver();
			xmlNamespaceResolver6.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver6.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver6.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver6.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver6.Add("syncfusionTB", "clr-namespace:Syncfusion.SfNumericTextBox.XForms;assembly=Syncfusion.SfNumericTextBox.XForms");
			xamlServiceProvider6.Add(typeFromHandle13, new XamlTypeResolver(xmlNamespaceResolver6, typeof(SettingsPIDOverridePage).GetTypeInfo().Assembly));
			xamlServiceProvider6.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(36, 29)));
			object obj12 = markupExtension6.ProvideValue(xamlServiceProvider6);
			bindingExtension6.Converter = obj12;
			bindingExtension6.Path = "CustomSkipCycles";
			BindingBase bindingBase6 = bindingExtension6.ProvideValue(null);
			picker.SetBinding(Picker.SelectedItemProperty, bindingBase6);
			picker.SetValue(Picker.ItemsSourceProperty, array);
			stackLayout.Children.Add(picker);
			translate5.Text = "pid_override_DefaultRole";
			IMarkupExtension markupExtension7 = translate5;
			XamlServiceProvider xamlServiceProvider7 = new XamlServiceProvider();
			Type typeFromHandle14 = typeof(IProvideValueTarget);
			object[] array8 = new object[0 + 7];
			array8[0] = span4;
			array8[1] = formattedString2;
			array8[2] = label6;
			array8[3] = stackLayout;
			array8[4] = stackLayout3;
			array8[5] = scrollView;
			array8[6] = this;
			object obj13;
			xamlServiceProvider7.Add(typeFromHandle14, obj13 = new SimpleValueTargetProvider(array8, Span.TextProperty, nameScope));
			xamlServiceProvider7.Add(typeof(IReferenceProvider), obj13);
			Type typeFromHandle15 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver7 = new XmlNamespaceResolver();
			xmlNamespaceResolver7.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver7.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver7.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver7.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver7.Add("syncfusionTB", "clr-namespace:Syncfusion.SfNumericTextBox.XForms;assembly=Syncfusion.SfNumericTextBox.XForms");
			xamlServiceProvider7.Add(typeFromHandle15, new XamlTypeResolver(xmlNamespaceResolver7, typeof(SettingsPIDOverridePage).GetTypeInfo().Assembly));
			xamlServiceProvider7.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(70, 39)));
			object obj14 = markupExtension7.ProvideValue(xamlServiceProvider7);
			span4.Text = obj14;
			formattedString2.Spans.Add(span4);
			span5.SetValue(Span.TextProperty, " ");
			formattedString2.Spans.Add(span5);
			span6.SetValue(Span.FontAttributesProperty, new FontAttributesConverter().ConvertFromInvariantString("Bold"));
			bindingExtension7.Mode = 2;
			bindingExtension7.Path = "OriginalRole";
			BindingBase bindingBase7 = bindingExtension7.ProvideValue(null);
			span6.SetBinding(Span.TextProperty, bindingBase7);
			formattedString2.Spans.Add(span6);
			label6.SetValue(Label.FormattedTextProperty, formattedString2);
			stackLayout.Children.Add(label6);
			translate6.Text = "pid_override_Role";
			IMarkupExtension markupExtension8 = translate6;
			XamlServiceProvider xamlServiceProvider8 = new XamlServiceProvider();
			Type typeFromHandle16 = typeof(IProvideValueTarget);
			object[] array9 = new object[0 + 5];
			array9[0] = label7;
			array9[1] = stackLayout;
			array9[2] = stackLayout3;
			array9[3] = scrollView;
			array9[4] = this;
			object obj15;
			xamlServiceProvider8.Add(typeFromHandle16, obj15 = new SimpleValueTargetProvider(array9, Label.TextProperty, nameScope));
			xamlServiceProvider8.Add(typeof(IReferenceProvider), obj15);
			Type typeFromHandle17 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver8 = new XmlNamespaceResolver();
			xmlNamespaceResolver8.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver8.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver8.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver8.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver8.Add("syncfusionTB", "clr-namespace:Syncfusion.SfNumericTextBox.XForms;assembly=Syncfusion.SfNumericTextBox.XForms");
			xamlServiceProvider8.Add(typeFromHandle17, new XamlTypeResolver(xmlNamespaceResolver8, typeof(SettingsPIDOverridePage).GetTypeInfo().Assembly));
			xamlServiceProvider8.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(76, 28)));
			object obj16 = markupExtension8.ProvideValue(xamlServiceProvider8);
			label7.Text = obj16;
			stackLayout.Children.Add(label7);
			bindingExtension8.Mode = 1;
			bindingExtension8.Path = "CustomRole";
			BindingBase bindingBase8 = bindingExtension8.ProvideValue(null);
			picker2.SetBinding(Picker.SelectedItemProperty, bindingBase8);
			stackLayout.Children.Add(picker2);
			stackLayout3.Children.Add(stackLayout);
			stackLayout2.SetValue(VisualElement.IsVisibleProperty, new VisualElement.VisibilityConverter().ConvertFromInvariantString("False"));
			stackLayout2.SetValue(StackLayout.OrientationProperty, 0);
			translate7.Text = "pid_override_DefaultUnits";
			IMarkupExtension markupExtension9 = translate7;
			XamlServiceProvider xamlServiceProvider9 = new XamlServiceProvider();
			Type typeFromHandle18 = typeof(IProvideValueTarget);
			object[] array10 = new object[0 + 7];
			array10[0] = span7;
			array10[1] = formattedString3;
			array10[2] = label8;
			array10[3] = stackLayout2;
			array10[4] = stackLayout3;
			array10[5] = scrollView;
			array10[6] = this;
			object obj17;
			xamlServiceProvider9.Add(typeFromHandle18, obj17 = new SimpleValueTargetProvider(array10, Span.TextProperty, nameScope));
			xamlServiceProvider9.Add(typeof(IReferenceProvider), obj17);
			Type typeFromHandle19 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver9 = new XmlNamespaceResolver();
			xmlNamespaceResolver9.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver9.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver9.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver9.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver9.Add("syncfusionTB", "clr-namespace:Syncfusion.SfNumericTextBox.XForms;assembly=Syncfusion.SfNumericTextBox.XForms");
			xamlServiceProvider9.Add(typeFromHandle19, new XamlTypeResolver(xmlNamespaceResolver9, typeof(SettingsPIDOverridePage).GetTypeInfo().Assembly));
			xamlServiceProvider9.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(89, 39)));
			object obj18 = markupExtension9.ProvideValue(xamlServiceProvider9);
			span7.Text = obj18;
			formattedString3.Spans.Add(span7);
			span8.SetValue(Span.TextProperty, " ");
			formattedString3.Spans.Add(span8);
			span9.SetValue(Span.FontAttributesProperty, new FontAttributesConverter().ConvertFromInvariantString("Bold"));
			staticResourceExtension3.Key = "UnitsToStringConverter";
			IMarkupExtension markupExtension10 = staticResourceExtension3;
			XamlServiceProvider xamlServiceProvider10 = new XamlServiceProvider();
			Type typeFromHandle20 = typeof(IProvideValueTarget);
			object[] array11 = new object[0 + 8];
			array11[0] = bindingExtension9;
			array11[1] = span9;
			array11[2] = formattedString3;
			array11[3] = label8;
			array11[4] = stackLayout2;
			array11[5] = stackLayout3;
			array11[6] = scrollView;
			array11[7] = this;
			object obj19;
			xamlServiceProvider10.Add(typeFromHandle20, obj19 = new SimpleValueTargetProvider(array11, typeof(BindingExtension).GetRuntimeProperty("Converter"), nameScope));
			xamlServiceProvider10.Add(typeof(IReferenceProvider), obj19);
			Type typeFromHandle21 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver10 = new XmlNamespaceResolver();
			xmlNamespaceResolver10.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver10.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver10.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver10.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver10.Add("syncfusionTB", "clr-namespace:Syncfusion.SfNumericTextBox.XForms;assembly=Syncfusion.SfNumericTextBox.XForms");
			xamlServiceProvider10.Add(typeFromHandle21, new XamlTypeResolver(xmlNamespaceResolver10, typeof(SettingsPIDOverridePage).GetTypeInfo().Assembly));
			xamlServiceProvider10.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(91, 61)));
			object obj20 = markupExtension10.ProvideValue(xamlServiceProvider10);
			bindingExtension9.Converter = obj20;
			bindingExtension9.Path = "Units";
			BindingBase bindingBase9 = bindingExtension9.ProvideValue(null);
			span9.SetBinding(Span.TextProperty, bindingBase9);
			formattedString3.Spans.Add(span9);
			label8.SetValue(Label.FormattedTextProperty, formattedString3);
			stackLayout2.Children.Add(label8);
			translate8.Text = "pid_override_Units";
			IMarkupExtension markupExtension11 = translate8;
			XamlServiceProvider xamlServiceProvider11 = new XamlServiceProvider();
			Type typeFromHandle22 = typeof(IProvideValueTarget);
			object[] array12 = new object[0 + 5];
			array12[0] = label9;
			array12[1] = stackLayout2;
			array12[2] = stackLayout3;
			array12[3] = scrollView;
			array12[4] = this;
			object obj21;
			xamlServiceProvider11.Add(typeFromHandle22, obj21 = new SimpleValueTargetProvider(array12, Label.TextProperty, nameScope));
			xamlServiceProvider11.Add(typeof(IReferenceProvider), obj21);
			Type typeFromHandle23 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver11 = new XmlNamespaceResolver();
			xmlNamespaceResolver11.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver11.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver11.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver11.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver11.Add("syncfusionTB", "clr-namespace:Syncfusion.SfNumericTextBox.XForms;assembly=Syncfusion.SfNumericTextBox.XForms");
			xamlServiceProvider11.Add(typeFromHandle23, new XamlTypeResolver(xmlNamespaceResolver11, typeof(SettingsPIDOverridePage).GetTypeInfo().Assembly));
			xamlServiceProvider11.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(95, 48)));
			object obj22 = markupExtension11.ProvideValue(xamlServiceProvider11);
			label9.Text = obj22;
			stackLayout2.Children.Add(label9);
			staticResourceExtension4.Key = "UnitsToStringInvariantConverter";
			IMarkupExtension markupExtension12 = staticResourceExtension4;
			XamlServiceProvider xamlServiceProvider12 = new XamlServiceProvider();
			Type typeFromHandle24 = typeof(IProvideValueTarget);
			object[] array13 = new object[0 + 6];
			array13[0] = bindingExtension10;
			array13[1] = picker3;
			array13[2] = stackLayout2;
			array13[3] = stackLayout3;
			array13[4] = scrollView;
			array13[5] = this;
			object obj23;
			xamlServiceProvider12.Add(typeFromHandle24, obj23 = new SimpleValueTargetProvider(array13, typeof(BindingExtension).GetRuntimeProperty("Converter"), nameScope));
			xamlServiceProvider12.Add(typeof(IReferenceProvider), obj23);
			Type typeFromHandle25 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver12 = new XmlNamespaceResolver();
			xmlNamespaceResolver12.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver12.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver12.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver12.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver12.Add("syncfusionTB", "clr-namespace:Syncfusion.SfNumericTextBox.XForms;assembly=Syncfusion.SfNumericTextBox.XForms");
			xamlServiceProvider12.Add(typeFromHandle25, new XamlTypeResolver(xmlNamespaceResolver12, typeof(SettingsPIDOverridePage).GetTypeInfo().Assembly));
			xamlServiceProvider12.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(98, 25)));
			object obj24 = markupExtension12.ProvideValue(xamlServiceProvider12);
			bindingExtension10.Converter = obj24;
			bindingExtension10.Path = ".";
			BindingBase bindingBase10 = bindingExtension10.ProvideValue(null);
			picker3.ItemDisplayBinding = bindingBase10;
			bindingExtension11.Mode = 1;
			bindingExtension11.Path = "CustomUnit";
			BindingBase bindingBase11 = bindingExtension11.ProvideValue(null);
			picker3.SetBinding(Picker.SelectedItemProperty, bindingBase11);
			stackLayout2.Children.Add(picker3);
			stackLayout3.Children.Add(stackLayout2);
			scrollView.Content = stackLayout3;
			this.SetValue(ContentPage.ContentProperty, scrollView);
		}

		// Token: 0x06001EF5 RID: 7925 RVA: 0x00153E58 File Offset: 0x00152058
		private void __InitComponentRuntime()
		{
			Extensions.LoadFromXaml<SettingsPIDOverridePage>(this, typeof(SettingsPIDOverridePage));
			this.panelNotForCalculated = NameScopeExtensions.FindByName<StackLayout>(this, "panelNotForCalculated");
			this.pickerRole = NameScopeExtensions.FindByName<Picker>(this, "pickerRole");
			this.panelUnits = NameScopeExtensions.FindByName<StackLayout>(this, "panelUnits");
			this.labelUnits = NameScopeExtensions.FindByName<Label>(this, "labelUnits");
			this.pickerUnits = NameScopeExtensions.FindByName<Picker>(this, "pickerUnits");
		}

		// Token: 0x04000F45 RID: 3909
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private StackLayout panelNotForCalculated;

		// Token: 0x04000F46 RID: 3910
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private Picker pickerRole;

		// Token: 0x04000F47 RID: 3911
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private StackLayout panelUnits;

		// Token: 0x04000F48 RID: 3912
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private Label labelUnits;

		// Token: 0x04000F49 RID: 3913
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private Picker pickerUnits;
	}
}
