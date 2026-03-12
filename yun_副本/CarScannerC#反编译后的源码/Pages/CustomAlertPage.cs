using System;
using System.CodeDom.Compiler;
using System.Diagnostics;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Internals;
using Xamarin.Forms.PlatformConfiguration.iOSSpecific;
using Xamarin.Forms.Xaml;
using Xamarin.Forms.Xaml.Diagnostics;
using Xamarin.Forms.Xaml.Internals;

namespace CarScannerXamarinForms.Pages
{
	// Token: 0x02000616 RID: 1558
	[XamlCompilation(2)]
	[XamlFilePath("Pages\\CustomAlertPage.xaml")]
	public class CustomAlertPage : ContentPage
	{
		// Token: 0x060036BE RID: 14014 RVA: 0x00287F70 File Offset: 0x00286170
		private CustomAlertPage(string title, string cancel, string destruction, TaskCompletionSource<string> taskCompletion, params string[] buttons)
		{
			this.InitializeComponent();
			this.cancelTitle = cancel;
			this.taskCompletion = taskCompletion;
			base.Title = title;
			this.labelTitle.Text = title;
			NavigationPage.SetBackButtonTitle(this, cancel);
			foreach (string text in buttons)
			{
				Button button = new Button
				{
					Text = text
				};
				button.Clicked -= this.button_Clicked;
				button.Clicked += this.button_Clicked;
				this.layoutRoot.Children.Add(button);
			}
			if (destruction != null)
			{
				Button button2 = new Button
				{
					Text = destruction,
					BackgroundColor = Color.Red,
					TextColor = Color.White
				};
				button2.Clicked -= this.button_Clicked;
				button2.Clicked += this.button_Clicked;
				this.layoutRoot.Children.Add(button2);
			}
			if (cancel != null)
			{
				Button button3 = new Button
				{
					Text = cancel
				};
				button3.Clicked -= this.button_Clicked;
				button3.Clicked += this.button_Clicked;
				this.layoutRoot.Children.Add(button3);
			}
		}

		// Token: 0x060036BF RID: 14015 RVA: 0x002880B1 File Offset: 0x002862B1
		protected override bool OnBackButtonPressed()
		{
			base.Navigation.PopModalAsync();
			this.taskCompletion.SetResult(this.cancelTitle);
			return true;
		}

		// Token: 0x060036C0 RID: 14016 RVA: 0x002880D4 File Offset: 0x002862D4
		private async void button_Clicked(object sender, EventArgs e)
		{
			Button btnView = sender as Button;
			if (btnView != null)
			{
				await base.Navigation.PopModalAsync();
				this.taskCompletion.TrySetResult(btnView.Text);
			}
		}

		// Token: 0x060036C1 RID: 14017 RVA: 0x00288114 File Offset: 0x00286314
		public static async Task<string> DisplayActionSheetCustom(Page page, string title, string cancel, string destruction, params string[] buttons)
		{
			TaskCompletionSource<string> taskCompletionSource = new TaskCompletionSource<string>();
			CustomAlertPage customAlertPage = new CustomAlertPage(title, cancel, destruction, taskCompletionSource, buttons);
			page.Navigation.PushModalAsync(customAlertPage);
			return await taskCompletionSource.Task;
		}

		// Token: 0x060036C2 RID: 14018 RVA: 0x00288178 File Offset: 0x00286378
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private void InitializeComponent()
		{
			if (ResourceLoader.IsEnabled && ResourceLoader.CanProvideContentFor(new ResourceLoader.ResourceLoadingQuery
			{
				AssemblyName = typeof(CustomAlertPage).GetTypeInfo().Assembly.GetName(),
				ResourcePath = "Pages/CustomAlertPage.xaml",
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
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension = new DynamicResourceExtension(), new Uri("Pages\\CustomAlertPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 11, 5);
			DynamicResourceExtension dynamicResourceExtension2;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension2 = new DynamicResourceExtension(), new Uri("Pages\\CustomAlertPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 22, 21);
			Label label;
			VisualDiagnostics.RegisterSourceInfo(label = new Label(), new Uri("Pages\\CustomAlertPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 20, 18);
			StackLayout stackLayout;
			VisualDiagnostics.RegisterSourceInfo(stackLayout = new StackLayout(), new Uri("Pages\\CustomAlertPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 16, 14);
			ScrollView scrollView;
			VisualDiagnostics.RegisterSourceInfo(scrollView = new ScrollView(), new Uri("Pages\\CustomAlertPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 15, 10);
			VisualDiagnostics.RegisterSourceInfo(this, new Uri("Pages\\CustomAlertPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 2, 2);
			NameScope nameScope = NameScope.GetNameScope(this) ?? new NameScope();
			NameScope.SetNameScope(this, nameScope);
			nameScope.RegisterName("layoutRoot", stackLayout);
			if (stackLayout.StyleId == null)
			{
				stackLayout.StyleId = "layoutRoot";
			}
			nameScope.RegisterName("labelTitle", label);
			if (label.StyleId == null)
			{
				label.StyleId = "labelTitle";
			}
			this.layoutRoot = stackLayout;
			this.labelTitle = label;
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
			xmlNamespaceResolver.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver.Add("local", "clr-namespace:CarScannerXamarinForms");
			xamlServiceProvider.Add(typeFromHandle2, new XamlTypeResolver(xmlNamespaceResolver, typeof(CustomAlertPage).GetTypeInfo().Assembly));
			xamlServiceProvider.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(11, 5)));
			DynamicResource dynamicResource = markupExtension.ProvideValue(xamlServiceProvider);
			this.SetDynamicResource(VisualElement.BackgroundColorProperty, dynamicResource.Key);
			this.SetValue(NavigationPage.HasBackButtonProperty, false);
			this.SetValue(NavigationPage.HasNavigationBarProperty, true);
			scrollView.SetValue(ScrollView.OrientationProperty, 0);
			stackLayout.SetValue(StackLayout.SpacingProperty, 10.0);
			label.SetValue(View.MarginProperty, new Thickness(10.0, 20.0, 10.0, 10.0));
			dynamicResourceExtension2.Key = "BaseFontSize+++";
			IMarkupExtension<DynamicResource> markupExtension2 = dynamicResourceExtension2;
			XamlServiceProvider xamlServiceProvider2 = new XamlServiceProvider();
			Type typeFromHandle3 = typeof(IProvideValueTarget);
			object[] array2 = new object[0 + 4];
			array2[0] = label;
			array2[1] = stackLayout;
			array2[2] = scrollView;
			array2[3] = this;
			object obj2;
			xamlServiceProvider2.Add(typeFromHandle3, obj2 = new SimpleValueTargetProvider(array2, Label.FontSizeProperty, nameScope));
			xamlServiceProvider2.Add(typeof(IReferenceProvider), obj2);
			Type typeFromHandle4 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver2 = new XmlNamespaceResolver();
			xmlNamespaceResolver2.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver2.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver2.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver2.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver2.Add("local", "clr-namespace:CarScannerXamarinForms");
			xamlServiceProvider2.Add(typeFromHandle4, new XamlTypeResolver(xmlNamespaceResolver2, typeof(CustomAlertPage).GetTypeInfo().Assembly));
			xamlServiceProvider2.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(22, 21)));
			DynamicResource dynamicResource2 = markupExtension2.ProvideValue(xamlServiceProvider2);
			label.SetDynamicResource(Label.FontSizeProperty, dynamicResource2.Key);
			label.SetValue(View.HorizontalOptionsProperty, LayoutOptions.Center);
			label.SetValue(Label.HorizontalTextAlignmentProperty, new TextAlignmentConverter().ConvertFromInvariantString("Center"));
			stackLayout.Children.Add(label);
			scrollView.Content = stackLayout;
			this.SetValue(ContentPage.ContentProperty, scrollView);
		}

		// Token: 0x060036C3 RID: 14019 RVA: 0x00288667 File Offset: 0x00286867
		private void __InitComponentRuntime()
		{
			Extensions.LoadFromXaml<CustomAlertPage>(this, typeof(CustomAlertPage));
			this.layoutRoot = NameScopeExtensions.FindByName<StackLayout>(this, "layoutRoot");
			this.labelTitle = NameScopeExtensions.FindByName<Label>(this, "labelTitle");
		}

		// Token: 0x04002100 RID: 8448
		private string cancelTitle;

		// Token: 0x04002101 RID: 8449
		private TaskCompletionSource<string> taskCompletion;

		// Token: 0x04002102 RID: 8450
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private StackLayout layoutRoot;

		// Token: 0x04002103 RID: 8451
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private Label labelTitle;

		// Token: 0x02000617 RID: 1559
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <DisplayActionSheetCustom>d__5 : IAsyncStateMachine
		{
			// Token: 0x060036C4 RID: 14020 RVA: 0x0028869C File Offset: 0x0028689C
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				string result;
				try
				{
					TaskAwaiter<string> taskAwaiter;
					if (num != 0)
					{
						TaskCompletionSource<string> taskCompletionSource = new TaskCompletionSource<string>();
						CustomAlertPage customAlertPage = new CustomAlertPage(title, cancel, destruction, taskCompletionSource, buttons);
						page.Navigation.PushModalAsync(customAlertPage);
						taskAwaiter = taskCompletionSource.Task.GetAwaiter();
						if (!taskAwaiter.IsCompleted)
						{
							num2 = 0;
							TaskAwaiter<string> taskAwaiter2 = taskAwaiter;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<string>, CustomAlertPage.<DisplayActionSheetCustom>d__5>(ref taskAwaiter, ref this);
							return;
						}
					}
					else
					{
						TaskAwaiter<string> taskAwaiter2;
						taskAwaiter = taskAwaiter2;
						taskAwaiter2 = default(TaskAwaiter<string>);
						num2 = -1;
					}
					result = taskAwaiter.GetResult();
				}
				catch (Exception ex)
				{
					num2 = -2;
					this.<>t__builder.SetException(ex);
					return;
				}
				num2 = -2;
				this.<>t__builder.SetResult(result);
			}

			// Token: 0x060036C5 RID: 14021 RVA: 0x00288788 File Offset: 0x00286988
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x04002104 RID: 8452
			public int <>1__state;

			// Token: 0x04002105 RID: 8453
			public AsyncTaskMethodBuilder<string> <>t__builder;

			// Token: 0x04002106 RID: 8454
			public string title;

			// Token: 0x04002107 RID: 8455
			public string cancel;

			// Token: 0x04002108 RID: 8456
			public string destruction;

			// Token: 0x04002109 RID: 8457
			public string[] buttons;

			// Token: 0x0400210A RID: 8458
			public Page page;

			// Token: 0x0400210B RID: 8459
			private TaskAwaiter<string> <>u__1;
		}

		// Token: 0x02000618 RID: 1560
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <button_Clicked>d__3 : IAsyncStateMachine
		{
			// Token: 0x060036C6 RID: 14022 RVA: 0x00288798 File Offset: 0x00286998
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				CustomAlertPage customAlertPage = this;
				try
				{
					TaskAwaiter<Page> taskAwaiter;
					if (num != 0)
					{
						btnView = sender as Button;
						if (btnView == null)
						{
							goto IL_009F;
						}
						taskAwaiter = customAlertPage.Navigation.PopModalAsync().GetAwaiter();
						if (!taskAwaiter.IsCompleted)
						{
							num2 = 0;
							TaskAwaiter<Page> taskAwaiter2 = taskAwaiter;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<Page>, CustomAlertPage.<button_Clicked>d__3>(ref taskAwaiter, ref this);
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
					customAlertPage.taskCompletion.TrySetResult(btnView.Text);
					IL_009F:;
				}
				catch (Exception ex)
				{
					num2 = -2;
					btnView = null;
					this.<>t__builder.SetException(ex);
					return;
				}
				num2 = -2;
				btnView = null;
				this.<>t__builder.SetResult();
			}

			// Token: 0x060036C7 RID: 14023 RVA: 0x00288890 File Offset: 0x00286A90
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x0400210C RID: 8460
			public int <>1__state;

			// Token: 0x0400210D RID: 8461
			public AsyncVoidMethodBuilder <>t__builder;

			// Token: 0x0400210E RID: 8462
			public object sender;

			// Token: 0x0400210F RID: 8463
			public CustomAlertPage <>4__this;

			// Token: 0x04002110 RID: 8464
			private Button <btnView>5__2;

			// Token: 0x04002111 RID: 8465
			private TaskAwaiter<Page> <>u__1;
		}
	}
}
