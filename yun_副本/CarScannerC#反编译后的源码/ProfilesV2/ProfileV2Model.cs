using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using CarScannerXamarinForms.Common;
using CarScannerXamarinForms.Settings;
using Newtonsoft.Json;
using Newtonsoft.Json.Bson;
using Xamarin.Essentials;

namespace CarScannerXamarinForms.ProfilesV2
{
	// Token: 0x020002CE RID: 718
	public class ProfileV2Model
	{
		// Token: 0x0600229D RID: 8861 RVA: 0x001AC37A File Offset: 0x001AA57A
		public ProfileV2Model()
		{
			this.LoadBrands();
		}

		// Token: 0x17001104 RID: 4356
		// (get) Token: 0x0600229E RID: 8862 RVA: 0x001AC39E File Offset: 0x001AA59E
		// (set) Token: 0x0600229F RID: 8863 RVA: 0x001AC3A6 File Offset: 0x001AA5A6
		public bool ProfilesLoaded
		{
			[CompilerGenerated]
			get
			{
				return this.<ProfilesLoaded>k__BackingField;
			}
			[CompilerGenerated]
			private set
			{
				this.<ProfilesLoaded>k__BackingField = value;
			}
		}

		// Token: 0x17001105 RID: 4357
		// (get) Token: 0x060022A0 RID: 8864 RVA: 0x001AC3AF File Offset: 0x001AA5AF
		// (set) Token: 0x060022A1 RID: 8865 RVA: 0x001AC3B7 File Offset: 0x001AA5B7
		public bool BrandsLoaded
		{
			[CompilerGenerated]
			get
			{
				return this.<BrandsLoaded>k__BackingField;
			}
			[CompilerGenerated]
			private set
			{
				this.<BrandsLoaded>k__BackingField = value;
			}
		}

		// Token: 0x17001106 RID: 4358
		// (get) Token: 0x060022A2 RID: 8866 RVA: 0x001AC3C0 File Offset: 0x001AA5C0
		// (set) Token: 0x060022A3 RID: 8867 RVA: 0x001AC3C8 File Offset: 0x001AA5C8
		public ObservableCollection<string> Brands
		{
			[CompilerGenerated]
			get
			{
				return this.<Brands>k__BackingField;
			}
			[CompilerGenerated]
			private set
			{
				this.<Brands>k__BackingField = value;
			}
		} = new ObservableCollection<string>();

		// Token: 0x060022A4 RID: 8868 RVA: 0x001AC3D4 File Offset: 0x001AA5D4
		private void LoadBrands()
		{
			try
			{
				if (VersionChecker.IsVersionNewer(ProfileV2Model.GetUpdateVersion(), ProfileV2Model.GetInternalVersion()))
				{
					this.LoadBrandsUpdated();
				}
				else
				{
					this.LoadBrandsInternal();
				}
			}
			catch (Exception)
			{
				this.LoadBrandsInternal();
			}
		}

		// Token: 0x060022A5 RID: 8869 RVA: 0x001AC41C File Offset: 0x001AA61C
		private void LoadBrandsUpdated()
		{
			using (StreamReader streamReader = new StreamReader(Path.Combine(FileSystem.CacheDirectory, "upd_brands.db")))
			{
				List<string> list = JsonConvert.DeserializeObject<List<string>>(streamReader.ReadToEnd());
				this.Brands.Clear();
				foreach (string text in list)
				{
					this.Brands.Add(text);
				}
				this.Brands.Add(Translate.GetString("ios_Other"));
				this.BrandsLoaded = true;
			}
		}

		// Token: 0x060022A6 RID: 8870 RVA: 0x001AC4D0 File Offset: 0x001AA6D0
		private void LoadBrandsInternal()
		{
			List<string> list = PackageFileReader.DeserilzeFromEmbeddedFile<List<string>>("brands.db");
			this.Brands.Clear();
			foreach (string text in list)
			{
				this.Brands.Add(text);
			}
			this.Brands.Add(Translate.GetString("ios_Other"));
			this.BrandsLoaded = true;
		}

		// Token: 0x060022A7 RID: 8871 RVA: 0x001AC554 File Offset: 0x001AA754
		public void LoadProfiles(bool updateExperimental = false)
		{
			this.RawProfiles.Clear();
			List<OBDReaderProfileV2> list;
			if (VersionChecker.IsVersionNewer(ProfileV2Model.GetUpdateVersion(), ProfileV2Model.GetInternalVersion()))
			{
				try
				{
					list = this.LoadProfilesUpdated();
					goto IL_0041;
				}
				catch (Exception)
				{
					ProfileV2Model.DeleteUpdated();
					list = this.LoadProfilesInternal();
					this.LoadBrands();
					goto IL_0041;
				}
			}
			list = this.LoadProfilesInternal();
			IL_0041:
			try
			{
				string @string = Translate.GetString("profiles_nowadays");
				foreach (OBDReaderProfileV2 obdreaderProfileV in list)
				{
					if (obdreaderProfileV.Name.IndexOf("n.d.") >= 0)
					{
						obdreaderProfileV.Name = obdreaderProfileV.Name.Replace("n.d.", @string);
					}
				}
			}
			catch (Exception)
			{
			}
			if (updateExperimental || (SharedSettings.Current.DeveloperMode && SharedSettings.Current.ShowExperimental))
			{
				this.RawProfiles.AddRange(list);
			}
			else
			{
				this.RawProfiles.AddRange(list.Where((OBDReaderProfileV2 x) => !x.IsExperimental));
			}
			this.ProfilesLoaded = true;
		}

		// Token: 0x060022A8 RID: 8872 RVA: 0x001AC690 File Offset: 0x001AA890
		private List<OBDReaderProfileV2> LoadProfilesUpdated()
		{
			string text = Path.Combine(FileSystem.CacheDirectory, "upd_profiles.db");
			return PackageFileReader.BinaryDeserializeFromEmbeddedFileUsingCompressedCryptoStreamWithDiff<List<OBDReaderProfileV2>>("profiles7.db", true, text);
		}

		// Token: 0x060022A9 RID: 8873 RVA: 0x001AC6B9 File Offset: 0x001AA8B9
		private List<OBDReaderProfileV2> LoadProfilesInternal()
		{
			return this.BinaryDeserializeFromEmbeddedFileUsingCompressedCryptoStream2<List<OBDReaderProfileV2>>("profiles7.db", true);
		}

		// Token: 0x060022AA RID: 8874 RVA: 0x001AC6C8 File Offset: 0x001AA8C8
		public T BinaryDeserializeFromEmbeddedFileUsingCompressedCryptoStream2<T>(string filename, bool rootIsArray)
		{
			T t;
			using (Stream manifestResourceStream = typeof(ProfileV2Model).GetTypeInfo().Assembly.GetManifestResourceStream("CarScannerXamarinForms." + filename))
			{
				using (MemoryStream memoryStream = new MemoryStream())
				{
					SevenZipHelper.Decompress(manifestResourceStream, memoryStream);
					memoryStream.Seek(0L, SeekOrigin.Begin);
					using (ShiftStream2 shiftStream = new ShiftStream2(memoryStream))
					{
						using (BsonReader bsonReader = new BsonReader(shiftStream))
						{
							try
							{
								bsonReader.ReadRootValueAsArray = rootIsArray;
								t = new JsonSerializer().Deserialize<T>(bsonReader);
							}
							catch (Exception)
							{
								t = default(T);
							}
						}
					}
				}
			}
			return t;
		}

		// Token: 0x060022AB RID: 8875 RVA: 0x001AC7B0 File Offset: 0x001AA9B0
		public async Task LoadProfilesAsync()
		{
			this.RawProfiles.Clear();
			await Task.Run(delegate
			{
				this.LoadProfiles(false);
			});
			this.ProfilesLoaded = true;
		}

		// Token: 0x060022AC RID: 8876 RVA: 0x001AC7F4 File Offset: 0x001AA9F4
		private BrandCollection GetProfilesForBrand(string brand)
		{
			List<OBDReaderProfileV2> list = new List<OBDReaderProfileV2>();
			foreach (OBDReaderProfileV2 obdreaderProfileV in this.RawProfiles)
			{
				if (obdreaderProfileV.Brands.Contains(brand))
				{
					list.Add(obdreaderProfileV);
				}
			}
			if (brand == "Nissan" || brand == "Infiniti")
			{
				list = (from x in list
					orderby x.Name == "OBD-II / EOBD" descending, x.Name.Contains("Consult", StringComparison.InvariantCultureIgnoreCase) descending, x.Name.StartsWith("OBD-II / EOBD") descending, x.Name
					select x).ToList<OBDReaderProfileV2>();
			}
			else if (brand == "Volvo")
			{
				list = (from x in list
					orderby x.Name == "OBD-II / EOBD", x.Name
					select x).ToList<OBDReaderProfileV2>();
			}
			else
			{
				list = (from x in list
					orderby x.Name == "OBD-II / EOBD" descending, x.Name.StartsWith("OBD-II / EOBD") descending, x.Name
					select x).ToList<OBDReaderProfileV2>();
			}
			OBDReaderProfileV2 obdreaderProfileV2 = list.FirstOrDefault((OBDReaderProfileV2 x) => x.Name == "OBD-II / EOBD");
			if (obdreaderProfileV2 != null)
			{
				obdreaderProfileV2.Description = Translate.GetString("Profile_OBD2_Description").Replace("{0}", brand);
			}
			return new BrandCollection(brand, list);
		}

		// Token: 0x060022AD RID: 8877 RVA: 0x001ACA30 File Offset: 0x001AAC30
		public BrandCollection GetProfilesForBrandWithFilter(string brand, string filter)
		{
			if (brand == Translate.GetString("ios_Other"))
			{
				List<OBDReaderProfileV2> list = new List<OBDReaderProfileV2>();
				list.Add(this.RawProfiles.First((OBDReaderProfileV2 x) => x.UpdateAliases.Any((string alias) => alias == "f133a2bb2f1a44de97371e08c70f9ad9")));
				return new BrandCollection(brand, list);
			}
			filter = filter.Trim();
			BrandCollection profilesForBrand = this.GetProfilesForBrand(brand);
			if (string.IsNullOrEmpty(filter))
			{
				return profilesForBrand;
			}
			List<OBDReaderProfileV2> list2 = new List<OBDReaderProfileV2>(profilesForBrand);
			string[] array = filter.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
			for (int i = 0; i < array.Length; i++)
			{
				string word = array[i];
				list2 = list2.Where((OBDReaderProfileV2 x) => (!string.IsNullOrEmpty(x.Name) && x.Name.IndexOf(word, StringComparison.OrdinalIgnoreCase) >= 0) || (!string.IsNullOrEmpty(x.Description) && x.Description.IndexOf(word, StringComparison.OrdinalIgnoreCase) >= 0)).ToList<OBDReaderProfileV2>();
			}
			return new BrandCollection(brand, list2);
		}

		// Token: 0x060022AE RID: 8878 RVA: 0x001ACB00 File Offset: 0x001AAD00
		public ObservableCollection<string> GetBrandsWithFilter(string filter)
		{
			filter = filter.Trim();
			if (string.IsNullOrEmpty(filter))
			{
				return this.Brands;
			}
			List<string> list = new List<string>(this.Brands);
			string[] array = filter.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
			for (int i = 0; i < array.Length; i++)
			{
				string word = array[i];
				list = list.Where((string x) => !string.IsNullOrEmpty(x) && x.IndexOf(word, StringComparison.OrdinalIgnoreCase) >= 0).ToList<string>();
			}
			return new ObservableCollection<string>(list);
		}

		// Token: 0x060022AF RID: 8879 RVA: 0x001ACB80 File Offset: 0x001AAD80
		public OBDReaderProfileV2 GetProfileForUpdate(string updateAlias)
		{
			if (!this.ProfilesLoaded)
			{
				this.LoadProfiles(false);
			}
			Func<string, bool> <>9__1;
			return this.RawProfiles.FirstOrDefault(delegate(OBDReaderProfileV2 x)
			{
				IEnumerable<string> updateAliases = x.UpdateAliases;
				Func<string, bool> func;
				if ((func = <>9__1) == null)
				{
					func = (<>9__1 = (string alias) => alias == updateAlias);
				}
				return updateAliases.Any(func);
			});
		}

		// Token: 0x060022B0 RID: 8880 RVA: 0x001ACBC0 File Offset: 0x001AADC0
		public static string GetCurrentVersion()
		{
			string internalVersion = ProfileV2Model.GetInternalVersion();
			string updateVersion = ProfileV2Model.GetUpdateVersion();
			if (string.IsNullOrEmpty(updateVersion))
			{
				return internalVersion;
			}
			if (VersionChecker.IsVersionNewer(updateVersion, internalVersion))
			{
				return updateVersion;
			}
			return internalVersion;
		}

		// Token: 0x060022B1 RID: 8881 RVA: 0x001ACBEF File Offset: 0x001AADEF
		public static string GetInternalVersion()
		{
			return App.Version;
		}

		// Token: 0x060022B2 RID: 8882 RVA: 0x001ACBF8 File Offset: 0x001AADF8
		public static string GetUpdateVersion()
		{
			try
			{
				string text = Path.Combine(FileSystem.CacheDirectory, "profiles_ver.txt");
				if (File.Exists(text))
				{
					using (StreamReader streamReader = new StreamReader(text))
					{
						string text2 = streamReader.ReadLine();
						if (text2 != null)
						{
							if (text2.Count((char c) => c == '.') == 2)
							{
								return text2;
							}
						}
					}
				}
			}
			catch (Exception)
			{
			}
			return "";
		}

		// Token: 0x060022B3 RID: 8883 RVA: 0x001ACC90 File Offset: 0x001AAE90
		private static async ValueTask SelectBestServer()
		{
			if (!ProfileV2Model.bestUrlSelected)
			{
				Task<string> t = HttpDownloader.Get("https://carscanner.am/profiles/ping.txt", 10);
				Task<string> t2 = HttpDownloader.Get("https://node4.carscanner.info/profiles/ping.txt", 10);
				Task t_delay = Task.Delay(TimeSpan.FromSeconds(15.0));
				Task task = await Task.WhenAny(new Task[] { t, t2, t_delay });
				if (task == t_delay)
				{
					ProfileV2Model.bestUrlSelected = false;
				}
				else if (task == t && t.Result == "ping-pong")
				{
					ProfileV2Model.urlBase = "https://carscanner.am";
					ProfileV2Model.bestUrlSelected = true;
				}
				else if (task == t2 && t2.Result == "ping-pong")
				{
					ProfileV2Model.urlBase = "https://node4.carscanner.info";
					ProfileV2Model.bestUrlSelected = true;
				}
			}
		}

		// Token: 0x060022B4 RID: 8884 RVA: 0x001ACCCC File Offset: 0x001AAECC
		public static async Task<ValueTuple<bool, string>> CheckForOnlineUpdatesAvailables()
		{
			try
			{
				await ProfileV2Model.SelectBestServer();
			}
			catch (Exception)
			{
			}
			using (MemoryStream ms = new MemoryStream())
			{
				CancellationTokenSource cancellationTokenSource = new CancellationTokenSource(TimeSpan.FromSeconds(15.0));
				try
				{
					await HttpDownloader.DownloadFileAsync(ProfileV2Model.urlBase + "/profiles/" + App.Version + "/ver.txt", null, cancellationTokenSource.Token, ms, 65536);
				}
				catch (Exception)
				{
					return new ValueTuple<bool, string>(false, "");
				}
				if (ms.Length > 0L)
				{
					ms.Seek(0L, SeekOrigin.Begin);
					using (StreamReader streamReader = new StreamReader(ms))
					{
						string text = streamReader.ReadLine();
						if (VersionChecker.IsVersionNewer(text, ProfileV2Model.GetCurrentVersion()))
						{
							return new ValueTuple<bool, string>(true, text);
						}
						return new ValueTuple<bool, string>(false, text);
					}
				}
			}
			MemoryStream ms = null;
			return new ValueTuple<bool, string>(false, "");
		}

		// Token: 0x060022B5 RID: 8885 RVA: 0x001ACD08 File Offset: 0x001AAF08
		public static async Task<bool> DownloadUpdate(IProgress<string> progress, CancellationToken cancelToken)
		{
			try
			{
				await ProfileV2Model.SelectBestServer();
			}
			catch (Exception)
			{
			}
			string updateVersion = "";
			long updateSize = -1L;
			using (MemoryStream ms = new MemoryStream())
			{
				CancellationTokenSource cancellationTokenSource = new CancellationTokenSource(TimeSpan.FromSeconds(15.0));
				try
				{
					await HttpDownloader.DownloadFileAsync(ProfileV2Model.urlBase + "/profiles/" + App.Version + "/ver.txt", null, cancellationTokenSource.Token, ms, 65536);
				}
				catch (Exception)
				{
					return false;
				}
				if (ms.Length > 0L)
				{
					ms.Seek(0L, SeekOrigin.Begin);
					using (StreamReader streamReader = new StreamReader(ms))
					{
						updateVersion = streamReader.ReadLine();
						long.TryParse(streamReader.ReadLine(), out updateSize);
					}
				}
			}
			MemoryStream ms = null;
			bool flag;
			if (cancelToken.IsCancellationRequested)
			{
				flag = false;
			}
			else
			{
				if (updateVersion != "" && VersionChecker.IsVersionNewer(updateVersion, ProfileV2Model.GetCurrentVersion()) && updateSize > 0L)
				{
					try
					{
						string brands_temp_file = Path.Combine(FileSystem.CacheDirectory, "temp_brands.db");
						string profiles_temp_file = Path.Combine(FileSystem.CacheDirectory, "temp_profiles.db");
						string ver_temp_file = Path.Combine(FileSystem.CacheDirectory, "temp_ver.txt");
						ProfileV2Model.DeleteTempFiles();
						using (FileStream fstream = File.Create(brands_temp_file))
						{
							try
							{
								if (progress != null)
								{
									progress.Report("Downloading brands");
								}
								CancellationTokenSource cancellationTokenSource2 = new CancellationTokenSource();
								await HttpDownloader.DownloadFileAsync(string.Concat(new string[]
								{
									ProfileV2Model.urlBase,
									"/profiles/",
									App.Version,
									"/",
									updateVersion,
									"_b.db"
								}), null, cancellationTokenSource2.Token, fstream, 65536);
								if (fstream.Length == 0L)
								{
									return false;
								}
							}
							catch (Exception)
							{
								return false;
							}
						}
						FileStream fstream = null;
						try
						{
							using (StreamReader streamReader2 = new StreamReader(brands_temp_file))
							{
								JsonConvert.DeserializeObject<List<string>>(streamReader2.ReadToEnd());
							}
						}
						catch (Exception)
						{
							ProfileV2Model.DeleteTempFiles();
							return false;
						}
						if (cancelToken.IsCancellationRequested)
						{
							ProfileV2Model.DeleteTempFiles();
							return false;
						}
						using (FileStream fstream = File.Create(profiles_temp_file))
						{
							try
							{
								Progress<double> progress2 = new Progress<double>(delegate(double d)
								{
									Math.Round(d, 0);
								});
								await HttpDownloader.DownloadFileAsync(string.Concat(new string[]
								{
									ProfileV2Model.urlBase,
									"/profiles/",
									App.Version,
									"/",
									updateVersion,
									"_p.db"
								}), progress2, cancelToken, fstream, 65536);
								if (fstream.Length != updateSize)
								{
									return false;
								}
							}
							catch (Exception)
							{
								ProfileV2Model.DeleteTempFiles();
								return false;
							}
						}
						fstream = null;
						try
						{
							PackageFileReader.BinaryDeserializeFromEmbeddedFileUsingCompressedCryptoStreamWithDiff<List<OBDReaderProfileV2>>("profiles7.db", true, profiles_temp_file);
						}
						catch (Exception)
						{
							ProfileV2Model.DeleteTempFiles();
							return false;
						}
						using (StreamWriter sw = new StreamWriter(ver_temp_file))
						{
							sw.WriteLine(updateVersion);
							sw.WriteLine(updateSize.ToString());
							await sw.FlushAsync();
						}
						StreamWriter sw = null;
						string text = Path.Combine(FileSystem.CacheDirectory, "upd_brands.db");
						string text2 = Path.Combine(FileSystem.CacheDirectory, "upd_profiles.db");
						string text3 = Path.Combine(FileSystem.CacheDirectory, "profiles_ver.txt");
						if (File.Exists(text))
						{
							File.Delete(text);
						}
						if (File.Exists(text2))
						{
							File.Delete(text2);
						}
						if (File.Exists(text3))
						{
							File.Delete(text3);
						}
						File.Move(brands_temp_file, text);
						File.Move(profiles_temp_file, text2);
						File.Move(ver_temp_file, text3);
						return true;
					}
					catch (Exception)
					{
						ProfileV2Model.DeleteTempFiles();
						return false;
					}
				}
				flag = false;
			}
			return flag;
		}

		// Token: 0x060022B6 RID: 8886 RVA: 0x001ACD54 File Offset: 0x001AAF54
		private static void DeleteTempFiles()
		{
			string text = Path.Combine(FileSystem.CacheDirectory, "temp_brands.db");
			string text2 = Path.Combine(FileSystem.CacheDirectory, "temp_profiles.db");
			string text3 = Path.Combine(FileSystem.CacheDirectory, "temp_ver.txt");
			try
			{
				if (File.Exists(text))
				{
					File.Delete(text);
				}
			}
			catch (Exception)
			{
			}
			try
			{
				if (File.Exists(text2))
				{
					File.Delete(text2);
				}
			}
			catch (Exception)
			{
			}
			try
			{
				if (File.Exists(text3))
				{
					File.Delete(text3);
				}
			}
			catch (Exception)
			{
			}
		}

		// Token: 0x060022B7 RID: 8887 RVA: 0x001ACDF4 File Offset: 0x001AAFF4
		public static void DeleteUpdated()
		{
			try
			{
				string text = Path.Combine(FileSystem.CacheDirectory, "upd_brands.db");
				string text2 = Path.Combine(FileSystem.CacheDirectory, "upd_profiles.db");
				string text3 = Path.Combine(FileSystem.CacheDirectory, "profiles_ver.txt");
				if (File.Exists(text))
				{
					File.Delete(text);
				}
				if (File.Exists(text2))
				{
					File.Delete(text2);
				}
				if (File.Exists(text3))
				{
					File.Delete(text3);
				}
			}
			catch (Exception)
			{
			}
		}

		// Token: 0x060022B8 RID: 8888 RVA: 0x001ACE70 File Offset: 0x001AB070
		public static bool VerifyUpdated()
		{
			bool flag;
			try
			{
				ProfileV2Model profileV2Model = new ProfileV2Model();
				profileV2Model.LoadBrandsUpdated();
				profileV2Model.LoadProfilesUpdated();
				profileV2Model.RawProfiles.Clear();
				profileV2Model.Brands.Clear();
				flag = true;
			}
			catch (Exception)
			{
				ProfileV2Model.DeleteUpdated();
				flag = false;
			}
			return flag;
		}

		// Token: 0x060022B9 RID: 8889 RVA: 0x001ACEC4 File Offset: 0x001AB0C4
		public static async Task DoBackgroundUpdate()
		{
			TimeSpan timeSpan = new TimeSpan(DateTimeNowHelper.NowSafe.Ticks - SharedSettings.Current.LastTimeDBUpdateChecked);
			TimeSpan timeSpan2 = TimeSpan.FromHours(12.0);
			if (timeSpan > timeSpan2)
			{
				ValueTuple<bool, string> valueTuple = await ProfileV2Model.CheckForOnlineUpdatesAvailables();
				bool item = valueTuple.Item1;
				string item2 = valueTuple.Item2;
				if (item && !string.IsNullOrEmpty(item2) && VersionChecker.IsVersionNewer(item2, ProfileV2Model.GetCurrentVersion()))
				{
					TaskAwaiter<bool> taskAwaiter = ProfileV2Model.DownloadUpdate(new Progress<string>(), CancellationToken.None).GetAwaiter();
					if (!taskAwaiter.IsCompleted)
					{
						await taskAwaiter;
						TaskAwaiter<bool> taskAwaiter2;
						taskAwaiter = taskAwaiter2;
						taskAwaiter2 = default(TaskAwaiter<bool>);
					}
					if (taskAwaiter.GetResult())
					{
						SharedSettings.Current.LastTimeDBUpdateChecked = DateTimeNowHelper.NowSafe.Ticks;
						SharedSettings.Current.ForceProfileUpdateScheduled = true;
					}
				}
				else
				{
					SharedSettings.Current.LastTimeDBUpdateChecked = DateTimeNowHelper.NowSafe.Ticks;
				}
			}
		}

		// Token: 0x060022BA RID: 8890 RVA: 0x001ACEFF File Offset: 0x001AB0FF
		// Note: this type is marked as 'beforefieldinit'.
		static ProfileV2Model()
		{
		}

		// Token: 0x060022BB RID: 8891 RVA: 0x001ACF11 File Offset: 0x001AB111
		[CompilerGenerated]
		private void <LoadProfilesAsync>b__21_0()
		{
			this.LoadProfiles(false);
		}

		// Token: 0x040010A9 RID: 4265
		[CompilerGenerated]
		private bool <ProfilesLoaded>k__BackingField;

		// Token: 0x040010AA RID: 4266
		[CompilerGenerated]
		private bool <BrandsLoaded>k__BackingField;

		// Token: 0x040010AB RID: 4267
		private List<OBDReaderProfileV2> RawProfiles = new List<OBDReaderProfileV2>();

		// Token: 0x040010AC RID: 4268
		[CompilerGenerated]
		private ObservableCollection<string> <Brands>k__BackingField;

		// Token: 0x040010AD RID: 4269
		private static string urlBase = "https://node4.carscanner.info";

		// Token: 0x040010AE RID: 4270
		private static bool bestUrlSelected = false;

		// Token: 0x020002CF RID: 719
		[CompilerGenerated]
		[Serializable]
		private sealed class <>c
		{
			// Token: 0x060022BC RID: 8892 RVA: 0x001ACF1A File Offset: 0x001AB11A
			// Note: this type is marked as 'beforefieldinit'.
			static <>c()
			{
			}

			// Token: 0x060022BD RID: 8893 RVA: 0x00002050 File Offset: 0x00000250
			public <>c()
			{
			}

			// Token: 0x060022BE RID: 8894 RVA: 0x001ACF26 File Offset: 0x001AB126
			internal bool <LoadProfiles>b__17_0(OBDReaderProfileV2 x)
			{
				return !x.IsExperimental;
			}

			// Token: 0x060022BF RID: 8895 RVA: 0x001A8F18 File Offset: 0x001A7118
			internal bool <GetProfilesForBrand>b__22_0(OBDReaderProfileV2 x)
			{
				return x.Name == "OBD-II / EOBD";
			}

			// Token: 0x060022C0 RID: 8896 RVA: 0x001ACF31 File Offset: 0x001AB131
			internal bool <GetProfilesForBrand>b__22_1(OBDReaderProfileV2 x)
			{
				return x.Name.Contains("Consult", StringComparison.InvariantCultureIgnoreCase);
			}

			// Token: 0x060022C1 RID: 8897 RVA: 0x001ACF44 File Offset: 0x001AB144
			internal bool <GetProfilesForBrand>b__22_2(OBDReaderProfileV2 x)
			{
				return x.Name.StartsWith("OBD-II / EOBD");
			}

			// Token: 0x060022C2 RID: 8898 RVA: 0x001ACF56 File Offset: 0x001AB156
			internal string <GetProfilesForBrand>b__22_3(OBDReaderProfileV2 x)
			{
				return x.Name;
			}

			// Token: 0x060022C3 RID: 8899 RVA: 0x001A8F18 File Offset: 0x001A7118
			internal bool <GetProfilesForBrand>b__22_4(OBDReaderProfileV2 x)
			{
				return x.Name == "OBD-II / EOBD";
			}

			// Token: 0x060022C4 RID: 8900 RVA: 0x001ACF56 File Offset: 0x001AB156
			internal string <GetProfilesForBrand>b__22_5(OBDReaderProfileV2 x)
			{
				return x.Name;
			}

			// Token: 0x060022C5 RID: 8901 RVA: 0x001A8F18 File Offset: 0x001A7118
			internal bool <GetProfilesForBrand>b__22_6(OBDReaderProfileV2 x)
			{
				return x.Name == "OBD-II / EOBD";
			}

			// Token: 0x060022C6 RID: 8902 RVA: 0x001ACF44 File Offset: 0x001AB144
			internal bool <GetProfilesForBrand>b__22_7(OBDReaderProfileV2 x)
			{
				return x.Name.StartsWith("OBD-II / EOBD");
			}

			// Token: 0x060022C7 RID: 8903 RVA: 0x001ACF56 File Offset: 0x001AB156
			internal string <GetProfilesForBrand>b__22_8(OBDReaderProfileV2 x)
			{
				return x.Name;
			}

			// Token: 0x060022C8 RID: 8904 RVA: 0x001A8F18 File Offset: 0x001A7118
			internal bool <GetProfilesForBrand>b__22_9(OBDReaderProfileV2 x)
			{
				return x.Name == "OBD-II / EOBD";
			}

			// Token: 0x060022C9 RID: 8905 RVA: 0x001ACF5E File Offset: 0x001AB15E
			internal bool <GetProfilesForBrandWithFilter>b__23_0(OBDReaderProfileV2 x)
			{
				return x.UpdateAliases.Any((string alias) => alias == "f133a2bb2f1a44de97371e08c70f9ad9");
			}

			// Token: 0x060022CA RID: 8906 RVA: 0x001ACF8A File Offset: 0x001AB18A
			internal bool <GetProfilesForBrandWithFilter>b__23_1(string alias)
			{
				return alias == "f133a2bb2f1a44de97371e08c70f9ad9";
			}

			// Token: 0x060022CB RID: 8907 RVA: 0x001ACF97 File Offset: 0x001AB197
			internal bool <GetUpdateVersion>b__28_0(char c)
			{
				return c == '.';
			}

			// Token: 0x060022CC RID: 8908 RVA: 0x001ACF9E File Offset: 0x001AB19E
			internal void <DownloadUpdate>b__33_0(double d)
			{
				Math.Round(d, 0);
			}

			// Token: 0x040010AF RID: 4271
			public static readonly ProfileV2Model.<>c <>9 = new ProfileV2Model.<>c();

			// Token: 0x040010B0 RID: 4272
			public static Func<OBDReaderProfileV2, bool> <>9__17_0;

			// Token: 0x040010B1 RID: 4273
			public static Func<OBDReaderProfileV2, bool> <>9__22_0;

			// Token: 0x040010B2 RID: 4274
			public static Func<OBDReaderProfileV2, bool> <>9__22_1;

			// Token: 0x040010B3 RID: 4275
			public static Func<OBDReaderProfileV2, bool> <>9__22_2;

			// Token: 0x040010B4 RID: 4276
			public static Func<OBDReaderProfileV2, string> <>9__22_3;

			// Token: 0x040010B5 RID: 4277
			public static Func<OBDReaderProfileV2, bool> <>9__22_4;

			// Token: 0x040010B6 RID: 4278
			public static Func<OBDReaderProfileV2, string> <>9__22_5;

			// Token: 0x040010B7 RID: 4279
			public static Func<OBDReaderProfileV2, bool> <>9__22_6;

			// Token: 0x040010B8 RID: 4280
			public static Func<OBDReaderProfileV2, bool> <>9__22_7;

			// Token: 0x040010B9 RID: 4281
			public static Func<OBDReaderProfileV2, string> <>9__22_8;

			// Token: 0x040010BA RID: 4282
			public static Func<OBDReaderProfileV2, bool> <>9__22_9;

			// Token: 0x040010BB RID: 4283
			public static Func<string, bool> <>9__23_1;

			// Token: 0x040010BC RID: 4284
			public static Func<OBDReaderProfileV2, bool> <>9__23_0;

			// Token: 0x040010BD RID: 4285
			public static Func<char, bool> <>9__28_0;

			// Token: 0x040010BE RID: 4286
			public static Action<double> <>9__33_0;
		}

		// Token: 0x020002D0 RID: 720
		[CompilerGenerated]
		private sealed class <>c__DisplayClass23_0
		{
			// Token: 0x060022CD RID: 8909 RVA: 0x00002050 File Offset: 0x00000250
			public <>c__DisplayClass23_0()
			{
			}

			// Token: 0x060022CE RID: 8910 RVA: 0x001ACFA8 File Offset: 0x001AB1A8
			internal bool <GetProfilesForBrandWithFilter>b__2(OBDReaderProfileV2 x)
			{
				return (!string.IsNullOrEmpty(x.Name) && x.Name.IndexOf(this.word, StringComparison.OrdinalIgnoreCase) >= 0) || (!string.IsNullOrEmpty(x.Description) && x.Description.IndexOf(this.word, StringComparison.OrdinalIgnoreCase) >= 0);
			}

			// Token: 0x040010BF RID: 4287
			public string word;
		}

		// Token: 0x020002D1 RID: 721
		[CompilerGenerated]
		private sealed class <>c__DisplayClass24_0
		{
			// Token: 0x060022CF RID: 8911 RVA: 0x00002050 File Offset: 0x00000250
			public <>c__DisplayClass24_0()
			{
			}

			// Token: 0x060022D0 RID: 8912 RVA: 0x001AD000 File Offset: 0x001AB200
			internal bool <GetBrandsWithFilter>b__0(string x)
			{
				return !string.IsNullOrEmpty(x) && x.IndexOf(this.word, StringComparison.OrdinalIgnoreCase) >= 0;
			}

			// Token: 0x040010C0 RID: 4288
			public string word;
		}

		// Token: 0x020002D2 RID: 722
		[CompilerGenerated]
		private sealed class <>c__DisplayClass25_0
		{
			// Token: 0x060022D1 RID: 8913 RVA: 0x00002050 File Offset: 0x00000250
			public <>c__DisplayClass25_0()
			{
			}

			// Token: 0x060022D2 RID: 8914 RVA: 0x001AD020 File Offset: 0x001AB220
			internal bool <GetProfileForUpdate>b__0(OBDReaderProfileV2 x)
			{
				IEnumerable<string> updateAliases = x.UpdateAliases;
				Func<string, bool> func;
				if ((func = this.<>9__1) == null)
				{
					func = (this.<>9__1 = (string alias) => alias == this.updateAlias);
				}
				return updateAliases.Any(func);
			}

			// Token: 0x060022D3 RID: 8915 RVA: 0x001AD057 File Offset: 0x001AB257
			internal bool <GetProfileForUpdate>b__1(string alias)
			{
				return alias == this.updateAlias;
			}

			// Token: 0x040010C1 RID: 4289
			public string updateAlias;

			// Token: 0x040010C2 RID: 4290
			public Func<string, bool> <>9__1;
		}

		// Token: 0x020002D3 RID: 723
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <CheckForOnlineUpdatesAvailables>d__32 : IAsyncStateMachine
		{
			// Token: 0x060022D4 RID: 8916 RVA: 0x001AD068 File Offset: 0x001AB268
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				ValueTuple<bool, string> valueTuple;
				try
				{
					if (num == 0 || num != 1)
					{
						try
						{
							ValueTaskAwaiter valueTaskAwaiter;
							if (num != 0)
							{
								valueTaskAwaiter = ProfileV2Model.SelectBestServer().GetAwaiter();
								if (!valueTaskAwaiter.IsCompleted)
								{
									num = (num2 = 0);
									ValueTaskAwaiter valueTaskAwaiter2 = valueTaskAwaiter;
									this.<>t__builder.AwaitUnsafeOnCompleted<ValueTaskAwaiter, ProfileV2Model.<CheckForOnlineUpdatesAvailables>d__32>(ref valueTaskAwaiter, ref this);
									return;
								}
							}
							else
							{
								ValueTaskAwaiter valueTaskAwaiter2;
								valueTaskAwaiter = valueTaskAwaiter2;
								valueTaskAwaiter2 = default(ValueTaskAwaiter);
								num = (num2 = -1);
							}
							valueTaskAwaiter.GetResult();
						}
						catch (Exception)
						{
						}
						ms = new MemoryStream();
					}
					try
					{
						CancellationTokenSource cancellationTokenSource;
						if (num != 1)
						{
							cancellationTokenSource = new CancellationTokenSource(TimeSpan.FromSeconds(15.0));
						}
						try
						{
							TaskAwaiter taskAwaiter;
							if (num != 1)
							{
								taskAwaiter = HttpDownloader.DownloadFileAsync(ProfileV2Model.urlBase + "/profiles/" + App.Version + "/ver.txt", null, cancellationTokenSource.Token, ms, 65536).GetAwaiter();
								if (!taskAwaiter.IsCompleted)
								{
									num = (num2 = 1);
									TaskAwaiter taskAwaiter2 = taskAwaiter;
									this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, ProfileV2Model.<CheckForOnlineUpdatesAvailables>d__32>(ref taskAwaiter, ref this);
									return;
								}
							}
							else
							{
								TaskAwaiter taskAwaiter2;
								taskAwaiter = taskAwaiter2;
								taskAwaiter2 = default(TaskAwaiter);
								num = (num2 = -1);
							}
							taskAwaiter.GetResult();
						}
						catch (Exception)
						{
							valueTuple = new ValueTuple<bool, string>(false, "");
							goto IL_01EB;
						}
						if (ms.Length > 0L)
						{
							ms.Seek(0L, SeekOrigin.Begin);
							StreamReader streamReader = new StreamReader(ms);
							try
							{
								string text = streamReader.ReadLine();
								if (VersionChecker.IsVersionNewer(text, ProfileV2Model.GetCurrentVersion()))
								{
									valueTuple = new ValueTuple<bool, string>(true, text);
									goto IL_01EB;
								}
								valueTuple = new ValueTuple<bool, string>(false, text);
								goto IL_01EB;
							}
							finally
							{
								if (num < 0 && streamReader != null)
								{
									((IDisposable)streamReader).Dispose();
								}
							}
						}
					}
					finally
					{
						if (num < 0 && ms != null)
						{
							((IDisposable)ms).Dispose();
						}
					}
					ms = null;
					valueTuple = new ValueTuple<bool, string>(false, "");
				}
				catch (Exception ex)
				{
					num2 = -2;
					this.<>t__builder.SetException(ex);
					return;
				}
				IL_01EB:
				num2 = -2;
				this.<>t__builder.SetResult(valueTuple);
			}

			// Token: 0x060022D5 RID: 8917 RVA: 0x001AD2F0 File Offset: 0x001AB4F0
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x040010C3 RID: 4291
			public int <>1__state;

			// Token: 0x040010C4 RID: 4292
			public AsyncTaskMethodBuilder<ValueTuple<bool, string>> <>t__builder;

			// Token: 0x040010C5 RID: 4293
			private ValueTaskAwaiter <>u__1;

			// Token: 0x040010C6 RID: 4294
			private MemoryStream <ms>5__2;

			// Token: 0x040010C7 RID: 4295
			private TaskAwaiter <>u__2;
		}

		// Token: 0x020002D4 RID: 724
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <DoBackgroundUpdate>d__37 : IAsyncStateMachine
		{
			// Token: 0x060022D6 RID: 8918 RVA: 0x001AD300 File Offset: 0x001AB500
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				try
				{
					TaskAwaiter<bool> taskAwaiter3;
					TaskAwaiter<ValueTuple<bool, string>> taskAwaiter4;
					if (num != 0)
					{
						if (num == 1)
						{
							taskAwaiter3 = taskAwaiter2;
							taskAwaiter2 = default(TaskAwaiter<bool>);
							num2 = -1;
							goto IL_0136;
						}
						TimeSpan timeSpan = new TimeSpan(DateTimeNowHelper.NowSafe.Ticks - SharedSettings.Current.LastTimeDBUpdateChecked);
						TimeSpan timeSpan2 = TimeSpan.FromHours(12.0);
						if (!(timeSpan > timeSpan2))
						{
							goto IL_017A;
						}
						taskAwaiter4 = ProfileV2Model.CheckForOnlineUpdatesAvailables().GetAwaiter();
						if (!taskAwaiter4.IsCompleted)
						{
							num2 = 0;
							TaskAwaiter<ValueTuple<bool, string>> taskAwaiter5 = taskAwaiter4;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<ValueTuple<bool, string>>, ProfileV2Model.<DoBackgroundUpdate>d__37>(ref taskAwaiter4, ref this);
							return;
						}
					}
					else
					{
						TaskAwaiter<ValueTuple<bool, string>> taskAwaiter5;
						taskAwaiter4 = taskAwaiter5;
						taskAwaiter5 = default(TaskAwaiter<ValueTuple<bool, string>>);
						num2 = -1;
					}
					ValueTuple<bool, string> result = taskAwaiter4.GetResult();
					bool item = result.Item1;
					string item2 = result.Item2;
					if (!item || string.IsNullOrEmpty(item2) || !VersionChecker.IsVersionNewer(item2, ProfileV2Model.GetCurrentVersion()))
					{
						SharedSettings.Current.LastTimeDBUpdateChecked = DateTimeNowHelper.NowSafe.Ticks;
						goto IL_017A;
					}
					taskAwaiter3 = ProfileV2Model.DownloadUpdate(new Progress<string>(), CancellationToken.None).GetAwaiter();
					if (!taskAwaiter3.IsCompleted)
					{
						num2 = 1;
						taskAwaiter2 = taskAwaiter3;
						this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<bool>, ProfileV2Model.<DoBackgroundUpdate>d__37>(ref taskAwaiter3, ref this);
						return;
					}
					IL_0136:
					if (taskAwaiter3.GetResult())
					{
						SharedSettings.Current.LastTimeDBUpdateChecked = DateTimeNowHelper.NowSafe.Ticks;
						SharedSettings.Current.ForceProfileUpdateScheduled = true;
					}
					IL_017A:;
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

			// Token: 0x060022D7 RID: 8919 RVA: 0x001AD4D4 File Offset: 0x001AB6D4
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x040010C8 RID: 4296
			public int <>1__state;

			// Token: 0x040010C9 RID: 4297
			public AsyncTaskMethodBuilder <>t__builder;

			// Token: 0x040010CA RID: 4298
			private TaskAwaiter<ValueTuple<bool, string>> <>u__1;

			// Token: 0x040010CB RID: 4299
			private TaskAwaiter<bool> <>u__2;
		}

		// Token: 0x020002D5 RID: 725
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <DownloadUpdate>d__33 : IAsyncStateMachine
		{
			// Token: 0x060022D8 RID: 8920 RVA: 0x001AD4E4 File Offset: 0x001AB6E4
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				bool flag;
				try
				{
					switch (num)
					{
					default:
						try
						{
							ValueTaskAwaiter valueTaskAwaiter;
							if (num != 0)
							{
								valueTaskAwaiter = ProfileV2Model.SelectBestServer().GetAwaiter();
								if (!valueTaskAwaiter.IsCompleted)
								{
									num = (num2 = 0);
									ValueTaskAwaiter valueTaskAwaiter2 = valueTaskAwaiter;
									this.<>t__builder.AwaitUnsafeOnCompleted<ValueTaskAwaiter, ProfileV2Model.<DownloadUpdate>d__33>(ref valueTaskAwaiter, ref this);
									return;
								}
							}
							else
							{
								ValueTaskAwaiter valueTaskAwaiter2;
								valueTaskAwaiter = valueTaskAwaiter2;
								valueTaskAwaiter2 = default(ValueTaskAwaiter);
								num = (num2 = -1);
							}
							valueTaskAwaiter.GetResult();
						}
						catch (Exception)
						{
						}
						updateVersion = "";
						updateSize = -1L;
						ms = new MemoryStream();
						break;
					case 1:
						break;
					case 2:
					case 3:
					case 4:
						goto IL_0220;
					}
					TaskAwaiter taskAwaiter2;
					try
					{
						CancellationTokenSource cancellationTokenSource;
						if (num != 1)
						{
							cancellationTokenSource = new CancellationTokenSource(TimeSpan.FromSeconds(15.0));
						}
						try
						{
							TaskAwaiter taskAwaiter;
							if (num != 1)
							{
								taskAwaiter = HttpDownloader.DownloadFileAsync(ProfileV2Model.urlBase + "/profiles/" + App.Version + "/ver.txt", null, cancellationTokenSource.Token, ms, 65536).GetAwaiter();
								if (!taskAwaiter.IsCompleted)
								{
									num = (num2 = 1);
									taskAwaiter2 = taskAwaiter;
									this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, ProfileV2Model.<DownloadUpdate>d__33>(ref taskAwaiter, ref this);
									return;
								}
							}
							else
							{
								taskAwaiter = taskAwaiter2;
								taskAwaiter2 = default(TaskAwaiter);
								num = (num2 = -1);
							}
							taskAwaiter.GetResult();
						}
						catch (Exception)
						{
							flag = false;
							goto IL_06CB;
						}
						if (ms.Length > 0L)
						{
							ms.Seek(0L, SeekOrigin.Begin);
							StreamReader streamReader = new StreamReader(ms);
							try
							{
								updateVersion = streamReader.ReadLine();
								long.TryParse(streamReader.ReadLine(), out updateSize);
							}
							finally
							{
								if (num < 0 && streamReader != null)
								{
									((IDisposable)streamReader).Dispose();
								}
							}
						}
					}
					finally
					{
						if (num < 0 && ms != null)
						{
							((IDisposable)ms).Dispose();
						}
					}
					ms = null;
					if (cancelToken.IsCancellationRequested)
					{
						flag = false;
						goto IL_06CB;
					}
					if (!(updateVersion != "") || !VersionChecker.IsVersionNewer(updateVersion, ProfileV2Model.GetCurrentVersion()) || updateSize <= 0L)
					{
						goto IL_06A7;
					}
					IL_0220:
					try
					{
						switch (num)
						{
						case 2:
							break;
						case 3:
							goto IL_0404;
						case 4:
							goto IL_055F;
						default:
							brands_temp_file = Path.Combine(FileSystem.CacheDirectory, "temp_brands.db");
							profiles_temp_file = Path.Combine(FileSystem.CacheDirectory, "temp_profiles.db");
							ver_temp_file = Path.Combine(FileSystem.CacheDirectory, "temp_ver.txt");
							ProfileV2Model.DeleteTempFiles();
							fstream = File.Create(brands_temp_file);
							break;
						}
						try
						{
							TaskAwaiter taskAwaiter;
							if (num != 2)
							{
								IProgress<string> progress = progress;
								if (progress != null)
								{
									progress.Report("Downloading brands");
								}
								CancellationTokenSource cancellationTokenSource2 = new CancellationTokenSource();
								taskAwaiter = HttpDownloader.DownloadFileAsync(string.Concat(new string[]
								{
									ProfileV2Model.urlBase,
									"/profiles/",
									App.Version,
									"/",
									updateVersion,
									"_b.db"
								}), null, cancellationTokenSource2.Token, fstream, 65536).GetAwaiter();
								if (!taskAwaiter.IsCompleted)
								{
									num = (num2 = 2);
									taskAwaiter2 = taskAwaiter;
									this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, ProfileV2Model.<DownloadUpdate>d__33>(ref taskAwaiter, ref this);
									return;
								}
							}
							else
							{
								taskAwaiter = taskAwaiter2;
								taskAwaiter2 = default(TaskAwaiter);
								num = (num2 = -1);
							}
							taskAwaiter.GetResult();
							if (fstream.Length == 0L)
							{
								flag = false;
								goto IL_06CB;
							}
						}
						catch (Exception)
						{
							flag = false;
							goto IL_06CB;
						}
						finally
						{
							if (num < 0 && fstream != null)
							{
								((IDisposable)fstream).Dispose();
							}
						}
						fstream = null;
						try
						{
							StreamReader streamReader2 = new StreamReader(brands_temp_file);
							try
							{
								JsonConvert.DeserializeObject<List<string>>(streamReader2.ReadToEnd());
							}
							finally
							{
								if (num < 0 && streamReader2 != null)
								{
									((IDisposable)streamReader2).Dispose();
								}
							}
						}
						catch (Exception)
						{
							ProfileV2Model.DeleteTempFiles();
							flag = false;
							goto IL_06CB;
						}
						if (cancelToken.IsCancellationRequested)
						{
							ProfileV2Model.DeleteTempFiles();
							flag = false;
							goto IL_06CB;
						}
						fstream = File.Create(profiles_temp_file);
						IL_0404:
						try
						{
							TaskAwaiter taskAwaiter;
							if (num != 3)
							{
								Progress<double> progress2 = new Progress<double>(delegate(double d)
								{
									Math.Round(d, 0);
								});
								taskAwaiter = HttpDownloader.DownloadFileAsync(string.Concat(new string[]
								{
									ProfileV2Model.urlBase,
									"/profiles/",
									App.Version,
									"/",
									updateVersion,
									"_p.db"
								}), progress2, cancelToken, fstream, 65536).GetAwaiter();
								if (!taskAwaiter.IsCompleted)
								{
									num = (num2 = 3);
									taskAwaiter2 = taskAwaiter;
									this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, ProfileV2Model.<DownloadUpdate>d__33>(ref taskAwaiter, ref this);
									return;
								}
							}
							else
							{
								taskAwaiter = taskAwaiter2;
								taskAwaiter2 = default(TaskAwaiter);
								num = (num2 = -1);
							}
							taskAwaiter.GetResult();
							if (fstream.Length != updateSize)
							{
								flag = false;
								goto IL_06CB;
							}
						}
						catch (Exception)
						{
							ProfileV2Model.DeleteTempFiles();
							flag = false;
							goto IL_06CB;
						}
						finally
						{
							if (num < 0 && fstream != null)
							{
								((IDisposable)fstream).Dispose();
							}
						}
						fstream = null;
						try
						{
							PackageFileReader.BinaryDeserializeFromEmbeddedFileUsingCompressedCryptoStreamWithDiff<List<OBDReaderProfileV2>>("profiles7.db", true, profiles_temp_file);
						}
						catch (Exception)
						{
							ProfileV2Model.DeleteTempFiles();
							flag = false;
							goto IL_06CB;
						}
						sw = new StreamWriter(ver_temp_file);
						IL_055F:
						try
						{
							TaskAwaiter taskAwaiter;
							if (num != 4)
							{
								sw.WriteLine(updateVersion);
								sw.WriteLine(updateSize.ToString());
								taskAwaiter = sw.FlushAsync().GetAwaiter();
								if (!taskAwaiter.IsCompleted)
								{
									num = (num2 = 4);
									taskAwaiter2 = taskAwaiter;
									this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, ProfileV2Model.<DownloadUpdate>d__33>(ref taskAwaiter, ref this);
									return;
								}
							}
							else
							{
								taskAwaiter = taskAwaiter2;
								taskAwaiter2 = default(TaskAwaiter);
								num = (num2 = -1);
							}
							taskAwaiter.GetResult();
						}
						finally
						{
							if (num < 0 && sw != null)
							{
								((IDisposable)sw).Dispose();
							}
						}
						sw = null;
						string text = Path.Combine(FileSystem.CacheDirectory, "upd_brands.db");
						string text2 = Path.Combine(FileSystem.CacheDirectory, "upd_profiles.db");
						string text3 = Path.Combine(FileSystem.CacheDirectory, "profiles_ver.txt");
						if (File.Exists(text))
						{
							File.Delete(text);
						}
						if (File.Exists(text2))
						{
							File.Delete(text2);
						}
						if (File.Exists(text3))
						{
							File.Delete(text3);
						}
						File.Move(brands_temp_file, text);
						File.Move(profiles_temp_file, text2);
						File.Move(ver_temp_file, text3);
						flag = true;
						goto IL_06CB;
					}
					catch (Exception)
					{
						ProfileV2Model.DeleteTempFiles();
						flag = false;
						goto IL_06CB;
					}
					IL_06A7:
					flag = false;
				}
				catch (Exception ex)
				{
					num2 = -2;
					updateVersion = null;
					this.<>t__builder.SetException(ex);
					return;
				}
				IL_06CB:
				num2 = -2;
				updateVersion = null;
				this.<>t__builder.SetResult(flag);
			}

			// Token: 0x060022D9 RID: 8921 RVA: 0x001ADD2C File Offset: 0x001ABF2C
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x040010CC RID: 4300
			public int <>1__state;

			// Token: 0x040010CD RID: 4301
			public AsyncTaskMethodBuilder<bool> <>t__builder;

			// Token: 0x040010CE RID: 4302
			public CancellationToken cancelToken;

			// Token: 0x040010CF RID: 4303
			public IProgress<string> progress;

			// Token: 0x040010D0 RID: 4304
			private string <updateVersion>5__2;

			// Token: 0x040010D1 RID: 4305
			private long <updateSize>5__3;

			// Token: 0x040010D2 RID: 4306
			private ValueTaskAwaiter <>u__1;

			// Token: 0x040010D3 RID: 4307
			private MemoryStream <ms>5__4;

			// Token: 0x040010D4 RID: 4308
			private TaskAwaiter <>u__2;

			// Token: 0x040010D5 RID: 4309
			private string <brands_temp_file>5__5;

			// Token: 0x040010D6 RID: 4310
			private string <profiles_temp_file>5__6;

			// Token: 0x040010D7 RID: 4311
			private string <ver_temp_file>5__7;

			// Token: 0x040010D8 RID: 4312
			private FileStream <fstream>5__8;

			// Token: 0x040010D9 RID: 4313
			private StreamWriter <sw>5__9;
		}

		// Token: 0x020002D6 RID: 726
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <LoadProfilesAsync>d__21 : IAsyncStateMachine
		{
			// Token: 0x060022DA RID: 8922 RVA: 0x001ADD3C File Offset: 0x001ABF3C
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				ProfileV2Model profileV2Model = this;
				try
				{
					TaskAwaiter taskAwaiter;
					if (num != 0)
					{
						profileV2Model.RawProfiles.Clear();
						taskAwaiter = Task.Run(delegate
						{
							base.LoadProfiles(false);
						}).GetAwaiter();
						if (!taskAwaiter.IsCompleted)
						{
							num2 = 0;
							TaskAwaiter taskAwaiter2 = taskAwaiter;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter, ProfileV2Model.<LoadProfilesAsync>d__21>(ref taskAwaiter, ref this);
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
					profileV2Model.ProfilesLoaded = true;
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

			// Token: 0x060022DB RID: 8923 RVA: 0x001ADE0C File Offset: 0x001AC00C
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x040010DA RID: 4314
			public int <>1__state;

			// Token: 0x040010DB RID: 4315
			public AsyncTaskMethodBuilder <>t__builder;

			// Token: 0x040010DC RID: 4316
			public ProfileV2Model <>4__this;

			// Token: 0x040010DD RID: 4317
			private TaskAwaiter <>u__1;
		}

		// Token: 0x020002D7 RID: 727
		[CompilerGenerated]
		[StructLayout(LayoutKind.Auto)]
		private struct <SelectBestServer>d__29 : IAsyncStateMachine
		{
			// Token: 0x060022DC RID: 8924 RVA: 0x001ADE1C File Offset: 0x001AC01C
			void IAsyncStateMachine.MoveNext()
			{
				int num2;
				int num = num2;
				try
				{
					TaskAwaiter<Task> taskAwaiter;
					if (num != 0)
					{
						if (ProfileV2Model.bestUrlSelected)
						{
							goto IL_0173;
						}
						t = HttpDownloader.Get("https://carscanner.am/profiles/ping.txt", 10);
						t2 = HttpDownloader.Get("https://node4.carscanner.info/profiles/ping.txt", 10);
						t_delay = Task.Delay(TimeSpan.FromSeconds(15.0));
						taskAwaiter = Task.WhenAny(new Task[] { t, t2, t_delay }).GetAwaiter();
						if (!taskAwaiter.IsCompleted)
						{
							num2 = 0;
							TaskAwaiter<Task> taskAwaiter2 = taskAwaiter;
							this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<Task>, ProfileV2Model.<SelectBestServer>d__29>(ref taskAwaiter, ref this);
							return;
						}
					}
					else
					{
						TaskAwaiter<Task> taskAwaiter2;
						taskAwaiter = taskAwaiter2;
						taskAwaiter2 = default(TaskAwaiter<Task>);
						num2 = -1;
					}
					Task result = taskAwaiter.GetResult();
					if (result == t_delay)
					{
						ProfileV2Model.bestUrlSelected = false;
					}
					else if (result == t && t.Result == "ping-pong")
					{
						ProfileV2Model.urlBase = "https://carscanner.am";
						ProfileV2Model.bestUrlSelected = true;
					}
					else if (result == t2 && t2.Result == "ping-pong")
					{
						ProfileV2Model.urlBase = "https://node4.carscanner.info";
						ProfileV2Model.bestUrlSelected = true;
					}
				}
				catch (Exception ex)
				{
					num2 = -2;
					t = null;
					t2 = null;
					t_delay = null;
					this.<>t__builder.SetException(ex);
					return;
				}
				IL_0173:
				num2 = -2;
				t = null;
				t2 = null;
				t_delay = null;
				this.<>t__builder.SetResult();
			}

			// Token: 0x060022DD RID: 8925 RVA: 0x001ADFE0 File Offset: 0x001AC1E0
			[DebuggerHidden]
			void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
			{
				this.<>t__builder.SetStateMachine(stateMachine);
			}

			// Token: 0x040010DE RID: 4318
			public int <>1__state;

			// Token: 0x040010DF RID: 4319
			public AsyncValueTaskMethodBuilder <>t__builder;

			// Token: 0x040010E0 RID: 4320
			private Task<string> <t1>5__2;

			// Token: 0x040010E1 RID: 4321
			private Task<string> <t2>5__3;

			// Token: 0x040010E2 RID: 4322
			private Task <t_delay>5__4;

			// Token: 0x040010E3 RID: 4323
			private TaskAwaiter<Task> <>u__1;
		}
	}
}
