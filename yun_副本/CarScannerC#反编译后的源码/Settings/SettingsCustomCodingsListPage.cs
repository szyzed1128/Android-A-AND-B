using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using CarScannerXamarinForms.Coding;
using Newtonsoft.Json;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Internals;
using Xamarin.Forms.PlatformConfiguration.iOSSpecific;
using Xamarin.Forms.Xaml;
using Xamarin.Forms.Xaml.Diagnostics;
using Xamarin.Forms.Xaml.Internals;

namespace CarScannerXamarinForms.Settings
{
	// Token: 0x02000225 RID: 549
	[XamlCompilation(2)]
	[XamlFilePath("Settings\\SettingsCustomCodingsListPage.xaml")]
	public class SettingsCustomCodingsListPage : ContentPage
	{
		// Token: 0x06001AE8 RID: 6888 RVA: 0x0012B2D0 File Offset: 0x001294D0
		public SettingsCustomCodingsListPage()
		{
			this.InitializeComponent();
			base.ToolbarItems.Add(new ToolbarItem("", (string)Application.Current.Resources["NB_add"], delegate
			{
				this.AddNewCustomPid();
			}, 0, 0));
			base.ToolbarItems.Add(new ToolbarItem("", (string)Application.Current.Resources["NB_delete"], delegate
			{
				this.DeleteAllCustomPidsQuestion();
			}, 0, 0));
			this.Model = new CustomCodingsListViewModel();
			this.Model.Load();
			this.lv.ItemsSource = this.Model;
		}

		// Token: 0x06001AE9 RID: 6889 RVA: 0x0012B388 File Offset: 0x00129588
		private void Lv_ItemSelected(object sender, SelectedItemChangedEventArgs e)
		{
			if (this.lv.SelectedItem == null)
			{
				return;
			}
			CustomizableCodingTemplate customizableCodingTemplate = (CustomizableCodingTemplate)this.lv.SelectedItem;
			this.lv.SelectedItem = null;
			CustomCodingEditor customCodingEditor = new CustomCodingEditor(this.Model, customizableCodingTemplate);
			base.Navigation.PushAsync(customCodingEditor);
		}

		// Token: 0x06001AEA RID: 6890 RVA: 0x0012B3DC File Offset: 0x001295DC
		private async void BtnExportAsFile_Clicked(object sender, EventArgs e)
		{
			string cacheDirectory = FileSystem.CacheDirectory;
			string text = "customcodings.csc";
			string text2 = Path.Combine(cacheDirectory, text);
			string text3 = JsonConvert.SerializeObject(this.Model);
			try
			{
				using (StreamWriter streamWriter = new StreamWriter(text2, false, Encoding.UTF8))
				{
					streamWriter.WriteLine(text3);
					streamWriter.Flush();
				}
			}
			catch (Exception)
			{
			}
			try
			{
				await Share.RequestAsync(new ShareFileRequest
				{
					Title = "Custom Codings",
					File = new ShareFile(text2)
				});
			}
			catch (Exception)
			{
			}
		}

		// Token: 0x06001AEB RID: 6891 RVA: 0x0012B414 File Offset: 0x00129614
		private async void btnDel_Clicked(object sender, EventArgs e)
		{
			this.lv.IsEnabled = false;
			CustomizableCodingTemplate customizableCodingTemplate = (sender as MenuItem).BindingContext as CustomizableCodingTemplate;
			try
			{
				this.Model.Remove(customizableCodingTemplate);
				this.Model.Save();
			}
			catch
			{
			}
			this.lv.IsEnabled = true;
		}

		// Token: 0x06001AEC RID: 6892 RVA: 0x0012B454 File Offset: 0x00129654
		private async void BtnImportFromFile_Clicked(object sender, EventArgs e)
		{
			try
			{
				FileResult fileResult = await FilePicker.PickAsync(null);
				if (fileResult != null && fileResult.FileName.EndsWith("csc", StringComparison.OrdinalIgnoreCase))
				{
					using (StreamReader streamReader = new StreamReader(fileResult.FullPath, Encoding.UTF8))
					{
						foreach (CustomizableCodingTemplate customizableCodingTemplate in JsonConvert.DeserializeObject<List<CustomizableCodingTemplate>>(streamReader.ReadToEnd()))
						{
							this.Model.Add(customizableCodingTemplate);
						}
						this.Model.Save();
					}
				}
			}
			catch (Exception)
			{
			}
		}

		// Token: 0x17000FA4 RID: 4004
		// (get) Token: 0x06001AED RID: 6893 RVA: 0x0012B48B File Offset: 0x0012968B
		// (set) Token: 0x06001AEE RID: 6894 RVA: 0x0012B493 File Offset: 0x00129693
		public CustomCodingsListViewModel Model
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

		// Token: 0x06001AEF RID: 6895 RVA: 0x0012B49C File Offset: 0x0012969C
		private void AddNewCustomPid()
		{
			CustomizableCodingTemplate customizableCodingTemplate = new CustomizableCodingTemplate
			{
				Name = "New coding"
			};
			this.Model.Add(customizableCodingTemplate);
			this.Model.Save();
		}

		// Token: 0x06001AF0 RID: 6896 RVA: 0x0012B4D4 File Offset: 0x001296D4
		private async void DeleteAllCustomPidsQuestion()
		{
			TaskAwaiter<bool> taskAwaiter = base.DisplayAlert(Translate.GetString("ios_PidListDeleteAllCustomTitle"), Translate.GetString("ios_PidListDeleteAllCustomText"), Translate.GetString("ios_Yes"), Translate.GetString("ios_No")).GetAwaiter();
			if (!taskAwaiter.IsCompleted)
			{
				await taskAwaiter;
				TaskAwaiter<bool> taskAwaiter2;
				taskAwaiter = taskAwaiter2;
				taskAwaiter2 = default(TaskAwaiter<bool>);
			}
			if (taskAwaiter.GetResult())
			{
				this.Model.Clear();
				this.Model.Save();
			}
		}

		// Token: 0x06001AF1 RID: 6897 RVA: 0x0012B50C File Offset: 0x0012970C
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private void InitializeComponent()
		{
			if (ResourceLoader.IsEnabled && ResourceLoader.CanProvideContentFor(new ResourceLoader.ResourceLoadingQuery
			{
				AssemblyName = typeof(SettingsCustomCodingsListPage).GetTypeInfo().Assembly.GetName(),
				ResourcePath = "Settings/SettingsCustomCodingsListPage.xaml",
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
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension = new DynamicResourceExtension(), new Uri("Settings\\SettingsCustomCodingsListPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 11, 5);
			BoolToNegativeConverter boolToNegativeConverter;
			VisualDiagnostics.RegisterSourceInfo(boolToNegativeConverter = new BoolToNegativeConverter(), new Uri("Settings\\SettingsCustomCodingsListPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 14, 14);
			ResourceDictionary resourceDictionary;
			VisualDiagnostics.RegisterSourceInfo(resourceDictionary = new ResourceDictionary(), new Uri("Settings\\SettingsCustomCodingsListPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 13, 10);
			RowDefinition rowDefinition;
			VisualDiagnostics.RegisterSourceInfo(rowDefinition = new RowDefinition(), new Uri("Settings\\SettingsCustomCodingsListPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 22, 18);
			RowDefinition rowDefinition2;
			VisualDiagnostics.RegisterSourceInfo(rowDefinition2 = new RowDefinition(), new Uri("Settings\\SettingsCustomCodingsListPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 23, 18);
			DataTemplate dataTemplate;
			VisualDiagnostics.RegisterSourceInfo(dataTemplate = new DataTemplate(), new Uri("Settings\\SettingsCustomCodingsListPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 32, 22);
			ListView listView;
			VisualDiagnostics.RegisterSourceInfo(listView = new ListView(), new Uri("Settings\\SettingsCustomCodingsListPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 25, 14);
			ColumnDefinition columnDefinition;
			VisualDiagnostics.RegisterSourceInfo(columnDefinition = new ColumnDefinition(), new Uri("Settings\\SettingsCustomCodingsListPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 47, 22);
			ColumnDefinition columnDefinition2;
			VisualDiagnostics.RegisterSourceInfo(columnDefinition2 = new ColumnDefinition(), new Uri("Settings\\SettingsCustomCodingsListPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 48, 22);
			Translate translate;
			VisualDiagnostics.RegisterSourceInfo(translate = new Translate(), new Uri("Settings\\SettingsCustomCodingsListPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 56, 21);
			Button button;
			VisualDiagnostics.RegisterSourceInfo(button = new Button(), new Uri("Settings\\SettingsCustomCodingsListPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 51, 18);
			Translate translate2;
			VisualDiagnostics.RegisterSourceInfo(translate2 = new Translate(), new Uri("Settings\\SettingsCustomCodingsListPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 63, 21);
			Button button2;
			VisualDiagnostics.RegisterSourceInfo(button2 = new Button(), new Uri("Settings\\SettingsCustomCodingsListPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 58, 18);
			Grid grid;
			VisualDiagnostics.RegisterSourceInfo(grid = new Grid(), new Uri("Settings\\SettingsCustomCodingsListPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 45, 14);
			Grid grid2;
			VisualDiagnostics.RegisterSourceInfo(grid2 = new Grid(), new Uri("Settings\\SettingsCustomCodingsListPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 20, 10);
			VisualDiagnostics.RegisterSourceInfo(this, new Uri("Settings\\SettingsCustomCodingsListPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 2, 2);
			NameScope nameScope = NameScope.GetNameScope(this) ?? new NameScope();
			NameScope.SetNameScope(this, nameScope);
			nameScope.RegisterName("lv", listView);
			if (listView.StyleId == null)
			{
				listView.StyleId = "lv";
			}
			nameScope.RegisterName("btnImportFromFile", button);
			if (button.StyleId == null)
			{
				button.StyleId = "btnImportFromFile";
			}
			nameScope.RegisterName("btnExportAsFile", button2);
			if (button2.StyleId == null)
			{
				button2.StyleId = "btnExportAsFile";
			}
			this.lv = listView;
			this.btnImportFromFile = button;
			this.btnExportAsFile = button2;
			this.Resources = resourceDictionary;
			resourceDictionary.Add("BoolToNegativeConverter", boolToNegativeConverter);
			this.SetValue(Page.TitleProperty, "Custom codings");
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
			xmlNamespaceResolver.Add("settings", "clr-namespace:CarScannerXamarinForms.Settings");
			xamlServiceProvider.Add(typeFromHandle2, new XamlTypeResolver(xmlNamespaceResolver, typeof(SettingsCustomCodingsListPage).GetTypeInfo().Assembly));
			xamlServiceProvider.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(11, 5)));
			DynamicResource dynamicResource = markupExtension.ProvideValue(xamlServiceProvider);
			this.SetDynamicResource(VisualElement.BackgroundColorProperty, dynamicResource.Key);
			this.Resources = resourceDictionary;
			rowDefinition.SetValue(RowDefinition.HeightProperty, new GridLengthTypeConverter().ConvertFromInvariantString("*"));
			grid2.GetValue(Grid.RowDefinitionsProperty).Add(rowDefinition);
			rowDefinition2.SetValue(RowDefinition.HeightProperty, new GridLengthTypeConverter().ConvertFromInvariantString("auto"));
			grid2.GetValue(Grid.RowDefinitionsProperty).Add(rowDefinition2);
			listView.SetValue(Grid.RowProperty, 0);
			listView.SetValue(View.MarginProperty, new Thickness(5.0, 0.0, 5.0, 0.0));
			listView.SetValue(ListView.HasUnevenRowsProperty, true);
			listView.ItemSelected += this.Lv_ItemSelected;
			IDataTemplate dataTemplate2 = dataTemplate;
			SettingsCustomCodingsListPage.<InitializeComponent>_anonXamlCDataTemplate_90 <InitializeComponent>_anonXamlCDataTemplate_ = new SettingsCustomCodingsListPage.<InitializeComponent>_anonXamlCDataTemplate_90();
			object[] array2 = new object[0 + 4];
			array2[0] = dataTemplate;
			array2[1] = listView;
			array2[2] = grid2;
			array2[3] = this;
			<InitializeComponent>_anonXamlCDataTemplate_.parentValues = array2;
			<InitializeComponent>_anonXamlCDataTemplate_.root = this;
			dataTemplate2.LoadTemplate = new Func<object>(<InitializeComponent>_anonXamlCDataTemplate_.LoadDataTemplate);
			listView.SetValue(ItemsView<Cell>.ItemTemplateProperty, dataTemplate);
			grid2.Children.Add(listView);
			grid.SetValue(Grid.RowProperty, 1);
			columnDefinition.SetValue(ColumnDefinition.WidthProperty, new GridLengthTypeConverter().ConvertFromInvariantString("*"));
			grid.GetValue(Grid.ColumnDefinitionsProperty).Add(columnDefinition);
			columnDefinition2.SetValue(ColumnDefinition.WidthProperty, new GridLengthTypeConverter().ConvertFromInvariantString("*"));
			grid.GetValue(Grid.ColumnDefinitionsProperty).Add(columnDefinition2);
			button.SetValue(Grid.ColumnProperty, 0);
			button.SetValue(Grid.ColumnSpanProperty, 1);
			button.Clicked += this.BtnImportFromFile_Clicked;
			translate.Text = "droid_ImportFromFile";
			IMarkupExtension markupExtension2 = translate;
			XamlServiceProvider xamlServiceProvider2 = new XamlServiceProvider();
			Type typeFromHandle3 = typeof(IProvideValueTarget);
			object[] array3 = new object[0 + 4];
			array3[0] = button;
			array3[1] = grid;
			array3[2] = grid2;
			array3[3] = this;
			object obj2;
			xamlServiceProvider2.Add(typeFromHandle3, obj2 = new SimpleValueTargetProvider(array3, Button.TextProperty, nameScope));
			xamlServiceProvider2.Add(typeof(IReferenceProvider), obj2);
			Type typeFromHandle4 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver2 = new XmlNamespaceResolver();
			xmlNamespaceResolver2.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver2.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver2.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver2.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver2.Add("settings", "clr-namespace:CarScannerXamarinForms.Settings");
			xamlServiceProvider2.Add(typeFromHandle4, new XamlTypeResolver(xmlNamespaceResolver2, typeof(SettingsCustomCodingsListPage).GetTypeInfo().Assembly));
			xamlServiceProvider2.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(56, 21)));
			object obj3 = markupExtension2.ProvideValue(xamlServiceProvider2);
			button.Text = obj3;
			grid.Children.Add(button);
			button2.SetValue(Grid.ColumnProperty, 1);
			button2.SetValue(Grid.ColumnSpanProperty, 1);
			button2.Clicked += this.BtnExportAsFile_Clicked;
			translate2.Text = "ios_Share";
			IMarkupExtension markupExtension3 = translate2;
			XamlServiceProvider xamlServiceProvider3 = new XamlServiceProvider();
			Type typeFromHandle5 = typeof(IProvideValueTarget);
			object[] array4 = new object[0 + 4];
			array4[0] = button2;
			array4[1] = grid;
			array4[2] = grid2;
			array4[3] = this;
			object obj4;
			xamlServiceProvider3.Add(typeFromHandle5, obj4 = new SimpleValueTargetProvider(array4, Button.TextProperty, nameScope));
			xamlServiceProvider3.Add(typeof(IReferenceProvider), obj4);
			Type typeFromHandle6 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver3 = new XmlNamespaceResolver();
			xmlNamespaceResolver3.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver3.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver3.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver3.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver3.Add("settings", "clr-namespace:CarScannerXamarinForms.Settings");
			xamlServiceProvider3.Add(typeFromHandle6, new XamlTypeResolver(xmlNamespaceResolver3, typeof(SettingsCustomCodingsListPage).GetTypeInfo().Assembly));
			xamlServiceProvider3.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(63, 21)));
			object obj5 = markupExtension3.ProvideValue(xamlServiceProvider3);
			button2.Text = obj5;
			grid.Children.Add(button2);
			grid2.Children.Add(grid);
			this.SetValue(ContentPage.ContentProperty, grid2);
		}

		// Token: 0x06001AF2 RID: 6898 RVA: 0x0012BE65 File Offset: 0x0012A065
		[CompilerGenerated]
		private void <.ctor>b__0_0()
		{
			this.AddNewCustomPid();
		}

		// Token: 0x06001AF3 RID: 6899 RVA: 0x0012BE6D File Offset: 0x0012A06D
		[CompilerGenerated]
		private void <.ctor>b__0_1()
		{
			this.DeleteAllCustomPidsQuestion();
		}

		// Token: 0x06001AF4 RID: 6900 RVA: 0x0012BE78 File Offset: 0x0012A078
		private void __InitComponentRuntime()
		{
			Extensions.LoadFromXaml<SettingsCustomCodingsListPage>(this, typeof(SettingsCustomCodingsListPage));
			this.lv = NameScopeExtensions.FindByName<ListView>(this, "lv");
			this.btnImportFromFile = NameScopeExtensions.FindByName<Button>(this, "btnImportFromFile");
			this.btnExportAsFile = NameScopeExtensions.FindByName<Button>(this, "btnExportAsFile");
		}

		// Token: 0x04000C43 RID: 3139
		[CompilerGenerated]
		private CustomCodingsListViewModel <Model>k__BackingField;

		// Token: 0x04000C44 RID: 3140
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private ListView lv;

		// Token: 0x04000C45 RID: 3141
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private Button btnImportFromFile;

		// Token: 0x04000C46 RID: 3142
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private Button btnExportAsFile;

		// Token: 0x02000226 RID: 550
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <BtnExportAsFile_Clicked>d__2 : IAsyncStateMachine
		{
			// Token: 0x06001AF5 RID: 6901 RVA: 0x0012BECC File Offset: 0x0012A0CC
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				SettingsCustomCodingsListPage settingsCustomCodingsListPage = this;
				try
				{
					string text2;
					if (num != 0)
					{
						string cacheDirectory = FileSystem.CacheDirectory;
						string text = "customcodings.csc";
						text2 = Path.Combine(cacheDirectory, text);
						string text3 = JsonConvert.SerializeObject(settingsCustomCodingsListPage.Model);
						try
						{
							StreamWriter streamWriter = new StreamWriter(text2, false, Encoding.UTF8);
							try
							{
								streamWriter.WriteLine(text3);
								streamWriter.Flush();
							}
							finally
							{
								if (num < 0 && streamWriter != null)
								{
									((IDisposable)streamWriter).Dispose();
								}
							}
						}
						catch (Exception)
						{
						}
					}
					try
					{
						TaskAwaiter taskAwaiter;
						if (num != 0)
						{
							taskAwaiter = Share.RequestAsync(new ShareFileRequest
							{
								Title = "Custom Codings",
								File = new ShareFile(text2)
							}).GetAwaiter();
							if (!taskAwaiter.IsCompleted)
							{
								num = (num2 = 0);
								TaskAwaiter taskAwaiter2 = taskAwaiter;
								this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, SettingsCustomCodingsListPage.<BtnExportAsFile_Clicked>d__2>(ref taskAwaiter, ref this);
								return;
							}
						}
						else
						{
							TaskAwaiter taskAwaiter2;
							taskAwaiter = taskAwaiter2;
							taskAwaiter2 = default(TaskAwaiter);
							num = (num2 = -1);
						}
						taskAwaiter.GetResult();
					}
					catch (Exception)
					{
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

			// Token: 0x06001AF6 RID: 6902 RVA: 0x0012C020 File Offset: 0x0012A220
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x04000C47 RID: 3143
			public int <>1__state;

			// Token: 0x04000C48 RID: 3144
			public AsyncVoidMethodBuilder <>t__builder;

			// Token: 0x04000C49 RID: 3145
			public SettingsCustomCodingsListPage <>4__this;

			// Token: 0x04000C4A RID: 3146
			private TaskAwaiter <>u__1;
		}

		// Token: 0x02000227 RID: 551
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <BtnImportFromFile_Clicked>d__4 : IAsyncStateMachine
		{
			// Token: 0x06001AF7 RID: 6903 RVA: 0x0012C030 File Offset: 0x0012A230
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				SettingsCustomCodingsListPage settingsCustomCodingsListPage = this;
				try
				{
					try
					{
						TaskAwaiter<FileResult> taskAwaiter;
						if (num != 0)
						{
							taskAwaiter = FilePicker.PickAsync(null).GetAwaiter();
							if (!taskAwaiter.IsCompleted)
							{
								num = (num2 = 0);
								TaskAwaiter<FileResult> taskAwaiter2 = taskAwaiter;
								this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<FileResult>, SettingsCustomCodingsListPage.<BtnImportFromFile_Clicked>d__4>(ref taskAwaiter, ref this);
								return;
							}
						}
						else
						{
							TaskAwaiter<FileResult> taskAwaiter2;
							taskAwaiter = taskAwaiter2;
							taskAwaiter2 = default(TaskAwaiter<FileResult>);
							num = (num2 = -1);
						}
						FileResult result = taskAwaiter.GetResult();
						if (result != null && result.FileName.EndsWith("csc", StringComparison.OrdinalIgnoreCase))
						{
							StreamReader streamReader = new StreamReader(result.FullPath, Encoding.UTF8);
							try
							{
								List<CustomizableCodingTemplate>.Enumerator enumerator = JsonConvert.DeserializeObject<List<CustomizableCodingTemplate>>(streamReader.ReadToEnd()).GetEnumerator();
								try
								{
									while (enumerator.MoveNext())
									{
										CustomizableCodingTemplate customizableCodingTemplate = enumerator.Current;
										settingsCustomCodingsListPage.Model.Add(customizableCodingTemplate);
									}
								}
								finally
								{
									if (num < 0)
									{
										((IDisposable)enumerator).Dispose();
									}
								}
								settingsCustomCodingsListPage.Model.Save();
							}
							finally
							{
								if (num < 0 && streamReader != null)
								{
									((IDisposable)streamReader).Dispose();
								}
							}
						}
					}
					catch (Exception)
					{
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

			// Token: 0x06001AF8 RID: 6904 RVA: 0x0012C1A4 File Offset: 0x0012A3A4
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x04000C4B RID: 3147
			public int <>1__state;

			// Token: 0x04000C4C RID: 3148
			public AsyncVoidMethodBuilder <>t__builder;

			// Token: 0x04000C4D RID: 3149
			public SettingsCustomCodingsListPage <>4__this;

			// Token: 0x04000C4E RID: 3150
			private TaskAwaiter<FileResult> <>u__1;
		}

		// Token: 0x02000228 RID: 552
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <DeleteAllCustomPidsQuestion>d__10 : IAsyncStateMachine
		{
			// Token: 0x06001AF9 RID: 6905 RVA: 0x0012C1B4 File Offset: 0x0012A3B4
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				SettingsCustomCodingsListPage settingsCustomCodingsListPage = this;
				try
				{
					TaskAwaiter<bool> taskAwaiter3;
					if (num != 0)
					{
						taskAwaiter3 = settingsCustomCodingsListPage.DisplayAlert(Translate.GetString("ios_PidListDeleteAllCustomTitle"), Translate.GetString("ios_PidListDeleteAllCustomText"), Translate.GetString("ios_Yes"), Translate.GetString("ios_No")).GetAwaiter();
						if (!taskAwaiter3.IsCompleted)
						{
							num2 = 0;
							taskAwaiter2 = taskAwaiter3;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<bool>, SettingsCustomCodingsListPage.<DeleteAllCustomPidsQuestion>d__10>(ref taskAwaiter3, ref this);
							return;
						}
					}
					else
					{
						taskAwaiter3 = taskAwaiter2;
						taskAwaiter2 = default(TaskAwaiter<bool>);
						num2 = -1;
					}
					if (taskAwaiter3.GetResult())
					{
						settingsCustomCodingsListPage.Model.Clear();
						settingsCustomCodingsListPage.Model.Save();
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

			// Token: 0x06001AFA RID: 6906 RVA: 0x0012C2A8 File Offset: 0x0012A4A8
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x04000C4F RID: 3151
			public int <>1__state;

			// Token: 0x04000C50 RID: 3152
			public AsyncVoidMethodBuilder <>t__builder;

			// Token: 0x04000C51 RID: 3153
			public SettingsCustomCodingsListPage <>4__this;

			// Token: 0x04000C52 RID: 3154
			private TaskAwaiter<bool> <>u__1;
		}

		// Token: 0x02000229 RID: 553
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <btnDel_Clicked>d__3 : IAsyncStateMachine
		{
			// Token: 0x06001AFB RID: 6907 RVA: 0x0012C2B8 File Offset: 0x0012A4B8
			void IAsyncStateMachine.MoveNext()
			{
				SettingsCustomCodingsListPage settingsCustomCodingsListPage = this;
				try
				{
					settingsCustomCodingsListPage.lv.IsEnabled = false;
					CustomizableCodingTemplate customizableCodingTemplate = (sender as MenuItem).BindingContext as CustomizableCodingTemplate;
					try
					{
						settingsCustomCodingsListPage.Model.Remove(customizableCodingTemplate);
						settingsCustomCodingsListPage.Model.Save();
					}
					catch
					{
					}
					settingsCustomCodingsListPage.lv.IsEnabled = true;
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

			// Token: 0x06001AFC RID: 6908 RVA: 0x0012C360 File Offset: 0x0012A560
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x04000C53 RID: 3155
			public int <>1__state;

			// Token: 0x04000C54 RID: 3156
			public AsyncVoidMethodBuilder <>t__builder;

			// Token: 0x04000C55 RID: 3157
			public SettingsCustomCodingsListPage <>4__this;

			// Token: 0x04000C56 RID: 3158
			public object sender;
		}

		// Token: 0x0200022A RID: 554
		[CompilerGenerated]
		private sealed class <InitializeComponent>_anonXamlCDataTemplate_90
		{
			// Token: 0x06001AFD RID: 6909 RVA: 0x0012C370 File Offset: 0x0012A570
			public <InitializeComponent>_anonXamlCDataTemplate_90()
			{
			}

			// Token: 0x06001AFE RID: 6910 RVA: 0x0012C384 File Offset: 0x0012A584
			internal object LoadDataTemplate()
			{
				BindingExtension bindingExtension;
				VisualDiagnostics.RegisterSourceInfo(bindingExtension = new BindingExtension(), new Uri("Settings\\SettingsCustomCodingsListPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 33, 35);
				BindingExtension bindingExtension2;
				VisualDiagnostics.RegisterSourceInfo(bindingExtension2 = new BindingExtension(), new Uri("Settings\\SettingsCustomCodingsListPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 33, 66);
				BindingExtension bindingExtension3;
				VisualDiagnostics.RegisterSourceInfo(bindingExtension3 = new BindingExtension(), new Uri("Settings\\SettingsCustomCodingsListPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 37, 37);
				Translate translate;
				VisualDiagnostics.RegisterSourceInfo(translate = new Translate(), new Uri("Settings\\SettingsCustomCodingsListPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 39, 37);
				MenuItem menuItem;
				VisualDiagnostics.RegisterSourceInfo(menuItem = new MenuItem(), new Uri("Settings\\SettingsCustomCodingsListPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 35, 34);
				TextCell textCell;
				VisualDiagnostics.RegisterSourceInfo(textCell = new TextCell(), new Uri("Settings\\SettingsCustomCodingsListPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 33, 26);
				NameScope nameScope = new NameScope();
				NameScope.SetNameScope(textCell, nameScope);
				bindingExtension.Path = "Description";
				BindingBase bindingBase = bindingExtension.ProvideValue(null);
				textCell.SetBinding(TextCell.DetailProperty, bindingBase);
				bindingExtension2.Path = "Name";
				BindingBase bindingBase2 = bindingExtension2.ProvideValue(null);
				textCell.SetBinding(TextCell.TextProperty, bindingBase2);
				menuItem.Clicked += this.root.btnDel_Clicked;
				bindingExtension3.Path = ".";
				BindingBase bindingBase3 = bindingExtension3.ProvideValue(null);
				menuItem.SetBinding(MenuItem.CommandParameterProperty, bindingBase3);
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
				array2[1] = textCell;
				object obj;
				xamlServiceProvider.Add(typeFromHandle, obj = new SimpleValueTargetProvider(array2, MenuItem.TextProperty, nameScope));
				xamlServiceProvider.Add(typeof(IReferenceProvider), obj);
				Type typeFromHandle2 = typeof(IXamlTypeResolver);
				XmlNamespaceResolver xmlNamespaceResolver = new XmlNamespaceResolver();
				xmlNamespaceResolver.Add("", "http://xamarin.com/schemas/2014/forms");
				xmlNamespaceResolver.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
				xmlNamespaceResolver.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
				xmlNamespaceResolver.Add("local", "clr-namespace:CarScannerXamarinForms");
				xmlNamespaceResolver.Add("settings", "clr-namespace:CarScannerXamarinForms.Settings");
				xamlServiceProvider.Add(typeFromHandle2, new XamlTypeResolver(xmlNamespaceResolver, typeof(SettingsCustomCodingsListPage.<InitializeComponent>_anonXamlCDataTemplate_90).GetTypeInfo().Assembly));
				xamlServiceProvider.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(39, 37)));
				object obj2 = markupExtension.ProvideValue(xamlServiceProvider);
				menuItem.Text = obj2;
				textCell.ContextActions.Add(menuItem);
				return textCell;
			}

			// Token: 0x04000C57 RID: 3159
			internal object[] parentValues;

			// Token: 0x04000C58 RID: 3160
			internal SettingsCustomCodingsListPage root;
		}
	}
}
