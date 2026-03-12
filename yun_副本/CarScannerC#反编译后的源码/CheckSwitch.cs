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
	// Token: 0x0200018E RID: 398
	[XamlCompilation(2)]
	[XamlFilePath("UserControls\\CheckSwitch.xaml")]
	public class CheckSwitch : ContentView
	{
		// Token: 0x0600160C RID: 5644 RVA: 0x0009BB50 File Offset: 0x00099D50
		public CheckSwitch()
		{
			this.InitializeComponent();
			if (this.sw.IsVisible)
			{
				this.sw.IsToggled = this.IsToggled;
			}
			if (this.cb.IsVisible)
			{
				this.cb.IsChecked = this.IsToggled;
			}
		}

		// Token: 0x1400000C RID: 12
		// (add) Token: 0x0600160D RID: 5645 RVA: 0x0009BBA8 File Offset: 0x00099DA8
		// (remove) Token: 0x0600160E RID: 5646 RVA: 0x0009BBE0 File Offset: 0x00099DE0
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

		// Token: 0x0600160F RID: 5647 RVA: 0x0009BC18 File Offset: 0x00099E18
		private static void IsToggledPropertyChanged(BindableObject bindable, object oldValue, object newValue)
		{
			CheckSwitch checkSwitch = (CheckSwitch)bindable;
			bool flag = (bool)newValue;
			if (checkSwitch.sw.IsVisible)
			{
				checkSwitch.sw.IsToggled = flag;
			}
			if (checkSwitch.cb.IsVisible)
			{
				checkSwitch.cb.IsChecked = flag;
			}
		}

		// Token: 0x06001610 RID: 5648 RVA: 0x0009BC65 File Offset: 0x00099E65
		private void sw_Toggled(object sender, ToggledEventArgs args)
		{
			base.SetValue(CheckSwitch.IsToggledProperty, this.sw.IsToggled);
			EventHandler<ToggledEventArgs> toggled = this.Toggled;
			if (toggled == null)
			{
				return;
			}
			toggled(this, args);
		}

		// Token: 0x17000F5D RID: 3933
		// (get) Token: 0x06001611 RID: 5649 RVA: 0x0009BC94 File Offset: 0x00099E94
		// (set) Token: 0x06001612 RID: 5650 RVA: 0x0009BCA6 File Offset: 0x00099EA6
		public bool IsToggled
		{
			get
			{
				return (bool)base.GetValue(CheckSwitch.IsToggledProperty);
			}
			set
			{
				base.SetValue(CheckSwitch.IsToggledProperty, value);
			}
		}

		// Token: 0x06001613 RID: 5651 RVA: 0x0009BCB9 File Offset: 0x00099EB9
		private void cb_CheckedChanged(object sender, CheckedChangedEventArgs e)
		{
			base.SetValue(CheckSwitch.IsToggledProperty, this.cb.IsChecked);
			EventHandler<ToggledEventArgs> toggled = this.Toggled;
			if (toggled == null)
			{
				return;
			}
			toggled(this, new ToggledEventArgs(e.Value));
		}

		// Token: 0x06001614 RID: 5652 RVA: 0x0009BCF4 File Offset: 0x00099EF4
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private void InitializeComponent()
		{
			if (ResourceLoader.IsEnabled && ResourceLoader.CanProvideContentFor(new ResourceLoader.ResourceLoadingQuery
			{
				AssemblyName = typeof(CheckSwitch).GetTypeInfo().Assembly.GetName(),
				ResourcePath = "UserControls/CheckSwitch.xaml",
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
			OnPlatform<bool> onPlatform;
			VisualDiagnostics.RegisterSourceInfo(onPlatform = new OnPlatform<bool>(), new Uri("UserControls\\CheckSwitch.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 16, 22);
			CheckBoxWithColor checkBoxWithColor;
			VisualDiagnostics.RegisterSourceInfo(checkBoxWithColor = new CheckBoxWithColor(), new Uri("UserControls\\CheckSwitch.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 10, 14);
			OnPlatform<bool> onPlatform2;
			VisualDiagnostics.RegisterSourceInfo(onPlatform2 = new OnPlatform<bool>(), new Uri("UserControls\\CheckSwitch.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 29, 22);
			Switch @switch;
			VisualDiagnostics.RegisterSourceInfo(@switch = new Switch(), new Uri("UserControls\\CheckSwitch.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 23, 14);
			StackLayout stackLayout;
			VisualDiagnostics.RegisterSourceInfo(stackLayout = new StackLayout(), new Uri("UserControls\\CheckSwitch.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 9, 10);
			VisualDiagnostics.RegisterSourceInfo(this, new Uri("UserControls\\CheckSwitch.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 2, 2);
			NameScope nameScope = NameScope.GetNameScope(this) ?? new NameScope();
			NameScope.SetNameScope(this, nameScope);
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
			this.cb = checkBoxWithColor;
			this.sw = @switch;
			checkBoxWithColor.SetValue(Grid.ColumnProperty, 1);
			checkBoxWithColor.CheckedChanged += this.cb_CheckedChanged;
			checkBoxWithColor.SetValue(View.VerticalOptionsProperty, LayoutOptions.Start);
			onPlatform.Android = true;
			onPlatform.Default = false;
			onPlatform.iOS = false;
			checkBoxWithColor.SetValue(VisualElement.IsVisibleProperty, onPlatform);
			stackLayout.Children.Add(checkBoxWithColor);
			@switch.SetValue(Grid.ColumnProperty, 1);
			@switch.Toggled += this.sw_Toggled;
			@switch.SetValue(View.VerticalOptionsProperty, LayoutOptions.Start);
			onPlatform2.Android = false;
			onPlatform2.Default = true;
			onPlatform2.iOS = true;
			@switch.SetValue(VisualElement.IsVisibleProperty, onPlatform2);
			stackLayout.Children.Add(@switch);
			this.SetValue(ContentView.ContentProperty, stackLayout);
		}

		// Token: 0x06001615 RID: 5653 RVA: 0x0009BFC4 File Offset: 0x0009A1C4
		// Note: this type is marked as 'beforefieldinit'.
		static CheckSwitch()
		{
		}

		// Token: 0x06001616 RID: 5654 RVA: 0x0009C006 File Offset: 0x0009A206
		private void __InitComponentRuntime()
		{
			Extensions.LoadFromXaml<CheckSwitch>(this, typeof(CheckSwitch));
			this.cb = NameScopeExtensions.FindByName<CheckBoxWithColor>(this, "cb");
			this.sw = NameScopeExtensions.FindByName<Switch>(this, "sw");
		}

		// Token: 0x0400065F RID: 1631
		[CompilerGenerated]
		private EventHandler<ToggledEventArgs> Toggled;

		// Token: 0x04000660 RID: 1632
		public static readonly BindableProperty IsToggledProperty = BindableProperty.Create("IsToggled", typeof(bool), typeof(LabelSwitch), null, 1, null, new BindableProperty.BindingPropertyChangedDelegate(CheckSwitch.IsToggledPropertyChanged), null, null, null);

		// Token: 0x04000661 RID: 1633
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private CheckBoxWithColor cb;

		// Token: 0x04000662 RID: 1634
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private Switch sw;
	}
}
