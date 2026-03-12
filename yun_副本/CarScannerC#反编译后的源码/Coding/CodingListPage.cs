using System;
using System.CodeDom.Compiler;
using System.Diagnostics;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using CarScannerXamarinForms.Coding.Pages;
using CarScannerXamarinForms.Coding.PagesV2;
using Xamarin.Forms;
using Xamarin.Forms.Internals;
using Xamarin.Forms.PlatformConfiguration.iOSSpecific;
using Xamarin.Forms.Xaml;
using Xamarin.Forms.Xaml.Diagnostics;
using Xamarin.Forms.Xaml.Internals;

namespace CarScannerXamarinForms.Coding
{
	// Token: 0x02000900 RID: 2304
	[XamlCompilation(2)]
	[XamlFilePath("Coding\\Pages\\CodingListPage.xaml")]
	public class CodingListPage : ContentPage
	{
		// Token: 0x06004D62 RID: 19810 RVA: 0x0039850A File Offset: 0x0039670A
		public CodingListPage(CodingListModel model, string Title)
		{
			this.InitializeComponent();
			this.Model = model;
			base.Title = Title;
			this.lv.ItemsSource = this.Model.FilteredCollection;
		}

		// Token: 0x06004D63 RID: 19811 RVA: 0x00398548 File Offset: 0x00396748
		private async void CodingListPage_Appearing(object sender, EventArgs e)
		{
		}

		// Token: 0x06004D64 RID: 19812 RVA: 0x00398578 File Offset: 0x00396778
		private void Lv_ItemSelected(object sender, SelectedItemChangedEventArgs e)
		{
			if (e.SelectedItem != null)
			{
				base.IsEnabled = false;
				ICodingContainer codingContainer = (ICodingContainer)e.SelectedItem;
				this.lv.SelectedItem = null;
				if (codingContainer.ValueType == AdaptationValueTypes.OptionType)
				{
					if (codingContainer is IServiceProcedure)
					{
						CodingServiceProcedurePage codingServiceProcedurePage = new CodingServiceProcedurePage(codingContainer);
						base.Navigation.PushAsync(codingServiceProcedurePage);
					}
					else
					{
						CodingWithOptionsDetailsPage codingWithOptionsDetailsPage = new CodingWithOptionsDetailsPage(codingContainer);
						base.Navigation.PushAsync(codingWithOptionsDetailsPage);
					}
				}
				else if (codingContainer.ValueType == AdaptationValueTypes.InputValueType)
				{
					CodingWithInputDetailsPage codingWithInputDetailsPage = new CodingWithInputDetailsPage(codingContainer);
					base.Navigation.PushAsync(codingWithInputDetailsPage);
				}
				else if (codingContainer.ValueType == AdaptationValueTypes.MQBLightConfiguration)
				{
					CodingLightConfigurationPage codingLightConfigurationPage = new CodingLightConfigurationPage(codingContainer);
					base.Navigation.PushAsync(codingLightConfigurationPage);
				}
				else if (codingContainer.ValueType == AdaptationValueTypes.InputHexDataType)
				{
					if (codingContainer is CustomizableCodingTemplate)
					{
						CodingWithHexInput codingWithHexInput = new CodingWithHexInput(codingContainer);
						base.Navigation.PushAsync(codingWithHexInput);
					}
					else
					{
						LongCodingPage longCodingPage = new LongCodingPage(codingContainer);
						base.Navigation.PushAsync(longCodingPage);
					}
				}
				else if (codingContainer.ValueType == AdaptationValueTypes.MQBColorList)
				{
					CodingWithColorSelectionPage codingWithColorSelectionPage = new CodingWithColorSelectionPage(codingContainer);
					base.Navigation.PushAsync(codingWithColorSelectionPage);
				}
				else if (codingContainer.ValueType == AdaptationValueTypes.MQBParametrizeDump)
				{
					DataSetDumperPage dataSetDumperPage = new DataSetDumperPage((MQBParametrizeBase)codingContainer);
					base.Navigation.PushAsync(dataSetDumperPage);
				}
				else if (codingContainer.ValueType == AdaptationValueTypes.InputTextType)
				{
					CodingWithTextInput codingWithTextInput = new CodingWithTextInput(codingContainer);
					base.Navigation.PushAsync(codingWithTextInput);
				}
				else if (codingContainer.ValueType == AdaptationValueTypes.MQBFPAEditor)
				{
					FPAProfileCodingPage fpaprofileCodingPage = new FPAProfileCodingPage(codingContainer);
					base.Navigation.PushAsync(fpaprofileCodingPage);
				}
				else if (codingContainer.ValueType == AdaptationValueTypes.ToyotaTPMSSensor)
				{
					ToyotaTPMS2APage toyotaTPMS2APage = new ToyotaTPMS2APage(codingContainer);
					base.Navigation.PushAsync(toyotaTPMS2APage);
				}
				else if (codingContainer.ValueType == AdaptationValueTypes.MQBRKDSGenerator)
				{
					VagRKDSPage vagRKDSPage = new VagRKDSPage(codingContainer);
					base.Navigation.PushAsync(vagRKDSPage);
				}
				else if (codingContainer.ValueType == AdaptationValueTypes.MQBA5Customization)
				{
					CodingA5CustomizationPage codingA5CustomizationPage = new CodingA5CustomizationPage(codingContainer);
					base.Navigation.PushAsync(codingA5CustomizationPage);
				}
				else if (codingContainer.ValueType == AdaptationValueTypes.InputFloatIEEE754)
				{
					UniversalCodingPageV2 universalCodingPageV = new UniversalCodingPageV2(codingContainer);
					base.Navigation.PushAsync(universalCodingPageV);
				}
				else if (codingContainer.ValueType == AdaptationValueTypes.TailgateCustomization)
				{
					MQBTailGatePageCustomization mqbtailGatePageCustomization = new MQBTailGatePageCustomization(codingContainer);
					base.Navigation.PushAsync(mqbtailGatePageCustomization);
				}
				else if (codingContainer.ValueType == AdaptationValueTypes.TPMS)
				{
					TPMS4WheelsPage tpms4WheelsPage = new TPMS4WheelsPage(codingContainer);
					base.Navigation.PushAsync(tpms4WheelsPage);
				}
				base.IsEnabled = true;
			}
		}

		// Token: 0x06004D65 RID: 19813 RVA: 0x003987EC File Offset: 0x003969EC
		private async void searchBar_TextChanged(object sender, TextChangedEventArgs e)
		{
			this.filtertext = this.searchBar.Text;
			await Task.Delay(500);
			if (this.filtertext == this.searchBar.Text)
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

		// Token: 0x06004D66 RID: 19814 RVA: 0x00398824 File Offset: 0x00396A24
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private void InitializeComponent()
		{
			if (ResourceLoader.IsEnabled && ResourceLoader.CanProvideContentFor(new ResourceLoader.ResourceLoadingQuery
			{
				AssemblyName = typeof(CodingListPage).GetTypeInfo().Assembly.GetName(),
				ResourcePath = "Coding/Pages/CodingListPage.xaml",
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
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension = new DynamicResourceExtension(), new Uri("Coding\\Pages\\CodingListPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 12, 5);
			BoolToNegativeConverter boolToNegativeConverter;
			VisualDiagnostics.RegisterSourceInfo(boolToNegativeConverter = new BoolToNegativeConverter(), new Uri("Coding\\Pages\\CodingListPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 15, 14);
			ResourceDictionary resourceDictionary;
			VisualDiagnostics.RegisterSourceInfo(resourceDictionary = new ResourceDictionary(), new Uri("Coding\\Pages\\CodingListPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 14, 10);
			RowDefinition rowDefinition;
			VisualDiagnostics.RegisterSourceInfo(rowDefinition = new RowDefinition(), new Uri("Coding\\Pages\\CodingListPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 21, 18);
			RowDefinition rowDefinition2;
			VisualDiagnostics.RegisterSourceInfo(rowDefinition2 = new RowDefinition(), new Uri("Coding\\Pages\\CodingListPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 22, 18);
			RowDefinition rowDefinition3;
			VisualDiagnostics.RegisterSourceInfo(rowDefinition3 = new RowDefinition(), new Uri("Coding\\Pages\\CodingListPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 23, 18);
			RowDefinition rowDefinition4;
			VisualDiagnostics.RegisterSourceInfo(rowDefinition4 = new RowDefinition(), new Uri("Coding\\Pages\\CodingListPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 24, 18);
			RowDefinition rowDefinition5;
			VisualDiagnostics.RegisterSourceInfo(rowDefinition5 = new RowDefinition(), new Uri("Coding\\Pages\\CodingListPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 25, 18);
			RowDefinition rowDefinition6;
			VisualDiagnostics.RegisterSourceInfo(rowDefinition6 = new RowDefinition(), new Uri("Coding\\Pages\\CodingListPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 26, 18);
			Entry entry;
			VisualDiagnostics.RegisterSourceInfo(entry = new Entry(), new Uri("Coding\\Pages\\CodingListPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 28, 14);
			BindingExtension bindingExtension;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension = new BindingExtension(), new Uri("Coding\\Pages\\CodingListPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 39, 17);
			DataTemplate dataTemplate;
			VisualDiagnostics.RegisterSourceInfo(dataTemplate = new DataTemplate(), new Uri("Coding\\Pages\\CodingListPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 41, 22);
			ListView listView;
			VisualDiagnostics.RegisterSourceInfo(listView = new ListView(), new Uri("Coding\\Pages\\CodingListPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 33, 14);
			Grid grid;
			VisualDiagnostics.RegisterSourceInfo(grid = new Grid(), new Uri("Coding\\Pages\\CodingListPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 19, 10);
			VisualDiagnostics.RegisterSourceInfo(this, new Uri("Coding\\Pages\\CodingListPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 2, 2);
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
			this.Resources = resourceDictionary;
			resourceDictionary.Add("BoolToNegativeConverter", boolToNegativeConverter);
			this.SetValue(Page.PaddingProperty, new Thickness(5.0, 0.0));
			this.SetValue(Page.UseSafeAreaProperty, true);
			this.Appearing += this.CodingListPage_Appearing;
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
			xmlNamespaceResolver.Add("settings", "clr-namespace:CarScannerXamarinForms.Settings");
			xamlServiceProvider.Add(typeFromHandle2, new XamlTypeResolver(xmlNamespaceResolver, typeof(CodingListPage).GetTypeInfo().Assembly));
			xamlServiceProvider.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(12, 5)));
			DynamicResource dynamicResource = markupExtension.ProvideValue(xamlServiceProvider);
			this.SetDynamicResource(VisualElement.BackgroundColorProperty, dynamicResource.Key);
			this.Resources = resourceDictionary;
			grid.SetValue(View.MarginProperty, new Thickness(0.0, 0.0, 0.0, 0.0));
			rowDefinition.SetValue(RowDefinition.HeightProperty, new GridLengthTypeConverter().ConvertFromInvariantString("auto"));
			grid.GetValue(Grid.RowDefinitionsProperty).Add(rowDefinition);
			rowDefinition2.SetValue(RowDefinition.HeightProperty, new GridLengthTypeConverter().ConvertFromInvariantString("auto"));
			grid.GetValue(Grid.RowDefinitionsProperty).Add(rowDefinition2);
			rowDefinition3.SetValue(RowDefinition.HeightProperty, new GridLengthTypeConverter().ConvertFromInvariantString("*"));
			grid.GetValue(Grid.RowDefinitionsProperty).Add(rowDefinition3);
			rowDefinition4.SetValue(RowDefinition.HeightProperty, new GridLengthTypeConverter().ConvertFromInvariantString("auto"));
			grid.GetValue(Grid.RowDefinitionsProperty).Add(rowDefinition4);
			rowDefinition5.SetValue(RowDefinition.HeightProperty, new GridLengthTypeConverter().ConvertFromInvariantString("auto"));
			grid.GetValue(Grid.RowDefinitionsProperty).Add(rowDefinition5);
			rowDefinition6.SetValue(RowDefinition.HeightProperty, new GridLengthTypeConverter().ConvertFromInvariantString("auto"));
			grid.GetValue(Grid.RowDefinitionsProperty).Add(rowDefinition6);
			entry.SetValue(Grid.RowProperty, 1);
			entry.SetValue(Entry.PlaceholderProperty, "\ud83d\udd0e");
			entry.TextChanged += this.searchBar_TextChanged;
			grid.Children.Add(entry);
			listView.SetValue(Grid.RowProperty, 2);
			listView.SetValue(VisualElement.BackgroundColorProperty, Color.Transparent);
			listView.SetValue(ListView.HasUnevenRowsProperty, true);
			listView.ItemSelected += this.Lv_ItemSelected;
			bindingExtension.Path = "FilteredCollection";
			BindingBase bindingBase = bindingExtension.ProvideValue(null);
			listView.SetBinding(ItemsView<Cell>.ItemsSourceProperty, bindingBase);
			IDataTemplate dataTemplate2 = dataTemplate;
			CodingListPage.<InitializeComponent>_anonXamlCDataTemplate_7 <InitializeComponent>_anonXamlCDataTemplate_ = new CodingListPage.<InitializeComponent>_anonXamlCDataTemplate_7();
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

		// Token: 0x06004D67 RID: 19815 RVA: 0x00398F68 File Offset: 0x00397168
		private void __InitComponentRuntime()
		{
			Extensions.LoadFromXaml<CodingListPage>(this, typeof(CodingListPage));
			this.searchBar = NameScopeExtensions.FindByName<Entry>(this, "searchBar");
			this.lv = NameScopeExtensions.FindByName<ListView>(this, "lv");
		}

		// Token: 0x04002DE7 RID: 11751
		private CodingListModel Model;

		// Token: 0x04002DE8 RID: 11752
		private string filtertext = "";

		// Token: 0x04002DE9 RID: 11753
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private Entry searchBar;

		// Token: 0x04002DEA RID: 11754
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private ListView lv;

		// Token: 0x02000901 RID: 2305
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <CodingListPage_Appearing>d__1 : IAsyncStateMachine
		{
			// Token: 0x06004D68 RID: 19816 RVA: 0x00398FA0 File Offset: 0x003971A0
			void IAsyncStateMachine.MoveNext()
			{
				try
				{
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

			// Token: 0x06004D69 RID: 19817 RVA: 0x00398FEC File Offset: 0x003971EC
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x04002DEB RID: 11755
			public int <>1__state;

			// Token: 0x04002DEC RID: 11756
			public AsyncVoidMethodBuilder <>t__builder;
		}

		// Token: 0x02000902 RID: 2306
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <searchBar_TextChanged>d__5 : IAsyncStateMachine
		{
			// Token: 0x06004D6A RID: 19818 RVA: 0x00398FFC File Offset: 0x003971FC
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				CodingListPage codingListPage = this;
				try
				{
					TaskAwaiter taskAwaiter;
					if (num != 0)
					{
						codingListPage.filtertext = codingListPage.searchBar.Text;
						taskAwaiter = Task.Delay(500).GetAwaiter();
						if (!taskAwaiter.IsCompleted)
						{
							num2 = 0;
							TaskAwaiter taskAwaiter2 = taskAwaiter;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, CodingListPage.<searchBar_TextChanged>d__5>(ref taskAwaiter, ref this);
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
					if (codingListPage.filtertext == codingListPage.searchBar.Text)
					{
						try
						{
							codingListPage.Model.Filter = codingListPage.filtertext;
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

			// Token: 0x06004D6B RID: 19819 RVA: 0x00399100 File Offset: 0x00397300
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x04002DED RID: 11757
			public int <>1__state;

			// Token: 0x04002DEE RID: 11758
			public AsyncVoidMethodBuilder <>t__builder;

			// Token: 0x04002DEF RID: 11759
			public CodingListPage <>4__this;

			// Token: 0x04002DF0 RID: 11760
			private TaskAwaiter <>u__1;
		}

		// Token: 0x02000903 RID: 2307
		[CompilerGenerated]
		private sealed class <InitializeComponent>_anonXamlCDataTemplate_7
		{
			// Token: 0x06004D6C RID: 19820 RVA: 0x00399110 File Offset: 0x00397310
			public <InitializeComponent>_anonXamlCDataTemplate_7()
			{
			}

			// Token: 0x06004D6D RID: 19821 RVA: 0x00399124 File Offset: 0x00397324
			internal object LoadDataTemplate()
			{
				BindingExtension bindingExtension;
				VisualDiagnostics.RegisterSourceInfo(bindingExtension = new BindingExtension(), new Uri("Coding\\Pages\\CodingListPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 44, 62);
				Label label;
				VisualDiagnostics.RegisterSourceInfo(label = new Label(), new Uri("Coding\\Pages\\CodingListPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 44, 34);
				BindingExtension bindingExtension2;
				VisualDiagnostics.RegisterSourceInfo(bindingExtension2 = new BindingExtension(), new Uri("Coding\\Pages\\CodingListPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 45, 40);
				DynamicResourceExtension dynamicResourceExtension;
				VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension = new DynamicResourceExtension(), new Uri("Coding\\Pages\\CodingListPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 45, 69);
				Label label2;
				VisualDiagnostics.RegisterSourceInfo(label2 = new Label(), new Uri("Coding\\Pages\\CodingListPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 45, 34);
				StackLayout stackLayout;
				VisualDiagnostics.RegisterSourceInfo(stackLayout = new StackLayout(), new Uri("Coding\\Pages\\CodingListPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 43, 30);
				ViewCell viewCell;
				VisualDiagnostics.RegisterSourceInfo(viewCell = new ViewCell(), new Uri("Coding\\Pages\\CodingListPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 42, 26);
				NameScope nameScope = new NameScope();
				NameScope.SetNameScope(viewCell, nameScope);
				stackLayout.SetValue(View.MarginProperty, new Thickness(0.0, 2.0, 0.0, 3.0));
				stackLayout.SetValue(StackLayout.OrientationProperty, 0);
				label.SetValue(Label.FontAttributesProperty, new FontAttributesConverter().ConvertFromInvariantString("Bold"));
				bindingExtension.Path = "Name";
				BindingBase bindingBase = bindingExtension.ProvideValue(null);
				label.SetBinding(Label.TextProperty, bindingBase);
				stackLayout.Children.Add(label);
				bindingExtension2.Path = "Description";
				BindingBase bindingBase2 = bindingExtension2.ProvideValue(null);
				label2.SetBinding(Label.TextProperty, bindingBase2);
				dynamicResourceExtension.Key = "GrayedTextColor";
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
				xamlServiceProvider.Add(typeFromHandle, obj = new SimpleValueTargetProvider(array2, Label.TextColorProperty, nameScope));
				xamlServiceProvider.Add(typeof(IReferenceProvider), obj);
				Type typeFromHandle2 = typeof(IXamlTypeResolver);
				XmlNamespaceResolver xmlNamespaceResolver = new XmlNamespaceResolver();
				xmlNamespaceResolver.Add("", "http://xamarin.com/schemas/2014/forms");
				xmlNamespaceResolver.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
				xmlNamespaceResolver.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
				xmlNamespaceResolver.Add("local", "clr-namespace:CarScannerXamarinForms");
				xmlNamespaceResolver.Add("settings", "clr-namespace:CarScannerXamarinForms.Settings");
				xamlServiceProvider.Add(typeFromHandle2, new XamlTypeResolver(xmlNamespaceResolver, typeof(CodingListPage.<InitializeComponent>_anonXamlCDataTemplate_7).GetTypeInfo().Assembly));
				xamlServiceProvider.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(45, 69)));
				DynamicResource dynamicResource = markupExtension.ProvideValue(xamlServiceProvider);
				label2.SetDynamicResource(Label.TextColorProperty, dynamicResource.Key);
				stackLayout.Children.Add(label2);
				viewCell.View = stackLayout;
				return viewCell;
			}

			// Token: 0x04002DF1 RID: 11761
			internal object[] parentValues;

			// Token: 0x04002DF2 RID: 11762
			internal CodingListPage root;
		}
	}
}
