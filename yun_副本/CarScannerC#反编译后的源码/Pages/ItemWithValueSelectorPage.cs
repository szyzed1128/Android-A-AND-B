using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using CarScannerXamarinForms.Common;
using Xamarin.Forms;
using Xamarin.Forms.Internals;
using Xamarin.Forms.PlatformConfiguration.iOSSpecific;
using Xamarin.Forms.Xaml;
using Xamarin.Forms.Xaml.Diagnostics;
using Xamarin.Forms.Xaml.Internals;

namespace CarScannerXamarinForms.Pages
{
	// Token: 0x02000629 RID: 1577
	[XamlFilePath("Pages\\ItemWithValueSelectorPage.xaml")]
	public class ItemWithValueSelectorPage : ContentPage
	{
		// Token: 0x06003721 RID: 14113 RVA: 0x00293554 File Offset: 0x00291754
		public ItemWithValueSelectorPage(string title, IEnumerable<ValueItemWithTranslation> items, ValueItemWithTranslation selected_item, Action<ValueItemWithTranslation> selected_callback)
		{
			this.items = items.ToList<ValueItemWithTranslation>();
			this.FilteredItems = new ObservableCollection<ValueItemWithTranslation>(items);
			this.selected_callback = selected_callback;
			this.InitializeComponent();
			base.Title = title;
			this.lv.ItemsSource = this.FilteredItems;
			this.lv.SelectedItem = selected_item;
			this.lv.ItemSelected += this.Lv_ItemSelected;
			this.lv.ScrollTo(selected_item, 0, false);
		}

		// Token: 0x06003722 RID: 14114 RVA: 0x002935D8 File Offset: 0x002917D8
		private async void Lv_ItemSelected(object sender, SelectedItemChangedEventArgs e)
		{
			if (this.lv.SelectedItem != null)
			{
				ValueItemWithTranslation item = (ValueItemWithTranslation)this.lv.SelectedItem;
				await base.Navigation.PopAsync();
				this.selected_callback(item);
			}
		}

		// Token: 0x17001382 RID: 4994
		// (get) Token: 0x06003723 RID: 14115 RVA: 0x0029360F File Offset: 0x0029180F
		// (set) Token: 0x06003724 RID: 14116 RVA: 0x00293617 File Offset: 0x00291817
		public ObservableCollection<ValueItemWithTranslation> FilteredItems
		{
			[CompilerGenerated]
			get
			{
				return this.<FilteredItems>k__BackingField;
			}
			[CompilerGenerated]
			private set
			{
				this.<FilteredItems>k__BackingField = value;
			}
		}

		// Token: 0x06003725 RID: 14117 RVA: 0x00293620 File Offset: 0x00291820
		private async void searchBar_TextChanged(object sender, TextChangedEventArgs e)
		{
			string filtertext = this.searchBar.Text;
			await Task.Delay(500);
			if (filtertext == this.searchBar.Text)
			{
				try
				{
					if (string.IsNullOrEmpty(filtertext))
					{
						this.SetFilteredCollection(this.items);
					}
					else
					{
						List<ValueItemWithTranslation> list = new List<ValueItemWithTranslation>(this.items);
						string[] array = filtertext.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
						for (int i = 0; i < array.Length; i++)
						{
							string word = array[i];
							list = list.Where((ValueItemWithTranslation x) => (!string.IsNullOrEmpty(x.Title) && x.Title.IndexOf(word, StringComparison.OrdinalIgnoreCase) >= 0) || (!string.IsNullOrEmpty(x.Description) && x.Description.IndexOf(word, StringComparison.OrdinalIgnoreCase) >= 0)).ToList<ValueItemWithTranslation>();
						}
						this.SetFilteredCollection(list);
					}
				}
				catch
				{
				}
			}
		}

		// Token: 0x06003726 RID: 14118 RVA: 0x00293658 File Offset: 0x00291858
		private void SetFilteredCollection(IEnumerable<ValueItemWithTranslation> input)
		{
			this.FilteredItems.Clear();
			foreach (ValueItemWithTranslation valueItemWithTranslation in input)
			{
				this.FilteredItems.Add(valueItemWithTranslation);
			}
		}

		// Token: 0x06003727 RID: 14119 RVA: 0x002936B0 File Offset: 0x002918B0
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private void InitializeComponent()
		{
			if (ResourceLoader.IsEnabled && ResourceLoader.CanProvideContentFor(new ResourceLoader.ResourceLoadingQuery
			{
				AssemblyName = typeof(ItemWithValueSelectorPage).GetTypeInfo().Assembly.GetName(),
				ResourcePath = "Pages/ItemWithValueSelectorPage.xaml",
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
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension = new DynamicResourceExtension(), new Uri("Pages\\ItemWithValueSelectorPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 9, 5);
			On on;
			VisualDiagnostics.RegisterSourceInfo(on = new On(), new Uri("Pages\\ItemWithValueSelectorPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 14, 14);
			On on2;
			VisualDiagnostics.RegisterSourceInfo(on2 = new On(), new Uri("Pages\\ItemWithValueSelectorPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 15, 14);
			OnPlatform<Thickness> onPlatform;
			VisualDiagnostics.RegisterSourceInfo(onPlatform = new OnPlatform<Thickness>(), new Uri("Pages\\ItemWithValueSelectorPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 13, 10);
			RowDefinition rowDefinition;
			VisualDiagnostics.RegisterSourceInfo(rowDefinition = new RowDefinition(), new Uri("Pages\\ItemWithValueSelectorPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 24, 18);
			RowDefinition rowDefinition2;
			VisualDiagnostics.RegisterSourceInfo(rowDefinition2 = new RowDefinition(), new Uri("Pages\\ItemWithValueSelectorPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 25, 18);
			RowDefinition rowDefinition3;
			VisualDiagnostics.RegisterSourceInfo(rowDefinition3 = new RowDefinition(), new Uri("Pages\\ItemWithValueSelectorPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 26, 18);
			RowDefinition rowDefinition4;
			VisualDiagnostics.RegisterSourceInfo(rowDefinition4 = new RowDefinition(), new Uri("Pages\\ItemWithValueSelectorPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 27, 18);
			ColumnDefinition columnDefinition;
			VisualDiagnostics.RegisterSourceInfo(columnDefinition = new ColumnDefinition(), new Uri("Pages\\ItemWithValueSelectorPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 30, 18);
			ColumnDefinition columnDefinition2;
			VisualDiagnostics.RegisterSourceInfo(columnDefinition2 = new ColumnDefinition(), new Uri("Pages\\ItemWithValueSelectorPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 31, 18);
			Entry entry;
			VisualDiagnostics.RegisterSourceInfo(entry = new Entry(), new Uri("Pages\\ItemWithValueSelectorPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 35, 14);
			DataTemplate dataTemplate;
			VisualDiagnostics.RegisterSourceInfo(dataTemplate = new DataTemplate(), new Uri("Pages\\ItemWithValueSelectorPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 53, 22);
			ListView listView;
			VisualDiagnostics.RegisterSourceInfo(listView = new ListView(), new Uri("Pages\\ItemWithValueSelectorPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 44, 14);
			Grid grid;
			VisualDiagnostics.RegisterSourceInfo(grid = new Grid(), new Uri("Pages\\ItemWithValueSelectorPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 22, 10);
			VisualDiagnostics.RegisterSourceInfo(this, new Uri("Pages\\ItemWithValueSelectorPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 1, 2);
			NameScope nameScope = NameScope.GetNameScope(this) ?? new NameScope();
			NameScope.SetNameScope(this, nameScope);
			nameScope.RegisterName("searchBar", entry);
			if (entry.StyleId == null)
			{
				entry.StyleId = "searchBar";
			}
			nameScope.RegisterName("lv", listView);
			if (listView.StyleId == null)
			{
				listView.StyleId = "lv";
			}
			this.searchBar = entry;
			this.lv = listView;
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
			xamlServiceProvider.Add(typeFromHandle2, new XamlTypeResolver(xmlNamespaceResolver, typeof(ItemWithValueSelectorPage).GetTypeInfo().Assembly));
			xamlServiceProvider.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(9, 5)));
			DynamicResource dynamicResource = markupExtension.ProvideValue(xamlServiceProvider);
			this.SetDynamicResource(VisualElement.BackgroundColorProperty, dynamicResource.Key);
			this.SetValue(NavigationPage.HasBackButtonProperty, true);
			this.SetValue(NavigationPage.HasNavigationBarProperty, true);
			on.Platform = new List<string>(1) { "Android" };
			on.Value = "0";
			onPlatform.Platforms.Add(on);
			on2.Platform = new List<string>(1) { "iOS" };
			on2.Value = "0,20,0,0";
			onPlatform.Platforms.Add(on2);
			this.SetValue(Page.PaddingProperty, onPlatform);
			rowDefinition.SetValue(RowDefinition.HeightProperty, new GridLengthTypeConverter().ConvertFromInvariantString("auto"));
			grid.GetValue(Grid.RowDefinitionsProperty).Add(rowDefinition);
			rowDefinition2.SetValue(RowDefinition.HeightProperty, new GridLengthTypeConverter().ConvertFromInvariantString("auto"));
			grid.GetValue(Grid.RowDefinitionsProperty).Add(rowDefinition2);
			rowDefinition3.SetValue(RowDefinition.HeightProperty, new GridLengthTypeConverter().ConvertFromInvariantString("*"));
			grid.GetValue(Grid.RowDefinitionsProperty).Add(rowDefinition3);
			rowDefinition4.SetValue(RowDefinition.HeightProperty, new GridLengthTypeConverter().ConvertFromInvariantString("auto"));
			grid.GetValue(Grid.RowDefinitionsProperty).Add(rowDefinition4);
			columnDefinition.SetValue(ColumnDefinition.WidthProperty, new GridLengthTypeConverter().ConvertFromInvariantString("*"));
			grid.GetValue(Grid.ColumnDefinitionsProperty).Add(columnDefinition);
			columnDefinition2.SetValue(ColumnDefinition.WidthProperty, new GridLengthTypeConverter().ConvertFromInvariantString("*"));
			grid.GetValue(Grid.ColumnDefinitionsProperty).Add(columnDefinition2);
			entry.SetValue(Grid.RowProperty, 1);
			entry.SetValue(Grid.ColumnProperty, 0);
			entry.SetValue(Grid.ColumnSpanProperty, 2);
			entry.SetValue(InputView.IsSpellCheckEnabledProperty, false);
			entry.SetValue(Entry.PlaceholderProperty, "\ud83d\udd0e");
			entry.TextChanged += this.searchBar_TextChanged;
			grid.Children.Add(entry);
			listView.SetValue(Grid.RowProperty, 2);
			listView.SetValue(Grid.ColumnProperty, 0);
			listView.SetValue(Grid.ColumnSpanProperty, 2);
			listView.SetValue(View.MarginProperty, new Thickness(5.0, 0.0, 5.0, 0.0));
			listView.SetValue(VisualElement.BackgroundColorProperty, Color.Transparent);
			listView.SetValue(ListView.HasUnevenRowsProperty, true);
			IDataTemplate dataTemplate2 = dataTemplate;
			ItemWithValueSelectorPage.<InitializeComponent>_anonXamlCDataTemplate_57 <InitializeComponent>_anonXamlCDataTemplate_ = new ItemWithValueSelectorPage.<InitializeComponent>_anonXamlCDataTemplate_57();
			object[] array2 = new object[0 + 4];
			array2[0] = dataTemplate;
			array2[1] = listView;
			array2[2] = grid;
			array2[3] = this;
			<InitializeComponent>_anonXamlCDataTemplate_.parentValues = array2;
			<InitializeComponent>_anonXamlCDataTemplate_.root = this;
			dataTemplate2.LoadTemplate = new Func<object>(<InitializeComponent>_anonXamlCDataTemplate_.LoadDataTemplate);
			listView.SetValue(ItemsView<Cell>.ItemTemplateProperty, dataTemplate);
			grid.Children.Add(listView);
			this.SetValue(ContentPage.ContentProperty, grid);
		}

		// Token: 0x06003728 RID: 14120 RVA: 0x00293E59 File Offset: 0x00292059
		private void __InitComponentRuntime()
		{
			Extensions.LoadFromXaml<ItemWithValueSelectorPage>(this, typeof(ItemWithValueSelectorPage));
			this.searchBar = NameScopeExtensions.FindByName<Entry>(this, "searchBar");
			this.lv = NameScopeExtensions.FindByName<ListView>(this, "lv");
		}

		// Token: 0x04002153 RID: 8531
		private Action<ValueItemWithTranslation> selected_callback;

		// Token: 0x04002154 RID: 8532
		private List<ValueItemWithTranslation> items;

		// Token: 0x04002155 RID: 8533
		[CompilerGenerated]
		private ObservableCollection<ValueItemWithTranslation> <FilteredItems>k__BackingField;

		// Token: 0x04002156 RID: 8534
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private Entry searchBar;

		// Token: 0x04002157 RID: 8535
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private ListView lv;

		// Token: 0x0200062A RID: 1578
		[CompilerGenerated]
		private sealed class <>c__DisplayClass8_0
		{
			// Token: 0x06003729 RID: 14121 RVA: 0x00002050 File Offset: 0x00000250
			public <>c__DisplayClass8_0()
			{
			}

			// Token: 0x0600372A RID: 14122 RVA: 0x00293E90 File Offset: 0x00292090
			internal bool <searchBar_TextChanged>b__0(ValueItemWithTranslation x)
			{
				return (!string.IsNullOrEmpty(x.Title) && x.Title.IndexOf(this.word, StringComparison.OrdinalIgnoreCase) >= 0) || (!string.IsNullOrEmpty(x.Description) && x.Description.IndexOf(this.word, StringComparison.OrdinalIgnoreCase) >= 0);
			}

			// Token: 0x04002158 RID: 8536
			public string word;
		}

		// Token: 0x0200062B RID: 1579
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <Lv_ItemSelected>d__1 : IAsyncStateMachine
		{
			// Token: 0x0600372B RID: 14123 RVA: 0x00293EE8 File Offset: 0x002920E8
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				ItemWithValueSelectorPage itemWithValueSelectorPage = this;
				try
				{
					TaskAwaiter<Page> taskAwaiter;
					if (num != 0)
					{
						if (itemWithValueSelectorPage.lv.SelectedItem == null)
						{
							goto IL_00C8;
						}
						item = (ValueItemWithTranslation)itemWithValueSelectorPage.lv.SelectedItem;
						taskAwaiter = itemWithValueSelectorPage.Navigation.PopAsync().GetAwaiter();
						if (!taskAwaiter.IsCompleted)
						{
							num2 = 0;
							TaskAwaiter<Page> taskAwaiter2 = taskAwaiter;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<Page>, ItemWithValueSelectorPage.<Lv_ItemSelected>d__1>(ref taskAwaiter, ref this);
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
					itemWithValueSelectorPage.selected_callback(item);
				}
				catch (Exception ex)
				{
					num2 = -2;
					item = null;
					this.<>t__builder.SetException(ex);
					return;
				}
				IL_00C8:
				num2 = -2;
				item = null;
				this.<>t__builder.SetResult();
			}

			// Token: 0x0600372C RID: 14124 RVA: 0x00293FE8 File Offset: 0x002921E8
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x04002159 RID: 8537
			public int <>1__state;

			// Token: 0x0400215A RID: 8538
			public AsyncVoidMethodBuilder <>t__builder;

			// Token: 0x0400215B RID: 8539
			public ItemWithValueSelectorPage <>4__this;

			// Token: 0x0400215C RID: 8540
			private ValueItemWithTranslation <item>5__2;

			// Token: 0x0400215D RID: 8541
			private TaskAwaiter<Page> <>u__1;
		}

		// Token: 0x0200062C RID: 1580
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <searchBar_TextChanged>d__8 : IAsyncStateMachine
		{
			// Token: 0x0600372D RID: 14125 RVA: 0x00293FF8 File Offset: 0x002921F8
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				ItemWithValueSelectorPage itemWithValueSelectorPage = this;
				try
				{
					TaskAwaiter taskAwaiter;
					if (num != 0)
					{
						filtertext = itemWithValueSelectorPage.searchBar.Text;
						taskAwaiter = Task.Delay(500).GetAwaiter();
						if (!taskAwaiter.IsCompleted)
						{
							num2 = 0;
							TaskAwaiter taskAwaiter2 = taskAwaiter;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, ItemWithValueSelectorPage.<searchBar_TextChanged>d__8>(ref taskAwaiter, ref this);
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
					if (filtertext == itemWithValueSelectorPage.searchBar.Text)
					{
						try
						{
							if (string.IsNullOrEmpty(filtertext))
							{
								itemWithValueSelectorPage.SetFilteredCollection(itemWithValueSelectorPage.items);
							}
							else
							{
								List<ValueItemWithTranslation> list = new List<ValueItemWithTranslation>(itemWithValueSelectorPage.items);
								string[] array = filtertext.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
								for (int i = 0; i < array.Length; i++)
								{
									list = list.Where(new Func<ValueItemWithTranslation, bool>(new ItemWithValueSelectorPage.<>c__DisplayClass8_0
									{
										word = array[i]
									}.<searchBar_TextChanged>b__0)).ToList<ValueItemWithTranslation>();
								}
								itemWithValueSelectorPage.SetFilteredCollection(list);
							}
						}
						catch
						{
						}
					}
				}
				catch (Exception ex)
				{
					num2 = -2;
					filtertext = null;
					this.<>t__builder.SetException(ex);
					return;
				}
				num2 = -2;
				filtertext = null;
				this.<>t__builder.SetResult();
			}

			// Token: 0x0600372E RID: 14126 RVA: 0x0029419C File Offset: 0x0029239C
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x0400215E RID: 8542
			public int <>1__state;

			// Token: 0x0400215F RID: 8543
			public AsyncVoidMethodBuilder <>t__builder;

			// Token: 0x04002160 RID: 8544
			public ItemWithValueSelectorPage <>4__this;

			// Token: 0x04002161 RID: 8545
			private string <filtertext>5__2;

			// Token: 0x04002162 RID: 8546
			private TaskAwaiter <>u__1;
		}

		// Token: 0x0200062D RID: 1581
		[CompilerGenerated]
		private sealed class <InitializeComponent>_anonXamlCDataTemplate_57
		{
			// Token: 0x0600372F RID: 14127 RVA: 0x002941AC File Offset: 0x002923AC
			public <InitializeComponent>_anonXamlCDataTemplate_57()
			{
			}

			// Token: 0x06003730 RID: 14128 RVA: 0x002941C0 File Offset: 0x002923C0
			internal object LoadDataTemplate()
			{
				BindingExtension bindingExtension;
				VisualDiagnostics.RegisterSourceInfo(bindingExtension = new BindingExtension(), new Uri("Pages\\ItemWithValueSelectorPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 59, 37);
				Label label;
				VisualDiagnostics.RegisterSourceInfo(label = new Label(), new Uri("Pages\\ItemWithValueSelectorPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 56, 34);
				DynamicResourceExtension dynamicResourceExtension;
				VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension = new DynamicResourceExtension(), new Uri("Pages\\ItemWithValueSelectorPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 63, 37);
				BindingExtension bindingExtension2;
				VisualDiagnostics.RegisterSourceInfo(bindingExtension2 = new BindingExtension(), new Uri("Pages\\ItemWithValueSelectorPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 65, 37);
				Label label2;
				VisualDiagnostics.RegisterSourceInfo(label2 = new Label(), new Uri("Pages\\ItemWithValueSelectorPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 60, 34);
				StackLayout stackLayout;
				VisualDiagnostics.RegisterSourceInfo(stackLayout = new StackLayout(), new Uri("Pages\\ItemWithValueSelectorPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 55, 30);
				ViewCell viewCell;
				VisualDiagnostics.RegisterSourceInfo(viewCell = new ViewCell(), new Uri("Pages\\ItemWithValueSelectorPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 54, 26);
				NameScope nameScope = new NameScope();
				NameScope.SetNameScope(viewCell, nameScope);
				stackLayout.SetValue(StackLayout.OrientationProperty, 0);
				label.SetValue(Label.FontAttributesProperty, new FontAttributesConverter().ConvertFromInvariantString("Bold"));
				label.SetValue(Label.LineBreakModeProperty, 1);
				bindingExtension.Path = "Title";
				BindingBase bindingBase = bindingExtension.ProvideValue(null);
				label.SetBinding(Label.TextProperty, bindingBase);
				stackLayout.Children.Add(label);
				label2.SetValue(View.MarginProperty, new Thickness(0.0, 0.0, 0.0, 4.0));
				label2.SetValue(Label.FontAttributesProperty, new FontAttributesConverter().ConvertFromInvariantString("Italic"));
				dynamicResourceExtension.Key = "BaseFontSize";
				IMarkupExtension<DynamicResource> markupExtension = dynamicResourceExtension;
				XamlServiceProvider xamlServiceProvider = new XamlServiceProvider();
				Type typeFromHandle = typeof(IProvideValueTarget);
				int num;
				object[] array = new object[(num = this.parentValues.Length) + 3];
				Array.Copy(this.parentValues, 0, array, 3, num);
				object[] array2 = array;
				array2[0] = label2;
				array2[1] = stackLayout;
				array2[2] = viewCell;
				object obj;
				xamlServiceProvider.Add(typeFromHandle, obj = new SimpleValueTargetProvider(array2, Label.FontSizeProperty, nameScope));
				xamlServiceProvider.Add(typeof(IReferenceProvider), obj);
				Type typeFromHandle2 = typeof(IXamlTypeResolver);
				XmlNamespaceResolver xmlNamespaceResolver = new XmlNamespaceResolver();
				xmlNamespaceResolver.Add("", "http://xamarin.com/schemas/2014/forms");
				xmlNamespaceResolver.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
				xmlNamespaceResolver.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
				xmlNamespaceResolver.Add("local", "clr-namespace:CarScannerXamarinForms");
				xamlServiceProvider.Add(typeFromHandle2, new XamlTypeResolver(xmlNamespaceResolver, typeof(ItemWithValueSelectorPage.<InitializeComponent>_anonXamlCDataTemplate_57).GetTypeInfo().Assembly));
				xamlServiceProvider.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(63, 37)));
				DynamicResource dynamicResource = markupExtension.ProvideValue(xamlServiceProvider);
				label2.SetDynamicResource(Label.FontSizeProperty, dynamicResource.Key);
				label2.SetValue(Label.LineBreakModeProperty, 1);
				bindingExtension2.Path = "Description";
				BindingBase bindingBase2 = bindingExtension2.ProvideValue(null);
				label2.SetBinding(Label.TextProperty, bindingBase2);
				stackLayout.Children.Add(label2);
				viewCell.View = stackLayout;
				return viewCell;
			}

			// Token: 0x04002163 RID: 8547
			internal object[] parentValues;

			// Token: 0x04002164 RID: 8548
			internal ItemWithValueSelectorPage root;
		}
	}
}
