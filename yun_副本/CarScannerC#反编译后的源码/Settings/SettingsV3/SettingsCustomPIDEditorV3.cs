using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using AiForms.Renderers;
using CarScannerXamarinForms.Common.XAMLConverters;
using CarScannerXamarinForms.OBD2;
using CarScannerXamarinForms.OBD2.PIDS;
using CarScannerXamarinForms.OBD2.VWTP20;
using CarScannerXamarinForms.UserControls;
using CarScannerXamarinForms.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Internals;
using Xamarin.Forms.PlatformConfiguration.iOSSpecific;
using Xamarin.Forms.Xaml;
using Xamarin.Forms.Xaml.Diagnostics;
using Xamarin.Forms.Xaml.Internals;

namespace CarScannerXamarinForms.Settings.SettingsV3
{
	// Token: 0x020002B5 RID: 693
	[XamlCompilation(2)]
	[XamlFilePath("Settings\\SettingsV3\\__SettingsCustomPIDEditorV3.xaml")]
	public class SettingsCustomPIDEditorV3 : ContentPage
	{
		// Token: 0x0600220C RID: 8716 RVA: 0x001A0CE4 File Offset: 0x0019EEE4
		public SettingsCustomPIDEditorV3(CustomPID cpid)
		{
			this.InitializeComponent();
			base.Disappearing += this.SettingsCustomPIDEditorV3_Disappearing;
			Array values = Enum.GetValues(typeof(Roles));
			this.pickerRole.ItemsSource = values;
			this.CPid = cpid;
			base.BindingContext = this.CPid;
			base.Content.BindingContext = this.CPid;
		}

		// Token: 0x170010E5 RID: 4325
		// (get) Token: 0x0600220D RID: 8717 RVA: 0x001A0D4F File Offset: 0x0019EF4F
		// (set) Token: 0x0600220E RID: 8718 RVA: 0x001A0D57 File Offset: 0x0019EF57
		public CustomPID CPid
		{
			[CompilerGenerated]
			get
			{
				return this.<CPid>k__BackingField;
			}
			[CompilerGenerated]
			private set
			{
				this.<CPid>k__BackingField = value;
			}
		}

		// Token: 0x0600220F RID: 8719 RVA: 0x001A0D60 File Offset: 0x0019EF60
		private async void btnUnitsOverride_Clicked(object sender, EventArgs e)
		{
			Page page;
			if (App.UseLegacyUI)
			{
				page = new SettingsPIDOverridePage(this.CPid);
			}
			else
			{
				page = new SettingsPIDOverrideEditorPageV3(this.CPid);
			}
			await base.Navigation.PushAsync(page);
		}

		// Token: 0x06002210 RID: 8720 RVA: 0x001A0D97 File Offset: 0x0019EF97
		private void SettingsCustomPIDEditorV3_Disappearing(object sender, EventArgs e)
		{
			CustomPIDViewModel.CurrentCustom.Save();
		}

		// Token: 0x06002211 RID: 8721 RVA: 0x001A0DA4 File Offset: 0x0019EFA4
		private async void UpdateTestCellVisible()
		{
			string tempres = this.testLabelCell.Title;
			await Task.Delay(300);
			if (tempres == this.testLabelCell.Title)
			{
				this.testLabelCell.IsVisible = false;
				await Task.Delay(300);
				this.testLabelCell.IsVisible = true;
			}
		}

		// Token: 0x06002212 RID: 8722 RVA: 0x001A0DDC File Offset: 0x0019EFDC
		private void BtnTest_Clicked(object sender, EventArgs e)
		{
			try
			{
				if (!App.OBDSimulator.IsActive && (App.OBDReader.CurrentStatus == OBDDataReaderStatus.ConnectedToECU || App.OBDReader.CurrentStatus == OBDDataReaderStatus.ConnectedToELM))
				{
					SettingsCustomPIDEditorV3.<>c__DisplayClass8_0 CS$<>8__locals1 = new SettingsCustomPIDEditorV3.<>c__DisplayClass8_0();
					CS$<>8__locals1.<>4__this = this;
					this.testLabelCell.Title = "";
					CS$<>8__locals1.model = new LiveDataPIDModel();
					CS$<>8__locals1.model.SelectedPID = this.CPid;
					List<OBDRequest> list = new List<OBDRequest>(1);
					CS$<>8__locals1.model.GetRequests(list, null, "");
					OBDRequest obdrequest = list[0];
					obdrequest.Repeat = false;
					obdrequest.ResponseReceived += delegate(OBDRequest request2, string data)
					{
						SettingsCustomPIDEditorV3.<>c__DisplayClass8_1 CS$<>8__locals2 = new SettingsCustomPIDEditorV3.<>c__DisplayClass8_1();
						CS$<>8__locals2.<>4__this = this;
						CS$<>8__locals2.data = data;
						Device.BeginInvokeOnMainThread(delegate
						{
							SettingsCustomPIDEditorV3.<>c__DisplayClass8_1.<<BtnTest_Clicked>b__3>d <<BtnTest_Clicked>b__3>d;
							<<BtnTest_Clicked>b__3>d.<>t__builder = AsyncVoidMethodBuilder.Create();
							<<BtnTest_Clicked>b__3>d.<>4__this = CS$<>8__locals2;
							<<BtnTest_Clicked>b__3>d.<>1__state = -1;
							<<BtnTest_Clicked>b__3>d.<>t__builder.Start<SettingsCustomPIDEditorV3.<>c__DisplayClass8_1.<<BtnTest_Clicked>b__3>d>(ref <<BtnTest_Clicked>b__3>d);
						});
					};
					obdrequest.ResponseDecoded += delegate(OBDRequest request2, byte[] data, bool decodeResult, string responseHeader)
					{
						if (data != null && data.Length != 0)
						{
							SettingsCustomPIDEditorV3.<>c__DisplayClass8_2 CS$<>8__locals3 = new SettingsCustomPIDEditorV3.<>c__DisplayClass8_2();
							CS$<>8__locals3.CS$<>8__locals1 = CS$<>8__locals1;
							if (CS$<>8__locals1.<>4__this.CPid.Type == CustomPIDType.Formula)
							{
								CS$<>8__locals3.sb = new StringBuilder(data.Length * 4);
								for (int i = 0; i < data.Length; i++)
								{
									string text = CustomPID.DICT_LETTERS[i];
									byte b = data[i];
									CS$<>8__locals3.sb.Append(text);
									CS$<>8__locals3.sb.Append(":");
									CS$<>8__locals3.sb.Append(b.ToString("X2"));
									CS$<>8__locals3.sb.Append("\n");
								}
							}
							else
							{
								CS$<>8__locals3.sb = new StringBuilder(data.Length * 4);
								for (int j = 0; j < data.Length; j++)
								{
									byte b2 = data[j];
									CS$<>8__locals3.sb.Append(j.ToString());
									CS$<>8__locals3.sb.Append(":");
									CS$<>8__locals3.sb.Append(b2.ToString("X2"));
									CS$<>8__locals3.sb.Append("\n");
								}
							}
							Device.BeginInvokeOnMainThread(delegate
							{
								SettingsCustomPIDEditorV3.<>c__DisplayClass8_2.<<BtnTest_Clicked>b__4>d <<BtnTest_Clicked>b__4>d;
								<<BtnTest_Clicked>b__4>d.<>t__builder = AsyncVoidMethodBuilder.Create();
								<<BtnTest_Clicked>b__4>d.<>4__this = CS$<>8__locals3;
								<<BtnTest_Clicked>b__4>d.<>1__state = -1;
								<<BtnTest_Clicked>b__4>d.<>t__builder.Start<SettingsCustomPIDEditorV3.<>c__DisplayClass8_2.<<BtnTest_Clicked>b__4>d>(ref <<BtnTest_Clicked>b__4>d);
							});
						}
					};
					CS$<>8__locals1.model.PropertyChanged += delegate(object senderModel, PropertyChangedEventArgs propertyArgs)
					{
						if (propertyArgs.PropertyName == "FloatValue")
						{
							Action action;
							if ((action = CS$<>8__locals1.<>9__5) == null)
							{
								action = (CS$<>8__locals1.<>9__5 = delegate
								{
									SettingsCustomPIDEditorV3.<>c__DisplayClass8_0.<<BtnTest_Clicked>b__5>d <<BtnTest_Clicked>b__5>d;
									<<BtnTest_Clicked>b__5>d.<>t__builder = AsyncVoidMethodBuilder.Create();
									<<BtnTest_Clicked>b__5>d.<>4__this = CS$<>8__locals1;
									<<BtnTest_Clicked>b__5>d.<>1__state = -1;
									<<BtnTest_Clicked>b__5>d.<>t__builder.Start<SettingsCustomPIDEditorV3.<>c__DisplayClass8_0.<<BtnTest_Clicked>b__5>d>(ref <<BtnTest_Clicked>b__5>d);
								});
							}
							Device.BeginInvokeOnMainThread(action);
						}
					};
					App.OBDReader.AddRequestToQueue(obdrequest);
				}
				else
				{
					base.DisplayAlert("Please connect to the car first", "", "OK");
				}
			}
			catch (Exception)
			{
			}
		}

		// Token: 0x06002213 RID: 8723 RVA: 0x001A0EEC File Offset: 0x0019F0EC
		private void btnApplyVagGroup_Tapped(object sender, EventArgs e)
		{
			string valueText = this.vagGroupEntry.ValueText;
			string text = this.vagUnitEntry.ValueText;
			if (text.Length < 2)
			{
				text = "0" + text;
			}
			text = text.ToUpperInvariant();
			string valueText2 = this.vagItemInGroupEntry.ValueText;
			if (string.IsNullOrEmpty(valueText) || string.IsNullOrEmpty(text))
			{
				return;
			}
			int num;
			int num2;
			if (int.TryParse(valueText, out num) && num > 0 && num <= 255 && int.TryParse(valueText2, out num2) && num2 >= 1 && num2 <= 4)
			{
				this.CPid.Command = "VWTP:" + VWTPManager.UnitToCANAddress(text) + ":21" + num.ToString("X2");
				this.CPid.StartByteId = num2 - 1;
				this.CPid.BeforeCommand = "ATCAF0;ATV1;ATSP6;ATCM000";
				this.CPid.AfterCommand = "ATSPDEF;ATCAF1;ATV0";
				this.CPid.Header = "000";
			}
		}

		// Token: 0x06002214 RID: 8724 RVA: 0x001A0FE8 File Offset: 0x0019F1E8
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private void InitializeComponent()
		{
			if (ResourceLoader.IsEnabled && ResourceLoader.CanProvideContentFor(new ResourceLoader.ResourceLoadingQuery
			{
				AssemblyName = typeof(SettingsCustomPIDEditorV3).GetTypeInfo().Assembly.GetName(),
				ResourcePath = "Settings/SettingsV3/__SettingsCustomPIDEditorV3.xaml",
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
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension = new DynamicResourceExtension(), new Uri("Settings\\SettingsV3\\__SettingsCustomPIDEditorV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 16, 5);
			UnitsToIntConverter unitsToIntConverter;
			VisualDiagnostics.RegisterSourceInfo(unitsToIntConverter = new UnitsToIntConverter(), new Uri("Settings\\SettingsV3\\__SettingsCustomPIDEditorV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 19, 14);
			BoolToNegativeConverter boolToNegativeConverter;
			VisualDiagnostics.RegisterSourceInfo(boolToNegativeConverter = new BoolToNegativeConverter(), new Uri("Settings\\SettingsV3\\__SettingsCustomPIDEditorV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 20, 14);
			DoubleToStringConverter doubleToStringConverter;
			VisualDiagnostics.RegisterSourceInfo(doubleToStringConverter = new DoubleToStringConverter(), new Uri("Settings\\SettingsV3\\__SettingsCustomPIDEditorV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 21, 14);
			SkipCyclesIntToPriorityStringConverter skipCyclesIntToPriorityStringConverter;
			VisualDiagnostics.RegisterSourceInfo(skipCyclesIntToPriorityStringConverter = new SkipCyclesIntToPriorityStringConverter(), new Uri("Settings\\SettingsV3\\__SettingsCustomPIDEditorV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 22, 14);
			CustomPIDTypeFormulaToTrue customPIDTypeFormulaToTrue;
			VisualDiagnostics.RegisterSourceInfo(customPIDTypeFormulaToTrue = new CustomPIDTypeFormulaToTrue(), new Uri("Settings\\SettingsV3\\__SettingsCustomPIDEditorV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 23, 14);
			EnumToIntConverter enumToIntConverter;
			VisualDiagnostics.RegisterSourceInfo(enumToIntConverter = new EnumToIntConverter(), new Uri("Settings\\SettingsV3\\__SettingsCustomPIDEditorV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 24, 14);
			EnumValueToTrueConverter enumValueToTrueConverter;
			VisualDiagnostics.RegisterSourceInfo(enumValueToTrueConverter = new EnumValueToTrueConverter(), new Uri("Settings\\SettingsV3\\__SettingsCustomPIDEditorV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 25, 14);
			EnumValueToFalseConverter enumValueToFalseConverter;
			VisualDiagnostics.RegisterSourceInfo(enumValueToFalseConverter = new EnumValueToFalseConverter(), new Uri("Settings\\SettingsV3\\__SettingsCustomPIDEditorV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 26, 14);
			UnitsToStringInvariantConverter unitsToStringInvariantConverter;
			VisualDiagnostics.RegisterSourceInfo(unitsToStringInvariantConverter = new UnitsToStringInvariantConverter(), new Uri("Settings\\SettingsV3\\__SettingsCustomPIDEditorV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 27, 14);
			UnitsToStringConverter unitsToStringConverter;
			VisualDiagnostics.RegisterSourceInfo(unitsToStringConverter = new UnitsToStringConverter(), new Uri("Settings\\SettingsV3\\__SettingsCustomPIDEditorV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 28, 14);
			ResourceDictionary resourceDictionary;
			VisualDiagnostics.RegisterSourceInfo(resourceDictionary = new ResourceDictionary(), new Uri("Settings\\SettingsV3\\__SettingsCustomPIDEditorV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 18, 10);
			Translate translate;
			VisualDiagnostics.RegisterSourceInfo(translate = new Translate(), new Uri("Settings\\SettingsV3\\__SettingsCustomPIDEditorV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 33, 25);
			BindingExtension bindingExtension;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension = new BindingExtension(), new Uri("Settings\\SettingsV3\\__SettingsCustomPIDEditorV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 35, 28);
			Entry entry;
			VisualDiagnostics.RegisterSourceInfo(entry = new Entry(), new Uri("Settings\\SettingsV3\\__SettingsCustomPIDEditorV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 35, 22);
			CustomCell customCell;
			VisualDiagnostics.RegisterSourceInfo(customCell = new CustomCell(), new Uri("Settings\\SettingsV3\\__SettingsCustomPIDEditorV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 34, 18);
			Section section;
			VisualDiagnostics.RegisterSourceInfo(section = new Section(), new Uri("Settings\\SettingsV3\\__SettingsCustomPIDEditorV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 33, 14);
			Translate translate2;
			VisualDiagnostics.RegisterSourceInfo(translate2 = new Translate(), new Uri("Settings\\SettingsV3\\__SettingsCustomPIDEditorV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 38, 25);
			BindingExtension bindingExtension2;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension2 = new BindingExtension(), new Uri("Settings\\SettingsV3\\__SettingsCustomPIDEditorV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 40, 28);
			Entry entry2;
			VisualDiagnostics.RegisterSourceInfo(entry2 = new Entry(), new Uri("Settings\\SettingsV3\\__SettingsCustomPIDEditorV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 40, 22);
			CustomCell customCell2;
			VisualDiagnostics.RegisterSourceInfo(customCell2 = new CustomCell(), new Uri("Settings\\SettingsV3\\__SettingsCustomPIDEditorV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 39, 18);
			Section section2;
			VisualDiagnostics.RegisterSourceInfo(section2 = new Section(), new Uri("Settings\\SettingsV3\\__SettingsCustomPIDEditorV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 38, 14);
			Translate translate3;
			VisualDiagnostics.RegisterSourceInfo(translate3 = new Translate(), new Uri("Settings\\SettingsV3\\__SettingsCustomPIDEditorV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 43, 25);
			StaticResourceExtension staticResourceExtension;
			VisualDiagnostics.RegisterSourceInfo(staticResourceExtension = new StaticResourceExtension(), new Uri("Settings\\SettingsV3\\__SettingsCustomPIDEditorV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 43, 87);
			BindingExtension bindingExtension3;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension3 = new BindingExtension(), new Uri("Settings\\SettingsV3\\__SettingsCustomPIDEditorV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 43, 87);
			BindingExtension bindingExtension4;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension4 = new BindingExtension(), new Uri("Settings\\SettingsV3\\__SettingsCustomPIDEditorV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 45, 28);
			Entry entry3;
			VisualDiagnostics.RegisterSourceInfo(entry3 = new Entry(), new Uri("Settings\\SettingsV3\\__SettingsCustomPIDEditorV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 45, 22);
			CustomCell customCell3;
			VisualDiagnostics.RegisterSourceInfo(customCell3 = new CustomCell(), new Uri("Settings\\SettingsV3\\__SettingsCustomPIDEditorV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 44, 18);
			Section section3;
			VisualDiagnostics.RegisterSourceInfo(section3 = new Section(), new Uri("Settings\\SettingsV3\\__SettingsCustomPIDEditorV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 43, 14);
			Translate translate4;
			VisualDiagnostics.RegisterSourceInfo(translate4 = new Translate(), new Uri("Settings\\SettingsV3\\__SettingsCustomPIDEditorV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 48, 25);
			StaticResourceExtension staticResourceExtension2;
			VisualDiagnostics.RegisterSourceInfo(staticResourceExtension2 = new StaticResourceExtension(), new Uri("Settings\\SettingsV3\\__SettingsCustomPIDEditorV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 48, 86);
			BindingExtension bindingExtension5;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension5 = new BindingExtension(), new Uri("Settings\\SettingsV3\\__SettingsCustomPIDEditorV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 48, 86);
			BindingExtension bindingExtension6;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension6 = new BindingExtension(), new Uri("Settings\\SettingsV3\\__SettingsCustomPIDEditorV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 50, 28);
			Entry entry4;
			VisualDiagnostics.RegisterSourceInfo(entry4 = new Entry(), new Uri("Settings\\SettingsV3\\__SettingsCustomPIDEditorV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 50, 22);
			CustomCell customCell4;
			VisualDiagnostics.RegisterSourceInfo(customCell4 = new CustomCell(), new Uri("Settings\\SettingsV3\\__SettingsCustomPIDEditorV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 49, 18);
			Section section4;
			VisualDiagnostics.RegisterSourceInfo(section4 = new Section(), new Uri("Settings\\SettingsV3\\__SettingsCustomPIDEditorV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 48, 14);
			Translate translate5;
			VisualDiagnostics.RegisterSourceInfo(translate5 = new Translate(), new Uri("Settings\\SettingsV3\\__SettingsCustomPIDEditorV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 53, 25);
			BindingExtension bindingExtension7;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension7 = new BindingExtension(), new Uri("Settings\\SettingsV3\\__SettingsCustomPIDEditorV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 53, 87);
			Translate translate6;
			VisualDiagnostics.RegisterSourceInfo(translate6 = new Translate(), new Uri("Settings\\SettingsV3\\__SettingsCustomPIDEditorV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 54, 31);
			CustomPIDType customPIDType = CustomPIDType.Formula;
			RadioCell radioCell;
			VisualDiagnostics.RegisterSourceInfo(radioCell = new RadioCell(), new Uri("Settings\\SettingsV3\\__SettingsCustomPIDEditorV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 54, 18);
			Translate translate7;
			VisualDiagnostics.RegisterSourceInfo(translate7 = new Translate(), new Uri("Settings\\SettingsV3\\__SettingsCustomPIDEditorV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 55, 31);
			CustomPIDType customPIDType2 = CustomPIDType.ByteSet;
			RadioCell radioCell2;
			VisualDiagnostics.RegisterSourceInfo(radioCell2 = new RadioCell(), new Uri("Settings\\SettingsV3\\__SettingsCustomPIDEditorV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 55, 18);
			Translate translate8;
			VisualDiagnostics.RegisterSourceInfo(translate8 = new Translate(), new Uri("Settings\\SettingsV3\\__SettingsCustomPIDEditorV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 56, 31);
			CustomPIDType customPIDType3 = CustomPIDType.BitValue;
			RadioCell radioCell3;
			VisualDiagnostics.RegisterSourceInfo(radioCell3 = new RadioCell(), new Uri("Settings\\SettingsV3\\__SettingsCustomPIDEditorV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 56, 18);
			Translate translate9;
			VisualDiagnostics.RegisterSourceInfo(translate9 = new Translate(), new Uri("Settings\\SettingsV3\\__SettingsCustomPIDEditorV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 57, 31);
			CustomPIDType customPIDType4 = CustomPIDType.Action;
			RadioCell radioCell4;
			VisualDiagnostics.RegisterSourceInfo(radioCell4 = new RadioCell(), new Uri("Settings\\SettingsV3\\__SettingsCustomPIDEditorV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 57, 18);
			CustomPIDType customPIDType5 = CustomPIDType.VWTPGroupItem;
			RadioCell radioCell5;
			VisualDiagnostics.RegisterSourceInfo(radioCell5 = new RadioCell(), new Uri("Settings\\SettingsV3\\__SettingsCustomPIDEditorV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 58, 18);
			Section section5;
			VisualDiagnostics.RegisterSourceInfo(section5 = new Section(), new Uri("Settings\\SettingsV3\\__SettingsCustomPIDEditorV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 53, 14);
			Translate translate10;
			VisualDiagnostics.RegisterSourceInfo(translate10 = new Translate(), new Uri("Settings\\SettingsV3\\__SettingsCustomPIDEditorV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 61, 25);
			StaticResourceExtension staticResourceExtension3;
			VisualDiagnostics.RegisterSourceInfo(staticResourceExtension3 = new StaticResourceExtension(), new Uri("Settings\\SettingsV3\\__SettingsCustomPIDEditorV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 61, 87);
			BindingExtension bindingExtension8;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension8 = new BindingExtension(), new Uri("Settings\\SettingsV3\\__SettingsCustomPIDEditorV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 61, 87);
			BindingExtension bindingExtension9;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension9 = new BindingExtension(), new Uri("Settings\\SettingsV3\\__SettingsCustomPIDEditorV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 63, 28);
			Entry entry5;
			VisualDiagnostics.RegisterSourceInfo(entry5 = new Entry(), new Uri("Settings\\SettingsV3\\__SettingsCustomPIDEditorV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 63, 22);
			CustomCell customCell5;
			VisualDiagnostics.RegisterSourceInfo(customCell5 = new CustomCell(), new Uri("Settings\\SettingsV3\\__SettingsCustomPIDEditorV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 62, 18);
			Translate translate11;
			VisualDiagnostics.RegisterSourceInfo(translate11 = new Translate(), new Uri("Settings\\SettingsV3\\__SettingsCustomPIDEditorV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 66, 21);
			BindingExtension bindingExtension10;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension10 = new BindingExtension(), new Uri("Settings\\SettingsV3\\__SettingsCustomPIDEditorV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 67, 21);
			DynamicResourceExtension dynamicResourceExtension2;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension2 = new DynamicResourceExtension(), new Uri("Settings\\SettingsV3\\__SettingsCustomPIDEditorV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 68, 21);
			LabelCell labelCell;
			VisualDiagnostics.RegisterSourceInfo(labelCell = new LabelCell(), new Uri("Settings\\SettingsV3\\__SettingsCustomPIDEditorV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 65, 18);
			Translate translate12;
			VisualDiagnostics.RegisterSourceInfo(translate12 = new Translate(), new Uri("Settings\\SettingsV3\\__SettingsCustomPIDEditorV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 70, 21);
			StaticResourceExtension staticResourceExtension4;
			VisualDiagnostics.RegisterSourceInfo(staticResourceExtension4 = new StaticResourceExtension(), new Uri("Settings\\SettingsV3\\__SettingsCustomPIDEditorV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 71, 21);
			BindingExtension bindingExtension11;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension11 = new BindingExtension(), new Uri("Settings\\SettingsV3\\__SettingsCustomPIDEditorV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 71, 21);
			DynamicResourceExtension dynamicResourceExtension3;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension3 = new DynamicResourceExtension(), new Uri("Settings\\SettingsV3\\__SettingsCustomPIDEditorV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 72, 21);
			LabelCell labelCell2;
			VisualDiagnostics.RegisterSourceInfo(labelCell2 = new LabelCell(), new Uri("Settings\\SettingsV3\\__SettingsCustomPIDEditorV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 69, 18);
			Section section6;
			VisualDiagnostics.RegisterSourceInfo(section6 = new Section(), new Uri("Settings\\SettingsV3\\__SettingsCustomPIDEditorV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 61, 14);
			StaticResourceExtension staticResourceExtension5;
			VisualDiagnostics.RegisterSourceInfo(staticResourceExtension5 = new StaticResourceExtension(), new Uri("Settings\\SettingsV3\\__SettingsCustomPIDEditorV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 75, 25);
			BindingExtension bindingExtension12;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension12 = new BindingExtension(), new Uri("Settings\\SettingsV3\\__SettingsCustomPIDEditorV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 75, 25);
			Translate translate13;
			VisualDiagnostics.RegisterSourceInfo(translate13 = new Translate(), new Uri("Settings\\SettingsV3\\__SettingsCustomPIDEditorV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 77, 21);
			BindingExtension bindingExtension13;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension13 = new BindingExtension(), new Uri("Settings\\SettingsV3\\__SettingsCustomPIDEditorV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 80, 21);
			NumberPickerCell numberPickerCell;
			VisualDiagnostics.RegisterSourceInfo(numberPickerCell = new NumberPickerCell(), new Uri("Settings\\SettingsV3\\__SettingsCustomPIDEditorV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 76, 18);
			Translate translate14;
			VisualDiagnostics.RegisterSourceInfo(translate14 = new Translate(), new Uri("Settings\\SettingsV3\\__SettingsCustomPIDEditorV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 82, 21);
			BindingExtension bindingExtension14;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension14 = new BindingExtension(), new Uri("Settings\\SettingsV3\\__SettingsCustomPIDEditorV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 85, 21);
			NumberPickerCell numberPickerCell2;
			VisualDiagnostics.RegisterSourceInfo(numberPickerCell2 = new NumberPickerCell(), new Uri("Settings\\SettingsV3\\__SettingsCustomPIDEditorV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 81, 18);
			BindingExtension bindingExtension15;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension15 = new BindingExtension(), new Uri("Settings\\SettingsV3\\__SettingsCustomPIDEditorV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 86, 77);
			SettingsCheckBoxCellPatched settingsCheckBoxCellPatched;
			VisualDiagnostics.RegisterSourceInfo(settingsCheckBoxCellPatched = new SettingsCheckBoxCellPatched(), new Uri("Settings\\SettingsV3\\__SettingsCustomPIDEditorV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 86, 18);
			BindingExtension bindingExtension16;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension16 = new BindingExtension(), new Uri("Settings\\SettingsV3\\__SettingsCustomPIDEditorV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 87, 70);
			SettingsCheckBoxCellPatched settingsCheckBoxCellPatched2;
			VisualDiagnostics.RegisterSourceInfo(settingsCheckBoxCellPatched2 = new SettingsCheckBoxCellPatched(), new Uri("Settings\\SettingsV3\\__SettingsCustomPIDEditorV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 87, 18);
			Section section7;
			VisualDiagnostics.RegisterSourceInfo(section7 = new Section(), new Uri("Settings\\SettingsV3\\__SettingsCustomPIDEditorV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 75, 14);
			StaticResourceExtension staticResourceExtension6;
			VisualDiagnostics.RegisterSourceInfo(staticResourceExtension6 = new StaticResourceExtension(), new Uri("Settings\\SettingsV3\\__SettingsCustomPIDEditorV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 90, 25);
			BindingExtension bindingExtension17;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension17 = new BindingExtension(), new Uri("Settings\\SettingsV3\\__SettingsCustomPIDEditorV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 90, 25);
			BindingExtension bindingExtension18;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension18 = new BindingExtension(), new Uri("Settings\\SettingsV3\\__SettingsCustomPIDEditorV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 95, 21);
			EntryCell entryCell;
			VisualDiagnostics.RegisterSourceInfo(entryCell = new EntryCell(), new Uri("Settings\\SettingsV3\\__SettingsCustomPIDEditorV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 92, 18);
			BindingExtension bindingExtension19;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension19 = new BindingExtension(), new Uri("Settings\\SettingsV3\\__SettingsCustomPIDEditorV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 100, 21);
			EntryCell entryCell2;
			VisualDiagnostics.RegisterSourceInfo(entryCell2 = new EntryCell(), new Uri("Settings\\SettingsV3\\__SettingsCustomPIDEditorV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 97, 18);
			BindingExtension bindingExtension20;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension20 = new BindingExtension(), new Uri("Settings\\SettingsV3\\__SettingsCustomPIDEditorV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 105, 21);
			EntryCell entryCell3;
			VisualDiagnostics.RegisterSourceInfo(entryCell3 = new EntryCell(), new Uri("Settings\\SettingsV3\\__SettingsCustomPIDEditorV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 102, 18);
			Translate translate15;
			VisualDiagnostics.RegisterSourceInfo(translate15 = new Translate(), new Uri("Settings\\SettingsV3\\__SettingsCustomPIDEditorV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 109, 21);
			DynamicResourceExtension dynamicResourceExtension4;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension4 = new DynamicResourceExtension(), new Uri("Settings\\SettingsV3\\__SettingsCustomPIDEditorV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 111, 21);
			ButtonCell buttonCell;
			VisualDiagnostics.RegisterSourceInfo(buttonCell = new ButtonCell(), new Uri("Settings\\SettingsV3\\__SettingsCustomPIDEditorV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 107, 18);
			Section section8;
			VisualDiagnostics.RegisterSourceInfo(section8 = new Section(), new Uri("Settings\\SettingsV3\\__SettingsCustomPIDEditorV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 90, 14);
			Translate translate16;
			VisualDiagnostics.RegisterSourceInfo(translate16 = new Translate(), new Uri("Settings\\SettingsV3\\__SettingsCustomPIDEditorV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 115, 25);
			StaticResourceExtension staticResourceExtension7;
			VisualDiagnostics.RegisterSourceInfo(staticResourceExtension7 = new StaticResourceExtension(), new Uri("Settings\\SettingsV3\\__SettingsCustomPIDEditorV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 115, 71);
			BindingExtension bindingExtension21;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension21 = new BindingExtension(), new Uri("Settings\\SettingsV3\\__SettingsCustomPIDEditorV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 115, 71);
			BindingExtension bindingExtension22;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension22 = new BindingExtension(), new Uri("Settings\\SettingsV3\\__SettingsCustomPIDEditorV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 117, 40);
			NumericEntryV3 numericEntryV;
			VisualDiagnostics.RegisterSourceInfo(numericEntryV = new NumericEntryV3(), new Uri("Settings\\SettingsV3\\__SettingsCustomPIDEditorV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 117, 22);
			CustomCell customCell6;
			VisualDiagnostics.RegisterSourceInfo(customCell6 = new CustomCell(), new Uri("Settings\\SettingsV3\\__SettingsCustomPIDEditorV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 116, 18);
			Section section9;
			VisualDiagnostics.RegisterSourceInfo(section9 = new Section(), new Uri("Settings\\SettingsV3\\__SettingsCustomPIDEditorV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 115, 14);
			Translate translate17;
			VisualDiagnostics.RegisterSourceInfo(translate17 = new Translate(), new Uri("Settings\\SettingsV3\\__SettingsCustomPIDEditorV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 120, 25);
			StaticResourceExtension staticResourceExtension8;
			VisualDiagnostics.RegisterSourceInfo(staticResourceExtension8 = new StaticResourceExtension(), new Uri("Settings\\SettingsV3\\__SettingsCustomPIDEditorV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 120, 68);
			BindingExtension bindingExtension23;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension23 = new BindingExtension(), new Uri("Settings\\SettingsV3\\__SettingsCustomPIDEditorV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 120, 68);
			BindingExtension bindingExtension24;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension24 = new BindingExtension(), new Uri("Settings\\SettingsV3\\__SettingsCustomPIDEditorV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 122, 40);
			NumericEntryV3 numericEntryV2;
			VisualDiagnostics.RegisterSourceInfo(numericEntryV2 = new NumericEntryV3(), new Uri("Settings\\SettingsV3\\__SettingsCustomPIDEditorV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 122, 22);
			CustomCell customCell7;
			VisualDiagnostics.RegisterSourceInfo(customCell7 = new CustomCell(), new Uri("Settings\\SettingsV3\\__SettingsCustomPIDEditorV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 121, 18);
			Section section10;
			VisualDiagnostics.RegisterSourceInfo(section10 = new Section(), new Uri("Settings\\SettingsV3\\__SettingsCustomPIDEditorV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 120, 14);
			Translate translate18;
			VisualDiagnostics.RegisterSourceInfo(translate18 = new Translate(), new Uri("Settings\\SettingsV3\\__SettingsCustomPIDEditorV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 125, 25);
			StaticResourceExtension staticResourceExtension9;
			VisualDiagnostics.RegisterSourceInfo(staticResourceExtension9 = new StaticResourceExtension(), new Uri("Settings\\SettingsV3\\__SettingsCustomPIDEditorV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 125, 67);
			BindingExtension bindingExtension25;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension25 = new BindingExtension(), new Uri("Settings\\SettingsV3\\__SettingsCustomPIDEditorV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 125, 67);
			BindingExtension bindingExtension26;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension26 = new BindingExtension(), new Uri("Settings\\SettingsV3\\__SettingsCustomPIDEditorV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 130, 25);
			NumericEntryV3 numericEntryV3;
			VisualDiagnostics.RegisterSourceInfo(numericEntryV3 = new NumericEntryV3(), new Uri("Settings\\SettingsV3\\__SettingsCustomPIDEditorV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 127, 22);
			CustomCell customCell8;
			VisualDiagnostics.RegisterSourceInfo(customCell8 = new CustomCell(), new Uri("Settings\\SettingsV3\\__SettingsCustomPIDEditorV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 126, 18);
			Section section11;
			VisualDiagnostics.RegisterSourceInfo(section11 = new Section(), new Uri("Settings\\SettingsV3\\__SettingsCustomPIDEditorV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 125, 14);
			StaticResourceExtension staticResourceExtension10;
			VisualDiagnostics.RegisterSourceInfo(staticResourceExtension10 = new StaticResourceExtension(), new Uri("Settings\\SettingsV3\\__SettingsCustomPIDEditorV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 134, 25);
			BindingExtension bindingExtension27;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension27 = new BindingExtension(), new Uri("Settings\\SettingsV3\\__SettingsCustomPIDEditorV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 134, 25);
			Translate translate19;
			VisualDiagnostics.RegisterSourceInfo(translate19 = new Translate(), new Uri("Settings\\SettingsV3\\__SettingsCustomPIDEditorV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 136, 21);
			BindingExtension bindingExtension28;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension28 = new BindingExtension(), new Uri("Settings\\SettingsV3\\__SettingsCustomPIDEditorV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 139, 21);
			NumberPickerCell numberPickerCell3;
			VisualDiagnostics.RegisterSourceInfo(numberPickerCell3 = new NumberPickerCell(), new Uri("Settings\\SettingsV3\\__SettingsCustomPIDEditorV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 135, 18);
			BindingExtension bindingExtension29;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension29 = new BindingExtension(), new Uri("Settings\\SettingsV3\\__SettingsCustomPIDEditorV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 144, 21);
			NumberPickerCell numberPickerCell4;
			VisualDiagnostics.RegisterSourceInfo(numberPickerCell4 = new NumberPickerCell(), new Uri("Settings\\SettingsV3\\__SettingsCustomPIDEditorV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 140, 18);
			Section section12;
			VisualDiagnostics.RegisterSourceInfo(section12 = new Section(), new Uri("Settings\\SettingsV3\\__SettingsCustomPIDEditorV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 134, 14);
			Translate translate20;
			VisualDiagnostics.RegisterSourceInfo(translate20 = new Translate(), new Uri("Settings\\SettingsV3\\__SettingsCustomPIDEditorV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 147, 25);
			StaticResourceExtension staticResourceExtension11;
			VisualDiagnostics.RegisterSourceInfo(staticResourceExtension11 = new StaticResourceExtension(), new Uri("Settings\\SettingsV3\\__SettingsCustomPIDEditorV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 147, 82);
			BindingExtension bindingExtension30;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension30 = new BindingExtension(), new Uri("Settings\\SettingsV3\\__SettingsCustomPIDEditorV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 147, 82);
			BindingExtension bindingExtension31;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension31 = new BindingExtension(), new Uri("Settings\\SettingsV3\\__SettingsCustomPIDEditorV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 149, 40);
			NumericEntryV3 numericEntryV4;
			VisualDiagnostics.RegisterSourceInfo(numericEntryV4 = new NumericEntryV3(), new Uri("Settings\\SettingsV3\\__SettingsCustomPIDEditorV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 149, 22);
			CustomCell customCell9;
			VisualDiagnostics.RegisterSourceInfo(customCell9 = new CustomCell(), new Uri("Settings\\SettingsV3\\__SettingsCustomPIDEditorV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 148, 18);
			Section section13;
			VisualDiagnostics.RegisterSourceInfo(section13 = new Section(), new Uri("Settings\\SettingsV3\\__SettingsCustomPIDEditorV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 147, 14);
			Translate translate21;
			VisualDiagnostics.RegisterSourceInfo(translate21 = new Translate(), new Uri("Settings\\SettingsV3\\__SettingsCustomPIDEditorV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 153, 25);
			StaticResourceExtension staticResourceExtension12;
			VisualDiagnostics.RegisterSourceInfo(staticResourceExtension12 = new StaticResourceExtension(), new Uri("Settings\\SettingsV3\\__SettingsCustomPIDEditorV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 153, 82);
			BindingExtension bindingExtension32;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension32 = new BindingExtension(), new Uri("Settings\\SettingsV3\\__SettingsCustomPIDEditorV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 153, 82);
			BindingExtension bindingExtension33;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension33 = new BindingExtension(), new Uri("Settings\\SettingsV3\\__SettingsCustomPIDEditorV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 155, 40);
			NumericEntryV3 numericEntryV5;
			VisualDiagnostics.RegisterSourceInfo(numericEntryV5 = new NumericEntryV3(), new Uri("Settings\\SettingsV3\\__SettingsCustomPIDEditorV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 155, 22);
			CustomCell customCell10;
			VisualDiagnostics.RegisterSourceInfo(customCell10 = new CustomCell(), new Uri("Settings\\SettingsV3\\__SettingsCustomPIDEditorV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 154, 18);
			Section section14;
			VisualDiagnostics.RegisterSourceInfo(section14 = new Section(), new Uri("Settings\\SettingsV3\\__SettingsCustomPIDEditorV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 153, 14);
			Translate translate22;
			VisualDiagnostics.RegisterSourceInfo(translate22 = new Translate(), new Uri("Settings\\SettingsV3\\__SettingsCustomPIDEditorV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 159, 25);
			StaticResourceExtension staticResourceExtension13;
			VisualDiagnostics.RegisterSourceInfo(staticResourceExtension13 = new StaticResourceExtension(), new Uri("Settings\\SettingsV3\\__SettingsCustomPIDEditorV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 159, 66);
			BindingExtension bindingExtension34;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension34 = new BindingExtension(), new Uri("Settings\\SettingsV3\\__SettingsCustomPIDEditorV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 159, 66);
			List<string> units;
			VisualDiagnostics.RegisterSourceInfo(units = StaticLists.Units, new Uri("Settings\\SettingsV3\\__SettingsCustomPIDEditorV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 161, 29);
			BindingExtension bindingExtension35;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension35 = new BindingExtension(), new Uri("Settings\\SettingsV3\\__SettingsCustomPIDEditorV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 161, 29);
			StaticResourceExtension staticResourceExtension14;
			VisualDiagnostics.RegisterSourceInfo(staticResourceExtension14 = new StaticResourceExtension(), new Uri("Settings\\SettingsV3\\__SettingsCustomPIDEditorV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 161, 95);
			BindingExtension bindingExtension36;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension36 = new BindingExtension(), new Uri("Settings\\SettingsV3\\__SettingsCustomPIDEditorV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 161, 95);
			Picker picker;
			VisualDiagnostics.RegisterSourceInfo(picker = new Picker(), new Uri("Settings\\SettingsV3\\__SettingsCustomPIDEditorV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 161, 22);
			SettingsCustomCellForPicker settingsCustomCellForPicker;
			VisualDiagnostics.RegisterSourceInfo(settingsCustomCellForPicker = new SettingsCustomCellForPicker(), new Uri("Settings\\SettingsV3\\__SettingsCustomPIDEditorV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 160, 18);
			DynamicResourceExtension dynamicResourceExtension5;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension5 = new DynamicResourceExtension(), new Uri("Settings\\SettingsV3\\__SettingsCustomPIDEditorV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 168, 21);
			ButtonCell buttonCell2;
			VisualDiagnostics.RegisterSourceInfo(buttonCell2 = new ButtonCell(), new Uri("Settings\\SettingsV3\\__SettingsCustomPIDEditorV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 164, 18);
			Section section15;
			VisualDiagnostics.RegisterSourceInfo(section15 = new Section(), new Uri("Settings\\SettingsV3\\__SettingsCustomPIDEditorV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 159, 14);
			Translate translate23;
			VisualDiagnostics.RegisterSourceInfo(translate23 = new Translate(), new Uri("Settings\\SettingsV3\\__SettingsCustomPIDEditorV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 170, 25);
			StaticResourceExtension staticResourceExtension15;
			VisualDiagnostics.RegisterSourceInfo(staticResourceExtension15 = new StaticResourceExtension(), new Uri("Settings\\SettingsV3\\__SettingsCustomPIDEditorV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 172, 29);
			BindingExtension bindingExtension37;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension37 = new BindingExtension(), new Uri("Settings\\SettingsV3\\__SettingsCustomPIDEditorV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 172, 29);
			Type typeFromHandle;
			VisualDiagnostics.RegisterSourceInfo(typeFromHandle = typeof(string), new Uri("Settings\\SettingsV3\\__SettingsCustomPIDEditorV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 174, 38);
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
			}, new Uri("Settings\\SettingsV3\\__SettingsCustomPIDEditorV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 174, 30);
			Picker picker2;
			VisualDiagnostics.RegisterSourceInfo(picker2 = new Picker(), new Uri("Settings\\SettingsV3\\__SettingsCustomPIDEditorV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 172, 22);
			SettingsCustomCellForPicker settingsCustomCellForPicker2;
			VisualDiagnostics.RegisterSourceInfo(settingsCustomCellForPicker2 = new SettingsCustomCellForPicker(), new Uri("Settings\\SettingsV3\\__SettingsCustomPIDEditorV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 171, 18);
			Section section16;
			VisualDiagnostics.RegisterSourceInfo(section16 = new Section(), new Uri("Settings\\SettingsV3\\__SettingsCustomPIDEditorV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 170, 14);
			Translate translate24;
			VisualDiagnostics.RegisterSourceInfo(translate24 = new Translate(), new Uri("Settings\\SettingsV3\\__SettingsCustomPIDEditorV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 205, 25);
			StaticResourceExtension staticResourceExtension16;
			VisualDiagnostics.RegisterSourceInfo(staticResourceExtension16 = new StaticResourceExtension(), new Uri("Settings\\SettingsV3\\__SettingsCustomPIDEditorV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 207, 49);
			BindingExtension bindingExtension38;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension38 = new BindingExtension(), new Uri("Settings\\SettingsV3\\__SettingsCustomPIDEditorV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 207, 49);
			Picker picker3;
			VisualDiagnostics.RegisterSourceInfo(picker3 = new Picker(), new Uri("Settings\\SettingsV3\\__SettingsCustomPIDEditorV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 207, 22);
			SettingsCustomCellForPicker settingsCustomCellForPicker3;
			VisualDiagnostics.RegisterSourceInfo(settingsCustomCellForPicker3 = new SettingsCustomCellForPicker(), new Uri("Settings\\SettingsV3\\__SettingsCustomPIDEditorV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 206, 18);
			Section section17;
			VisualDiagnostics.RegisterSourceInfo(section17 = new Section(), new Uri("Settings\\SettingsV3\\__SettingsCustomPIDEditorV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 205, 14);
			Translate translate25;
			VisualDiagnostics.RegisterSourceInfo(translate25 = new Translate(), new Uri("Settings\\SettingsV3\\__SettingsCustomPIDEditorV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 211, 25);
			BindingExtension bindingExtension39;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension39 = new BindingExtension(), new Uri("Settings\\SettingsV3\\__SettingsCustomPIDEditorV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 213, 28);
			Entry entry6;
			VisualDiagnostics.RegisterSourceInfo(entry6 = new Entry(), new Uri("Settings\\SettingsV3\\__SettingsCustomPIDEditorV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 213, 22);
			CustomCell customCell11;
			VisualDiagnostics.RegisterSourceInfo(customCell11 = new CustomCell(), new Uri("Settings\\SettingsV3\\__SettingsCustomPIDEditorV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 212, 18);
			Section section18;
			VisualDiagnostics.RegisterSourceInfo(section18 = new Section(), new Uri("Settings\\SettingsV3\\__SettingsCustomPIDEditorV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 211, 14);
			Translate translate26;
			VisualDiagnostics.RegisterSourceInfo(translate26 = new Translate(), new Uri("Settings\\SettingsV3\\__SettingsCustomPIDEditorV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 217, 25);
			BindingExtension bindingExtension40;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension40 = new BindingExtension(), new Uri("Settings\\SettingsV3\\__SettingsCustomPIDEditorV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 219, 28);
			Entry entry7;
			VisualDiagnostics.RegisterSourceInfo(entry7 = new Entry(), new Uri("Settings\\SettingsV3\\__SettingsCustomPIDEditorV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 219, 22);
			CustomCell customCell12;
			VisualDiagnostics.RegisterSourceInfo(customCell12 = new CustomCell(), new Uri("Settings\\SettingsV3\\__SettingsCustomPIDEditorV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 218, 18);
			Section section19;
			VisualDiagnostics.RegisterSourceInfo(section19 = new Section(), new Uri("Settings\\SettingsV3\\__SettingsCustomPIDEditorV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 217, 14);
			DynamicResourceExtension dynamicResourceExtension6;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension6 = new DynamicResourceExtension(), new Uri("Settings\\SettingsV3\\__SettingsCustomPIDEditorV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 227, 21);
			ButtonCell buttonCell3;
			VisualDiagnostics.RegisterSourceInfo(buttonCell3 = new ButtonCell(), new Uri("Settings\\SettingsV3\\__SettingsCustomPIDEditorV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 224, 18);
			ReferenceExtension referenceExtension;
			VisualDiagnostics.RegisterSourceInfo(referenceExtension = new ReferenceExtension(), new Uri("Settings\\SettingsV3\\__SettingsCustomPIDEditorV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 230, 28);
			BindingExtension bindingExtension41;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension41 = new BindingExtension(), new Uri("Settings\\SettingsV3\\__SettingsCustomPIDEditorV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 230, 28);
			Label label;
			VisualDiagnostics.RegisterSourceInfo(label = new Label(), new Uri("Settings\\SettingsV3\\__SettingsCustomPIDEditorV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 230, 22);
			CustomCell customCell13;
			VisualDiagnostics.RegisterSourceInfo(customCell13 = new CustomCell(), new Uri("Settings\\SettingsV3\\__SettingsCustomPIDEditorV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 229, 18);
			Section section20;
			VisualDiagnostics.RegisterSourceInfo(section20 = new Section(), new Uri("Settings\\SettingsV3\\__SettingsCustomPIDEditorV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 223, 14);
			SettingsView settingsView;
			VisualDiagnostics.RegisterSourceInfo(settingsView = new SettingsView(), new Uri("Settings\\SettingsV3\\__SettingsCustomPIDEditorV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 32, 10);
			VisualDiagnostics.RegisterSourceInfo(this, new Uri("Settings\\SettingsV3\\__SettingsCustomPIDEditorV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 2, 2);
			NameScope nameScope = NameScope.GetNameScope(this) ?? new NameScope();
			NameScope.SetNameScope(this, nameScope);
			nameScope.RegisterName("settingsLayoutRoot", settingsView);
			if (settingsView.StyleId == null)
			{
				settingsView.StyleId = "settingsLayoutRoot";
			}
			nameScope.RegisterName("vagUnitEntry", entryCell);
			if (entryCell.StyleId == null)
			{
				entryCell.StyleId = "vagUnitEntry";
			}
			nameScope.RegisterName("vagGroupEntry", entryCell2);
			if (entryCell2.StyleId == null)
			{
				entryCell2.StyleId = "vagGroupEntry";
			}
			nameScope.RegisterName("vagItemInGroupEntry", entryCell3);
			if (entryCell3.StyleId == null)
			{
				entryCell3.StyleId = "vagItemInGroupEntry";
			}
			nameScope.RegisterName("btnApplyVagGroup", buttonCell);
			if (buttonCell.StyleId == null)
			{
				buttonCell.StyleId = "btnApplyVagGroup";
			}
			nameScope.RegisterName("btnUnitsOverride", buttonCell2);
			if (buttonCell2.StyleId == null)
			{
				buttonCell2.StyleId = "btnUnitsOverride";
			}
			nameScope.RegisterName("pickerRole", picker3);
			if (picker3.StyleId == null)
			{
				picker3.StyleId = "pickerRole";
			}
			nameScope.RegisterName("testLabelCell", customCell13);
			if (customCell13.StyleId == null)
			{
				customCell13.StyleId = "testLabelCell";
			}
			this.settingsLayoutRoot = settingsView;
			this.vagUnitEntry = entryCell;
			this.vagGroupEntry = entryCell2;
			this.vagItemInGroupEntry = entryCell3;
			this.btnApplyVagGroup = buttonCell;
			this.btnUnitsOverride = buttonCell2;
			this.pickerRole = picker3;
			this.testLabelCell = customCell13;
			this.Resources = resourceDictionary;
			resourceDictionary.Add("UnitsToIntConverter", unitsToIntConverter);
			resourceDictionary.Add("BoolToNegativeConverter", boolToNegativeConverter);
			resourceDictionary.Add("DoubleToStringConverter", doubleToStringConverter);
			resourceDictionary.Add("SkipCyclesIntToPriorityStringConverter", skipCyclesIntToPriorityStringConverter);
			resourceDictionary.Add("CustomPIDTypeFormulaToTrue", customPIDTypeFormulaToTrue);
			resourceDictionary.Add("EnumToIntConverter", enumToIntConverter);
			resourceDictionary.Add("EnumValueToTrueConverter", enumValueToTrueConverter);
			resourceDictionary.Add("EnumValueToFalseConverter", enumValueToFalseConverter);
			resourceDictionary.Add("UnitsToStringInvariantConverter", unitsToStringInvariantConverter);
			resourceDictionary.Add("UnitsToStringConverter", unitsToStringConverter);
			this.SetValue(Page.TitleProperty, "Custom PID editor");
			this.SetValue(Page.UseSafeAreaProperty, true);
			dynamicResourceExtension.Key = "BackgroundColor";
			IMarkupExtension<DynamicResource> markupExtension = dynamicResourceExtension;
			XamlServiceProvider xamlServiceProvider = new XamlServiceProvider();
			Type typeFromHandle2 = typeof(IProvideValueTarget);
			object[] array2 = new object[0 + 1];
			array2[0] = this;
			object obj;
			xamlServiceProvider.Add(typeFromHandle2, obj = new SimpleValueTargetProvider(array2, VisualElement.BackgroundColorProperty, nameScope));
			xamlServiceProvider.Add(typeof(IReferenceProvider), obj);
			Type typeFromHandle3 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver = new XmlNamespaceResolver();
			xmlNamespaceResolver.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver.Add("pids", "clr-namespace:CarScannerXamarinForms.OBD2.PIDS");
			xmlNamespaceResolver.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xmlNamespaceResolver.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider.Add(typeFromHandle3, new XamlTypeResolver(xmlNamespaceResolver, typeof(SettingsCustomPIDEditorV3).GetTypeInfo().Assembly));
			xamlServiceProvider.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(16, 5)));
			DynamicResource dynamicResource = markupExtension.ProvideValue(xamlServiceProvider);
			this.SetDynamicResource(VisualElement.BackgroundColorProperty, dynamicResource.Key);
			this.Resources = resourceDictionary;
			settingsView.SetValue(TableView.HasUnevenRowsProperty, true);
			translate.Text = "CustomPidEditor_tbName.Text";
			IMarkupExtension markupExtension2 = translate;
			XamlServiceProvider xamlServiceProvider2 = new XamlServiceProvider();
			Type typeFromHandle4 = typeof(IProvideValueTarget);
			object[] array3 = new object[0 + 3];
			array3[0] = section;
			array3[1] = settingsView;
			array3[2] = this;
			object obj2;
			xamlServiceProvider2.Add(typeFromHandle4, obj2 = new SimpleValueTargetProvider(array3, SectionBase.TitleProperty, nameScope));
			xamlServiceProvider2.Add(typeof(IReferenceProvider), obj2);
			Type typeFromHandle5 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver2 = new XmlNamespaceResolver();
			xmlNamespaceResolver2.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver2.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver2.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver2.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver2.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver2.Add("pids", "clr-namespace:CarScannerXamarinForms.OBD2.PIDS");
			xmlNamespaceResolver2.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver2.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xmlNamespaceResolver2.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver2.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider2.Add(typeFromHandle5, new XamlTypeResolver(xmlNamespaceResolver2, typeof(SettingsCustomPIDEditorV3).GetTypeInfo().Assembly));
			xamlServiceProvider2.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(33, 25)));
			object obj3 = markupExtension2.ProvideValue(xamlServiceProvider2);
			section.Title = obj3;
			customCell.SetValue(CustomCell.IsSelectableProperty, false);
			bindingExtension.Mode = 1;
			bindingExtension.Path = "Name";
			BindingBase bindingBase = bindingExtension.ProvideValue(null);
			entry.SetBinding(Entry.TextProperty, bindingBase);
			customCell.SetValue(CustomCell.ContentProperty, entry);
			section.Add(customCell);
			settingsView.Root.Add(section);
			translate2.Text = "CustomPidEditor_tbShortName.Text";
			IMarkupExtension markupExtension3 = translate2;
			XamlServiceProvider xamlServiceProvider3 = new XamlServiceProvider();
			Type typeFromHandle6 = typeof(IProvideValueTarget);
			object[] array4 = new object[0 + 3];
			array4[0] = section2;
			array4[1] = settingsView;
			array4[2] = this;
			object obj4;
			xamlServiceProvider3.Add(typeFromHandle6, obj4 = new SimpleValueTargetProvider(array4, SectionBase.TitleProperty, nameScope));
			xamlServiceProvider3.Add(typeof(IReferenceProvider), obj4);
			Type typeFromHandle7 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver3 = new XmlNamespaceResolver();
			xmlNamespaceResolver3.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver3.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver3.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver3.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver3.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver3.Add("pids", "clr-namespace:CarScannerXamarinForms.OBD2.PIDS");
			xmlNamespaceResolver3.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver3.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xmlNamespaceResolver3.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver3.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider3.Add(typeFromHandle7, new XamlTypeResolver(xmlNamespaceResolver3, typeof(SettingsCustomPIDEditorV3).GetTypeInfo().Assembly));
			xamlServiceProvider3.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(38, 25)));
			object obj5 = markupExtension3.ProvideValue(xamlServiceProvider3);
			section2.Title = obj5;
			customCell2.SetValue(CustomCell.IsSelectableProperty, false);
			bindingExtension2.Mode = 1;
			bindingExtension2.Path = "ShortName";
			BindingBase bindingBase2 = bindingExtension2.ProvideValue(null);
			entry2.SetBinding(Entry.TextProperty, bindingBase2);
			customCell2.SetValue(CustomCell.ContentProperty, entry2);
			section2.Add(customCell2);
			settingsView.Root.Add(section2);
			translate3.Text = "CustomPidEditor_tbCommand.Text";
			IMarkupExtension markupExtension4 = translate3;
			XamlServiceProvider xamlServiceProvider4 = new XamlServiceProvider();
			Type typeFromHandle8 = typeof(IProvideValueTarget);
			object[] array5 = new object[0 + 3];
			array5[0] = section3;
			array5[1] = settingsView;
			array5[2] = this;
			object obj6;
			xamlServiceProvider4.Add(typeFromHandle8, obj6 = new SimpleValueTargetProvider(array5, SectionBase.TitleProperty, nameScope));
			xamlServiceProvider4.Add(typeof(IReferenceProvider), obj6);
			Type typeFromHandle9 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver4 = new XmlNamespaceResolver();
			xmlNamespaceResolver4.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver4.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver4.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver4.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver4.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver4.Add("pids", "clr-namespace:CarScannerXamarinForms.OBD2.PIDS");
			xmlNamespaceResolver4.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver4.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xmlNamespaceResolver4.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver4.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider4.Add(typeFromHandle9, new XamlTypeResolver(xmlNamespaceResolver4, typeof(SettingsCustomPIDEditorV3).GetTypeInfo().Assembly));
			xamlServiceProvider4.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(43, 25)));
			object obj7 = markupExtension4.ProvideValue(xamlServiceProvider4);
			section3.Title = obj7;
			bindingExtension3.Mode = 2;
			staticResourceExtension.Key = "BoolToNegativeConverter";
			IMarkupExtension markupExtension5 = staticResourceExtension;
			XamlServiceProvider xamlServiceProvider5 = new XamlServiceProvider();
			Type typeFromHandle10 = typeof(IProvideValueTarget);
			object[] array6 = new object[0 + 4];
			array6[0] = bindingExtension3;
			array6[1] = section3;
			array6[2] = settingsView;
			array6[3] = this;
			object obj8;
			xamlServiceProvider5.Add(typeFromHandle10, obj8 = new SimpleValueTargetProvider(array6, typeof(BindingExtension).GetRuntimeProperty("Converter"), nameScope));
			xamlServiceProvider5.Add(typeof(IReferenceProvider), obj8);
			Type typeFromHandle11 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver5 = new XmlNamespaceResolver();
			xmlNamespaceResolver5.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver5.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver5.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver5.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver5.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver5.Add("pids", "clr-namespace:CarScannerXamarinForms.OBD2.PIDS");
			xmlNamespaceResolver5.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver5.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xmlNamespaceResolver5.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver5.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider5.Add(typeFromHandle11, new XamlTypeResolver(xmlNamespaceResolver5, typeof(SettingsCustomPIDEditorV3).GetTypeInfo().Assembly));
			xamlServiceProvider5.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(43, 87)));
			object obj9 = markupExtension5.ProvideValue(xamlServiceProvider5);
			bindingExtension3.Converter = obj9;
			bindingExtension3.Path = "IsFormulaHidden";
			BindingBase bindingBase3 = bindingExtension3.ProvideValue(null);
			section3.SetBinding(Section.IsVisibleProperty, bindingBase3);
			customCell3.SetValue(CustomCell.IsSelectableProperty, false);
			bindingExtension4.Mode = 1;
			bindingExtension4.Path = "Command";
			BindingBase bindingBase4 = bindingExtension4.ProvideValue(null);
			entry3.SetBinding(Entry.TextProperty, bindingBase4);
			customCell3.SetValue(CustomCell.ContentProperty, entry3);
			section3.Add(customCell3);
			settingsView.Root.Add(section3);
			translate4.Text = "CustomPidEditor_tbHeader.Text";
			IMarkupExtension markupExtension6 = translate4;
			XamlServiceProvider xamlServiceProvider6 = new XamlServiceProvider();
			Type typeFromHandle12 = typeof(IProvideValueTarget);
			object[] array7 = new object[0 + 3];
			array7[0] = section4;
			array7[1] = settingsView;
			array7[2] = this;
			object obj10;
			xamlServiceProvider6.Add(typeFromHandle12, obj10 = new SimpleValueTargetProvider(array7, SectionBase.TitleProperty, nameScope));
			xamlServiceProvider6.Add(typeof(IReferenceProvider), obj10);
			Type typeFromHandle13 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver6 = new XmlNamespaceResolver();
			xmlNamespaceResolver6.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver6.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver6.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver6.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver6.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver6.Add("pids", "clr-namespace:CarScannerXamarinForms.OBD2.PIDS");
			xmlNamespaceResolver6.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver6.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xmlNamespaceResolver6.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver6.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider6.Add(typeFromHandle13, new XamlTypeResolver(xmlNamespaceResolver6, typeof(SettingsCustomPIDEditorV3).GetTypeInfo().Assembly));
			xamlServiceProvider6.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(48, 25)));
			object obj11 = markupExtension6.ProvideValue(xamlServiceProvider6);
			section4.Title = obj11;
			bindingExtension5.Mode = 2;
			staticResourceExtension2.Key = "BoolToNegativeConverter";
			IMarkupExtension markupExtension7 = staticResourceExtension2;
			XamlServiceProvider xamlServiceProvider7 = new XamlServiceProvider();
			Type typeFromHandle14 = typeof(IProvideValueTarget);
			object[] array8 = new object[0 + 4];
			array8[0] = bindingExtension5;
			array8[1] = section4;
			array8[2] = settingsView;
			array8[3] = this;
			object obj12;
			xamlServiceProvider7.Add(typeFromHandle14, obj12 = new SimpleValueTargetProvider(array8, typeof(BindingExtension).GetRuntimeProperty("Converter"), nameScope));
			xamlServiceProvider7.Add(typeof(IReferenceProvider), obj12);
			Type typeFromHandle15 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver7 = new XmlNamespaceResolver();
			xmlNamespaceResolver7.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver7.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver7.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver7.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver7.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver7.Add("pids", "clr-namespace:CarScannerXamarinForms.OBD2.PIDS");
			xmlNamespaceResolver7.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver7.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xmlNamespaceResolver7.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver7.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider7.Add(typeFromHandle15, new XamlTypeResolver(xmlNamespaceResolver7, typeof(SettingsCustomPIDEditorV3).GetTypeInfo().Assembly));
			xamlServiceProvider7.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(48, 86)));
			object obj13 = markupExtension7.ProvideValue(xamlServiceProvider7);
			bindingExtension5.Converter = obj13;
			bindingExtension5.Path = "IsFormulaHidden";
			BindingBase bindingBase5 = bindingExtension5.ProvideValue(null);
			section4.SetBinding(Section.IsVisibleProperty, bindingBase5);
			customCell4.SetValue(CustomCell.IsSelectableProperty, false);
			bindingExtension6.Mode = 1;
			bindingExtension6.Path = "Header";
			BindingBase bindingBase6 = bindingExtension6.ProvideValue(null);
			entry4.SetBinding(Entry.TextProperty, bindingBase6);
			customCell4.SetValue(CustomCell.ContentProperty, entry4);
			section4.Add(customCell4);
			settingsView.Root.Add(section4);
			translate5.Text = "ios_CustomPIDEditor_DecodeType";
			IMarkupExtension markupExtension8 = translate5;
			XamlServiceProvider xamlServiceProvider8 = new XamlServiceProvider();
			Type typeFromHandle16 = typeof(IProvideValueTarget);
			object[] array9 = new object[0 + 3];
			array9[0] = section5;
			array9[1] = settingsView;
			array9[2] = this;
			object obj14;
			xamlServiceProvider8.Add(typeFromHandle16, obj14 = new SimpleValueTargetProvider(array9, SectionBase.TitleProperty, nameScope));
			xamlServiceProvider8.Add(typeof(IReferenceProvider), obj14);
			Type typeFromHandle17 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver8 = new XmlNamespaceResolver();
			xmlNamespaceResolver8.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver8.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver8.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver8.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver8.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver8.Add("pids", "clr-namespace:CarScannerXamarinForms.OBD2.PIDS");
			xmlNamespaceResolver8.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver8.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xmlNamespaceResolver8.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver8.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider8.Add(typeFromHandle17, new XamlTypeResolver(xmlNamespaceResolver8, typeof(SettingsCustomPIDEditorV3).GetTypeInfo().Assembly));
			xamlServiceProvider8.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(53, 25)));
			object obj15 = markupExtension8.ProvideValue(xamlServiceProvider8);
			section5.Title = obj15;
			bindingExtension7.Mode = 1;
			bindingExtension7.Path = "Type";
			BindingBase bindingBase7 = bindingExtension7.ProvideValue(null);
			section5.SetBinding(RadioCell.SelectedValueProperty, bindingBase7);
			translate6.Text = "CustomPidEditor_tbFormula.Text";
			IMarkupExtension markupExtension9 = translate6;
			XamlServiceProvider xamlServiceProvider9 = new XamlServiceProvider();
			Type typeFromHandle18 = typeof(IProvideValueTarget);
			object[] array10 = new object[0 + 4];
			array10[0] = radioCell;
			array10[1] = section5;
			array10[2] = settingsView;
			array10[3] = this;
			object obj16;
			xamlServiceProvider9.Add(typeFromHandle18, obj16 = new SimpleValueTargetProvider(array10, CellBase.TitleProperty, nameScope));
			xamlServiceProvider9.Add(typeof(IReferenceProvider), obj16);
			Type typeFromHandle19 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver9 = new XmlNamespaceResolver();
			xmlNamespaceResolver9.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver9.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver9.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver9.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver9.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver9.Add("pids", "clr-namespace:CarScannerXamarinForms.OBD2.PIDS");
			xmlNamespaceResolver9.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver9.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xmlNamespaceResolver9.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver9.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider9.Add(typeFromHandle19, new XamlTypeResolver(xmlNamespaceResolver9, typeof(SettingsCustomPIDEditorV3).GetTypeInfo().Assembly));
			xamlServiceProvider9.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(54, 31)));
			object obj17 = markupExtension9.ProvideValue(xamlServiceProvider9);
			radioCell.Title = obj17;
			radioCell.SetValue(RadioCell.ValueProperty, customPIDType);
			section5.Add(radioCell);
			translate7.Text = "ios_CustomPIDEditor_ByteSet";
			IMarkupExtension markupExtension10 = translate7;
			XamlServiceProvider xamlServiceProvider10 = new XamlServiceProvider();
			Type typeFromHandle20 = typeof(IProvideValueTarget);
			object[] array11 = new object[0 + 4];
			array11[0] = radioCell2;
			array11[1] = section5;
			array11[2] = settingsView;
			array11[3] = this;
			object obj18;
			xamlServiceProvider10.Add(typeFromHandle20, obj18 = new SimpleValueTargetProvider(array11, CellBase.TitleProperty, nameScope));
			xamlServiceProvider10.Add(typeof(IReferenceProvider), obj18);
			Type typeFromHandle21 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver10 = new XmlNamespaceResolver();
			xmlNamespaceResolver10.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver10.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver10.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver10.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver10.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver10.Add("pids", "clr-namespace:CarScannerXamarinForms.OBD2.PIDS");
			xmlNamespaceResolver10.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver10.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xmlNamespaceResolver10.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver10.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider10.Add(typeFromHandle21, new XamlTypeResolver(xmlNamespaceResolver10, typeof(SettingsCustomPIDEditorV3).GetTypeInfo().Assembly));
			xamlServiceProvider10.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(55, 31)));
			object obj19 = markupExtension10.ProvideValue(xamlServiceProvider10);
			radioCell2.Title = obj19;
			radioCell2.SetValue(RadioCell.ValueProperty, customPIDType2);
			section5.Add(radioCell2);
			translate8.Text = "ios_Bit";
			IMarkupExtension markupExtension11 = translate8;
			XamlServiceProvider xamlServiceProvider11 = new XamlServiceProvider();
			Type typeFromHandle22 = typeof(IProvideValueTarget);
			object[] array12 = new object[0 + 4];
			array12[0] = radioCell3;
			array12[1] = section5;
			array12[2] = settingsView;
			array12[3] = this;
			object obj20;
			xamlServiceProvider11.Add(typeFromHandle22, obj20 = new SimpleValueTargetProvider(array12, CellBase.TitleProperty, nameScope));
			xamlServiceProvider11.Add(typeof(IReferenceProvider), obj20);
			Type typeFromHandle23 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver11 = new XmlNamespaceResolver();
			xmlNamespaceResolver11.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver11.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver11.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver11.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver11.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver11.Add("pids", "clr-namespace:CarScannerXamarinForms.OBD2.PIDS");
			xmlNamespaceResolver11.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver11.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xmlNamespaceResolver11.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver11.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider11.Add(typeFromHandle23, new XamlTypeResolver(xmlNamespaceResolver11, typeof(SettingsCustomPIDEditorV3).GetTypeInfo().Assembly));
			xamlServiceProvider11.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(56, 31)));
			object obj21 = markupExtension11.ProvideValue(xamlServiceProvider11);
			radioCell3.Title = obj21;
			radioCell3.SetValue(RadioCell.ValueProperty, customPIDType3);
			section5.Add(radioCell3);
			translate9.Text = "ios_CustomPIDEditor_IsAction";
			IMarkupExtension markupExtension12 = translate9;
			XamlServiceProvider xamlServiceProvider12 = new XamlServiceProvider();
			Type typeFromHandle24 = typeof(IProvideValueTarget);
			object[] array13 = new object[0 + 4];
			array13[0] = radioCell4;
			array13[1] = section5;
			array13[2] = settingsView;
			array13[3] = this;
			object obj22;
			xamlServiceProvider12.Add(typeFromHandle24, obj22 = new SimpleValueTargetProvider(array13, CellBase.TitleProperty, nameScope));
			xamlServiceProvider12.Add(typeof(IReferenceProvider), obj22);
			Type typeFromHandle25 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver12 = new XmlNamespaceResolver();
			xmlNamespaceResolver12.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver12.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver12.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver12.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver12.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver12.Add("pids", "clr-namespace:CarScannerXamarinForms.OBD2.PIDS");
			xmlNamespaceResolver12.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver12.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xmlNamespaceResolver12.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver12.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider12.Add(typeFromHandle25, new XamlTypeResolver(xmlNamespaceResolver12, typeof(SettingsCustomPIDEditorV3).GetTypeInfo().Assembly));
			xamlServiceProvider12.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(57, 31)));
			object obj23 = markupExtension12.ProvideValue(xamlServiceProvider12);
			radioCell4.Title = obj23;
			radioCell4.SetValue(RadioCell.ValueProperty, customPIDType4);
			section5.Add(radioCell4);
			radioCell5.SetValue(CellBase.TitleProperty, "VW TP2.0 Group Item");
			radioCell5.SetValue(RadioCell.ValueProperty, customPIDType5);
			section5.Add(radioCell5);
			settingsView.Root.Add(section5);
			translate10.Text = "CustomPidEditor_tbFormula.Text";
			IMarkupExtension markupExtension13 = translate10;
			XamlServiceProvider xamlServiceProvider13 = new XamlServiceProvider();
			Type typeFromHandle26 = typeof(IProvideValueTarget);
			object[] array14 = new object[0 + 3];
			array14[0] = section6;
			array14[1] = settingsView;
			array14[2] = this;
			object obj24;
			xamlServiceProvider13.Add(typeFromHandle26, obj24 = new SimpleValueTargetProvider(array14, SectionBase.TitleProperty, nameScope));
			xamlServiceProvider13.Add(typeof(IReferenceProvider), obj24);
			Type typeFromHandle27 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver13 = new XmlNamespaceResolver();
			xmlNamespaceResolver13.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver13.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver13.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver13.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver13.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver13.Add("pids", "clr-namespace:CarScannerXamarinForms.OBD2.PIDS");
			xmlNamespaceResolver13.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver13.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xmlNamespaceResolver13.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver13.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider13.Add(typeFromHandle27, new XamlTypeResolver(xmlNamespaceResolver13, typeof(SettingsCustomPIDEditorV3).GetTypeInfo().Assembly));
			xamlServiceProvider13.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(61, 25)));
			object obj25 = markupExtension13.ProvideValue(xamlServiceProvider13);
			section6.Title = obj25;
			bindingExtension8.Path = "Type";
			bindingExtension8.Mode = 2;
			staticResourceExtension3.Key = "EnumValueToTrueConverter";
			IMarkupExtension markupExtension14 = staticResourceExtension3;
			XamlServiceProvider xamlServiceProvider14 = new XamlServiceProvider();
			Type typeFromHandle28 = typeof(IProvideValueTarget);
			object[] array15 = new object[0 + 4];
			array15[0] = bindingExtension8;
			array15[1] = section6;
			array15[2] = settingsView;
			array15[3] = this;
			object obj26;
			xamlServiceProvider14.Add(typeFromHandle28, obj26 = new SimpleValueTargetProvider(array15, typeof(BindingExtension).GetRuntimeProperty("Converter"), nameScope));
			xamlServiceProvider14.Add(typeof(IReferenceProvider), obj26);
			Type typeFromHandle29 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver14 = new XmlNamespaceResolver();
			xmlNamespaceResolver14.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver14.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver14.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver14.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver14.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver14.Add("pids", "clr-namespace:CarScannerXamarinForms.OBD2.PIDS");
			xmlNamespaceResolver14.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver14.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xmlNamespaceResolver14.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver14.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider14.Add(typeFromHandle29, new XamlTypeResolver(xmlNamespaceResolver14, typeof(SettingsCustomPIDEditorV3).GetTypeInfo().Assembly));
			xamlServiceProvider14.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(61, 87)));
			object obj27 = markupExtension14.ProvideValue(xamlServiceProvider14);
			bindingExtension8.Converter = obj27;
			bindingExtension8.ConverterParameter = "0";
			BindingBase bindingBase8 = bindingExtension8.ProvideValue(null);
			section6.SetBinding(Section.IsVisibleProperty, bindingBase8);
			customCell5.SetValue(CustomCell.IsSelectableProperty, false);
			bindingExtension9.Mode = 1;
			bindingExtension9.Path = "Formula";
			BindingBase bindingBase9 = bindingExtension9.ProvideValue(null);
			entry5.SetBinding(Entry.TextProperty, bindingBase9);
			customCell5.SetValue(CustomCell.ContentProperty, entry5);
			section6.Add(customCell5);
			translate11.Text = "CustomPidEditor_tbOK.Text";
			IMarkupExtension markupExtension15 = translate11;
			XamlServiceProvider xamlServiceProvider15 = new XamlServiceProvider();
			Type typeFromHandle30 = typeof(IProvideValueTarget);
			object[] array16 = new object[0 + 4];
			array16[0] = labelCell;
			array16[1] = section6;
			array16[2] = settingsView;
			array16[3] = this;
			object obj28;
			xamlServiceProvider15.Add(typeFromHandle30, obj28 = new SimpleValueTargetProvider(array16, CellBase.TitleProperty, nameScope));
			xamlServiceProvider15.Add(typeof(IReferenceProvider), obj28);
			Type typeFromHandle31 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver15 = new XmlNamespaceResolver();
			xmlNamespaceResolver15.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver15.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver15.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver15.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver15.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver15.Add("pids", "clr-namespace:CarScannerXamarinForms.OBD2.PIDS");
			xmlNamespaceResolver15.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver15.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xmlNamespaceResolver15.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver15.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider15.Add(typeFromHandle31, new XamlTypeResolver(xmlNamespaceResolver15, typeof(SettingsCustomPIDEditorV3).GetTypeInfo().Assembly));
			xamlServiceProvider15.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(66, 21)));
			object obj29 = markupExtension15.ProvideValue(xamlServiceProvider15);
			labelCell.Title = obj29;
			bindingExtension10.Mode = 2;
			bindingExtension10.Path = "IsFormulaCorrect";
			BindingBase bindingBase10 = bindingExtension10.ProvideValue(null);
			labelCell.SetBinding(CellBase.IsVisibleProperty, bindingBase10);
			dynamicResourceExtension2.Key = "GreenTextColor";
			IMarkupExtension<DynamicResource> markupExtension16 = dynamicResourceExtension2;
			XamlServiceProvider xamlServiceProvider16 = new XamlServiceProvider();
			Type typeFromHandle32 = typeof(IProvideValueTarget);
			object[] array17 = new object[0 + 4];
			array17[0] = labelCell;
			array17[1] = section6;
			array17[2] = settingsView;
			array17[3] = this;
			object obj30;
			xamlServiceProvider16.Add(typeFromHandle32, obj30 = new SimpleValueTargetProvider(array17, CellBase.TitleColorProperty, nameScope));
			xamlServiceProvider16.Add(typeof(IReferenceProvider), obj30);
			Type typeFromHandle33 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver16 = new XmlNamespaceResolver();
			xmlNamespaceResolver16.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver16.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver16.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver16.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver16.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver16.Add("pids", "clr-namespace:CarScannerXamarinForms.OBD2.PIDS");
			xmlNamespaceResolver16.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver16.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xmlNamespaceResolver16.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver16.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider16.Add(typeFromHandle33, new XamlTypeResolver(xmlNamespaceResolver16, typeof(SettingsCustomPIDEditorV3).GetTypeInfo().Assembly));
			xamlServiceProvider16.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(68, 21)));
			DynamicResource dynamicResource2 = markupExtension16.ProvideValue(xamlServiceProvider16);
			labelCell.SetDynamicResource(CellBase.TitleColorProperty, dynamicResource2.Key);
			section6.Add(labelCell);
			translate12.Text = "CustomPidEditor_tbError.Text";
			IMarkupExtension markupExtension17 = translate12;
			XamlServiceProvider xamlServiceProvider17 = new XamlServiceProvider();
			Type typeFromHandle34 = typeof(IProvideValueTarget);
			object[] array18 = new object[0 + 4];
			array18[0] = labelCell2;
			array18[1] = section6;
			array18[2] = settingsView;
			array18[3] = this;
			object obj31;
			xamlServiceProvider17.Add(typeFromHandle34, obj31 = new SimpleValueTargetProvider(array18, CellBase.TitleProperty, nameScope));
			xamlServiceProvider17.Add(typeof(IReferenceProvider), obj31);
			Type typeFromHandle35 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver17 = new XmlNamespaceResolver();
			xmlNamespaceResolver17.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver17.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver17.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver17.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver17.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver17.Add("pids", "clr-namespace:CarScannerXamarinForms.OBD2.PIDS");
			xmlNamespaceResolver17.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver17.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xmlNamespaceResolver17.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver17.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider17.Add(typeFromHandle35, new XamlTypeResolver(xmlNamespaceResolver17, typeof(SettingsCustomPIDEditorV3).GetTypeInfo().Assembly));
			xamlServiceProvider17.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(70, 21)));
			object obj32 = markupExtension17.ProvideValue(xamlServiceProvider17);
			labelCell2.Title = obj32;
			bindingExtension11.Mode = 2;
			staticResourceExtension4.Key = "BoolToNegativeConverter";
			IMarkupExtension markupExtension18 = staticResourceExtension4;
			XamlServiceProvider xamlServiceProvider18 = new XamlServiceProvider();
			Type typeFromHandle36 = typeof(IProvideValueTarget);
			object[] array19 = new object[0 + 5];
			array19[0] = bindingExtension11;
			array19[1] = labelCell2;
			array19[2] = section6;
			array19[3] = settingsView;
			array19[4] = this;
			object obj33;
			xamlServiceProvider18.Add(typeFromHandle36, obj33 = new SimpleValueTargetProvider(array19, typeof(BindingExtension).GetRuntimeProperty("Converter"), nameScope));
			xamlServiceProvider18.Add(typeof(IReferenceProvider), obj33);
			Type typeFromHandle37 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver18 = new XmlNamespaceResolver();
			xmlNamespaceResolver18.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver18.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver18.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver18.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver18.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver18.Add("pids", "clr-namespace:CarScannerXamarinForms.OBD2.PIDS");
			xmlNamespaceResolver18.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver18.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xmlNamespaceResolver18.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver18.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider18.Add(typeFromHandle37, new XamlTypeResolver(xmlNamespaceResolver18, typeof(SettingsCustomPIDEditorV3).GetTypeInfo().Assembly));
			xamlServiceProvider18.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(71, 21)));
			object obj34 = markupExtension18.ProvideValue(xamlServiceProvider18);
			bindingExtension11.Converter = obj34;
			bindingExtension11.Path = "IsFormulaCorrect";
			BindingBase bindingBase11 = bindingExtension11.ProvideValue(null);
			labelCell2.SetBinding(CellBase.IsVisibleProperty, bindingBase11);
			dynamicResourceExtension3.Key = "RedTextColor";
			IMarkupExtension<DynamicResource> markupExtension19 = dynamicResourceExtension3;
			XamlServiceProvider xamlServiceProvider19 = new XamlServiceProvider();
			Type typeFromHandle38 = typeof(IProvideValueTarget);
			object[] array20 = new object[0 + 4];
			array20[0] = labelCell2;
			array20[1] = section6;
			array20[2] = settingsView;
			array20[3] = this;
			object obj35;
			xamlServiceProvider19.Add(typeFromHandle38, obj35 = new SimpleValueTargetProvider(array20, CellBase.TitleColorProperty, nameScope));
			xamlServiceProvider19.Add(typeof(IReferenceProvider), obj35);
			Type typeFromHandle39 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver19 = new XmlNamespaceResolver();
			xmlNamespaceResolver19.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver19.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver19.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver19.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver19.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver19.Add("pids", "clr-namespace:CarScannerXamarinForms.OBD2.PIDS");
			xmlNamespaceResolver19.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver19.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xmlNamespaceResolver19.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver19.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider19.Add(typeFromHandle39, new XamlTypeResolver(xmlNamespaceResolver19, typeof(SettingsCustomPIDEditorV3).GetTypeInfo().Assembly));
			xamlServiceProvider19.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(72, 21)));
			DynamicResource dynamicResource3 = markupExtension19.ProvideValue(xamlServiceProvider19);
			labelCell2.SetDynamicResource(CellBase.TitleColorProperty, dynamicResource3.Key);
			section6.Add(labelCell2);
			settingsView.Root.Add(section6);
			bindingExtension12.Path = "Type";
			bindingExtension12.Mode = 2;
			staticResourceExtension5.Key = "EnumValueToTrueConverter";
			IMarkupExtension markupExtension20 = staticResourceExtension5;
			XamlServiceProvider xamlServiceProvider20 = new XamlServiceProvider();
			Type typeFromHandle40 = typeof(IProvideValueTarget);
			object[] array21 = new object[0 + 4];
			array21[0] = bindingExtension12;
			array21[1] = section7;
			array21[2] = settingsView;
			array21[3] = this;
			object obj36;
			xamlServiceProvider20.Add(typeFromHandle40, obj36 = new SimpleValueTargetProvider(array21, typeof(BindingExtension).GetRuntimeProperty("Converter"), nameScope));
			xamlServiceProvider20.Add(typeof(IReferenceProvider), obj36);
			Type typeFromHandle41 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver20 = new XmlNamespaceResolver();
			xmlNamespaceResolver20.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver20.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver20.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver20.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver20.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver20.Add("pids", "clr-namespace:CarScannerXamarinForms.OBD2.PIDS");
			xmlNamespaceResolver20.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver20.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xmlNamespaceResolver20.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver20.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider20.Add(typeFromHandle41, new XamlTypeResolver(xmlNamespaceResolver20, typeof(SettingsCustomPIDEditorV3).GetTypeInfo().Assembly));
			xamlServiceProvider20.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(75, 25)));
			object obj37 = markupExtension20.ProvideValue(xamlServiceProvider20);
			bindingExtension12.Converter = obj37;
			bindingExtension12.ConverterParameter = "1";
			BindingBase bindingBase12 = bindingExtension12.ProvideValue(null);
			section7.SetBinding(Section.IsVisibleProperty, bindingBase12);
			translate13.Text = "ios_Byte";
			IMarkupExtension markupExtension21 = translate13;
			XamlServiceProvider xamlServiceProvider21 = new XamlServiceProvider();
			Type typeFromHandle42 = typeof(IProvideValueTarget);
			object[] array22 = new object[0 + 4];
			array22[0] = numberPickerCell;
			array22[1] = section7;
			array22[2] = settingsView;
			array22[3] = this;
			object obj38;
			xamlServiceProvider21.Add(typeFromHandle42, obj38 = new SimpleValueTargetProvider(array22, CellBase.TitleProperty, nameScope));
			xamlServiceProvider21.Add(typeof(IReferenceProvider), obj38);
			Type typeFromHandle43 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver21 = new XmlNamespaceResolver();
			xmlNamespaceResolver21.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver21.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver21.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver21.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver21.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver21.Add("pids", "clr-namespace:CarScannerXamarinForms.OBD2.PIDS");
			xmlNamespaceResolver21.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver21.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xmlNamespaceResolver21.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver21.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider21.Add(typeFromHandle43, new XamlTypeResolver(xmlNamespaceResolver21, typeof(SettingsCustomPIDEditorV3).GetTypeInfo().Assembly));
			xamlServiceProvider21.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(77, 21)));
			object obj39 = markupExtension21.ProvideValue(xamlServiceProvider21);
			numberPickerCell.Title = obj39;
			numberPickerCell.SetValue(NumberPickerCell.MaxProperty, 4096);
			numberPickerCell.SetValue(NumberPickerCell.MinProperty, 0);
			bindingExtension13.Mode = 1;
			bindingExtension13.Path = "StartByteId";
			BindingBase bindingBase13 = bindingExtension13.ProvideValue(null);
			numberPickerCell.SetBinding(NumberPickerCell.NumberProperty, bindingBase13);
			section7.Add(numberPickerCell);
			translate14.Text = "ios_DataLength";
			IMarkupExtension markupExtension22 = translate14;
			XamlServiceProvider xamlServiceProvider22 = new XamlServiceProvider();
			Type typeFromHandle44 = typeof(IProvideValueTarget);
			object[] array23 = new object[0 + 4];
			array23[0] = numberPickerCell2;
			array23[1] = section7;
			array23[2] = settingsView;
			array23[3] = this;
			object obj40;
			xamlServiceProvider22.Add(typeFromHandle44, obj40 = new SimpleValueTargetProvider(array23, CellBase.TitleProperty, nameScope));
			xamlServiceProvider22.Add(typeof(IReferenceProvider), obj40);
			Type typeFromHandle45 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver22 = new XmlNamespaceResolver();
			xmlNamespaceResolver22.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver22.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver22.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver22.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver22.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver22.Add("pids", "clr-namespace:CarScannerXamarinForms.OBD2.PIDS");
			xmlNamespaceResolver22.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver22.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xmlNamespaceResolver22.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver22.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider22.Add(typeFromHandle45, new XamlTypeResolver(xmlNamespaceResolver22, typeof(SettingsCustomPIDEditorV3).GetTypeInfo().Assembly));
			xamlServiceProvider22.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(82, 21)));
			object obj41 = markupExtension22.ProvideValue(xamlServiceProvider22);
			numberPickerCell2.Title = obj41;
			numberPickerCell2.SetValue(NumberPickerCell.MaxProperty, 4);
			numberPickerCell2.SetValue(NumberPickerCell.MinProperty, 1);
			bindingExtension14.Mode = 1;
			bindingExtension14.Path = "DataLength";
			BindingBase bindingBase14 = bindingExtension14.ProvideValue(null);
			numberPickerCell2.SetBinding(NumberPickerCell.NumberProperty, bindingBase14);
			section7.Add(numberPickerCell2);
			settingsCheckBoxCellPatched.SetValue(CellBase.TitleProperty, "Reversed byte order");
			bindingExtension15.Mode = 1;
			bindingExtension15.Path = "ReversedByteSet";
			BindingBase bindingBase15 = bindingExtension15.ProvideValue(null);
			settingsCheckBoxCellPatched.SetBinding(CheckboxCell.CheckedProperty, bindingBase15);
			section7.Add(settingsCheckBoxCellPatched);
			settingsCheckBoxCellPatched2.SetValue(CellBase.TitleProperty, "Signed value");
			bindingExtension16.Mode = 1;
			bindingExtension16.Path = "IsSigned";
			BindingBase bindingBase16 = bindingExtension16.ProvideValue(null);
			settingsCheckBoxCellPatched2.SetBinding(CheckboxCell.CheckedProperty, bindingBase16);
			section7.Add(settingsCheckBoxCellPatched2);
			settingsView.Root.Add(section7);
			bindingExtension17.Path = "Type";
			bindingExtension17.Mode = 2;
			staticResourceExtension6.Key = "EnumValueToTrueConverter";
			IMarkupExtension markupExtension23 = staticResourceExtension6;
			XamlServiceProvider xamlServiceProvider23 = new XamlServiceProvider();
			Type typeFromHandle46 = typeof(IProvideValueTarget);
			object[] array24 = new object[0 + 4];
			array24[0] = bindingExtension17;
			array24[1] = section8;
			array24[2] = settingsView;
			array24[3] = this;
			object obj42;
			xamlServiceProvider23.Add(typeFromHandle46, obj42 = new SimpleValueTargetProvider(array24, typeof(BindingExtension).GetRuntimeProperty("Converter"), nameScope));
			xamlServiceProvider23.Add(typeof(IReferenceProvider), obj42);
			Type typeFromHandle47 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver23 = new XmlNamespaceResolver();
			xmlNamespaceResolver23.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver23.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver23.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver23.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver23.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver23.Add("pids", "clr-namespace:CarScannerXamarinForms.OBD2.PIDS");
			xmlNamespaceResolver23.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver23.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xmlNamespaceResolver23.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver23.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider23.Add(typeFromHandle47, new XamlTypeResolver(xmlNamespaceResolver23, typeof(SettingsCustomPIDEditorV3).GetTypeInfo().Assembly));
			xamlServiceProvider23.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(90, 25)));
			object obj43 = markupExtension23.ProvideValue(xamlServiceProvider23);
			bindingExtension17.Converter = obj43;
			bindingExtension17.ConverterParameter = "5";
			BindingBase bindingBase17 = bindingExtension17.ProvideValue(null);
			section8.SetBinding(Section.IsVisibleProperty, bindingBase17);
			entryCell.SetValue(CellBase.TitleProperty, "Unit");
			bindingExtension18.Mode = 2;
			bindingExtension18.Path = "VagUnit";
			BindingBase bindingBase18 = bindingExtension18.ProvideValue(null);
			entryCell.SetBinding(EntryCell.ValueTextProperty, bindingBase18);
			section8.Add(entryCell);
			entryCell2.SetValue(CellBase.TitleProperty, "Group:");
			bindingExtension19.Mode = 2;
			bindingExtension19.Path = "VagGroup";
			BindingBase bindingBase19 = bindingExtension19.ProvideValue(null);
			entryCell2.SetBinding(EntryCell.ValueTextProperty, bindingBase19);
			section8.Add(entryCell2);
			entryCell3.SetValue(CellBase.TitleProperty, "Item (1-4):");
			bindingExtension20.Mode = 2;
			bindingExtension20.Path = "VagItemInGroup";
			BindingBase bindingBase20 = bindingExtension20.ProvideValue(null);
			entryCell3.SetBinding(EntryCell.ValueTextProperty, bindingBase20);
			section8.Add(entryCell3);
			translate15.Text = "coding_Apply";
			IMarkupExtension markupExtension24 = translate15;
			XamlServiceProvider xamlServiceProvider24 = new XamlServiceProvider();
			Type typeFromHandle48 = typeof(IProvideValueTarget);
			object[] array25 = new object[0 + 4];
			array25[0] = buttonCell;
			array25[1] = section8;
			array25[2] = settingsView;
			array25[3] = this;
			object obj44;
			xamlServiceProvider24.Add(typeFromHandle48, obj44 = new SimpleValueTargetProvider(array25, CellBase.TitleProperty, nameScope));
			xamlServiceProvider24.Add(typeof(IReferenceProvider), obj44);
			Type typeFromHandle49 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver24 = new XmlNamespaceResolver();
			xmlNamespaceResolver24.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver24.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver24.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver24.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver24.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver24.Add("pids", "clr-namespace:CarScannerXamarinForms.OBD2.PIDS");
			xmlNamespaceResolver24.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver24.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xmlNamespaceResolver24.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver24.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider24.Add(typeFromHandle49, new XamlTypeResolver(xmlNamespaceResolver24, typeof(SettingsCustomPIDEditorV3).GetTypeInfo().Assembly));
			xamlServiceProvider24.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(109, 21)));
			object obj45 = markupExtension24.ProvideValue(xamlServiceProvider24);
			buttonCell.Title = obj45;
			buttonCell.Tapped += this.btnApplyVagGroup_Tapped;
			dynamicResourceExtension4.Key = "ButtonAccentColor";
			IMarkupExtension<DynamicResource> markupExtension25 = dynamicResourceExtension4;
			XamlServiceProvider xamlServiceProvider25 = new XamlServiceProvider();
			Type typeFromHandle50 = typeof(IProvideValueTarget);
			object[] array26 = new object[0 + 4];
			array26[0] = buttonCell;
			array26[1] = section8;
			array26[2] = settingsView;
			array26[3] = this;
			object obj46;
			xamlServiceProvider25.Add(typeFromHandle50, obj46 = new SimpleValueTargetProvider(array26, CellBase.TitleColorProperty, nameScope));
			xamlServiceProvider25.Add(typeof(IReferenceProvider), obj46);
			Type typeFromHandle51 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver25 = new XmlNamespaceResolver();
			xmlNamespaceResolver25.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver25.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver25.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver25.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver25.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver25.Add("pids", "clr-namespace:CarScannerXamarinForms.OBD2.PIDS");
			xmlNamespaceResolver25.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver25.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xmlNamespaceResolver25.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver25.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider25.Add(typeFromHandle51, new XamlTypeResolver(xmlNamespaceResolver25, typeof(SettingsCustomPIDEditorV3).GetTypeInfo().Assembly));
			xamlServiceProvider25.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(111, 21)));
			DynamicResource dynamicResource4 = markupExtension25.ProvideValue(xamlServiceProvider25);
			buttonCell.SetDynamicResource(CellBase.TitleColorProperty, dynamicResource4.Key);
			section8.Add(buttonCell);
			settingsView.Root.Add(section8);
			translate16.Text = "ios_Multiplier";
			IMarkupExtension markupExtension26 = translate16;
			XamlServiceProvider xamlServiceProvider26 = new XamlServiceProvider();
			Type typeFromHandle52 = typeof(IProvideValueTarget);
			object[] array27 = new object[0 + 3];
			array27[0] = section9;
			array27[1] = settingsView;
			array27[2] = this;
			object obj47;
			xamlServiceProvider26.Add(typeFromHandle52, obj47 = new SimpleValueTargetProvider(array27, SectionBase.TitleProperty, nameScope));
			xamlServiceProvider26.Add(typeof(IReferenceProvider), obj47);
			Type typeFromHandle53 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver26 = new XmlNamespaceResolver();
			xmlNamespaceResolver26.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver26.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver26.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver26.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver26.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver26.Add("pids", "clr-namespace:CarScannerXamarinForms.OBD2.PIDS");
			xmlNamespaceResolver26.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver26.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xmlNamespaceResolver26.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver26.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider26.Add(typeFromHandle53, new XamlTypeResolver(xmlNamespaceResolver26, typeof(SettingsCustomPIDEditorV3).GetTypeInfo().Assembly));
			xamlServiceProvider26.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(115, 25)));
			object obj48 = markupExtension26.ProvideValue(xamlServiceProvider26);
			section9.Title = obj48;
			bindingExtension21.Path = "Type";
			bindingExtension21.Mode = 2;
			staticResourceExtension7.Key = "EnumValueToTrueConverter";
			IMarkupExtension markupExtension27 = staticResourceExtension7;
			XamlServiceProvider xamlServiceProvider27 = new XamlServiceProvider();
			Type typeFromHandle54 = typeof(IProvideValueTarget);
			object[] array28 = new object[0 + 4];
			array28[0] = bindingExtension21;
			array28[1] = section9;
			array28[2] = settingsView;
			array28[3] = this;
			object obj49;
			xamlServiceProvider27.Add(typeFromHandle54, obj49 = new SimpleValueTargetProvider(array28, typeof(BindingExtension).GetRuntimeProperty("Converter"), nameScope));
			xamlServiceProvider27.Add(typeof(IReferenceProvider), obj49);
			Type typeFromHandle55 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver27 = new XmlNamespaceResolver();
			xmlNamespaceResolver27.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver27.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver27.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver27.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver27.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver27.Add("pids", "clr-namespace:CarScannerXamarinForms.OBD2.PIDS");
			xmlNamespaceResolver27.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver27.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xmlNamespaceResolver27.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver27.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider27.Add(typeFromHandle55, new XamlTypeResolver(xmlNamespaceResolver27, typeof(SettingsCustomPIDEditorV3).GetTypeInfo().Assembly));
			xamlServiceProvider27.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(115, 71)));
			object obj50 = markupExtension27.ProvideValue(xamlServiceProvider27);
			bindingExtension21.Converter = obj50;
			bindingExtension21.ConverterParameter = "1";
			BindingBase bindingBase21 = bindingExtension21.ProvideValue(null);
			section9.SetBinding(Section.IsVisibleProperty, bindingBase21);
			customCell6.SetValue(CustomCell.IsSelectableProperty, false);
			bindingExtension22.Mode = 1;
			bindingExtension22.Path = "Multiplier";
			BindingBase bindingBase22 = bindingExtension22.ProvideValue(null);
			numericEntryV.SetBinding(Entry.TextProperty, bindingBase22);
			customCell6.SetValue(CustomCell.ContentProperty, numericEntryV);
			section9.Add(customCell6);
			settingsView.Root.Add(section9);
			translate17.Text = "ios_Divider";
			IMarkupExtension markupExtension28 = translate17;
			XamlServiceProvider xamlServiceProvider28 = new XamlServiceProvider();
			Type typeFromHandle56 = typeof(IProvideValueTarget);
			object[] array29 = new object[0 + 3];
			array29[0] = section10;
			array29[1] = settingsView;
			array29[2] = this;
			object obj51;
			xamlServiceProvider28.Add(typeFromHandle56, obj51 = new SimpleValueTargetProvider(array29, SectionBase.TitleProperty, nameScope));
			xamlServiceProvider28.Add(typeof(IReferenceProvider), obj51);
			Type typeFromHandle57 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver28 = new XmlNamespaceResolver();
			xmlNamespaceResolver28.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver28.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver28.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver28.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver28.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver28.Add("pids", "clr-namespace:CarScannerXamarinForms.OBD2.PIDS");
			xmlNamespaceResolver28.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver28.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xmlNamespaceResolver28.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver28.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider28.Add(typeFromHandle57, new XamlTypeResolver(xmlNamespaceResolver28, typeof(SettingsCustomPIDEditorV3).GetTypeInfo().Assembly));
			xamlServiceProvider28.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(120, 25)));
			object obj52 = markupExtension28.ProvideValue(xamlServiceProvider28);
			section10.Title = obj52;
			bindingExtension23.Path = "Type";
			bindingExtension23.Mode = 2;
			staticResourceExtension8.Key = "EnumValueToTrueConverter";
			IMarkupExtension markupExtension29 = staticResourceExtension8;
			XamlServiceProvider xamlServiceProvider29 = new XamlServiceProvider();
			Type typeFromHandle58 = typeof(IProvideValueTarget);
			object[] array30 = new object[0 + 4];
			array30[0] = bindingExtension23;
			array30[1] = section10;
			array30[2] = settingsView;
			array30[3] = this;
			object obj53;
			xamlServiceProvider29.Add(typeFromHandle58, obj53 = new SimpleValueTargetProvider(array30, typeof(BindingExtension).GetRuntimeProperty("Converter"), nameScope));
			xamlServiceProvider29.Add(typeof(IReferenceProvider), obj53);
			Type typeFromHandle59 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver29 = new XmlNamespaceResolver();
			xmlNamespaceResolver29.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver29.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver29.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver29.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver29.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver29.Add("pids", "clr-namespace:CarScannerXamarinForms.OBD2.PIDS");
			xmlNamespaceResolver29.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver29.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xmlNamespaceResolver29.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver29.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider29.Add(typeFromHandle59, new XamlTypeResolver(xmlNamespaceResolver29, typeof(SettingsCustomPIDEditorV3).GetTypeInfo().Assembly));
			xamlServiceProvider29.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(120, 68)));
			object obj54 = markupExtension29.ProvideValue(xamlServiceProvider29);
			bindingExtension23.Converter = obj54;
			bindingExtension23.ConverterParameter = "1";
			BindingBase bindingBase23 = bindingExtension23.ProvideValue(null);
			section10.SetBinding(Section.IsVisibleProperty, bindingBase23);
			customCell7.SetValue(CustomCell.IsSelectableProperty, false);
			bindingExtension24.Mode = 1;
			bindingExtension24.Path = "Divider";
			BindingBase bindingBase24 = bindingExtension24.ProvideValue(null);
			numericEntryV2.SetBinding(NumericEntryV3.ValueProperty, bindingBase24);
			customCell7.SetValue(CustomCell.ContentProperty, numericEntryV2);
			section10.Add(customCell7);
			settingsView.Root.Add(section10);
			translate18.Text = "ios_Offset";
			IMarkupExtension markupExtension30 = translate18;
			XamlServiceProvider xamlServiceProvider30 = new XamlServiceProvider();
			Type typeFromHandle60 = typeof(IProvideValueTarget);
			object[] array31 = new object[0 + 3];
			array31[0] = section11;
			array31[1] = settingsView;
			array31[2] = this;
			object obj55;
			xamlServiceProvider30.Add(typeFromHandle60, obj55 = new SimpleValueTargetProvider(array31, SectionBase.TitleProperty, nameScope));
			xamlServiceProvider30.Add(typeof(IReferenceProvider), obj55);
			Type typeFromHandle61 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver30 = new XmlNamespaceResolver();
			xmlNamespaceResolver30.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver30.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver30.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver30.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver30.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver30.Add("pids", "clr-namespace:CarScannerXamarinForms.OBD2.PIDS");
			xmlNamespaceResolver30.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver30.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xmlNamespaceResolver30.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver30.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider30.Add(typeFromHandle61, new XamlTypeResolver(xmlNamespaceResolver30, typeof(SettingsCustomPIDEditorV3).GetTypeInfo().Assembly));
			xamlServiceProvider30.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(125, 25)));
			object obj56 = markupExtension30.ProvideValue(xamlServiceProvider30);
			section11.Title = obj56;
			bindingExtension25.Path = "Type";
			bindingExtension25.Mode = 2;
			staticResourceExtension9.Key = "EnumValueToTrueConverter";
			IMarkupExtension markupExtension31 = staticResourceExtension9;
			XamlServiceProvider xamlServiceProvider31 = new XamlServiceProvider();
			Type typeFromHandle62 = typeof(IProvideValueTarget);
			object[] array32 = new object[0 + 4];
			array32[0] = bindingExtension25;
			array32[1] = section11;
			array32[2] = settingsView;
			array32[3] = this;
			object obj57;
			xamlServiceProvider31.Add(typeFromHandle62, obj57 = new SimpleValueTargetProvider(array32, typeof(BindingExtension).GetRuntimeProperty("Converter"), nameScope));
			xamlServiceProvider31.Add(typeof(IReferenceProvider), obj57);
			Type typeFromHandle63 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver31 = new XmlNamespaceResolver();
			xmlNamespaceResolver31.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver31.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver31.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver31.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver31.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver31.Add("pids", "clr-namespace:CarScannerXamarinForms.OBD2.PIDS");
			xmlNamespaceResolver31.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver31.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xmlNamespaceResolver31.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver31.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider31.Add(typeFromHandle63, new XamlTypeResolver(xmlNamespaceResolver31, typeof(SettingsCustomPIDEditorV3).GetTypeInfo().Assembly));
			xamlServiceProvider31.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(125, 67)));
			object obj58 = markupExtension31.ProvideValue(xamlServiceProvider31);
			bindingExtension25.Converter = obj58;
			bindingExtension25.ConverterParameter = "1";
			BindingBase bindingBase25 = bindingExtension25.ProvideValue(null);
			section11.SetBinding(Section.IsVisibleProperty, bindingBase25);
			customCell8.SetValue(CustomCell.IsSelectableProperty, false);
			numericEntryV3.SetValue(Grid.RowProperty, 4);
			numericEntryV3.SetValue(Grid.ColumnProperty, 1);
			bindingExtension26.Mode = 1;
			bindingExtension26.Path = "Offset";
			BindingBase bindingBase26 = bindingExtension26.ProvideValue(null);
			numericEntryV3.SetBinding(NumericEntryV3.ValueProperty, bindingBase26);
			customCell8.SetValue(CustomCell.ContentProperty, numericEntryV3);
			section11.Add(customCell8);
			settingsView.Root.Add(section11);
			bindingExtension27.Path = "Type";
			bindingExtension27.Mode = 2;
			staticResourceExtension10.Key = "EnumValueToTrueConverter";
			IMarkupExtension markupExtension32 = staticResourceExtension10;
			XamlServiceProvider xamlServiceProvider32 = new XamlServiceProvider();
			Type typeFromHandle64 = typeof(IProvideValueTarget);
			object[] array33 = new object[0 + 4];
			array33[0] = bindingExtension27;
			array33[1] = section12;
			array33[2] = settingsView;
			array33[3] = this;
			object obj59;
			xamlServiceProvider32.Add(typeFromHandle64, obj59 = new SimpleValueTargetProvider(array33, typeof(BindingExtension).GetRuntimeProperty("Converter"), nameScope));
			xamlServiceProvider32.Add(typeof(IReferenceProvider), obj59);
			Type typeFromHandle65 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver32 = new XmlNamespaceResolver();
			xmlNamespaceResolver32.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver32.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver32.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver32.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver32.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver32.Add("pids", "clr-namespace:CarScannerXamarinForms.OBD2.PIDS");
			xmlNamespaceResolver32.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver32.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xmlNamespaceResolver32.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver32.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider32.Add(typeFromHandle65, new XamlTypeResolver(xmlNamespaceResolver32, typeof(SettingsCustomPIDEditorV3).GetTypeInfo().Assembly));
			xamlServiceProvider32.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(134, 25)));
			object obj60 = markupExtension32.ProvideValue(xamlServiceProvider32);
			bindingExtension27.Converter = obj60;
			bindingExtension27.ConverterParameter = "2";
			BindingBase bindingBase27 = bindingExtension27.ProvideValue(null);
			section12.SetBinding(Section.IsVisibleProperty, bindingBase27);
			translate19.Text = "ios_Byte";
			IMarkupExtension markupExtension33 = translate19;
			XamlServiceProvider xamlServiceProvider33 = new XamlServiceProvider();
			Type typeFromHandle66 = typeof(IProvideValueTarget);
			object[] array34 = new object[0 + 4];
			array34[0] = numberPickerCell3;
			array34[1] = section12;
			array34[2] = settingsView;
			array34[3] = this;
			object obj61;
			xamlServiceProvider33.Add(typeFromHandle66, obj61 = new SimpleValueTargetProvider(array34, CellBase.TitleProperty, nameScope));
			xamlServiceProvider33.Add(typeof(IReferenceProvider), obj61);
			Type typeFromHandle67 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver33 = new XmlNamespaceResolver();
			xmlNamespaceResolver33.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver33.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver33.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver33.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver33.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver33.Add("pids", "clr-namespace:CarScannerXamarinForms.OBD2.PIDS");
			xmlNamespaceResolver33.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver33.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xmlNamespaceResolver33.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver33.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider33.Add(typeFromHandle67, new XamlTypeResolver(xmlNamespaceResolver33, typeof(SettingsCustomPIDEditorV3).GetTypeInfo().Assembly));
			xamlServiceProvider33.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(136, 21)));
			object obj62 = markupExtension33.ProvideValue(xamlServiceProvider33);
			numberPickerCell3.Title = obj62;
			numberPickerCell3.SetValue(NumberPickerCell.MaxProperty, 4096);
			numberPickerCell3.SetValue(NumberPickerCell.MinProperty, 0);
			bindingExtension28.Mode = 1;
			bindingExtension28.Path = "StartByteId";
			BindingBase bindingBase28 = bindingExtension28.ProvideValue(null);
			numberPickerCell3.SetBinding(NumberPickerCell.NumberProperty, bindingBase28);
			section12.Add(numberPickerCell3);
			numberPickerCell4.SetValue(CellBase.TitleProperty, "Bit");
			numberPickerCell4.SetValue(NumberPickerCell.MaxProperty, 7);
			numberPickerCell4.SetValue(NumberPickerCell.MinProperty, 0);
			bindingExtension29.Mode = 1;
			bindingExtension29.Path = "Bit";
			BindingBase bindingBase29 = bindingExtension29.ProvideValue(null);
			numberPickerCell4.SetBinding(NumberPickerCell.NumberProperty, bindingBase29);
			section12.Add(numberPickerCell4);
			settingsView.Root.Add(section12);
			translate20.Text = "Mode06Page_tbMinimum.Text";
			IMarkupExtension markupExtension34 = translate20;
			XamlServiceProvider xamlServiceProvider34 = new XamlServiceProvider();
			Type typeFromHandle68 = typeof(IProvideValueTarget);
			object[] array35 = new object[0 + 3];
			array35[0] = section13;
			array35[1] = settingsView;
			array35[2] = this;
			object obj63;
			xamlServiceProvider34.Add(typeFromHandle68, obj63 = new SimpleValueTargetProvider(array35, SectionBase.TitleProperty, nameScope));
			xamlServiceProvider34.Add(typeof(IReferenceProvider), obj63);
			Type typeFromHandle69 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver34 = new XmlNamespaceResolver();
			xmlNamespaceResolver34.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver34.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver34.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver34.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver34.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver34.Add("pids", "clr-namespace:CarScannerXamarinForms.OBD2.PIDS");
			xmlNamespaceResolver34.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver34.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xmlNamespaceResolver34.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver34.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider34.Add(typeFromHandle69, new XamlTypeResolver(xmlNamespaceResolver34, typeof(SettingsCustomPIDEditorV3).GetTypeInfo().Assembly));
			xamlServiceProvider34.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(147, 25)));
			object obj64 = markupExtension34.ProvideValue(xamlServiceProvider34);
			section13.Title = obj64;
			bindingExtension30.Path = "Type";
			bindingExtension30.Mode = 2;
			staticResourceExtension11.Key = "EnumValueToFalseConverter";
			IMarkupExtension markupExtension35 = staticResourceExtension11;
			XamlServiceProvider xamlServiceProvider35 = new XamlServiceProvider();
			Type typeFromHandle70 = typeof(IProvideValueTarget);
			object[] array36 = new object[0 + 4];
			array36[0] = bindingExtension30;
			array36[1] = section13;
			array36[2] = settingsView;
			array36[3] = this;
			object obj65;
			xamlServiceProvider35.Add(typeFromHandle70, obj65 = new SimpleValueTargetProvider(array36, typeof(BindingExtension).GetRuntimeProperty("Converter"), nameScope));
			xamlServiceProvider35.Add(typeof(IReferenceProvider), obj65);
			Type typeFromHandle71 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver35 = new XmlNamespaceResolver();
			xmlNamespaceResolver35.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver35.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver35.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver35.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver35.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver35.Add("pids", "clr-namespace:CarScannerXamarinForms.OBD2.PIDS");
			xmlNamespaceResolver35.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver35.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xmlNamespaceResolver35.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver35.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider35.Add(typeFromHandle71, new XamlTypeResolver(xmlNamespaceResolver35, typeof(SettingsCustomPIDEditorV3).GetTypeInfo().Assembly));
			xamlServiceProvider35.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(147, 82)));
			object obj66 = markupExtension35.ProvideValue(xamlServiceProvider35);
			bindingExtension30.Converter = obj66;
			bindingExtension30.ConverterParameter = "3";
			BindingBase bindingBase30 = bindingExtension30.ProvideValue(null);
			section13.SetBinding(Section.IsVisibleProperty, bindingBase30);
			customCell9.SetValue(CustomCell.IsSelectableProperty, false);
			bindingExtension31.Mode = 1;
			bindingExtension31.Path = "Minimum";
			BindingBase bindingBase31 = bindingExtension31.ProvideValue(null);
			numericEntryV4.SetBinding(NumericEntryV3.ValueProperty, bindingBase31);
			customCell9.SetValue(CustomCell.ContentProperty, numericEntryV4);
			section13.Add(customCell9);
			settingsView.Root.Add(section13);
			translate21.Text = "Mode06Page_tbMaximum.Text";
			IMarkupExtension markupExtension36 = translate21;
			XamlServiceProvider xamlServiceProvider36 = new XamlServiceProvider();
			Type typeFromHandle72 = typeof(IProvideValueTarget);
			object[] array37 = new object[0 + 3];
			array37[0] = section14;
			array37[1] = settingsView;
			array37[2] = this;
			object obj67;
			xamlServiceProvider36.Add(typeFromHandle72, obj67 = new SimpleValueTargetProvider(array37, SectionBase.TitleProperty, nameScope));
			xamlServiceProvider36.Add(typeof(IReferenceProvider), obj67);
			Type typeFromHandle73 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver36 = new XmlNamespaceResolver();
			xmlNamespaceResolver36.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver36.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver36.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver36.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver36.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver36.Add("pids", "clr-namespace:CarScannerXamarinForms.OBD2.PIDS");
			xmlNamespaceResolver36.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver36.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xmlNamespaceResolver36.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver36.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider36.Add(typeFromHandle73, new XamlTypeResolver(xmlNamespaceResolver36, typeof(SettingsCustomPIDEditorV3).GetTypeInfo().Assembly));
			xamlServiceProvider36.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(153, 25)));
			object obj68 = markupExtension36.ProvideValue(xamlServiceProvider36);
			section14.Title = obj68;
			bindingExtension32.Path = "Type";
			bindingExtension32.Mode = 2;
			staticResourceExtension12.Key = "EnumValueToFalseConverter";
			IMarkupExtension markupExtension37 = staticResourceExtension12;
			XamlServiceProvider xamlServiceProvider37 = new XamlServiceProvider();
			Type typeFromHandle74 = typeof(IProvideValueTarget);
			object[] array38 = new object[0 + 4];
			array38[0] = bindingExtension32;
			array38[1] = section14;
			array38[2] = settingsView;
			array38[3] = this;
			object obj69;
			xamlServiceProvider37.Add(typeFromHandle74, obj69 = new SimpleValueTargetProvider(array38, typeof(BindingExtension).GetRuntimeProperty("Converter"), nameScope));
			xamlServiceProvider37.Add(typeof(IReferenceProvider), obj69);
			Type typeFromHandle75 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver37 = new XmlNamespaceResolver();
			xmlNamespaceResolver37.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver37.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver37.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver37.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver37.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver37.Add("pids", "clr-namespace:CarScannerXamarinForms.OBD2.PIDS");
			xmlNamespaceResolver37.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver37.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xmlNamespaceResolver37.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver37.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider37.Add(typeFromHandle75, new XamlTypeResolver(xmlNamespaceResolver37, typeof(SettingsCustomPIDEditorV3).GetTypeInfo().Assembly));
			xamlServiceProvider37.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(153, 82)));
			object obj70 = markupExtension37.ProvideValue(xamlServiceProvider37);
			bindingExtension32.Converter = obj70;
			bindingExtension32.ConverterParameter = "3";
			BindingBase bindingBase32 = bindingExtension32.ProvideValue(null);
			section14.SetBinding(Section.IsVisibleProperty, bindingBase32);
			customCell10.SetValue(CustomCell.IsSelectableProperty, false);
			bindingExtension33.Mode = 1;
			bindingExtension33.Path = "Maximum";
			BindingBase bindingBase33 = bindingExtension33.ProvideValue(null);
			numericEntryV5.SetBinding(NumericEntryV3.ValueProperty, bindingBase33);
			customCell10.SetValue(CustomCell.ContentProperty, numericEntryV5);
			section14.Add(customCell10);
			settingsView.Root.Add(section14);
			translate22.Text = "ios_Units";
			IMarkupExtension markupExtension38 = translate22;
			XamlServiceProvider xamlServiceProvider38 = new XamlServiceProvider();
			Type typeFromHandle76 = typeof(IProvideValueTarget);
			object[] array39 = new object[0 + 3];
			array39[0] = section15;
			array39[1] = settingsView;
			array39[2] = this;
			object obj71;
			xamlServiceProvider38.Add(typeFromHandle76, obj71 = new SimpleValueTargetProvider(array39, SectionBase.TitleProperty, nameScope));
			xamlServiceProvider38.Add(typeof(IReferenceProvider), obj71);
			Type typeFromHandle77 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver38 = new XmlNamespaceResolver();
			xmlNamespaceResolver38.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver38.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver38.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver38.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver38.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver38.Add("pids", "clr-namespace:CarScannerXamarinForms.OBD2.PIDS");
			xmlNamespaceResolver38.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver38.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xmlNamespaceResolver38.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver38.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider38.Add(typeFromHandle77, new XamlTypeResolver(xmlNamespaceResolver38, typeof(SettingsCustomPIDEditorV3).GetTypeInfo().Assembly));
			xamlServiceProvider38.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(159, 25)));
			object obj72 = markupExtension38.ProvideValue(xamlServiceProvider38);
			section15.Title = obj72;
			bindingExtension34.Path = "Type";
			bindingExtension34.Mode = 2;
			staticResourceExtension13.Key = "EnumValueToFalseConverter";
			IMarkupExtension markupExtension39 = staticResourceExtension13;
			XamlServiceProvider xamlServiceProvider39 = new XamlServiceProvider();
			Type typeFromHandle78 = typeof(IProvideValueTarget);
			object[] array40 = new object[0 + 4];
			array40[0] = bindingExtension34;
			array40[1] = section15;
			array40[2] = settingsView;
			array40[3] = this;
			object obj73;
			xamlServiceProvider39.Add(typeFromHandle78, obj73 = new SimpleValueTargetProvider(array40, typeof(BindingExtension).GetRuntimeProperty("Converter"), nameScope));
			xamlServiceProvider39.Add(typeof(IReferenceProvider), obj73);
			Type typeFromHandle79 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver39 = new XmlNamespaceResolver();
			xmlNamespaceResolver39.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver39.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver39.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver39.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver39.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver39.Add("pids", "clr-namespace:CarScannerXamarinForms.OBD2.PIDS");
			xmlNamespaceResolver39.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver39.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xmlNamespaceResolver39.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver39.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider39.Add(typeFromHandle79, new XamlTypeResolver(xmlNamespaceResolver39, typeof(SettingsCustomPIDEditorV3).GetTypeInfo().Assembly));
			xamlServiceProvider39.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(159, 66)));
			object obj74 = markupExtension39.ProvideValue(xamlServiceProvider39);
			bindingExtension34.Converter = obj74;
			bindingExtension34.ConverterParameter = "3";
			BindingBase bindingBase34 = bindingExtension34.ProvideValue(null);
			section15.SetBinding(Section.IsVisibleProperty, bindingBase34);
			bindingExtension35.Source = units;
			BindingBase bindingBase35 = bindingExtension35.ProvideValue(null);
			picker.SetBinding(Picker.ItemsSourceProperty, bindingBase35);
			bindingExtension36.Mode = 1;
			staticResourceExtension14.Key = "UnitsToIntConverter";
			IMarkupExtension markupExtension40 = staticResourceExtension14;
			XamlServiceProvider xamlServiceProvider40 = new XamlServiceProvider();
			Type typeFromHandle80 = typeof(IProvideValueTarget);
			object[] array41 = new object[0 + 6];
			array41[0] = bindingExtension36;
			array41[1] = picker;
			array41[2] = settingsCustomCellForPicker;
			array41[3] = section15;
			array41[4] = settingsView;
			array41[5] = this;
			object obj75;
			xamlServiceProvider40.Add(typeFromHandle80, obj75 = new SimpleValueTargetProvider(array41, typeof(BindingExtension).GetRuntimeProperty("Converter"), nameScope));
			xamlServiceProvider40.Add(typeof(IReferenceProvider), obj75);
			Type typeFromHandle81 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver40 = new XmlNamespaceResolver();
			xmlNamespaceResolver40.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver40.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver40.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver40.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver40.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver40.Add("pids", "clr-namespace:CarScannerXamarinForms.OBD2.PIDS");
			xmlNamespaceResolver40.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver40.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xmlNamespaceResolver40.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver40.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider40.Add(typeFromHandle81, new XamlTypeResolver(xmlNamespaceResolver40, typeof(SettingsCustomPIDEditorV3).GetTypeInfo().Assembly));
			xamlServiceProvider40.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(161, 95)));
			object obj76 = markupExtension40.ProvideValue(xamlServiceProvider40);
			bindingExtension36.Converter = obj76;
			bindingExtension36.Path = "Units";
			BindingBase bindingBase36 = bindingExtension36.ProvideValue(null);
			picker.SetBinding(Picker.SelectedIndexProperty, bindingBase36);
			settingsCustomCellForPicker.SetValue(CustomCell.ContentProperty, picker);
			section15.Add(settingsCustomCellForPicker);
			buttonCell2.SetValue(CellBase.TitleProperty, "Units override");
			buttonCell2.Tapped += this.btnUnitsOverride_Clicked;
			dynamicResourceExtension5.Key = "ButtonAccentColor";
			IMarkupExtension<DynamicResource> markupExtension41 = dynamicResourceExtension5;
			XamlServiceProvider xamlServiceProvider41 = new XamlServiceProvider();
			Type typeFromHandle82 = typeof(IProvideValueTarget);
			object[] array42 = new object[0 + 4];
			array42[0] = buttonCell2;
			array42[1] = section15;
			array42[2] = settingsView;
			array42[3] = this;
			object obj77;
			xamlServiceProvider41.Add(typeFromHandle82, obj77 = new SimpleValueTargetProvider(array42, CellBase.TitleColorProperty, nameScope));
			xamlServiceProvider41.Add(typeof(IReferenceProvider), obj77);
			Type typeFromHandle83 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver41 = new XmlNamespaceResolver();
			xmlNamespaceResolver41.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver41.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver41.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver41.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver41.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver41.Add("pids", "clr-namespace:CarScannerXamarinForms.OBD2.PIDS");
			xmlNamespaceResolver41.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver41.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xmlNamespaceResolver41.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver41.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider41.Add(typeFromHandle83, new XamlTypeResolver(xmlNamespaceResolver41, typeof(SettingsCustomPIDEditorV3).GetTypeInfo().Assembly));
			xamlServiceProvider41.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(168, 21)));
			DynamicResource dynamicResource5 = markupExtension41.ProvideValue(xamlServiceProvider41);
			buttonCell2.SetDynamicResource(CellBase.TitleColorProperty, dynamicResource5.Key);
			section15.Add(buttonCell2);
			settingsView.Root.Add(section15);
			translate23.Text = "ios_Priority";
			IMarkupExtension markupExtension42 = translate23;
			XamlServiceProvider xamlServiceProvider42 = new XamlServiceProvider();
			Type typeFromHandle84 = typeof(IProvideValueTarget);
			object[] array43 = new object[0 + 3];
			array43[0] = section16;
			array43[1] = settingsView;
			array43[2] = this;
			object obj78;
			xamlServiceProvider42.Add(typeFromHandle84, obj78 = new SimpleValueTargetProvider(array43, SectionBase.TitleProperty, nameScope));
			xamlServiceProvider42.Add(typeof(IReferenceProvider), obj78);
			Type typeFromHandle85 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver42 = new XmlNamespaceResolver();
			xmlNamespaceResolver42.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver42.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver42.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver42.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver42.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver42.Add("pids", "clr-namespace:CarScannerXamarinForms.OBD2.PIDS");
			xmlNamespaceResolver42.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver42.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xmlNamespaceResolver42.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver42.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider42.Add(typeFromHandle85, new XamlTypeResolver(xmlNamespaceResolver42, typeof(SettingsCustomPIDEditorV3).GetTypeInfo().Assembly));
			xamlServiceProvider42.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(170, 25)));
			object obj79 = markupExtension42.ProvideValue(xamlServiceProvider42);
			section16.Title = obj79;
			bindingExtension37.Mode = 1;
			staticResourceExtension15.Key = "SkipCyclesIntToPriorityStringConverter";
			IMarkupExtension markupExtension43 = staticResourceExtension15;
			XamlServiceProvider xamlServiceProvider43 = new XamlServiceProvider();
			Type typeFromHandle86 = typeof(IProvideValueTarget);
			object[] array44 = new object[0 + 6];
			array44[0] = bindingExtension37;
			array44[1] = picker2;
			array44[2] = settingsCustomCellForPicker2;
			array44[3] = section16;
			array44[4] = settingsView;
			array44[5] = this;
			object obj80;
			xamlServiceProvider43.Add(typeFromHandle86, obj80 = new SimpleValueTargetProvider(array44, typeof(BindingExtension).GetRuntimeProperty("Converter"), nameScope));
			xamlServiceProvider43.Add(typeof(IReferenceProvider), obj80);
			Type typeFromHandle87 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver43 = new XmlNamespaceResolver();
			xmlNamespaceResolver43.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver43.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver43.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver43.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver43.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver43.Add("pids", "clr-namespace:CarScannerXamarinForms.OBD2.PIDS");
			xmlNamespaceResolver43.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver43.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xmlNamespaceResolver43.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver43.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider43.Add(typeFromHandle87, new XamlTypeResolver(xmlNamespaceResolver43, typeof(SettingsCustomPIDEditorV3).GetTypeInfo().Assembly));
			xamlServiceProvider43.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(172, 29)));
			object obj81 = markupExtension43.ProvideValue(xamlServiceProvider43);
			bindingExtension37.Converter = obj81;
			bindingExtension37.Path = "SkipCycles";
			BindingBase bindingBase37 = bindingExtension37.ProvideValue(null);
			picker2.SetBinding(Picker.SelectedItemProperty, bindingBase37);
			picker2.SetValue(Picker.ItemsSourceProperty, array);
			settingsCustomCellForPicker2.SetValue(CustomCell.ContentProperty, picker2);
			section16.Add(settingsCustomCellForPicker2);
			settingsView.Root.Add(section16);
			translate24.Text = "pid_override_Role";
			IMarkupExtension markupExtension44 = translate24;
			XamlServiceProvider xamlServiceProvider44 = new XamlServiceProvider();
			Type typeFromHandle88 = typeof(IProvideValueTarget);
			object[] array45 = new object[0 + 3];
			array45[0] = section17;
			array45[1] = settingsView;
			array45[2] = this;
			object obj82;
			xamlServiceProvider44.Add(typeFromHandle88, obj82 = new SimpleValueTargetProvider(array45, SectionBase.TitleProperty, nameScope));
			xamlServiceProvider44.Add(typeof(IReferenceProvider), obj82);
			Type typeFromHandle89 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver44 = new XmlNamespaceResolver();
			xmlNamespaceResolver44.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver44.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver44.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver44.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver44.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver44.Add("pids", "clr-namespace:CarScannerXamarinForms.OBD2.PIDS");
			xmlNamespaceResolver44.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver44.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xmlNamespaceResolver44.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver44.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider44.Add(typeFromHandle89, new XamlTypeResolver(xmlNamespaceResolver44, typeof(SettingsCustomPIDEditorV3).GetTypeInfo().Assembly));
			xamlServiceProvider44.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(205, 25)));
			object obj83 = markupExtension44.ProvideValue(xamlServiceProvider44);
			section17.Title = obj83;
			bindingExtension38.Mode = 1;
			staticResourceExtension16.Key = "EnumToIntConverter";
			IMarkupExtension markupExtension45 = staticResourceExtension16;
			XamlServiceProvider xamlServiceProvider45 = new XamlServiceProvider();
			Type typeFromHandle90 = typeof(IProvideValueTarget);
			object[] array46 = new object[0 + 6];
			array46[0] = bindingExtension38;
			array46[1] = picker3;
			array46[2] = settingsCustomCellForPicker3;
			array46[3] = section17;
			array46[4] = settingsView;
			array46[5] = this;
			object obj84;
			xamlServiceProvider45.Add(typeFromHandle90, obj84 = new SimpleValueTargetProvider(array46, typeof(BindingExtension).GetRuntimeProperty("Converter"), nameScope));
			xamlServiceProvider45.Add(typeof(IReferenceProvider), obj84);
			Type typeFromHandle91 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver45 = new XmlNamespaceResolver();
			xmlNamespaceResolver45.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver45.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver45.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver45.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver45.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver45.Add("pids", "clr-namespace:CarScannerXamarinForms.OBD2.PIDS");
			xmlNamespaceResolver45.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver45.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xmlNamespaceResolver45.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver45.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider45.Add(typeFromHandle91, new XamlTypeResolver(xmlNamespaceResolver45, typeof(SettingsCustomPIDEditorV3).GetTypeInfo().Assembly));
			xamlServiceProvider45.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(207, 49)));
			object obj85 = markupExtension45.ProvideValue(xamlServiceProvider45);
			bindingExtension38.Converter = obj85;
			bindingExtension38.Path = "Role";
			BindingBase bindingBase38 = bindingExtension38.ProvideValue(null);
			picker3.SetBinding(Picker.SelectedIndexProperty, bindingBase38);
			settingsCustomCellForPicker3.SetValue(CustomCell.ContentProperty, picker3);
			section17.Add(settingsCustomCellForPicker3);
			settingsView.Root.Add(section17);
			translate25.Text = "CustomPidEditor_tbBeforeCommands.Text";
			IMarkupExtension markupExtension46 = translate25;
			XamlServiceProvider xamlServiceProvider46 = new XamlServiceProvider();
			Type typeFromHandle92 = typeof(IProvideValueTarget);
			object[] array47 = new object[0 + 3];
			array47[0] = section18;
			array47[1] = settingsView;
			array47[2] = this;
			object obj86;
			xamlServiceProvider46.Add(typeFromHandle92, obj86 = new SimpleValueTargetProvider(array47, SectionBase.TitleProperty, nameScope));
			xamlServiceProvider46.Add(typeof(IReferenceProvider), obj86);
			Type typeFromHandle93 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver46 = new XmlNamespaceResolver();
			xmlNamespaceResolver46.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver46.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver46.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver46.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver46.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver46.Add("pids", "clr-namespace:CarScannerXamarinForms.OBD2.PIDS");
			xmlNamespaceResolver46.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver46.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xmlNamespaceResolver46.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver46.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider46.Add(typeFromHandle93, new XamlTypeResolver(xmlNamespaceResolver46, typeof(SettingsCustomPIDEditorV3).GetTypeInfo().Assembly));
			xamlServiceProvider46.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(211, 25)));
			object obj87 = markupExtension46.ProvideValue(xamlServiceProvider46);
			section18.Title = obj87;
			customCell11.SetValue(CustomCell.IsSelectableProperty, false);
			bindingExtension39.Mode = 1;
			bindingExtension39.Path = "BeforeCommand";
			BindingBase bindingBase39 = bindingExtension39.ProvideValue(null);
			entry6.SetBinding(Entry.TextProperty, bindingBase39);
			customCell11.SetValue(CustomCell.ContentProperty, entry6);
			section18.Add(customCell11);
			settingsView.Root.Add(section18);
			translate26.Text = "CustomPidEditor_tbAfterCommands.Text";
			IMarkupExtension markupExtension47 = translate26;
			XamlServiceProvider xamlServiceProvider47 = new XamlServiceProvider();
			Type typeFromHandle94 = typeof(IProvideValueTarget);
			object[] array48 = new object[0 + 3];
			array48[0] = section19;
			array48[1] = settingsView;
			array48[2] = this;
			object obj88;
			xamlServiceProvider47.Add(typeFromHandle94, obj88 = new SimpleValueTargetProvider(array48, SectionBase.TitleProperty, nameScope));
			xamlServiceProvider47.Add(typeof(IReferenceProvider), obj88);
			Type typeFromHandle95 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver47 = new XmlNamespaceResolver();
			xmlNamespaceResolver47.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver47.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver47.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver47.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver47.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver47.Add("pids", "clr-namespace:CarScannerXamarinForms.OBD2.PIDS");
			xmlNamespaceResolver47.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver47.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xmlNamespaceResolver47.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver47.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider47.Add(typeFromHandle95, new XamlTypeResolver(xmlNamespaceResolver47, typeof(SettingsCustomPIDEditorV3).GetTypeInfo().Assembly));
			xamlServiceProvider47.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(217, 25)));
			object obj89 = markupExtension47.ProvideValue(xamlServiceProvider47);
			section19.Title = obj89;
			customCell12.SetValue(CustomCell.IsSelectableProperty, false);
			bindingExtension40.Mode = 1;
			bindingExtension40.Path = "AfterCommand";
			BindingBase bindingBase40 = bindingExtension40.ProvideValue(null);
			entry7.SetBinding(Entry.TextProperty, bindingBase40);
			customCell12.SetValue(CustomCell.ContentProperty, entry7);
			section19.Add(customCell12);
			settingsView.Root.Add(section19);
			section20.SetValue(SectionBase.TitleProperty, "Test");
			buttonCell3.SetValue(CellBase.TitleProperty, "Test");
			buttonCell3.Tapped += this.BtnTest_Clicked;
			dynamicResourceExtension6.Key = "ButtonAccentColor";
			IMarkupExtension<DynamicResource> markupExtension48 = dynamicResourceExtension6;
			XamlServiceProvider xamlServiceProvider48 = new XamlServiceProvider();
			Type typeFromHandle96 = typeof(IProvideValueTarget);
			object[] array49 = new object[0 + 4];
			array49[0] = buttonCell3;
			array49[1] = section20;
			array49[2] = settingsView;
			array49[3] = this;
			object obj90;
			xamlServiceProvider48.Add(typeFromHandle96, obj90 = new SimpleValueTargetProvider(array49, CellBase.TitleColorProperty, nameScope));
			xamlServiceProvider48.Add(typeof(IReferenceProvider), obj90);
			Type typeFromHandle97 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver48 = new XmlNamespaceResolver();
			xmlNamespaceResolver48.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver48.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver48.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver48.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver48.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver48.Add("pids", "clr-namespace:CarScannerXamarinForms.OBD2.PIDS");
			xmlNamespaceResolver48.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver48.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xmlNamespaceResolver48.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver48.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider48.Add(typeFromHandle97, new XamlTypeResolver(xmlNamespaceResolver48, typeof(SettingsCustomPIDEditorV3).GetTypeInfo().Assembly));
			xamlServiceProvider48.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(227, 21)));
			DynamicResource dynamicResource6 = markupExtension48.ProvideValue(xamlServiceProvider48);
			buttonCell3.SetDynamicResource(CellBase.TitleColorProperty, dynamicResource6.Key);
			section20.Add(buttonCell3);
			customCell13.SetValue(CustomCell.IsMeasureOnceProperty, false);
			referenceExtension.Name = "testLabelCell";
			IMarkupExtension markupExtension49 = referenceExtension;
			XamlServiceProvider xamlServiceProvider49 = new XamlServiceProvider();
			Type typeFromHandle98 = typeof(IProvideValueTarget);
			object[] array50 = new object[0 + 6];
			array50[0] = bindingExtension41;
			array50[1] = label;
			array50[2] = customCell13;
			array50[3] = section20;
			array50[4] = settingsView;
			array50[5] = this;
			object obj91;
			xamlServiceProvider49.Add(typeFromHandle98, obj91 = new SimpleValueTargetProvider(array50, typeof(BindingExtension).GetRuntimeProperty("Source"), nameScope));
			xamlServiceProvider49.Add(typeof(IReferenceProvider), obj91);
			Type typeFromHandle99 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver49 = new XmlNamespaceResolver();
			xmlNamespaceResolver49.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver49.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver49.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver49.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver49.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver49.Add("pids", "clr-namespace:CarScannerXamarinForms.OBD2.PIDS");
			xmlNamespaceResolver49.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver49.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xmlNamespaceResolver49.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xmlNamespaceResolver49.Add("xct", "http://xamarin.com/schemas/2020/toolkit");
			xamlServiceProvider49.Add(typeFromHandle99, new XamlTypeResolver(xmlNamespaceResolver49, typeof(SettingsCustomPIDEditorV3).GetTypeInfo().Assembly));
			xamlServiceProvider49.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(230, 28)));
			object obj92 = markupExtension49.ProvideValue(xamlServiceProvider49);
			bindingExtension41.Source = obj92;
			bindingExtension41.Path = "Title";
			BindingBase bindingBase41 = bindingExtension41.ProvideValue(null);
			label.SetBinding(Label.TextProperty, bindingBase41);
			customCell13.SetValue(CustomCell.ContentProperty, label);
			section20.Add(customCell13);
			settingsView.Root.Add(section20);
			this.SetValue(ContentPage.ContentProperty, settingsView);
		}

		// Token: 0x06002215 RID: 8725 RVA: 0x001A86CF File Offset: 0x001A68CF
		[CompilerGenerated]
		private void <BtnTest_Clicked>b__8_0(OBDRequest request2, string data)
		{
			SettingsCustomPIDEditorV3.<>c__DisplayClass8_1 CS$<>8__locals1 = new SettingsCustomPIDEditorV3.<>c__DisplayClass8_1();
			CS$<>8__locals1.<>4__this = this;
			CS$<>8__locals1.data = data;
			Device.BeginInvokeOnMainThread(delegate
			{
				SettingsCustomPIDEditorV3.<>c__DisplayClass8_1.<<BtnTest_Clicked>b__3>d <<BtnTest_Clicked>b__3>d;
				<<BtnTest_Clicked>b__3>d.<>t__builder = AsyncVoidMethodBuilder.Create();
				<<BtnTest_Clicked>b__3>d.<>4__this = CS$<>8__locals1;
				<<BtnTest_Clicked>b__3>d.<>1__state = -1;
				<<BtnTest_Clicked>b__3>d.<>t__builder.Start<SettingsCustomPIDEditorV3.<>c__DisplayClass8_1.<<BtnTest_Clicked>b__3>d>(ref <<BtnTest_Clicked>b__3>d);
			});
		}

		// Token: 0x06002216 RID: 8726 RVA: 0x001A86F4 File Offset: 0x001A68F4
		private void __InitComponentRuntime()
		{
			Extensions.LoadFromXaml<SettingsCustomPIDEditorV3>(this, typeof(SettingsCustomPIDEditorV3));
			this.settingsLayoutRoot = NameScopeExtensions.FindByName<SettingsView>(this, "settingsLayoutRoot");
			this.vagUnitEntry = NameScopeExtensions.FindByName<EntryCell>(this, "vagUnitEntry");
			this.vagGroupEntry = NameScopeExtensions.FindByName<EntryCell>(this, "vagGroupEntry");
			this.vagItemInGroupEntry = NameScopeExtensions.FindByName<EntryCell>(this, "vagItemInGroupEntry");
			this.btnApplyVagGroup = NameScopeExtensions.FindByName<ButtonCell>(this, "btnApplyVagGroup");
			this.btnUnitsOverride = NameScopeExtensions.FindByName<ButtonCell>(this, "btnUnitsOverride");
			this.pickerRole = NameScopeExtensions.FindByName<Picker>(this, "pickerRole");
			this.testLabelCell = NameScopeExtensions.FindByName<CustomCell>(this, "testLabelCell");
		}

		// Token: 0x04001029 RID: 4137
		[CompilerGenerated]
		private CustomPID <CPid>k__BackingField;

		// Token: 0x0400102A RID: 4138
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private SettingsView settingsLayoutRoot;

		// Token: 0x0400102B RID: 4139
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private EntryCell vagUnitEntry;

		// Token: 0x0400102C RID: 4140
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private EntryCell vagGroupEntry;

		// Token: 0x0400102D RID: 4141
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private EntryCell vagItemInGroupEntry;

		// Token: 0x0400102E RID: 4142
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private ButtonCell btnApplyVagGroup;

		// Token: 0x0400102F RID: 4143
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private ButtonCell btnUnitsOverride;

		// Token: 0x04001030 RID: 4144
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private Picker pickerRole;

		// Token: 0x04001031 RID: 4145
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private CustomCell testLabelCell;

		// Token: 0x020002B6 RID: 694
		[CompilerGenerated]
		private sealed class <>c__DisplayClass8_0
		{
			// Token: 0x06002217 RID: 8727 RVA: 0x00002050 File Offset: 0x00000250
			public <>c__DisplayClass8_0()
			{
			}

			// Token: 0x06002218 RID: 8728 RVA: 0x001A879C File Offset: 0x001A699C
			internal void <BtnTest_Clicked>b__1(OBDRequest request2, byte[] data, bool decodeResult, string responseHeader)
			{
				if (data != null && data.Length != 0)
				{
					SettingsCustomPIDEditorV3.<>c__DisplayClass8_2 CS$<>8__locals1 = new SettingsCustomPIDEditorV3.<>c__DisplayClass8_2();
					CS$<>8__locals1.CS$<>8__locals1 = this;
					if (this.<>4__this.CPid.Type == CustomPIDType.Formula)
					{
						CS$<>8__locals1.sb = new StringBuilder(data.Length * 4);
						for (int i = 0; i < data.Length; i++)
						{
							string text = CustomPID.DICT_LETTERS[i];
							byte b = data[i];
							CS$<>8__locals1.sb.Append(text);
							CS$<>8__locals1.sb.Append(":");
							CS$<>8__locals1.sb.Append(b.ToString("X2"));
							CS$<>8__locals1.sb.Append("\n");
						}
					}
					else
					{
						CS$<>8__locals1.sb = new StringBuilder(data.Length * 4);
						for (int j = 0; j < data.Length; j++)
						{
							byte b2 = data[j];
							CS$<>8__locals1.sb.Append(j.ToString());
							CS$<>8__locals1.sb.Append(":");
							CS$<>8__locals1.sb.Append(b2.ToString("X2"));
							CS$<>8__locals1.sb.Append("\n");
						}
					}
					Device.BeginInvokeOnMainThread(delegate
					{
						SettingsCustomPIDEditorV3.<>c__DisplayClass8_2.<<BtnTest_Clicked>b__4>d <<BtnTest_Clicked>b__4>d;
						<<BtnTest_Clicked>b__4>d.<>t__builder = AsyncVoidMethodBuilder.Create();
						<<BtnTest_Clicked>b__4>d.<>4__this = CS$<>8__locals1;
						<<BtnTest_Clicked>b__4>d.<>1__state = -1;
						<<BtnTest_Clicked>b__4>d.<>t__builder.Start<SettingsCustomPIDEditorV3.<>c__DisplayClass8_2.<<BtnTest_Clicked>b__4>d>(ref <<BtnTest_Clicked>b__4>d);
					});
				}
			}

			// Token: 0x06002219 RID: 8729 RVA: 0x001A88D0 File Offset: 0x001A6AD0
			internal void <BtnTest_Clicked>b__2(object senderModel, PropertyChangedEventArgs propertyArgs)
			{
				if (propertyArgs.PropertyName == "FloatValue")
				{
					Action action;
					if ((action = this.<>9__5) == null)
					{
						action = (this.<>9__5 = async delegate
						{
							this.<>4__this.testLabelCell.Title = string.Concat(new string[]
							{
								this.<>4__this.testLabelCell.Title,
								"Calculation result = ",
								this.model.FloatValue.ToString(),
								" ",
								this.model.Units,
								"\n"
							});
							this.<>4__this.UpdateTestCellVisible();
						});
					}
					Device.BeginInvokeOnMainThread(action);
				}
			}

			// Token: 0x0600221A RID: 8730 RVA: 0x001A8914 File Offset: 0x001A6B14
			internal async void <BtnTest_Clicked>b__5()
			{
				this.<>4__this.testLabelCell.Title = string.Concat(new string[]
				{
					this.<>4__this.testLabelCell.Title,
					"Calculation result = ",
					this.model.FloatValue.ToString(),
					" ",
					this.model.Units,
					"\n"
				});
				this.<>4__this.UpdateTestCellVisible();
			}

			// Token: 0x04001032 RID: 4146
			public LiveDataPIDModel model;

			// Token: 0x04001033 RID: 4147
			public SettingsCustomPIDEditorV3 <>4__this;

			// Token: 0x04001034 RID: 4148
			public Action <>9__5;

			// Token: 0x020002B7 RID: 695
			[StructLayout(LayoutKind.Auto)]
			private struct <<BtnTest_Clicked>b__5>d : IAsyncStateMachine
			{
				// Token: 0x0600221B RID: 8731 RVA: 0x001A894C File Offset: 0x001A6B4C
				void IAsyncStateMachine.MoveNext()
				{
					SettingsCustomPIDEditorV3.<>c__DisplayClass8_0 CS$<>8__locals1 = this;
					try
					{
						CS$<>8__locals1.<>4__this.testLabelCell.Title = string.Concat(new string[]
						{
							CS$<>8__locals1.<>4__this.testLabelCell.Title,
							"Calculation result = ",
							CS$<>8__locals1.model.FloatValue.ToString(),
							" ",
							CS$<>8__locals1.model.Units,
							"\n"
						});
						CS$<>8__locals1.<>4__this.UpdateTestCellVisible();
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

				// Token: 0x0600221C RID: 8732 RVA: 0x001A8A14 File Offset: 0x001A6C14
				[DebuggerHidden]
				void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
				{
					this.<>t__builder.SetStateMachine(stateMachine);
				}

				// Token: 0x04001035 RID: 4149
				public int <>1__state;

				// Token: 0x04001036 RID: 4150
				public AsyncVoidMethodBuilder <>t__builder;

				// Token: 0x04001037 RID: 4151
				public SettingsCustomPIDEditorV3.<>c__DisplayClass8_0 <>4__this;
			}
		}

		// Token: 0x020002B8 RID: 696
		[CompilerGenerated]
		private sealed class <>c__DisplayClass8_1
		{
			// Token: 0x0600221D RID: 8733 RVA: 0x00002050 File Offset: 0x00000250
			public <>c__DisplayClass8_1()
			{
			}

			// Token: 0x0600221E RID: 8734 RVA: 0x001A8A24 File Offset: 0x001A6C24
			internal async void <BtnTest_Clicked>b__3()
			{
				this.<>4__this.testLabelCell.Title = this.<>4__this.testLabelCell.Title + "ELM:\n" + this.data.Replace('\r', '\n') + "\n";
				this.<>4__this.UpdateTestCellVisible();
			}

			// Token: 0x04001038 RID: 4152
			public string data;

			// Token: 0x04001039 RID: 4153
			public SettingsCustomPIDEditorV3 <>4__this;

			// Token: 0x020002B9 RID: 697
			[StructLayout(LayoutKind.Auto)]
			private struct <<BtnTest_Clicked>b__3>d : IAsyncStateMachine
			{
				// Token: 0x0600221F RID: 8735 RVA: 0x001A8A5C File Offset: 0x001A6C5C
				void IAsyncStateMachine.MoveNext()
				{
					SettingsCustomPIDEditorV3.<>c__DisplayClass8_1 CS$<>8__locals1 = this;
					try
					{
						CS$<>8__locals1.<>4__this.testLabelCell.Title = CS$<>8__locals1.<>4__this.testLabelCell.Title + "ELM:\n" + CS$<>8__locals1.data.Replace('\r', '\n') + "\n";
						CS$<>8__locals1.<>4__this.UpdateTestCellVisible();
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

				// Token: 0x06002220 RID: 8736 RVA: 0x001A8AF8 File Offset: 0x001A6CF8
				[DebuggerHidden]
				void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
				{
					this.<>t__builder.SetStateMachine(stateMachine);
				}

				// Token: 0x0400103A RID: 4154
				public int <>1__state;

				// Token: 0x0400103B RID: 4155
				public AsyncVoidMethodBuilder <>t__builder;

				// Token: 0x0400103C RID: 4156
				public SettingsCustomPIDEditorV3.<>c__DisplayClass8_1 <>4__this;
			}
		}

		// Token: 0x020002BA RID: 698
		[CompilerGenerated]
		private sealed class <>c__DisplayClass8_2
		{
			// Token: 0x06002221 RID: 8737 RVA: 0x00002050 File Offset: 0x00000250
			public <>c__DisplayClass8_2()
			{
			}

			// Token: 0x06002222 RID: 8738 RVA: 0x001A8B08 File Offset: 0x001A6D08
			internal async void <BtnTest_Clicked>b__4()
			{
				this.CS$<>8__locals1.<>4__this.testLabelCell.Title = this.CS$<>8__locals1.<>4__this.testLabelCell.Title + "Data for calculation:\n" + this.sb.ToString() + "\n";
				this.CS$<>8__locals1.model.Unsubscribe();
				this.CS$<>8__locals1.<>4__this.UpdateTestCellVisible();
			}

			// Token: 0x0400103D RID: 4157
			public StringBuilder sb;

			// Token: 0x0400103E RID: 4158
			public SettingsCustomPIDEditorV3.<>c__DisplayClass8_0 CS$<>8__locals1;

			// Token: 0x020002BB RID: 699
			[StructLayout(LayoutKind.Auto)]
			private struct <<BtnTest_Clicked>b__4>d : IAsyncStateMachine
			{
				// Token: 0x06002223 RID: 8739 RVA: 0x001A8B40 File Offset: 0x001A6D40
				void IAsyncStateMachine.MoveNext()
				{
					SettingsCustomPIDEditorV3.<>c__DisplayClass8_2 CS$<>8__locals1 = this;
					try
					{
						CS$<>8__locals1.CS$<>8__locals1.<>4__this.testLabelCell.Title = CS$<>8__locals1.CS$<>8__locals1.<>4__this.testLabelCell.Title + "Data for calculation:\n" + CS$<>8__locals1.sb.ToString() + "\n";
						CS$<>8__locals1.CS$<>8__locals1.model.Unsubscribe();
						CS$<>8__locals1.CS$<>8__locals1.<>4__this.UpdateTestCellVisible();
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

				// Token: 0x06002224 RID: 8740 RVA: 0x001A8BF4 File Offset: 0x001A6DF4
				[DebuggerHidden]
				void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
				{
					this.<>t__builder.SetStateMachine(stateMachine);
				}

				// Token: 0x0400103F RID: 4159
				public int <>1__state;

				// Token: 0x04001040 RID: 4160
				public AsyncVoidMethodBuilder <>t__builder;

				// Token: 0x04001041 RID: 4161
				public SettingsCustomPIDEditorV3.<>c__DisplayClass8_2 <>4__this;
			}
		}

		// Token: 0x020002BC RID: 700
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <UpdateTestCellVisible>d__7 : IAsyncStateMachine
		{
			// Token: 0x06002225 RID: 8741 RVA: 0x001A8C04 File Offset: 0x001A6E04
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				SettingsCustomPIDEditorV3 settingsCustomPIDEditorV = this;
				try
				{
					TaskAwaiter taskAwaiter;
					if (num != 0)
					{
						if (num == 1)
						{
							TaskAwaiter taskAwaiter2;
							taskAwaiter = taskAwaiter2;
							taskAwaiter2 = default(TaskAwaiter);
							num2 = -1;
							goto IL_0101;
						}
						tempres = settingsCustomPIDEditorV.testLabelCell.Title;
						taskAwaiter = Task.Delay(300).GetAwaiter();
						if (!taskAwaiter.IsCompleted)
						{
							num2 = 0;
							TaskAwaiter taskAwaiter2 = taskAwaiter;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, SettingsCustomPIDEditorV3.<UpdateTestCellVisible>d__7>(ref taskAwaiter, ref this);
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
					if (!(tempres == settingsCustomPIDEditorV.testLabelCell.Title))
					{
						goto IL_0114;
					}
					settingsCustomPIDEditorV.testLabelCell.IsVisible = false;
					taskAwaiter = Task.Delay(300).GetAwaiter();
					if (!taskAwaiter.IsCompleted)
					{
						num2 = 1;
						TaskAwaiter taskAwaiter2 = taskAwaiter;
						this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, SettingsCustomPIDEditorV3.<UpdateTestCellVisible>d__7>(ref taskAwaiter, ref this);
						return;
					}
					IL_0101:
					taskAwaiter.GetResult();
					settingsCustomPIDEditorV.testLabelCell.IsVisible = true;
					IL_0114:;
				}
				catch (Exception ex)
				{
					num2 = -2;
					tempres = null;
					this.<>t__builder.SetException(ex);
					return;
				}
				num2 = -2;
				tempres = null;
				this.<>t__builder.SetResult();
			}

			// Token: 0x06002226 RID: 8742 RVA: 0x001A8D7C File Offset: 0x001A6F7C
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x04001042 RID: 4162
			public int <>1__state;

			// Token: 0x04001043 RID: 4163
			public AsyncVoidMethodBuilder <>t__builder;

			// Token: 0x04001044 RID: 4164
			public SettingsCustomPIDEditorV3 <>4__this;

			// Token: 0x04001045 RID: 4165
			private string <tempres>5__2;

			// Token: 0x04001046 RID: 4166
			private TaskAwaiter <>u__1;
		}

		// Token: 0x020002BD RID: 701
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <btnUnitsOverride_Clicked>d__5 : IAsyncStateMachine
		{
			// Token: 0x06002227 RID: 8743 RVA: 0x001A8D8C File Offset: 0x001A6F8C
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				SettingsCustomPIDEditorV3 settingsCustomPIDEditorV = this;
				try
				{
					TaskAwaiter taskAwaiter;
					if (num != 0)
					{
						Page page;
						if (App.UseLegacyUI)
						{
							page = new SettingsPIDOverridePage(settingsCustomPIDEditorV.CPid);
						}
						else
						{
							page = new SettingsPIDOverrideEditorPageV3(settingsCustomPIDEditorV.CPid);
						}
						taskAwaiter = settingsCustomPIDEditorV.Navigation.PushAsync(page).GetAwaiter();
						if (!taskAwaiter.IsCompleted)
						{
							num2 = 0;
							TaskAwaiter taskAwaiter2 = taskAwaiter;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, SettingsCustomPIDEditorV3.<btnUnitsOverride_Clicked>d__5>(ref taskAwaiter, ref this);
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

			// Token: 0x06002228 RID: 8744 RVA: 0x001A8E6C File Offset: 0x001A706C
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x04001047 RID: 4167
			public int <>1__state;

			// Token: 0x04001048 RID: 4168
			public AsyncVoidMethodBuilder <>t__builder;

			// Token: 0x04001049 RID: 4169
			public SettingsCustomPIDEditorV3 <>4__this;

			// Token: 0x0400104A RID: 4170
			private TaskAwaiter <>u__1;
		}
	}
}
