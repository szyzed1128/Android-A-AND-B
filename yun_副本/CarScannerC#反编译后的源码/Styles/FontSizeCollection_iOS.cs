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
	// Token: 0x020001ED RID: 493
	[XamlCompilation(2)]
	[XamlFilePath("Styles\\FontSizeCollection_iOS.xaml")]
	public class FontSizeCollection_iOS : ResourceDictionary
	{
		// Token: 0x060019F0 RID: 6640 RVA: 0x0011229F File Offset: 0x0011049F
		public FontSizeCollection_iOS()
		{
			this.InitializeComponent();
		}

		// Token: 0x060019F1 RID: 6641 RVA: 0x001122B0 File Offset: 0x001104B0
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private void InitializeComponent()
		{
			if (ResourceLoader.IsEnabled && ResourceLoader.CanProvideContentFor(new ResourceLoader.ResourceLoadingQuery
			{
				AssemblyName = typeof(FontSizeCollection_iOS).GetTypeInfo().Assembly.GetName(),
				ResourcePath = "Styles/FontSizeCollection_iOS.xaml",
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
			double num = 17.0;
			double num2 = 12.0;
			double num3 = 14.0;
			double num4 = 17.0;
			double num5 = 22.0;
			double num6 = 17.0;
			double num7 = 17.0;
			double num8 = 28.0;
			double num9 = 22.0;
			double num10 = 12.0;
			VisualDiagnostics.RegisterSourceInfo(this, new Uri("Styles\\FontSizeCollection_iOS.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 2, 2);
			NameScope nameScope = new NameScope();
			this.Add("Default", num);
			this.Add("Micro", num2);
			this.Add("Small", num3);
			this.Add("Medium", num4);
			this.Add("Large", num5);
			this.Add("Body", num6);
			this.Add("Header", num7);
			this.Add("Title", num8);
			this.Add("Subtitle", num9);
			this.Add("Caption", num10);
		}

		// Token: 0x060019F2 RID: 6642 RVA: 0x0011247B File Offset: 0x0011067B
		private void __InitComponentRuntime()
		{
			Extensions.LoadFromXaml<FontSizeCollection_iOS>(this, typeof(FontSizeCollection_iOS));
		}
	}
}
