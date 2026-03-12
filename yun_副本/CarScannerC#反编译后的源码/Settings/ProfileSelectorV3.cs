using System;
using System.CodeDom.Compiler;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using AiForms.Renderers;
using CarScannerXamarinForms.PlatformAdapters;
using CarScannerXamarinForms.ProfilesV2;
using Xamarin.Forms;
using Xamarin.Forms.Internals;
using Xamarin.Forms.Xaml;
using Xamarin.Forms.Xaml.Diagnostics;
using Xamarin.Forms.Xaml.Internals;

namespace CarScannerXamarinForms.Settings
{
	// Token: 0x0200020B RID: 523
	[XamlCompilation(2)]
	[XamlFilePath("Settings\\ProfileSelectorV3.xaml")]
	public class ProfileSelectorV3 : ContentView, IProfileSelector, INotifyPropertyChanged
	{
		// Token: 0x06001A84 RID: 6788 RVA: 0x001254C8 File Offset: 0x001236C8
		public ProfileSelectorV3()
		{
			try
			{
				this.InitializeComponent();
				this.layoutRoot.BindingContext = this.Model;
				this.btnBack.IsVisible = this.BackButtonVisible;
				this.lbTitle.IsVisible = this.TitleVisible;
				this.TitleText = Translate.GetString("ios_ChooseCarBrand");
				this.lbTitle.Text = this.TitleText;
			}
			catch (Exception)
			{
			}
		}

		// Token: 0x06001A85 RID: 6789 RVA: 0x00125560 File Offset: 0x00123760
		private async void BrandsCell_Tapped(object sender, EventArgs e)
		{
			PlatformHelper.CommonService.HideKeyboard();
			string text = (string)((Element)sender).BindingContext;
			this.SelectedBrand = text;
			this.OnPropertyChanged("SelectedBrand");
			RadioCell.SetSelectedValue(this.lvBrandsSection, SharedSettings.Current.SelectedBrand);
			await this.SelectBrand(text);
		}

		// Token: 0x06001A86 RID: 6790 RVA: 0x001255A0 File Offset: 0x001237A0
		private async void ProfileCell_Tapped(object sender, EventArgs e)
		{
			PlatformHelper.CommonService.HideKeyboard();
			OBDReaderProfileV2 obdreaderProfileV = (OBDReaderProfileV2)((Element)sender).BindingContext;
			int num = (this.lvProfilesSection.ItemsSource as BrandCollection).IndexOf(obdreaderProfileV);
			if (this.CreateBackItem && num == 0)
			{
				this.GoBack();
			}
			else
			{
				await this.ApplyProfile(obdreaderProfileV);
			}
		}

		// Token: 0x06001A87 RID: 6791 RVA: 0x00022295 File Offset: 0x00020495
		private void EntrySearch_Completed(object sender, EventArgs e)
		{
			PlatformHelper.CommonService.HideKeyboard();
		}

		// Token: 0x17000F9D RID: 3997
		// (get) Token: 0x06001A88 RID: 6792 RVA: 0x001255DF File Offset: 0x001237DF
		private ProfileSelectorV3.Step CurrentStep
		{
			get
			{
				if (this.gridBrands.IsVisible)
				{
					return ProfileSelectorV3.Step.BrandSelection;
				}
				return ProfileSelectorV3.Step.ProfileSelection;
			}
		}

		// Token: 0x17000F9E RID: 3998
		// (get) Token: 0x06001A89 RID: 6793 RVA: 0x001255F1 File Offset: 0x001237F1
		// (set) Token: 0x06001A8A RID: 6794 RVA: 0x001255F9 File Offset: 0x001237F9
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

		// Token: 0x14000015 RID: 21
		// (add) Token: 0x06001A8B RID: 6795 RVA: 0x00125604 File Offset: 0x00123804
		// (remove) Token: 0x06001A8C RID: 6796 RVA: 0x0012563C File Offset: 0x0012383C
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

		// Token: 0x06001A8D RID: 6797 RVA: 0x00125671 File Offset: 0x00123871
		private static void TitleVisiblePropertyChanged(BindableObject bindable, object oldValue, object newValue)
		{
			((ProfileSelectorV3)bindable).lbTitle.IsVisible = (bool)newValue;
		}

		// Token: 0x06001A8E RID: 6798 RVA: 0x00125689 File Offset: 0x00123889
		private static void BackButtonVisiblePropertyChanged(BindableObject bindable, object oldValue, object newValue)
		{
			((ProfileSelectorV3)bindable).btnBack.IsVisible = (bool)newValue;
		}

		// Token: 0x17000F9F RID: 3999
		// (get) Token: 0x06001A8F RID: 6799 RVA: 0x001256A1 File Offset: 0x001238A1
		// (set) Token: 0x06001A90 RID: 6800 RVA: 0x001256B3 File Offset: 0x001238B3
		public bool BackButtonVisible
		{
			get
			{
				return (bool)base.GetValue(ProfileSelectorV3.BackButtonVisibleProperty);
			}
			set
			{
				base.SetValue(ProfileSelectorV3.BackButtonVisibleProperty, value);
			}
		}

		// Token: 0x06001A91 RID: 6801 RVA: 0x000027D4 File Offset: 0x000009D4
		private static void CreateBackItemPropertyChanged(BindableObject bindable, object oldValue, object newValue)
		{
		}

		// Token: 0x17000FA0 RID: 4000
		// (get) Token: 0x06001A92 RID: 6802 RVA: 0x001256C6 File Offset: 0x001238C6
		// (set) Token: 0x06001A93 RID: 6803 RVA: 0x001256D8 File Offset: 0x001238D8
		public bool CreateBackItem
		{
			get
			{
				return (bool)base.GetValue(ProfileSelectorV3.CreateBackItemProperty);
			}
			set
			{
				base.SetValue(ProfileSelectorV3.CreateBackItemProperty, value);
			}
		}

		// Token: 0x06001A94 RID: 6804 RVA: 0x001256EB File Offset: 0x001238EB
		private static void TitleTextPropertyChanged(BindableObject bindable, object oldValue, object newValue)
		{
			((ProfileSelectorV3)bindable).lbTitle.Text = (string)newValue;
		}

		// Token: 0x17000FA1 RID: 4001
		// (get) Token: 0x06001A95 RID: 6805 RVA: 0x00125703 File Offset: 0x00123903
		// (set) Token: 0x06001A96 RID: 6806 RVA: 0x00125715 File Offset: 0x00123915
		public bool TitleVisible
		{
			get
			{
				return (bool)base.GetValue(ProfileSelectorV3.TitleVisibleProperty);
			}
			set
			{
				base.SetValue(ProfileSelectorV3.TitleVisibleProperty, value);
			}
		}

		// Token: 0x17000FA2 RID: 4002
		// (get) Token: 0x06001A97 RID: 6807 RVA: 0x00125728 File Offset: 0x00123928
		// (set) Token: 0x06001A98 RID: 6808 RVA: 0x0012573A File Offset: 0x0012393A
		public string TitleText
		{
			get
			{
				return (string)base.GetValue(ProfileSelectorV3.TitleTextProperty);
			}
			set
			{
				base.SetValue(ProfileSelectorV3.TitleTextProperty, value);
			}
		}

		// Token: 0x17000FA3 RID: 4003
		// (get) Token: 0x06001A99 RID: 6809 RVA: 0x00125748 File Offset: 0x00123948
		// (set) Token: 0x06001A9A RID: 6810 RVA: 0x00125750 File Offset: 0x00123950
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

		// Token: 0x06001A9B RID: 6811 RVA: 0x0012575C File Offset: 0x0012395C
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
				this.lvProfilesSettings.BindingContext = profilesForBrandWithFilter;
				this.lvProfilesSection.ItemsSource = profilesForBrandWithFilter;
				this.TitleText = Translate.GetString("ios_ConnectionProfile");
				this.gridBrands.IsEnabled = true;
				this.gridProfiles.IsVisible = true;
				await ViewExtensions.FadeTo(this.gridProfiles, 0.0, 0U, null);
				await Task.WhenAll<bool>(new Task<bool>[]
				{
					ViewExtensions.FadeTo(this.gridProfiles, 1.0, 200U, null),
					ViewExtensions.FadeTo(this.gridBrands, 0.0, 200U, null)
				});
				this.gridBrands.IsVisible = false;
				await ViewExtensions.FadeTo(this.gridBrands, 1.0, 0U, null);
				this.activityFrame.IsVisible = false;
				this.btnBack.IsEnabled = true;
			}
		}

		// Token: 0x06001A9C RID: 6812 RVA: 0x001257A8 File Offset: 0x001239A8
		public async void GoBack()
		{
			if (this.CurrentStep == ProfileSelectorV3.Step.BrandSelection)
			{
				EventHandler<bool> windowCloseRequested = this.WindowCloseRequested;
				if (windowCloseRequested != null)
				{
					windowCloseRequested(this, false);
				}
			}
			else
			{
				this.gridBrands.IsVisible = true;
				await ViewExtensions.FadeTo(this.gridBrands, 0.0, 0U, null);
				await Task.WhenAll<bool>(new Task<bool>[]
				{
					ViewExtensions.FadeTo(this.gridProfiles, 0.0, 200U, null),
					ViewExtensions.FadeTo(this.gridBrands, 1.0, 200U, null)
				});
				this.gridProfiles.IsVisible = false;
				this.searchBarProfiles.Text = "";
				this.TitleText = Translate.GetString("ios_ChooseCarBrand");
				try
				{
					RadioCell.SetSelectedValue(this.lvBrandsSection, SharedSettings.Current.SelectedBrand);
				}
				catch (Exception)
				{
				}
			}
		}

		// Token: 0x06001A9D RID: 6813 RVA: 0x001257E0 File Offset: 0x001239E0
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

		// Token: 0x06001A9E RID: 6814 RVA: 0x0012582B File Offset: 0x00123A2B
		private void BtnBack_Clicked(object sender, EventArgs e)
		{
			this.GoBack();
		}

		// Token: 0x06001A9F RID: 6815 RVA: 0x00125834 File Offset: 0x00123A34
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
						if (this.CurrentStep == ProfileSelectorV3.Step.ProfileSelection)
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
							this.lvProfilesSection.ItemsSource = profilesForBrandWithFilter;
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

		// Token: 0x06001AA0 RID: 6816 RVA: 0x00125874 File Offset: 0x00123A74
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
						if (this.CurrentStep == ProfileSelectorV3.Step.BrandSelection)
						{
							ObservableCollection<string> brandsWithFilter = this.Model.GetBrandsWithFilter(filtertext);
							this.gridBrands.BindingContext = brandsWithFilter;
							this.lvBrandsSection.ItemsSource = brandsWithFilter;
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

		// Token: 0x06001AA1 RID: 6817 RVA: 0x001258B4 File Offset: 0x00123AB4
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private void InitializeComponent()
		{
			if (ResourceLoader.IsEnabled && ResourceLoader.CanProvideContentFor(new ResourceLoader.ResourceLoadingQuery
			{
				AssemblyName = typeof(ProfileSelectorV3).GetTypeInfo().Assembly.GetName(),
				ResourcePath = "Settings/ProfileSelectorV3.xaml",
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
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension = new DynamicResourceExtension(), new Uri("Settings\\ProfileSelectorV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 10, 5);
			RowDefinition rowDefinition;
			VisualDiagnostics.RegisterSourceInfo(rowDefinition = new RowDefinition(), new Uri("Settings\\ProfileSelectorV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 17, 18);
			RowDefinition rowDefinition2;
			VisualDiagnostics.RegisterSourceInfo(rowDefinition2 = new RowDefinition(), new Uri("Settings\\ProfileSelectorV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 18, 18);
			RowDefinition rowDefinition3;
			VisualDiagnostics.RegisterSourceInfo(rowDefinition3 = new RowDefinition(), new Uri("Settings\\ProfileSelectorV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 19, 18);
			RowDefinition rowDefinition4;
			VisualDiagnostics.RegisterSourceInfo(rowDefinition4 = new RowDefinition(), new Uri("Settings\\ProfileSelectorV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 20, 18);
			Label label;
			VisualDiagnostics.RegisterSourceInfo(label = new Label(), new Uri("Settings\\ProfileSelectorV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 22, 14);
			RowDefinition rowDefinition5;
			VisualDiagnostics.RegisterSourceInfo(rowDefinition5 = new RowDefinition(), new Uri("Settings\\ProfileSelectorV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 37, 22);
			RowDefinition rowDefinition6;
			VisualDiagnostics.RegisterSourceInfo(rowDefinition6 = new RowDefinition(), new Uri("Settings\\ProfileSelectorV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 38, 22);
			Entry entry;
			VisualDiagnostics.RegisterSourceInfo(entry = new Entry(), new Uri("Settings\\ProfileSelectorV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 40, 18);
			SharedSettings sharedSettings;
			VisualDiagnostics.RegisterSourceInfo(sharedSettings = SharedSettings.Current, new Uri("Settings\\ProfileSelectorV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 55, 25);
			BindingExtension bindingExtension;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension = new BindingExtension(), new Uri("Settings\\ProfileSelectorV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 55, 25);
			BindingExtension bindingExtension2;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension2 = new BindingExtension(), new Uri("Settings\\ProfileSelectorV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 56, 25);
			DataTemplate dataTemplate;
			VisualDiagnostics.RegisterSourceInfo(dataTemplate = new DataTemplate(), new Uri("Settings\\ProfileSelectorV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 58, 30);
			Section section;
			VisualDiagnostics.RegisterSourceInfo(section = new Section(), new Uri("Settings\\ProfileSelectorV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 53, 22);
			SettingsView settingsView;
			VisualDiagnostics.RegisterSourceInfo(settingsView = new SettingsView(), new Uri("Settings\\ProfileSelectorV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 48, 18);
			Grid grid;
			VisualDiagnostics.RegisterSourceInfo(grid = new Grid(), new Uri("Settings\\ProfileSelectorV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 31, 14);
			RowDefinition rowDefinition7;
			VisualDiagnostics.RegisterSourceInfo(rowDefinition7 = new RowDefinition(), new Uri("Settings\\ProfileSelectorV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 91, 22);
			RowDefinition rowDefinition8;
			VisualDiagnostics.RegisterSourceInfo(rowDefinition8 = new RowDefinition(), new Uri("Settings\\ProfileSelectorV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 92, 22);
			RowDefinition rowDefinition9;
			VisualDiagnostics.RegisterSourceInfo(rowDefinition9 = new RowDefinition(), new Uri("Settings\\ProfileSelectorV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 93, 22);
			RowDefinition rowDefinition10;
			VisualDiagnostics.RegisterSourceInfo(rowDefinition10 = new RowDefinition(), new Uri("Settings\\ProfileSelectorV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 94, 22);
			BindingExtension bindingExtension3;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension3 = new BindingExtension(), new Uri("Settings\\ProfileSelectorV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 102, 21);
			Label label2;
			VisualDiagnostics.RegisterSourceInfo(label2 = new Label(), new Uri("Settings\\ProfileSelectorV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 96, 18);
			Entry entry2;
			VisualDiagnostics.RegisterSourceInfo(entry2 = new Entry(), new Uri("Settings\\ProfileSelectorV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 103, 18);
			BindingExtension bindingExtension4;
			VisualDiagnostics.RegisterSourceInfo(bindingExtension4 = new BindingExtension(), new Uri("Settings\\ProfileSelectorV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 114, 60);
			DataTemplate dataTemplate2;
			VisualDiagnostics.RegisterSourceInfo(dataTemplate2 = new DataTemplate(), new Uri("Settings\\ProfileSelectorV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 116, 30);
			Section section2;
			VisualDiagnostics.RegisterSourceInfo(section2 = new Section(), new Uri("Settings\\ProfileSelectorV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 114, 22);
			SettingsView settingsView2;
			VisualDiagnostics.RegisterSourceInfo(settingsView2 = new SettingsView(), new Uri("Settings\\ProfileSelectorV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 110, 18);
			DynamicResourceExtension dynamicResourceExtension2;
			VisualDiagnostics.RegisterSourceInfo(dynamicResourceExtension2 = new DynamicResourceExtension(), new Uri("Settings\\ProfileSelectorV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 131, 21);
			Translate translate;
			VisualDiagnostics.RegisterSourceInfo(translate = new Translate(), new Uri("Settings\\ProfileSelectorV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 134, 21);
			Label label3;
			VisualDiagnostics.RegisterSourceInfo(label3 = new Label(), new Uri("Settings\\ProfileSelectorV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 129, 18);
			Grid grid2;
			VisualDiagnostics.RegisterSourceInfo(grid2 = new Grid(), new Uri("Settings\\ProfileSelectorV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 85, 14);
			ActivityFrame activityFrame;
			VisualDiagnostics.RegisterSourceInfo(activityFrame = new ActivityFrame(), new Uri("Settings\\ProfileSelectorV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 138, 14);
			Translate translate2;
			VisualDiagnostics.RegisterSourceInfo(translate2 = new Translate(), new Uri("Settings\\ProfileSelectorV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 150, 17);
			Button button;
			VisualDiagnostics.RegisterSourceInfo(button = new Button(), new Uri("Settings\\ProfileSelectorV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 145, 14);
			Grid grid3;
			VisualDiagnostics.RegisterSourceInfo(grid3 = new Grid(), new Uri("Settings\\ProfileSelectorV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 12, 10);
			VisualDiagnostics.RegisterSourceInfo(this, new Uri("Settings\\ProfileSelectorV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 2, 2);
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
			nameScope.RegisterName("lvBrandsSettings", settingsView);
			if (settingsView.StyleId == null)
			{
				settingsView.StyleId = "lvBrandsSettings";
			}
			nameScope.RegisterName("lvBrandsSection", section);
			if (section.StyleId == null)
			{
				section.StyleId = "lvBrandsSection";
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
			nameScope.RegisterName("lvProfilesSettings", settingsView2);
			if (settingsView2.StyleId == null)
			{
				settingsView2.StyleId = "lvProfilesSettings";
			}
			nameScope.RegisterName("lvProfilesSection", section2);
			if (section2.StyleId == null)
			{
				section2.StyleId = "lvProfilesSection";
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
			this.lvBrandsSettings = settingsView;
			this.lvBrandsSection = section;
			this.gridProfiles = grid2;
			this.lbSelectedBrand = label2;
			this.searchBarProfiles = entry2;
			this.lvProfilesSettings = settingsView2;
			this.lvProfilesSection = section2;
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
			xmlNamespaceResolver.Add("obd2", "clr-namespace:CarScannerXamarinForms.OBD2");
			xmlNamespaceResolver.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xamlServiceProvider.Add(typeFromHandle2, new XamlTypeResolver(xmlNamespaceResolver, typeof(ProfileSelectorV3).GetTypeInfo().Assembly));
			xamlServiceProvider.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(10, 5)));
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
			label.SetValue(View.HorizontalOptionsProperty, LayoutOptions.Center);
			label.SetValue(Label.HorizontalTextAlignmentProperty, new TextAlignmentConverter().ConvertFromInvariantString("Center"));
			label.SetValue(VisualElement.IsVisibleProperty, new VisualElement.VisibilityConverter().ConvertFromInvariantString("False"));
			label.SetValue(Label.TextProperty, "TEST TITLE");
			grid3.Children.Add(label);
			grid.SetValue(Grid.RowProperty, 2);
			grid.SetValue(VisualElement.IsVisibleProperty, new VisualElement.VisibilityConverter().ConvertFromInvariantString("True"));
			grid.SetValue(Grid.RowSpacingProperty, 0.0);
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
			settingsView.SetValue(Grid.RowProperty, 1);
			settingsView.SetValue(SettingsView.HeaderHeightProperty, 0.0);
			bindingExtension.Source = sharedSettings;
			bindingExtension.Mode = 2;
			bindingExtension.Path = "SelectedBrand";
			BindingBase bindingBase = bindingExtension.ProvideValue(null);
			section.SetBinding(RadioCell.SelectedValueProperty, bindingBase);
			bindingExtension2.Path = "Brands";
			BindingBase bindingBase2 = bindingExtension2.ProvideValue(null);
			section.SetBinding(Section.ItemsSourceProperty, bindingBase2);
			IDataTemplate dataTemplate3 = dataTemplate;
			ProfileSelectorV3.<InitializeComponent>_anonXamlCDataTemplate_87 <InitializeComponent>_anonXamlCDataTemplate_ = new ProfileSelectorV3.<InitializeComponent>_anonXamlCDataTemplate_87();
			object[] array2 = new object[0 + 6];
			array2[0] = dataTemplate;
			array2[1] = section;
			array2[2] = settingsView;
			array2[3] = grid;
			array2[4] = grid3;
			array2[5] = this;
			<InitializeComponent>_anonXamlCDataTemplate_.parentValues = array2;
			<InitializeComponent>_anonXamlCDataTemplate_.root = this;
			dataTemplate3.LoadTemplate = new Func<object>(<InitializeComponent>_anonXamlCDataTemplate_.LoadDataTemplate);
			section.SetValue(Section.ItemTemplateProperty, dataTemplate);
			settingsView.Root.Add(section);
			grid.Children.Add(settingsView);
			grid3.Children.Add(grid);
			grid2.SetValue(Grid.RowProperty, 2);
			grid2.SetValue(VisualElement.IsVisibleProperty, new VisualElement.VisibilityConverter().ConvertFromInvariantString("False"));
			grid2.SetValue(Grid.RowSpacingProperty, 0.0);
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
			label2.SetValue(View.HorizontalOptionsProperty, LayoutOptions.Center);
			label2.SetValue(Label.HorizontalTextAlignmentProperty, new TextAlignmentConverter().ConvertFromInvariantString("Center"));
			bindingExtension3.Path = "Name";
			BindingBase bindingBase3 = bindingExtension3.ProvideValue(null);
			label2.SetBinding(Label.TextProperty, bindingBase3);
			grid2.Children.Add(label2);
			entry2.SetValue(Grid.RowProperty, 1);
			entry2.Completed += this.EntrySearch_Completed;
			entry2.SetValue(InputView.IsSpellCheckEnabledProperty, false);
			entry2.SetValue(Entry.PlaceholderProperty, "\ud83d\udd0e");
			entry2.TextChanged += this.searchBarProfiles_TextChanged;
			grid2.Children.Add(entry2);
			settingsView2.SetValue(Grid.RowProperty, 2);
			settingsView2.SetValue(TableView.HasUnevenRowsProperty, true);
			bindingExtension4.Mode = 2;
			bindingExtension4.Path = "SelectedProfileV2Name";
			BindingBase bindingBase4 = bindingExtension4.ProvideValue(null);
			section2.SetBinding(RadioCell.SelectedValueProperty, bindingBase4);
			IDataTemplate dataTemplate4 = dataTemplate2;
			ProfileSelectorV3.<InitializeComponent>_anonXamlCDataTemplate_88 <InitializeComponent>_anonXamlCDataTemplate_2 = new ProfileSelectorV3.<InitializeComponent>_anonXamlCDataTemplate_88();
			object[] array3 = new object[0 + 6];
			array3[0] = dataTemplate2;
			array3[1] = section2;
			array3[2] = settingsView2;
			array3[3] = grid2;
			array3[4] = grid3;
			array3[5] = this;
			<InitializeComponent>_anonXamlCDataTemplate_2.parentValues = array3;
			<InitializeComponent>_anonXamlCDataTemplate_2.root = this;
			dataTemplate4.LoadTemplate = new Func<object>(<InitializeComponent>_anonXamlCDataTemplate_2.LoadDataTemplate);
			section2.SetValue(Section.ItemTemplateProperty, dataTemplate2);
			settingsView2.Root.Add(section2);
			grid2.Children.Add(settingsView2);
			label3.SetValue(Grid.RowProperty, 3);
			dynamicResourceExtension2.Key = "BaseFontSize";
			IMarkupExtension<DynamicResource> markupExtension2 = dynamicResourceExtension2;
			XamlServiceProvider xamlServiceProvider2 = new XamlServiceProvider();
			Type typeFromHandle3 = typeof(IProvideValueTarget);
			object[] array4 = new object[0 + 4];
			array4[0] = label3;
			array4[1] = grid2;
			array4[2] = grid3;
			array4[3] = this;
			object obj2;
			xamlServiceProvider2.Add(typeFromHandle3, obj2 = new SimpleValueTargetProvider(array4, Label.FontSizeProperty, nameScope));
			xamlServiceProvider2.Add(typeof(IReferenceProvider), obj2);
			Type typeFromHandle4 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver2 = new XmlNamespaceResolver();
			xmlNamespaceResolver2.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver2.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver2.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver2.Add("obd2", "clr-namespace:CarScannerXamarinForms.OBD2");
			xmlNamespaceResolver2.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver2.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xamlServiceProvider2.Add(typeFromHandle4, new XamlTypeResolver(xmlNamespaceResolver2, typeof(ProfileSelectorV3).GetTypeInfo().Assembly));
			xamlServiceProvider2.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(131, 21)));
			DynamicResource dynamicResource2 = markupExtension2.ProvideValue(xamlServiceProvider2);
			label3.SetDynamicResource(Label.FontSizeProperty, dynamicResource2.Key);
			label3.SetValue(View.HorizontalOptionsProperty, LayoutOptions.Center);
			label3.SetValue(Label.HorizontalTextAlignmentProperty, new TextAlignmentConverter().ConvertFromInvariantString("Center"));
			translate.Text = "ios_DontKnowProfile";
			IMarkupExtension markupExtension3 = translate;
			XamlServiceProvider xamlServiceProvider3 = new XamlServiceProvider();
			Type typeFromHandle5 = typeof(IProvideValueTarget);
			object[] array5 = new object[0 + 4];
			array5[0] = label3;
			array5[1] = grid2;
			array5[2] = grid3;
			array5[3] = this;
			object obj3;
			xamlServiceProvider3.Add(typeFromHandle5, obj3 = new SimpleValueTargetProvider(array5, Label.TextProperty, nameScope));
			xamlServiceProvider3.Add(typeof(IReferenceProvider), obj3);
			Type typeFromHandle6 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver3 = new XmlNamespaceResolver();
			xmlNamespaceResolver3.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver3.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver3.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver3.Add("obd2", "clr-namespace:CarScannerXamarinForms.OBD2");
			xmlNamespaceResolver3.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver3.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xamlServiceProvider3.Add(typeFromHandle6, new XamlTypeResolver(xmlNamespaceResolver3, typeof(ProfileSelectorV3).GetTypeInfo().Assembly));
			xamlServiceProvider3.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(134, 21)));
			object obj4 = markupExtension3.ProvideValue(xamlServiceProvider3);
			label3.Text = obj4;
			label3.SetValue(Label.TextColorProperty, Color.Red);
			grid2.Children.Add(label3);
			grid3.Children.Add(grid2);
			activityFrame.SetValue(Grid.RowProperty, 2);
			activityFrame.SetValue(View.HorizontalOptionsProperty, LayoutOptions.Center);
			activityFrame.SetValue(ActivityFrame.IsCancelVisibleProperty, false);
			activityFrame.SetValue(VisualElement.IsVisibleProperty, new VisualElement.VisibilityConverter().ConvertFromInvariantString("False"));
			activityFrame.SetValue(View.VerticalOptionsProperty, LayoutOptions.Center);
			grid3.Children.Add(activityFrame);
			button.SetValue(Grid.RowProperty, 3);
			button.SetValue(VisualElement.BackgroundColorProperty, Color.Red);
			button.Clicked += this.BtnBack_Clicked;
			translate2.Text = "ios_Back";
			IMarkupExtension markupExtension4 = translate2;
			XamlServiceProvider xamlServiceProvider4 = new XamlServiceProvider();
			Type typeFromHandle7 = typeof(IProvideValueTarget);
			object[] array6 = new object[0 + 3];
			array6[0] = button;
			array6[1] = grid3;
			array6[2] = this;
			object obj5;
			xamlServiceProvider4.Add(typeFromHandle7, obj5 = new SimpleValueTargetProvider(array6, Button.TextProperty, nameScope));
			xamlServiceProvider4.Add(typeof(IReferenceProvider), obj5);
			Type typeFromHandle8 = typeof(IXamlTypeResolver);
			XmlNamespaceResolver xmlNamespaceResolver4 = new XmlNamespaceResolver();
			xmlNamespaceResolver4.Add("", "http://xamarin.com/schemas/2014/forms");
			xmlNamespaceResolver4.Add("x", "http://schemas.microsoft.com/winfx/2009/xaml");
			xmlNamespaceResolver4.Add("local", "clr-namespace:CarScannerXamarinForms");
			xmlNamespaceResolver4.Add("obd2", "clr-namespace:CarScannerXamarinForms.OBD2");
			xmlNamespaceResolver4.Add("shset", "clr-namespace:CarScannerXamarinForms.Settings");
			xmlNamespaceResolver4.Add("sv", "clr-namespace:AiForms.Renderers;assembly=SettingsView");
			xamlServiceProvider4.Add(typeFromHandle8, new XamlTypeResolver(xmlNamespaceResolver4, typeof(ProfileSelectorV3).GetTypeInfo().Assembly));
			xamlServiceProvider4.Add(typeof(IXmlLineInfoProvider), new XmlLineInfoProvider(new XmlLineInfo(150, 17)));
			object obj6 = markupExtension4.ProvideValue(xamlServiceProvider4);
			button.Text = obj6;
			button.SetValue(Button.TextColorProperty, Color.White);
			grid3.Children.Add(button);
			this.SetValue(ContentView.ContentProperty, grid3);
		}

		// Token: 0x06001AA2 RID: 6818 RVA: 0x00126DC0 File Offset: 0x00124FC0
		// Note: this type is marked as 'beforefieldinit'.
		static ProfileSelectorV3()
		{
		}

		// Token: 0x06001AA3 RID: 6819 RVA: 0x00126EA8 File Offset: 0x001250A8
		private void __InitComponentRuntime()
		{
			Extensions.LoadFromXaml<ProfileSelectorV3>(this, typeof(ProfileSelectorV3));
			this.layoutRoot = NameScopeExtensions.FindByName<Grid>(this, "layoutRoot");
			this.lbTitle = NameScopeExtensions.FindByName<Label>(this, "lbTitle");
			this.gridBrands = NameScopeExtensions.FindByName<Grid>(this, "gridBrands");
			this.searchBarBrands = NameScopeExtensions.FindByName<Entry>(this, "searchBarBrands");
			this.lvBrandsSettings = NameScopeExtensions.FindByName<SettingsView>(this, "lvBrandsSettings");
			this.lvBrandsSection = NameScopeExtensions.FindByName<Section>(this, "lvBrandsSection");
			this.gridProfiles = NameScopeExtensions.FindByName<Grid>(this, "gridProfiles");
			this.lbSelectedBrand = NameScopeExtensions.FindByName<Label>(this, "lbSelectedBrand");
			this.searchBarProfiles = NameScopeExtensions.FindByName<Entry>(this, "searchBarProfiles");
			this.lvProfilesSettings = NameScopeExtensions.FindByName<SettingsView>(this, "lvProfilesSettings");
			this.lvProfilesSection = NameScopeExtensions.FindByName<Section>(this, "lvProfilesSection");
			this.activityFrame = NameScopeExtensions.FindByName<ActivityFrame>(this, "activityFrame");
			this.btnBack = NameScopeExtensions.FindByName<Button>(this, "btnBack");
		}

		// Token: 0x04000BBD RID: 3005
		[CompilerGenerated]
		private string <SelectedBrand>k__BackingField;

		// Token: 0x04000BBE RID: 3006
		[CompilerGenerated]
		private EventHandler<bool> WindowCloseRequested;

		// Token: 0x04000BBF RID: 3007
		public static readonly BindableProperty TitleVisibleProperty = BindableProperty.Create("TitleVisible", typeof(bool), typeof(ProfileSelectorV3), null, 2, null, new BindableProperty.BindingPropertyChangedDelegate(ProfileSelectorV3.TitleVisiblePropertyChanged), null, null, null);

		// Token: 0x04000BC0 RID: 3008
		public static readonly BindableProperty BackButtonVisibleProperty = BindableProperty.Create("BackButtonVisible", typeof(bool), typeof(ProfileSelectorV3), null, 2, null, new BindableProperty.BindingPropertyChangedDelegate(ProfileSelectorV3.BackButtonVisiblePropertyChanged), null, null, null);

		// Token: 0x04000BC1 RID: 3009
		public static readonly BindableProperty CreateBackItemProperty = BindableProperty.Create("CreateBackItem", typeof(bool), typeof(ProfileSelectorV3), false, 2, null, new BindableProperty.BindingPropertyChangedDelegate(ProfileSelectorV3.CreateBackItemPropertyChanged), null, null, null);

		// Token: 0x04000BC2 RID: 3010
		public static readonly BindableProperty TitleTextProperty = BindableProperty.Create("TitleText", typeof(string), typeof(ProfileSelectorV3), null, 2, null, new BindableProperty.BindingPropertyChangedDelegate(ProfileSelectorV3.TitleTextPropertyChanged), null, null, null);

		// Token: 0x04000BC3 RID: 3011
		[CompilerGenerated]
		private ProfileV2Model <Model>k__BackingField;

		// Token: 0x04000BC4 RID: 3012
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private Grid layoutRoot;

		// Token: 0x04000BC5 RID: 3013
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private Label lbTitle;

		// Token: 0x04000BC6 RID: 3014
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private Grid gridBrands;

		// Token: 0x04000BC7 RID: 3015
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private Entry searchBarBrands;

		// Token: 0x04000BC8 RID: 3016
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private SettingsView lvBrandsSettings;

		// Token: 0x04000BC9 RID: 3017
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private Section lvBrandsSection;

		// Token: 0x04000BCA RID: 3018
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private Grid gridProfiles;

		// Token: 0x04000BCB RID: 3019
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private Label lbSelectedBrand;

		// Token: 0x04000BCC RID: 3020
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private Entry searchBarProfiles;

		// Token: 0x04000BCD RID: 3021
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private SettingsView lvProfilesSettings;

		// Token: 0x04000BCE RID: 3022
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private Section lvProfilesSection;

		// Token: 0x04000BCF RID: 3023
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private ActivityFrame activityFrame;

		// Token: 0x04000BD0 RID: 3024
		[GeneratedCode("Xamarin.Forms.Build.Tasks.XamlG", "2.0.0.0")]
		private Button btnBack;

		// Token: 0x0200020C RID: 524
		private enum Step
		{
			// Token: 0x04000BD2 RID: 3026
			BrandSelection,
			// Token: 0x04000BD3 RID: 3027
			ProfileSelection
		}

		// Token: 0x0200020D RID: 525
		[CompilerGenerated]
		private sealed class <>c__DisplayClass40_0
		{
			// Token: 0x06001AA4 RID: 6820 RVA: 0x00002050 File Offset: 0x00000250
			public <>c__DisplayClass40_0()
			{
			}

			// Token: 0x06001AA5 RID: 6821 RVA: 0x00126FA3 File Offset: 0x001251A3
			internal void <ApplyProfile>b__0()
			{
				this.profile.Apply(this.<>4__this.SelectedBrand);
			}

			// Token: 0x04000BD4 RID: 3028
			public OBDReaderProfileV2 profile;

			// Token: 0x04000BD5 RID: 3029
			public ProfileSelectorV3 <>4__this;
		}

		// Token: 0x0200020E RID: 526
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <ApplyProfile>d__40 : IAsyncStateMachine
		{
			// Token: 0x06001AA6 RID: 6822 RVA: 0x00126FBC File Offset: 0x001251BC
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				ProfileSelectorV3 profileSelectorV = this;
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
						CS$<>8__locals1 = new ProfileSelectorV3.<>c__DisplayClass40_0();
						CS$<>8__locals1.profile = profile;
						CS$<>8__locals1.<>4__this = this;
						taskAwaiter5 = App.GetCurrentPage().DisplayAlert(Translate.GetString("ios_ApplyProfileQuestion"), profileSelectorV.SelectedBrand + " " + CS$<>8__locals1.profile.Name, "OK", Translate.GetString("btnCancel.Content")).GetAwaiter();
						if (!taskAwaiter5.IsCompleted)
						{
							num2 = 0;
							taskAwaiter2 = taskAwaiter5;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<bool>, ProfileSelectorV3.<ApplyProfile>d__40>(ref taskAwaiter5, ref this);
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
						this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, ProfileSelectorV3.<ApplyProfile>d__40>(ref taskAwaiter3, ref this);
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

			// Token: 0x06001AA7 RID: 6823 RVA: 0x001271D0 File Offset: 0x001253D0
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x04000BD6 RID: 3030
			public int <>1__state;

			// Token: 0x04000BD7 RID: 3031
			public AsyncTaskMethodBuilder <>t__builder;

			// Token: 0x04000BD8 RID: 3032
			public OBDReaderProfileV2 profile;

			// Token: 0x04000BD9 RID: 3033
			public ProfileSelectorV3 <>4__this;

			// Token: 0x04000BDA RID: 3034
			private ProfileSelectorV3.<>c__DisplayClass40_0 <>8__1;

			// Token: 0x04000BDB RID: 3035
			private TaskAwaiter<bool> <>u__1;

			// Token: 0x04000BDC RID: 3036
			private TaskAwaiter <>u__2;
		}

		// Token: 0x0200020F RID: 527
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <BrandsCell_Tapped>d__1 : IAsyncStateMachine
		{
			// Token: 0x06001AA8 RID: 6824 RVA: 0x001271E0 File Offset: 0x001253E0
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				ProfileSelectorV3 profileSelectorV = this;
				try
				{
					TaskAwaiter taskAwaiter;
					if (num != 0)
					{
						PlatformHelper.CommonService.HideKeyboard();
						string text = (string)((Element)sender).BindingContext;
						profileSelectorV.SelectedBrand = text;
						profileSelectorV.OnPropertyChanged("SelectedBrand");
						RadioCell.SetSelectedValue(profileSelectorV.lvBrandsSection, SharedSettings.Current.SelectedBrand);
						taskAwaiter = profileSelectorV.SelectBrand(text).GetAwaiter();
						if (!taskAwaiter.IsCompleted)
						{
							num2 = 0;
							TaskAwaiter taskAwaiter2 = taskAwaiter;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, ProfileSelectorV3.<BrandsCell_Tapped>d__1>(ref taskAwaiter, ref this);
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
				num2 = -2;
				this.<>t__builder.SetResult();
			}

			// Token: 0x06001AA9 RID: 6825 RVA: 0x001272DC File Offset: 0x001254DC
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x04000BDD RID: 3037
			public int <>1__state;

			// Token: 0x04000BDE RID: 3038
			public AsyncVoidMethodBuilder <>t__builder;

			// Token: 0x04000BDF RID: 3039
			public object sender;

			// Token: 0x04000BE0 RID: 3040
			public ProfileSelectorV3 <>4__this;

			// Token: 0x04000BE1 RID: 3041
			private TaskAwaiter <>u__1;
		}

		// Token: 0x02000210 RID: 528
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <GoBack>d__39 : IAsyncStateMachine
		{
			// Token: 0x06001AAA RID: 6826 RVA: 0x001272EC File Offset: 0x001254EC
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				ProfileSelectorV3 profileSelectorV = this;
				try
				{
					TaskAwaiter<bool[]> taskAwaiter;
					TaskAwaiter<bool> taskAwaiter3;
					if (num != 0)
					{
						if (num == 1)
						{
							TaskAwaiter<bool[]> taskAwaiter2;
							taskAwaiter = taskAwaiter2;
							taskAwaiter2 = default(TaskAwaiter<bool[]>);
							num2 = -1;
							goto IL_0146;
						}
						if (profileSelectorV.CurrentStep == ProfileSelectorV3.Step.BrandSelection)
						{
							EventHandler<bool> windowCloseRequested = profileSelectorV.WindowCloseRequested;
							if (windowCloseRequested == null)
							{
								goto IL_0194;
							}
							windowCloseRequested(profileSelectorV, false);
							goto IL_0194;
						}
						else
						{
							profileSelectorV.gridBrands.IsVisible = true;
							taskAwaiter3 = ViewExtensions.FadeTo(profileSelectorV.gridBrands, 0.0, 0U, null).GetAwaiter();
							if (!taskAwaiter3.IsCompleted)
							{
								num2 = 0;
								TaskAwaiter<bool> taskAwaiter4 = taskAwaiter3;
								this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<bool>, ProfileSelectorV3.<GoBack>d__39>(ref taskAwaiter3, ref this);
								return;
							}
						}
					}
					else
					{
						TaskAwaiter<bool> taskAwaiter4;
						taskAwaiter3 = taskAwaiter4;
						taskAwaiter4 = default(TaskAwaiter<bool>);
						num2 = -1;
					}
					taskAwaiter3.GetResult();
					taskAwaiter = Task.WhenAll<bool>(new Task<bool>[]
					{
						ViewExtensions.FadeTo(profileSelectorV.gridProfiles, 0.0, 200U, null),
						ViewExtensions.FadeTo(profileSelectorV.gridBrands, 1.0, 200U, null)
					}).GetAwaiter();
					if (!taskAwaiter.IsCompleted)
					{
						num2 = 1;
						TaskAwaiter<bool[]> taskAwaiter2 = taskAwaiter;
						this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<bool[]>, ProfileSelectorV3.<GoBack>d__39>(ref taskAwaiter, ref this);
						return;
					}
					IL_0146:
					taskAwaiter.GetResult();
					profileSelectorV.gridProfiles.IsVisible = false;
					profileSelectorV.searchBarProfiles.Text = "";
					profileSelectorV.TitleText = Translate.GetString("ios_ChooseCarBrand");
					try
					{
						RadioCell.SetSelectedValue(profileSelectorV.lvBrandsSection, SharedSettings.Current.SelectedBrand);
					}
					catch (Exception)
					{
					}
					IL_0194:;
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

			// Token: 0x06001AAB RID: 6827 RVA: 0x001274F0 File Offset: 0x001256F0
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x04000BE2 RID: 3042
			public int <>1__state;

			// Token: 0x04000BE3 RID: 3043
			public AsyncVoidMethodBuilder <>t__builder;

			// Token: 0x04000BE4 RID: 3044
			public ProfileSelectorV3 <>4__this;

			// Token: 0x04000BE5 RID: 3045
			private TaskAwaiter<bool> <>u__1;

			// Token: 0x04000BE6 RID: 3046
			private TaskAwaiter<bool[]> <>u__2;
		}

		// Token: 0x02000211 RID: 529
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <ProfileCell_Tapped>d__2 : IAsyncStateMachine
		{
			// Token: 0x06001AAC RID: 6828 RVA: 0x00127500 File Offset: 0x00125700
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				ProfileSelectorV3 profileSelectorV = this;
				try
				{
					TaskAwaiter taskAwaiter;
					if (num != 0)
					{
						PlatformHelper.CommonService.HideKeyboard();
						OBDReaderProfileV2 obdreaderProfileV = (OBDReaderProfileV2)((Element)sender).BindingContext;
						int num3 = (profileSelectorV.lvProfilesSection.ItemsSource as BrandCollection).IndexOf(obdreaderProfileV);
						if (profileSelectorV.CreateBackItem && num3 == 0)
						{
							profileSelectorV.GoBack();
							goto IL_00BA;
						}
						taskAwaiter = profileSelectorV.ApplyProfile(obdreaderProfileV).GetAwaiter();
						if (!taskAwaiter.IsCompleted)
						{
							num2 = 0;
							TaskAwaiter taskAwaiter2 = taskAwaiter;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, ProfileSelectorV3.<ProfileCell_Tapped>d__2>(ref taskAwaiter, ref this);
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
					IL_00BA:;
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

			// Token: 0x06001AAD RID: 6829 RVA: 0x00127608 File Offset: 0x00125808
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x04000BE7 RID: 3047
			public int <>1__state;

			// Token: 0x04000BE8 RID: 3048
			public AsyncVoidMethodBuilder <>t__builder;

			// Token: 0x04000BE9 RID: 3049
			public object sender;

			// Token: 0x04000BEA RID: 3050
			public ProfileSelectorV3 <>4__this;

			// Token: 0x04000BEB RID: 3051
			private TaskAwaiter <>u__1;
		}

		// Token: 0x02000212 RID: 530
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <SelectBrand>d__38 : IAsyncStateMachine
		{
			// Token: 0x06001AAE RID: 6830 RVA: 0x00127618 File Offset: 0x00125818
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				ProfileSelectorV3 profileSelectorV = this;
				try
				{
					TaskAwaiter taskAwaiter;
					TaskAwaiter<bool> taskAwaiter3;
					TaskAwaiter<bool[]> taskAwaiter5;
					switch (num)
					{
					case 0:
					{
						TaskAwaiter taskAwaiter2;
						taskAwaiter = taskAwaiter2;
						taskAwaiter2 = default(TaskAwaiter);
						num2 = -1;
						break;
					}
					case 1:
					{
						TaskAwaiter taskAwaiter2;
						taskAwaiter = taskAwaiter2;
						taskAwaiter2 = default(TaskAwaiter);
						num2 = -1;
						goto IL_0169;
					}
					case 2:
					{
						TaskAwaiter<bool> taskAwaiter4;
						taskAwaiter3 = taskAwaiter4;
						taskAwaiter4 = default(TaskAwaiter<bool>);
						num2 = -1;
						goto IL_027F;
					}
					case 3:
					{
						TaskAwaiter<bool[]> taskAwaiter6;
						taskAwaiter5 = taskAwaiter6;
						taskAwaiter6 = default(TaskAwaiter<bool[]>);
						num2 = -1;
						goto IL_031D;
					}
					case 4:
					{
						TaskAwaiter<bool> taskAwaiter4;
						taskAwaiter3 = taskAwaiter4;
						taskAwaiter4 = default(TaskAwaiter<bool>);
						num2 = -1;
						goto IL_0395;
					}
					default:
						profileSelectorV.gridBrands.IsEnabled = false;
						profileSelectorV.btnBack.IsEnabled = false;
						profileSelectorV.activityFrame.IsVisible = true;
						if (profileSelectorV.Model.ProfilesLoaded)
						{
							goto IL_00B9;
						}
						taskAwaiter = profileSelectorV.Model.LoadProfilesAsync().GetAwaiter();
						if (!taskAwaiter.IsCompleted)
						{
							num2 = 0;
							TaskAwaiter taskAwaiter2 = taskAwaiter;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, ProfileSelectorV3.<SelectBrand>d__38>(ref taskAwaiter, ref this);
							return;
						}
						break;
					}
					taskAwaiter.GetResult();
					IL_00B9:
					BrandCollection profilesForBrandWithFilter = profileSelectorV.Model.GetProfilesForBrandWithFilter(brand, "");
					if (profilesForBrandWithFilter.Count == 1 && profilesForBrandWithFilter[0].Name.Contains("OBD"))
					{
						profileSelectorV.SelectedBrand = brand;
						profileSelectorV.OnPropertyChanged("SelectedBrand");
						taskAwaiter = profileSelectorV.ApplyProfile(profilesForBrandWithFilter[0]).GetAwaiter();
						if (!taskAwaiter.IsCompleted)
						{
							num2 = 1;
							TaskAwaiter taskAwaiter2 = taskAwaiter;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, ProfileSelectorV3.<SelectBrand>d__38>(ref taskAwaiter, ref this);
							return;
						}
					}
					else
					{
						if (profileSelectorV.CreateBackItem)
						{
							profilesForBrandWithFilter.Insert(0, new OBDReaderProfileV2
							{
								Name = "[ " + Translate.GetString("ios_Back2") + " ]"
							});
						}
						profileSelectorV.lbSelectedBrand.BindingContext = profilesForBrandWithFilter;
						profileSelectorV.lvProfilesSettings.BindingContext = profilesForBrandWithFilter;
						profileSelectorV.lvProfilesSection.ItemsSource = profilesForBrandWithFilter;
						profileSelectorV.TitleText = Translate.GetString("ios_ConnectionProfile");
						profileSelectorV.gridBrands.IsEnabled = true;
						profileSelectorV.gridProfiles.IsVisible = true;
						taskAwaiter3 = ViewExtensions.FadeTo(profileSelectorV.gridProfiles, 0.0, 0U, null).GetAwaiter();
						if (!taskAwaiter3.IsCompleted)
						{
							num2 = 2;
							TaskAwaiter<bool> taskAwaiter4 = taskAwaiter3;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<bool>, ProfileSelectorV3.<SelectBrand>d__38>(ref taskAwaiter3, ref this);
							return;
						}
						goto IL_027F;
					}
					IL_0169:
					taskAwaiter.GetResult();
					profileSelectorV.gridBrands.IsEnabled = true;
					profileSelectorV.activityFrame.IsVisible = false;
					profileSelectorV.btnBack.IsEnabled = true;
					goto IL_03B5;
					IL_027F:
					taskAwaiter3.GetResult();
					taskAwaiter5 = Task.WhenAll<bool>(new Task<bool>[]
					{
						ViewExtensions.FadeTo(profileSelectorV.gridProfiles, 1.0, 200U, null),
						ViewExtensions.FadeTo(profileSelectorV.gridBrands, 0.0, 200U, null)
					}).GetAwaiter();
					if (!taskAwaiter5.IsCompleted)
					{
						num2 = 3;
						TaskAwaiter<bool[]> taskAwaiter6 = taskAwaiter5;
						this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<bool[]>, ProfileSelectorV3.<SelectBrand>d__38>(ref taskAwaiter5, ref this);
						return;
					}
					IL_031D:
					taskAwaiter5.GetResult();
					profileSelectorV.gridBrands.IsVisible = false;
					taskAwaiter3 = ViewExtensions.FadeTo(profileSelectorV.gridBrands, 1.0, 0U, null).GetAwaiter();
					if (!taskAwaiter3.IsCompleted)
					{
						num2 = 4;
						TaskAwaiter<bool> taskAwaiter4 = taskAwaiter3;
						this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<bool>, ProfileSelectorV3.<SelectBrand>d__38>(ref taskAwaiter3, ref this);
						return;
					}
					IL_0395:
					taskAwaiter3.GetResult();
					profileSelectorV.activityFrame.IsVisible = false;
					profileSelectorV.btnBack.IsEnabled = true;
					IL_03B5:;
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

			// Token: 0x06001AAF RID: 6831 RVA: 0x00127A24 File Offset: 0x00125C24
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x04000BEC RID: 3052
			public int <>1__state;

			// Token: 0x04000BED RID: 3053
			public AsyncTaskMethodBuilder <>t__builder;

			// Token: 0x04000BEE RID: 3054
			public ProfileSelectorV3 <>4__this;

			// Token: 0x04000BEF RID: 3055
			public string brand;

			// Token: 0x04000BF0 RID: 3056
			private TaskAwaiter <>u__1;

			// Token: 0x04000BF1 RID: 3057
			private TaskAwaiter<bool> <>u__2;

			// Token: 0x04000BF2 RID: 3058
			private TaskAwaiter<bool[]> <>u__3;
		}

		// Token: 0x02000213 RID: 531
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <searchBarBrands_TextChanged>d__43 : IAsyncStateMachine
		{
			// Token: 0x06001AB0 RID: 6832 RVA: 0x00127A34 File Offset: 0x00125C34
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				ProfileSelectorV3 profileSelectorV = this;
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
								this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, ProfileSelectorV3.<searchBarBrands_TextChanged>d__43>(ref taskAwaiter, ref this);
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
								if (profileSelectorV.CurrentStep == ProfileSelectorV3.Step.BrandSelection)
								{
									ObservableCollection<string> brandsWithFilter = profileSelectorV.Model.GetBrandsWithFilter(filtertext);
									profileSelectorV.gridBrands.BindingContext = brandsWithFilter;
									profileSelectorV.lvBrandsSection.ItemsSource = brandsWithFilter;
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

			// Token: 0x06001AB1 RID: 6833 RVA: 0x00127B90 File Offset: 0x00125D90
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x04000BF3 RID: 3059
			public int <>1__state;

			// Token: 0x04000BF4 RID: 3060
			public AsyncVoidMethodBuilder <>t__builder;

			// Token: 0x04000BF5 RID: 3061
			public object sender;

			// Token: 0x04000BF6 RID: 3062
			public ProfileSelectorV3 <>4__this;

			// Token: 0x04000BF7 RID: 3063
			private Entry <searchBar>5__2;

			// Token: 0x04000BF8 RID: 3064
			private string <filtertext>5__3;

			// Token: 0x04000BF9 RID: 3065
			private TaskAwaiter <>u__1;
		}

		// Token: 0x02000214 RID: 532
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <searchBarProfiles_TextChanged>d__42 : IAsyncStateMachine
		{
			// Token: 0x06001AB2 RID: 6834 RVA: 0x00127BA0 File Offset: 0x00125DA0
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				ProfileSelectorV3 profileSelectorV = this;
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
								this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, ProfileSelectorV3.<searchBarProfiles_TextChanged>d__42>(ref taskAwaiter, ref this);
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
								if (profileSelectorV.CurrentStep == ProfileSelectorV3.Step.ProfileSelection)
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
									profileSelectorV.lvProfilesSection.ItemsSource = profilesForBrandWithFilter;
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

			// Token: 0x06001AB3 RID: 6835 RVA: 0x00127D58 File Offset: 0x00125F58
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x04000BFA RID: 3066
			public int <>1__state;

			// Token: 0x04000BFB RID: 3067
			public AsyncVoidMethodBuilder <>t__builder;

			// Token: 0x04000BFC RID: 3068
			public object sender;

			// Token: 0x04000BFD RID: 3069
			public ProfileSelectorV3 <>4__this;

			// Token: 0x04000BFE RID: 3070
			private Entry <searchBar>5__2;

			// Token: 0x04000BFF RID: 3071
			private string <filtertext>5__3;

			// Token: 0x04000C00 RID: 3072
			private TaskAwaiter <>u__1;
		}

		// Token: 0x02000215 RID: 533
		[CompilerGenerated]
		private sealed class <InitializeComponent>_anonXamlCDataTemplate_87
		{
			// Token: 0x06001AB4 RID: 6836 RVA: 0x00127D68 File Offset: 0x00125F68
			public <InitializeComponent>_anonXamlCDataTemplate_87()
			{
			}

			// Token: 0x06001AB5 RID: 6837 RVA: 0x00127D7C File Offset: 0x00125F7C
			internal object LoadDataTemplate()
			{
				BindingExtension bindingExtension;
				VisualDiagnostics.RegisterSourceInfo(bindingExtension = new BindingExtension(), new Uri("Settings\\ProfileSelectorV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 60, 37);
				BindingExtension bindingExtension2;
				VisualDiagnostics.RegisterSourceInfo(bindingExtension2 = new BindingExtension(), new Uri("Settings\\ProfileSelectorV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 62, 37);
				RadioCell radioCell;
				VisualDiagnostics.RegisterSourceInfo(radioCell = new RadioCell(), new Uri("Settings\\ProfileSelectorV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 59, 34);
				NameScope nameScope = new NameScope();
				NameScope.SetNameScope(radioCell, nameScope);
				bindingExtension.Path = ".";
				BindingBase bindingBase = bindingExtension.ProvideValue(null);
				radioCell.SetBinding(CellBase.TitleProperty, bindingBase);
				radioCell.Tapped += this.root.BrandsCell_Tapped;
				bindingExtension2.Path = ".";
				BindingBase bindingBase2 = bindingExtension2.ProvideValue(null);
				radioCell.SetBinding(RadioCell.ValueProperty, bindingBase2);
				return radioCell;
			}

			// Token: 0x04000C01 RID: 3073
			internal object[] parentValues;

			// Token: 0x04000C02 RID: 3074
			internal ProfileSelectorV3 root;
		}

		// Token: 0x02000216 RID: 534
		[CompilerGenerated]
		private sealed class <InitializeComponent>_anonXamlCDataTemplate_88
		{
			// Token: 0x06001AB6 RID: 6838 RVA: 0x00127E70 File Offset: 0x00126070
			public <InitializeComponent>_anonXamlCDataTemplate_88()
			{
			}

			// Token: 0x06001AB7 RID: 6839 RVA: 0x00127E84 File Offset: 0x00126084
			internal object LoadDataTemplate()
			{
				BindingExtension bindingExtension;
				VisualDiagnostics.RegisterSourceInfo(bindingExtension = new BindingExtension(), new Uri("Settings\\ProfileSelectorV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 118, 37);
				BindingExtension bindingExtension2;
				VisualDiagnostics.RegisterSourceInfo(bindingExtension2 = new BindingExtension(), new Uri("Settings\\ProfileSelectorV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 119, 37);
				BindingExtension bindingExtension3;
				VisualDiagnostics.RegisterSourceInfo(bindingExtension3 = new BindingExtension(), new Uri("Settings\\ProfileSelectorV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 121, 37);
				RadioCell radioCell;
				VisualDiagnostics.RegisterSourceInfo(radioCell = new RadioCell(), new Uri("Settings\\ProfileSelectorV3.xaml" + ";assembly=" + "CarScannerXamarinForms", UriKind.RelativeOrAbsolute), 117, 34);
				NameScope nameScope = new NameScope();
				NameScope.SetNameScope(radioCell, nameScope);
				bindingExtension.Path = "Name";
				BindingBase bindingBase = bindingExtension.ProvideValue(null);
				radioCell.SetBinding(CellBase.TitleProperty, bindingBase);
				bindingExtension2.Path = "Description";
				BindingBase bindingBase2 = bindingExtension2.ProvideValue(null);
				radioCell.SetBinding(CellBase.DescriptionProperty, bindingBase2);
				radioCell.Tapped += this.root.ProfileCell_Tapped;
				bindingExtension3.Path = "Name";
				BindingBase bindingBase3 = bindingExtension3.ProvideValue(null);
				radioCell.SetBinding(RadioCell.ValueProperty, bindingBase3);
				return radioCell;
			}

			// Token: 0x04000C03 RID: 3075
			internal object[] parentValues;

			// Token: 0x04000C04 RID: 3076
			internal ProfileSelectorV3 root;
		}
	}
}
