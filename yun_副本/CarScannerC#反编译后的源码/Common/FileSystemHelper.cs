using System;
using System.IO;
using CarScannerXamarinForms.PlatformAdapters;
using Xamarin.Essentials;

namespace CarScannerXamarinForms.Common
{
	// Token: 0x020007D0 RID: 2000
	public static class FileSystemHelper
	{
		// Token: 0x17001617 RID: 5655
		// (get) Token: 0x060046C1 RID: 18113 RVA: 0x0036B606 File Offset: 0x00369806
		public static string LocalStoragePath
		{
			get
			{
				return Environment.GetFolderPath(Environment.SpecialFolder.Personal);
			}
		}

		// Token: 0x17001618 RID: 5656
		// (get) Token: 0x060046C2 RID: 18114 RVA: 0x0036B60E File Offset: 0x0036980E
		public static string LocalCachePath
		{
			get
			{
				return FileSystem.CacheDirectory;
			}
		}

		// Token: 0x060046C3 RID: 18115 RVA: 0x0036B615 File Offset: 0x00369815
		public static bool LocalFileExists(string filename)
		{
			return File.Exists(FileSystemHelper.GetLocalFilePath(filename));
		}

		// Token: 0x060046C4 RID: 18116 RVA: 0x0036B622 File Offset: 0x00369822
		public static string GetCacheFilePath(string filename)
		{
			return Path.Combine(FileSystemHelper.LocalCachePath, filename);
		}

		// Token: 0x060046C5 RID: 18117 RVA: 0x0036B630 File Offset: 0x00369830
		public static string GetExternalOrLocalFilePath(string filename)
		{
			string text = FileSystemHelper.ExternalStoragePath;
			if (text == null)
			{
				text = FileSystemHelper.LocalStoragePath;
			}
			return Path.Combine(text, filename);
		}

		// Token: 0x060046C6 RID: 18118 RVA: 0x0036B654 File Offset: 0x00369854
		public static string GetLocalFilePath(string filename)
		{
			if (filename == null)
			{
				return FileSystemHelper.LocalStoragePath;
			}
			string extension = Path.GetExtension(filename);
			if (".brc".Equals(extension, StringComparison.OrdinalIgnoreCase))
			{
				try
				{
					string externalStoragePath = FileSystemHelper.ExternalStoragePath;
					if (externalStoragePath != null)
					{
						string text = Path.Combine(externalStoragePath, "records");
						if (!Directory.Exists(text))
						{
							try
							{
								Directory.CreateDirectory(text);
							}
							catch (Exception)
							{
								text = FileSystemHelper.LocalStoragePath;
							}
						}
						return Path.Combine(text, filename);
					}
					goto IL_00A4;
				}
				catch (Exception)
				{
					goto IL_00A4;
				}
			}
			if (".cbz".Equals(extension, StringComparison.OrdinalIgnoreCase))
			{
				try
				{
					string externalStoragePath2 = FileSystemHelper.ExternalStoragePath;
					if (externalStoragePath2 != null)
					{
						string text2 = Path.Combine(externalStoragePath2, "backup");
						if (!Directory.Exists(text2))
						{
							Directory.CreateDirectory(text2);
						}
						return Path.Combine(text2, filename);
					}
				}
				catch (Exception)
				{
				}
			}
			IL_00A4:
			return Path.Combine(FileSystemHelper.LocalStoragePath, filename);
		}

		// Token: 0x060046C7 RID: 18119 RVA: 0x0036B73C File Offset: 0x0036993C
		public static string[] GetLocalFiles()
		{
			return Directory.GetFiles(FileSystemHelper.LocalStoragePath);
		}

		// Token: 0x060046C8 RID: 18120 RVA: 0x0036B748 File Offset: 0x00369948
		public static string[] GetLocalFolders()
		{
			return Directory.GetDirectories(FileSystemHelper.LocalStoragePath);
		}

		// Token: 0x17001619 RID: 5657
		// (get) Token: 0x060046C9 RID: 18121 RVA: 0x0036B754 File Offset: 0x00369954
		public static string ExternalStoragePath
		{
			get
			{
				if (PlatformHelper.IsAndroid)
				{
					return PlatformHelper.DroidService.GetExternalStoragePath;
				}
				return null;
			}
		}
	}
}
