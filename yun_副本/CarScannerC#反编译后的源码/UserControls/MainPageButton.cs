using System;
using System.CodeDom.Compiler;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Threading;
using CarScannerXamarinForms.Settings;
using Xamarin.Forms;
using Xamarin.Forms.Internals;
using Xamarin.Forms.Xaml;
using Xamarin.Forms.Xaml.Diagnostics;
using Xamarin.Forms.Xaml.Internals;

namespace CarScannerXamarinForms.UserControls
{
	// Token: 0x020005CD RID: 1485
	[XamlCompilation(2)]
	[XamlFilePath("UserControls\\MainPageButton.xaml")]
	public class MainPageButton : ContentView
	{
		// Token: 0x06003568 RID: 13672 RVA: 0x002651C2 File Offset: 0x002633C2
		public MainPageButton()
		{
			this.InitializeComponent();
			base.SetValue(MainPageButton.IsActiveProperty, false);
		}

		// Token: 0x06003569 RID: 13673 RVA: 0x002651E1 File Offset: 0x002633E1
		private void Frame_Tapped(object sender, EventArgs e)
		{
			EventHandler tapped = this.Tapped;
			if (tapped == null)
			{
				return;
			}
			tapped(this, e);
		}

		// Token: 0x0600356A RID: 13674 RVA: 0x002651F8 File Offset: 0x002633F8
		private static void IsActivePropertyChanged(BindableObject bindable, object oldValue, object newValue)
		{
			if (oldValue != newValue)
			{
				MainPageButton mainPageButton = (MainPageButton)bindable;
				if ((bool)newValue)
				{
					mainPageButton.active_image.IsVisible = true;
					mainPageButton.inactive_image.IsVisible = false;
					return;
				}
				mainPageButton.inactive_image.IsVisible = true;
				mainPageButton.active_image.IsVisible = false;
			}
		}

		// Token: 0x17001361 RID: 4961
		// (get) Token: 0x0600356B RID: 13675 RVA: 0x00265249 File Offset: 0x00263449
		// (set) Token: 0x0600356C RID: 13676 RVA: 0x0026525B File Offset: 0x0026345B
		public bool IsActive
		{
			get
			{
				return (bool)base.GetValue(MainPageButton.IsActiveProperty);
			}
			set
			{
				base.SetValue(MainPageButton.IsActiveProperty, value);
			}
		}

		// Token: 0x0600356D RID: 13677 RVA: 0x0026526E File Offset: 0x0026346E
		private static void TextPropertyChanged(BindableObject bindable, object oldValue, object newValue)
		{
			((MainPageButton)bindable).label.Text = (string)newValue;
		}

		// Token: 0x17001362 RID: 4962
		// (get) Token: 0x0600356E RID: 13678 RVA: 0x00265286 File Offset: 0x00263486
		// (set) Token: 0x0600356F RID: 13679 RVA: 0x00265298 File Offset: 0x00263498
		public string Text
		{
			get
			{
				return (string)base.GetValue(MainPageButton.TextProperty);
			}
			set
			{
				base.SetValue(MainPageButton.TextProperty, value);
			}
		}

		// Token: 0x06003570 RID: 13680 RVA: 0x002652A6 File Offset: 0x002634A6
		private static void ActiveImagePropertyChanged(BindableObject bindable, object oldValue, object newValue)
		{
			if (oldValue != newValue)
			{
				((MainPageButton)bindable).active_image.Source = (ImageSource)newValue;
			}
		}

		// Token: 0x17001363 RID: 4963
		// (get) Token: 0x06003571 RID: 13681 RVA: 0x002652C2 File Offset: 0x002634C2
		// (set) Token: 0x06003572 RID: 13682 RVA: 0x002652D4 File Offset: 0x002634D4
		public ImageSource ActiveImage
		{
			get
			{
				return (ImageSource)base.GetValue(MainPageButton.ActiveImageProperty);
			}
			set
			{
				base.SetValue(MainPageButton.ActiveImageProperty, value);
			}
		}

		// Token: 0x06003573 RID: 13683 RVA: 0x002652E2 File Offset: 0x002634E2
		private static void InactiveImagePropertyChanged(BindableObject bindable, object oldValue, object newValue)
		{
			((MainPageButton)bindable).inactive_image.Source = (ImageSource)newValue;
		}

		// Token: 0x17001364 RID: 4964
		// (get) Token: 0x06003574 RID: 13684 RVA: 0x002652FA File Offset: 0x002634FA
		// (set) Token: 0x06003575 RID: 13685 RVA: 0x0026530C File Offset: 0x0026350C
		public ImageSource InactiveImage
		{
			get
			{
				return (ImageSource)base.GetValue(MainPageButton.InactiveImageProperty);
			}
			set
			{
				base.SetValue(MainPageButton.InactiveImageProperty, value);
			}
		}

		// Token: 0x14000033 RID: 51
		// (add) Token: 0x06003576 RID: 13686 RVA: 0x0026531C File Offset: 0x0026351C
		// (remove) Token: 0x06003577 RID: 13687 RVA: 0x00265354 File Offset: 0x00263554
		public event EventHandler Tapped
		{
			[CompilerGenerated]
			add
			{
				EventHandler eventHandler = this.Tapped;
				EventHandler eventHandler2;
				do
				{
					eventHandler2 = eventHandler;
					EventHandler eventHandler3 = (EventHandler)Delegate.Combine(eventHandler2, value);
					eventHandler = Interlocked.CompareExchange<EventHandler>(ref this.Tapped, eventHandler3, eventHandler2);
				}
				while (eventHandler != eventHandler2);
			}
			[CompilerGenerated]
			remove
			{
				EventHandler eventHandler = this.Tapped;
				EventHandler eventHandler2;
				do
				{
					eventHandler2 = eventHandler;
					EventHandler eventHandler3 = (EventHandler)Delegate.Remove(eventHandler2, value);
					eventHandler = Interlocked.CompareExchange<EventHandler>(ref this.Tapped, eventHandler3, eventHandler2);
				}
				while (eventHandler != eventHandler2);
			}
		}

		// Token: 0x17001365 RID: 4965
		// (get) Token: 0x06003578 RID: 13688 RVA: 0x00265389 File Offset: 0x00263589
		// (set) Token: 0x06003579 RID: 13689 RVA: 0x00265391 File Offset: 0x00263591
		public MainPageButtons ButtonType
		{
			[CompilerGenerated]
			get
			{
				return this.<ButtonType>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<ButtonType>k__BackingField = value;
			}
		}

		// Token: 0x0600357A RID: 13690 RVA: 0x0026539C File Offset: 0x0026359C
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private void InitializeComponent()
		{
			if (ResourceLoader.IsEnabled && ResourceLoader.CanProvideContentFor(new ResourceLoader.ResourceLoadingQuery
			{
				AssemblyName = typeof(MainPageButton).GetTypeInfo().Assembly.GetName(),
				ResourcePath = "UserControls/MainPageButton.xaml",
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
			TapGestureRecognizer tapGestureRecognizer;
			VisualDiagnostics.RegisterSourceInfo(tapGestureRecognizer = new TapGestureRecognizer(), new Uri("UserControls\\MainPageButton.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 16, 18);
			Image image;
			VisualDiagnostics.RegisterSourceInfo(image = new Image(), new Uri("UserControls\\MainPageButton.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 23, 18);
			Image image2;
			VisualDiagnostics.RegisterSourceInfo(image2 = new Image(), new Uri("UserControls\\MainPageButton.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 27, 18);
			DynamicResourceExtension dynamicResourceExtension;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension = new DynamicResourceExtension(), new Uri("UserControls\\MainPageButton.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 33, 21);
			Label label;
			VisualDiagnostics.RegisterSourceInfo(label = new Label(), new Uri("UserControls\\MainPageButton.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 31, 18);
			StackLayout stackLayout;
			VisualDiagnostics.RegisterSourceInfo(stackLayout = new StackLayout(), new Uri("UserControls\\MainPageButton.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 19, 14);
			Frame frame;
			VisualDiagnostics.RegisterSourceInfo(frame = new Frame(), new Uri("UserControls\\MainPageButton.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 11, 10);
			VisualDiagnostics.RegisterSourceInfo(this, new Uri("UserControls\\MainPageButton.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 2, 2);
			NameScope nameScope = NameScope.GetNameScope(this) ?? new NameScope();
			NameScope.SetNameScope(this, nameScope);
			nameScope.RegisterName("active_image", image);
			if (image.StyleId == null)
			{
				image.StyleId = "active_image";
			}
			nameScope.RegisterName("inactive_image", image2);
			if (image2.StyleId == null)
			{
				image2.StyleId = "inactive_image";
			}
			nameScope.RegisterName("label", label);
			if (label.StyleId == null)
			{
				label.StyleId = "label";
			}
			this.active_image = image;
			this.inactive_image = image2;
			this.label = label;
			frame.SetValue(Layout.PaddingProperty, new Thickness(4.0));
			frame.SetValue(VisualElement.BackgroundColorProperty, Color.Transparent);
			frame.SetValue(Frame.HasShadowProperty, false);
			tapGestureRecognizer.Tapped += this.Frame_Tapped;
			frame.GestureRecognizers.Add(tapGestureRecognizer);
			stackLayout.SetValue(VisualElement.BackgroundColorProperty, Color.Transparent);
			stackLayout.SetValue(View.HorizontalOptionsProperty, LayoutOptions.Center);
			stackLayout.SetValue(StackLayout.OrientationProperty, 0);
			image.SetValue(View.HorizontalOptionsProperty, LayoutOptions.Center);
			image.SetValue(VisualElement.IsVisibleProperty, new VisualElement.VisibilityConverter().ConvertFromInvariantString("False"));
			stackLayout.Children.Add(image);
			image2.SetValue(View.HorizontalOptionsProperty, LayoutOptions.Center);
			image2.SetValue(VisualElement.IsVisibleProperty, new VisualElement.VisibilityConverter().ConvertFromInvariantString("True"));
			stackLayout.Children.Add(image2);
			dynamicResourceExtension.Key = "BaseFontSize+";
			IMarkupExtension<DynamicResource> markupExtension = dynamicResourceExtension;
			XamlServiceProvider xamlServiceProvider = new XamlServiceProvider();
			Type typeFromHandle = typeof(IProvideValueTarget);
			object[] array = new object[0 + 4];
			array[0] = label;
			array[1] = stackLayout;
			array[2] = frame;
			array[3] = this;
			object obj;
			xamlServiceProvider.Add(typeFromHandle, obj = new SimpleValueTargetProvider(array, Label.FontSizeProperty, nameScope));
			xamlServiceProvider.Add(typeof(IReferenceProvider), obj);
			Type typeFromHandle2 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver = new XmlNamespaceResolver();
			xmlNamespaceResolver.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver.Add("d", "http://xamarin.com/schemas/2014/forms/design");
			xmlNamespaceResolver.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver.Add("mc", "http://schemas.openxmlformats.org/markup-compatibility/2006");
			xamlServiceProvider.Add(typeFromHandle2, new XamlTypeResolver(xmlNamespaceResolver, typeof(MainPageButton).GetTypeInfo().Assembly));
			xamlServiceProvider.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(33, 21)));
			DynamicResource dynamicResource = markupExtension.ProvideValue(xamlServiceProvider);
			label.SetDynamicResource(Label.FontSizeProperty, dynamicResource.Key);
			label.SetValue(Label.HorizontalTextAlignmentProperty, new TextAlignmentConverter().ConvertFromInvariantString("Center"));
			label.SetValue(Label.LineBreakModeProperty, 1);
			stackLayout.Children.Add(label);
			frame.SetValue(ContentView.ContentProperty, stackLayout);
			this.SetValue(ContentView.ContentProperty, frame);
		}

		// Token: 0x0600357B RID: 13691 RVA: 0x00265884 File Offset: 0x00263A84
		// Note: this type is marked as 'beforefieldinit'.
		static MainPageButton()
		{
		}

		// Token: 0x0600357C RID: 13692 RVA: 0x0026596C File Offset: 0x00263B6C
		private void __InitComponentRuntime()
		{
			Extensions.LoadFromXaml<MainPageButton>(this, typeof(MainPageButton));
			this.active_image = NameScopeExtensions.FindByName<Image>(this, "active_image");
			this.inactive_image = NameScopeExtensions.FindByName<Image>(this, "inactive_image");
			this.label = NameScopeExtensions.FindByName<Label>(this, "label");
		}

		// Token: 0x04001FC6 RID: 8134
		public static readonly BindableProperty IsActiveProperty = BindableProperty.Create("IsActive", typeof(bool), typeof(MainPageButton), false, 2, null, new BindableProperty.BindingPropertyChangedDelegate(MainPageButton.IsActivePropertyChanged), null, null, null);

		// Token: 0x04001FC7 RID: 8135
		public static readonly BindableProperty TextProperty = BindableProperty.Create("Text", typeof(string), typeof(MainPageButton), null, 2, null, new BindableProperty.BindingPropertyChangedDelegate(MainPageButton.TextPropertyChanged), null, null, null);

		// Token: 0x04001FC8 RID: 8136
		public static readonly BindableProperty ActiveImageProperty = BindableProperty.Create("ActiveImage", typeof(ImageSource), typeof(MainPageButton), null, 2, null, new BindableProperty.BindingPropertyChangedDelegate(MainPageButton.ActiveImagePropertyChanged), null, null, null);

		// Token: 0x04001FC9 RID: 8137
		public static readonly BindableProperty InactiveImageProperty = BindableProperty.Create("InactiveImage", typeof(ImageSource), typeof(MainPageButton), null, 2, null, new BindableProperty.BindingPropertyChangedDelegate(MainPageButton.InactiveImagePropertyChanged), null, null, null);

		// Token: 0x04001FCA RID: 8138
		[CompilerGenerated]
		private EventHandler Tapped;

		// Token: 0x04001FCB RID: 8139
		[CompilerGenerated]
		private MainPageButtons <ButtonType>k__BackingField;

		// Token: 0x04001FCC RID: 8140
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private Image active_image;

		// Token: 0x04001FCD RID: 8141
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private Image inactive_image;

		// Token: 0x04001FCE RID: 8142
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private Label label;
	}
}
