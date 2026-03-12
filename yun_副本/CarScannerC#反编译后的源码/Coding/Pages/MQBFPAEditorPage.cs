using System;
using System.CodeDom.Compiler;
using System.Reflection;
using Xamarin.Forms;
using Xamarin.Forms.Internals;
using Xamarin.Forms.Xaml;
using Xamarin.Forms.Xaml.Diagnostics;
using Xamarin.Forms.Xaml.Internals;

namespace CarScannerXamarinForms.Coding.Pages
{
	// Token: 0x02000960 RID: 2400
	[XamlCompilation(2)]
	[XamlFilePath("Coding\\Pages\\MQBFPAEditorPage.xaml")]
	public class MQBFPAEditorPage : ContentPage
	{
		// Token: 0x06004ED1 RID: 20177 RVA: 0x003BEF80 File Offset: 0x003BD180
		public MQBFPAEditorPage()
		{
			this.InitializeComponent();
		}

		// Token: 0x06004ED2 RID: 20178 RVA: 0x003BEF90 File Offset: 0x003BD190
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private void InitializeComponent()
		{
			if (ResourceLoader.IsEnabled && ResourceLoader.CanProvideContentFor(new ResourceLoader.ResourceLoadingQuery
			{
				AssemblyName = typeof(MQBFPAEditorPage).GetTypeInfo().Assembly.GetName(),
				ResourcePath = "Coding/Pages/MQBFPAEditorPage.xaml",
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
			VisualDiagnostics.RegisterSourceInfo(label = new Label(), new Uri("Coding\\Pages\\MQBFPAEditorPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 8, 14);
			StackLayout stackLayout;
			VisualDiagnostics.RegisterSourceInfo(stackLayout = new StackLayout(), new Uri("Coding\\Pages\\MQBFPAEditorPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 7, 10);
			VisualDiagnostics.RegisterSourceInfo(this, new Uri("Coding\\Pages\\MQBFPAEditorPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 2, 2);
			NameScope nameScope = NameScope.GetNameScope(this) ?? new NameScope();
			NameScope.SetNameScope(this, nameScope);
			label.SetValue(View.HorizontalOptionsProperty, LayoutOptions.CenterAndExpand);
			label.SetValue(Label.TextProperty, "Welcome to Xamarin.Forms!");
			label.SetValue(View.VerticalOptionsProperty, LayoutOptions.CenterAndExpand);
			stackLayout.Children.Add(label);
			this.SetValue(ContentPage.ContentProperty, stackLayout);
		}

		// Token: 0x06004ED3 RID: 20179 RVA: 0x003BF0EA File Offset: 0x003BD2EA
		private void __InitComponentRuntime()
		{
			Extensions.LoadFromXaml<MQBFPAEditorPage>(this, typeof(MQBFPAEditorPage));
		}
	}
}
