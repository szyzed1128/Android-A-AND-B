using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using CarScannerXamarinForms.OBD2.PIDS;
using Xamarin.Forms;
using Xamarin.Forms.Internals;
using Xamarin.Forms.PlatformConfiguration.iOSSpecific;
using Xamarin.Forms.Xaml;
using Xamarin.Forms.Xaml.Diagnostics;
using Xamarin.Forms.Xaml.Internals;

namespace CarScannerXamarinForms
{
	// Token: 0x02000134 RID: 308
	[XamlFilePath("Pages\\SpeedTestAdder.xaml")]
	public class SpeedTestAdder : ContentPage
	{
		// Token: 0x060005C8 RID: 1480 RVA: 0x0005C246 File Offset: 0x0005A446
		public SpeedTestAdder(SpeedTestViewModel model)
		{
			this.InitializeComponent();
			base.Disappearing += this.SpeedTestAdder_Disappearing;
			this.model = model;
		}

		// Token: 0x060005C9 RID: 1481 RVA: 0x0005C270 File Offset: 0x0005A470
		private void SpeedTestAdder_Disappearing(object sender, EventArgs e)
		{
			try
			{
				if (base.Navigation.ModalStack.Contains(this))
				{
					base.Navigation.PopAsync();
				}
			}
			catch
			{
			}
		}

		// Token: 0x060005CA RID: 1482 RVA: 0x0005C2B4 File Offset: 0x0005A4B4
		private void ComboTest_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (this.comboTest.SelectedIndex >= 3)
			{
				this.panelFromTo.IsVisible = false;
				return;
			}
			this.panelFromTo.IsVisible = true;
		}

		// Token: 0x060005CB RID: 1483 RVA: 0x000027D4 File Offset: 0x000009D4
		private void Handle_SizeChanged(object sender, EventArgs e)
		{
		}

		// Token: 0x060005CC RID: 1484 RVA: 0x0005C2E0 File Offset: 0x0005A4E0
		private async void btnBack_Clicked(object sender, EventArgs e)
		{
			App.OBDReader.ClearRequestQueue();
			await base.Navigation.PopAsync(true);
		}

		// Token: 0x060005CD RID: 1485 RVA: 0x0005C318 File Offset: 0x0005A518
		private void Handle_TextChanged(object sender, TextChangedEventArgs e)
		{
			if (e.NewTextValue == "")
			{
				return;
			}
			Entry entry = sender as Entry;
			int num;
			if (!int.TryParse(e.NewTextValue, out num))
			{
				if (int.TryParse(e.OldTextValue, out num))
				{
					entry.Text = e.OldTextValue;
					return;
				}
				entry.Text = "0";
			}
		}

		// Token: 0x060005CE RID: 1486 RVA: 0x0005C378 File Offset: 0x0005A578
		private ISpeedTest BuildSpeedTest()
		{
			int num;
			int.TryParse(this.numUpDownFrom.Text, out num);
			int num2;
			int.TryParse(this.numUpDownTo.Text, out num2);
			ISpeedTest speedTest;
			switch (this.comboTest.SelectedIndex)
			{
			case 1:
			{
				string text = string.Concat(new string[]
				{
					Translate.GetString("SpeedTest_BrakeTime"),
					" ",
					num.ToString(CultureInfo.InvariantCulture),
					"-",
					num2.ToString(CultureInfo.InvariantCulture),
					" ",
					UnitsHelper.GetCaption(UnitsHelper.Units.kmh)
				});
				speedTest = new BrakeTimeTest(App.OBDReader, text, (double)num, (double)num2);
				goto IL_01AB;
			}
			case 2:
			{
				string text2 = string.Concat(new string[]
				{
					Translate.GetString("SpeedTest_BrakeDistance"),
					" ",
					num.ToString(CultureInfo.InvariantCulture),
					"-",
					num2.ToString(CultureInfo.InvariantCulture),
					" ",
					UnitsHelper.GetCaption(UnitsHelper.Units.kmh)
				});
				speedTest = new BrakeDistanceTest(App.OBDReader, text2, (double)num, (double)num2);
				goto IL_01AB;
			}
			case 3:
			{
				string text3 = "1/4 mile / 402m.";
				speedTest = new Acceleration402mTest(App.OBDReader, text3);
				goto IL_01AB;
			}
			}
			string text4 = string.Concat(new string[]
			{
				Translate.GetString("SpeedTest_AccelerationTime"),
				" ",
				num.ToString(CultureInfo.InvariantCulture),
				"-",
				num2.ToString(CultureInfo.InvariantCulture),
				" ",
				UnitsHelper.GetCaption(UnitsHelper.Units.kmh)
			});
			speedTest = new SpeedTestV1(App.OBDReader, text4, (double)num, (double)num2);
			IL_01AB:
			speedTest.Start();
			return speedTest;
		}

		// Token: 0x060005CF RID: 1487 RVA: 0x0005C538 File Offset: 0x0005A738
		private void Handle_Clicked(object sender, EventArgs e)
		{
			ISpeedTest speedTest = this.BuildSpeedTest();
			this.model.TestCollection.Add(speedTest);
			this.model.SaveToFile();
			this.model.Stop();
			this.model.LoadFromFile();
			this.model.Start();
			base.Navigation.PopAsync(true);
		}

		// Token: 0x060005D0 RID: 1488 RVA: 0x0005C598 File Offset: 0x0005A798
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private void InitializeComponent()
		{
			if (ResourceLoader.IsEnabled && ResourceLoader.CanProvideContentFor(new ResourceLoader.ResourceLoadingQuery
			{
				AssemblyName = typeof(SpeedTestAdder).GetTypeInfo().Assembly.GetName(),
				ResourcePath = "Pages/SpeedTestAdder.xaml",
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
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension = new DynamicResourceExtension(), new Uri("Pages\\SpeedTestAdder.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 10, 5);
			On on;
			VisualDiagnostics.RegisterSourceInfo(on = new On(), new Uri("Pages\\SpeedTestAdder.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 16, 14);
			On on2;
			VisualDiagnostics.RegisterSourceInfo(on2 = new On(), new Uri("Pages\\SpeedTestAdder.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 17, 14);
			OnPlatform<Thickness> onPlatform;
			VisualDiagnostics.RegisterSourceInfo(onPlatform = new OnPlatform<Thickness>(), new Uri("Pages\\SpeedTestAdder.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 15, 10);
			ColumnDefinition columnDefinition;
			VisualDiagnostics.RegisterSourceInfo(columnDefinition = new ColumnDefinition(), new Uri("Pages\\SpeedTestAdder.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 23, 18);
			ColumnDefinition columnDefinition2;
			VisualDiagnostics.RegisterSourceInfo(columnDefinition2 = new ColumnDefinition(), new Uri("Pages\\SpeedTestAdder.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 24, 18);
			ColumnDefinition columnDefinition3;
			VisualDiagnostics.RegisterSourceInfo(columnDefinition3 = new ColumnDefinition(), new Uri("Pages\\SpeedTestAdder.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 25, 18);
			DynamicResourceExtension dynamicResourceExtension2;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension2 = new DynamicResourceExtension(), new Uri("Pages\\SpeedTestAdder.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 31, 17);
			Translate translate;
			VisualDiagnostics.RegisterSourceInfo(translate = new Translate(), new Uri("Pages\\SpeedTestAdder.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 32, 17);
			OnPlatform<bool> onPlatform2;
			VisualDiagnostics.RegisterSourceInfo(onPlatform2 = new OnPlatform<bool>(), new Uri("Pages\\SpeedTestAdder.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 34, 22);
			LinkButton linkButton;
			VisualDiagnostics.RegisterSourceInfo(linkButton = new LinkButton(), new Uri("Pages\\SpeedTestAdder.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 27, 14);
			DynamicResourceExtension dynamicResourceExtension3;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension3 = new DynamicResourceExtension(), new Uri("Pages\\SpeedTestAdder.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 44, 17);
			Translate translate2;
			VisualDiagnostics.RegisterSourceInfo(translate2 = new Translate(), new Uri("Pages\\SpeedTestAdder.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 45, 17);
			Label label;
			VisualDiagnostics.RegisterSourceInfo(label = new Label(), new Uri("Pages\\SpeedTestAdder.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 41, 14);
			Grid grid;
			VisualDiagnostics.RegisterSourceInfo(grid = new Grid(), new Uri("Pages\\SpeedTestAdder.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 21, 10);
			RowDefinition rowDefinition;
			VisualDiagnostics.RegisterSourceInfo(rowDefinition = new RowDefinition(), new Uri("Pages\\SpeedTestAdder.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 52, 18);
			RowDefinition rowDefinition2;
			VisualDiagnostics.RegisterSourceInfo(rowDefinition2 = new RowDefinition(), new Uri("Pages\\SpeedTestAdder.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 53, 18);
			RowDefinition rowDefinition3;
			VisualDiagnostics.RegisterSourceInfo(rowDefinition3 = new RowDefinition(), new Uri("Pages\\SpeedTestAdder.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 54, 18);
			Translate translate3;
			VisualDiagnostics.RegisterSourceInfo(translate3 = new Translate(), new Uri("Pages\\SpeedTestAdder.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 58, 24);
			Label label2;
			VisualDiagnostics.RegisterSourceInfo(label2 = new Label(), new Uri("Pages\\SpeedTestAdder.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 58, 18);
			List<string> speedTestTypesList;
			VisualDiagnostics.RegisterSourceInfo(speedTestTypesList = StaticLists.SpeedTestTypesList, new Uri("Pages\\SpeedTestAdder.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 61, 21);
			BindingExtension bindingExtension;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension = new BindingExtension(), new Uri("Pages\\SpeedTestAdder.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 61, 21);
			Picker picker;
			VisualDiagnostics.RegisterSourceInfo(picker = new Picker(), new Uri("Pages\\SpeedTestAdder.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 59, 18);
			Translate translate4;
			VisualDiagnostics.RegisterSourceInfo(translate4 = new Translate(), new Uri("Pages\\SpeedTestAdder.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 65, 28);
			Label label3;
			VisualDiagnostics.RegisterSourceInfo(label3 = new Label(), new Uri("Pages\\SpeedTestAdder.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 65, 22);
			Entry entry;
			VisualDiagnostics.RegisterSourceInfo(entry = new Entry(), new Uri("Pages\\SpeedTestAdder.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 66, 22);
			Translate translate5;
			VisualDiagnostics.RegisterSourceInfo(translate5 = new Translate(), new Uri("Pages\\SpeedTestAdder.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 71, 28);
			Label label4;
			VisualDiagnostics.RegisterSourceInfo(label4 = new Label(), new Uri("Pages\\SpeedTestAdder.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 71, 22);
			Entry entry2;
			VisualDiagnostics.RegisterSourceInfo(entry2 = new Entry(), new Uri("Pages\\SpeedTestAdder.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 72, 22);
			StackLayout stackLayout;
			VisualDiagnostics.RegisterSourceInfo(stackLayout = new StackLayout(), new Uri("Pages\\SpeedTestAdder.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 64, 18);
			StackLayout stackLayout2;
			VisualDiagnostics.RegisterSourceInfo(stackLayout2 = new StackLayout(), new Uri("Pages\\SpeedTestAdder.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 57, 14);
			Button button;
			VisualDiagnostics.RegisterSourceInfo(button = new Button(), new Uri("Pages\\SpeedTestAdder.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 81, 14);
			Grid grid2;
			VisualDiagnostics.RegisterSourceInfo(grid2 = new Grid(), new Uri("Pages\\SpeedTestAdder.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 50, 10);
			VisualDiagnostics.RegisterSourceInfo(this, new Uri("Pages\\SpeedTestAdder.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 2, 2);
			NameScope nameScope = NameScope.GetNameScope(this) ?? new NameScope();
			NameScope.SetNameScope(this, nameScope);
			nameScope.RegisterName("gridButtons", grid);
			if (grid.StyleId == null)
			{
				grid.StyleId = "gridButtons";
			}
			nameScope.RegisterName("LayoutRoot", grid2);
			if (grid2.StyleId == null)
			{
				grid2.StyleId = "LayoutRoot";
			}
			nameScope.RegisterName("comboTest", picker);
			if (picker.StyleId == null)
			{
				picker.StyleId = "comboTest";
			}
			nameScope.RegisterName("panelFromTo", stackLayout);
			if (stackLayout.StyleId == null)
			{
				stackLayout.StyleId = "panelFromTo";
			}
			nameScope.RegisterName("numUpDownFrom", entry);
			if (entry.StyleId == null)
			{
				entry.StyleId = "numUpDownFrom";
			}
			nameScope.RegisterName("numUpDownTo", entry2);
			if (entry2.StyleId == null)
			{
				entry2.StyleId = "numUpDownTo";
			}
			nameScope.RegisterName("btnOK", button);
			if (button.StyleId == null)
			{
				button.StyleId = "btnOK";
			}
			this.gridButtons = grid;
			this.LayoutRoot = grid2;
			this.comboTest = picker;
			this.panelFromTo = stackLayout;
			this.numUpDownFrom = entry;
			this.numUpDownTo = entry2;
			this.btnOK = button;
			this.SetValue(Page.PrefersStatusBarHiddenProperty, 2);
			this.SetValue(Page.UseSafeAreaProperty, true);
			dynamicResourceExtension.Key = "BackgroundColor";
			IMarkupExtension<DynamicResource> markupExtension = dynamicResourceExtension;
			XamlServiceProvider xamlServiceProvider = new XamlServiceProvider();
			Type typeFromHandle = typeof(IProvideValueTarget);
			object[] array = new object[0 + 1];
			array[0] = this;
			object obj;
			xamlServiceProvider.Add(typeFromHandle, obj = new SimpleValueTargetProvider(array, VisualElement.BackgroundColorProperty, nameScope));
			xamlServiceProvider.Add(typeof(IReferenceProvider), obj);
			Type typeFromHandle2 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver = new XmlNamespaceResolver();
			xmlNamespaceResolver.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver.Add("local", "clr-namespace:CarScannerXamarinForms");
			xamlServiceProvider.Add(typeFromHandle2, new XamlTypeResolver(xmlNamespaceResolver, typeof(SpeedTestAdder).GetTypeInfo().Assembly));
			xamlServiceProvider.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(10, 5)));
			DynamicResource dynamicResource = markupExtension.ProvideValue(xamlServiceProvider);
			this.SetDynamicResource(VisualElement.BackgroundColorProperty, dynamicResource.Key);
			this.SetValue(NavigationPage.HasBackButtonProperty, false);
			this.SetValue(NavigationPage.HasNavigationBarProperty, true);
			this.SizeChanged += this.Handle_SizeChanged;
			on.Platform = new List<string>(2) { "Android", "WinPhone" };
			on.Value = "0";
			onPlatform.Platforms.Add(on);
			on2.Platform = new List<string>(1) { "iOS" };
			on2.Value = "5,20,5,0";
			onPlatform.Platforms.Add(on2);
			this.SetValue(Page.PaddingProperty, onPlatform);
			grid.SetValue(Grid.RowProperty, 0);
			columnDefinition.SetValue(ColumnDefinition.WidthProperty, new GridLengthTypeConverter().ConvertFromInvariantString("*"));
			grid.GetValue(Grid.ColumnDefinitionsProperty).Add(columnDefinition);
			columnDefinition2.SetValue(ColumnDefinition.WidthProperty, new GridLengthTypeConverter().ConvertFromInvariantString("auto"));
			grid.GetValue(Grid.ColumnDefinitionsProperty).Add(columnDefinition2);
			columnDefinition3.SetValue(ColumnDefinition.WidthProperty, new GridLengthTypeConverter().ConvertFromInvariantString("*"));
			grid.GetValue(Grid.ColumnDefinitionsProperty).Add(columnDefinition3);
			linkButton.SetValue(Grid.ColumnProperty, 0);
			linkButton.Clicked += this.btnBack_Clicked;
			linkButton.SetValue(View.HorizontalOptionsProperty, LayoutOptions.Start);
			dynamicResourceExtension2.Key = "NavigationBarButton";
			IMarkupExtension<DynamicResource> markupExtension2 = dynamicResourceExtension2;
			XamlServiceProvider xamlServiceProvider2 = new XamlServiceProvider();
			Type typeFromHandle3 = typeof(IProvideValueTarget);
			object[] array2 = new object[0 + 3];
			array2[0] = linkButton;
			array2[1] = grid;
			array2[2] = this;
			object obj2;
			xamlServiceProvider2.Add(typeFromHandle3, obj2 = new SimpleValueTargetProvider(array2, VisualElement.StyleProperty, nameScope));
			xamlServiceProvider2.Add(typeof(IReferenceProvider), obj2);
			Type typeFromHandle4 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver2 = new XmlNamespaceResolver();
			xmlNamespaceResolver2.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver2.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver2.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver2.Add("local", "clr-namespace:CarScannerXamarinForms");
			xamlServiceProvider2.Add(typeFromHandle4, new XamlTypeResolver(xmlNamespaceResolver2, typeof(SpeedTestAdder).GetTypeInfo().Assembly));
			xamlServiceProvider2.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(31, 17)));
			DynamicResource dynamicResource2 = markupExtension2.ProvideValue(xamlServiceProvider2);
			linkButton.SetDynamicResource(VisualElement.StyleProperty, dynamicResource2.Key);
			translate.Text = "ios_Cancel";
			IMarkupExtension markupExtension3 = translate;
			XamlServiceProvider xamlServiceProvider3 = new XamlServiceProvider();
			Type typeFromHandle5 = typeof(IProvideValueTarget);
			object[] array3 = new object[0 + 3];
			array3[0] = linkButton;
			array3[1] = grid;
			array3[2] = this;
			object obj3;
			xamlServiceProvider3.Add(typeFromHandle5, obj3 = new SimpleValueTargetProvider(array3, Button.TextProperty, nameScope));
			xamlServiceProvider3.Add(typeof(IReferenceProvider), obj3);
			Type typeFromHandle6 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver3 = new XmlNamespaceResolver();
			xmlNamespaceResolver3.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver3.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver3.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver3.Add("local", "clr-namespace:CarScannerXamarinForms");
			xamlServiceProvider3.Add(typeFromHandle6, new XamlTypeResolver(xmlNamespaceResolver3, typeof(SpeedTestAdder).GetTypeInfo().Assembly));
			xamlServiceProvider3.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(32, 17)));
			object obj4 = markupExtension3.ProvideValue(xamlServiceProvider3);
			linkButton.Text = obj4;
			onPlatform2.iOS = true;
			onPlatform2.Android = false;
			linkButton.SetValue(VisualElement.IsVisibleProperty, onPlatform2);
			grid.Children.Add(linkButton);
			label.SetValue(Grid.ColumnProperty, 1);
			label.SetValue(View.HorizontalOptionsProperty, LayoutOptions.Center);
			dynamicResourceExtension3.Key = "NavigationBarLabel";
			IMarkupExtension<DynamicResource> markupExtension4 = dynamicResourceExtension3;
			XamlServiceProvider xamlServiceProvider4 = new XamlServiceProvider();
			Type typeFromHandle7 = typeof(IProvideValueTarget);
			object[] array4 = new object[0 + 3];
			array4[0] = label;
			array4[1] = grid;
			array4[2] = this;
			object obj5;
			xamlServiceProvider4.Add(typeFromHandle7, obj5 = new SimpleValueTargetProvider(array4, VisualElement.StyleProperty, nameScope));
			xamlServiceProvider4.Add(typeof(IReferenceProvider), obj5);
			Type typeFromHandle8 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver4 = new XmlNamespaceResolver();
			xmlNamespaceResolver4.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver4.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver4.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver4.Add("local", "clr-namespace:CarScannerXamarinForms");
			xamlServiceProvider4.Add(typeFromHandle8, new XamlTypeResolver(xmlNamespaceResolver4, typeof(SpeedTestAdder).GetTypeInfo().Assembly));
			xamlServiceProvider4.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(44, 17)));
			DynamicResource dynamicResource3 = markupExtension4.ProvideValue(xamlServiceProvider4);
			label.SetDynamicResource(VisualElement.StyleProperty, dynamicResource3.Key);
			translate2.Text = "ios_NewSpeedTest";
			IMarkupExtension markupExtension5 = translate2;
			XamlServiceProvider xamlServiceProvider5 = new XamlServiceProvider();
			Type typeFromHandle9 = typeof(IProvideValueTarget);
			object[] array5 = new object[0 + 3];
			array5[0] = label;
			array5[1] = grid;
			array5[2] = this;
			object obj6;
			xamlServiceProvider5.Add(typeFromHandle9, obj6 = new SimpleValueTargetProvider(array5, Label.TextProperty, nameScope));
			xamlServiceProvider5.Add(typeof(IReferenceProvider), obj6);
			Type typeFromHandle10 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver5 = new XmlNamespaceResolver();
			xmlNamespaceResolver5.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver5.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver5.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver5.Add("local", "clr-namespace:CarScannerXamarinForms");
			xamlServiceProvider5.Add(typeFromHandle10, new XamlTypeResolver(xmlNamespaceResolver5, typeof(SpeedTestAdder).GetTypeInfo().Assembly));
			xamlServiceProvider5.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(45, 17)));
			object obj7 = markupExtension5.ProvideValue(xamlServiceProvider5);
			label.Text = obj7;
			grid.Children.Add(label);
			this.SetValue(NavigationPage.TitleViewProperty, grid);
			grid2.SetValue(View.MarginProperty, new Thickness(5.0, 0.0, 5.0, 0.0));
			rowDefinition.SetValue(RowDefinition.HeightProperty, new GridLengthTypeConverter().ConvertFromInvariantString("auto"));
			grid2.GetValue(Grid.RowDefinitionsProperty).Add(rowDefinition);
			rowDefinition2.SetValue(RowDefinition.HeightProperty, new GridLengthTypeConverter().ConvertFromInvariantString("auto"));
			grid2.GetValue(Grid.RowDefinitionsProperty).Add(rowDefinition2);
			rowDefinition3.SetValue(RowDefinition.HeightProperty, new GridLengthTypeConverter().ConvertFromInvariantString("auto"));
			grid2.GetValue(Grid.RowDefinitionsProperty).Add(rowDefinition3);
			stackLayout2.SetValue(Grid.RowProperty, 1);
			stackLayout2.SetValue(StackLayout.OrientationProperty, 0);
			translate3.Text = "SpeedTest_tbTestType.Text";
			IMarkupExtension markupExtension6 = translate3;
			XamlServiceProvider xamlServiceProvider6 = new XamlServiceProvider();
			Type typeFromHandle11 = typeof(IProvideValueTarget);
			object[] array6 = new object[0 + 4];
			array6[0] = label2;
			array6[1] = stackLayout2;
			array6[2] = grid2;
			array6[3] = this;
			object obj8;
			xamlServiceProvider6.Add(typeFromHandle11, obj8 = new SimpleValueTargetProvider(array6, Label.TextProperty, nameScope));
			xamlServiceProvider6.Add(typeof(IReferenceProvider), obj8);
			Type typeFromHandle12 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver6 = new XmlNamespaceResolver();
			xmlNamespaceResolver6.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver6.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver6.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver6.Add("local", "clr-namespace:CarScannerXamarinForms");
			xamlServiceProvider6.Add(typeFromHandle12, new XamlTypeResolver(xmlNamespaceResolver6, typeof(SpeedTestAdder).GetTypeInfo().Assembly));
			xamlServiceProvider6.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(58, 24)));
			object obj9 = markupExtension6.ProvideValue(xamlServiceProvider6);
			label2.Text = obj9;
			stackLayout2.Children.Add(label2);
			bindingExtension.Source = speedTestTypesList;
			BindingBase bindingBase = bindingExtension.ProvideValue(null);
			picker.SetBinding(Picker.ItemsSourceProperty, bindingBase);
			picker.SetValue(Picker.SelectedIndexProperty, 0);
			picker.SelectedIndexChanged += this.ComboTest_SelectedIndexChanged;
			stackLayout2.Children.Add(picker);
			stackLayout.SetValue(StackLayout.OrientationProperty, 0);
			translate4.Text = "SpeedTest_SpeedFrom.Text";
			IMarkupExtension markupExtension7 = translate4;
			XamlServiceProvider xamlServiceProvider7 = new XamlServiceProvider();
			Type typeFromHandle13 = typeof(IProvideValueTarget);
			object[] array7 = new object[0 + 5];
			array7[0] = label3;
			array7[1] = stackLayout;
			array7[2] = stackLayout2;
			array7[3] = grid2;
			array7[4] = this;
			object obj10;
			xamlServiceProvider7.Add(typeFromHandle13, obj10 = new SimpleValueTargetProvider(array7, Label.TextProperty, nameScope));
			xamlServiceProvider7.Add(typeof(IReferenceProvider), obj10);
			Type typeFromHandle14 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver7 = new XmlNamespaceResolver();
			xmlNamespaceResolver7.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver7.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver7.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver7.Add("local", "clr-namespace:CarScannerXamarinForms");
			xamlServiceProvider7.Add(typeFromHandle14, new XamlTypeResolver(xmlNamespaceResolver7, typeof(SpeedTestAdder).GetTypeInfo().Assembly));
			xamlServiceProvider7.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(65, 28)));
			object obj11 = markupExtension7.ProvideValue(xamlServiceProvider7);
			label3.Text = obj11;
			stackLayout.Children.Add(label3);
			entry.SetValue(InputView.KeyboardProperty, new KeyboardTypeConverter().ConvertFromInvariantString("Numeric"));
			entry.SetValue(Entry.TextProperty, "0");
			entry.TextChanged += this.Handle_TextChanged;
			stackLayout.Children.Add(entry);
			translate5.Text = "SpeedTest_SpeedTo.Text";
			IMarkupExtension markupExtension8 = translate5;
			XamlServiceProvider xamlServiceProvider8 = new XamlServiceProvider();
			Type typeFromHandle15 = typeof(IProvideValueTarget);
			object[] array8 = new object[0 + 5];
			array8[0] = label4;
			array8[1] = stackLayout;
			array8[2] = stackLayout2;
			array8[3] = grid2;
			array8[4] = this;
			object obj12;
			xamlServiceProvider8.Add(typeFromHandle15, obj12 = new SimpleValueTargetProvider(array8, Label.TextProperty, nameScope));
			xamlServiceProvider8.Add(typeof(IReferenceProvider), obj12);
			Type typeFromHandle16 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver8 = new XmlNamespaceResolver();
			xmlNamespaceResolver8.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver8.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver8.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver8.Add("local", "clr-namespace:CarScannerXamarinForms");
			xamlServiceProvider8.Add(typeFromHandle16, new XamlTypeResolver(xmlNamespaceResolver8, typeof(SpeedTestAdder).GetTypeInfo().Assembly));
			xamlServiceProvider8.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(71, 28)));
			object obj13 = markupExtension8.ProvideValue(xamlServiceProvider8);
			label4.Text = obj13;
			stackLayout.Children.Add(label4);
			entry2.SetValue(InputView.KeyboardProperty, new KeyboardTypeConverter().ConvertFromInvariantString("Numeric"));
			entry2.SetValue(Entry.TextProperty, "100");
			entry2.TextChanged += this.Handle_TextChanged;
			stackLayout.Children.Add(entry2);
			stackLayout2.Children.Add(stackLayout);
			grid2.Children.Add(stackLayout2);
			button.SetValue(Grid.RowProperty, 2);
			button.Clicked += this.Handle_Clicked;
			button.SetValue(View.HorizontalOptionsProperty, LayoutOptions.Center);
			button.SetValue(Button.TextProperty, "OK");
			button.SetValue(View.VerticalOptionsProperty, LayoutOptions.Start);
			grid2.Children.Add(button);
			this.SetValue(ContentPage.ContentProperty, grid2);
		}

		// Token: 0x060005D1 RID: 1489 RVA: 0x0005D9CC File Offset: 0x0005BBCC
		private void __InitComponentRuntime()
		{
			Extensions.LoadFromXaml<SpeedTestAdder>(this, typeof(SpeedTestAdder));
			this.gridButtons = NameScopeExtensions.FindByName<Grid>(this, "gridButtons");
			this.LayoutRoot = NameScopeExtensions.FindByName<Grid>(this, "LayoutRoot");
			this.comboTest = NameScopeExtensions.FindByName<Picker>(this, "comboTest");
			this.panelFromTo = NameScopeExtensions.FindByName<StackLayout>(this, "panelFromTo");
			this.numUpDownFrom = NameScopeExtensions.FindByName<Entry>(this, "numUpDownFrom");
			this.numUpDownTo = NameScopeExtensions.FindByName<Entry>(this, "numUpDownTo");
			this.btnOK = NameScopeExtensions.FindByName<Button>(this, "btnOK");
		}

		// Token: 0x040004B3 RID: 1203
		private SpeedTestViewModel model;

		// Token: 0x040004B4 RID: 1204
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private Grid gridButtons;

		// Token: 0x040004B5 RID: 1205
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private Grid LayoutRoot;

		// Token: 0x040004B6 RID: 1206
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private Picker comboTest;

		// Token: 0x040004B7 RID: 1207
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private StackLayout panelFromTo;

		// Token: 0x040004B8 RID: 1208
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private Entry numUpDownFrom;

		// Token: 0x040004B9 RID: 1209
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private Entry numUpDownTo;

		// Token: 0x040004BA RID: 1210
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private Button btnOK;

		// Token: 0x02000135 RID: 309
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <btnBack_Clicked>d__4 : IAsyncStateMachine
		{
			// Token: 0x060005D2 RID: 1490 RVA: 0x0005DA64 File Offset: 0x0005BC64
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				SpeedTestAdder speedTestAdder = this;
				try
				{
					TaskAwaiter<Page> taskAwaiter;
					if (num != 0)
					{
						App.OBDReader.ClearRequestQueue();
						taskAwaiter = speedTestAdder.Navigation.PopAsync(true).GetAwaiter();
						if (!taskAwaiter.IsCompleted)
						{
							num2 = 0;
							TaskAwaiter<Page> taskAwaiter2 = taskAwaiter;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<Page>, SpeedTestAdder.<btnBack_Clicked>d__4>(ref taskAwaiter, ref this);
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

			// Token: 0x060005D3 RID: 1491 RVA: 0x0005DB28 File Offset: 0x0005BD28
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x040004BB RID: 1211
			public int <>1__state;

			// Token: 0x040004BC RID: 1212
			public AsyncVoidMethodBuilder <>t__builder;

			// Token: 0x040004BD RID: 1213
			public SpeedTestAdder <>4__this;

			// Token: 0x040004BE RID: 1214
			private TaskAwaiter<Page> <>u__1;
		}
	}
}
