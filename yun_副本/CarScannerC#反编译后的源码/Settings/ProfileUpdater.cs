using System;
using System.Linq;
using CarScannerXamarinForms.Common;
using CarScannerXamarinForms.ProfilesV2;
using CarScannerXamarinForms.ViewModels;

namespace CarScannerXamarinForms.Settings
{
	// Token: 0x020001F0 RID: 496
	public static class ProfileUpdater
	{
		// Token: 0x06001A00 RID: 6656 RVA: 0x00112E9C File Offset: 0x0011109C
		public static bool CheckRequiresUpdate()
		{
			if (SharedSettings.Current.ForceProfileUpdateScheduled)
			{
				return true;
			}
			string latestVersion = SharedSettings.Current.LatestVersion;
			return !(App.Version == latestVersion);
		}

		// Token: 0x06001A01 RID: 6657 RVA: 0x00112ED4 File Offset: 0x001110D4
		public static void PerformUpdate()
		{
			string latestVersion = SharedSettings.Current.LatestVersion;
			if (VersionChecker.IsVersionNewer(App.Version, latestVersion) || SharedSettings.Current.ForceProfileUpdateScheduled)
			{
				ProfileUpdater.ForceUpdate();
			}
		}

		// Token: 0x06001A02 RID: 6658 RVA: 0x00112F0C File Offset: 0x0011110C
		public static void ForceUpdate()
		{
			try
			{
				ProfileV2Model profileV2Model = new ProfileV2Model();
				profileV2Model.LoadProfiles(true);
				if (!string.IsNullOrEmpty(SharedSettings.Current.ProfileUpdateAlias))
				{
					OBDReaderProfileV2 profileForUpdate = profileV2Model.GetProfileForUpdate(SharedSettings.Current.ProfileUpdateAlias);
					if (profileForUpdate != null)
					{
						ProfileUpdater.ApplyProfileAsUpdate(profileForUpdate);
					}
				}
				else
				{
					OBDReaderProfileV2 profileForUpdate2 = profileV2Model.GetProfileForUpdate(SharedSettings.Current.SelectedProfileName);
					if (profileForUpdate2 != null)
					{
						if (profileForUpdate2.Brands.Count == 1)
						{
							SharedSettings.Current.SelectedBrand = profileForUpdate2.Brands.First<string>();
							SharedSettings.Current.SelectedProfileV2Name = profileForUpdate2.Name;
							SharedSettings.Current.SelectedProfileName = "";
							SharedSettings.Current.ProfileUpdateAlias = profileForUpdate2.UpdateAliases[0];
						}
						ProfileUpdater.ApplyProfileAsUpdate(profileForUpdate2);
					}
				}
			}
			catch (Exception)
			{
			}
			finally
			{
				SharedSettings.Current.LatestVersion = App.Version;
				SharedSettings.Current.ForceProfileUpdateScheduled = false;
			}
		}

		// Token: 0x06001A03 RID: 6659 RVA: 0x00113008 File Offset: 0x00111208
		private static void ApplyProfileAsUpdate(OBDReaderProfileV2 profile)
		{
			OBDReaderProfileV2.ImportProfilePids(profile);
			SharedSettings.Current.DTCReadingModeV2 = profile.DTCReadingModeV2;
			SharedSettings.Current.DTCReadingSequence = profile.DTCReadingSequence;
			SharedSettings.Current.DTCClearingModeV2 = profile.DTCClearingModeV2;
			SharedSettings.Current.DTCClearingSequence = profile.DTCClearingSequence;
			SharedSettings.Current.ForceOnlyOneProtocol = profile.ForceOnlyOneProtocol;
			SharedSettings.Current.RequestECUInfo = profile.RequestECUInfo;
			SharedSettings.Current.Mode01Prefix = profile.Mode01Prefix;
			SharedSettings.Current.ResponseMarkerLength = profile.ResponseMarkerLength;
			SharedSettings.Current.BrandForDTC = profile.BrandForDTC;
			SharedSettings.Current.ProfileHasPids = profile.ProfileHasPids;
			if (SharedSettings.Current.ToyotaJDMMode21)
			{
				SharedSettings.Current.Mode01Prefix = "21";
			}
			SharedSettings.Current.SelectedProfileName = profile.Name;
			SharedSettings.Current.SelectedProfileV2Name = profile.Name;
			CustomPIDViewModel.CurrentProfile.Save();
			CustomPIDViewModel.Reload();
			SharedSettings.Current.ProfileUpdateAlias = profile.UpdateAliases[0];
		}
	}
}
