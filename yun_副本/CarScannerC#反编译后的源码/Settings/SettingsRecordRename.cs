using System;
using System.CodeDom.Compiler;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using CarScannerXamarinForms.Common;
using Xamarin.Forms;
using Xamarin.Forms.Internals;
using Xamarin.Forms.PlatformConfiguration.iOSSpecific;
using Xamarin.Forms.Xaml;
using Xamarin.Forms.Xaml.Diagnostics;
using Xamarin.Forms.Xaml.Internals;

namespace CarScannerXamarinForms.Settings
{
	// Token: 0x0200026A RID: 618
	[XamlFilePath("Settings\\SettingsRecordRename.xaml")]
	public class SettingsRecordRename : ContentPage
	{
		// Token: 0x06001C1B RID: 7195 RVA: 0x00143140 File Offset: 0x00141340
		public SettingsRecordRename(FileSystemElement file, Action UpdateAction)
		{
			this.InitializeComponent();
			this.file = file;
			string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(file.Name);
			this.entryName.Text = fileNameWithoutExtension;
			this.updateAction = UpdateAction;
		}

		// Token: 0x06001C1C RID: 7196 RVA: 0x000027D4 File Offset: 0x000009D4
		private void Handle_SizeChanged(object sender, EventArgs e)
		{
		}

		// Token: 0x06001C1D RID: 7197 RVA: 0x000027D4 File Offset: 0x000009D4
		private void Page_Disappearing(object sender, EventArgs e)
		{
		}

		// Token: 0x06001C1E RID: 7198 RVA: 0x00143180 File Offset: 0x00141380
		private async void btnOK_Clicked(object sender, EventArgs e)
		{
			string fn = this.entryName.Text;
			if (fn == this.file.Name)
			{
				await base.Navigation.PopAsync();
			}
			else
			{
				if (SettingsRecordRename.FilePathHasInvalidChars(fn))
				{
					await base.DisplayAlert("Wrong filename", "Filename contains characters, that are not allowed", "OK");
				}
				string extension = Path.GetExtension(this.file.Name);
				string fn_with_ext = fn + extension;
				if (File.Exists(Path.Combine(Path.GetDirectoryName(this.file.Path), fn_with_ext)))
				{
					TaskAwaiter<bool> taskAwaiter = base.DisplayAlert("File exists", "File with the same name (" + fn_with_ext + ") already exists.\nDo you want to overrite it?", "OK", Translate.GetString("btnCancel.Content")).GetAwaiter();
					if (!taskAwaiter.IsCompleted)
					{
						await taskAwaiter;
						TaskAwaiter<bool> taskAwaiter2;
						taskAwaiter = taskAwaiter2;
						taskAwaiter2 = default(TaskAwaiter<bool>);
					}
					if (!taskAwaiter.GetResult())
					{
						return;
					}
				}
				int num = 0;
				try
				{
					this.file.Rename(fn_with_ext);
					await base.Navigation.PopAsync();
					this.updateAction();
				}
				catch
				{
					num = 1;
				}
				if (num == 1)
				{
					await base.DisplayAlert("Rename failed :(", "File with the same name already exists OR it's locked by another proccess", "OK");
				}
			}
		}

		// Token: 0x06001C1F RID: 7199 RVA: 0x001431B8 File Offset: 0x001413B8
		public static bool FilePathHasInvalidChars(string path)
		{
			char[] array = new char[]
			{
				'<', '>', '?', '\\', '|', '/', '\'', '"', '*', '[',
				']', ':', ';', '=', '+'
			};
			return !string.IsNullOrEmpty(path) && path.IndexOfAny(array) >= 0;
		}

		// Token: 0x06001C20 RID: 7200 RVA: 0x001431F0 File Offset: 0x001413F0
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private void InitializeComponent()
		{
			if (ResourceLoader.IsEnabled && ResourceLoader.CanProvideContentFor(new ResourceLoader.ResourceLoadingQuery
			{
				AssemblyName = typeof(SettingsRecordRename).GetTypeInfo().Assembly.GetName(),
				ResourcePath = "Settings/SettingsRecordRename.xaml",
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
			VisualDiagnostics.RegisterSourceInfo(translate = new Translate(), new Uri("Settings\\SettingsRecordRename.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 8, 5);
			DynamicResourceExtension dynamicResourceExtension;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension = new DynamicResourceExtension(), new Uri("Settings\\SettingsRecordRename.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 11, 5);
			BoolToNegativeConverter boolToNegativeConverter;
			VisualDiagnostics.RegisterSourceInfo(boolToNegativeConverter = new BoolToNegativeConverter(), new Uri("Settings\\SettingsRecordRename.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 16, 14);
			ResourceDictionary resourceDictionary;
			VisualDiagnostics.RegisterSourceInfo(resourceDictionary = new ResourceDictionary(), new Uri("Settings\\SettingsRecordRename.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 15, 10);
			OnPlatform<Thickness> onPlatform;
			VisualDiagnostics.RegisterSourceInfo(onPlatform = new OnPlatform<Thickness>(), new Uri("Settings\\SettingsRecordRename.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 23, 18);
			RowDefinition rowDefinition;
			VisualDiagnostics.RegisterSourceInfo(rowDefinition = new RowDefinition(), new Uri("Settings\\SettingsRecordRename.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 29, 18);
			RowDefinition rowDefinition2;
			VisualDiagnostics.RegisterSourceInfo(rowDefinition2 = new RowDefinition(), new Uri("Settings\\SettingsRecordRename.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 30, 18);
			Label label;
			VisualDiagnostics.RegisterSourceInfo(label = new Label(), new Uri("Settings\\SettingsRecordRename.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 36, 18);
			Entry entry;
			VisualDiagnostics.RegisterSourceInfo(entry = new Entry(), new Uri("Settings\\SettingsRecordRename.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 37, 18);
			StackLayout stackLayout;
			VisualDiagnostics.RegisterSourceInfo(stackLayout = new StackLayout(), new Uri("Settings\\SettingsRecordRename.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 32, 14);
			Button button;
			VisualDiagnostics.RegisterSourceInfo(button = new Button(), new Uri("Settings\\SettingsRecordRename.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 41, 14);
			Grid grid;
			VisualDiagnostics.RegisterSourceInfo(grid = new Grid(), new Uri("Settings\\SettingsRecordRename.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 21, 10);
			VisualDiagnostics.RegisterSourceInfo(this, new Uri("Settings\\SettingsRecordRename.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 2, 2);
			NameScope nameScope = NameScope.GetNameScope(this) ?? new NameScope();
			NameScope.SetNameScope(this, nameScope);
			nameScope.RegisterName("stack", stackLayout);
			if (stackLayout.StyleId == null)
			{
				stackLayout.StyleId = "stack";
			}
			nameScope.RegisterName("entryName", entry);
			if (entry.StyleId == null)
			{
				entry.StyleId = "entryName";
			}
			nameScope.RegisterName("btnOK", button);
			if (button.StyleId == null)
			{
				button.StyleId = "btnOK";
			}
			this.stack = stackLayout;
			this.entryName = entry;
			this.btnOK = button;
			this.Resources = resourceDictionary;
			resourceDictionary.Add("BoolToNegativeConverter", boolToNegativeConverter);
			translate.Text = "Settings_Control_DataRecording.Content";
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
			xamlServiceProvider.Add(typeFromHandle2, new XamlTypeResolver(xmlNamespaceResolver, typeof(SettingsRecordRename).GetTypeInfo().Assembly));
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
			xamlServiceProvider2.Add(typeFromHandle4, new XamlTypeResolver(xmlNamespaceResolver2, typeof(SettingsRecordRename).GetTypeInfo().Assembly));
			xamlServiceProvider2.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(11, 5)));
			DynamicResource dynamicResource = markupExtension2.ProvideValue(xamlServiceProvider2);
			this.SetDynamicResource(VisualElement.BackgroundColorProperty, dynamicResource.Key);
			this.Disappearing += this.Page_Disappearing;
			this.SizeChanged += this.Handle_SizeChanged;
			this.Resources = resourceDictionary;
			onPlatform.Android = new Thickness(5.0, 0.0, 5.0, 0.0);
			onPlatform.iOS = new Thickness(5.0, 0.0, 5.0, 0.0);
			grid.SetValue(View.MarginProperty, onPlatform);
			rowDefinition.SetValue(RowDefinition.HeightProperty, new GridLengthTypeConverter().ConvertFromInvariantString("auto"));
			grid.GetValue(Grid.RowDefinitionsProperty).Add(rowDefinition);
			rowDefinition2.SetValue(RowDefinition.HeightProperty, new GridLengthTypeConverter().ConvertFromInvariantString("*"));
			grid.GetValue(Grid.RowDefinitionsProperty).Add(rowDefinition2);
			stackLayout.SetValue(Grid.RowProperty, 0);
			stackLayout.SetValue(StackLayout.OrientationProperty, 0);
			label.SetValue(Label.TextProperty, "Choose name:");
			stackLayout.Children.Add(label);
			stackLayout.Children.Add(entry);
			grid.Children.Add(stackLayout);
			button.SetValue(Grid.RowProperty, 1);
			button.SetValue(Grid.ColumnProperty, 0);
			button.Clicked += this.btnOK_Clicked;
			button.SetValue(Button.TextProperty, "OK");
			button.SetValue(View.VerticalOptionsProperty, LayoutOptions.End);
			grid.Children.Add(button);
			this.SetValue(ContentPage.ContentProperty, grid);
		}

		// Token: 0x06001C21 RID: 7201 RVA: 0x0014392C File Offset: 0x00141B2C
		private void __InitComponentRuntime()
		{
			Extensions.LoadFromXaml<SettingsRecordRename>(this, typeof(SettingsRecordRename));
			this.stack = NameScopeExtensions.FindByName<StackLayout>(this, "stack");
			this.entryName = NameScopeExtensions.FindByName<Entry>(this, "entryName");
			this.btnOK = NameScopeExtensions.FindByName<Button>(this, "btnOK");
		}

		// Token: 0x04000D67 RID: 3431
		private Action updateAction;

		// Token: 0x04000D68 RID: 3432
		private FileSystemElement file;

		// Token: 0x04000D69 RID: 3433
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private StackLayout stack;

		// Token: 0x04000D6A RID: 3434
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private Entry entryName;

		// Token: 0x04000D6B RID: 3435
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private Button btnOK;

		// Token: 0x0200026B RID: 619
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <btnOK_Clicked>d__5 : IAsyncStateMachine
		{
			// Token: 0x06001C22 RID: 7202 RVA: 0x00143980 File Offset: 0x00141B80
			void IAsyncStateMachine.MoveNext()
			{
				int num3;
				int num2 = num3;
				SettingsRecordRename settingsRecordRename = this;
				try
				{
					TaskAwaiter<Page> taskAwaiter3;
					TaskAwaiter taskAwaiter5;
					TaskAwaiter<bool> taskAwaiter7;
					switch (num2)
					{
					case 0:
					{
						TaskAwaiter<Page> taskAwaiter4;
						taskAwaiter3 = taskAwaiter4;
						taskAwaiter4 = default(TaskAwaiter<Page>);
						num3 = -1;
						break;
					}
					case 1:
					{
						TaskAwaiter taskAwaiter6;
						taskAwaiter5 = taskAwaiter6;
						taskAwaiter6 = default(TaskAwaiter);
						num2 = (num3 = -1);
						goto IL_012A;
					}
					case 2:
						taskAwaiter7 = taskAwaiter2;
						taskAwaiter2 = default(TaskAwaiter<bool>);
						num2 = (num3 = -1);
						goto IL_01F9;
					case 3:
					{
						IL_020E:
						try
						{
							if (num2 != 3)
							{
								settingsRecordRename.file.Rename(fn_with_ext);
								taskAwaiter3 = settingsRecordRename.Navigation.PopAsync().GetAwaiter();
								if (!taskAwaiter3.IsCompleted)
								{
									num3 = 3;
									TaskAwaiter<Page> taskAwaiter4 = taskAwaiter3;
									this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<Page>, SettingsRecordRename.<btnOK_Clicked>d__5>(ref taskAwaiter3, ref this);
									return;
								}
							}
							else
							{
								TaskAwaiter<Page> taskAwaiter4;
								taskAwaiter3 = taskAwaiter4;
								taskAwaiter4 = default(TaskAwaiter<Page>);
								num3 = -1;
							}
							taskAwaiter3.GetResult();
							settingsRecordRename.updateAction();
						}
						catch
						{
							num = 1;
						}
						int num4 = num;
						if (num4 != 1)
						{
							goto IL_0313;
						}
						taskAwaiter5 = settingsRecordRename.DisplayAlert("Rename failed :(", "File with the same name already exists OR it's locked by another proccess", "OK").GetAwaiter();
						if (!taskAwaiter5.IsCompleted)
						{
							num3 = 4;
							TaskAwaiter taskAwaiter6 = taskAwaiter5;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, SettingsRecordRename.<btnOK_Clicked>d__5>(ref taskAwaiter5, ref this);
							return;
						}
						goto IL_030C;
					}
					case 4:
					{
						TaskAwaiter taskAwaiter6;
						taskAwaiter5 = taskAwaiter6;
						taskAwaiter6 = default(TaskAwaiter);
						num3 = -1;
						goto IL_030C;
					}
					default:
						fn = settingsRecordRename.entryName.Text;
						if (fn == settingsRecordRename.file.Name)
						{
							taskAwaiter3 = settingsRecordRename.Navigation.PopAsync().GetAwaiter();
							if (!taskAwaiter3.IsCompleted)
							{
								num3 = 0;
								TaskAwaiter<Page> taskAwaiter4 = taskAwaiter3;
								this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<Page>, SettingsRecordRename.<btnOK_Clicked>d__5>(ref taskAwaiter3, ref this);
								return;
							}
						}
						else
						{
							if (!SettingsRecordRename.FilePathHasInvalidChars(fn))
							{
								goto IL_0131;
							}
							taskAwaiter5 = settingsRecordRename.DisplayAlert("Wrong filename", "Filename contains characters, that are not allowed", "OK").GetAwaiter();
							if (!taskAwaiter5.IsCompleted)
							{
								num3 = 1;
								TaskAwaiter taskAwaiter6 = taskAwaiter5;
								this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, SettingsRecordRename.<btnOK_Clicked>d__5>(ref taskAwaiter5, ref this);
								return;
							}
							goto IL_012A;
						}
						break;
					}
					taskAwaiter3.GetResult();
					goto IL_033C;
					IL_012A:
					taskAwaiter5.GetResult();
					IL_0131:
					string extension = Path.GetExtension(settingsRecordRename.file.Name);
					fn_with_ext = fn + extension;
					if (!File.Exists(Path.Combine(Path.GetDirectoryName(settingsRecordRename.file.Path), fn_with_ext)))
					{
						goto IL_0207;
					}
					taskAwaiter7 = settingsRecordRename.DisplayAlert("File exists", "File with the same name (" + fn_with_ext + ") already exists.\nDo you want to overrite it?", "OK", Translate.GetString("btnCancel.Content")).GetAwaiter();
					if (!taskAwaiter7.IsCompleted)
					{
						num3 = 2;
						taskAwaiter2 = taskAwaiter7;
						this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<bool>, SettingsRecordRename.<btnOK_Clicked>d__5>(ref taskAwaiter7, ref this);
						return;
					}
					IL_01F9:
					if (!taskAwaiter7.GetResult())
					{
						goto IL_033C;
					}
					IL_0207:
					num = 0;
					goto IL_020E;
					IL_030C:
					taskAwaiter5.GetResult();
					IL_0313:;
				}
				catch (Exception ex)
				{
					num3 = -2;
					fn = null;
					fn_with_ext = null;
					this.<>t__builder.SetException(ex);
					return;
				}
				IL_033C:
				num3 = -2;
				fn = null;
				fn_with_ext = null;
				this.<>t__builder.SetResult();
			}

			// Token: 0x06001C23 RID: 7203 RVA: 0x00143D20 File Offset: 0x00141F20
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x04000D6C RID: 3436
			public int <>1__state;

			// Token: 0x04000D6D RID: 3437
			public AsyncVoidMethodBuilder <>t__builder;

			// Token: 0x04000D6E RID: 3438
			public SettingsRecordRename <>4__this;

			// Token: 0x04000D6F RID: 3439
			private string <fn>5__2;

			// Token: 0x04000D70 RID: 3440
			private string <fn_with_ext>5__3;

			// Token: 0x04000D71 RID: 3441
			private TaskAwaiter<Page> <>u__1;

			// Token: 0x04000D72 RID: 3442
			private TaskAwaiter <>u__2;

			// Token: 0x04000D73 RID: 3443
			private TaskAwaiter<bool> <>u__3;

			// Token: 0x04000D74 RID: 3444
			private int <>7__wrap3;
		}
	}
}
