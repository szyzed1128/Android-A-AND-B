using System;
using System.CodeDom.Compiler;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using CarScannerXamarinForms.OBD2.PIDS;
using CarScannerXamarinForms.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Internals;
using Xamarin.Forms.Xaml;
using Xamarin.Forms.Xaml.Diagnostics;
using Xamarin.Forms.Xaml.Internals;

namespace CarScannerXamarinForms
{
	// Token: 0x02000145 RID: 325
	[XamlFilePath("Pages\\TestPage.xaml")]
	public class TestPage : ContentPage
	{
		// Token: 0x06000615 RID: 1557 RVA: 0x00065EF8 File Offset: 0x000640F8
		public TestPage()
		{
			this.InitializeComponent();
			LiveDataPIDModel liveDataPIDModel = new LiveDataPIDModel();
			liveDataPIDModel.SelectedPID = LiveDataPIDModel._PIDCollection.FirstOrDefault((PID x) => x is IPIDFloatValue);
			this.ldc.BindingContext = liveDataPIDModel;
		}

		// Token: 0x06000616 RID: 1558 RVA: 0x00065F54 File Offset: 0x00064154
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private void InitializeComponent()
		{
			if (ResourceLoader.IsEnabled && ResourceLoader.CanProvideContentFor(new ResourceLoader.ResourceLoadingQuery
			{
				AssemblyName = typeof(TestPage).GetTypeInfo().Assembly.GetName(),
				ResourcePath = "Pages/TestPage.xaml",
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
			VisualDiagnostics.RegisterSourceInfo(translate = new Translate(), new Uri("Pages\\TestPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 9, 5);
			ExcludeInfinityFromDoubleConverter excludeInfinityFromDoubleConverter;
			VisualDiagnostics.RegisterSourceInfo(excludeInfinityFromDoubleConverter = new ExcludeInfinityFromDoubleConverter(), new Uri("Pages\\TestPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 12, 14);
			BoolToNegativeConverter boolToNegativeConverter;
			VisualDiagnostics.RegisterSourceInfo(boolToNegativeConverter = new BoolToNegativeConverter(), new Uri("Pages\\TestPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 13, 14);
			ResourceDictionary resourceDictionary;
			VisualDiagnostics.RegisterSourceInfo(resourceDictionary = new ResourceDictionary(), new Uri("Pages\\TestPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 11, 10);
			OnPlatform<Thickness> onPlatform;
			VisualDiagnostics.RegisterSourceInfo(onPlatform = new OnPlatform<Thickness>(), new Uri("Pages\\TestPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 17, 10);
			LiveDataChartUC liveDataChartUC;
			VisualDiagnostics.RegisterSourceInfo(liveDataChartUC = new LiveDataChartUC(), new Uri("Pages\\TestPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 25, 10);
			VisualDiagnostics.RegisterSourceInfo(this, new Uri("Pages\\TestPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 2, 2);
			NameScope nameScope = NameScope.GetNameScope(this) ?? new NameScope();
			NameScope.SetNameScope(this, nameScope);
			nameScope.RegisterName("ldc", liveDataChartUC);
			if (liveDataChartUC.StyleId == null)
			{
				liveDataChartUC.StyleId = "ldc";
			}
			this.ldc = liveDataChartUC;
			this.Resources = resourceDictionary;
			resourceDictionary.Add("ExcludeInfinityFromDoubleConverter", excludeInfinityFromDoubleConverter);
			resourceDictionary.Add("BoolToNegativeConverter", boolToNegativeConverter);
			translate.Text = "ios_MainPage_Connection";
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
			xmlNamespaceResolver.Add("gauge", "clr-namespace:Syncfusion.SfGauge.XForms;assembly=Syncfusion.SfGauge.XForms");
			xmlNamespaceResolver.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver.Add("mr", "clr-namespace:MR.Gestures;assembly=MR.Gestures");
			xamlServiceProvider.Add(typeFromHandle2, new XamlTypeResolver(xmlNamespaceResolver, typeof(TestPage).GetTypeInfo().Assembly));
			xamlServiceProvider.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(9, 5)));
			object obj2 = markupExtension.ProvideValue(xamlServiceProvider);
			this.Title = obj2;
			this.Resources = resourceDictionary;
			onPlatform.Android = new Thickness(20.0, 20.0, 20.0, 20.0);
			onPlatform.WinPhone = new Thickness(20.0, 20.0, 20.0, 20.0);
			onPlatform.iOS = new Thickness(5.0, 20.0, 5.0, 5.0);
			this.SetValue(Page.PaddingProperty, onPlatform);
			this.SetValue(ContentPage.ContentProperty, liveDataChartUC);
		}

		// Token: 0x06000617 RID: 1559 RVA: 0x00066310 File Offset: 0x00064510
		private void __InitComponentRuntime()
		{
			Extensions.LoadFromXaml<TestPage>(this, typeof(TestPage));
			this.ldc = NameScopeExtensions.FindByName<LiveDataChartUC>(this, "ldc");
		}

		// Token: 0x040004F8 RID: 1272
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private LiveDataChartUC ldc;

		// Token: 0x02000146 RID: 326
		[CompilerGenerated]
		[Serializable]
		private sealed class <>c
		{
			// Token: 0x06000618 RID: 1560 RVA: 0x00066334 File Offset: 0x00064534
			// Note: this type is marked as 'beforefieldinit'.
			static <>c()
			{
			}

			// Token: 0x06000619 RID: 1561 RVA: 0x00002050 File Offset: 0x00000250
			public <>c()
			{
			}

			// Token: 0x0600061A RID: 1562 RVA: 0x00066340 File Offset: 0x00064540
			internal bool <.ctor>b__0_0(PID x)
			{
				return x is IPIDFloatValue;
			}

			// Token: 0x040004F9 RID: 1273
			public static readonly TestPage.<>c <>9 = new TestPage.<>c();

			// Token: 0x040004FA RID: 1274
			public static Func<PID, bool> <>9__0_0;
		}
	}
}
