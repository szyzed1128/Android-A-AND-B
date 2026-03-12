using System;
using System.CodeDom.Compiler;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using CarScannerXamarinForms.UserControls;
using Xamarin.Forms;
using Xamarin.Forms.Internals;
using Xamarin.Forms.PlatformConfiguration.iOSSpecific;
using Xamarin.Forms.Xaml;
using Xamarin.Forms.Xaml.Diagnostics;
using Xamarin.Forms.Xaml.Internals;

namespace CarScannerXamarinForms.Pages
{
	// Token: 0x0200061B RID: 1563
	[XamlCompilation(2)]
	[XamlFilePath("Pages\\DroidPermissionRequestPage.xaml")]
	public class DroidPermissionRequestPage : ContentPage
	{
		// Token: 0x060036D2 RID: 14034 RVA: 0x0028A4DE File Offset: 0x002886DE
		public DroidPermissionRequestPage(bool displayBluetooth = true, bool displayGPS = true)
		{
			this.InitializeComponent();
			this.DisplayBluetooth = displayBluetooth;
			this.DisplayGPS = displayGPS;
		}

		// Token: 0x1700137E RID: 4990
		// (get) Token: 0x060036D3 RID: 14035 RVA: 0x0028A4FA File Offset: 0x002886FA
		// (set) Token: 0x060036D4 RID: 14036 RVA: 0x0028A507 File Offset: 0x00288707
		public bool DisplayBluetooth
		{
			get
			{
				return this.permissionsUC.DisplayBluetooth;
			}
			set
			{
				this.permissionsUC.DisplayBluetooth = value;
			}
		}

		// Token: 0x1700137F RID: 4991
		// (get) Token: 0x060036D5 RID: 14037 RVA: 0x0028A515 File Offset: 0x00288715
		// (set) Token: 0x060036D6 RID: 14038 RVA: 0x0028A522 File Offset: 0x00288722
		public bool DisplayGPS
		{
			get
			{
				return this.permissionsUC.DisplayGPS;
			}
			set
			{
				this.permissionsUC.DisplayGPS = value;
			}
		}

		// Token: 0x060036D7 RID: 14039 RVA: 0x0028A530 File Offset: 0x00288730
		private void DroidPermissionRequestPage_Appearing(object sender, EventArgs e)
		{
			this.permissionsUC.UpdateCurrentStatus();
		}

		// Token: 0x060036D8 RID: 14040 RVA: 0x0028A540 File Offset: 0x00288740
		private async void PermissionsRequestControl_NextClicked(object sender, EventArgs e)
		{
			try
			{
				if (base.Navigation.ModalStack.Contains(this))
				{
					await base.Navigation.PopModalAsync();
				}
				else
				{
					await base.Navigation.PopAsync();
				}
			}
			catch (Exception)
			{
			}
		}

		// Token: 0x060036D9 RID: 14041 RVA: 0x0028A578 File Offset: 0x00288778
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private void InitializeComponent()
		{
			if (ResourceLoader.IsEnabled && ResourceLoader.CanProvideContentFor(new ResourceLoader.ResourceLoadingQuery
			{
				AssemblyName = typeof(DroidPermissionRequestPage).GetTypeInfo().Assembly.GetName(),
				ResourcePath = "Pages/DroidPermissionRequestPage.xaml",
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
			VisualDiagnostics.RegisterSourceInfo(translate = new Translate(), new Uri("Pages\\DroidPermissionRequestPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 12, 5);
			DynamicResourceExtension dynamicResourceExtension;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension = new DynamicResourceExtension(), new Uri("Pages\\DroidPermissionRequestPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 15, 5);
			PermissionsRequestControl permissionsRequestControl;
			VisualDiagnostics.RegisterSourceInfo(permissionsRequestControl = new PermissionsRequestControl(), new Uri("Pages\\DroidPermissionRequestPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 17, 10);
			VisualDiagnostics.RegisterSourceInfo(this, new Uri("Pages\\DroidPermissionRequestPage.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 2, 2);
			NameScope nameScope = NameScope.GetNameScope(this) ?? new NameScope();
			NameScope.SetNameScope(this, nameScope);
			nameScope.RegisterName("permissionsUC", permissionsRequestControl);
			if (permissionsRequestControl.StyleId == null)
			{
				permissionsRequestControl.StyleId = "permissionsUC";
			}
			this.permissionsUC = permissionsRequestControl;
			translate.Text = "ios_Permissions";
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
			xmlNamespaceResolver.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xmlNamespaceResolver.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider.Add(typeFromHandle2, new XamlTypeResolver(xmlNamespaceResolver, typeof(DroidPermissionRequestPage).GetTypeInfo().Assembly));
			xamlServiceProvider.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(12, 5)));
			object obj2 = markupExtension.ProvideValue(xamlServiceProvider);
			this.Title = obj2;
			this.SetValue(Page.UseSafeAreaProperty, true);
			this.Appearing += this.DroidPermissionRequestPage_Appearing;
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
			xmlNamespaceResolver2.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver2.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver2.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver2.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver2.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xmlNamespaceResolver2.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider2.Add(typeFromHandle4, new XamlTypeResolver(xmlNamespaceResolver2, typeof(DroidPermissionRequestPage).GetTypeInfo().Assembly));
			xamlServiceProvider2.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(15, 5)));
			DynamicResource dynamicResource = markupExtension2.ProvideValue(xamlServiceProvider2);
			this.SetDynamicResource(VisualElement.BackgroundColorProperty, dynamicResource.Key);
			permissionsRequestControl.NextClicked += this.PermissionsRequestControl_NextClicked;
			this.SetValue(ContentPage.ContentProperty, permissionsRequestControl);
		}

		// Token: 0x060036DA RID: 14042 RVA: 0x0028A96F File Offset: 0x00288B6F
		private void __InitComponentRuntime()
		{
			Extensions.LoadFromXaml<DroidPermissionRequestPage>(this, typeof(DroidPermissionRequestPage));
			this.permissionsUC = NameScopeExtensions.FindByName<PermissionsRequestControl>(this, "permissionsUC");
		}

		// Token: 0x04002119 RID: 8473
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private PermissionsRequestControl permissionsUC;

		// Token: 0x0200061C RID: 1564
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <PermissionsRequestControl_NextClicked>d__8 : IAsyncStateMachine
		{
			// Token: 0x060036DB RID: 14043 RVA: 0x0028A994 File Offset: 0x00288B94
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				DroidPermissionRequestPage droidPermissionRequestPage = this;
				try
				{
					try
					{
						TaskAwaiter<Page> taskAwaiter;
						TaskAwaiter<Page> taskAwaiter2;
						if (num != 0)
						{
							if (num != 1)
							{
								if (droidPermissionRequestPage.Navigation.ModalStack.Contains(droidPermissionRequestPage))
								{
									taskAwaiter = droidPermissionRequestPage.Navigation.PopModalAsync().GetAwaiter();
									if (!taskAwaiter.IsCompleted)
									{
										num2 = 0;
										taskAwaiter2 = taskAwaiter;
										this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<Page>, DroidPermissionRequestPage.<PermissionsRequestControl_NextClicked>d__8>(ref taskAwaiter, ref this);
										return;
									}
									goto IL_0089;
								}
								else
								{
									taskAwaiter = droidPermissionRequestPage.Navigation.PopAsync().GetAwaiter();
									if (!taskAwaiter.IsCompleted)
									{
										num2 = 1;
										taskAwaiter2 = taskAwaiter;
										this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<Page>, DroidPermissionRequestPage.<PermissionsRequestControl_NextClicked>d__8>(ref taskAwaiter, ref this);
										return;
									}
								}
							}
							else
							{
								taskAwaiter = taskAwaiter2;
								taskAwaiter2 = default(TaskAwaiter<Page>);
								num2 = -1;
							}
							taskAwaiter.GetResult();
							goto IL_00F1;
						}
						taskAwaiter = taskAwaiter2;
						taskAwaiter2 = default(TaskAwaiter<Page>);
						num2 = -1;
						IL_0089:
						taskAwaiter.GetResult();
						IL_00F1:;
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

			// Token: 0x060036DC RID: 14044 RVA: 0x0028AAE0 File Offset: 0x00288CE0
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x0400211A RID: 8474
			public int <>1__state;

			// Token: 0x0400211B RID: 8475
			public AsyncVoidMethodBuilder <>t__builder;

			// Token: 0x0400211C RID: 8476
			public DroidPermissionRequestPage <>4__this;

			// Token: 0x0400211D RID: 8477
			private TaskAwaiter<Page> <>u__1;
		}
	}
}
