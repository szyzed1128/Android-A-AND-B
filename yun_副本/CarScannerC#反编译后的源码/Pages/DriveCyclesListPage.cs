using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using CarScannerXamarinForms.DriveCycles;
using CarScannerXamarinForms.Settings;
using CarScannerXamarinForms.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Internals;
using Xamarin.Forms.PlatformConfiguration.iOSSpecific;
using Xamarin.Forms.Xaml;
using Xamarin.Forms.Xaml.Diagnostics;
using Xamarin.Forms.Xaml.Internals;

namespace CarScannerXamarinForms.Pages
{
	// Token: 0x020005FF RID: 1535
	[XamlCompilation(2)]
	[XamlFilePath("DriveCycles\\DriveCyclesListPage.xaml")]
	public class DriveCyclesListPage : ContentPage
	{
		// Token: 0x0600365C RID: 13916 RVA: 0x002748B0 File Offset: 0x00272AB0
		public DriveCyclesListPage(DriveCycleViewModel model)
		{
			this.InitializeComponent();
			if (SharedSettings.Current.AdsProductPurchased)
			{
				this.ad.IsVisible = false;
			}
			else
			{
				this.ad.IsVisible = true;
			}
			this.model = model;
			List<CombinedDriveCycle> combinedDriveCycles = model.GetCombinedDriveCycles();
			combinedDriveCycles.Reverse();
			this.lv.ItemsSource = combinedDriveCycles;
		}

		// Token: 0x0600365D RID: 13917 RVA: 0x000027D4 File Offset: 0x000009D4
		private void Page_Appearing(object sender, EventArgs e)
		{
		}

		// Token: 0x0600365E RID: 13918 RVA: 0x000027D4 File Offset: 0x000009D4
		private void Page_Disappearing(object sender, EventArgs e)
		{
		}

		// Token: 0x0600365F RID: 13919 RVA: 0x000027D4 File Offset: 0x000009D4
		private void Handle_SizeChanged(object sender, EventArgs e)
		{
		}

		// Token: 0x06003660 RID: 13920 RVA: 0x00274910 File Offset: 0x00272B10
		private async void btnDelete_Clicked(object sender, EventArgs e)
		{
			this.activityFrame.IsVisible = true;
			this.lv.IsEnabled = false;
			List<CombinedDriveCycle> comboCycles = new List<CombinedDriveCycle>();
			await Task.Run(delegate
			{
				CombinedDriveCycle combinedDriveCycle = (sender as MenuItem).BindingContext as CombinedDriveCycle;
				this.model.DeleteDriveCycles(combinedDriveCycle.Cycles);
				comboCycles = this.model.GetCombinedDriveCycles();
				comboCycles.Reverse();
			});
			this.model.PeriodAllTime.Execute(null);
			this.lv.ItemsSource = comboCycles;
			this.lv.IsEnabled = true;
			this.activityFrame.IsVisible = false;
		}

		// Token: 0x06003661 RID: 13921 RVA: 0x0027494F File Offset: 0x00272B4F
		private void lv_ItemTapped(object sender, ItemTappedEventArgs e)
		{
			this.lv.SelectedItem = null;
		}

		// Token: 0x06003662 RID: 13922 RVA: 0x00274960 File Offset: 0x00272B60
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private void InitializeComponent()
		{
			if (ResourceLoader.IsEnabled && ResourceLoader.CanProvideContentFor(new ResourceLoader.ResourceLoadingQuery
			{
				AssemblyName = typeof(DriveCyclesListPage).GetTypeInfo().Assembly.GetName(),
				ResourcePath = "DriveCycles/DriveCyclesListPage.xaml",
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
			VisualDiagnostics.RegisterSourceInfo(translate = new Translate(), new Uri("DriveCycles\\DriveCyclesListPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 8, 5);
			DynamicResourceExtension dynamicResourceExtension;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension = new DynamicResourceExtension(), new Uri("DriveCycles\\DriveCyclesListPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 11, 5);
			BoolToNegativeConverter boolToNegativeConverter;
			VisualDiagnostics.RegisterSourceInfo(boolToNegativeConverter = new BoolToNegativeConverter(), new Uri("DriveCycles\\DriveCyclesListPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 18, 14);
			UnitsToStringConverter unitsToStringConverter;
			VisualDiagnostics.RegisterSourceInfo(unitsToStringConverter = new UnitsToStringConverter(), new Uri("DriveCycles\\DriveCyclesListPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 19, 14);
			ResourceDictionary resourceDictionary;
			VisualDiagnostics.RegisterSourceInfo(resourceDictionary = new ResourceDictionary(), new Uri("DriveCycles\\DriveCyclesListPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 17, 10);
			RowDefinition rowDefinition;
			VisualDiagnostics.RegisterSourceInfo(rowDefinition = new RowDefinition(), new Uri("DriveCycles\\DriveCyclesListPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 25, 18);
			RowDefinition rowDefinition2;
			VisualDiagnostics.RegisterSourceInfo(rowDefinition2 = new RowDefinition(), new Uri("DriveCycles\\DriveCyclesListPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 26, 18);
			BindingExtension bindingExtension;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension = new BindingExtension(), new Uri("DriveCycles\\DriveCyclesListPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 36, 17);
			DataTemplate dataTemplate;
			VisualDiagnostics.RegisterSourceInfo(dataTemplate = new DataTemplate(), new Uri("DriveCycles\\DriveCyclesListPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 38, 22);
			ListView listView;
			VisualDiagnostics.RegisterSourceInfo(listView = new ListView(), new Uri("DriveCycles\\DriveCyclesListPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 29, 14);
			ActivityFrame activityFrame;
			VisualDiagnostics.RegisterSourceInfo(activityFrame = new ActivityFrame(), new Uri("DriveCycles\\DriveCyclesListPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 178, 14);
			ComplexAdView complexAdView;
			VisualDiagnostics.RegisterSourceInfo(complexAdView = new ComplexAdView(), new Uri("DriveCycles\\DriveCyclesListPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 184, 14);
			Grid grid;
			VisualDiagnostics.RegisterSourceInfo(grid = new Grid(), new Uri("DriveCycles\\DriveCyclesListPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 23, 10);
			VisualDiagnostics.RegisterSourceInfo(this, new Uri("DriveCycles\\DriveCyclesListPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 2, 2);
			NameScope nameScope = NameScope.GetNameScope(this) ?? new NameScope();
			NameScope.SetNameScope(this, nameScope);
			nameScope.RegisterName("lv", listView);
			if (listView.StyleId == null)
			{
				listView.StyleId = "lv";
			}
			nameScope.RegisterName("activityFrame", activityFrame);
			if (activityFrame.StyleId == null)
			{
				activityFrame.StyleId = "activityFrame";
			}
			nameScope.RegisterName("ad", complexAdView);
			if (complexAdView.StyleId == null)
			{
				complexAdView.StyleId = "ad";
			}
			this.lv = listView;
			this.activityFrame = activityFrame;
			this.ad = complexAdView;
			this.Resources = resourceDictionary;
			resourceDictionary.Add("BoolToNegativeConverter", boolToNegativeConverter);
			resourceDictionary.Add("UnitsToStringConverter", unitsToStringConverter);
			translate.Text = "ios_FuelStatisticsPage.Text";
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
			xamlServiceProvider.Add(typeFromHandle2, new XamlTypeResolver(xmlNamespaceResolver, typeof(DriveCyclesListPage).GetTypeInfo().Assembly));
			xamlServiceProvider.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(8, 5)));
			object obj2 = markupExtension.ProvideValue(xamlServiceProvider);
			this.Title = obj2;
			this.SetValue(Page.PrefersStatusBarHiddenProperty, 2);
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
			xamlServiceProvider2.Add(typeFromHandle4, new XamlTypeResolver(xmlNamespaceResolver2, typeof(DriveCyclesListPage).GetTypeInfo().Assembly));
			xamlServiceProvider2.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(11, 5)));
			DynamicResource dynamicResource = markupExtension2.ProvideValue(xamlServiceProvider2);
			this.SetDynamicResource(VisualElement.BackgroundColorProperty, dynamicResource.Key);
			this.Disappearing += this.Page_Disappearing;
			this.SetValue(NavigationPage.HasBackButtonProperty, true);
			this.SetValue(NavigationPage.HasNavigationBarProperty, true);
			this.SizeChanged += this.Handle_SizeChanged;
			this.Resources = resourceDictionary;
			rowDefinition.SetValue(RowDefinition.HeightProperty, new GridLengthTypeConverter().ConvertFromInvariantString("*"));
			grid.GetValue(Grid.RowDefinitionsProperty).Add(rowDefinition);
			rowDefinition2.SetValue(RowDefinition.HeightProperty, new GridLengthTypeConverter().ConvertFromInvariantString("auto"));
			grid.GetValue(Grid.RowDefinitionsProperty).Add(rowDefinition2);
			listView.SetValue(Grid.RowProperty, 0);
			listView.SetValue(View.MarginProperty, new Thickness(5.0, 5.0));
			listView.SetValue(VisualElement.BackgroundColorProperty, Color.Transparent);
			listView.SetValue(ListView.HasUnevenRowsProperty, true);
			listView.ItemTapped += this.lv_ItemTapped;
			bindingExtension.Path = "RecorderedCycles";
			BindingBase bindingBase = bindingExtension.ProvideValue(null);
			listView.SetBinding(ItemsView<Cell>.ItemsSourceProperty, bindingBase);
			IDataTemplate dataTemplate2 = dataTemplate;
			DriveCyclesListPage.<InitializeComponent>_anonXamlCDataTemplate_25 <InitializeComponent>_anonXamlCDataTemplate_ = new DriveCyclesListPage.<InitializeComponent>_anonXamlCDataTemplate_25();
			object[] array3 = new object[0 + 4];
			array3[0] = dataTemplate;
			array3[1] = listView;
			array3[2] = grid;
			array3[3] = this;
			<InitializeComponent>_anonXamlCDataTemplate_.parentValues = array3;
			<InitializeComponent>_anonXamlCDataTemplate_.root = this;
			dataTemplate2.LoadTemplate = new Func<object>(<InitializeComponent>_anonXamlCDataTemplate_.LoadDataTemplate);
			listView.SetValue(ItemsView<Cell>.ItemTemplateProperty, dataTemplate);
			grid.Children.Add(listView);
			activityFrame.SetValue(Grid.RowProperty, 0);
			activityFrame.SetValue(VisualElement.InputTransparentProperty, true);
			activityFrame.SetValue(VisualElement.IsVisibleProperty, new VisualElement.VisibilityConverter().ConvertFromInvariantString("false"));
			activityFrame.SetValue(View.VerticalOptionsProperty, LayoutOptions.Start);
			grid.Children.Add(activityFrame);
			complexAdView.SetValue(Grid.RowProperty, 1);
			complexAdView.SetValue(VisualElement.IsVisibleProperty, new VisualElement.VisibilityConverter().ConvertFromInvariantString("true"));
			complexAdView.SetValue(View.VerticalOptionsProperty, LayoutOptions.End);
			grid.Children.Add(complexAdView);
			this.SetValue(ContentPage.ContentProperty, grid);
		}

		// Token: 0x06003663 RID: 13923 RVA: 0x0027515C File Offset: 0x0027335C
		private void __InitComponentRuntime()
		{
			Extensions.LoadFromXaml<DriveCyclesListPage>(this, typeof(DriveCyclesListPage));
			this.lv = NameScopeExtensions.FindByName<ListView>(this, "lv");
			this.activityFrame = NameScopeExtensions.FindByName<ActivityFrame>(this, "activityFrame");
			this.ad = NameScopeExtensions.FindByName<ComplexAdView>(this, "ad");
		}

		// Token: 0x0400208B RID: 8331
		private DriveCycleViewModel model;

		// Token: 0x0400208C RID: 8332
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private ListView lv;

		// Token: 0x0400208D RID: 8333
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private ActivityFrame activityFrame;

		// Token: 0x0400208E RID: 8334
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private ComplexAdView ad;

		// Token: 0x02000600 RID: 1536
		[CompilerGenerated]
		private sealed class <>c__DisplayClass5_0
		{
			// Token: 0x06003664 RID: 13924 RVA: 0x00002050 File Offset: 0x00000250
			public <>c__DisplayClass5_0()
			{
			}

			// Token: 0x06003665 RID: 13925 RVA: 0x002751B0 File Offset: 0x002733B0
			internal void <btnDelete_Clicked>b__0()
			{
				CombinedDriveCycle combinedDriveCycle = (this.sender as MenuItem).BindingContext as CombinedDriveCycle;
				this.<>4__this.model.DeleteDriveCycles(combinedDriveCycle.Cycles);
				this.comboCycles = this.<>4__this.model.GetCombinedDriveCycles();
				this.comboCycles.Reverse();
			}

			// Token: 0x0400208F RID: 8335
			public object sender;

			// Token: 0x04002090 RID: 8336
			public DriveCyclesListPage <>4__this;

			// Token: 0x04002091 RID: 8337
			public List<CombinedDriveCycle> comboCycles;
		}

		// Token: 0x02000601 RID: 1537
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <btnDelete_Clicked>d__5 : IAsyncStateMachine
		{
			// Token: 0x06003666 RID: 13926 RVA: 0x0027520C File Offset: 0x0027340C
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				DriveCyclesListPage driveCyclesListPage = this;
				try
				{
					TaskAwaiter taskAwaiter;
					if (num != 0)
					{
						CS$<>8__locals1 = new DriveCyclesListPage.<>c__DisplayClass5_0();
						CS$<>8__locals1.sender = sender;
						CS$<>8__locals1.<>4__this = this;
						driveCyclesListPage.activityFrame.IsVisible = true;
						driveCyclesListPage.lv.IsEnabled = false;
						CS$<>8__locals1.comboCycles = new List<CombinedDriveCycle>();
						taskAwaiter = Task.Run(delegate
						{
							CombinedDriveCycle combinedDriveCycle = (CS$<>8__locals1.sender as MenuItem).BindingContext as CombinedDriveCycle;
							CS$<>8__locals1.<>4__this.model.DeleteDriveCycles(combinedDriveCycle.Cycles);
							CS$<>8__locals1.comboCycles = CS$<>8__locals1.<>4__this.model.GetCombinedDriveCycles();
							CS$<>8__locals1.comboCycles.Reverse();
						}).GetAwaiter();
						if (!taskAwaiter.IsCompleted)
						{
							num2 = 0;
							TaskAwaiter taskAwaiter2 = taskAwaiter;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, DriveCyclesListPage.<btnDelete_Clicked>d__5>(ref taskAwaiter, ref this);
							return;
						}
					}
					else
					{
						TaskAwaiter taskAwaiter2;
						taskAwaiter = taskAwaiter2;
						taskAwaiter2 = default(TaskAwaiter);
						num2 = -1;
					}
					taskAwaiter.GetResult();
					driveCyclesListPage.model.PeriodAllTime.Execute(null);
					driveCyclesListPage.lv.ItemsSource = CS$<>8__locals1.comboCycles;
					driveCyclesListPage.lv.IsEnabled = true;
					driveCyclesListPage.activityFrame.IsVisible = false;
				}
				catch (Exception ex)
				{
					num2 = -2;
					CS$<>8__locals1 = null;
					this.<>t__builder.SetException(ex);
					return;
				}
				num2 = -2;
				CS$<>8__locals1 = null;
				this.<>t__builder.SetResult();
			}

			// Token: 0x06003667 RID: 13927 RVA: 0x00275384 File Offset: 0x00273584
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x04002092 RID: 8338
			public int <>1__state;

			// Token: 0x04002093 RID: 8339
			public AsyncVoidMethodBuilder <>t__builder;

			// Token: 0x04002094 RID: 8340
			public object sender;

			// Token: 0x04002095 RID: 8341
			public DriveCyclesListPage <>4__this;

			// Token: 0x04002096 RID: 8342
			private DriveCyclesListPage.<>c__DisplayClass5_0 <>8__1;

			// Token: 0x04002097 RID: 8343
			private TaskAwaiter <>u__1;
		}

		// Token: 0x02000602 RID: 1538
		[CompilerGenerated]
		private sealed class <InitializeComponent>_anonXamlCDataTemplate_25
		{
			// Token: 0x06003668 RID: 13928 RVA: 0x00275394 File Offset: 0x00273594
			public <InitializeComponent>_anonXamlCDataTemplate_25()
			{
			}

			// Token: 0x06003669 RID: 13929 RVA: 0x002753A8 File Offset: 0x002735A8
			internal object LoadDataTemplate()
			{
				BindingExtension bindingExtension;
				VisualDiagnostics.RegisterSourceInfo(bindingExtension = new BindingExtension(), new Uri("DriveCycles\\DriveCyclesListPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 170, 37);
				Translate translate;
				VisualDiagnostics.RegisterSourceInfo(translate = new Translate(), new Uri("DriveCycles\\DriveCyclesListPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 172, 37);
				MenuItem menuItem;
				VisualDiagnostics.RegisterSourceInfo(menuItem = new MenuItem(), new Uri("DriveCycles\\DriveCyclesListPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 168, 34);
				ColumnDefinition columnDefinition;
				VisualDiagnostics.RegisterSourceInfo(columnDefinition = new ColumnDefinition(), new Uri("DriveCycles\\DriveCyclesListPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 43, 42);
				ColumnDefinition columnDefinition2;
				VisualDiagnostics.RegisterSourceInfo(columnDefinition2 = new ColumnDefinition(), new Uri("DriveCycles\\DriveCyclesListPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 44, 42);
				ColumnDefinition columnDefinition3;
				VisualDiagnostics.RegisterSourceInfo(columnDefinition3 = new ColumnDefinition(), new Uri("DriveCycles\\DriveCyclesListPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 45, 42);
				DynamicResourceExtension dynamicResourceExtension;
				VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension = new DynamicResourceExtension(), new Uri("DriveCycles\\DriveCyclesListPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 50, 41);
				BindingExtension bindingExtension2;
				VisualDiagnostics.RegisterSourceInfo(bindingExtension2 = new BindingExtension(), new Uri("DriveCycles\\DriveCyclesListPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 53, 55);
				Span span;
				VisualDiagnostics.RegisterSourceInfo(span = new Span(), new Uri("DriveCycles\\DriveCyclesListPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 53, 50);
				Span span2;
				VisualDiagnostics.RegisterSourceInfo(span2 = new Span(), new Uri("DriveCycles\\DriveCyclesListPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 54, 50);
				BindingExtension bindingExtension3;
				VisualDiagnostics.RegisterSourceInfo(bindingExtension3 = new BindingExtension(), new Uri("DriveCycles\\DriveCyclesListPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 55, 55);
				Span span3;
				VisualDiagnostics.RegisterSourceInfo(span3 = new Span(), new Uri("DriveCycles\\DriveCyclesListPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 55, 50);
				Span span4;
				VisualDiagnostics.RegisterSourceInfo(span4 = new Span(), new Uri("DriveCycles\\DriveCyclesListPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 56, 50);
				BindingExtension bindingExtension4;
				VisualDiagnostics.RegisterSourceInfo(bindingExtension4 = new BindingExtension(), new Uri("DriveCycles\\DriveCyclesListPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 57, 55);
				Span span5;
				VisualDiagnostics.RegisterSourceInfo(span5 = new Span(), new Uri("DriveCycles\\DriveCyclesListPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 57, 50);
				Span span6;
				VisualDiagnostics.RegisterSourceInfo(span6 = new Span(), new Uri("DriveCycles\\DriveCyclesListPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 58, 50);
				FormattedString formattedString;
				VisualDiagnostics.RegisterSourceInfo(formattedString = new FormattedString(), new Uri("DriveCycles\\DriveCyclesListPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 52, 46);
				Label label;
				VisualDiagnostics.RegisterSourceInfo(label = new Label(), new Uri("DriveCycles\\DriveCyclesListPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 47, 38);
				Grid grid;
				VisualDiagnostics.RegisterSourceInfo(grid = new Grid(), new Uri("DriveCycles\\DriveCyclesListPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 41, 34);
				ColumnDefinition columnDefinition4;
				VisualDiagnostics.RegisterSourceInfo(columnDefinition4 = new ColumnDefinition(), new Uri("DriveCycles\\DriveCyclesListPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 79, 42);
				ColumnDefinition columnDefinition5;
				VisualDiagnostics.RegisterSourceInfo(columnDefinition5 = new ColumnDefinition(), new Uri("DriveCycles\\DriveCyclesListPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 80, 42);
				RowDefinition rowDefinition;
				VisualDiagnostics.RegisterSourceInfo(rowDefinition = new RowDefinition(), new Uri("DriveCycles\\DriveCyclesListPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 83, 42);
				RowDefinition rowDefinition2;
				VisualDiagnostics.RegisterSourceInfo(rowDefinition2 = new RowDefinition(), new Uri("DriveCycles\\DriveCyclesListPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 84, 42);
				RowDefinition rowDefinition3;
				VisualDiagnostics.RegisterSourceInfo(rowDefinition3 = new RowDefinition(), new Uri("DriveCycles\\DriveCyclesListPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 85, 42);
				RowDefinition rowDefinition4;
				VisualDiagnostics.RegisterSourceInfo(rowDefinition4 = new RowDefinition(), new Uri("DriveCycles\\DriveCyclesListPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 86, 42);
				RowDefinition rowDefinition5;
				VisualDiagnostics.RegisterSourceInfo(rowDefinition5 = new RowDefinition(), new Uri("DriveCycles\\DriveCyclesListPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 87, 42);
				RowDefinition rowDefinition6;
				VisualDiagnostics.RegisterSourceInfo(rowDefinition6 = new RowDefinition(), new Uri("DriveCycles\\DriveCyclesListPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 88, 42);
				DynamicResourceExtension dynamicResourceExtension2;
				VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension2 = new DynamicResourceExtension(), new Uri("DriveCycles\\DriveCyclesListPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 97, 41);
				Translate translate2;
				VisualDiagnostics.RegisterSourceInfo(translate2 = new Translate(), new Uri("DriveCycles\\DriveCyclesListPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 98, 41);
				Label label2;
				VisualDiagnostics.RegisterSourceInfo(label2 = new Label(), new Uri("DriveCycles\\DriveCyclesListPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 94, 38);
				DynamicResourceExtension dynamicResourceExtension3;
				VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension3 = new DynamicResourceExtension(), new Uri("DriveCycles\\DriveCyclesListPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 104, 48);
				BindingExtension bindingExtension5;
				VisualDiagnostics.RegisterSourceInfo(bindingExtension5 = new BindingExtension(), new Uri("DriveCycles\\DriveCyclesListPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 104, 90);
				Label label3;
				VisualDiagnostics.RegisterSourceInfo(label3 = new Label(), new Uri("DriveCycles\\DriveCyclesListPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 104, 42);
				DynamicResourceExtension dynamicResourceExtension4;
				VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension4 = new DynamicResourceExtension(), new Uri("DriveCycles\\DriveCyclesListPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 105, 48);
				BindingExtension bindingExtension6;
				VisualDiagnostics.RegisterSourceInfo(bindingExtension6 = new BindingExtension(), new Uri("DriveCycles\\DriveCyclesListPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 105, 90);
				Label label4;
				VisualDiagnostics.RegisterSourceInfo(label4 = new Label(), new Uri("DriveCycles\\DriveCyclesListPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 105, 42);
				StackLayout stackLayout;
				VisualDiagnostics.RegisterSourceInfo(stackLayout = new StackLayout(), new Uri("DriveCycles\\DriveCyclesListPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 99, 38);
				DynamicResourceExtension dynamicResourceExtension5;
				VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension5 = new DynamicResourceExtension(), new Uri("DriveCycles\\DriveCyclesListPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 111, 41);
				Translate translate3;
				VisualDiagnostics.RegisterSourceInfo(translate3 = new Translate(), new Uri("DriveCycles\\DriveCyclesListPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 112, 41);
				Label label5;
				VisualDiagnostics.RegisterSourceInfo(label5 = new Label(), new Uri("DriveCycles\\DriveCyclesListPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 108, 38);
				DynamicResourceExtension dynamicResourceExtension6;
				VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension6 = new DynamicResourceExtension(), new Uri("DriveCycles\\DriveCyclesListPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 118, 48);
				BindingExtension bindingExtension7;
				VisualDiagnostics.RegisterSourceInfo(bindingExtension7 = new BindingExtension(), new Uri("DriveCycles\\DriveCyclesListPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 118, 90);
				Label label6;
				VisualDiagnostics.RegisterSourceInfo(label6 = new Label(), new Uri("DriveCycles\\DriveCyclesListPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 118, 42);
				DynamicResourceExtension dynamicResourceExtension7;
				VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension7 = new DynamicResourceExtension(), new Uri("DriveCycles\\DriveCyclesListPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 119, 48);
				BindingExtension bindingExtension8;
				VisualDiagnostics.RegisterSourceInfo(bindingExtension8 = new BindingExtension(), new Uri("DriveCycles\\DriveCyclesListPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 119, 90);
				Label label7;
				VisualDiagnostics.RegisterSourceInfo(label7 = new Label(), new Uri("DriveCycles\\DriveCyclesListPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 119, 42);
				StackLayout stackLayout2;
				VisualDiagnostics.RegisterSourceInfo(stackLayout2 = new StackLayout(), new Uri("DriveCycles\\DriveCyclesListPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 113, 38);
				DynamicResourceExtension dynamicResourceExtension8;
				VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension8 = new DynamicResourceExtension(), new Uri("DriveCycles\\DriveCyclesListPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 125, 41);
				Translate translate4;
				VisualDiagnostics.RegisterSourceInfo(translate4 = new Translate(), new Uri("DriveCycles\\DriveCyclesListPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 126, 41);
				Label label8;
				VisualDiagnostics.RegisterSourceInfo(label8 = new Label(), new Uri("DriveCycles\\DriveCyclesListPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 122, 38);
				DynamicResourceExtension dynamicResourceExtension9;
				VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension9 = new DynamicResourceExtension(), new Uri("DriveCycles\\DriveCyclesListPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 132, 48);
				BindingExtension bindingExtension9;
				VisualDiagnostics.RegisterSourceInfo(bindingExtension9 = new BindingExtension(), new Uri("DriveCycles\\DriveCyclesListPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 132, 90);
				Label label9;
				VisualDiagnostics.RegisterSourceInfo(label9 = new Label(), new Uri("DriveCycles\\DriveCyclesListPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 132, 42);
				DynamicResourceExtension dynamicResourceExtension10;
				VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension10 = new DynamicResourceExtension(), new Uri("DriveCycles\\DriveCyclesListPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 133, 48);
				BindingExtension bindingExtension10;
				VisualDiagnostics.RegisterSourceInfo(bindingExtension10 = new BindingExtension(), new Uri("DriveCycles\\DriveCyclesListPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 133, 90);
				Label label10;
				VisualDiagnostics.RegisterSourceInfo(label10 = new Label(), new Uri("DriveCycles\\DriveCyclesListPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 133, 42);
				StackLayout stackLayout3;
				VisualDiagnostics.RegisterSourceInfo(stackLayout3 = new StackLayout(), new Uri("DriveCycles\\DriveCyclesListPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 127, 38);
				DynamicResourceExtension dynamicResourceExtension11;
				VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension11 = new DynamicResourceExtension(), new Uri("DriveCycles\\DriveCyclesListPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 139, 41);
				Translate translate5;
				VisualDiagnostics.RegisterSourceInfo(translate5 = new Translate(), new Uri("DriveCycles\\DriveCyclesListPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 140, 41);
				Label label11;
				VisualDiagnostics.RegisterSourceInfo(label11 = new Label(), new Uri("DriveCycles\\DriveCyclesListPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 136, 38);
				DynamicResourceExtension dynamicResourceExtension12;
				VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension12 = new DynamicResourceExtension(), new Uri("DriveCycles\\DriveCyclesListPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 146, 48);
				BindingExtension bindingExtension11;
				VisualDiagnostics.RegisterSourceInfo(bindingExtension11 = new BindingExtension(), new Uri("DriveCycles\\DriveCyclesListPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 146, 90);
				Label label12;
				VisualDiagnostics.RegisterSourceInfo(label12 = new Label(), new Uri("DriveCycles\\DriveCyclesListPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 146, 42);
				DynamicResourceExtension dynamicResourceExtension13;
				VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension13 = new DynamicResourceExtension(), new Uri("DriveCycles\\DriveCyclesListPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 147, 48);
				BindingExtension bindingExtension12;
				VisualDiagnostics.RegisterSourceInfo(bindingExtension12 = new BindingExtension(), new Uri("DriveCycles\\DriveCyclesListPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 147, 90);
				Label label13;
				VisualDiagnostics.RegisterSourceInfo(label13 = new Label(), new Uri("DriveCycles\\DriveCyclesListPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 147, 42);
				StackLayout stackLayout4;
				VisualDiagnostics.RegisterSourceInfo(stackLayout4 = new StackLayout(), new Uri("DriveCycles\\DriveCyclesListPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 141, 38);
				DynamicResourceExtension dynamicResourceExtension14;
				VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension14 = new DynamicResourceExtension(), new Uri("DriveCycles\\DriveCyclesListPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 153, 41);
				Translate translate6;
				VisualDiagnostics.RegisterSourceInfo(translate6 = new Translate(), new Uri("DriveCycles\\DriveCyclesListPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 154, 41);
				Label label14;
				VisualDiagnostics.RegisterSourceInfo(label14 = new Label(), new Uri("DriveCycles\\DriveCyclesListPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 150, 38);
				DynamicResourceExtension dynamicResourceExtension15;
				VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension15 = new DynamicResourceExtension(), new Uri("DriveCycles\\DriveCyclesListPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 161, 48);
				BindingExtension bindingExtension13;
				VisualDiagnostics.RegisterSourceInfo(bindingExtension13 = new BindingExtension(), new Uri("DriveCycles\\DriveCyclesListPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 161, 90);
				Label label15;
				VisualDiagnostics.RegisterSourceInfo(label15 = new Label(), new Uri("DriveCycles\\DriveCyclesListPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 161, 42);
				DynamicResourceExtension dynamicResourceExtension16;
				VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension16 = new DynamicResourceExtension(), new Uri("DriveCycles\\DriveCyclesListPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 162, 48);
				BindingExtension bindingExtension14;
				VisualDiagnostics.RegisterSourceInfo(bindingExtension14 = new BindingExtension(), new Uri("DriveCycles\\DriveCyclesListPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 162, 90);
				Label label16;
				VisualDiagnostics.RegisterSourceInfo(label16 = new Label(), new Uri("DriveCycles\\DriveCyclesListPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 162, 42);
				StackLayout stackLayout5;
				VisualDiagnostics.RegisterSourceInfo(stackLayout5 = new StackLayout(), new Uri("DriveCycles\\DriveCyclesListPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 155, 38);
				Grid grid2;
				VisualDiagnostics.RegisterSourceInfo(grid2 = new Grid(), new Uri("DriveCycles\\DriveCyclesListPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 77, 34);
				StackLayout stackLayout6;
				VisualDiagnostics.RegisterSourceInfo(stackLayout6 = new StackLayout(), new Uri("DriveCycles\\DriveCyclesListPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 40, 30);
				ViewCell viewCell;
				VisualDiagnostics.RegisterSourceInfo(viewCell = new ViewCell(), new Uri("DriveCycles\\DriveCyclesListPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 39, 26);
				NameScope nameScope = new NameScope();
				NameScope.SetNameScope(viewCell, nameScope);
				menuItem.Clicked += this.root.btnDelete_Clicked;
				bindingExtension.Path = ".";
				BindingBase bindingBase = bindingExtension.ProvideValue(null);
				menuItem.SetBinding(MenuItem.CommandParameterProperty, bindingBase);
				menuItem.SetValue(MenuItem.IsDestructiveProperty, true);
				translate.Text = "SpeedTest_btnRemove.Label";
				IMarkupExtension markupExtension = translate;
				XamlServiceProvider xamlServiceProvider = new XamlServiceProvider();
				Type typeFromHandle = typeof(IProvideValueTarget);
				int num;
				object[] array = new object[(num = this.parentValues.Length) + 2];
				Array.Copy(this.parentValues, 0, array, 2, num);
				object[] array2 = array;
				array2[0] = menuItem;
				array2[1] = viewCell;
				object obj;
				xamlServiceProvider.Add(typeFromHandle, obj = new SimpleValueTargetProvider(array2, MenuItem.TextProperty, nameScope));
				xamlServiceProvider.Add(typeof(IReferenceProvider), obj);
				Type typeFromHandle2 = typeof(IXamlTypeResolver);
				XmlNamespaceResolver xmlNamespaceResolver = new XmlNamespaceResolver();
				xmlNamespaceResolver.Add("", "http://xamarin.com/schemas/2014/forms");
				xmlNamespaceResolver.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
				xmlNamespaceResolver.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
				xmlNamespaceResolver.Add("local", "clr-namespace:CarScannerXamarinForms");
				xamlServiceProvider.Add(typeFromHandle2, new XamlTypeResolver(xmlNamespaceResolver, typeof(DriveCyclesListPage.<InitializeComponent>_anonXamlCDataTemplate_25).GetTypeInfo().Assembly));
				xamlServiceProvider.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(172, 37)));
				object obj2 = markupExtension.ProvideValue(xamlServiceProvider);
				menuItem.Text = obj2;
				viewCell.ContextActions.Add(menuItem);
				stackLayout6.SetValue(StackLayout.OrientationProperty, 0);
				columnDefinition.SetValue(ColumnDefinition.WidthProperty, new GridLengthTypeConverter().ConvertFromInvariantString("*"));
				grid.GetValue(Grid.ColumnDefinitionsProperty).Add(columnDefinition);
				columnDefinition2.SetValue(ColumnDefinition.WidthProperty, new GridLengthTypeConverter().ConvertFromInvariantString("auto"));
				grid.GetValue(Grid.ColumnDefinitionsProperty).Add(columnDefinition2);
				columnDefinition3.SetValue(ColumnDefinition.WidthProperty, new GridLengthTypeConverter().ConvertFromInvariantString("*"));
				grid.GetValue(Grid.ColumnDefinitionsProperty).Add(columnDefinition3);
				label.SetValue(Grid.ColumnProperty, 0);
				label.SetValue(Grid.ColumnSpanProperty, 3);
				dynamicResourceExtension.Key = "BaseFontSize++";
				IMarkupExtension<DynamicResource> markupExtension2 = dynamicResourceExtension;
				XamlServiceProvider xamlServiceProvider2 = new XamlServiceProvider();
				Type typeFromHandle3 = typeof(IProvideValueTarget);
				int num2;
				object[] array3 = new object[(num2 = this.parentValues.Length) + 4];
				Array.Copy(this.parentValues, 0, array3, 4, num2);
				object[] array4 = array3;
				array4[0] = label;
				array4[1] = grid;
				array4[2] = stackLayout6;
				array4[3] = viewCell;
				object obj3;
				xamlServiceProvider2.Add(typeFromHandle3, obj3 = new SimpleValueTargetProvider(array4, Label.FontSizeProperty, nameScope));
				xamlServiceProvider2.Add(typeof(IReferenceProvider), obj3);
				Type typeFromHandle4 = typeof(IXamlTypeResolver);
				XmlNamespaceResolver xmlNamespaceResolver2 = new XmlNamespaceResolver();
				xmlNamespaceResolver2.Add("", "http://xamarin.com/schemas/2014/forms");
				xmlNamespaceResolver2.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
				xmlNamespaceResolver2.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
				xmlNamespaceResolver2.Add("local", "clr-namespace:CarScannerXamarinForms");
				xamlServiceProvider2.Add(typeFromHandle4, new XamlTypeResolver(xmlNamespaceResolver2, typeof(DriveCyclesListPage.<InitializeComponent>_anonXamlCDataTemplate_25).GetTypeInfo().Assembly));
				xamlServiceProvider2.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(50, 41)));
				DynamicResource dynamicResource = markupExtension2.ProvideValue(xamlServiceProvider2);
				label.SetDynamicResource(Label.FontSizeProperty, dynamicResource.Key);
				bindingExtension2.StringFormat = "{0:dd-MM-yyyy HH\\:mm}";
				bindingExtension2.Path = "TimeStarted";
				BindingBase bindingBase2 = bindingExtension2.ProvideValue(null);
				span.SetBinding(Span.TextProperty, bindingBase2);
				formattedString.Spans.Add(span);
				span2.SetValue(Span.TextProperty, " - ");
				formattedString.Spans.Add(span2);
				bindingExtension3.StringFormat = "{0:dd-MM-yyyy HH\\:mm}";
				bindingExtension3.Path = "TimeFinished";
				BindingBase bindingBase3 = bindingExtension3.ProvideValue(null);
				span3.SetBinding(Span.TextProperty, bindingBase3);
				formattedString.Spans.Add(span3);
				span4.SetValue(Span.TextProperty, " (");
				formattedString.Spans.Add(span4);
				bindingExtension4.StringFormat = "{0:hh\\:mm}";
				bindingExtension4.Path = "Duration";
				BindingBase bindingBase4 = bindingExtension4.ProvideValue(null);
				span5.SetBinding(Span.TextProperty, bindingBase4);
				formattedString.Spans.Add(span5);
				span6.SetValue(Span.TextProperty, ")");
				formattedString.Spans.Add(span6);
				label.SetValue(Label.FormattedTextProperty, formattedString);
				grid.Children.Add(label);
				stackLayout6.Children.Add(grid);
				columnDefinition4.SetValue(ColumnDefinition.WidthProperty, new GridLengthTypeConverter().ConvertFromInvariantString("*"));
				grid2.GetValue(Grid.ColumnDefinitionsProperty).Add(columnDefinition4);
				columnDefinition5.SetValue(ColumnDefinition.WidthProperty, new GridLengthTypeConverter().ConvertFromInvariantString("auto"));
				grid2.GetValue(Grid.ColumnDefinitionsProperty).Add(columnDefinition5);
				rowDefinition.SetValue(RowDefinition.HeightProperty, new GridLengthTypeConverter().ConvertFromInvariantString("auto"));
				grid2.GetValue(Grid.RowDefinitionsProperty).Add(rowDefinition);
				rowDefinition2.SetValue(RowDefinition.HeightProperty, new GridLengthTypeConverter().ConvertFromInvariantString("auto"));
				grid2.GetValue(Grid.RowDefinitionsProperty).Add(rowDefinition2);
				rowDefinition3.SetValue(RowDefinition.HeightProperty, new GridLengthTypeConverter().ConvertFromInvariantString("auto"));
				grid2.GetValue(Grid.RowDefinitionsProperty).Add(rowDefinition3);
				rowDefinition4.SetValue(RowDefinition.HeightProperty, new GridLengthTypeConverter().ConvertFromInvariantString("auto"));
				grid2.GetValue(Grid.RowDefinitionsProperty).Add(rowDefinition4);
				rowDefinition5.SetValue(RowDefinition.HeightProperty, new GridLengthTypeConverter().ConvertFromInvariantString("auto"));
				grid2.GetValue(Grid.RowDefinitionsProperty).Add(rowDefinition5);
				rowDefinition6.SetValue(RowDefinition.HeightProperty, new GridLengthTypeConverter().ConvertFromInvariantString("auto"));
				grid2.GetValue(Grid.RowDefinitionsProperty).Add(rowDefinition6);
				label2.SetValue(Grid.RowProperty, 0);
				label2.SetValue(Grid.ColumnProperty, 0);
				dynamicResourceExtension2.Key = "BaseFontSize";
				IMarkupExtension<DynamicResource> markupExtension3 = dynamicResourceExtension2;
				XamlServiceProvider xamlServiceProvider3 = new XamlServiceProvider();
				Type typeFromHandle5 = typeof(IProvideValueTarget);
				int num3;
				object[] array5 = new object[(num3 = this.parentValues.Length) + 4];
				Array.Copy(this.parentValues, 0, array5, 4, num3);
				object[] array6 = array5;
				array6[0] = label2;
				array6[1] = grid2;
				array6[2] = stackLayout6;
				array6[3] = viewCell;
				object obj4;
				xamlServiceProvider3.Add(typeFromHandle5, obj4 = new SimpleValueTargetProvider(array6, Label.FontSizeProperty, nameScope));
				xamlServiceProvider3.Add(typeof(IReferenceProvider), obj4);
				Type typeFromHandle6 = typeof(IXamlTypeResolver);
				XmlNamespaceResolver xmlNamespaceResolver3 = new XmlNamespaceResolver();
				xmlNamespaceResolver3.Add("", "http://xamarin.com/schemas/2014/forms");
				xmlNamespaceResolver3.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
				xmlNamespaceResolver3.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
				xmlNamespaceResolver3.Add("local", "clr-namespace:CarScannerXamarinForms");
				xamlServiceProvider3.Add(typeFromHandle6, new XamlTypeResolver(xmlNamespaceResolver3, typeof(DriveCyclesListPage.<InitializeComponent>_anonXamlCDataTemplate_25).GetTypeInfo().Assembly));
				xamlServiceProvider3.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(97, 41)));
				DynamicResource dynamicResource2 = markupExtension3.ProvideValue(xamlServiceProvider3);
				label2.SetDynamicResource(Label.FontSizeProperty, dynamicResource2.Key);
				translate2.Text = "PID_TotalDistance";
				IMarkupExtension markupExtension4 = translate2;
				XamlServiceProvider xamlServiceProvider4 = new XamlServiceProvider();
				Type typeFromHandle7 = typeof(IProvideValueTarget);
				int num4;
				object[] array7 = new object[(num4 = this.parentValues.Length) + 4];
				Array.Copy(this.parentValues, 0, array7, 4, num4);
				object[] array8 = array7;
				array8[0] = label2;
				array8[1] = grid2;
				array8[2] = stackLayout6;
				array8[3] = viewCell;
				object obj5;
				xamlServiceProvider4.Add(typeFromHandle7, obj5 = new SimpleValueTargetProvider(array8, Label.TextProperty, nameScope));
				xamlServiceProvider4.Add(typeof(IReferenceProvider), obj5);
				Type typeFromHandle8 = typeof(IXamlTypeResolver);
				XmlNamespaceResolver xmlNamespaceResolver4 = new XmlNamespaceResolver();
				xmlNamespaceResolver4.Add("", "http://xamarin.com/schemas/2014/forms");
				xmlNamespaceResolver4.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
				xmlNamespaceResolver4.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
				xmlNamespaceResolver4.Add("local", "clr-namespace:CarScannerXamarinForms");
				xamlServiceProvider4.Add(typeFromHandle8, new XamlTypeResolver(xmlNamespaceResolver4, typeof(DriveCyclesListPage.<InitializeComponent>_anonXamlCDataTemplate_25).GetTypeInfo().Assembly));
				xamlServiceProvider4.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(98, 41)));
				object obj6 = markupExtension4.ProvideValue(xamlServiceProvider4);
				label2.Text = obj6;
				grid2.Children.Add(label2);
				stackLayout.SetValue(Grid.RowProperty, 0);
				stackLayout.SetValue(Grid.ColumnProperty, 1);
				stackLayout.SetValue(View.HorizontalOptionsProperty, LayoutOptions.End);
				stackLayout.SetValue(StackLayout.OrientationProperty, 1);
				dynamicResourceExtension3.Key = "BaseFontSize";
				IMarkupExtension<DynamicResource> markupExtension5 = dynamicResourceExtension3;
				XamlServiceProvider xamlServiceProvider5 = new XamlServiceProvider();
				Type typeFromHandle9 = typeof(IProvideValueTarget);
				int num5;
				object[] array9 = new object[(num5 = this.parentValues.Length) + 5];
				Array.Copy(this.parentValues, 0, array9, 5, num5);
				object[] array10 = array9;
				array10[0] = label3;
				array10[1] = stackLayout;
				array10[2] = grid2;
				array10[3] = stackLayout6;
				array10[4] = viewCell;
				object obj7;
				xamlServiceProvider5.Add(typeFromHandle9, obj7 = new SimpleValueTargetProvider(array10, Label.FontSizeProperty, nameScope));
				xamlServiceProvider5.Add(typeof(IReferenceProvider), obj7);
				Type typeFromHandle10 = typeof(IXamlTypeResolver);
				XmlNamespaceResolver xmlNamespaceResolver5 = new XmlNamespaceResolver();
				xmlNamespaceResolver5.Add("", "http://xamarin.com/schemas/2014/forms");
				xmlNamespaceResolver5.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
				xmlNamespaceResolver5.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
				xmlNamespaceResolver5.Add("local", "clr-namespace:CarScannerXamarinForms");
				xamlServiceProvider5.Add(typeFromHandle10, new XamlTypeResolver(xmlNamespaceResolver5, typeof(DriveCyclesListPage.<InitializeComponent>_anonXamlCDataTemplate_25).GetTypeInfo().Assembly));
				xamlServiceProvider5.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(104, 48)));
				DynamicResource dynamicResource3 = markupExtension5.ProvideValue(xamlServiceProvider5);
				label3.SetDynamicResource(Label.FontSizeProperty, dynamicResource3.Key);
				bindingExtension5.StringFormat = "{0:N}";
				bindingExtension5.Path = "DistanceUserUnits";
				BindingBase bindingBase5 = bindingExtension5.ProvideValue(null);
				label3.SetBinding(Label.TextProperty, bindingBase5);
				stackLayout.Children.Add(label3);
				dynamicResourceExtension4.Key = "BaseFontSize";
				IMarkupExtension<DynamicResource> markupExtension6 = dynamicResourceExtension4;
				XamlServiceProvider xamlServiceProvider6 = new XamlServiceProvider();
				Type typeFromHandle11 = typeof(IProvideValueTarget);
				int num6;
				object[] array11 = new object[(num6 = this.parentValues.Length) + 5];
				Array.Copy(this.parentValues, 0, array11, 5, num6);
				object[] array12 = array11;
				array12[0] = label4;
				array12[1] = stackLayout;
				array12[2] = grid2;
				array12[3] = stackLayout6;
				array12[4] = viewCell;
				object obj8;
				xamlServiceProvider6.Add(typeFromHandle11, obj8 = new SimpleValueTargetProvider(array12, Label.FontSizeProperty, nameScope));
				xamlServiceProvider6.Add(typeof(IReferenceProvider), obj8);
				Type typeFromHandle12 = typeof(IXamlTypeResolver);
				XmlNamespaceResolver xmlNamespaceResolver6 = new XmlNamespaceResolver();
				xmlNamespaceResolver6.Add("", "http://xamarin.com/schemas/2014/forms");
				xmlNamespaceResolver6.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
				xmlNamespaceResolver6.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
				xmlNamespaceResolver6.Add("local", "clr-namespace:CarScannerXamarinForms");
				xamlServiceProvider6.Add(typeFromHandle12, new XamlTypeResolver(xmlNamespaceResolver6, typeof(DriveCyclesListPage.<InitializeComponent>_anonXamlCDataTemplate_25).GetTypeInfo().Assembly));
				xamlServiceProvider6.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(105, 48)));
				DynamicResource dynamicResource4 = markupExtension6.ProvideValue(xamlServiceProvider6);
				label4.SetDynamicResource(Label.FontSizeProperty, dynamicResource4.Key);
				bindingExtension6.Path = "DistanceUnits";
				BindingBase bindingBase6 = bindingExtension6.ProvideValue(null);
				label4.SetBinding(Label.TextProperty, bindingBase6);
				stackLayout.Children.Add(label4);
				grid2.Children.Add(stackLayout);
				label5.SetValue(Grid.RowProperty, 1);
				label5.SetValue(Grid.ColumnProperty, 0);
				dynamicResourceExtension5.Key = "BaseFontSize";
				IMarkupExtension<DynamicResource> markupExtension7 = dynamicResourceExtension5;
				XamlServiceProvider xamlServiceProvider7 = new XamlServiceProvider();
				Type typeFromHandle13 = typeof(IProvideValueTarget);
				int num7;
				object[] array13 = new object[(num7 = this.parentValues.Length) + 4];
				Array.Copy(this.parentValues, 0, array13, 4, num7);
				object[] array14 = array13;
				array14[0] = label5;
				array14[1] = grid2;
				array14[2] = stackLayout6;
				array14[3] = viewCell;
				object obj9;
				xamlServiceProvider7.Add(typeFromHandle13, obj9 = new SimpleValueTargetProvider(array14, Label.FontSizeProperty, nameScope));
				xamlServiceProvider7.Add(typeof(IReferenceProvider), obj9);
				Type typeFromHandle14 = typeof(IXamlTypeResolver);
				XmlNamespaceResolver xmlNamespaceResolver7 = new XmlNamespaceResolver();
				xmlNamespaceResolver7.Add("", "http://xamarin.com/schemas/2014/forms");
				xmlNamespaceResolver7.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
				xmlNamespaceResolver7.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
				xmlNamespaceResolver7.Add("local", "clr-namespace:CarScannerXamarinForms");
				xamlServiceProvider7.Add(typeFromHandle14, new XamlTypeResolver(xmlNamespaceResolver7, typeof(DriveCyclesListPage.<InitializeComponent>_anonXamlCDataTemplate_25).GetTypeInfo().Assembly));
				xamlServiceProvider7.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(111, 41)));
				DynamicResource dynamicResource5 = markupExtension7.ProvideValue(xamlServiceProvider7);
				label5.SetDynamicResource(Label.FontSizeProperty, dynamicResource5.Key);
				translate3.Text = "PID_TotalFuelUsed";
				IMarkupExtension markupExtension8 = translate3;
				XamlServiceProvider xamlServiceProvider8 = new XamlServiceProvider();
				Type typeFromHandle15 = typeof(IProvideValueTarget);
				int num8;
				object[] array15 = new object[(num8 = this.parentValues.Length) + 4];
				Array.Copy(this.parentValues, 0, array15, 4, num8);
				object[] array16 = array15;
				array16[0] = label5;
				array16[1] = grid2;
				array16[2] = stackLayout6;
				array16[3] = viewCell;
				object obj10;
				xamlServiceProvider8.Add(typeFromHandle15, obj10 = new SimpleValueTargetProvider(array16, Label.TextProperty, nameScope));
				xamlServiceProvider8.Add(typeof(IReferenceProvider), obj10);
				Type typeFromHandle16 = typeof(IXamlTypeResolver);
				XmlNamespaceResolver xmlNamespaceResolver8 = new XmlNamespaceResolver();
				xmlNamespaceResolver8.Add("", "http://xamarin.com/schemas/2014/forms");
				xmlNamespaceResolver8.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
				xmlNamespaceResolver8.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
				xmlNamespaceResolver8.Add("local", "clr-namespace:CarScannerXamarinForms");
				xamlServiceProvider8.Add(typeFromHandle16, new XamlTypeResolver(xmlNamespaceResolver8, typeof(DriveCyclesListPage.<InitializeComponent>_anonXamlCDataTemplate_25).GetTypeInfo().Assembly));
				xamlServiceProvider8.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(112, 41)));
				object obj11 = markupExtension8.ProvideValue(xamlServiceProvider8);
				label5.Text = obj11;
				grid2.Children.Add(label5);
				stackLayout2.SetValue(Grid.RowProperty, 1);
				stackLayout2.SetValue(Grid.ColumnProperty, 1);
				stackLayout2.SetValue(View.HorizontalOptionsProperty, LayoutOptions.End);
				stackLayout2.SetValue(StackLayout.OrientationProperty, 1);
				dynamicResourceExtension6.Key = "BaseFontSize";
				IMarkupExtension<DynamicResource> markupExtension9 = dynamicResourceExtension6;
				XamlServiceProvider xamlServiceProvider9 = new XamlServiceProvider();
				Type typeFromHandle17 = typeof(IProvideValueTarget);
				int num9;
				object[] array17 = new object[(num9 = this.parentValues.Length) + 5];
				Array.Copy(this.parentValues, 0, array17, 5, num9);
				object[] array18 = array17;
				array18[0] = label6;
				array18[1] = stackLayout2;
				array18[2] = grid2;
				array18[3] = stackLayout6;
				array18[4] = viewCell;
				object obj12;
				xamlServiceProvider9.Add(typeFromHandle17, obj12 = new SimpleValueTargetProvider(array18, Label.FontSizeProperty, nameScope));
				xamlServiceProvider9.Add(typeof(IReferenceProvider), obj12);
				Type typeFromHandle18 = typeof(IXamlTypeResolver);
				XmlNamespaceResolver xmlNamespaceResolver9 = new XmlNamespaceResolver();
				xmlNamespaceResolver9.Add("", "http://xamarin.com/schemas/2014/forms");
				xmlNamespaceResolver9.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
				xmlNamespaceResolver9.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
				xmlNamespaceResolver9.Add("local", "clr-namespace:CarScannerXamarinForms");
				xamlServiceProvider9.Add(typeFromHandle18, new XamlTypeResolver(xmlNamespaceResolver9, typeof(DriveCyclesListPage.<InitializeComponent>_anonXamlCDataTemplate_25).GetTypeInfo().Assembly));
				xamlServiceProvider9.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(118, 48)));
				DynamicResource dynamicResource6 = markupExtension9.ProvideValue(xamlServiceProvider9);
				label6.SetDynamicResource(Label.FontSizeProperty, dynamicResource6.Key);
				bindingExtension7.StringFormat = "{0:N}";
				bindingExtension7.Path = "FuelUsedUserUnits";
				BindingBase bindingBase7 = bindingExtension7.ProvideValue(null);
				label6.SetBinding(Label.TextProperty, bindingBase7);
				stackLayout2.Children.Add(label6);
				dynamicResourceExtension7.Key = "BaseFontSize";
				IMarkupExtension<DynamicResource> markupExtension10 = dynamicResourceExtension7;
				XamlServiceProvider xamlServiceProvider10 = new XamlServiceProvider();
				Type typeFromHandle19 = typeof(IProvideValueTarget);
				int num10;
				object[] array19 = new object[(num10 = this.parentValues.Length) + 5];
				Array.Copy(this.parentValues, 0, array19, 5, num10);
				object[] array20 = array19;
				array20[0] = label7;
				array20[1] = stackLayout2;
				array20[2] = grid2;
				array20[3] = stackLayout6;
				array20[4] = viewCell;
				object obj13;
				xamlServiceProvider10.Add(typeFromHandle19, obj13 = new SimpleValueTargetProvider(array20, Label.FontSizeProperty, nameScope));
				xamlServiceProvider10.Add(typeof(IReferenceProvider), obj13);
				Type typeFromHandle20 = typeof(IXamlTypeResolver);
				XmlNamespaceResolver xmlNamespaceResolver10 = new XmlNamespaceResolver();
				xmlNamespaceResolver10.Add("", "http://xamarin.com/schemas/2014/forms");
				xmlNamespaceResolver10.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
				xmlNamespaceResolver10.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
				xmlNamespaceResolver10.Add("local", "clr-namespace:CarScannerXamarinForms");
				xamlServiceProvider10.Add(typeFromHandle20, new XamlTypeResolver(xmlNamespaceResolver10, typeof(DriveCyclesListPage.<InitializeComponent>_anonXamlCDataTemplate_25).GetTypeInfo().Assembly));
				xamlServiceProvider10.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(119, 48)));
				DynamicResource dynamicResource7 = markupExtension10.ProvideValue(xamlServiceProvider10);
				label7.SetDynamicResource(Label.FontSizeProperty, dynamicResource7.Key);
				bindingExtension8.Path = "FuelUsedUnits";
				BindingBase bindingBase8 = bindingExtension8.ProvideValue(null);
				label7.SetBinding(Label.TextProperty, bindingBase8);
				stackLayout2.Children.Add(label7);
				grid2.Children.Add(stackLayout2);
				label8.SetValue(Grid.RowProperty, 2);
				label8.SetValue(Grid.ColumnProperty, 0);
				dynamicResourceExtension8.Key = "BaseFontSize";
				IMarkupExtension<DynamicResource> markupExtension11 = dynamicResourceExtension8;
				XamlServiceProvider xamlServiceProvider11 = new XamlServiceProvider();
				Type typeFromHandle21 = typeof(IProvideValueTarget);
				int num11;
				object[] array21 = new object[(num11 = this.parentValues.Length) + 4];
				Array.Copy(this.parentValues, 0, array21, 4, num11);
				object[] array22 = array21;
				array22[0] = label8;
				array22[1] = grid2;
				array22[2] = stackLayout6;
				array22[3] = viewCell;
				object obj14;
				xamlServiceProvider11.Add(typeFromHandle21, obj14 = new SimpleValueTargetProvider(array22, Label.FontSizeProperty, nameScope));
				xamlServiceProvider11.Add(typeof(IReferenceProvider), obj14);
				Type typeFromHandle22 = typeof(IXamlTypeResolver);
				XmlNamespaceResolver xmlNamespaceResolver11 = new XmlNamespaceResolver();
				xmlNamespaceResolver11.Add("", "http://xamarin.com/schemas/2014/forms");
				xmlNamespaceResolver11.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
				xmlNamespaceResolver11.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
				xmlNamespaceResolver11.Add("local", "clr-namespace:CarScannerXamarinForms");
				xamlServiceProvider11.Add(typeFromHandle22, new XamlTypeResolver(xmlNamespaceResolver11, typeof(DriveCyclesListPage.<InitializeComponent>_anonXamlCDataTemplate_25).GetTypeInfo().Assembly));
				xamlServiceProvider11.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(125, 41)));
				DynamicResource dynamicResource8 = markupExtension11.ProvideValue(xamlServiceProvider11);
				label8.SetDynamicResource(Label.FontSizeProperty, dynamicResource8.Key);
				translate4.Text = "PID_FuelMoney";
				IMarkupExtension markupExtension12 = translate4;
				XamlServiceProvider xamlServiceProvider12 = new XamlServiceProvider();
				Type typeFromHandle23 = typeof(IProvideValueTarget);
				int num12;
				object[] array23 = new object[(num12 = this.parentValues.Length) + 4];
				Array.Copy(this.parentValues, 0, array23, 4, num12);
				object[] array24 = array23;
				array24[0] = label8;
				array24[1] = grid2;
				array24[2] = stackLayout6;
				array24[3] = viewCell;
				object obj15;
				xamlServiceProvider12.Add(typeFromHandle23, obj15 = new SimpleValueTargetProvider(array24, Label.TextProperty, nameScope));
				xamlServiceProvider12.Add(typeof(IReferenceProvider), obj15);
				Type typeFromHandle24 = typeof(IXamlTypeResolver);
				XmlNamespaceResolver xmlNamespaceResolver12 = new XmlNamespaceResolver();
				xmlNamespaceResolver12.Add("", "http://xamarin.com/schemas/2014/forms");
				xmlNamespaceResolver12.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
				xmlNamespaceResolver12.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
				xmlNamespaceResolver12.Add("local", "clr-namespace:CarScannerXamarinForms");
				xamlServiceProvider12.Add(typeFromHandle24, new XamlTypeResolver(xmlNamespaceResolver12, typeof(DriveCyclesListPage.<InitializeComponent>_anonXamlCDataTemplate_25).GetTypeInfo().Assembly));
				xamlServiceProvider12.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(126, 41)));
				object obj16 = markupExtension12.ProvideValue(xamlServiceProvider12);
				label8.Text = obj16;
				grid2.Children.Add(label8);
				stackLayout3.SetValue(Grid.RowProperty, 2);
				stackLayout3.SetValue(Grid.ColumnProperty, 1);
				stackLayout3.SetValue(View.HorizontalOptionsProperty, LayoutOptions.End);
				stackLayout3.SetValue(StackLayout.OrientationProperty, 1);
				dynamicResourceExtension9.Key = "BaseFontSize";
				IMarkupExtension<DynamicResource> markupExtension13 = dynamicResourceExtension9;
				XamlServiceProvider xamlServiceProvider13 = new XamlServiceProvider();
				Type typeFromHandle25 = typeof(IProvideValueTarget);
				int num13;
				object[] array25 = new object[(num13 = this.parentValues.Length) + 5];
				Array.Copy(this.parentValues, 0, array25, 5, num13);
				object[] array26 = array25;
				array26[0] = label9;
				array26[1] = stackLayout3;
				array26[2] = grid2;
				array26[3] = stackLayout6;
				array26[4] = viewCell;
				object obj17;
				xamlServiceProvider13.Add(typeFromHandle25, obj17 = new SimpleValueTargetProvider(array26, Label.FontSizeProperty, nameScope));
				xamlServiceProvider13.Add(typeof(IReferenceProvider), obj17);
				Type typeFromHandle26 = typeof(IXamlTypeResolver);
				XmlNamespaceResolver xmlNamespaceResolver13 = new XmlNamespaceResolver();
				xmlNamespaceResolver13.Add("", "http://xamarin.com/schemas/2014/forms");
				xmlNamespaceResolver13.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
				xmlNamespaceResolver13.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
				xmlNamespaceResolver13.Add("local", "clr-namespace:CarScannerXamarinForms");
				xamlServiceProvider13.Add(typeFromHandle26, new XamlTypeResolver(xmlNamespaceResolver13, typeof(DriveCyclesListPage.<InitializeComponent>_anonXamlCDataTemplate_25).GetTypeInfo().Assembly));
				xamlServiceProvider13.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(132, 48)));
				DynamicResource dynamicResource9 = markupExtension13.ProvideValue(xamlServiceProvider13);
				label9.SetDynamicResource(Label.FontSizeProperty, dynamicResource9.Key);
				bindingExtension9.StringFormat = "{0:N}";
				bindingExtension9.Path = "TotalFuelPrice";
				BindingBase bindingBase9 = bindingExtension9.ProvideValue(null);
				label9.SetBinding(Label.TextProperty, bindingBase9);
				stackLayout3.Children.Add(label9);
				dynamicResourceExtension10.Key = "BaseFontSize";
				IMarkupExtension<DynamicResource> markupExtension14 = dynamicResourceExtension10;
				XamlServiceProvider xamlServiceProvider14 = new XamlServiceProvider();
				Type typeFromHandle27 = typeof(IProvideValueTarget);
				int num14;
				object[] array27 = new object[(num14 = this.parentValues.Length) + 5];
				Array.Copy(this.parentValues, 0, array27, 5, num14);
				object[] array28 = array27;
				array28[0] = label10;
				array28[1] = stackLayout3;
				array28[2] = grid2;
				array28[3] = stackLayout6;
				array28[4] = viewCell;
				object obj18;
				xamlServiceProvider14.Add(typeFromHandle27, obj18 = new SimpleValueTargetProvider(array28, Label.FontSizeProperty, nameScope));
				xamlServiceProvider14.Add(typeof(IReferenceProvider), obj18);
				Type typeFromHandle28 = typeof(IXamlTypeResolver);
				XmlNamespaceResolver xmlNamespaceResolver14 = new XmlNamespaceResolver();
				xmlNamespaceResolver14.Add("", "http://xamarin.com/schemas/2014/forms");
				xmlNamespaceResolver14.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
				xmlNamespaceResolver14.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
				xmlNamespaceResolver14.Add("local", "clr-namespace:CarScannerXamarinForms");
				xamlServiceProvider14.Add(typeFromHandle28, new XamlTypeResolver(xmlNamespaceResolver14, typeof(DriveCyclesListPage.<InitializeComponent>_anonXamlCDataTemplate_25).GetTypeInfo().Assembly));
				xamlServiceProvider14.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(133, 48)));
				DynamicResource dynamicResource10 = markupExtension14.ProvideValue(xamlServiceProvider14);
				label10.SetDynamicResource(Label.FontSizeProperty, dynamicResource10.Key);
				bindingExtension10.Path = "FuelPriceUnits";
				BindingBase bindingBase10 = bindingExtension10.ProvideValue(null);
				label10.SetBinding(Label.TextProperty, bindingBase10);
				stackLayout3.Children.Add(label10);
				grid2.Children.Add(stackLayout3);
				label11.SetValue(Grid.RowProperty, 3);
				label11.SetValue(Grid.ColumnProperty, 0);
				dynamicResourceExtension11.Key = "BaseFontSize";
				IMarkupExtension<DynamicResource> markupExtension15 = dynamicResourceExtension11;
				XamlServiceProvider xamlServiceProvider15 = new XamlServiceProvider();
				Type typeFromHandle29 = typeof(IProvideValueTarget);
				int num15;
				object[] array29 = new object[(num15 = this.parentValues.Length) + 4];
				Array.Copy(this.parentValues, 0, array29, 4, num15);
				object[] array30 = array29;
				array30[0] = label11;
				array30[1] = grid2;
				array30[2] = stackLayout6;
				array30[3] = viewCell;
				object obj19;
				xamlServiceProvider15.Add(typeFromHandle29, obj19 = new SimpleValueTargetProvider(array30, Label.FontSizeProperty, nameScope));
				xamlServiceProvider15.Add(typeof(IReferenceProvider), obj19);
				Type typeFromHandle30 = typeof(IXamlTypeResolver);
				XmlNamespaceResolver xmlNamespaceResolver15 = new XmlNamespaceResolver();
				xmlNamespaceResolver15.Add("", "http://xamarin.com/schemas/2014/forms");
				xmlNamespaceResolver15.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
				xmlNamespaceResolver15.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
				xmlNamespaceResolver15.Add("local", "clr-namespace:CarScannerXamarinForms");
				xamlServiceProvider15.Add(typeFromHandle30, new XamlTypeResolver(xmlNamespaceResolver15, typeof(DriveCyclesListPage.<InitializeComponent>_anonXamlCDataTemplate_25).GetTypeInfo().Assembly));
				xamlServiceProvider15.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(139, 41)));
				DynamicResource dynamicResource11 = markupExtension15.ProvideValue(xamlServiceProvider15);
				label11.SetDynamicResource(Label.FontSizeProperty, dynamicResource11.Key);
				translate5.Text = "PID_AvgFuelConsumption_Short";
				IMarkupExtension markupExtension16 = translate5;
				XamlServiceProvider xamlServiceProvider16 = new XamlServiceProvider();
				Type typeFromHandle31 = typeof(IProvideValueTarget);
				int num16;
				object[] array31 = new object[(num16 = this.parentValues.Length) + 4];
				Array.Copy(this.parentValues, 0, array31, 4, num16);
				object[] array32 = array31;
				array32[0] = label11;
				array32[1] = grid2;
				array32[2] = stackLayout6;
				array32[3] = viewCell;
				object obj20;
				xamlServiceProvider16.Add(typeFromHandle31, obj20 = new SimpleValueTargetProvider(array32, Label.TextProperty, nameScope));
				xamlServiceProvider16.Add(typeof(IReferenceProvider), obj20);
				Type typeFromHandle32 = typeof(IXamlTypeResolver);
				XmlNamespaceResolver xmlNamespaceResolver16 = new XmlNamespaceResolver();
				xmlNamespaceResolver16.Add("", "http://xamarin.com/schemas/2014/forms");
				xmlNamespaceResolver16.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
				xmlNamespaceResolver16.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
				xmlNamespaceResolver16.Add("local", "clr-namespace:CarScannerXamarinForms");
				xamlServiceProvider16.Add(typeFromHandle32, new XamlTypeResolver(xmlNamespaceResolver16, typeof(DriveCyclesListPage.<InitializeComponent>_anonXamlCDataTemplate_25).GetTypeInfo().Assembly));
				xamlServiceProvider16.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(140, 41)));
				object obj21 = markupExtension16.ProvideValue(xamlServiceProvider16);
				label11.Text = obj21;
				grid2.Children.Add(label11);
				stackLayout4.SetValue(Grid.RowProperty, 3);
				stackLayout4.SetValue(Grid.ColumnProperty, 1);
				stackLayout4.SetValue(View.HorizontalOptionsProperty, LayoutOptions.End);
				stackLayout4.SetValue(StackLayout.OrientationProperty, 1);
				dynamicResourceExtension12.Key = "BaseFontSize";
				IMarkupExtension<DynamicResource> markupExtension17 = dynamicResourceExtension12;
				XamlServiceProvider xamlServiceProvider17 = new XamlServiceProvider();
				Type typeFromHandle33 = typeof(IProvideValueTarget);
				int num17;
				object[] array33 = new object[(num17 = this.parentValues.Length) + 5];
				Array.Copy(this.parentValues, 0, array33, 5, num17);
				object[] array34 = array33;
				array34[0] = label12;
				array34[1] = stackLayout4;
				array34[2] = grid2;
				array34[3] = stackLayout6;
				array34[4] = viewCell;
				object obj22;
				xamlServiceProvider17.Add(typeFromHandle33, obj22 = new SimpleValueTargetProvider(array34, Label.FontSizeProperty, nameScope));
				xamlServiceProvider17.Add(typeof(IReferenceProvider), obj22);
				Type typeFromHandle34 = typeof(IXamlTypeResolver);
				XmlNamespaceResolver xmlNamespaceResolver17 = new XmlNamespaceResolver();
				xmlNamespaceResolver17.Add("", "http://xamarin.com/schemas/2014/forms");
				xmlNamespaceResolver17.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
				xmlNamespaceResolver17.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
				xmlNamespaceResolver17.Add("local", "clr-namespace:CarScannerXamarinForms");
				xamlServiceProvider17.Add(typeFromHandle34, new XamlTypeResolver(xmlNamespaceResolver17, typeof(DriveCyclesListPage.<InitializeComponent>_anonXamlCDataTemplate_25).GetTypeInfo().Assembly));
				xamlServiceProvider17.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(146, 48)));
				DynamicResource dynamicResource12 = markupExtension17.ProvideValue(xamlServiceProvider17);
				label12.SetDynamicResource(Label.FontSizeProperty, dynamicResource12.Key);
				bindingExtension11.StringFormat = "{0:N}";
				bindingExtension11.Path = "AvgFuelConsumptionUserUnits";
				BindingBase bindingBase11 = bindingExtension11.ProvideValue(null);
				label12.SetBinding(Label.TextProperty, bindingBase11);
				stackLayout4.Children.Add(label12);
				dynamicResourceExtension13.Key = "BaseFontSize";
				IMarkupExtension<DynamicResource> markupExtension18 = dynamicResourceExtension13;
				XamlServiceProvider xamlServiceProvider18 = new XamlServiceProvider();
				Type typeFromHandle35 = typeof(IProvideValueTarget);
				int num18;
				object[] array35 = new object[(num18 = this.parentValues.Length) + 5];
				Array.Copy(this.parentValues, 0, array35, 5, num18);
				object[] array36 = array35;
				array36[0] = label13;
				array36[1] = stackLayout4;
				array36[2] = grid2;
				array36[3] = stackLayout6;
				array36[4] = viewCell;
				object obj23;
				xamlServiceProvider18.Add(typeFromHandle35, obj23 = new SimpleValueTargetProvider(array36, Label.FontSizeProperty, nameScope));
				xamlServiceProvider18.Add(typeof(IReferenceProvider), obj23);
				Type typeFromHandle36 = typeof(IXamlTypeResolver);
				XmlNamespaceResolver xmlNamespaceResolver18 = new XmlNamespaceResolver();
				xmlNamespaceResolver18.Add("", "http://xamarin.com/schemas/2014/forms");
				xmlNamespaceResolver18.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
				xmlNamespaceResolver18.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
				xmlNamespaceResolver18.Add("local", "clr-namespace:CarScannerXamarinForms");
				xamlServiceProvider18.Add(typeFromHandle36, new XamlTypeResolver(xmlNamespaceResolver18, typeof(DriveCyclesListPage.<InitializeComponent>_anonXamlCDataTemplate_25).GetTypeInfo().Assembly));
				xamlServiceProvider18.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(147, 48)));
				DynamicResource dynamicResource13 = markupExtension18.ProvideValue(xamlServiceProvider18);
				label13.SetDynamicResource(Label.FontSizeProperty, dynamicResource13.Key);
				bindingExtension12.Path = "FuelConsumptionsUnits";
				BindingBase bindingBase12 = bindingExtension12.ProvideValue(null);
				label13.SetBinding(Label.TextProperty, bindingBase12);
				stackLayout4.Children.Add(label13);
				grid2.Children.Add(stackLayout4);
				label14.SetValue(Grid.RowProperty, 4);
				label14.SetValue(Grid.ColumnProperty, 0);
				dynamicResourceExtension14.Key = "BaseFontSize";
				IMarkupExtension<DynamicResource> markupExtension19 = dynamicResourceExtension14;
				XamlServiceProvider xamlServiceProvider19 = new XamlServiceProvider();
				Type typeFromHandle37 = typeof(IProvideValueTarget);
				int num19;
				object[] array37 = new object[(num19 = this.parentValues.Length) + 4];
				Array.Copy(this.parentValues, 0, array37, 4, num19);
				object[] array38 = array37;
				array38[0] = label14;
				array38[1] = grid2;
				array38[2] = stackLayout6;
				array38[3] = viewCell;
				object obj24;
				xamlServiceProvider19.Add(typeFromHandle37, obj24 = new SimpleValueTargetProvider(array38, Label.FontSizeProperty, nameScope));
				xamlServiceProvider19.Add(typeof(IReferenceProvider), obj24);
				Type typeFromHandle38 = typeof(IXamlTypeResolver);
				XmlNamespaceResolver xmlNamespaceResolver19 = new XmlNamespaceResolver();
				xmlNamespaceResolver19.Add("", "http://xamarin.com/schemas/2014/forms");
				xmlNamespaceResolver19.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
				xmlNamespaceResolver19.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
				xmlNamespaceResolver19.Add("local", "clr-namespace:CarScannerXamarinForms");
				xamlServiceProvider19.Add(typeFromHandle38, new XamlTypeResolver(xmlNamespaceResolver19, typeof(DriveCyclesListPage.<InitializeComponent>_anonXamlCDataTemplate_25).GetTypeInfo().Assembly));
				xamlServiceProvider19.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(153, 41)));
				DynamicResource dynamicResource14 = markupExtension19.ProvideValue(xamlServiceProvider19);
				label14.SetDynamicResource(Label.FontSizeProperty, dynamicResource14.Key);
				translate6.Text = "PID_CalculatedAvgSpeed";
				IMarkupExtension markupExtension20 = translate6;
				XamlServiceProvider xamlServiceProvider20 = new XamlServiceProvider();
				Type typeFromHandle39 = typeof(IProvideValueTarget);
				int num20;
				object[] array39 = new object[(num20 = this.parentValues.Length) + 4];
				Array.Copy(this.parentValues, 0, array39, 4, num20);
				object[] array40 = array39;
				array40[0] = label14;
				array40[1] = grid2;
				array40[2] = stackLayout6;
				array40[3] = viewCell;
				object obj25;
				xamlServiceProvider20.Add(typeFromHandle39, obj25 = new SimpleValueTargetProvider(array40, Label.TextProperty, nameScope));
				xamlServiceProvider20.Add(typeof(IReferenceProvider), obj25);
				Type typeFromHandle40 = typeof(IXamlTypeResolver);
				XmlNamespaceResolver xmlNamespaceResolver20 = new XmlNamespaceResolver();
				xmlNamespaceResolver20.Add("", "http://xamarin.com/schemas/2014/forms");
				xmlNamespaceResolver20.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
				xmlNamespaceResolver20.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
				xmlNamespaceResolver20.Add("local", "clr-namespace:CarScannerXamarinForms");
				xamlServiceProvider20.Add(typeFromHandle40, new XamlTypeResolver(xmlNamespaceResolver20, typeof(DriveCyclesListPage.<InitializeComponent>_anonXamlCDataTemplate_25).GetTypeInfo().Assembly));
				xamlServiceProvider20.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(154, 41)));
				object obj26 = markupExtension20.ProvideValue(xamlServiceProvider20);
				label14.Text = obj26;
				grid2.Children.Add(label14);
				stackLayout5.SetValue(Grid.RowProperty, 4);
				stackLayout5.SetValue(Grid.ColumnProperty, 1);
				stackLayout5.SetValue(View.MarginProperty, new Thickness(0.0, 0.0, 0.0, 5.0));
				stackLayout5.SetValue(View.HorizontalOptionsProperty, LayoutOptions.End);
				stackLayout5.SetValue(StackLayout.OrientationProperty, 1);
				dynamicResourceExtension15.Key = "BaseFontSize";
				IMarkupExtension<DynamicResource> markupExtension21 = dynamicResourceExtension15;
				XamlServiceProvider xamlServiceProvider21 = new XamlServiceProvider();
				Type typeFromHandle41 = typeof(IProvideValueTarget);
				int num21;
				object[] array41 = new object[(num21 = this.parentValues.Length) + 5];
				Array.Copy(this.parentValues, 0, array41, 5, num21);
				object[] array42 = array41;
				array42[0] = label15;
				array42[1] = stackLayout5;
				array42[2] = grid2;
				array42[3] = stackLayout6;
				array42[4] = viewCell;
				object obj27;
				xamlServiceProvider21.Add(typeFromHandle41, obj27 = new SimpleValueTargetProvider(array42, Label.FontSizeProperty, nameScope));
				xamlServiceProvider21.Add(typeof(IReferenceProvider), obj27);
				Type typeFromHandle42 = typeof(IXamlTypeResolver);
				XmlNamespaceResolver xmlNamespaceResolver21 = new XmlNamespaceResolver();
				xmlNamespaceResolver21.Add("", "http://xamarin.com/schemas/2014/forms");
				xmlNamespaceResolver21.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
				xmlNamespaceResolver21.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
				xmlNamespaceResolver21.Add("local", "clr-namespace:CarScannerXamarinForms");
				xamlServiceProvider21.Add(typeFromHandle42, new XamlTypeResolver(xmlNamespaceResolver21, typeof(DriveCyclesListPage.<InitializeComponent>_anonXamlCDataTemplate_25).GetTypeInfo().Assembly));
				xamlServiceProvider21.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(161, 48)));
				DynamicResource dynamicResource15 = markupExtension21.ProvideValue(xamlServiceProvider21);
				label15.SetDynamicResource(Label.FontSizeProperty, dynamicResource15.Key);
				bindingExtension13.StringFormat = "{0:N}";
				bindingExtension13.Path = "AvgSpeedUserUnits";
				BindingBase bindingBase13 = bindingExtension13.ProvideValue(null);
				label15.SetBinding(Label.TextProperty, bindingBase13);
				stackLayout5.Children.Add(label15);
				dynamicResourceExtension16.Key = "BaseFontSize";
				IMarkupExtension<DynamicResource> markupExtension22 = dynamicResourceExtension16;
				XamlServiceProvider xamlServiceProvider22 = new XamlServiceProvider();
				Type typeFromHandle43 = typeof(IProvideValueTarget);
				int num22;
				object[] array43 = new object[(num22 = this.parentValues.Length) + 5];
				Array.Copy(this.parentValues, 0, array43, 5, num22);
				object[] array44 = array43;
				array44[0] = label16;
				array44[1] = stackLayout5;
				array44[2] = grid2;
				array44[3] = stackLayout6;
				array44[4] = viewCell;
				object obj28;
				xamlServiceProvider22.Add(typeFromHandle43, obj28 = new SimpleValueTargetProvider(array44, Label.FontSizeProperty, nameScope));
				xamlServiceProvider22.Add(typeof(IReferenceProvider), obj28);
				Type typeFromHandle44 = typeof(IXamlTypeResolver);
				XmlNamespaceResolver xmlNamespaceResolver22 = new XmlNamespaceResolver();
				xmlNamespaceResolver22.Add("", "http://xamarin.com/schemas/2014/forms");
				xmlNamespaceResolver22.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
				xmlNamespaceResolver22.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
				xmlNamespaceResolver22.Add("local", "clr-namespace:CarScannerXamarinForms");
				xamlServiceProvider22.Add(typeFromHandle44, new XamlTypeResolver(xmlNamespaceResolver22, typeof(DriveCyclesListPage.<InitializeComponent>_anonXamlCDataTemplate_25).GetTypeInfo().Assembly));
				xamlServiceProvider22.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(162, 48)));
				DynamicResource dynamicResource16 = markupExtension22.ProvideValue(xamlServiceProvider22);
				label16.SetDynamicResource(Label.FontSizeProperty, dynamicResource16.Key);
				bindingExtension14.Path = "SpeedUnits";
				BindingBase bindingBase14 = bindingExtension14.ProvideValue(null);
				label16.SetBinding(Label.TextProperty, bindingBase14);
				stackLayout5.Children.Add(label16);
				grid2.Children.Add(stackLayout5);
				stackLayout6.Children.Add(grid2);
				viewCell.View = stackLayout6;
				return viewCell;
			}

			// Token: 0x04002098 RID: 8344
			internal object[] parentValues;

			// Token: 0x04002099 RID: 8345
			internal DriveCyclesListPage root;
		}
	}
}
