using System;
using System.CodeDom.Compiler;
using System.Reflection;
using Xamarin.Forms;
using Xamarin.Forms.Internals;
using Xamarin.Forms.Xaml;
using Xamarin.Forms.Xaml.Diagnostics;
using Xamarin.Forms.Xaml.Internals;

namespace CarScannerXamarinForms
{
	// Token: 0x020001A2 RID: 418
	[XamlFilePath("UserControls\\SliderColorSelector.xaml")]
	public class SliderColorSelector : ContentView
	{
		// Token: 0x17000F6F RID: 3951
		// (get) Token: 0x060016AF RID: 5807 RVA: 0x000A825D File Offset: 0x000A645D
		// (set) Token: 0x060016B0 RID: 5808 RVA: 0x000A8270 File Offset: 0x000A6470
		public Color SelectedColor
		{
			get
			{
				return (Color)base.GetValue(SliderColorSelector.SelectedColorProperty);
			}
			set
			{
				base.SetValue(SliderColorSelector.SelectedColorProperty, value);
				this.RSlider.Value = value.R * 255.0;
				this.GSlider.Value = value.G * 255.0;
				this.BSlider.Value = value.B * 255.0;
			}
		}

		// Token: 0x17000F70 RID: 3952
		// (get) Token: 0x060016B1 RID: 5809 RVA: 0x000A82E2 File Offset: 0x000A64E2
		// (set) Token: 0x060016B2 RID: 5810 RVA: 0x000A82F4 File Offset: 0x000A64F4
		public string Text
		{
			get
			{
				return (string)base.GetValue(SliderColorSelector.TextProperty);
			}
			set
			{
				base.SetValue(SliderColorSelector.TextProperty, value);
			}
		}

		// Token: 0x060016B3 RID: 5811 RVA: 0x000A8302 File Offset: 0x000A6502
		public SliderColorSelector()
		{
			this.InitializeComponent();
			base.Content.BindingContext = this;
		}

		// Token: 0x060016B4 RID: 5812 RVA: 0x000A831C File Offset: 0x000A651C
		private void boxView_Tapped(object sender, EventArgs e)
		{
			this.panelSliders.IsVisible = !this.panelSliders.IsVisible;
		}

		// Token: 0x060016B5 RID: 5813 RVA: 0x000A8338 File Offset: 0x000A6538
		private void RSlider_ValueChanged(object sender, ValueChangedEventArgs e)
		{
			Color selectedColor = this.SelectedColor;
			Color color = Color.FromRgb(this.RSlider.Value / 255.0, selectedColor.G / 255.0, selectedColor.B / 255.0);
			this.SelectedColor = color;
		}

		// Token: 0x060016B6 RID: 5814 RVA: 0x000A8390 File Offset: 0x000A6590
		private void GSlider_ValueChanged(object sender, ValueChangedEventArgs e)
		{
			Color selectedColor = this.SelectedColor;
			Color color;
			color..ctor(selectedColor.R / 255.0, this.GSlider.Value / 255.0, selectedColor.B / 255.0);
			this.SelectedColor = color;
		}

		// Token: 0x060016B7 RID: 5815 RVA: 0x000A83EC File Offset: 0x000A65EC
		private void BSlider_ValueChanged(object sender, ValueChangedEventArgs e)
		{
			Color selectedColor = this.SelectedColor;
			Color color;
			color..ctor(selectedColor.R / 255.0, selectedColor.G / 255.0, this.BSlider.Value / 255.0);
			this.SelectedColor = color;
		}

		// Token: 0x060016B8 RID: 5816 RVA: 0x000A8448 File Offset: 0x000A6648
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private void InitializeComponent()
		{
			if (ResourceLoader.IsEnabled && ResourceLoader.CanProvideContentFor(new ResourceLoader.ResourceLoadingQuery
			{
				AssemblyName = typeof(SliderColorSelector).GetTypeInfo().Assembly.GetName(),
				ResourcePath = "UserControls/SliderColorSelector.xaml",
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
			ColorToHexStringConverter colorToHexStringConverter;
			VisualDiagnostics.RegisterSourceInfo(colorToHexStringConverter = new ColorToHexStringConverter(), new Uri("UserControls\\SliderColorSelector.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 9, 14);
			ResourceDictionary resourceDictionary;
			VisualDiagnostics.RegisterSourceInfo(resourceDictionary = new ResourceDictionary(), new Uri("UserControls\\SliderColorSelector.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 8, 10);
			RowDefinition rowDefinition;
			VisualDiagnostics.RegisterSourceInfo(rowDefinition = new RowDefinition(), new Uri("UserControls\\SliderColorSelector.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 15, 18);
			RowDefinition rowDefinition2;
			VisualDiagnostics.RegisterSourceInfo(rowDefinition2 = new RowDefinition(), new Uri("UserControls\\SliderColorSelector.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 16, 18);
			ColumnDefinition columnDefinition;
			VisualDiagnostics.RegisterSourceInfo(columnDefinition = new ColumnDefinition(), new Uri("UserControls\\SliderColorSelector.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 19, 18);
			ColumnDefinition columnDefinition2;
			VisualDiagnostics.RegisterSourceInfo(columnDefinition2 = new ColumnDefinition(), new Uri("UserControls\\SliderColorSelector.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 20, 18);
			BindingExtension bindingExtension;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension = new BindingExtension(), new Uri("UserControls\\SliderColorSelector.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 25, 17);
			Label label;
			VisualDiagnostics.RegisterSourceInfo(label = new Label(), new Uri("UserControls\\SliderColorSelector.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 22, 14);
			BindingExtension bindingExtension2;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension2 = new BindingExtension(), new Uri("UserControls\\SliderColorSelector.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 32, 17);
			TapGestureRecognizer tapGestureRecognizer;
			VisualDiagnostics.RegisterSourceInfo(tapGestureRecognizer = new TapGestureRecognizer(), new Uri("UserControls\\SliderColorSelector.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 34, 22);
			BoxView boxView;
			VisualDiagnostics.RegisterSourceInfo(boxView = new BoxView(), new Uri("UserControls\\SliderColorSelector.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 27, 14);
			Slider slider;
			VisualDiagnostics.RegisterSourceInfo(slider = new Slider(), new Uri("UserControls\\SliderColorSelector.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 44, 18);
			Slider slider2;
			VisualDiagnostics.RegisterSourceInfo(slider2 = new Slider(), new Uri("UserControls\\SliderColorSelector.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 50, 18);
			Slider slider3;
			VisualDiagnostics.RegisterSourceInfo(slider3 = new Slider(), new Uri("UserControls\\SliderColorSelector.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 56, 18);
			StackLayout stackLayout;
			VisualDiagnostics.RegisterSourceInfo(stackLayout = new StackLayout(), new Uri("UserControls\\SliderColorSelector.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 37, 14);
			Grid grid;
			VisualDiagnostics.RegisterSourceInfo(grid = new Grid(), new Uri("UserControls\\SliderColorSelector.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 13, 10);
			VisualDiagnostics.RegisterSourceInfo(this, new Uri("UserControls\\SliderColorSelector.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 2, 2);
			NameScope nameScope = NameScope.GetNameScope(this) ?? new NameScope();
			NameScope.SetNameScope(this, nameScope);
			nameScope.RegisterName("panelSliders", stackLayout);
			if (stackLayout.StyleId == null)
			{
				stackLayout.StyleId = "panelSliders";
			}
			nameScope.RegisterName("RSlider", slider);
			if (slider.StyleId == null)
			{
				slider.StyleId = "RSlider";
			}
			nameScope.RegisterName("GSlider", slider2);
			if (slider2.StyleId == null)
			{
				slider2.StyleId = "GSlider";
			}
			nameScope.RegisterName("BSlider", slider3);
			if (slider3.StyleId == null)
			{
				slider3.StyleId = "BSlider";
			}
			this.panelSliders = stackLayout;
			this.RSlider = slider;
			this.GSlider = slider2;
			this.BSlider = slider3;
			this.Resources = resourceDictionary;
			resourceDictionary.Add("ColorToHexStringConverter", colorToHexStringConverter);
			this.Resources = resourceDictionary;
			rowDefinition.SetValue(RowDefinition.HeightProperty, new GridLengthTypeConverter().ConvertFromInvariantString("auto"));
			grid.GetValue(Grid.RowDefinitionsProperty).Add(rowDefinition);
			rowDefinition2.SetValue(RowDefinition.HeightProperty, new GridLengthTypeConverter().ConvertFromInvariantString("auto"));
			grid.GetValue(Grid.RowDefinitionsProperty).Add(rowDefinition2);
			columnDefinition.SetValue(ColumnDefinition.WidthProperty, new GridLengthTypeConverter().ConvertFromInvariantString("0.8*"));
			grid.GetValue(Grid.ColumnDefinitionsProperty).Add(columnDefinition);
			columnDefinition2.SetValue(ColumnDefinition.WidthProperty, new GridLengthTypeConverter().ConvertFromInvariantString("0.2*"));
			grid.GetValue(Grid.ColumnDefinitionsProperty).Add(columnDefinition2);
			label.SetValue(Grid.RowProperty, 0);
			label.SetValue(Grid.ColumnProperty, 0);
			bindingExtension.Mode = 2;
			bindingExtension.Path = "Text";
			BindingBase bindingBase = bindingExtension.ProvideValue(null);
			label.SetBinding(Label.TextProperty, bindingBase);
			label.SetValue(View.VerticalOptionsProperty, LayoutOptions.Center);
			grid.Children.Add(label);
			boxView.SetValue(Grid.RowProperty, 0);
			boxView.SetValue(Grid.ColumnProperty, 1);
			boxView.SetValue(VisualElement.MinimumHeightRequestProperty, 40.0);
			boxView.SetValue(View.VerticalOptionsProperty, LayoutOptions.Center);
			bindingExtension2.Mode = 2;
			bindingExtension2.Path = "SelectedColor";
			BindingBase bindingBase2 = bindingExtension2.ProvideValue(null);
			boxView.SetBinding(BoxView.ColorProperty, bindingBase2);
			tapGestureRecognizer.Tapped += this.boxView_Tapped;
			boxView.GestureRecognizers.Add(tapGestureRecognizer);
			grid.Children.Add(boxView);
			stackLayout.SetValue(Grid.RowProperty, 1);
			stackLayout.SetValue(Grid.ColumnProperty, 0);
			stackLayout.SetValue(Grid.ColumnSpanProperty, 2);
			stackLayout.SetValue(VisualElement.IsVisibleProperty, new VisualElement.VisibilityConverter().ConvertFromInvariantString("false"));
			stackLayout.SetValue(StackLayout.OrientationProperty, 0);
			slider.SetValue(VisualElement.BackgroundColorProperty, Color.Red);
			slider.SetValue(Slider.MaximumProperty, 255.0);
			slider.SetValue(Slider.MinimumProperty, 0.0);
			slider.ValueChanged += this.RSlider_ValueChanged;
			stackLayout.Children.Add(slider);
			slider2.SetValue(VisualElement.BackgroundColorProperty, Color.Green);
			slider2.SetValue(Slider.MaximumProperty, 255.0);
			slider2.SetValue(Slider.MinimumProperty, 0.0);
			slider2.ValueChanged += this.GSlider_ValueChanged;
			stackLayout.Children.Add(slider2);
			slider3.SetValue(VisualElement.BackgroundColorProperty, Color.Blue);
			slider3.SetValue(Slider.MaximumProperty, 255.0);
			slider3.SetValue(Slider.MinimumProperty, 0.0);
			slider3.ValueChanged += this.BSlider_ValueChanged;
			stackLayout.Children.Add(slider3);
			grid.Children.Add(stackLayout);
			this.SetValue(ContentView.ContentProperty, grid);
		}

		// Token: 0x060016B9 RID: 5817 RVA: 0x000A8C18 File Offset: 0x000A6E18
		// Note: this type is marked as 'beforefieldinit'.
		static SliderColorSelector()
		{
		}

		// Token: 0x060016BA RID: 5818 RVA: 0x000A8C88 File Offset: 0x000A6E88
		private void __InitComponentRuntime()
		{
			Extensions.LoadFromXaml<SliderColorSelector>(this, typeof(SliderColorSelector));
			this.panelSliders = NameScopeExtensions.FindByName<StackLayout>(this, "panelSliders");
			this.RSlider = NameScopeExtensions.FindByName<Slider>(this, "RSlider");
			this.GSlider = NameScopeExtensions.FindByName<Slider>(this, "GSlider");
			this.BSlider = NameScopeExtensions.FindByName<Slider>(this, "BSlider");
		}

		// Token: 0x04000999 RID: 2457
		public static BindableProperty SelectedColorProperty = BindableProperty.Create("SelectedColor", typeof(Color), typeof(SliderColorSelector), Color.White, 1, null, null, null, null, null);

		// Token: 0x0400099A RID: 2458
		public static BindableProperty TextProperty = BindableProperty.Create("Text", typeof(string), typeof(SliderColorSelector), "", 2, null, null, null, null, null);

		// Token: 0x0400099B RID: 2459
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private StackLayout panelSliders;

		// Token: 0x0400099C RID: 2460
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private Slider RSlider;

		// Token: 0x0400099D RID: 2461
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private Slider GSlider;

		// Token: 0x0400099E RID: 2462
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private Slider BSlider;
	}
}
