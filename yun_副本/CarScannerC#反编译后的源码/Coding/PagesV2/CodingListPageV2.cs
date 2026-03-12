using System;
using System.CodeDom.Compiler;
using System.Diagnostics;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using AiForms.Renderers;
using CarScannerXamarinForms.Coding.Pages;
using Xamarin.Forms;
using Xamarin.Forms.Internals;
using Xamarin.Forms.PlatformConfiguration.iOSSpecific;
using Xamarin.Forms.Xaml;
using Xamarin.Forms.Xaml.Diagnostics;
using Xamarin.Forms.Xaml.Internals;

namespace CarScannerXamarinForms.Coding.PagesV2
{
	// Token: 0x02000989 RID: 2441
	[XamlCompilation(2)]
	[XamlFilePath("Coding\\PagesV2\\CodingListPageV2.xaml")]
	public class CodingListPageV2 : ContentPage
	{
		// Token: 0x06005056 RID: 20566 RVA: 0x003DC924 File Offset: 0x003DAB24
		public CodingListPageV2(CodingListModel model, string Title)
		{
			this.InitializeComponent();
			this.Model = model;
			base.Title = Title;
			this.lv.BindingContext = this.Model;
			base.Disappearing += this.CodingListPageV2_Disappearing;
			base.Appearing += this.CodingListPageV2_Appearing;
		}

		// Token: 0x06005057 RID: 20567 RVA: 0x003DC98B File Offset: 0x003DAB8B
		private void CodingListPageV2_Appearing(object sender, EventArgs e)
		{
			this.searchBar.TextChanged -= this.searchBar_TextChanged;
			this.searchBar.TextChanged += this.searchBar_TextChanged;
		}

		// Token: 0x06005058 RID: 20568 RVA: 0x003DC98B File Offset: 0x003DAB8B
		private void CodingListPageV2_Disappearing(object sender, EventArgs e)
		{
			this.searchBar.TextChanged -= this.searchBar_TextChanged;
			this.searchBar.TextChanged += this.searchBar_TextChanged;
		}

		// Token: 0x06005059 RID: 20569 RVA: 0x003DC9BC File Offset: 0x003DABBC
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

		// Token: 0x0600505A RID: 20570 RVA: 0x003DC9F4 File Offset: 0x003DABF4
		private async void cell_Tapped(object sender, EventArgs e)
		{
			base.IsEnabled = false;
			ICodingContainer codingContainer = (ICodingContainer)((Element)sender).BindingContext;
			if (codingContainer.ValueType == AdaptationValueTypes.OptionType)
			{
				if (codingContainer is IServiceProcedure)
				{
					CodingServiceProcedurePage codingServiceProcedurePage = new CodingServiceProcedurePage(codingContainer);
					base.Navigation.PushAsync(codingServiceProcedurePage);
				}
				else
				{
					UniversalCodingPageV2 universalCodingPageV = new UniversalCodingPageV2(codingContainer);
					base.Navigation.PushAsync(universalCodingPageV);
				}
			}
			else if (codingContainer.ValueType == AdaptationValueTypes.InputValueType)
			{
				UniversalCodingPageV2 universalCodingPageV2 = new UniversalCodingPageV2(codingContainer);
				base.Navigation.PushAsync(universalCodingPageV2);
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
				UniversalCodingPageV2 universalCodingPageV3 = new UniversalCodingPageV2(codingContainer);
				base.Navigation.PushAsync(universalCodingPageV3);
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
				UniversalCodingPageV2 universalCodingPageV4 = new UniversalCodingPageV2(codingContainer);
				base.Navigation.PushAsync(universalCodingPageV4);
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

		// Token: 0x0600505B RID: 20571 RVA: 0x003DCA34 File Offset: 0x003DAC34
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private void InitializeComponent()
		{
			if (ResourceLoader.IsEnabled && ResourceLoader.CanProvideContentFor(new ResourceLoader.ResourceLoadingQuery
			{
				AssemblyName = typeof(CodingListPageV2).GetTypeInfo().Assembly.GetName(),
				ResourcePath = "Coding/PagesV2/CodingListPageV2.xaml",
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
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension = new DynamicResourceExtension(), new Uri("Coding\\PagesV2\\CodingListPageV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 13, 5);
			BoolToNegativeConverter boolToNegativeConverter;
			VisualDiagnostics.RegisterSourceInfo(boolToNegativeConverter = new BoolToNegativeConverter(), new Uri("Coding\\PagesV2\\CodingListPageV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 16, 14);
			ResourceDictionary resourceDictionary;
			VisualDiagnostics.RegisterSourceInfo(resourceDictionary = new ResourceDictionary(), new Uri("Coding\\PagesV2\\CodingListPageV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 15, 10);
			Entry entry;
			VisualDiagnostics.RegisterSourceInfo(entry = new Entry(), new Uri("Coding\\PagesV2\\CodingListPageV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 21, 14);
			BindingExtension bindingExtension;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension = new BindingExtension(), new Uri("Coding\\PagesV2\\CodingListPageV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 31, 29);
			DataTemplate dataTemplate;
			VisualDiagnostics.RegisterSourceInfo(dataTemplate = new DataTemplate(), new Uri("Coding\\PagesV2\\CodingListPageV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 33, 26);
			Section section;
			VisualDiagnostics.RegisterSourceInfo(section = new Section(), new Uri("Coding\\PagesV2\\CodingListPageV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 31, 18);
			SettingsView settingsView;
			VisualDiagnostics.RegisterSourceInfo(settingsView = new SettingsView(), new Uri("Coding\\PagesV2\\CodingListPageV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 26, 14);
			Grid grid;
			VisualDiagnostics.RegisterSourceInfo(grid = new Grid(), new Uri("Coding\\PagesV2\\CodingListPageV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 20, 10);
			VisualDiagnostics.RegisterSourceInfo(this, new Uri("Coding\\PagesV2\\CodingListPageV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 2, 2);
			NameScope nameScope = NameScope.GetNameScope(this) ?? new NameScope();
			NameScope.SetNameScope(this, nameScope);
			nameScope.RegisterName("searchBar", entry);
			if (entry.StyleId == null)
			{
				entry.StyleId = "searchBar";
			}
			nameScope.RegisterName("lv", settingsView);
			if (settingsView.StyleId == null)
			{
				settingsView.StyleId = "lv";
			}
			this.searchBar = entry;
			this.lv = settingsView;
			this.Resources = resourceDictionary;
			resourceDictionary.Add("BoolToNegativeConverter", boolToNegativeConverter);
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
			xmlNamespaceResolver.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xmlNamespaceResolver.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider.Add(typeFromHandle2, new XamlTypeResolver(xmlNamespaceResolver, typeof(CodingListPageV2).GetTypeInfo().Assembly));
			xamlServiceProvider.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(13, 5)));
			DynamicResource dynamicResource = markupExtension.ProvideValue(xamlServiceProvider);
			this.SetDynamicResource(VisualElement.BackgroundColorProperty, dynamicResource.Key);
			this.Resources = resourceDictionary;
			grid.SetValue(Grid.RowDefinitionsProperty, new RowDefinitionCollectionTypeConverter().ConvertFromInvariantString("Auto, *"));
			grid.SetValue(Grid.RowSpacingProperty, 0.0);
			entry.SetValue(Grid.RowProperty, 0);
			entry.SetValue(Entry.PlaceholderProperty, "\ud83d\udd0e");
			grid.Children.Add(entry);
			settingsView.SetValue(Grid.RowProperty, 1);
			settingsView.SetValue(TableView.HasUnevenRowsProperty, true);
			settingsView.SetValue(SettingsView.HeaderHeightProperty, 0.0);
			bindingExtension.Path = "FilteredCollection";
			BindingBase bindingBase = bindingExtension.ProvideValue(null);
			section.SetBinding(Section.ItemsSourceProperty, bindingBase);
			IDataTemplate dataTemplate2 = dataTemplate;
			CodingListPageV2.<InitializeComponent>_anonXamlCDataTemplate_2 <InitializeComponent>_anonXamlCDataTemplate_ = new CodingListPageV2.<InitializeComponent>_anonXamlCDataTemplate_2();
			object[] array2 = new object[0 + 5];
			array2[0] = dataTemplate;
			array2[1] = section;
			array2[2] = settingsView;
			array2[3] = grid;
			array2[4] = this;
			<InitializeComponent>_anonXamlCDataTemplate_.parentValues = array2;
			<InitializeComponent>_anonXamlCDataTemplate_.root = this;
			dataTemplate2.LoadTemplate = new Func<object>(<InitializeComponent>_anonXamlCDataTemplate_.LoadDataTemplate);
			section.SetValue(Section.ItemTemplateProperty, dataTemplate);
			settingsView.Root.Add(section);
			grid.Children.Add(settingsView);
			this.SetValue(ContentPage.ContentProperty, grid);
		}

		// Token: 0x0600505C RID: 20572 RVA: 0x003DCF66 File Offset: 0x003DB166
		private void __InitComponentRuntime()
		{
			Extensions.LoadFromXaml<CodingListPageV2>(this, typeof(CodingListPageV2));
			this.searchBar = NameScopeExtensions.FindByName<Entry>(this, "searchBar");
			this.lv = NameScopeExtensions.FindByName<SettingsView>(this, "lv");
		}

		// Token: 0x04003027 RID: 12327
		private CodingListModel Model;

		// Token: 0x04003028 RID: 12328
		private string filtertext = "";

		// Token: 0x04003029 RID: 12329
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private Entry searchBar;

		// Token: 0x0400302A RID: 12330
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private SettingsView lv;

		// Token: 0x0200098A RID: 2442
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <cell_Tapped>d__6 : IAsyncStateMachine
		{
			// Token: 0x0600505D RID: 20573 RVA: 0x003DCF9C File Offset: 0x003DB19C
			void IAsyncStateMachine.MoveNext()
			{
				CodingListPageV2 codingListPageV = this;
				try
				{
					codingListPageV.IsEnabled = false;
					ICodingContainer codingContainer = (ICodingContainer)((Element)sender).BindingContext;
					if (codingContainer.ValueType == AdaptationValueTypes.OptionType)
					{
						if (codingContainer is IServiceProcedure)
						{
							CodingServiceProcedurePage codingServiceProcedurePage = new CodingServiceProcedurePage(codingContainer);
							codingListPageV.Navigation.PushAsync(codingServiceProcedurePage);
						}
						else
						{
							UniversalCodingPageV2 universalCodingPageV = new UniversalCodingPageV2(codingContainer);
							codingListPageV.Navigation.PushAsync(universalCodingPageV);
						}
					}
					else if (codingContainer.ValueType == AdaptationValueTypes.InputValueType)
					{
						UniversalCodingPageV2 universalCodingPageV2 = new UniversalCodingPageV2(codingContainer);
						codingListPageV.Navigation.PushAsync(universalCodingPageV2);
					}
					else if (codingContainer.ValueType == AdaptationValueTypes.MQBLightConfiguration)
					{
						CodingLightConfigurationPage codingLightConfigurationPage = new CodingLightConfigurationPage(codingContainer);
						codingListPageV.Navigation.PushAsync(codingLightConfigurationPage);
					}
					else if (codingContainer.ValueType == AdaptationValueTypes.InputHexDataType)
					{
						if (codingContainer is CustomizableCodingTemplate)
						{
							CodingWithHexInput codingWithHexInput = new CodingWithHexInput(codingContainer);
							codingListPageV.Navigation.PushAsync(codingWithHexInput);
						}
						else
						{
							LongCodingPage longCodingPage = new LongCodingPage(codingContainer);
							codingListPageV.Navigation.PushAsync(longCodingPage);
						}
					}
					else if (codingContainer.ValueType == AdaptationValueTypes.MQBColorList)
					{
						CodingWithColorSelectionPage codingWithColorSelectionPage = new CodingWithColorSelectionPage(codingContainer);
						codingListPageV.Navigation.PushAsync(codingWithColorSelectionPage);
					}
					else if (codingContainer.ValueType == AdaptationValueTypes.MQBParametrizeDump)
					{
						DataSetDumperPage dataSetDumperPage = new DataSetDumperPage((MQBParametrizeBase)codingContainer);
						codingListPageV.Navigation.PushAsync(dataSetDumperPage);
					}
					else if (codingContainer.ValueType == AdaptationValueTypes.InputTextType)
					{
						UniversalCodingPageV2 universalCodingPageV3 = new UniversalCodingPageV2(codingContainer);
						codingListPageV.Navigation.PushAsync(universalCodingPageV3);
					}
					else if (codingContainer.ValueType == AdaptationValueTypes.MQBFPAEditor)
					{
						FPAProfileCodingPage fpaprofileCodingPage = new FPAProfileCodingPage(codingContainer);
						codingListPageV.Navigation.PushAsync(fpaprofileCodingPage);
					}
					else if (codingContainer.ValueType == AdaptationValueTypes.ToyotaTPMSSensor)
					{
						ToyotaTPMS2APage toyotaTPMS2APage = new ToyotaTPMS2APage(codingContainer);
						codingListPageV.Navigation.PushAsync(toyotaTPMS2APage);
					}
					else if (codingContainer.ValueType == AdaptationValueTypes.MQBRKDSGenerator)
					{
						VagRKDSPage vagRKDSPage = new VagRKDSPage(codingContainer);
						codingListPageV.Navigation.PushAsync(vagRKDSPage);
					}
					else if (codingContainer.ValueType == AdaptationValueTypes.MQBA5Customization)
					{
						CodingA5CustomizationPage codingA5CustomizationPage = new CodingA5CustomizationPage(codingContainer);
						codingListPageV.Navigation.PushAsync(codingA5CustomizationPage);
					}
					else if (codingContainer.ValueType == AdaptationValueTypes.InputFloatIEEE754)
					{
						UniversalCodingPageV2 universalCodingPageV4 = new UniversalCodingPageV2(codingContainer);
						codingListPageV.Navigation.PushAsync(universalCodingPageV4);
					}
					else if (codingContainer.ValueType == AdaptationValueTypes.TailgateCustomization)
					{
						MQBTailGatePageCustomization mqbtailGatePageCustomization = new MQBTailGatePageCustomization(codingContainer);
						codingListPageV.Navigation.PushAsync(mqbtailGatePageCustomization);
					}
					else if (codingContainer.ValueType == AdaptationValueTypes.TPMS)
					{
						TPMS4WheelsPage tpms4WheelsPage = new TPMS4WheelsPage(codingContainer);
						codingListPageV.Navigation.PushAsync(tpms4WheelsPage);
					}
					codingListPageV.IsEnabled = true;
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

			// Token: 0x0600505E RID: 20574 RVA: 0x003DD254 File Offset: 0x003DB454
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x0400302B RID: 12331
			public int <>1__state;

			// Token: 0x0400302C RID: 12332
			public AsyncVoidMethodBuilder <>t__builder;

			// Token: 0x0400302D RID: 12333
			public CodingListPageV2 <>4__this;

			// Token: 0x0400302E RID: 12334
			public object sender;
		}

		// Token: 0x0200098B RID: 2443
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <searchBar_TextChanged>d__5 : IAsyncStateMachine
		{
			// Token: 0x0600505F RID: 20575 RVA: 0x003DD264 File Offset: 0x003DB464
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				CodingListPageV2 codingListPageV = this;
				try
				{
					TaskAwaiter taskAwaiter;
					if (num != 0)
					{
						codingListPageV.filtertext = codingListPageV.searchBar.Text;
						taskAwaiter = Task.Delay(500).GetAwaiter();
						if (!taskAwaiter.IsCompleted)
						{
							num2 = 0;
							TaskAwaiter taskAwaiter2 = taskAwaiter;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, CodingListPageV2.<searchBar_TextChanged>d__5>(ref taskAwaiter, ref this);
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
					if (codingListPageV.filtertext == codingListPageV.searchBar.Text)
					{
						try
						{
							codingListPageV.Model.Filter = codingListPageV.filtertext;
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

			// Token: 0x06005060 RID: 20576 RVA: 0x003DD368 File Offset: 0x003DB568
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x0400302F RID: 12335
			public int <>1__state;

			// Token: 0x04003030 RID: 12336
			public AsyncVoidMethodBuilder <>t__builder;

			// Token: 0x04003031 RID: 12337
			public CodingListPageV2 <>4__this;

			// Token: 0x04003032 RID: 12338
			private TaskAwaiter <>u__1;
		}

		// Token: 0x0200098C RID: 2444
		[CompilerGenerated]
		private sealed class <InitializeComponent>_anonXamlCDataTemplate_2
		{
			// Token: 0x06005061 RID: 20577 RVA: 0x003DD378 File Offset: 0x003DB578
			public <InitializeComponent>_anonXamlCDataTemplate_2()
			{
			}

			// Token: 0x06005062 RID: 20578 RVA: 0x003DD38C File Offset: 0x003DB58C
			internal object LoadDataTemplate()
			{
				BindingExtension bindingExtension;
				VisualDiagnostics.RegisterSourceInfo(bindingExtension = new BindingExtension(), new Uri("Coding\\PagesV2\\CodingListPageV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 35, 33);
				BindingExtension bindingExtension2;
				VisualDiagnostics.RegisterSourceInfo(bindingExtension2 = new BindingExtension(), new Uri("Coding\\PagesV2\\CodingListPageV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 36, 33);
				DynamicResourceExtension dynamicResourceExtension;
				VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension = new DynamicResourceExtension(), new Uri("Coding\\PagesV2\\CodingListPageV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 38, 33);
				LabelCell labelCell;
				VisualDiagnostics.RegisterSourceInfo(labelCell = new LabelCell(), new Uri("Coding\\PagesV2\\CodingListPageV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 34, 30);
				NameScope nameScope = new NameScope();
				NameScope.SetNameScope(labelCell, nameScope);
				bindingExtension.Path = "Name";
				BindingBase bindingBase = bindingExtension.ProvideValue(null);
				labelCell.SetBinding(CellBase.TitleProperty, bindingBase);
				bindingExtension2.Path = "Description";
				BindingBase bindingBase2 = bindingExtension2.ProvideValue(null);
				labelCell.SetBinding(CellBase.DescriptionProperty, bindingBase2);
				labelCell.Tapped += this.root.cell_Tapped;
				dynamicResourceExtension.Key = "ButtonAccentColor";
				IMarkupExtension<DynamicResource> markupExtension = dynamicResourceExtension;
				XamlServiceProvider xamlServiceProvider = new XamlServiceProvider();
				Type typeFromHandle = typeof(IProvideValueTarget);
				int num;
				object[] array = new object[(num = this.parentValues.Length) + 1];
				Array.Copy(this.parentValues, 0, array, 1, num);
				object[] array2 = array;
				array2[0] = labelCell;
				object obj;
				xamlServiceProvider.Add(typeFromHandle, obj = new SimpleValueTargetProvider(array2, CellBase.TitleColorProperty, nameScope));
				xamlServiceProvider.Add(typeof(IReferenceProvider), obj);
				Type typeFromHandle2 = typeof(IXamlTypeResolver);
				XmlNamespaceResolver xmlNamespaceResolver = new XmlNamespaceResolver();
				xmlNamespaceResolver.Add("", "http://xamarin.com/schemas/2014/forms");
				xmlNamespaceResolver.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
				xmlNamespaceResolver.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
				xmlNamespaceResolver.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
				xmlNamespaceResolver.Add("local", "clr-namespace:CarScannerXamarinForms");
				xmlNamespaceResolver.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
				xmlNamespaceResolver.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
				xmlNamespaceResolver.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
				xamlServiceProvider.Add(typeFromHandle2, new XamlTypeResolver(xmlNamespaceResolver, typeof(CodingListPageV2.<InitializeComponent>_anonXamlCDataTemplate_2).GetTypeInfo().Assembly));
				xamlServiceProvider.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(38, 33)));
				DynamicResource dynamicResource = markupExtension.ProvideValue(xamlServiceProvider);
				labelCell.SetDynamicResource(CellBase.TitleColorProperty, dynamicResource.Key);
				return labelCell;
			}

			// Token: 0x04003033 RID: 12339
			internal object[] parentValues;

			// Token: 0x04003034 RID: 12340
			internal CodingListPageV2 root;
		}
	}
}
