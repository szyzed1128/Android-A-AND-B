using System;
using System.CodeDom.Compiler;
using System.Diagnostics;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using CarScannerXamarinForms.Settings;
using Xamarin.Forms;
using Xamarin.Forms.Internals;
using Xamarin.Forms.PlatformConfiguration.iOSSpecific;
using Xamarin.Forms.Xaml;
using Xamarin.Forms.Xaml.Diagnostics;
using Xamarin.Forms.Xaml.Internals;

namespace CarScannerXamarinForms.Pages
{
	// Token: 0x02000669 RID: 1641
	[XamlCompilation(2)]
	[XamlFilePath("Pages\\SpeedTestPageV2.xaml")]
	public class SpeedTestPageV2 : ContentPage
	{
		// Token: 0x0600385B RID: 14427 RVA: 0x002A9672 File Offset: 0x002A7872
		public SpeedTestPageV2()
		{
			this.InitializeComponent();
		}

		// Token: 0x0600385C RID: 14428 RVA: 0x002A9680 File Offset: 0x002A7880
		private async void Page_Appearing(object sender, EventArgs e)
		{
			if (!SharedSettings.Current.InfoShowed_SpeedTest)
			{
				await Task.Delay(1000);
				if (App.GetCurrentPage() == this)
				{
					SharedSettings.Current.InfoShowed_SpeedTest = true;
					this.btnInfo_Clicked(null, null);
				}
			}
		}

		// Token: 0x0600385D RID: 14429 RVA: 0x0005DCF6 File Offset: 0x0005BEF6
		private void btnInfo_Clicked(object sender, EventArgs e)
		{
			base.DisplayAlert(Translate.GetString("ios_MainPage_TileSpeedTest"), Translate.GetString("ios_SpeedTest_InfoText"), "OK");
		}

		// Token: 0x0600385E RID: 14430 RVA: 0x002A96B8 File Offset: 0x002A78B8
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private void InitializeComponent()
		{
			if (ResourceLoader.IsEnabled && ResourceLoader.CanProvideContentFor(new ResourceLoader.ResourceLoadingQuery
			{
				AssemblyName = typeof(SpeedTestPageV2).GetTypeInfo().Assembly.GetName(),
				ResourcePath = "Pages/SpeedTestPageV2.xaml",
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
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension = new DynamicResourceExtension(), new Uri("Pages\\SpeedTestPageV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 11, 5);
			Label label;
			VisualDiagnostics.RegisterSourceInfo(label = new Label(), new Uri("Pages\\SpeedTestPageV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 16, 14);
			StackLayout stackLayout;
			VisualDiagnostics.RegisterSourceInfo(stackLayout = new StackLayout(), new Uri("Pages\\SpeedTestPageV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 15, 10);
			VisualDiagnostics.RegisterSourceInfo(this, new Uri("Pages\\SpeedTestPageV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 2, 2);
			NameScope nameScope = NameScope.GetNameScope(this) ?? new NameScope();
			NameScope.SetNameScope(this, nameScope);
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
			xmlNamespaceResolver.Add("chart", "clr-namespace:Syncfusion.SfChart.XForms;assembly=Syncfusion.SfChart.XForms");
			xmlNamespaceResolver.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver.Add("local", "clr-namespace:CarScannerXamarinForms");
			xamlServiceProvider.Add(typeFromHandle2, new XamlTypeResolver(xmlNamespaceResolver, typeof(SpeedTestPageV2).GetTypeInfo().Assembly));
			xamlServiceProvider.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(11, 5)));
			DynamicResource dynamicResource = markupExtension.ProvideValue(xamlServiceProvider);
			this.SetDynamicResource(VisualElement.BackgroundColorProperty, dynamicResource.Key);
			this.SetValue(NavigationPage.HasBackButtonProperty, false);
			this.SetValue(NavigationPage.HasNavigationBarProperty, true);
			label.SetValue(View.HorizontalOptionsProperty, LayoutOptions.CenterAndExpand);
			label.SetValue(Label.TextProperty, "Welcome to Xamarin.Forms!");
			label.SetValue(View.VerticalOptionsProperty, LayoutOptions.CenterAndExpand);
			stackLayout.Children.Add(label);
			this.SetValue(ContentPage.ContentProperty, stackLayout);
		}

		// Token: 0x0600385F RID: 14431 RVA: 0x002A9985 File Offset: 0x002A7B85
		private void __InitComponentRuntime()
		{
			Extensions.LoadFromXaml<SpeedTestPageV2>(this, typeof(SpeedTestPageV2));
		}

		// Token: 0x0200066A RID: 1642
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <Page_Appearing>d__1 : IAsyncStateMachine
		{
			// Token: 0x06003860 RID: 14432 RVA: 0x002A9998 File Offset: 0x002A7B98
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				SpeedTestPageV2 speedTestPageV = this;
				try
				{
					TaskAwaiter taskAwaiter;
					if (num != 0)
					{
						if (SharedSettings.Current.InfoShowed_SpeedTest)
						{
							goto IL_0094;
						}
						taskAwaiter = Task.Delay(1000).GetAwaiter();
						if (!taskAwaiter.IsCompleted)
						{
							num2 = 0;
							TaskAwaiter taskAwaiter2 = taskAwaiter;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, SpeedTestPageV2.<Page_Appearing>d__1>(ref taskAwaiter, ref this);
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
					if (App.GetCurrentPage() == speedTestPageV)
					{
						SharedSettings.Current.InfoShowed_SpeedTest = true;
						speedTestPageV.btnInfo_Clicked(null, null);
					}
					IL_0094:;
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

			// Token: 0x06003861 RID: 14433 RVA: 0x002A9A78 File Offset: 0x002A7C78
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x04002244 RID: 8772
			public int <>1__state;

			// Token: 0x04002245 RID: 8773
			public AsyncVoidMethodBuilder <>t__builder;

			// Token: 0x04002246 RID: 8774
			public SpeedTestPageV2 <>4__this;

			// Token: 0x04002247 RID: 8775
			private TaskAwaiter <>u__1;
		}
	}
}
