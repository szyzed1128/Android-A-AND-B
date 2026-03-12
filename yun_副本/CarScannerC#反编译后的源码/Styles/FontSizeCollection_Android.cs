using System;
using System.CodeDom.Compiler;
using System.Reflection;
using Xamarin.Forms;
using Xamarin.Forms.Internals;
using Xamarin.Forms.Xaml;
using Xamarin.Forms.Xaml.Diagnostics;
using Xamarin.Forms.Xaml.Internals;

namespace CarScannerXamarinForms.Styles
{
	// Token: 0x020001EC RID: 492
	[XamlCompilation(2)]
	[XamlFilePath("Styles\\FontSizeCollection_Android.xaml")]
	public class FontSizeCollection_Android : ContentView
	{
		// Token: 0x060019ED RID: 6637 RVA: 0x0011214C File Offset: 0x0011034C
		public FontSizeCollection_Android()
		{
			this.InitializeComponent();
		}

		// Token: 0x060019EE RID: 6638 RVA: 0x0011215C File Offset: 0x0011035C
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private void InitializeComponent()
		{
			if (ResourceLoader.IsEnabled && ResourceLoader.CanProvideContentFor(new ResourceLoader.ResourceLoadingQuery
			{
				AssemblyName = typeof(FontSizeCollection_Android).GetTypeInfo().Assembly.GetName(),
				ResourcePath = "Styles/FontSizeCollection_Android.xaml",
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
			Label label;
			VisualDiagnostics.RegisterSourceInfo(label = new Label(), new Uri("Styles\\FontSizeCollection_Android.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 8, 14);
			StackLayout stackLayout;
			VisualDiagnostics.RegisterSourceInfo(stackLayout = new StackLayout(), new Uri("Styles\\FontSizeCollection_Android.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 7, 10);
			VisualDiagnostics.RegisterSourceInfo(this, new Uri("Styles\\FontSizeCollection_Android.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 2, 2);
			NameScope nameScope = NameScope.GetNameScope(this) ?? new NameScope();
			NameScope.SetNameScope(this, nameScope);
			label.SetValue(Label.TextProperty, "Hello Xamarin.Forms!");
			stackLayout.Children.Add(label);
			this.SetValue(ContentView.ContentProperty, stackLayout);
		}

		// Token: 0x060019EF RID: 6639 RVA: 0x0011228C File Offset: 0x0011048C
		private void __InitComponentRuntime()
		{
			Extensions.LoadFromXaml<FontSizeCollection_Android>(this, typeof(FontSizeCollection_Android));
		}
	}
}
