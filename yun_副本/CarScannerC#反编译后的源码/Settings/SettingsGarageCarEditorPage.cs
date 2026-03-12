using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using CarScannerXamarinForms.Common;
using CarScannerXamarinForms.Garage;
using Xamarin.Forms;
using Xamarin.Forms.Internals;
using Xamarin.Forms.PlatformConfiguration.iOSSpecific;
using Xamarin.Forms.Xaml;
using Xamarin.Forms.Xaml.Diagnostics;
using Xamarin.Forms.Xaml.Internals;

namespace CarScannerXamarinForms.Settings
{
	// Token: 0x02000240 RID: 576
	[XamlFilePath("Settings\\SettingsGarageCarEditorPage.xaml")]
	public class SettingsGarageCarEditorPage : ContentPage
	{
		// Token: 0x06001B46 RID: 6982 RVA: 0x001316F0 File Offset: 0x0012F8F0
		public SettingsGarageCarEditorPage(MyCar car, GarageModel model)
		{
			this.InitializeComponent();
			List<int> list = new List<int>(64);
			int num = 1990;
			int num2 = DateTimeNowHelper.NowSafe.Year + 1;
			for (int i = num; i <= num2; i++)
			{
				list.Add(i);
			}
			this.pickerYear.ItemsSource = list;
			if (car != null)
			{
				this.car = car;
			}
			else
			{
				this.car = new MyCar
				{
					Name = Translate.GetString("ios_MY_CAR"),
					Year = DateTimeNowHelper.NowSafe.Year,
					Id = DateTimeNowHelper.NowSafe.Ticks
				};
				this.createNew = true;
			}
			this.pickerYear.SelectedItem = this.car.Year;
			this.entry_CarName.Text = this.car.Name;
			this.model = model;
		}

		// Token: 0x06001B47 RID: 6983 RVA: 0x000027D4 File Offset: 0x000009D4
		private void Handle_SizeChanged(object sender, EventArgs e)
		{
		}

		// Token: 0x06001B48 RID: 6984 RVA: 0x001317D0 File Offset: 0x0012F9D0
		private async void btnOK_Clicked(object sender, EventArgs e)
		{
			if (this.IsNameUnique())
			{
				this.car.Name = this.entry_CarName.Text;
				this.car.Year = (int)this.pickerYear.SelectedItem;
				if (this.createNew)
				{
					this.model.Cars.Add(this.car);
				}
				this.activityFrame.IsVisible = true;
				this.btnOK.IsEnabled = false;
				this.btnCancel.IsEnabled = false;
				this.model.SaveToFile();
				if (SharedSettings.Current.CurrentCarId == this.car.Id)
				{
					SharedSettings.Current.CurrentCarName = this.car.Name;
				}
				this.activityFrame.IsVisible = false;
				await base.Navigation.PopAsync();
			}
			else
			{
				await base.DisplayAlert(Translate.GetString("ios_WrongCarName"), Translate.GetString("ios_CarWithSameNameExists"), "OK");
			}
		}

		// Token: 0x06001B49 RID: 6985 RVA: 0x00026430 File Offset: 0x00024630
		private void btnCancel_Clicked(object sender, EventArgs e)
		{
			base.Navigation.PopAsync();
		}

		// Token: 0x06001B4A RID: 6986 RVA: 0x00131807 File Offset: 0x0012FA07
		private bool IsNameUnique()
		{
			return !this.model.Cars.Where((MyCar x) => x != this.car).Any((MyCar x) => x.Name.Equals(this.entry_CarName.Text, StringComparison.InvariantCultureIgnoreCase));
		}

		// Token: 0x06001B4B RID: 6987 RVA: 0x0013183C File Offset: 0x0012FA3C
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private void InitializeComponent()
		{
			if (ResourceLoader.IsEnabled && ResourceLoader.CanProvideContentFor(new ResourceLoader.ResourceLoadingQuery
			{
				AssemblyName = typeof(SettingsGarageCarEditorPage).GetTypeInfo().Assembly.GetName(),
				ResourcePath = "Settings/SettingsGarageCarEditorPage.xaml",
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
			VisualDiagnostics.RegisterSourceInfo(translate = new Translate(), new Uri("Settings\\SettingsGarageCarEditorPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 8, 5);
			DynamicResourceExtension dynamicResourceExtension;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension = new DynamicResourceExtension(), new Uri("Settings\\SettingsGarageCarEditorPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 11, 5);
			ActivityFrame activityFrame;
			VisualDiagnostics.RegisterSourceInfo(activityFrame = new ActivityFrame(), new Uri("Settings\\SettingsGarageCarEditorPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 20, 18);
			Translate translate2;
			VisualDiagnostics.RegisterSourceInfo(translate2 = new Translate(), new Uri("Settings\\SettingsGarageCarEditorPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 25, 24);
			Label label;
			VisualDiagnostics.RegisterSourceInfo(label = new Label(), new Uri("Settings\\SettingsGarageCarEditorPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 25, 18);
			Entry entry;
			VisualDiagnostics.RegisterSourceInfo(entry = new Entry(), new Uri("Settings\\SettingsGarageCarEditorPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 26, 18);
			Translate translate3;
			VisualDiagnostics.RegisterSourceInfo(translate3 = new Translate(), new Uri("Settings\\SettingsGarageCarEditorPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 27, 24);
			Label label2;
			VisualDiagnostics.RegisterSourceInfo(label2 = new Label(), new Uri("Settings\\SettingsGarageCarEditorPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 27, 18);
			Picker picker;
			VisualDiagnostics.RegisterSourceInfo(picker = new Picker(), new Uri("Settings\\SettingsGarageCarEditorPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 28, 18);
			ColumnDefinition columnDefinition;
			VisualDiagnostics.RegisterSourceInfo(columnDefinition = new ColumnDefinition(), new Uri("Settings\\SettingsGarageCarEditorPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 31, 26);
			ColumnDefinition columnDefinition2;
			VisualDiagnostics.RegisterSourceInfo(columnDefinition2 = new ColumnDefinition(), new Uri("Settings\\SettingsGarageCarEditorPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 32, 26);
			Button button;
			VisualDiagnostics.RegisterSourceInfo(button = new Button(), new Uri("Settings\\SettingsGarageCarEditorPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 34, 22);
			Translate translate4;
			VisualDiagnostics.RegisterSourceInfo(translate4 = new Translate(), new Uri("Settings\\SettingsGarageCarEditorPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 46, 25);
			Button button2;
			VisualDiagnostics.RegisterSourceInfo(button2 = new Button(), new Uri("Settings\\SettingsGarageCarEditorPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 41, 22);
			Grid grid;
			VisualDiagnostics.RegisterSourceInfo(grid = new Grid(), new Uri("Settings\\SettingsGarageCarEditorPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 29, 18);
			StackLayout stackLayout;
			VisualDiagnostics.RegisterSourceInfo(stackLayout = new StackLayout(), new Uri("Settings\\SettingsGarageCarEditorPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 19, 14);
			ScrollView scrollView;
			VisualDiagnostics.RegisterSourceInfo(scrollView = new ScrollView(), new Uri("Settings\\SettingsGarageCarEditorPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 15, 10);
			VisualDiagnostics.RegisterSourceInfo(this, new Uri("Settings\\SettingsGarageCarEditorPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 2, 2);
			NameScope nameScope = NameScope.GetNameScope(this) ?? new NameScope();
			NameScope.SetNameScope(this, nameScope);
			nameScope.RegisterName("panelStep1", scrollView);
			if (scrollView.StyleId == null)
			{
				scrollView.StyleId = "panelStep1";
			}
			nameScope.RegisterName("activityFrame", activityFrame);
			if (activityFrame.StyleId == null)
			{
				activityFrame.StyleId = "activityFrame";
			}
			nameScope.RegisterName("entry_CarName", entry);
			if (entry.StyleId == null)
			{
				entry.StyleId = "entry_CarName";
			}
			nameScope.RegisterName("pickerYear", picker);
			if (picker.StyleId == null)
			{
				picker.StyleId = "pickerYear";
			}
			nameScope.RegisterName("gridOKCancel", grid);
			if (grid.StyleId == null)
			{
				grid.StyleId = "gridOKCancel";
			}
			nameScope.RegisterName("btnOK", button);
			if (button.StyleId == null)
			{
				button.StyleId = "btnOK";
			}
			nameScope.RegisterName("btnCancel", button2);
			if (button2.StyleId == null)
			{
				button2.StyleId = "btnCancel";
			}
			this.panelStep1 = scrollView;
			this.activityFrame = activityFrame;
			this.entry_CarName = entry;
			this.pickerYear = picker;
			this.gridOKCancel = grid;
			this.btnOK = button;
			this.btnCancel = button2;
			translate.Text = "ios_GarageTitle";
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
			xmlNamespaceResolver.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver.Add("local", "clr-namespace:CarScannerXamarinForms");
			xamlServiceProvider.Add(typeFromHandle2, new XamlTypeResolver(xmlNamespaceResolver, typeof(SettingsGarageCarEditorPage).GetTypeInfo().Assembly));
			xamlServiceProvider.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(8, 5)));
			object obj2 = markupExtension.ProvideValue(xamlServiceProvider);
			this.Title = obj2;
			this.SetValue(Page.PaddingProperty, new Thickness(0.0));
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
			xmlNamespaceResolver2.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver2.Add("local", "clr-namespace:CarScannerXamarinForms");
			xamlServiceProvider2.Add(typeFromHandle4, new XamlTypeResolver(xmlNamespaceResolver2, typeof(SettingsGarageCarEditorPage).GetTypeInfo().Assembly));
			xamlServiceProvider2.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(11, 5)));
			DynamicResource dynamicResource = markupExtension2.ProvideValue(xamlServiceProvider2);
			this.SetDynamicResource(VisualElement.BackgroundColorProperty, dynamicResource.Key);
			this.SizeChanged += this.Handle_SizeChanged;
			scrollView.SetValue(View.MarginProperty, new Thickness(5.0, 0.0, 5.0, 0.0));
			scrollView.SetValue(ScrollView.OrientationProperty, 0);
			stackLayout.SetValue(StackLayout.OrientationProperty, 0);
			activityFrame.SetValue(Grid.RowProperty, 1);
			activityFrame.SetValue(VisualElement.IsVisibleProperty, new VisualElement.VisibilityConverter().ConvertFromInvariantString("false"));
			activityFrame.SetValue(View.VerticalOptionsProperty, LayoutOptions.Start);
			stackLayout.Children.Add(activityFrame);
			translate2.Text = "ios_CarName";
			IMarkupExtension markupExtension3 = translate2;
			XamlServiceProvider xamlServiceProvider3 = new XamlServiceProvider();
			Type typeFromHandle5 = typeof(IProvideValueTarget);
			object[] array3 = new object[0 + 4];
			array3[0] = label;
			array3[1] = stackLayout;
			array3[2] = scrollView;
			array3[3] = this;
			object obj4;
			xamlServiceProvider3.Add(typeFromHandle5, obj4 = new SimpleValueTargetProvider(array3, Label.TextProperty, nameScope));
			xamlServiceProvider3.Add(typeof(IReferenceProvider), obj4);
			Type typeFromHandle6 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver3 = new XmlNamespaceResolver();
			xmlNamespaceResolver3.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver3.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver3.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver3.Add("local", "clr-namespace:CarScannerXamarinForms");
			xamlServiceProvider3.Add(typeFromHandle6, new XamlTypeResolver(xmlNamespaceResolver3, typeof(SettingsGarageCarEditorPage).GetTypeInfo().Assembly));
			xamlServiceProvider3.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(25, 24)));
			object obj5 = markupExtension3.ProvideValue(xamlServiceProvider3);
			label.Text = obj5;
			stackLayout.Children.Add(label);
			entry.SetValue(Entry.TextProperty, "");
			stackLayout.Children.Add(entry);
			translate3.Text = "ios_CarYear";
			IMarkupExtension markupExtension4 = translate3;
			XamlServiceProvider xamlServiceProvider4 = new XamlServiceProvider();
			Type typeFromHandle7 = typeof(IProvideValueTarget);
			object[] array4 = new object[0 + 4];
			array4[0] = label2;
			array4[1] = stackLayout;
			array4[2] = scrollView;
			array4[3] = this;
			object obj6;
			xamlServiceProvider4.Add(typeFromHandle7, obj6 = new SimpleValueTargetProvider(array4, Label.TextProperty, nameScope));
			xamlServiceProvider4.Add(typeof(IReferenceProvider), obj6);
			Type typeFromHandle8 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver4 = new XmlNamespaceResolver();
			xmlNamespaceResolver4.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver4.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver4.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver4.Add("local", "clr-namespace:CarScannerXamarinForms");
			xamlServiceProvider4.Add(typeFromHandle8, new XamlTypeResolver(xmlNamespaceResolver4, typeof(SettingsGarageCarEditorPage).GetTypeInfo().Assembly));
			xamlServiceProvider4.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(27, 24)));
			object obj7 = markupExtension4.ProvideValue(xamlServiceProvider4);
			label2.Text = obj7;
			stackLayout.Children.Add(label2);
			stackLayout.Children.Add(picker);
			columnDefinition.SetValue(ColumnDefinition.WidthProperty, new GridLengthTypeConverter().ConvertFromInvariantString("*"));
			grid.GetValue(Grid.ColumnDefinitionsProperty).Add(columnDefinition);
			columnDefinition2.SetValue(ColumnDefinition.WidthProperty, new GridLengthTypeConverter().ConvertFromInvariantString("*"));
			grid.GetValue(Grid.ColumnDefinitionsProperty).Add(columnDefinition2);
			button.SetValue(Grid.ColumnProperty, 0);
			button.SetValue(VisualElement.BackgroundColorProperty, Color.DarkGreen);
			button.Clicked += this.btnOK_Clicked;
			button.SetValue(Button.TextProperty, "OK");
			button.SetValue(Button.TextColorProperty, Color.White);
			grid.Children.Add(button);
			button2.SetValue(Grid.ColumnProperty, 1);
			button2.SetValue(VisualElement.BackgroundColorProperty, Color.Red);
			button2.Clicked += this.btnCancel_Clicked;
			translate4.Text = "ios_Cancel";
			IMarkupExtension markupExtension5 = translate4;
			XamlServiceProvider xamlServiceProvider5 = new XamlServiceProvider();
			Type typeFromHandle9 = typeof(IProvideValueTarget);
			object[] array5 = new object[0 + 5];
			array5[0] = button2;
			array5[1] = grid;
			array5[2] = stackLayout;
			array5[3] = scrollView;
			array5[4] = this;
			object obj8;
			xamlServiceProvider5.Add(typeFromHandle9, obj8 = new SimpleValueTargetProvider(array5, Button.TextProperty, nameScope));
			xamlServiceProvider5.Add(typeof(IReferenceProvider), obj8);
			Type typeFromHandle10 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver5 = new XmlNamespaceResolver();
			xmlNamespaceResolver5.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver5.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver5.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver5.Add("local", "clr-namespace:CarScannerXamarinForms");
			xamlServiceProvider5.Add(typeFromHandle10, new XamlTypeResolver(xmlNamespaceResolver5, typeof(SettingsGarageCarEditorPage).GetTypeInfo().Assembly));
			xamlServiceProvider5.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(46, 25)));
			object obj9 = markupExtension5.ProvideValue(xamlServiceProvider5);
			button2.Text = obj9;
			button2.SetValue(Button.TextColorProperty, Color.White);
			grid.Children.Add(button2);
			stackLayout.Children.Add(grid);
			scrollView.Content = stackLayout;
			this.SetValue(ContentPage.ContentProperty, scrollView);
		}

		// Token: 0x06001B4C RID: 6988 RVA: 0x0013245A File Offset: 0x0013065A
		[CompilerGenerated]
		private bool <IsNameUnique>b__7_0(MyCar x)
		{
			return x != this.car;
		}

		// Token: 0x06001B4D RID: 6989 RVA: 0x00132468 File Offset: 0x00130668
		[CompilerGenerated]
		private bool <IsNameUnique>b__7_1(MyCar x)
		{
			return x.Name.Equals(this.entry_CarName.Text, StringComparison.InvariantCultureIgnoreCase);
		}

		// Token: 0x06001B4E RID: 6990 RVA: 0x00132484 File Offset: 0x00130684
		private void __InitComponentRuntime()
		{
			Extensions.LoadFromXaml<SettingsGarageCarEditorPage>(this, typeof(SettingsGarageCarEditorPage));
			this.panelStep1 = NameScopeExtensions.FindByName<ScrollView>(this, "panelStep1");
			this.activityFrame = NameScopeExtensions.FindByName<ActivityFrame>(this, "activityFrame");
			this.entry_CarName = NameScopeExtensions.FindByName<Entry>(this, "entry_CarName");
			this.pickerYear = NameScopeExtensions.FindByName<Picker>(this, "pickerYear");
			this.gridOKCancel = NameScopeExtensions.FindByName<Grid>(this, "gridOKCancel");
			this.btnOK = NameScopeExtensions.FindByName<Button>(this, "btnOK");
			this.btnCancel = NameScopeExtensions.FindByName<Button>(this, "btnCancel");
		}

		// Token: 0x04000CA8 RID: 3240
		public MyCar car;

		// Token: 0x04000CA9 RID: 3241
		private GarageModel model;

		// Token: 0x04000CAA RID: 3242
		private bool createNew;

		// Token: 0x04000CAB RID: 3243
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private ScrollView panelStep1;

		// Token: 0x04000CAC RID: 3244
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private ActivityFrame activityFrame;

		// Token: 0x04000CAD RID: 3245
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private Entry entry_CarName;

		// Token: 0x04000CAE RID: 3246
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private Picker pickerYear;

		// Token: 0x04000CAF RID: 3247
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private Grid gridOKCancel;

		// Token: 0x04000CB0 RID: 3248
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private Button btnOK;

		// Token: 0x04000CB1 RID: 3249
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private Button btnCancel;

		// Token: 0x02000241 RID: 577
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <btnOK_Clicked>d__5 : IAsyncStateMachine
		{
			// Token: 0x06001B4F RID: 6991 RVA: 0x0013251C File Offset: 0x0013071C
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				SettingsGarageCarEditorPage settingsGarageCarEditorPage = this;
				try
				{
					TaskAwaiter<Page> taskAwaiter;
					TaskAwaiter<Page> taskAwaiter2;
					if (num != 0)
					{
						TaskAwaiter taskAwaiter3;
						if (num != 1)
						{
							if (settingsGarageCarEditorPage.IsNameUnique())
							{
								settingsGarageCarEditorPage.car.Name = settingsGarageCarEditorPage.entry_CarName.Text;
								settingsGarageCarEditorPage.car.Year = (int)settingsGarageCarEditorPage.pickerYear.SelectedItem;
								if (settingsGarageCarEditorPage.createNew)
								{
									settingsGarageCarEditorPage.model.Cars.Add(settingsGarageCarEditorPage.car);
								}
								settingsGarageCarEditorPage.activityFrame.IsVisible = true;
								settingsGarageCarEditorPage.btnOK.IsEnabled = false;
								settingsGarageCarEditorPage.btnCancel.IsEnabled = false;
								settingsGarageCarEditorPage.model.SaveToFile();
								if (SharedSettings.Current.CurrentCarId == settingsGarageCarEditorPage.car.Id)
								{
									SharedSettings.Current.CurrentCarName = settingsGarageCarEditorPage.car.Name;
								}
								settingsGarageCarEditorPage.activityFrame.IsVisible = false;
								taskAwaiter = settingsGarageCarEditorPage.Navigation.PopAsync().GetAwaiter();
								if (!taskAwaiter.IsCompleted)
								{
									num2 = 0;
									taskAwaiter2 = taskAwaiter;
									this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<Page>, SettingsGarageCarEditorPage.<btnOK_Clicked>d__5>(ref taskAwaiter, ref this);
									return;
								}
								goto IL_0136;
							}
							else
							{
								taskAwaiter3 = settingsGarageCarEditorPage.DisplayAlert(Translate.GetString("ios_WrongCarName"), Translate.GetString("ios_CarWithSameNameExists"), "OK").GetAwaiter();
								if (!taskAwaiter3.IsCompleted)
								{
									num2 = 1;
									TaskAwaiter taskAwaiter4 = taskAwaiter3;
									this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, SettingsGarageCarEditorPage.<btnOK_Clicked>d__5>(ref taskAwaiter3, ref this);
									return;
								}
							}
						}
						else
						{
							TaskAwaiter taskAwaiter4;
							taskAwaiter3 = taskAwaiter4;
							taskAwaiter4 = default(TaskAwaiter);
							num2 = -1;
						}
						taskAwaiter3.GetResult();
						goto IL_01B1;
					}
					taskAwaiter = taskAwaiter2;
					taskAwaiter2 = default(TaskAwaiter<Page>);
					num2 = -1;
					IL_0136:
					taskAwaiter.GetResult();
					IL_01B1:;
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

			// Token: 0x06001B50 RID: 6992 RVA: 0x00132724 File Offset: 0x00130924
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x04000CB2 RID: 3250
			public int <>1__state;

			// Token: 0x04000CB3 RID: 3251
			public AsyncVoidMethodBuilder <>t__builder;

			// Token: 0x04000CB4 RID: 3252
			public SettingsGarageCarEditorPage <>4__this;

			// Token: 0x04000CB5 RID: 3253
			private TaskAwaiter<Page> <>u__1;

			// Token: 0x04000CB6 RID: 3254
			private TaskAwaiter <>u__2;
		}
	}
}
