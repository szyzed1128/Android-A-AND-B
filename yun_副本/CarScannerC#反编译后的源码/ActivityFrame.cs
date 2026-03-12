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
	// Token: 0x0200018C RID: 396
	[XamlFilePath("UserControls\\ActivityFrame.xaml")]
	public class ActivityFrame : ContentView
	{
		// Token: 0x060015FA RID: 5626 RVA: 0x0009AEC8 File Offset: 0x000990C8
		public ActivityFrame()
		{
			this._Text = Translate.GetString("ios_PleaseWait");
			this.InitializeComponent();
			this.label.BindingContext = this;
			this.btnCancel.BindingContext = this;
			this.cancelFrame.BindingContext = this;
		}

		// Token: 0x17000F58 RID: 3928
		// (get) Token: 0x060015FB RID: 5627 RVA: 0x0009AF20 File Offset: 0x00099120
		// (set) Token: 0x060015FC RID: 5628 RVA: 0x0009AF32 File Offset: 0x00099132
		public string Text
		{
			get
			{
				return (string)base.GetValue(ActivityFrame.TextProperty);
			}
			set
			{
				base.SetValue(ActivityFrame.TextProperty, value);
			}
		}

		// Token: 0x17000F59 RID: 3929
		// (get) Token: 0x060015FD RID: 5629 RVA: 0x0009AF40 File Offset: 0x00099140
		// (set) Token: 0x060015FE RID: 5630 RVA: 0x0009AF52 File Offset: 0x00099152
		public string CancelText
		{
			get
			{
				return (string)base.GetValue(ActivityFrame.CancelTextProperty);
			}
			set
			{
				base.SetValue(ActivityFrame.CancelTextProperty, value);
			}
		}

		// Token: 0x17000F5A RID: 3930
		// (get) Token: 0x060015FF RID: 5631 RVA: 0x0009AF60 File Offset: 0x00099160
		// (set) Token: 0x06001600 RID: 5632 RVA: 0x0009AF72 File Offset: 0x00099172
		public bool IsCancelVisible
		{
			get
			{
				return (bool)base.GetValue(ActivityFrame.IsCancelVisibleProperty);
			}
			set
			{
				base.SetValue(ActivityFrame.IsCancelVisibleProperty, value);
				if (value)
				{
					this.cancelFrame.BorderColor = Color.White;
					return;
				}
				this.cancelFrame.BorderColor = Color.Transparent;
			}
		}

		// Token: 0x06001601 RID: 5633 RVA: 0x0009AFA9 File Offset: 0x000991A9
		public void ResetTextToDefault()
		{
			this.Text = Translate.GetString("ios_PleaseWait");
		}

		// Token: 0x17000F5B RID: 3931
		// (get) Token: 0x06001602 RID: 5634 RVA: 0x0009AFBB File Offset: 0x000991BB
		public static string PLEASE_WAIT_TEXT
		{
			get
			{
				return Translate.GetString("ios_PleaseWait");
			}
		}

		// Token: 0x06001603 RID: 5635 RVA: 0x0009AFC7 File Offset: 0x000991C7
		private void BtnCancel_Clicked(object sender, EventArgs e)
		{
			EventHandler cancelClicked = this.CancelClicked;
			if (cancelClicked == null)
			{
				return;
			}
			cancelClicked(this, e);
		}

		// Token: 0x06001604 RID: 5636 RVA: 0x0009AFDC File Offset: 0x000991DC
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private void InitializeComponent()
		{
			if (ResourceLoader.IsEnabled && ResourceLoader.CanProvideContentFor(new ResourceLoader.ResourceLoadingQuery
			{
				AssemblyName = typeof(ActivityFrame).GetTypeInfo().Assembly.GetName(),
				ResourcePath = "UserControls/ActivityFrame.xaml",
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
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension = new DynamicResourceExtension(), new Uri("UserControls\\ActivityFrame.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 11, 16);
			RowDefinition rowDefinition;
			VisualDiagnostics.RegisterSourceInfo(rowDefinition = new RowDefinition(), new Uri("UserControls\\ActivityFrame.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 14, 22);
			RowDefinition rowDefinition2;
			VisualDiagnostics.RegisterSourceInfo(rowDefinition2 = new RowDefinition(), new Uri("UserControls\\ActivityFrame.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 15, 22);
			RowDefinition rowDefinition3;
			VisualDiagnostics.RegisterSourceInfo(rowDefinition3 = new RowDefinition(), new Uri("UserControls\\ActivityFrame.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 16, 22);
			BindingExtension bindingExtension;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension = new BindingExtension(), new Uri("UserControls\\ActivityFrame.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 23, 21);
			DynamicResourceExtension dynamicResourceExtension2;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension2 = new DynamicResourceExtension(), new Uri("UserControls\\ActivityFrame.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 24, 21);
			Label label;
			VisualDiagnostics.RegisterSourceInfo(label = new Label(), new Uri("UserControls\\ActivityFrame.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 18, 18);
			DynamicResourceExtension dynamicResourceExtension3;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension3 = new DynamicResourceExtension(), new Uri("UserControls\\ActivityFrame.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 28, 21);
			ActivityIndicator activityIndicator;
			VisualDiagnostics.RegisterSourceInfo(activityIndicator = new ActivityIndicator(), new Uri("UserControls\\ActivityFrame.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 25, 18);
			DynamicResourceExtension dynamicResourceExtension4;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension4 = new DynamicResourceExtension(), new Uri("UserControls\\ActivityFrame.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 32, 21);
			BindingExtension bindingExtension2;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension2 = new BindingExtension(), new Uri("UserControls\\ActivityFrame.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 35, 21);
			BindingExtension bindingExtension3;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension3 = new BindingExtension(), new Uri("UserControls\\ActivityFrame.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 40, 25);
			LinkButton linkButton;
			VisualDiagnostics.RegisterSourceInfo(linkButton = new LinkButton(), new Uri("UserControls\\ActivityFrame.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 36, 22);
			Frame frame;
			VisualDiagnostics.RegisterSourceInfo(frame = new Frame(), new Uri("UserControls\\ActivityFrame.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 29, 18);
			Grid grid;
			VisualDiagnostics.RegisterSourceInfo(grid = new Grid(), new Uri("UserControls\\ActivityFrame.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 12, 14);
			Frame frame2;
			VisualDiagnostics.RegisterSourceInfo(frame2 = new Frame(), new Uri("UserControls\\ActivityFrame.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 11, 10);
			VisualDiagnostics.RegisterSourceInfo(this, new Uri("UserControls\\ActivityFrame.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 2, 2);
			NameScope nameScope = NameScope.GetNameScope(this) ?? new NameScope();
			NameScope.SetNameScope(this, nameScope);
			nameScope.RegisterName("label", label);
			if (label.StyleId == null)
			{
				label.StyleId = "label";
			}
			nameScope.RegisterName("cancelFrame", frame);
			if (frame.StyleId == null)
			{
				frame.StyleId = "cancelFrame";
			}
			nameScope.RegisterName("btnCancel", linkButton);
			if (linkButton.StyleId == null)
			{
				linkButton.StyleId = "btnCancel";
			}
			this.label = label;
			this.cancelFrame = frame;
			this.btnCancel = linkButton;
			this.SetValue(View.MarginProperty, new Thickness(30.0));
			this.SetValue(VisualElement.MinimumHeightRequestProperty, 50.0);
			this.SetValue(VisualElement.MinimumWidthRequestProperty, 100.0);
			dynamicResourceExtension.Key = "TextColor";
			IMarkupExtension<DynamicResource> markupExtension = dynamicResourceExtension;
			XamlServiceProvider xamlServiceProvider = new XamlServiceProvider();
			Type typeFromHandle = typeof(IProvideValueTarget);
			object[] array = new object[0 + 2];
			array[0] = frame2;
			array[1] = this;
			object obj;
			xamlServiceProvider.Add(typeFromHandle, obj = new SimpleValueTargetProvider(array, VisualElement.BackgroundColorProperty, nameScope));
			xamlServiceProvider.Add(typeof(IReferenceProvider), obj);
			Type typeFromHandle2 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver = new XmlNamespaceResolver();
			xmlNamespaceResolver.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver.Add("local", "clr-namespace:CarScannerXamarinForms");
			xamlServiceProvider.Add(typeFromHandle2, new XamlTypeResolver(xmlNamespaceResolver, typeof(ActivityFrame).GetTypeInfo().Assembly));
			xamlServiceProvider.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(11, 16)));
			DynamicResource dynamicResource = markupExtension.ProvideValue(xamlServiceProvider);
			frame2.SetDynamicResource(VisualElement.BackgroundColorProperty, dynamicResource.Key);
			frame2.SetValue(VisualElement.OpacityProperty, 0.8);
			rowDefinition.SetValue(RowDefinition.HeightProperty, new GridLengthTypeConverter().ConvertFromInvariantString("auto"));
			grid.GetValue(Grid.RowDefinitionsProperty).Add(rowDefinition);
			rowDefinition2.SetValue(RowDefinition.HeightProperty, new GridLengthTypeConverter().ConvertFromInvariantString("auto"));
			grid.GetValue(Grid.RowDefinitionsProperty).Add(rowDefinition2);
			rowDefinition3.SetValue(RowDefinition.HeightProperty, new GridLengthTypeConverter().ConvertFromInvariantString("auto"));
			grid.GetValue(Grid.RowDefinitionsProperty).Add(rowDefinition3);
			label.SetValue(Grid.RowProperty, 0);
			label.SetValue(View.HorizontalOptionsProperty, LayoutOptions.Center);
			label.SetValue(Label.HorizontalTextAlignmentProperty, new TextAlignmentConverter().ConvertFromInvariantString("Center"));
			bindingExtension.Mode = 2;
			bindingExtension.Path = "Text";
			BindingBase bindingBase = bindingExtension.ProvideValue(null);
			label.SetBinding(Label.TextProperty, bindingBase);
			dynamicResourceExtension2.Key = "TextInverseColor";
			IMarkupExtension<DynamicResource> markupExtension2 = dynamicResourceExtension2;
			XamlServiceProvider xamlServiceProvider2 = new XamlServiceProvider();
			Type typeFromHandle3 = typeof(IProvideValueTarget);
			object[] array2 = new object[0 + 4];
			array2[0] = label;
			array2[1] = grid;
			array2[2] = frame2;
			array2[3] = this;
			object obj2;
			xamlServiceProvider2.Add(typeFromHandle3, obj2 = new SimpleValueTargetProvider(array2, Label.TextColorProperty, nameScope));
			xamlServiceProvider2.Add(typeof(IReferenceProvider), obj2);
			Type typeFromHandle4 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver2 = new XmlNamespaceResolver();
			xmlNamespaceResolver2.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver2.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver2.Add("local", "clr-namespace:CarScannerXamarinForms");
			xamlServiceProvider2.Add(typeFromHandle4, new XamlTypeResolver(xmlNamespaceResolver2, typeof(ActivityFrame).GetTypeInfo().Assembly));
			xamlServiceProvider2.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(24, 21)));
			DynamicResource dynamicResource2 = markupExtension2.ProvideValue(xamlServiceProvider2);
			label.SetDynamicResource(Label.TextColorProperty, dynamicResource2.Key);
			grid.Children.Add(label);
			activityIndicator.SetValue(Grid.RowProperty, 1);
			activityIndicator.SetValue(ActivityIndicator.IsRunningProperty, true);
			dynamicResourceExtension3.Key = "TextInverseColor";
			IMarkupExtension<DynamicResource> markupExtension3 = dynamicResourceExtension3;
			XamlServiceProvider xamlServiceProvider3 = new XamlServiceProvider();
			Type typeFromHandle5 = typeof(IProvideValueTarget);
			object[] array3 = new object[0 + 4];
			array3[0] = activityIndicator;
			array3[1] = grid;
			array3[2] = frame2;
			array3[3] = this;
			object obj3;
			xamlServiceProvider3.Add(typeFromHandle5, obj3 = new SimpleValueTargetProvider(array3, ActivityIndicator.ColorProperty, nameScope));
			xamlServiceProvider3.Add(typeof(IReferenceProvider), obj3);
			Type typeFromHandle6 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver3 = new XmlNamespaceResolver();
			xmlNamespaceResolver3.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver3.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver3.Add("local", "clr-namespace:CarScannerXamarinForms");
			xamlServiceProvider3.Add(typeFromHandle6, new XamlTypeResolver(xmlNamespaceResolver3, typeof(ActivityFrame).GetTypeInfo().Assembly));
			xamlServiceProvider3.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(28, 21)));
			DynamicResource dynamicResource3 = markupExtension3.ProvideValue(xamlServiceProvider3);
			activityIndicator.SetDynamicResource(ActivityIndicator.ColorProperty, dynamicResource3.Key);
			grid.Children.Add(activityIndicator);
			frame.SetValue(Grid.RowProperty, 2);
			dynamicResourceExtension4.Key = "TextColor";
			IMarkupExtension<DynamicResource> markupExtension4 = dynamicResourceExtension4;
			XamlServiceProvider xamlServiceProvider4 = new XamlServiceProvider();
			Type typeFromHandle7 = typeof(IProvideValueTarget);
			object[] array4 = new object[0 + 4];
			array4[0] = frame;
			array4[1] = grid;
			array4[2] = frame2;
			array4[3] = this;
			object obj4;
			xamlServiceProvider4.Add(typeFromHandle7, obj4 = new SimpleValueTargetProvider(array4, VisualElement.BackgroundColorProperty, nameScope));
			xamlServiceProvider4.Add(typeof(IReferenceProvider), obj4);
			Type typeFromHandle8 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver4 = new XmlNamespaceResolver();
			xmlNamespaceResolver4.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver4.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver4.Add("local", "clr-namespace:CarScannerXamarinForms");
			xamlServiceProvider4.Add(typeFromHandle8, new XamlTypeResolver(xmlNamespaceResolver4, typeof(ActivityFrame).GetTypeInfo().Assembly));
			xamlServiceProvider4.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(32, 21)));
			DynamicResource dynamicResource4 = markupExtension4.ProvideValue(xamlServiceProvider4);
			frame.SetDynamicResource(VisualElement.BackgroundColorProperty, dynamicResource4.Key);
			frame.SetValue(Frame.BorderColorProperty, Color.White);
			frame.SetValue(View.HorizontalOptionsProperty, LayoutOptions.Center);
			bindingExtension2.Mode = 2;
			bindingExtension2.Path = "IsCancelVisible";
			BindingBase bindingBase2 = bindingExtension2.ProvideValue(null);
			frame.SetBinding(VisualElement.IsVisibleProperty, bindingBase2);
			linkButton.SetValue(VisualElement.BackgroundColorProperty, Color.Transparent);
			linkButton.Clicked += this.BtnCancel_Clicked;
			bindingExtension3.Mode = 2;
			bindingExtension3.Path = "CancelText";
			BindingBase bindingBase3 = bindingExtension3.ProvideValue(null);
			linkButton.SetBinding(Button.TextProperty, bindingBase3);
			frame.SetValue(ContentView.ContentProperty, linkButton);
			grid.Children.Add(frame);
			frame2.SetValue(ContentView.ContentProperty, grid);
			this.SetValue(ContentView.ContentProperty, frame2);
		}

		// Token: 0x06001605 RID: 5637 RVA: 0x0009BA2C File Offset: 0x00099C2C
		// Note: this type is marked as 'beforefieldinit'.
		static ActivityFrame()
		{
		}

		// Token: 0x06001606 RID: 5638 RVA: 0x0009BAD0 File Offset: 0x00099CD0
		private void __InitComponentRuntime()
		{
			Extensions.LoadFromXaml<ActivityFrame>(this, typeof(ActivityFrame));
			this.label = NameScopeExtensions.FindByName<Label>(this, "label");
			this.cancelFrame = NameScopeExtensions.FindByName<Frame>(this, "cancelFrame");
			this.btnCancel = NameScopeExtensions.FindByName<LinkButton>(this, "btnCancel");
		}

		// Token: 0x04000656 RID: 1622
		private string _Text = "";

		// Token: 0x04000657 RID: 1623
		public static BindableProperty TextProperty = BindableProperty.Create("Text", typeof(string), typeof(ActivityFrame), Translate.GetString("ios_PleaseWait"), 0, null, null, null, null, null);

		// Token: 0x04000658 RID: 1624
		public static BindableProperty CancelTextProperty = BindableProperty.Create("CancelText", typeof(string), typeof(ActivityFrame), Translate.GetString("btnCancel.Content"), 0, null, null, null, null, null);

		// Token: 0x04000659 RID: 1625
		public static BindableProperty IsCancelVisibleProperty = BindableProperty.Create("IsCancelVisible", typeof(bool), typeof(ActivityFrame), false, 0, null, null, null, null, null);

		// Token: 0x0400065A RID: 1626
		public EventHandler CancelClicked;

		// Token: 0x0400065B RID: 1627
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private Label label;

		// Token: 0x0400065C RID: 1628
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private Frame cancelFrame;

		// Token: 0x0400065D RID: 1629
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private LinkButton btnCancel;
	}
}
