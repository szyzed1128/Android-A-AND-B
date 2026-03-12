using System;
using System.CodeDom.Compiler;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using CarScannerXamarinForms.PlatformAdapters;
using CarScannerXamarinForms.ProfilesV2;
using Xamarin.Forms;
using Xamarin.Forms.Internals;
using Xamarin.Forms.Xaml;
using Xamarin.Forms.Xaml.Diagnostics;
using Xamarin.Forms.Xaml.Internals;

namespace CarScannerXamarinForms.Settings
{
	// Token: 0x0200027D RID: 637
	[XamlCompilation(2)]
	[XamlFilePath("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Settings\\ProfileSelectorV2.xaml")]
	public class ProfileSelectorV2 : ContentView, IProfileSelector, INotifyPropertyChanged
	{
		// Token: 0x06001EBC RID: 7868 RVA: 0x0014ECF0 File Offset: 0x0014CEF0
		public ProfileSelectorV2()
		{
			try
			{
				this.InitializeComponent();
				this.layoutRoot.BindingContext = this.Model;
				this.btnBack.IsVisible = this.BackButtonVisible;
				this.lbTitle.IsVisible = this.TitleVisible;
				this.TitleText = Translate.GetString("ios_ChooseCarBrand");
				this.lbTitle.Text = this.TitleText;
				if (!string.IsNullOrEmpty(SharedSettings.Current.SelectedBrand))
				{
					this.lvBrands.ItemSelected -= this.LvBrands_ItemSelected;
					this.lvBrands.SelectedItem = SharedSettings.Current.SelectedBrand;
					this.lvBrands.ItemSelected += this.LvBrands_ItemSelected;
					this.lvBrands.ScrollTo(this.lvBrands.SelectedItem, 2, false);
				}
			}
			catch (Exception)
			{
			}
		}

		// Token: 0x06001EBD RID: 7869 RVA: 0x00022295 File Offset: 0x00020495
		private void EntrySearch_Completed(object sender, EventArgs e)
		{
			PlatformHelper.CommonService.HideKeyboard();
		}

		// Token: 0x170010DB RID: 4315
		// (get) Token: 0x06001EBE RID: 7870 RVA: 0x0014EDF4 File Offset: 0x0014CFF4
		private ProfileSelectorV2.Step CurrentStep
		{
			get
			{
				if (this.gridBrands.IsVisible)
				{
					return ProfileSelectorV2.Step.BrandSelection;
				}
				return ProfileSelectorV2.Step.ProfileSelection;
			}
		}

		// Token: 0x170010DC RID: 4316
		// (get) Token: 0x06001EBF RID: 7871 RVA: 0x0014EE06 File Offset: 0x0014D006
		// (set) Token: 0x06001EC0 RID: 7872 RVA: 0x0014EE0E File Offset: 0x0014D00E
		public string SelectedBrand
		{
			[CompilerGenerated]
			get
			{
				return this.<SelectedBrand>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<SelectedBrand>k__BackingField = value;
			}
		} = "";

		// Token: 0x14000017 RID: 23
		// (add) Token: 0x06001EC1 RID: 7873 RVA: 0x0014EE18 File Offset: 0x0014D018
		// (remove) Token: 0x06001EC2 RID: 7874 RVA: 0x0014EE50 File Offset: 0x0014D050
		public event EventHandler<bool> WindowCloseRequested
		{
			[CompilerGenerated]
			add
			{
				EventHandler<bool> eventHandler = this.WindowCloseRequested;
				EventHandler<bool> eventHandler2;
				do
				{
					eventHandler2 = eventHandler;
					EventHandler<bool> eventHandler3 = (EventHandler<bool>)Delegate.Combine(eventHandler2, value);
					eventHandler = Interlocked.CompareExchange<EventHandler<bool>>(ref this.WindowCloseRequested, eventHandler3, eventHandler2);
				}
				while (eventHandler != eventHandler2);
			}
			[CompilerGenerated]
			remove
			{
				EventHandler<bool> eventHandler = this.WindowCloseRequested;
				EventHandler<bool> eventHandler2;
				do
				{
					eventHandler2 = eventHandler;
					EventHandler<bool> eventHandler3 = (EventHandler<bool>)Delegate.Remove(eventHandler2, value);
					eventHandler = Interlocked.CompareExchange<EventHandler<bool>>(ref this.WindowCloseRequested, eventHandler3, eventHandler2);
				}
				while (eventHandler != eventHandler2);
			}
		}

		// Token: 0x06001EC3 RID: 7875 RVA: 0x0014EE85 File Offset: 0x0014D085
		private static void TitleVisiblePropertyChanged(BindableObject bindable, object oldValue, object newValue)
		{
			((ProfileSelectorV2)bindable).lbTitle.IsVisible = (bool)newValue;
		}

		// Token: 0x06001EC4 RID: 7876 RVA: 0x0014EE9D File Offset: 0x0014D09D
		private static void BackButtonVisiblePropertyChanged(BindableObject bindable, object oldValue, object newValue)
		{
			((ProfileSelectorV2)bindable).btnBack.IsVisible = (bool)newValue;
		}

		// Token: 0x170010DD RID: 4317
		// (get) Token: 0x06001EC5 RID: 7877 RVA: 0x0014EEB5 File Offset: 0x0014D0B5
		// (set) Token: 0x06001EC6 RID: 7878 RVA: 0x0014EEC7 File Offset: 0x0014D0C7
		public bool BackButtonVisible
		{
			get
			{
				return (bool)base.GetValue(ProfileSelectorV2.BackButtonVisibleProperty);
			}
			set
			{
				base.SetValue(ProfileSelectorV2.BackButtonVisibleProperty, value);
			}
		}

		// Token: 0x06001EC7 RID: 7879 RVA: 0x000027D4 File Offset: 0x000009D4
		private static void CreateBackItemPropertyChanged(BindableObject bindable, object oldValue, object newValue)
		{
		}

		// Token: 0x170010DE RID: 4318
		// (get) Token: 0x06001EC8 RID: 7880 RVA: 0x0014EEDA File Offset: 0x0014D0DA
		// (set) Token: 0x06001EC9 RID: 7881 RVA: 0x0014EEEC File Offset: 0x0014D0EC
		public bool CreateBackItem
		{
			get
			{
				return (bool)base.GetValue(ProfileSelectorV2.CreateBackItemProperty);
			}
			set
			{
				base.SetValue(ProfileSelectorV2.CreateBackItemProperty, value);
			}
		}

		// Token: 0x06001ECA RID: 7882 RVA: 0x0014EEFF File Offset: 0x0014D0FF
		private static void TitleTextPropertyChanged(BindableObject bindable, object oldValue, object newValue)
		{
			((ProfileSelectorV2)bindable).lbTitle.Text = (string)newValue;
		}

		// Token: 0x170010DF RID: 4319
		// (get) Token: 0x06001ECB RID: 7883 RVA: 0x0014EF17 File Offset: 0x0014D117
		// (set) Token: 0x06001ECC RID: 7884 RVA: 0x0014EF29 File Offset: 0x0014D129
		public bool TitleVisible
		{
			get
			{
				return (bool)base.GetValue(ProfileSelectorV2.TitleVisibleProperty);
			}
			set
			{
				base.SetValue(ProfileSelectorV2.TitleVisibleProperty, value);
			}
		}

		// Token: 0x170010E0 RID: 4320
		// (get) Token: 0x06001ECD RID: 7885 RVA: 0x0014EF3C File Offset: 0x0014D13C
		// (set) Token: 0x06001ECE RID: 7886 RVA: 0x0014EF4E File Offset: 0x0014D14E
		public string TitleText
		{
			get
			{
				return (string)base.GetValue(ProfileSelectorV2.TitleTextProperty);
			}
			set
			{
				base.SetValue(ProfileSelectorV2.TitleTextProperty, value);
			}
		}

		// Token: 0x170010E1 RID: 4321
		// (get) Token: 0x06001ECF RID: 7887 RVA: 0x0014EF5C File Offset: 0x0014D15C
		// (set) Token: 0x06001ED0 RID: 7888 RVA: 0x0014EF64 File Offset: 0x0014D164
		public ProfileV2Model Model
		{
			[CompilerGenerated]
			get
			{
				return this.<Model>k__BackingField;
			}
			[CompilerGenerated]
			private set
			{
				this.<Model>k__BackingField = value;
			}
		} = new ProfileV2Model();

		// Token: 0x06001ED1 RID: 7889 RVA: 0x0014EF70 File Offset: 0x0014D170
		private async Task SelectBrand(string brand)
		{
			this.gridBrands.IsEnabled = false;
			this.btnBack.IsEnabled = false;
			this.activityFrame.IsVisible = true;
			if (!this.Model.ProfilesLoaded)
			{
				await this.Model.LoadProfilesAsync();
			}
			BrandCollection profilesForBrandWithFilter = this.Model.GetProfilesForBrandWithFilter(brand, "");
			if (profilesForBrandWithFilter.Count == 1 && profilesForBrandWithFilter[0].Name.Contains("OBD"))
			{
				this.SelectedBrand = brand;
				this.OnPropertyChanged("SelectedBrand");
				await this.ApplyProfile(profilesForBrandWithFilter[0]);
				this.gridBrands.IsEnabled = true;
				this.activityFrame.IsVisible = false;
				this.btnBack.IsEnabled = true;
			}
			else
			{
				if (this.CreateBackItem)
				{
					profilesForBrandWithFilter.Insert(0, new OBDReaderProfileV2
					{
						Name = "[ " + Translate.GetString("ios_Back2") + " ]"
					});
				}
				this.lbSelectedBrand.BindingContext = profilesForBrandWithFilter;
				this.lvProfiles.BindingContext = profilesForBrandWithFilter;
				this.lvProfiles.ItemsSource = profilesForBrandWithFilter;
				if (!string.IsNullOrEmpty(SharedSettings.Current.ProfileUpdateAlias))
				{
					this.lvProfiles.ItemSelected -= this.LvProfiles_ItemSelected;
					this.lvProfiles.SelectedItem = profilesForBrandWithFilter.FirstOrDefault((OBDReaderProfileV2 x) => x.UpdateAliases.Any((string ua) => ua == SharedSettings.Current.ProfileUpdateAlias));
					if (this.lvProfiles.SelectedItem != null)
					{
						this.lvProfiles.ScrollTo(this.lvProfiles.SelectedItem, 2, false);
					}
					this.lvProfiles.ItemSelected += this.LvProfiles_ItemSelected;
				}
				this.TitleText = Translate.GetString("ios_ConnectionProfile");
				this.activityFrame.IsVisible = false;
				this.gridBrands.IsEnabled = true;
				this.gridBrands.IsVisible = false;
				this.gridProfiles.IsVisible = true;
				this.btnBack.IsEnabled = true;
			}
		}

		// Token: 0x06001ED2 RID: 7890 RVA: 0x0014EFBC File Offset: 0x0014D1BC
		public void GoBack()
		{
			if (this.CurrentStep != ProfileSelectorV2.Step.BrandSelection)
			{
				this.gridBrands.IsVisible = true;
				this.gridProfiles.IsVisible = false;
				this.searchBarProfiles.Text = "";
				this.TitleText = Translate.GetString("ios_ChooseCarBrand");
				return;
			}
			EventHandler<bool> windowCloseRequested = this.WindowCloseRequested;
			if (windowCloseRequested == null)
			{
				return;
			}
			windowCloseRequested(this, false);
		}

		// Token: 0x06001ED3 RID: 7891 RVA: 0x0014F01C File Offset: 0x0014D21C
		private async void LvProfiles_ItemSelected(object sender, SelectedItemChangedEventArgs e)
		{
			PlatformHelper.CommonService.HideKeyboard();
			if (this.lvProfiles.SelectedItem != null)
			{
				OBDReaderProfileV2 obdreaderProfileV = this.lvProfiles.SelectedItem as OBDReaderProfileV2;
				this.lvProfiles.SelectedItem = null;
				int num = (this.lvProfiles.ItemsSource as BrandCollection).IndexOf(obdreaderProfileV);
				if (this.CreateBackItem && num == 0)
				{
					this.GoBack();
				}
				else
				{
					await this.ApplyProfile(obdreaderProfileV);
				}
			}
		}

		// Token: 0x06001ED4 RID: 7892 RVA: 0x0014F054 File Offset: 0x0014D254
		private async Task ApplyProfile(OBDReaderProfileV2 profile)
		{
			TaskAwaiter<bool> taskAwaiter = App.GetCurrentPage().DisplayAlert(Translate.GetString("ios_ApplyProfileQuestion"), this.SelectedBrand + " " + profile.Name, "OK", Translate.GetString("btnCancel.Content")).GetAwaiter();
			if (!taskAwaiter.IsCompleted)
			{
				await taskAwaiter;
				TaskAwaiter<bool> taskAwaiter2;
				taskAwaiter = taskAwaiter2;
				taskAwaiter2 = default(TaskAwaiter<bool>);
			}
			if (taskAwaiter.GetResult())
			{
				this.activityFrame.IsVisible = true;
				this.gridProfiles.IsEnabled = false;
				await Task.Run(delegate
				{
					profile.Apply(this.SelectedBrand);
				});
				SharedSettings.Current.ShouldCheckProfilePIDs = profile.ShouldCheckProfilePIDs;
				this.activityFrame.IsVisible = false;
				this.gridProfiles.IsEnabled = true;
				EventHandler<bool> windowCloseRequested = this.WindowCloseRequested;
				if (windowCloseRequested != null)
				{
					windowCloseRequested(this, true);
				}
			}
		}

		// Token: 0x06001ED5 RID: 7893 RVA: 0x0014F0A0 File Offset: 0x0014D2A0
		private async void LvBrands_ItemSelected(object sender, SelectedItemChangedEventArgs e)
		{
			PlatformHelper.CommonService.HideKeyboard();
			if (this.lvBrands.SelectedItem != null)
			{
				string text = (string)this.lvBrands.SelectedItem;
				this.SelectedBrand = text;
				this.OnPropertyChanged("SelectedBrand");
				this.lvBrands.SelectedItem = null;
				await this.SelectBrand(text);
			}
		}

		// Token: 0x06001ED6 RID: 7894 RVA: 0x0014F0D7 File Offset: 0x0014D2D7
		private void BtnBack_Clicked(object sender, EventArgs e)
		{
			this.GoBack();
		}

		// Token: 0x06001ED7 RID: 7895 RVA: 0x0014F0E0 File Offset: 0x0014D2E0
		private async void searchBarProfiles_TextChanged(object sender, TextChangedEventArgs e)
		{
			try
			{
				Entry searchBar = sender as Entry;
				string filtertext = searchBar.Text;
				await Task.Delay(500);
				if (filtertext == searchBar.Text)
				{
					try
					{
						if (this.CurrentStep == ProfileSelectorV2.Step.ProfileSelection)
						{
							BrandCollection profilesForBrandWithFilter = this.Model.GetProfilesForBrandWithFilter(this.SelectedBrand, filtertext);
							if (this.CreateBackItem)
							{
								profilesForBrandWithFilter.Insert(0, new OBDReaderProfileV2
								{
									Name = "[ " + Translate.GetString("ios_Back2") + " ]"
								});
							}
							this.gridProfiles.BindingContext = profilesForBrandWithFilter;
							this.lvProfiles.ItemsSource = profilesForBrandWithFilter;
						}
					}
					catch (Exception)
					{
					}
				}
				searchBar = null;
				filtertext = null;
			}
			catch
			{
			}
		}

		// Token: 0x06001ED8 RID: 7896 RVA: 0x0014F120 File Offset: 0x0014D320
		private async void searchBarBrands_TextChanged(object sender, TextChangedEventArgs e)
		{
			try
			{
				Entry searchBar = sender as Entry;
				string filtertext = searchBar.Text;
				await Task.Delay(500);
				if (filtertext == searchBar.Text)
				{
					try
					{
						if (this.CurrentStep == ProfileSelectorV2.Step.BrandSelection)
						{
							ObservableCollection<string> brandsWithFilter = this.Model.GetBrandsWithFilter(filtertext);
							this.gridBrands.BindingContext = brandsWithFilter;
							this.lvBrands.ItemsSource = brandsWithFilter;
						}
					}
					catch (Exception)
					{
					}
				}
				searchBar = null;
				filtertext = null;
			}
			catch
			{
			}
		}

		// Token: 0x06001ED9 RID: 7897 RVA: 0x0014F160 File Offset: 0x0014D360
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private void InitializeComponent()
		{
			if (ResourceLoader.IsEnabled && ResourceLoader.CanProvideContentFor(new ResourceLoader.ResourceLoadingQuery
			{
				AssemblyName = typeof(ProfileSelectorV2).GetTypeInfo().Assembly.GetName(),
				ResourcePath = "Settings/ProfileSelectorV2.xaml",
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
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension = new DynamicResourceExtension(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Settings\\ProfileSelectorV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 7, 5);
			RowDefinition rowDefinition;
			VisualDiagnostics.RegisterSourceInfo(rowDefinition = new RowDefinition(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Settings\\ProfileSelectorV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 14, 18);
			RowDefinition rowDefinition2;
			VisualDiagnostics.RegisterSourceInfo(rowDefinition2 = new RowDefinition(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Settings\\ProfileSelectorV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 15, 18);
			RowDefinition rowDefinition3;
			VisualDiagnostics.RegisterSourceInfo(rowDefinition3 = new RowDefinition(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Settings\\ProfileSelectorV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 16, 18);
			RowDefinition rowDefinition4;
			VisualDiagnostics.RegisterSourceInfo(rowDefinition4 = new RowDefinition(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Settings\\ProfileSelectorV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 17, 18);
			DynamicResourceExtension dynamicResourceExtension2;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension2 = new DynamicResourceExtension(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Settings\\ProfileSelectorV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 23, 17);
			Label label;
			VisualDiagnostics.RegisterSourceInfo(label = new Label(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Settings\\ProfileSelectorV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 19, 14);
			RowDefinition rowDefinition5;
			VisualDiagnostics.RegisterSourceInfo(rowDefinition5 = new RowDefinition(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Settings\\ProfileSelectorV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 34, 22);
			RowDefinition rowDefinition6;
			VisualDiagnostics.RegisterSourceInfo(rowDefinition6 = new RowDefinition(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Settings\\ProfileSelectorV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 35, 22);
			Entry entry;
			VisualDiagnostics.RegisterSourceInfo(entry = new Entry(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Settings\\ProfileSelectorV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 37, 18);
			BindingExtension bindingExtension;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension = new BindingExtension(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Settings\\ProfileSelectorV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 51, 21);
			DataTemplate dataTemplate;
			VisualDiagnostics.RegisterSourceInfo(dataTemplate = new DataTemplate(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Settings\\ProfileSelectorV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 53, 26);
			ListView listView;
			VisualDiagnostics.RegisterSourceInfo(listView = new ListView(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Settings\\ProfileSelectorV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 44, 18);
			Grid grid;
			VisualDiagnostics.RegisterSourceInfo(grid = new Grid(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Settings\\ProfileSelectorV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 29, 14);
			RowDefinition rowDefinition7;
			VisualDiagnostics.RegisterSourceInfo(rowDefinition7 = new RowDefinition(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Settings\\ProfileSelectorV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 65, 22);
			RowDefinition rowDefinition8;
			VisualDiagnostics.RegisterSourceInfo(rowDefinition8 = new RowDefinition(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Settings\\ProfileSelectorV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 66, 22);
			RowDefinition rowDefinition9;
			VisualDiagnostics.RegisterSourceInfo(rowDefinition9 = new RowDefinition(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Settings\\ProfileSelectorV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 67, 22);
			RowDefinition rowDefinition10;
			VisualDiagnostics.RegisterSourceInfo(rowDefinition10 = new RowDefinition(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Settings\\ProfileSelectorV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 68, 22);
			DynamicResourceExtension dynamicResourceExtension3;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension3 = new DynamicResourceExtension(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Settings\\ProfileSelectorV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 74, 21);
			BindingExtension bindingExtension2;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension2 = new BindingExtension(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Settings\\ProfileSelectorV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 77, 21);
			Label label2;
			VisualDiagnostics.RegisterSourceInfo(label2 = new Label(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Settings\\ProfileSelectorV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 70, 18);
			Entry entry2;
			VisualDiagnostics.RegisterSourceInfo(entry2 = new Entry(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Settings\\ProfileSelectorV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 78, 18);
			DataTemplate dataTemplate2;
			VisualDiagnostics.RegisterSourceInfo(dataTemplate2 = new DataTemplate(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Settings\\ProfileSelectorV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 92, 26);
			ListView listView2;
			VisualDiagnostics.RegisterSourceInfo(listView2 = new ListView(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Settings\\ProfileSelectorV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 85, 18);
			DynamicResourceExtension dynamicResourceExtension4;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension4 = new DynamicResourceExtension(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Settings\\ProfileSelectorV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 125, 21);
			Translate translate;
			VisualDiagnostics.RegisterSourceInfo(translate = new Translate(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Settings\\ProfileSelectorV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 128, 21);
			Label label3;
			VisualDiagnostics.RegisterSourceInfo(label3 = new Label(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Settings\\ProfileSelectorV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 123, 18);
			Grid grid2;
			VisualDiagnostics.RegisterSourceInfo(grid2 = new Grid(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Settings\\ProfileSelectorV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 60, 14);
			ActivityFrame activityFrame;
			VisualDiagnostics.RegisterSourceInfo(activityFrame = new ActivityFrame(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Settings\\ProfileSelectorV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 132, 14);
			Translate translate2;
			VisualDiagnostics.RegisterSourceInfo(translate2 = new Translate(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Settings\\ProfileSelectorV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 143, 17);
			Button button;
			VisualDiagnostics.RegisterSourceInfo(button = new Button(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Settings\\ProfileSelectorV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 138, 14);
			Grid grid3;
			VisualDiagnostics.RegisterSourceInfo(grid3 = new Grid(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Settings\\ProfileSelectorV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 9, 10);
			VisualDiagnostics.RegisterSourceInfo(this, new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Settings\\ProfileSelectorV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 2, 2);
			NameScope nameScope = NameScope.GetNameScope(this) ?? new NameScope();
			NameScope.SetNameScope(this, nameScope);
			nameScope.RegisterName("layoutRoot", grid3);
			if (grid3.StyleId == null)
			{
				grid3.StyleId = "layoutRoot";
			}
			nameScope.RegisterName("lbTitle", label);
			if (label.StyleId == null)
			{
				label.StyleId = "lbTitle";
			}
			nameScope.RegisterName("gridBrands", grid);
			if (grid.StyleId == null)
			{
				grid.StyleId = "gridBrands";
			}
			nameScope.RegisterName("searchBarBrands", entry);
			if (entry.StyleId == null)
			{
				entry.StyleId = "searchBarBrands";
			}
			nameScope.RegisterName("lvBrands", listView);
			if (listView.StyleId == null)
			{
				listView.StyleId = "lvBrands";
			}
			nameScope.RegisterName("gridProfiles", grid2);
			if (grid2.StyleId == null)
			{
				grid2.StyleId = "gridProfiles";
			}
			nameScope.RegisterName("lbSelectedBrand", label2);
			if (label2.StyleId == null)
			{
				label2.StyleId = "lbSelectedBrand";
			}
			nameScope.RegisterName("searchBarProfiles", entry2);
			if (entry2.StyleId == null)
			{
				entry2.StyleId = "searchBarProfiles";
			}
			nameScope.RegisterName("lvProfiles", listView2);
			if (listView2.StyleId == null)
			{
				listView2.StyleId = "lvProfiles";
			}
			nameScope.RegisterName("activityFrame", activityFrame);
			if (activityFrame.StyleId == null)
			{
				activityFrame.StyleId = "activityFrame";
			}
			nameScope.RegisterName("btnBack", button);
			if (button.StyleId == null)
			{
				button.StyleId = "btnBack";
			}
			this.layoutRoot = grid3;
			this.lbTitle = label;
			this.gridBrands = grid;
			this.searchBarBrands = entry;
			this.lvBrands = listView;
			this.gridProfiles = grid2;
			this.lbSelectedBrand = label2;
			this.searchBarProfiles = entry2;
			this.lvProfiles = listView2;
			this.activityFrame = activityFrame;
			this.btnBack = button;
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
			xmlNamespaceResolver.Add("local", "clr-namespace:CarScannerXamarinForms");
			xamlServiceProvider.Add(typeFromHandle2, new XamlTypeResolver(xmlNamespaceResolver, typeof(ProfileSelectorV2).GetTypeInfo().Assembly));
			xamlServiceProvider.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(7, 5)));
			DynamicResource dynamicResource = markupExtension.ProvideValue(xamlServiceProvider);
			this.SetDynamicResource(VisualElement.BackgroundColorProperty, dynamicResource.Key);
			grid3.SetValue(Grid.ColumnSpacingProperty, 0.0);
			grid3.SetValue(Grid.RowSpacingProperty, 0.0);
			rowDefinition.SetValue(RowDefinition.HeightProperty, new GridLengthTypeConverter().ConvertFromInvariantString("auto"));
			grid3.GetValue(Grid.RowDefinitionsProperty).Add(rowDefinition);
			rowDefinition2.SetValue(RowDefinition.HeightProperty, new GridLengthTypeConverter().ConvertFromInvariantString("auto"));
			grid3.GetValue(Grid.RowDefinitionsProperty).Add(rowDefinition2);
			rowDefinition3.SetValue(RowDefinition.HeightProperty, new GridLengthTypeConverter().ConvertFromInvariantString("*"));
			grid3.GetValue(Grid.RowDefinitionsProperty).Add(rowDefinition3);
			rowDefinition4.SetValue(RowDefinition.HeightProperty, new GridLengthTypeConverter().ConvertFromInvariantString("auto"));
			grid3.GetValue(Grid.RowDefinitionsProperty).Add(rowDefinition4);
			label.SetValue(Grid.RowProperty, 0);
			label.SetValue(Label.FontAttributesProperty, new FontAttributesConverter().ConvertFromInvariantString("Bold"));
			dynamicResourceExtension2.Key = "BaseFontSize++";
			IMarkupExtension<DynamicResource> markupExtension2 = dynamicResourceExtension2;
			XamlServiceProvider xamlServiceProvider2 = new XamlServiceProvider();
			Type typeFromHandle3 = typeof(IProvideValueTarget);
			object[] array2 = new object[0 + 3];
			array2[0] = label;
			array2[1] = grid3;
			array2[2] = this;
			object obj2;
			xamlServiceProvider2.Add(typeFromHandle3, obj2 = new SimpleValueTargetProvider(array2, Label.FontSizeProperty, nameScope));
			xamlServiceProvider2.Add(typeof(IReferenceProvider), obj2);
			Type typeFromHandle4 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver2 = new XmlNamespaceResolver();
			xmlNamespaceResolver2.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver2.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver2.Add("local", "clr-namespace:CarScannerXamarinForms");
			xamlServiceProvider2.Add(typeFromHandle4, new XamlTypeResolver(xmlNamespaceResolver2, typeof(ProfileSelectorV2).GetTypeInfo().Assembly));
			xamlServiceProvider2.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(23, 17)));
			DynamicResource dynamicResource2 = markupExtension2.ProvideValue(xamlServiceProvider2);
			label.SetDynamicResource(Label.FontSizeProperty, dynamicResource2.Key);
			label.SetValue(View.HorizontalOptionsProperty, LayoutOptions.Center);
			label.SetValue(Label.HorizontalTextAlignmentProperty, new TextAlignmentConverter().ConvertFromInvariantString("Center"));
			label.SetValue(VisualElement.IsVisibleProperty, new VisualElement.VisibilityConverter().ConvertFromInvariantString("False"));
			label.SetValue(Label.TextProperty, "TEST TITLE");
			grid3.Children.Add(label);
			grid.SetValue(Grid.RowProperty, 2);
			grid.SetValue(VisualElement.IsVisibleProperty, new VisualElement.VisibilityConverter().ConvertFromInvariantString("True"));
			rowDefinition5.SetValue(RowDefinition.HeightProperty, new GridLengthTypeConverter().ConvertFromInvariantString("auto"));
			grid.GetValue(Grid.RowDefinitionsProperty).Add(rowDefinition5);
			rowDefinition6.SetValue(RowDefinition.HeightProperty, new GridLengthTypeConverter().ConvertFromInvariantString("*"));
			grid.GetValue(Grid.RowDefinitionsProperty).Add(rowDefinition6);
			entry.SetValue(Grid.RowProperty, 0);
			entry.Completed += this.EntrySearch_Completed;
			entry.SetValue(InputView.IsSpellCheckEnabledProperty, false);
			entry.SetValue(Entry.PlaceholderProperty, "\ud83d\udd0e");
			entry.TextChanged += this.searchBarBrands_TextChanged;
			grid.Children.Add(entry);
			listView.SetValue(Grid.RowProperty, 1);
			listView.SetValue(VisualElement.BackgroundColorProperty, Color.Transparent);
			listView.SetValue(ListView.HasUnevenRowsProperty, true);
			listView.SetValue(VisualElement.IsVisibleProperty, new VisualElement.VisibilityConverter().ConvertFromInvariantString("True"));
			listView.ItemSelected += this.LvBrands_ItemSelected;
			bindingExtension.Path = "Brands";
			BindingBase bindingBase = bindingExtension.ProvideValue(null);
			listView.SetBinding(ItemsView<Cell>.ItemsSourceProperty, bindingBase);
			IDataTemplate dataTemplate3 = dataTemplate;
			ProfileSelectorV2.<InitializeComponent>_anonXamlCDataTemplate_100 <InitializeComponent>_anonXamlCDataTemplate_ = new ProfileSelectorV2.<InitializeComponent>_anonXamlCDataTemplate_100();
			object[] array3 = new object[0 + 5];
			array3[0] = dataTemplate;
			array3[1] = listView;
			array3[2] = grid;
			array3[3] = grid3;
			array3[4] = this;
			<InitializeComponent>_anonXamlCDataTemplate_.parentValues = array3;
			<InitializeComponent>_anonXamlCDataTemplate_.root = this;
			dataTemplate3.LoadTemplate = new Func<object>(<InitializeComponent>_anonXamlCDataTemplate_.LoadDataTemplate);
			listView.SetValue(ItemsView<Cell>.ItemTemplateProperty, dataTemplate);
			grid.Children.Add(listView);
			grid3.Children.Add(grid);
			grid2.SetValue(Grid.RowProperty, 2);
			grid2.SetValue(VisualElement.IsVisibleProperty, new VisualElement.VisibilityConverter().ConvertFromInvariantString("False"));
			rowDefinition7.SetValue(RowDefinition.HeightProperty, new GridLengthTypeConverter().ConvertFromInvariantString("auto"));
			grid2.GetValue(Grid.RowDefinitionsProperty).Add(rowDefinition7);
			rowDefinition8.SetValue(RowDefinition.HeightProperty, new GridLengthTypeConverter().ConvertFromInvariantString("auto"));
			grid2.GetValue(Grid.RowDefinitionsProperty).Add(rowDefinition8);
			rowDefinition9.SetValue(RowDefinition.HeightProperty, new GridLengthTypeConverter().ConvertFromInvariantString("*"));
			grid2.GetValue(Grid.RowDefinitionsProperty).Add(rowDefinition9);
			rowDefinition10.SetValue(RowDefinition.HeightProperty, new GridLengthTypeConverter().ConvertFromInvariantString("auto"));
			grid2.GetValue(Grid.RowDefinitionsProperty).Add(rowDefinition10);
			label2.SetValue(Grid.RowProperty, 0);
			label2.SetValue(Label.FontAttributesProperty, new FontAttributesConverter().ConvertFromInvariantString("Bold"));
			dynamicResourceExtension3.Key = "BaseFontSize++";
			IMarkupExtension<DynamicResource> markupExtension3 = dynamicResourceExtension3;
			XamlServiceProvider xamlServiceProvider3 = new XamlServiceProvider();
			Type typeFromHandle5 = typeof(IProvideValueTarget);
			object[] array4 = new object[0 + 4];
			array4[0] = label2;
			array4[1] = grid2;
			array4[2] = grid3;
			array4[3] = this;
			object obj3;
			xamlServiceProvider3.Add(typeFromHandle5, obj3 = new SimpleValueTargetProvider(array4, Label.FontSizeProperty, nameScope));
			xamlServiceProvider3.Add(typeof(IReferenceProvider), obj3);
			Type typeFromHandle6 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver3 = new XmlNamespaceResolver();
			xmlNamespaceResolver3.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver3.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver3.Add("local", "clr-namespace:CarScannerXamarinForms");
			xamlServiceProvider3.Add(typeFromHandle6, new XamlTypeResolver(xmlNamespaceResolver3, typeof(ProfileSelectorV2).GetTypeInfo().Assembly));
			xamlServiceProvider3.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(74, 21)));
			DynamicResource dynamicResource3 = markupExtension3.ProvideValue(xamlServiceProvider3);
			label2.SetDynamicResource(Label.FontSizeProperty, dynamicResource3.Key);
			label2.SetValue(View.HorizontalOptionsProperty, LayoutOptions.Center);
			label2.SetValue(Label.HorizontalTextAlignmentProperty, new TextAlignmentConverter().ConvertFromInvariantString("Center"));
			bindingExtension2.Path = "Name";
			BindingBase bindingBase2 = bindingExtension2.ProvideValue(null);
			label2.SetBinding(Label.TextProperty, bindingBase2);
			grid2.Children.Add(label2);
			entry2.SetValue(Grid.RowProperty, 1);
			entry2.Completed += this.EntrySearch_Completed;
			entry2.SetValue(InputView.IsSpellCheckEnabledProperty, false);
			entry2.SetValue(Entry.PlaceholderProperty, "\ud83d\udd0e");
			entry2.TextChanged += this.searchBarProfiles_TextChanged;
			grid2.Children.Add(entry2);
			listView2.SetValue(Grid.RowProperty, 2);
			listView2.SetValue(VisualElement.BackgroundColorProperty, Color.Transparent);
			listView2.SetValue(ListView.HasUnevenRowsProperty, true);
			listView2.ItemSelected += this.LvProfiles_ItemSelected;
			IDataTemplate dataTemplate4 = dataTemplate2;
			ProfileSelectorV2.<InitializeComponent>_anonXamlCDataTemplate_101 <InitializeComponent>_anonXamlCDataTemplate_2 = new ProfileSelectorV2.<InitializeComponent>_anonXamlCDataTemplate_101();
			object[] array5 = new object[0 + 5];
			array5[0] = dataTemplate2;
			array5[1] = listView2;
			array5[2] = grid2;
			array5[3] = grid3;
			array5[4] = this;
			<InitializeComponent>_anonXamlCDataTemplate_2.parentValues = array5;
			<InitializeComponent>_anonXamlCDataTemplate_2.root = this;
			dataTemplate4.LoadTemplate = new Func<object>(<InitializeComponent>_anonXamlCDataTemplate_2.LoadDataTemplate);
			listView2.SetValue(ItemsView<Cell>.ItemTemplateProperty, dataTemplate2);
			grid2.Children.Add(listView2);
			label3.SetValue(Grid.RowProperty, 3);
			dynamicResourceExtension4.Key = "BaseFontSize";
			IMarkupExtension<DynamicResource> markupExtension4 = dynamicResourceExtension4;
			XamlServiceProvider xamlServiceProvider4 = new XamlServiceProvider();
			Type typeFromHandle7 = typeof(IProvideValueTarget);
			object[] array6 = new object[0 + 4];
			array6[0] = label3;
			array6[1] = grid2;
			array6[2] = grid3;
			array6[3] = this;
			object obj4;
			xamlServiceProvider4.Add(typeFromHandle7, obj4 = new SimpleValueTargetProvider(array6, Label.FontSizeProperty, nameScope));
			xamlServiceProvider4.Add(typeof(IReferenceProvider), obj4);
			Type typeFromHandle8 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver4 = new XmlNamespaceResolver();
			xmlNamespaceResolver4.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver4.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver4.Add("local", "clr-namespace:CarScannerXamarinForms");
			xamlServiceProvider4.Add(typeFromHandle8, new XamlTypeResolver(xmlNamespaceResolver4, typeof(ProfileSelectorV2).GetTypeInfo().Assembly));
			xamlServiceProvider4.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(125, 21)));
			DynamicResource dynamicResource4 = markupExtension4.ProvideValue(xamlServiceProvider4);
			label3.SetDynamicResource(Label.FontSizeProperty, dynamicResource4.Key);
			label3.SetValue(View.HorizontalOptionsProperty, LayoutOptions.Center);
			label3.SetValue(Label.HorizontalTextAlignmentProperty, new TextAlignmentConverter().ConvertFromInvariantString("Center"));
			translate.Text = "ios_DontKnowProfile";
			IMarkupExtension markupExtension5 = translate;
			XamlServiceProvider xamlServiceProvider5 = new XamlServiceProvider();
			Type typeFromHandle9 = typeof(IProvideValueTarget);
			object[] array7 = new object[0 + 4];
			array7[0] = label3;
			array7[1] = grid2;
			array7[2] = grid3;
			array7[3] = this;
			object obj5;
			xamlServiceProvider5.Add(typeFromHandle9, obj5 = new SimpleValueTargetProvider(array7, Label.TextProperty, nameScope));
			xamlServiceProvider5.Add(typeof(IReferenceProvider), obj5);
			Type typeFromHandle10 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver5 = new XmlNamespaceResolver();
			xmlNamespaceResolver5.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver5.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver5.Add("local", "clr-namespace:CarScannerXamarinForms");
			xamlServiceProvider5.Add(typeFromHandle10, new XamlTypeResolver(xmlNamespaceResolver5, typeof(ProfileSelectorV2).GetTypeInfo().Assembly));
			xamlServiceProvider5.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(128, 21)));
			object obj6 = markupExtension5.ProvideValue(xamlServiceProvider5);
			label3.Text = obj6;
			label3.SetValue(Label.TextColorProperty, Color.Red);
			grid2.Children.Add(label3);
			grid3.Children.Add(grid2);
			activityFrame.SetValue(Grid.RowProperty, 2);
			activityFrame.SetValue(View.HorizontalOptionsProperty, LayoutOptions.Center);
			activityFrame.SetValue(VisualElement.IsVisibleProperty, new VisualElement.VisibilityConverter().ConvertFromInvariantString("False"));
			activityFrame.SetValue(View.VerticalOptionsProperty, LayoutOptions.Center);
			grid3.Children.Add(activityFrame);
			button.SetValue(Grid.RowProperty, 3);
			button.SetValue(VisualElement.BackgroundColorProperty, Color.Red);
			button.Clicked += this.BtnBack_Clicked;
			translate2.Text = "ios_Back";
			IMarkupExtension markupExtension6 = translate2;
			XamlServiceProvider xamlServiceProvider6 = new XamlServiceProvider();
			Type typeFromHandle11 = typeof(IProvideValueTarget);
			object[] array8 = new object[0 + 3];
			array8[0] = button;
			array8[1] = grid3;
			array8[2] = this;
			object obj7;
			xamlServiceProvider6.Add(typeFromHandle11, obj7 = new SimpleValueTargetProvider(array8, Button.TextProperty, nameScope));
			xamlServiceProvider6.Add(typeof(IReferenceProvider), obj7);
			Type typeFromHandle12 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver6 = new XmlNamespaceResolver();
			xmlNamespaceResolver6.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver6.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver6.Add("local", "clr-namespace:CarScannerXamarinForms");
			xamlServiceProvider6.Add(typeFromHandle12, new XamlTypeResolver(xmlNamespaceResolver6, typeof(ProfileSelectorV2).GetTypeInfo().Assembly));
			xamlServiceProvider6.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(143, 17)));
			object obj8 = markupExtension6.ProvideValue(xamlServiceProvider6);
			button.Text = obj8;
			button.SetValue(Button.TextColorProperty, Color.White);
			grid3.Children.Add(button);
			this.SetValue(ContentView.ContentProperty, grid3);
		}

		// Token: 0x06001EDA RID: 7898 RVA: 0x00150648 File Offset: 0x0014E848
		// Note: this type is marked as 'beforefieldinit'.
		static ProfileSelectorV2()
		{
		}

		// Token: 0x06001EDB RID: 7899 RVA: 0x00150730 File Offset: 0x0014E930
		private void __InitComponentRuntime()
		{
			Extensions.LoadFromXaml<ProfileSelectorV2>(this, typeof(ProfileSelectorV2));
			this.layoutRoot = NameScopeExtensions.FindByName<Grid>(this, "layoutRoot");
			this.lbTitle = NameScopeExtensions.FindByName<Label>(this, "lbTitle");
			this.gridBrands = NameScopeExtensions.FindByName<Grid>(this, "gridBrands");
			this.searchBarBrands = NameScopeExtensions.FindByName<Entry>(this, "searchBarBrands");
			this.lvBrands = NameScopeExtensions.FindByName<ListView>(this, "lvBrands");
			this.gridProfiles = NameScopeExtensions.FindByName<Grid>(this, "gridProfiles");
			this.lbSelectedBrand = NameScopeExtensions.FindByName<Label>(this, "lbSelectedBrand");
			this.searchBarProfiles = NameScopeExtensions.FindByName<Entry>(this, "searchBarProfiles");
			this.lvProfiles = NameScopeExtensions.FindByName<ListView>(this, "lvProfiles");
			this.activityFrame = NameScopeExtensions.FindByName<ActivityFrame>(this, "activityFrame");
			this.btnBack = NameScopeExtensions.FindByName<Button>(this, "btnBack");
		}

		// Token: 0x04000F05 RID: 3845
		[CompilerGenerated]
		private string <SelectedBrand>k__BackingField;

		// Token: 0x04000F06 RID: 3846
		[CompilerGenerated]
		private EventHandler<bool> WindowCloseRequested;

		// Token: 0x04000F07 RID: 3847
		public static readonly BindableProperty TitleVisibleProperty = BindableProperty.Create("TitleVisible", typeof(bool), typeof(ProfileSelectorV2), null, 2, null, new BindableProperty.BindingPropertyChangedDelegate(ProfileSelectorV2.TitleVisiblePropertyChanged), null, null, null);

		// Token: 0x04000F08 RID: 3848
		public static readonly BindableProperty BackButtonVisibleProperty = BindableProperty.Create("BackButtonVisible", typeof(bool), typeof(ProfileSelectorV2), null, 2, null, new BindableProperty.BindingPropertyChangedDelegate(ProfileSelectorV2.BackButtonVisiblePropertyChanged), null, null, null);

		// Token: 0x04000F09 RID: 3849
		public static readonly BindableProperty CreateBackItemProperty = BindableProperty.Create("CreateBackItem", typeof(bool), typeof(ProfileSelectorV2), false, 2, null, new BindableProperty.BindingPropertyChangedDelegate(ProfileSelectorV2.CreateBackItemPropertyChanged), null, null, null);

		// Token: 0x04000F0A RID: 3850
		public static readonly BindableProperty TitleTextProperty = BindableProperty.Create("TitleText", typeof(string), typeof(ProfileSelectorV2), null, 2, null, new BindableProperty.BindingPropertyChangedDelegate(ProfileSelectorV2.TitleTextPropertyChanged), null, null, null);

		// Token: 0x04000F0B RID: 3851
		[CompilerGenerated]
		private ProfileV2Model <Model>k__BackingField;

		// Token: 0x04000F0C RID: 3852
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private Grid layoutRoot;

		// Token: 0x04000F0D RID: 3853
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private Label lbTitle;

		// Token: 0x04000F0E RID: 3854
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private Grid gridBrands;

		// Token: 0x04000F0F RID: 3855
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private Entry searchBarBrands;

		// Token: 0x04000F10 RID: 3856
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private ListView lvBrands;

		// Token: 0x04000F11 RID: 3857
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private Grid gridProfiles;

		// Token: 0x04000F12 RID: 3858
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private Label lbSelectedBrand;

		// Token: 0x04000F13 RID: 3859
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private Entry searchBarProfiles;

		// Token: 0x04000F14 RID: 3860
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private ListView lvProfiles;

		// Token: 0x04000F15 RID: 3861
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private ActivityFrame activityFrame;

		// Token: 0x04000F16 RID: 3862
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private Button btnBack;

		// Token: 0x0200027E RID: 638
		private enum Step
		{
			// Token: 0x04000F18 RID: 3864
			BrandSelection,
			// Token: 0x04000F19 RID: 3865
			ProfileSelection
		}

		// Token: 0x0200027F RID: 639
		[CompilerGenerated]
		[Serializable]
		private sealed class <>c
		{
			// Token: 0x06001EDC RID: 7900 RVA: 0x00150809 File Offset: 0x0014EA09
			// Note: this type is marked as 'beforefieldinit'.
			static <>c()
			{
			}

			// Token: 0x06001EDD RID: 7901 RVA: 0x00002050 File Offset: 0x00000250
			public <>c()
			{
			}

			// Token: 0x06001EDE RID: 7902 RVA: 0x00150815 File Offset: 0x0014EA15
			internal bool <SelectBrand>b__36_0(OBDReaderProfileV2 x)
			{
				return x.UpdateAliases.Any((string ua) => ua == SharedSettings.Current.ProfileUpdateAlias);
			}

			// Token: 0x06001EDF RID: 7903 RVA: 0x00150841 File Offset: 0x0014EA41
			internal bool <SelectBrand>b__36_1(string ua)
			{
				return ua == SharedSettings.Current.ProfileUpdateAlias;
			}

			// Token: 0x04000F1A RID: 3866
			public static readonly ProfileSelectorV2.<>c <>9 = new ProfileSelectorV2.<>c();

			// Token: 0x04000F1B RID: 3867
			public static Func<string, bool> <>9__36_1;

			// Token: 0x04000F1C RID: 3868
			public static Func<OBDReaderProfileV2, bool> <>9__36_0;
		}

		// Token: 0x02000280 RID: 640
		[CompilerGenerated]
		private sealed class <>c__DisplayClass39_0
		{
			// Token: 0x06001EE0 RID: 7904 RVA: 0x00002050 File Offset: 0x00000250
			public <>c__DisplayClass39_0()
			{
			}

			// Token: 0x06001EE1 RID: 7905 RVA: 0x00150853 File Offset: 0x0014EA53
			internal void <ApplyProfile>b__0()
			{
				this.profile.Apply(this.<>4__this.SelectedBrand);
			}

			// Token: 0x04000F1D RID: 3869
			public OBDReaderProfileV2 profile;

			// Token: 0x04000F1E RID: 3870
			public ProfileSelectorV2 <>4__this;
		}

		// Token: 0x02000281 RID: 641
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <ApplyProfile>d__39 : IAsyncStateMachine
		{
			// Token: 0x06001EE2 RID: 7906 RVA: 0x0015086C File Offset: 0x0014EA6C
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				ProfileSelectorV2 profileSelectorV = this;
				try
				{
					TaskAwaiter taskAwaiter3;
					TaskAwaiter<bool> taskAwaiter5;
					if (num != 0)
					{
						if (num == 1)
						{
							TaskAwaiter taskAwaiter4;
							taskAwaiter3 = taskAwaiter4;
							taskAwaiter4 = default(TaskAwaiter);
							num2 = -1;
							goto IL_0161;
						}
						CS$<>8__locals1 = new ProfileSelectorV2.<>c__DisplayClass39_0();
						CS$<>8__locals1.profile = profile;
						CS$<>8__locals1.<>4__this = this;
						taskAwaiter5 = App.GetCurrentPage().DisplayAlert(Translate.GetString("ios_ApplyProfileQuestion"), profileSelectorV.SelectedBrand + " " + CS$<>8__locals1.profile.Name, "OK", Translate.GetString("btnCancel.Content")).GetAwaiter();
						if (!taskAwaiter5.IsCompleted)
						{
							num2 = 0;
							taskAwaiter2 = taskAwaiter5;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<bool>, ProfileSelectorV2.<ApplyProfile>d__39>(ref taskAwaiter5, ref this);
							return;
						}
					}
					else
					{
						taskAwaiter5 = taskAwaiter2;
						taskAwaiter2 = default(TaskAwaiter<bool>);
						num2 = -1;
					}
					if (!taskAwaiter5.GetResult())
					{
						goto IL_01AD;
					}
					profileSelectorV.activityFrame.IsVisible = true;
					profileSelectorV.gridProfiles.IsEnabled = false;
					taskAwaiter3 = Task.Run(delegate
					{
						CS$<>8__locals1.profile.Apply(CS$<>8__locals1.<>4__this.SelectedBrand);
					}).GetAwaiter();
					if (!taskAwaiter3.IsCompleted)
					{
						num2 = 1;
						TaskAwaiter taskAwaiter4 = taskAwaiter3;
						this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, ProfileSelectorV2.<ApplyProfile>d__39>(ref taskAwaiter3, ref this);
						return;
					}
					IL_0161:
					taskAwaiter3.GetResult();
					SharedSettings.Current.ShouldCheckProfilePIDs = CS$<>8__locals1.profile.ShouldCheckProfilePIDs;
					profileSelectorV.activityFrame.IsVisible = false;
					profileSelectorV.gridProfiles.IsEnabled = true;
					EventHandler<bool> windowCloseRequested = profileSelectorV.WindowCloseRequested;
					if (windowCloseRequested != null)
					{
						windowCloseRequested(profileSelectorV, true);
					}
					IL_01AD:;
				}
				catch (Exception ex)
				{
					num2 = -2;
					CS$<>8__locals1 = null;
					this.<>t__builder.SetException(ex);
					return;
				}
				num2 = -2;
				CS$<>8__locals1 = null;
				this.<>t__builder.SetResult();
			}

			// Token: 0x06001EE3 RID: 7907 RVA: 0x00150A80 File Offset: 0x0014EC80
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x04000F1F RID: 3871
			public int <>1__state;

			// Token: 0x04000F20 RID: 3872
			public AsyncTaskMethodBuilder <>t__builder;

			// Token: 0x04000F21 RID: 3873
			public OBDReaderProfileV2 profile;

			// Token: 0x04000F22 RID: 3874
			public ProfileSelectorV2 <>4__this;

			// Token: 0x04000F23 RID: 3875
			private ProfileSelectorV2.<>c__DisplayClass39_0 <>8__1;

			// Token: 0x04000F24 RID: 3876
			private TaskAwaiter<bool> <>u__1;

			// Token: 0x04000F25 RID: 3877
			private TaskAwaiter <>u__2;
		}

		// Token: 0x02000282 RID: 642
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <LvBrands_ItemSelected>d__40 : IAsyncStateMachine
		{
			// Token: 0x06001EE4 RID: 7908 RVA: 0x00150A90 File Offset: 0x0014EC90
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				ProfileSelectorV2 profileSelectorV = this;
				try
				{
					TaskAwaiter taskAwaiter;
					if (num != 0)
					{
						PlatformHelper.CommonService.HideKeyboard();
						if (profileSelectorV.lvBrands.SelectedItem == null)
						{
							goto IL_00D3;
						}
						string text = (string)profileSelectorV.lvBrands.SelectedItem;
						profileSelectorV.SelectedBrand = text;
						profileSelectorV.OnPropertyChanged("SelectedBrand");
						profileSelectorV.lvBrands.SelectedItem = null;
						taskAwaiter = profileSelectorV.SelectBrand(text).GetAwaiter();
						if (!taskAwaiter.IsCompleted)
						{
							num2 = 0;
							TaskAwaiter taskAwaiter2 = taskAwaiter;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, ProfileSelectorV2.<LvBrands_ItemSelected>d__40>(ref taskAwaiter, ref this);
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
				}
				catch (Exception ex)
				{
					num2 = -2;
					this.<>t__builder.SetException(ex);
					return;
				}
				IL_00D3:
				num2 = -2;
				this.<>t__builder.SetResult();
			}

			// Token: 0x06001EE5 RID: 7909 RVA: 0x00150B94 File Offset: 0x0014ED94
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x04000F26 RID: 3878
			public int <>1__state;

			// Token: 0x04000F27 RID: 3879
			public AsyncVoidMethodBuilder <>t__builder;

			// Token: 0x04000F28 RID: 3880
			public ProfileSelectorV2 <>4__this;

			// Token: 0x04000F29 RID: 3881
			private TaskAwaiter <>u__1;
		}

		// Token: 0x02000283 RID: 643
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <LvProfiles_ItemSelected>d__38 : IAsyncStateMachine
		{
			// Token: 0x06001EE6 RID: 7910 RVA: 0x00150BA4 File Offset: 0x0014EDA4
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				ProfileSelectorV2 profileSelectorV = this;
				try
				{
					TaskAwaiter taskAwaiter;
					if (num != 0)
					{
						PlatformHelper.CommonService.HideKeyboard();
						if (profileSelectorV.lvProfiles.SelectedItem == null)
						{
							goto IL_00EE;
						}
						OBDReaderProfileV2 obdreaderProfileV = profileSelectorV.lvProfiles.SelectedItem as OBDReaderProfileV2;
						profileSelectorV.lvProfiles.SelectedItem = null;
						int num3 = (profileSelectorV.lvProfiles.ItemsSource as BrandCollection).IndexOf(obdreaderProfileV);
						if (profileSelectorV.CreateBackItem && num3 == 0)
						{
							profileSelectorV.GoBack();
							goto IL_00D3;
						}
						taskAwaiter = profileSelectorV.ApplyProfile(obdreaderProfileV).GetAwaiter();
						if (!taskAwaiter.IsCompleted)
						{
							num2 = 0;
							TaskAwaiter taskAwaiter2 = taskAwaiter;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, ProfileSelectorV2.<LvProfiles_ItemSelected>d__38>(ref taskAwaiter, ref this);
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
					IL_00D3:;
				}
				catch (Exception ex)
				{
					num2 = -2;
					this.<>t__builder.SetException(ex);
					return;
				}
				IL_00EE:
				num2 = -2;
				this.<>t__builder.SetResult();
			}

			// Token: 0x06001EE7 RID: 7911 RVA: 0x00150CC4 File Offset: 0x0014EEC4
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x04000F2A RID: 3882
			public int <>1__state;

			// Token: 0x04000F2B RID: 3883
			public AsyncVoidMethodBuilder <>t__builder;

			// Token: 0x04000F2C RID: 3884
			public ProfileSelectorV2 <>4__this;

			// Token: 0x04000F2D RID: 3885
			private TaskAwaiter <>u__1;
		}

		// Token: 0x02000284 RID: 644
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <SelectBrand>d__36 : IAsyncStateMachine
		{
			// Token: 0x06001EE8 RID: 7912 RVA: 0x00150CD4 File Offset: 0x0014EED4
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				ProfileSelectorV2 profileSelectorV = this;
				try
				{
					TaskAwaiter taskAwaiter;
					if (num != 0)
					{
						if (num == 1)
						{
							TaskAwaiter taskAwaiter2;
							taskAwaiter = taskAwaiter2;
							taskAwaiter2 = default(TaskAwaiter);
							num2 = -1;
							goto IL_0159;
						}
						profileSelectorV.gridBrands.IsEnabled = false;
						profileSelectorV.btnBack.IsEnabled = false;
						profileSelectorV.activityFrame.IsVisible = true;
						if (profileSelectorV.Model.ProfilesLoaded)
						{
							goto IL_00A9;
						}
						taskAwaiter = profileSelectorV.Model.LoadProfilesAsync().GetAwaiter();
						if (!taskAwaiter.IsCompleted)
						{
							num2 = 0;
							TaskAwaiter taskAwaiter2 = taskAwaiter;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, ProfileSelectorV2.<SelectBrand>d__36>(ref taskAwaiter, ref this);
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
					IL_00A9:
					BrandCollection profilesForBrandWithFilter = profileSelectorV.Model.GetProfilesForBrandWithFilter(brand, "");
					if (profilesForBrandWithFilter.Count != 1 || !profilesForBrandWithFilter[0].Name.Contains("OBD"))
					{
						if (profileSelectorV.CreateBackItem)
						{
							profilesForBrandWithFilter.Insert(0, new OBDReaderProfileV2
							{
								Name = "[ " + Translate.GetString("ios_Back2") + " ]"
							});
						}
						profileSelectorV.lbSelectedBrand.BindingContext = profilesForBrandWithFilter;
						profileSelectorV.lvProfiles.BindingContext = profilesForBrandWithFilter;
						profileSelectorV.lvProfiles.ItemsSource = profilesForBrandWithFilter;
						if (!string.IsNullOrEmpty(SharedSettings.Current.ProfileUpdateAlias))
						{
							profileSelectorV.lvProfiles.ItemSelected -= profileSelectorV.LvProfiles_ItemSelected;
							profileSelectorV.lvProfiles.SelectedItem = profilesForBrandWithFilter.FirstOrDefault((OBDReaderProfileV2 x) => x.UpdateAliases.Any((string ua) => ua == SharedSettings.Current.ProfileUpdateAlias));
							if (profileSelectorV.lvProfiles.SelectedItem != null)
							{
								profileSelectorV.lvProfiles.ScrollTo(profileSelectorV.lvProfiles.SelectedItem, 2, false);
							}
							profileSelectorV.lvProfiles.ItemSelected += profileSelectorV.LvProfiles_ItemSelected;
						}
						profileSelectorV.TitleText = Translate.GetString("ios_ConnectionProfile");
						profileSelectorV.activityFrame.IsVisible = false;
						profileSelectorV.gridBrands.IsEnabled = true;
						profileSelectorV.gridBrands.IsVisible = false;
						profileSelectorV.gridProfiles.IsVisible = true;
						profileSelectorV.btnBack.IsEnabled = true;
						goto IL_02C3;
					}
					profileSelectorV.SelectedBrand = brand;
					profileSelectorV.OnPropertyChanged("SelectedBrand");
					taskAwaiter = profileSelectorV.ApplyProfile(profilesForBrandWithFilter[0]).GetAwaiter();
					if (!taskAwaiter.IsCompleted)
					{
						num2 = 1;
						TaskAwaiter taskAwaiter2 = taskAwaiter;
						this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, ProfileSelectorV2.<SelectBrand>d__36>(ref taskAwaiter, ref this);
						return;
					}
					IL_0159:
					taskAwaiter.GetResult();
					profileSelectorV.gridBrands.IsEnabled = true;
					profileSelectorV.activityFrame.IsVisible = false;
					profileSelectorV.btnBack.IsEnabled = true;
					IL_02C3:;
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

			// Token: 0x06001EE9 RID: 7913 RVA: 0x00150FF0 File Offset: 0x0014F1F0
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x04000F2E RID: 3886
			public int <>1__state;

			// Token: 0x04000F2F RID: 3887
			public AsyncTaskMethodBuilder <>t__builder;

			// Token: 0x04000F30 RID: 3888
			public ProfileSelectorV2 <>4__this;

			// Token: 0x04000F31 RID: 3889
			public string brand;

			// Token: 0x04000F32 RID: 3890
			private TaskAwaiter <>u__1;
		}

		// Token: 0x02000285 RID: 645
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <searchBarBrands_TextChanged>d__43 : IAsyncStateMachine
		{
			// Token: 0x06001EEA RID: 7914 RVA: 0x00151000 File Offset: 0x0014F200
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				ProfileSelectorV2 profileSelectorV = this;
				try
				{
					try
					{
						TaskAwaiter taskAwaiter;
						if (num != 0)
						{
							searchBar = sender as Entry;
							filtertext = searchBar.Text;
							taskAwaiter = Task.Delay(500).GetAwaiter();
							if (!taskAwaiter.IsCompleted)
							{
								num2 = 0;
								TaskAwaiter taskAwaiter2 = taskAwaiter;
								this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, ProfileSelectorV2.<searchBarBrands_TextChanged>d__43>(ref taskAwaiter, ref this);
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
						if (filtertext == searchBar.Text)
						{
							try
							{
								if (profileSelectorV.CurrentStep == ProfileSelectorV2.Step.BrandSelection)
								{
									ObservableCollection<string> brandsWithFilter = profileSelectorV.Model.GetBrandsWithFilter(filtertext);
									profileSelectorV.gridBrands.BindingContext = brandsWithFilter;
									profileSelectorV.lvBrands.ItemsSource = brandsWithFilter;
								}
							}
							catch (Exception)
							{
							}
						}
						searchBar = null;
						filtertext = null;
					}
					catch
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

			// Token: 0x06001EEB RID: 7915 RVA: 0x0015115C File Offset: 0x0014F35C
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x04000F33 RID: 3891
			public int <>1__state;

			// Token: 0x04000F34 RID: 3892
			public AsyncVoidMethodBuilder <>t__builder;

			// Token: 0x04000F35 RID: 3893
			public object sender;

			// Token: 0x04000F36 RID: 3894
			public ProfileSelectorV2 <>4__this;

			// Token: 0x04000F37 RID: 3895
			private Entry <searchBar>5__2;

			// Token: 0x04000F38 RID: 3896
			private string <filtertext>5__3;

			// Token: 0x04000F39 RID: 3897
			private TaskAwaiter <>u__1;
		}

		// Token: 0x02000286 RID: 646
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <searchBarProfiles_TextChanged>d__42 : IAsyncStateMachine
		{
			// Token: 0x06001EEC RID: 7916 RVA: 0x0015116C File Offset: 0x0014F36C
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				ProfileSelectorV2 profileSelectorV = this;
				try
				{
					try
					{
						TaskAwaiter taskAwaiter;
						if (num != 0)
						{
							searchBar = sender as Entry;
							filtertext = searchBar.Text;
							taskAwaiter = Task.Delay(500).GetAwaiter();
							if (!taskAwaiter.IsCompleted)
							{
								num2 = 0;
								TaskAwaiter taskAwaiter2 = taskAwaiter;
								this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, ProfileSelectorV2.<searchBarProfiles_TextChanged>d__42>(ref taskAwaiter, ref this);
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
						if (filtertext == searchBar.Text)
						{
							try
							{
								if (profileSelectorV.CurrentStep == ProfileSelectorV2.Step.ProfileSelection)
								{
									BrandCollection profilesForBrandWithFilter = profileSelectorV.Model.GetProfilesForBrandWithFilter(profileSelectorV.SelectedBrand, filtertext);
									if (profileSelectorV.CreateBackItem)
									{
										profilesForBrandWithFilter.Insert(0, new OBDReaderProfileV2
										{
											Name = "[ " + Translate.GetString("ios_Back2") + " ]"
										});
									}
									profileSelectorV.gridProfiles.BindingContext = profilesForBrandWithFilter;
									profileSelectorV.lvProfiles.ItemsSource = profilesForBrandWithFilter;
								}
							}
							catch (Exception)
							{
							}
						}
						searchBar = null;
						filtertext = null;
					}
					catch
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

			// Token: 0x06001EED RID: 7917 RVA: 0x00151324 File Offset: 0x0014F524
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x04000F3A RID: 3898
			public int <>1__state;

			// Token: 0x04000F3B RID: 3899
			public AsyncVoidMethodBuilder <>t__builder;

			// Token: 0x04000F3C RID: 3900
			public object sender;

			// Token: 0x04000F3D RID: 3901
			public ProfileSelectorV2 <>4__this;

			// Token: 0x04000F3E RID: 3902
			private Entry <searchBar>5__2;

			// Token: 0x04000F3F RID: 3903
			private string <filtertext>5__3;

			// Token: 0x04000F40 RID: 3904
			private TaskAwaiter <>u__1;
		}

		// Token: 0x02000287 RID: 647
		[CompilerGenerated]
		private sealed class <InitializeComponent>_anonXamlCDataTemplate_100
		{
			// Token: 0x06001EEE RID: 7918 RVA: 0x00151334 File Offset: 0x0014F534
			public <InitializeComponent>_anonXamlCDataTemplate_100()
			{
			}

			// Token: 0x06001EEF RID: 7919 RVA: 0x00151348 File Offset: 0x0014F548
			internal object LoadDataTemplate()
			{
				BindingExtension bindingExtension;
				VisualDiagnostics.RegisterSourceInfo(bindingExtension = new BindingExtension(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Settings\\ProfileSelectorV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 54, 39);
				DynamicResourceExtension dynamicResourceExtension;
				VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension = new DynamicResourceExtension(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Settings\\ProfileSelectorV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 54, 58);
				TextCell textCell;
				VisualDiagnostics.RegisterSourceInfo(textCell = new TextCell(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Settings\\ProfileSelectorV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 54, 30);
				NameScope nameScope = new NameScope();
				NameScope.SetNameScope(textCell, nameScope);
				bindingExtension.Path = ".";
				BindingBase bindingBase = bindingExtension.ProvideValue(null);
				textCell.SetBinding(TextCell.TextProperty, bindingBase);
				dynamicResourceExtension.Key = "TextColor";
				IMarkupExtension<DynamicResource> markupExtension = dynamicResourceExtension;
				XamlServiceProvider xamlServiceProvider = new XamlServiceProvider();
				Type typeFromHandle = typeof(IProvideValueTarget);
				int num;
				object[] array = new object[(num = this.parentValues.Length) + 1];
				Array.Copy(this.parentValues, 0, array, 1, num);
				object[] array2 = array;
				array2[0] = textCell;
				object obj;
				xamlServiceProvider.Add(typeFromHandle, obj = new SimpleValueTargetProvider(array2, TextCell.TextColorProperty, nameScope));
				xamlServiceProvider.Add(typeof(IReferenceProvider), obj);
				Type typeFromHandle2 = typeof(IXamlTypeResolver);
				XmlNamespaceResolver xmlNamespaceResolver = new XmlNamespaceResolver();
				xmlNamespaceResolver.Add("", "http://xamarin.com/schemas/2014/forms");
				xmlNamespaceResolver.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
				xmlNamespaceResolver.Add("local", "clr-namespace:CarScannerXamarinForms");
				xamlServiceProvider.Add(typeFromHandle2, new XamlTypeResolver(xmlNamespaceResolver, typeof(ProfileSelectorV2.<InitializeComponent>_anonXamlCDataTemplate_100).GetTypeInfo().Assembly));
				xamlServiceProvider.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(54, 58)));
				DynamicResource dynamicResource = markupExtension.ProvideValue(xamlServiceProvider);
				textCell.SetDynamicResource(TextCell.TextColorProperty, dynamicResource.Key);
				return textCell;
			}

			// Token: 0x04000F41 RID: 3905
			internal object[] parentValues;

			// Token: 0x04000F42 RID: 3906
			internal ProfileSelectorV2 root;
		}

		// Token: 0x02000288 RID: 648
		[CompilerGenerated]
		private sealed class <InitializeComponent>_anonXamlCDataTemplate_101
		{
			// Token: 0x06001EF0 RID: 7920 RVA: 0x00151504 File Offset: 0x0014F704
			public <InitializeComponent>_anonXamlCDataTemplate_101()
			{
			}

			// Token: 0x06001EF1 RID: 7921 RVA: 0x00151518 File Offset: 0x0014F718
			internal object LoadDataTemplate()
			{
				RowDefinition rowDefinition;
				VisualDiagnostics.RegisterSourceInfo(rowDefinition = new RowDefinition(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Settings\\ProfileSelectorV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 96, 42);
				RowDefinition rowDefinition2;
				VisualDiagnostics.RegisterSourceInfo(rowDefinition2 = new RowDefinition(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Settings\\ProfileSelectorV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 97, 42);
				DynamicResourceExtension dynamicResourceExtension;
				VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension = new DynamicResourceExtension(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Settings\\ProfileSelectorV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 103, 41);
				BindingExtension bindingExtension;
				VisualDiagnostics.RegisterSourceInfo(bindingExtension = new BindingExtension(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Settings\\ProfileSelectorV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 105, 41);
				DynamicResourceExtension dynamicResourceExtension2;
				VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension2 = new DynamicResourceExtension(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Settings\\ProfileSelectorV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 106, 41);
				Label label;
				VisualDiagnostics.RegisterSourceInfo(label = new Label(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Settings\\ProfileSelectorV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 99, 38);
				DynamicResourceExtension dynamicResourceExtension3;
				VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension3 = new DynamicResourceExtension(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Settings\\ProfileSelectorV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 112, 41);
				BindingExtension bindingExtension2;
				VisualDiagnostics.RegisterSourceInfo(bindingExtension2 = new BindingExtension(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Settings\\ProfileSelectorV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 114, 41);
				DynamicResourceExtension dynamicResourceExtension4;
				VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension4 = new DynamicResourceExtension(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Settings\\ProfileSelectorV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 115, 41);
				Label label2;
				VisualDiagnostics.RegisterSourceInfo(label2 = new Label(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Settings\\ProfileSelectorV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 109, 38);
				Grid grid;
				VisualDiagnostics.RegisterSourceInfo(grid = new Grid(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Settings\\ProfileSelectorV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 94, 34);
				ViewCell viewCell;
				VisualDiagnostics.RegisterSourceInfo(viewCell = new ViewCell(), new Uri("C:\\xp\\CarScannerXF2\\CarScanner.LegacyUI\\Settings\\ProfileSelectorV2.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 93, 30);
				NameScope nameScope = new NameScope();
				NameScope.SetNameScope(viewCell, nameScope);
				grid.SetValue(Grid.RowSpacingProperty, 0.0);
				rowDefinition.SetValue(RowDefinition.HeightProperty, new GridLengthTypeConverter().ConvertFromInvariantString("auto"));
				grid.GetValue(Grid.RowDefinitionsProperty).Add(rowDefinition);
				rowDefinition2.SetValue(RowDefinition.HeightProperty, new GridLengthTypeConverter().ConvertFromInvariantString("auto"));
				grid.GetValue(Grid.RowDefinitionsProperty).Add(rowDefinition2);
				label.SetValue(Grid.RowProperty, 0);
				label.SetValue(View.MarginProperty, new Thickness(0.0));
				label.SetValue(Label.FontAttributesProperty, new FontAttributesConverter().ConvertFromInvariantString("Bold"));
				dynamicResourceExtension.Key = "BaseFontSize+";
				IMarkupExtension<DynamicResource> markupExtension = dynamicResourceExtension;
				XamlServiceProvider xamlServiceProvider = new XamlServiceProvider();
				Type typeFromHandle = typeof(IProvideValueTarget);
				int num;
				object[] array = new object[(num = this.parentValues.Length) + 3];
				Array.Copy(this.parentValues, 0, array, 3, num);
				object[] array2 = array;
				array2[0] = label;
				array2[1] = grid;
				array2[2] = viewCell;
				object obj;
				xamlServiceProvider.Add(typeFromHandle, obj = new SimpleValueTargetProvider(array2, Label.FontSizeProperty, nameScope));
				xamlServiceProvider.Add(typeof(IReferenceProvider), obj);
				Type typeFromHandle2 = typeof(IXamlTypeResolver);
				XmlNamespaceResolver xmlNamespaceResolver = new XmlNamespaceResolver();
				xmlNamespaceResolver.Add("", "http://xamarin.com/schemas/2014/forms");
				xmlNamespaceResolver.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
				xmlNamespaceResolver.Add("local", "clr-namespace:CarScannerXamarinForms");
				xamlServiceProvider.Add(typeFromHandle2, new XamlTypeResolver(xmlNamespaceResolver, typeof(ProfileSelectorV2.<InitializeComponent>_anonXamlCDataTemplate_101).GetTypeInfo().Assembly));
				xamlServiceProvider.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(103, 41)));
				DynamicResource dynamicResource = markupExtension.ProvideValue(xamlServiceProvider);
				label.SetDynamicResource(Label.FontSizeProperty, dynamicResource.Key);
				label.SetValue(Label.LineBreakModeProperty, 1);
				bindingExtension.Path = "Name";
				BindingBase bindingBase = bindingExtension.ProvideValue(null);
				label.SetBinding(Label.TextProperty, bindingBase);
				dynamicResourceExtension2.Key = "TextColor";
				IMarkupExtension<DynamicResource> markupExtension2 = dynamicResourceExtension2;
				XamlServiceProvider xamlServiceProvider2 = new XamlServiceProvider();
				Type typeFromHandle3 = typeof(IProvideValueTarget);
				int num2;
				object[] array3 = new object[(num2 = this.parentValues.Length) + 3];
				Array.Copy(this.parentValues, 0, array3, 3, num2);
				object[] array4 = array3;
				array4[0] = label;
				array4[1] = grid;
				array4[2] = viewCell;
				object obj2;
				xamlServiceProvider2.Add(typeFromHandle3, obj2 = new SimpleValueTargetProvider(array4, Label.TextColorProperty, nameScope));
				xamlServiceProvider2.Add(typeof(IReferenceProvider), obj2);
				Type typeFromHandle4 = typeof(IXamlTypeResolver);
				XmlNamespaceResolver xmlNamespaceResolver2 = new XmlNamespaceResolver();
				xmlNamespaceResolver2.Add("", "http://xamarin.com/schemas/2014/forms");
				xmlNamespaceResolver2.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
				xmlNamespaceResolver2.Add("local", "clr-namespace:CarScannerXamarinForms");
				xamlServiceProvider2.Add(typeFromHandle4, new XamlTypeResolver(xmlNamespaceResolver2, typeof(ProfileSelectorV2.<InitializeComponent>_anonXamlCDataTemplate_101).GetTypeInfo().Assembly));
				xamlServiceProvider2.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(106, 41)));
				DynamicResource dynamicResource2 = markupExtension2.ProvideValue(xamlServiceProvider2);
				label.SetDynamicResource(Label.TextColorProperty, dynamicResource2.Key);
				label.SetValue(View.VerticalOptionsProperty, LayoutOptions.Center);
				label.SetValue(Label.VerticalTextAlignmentProperty, new TextAlignmentConverter().ConvertFromInvariantString("Center"));
				grid.Children.Add(label);
				label2.SetValue(Grid.RowProperty, 1);
				label2.SetValue(View.MarginProperty, new Thickness(0.0, 0.0, 0.0, 5.0));
				dynamicResourceExtension3.Key = "BaseFontSize";
				IMarkupExtension<DynamicResource> markupExtension3 = dynamicResourceExtension3;
				XamlServiceProvider xamlServiceProvider3 = new XamlServiceProvider();
				Type typeFromHandle5 = typeof(IProvideValueTarget);
				int num3;
				object[] array5 = new object[(num3 = this.parentValues.Length) + 3];
				Array.Copy(this.parentValues, 0, array5, 3, num3);
				object[] array6 = array5;
				array6[0] = label2;
				array6[1] = grid;
				array6[2] = viewCell;
				object obj3;
				xamlServiceProvider3.Add(typeFromHandle5, obj3 = new SimpleValueTargetProvider(array6, Label.FontSizeProperty, nameScope));
				xamlServiceProvider3.Add(typeof(IReferenceProvider), obj3);
				Type typeFromHandle6 = typeof(IXamlTypeResolver);
				XmlNamespaceResolver xmlNamespaceResolver3 = new XmlNamespaceResolver();
				xmlNamespaceResolver3.Add("", "http://xamarin.com/schemas/2014/forms");
				xmlNamespaceResolver3.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
				xmlNamespaceResolver3.Add("local", "clr-namespace:CarScannerXamarinForms");
				xamlServiceProvider3.Add(typeFromHandle6, new XamlTypeResolver(xmlNamespaceResolver3, typeof(ProfileSelectorV2.<InitializeComponent>_anonXamlCDataTemplate_101).GetTypeInfo().Assembly));
				xamlServiceProvider3.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(112, 41)));
				DynamicResource dynamicResource3 = markupExtension3.ProvideValue(xamlServiceProvider3);
				label2.SetDynamicResource(Label.FontSizeProperty, dynamicResource3.Key);
				label2.SetValue(Label.LineBreakModeProperty, 1);
				bindingExtension2.Path = "Description";
				BindingBase bindingBase2 = bindingExtension2.ProvideValue(null);
				label2.SetBinding(Label.TextProperty, bindingBase2);
				dynamicResourceExtension4.Key = "TextColor";
				IMarkupExtension<DynamicResource> markupExtension4 = dynamicResourceExtension4;
				XamlServiceProvider xamlServiceProvider4 = new XamlServiceProvider();
				Type typeFromHandle7 = typeof(IProvideValueTarget);
				int num4;
				object[] array7 = new object[(num4 = this.parentValues.Length) + 3];
				Array.Copy(this.parentValues, 0, array7, 3, num4);
				object[] array8 = array7;
				array8[0] = label2;
				array8[1] = grid;
				array8[2] = viewCell;
				object obj4;
				xamlServiceProvider4.Add(typeFromHandle7, obj4 = new SimpleValueTargetProvider(array8, Label.TextColorProperty, nameScope));
				xamlServiceProvider4.Add(typeof(IReferenceProvider), obj4);
				Type typeFromHandle8 = typeof(IXamlTypeResolver);
				XmlNamespaceResolver xmlNamespaceResolver4 = new XmlNamespaceResolver();
				xmlNamespaceResolver4.Add("", "http://xamarin.com/schemas/2014/forms");
				xmlNamespaceResolver4.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
				xmlNamespaceResolver4.Add("local", "clr-namespace:CarScannerXamarinForms");
				xamlServiceProvider4.Add(typeFromHandle8, new XamlTypeResolver(xmlNamespaceResolver4, typeof(ProfileSelectorV2.<InitializeComponent>_anonXamlCDataTemplate_101).GetTypeInfo().Assembly));
				xamlServiceProvider4.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(115, 41)));
				DynamicResource dynamicResource4 = markupExtension4.ProvideValue(xamlServiceProvider4);
				label2.SetDynamicResource(Label.TextColorProperty, dynamicResource4.Key);
				label2.SetValue(View.VerticalOptionsProperty, LayoutOptions.Start);
				label2.SetValue(Label.VerticalTextAlignmentProperty, new TextAlignmentConverter().ConvertFromInvariantString("Start"));
				grid.Children.Add(label2);
				viewCell.View = grid;
				return viewCell;
			}

			// Token: 0x04000F43 RID: 3907
			internal object[] parentValues;

			// Token: 0x04000F44 RID: 3908
			internal ProfileSelectorV2 root;
		}
	}
}
