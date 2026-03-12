using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using CarScannerXamarinForms.OBD2.PIDS;
using CarScannerXamarinForms.PlatformAdapters;
using CarScannerXamarinForms.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Internals;
using Xamarin.Forms.PlatformConfiguration.iOSSpecific;
using Xamarin.Forms.Xaml;
using Xamarin.Forms.Xaml.Diagnostics;
using Xamarin.Forms.Xaml.Internals;

namespace CarScannerXamarinForms.Pages.Dashboard
{
	// Token: 0x020006BE RID: 1726
	[XamlCompilation(2)]
	[XamlFilePath("Pages\\Dashboard\\DashboardMultiPidSelector.xaml")]
	public class DashboardMultiPidSelector : ContentPage
	{
		// Token: 0x06003AF8 RID: 15096 RVA: 0x003115C8 File Offset: 0x0030F7C8
		public DashboardMultiPidSelector(Action<List<IPID>> callback, List<int> alreadySelectedPids)
		{
			this.InitializeComponent();
			IEnumerable<PID> enumerable = LiveDataPIDModel._PIDCollection.Where((PID x) => x is IPIDFloatValue);
			this.Model = new MultiPIDSelectorModel(enumerable, alreadySelectedPids);
			this.callback = callback;
			this.lvAvailablePids.BindingContext = this.Model;
			base.ToolbarItems.Add(new ToolbarItem("", "icons8_checked_checkbox", delegate
			{
				foreach (ProxyPidForSelector proxyPidForSelector in this.Model.FilteredPidList)
				{
					proxyPidForSelector.IsSelected = true;
				}
				this.UpdateBtnOK();
			}, 0, 0));
			base.ToolbarItems.Add(new ToolbarItem("", "icons8_unchecked_checkbox", delegate
			{
				foreach (ProxyPidForSelector proxyPidForSelector2 in this.Model.FilteredPidList)
				{
					proxyPidForSelector2.IsSelected = false;
				}
				this.UpdateBtnOK();
			}, 0, 0));
			this.UpdateBtnOK();
			new LabelSwitch();
		}

		// Token: 0x06003AF9 RID: 15097 RVA: 0x00311697 File Offset: 0x0030F897
		private void LabelSwitch_Toggled(object sender, ToggledEventArgs e)
		{
			this.UpdateBtnOK();
		}

		// Token: 0x06003AFA RID: 15098 RVA: 0x00311697 File Offset: 0x0030F897
		private void LvAvailablePids_ItemTapped(object sender, ItemTappedEventArgs e)
		{
			this.UpdateBtnOK();
		}

		// Token: 0x17001398 RID: 5016
		// (get) Token: 0x06003AFB RID: 15099 RVA: 0x0031169F File Offset: 0x0030F89F
		// (set) Token: 0x06003AFC RID: 15100 RVA: 0x003116A7 File Offset: 0x0030F8A7
		internal MultiPIDSelectorModel Model
		{
			[CompilerGenerated]
			get
			{
				return this.<Model>k__BackingField;
			}
			[CompilerGenerated]
			private set
			{
				this.<Model>k__BackingField = value;
			}
		}

		// Token: 0x06003AFD RID: 15101 RVA: 0x003116B0 File Offset: 0x0030F8B0
		private async void entrySearch_TextChanged(object sender, TextChangedEventArgs e)
		{
			this.filtertext = this.entrySearch.Text;
			await Task.Delay(500);
			if (this.filtertext == this.entrySearch.Text)
			{
				try
				{
					this.Model.Filter = this.filtertext;
				}
				catch
				{
				}
			}
		}

		// Token: 0x06003AFE RID: 15102 RVA: 0x00022295 File Offset: 0x00020495
		private void EntrySearch_Completed(object sender, EventArgs e)
		{
			PlatformHelper.CommonService.HideKeyboard();
		}

		// Token: 0x06003AFF RID: 15103 RVA: 0x003116E8 File Offset: 0x0030F8E8
		private async void LvAvailablePids_ItemSelected(object sender, SelectedItemChangedEventArgs e)
		{
			if (this.lvAvailablePids.SelectedItem != null)
			{
				PlatformHelper.CommonService.HideKeyboard();
				ProxyPidForSelector proxyPidForSelector = this.lvAvailablePids.SelectedItem as ProxyPidForSelector;
				this.lvAvailablePids.SelectedItem = null;
				proxyPidForSelector.IsSelected = !proxyPidForSelector.IsSelected;
			}
			this.UpdateBtnOK();
		}

		// Token: 0x06003B00 RID: 15104 RVA: 0x00311720 File Offset: 0x0030F920
		private async void btnOK_Clicked(object sender, EventArgs e)
		{
			this.btnOK.IsEnabled = false;
			Action<List<IPID>> action = this.callback;
			if (action != null)
			{
				action(this.Model.GetSelected());
			}
			await base.Navigation.PopAsync();
			this.btnOK.IsEnabled = true;
		}

		// Token: 0x06003B01 RID: 15105 RVA: 0x00311758 File Offset: 0x0030F958
		private void UpdateBtnOK()
		{
			if (this.Model.GetSelected().Count == 0)
			{
				this.btnOK.IsEnabled = false;
				this.btnOK.TextColor = Color.White;
				this.btnOK.BackgroundColor = Color.DarkGray;
				return;
			}
			this.btnOK.IsEnabled = true;
			this.btnOK.TextColor = Color.White;
			this.btnOK.BackgroundColor = Color.Green;
		}

		// Token: 0x06003B02 RID: 15106 RVA: 0x003117D0 File Offset: 0x0030F9D0
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private void InitializeComponent()
		{
			if (ResourceLoader.IsEnabled && ResourceLoader.CanProvideContentFor(new ResourceLoader.ResourceLoadingQuery
			{
				AssemblyName = typeof(DashboardMultiPidSelector).GetTypeInfo().Assembly.GetName(),
				ResourcePath = "Pages/Dashboard/DashboardMultiPidSelector.xaml",
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
			VisualDiagnostics.RegisterSourceInfo(translate = new Translate(), new Uri("Pages\\Dashboard\\DashboardMultiPidSelector.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 10, 5);
			DynamicResourceExtension dynamicResourceExtension;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension = new DynamicResourceExtension(), new Uri("Pages\\Dashboard\\DashboardMultiPidSelector.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 14, 5);
			BoolToNegativeConverter boolToNegativeConverter;
			VisualDiagnostics.RegisterSourceInfo(boolToNegativeConverter = new BoolToNegativeConverter(), new Uri("Pages\\Dashboard\\DashboardMultiPidSelector.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 19, 14);
			ResourceDictionary resourceDictionary;
			VisualDiagnostics.RegisterSourceInfo(resourceDictionary = new ResourceDictionary(), new Uri("Pages\\Dashboard\\DashboardMultiPidSelector.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 18, 10);
			RowDefinition rowDefinition;
			VisualDiagnostics.RegisterSourceInfo(rowDefinition = new RowDefinition(), new Uri("Pages\\Dashboard\\DashboardMultiPidSelector.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 25, 18);
			RowDefinition rowDefinition2;
			VisualDiagnostics.RegisterSourceInfo(rowDefinition2 = new RowDefinition(), new Uri("Pages\\Dashboard\\DashboardMultiPidSelector.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 26, 18);
			RowDefinition rowDefinition3;
			VisualDiagnostics.RegisterSourceInfo(rowDefinition3 = new RowDefinition(), new Uri("Pages\\Dashboard\\DashboardMultiPidSelector.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 27, 18);
			Entry entry;
			VisualDiagnostics.RegisterSourceInfo(entry = new Entry(), new Uri("Pages\\Dashboard\\DashboardMultiPidSelector.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 37, 18);
			Grid grid;
			VisualDiagnostics.RegisterSourceInfo(grid = new Grid(), new Uri("Pages\\Dashboard\\DashboardMultiPidSelector.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 29, 14);
			BindingExtension bindingExtension;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension = new BindingExtension(), new Uri("Pages\\Dashboard\\DashboardMultiPidSelector.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 52, 17);
			DataTemplate dataTemplate;
			VisualDiagnostics.RegisterSourceInfo(dataTemplate = new DataTemplate(), new Uri("Pages\\Dashboard\\DashboardMultiPidSelector.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 54, 22);
			ListView listView;
			VisualDiagnostics.RegisterSourceInfo(listView = new ListView(), new Uri("Pages\\Dashboard\\DashboardMultiPidSelector.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 47, 14);
			Button button;
			VisualDiagnostics.RegisterSourceInfo(button = new Button(), new Uri("Pages\\Dashboard\\DashboardMultiPidSelector.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 64, 14);
			Grid grid2;
			VisualDiagnostics.RegisterSourceInfo(grid2 = new Grid(), new Uri("Pages\\Dashboard\\DashboardMultiPidSelector.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 23, 10);
			VisualDiagnostics.RegisterSourceInfo(this, new Uri("Pages\\Dashboard\\DashboardMultiPidSelector.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 2, 2);
			NameScope nameScope = NameScope.GetNameScope(this) ?? new NameScope();
			NameScope.SetNameScope(this, nameScope);
			nameScope.RegisterName("panelStep2", grid2);
			if (grid2.StyleId == null)
			{
				grid2.StyleId = "panelStep2";
			}
			nameScope.RegisterName("entrySearch", entry);
			if (entry.StyleId == null)
			{
				entry.StyleId = "entrySearch";
			}
			nameScope.RegisterName("lvAvailablePids", listView);
			if (listView.StyleId == null)
			{
				listView.StyleId = "lvAvailablePids";
			}
			nameScope.RegisterName("btnOK", button);
			if (button.StyleId == null)
			{
				button.StyleId = "btnOK";
			}
			this.panelStep2 = grid2;
			this.entrySearch = entry;
			this.lvAvailablePids = listView;
			this.btnOK = button;
			this.Resources = resourceDictionary;
			resourceDictionary.Add("BoolToNegativeConverter", boolToNegativeConverter);
			translate.Text = "dash_SelectMultiple";
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
			xmlNamespaceResolver.Add("syncfusion", "clr-namespace:Syncfusion.ListView.XForms;assembly=Syncfusion.SfListView.XForms");
			xmlNamespaceResolver.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider.Add(typeFromHandle2, new XamlTypeResolver(xmlNamespaceResolver, typeof(DashboardMultiPidSelector).GetTypeInfo().Assembly));
			xamlServiceProvider.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(10, 5)));
			object obj2 = markupExtension.ProvideValue(xamlServiceProvider);
			this.Title = obj2;
			this.SetValue(Page.PaddingProperty, new Thickness(5.0, 20.0, 5.0, 5.0));
			this.SetValue(Page.PrefersStatusBarHiddenProperty, 0);
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
			xmlNamespaceResolver2.Add("syncfusion", "clr-namespace:Syncfusion.ListView.XForms;assembly=Syncfusion.SfListView.XForms");
			xmlNamespaceResolver2.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider2.Add(typeFromHandle4, new XamlTypeResolver(xmlNamespaceResolver2, typeof(DashboardMultiPidSelector).GetTypeInfo().Assembly));
			xamlServiceProvider2.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(14, 5)));
			DynamicResource dynamicResource = markupExtension2.ProvideValue(xamlServiceProvider2);
			this.SetDynamicResource(VisualElement.BackgroundColorProperty, dynamicResource.Key);
			this.SetValue(NavigationPage.HasBackButtonProperty, true);
			this.SetValue(NavigationPage.HasNavigationBarProperty, true);
			this.Resources = resourceDictionary;
			rowDefinition.SetValue(RowDefinition.HeightProperty, new GridLengthTypeConverter().ConvertFromInvariantString("auto"));
			grid2.GetValue(Grid.RowDefinitionsProperty).Add(rowDefinition);
			rowDefinition2.SetValue(RowDefinition.HeightProperty, new GridLengthTypeConverter().ConvertFromInvariantString("*"));
			grid2.GetValue(Grid.RowDefinitionsProperty).Add(rowDefinition2);
			rowDefinition3.SetValue(RowDefinition.HeightProperty, new GridLengthTypeConverter().ConvertFromInvariantString("auto"));
			grid2.GetValue(Grid.RowDefinitionsProperty).Add(rowDefinition3);
			grid.SetValue(Grid.RowProperty, 0);
			entry.SetValue(Grid.RowProperty, 0);
			entry.Completed += this.EntrySearch_Completed;
			entry.SetValue(InputView.IsSpellCheckEnabledProperty, false);
			entry.SetValue(Entry.IsTextPredictionEnabledProperty, false);
			entry.SetValue(Entry.PlaceholderProperty, "\ud83d\udd0e");
			entry.TextChanged += this.entrySearch_TextChanged;
			entry.SetValue(View.VerticalOptionsProperty, LayoutOptions.Center);
			grid.Children.Add(entry);
			grid2.Children.Add(grid);
			listView.SetValue(Grid.RowProperty, 1);
			listView.SetValue(ListView.HasUnevenRowsProperty, true);
			listView.ItemSelected += this.LvAvailablePids_ItemSelected;
			bindingExtension.Path = "FilteredPidList";
			BindingBase bindingBase = bindingExtension.ProvideValue(null);
			listView.SetBinding(ItemsView<Cell>.ItemsSourceProperty, bindingBase);
			IDataTemplate dataTemplate2 = dataTemplate;
			DashboardMultiPidSelector.<InitializeComponent>_anonXamlCDataTemplate_39 <InitializeComponent>_anonXamlCDataTemplate_ = new DashboardMultiPidSelector.<InitializeComponent>_anonXamlCDataTemplate_39();
			object[] array3 = new object[0 + 4];
			array3[0] = dataTemplate;
			array3[1] = listView;
			array3[2] = grid2;
			array3[3] = this;
			<InitializeComponent>_anonXamlCDataTemplate_.parentValues = array3;
			<InitializeComponent>_anonXamlCDataTemplate_.root = this;
			dataTemplate2.LoadTemplate = new Func<object>(<InitializeComponent>_anonXamlCDataTemplate_.LoadDataTemplate);
			listView.SetValue(ItemsView<Cell>.ItemTemplateProperty, dataTemplate);
			grid2.Children.Add(listView);
			button.SetValue(Grid.RowProperty, 2);
			button.Clicked += this.btnOK_Clicked;
			button.SetValue(Button.TextProperty, "OK");
			grid2.Children.Add(button);
			this.SetValue(ContentPage.ContentProperty, grid2);
		}

		// Token: 0x06003B03 RID: 15107 RVA: 0x00312094 File Offset: 0x00310294
		[CompilerGenerated]
		private void <.ctor>b__0_1()
		{
			foreach (ProxyPidForSelector proxyPidForSelector in this.Model.FilteredPidList)
			{
				proxyPidForSelector.IsSelected = true;
			}
			this.UpdateBtnOK();
		}

		// Token: 0x06003B04 RID: 15108 RVA: 0x003120EC File Offset: 0x003102EC
		[CompilerGenerated]
		private void <.ctor>b__0_2()
		{
			foreach (ProxyPidForSelector proxyPidForSelector in this.Model.FilteredPidList)
			{
				proxyPidForSelector.IsSelected = false;
			}
			this.UpdateBtnOK();
		}

		// Token: 0x06003B05 RID: 15109 RVA: 0x00312144 File Offset: 0x00310344
		private void __InitComponentRuntime()
		{
			Extensions.LoadFromXaml<DashboardMultiPidSelector>(this, typeof(DashboardMultiPidSelector));
			this.panelStep2 = NameScopeExtensions.FindByName<Grid>(this, "panelStep2");
			this.entrySearch = NameScopeExtensions.FindByName<Entry>(this, "entrySearch");
			this.lvAvailablePids = NameScopeExtensions.FindByName<ListView>(this, "lvAvailablePids");
			this.btnOK = NameScopeExtensions.FindByName<Button>(this, "btnOK");
		}

		// Token: 0x04002414 RID: 9236
		[CompilerGenerated]
		private MultiPIDSelectorModel <Model>k__BackingField;

		// Token: 0x04002415 RID: 9237
		private Action<List<IPID>> callback;

		// Token: 0x04002416 RID: 9238
		private string filtertext = "";

		// Token: 0x04002417 RID: 9239
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private Grid panelStep2;

		// Token: 0x04002418 RID: 9240
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private Entry entrySearch;

		// Token: 0x04002419 RID: 9241
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private ListView lvAvailablePids;

		// Token: 0x0400241A RID: 9242
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private Button btnOK;

		// Token: 0x020006BF RID: 1727
		[CompilerGenerated]
		[Serializable]
		private sealed class <>c
		{
			// Token: 0x06003B06 RID: 15110 RVA: 0x003121A6 File Offset: 0x003103A6
			// Note: this type is marked as 'beforefieldinit'.
			static <>c()
			{
			}

			// Token: 0x06003B07 RID: 15111 RVA: 0x00002050 File Offset: 0x00000250
			public <>c()
			{
			}

			// Token: 0x06003B08 RID: 15112 RVA: 0x00066340 File Offset: 0x00064540
			internal bool <.ctor>b__0_0(PID x)
			{
				return x is IPIDFloatValue;
			}

			// Token: 0x0400241B RID: 9243
			public static readonly DashboardMultiPidSelector.<>c <>9 = new DashboardMultiPidSelector.<>c();

			// Token: 0x0400241C RID: 9244
			public static Func<PID, bool> <>9__0_0;
		}

		// Token: 0x020006C0 RID: 1728
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <LvAvailablePids_ItemSelected>d__11 : IAsyncStateMachine
		{
			// Token: 0x06003B09 RID: 15113 RVA: 0x003121B4 File Offset: 0x003103B4
			void IAsyncStateMachine.MoveNext()
			{
				DashboardMultiPidSelector dashboardMultiPidSelector = this;
				try
				{
					if (dashboardMultiPidSelector.lvAvailablePids.SelectedItem != null)
					{
						PlatformHelper.CommonService.HideKeyboard();
						ProxyPidForSelector proxyPidForSelector = dashboardMultiPidSelector.lvAvailablePids.SelectedItem as ProxyPidForSelector;
						dashboardMultiPidSelector.lvAvailablePids.SelectedItem = null;
						proxyPidForSelector.IsSelected = !proxyPidForSelector.IsSelected;
					}
					dashboardMultiPidSelector.UpdateBtnOK();
				}
				catch (Exception ex)
				{
					this.<>1__state = -2;
					this.<>t__builder.SetException(ex);
					return;
				}
				this.<>1__state = -2;
				this.<>t__builder.SetResult();
			}

			// Token: 0x06003B0A RID: 15114 RVA: 0x00312250 File Offset: 0x00310450
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x0400241D RID: 9245
			public int <>1__state;

			// Token: 0x0400241E RID: 9246
			public AsyncVoidMethodBuilder <>t__builder;

			// Token: 0x0400241F RID: 9247
			public DashboardMultiPidSelector <>4__this;
		}

		// Token: 0x020006C1 RID: 1729
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <btnOK_Clicked>d__12 : IAsyncStateMachine
		{
			// Token: 0x06003B0B RID: 15115 RVA: 0x00312260 File Offset: 0x00310460
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				DashboardMultiPidSelector dashboardMultiPidSelector = this;
				try
				{
					TaskAwaiter<Page> taskAwaiter;
					if (num != 0)
					{
						dashboardMultiPidSelector.btnOK.IsEnabled = false;
						Action<List<IPID>> callback = dashboardMultiPidSelector.callback;
						if (callback != null)
						{
							callback(dashboardMultiPidSelector.Model.GetSelected());
						}
						taskAwaiter = dashboardMultiPidSelector.Navigation.PopAsync().GetAwaiter();
						if (!taskAwaiter.IsCompleted)
						{
							num2 = 0;
							TaskAwaiter<Page> taskAwaiter2 = taskAwaiter;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<Page>, DashboardMultiPidSelector.<btnOK_Clicked>d__12>(ref taskAwaiter, ref this);
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
					dashboardMultiPidSelector.btnOK.IsEnabled = true;
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

			// Token: 0x06003B0C RID: 15116 RVA: 0x0031234C File Offset: 0x0031054C
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x04002420 RID: 9248
			public int <>1__state;

			// Token: 0x04002421 RID: 9249
			public AsyncVoidMethodBuilder <>t__builder;

			// Token: 0x04002422 RID: 9250
			public DashboardMultiPidSelector <>4__this;

			// Token: 0x04002423 RID: 9251
			private TaskAwaiter<Page> <>u__1;
		}

		// Token: 0x020006C2 RID: 1730
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <entrySearch_TextChanged>d__9 : IAsyncStateMachine
		{
			// Token: 0x06003B0D RID: 15117 RVA: 0x0031235C File Offset: 0x0031055C
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				DashboardMultiPidSelector dashboardMultiPidSelector = this;
				try
				{
					TaskAwaiter taskAwaiter;
					if (num != 0)
					{
						dashboardMultiPidSelector.filtertext = dashboardMultiPidSelector.entrySearch.Text;
						taskAwaiter = Task.Delay(500).GetAwaiter();
						if (!taskAwaiter.IsCompleted)
						{
							num2 = 0;
							TaskAwaiter taskAwaiter2 = taskAwaiter;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, DashboardMultiPidSelector.<entrySearch_TextChanged>d__9>(ref taskAwaiter, ref this);
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
					if (dashboardMultiPidSelector.filtertext == dashboardMultiPidSelector.entrySearch.Text)
					{
						try
						{
							dashboardMultiPidSelector.Model.Filter = dashboardMultiPidSelector.filtertext;
						}
						catch
						{
						}
					}
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

			// Token: 0x06003B0E RID: 15118 RVA: 0x00312460 File Offset: 0x00310660
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x04002424 RID: 9252
			public int <>1__state;

			// Token: 0x04002425 RID: 9253
			public AsyncVoidMethodBuilder <>t__builder;

			// Token: 0x04002426 RID: 9254
			public DashboardMultiPidSelector <>4__this;

			// Token: 0x04002427 RID: 9255
			private TaskAwaiter <>u__1;
		}

		// Token: 0x020006C3 RID: 1731
		[CompilerGenerated]
		private sealed class <InitializeComponent>_anonXamlCDataTemplate_39
		{
			// Token: 0x06003B0F RID: 15119 RVA: 0x00312470 File Offset: 0x00310670
			public <InitializeComponent>_anonXamlCDataTemplate_39()
			{
			}

			// Token: 0x06003B10 RID: 15120 RVA: 0x00312484 File Offset: 0x00310684
			internal object LoadDataTemplate()
			{
				BindingExtension bindingExtension;
				VisualDiagnostics.RegisterSourceInfo(bindingExtension = new BindingExtension(), new Uri("Pages\\Dashboard\\DashboardMultiPidSelector.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 57, 33);
				BindingExtension bindingExtension2;
				VisualDiagnostics.RegisterSourceInfo(bindingExtension2 = new BindingExtension(), new Uri("Pages\\Dashboard\\DashboardMultiPidSelector.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 58, 33);
				LabelSwitch labelSwitch;
				VisualDiagnostics.RegisterSourceInfo(labelSwitch = new LabelSwitch(), new Uri("Pages\\Dashboard\\DashboardMultiPidSelector.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 56, 30);
				ViewCell viewCell;
				VisualDiagnostics.RegisterSourceInfo(viewCell = new ViewCell(), new Uri("Pages\\Dashboard\\DashboardMultiPidSelector.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 55, 26);
				NameScope nameScope = new NameScope();
				NameScope.SetNameScope(viewCell, nameScope);
				bindingExtension.Mode = 1;
				bindingExtension.Path = "IsSelected";
				BindingBase bindingBase = bindingExtension.ProvideValue(null);
				labelSwitch.SetBinding(LabelSwitch.IsToggledProperty, bindingBase);
				bindingExtension2.Path = "Pid.Name";
				BindingBase bindingBase2 = bindingExtension2.ProvideValue(null);
				labelSwitch.SetBinding(LabelSwitch.TextProperty, bindingBase2);
				labelSwitch.Toggled += this.root.LabelSwitch_Toggled;
				viewCell.View = labelSwitch;
				return viewCell;
			}

			// Token: 0x04002428 RID: 9256
			internal object[] parentValues;

			// Token: 0x04002429 RID: 9257
			internal DashboardMultiPidSelector root;
		}
	}
}
