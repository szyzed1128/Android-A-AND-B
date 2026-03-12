using System;
using System.CodeDom.Compiler;
using System.Reflection;
using Xamarin.Forms;
using Xamarin.Forms.Internals;
using Xamarin.Forms.Maps;
using Xamarin.Forms.Xaml;
using Xamarin.Forms.Xaml.Diagnostics;
using Xamarin.Forms.Xaml.Internals;

namespace CarScannerXamarinForms.DataRecorder
{
	// Token: 0x02000708 RID: 1800
	[XamlCompilation(2)]
	[XamlFilePath("DataRecorder\\TestPage.xaml")]
	public class TestPage : ContentPage
	{
		// Token: 0x06003D35 RID: 15669 RVA: 0x00326DAE File Offset: 0x00324FAE
		public TestPage()
		{
			this.InitializeComponent();
		}

		// Token: 0x06003D36 RID: 15670 RVA: 0x00326DBC File Offset: 0x00324FBC
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private void InitializeComponent()
		{
			if (ResourceLoader.IsEnabled && ResourceLoader.CanProvideContentFor(new ResourceLoader.ResourceLoadingQuery
			{
				AssemblyName = typeof(TestPage).GetTypeInfo().Assembly.GetName(),
				ResourcePath = "DataRecorder/TestPage.xaml",
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
			Map map;
			VisualDiagnostics.RegisterSourceInfo(map = new Map(), new Uri("DataRecorder\\TestPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 10, 14);
			Grid grid;
			VisualDiagnostics.RegisterSourceInfo(grid = new Grid(), new Uri("DataRecorder\\TestPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 9, 10);
			VisualDiagnostics.RegisterSourceInfo(this, new Uri("DataRecorder\\TestPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 2, 2);
			NameScope nameScope = NameScope.GetNameScope(this) ?? new NameScope();
			NameScope.SetNameScope(this, nameScope);
			nameScope.RegisterName("map", map);
			if (map.StyleId == null)
			{
				map.StyleId = "map";
			}
			this.map = map;
			grid.Children.Add(map);
			this.SetValue(ContentPage.ContentProperty, grid);
		}

		// Token: 0x06003D37 RID: 15671 RVA: 0x00326F05 File Offset: 0x00325105
		private void __InitComponentRuntime()
		{
			Extensions.LoadFromXaml<TestPage>(this, typeof(TestPage));
			this.map = NameScopeExtensions.FindByName<Map>(this, "map");
		}

		// Token: 0x04002587 RID: 9607
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private Map map;
	}
}
