using System;
using System.CodeDom.Compiler;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Threading;
using CarScannerXamarinForms.UserControls;
using Xamarin.Forms;
using Xamarin.Forms.Internals;
using Xamarin.Forms.Xaml;
using Xamarin.Forms.Xaml.Diagnostics;
using Xamarin.Forms.Xaml.Internals;

namespace CarScannerXamarinForms
{
	// Token: 0x0200019D RID: 413
	[XamlCompilation(2)]
	[XamlFilePath("UserControls\\LabelSwitch.xaml")]
	public class LabelSwitch : ContentView
	{
		// Token: 0x0600165C RID: 5724 RVA: 0x0009FEA4 File Offset: 0x0009E0A4
		public LabelSwitch()
		{
			this.InitializeComponent();
			this.label.Text = this.Text;
			if (this.sw.IsVisible)
			{
				this.sw.IsToggled = this.IsToggled;
			}
			if (this.cb.IsVisible)
			{
				this.cb.IsChecked = this.IsToggled;
			}
		}

		// Token: 0x17000F69 RID: 3945
		// (get) Token: 0x0600165D RID: 5725 RVA: 0x0009FF0A File Offset: 0x0009E10A
		public Label Label
		{
			get
			{
				return this.label;
			}
		}

		// Token: 0x1400000E RID: 14
		// (add) Token: 0x0600165E RID: 5726 RVA: 0x0009FF14 File Offset: 0x0009E114
		// (remove) Token: 0x0600165F RID: 5727 RVA: 0x0009FF4C File Offset: 0x0009E14C
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

		// Token: 0x06001660 RID: 5728 RVA: 0x0009FF81 File Offset: 0x0009E181
		private static void TextPropertyChanged(BindableObject bindable, object oldValue, object newValue)
		{
			((LabelSwitch)bindable).label.Text = (string)newValue;
		}

		// Token: 0x17000F6A RID: 3946
		// (get) Token: 0x06001661 RID: 5729 RVA: 0x0009FF99 File Offset: 0x0009E199
		// (set) Token: 0x06001662 RID: 5730 RVA: 0x0009FFAB File Offset: 0x0009E1AB
		public string Text
		{
			get
			{
				return (string)base.GetValue(LabelSwitch.TextProperty);
			}
			set
			{
				base.SetValue(LabelSwitch.TextProperty, value);
			}
		}

		// Token: 0x06001663 RID: 5731 RVA: 0x0009FFBC File Offset: 0x0009E1BC
		private static void IsToggledPropertyChanged(BindableObject bindable, object oldValue, object newValue)
		{
			LabelSwitch labelSwitch = (LabelSwitch)bindable;
			bool flag = (bool)newValue;
			if (labelSwitch.sw.IsVisible)
			{
				labelSwitch.sw.IsToggled = flag;
			}
			if (labelSwitch.cb.IsVisible)
			{
				labelSwitch.cb.IsChecked = flag;
			}
		}

		// Token: 0x06001664 RID: 5732 RVA: 0x000A0009 File Offset: 0x0009E209
		private void sw_Toggled(object sender, ToggledEventArgs args)
		{
			base.SetValue(LabelSwitch.IsToggledProperty, this.sw.IsToggled);
			EventHandler<ToggledEventArgs> toggled = this.Toggled;
			if (toggled == null)
			{
				return;
			}
			toggled(this, args);
		}

		// Token: 0x17000F6B RID: 3947
		// (get) Token: 0x06001665 RID: 5733 RVA: 0x000A0038 File Offset: 0x0009E238
		// (set) Token: 0x06001666 RID: 5734 RVA: 0x000A004A File Offset: 0x0009E24A
		public bool IsToggled
		{
			get
			{
				return (bool)base.GetValue(LabelSwitch.IsToggledProperty);
			}
			set
			{
				base.SetValue(LabelSwitch.IsToggledProperty, value);
			}
		}

		// Token: 0x06001667 RID: 5735 RVA: 0x000A005D File Offset: 0x0009E25D
		private void cb_CheckedChanged(object sender, CheckedChangedEventArgs e)
		{
			base.SetValue(LabelSwitch.IsToggledProperty, this.cb.IsChecked);
			EventHandler<ToggledEventArgs> toggled = this.Toggled;
			if (toggled == null)
			{
				return;
			}
			toggled(this, new ToggledEventArgs(e.Value));
		}

		// Token: 0x06001668 RID: 5736 RVA: 0x000A0096 File Offset: 0x0009E296
		private void TapGestureRecognizer_Tapped(object sender, EventArgs e)
		{
			this.IsToggled = !this.IsToggled;
		}

		// Token: 0x06001669 RID: 5737 RVA: 0x000A00A7 File Offset: 0x0009E2A7
		private static void TextColorPropertyChanged(BindableObject bindable, object oldValue, object newValue)
		{
			((LabelSwitch)bindable).label.TextColor = (Color)newValue;
		}

		// Token: 0x17000F6C RID: 3948
		// (get) Token: 0x0600166A RID: 5738 RVA: 0x000A00BF File Offset: 0x0009E2BF
		// (set) Token: 0x0600166B RID: 5739 RVA: 0x000A00D1 File Offset: 0x0009E2D1
		public Color TextColor
		{
			get
			{
				return (Color)base.GetValue(LabelSwitch.TextColorProperty);
			}
			set
			{
				base.SetValue(LabelSwitch.TextColorProperty, value);
			}
		}

		// Token: 0x17000F6D RID: 3949
		// (get) Token: 0x0600166C RID: 5740 RVA: 0x0009FF0A File Offset: 0x0009E10A
		public Label TextLabel
		{
			get
			{
				return this.label;
			}
		}

		// Token: 0x0600166D RID: 5741 RVA: 0x000A00E4 File Offset: 0x0009E2E4
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private void InitializeComponent()
		{
			if (ResourceLoader.IsEnabled && ResourceLoader.CanProvideContentFor(new ResourceLoader.ResourceLoadingQuery
			{
				AssemblyName = typeof(LabelSwitch).GetTypeInfo().Assembly.GetName(),
				ResourcePath = "UserControls/LabelSwitch.xaml",
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
			VisualDiagnostics.RegisterSourceInfo(columnDefinition = new ColumnDefinition(), new Uri("UserControls\\LabelSwitch.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 11, 18);
			ColumnDefinition columnDefinition2;
			VisualDiagnostics.RegisterSourceInfo(columnDefinition2 = new ColumnDefinition(), new Uri("UserControls\\LabelSwitch.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 12, 18);
			TapGestureRecognizer tapGestureRecognizer;
			VisualDiagnostics.RegisterSourceInfo(tapGestureRecognizer = new TapGestureRecognizer(), new Uri("UserControls\\LabelSwitch.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 20, 22);
			Label label;
			VisualDiagnostics.RegisterSourceInfo(label = new Label(), new Uri("UserControls\\LabelSwitch.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 14, 14);
			OnPlatform<bool> onPlatform;
			VisualDiagnostics.RegisterSourceInfo(onPlatform = new OnPlatform<bool>(), new Uri("UserControls\\LabelSwitch.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 29, 22);
			CheckBoxWithColor checkBoxWithColor;
			VisualDiagnostics.RegisterSourceInfo(checkBoxWithColor = new CheckBoxWithColor(), new Uri("UserControls\\LabelSwitch.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 23, 14);
			OnPlatform<bool> onPlatform2;
			VisualDiagnostics.RegisterSourceInfo(onPlatform2 = new OnPlatform<bool>(), new Uri("UserControls\\LabelSwitch.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 42, 22);
			Switch @switch;
			VisualDiagnostics.RegisterSourceInfo(@switch = new Switch(), new Uri("UserControls\\LabelSwitch.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 36, 14);
			Grid grid;
			VisualDiagnostics.RegisterSourceInfo(grid = new Grid(), new Uri("UserControls\\LabelSwitch.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 9, 10);
			VisualDiagnostics.RegisterSourceInfo(this, new Uri("UserControls\\LabelSwitch.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 2, 2);
			NameScope nameScope = NameScope.GetNameScope(this) ?? new NameScope();
			NameScope.SetNameScope(this, nameScope);
			nameScope.RegisterName("label", label);
			if (label.StyleId == null)
			{
				label.StyleId = "label";
			}
			nameScope.RegisterName("cb", checkBoxWithColor);
			if (checkBoxWithColor.StyleId == null)
			{
				checkBoxWithColor.StyleId = "cb";
			}
			nameScope.RegisterName("sw", @switch);
			if (@switch.StyleId == null)
			{
				@switch.StyleId = "sw";
			}
			this.label = label;
			this.cb = checkBoxWithColor;
			this.sw = @switch;
			columnDefinition.SetValue(ColumnDefinition.WidthProperty, new GridLengthTypeConverter().ConvertFromInvariantString("*"));
			grid.GetValue(Grid.ColumnDefinitionsProperty).Add(columnDefinition);
			columnDefinition2.SetValue(ColumnDefinition.WidthProperty, new GridLengthTypeConverter().ConvertFromInvariantString("auto"));
			grid.GetValue(Grid.ColumnDefinitionsProperty).Add(columnDefinition2);
			label.SetValue(Grid.ColumnProperty, 0);
			label.SetValue(Label.TextProperty, "");
			label.SetValue(View.VerticalOptionsProperty, LayoutOptions.Start);
			tapGestureRecognizer.SetValue(TapGestureRecognizer.NumberOfTapsRequiredProperty, 1);
			tapGestureRecognizer.Tapped += this.TapGestureRecognizer_Tapped;
			label.GestureRecognizers.Add(tapGestureRecognizer);
			grid.Children.Add(label);
			checkBoxWithColor.SetValue(Grid.ColumnProperty, 1);
			checkBoxWithColor.CheckedChanged += this.cb_CheckedChanged;
			checkBoxWithColor.SetValue(View.VerticalOptionsProperty, LayoutOptions.Start);
			onPlatform.Android = true;
			onPlatform.Default = false;
			onPlatform.iOS = false;
			checkBoxWithColor.SetValue(VisualElement.IsVisibleProperty, onPlatform);
			grid.Children.Add(checkBoxWithColor);
			@switch.SetValue(Grid.ColumnProperty, 1);
			@switch.Toggled += this.sw_Toggled;
			@switch.SetValue(View.VerticalOptionsProperty, LayoutOptions.Start);
			onPlatform2.Android = false;
			onPlatform2.Default = true;
			onPlatform2.iOS = true;
			@switch.SetValue(VisualElement.IsVisibleProperty, onPlatform2);
			grid.Children.Add(@switch);
			this.SetValue(ContentView.ContentProperty, grid);
		}

		// Token: 0x0600166E RID: 5742 RVA: 0x000A056C File Offset: 0x0009E76C
		// Note: this type is marked as 'beforefieldinit'.
		static LabelSwitch()
		{
		}

		// Token: 0x0600166F RID: 5743 RVA: 0x000A0618 File Offset: 0x0009E818
		private void __InitComponentRuntime()
		{
			Extensions.LoadFromXaml<LabelSwitch>(this, typeof(LabelSwitch));
			this.label = NameScopeExtensions.FindByName<Label>(this, "label");
			this.cb = NameScopeExtensions.FindByName<CheckBoxWithColor>(this, "cb");
			this.sw = NameScopeExtensions.FindByName<Switch>(this, "sw");
		}

		// Token: 0x04000693 RID: 1683
		[CompilerGenerated]
		private EventHandler<ToggledEventArgs> Toggled;

		// Token: 0x04000694 RID: 1684
		public static readonly BindableProperty TextProperty = BindableProperty.Create("Text", typeof(string), typeof(LabelSwitch), null, 2, null, new BindableProperty.BindingPropertyChangedDelegate(LabelSwitch.TextPropertyChanged), null, null, null);

		// Token: 0x04000695 RID: 1685
		public static readonly BindableProperty IsToggledProperty = BindableProperty.Create("IsToggled", typeof(bool), typeof(LabelSwitch), null, 1, null, new BindableProperty.BindingPropertyChangedDelegate(LabelSwitch.IsToggledPropertyChanged), null, null, null);

		// Token: 0x04000696 RID: 1686
		public static readonly BindableProperty TextColorProperty = BindableProperty.Create("TextColor", typeof(Color), typeof(LabelSwitch), null, 2, null, new BindableProperty.BindingPropertyChangedDelegate(LabelSwitch.TextColorPropertyChanged), null, null, null);

		// Token: 0x04000697 RID: 1687
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private Label label;

		// Token: 0x04000698 RID: 1688
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private CheckBoxWithColor cb;

		// Token: 0x04000699 RID: 1689
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private Switch sw;
	}
}
