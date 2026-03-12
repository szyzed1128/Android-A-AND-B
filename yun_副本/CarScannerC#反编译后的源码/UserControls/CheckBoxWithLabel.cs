using System;
using System.CodeDom.Compiler;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Threading;
using Xamarin.Forms;
using Xamarin.Forms.Internals;
using Xamarin.Forms.Xaml;
using Xamarin.Forms.Xaml.Diagnostics;
using Xamarin.Forms.Xaml.Internals;

namespace CarScannerXamarinForms.UserControls
{
	// Token: 0x020005C7 RID: 1479
	[XamlCompilation(2)]
	[XamlFilePath("UserControls\\CheckBoxWithLabel.xaml")]
	public class CheckBoxWithLabel : ContentView
	{
		// Token: 0x06003535 RID: 13621 RVA: 0x00263264 File Offset: 0x00261464
		public CheckBoxWithLabel()
		{
			this.InitializeComponent();
			this.label.Text = this.Text;
			this.sw.IsChecked = this.IsToggled;
			this.sw.Color = this.CheckColor;
		}

		// Token: 0x17001358 RID: 4952
		// (get) Token: 0x06003536 RID: 13622 RVA: 0x002632B0 File Offset: 0x002614B0
		public Label Label
		{
			get
			{
				return this.label;
			}
		}

		// Token: 0x14000030 RID: 48
		// (add) Token: 0x06003537 RID: 13623 RVA: 0x002632B8 File Offset: 0x002614B8
		// (remove) Token: 0x06003538 RID: 13624 RVA: 0x002632F0 File Offset: 0x002614F0
		public event EventHandler<ToggledEventArgs> Toggled
		{
			[CompilerGenerated]
			add
			{
				EventHandler<ToggledEventArgs> eventHandler = this.Toggled;
				EventHandler<ToggledEventArgs> eventHandler2;
				do
				{
					eventHandler2 = eventHandler;
					EventHandler<ToggledEventArgs> eventHandler3 = (EventHandler<ToggledEventArgs>)Delegate.Combine(eventHandler2, value);
					eventHandler = Interlocked.CompareExchange<EventHandler<ToggledEventArgs>>(ref this.Toggled, eventHandler3, eventHandler2);
				}
				while (eventHandler != eventHandler2);
			}
			[CompilerGenerated]
			remove
			{
				EventHandler<ToggledEventArgs> eventHandler = this.Toggled;
				EventHandler<ToggledEventArgs> eventHandler2;
				do
				{
					eventHandler2 = eventHandler;
					EventHandler<ToggledEventArgs> eventHandler3 = (EventHandler<ToggledEventArgs>)Delegate.Remove(eventHandler2, value);
					eventHandler = Interlocked.CompareExchange<EventHandler<ToggledEventArgs>>(ref this.Toggled, eventHandler3, eventHandler2);
				}
				while (eventHandler != eventHandler2);
			}
		}

		// Token: 0x06003539 RID: 13625 RVA: 0x00263325 File Offset: 0x00261525
		private static void TextPropertyChanged(BindableObject bindable, object oldValue, object newValue)
		{
			((CheckBoxWithLabel)bindable).label.Text = (string)newValue;
		}

		// Token: 0x17001359 RID: 4953
		// (get) Token: 0x0600353A RID: 13626 RVA: 0x0026333D File Offset: 0x0026153D
		// (set) Token: 0x0600353B RID: 13627 RVA: 0x0026334F File Offset: 0x0026154F
		public string Text
		{
			get
			{
				return (string)base.GetValue(CheckBoxWithLabel.TextProperty);
			}
			set
			{
				base.SetValue(CheckBoxWithLabel.TextProperty, value);
			}
		}

		// Token: 0x0600353C RID: 13628 RVA: 0x00263360 File Offset: 0x00261560
		private static void TextColorPropertyChanged(BindableObject bindable, object oldValue, object newValue)
		{
			CheckBoxWithLabel checkBoxWithLabel = (CheckBoxWithLabel)bindable;
			if (checkBoxWithLabel.label == null)
			{
				return;
			}
			checkBoxWithLabel.label.TextColor = (Color)newValue;
		}

		// Token: 0x1700135A RID: 4954
		// (get) Token: 0x0600353D RID: 13629 RVA: 0x0026338E File Offset: 0x0026158E
		// (set) Token: 0x0600353E RID: 13630 RVA: 0x002633A0 File Offset: 0x002615A0
		public Color TextColor
		{
			get
			{
				return (Color)base.GetValue(CheckBoxWithLabel.TextColorProperty);
			}
			set
			{
				base.SetValue(CheckBoxWithLabel.TextColorProperty, value);
			}
		}

		// Token: 0x0600353F RID: 13631 RVA: 0x002633B4 File Offset: 0x002615B4
		private static void CheckColorPropertyChanged(BindableObject bindable, object oldValue, object newValue)
		{
			CheckBoxWithLabel checkBoxWithLabel = (CheckBoxWithLabel)bindable;
			if (checkBoxWithLabel.sw == null)
			{
				return;
			}
			checkBoxWithLabel.sw.Color = (Color)newValue;
		}

		// Token: 0x1700135B RID: 4955
		// (get) Token: 0x06003540 RID: 13632 RVA: 0x002633E2 File Offset: 0x002615E2
		// (set) Token: 0x06003541 RID: 13633 RVA: 0x002633F4 File Offset: 0x002615F4
		public Color CheckColor
		{
			get
			{
				return (Color)base.GetValue(CheckBoxWithLabel.CheckColorProperty);
			}
			set
			{
				base.SetValue(CheckBoxWithLabel.CheckColorProperty, value);
			}
		}

		// Token: 0x06003542 RID: 13634 RVA: 0x00263408 File Offset: 0x00261608
		private static void IsToggledPropertyChanged(BindableObject bindable, object oldValue, object newValue)
		{
			CheckBoxWithLabel checkBoxWithLabel = (CheckBoxWithLabel)bindable;
			bool flag = (bool)newValue;
			checkBoxWithLabel.sw.IsChecked = flag;
		}

		// Token: 0x06003543 RID: 13635 RVA: 0x0026342D File Offset: 0x0026162D
		private void sw_Toggled(object sender, ToggledEventArgs args)
		{
			base.SetValue(CheckBoxWithLabel.IsToggledProperty, this.sw.IsChecked);
			EventHandler<ToggledEventArgs> toggled = this.Toggled;
			if (toggled == null)
			{
				return;
			}
			toggled(this, args);
		}

		// Token: 0x1700135C RID: 4956
		// (get) Token: 0x06003544 RID: 13636 RVA: 0x0026345C File Offset: 0x0026165C
		// (set) Token: 0x06003545 RID: 13637 RVA: 0x0026346E File Offset: 0x0026166E
		public bool IsToggled
		{
			get
			{
				return (bool)base.GetValue(CheckBoxWithLabel.IsToggledProperty);
			}
			set
			{
				base.SetValue(CheckBoxWithLabel.IsToggledProperty, value);
			}
		}

		// Token: 0x06003546 RID: 13638 RVA: 0x00263481 File Offset: 0x00261681
		private void cb_CheckedChanged(object sender, CheckedChangedEventArgs e)
		{
			base.SetValue(CheckBoxWithLabel.IsToggledProperty, this.sw.IsChecked);
			EventHandler<ToggledEventArgs> toggled = this.Toggled;
			if (toggled == null)
			{
				return;
			}
			toggled(this, new ToggledEventArgs(e.Value));
		}

		// Token: 0x06003547 RID: 13639 RVA: 0x002634BA File Offset: 0x002616BA
		private void TapGestureRecognizer_Tapped(object sender, EventArgs e)
		{
			this.IsToggled = !this.IsToggled;
		}

		// Token: 0x06003548 RID: 13640 RVA: 0x002634CC File Offset: 0x002616CC
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private void InitializeComponent()
		{
			if (ResourceLoader.IsEnabled && ResourceLoader.CanProvideContentFor(new ResourceLoader.ResourceLoadingQuery
			{
				AssemblyName = typeof(CheckBoxWithLabel).GetTypeInfo().Assembly.GetName(),
				ResourcePath = "UserControls/CheckBoxWithLabel.xaml",
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
			ColumnDefinition columnDefinition;
			VisualDiagnostics.RegisterSourceInfo(columnDefinition = new ColumnDefinition(), new Uri("UserControls\\CheckBoxWithLabel.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 10, 18);
			ColumnDefinition columnDefinition2;
			VisualDiagnostics.RegisterSourceInfo(columnDefinition2 = new ColumnDefinition(), new Uri("UserControls\\CheckBoxWithLabel.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 11, 18);
			CheckBoxWithColor checkBoxWithColor;
			VisualDiagnostics.RegisterSourceInfo(checkBoxWithColor = new CheckBoxWithColor(), new Uri("UserControls\\CheckBoxWithLabel.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 13, 14);
			TapGestureRecognizer tapGestureRecognizer;
			VisualDiagnostics.RegisterSourceInfo(tapGestureRecognizer = new TapGestureRecognizer(), new Uri("UserControls\\CheckBoxWithLabel.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 24, 22);
			Label label;
			VisualDiagnostics.RegisterSourceInfo(label = new Label(), new Uri("UserControls\\CheckBoxWithLabel.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 18, 14);
			Grid grid;
			VisualDiagnostics.RegisterSourceInfo(grid = new Grid(), new Uri("UserControls\\CheckBoxWithLabel.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 8, 10);
			VisualDiagnostics.RegisterSourceInfo(this, new Uri("UserControls\\CheckBoxWithLabel.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 2, 2);
			NameScope nameScope = NameScope.GetNameScope(this) ?? new NameScope();
			NameScope.SetNameScope(this, nameScope);
			nameScope.RegisterName("sw", checkBoxWithColor);
			if (checkBoxWithColor.StyleId == null)
			{
				checkBoxWithColor.StyleId = "sw";
			}
			nameScope.RegisterName("label", label);
			if (label.StyleId == null)
			{
				label.StyleId = "label";
			}
			this.sw = checkBoxWithColor;
			this.label = label;
			columnDefinition.SetValue(ColumnDefinition.WidthProperty, new GridLengthTypeConverter().ConvertFromInvariantString("auto"));
			grid.GetValue(Grid.ColumnDefinitionsProperty).Add(columnDefinition);
			columnDefinition2.SetValue(ColumnDefinition.WidthProperty, new GridLengthTypeConverter().ConvertFromInvariantString("*"));
			grid.GetValue(Grid.ColumnDefinitionsProperty).Add(columnDefinition2);
			checkBoxWithColor.SetValue(Grid.ColumnProperty, 0);
			checkBoxWithColor.CheckedChanged += this.cb_CheckedChanged;
			checkBoxWithColor.SetValue(View.VerticalOptionsProperty, LayoutOptions.Center);
			grid.Children.Add(checkBoxWithColor);
			label.SetValue(Grid.ColumnProperty, 1);
			label.SetValue(Label.TextProperty, "");
			label.SetValue(View.VerticalOptionsProperty, LayoutOptions.Center);
			tapGestureRecognizer.SetValue(TapGestureRecognizer.NumberOfTapsRequiredProperty, 1);
			tapGestureRecognizer.Tapped += this.TapGestureRecognizer_Tapped;
			label.GestureRecognizers.Add(tapGestureRecognizer);
			grid.Children.Add(label);
			this.SetValue(ContentView.ContentProperty, grid);
		}

		// Token: 0x06003549 RID: 13641 RVA: 0x002637FC File Offset: 0x002619FC
		// Note: this type is marked as 'beforefieldinit'.
		static CheckBoxWithLabel()
		{
			string text = "CheckColor";
			Type typeFromHandle = typeof(Color);
			Type typeFromHandle2 = typeof(CheckBoxWithLabel);
			BindableProperty.BindingPropertyChangedDelegate bindingPropertyChangedDelegate = new BindableProperty.BindingPropertyChangedDelegate(CheckBoxWithLabel.CheckColorPropertyChanged);
			CheckBoxWithLabel.CheckColorProperty = BindableProperty.Create(text, typeFromHandle, typeFromHandle2, Color.Blue, 2, null, bindingPropertyChangedDelegate, null, null, null);
			CheckBoxWithLabel.IsToggledProperty = BindableProperty.Create("IsToggled", typeof(bool), typeof(CheckBoxWithLabel), null, 1, null, new BindableProperty.BindingPropertyChangedDelegate(CheckBoxWithLabel.IsToggledPropertyChanged), null, null, null);
		}

		// Token: 0x0600354A RID: 13642 RVA: 0x002638E8 File Offset: 0x00261AE8
		private void __InitComponentRuntime()
		{
			Extensions.LoadFromXaml<CheckBoxWithLabel>(this, typeof(CheckBoxWithLabel));
			this.sw = NameScopeExtensions.FindByName<CheckBoxWithColor>(this, "sw");
			this.label = NameScopeExtensions.FindByName<Label>(this, "label");
		}

		// Token: 0x04001FAD RID: 8109
		[CompilerGenerated]
		private EventHandler<ToggledEventArgs> Toggled;

		// Token: 0x04001FAE RID: 8110
		public static readonly BindableProperty TextProperty = BindableProperty.Create("Text", typeof(string), typeof(CheckBoxWithLabel), null, 2, null, new BindableProperty.BindingPropertyChangedDelegate(CheckBoxWithLabel.TextPropertyChanged), null, null, null);

		// Token: 0x04001FAF RID: 8111
		public static readonly BindableProperty TextColorProperty = BindableProperty.Create("TextColor", typeof(Color), typeof(CheckBoxWithLabel), null, 2, null, new BindableProperty.BindingPropertyChangedDelegate(CheckBoxWithLabel.TextColorPropertyChanged), null, null, null);

		// Token: 0x04001FB0 RID: 8112
		public static readonly BindableProperty CheckColorProperty;

		// Token: 0x04001FB1 RID: 8113
		public static readonly BindableProperty IsToggledProperty;

		// Token: 0x04001FB2 RID: 8114
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private CheckBoxWithColor sw;

		// Token: 0x04001FB3 RID: 8115
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private Label label;
	}
}
