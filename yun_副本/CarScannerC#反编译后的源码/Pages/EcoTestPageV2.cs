using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using AiForms.Renderers;
using CarScannerXamarinForms.Common;
using CarScannerXamarinForms.Common.XAMLConverters;
using CarScannerXamarinForms.OBD2;
using CarScannerXamarinForms.OBD2.PIDS;
using CarScannerXamarinForms.UserControls;
using CarScannerXamarinForms.ViewModels;
using Syncfusion.DataSource;
using Xamarin.Forms;
using Xamarin.Forms.Internals;
using Xamarin.Forms.PlatformConfiguration.iOSSpecific;
using Xamarin.Forms.Xaml;
using Xamarin.Forms.Xaml.Diagnostics;
using Xamarin.Forms.Xaml.Internals;

namespace CarScannerXamarinForms.Pages
{
	// Token: 0x0200061D RID: 1565
	[XamlCompilation(2)]
	[XamlFilePath("Pages\\EcoTestPageV2.xaml")]
	public class EcoTestPageV2 : ContentPage
	{
		// Token: 0x060036DD RID: 14045 RVA: 0x0028AAEE File Offset: 0x00288CEE
		public EcoTestPageV2()
		{
			this.InitializeComponent();
			this.Init();
		}

		// Token: 0x060036DE RID: 14046 RVA: 0x0028AB18 File Offset: 0x00288D18
		protected override bool OnBackButtonPressed()
		{
			if (this.pidSinceDTC != null)
			{
				this.pidSinceDTC.ValueChanged -= this.PidSinceDTC_ValueChanged;
			}
			if (this.pidCurrentCycle != null)
			{
				this.pidCurrentCycle.ValueChanged -= this.PidCurrentCycle_ValueChanged;
			}
			if (Device.RuntimePlatform == "UWP" || Device.RuntimePlatform == "Android")
			{
				this.btnBack_Clicked(this, null);
			}
			return true;
		}

		// Token: 0x060036DF RID: 14047 RVA: 0x0028AB90 File Offset: 0x00288D90
		private async void btnBack_Clicked(object sender, EventArgs e)
		{
			if (this.pidSinceDTC != null)
			{
				this.pidSinceDTC.ValueChanged -= this.PidSinceDTC_ValueChanged;
			}
			if (this.pidCurrentCycle != null)
			{
				this.pidCurrentCycle.ValueChanged -= this.PidCurrentCycle_ValueChanged;
			}
			try
			{
				await base.Navigation.PopAsync();
			}
			catch (Exception)
			{
			}
		}

		// Token: 0x060036E0 RID: 14048 RVA: 0x0028ABC8 File Offset: 0x00288DC8
		private void Init()
		{
			this.pidSinceDTC = App.OBDReader.CurrentCarData.LiveDataPIDs.FirstOrDefault((PID x) => x.Id == 2) as PID_Status;
			this.pidCurrentCycle = App.OBDReader.CurrentCarData.LiveDataPIDs.FirstOrDefault((PID x) => x.Id == 89) as PID_Status;
			this.sectSinceDTC.IsVisible = this.pidSinceDTC.IsAvailable;
			this.sectCurrentCycle.IsVisible = this.pidCurrentCycle.IsAvailable;
			List<OBDRequest> list = new List<OBDRequest>();
			if (this.pidSinceDTC.IsAvailable)
			{
				LiveDataPIDModel.GetRequests(this.pidSinceDTC, list, null, "");
				this.pidSinceDTC.ValueChanged -= this.PidSinceDTC_ValueChanged;
				this.pidSinceDTC.ValueChanged += this.PidSinceDTC_ValueChanged;
			}
			if (this.pidCurrentCycle.IsAvailable)
			{
				LiveDataPIDModel.GetRequests(this.pidCurrentCycle, list, null, "");
				this.pidCurrentCycle.ValueChanged -= this.PidCurrentCycle_ValueChanged;
				this.pidCurrentCycle.ValueChanged += this.PidCurrentCycle_ValueChanged;
			}
			if (list.Count > 0)
			{
				foreach (OBDRequest obdrequest in list)
				{
					App.OBDReader.AddRequestToQueue(obdrequest);
				}
			}
		}

		// Token: 0x060036E1 RID: 14049 RVA: 0x000345C3 File Offset: 0x000327C3
		private void btnInfo_Clicked(object sender, EventArgs e)
		{
			base.DisplayAlert(Translate.GetString("ios_MainPage_TileEmissionTests"), Translate.GetString("ios_MainPage_EmissionTestsInfoText"), "OK");
		}

		// Token: 0x17001380 RID: 4992
		// (get) Token: 0x060036E2 RID: 14050 RVA: 0x0028AD6C File Offset: 0x00288F6C
		// (set) Token: 0x060036E3 RID: 14051 RVA: 0x0028AD74 File Offset: 0x00288F74
		public ObservableCollection<ECUTest> TestsSinceDTC
		{
			[CompilerGenerated]
			get
			{
				return this.<TestsSinceDTC>k__BackingField;
			}
			[CompilerGenerated]
			private set
			{
				this.<TestsSinceDTC>k__BackingField = value;
			}
		} = new ObservableCollection<ECUTest>();

		// Token: 0x17001381 RID: 4993
		// (get) Token: 0x060036E4 RID: 14052 RVA: 0x0028AD7D File Offset: 0x00288F7D
		// (set) Token: 0x060036E5 RID: 14053 RVA: 0x0028AD85 File Offset: 0x00288F85
		public ObservableCollection<ECUTest> TestsCurrentCycle
		{
			[CompilerGenerated]
			get
			{
				return this.<TestsCurrentCycle>k__BackingField;
			}
			[CompilerGenerated]
			private set
			{
				this.<TestsCurrentCycle>k__BackingField = value;
			}
		} = new ObservableCollection<ECUTest>();

		// Token: 0x060036E6 RID: 14054 RVA: 0x0028AD8E File Offset: 0x00288F8E
		private void PidCurrentCycle_ValueChanged(object sender, PID e)
		{
			MainThreadHelper.InvokeOnMainThread(delegate
			{
				PID_Status pid_Status = e as PID_Status;
				if (pid_Status != null)
				{
					ECUTest[] ecutests = pid_Status.Value.ECUTests;
					for (int i = 0; i < ecutests.Length; i++)
					{
						ECUTest t = ecutests[i];
						ECUTest ecutest = this.TestsCurrentCycle.FirstOrDefault((ECUTest x) => x.Name == t.Name);
						if (ecutest == null)
						{
							this.TestsCurrentCycle.Add(t);
						}
						else if (ecutest.Complete != t.Complete || ecutest.Available != t.Available)
						{
							int num = this.TestsCurrentCycle.IndexOf(ecutest);
							this.TestsCurrentCycle[num] = t;
						}
					}
				}
			});
		}

		// Token: 0x060036E7 RID: 14055 RVA: 0x0028ADB3 File Offset: 0x00288FB3
		private void PidSinceDTC_ValueChanged(object sender, PID e)
		{
			MainThreadHelper.InvokeOnMainThread(delegate
			{
				PID_Status pid_Status = e as PID_Status;
				if (pid_Status != null)
				{
					ECUTest[] ecutests = pid_Status.Value.ECUTests;
					for (int i = 0; i < ecutests.Length; i++)
					{
						ECUTest t = ecutests[i];
						ECUTest ecutest = this.TestsSinceDTC.FirstOrDefault((ECUTest x) => x.Name == t.Name);
						if (ecutest == null)
						{
							this.TestsSinceDTC.Add(t);
						}
						else if (ecutest.Complete != t.Complete || ecutest.Available != t.Available)
						{
							int num = this.TestsSinceDTC.IndexOf(ecutest);
							this.TestsSinceDTC[num] = t;
						}
					}
				}
			});
		}

		// Token: 0x060036E8 RID: 14056 RVA: 0x0028ADD8 File Offset: 0x00288FD8
		private void setDataSource()
		{
			PID_Status pid_Status = App.OBDReader.CurrentCarData.LiveDataPIDs.FirstOrDefault((PID x) => x.Id == 2) as PID_Status;
			PID_Status pid_Status2 = App.OBDReader.CurrentCarData.LiveDataPIDs.FirstOrDefault((PID x) => x.Id == 89) as PID_Status;
			List<ECUTest> list = new List<ECUTest>();
			if (pid_Status.IsAvailable)
			{
				list.AddRange(pid_Status.Value.ECUTests);
			}
			if (pid_Status2.IsAvailable)
			{
				list.AddRange(pid_Status2.Value.ECUTests);
			}
			Collection<GroupDescriptor> groupDescriptors = new DataSource
			{
				Source = list
			}.GroupDescriptors;
			GroupDescriptor groupDescriptor = new GroupDescriptor();
			groupDescriptor.PropertyName = "Cycle";
			groupDescriptor.KeySelector = delegate(object obj)
			{
				if (((ECUTest)obj).Cycle == TestPidCycle.SinceDTCCleared)
				{
					return "Since DTC Cleared";
				}
				return "Current drive cycle";
			};
			groupDescriptors.Add(groupDescriptor);
		}

		// Token: 0x060036E9 RID: 14057 RVA: 0x0028AEDC File Offset: 0x002890DC
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private void InitializeComponent()
		{
			if (ResourceLoader.IsEnabled && ResourceLoader.CanProvideContentFor(new ResourceLoader.ResourceLoadingQuery
			{
				AssemblyName = typeof(EcoTestPageV2).GetTypeInfo().Assembly.GetName(),
				ResourcePath = "Pages/EcoTestPageV2.xaml",
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
			VisualDiagnostics.RegisterSourceInfo(translate = new Translate(), new Uri("Pages\\EcoTestPageV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 13, 5);
			DynamicResourceExtension dynamicResourceExtension;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension = new DynamicResourceExtension(), new Uri("Pages\\EcoTestPageV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 15, 5);
			BoolToColorVariantsConverter boolToColorVariantsConverter;
			VisualDiagnostics.RegisterSourceInfo(boolToColorVariantsConverter = new BoolToColorVariantsConverter(), new Uri("Pages\\EcoTestPageV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 21, 14);
			Translate translate2;
			VisualDiagnostics.RegisterSourceInfo(translate2 = new Translate(), new Uri("Pages\\EcoTestPageV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 30, 17);
			Translate translate3;
			VisualDiagnostics.RegisterSourceInfo(translate3 = new Translate(), new Uri("Pages\\EcoTestPageV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 31, 17);
			BoolToStringsVariantsConverter boolToStringsVariantsConverter;
			VisualDiagnostics.RegisterSourceInfo(boolToStringsVariantsConverter = new BoolToStringsVariantsConverter(), new Uri("Pages\\EcoTestPageV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 28, 14);
			Translate translate4;
			VisualDiagnostics.RegisterSourceInfo(translate4 = new Translate(), new Uri("Pages\\EcoTestPageV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 35, 17);
			Translate translate5;
			VisualDiagnostics.RegisterSourceInfo(translate5 = new Translate(), new Uri("Pages\\EcoTestPageV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 36, 17);
			BoolToStringsVariantsConverter boolToStringsVariantsConverter2;
			VisualDiagnostics.RegisterSourceInfo(boolToStringsVariantsConverter2 = new BoolToStringsVariantsConverter(), new Uri("Pages\\EcoTestPageV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 33, 14);
			ResourceDictionary resourceDictionary;
			VisualDiagnostics.RegisterSourceInfo(resourceDictionary = new ResourceDictionary(), new Uri("Pages\\EcoTestPageV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 19, 10);
			On on;
			VisualDiagnostics.RegisterSourceInfo(on = new On(), new Uri("Pages\\EcoTestPageV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 41, 14);
			On on2;
			VisualDiagnostics.RegisterSourceInfo(on2 = new On(), new Uri("Pages\\EcoTestPageV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 42, 14);
			OnPlatform<Thickness> onPlatform;
			VisualDiagnostics.RegisterSourceInfo(onPlatform = new OnPlatform<Thickness>(), new Uri("Pages\\EcoTestPageV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 40, 10);
			ColumnDefinition columnDefinition;
			VisualDiagnostics.RegisterSourceInfo(columnDefinition = new ColumnDefinition(), new Uri("Pages\\EcoTestPageV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 50, 18);
			ColumnDefinition columnDefinition2;
			VisualDiagnostics.RegisterSourceInfo(columnDefinition2 = new ColumnDefinition(), new Uri("Pages\\EcoTestPageV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 51, 18);
			ColumnDefinition columnDefinition3;
			VisualDiagnostics.RegisterSourceInfo(columnDefinition3 = new ColumnDefinition(), new Uri("Pages\\EcoTestPageV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 52, 18);
			DynamicResourceExtension dynamicResourceExtension2;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension2 = new DynamicResourceExtension(), new Uri("Pages\\EcoTestPageV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 58, 17);
			Translate translate6;
			VisualDiagnostics.RegisterSourceInfo(translate6 = new Translate(), new Uri("Pages\\EcoTestPageV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 59, 17);
			LinkButton linkButton;
			VisualDiagnostics.RegisterSourceInfo(linkButton = new LinkButton(), new Uri("Pages\\EcoTestPageV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 54, 14);
			DynamicResourceExtension dynamicResourceExtension3;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension3 = new DynamicResourceExtension(), new Uri("Pages\\EcoTestPageV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 70, 17);
			Translate translate7;
			VisualDiagnostics.RegisterSourceInfo(translate7 = new Translate(), new Uri("Pages\\EcoTestPageV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 71, 17);
			DynamicResourceExtension dynamicResourceExtension4;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension4 = new DynamicResourceExtension(), new Uri("Pages\\EcoTestPageV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 72, 17);
			NonScalableLabel nonScalableLabel;
			VisualDiagnostics.RegisterSourceInfo(nonScalableLabel = new NonScalableLabel(), new Uri("Pages\\EcoTestPageV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 68, 14);
			DynamicResourceExtension dynamicResourceExtension5;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension5 = new DynamicResourceExtension(), new Uri("Pages\\EcoTestPageV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 79, 17);
			DynamicResourceExtension dynamicResourceExtension6;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension6 = new DynamicResourceExtension(), new Uri("Pages\\EcoTestPageV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 80, 17);
			On on3;
			VisualDiagnostics.RegisterSourceInfo(on3 = new On(), new Uri("Pages\\EcoTestPageV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 84, 26);
			On on4;
			VisualDiagnostics.RegisterSourceInfo(on4 = new On(), new Uri("Pages\\EcoTestPageV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 85, 26);
			OnPlatform<Thickness> onPlatform2;
			VisualDiagnostics.RegisterSourceInfo(onPlatform2 = new OnPlatform<Thickness>(), new Uri("Pages\\EcoTestPageV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 83, 22);
			LinkButton linkButton2;
			VisualDiagnostics.RegisterSourceInfo(linkButton2 = new LinkButton(), new Uri("Pages\\EcoTestPageV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 75, 14);
			Grid grid;
			VisualDiagnostics.RegisterSourceInfo(grid = new Grid(), new Uri("Pages\\EcoTestPageV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 47, 10);
			ReferenceExtension referenceExtension;
			VisualDiagnostics.RegisterSourceInfo(referenceExtension = new ReferenceExtension(), new Uri("Pages\\EcoTestPageV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 95, 13);
			Translate translate8;
			VisualDiagnostics.RegisterSourceInfo(translate8 = new Translate(), new Uri("Pages\\EcoTestPageV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 99, 17);
			BindingExtension bindingExtension;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension = new BindingExtension(), new Uri("Pages\\EcoTestPageV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 100, 17);
			DataTemplate dataTemplate;
			VisualDiagnostics.RegisterSourceInfo(dataTemplate = new DataTemplate(), new Uri("Pages\\EcoTestPageV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 102, 22);
			Section section;
			VisualDiagnostics.RegisterSourceInfo(section = new Section(), new Uri("Pages\\EcoTestPageV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 97, 14);
			Translate translate9;
			VisualDiagnostics.RegisterSourceInfo(translate9 = new Translate(), new Uri("Pages\\EcoTestPageV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 114, 17);
			BindingExtension bindingExtension2;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension2 = new BindingExtension(), new Uri("Pages\\EcoTestPageV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 115, 17);
			DataTemplate dataTemplate2;
			VisualDiagnostics.RegisterSourceInfo(dataTemplate2 = new DataTemplate(), new Uri("Pages\\EcoTestPageV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 117, 22);
			Section section2;
			VisualDiagnostics.RegisterSourceInfo(section2 = new Section(), new Uri("Pages\\EcoTestPageV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 112, 14);
			SettingsView settingsView;
			VisualDiagnostics.RegisterSourceInfo(settingsView = new SettingsView(), new Uri("Pages\\EcoTestPageV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 93, 10);
			VisualDiagnostics.RegisterSourceInfo(this, new Uri("Pages\\EcoTestPageV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 2, 2);
			NameScope nameScope = NameScope.GetNameScope(this) ?? new NameScope();
			NameScope.SetNameScope(this, nameScope);
			nameScope.RegisterName("page", this);
			if (this.StyleId == null)
			{
				this.StyleId = "page";
			}
			nameScope.RegisterName("settingsLayoutRoot", settingsView);
			if (settingsView.StyleId == null)
			{
				settingsView.StyleId = "settingsLayoutRoot";
			}
			nameScope.RegisterName("sectSinceDTC", section);
			if (section.StyleId == null)
			{
				section.StyleId = "sectSinceDTC";
			}
			nameScope.RegisterName("sectCurrentCycle", section2);
			if (section2.StyleId == null)
			{
				section2.StyleId = "sectCurrentCycle";
			}
			this.page = this;
			this.settingsLayoutRoot = settingsView;
			this.sectSinceDTC = section;
			this.sectCurrentCycle = section2;
			this.Resources = resourceDictionary;
			boolToColorVariantsConverter.FalseValue = Color.Red;
			boolToColorVariantsConverter.TrueValue = Color.Green;
			resourceDictionary.Add("ColorConverter", boolToColorVariantsConverter);
			translate2.Text = "test_NotCompleted";
			IMarkupExtension markupExtension = translate2;
			XamlServiceProvider xamlServiceProvider = new XamlServiceProvider();
			Type typeFromHandle = typeof(IProvideValueTarget);
			object[] array = new object[0 + 3];
			array[0] = boolToStringsVariantsConverter;
			array[1] = resourceDictionary;
			array[2] = this;
			object obj;
			xamlServiceProvider.Add(typeFromHandle, obj = new SimpleValueTargetProvider(array, typeof(BoolToStringsVariantsConverter).GetRuntimeProperty("FalseValue"), nameScope));
			xamlServiceProvider.Add(typeof(IReferenceProvider), obj);
			Type typeFromHandle2 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver = new XmlNamespaceResolver();
			xmlNamespaceResolver.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver.Add("pids", "clr-namespace:CarScannerXamarinForms.OBD2.PIDS");
			xmlNamespaceResolver.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xmlNamespaceResolver.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider.Add(typeFromHandle2, new XamlTypeResolver(xmlNamespaceResolver, typeof(EcoTestPageV2).GetTypeInfo().Assembly));
			xamlServiceProvider.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(30, 17)));
			object obj2 = markupExtension.ProvideValue(xamlServiceProvider);
			boolToStringsVariantsConverter.FalseValue = obj2;
			translate3.Text = "test_Completed";
			IMarkupExtension markupExtension2 = translate3;
			XamlServiceProvider xamlServiceProvider2 = new XamlServiceProvider();
			Type typeFromHandle3 = typeof(IProvideValueTarget);
			object[] array2 = new object[0 + 3];
			array2[0] = boolToStringsVariantsConverter;
			array2[1] = resourceDictionary;
			array2[2] = this;
			object obj3;
			xamlServiceProvider2.Add(typeFromHandle3, obj3 = new SimpleValueTargetProvider(array2, typeof(BoolToStringsVariantsConverter).GetRuntimeProperty("TrueValue"), nameScope));
			xamlServiceProvider2.Add(typeof(IReferenceProvider), obj3);
			Type typeFromHandle4 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver2 = new XmlNamespaceResolver();
			xmlNamespaceResolver2.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver2.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver2.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver2.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver2.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver2.Add("pids", "clr-namespace:CarScannerXamarinForms.OBD2.PIDS");
			xmlNamespaceResolver2.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xmlNamespaceResolver2.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider2.Add(typeFromHandle4, new XamlTypeResolver(xmlNamespaceResolver2, typeof(EcoTestPageV2).GetTypeInfo().Assembly));
			xamlServiceProvider2.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(31, 17)));
			object obj4 = markupExtension2.ProvideValue(xamlServiceProvider2);
			boolToStringsVariantsConverter.TrueValue = obj4;
			resourceDictionary.Add("CompleteConverter", boolToStringsVariantsConverter);
			translate4.Text = "test_NotAvailable";
			IMarkupExtension markupExtension3 = translate4;
			XamlServiceProvider xamlServiceProvider3 = new XamlServiceProvider();
			Type typeFromHandle5 = typeof(IProvideValueTarget);
			object[] array3 = new object[0 + 3];
			array3[0] = boolToStringsVariantsConverter2;
			array3[1] = resourceDictionary;
			array3[2] = this;
			object obj5;
			xamlServiceProvider3.Add(typeFromHandle5, obj5 = new SimpleValueTargetProvider(array3, typeof(BoolToStringsVariantsConverter).GetRuntimeProperty("FalseValue"), nameScope));
			xamlServiceProvider3.Add(typeof(IReferenceProvider), obj5);
			Type typeFromHandle6 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver3 = new XmlNamespaceResolver();
			xmlNamespaceResolver3.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver3.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver3.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver3.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver3.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver3.Add("pids", "clr-namespace:CarScannerXamarinForms.OBD2.PIDS");
			xmlNamespaceResolver3.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xmlNamespaceResolver3.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider3.Add(typeFromHandle6, new XamlTypeResolver(xmlNamespaceResolver3, typeof(EcoTestPageV2).GetTypeInfo().Assembly));
			xamlServiceProvider3.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(35, 17)));
			object obj6 = markupExtension3.ProvideValue(xamlServiceProvider3);
			boolToStringsVariantsConverter2.FalseValue = obj6;
			translate5.Text = "test_Available";
			IMarkupExtension markupExtension4 = translate5;
			XamlServiceProvider xamlServiceProvider4 = new XamlServiceProvider();
			Type typeFromHandle7 = typeof(IProvideValueTarget);
			object[] array4 = new object[0 + 3];
			array4[0] = boolToStringsVariantsConverter2;
			array4[1] = resourceDictionary;
			array4[2] = this;
			object obj7;
			xamlServiceProvider4.Add(typeFromHandle7, obj7 = new SimpleValueTargetProvider(array4, typeof(BoolToStringsVariantsConverter).GetRuntimeProperty("TrueValue"), nameScope));
			xamlServiceProvider4.Add(typeof(IReferenceProvider), obj7);
			Type typeFromHandle8 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver4 = new XmlNamespaceResolver();
			xmlNamespaceResolver4.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver4.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver4.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver4.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver4.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver4.Add("pids", "clr-namespace:CarScannerXamarinForms.OBD2.PIDS");
			xmlNamespaceResolver4.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xmlNamespaceResolver4.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider4.Add(typeFromHandle8, new XamlTypeResolver(xmlNamespaceResolver4, typeof(EcoTestPageV2).GetTypeInfo().Assembly));
			xamlServiceProvider4.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(36, 17)));
			object obj8 = markupExtension4.ProvideValue(xamlServiceProvider4);
			boolToStringsVariantsConverter2.TrueValue = obj8;
			resourceDictionary.Add("AvailableConverter", boolToStringsVariantsConverter2);
			translate.Text = "ios_MainPage_TileEmissionTests";
			IMarkupExtension markupExtension5 = translate;
			XamlServiceProvider xamlServiceProvider5 = new XamlServiceProvider();
			Type typeFromHandle9 = typeof(IProvideValueTarget);
			object[] array5 = new object[0 + 1];
			array5[0] = this;
			object obj9;
			xamlServiceProvider5.Add(typeFromHandle9, obj9 = new SimpleValueTargetProvider(array5, Page.TitleProperty, nameScope));
			xamlServiceProvider5.Add(typeof(IReferenceProvider), obj9);
			Type typeFromHandle10 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver5 = new XmlNamespaceResolver();
			xmlNamespaceResolver5.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver5.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver5.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver5.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver5.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver5.Add("pids", "clr-namespace:CarScannerXamarinForms.OBD2.PIDS");
			xmlNamespaceResolver5.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xmlNamespaceResolver5.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider5.Add(typeFromHandle10, new XamlTypeResolver(xmlNamespaceResolver5, typeof(EcoTestPageV2).GetTypeInfo().Assembly));
			xamlServiceProvider5.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(13, 5)));
			object obj10 = markupExtension5.ProvideValue(xamlServiceProvider5);
			this.Title = obj10;
			this.SetValue(Page.UseSafeAreaProperty, true);
			dynamicResourceExtension.Key = "BackgroundColor";
			IMarkupExtension<DynamicResource> markupExtension6 = dynamicResourceExtension;
			XamlServiceProvider xamlServiceProvider6 = new XamlServiceProvider();
			Type typeFromHandle11 = typeof(IProvideValueTarget);
			object[] array6 = new object[0 + 1];
			array6[0] = this;
			object obj11;
			xamlServiceProvider6.Add(typeFromHandle11, obj11 = new SimpleValueTargetProvider(array6, VisualElement.BackgroundColorProperty, nameScope));
			xamlServiceProvider6.Add(typeof(IReferenceProvider), obj11);
			Type typeFromHandle12 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver6 = new XmlNamespaceResolver();
			xmlNamespaceResolver6.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver6.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver6.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver6.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver6.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver6.Add("pids", "clr-namespace:CarScannerXamarinForms.OBD2.PIDS");
			xmlNamespaceResolver6.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xmlNamespaceResolver6.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider6.Add(typeFromHandle12, new XamlTypeResolver(xmlNamespaceResolver6, typeof(EcoTestPageV2).GetTypeInfo().Assembly));
			xamlServiceProvider6.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(15, 5)));
			DynamicResource dynamicResource = markupExtension6.ProvideValue(xamlServiceProvider6);
			this.SetDynamicResource(VisualElement.BackgroundColorProperty, dynamicResource.Key);
			this.SetValue(NavigationPage.HasBackButtonProperty, false);
			this.SetValue(NavigationPage.HasNavigationBarProperty, true);
			this.Resources = resourceDictionary;
			on.Platform = new List<string>(2) { "Android", "WinPhone" };
			on.Value = "0";
			onPlatform.Platforms.Add(on);
			on2.Platform = new List<string>(1) { "iOS" };
			on2.Value = "5,20,5,5";
			onPlatform.Platforms.Add(on2);
			this.SetValue(Page.PaddingProperty, onPlatform);
			grid.SetValue(Grid.RowProperty, 0);
			grid.SetValue(View.MarginProperty, new Thickness(5.0, 0.0, 5.0, 0.0));
			columnDefinition.SetValue(ColumnDefinition.WidthProperty, new GridLengthTypeConverter().ConvertFromInvariantString("*"));
			grid.GetValue(Grid.ColumnDefinitionsProperty).Add(columnDefinition);
			columnDefinition2.SetValue(ColumnDefinition.WidthProperty, new GridLengthTypeConverter().ConvertFromInvariantString("auto"));
			grid.GetValue(Grid.ColumnDefinitionsProperty).Add(columnDefinition2);
			columnDefinition3.SetValue(ColumnDefinition.WidthProperty, new GridLengthTypeConverter().ConvertFromInvariantString("*"));
			grid.GetValue(Grid.ColumnDefinitionsProperty).Add(columnDefinition3);
			linkButton.SetValue(Grid.ColumnProperty, 0);
			linkButton.Clicked += this.btnBack_Clicked;
			linkButton.SetValue(View.HorizontalOptionsProperty, LayoutOptions.Start);
			dynamicResourceExtension2.Key = "NavigationBarButton";
			IMarkupExtension<DynamicResource> markupExtension7 = dynamicResourceExtension2;
			XamlServiceProvider xamlServiceProvider7 = new XamlServiceProvider();
			Type typeFromHandle13 = typeof(IProvideValueTarget);
			object[] array7 = new object[0 + 3];
			array7[0] = linkButton;
			array7[1] = grid;
			array7[2] = this;
			object obj12;
			xamlServiceProvider7.Add(typeFromHandle13, obj12 = new SimpleValueTargetProvider(array7, VisualElement.StyleProperty, nameScope));
			xamlServiceProvider7.Add(typeof(IReferenceProvider), obj12);
			Type typeFromHandle14 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver7 = new XmlNamespaceResolver();
			xmlNamespaceResolver7.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver7.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver7.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver7.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver7.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver7.Add("pids", "clr-namespace:CarScannerXamarinForms.OBD2.PIDS");
			xmlNamespaceResolver7.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xmlNamespaceResolver7.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider7.Add(typeFromHandle14, new XamlTypeResolver(xmlNamespaceResolver7, typeof(EcoTestPageV2).GetTypeInfo().Assembly));
			xamlServiceProvider7.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(58, 17)));
			DynamicResource dynamicResource2 = markupExtension7.ProvideValue(xamlServiceProvider7);
			linkButton.SetDynamicResource(VisualElement.StyleProperty, dynamicResource2.Key);
			translate6.Text = "ios_Back";
			IMarkupExtension markupExtension8 = translate6;
			XamlServiceProvider xamlServiceProvider8 = new XamlServiceProvider();
			Type typeFromHandle15 = typeof(IProvideValueTarget);
			object[] array8 = new object[0 + 3];
			array8[0] = linkButton;
			array8[1] = grid;
			array8[2] = this;
			object obj13;
			xamlServiceProvider8.Add(typeFromHandle15, obj13 = new SimpleValueTargetProvider(array8, Button.TextProperty, nameScope));
			xamlServiceProvider8.Add(typeof(IReferenceProvider), obj13);
			Type typeFromHandle16 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver8 = new XmlNamespaceResolver();
			xmlNamespaceResolver8.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver8.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver8.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver8.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver8.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver8.Add("pids", "clr-namespace:CarScannerXamarinForms.OBD2.PIDS");
			xmlNamespaceResolver8.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xmlNamespaceResolver8.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider8.Add(typeFromHandle16, new XamlTypeResolver(xmlNamespaceResolver8, typeof(EcoTestPageV2).GetTypeInfo().Assembly));
			xamlServiceProvider8.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(59, 17)));
			object obj14 = markupExtension8.ProvideValue(xamlServiceProvider8);
			linkButton.Text = obj14;
			linkButton.SetValue(View.VerticalOptionsProperty, LayoutOptions.Center);
			grid.Children.Add(linkButton);
			nonScalableLabel.SetValue(Grid.ColumnProperty, 1);
			dynamicResourceExtension3.Key = "NavigationBarNonScalableLabel";
			IMarkupExtension<DynamicResource> markupExtension9 = dynamicResourceExtension3;
			XamlServiceProvider xamlServiceProvider9 = new XamlServiceProvider();
			Type typeFromHandle17 = typeof(IProvideValueTarget);
			object[] array9 = new object[0 + 3];
			array9[0] = nonScalableLabel;
			array9[1] = grid;
			array9[2] = this;
			object obj15;
			xamlServiceProvider9.Add(typeFromHandle17, obj15 = new SimpleValueTargetProvider(array9, VisualElement.StyleProperty, nameScope));
			xamlServiceProvider9.Add(typeof(IReferenceProvider), obj15);
			Type typeFromHandle18 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver9 = new XmlNamespaceResolver();
			xmlNamespaceResolver9.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver9.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver9.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver9.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver9.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver9.Add("pids", "clr-namespace:CarScannerXamarinForms.OBD2.PIDS");
			xmlNamespaceResolver9.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xmlNamespaceResolver9.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider9.Add(typeFromHandle18, new XamlTypeResolver(xmlNamespaceResolver9, typeof(EcoTestPageV2).GetTypeInfo().Assembly));
			xamlServiceProvider9.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(70, 17)));
			DynamicResource dynamicResource3 = markupExtension9.ProvideValue(xamlServiceProvider9);
			nonScalableLabel.SetDynamicResource(VisualElement.StyleProperty, dynamicResource3.Key);
			translate7.Text = "ios_MainPage_TileEmissionTests";
			IMarkupExtension markupExtension10 = translate7;
			XamlServiceProvider xamlServiceProvider10 = new XamlServiceProvider();
			Type typeFromHandle19 = typeof(IProvideValueTarget);
			object[] array10 = new object[0 + 3];
			array10[0] = nonScalableLabel;
			array10[1] = grid;
			array10[2] = this;
			object obj16;
			xamlServiceProvider10.Add(typeFromHandle19, obj16 = new SimpleValueTargetProvider(array10, Label.TextProperty, nameScope));
			xamlServiceProvider10.Add(typeof(IReferenceProvider), obj16);
			Type typeFromHandle20 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver10 = new XmlNamespaceResolver();
			xmlNamespaceResolver10.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver10.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver10.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver10.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver10.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver10.Add("pids", "clr-namespace:CarScannerXamarinForms.OBD2.PIDS");
			xmlNamespaceResolver10.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xmlNamespaceResolver10.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider10.Add(typeFromHandle20, new XamlTypeResolver(xmlNamespaceResolver10, typeof(EcoTestPageV2).GetTypeInfo().Assembly));
			xamlServiceProvider10.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(71, 17)));
			object obj17 = markupExtension10.ProvideValue(xamlServiceProvider10);
			nonScalableLabel.Text = obj17;
			dynamicResourceExtension4.Key = "NavigationBarTextColor";
			IMarkupExtension<DynamicResource> markupExtension11 = dynamicResourceExtension4;
			XamlServiceProvider xamlServiceProvider11 = new XamlServiceProvider();
			Type typeFromHandle21 = typeof(IProvideValueTarget);
			object[] array11 = new object[0 + 3];
			array11[0] = nonScalableLabel;
			array11[1] = grid;
			array11[2] = this;
			object obj18;
			xamlServiceProvider11.Add(typeFromHandle21, obj18 = new SimpleValueTargetProvider(array11, Label.TextColorProperty, nameScope));
			xamlServiceProvider11.Add(typeof(IReferenceProvider), obj18);
			Type typeFromHandle22 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver11 = new XmlNamespaceResolver();
			xmlNamespaceResolver11.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver11.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver11.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver11.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver11.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver11.Add("pids", "clr-namespace:CarScannerXamarinForms.OBD2.PIDS");
			xmlNamespaceResolver11.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xmlNamespaceResolver11.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider11.Add(typeFromHandle22, new XamlTypeResolver(xmlNamespaceResolver11, typeof(EcoTestPageV2).GetTypeInfo().Assembly));
			xamlServiceProvider11.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(72, 17)));
			DynamicResource dynamicResource4 = markupExtension11.ProvideValue(xamlServiceProvider11);
			nonScalableLabel.SetDynamicResource(Label.TextColorProperty, dynamicResource4.Key);
			nonScalableLabel.SetValue(View.VerticalOptionsProperty, LayoutOptions.Center);
			nonScalableLabel.SetValue(Label.VerticalTextAlignmentProperty, new TextAlignmentConverter().ConvertFromInvariantString("Center"));
			grid.Children.Add(nonScalableLabel);
			linkButton2.SetValue(Grid.ColumnProperty, 2);
			linkButton2.Clicked += this.btnInfo_Clicked;
			linkButton2.SetValue(View.HorizontalOptionsProperty, LayoutOptions.End);
			dynamicResourceExtension5.Key = "InfoImageNavigationBarTextColor";
			IMarkupExtension<DynamicResource> markupExtension12 = dynamicResourceExtension5;
			XamlServiceProvider xamlServiceProvider12 = new XamlServiceProvider();
			Type typeFromHandle23 = typeof(IProvideValueTarget);
			object[] array12 = new object[0 + 3];
			array12[0] = linkButton2;
			array12[1] = grid;
			array12[2] = this;
			object obj19;
			xamlServiceProvider12.Add(typeFromHandle23, obj19 = new SimpleValueTargetProvider(array12, Button.ImageProperty, nameScope));
			xamlServiceProvider12.Add(typeof(IReferenceProvider), obj19);
			Type typeFromHandle24 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver12 = new XmlNamespaceResolver();
			xmlNamespaceResolver12.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver12.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver12.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver12.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver12.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver12.Add("pids", "clr-namespace:CarScannerXamarinForms.OBD2.PIDS");
			xmlNamespaceResolver12.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xmlNamespaceResolver12.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider12.Add(typeFromHandle24, new XamlTypeResolver(xmlNamespaceResolver12, typeof(EcoTestPageV2).GetTypeInfo().Assembly));
			xamlServiceProvider12.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(79, 17)));
			DynamicResource dynamicResource5 = markupExtension12.ProvideValue(xamlServiceProvider12);
			linkButton2.SetDynamicResource(Button.ImageProperty, dynamicResource5.Key);
			dynamicResourceExtension6.Key = "NavigationBarButton";
			IMarkupExtension<DynamicResource> markupExtension13 = dynamicResourceExtension6;
			XamlServiceProvider xamlServiceProvider13 = new XamlServiceProvider();
			Type typeFromHandle25 = typeof(IProvideValueTarget);
			object[] array13 = new object[0 + 3];
			array13[0] = linkButton2;
			array13[1] = grid;
			array13[2] = this;
			object obj20;
			xamlServiceProvider13.Add(typeFromHandle25, obj20 = new SimpleValueTargetProvider(array13, VisualElement.StyleProperty, nameScope));
			xamlServiceProvider13.Add(typeof(IReferenceProvider), obj20);
			Type typeFromHandle26 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver13 = new XmlNamespaceResolver();
			xmlNamespaceResolver13.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver13.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver13.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver13.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver13.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver13.Add("pids", "clr-namespace:CarScannerXamarinForms.OBD2.PIDS");
			xmlNamespaceResolver13.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xmlNamespaceResolver13.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider13.Add(typeFromHandle26, new XamlTypeResolver(xmlNamespaceResolver13, typeof(EcoTestPageV2).GetTypeInfo().Assembly));
			xamlServiceProvider13.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(80, 17)));
			DynamicResource dynamicResource6 = markupExtension13.ProvideValue(xamlServiceProvider13);
			linkButton2.SetDynamicResource(VisualElement.StyleProperty, dynamicResource6.Key);
			linkButton2.SetValue(View.VerticalOptionsProperty, LayoutOptions.Center);
			on3.Platform = new List<string>(1) { "iOS" };
			on3.Value = "0";
			onPlatform2.Platforms.Add(on3);
			on4.Platform = new List<string>(1) { "Android" };
			on4.Value = "0,0,5,0";
			onPlatform2.Platforms.Add(on4);
			linkButton2.SetValue(View.MarginProperty, onPlatform2);
			grid.Children.Add(linkButton2);
			this.SetValue(NavigationPage.TitleViewProperty, grid);
			referenceExtension.Name = "page";
			IMarkupExtension markupExtension14 = referenceExtension;
			XamlServiceProvider xamlServiceProvider14 = new XamlServiceProvider();
			Type typeFromHandle27 = typeof(IProvideValueTarget);
			object[] array14 = new object[0 + 2];
			array14[0] = settingsView;
			array14[1] = this;
			object obj21;
			xamlServiceProvider14.Add(typeFromHandle27, obj21 = new SimpleValueTargetProvider(array14, BindableObject.BindingContextProperty, nameScope));
			xamlServiceProvider14.Add(typeof(IReferenceProvider), obj21);
			Type typeFromHandle28 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver14 = new XmlNamespaceResolver();
			xmlNamespaceResolver14.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver14.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver14.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver14.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver14.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver14.Add("pids", "clr-namespace:CarScannerXamarinForms.OBD2.PIDS");
			xmlNamespaceResolver14.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xmlNamespaceResolver14.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider14.Add(typeFromHandle28, new XamlTypeResolver(xmlNamespaceResolver14, typeof(EcoTestPageV2).GetTypeInfo().Assembly));
			xamlServiceProvider14.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(95, 13)));
			object obj22 = markupExtension14.ProvideValue(xamlServiceProvider14);
			settingsView.SetValue(BindableObject.BindingContextProperty, obj22);
			settingsView.SetValue(TableView.HasUnevenRowsProperty, true);
			translate8.Text = "ios_EmissionTests_SinceDTCReset";
			IMarkupExtension markupExtension15 = translate8;
			XamlServiceProvider xamlServiceProvider15 = new XamlServiceProvider();
			Type typeFromHandle29 = typeof(IProvideValueTarget);
			object[] array15 = new object[0 + 3];
			array15[0] = section;
			array15[1] = settingsView;
			array15[2] = this;
			object obj23;
			xamlServiceProvider15.Add(typeFromHandle29, obj23 = new SimpleValueTargetProvider(array15, SectionBase.TitleProperty, nameScope));
			xamlServiceProvider15.Add(typeof(IReferenceProvider), obj23);
			Type typeFromHandle30 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver15 = new XmlNamespaceResolver();
			xmlNamespaceResolver15.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver15.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver15.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver15.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver15.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver15.Add("pids", "clr-namespace:CarScannerXamarinForms.OBD2.PIDS");
			xmlNamespaceResolver15.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xmlNamespaceResolver15.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider15.Add(typeFromHandle30, new XamlTypeResolver(xmlNamespaceResolver15, typeof(EcoTestPageV2).GetTypeInfo().Assembly));
			xamlServiceProvider15.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(99, 17)));
			object obj24 = markupExtension15.ProvideValue(xamlServiceProvider15);
			section.Title = obj24;
			bindingExtension.Mode = 2;
			bindingExtension.Path = "TestsSinceDTC";
			BindingBase bindingBase = bindingExtension.ProvideValue(null);
			section.SetBinding(Section.ItemsSourceProperty, bindingBase);
			IDataTemplate dataTemplate3 = dataTemplate;
			EcoTestPageV2.<InitializeComponent>_anonXamlCDataTemplate_52 <InitializeComponent>_anonXamlCDataTemplate_ = new EcoTestPageV2.<InitializeComponent>_anonXamlCDataTemplate_52();
			object[] array16 = new object[0 + 4];
			array16[0] = dataTemplate;
			array16[1] = section;
			array16[2] = settingsView;
			array16[3] = this;
			<InitializeComponent>_anonXamlCDataTemplate_.parentValues = array16;
			<InitializeComponent>_anonXamlCDataTemplate_.root = this;
			dataTemplate3.LoadTemplate = new Func<object>(<InitializeComponent>_anonXamlCDataTemplate_.LoadDataTemplate);
			section.SetValue(Section.ItemTemplateProperty, dataTemplate);
			settingsView.Root.Add(section);
			translate9.Text = "ios_EmissionTests_CurrentDriveCycle";
			IMarkupExtension markupExtension16 = translate9;
			XamlServiceProvider xamlServiceProvider16 = new XamlServiceProvider();
			Type typeFromHandle31 = typeof(IProvideValueTarget);
			object[] array17 = new object[0 + 3];
			array17[0] = section2;
			array17[1] = settingsView;
			array17[2] = this;
			object obj25;
			xamlServiceProvider16.Add(typeFromHandle31, obj25 = new SimpleValueTargetProvider(array17, SectionBase.TitleProperty, nameScope));
			xamlServiceProvider16.Add(typeof(IReferenceProvider), obj25);
			Type typeFromHandle32 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver16 = new XmlNamespaceResolver();
			xmlNamespaceResolver16.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver16.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver16.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
			xmlNamespaceResolver16.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
			xmlNamespaceResolver16.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver16.Add("pids", "clr-namespace:CarScannerXamarinForms.OBD2.PIDS");
			xmlNamespaceResolver16.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xmlNamespaceResolver16.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
			xamlServiceProvider16.Add(typeFromHandle32, new XamlTypeResolver(xmlNamespaceResolver16, typeof(EcoTestPageV2).GetTypeInfo().Assembly));
			xamlServiceProvider16.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(114, 17)));
			object obj26 = markupExtension16.ProvideValue(xamlServiceProvider16);
			section2.Title = obj26;
			bindingExtension2.Mode = 2;
			bindingExtension2.Path = "TestsCurrentCycle";
			BindingBase bindingBase2 = bindingExtension2.ProvideValue(null);
			section2.SetBinding(Section.ItemsSourceProperty, bindingBase2);
			IDataTemplate dataTemplate4 = dataTemplate2;
			EcoTestPageV2.<InitializeComponent>_anonXamlCDataTemplate_53 <InitializeComponent>_anonXamlCDataTemplate_2 = new EcoTestPageV2.<InitializeComponent>_anonXamlCDataTemplate_53();
			object[] array18 = new object[0 + 4];
			array18[0] = dataTemplate2;
			array18[1] = section2;
			array18[2] = settingsView;
			array18[3] = this;
			<InitializeComponent>_anonXamlCDataTemplate_2.parentValues = array18;
			<InitializeComponent>_anonXamlCDataTemplate_2.root = this;
			dataTemplate4.LoadTemplate = new Func<object>(<InitializeComponent>_anonXamlCDataTemplate_2.LoadDataTemplate);
			section2.SetValue(Section.ItemTemplateProperty, dataTemplate2);
			settingsView.Root.Add(section2);
			this.SetValue(ContentPage.ContentProperty, settingsView);
		}

		// Token: 0x060036EA RID: 14058 RVA: 0x0028CF78 File Offset: 0x0028B178
		private void __InitComponentRuntime()
		{
			Extensions.LoadFromXaml<EcoTestPageV2>(this, typeof(EcoTestPageV2));
			this.page = NameScopeExtensions.FindByName<ContentPage>(this, "page");
			this.settingsLayoutRoot = NameScopeExtensions.FindByName<SettingsView>(this, "settingsLayoutRoot");
			this.sectSinceDTC = NameScopeExtensions.FindByName<Section>(this, "sectSinceDTC");
			this.sectCurrentCycle = NameScopeExtensions.FindByName<Section>(this, "sectCurrentCycle");
		}

		// Token: 0x0400211E RID: 8478
		private PID_Status pidSinceDTC;

		// Token: 0x0400211F RID: 8479
		private PID_Status pidCurrentCycle;

		// Token: 0x04002120 RID: 8480
		[CompilerGenerated]
		private ObservableCollection<ECUTest> <TestsSinceDTC>k__BackingField;

		// Token: 0x04002121 RID: 8481
		[CompilerGenerated]
		private ObservableCollection<ECUTest> <TestsCurrentCycle>k__BackingField;

		// Token: 0x04002122 RID: 8482
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private ContentPage page;

		// Token: 0x04002123 RID: 8483
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private SettingsView settingsLayoutRoot;

		// Token: 0x04002124 RID: 8484
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private Section sectSinceDTC;

		// Token: 0x04002125 RID: 8485
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private Section sectCurrentCycle;

		// Token: 0x0200061E RID: 1566
		[CompilerGenerated]
		[Serializable]
		private sealed class <>c
		{
			// Token: 0x060036EB RID: 14059 RVA: 0x0028CFDA File Offset: 0x0028B1DA
			// Note: this type is marked as 'beforefieldinit'.
			static <>c()
			{
			}

			// Token: 0x060036EC RID: 14060 RVA: 0x00002050 File Offset: 0x00000250
			public <>c()
			{
			}

			// Token: 0x060036ED RID: 14061 RVA: 0x0028CFE6 File Offset: 0x0028B1E6
			internal bool <Init>b__5_0(PID x)
			{
				return x.Id == 2;
			}

			// Token: 0x060036EE RID: 14062 RVA: 0x0028CFF1 File Offset: 0x0028B1F1
			internal bool <Init>b__5_1(PID x)
			{
				return x.Id == 89;
			}

			// Token: 0x060036EF RID: 14063 RVA: 0x0028CFE6 File Offset: 0x0028B1E6
			internal bool <setDataSource>b__17_0(PID x)
			{
				return x.Id == 2;
			}

			// Token: 0x060036F0 RID: 14064 RVA: 0x0028CFF1 File Offset: 0x0028B1F1
			internal bool <setDataSource>b__17_1(PID x)
			{
				return x.Id == 89;
			}

			// Token: 0x060036F1 RID: 14065 RVA: 0x0028CFFD File Offset: 0x0028B1FD
			internal object <setDataSource>b__17_2(object obj)
			{
				if (((ECUTest)obj).Cycle == TestPidCycle.SinceDTCCleared)
				{
					return "Since DTC Cleared";
				}
				return "Current drive cycle";
			}

			// Token: 0x04002126 RID: 8486
			public static readonly EcoTestPageV2.<>c <>9 = new EcoTestPageV2.<>c();

			// Token: 0x04002127 RID: 8487
			public static Func<PID, bool> <>9__5_0;

			// Token: 0x04002128 RID: 8488
			public static Func<PID, bool> <>9__5_1;

			// Token: 0x04002129 RID: 8489
			public static Func<PID, bool> <>9__17_0;

			// Token: 0x0400212A RID: 8490
			public static Func<PID, bool> <>9__17_1;

			// Token: 0x0400212B RID: 8491
			public static Func<object, object> <>9__17_2;
		}

		// Token: 0x0200061F RID: 1567
		[CompilerGenerated]
		private sealed class <>c__DisplayClass15_0
		{
			// Token: 0x060036F2 RID: 14066 RVA: 0x00002050 File Offset: 0x00000250
			public <>c__DisplayClass15_0()
			{
			}

			// Token: 0x060036F3 RID: 14067 RVA: 0x0028D018 File Offset: 0x0028B218
			internal void <PidCurrentCycle_ValueChanged>b__0()
			{
				PID_Status pid_Status = this.e as PID_Status;
				if (pid_Status != null)
				{
					ECUTest[] ecutests = pid_Status.Value.ECUTests;
					for (int i = 0; i < ecutests.Length; i++)
					{
						EcoTestPageV2.<>c__DisplayClass15_1 CS$<>8__locals1 = new EcoTestPageV2.<>c__DisplayClass15_1();
						CS$<>8__locals1.t = ecutests[i];
						ECUTest ecutest = this.<>4__this.TestsCurrentCycle.FirstOrDefault((ECUTest x) => x.Name == CS$<>8__locals1.t.Name);
						if (ecutest == null)
						{
							this.<>4__this.TestsCurrentCycle.Add(CS$<>8__locals1.t);
						}
						else if (ecutest.Complete != CS$<>8__locals1.t.Complete || ecutest.Available != CS$<>8__locals1.t.Available)
						{
							int num = this.<>4__this.TestsCurrentCycle.IndexOf(ecutest);
							this.<>4__this.TestsCurrentCycle[num] = CS$<>8__locals1.t;
						}
					}
				}
			}

			// Token: 0x0400212C RID: 8492
			public PID e;

			// Token: 0x0400212D RID: 8493
			public EcoTestPageV2 <>4__this;
		}

		// Token: 0x02000620 RID: 1568
		[CompilerGenerated]
		private sealed class <>c__DisplayClass15_1
		{
			// Token: 0x060036F4 RID: 14068 RVA: 0x00002050 File Offset: 0x00000250
			public <>c__DisplayClass15_1()
			{
			}

			// Token: 0x060036F5 RID: 14069 RVA: 0x0028D0F4 File Offset: 0x0028B2F4
			internal bool <PidCurrentCycle_ValueChanged>b__1(ECUTest x)
			{
				return x.Name == this.t.Name;
			}

			// Token: 0x0400212E RID: 8494
			public ECUTest t;
		}

		// Token: 0x02000621 RID: 1569
		[CompilerGenerated]
		private sealed class <>c__DisplayClass16_0
		{
			// Token: 0x060036F6 RID: 14070 RVA: 0x00002050 File Offset: 0x00000250
			public <>c__DisplayClass16_0()
			{
			}

			// Token: 0x060036F7 RID: 14071 RVA: 0x0028D10C File Offset: 0x0028B30C
			internal void <PidSinceDTC_ValueChanged>b__0()
			{
				PID_Status pid_Status = this.e as PID_Status;
				if (pid_Status != null)
				{
					ECUTest[] ecutests = pid_Status.Value.ECUTests;
					for (int i = 0; i < ecutests.Length; i++)
					{
						EcoTestPageV2.<>c__DisplayClass16_1 CS$<>8__locals1 = new EcoTestPageV2.<>c__DisplayClass16_1();
						CS$<>8__locals1.t = ecutests[i];
						ECUTest ecutest = this.<>4__this.TestsSinceDTC.FirstOrDefault((ECUTest x) => x.Name == CS$<>8__locals1.t.Name);
						if (ecutest == null)
						{
							this.<>4__this.TestsSinceDTC.Add(CS$<>8__locals1.t);
						}
						else if (ecutest.Complete != CS$<>8__locals1.t.Complete || ecutest.Available != CS$<>8__locals1.t.Available)
						{
							int num = this.<>4__this.TestsSinceDTC.IndexOf(ecutest);
							this.<>4__this.TestsSinceDTC[num] = CS$<>8__locals1.t;
						}
					}
				}
			}

			// Token: 0x0400212F RID: 8495
			public PID e;

			// Token: 0x04002130 RID: 8496
			public EcoTestPageV2 <>4__this;
		}

		// Token: 0x02000622 RID: 1570
		[CompilerGenerated]
		private sealed class <>c__DisplayClass16_1
		{
			// Token: 0x060036F8 RID: 14072 RVA: 0x00002050 File Offset: 0x00000250
			public <>c__DisplayClass16_1()
			{
			}

			// Token: 0x060036F9 RID: 14073 RVA: 0x0028D1E8 File Offset: 0x0028B3E8
			internal bool <PidSinceDTC_ValueChanged>b__1(ECUTest x)
			{
				return x.Name == this.t.Name;
			}

			// Token: 0x04002131 RID: 8497
			public ECUTest t;
		}

		// Token: 0x02000623 RID: 1571
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <btnBack_Clicked>d__2 : IAsyncStateMachine
		{
			// Token: 0x060036FA RID: 14074 RVA: 0x0028D200 File Offset: 0x0028B400
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				EcoTestPageV2 ecoTestPageV = this;
				try
				{
					if (num != 0)
					{
						if (ecoTestPageV.pidSinceDTC != null)
						{
							ecoTestPageV.pidSinceDTC.ValueChanged -= ecoTestPageV.PidSinceDTC_ValueChanged;
						}
						if (ecoTestPageV.pidCurrentCycle != null)
						{
							ecoTestPageV.pidCurrentCycle.ValueChanged -= ecoTestPageV.PidCurrentCycle_ValueChanged;
						}
					}
					try
					{
						TaskAwaiter<Page> taskAwaiter;
						if (num != 0)
						{
							taskAwaiter = ecoTestPageV.Navigation.PopAsync().GetAwaiter();
							if (!taskAwaiter.IsCompleted)
							{
								num2 = 0;
								TaskAwaiter<Page> taskAwaiter2 = taskAwaiter;
								this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<Page>, EcoTestPageV2.<btnBack_Clicked>d__2>(ref taskAwaiter, ref this);
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

			// Token: 0x060036FB RID: 14075 RVA: 0x0028D30C File Offset: 0x0028B50C
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x04002132 RID: 8498
			public int <>1__state;

			// Token: 0x04002133 RID: 8499
			public AsyncVoidMethodBuilder <>t__builder;

			// Token: 0x04002134 RID: 8500
			public EcoTestPageV2 <>4__this;

			// Token: 0x04002135 RID: 8501
			private TaskAwaiter<Page> <>u__1;
		}

		// Token: 0x02000624 RID: 1572
		[CompilerGenerated]
		private sealed class <InitializeComponent>_anonXamlCDataTemplate_52
		{
			// Token: 0x060036FC RID: 14076 RVA: 0x0028D31C File Offset: 0x0028B51C
			public <InitializeComponent>_anonXamlCDataTemplate_52()
			{
			}

			// Token: 0x060036FD RID: 14077 RVA: 0x0028D330 File Offset: 0x0028B530
			internal object LoadDataTemplate()
			{
				BindingExtension bindingExtension;
				VisualDiagnostics.RegisterSourceInfo(bindingExtension = new BindingExtension(), new Uri("Pages\\EcoTestPageV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 104, 29);
				StaticResourceExtension staticResourceExtension;
				VisualDiagnostics.RegisterSourceInfo(staticResourceExtension = new StaticResourceExtension(), new Uri("Pages\\EcoTestPageV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 105, 29);
				BindingExtension bindingExtension2;
				VisualDiagnostics.RegisterSourceInfo(bindingExtension2 = new BindingExtension(), new Uri("Pages\\EcoTestPageV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 105, 29);
				StaticResourceExtension staticResourceExtension2;
				VisualDiagnostics.RegisterSourceInfo(staticResourceExtension2 = new StaticResourceExtension(), new Uri("Pages\\EcoTestPageV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 106, 29);
				BindingExtension bindingExtension3;
				VisualDiagnostics.RegisterSourceInfo(bindingExtension3 = new BindingExtension(), new Uri("Pages\\EcoTestPageV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 106, 29);
				StaticResourceExtension staticResourceExtension3;
				VisualDiagnostics.RegisterSourceInfo(staticResourceExtension3 = new StaticResourceExtension(), new Uri("Pages\\EcoTestPageV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 107, 29);
				BindingExtension bindingExtension4;
				VisualDiagnostics.RegisterSourceInfo(bindingExtension4 = new BindingExtension(), new Uri("Pages\\EcoTestPageV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 107, 29);
				StaticResourceExtension staticResourceExtension4;
				VisualDiagnostics.RegisterSourceInfo(staticResourceExtension4 = new StaticResourceExtension(), new Uri("Pages\\EcoTestPageV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 108, 29);
				BindingExtension bindingExtension5;
				VisualDiagnostics.RegisterSourceInfo(bindingExtension5 = new BindingExtension(), new Uri("Pages\\EcoTestPageV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 108, 29);
				LabelCell labelCell;
				VisualDiagnostics.RegisterSourceInfo(labelCell = new LabelCell(), new Uri("Pages\\EcoTestPageV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 103, 26);
				NameScope nameScope = new NameScope();
				NameScope.SetNameScope(labelCell, nameScope);
				bindingExtension.Path = "Name";
				bindingExtension.TypedBinding = new TypedBinding<ECUTest, string>(delegate(ECUTest A_0)
				{
					if (A_0 != null)
					{
						return new ValueTuple<string, bool>(A_0.Name, true);
					}
					return default(ValueTuple<string, bool>);
				}, delegate(ECUTest A_0, string A_1)
				{
					if (A_0 != null)
					{
						A_0.Name = A_1;
						return;
					}
				}, new Tuple<Func<ECUTest, object>, string>[]
				{
					new Tuple<Func<ECUTest, object>, string>((ECUTest A_0) => A_0, "Name")
				});
				BindingBase bindingBase = bindingExtension.ProvideValue(null);
				labelCell.SetBinding(CellBase.TitleProperty, bindingBase);
				bindingExtension2.Mode = 2;
				staticResourceExtension.Key = "AvailableConverter";
				IMarkupExtension markupExtension = staticResourceExtension;
				XamlServiceProvider xamlServiceProvider = new XamlServiceProvider();
				Type typeFromHandle = typeof(IProvideValueTarget);
				int num;
				object[] array = new object[(num = this.parentValues.Length) + 2];
				Array.Copy(this.parentValues, 0, array, 2, num);
				object[] array2 = array;
				array2[0] = bindingExtension2;
				array2[1] = labelCell;
				object obj;
				xamlServiceProvider.Add(typeFromHandle, obj = new SimpleValueTargetProvider(array2, typeof(BindingExtension).GetRuntimeProperty("Converter"), nameScope));
				xamlServiceProvider.Add(typeof(IReferenceProvider), obj);
				Type typeFromHandle2 = typeof(IXamlTypeResolver);
				XmlNamespaceResolver xmlNamespaceResolver = new XmlNamespaceResolver();
				xmlNamespaceResolver.Add("", "http://xamarin.com/schemas/2014/forms");
				xmlNamespaceResolver.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
				xmlNamespaceResolver.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
				xmlNamespaceResolver.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
				xmlNamespaceResolver.Add("local", "clr-namespace:CarScannerXamarinForms");
				xmlNamespaceResolver.Add("pids", "clr-namespace:CarScannerXamarinForms.OBD2.PIDS");
				xmlNamespaceResolver.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
				xmlNamespaceResolver.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
				xamlServiceProvider.Add(typeFromHandle2, new XamlTypeResolver(xmlNamespaceResolver, typeof(EcoTestPageV2.<InitializeComponent>_anonXamlCDataTemplate_52).GetTypeInfo().Assembly));
				xamlServiceProvider.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(105, 29)));
				object obj2 = markupExtension.ProvideValue(xamlServiceProvider);
				bindingExtension2.Converter = obj2;
				bindingExtension2.Path = "Available";
				bindingExtension2.TypedBinding = new TypedBinding<ECUTest, bool>(delegate(ECUTest A_0)
				{
					if (A_0 != null)
					{
						return new ValueTuple<bool, bool>(A_0.Available, true);
					}
					return default(ValueTuple<bool, bool>);
				}, null, new Tuple<Func<ECUTest, object>, string>[]
				{
					new Tuple<Func<ECUTest, object>, string>((ECUTest A_0) => A_0, "Available")
				});
				BindingBase bindingBase2 = bindingExtension2.ProvideValue(null);
				labelCell.SetBinding(CellBase.HintTextProperty, bindingBase2);
				bindingExtension3.Mode = 2;
				staticResourceExtension2.Key = "ColorConverter";
				IMarkupExtension markupExtension2 = staticResourceExtension2;
				XamlServiceProvider xamlServiceProvider2 = new XamlServiceProvider();
				Type typeFromHandle3 = typeof(IProvideValueTarget);
				int num2;
				object[] array3 = new object[(num2 = this.parentValues.Length) + 2];
				Array.Copy(this.parentValues, 0, array3, 2, num2);
				object[] array4 = array3;
				array4[0] = bindingExtension3;
				array4[1] = labelCell;
				object obj3;
				xamlServiceProvider2.Add(typeFromHandle3, obj3 = new SimpleValueTargetProvider(array4, typeof(BindingExtension).GetRuntimeProperty("Converter"), nameScope));
				xamlServiceProvider2.Add(typeof(IReferenceProvider), obj3);
				Type typeFromHandle4 = typeof(IXamlTypeResolver);
				XmlNamespaceResolver xmlNamespaceResolver2 = new XmlNamespaceResolver();
				xmlNamespaceResolver2.Add("", "http://xamarin.com/schemas/2014/forms");
				xmlNamespaceResolver2.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
				xmlNamespaceResolver2.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
				xmlNamespaceResolver2.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
				xmlNamespaceResolver2.Add("local", "clr-namespace:CarScannerXamarinForms");
				xmlNamespaceResolver2.Add("pids", "clr-namespace:CarScannerXamarinForms.OBD2.PIDS");
				xmlNamespaceResolver2.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
				xmlNamespaceResolver2.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
				xamlServiceProvider2.Add(typeFromHandle4, new XamlTypeResolver(xmlNamespaceResolver2, typeof(EcoTestPageV2.<InitializeComponent>_anonXamlCDataTemplate_52).GetTypeInfo().Assembly));
				xamlServiceProvider2.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(106, 29)));
				object obj4 = markupExtension2.ProvideValue(xamlServiceProvider2);
				bindingExtension3.Converter = obj4;
				bindingExtension3.Path = "Available";
				bindingExtension3.TypedBinding = new TypedBinding<ECUTest, bool>(delegate(ECUTest A_0)
				{
					if (A_0 != null)
					{
						return new ValueTuple<bool, bool>(A_0.Available, true);
					}
					return default(ValueTuple<bool, bool>);
				}, null, new Tuple<Func<ECUTest, object>, string>[]
				{
					new Tuple<Func<ECUTest, object>, string>((ECUTest A_0) => A_0, "Available")
				});
				BindingBase bindingBase3 = bindingExtension3.ProvideValue(null);
				labelCell.SetBinding(CellBase.HintTextColorProperty, bindingBase3);
				bindingExtension4.Mode = 2;
				staticResourceExtension3.Key = "CompleteConverter";
				IMarkupExtension markupExtension3 = staticResourceExtension3;
				XamlServiceProvider xamlServiceProvider3 = new XamlServiceProvider();
				Type typeFromHandle5 = typeof(IProvideValueTarget);
				int num3;
				object[] array5 = new object[(num3 = this.parentValues.Length) + 2];
				Array.Copy(this.parentValues, 0, array5, 2, num3);
				object[] array6 = array5;
				array6[0] = bindingExtension4;
				array6[1] = labelCell;
				object obj5;
				xamlServiceProvider3.Add(typeFromHandle5, obj5 = new SimpleValueTargetProvider(array6, typeof(BindingExtension).GetRuntimeProperty("Converter"), nameScope));
				xamlServiceProvider3.Add(typeof(IReferenceProvider), obj5);
				Type typeFromHandle6 = typeof(IXamlTypeResolver);
				XmlNamespaceResolver xmlNamespaceResolver3 = new XmlNamespaceResolver();
				xmlNamespaceResolver3.Add("", "http://xamarin.com/schemas/2014/forms");
				xmlNamespaceResolver3.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
				xmlNamespaceResolver3.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
				xmlNamespaceResolver3.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
				xmlNamespaceResolver3.Add("local", "clr-namespace:CarScannerXamarinForms");
				xmlNamespaceResolver3.Add("pids", "clr-namespace:CarScannerXamarinForms.OBD2.PIDS");
				xmlNamespaceResolver3.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
				xmlNamespaceResolver3.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
				xamlServiceProvider3.Add(typeFromHandle6, new XamlTypeResolver(xmlNamespaceResolver3, typeof(EcoTestPageV2.<InitializeComponent>_anonXamlCDataTemplate_52).GetTypeInfo().Assembly));
				xamlServiceProvider3.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(107, 29)));
				object obj6 = markupExtension3.ProvideValue(xamlServiceProvider3);
				bindingExtension4.Converter = obj6;
				bindingExtension4.Path = "Complete";
				bindingExtension4.TypedBinding = new TypedBinding<ECUTest, bool>(delegate(ECUTest A_0)
				{
					if (A_0 != null)
					{
						return new ValueTuple<bool, bool>(A_0.Complete, true);
					}
					return default(ValueTuple<bool, bool>);
				}, null, new Tuple<Func<ECUTest, object>, string>[]
				{
					new Tuple<Func<ECUTest, object>, string>((ECUTest A_0) => A_0, "Complete")
				});
				BindingBase bindingBase4 = bindingExtension4.ProvideValue(null);
				labelCell.SetBinding(LabelCell.ValueTextProperty, bindingBase4);
				bindingExtension5.Mode = 2;
				staticResourceExtension4.Key = "ColorConverter";
				IMarkupExtension markupExtension4 = staticResourceExtension4;
				XamlServiceProvider xamlServiceProvider4 = new XamlServiceProvider();
				Type typeFromHandle7 = typeof(IProvideValueTarget);
				int num4;
				object[] array7 = new object[(num4 = this.parentValues.Length) + 2];
				Array.Copy(this.parentValues, 0, array7, 2, num4);
				object[] array8 = array7;
				array8[0] = bindingExtension5;
				array8[1] = labelCell;
				object obj7;
				xamlServiceProvider4.Add(typeFromHandle7, obj7 = new SimpleValueTargetProvider(array8, typeof(BindingExtension).GetRuntimeProperty("Converter"), nameScope));
				xamlServiceProvider4.Add(typeof(IReferenceProvider), obj7);
				Type typeFromHandle8 = typeof(IXamlTypeResolver);
				XmlNamespaceResolver xmlNamespaceResolver4 = new XmlNamespaceResolver();
				xmlNamespaceResolver4.Add("", "http://xamarin.com/schemas/2014/forms");
				xmlNamespaceResolver4.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
				xmlNamespaceResolver4.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
				xmlNamespaceResolver4.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
				xmlNamespaceResolver4.Add("local", "clr-namespace:CarScannerXamarinForms");
				xmlNamespaceResolver4.Add("pids", "clr-namespace:CarScannerXamarinForms.OBD2.PIDS");
				xmlNamespaceResolver4.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
				xmlNamespaceResolver4.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
				xamlServiceProvider4.Add(typeFromHandle8, new XamlTypeResolver(xmlNamespaceResolver4, typeof(EcoTestPageV2.<InitializeComponent>_anonXamlCDataTemplate_52).GetTypeInfo().Assembly));
				xamlServiceProvider4.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(108, 29)));
				object obj8 = markupExtension4.ProvideValue(xamlServiceProvider4);
				bindingExtension5.Converter = obj8;
				bindingExtension5.Path = "Complete";
				bindingExtension5.TypedBinding = new TypedBinding<ECUTest, bool>(delegate(ECUTest A_0)
				{
					if (A_0 != null)
					{
						return new ValueTuple<bool, bool>(A_0.Complete, true);
					}
					return default(ValueTuple<bool, bool>);
				}, null, new Tuple<Func<ECUTest, object>, string>[]
				{
					new Tuple<Func<ECUTest, object>, string>((ECUTest A_0) => A_0, "Complete")
				});
				BindingBase bindingBase5 = bindingExtension5.ProvideValue(null);
				labelCell.SetBinding(LabelCell.ValueTextColorProperty, bindingBase5);
				return labelCell;
			}

			// Token: 0x060036FE RID: 14078 RVA: 0x0028DC64 File Offset: 0x0028BE64
			[CompilerGenerated]
			private static ValueTuple<string, bool> <LoadDataTemplate>typedBindingsM__1334(ECUTest A_0)
			{
				if (A_0 != null)
				{
					return new ValueTuple<string, bool>(A_0.Name, true);
				}
				return default(ValueTuple<string, bool>);
			}

			// Token: 0x060036FF RID: 14079 RVA: 0x0028DC94 File Offset: 0x0028BE94
			[CompilerGenerated]
			private static void <LoadDataTemplate>typedBindingsM__1335(ECUTest A_0, string A_1)
			{
				if (A_0 != null)
				{
					A_0.Name = A_1;
					return;
				}
			}

			// Token: 0x06003700 RID: 14080 RVA: 0x0028DCB0 File Offset: 0x0028BEB0
			[CompilerGenerated]
			private static object <LoadDataTemplate>typedBindingsM__1336(ECUTest A_0)
			{
				return A_0;
			}

			// Token: 0x06003701 RID: 14081 RVA: 0x0028DCC0 File Offset: 0x0028BEC0
			[CompilerGenerated]
			private static ValueTuple<bool, bool> <LoadDataTemplate>typedBindingsM__1337(ECUTest A_0)
			{
				if (A_0 != null)
				{
					return new ValueTuple<bool, bool>(A_0.Available, true);
				}
				return default(ValueTuple<bool, bool>);
			}

			// Token: 0x06003702 RID: 14082 RVA: 0x0028DCF0 File Offset: 0x0028BEF0
			[CompilerGenerated]
			private static object <LoadDataTemplate>typedBindingsM__1338(ECUTest A_0)
			{
				return A_0;
			}

			// Token: 0x06003703 RID: 14083 RVA: 0x0028DD00 File Offset: 0x0028BF00
			[CompilerGenerated]
			private static ValueTuple<bool, bool> <LoadDataTemplate>typedBindingsM__1339(ECUTest A_0)
			{
				if (A_0 != null)
				{
					return new ValueTuple<bool, bool>(A_0.Available, true);
				}
				return default(ValueTuple<bool, bool>);
			}

			// Token: 0x06003704 RID: 14084 RVA: 0x0028DD30 File Offset: 0x0028BF30
			[CompilerGenerated]
			private static object <LoadDataTemplate>typedBindingsM__1340(ECUTest A_0)
			{
				return A_0;
			}

			// Token: 0x06003705 RID: 14085 RVA: 0x0028DD40 File Offset: 0x0028BF40
			[CompilerGenerated]
			private static ValueTuple<bool, bool> <LoadDataTemplate>typedBindingsM__1341(ECUTest A_0)
			{
				if (A_0 != null)
				{
					return new ValueTuple<bool, bool>(A_0.Complete, true);
				}
				return default(ValueTuple<bool, bool>);
			}

			// Token: 0x06003706 RID: 14086 RVA: 0x0028DD70 File Offset: 0x0028BF70
			[CompilerGenerated]
			private static object <LoadDataTemplate>typedBindingsM__1342(ECUTest A_0)
			{
				return A_0;
			}

			// Token: 0x06003707 RID: 14087 RVA: 0x0028DD80 File Offset: 0x0028BF80
			[CompilerGenerated]
			private static ValueTuple<bool, bool> <LoadDataTemplate>typedBindingsM__1343(ECUTest A_0)
			{
				if (A_0 != null)
				{
					return new ValueTuple<bool, bool>(A_0.Complete, true);
				}
				return default(ValueTuple<bool, bool>);
			}

			// Token: 0x06003708 RID: 14088 RVA: 0x0028DDB0 File Offset: 0x0028BFB0
			[CompilerGenerated]
			private static object <LoadDataTemplate>typedBindingsM__1344(ECUTest A_0)
			{
				return A_0;
			}

			// Token: 0x04002136 RID: 8502
			internal object[] parentValues;

			// Token: 0x04002137 RID: 8503
			internal EcoTestPageV2 root;
		}

		// Token: 0x02000625 RID: 1573
		[CompilerGenerated]
		private sealed class <InitializeComponent>_anonXamlCDataTemplate_53
		{
			// Token: 0x06003709 RID: 14089 RVA: 0x0028DDC0 File Offset: 0x0028BFC0
			public <InitializeComponent>_anonXamlCDataTemplate_53()
			{
			}

			// Token: 0x0600370A RID: 14090 RVA: 0x0028DDD4 File Offset: 0x0028BFD4
			internal object LoadDataTemplate()
			{
				BindingExtension bindingExtension;
				VisualDiagnostics.RegisterSourceInfo(bindingExtension = new BindingExtension(), new Uri("Pages\\EcoTestPageV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 119, 29);
				StaticResourceExtension staticResourceExtension;
				VisualDiagnostics.RegisterSourceInfo(staticResourceExtension = new StaticResourceExtension(), new Uri("Pages\\EcoTestPageV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 120, 29);
				BindingExtension bindingExtension2;
				VisualDiagnostics.RegisterSourceInfo(bindingExtension2 = new BindingExtension(), new Uri("Pages\\EcoTestPageV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 120, 29);
				StaticResourceExtension staticResourceExtension2;
				VisualDiagnostics.RegisterSourceInfo(staticResourceExtension2 = new StaticResourceExtension(), new Uri("Pages\\EcoTestPageV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 121, 29);
				BindingExtension bindingExtension3;
				VisualDiagnostics.RegisterSourceInfo(bindingExtension3 = new BindingExtension(), new Uri("Pages\\EcoTestPageV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 121, 29);
				StaticResourceExtension staticResourceExtension3;
				VisualDiagnostics.RegisterSourceInfo(staticResourceExtension3 = new StaticResourceExtension(), new Uri("Pages\\EcoTestPageV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 122, 29);
				BindingExtension bindingExtension4;
				VisualDiagnostics.RegisterSourceInfo(bindingExtension4 = new BindingExtension(), new Uri("Pages\\EcoTestPageV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 122, 29);
				StaticResourceExtension staticResourceExtension4;
				VisualDiagnostics.RegisterSourceInfo(staticResourceExtension4 = new StaticResourceExtension(), new Uri("Pages\\EcoTestPageV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 123, 29);
				BindingExtension bindingExtension5;
				VisualDiagnostics.RegisterSourceInfo(bindingExtension5 = new BindingExtension(), new Uri("Pages\\EcoTestPageV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 123, 29);
				LabelCell labelCell;
				VisualDiagnostics.RegisterSourceInfo(labelCell = new LabelCell(), new Uri("Pages\\EcoTestPageV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 118, 26);
				NameScope nameScope = new NameScope();
				NameScope.SetNameScope(labelCell, nameScope);
				bindingExtension.Path = "Name";
				bindingExtension.TypedBinding = new TypedBinding<ECUTest, string>(delegate(ECUTest A_0)
				{
					if (A_0 != null)
					{
						return new ValueTuple<string, bool>(A_0.Name, true);
					}
					return default(ValueTuple<string, bool>);
				}, delegate(ECUTest A_0, string A_1)
				{
					if (A_0 != null)
					{
						A_0.Name = A_1;
						return;
					}
				}, new Tuple<Func<ECUTest, object>, string>[]
				{
					new Tuple<Func<ECUTest, object>, string>((ECUTest A_0) => A_0, "Name")
				});
				BindingBase bindingBase = bindingExtension.ProvideValue(null);
				labelCell.SetBinding(CellBase.TitleProperty, bindingBase);
				bindingExtension2.Mode = 2;
				staticResourceExtension.Key = "AvailableConverter";
				IMarkupExtension markupExtension = staticResourceExtension;
				XamlServiceProvider xamlServiceProvider = new XamlServiceProvider();
				Type typeFromHandle = typeof(IProvideValueTarget);
				int num;
				object[] array = new object[(num = this.parentValues.Length) + 2];
				Array.Copy(this.parentValues, 0, array, 2, num);
				object[] array2 = array;
				array2[0] = bindingExtension2;
				array2[1] = labelCell;
				object obj;
				xamlServiceProvider.Add(typeFromHandle, obj = new SimpleValueTargetProvider(array2, typeof(BindingExtension).GetRuntimeProperty("Converter"), nameScope));
				xamlServiceProvider.Add(typeof(IReferenceProvider), obj);
				Type typeFromHandle2 = typeof(IXamlTypeResolver);
				XmlNamespaceResolver xmlNamespaceResolver = new XmlNamespaceResolver();
				xmlNamespaceResolver.Add("", "http://xamarin.com/schemas/2014/forms");
				xmlNamespaceResolver.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
				xmlNamespaceResolver.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
				xmlNamespaceResolver.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
				xmlNamespaceResolver.Add("local", "clr-namespace:CarScannerXamarinForms");
				xmlNamespaceResolver.Add("pids", "clr-namespace:CarScannerXamarinForms.OBD2.PIDS");
				xmlNamespaceResolver.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
				xmlNamespaceResolver.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
				xamlServiceProvider.Add(typeFromHandle2, new XamlTypeResolver(xmlNamespaceResolver, typeof(EcoTestPageV2.<InitializeComponent>_anonXamlCDataTemplate_53).GetTypeInfo().Assembly));
				xamlServiceProvider.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(120, 29)));
				object obj2 = markupExtension.ProvideValue(xamlServiceProvider);
				bindingExtension2.Converter = obj2;
				bindingExtension2.Path = "Available";
				bindingExtension2.TypedBinding = new TypedBinding<ECUTest, bool>(delegate(ECUTest A_0)
				{
					if (A_0 != null)
					{
						return new ValueTuple<bool, bool>(A_0.Available, true);
					}
					return default(ValueTuple<bool, bool>);
				}, null, new Tuple<Func<ECUTest, object>, string>[]
				{
					new Tuple<Func<ECUTest, object>, string>((ECUTest A_0) => A_0, "Available")
				});
				BindingBase bindingBase2 = bindingExtension2.ProvideValue(null);
				labelCell.SetBinding(CellBase.HintTextProperty, bindingBase2);
				bindingExtension3.Mode = 2;
				staticResourceExtension2.Key = "ColorConverter";
				IMarkupExtension markupExtension2 = staticResourceExtension2;
				XamlServiceProvider xamlServiceProvider2 = new XamlServiceProvider();
				Type typeFromHandle3 = typeof(IProvideValueTarget);
				int num2;
				object[] array3 = new object[(num2 = this.parentValues.Length) + 2];
				Array.Copy(this.parentValues, 0, array3, 2, num2);
				object[] array4 = array3;
				array4[0] = bindingExtension3;
				array4[1] = labelCell;
				object obj3;
				xamlServiceProvider2.Add(typeFromHandle3, obj3 = new SimpleValueTargetProvider(array4, typeof(BindingExtension).GetRuntimeProperty("Converter"), nameScope));
				xamlServiceProvider2.Add(typeof(IReferenceProvider), obj3);
				Type typeFromHandle4 = typeof(IXamlTypeResolver);
				XmlNamespaceResolver xmlNamespaceResolver2 = new XmlNamespaceResolver();
				xmlNamespaceResolver2.Add("", "http://xamarin.com/schemas/2014/forms");
				xmlNamespaceResolver2.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
				xmlNamespaceResolver2.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
				xmlNamespaceResolver2.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
				xmlNamespaceResolver2.Add("local", "clr-namespace:CarScannerXamarinForms");
				xmlNamespaceResolver2.Add("pids", "clr-namespace:CarScannerXamarinForms.OBD2.PIDS");
				xmlNamespaceResolver2.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
				xmlNamespaceResolver2.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
				xamlServiceProvider2.Add(typeFromHandle4, new XamlTypeResolver(xmlNamespaceResolver2, typeof(EcoTestPageV2.<InitializeComponent>_anonXamlCDataTemplate_53).GetTypeInfo().Assembly));
				xamlServiceProvider2.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(121, 29)));
				object obj4 = markupExtension2.ProvideValue(xamlServiceProvider2);
				bindingExtension3.Converter = obj4;
				bindingExtension3.Path = "Available";
				bindingExtension3.TypedBinding = new TypedBinding<ECUTest, bool>(delegate(ECUTest A_0)
				{
					if (A_0 != null)
					{
						return new ValueTuple<bool, bool>(A_0.Available, true);
					}
					return default(ValueTuple<bool, bool>);
				}, null, new Tuple<Func<ECUTest, object>, string>[]
				{
					new Tuple<Func<ECUTest, object>, string>((ECUTest A_0) => A_0, "Available")
				});
				BindingBase bindingBase3 = bindingExtension3.ProvideValue(null);
				labelCell.SetBinding(CellBase.HintTextColorProperty, bindingBase3);
				bindingExtension4.Mode = 2;
				staticResourceExtension3.Key = "CompleteConverter";
				IMarkupExtension markupExtension3 = staticResourceExtension3;
				XamlServiceProvider xamlServiceProvider3 = new XamlServiceProvider();
				Type typeFromHandle5 = typeof(IProvideValueTarget);
				int num3;
				object[] array5 = new object[(num3 = this.parentValues.Length) + 2];
				Array.Copy(this.parentValues, 0, array5, 2, num3);
				object[] array6 = array5;
				array6[0] = bindingExtension4;
				array6[1] = labelCell;
				object obj5;
				xamlServiceProvider3.Add(typeFromHandle5, obj5 = new SimpleValueTargetProvider(array6, typeof(BindingExtension).GetRuntimeProperty("Converter"), nameScope));
				xamlServiceProvider3.Add(typeof(IReferenceProvider), obj5);
				Type typeFromHandle6 = typeof(IXamlTypeResolver);
				XmlNamespaceResolver xmlNamespaceResolver3 = new XmlNamespaceResolver();
				xmlNamespaceResolver3.Add("", "http://xamarin.com/schemas/2014/forms");
				xmlNamespaceResolver3.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
				xmlNamespaceResolver3.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
				xmlNamespaceResolver3.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
				xmlNamespaceResolver3.Add("local", "clr-namespace:CarScannerXamarinForms");
				xmlNamespaceResolver3.Add("pids", "clr-namespace:CarScannerXamarinForms.OBD2.PIDS");
				xmlNamespaceResolver3.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
				xmlNamespaceResolver3.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
				xamlServiceProvider3.Add(typeFromHandle6, new XamlTypeResolver(xmlNamespaceResolver3, typeof(EcoTestPageV2.<InitializeComponent>_anonXamlCDataTemplate_53).GetTypeInfo().Assembly));
				xamlServiceProvider3.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(122, 29)));
				object obj6 = markupExtension3.ProvideValue(xamlServiceProvider3);
				bindingExtension4.Converter = obj6;
				bindingExtension4.Path = "Complete";
				bindingExtension4.TypedBinding = new TypedBinding<ECUTest, bool>(delegate(ECUTest A_0)
				{
					if (A_0 != null)
					{
						return new ValueTuple<bool, bool>(A_0.Complete, true);
					}
					return default(ValueTuple<bool, bool>);
				}, null, new Tuple<Func<ECUTest, object>, string>[]
				{
					new Tuple<Func<ECUTest, object>, string>((ECUTest A_0) => A_0, "Complete")
				});
				BindingBase bindingBase4 = bindingExtension4.ProvideValue(null);
				labelCell.SetBinding(LabelCell.ValueTextProperty, bindingBase4);
				bindingExtension5.Mode = 2;
				staticResourceExtension4.Key = "ColorConverter";
				IMarkupExtension markupExtension4 = staticResourceExtension4;
				XamlServiceProvider xamlServiceProvider4 = new XamlServiceProvider();
				Type typeFromHandle7 = typeof(IProvideValueTarget);
				int num4;
				object[] array7 = new object[(num4 = this.parentValues.Length) + 2];
				Array.Copy(this.parentValues, 0, array7, 2, num4);
				object[] array8 = array7;
				array8[0] = bindingExtension5;
				array8[1] = labelCell;
				object obj7;
				xamlServiceProvider4.Add(typeFromHandle7, obj7 = new SimpleValueTargetProvider(array8, typeof(BindingExtension).GetRuntimeProperty("Converter"), nameScope));
				xamlServiceProvider4.Add(typeof(IReferenceProvider), obj7);
				Type typeFromHandle8 = typeof(IXamlTypeResolver);
				XmlNamespaceResolver xmlNamespaceResolver4 = new XmlNamespaceResolver();
				xmlNamespaceResolver4.Add("", "http://xamarin.com/schemas/2014/forms");
				xmlNamespaceResolver4.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
				xmlNamespaceResolver4.Add("converters", "clr-namespace:CarScannerXamarinForms.Common.XAMLConverters");
				xmlNamespaceResolver4.Add("ios", "clr-namespace:Xamarin.Forms.PlatformConfiguration.iOSSpecific;assembly=Xamarin.Forms.Core");
				xmlNamespaceResolver4.Add("local", "clr-namespace:CarScannerXamarinForms");
				xmlNamespaceResolver4.Add("pids", "clr-namespace:CarScannerXamarinForms.OBD2.PIDS");
				xmlNamespaceResolver4.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
				xmlNamespaceResolver4.Add("uc", "clr-namespace:CarScannerXamarinForms.UserControls");
				xamlServiceProvider4.Add(typeFromHandle8, new XamlTypeResolver(xmlNamespaceResolver4, typeof(EcoTestPageV2.<InitializeComponent>_anonXamlCDataTemplate_53).GetTypeInfo().Assembly));
				xamlServiceProvider4.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(123, 29)));
				object obj8 = markupExtension4.ProvideValue(xamlServiceProvider4);
				bindingExtension5.Converter = obj8;
				bindingExtension5.Path = "Complete";
				bindingExtension5.TypedBinding = new TypedBinding<ECUTest, bool>(delegate(ECUTest A_0)
				{
					if (A_0 != null)
					{
						return new ValueTuple<bool, bool>(A_0.Complete, true);
					}
					return default(ValueTuple<bool, bool>);
				}, null, new Tuple<Func<ECUTest, object>, string>[]
				{
					new Tuple<Func<ECUTest, object>, string>((ECUTest A_0) => A_0, "Complete")
				});
				BindingBase bindingBase5 = bindingExtension5.ProvideValue(null);
				labelCell.SetBinding(LabelCell.ValueTextColorProperty, bindingBase5);
				return labelCell;
			}

			// Token: 0x0600370B RID: 14091 RVA: 0x0028E708 File Offset: 0x0028C908
			[CompilerGenerated]
			private static ValueTuple<string, bool> <LoadDataTemplate>typedBindingsM__1345(ECUTest A_0)
			{
				if (A_0 != null)
				{
					return new ValueTuple<string, bool>(A_0.Name, true);
				}
				return default(ValueTuple<string, bool>);
			}

			// Token: 0x0600370C RID: 14092 RVA: 0x0028E738 File Offset: 0x0028C938
			[CompilerGenerated]
			private static void <LoadDataTemplate>typedBindingsM__1346(ECUTest A_0, string A_1)
			{
				if (A_0 != null)
				{
					A_0.Name = A_1;
					return;
				}
			}

			// Token: 0x0600370D RID: 14093 RVA: 0x0028E754 File Offset: 0x0028C954
			[CompilerGenerated]
			private static object <LoadDataTemplate>typedBindingsM__1347(ECUTest A_0)
			{
				return A_0;
			}

			// Token: 0x0600370E RID: 14094 RVA: 0x0028E764 File Offset: 0x0028C964
			[CompilerGenerated]
			private static ValueTuple<bool, bool> <LoadDataTemplate>typedBindingsM__1348(ECUTest A_0)
			{
				if (A_0 != null)
				{
					return new ValueTuple<bool, bool>(A_0.Available, true);
				}
				return default(ValueTuple<bool, bool>);
			}

			// Token: 0x0600370F RID: 14095 RVA: 0x0028E794 File Offset: 0x0028C994
			[CompilerGenerated]
			private static object <LoadDataTemplate>typedBindingsM__1349(ECUTest A_0)
			{
				return A_0;
			}

			// Token: 0x06003710 RID: 14096 RVA: 0x0028E7A4 File Offset: 0x0028C9A4
			[CompilerGenerated]
			private static ValueTuple<bool, bool> <LoadDataTemplate>typedBindingsM__1350(ECUTest A_0)
			{
				if (A_0 != null)
				{
					return new ValueTuple<bool, bool>(A_0.Available, true);
				}
				return default(ValueTuple<bool, bool>);
			}

			// Token: 0x06003711 RID: 14097 RVA: 0x0028E7D4 File Offset: 0x0028C9D4
			[CompilerGenerated]
			private static object <LoadDataTemplate>typedBindingsM__1351(ECUTest A_0)
			{
				return A_0;
			}

			// Token: 0x06003712 RID: 14098 RVA: 0x0028E7E4 File Offset: 0x0028C9E4
			[CompilerGenerated]
			private static ValueTuple<bool, bool> <LoadDataTemplate>typedBindingsM__1352(ECUTest A_0)
			{
				if (A_0 != null)
				{
					return new ValueTuple<bool, bool>(A_0.Complete, true);
				}
				return default(ValueTuple<bool, bool>);
			}

			// Token: 0x06003713 RID: 14099 RVA: 0x0028E814 File Offset: 0x0028CA14
			[CompilerGenerated]
			private static object <LoadDataTemplate>typedBindingsM__1353(ECUTest A_0)
			{
				return A_0;
			}

			// Token: 0x06003714 RID: 14100 RVA: 0x0028E824 File Offset: 0x0028CA24
			[CompilerGenerated]
			private static ValueTuple<bool, bool> <LoadDataTemplate>typedBindingsM__1354(ECUTest A_0)
			{
				if (A_0 != null)
				{
					return new ValueTuple<bool, bool>(A_0.Complete, true);
				}
				return default(ValueTuple<bool, bool>);
			}

			// Token: 0x06003715 RID: 14101 RVA: 0x0028E854 File Offset: 0x0028CA54
			[CompilerGenerated]
			private static object <LoadDataTemplate>typedBindingsM__1355(ECUTest A_0)
			{
				return A_0;
			}

			// Token: 0x04002138 RID: 8504
			internal object[] parentValues;

			// Token: 0x04002139 RID: 8505
			internal EcoTestPageV2 root;
		}
	}
}
