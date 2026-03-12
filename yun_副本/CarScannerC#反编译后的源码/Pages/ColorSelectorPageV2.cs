using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Input;
using CarScannerXamarinForms.Coding.Models;
using CarScannerXamarinForms.Common.XAMLConverters;
using CarScannerXamarinForms.Settings;
using DLToolkit.Forms.Controls;
using Spillman.Xamarin.Forms.ColorPicker;
using Xam.Plugin.SimpleColorPicker;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Internals;
using Xamarin.Forms.PlatformConfiguration.iOSSpecific;
using Xamarin.Forms.Xaml;
using Xamarin.Forms.Xaml.Diagnostics;
using Xamarin.Forms.Xaml.Internals;

namespace CarScannerXamarinForms.Pages
{
	// Token: 0x0200060E RID: 1550
	[XamlCompilation(2)]
	[XamlFilePath("Pages\\ColorSelectorPageV2.xaml")]
	public class ColorSelectorPageV2 : ContentPage
	{
		// Token: 0x0600369B RID: 13979 RVA: 0x002857BC File Offset: 0x002839BC
		public ColorSelectorPageV2(Color initialColor, Action<Color> ColorSelected)
		{
			ColorSelectorPageV2.<>c__DisplayClass0_0 CS$<>8__locals1 = new ColorSelectorPageV2.<>c__DisplayClass0_0();
			CS$<>8__locals1.ColorSelected = ColorSelected;
			base..ctor();
			CS$<>8__locals1.<>4__this = this;
			this.InitializeComponent();
			this.CreateColors();
			this.ItemTappedCommand = new Command(delegate(object param)
			{
				ColorSelectorPageV2.<>c__DisplayClass0_0.<<-ctor>b__0>d <<-ctor>b__0>d;
				<<-ctor>b__0>d.<>t__builder = AsyncVoidMethodBuilder.Create();
				<<-ctor>b__0>d.<>4__this = CS$<>8__locals1;
				<<-ctor>b__0>d.param = param;
				<<-ctor>b__0>d.<>1__state = -1;
				<<-ctor>b__0>d.<>t__builder.Start<ColorSelectorPageV2.<>c__DisplayClass0_0.<<-ctor>b__0>d>(ref <<-ctor>b__0>d);
			});
			this.SelectedColor = new ColorObject
			{
				Color = initialColor
			};
			this.lvColors.BindingContext = this;
			this.stack.BindingContext = this.SelectedColor;
			this.ColorModel.Color = initialColor;
			this.ColorModel.IsAlphaEnabled = false;
			this.ColorModel.UpdateHex();
			this.colorMixer.BindingContext = this.ColorModel;
			this.spillManCP.BindingContext = this.ColorModel;
			this.colorMixer.EditAlpha = false;
			this.ColorSelected = CS$<>8__locals1.ColorSelected;
		}

		// Token: 0x0600369C RID: 13980 RVA: 0x002858A0 File Offset: 0x00283AA0
		private void ColorSelectorPageV2_SizeChanged(object sender, EventArgs e)
		{
			if (DeviceDisplay.MainDisplayInfo.Orientation == 2)
			{
				this.lvColors.FlowColumnCount = new int?(8);
				return;
			}
			this.lvColors.FlowColumnCount = new int?(5);
		}

		// Token: 0x1700137B RID: 4987
		// (get) Token: 0x0600369D RID: 13981 RVA: 0x002858E0 File Offset: 0x00283AE0
		// (set) Token: 0x0600369E RID: 13982 RVA: 0x002858E8 File Offset: 0x00283AE8
		public ICommand ItemTappedCommand
		{
			[CompilerGenerated]
			get
			{
				return this.<ItemTappedCommand>k__BackingField;
			}
			[CompilerGenerated]
			private set
			{
				this.<ItemTappedCommand>k__BackingField = value;
			}
		}

		// Token: 0x0600369F RID: 13983 RVA: 0x002858F4 File Offset: 0x00283AF4
		private void AddToRecent(Color color)
		{
			List<Color> list = this.LoadRecentColors();
			int num = list.IndexOf(color);
			if (num >= 0)
			{
				list.RemoveAt(num);
			}
			list.Insert(0, color);
			if (list.Count > 16)
			{
				list = list.Take(16).ToList<Color>();
			}
			StringBuilder stringBuilder = new StringBuilder(list.Count * 2);
			foreach (Color color2 in list)
			{
				stringBuilder.Append(color2.ToHex());
				stringBuilder.Append(';');
			}
			SharedSettings.Current.RecentColors = stringBuilder.ToString();
		}

		// Token: 0x060036A0 RID: 13984 RVA: 0x002859AC File Offset: 0x00283BAC
		private List<Color> LoadRecentColors()
		{
			string recentColors = SharedSettings.Current.RecentColors;
			if (string.IsNullOrEmpty(recentColors))
			{
				return new List<Color>
				{
					Color.Black,
					Color.White,
					Color.Red,
					Color.Green,
					Color.Blue,
					Color.Yellow,
					Color.Purple,
					Color.Cyan,
					Color.Brown,
					Color.DarkRed,
					Color.DarkBlue,
					Color.DarkGreen,
					Color.Orange,
					Color.Gray,
					Color.LimeGreen,
					Color.Indigo
				};
			}
			List<Color> list = new List<Color>(20);
			try
			{
				foreach (string text in recentColors.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries))
				{
					try
					{
						Color color = Color.FromHex(text);
						list.Add(color);
					}
					catch
					{
					}
				}
			}
			catch (Exception)
			{
			}
			return list;
		}

		// Token: 0x060036A1 RID: 13985 RVA: 0x00285AEC File Offset: 0x00283CEC
		private void CreateColors()
		{
			int num = 44;
			this._Colors = new List<ColorItem>(256 / num * 3 + 12);
			for (int i = 0; i <= 256; i += num)
			{
				for (int j = 0; j <= 256; j += num)
				{
					for (int k = 0; k <= 256; k += num)
					{
						double num2 = (double)i / 256.0;
						double num3 = (double)j / 256.0;
						double num4 = (double)k / 256.0;
						Color color;
						color..ctor(num2, num3, num4);
						ColorItem colorItem = new ColorItem
						{
							Color = color
						};
						this._Colors.Add(colorItem);
					}
				}
			}
			this._Colors.Sort((ColorItem x, ColorItem y) => ColorSelectorPageV2.CompareColors(x, y));
			this._Colors.Insert(0, new ColorItem
			{
				Color = Color.Transparent
			});
			List<ColorItem> list = (from x in this.LoadRecentColors()
				select new ColorItem
				{
					Color = x
				}).ToList<ColorItem>();
			this._Colors = list.Concat(this._Colors).ToList<ColorItem>();
		}

		// Token: 0x060036A2 RID: 13986 RVA: 0x00285C2C File Offset: 0x00283E2C
		private static int CompareColors(object x, object y)
		{
			Color color = ((ColorItem)x).Color;
			Color color2 = ((ColorItem)y).Color;
			double saturation = color.Saturation;
			double saturation2 = color2.Saturation;
			double hue = color.Hue;
			double hue2 = color2.Hue;
			double brightness = ColorSelectorPageV2.GetBrightness(color);
			double brightness2 = ColorSelectorPageV2.GetBrightness(color2);
			if (hue < hue2)
			{
				return -1;
			}
			if (hue > hue2)
			{
				return 1;
			}
			if (saturation < saturation2)
			{
				return -1;
			}
			if (saturation > saturation2)
			{
				return 1;
			}
			if (brightness < brightness2)
			{
				return -1;
			}
			if (brightness > brightness2)
			{
				return 1;
			}
			return 0;
		}

		// Token: 0x060036A3 RID: 13987 RVA: 0x00285CB0 File Offset: 0x00283EB0
		private static double GetBrightness(Color c)
		{
			double num = c.R / 255.0;
			double num2 = c.G / 255.0;
			double num3 = c.B / 255.0;
			double num4 = num;
			double num5 = num;
			if (num2 > num4)
			{
				num4 = num2;
			}
			if (num3 > num4)
			{
				num4 = num3;
			}
			if (num2 < num5)
			{
				num5 = num2;
			}
			if (num3 < num5)
			{
				num5 = num3;
			}
			return (num4 + num5) / 2.0;
		}

		// Token: 0x1700137C RID: 4988
		// (get) Token: 0x060036A4 RID: 13988 RVA: 0x00285D1D File Offset: 0x00283F1D
		public List<ColorItem> Colors
		{
			get
			{
				return this._Colors;
			}
		}

		// Token: 0x060036A5 RID: 13989 RVA: 0x00285D28 File Offset: 0x00283F28
		private async void btnCopy_Clicked(object sender, EventArgs e)
		{
			this.ColorModel.UpdateHex();
			try
			{
				await Clipboard.SetTextAsync(this.ColorModel.Hex);
			}
			catch (Exception)
			{
			}
		}

		// Token: 0x060036A6 RID: 13990 RVA: 0x00285D60 File Offset: 0x00283F60
		private async void btnPaste_Clicked(object sender, EventArgs e)
		{
			try
			{
				string text = await Clipboard.GetTextAsync();
				if (!string.IsNullOrEmpty(text))
				{
					try
					{
						this.ColorModel.Hex = text;
					}
					catch (Exception)
					{
					}
				}
			}
			catch (Exception)
			{
			}
			this.ColorModel.UpdateHex();
		}

		// Token: 0x060036A7 RID: 13991 RVA: 0x00285D98 File Offset: 0x00283F98
		private async void btnOK_Clicked(object sender, EventArgs e)
		{
			this.btnOK.IsEnabled = false;
			this.AddToRecent(this.ColorModel.Color);
			Action<Color> colorSelected = this.ColorSelected;
			if (colorSelected != null)
			{
				colorSelected(this.ColorModel.Color);
			}
			await base.Navigation.PopAsync();
			this.btnOK.IsEnabled = true;
		}

		// Token: 0x1700137D RID: 4989
		// (get) Token: 0x060036A8 RID: 13992 RVA: 0x00285DCF File Offset: 0x00283FCF
		// (set) Token: 0x060036A9 RID: 13993 RVA: 0x00285DD7 File Offset: 0x00283FD7
		public ColorObject SelectedColor
		{
			[CompilerGenerated]
			get
			{
				return this.<SelectedColor>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<SelectedColor>k__BackingField = value;
			}
		}

		// Token: 0x060036AA RID: 13994 RVA: 0x000027D4 File Offset: 0x000009D4
		private void ColorPicker_PickedColorChanged(object sender, Color colorPicked)
		{
		}

		// Token: 0x060036AB RID: 13995 RVA: 0x00285DE0 File Offset: 0x00283FE0
		private void TapGestureRecognizer_Tapped(object sender, EventArgs e)
		{
			this.ColorModel.Color = Color.Transparent;
		}

		// Token: 0x060036AC RID: 13996 RVA: 0x00285DF4 File Offset: 0x00283FF4
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private void InitializeComponent()
		{
			if (ResourceLoader.IsEnabled && ResourceLoader.CanProvideContentFor(new ResourceLoader.ResourceLoadingQuery
			{
				AssemblyName = typeof(ColorSelectorPageV2).GetTypeInfo().Assembly.GetName(),
				ResourcePath = "Pages/ColorSelectorPageV2.xaml",
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
			VisualDiagnostics.RegisterSourceInfo(translate = new Translate(), new Uri("Pages\\ColorSelectorPageV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 13, 5);
			DynamicResourceExtension dynamicResourceExtension;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension = new DynamicResourceExtension(), new Uri("Pages\\ColorSelectorPageV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 16, 5);
			EmptyStringToFalseConverter emptyStringToFalseConverter;
			VisualDiagnostics.RegisterSourceInfo(emptyStringToFalseConverter = new EmptyStringToFalseConverter(), new Uri("Pages\\ColorSelectorPageV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 21, 14);
			EmptyStringToTrueConverter emptyStringToTrueConverter;
			VisualDiagnostics.RegisterSourceInfo(emptyStringToTrueConverter = new EmptyStringToTrueConverter(), new Uri("Pages\\ColorSelectorPageV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 22, 14);
			TranparentColorToTrueConverter tranparentColorToTrueConverter;
			VisualDiagnostics.RegisterSourceInfo(tranparentColorToTrueConverter = new TranparentColorToTrueConverter(), new Uri("Pages\\ColorSelectorPageV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 23, 14);
			TransparentColorToTransparentImagePathConverter transparentColorToTransparentImagePathConverter;
			VisualDiagnostics.RegisterSourceInfo(transparentColorToTransparentImagePathConverter = new TransparentColorToTransparentImagePathConverter(), new Uri("Pages\\ColorSelectorPageV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 24, 14);
			ResourceDictionary resourceDictionary;
			VisualDiagnostics.RegisterSourceInfo(resourceDictionary = new ResourceDictionary(), new Uri("Pages\\ColorSelectorPageV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 20, 10);
			RowDefinition rowDefinition;
			VisualDiagnostics.RegisterSourceInfo(rowDefinition = new RowDefinition(), new Uri("Pages\\ColorSelectorPageV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 30, 18);
			RowDefinition rowDefinition2;
			VisualDiagnostics.RegisterSourceInfo(rowDefinition2 = new RowDefinition(), new Uri("Pages\\ColorSelectorPageV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 31, 18);
			BindingExtension bindingExtension;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension = new BindingExtension(), new Uri("Pages\\ColorSelectorPageV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 39, 17);
			BindingExtension bindingExtension2;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension2 = new BindingExtension(), new Uri("Pages\\ColorSelectorPageV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 40, 17);
			DynamicResourceExtension dynamicResourceExtension2;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension2 = new DynamicResourceExtension(), new Uri("Pages\\ColorSelectorPageV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 42, 17);
			ColorPickerView colorPickerView;
			VisualDiagnostics.RegisterSourceInfo(colorPickerView = new ColorPickerView(), new Uri("Pages\\ColorSelectorPageV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 49, 26);
			ColumnDefinition columnDefinition;
			VisualDiagnostics.RegisterSourceInfo(columnDefinition = new ColumnDefinition(), new Uri("Pages\\ColorSelectorPageV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 52, 34);
			ColumnDefinition columnDefinition2;
			VisualDiagnostics.RegisterSourceInfo(columnDefinition2 = new ColumnDefinition(), new Uri("Pages\\ColorSelectorPageV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 53, 34);
			Translate translate2;
			VisualDiagnostics.RegisterSourceInfo(translate2 = new Translate(), new Uri("Pages\\ColorSelectorPageV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 61, 33);
			LinkButton linkButton;
			VisualDiagnostics.RegisterSourceInfo(linkButton = new LinkButton(), new Uri("Pages\\ColorSelectorPageV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 55, 30);
			Translate translate3;
			VisualDiagnostics.RegisterSourceInfo(translate3 = new Translate(), new Uri("Pages\\ColorSelectorPageV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 68, 33);
			LinkButton linkButton2;
			VisualDiagnostics.RegisterSourceInfo(linkButton2 = new LinkButton(), new Uri("Pages\\ColorSelectorPageV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 62, 30);
			Grid grid;
			VisualDiagnostics.RegisterSourceInfo(grid = new Grid(), new Uri("Pages\\ColorSelectorPageV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 50, 26);
			BindingExtension bindingExtension3;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension3 = new BindingExtension(), new Uri("Pages\\ColorSelectorPageV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 73, 29);
			ColorPickerMixer colorPickerMixer;
			VisualDiagnostics.RegisterSourceInfo(colorPickerMixer = new ColorPickerMixer(), new Uri("Pages\\ColorSelectorPageV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 70, 26);
			StackLayout stackLayout;
			VisualDiagnostics.RegisterSourceInfo(stackLayout = new StackLayout(), new Uri("Pages\\ColorSelectorPageV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 45, 22);
			DataTemplate dataTemplate;
			VisualDiagnostics.RegisterSourceInfo(dataTemplate = new DataTemplate(), new Uri("Pages\\ColorSelectorPageV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 77, 22);
			FlowListView flowListView;
			VisualDiagnostics.RegisterSourceInfo(flowListView = new FlowListView(), new Uri("Pages\\ColorSelectorPageV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 35, 14);
			DynamicResourceExtension dynamicResourceExtension3;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension3 = new DynamicResourceExtension(), new Uri("Pages\\ColorSelectorPageV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 113, 17);
			Button button;
			VisualDiagnostics.RegisterSourceInfo(button = new Button(), new Uri("Pages\\ColorSelectorPageV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 108, 14);
			Grid grid2;
			VisualDiagnostics.RegisterSourceInfo(grid2 = new Grid(), new Uri("Pages\\ColorSelectorPageV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 28, 10);
			VisualDiagnostics.RegisterSourceInfo(this, new Uri("Pages\\ColorSelectorPageV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 2, 2);
			NameScope nameScope = NameScope.GetNameScope(this) ?? new NameScope();
			NameScope.SetNameScope(this, nameScope);
			nameScope.RegisterName("lvColors", flowListView);
			if (flowListView.StyleId == null)
			{
				flowListView.StyleId = "lvColors";
			}
			nameScope.RegisterName("stack", stackLayout);
			if (stackLayout.StyleId == null)
			{
				stackLayout.StyleId = "stack";
			}
			nameScope.RegisterName("spillManCP", colorPickerView);
			if (colorPickerView.StyleId == null)
			{
				colorPickerView.StyleId = "spillManCP";
			}
			nameScope.RegisterName("btnCopy", linkButton);
			if (linkButton.StyleId == null)
			{
				linkButton.StyleId = "btnCopy";
			}
			nameScope.RegisterName("btnPaste", linkButton2);
			if (linkButton2.StyleId == null)
			{
				linkButton2.StyleId = "btnPaste";
			}
			nameScope.RegisterName("colorMixer", colorPickerMixer);
			if (colorPickerMixer.StyleId == null)
			{
				colorPickerMixer.StyleId = "colorMixer";
			}
			nameScope.RegisterName("btnOK", button);
			if (button.StyleId == null)
			{
				button.StyleId = "btnOK";
			}
			this.lvColors = flowListView;
			this.stack = stackLayout;
			this.spillManCP = colorPickerView;
			this.btnCopy = linkButton;
			this.btnPaste = linkButton2;
			this.colorMixer = colorPickerMixer;
			this.btnOK = button;
			this.Resources = resourceDictionary;
			resourceDictionary.Add("EmptyStringToFalseConverter", emptyStringToFalseConverter);
			resourceDictionary.Add("EmptyStringToTrueConverter", emptyStringToTrueConverter);
			resourceDictionary.Add("TranparentColorToTrueConverter", tranparentColorToTrueConverter);
			resourceDictionary.Add("TransparentColorToTransparentImagePathConverter", transparentColorToTransparentImagePathConverter);
			translate.Text = "ios_SelectColor";
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
			xmlNamespaceResolver.Add("colorPicker", "clr-namespace:Spillman.Xamarin.Forms.ColorPicker;assembly=Spillman.Xamarin.Forms.ColorPicker");
			xmlNamespaceResolver.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver.Add("cp", "clr-namespace:Xam.Plugin.SimpleColorPicker");
			xmlNamespaceResolver.Add("flv", "clr-namespace:DLToolkit.Forms.Controls;assembly=DLToolkit.Forms.Controls.FlowListView");
			xmlNamespaceResolver.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver.Add("settings", "clr-namespace:CarScannerXamarinForms.Settings");
			xamlServiceProvider.Add(typeFromHandle2, new XamlTypeResolver(xmlNamespaceResolver, typeof(ColorSelectorPageV2).GetTypeInfo().Assembly));
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
			xmlNamespaceResolver2.Add("colorPicker", "clr-namespace:Spillman.Xamarin.Forms.ColorPicker;assembly=Spillman.Xamarin.Forms.ColorPicker");
			xmlNamespaceResolver2.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver2.Add("cp", "clr-namespace:Xam.Plugin.SimpleColorPicker");
			xmlNamespaceResolver2.Add("flv", "clr-namespace:DLToolkit.Forms.Controls;assembly=DLToolkit.Forms.Controls.FlowListView");
			xmlNamespaceResolver2.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver2.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver2.Add("settings", "clr-namespace:CarScannerXamarinForms.Settings");
			xamlServiceProvider2.Add(typeFromHandle4, new XamlTypeResolver(xmlNamespaceResolver2, typeof(ColorSelectorPageV2).GetTypeInfo().Assembly));
			xamlServiceProvider2.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(16, 5)));
			DynamicResource dynamicResource = markupExtension2.ProvideValue(xamlServiceProvider2);
			this.SetDynamicResource(VisualElement.BackgroundColorProperty, dynamicResource.Key);
			this.SizeChanged += this.ColorSelectorPageV2_SizeChanged;
			this.Resources = resourceDictionary;
			rowDefinition.SetValue(RowDefinition.HeightProperty, new GridLengthTypeConverter().ConvertFromInvariantString("*"));
			grid2.GetValue(Grid.RowDefinitionsProperty).Add(rowDefinition);
			rowDefinition2.SetValue(RowDefinition.HeightProperty, new GridLengthTypeConverter().ConvertFromInvariantString("auto"));
			grid2.GetValue(Grid.RowDefinitionsProperty).Add(rowDefinition2);
			flowListView.SetValue(Grid.RowProperty, 0);
			flowListView.SetValue(FlowListView.FlowColumnCountProperty, new int?(5));
			bindingExtension.Path = "ItemTappedCommand";
			BindingBase bindingBase = bindingExtension.ProvideValue(null);
			flowListView.SetBinding(FlowListView.FlowItemTappedCommandProperty, bindingBase);
			bindingExtension2.Path = "Colors";
			BindingBase bindingBase2 = bindingExtension2.ProvideValue(null);
			flowListView.SetBinding(FlowListView.FlowItemsSourceProperty, bindingBase2);
			flowListView.SetValue(ListView.HasUnevenRowsProperty, false);
			dynamicResourceExtension2.Key = "BackgroundColor";
			IMarkupExtension<DynamicResource> markupExtension3 = dynamicResourceExtension2;
			XamlServiceProvider xamlServiceProvider3 = new XamlServiceProvider();
			Type typeFromHandle5 = typeof(IProvideValueTarget);
			object[] array3 = new object[0 + 3];
			array3[0] = flowListView;
			array3[1] = grid2;
			array3[2] = this;
			object obj4;
			xamlServiceProvider3.Add(typeFromHandle5, obj4 = new SimpleValueTargetProvider(array3, ListView.SeparatorColorProperty, nameScope));
			xamlServiceProvider3.Add(typeof(IReferenceProvider), obj4);
			Type typeFromHandle6 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver3 = new XmlNamespaceResolver();
			xmlNamespaceResolver3.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver3.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver3.Add("colorPicker", "clr-namespace:Spillman.Xamarin.Forms.ColorPicker;assembly=Spillman.Xamarin.Forms.ColorPicker");
			xmlNamespaceResolver3.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver3.Add("cp", "clr-namespace:Xam.Plugin.SimpleColorPicker");
			xmlNamespaceResolver3.Add("flv", "clr-namespace:DLToolkit.Forms.Controls;assembly=DLToolkit.Forms.Controls.FlowListView");
			xmlNamespaceResolver3.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver3.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver3.Add("settings", "clr-namespace:CarScannerXamarinForms.Settings");
			xamlServiceProvider3.Add(typeFromHandle6, new XamlTypeResolver(xmlNamespaceResolver3, typeof(ColorSelectorPageV2).GetTypeInfo().Assembly));
			xamlServiceProvider3.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(42, 17)));
			DynamicResource dynamicResource2 = markupExtension3.ProvideValue(xamlServiceProvider3);
			flowListView.SetDynamicResource(ListView.SeparatorColorProperty, dynamicResource2.Key);
			flowListView.SetValue(ListView.SeparatorVisibilityProperty, 1);
			stackLayout.SetValue(View.MarginProperty, new Thickness(0.0, 0.0, 0.0, 5.0));
			stackLayout.SetValue(StackLayout.OrientationProperty, 0);
			stackLayout.Children.Add(colorPickerView);
			columnDefinition.SetValue(ColumnDefinition.WidthProperty, new GridLengthTypeConverter().ConvertFromInvariantString("*"));
			grid.GetValue(Grid.ColumnDefinitionsProperty).Add(columnDefinition);
			columnDefinition2.SetValue(ColumnDefinition.WidthProperty, new GridLengthTypeConverter().ConvertFromInvariantString("*"));
			grid.GetValue(Grid.ColumnDefinitionsProperty).Add(columnDefinition2);
			linkButton.SetValue(Grid.ColumnProperty, 0);
			linkButton.SetValue(VisualElement.BackgroundColorProperty, Color.Transparent);
			linkButton.SetValue(Button.BorderColorProperty, Color.Transparent);
			linkButton.Clicked += this.btnCopy_Clicked;
			translate2.Text = "ios_Copy";
			IMarkupExtension markupExtension4 = translate2;
			XamlServiceProvider xamlServiceProvider4 = new XamlServiceProvider();
			Type typeFromHandle7 = typeof(IProvideValueTarget);
			object[] array4 = new object[0 + 6];
			array4[0] = linkButton;
			array4[1] = grid;
			array4[2] = stackLayout;
			array4[3] = flowListView;
			array4[4] = grid2;
			array4[5] = this;
			object obj5;
			xamlServiceProvider4.Add(typeFromHandle7, obj5 = new SimpleValueTargetProvider(array4, Button.TextProperty, nameScope));
			xamlServiceProvider4.Add(typeof(IReferenceProvider), obj5);
			Type typeFromHandle8 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver4 = new XmlNamespaceResolver();
			xmlNamespaceResolver4.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver4.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver4.Add("colorPicker", "clr-namespace:Spillman.Xamarin.Forms.ColorPicker;assembly=Spillman.Xamarin.Forms.ColorPicker");
			xmlNamespaceResolver4.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver4.Add("cp", "clr-namespace:Xam.Plugin.SimpleColorPicker");
			xmlNamespaceResolver4.Add("flv", "clr-namespace:DLToolkit.Forms.Controls;assembly=DLToolkit.Forms.Controls.FlowListView");
			xmlNamespaceResolver4.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver4.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver4.Add("settings", "clr-namespace:CarScannerXamarinForms.Settings");
			xamlServiceProvider4.Add(typeFromHandle8, new XamlTypeResolver(xmlNamespaceResolver4, typeof(ColorSelectorPageV2).GetTypeInfo().Assembly));
			xamlServiceProvider4.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(61, 33)));
			object obj6 = markupExtension4.ProvideValue(xamlServiceProvider4);
			linkButton.Text = obj6;
			grid.Children.Add(linkButton);
			linkButton2.SetValue(Grid.ColumnProperty, 1);
			linkButton2.SetValue(VisualElement.BackgroundColorProperty, Color.Transparent);
			linkButton2.SetValue(Button.BorderColorProperty, Color.Transparent);
			linkButton2.Clicked += this.btnPaste_Clicked;
			translate3.Text = "ios_Paste";
			IMarkupExtension markupExtension5 = translate3;
			XamlServiceProvider xamlServiceProvider5 = new XamlServiceProvider();
			Type typeFromHandle9 = typeof(IProvideValueTarget);
			object[] array5 = new object[0 + 6];
			array5[0] = linkButton2;
			array5[1] = grid;
			array5[2] = stackLayout;
			array5[3] = flowListView;
			array5[4] = grid2;
			array5[5] = this;
			object obj7;
			xamlServiceProvider5.Add(typeFromHandle9, obj7 = new SimpleValueTargetProvider(array5, Button.TextProperty, nameScope));
			xamlServiceProvider5.Add(typeof(IReferenceProvider), obj7);
			Type typeFromHandle10 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver5 = new XmlNamespaceResolver();
			xmlNamespaceResolver5.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver5.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver5.Add("colorPicker", "clr-namespace:Spillman.Xamarin.Forms.ColorPicker;assembly=Spillman.Xamarin.Forms.ColorPicker");
			xmlNamespaceResolver5.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver5.Add("cp", "clr-namespace:Xam.Plugin.SimpleColorPicker");
			xmlNamespaceResolver5.Add("flv", "clr-namespace:DLToolkit.Forms.Controls;assembly=DLToolkit.Forms.Controls.FlowListView");
			xmlNamespaceResolver5.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver5.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver5.Add("settings", "clr-namespace:CarScannerXamarinForms.Settings");
			xamlServiceProvider5.Add(typeFromHandle10, new XamlTypeResolver(xmlNamespaceResolver5, typeof(ColorSelectorPageV2).GetTypeInfo().Assembly));
			xamlServiceProvider5.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(68, 33)));
			object obj8 = markupExtension5.ProvideValue(xamlServiceProvider5);
			linkButton2.Text = obj8;
			grid.Children.Add(linkButton2);
			stackLayout.Children.Add(grid);
			colorPickerMixer.EditAlpha = true;
			bindingExtension3.Path = "Color";
			BindingBase bindingBase3 = bindingExtension3.ProvideValue(null);
			colorPickerMixer.SetBinding(ColorPickerMixer.ColorProperty, bindingBase3);
			stackLayout.Children.Add(colorPickerMixer);
			flowListView.SetValue(ListView.HeaderProperty, stackLayout);
			IDataTemplate dataTemplate2 = dataTemplate;
			ColorSelectorPageV2.<InitializeComponent>_anonXamlCDataTemplate_34 <InitializeComponent>_anonXamlCDataTemplate_ = new ColorSelectorPageV2.<InitializeComponent>_anonXamlCDataTemplate_34();
			object[] array6 = new object[0 + 4];
			array6[0] = dataTemplate;
			array6[1] = flowListView;
			array6[2] = grid2;
			array6[3] = this;
			<InitializeComponent>_anonXamlCDataTemplate_.parentValues = array6;
			<InitializeComponent>_anonXamlCDataTemplate_.root = this;
			dataTemplate2.LoadTemplate = new Func<object>(<InitializeComponent>_anonXamlCDataTemplate_.LoadDataTemplate);
			flowListView.SetValue(FlowListView.FlowColumnTemplateProperty, dataTemplate);
			grid2.Children.Add(flowListView);
			button.SetValue(Grid.RowProperty, 1);
			button.Clicked += this.btnOK_Clicked;
			button.SetValue(Button.TextProperty, "OK");
			dynamicResourceExtension3.Key = "ButtonAccentColor";
			IMarkupExtension<DynamicResource> markupExtension6 = dynamicResourceExtension3;
			XamlServiceProvider xamlServiceProvider6 = new XamlServiceProvider();
			Type typeFromHandle11 = typeof(IProvideValueTarget);
			object[] array7 = new object[0 + 3];
			array7[0] = button;
			array7[1] = grid2;
			array7[2] = this;
			object obj9;
			xamlServiceProvider6.Add(typeFromHandle11, obj9 = new SimpleValueTargetProvider(array7, Button.TextColorProperty, nameScope));
			xamlServiceProvider6.Add(typeof(IReferenceProvider), obj9);
			Type typeFromHandle12 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver6 = new XmlNamespaceResolver();
			xmlNamespaceResolver6.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver6.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver6.Add("colorPicker", "clr-namespace:Spillman.Xamarin.Forms.ColorPicker;assembly=Spillman.Xamarin.Forms.ColorPicker");
			xmlNamespaceResolver6.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver6.Add("cp", "clr-namespace:Xam.Plugin.SimpleColorPicker");
			xmlNamespaceResolver6.Add("flv", "clr-namespace:DLToolkit.Forms.Controls;assembly=DLToolkit.Forms.Controls.FlowListView");
			xmlNamespaceResolver6.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver6.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver6.Add("settings", "clr-namespace:CarScannerXamarinForms.Settings");
			xamlServiceProvider6.Add(typeFromHandle12, new XamlTypeResolver(xmlNamespaceResolver6, typeof(ColorSelectorPageV2).GetTypeInfo().Assembly));
			xamlServiceProvider6.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(113, 17)));
			DynamicResource dynamicResource3 = markupExtension6.ProvideValue(xamlServiceProvider6);
			button.SetDynamicResource(Button.TextColorProperty, dynamicResource3.Key);
			grid2.Children.Add(button);
			this.SetValue(ContentPage.ContentProperty, grid2);
		}

		// Token: 0x060036AD RID: 13997 RVA: 0x00287060 File Offset: 0x00285260
		private void __InitComponentRuntime()
		{
			Extensions.LoadFromXaml<ColorSelectorPageV2>(this, typeof(ColorSelectorPageV2));
			this.lvColors = NameScopeExtensions.FindByName<FlowListView>(this, "lvColors");
			this.stack = NameScopeExtensions.FindByName<StackLayout>(this, "stack");
			this.spillManCP = NameScopeExtensions.FindByName<ColorPickerView>(this, "spillManCP");
			this.btnCopy = NameScopeExtensions.FindByName<LinkButton>(this, "btnCopy");
			this.btnPaste = NameScopeExtensions.FindByName<LinkButton>(this, "btnPaste");
			this.colorMixer = NameScopeExtensions.FindByName<ColorPickerMixer>(this, "colorMixer");
			this.btnOK = NameScopeExtensions.FindByName<Button>(this, "btnOK");
		}

		// Token: 0x040020DC RID: 8412
		[CompilerGenerated]
		private ICommand <ItemTappedCommand>k__BackingField;

		// Token: 0x040020DD RID: 8413
		private List<ColorItem> _Colors;

		// Token: 0x040020DE RID: 8414
		private ColorPickerViewModel ColorModel = new ColorPickerViewModel();

		// Token: 0x040020DF RID: 8415
		public Action<Color> ColorSelected;

		// Token: 0x040020E0 RID: 8416
		[CompilerGenerated]
		private ColorObject <SelectedColor>k__BackingField;

		// Token: 0x040020E1 RID: 8417
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private FlowListView lvColors;

		// Token: 0x040020E2 RID: 8418
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private StackLayout stack;

		// Token: 0x040020E3 RID: 8419
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private ColorPickerView spillManCP;

		// Token: 0x040020E4 RID: 8420
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private LinkButton btnCopy;

		// Token: 0x040020E5 RID: 8421
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private LinkButton btnPaste;

		// Token: 0x040020E6 RID: 8422
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private ColorPickerMixer colorMixer;

		// Token: 0x040020E7 RID: 8423
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private Button btnOK;

		// Token: 0x0200060F RID: 1551
		[CompilerGenerated]
		[Serializable]
		private sealed class <>c
		{
			// Token: 0x060036AE RID: 13998 RVA: 0x002870F5 File Offset: 0x002852F5
			// Note: this type is marked as 'beforefieldinit'.
			static <>c()
			{
			}

			// Token: 0x060036AF RID: 13999 RVA: 0x00002050 File Offset: 0x00000250
			public <>c()
			{
			}

			// Token: 0x060036B0 RID: 14000 RVA: 0x00287101 File Offset: 0x00285301
			internal int <CreateColors>b__8_0(ColorItem x, ColorItem y)
			{
				return ColorSelectorPageV2.CompareColors(x, y);
			}

			// Token: 0x060036B1 RID: 14001 RVA: 0x0028710A File Offset: 0x0028530A
			internal ColorItem <CreateColors>b__8_1(Color x)
			{
				return new ColorItem
				{
					Color = x
				};
			}

			// Token: 0x040020E8 RID: 8424
			public static readonly ColorSelectorPageV2.<>c <>9 = new ColorSelectorPageV2.<>c();

			// Token: 0x040020E9 RID: 8425
			public static Comparison<ColorItem> <>9__8_0;

			// Token: 0x040020EA RID: 8426
			public static Func<Color, ColorItem> <>9__8_1;
		}

		// Token: 0x02000610 RID: 1552
		[CompilerGenerated]
		private sealed class <>c__DisplayClass0_0
		{
			// Token: 0x060036B2 RID: 14002 RVA: 0x00002050 File Offset: 0x00000250
			public <>c__DisplayClass0_0()
			{
			}

			// Token: 0x060036B3 RID: 14003 RVA: 0x00287118 File Offset: 0x00285318
			internal async void <.ctor>b__0(object param)
			{
				Color color = (param as ColorItem).Color;
				this.<>4__this.ColorModel.Color = color;
				this.<>4__this.ColorModel.UpdateHex();
				this.<>4__this.AddToRecent(color);
				this.<>4__this.lvColors.IsEnabled = false;
				Action<Color> colorSelected = this.ColorSelected;
				if (colorSelected != null)
				{
					colorSelected(this.<>4__this.ColorModel.Color);
				}
				this.<>4__this.lvColors.IsEnabled = true;
				await this.<>4__this.Navigation.PopAsync();
			}

			// Token: 0x040020EB RID: 8427
			public ColorSelectorPageV2 <>4__this;

			// Token: 0x040020EC RID: 8428
			public Action<Color> ColorSelected;

			// Token: 0x02000611 RID: 1553
			[StructLayout(LayoutKind.Auto)]
			private struct <<-ctor>b__0>d : IAsyncStateMachine
			{
				// Token: 0x060036B4 RID: 14004 RVA: 0x00287158 File Offset: 0x00285358
				void IAsyncStateMachine.MoveNext()
				{
					int num2;
					int num = num2;
					ColorSelectorPageV2.<>c__DisplayClass0_0 CS$<>8__locals1 = this;
					try
					{
						TaskAwaiter<Page> taskAwaiter;
						if (num != 0)
						{
							Color color = (param as ColorItem).Color;
							CS$<>8__locals1.<>4__this.ColorModel.Color = color;
							CS$<>8__locals1.<>4__this.ColorModel.UpdateHex();
							CS$<>8__locals1.<>4__this.AddToRecent(color);
							CS$<>8__locals1.<>4__this.lvColors.IsEnabled = false;
							Action<Color> colorSelected = CS$<>8__locals1.ColorSelected;
							if (colorSelected != null)
							{
								colorSelected(CS$<>8__locals1.<>4__this.ColorModel.Color);
							}
							CS$<>8__locals1.<>4__this.lvColors.IsEnabled = true;
							taskAwaiter = CS$<>8__locals1.<>4__this.Navigation.PopAsync().GetAwaiter();
							if (!taskAwaiter.IsCompleted)
							{
								num2 = 0;
								TaskAwaiter<Page> taskAwaiter2 = taskAwaiter;
								this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<Page>, ColorSelectorPageV2.<>c__DisplayClass0_0.<<-ctor>b__0>d>(ref taskAwaiter, ref this);
								return;
							}
						}
						else
						{
							TaskAwaiter<Page> taskAwaiter2;
							taskAwaiter = taskAwaiter2;
							taskAwaiter2 = default(TaskAwaiter<Page>);
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

				// Token: 0x060036B5 RID: 14005 RVA: 0x0028729C File Offset: 0x0028549C
				[DebuggerHidden]
				void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
				{
					this.<>t__builder.SetStateMachine(stateMachine);
				}

				// Token: 0x040020ED RID: 8429
				public int <>1__state;

				// Token: 0x040020EE RID: 8430
				public AsyncVoidMethodBuilder <>t__builder;

				// Token: 0x040020EF RID: 8431
				public object param;

				// Token: 0x040020F0 RID: 8432
				public ColorSelectorPageV2.<>c__DisplayClass0_0 <>4__this;

				// Token: 0x040020F1 RID: 8433
				private TaskAwaiter<Page> <>u__1;
			}
		}

		// Token: 0x02000612 RID: 1554
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <btnCopy_Clicked>d__14 : IAsyncStateMachine
		{
			// Token: 0x060036B6 RID: 14006 RVA: 0x002872AC File Offset: 0x002854AC
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				ColorSelectorPageV2 colorSelectorPageV = this;
				try
				{
					if (num != 0)
					{
						colorSelectorPageV.ColorModel.UpdateHex();
					}
					try
					{
						TaskAwaiter taskAwaiter;
						if (num != 0)
						{
							taskAwaiter = Clipboard.SetTextAsync(colorSelectorPageV.ColorModel.Hex).GetAwaiter();
							if (!taskAwaiter.IsCompleted)
							{
								num2 = 0;
								TaskAwaiter taskAwaiter2 = taskAwaiter;
								this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, ColorSelectorPageV2.<btnCopy_Clicked>d__14>(ref taskAwaiter, ref this);
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

			// Token: 0x060036B7 RID: 14007 RVA: 0x00287388 File Offset: 0x00285588
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x040020F2 RID: 8434
			public int <>1__state;

			// Token: 0x040020F3 RID: 8435
			public AsyncVoidMethodBuilder <>t__builder;

			// Token: 0x040020F4 RID: 8436
			public ColorSelectorPageV2 <>4__this;

			// Token: 0x040020F5 RID: 8437
			private TaskAwaiter <>u__1;
		}

		// Token: 0x02000613 RID: 1555
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <btnOK_Clicked>d__16 : IAsyncStateMachine
		{
			// Token: 0x060036B8 RID: 14008 RVA: 0x00287398 File Offset: 0x00285598
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				ColorSelectorPageV2 colorSelectorPageV = this;
				try
				{
					TaskAwaiter<Page> taskAwaiter;
					if (num != 0)
					{
						colorSelectorPageV.btnOK.IsEnabled = false;
						colorSelectorPageV.AddToRecent(colorSelectorPageV.ColorModel.Color);
						Action<Color> colorSelected = colorSelectorPageV.ColorSelected;
						if (colorSelected != null)
						{
							colorSelected(colorSelectorPageV.ColorModel.Color);
						}
						taskAwaiter = colorSelectorPageV.Navigation.PopAsync().GetAwaiter();
						if (!taskAwaiter.IsCompleted)
						{
							num2 = 0;
							TaskAwaiter<Page> taskAwaiter2 = taskAwaiter;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<Page>, ColorSelectorPageV2.<btnOK_Clicked>d__16>(ref taskAwaiter, ref this);
							return;
						}
					}
					else
					{
						TaskAwaiter<Page> taskAwaiter2;
						taskAwaiter = taskAwaiter2;
						taskAwaiter2 = default(TaskAwaiter<Page>);
						num2 = -1;
					}
					taskAwaiter.GetResult();
					colorSelectorPageV.btnOK.IsEnabled = true;
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

			// Token: 0x060036B9 RID: 14009 RVA: 0x00287498 File Offset: 0x00285698
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x040020F6 RID: 8438
			public int <>1__state;

			// Token: 0x040020F7 RID: 8439
			public AsyncVoidMethodBuilder <>t__builder;

			// Token: 0x040020F8 RID: 8440
			public ColorSelectorPageV2 <>4__this;

			// Token: 0x040020F9 RID: 8441
			private TaskAwaiter<Page> <>u__1;
		}

		// Token: 0x02000614 RID: 1556
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <btnPaste_Clicked>d__15 : IAsyncStateMachine
		{
			// Token: 0x060036BA RID: 14010 RVA: 0x002874A8 File Offset: 0x002856A8
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				ColorSelectorPageV2 colorSelectorPageV = this;
				try
				{
					try
					{
						TaskAwaiter<string> taskAwaiter;
						if (num != 0)
						{
							taskAwaiter = Clipboard.GetTextAsync().GetAwaiter();
							if (!taskAwaiter.IsCompleted)
							{
								num2 = 0;
								TaskAwaiter<string> taskAwaiter2 = taskAwaiter;
								this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<string>, ColorSelectorPageV2.<btnPaste_Clicked>d__15>(ref taskAwaiter, ref this);
								return;
							}
						}
						else
						{
							TaskAwaiter<string> taskAwaiter2;
							taskAwaiter = taskAwaiter2;
							taskAwaiter2 = default(TaskAwaiter<string>);
							num2 = -1;
						}
						string result = taskAwaiter.GetResult();
						if (!string.IsNullOrEmpty(result))
						{
							try
							{
								colorSelectorPageV.ColorModel.Hex = result;
							}
							catch (Exception)
							{
							}
						}
					}
					catch (Exception)
					{
					}
					colorSelectorPageV.ColorModel.UpdateHex();
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

			// Token: 0x060036BB RID: 14011 RVA: 0x002875A0 File Offset: 0x002857A0
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x040020FA RID: 8442
			public int <>1__state;

			// Token: 0x040020FB RID: 8443
			public AsyncVoidMethodBuilder <>t__builder;

			// Token: 0x040020FC RID: 8444
			public ColorSelectorPageV2 <>4__this;

			// Token: 0x040020FD RID: 8445
			private TaskAwaiter<string> <>u__1;
		}

		// Token: 0x02000615 RID: 1557
		[CompilerGenerated]
		private sealed class <InitializeComponent>_anonXamlCDataTemplate_34
		{
			// Token: 0x060036BC RID: 14012 RVA: 0x002875B0 File Offset: 0x002857B0
			public <InitializeComponent>_anonXamlCDataTemplate_34()
			{
			}

			// Token: 0x060036BD RID: 14013 RVA: 0x002875C4 File Offset: 0x002857C4
			internal object LoadDataTemplate()
			{
				DynamicResourceExtension dynamicResourceExtension;
				VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension = new DynamicResourceExtension(), new Uri("Pages\\ColorSelectorPageV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 80, 29);
				DynamicResourceExtension dynamicResourceExtension2;
				VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension2 = new DynamicResourceExtension(), new Uri("Pages\\ColorSelectorPageV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 83, 29);
				RowDefinition rowDefinition;
				VisualDiagnostics.RegisterSourceInfo(rowDefinition = new RowDefinition(), new Uri("Pages\\ColorSelectorPageV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 86, 38);
				BindingExtension bindingExtension;
				VisualDiagnostics.RegisterSourceInfo(bindingExtension = new BindingExtension(), new Uri("Pages\\ColorSelectorPageV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 94, 37);
				BoxView boxView;
				VisualDiagnostics.RegisterSourceInfo(boxView = new BoxView(), new Uri("Pages\\ColorSelectorPageV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 88, 34);
				StaticResourceExtension staticResourceExtension;
				VisualDiagnostics.RegisterSourceInfo(staticResourceExtension = new StaticResourceExtension(), new Uri("Pages\\ColorSelectorPageV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 100, 37);
				BindingExtension bindingExtension2;
				VisualDiagnostics.RegisterSourceInfo(bindingExtension2 = new BindingExtension(), new Uri("Pages\\ColorSelectorPageV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 100, 37);
				StaticResourceExtension staticResourceExtension2;
				VisualDiagnostics.RegisterSourceInfo(staticResourceExtension2 = new StaticResourceExtension(), new Uri("Pages\\ColorSelectorPageV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 101, 37);
				BindingExtension bindingExtension3;
				VisualDiagnostics.RegisterSourceInfo(bindingExtension3 = new BindingExtension(), new Uri("Pages\\ColorSelectorPageV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 101, 37);
				Image image;
				VisualDiagnostics.RegisterSourceInfo(image = new Image(), new Uri("Pages\\ColorSelectorPageV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 95, 34);
				Grid grid;
				VisualDiagnostics.RegisterSourceInfo(grid = new Grid(), new Uri("Pages\\ColorSelectorPageV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 84, 30);
				Frame frame;
				VisualDiagnostics.RegisterSourceInfo(frame = new Frame(), new Uri("Pages\\ColorSelectorPageV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 78, 26);
				NameScope nameScope = new NameScope();
				NameScope.SetNameScope(frame, nameScope);
				frame.SetValue(Layout.PaddingProperty, new Thickness(1.0));
				dynamicResourceExtension.Key = "TextColor";
				IMarkupExtension<DynamicResource> markupExtension = dynamicResourceExtension;
				XamlServiceProvider xamlServiceProvider = new XamlServiceProvider();
				Type typeFromHandle = typeof(IProvideValueTarget);
				int num;
				object[] array = new object[(num = this.parentValues.Length) + 1];
				Array.Copy(this.parentValues, 0, array, 1, num);
				object[] array2 = array;
				array2[0] = frame;
				object obj;
				xamlServiceProvider.Add(typeFromHandle, obj = new SimpleValueTargetProvider(array2, VisualElement.BackgroundColorProperty, nameScope));
				xamlServiceProvider.Add(typeof(IReferenceProvider), obj);
				Type typeFromHandle2 = typeof(IXamlTypeResolver);
				XmlNamespaceResolver xmlNamespaceResolver = new XmlNamespaceResolver();
				xmlNamespaceResolver.Add("", "http://xamarin.com/schemas/2014/forms");
				xmlNamespaceResolver.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
				xmlNamespaceResolver.Add("colorPicker", "clr-namespace:Spillman.Xamarin.Forms.ColorPicker;assembly=Spillman.Xamarin.Forms.ColorPicker");
				xmlNamespaceResolver.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
				xmlNamespaceResolver.Add("cp", "clr-namespace:Xam.Plugin.SimpleColorPicker");
				xmlNamespaceResolver.Add("flv", "clr-namespace:DLToolkit.Forms.Controls;assembly=DLToolkit.Forms.Controls.FlowListView");
				xmlNamespaceResolver.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
				xmlNamespaceResolver.Add("local", "clr-namespace:CarScannerXamarinForms");
				xmlNamespaceResolver.Add("settings", "clr-namespace:CarScannerXamarinForms.Settings");
				xamlServiceProvider.Add(typeFromHandle2, new XamlTypeResolver(xmlNamespaceResolver, typeof(ColorSelectorPageV2.<InitializeComponent>_anonXamlCDataTemplate_34).GetTypeInfo().Assembly));
				xamlServiceProvider.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(80, 29)));
				DynamicResource dynamicResource = markupExtension.ProvideValue(xamlServiceProvider);
				frame.SetDynamicResource(VisualElement.BackgroundColorProperty, dynamicResource.Key);
				frame.SetValue(Frame.CornerRadiusProperty, 0f);
				frame.SetValue(Frame.HasShadowProperty, false);
				dynamicResourceExtension2.Key = "TextColor";
				IMarkupExtension<DynamicResource> markupExtension2 = dynamicResourceExtension2;
				XamlServiceProvider xamlServiceProvider2 = new XamlServiceProvider();
				Type typeFromHandle3 = typeof(IProvideValueTarget);
				int num2;
				object[] array3 = new object[(num2 = this.parentValues.Length) + 1];
				Array.Copy(this.parentValues, 0, array3, 1, num2);
				object[] array4 = array3;
				array4[0] = frame;
				object obj2;
				xamlServiceProvider2.Add(typeFromHandle3, obj2 = new SimpleValueTargetProvider(array4, Frame.OutlineColorProperty, nameScope));
				xamlServiceProvider2.Add(typeof(IReferenceProvider), obj2);
				Type typeFromHandle4 = typeof(IXamlTypeResolver);
				XmlNamespaceResolver xmlNamespaceResolver2 = new XmlNamespaceResolver();
				xmlNamespaceResolver2.Add("", "http://xamarin.com/schemas/2014/forms");
				xmlNamespaceResolver2.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
				xmlNamespaceResolver2.Add("colorPicker", "clr-namespace:Spillman.Xamarin.Forms.ColorPicker;assembly=Spillman.Xamarin.Forms.ColorPicker");
				xmlNamespaceResolver2.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
				xmlNamespaceResolver2.Add("cp", "clr-namespace:Xam.Plugin.SimpleColorPicker");
				xmlNamespaceResolver2.Add("flv", "clr-namespace:DLToolkit.Forms.Controls;assembly=DLToolkit.Forms.Controls.FlowListView");
				xmlNamespaceResolver2.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
				xmlNamespaceResolver2.Add("local", "clr-namespace:CarScannerXamarinForms");
				xmlNamespaceResolver2.Add("settings", "clr-namespace:CarScannerXamarinForms.Settings");
				xamlServiceProvider2.Add(typeFromHandle4, new XamlTypeResolver(xmlNamespaceResolver2, typeof(ColorSelectorPageV2.<InitializeComponent>_anonXamlCDataTemplate_34).GetTypeInfo().Assembly));
				xamlServiceProvider2.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(83, 29)));
				DynamicResource dynamicResource2 = markupExtension2.ProvideValue(xamlServiceProvider2);
				frame.SetDynamicResource(Frame.OutlineColorProperty, dynamicResource2.Key);
				rowDefinition.SetValue(RowDefinition.HeightProperty, new GridLengthTypeConverter().ConvertFromInvariantString("auto"));
				grid.GetValue(Grid.RowDefinitionsProperty).Add(rowDefinition);
				boxView.SetValue(Grid.RowProperty, 0);
				boxView.SetValue(View.MarginProperty, new Thickness(0.0));
				boxView.SetValue(VisualElement.HeightRequestProperty, 50.0);
				boxView.SetValue(View.HorizontalOptionsProperty, LayoutOptions.Fill);
				boxView.SetValue(View.VerticalOptionsProperty, LayoutOptions.Fill);
				bindingExtension.Path = "Color";
				BindingBase bindingBase = bindingExtension.ProvideValue(null);
				boxView.SetBinding(BoxView.ColorProperty, bindingBase);
				grid.Children.Add(boxView);
				image.SetValue(Grid.RowProperty, 0);
				image.SetValue(Image.AspectProperty, 2);
				image.SetValue(VisualElement.HeightRequestProperty, 50.0);
				image.SetValue(VisualElement.InputTransparentProperty, true);
				staticResourceExtension.Key = "TranparentColorToTrueConverter";
				IMarkupExtension markupExtension3 = staticResourceExtension;
				XamlServiceProvider xamlServiceProvider3 = new XamlServiceProvider();
				Type typeFromHandle5 = typeof(IProvideValueTarget);
				int num3;
				object[] array5 = new object[(num3 = this.parentValues.Length) + 4];
				Array.Copy(this.parentValues, 0, array5, 4, num3);
				object[] array6 = array5;
				array6[0] = bindingExtension2;
				array6[1] = image;
				array6[2] = grid;
				array6[3] = frame;
				object obj3;
				xamlServiceProvider3.Add(typeFromHandle5, obj3 = new SimpleValueTargetProvider(array6, typeof(BindingExtension).GetRuntimeProperty("Converter"), nameScope));
				xamlServiceProvider3.Add(typeof(IReferenceProvider), obj3);
				Type typeFromHandle6 = typeof(IXamlTypeResolver);
				XmlNamespaceResolver xmlNamespaceResolver3 = new XmlNamespaceResolver();
				xmlNamespaceResolver3.Add("", "http://xamarin.com/schemas/2014/forms");
				xmlNamespaceResolver3.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
				xmlNamespaceResolver3.Add("colorPicker", "clr-namespace:Spillman.Xamarin.Forms.ColorPicker;assembly=Spillman.Xamarin.Forms.ColorPicker");
				xmlNamespaceResolver3.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
				xmlNamespaceResolver3.Add("cp", "clr-namespace:Xam.Plugin.SimpleColorPicker");
				xmlNamespaceResolver3.Add("flv", "clr-namespace:DLToolkit.Forms.Controls;assembly=DLToolkit.Forms.Controls.FlowListView");
				xmlNamespaceResolver3.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
				xmlNamespaceResolver3.Add("local", "clr-namespace:CarScannerXamarinForms");
				xmlNamespaceResolver3.Add("settings", "clr-namespace:CarScannerXamarinForms.Settings");
				xamlServiceProvider3.Add(typeFromHandle6, new XamlTypeResolver(xmlNamespaceResolver3, typeof(ColorSelectorPageV2.<InitializeComponent>_anonXamlCDataTemplate_34).GetTypeInfo().Assembly));
				xamlServiceProvider3.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(100, 37)));
				object obj4 = markupExtension3.ProvideValue(xamlServiceProvider3);
				bindingExtension2.Converter = obj4;
				bindingExtension2.Path = "Color";
				BindingBase bindingBase2 = bindingExtension2.ProvideValue(null);
				image.SetBinding(VisualElement.IsVisibleProperty, bindingBase2);
				staticResourceExtension2.Key = "TransparentColorToTransparentImagePathConverter";
				IMarkupExtension markupExtension4 = staticResourceExtension2;
				XamlServiceProvider xamlServiceProvider4 = new XamlServiceProvider();
				Type typeFromHandle7 = typeof(IProvideValueTarget);
				int num4;
				object[] array7 = new object[(num4 = this.parentValues.Length) + 4];
				Array.Copy(this.parentValues, 0, array7, 4, num4);
				object[] array8 = array7;
				array8[0] = bindingExtension3;
				array8[1] = image;
				array8[2] = grid;
				array8[3] = frame;
				object obj5;
				xamlServiceProvider4.Add(typeFromHandle7, obj5 = new SimpleValueTargetProvider(array8, typeof(BindingExtension).GetRuntimeProperty("Converter"), nameScope));
				xamlServiceProvider4.Add(typeof(IReferenceProvider), obj5);
				Type typeFromHandle8 = typeof(IXamlTypeResolver);
				XmlNamespaceResolver xmlNamespaceResolver4 = new XmlNamespaceResolver();
				xmlNamespaceResolver4.Add("", "http://xamarin.com/schemas/2014/forms");
				xmlNamespaceResolver4.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
				xmlNamespaceResolver4.Add("colorPicker", "clr-namespace:Spillman.Xamarin.Forms.ColorPicker;assembly=Spillman.Xamarin.Forms.ColorPicker");
				xmlNamespaceResolver4.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
				xmlNamespaceResolver4.Add("cp", "clr-namespace:Xam.Plugin.SimpleColorPicker");
				xmlNamespaceResolver4.Add("flv", "clr-namespace:DLToolkit.Forms.Controls;assembly=DLToolkit.Forms.Controls.FlowListView");
				xmlNamespaceResolver4.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
				xmlNamespaceResolver4.Add("local", "clr-namespace:CarScannerXamarinForms");
				xmlNamespaceResolver4.Add("settings", "clr-namespace:CarScannerXamarinForms.Settings");
				xamlServiceProvider4.Add(typeFromHandle8, new XamlTypeResolver(xmlNamespaceResolver4, typeof(ColorSelectorPageV2.<InitializeComponent>_anonXamlCDataTemplate_34).GetTypeInfo().Assembly));
				xamlServiceProvider4.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(101, 37)));
				object obj6 = markupExtension4.ProvideValue(xamlServiceProvider4);
				bindingExtension3.Converter = obj6;
				bindingExtension3.Path = "Color";
				BindingBase bindingBase3 = bindingExtension3.ProvideValue(null);
				image.SetBinding(Image.SourceProperty, bindingBase3);
				grid.Children.Add(image);
				frame.SetValue(ContentView.ContentProperty, grid);
				return frame;
			}

			// Token: 0x040020FE RID: 8446
			internal object[] parentValues;

			// Token: 0x040020FF RID: 8447
			internal ColorSelectorPageV2 root;
		}
	}
}
