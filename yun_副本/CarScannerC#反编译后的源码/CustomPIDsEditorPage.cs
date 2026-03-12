using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using CarScannerXamarinForms.Common.XAMLConverters;
using CarScannerXamarinForms.OBD2;
using CarScannerXamarinForms.OBD2.PIDS;
using CarScannerXamarinForms.OBD2.VWTP20;
using CarScannerXamarinForms.Pages;
using CarScannerXamarinForms.PlatformAdapters;
using CarScannerXamarinForms.Settings;
using CarScannerXamarinForms.Settings.SettingsV3;
using CarScannerXamarinForms.UserControls;
using CarScannerXamarinForms.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Internals;
using Xamarin.Forms.PlatformConfiguration.iOSSpecific;
using Xamarin.Forms.Xaml;
using Xamarin.Forms.Xaml.Diagnostics;
using Xamarin.Forms.Xaml.Internals;

namespace CarScannerXamarinForms
{
	// Token: 0x02000162 RID: 354
	[XamlFilePath("Settings\\CustomPIDsEditorPage.xaml")]
	public class CustomPIDsEditorPage : ContentPage
	{
		// Token: 0x06001520 RID: 5408 RVA: 0x000874BC File Offset: 0x000856BC
		public CustomPIDsEditorPage(CustomPID cpid)
		{
			this.InitializeComponent();
			Array values = Enum.GetValues(typeof(Roles));
			this.pickerRole.ItemsSource = values;
			this.CPid = cpid;
			base.BindingContext = this.CPid;
			this.panelFormula.BindingContext = this.CPid;
			base.Content.BindingContext = this.CPid;
		}

		// Token: 0x06001521 RID: 5409 RVA: 0x00087528 File Offset: 0x00085728
		private async void BtnInsertPidInFormula_Clicked(object sender, EventArgs e)
		{
			IPID ipid = await PIDSelector.SelectPIDAsync(null, (PID x) => x is IPIDFloatValue);
			if (ipid != null)
			{
				string @string = Translate.GetString("ios_Cancel");
				string byName = "{" + ipid.Name + "}";
				string byID = "PID(" + ipid.Id.ToString() + ")";
				string text = await this.DisplayActionSheetCustom("Insert PID by ID or by name:", @string, null, new string[] { byName, byID });
				if (text == byName)
				{
					this.CPid.Formula = this.CPid.Formula + byName;
				}
				else if (text == byID)
				{
					this.CPid.Formula = this.CPid.Formula + byID;
				}
				byName = null;
				byID = null;
			}
		}

		// Token: 0x06001522 RID: 5410 RVA: 0x00087560 File Offset: 0x00085760
		private void BtnTest_Clicked(object sender, EventArgs e)
		{
			try
			{
				if (!App.OBDSimulator.IsActive && (App.OBDReader.CurrentStatus == OBDDataReaderStatus.ConnectedToECU || App.OBDReader.CurrentStatus == OBDDataReaderStatus.ConnectedToELM))
				{
					CustomPIDsEditorPage.<>c__DisplayClass2_0 CS$<>8__locals1 = new CustomPIDsEditorPage.<>c__DisplayClass2_0();
					CS$<>8__locals1.<>4__this = this;
					this.lbTestResult.FormattedText = new FormattedString();
					CS$<>8__locals1.model = new LiveDataPIDModel();
					CS$<>8__locals1.model.SelectedPID = this.CPid;
					List<OBDRequest> list = new List<OBDRequest>(1);
					CS$<>8__locals1.model.GetRequests(list, null, "");
					OBDRequest obdrequest = list[0];
					obdrequest.Repeat = false;
					obdrequest.ResponseReceived += delegate(OBDRequest request2, string data)
					{
						Device.BeginInvokeOnMainThread(delegate
						{
							this.lbTestResult.FormattedText.Spans.Add(new Span
							{
								Text = "ELM:\n",
								FontAttributes = 1
							});
							this.lbTestResult.FormattedText.Spans.Add(new Span
							{
								Text = data.Replace('\r', '\n') + "\n"
							});
						});
					};
					obdrequest.ResponseDecoded += delegate(OBDRequest request2, byte[] data, bool decodeResult, string responseHeader)
					{
						if (data != null && data.Length != 0)
						{
							CustomPIDsEditorPage.<>c__DisplayClass2_2 CS$<>8__locals3 = new CustomPIDsEditorPage.<>c__DisplayClass2_2();
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
								CustomPIDsEditorPage.<>c__DisplayClass2_2.<<BtnTest_Clicked>b__4>d <<BtnTest_Clicked>b__4>d;
								<<BtnTest_Clicked>b__4>d.<>t__builder = AsyncVoidMethodBuilder.Create();
								<<BtnTest_Clicked>b__4>d.<>4__this = CS$<>8__locals3;
								<<BtnTest_Clicked>b__4>d.<>1__state = -1;
								<<BtnTest_Clicked>b__4>d.<>t__builder.Start<CustomPIDsEditorPage.<>c__DisplayClass2_2.<<BtnTest_Clicked>b__4>d>(ref <<BtnTest_Clicked>b__4>d);
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
									try
									{
										CS$<>8__locals1.<>4__this.lbTestResult.FormattedText.Spans.Add(new Span
										{
											Text = "Calculation result = ",
											FontAttributes = 1
										});
										CS$<>8__locals1.<>4__this.lbTestResult.FormattedText.Spans.Add(new Span
										{
											Text = CS$<>8__locals1.model.FloatValue.ToString() + " " + CS$<>8__locals1.model.Units + "\n"
										});
									}
									catch (Exception)
									{
									}
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

		// Token: 0x06001523 RID: 5411 RVA: 0x000027D4 File Offset: 0x000009D4
		private void Model_PropertyChanged(object sender, PropertyChangedEventArgs e)
		{
		}

		// Token: 0x06001524 RID: 5412 RVA: 0x000027D4 File Offset: 0x000009D4
		private void Request_ResponseDecoded(OBDRequest request, byte[] data, bool decodeResult, string responseHeader)
		{
		}

		// Token: 0x17000F3F RID: 3903
		// (get) Token: 0x06001525 RID: 5413 RVA: 0x00087670 File Offset: 0x00085870
		// (set) Token: 0x06001526 RID: 5414 RVA: 0x00087678 File Offset: 0x00085878
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

		// Token: 0x06001527 RID: 5415 RVA: 0x000027D4 File Offset: 0x000009D4
		private void Handle_SizeChanged(object sender, EventArgs e)
		{
		}

		// Token: 0x06001528 RID: 5416 RVA: 0x000027D4 File Offset: 0x000009D4
		private void Handle_Appearing(object sender, EventArgs e)
		{
		}

		// Token: 0x06001529 RID: 5417 RVA: 0x00087684 File Offset: 0x00085884
		private async void Handle_Disappearing(object sender, EventArgs e)
		{
			CustomPIDViewModel.CurrentCustom.Save();
		}

		// Token: 0x0600152A RID: 5418 RVA: 0x000876B4 File Offset: 0x000858B4
		private void numericEntry_TextChanged(object sender, TextChangedEventArgs e)
		{
			string text = e.NewTextValue.Replace(".", CultureInfo.CurrentUICulture.NumberFormat.NumberDecimalSeparator).Replace(",", CultureInfo.CurrentUICulture.NumberFormat.NumberDecimalSeparator);
			Entry entry = sender as Entry;
			CustomPID customPID = entry.BindingContext as CustomPID;
			PropertyInfo property = customPID.GetType().GetProperty(entry.ClassId);
			double num = 0.0;
			if (e.NewTextValue == "")
			{
				property.SetValue(customPID, 0);
				return;
			}
			if (double.TryParse(text, out num))
			{
				property.SetValue(customPID, num);
				return;
			}
			if (!double.TryParse(e.OldTextValue, out num))
			{
				num = 0.0;
			}
			property.SetValue(customPID, num);
			entry.Text = e.OldTextValue;
		}

		// Token: 0x0600152B RID: 5419 RVA: 0x00087798 File Offset: 0x00085998
		private async void btnUnitsOverride_Clicked(object sender, EventArgs e)
		{
			if (App.UseLegacyUI)
			{
				if (PlatformHelper.IsiOS)
				{
					SettingsPIDOverridePage settingsPIDOverridePage = new SettingsPIDOverridePage(this.CPid);
					await base.Navigation.PushAsync(settingsPIDOverridePage);
				}
			}
			else
			{
				SettingsPIDOverrideEditorPageV3 settingsPIDOverrideEditorPageV = new SettingsPIDOverrideEditorPageV3(this.CPid);
				await base.Navigation.PushAsync(settingsPIDOverrideEditorPageV);
			}
		}

		// Token: 0x0600152C RID: 5420 RVA: 0x000877D0 File Offset: 0x000859D0
		private void btnApplyVagGroup_Tapped(object sender, EventArgs e)
		{
			string text = this.vagGroupEntry.Text;
			string text2 = this.vagUnitEntry.Text;
			if (text2.Length < 2)
			{
				text2 = "0" + text2;
			}
			text2 = text2.ToUpperInvariant();
			int num = (int)this.vagItemInGroupEntry.Value;
			int num2;
			if (int.TryParse(text, out num2) && num2 > 0 && num2 <= 255 && num >= 1 && num <= 4)
			{
				this.CPid.Command = "VWTP:" + VWTPManager.UnitToCANAddress(text2) + ":21" + num2.ToString("X2");
				this.CPid.StartByteId = num - 1;
				this.CPid.BeforeCommand = "ATCAF0;ATV1;ATSP6;ATCM000";
				this.CPid.AfterCommand = "ATSPDEF;ATCAF1;ATV0";
				this.CPid.Header = "000";
			}
		}

		// Token: 0x0600152D RID: 5421 RVA: 0x000878A4 File Offset: 0x00085AA4
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private void InitializeComponent()
		{
			if (ResourceLoader.IsEnabled && ResourceLoader.CanProvideContentFor(new ResourceLoader.ResourceLoadingQuery
			{
				AssemblyName = typeof(CustomPIDsEditorPage).GetTypeInfo().Assembly.GetName(),
				ResourcePath = "Settings/CustomPIDsEditorPage.xaml",
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
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension = new DynamicResourceExtension(), new Uri("Settings\\CustomPIDsEditorPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 16, 5);
			UnitsToIntConverter unitsToIntConverter;
			VisualDiagnostics.RegisterSourceInfo(unitsToIntConverter = new UnitsToIntConverter(), new Uri("Settings\\CustomPIDsEditorPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 21, 14);
			BoolToNegativeConverter boolToNegativeConverter;
			VisualDiagnostics.RegisterSourceInfo(boolToNegativeConverter = new BoolToNegativeConverter(), new Uri("Settings\\CustomPIDsEditorPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 22, 14);
			DoubleToStringConverter doubleToStringConverter;
			VisualDiagnostics.RegisterSourceInfo(doubleToStringConverter = new DoubleToStringConverter(), new Uri("Settings\\CustomPIDsEditorPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 23, 14);
			SkipCyclesIntToPriorityStringConverter skipCyclesIntToPriorityStringConverter;
			VisualDiagnostics.RegisterSourceInfo(skipCyclesIntToPriorityStringConverter = new SkipCyclesIntToPriorityStringConverter(), new Uri("Settings\\CustomPIDsEditorPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 24, 14);
			CustomPIDTypeFormulaToTrue customPIDTypeFormulaToTrue;
			VisualDiagnostics.RegisterSourceInfo(customPIDTypeFormulaToTrue = new CustomPIDTypeFormulaToTrue(), new Uri("Settings\\CustomPIDsEditorPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 25, 14);
			EnumToIntConverter enumToIntConverter;
			VisualDiagnostics.RegisterSourceInfo(enumToIntConverter = new EnumToIntConverter(), new Uri("Settings\\CustomPIDsEditorPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 26, 14);
			EnumValueToTrueConverter enumValueToTrueConverter;
			VisualDiagnostics.RegisterSourceInfo(enumValueToTrueConverter = new EnumValueToTrueConverter(), new Uri("Settings\\CustomPIDsEditorPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 27, 14);
			EnumValueToFalseConverter enumValueToFalseConverter;
			VisualDiagnostics.RegisterSourceInfo(enumValueToFalseConverter = new EnumValueToFalseConverter(), new Uri("Settings\\CustomPIDsEditorPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 28, 14);
			UnitsToStringInvariantConverter unitsToStringInvariantConverter;
			VisualDiagnostics.RegisterSourceInfo(unitsToStringInvariantConverter = new UnitsToStringInvariantConverter(), new Uri("Settings\\CustomPIDsEditorPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 29, 14);
			UnitsToStringConverter unitsToStringConverter;
			VisualDiagnostics.RegisterSourceInfo(unitsToStringConverter = new UnitsToStringConverter(), new Uri("Settings\\CustomPIDsEditorPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 30, 14);
			ResourceDictionary resourceDictionary;
			VisualDiagnostics.RegisterSourceInfo(resourceDictionary = new ResourceDictionary(), new Uri("Settings\\CustomPIDsEditorPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 20, 10);
			OnPlatform<Thickness> onPlatform;
			VisualDiagnostics.RegisterSourceInfo(onPlatform = new OnPlatform<Thickness>(), new Uri("Settings\\CustomPIDsEditorPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 38, 22);
			Translate translate;
			VisualDiagnostics.RegisterSourceInfo(translate = new Translate(), new Uri("Settings\\CustomPIDsEditorPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 45, 24);
			Label label;
			VisualDiagnostics.RegisterSourceInfo(label = new Label(), new Uri("Settings\\CustomPIDsEditorPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 45, 18);
			BindingExtension bindingExtension;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension = new BindingExtension(), new Uri("Settings\\CustomPIDsEditorPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 46, 24);
			Entry entry;
			VisualDiagnostics.RegisterSourceInfo(entry = new Entry(), new Uri("Settings\\CustomPIDsEditorPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 46, 18);
			Translate translate2;
			VisualDiagnostics.RegisterSourceInfo(translate2 = new Translate(), new Uri("Settings\\CustomPIDsEditorPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 47, 60);
			Label label2;
			VisualDiagnostics.RegisterSourceInfo(label2 = new Label(), new Uri("Settings\\CustomPIDsEditorPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 47, 18);
			BindingExtension bindingExtension2;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension2 = new BindingExtension(), new Uri("Settings\\CustomPIDsEditorPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 48, 24);
			Entry entry2;
			VisualDiagnostics.RegisterSourceInfo(entry2 = new Entry(), new Uri("Settings\\CustomPIDsEditorPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 48, 18);
			StaticResourceExtension staticResourceExtension;
			VisualDiagnostics.RegisterSourceInfo(staticResourceExtension = new StaticResourceExtension(), new Uri("Settings\\CustomPIDsEditorPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 49, 24);
			BindingExtension bindingExtension3;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension3 = new BindingExtension(), new Uri("Settings\\CustomPIDsEditorPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 49, 24);
			Translate translate3;
			VisualDiagnostics.RegisterSourceInfo(translate3 = new Translate(), new Uri("Settings\\CustomPIDsEditorPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 49, 127);
			Label label3;
			VisualDiagnostics.RegisterSourceInfo(label3 = new Label(), new Uri("Settings\\CustomPIDsEditorPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 49, 18);
			StaticResourceExtension staticResourceExtension2;
			VisualDiagnostics.RegisterSourceInfo(staticResourceExtension2 = new StaticResourceExtension(), new Uri("Settings\\CustomPIDsEditorPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 50, 24);
			BindingExtension bindingExtension4;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension4 = new BindingExtension(), new Uri("Settings\\CustomPIDsEditorPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 50, 24);
			BindingExtension bindingExtension5;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension5 = new BindingExtension(), new Uri("Settings\\CustomPIDsEditorPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 50, 127);
			Entry entry3;
			VisualDiagnostics.RegisterSourceInfo(entry3 = new Entry(), new Uri("Settings\\CustomPIDsEditorPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 50, 18);
			StaticResourceExtension staticResourceExtension3;
			VisualDiagnostics.RegisterSourceInfo(staticResourceExtension3 = new StaticResourceExtension(), new Uri("Settings\\CustomPIDsEditorPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 51, 24);
			BindingExtension bindingExtension6;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension6 = new BindingExtension(), new Uri("Settings\\CustomPIDsEditorPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 51, 24);
			Translate translate4;
			VisualDiagnostics.RegisterSourceInfo(translate4 = new Translate(), new Uri("Settings\\CustomPIDsEditorPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 51, 127);
			Label label4;
			VisualDiagnostics.RegisterSourceInfo(label4 = new Label(), new Uri("Settings\\CustomPIDsEditorPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 51, 18);
			StaticResourceExtension staticResourceExtension4;
			VisualDiagnostics.RegisterSourceInfo(staticResourceExtension4 = new StaticResourceExtension(), new Uri("Settings\\CustomPIDsEditorPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 52, 24);
			BindingExtension bindingExtension7;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension7 = new BindingExtension(), new Uri("Settings\\CustomPIDsEditorPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 52, 24);
			BindingExtension bindingExtension8;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension8 = new BindingExtension(), new Uri("Settings\\CustomPIDsEditorPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 52, 127);
			Entry entry4;
			VisualDiagnostics.RegisterSourceInfo(entry4 = new Entry(), new Uri("Settings\\CustomPIDsEditorPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 52, 18);
			StaticResourceExtension staticResourceExtension5;
			VisualDiagnostics.RegisterSourceInfo(staticResourceExtension5 = new StaticResourceExtension(), new Uri("Settings\\CustomPIDsEditorPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 54, 30);
			BindingExtension bindingExtension9;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension9 = new BindingExtension(), new Uri("Settings\\CustomPIDsEditorPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 54, 30);
			Translate translate5;
			VisualDiagnostics.RegisterSourceInfo(translate5 = new Translate(), new Uri("Settings\\CustomPIDsEditorPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 57, 28);
			Label label5;
			VisualDiagnostics.RegisterSourceInfo(label5 = new Label(), new Uri("Settings\\CustomPIDsEditorPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 57, 22);
			BindingExtension bindingExtension10;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension10 = new BindingExtension(), new Uri("Settings\\CustomPIDsEditorPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 63, 25);
			Translate translate6;
			VisualDiagnostics.RegisterSourceInfo(translate6 = new Translate(), new Uri("Settings\\CustomPIDsEditorPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 65, 29);
			CustomPIDType customPIDType = CustomPIDType.Formula;
			RadioButtonWithColor radioButtonWithColor;
			VisualDiagnostics.RegisterSourceInfo(radioButtonWithColor = new RadioButtonWithColor(), new Uri("Settings\\CustomPIDsEditorPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 64, 26);
			Translate translate7;
			VisualDiagnostics.RegisterSourceInfo(translate7 = new Translate(), new Uri("Settings\\CustomPIDsEditorPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 69, 29);
			CustomPIDType customPIDType2 = CustomPIDType.ByteSet;
			RadioButtonWithColor radioButtonWithColor2;
			VisualDiagnostics.RegisterSourceInfo(radioButtonWithColor2 = new RadioButtonWithColor(), new Uri("Settings\\CustomPIDsEditorPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 68, 26);
			Translate translate8;
			VisualDiagnostics.RegisterSourceInfo(translate8 = new Translate(), new Uri("Settings\\CustomPIDsEditorPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 73, 29);
			CustomPIDType customPIDType3 = CustomPIDType.BitValue;
			RadioButtonWithColor radioButtonWithColor3;
			VisualDiagnostics.RegisterSourceInfo(radioButtonWithColor3 = new RadioButtonWithColor(), new Uri("Settings\\CustomPIDsEditorPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 72, 26);
			Translate translate9;
			VisualDiagnostics.RegisterSourceInfo(translate9 = new Translate(), new Uri("Settings\\CustomPIDsEditorPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 77, 29);
			CustomPIDType customPIDType4 = CustomPIDType.Action;
			RadioButtonWithColor radioButtonWithColor4;
			VisualDiagnostics.RegisterSourceInfo(radioButtonWithColor4 = new RadioButtonWithColor(), new Uri("Settings\\CustomPIDsEditorPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 76, 26);
			CustomPIDType customPIDType5 = CustomPIDType.VWTPGroupItem;
			RadioButtonWithColor radioButtonWithColor5;
			VisualDiagnostics.RegisterSourceInfo(radioButtonWithColor5 = new RadioButtonWithColor(), new Uri("Settings\\CustomPIDsEditorPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 80, 26);
			StackLayout stackLayout;
			VisualDiagnostics.RegisterSourceInfo(stackLayout = new StackLayout(), new Uri("Settings\\CustomPIDsEditorPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 59, 22);
			StaticResourceExtension staticResourceExtension6;
			VisualDiagnostics.RegisterSourceInfo(staticResourceExtension6 = new StaticResourceExtension(), new Uri("Settings\\CustomPIDsEditorPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 89, 25);
			BindingExtension bindingExtension11;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension11 = new BindingExtension(), new Uri("Settings\\CustomPIDsEditorPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 89, 25);
			Translate translate10;
			VisualDiagnostics.RegisterSourceInfo(translate10 = new Translate(), new Uri("Settings\\CustomPIDsEditorPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 92, 36);
			Label label6;
			VisualDiagnostics.RegisterSourceInfo(label6 = new Label(), new Uri("Settings\\CustomPIDsEditorPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 92, 30);
			StaticResourceExtension staticResourceExtension7;
			VisualDiagnostics.RegisterSourceInfo(staticResourceExtension7 = new StaticResourceExtension(), new Uri("Settings\\CustomPIDsEditorPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 97, 33);
			BindingExtension bindingExtension12;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension12 = new BindingExtension(), new Uri("Settings\\CustomPIDsEditorPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 97, 33);
			Translate translate11;
			VisualDiagnostics.RegisterSourceInfo(translate11 = new Translate(), new Uri("Settings\\CustomPIDsEditorPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 98, 33);
			Label label7;
			VisualDiagnostics.RegisterSourceInfo(label7 = new Label(), new Uri("Settings\\CustomPIDsEditorPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 95, 30);
			BindingExtension bindingExtension13;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension13 = new BindingExtension(), new Uri("Settings\\CustomPIDsEditorPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 102, 33);
			Translate translate12;
			VisualDiagnostics.RegisterSourceInfo(translate12 = new Translate(), new Uri("Settings\\CustomPIDsEditorPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 103, 33);
			Label label8;
			VisualDiagnostics.RegisterSourceInfo(label8 = new Label(), new Uri("Settings\\CustomPIDsEditorPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 100, 30);
			StackLayout stackLayout2;
			VisualDiagnostics.RegisterSourceInfo(stackLayout2 = new StackLayout(), new Uri("Settings\\CustomPIDsEditorPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 91, 26);
			StaticResourceExtension staticResourceExtension8;
			VisualDiagnostics.RegisterSourceInfo(staticResourceExtension8 = new StaticResourceExtension(), new Uri("Settings\\CustomPIDsEditorPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 106, 32);
			BindingExtension bindingExtension14;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension14 = new BindingExtension(), new Uri("Settings\\CustomPIDsEditorPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 106, 32);
			BindingExtension bindingExtension15;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension15 = new BindingExtension(), new Uri("Settings\\CustomPIDsEditorPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 106, 135);
			Entry entry5;
			VisualDiagnostics.RegisterSourceInfo(entry5 = new Entry(), new Uri("Settings\\CustomPIDsEditorPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 106, 26);
			Translate translate13;
			VisualDiagnostics.RegisterSourceInfo(translate13 = new Translate(), new Uri("Settings\\CustomPIDsEditorPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 107, 73);
			Button button;
			VisualDiagnostics.RegisterSourceInfo(button = new Button(), new Uri("Settings\\CustomPIDsEditorPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 107, 26);
			StackLayout stackLayout3;
			VisualDiagnostics.RegisterSourceInfo(stackLayout3 = new StackLayout(), new Uri("Settings\\CustomPIDsEditorPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 87, 22);
			StaticResourceExtension staticResourceExtension9;
			VisualDiagnostics.RegisterSourceInfo(staticResourceExtension9 = new StaticResourceExtension(), new Uri("Settings\\CustomPIDsEditorPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 113, 49);
			BindingExtension bindingExtension16;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension16 = new BindingExtension(), new Uri("Settings\\CustomPIDsEditorPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 113, 49);
			ColumnDefinition columnDefinition;
			VisualDiagnostics.RegisterSourceInfo(columnDefinition = new ColumnDefinition(), new Uri("Settings\\CustomPIDsEditorPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 115, 30);
			ColumnDefinition columnDefinition2;
			VisualDiagnostics.RegisterSourceInfo(columnDefinition2 = new ColumnDefinition(), new Uri("Settings\\CustomPIDsEditorPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 116, 30);
			RowDefinition rowDefinition;
			VisualDiagnostics.RegisterSourceInfo(rowDefinition = new RowDefinition(), new Uri("Settings\\CustomPIDsEditorPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 119, 30);
			RowDefinition rowDefinition2;
			VisualDiagnostics.RegisterSourceInfo(rowDefinition2 = new RowDefinition(), new Uri("Settings\\CustomPIDsEditorPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 120, 30);
			RowDefinition rowDefinition3;
			VisualDiagnostics.RegisterSourceInfo(rowDefinition3 = new RowDefinition(), new Uri("Settings\\CustomPIDsEditorPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 121, 30);
			RowDefinition rowDefinition4;
			VisualDiagnostics.RegisterSourceInfo(rowDefinition4 = new RowDefinition(), new Uri("Settings\\CustomPIDsEditorPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 122, 30);
			RowDefinition rowDefinition5;
			VisualDiagnostics.RegisterSourceInfo(rowDefinition5 = new RowDefinition(), new Uri("Settings\\CustomPIDsEditorPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 123, 30);
			RowDefinition rowDefinition6;
			VisualDiagnostics.RegisterSourceInfo(rowDefinition6 = new RowDefinition(), new Uri("Settings\\CustomPIDsEditorPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 124, 30);
			RowDefinition rowDefinition7;
			VisualDiagnostics.RegisterSourceInfo(rowDefinition7 = new RowDefinition(), new Uri("Settings\\CustomPIDsEditorPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 125, 30);
			Translate translate14;
			VisualDiagnostics.RegisterSourceInfo(translate14 = new Translate(), new Uri("Settings\\CustomPIDsEditorPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 131, 29);
			Label label9;
			VisualDiagnostics.RegisterSourceInfo(label9 = new Label(), new Uri("Settings\\CustomPIDsEditorPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 128, 26);
			BindingExtension bindingExtension17;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension17 = new BindingExtension(), new Uri("Settings\\CustomPIDsEditorPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 140, 29);
			NumericEntryV3 numericEntryV;
			VisualDiagnostics.RegisterSourceInfo(numericEntryV = new NumericEntryV3(), new Uri("Settings\\CustomPIDsEditorPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 135, 26);
			Translate translate15;
			VisualDiagnostics.RegisterSourceInfo(translate15 = new Translate(), new Uri("Settings\\CustomPIDsEditorPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 146, 29);
			Label label10;
			VisualDiagnostics.RegisterSourceInfo(label10 = new Label(), new Uri("Settings\\CustomPIDsEditorPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 143, 26);
			BindingExtension bindingExtension18;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension18 = new BindingExtension(), new Uri("Settings\\CustomPIDsEditorPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 154, 29);
			NumericEntryV3 numericEntryV2;
			VisualDiagnostics.RegisterSourceInfo(numericEntryV2 = new NumericEntryV3(), new Uri("Settings\\CustomPIDsEditorPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 148, 26);
			BindingExtension bindingExtension19;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension19 = new BindingExtension(), new Uri("Settings\\CustomPIDsEditorPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 160, 29);
			LabelSwitch labelSwitch;
			VisualDiagnostics.RegisterSourceInfo(labelSwitch = new LabelSwitch(), new Uri("Settings\\CustomPIDsEditorPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 156, 26);
			BindingExtension bindingExtension20;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension20 = new BindingExtension(), new Uri("Settings\\CustomPIDsEditorPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 166, 29);
			LabelSwitch labelSwitch2;
			VisualDiagnostics.RegisterSourceInfo(labelSwitch2 = new LabelSwitch(), new Uri("Settings\\CustomPIDsEditorPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 162, 26);
			Translate translate16;
			VisualDiagnostics.RegisterSourceInfo(translate16 = new Translate(), new Uri("Settings\\CustomPIDsEditorPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 171, 29);
			Label label11;
			VisualDiagnostics.RegisterSourceInfo(label11 = new Label(), new Uri("Settings\\CustomPIDsEditorPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 168, 26);
			BindingExtension bindingExtension21;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension21 = new BindingExtension(), new Uri("Settings\\CustomPIDsEditorPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 176, 29);
			NumericEntryV3 numericEntryV3;
			VisualDiagnostics.RegisterSourceInfo(numericEntryV3 = new NumericEntryV3(), new Uri("Settings\\CustomPIDsEditorPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 173, 26);
			Translate translate17;
			VisualDiagnostics.RegisterSourceInfo(translate17 = new Translate(), new Uri("Settings\\CustomPIDsEditorPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 180, 29);
			Label label12;
			VisualDiagnostics.RegisterSourceInfo(label12 = new Label(), new Uri("Settings\\CustomPIDsEditorPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 177, 26);
			BindingExtension bindingExtension22;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension22 = new BindingExtension(), new Uri("Settings\\CustomPIDsEditorPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 185, 29);
			NumericEntryV3 numericEntryV4;
			VisualDiagnostics.RegisterSourceInfo(numericEntryV4 = new NumericEntryV3(), new Uri("Settings\\CustomPIDsEditorPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 182, 26);
			Translate translate18;
			VisualDiagnostics.RegisterSourceInfo(translate18 = new Translate(), new Uri("Settings\\CustomPIDsEditorPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 189, 29);
			Label label13;
			VisualDiagnostics.RegisterSourceInfo(label13 = new Label(), new Uri("Settings\\CustomPIDsEditorPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 186, 26);
			BindingExtension bindingExtension23;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension23 = new BindingExtension(), new Uri("Settings\\CustomPIDsEditorPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 194, 29);
			NumericEntryV3 numericEntryV5;
			VisualDiagnostics.RegisterSourceInfo(numericEntryV5 = new NumericEntryV3(), new Uri("Settings\\CustomPIDsEditorPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 191, 26);
			Grid grid;
			VisualDiagnostics.RegisterSourceInfo(grid = new Grid(), new Uri("Settings\\CustomPIDsEditorPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 113, 22);
			StaticResourceExtension staticResourceExtension10;
			VisualDiagnostics.RegisterSourceInfo(staticResourceExtension10 = new StaticResourceExtension(), new Uri("Settings\\CustomPIDsEditorPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 197, 50);
			BindingExtension bindingExtension24;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension24 = new BindingExtension(), new Uri("Settings\\CustomPIDsEditorPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 197, 50);
			ColumnDefinition columnDefinition3;
			VisualDiagnostics.RegisterSourceInfo(columnDefinition3 = new ColumnDefinition(), new Uri("Settings\\CustomPIDsEditorPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 199, 30);
			ColumnDefinition columnDefinition4;
			VisualDiagnostics.RegisterSourceInfo(columnDefinition4 = new ColumnDefinition(), new Uri("Settings\\CustomPIDsEditorPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 200, 30);
			RowDefinition rowDefinition8;
			VisualDiagnostics.RegisterSourceInfo(rowDefinition8 = new RowDefinition(), new Uri("Settings\\CustomPIDsEditorPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 203, 30);
			RowDefinition rowDefinition9;
			VisualDiagnostics.RegisterSourceInfo(rowDefinition9 = new RowDefinition(), new Uri("Settings\\CustomPIDsEditorPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 204, 30);
			RowDefinition rowDefinition10;
			VisualDiagnostics.RegisterSourceInfo(rowDefinition10 = new RowDefinition(), new Uri("Settings\\CustomPIDsEditorPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 205, 30);
			RowDefinition rowDefinition11;
			VisualDiagnostics.RegisterSourceInfo(rowDefinition11 = new RowDefinition(), new Uri("Settings\\CustomPIDsEditorPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 206, 30);
			RowDefinition rowDefinition12;
			VisualDiagnostics.RegisterSourceInfo(rowDefinition12 = new RowDefinition(), new Uri("Settings\\CustomPIDsEditorPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 207, 30);
			RowDefinition rowDefinition13;
			VisualDiagnostics.RegisterSourceInfo(rowDefinition13 = new RowDefinition(), new Uri("Settings\\CustomPIDsEditorPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 208, 30);
			RowDefinition rowDefinition14;
			VisualDiagnostics.RegisterSourceInfo(rowDefinition14 = new RowDefinition(), new Uri("Settings\\CustomPIDsEditorPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 209, 30);
			Translate translate19;
			VisualDiagnostics.RegisterSourceInfo(translate19 = new Translate(), new Uri("Settings\\CustomPIDsEditorPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 215, 29);
			Label label14;
			VisualDiagnostics.RegisterSourceInfo(label14 = new Label(), new Uri("Settings\\CustomPIDsEditorPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 212, 26);
			BindingExtension bindingExtension25;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension25 = new BindingExtension(), new Uri("Settings\\CustomPIDsEditorPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 221, 29);
			NumericEntryV3 numericEntryV6;
			VisualDiagnostics.RegisterSourceInfo(numericEntryV6 = new NumericEntryV3(), new Uri("Settings\\CustomPIDsEditorPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 216, 26);
			Translate translate20;
			VisualDiagnostics.RegisterSourceInfo(translate20 = new Translate(), new Uri("Settings\\CustomPIDsEditorPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 225, 29);
			Label label15;
			VisualDiagnostics.RegisterSourceInfo(label15 = new Label(), new Uri("Settings\\CustomPIDsEditorPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 222, 26);
			BindingExtension bindingExtension26;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension26 = new BindingExtension(), new Uri("Settings\\CustomPIDsEditorPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 232, 29);
			NumericEntryV3 numericEntryV7;
			VisualDiagnostics.RegisterSourceInfo(numericEntryV7 = new NumericEntryV3(), new Uri("Settings\\CustomPIDsEditorPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 226, 26);
			Grid grid2;
			VisualDiagnostics.RegisterSourceInfo(grid2 = new Grid(), new Uri("Settings\\CustomPIDsEditorPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 197, 22);
			StackLayout stackLayout4;
			VisualDiagnostics.RegisterSourceInfo(stackLayout4 = new StackLayout(), new Uri("Settings\\CustomPIDsEditorPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 54, 18);
			StaticResourceExtension staticResourceExtension11;
			VisualDiagnostics.RegisterSourceInfo(staticResourceExtension11 = new StaticResourceExtension(), new Uri("Settings\\CustomPIDsEditorPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 239, 21);
			BindingExtension bindingExtension27;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension27 = new BindingExtension(), new Uri("Settings\\CustomPIDsEditorPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 239, 21);
			Label label16;
			VisualDiagnostics.RegisterSourceInfo(label16 = new Label(), new Uri("Settings\\CustomPIDsEditorPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 241, 22);
			BindingExtension bindingExtension28;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension28 = new BindingExtension(), new Uri("Settings\\CustomPIDsEditorPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 249, 25);
			Entry entry6;
			VisualDiagnostics.RegisterSourceInfo(entry6 = new Entry(), new Uri("Settings\\CustomPIDsEditorPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 245, 22);
			Label label17;
			VisualDiagnostics.RegisterSourceInfo(label17 = new Label(), new Uri("Settings\\CustomPIDsEditorPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 251, 22);
			BindingExtension bindingExtension29;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension29 = new BindingExtension(), new Uri("Settings\\CustomPIDsEditorPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 262, 25);
			NumericEntryV3 numericEntryV8;
			VisualDiagnostics.RegisterSourceInfo(numericEntryV8 = new NumericEntryV3(), new Uri("Settings\\CustomPIDsEditorPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 255, 22);
			Label label18;
			VisualDiagnostics.RegisterSourceInfo(label18 = new Label(), new Uri("Settings\\CustomPIDsEditorPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 264, 22);
			BindingExtension bindingExtension30;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension30 = new BindingExtension(), new Uri("Settings\\CustomPIDsEditorPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 275, 25);
			NumericEntryV3 numericEntryV9;
			VisualDiagnostics.RegisterSourceInfo(numericEntryV9 = new NumericEntryV3(), new Uri("Settings\\CustomPIDsEditorPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 268, 22);
			Translate translate21;
			VisualDiagnostics.RegisterSourceInfo(translate21 = new Translate(), new Uri("Settings\\CustomPIDsEditorPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 280, 25);
			Button button2;
			VisualDiagnostics.RegisterSourceInfo(button2 = new Button(), new Uri("Settings\\CustomPIDsEditorPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 277, 22);
			StackLayout stackLayout5;
			VisualDiagnostics.RegisterSourceInfo(stackLayout5 = new StackLayout(), new Uri("Settings\\CustomPIDsEditorPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 237, 18);
			StaticResourceExtension staticResourceExtension12;
			VisualDiagnostics.RegisterSourceInfo(staticResourceExtension12 = new StaticResourceExtension(), new Uri("Settings\\CustomPIDsEditorPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 283, 30);
			BindingExtension bindingExtension31;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension31 = new BindingExtension(), new Uri("Settings\\CustomPIDsEditorPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 283, 30);
			Translate translate22;
			VisualDiagnostics.RegisterSourceInfo(translate22 = new Translate(), new Uri("Settings\\CustomPIDsEditorPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 284, 28);
			Label label19;
			VisualDiagnostics.RegisterSourceInfo(label19 = new Label(), new Uri("Settings\\CustomPIDsEditorPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 284, 22);
			BindingExtension bindingExtension32;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension32 = new BindingExtension(), new Uri("Settings\\CustomPIDsEditorPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 285, 40);
			NumericEntryV3 numericEntryV10;
			VisualDiagnostics.RegisterSourceInfo(numericEntryV10 = new NumericEntryV3(), new Uri("Settings\\CustomPIDsEditorPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 285, 22);
			Translate translate23;
			VisualDiagnostics.RegisterSourceInfo(translate23 = new Translate(), new Uri("Settings\\CustomPIDsEditorPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 286, 28);
			Label label20;
			VisualDiagnostics.RegisterSourceInfo(label20 = new Label(), new Uri("Settings\\CustomPIDsEditorPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 286, 22);
			BindingExtension bindingExtension33;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension33 = new BindingExtension(), new Uri("Settings\\CustomPIDsEditorPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 287, 40);
			NumericEntryV3 numericEntryV11;
			VisualDiagnostics.RegisterSourceInfo(numericEntryV11 = new NumericEntryV3(), new Uri("Settings\\CustomPIDsEditorPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 287, 22);
			Translate translate24;
			VisualDiagnostics.RegisterSourceInfo(translate24 = new Translate(), new Uri("Settings\\CustomPIDsEditorPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 289, 28);
			Label label21;
			VisualDiagnostics.RegisterSourceInfo(label21 = new Label(), new Uri("Settings\\CustomPIDsEditorPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 289, 22);
			List<string> units;
			VisualDiagnostics.RegisterSourceInfo(units = StaticLists.Units, new Uri("Settings\\CustomPIDsEditorPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 290, 29);
			BindingExtension bindingExtension34;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension34 = new BindingExtension(), new Uri("Settings\\CustomPIDsEditorPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 290, 29);
			StaticResourceExtension staticResourceExtension13;
			VisualDiagnostics.RegisterSourceInfo(staticResourceExtension13 = new StaticResourceExtension(), new Uri("Settings\\CustomPIDsEditorPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 290, 95);
			BindingExtension bindingExtension35;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension35 = new BindingExtension(), new Uri("Settings\\CustomPIDsEditorPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 290, 95);
			Picker picker;
			VisualDiagnostics.RegisterSourceInfo(picker = new Picker(), new Uri("Settings\\CustomPIDsEditorPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 290, 22);
			Translate translate25;
			VisualDiagnostics.RegisterSourceInfo(translate25 = new Translate(), new Uri("Settings\\CustomPIDsEditorPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 315, 28);
			Label label22;
			VisualDiagnostics.RegisterSourceInfo(label22 = new Label(), new Uri("Settings\\CustomPIDsEditorPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 315, 22);
			StaticResourceExtension staticResourceExtension14;
			VisualDiagnostics.RegisterSourceInfo(staticResourceExtension14 = new StaticResourceExtension(), new Uri("Settings\\CustomPIDsEditorPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 316, 29);
			BindingExtension bindingExtension36;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension36 = new BindingExtension(), new Uri("Settings\\CustomPIDsEditorPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 316, 29);
			Type typeFromHandle;
			VisualDiagnostics.RegisterSourceInfo(typeFromHandle = typeof(string), new Uri("Settings\\CustomPIDsEditorPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 318, 38);
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
			}, new Uri("Settings\\CustomPIDsEditorPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 318, 30);
			Picker picker2;
			VisualDiagnostics.RegisterSourceInfo(picker2 = new Picker(), new Uri("Settings\\CustomPIDsEditorPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 316, 22);
			Translate translate26;
			VisualDiagnostics.RegisterSourceInfo(translate26 = new Translate(), new Uri("Settings\\CustomPIDsEditorPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 349, 28);
			Label label23;
			VisualDiagnostics.RegisterSourceInfo(label23 = new Label(), new Uri("Settings\\CustomPIDsEditorPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 349, 22);
			StaticResourceExtension staticResourceExtension15;
			VisualDiagnostics.RegisterSourceInfo(staticResourceExtension15 = new StaticResourceExtension(), new Uri("Settings\\CustomPIDsEditorPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 350, 49);
			BindingExtension bindingExtension37;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension37 = new BindingExtension(), new Uri("Settings\\CustomPIDsEditorPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 350, 49);
			Picker picker3;
			VisualDiagnostics.RegisterSourceInfo(picker3 = new Picker(), new Uri("Settings\\CustomPIDsEditorPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 350, 22);
			DynamicResourceExtension dynamicResourceExtension2;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension2 = new DynamicResourceExtension(), new Uri("Settings\\CustomPIDsEditorPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 353, 25);
			Translate translate27;
			VisualDiagnostics.RegisterSourceInfo(translate27 = new Translate(), new Uri("Settings\\CustomPIDsEditorPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 354, 25);
			DynamicResourceExtension dynamicResourceExtension3;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension3 = new DynamicResourceExtension(), new Uri("Settings\\CustomPIDsEditorPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 355, 25);
			Label label24;
			VisualDiagnostics.RegisterSourceInfo(label24 = new Label(), new Uri("Settings\\CustomPIDsEditorPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 351, 22);
			StackLayout stackLayout6;
			VisualDiagnostics.RegisterSourceInfo(stackLayout6 = new StackLayout(), new Uri("Settings\\CustomPIDsEditorPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 283, 18);
			Translate translate28;
			VisualDiagnostics.RegisterSourceInfo(translate28 = new Translate(), new Uri("Settings\\CustomPIDsEditorPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 358, 24);
			Label label25;
			VisualDiagnostics.RegisterSourceInfo(label25 = new Label(), new Uri("Settings\\CustomPIDsEditorPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 358, 18);
			BindingExtension bindingExtension38;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension38 = new BindingExtension(), new Uri("Settings\\CustomPIDsEditorPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 359, 24);
			Entry entry7;
			VisualDiagnostics.RegisterSourceInfo(entry7 = new Entry(), new Uri("Settings\\CustomPIDsEditorPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 359, 18);
			Translate translate29;
			VisualDiagnostics.RegisterSourceInfo(translate29 = new Translate(), new Uri("Settings\\CustomPIDsEditorPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 360, 24);
			Label label26;
			VisualDiagnostics.RegisterSourceInfo(label26 = new Label(), new Uri("Settings\\CustomPIDsEditorPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 360, 18);
			BindingExtension bindingExtension39;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension39 = new BindingExtension(), new Uri("Settings\\CustomPIDsEditorPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 361, 24);
			Entry entry8;
			VisualDiagnostics.RegisterSourceInfo(entry8 = new Entry(), new Uri("Settings\\CustomPIDsEditorPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 361, 18);
			Label label27;
			VisualDiagnostics.RegisterSourceInfo(label27 = new Label(), new Uri("Settings\\CustomPIDsEditorPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 363, 18);
			BindingExtension bindingExtension40;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension40 = new BindingExtension(), new Uri("Settings\\CustomPIDsEditorPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 368, 21);
			Editor editor;
			VisualDiagnostics.RegisterSourceInfo(editor = new Editor(), new Uri("Settings\\CustomPIDsEditorPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 364, 18);
			Button button3;
			VisualDiagnostics.RegisterSourceInfo(button3 = new Button(), new Uri("Settings\\CustomPIDsEditorPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 371, 18);
			Button button4;
			VisualDiagnostics.RegisterSourceInfo(button4 = new Button(), new Uri("Settings\\CustomPIDsEditorPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 378, 22);
			Label label28;
			VisualDiagnostics.RegisterSourceInfo(label28 = new Label(), new Uri("Settings\\CustomPIDsEditorPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 382, 22);
			StackLayout stackLayout7;
			VisualDiagnostics.RegisterSourceInfo(stackLayout7 = new StackLayout(), new Uri("Settings\\CustomPIDsEditorPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 377, 18);
			StackLayout stackLayout8;
			VisualDiagnostics.RegisterSourceInfo(stackLayout8 = new StackLayout(), new Uri("Settings\\CustomPIDsEditorPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 36, 14);
			ScrollView scrollView;
			VisualDiagnostics.RegisterSourceInfo(scrollView = new ScrollView(), new Uri("Settings\\CustomPIDsEditorPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 35, 10);
			VisualDiagnostics.RegisterSourceInfo(this, new Uri("Settings\\CustomPIDsEditorPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 2, 2);
			NameScope nameScope = NameScope.GetNameScope(this) ?? new NameScope();
			NameScope.SetNameScope(this, nameScope);
			nameScope.RegisterName("scrollView", scrollView);
			if (scrollView.StyleId == null)
			{
				scrollView.StyleId = "scrollView";
			}
			nameScope.RegisterName("panelDecodeType", stackLayout);
			if (stackLayout.StyleId == null)
			{
				stackLayout.StyleId = "panelDecodeType";
			}
			nameScope.RegisterName("panelFormula", stackLayout3);
			if (stackLayout3.StyleId == null)
			{
				stackLayout3.StyleId = "panelFormula";
			}
			nameScope.RegisterName("panelByteSet", grid);
			if (grid.StyleId == null)
			{
				grid.StyleId = "panelByteSet";
			}
			nameScope.RegisterName("panelBitValue", grid2);
			if (grid2.StyleId == null)
			{
				grid2.StyleId = "panelBitValue";
			}
			nameScope.RegisterName("panelVagGroup", stackLayout5);
			if (stackLayout5.StyleId == null)
			{
				stackLayout5.StyleId = "panelVagGroup";
			}
			nameScope.RegisterName("vagUnitEntry", entry6);
			if (entry6.StyleId == null)
			{
				entry6.StyleId = "vagUnitEntry";
			}
			nameScope.RegisterName("vagGroupEntry", numericEntryV8);
			if (numericEntryV8.StyleId == null)
			{
				numericEntryV8.StyleId = "vagGroupEntry";
			}
			nameScope.RegisterName("vagItemInGroupEntry", numericEntryV9);
			if (numericEntryV9.StyleId == null)
			{
				numericEntryV9.StyleId = "vagItemInGroupEntry";
			}
			nameScope.RegisterName("btnApplyVagGroup", button2);
			if (button2.StyleId == null)
			{
				button2.StyleId = "btnApplyVagGroup";
			}
			nameScope.RegisterName("pickerRole", picker3);
			if (picker3.StyleId == null)
			{
				picker3.StyleId = "pickerRole";
			}
			nameScope.RegisterName("btnUnitsOverride", button3);
			if (button3.StyleId == null)
			{
				button3.StyleId = "btnUnitsOverride";
			}
			nameScope.RegisterName("panelTest", stackLayout7);
			if (stackLayout7.StyleId == null)
			{
				stackLayout7.StyleId = "panelTest";
			}
			nameScope.RegisterName("btnTestPid", button4);
			if (button4.StyleId == null)
			{
				button4.StyleId = "btnTestPid";
			}
			nameScope.RegisterName("lbTestResult", label28);
			if (label28.StyleId == null)
			{
				label28.StyleId = "lbTestResult";
			}
			this.scrollView = scrollView;
			this.panelDecodeType = stackLayout;
			this.panelFormula = stackLayout3;
			this.panelByteSet = grid;
			this.panelBitValue = grid2;
			this.panelVagGroup = stackLayout5;
			this.vagUnitEntry = entry6;
			this.vagGroupEntry = numericEntryV8;
			this.vagItemInGroupEntry = numericEntryV9;
			this.btnApplyVagGroup = button2;
			this.pickerRole = picker3;
			this.btnUnitsOverride = button3;
			this.panelTest = stackLayout7;
			this.btnTestPid = button4;
			this.lbTestResult = label28;
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
			this.SetValue(Page.PaddingProperty, new Thickness(0.0));
			this.SetValue(Page.UseSafeAreaProperty, true);
			this.Appearing += this.Handle_Appearing;
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
			xmlNamespaceResolver.Add("syncfusion", "clr-namespace:Syncfusion.XForms.Buttons;assembly=Syncfusion.Buttons.XForms");
			xmlNamespaceResolver.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider.Add(typeFromHandle3, new XamlTypeResolver(xmlNamespaceResolver, typeof(CustomPIDsEditorPage).GetTypeInfo().Assembly));
			xamlServiceProvider.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(16, 5)));
			DynamicResource dynamicResource = markupExtension.ProvideValue(xamlServiceProvider);
			this.SetDynamicResource(VisualElement.BackgroundColorProperty, dynamicResource.Key);
			this.Disappearing += this.Handle_Disappearing;
			this.SizeChanged += this.Handle_SizeChanged;
			this.Resources = resourceDictionary;
			scrollView.SetValue(ScrollView.OrientationProperty, 0);
			stackLayout8.SetValue(StackLayout.OrientationProperty, 0);
			onPlatform.Android = new Thickness(5.0, 5.0, 5.0, 5.0);
			onPlatform.WinPhone = new Thickness(0.0);
			onPlatform.iOS = new Thickness(5.0, 0.0, 5.0, 0.0);
			stackLayout8.SetValue(View.MarginProperty, onPlatform);
			translate.Text = "CustomPidEditor_tbName.Text";
			IMarkupExtension markupExtension2 = translate;
			XamlServiceProvider xamlServiceProvider2 = new XamlServiceProvider();
			Type typeFromHandle4 = typeof(IProvideValueTarget);
			object[] array3 = new object[0 + 4];
			array3[0] = label;
			array3[1] = stackLayout8;
			array3[2] = scrollView;
			array3[3] = this;
			object obj2;
			xamlServiceProvider2.Add(typeFromHandle4, obj2 = new SimpleValueTargetProvider(array3, Label.TextProperty, nameScope));
			xamlServiceProvider2.Add(typeof(IReferenceProvider), obj2);
			Type typeFromHandle5 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver2 = new XmlNamespaceResolver();
			xmlNamespaceResolver2.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver2.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver2.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver2.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver2.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver2.Add("pids", "clr-namespace:CarScannerXamarinForms.OBD2.PIDS");
			xmlNamespaceResolver2.Add("syncfusion", "clr-namespace:Syncfusion.XForms.Buttons;assembly=Syncfusion.Buttons.XForms");
			xmlNamespaceResolver2.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider2.Add(typeFromHandle5, new XamlTypeResolver(xmlNamespaceResolver2, typeof(CustomPIDsEditorPage).GetTypeInfo().Assembly));
			xamlServiceProvider2.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(45, 24)));
			object obj3 = markupExtension2.ProvideValue(xamlServiceProvider2);
			label.Text = obj3;
			stackLayout8.Children.Add(label);
			bindingExtension.Mode = 1;
			bindingExtension.Path = "Name";
			BindingBase bindingBase = bindingExtension.ProvideValue(null);
			entry.SetBinding(Entry.TextProperty, bindingBase);
			stackLayout8.Children.Add(entry);
			translate2.Text = "CustomPidEditor_tbShortName.Text";
			IMarkupExtension markupExtension3 = translate2;
			XamlServiceProvider xamlServiceProvider3 = new XamlServiceProvider();
			Type typeFromHandle6 = typeof(IProvideValueTarget);
			object[] array4 = new object[0 + 4];
			array4[0] = label2;
			array4[1] = stackLayout8;
			array4[2] = scrollView;
			array4[3] = this;
			object obj4;
			xamlServiceProvider3.Add(typeFromHandle6, obj4 = new SimpleValueTargetProvider(array4, Label.TextProperty, nameScope));
			xamlServiceProvider3.Add(typeof(IReferenceProvider), obj4);
			Type typeFromHandle7 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver3 = new XmlNamespaceResolver();
			xmlNamespaceResolver3.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver3.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver3.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver3.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver3.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver3.Add("pids", "clr-namespace:CarScannerXamarinForms.OBD2.PIDS");
			xmlNamespaceResolver3.Add("syncfusion", "clr-namespace:Syncfusion.XForms.Buttons;assembly=Syncfusion.Buttons.XForms");
			xmlNamespaceResolver3.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider3.Add(typeFromHandle7, new XamlTypeResolver(xmlNamespaceResolver3, typeof(CustomPIDsEditorPage).GetTypeInfo().Assembly));
			xamlServiceProvider3.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(47, 60)));
			object obj5 = markupExtension3.ProvideValue(xamlServiceProvider3);
			label2.Text = obj5;
			stackLayout8.Children.Add(label2);
			bindingExtension2.Mode = 1;
			bindingExtension2.Path = "ShortName";
			BindingBase bindingBase2 = bindingExtension2.ProvideValue(null);
			entry2.SetBinding(Entry.TextProperty, bindingBase2);
			stackLayout8.Children.Add(entry2);
			bindingExtension3.Mode = 2;
			staticResourceExtension.Key = "BoolToNegativeConverter";
			IMarkupExtension markupExtension4 = staticResourceExtension;
			XamlServiceProvider xamlServiceProvider4 = new XamlServiceProvider();
			Type typeFromHandle8 = typeof(IProvideValueTarget);
			object[] array5 = new object[0 + 5];
			array5[0] = bindingExtension3;
			array5[1] = label3;
			array5[2] = stackLayout8;
			array5[3] = scrollView;
			array5[4] = this;
			object obj6;
			xamlServiceProvider4.Add(typeFromHandle8, obj6 = new SimpleValueTargetProvider(array5, typeof(BindingExtension).GetRuntimeProperty("Converter"), nameScope));
			xamlServiceProvider4.Add(typeof(IReferenceProvider), obj6);
			Type typeFromHandle9 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver4 = new XmlNamespaceResolver();
			xmlNamespaceResolver4.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver4.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver4.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver4.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver4.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver4.Add("pids", "clr-namespace:CarScannerXamarinForms.OBD2.PIDS");
			xmlNamespaceResolver4.Add("syncfusion", "clr-namespace:Syncfusion.XForms.Buttons;assembly=Syncfusion.Buttons.XForms");
			xmlNamespaceResolver4.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider4.Add(typeFromHandle9, new XamlTypeResolver(xmlNamespaceResolver4, typeof(CustomPIDsEditorPage).GetTypeInfo().Assembly));
			xamlServiceProvider4.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(49, 24)));
			object obj7 = markupExtension4.ProvideValue(xamlServiceProvider4);
			bindingExtension3.Converter = obj7;
			bindingExtension3.Path = "IsFormulaHidden";
			BindingBase bindingBase3 = bindingExtension3.ProvideValue(null);
			label3.SetBinding(VisualElement.IsVisibleProperty, bindingBase3);
			translate3.Text = "CustomPidEditor_tbCommand.Text";
			IMarkupExtension markupExtension5 = translate3;
			XamlServiceProvider xamlServiceProvider5 = new XamlServiceProvider();
			Type typeFromHandle10 = typeof(IProvideValueTarget);
			object[] array6 = new object[0 + 4];
			array6[0] = label3;
			array6[1] = stackLayout8;
			array6[2] = scrollView;
			array6[3] = this;
			object obj8;
			xamlServiceProvider5.Add(typeFromHandle10, obj8 = new SimpleValueTargetProvider(array6, Label.TextProperty, nameScope));
			xamlServiceProvider5.Add(typeof(IReferenceProvider), obj8);
			Type typeFromHandle11 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver5 = new XmlNamespaceResolver();
			xmlNamespaceResolver5.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver5.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver5.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver5.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver5.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver5.Add("pids", "clr-namespace:CarScannerXamarinForms.OBD2.PIDS");
			xmlNamespaceResolver5.Add("syncfusion", "clr-namespace:Syncfusion.XForms.Buttons;assembly=Syncfusion.Buttons.XForms");
			xmlNamespaceResolver5.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider5.Add(typeFromHandle11, new XamlTypeResolver(xmlNamespaceResolver5, typeof(CustomPIDsEditorPage).GetTypeInfo().Assembly));
			xamlServiceProvider5.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(49, 127)));
			object obj9 = markupExtension5.ProvideValue(xamlServiceProvider5);
			label3.Text = obj9;
			stackLayout8.Children.Add(label3);
			bindingExtension4.Mode = 2;
			staticResourceExtension2.Key = "BoolToNegativeConverter";
			IMarkupExtension markupExtension6 = staticResourceExtension2;
			XamlServiceProvider xamlServiceProvider6 = new XamlServiceProvider();
			Type typeFromHandle12 = typeof(IProvideValueTarget);
			object[] array7 = new object[0 + 5];
			array7[0] = bindingExtension4;
			array7[1] = entry3;
			array7[2] = stackLayout8;
			array7[3] = scrollView;
			array7[4] = this;
			object obj10;
			xamlServiceProvider6.Add(typeFromHandle12, obj10 = new SimpleValueTargetProvider(array7, typeof(BindingExtension).GetRuntimeProperty("Converter"), nameScope));
			xamlServiceProvider6.Add(typeof(IReferenceProvider), obj10);
			Type typeFromHandle13 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver6 = new XmlNamespaceResolver();
			xmlNamespaceResolver6.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver6.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver6.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver6.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver6.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver6.Add("pids", "clr-namespace:CarScannerXamarinForms.OBD2.PIDS");
			xmlNamespaceResolver6.Add("syncfusion", "clr-namespace:Syncfusion.XForms.Buttons;assembly=Syncfusion.Buttons.XForms");
			xmlNamespaceResolver6.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider6.Add(typeFromHandle13, new XamlTypeResolver(xmlNamespaceResolver6, typeof(CustomPIDsEditorPage).GetTypeInfo().Assembly));
			xamlServiceProvider6.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(50, 24)));
			object obj11 = markupExtension6.ProvideValue(xamlServiceProvider6);
			bindingExtension4.Converter = obj11;
			bindingExtension4.Path = "IsFormulaHidden";
			BindingBase bindingBase4 = bindingExtension4.ProvideValue(null);
			entry3.SetBinding(VisualElement.IsVisibleProperty, bindingBase4);
			bindingExtension5.Mode = 1;
			bindingExtension5.Path = "Command";
			BindingBase bindingBase5 = bindingExtension5.ProvideValue(null);
			entry3.SetBinding(Entry.TextProperty, bindingBase5);
			stackLayout8.Children.Add(entry3);
			bindingExtension6.Mode = 2;
			staticResourceExtension3.Key = "BoolToNegativeConverter";
			IMarkupExtension markupExtension7 = staticResourceExtension3;
			XamlServiceProvider xamlServiceProvider7 = new XamlServiceProvider();
			Type typeFromHandle14 = typeof(IProvideValueTarget);
			object[] array8 = new object[0 + 5];
			array8[0] = bindingExtension6;
			array8[1] = label4;
			array8[2] = stackLayout8;
			array8[3] = scrollView;
			array8[4] = this;
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
			xmlNamespaceResolver7.Add("syncfusion", "clr-namespace:Syncfusion.XForms.Buttons;assembly=Syncfusion.Buttons.XForms");
			xmlNamespaceResolver7.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider7.Add(typeFromHandle15, new XamlTypeResolver(xmlNamespaceResolver7, typeof(CustomPIDsEditorPage).GetTypeInfo().Assembly));
			xamlServiceProvider7.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(51, 24)));
			object obj13 = markupExtension7.ProvideValue(xamlServiceProvider7);
			bindingExtension6.Converter = obj13;
			bindingExtension6.Path = "IsFormulaHidden";
			BindingBase bindingBase6 = bindingExtension6.ProvideValue(null);
			label4.SetBinding(VisualElement.IsVisibleProperty, bindingBase6);
			translate4.Text = "CustomPidEditor_tbHeader.Text";
			IMarkupExtension markupExtension8 = translate4;
			XamlServiceProvider xamlServiceProvider8 = new XamlServiceProvider();
			Type typeFromHandle16 = typeof(IProvideValueTarget);
			object[] array9 = new object[0 + 4];
			array9[0] = label4;
			array9[1] = stackLayout8;
			array9[2] = scrollView;
			array9[3] = this;
			object obj14;
			xamlServiceProvider8.Add(typeFromHandle16, obj14 = new SimpleValueTargetProvider(array9, Label.TextProperty, nameScope));
			xamlServiceProvider8.Add(typeof(IReferenceProvider), obj14);
			Type typeFromHandle17 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver8 = new XmlNamespaceResolver();
			xmlNamespaceResolver8.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver8.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver8.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver8.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver8.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver8.Add("pids", "clr-namespace:CarScannerXamarinForms.OBD2.PIDS");
			xmlNamespaceResolver8.Add("syncfusion", "clr-namespace:Syncfusion.XForms.Buttons;assembly=Syncfusion.Buttons.XForms");
			xmlNamespaceResolver8.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider8.Add(typeFromHandle17, new XamlTypeResolver(xmlNamespaceResolver8, typeof(CustomPIDsEditorPage).GetTypeInfo().Assembly));
			xamlServiceProvider8.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(51, 127)));
			object obj15 = markupExtension8.ProvideValue(xamlServiceProvider8);
			label4.Text = obj15;
			stackLayout8.Children.Add(label4);
			bindingExtension7.Mode = 2;
			staticResourceExtension4.Key = "BoolToNegativeConverter";
			IMarkupExtension markupExtension9 = staticResourceExtension4;
			XamlServiceProvider xamlServiceProvider9 = new XamlServiceProvider();
			Type typeFromHandle18 = typeof(IProvideValueTarget);
			object[] array10 = new object[0 + 5];
			array10[0] = bindingExtension7;
			array10[1] = entry4;
			array10[2] = stackLayout8;
			array10[3] = scrollView;
			array10[4] = this;
			object obj16;
			xamlServiceProvider9.Add(typeFromHandle18, obj16 = new SimpleValueTargetProvider(array10, typeof(BindingExtension).GetRuntimeProperty("Converter"), nameScope));
			xamlServiceProvider9.Add(typeof(IReferenceProvider), obj16);
			Type typeFromHandle19 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver9 = new XmlNamespaceResolver();
			xmlNamespaceResolver9.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver9.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver9.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver9.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver9.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver9.Add("pids", "clr-namespace:CarScannerXamarinForms.OBD2.PIDS");
			xmlNamespaceResolver9.Add("syncfusion", "clr-namespace:Syncfusion.XForms.Buttons;assembly=Syncfusion.Buttons.XForms");
			xmlNamespaceResolver9.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider9.Add(typeFromHandle19, new XamlTypeResolver(xmlNamespaceResolver9, typeof(CustomPIDsEditorPage).GetTypeInfo().Assembly));
			xamlServiceProvider9.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(52, 24)));
			object obj17 = markupExtension9.ProvideValue(xamlServiceProvider9);
			bindingExtension7.Converter = obj17;
			bindingExtension7.Path = "IsFormulaHidden";
			BindingBase bindingBase7 = bindingExtension7.ProvideValue(null);
			entry4.SetBinding(VisualElement.IsVisibleProperty, bindingBase7);
			bindingExtension8.Mode = 1;
			bindingExtension8.Path = "Header";
			BindingBase bindingBase8 = bindingExtension8.ProvideValue(null);
			entry4.SetBinding(Entry.TextProperty, bindingBase8);
			stackLayout8.Children.Add(entry4);
			bindingExtension9.Mode = 2;
			staticResourceExtension5.Key = "BoolToNegativeConverter";
			IMarkupExtension markupExtension10 = staticResourceExtension5;
			XamlServiceProvider xamlServiceProvider10 = new XamlServiceProvider();
			Type typeFromHandle20 = typeof(IProvideValueTarget);
			object[] array11 = new object[0 + 5];
			array11[0] = bindingExtension9;
			array11[1] = stackLayout4;
			array11[2] = stackLayout8;
			array11[3] = scrollView;
			array11[4] = this;
			object obj18;
			xamlServiceProvider10.Add(typeFromHandle20, obj18 = new SimpleValueTargetProvider(array11, typeof(BindingExtension).GetRuntimeProperty("Converter"), nameScope));
			xamlServiceProvider10.Add(typeof(IReferenceProvider), obj18);
			Type typeFromHandle21 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver10 = new XmlNamespaceResolver();
			xmlNamespaceResolver10.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver10.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver10.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver10.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver10.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver10.Add("pids", "clr-namespace:CarScannerXamarinForms.OBD2.PIDS");
			xmlNamespaceResolver10.Add("syncfusion", "clr-namespace:Syncfusion.XForms.Buttons;assembly=Syncfusion.Buttons.XForms");
			xmlNamespaceResolver10.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider10.Add(typeFromHandle21, new XamlTypeResolver(xmlNamespaceResolver10, typeof(CustomPIDsEditorPage).GetTypeInfo().Assembly));
			xamlServiceProvider10.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(54, 30)));
			object obj19 = markupExtension10.ProvideValue(xamlServiceProvider10);
			bindingExtension9.Converter = obj19;
			bindingExtension9.Path = "IsFormulaHidden";
			BindingBase bindingBase9 = bindingExtension9.ProvideValue(null);
			stackLayout4.SetBinding(VisualElement.IsVisibleProperty, bindingBase9);
			stackLayout4.SetValue(StackLayout.OrientationProperty, 0);
			translate5.Text = "ios_CustomPIDEditor_DecodeType";
			IMarkupExtension markupExtension11 = translate5;
			XamlServiceProvider xamlServiceProvider11 = new XamlServiceProvider();
			Type typeFromHandle22 = typeof(IProvideValueTarget);
			object[] array12 = new object[0 + 5];
			array12[0] = label5;
			array12[1] = stackLayout4;
			array12[2] = stackLayout8;
			array12[3] = scrollView;
			array12[4] = this;
			object obj20;
			xamlServiceProvider11.Add(typeFromHandle22, obj20 = new SimpleValueTargetProvider(array12, Label.TextProperty, nameScope));
			xamlServiceProvider11.Add(typeof(IReferenceProvider), obj20);
			Type typeFromHandle23 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver11 = new XmlNamespaceResolver();
			xmlNamespaceResolver11.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver11.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver11.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver11.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver11.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver11.Add("pids", "clr-namespace:CarScannerXamarinForms.OBD2.PIDS");
			xmlNamespaceResolver11.Add("syncfusion", "clr-namespace:Syncfusion.XForms.Buttons;assembly=Syncfusion.Buttons.XForms");
			xmlNamespaceResolver11.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider11.Add(typeFromHandle23, new XamlTypeResolver(xmlNamespaceResolver11, typeof(CustomPIDsEditorPage).GetTypeInfo().Assembly));
			xamlServiceProvider11.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(57, 28)));
			object obj21 = markupExtension11.ProvideValue(xamlServiceProvider11);
			label5.Text = obj21;
			stackLayout4.Children.Add(label5);
			stackLayout.SetValue(StackLayout.OrientationProperty, 0);
			stackLayout.SetValue(RadioButtonGroup.GroupNameProperty, "decodeType");
			bindingExtension10.Path = "Type";
			BindingBase bindingBase10 = bindingExtension10.ProvideValue(null);
			stackLayout.SetBinding(RadioButtonGroup.SelectedValueProperty, bindingBase10);
			translate6.Text = "CustomPidEditor_tbFormula.Text";
			IMarkupExtension markupExtension12 = translate6;
			XamlServiceProvider xamlServiceProvider12 = new XamlServiceProvider();
			Type typeFromHandle24 = typeof(IProvideValueTarget);
			object[] array13 = new object[0 + 6];
			array13[0] = radioButtonWithColor;
			array13[1] = stackLayout;
			array13[2] = stackLayout4;
			array13[3] = stackLayout8;
			array13[4] = scrollView;
			array13[5] = this;
			object obj22;
			xamlServiceProvider12.Add(typeFromHandle24, obj22 = new SimpleValueTargetProvider(array13, RadioButton.ContentProperty, nameScope));
			xamlServiceProvider12.Add(typeof(IReferenceProvider), obj22);
			Type typeFromHandle25 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver12 = new XmlNamespaceResolver();
			xmlNamespaceResolver12.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver12.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver12.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver12.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver12.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver12.Add("pids", "clr-namespace:CarScannerXamarinForms.OBD2.PIDS");
			xmlNamespaceResolver12.Add("syncfusion", "clr-namespace:Syncfusion.XForms.Buttons;assembly=Syncfusion.Buttons.XForms");
			xmlNamespaceResolver12.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider12.Add(typeFromHandle25, new XamlTypeResolver(xmlNamespaceResolver12, typeof(CustomPIDsEditorPage).GetTypeInfo().Assembly));
			xamlServiceProvider12.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(65, 29)));
			object obj23 = markupExtension12.ProvideValue(xamlServiceProvider12);
			radioButtonWithColor.SetValue(RadioButton.ContentProperty, obj23);
			radioButtonWithColor.SetValue(RadioButton.GroupNameProperty, "decodeType");
			radioButtonWithColor.SetValue(RadioButton.ValueProperty, customPIDType);
			stackLayout.Children.Add(radioButtonWithColor);
			translate7.Text = "ios_CustomPIDEditor_ByteSet";
			IMarkupExtension markupExtension13 = translate7;
			XamlServiceProvider xamlServiceProvider13 = new XamlServiceProvider();
			Type typeFromHandle26 = typeof(IProvideValueTarget);
			object[] array14 = new object[0 + 6];
			array14[0] = radioButtonWithColor2;
			array14[1] = stackLayout;
			array14[2] = stackLayout4;
			array14[3] = stackLayout8;
			array14[4] = scrollView;
			array14[5] = this;
			object obj24;
			xamlServiceProvider13.Add(typeFromHandle26, obj24 = new SimpleValueTargetProvider(array14, RadioButton.ContentProperty, nameScope));
			xamlServiceProvider13.Add(typeof(IReferenceProvider), obj24);
			Type typeFromHandle27 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver13 = new XmlNamespaceResolver();
			xmlNamespaceResolver13.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver13.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver13.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver13.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver13.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver13.Add("pids", "clr-namespace:CarScannerXamarinForms.OBD2.PIDS");
			xmlNamespaceResolver13.Add("syncfusion", "clr-namespace:Syncfusion.XForms.Buttons;assembly=Syncfusion.Buttons.XForms");
			xmlNamespaceResolver13.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider13.Add(typeFromHandle27, new XamlTypeResolver(xmlNamespaceResolver13, typeof(CustomPIDsEditorPage).GetTypeInfo().Assembly));
			xamlServiceProvider13.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(69, 29)));
			object obj25 = markupExtension13.ProvideValue(xamlServiceProvider13);
			radioButtonWithColor2.SetValue(RadioButton.ContentProperty, obj25);
			radioButtonWithColor2.SetValue(RadioButton.GroupNameProperty, "decodeType");
			radioButtonWithColor2.SetValue(RadioButton.ValueProperty, customPIDType2);
			stackLayout.Children.Add(radioButtonWithColor2);
			translate8.Text = "ios_Bit";
			IMarkupExtension markupExtension14 = translate8;
			XamlServiceProvider xamlServiceProvider14 = new XamlServiceProvider();
			Type typeFromHandle28 = typeof(IProvideValueTarget);
			object[] array15 = new object[0 + 6];
			array15[0] = radioButtonWithColor3;
			array15[1] = stackLayout;
			array15[2] = stackLayout4;
			array15[3] = stackLayout8;
			array15[4] = scrollView;
			array15[5] = this;
			object obj26;
			xamlServiceProvider14.Add(typeFromHandle28, obj26 = new SimpleValueTargetProvider(array15, RadioButton.ContentProperty, nameScope));
			xamlServiceProvider14.Add(typeof(IReferenceProvider), obj26);
			Type typeFromHandle29 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver14 = new XmlNamespaceResolver();
			xmlNamespaceResolver14.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver14.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver14.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver14.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver14.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver14.Add("pids", "clr-namespace:CarScannerXamarinForms.OBD2.PIDS");
			xmlNamespaceResolver14.Add("syncfusion", "clr-namespace:Syncfusion.XForms.Buttons;assembly=Syncfusion.Buttons.XForms");
			xmlNamespaceResolver14.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider14.Add(typeFromHandle29, new XamlTypeResolver(xmlNamespaceResolver14, typeof(CustomPIDsEditorPage).GetTypeInfo().Assembly));
			xamlServiceProvider14.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(73, 29)));
			object obj27 = markupExtension14.ProvideValue(xamlServiceProvider14);
			radioButtonWithColor3.SetValue(RadioButton.ContentProperty, obj27);
			radioButtonWithColor3.SetValue(RadioButton.GroupNameProperty, "decodeType");
			radioButtonWithColor3.SetValue(RadioButton.ValueProperty, customPIDType3);
			stackLayout.Children.Add(radioButtonWithColor3);
			translate9.Text = "ios_CustomPIDEditor_IsAction";
			IMarkupExtension markupExtension15 = translate9;
			XamlServiceProvider xamlServiceProvider15 = new XamlServiceProvider();
			Type typeFromHandle30 = typeof(IProvideValueTarget);
			object[] array16 = new object[0 + 6];
			array16[0] = radioButtonWithColor4;
			array16[1] = stackLayout;
			array16[2] = stackLayout4;
			array16[3] = stackLayout8;
			array16[4] = scrollView;
			array16[5] = this;
			object obj28;
			xamlServiceProvider15.Add(typeFromHandle30, obj28 = new SimpleValueTargetProvider(array16, RadioButton.ContentProperty, nameScope));
			xamlServiceProvider15.Add(typeof(IReferenceProvider), obj28);
			Type typeFromHandle31 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver15 = new XmlNamespaceResolver();
			xmlNamespaceResolver15.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver15.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver15.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver15.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver15.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver15.Add("pids", "clr-namespace:CarScannerXamarinForms.OBD2.PIDS");
			xmlNamespaceResolver15.Add("syncfusion", "clr-namespace:Syncfusion.XForms.Buttons;assembly=Syncfusion.Buttons.XForms");
			xmlNamespaceResolver15.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider15.Add(typeFromHandle31, new XamlTypeResolver(xmlNamespaceResolver15, typeof(CustomPIDsEditorPage).GetTypeInfo().Assembly));
			xamlServiceProvider15.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(77, 29)));
			object obj29 = markupExtension15.ProvideValue(xamlServiceProvider15);
			radioButtonWithColor4.SetValue(RadioButton.ContentProperty, obj29);
			radioButtonWithColor4.SetValue(RadioButton.GroupNameProperty, "decodeType");
			radioButtonWithColor4.SetValue(RadioButton.ValueProperty, customPIDType4);
			stackLayout.Children.Add(radioButtonWithColor4);
			radioButtonWithColor5.SetValue(RadioButton.ContentProperty, "VW TP2.0 Group Item");
			radioButtonWithColor5.SetValue(RadioButton.GroupNameProperty, "decodeType");
			radioButtonWithColor5.SetValue(RadioButton.ValueProperty, customPIDType5);
			stackLayout.Children.Add(radioButtonWithColor5);
			stackLayout4.Children.Add(stackLayout);
			bindingExtension11.Path = "Type";
			bindingExtension11.Mode = 2;
			staticResourceExtension6.Key = "EnumValueToTrueConverter";
			IMarkupExtension markupExtension16 = staticResourceExtension6;
			XamlServiceProvider xamlServiceProvider16 = new XamlServiceProvider();
			Type typeFromHandle32 = typeof(IProvideValueTarget);
			object[] array17 = new object[0 + 6];
			array17[0] = bindingExtension11;
			array17[1] = stackLayout3;
			array17[2] = stackLayout4;
			array17[3] = stackLayout8;
			array17[4] = scrollView;
			array17[5] = this;
			object obj30;
			xamlServiceProvider16.Add(typeFromHandle32, obj30 = new SimpleValueTargetProvider(array17, typeof(BindingExtension).GetRuntimeProperty("Converter"), nameScope));
			xamlServiceProvider16.Add(typeof(IReferenceProvider), obj30);
			Type typeFromHandle33 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver16 = new XmlNamespaceResolver();
			xmlNamespaceResolver16.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver16.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver16.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver16.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver16.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver16.Add("pids", "clr-namespace:CarScannerXamarinForms.OBD2.PIDS");
			xmlNamespaceResolver16.Add("syncfusion", "clr-namespace:Syncfusion.XForms.Buttons;assembly=Syncfusion.Buttons.XForms");
			xmlNamespaceResolver16.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider16.Add(typeFromHandle33, new XamlTypeResolver(xmlNamespaceResolver16, typeof(CustomPIDsEditorPage).GetTypeInfo().Assembly));
			xamlServiceProvider16.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(89, 25)));
			object obj31 = markupExtension16.ProvideValue(xamlServiceProvider16);
			bindingExtension11.Converter = obj31;
			bindingExtension11.ConverterParameter = "0";
			BindingBase bindingBase11 = bindingExtension11.ProvideValue(null);
			stackLayout3.SetBinding(VisualElement.IsVisibleProperty, bindingBase11);
			stackLayout3.SetValue(StackLayout.OrientationProperty, 0);
			stackLayout2.SetValue(StackLayout.OrientationProperty, 1);
			translate10.Text = "CustomPidEditor_tbFormula.Text";
			IMarkupExtension markupExtension17 = translate10;
			XamlServiceProvider xamlServiceProvider17 = new XamlServiceProvider();
			Type typeFromHandle34 = typeof(IProvideValueTarget);
			object[] array18 = new object[0 + 7];
			array18[0] = label6;
			array18[1] = stackLayout2;
			array18[2] = stackLayout3;
			array18[3] = stackLayout4;
			array18[4] = stackLayout8;
			array18[5] = scrollView;
			array18[6] = this;
			object obj32;
			xamlServiceProvider17.Add(typeFromHandle34, obj32 = new SimpleValueTargetProvider(array18, Label.TextProperty, nameScope));
			xamlServiceProvider17.Add(typeof(IReferenceProvider), obj32);
			Type typeFromHandle35 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver17 = new XmlNamespaceResolver();
			xmlNamespaceResolver17.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver17.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver17.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver17.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver17.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver17.Add("pids", "clr-namespace:CarScannerXamarinForms.OBD2.PIDS");
			xmlNamespaceResolver17.Add("syncfusion", "clr-namespace:Syncfusion.XForms.Buttons;assembly=Syncfusion.Buttons.XForms");
			xmlNamespaceResolver17.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider17.Add(typeFromHandle35, new XamlTypeResolver(xmlNamespaceResolver17, typeof(CustomPIDsEditorPage).GetTypeInfo().Assembly));
			xamlServiceProvider17.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(92, 36)));
			object obj33 = markupExtension17.ProvideValue(xamlServiceProvider17);
			label6.Text = obj33;
			stackLayout2.Children.Add(label6);
			label7.SetValue(Label.FontAttributesProperty, new FontAttributesConverter().ConvertFromInvariantString("Bold"));
			bindingExtension12.Mode = 2;
			staticResourceExtension7.Key = "BoolToNegativeConverter";
			IMarkupExtension markupExtension18 = staticResourceExtension7;
			XamlServiceProvider xamlServiceProvider18 = new XamlServiceProvider();
			Type typeFromHandle36 = typeof(IProvideValueTarget);
			object[] array19 = new object[0 + 8];
			array19[0] = bindingExtension12;
			array19[1] = label7;
			array19[2] = stackLayout2;
			array19[3] = stackLayout3;
			array19[4] = stackLayout4;
			array19[5] = stackLayout8;
			array19[6] = scrollView;
			array19[7] = this;
			object obj34;
			xamlServiceProvider18.Add(typeFromHandle36, obj34 = new SimpleValueTargetProvider(array19, typeof(BindingExtension).GetRuntimeProperty("Converter"), nameScope));
			xamlServiceProvider18.Add(typeof(IReferenceProvider), obj34);
			Type typeFromHandle37 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver18 = new XmlNamespaceResolver();
			xmlNamespaceResolver18.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver18.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver18.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver18.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver18.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver18.Add("pids", "clr-namespace:CarScannerXamarinForms.OBD2.PIDS");
			xmlNamespaceResolver18.Add("syncfusion", "clr-namespace:Syncfusion.XForms.Buttons;assembly=Syncfusion.Buttons.XForms");
			xmlNamespaceResolver18.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider18.Add(typeFromHandle37, new XamlTypeResolver(xmlNamespaceResolver18, typeof(CustomPIDsEditorPage).GetTypeInfo().Assembly));
			xamlServiceProvider18.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(97, 33)));
			object obj35 = markupExtension18.ProvideValue(xamlServiceProvider18);
			bindingExtension12.Converter = obj35;
			bindingExtension12.Path = "IsFormulaCorrect";
			BindingBase bindingBase12 = bindingExtension12.ProvideValue(null);
			label7.SetBinding(VisualElement.IsVisibleProperty, bindingBase12);
			translate11.Text = "CustomPidEditor_tbError.Text";
			IMarkupExtension markupExtension19 = translate11;
			XamlServiceProvider xamlServiceProvider19 = new XamlServiceProvider();
			Type typeFromHandle38 = typeof(IProvideValueTarget);
			object[] array20 = new object[0 + 7];
			array20[0] = label7;
			array20[1] = stackLayout2;
			array20[2] = stackLayout3;
			array20[3] = stackLayout4;
			array20[4] = stackLayout8;
			array20[5] = scrollView;
			array20[6] = this;
			object obj36;
			xamlServiceProvider19.Add(typeFromHandle38, obj36 = new SimpleValueTargetProvider(array20, Label.TextProperty, nameScope));
			xamlServiceProvider19.Add(typeof(IReferenceProvider), obj36);
			Type typeFromHandle39 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver19 = new XmlNamespaceResolver();
			xmlNamespaceResolver19.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver19.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver19.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver19.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver19.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver19.Add("pids", "clr-namespace:CarScannerXamarinForms.OBD2.PIDS");
			xmlNamespaceResolver19.Add("syncfusion", "clr-namespace:Syncfusion.XForms.Buttons;assembly=Syncfusion.Buttons.XForms");
			xmlNamespaceResolver19.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider19.Add(typeFromHandle39, new XamlTypeResolver(xmlNamespaceResolver19, typeof(CustomPIDsEditorPage).GetTypeInfo().Assembly));
			xamlServiceProvider19.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(98, 33)));
			object obj37 = markupExtension19.ProvideValue(xamlServiceProvider19);
			label7.Text = obj37;
			label7.SetValue(Label.TextColorProperty, Color.Red);
			stackLayout2.Children.Add(label7);
			label8.SetValue(Label.FontAttributesProperty, new FontAttributesConverter().ConvertFromInvariantString("Bold"));
			bindingExtension13.Mode = 2;
			bindingExtension13.Path = "IsFormulaCorrect";
			BindingBase bindingBase13 = bindingExtension13.ProvideValue(null);
			label8.SetBinding(VisualElement.IsVisibleProperty, bindingBase13);
			translate12.Text = "CustomPidEditor_tbOK.Text";
			IMarkupExtension markupExtension20 = translate12;
			XamlServiceProvider xamlServiceProvider20 = new XamlServiceProvider();
			Type typeFromHandle40 = typeof(IProvideValueTarget);
			object[] array21 = new object[0 + 7];
			array21[0] = label8;
			array21[1] = stackLayout2;
			array21[2] = stackLayout3;
			array21[3] = stackLayout4;
			array21[4] = stackLayout8;
			array21[5] = scrollView;
			array21[6] = this;
			object obj38;
			xamlServiceProvider20.Add(typeFromHandle40, obj38 = new SimpleValueTargetProvider(array21, Label.TextProperty, nameScope));
			xamlServiceProvider20.Add(typeof(IReferenceProvider), obj38);
			Type typeFromHandle41 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver20 = new XmlNamespaceResolver();
			xmlNamespaceResolver20.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver20.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver20.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver20.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver20.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver20.Add("pids", "clr-namespace:CarScannerXamarinForms.OBD2.PIDS");
			xmlNamespaceResolver20.Add("syncfusion", "clr-namespace:Syncfusion.XForms.Buttons;assembly=Syncfusion.Buttons.XForms");
			xmlNamespaceResolver20.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider20.Add(typeFromHandle41, new XamlTypeResolver(xmlNamespaceResolver20, typeof(CustomPIDsEditorPage).GetTypeInfo().Assembly));
			xamlServiceProvider20.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(103, 33)));
			object obj39 = markupExtension20.ProvideValue(xamlServiceProvider20);
			label8.Text = obj39;
			label8.SetValue(Label.TextColorProperty, Color.Green);
			stackLayout2.Children.Add(label8);
			stackLayout3.Children.Add(stackLayout2);
			bindingExtension14.Mode = 2;
			staticResourceExtension8.Key = "BoolToNegativeConverter";
			IMarkupExtension markupExtension21 = staticResourceExtension8;
			XamlServiceProvider xamlServiceProvider21 = new XamlServiceProvider();
			Type typeFromHandle42 = typeof(IProvideValueTarget);
			object[] array22 = new object[0 + 7];
			array22[0] = bindingExtension14;
			array22[1] = entry5;
			array22[2] = stackLayout3;
			array22[3] = stackLayout4;
			array22[4] = stackLayout8;
			array22[5] = scrollView;
			array22[6] = this;
			object obj40;
			xamlServiceProvider21.Add(typeFromHandle42, obj40 = new SimpleValueTargetProvider(array22, typeof(BindingExtension).GetRuntimeProperty("Converter"), nameScope));
			xamlServiceProvider21.Add(typeof(IReferenceProvider), obj40);
			Type typeFromHandle43 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver21 = new XmlNamespaceResolver();
			xmlNamespaceResolver21.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver21.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver21.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver21.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver21.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver21.Add("pids", "clr-namespace:CarScannerXamarinForms.OBD2.PIDS");
			xmlNamespaceResolver21.Add("syncfusion", "clr-namespace:Syncfusion.XForms.Buttons;assembly=Syncfusion.Buttons.XForms");
			xmlNamespaceResolver21.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider21.Add(typeFromHandle43, new XamlTypeResolver(xmlNamespaceResolver21, typeof(CustomPIDsEditorPage).GetTypeInfo().Assembly));
			xamlServiceProvider21.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(106, 32)));
			object obj41 = markupExtension21.ProvideValue(xamlServiceProvider21);
			bindingExtension14.Converter = obj41;
			bindingExtension14.Path = "IsFormulaHidden";
			BindingBase bindingBase14 = bindingExtension14.ProvideValue(null);
			entry5.SetBinding(VisualElement.IsVisibleProperty, bindingBase14);
			bindingExtension15.Mode = 1;
			bindingExtension15.Path = "Formula";
			BindingBase bindingBase15 = bindingExtension15.ProvideValue(null);
			entry5.SetBinding(Entry.TextProperty, bindingBase15);
			stackLayout3.Children.Add(entry5);
			button.Clicked += this.BtnInsertPidInFormula_Clicked;
			translate13.Text = "settings_InsertPid";
			IMarkupExtension markupExtension22 = translate13;
			XamlServiceProvider xamlServiceProvider22 = new XamlServiceProvider();
			Type typeFromHandle44 = typeof(IProvideValueTarget);
			object[] array23 = new object[0 + 6];
			array23[0] = button;
			array23[1] = stackLayout3;
			array23[2] = stackLayout4;
			array23[3] = stackLayout8;
			array23[4] = scrollView;
			array23[5] = this;
			object obj42;
			xamlServiceProvider22.Add(typeFromHandle44, obj42 = new SimpleValueTargetProvider(array23, Button.TextProperty, nameScope));
			xamlServiceProvider22.Add(typeof(IReferenceProvider), obj42);
			Type typeFromHandle45 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver22 = new XmlNamespaceResolver();
			xmlNamespaceResolver22.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver22.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver22.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver22.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver22.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver22.Add("pids", "clr-namespace:CarScannerXamarinForms.OBD2.PIDS");
			xmlNamespaceResolver22.Add("syncfusion", "clr-namespace:Syncfusion.XForms.Buttons;assembly=Syncfusion.Buttons.XForms");
			xmlNamespaceResolver22.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider22.Add(typeFromHandle45, new XamlTypeResolver(xmlNamespaceResolver22, typeof(CustomPIDsEditorPage).GetTypeInfo().Assembly));
			xamlServiceProvider22.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(107, 73)));
			object obj43 = markupExtension22.ProvideValue(xamlServiceProvider22);
			button.Text = obj43;
			stackLayout3.Children.Add(button);
			stackLayout4.Children.Add(stackLayout3);
			bindingExtension16.Path = "Type";
			bindingExtension16.Mode = 2;
			staticResourceExtension9.Key = "EnumValueToTrueConverter";
			IMarkupExtension markupExtension23 = staticResourceExtension9;
			XamlServiceProvider xamlServiceProvider23 = new XamlServiceProvider();
			Type typeFromHandle46 = typeof(IProvideValueTarget);
			object[] array24 = new object[0 + 6];
			array24[0] = bindingExtension16;
			array24[1] = grid;
			array24[2] = stackLayout4;
			array24[3] = stackLayout8;
			array24[4] = scrollView;
			array24[5] = this;
			object obj44;
			xamlServiceProvider23.Add(typeFromHandle46, obj44 = new SimpleValueTargetProvider(array24, typeof(BindingExtension).GetRuntimeProperty("Converter"), nameScope));
			xamlServiceProvider23.Add(typeof(IReferenceProvider), obj44);
			Type typeFromHandle47 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver23 = new XmlNamespaceResolver();
			xmlNamespaceResolver23.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver23.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver23.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver23.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver23.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver23.Add("pids", "clr-namespace:CarScannerXamarinForms.OBD2.PIDS");
			xmlNamespaceResolver23.Add("syncfusion", "clr-namespace:Syncfusion.XForms.Buttons;assembly=Syncfusion.Buttons.XForms");
			xmlNamespaceResolver23.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider23.Add(typeFromHandle47, new XamlTypeResolver(xmlNamespaceResolver23, typeof(CustomPIDsEditorPage).GetTypeInfo().Assembly));
			xamlServiceProvider23.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(113, 49)));
			object obj45 = markupExtension23.ProvideValue(xamlServiceProvider23);
			bindingExtension16.Converter = obj45;
			bindingExtension16.ConverterParameter = "1";
			BindingBase bindingBase16 = bindingExtension16.ProvideValue(null);
			grid.SetBinding(VisualElement.IsVisibleProperty, bindingBase16);
			columnDefinition.SetValue(ColumnDefinition.WidthProperty, new GridLengthTypeConverter().ConvertFromInvariantString("auto"));
			grid.GetValue(Grid.ColumnDefinitionsProperty).Add(columnDefinition);
			columnDefinition2.SetValue(ColumnDefinition.WidthProperty, new GridLengthTypeConverter().ConvertFromInvariantString("*"));
			grid.GetValue(Grid.ColumnDefinitionsProperty).Add(columnDefinition2);
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
			rowDefinition6.SetValue(RowDefinition.HeightProperty, new GridLengthTypeConverter().ConvertFromInvariantString("auto"));
			grid.GetValue(Grid.RowDefinitionsProperty).Add(rowDefinition6);
			rowDefinition7.SetValue(RowDefinition.HeightProperty, new GridLengthTypeConverter().ConvertFromInvariantString("auto"));
			grid.GetValue(Grid.RowDefinitionsProperty).Add(rowDefinition7);
			label9.SetValue(Grid.RowProperty, 0);
			label9.SetValue(Grid.ColumnProperty, 0);
			translate14.Text = "ios_Byte";
			IMarkupExtension markupExtension24 = translate14;
			XamlServiceProvider xamlServiceProvider24 = new XamlServiceProvider();
			Type typeFromHandle48 = typeof(IProvideValueTarget);
			object[] array25 = new object[0 + 6];
			array25[0] = label9;
			array25[1] = grid;
			array25[2] = stackLayout4;
			array25[3] = stackLayout8;
			array25[4] = scrollView;
			array25[5] = this;
			object obj46;
			xamlServiceProvider24.Add(typeFromHandle48, obj46 = new SimpleValueTargetProvider(array25, Label.TextProperty, nameScope));
			xamlServiceProvider24.Add(typeof(IReferenceProvider), obj46);
			Type typeFromHandle49 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver24 = new XmlNamespaceResolver();
			xmlNamespaceResolver24.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver24.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver24.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver24.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver24.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver24.Add("pids", "clr-namespace:CarScannerXamarinForms.OBD2.PIDS");
			xmlNamespaceResolver24.Add("syncfusion", "clr-namespace:Syncfusion.XForms.Buttons;assembly=Syncfusion.Buttons.XForms");
			xmlNamespaceResolver24.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider24.Add(typeFromHandle49, new XamlTypeResolver(xmlNamespaceResolver24, typeof(CustomPIDsEditorPage).GetTypeInfo().Assembly));
			xamlServiceProvider24.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(131, 29)));
			object obj47 = markupExtension24.ProvideValue(xamlServiceProvider24);
			label9.Text = obj47;
			label9.SetValue(View.VerticalOptionsProperty, LayoutOptions.Center);
			grid.Children.Add(label9);
			numericEntryV.SetValue(Grid.RowProperty, 0);
			numericEntryV.SetValue(Grid.ColumnProperty, 1);
			numericEntryV.SetValue(NumericEntryV3.DoubleFormatProperty, "0");
			numericEntryV.SetValue(NumericEntryV3.MinimumProperty, 0.0);
			bindingExtension17.Mode = 1;
			bindingExtension17.Path = "StartByteId";
			BindingBase bindingBase17 = bindingExtension17.ProvideValue(null);
			numericEntryV.SetBinding(NumericEntryV3.ValueProperty, bindingBase17);
			grid.Children.Add(numericEntryV);
			label10.SetValue(Grid.RowProperty, 1);
			label10.SetValue(Grid.ColumnProperty, 0);
			translate15.Text = "ios_DataLength";
			IMarkupExtension markupExtension25 = translate15;
			XamlServiceProvider xamlServiceProvider25 = new XamlServiceProvider();
			Type typeFromHandle50 = typeof(IProvideValueTarget);
			object[] array26 = new object[0 + 6];
			array26[0] = label10;
			array26[1] = grid;
			array26[2] = stackLayout4;
			array26[3] = stackLayout8;
			array26[4] = scrollView;
			array26[5] = this;
			object obj48;
			xamlServiceProvider25.Add(typeFromHandle50, obj48 = new SimpleValueTargetProvider(array26, Label.TextProperty, nameScope));
			xamlServiceProvider25.Add(typeof(IReferenceProvider), obj48);
			Type typeFromHandle51 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver25 = new XmlNamespaceResolver();
			xmlNamespaceResolver25.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver25.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver25.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver25.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver25.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver25.Add("pids", "clr-namespace:CarScannerXamarinForms.OBD2.PIDS");
			xmlNamespaceResolver25.Add("syncfusion", "clr-namespace:Syncfusion.XForms.Buttons;assembly=Syncfusion.Buttons.XForms");
			xmlNamespaceResolver25.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider25.Add(typeFromHandle51, new XamlTypeResolver(xmlNamespaceResolver25, typeof(CustomPIDsEditorPage).GetTypeInfo().Assembly));
			xamlServiceProvider25.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(146, 29)));
			object obj49 = markupExtension25.ProvideValue(xamlServiceProvider25);
			label10.Text = obj49;
			label10.SetValue(View.VerticalOptionsProperty, LayoutOptions.Center);
			grid.Children.Add(label10);
			numericEntryV2.SetValue(Grid.RowProperty, 1);
			numericEntryV2.SetValue(Grid.ColumnProperty, 1);
			numericEntryV2.SetValue(NumericEntryV3.DoubleFormatProperty, "0");
			numericEntryV2.SetValue(NumericEntryV3.MaximumProperty, 4.0);
			numericEntryV2.SetValue(NumericEntryV3.MinimumProperty, 1.0);
			bindingExtension18.Mode = 1;
			bindingExtension18.Path = "DataLength";
			BindingBase bindingBase18 = bindingExtension18.ProvideValue(null);
			numericEntryV2.SetBinding(NumericEntryV3.ValueProperty, bindingBase18);
			grid.Children.Add(numericEntryV2);
			labelSwitch.SetValue(Grid.RowProperty, 2);
			labelSwitch.SetValue(Grid.ColumnProperty, 0);
			labelSwitch.SetValue(Grid.ColumnSpanProperty, 2);
			bindingExtension19.Mode = 1;
			bindingExtension19.Path = "ReversedByteSet";
			BindingBase bindingBase19 = bindingExtension19.ProvideValue(null);
			labelSwitch.SetBinding(LabelSwitch.IsToggledProperty, bindingBase19);
			labelSwitch.SetValue(LabelSwitch.TextProperty, "Reversed byte order");
			grid.Children.Add(labelSwitch);
			labelSwitch2.SetValue(Grid.RowProperty, 3);
			labelSwitch2.SetValue(Grid.ColumnProperty, 0);
			labelSwitch2.SetValue(Grid.ColumnSpanProperty, 2);
			bindingExtension20.Mode = 1;
			bindingExtension20.Path = "IsSigned";
			BindingBase bindingBase20 = bindingExtension20.ProvideValue(null);
			labelSwitch2.SetBinding(LabelSwitch.IsToggledProperty, bindingBase20);
			labelSwitch2.SetValue(LabelSwitch.TextProperty, "Signed value");
			grid.Children.Add(labelSwitch2);
			label11.SetValue(Grid.RowProperty, 4);
			label11.SetValue(Grid.ColumnProperty, 0);
			translate16.Text = "ios_Multiplier";
			IMarkupExtension markupExtension26 = translate16;
			XamlServiceProvider xamlServiceProvider26 = new XamlServiceProvider();
			Type typeFromHandle52 = typeof(IProvideValueTarget);
			object[] array27 = new object[0 + 6];
			array27[0] = label11;
			array27[1] = grid;
			array27[2] = stackLayout4;
			array27[3] = stackLayout8;
			array27[4] = scrollView;
			array27[5] = this;
			object obj50;
			xamlServiceProvider26.Add(typeFromHandle52, obj50 = new SimpleValueTargetProvider(array27, Label.TextProperty, nameScope));
			xamlServiceProvider26.Add(typeof(IReferenceProvider), obj50);
			Type typeFromHandle53 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver26 = new XmlNamespaceResolver();
			xmlNamespaceResolver26.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver26.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver26.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver26.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver26.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver26.Add("pids", "clr-namespace:CarScannerXamarinForms.OBD2.PIDS");
			xmlNamespaceResolver26.Add("syncfusion", "clr-namespace:Syncfusion.XForms.Buttons;assembly=Syncfusion.Buttons.XForms");
			xmlNamespaceResolver26.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider26.Add(typeFromHandle53, new XamlTypeResolver(xmlNamespaceResolver26, typeof(CustomPIDsEditorPage).GetTypeInfo().Assembly));
			xamlServiceProvider26.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(171, 29)));
			object obj51 = markupExtension26.ProvideValue(xamlServiceProvider26);
			label11.Text = obj51;
			label11.SetValue(View.VerticalOptionsProperty, LayoutOptions.Center);
			grid.Children.Add(label11);
			numericEntryV3.SetValue(Grid.RowProperty, 4);
			numericEntryV3.SetValue(Grid.ColumnProperty, 1);
			bindingExtension21.Mode = 1;
			bindingExtension21.Path = "Multiplier";
			BindingBase bindingBase21 = bindingExtension21.ProvideValue(null);
			numericEntryV3.SetBinding(NumericEntryV3.ValueProperty, bindingBase21);
			grid.Children.Add(numericEntryV3);
			label12.SetValue(Grid.RowProperty, 5);
			label12.SetValue(Grid.ColumnProperty, 0);
			translate17.Text = "ios_Divider";
			IMarkupExtension markupExtension27 = translate17;
			XamlServiceProvider xamlServiceProvider27 = new XamlServiceProvider();
			Type typeFromHandle54 = typeof(IProvideValueTarget);
			object[] array28 = new object[0 + 6];
			array28[0] = label12;
			array28[1] = grid;
			array28[2] = stackLayout4;
			array28[3] = stackLayout8;
			array28[4] = scrollView;
			array28[5] = this;
			object obj52;
			xamlServiceProvider27.Add(typeFromHandle54, obj52 = new SimpleValueTargetProvider(array28, Label.TextProperty, nameScope));
			xamlServiceProvider27.Add(typeof(IReferenceProvider), obj52);
			Type typeFromHandle55 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver27 = new XmlNamespaceResolver();
			xmlNamespaceResolver27.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver27.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver27.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver27.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver27.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver27.Add("pids", "clr-namespace:CarScannerXamarinForms.OBD2.PIDS");
			xmlNamespaceResolver27.Add("syncfusion", "clr-namespace:Syncfusion.XForms.Buttons;assembly=Syncfusion.Buttons.XForms");
			xmlNamespaceResolver27.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider27.Add(typeFromHandle55, new XamlTypeResolver(xmlNamespaceResolver27, typeof(CustomPIDsEditorPage).GetTypeInfo().Assembly));
			xamlServiceProvider27.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(180, 29)));
			object obj53 = markupExtension27.ProvideValue(xamlServiceProvider27);
			label12.Text = obj53;
			label12.SetValue(View.VerticalOptionsProperty, LayoutOptions.Center);
			grid.Children.Add(label12);
			numericEntryV4.SetValue(Grid.RowProperty, 5);
			numericEntryV4.SetValue(Grid.ColumnProperty, 1);
			bindingExtension22.Mode = 1;
			bindingExtension22.Path = "Divider";
			BindingBase bindingBase22 = bindingExtension22.ProvideValue(null);
			numericEntryV4.SetBinding(NumericEntryV3.ValueProperty, bindingBase22);
			grid.Children.Add(numericEntryV4);
			label13.SetValue(Grid.RowProperty, 6);
			label13.SetValue(Grid.ColumnProperty, 0);
			translate18.Text = "ios_Offset";
			IMarkupExtension markupExtension28 = translate18;
			XamlServiceProvider xamlServiceProvider28 = new XamlServiceProvider();
			Type typeFromHandle56 = typeof(IProvideValueTarget);
			object[] array29 = new object[0 + 6];
			array29[0] = label13;
			array29[1] = grid;
			array29[2] = stackLayout4;
			array29[3] = stackLayout8;
			array29[4] = scrollView;
			array29[5] = this;
			object obj54;
			xamlServiceProvider28.Add(typeFromHandle56, obj54 = new SimpleValueTargetProvider(array29, Label.TextProperty, nameScope));
			xamlServiceProvider28.Add(typeof(IReferenceProvider), obj54);
			Type typeFromHandle57 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver28 = new XmlNamespaceResolver();
			xmlNamespaceResolver28.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver28.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver28.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver28.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver28.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver28.Add("pids", "clr-namespace:CarScannerXamarinForms.OBD2.PIDS");
			xmlNamespaceResolver28.Add("syncfusion", "clr-namespace:Syncfusion.XForms.Buttons;assembly=Syncfusion.Buttons.XForms");
			xmlNamespaceResolver28.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider28.Add(typeFromHandle57, new XamlTypeResolver(xmlNamespaceResolver28, typeof(CustomPIDsEditorPage).GetTypeInfo().Assembly));
			xamlServiceProvider28.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(189, 29)));
			object obj55 = markupExtension28.ProvideValue(xamlServiceProvider28);
			label13.Text = obj55;
			label13.SetValue(View.VerticalOptionsProperty, LayoutOptions.Center);
			grid.Children.Add(label13);
			numericEntryV5.SetValue(Grid.RowProperty, 6);
			numericEntryV5.SetValue(Grid.ColumnProperty, 1);
			bindingExtension23.Mode = 1;
			bindingExtension23.Path = "Offset";
			BindingBase bindingBase23 = bindingExtension23.ProvideValue(null);
			numericEntryV5.SetBinding(NumericEntryV3.ValueProperty, bindingBase23);
			grid.Children.Add(numericEntryV5);
			stackLayout4.Children.Add(grid);
			bindingExtension24.Path = "Type";
			bindingExtension24.Mode = 2;
			staticResourceExtension10.Key = "EnumValueToTrueConverter";
			IMarkupExtension markupExtension29 = staticResourceExtension10;
			XamlServiceProvider xamlServiceProvider29 = new XamlServiceProvider();
			Type typeFromHandle58 = typeof(IProvideValueTarget);
			object[] array30 = new object[0 + 6];
			array30[0] = bindingExtension24;
			array30[1] = grid2;
			array30[2] = stackLayout4;
			array30[3] = stackLayout8;
			array30[4] = scrollView;
			array30[5] = this;
			object obj56;
			xamlServiceProvider29.Add(typeFromHandle58, obj56 = new SimpleValueTargetProvider(array30, typeof(BindingExtension).GetRuntimeProperty("Converter"), nameScope));
			xamlServiceProvider29.Add(typeof(IReferenceProvider), obj56);
			Type typeFromHandle59 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver29 = new XmlNamespaceResolver();
			xmlNamespaceResolver29.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver29.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver29.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver29.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver29.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver29.Add("pids", "clr-namespace:CarScannerXamarinForms.OBD2.PIDS");
			xmlNamespaceResolver29.Add("syncfusion", "clr-namespace:Syncfusion.XForms.Buttons;assembly=Syncfusion.Buttons.XForms");
			xmlNamespaceResolver29.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider29.Add(typeFromHandle59, new XamlTypeResolver(xmlNamespaceResolver29, typeof(CustomPIDsEditorPage).GetTypeInfo().Assembly));
			xamlServiceProvider29.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(197, 50)));
			object obj57 = markupExtension29.ProvideValue(xamlServiceProvider29);
			bindingExtension24.Converter = obj57;
			bindingExtension24.ConverterParameter = "2";
			BindingBase bindingBase24 = bindingExtension24.ProvideValue(null);
			grid2.SetBinding(VisualElement.IsVisibleProperty, bindingBase24);
			columnDefinition3.SetValue(ColumnDefinition.WidthProperty, new GridLengthTypeConverter().ConvertFromInvariantString("auto"));
			grid2.GetValue(Grid.ColumnDefinitionsProperty).Add(columnDefinition3);
			columnDefinition4.SetValue(ColumnDefinition.WidthProperty, new GridLengthTypeConverter().ConvertFromInvariantString("*"));
			grid2.GetValue(Grid.ColumnDefinitionsProperty).Add(columnDefinition4);
			rowDefinition8.SetValue(RowDefinition.HeightProperty, new GridLengthTypeConverter().ConvertFromInvariantString("auto"));
			grid2.GetValue(Grid.RowDefinitionsProperty).Add(rowDefinition8);
			rowDefinition9.SetValue(RowDefinition.HeightProperty, new GridLengthTypeConverter().ConvertFromInvariantString("auto"));
			grid2.GetValue(Grid.RowDefinitionsProperty).Add(rowDefinition9);
			rowDefinition10.SetValue(RowDefinition.HeightProperty, new GridLengthTypeConverter().ConvertFromInvariantString("auto"));
			grid2.GetValue(Grid.RowDefinitionsProperty).Add(rowDefinition10);
			rowDefinition11.SetValue(RowDefinition.HeightProperty, new GridLengthTypeConverter().ConvertFromInvariantString("auto"));
			grid2.GetValue(Grid.RowDefinitionsProperty).Add(rowDefinition11);
			rowDefinition12.SetValue(RowDefinition.HeightProperty, new GridLengthTypeConverter().ConvertFromInvariantString("auto"));
			grid2.GetValue(Grid.RowDefinitionsProperty).Add(rowDefinition12);
			rowDefinition13.SetValue(RowDefinition.HeightProperty, new GridLengthTypeConverter().ConvertFromInvariantString("auto"));
			grid2.GetValue(Grid.RowDefinitionsProperty).Add(rowDefinition13);
			rowDefinition14.SetValue(RowDefinition.HeightProperty, new GridLengthTypeConverter().ConvertFromInvariantString("auto"));
			grid2.GetValue(Grid.RowDefinitionsProperty).Add(rowDefinition14);
			label14.SetValue(Grid.RowProperty, 0);
			label14.SetValue(Grid.ColumnProperty, 0);
			translate19.Text = "ios_Byte";
			IMarkupExtension markupExtension30 = translate19;
			XamlServiceProvider xamlServiceProvider30 = new XamlServiceProvider();
			Type typeFromHandle60 = typeof(IProvideValueTarget);
			object[] array31 = new object[0 + 6];
			array31[0] = label14;
			array31[1] = grid2;
			array31[2] = stackLayout4;
			array31[3] = stackLayout8;
			array31[4] = scrollView;
			array31[5] = this;
			object obj58;
			xamlServiceProvider30.Add(typeFromHandle60, obj58 = new SimpleValueTargetProvider(array31, Label.TextProperty, nameScope));
			xamlServiceProvider30.Add(typeof(IReferenceProvider), obj58);
			Type typeFromHandle61 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver30 = new XmlNamespaceResolver();
			xmlNamespaceResolver30.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver30.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver30.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver30.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver30.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver30.Add("pids", "clr-namespace:CarScannerXamarinForms.OBD2.PIDS");
			xmlNamespaceResolver30.Add("syncfusion", "clr-namespace:Syncfusion.XForms.Buttons;assembly=Syncfusion.Buttons.XForms");
			xmlNamespaceResolver30.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider30.Add(typeFromHandle61, new XamlTypeResolver(xmlNamespaceResolver30, typeof(CustomPIDsEditorPage).GetTypeInfo().Assembly));
			xamlServiceProvider30.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(215, 29)));
			object obj59 = markupExtension30.ProvideValue(xamlServiceProvider30);
			label14.Text = obj59;
			grid2.Children.Add(label14);
			numericEntryV6.SetValue(Grid.RowProperty, 0);
			numericEntryV6.SetValue(Grid.ColumnProperty, 1);
			numericEntryV6.SetValue(NumericEntryV3.DoubleFormatProperty, "0");
			numericEntryV6.SetValue(NumericEntryV3.MinimumProperty, 0.0);
			bindingExtension25.Mode = 1;
			bindingExtension25.Path = "StartByteId";
			BindingBase bindingBase25 = bindingExtension25.ProvideValue(null);
			numericEntryV6.SetBinding(NumericEntryV3.ValueProperty, bindingBase25);
			grid2.Children.Add(numericEntryV6);
			label15.SetValue(Grid.RowProperty, 1);
			label15.SetValue(Grid.ColumnProperty, 0);
			translate20.Text = "ios_Bit";
			IMarkupExtension markupExtension31 = translate20;
			XamlServiceProvider xamlServiceProvider31 = new XamlServiceProvider();
			Type typeFromHandle62 = typeof(IProvideValueTarget);
			object[] array32 = new object[0 + 6];
			array32[0] = label15;
			array32[1] = grid2;
			array32[2] = stackLayout4;
			array32[3] = stackLayout8;
			array32[4] = scrollView;
			array32[5] = this;
			object obj60;
			xamlServiceProvider31.Add(typeFromHandle62, obj60 = new SimpleValueTargetProvider(array32, Label.TextProperty, nameScope));
			xamlServiceProvider31.Add(typeof(IReferenceProvider), obj60);
			Type typeFromHandle63 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver31 = new XmlNamespaceResolver();
			xmlNamespaceResolver31.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver31.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver31.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver31.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver31.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver31.Add("pids", "clr-namespace:CarScannerXamarinForms.OBD2.PIDS");
			xmlNamespaceResolver31.Add("syncfusion", "clr-namespace:Syncfusion.XForms.Buttons;assembly=Syncfusion.Buttons.XForms");
			xmlNamespaceResolver31.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider31.Add(typeFromHandle63, new XamlTypeResolver(xmlNamespaceResolver31, typeof(CustomPIDsEditorPage).GetTypeInfo().Assembly));
			xamlServiceProvider31.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(225, 29)));
			object obj61 = markupExtension31.ProvideValue(xamlServiceProvider31);
			label15.Text = obj61;
			grid2.Children.Add(label15);
			numericEntryV7.SetValue(Grid.RowProperty, 1);
			numericEntryV7.SetValue(Grid.ColumnProperty, 1);
			numericEntryV7.SetValue(NumericEntryV3.DoubleFormatProperty, "0");
			numericEntryV7.SetValue(NumericEntryV3.MaximumProperty, 7.0);
			numericEntryV7.SetValue(NumericEntryV3.MinimumProperty, 0.0);
			bindingExtension26.Mode = 1;
			bindingExtension26.Path = "Bit";
			BindingBase bindingBase26 = bindingExtension26.ProvideValue(null);
			numericEntryV7.SetBinding(NumericEntryV3.ValueProperty, bindingBase26);
			grid2.Children.Add(numericEntryV7);
			stackLayout4.Children.Add(grid2);
			stackLayout8.Children.Add(stackLayout4);
			bindingExtension27.Path = "Type";
			bindingExtension27.Mode = 2;
			staticResourceExtension11.Key = "EnumValueToTrueConverter";
			IMarkupExtension markupExtension32 = staticResourceExtension11;
			XamlServiceProvider xamlServiceProvider32 = new XamlServiceProvider();
			Type typeFromHandle64 = typeof(IProvideValueTarget);
			object[] array33 = new object[0 + 5];
			array33[0] = bindingExtension27;
			array33[1] = stackLayout5;
			array33[2] = stackLayout8;
			array33[3] = scrollView;
			array33[4] = this;
			object obj62;
			xamlServiceProvider32.Add(typeFromHandle64, obj62 = new SimpleValueTargetProvider(array33, typeof(BindingExtension).GetRuntimeProperty("Converter"), nameScope));
			xamlServiceProvider32.Add(typeof(IReferenceProvider), obj62);
			Type typeFromHandle65 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver32 = new XmlNamespaceResolver();
			xmlNamespaceResolver32.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver32.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver32.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver32.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver32.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver32.Add("pids", "clr-namespace:CarScannerXamarinForms.OBD2.PIDS");
			xmlNamespaceResolver32.Add("syncfusion", "clr-namespace:Syncfusion.XForms.Buttons;assembly=Syncfusion.Buttons.XForms");
			xmlNamespaceResolver32.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider32.Add(typeFromHandle65, new XamlTypeResolver(xmlNamespaceResolver32, typeof(CustomPIDsEditorPage).GetTypeInfo().Assembly));
			xamlServiceProvider32.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(239, 21)));
			object obj63 = markupExtension32.ProvideValue(xamlServiceProvider32);
			bindingExtension27.Converter = obj63;
			bindingExtension27.ConverterParameter = "5";
			BindingBase bindingBase27 = bindingExtension27.ProvideValue(null);
			stackLayout5.SetBinding(VisualElement.IsVisibleProperty, bindingBase27);
			stackLayout5.SetValue(StackLayout.OrientationProperty, 0);
			label16.SetValue(Grid.RowProperty, 0);
			label16.SetValue(Grid.ColumnProperty, 0);
			label16.SetValue(Label.TextProperty, "Unit:");
			stackLayout5.Children.Add(label16);
			entry6.SetValue(Grid.RowProperty, 0);
			entry6.SetValue(Grid.ColumnProperty, 1);
			bindingExtension28.Mode = 1;
			bindingExtension28.Path = "VagUnit";
			BindingBase bindingBase28 = bindingExtension28.ProvideValue(null);
			entry6.SetBinding(Entry.TextProperty, bindingBase28);
			stackLayout5.Children.Add(entry6);
			label17.SetValue(Grid.RowProperty, 1);
			label17.SetValue(Grid.ColumnProperty, 0);
			label17.SetValue(Label.TextProperty, "Group:");
			stackLayout5.Children.Add(label17);
			numericEntryV8.SetValue(Grid.RowProperty, 1);
			numericEntryV8.SetValue(Grid.ColumnProperty, 1);
			numericEntryV8.SetValue(NumericEntryV3.DoubleFormatProperty, "0");
			numericEntryV8.SetValue(NumericEntryV3.MaximumProperty, 999.0);
			numericEntryV8.SetValue(NumericEntryV3.MinimumProperty, 0.0);
			bindingExtension29.Mode = 1;
			bindingExtension29.Path = "VagGroup";
			BindingBase bindingBase29 = bindingExtension29.ProvideValue(null);
			numericEntryV8.SetBinding(NumericEntryV3.ValueProperty, bindingBase29);
			stackLayout5.Children.Add(numericEntryV8);
			label18.SetValue(Grid.RowProperty, 1);
			label18.SetValue(Grid.ColumnProperty, 0);
			label18.SetValue(Label.TextProperty, "Item (1-4):");
			stackLayout5.Children.Add(label18);
			numericEntryV9.SetValue(Grid.RowProperty, 1);
			numericEntryV9.SetValue(Grid.ColumnProperty, 1);
			numericEntryV9.SetValue(NumericEntryV3.DoubleFormatProperty, "0");
			numericEntryV9.SetValue(NumericEntryV3.MaximumProperty, 4.0);
			numericEntryV9.SetValue(NumericEntryV3.MinimumProperty, 1.0);
			bindingExtension30.Mode = 1;
			bindingExtension30.Path = "VagItemInGroup";
			BindingBase bindingBase30 = bindingExtension30.ProvideValue(null);
			numericEntryV9.SetBinding(NumericEntryV3.ValueProperty, bindingBase30);
			stackLayout5.Children.Add(numericEntryV9);
			button2.Clicked += this.btnApplyVagGroup_Tapped;
			translate21.Text = "coding_Apply";
			IMarkupExtension markupExtension33 = translate21;
			XamlServiceProvider xamlServiceProvider33 = new XamlServiceProvider();
			Type typeFromHandle66 = typeof(IProvideValueTarget);
			object[] array34 = new object[0 + 5];
			array34[0] = button2;
			array34[1] = stackLayout5;
			array34[2] = stackLayout8;
			array34[3] = scrollView;
			array34[4] = this;
			object obj64;
			xamlServiceProvider33.Add(typeFromHandle66, obj64 = new SimpleValueTargetProvider(array34, Button.TextProperty, nameScope));
			xamlServiceProvider33.Add(typeof(IReferenceProvider), obj64);
			Type typeFromHandle67 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver33 = new XmlNamespaceResolver();
			xmlNamespaceResolver33.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver33.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver33.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver33.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver33.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver33.Add("pids", "clr-namespace:CarScannerXamarinForms.OBD2.PIDS");
			xmlNamespaceResolver33.Add("syncfusion", "clr-namespace:Syncfusion.XForms.Buttons;assembly=Syncfusion.Buttons.XForms");
			xmlNamespaceResolver33.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider33.Add(typeFromHandle67, new XamlTypeResolver(xmlNamespaceResolver33, typeof(CustomPIDsEditorPage).GetTypeInfo().Assembly));
			xamlServiceProvider33.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(280, 25)));
			object obj65 = markupExtension33.ProvideValue(xamlServiceProvider33);
			button2.Text = obj65;
			stackLayout5.Children.Add(button2);
			stackLayout8.Children.Add(stackLayout5);
			bindingExtension31.Path = "Type";
			bindingExtension31.Mode = 2;
			staticResourceExtension12.Key = "EnumValueToFalseConverter";
			IMarkupExtension markupExtension34 = staticResourceExtension12;
			XamlServiceProvider xamlServiceProvider34 = new XamlServiceProvider();
			Type typeFromHandle68 = typeof(IProvideValueTarget);
			object[] array35 = new object[0 + 5];
			array35[0] = bindingExtension31;
			array35[1] = stackLayout6;
			array35[2] = stackLayout8;
			array35[3] = scrollView;
			array35[4] = this;
			object obj66;
			xamlServiceProvider34.Add(typeFromHandle68, obj66 = new SimpleValueTargetProvider(array35, typeof(BindingExtension).GetRuntimeProperty("Converter"), nameScope));
			xamlServiceProvider34.Add(typeof(IReferenceProvider), obj66);
			Type typeFromHandle69 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver34 = new XmlNamespaceResolver();
			xmlNamespaceResolver34.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver34.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver34.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver34.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver34.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver34.Add("pids", "clr-namespace:CarScannerXamarinForms.OBD2.PIDS");
			xmlNamespaceResolver34.Add("syncfusion", "clr-namespace:Syncfusion.XForms.Buttons;assembly=Syncfusion.Buttons.XForms");
			xmlNamespaceResolver34.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider34.Add(typeFromHandle69, new XamlTypeResolver(xmlNamespaceResolver34, typeof(CustomPIDsEditorPage).GetTypeInfo().Assembly));
			xamlServiceProvider34.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(283, 30)));
			object obj67 = markupExtension34.ProvideValue(xamlServiceProvider34);
			bindingExtension31.Converter = obj67;
			bindingExtension31.ConverterParameter = "3";
			BindingBase bindingBase31 = bindingExtension31.ProvideValue(null);
			stackLayout6.SetBinding(VisualElement.IsVisibleProperty, bindingBase31);
			stackLayout6.SetValue(StackLayout.OrientationProperty, 0);
			translate22.Text = "Mode06Page_tbMinimum.Text";
			IMarkupExtension markupExtension35 = translate22;
			XamlServiceProvider xamlServiceProvider35 = new XamlServiceProvider();
			Type typeFromHandle70 = typeof(IProvideValueTarget);
			object[] array36 = new object[0 + 5];
			array36[0] = label19;
			array36[1] = stackLayout6;
			array36[2] = stackLayout8;
			array36[3] = scrollView;
			array36[4] = this;
			object obj68;
			xamlServiceProvider35.Add(typeFromHandle70, obj68 = new SimpleValueTargetProvider(array36, Label.TextProperty, nameScope));
			xamlServiceProvider35.Add(typeof(IReferenceProvider), obj68);
			Type typeFromHandle71 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver35 = new XmlNamespaceResolver();
			xmlNamespaceResolver35.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver35.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver35.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver35.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver35.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver35.Add("pids", "clr-namespace:CarScannerXamarinForms.OBD2.PIDS");
			xmlNamespaceResolver35.Add("syncfusion", "clr-namespace:Syncfusion.XForms.Buttons;assembly=Syncfusion.Buttons.XForms");
			xmlNamespaceResolver35.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider35.Add(typeFromHandle71, new XamlTypeResolver(xmlNamespaceResolver35, typeof(CustomPIDsEditorPage).GetTypeInfo().Assembly));
			xamlServiceProvider35.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(284, 28)));
			object obj69 = markupExtension35.ProvideValue(xamlServiceProvider35);
			label19.Text = obj69;
			stackLayout6.Children.Add(label19);
			bindingExtension32.Mode = 1;
			bindingExtension32.Path = "Minimum";
			BindingBase bindingBase32 = bindingExtension32.ProvideValue(null);
			numericEntryV10.SetBinding(NumericEntryV3.ValueProperty, bindingBase32);
			stackLayout6.Children.Add(numericEntryV10);
			translate23.Text = "Mode06Page_tbMaximum.Text";
			IMarkupExtension markupExtension36 = translate23;
			XamlServiceProvider xamlServiceProvider36 = new XamlServiceProvider();
			Type typeFromHandle72 = typeof(IProvideValueTarget);
			object[] array37 = new object[0 + 5];
			array37[0] = label20;
			array37[1] = stackLayout6;
			array37[2] = stackLayout8;
			array37[3] = scrollView;
			array37[4] = this;
			object obj70;
			xamlServiceProvider36.Add(typeFromHandle72, obj70 = new SimpleValueTargetProvider(array37, Label.TextProperty, nameScope));
			xamlServiceProvider36.Add(typeof(IReferenceProvider), obj70);
			Type typeFromHandle73 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver36 = new XmlNamespaceResolver();
			xmlNamespaceResolver36.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver36.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver36.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver36.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver36.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver36.Add("pids", "clr-namespace:CarScannerXamarinForms.OBD2.PIDS");
			xmlNamespaceResolver36.Add("syncfusion", "clr-namespace:Syncfusion.XForms.Buttons;assembly=Syncfusion.Buttons.XForms");
			xmlNamespaceResolver36.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider36.Add(typeFromHandle73, new XamlTypeResolver(xmlNamespaceResolver36, typeof(CustomPIDsEditorPage).GetTypeInfo().Assembly));
			xamlServiceProvider36.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(286, 28)));
			object obj71 = markupExtension36.ProvideValue(xamlServiceProvider36);
			label20.Text = obj71;
			stackLayout6.Children.Add(label20);
			bindingExtension33.Mode = 1;
			bindingExtension33.Path = "Maximum";
			BindingBase bindingBase33 = bindingExtension33.ProvideValue(null);
			numericEntryV11.SetBinding(NumericEntryV3.ValueProperty, bindingBase33);
			stackLayout6.Children.Add(numericEntryV11);
			translate24.Text = "ios_Units";
			IMarkupExtension markupExtension37 = translate24;
			XamlServiceProvider xamlServiceProvider37 = new XamlServiceProvider();
			Type typeFromHandle74 = typeof(IProvideValueTarget);
			object[] array38 = new object[0 + 5];
			array38[0] = label21;
			array38[1] = stackLayout6;
			array38[2] = stackLayout8;
			array38[3] = scrollView;
			array38[4] = this;
			object obj72;
			xamlServiceProvider37.Add(typeFromHandle74, obj72 = new SimpleValueTargetProvider(array38, Label.TextProperty, nameScope));
			xamlServiceProvider37.Add(typeof(IReferenceProvider), obj72);
			Type typeFromHandle75 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver37 = new XmlNamespaceResolver();
			xmlNamespaceResolver37.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver37.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver37.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver37.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver37.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver37.Add("pids", "clr-namespace:CarScannerXamarinForms.OBD2.PIDS");
			xmlNamespaceResolver37.Add("syncfusion", "clr-namespace:Syncfusion.XForms.Buttons;assembly=Syncfusion.Buttons.XForms");
			xmlNamespaceResolver37.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider37.Add(typeFromHandle75, new XamlTypeResolver(xmlNamespaceResolver37, typeof(CustomPIDsEditorPage).GetTypeInfo().Assembly));
			xamlServiceProvider37.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(289, 28)));
			object obj73 = markupExtension37.ProvideValue(xamlServiceProvider37);
			label21.Text = obj73;
			stackLayout6.Children.Add(label21);
			bindingExtension34.Source = units;
			BindingBase bindingBase34 = bindingExtension34.ProvideValue(null);
			picker.SetBinding(Picker.ItemsSourceProperty, bindingBase34);
			bindingExtension35.Mode = 1;
			staticResourceExtension13.Key = "UnitsToIntConverter";
			IMarkupExtension markupExtension38 = staticResourceExtension13;
			XamlServiceProvider xamlServiceProvider38 = new XamlServiceProvider();
			Type typeFromHandle76 = typeof(IProvideValueTarget);
			object[] array39 = new object[0 + 6];
			array39[0] = bindingExtension35;
			array39[1] = picker;
			array39[2] = stackLayout6;
			array39[3] = stackLayout8;
			array39[4] = scrollView;
			array39[5] = this;
			object obj74;
			xamlServiceProvider38.Add(typeFromHandle76, obj74 = new SimpleValueTargetProvider(array39, typeof(BindingExtension).GetRuntimeProperty("Converter"), nameScope));
			xamlServiceProvider38.Add(typeof(IReferenceProvider), obj74);
			Type typeFromHandle77 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver38 = new XmlNamespaceResolver();
			xmlNamespaceResolver38.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver38.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver38.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver38.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver38.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver38.Add("pids", "clr-namespace:CarScannerXamarinForms.OBD2.PIDS");
			xmlNamespaceResolver38.Add("syncfusion", "clr-namespace:Syncfusion.XForms.Buttons;assembly=Syncfusion.Buttons.XForms");
			xmlNamespaceResolver38.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider38.Add(typeFromHandle77, new XamlTypeResolver(xmlNamespaceResolver38, typeof(CustomPIDsEditorPage).GetTypeInfo().Assembly));
			xamlServiceProvider38.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(290, 95)));
			object obj75 = markupExtension38.ProvideValue(xamlServiceProvider38);
			bindingExtension35.Converter = obj75;
			bindingExtension35.Path = "Units";
			BindingBase bindingBase35 = bindingExtension35.ProvideValue(null);
			picker.SetBinding(Picker.SelectedIndexProperty, bindingBase35);
			stackLayout6.Children.Add(picker);
			translate25.Text = "ios_Priority";
			IMarkupExtension markupExtension39 = translate25;
			XamlServiceProvider xamlServiceProvider39 = new XamlServiceProvider();
			Type typeFromHandle78 = typeof(IProvideValueTarget);
			object[] array40 = new object[0 + 5];
			array40[0] = label22;
			array40[1] = stackLayout6;
			array40[2] = stackLayout8;
			array40[3] = scrollView;
			array40[4] = this;
			object obj76;
			xamlServiceProvider39.Add(typeFromHandle78, obj76 = new SimpleValueTargetProvider(array40, Label.TextProperty, nameScope));
			xamlServiceProvider39.Add(typeof(IReferenceProvider), obj76);
			Type typeFromHandle79 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver39 = new XmlNamespaceResolver();
			xmlNamespaceResolver39.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver39.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver39.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver39.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver39.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver39.Add("pids", "clr-namespace:CarScannerXamarinForms.OBD2.PIDS");
			xmlNamespaceResolver39.Add("syncfusion", "clr-namespace:Syncfusion.XForms.Buttons;assembly=Syncfusion.Buttons.XForms");
			xmlNamespaceResolver39.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider39.Add(typeFromHandle79, new XamlTypeResolver(xmlNamespaceResolver39, typeof(CustomPIDsEditorPage).GetTypeInfo().Assembly));
			xamlServiceProvider39.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(315, 28)));
			object obj77 = markupExtension39.ProvideValue(xamlServiceProvider39);
			label22.Text = obj77;
			stackLayout6.Children.Add(label22);
			bindingExtension36.Mode = 1;
			staticResourceExtension14.Key = "SkipCyclesIntToPriorityStringConverter";
			IMarkupExtension markupExtension40 = staticResourceExtension14;
			XamlServiceProvider xamlServiceProvider40 = new XamlServiceProvider();
			Type typeFromHandle80 = typeof(IProvideValueTarget);
			object[] array41 = new object[0 + 6];
			array41[0] = bindingExtension36;
			array41[1] = picker2;
			array41[2] = stackLayout6;
			array41[3] = stackLayout8;
			array41[4] = scrollView;
			array41[5] = this;
			object obj78;
			xamlServiceProvider40.Add(typeFromHandle80, obj78 = new SimpleValueTargetProvider(array41, typeof(BindingExtension).GetRuntimeProperty("Converter"), nameScope));
			xamlServiceProvider40.Add(typeof(IReferenceProvider), obj78);
			Type typeFromHandle81 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver40 = new XmlNamespaceResolver();
			xmlNamespaceResolver40.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver40.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver40.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver40.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver40.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver40.Add("pids", "clr-namespace:CarScannerXamarinForms.OBD2.PIDS");
			xmlNamespaceResolver40.Add("syncfusion", "clr-namespace:Syncfusion.XForms.Buttons;assembly=Syncfusion.Buttons.XForms");
			xmlNamespaceResolver40.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider40.Add(typeFromHandle81, new XamlTypeResolver(xmlNamespaceResolver40, typeof(CustomPIDsEditorPage).GetTypeInfo().Assembly));
			xamlServiceProvider40.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(316, 29)));
			object obj79 = markupExtension40.ProvideValue(xamlServiceProvider40);
			bindingExtension36.Converter = obj79;
			bindingExtension36.Path = "SkipCycles";
			BindingBase bindingBase36 = bindingExtension36.ProvideValue(null);
			picker2.SetBinding(Picker.SelectedItemProperty, bindingBase36);
			picker2.SetValue(Picker.ItemsSourceProperty, array);
			stackLayout6.Children.Add(picker2);
			translate26.Text = "pid_override_Role";
			IMarkupExtension markupExtension41 = translate26;
			XamlServiceProvider xamlServiceProvider41 = new XamlServiceProvider();
			Type typeFromHandle82 = typeof(IProvideValueTarget);
			object[] array42 = new object[0 + 5];
			array42[0] = label23;
			array42[1] = stackLayout6;
			array42[2] = stackLayout8;
			array42[3] = scrollView;
			array42[4] = this;
			object obj80;
			xamlServiceProvider41.Add(typeFromHandle82, obj80 = new SimpleValueTargetProvider(array42, Label.TextProperty, nameScope));
			xamlServiceProvider41.Add(typeof(IReferenceProvider), obj80);
			Type typeFromHandle83 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver41 = new XmlNamespaceResolver();
			xmlNamespaceResolver41.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver41.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver41.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver41.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver41.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver41.Add("pids", "clr-namespace:CarScannerXamarinForms.OBD2.PIDS");
			xmlNamespaceResolver41.Add("syncfusion", "clr-namespace:Syncfusion.XForms.Buttons;assembly=Syncfusion.Buttons.XForms");
			xmlNamespaceResolver41.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider41.Add(typeFromHandle83, new XamlTypeResolver(xmlNamespaceResolver41, typeof(CustomPIDsEditorPage).GetTypeInfo().Assembly));
			xamlServiceProvider41.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(349, 28)));
			object obj81 = markupExtension41.ProvideValue(xamlServiceProvider41);
			label23.Text = obj81;
			stackLayout6.Children.Add(label23);
			bindingExtension37.Mode = 1;
			staticResourceExtension15.Key = "EnumToIntConverter";
			IMarkupExtension markupExtension42 = staticResourceExtension15;
			XamlServiceProvider xamlServiceProvider42 = new XamlServiceProvider();
			Type typeFromHandle84 = typeof(IProvideValueTarget);
			object[] array43 = new object[0 + 6];
			array43[0] = bindingExtension37;
			array43[1] = picker3;
			array43[2] = stackLayout6;
			array43[3] = stackLayout8;
			array43[4] = scrollView;
			array43[5] = this;
			object obj82;
			xamlServiceProvider42.Add(typeFromHandle84, obj82 = new SimpleValueTargetProvider(array43, typeof(BindingExtension).GetRuntimeProperty("Converter"), nameScope));
			xamlServiceProvider42.Add(typeof(IReferenceProvider), obj82);
			Type typeFromHandle85 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver42 = new XmlNamespaceResolver();
			xmlNamespaceResolver42.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver42.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver42.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver42.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver42.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver42.Add("pids", "clr-namespace:CarScannerXamarinForms.OBD2.PIDS");
			xmlNamespaceResolver42.Add("syncfusion", "clr-namespace:Syncfusion.XForms.Buttons;assembly=Syncfusion.Buttons.XForms");
			xmlNamespaceResolver42.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider42.Add(typeFromHandle85, new XamlTypeResolver(xmlNamespaceResolver42, typeof(CustomPIDsEditorPage).GetTypeInfo().Assembly));
			xamlServiceProvider42.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(350, 49)));
			object obj83 = markupExtension42.ProvideValue(xamlServiceProvider42);
			bindingExtension37.Converter = obj83;
			bindingExtension37.Path = "Role";
			BindingBase bindingBase37 = bindingExtension37.ProvideValue(null);
			picker3.SetBinding(Picker.SelectedIndexProperty, bindingBase37);
			stackLayout6.Children.Add(picker3);
			label24.SetValue(Label.FontAttributesProperty, new FontAttributesConverter().ConvertFromInvariantString("Bold"));
			dynamicResourceExtension2.Key = "BaseFontSize";
			IMarkupExtension<DynamicResource> markupExtension43 = dynamicResourceExtension2;
			XamlServiceProvider xamlServiceProvider43 = new XamlServiceProvider();
			Type typeFromHandle86 = typeof(IProvideValueTarget);
			object[] array44 = new object[0 + 5];
			array44[0] = label24;
			array44[1] = stackLayout6;
			array44[2] = stackLayout8;
			array44[3] = scrollView;
			array44[4] = this;
			object obj84;
			xamlServiceProvider43.Add(typeFromHandle86, obj84 = new SimpleValueTargetProvider(array44, Label.FontSizeProperty, nameScope));
			xamlServiceProvider43.Add(typeof(IReferenceProvider), obj84);
			Type typeFromHandle87 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver43 = new XmlNamespaceResolver();
			xmlNamespaceResolver43.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver43.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver43.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver43.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver43.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver43.Add("pids", "clr-namespace:CarScannerXamarinForms.OBD2.PIDS");
			xmlNamespaceResolver43.Add("syncfusion", "clr-namespace:Syncfusion.XForms.Buttons;assembly=Syncfusion.Buttons.XForms");
			xmlNamespaceResolver43.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider43.Add(typeFromHandle87, new XamlTypeResolver(xmlNamespaceResolver43, typeof(CustomPIDsEditorPage).GetTypeInfo().Assembly));
			xamlServiceProvider43.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(353, 25)));
			DynamicResource dynamicResource2 = markupExtension43.ProvideValue(xamlServiceProvider43);
			label24.SetDynamicResource(Label.FontSizeProperty, dynamicResource2.Key);
			translate27.Text = "settings_DontTouchRoles";
			IMarkupExtension markupExtension44 = translate27;
			XamlServiceProvider xamlServiceProvider44 = new XamlServiceProvider();
			Type typeFromHandle88 = typeof(IProvideValueTarget);
			object[] array45 = new object[0 + 5];
			array45[0] = label24;
			array45[1] = stackLayout6;
			array45[2] = stackLayout8;
			array45[3] = scrollView;
			array45[4] = this;
			object obj85;
			xamlServiceProvider44.Add(typeFromHandle88, obj85 = new SimpleValueTargetProvider(array45, Label.TextProperty, nameScope));
			xamlServiceProvider44.Add(typeof(IReferenceProvider), obj85);
			Type typeFromHandle89 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver44 = new XmlNamespaceResolver();
			xmlNamespaceResolver44.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver44.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver44.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver44.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver44.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver44.Add("pids", "clr-namespace:CarScannerXamarinForms.OBD2.PIDS");
			xmlNamespaceResolver44.Add("syncfusion", "clr-namespace:Syncfusion.XForms.Buttons;assembly=Syncfusion.Buttons.XForms");
			xmlNamespaceResolver44.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider44.Add(typeFromHandle89, new XamlTypeResolver(xmlNamespaceResolver44, typeof(CustomPIDsEditorPage).GetTypeInfo().Assembly));
			xamlServiceProvider44.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(354, 25)));
			object obj86 = markupExtension44.ProvideValue(xamlServiceProvider44);
			label24.Text = obj86;
			dynamicResourceExtension3.Key = "RedTextColor";
			IMarkupExtension<DynamicResource> markupExtension45 = dynamicResourceExtension3;
			XamlServiceProvider xamlServiceProvider45 = new XamlServiceProvider();
			Type typeFromHandle90 = typeof(IProvideValueTarget);
			object[] array46 = new object[0 + 5];
			array46[0] = label24;
			array46[1] = stackLayout6;
			array46[2] = stackLayout8;
			array46[3] = scrollView;
			array46[4] = this;
			object obj87;
			xamlServiceProvider45.Add(typeFromHandle90, obj87 = new SimpleValueTargetProvider(array46, Label.TextColorProperty, nameScope));
			xamlServiceProvider45.Add(typeof(IReferenceProvider), obj87);
			Type typeFromHandle91 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver45 = new XmlNamespaceResolver();
			xmlNamespaceResolver45.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver45.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver45.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver45.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver45.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver45.Add("pids", "clr-namespace:CarScannerXamarinForms.OBD2.PIDS");
			xmlNamespaceResolver45.Add("syncfusion", "clr-namespace:Syncfusion.XForms.Buttons;assembly=Syncfusion.Buttons.XForms");
			xmlNamespaceResolver45.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider45.Add(typeFromHandle91, new XamlTypeResolver(xmlNamespaceResolver45, typeof(CustomPIDsEditorPage).GetTypeInfo().Assembly));
			xamlServiceProvider45.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(355, 25)));
			DynamicResource dynamicResource3 = markupExtension45.ProvideValue(xamlServiceProvider45);
			label24.SetDynamicResource(Label.TextColorProperty, dynamicResource3.Key);
			stackLayout6.Children.Add(label24);
			stackLayout8.Children.Add(stackLayout6);
			translate28.Text = "CustomPidEditor_tbBeforeCommands.Text";
			IMarkupExtension markupExtension46 = translate28;
			XamlServiceProvider xamlServiceProvider46 = new XamlServiceProvider();
			Type typeFromHandle92 = typeof(IProvideValueTarget);
			object[] array47 = new object[0 + 4];
			array47[0] = label25;
			array47[1] = stackLayout8;
			array47[2] = scrollView;
			array47[3] = this;
			object obj88;
			xamlServiceProvider46.Add(typeFromHandle92, obj88 = new SimpleValueTargetProvider(array47, Label.TextProperty, nameScope));
			xamlServiceProvider46.Add(typeof(IReferenceProvider), obj88);
			Type typeFromHandle93 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver46 = new XmlNamespaceResolver();
			xmlNamespaceResolver46.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver46.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver46.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver46.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver46.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver46.Add("pids", "clr-namespace:CarScannerXamarinForms.OBD2.PIDS");
			xmlNamespaceResolver46.Add("syncfusion", "clr-namespace:Syncfusion.XForms.Buttons;assembly=Syncfusion.Buttons.XForms");
			xmlNamespaceResolver46.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider46.Add(typeFromHandle93, new XamlTypeResolver(xmlNamespaceResolver46, typeof(CustomPIDsEditorPage).GetTypeInfo().Assembly));
			xamlServiceProvider46.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(358, 24)));
			object obj89 = markupExtension46.ProvideValue(xamlServiceProvider46);
			label25.Text = obj89;
			stackLayout8.Children.Add(label25);
			bindingExtension38.Mode = 1;
			bindingExtension38.Path = "BeforeCommand";
			BindingBase bindingBase38 = bindingExtension38.ProvideValue(null);
			entry7.SetBinding(Entry.TextProperty, bindingBase38);
			stackLayout8.Children.Add(entry7);
			translate29.Text = "CustomPidEditor_tbAfterCommands.Text";
			IMarkupExtension markupExtension47 = translate29;
			XamlServiceProvider xamlServiceProvider47 = new XamlServiceProvider();
			Type typeFromHandle94 = typeof(IProvideValueTarget);
			object[] array48 = new object[0 + 4];
			array48[0] = label26;
			array48[1] = stackLayout8;
			array48[2] = scrollView;
			array48[3] = this;
			object obj90;
			xamlServiceProvider47.Add(typeFromHandle94, obj90 = new SimpleValueTargetProvider(array48, Label.TextProperty, nameScope));
			xamlServiceProvider47.Add(typeof(IReferenceProvider), obj90);
			Type typeFromHandle95 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver47 = new XmlNamespaceResolver();
			xmlNamespaceResolver47.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver47.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver47.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver47.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver47.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver47.Add("pids", "clr-namespace:CarScannerXamarinForms.OBD2.PIDS");
			xmlNamespaceResolver47.Add("syncfusion", "clr-namespace:Syncfusion.XForms.Buttons;assembly=Syncfusion.Buttons.XForms");
			xmlNamespaceResolver47.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider47.Add(typeFromHandle95, new XamlTypeResolver(xmlNamespaceResolver47, typeof(CustomPIDsEditorPage).GetTypeInfo().Assembly));
			xamlServiceProvider47.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(360, 24)));
			object obj91 = markupExtension47.ProvideValue(xamlServiceProvider47);
			label26.Text = obj91;
			stackLayout8.Children.Add(label26);
			bindingExtension39.Mode = 1;
			bindingExtension39.Path = "AfterCommand";
			BindingBase bindingBase39 = bindingExtension39.ProvideValue(null);
			entry8.SetBinding(Entry.TextProperty, bindingBase39);
			stackLayout8.Children.Add(entry8);
			label27.SetValue(Label.TextProperty, "Value to text conversions (1=OK)");
			stackLayout8.Children.Add(label27);
			editor.SetValue(Editor.AutoSizeProperty, 1);
			editor.SetValue(InputView.KeyboardProperty, new KeyboardTypeConverter().ConvertFromInvariantString("Plain"));
			editor.SetValue(InputView.MaxLengthProperty, 180);
			bindingExtension40.Mode = 1;
			bindingExtension40.Path = "TextValueVariants";
			BindingBase bindingBase40 = bindingExtension40.ProvideValue(null);
			editor.SetBinding(Editor.TextProperty, bindingBase40);
			stackLayout8.Children.Add(editor);
			button3.Clicked += this.btnUnitsOverride_Clicked;
			button3.SetValue(Button.TextProperty, "Units override");
			stackLayout8.Children.Add(button3);
			stackLayout7.SetValue(StackLayout.OrientationProperty, 0);
			button4.Clicked += this.BtnTest_Clicked;
			button4.SetValue(Button.TextProperty, "Test");
			stackLayout7.Children.Add(button4);
			stackLayout7.Children.Add(label28);
			stackLayout8.Children.Add(stackLayout7);
			scrollView.Content = stackLayout8;
			this.SetValue(ContentPage.ContentProperty, scrollView);
		}

		// Token: 0x0600152E RID: 5422 RVA: 0x0008F6B1 File Offset: 0x0008D8B1
		[CompilerGenerated]
		private void <BtnTest_Clicked>b__2_0(OBDRequest request2, string data)
		{
			Device.BeginInvokeOnMainThread(delegate
			{
				this.lbTestResult.FormattedText.Spans.Add(new Span
				{
					Text = "ELM:\n",
					FontAttributes = 1
				});
				this.lbTestResult.FormattedText.Spans.Add(new Span
				{
					Text = data.Replace('\r', '\n') + "\n"
				});
			});
		}

		// Token: 0x0600152F RID: 5423 RVA: 0x0008F6D8 File Offset: 0x0008D8D8
		private void __InitComponentRuntime()
		{
			Extensions.LoadFromXaml<CustomPIDsEditorPage>(this, typeof(CustomPIDsEditorPage));
			this.scrollView = NameScopeExtensions.FindByName<ScrollView>(this, "scrollView");
			this.panelDecodeType = NameScopeExtensions.FindByName<StackLayout>(this, "panelDecodeType");
			this.panelFormula = NameScopeExtensions.FindByName<StackLayout>(this, "panelFormula");
			this.panelByteSet = NameScopeExtensions.FindByName<Grid>(this, "panelByteSet");
			this.panelBitValue = NameScopeExtensions.FindByName<Grid>(this, "panelBitValue");
			this.panelVagGroup = NameScopeExtensions.FindByName<StackLayout>(this, "panelVagGroup");
			this.vagUnitEntry = NameScopeExtensions.FindByName<Entry>(this, "vagUnitEntry");
			this.vagGroupEntry = NameScopeExtensions.FindByName<NumericEntryV3>(this, "vagGroupEntry");
			this.vagItemInGroupEntry = NameScopeExtensions.FindByName<NumericEntryV3>(this, "vagItemInGroupEntry");
			this.btnApplyVagGroup = NameScopeExtensions.FindByName<Button>(this, "btnApplyVagGroup");
			this.pickerRole = NameScopeExtensions.FindByName<Picker>(this, "pickerRole");
			this.btnUnitsOverride = NameScopeExtensions.FindByName<Button>(this, "btnUnitsOverride");
			this.panelTest = NameScopeExtensions.FindByName<StackLayout>(this, "panelTest");
			this.btnTestPid = NameScopeExtensions.FindByName<Button>(this, "btnTestPid");
			this.lbTestResult = NameScopeExtensions.FindByName<Label>(this, "lbTestResult");
		}

		// Token: 0x04000598 RID: 1432
		[CompilerGenerated]
		private CustomPID <CPid>k__BackingField;

		// Token: 0x04000599 RID: 1433
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private ScrollView scrollView;

		// Token: 0x0400059A RID: 1434
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private StackLayout panelDecodeType;

		// Token: 0x0400059B RID: 1435
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private StackLayout panelFormula;

		// Token: 0x0400059C RID: 1436
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private Grid panelByteSet;

		// Token: 0x0400059D RID: 1437
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private Grid panelBitValue;

		// Token: 0x0400059E RID: 1438
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private StackLayout panelVagGroup;

		// Token: 0x0400059F RID: 1439
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private Entry vagUnitEntry;

		// Token: 0x040005A0 RID: 1440
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private NumericEntryV3 vagGroupEntry;

		// Token: 0x040005A1 RID: 1441
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private NumericEntryV3 vagItemInGroupEntry;

		// Token: 0x040005A2 RID: 1442
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private Button btnApplyVagGroup;

		// Token: 0x040005A3 RID: 1443
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private Picker pickerRole;

		// Token: 0x040005A4 RID: 1444
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private Button btnUnitsOverride;

		// Token: 0x040005A5 RID: 1445
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private StackLayout panelTest;

		// Token: 0x040005A6 RID: 1446
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private Button btnTestPid;

		// Token: 0x040005A7 RID: 1447
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private Label lbTestResult;

		// Token: 0x02000163 RID: 355
		[CompilerGenerated]
		[Serializable]
		private sealed class <>c
		{
			// Token: 0x06001530 RID: 5424 RVA: 0x0008F7F5 File Offset: 0x0008D9F5
			// Note: this type is marked as 'beforefieldinit'.
			static <>c()
			{
			}

			// Token: 0x06001531 RID: 5425 RVA: 0x00002050 File Offset: 0x00000250
			public <>c()
			{
			}

			// Token: 0x06001532 RID: 5426 RVA: 0x00066340 File Offset: 0x00064540
			internal bool <BtnInsertPidInFormula_Clicked>b__1_0(PID x)
			{
				return x is IPIDFloatValue;
			}

			// Token: 0x040005A8 RID: 1448
			public static readonly CustomPIDsEditorPage.<>c <>9 = new CustomPIDsEditorPage.<>c();

			// Token: 0x040005A9 RID: 1449
			public static Func<PID, bool> <>9__1_0;
		}

		// Token: 0x02000164 RID: 356
		[CompilerGenerated]
		private sealed class <>c__DisplayClass2_0
		{
			// Token: 0x06001533 RID: 5427 RVA: 0x00002050 File Offset: 0x00000250
			public <>c__DisplayClass2_0()
			{
			}

			// Token: 0x06001534 RID: 5428 RVA: 0x0008F804 File Offset: 0x0008DA04
			internal void <BtnTest_Clicked>b__1(OBDRequest request2, byte[] data, bool decodeResult, string responseHeader)
			{
				if (data != null && data.Length != 0)
				{
					CustomPIDsEditorPage.<>c__DisplayClass2_2 CS$<>8__locals1 = new CustomPIDsEditorPage.<>c__DisplayClass2_2();
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
						CustomPIDsEditorPage.<>c__DisplayClass2_2.<<BtnTest_Clicked>b__4>d <<BtnTest_Clicked>b__4>d;
						<<BtnTest_Clicked>b__4>d.<>t__builder = AsyncVoidMethodBuilder.Create();
						<<BtnTest_Clicked>b__4>d.<>4__this = CS$<>8__locals1;
						<<BtnTest_Clicked>b__4>d.<>1__state = -1;
						<<BtnTest_Clicked>b__4>d.<>t__builder.Start<CustomPIDsEditorPage.<>c__DisplayClass2_2.<<BtnTest_Clicked>b__4>d>(ref <<BtnTest_Clicked>b__4>d);
					});
				}
			}

			// Token: 0x06001535 RID: 5429 RVA: 0x0008F938 File Offset: 0x0008DB38
			internal void <BtnTest_Clicked>b__2(object senderModel, PropertyChangedEventArgs propertyArgs)
			{
				if (propertyArgs.PropertyName == "FloatValue")
				{
					Action action;
					if ((action = this.<>9__5) == null)
					{
						action = (this.<>9__5 = delegate
						{
							try
							{
								this.<>4__this.lbTestResult.FormattedText.Spans.Add(new Span
								{
									Text = "Calculation result = ",
									FontAttributes = 1
								});
								this.<>4__this.lbTestResult.FormattedText.Spans.Add(new Span
								{
									Text = this.model.FloatValue.ToString() + " " + this.model.Units + "\n"
								});
							}
							catch (Exception)
							{
							}
						});
					}
					Device.BeginInvokeOnMainThread(action);
				}
			}

			// Token: 0x06001536 RID: 5430 RVA: 0x0008F97C File Offset: 0x0008DB7C
			internal void <BtnTest_Clicked>b__5()
			{
				try
				{
					this.<>4__this.lbTestResult.FormattedText.Spans.Add(new Span
					{
						Text = "Calculation result = ",
						FontAttributes = 1
					});
					this.<>4__this.lbTestResult.FormattedText.Spans.Add(new Span
					{
						Text = this.model.FloatValue.ToString() + " " + this.model.Units + "\n"
					});
				}
				catch (Exception)
				{
				}
			}

			// Token: 0x040005AA RID: 1450
			public LiveDataPIDModel model;

			// Token: 0x040005AB RID: 1451
			public CustomPIDsEditorPage <>4__this;

			// Token: 0x040005AC RID: 1452
			public Action <>9__5;
		}

		// Token: 0x02000165 RID: 357
		[CompilerGenerated]
		private sealed class <>c__DisplayClass2_1
		{
			// Token: 0x06001537 RID: 5431 RVA: 0x00002050 File Offset: 0x00000250
			public <>c__DisplayClass2_1()
			{
			}

			// Token: 0x06001538 RID: 5432 RVA: 0x0008FA24 File Offset: 0x0008DC24
			internal void <BtnTest_Clicked>b__3()
			{
				this.<>4__this.lbTestResult.FormattedText.Spans.Add(new Span
				{
					Text = "ELM:\n",
					FontAttributes = 1
				});
				this.<>4__this.lbTestResult.FormattedText.Spans.Add(new Span
				{
					Text = this.data.Replace('\r', '\n') + "\n"
				});
			}

			// Token: 0x040005AD RID: 1453
			public string data;

			// Token: 0x040005AE RID: 1454
			public CustomPIDsEditorPage <>4__this;
		}

		// Token: 0x02000166 RID: 358
		[CompilerGenerated]
		private sealed class <>c__DisplayClass2_2
		{
			// Token: 0x06001539 RID: 5433 RVA: 0x00002050 File Offset: 0x00000250
			public <>c__DisplayClass2_2()
			{
			}

			// Token: 0x0600153A RID: 5434 RVA: 0x0008FAA0 File Offset: 0x0008DCA0
			internal async void <BtnTest_Clicked>b__4()
			{
				try
				{
					this.CS$<>8__locals1.<>4__this.lbTestResult.FormattedText.Spans.Add(new Span
					{
						Text = "Data for calculation:\n",
						FontAttributes = 1
					});
					this.CS$<>8__locals1.<>4__this.lbTestResult.FormattedText.Spans.Add(new Span
					{
						Text = this.sb.ToString() + "\n"
					});
					this.CS$<>8__locals1.<>4__this.scrollView.ScrollToAsync(this.CS$<>8__locals1.<>4__this.lbTestResult, 3, false);
					await Task.Delay(200);
					this.CS$<>8__locals1.model.Unsubscribe();
				}
				catch (Exception)
				{
				}
			}

			// Token: 0x040005AF RID: 1455
			public StringBuilder sb;

			// Token: 0x040005B0 RID: 1456
			public CustomPIDsEditorPage.<>c__DisplayClass2_0 CS$<>8__locals1;

			// Token: 0x02000167 RID: 359
			[StructLayout(LayoutKind.Auto)]
			private struct <<BtnTest_Clicked>b__4>d : IAsyncStateMachine
			{
				// Token: 0x0600153B RID: 5435 RVA: 0x0008FAD8 File Offset: 0x0008DCD8
				void IAsyncStateMachine.MoveNext()
				{
					int num2;
					int num = num2;
					CustomPIDsEditorPage.<>c__DisplayClass2_2 CS$<>8__locals1 = this;
					try
					{
						try
						{
							TaskAwaiter taskAwaiter;
							if (num != 0)
							{
								CS$<>8__locals1.CS$<>8__locals1.<>4__this.lbTestResult.FormattedText.Spans.Add(new Span
								{
									Text = "Data for calculation:\n",
									FontAttributes = 1
								});
								CS$<>8__locals1.CS$<>8__locals1.<>4__this.lbTestResult.FormattedText.Spans.Add(new Span
								{
									Text = CS$<>8__locals1.sb.ToString() + "\n"
								});
								CS$<>8__locals1.CS$<>8__locals1.<>4__this.scrollView.ScrollToAsync(CS$<>8__locals1.CS$<>8__locals1.<>4__this.lbTestResult, 3, false);
								taskAwaiter = Task.Delay(200).GetAwaiter();
								if (!taskAwaiter.IsCompleted)
								{
									num2 = 0;
									TaskAwaiter taskAwaiter2 = taskAwaiter;
									this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, CustomPIDsEditorPage.<>c__DisplayClass2_2.<<BtnTest_Clicked>b__4>d>(ref taskAwaiter, ref this);
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
							CS$<>8__locals1.CS$<>8__locals1.model.Unsubscribe();
						}
						catch (Exception)
						{
						}
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

				// Token: 0x0600153C RID: 5436 RVA: 0x0008FC6C File Offset: 0x0008DE6C
				[DebuggerHidden]
				void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
				{
					this.<>t__builder.SetStateMachine(stateMachine);
				}

				// Token: 0x040005B1 RID: 1457
				public int <>1__state;

				// Token: 0x040005B2 RID: 1458
				public AsyncVoidMethodBuilder <>t__builder;

				// Token: 0x040005B3 RID: 1459
				public CustomPIDsEditorPage.<>c__DisplayClass2_2 <>4__this;

				// Token: 0x040005B4 RID: 1460
				private TaskAwaiter <>u__1;
			}
		}

		// Token: 0x02000168 RID: 360
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <BtnInsertPidInFormula_Clicked>d__1 : IAsyncStateMachine
		{
			// Token: 0x0600153D RID: 5437 RVA: 0x0008FC7C File Offset: 0x0008DE7C
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				CustomPIDsEditorPage customPIDsEditorPage = this;
				try
				{
					TaskAwaiter<string> taskAwaiter;
					TaskAwaiter<IPID> taskAwaiter3;
					if (num != 0)
					{
						if (num == 1)
						{
							TaskAwaiter<string> taskAwaiter2;
							taskAwaiter = taskAwaiter2;
							taskAwaiter2 = default(TaskAwaiter<string>);
							num2 = -1;
							goto IL_015B;
						}
						taskAwaiter3 = PIDSelector.SelectPIDAsync(null, (PID x) => x is IPIDFloatValue).GetAwaiter();
						if (!taskAwaiter3.IsCompleted)
						{
							num2 = 0;
							TaskAwaiter<IPID> taskAwaiter4 = taskAwaiter3;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<IPID>, CustomPIDsEditorPage.<BtnInsertPidInFormula_Clicked>d__1>(ref taskAwaiter3, ref this);
							return;
						}
					}
					else
					{
						TaskAwaiter<IPID> taskAwaiter4;
						taskAwaiter3 = taskAwaiter4;
						taskAwaiter4 = default(TaskAwaiter<IPID>);
						num2 = -1;
					}
					IPID result = taskAwaiter3.GetResult();
					if (result == null)
					{
						goto IL_01D4;
					}
					string @string = Translate.GetString("ios_Cancel");
					byName = "{" + result.Name + "}";
					byID = "PID(" + result.Id.ToString() + ")";
					taskAwaiter = customPIDsEditorPage.DisplayActionSheetCustom("Insert PID by ID or by name:", @string, null, new string[] { byName, byID }).GetAwaiter();
					if (!taskAwaiter.IsCompleted)
					{
						num2 = 1;
						TaskAwaiter<string> taskAwaiter2 = taskAwaiter;
						this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<string>, CustomPIDsEditorPage.<BtnInsertPidInFormula_Clicked>d__1>(ref taskAwaiter, ref this);
						return;
					}
					IL_015B:
					string result2 = taskAwaiter.GetResult();
					if (result2 == byName)
					{
						customPIDsEditorPage.CPid.Formula = customPIDsEditorPage.CPid.Formula + byName;
					}
					else if (result2 == byID)
					{
						customPIDsEditorPage.CPid.Formula = customPIDsEditorPage.CPid.Formula + byID;
					}
					byName = null;
					byID = null;
					IL_01D4:;
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

			// Token: 0x0600153E RID: 5438 RVA: 0x0008FEA8 File Offset: 0x0008E0A8
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x040005B5 RID: 1461
			public int <>1__state;

			// Token: 0x040005B6 RID: 1462
			public AsyncVoidMethodBuilder <>t__builder;

			// Token: 0x040005B7 RID: 1463
			public CustomPIDsEditorPage <>4__this;

			// Token: 0x040005B8 RID: 1464
			private TaskAwaiter<IPID> <>u__1;

			// Token: 0x040005B9 RID: 1465
			private string <byName>5__2;

			// Token: 0x040005BA RID: 1466
			private string <byID>5__3;

			// Token: 0x040005BB RID: 1467
			private TaskAwaiter<string> <>u__2;
		}

		// Token: 0x02000169 RID: 361
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <Handle_Disappearing>d__11 : IAsyncStateMachine
		{
			// Token: 0x0600153F RID: 5439 RVA: 0x0008FEB8 File Offset: 0x0008E0B8
			void IAsyncStateMachine.MoveNext()
			{
				try
				{
					CustomPIDViewModel.CurrentCustom.Save();
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

			// Token: 0x06001540 RID: 5440 RVA: 0x0008FF0C File Offset: 0x0008E10C
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x040005BC RID: 1468
			public int <>1__state;

			// Token: 0x040005BD RID: 1469
			public AsyncVoidMethodBuilder <>t__builder;
		}

		// Token: 0x0200016A RID: 362
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <btnUnitsOverride_Clicked>d__13 : IAsyncStateMachine
		{
			// Token: 0x06001541 RID: 5441 RVA: 0x0008FF1C File Offset: 0x0008E11C
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				CustomPIDsEditorPage customPIDsEditorPage = this;
				try
				{
					TaskAwaiter taskAwaiter;
					TaskAwaiter taskAwaiter2;
					if (num != 0)
					{
						if (num != 1)
						{
							if (App.UseLegacyUI)
							{
								if (!PlatformHelper.IsiOS)
								{
									goto IL_0104;
								}
								SettingsPIDOverridePage settingsPIDOverridePage = new SettingsPIDOverridePage(customPIDsEditorPage.CPid);
								taskAwaiter = customPIDsEditorPage.Navigation.PushAsync(settingsPIDOverridePage).GetAwaiter();
								if (!taskAwaiter.IsCompleted)
								{
									num2 = 0;
									taskAwaiter2 = taskAwaiter;
									this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, CustomPIDsEditorPage.<btnUnitsOverride_Clicked>d__13>(ref taskAwaiter, ref this);
									return;
								}
								goto IL_008F;
							}
							else
							{
								SettingsPIDOverrideEditorPageV3 settingsPIDOverrideEditorPageV = new SettingsPIDOverrideEditorPageV3(customPIDsEditorPage.CPid);
								taskAwaiter = customPIDsEditorPage.Navigation.PushAsync(settingsPIDOverrideEditorPageV).GetAwaiter();
								if (!taskAwaiter.IsCompleted)
								{
									num2 = 1;
									taskAwaiter2 = taskAwaiter;
									this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, CustomPIDsEditorPage.<btnUnitsOverride_Clicked>d__13>(ref taskAwaiter, ref this);
									return;
								}
							}
						}
						else
						{
							taskAwaiter = taskAwaiter2;
							taskAwaiter2 = default(TaskAwaiter);
							num2 = -1;
						}
						taskAwaiter.GetResult();
						goto IL_0104;
					}
					taskAwaiter = taskAwaiter2;
					taskAwaiter2 = default(TaskAwaiter);
					num2 = -1;
					IL_008F:
					taskAwaiter.GetResult();
					IL_0104:;
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

			// Token: 0x06001542 RID: 5442 RVA: 0x0009006C File Offset: 0x0008E26C
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x040005BE RID: 1470
			public int <>1__state;

			// Token: 0x040005BF RID: 1471
			public AsyncVoidMethodBuilder <>t__builder;

			// Token: 0x040005C0 RID: 1472
			public CustomPIDsEditorPage <>4__this;

			// Token: 0x040005C1 RID: 1473
			private TaskAwaiter <>u__1;
		}
	}
}
