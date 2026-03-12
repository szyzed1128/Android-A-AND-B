using System;
using System.CodeDom.Compiler;
using System.Reflection;
using Xamarin.Forms;
using Xamarin.Forms.Internals;
using Xamarin.Forms.Xaml;
using Xamarin.Forms.Xaml.Diagnostics;
using Xamarin.Forms.Xaml.Internals;

namespace CarScannerXamarinForms.Settings
{
	// Token: 0x02000243 RID: 579
	[XamlFilePath("Settings\\SettingsOptimizations.xaml")]
	public class SettingsOptimizations : ContentPage
	{
		// Token: 0x06001B53 RID: 6995 RVA: 0x00132732 File Offset: 0x00130932
		public SettingsOptimizations()
		{
			this.InitializeComponent();
		}

		// Token: 0x06001B54 RID: 6996 RVA: 0x00132740 File Offset: 0x00130940
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private void InitializeComponent()
		{
			if (ResourceLoader.IsEnabled && ResourceLoader.CanProvideContentFor(new ResourceLoader.ResourceLoadingQuery
			{
				AssemblyName = typeof(SettingsOptimizations).GetTypeInfo().Assembly.GetName(),
				ResourcePath = "Settings/SettingsOptimizations.xaml",
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
			VisualDiagnostics.RegisterSourceInfo(this, new Uri("Settings\\SettingsOptimizations.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 2, 2);
			NameScope nameScope = NameScope.GetNameScope(this) ?? new NameScope();
			NameScope.SetNameScope(this, nameScope);
		}

		// Token: 0x06001B55 RID: 6997 RVA: 0x001327F6 File Offset: 0x001309F6
		private void __InitComponentRuntime()
		{
			Extensions.LoadFromXaml<SettingsOptimizations>(this, typeof(SettingsOptimizations));
		}
	}
}
