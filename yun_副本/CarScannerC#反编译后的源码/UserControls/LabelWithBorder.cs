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
	// Token: 0x020005CC RID: 1484
	[XamlCompilation(2)]
	[XamlFilePath("UserControls\\LabelWithBorder.xaml")]
	public class LabelWithBorder : ContentView
	{
		// Token: 0x0600355C RID: 13660 RVA: 0x00264ACD File Offset: 0x00262CCD
		public LabelWithBorder()
		{
			this.InitializeComponent();
			this.label.Text = this.Text;
		}

		// Token: 0x14000032 RID: 50
		// (add) Token: 0x0600355D RID: 13661 RVA: 0x00264AF8 File Offset: 0x00262CF8
		// (remove) Token: 0x0600355E RID: 13662 RVA: 0x00264B30 File Offset: 0x00262D30
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

		// Token: 0x0600355F RID: 13663 RVA: 0x00264B65 File Offset: 0x00262D65
		private static void TextPropertyChanged(BindableObject bindable, object oldValue, object newValue)
		{
			((LabelWithBorder)bindable).label.Text = (string)newValue;
		}

		// Token: 0x1700135F RID: 4959
		// (get) Token: 0x06003560 RID: 13664 RVA: 0x00264B7D File Offset: 0x00262D7D
		// (set) Token: 0x06003561 RID: 13665 RVA: 0x00264B8F File Offset: 0x00262D8F
		public string Text
		{
			get
			{
				return (string)base.GetValue(LabelWithBorder.TextProperty);
			}
			set
			{
				base.SetValue(LabelWithBorder.TextProperty, value);
			}
		}

		// Token: 0x06003562 RID: 13666 RVA: 0x00264B9D File Offset: 0x00262D9D
		private void OnTapGestureRecognizerTapped(object sender, EventArgs args)
		{
			EventHandler tapped = this.Tapped;
			if (tapped == null)
			{
				return;
			}
			tapped(this, args);
		}

		// Token: 0x17001360 RID: 4960
		// (get) Token: 0x06003563 RID: 13667 RVA: 0x00264BB1 File Offset: 0x00262DB1
		// (set) Token: 0x06003564 RID: 13668 RVA: 0x00264BB9 File Offset: 0x00262DB9
		public string Tag
		{
			[CompilerGenerated]
			get
			{
				return this.<Tag>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<Tag>k__BackingField = value;
			}
		} = "";

		// Token: 0x06003565 RID: 13669 RVA: 0x00264BC4 File Offset: 0x00262DC4
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private void InitializeComponent()
		{
			if (ResourceLoader.IsEnabled && ResourceLoader.CanProvideContentFor(new ResourceLoader.ResourceLoadingQuery
			{
				AssemblyName = typeof(LabelWithBorder).GetTypeInfo().Assembly.GetName(),
				ResourcePath = "UserControls/LabelWithBorder.xaml",
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
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension = new DynamicResourceExtension(), new Uri("UserControls\\LabelWithBorder.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 10, 13);
			DynamicResourceExtension dynamicResourceExtension2;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension2 = new DynamicResourceExtension(), new Uri("UserControls\\LabelWithBorder.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 11, 13);
			TapGestureRecognizer tapGestureRecognizer;
			VisualDiagnostics.RegisterSourceInfo(tapGestureRecognizer = new TapGestureRecognizer(), new Uri("UserControls\\LabelWithBorder.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 14, 18);
			DynamicResourceExtension dynamicResourceExtension3;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension3 = new DynamicResourceExtension(), new Uri("UserControls\\LabelWithBorder.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 19, 17);
			TapGestureRecognizer tapGestureRecognizer2;
			VisualDiagnostics.RegisterSourceInfo(tapGestureRecognizer2 = new TapGestureRecognizer(), new Uri("UserControls\\LabelWithBorder.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 21, 22);
			Label label;
			VisualDiagnostics.RegisterSourceInfo(label = new Label(), new Uri("UserControls\\LabelWithBorder.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 16, 14);
			Frame frame;
			VisualDiagnostics.RegisterSourceInfo(frame = new Frame(), new Uri("UserControls\\LabelWithBorder.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 8, 10);
			VisualDiagnostics.RegisterSourceInfo(this, new Uri("UserControls\\LabelWithBorder.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 2, 2);
			NameScope nameScope = NameScope.GetNameScope(this) ?? new NameScope();
			NameScope.SetNameScope(this, nameScope);
			nameScope.RegisterName("label", label);
			if (label.StyleId == null)
			{
				label.StyleId = "label";
			}
			this.label = label;
			frame.SetValue(Layout.PaddingProperty, new Thickness(5.0));
			dynamicResourceExtension.Key = "EntryBackgroundColor";
			IMarkupExtension<DynamicResource> markupExtension = dynamicResourceExtension;
			XamlServiceProvider xamlServiceProvider = new XamlServiceProvider();
			Type typeFromHandle = typeof(IProvideValueTarget);
			object[] array = new object[0 + 2];
			array[0] = frame;
			array[1] = this;
			object obj;
			xamlServiceProvider.Add(typeFromHandle, obj = new SimpleValueTargetProvider(array, VisualElement.BackgroundColorProperty, nameScope));
			xamlServiceProvider.Add(typeof(IReferenceProvider), obj);
			Type typeFromHandle2 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver = new XmlNamespaceResolver();
			xmlNamespaceResolver.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver.Add("local", "clr-namespace:CarScannerXamarinForms");
			xamlServiceProvider.Add(typeFromHandle2, new XamlTypeResolver(xmlNamespaceResolver, typeof(LabelWithBorder).GetTypeInfo().Assembly));
			xamlServiceProvider.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(10, 13)));
			DynamicResource dynamicResource = markupExtension.ProvideValue(xamlServiceProvider);
			frame.SetDynamicResource(VisualElement.BackgroundColorProperty, dynamicResource.Key);
			dynamicResourceExtension2.Key = "GrayedTextColor";
			IMarkupExtension<DynamicResource> markupExtension2 = dynamicResourceExtension2;
			XamlServiceProvider xamlServiceProvider2 = new XamlServiceProvider();
			Type typeFromHandle3 = typeof(IProvideValueTarget);
			object[] array2 = new object[0 + 2];
			array2[0] = frame;
			array2[1] = this;
			object obj2;
			xamlServiceProvider2.Add(typeFromHandle3, obj2 = new SimpleValueTargetProvider(array2, Frame.BorderColorProperty, nameScope));
			xamlServiceProvider2.Add(typeof(IReferenceProvider), obj2);
			Type typeFromHandle4 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver2 = new XmlNamespaceResolver();
			xmlNamespaceResolver2.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver2.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver2.Add("local", "clr-namespace:CarScannerXamarinForms");
			xamlServiceProvider2.Add(typeFromHandle4, new XamlTypeResolver(xmlNamespaceResolver2, typeof(LabelWithBorder).GetTypeInfo().Assembly));
			xamlServiceProvider2.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(11, 13)));
			DynamicResource dynamicResource2 = markupExtension2.ProvideValue(xamlServiceProvider2);
			frame.SetDynamicResource(Frame.BorderColorProperty, dynamicResource2.Key);
			frame.SetValue(Frame.HasShadowProperty, false);
			tapGestureRecognizer.SetValue(TapGestureRecognizer.NumberOfTapsRequiredProperty, 1);
			tapGestureRecognizer.Tapped += this.OnTapGestureRecognizerTapped;
			frame.GestureRecognizers.Add(tapGestureRecognizer);
			label.SetValue(VisualElement.InputTransparentProperty, true);
			dynamicResourceExtension3.Key = "ButtonAccentColor";
			IMarkupExtension<DynamicResource> markupExtension3 = dynamicResourceExtension3;
			XamlServiceProvider xamlServiceProvider3 = new XamlServiceProvider();
			Type typeFromHandle5 = typeof(IProvideValueTarget);
			object[] array3 = new object[0 + 3];
			array3[0] = label;
			array3[1] = frame;
			array3[2] = this;
			object obj3;
			xamlServiceProvider3.Add(typeFromHandle5, obj3 = new SimpleValueTargetProvider(array3, Label.TextColorProperty, nameScope));
			xamlServiceProvider3.Add(typeof(IReferenceProvider), obj3);
			Type typeFromHandle6 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver3 = new XmlNamespaceResolver();
			xmlNamespaceResolver3.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver3.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver3.Add("local", "clr-namespace:CarScannerXamarinForms");
			xamlServiceProvider3.Add(typeFromHandle6, new XamlTypeResolver(xmlNamespaceResolver3, typeof(LabelWithBorder).GetTypeInfo().Assembly));
			xamlServiceProvider3.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(19, 17)));
			DynamicResource dynamicResource3 = markupExtension3.ProvideValue(xamlServiceProvider3);
			label.SetDynamicResource(Label.TextColorProperty, dynamicResource3.Key);
			tapGestureRecognizer2.SetValue(TapGestureRecognizer.NumberOfTapsRequiredProperty, 1);
			tapGestureRecognizer2.Tapped += this.OnTapGestureRecognizerTapped;
			label.GestureRecognizers.Add(tapGestureRecognizer2);
			frame.SetValue(ContentView.ContentProperty, label);
			this.SetValue(ContentView.ContentProperty, frame);
		}

		// Token: 0x06003566 RID: 13670 RVA: 0x0026515C File Offset: 0x0026335C
		// Note: this type is marked as 'beforefieldinit'.
		static LabelWithBorder()
		{
		}

		// Token: 0x06003567 RID: 13671 RVA: 0x0026519E File Offset: 0x0026339E
		private void __InitComponentRuntime()
		{
			Extensions.LoadFromXaml<LabelWithBorder>(this, typeof(LabelWithBorder));
			this.label = NameScopeExtensions.FindByName<Label>(this, "label");
		}

		// Token: 0x04001FC2 RID: 8130
		[CompilerGenerated]
		private EventHandler Tapped;

		// Token: 0x04001FC3 RID: 8131
		public static readonly BindableProperty TextProperty = BindableProperty.Create("Text", typeof(string), typeof(LabelWithBorder), null, 2, null, new BindableProperty.BindingPropertyChangedDelegate(LabelWithBorder.TextPropertyChanged), null, null, null);

		// Token: 0x04001FC4 RID: 8132
		[CompilerGenerated]
		private string <Tag>k__BackingField;

		// Token: 0x04001FC5 RID: 8133
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private Label label;
	}
}
