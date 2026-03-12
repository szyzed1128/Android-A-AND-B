using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Reflection;
using System.Runtime.CompilerServices;
using Xamarin.Forms;
using Xamarin.Forms.Internals;
using Xamarin.Forms.Xaml;
using Xamarin.Forms.Xaml.Diagnostics;
using Xamarin.Forms.Xaml.Internals;

namespace Xam.Plugin.SimpleColorPicker
{
	// Token: 0x02000012 RID: 18
	[XamlCompilation(2)]
	[XamlFilePath("UserControls\\ColorMixer\\ColorPickerMixer.xaml")]
	public class ColorPickerMixer : ContentView
	{
		// Token: 0x0600006F RID: 111 RVA: 0x00003CE0 File Offset: 0x00001EE0
		public ColorPickerMixer()
		{
			this.InitializeComponent();
			this.gMain.BindingContext = this;
			this.ColorVal.PropertyChanged += delegate(object s, PropertyChangedEventArgs e)
			{
				string propertyName = e.PropertyName;
				if (propertyName == "Value")
				{
					this.Color = ((ColorValue)s).Value;
					return;
				}
				if (!(propertyName == "EditAlpha"))
				{
					return;
				}
				this.ValueChanged("EditAlpha");
			};
		}

		// Token: 0x1700001E RID: 30
		// (get) Token: 0x06000070 RID: 112 RVA: 0x00003D3D File Offset: 0x00001F3D
		// (set) Token: 0x06000071 RID: 113 RVA: 0x00003D45 File Offset: 0x00001F45
		public ColorValue ColorVal
		{
			[CompilerGenerated]
			get
			{
				return this.<ColorVal>k__BackingField;
			}
			[CompilerGenerated]
			private set
			{
				this.<ColorVal>k__BackingField = value;
			}
		} = new ColorValue();

		// Token: 0x1700001F RID: 31
		// (get) Token: 0x06000072 RID: 114 RVA: 0x00003D4E File Offset: 0x00001F4E
		// (set) Token: 0x06000073 RID: 115 RVA: 0x00003D56 File Offset: 0x00001F56
		public Color EditorsColor
		{
			get
			{
				return this.editorsColor;
			}
			set
			{
				this.editorsColor = value;
				this.ValueChanged("EditorsColor");
			}
		}

		// Token: 0x17000020 RID: 32
		// (get) Token: 0x06000074 RID: 116 RVA: 0x00003D6A File Offset: 0x00001F6A
		// (set) Token: 0x06000075 RID: 117 RVA: 0x00003D72 File Offset: 0x00001F72
		public Color ColorPreviewBorderColor
		{
			get
			{
				return this.colorPreviewBorderColor;
			}
			set
			{
				this.colorPreviewBorderColor = value;
				this.ValueChanged("ColorPreviewBorderColor");
			}
		}

		// Token: 0x17000021 RID: 33
		// (get) Token: 0x06000076 RID: 118 RVA: 0x00003D86 File Offset: 0x00001F86
		// (set) Token: 0x06000077 RID: 119 RVA: 0x00003D93 File Offset: 0x00001F93
		public bool EditAlpha
		{
			get
			{
				return this.ColorVal.EditAlpha;
			}
			set
			{
				this.ColorVal.EditAlpha = value;
				this.ColorVal.EditAlpha = value;
				this.ValueChanged("EditAlpha");
			}
		}

		// Token: 0x06000078 RID: 120 RVA: 0x00003DB8 File Offset: 0x00001FB8
		private static void ColorChanged(BindableObject bindable, object oldValue, object newValue)
		{
			if ((Color)oldValue != (Color)newValue)
			{
				((ColorPickerMixer)bindable).ColorVal.Value = (Color)newValue;
			}
		}

		// Token: 0x17000022 RID: 34
		// (get) Token: 0x06000079 RID: 121 RVA: 0x00003DE3 File Offset: 0x00001FE3
		// (set) Token: 0x0600007A RID: 122 RVA: 0x00003DF5 File Offset: 0x00001FF5
		public Color Color
		{
			get
			{
				return (Color)base.GetValue(ColorPickerMixer.ColorProperty);
			}
			set
			{
				if (this.Color != value)
				{
					base.SetValue(ColorPickerMixer.ColorProperty, value);
				}
			}
		}

		// Token: 0x0600007B RID: 123 RVA: 0x00003E16 File Offset: 0x00002016
		private static void TextColorChanged(BindableObject bindable, object oldValue, object newValue)
		{
			if ((Color)oldValue != (Color)newValue)
			{
				((ColorPickerMixer)bindable).TextColor = (Color)newValue;
			}
		}

		// Token: 0x17000023 RID: 35
		// (get) Token: 0x0600007C RID: 124 RVA: 0x00003E3C File Offset: 0x0000203C
		// (set) Token: 0x0600007D RID: 125 RVA: 0x00003E4E File Offset: 0x0000204E
		public Color TextColor
		{
			get
			{
				return (Color)base.GetValue(ColorPickerMixer.TextColorProperty);
			}
			set
			{
				if (this.TextColor != value)
				{
					base.SetValue(ColorPickerMixer.TextColorProperty, value);
				}
				this.ValueChanged("TextColor");
			}
		}

		// Token: 0x0600007E RID: 126 RVA: 0x00003E7A File Offset: 0x0000207A
		private void ValueChanged([CallerMemberName] string propName = null)
		{
			this.OnPropertyChanged(propName);
		}

		// Token: 0x0600007F RID: 127 RVA: 0x00003E84 File Offset: 0x00002084
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private void InitializeComponent()
		{
			if (ResourceLoader.IsEnabled && ResourceLoader.CanProvideContentFor(new ResourceLoader.ResourceLoadingQuery
			{
				AssemblyName = typeof(ColorPickerMixer).GetTypeInfo().Assembly.GetName(),
				ResourcePath = "UserControls/ColorMixer/ColorPickerMixer.xaml",
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
			ByteToStrConverter byteToStrConverter;
			VisualDiagnostics.RegisterSourceInfo(byteToStrConverter = new ByteToStrConverter(), new Uri("UserControls\\ColorMixer\\ColorPickerMixer.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 15, 22);
			Setter setter;
			VisualDiagnostics.RegisterSourceInfo(setter = new Setter(), new Uri("UserControls\\ColorMixer\\ColorPickerMixer.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 17, 26);
			Setter setter2;
			VisualDiagnostics.RegisterSourceInfo(setter2 = new Setter(), new Uri("UserControls\\ColorMixer\\ColorPickerMixer.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 18, 26);
			Setter setter3;
			VisualDiagnostics.RegisterSourceInfo(setter3 = new Setter(), new Uri("UserControls\\ColorMixer\\ColorPickerMixer.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 19, 26);
			Style style;
			VisualDiagnostics.RegisterSourceInfo(style = new Style(typeof(Slider)), new Uri("UserControls\\ColorMixer\\ColorPickerMixer.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 16, 22);
			Setter setter4;
			VisualDiagnostics.RegisterSourceInfo(setter4 = new Setter(), new Uri("UserControls\\ColorMixer\\ColorPickerMixer.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 22, 26);
			BindingExtension bindingExtension;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension = new BindingExtension(), new Uri("UserControls\\ColorMixer\\ColorPickerMixer.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 23, 54);
			Setter setter5;
			VisualDiagnostics.RegisterSourceInfo(setter5 = new Setter(), new Uri("UserControls\\ColorMixer\\ColorPickerMixer.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 23, 26);
			Style style2;
			VisualDiagnostics.RegisterSourceInfo(style2 = new Style(typeof(Label)), new Uri("UserControls\\ColorMixer\\ColorPickerMixer.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 21, 22);
			Setter setter6;
			VisualDiagnostics.RegisterSourceInfo(setter6 = new Setter(), new Uri("UserControls\\ColorMixer\\ColorPickerMixer.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 26, 26);
			BindingExtension bindingExtension2;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension2 = new BindingExtension(), new Uri("UserControls\\ColorMixer\\ColorPickerMixer.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 27, 54);
			Setter setter7;
			VisualDiagnostics.RegisterSourceInfo(setter7 = new Setter(), new Uri("UserControls\\ColorMixer\\ColorPickerMixer.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 27, 26);
			BindingExtension bindingExtension3;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension3 = new BindingExtension(), new Uri("UserControls\\ColorMixer\\ColorPickerMixer.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 28, 60);
			Setter setter8;
			VisualDiagnostics.RegisterSourceInfo(setter8 = new Setter(), new Uri("UserControls\\ColorMixer\\ColorPickerMixer.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 28, 26);
			Style style3;
			VisualDiagnostics.RegisterSourceInfo(style3 = new Style(typeof(Entry)), new Uri("UserControls\\ColorMixer\\ColorPickerMixer.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 25, 22);
			ResourceDictionary resourceDictionary;
			VisualDiagnostics.RegisterSourceInfo(resourceDictionary = new ResourceDictionary(), new Uri("UserControls\\ColorMixer\\ColorPickerMixer.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 14, 18);
			ColumnDefinition columnDefinition;
			VisualDiagnostics.RegisterSourceInfo(columnDefinition = new ColumnDefinition(), new Uri("UserControls\\ColorMixer\\ColorPickerMixer.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 34, 18);
			ColumnDefinition columnDefinition2;
			VisualDiagnostics.RegisterSourceInfo(columnDefinition2 = new ColumnDefinition(), new Uri("UserControls\\ColorMixer\\ColorPickerMixer.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 35, 18);
			RowDefinition rowDefinition;
			VisualDiagnostics.RegisterSourceInfo(rowDefinition = new RowDefinition(), new Uri("UserControls\\ColorMixer\\ColorPickerMixer.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 39, 18);
			RowDefinition rowDefinition2;
			VisualDiagnostics.RegisterSourceInfo(rowDefinition2 = new RowDefinition(), new Uri("UserControls\\ColorMixer\\ColorPickerMixer.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 40, 18);
			BindingExtension bindingExtension4;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension4 = new BindingExtension(), new Uri("UserControls\\ColorMixer\\ColorPickerMixer.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 67, 21);
			BindingExtension bindingExtension5;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension5 = new BindingExtension(), new Uri("UserControls\\ColorMixer\\ColorPickerMixer.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 71, 21);
			Slider slider;
			VisualDiagnostics.RegisterSourceInfo(slider = new Slider(), new Uri("UserControls\\ColorMixer\\ColorPickerMixer.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 65, 18);
			BindingExtension bindingExtension6;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension6 = new BindingExtension(), new Uri("UserControls\\ColorMixer\\ColorPickerMixer.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 77, 21);
			Slider slider2;
			VisualDiagnostics.RegisterSourceInfo(slider2 = new Slider(), new Uri("UserControls\\ColorMixer\\ColorPickerMixer.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 72, 18);
			BindingExtension bindingExtension7;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension7 = new BindingExtension(), new Uri("UserControls\\ColorMixer\\ColorPickerMixer.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 83, 21);
			Slider slider3;
			VisualDiagnostics.RegisterSourceInfo(slider3 = new Slider(), new Uri("UserControls\\ColorMixer\\ColorPickerMixer.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 78, 18);
			BindingExtension bindingExtension8;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension8 = new BindingExtension(), new Uri("UserControls\\ColorMixer\\ColorPickerMixer.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 89, 21);
			Slider slider4;
			VisualDiagnostics.RegisterSourceInfo(slider4 = new Slider(), new Uri("UserControls\\ColorMixer\\ColorPickerMixer.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 84, 18);
			StackLayout stackLayout;
			VisualDiagnostics.RegisterSourceInfo(stackLayout = new StackLayout(), new Uri("UserControls\\ColorMixer\\ColorPickerMixer.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 56, 14);
			Grid grid;
			VisualDiagnostics.RegisterSourceInfo(grid = new Grid(), new Uri("UserControls\\ColorMixer\\ColorPickerMixer.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 9, 10);
			VisualDiagnostics.RegisterSourceInfo(this, new Uri("UserControls\\ColorMixer\\ColorPickerMixer.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 2, 2);
			NameScope nameScope = NameScope.GetNameScope(this) ?? new NameScope();
			NameScope.SetNameScope(this, nameScope);
			nameScope.RegisterName("pContent", this);
			if (this.StyleId == null)
			{
				this.StyleId = "pContent";
			}
			nameScope.RegisterName("gMain", grid);
			if (grid.StyleId == null)
			{
				grid.StyleId = "gMain";
			}
			NameScope nameScope2 = new NameScope();
			NameScope nameScope3 = new NameScope();
			NameScope nameScope4 = new NameScope();
			NameScope nameScope5 = new NameScope();
			NameScope nameScope6 = new NameScope();
			NameScope nameScope7 = new NameScope();
			NameScope nameScope8 = new NameScope();
			NameScope nameScope9 = new NameScope();
			nameScope.RegisterName("sMixerControl", stackLayout);
			if (stackLayout.StyleId == null)
			{
				stackLayout.StyleId = "sMixerControl";
			}
			this.pContent = this;
			this.gMain = grid;
			this.sMixerControl = stackLayout;
			grid.Resources = resourceDictionary;
			resourceDictionary.Add("cnvNumber", byteToStrConverter);
			setter.Property = Slider.MinimumProperty;
			setter.Value = "0";
			setter.Value = 0.0;
			style.Setters.Add(setter);
			setter2.Property = Slider.MaximumProperty;
			setter2.Value = "255";
			setter2.Value = 255.0;
			style.Setters.Add(setter2);
			setter3.Property = View.VerticalOptionsProperty;
			setter3.Value = "Center";
			setter3.Value = LayoutOptions.Center;
			style.Setters.Add(setter3);
			resourceDictionary.Add(style);
			setter4.Property = View.VerticalOptionsProperty;
			setter4.Value = "Center";
			setter4.Value = LayoutOptions.Center;
			style2.Setters.Add(setter4);
			setter5.Property = Label.TextColorProperty;
			bindingExtension.Path = "TextColor";
			BindingBase bindingBase = bindingExtension.ProvideValue(null);
			setter5.Value = bindingBase;
			style2.Setters.Add(setter5);
			resourceDictionary.Add(style2);
			setter6.Property = View.VerticalOptionsProperty;
			setter6.Value = "Center";
			setter6.Value = LayoutOptions.Center;
			style3.Setters.Add(setter6);
			setter7.Property = Entry.TextColorProperty;
			bindingExtension2.Path = "TextColor";
			BindingBase bindingBase2 = bindingExtension2.ProvideValue(null);
			setter7.Value = bindingBase2;
			style3.Setters.Add(setter7);
			setter8.Property = VisualElement.BackgroundColorProperty;
			bindingExtension3.Path = "EditorsColor";
			BindingBase bindingBase3 = bindingExtension3.ProvideValue(null);
			setter8.Value = bindingBase3;
			style3.Setters.Add(setter8);
			resourceDictionary.Add(style3);
			grid.SetValue(Layout.PaddingProperty, new Thickness(0.0));
			grid.SetValue(View.VerticalOptionsProperty, LayoutOptions.StartAndExpand);
			grid.Resources = resourceDictionary;
			columnDefinition.SetValue(ColumnDefinition.WidthProperty, new GridLengthTypeConverter().ConvertFromInvariantString("5*"));
			grid.GetValue(Grid.ColumnDefinitionsProperty).Add(columnDefinition);
			columnDefinition2.SetValue(ColumnDefinition.WidthProperty, new GridLengthTypeConverter().ConvertFromInvariantString("5*"));
			grid.GetValue(Grid.ColumnDefinitionsProperty).Add(columnDefinition2);
			rowDefinition.SetValue(RowDefinition.HeightProperty, new GridLengthTypeConverter().ConvertFromInvariantString("auto"));
			grid.GetValue(Grid.RowDefinitionsProperty).Add(rowDefinition);
			rowDefinition2.SetValue(RowDefinition.HeightProperty, new GridLengthTypeConverter().ConvertFromInvariantString("*"));
			grid.GetValue(Grid.RowDefinitionsProperty).Add(rowDefinition2);
			stackLayout.SetValue(Grid.RowProperty, 1);
			stackLayout.SetValue(Grid.RowSpanProperty, 1);
			stackLayout.SetValue(Grid.ColumnProperty, 0);
			stackLayout.SetValue(Grid.ColumnSpanProperty, 2);
			stackLayout.SetValue(View.HorizontalOptionsProperty, LayoutOptions.FillAndExpand);
			stackLayout.SetValue(StackLayout.SpacingProperty, 0.0);
			stackLayout.SetValue(View.VerticalOptionsProperty, LayoutOptions.StartAndExpand);
			slider.SetValue(View.MarginProperty, new Thickness(0.0, 10.0, 0.0, 10.0));
			bindingExtension4.Path = "EditAlpha";
			BindingBase bindingBase4 = bindingExtension4.ProvideValue(null);
			slider.SetBinding(VisualElement.IsVisibleProperty, bindingBase4);
			slider.SetValue(Slider.MaximumTrackColorProperty, Color.LightGray);
			slider.SetValue(Slider.MinimumTrackColorProperty, Color.Black);
			slider.SetValue(Slider.ThumbColorProperty, Color.Black);
			bindingExtension5.Mode = 1;
			bindingExtension5.Path = "ColorVal.A";
			BindingBase bindingBase5 = bindingExtension5.ProvideValue(null);
			slider.SetBinding(Slider.ValueProperty, bindingBase5);
			stackLayout.Children.Add(slider);
			slider2.SetValue(View.MarginProperty, new Thickness(0.0, 10.0, 0.0, 10.0));
			slider2.SetValue(Slider.MaximumTrackColorProperty, Color.LightGray);
			slider2.SetValue(Slider.MinimumTrackColorProperty, Color.Red);
			slider2.SetValue(Slider.ThumbColorProperty, Color.Red);
			bindingExtension6.Mode = 1;
			bindingExtension6.Path = "ColorVal.R";
			BindingBase bindingBase6 = bindingExtension6.ProvideValue(null);
			slider2.SetBinding(Slider.ValueProperty, bindingBase6);
			stackLayout.Children.Add(slider2);
			slider3.SetValue(View.MarginProperty, new Thickness(0.0, 10.0, 0.0, 10.0));
			slider3.SetValue(Slider.MaximumTrackColorProperty, Color.LightGray);
			slider3.SetValue(Slider.MinimumTrackColorProperty, Color.Green);
			slider3.SetValue(Slider.ThumbColorProperty, Color.Green);
			bindingExtension7.Mode = 1;
			bindingExtension7.Path = "ColorVal.G";
			BindingBase bindingBase7 = bindingExtension7.ProvideValue(null);
			slider3.SetBinding(Slider.ValueProperty, bindingBase7);
			stackLayout.Children.Add(slider3);
			slider4.SetValue(View.MarginProperty, new Thickness(0.0, 10.0, 0.0, 10.0));
			slider4.SetValue(Slider.MaximumTrackColorProperty, Color.LightGray);
			slider4.SetValue(Slider.MinimumTrackColorProperty, Color.Blue);
			slider4.SetValue(Slider.ThumbColorProperty, Color.Blue);
			bindingExtension8.Mode = 1;
			bindingExtension8.Path = "ColorVal.B";
			BindingBase bindingBase8 = bindingExtension8.ProvideValue(null);
			slider4.SetBinding(Slider.ValueProperty, bindingBase8);
			stackLayout.Children.Add(slider4);
			grid.Children.Add(stackLayout);
			this.SetValue(ContentView.ContentProperty, grid);
		}

		// Token: 0x06000080 RID: 128 RVA: 0x00004BD4 File Offset: 0x00002DD4
		// Note: this type is marked as 'beforefieldinit'.
		static ColorPickerMixer()
		{
		}

		// Token: 0x06000081 RID: 129 RVA: 0x00004C60 File Offset: 0x00002E60
		[CompilerGenerated]
		private void <.ctor>b__0_0(object s, PropertyChangedEventArgs e)
		{
			string propertyName = e.PropertyName;
			if (propertyName == "Value")
			{
				this.Color = ((ColorValue)s).Value;
				return;
			}
			if (!(propertyName == "EditAlpha"))
			{
				return;
			}
			this.ValueChanged("EditAlpha");
		}

		// Token: 0x06000082 RID: 130 RVA: 0x00004CAC File Offset: 0x00002EAC
		private void __InitComponentRuntime()
		{
			Extensions.LoadFromXaml<ColorPickerMixer>(this, typeof(ColorPickerMixer));
			this.pContent = NameScopeExtensions.FindByName<ContentView>(this, "pContent");
			this.gMain = NameScopeExtensions.FindByName<Grid>(this, "gMain");
			this.sMixerControl = NameScopeExtensions.FindByName<StackLayout>(this, "sMixerControl");
		}

		// Token: 0x0400004F RID: 79
		[CompilerGenerated]
		private ColorValue <ColorVal>k__BackingField;

		// Token: 0x04000050 RID: 80
		private Color editorsColor = Color.White;

		// Token: 0x04000051 RID: 81
		private Color colorPreviewBorderColor = Color.Black;

		// Token: 0x04000052 RID: 82
		public static readonly BindableProperty ColorProperty = BindableProperty.Create("Color", typeof(Color), typeof(ColorPickerEntry), Color.White, 1, null, new BindableProperty.BindingPropertyChangedDelegate(ColorPickerMixer.ColorChanged), null, null, null);

		// Token: 0x04000053 RID: 83
		public static readonly BindableProperty TextColorProperty = BindableProperty.Create("TextColor", typeof(Color), typeof(ColorPickerEntry), Color.Black, 1, null, new BindableProperty.BindingPropertyChangedDelegate(ColorPickerMixer.TextColorChanged), null, null, null);

		// Token: 0x04000054 RID: 84
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private ContentView pContent;

		// Token: 0x04000055 RID: 85
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private Grid gMain;

		// Token: 0x04000056 RID: 86
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private StackLayout sMixerControl;
	}
}
